/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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

    For more information, the license file LICENSE.TXT that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef CFMTWAIN_H
#define CFMTWAIN_H

// CFM-Capabilities
#define CAP_CFMSTART                            0x0800

#define CAP_DUPLEXSCANNER                       ( CAP_CUSTOMBASE + CAP_CFMSTART + 10 )
#define CAP_DUPLEXENABLE                        ( CAP_CUSTOMBASE + CAP_CFMSTART + 11 )
#define CAP_SCANNERNAME                         ( CAP_CUSTOMBASE + CAP_CFMSTART + 12 )
#define CAP_SINGLEPASS                          ( CAP_CUSTOMBASE + CAP_CFMSTART + 13 )

#define CAP_ERRHANDLING                         ( CAP_CUSTOMBASE + CAP_CFMSTART + 20 )
#define CAP_FEEDERSTATUS                        ( CAP_CUSTOMBASE + CAP_CFMSTART + 21 )
#define CAP_FEEDMEDIUMWAIT                      ( CAP_CUSTOMBASE + CAP_CFMSTART + 22 )
#define CAP_FEEDWAITTIME                        ( CAP_CUSTOMBASE + CAP_CFMSTART + 23 )





#define ICAP_WHITEBALANCE                       ( CAP_CUSTOMBASE + CAP_CFMSTART + 24 )
        #define TWWB_RELATIVE           0
        #define TWWB_ABSOLUTE           1

#define ICAP_AUTOBINARY                         ( CAP_CUSTOMBASE + CAP_CFMSTART + 25 )
        #define TWAB_OFF                        0
        #define TWAB_DEFAULT            1
        #define TWAB_CHARACTER          2
        #define TWAB_TEXTURE            3

#define ICAP_IMAGESEPARATION            ( CAP_CUSTOMBASE + CAP_CFMSTART + 26 )


#define ICAP_HARDWARECOMPRESSION        ( CAP_CUSTOMBASE + CAP_CFMSTART + 27 )

#define ICAP_IMAGEEMPHASIS                      ( CAP_CUSTOMBASE + CAP_CFMSTART + 28 )
        #define TWIE_OFF                        0
        #define TWIE_LOW                        1
        #define TWIE_MEDIUM                     2
        #define TWIE_HIGH                       3
        #define TWIE_SMOOTHING          4


#define ICAP_OUTLINING                          ( CAP_CUSTOMBASE + CAP_CFMSTART + 29 )

#define ICAP_DYNTHRESHOLD                       ( CAP_CUSTOMBASE + CAP_CFMSTART + 30 )
        #define TWDT_OFF                        0
        #define TWDT_NORMAL                     1       // Normal Dynamic Threshold
        #define TWDT_ENHANCMENT         2       // Character enhancement
        #define TWDT_REMOVAL            3       // backgroung removal
        #define TWDT_SIMPLIFIED         4       // simplified dynamic threshold

// Variance rate is only active if Dynamic Threshold is TWDT_SIMPLIFIED
#define ICAP_VARIANCE                           ( CAP_CUSTOMBASE + CAP_CFMSTART + 31 )




#define CAP_ENDORSERAVAILABLE           ( CAP_CUSTOMBASE + CAP_CFMSTART + 32 )
#define CAP_ENDORSERENABLE                      ( CAP_CUSTOMBASE + CAP_CFMSTART + 33 )
#define CAP_ENDORSERCHARSET                     ( CAP_CUSTOMBASE + CAP_CFMSTART + 34 )
#define CAP_ENDORSERSTRINGLENGTH        ( CAP_CUSTOMBASE + CAP_CFMSTART + 35 )
#define CAP_ENDORSERSTRING                      ( CAP_CUSTOMBASE + CAP_CFMSTART + 36 )



#define ICAP_DYNTHRESHOLDCURVE          ( CAP_CUSTOMBASE + CAP_CFMSTART + 48 )
        #define TWTC_IMAGE_DARK                 0
        #define TWTC_IMAGE_LIGHT                1
        #define TWTC_OCR_VERYDARK               2
        #define TWTC_OCR_DARK                   3
        #define TWTC_OCR_DARKNORMAL             4
        #define TWTC_OCR_LIGHTNORMAL    5
        #define TWTC_OCR_LIGHT                  6
        #define TWTC_OCR_VERYLIGHT              7


#define ICAP_SMOOTHINGMODE                      ( CAP_CUSTOMBASE + CAP_CFMSTART + 49 )
        #define TWSM_OCR                                0
        #define TWSM_IMAGE                              1

#define ICAP_FILTERMODE                         ( CAP_CUSTOMBASE + CAP_CFMSTART + 50 )
        #define TWFM_ORDINARY                   0
        #define TWFM_BALLPOINTPEN               1

#define ICAP_GRADATION                          ( CAP_CUSTOMBASE + CAP_CFMSTART + 51 )
        #define TWGR_ORDINARY                   0
        #define TWGR_HIGHCONTRAST               1


#define ICAP_CFM_MIRROR                        ( CAP_CUSTOMBASE + CAP_CFMSTART + 52 )


#define ICAP_EASYSCANMODE                       ( CAP_CUSTOMBASE + CAP_CFMSTART + 53 )
        #define TWES_BINARYTHRESHOLD            1
        #define TWES_BINARYDIFFUSION            2
        #define TWES_BINARYHALFTONE                     3
        #define TWES_BINARYSEPARATION           4
        #define TWES_MONO4BIT                           5
        #define TWES_MONO8BIT                           6
        #define TWES_MONO10BIT                          7
        #define TWES_MONO12BIT                          8
        #define TWES_MONO14BIT                          9
        #define TWES_MONO16BIT                          10
        #define TWES_PALETTE8BIT                        11
        #define TWES_PALETTE8BITDIFFUSION       12
        #define TWES_COLOR8BIT                          13
        #define TWES_COLOR10BIT                         14
        #define TWES_COLOR12BIT                         15
        #define TWES_COLOR14BIT                         16
        #define TWES_COLOR16BIT                         17


#define ICAP_SOFTWAREINTERPOLATION      ( CAP_CUSTOMBASE + CAP_CFMSTART + 54 )

#define ICAP_IMAGESEPARATIONEX          ( CAP_CUSTOMBASE + CAP_CFMSTART + 55 )
        #define TWIS_AUTOFOTO           0
        #define TWIS_AUTONOFOTO         1

#define CAP_DUPLEXPAGE                          ( CAP_CUSTOMBASE + CAP_CFMSTART + 56 )
        #define TWDP_DUPLEXFRONT                0
        #define TWDP_DUPLEXBACK                 1
        #define TWDP_DUPLEXFRONTFIRST   2
        #define TWDP_DUPLEXBACKFIRST    3

#define ICAP_INVERTIMAGE                        ( CAP_CUSTOMBASE + CAP_CFMSTART + 57 )

#define ICAP_SPECKLEREMOVE                      ( CAP_CUSTOMBASE + CAP_CFMSTART + 58 )
        #define TWSR_REMOVE_OFF                 0
        #define TWSR_REMOVE_2X2                 1
        #define TWSR_REMOVE_3X3                 2
        #define TWSR_REMOVE_4X4                 4
        #define TWSR_REMOVE_5X5                 8
        #define TWSR_REMOVE_ALL                 ( TWSR_REMOVE_2X2 | TWSR_REMOVE_3X3 | TWSR_REMOVE_4X4 | TWSR_REMOVE_5X5 )


#define ICAP_USMFILTER                          ( CAP_CUSTOMBASE + CAP_CFMSTART + 59 )
        #define TWUF_USMSMOOTH                  0
        #define TWUF_USMMEDIUMSMOOTH    1
        #define TWUF_USMWEAKSMOOTH              2
        #define TWUF_USMOFF                             3
        #define TWUF_USMWEAK                    4
        #define TWUF_USMMEDIUM                  5
        #define TWUF_USMSTRONG                  6

#define ICAP_NOISEFILTERCFM                     ( CAP_CUSTOMBASE + CAP_CFMSTART + 60 )
        #define TWNF_NOISEOFF                   0
        #define TWNF_NOISE3X3                   1
        #define TWNF_NOISE5X5                   2

#define ICAP_DESCREENING                        ( CAP_CUSTOMBASE + CAP_CFMSTART + 61 )
        #define TWDS_MOOFF                              1
        #define TWDS_MOARTPRINT                 2
        #define TWDS_MOMAGAZINE                 3
        #define TWDS_MONEWSPAPER                4
        #define TWDS_MOCUSTOM                   5

#define ICAP_QUALITYFILTER                      ( CAP_CUSTOMBASE + CAP_CFMSTART + 62 )
        #define TWQF_QUNORMAL                   1
        #define TWQF_QUHIGHSPEED                2
        #define TWQF_QUGOOD                             3
        #define TWQF_QUVERYGOOD                 4

#define ICAP_BINARYFILTER                       ( CAP_CUSTOMBASE + CAP_CFMSTART + 63 )
        #define TWBF_BINARYFILTEROFF    0
        #define TWBF_BOLDLINING                 1
        #define TWBF_EDGEEXTRACTION             2

#define TWCC_FEEDCOVEROPEN                      ( TWCC_CUSTOMBASE + CAP_CFMSTART + 10 )
#define TWCC_FEEDLIFTUP                         ( TWCC_CUSTOMBASE + CAP_CFMSTART + 11 )
#define TWCC_FEEDJAM                            ( TWCC_CUSTOMBASE + CAP_CFMSTART + 12 )
#define TWCC_FEEDMISSFEED                       ( TWCC_CUSTOMBASE + CAP_CFMSTART + 13 )
#define TWCC_SERVICECALL                        ( TWCC_CUSTOMBASE + CAP_CFMSTART + 14 )
#endif
