/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2024 Dynarithmic Software.

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
// transfer type (native, buffered, file)

#ifndef DTWAIN_TWAIN_TYPES_HPP
#define DTWAIN_TWAIN_TYPES_HPP

#ifdef DTWAIN_CPP_NOIMPORTLIB
    #include <dtwainx2.h>
#else
    #include <dtwain.h>
#endif
#include <ostream>

namespace dynarithmic
{
    namespace twain
    {
        /**
        Constants describing the various TWAIN Data Source Manager (DSM) types to use when starting a TWAIN session.  
        */
        enum class dsm_type
        {
            legacy_dsm = DTWAIN_TWAINDSM_LEGACY, /**< Use legacy version of Data Source Manager (TWAIN_32.DLL) */
            version2_dsm = DTWAIN_TWAINDSM_VERSION2, /**< Use version 2.x of Data Source Manager (TWAINDSM.DLL) */
            default_dsm = DTWAIN_TWAINDSM_LATESTVERSION /**< Use latest DSM version installed */
        };

        /**
        Constants describing the customization of the search path for a TWAIN Data Source Manager (DSM) TWAIN_32.DLL or TWAINDSM.DLL
        */
        enum class dsm_search_order
        {
            search_wso = DTWAIN_TWAINDSMSEARCH_WSO, /**< Windows -> System -> OS Path */
            search_wos = DTWAIN_TWAINDSMSEARCH_WOS, /**< Windows -> OS Path -> System */
            search_swo = DTWAIN_TWAINDSMSEARCH_SWO, /**< System -> Windows -> OS Path */
            search_sow = DTWAIN_TWAINDSMSEARCH_SOW, /**< System -> OS Path -> Windows */
            search_ows = DTWAIN_TWAINDSMSEARCH_OWS, /**< OS Path -> Windows -> System */
            search_osw = DTWAIN_TWAINDSMSEARCH_OSW, /**< OS Path -> System -> Windows */
            search_w = DTWAIN_TWAINDSMSEARCH_W, /**< Windows */
            search_s = DTWAIN_TWAINDSMSEARCH_S, /**< System */
            search_o = DTWAIN_TWAINDSMSEARCH_O, /**< OS Path */
            search_ws = DTWAIN_TWAINDSMSEARCH_WS,/**< Window -> System */
            search_wo = DTWAIN_TWAINDSMSEARCH_WO,/**< Window -> OS Path */
            search_sw = DTWAIN_TWAINDSMSEARCH_SW,/**< System -> Windows */
            search_so = DTWAIN_TWAINDSMSEARCH_SO,/**< System -> OS Path */
            search_ow = DTWAIN_TWAINDSMSEARCH_OW,/**< OS Path -> Windows */
            search_os = DTWAIN_TWAINDSMSEARCH_OS/**< OS Path -> System */
        };

        enum class feedermode_value
        {
            feeder,
            feeder_flatbed
        };

        /**
        Constants describing the manual duplex mode options.  Manual duplex is used to implement duplexing for non-duplex devices
        */
        enum class manualduplexmode_value
        {
            none = 1000,        /**< no manual duplexing will be done */
            manual_faceuptopfeed = DTWAIN_MANDUP_FACEUPTOPPAGE,  /**< face up, feeds from top */
            manual_faceupbottomfeed = DTWAIN_MANDUP_FACEUPBOTTOMPAGE,  /**< face up, feeds from bottom */
            manual_facedowntopfeed = DTWAIN_MANDUP_FACEDOWNTOPPAGE,  /**< face down, feeds from top */
            manual_facedownbottomfeed = DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE  /**< face down, feeds from bottom */
        };

        enum class transfer_type
        {
            image_native = 0,
            image_buffered = 1,
            file_using_native = 2,
            file_using_buffered = 3,
            file_using_source = 4,
            default_val = 1000
        };

        enum class twaintype_index
        {
            tw_bool = 0,
            tw_int8,
            tw_int16,
            tw_int32,
            tw_uint8,
            tw_uint16,
            tw_uint32,
            tw_str,
            tw_fix32,
            tw_frame,
            tw_unknown,
            tw_invalid
        };

        // image color types
        struct color_value
        {
            static constexpr TW_UINT16 bw = TWPT_BW;
            static constexpr TW_UINT16 gray = TWPT_GRAY;
            static constexpr TW_UINT16 rgb = TWPT_RGB;
            static constexpr TW_UINT16 palette = TWPT_PALETTE;
            static constexpr TW_UINT16 cmy = TWPT_CMY;
            static constexpr TW_UINT16 cmyk = TWPT_CMYK;
            static constexpr TW_UINT16 yuv = TWPT_YUV;
            static constexpr TW_UINT16 yuvk = TWPT_YUVK;
            static constexpr TW_UINT16 CIEXYZ = TWPT_CIEXYZ;
            static constexpr TW_UINT16 lab = TWPT_LAB;
            static constexpr TW_UINT16 srgb = TWPT_SRGB;
            static constexpr TW_UINT16 srgb64 = TWPT_SRGB64;
            static constexpr TW_UINT16 bgr = TWPT_BGR;
            static constexpr TW_UINT16 cielab = TWPT_CIELAB;
            static constexpr TW_UINT16 cieluv = TWPT_CIELUV;
            static constexpr TW_UINT16 ycbcr = TWPT_YCBCR;
            static constexpr TW_UINT16 infrared = TWPT_INFRARED;
            static constexpr TW_UINT16 default_color = DTWAIN_PT_DEFAULT;
            typedef TW_UINT16 value_type;
        };

        inline std::ostream& operator << (std::ostream& os, const color_value& ct)
        {
            os << ct;
            return os;
        }

        enum class high_level_cap : int32_t
        {
            duplexenabled,
            feederenabled,
            feederloaded,
            paperdetectable,
            filmscan,
            uionly,
            indicators,
            customdata,
            orientation,
            rotation,
            lightsource,
            brightness,
            contrast,
            gamma,
            highlight,
            threshold,
            deviceonline,
            jobcontrol,
            automaticdeskew,
            automaticrotate,
            autobright,
            overscan,
            shadow,
            printer,
            printerstring,
            printercharrotation,
            printersuffix,
            printerindex, 
            printerindexstep,
            printerindexmaxvalue, 
            printerindexnumdigits,
            printerindexleadchar, 
            printermode, 
            printerverticaloffset,
            printerindextrigger,
            printerstringpreview
        };
    }
}   
#endif
