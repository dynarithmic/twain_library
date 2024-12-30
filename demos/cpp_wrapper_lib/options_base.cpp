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
#include <dynarithmic/twain/options/options_base.hpp>
#include <dynarithmic/twain/twain_source.hpp>
#include <dynarithmic/twain/capability_interface/capability_interface.hpp>
#include <dynarithmic/twain/options/pages_options.hpp>
#include <dynarithmic/twain/acquire_characteristics/acquire_characteristics.hpp>

namespace dynarithmic
{
    namespace twain
    {
        void options_base::apply(twain_source& ts, pages_options& po)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values<CAP_SEGMENTED_>({ static_cast<capability_type::segmented_type>(po.m_SegmentedValue) });
            if (po.m_MaxFrames >= 1)
                ci.set_cap_values<ICAP_MAXFRAMES_>({ po.m_MaxFrames });
            else
                ci.set_cap_values<ICAP_MAXFRAMES_>({});
            if (po.m_Frame != twain_frame<double>())
                ci.set_cap_values<ICAP_FRAMES_>({ po.m_Frame });
            else
                ci.set_cap_values<ICAP_FRAMES_>({});
            if (po.m_SupportedSize != supportedsizes_value::default_val)
                ci.set_cap_values<ICAP_SUPPORTEDSIZES_>({ static_cast<capability_type::supportedsizes_type> (po.m_SupportedSize) });
            else
                ci.set_cap_values<ICAP_SUPPORTEDSIZES_>({});
        }

        void options_base::apply(twain_source& ts, jobcontrol_options& jc)
        {
            auto& ci = ts.get_capability_interface();
            if (ci.is_cap_supported(CAP_JOBCONTROL))
            {
                ci.set_cap_values<CAP_JOBCONTROL_>({ jc.m_option });
            }
        }

        void options_base::apply(twain_source& ts, paperhandling_options& po)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_AUTOFEED_>({ po.m_bAutoFeed });
            ci.set_cap_values< CAP_DUPLEXENABLED_>({ po.m_bDuplexEnabled });
            ci.set_cap_values< CAP_FEEDERENABLED_>({ po.m_bFeederEnabled });
            ci.set_cap_values< CAP_FEEDERPREP_>({ po.m_bFeederPrep });
            if (po.m_FeederType != feedertype_value::default_val)
                ci.set_cap_values< ICAP_FEEDERTYPE_>({ static_cast<capability_type::feedertype_type>(po.m_FeederType) });
            if (po.m_FeederOrder != feederorder_value::default_val)
                ci.set_cap_values< CAP_FEEDERORDER_>({ static_cast<capability_type::feederorder_type>(po.m_FeederOrder) });
            ci.set_cap_values< CAP_FEEDERPOCKET_>(po.m_vFeederPocket);
            ci.set_cap_values< CAP_PAPERHANDLING_>(po.m_vPaperHandling);
        }

        void options_base::apply(twain_source& ts, imagetype_options& io)
        {
            auto& ci = ts.get_capability_interface();
            if (io.m_PixelType != color_value::default_color)
                ci.set_cap_values< ICAP_PIXELTYPE_>({ io.m_PixelType });
            else
                ci.set_cap_values< ICAP_PIXELTYPE_>({});

            if (io.m_BitDepth != 0)
                ci.set_cap_values< ICAP_BITDEPTH_>({ io.m_BitDepth });
            else
                ci.set_cap_values< ICAP_BITDEPTH_>({});

            if (io.m_BitDepthReduction != bitdepthreduction_value::default_val)
                ci.set_cap_values< ICAP_BITDEPTHREDUCTION_>({ io.m_BitDepthReduction });
            else
                ci.set_cap_values< ICAP_BITDEPTHREDUCTION_>({});

            if (io.m_BitOrderValue != bitorder_value::default_val)
                ci.set_cap_values< ICAP_BITORDER_>({ io.m_BitOrderValue });
            else
                ci.set_cap_values< ICAP_BITORDER_>({});

            if (!io.m_vCustHalfTone.empty())
                ci.set_cap_values< ICAP_CUSTHALFTONE_>(io.m_vCustHalfTone);
            else
                ci.set_cap_values< ICAP_CUSTHALFTONE_>({});

            if (!io.m_sHalftone.empty())
                ci.set_cap_values< ICAP_HALFTONES_>({ io.m_sHalftone });
            else
                ci.set_cap_values< ICAP_HALFTONES_>({});

            if (io.m_PixelFlavor != pixelflavor_value::default_val)
                ci.set_cap_values< ICAP_PIXELFLAVOR_>({ io.m_PixelFlavor });
            else
                ci.set_cap_values< ICAP_PIXELFLAVOR_>({});

            if (io.m_Threshold != imagetype_options::default_threshold)
                ci.set_cap_values< ICAP_THRESHOLD_>({ io.m_Threshold });
            else
                ci.set_cap_values< ICAP_THRESHOLD_>({});
        }

        void options_base::apply(twain_source& ts, file_transfer_options_ex& fo)
        {
            auto& ci = ts.get_capability_interface();
            auto vFormat = ci.get_cap_values<ICAP_IMAGEFILEFORMAT_>();
            std::copy(vFormat.begin(), vFormat.end(), std::inserter(fo.all_file_types, fo.all_file_types.end()));
            fo.m_twain_source = ts.get_source();
        }

        void options_base::apply(twain_source& ts, compression_options& co)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< ICAP_COMPRESSION_>({ co.m_CompressionValue });
            ci.set_cap_values< ICAP_BITORDERCODES_>({ co.m_BitOrderValue });
            ci.set_cap_values< ICAP_CCITTKFACTOR_>({ co.m_CCITKFactor });
            if (co.m_JpegPixelType != jpegpixel_value::default_val)
                ci.set_cap_values< ICAP_JPEGPIXELTYPE_>({ co.m_JpegPixelType });
            ci.set_cap_values< ICAP_JPEGQUALITY_>({ co.m_JpegQuality });
            if (co.m_JpegSubSampleValue != jpegsubsampling_value::default_val)
                ci.set_cap_values< ICAP_JPEGSUBSAMPLING_>({ co.m_JpegSubSampleValue });
            ci.set_cap_values< ICAP_PIXELFLAVORCODES_>({ co.m_PixelFlavor });
            ci.set_cap_values< ICAP_TIMEFILL_>({ co.m_TimeFill });
        }

        void options_base::apply(twain_source& ts, userinterface_options& ui)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_INDICATORS_>({ ui.m_bShowIndicators });
            ci.set_cap_values< CAP_INDICATORSMODE_>(ui.m_vIndicatorMode);
        }

        void options_base::apply(twain_source& ts, language_options& lo)
        {
            auto& ci = ts.get_capability_interface();
            if (lo.m_Language != language_value::default_val)
                ci.set_cap_values< CAP_LANGUAGE_>({ lo.m_Language });
            else
                ci.set_cap_values< CAP_LANGUAGE_>({});
        }

        void options_base::apply(twain_source& ts, deviceparams_options& dp)
        {
            auto& ci = ts.get_capability_interface();
            if (dp.m_unitvalue != units_value::default_val)
                ci.set_cap_values< ICAP_UNITS_>({ dp.m_unitvalue });
            else
                ci.set_cap_values< ICAP_UNITS_>({});
            if (dp.m_ExposureTime != (std::numeric_limits<double>::min)())
                ci.set_cap_values< ICAP_EXPOSURETIME_>({ dp.m_ExposureTime });
            if (dp.m_FlashUsed != flashused_value::default_val)
                ci.set_cap_values< ICAP_FLASHUSED2_>({ dp.m_FlashUsed });
            if (dp.m_ImageFilter != imagefilter_value::default_val)
                ci.set_cap_values< ICAP_IMAGEFILTER_>({ dp.m_ImageFilter });
            if (dp.m_lightpath != lightpath_value::default_val)
                ci.set_cap_values< ICAP_LIGHTPATH_>({ dp.m_lightpath });
            if (dp.m_filmType != filmtype_value::default_val)
                ci.set_cap_values< ICAP_FILMTYPE_>({ dp.m_filmType });
            if (dp.m_lightsource != lightsource_value::default_val)
                ci.set_cap_values< ICAP_LIGHTSOURCE_>({ dp.m_lightsource });
            if (dp.m_noisefilter != noisefilter_value::default_val)
                ci.set_cap_values< ICAP_NOISEFILTER_>({ dp.m_noisefilter });
            if (dp.m_overscan != overscan_value::default_val)
                ci.set_cap_values< ICAP_OVERSCAN_>({ dp.m_overscan });
            if (dp.m_zoomFactor != (std::numeric_limits<capability_type::zoomfactor_type>::min)())
                ci.set_cap_values< ICAP_ZOOMFACTOR_>({ dp.m_zoomFactor });
        }

        void options_base::apply(twain_source& ts, powermonitor_options& po)
        {
            if (po.m_powersavetime != powermonitor_options::default_val)
            {
                auto& ci = ts.get_capability_interface();
                ci.set_cap_values< CAP_POWERSAVETIME_>({ po.m_powersavetime });
            }
        }

        void options_base::apply(twain_source& ts, doublefeed_options& df)
        {
            auto& ci = ts.get_capability_interface();
            if (df.m_detection != doublefeeddetection_value::default_val)
                ci.set_cap_values< CAP_DOUBLEFEEDDETECTION_>({ df.m_detection });
            if (df.m_sensitivity != doublefeedsensitivity_value::default_val)
                ci.set_cap_values< CAP_DOUBLEFEEDDETECTIONSENSITIVITY_>({ df.m_sensitivity });
            if (df.m_length != doublefeed_options::default_length)
                ci.set_cap_values< CAP_DOUBLEFEEDDETECTIONLENGTH_>({ df.m_length });
            else
                ci.set_cap_values< CAP_DOUBLEFEEDDETECTIONLENGTH_>({});
            ci.set_cap_values< CAP_DOUBLEFEEDDETECTIONRESPONSE_>(df.m_vResponses);
        };

        void options_base::apply(twain_source& ts, autoadjust_options& ao)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_AUTOMATICSENSEMEDIUM_>({ ao.m_bSenseMedium });
            ci.set_cap_values< ICAP_AUTODISCARDBLANKPAGES_>({ ao.m_bDiscardBlankPages });
            ci.set_cap_values< ICAP_AUTOMATICBORDERDETECTION_>({ ao.m_bBorderDetection });
            ci.set_cap_values< ICAP_AUTOMATICCOLORENABLED_>({ ao.m_bColorEnabled });
            ci.set_cap_values< ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_>({ ao.m_ColorNonColorPixelType });
            ci.set_cap_values< ICAP_AUTOMATICDESKEW_>({ ao.m_bDeskew });
            ci.set_cap_values< ICAP_AUTOMATICLENGTHDETECTION_>({ ao.m_bLengthDetection });
            ci.set_cap_values< ICAP_AUTOMATICROTATE_>({ ao.m_bRotate });
            ci.set_cap_values< ICAP_AUTOSIZE_>({ ao.m_AutoSize });
            if (ao.m_FlipRotation == fliprotation_value::default_flip)
                ci.set_cap_values< ICAP_FLIPROTATION_>({});
            else
                ci.set_cap_values< ICAP_FLIPROTATION_>({ ao.m_FlipRotation });
            ci.set_cap_values< ICAP_IMAGEMERGE_>({ ao.m_ImageMerge });
            if (ao.m_ImageMergeHeightThreshold != autoadjust_options::disable_threshold)
                ci.set_cap_values< ICAP_IMAGEMERGEHEIGHTTHRESHOLD_>({ ao.m_ImageMergeHeightThreshold });
        }

        void options_base::apply(twain_source& ts, barcodedetection_options& bo)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< ICAP_BARCODEDETECTIONENABLED_>({ bo.m_bDetectionEnabled });
            if (bo.m_bDetectionEnabled)
            {
                ci.set_cap_values< ICAP_BARCODEMAXRETRIES_>({ bo.m_MaxRetries });
                ci.set_cap_values< ICAP_BARCODEMAXSEARCHPRIORITIES_>({ bo.m_MaxSearchPriorities });
                if (bo.m_SearchMode != barcodesearchmode_value::default_val)
                    ci.set_cap_values< ICAP_BARCODESEARCHMODE_>({ bo.m_SearchMode });
                ci.set_cap_values< ICAP_BARCODESEARCHPRIORITIES_>(bo.m_vSearchPriority);
                ci.set_cap_values< ICAP_BARCODETIMEOUT_>({ bo.m_TimeOut });
            }
        }

        void options_base::apply(twain_source& ts, patchcode_options& pc)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< ICAP_PATCHCODEDETECTIONENABLED_>({ pc.m_bDetectionEnabled });
            if (pc.m_bDetectionEnabled)
            {
                ci.set_cap_values< ICAP_PATCHCODEMAXRETRIES_>({ pc.m_MaxRetries });
                ci.set_cap_values< ICAP_PATCHCODEMAXSEARCHPRIORITIES_>({ pc.m_MaxSearchPriorities });
                if (pc.m_SearchMode != patchcodesearchmode_value::default_val)
                    ci.set_cap_values< ICAP_PATCHCODESEARCHMODE_>({ pc.m_SearchMode });
                ci.set_cap_values< ICAP_PATCHCODESEARCHPRIORITIES_>(pc.m_vSearchPriority);
                ci.set_cap_values< ICAP_PATCHCODETIMEOUT_>({ pc.m_TimeOut });
            }
        }

        void options_base::apply(twain_source& ts, autocapture_options& ac)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_AUTOMATICCAPTURE_>({ ac.m_NumImages });
            ci.set_cap_values< CAP_TIMEBEFOREFIRSTCAPTURE_>({ ac.m_TimeBefore });
            ci.set_cap_values< CAP_TIMEBETWEENCAPTURES_>({ ac.m_TimeBetween });
        }

        void options_base::apply(twain_source& ts, imageinformation_options& io)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_AUTHOR_>({ io.m_sAuthor });
            ci.set_cap_values< CAP_CAPTION_>({ io.m_sCaption });
            ci.set_cap_values< ICAP_EXTIMAGEINFO_>({ io.m_bExtImageInfo });
        }

        void options_base::apply(twain_source& ts, imageparameter_options& io)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_THUMBNAILSENABLED_>({ io.m_bThumbnailsEnabled });
            ci.set_cap_values< ICAP_AUTOBRIGHT_>({ io.m_bAutoBright });
            ci.set_cap_values< ICAP_BRIGHTNESS_>({ io.m_Brightness });
            ci.set_cap_values< ICAP_CONTRAST_>({ io.m_Contrast });
            ci.set_cap_values< ICAP_HIGHLIGHT_>({ io.m_Highlight });
            ci.set_cap_values< ICAP_MIRROR_>({ io.m_MirrorValue });
            ci.set_cap_values< ICAP_ORIENTATION_>({ io.m_OrientationValue });
            ci.set_cap_values< ICAP_ROTATION_>({ io.m_RotationValue });
            ci.set_cap_values< ICAP_SHADOW_>({ io.m_ShadowValue });
            ci.set_cap_values< ICAP_XSCALING_>({ io.m_xScaling });
            ci.set_cap_values< ICAP_YSCALING_>({ io.m_yScaling });
            ci.set_cap_values< ICAP_IMAGEDATASET_>(io.m_vImageDataSets);
        }

        void options_base::apply(twain_source& ts, audiblealarms_options& aa)
        {
            auto& ci = ts.get_capability_interface();
            if (aa.m_AlarmVolume != audiblealarms_options::disable_volume)
                ci.set_cap_values< CAP_ALARMVOLUME_>({ aa.m_AlarmVolume });
            for (auto& a : aa.m_vAlarms)
                ci.set_cap_values< CAP_ALARMS_>({ a });
        }

        void options_base::apply(twain_source& ts, deviceevent_options& dopt)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_DEVICEEVENT_>(dopt.m_vDeviceEvents);
        }

        void options_base::apply(twain_source& ts, resolution_options& ro)
        {
            auto& ci = ts.get_capability_interface();
            if (ro.m_xResolution != (std::numeric_limits<double>::min)())
                ci.set_cap_values< ICAP_XRESOLUTION_>({ ro.m_xResolution });
            if (ro.m_yResolution != (std::numeric_limits<double>::min)())
                ci.set_cap_values< ICAP_YRESOLUTION_>({ ro.m_yResolution });
        }

        void options_base::apply(twain_source& ts, color_options& co)
        {
            auto& ci = ts.get_capability_interface();
            if (co.m_Gamma != color_options::disable_gamma)
                ci.set_cap_values< ICAP_GAMMA_>({ co.m_Gamma });
            else
                ci.set_cap_values<ICAP_GAMMA_>({});
            ci.set_cap_values< ICAP_COLORMANAGEMENTENABLED_>({ co.m_bColorManagementEnabled });
            ci.set_cap_values< ICAP_FILTER_>(co.m_vFilterValue);
            ci.set_cap_values< ICAP_ICCPROFILE_>({ co.m_ICCProfileValue });
            ci.set_cap_values< ICAP_PLANARCHUNKY_>({ co.m_PlanarChunkyValue });
        }

        void options_base::apply(twain_source& ts, capnegotiation_options& co)
        {
            if (co.m_bSetExtendedCaps)
            {
                auto& ci = ts.get_capability_interface();
                ci.set_cap_values< CAP_EXTENDEDCAPS_>(co.m_vExtendedCaps);
            }
        }

        void options_base::apply(twain_source& ts, micr_options& mo)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_MICRENABLED_>({ mo.m_bMicrEnabled });
        }

        void options_base::apply(twain_source& ts, imprinter_options& io)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values< CAP_PRINTERENABLED_>({ io.m_bEnable });
            if (io.m_bEnable)
            {
                ci.set_cap_values< CAP_PRINTERINDEX_>({ io.m_printerIndex });
                ci.set_cap_values< CAP_PRINTERCHARROTATION_>({ io.m_charRotation });
                ci.set_cap_values< CAP_PRINTERINDEXLEADCHAR_>({ io.m_printerLeadChar });
                ci.set_cap_values< CAP_PRINTERINDEXMAXVALUE_>({ io.m_printerMaxValue });
                ci.set_cap_values< CAP_PRINTERINDEXNUMDIGITS_>({ io.m_printerNumDigits });
                ci.set_cap_values< CAP_PRINTERINDEXSTEP_>({ io.m_printerIndexStep });
                ci.set_cap_values< CAP_PRINTERMODE_>({ io.m_stringMode });
                ci.set_cap_values< CAP_PRINTERSUFFIX_>({ io.m_suffixString });
                ci.set_cap_values< CAP_PRINTERVERTICALOFFSET_>({ io.m_vertical_offset });
                ci.set_cap_values< CAP_PRINTERFONTSTYLE_>(io.m_fontStyles);
                ci.set_cap_values< CAP_PRINTERSTRING_>(io.m_printerStrings);
                ci.set_cap_values< CAP_PRINTERINDEXTRIGGER_>(io.m_indexTriggers);
            }
        }

        void options_base::apply(twain_source& ts, autoscanning_options& ao)
        {
            auto& ci = ts.get_capability_interface();
            ci.set_cap_values<CAP_AUTOSCAN_>({ ao.m_bAutoScan });
            ci.set_cap_values<CAP_CAMERAENABLED_>({ ao.m_bCameraEnabled });
            ci.set_cap_values<CAP_CAMERASIDE_>({ ao.m_CameraSide });
            ci.set_cap_values<CAP_CAMERAORDER_>(ao.m_vCameraOrder);
            ci.set_cap_values<CAP_MAXBATCHBUFFERS_>({ ao.m_MaxBatchBuffers });
        }
    }
}
