/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2023 Dynarithmic Software.

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
#ifndef DTWAIN_ACQUIRE_CHARACTERISTICS_HPP
#define DTWAIN_ACQUIRE_CHARACTERISTICS_HPP

#include <array>
#include <algorithm>

#include <dynarithmic/twain/options/pages_options.hpp>
#include <dynarithmic/twain/options/jobcontrol_options.hpp>
#include <dynarithmic/twain/options/paperhandling_options.hpp>
#include <dynarithmic/twain/options/imagetype_options.hpp>
#include <dynarithmic/twain/options/file_transfer_options.hpp>
#include <dynarithmic/twain/options/general_options.hpp>
#include <dynarithmic/twain/options/compression_options.hpp>
#include <dynarithmic/twain/options/pdf_options.hpp>
#include <dynarithmic/twain/options/language_options.hpp>
#include <dynarithmic/twain/options/buffered_transfer_options.hpp>
#include <dynarithmic/twain/options/ui_options.hpp>
#include <dynarithmic/twain/options/deviceparams_options.hpp>
#include <dynarithmic/twain/options/powermonitor_options.hpp>
#include <dynarithmic/twain/options/doublefeed_options.hpp>
#include <dynarithmic/twain/options/autoadjust_options.hpp>
#include <dynarithmic/twain/options/barcodedetection_options.hpp>
#include <dynarithmic/twain/options/capnegotiation_options.hpp>
#include <dynarithmic/twain/options/patchcode_options.hpp>
#include <dynarithmic/twain/options/deviceevents_options.hpp>
#include <dynarithmic/twain/imagehandler/image_handler.hpp>
#include <dynarithmic/twain/options/autocapture_options.hpp>
#include <dynarithmic/twain/options/audiblealarms_options.hpp>
#include <dynarithmic/twain/options/imageinformation_options.hpp>
#include <dynarithmic/twain/options/imageparameter_options.hpp>
#include <dynarithmic/twain/options/resolution_options.hpp>
#include <dynarithmic/twain/options/color_options.hpp>
#include <dynarithmic/twain/options/micr_options.hpp>
#include <dynarithmic/twain/options/imprinter_options.hpp>
#include <dynarithmic/twain/options/blankpage_options.hpp>
#include <dynarithmic/twain/options/autoscanning_options.hpp>

namespace dynarithmic {
namespace twain {
     class acquire_characteristics
     {
         private:
             audiblealarms_options m_audiblealarms_options;
             autoadjust_options m_autoadjust_options;
             autocapture_options m_autocapture_options;
             autoscanning_options m_autoscanning_options;
             barcodedetection_options m_barcodedetection_options;
             blankpage_options m_blankpage_options;
             buffered_transfer_options m_bufferedtransfer_options;
             capnegotiation_options m_capnegotiation_options;
             color_options m_color_options;
             compression_options m_compression_options;
             deviceevent_options m_deviceevents_options;
             deviceparams_options m_deviceparams_options;
             doublefeed_options m_doublefeed_options;
             file_transfer_options m_filetransfer_options;
             general_options m_general_options;
             imageinformation_options m_imageinformation_options;
             imageparameter_options m_imageparameter_options;
             imagetype_options m_imagetype_options;
             imprinter_options m_imprinter_options;
             jobcontrol_options m_jobcontrol_options;
             language_options m_language_options;
             micr_options m_micr_options;
             pages_options m_pages_options;
             paperhandling_options m_paperhandling_options;
             patchcode_options m_patchcode_options;
             pdf_options m_pdf_options;
             powermonitor_options m_powermonitor_options;
             resolution_options m_resolution_options;
             userinterface_options m_userinterface_options;

             friend class twain_source;

         public:
             static constexpr uint8_t apply_audiblealarms       = 0;
             static constexpr uint8_t apply_autoadjust          = apply_audiblealarms + 1;
             static constexpr uint8_t apply_autocapture         = apply_audiblealarms + 2;
             static constexpr uint8_t apply_autoscanning        = apply_audiblealarms + 3;
             static constexpr uint8_t apply_barcodedetection    = apply_audiblealarms + 4;
             static constexpr uint8_t apply_capnegotiation      = apply_audiblealarms + 5;
             static constexpr uint8_t apply_coloroptions        = apply_audiblealarms + 6;
             static constexpr uint8_t apply_compressionoptions  = apply_audiblealarms + 7;
             static constexpr uint8_t apply_deviceevents        = apply_audiblealarms + 8;
             static constexpr uint8_t apply_deviceparams        = apply_audiblealarms + 9;
             static constexpr uint8_t apply_doublefeedoptions   = apply_audiblealarms + 10;
             static constexpr uint8_t apply_filetransferoptions = apply_audiblealarms + 11;
             static constexpr uint8_t apply_generaloptions      = apply_audiblealarms + 12;
             static constexpr uint8_t apply_imageinformation    = apply_audiblealarms + 13;
             static constexpr uint8_t apply_imageparameter      = apply_audiblealarms + 14;
             static constexpr uint8_t apply_imprinter           = apply_audiblealarms + 15;
             static constexpr uint8_t apply_pages               = apply_audiblealarms + 16;
             static constexpr uint8_t apply_pdfoptions          = apply_audiblealarms + 17;
             static constexpr uint8_t apply_jobcontroloptions   = apply_audiblealarms + 18;
             static constexpr uint8_t apply_microptions         = apply_audiblealarms + 19;
             static constexpr uint8_t apply_paperhandlingoptions = apply_audiblealarms + 20;
             static constexpr uint8_t apply_patchcodeoptions    = apply_audiblealarms + 21;
             static constexpr uint8_t apply_imagetypeoptions    = apply_audiblealarms + 22;
             static constexpr uint8_t apply_bufferedtransferoptions = apply_audiblealarms + 23;
             static constexpr uint8_t apply_languageoptions     = apply_audiblealarms + 24;
             static constexpr uint8_t apply_userinterfaceoptions = apply_audiblealarms + 25;
             static constexpr uint8_t apply_blankpageoptions    = apply_audiblealarms + 26;
             static constexpr uint8_t apply_resolutionoptions   = apply_audiblealarms + 27;
             static constexpr uint8_t apply_powermonitor        = apply_audiblealarms + 28;

        private:
            static constexpr uint8_t num_appliers = apply_powermonitor + 1;
            std::array<bool, num_appliers> m_aAppliers;

        public:
            acquire_characteristics();

            const std::array<bool, num_appliers>& get_appliers() { return m_aAppliers; }

            template <typename Container = std::vector<uint8_t>>
            acquire_characteristics& set_appliers(Container& c, bool turnon = true)
            {
                for (size_t i = 0; i < std::size(c); ++i)
                {
                    if (c[i] < num_appliers)
                        m_aAppliers[c[i]] = turnon;
                }
                return *this;
            }

             /// The audiblealarms_options allows setting the audible alarms options for the TWAIN device
             /// 
             /// The audiblealarms_options are described by the following TWAIN capabilities:\n
             /// **CAP_ALARMS**      -- Turns specific audible alarms on and off.<br>
             /// **CAP_ALARMVOLUME** -- Controls the volume of a device's audible alarm.<br>
             /// @returns a reference to the audiblealarms_options instance
             /// @note Refer to the TWAIN Specification 2.4 Chapter 10
             /// @note https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
             audiblealarms_options&      get_audiblealarms_options() noexcept { return m_audiblealarms_options; }

             /// The autoadjust_options describes the options used by the TWAIN device that can be used to
             /// automatically adjust the color, skew, rotation, size, etc.
             /// 
             /// **CAP_AUTOMATICSENSEMEDIUM** -- Configures a Source to check for paper in the Automatic Document Feeder. <br>
             /// **ICAP_AUTODISCARDBLANKPAGES** -- Discards blank pages.<br>
             /// **ICAP_AUTOMATICBORDERDETECTION** -- Turns automatic border detection on and off.<br>
             /// **ICAP_AUTOMATICCOLORENABLED** -- Detects the pixel type of the image and returns either a color image or a non - color image specified by ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE.<br>
             /// **ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE** -- Specifies the non-color pixel type to use when automatic color is enabled.<br>                                  
             /// **ICAP_AUTOMATICCROPUSESFRAME** -- Reduces the amount of data captured from the device, potentially improving the performance of the driver.<br>
             /// **ICAP_AUTOMATICDESKEW** -- Turns automatic skew correction on and off.<br>
             /// **ICAP_AUTOMATICLENGTHDETECTION** -- Controls the automatic detection of the length of a document, this is intended for use with an Automatic Document Feeder.<br>
             /// **ICAP_AUTOMATICROTATE** -- When **true**, depends on source to automatically rotate the image. <br>
             /// **ICAP_AUTOSIZE** -- Force the output image dimensions to match either the current value of ICAP_SUPPORTEDSIZES or any of its current allowed values.<br>
             /// **ICAP_FLIPROTATION** -- Orients images that flip orientation every other image.<br>
             /// **ICAP_IMAGEMERGE** -- Merges the front and rear image of a document in one of four orientations: front on the top.<br>
             /// **ICAP_IMAGEMERGEHEIGHTTHRESHOLD** -- Specifies a Y-Offset in ICAP_UNITS units.
             /// @returns a reference to the autoadjust_options instance
             /// @note Refer to the TWAIN Specification 2.4 Chapter 10
             /// @note https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
             autoadjust_options&         get_autoadjust_options()  noexcept { return m_autoadjust_options; }

             /// The autocapture_options specifies time intervals before and after an image is captured.
             /// The autocapture_options are described by the following TWAIN capabilities:\n
             /// **CAP_AUTOMATICCAPTURE** -- Specifies the number of images to automatically capture.<br>
             /// **CAP_TIMEBEFOREFIRSTCAPTURE** -- Selects the number of seconds before the first picture taken.<br>
             /// **CAP_TIMEBETWEENCAPTURES** -- Selects the hundredths of a second to wait between pictures taken.<br>
             /// @returns a reference to the autocapture_options instance
             /// @note Refer to the TWAIN Specification 2.4 Chapter 10
             /// @note https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
             autocapture_options&        get_autocapture_options() noexcept { return m_autocapture_options; }

             /// The autoscanning_options specifies automatic delivery of images, buffering images etc.
             /// The autoscanning_options are described by the following TWAIN capabilities:\n
             /// **CAP_AUTOSCAN** -- Enables the source’s automatic document scanning process.<br>
             /// **CAP_CAMERAENABLED** -- Delivers images from the current camera.<br>
             /// **CAP_CAMERAORDER** -- Selects the order of output for Single Document Multiple Image mode.<br>
             /// **CAP_CAMERASIDE** -- Sets the top and bottom values of cameras in a scanning device.<br>
             /// **CAP_MAXBATCHBUFFERS** -- Describes the number of pages that the scanner can buffer when CAP_AUTOSCAN is enabled.<br>
             /// @returns a reference to the autoscanning_options instance
             /// @note Refer to the TWAIN Specification 2.4 Chapter 10
             /// @note https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
             /// @note The application may have to explicitly set the camera side option prior to the acquire() 
             /// call if capabilities need to be set for the selected camera.  For example, to set capabilities for a 
             /// particular camera, the camera side must be set first before the capability can take affect for that particular camera.
             /// @see dynarithmic::twain::capability_interface::set_cameraside()         
             autoscanning_options&       get_autoscanning_options() noexcept { return m_autoscanning_options; }

             /// The barcodedetection_options specifies automatic delivery of images, buffering images etc.
             /// The barcodedetection_options are described by the following TWAIN capabilities:\n
             /// **ICAP_BARCODEDETECTIONENABLED** -- Turns bar code detection on and off.<br>
             /// **ICAP_SUPPORTEDBARCODETYPES** -- Provides a list of bar code types that can be detected by current twain_source<br>
             /// **ICAP_BARCODEMAXRETRIES** -- Restricts the number of times a search will be retried if no bar codes are found.<br>
             /// **ICAP_BARCODEMAXSEARCHPRIORITIES** -- Specifies the maximum number of supported search priorities.<br>
             /// **ICAP_BARCODESEARCHMODE** -- Restricts bar code searching to certain orientations, or prioritizes one orientation over another.<br>
             /// **ICAP_BARCODESEARCHPRIORITIES** -- A prioritized list of bar code types dictating the order in which they will be sought.<br>
             /// **ICAP_BARCODETIMEOUT** -- Restricts the total time spent on searching for bar codes on a page.
             /// @returns a reference to the barcodedetection_options instance
             /// @note Refer to the TWAIN Specification 2.4 Chapter 10
             /// @note https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
             barcodedetection_options&   get_barcodedetection_options() noexcept { return m_barcodedetection_options; }

             buffered_transfer_options&  get_buffered_transfer_options()  noexcept { return m_bufferedtransfer_options; }
             capnegotiation_options&     get_capnegotiation_options() noexcept { return m_capnegotiation_options; }
             color_options&              get_color_options() noexcept { return m_color_options; }
             compression_options&        get_compression_options() noexcept { return m_compression_options; }
             deviceevent_options&        get_deviceevent_options() noexcept { return m_deviceevents_options; }
             deviceparams_options&       get_deviceparams_options() noexcept { return m_deviceparams_options; }
             doublefeed_options&         get_doublefeed_options() noexcept { return m_doublefeed_options; }
             file_transfer_options&      get_file_transfer_options() noexcept { return m_filetransfer_options; }
             general_options&            get_general_options() noexcept { return m_general_options; }
             imageinformation_options&   get_imageinformation_options() noexcept { return m_imageinformation_options; }
             imageparameter_options&     get_imageparamter_options() noexcept { return m_imageparameter_options; }
             imagetype_options&          get_imagetype_options() noexcept { return m_imagetype_options; }
             imprinter_options&          get_imprinter_options() noexcept { return m_imprinter_options; }
             jobcontrol_options&         get_jobcontrol_options() noexcept { return m_jobcontrol_options; }
             language_options&           get_language_options() noexcept { return m_language_options;  }
             micr_options&               get_micr_options() noexcept { return m_micr_options; }
             pages_options&              get_pages_options() noexcept { return m_pages_options; }
             paperhandling_options&      get_paperhandling_options() noexcept { return m_paperhandling_options; }
             patchcode_options&          get_patchcode_options() noexcept { return m_patchcode_options; }
             powermonitor_options&       get_powermonitor_options() noexcept { return m_powermonitor_options; }
             resolution_options&         get_resolution_options() noexcept { return m_resolution_options; }
             userinterface_options&      get_userinterface_options() noexcept { return m_userinterface_options; }
             blankpage_options&          get_blank_page_options() noexcept { return m_blankpage_options; }
             pdf_options&                get_pdf_options() noexcept { return m_pdf_options; }
     };
  }
}
#endif