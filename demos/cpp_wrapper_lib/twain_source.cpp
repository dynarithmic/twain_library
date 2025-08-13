/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2025 Dynarithmic Software.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS.
*/
#include <dynarithmic/twain/twain_source.hpp>
#include <dynarithmic/twain/options/options_base.hpp>
#include <dynarithmic/twain/acquire_characteristics/acquire_characteristics.hpp>
#include <dynarithmic/twain/twain_session.hpp>
#include <dynarithmic/twain/info/paperhandling_info.hpp>
#include <dynarithmic/twain/types/twain_timer.hpp>
#include <dynarithmic/twain/source/twain_source_pimpl.hpp>
#include <chrono>
#include <thread>

namespace tb_namespace = dynarithmic::twain::tribool;
namespace dynarithmic
{
	namespace twain
	{
        twain_source::twain_source(const source_select_info& select_info) :
            m_bIsSelected(false),
            m_sourceInfo{},
            m_pSession{},
            m_bCloseable{},
            m_theSource{},
            m_bUIOnlyOn{},
            m_bUIOnlySupported{tb_namespace::make_tribool(0)},
            m_bWeakAttach{},
            m_pTwainSourceImpl{}
        {
            create_interfaces();
            attach(select_info);
        }

        twain_source& twain_source::operator=(const source_select_info& select_info)
        {
            if (select_info.source_handle != m_theSource)
            {
                detach();
                m_bIsSelected = false;
                m_sourceInfo = {};
                m_pSession = {};
                m_bCloseable = {};
                m_theSource = {};
                m_bUIOnlyOn = {};
                m_bUIOnlySupported = {tb_namespace::tribool::indeterminate};
                m_bWeakAttach = {};
                m_pTwainSourceImpl = {};
                m_theSource = select_info.source_handle;
                m_pSession = select_info.session_handle;
                m_pSession->add_source(this);
                create_interfaces();
                attach(select_info);
            }
            return *this;
        }

        twain_source& twain_source::operator=(twain_source&& rhs) noexcept
        {
            if (rhs.m_theSource != m_theSource)
            {
                detach();
                m_bIsSelected = rhs.m_bIsSelected;
                m_bCloseable = rhs.m_bCloseable;
                m_bUIOnlyOn = rhs.m_bUIOnlyOn;
                m_bUIOnlySupported = rhs.m_bUIOnlySupported;
                m_bWeakAttach = rhs.m_bWeakAttach;
                m_pTwainSourceImpl = rhs.m_pTwainSourceImpl;
                m_theSource = rhs.m_theSource;
                m_pSession = rhs.m_pSession;
                m_sourceInfo = rhs.m_sourceInfo;
                rhs.m_theSource = nullptr;
                rhs.m_pSession = nullptr;
            }
            return *this;
        }

        twain_source::~twain_source() noexcept
        {
            try
            {
                if (!m_bWeakAttach)
                    close();
                else
                    detach();
            }
            catch (...) {}
        }

        void twain_source::swap(twain_source& left, twain_source& right) noexcept
        {
            std::swap(left.m_bIsSelected, right.m_bIsSelected);
            std::swap(left.m_sourceInfo, right.m_sourceInfo);
            std::swap(left.m_pSession, right.m_pSession);
            std::swap(left.m_bCloseable, right.m_bCloseable);
            std::swap(left.m_bWeakAttach, right.m_bWeakAttach);
            std::swap(left.m_source_details, right.m_source_details);
            std::swap(left.m_theSource, right.m_theSource);
            std::swap(left.m_bUIOnlyOn, right.m_bUIOnlyOn);
			std::swap(left.m_bUIOnlySupported, right.m_bUIOnlySupported);
            std::swap(left.m_pTwainSourceImpl, right.m_pTwainSourceImpl);
            std::swap(left.m_extImageInfo, right.m_extImageInfo);
        }

        void twain_source::create_interfaces()
        {
            m_pTwainSourceImpl = std::make_shared<twain_source_pimpl>();
            m_pTwainSourceImpl->m_acquire_characteristics = std::make_unique<acquire_characteristics>();
            m_pTwainSourceImpl->m_buffered_info = std::make_unique<buffered_transfer_info>();
            m_pTwainSourceImpl->m_filetransfer_info = std::make_unique<file_transfer_info>();
            m_pTwainSourceImpl->m_capability_listener = std::make_unique<capability_listener>();
            m_pTwainSourceImpl->m_capability_info = std::make_unique<capability_interface>();
        }

        void twain_source::get_source_info_internal()
        {
            const auto p_id = static_cast<TW_IDENTITY*>(API_INSTANCE DTWAIN_GetSourceID(m_theSource));
            m_sourceInfo = *p_id;
        }

        const TW_IDENTITY* twain_source::get_twain_id(bool bRefresh/* = true*/)
        {
            if (bRefresh)
                get_source_info_internal();
            return &m_sourceInfo.get_identity();
        }

        void twain_source::attach(twain_session& twSession, DTWAIN_SOURCE source)
        {
            m_pSession = &twSession;
            attach(source);
        }

        void twain_source::attach(DTWAIN_SOURCE source)
        {
            m_theSource = source;
            if (source)
            {
                get_source_info_internal();
                m_pTwainSourceImpl->m_capability_info->attach(source);
                m_pTwainSourceImpl->m_buffered_info->attach(*this);
                m_bIsSelected = true;
                m_source_details.clear();
                m_pSession->update_source_status(*this);
                if (m_pSession->get_source_status(*this) == twain_session::source_status::opened)
                {
                    auto uiOnlySupported = API_INSTANCE DTWAIN_IsUIOnlySupported(m_theSource);
                    m_bUIOnlySupported = uiOnlySupported ? tribool::make_tribool(1) : tribool::make_tribool(-1);
                    auto vXferMechs = get_capability_interface().get_image_xfermech();
                    m_vAllXferMechs.clear();
                    for (auto value : vXferMechs)
                        m_vAllXferMechs.push_back(static_cast<xfermech_value::value_type>(value));
                }
            }
            else
                m_bIsSelected = false;
        }

        void twain_source::attach(const source_select_info& select_return)
        {
            m_theSource = select_return.source_handle;
            if (m_theSource)
            {
                m_pSession = select_return.session_handle;
                attach(m_theSource);
            }
        }

        twain_source& twain_source::make_weak(bool isWeak)
        {
            m_bWeakAttach = isWeak;
            return *this;
        }

        void twain_source::detach()
        {
            m_theSource = nullptr;
            create_interfaces();
        }

        bool twain_source::open()
        {
            if (m_theSource)
            {
                if (API_INSTANCE DTWAIN_OpenSource(m_theSource))
                {
                    get_source_info_internal();
                    attach(m_theSource);
                    return true;
                }
            }
            return false;
        }

        bool twain_source::close()
        {
            bool retVal = true;
            if (m_theSource && m_pSession)
            {
                if (API_INSTANCE DTWAIN_IsSourceValid( m_theSource ))
                    retVal = API_INSTANCE DTWAIN_CloseSource(m_theSource) ? true : false;
                m_pSession->remove_source(this);
                m_bIsSelected = false;
                m_pSession->update_source_status(*this);
                m_theSource = nullptr;
                m_pSession = nullptr;
                return retVal;
            }
            return false;
        }

        void twain_source::start_apply()
        {
            auto& ac = get_acquire_characteristics();
            auto allAppliers = ac.get_appliers();
            if (allAppliers[acquire_characteristics::apply_languageoptions])
                options_base::apply(*this, ac.get_language_options());

            if (allAppliers[acquire_characteristics::apply_deviceparams])
                options_base::apply(*this, ac.get_deviceparams_options());

            if (allAppliers[acquire_characteristics::apply_powermonitor])
                options_base::apply(*this, ac.get_powermonitor_options());

            if (allAppliers[acquire_characteristics::apply_doublefeedoptions])
                options_base::apply(*this, ac.get_doublefeed_options());

            if (allAppliers[acquire_characteristics::apply_autoadjust])
                options_base::apply(*this, ac.get_autoadjust_options());

            if (allAppliers[acquire_characteristics::apply_autoscanning])
                options_base::apply(*this, ac.get_autoscanning_options());

            if (allAppliers[acquire_characteristics::apply_barcodedetection])
                options_base::apply(*this, ac.get_barcodedetection_options());

            if (allAppliers[acquire_characteristics::apply_patchcodeoptions])
                options_base::apply(*this, ac.get_patchcode_options());

            if (allAppliers[acquire_characteristics::apply_autocapture])
                options_base::apply(*this, ac.get_autocapture_options());

            if (allAppliers[acquire_characteristics::apply_imagetypeoptions])
                options_base::apply(*this, ac.get_imagetype_options());

            if (allAppliers[acquire_characteristics::apply_imageinformation])
                options_base::apply(*this, ac.get_imageinformation_options());

            if (allAppliers[acquire_characteristics::apply_userinterfaceoptions])
                options_base::apply(*this, ac.get_userinterface_options());

            if (allAppliers[acquire_characteristics::apply_imageparameter])
                options_base::apply(*this, ac.get_imageparameter_options());

            if (allAppliers[acquire_characteristics::apply_audiblealarms])
                options_base::apply(*this, ac.get_audiblealarms_options());

            if (allAppliers[acquire_characteristics::apply_deviceevents])
                options_base::apply(*this, ac.get_deviceevent_options());

            if (allAppliers[acquire_characteristics::apply_resolutionoptions])
                options_base::apply(*this, ac.get_resolution_options());

            if (allAppliers[acquire_characteristics::apply_paperhandlingoptions])
                options_base::apply(*this, ac.get_paperhandling_options());

            if (allAppliers[acquire_characteristics::apply_coloroptions])
                options_base::apply(*this, ac.get_color_options());

            if (allAppliers[acquire_characteristics::apply_capnegotiation])
                options_base::apply(*this, ac.get_capnegotiation_options());

            if (allAppliers[acquire_characteristics::apply_microptions])
                options_base::apply(*this, ac.get_micr_options());

            if (allAppliers[acquire_characteristics::apply_pages])
                options_base::apply(*this, ac.get_pages_options());

            if (allAppliers[acquire_characteristics::apply_imprinter])
                options_base::apply(*this, ac.get_imprinter_options());
        }
            
        void twain_source::prepare_acquisition()
        {
            acquire_characteristics& ac = *(m_pTwainSourceImpl->m_acquire_characteristics);
            start_apply();

            // set the acquisition area
            auto twframe = ac.get_pages_options().get_frame();

            // if user has overridden the default...
            if (twframe != twain_frame<>())
            {
                DTWAIN_ARRAY area = API_INSTANCE DTWAIN_ArrayCreate(DTWAIN_ARRAYFLOAT, 4);
                twain_array arr(area);
                double* buffer = arr.get_buffer<double>();
                buffer[0] = twframe.left;
                buffer[1] = twframe.top;
                buffer[2] = twframe.right;
                buffer[3] = twframe.bottom;
                API_INSTANCE DTWAIN_SetAcquireArea(m_theSource, DTWAIN_AREASET, area, NULL);
            }
            else
                API_INSTANCE DTWAIN_SetAcquireArea(m_theSource, DTWAIN_AREARESET, NULL, NULL);

            // Set the job control option
            API_INSTANCE DTWAIN_SetJobControl(m_theSource, static_cast<LONG>(ac.get_jobcontrol_options().get_option()), TRUE);

            // Disable the manual duplex mode
            API_INSTANCE DTWAIN_SetManualDuplexMode(m_theSource, 0, FALSE);

            // Get the duplex mode
            auto dupmode = ac.get_paperhandling_options().get_manualduplexmode();

            switch (dupmode)
            {
                // Duplex on or off was chosen
            case manualduplexmode_value::none:
            {}
            break;

            // manual duplex mode chosen, so turn this on
            default:

                // turn off device's duplex mode, if available
                get_capability_interface().set_cap_values< CAP_DUPLEXENABLED_>({ false });

                // turn on manual duplex mode
                API_INSTANCE DTWAIN_SetManualDuplexMode(m_theSource, static_cast<LONG>(dupmode), TRUE);
                break;
            }

            API_INSTANCE DTWAIN_SetAcquireImageNegative(m_theSource, ac.get_imagetype_options().is_negate_enabled() ? TRUE : FALSE);
            auto& blank_handler = ac.get_blank_page_options();
            API_INSTANCE DTWAIN_SetBlankPageDetectionEx(m_theSource, blank_handler.get_threshold(),
                                                        static_cast<LONG>(blank_handler.get_discard_option()),
                                                        static_cast<LONG>(blank_handler.get_detection_option()),
                                                        static_cast<LONG>(blank_handler.is_enabled()));
            auto& multisave_info = ac.get_file_transfer_options().get_multipage_save_options();
            API_INSTANCE DTWAIN_SetMultipageScanMode(m_theSource,
                static_cast<LONG>(multisave_info.get_save_mode())
                |
                (multisave_info.is_save_incomplete() ? static_cast<LONG>(multipage_save_mode::save_incomplete) : 0));

            // Get the general options
            general_options& gOpts = ac.get_general_options();
            API_INSTANCE DTWAIN_SetMaxAcquisitions(m_theSource, gOpts.get_max_acquisitions());

            // Set the JPEG quality in case we acquire to JPEG files
            imagetype_options& iOpts = ac.get_imagetype_options();
            API_INSTANCE DTWAIN_SetJpegValues(m_theSource, iOpts.get_jpegquality(), false);

            // If non-TWAIN scaling is enabled, enable it now
            auto& imageOptions = ac.get_imageparameter_options();
            if (imageOptions.is_force_scaling_enabled())
                API_INSTANCE DTWAIN_SetAcquireImageScale(m_theSource, imageOptions.get_xscaling(), imageOptions.get_yscaling());
            set_pdf_options();
        }

        void twain_source::set_pdf_options()
        {
            auto source = get_source();

            // set the PDF file properties
            pdf_options& po = m_pTwainSourceImpl->m_acquire_characteristics->get_pdf_options();
            API_INSTANCE DTWAIN_SetPDFCreatorA(source, po.get_creator().c_str());
            API_INSTANCE DTWAIN_SetPDFTitleA(source, po.get_title().c_str());
            API_INSTANCE DTWAIN_SetPDFProducerA(source, po.get_creator().c_str());
            API_INSTANCE DTWAIN_SetPDFAuthorA(source, po.get_author().c_str());
            API_INSTANCE DTWAIN_SetPDFSubjectA(source, po.get_subject().c_str());
            API_INSTANCE DTWAIN_SetPDFKeywordsA(source, po.get_keywords().c_str());
            API_INSTANCE DTWAIN_SetPDFASCIICompression(source, po.is_use_ASCII());
            API_INSTANCE DTWAIN_SetPDFOrientation(source, static_cast<LONG>(po.get_orientation()));

            // Set PDF page size
            auto& pagesizeopts = po.get_page_size_options();
            bool custom_used = pagesizeopts.is_custom_size_used();
            double width = 0.0;
            double height = 0.0;
            if (custom_used)
            {
                auto pr = pagesizeopts.get_custom_size();
                width = pr.first;
                height = pr.second;
            }
            API_INSTANCE DTWAIN_SetPDFPageSize(source, static_cast<LONG>(po.get_page_size_options().get_page_size()), width, height);

            // Set PDF page scale
            auto& pagescaleopts = po.get_page_scale_options();
            custom_used = pagescaleopts.is_custom_scale_used();
            double xscale = 0.0;
            double yscale = 0.0;
            if (custom_used)
            {
                auto pr = pagescaleopts.get_custom_scale();
                xscale = pr.first;
                yscale = pr.second;
            }
            API_INSTANCE DTWAIN_SetPDFPageScale(source, static_cast<LONG>(po.get_page_scale_options().get_page_scale()), xscale, yscale);

            // Set encryption options
            auto& encrypt_opts = po.get_encryption_options();
            if (encrypt_opts.is_use_encryption())
            {
                API_INSTANCE DTWAIN_SetPDFEncryptionA(source, 1, encrypt_opts.get_user_password().c_str(),
                    encrypt_opts.get_owner_password().c_str(),
                    encrypt_opts.get_permissions_int(),
                    encrypt_opts.is_use_strong_encryption());
                API_INSTANCE DTWAIN_SetPDFAESEncryption(source, encrypt_opts.is_use_AES_encryption());
            }
        }

        file_transfer_info twain_source::get_file_transfer_info()
        {
            file_transfer_info finfo;
            auto& ci = get_capability_interface();
            auto vFormat = ci.get_cap_values(ICAP_IMAGEFILEFORMAT);
            std::copy(vFormat.begin(), vFormat.end(), std::inserter(finfo.all_file_types, finfo.all_file_types.end()));
            return finfo;
        }

        twain_source::acquire_return_type twain_source::acquire()
        {
            if (!m_pSession)
            {
                API_INSTANCE DTWAIN_SetLastError(DTWAIN_ERR_BAD_SOURCE);
                return { DTWAIN_ERR_BAD_SOURCE, {} };
            }
            bool fstatus = true;
            prepare_acquisition();
            if (!m_pTwainSourceImpl->m_acquire_characteristics->get_paperhandling_options().is_feeder_enabled())
                API_INSTANCE DTWAIN_EnableFeeder(m_theSource, FALSE);
            else
            {
                auto& feedOptions = m_pTwainSourceImpl->m_acquire_characteristics->get_paperhandling_options();
                auto fmode = feedOptions.get_feedermode();
                bool use_feeder_or_flatbed = (fmode == feedermode_value::feeder_flatbed);
                bool use_wait = (feedOptions.get_feederwait() != 0);
                if (use_wait || use_feeder_or_flatbed)
                {
                    fstatus = false;
                    wait_for_feeder(fstatus);

                    // timed out waiting for the feeder to be loaded, or device doesn't support feeder
                    if (!fstatus && !use_feeder_or_flatbed)
                        return acquire_return_type{ acquire_timeout, {} };
                }

                // if we got a timeout on the feeder, but use flatbed as backup, disable the feeder and use the flatbed
                if (!fstatus && use_feeder_or_flatbed)
                {
                    API_INSTANCE DTWAIN_EnableFeeder(m_theSource, FALSE);
                    fstatus = true;
                }
            }

            if (fstatus)
            {
                if (twain_session::callback_proc(twain_callback_values::DTWAIN_PREACQUIRE_START, 0, reinterpret_cast<UINT_PTR>(m_pSession)))
                {
                    const auto transtype = m_pTwainSourceImpl->m_acquire_characteristics->get_general_options().get_transfer_type();
                    if (transtype == transfer_type::file_using_native ||
                        transtype == transfer_type::file_using_buffered ||
                        transtype == transfer_type::file_using_source)
                        return acquire_to_file(transtype);
                    return acquire_to_image_handles(transtype);
                }
                else
                    twain_session::callback_proc(twain_callback_values::DTWAIN_PREACQUIRE_TERMINATE, 0, reinterpret_cast<UINT_PTR>(m_pSession));
            }
            return acquire_return_type{ acquire_canceled, {} };
        }


        twain_source::acquire_return_type twain_source::acquire_to_file(transfer_type transtype)
        {
            acquire_characteristics& ac = *(m_pTwainSourceImpl->m_acquire_characteristics);
            file_transfer_options& ftOptions = ac.get_file_transfer_options();

            LONG dtwain_transfer_type = DTWAIN_USENATIVE;
            if (transtype == transfer_type::file_using_buffered)
                dtwain_transfer_type = DTWAIN_USEBUFFERED;
            else
            if (transtype == transfer_type::file_using_source)
                dtwain_transfer_type = DTWAIN_USESOURCEMODE;
            dtwain_transfer_type |= static_cast<LONG>(ftOptions.get_transfer_flags());

            const auto file_type = ftOptions.get_type();
            if (!file_type_info::is_universal_support(file_type))
            {
                // Test for file transfer support
                if (dtwain_transfer_type & DTWAIN_USESOURCEMODE)
                {
                    if (!API_INSTANCE DTWAIN_IsFileXferSupported(m_theSource, file_type))
                        return { API_INSTANCE DTWAIN_GetLastError(), {} };
                }

                // Check and set the compression type
                auto compress_option = ac.get_compression_options().get_compression();
                auto compressOk = API_INSTANCE DTWAIN_SetCompressionType(m_theSource, compress_option, 1);
                if ( !compressOk )
                    return { API_INSTANCE DTWAIN_GetLastError(), {} };
            }

            if (ftOptions.is_autocreate_directory())
                dtwain_transfer_type |= DTWAIN_CREATE_DIRECTORY;

            // check for auto increment
            filename_increment_options& inc = ftOptions.get_filename_increment_options();
            file_transfer_info fTransfer = get_file_transfer_info();
            API_INSTANCE DTWAIN_SetFileAutoIncrement(m_theSource, inc.get_increment(), inc.is_reset_count_used() ? TRUE : FALSE,
                inc.is_enabled() ? TRUE : FALSE);
            API_INSTANCE DTWAIN_EnableMsgNotify(1);

            if (dtwain_transfer_type & DTWAIN_USESOURCEMODE)
            {
                if ( !API_INSTANCE DTWAIN_IsFileXferSupported(m_theSource, file_type) )
                    return { API_INSTANCE DTWAIN_GetLastError(), {} };
            }
            if (file_type_info::is_multipage_type(file_type))
            {
                auto& paper_options = ac.get_paperhandling_options();

                // Check if feeder wasn't enabled
                if (!paper_options.is_feeder_enabled())
                {
                    // Now get if the user is using the flatbed to save multi-page images
                    auto& multisave_info = ac.get_file_transfer_options().get_multipage_save_options();

                    // turn on the feeder only if we see that the user has not set any
                    // multipage scan modes to save multi-page images using a flatbed device
                    if (multisave_info.get_save_mode() == multipage_save_mode::save_default)
                        API_INSTANCE DTWAIN_EnableFeeder(m_theSource, TRUE);
                }
            }
            general_options& gOpts = ac.get_general_options();
            bool isModeless = m_pSession->is_custom_twain_loop();
            API_INSTANCE DTWAIN_SetTwainMode(isModeless ? DTWAIN_MODELESS : DTWAIN_MODAL);
            LONG status;

            /* Create the array of names.  This function is to be used
               since the user may have entered a file name that has
               embedded spaces */
            auto AFileNames = API_INSTANCE DTWAIN_ArrayCreate(DTWAIN_ARRAYANSISTRING, 1);
            twain_array ta(AFileNames);
            API_INSTANCE DTWAIN_ArraySetAtANSIString(AFileNames, 0, ftOptions.get_name().c_str());

            auto retval = API_INSTANCE DTWAIN_AcquireFileEx(m_theSource, AFileNames,
                file_type,
                dtwain_transfer_type,
                gOpts.get_pixel_type(),
                static_cast<LONG>(gOpts.get_max_page_count()),
                ac.get_userinterface_options().is_shown(),
                gOpts.get_source_action() == sourceaction_type::closeafteracquire,
                &status) != 0;

            // if app is modeless, this needs to go back to the caller to loop
            if (retval)
            {
                if (status == DTWAIN_TN_ACQUIRECANCELLED)
                    return { acquire_canceled, {} };
                else
                    return{ acquire_ok, {} };
            }
            return { API_INSTANCE DTWAIN_GetLastError(), {} };
        }

        twain_source::acquire_return_type twain_source::acquire_to_image_handles(transfer_type transtype)
        {
            acquire_characteristics& ac = *(m_pTwainSourceImpl->m_acquire_characteristics);
            general_options& gOpts = ac.get_general_options();
            color_value::value_type ct = m_pTwainSourceImpl->m_capability_info->get_cap_values(ICAP_PIXELTYPE, capability_interface::get_current()).front();

            bool isModeless = m_pSession->is_custom_twain_loop();
            API_INSTANCE DTWAIN_SetTwainMode(isModeless ? DTWAIN_MODELESS : DTWAIN_MODAL);

            if (transtype == transfer_type::image_native || transtype == transfer_type::image_buffered)
            {
                twain_array images(API_INSTANCE DTWAIN_CreateAcquisitionArray());
                bool retval = false;
                if (transtype == transfer_type::image_native)
                {
                    retval = API_INSTANCE DTWAIN_AcquireNativeEx(m_theSource,
                        static_cast<LONG>(ct),
                        static_cast<LONG>(gOpts.get_max_page_count()),
                        ac.get_userinterface_options().is_shown(),
                        ac.get_general_options().get_source_action() == sourceaction_type::closeafteracquire,
                        images.get_array(), nullptr) != 0;
                }
                else
                {
                    // Set the compression type first, if it needs to be set
                    options_base::apply(*this, ac.get_compression_options());
                    buffered_transfer_info& bt = get_buffered_transfer_info();
                    bt.init_transfer(
                        static_cast<compression_value::value_type>(m_pTwainSourceImpl->m_capability_info->get_cap_values(ICAP_COMPRESSION, capability_interface::get_current()).front()));
                    retval = API_INSTANCE DTWAIN_AcquireBufferedEx(m_theSource,
                        static_cast<LONG>(ct),
                        static_cast<LONG>(gOpts.get_max_page_count()),
                        ac.get_userinterface_options().is_shown(),
                        gOpts.get_source_action() == sourceaction_type::closeafteracquire,
                        images.get_array(),
                        nullptr) != 0;
                }
                int32_t last_error = twain_session::get_last_error();
                if (retval || last_error == DTWAIN_NO_ERROR)
                    return { acquire_ok, std::move(images) };
                else
                    return { last_error, {} };
            }
            return { acquire_canceled, {} };
        }
        void twain_source::wait_for_feeder(bool& status)
        {
            using namespace std::chrono_literals;
            // check for feeder stuff here
            paperhandling_info paperinfo;
            paperinfo.get_info(*this);

            bool isfeedersupported = paperinfo.is_feedersupported();
            if (!isfeedersupported)
            {
                status = true;
                return;
            }
            twain_std_array<capability_type::feederenabled_type, 1> arr;
            arr[0] = 1;
            m_pTwainSourceImpl->m_capability_info->set_cap_values< CAP_FEEDERENABLED_>(arr);
            auto vEnabled = m_pTwainSourceImpl->m_capability_info->get_cap_values< CAP_FEEDERENABLED_>(capability_interface::get_current());
            if (vEnabled.empty() || !vEnabled.front())
            {
                // feeder not enabled
                status = true;
                return;
            }

            bool isfeederloaded = m_pTwainSourceImpl->m_capability_info->is_cap_supported(CAP_FEEDERLOADED);
            if (!isfeederloaded)
            {
                // Cannot detect if feeder is loaded
                status = true;
                return;
            }

            auto timeoutval = get_acquire_characteristics().get_paperhandling_options().get_feederwait();

            twain_timer theTimer;

            // loop until feeder is loaded
            while (!m_pTwainSourceImpl->m_capability_info->get_cap_values<CAP_FEEDERLOADED_>(capability_interface::get_current()).front())
            {
                if (timeoutval != -1)
                {
                    if (theTimer.elapsed() > timeoutval)
                    {
                        status = false;
                        return;
                    }
                }
                std::this_thread::sleep_for(1ms);
            }
            status = true;
            return;
        }
 
        std::string& twain_source::get_details(dynarithmic::twain::details_info info)
        {
            bool bGetDetails = false;
            if (!info.bRefresh)
            {
                if (m_source_details.empty())
                    bGetDetails = true;
            }
            else
                bGetDetails = true;
            if (bGetDetails)
            {
                auto nChars = API_INSTANCE DTWAIN_GetSourceDetailsA(get_source_info().get_product_name().c_str(), nullptr, 0, info.indentFactor, TRUE); 
                m_source_details.clear();
                m_source_details.resize(nChars);
                API_INSTANCE DTWAIN_GetSourceDetailsA(get_source_info().get_product_name().c_str(), &m_source_details[0], 
                                                        static_cast<LONG>(m_source_details.size()), info.indentFactor, FALSE);
            }
            return m_source_details;
        }

        HANDLE twain_source::get_current_image() 
        { 
            if ( m_theSource ) 
                return API_INSTANCE DTWAIN_GetCurrentAcquiredImage(m_theSource);  
            return nullptr;
        }

        image_information twain_source::get_current_image_information() const
        {
            DTWAIN_FLOAT xRes, yRes;
            LONG width, length, numsamples;
            DTWAIN_ARRAY bitspersample;
            LONG bitsperpixel;
            LONG planar;
            LONG pixeltype, compression;
            image_information iinfo;
            if (API_INSTANCE DTWAIN_GetImageInfo(m_theSource, &xRes, &yRes, &width, &length, &numsamples,
                &bitspersample, &bitsperpixel, &planar, &pixeltype, &compression))
            {
                iinfo.x_resolution = xRes;
                iinfo.y_resolution = yRes;
                iinfo.bitsPerPixel = bitsperpixel;
                iinfo.compression = compression;
                iinfo.length = length;
                iinfo.pixelType = pixeltype;
                iinfo.planar = planar ? true : false;
                iinfo.width = width;
                iinfo.numsamples = numsamples;
                twain_array arr(bitspersample);
                twain_array_copy_traits::copy_from_twain_array(arr, iinfo.bitsPerSample);
                iinfo.bitsPerSample.resize(numsamples);
            }
            return iinfo;
        }

        bool twain_source::acquire_no_error(int32_t errCode)
        {
            return errCode == acquire_ok ||
                errCode == acquire_canceled;
        }

        bool twain_source::acquire_timed_out(int32_t errCode)
        {
            return errCode == acquire_timeout;
        }

        bool twain_source::acquire_internal_error(int32_t errCode)
        {
            return !(acquire_no_error(errCode) || acquire_timed_out(errCode));
        }

        image_handler twain_source::get_images(const twain_array& images)
        {
            image_handler ih;
            const auto acq_count = images.get_count();
            for (size_t i = 0; i < acq_count; ++i)
            {
                twain_array img_array(API_INSTANCE DTWAIN_GetAcquiredImageArray(images.get_array(), static_cast<LONG>(i)));
                ih.add_new_acquisition();
                const size_t image_count = img_array.get_count();
                const HANDLE* handleBuffer = reinterpret_cast<HANDLE*>(img_array.get_buffer<HANDLE>());
                for (size_t j = 0; j < image_count; ++j)
                    ih.push_back_image(handleBuffer[j]);
            }
            return ih;
        }

        std::vector<twain_source::custom_data_type> twain_source::get_custom_data() const
        {
            const capability_interface& ci = get_capability_interface();
            if (!ci.is_customdsdata_supported())
                return {};
            DWORD actualSize = 0;
            if (!API_INSTANCE DTWAIN_GetCustomDSData(m_theSource, nullptr, 0, &actualSize, DTWAINGCD_COPYDATA))
                return {};
            if (actualSize > 0)
            {
                std::vector<custom_data_type> retContainer(actualSize);
                API_INSTANCE DTWAIN_GetCustomDSData(m_theSource, retContainer.data(), actualSize, nullptr, DTWAINGCD_COPYDATA);
                return retContainer;
            }
            return {};
        }

        std::unique_ptr<extendedimage_info> twain_source::init_extendedimage_info()
        {
            auto extInfo = std::make_unique<extendedimage_info>();
            extInfo->attach(this);
            return extInfo;
        }

        bool twain_source::showui_only()
        {
            const capability_interface& ci = get_capability_interface();
            if (!ci.is_customdsdata_supported())
                return false;
            if (!API_INSTANCE DTWAIN_ShowUIOnly(m_theSource))
                return false;
            return true;
        }


        const capability_interface& twain_source::get_capability_interface() const noexcept { return *(m_pTwainSourceImpl->m_capability_info); }
        buffered_transfer_info& twain_source::get_buffered_transfer_info() noexcept { return *(m_pTwainSourceImpl->m_buffered_info); }
        acquire_characteristics& twain_source::get_acquire_characteristics() { return *(m_pTwainSourceImpl->m_acquire_characteristics); }
        twain_identity twain_source::get_source_info() const noexcept { return m_sourceInfo; }
        bool twain_source::is_selected() const noexcept { return m_bIsSelected; }
        bool twain_source::is_open() const { return API_INSTANCE DTWAIN_IsSourceOpen(m_theSource) ? true : false; }
        twain_source& twain_source::set_acquire_characteristics(const acquire_characteristics& ac) noexcept 
                    { *(m_pTwainSourceImpl->m_acquire_characteristics) = ac; return *this; }
        bool twain_source::set_tiff_compress_type(tiffcompress_value::value_type compress_type) 
             { return API_INSTANCE DTWAIN_SetTIFFCompressType(m_theSource, static_cast<LONG>(compress_type)); }

        bool twain_source::is_acquiring() const
        {
            if (m_theSource)
                return API_INSTANCE DTWAIN_IsSourceAcquiring(m_theSource);
            return false;
        }

        bool twain_source::is_uienabled() const
        {
            if (m_theSource)
                return API_INSTANCE DTWAIN_IsUIEnabled(m_theSource);
            return false;
        }

		bool twain_source::is_uionlysupported() const
		{
			if (m_theSource)
                return tribool::true_(m_bUIOnlySupported) ? true : false;
            return false;
        }

        bool twain_source::feederwait_supported() const
        {
            if (m_theSource)
            {
                const capability_interface& ci = get_capability_interface();

                // First check if ICAP_FEEDERENABLED is supported, and if so, get the
                // current value.
                auto vect = ci.get_feederenabled();
                if (!vect.empty())
                {
                    // Supported, so see if we can actually use the feeder    
                    if (vect[0] == false)
                    {
                        // Feeder turned off, so
                        // temporarily enable the feeder
                        ci.set_feederenabled({ true });

                        // See if we enabled it successfully
                        bool feederenabled = false;
                        vect = ci.get_feederenabled();
                        feederenabled = !vect.empty() && vect[0] == true;

                        if (feederenabled)
                        {
                            // reset to original value
                            ci.set_feederenabled({ false });
                        }
                        else
                            return false; // couldn't enable the feeder, so can't really
                                          // test the feederloaded capability.
                    }
                    // check to see if ICAP_FEEDERLOADED capability is supported.
                    return ci.is_feederloaded_supported();
                }
            }
			return false;
		}
	}
}
