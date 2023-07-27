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
#ifndef DTWAIN_TWAIN_VALUES_HPP
#define DTWAIN_TWAIN_VALUES_HPP

#include <dtwain.h>
#include <cstdint>
#include <map>
#include <unordered_set>
#include <unordered_map>
#include <array>
#include <vector>
#include <string>
#include <algorithm>
#include <dynarithmic/twain/tostring/tostring.hpp>

#define DTWAIN_DEFINE_STRING_MAP()  static const std::map<value_type, std::pair<const char *, const char*>>& get_names() \
        { \
            static std::map<value_type, std::pair<const char *, const char*>> sMap = \
            { 

#define DTWAIN_ADD_MAP_ENTRY(a,b) std::make_pair(a, std::make_pair(#b,#a)) ,
#define DTWAIN_END_STRING_MAP() }; return sMap; }
#define DTWAIN_ADD_TO_STRING1(a) static std::pair<const char *, const char*> to_twain_string(a val) \
                                 { \
                                    std::array<a, 1> v = {val}; \
                                    return dynarithmic::twain::to_twain_string(v, get_names()).front(); \
                                 }\
                                 template <typename Iter> \
                                 static std::vector<std::pair<const char*, const char*>> \
                                    to_twain_string(Iter it1, Iter it2) \
                                { \
                                   return dynarithmic::twain::to_twain_string(it1, it2, get_names());\
                                 }

namespace dynarithmic
{
    namespace twain
    {
        struct alarm_value
        {
            static constexpr uint16_t alarm         = TWAL_ALARM;
            static constexpr uint16_t feedererror   = TWAL_FEEDERERROR;
            static constexpr uint16_t feederwarning = TWAL_FEEDERWARNING;
            static constexpr uint16_t barcode       = TWAL_BARCODE;
            static constexpr uint16_t doublefeed    = TWAL_DOUBLEFEED;
            static constexpr uint16_t jam           = TWAL_JAM;
            static constexpr uint16_t patchcode     = TWAL_PATCHCODE;
            static constexpr uint16_t power         = TWAL_POWER;
            static constexpr uint16_t skew          = TWAL_SKEW;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(alarm,TWAL_ALARM)
            DTWAIN_ADD_MAP_ENTRY(feedererror,TWAL_FEEDERERROR)
            DTWAIN_ADD_MAP_ENTRY(feederwarning,TWAL_FEEDERWARNING)
            DTWAIN_ADD_MAP_ENTRY(barcode,TWAL_BARCODE)
            DTWAIN_ADD_MAP_ENTRY(doublefeed,TWAL_DOUBLEFEED)
            DTWAIN_ADD_MAP_ENTRY(jam,TWAL_JAM)
            DTWAIN_ADD_MAP_ENTRY(patchcode,TWAL_PATCHCODE)
            DTWAIN_ADD_MAP_ENTRY(power,TWAL_POWER)
            DTWAIN_ADD_MAP_ENTRY(skew,TWAL_SKEW)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct autosize_value
        {
            static constexpr uint16_t none          = TWAS_NONE;
            static constexpr uint16_t autosize      = TWAS_AUTO;
            static constexpr uint16_t currentsize   = TWAS_CURRENT;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none,TWAS_NONE)
            DTWAIN_ADD_MAP_ENTRY(autosize,TWAS_AUTO)
            DTWAIN_ADD_MAP_ENTRY(currentsize,TWAS_CURRENT)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct barcodesearchmode_value
        {
            static constexpr uint16_t horz          = TWBD_HORZ;
            static constexpr uint16_t vert          = TWBD_VERT;
            static constexpr uint16_t horzvert      = TWBD_HORZVERT;
            static constexpr uint16_t verthorz      = TWBD_VERTHORZ;
            static constexpr uint16_t default_val   = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(horz,TWBD_HORZ)
            DTWAIN_ADD_MAP_ENTRY(vert,TWBD_VERT)
            DTWAIN_ADD_MAP_ENTRY(horzvert,TWBD_HORZVERT)
            DTWAIN_ADD_MAP_ENTRY(verthorz,TWBD_VERTHORZ)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct barcodetype_value
        {
            static constexpr uint16_t sp_2of5datalogic      = TWBT_2OF5DATALOGIC;
            static constexpr uint16_t sp_2of5iata           = TWBT_2OF5IATA;
            static constexpr uint16_t sp_2of5industrial     = TWBT_2OF5INDUSTRIAL;
            static constexpr uint16_t sp_2of5interleaved    = TWBT_2OF5INTERLEAVED;
            static constexpr uint16_t sp_2of5matrix         = TWBT_2OF5MATRIX;
            static constexpr uint16_t sp_2of5noninterleaved = TWBT_2OF5NONINTERLEAVED;
            static constexpr uint16_t sp_3of9               = TWBT_3OF9;
            static constexpr uint16_t sp_3of9fullascii      = TWBT_3OF9FULLASCII;
            static constexpr uint16_t sp_codabar            = TWBT_CODABAR;
            static constexpr uint16_t sp_codabarwithstartstop = TWBT_CODABARWITHSTARTSTOP;
            static constexpr uint16_t sp_code128            = TWBT_CODE128;
            static constexpr uint16_t sp_code93             = TWBT_CODE93;
            static constexpr uint16_t sp_ean13              = TWBT_EAN13;
            static constexpr uint16_t sp_ean8               = TWBT_EAN8;
            static constexpr uint16_t sp_maxicode           = TWBT_MAXICODE;
            static constexpr uint16_t sp_pdf417             = TWBT_PDF417;
            static constexpr uint16_t sp_postnet            = TWBT_POSTNET;
            static constexpr uint16_t sp_qrcode             = TWBT_QRCODE;
            static constexpr uint16_t sp_ucc128             = TWBT_UCC128;
            static constexpr uint16_t sp_upca               = TWBT_UPCA;
            static constexpr uint16_t sp_upce               = TWBT_UPCE;
            static constexpr uint16_t default_val           = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(sp_2of5datalogic, TWBT_2OF5DATALOGIC)
            DTWAIN_ADD_MAP_ENTRY(sp_2of5iata, TWBT_2OF5IATA)
            DTWAIN_ADD_MAP_ENTRY(sp_2of5industrial, TWBT_2OF5INDUSTRIAL)
            DTWAIN_ADD_MAP_ENTRY(sp_2of5interleaved, TWBT_2OF5INTERLEAVED)
            DTWAIN_ADD_MAP_ENTRY(sp_2of5matrix, TWBT_2OF5MATRIX)
            DTWAIN_ADD_MAP_ENTRY(sp_2of5noninterleaved, TWBT_2OF5NONINTERLEAVED)
            DTWAIN_ADD_MAP_ENTRY(sp_3of9, TWBT_3OF9)
            DTWAIN_ADD_MAP_ENTRY(sp_3of9fullascii, TWBT_3OF9FULLASCII)
            DTWAIN_ADD_MAP_ENTRY(sp_codabar, TWBT_CODABAR)
            DTWAIN_ADD_MAP_ENTRY(sp_codabarwithstartstop, TWBT_CODABARWITHSTARTSTOP)
            DTWAIN_ADD_MAP_ENTRY(sp_code128, TWBT_CODE128)
            DTWAIN_ADD_MAP_ENTRY(sp_code93, TWBT_CODE93)
            DTWAIN_ADD_MAP_ENTRY(sp_ean13, TWBT_EAN13)
            DTWAIN_ADD_MAP_ENTRY(sp_ean8, TWBT_EAN8)
            DTWAIN_ADD_MAP_ENTRY(sp_maxicode, TWBT_MAXICODE)
            DTWAIN_ADD_MAP_ENTRY(sp_pdf417, TWBT_PDF417)
            DTWAIN_ADD_MAP_ENTRY(sp_postnet, TWBT_POSTNET)
            DTWAIN_ADD_MAP_ENTRY(sp_qrcode, TWBT_QRCODE)
            DTWAIN_ADD_MAP_ENTRY(sp_ucc128, TWBT_UCC128)
            DTWAIN_ADD_MAP_ENTRY(sp_upca, TWBT_UPCA)
            DTWAIN_ADD_MAP_ENTRY(sp_upce, TWBT_UPCE)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct bitdepthreduction_value
        {
            static constexpr uint16_t threshold         = TWBR_THRESHOLD;
            static constexpr uint16_t halftone          = TWBR_HALFTONE;
            static constexpr uint16_t custhalftone      = TWBR_CUSTHALFTONE;
            static constexpr uint16_t diffusion         = TWBR_DIFFUSION;
            static constexpr uint16_t dynamicthreshold  = TWBR_DYNAMICTHRESHOLD;
            static constexpr uint16_t default_val       = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(threshold, TWBR_THRESHOLD)
            DTWAIN_ADD_MAP_ENTRY(halftone, TWBR_HALFTONE)
            DTWAIN_ADD_MAP_ENTRY(custhalftone, TWBR_CUSTHALFTONE)
            DTWAIN_ADD_MAP_ENTRY(diffusion, TWBR_DIFFUSION)
            DTWAIN_ADD_MAP_ENTRY(dynamicthreshold, TWBR_DYNAMICTHRESHOLD)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct bitorder_value
        {
            static constexpr uint16_t lsbfirst = TWBO_LSBFIRST;
            static constexpr uint16_t msbfirst = TWBO_MSBFIRST;
            static constexpr uint16_t default_val = (std::numeric_limits<uint16_t>::max)();
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(lsbfirst, TWBO_LSBFIRST)
            DTWAIN_ADD_MAP_ENTRY(msbfirst, TWBO_MSBFIRST)
            DTWAIN_ADD_MAP_ENTRY(default_val, (std::numeric_limits<uint16_t>::max)())
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct cameraside_value
        {
            static constexpr uint16_t both      = TWCS_BOTH;
            static constexpr uint16_t top       = TWCS_TOP;
            static constexpr uint16_t bottom    = TWCS_BOTTOM;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(both,TWCS_BOTH)
            DTWAIN_ADD_MAP_ENTRY(top,TWCS_TOP)
            DTWAIN_ADD_MAP_ENTRY(bottom,TWCS_BOTTOM)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct compression_value
        {
            static constexpr uint16_t none      = TWCP_NONE;
            static constexpr uint16_t packbits  = TWCP_PACKBITS;
            static constexpr uint16_t group31D  = TWCP_GROUP31D;
            static constexpr uint16_t group31DEOL = TWCP_GROUP31DEOL;
            static constexpr uint16_t group32D  = TWCP_GROUP32D;
            static constexpr uint16_t group4    = TWCP_GROUP4;
            static constexpr uint16_t jpeg      = TWCP_JPEG;
            static constexpr uint16_t lzw       = TWCP_LZW;
            static constexpr uint16_t jbig      = TWCP_JBIG;
            static constexpr uint16_t png       = TWCP_PNG;
            static constexpr uint16_t rle4      = TWCP_RLE4;
            static constexpr uint16_t rle8      = TWCP_RLE8;
            static constexpr uint16_t bitfields = TWCP_BITFIELDS;
            static constexpr uint16_t zip       = TWCP_ZIP;
            static constexpr uint16_t jpeg2000  = TWCP_JPEG2000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none,TWCP_NONE)
            DTWAIN_ADD_MAP_ENTRY(packbits,TWCP_PACKBITS)
            DTWAIN_ADD_MAP_ENTRY(group31D,TWCP_GROUP31D)
            DTWAIN_ADD_MAP_ENTRY(group31DEOL,TWCP_GROUP31DEOL)
            DTWAIN_ADD_MAP_ENTRY(group32D,TWCP_GROUP32D)
            DTWAIN_ADD_MAP_ENTRY(group4,TWCP_GROUP4)
            DTWAIN_ADD_MAP_ENTRY(jpeg,TWCP_JPEG)
            DTWAIN_ADD_MAP_ENTRY(lzw,TWCP_LZW)
            DTWAIN_ADD_MAP_ENTRY(jbig,TWCP_JBIG)
            DTWAIN_ADD_MAP_ENTRY(png,TWCP_PNG)
            DTWAIN_ADD_MAP_ENTRY(rle4,TWCP_RLE4)
            DTWAIN_ADD_MAP_ENTRY(rle8,TWCP_RLE8)
            DTWAIN_ADD_MAP_ENTRY(bitfields,TWCP_BITFIELDS)
            DTWAIN_ADD_MAP_ENTRY(zip,TWCP_ZIP)
            DTWAIN_ADD_MAP_ENTRY(jpeg2000,TWCP_JPEG2000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct deviceevent_value
        {
            static constexpr uint16_t checkautomaticcapture = TWDE_CHECKAUTOMATICCAPTURE;
            static constexpr uint16_t checkbattery          = TWDE_CHECKBATTERY;
            static constexpr uint16_t checkflash            = TWDE_CHECKFLASH;
            static constexpr uint16_t checkpowersupply      = TWDE_CHECKPOWERSUPPLY;
            static constexpr uint16_t checkresolution       = TWDE_CHECKRESOLUTION;
            static constexpr uint16_t deviceadded           = TWDE_DEVICEADDED;
            static constexpr uint16_t deviceoffline         = TWDE_DEVICEOFFLINE;
            static constexpr uint16_t deviceready           = TWDE_DEVICEREADY;
            static constexpr uint16_t deviceremoved         = TWDE_DEVICEREMOVED;
            static constexpr uint16_t imagecaptured         = TWDE_IMAGECAPTURED;
            static constexpr uint16_t imagedeleted          = TWDE_IMAGEDELETED;
            static constexpr uint16_t paperdoublefeed       = TWDE_PAPERDOUBLEFEED;
            static constexpr uint16_t paperjam              = TWDE_PAPERJAM;
            static constexpr uint16_t lampfailure           = TWDE_LAMPFAILURE;
            static constexpr uint16_t checkdeviceonline     = TWDE_CHECKDEVICEONLINE;
            static constexpr uint16_t powersave             = TWDE_POWERSAVE;
            static constexpr uint16_t powersavenotify       = TWDE_POWERSAVENOTIFY;
            static constexpr uint16_t customevents          = TWDE_CUSTOMEVENTS;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(checkautomaticcapture,TWDE_CHECKAUTOMATICCAPTURE)
            DTWAIN_ADD_MAP_ENTRY(checkbattery,TWDE_CHECKBATTERY)
            DTWAIN_ADD_MAP_ENTRY(checkflash,TWDE_CHECKFLASH)
            DTWAIN_ADD_MAP_ENTRY(checkpowersupply,TWDE_CHECKPOWERSUPPLY)
            DTWAIN_ADD_MAP_ENTRY(checkresolution,TWDE_CHECKRESOLUTION)
            DTWAIN_ADD_MAP_ENTRY(deviceadded,TWDE_DEVICEADDED)
            DTWAIN_ADD_MAP_ENTRY(deviceoffline,TWDE_DEVICEOFFLINE)
            DTWAIN_ADD_MAP_ENTRY(deviceready,TWDE_DEVICEREADY)
            DTWAIN_ADD_MAP_ENTRY(deviceremoved,TWDE_DEVICEREMOVED)
            DTWAIN_ADD_MAP_ENTRY(imagecaptured,TWDE_IMAGECAPTURED)
            DTWAIN_ADD_MAP_ENTRY(imagedeleted,TWDE_IMAGEDELETED)
            DTWAIN_ADD_MAP_ENTRY(paperdoublefeed,TWDE_PAPERDOUBLEFEED)
            DTWAIN_ADD_MAP_ENTRY(paperjam,TWDE_PAPERJAM)
            DTWAIN_ADD_MAP_ENTRY(lampfailure,TWDE_LAMPFAILURE)
            DTWAIN_ADD_MAP_ENTRY(checkdeviceonline,TWDE_CHECKDEVICEONLINE)
            DTWAIN_ADD_MAP_ENTRY(powersave,TWDE_POWERSAVE)
            DTWAIN_ADD_MAP_ENTRY(powersavenotify,TWDE_POWERSAVENOTIFY)
            DTWAIN_ADD_MAP_ENTRY(customevents,TWDE_CUSTOMEVENTS)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct doublefeeddetection_value
        {
            static constexpr uint16_t ultrasonic    = TWDF_ULTRASONIC;
            static constexpr uint16_t bylength      = TWDF_BYLENGTH;
            static constexpr uint16_t infrared      = TWDF_INFRARED;
            static constexpr uint16_t default_val   = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(ultrasonic, TWDF_ULTRASONIC)
            DTWAIN_ADD_MAP_ENTRY(bylength, TWDF_BYLENGTH)
            DTWAIN_ADD_MAP_ENTRY(infrared, TWDF_INFRARED)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct doublefeedresponse_value
        {
            static constexpr uint16_t stop        = TWDP_STOP;
            static constexpr uint16_t stopandwait = TWDP_STOPANDWAIT;
            static constexpr uint16_t sound       = TWDP_SOUND;
            static constexpr uint16_t donotimprint = TWDP_DONOTIMPRINT;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(stop, TWDP_STOP)
            DTWAIN_ADD_MAP_ENTRY(stopandwait, TWDP_STOPANDWAIT)
            DTWAIN_ADD_MAP_ENTRY(sound, TWDP_SOUND)
            DTWAIN_ADD_MAP_ENTRY(donotimprint, TWDP_DONOTIMPRINT)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct doublefeedsensitivity_value
        {
            static constexpr uint16_t low       = TWUS_LOW;
            static constexpr uint16_t medium    = TWUS_MEDIUM;
            static constexpr uint16_t high      = TWUS_HIGH;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(low, TWUS_LOW)
            DTWAIN_ADD_MAP_ENTRY(medium, TWUS_MEDIUM)
            DTWAIN_ADD_MAP_ENTRY(high, TWUS_HIGH)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct duplextype_value
        {
            static constexpr uint16_t none      = TWDX_NONE;
            static constexpr uint16_t onepass   = TWDX_1PASSDUPLEX;
            static constexpr uint16_t twopass   = TWDX_2PASSDUPLEX;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none, TWDX_NONE)
            DTWAIN_ADD_MAP_ENTRY(onepass, TWDX_1PASSDUPLEX)
            DTWAIN_ADD_MAP_ENTRY(twopass, TWDX_2PASSDUPLEX)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct feederalignment_value
        {
            static constexpr uint16_t none      = TWFA_NONE;
            static constexpr uint16_t left      = TWFA_LEFT;
            static constexpr uint16_t center    = TWFA_CENTER;
            static constexpr uint16_t right     = TWFA_RIGHT;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none, TWFA_NONE)
            DTWAIN_ADD_MAP_ENTRY(left, TWFA_LEFT)
            DTWAIN_ADD_MAP_ENTRY(center, TWFA_CENTER)
            DTWAIN_ADD_MAP_ENTRY(right, TWFA_RIGHT)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct feederorder_value
        {
            static constexpr uint16_t firstpagefirst    = TWFO_FIRSTPAGEFIRST;
            static constexpr uint16_t lastpagefirst     = TWFO_LASTPAGEFIRST;
            static constexpr uint16_t default_val       = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(firstpagefirst, TWFO_FIRSTPAGEFIRST)
            DTWAIN_ADD_MAP_ENTRY(lastpagefirst, TWFO_LASTPAGEFIRST)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct feederpocket_value
        {
            static constexpr uint16_t pocket1 = TWFP_POCKET1;
            static constexpr uint16_t pocket2 = TWFP_POCKET2;
            static constexpr uint16_t pocket3 = TWFP_POCKET3;
            static constexpr uint16_t pocket4 = TWFP_POCKET4;
            static constexpr uint16_t pocket5 = TWFP_POCKET5;
            static constexpr uint16_t pocket6 = TWFP_POCKET6;
            static constexpr uint16_t pocket7 = TWFP_POCKET7;
            static constexpr uint16_t pocket8 = TWFP_POCKET8;
            static constexpr uint16_t pocket9 = TWFP_POCKET9;
            static constexpr uint16_t pocket10 = TWFP_POCKET10;
            static constexpr uint16_t pocket11 = TWFP_POCKET11;
            static constexpr uint16_t pocket12 = TWFP_POCKET12;
            static constexpr uint16_t pocket13 = TWFP_POCKET13;
            static constexpr uint16_t pocket14 = TWFP_POCKET14;
            static constexpr uint16_t pocket15 = TWFP_POCKET15;
            static constexpr uint16_t pocket16 = TWFP_POCKET16;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(pocket1, TWFP_POCKET1)
            DTWAIN_ADD_MAP_ENTRY(pocket2, TWFP_POCKET2)
            DTWAIN_ADD_MAP_ENTRY(pocket3, TWFP_POCKET3)
            DTWAIN_ADD_MAP_ENTRY(pocket4, TWFP_POCKET4)
            DTWAIN_ADD_MAP_ENTRY(pocket5, TWFP_POCKET5)
            DTWAIN_ADD_MAP_ENTRY(pocket6, TWFP_POCKET6)
            DTWAIN_ADD_MAP_ENTRY(pocket7, TWFP_POCKET7)
            DTWAIN_ADD_MAP_ENTRY(pocket8, TWFP_POCKET8)
            DTWAIN_ADD_MAP_ENTRY(pocket9, TWFP_POCKET9)
            DTWAIN_ADD_MAP_ENTRY(pocket10, TWFP_POCKET10)
            DTWAIN_ADD_MAP_ENTRY(pocket11, TWFP_POCKET11)
            DTWAIN_ADD_MAP_ENTRY(pocket12, TWFP_POCKET12)
            DTWAIN_ADD_MAP_ENTRY(pocket13, TWFP_POCKET13)
            DTWAIN_ADD_MAP_ENTRY(pocket14, TWFP_POCKET14)
            DTWAIN_ADD_MAP_ENTRY(pocket15, TWFP_POCKET15)
            DTWAIN_ADD_MAP_ENTRY(pocket16, TWFP_POCKET16)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct feedertype_value
        {
            static constexpr uint16_t general   = TWFE_GENERAL;
            static constexpr uint16_t photo     = TWFE_PHOTO;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(general, TWFE_GENERAL)
            DTWAIN_ADD_MAP_ENTRY(photo, TWFE_PHOTO)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct filetype_value
        {
            typedef uint16_t value_type;
            static constexpr value_type  bmp = DTWAIN_BMP;
            static constexpr value_type  dcx = DTWAIN_DCX;
            static constexpr value_type  enhancedmetafile = DTWAIN_EMF;
            static constexpr value_type  gif = DTWAIN_GIF;
            static constexpr value_type  googlewebp = DTWAIN_WEBP;
            static constexpr value_type  jpeg = DTWAIN_JPEG;
            static constexpr value_type  jpeg2k = DTWAIN_JPEG2000;
            static constexpr value_type  pcx = DTWAIN_PCX;
            static constexpr value_type  pdf = DTWAIN_PDF;
            static constexpr value_type  png = DTWAIN_PNG;
            static constexpr value_type  postscript1 = DTWAIN_POSTSCRIPT1;
            static constexpr value_type  postscript2 = DTWAIN_POSTSCRIPT2;
            static constexpr value_type  postscript3 = DTWAIN_POSTSCRIPT3;
            static constexpr value_type  psd = DTWAIN_PSD;
            static constexpr value_type  targa = DTWAIN_TGA;
            static constexpr value_type  text = DTWAIN_TEXT;
            static constexpr value_type  tiffdeflate = DTWAIN_TIFFDEFLATE;
            static constexpr value_type  tiffgroup3 = DTWAIN_TIFFG3;
            static constexpr value_type  tiffgroup4 = DTWAIN_TIFFG4;
            static constexpr value_type  tiffjpeg = DTWAIN_TIFFJPEG;
            static constexpr value_type  tifflzw = DTWAIN_TIFFLZW;
            static constexpr value_type  tiffnocompress = DTWAIN_TIFFNONE;
            static constexpr value_type  tiffpackbits = DTWAIN_TIFFPACKBITS;
            static constexpr value_type  windowsicon = DTWAIN_ICO;
            static constexpr value_type  windowsvistaicon = DTWAIN_ICO_VISTA;
            static constexpr value_type  windowsiconresized = DTWAIN_ICO_RESIZED;
            static constexpr value_type  windowsmetafile = DTWAIN_WMF;
            static constexpr value_type  wirelessbmp = DTWAIN_WBMP;
            static constexpr value_type  wirelessbmpresized = DTWAIN_WBMP_RESIZED;
            static constexpr value_type  portablebitmap = DTWAIN_PBM;
            static constexpr value_type  dcxmulti = DTWAIN_DCX;
            static constexpr value_type  pdfmulti = DTWAIN_PDFMULTI;
            static constexpr value_type  postscript1multi = DTWAIN_POSTSCRIPT1MULTI;
            static constexpr value_type  postscript2multi = DTWAIN_POSTSCRIPT2MULTI;
            static constexpr value_type  postscript3multi = DTWAIN_POSTSCRIPT3MULTI;
            static constexpr value_type  tiffdeflatemulti = DTWAIN_TIFFDEFLATEMULTI;
            static constexpr value_type  tiffgroup3multi = DTWAIN_TIFFG3MULTI;
            static constexpr value_type  tiffgroup4multi = DTWAIN_TIFFG4MULTI;
            static constexpr value_type  tiffjpegmulti = DTWAIN_TIFFJPEGMULTI;
            static constexpr value_type  tifflzwmulti = DTWAIN_TIFFLZWMULTI;
            static constexpr value_type  tiffnocompressmulti = DTWAIN_TIFFNONEMULTI;
            static constexpr value_type  tiffpackbitsmulti = DTWAIN_TIFFPACKBITSMULTI;
            static constexpr value_type  textmulti = DTWAIN_TEXTMULTI;
            static constexpr value_type  bmp_source_mode = DTWAIN_FF_BMP;
            static constexpr value_type  tiff_source_mode = DTWAIN_FF_TIFF;
            static constexpr value_type  pict_source_mode = DTWAIN_FF_PICT;
            static constexpr value_type  xbm_source_mode = DTWAIN_FF_XBM;
            static constexpr value_type  jfif_source_mode = DTWAIN_FF_JFIF;
            static constexpr value_type  fpx_source_mode = DTWAIN_FF_FPX;
            static constexpr value_type  tiffmulti_source_mode = DTWAIN_FF_TIFFMULTI;
            static constexpr value_type  png_source_mode = DTWAIN_FF_PNG;
            static constexpr value_type  spiff_source_mode = DTWAIN_FF_SPIFF;
            static constexpr value_type  exif_source_mode = DTWAIN_FF_EXIF;
            static constexpr value_type  pdf_source_mode = DTWAIN_FF_PDF;
            static constexpr value_type  jp2_source_mode = DTWAIN_FF_JP2;
            static constexpr value_type  jpx_source_mode = DTWAIN_FF_JPX;
            static constexpr value_type  dejavu_source_mode = DTWAIN_FF_DEJAVU;
            static constexpr value_type  pdfa_source_mode = DTWAIN_FF_PDFA;
            static constexpr value_type  pdfa2_source_mode = DTWAIN_FF_PDFA2;
            static constexpr value_type  pdfraster_source_mode = DTWAIN_FF_PDFRASTER;
        };

        struct tiffcompress_value
        {
            typedef uint16_t value_type;
            static constexpr value_type  deflate = DTWAIN_TIFFDEFLATE;
            static constexpr value_type  group3 = DTWAIN_TIFFG3;
            static constexpr value_type  group4 = DTWAIN_TIFFG4;
            static constexpr value_type  jpeg = DTWAIN_TIFFJPEG;
            static constexpr value_type  lzw = DTWAIN_TIFFLZW;
            static constexpr value_type  nocompress = DTWAIN_TIFFNONE;
            static constexpr value_type  packbits = DTWAIN_TIFFPACKBITS;
        };

        struct filmtype_value
        {
            static constexpr uint16_t positive  = TWFM_POSITIVE;
            static constexpr uint16_t negative  = TWFM_NEGATIVE;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(positive,TWFM_POSITIVE)
            DTWAIN_ADD_MAP_ENTRY(negative,TWFM_NEGATIVE)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct filter_value
        {
            static constexpr uint16_t red       = TWFT_RED;
            static constexpr uint16_t green     = TWFT_GREEN;
            static constexpr uint16_t blue      = TWFT_BLUE;
            static constexpr uint16_t none      = TWFT_NONE;
            static constexpr uint16_t white     = TWFT_WHITE;
            static constexpr uint16_t cyan      = TWFT_CYAN;
            static constexpr uint16_t magenta   = TWFT_MAGENTA;
            static constexpr uint16_t yellow    = TWFT_YELLOW;
            static constexpr uint16_t black     = TWFT_BLACK;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(red,TWFT_RED)
            DTWAIN_ADD_MAP_ENTRY(green,TWFT_GREEN)
            DTWAIN_ADD_MAP_ENTRY(blue,TWFT_BLUE)
            DTWAIN_ADD_MAP_ENTRY(none,TWFT_NONE)
            DTWAIN_ADD_MAP_ENTRY(white,TWFT_WHITE)
            DTWAIN_ADD_MAP_ENTRY(cyan,TWFT_CYAN)
            DTWAIN_ADD_MAP_ENTRY(magenta,TWFT_MAGENTA)
            DTWAIN_ADD_MAP_ENTRY(yellow,TWFT_YELLOW)
            DTWAIN_ADD_MAP_ENTRY(black,TWFT_BLACK)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct flashused_value
        {
            static constexpr uint16_t none        = TWFL_NONE;
            static constexpr uint16_t off         = TWFL_OFF;
            static constexpr uint16_t on          = TWFL_ON;
            static constexpr uint16_t automatic   = TWFL_AUTO;
            static constexpr uint16_t redeye      = TWFL_REDEYE;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none,TWFL_NONE)
            DTWAIN_ADD_MAP_ENTRY(off,TWFL_OFF)
            DTWAIN_ADD_MAP_ENTRY(on,TWFL_ON)
            DTWAIN_ADD_MAP_ENTRY(automatic,TWFL_AUTO)
            DTWAIN_ADD_MAP_ENTRY(redeye,TWFL_REDEYE)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct fliprotation_value
        {
            static constexpr uint16_t book          = TWFR_BOOK;
            static constexpr uint16_t fanfold       = TWFR_FANFOLD;
            static constexpr uint16_t default_flip  = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(book, TWFR_BOOK)
            DTWAIN_ADD_MAP_ENTRY(fanfold, TWFR_FANFOLD)
            DTWAIN_ADD_MAP_ENTRY(default_flip, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct fontstyle_value
        {
            static constexpr uint16_t normal    = TWPF_NORMAL;
            static constexpr uint16_t italic    = TWPF_ITALIC;
            static constexpr uint16_t bold      = TWPF_BOLD;
            static constexpr uint16_t smallsize = TWPF_SMALLSIZE;
            static constexpr uint16_t largesize = TWPF_LARGESIZE;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(normal, TWPF_NORMAL)
            DTWAIN_ADD_MAP_ENTRY(italic, TWPF_ITALIC)
            DTWAIN_ADD_MAP_ENTRY(bold, TWPF_BOLD)
            DTWAIN_ADD_MAP_ENTRY(smallsize, TWPF_SMALLSIZE)
            DTWAIN_ADD_MAP_ENTRY(largesize, TWPF_LARGESIZE)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct iccprofile_value
        {
            static constexpr uint16_t none      = TWIC_NONE;
            static constexpr uint16_t embed     = TWIC_EMBED;
            static constexpr uint16_t link      = TWIC_LINK;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none, TWIC_NONE)
            DTWAIN_ADD_MAP_ENTRY(embed, TWIC_EMBED)
            DTWAIN_ADD_MAP_ENTRY(link, TWIC_LINK)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct imagefilter_value
        {
            static constexpr uint16_t  none         = TWIF_NONE;
            static constexpr uint16_t  automatic    = TWIF_AUTO;
            static constexpr uint16_t  lowpass      = TWIF_LOWPASS;
            static constexpr uint16_t  highpass     = TWIF_HIGHPASS;
            static constexpr uint16_t  text         = TWIF_TEXT;
            static constexpr uint16_t  fineline     = TWIF_FINELINE;
            static constexpr uint16_t  default_val  = 1000;
            typedef uint16_t  value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none,TWIF_NONE)
            DTWAIN_ADD_MAP_ENTRY(automatic,TWIF_AUTO)
            DTWAIN_ADD_MAP_ENTRY(lowpass,TWIF_LOWPASS)
            DTWAIN_ADD_MAP_ENTRY(highpass,TWIF_HIGHPASS)
            DTWAIN_ADD_MAP_ENTRY(text,TWIF_TEXT)
            DTWAIN_ADD_MAP_ENTRY(fineline,TWIF_FINELINE)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct imagemerge_value
        {
            static constexpr uint16_t none          = TWIM_NONE;
            static constexpr uint16_t frontontop    = TWIM_FRONTONTOP;
            static constexpr uint16_t frontonbottom = TWIM_FRONTONBOTTOM;
            static constexpr uint16_t frontonleft   = TWIM_FRONTONLEFT;
            static constexpr uint16_t frontonright  = TWIM_FRONTONRIGHT;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none, TWIM_NONE)
            DTWAIN_ADD_MAP_ENTRY(frontontop, TWIM_FRONTONTOP)
            DTWAIN_ADD_MAP_ENTRY(frontonbottom, TWIM_FRONTONBOTTOM)
            DTWAIN_ADD_MAP_ENTRY(frontonleft, TWIM_FRONTONLEFT)
            DTWAIN_ADD_MAP_ENTRY(frontonright, TWIM_FRONTONRIGHT)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct indextrigger_value
        {
            static constexpr uint16_t page   = TWCT_PAGE;
            static constexpr uint16_t patch1 = TWCT_PATCH1;
            static constexpr uint16_t patch2 = TWCT_PATCH2;
            static constexpr uint16_t patch3 = TWCT_PATCH3;
            static constexpr uint16_t patch4 = TWCT_PATCH4;
            static constexpr uint16_t patcht = TWCT_PATCHT;
            static constexpr uint16_t patch6 = TWCT_PATCH6;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(page, TWCT_PAGE)
            DTWAIN_ADD_MAP_ENTRY(patch1, TWCT_PATCH1)
            DTWAIN_ADD_MAP_ENTRY(patch2, TWCT_PATCH2)
            DTWAIN_ADD_MAP_ENTRY(patch3, TWCT_PATCH3)
            DTWAIN_ADD_MAP_ENTRY(patch4, TWCT_PATCH4)
            DTWAIN_ADD_MAP_ENTRY(patcht, TWCT_PATCHT)
            DTWAIN_ADD_MAP_ENTRY(patch6, TWCT_PATCH6)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct indicatormode_value
        {
            static constexpr uint16_t info      = TWCI_INFO;
            static constexpr uint16_t warning   = TWCI_WARNING;
            static constexpr uint16_t error     = TWCI_ERROR;
            static constexpr uint16_t warmup    = TWCI_WARMUP;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(info, TWCI_INFO)
            DTWAIN_ADD_MAP_ENTRY(warning, TWCI_WARNING)
            DTWAIN_ADD_MAP_ENTRY(error, TWCI_ERROR)
            DTWAIN_ADD_MAP_ENTRY(warmup, TWCI_WARMUP)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct jobcontrol_value
        {
            static constexpr uint16_t none                   = TWJC_NONE;
            static constexpr uint16_t include_separator      = TWJC_JSIC;
            static constexpr uint16_t include_separator_stop = TWJC_JSIS;
            static constexpr uint16_t exclude_separator      = TWJC_JSXC;
            static constexpr uint16_t exclude_separator_stop = TWJC_JSXS;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none, TWJC_NONE)
            DTWAIN_ADD_MAP_ENTRY(include_separator, TWJC_JSIC)
            DTWAIN_ADD_MAP_ENTRY(include_separator_stop, TWJC_JSIS)
            DTWAIN_ADD_MAP_ENTRY(exclude_separator, TWJC_JSXC)
            DTWAIN_ADD_MAP_ENTRY(exclude_separator_stop, TWJC_JSXS)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct jpegpixel_value
        {
            static constexpr uint16_t bw        = TWPT_BW;
            static constexpr uint16_t gray      = TWPT_GRAY;
            static constexpr uint16_t rgb       = TWPT_RGB;
            static constexpr uint16_t palette   = TWPT_PALETTE;
            static constexpr uint16_t cmy       = TWPT_CMY;
            static constexpr uint16_t cmyk      = TWPT_CMYK;
            static constexpr uint16_t yuv       = TWPT_YUV;
            static constexpr uint16_t yuvk      = TWPT_YUVK;
            static constexpr uint16_t ciexyz    = TWPT_CIEXYZ;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(bw,TWPT_BW)
            DTWAIN_ADD_MAP_ENTRY(gray,TWPT_GRAY)
            DTWAIN_ADD_MAP_ENTRY(rgb,TWPT_RGB)
            DTWAIN_ADD_MAP_ENTRY(palette,TWPT_PALETTE)
            DTWAIN_ADD_MAP_ENTRY(cmy,TWPT_CMY)
            DTWAIN_ADD_MAP_ENTRY(cmyk,TWPT_CMYK)
            DTWAIN_ADD_MAP_ENTRY(yuv,TWPT_YUV)
            DTWAIN_ADD_MAP_ENTRY(yuvk,TWPT_YUVK)
            DTWAIN_ADD_MAP_ENTRY(ciexyz,TWPT_CIEXYZ)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct jpegsubsampling_value
        {
            static constexpr uint16_t js_444YCBCR   = TWJS_444YCBCR;
            static constexpr uint16_t js_444RGB     = TWJS_444RGB;
            static constexpr uint16_t js_422        = TWJS_422;
            static constexpr uint16_t js_421        = TWJS_421;
            static constexpr uint16_t js_411        = TWJS_411;
            static constexpr uint16_t js_420        = TWJS_420;
            static constexpr uint16_t js_410        = TWJS_410;
            static constexpr uint16_t js_311        = TWJS_311;
            static constexpr uint16_t default_val   = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(js_444YCBCR,TWJS_444YCBCR)
            DTWAIN_ADD_MAP_ENTRY(js_444RGB,TWJS_444RGB)
            DTWAIN_ADD_MAP_ENTRY(js_422,TWJS_422)
            DTWAIN_ADD_MAP_ENTRY(js_421,TWJS_421)
            DTWAIN_ADD_MAP_ENTRY(js_411,TWJS_411)
            DTWAIN_ADD_MAP_ENTRY(js_420,TWJS_420)
            DTWAIN_ADD_MAP_ENTRY(js_410,TWJS_410)
            DTWAIN_ADD_MAP_ENTRY(js_311,TWJS_311)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct jpegquality_value
        {
            static constexpr int16_t unknown    = TWJQ_UNKNOWN;
            static constexpr int16_t low        = TWJQ_LOW;
            static constexpr int16_t medium     = TWJQ_MEDIUM;
            static constexpr int16_t high       = TWJQ_HIGH;
            static constexpr int16_t default_val = 1000;
            typedef int16_t  value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(unknown,TWJQ_UNKNOWN)
            DTWAIN_ADD_MAP_ENTRY(low,TWJQ_LOW)
            DTWAIN_ADD_MAP_ENTRY(medium,TWJQ_MEDIUM)
            DTWAIN_ADD_MAP_ENTRY(high,TWJQ_HIGH)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(int16_t)
        };

        struct language_value
        {
            static constexpr uint16_t Userlocale = TWLG_USERLOCALE;
            static constexpr uint16_t Dan = TWLG_DAN;
            static constexpr uint16_t Dut = TWLG_DUT;
            static constexpr uint16_t Eng = TWLG_ENG;
            static constexpr uint16_t Fcf = TWLG_FCF;
            static constexpr uint16_t Fin = TWLG_FIN;
            static constexpr uint16_t Frn = TWLG_FRN;
            static constexpr uint16_t Ger = TWLG_GER;
            static constexpr uint16_t Ice = TWLG_ICE;
            static constexpr uint16_t Itn = TWLG_ITN;
            static constexpr uint16_t Nor = TWLG_NOR;
            static constexpr uint16_t Por = TWLG_POR;
            static constexpr uint16_t Spa = TWLG_SPA;
            static constexpr uint16_t Swe = TWLG_SWE;
            static constexpr uint16_t Usa = TWLG_USA;
            static constexpr uint16_t Afrikaans = TWLG_AFRIKAANS;
            static constexpr uint16_t Albania = TWLG_ALBANIA;
            static constexpr uint16_t Arabic = TWLG_ARABIC;
            static constexpr uint16_t Arabic_Algeria = TWLG_ARABIC_ALGERIA;
            static constexpr uint16_t Arabic_Bahrain = TWLG_ARABIC_BAHRAIN;
            static constexpr uint16_t Arabic_Egypt = TWLG_ARABIC_EGYPT;
            static constexpr uint16_t Arabic_Iraq = TWLG_ARABIC_IRAQ;
            static constexpr uint16_t Arabic_Jordan = TWLG_ARABIC_JORDAN;
            static constexpr uint16_t Arabic_Kuwait = TWLG_ARABIC_KUWAIT;
            static constexpr uint16_t Arabic_Lebanon = TWLG_ARABIC_LEBANON;
            static constexpr uint16_t Arabic_Libya = TWLG_ARABIC_LIBYA;
            static constexpr uint16_t Arabic_Morocco = TWLG_ARABIC_MOROCCO;
            static constexpr uint16_t Arabic_Oman = TWLG_ARABIC_OMAN;
            static constexpr uint16_t Arabic_Qatar = TWLG_ARABIC_QATAR;
            static constexpr uint16_t Arabic_Saudiarabia = TWLG_ARABIC_SAUDIARABIA;
            static constexpr uint16_t Arabic_Syria = TWLG_ARABIC_SYRIA;
            static constexpr uint16_t Arabic_Tunisia = TWLG_ARABIC_TUNISIA;
            static constexpr uint16_t French_Luxembourg = TWLG_FRENCH_LUXEMBOURG;
            static constexpr uint16_t French_Swiss = TWLG_FRENCH_SWISS;
            static constexpr uint16_t German = TWLG_GERMAN;
            static constexpr uint16_t German_Austrian = TWLG_GERMAN_AUSTRIAN;
            static constexpr uint16_t German_Luxembourg = TWLG_GERMAN_LUXEMBOURG;
            static constexpr uint16_t German_Liechtenstein = TWLG_GERMAN_LIECHTENSTEIN;
            static constexpr uint16_t German_Swiss = TWLG_GERMAN_SWISS;
            static constexpr uint16_t Greek = TWLG_GREEK;
            static constexpr uint16_t Hebrew = TWLG_HEBREW;
            static constexpr uint16_t Hungarian = TWLG_HUNGARIAN;
            static constexpr uint16_t Icelandic = TWLG_ICELANDIC;
            static constexpr uint16_t Indonesian = TWLG_INDONESIAN;
            static constexpr uint16_t Italian = TWLG_ITALIAN;
            static constexpr uint16_t Italian_Swiss = TWLG_ITALIAN_SWISS;
            static constexpr uint16_t Japanese = TWLG_JAPANESE;
            static constexpr uint16_t Korean = TWLG_KOREAN;
            static constexpr uint16_t Korean_Johab = TWLG_KOREAN_JOHAB;
            static constexpr uint16_t Arabic_Uae = TWLG_ARABIC_UAE;
            static constexpr uint16_t Arabic_Yemen = TWLG_ARABIC_YEMEN;
            static constexpr uint16_t Basque = TWLG_BASQUE;
            static constexpr uint16_t Byelorussian = TWLG_BYELORUSSIAN;
            static constexpr uint16_t Bulgarian = TWLG_BULGARIAN;
            static constexpr uint16_t Catalan = TWLG_CATALAN;
            static constexpr uint16_t Chinese = TWLG_CHINESE;
            static constexpr uint16_t Chinese_Hongkong = TWLG_CHINESE_HONGKONG;
            static constexpr uint16_t Chinese_Prc = TWLG_CHINESE_PRC;
            static constexpr uint16_t Chinese_Singapore = TWLG_CHINESE_SINGAPORE;
            static constexpr uint16_t Chinese_Simplified = TWLG_CHINESE_SIMPLIFIED;
            static constexpr uint16_t Chinese_Taiwan = TWLG_CHINESE_TAIWAN;
            static constexpr uint16_t Chinese_Traditional = TWLG_CHINESE_TRADITIONAL;
            static constexpr uint16_t Croatia = TWLG_CROATIA;
            static constexpr uint16_t Czech = TWLG_CZECH;
            static constexpr uint16_t Danish = TWLG_DANISH;
            static constexpr uint16_t Dutch = TWLG_DUTCH;
            static constexpr uint16_t Dutch_Belgian = TWLG_DUTCH_BELGIAN;
            static constexpr uint16_t English = TWLG_ENGLISH;
            static constexpr uint16_t English_Australian = TWLG_ENGLISH_AUSTRALIAN;
            static constexpr uint16_t Latvian = TWLG_LATVIAN;
            static constexpr uint16_t Lithuanian = TWLG_LITHUANIAN;
            static constexpr uint16_t Norwegian = TWLG_NORWEGIAN;
            static constexpr uint16_t Norwegian_Bokmal = TWLG_NORWEGIAN_BOKMAL;
            static constexpr uint16_t Norwegian_Nynorsk = TWLG_NORWEGIAN_NYNORSK;
            static constexpr uint16_t Polish = TWLG_POLISH;
            static constexpr uint16_t Portuguese = TWLG_PORTUGUESE;
            static constexpr uint16_t Portuguese_Brazil = TWLG_PORTUGUESE_BRAZIL;
            static constexpr uint16_t Romanian = TWLG_ROMANIAN;
            static constexpr uint16_t Russian = TWLG_RUSSIAN;
            static constexpr uint16_t Serbian_Latin = TWLG_SERBIAN_LATIN;
            static constexpr uint16_t Slovak = TWLG_SLOVAK;
            static constexpr uint16_t Slovenian = TWLG_SLOVENIAN;
            static constexpr uint16_t Spanish = TWLG_SPANISH;
            static constexpr uint16_t Spanish_Mexican = TWLG_SPANISH_MEXICAN;
            static constexpr uint16_t Spanish_Modern = TWLG_SPANISH_MODERN;
            static constexpr uint16_t Swedish = TWLG_SWEDISH;
            static constexpr uint16_t Thai = TWLG_THAI;
            static constexpr uint16_t Turkish = TWLG_TURKISH;
            static constexpr uint16_t Ukranian = TWLG_UKRANIAN;
            static constexpr uint16_t English_Canadian = TWLG_ENGLISH_CANADIAN;
            static constexpr uint16_t English_Ireland = TWLG_ENGLISH_IRELAND;
            static constexpr uint16_t English_Newzealand = TWLG_ENGLISH_NEWZEALAND;
            static constexpr uint16_t English_Southafrica = TWLG_ENGLISH_SOUTHAFRICA;
            static constexpr uint16_t English_UK = TWLG_ENGLISH_UK;
            static constexpr uint16_t English_USA = TWLG_ENGLISH_USA;
            static constexpr uint16_t Estonian = TWLG_ESTONIAN;
            static constexpr uint16_t Faeroese = TWLG_FAEROESE;
            static constexpr uint16_t Farsi = TWLG_FARSI;
            static constexpr uint16_t Finnish = TWLG_FINNISH;
            static constexpr uint16_t French = TWLG_FRENCH;
            static constexpr uint16_t French_Belgian = TWLG_FRENCH_BELGIAN;
            static constexpr uint16_t French_Canadian = TWLG_FRENCH_CANADIAN;
            static constexpr uint16_t Assamese = TWLG_ASSAMESE;
            static constexpr uint16_t Bengali = TWLG_BENGALI;
            static constexpr uint16_t Bihari = TWLG_BIHARI;
            static constexpr uint16_t Bodo = TWLG_BODO;
            static constexpr uint16_t Dogri = TWLG_DOGRI;
            static constexpr uint16_t Gujarati = TWLG_GUJARATI;
            static constexpr uint16_t Haryanvi = TWLG_HARYANVI;
            static constexpr uint16_t Hindi = TWLG_HINDI;
            static constexpr uint16_t Kannada = TWLG_KANNADA;
            static constexpr uint16_t Kashmiri = TWLG_KASHMIRI;
            static constexpr uint16_t Malayalam = TWLG_MALAYALAM;
            static constexpr uint16_t Marathi = TWLG_MARATHI;
            static constexpr uint16_t Marwari = TWLG_MARWARI;
            static constexpr uint16_t Meghalayan = TWLG_MEGHALAYAN;
            static constexpr uint16_t Mizo = TWLG_MIZO;
            static constexpr uint16_t Naga = TWLG_NAGA;
            static constexpr uint16_t Orissi = TWLG_ORISSI;
            static constexpr uint16_t Punjabi = TWLG_PUNJABI;
            static constexpr uint16_t Pushtu = TWLG_PUSHTU;
            static constexpr uint16_t Serbian_Cyrillic = TWLG_SERBIAN_CYRILLIC;
            static constexpr uint16_t Sikkimi = TWLG_SIKKIMI;
            static constexpr uint16_t Swedish_Finland = TWLG_SWEDISH_FINLAND;
            static constexpr uint16_t Tamil = TWLG_TAMIL;
            static constexpr uint16_t Telugu = TWLG_TELUGU;
            static constexpr uint16_t Tripuri = TWLG_TRIPURI;
            static constexpr uint16_t Urdu = TWLG_URDU;
            static constexpr uint16_t Vietnamese = TWLG_VIETNAMESE;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(Userlocale, TWLG_USERLOCALE)
            DTWAIN_ADD_MAP_ENTRY(Dan, TWLG_DAN)
            DTWAIN_ADD_MAP_ENTRY(Dut, TWLG_DUT)
            DTWAIN_ADD_MAP_ENTRY(Eng, TWLG_ENG)
            DTWAIN_ADD_MAP_ENTRY(Fcf, TWLG_FCF)
            DTWAIN_ADD_MAP_ENTRY(Fin, TWLG_FIN)
            DTWAIN_ADD_MAP_ENTRY(Frn, TWLG_FRN)
            DTWAIN_ADD_MAP_ENTRY(Ger, TWLG_GER)
            DTWAIN_ADD_MAP_ENTRY(Ice, TWLG_ICE)
            DTWAIN_ADD_MAP_ENTRY(Itn, TWLG_ITN)
            DTWAIN_ADD_MAP_ENTRY(Nor, TWLG_NOR)
            DTWAIN_ADD_MAP_ENTRY(Por, TWLG_POR)
            DTWAIN_ADD_MAP_ENTRY(Spa, TWLG_SPA)
            DTWAIN_ADD_MAP_ENTRY(Swe, TWLG_SWE)
            DTWAIN_ADD_MAP_ENTRY(Usa, TWLG_USA)
            DTWAIN_ADD_MAP_ENTRY(Afrikaans, TWLG_AFRIKAANS)
            DTWAIN_ADD_MAP_ENTRY(Albania, TWLG_ALBANIA)
            DTWAIN_ADD_MAP_ENTRY(Arabic, TWLG_ARABIC)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Algeria, TWLG_ARABIC_ALGERIA)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Bahrain, TWLG_ARABIC_BAHRAIN)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Egypt, TWLG_ARABIC_EGYPT)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Iraq, TWLG_ARABIC_IRAQ)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Jordan, TWLG_ARABIC_JORDAN)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Kuwait, TWLG_ARABIC_KUWAIT)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Lebanon, TWLG_ARABIC_LEBANON)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Libya, TWLG_ARABIC_LIBYA)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Morocco, TWLG_ARABIC_MOROCCO)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Oman, TWLG_ARABIC_OMAN)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Qatar, TWLG_ARABIC_QATAR)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Saudiarabia, TWLG_ARABIC_SAUDIARABIA)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Syria, TWLG_ARABIC_SYRIA)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Tunisia, TWLG_ARABIC_TUNISIA)
            DTWAIN_ADD_MAP_ENTRY(French_Luxembourg, TWLG_FRENCH_LUXEMBOURG)
            DTWAIN_ADD_MAP_ENTRY(French_Swiss, TWLG_FRENCH_SWISS)
            DTWAIN_ADD_MAP_ENTRY(German, TWLG_GERMAN)
            DTWAIN_ADD_MAP_ENTRY(German_Austrian, TWLG_GERMAN_AUSTRIAN)
            DTWAIN_ADD_MAP_ENTRY(German_Luxembourg, TWLG_GERMAN_LUXEMBOURG)
            DTWAIN_ADD_MAP_ENTRY(German_Liechtenstein, TWLG_GERMAN_LIECHTENSTEIN)
            DTWAIN_ADD_MAP_ENTRY(German_Swiss, TWLG_GERMAN_SWISS)
            DTWAIN_ADD_MAP_ENTRY(Greek, TWLG_GREEK)
            DTWAIN_ADD_MAP_ENTRY(Hebrew, TWLG_HEBREW)
            DTWAIN_ADD_MAP_ENTRY(Hungarian, TWLG_HUNGARIAN)
            DTWAIN_ADD_MAP_ENTRY(Icelandic, TWLG_ICELANDIC)
            DTWAIN_ADD_MAP_ENTRY(Indonesian, TWLG_INDONESIAN)
            DTWAIN_ADD_MAP_ENTRY(Italian, TWLG_ITALIAN)
            DTWAIN_ADD_MAP_ENTRY(Italian_Swiss, TWLG_ITALIAN_SWISS)
            DTWAIN_ADD_MAP_ENTRY(Japanese, TWLG_JAPANESE)
            DTWAIN_ADD_MAP_ENTRY(Korean, TWLG_KOREAN)
            DTWAIN_ADD_MAP_ENTRY(Korean_Johab, TWLG_KOREAN_JOHAB)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Uae, TWLG_ARABIC_UAE)
            DTWAIN_ADD_MAP_ENTRY(Arabic_Yemen, TWLG_ARABIC_YEMEN)
            DTWAIN_ADD_MAP_ENTRY(Basque, TWLG_BASQUE)
            DTWAIN_ADD_MAP_ENTRY(Byelorussian, TWLG_BYELORUSSIAN)
            DTWAIN_ADD_MAP_ENTRY(Bulgarian, TWLG_BULGARIAN)
            DTWAIN_ADD_MAP_ENTRY(Catalan, TWLG_CATALAN)
            DTWAIN_ADD_MAP_ENTRY(Chinese, TWLG_CHINESE)
            DTWAIN_ADD_MAP_ENTRY(Chinese_Hongkong, TWLG_CHINESE_HONGKONG)
            DTWAIN_ADD_MAP_ENTRY(Chinese_Prc, TWLG_CHINESE_PRC)
            DTWAIN_ADD_MAP_ENTRY(Chinese_Singapore, TWLG_CHINESE_SINGAPORE)
            DTWAIN_ADD_MAP_ENTRY(Chinese_Simplified, TWLG_CHINESE_SIMPLIFIED)
            DTWAIN_ADD_MAP_ENTRY(Chinese_Taiwan, TWLG_CHINESE_TAIWAN)
            DTWAIN_ADD_MAP_ENTRY(Chinese_Traditional, TWLG_CHINESE_TRADITIONAL)
            DTWAIN_ADD_MAP_ENTRY(Croatia, TWLG_CROATIA)
            DTWAIN_ADD_MAP_ENTRY(Czech, TWLG_CZECH)
            DTWAIN_ADD_MAP_ENTRY(Danish, TWLG_DANISH)
            DTWAIN_ADD_MAP_ENTRY(Dutch, TWLG_DUTCH)
            DTWAIN_ADD_MAP_ENTRY(Dutch_Belgian, TWLG_DUTCH_BELGIAN)
            DTWAIN_ADD_MAP_ENTRY(English, TWLG_ENGLISH)
            DTWAIN_ADD_MAP_ENTRY(English_Australian, TWLG_ENGLISH_AUSTRALIAN)
            DTWAIN_ADD_MAP_ENTRY(Latvian, TWLG_LATVIAN)
            DTWAIN_ADD_MAP_ENTRY(Lithuanian, TWLG_LITHUANIAN)
            DTWAIN_ADD_MAP_ENTRY(Norwegian, TWLG_NORWEGIAN)
            DTWAIN_ADD_MAP_ENTRY(Norwegian_Bokmal, TWLG_NORWEGIAN_BOKMAL)
            DTWAIN_ADD_MAP_ENTRY(Norwegian_Nynorsk, TWLG_NORWEGIAN_NYNORSK)
            DTWAIN_ADD_MAP_ENTRY(Polish, TWLG_POLISH)
            DTWAIN_ADD_MAP_ENTRY(Portuguese, TWLG_PORTUGUESE)
            DTWAIN_ADD_MAP_ENTRY(Portuguese_Brazil, TWLG_PORTUGUESE_BRAZIL)
            DTWAIN_ADD_MAP_ENTRY(Romanian, TWLG_ROMANIAN)
            DTWAIN_ADD_MAP_ENTRY(Russian, TWLG_RUSSIAN)
            DTWAIN_ADD_MAP_ENTRY(Serbian_Latin, TWLG_SERBIAN_LATIN)
            DTWAIN_ADD_MAP_ENTRY(Slovak, TWLG_SLOVAK)
            DTWAIN_ADD_MAP_ENTRY(Slovenian, TWLG_SLOVENIAN)
            DTWAIN_ADD_MAP_ENTRY(Spanish, TWLG_SPANISH)
            DTWAIN_ADD_MAP_ENTRY(Spanish_Mexican, TWLG_SPANISH_MEXICAN)
            DTWAIN_ADD_MAP_ENTRY(Spanish_Modern, TWLG_SPANISH_MODERN)
            DTWAIN_ADD_MAP_ENTRY(Swedish, TWLG_SWEDISH)
            DTWAIN_ADD_MAP_ENTRY(Thai, TWLG_THAI)
            DTWAIN_ADD_MAP_ENTRY(Turkish, TWLG_TURKISH)
            DTWAIN_ADD_MAP_ENTRY(Ukranian, TWLG_UKRANIAN)
            DTWAIN_ADD_MAP_ENTRY(English_Canadian, TWLG_ENGLISH_CANADIAN)
            DTWAIN_ADD_MAP_ENTRY(English_Ireland, TWLG_ENGLISH_IRELAND)
            DTWAIN_ADD_MAP_ENTRY(English_Newzealand, TWLG_ENGLISH_NEWZEALAND)
            DTWAIN_ADD_MAP_ENTRY(English_Southafrica, TWLG_ENGLISH_SOUTHAFRICA)
            DTWAIN_ADD_MAP_ENTRY(English_UK, TWLG_ENGLISH_UK)
            DTWAIN_ADD_MAP_ENTRY(English_USA, TWLG_ENGLISH_USA)
            DTWAIN_ADD_MAP_ENTRY(Estonian, TWLG_ESTONIAN)
            DTWAIN_ADD_MAP_ENTRY(Faeroese, TWLG_FAEROESE)
            DTWAIN_ADD_MAP_ENTRY(Farsi, TWLG_FARSI)
            DTWAIN_ADD_MAP_ENTRY(Finnish, TWLG_FINNISH)
            DTWAIN_ADD_MAP_ENTRY(French, TWLG_FRENCH)
            DTWAIN_ADD_MAP_ENTRY(French_Belgian, TWLG_FRENCH_BELGIAN)
            DTWAIN_ADD_MAP_ENTRY(French_Canadian, TWLG_FRENCH_CANADIAN)
            DTWAIN_ADD_MAP_ENTRY(Assamese, TWLG_ASSAMESE)
            DTWAIN_ADD_MAP_ENTRY(Bengali, TWLG_BENGALI)
            DTWAIN_ADD_MAP_ENTRY(Bihari, TWLG_BIHARI)
            DTWAIN_ADD_MAP_ENTRY(Bodo, TWLG_BODO)
            DTWAIN_ADD_MAP_ENTRY(Dogri, TWLG_DOGRI)
            DTWAIN_ADD_MAP_ENTRY(Gujarati, TWLG_GUJARATI)
            DTWAIN_ADD_MAP_ENTRY(Haryanvi, TWLG_HARYANVI)
            DTWAIN_ADD_MAP_ENTRY(Hindi, TWLG_HINDI)
            DTWAIN_ADD_MAP_ENTRY(Kannada, TWLG_KANNADA)
            DTWAIN_ADD_MAP_ENTRY(Kashmiri, TWLG_KASHMIRI)
            DTWAIN_ADD_MAP_ENTRY(Malayalam, TWLG_MALAYALAM)
            DTWAIN_ADD_MAP_ENTRY(Marathi, TWLG_MARATHI)
            DTWAIN_ADD_MAP_ENTRY(Marwari, TWLG_MARWARI)
            DTWAIN_ADD_MAP_ENTRY(Meghalayan, TWLG_MEGHALAYAN)
            DTWAIN_ADD_MAP_ENTRY(Mizo, TWLG_MIZO)
            DTWAIN_ADD_MAP_ENTRY(Naga, TWLG_NAGA)
            DTWAIN_ADD_MAP_ENTRY(Orissi, TWLG_ORISSI)
            DTWAIN_ADD_MAP_ENTRY(Punjabi, TWLG_PUNJABI)
            DTWAIN_ADD_MAP_ENTRY(Pushtu, TWLG_PUSHTU)
            DTWAIN_ADD_MAP_ENTRY(Serbian_Cyrillic, TWLG_SERBIAN_CYRILLIC)
            DTWAIN_ADD_MAP_ENTRY(Sikkimi, TWLG_SIKKIMI)
            DTWAIN_ADD_MAP_ENTRY(Swedish_Finland, TWLG_SWEDISH_FINLAND)
            DTWAIN_ADD_MAP_ENTRY(Tamil, TWLG_TAMIL)
            DTWAIN_ADD_MAP_ENTRY(Telugu, TWLG_TELUGU)
            DTWAIN_ADD_MAP_ENTRY(Tripuri, TWLG_TRIPURI)
            DTWAIN_ADD_MAP_ENTRY(Urdu, TWLG_URDU)
            DTWAIN_ADD_MAP_ENTRY(Vietnamese, TWLG_VIETNAMESE)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };


        struct lightpath_value
        {
            static constexpr uint16_t reflective = TWLP_REFLECTIVE;
            static constexpr uint16_t transmissive = TWLP_TRANSMISSIVE;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t  value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(reflective,TWLP_REFLECTIVE)
            DTWAIN_ADD_MAP_ENTRY(transmissive,TWLP_TRANSMISSIVE)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct lightsource_value
        {
            static constexpr uint16_t red   = TWLS_RED;
            static constexpr uint16_t green = TWLS_GREEN;
            static constexpr uint16_t blue  = TWLS_BLUE;
            static constexpr uint16_t none  = TWLS_NONE;
            static constexpr uint16_t white = TWLS_WHITE;
            static constexpr uint16_t uv    = TWLS_UV;
            static constexpr uint16_t ir    = TWLS_IR;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(red,TWLS_RED)
            DTWAIN_ADD_MAP_ENTRY(green,TWLS_GREEN)
            DTWAIN_ADD_MAP_ENTRY(blue,TWLS_BLUE)
            DTWAIN_ADD_MAP_ENTRY(none,TWLS_NONE)
            DTWAIN_ADD_MAP_ENTRY(white,TWLS_WHITE)
            DTWAIN_ADD_MAP_ENTRY(uv,TWLS_UV)
            DTWAIN_ADD_MAP_ENTRY(ir,TWLS_IR)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct mirror_value
        {
            static constexpr uint16_t none          = TWMR_NONE;
            static constexpr uint16_t vertical      = TWMR_VERTICAL;
            static constexpr uint16_t horizontal    = TWMR_HORIZONTAL;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none, TWMR_NONE)
            DTWAIN_ADD_MAP_ENTRY(vertical, TWMR_VERTICAL)
            DTWAIN_ADD_MAP_ENTRY(horizontal, TWMR_HORIZONTAL)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct noisefilter_value
        {
            static constexpr uint16_t none          = TWNF_NONE;
            static constexpr uint16_t automatic     = TWNF_AUTO;
            static constexpr uint16_t lonepixel     = TWNF_LONEPIXEL;
            static constexpr uint16_t majorityrule  = TWNF_MAJORITYRULE;
            static constexpr uint16_t default_val   = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none,TWNF_NONE)
            DTWAIN_ADD_MAP_ENTRY(automatic,TWNF_AUTO)
            DTWAIN_ADD_MAP_ENTRY(lonepixel,TWNF_LONEPIXEL)
            DTWAIN_ADD_MAP_ENTRY(majorityrule,TWNF_MAJORITYRULE)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct orientation_value
        {
            static constexpr uint16_t orient_0      = TWOR_ROT0;
            static constexpr uint16_t orient_90     = TWOR_ROT90;
            static constexpr uint16_t orient_180    = TWOR_ROT180;
            static constexpr uint16_t orient_270    = TWOR_ROT270;
            static constexpr uint16_t portrait      = TWOR_ROT0; 
            static constexpr uint16_t landscape     = TWOR_ROT90;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(orient_0, TWOR_ROT0)
            DTWAIN_ADD_MAP_ENTRY(orient_90, TWOR_ROT90)
            DTWAIN_ADD_MAP_ENTRY(orient_180, TWOR_ROT180)
            DTWAIN_ADD_MAP_ENTRY(orient_270, TWOR_ROT270)
            DTWAIN_ADD_MAP_ENTRY(portrait, TWOR_ROT0)
            DTWAIN_ADD_MAP_ENTRY(landscape, TWOR_ROT90)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct overscan_value
        {
            static constexpr uint16_t none      = TWOV_NONE;
            static constexpr uint16_t automatic = TWOV_AUTO;
            static constexpr uint16_t topbottom = TWOV_TOPBOTTOM;
            static constexpr uint16_t leftright = TWOV_LEFTRIGHT;
            static constexpr uint16_t all       = TWOV_ALL;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none,TWOV_NONE)
            DTWAIN_ADD_MAP_ENTRY(automatic,TWOV_AUTO)
            DTWAIN_ADD_MAP_ENTRY(topbottom,TWOV_TOPBOTTOM)
            DTWAIN_ADD_MAP_ENTRY(leftright,TWOV_LEFTRIGHT)
            DTWAIN_ADD_MAP_ENTRY(all,TWOV_ALL)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct paperhandling_value
        {
            static constexpr uint16_t normal    = TWPH_NORMAL;
            static constexpr uint16_t fragile   = TWPH_FRAGILE;
            static constexpr uint16_t thick     = TWPH_THICK;
            static constexpr uint16_t trifold   = TWPH_TRIFOLD;
            static constexpr uint16_t photograph = TWPH_PHOTOGRAPH;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(normal, TWPH_NORMAL)
            DTWAIN_ADD_MAP_ENTRY(fragile, TWPH_FRAGILE)
            DTWAIN_ADD_MAP_ENTRY(thick, TWPH_THICK)
            DTWAIN_ADD_MAP_ENTRY(trifold, TWPH_TRIFOLD)
            DTWAIN_ADD_MAP_ENTRY(photograph, TWPH_PHOTOGRAPH)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct supportedsizes_value
        {
            static constexpr uint16_t A0 = TWSS_A0;
            static constexpr uint16_t A1 = TWSS_A1;
            static constexpr uint16_t A10 = TWSS_A10;
            static constexpr uint16_t A2 = TWSS_A2;
            static constexpr uint16_t A3 = TWSS_A3;
            static constexpr uint16_t A4 = TWSS_A4;
            static constexpr uint16_t A4LETTER = TWSS_A4LETTER;
            static constexpr uint16_t A5 = TWSS_A5;
            static constexpr uint16_t A6 = TWSS_A6;
            static constexpr uint16_t A7 = TWSS_A7;
            static constexpr uint16_t A8 = TWSS_A8;
            static constexpr uint16_t A9 = TWSS_A9;
            static constexpr uint16_t B3 = TWSS_B3;
            static constexpr uint16_t B4 = TWSS_B4;
            static constexpr uint16_t B5LETTER = TWSS_B5LETTER;
            static constexpr uint16_t B6 = TWSS_B6;
            static constexpr uint16_t BUSINESSCARD = TWSS_BUSINESSCARD;
            static constexpr uint16_t C0 = TWSS_C0;
            static constexpr uint16_t C1 = TWSS_C1;
            static constexpr uint16_t C10 = TWSS_C10;
            static constexpr uint16_t C2 = TWSS_C2;
            static constexpr uint16_t C3 = TWSS_C3;
            static constexpr uint16_t C4 = TWSS_C4;
            static constexpr uint16_t C5 = TWSS_C5;
            static constexpr uint16_t C6 = TWSS_C6;
            static constexpr uint16_t C7 = TWSS_C7;
            static constexpr uint16_t C8 = TWSS_C8;
            static constexpr uint16_t C9 = TWSS_C9;
            static constexpr uint16_t FOURA0 = TWSS_4A0;
            static constexpr uint16_t ISOB0 = TWSS_ISOB0;
            static constexpr uint16_t ISOB1 = TWSS_ISOB1;
            static constexpr uint16_t ISOB10 = TWSS_ISOB10;
            static constexpr uint16_t ISOB2 = TWSS_ISOB2;
            static constexpr uint16_t ISOB3 = TWSS_ISOB3;
            static constexpr uint16_t ISOB4 = TWSS_ISOB4;
            static constexpr uint16_t ISOB5 = TWSS_ISOB5;
            static constexpr uint16_t ISOB6 = TWSS_ISOB6;
            static constexpr uint16_t ISOB7 = TWSS_ISOB7;
            static constexpr uint16_t ISOB8 = TWSS_ISOB8;
            static constexpr uint16_t ISOB9 = TWSS_ISOB9;
            static constexpr uint16_t JISB0 = TWSS_JISB0;
            static constexpr uint16_t JISB1 = TWSS_JISB1;
            static constexpr uint16_t JISB10 = TWSS_JISB10;
            static constexpr uint16_t JISB2 = TWSS_JISB2;
            static constexpr uint16_t JISB3 = TWSS_JISB3;
            static constexpr uint16_t JISB4 = TWSS_JISB4;
            static constexpr uint16_t JISB5 = TWSS_JISB5;
            static constexpr uint16_t JISB6 = TWSS_JISB6;
            static constexpr uint16_t JISB7 = TWSS_JISB7;
            static constexpr uint16_t JISB8 = TWSS_JISB8;
            static constexpr uint16_t JISB9 = TWSS_JISB9;
            static constexpr uint16_t MAXSIZE = TWSS_MAXSIZE;
            static constexpr uint16_t NONE = TWSS_NONE;
            static constexpr uint16_t TWOA0 = TWSS_2A0;
            static constexpr uint16_t USEXECUTIVE = TWSS_USEXECUTIVE;
            static constexpr uint16_t USLEDGER = TWSS_USLEDGER;
            static constexpr uint16_t USLEGAL = TWSS_USLEGAL;
            static constexpr uint16_t USLETTER = TWSS_USLETTER;
            static constexpr uint16_t USSTATEMENT = TWSS_USSTATEMENT;
            static constexpr uint16_t CUSTOM = DTWAIN_PDF_CUSTOMSIZE;
            static constexpr uint16_t VARIABLE = DTWAIN_PDF_VARIABLEPAGESIZE;
            static constexpr uint16_t PIXELSPERMETER = DTWAIN_PDF_PIXELSPERMETERSIZE;
            static constexpr uint16_t default_val = (std::numeric_limits<uint16_t>::max)();
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(A0, TWSS_A0)
            DTWAIN_ADD_MAP_ENTRY(A1, TWSS_A1)
            DTWAIN_ADD_MAP_ENTRY(A10, TWSS_A10)
            DTWAIN_ADD_MAP_ENTRY(A2, TWSS_A2)
            DTWAIN_ADD_MAP_ENTRY(A3, TWSS_A3)
            DTWAIN_ADD_MAP_ENTRY(A4, TWSS_A4)
            DTWAIN_ADD_MAP_ENTRY(A4LETTER, TWSS_A4LETTER)
            DTWAIN_ADD_MAP_ENTRY(A5, TWSS_A5)
            DTWAIN_ADD_MAP_ENTRY(A6, TWSS_A6)
            DTWAIN_ADD_MAP_ENTRY(A7, TWSS_A7)
            DTWAIN_ADD_MAP_ENTRY(A8, TWSS_A8)
            DTWAIN_ADD_MAP_ENTRY(A9, TWSS_A9)
            DTWAIN_ADD_MAP_ENTRY(B3, TWSS_B3)
            DTWAIN_ADD_MAP_ENTRY(B4, TWSS_B4)
            DTWAIN_ADD_MAP_ENTRY(B5LETTER, TWSS_B5LETTER)
            DTWAIN_ADD_MAP_ENTRY(B6, TWSS_B6)
            DTWAIN_ADD_MAP_ENTRY(BUSINESSCARD, TWSS_BUSINESSCARD)
            DTWAIN_ADD_MAP_ENTRY(C0, TWSS_C0)
            DTWAIN_ADD_MAP_ENTRY(C1, TWSS_C1)
            DTWAIN_ADD_MAP_ENTRY(C10, TWSS_C10)
            DTWAIN_ADD_MAP_ENTRY(C2, TWSS_C2)
            DTWAIN_ADD_MAP_ENTRY(C3, TWSS_C3)
            DTWAIN_ADD_MAP_ENTRY(C4, TWSS_C4)
            DTWAIN_ADD_MAP_ENTRY(C5, TWSS_C5)
            DTWAIN_ADD_MAP_ENTRY(C6, TWSS_C6)
            DTWAIN_ADD_MAP_ENTRY(C7, TWSS_C7)
            DTWAIN_ADD_MAP_ENTRY(C8, TWSS_C8)
            DTWAIN_ADD_MAP_ENTRY(C9, TWSS_C9)
            DTWAIN_ADD_MAP_ENTRY(FOURA0, TWSS_4A0)
            DTWAIN_ADD_MAP_ENTRY(ISOB0, TWSS_ISOB0)
            DTWAIN_ADD_MAP_ENTRY(ISOB1, TWSS_ISOB1)
            DTWAIN_ADD_MAP_ENTRY(ISOB10, TWSS_ISOB10)
            DTWAIN_ADD_MAP_ENTRY(ISOB2, TWSS_ISOB2)
            DTWAIN_ADD_MAP_ENTRY(ISOB3, TWSS_ISOB3)
            DTWAIN_ADD_MAP_ENTRY(ISOB4, TWSS_ISOB4)
            DTWAIN_ADD_MAP_ENTRY(ISOB5, TWSS_ISOB5)
            DTWAIN_ADD_MAP_ENTRY(ISOB6, TWSS_ISOB6)
            DTWAIN_ADD_MAP_ENTRY(ISOB7, TWSS_ISOB7)
            DTWAIN_ADD_MAP_ENTRY(ISOB8, TWSS_ISOB8)
            DTWAIN_ADD_MAP_ENTRY(ISOB9, TWSS_ISOB9)
            DTWAIN_ADD_MAP_ENTRY(JISB0, TWSS_JISB0)
            DTWAIN_ADD_MAP_ENTRY(JISB1, TWSS_JISB1)
            DTWAIN_ADD_MAP_ENTRY(JISB10, TWSS_JISB10)
            DTWAIN_ADD_MAP_ENTRY(JISB2, TWSS_JISB2)
            DTWAIN_ADD_MAP_ENTRY(JISB3, TWSS_JISB3)
            DTWAIN_ADD_MAP_ENTRY(JISB4, TWSS_JISB4)
            DTWAIN_ADD_MAP_ENTRY(JISB5, TWSS_JISB5)
            DTWAIN_ADD_MAP_ENTRY(JISB6, TWSS_JISB6)
            DTWAIN_ADD_MAP_ENTRY(JISB7, TWSS_JISB7)
            DTWAIN_ADD_MAP_ENTRY(JISB8, TWSS_JISB8)
            DTWAIN_ADD_MAP_ENTRY(JISB9, TWSS_JISB9)
            DTWAIN_ADD_MAP_ENTRY(MAXSIZE, TWSS_MAXSIZE)
            DTWAIN_ADD_MAP_ENTRY(NONE, TWSS_NONE)
            DTWAIN_ADD_MAP_ENTRY(TWOA0, TWSS_2A0)
            DTWAIN_ADD_MAP_ENTRY(USEXECUTIVE, TWSS_USEXECUTIVE)
            DTWAIN_ADD_MAP_ENTRY(USLEDGER, TWSS_USLEDGER)
            DTWAIN_ADD_MAP_ENTRY(USLEGAL, TWSS_USLEGAL)
            DTWAIN_ADD_MAP_ENTRY(USLETTER, TWSS_USLETTER)
            DTWAIN_ADD_MAP_ENTRY(USSTATEMENT, TWSS_USSTATEMENT)
            DTWAIN_ADD_MAP_ENTRY(CUSTOM, DTWAIN_PDF_CUSTOMSIZE)
            DTWAIN_ADD_MAP_ENTRY(VARIABLE, DTWAIN_PDF_VARIABLEPAGESIZE)
            DTWAIN_ADD_MAP_ENTRY(PIXELSPERMETER, DTWAIN_PDF_PIXELSPERMETERSIZE)
            DTWAIN_ADD_MAP_ENTRY(default_val, (std::numeric_limits<uint16_t>::max)())
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct patchcodesearchmode_value
        {
            static constexpr uint16_t horz      = TWBD_HORZ;
            static constexpr uint16_t vert      = TWBD_VERT;
            static constexpr uint16_t horzvert  = TWBD_HORZVERT;
            static constexpr uint16_t verthorz  = TWBD_VERTHORZ;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(horz, TWBD_HORZ)
            DTWAIN_ADD_MAP_ENTRY(vert, TWBD_VERT)
            DTWAIN_ADD_MAP_ENTRY(horzvert, TWBD_HORZVERT)
            DTWAIN_ADD_MAP_ENTRY(verthorz, TWBD_VERTHORZ)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct patchcodetype_value
        {
            static constexpr uint16_t patch1 = TWPCH_PATCH1;
            static constexpr uint16_t patch2 = TWPCH_PATCH2;
            static constexpr uint16_t patch3 = TWPCH_PATCH3;
            static constexpr uint16_t patch4 = TWPCH_PATCH4;
            static constexpr uint16_t patch6 = TWPCH_PATCH6;
            static constexpr uint16_t patcht = TWPCH_PATCHT;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(patch1, TWPCH_PATCH1)
            DTWAIN_ADD_MAP_ENTRY(patch2, TWPCH_PATCH2)
            DTWAIN_ADD_MAP_ENTRY(patch3, TWPCH_PATCH3)
            DTWAIN_ADD_MAP_ENTRY(patch4, TWPCH_PATCH4)
            DTWAIN_ADD_MAP_ENTRY(patch6, TWPCH_PATCH6)
            DTWAIN_ADD_MAP_ENTRY(patcht, TWPCH_PATCHT)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct pixelflavor_value
        {
            static constexpr uint16_t chocolate = TWPF_CHOCOLATE;
            static constexpr uint16_t vanilla   = TWPF_VANILLA;
            static constexpr uint16_t default_val = (std::numeric_limits<uint16_t>::max)();
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(chocolate, TWPF_CHOCOLATE)
            DTWAIN_ADD_MAP_ENTRY(vanilla, TWPF_VANILLA)
            DTWAIN_ADD_MAP_ENTRY(default_val, (std::numeric_limits<uint16_t>::max)())
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct planarchunky_value
        {
            static constexpr uint16_t chunky = TWPC_CHUNKY;
            static constexpr uint16_t planar = TWPC_PLANAR;
            static constexpr uint16_t default_val = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(chunky, TWPC_CHUNKY)
            DTWAIN_ADD_MAP_ENTRY(planar, TWPC_PLANAR)
            DTWAIN_ADD_MAP_ENTRY(default_val, 1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct powersupply_value
        {
            static constexpr uint16_t external  = TWPS_EXTERNAL;
            static constexpr uint16_t battery   = TWPS_BATTERY;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(external, TWPS_EXTERNAL)
            DTWAIN_ADD_MAP_ENTRY(battery, TWPS_BATTERY)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct printertype_value
        {
            static constexpr uint16_t imprintertopbefore    = TWPR_IMPRINTERTOPBEFORE;
            static constexpr uint16_t imprintertopafter     = TWPR_IMPRINTERTOPAFTER;
            static constexpr uint16_t imprinterbottombefore = TWPR_IMPRINTERBOTTOMBEFORE;
            static constexpr uint16_t imprinterbottomafter  = TWPR_IMPRINTERBOTTOMAFTER;
            static constexpr uint16_t endorsertopbefore     = TWPR_ENDORSERTOPBEFORE;
            static constexpr uint16_t endorsertopafter      = TWPR_ENDORSERTOPAFTER;
            static constexpr uint16_t endorserbottombefore  = TWPR_ENDORSERBOTTOMBEFORE;
            static constexpr uint16_t endorserbottomafter   = TWPR_ENDORSERBOTTOMAFTER;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(imprintertopbefore, TWPR_IMPRINTERTOPBEFORE)
            DTWAIN_ADD_MAP_ENTRY(imprintertopafter, TWPR_IMPRINTERTOPAFTER)
            DTWAIN_ADD_MAP_ENTRY(imprinterbottombefore, TWPR_IMPRINTERBOTTOMBEFORE)
            DTWAIN_ADD_MAP_ENTRY(imprinterbottomafter, TWPR_IMPRINTERBOTTOMAFTER)
            DTWAIN_ADD_MAP_ENTRY(endorsertopbefore, TWPR_ENDORSERTOPBEFORE)
            DTWAIN_ADD_MAP_ENTRY(endorsertopafter, TWPR_ENDORSERTOPAFTER)
            DTWAIN_ADD_MAP_ENTRY(endorserbottombefore, TWPR_ENDORSERBOTTOMBEFORE)
            DTWAIN_ADD_MAP_ENTRY(endorserbottomafter, TWPR_ENDORSERBOTTOMAFTER)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct segmented_value
        {
            static constexpr uint16_t none      = TWSG_NONE;
            static constexpr uint16_t automatic = TWSG_AUTO;
            static constexpr uint16_t manual    = TWSG_MANUAL;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(none, TWSG_NONE)
            DTWAIN_ADD_MAP_ENTRY(automatic, TWSG_AUTO)
            DTWAIN_ADD_MAP_ENTRY(manual, TWSG_MANUAL)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct stringmode_value
        {
            static constexpr uint16_t single    = TWPM_SINGLESTRING;
            static constexpr uint16_t multi     = TWPM_MULTISTRING;
            static constexpr uint16_t compound  = TWPM_COMPOUNDSTRING;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(single, TWPM_SINGLESTRING)
            DTWAIN_ADD_MAP_ENTRY(multi, TWPM_MULTISTRING)
            DTWAIN_ADD_MAP_ENTRY(compound, TWPM_COMPOUNDSTRING)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct units_value
        {
            static constexpr uint16_t inches        = TWUN_INCHES;
            static constexpr uint16_t centimeters   = TWUN_CENTIMETERS;
            static constexpr uint16_t picas         = TWUN_PICAS;
            static constexpr uint16_t pixels        = TWUN_PIXELS;
            static constexpr uint16_t points        = TWUN_POINTS;
            static constexpr uint16_t millimeters   = TWUN_MILLIMETERS;
            static constexpr uint16_t twips         = TWUN_TWIPS;
            static constexpr uint16_t default_val   = 1000;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(inches,TWUN_INCHES)
            DTWAIN_ADD_MAP_ENTRY(centimeters,TWUN_CENTIMETERS)
            DTWAIN_ADD_MAP_ENTRY(picas,TWUN_PICAS)
            DTWAIN_ADD_MAP_ENTRY(pixels,TWUN_PIXELS)
            DTWAIN_ADD_MAP_ENTRY(points,TWUN_POINTS)
            DTWAIN_ADD_MAP_ENTRY(millimeters,TWUN_MILLIMETERS)
            DTWAIN_ADD_MAP_ENTRY(twips,TWUN_TWIPS)
            DTWAIN_ADD_MAP_ENTRY(default_val,1000)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct xfermech_value
        {
            static constexpr uint16_t native_transfer   = TWSX_NATIVE;
            static constexpr uint16_t file_transfer     = TWSX_FILE;
            static constexpr uint16_t memory_transfer   = TWSX_MEMORY;
            static constexpr uint16_t memfile_transfer  = TWSX_MEMFILE;
            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(native_transfer,TWSX_NATIVE)
            DTWAIN_ADD_MAP_ENTRY(file_transfer,TWSX_FILE)
            DTWAIN_ADD_MAP_ENTRY(memory_transfer,TWSX_MEMORY)
            DTWAIN_ADD_MAP_ENTRY(memfile_transfer,TWSX_MEMFILE)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        enum class sourceaction_type
        {
            openafteracquire = 0,
            closeafteracquire = 1
        };

        enum class file_transfer_flags
        {
            use_name = DTWAIN_USELONGNAME,
            use_prompt = DTWAIN_USEPROMPT
        };

        enum class multipage_save_mode
        {
            save_default = DTWAIN_FILESAVE_DEFAULT,
            save_uiclose = DTWAIN_FILESAVE_UICLOSE,
            save_sourceclose = DTWAIN_FILESAVE_SOURCECLOSE,
            save_manual  = DTWAIN_FILESAVE_MANUALSAVE,
            save_endacquire = DTWAIN_FILESAVE_ENDACQUIRE,
            save_incomplete = DTWAIN_FILESAVE_SAVEINCOMPLETE
        };

        struct capoperation_value
        {
            static constexpr uint16_t get           = TWQC_GET;
            static constexpr uint16_t get_current   = TWQC_GETCURRENT;
            static constexpr uint16_t get_default   = TWQC_GETDEFAULT;
            static constexpr uint16_t set           = TWQC_SET;
            static constexpr uint16_t reset         = TWQC_RESET;
            static constexpr uint16_t set_constraint = TWQC_SETCONSTRAINT;
            static constexpr uint16_t get_help       = TWQC_GETHELP;
            static constexpr uint16_t get_label     = TWQC_GETLABEL;
            static constexpr uint16_t get_labelenum = TWQC_GETLABELENUM;

            typedef uint16_t value_type;

            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(get, TWQC_GET)
            DTWAIN_ADD_MAP_ENTRY(get_current, TWQC_GETCURRENT)
            DTWAIN_ADD_MAP_ENTRY(get_default, TWQC_GETDEFAULT)
            DTWAIN_ADD_MAP_ENTRY(set, TWQC_SET)
            DTWAIN_ADD_MAP_ENTRY(reset, TWQC_RESET)
            DTWAIN_ADD_MAP_ENTRY(set_constraint, TWQC_SETCONSTRAINT)
            DTWAIN_ADD_MAP_ENTRY(get_help, TWQC_GETHELP)
            DTWAIN_ADD_MAP_ENTRY(get_label, TWQC_GETLABEL)
            DTWAIN_ADD_MAP_ENTRY(get_labelenum, TWQC_GETLABELENUM)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(uint16_t)
        };

        struct twaindatatype_value
        {
            static constexpr int32_t TYPE_INT8 = TWTY_INT8;
            static constexpr int32_t TYPE_UINT8 = TWTY_UINT8;
            static constexpr int32_t TYPE_BOOL = TWTY_BOOL;
            static constexpr int32_t TYPE_INT16 = TWTY_INT16;
            static constexpr int32_t TYPE_INT32 = TWTY_INT32;
            static constexpr int32_t TYPE_UINT16 = TWTY_UINT16;
            static constexpr int32_t TYPE_UINT32 = TWTY_UINT32;
            static constexpr int32_t TYPE_STR32 = TWTY_STR32;
            static constexpr int32_t TYPE_STR64 = TWTY_STR64;
            static constexpr int32_t TYPE_STR128 = TWTY_STR128;
            static constexpr int32_t TYPE_STR255 = TWTY_STR255;
            static constexpr int32_t TYPE_STR1024 = TWTY_STR1024;
            static constexpr int32_t TYPE_FIX32 = TWTY_FIX32;
            static constexpr int32_t TYPE_FRAME = TWTY_FRAME;
            static constexpr int32_t TYPE_HANDLE = TWTY_HANDLE;

            typedef int32_t value_type;
        
            DTWAIN_DEFINE_STRING_MAP()
            DTWAIN_ADD_MAP_ENTRY(TYPE_INT8, TWTY_INT8)
            DTWAIN_ADD_MAP_ENTRY(TYPE_UINT8, TWTY_UINT8)
            DTWAIN_ADD_MAP_ENTRY(TYPE_BOOL, TWTY_BOOL)
            DTWAIN_ADD_MAP_ENTRY(TYPE_INT16, TWTY_INT16)
            DTWAIN_ADD_MAP_ENTRY(TYPE_INT32, TWTY_INT32)
            DTWAIN_ADD_MAP_ENTRY(TYPE_UINT16, TWTY_UINT16)
            DTWAIN_ADD_MAP_ENTRY(TYPE_UINT32, TWTY_UINT32)
            DTWAIN_ADD_MAP_ENTRY(TYPE_STR32, TWTY_STR32)
            DTWAIN_ADD_MAP_ENTRY(TYPE_STR64, TWTY_STR64)
            DTWAIN_ADD_MAP_ENTRY(TYPE_STR128, TWTY_STR128)
            DTWAIN_ADD_MAP_ENTRY(TYPE_STR255, TWTY_STR255)
            DTWAIN_ADD_MAP_ENTRY(TYPE_STR1024, TWTY_STR1024)
            DTWAIN_ADD_MAP_ENTRY(TYPE_FIX32, TWTY_FIX32)
            DTWAIN_ADD_MAP_ENTRY(TYPE_FRAME, TWTY_FRAME)
            DTWAIN_ADD_MAP_ENTRY(TYPE_HANDLE, TWTY_HANDLE)
            DTWAIN_END_STRING_MAP()
            DTWAIN_ADD_TO_STRING1(int32_t)
        };
        
        class multipage_save_options
        {
            multipage_save_mode m_save_mode;
            bool m_save_incomplete;
            public:
                multipage_save_options() : m_save_mode(multipage_save_mode::save_default), m_save_incomplete(false) {}
                multipage_save_options& set_save_mode(multipage_save_mode mt) { m_save_mode = mt; return *this; }
                multipage_save_options& set_save_incomplete(bool bSet = true) { m_save_incomplete = bSet; return *this; }
                multipage_save_mode  get_save_mode() const { return m_save_mode; }
                bool is_save_incomplete() const { return m_save_incomplete; }
        };

        // Information on various file types
        struct file_type_info
        {
            static constexpr filetype_value::value_type aSingle[] =
            {
                filetype_value::bmp,
                filetype_value::dcx,
                filetype_value::enhancedmetafile,
                filetype_value::gif,
                filetype_value::googlewebp,
                filetype_value::jpeg,
                filetype_value::jpeg2k,
                filetype_value::pcx,
                filetype_value::pdf,
                filetype_value::png,
                filetype_value::postscript1,
                filetype_value::postscript2,
                filetype_value::postscript3,
                filetype_value::psd,
                filetype_value::targa,
                filetype_value::text,
                filetype_value::tiffdeflate,
                filetype_value::tiffgroup3,
                filetype_value::tiffgroup4,
                filetype_value::tiffjpeg,
                filetype_value::tifflzw,
                filetype_value::tiffnocompress,
                filetype_value::tiffpackbits,
                filetype_value::windowsicon,
                filetype_value::windowsiconresized,
                filetype_value::windowsmetafile,
                filetype_value::wirelessbmp,
                filetype_value::wirelessbmpresized
            };

            static constexpr filetype_value::value_type aMulti [] = 
            {
                filetype_value::postscript1multi, 
                filetype_value::postscript2multi, 
                filetype_value::postscript3multi,
                filetype_value::tiffdeflatemulti,
                filetype_value::tiffgroup3multi,
                filetype_value::tiffgroup4multi,
                filetype_value::tiffjpegmulti, 
                filetype_value::tifflzwmulti, 
                filetype_value::tiffnocompressmulti,
                filetype_value::tiffpackbitsmulti,
                filetype_value::pdfmulti,
                filetype_value::dcxmulti
            };

            static bool is_multipage_type(filetype_value::value_type ft)
            {
                return std::find(std::begin(aMulti), std::end(aMulti), ft) != std::end(aMulti);
            }

            static bool is_singlepage_type(filetype_value::value_type ft)
            {
                return std::find(std::begin(aSingle), std::end(aSingle), ft) != std::end(aSingle);
            }

            static int32_t get_multipage_type(filetype_value::value_type ft)
            {
                static std::unordered_map<filetype_value::value_type, int32_t> multipage_map =
                {
                    {filetype_value::dcx, DTWAIN_DCX},
                    {filetype_value::pdf, DTWAIN_PDFMULTI},
                    {filetype_value::postscript1, DTWAIN_POSTSCRIPT1MULTI},
                    {filetype_value::postscript2, DTWAIN_POSTSCRIPT2MULTI},
                    {filetype_value::postscript3, DTWAIN_POSTSCRIPT3MULTI},
                    {filetype_value::tiffdeflate, DTWAIN_TIFFDEFLATEMULTI},
                    {filetype_value::tiffgroup3, DTWAIN_TIFFG3MULTI},
                    {filetype_value::tiffgroup4, DTWAIN_TIFFG4MULTI},
                    {filetype_value::tiffjpeg, DTWAIN_TIFFJPEGMULTI},
                    {filetype_value::tifflzw, DTWAIN_TIFFLZWMULTI},
                    {filetype_value::tiffnocompress, DTWAIN_TIFFNONEMULTI},
                    {filetype_value::tiffpackbits, DTWAIN_TIFFPACKBITSMULTI},
                    {filetype_value::dcxmulti, DTWAIN_DCX},
                    {filetype_value::pdfmulti, DTWAIN_PDFMULTI},
                    {filetype_value::postscript1multi, DTWAIN_POSTSCRIPT1MULTI},
                    {filetype_value::postscript2multi, DTWAIN_POSTSCRIPT2MULTI},
                    {filetype_value::postscript3multi, DTWAIN_POSTSCRIPT3MULTI},
                    {filetype_value::tiffdeflatemulti, DTWAIN_TIFFDEFLATEMULTI},
                    {filetype_value::tiffgroup3multi, DTWAIN_TIFFG3MULTI},
                    {filetype_value::tiffgroup4multi, DTWAIN_TIFFG4MULTI},
                    {filetype_value::tiffjpegmulti, DTWAIN_TIFFJPEGMULTI},
                    {filetype_value::tifflzwmulti, DTWAIN_TIFFLZWMULTI},
                    {filetype_value::tiffnocompressmulti, DTWAIN_TIFFNONEMULTI},
                    {filetype_value::tiffpackbitsmulti, DTWAIN_TIFFPACKBITSMULTI},
                };

                auto iter = multipage_map.find(ft);
                if (iter != multipage_map.end())
                    return iter->second;
                return static_cast<int32_t>(ft);
            }

            // returns true if the image type is supported by all TWAIN devices
            static bool is_universal_support(filetype_value::value_type ft)
            {
                static std::unordered_set<filetype_value::value_type> supported_set =
                {
                    filetype_value::bmp,
                    filetype_value::dcx,
                    filetype_value::enhancedmetafile,
                    filetype_value::gif,
                    filetype_value::googlewebp,
                    filetype_value::jpeg,
                    filetype_value::jpeg2k,
                    filetype_value::pcx,
                    filetype_value::pdf,
                    filetype_value::png,
                    filetype_value::postscript1,
                    filetype_value::postscript2,
                    filetype_value::postscript3,
                    filetype_value::psd,
                    filetype_value::targa,
                    filetype_value::text,
                    filetype_value::tiffdeflate,
                    filetype_value::tiffgroup3,
                    filetype_value::tiffgroup4,
                    filetype_value::tiffjpeg,
                    filetype_value::tifflzw,
                    filetype_value::tiffnocompress,
                    filetype_value::tiffpackbits,
                    filetype_value::windowsicon,
                    filetype_value::windowsvistaicon,
                    filetype_value::windowsiconresized,
                    filetype_value::portablebitmap,
                    filetype_value::windowsmetafile,
                    filetype_value::wirelessbmp,
                    filetype_value::wirelessbmpresized,
                    filetype_value::dcxmulti,
                    filetype_value::pdfmulti,
                    filetype_value::postscript1multi,
                    filetype_value::postscript2multi,
                    filetype_value::postscript3multi,
                    filetype_value::tiffdeflatemulti,
                    filetype_value::tiffgroup3multi,
                    filetype_value::tiffgroup4multi,
                    filetype_value::tiffjpegmulti,
                    filetype_value::tifflzwmulti,
                    filetype_value::tiffnocompressmulti,
                    filetype_value::tiffpackbitsmulti
                };
                return supported_set.find(ft) != supported_set.end();
            }
        };

        struct twain_callback_values
        {
            static constexpr LONG DTWAIN_PREACQUIRE_START = -1L;
            static constexpr LONG DTWAIN_PREACQUIRE_TERMINATE = -2L;
        };

        class twain_session;
        enum class startup_mode
        {
            none,
            autostart
        };

        enum class twain_constant_category : uint16_t
        {
            TWPT,
            TWUN,
            TWCY,
            TWAL,
            TWAS,
            TWBCOR,
            TWBD,
            TWBO,
            TWBP,
            TWBR,
            TWBT,
            TWCP,
            TWCS,
            TWDE,
            TWDR,
            TWDSK,
            TWDX,
            TWFA,
            TWFE,
            TWFF,
            TWFL,
            TWFO,
            TWFP,
            TWFR,
            TWFT,
            TWFY,
            TWIA,
            TWIC,
            TWIF,
            TWIM,
            TWJC,
            TWJQ,
            TWLP,
            TWLS,
            TWMD,
            TWNF,
            TWOR,
            TWOV,
            TWPA,
            TWPC,
            TWPCH,
            TWPF,
            TWPM,
            TWPR,
            TWPF2,
            TWCT,
            TWPS,
            TWSS,
        };
        class twain_session;
        struct source_select_info
        {
            DTWAIN_SOURCE source_handle = nullptr;      /**< [out] Low-level source_handle */
            twain_session* session_handle = nullptr;    /**< [out] twain_session that opened the source */
            bool is_canceled = false;                   /**< [out] status indicator */
            bool canceled() const { return is_canceled; }
        };

        struct select_type
        {
            enum { use_orig_dialog = 0 };
            enum { use_name = 1 };
            enum { use_default = 2 };
            enum { use_dialog = 3 };
        };
    }
}
#endif