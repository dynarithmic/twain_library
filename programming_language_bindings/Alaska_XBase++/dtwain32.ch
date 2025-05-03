//////////////////////////////////////////////////////////////////////
//    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
//    Copyright (c) 2002-2025 Dynarithmic Software.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
//    DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
//    OF THIRD PARTY RIGHTS.
//
#ifndef _DTWAIN32_CH
#define _DTWAIN32_CH

#define DTWAIN_TRUE           1
#define DTWAIN_FALSE          0

#define DTWAIN_FF_TIFF        0
#define DTWAIN_FF_PICT        1
#define DTWAIN_FF_BMP         2
#define DTWAIN_FF_XBM         3
#define DTWAIN_FF_JFIF        4
#define DTWAIN_FF_FPX         5
#define DTWAIN_FF_TIFFMULTI   6
#define DTWAIN_FF_PNG         7
#define DTWAIN_FF_SPIFF       8
#define DTWAIN_FF_EXIF        9
#define DTWAIN_FF_PDF         10
#define DTWAIN_FF_JP2         11
#define DTWAIN_FF_JPX         13
#define DTWAIN_FF_DEJAVU      14
#define DTWAIN_FF_PDFA        15
#define DTWAIN_FF_PDFA2       16
#define DTWAIN_FF_PDFRASTER   17

/* Compression Types for buffered and file transfers */
#define DTWAIN_CP_NONE        0
#define DTWAIN_CP_PACKBITS    1
#define DTWAIN_CP_GROUP31D    2
#define DTWAIN_CP_GROUP31DEOL 3
#define DTWAIN_CP_GROUP32D    4
#define DTWAIN_CP_GROUP4      5
#define DTWAIN_CP_JPEG        6
#define DTWAIN_CP_LZW         7
#define DTWAIN_CP_JBIG        8
#define DTWAIN_CP_PNG         9
#define DTWAIN_CP_RLE4        10
#define DTWAIN_CP_RLE8        11
#define DTWAIN_CP_BITFIELDS   12
#define DTWAIN_CP_ZIP         13
#define DTWAIN_CP_JPEG2000    14


/* Frame Sizes.  Same as TWAIN 1.8.  Use these values for
   setting the frame size, or to specify a PDF page size
   if acquiring to PDF files */
#define DTWAIN_FS_NONE        0
#define DTWAIN_FS_A4LETTER    1
#define DTWAIN_FS_B5LETTER    2
#define DTWAIN_FS_USLETTER    3
#define DTWAIN_FS_USLEGAL     4
#define DTWAIN_FS_A5          5
#define DTWAIN_FS_B4          6
#define DTWAIN_FS_B6          7
#define DTWAIN_FS_USLEDGER    9
#define DTWAIN_FS_USEXECUTIVE 10
#define DTWAIN_FS_A3          11
#define DTWAIN_FS_B3          12
#define DTWAIN_FS_A6          13
#define DTWAIN_FS_C4          14
#define DTWAIN_FS_C5          15
#define DTWAIN_FS_C6          16
#define DTWAIN_FS_4A0          17
#define DTWAIN_FS_2A0          18
#define DTWAIN_FS_A0           19
#define DTWAIN_FS_A1           20
#define DTWAIN_FS_A2           21
#define DTWAIN_FS_A4           DTWAIN_FS_A4LETTER
#define DTWAIN_FS_A7           22
#define DTWAIN_FS_A8           23
#define DTWAIN_FS_A9           24
#define DTWAIN_FS_A10          25
#define DTWAIN_FS_ISOB0        26
#define DTWAIN_FS_ISOB1        27
#define DTWAIN_FS_ISOB2        28
#define DTWAIN_FS_ISOB3        DTWAIN_FS_B3
#define DTWAIN_FS_ISOB4        DTWAIN_FS_B4
#define DTWAIN_FS_ISOB5        29
#define DTWAIN_FS_ISOB6        DTWAIN_FS_B6
#define DTWAIN_FS_ISOB7        30
#define DTWAIN_FS_ISOB8        31
#define DTWAIN_FS_ISOB9        32
#define DTWAIN_FS_ISOB10       33
#define DTWAIN_FS_JISB0        34
#define DTWAIN_FS_JISB1        35
#define DTWAIN_FS_JISB2        36
#define DTWAIN_FS_JISB3        37
#define DTWAIN_FS_JISB4        38
#define DTWAIN_FS_JISB5        DTWAIN_FS_B5LETTER
#define DTWAIN_FS_JISB6        39
#define DTWAIN_FS_JISB7        40
#define DTWAIN_FS_JISB8        41
#define DTWAIN_FS_JISB9        42
#define DTWAIN_FS_JISB10       43
#define DTWAIN_FS_C0           44
#define DTWAIN_FS_C1           45
#define DTWAIN_FS_C2           46
#define DTWAIN_FS_C3           47
#define DTWAIN_FS_C7           48
#define DTWAIN_FS_C8           49
#define DTWAIN_FS_C9           50
#define DTWAIN_FS_C10          51
#define DTWAIN_FS_USSTATEMENT  52
#define DTWAIN_FS_BUSINESSCARD 53


/* Parameter used when any support is desired */
#define DTWAIN_ANYSUPPORT   (-1)

#include "dtwain_filetypes.h"

/* Units of measure */
#define DTWAIN_INCHES      0
#define DTWAIN_CENTIMETERS 1
#define DTWAIN_PICAS       2
#define DTWAIN_POINTS      3
#define DTWAIN_TWIPS       4
#define DTWAIN_PIXELS      5
#define DTWAIN_MILLIMETERS 6

/* File Acquire flags */
#define DTWAIN_USENATIVE           1
#define DTWAIN_USEBUFFERED         2
#define DTWAIN_USECOMPRESSION      4
#define DTWAIN_USEMEMFILE          8
#define DTWAIN_USENAME            16
#define DTWAIN_USEPROMPT          32
#define DTWAIN_USELONGNAME        64
#define DTWAIN_USESOURCEMODE      128
#define DTWAIN_USELIST            256
#define DTWAIN_CREATE_DIRECTORY   512


/* DTWAIN_ARRAY types */
#define DTWAIN_ARRAYANY             1
#define DTWAIN_ARRAYLONG            2
#define DTWAIN_ARRAYFLOAT           3
#define DTWAIN_ARRAYHANDLE          4
#define DTWAIN_ARRAYSOURCE          5
#define DTWAIN_ARRAYSTRING          6
#define DTWAIN_ARRAYFRAME           7
#define DTWAIN_ARRAYBOOL            DTWAIN_ARRAYLONG
#define DTWAIN_ARRAYLONGSTRING      8
#define DTWAIN_ARRAYUNICODESTRING   9
#define DTWAIN_ARRAYLONG64          10
#define DTWAIN_ArrayTypePTR         DTWAIN_ARRAYHANDLE
#define DTWAIN_ARRAYOFHANDLEARRAYS  2000
/* Same string type as DTWAIN_ARRAYSTRING
   if compiling non-UNICODE (MBCS) applications */
#define DTWAIN_ARRAYANSISTRING      11

/* Same string type as DTWAIN_ARRAYSTRING
   if compiling UNICODE applications */
#define DTWAIN_ARRAYWIDESTRING      12
#define DTWAIN_ARRAYTWFIX32         200

#define DTWAIN_ArrayTypeINVALID     0

/* DTWAIN_ARRAY interpreted constants (used to distinguish integer types)
   All integer types are stored as LONG or LONG64.  The interpreter is used to
   let the app know what type of LONGs are stored (signed, unsigned, 16 bit signed
   16-bit unsigned, BOOLs
 */
#define DTWAIN_ARRAYINT16         100
#define DTWAIN_ARRAYUINT16        110
#define DTWAIN_ARRAYUINT32        120
#define DTWAIN_ARRAYINT32         130
#define DTWAIN_ARRAYINT64         140

/* DTWAIN_RANGE types */
#define DTWAIN_RANGELONG      DTWAIN_ARRAYLONG
#define DTWAIN_RANGEFLOAT     DTWAIN_ARRAYFLOAT

/* DTWAIN_RANGE constants */
#define DTWAIN_RANGEMIN     0
#define DTWAIN_RANGEMAX     1
#define DTWAIN_RANGESTEP    2
#define DTWAIN_RANGEDEFAULT   3
#define DTWAIN_RANGECURRENT   4

/* DTWAIN_FRAME constants */
#define DTWAIN_FRAMELEFT      0
#define DTWAIN_FRAMETOP       1
#define DTWAIN_FRAMERIGHT     2
#define DTWAIN_FRAMEBOTTOM    3

/* DTWAIN_FIX32 constants */
#define DTWAIN_FIX32WHOLE     0
#define DTWAIN_FIX32FRAC      1

/* DTWAIN Job Control constants */
#define DTWAIN_JC_NONE        0
#define DTWAIN_JC_JSIC        1
#define DTWAIN_JC_JSIS        2
#define DTWAIN_JC_JSXC        3
#define DTWAIN_JC_JSXS        4

/* Constant used for unknown capability type */
#define DTWAIN_CAPDATATYPE_UNKNOWN  (-10)

/* These job control constants are for devices that do
   not have TWAIN job control support */
#define DTWAIN_JCBP_JSIC        5
#define DTWAIN_JCBP_JSIS        6
#define DTWAIN_JCBP_JSXC        7
#define DTWAIN_JCBP_JSXS        8


/* DTWAIN Feeder control constants */
#define DTWAIN_FEEDPAGEON    1
#define DTWAIN_CLEARPAGEON   2
#define DTWAIN_REWINDPAGEON  4

/* Source settings */
#define DTWAIN_AppOwnsDib              1
#define DTWAIN_SourceOwnsDib           2

/* Container Types */
/* Start at 2^3=8 since the TWAIN constants start at 3 and are contiguous */
#define DTWAIN_CONTARRAY           8
#define DTWAIN_CONTENUMERATION     16
#define DTWAIN_CONTONEVALUE        32
#define DTWAIN_CONTRANGE           64
#define DTWAIN_CONTDEFAULT         0

/* Get capability types */
#define DTWAIN_CAPGET                1
#define DTWAIN_CAPGETCURRENT         2
#define DTWAIN_CAPGETDEFAULT         3

#define DTWAIN_CAPSET                6 /* Set one or more values                   */
#define DTWAIN_CAPRESET              7 /* Set current value to default value       */
#define DTWAIN_CAPRESETALL           8 /* Reset all capabilities */
#define DTWAIN_CAPSETCONSTRAINT      9 /* constrain values */

#define DTWAIN_CAPGETHELP            9
#define DTWAIN_CAPGETLABEL           10
#define DTWAIN_CAPGETLABELENUM       11

/* The following values are ORed with the DTWAIN_CAPSET value */
#define DTWAIN_CAPSETAVAILABLE       8  /* Sets available values  */
#define DTWAIN_CAPSETCURRENT         16 /* Sets current values    */

#define DTWAIN_AREASET               DTWAIN_CAPSET
#define DTWAIN_AREARESET             DTWAIN_CAPRESET
#define DTWAIN_AREACURRENT           DTWAIN_CAPGETCURRENT
#define DTWAIN_AREADEFAULT           DTWAIN_CAPGETDEFAULT

/* Twain version types */
#define DTWAIN_VER15                0
#define DTWAIN_VER16                1
#define DTWAIN_VER17                2
#define DTWAIN_VER18                3
#define DTWAIN_VER20                4
#define DTWAIN_VER21                5
#define DTWAIN_VER22                6

/* DTWAIN transfer */
#define DTWAIN_ACQUIREALL            (-1)
#define DTWAIN_MAXACQUIRE            (-1)

/* DTWAIN Duplex constants */
#define DTWAIN_DX_NONE         0
#define DTWAIN_DX_1PASSDUPLEX  1
#define DTWAIN_DX_2PASSDUPLEX  2

/* Twain Pixel Types */
#define DTWAIN_PT_BW      0
#define DTWAIN_PT_GRAY    1
#define DTWAIN_PT_RGB     2
#define DTWAIN_PT_PALETTE 3
#define DTWAIN_PT_CMY     4
#define DTWAIN_PT_CMYK    5
#define DTWAIN_PT_YUV     6
#define DTWAIN_PT_YUVK    7
#define DTWAIN_PT_CIEXYZ  8
#define DTWAIN_PT_LAB     9
#define DTWAIN_PT_SRGB    10
#define DTWAIN_PT_SCRGB   11
#define DTWAIN_PT_INFRARED 16
#define DTWAIN_PT_DEFAULT 1000

#define DTWAIN_CURRENT     (-2)
#define DTWAIN_DEFAULT     (-1)
#define DTWAIN_FLOATDEFAULT (-9999.0)

#define DTWAIN_DGNAME   0
#define DTWAIN_DATNAME  1
#define DTWAIN_MSGNAME  2

/* DTWAIN Callback constants */
#define DTWAIN_CallbackERROR      1
#define DTWAIN_CallbackMESSAGE    2

/* DTWAIN Special Failure codes */
#define DTWAIN_FAILURE1       (-1)
#define DTWAIN_FAILURE2       (-2)

/* Other miscellaneous constants */
#define DTWAIN_DELETEALL      (-1)

/* Page total page(s) transferred wParam's */
/* Sent when an acquisition has been successful */
#define  DTWAIN_TN_ACQUIREDONE                    1000

/* Sent when an acquisition has been failed */
#define  DTWAIN_TN_ACQUIREFAILED                  1001
/* Cancelled the acquisition */
#define  DTWAIN_TN_ACQUIRECANCELLED               1002
#define  DTWAIN_TN_ACQUIRECANCELED                1002
/* Started an acquisition */
#define  DTWAIN_TN_ACQUIRESTARTED                 1003

/* Sent when DTWAIN transfers individual pages for a multi-page scan */
/* Do we get the next page? */
#define  DTWAIN_TN_PAGECONTINUE                   1004
/* Page failed to be acquired */
#define  DTWAIN_TN_PAGEFAILED                     1005
/* Page cancelled */
#define  DTWAIN_TN_PAGECANCELLED                  1006
#define  DTWAIN_TN_PAGECANCELED                   1006

/* Sent when TWAIN is in the "Transfer ready" state (State 6) */
#define  DTWAIN_TN_TRANSFERREADY                  1009
/* Sent when TWAIN is in the "Transfer done" state (State 7) */
#define  DTWAIN_TN_TRANSFERDONE                   1010
#define  DTWAIN_TN_ACQUIREPAGEDONE                1010

/* Source closing/opening wParam's */
#define  DTWAIN_TN_UICLOSING                      3000
#define  DTWAIN_TN_UICLOSED                       3001
#define  DTWAIN_TN_UIOPENED                       3002
#define  DTWAIN_TN_UIOPENING                      3003
#define  DTWAIN_TN_UIOPENFAILURE                  3004

/* Twain page transferrance wParam's */
#define  DTWAIN_TN_CLIPTRANSFERDONE               1014

/* Invalid image file format specified or image could not be saved to file */
#define  DTWAIN_TN_INVALIDIMAGEFORMAT             1015

/* Sent when the UI is closed, and all processing of DIBs has been done */
#define  DTWAIN_TN_ACQUIRETERMINATED              1021

/* Sent when a strip has been successfully transferred from a buffered
   transfer */
#define DTWAIN_TN_TRANSFERSTRIPREADY              1022
#define DTWAIN_TN_TRANSFERSTRIPDONE               1023

/* Sent if a buffered strip transfer fails due to lack of memory */
#define DTWAIN_TN_TRANSFERSTRIPFAILED             1029

/* Sent if the image info is invalid */
#define DTWAIN_TN_IMAGEINFOERROR                  1024

/* Sent if program decides to cancel the initial
   transfer */
#define DTWAIN_TN_TRANSFERCANCELLED          1030
#define DTWAIN_TN_TRANSFERCANCELED           1030

/* Sent if user cancels the saving of a file using the file prompt */
#define DTWAIN_TN_FILESAVECANCELLED         1031
#define DTWAIN_TN_FILESAVECANCELED         1031
#define DTWAIN_TN_FILESAVEOK                1032
#define DTWAIN_TN_FILESAVEERROR             1033
#define DTWAIN_TN_FILEPAGESAVEOK            1034
#define DTWAIN_TN_FILEPAGESAVEERROR         1035
#define DTWAIN_TN_PROCESSEDDIB              1036


/* Sent if document feeder has paper loaded */
#define DTWAIN_TN_FEEDERLOADED              1037

/* Sent whenever a DTWAIN error is generated
 * This is only available for DTWAIN_SetCallback
 * functions */
#define DTWAIN_TN_GENERALERROR             1038

/* Sent when in manual duplex mode */
/* Side 1 completed successfully */
#define DTWAIN_TN_MANDUPFLIPPAGES          1040

/* Side 1 completed, maybe successful depending on return value */
#define DTWAIN_TN_MANDUPSIDE1DONE          1041

/* Side 2 completed, maybe successful depending on return value */
#define DTWAIN_TN_MANDUPSIDE2DONE          1042

/* Both sides done, but page count mismatch occurred */
#define DTWAIN_TN_MANDUPPAGECOUNTERROR     1043

/* Both sides done, everything is successful */
#define DTWAIN_TN_MANDUPACQUIREDONE        1044

/* Side 1 started */
#define DTWAIN_TN_MANDUPSIDE1START         1045

/* Side 2 started */
#define DTWAIN_TN_MANDUPSIDE2START         1046

/* Error when merging the acquired pages */
#define DTWAIN_TN_MANDUPMERGEERROR         1047

/* Error when attempting to allocate memory for duplexed page */
#define DTWAIN_TN_MANDUPMEMORYERROR        1048

/* Error when attempting to read image data for duplexed page */
#define DTWAIN_TN_MANDUPFILEERROR          1049

/* Error when attempting to save duplex file */
#define DTWAIN_TN_MANDUPFILESAVEERROR      1050

/* End of Job control message */
#define DTWAIN_TN_ENDOFJOBDETECTED         1051
#define DTWAIN_TN_EOJDETECTED              1051

/* End of job when message when transfer is initially done */
#define DTWAIN_TN_EOJDETECTED_XFERDONE    1052

/* Query if page should be saved or discarded */
#define DTWAIN_TN_QUERYPAGEDISCARD          1053

/* Send if page was discarded from save */
#define DTWAIN_TN_PAGEDISCARDED             1054

/* Sent to application to acknowledge that
   the page will no longer go through further processing when acquired
   from device */
#define DTWAIN_TN_PROCESSDIBACCEPTED   1055
#define DTWAIN_TN_PROCESSDIBFINALACCEPTED   1056

/* Miscellaneous file transfer notifications */
#define DTWAIN_TN_CLOSEDIBFAILED       1057
#define DTWAIN_TN_INVALID_TWAINDSM2_BITMAP 1058

/* Device event for TWAIN 1.8 Sources */
#define  DTWAIN_TN_DEVICEEVENT                    1100

/* Sent if TWAIN driver sends cancel code during acquisition */
#define DTWAIN_TN_TWAINPAGECANCELLED       1105
#define DTWAIN_TN_TWAINPAGECANCELED        1105

/* Sent if TWAIN driver sends failure code during acquisition */
#define DTWAIN_TN_TWAINPAGEFAILED          1106

/* Sent if application changes DIB */
#define DTWAIN_TN_APPUPDATEDDIB            1107

/* Sent when saving a page using the
   DTWAIN_FILESAVE_UICLOSE, DTWAIN_FILESAVE_SOURCECLOSE option is used */
#define DTWAIN_TN_FILEPAGESAVING           1110

/* Sent when job is being saved to file */
#define DTWAIN_TN_EOJBEGINFILESAVE         1112

/* Sent after job saved to file (sent regardless if there is an error) */
#define DTWAIN_TN_EOJENDFILESAVE           1113

/* Sent when copping DIB fails */
#define DTWAIN_TN_CROPFAILED               1120

/* Sent on naive or buffered transfer done
 * and DIB has been fully processed by DTWAIN */
#define DTWAIN_TN_PROCESSEDDIBFINAL        1121

/* Determine if blank page has been detected */
#define DTWAIN_TN_BLANKPAGEDETECTED1       1130
#define DTWAIN_TN_BLANKPAGEDETECTED2       1131
#define DTWAIN_TN_BLANKPAGEDETECTED3       1132
#define DTWAIN_TN_BLANKPAGEDISCARDED1      1133
#define DTWAIN_TN_BLANKPAGEDISCARDED2      1134

/* Messages sent when text acquired from OCR */
#define DTWAIN_TN_OCRTEXTRETRIEVED         1140
#define DTWAIN_TN_QUERYOCRTEXT             1141
#define DTWAIN_TN_PDFOCRREADY              1142
#define DTWAIN_TN_PDFOCRDONE               1143
#define DTWAIN_TN_PDFOCRERROR              1144

/* Message sent when DTWAIN_SetCallback() is invoked on a non-NULL callback function */
#define DTWAIN_TN_SETCALLBACKINIT          1150
#define DTWAIN_TN_SETCALLBACK64INIT        1151

/* Sent when a file name is generated when saving images to files */
#define DTWAIN_TN_FILENAMECHANGING         1160
#define DTWAIN_TN_FILENAMECHANGED          1161

/* Sent when an audio file has been transferred */
#define DTWAIN_TN_PROCESSEDAUDIOFINAL       1180
#define DTWAIN_TN_PROCESSAUDIOFINALACCEPTED 1181
#define DTWAIN_TN_PROCESSEDAUDIOFILE        1182

/* Sent when a TWAIN triplet is being processed */
#define DTWAIN_TN_TWAINTRIPLETBEGIN         1183
#define DTWAIN_TN_TWAINTRIPLETEND           1184

/* Sent if document feeder has no paper loaded */
#define DTWAIN_TN_FEEDERNOTLOADED           1200

/* PDF OCR clean text flags */
#define DTWAIN_PDFOCR_CLEANTEXT1            1
#define DTWAIN_PDFOCR_CLEANTEXT2            2

/* DTWAIN Acquire Modes */
#define  DTWAIN_MODAL                        0
#define  DTWAIN_MODELESS                     1

/* DTWAIN Source UI Mode */
#define DTWAIN_UIModeCLOSE                    0
#define DTWAIN_UIModeOPEN                     1

#define DTWAIN_REOPEN_SOURCE                  2

/* DTWAIN Rounding for range values */
#define DTWAIN_ROUNDNEAREST   0
#define DTWAIN_ROUNDUP        1
#define DTWAIN_ROUNDDOWN      2
#define DTWAIN_FLOATDELTA    (+1.0e-8)

/* Rotations (same as TWAIN definitions) */
#define DTWAIN_OR_ROT0           0
#define DTWAIN_OR_ROT90          1
#define DTWAIN_OR_ROT180         2
#define DTWAIN_OR_ROT270         3
#define DTWAIN_OR_PORTRAIT       DTWAIN_OR_ROT0
#define DTWAIN_OR_LANDSCAPE      DTWAIN_OR_ROT270
#define DTWAIN_OR_ANYROTATION    (-1)

/* Cap operation support constants (same as TWAIN 2.x ) */
#define DTWAIN_CO_GET           0x0001
#define DTWAIN_CO_SET           0x0002
#define DTWAIN_CO_GETDEFAULT    0x0004
#define DTWAIN_CO_GETCURRENT    0x0008
#define DTWAIN_CO_RESET         0x0010
#define DTWAIN_CO_SETCONSTRAINT 0x0020
#define DTWAIN_CO_CONSTRAINABLE    0x0040
#define DTWAIN_CO_GETHELP          0x0100
#define DTWAIN_CO_GETLABEL         0x0200
#define DTWAIN_CO_GETLABELENUM     0x0400


/* Country information */
#define DTWAIN_CNTYAFGHANISTAN             1001
#define DTWAIN_CNTYALGERIA                  213
#define DTWAIN_CNTYAMERICANSAMOA            684
#define DTWAIN_CNTYANDORRA                   33
#define DTWAIN_CNTYANGOLA                  1002
#define DTWAIN_CNTYANGUILLA                8090
#define DTWAIN_CNTYANTIGUA                 8091
#define DTWAIN_CNTYARGENTINA                 54
#define DTWAIN_CNTYARUBA                    297
#define DTWAIN_CNTYASCENSIONI               247
#define DTWAIN_CNTYAUSTRALIA                 61
#define DTWAIN_CNTYAUSTRIA                   43
#define DTWAIN_CNTYBAHAMAS                 8092
#define DTWAIN_CNTYBAHRAIN                  973
#define DTWAIN_CNTYBANGLADESH               880
#define DTWAIN_CNTYBARBADOS                8093
#define DTWAIN_CNTYBELGIUM                   32
#define DTWAIN_CNTYBELIZE                   501
#define DTWAIN_CNTYBENIN                    229
#define DTWAIN_CNTYBERMUDA                 8094
#define DTWAIN_CNTYBHUTAN                  1003
#define DTWAIN_CNTYBOLIVIA                  591
#define DTWAIN_CNTYBOTSWANA                 267
#define DTWAIN_CNTYBRITAIN                    6
#define DTWAIN_CNTYBRITVIRGINIS            8095
#define DTWAIN_CNTYBRAZIL                    55
#define DTWAIN_CNTYBRUNEI                   673
#define DTWAIN_CNTYBULGARIA                 359
#define DTWAIN_CNTYBURKINAFASO             1004
#define DTWAIN_CNTYBURMA                   1005
#define DTWAIN_CNTYBURUNDI                 1006
#define DTWAIN_CNTYCAMAROON                 237
#define DTWAIN_CNTYCANADA                     2
#define DTWAIN_CNTYCAPEVERDEIS              238
#define DTWAIN_CNTYCAYMANIS                8096
#define DTWAIN_CNTYCENTRALAFREP            1007
#define DTWAIN_CNTYCHAD                    1008
#define DTWAIN_CNTYCHILE                     56
#define DTWAIN_CNTYCHINA                     86
#define DTWAIN_CNTYCHRISTMASIS             1009
#define DTWAIN_CNTYCOCOSIS                 1009
#define DTWAIN_CNTYCOLOMBIA                  57
#define DTWAIN_CNTYCOMOROS                 1010
#define DTWAIN_CNTYCONGO                   1011
#define DTWAIN_CNTYCOOKIS                  1012
#define DTWAIN_CNTYCOSTARICA               506
#define DTWAIN_CNTYCUBA                     5
#define DTWAIN_CNTYCYPRUS                   357
#define DTWAIN_CNTYCZECHOSLOVAKIA            42
#define DTWAIN_CNTYDENMARK                   45
#define DTWAIN_CNTYDJIBOUTI                1013
#define DTWAIN_CNTYDOMINICA                8097
#define DTWAIN_CNTYDOMINCANREP             8098
#define DTWAIN_CNTYEASTERIS                1014
#define DTWAIN_CNTYECUADOR                  593
#define DTWAIN_CNTYEGYPT                     20
#define DTWAIN_CNTYELSALVADOR               503
#define DTWAIN_CNTYEQGUINEA                1015
#define DTWAIN_CNTYETHIOPIA                 251
#define DTWAIN_CNTYFALKLANDIS              1016
#define DTWAIN_CNTYFAEROEIS                 298
#define DTWAIN_CNTYFIJIISLANDS              679
#define DTWAIN_CNTYFINLAND                  358
#define DTWAIN_CNTYFRANCE                    33
#define DTWAIN_CNTYFRANTILLES               596
#define DTWAIN_CNTYFRGUIANA                 594
#define DTWAIN_CNTYFRPOLYNEISA              689
#define DTWAIN_CNTYFUTANAIS                1043
#define DTWAIN_CNTYGABON                    241
#define DTWAIN_CNTYGAMBIA                   220
#define DTWAIN_CNTYGERMANY                   49
#define DTWAIN_CNTYGHANA                    233
#define DTWAIN_CNTYGIBRALTER                350
#define DTWAIN_CNTYGREECE                    30
#define DTWAIN_CNTYGREENLAND                299
#define DTWAIN_CNTYGRENADA                 8099
#define DTWAIN_CNTYGRENEDINES              8015
#define DTWAIN_CNTYGUADELOUPE               590
#define DTWAIN_CNTYGUAM                     671
#define DTWAIN_CNTYGUANTANAMOBAY           5399
#define DTWAIN_CNTYGUATEMALA                502
#define DTWAIN_CNTYGUINEA                   224
#define DTWAIN_CNTYGUINEABISSAU            1017
#define DTWAIN_CNTYGUYANA                   592
#define DTWAIN_CNTYHAITI                    509
#define DTWAIN_CNTYHONDURAS                 504
#define DTWAIN_CNTYHONGKONG                852
#define DTWAIN_CNTYHUNGARY                   36
#define DTWAIN_CNTYICELAND                  354
#define DTWAIN_CNTYINDIA                     91
#define DTWAIN_CNTYINDONESIA                 62
#define DTWAIN_CNTYIRAN                      98
#define DTWAIN_CNTYIRAQ                     964
#define DTWAIN_CNTYIRELAND                  353
#define DTWAIN_CNTYISRAEL                   972
#define DTWAIN_CNTYITALY                     39
#define DTWAIN_CNTYIVORYCOAST              225
#define DTWAIN_CNTYJAMAICA                 8010
#define DTWAIN_CNTYJAPAN                     81
#define DTWAIN_CNTYJORDAN                   962
#define DTWAIN_CNTYKENYA                    254
#define DTWAIN_CNTYKIRIBATI                1018
#define DTWAIN_CNTYKOREA                     82
#define DTWAIN_CNTYKUWAIT                   965
#define DTWAIN_CNTYLAOS                    1019
#define DTWAIN_CNTYLEBANON                 1020
#define DTWAIN_CNTYLIBERIA                  231
#define DTWAIN_CNTYLIBYA                    218
#define DTWAIN_CNTYLIECHTENSTEIN             41
#define DTWAIN_CNTYLUXENBOURG               352
#define DTWAIN_CNTYMACAO                    853
#define DTWAIN_CNTYMADAGASCAR              1021
#define DTWAIN_CNTYMALAWI                   265
#define DTWAIN_CNTYMALAYSIA                  60
#define DTWAIN_CNTYMALDIVES                 960
#define DTWAIN_CNTYMALI                    1022
#define DTWAIN_CNTYMALTA                    356
#define DTWAIN_CNTYMARSHALLIS               692
#define DTWAIN_CNTYMAURITANIA              1023
#define DTWAIN_CNTYMAURITIUS                230
#define DTWAIN_CNTYMEXICO                     3
#define DTWAIN_CNTYMICRONESIA               691
#define DTWAIN_CNTYMIQUELON                 508
#define DTWAIN_CNTYMONACO                    33
#define DTWAIN_CNTYMONGOLIA                1024
#define DTWAIN_CNTYMONTSERRAT              8011
#define DTWAIN_CNTYMOROCCO                  212
#define DTWAIN_CNTYMOZAMBIQUE              1025
#define DTWAIN_CNTYNAMIBIA                  264
#define DTWAIN_CNTYNAURU                   1026
#define DTWAIN_CNTYNEPAL                    977
#define DTWAIN_CNTYNETHERLANDS               31
#define DTWAIN_CNTYNETHANTILLES             599
#define DTWAIN_CNTYNEVIS                   8012
#define DTWAIN_CNTYNEWCALEDONIA             687
#define DTWAIN_CNTYNEWZEALAND                64
#define DTWAIN_CNTYNICARAGUA                505
#define DTWAIN_CNTYNIGER                    227
#define DTWAIN_CNTYNIGERIA                  234
#define DTWAIN_CNTYNIUE                    1027
#define DTWAIN_CNTYNORFOLKI                1028
#define DTWAIN_CNTYNORWAY                    47
#define DTWAIN_CNTYOMAN                     968
#define DTWAIN_CNTYPAKISTAN                  92
#define DTWAIN_CNTYPALAU                   1029
#define DTWAIN_CNTYPANAMA                   507
#define DTWAIN_CNTYPARAGUAY                 595
#define DTWAIN_CNTYPERU                      51
#define DTWAIN_CNTYPHILLIPPINES              63
#define DTWAIN_CNTYPITCAIRNIS              1030
#define DTWAIN_CNTYPNEWGUINEA               675
#define DTWAIN_CNTYPOLAND                    48
#define DTWAIN_CNTYPORTUGAL                 351
#define DTWAIN_CNTYQATAR                    974
#define DTWAIN_CNTYREUNIONI                1031
#define DTWAIN_CNTYROMANIA                   40
#define DTWAIN_CNTYRWANDA                   250
#define DTWAIN_CNTYSAIPAN                   670
#define DTWAIN_CNTYSANMARINO                 39
#define DTWAIN_CNTYSAOTOME                 1033
#define DTWAIN_CNTYSAUDIARABIA              966
#define DTWAIN_CNTYSENEGAL                  221
#define DTWAIN_CNTYSEYCHELLESIS            1034
#define DTWAIN_CNTYSIERRALEONE             1035
#define DTWAIN_CNTYSINGAPORE                 65
#define DTWAIN_CNTYSOLOMONIS               1036
#define DTWAIN_CNTYSOMALI                  1037
#define DTWAIN_CNTYSOUTHAFRICA               27
#define DTWAIN_CNTYSPAIN                     34
#define DTWAIN_CNTYSRILANKA                  94
#define DTWAIN_CNTYSTHELENA                1032
#define DTWAIN_CNTYSTKITTS                 8013
#define DTWAIN_CNTYSTLUCIA                 8014
#define DTWAIN_CNTYSTPIERRE                 508
#define DTWAIN_CNTYSTVINCENT               8015
#define DTWAIN_CNTYSUDAN                   1038
#define DTWAIN_CNTYSURINAME                 597
#define DTWAIN_CNTYSWAZILAND                268
#define DTWAIN_CNTYSWEDEN                    46
#define DTWAIN_CNTYSWITZERLAND               41
#define DTWAIN_CNTYSYRIA                   1039
#define DTWAIN_CNTYTAIWAN                   886
#define DTWAIN_CNTYTANZANIA                 255
#define DTWAIN_CNTYTHAILAND                  66
#define DTWAIN_CNTYTOBAGO                  8016
#define DTWAIN_CNTYTOGO                     228
#define DTWAIN_CNTYTONGAIS                  676
#define DTWAIN_CNTYTRINIDAD                8016
#define DTWAIN_CNTYTUNISIA                  216
#define DTWAIN_CNTYTURKEY                    90
#define DTWAIN_CNTYTURKSCAICOS             8017
#define DTWAIN_CNTYTUVALU                  1040
#define DTWAIN_CNTYUGANDA                   256
#define DTWAIN_CNTYUSSR                       7
#define DTWAIN_CNTYUAEMIRATES               971
#define DTWAIN_CNTYUNITEDKINGDOM             44
#define DTWAIN_CNTYUSA                        1
#define DTWAIN_CNTYURUGUAY                  598
#define DTWAIN_CNTYVANUATU                 1041
#define DTWAIN_CNTYVATICANCITY               39
#define DTWAIN_CNTYVENEZUELA                 58
#define DTWAIN_CNTYWAKE                    1042
#define DTWAIN_CNTYWALLISIS                1043
#define DTWAIN_CNTYWESTERNSAHARA           1044
#define DTWAIN_CNTYWESTERNSAMOA            1045
#define DTWAIN_CNTYYEMEN                   1046
#define DTWAIN_CNTYYUGOSLAVIA                38
#define DTWAIN_CNTYZAIRE                    243
#define DTWAIN_CNTYZAMBIA                   260
#define DTWAIN_CNTYZIMBABWE                 263


/* Language information */
#define DTWAIN_LANGDANISH                     0
#define DTWAIN_LANGDUTCH                      1
#define DTWAIN_LANGINTERNATIONALENGLISH       2
#define DTWAIN_LANGFRENCHCANADIAN             3
#define DTWAIN_LANGFINNISH                    4
#define DTWAIN_LANGFRENCH                     5
#define DTWAIN_LANGGERMAN                     6
#define DTWAIN_LANGICELANDIC                  7
#define DTWAIN_LANGITALIAN                    8
#define DTWAIN_LANGNORWEGIAN                  9
#define DTWAIN_LANGPORTUGUESE                10
#define DTWAIN_LANGSPANISH                   11
#define DTWAIN_LANGSWEDISH                   12
#define DTWAIN_LANGUSAENGLISH                13

/* DTWAIN API and Twain status flags */
#define DTWAIN_APIHANDLEOK                    1
#define DTWAIN_TWAINSESSIONOK                 2

/* Error codes (returned by DTWAIN_GetLastError() */
#define DTWAIN_NO_ERROR               (0)
#define DTWAIN_ERR_FIRST              (-1000)
#define DTWAIN_ERR_BAD_HANDLE         (-1001)
#define DTWAIN_ERR_BAD_SOURCE         (-1002)
#define DTWAIN_ERR_BAD_ARRAY          (-1003)
#define DTWAIN_ERR_WRONG_ARRAY_TYPE   (-1004)
#define DTWAIN_ERR_INDEX_BOUNDS       (-1005)
#define DTWAIN_ERR_OUT_OF_MEMORY      (-1006)
#define DTWAIN_ERR_NULL_WINDOW        (-1007)
#define DTWAIN_ERR_BAD_PIXTYPE        (-1008)
#define DTWAIN_ERR_BAD_CONTAINER      (-1009)
#define DTWAIN_ERR_NO_SESSION         (-1010)
#define DTWAIN_ERR_BAD_ACQUIRE_NUM    (-1011)
#define DTWAIN_ERR_BAD_CAP            (-1012)
#define DTWAIN_ERR_CAP_NO_SUPPORT     (-1013)
#define DTWAIN_ERR_TWAIN              (-1014)
#define DTWAIN_ERR_HOOK_FAILED        (-1015)
#define DTWAIN_ERR_BAD_FILENAME       (-1016)
#define DTWAIN_ERR_EMPTY_ARRAY        (-1017)
#define DTWAIN_ERR_FILE_FORMAT        (-1018)
#define DTWAIN_ERR_BAD_DIB_PAGE       (-1019)
#define DTWAIN_ERR_SOURCE_ACQUIRING   (-1020)
#define DTWAIN_ERR_INVALID_PARAM      (-1021)
#define DTWAIN_ERR_INVALID_RANGE      (-1022)
#define DTWAIN_ERR_UI_ERROR           (-1023)
#define DTWAIN_ERR_BAD_UNIT           (-1024)
#define DTWAIN_ERR_LANGDLL_NOT_FOUND  (-1025)
#define DTWAIN_ERR_SOURCE_NOT_OPEN    (-1026)
#define DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED (-1027)
#define DTWAIN_ERR_UIONLY_NOT_SUPPORTED (-1028)
#define DTWAIN_ERR_UI_ALREADY_OPENED  (-1029)
#define DTWAIN_ERR_CAPSET_NOSUPPORT   (-1030)
#define DTWAIN_ERR_NO_FILE_XFER       (-1031)
#define DTWAIN_ERR_INVALID_BITDEPTH   (-1032)
#define DTWAIN_ERR_NO_CAPS_DEFINED    (-1033)
#define DTWAIN_ERR_TILES_NOT_SUPPORTED (-1034)
#define DTWAIN_ERR_INVALID_DTWAIN_FRAME (-1035)
#define DTWAIN_ERR_LIMITED_VERSION  (-1036)
#define DTWAIN_ERR_NO_FEEDER        (-1037)
#define DTWAIN_ERR_NO_FEEDER_QUERY  (-1038)
#define DTWAIN_ERR_EXCEPTION_ERROR  (-1039)
#define DTWAIN_ERR_INVALID_STATE    (-1040)
#define DTWAIN_ERR_UNSUPPORTED_EXTINFO (-1041)
#define DTWAIN_ERR_DLLRESOURCE_NOTFOUND (-1042)
#define DTWAIN_ERR_NOT_INITIALIZED (-1043)
#define DTWAIN_ERR_NO_SOURCES (-1044)
#define DTWAIN_ERR_TWAIN_NOT_INSTALLED (-1045)
#define DTWAIN_ERR_WRONG_THREAD  (-1046)
#define DTWAIN_ERR_BAD_CAPTYPE   (-1047)
#define DTWAIN_ERR_UNKNOWN_CAPDATATYPE   (-1048)
#define DTWAIN_ERR_DEMO_NOFILETYPE (-1049)
#define DTWAIN_ERR_SOURCESELECTION_CANCELED (-1050)
#define DTWAIN_ERR_RESOURCES_NOT_FOUND (-1051)
#define DTWAIN_ERR_STRINGTYPE_MISMATCH (-1052)
#define DTWAIN_ERR_ARRAYTYPE_MISMATCH (-1053)
#define DTWAIN_ERR_SOURCENAME_NOTINSTALLED (-1054)
#define DTWAIN_ERR_NO_MEMFILE_XFER       (-1055)
#define DTWAIN_ERR_AREA_ARRAY_TOO_SMALL  (-1056)
#define DTWAIN_ERR_LOG_CREATE_ERROR  (-1057)
#define DTWAIN_ERR_FILESYSTEM_NOT_SUPPORTED (-1058)
#define DTWAIN_ERR_TILEMODE_NOTSET   (-1059)
#define DTWAIN_ERR_INI32_NOT_FOUND   (-1060)
#define DTWAIN_ERR_INI64_NOT_FOUND  (-1061)
#define DTWAIN_ERR_CRC_CHECK   (-1062)
#define DTWAIN_ERR_RESOURCES_BAD_VERSION  (-1063)
#define DTWAIN_ERR_WIN32_ERROR  (-1064)
#define DTWAIN_ERR_STRINGID_NOTFOUND  (-1065)
#define DTWAIN_ERR_RESOURCES_DUPLICATEID_FOUND (-1066)
#define DTWAIN_ERR_UNAVAILABLE_EXTINFO (-1067)
#define DTWAIN_ERR_TWAINDSM2_BADBITMAP (-1068)
#define DTWAIN_ERR_ACQUISITION_CANCELED (-1069)
#define DTWAIN_ERR_IMAGE_RESAMPLED (-1070)
#define DTWAIN_ERR_UNKNOWN_TWAIN_RC (-1071)
#define DTWAIN_ERR_UNKNOWN_TWAIN_CC (-1072)
#define DTWAIN_ERR_RESOURCES_DATA_EXCEPTION (-1073)
#define DTWAIN_ERR_AUDIO_TRANSFER_NOTSUPPORTED (-1074)
#define DTWAIN_ERR_FEEDER_COMPLIANCY (-1075)
#define DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY1 (-1076)
#define DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY2 (-1077)
#define DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY1 (-1078)
#define DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY2 (-1079)
#define DTWAIN_ERR_ICAPBITDEPTH_COMPLIANCY1 (-1080)
#define DTWAIN_ERR_XFERMECH_COMPLIANCY      (-1081)
#define DTWAIN_ERR_STANDARDCAPS_COMPLIANCY  (-1082)
#define DTWAIN_ERR_EXTIMAGEINFO_DATATYPE_MISMATCH (-1083)
#define DTWAIN_ERR_EXTIMAGEINFO_RETRIEVAL (-1084)
#define DTWAIN_ERR_RANGE_OUTOFBOUNDS      (-1085)
#define DTWAIN_ERR_RANGE_STEPISZERO       (-1086)
#define DTWAIN_ERR_BLANKNAMEDETECTED   (-1087)

#define TWAIN_ERR_LOW_MEMORY        (-1100)
#define TWAIN_ERR_FALSE_ALARM       (-1101)
#define TWAIN_ERR_BUMMER            (-1102)
#define TWAIN_ERR_NODATASOURCE      (-1103)
#define TWAIN_ERR_MAXCONNECTIONS    (-1104)
#define TWAIN_ERR_OPERATIONERROR    (-1105)
#define TWAIN_ERR_BADCAPABILITY     (-1106)
#define TWAIN_ERR_BADVALUE          (-1107)
#define TWAIN_ERR_BADPROTOCOL       (-1108)
#define TWAIN_ERR_SEQUENCEERROR     (-1109)
#define TWAIN_ERR_BADDESTINATION    (-1110)
#define TWAIN_ERR_CAPNOTSUPPORTED   (-1111)
#define TWAIN_ERR_CAPBADOPERATION   (-1112)
#define TWAIN_ERR_CAPSEQUENCEERROR  (-1113)
#define TWAIN_ERR_FILEPROTECTEDERROR  (-1114)
#define TWAIN_ERR_FILEEXISTERROR      (-1115)
#define TWAIN_ERR_FILENOTFOUND        (-1116)
#define TWAIN_ERR_DIRNOTEMPTY         (-1117)
#define TWAIN_ERR_FEEDERJAMMED        (-1118)
#define TWAIN_ERR_FEEDERMULTPAGES     (-1119)
#define TWAIN_ERR_FEEDERWRITEERROR    (-1120)
#define TWAIN_ERR_DEVICEOFFLINE       (-1121)
#define TWAIN_ERR_NULL_CONTAINER      (-1122)
#define TWAIN_ERR_INTERLOCK           (-1123)
#define TWAIN_ERR_DAMAGEDCORNER       (-1124)
#define TWAIN_ERR_FOCUSERROR          (-1125)
#define TWAIN_ERR_DOCTOOLIGHT         (-1126)
#define TWAIN_ERR_DOCTOODARK          (-1127)
#define TWAIN_ERR_NOMEDIA             (-1128)

/* File errors generated when calling DTWAIN_AcquireFile or DTWAIN_AcquireFileEx */
#define DTWAIN_ERR_FILEXFERSTART    (-2000)
#define DTWAIN_ERR_MEM              (-2001)
#define DTWAIN_ERR_FILEOPEN         (-2002)
#define DTWAIN_ERR_FILEREAD         (-2003)
#define DTWAIN_ERR_FILEWRITE        (-2004)
#define DTWAIN_ERR_BADPARAM         (-2005)
#define DTWAIN_ERR_INVALIDBMP       (-2006)
#define DTWAIN_ERR_BMPRLE           (-2007)
#define DTWAIN_ERR_RESERVED1        (-2008)
#define DTWAIN_ERR_INVALIDJPG       (-2009)
#define DTWAIN_ERR_DC               (-2010)
#define DTWAIN_ERR_DIB              (-2011)
#define DTWAIN_ERR_RESERVED2        (-2012)
#define DTWAIN_ERR_NORESOURCE       (-2013)
#define DTWAIN_ERR_CALLBACKCANCEL   (-2014)
#define DTWAIN_ERR_INVALIDPNG       (-2015)
#define DTWAIN_ERR_PNGCREATE        (-2016)
#define DTWAIN_ERR_INTERNAL         (-2017)
#define DTWAIN_ERR_FONT             (-2018)
#define DTWAIN_ERR_INTTIFF          (-2019)
#define DTWAIN_ERR_INVALIDTIFF      (-2020)
#define DTWAIN_ERR_NOTIFFLZW        (-2021)
#define DTWAIN_ERR_INVALIDPCX       (-2022)
#define DTWAIN_ERR_CREATEBMP        (-2023)
#define DTWAIN_ERR_NOLINES          (-2024)
#define DTWAIN_ERR_GETDIB           (-2025)
#define DTWAIN_ERR_NODEVOP          (-2026)
#define DTWAIN_ERR_INVALIDWMF       (-2027)
#define DTWAIN_ERR_DEPTHMISMATCH    (-2028)
#define DTWAIN_ERR_BITBLT           (-2029)
#define DTWAIN_ERR_BUFTOOSMALL      (-2030)
#define DTWAIN_ERR_TOOMANYCOLORS    (-2031)
#define DTWAIN_ERR_INVALIDTGA       (-2032)
#define DTWAIN_ERR_NOTGATHUMBNAIL   (-2033)
#define DTWAIN_ERR_RESERVED3        (-2034)
#define DTWAIN_ERR_CREATEDIB        (-2035)
#define DTWAIN_ERR_NOLZW            (-2036)
#define DTWAIN_ERR_SELECTOBJ        (-2037)
#define DTWAIN_ERR_BADMANAGER       (-2038)
#define DTWAIN_ERR_OBSOLETE         (-2039)
#define DTWAIN_ERR_CREATEDIBSECTION        (-2040)
#define DTWAIN_ERR_SETWINMETAFILEBITS      (-2041)
#define DTWAIN_ERR_GETWINMETAFILEBITS      (-2042)
#define DTWAIN_ERR_PAXPWD                  (-2043)
#define DTWAIN_ERR_INVALIDPAX              (-2044)
#define DTWAIN_ERR_NOSUPPORT               (-2045)
#define DTWAIN_ERR_INVALIDPSD              (-2046)
#define DTWAIN_ERR_PSDNOTSUPPORTED         (-2047)
#define DTWAIN_ERR_DECRYPT                 (-2048)
#define DTWAIN_ERR_ENCRYPT                 (-2049)
#define DTWAIN_ERR_COMPRESSION             (-2050)
#define DTWAIN_ERR_DECOMPRESSION           (-2051)
#define DTWAIN_ERR_INVALIDTLA              (-2052)
#define DTWAIN_ERR_INVALIDWBMP             (-2053)
#define DTWAIN_ERR_NOTIFFTAG               (-2054)
#define DTWAIN_ERR_NOLOCALSTORAGE          (-2055)
#define DTWAIN_ERR_INVALIDEXIF             (-2056)
#define DTWAIN_ERR_NOEXIFSTRING            (-2057)
#define DTWAIN_ERR_TIFFDLL32NOTFOUND       (-2058)
#define DTWAIN_ERR_TIFFDLL16NOTFOUND       (-2059)
#define DTWAIN_ERR_PNGDLL16NOTFOUND        (-2060)
#define DTWAIN_ERR_JPEGDLL16NOTFOUND       (-2061)
#define DTWAIN_ERR_BADBITSPERPIXEL         (-2062)
#define DTWAIN_ERR_TIFFDLL32INVALIDVER     (-2063)
#define DTWAIN_ERR_PDFDLL32NOTFOUND        (-2064)
#define DTWAIN_ERR_PDFDLL32INVALIDVER      (-2065)
#define DTWAIN_ERR_JPEGDLL32NOTFOUND       (-2066)
#define DTWAIN_ERR_JPEGDLL32INVALIDVER     (-2067)
#define DTWAIN_ERR_PNGDLL32NOTFOUND        (-2068)
#define DTWAIN_ERR_PNGDLL32INVALIDVER      (-2069)
#define DTWAIN_ERR_J2KDLL32NOTFOUND        (-2070)
#define DTWAIN_ERR_J2KDLL32INVALIDVER      (-2071)
#define DTWAIN_ERR_MANDUPLEX_UNAVAILABLE   (-2072)
#define DTWAIN_ERR_TIMEOUT                 (-2073)
#define DTWAIN_ERR_INVALIDICONFORMAT       (-2074)
#define DTWAIN_ERR_TWAIN32DSMNOTFOUND      (-2075)
#define DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND (-2076)
#define DTWAIN_ERR_INVALID_DIRECTORY (-2077)
#define DTWAIN_ERR_CREATE_DIRECTORY (-2078)
#define DTWAIN_ERR_OCRLIBRARY_NOTFOUND (-2079)

/* TwainSave errors */
#define DTWAIN_TWAINSAVE_OK                (0)
#define DTWAIN_ERR_TS_FIRST                (-2080)
#define DTWAIN_ERR_TS_NOFILENAME           (-2081)
#define DTWAIN_ERR_TS_NOTWAINSYS           (-2082)
#define DTWAIN_ERR_TS_DEVICEFAILURE        (-2083)
#define DTWAIN_ERR_TS_FILESAVEERROR        (-2084)
#define DTWAIN_ERR_TS_COMMANDILLEGAL       (-2085)
#define DTWAIN_ERR_TS_CANCELLED            (-2086)
#define DTWAIN_ERR_TS_ACQUISITIONERROR     (-2087)
#define DTWAIN_ERR_TS_INVALIDCOLORSPACE    (-2088)
#define DTWAIN_ERR_TS_PDFNOTSUPPORTED      (-2089)
#define DTWAIN_ERR_TS_NOTAVAILABLE         (-2090)

/* OCR errors */
#define DTWAIN_ERR_OCR_FIRST               (-2100)
#define DTWAIN_ERR_OCR_INVALIDPAGENUM      (-2101)
#define DTWAIN_ERR_OCR_INVALIDENGINE       (-2102)
#define DTWAIN_ERR_OCR_NOTACTIVE           (-2103)
#define DTWAIN_ERR_OCR_INVALIDFILETYPE     (-2104)
#define DTWAIN_ERR_OCR_INVALIDPIXELTYPE    (-2105)
#define DTWAIN_ERR_OCR_INVALIDBITDEPTH     (-2106)
#define DTWAIN_ERR_OCR_RECOGNITIONERROR    (-2107)
#define DTWAIN_ERR_OCR_LAST                (-2108)
#define DTWAIN_ERR_SOURCE_COULD_NOT_OPEN   (-2500)
#define DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE  (-2501)
#define DTWAIN_ERR_IMAGEINFO_INVALID       (-2502)
#define DTWAIN_ERR_WRITEDATA_TOFILE        (-2503)

#define DTWAIN_ERR_LAST                    DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE
#define DTWAIN_ERR_USER_START              (-80000)

/* Device event constants (same as TWAIN 1.8 value plus 1)*/
#define DTWAIN_DE_CHKAUTOCAPTURE    1
#define DTWAIN_DE_CHKBATTERY        2
#define DTWAIN_DE_CHKDEVICEONLINE   4
#define DTWAIN_DE_CHKFLASH          8
#define DTWAIN_DE_CHKPOWERSUPPLY    16
#define DTWAIN_DE_CHKRESOLUTION     32
#define DTWAIN_DE_DEVICEADDED       64
#define DTWAIN_DE_DEVICEOFFLINE     128
#define DTWAIN_DE_DEVICEREADY       256
#define DTWAIN_DE_DEVICEREMOVED     512
#define DTWAIN_DE_IMAGECAPTURED     1024
#define DTWAIN_DE_IMAGEDELETED      2048
#define DTWAIN_DE_PAPERDOUBLEFEED   4096
#define DTWAIN_DE_PAPERJAM          8192
#define DTWAIN_DE_LAMPFAILURE       16384
#define DTWAIN_DE_POWERSAVE         32768
#define DTWAIN_DE_POWERSAVENOTIFY   65536
#define DTWAIN_DE_CUSTOMEVENTS       0x8000

/* DTWAIN Constants used for getting Device Event Info*/
#define DTWAIN_GETDE_EVENT           0
#define DTWAIN_GETDE_DEVNAME         1
#define DTWAIN_GETDE_BATTERYMINUTES  2
#define DTWAIN_GETDE_BATTERYPCT      3
#define DTWAIN_GETDE_XRESOLUTION     4
#define DTWAIN_GETDE_YRESOLUTION     5
#define DTWAIN_GETDE_FLASHUSED       6
#define DTWAIN_GETDE_AUTOCAPTURE     7
#define DTWAIN_GETDE_TIMEBEFORECAPTURE 8
#define DTWAIN_GETDE_TIMEBETWEENCAPTURES  9
#define DTWAIN_GETDE_POWERSUPPLY      10

/* DTWAIN Imprinter/Endorser Constants  (TWAIN 1.8 values)*/
#define DTWAIN_IMPRINTERTOPBEFORE     1   /*(0)*/
#define DTWAIN_IMPRINTERTOPAFTER      2   /*(1)*/
#define DTWAIN_IMPRINTERBOTTOMBEFORE  4   /*(2)*/
#define DTWAIN_IMPRINTERBOTTOMAFTER   8   /*(3)*/
#define DTWAIN_ENDORSERTOPBEFORE      16  /*(4)*/
#define DTWAIN_ENDORSERTOPAFTER       32  /*(5)*/
#define DTWAIN_ENDORSERBOTTOMBEFORE   64  /*(6)*/
#define DTWAIN_ENDORSERBOTTOMAFTER    128 /*(7)*/

#define DTWAIN_TWPR_IMPRINTERTOPBEFORE     0
#define DTWAIN_TWPR_IMPRINTERTOPAFTER      1
#define DTWAIN_TWPR_IMPRINTERBOTTOMBEFORE  2
#define DTWAIN_TWPR_IMPRINTERBOTTOMAFTER   3
#define DTWAIN_TWPR_ENDORSERTOPBEFORE      4
#define DTWAIN_TWPR_ENDORSERTOPAFTER       5
#define DTWAIN_TWPR_ENDORSERBOTTOMBEFORE   6
#define DTWAIN_TWPR_ENDORSERBOTTOMAFTER    7

/* DTWAIN Printermode constants (same as TWAIN 1.8) */
#define DTWAIN_PM_SINGLESTRING     0
#define DTWAIN_PM_MULTISTRING      1
#define DTWAIN_PM_COMPOUNDSTRING   2

/* TWAIN Data Types */
#define DTWAIN_TWTY_INT8        0x0000
#define DTWAIN_TWTY_INT16       0x0001
#define DTWAIN_TWTY_INT32       0x0002
#define DTWAIN_TWTY_UINT8       0x0003
#define DTWAIN_TWTY_UINT16      0x0004
#define DTWAIN_TWTY_UINT32      0x0005
#define DTWAIN_TWTY_BOOL        0x0006
#define DTWAIN_TWTY_FIX32       0x0007
#define DTWAIN_TWTY_FRAME       0x0008
#define DTWAIN_TWTY_STR32       0x0009
#define DTWAIN_TWTY_STR64       0x000A
#define DTWAIN_TWTY_STR128      0x000B
#define DTWAIN_TWTY_STR255      0x000C
#define DTWAIN_TWTY_STR1024     0x000D
#define DTWAIN_TWTY_UNI512      0x000E

/* Extended image attributes */
#define DTWAIN_EI_BARCODEX               0x1200
#define DTWAIN_EI_BARCODEY               0x1201
#define DTWAIN_EI_BARCODETEXT            0x1202
#define DTWAIN_EI_BARCODETYPE            0x1203
#define DTWAIN_EI_DESHADETOP             0x1204
#define DTWAIN_EI_DESHADELEFT            0x1205
#define DTWAIN_EI_DESHADEHEIGHT          0x1206
#define DTWAIN_EI_DESHADEWIDTH           0x1207
#define DTWAIN_EI_DESHADESIZE            0x1208
#define DTWAIN_EI_SPECKLESREMOVED        0x1209
#define DTWAIN_EI_HORZLINEXCOORD         0x120A
#define DTWAIN_EI_HORZLINEYCOORD         0x120B
#define DTWAIN_EI_HORZLINELENGTH         0x120C
#define DTWAIN_EI_HORZLINETHICKNESS      0x120D
#define DTWAIN_EI_VERTLINEXCOORD         0x120E
#define DTWAIN_EI_VERTLINEYCOORD         0x120F
#define DTWAIN_EI_VERTLINELENGTH         0x1210
#define DTWAIN_EI_VERTLINETHICKNESS      0x1211
#define DTWAIN_EI_PATCHCODE              0x1212
#define DTWAIN_EI_ENDORSEDTEXT           0x1213
#define DTWAIN_EI_FORMCONFIDENCE         0x1214
#define DTWAIN_EI_FORMTEMPLATEMATCH      0x1215
#define DTWAIN_EI_FORMTEMPLATEPAGEMATCH  0x1216
#define DTWAIN_EI_FORMHORZDOCOFFSET      0x1217
#define DTWAIN_EI_FORMVERTDOCOFFSET      0x1218
#define DTWAIN_EI_BARCODECOUNT           0x1219
#define DTWAIN_EI_BARCODECONFIDENCE      0x121A
#define DTWAIN_EI_BARCODEROTATION        0x121B
#define DTWAIN_EI_BARCODETEXTLENGTH      0x121C
#define DTWAIN_EI_DESHADECOUNT           0x121D
#define DTWAIN_EI_DESHADEBLACKCOUNTOLD   0x121E
#define DTWAIN_EI_DESHADEBLACKCOUNTNEW   0x121F
#define DTWAIN_EI_DESHADEBLACKRLMIN      0x1220
#define DTWAIN_EI_DESHADEBLACKRLMAX      0x1221
#define DTWAIN_EI_DESHADEWHITECOUNTOLD   0x1222
#define DTWAIN_EI_DESHADEWHITECOUNTNEW   0x1223
#define DTWAIN_EI_DESHADEWHITERLMIN      0x1224
#define DTWAIN_EI_DESHADEWHITERLAVE      0x1225
#define DTWAIN_EI_DESHADEWHITERLMAX      0x1226
#define DTWAIN_EI_BLACKSPECKLESREMOVED   0x1227
#define DTWAIN_EI_WHITESPECKLESREMOVED   0x1228
#define DTWAIN_EI_HORZLINECOUNT          0x1229
#define DTWAIN_EI_VERTLINECOUNT          0x122A
#define DTWAIN_EI_DESKEWSTATUS           0x122B
#define DTWAIN_EI_SKEWORIGINALANGLE      0x122C
#define DTWAIN_EI_SKEWFINALANGLE         0x122D
#define DTWAIN_EI_SKEWCONFIDENCE         0x122E
#define DTWAIN_EI_SKEWWINDOWX1           0x122F
#define DTWAIN_EI_SKEWWINDOWY1           0x1230
#define DTWAIN_EI_SKEWWINDOWX2           0x1231
#define DTWAIN_EI_SKEWWINDOWY2           0x1232
#define DTWAIN_EI_SKEWWINDOWX3           0x1233
#define DTWAIN_EI_SKEWWINDOWY3           0x1234
#define DTWAIN_EI_SKEWWINDOWX4           0x1235
#define DTWAIN_EI_SKEWWINDOWY4           0x1236
#define DTWAIN_EI_BOOKNAME               0x1238
#define DTWAIN_EI_CHAPTERNUMBER          0x1239
#define DTWAIN_EI_DOCUMENTNUMBER         0x123A
#define DTWAIN_EI_PAGENUMBER             0x123B
#define DTWAIN_EI_CAMERA                 0x123C
#define DTWAIN_EI_FRAMENUMBER            0x123D
#define DTWAIN_EI_FRAME                  0x123E
#define DTWAIN_EI_PIXELFLAVOR            0x123F
#define DTWAIN_EI_ICCPROFILE             0x1240
#define DTWAIN_EI_LASTSEGMENT            0x1241
#define DTWAIN_EI_SEGMENTNUMBER          0x1242
#define DTWAIN_EI_MAGDATA                0x1243
#define DTWAIN_EI_MAGTYPE                0x1244
#define DTWAIN_EI_PAGESIDE               0x1245
#define DTWAIN_EI_FILESYSTEMSOURCE       0x1246
#define DTWAIN_EI_IMAGEMERGED            0x1247
#define DTWAIN_EI_MAGDATALENGTH          0x1248
#define DTWAIN_EI_PAPERCOUNT             0x1249
#define DTWAIN_EI_PRINTERTEXT            0x124A
#define DTWAIN_EI_TWAINDIRECTMETADATA    0x124B
#define DTWAIN_EI_IAFIELDA_VALUE         0x124C
#define DTWAIN_EI_IAFIELDB_VALUE         0x124D
#define DTWAIN_EI_IAFIELDC_VALUE         0x124E
#define DTWAIN_EI_IAFIELDD_VALUE         0x124F
#define DTWAIN_EI_IAFIELDE_VALUE         0x1250
#define DTWAIN_EI_IALEVEL                0x1251
#define DTWAIN_EI_PRINTER                0x1252
#define DTWAIN_EI_BARCODETEXT2           0x1253

/* TWAIN Data Source Error logging functions */
#define DTWAIN_LOG_DECODE_SOURCE      0x00000001
#define DTWAIN_LOG_DECODE_DEST        0x00000002
#define DTWAIN_LOG_DECODE_TWMEMREF    0x00000004
#define DTWAIN_LOG_DECODE_TWEVENT     0x00000008

/* DTWAIN Call stack logging */
#define DTWAIN_LOG_CALLSTACK          0x00000010

/* DTWAIN LOG DTWAIN_IsTwainMsg
 * Note that enabling this will produce very large log files.
 */
#define DTWAIN_LOG_ISTWAINMSG         0x00000020

/* Display message if DTWAIN function called on bad DLL
If this flag is not set, calls to an uninitialized
DTWAIN DLL are not displayed */
#define DTWAIN_LOG_INITFAILURE        0x00000040

/* All other lower-level TWAIN activity*/
#define DTWAIN_LOG_LOWLEVELTWAIN      0x00000080

/* Decode bitmap info returned by TWAIN device when acquiring images*/
#define DTWAIN_LOG_DECODE_BITMAP      0x00000100

/* Log DTWAIN_TN_ notifications  */
#define DTWAIN_LOG_NOTIFICATIONS      0x00000200

/* All other DTWAIN information such as CAP listings, etc.*/
#define DTWAIN_LOG_MISCELLANEOUS      0x00000400

/* Any DTWAIN errors (not TWAIN related) */
#define DTWAIN_LOG_DTWAINERRORS       0x00000800

#define DTWAIN_LOG_ALL (DTWAIN_LOG_DECODE_SOURCE | ;
                        DTWAIN_LOG_DECODE_DEST | ;
                        DTWAIN_LOG_DECODE_TWEVENT | ;
                        DTWAIN_LOG_DECODE_TWMEMREF | ;
                        DTWAIN_LOG_CALLSTACK | ;
                        DTWAIN_LOG_ISTWAINMSG | ;
                        DTWAIN_LOG_INITFAILURE | ;
                        DTWAIN_LOG_LOWLEVELTWAIN | ;
                        DTWAIN_LOG_NOTIFICATIONS | ;
                        DTWAIN_LOG_MISCELLANEOUS | ;
                        DTWAIN_LOG_DTWAINERRORS | ;
                        DTWAIN_LOG_DECODE_BITMAP)

/* ------------------------- */

/* DTWAIN Log to a file */
#define DTWAIN_LOG_USEFILE         0x00010000

/* log exception errors to message boxes */
#define DTWAIN_LOG_SHOWEXCEPTIONS  0x00020000

/* Display standard message box if DTWAIN error */
#define DTWAIN_LOG_ERRORMSGBOX     0x00040000

/* Log errors to a buffer of all DTWAIN errors */
#define DTWAIN_LOG_USEBUFFER       0x00080000

/* Append to log file */
#define DTWAIN_LOG_FILEAPPEND      0x00100000

/* If this flag is ON, a callback function is invoked */
#define DTWAIN_LOG_USECALLBACK     0x00200000

/* If cr/lf is added to end of log message when writing to
   debug monitor, this flag is ON */
#define DTWAIN_LOG_USECRLF         0x00400000

/* Log to the console */
#define DTWAIN_LOG_CONSOLE         0x00800000

/* Log to debug monitor */
#define DTWAIN_LOG_DEBUGMONITOR    0x01000000

/* DTWAIN Log to window (not yet implemented) */
#define DTWAIN_LOG_USEWINDOW       0x02000000

/* log everything, including displaying exceptions */
#define DTWAIN_LOG_ALL_NOCALLBACK   (DTWAIN_LOG_ALL &~ (DTWAIN_LOG_USECALLBACK))

/* log everything using new log file */
#define DTWAIN_LOG_ALL_FILEAPPEND    (DTWAIN_LOG_FILEAPPEND | DTWAIN_LOG_ALL)

/* turn off the DTWAIN_IsTwainMsg logging */
#define DTWAIN_LOG_NOISTWAINMSG(x) { if ((x) | DTWAIN_LOG_ISTWAINMSG) (x) &= ~DTWAIN_LOG_ISTWAINMSG; }
#define DTWAIN_LOG_NOLOWLEVELTWAIN(x) { (x) &= ~(DTWAIN_LOG_LOWLEVELTWAIN); }

/* CAP_CUSTOMDSDATA constants */
#define DTWAINGCD_RETURNHANDLE      1
#define DTWAINGCD_COPYDATA          2

/* DTWAIN Search constants */
#define DTWAIN_BYPOSITION           0
#define DTWAIN_BYID                 1

#define DTWAINSCD_USEHANDLE         1
#define DTWAINSCD_USEDATA           2

/* DTWAIN Page Failure Action constants */
#define DTWAIN_PAGEFAIL_RETRY       1
#define DTWAIN_PAGEFAIL_TERMINATE   2
#define DTWAIN_MAXRETRY_ATTEMPTS    3 /* Can be set by DTWAIN_SetMaxRetryAttempts() */
#define DTWAIN_RETRY_FOREVER        (-1)

/* PDF page settings */
#define DTWAIN_PDF_NOSCALING         128  /* Places image as-is on PDF page */
#define DTWAIN_PDF_FITPAGE           256  /* Fits the image into the page size */
#define DTWAIN_PDF_VARIABLEPAGESIZE  512  /* PDF page is determined by acquired page. */
#define DTWAIN_PDF_CUSTOMSIZE        1024  /* Page size is custom */
#define DTWAIN_PDF_USECOMPRESSION    2048  /* Use zlib compression */
#define DTWAIN_PDF_CUSTOMSCALE       4096  /* Custom scaling value */
#define DTWAIN_PDF_PIXELSPERMETERSIZE 8192  /* Determine PDF page size given image bit depth,
                                              image dimensions, and pixels per meter setting */

/* PDF encryption settings */
#define DTWAIN_PDF_ALLOWPRINTING     2052 /* (2048 + 4) */
#define DTWAIN_PDF_ALLOWMOD          8
#define DTWAIN_PDF_ALLOWCOPY         16
#define DTWAIN_PDF_ALLOWMODANNOTATIONS 32
#define DTWAIN_PDF_ALLOWFILLIN       256
#define DTWAIN_PDF_ALLOWEXTRACTION   512
#define DTWAIN_PDF_ALLOWASSEMBLY     1024
#define DTWAIN_PDF_ALLOWDEGRADEDPRINTING 4

/* PDF Orientation */
#define DTWAIN_PDF_PORTRAIT         0
#define DTWAIN_PDF_LANDSCAPE        1


/* Postscript options */
#define DTWAIN_PS_REGULAR           0
#define DTWAIN_PS_ENCAPSULATED      1

/* Blank page detection options */
#define DTWAIN_BP_AUTODISCARD_NONE         0
#define DTWAIN_BP_AUTODISCARD_IMMEDIATE    1
#define DTWAIN_BP_AUTODISCARD_AFTERPROCESS 2
#define DTWAIN_BP_DETECTORIGINAL        1
#define DTWAIN_BP_DETECTADJUSTED        2
#define DTWAIN_BP_DETECTALL         (DTWAIN_BP_DETECTORIGINAL | DTWAIN_BP_DETECTADJUSTED)
#define DTWAIN_BP_AUTODISCARD_ANY          0xFFFF

/* Lightpath */
#define DTWAIN_LP_REFLECTIVE        0
#define DTWAIN_LP_TRANSMISSIVE      1

/* Light Source */
#define DTWAIN_LS_RED               0
#define DTWAIN_LS_GREEN             1
#define DTWAIN_LS_BLUE              2
#define DTWAIN_LS_NONE              3
#define DTWAIN_LS_WHITE             4
#define DTWAIN_LS_UV                5
#define DTWAIN_LS_IR                6

/* DTWAIN Twain dialog options */
#define DTWAIN_DLG_SORTNAMES            1
#define DTWAIN_DLG_CENTER               2
#define DTWAIN_DLG_CENTER_SCREEN        4
#define DTWAIN_DLG_USETEMPLATE          8
#define DTWAIN_DLG_CLEAR_PARAMS         16
#define DTWAIN_DLG_HORIZONTALSCROLL     32
#define DTWAIN_DLG_USEINCLUDENAMES      64
#define DTWAIN_DLG_USEEXCLUDENAMES      128
#define DTWAIN_DLG_USENAMEMAPPING       256
#define DTWAIN_DLG_USEDEFAULTTITLE      512
#define DTWAIN_DLG_TOPMOSTWINDOW        1024
#define DTWAIN_DLG_OPENONSELECT         2048
#define DTWAIN_DLG_OPENONSELECTOVERRIDE 4096
#define DTWAIN_DLG_OPENONSELECTON       (DTWAIN_DLG_OPENONSELECT | DTWAIN_DLG_OPENONSELECTOVERRIDE)
#define DTWAIN_DLG_OPENONSELECTOFF      (DTWAIN_DLG_OPENONSELECTOVERRIDE)

/* DTWAIN Language resource constants */
#define DTWAIN_RES_ENGLISH              0
#define DTWAIN_RES_FRENCH               1
#define DTWAIN_RES_SPANISH              2
#define DTWAIN_RES_DUTCH                3
#define DTWAIN_RES_GERMAN               4
#define DTWAIN_RES_ITALIAN              5

/* DTWAIN Alarm constants */
#define DTWAIN_AL_ALARM             0
#define DTWAIN_AL_FEEDERERROR       1
#define DTWAIN_AL_FEEDERWARNING     2
#define DTWAIN_AL_BARCODE           3
#define DTWAIN_AL_DOUBLEFEED        4
#define DTWAIN_AL_JAM               5
#define DTWAIN_AL_PATCHCODE         6
#define DTWAIN_AL_POWER             7
#define DTWAIN_AL_SKEW              8

/* DTWAIN File System constants */
#define DTWAIN_FT_CAMERA         0
#define DTWAIN_FT_CAMERATOP      1
#define DTWAIN_FT_CAMERABOTTOM   2
#define DTWAIN_FT_CAMERAPREVIEW  3
#define DTWAIN_FT_DOMAIN         4
#define DTWAIN_FT_HOST           5
#define DTWAIN_FT_DIRECTORY      6
#define DTWAIN_FT_IMAGE          7
#define DTWAIN_FT_UNKNOWN        8

/* DTWAIN Noise Filter constants */
#define DTWAIN_NF_NONE           0
#define DTWAIN_NF_AUTO           1
#define DTWAIN_NF_LONEPIXEL      2
#define DTWAIN_NF_MAJORITYRULE   3

/* DTWAIN Clear Buffers */
#define DTWAIN_CB_AUTO          0
#define DTWAIN_CB_CLEAR         1
#define DTWAIN_CB_NOCLEAR       2

/* DTWAIN Feeder Alignment */
#define DTWAIN_FA_NONE          0
#define DTWAIN_FA_LEFT          1
#define DTWAIN_FA_CENTER        2
#define DTWAIN_FA_RIGHT         3

/* DTWAIN Pixel Flavor */
#define DTWAIN_PF_CHOCOLATE     0
#define DTWAIN_PF_VANILLA       1

/* DTWAIN Feeder Order */
#define DTWAIN_FO_FIRSTPAGEFIRST 0
#define DTWAIN_FO_LASTPAGEFIRST  1

/* DTWAIN File increment flags */
#define DTWAIN_INCREMENT_STATIC  0
#define DTWAIN_INCREMENT_DYNAMIC 1
#define DTWAIN_INCREMENT_DEFAULT -1

/* DTWAIN Manual Duplex mode constants */
#define DTWAIN_MANDUP_FACEUPTOPPAGE      0
#define DTWAIN_MANDUP_FACEUPBOTTOMPAGE   1
#define DTWAIN_MANDUP_FACEDOWNTOPPAGE    2
#define DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE 3

#define DTWAIN_FILESAVE_DEFAULT          0
#define DTWAIN_FILESAVE_UICLOSE          1
#define DTWAIN_FILESAVE_SOURCECLOSE      2
#define DTWAIN_FILESAVE_ENDACQUIRE       3
#define DTWAIN_FILESAVE_MANUALSAVE       4
#define DTWAIN_FILESAVE_SAVEINCOMPLETE   128

#define DTWAIN_MANDUP_SCANOK                    1 /* must be 1 for
                                                     proper processing */
#define DTWAIN_MANDUP_SIDE1RESCAN               2
#define DTWAIN_MANDUP_SIDE2RESCAN               3
#define DTWAIN_MANDUP_RESCANALL                 4
#define DTWAIN_MANDUP_PAGEMISSING               5

#define DTWAIN_DEMODLL_VERSION                  0x00000001
#define DTWAIN_UNLICENSED_VERSION               0x00000002
#define DTWAIN_COMPANY_VERSION                  0x00000004
#define DTWAIN_GENERAL_VERSION                  0x00000008
#define DTWAIN_DEVELOP_VERSION                  0x00000010
#define DTWAIN_JAVA_VERSION                     0x00000020
#define DTWAIN_TOOLKIT_VERSION                  0x00000040
#define DTWAIN_LIMITEDDLL_VERSION               0x00000080
#define DTWAIN_STATICLIB_VERSION                0x00000100
#define DTWAIN_STATICLIB_STDCALL_VERSION        0x00000200
#define DTWAIN_PDF_VERSION                      0x00010000
#define DTWAIN_TWAINSAVE_VERSION                0x00020000
#define DTWAIN_OCR_VERSION                      0x00040000
#define DTWAIN_BARCODE_VERSION                  0x00080000
#define DTWAIN_ACTIVEX_VERSION                  0x00100000
#define DTWAIN_32BIT_VERSION                    0x00200000
#define DTWAIN_64BIT_VERSION                    0x00400000
#define DTWAIN_UNICODE_VERSION                  0x00800000
#define DTWAIN_OPENSOURCE_VERSION               0x01000000


/* OCR defines */
#define DTWAINOCR_RETURNHANDLE                  1
#define DTWAINOCR_COPYDATA                      2

#define DTWAIN_OCRINFO_CHAR                     0
#define DTWAIN_OCRINFO_CHARXPOS                 1
#define DTWAIN_OCRINFO_CHARYPOS                 2
#define DTWAIN_OCRINFO_CHARXWIDTH               3
#define DTWAIN_OCRINFO_CHARYWIDTH               4
#define DTWAIN_OCRINFO_CHARCONFIDENCE           5
#define DTWAIN_OCRINFO_PAGENUM                  6
#define DTWAIN_OCRINFO_OCRENGINE                7
#define DTWAIN_OCRINFO_TEXTLENGTH               8

#define DTWAIN_PDFPAGETYPE_COLOR                0
#define DTWAIN_PDFPAGETYPE_BW                   1

/* DSM types */
#define DTWAIN_TWAINDSM_LEGACY                  1
#define DTWAIN_TWAINDSM_VERSION2                2
#define DTWAIN_TWAINDSM_LATESTVERSION           4

/* Windows TWAIN DSM search logic constants */
#define DTWAIN_TWAINDSMSEARCH_NOTFOUND         (-1)
#define DTWAIN_TWAINDSMSEARCH_WSO               0
#define DTWAIN_TWAINDSMSEARCH_WOS               1
#define DTWAIN_TWAINDSMSEARCH_SWO               2
#define DTWAIN_TWAINDSMSEARCH_SOW               3
#define DTWAIN_TWAINDSMSEARCH_OWS               4
#define DTWAIN_TWAINDSMSEARCH_OSW               5
#define DTWAIN_TWAINDSMSEARCH_W                 6
#define DTWAIN_TWAINDSMSEARCH_S                 7
#define DTWAIN_TWAINDSMSEARCH_O                 8
#define DTWAIN_TWAINDSMSEARCH_WS                9
#define DTWAIN_TWAINDSMSEARCH_WO                10
#define DTWAIN_TWAINDSMSEARCH_SW                11
#define DTWAIN_TWAINDSMSEARCH_SO                12
#define DTWAIN_TWAINDSMSEARCH_OW                13
#define DTWAIN_TWAINDSMSEARCH_OS                14
#define DTWAIN_TWAINDSMSEARCH_C                 15

#define DTWAIN_PDFPOLARITY_POSITIVE             1
#define DTWAIN_PDFPOLARITY_NEGATIVE             2

/* CAP_PRINTERFONTSTYLE */
#define DTWAIN_TWPF_NORMAL              0
#define DTWAIN_TWPF_BOLD                1
#define DTWAIN_TWPF_ITALIC              2
#define DTWAIN_TWPF_LARGESIZE           3
#define DTWAIN_TWPF_SMALLSIZE           4

/* CAP_PRINTERINDEXTRIGGER Added 2.3 */
#define DTWAIN_TWCT_PAGE                0
#define DTWAIN_TWCT_PATCH1              1
#define DTWAIN_TWCT_PATCH2              2
#define DTWAIN_TWCT_PATCH3              3
#define DTWAIN_TWCT_PATCH4              4
#define DTWAIN_TWCT_PATCH5              5
#define DTWAIN_TWCT_PATCH6              6

/* CAP_DOUBLEFEED... */
#define DTWAIN_TWDF_ULTRASONIC          0
#define DTWAIN_TWDF_BYLENGTH            1
#define DTWAIN_TWDF_INFRARED            2

/* DTWAIN Twain name lookup constants */
#define DTWAIN_CONSTANT_TWPT     0
#define DTWAIN_CONSTANT_TWUN     1
#define DTWAIN_CONSTANT_TWCY     2
#define DTWAIN_CONSTANT_TWAL     3
#define DTWAIN_CONSTANT_TWAS     4
#define DTWAIN_CONSTANT_TWBCOR   5
#define DTWAIN_CONSTANT_TWBD     6
#define DTWAIN_CONSTANT_TWBO     7
#define DTWAIN_CONSTANT_TWBP     8
#define DTWAIN_CONSTANT_TWBR     9
#define DTWAIN_CONSTANT_TWBT     10
#define DTWAIN_CONSTANT_TWCP     11
#define DTWAIN_CONSTANT_TWCS     12
#define DTWAIN_CONSTANT_TWDE     13
#define DTWAIN_CONSTANT_TWDR     14
#define DTWAIN_CONSTANT_TWDSK    15
#define DTWAIN_CONSTANT_TWDX     16
#define DTWAIN_CONSTANT_TWFA     17
#define DTWAIN_CONSTANT_TWFE     18
#define DTWAIN_CONSTANT_TWFF     19
#define DTWAIN_CONSTANT_TWFL     20
#define DTWAIN_CONSTANT_TWFO     21
#define DTWAIN_CONSTANT_TWFP     22
#define DTWAIN_CONSTANT_TWFR     23
#define DTWAIN_CONSTANT_TWFT     24
#define DTWAIN_CONSTANT_TWFY     22
#define DTWAIN_CONSTANT_TWIA     23
#define DTWAIN_CONSTANT_TWIC     27
#define DTWAIN_CONSTANT_TWIF     28
#define DTWAIN_CONSTANT_TWIM     29
#define DTWAIN_CONSTANT_TWJC     30
#define DTWAIN_CONSTANT_TWJQ     31
#define DTWAIN_CONSTANT_TWLP     32
#define DTWAIN_CONSTANT_TWLS     33
#define DTWAIN_CONSTANT_TWMD     34
#define DTWAIN_CONSTANT_TWNF     35
#define DTWAIN_CONSTANT_TWOR     36
#define DTWAIN_CONSTANT_TWOV     37
#define DTWAIN_CONSTANT_TWPA     38
#define DTWAIN_CONSTANT_TWPC     39
#define DTWAIN_CONSTANT_TWPCH    40
#define DTWAIN_CONSTANT_TWPF     41
#define DTWAIN_CONSTANT_TWPM     42
#define DTWAIN_CONSTANT_TWPR     43
#define DTWAIN_CONSTANT_TWPF2    44
#define DTWAIN_CONSTANT_TWCT     45
#define DTWAIN_CONSTANT_TWPS     46
#define DTWAIN_CONSTANT_TWSS     47
#define DTWAIN_CONSTANT_TWPH     48
#define DTWAIN_CONSTANT_TWCI     49
#define DTWAIN_CONSTANT_FONTNAME 50
#define DTWAIN_CONSTANT_TWEI     51
#define DTWAIN_CONSTANT_TWEJ     52
#define DTWAIN_CONSTANT_TWCC     53
#define DTWAIN_CONSTANT_TWQC     54
#define DTWAIN_CONSTANT_TWRC     55
#define DTWAIN_CONSTANT_MSG      56
#define DTWAIN_CONSTANT_TWLG     57
#define DTWAIN_CONSTANT_DLLINFO  58
#define DTWAIN_CONSTANT_DG       59
#define DTWAIN_CONSTANT_DAT      60
#define DTWAIN_CONSTANT_DF       61
#define DTWAIN_CONSTANT_TWTY     62
#define DTWAIN_CONSTANT_TWCB     63
#define DTWAIN_CONSTANT_TWAF     64
#define DTWAIN_CONSTANT_TWFS     65
#define DTWAIN_CONSTANT_TWJS     66
#define DTWAIN_CONSTANT_TWMR     67
#define DTWAIN_CONSTANT_TWDP     68
#define DTWAIN_CONSTANT_TWUS     69
#define DTWAIN_CONSTANT_TWDF     70
#define DTWAIN_CONSTANT_TWFM     71
#define DTWAIN_CONSTANT_TWSG     72
#define DTWAIN_CONSTANT_DTWAIN_TN 73
#define DTWAIN_CONSTANT_TWON     74
#define DTWAIN_CONSTANT_TWMF     75
#define DTWAIN_CONSTANT_TWSX     76
#define DTWAIN_CONSTANT_CAP      77
#define DTWAIN_CONSTANT_ICAP     78

/* This ID is the start of user-defined custom resources */
#define DTWAIN_USERRES_START     20000

/* Maximum length for a resource string*/
#define DTWAIN_USERRES_MAXSIZE   8192

#endif
