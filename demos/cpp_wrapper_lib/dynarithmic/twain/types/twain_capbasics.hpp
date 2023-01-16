/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2022 Dynarithmic Software.

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
// This template is a generic TWAIN capability descriptor

#ifndef DTWAIN_TWAIN_CAPBASICS_HPP
#define DTWAIN_TWAIN_CAPBASICS_HPP

#include <dynarithmic/twain/types/twain_range.hpp>
#include <dynarithmic/twain/types/twain_array.hpp>

namespace dynarithmic
{
    namespace twain
    {
        template <typename CapType, typename CTraits, int dataType, bool isRangeExpandable, bool cacheable, bool isExtendedImage, int val = 0>
        struct cap_basic_type
        {
            typedef CapType value_type;
            static const int cap_value = val;
            static const int data_type = dataType;
            static const bool is_range_expandable = isRangeExpandable;
            static const bool get_all_cacheable = cacheable;
            static const bool is_extendedimage = isExtendedImage;
            static CTraits copy_traits;
        };

        template <int CapVal>
        struct cap_value_translate
        {
            static auto get_cap_type()
            {
                return 0;
            }
        };

        #define CAPCUSTOM_LONG      (CAP_CUSTOMBASE + 1)
        #define CAPCUSTOM_FLOAT     (CAP_CUSTOMBASE + 2)
        #define CAPCUSTOM_STRING    (CAP_CUSTOMBASE + 3)
        #define CAPCUSTOM_FRAME     (CAP_CUSTOMBASE + 4)


        #define DECLARE_TWAIN_TWBOOL_TYPE(x, c, r) struct x##_:cap_basic_type<bool, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_TWINT8_TYPE(x, c, r) struct x##_:cap_basic_type<int8_t, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_TWINT16_TYPE(x, c, r) struct x##_:cap_basic_type<int16_t, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_TWINT32_TYPE(x, c, r) struct x##_:cap_basic_type<int32_t, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_TWUINT8_TYPE(x, c, r) struct x##_:cap_basic_type<uint8_t, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_TWUINT16_TYPE(x, c, r) struct x##_:cap_basic_type<uint16_t, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_TWUINT32_TYPE(x, c, r) struct x##_:cap_basic_type<uint32_t, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_FLOAT_TYPE(x, c, r) struct x##_:cap_basic_type<double, twain_array_copy_traits, DTWAIN_ARRAYFLOAT, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_STRING_TYPE(x, c, r) struct x##_:cap_basic_type<std::string, twain_array_copy_traits, DTWAIN_ARRAYSTRING, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};
        #define DECLARE_TWAIN_FRAME_TYPE(x, c, r) struct x##_:cap_basic_type<twain_frame<>, twain_array_copy_traits, DTWAIN_ARRAYFRAME, r, c,false ,x>{}; template <> struct cap_value_translate<x>{static auto get_cap_type(){return x##_();}};

        #define DECLARE_TWAIN_TWBOOL_TYPEEXT(x, c, r) struct x##_extended_:cap_basic_type<bool, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c,true,x>{};             
        #define DECLARE_TWAIN_TWUINT32_TYPEEXT(x, c, r) struct x##_extended_:cap_basic_type<uint32_t, twain_array_copy_traits, DTWAIN_ARRAYLONG, r, c, true,x>{};      
        #define DECLARE_TWAIN_STRING_TYPEEXT(x, c, r) struct x##_extended_:cap_basic_type<std::string, twain_array_copy_traits, DTWAIN_ARRAYSTRING, r, c, true ,x>{};  
        #define DECLARE_TWAIN_FRAME_TYPEEXT(x, c, r) struct x##_extended_:cap_basic_type<twain_frame<>, twain_array_copy_traits, DTWAIN_ARRAYFRAME, r, c,true ,x>{};   
        #define DECLARE_TWAIN_HANDLE_TYPEEXT(x, c, r) struct x##_extended_:cap_basic_type<void*, twain_array_copy_traits, DTWAIN_ARRAYHANDLE, r, c,true ,x>{};         

        DECLARE_TWAIN_TWUINT16_TYPE(CAPCUSTOM_LONG, true, false)
        DECLARE_TWAIN_FLOAT_TYPE(CAPCUSTOM_FLOAT, true, false)
        DECLARE_TWAIN_FRAME_TYPE(CAPCUSTOM_FRAME, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAPCUSTOM_STRING, true, false)

        // These are all of the capabilities we know of, as of TWAIN specification 2.4
        DECLARE_TWAIN_TWUINT16_TYPE(ACAP_XFERMECH, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_ALARMS, true, false)
        DECLARE_TWAIN_TWINT32_TYPE(CAP_ALARMVOLUME, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_AUTOFEED, true, false)
        DECLARE_TWAIN_TWINT32_TYPE(CAP_AUTOMATICCAPTURE, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_AUTOMATICSENSEMEDIUM, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_AUTOSCAN, true, false)
        DECLARE_TWAIN_TWINT32_TYPE(CAP_BATTERYMINUTES, true, false)
        DECLARE_TWAIN_TWINT16_TYPE(CAP_BATTERYPERCENTAGE, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_CAMERAENABLED, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_CAMERAORDER, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_CAMERAPREVIEWUI, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_CAMERASIDE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_CLEARBUFFERS, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_CLEARPAGE, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_CUSTOMDSDATA, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_DEVICEEVENT, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_DEVICEONLINE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_DOUBLEFEEDDETECTION, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_DOUBLEFEEDDETECTIONRESPONSE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_DOUBLEFEEDDETECTIONSENSITIVITY, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_DUPLEX, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_DUPLEXENABLED, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_ENABLEDSUIONLY, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_ENDORSER, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_EXTENDEDCAPS, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_FEEDERALIGNMENT, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_FEEDERENABLED, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_FEEDERLOADED, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_FEEDERORDER, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_FEEDERPOCKET, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_FEEDERPREP, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_FEEDPAGE, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_INDICATORS, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_INDICATORSMODE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_JOBCONTROL, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_LANGUAGE, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_MAXBATCHBUFFERS, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_MICRENABLED, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_PAPERDETECTABLE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_PAPERHANDLING, true, false)
        DECLARE_TWAIN_TWINT32_TYPE(CAP_POWERSAVETIME, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_POWERSUPPLY, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_PRINTER, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_PRINTERENABLED, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_PRINTERCHARROTATION, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_PRINTERFONTSTYLE, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_PRINTERINDEX, true, true)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_PRINTERINDEXMAXVALUE, true, true)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_PRINTERINDEXNUMDIGITS, true, true)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_PRINTERINDEXSTEP, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_PRINTERINDEXTRIGGER, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_PRINTERMODE, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_REACQUIREALLOWED, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_REWINDPAGE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_SEGMENTED, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_SHEETCOUNT, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_SUPPORTEDCAPS, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(CAP_SUPPORTEDCAPSSEGMENTUNIQUE, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(CAP_SUPPORTEDDATS, true, false)
        DECLARE_TWAIN_TWINT32_TYPE(CAP_TIMEBEFOREFIRSTCAPTURE, true, true)
        DECLARE_TWAIN_TWINT32_TYPE(CAP_TIMEBETWEENCAPTURES, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_THUMBNAILSENABLED, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(CAP_UICONTROLLABLE, true, false)
        DECLARE_TWAIN_TWINT16_TYPE(CAP_XFERCOUNT, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_AUTOBRIGHT, true, false)
        DECLARE_TWAIN_TWINT32_TYPE(ICAP_AUTODISCARDBLANKPAGES, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_AUTOMATICBORDERDETECTION, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_AUTOMATICCOLORENABLED, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_AUTOMATICCROPUSESFRAME, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_AUTOMATICDESKEW, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_AUTOMATICLENGTHDETECTION, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_AUTOMATICROTATE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_AUTOSIZE, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_BARCODEDETECTIONENABLED, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(ICAP_BARCODEMAXRETRIES, true, true)
        DECLARE_TWAIN_TWUINT32_TYPE(ICAP_BARCODEMAXSEARCHPRIORITIES, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_BARCODESEARCHMODE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_BARCODESEARCHPRIORITIES, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(ICAP_BARCODETIMEOUT, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_BITDEPTH, false, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_BITDEPTHREDUCTION, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_BITORDER, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_BITORDERCODES, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_CCITTKFACTOR, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_COLORMANAGEMENTENABLED, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_COMPRESSION, true, false)
        DECLARE_TWAIN_TWUINT8_TYPE(ICAP_CUSTHALFTONE, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_EXTIMAGEINFO, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_FEEDERTYPE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_FILMTYPE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_FILTER, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_FLASHUSED, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_FLASHUSED2, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_FLIPROTATION, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_ICCPROFILE, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(ICAP_IMAGEDATASET, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_IMAGEFILEFORMAT, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_IMAGEFILTER, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_IMAGEMERGE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_JPEGPIXELTYPE, true, false)
        DECLARE_TWAIN_TWINT16_TYPE(ICAP_JPEGQUALITY, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_JPEGSUBSAMPLING, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_LAMPSTATE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_LIGHTPATH, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_LIGHTSOURCE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_MAXFRAMES, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_MIRROR, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_NOISEFILTER, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_ORIENTATION, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_OVERSCAN, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_PATCHCODEDETECTIONENABLED, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(ICAP_PATCHCODEMAXRETRIES, true, true)
        DECLARE_TWAIN_TWUINT32_TYPE(ICAP_PATCHCODEMAXSEARCHPRIORITIES, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_PATCHCODESEARCHMODE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_PATCHCODESEARCHPRIORITIES, true, false)
        DECLARE_TWAIN_TWUINT32_TYPE(ICAP_PATCHCODETIMEOUT, true, true)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_PIXELFLAVOR, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_PIXELFLAVORCODES, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_PIXELTYPE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_PLANARCHUNKY, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_SUPPORTEDBARCODETYPES, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_SUPPORTEDEXTIMAGEINFO, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_SUPPORTEDPATCHCODETYPES, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_SUPPORTEDSIZES, true, false)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_TILES, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_TIMEFILL, true, true)
        DECLARE_TWAIN_TWBOOL_TYPE(ICAP_UNDEFINEDIMAGESIZE, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_UNITS, true, false)
        DECLARE_TWAIN_TWUINT16_TYPE(ICAP_XFERMECH, true, false)
        DECLARE_TWAIN_TWINT16_TYPE(ICAP_ZOOMFACTOR, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(CAP_DOUBLEFEEDDETECTIONLENGTH, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(CAP_PRINTERVERTICALOFFSET, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_BRIGHTNESS, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_CONTRAST, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_EXPOSURETIME, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_GAMMA, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_HIGHLIGHT, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_IMAGEMERGEHEIGHTTHRESHOLD, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_MINIMUMHEIGHT, true, false)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_MINIMUMWIDTH, true, false)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_PHYSICALHEIGHT, true, false)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_PHYSICALWIDTH, true, false)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_ROTATION, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_SHADOW, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_THRESHOLD, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_XNATIVERESOLUTION, true, false)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_XRESOLUTION, false, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_XSCALING, true, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_YNATIVERESOLUTION, true, false)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_YRESOLUTION, false, true)
        DECLARE_TWAIN_FLOAT_TYPE(ICAP_YSCALING, true, true)

        DECLARE_TWAIN_STRING_TYPE(CAP_AUTHOR, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_CAPTION, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_CUSTOMINTERFACEGUID, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_DEVICETIMEDATE, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_PRINTERINDEXLEADCHAR, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_PRINTERSTRING, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_PRINTERSTRINGPREVIEW, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_PRINTERSUFFIX, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_SERIALNUMBER, true, false)
        DECLARE_TWAIN_STRING_TYPE(CAP_TIMEDATE, true, false)
        DECLARE_TWAIN_STRING_TYPE(ICAP_HALFTONES, true, false)
        DECLARE_TWAIN_FRAME_TYPE(ICAP_FRAMES, false, false)

        // The Extended image attribute capabilities
        DECLARE_TWAIN_FRAME_TYPEEXT(TWEI_FRAME,true, false)
        DECLARE_TWAIN_HANDLE_TYPEEXT(TWEI_BARCODETEXT,true, false)
        DECLARE_TWAIN_HANDLE_TYPEEXT(TWEI_MAGDATA,true, false)
        DECLARE_TWAIN_HANDLE_TYPEEXT(TWEI_TWAINDIRECTMETADATA,true, false)
        DECLARE_TWAIN_STRING_TYPEEXT(TWEI_BOOKNAME,true, false)
        DECLARE_TWAIN_STRING_TYPEEXT(TWEI_CAMERA,true, false)
        DECLARE_TWAIN_STRING_TYPEEXT(TWEI_ENDORSEDTEXT,true, false)
        DECLARE_TWAIN_STRING_TYPEEXT(TWEI_FILESYSTEMSOURCE,true, false)
        DECLARE_TWAIN_STRING_TYPEEXT(TWEI_FORMTEMPLATEMATCH,true, false)
        DECLARE_TWAIN_STRING_TYPEEXT(TWEI_ICCPROFILE,true, false)
        DECLARE_TWAIN_STRING_TYPEEXT(TWEI_PRINTERTEXT,true, false)
        DECLARE_TWAIN_TWBOOL_TYPEEXT(TWEI_IMAGEMERGED,true, false)
        DECLARE_TWAIN_TWBOOL_TYPEEXT(TWEI_LASTSEGMENT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BARCODECONFIDENCE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BARCODECOUNT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BARCODEROTATION,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BARCODETEXTLENGTH,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BARCODETYPE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BARCODEX,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BARCODEY,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_BLACKSPECKLESREMOVED,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_CHAPTERNUMBER,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEBLACKCOUNTNEW,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEBLACKCOUNTOLD,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEBLACKRLMAX,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEBLACKRLMIN,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADECOUNT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEHEIGHT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADELEFT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADESIZE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADETOP,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEWHITECOUNTNEW,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEWHITECOUNTOLD,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEWHITERLAVE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEWHITERLMAX,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEWHITERLMIN,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESHADEWIDTH,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DESKEWSTATUS,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_DOCUMENTNUMBER,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_FORMCONFIDENCE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_FORMHORZDOCOFFSET,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_FORMTEMPLATEPAGEMATCH,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_FORMVERTDOCOFFSET,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_FRAMENUMBER,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_HORZLINECOUNT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_HORZLINELENGTH,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_HORZLINETHICKNESS,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_HORZLINEXCOORD,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_HORZLINEYCOORD,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_MAGDATALENGTH,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_MAGTYPE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_PAGENUMBER,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_PAGESIDE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_PAPERCOUNT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_PATCHCODE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_PIXELFLAVOR,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SEGMENTNUMBER,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWCONFIDENCE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWFINALANGLE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWORIGINALANGLE,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWX1,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWX2,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWX3,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWX4,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWY1,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWY2,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWY3,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SKEWWINDOWY4,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_SPECKLESREMOVED,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_VERTLINECOUNT,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_VERTLINELENGTH,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_VERTLINETHICKNESS,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_VERTLINEXCOORD,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_VERTLINEYCOORD,true, false)
        DECLARE_TWAIN_TWUINT32_TYPEEXT(TWEI_WHITESPECKLESREMOVED,true, false)

        struct capability_type
        {
            // These are aliases for all of the types supported by each capability
            typedef CAP_ALARMS_::value_type                             alarms_type;  
            typedef CAP_ALARMVOLUME_::value_type                        alarmvolume_type;  
            typedef ACAP_XFERMECH_::value_type                          audio_xfermech_type;  
            typedef CAP_AUTHOR_::value_type                             author_type;  
            typedef ICAP_AUTOBRIGHT_::value_type                        autobright_type;  
            typedef ICAP_AUTODISCARDBLANKPAGES_::value_type             autodiscardblankpages_type;  
            typedef CAP_AUTOFEED_::value_type                           autofeed_type;  
            typedef ICAP_AUTOMATICBORDERDETECTION_::value_type          automaticborderdetection_type;  
            typedef CAP_AUTOMATICCAPTURE_::value_type                   automaticcapture_type;  
            typedef ICAP_AUTOMATICCOLORENABLED_::value_type             automaticcolorenabled_type;  
            typedef ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_::value_type   automaticcolornoncolorpixeltype_type;  
            typedef ICAP_AUTOMATICCROPUSESFRAME_::value_type            automaticcropusesframe_type;  
            typedef ICAP_AUTOMATICDESKEW_::value_type                   automaticdeskew_type;  
            typedef ICAP_AUTOMATICLENGTHDETECTION_::value_type          automaticlengthdetection_type;  
            typedef ICAP_AUTOMATICROTATE_::value_type                   automaticrotate_type;  
            typedef CAP_AUTOMATICSENSEMEDIUM_::value_type               automaticsensemedium_type;  
            typedef CAP_AUTOSCAN_::value_type                           autoscan_type;  
            typedef ICAP_AUTOSIZE_::value_type                          autosize_type;  
            typedef ICAP_BARCODEDETECTIONENABLED_::value_type           barcodedetectionenabled_type;  
            typedef ICAP_BARCODEMAXRETRIES_::value_type                 barcodemaxretries_type;  
            typedef ICAP_BARCODEMAXSEARCHPRIORITIES_::value_type        barcodemaxsearchpriorities_type;  
            typedef ICAP_BARCODESEARCHMODE_::value_type                 barcodesearchmode_type;  
            typedef ICAP_BARCODESEARCHPRIORITIES_::value_type           barcodesearchpriorities_type;  
            typedef ICAP_BARCODETIMEOUT_::value_type                    barcodetimeout_type;  
            typedef CAP_BATTERYMINUTES_::value_type                     batteryminutes_type;  
            typedef CAP_BATTERYPERCENTAGE_::value_type                  batterypercentage_type;  
            typedef ICAP_BITDEPTH_::value_type                          bitdepth_type;  
            typedef ICAP_BITDEPTHREDUCTION_::value_type                 bitdepthreduction_type;  
            typedef ICAP_BITORDER_::value_type                          bitorder_type;  
            typedef ICAP_BITORDERCODES_::value_type                     bitordercodes_type;  
            typedef ICAP_BRIGHTNESS_::value_type                        brightness_type;  
            typedef CAP_CAMERAENABLED_::value_type                      cameraenabled_type;  
            typedef CAP_CAMERAORDER_::value_type                        cameraorder_type;  
            typedef CAP_CAMERAPREVIEWUI_::value_type                    camerapreviewui_type;  
            typedef CAP_CAMERASIDE_::value_type                         cameraside_type;  
            typedef CAP_CAPTION_::value_type                            caption_type;  
            typedef ICAP_CCITTKFACTOR_::value_type                      ccittkfactor_type;  
            typedef CAP_CLEARPAGE_::value_type                          clearpage_type;  
            typedef ICAP_COLORMANAGEMENTENABLED_::value_type            colormanagementenabled_type;  
            typedef ICAP_COMPRESSION_::value_type                       compression_type;  
            typedef ICAP_CONTRAST_::value_type                          contrast_type;  
            typedef ICAP_CUSTHALFTONE_::value_type                      custhalftone_type;  
            typedef CAP_CUSTOMDSDATA_::value_type                       customdsdata_type;  
            typedef CAP_CUSTOMINTERFACEGUID_::value_type                custominterfaceguid_type;  
            typedef CAP_DEVICEEVENT_::value_type                        deviceevent_type;  
            typedef CAP_DEVICEONLINE_::value_type                       deviceonline_type;  
            typedef CAP_DEVICETIMEDATE_::value_type                     devicetimedate_type;  
            typedef CAP_DOUBLEFEEDDETECTION_::value_type                doublefeeddetection_type;  
            typedef CAP_DOUBLEFEEDDETECTIONLENGTH_::value_type          doublefeeddetectionlength_type;  
            typedef CAP_DOUBLEFEEDDETECTIONRESPONSE_::value_type        doublefeeddetectionresponse_type;  
            typedef CAP_DOUBLEFEEDDETECTIONSENSITIVITY_::value_type     doublefeeddetectionsensitivity_type;  
            typedef CAP_DUPLEX_::value_type                             duplex_type;  
            typedef CAP_DUPLEXENABLED_::value_type                      duplexenabled_type;  
            typedef CAP_ENABLEDSUIONLY_::value_type                     enabledsuionly_type;  
            typedef CAP_ENDORSER_::value_type                           endorser_type;  
            typedef ICAP_EXPOSURETIME_::value_type                      exposuretime_type;  
            typedef CAP_EXTENDEDCAPS_::value_type                       extendedcaps_type;  
            typedef ICAP_EXTIMAGEINFO_::value_type                      extimageinfo_type;  
            typedef CAP_FEEDERALIGNMENT_::value_type                    feederalignment_type;  
            typedef CAP_FEEDERENABLED_::value_type                      feederenabled_type;  
            typedef CAP_FEEDERLOADED_::value_type                       feederloaded_type;  
            typedef CAP_FEEDERORDER_::value_type                        feederorder_type;  
            typedef CAP_FEEDERPOCKET_::value_type                       feederpocket_type;  
            typedef CAP_FEEDERPREP_::value_type                         feederprep_type;  
            typedef ICAP_FEEDERTYPE_::value_type                        feedertype_type;  
            typedef CAP_FEEDPAGE_::value_type                           feedpage_type;  
            typedef ICAP_FILMTYPE_::value_type                          filmtype_type;  
            typedef ICAP_FILTER_::value_type                            filter_type;  
            typedef ICAP_FLASHUSED2_::value_type                        flashused2_type;  
            typedef ICAP_FLIPROTATION_::value_type                      fliprotation_type;  
            typedef ICAP_FRAMES_::value_type                            frames_type;  
            typedef ICAP_GAMMA_::value_type                             gamma_type;  
            typedef ICAP_HALFTONES_::value_type                         halftones_type;  
            typedef ICAP_HIGHLIGHT_::value_type                         highlight_type;  
            typedef ICAP_ICCPROFILE_::value_type                        iccprofile_type;  
            typedef ICAP_XFERMECH_::value_type                          image_xfermech_type;  
            typedef ICAP_IMAGEDATASET_::value_type                      imagedataset_type;  
            typedef ICAP_IMAGEFILEFORMAT_::value_type                   imagefileformat_type;  
            typedef ICAP_IMAGEFILTER_::value_type                       imagefilter_type;  
            typedef ICAP_IMAGEMERGE_::value_type                        imagemerge_type;  
            typedef ICAP_IMAGEMERGEHEIGHTTHRESHOLD_::value_type         imagemergeheightthreshold_type;  
            typedef CAP_INDICATORS_::value_type                         indicators_type;  
            typedef CAP_INDICATORSMODE_::value_type                     indicatorsmode_type;  
            typedef CAP_JOBCONTROL_::value_type                         jobcontrol_type;  
            typedef ICAP_JPEGPIXELTYPE_::value_type                     jpegpixeltype_type;  
            typedef ICAP_JPEGQUALITY_::value_type                       jpegquality_type;  
            typedef ICAP_JPEGSUBSAMPLING_::value_type                   jpegsubsampling_type;  
            typedef ICAP_LAMPSTATE_::value_type                         lampstate_type;  
            typedef CAP_LANGUAGE_::value_type                           language_type;  
            typedef ICAP_LIGHTPATH_::value_type                         lightpath_type;  
            typedef ICAP_LIGHTSOURCE_::value_type                       lightsource_type;  
            typedef CAP_MAXBATCHBUFFERS_::value_type                    maxbatchbuffers_type;  
            typedef ICAP_MAXFRAMES_::value_type                         maxframes_type;  
            typedef CAP_MICRENABLED_::value_type                        micrenabled_type;  
            typedef ICAP_MINIMUMHEIGHT_::value_type                     minimumheight_type;  
            typedef ICAP_MINIMUMWIDTH_::value_type                      minimumwidth_type;  
            typedef ICAP_MIRROR_::value_type                            mirror_type;  
            typedef ICAP_NOISEFILTER_::value_type                       noisefilter_type;  
            typedef ICAP_ORIENTATION_::value_type                       orientation_type;  
            typedef ICAP_OVERSCAN_::value_type                          overscan_type;  
            typedef CAP_PAPERDETECTABLE_::value_type                    paperdetectable_type;  
            typedef CAP_PAPERHANDLING_::value_type                      paperhandling_type;  
            typedef ICAP_PATCHCODEDETECTIONENABLED_::value_type         patchcodedetectionenabled_type;  
            typedef ICAP_PATCHCODEMAXRETRIES_::value_type               patchcodemaxretries_type;  
            typedef ICAP_PATCHCODEMAXSEARCHPRIORITIES_::value_type      patchcodemaxsearchpriorities_type;  
            typedef ICAP_PATCHCODESEARCHMODE_::value_type               patchcodesearchmode_type;  
            typedef ICAP_PATCHCODESEARCHPRIORITIES_::value_type         patchcodesearchpriorities_type;  
            typedef ICAP_PATCHCODETIMEOUT_::value_type                  patchcodetimeout_type;  
            typedef ICAP_PHYSICALHEIGHT_::value_type                    physicalheight_type;  
            typedef ICAP_PHYSICALWIDTH_::value_type                     physicalwidth_type;  
            typedef ICAP_PIXELFLAVOR_::value_type                       pixelflavor_type;  
            typedef ICAP_PIXELFLAVORCODES_::value_type                  pixelflavorcodes_type;  
            typedef ICAP_PIXELTYPE_::value_type                         pixeltype_type;  
            typedef ICAP_PLANARCHUNKY_::value_type                      planarchunky_type;  
            typedef CAP_POWERSAVETIME_::value_type                      powersavetime_type;  
            typedef CAP_POWERSUPPLY_::value_type                        powersupply_type;  
            typedef CAP_PRINTER_::value_type                            printer_type;  
            typedef CAP_PRINTERCHARROTATION_::value_type                printercharrotation_type;  
            typedef CAP_PRINTERENABLED_::value_type                     printerenabled_type;  
            typedef CAP_PRINTERFONTSTYLE_::value_type                   printerfontstyle_type;  
            typedef CAP_PRINTERINDEX_::value_type                       printerindex_type;  
            typedef CAP_PRINTERINDEXLEADCHAR_::value_type               printerindexleadchar_type;  
            typedef CAP_PRINTERINDEXMAXVALUE_::value_type               printerindexmaxvalue_type;  
            typedef CAP_PRINTERINDEXNUMDIGITS_::value_type              printerindexnumdigits_type;  
            typedef CAP_PRINTERINDEXSTEP_::value_type                   printerindexstep_type;  
            typedef CAP_PRINTERINDEXTRIGGER_::value_type                printerindextrigger_type;  
            typedef CAP_PRINTERMODE_::value_type                        printermode_type;  
            typedef CAP_PRINTERSTRING_::value_type                      printerstring_type;  
            typedef CAP_PRINTERSTRINGPREVIEW_::value_type               printerstringpreview_type;  
            typedef CAP_PRINTERSUFFIX_::value_type                      printersuffix_type;  
            typedef CAP_PRINTERVERTICALOFFSET_::value_type              printerverticaloffset_type;  
            typedef CAP_REACQUIREALLOWED_::value_type                   reacquireallowed_type;  
            typedef CAP_REWINDPAGE_::value_type                         rewindpage_type;  
            typedef ICAP_ROTATION_::value_type                          rotation_type;  
            typedef CAP_SEGMENTED_::value_type                          segmented_type;  
            typedef CAP_SERIALNUMBER_::value_type                       serialnumber_type;  
            typedef ICAP_SHADOW_::value_type                            shadow_type;  
            typedef CAP_SHEETCOUNT_::value_type                         sheetcount_type;  
            typedef ICAP_SUPPORTEDBARCODETYPES_::value_type             supportedbarcodetypes_type;  
            typedef CAP_SUPPORTEDCAPS_::value_type                      supportedcaps_type;  
            typedef CAP_SUPPORTEDCAPSSEGMENTUNIQUE_::value_type         supportedcapssegmentunique_type;  
            typedef CAP_SUPPORTEDDATS_::value_type                      supporteddats_type;  
            typedef ICAP_SUPPORTEDEXTIMAGEINFO_::value_type             supportedextimageinfo_type;  
            typedef ICAP_SUPPORTEDPATCHCODETYPES_::value_type           supportedpatchcodetypes_type;  
            typedef ICAP_SUPPORTEDSIZES_::value_type                    supportedsizes_type;  
            typedef ICAP_THRESHOLD_::value_type                         threshold_type;  
            typedef CAP_THUMBNAILSENABLED_::value_type                  thumbnailsenabled_type;  
            typedef ICAP_TILES_::value_type                             tiles_type;  
            typedef CAP_TIMEBEFOREFIRSTCAPTURE_::value_type             timebeforefirstcapture_type;  
            typedef CAP_TIMEBETWEENCAPTURES_::value_type                timebetweencaptures_type;  
            typedef CAP_TIMEDATE_::value_type                           timedate_type;  
            typedef ICAP_TIMEFILL_::value_type                          timefill_type;  
            typedef CAP_UICONTROLLABLE_::value_type                     uicontrollable_type;  
            typedef ICAP_UNDEFINEDIMAGESIZE_::value_type                undefinedimagesize_type;  
            typedef ICAP_UNITS_::value_type                             units_type;  
            typedef CAP_XFERCOUNT_::value_type                          xfercount_type;  
            typedef ICAP_XNATIVERESOLUTION_::value_type                 xnativeresolution_type;  
            typedef ICAP_XRESOLUTION_::value_type                       xresolution_type;  
            typedef xresolution_type                                    resolution_type;  
            typedef ICAP_XSCALING_::value_type                          xscaling_type;  
            typedef ICAP_YNATIVERESOLUTION_::value_type                 ynativeresolution_type;  
            typedef ICAP_YRESOLUTION_::value_type                       yresolution_type;  
            typedef ICAP_YSCALING_::value_type                          yscaling_type;  
            typedef ICAP_ZOOMFACTOR_::value_type                        zoomfactor_type;  
                                                                                
            // extended image information types.
            typedef TWEI_BARCODECONFIDENCE_extended_::value_type        ext_barcodeconfidence_type;  
            typedef TWEI_BARCODECOUNT_extended_::value_type             ext_barcodecount_type;  
            typedef TWEI_BARCODEROTATION_extended_::value_type          ext_barcoderotation_type;  
            typedef TWEI_BARCODETEXT_extended_::value_type              ext_barcodetext_type;  
            typedef TWEI_BARCODETEXTLENGTH_extended_::value_type        ext_barcodetextlength_type;  
            typedef TWEI_BARCODETYPE_extended_::value_type              ext_barcodetype_type;  
            typedef TWEI_BARCODEX_extended_::value_type                 ext_barcodex_type;  
            typedef TWEI_BARCODEY_extended_::value_type                 ext_barcodey_type;  
            typedef TWEI_BLACKSPECKLESREMOVED_extended_::value_type     ext_blackspecklesremoved_type;  
            typedef TWEI_BOOKNAME_extended_::value_type                 ext_bookname_type;  
            typedef TWEI_CAMERA_extended_::value_type                   ext_camera_type;  
            typedef TWEI_CHAPTERNUMBER_extended_::value_type            ext_chapternumber_type;  
            typedef TWEI_DESHADEBLACKCOUNTNEW_extended_::value_type     ext_deshadeblackcountnew_type;  
            typedef TWEI_DESHADEBLACKCOUNTOLD_extended_::value_type     ext_deshadeblackcountold_type;  
            typedef TWEI_DESHADEBLACKRLMAX_extended_::value_type        ext_deshadeblackrlmax_type;  
            typedef TWEI_DESHADEBLACKRLMIN_extended_::value_type        ext_deshadeblackrlmin_type;  
            typedef TWEI_DESHADECOUNT_extended_::value_type             ext_deshadecount_type;  
            typedef TWEI_DESHADEHEIGHT_extended_::value_type            ext_deshadeheight_type;  
            typedef TWEI_DESHADELEFT_extended_::value_type              ext_deshadeleft_type;  
            typedef TWEI_DESHADESIZE_extended_::value_type              ext_deshadesize_type;  
            typedef TWEI_DESHADETOP_extended_::value_type               ext_deshadetop_type;  
            typedef TWEI_DESHADEWHITECOUNTNEW_extended_::value_type     ext_deshadewhitecountnew_type;  
            typedef TWEI_DESHADEWHITECOUNTOLD_extended_::value_type     ext_deshadewhitecountold_type;  
            typedef TWEI_DESHADEWHITERLAVE_extended_::value_type        ext_deshadewhiterlave_type;  
            typedef TWEI_DESHADEWHITERLMAX_extended_::value_type        ext_deshadewhiterlmax_type;  
            typedef TWEI_DESHADEWHITERLMIN_extended_::value_type        ext_deshadewhiterlmin_type;  
            typedef TWEI_DESHADEWIDTH_extended_::value_type             ext_deshadewidth_type;  
            typedef TWEI_DESKEWSTATUS_extended_::value_type             ext_deskewstatus_type;  
            typedef TWEI_DOCUMENTNUMBER_extended_::value_type           ext_documentnumber_type;  
            typedef TWEI_ENDORSEDTEXT_extended_::value_type             ext_endorsedtext_type;  
            typedef TWEI_FILESYSTEMSOURCE_extended_::value_type         ext_filesystemsource_type;  
            typedef TWEI_FORMCONFIDENCE_extended_::value_type           ext_formconfidence_type;  
            typedef TWEI_FORMHORZDOCOFFSET_extended_::value_type        ext_formhorzdocoffset_type;  
            typedef TWEI_FORMTEMPLATEMATCH_extended_::value_type        ext_formtemplatematch_type;  
            typedef TWEI_FORMTEMPLATEPAGEMATCH_extended_::value_type    ext_formtemplatepagematch_type;  
            typedef TWEI_FORMVERTDOCOFFSET_extended_::value_type        ext_formvertdocoffset_type;  
            typedef TWEI_FRAME_extended_::value_type                    ext_frame_type;  
            typedef TWEI_FRAMENUMBER_extended_::value_type              ext_framenumber_type;  
            typedef TWEI_HORZLINECOUNT_extended_::value_type            ext_horzlinecount_type;  
            typedef TWEI_HORZLINELENGTH_extended_::value_type           ext_horzlinelength_type;  
            typedef TWEI_HORZLINETHICKNESS_extended_::value_type        ext_horzlinethickness_type;  
            typedef TWEI_HORZLINEXCOORD_extended_::value_type           ext_horzlinexcoord_type;  
            typedef TWEI_HORZLINEYCOORD_extended_::value_type           ext_horzlineycoord_type;  
            typedef TWEI_ICCPROFILE_extended_::value_type               ext_iccprofile_type;  
            typedef TWEI_IMAGEMERGED_extended_::value_type              ext_imagemerged_type;  
            typedef TWEI_LASTSEGMENT_extended_::value_type              ext_lastsegment_type;  
            typedef TWEI_MAGDATA_extended_::value_type                  ext_magdata_type;  
            typedef TWEI_MAGDATALENGTH_extended_::value_type            ext_magdatalength_type;  
            typedef TWEI_MAGTYPE_extended_::value_type                  ext_magtype_type;  
            typedef TWEI_PAGENUMBER_extended_::value_type               ext_pagenumber_type;  
            typedef TWEI_PAGESIDE_extended_::value_type                 ext_pageside_type;  
            typedef TWEI_PAPERCOUNT_extended_::value_type               ext_papercount_type;  
            typedef TWEI_PATCHCODE_extended_::value_type                ext_patchcode_type;  
            typedef TWEI_PIXELFLAVOR_extended_::value_type              ext_pixelflavor_type;  
            typedef TWEI_PRINTERTEXT_extended_::value_type              ext_printertext_type;  
            typedef TWEI_SEGMENTNUMBER_extended_::value_type            ext_segmentnumber_type;  
            typedef TWEI_SKEWCONFIDENCE_extended_::value_type           ext_skewconfidence_type;  
            typedef TWEI_SKEWFINALANGLE_extended_::value_type           ext_skewfinalangle_type;  
            typedef TWEI_SKEWORIGINALANGLE_extended_::value_type        ext_skeworiginalangle_type;  
            typedef TWEI_SKEWWINDOWX1_extended_::value_type             ext_skewwindowx1_type;  
            typedef TWEI_SKEWWINDOWX2_extended_::value_type             ext_skewwindowx2_type;  
            typedef TWEI_SKEWWINDOWX3_extended_::value_type             ext_skewwindowx3_type;  
            typedef TWEI_SKEWWINDOWX4_extended_::value_type             ext_skewwindowx4_type;  
            typedef TWEI_SKEWWINDOWY1_extended_::value_type             ext_skewwindowy1_type;  
            typedef TWEI_SKEWWINDOWY2_extended_::value_type             ext_skewwindowy2_type;  
            typedef TWEI_SKEWWINDOWY3_extended_::value_type             ext_skewwindowy3_type;  
            typedef TWEI_SKEWWINDOWY4_extended_::value_type             ext_skewwindowy4_type;  
            typedef TWEI_SPECKLESREMOVED_extended_::value_type          ext_specklesremoved_type;  
            typedef TWEI_TWAINDIRECTMETADATA_extended_::value_type      ext_twaindirectmetadata_type;  
            typedef TWEI_VERTLINECOUNT_extended_::value_type            ext_vertlinecount_type;  
            typedef TWEI_VERTLINELENGTH_extended_::value_type           ext_vertlinelength_type;  
            typedef TWEI_VERTLINETHICKNESS_extended_::value_type        ext_vertlinethickness_type;  
            typedef TWEI_VERTLINEXCOORD_extended_::value_type           ext_vertlinexcoord_type;  
            typedef TWEI_VERTLINEYCOORD_extended_::value_type           ext_vertlineycoord_type;  
            typedef TWEI_WHITESPECKLESREMOVED_extended_::value_type     ext_whitespecklesremoved_type;  
        };
    }
}
#endif
