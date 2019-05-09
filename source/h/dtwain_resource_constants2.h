/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2019 Dynarithmic Software.

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
#ifndef DTWAIN_RESOURCE_CONSTANTS2_H
#define DTWAIN_RESOURCE_CONSTANTS2_H

#define IDS_ErrNullWindow           1
#define IDS_ErrAllocationFailure    2
#define IDS_ErrInvalidDLLHandle     3
#define IDS_ErrInvalidSourceHandle  4
#define IDS_ErrTwainDLLNotFound     5
#define IDS_ErrTwainDLLInvalid      6
#define IDS_ErrInvalidSessionHandle 7
#define IDS_ErrTwainMgrInvalid      8
#define IDS_ErrTwainDLLLoad         9
#define IDS_ErrTwainSourceOpen      10
#define IDS_ErrTwainSourceClose     11
#define IDS_ErrTwainSourceNotOpened 12
#define IDS_ErrTwainResolutionMatchError    27
#define IDS_ErrInvalidFileName      28
#define IDS_ErrInvalidFileFormat    29
#define IDS_ErrSourceMgrOpen        30
#define IDS_ErrSourceMgrClose       31
#define IDS_ErrTripletNotExecuted   32

#define IDS_ErrDTWAINStart            1000
#define DTWAIN_ERR_BAD_HANDLE_         1001
#define DTWAIN_ERR_BAD_SOURCE_         1002
#define DTWAIN_ERR_BAD_ARRAY_          1003
#define DTWAIN_ERR_WRONG_ARRAY_TYPE_   1004
#define DTWAIN_ERR_INDEX_BOUNDS_       1005
#define DTWAIN_ERR_OUT_OF_MEMORY_      1006
#define DTWAIN_ERR_NULL_WINDOW_        1007
#define DTWAIN_ERR_BAD_PIXTYPE_        1008
#define DTWAIN_ERR_BAD_CONTAINER_      1009
#define DTWAIN_ERR_NO_SESSION_         1010
#define DTWAIN_ERR_BAD_ACQUIRE_NUM_    1011
#define DTWAIN_ERR_BAD_CAP_            1012
#define DTWAIN_ERR_CAP_NO_SUPPORT_     1013
#define DTWAIN_ERR_TWAIN_              1014
#define DTWAIN_ERR_HOOK_FAILED_        1015
#define DTWAIN_ERR_BAD_FILENAME_       1016
#define DTWAIN_ERR_EMPTY_ARRAY_        1017
#define DTWAIN_ERR_FILE_FORMAT_        1018
#define DTWAIN_ERR_BAD_DIB_PAGE_       1019
#define DTWAIN_ERR_SOURCE_ACQUIRING_   1020
#define DTWAIN_ERR_INVALID_PARAM_      1021
#define DTWAIN_ERR_INVALID_RANGE_      1022
#define DTWAIN_ERR_UI_ERROR_           1023
#define DTWAIN_ERR_BAD_UNIT_           1024
#define DTWAIN_ERR_LANGDLL_NOT_FOUND_  1025
#define DTWAIN_ERR_SOURCE_NOT_OPEN_    1026
#define DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED_ 1027
#define DTWAIN_ERR_UIONLY_NOT_SUPPORTED_ 1028
#define DTWAIN_ERR_UI_ALREADY_OPENED_    1029
#define DTWAIN_ERR_CAPSET_NOSUPPORT_   1030
#define DTWAIN_ERR_NO_FILE_XFER_       1031
#define DTWAIN_ERR_INVALID_BITDEPTH_   1032
#define DTWAIN_ERR_NO_CAPS_DEFINED_    1033
#define DTWAIN_ERR_TILES_NOT_SUPPORTED_ 1034
#define DTWAIN_ERR_INVALID_DTWAIN_FRAME_ 1035
#define DTWAIN_ERR_LIMITED_VERSION_  1036
#define DTWAIN_ERR_NO_FEEDER_         1037
#define DTWAIN_ERR_NO_FEEDER_QUERY_   1038
#define DTWAIN_ERR_EXCEPTION_ERROR_   1039
#define DTWAIN_ERR_INVALID_STATE_     1040
#define DTWAIN_ERR_UNSUPPORTED_EXTINFO_  1041
#define DTWAIN_ERR_DLLRESOURCE_NOTFOUND_ 1042
#define DTWAIN_ERR_NOT_INITIALIZED_      1043
#define DTWAIN_ERR_NO_SOURCES_           1044
#define DTWAIN_ERR_TWAIN_NOT_INSTALLED_  1045
#define DTWAIN_ERR_WRONG_THREAD_         1046
#define DTWAIN_ERR_BAD_CAPTYPE_          1047
#define DTWAIN_ERR_UNKNOWN_CAPDATATYPE_  1048
#define DTWAIN_ERR_DEMO_NOFILETYPE_      1049

#define TWAIN_ERR_LOW_MEMORY_        1100
#define TWAIN_ERR_FALSE_ALARM_       1101
#define TWAIN_ERR_BUMMER_            1102
#define TWAIN_ERR_NODATASOURCE_      1103
#define TWAIN_ERR_MAXCONNECTIONS_    1104
#define TWAIN_ERR_OPERATIONERROR_    1105
#define TWAIN_ERR_BADCAPABILITY_     1106
#define TWAIN_ERR_BADVALUE_          1107
#define TWAIN_ERR_BADPROTOCOL_       1108
#define TWAIN_ERR_SEQUENCEERROR_     1109
#define TWAIN_ERR_BADDESTINATION_    1110
#define TWAIN_ERR_CAPNOTSUPPORTED_   1111
#define TWAIN_ERR_CAPBADOPERATION_   1112
#define TWAIN_ERR_CAPSEQUENCEERROR_  1113
#define TWAIN_ERR_FILEPROTECTEDERROR_ 1114
#define TWAIN_ERR_FILEEXISTERROR_    1115
#define TWAIN_ERR_FILENOTFOUND_      1116
#define TWAIN_ERR_DIRNOTEMPTY_       1117
#define TWAIN_ERR_FEEDERJAMMED_      1118
#define TWAIN_ERR_FEEDERMULTPAGES_   1119
#define TWAIN_ERR_FEEDERWRITEERROR_  1120
#define TWAIN_ERR_DEVICEOFFLINE_     1121
#define TWAIN_ERR_NULL_CONTAINER_    1122
#define TWAIN_ERR_INTERLOCK_         1123
#define TWAIN_ERR_DAMAGEDCORNER_     1124
#define TWAIN_ERR_FOCUSERROR_        1125
#define TWAIN_ERR_DOCTOOLIGHT_       1126
#define TWAIN_ERR_DOCTOODARK_        1127
#define TWAIN_ERR_NOMEDIA_           1128

#define DTWAIN_ERR_FILEXFERSTART_   2000
#define DTWAIN_ERR_MEM_             2001
#define DTWAIN_ERR_FILEOPEN_        2002
#define DTWAIN_ERR_FILEREAD_        2003
#define DTWAIN_ERR_FILEWRITE_       2004
#define DTWAIN_ERR_BADPARAM_        2005
#define DTWAIN_ERR_INVALIDBMP_      2006
#define DTWAIN_ERR_BMPRLE_          2007
#define DTWAIN_ERR_RESERVED1_       2008
#define DTWAIN_ERR_INVALIDJPG_      2009
#define DTWAIN_ERR_DC_              2010
#define DTWAIN_ERR_DIB_             2011
#define DTWAIN_ERR_RESERVED2_       2012
#define DTWAIN_ERR_NORESOURCE_      2013
#define DTWAIN_ERR_CALLBACKCANCEL_  2014
#define DTWAIN_ERR_INVALIDPNG_      2015
#define DTWAIN_ERR_PNGCREATE_       2016
#define DTWAIN_ERR_INTERNAL_        2017
#define DTWAIN_ERR_FONT_            2018
#define DTWAIN_ERR_INTTIFF_         2019
#define DTWAIN_ERR_INVALIDTIFF_     2020
#define DTWAIN_ERR_NOTIFFLZW_       2021
#define DTWAIN_ERR_INVALIDPCX_      2022
#define DTWAIN_ERR_CREATEBMP_       2023
#define DTWAIN_ERR_NOLINES_         2024
#define DTWAIN_ERR_GETDIB_          2025
#define DTWAIN_ERR_NODEVOP_         2026
#define DTWAIN_ERR_INVALIDWMF_      2027
#define DTWAIN_ERR_DEPTHMISMATCH_   2028
#define DTWAIN_ERR_BITBLT_          2029
#define DTWAIN_ERR_BUFTOOSMALL_     2030
#define DTWAIN_ERR_TOOMANYCOLORS_   2031
#define DTWAIN_ERR_INVALIDTGA_      2032
#define DTWAIN_ERR_NOTGATHUMBNAIL_  2033
#define DTWAIN_ERR_RESERVED3_       2034
#define DTWAIN_ERR_CREATEDIB_       2035
#define DTWAIN_ERR_NOLZW_           2036
#define DTWAIN_ERR_SELECTOBJ_       2037
#define DTWAIN_ERR_BADMANAGER_      2038
#define DTWAIN_ERR_OBSOLETE_        2039
#define DTWAIN_ERR_CREATEDIBSECTION_      2040
#define DTWAIN_ERR_SETWINMETAFILEBITS_    2041
#define DTWAIN_ERR_GETWINMETAFILEBITS_    2042
#define DTWAIN_ERR_PAXPWD_                2043
#define DTWAIN_ERR_INVALIDPAX_            2044
#define DTWAIN_ERR_NOSUPPORT_             2045
#define DTWAIN_ERR_INVALIDPSD_            2046
#define DTWAIN_ERR_PSDNOTSUPPORTED_       2047
#define DTWAIN_ERR_DECRYPT_         2048
#define DTWAIN_ERR_ENCRYPT_         2049
#define DTWAIN_ERR_COMPRESSION_     2050
#define DTWAIN_ERR_DECOMPRESSION_   2051
#define DTWAIN_ERR_INVALIDTLA_      2052
#define DTWAIN_ERR_INVALIDWBMP_     2053
#define DTWAIN_ERR_NOTIFFTAG_       2054
#define DTWAIN_ERR_NOLOCALSTORAGE_  2055
#define DTWAIN_ERR_INVALIDEXIF_     2056
#define DTWAIN_ERR_NOEXIFSTRING_    2057
#define DTWAIN_ERR_TIFFDLL32NOTFOUND_  2058
#define DTWAIN_ERR_TIFFDLL16NOTFOUND_  2059
#define DTWAIN_ERR_PNGDLL16NOTFOUND_    2060
#define DTWAIN_ERR_JPEGDLL16NOTFOUND_   2061
#define DTWAIN_ERR_BADBITSPERPIXEL_  2062
#define DTWAIN_ERR_TIFFDLL32INVALIDVER_     2063
#define DTWAIN_ERR_PDFDLL32NOTFOUND_        2064
#define DTWAIN_ERR_PDFDLL32INVALIDVER_      2065
#define DTWAIN_ERR_JPEGDLL32NOTFOUND_       2066
#define DTWAIN_ERR_JPEGDLL32INVALIDVER_     2067
#define DTWAIN_ERR_PNGDLL32NOTFOUND_        2068
#define DTWAIN_ERR_PNGDLL32INVALIDVER_      2069
#define DTWAIN_ERR_J2KDLL32NOTFOUND_        2070
#define DTWAIN_ERR_J2KDLL32INVALIDVER_      2071
#define DTWAIN_ERR_MANDUPLEX_UNAVAILABLE_   2072
#define DTWAIN_ERR_TIMEOUT_                 2073
#define DTWAIN_ERR_INVALIDICONFORMAT_       2074
#define DTWAIN_ERR_TWAIN32DSMNOTFOUND_      2075
#define DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND_ 2076

#define DTWAIN_ERR_TS_FIRST_                2080
#define DTWAIN_ERR_TS_NOFILENAME_           2081
#define DTWAIN_ERR_TS_NOTWAINSYS_           2082
#define DTWAIN_ERR_TS_DEVICEFAILURE_        2083
#define DTWAIN_ERR_TS_FILESAVEERROR_        2084
#define DTWAIN_ERR_TS_COMMANDILLEGAL_       2085
#define DTWAIN_ERR_TS_CANCELLED_            2086
#define DTWAIN_ERR_TS_ACQUISITIONERROR_     2087
#define DTWAIN_ERR_TS_INVALIDCOLORSPACE_    2088
#define DTWAIN_ERR_TS_PDFNOTSUPPORTED_      2089
#define DTWAIN_ERR_TS_NOTAVAILABLE_         2090

// OCR errors
#define DTWAIN_ERR_OCR_INVALIDPAGENUM_      2100
#define DTWAIN_ERR_OCR_INVALIDENGINE_       2101
#define DTWAIN_ERR_OCR_NOTACTIVE_           2102
#define DTWAIN_ERR_OCR_INVALIDFILETYPE_     2103
#define DTWAIN_ERR_OCR_INVALIDPIXELTYPE_    2104
#define DTWAIN_ERR_OCR_INVALIDBITDEPTH_     2105
#define DTWAIN_ERR_OCR_RECOGNITIONERROR_    2106

// PDF Font text
#define DTWAIN_FONT_START_                 5000
#define DTWAIN_FONT_COURIER_              (DTWAIN_FONT_START_ + 0)
#define DTWAIN_FONT_COURIERBOLD_          (DTWAIN_FONT_START_ + 1)
#define DTWAIN_FONT_COURIERBOLDOBLIQUE_   (DTWAIN_FONT_START_ + 2)
#define DTWAIN_FONT_COURIEROBLIQUE_       (DTWAIN_FONT_START_ + 3)
#define DTWAIN_FONT_HELVETICA_            (DTWAIN_FONT_START_ + 4)
#define DTWAIN_FONT_HELVETICABOLD_        (DTWAIN_FONT_START_ + 5)
#define DTWAIN_FONT_HELVETICABOLDOBLIQUE_ (DTWAIN_FONT_START_ + 6)
#define DTWAIN_FONT_HELVETICAOBLIQUE_     (DTWAIN_FONT_START_ + 7)
#define DTWAIN_FONT_TIMESBOLD_            (DTWAIN_FONT_START_ + 8)
#define DTWAIN_FONT_TIMESBOLDITALIC_      (DTWAIN_FONT_START_ + 9)
#define DTWAIN_FONT_TIMESROMAN_           (DTWAIN_FONT_START_ + 10)
#define DTWAIN_FONT_TIMESITALIC_          (DTWAIN_FONT_START_ + 11)
#define DTWAIN_FONT_SYMBOL_               (DTWAIN_FONT_START_ + 12)
#define DTWAIN_FONT_ZAPFDINGBATS_         (DTWAIN_FONT_START_ + 13)

// Select Source dialog contants
#define IDS_SELECT_SOURCE_TEXT              3000
#define IDS_SELECT_TEXT                     3001
#define IDS_CANCEL_TEXT                     3002
#define IDS_SOURCES_TEXT                    3003

#define IDS_LOGMSG_START                    3005
#define IDS_LOGMSG_ENTERTEXT                (IDS_LOGMSG_START + 0)
#define IDS_LOGMSG_EXITTEXT                 (IDS_LOGMSG_START + 1)
#define IDS_LOGMSG_RETURNTEXT               (IDS_LOGMSG_START + 2)
#define IDS_LOGMSG_EXCEPTERRORTEXT          (IDS_LOGMSG_START + 3)
#define IDS_LOGMSG_MODULETEXT               (IDS_LOGMSG_START + 4)
#define IDS_LOGMSG_NOPARAMINFOTEXT          (IDS_LOGMSG_START + 5)
#define IDS_LOGMSG_NOINFOERRORTEXT          (IDS_LOGMSG_START + 6)
#define IDS_LOGMSG_INPUTTEXT                (IDS_LOGMSG_START + 7)
#define IDS_LOGMSG_OUTPUTDSMTEXT            (IDS_LOGMSG_START + 8)
#define IDS_LOGMSG_END                      (IDS_LOGMSG_START + 8)

#define IDS_ErrCCLowMemory          TWAIN_ERR_LOW_MEMORY_
#define IDS_ErrCCFalseAlarm         TWAIN_ERR_FALSE_ALARM_
#define IDS_ErrCCBummer             TWAIN_ERR_BUMMER_
#define IDS_ErrCCNoDataSource       TWAIN_ERR_NODATASOURCE_
#define IDS_ErrCCMaxConnections     TWAIN_ERR_MAXCONNECTIONS_
#define IDS_ErrCCOperationError     TWAIN_ERR_OPERATIONERROR_
#define IDS_ErrCCBadCapability      TWAIN_ERR_BADCAPABILITY_
#define IDS_ErrCCBadValue           TWAIN_ERR_BADVALUE_
#define IDS_ErrCCBadProtocol        TWAIN_ERR_BADPROTOCOL_
#define IDS_ErrCCSequenceError      TWAIN_ERR_SEQUENCEERROR_
#define IDS_ErrCCBadDestination     TWAIN_ERR_BADDESTINATION_
#define IDS_ErrCCCapNotSupported    TWAIN_ERR_CAPNOTSUPPORTED_
#define IDS_ErrCCCapBadOperation    TWAIN_ERR_CAPBADOPERATION_
#define IDS_ErrCCCapSequenceError   TWAIN_ERR_CAPSEQUENCEERROR_

#define IDS_ErrCCFileProtected      TWAIN_ERR_FILEPROTECTEDERROR_
#define IDS_ErrCCFileExists         TWAIN_ERR_FILEEXISTERROR_
#define IDS_ErrCCFileNotFound       TWAIN_ERR_FILENOTFOUND_
#define IDS_ErrCCDirectoryNotEmpty  TWAIN_ERR_DIRNOTEMPTY_
#define IDS_ErrCCFeederJammed       TWAIN_ERR_FEEDERJAMMED_
#define IDS_ErrCCFeederMultPages    TWAIN_ERR_FEEDERMULTPAGES_
#define IDS_ErrCCFileWriteError     TWAIN_ERR_FEEDERWRITEERROR_
#define IDS_ErrCCDeviceOffline      TWAIN_ERR_DEVICEOFFLINE_
#define IDS_ErrCCInterlock          TWAIN_ERR_INTERLOCK_
#define IDS_ErrCCDamagedCorner      TWAIN_ERR_DAMAGEDCORNER_
#define IDS_ErrCCFocusError         TWAIN_ERR_FOCUSERROR_
#define IDS_ErrCCDoctooLight        TWAIN_ERR_DOCTOOLIGHT_
#define IDS_ErrCCDoctooDark         TWAIN_ERR_DOCTOODARK_
#define IDS_ErrCCNoMedia            TWAIN_ERR_NOMEDIA_

#define IDC_TWAINDATA           8888
#define IDC_TWAINDEBUGDATA      8889
#define IDC_TWAINDGDATA         8890
#define IDC_TWAINDATDATA        8891
#define IDC_TWAINMSGDATA        8892
#define IDC_TWAINFILEDATA       8893
#define IDS_LIMITEDFUNCMSG1     8894
#define IDS_LIMITEDFUNCMSG2     8895
#define IDS_LIMITEDFUNCMSG3     8896
#define IDS_TWCCBASE            9500
#define IDS_TWRCBASE            9600
#define IDS_TWCC_EXCEPTION      9999
#define IDS_DTWAINFUNCSTART     9001

#define IDS_TWCC_ERRORSTART        9500
#define IDS_TWCC_SUCCESS           9500 /* OkIt worked!                                */
#define IDS_TWCC_BUMMER            9501 /* Failure due to unknown causes             */
#define IDS_TWCC_LOWMEMORY         9502 /* Not enough memory to perform operation    */
#define IDS_TWCC_NODS              9503 /* No Data Source                            */
#define IDS_TWCC_MAXCONNECTIONS    9504 /* DS is connected to max possible applications      */
#define IDS_TWCC_OPERATIONERROR    9505 /* DS or DSM reported error, application shouldn't   */
#define IDS_TWCC_BADCAP            9506 /* Unknown capability                        */
#define IDS_TWCC_BADPROTOCOL       9509 /* Unrecognized MSG DG DAT combination       */
#define IDS_TWCC_BADVALUE          9510 /* Data parameter out of range              */
#define IDS_TWCC_SEQERROR          9511 /* DG DAT MSG out of expected sequence      */
#define IDS_TWCC_BADDEST           9512 /* Unknown destination Application/Source in DSM_Entry */
#define IDS_TWCC_CAPUNSUPPORTED    9513 /* Capability not supported by source            */
#define IDS_TWCC_CAPBADOPERATION   9514 /* Operation not supported by capability         */
#define IDS_TWCC_CAPSEQERROR       9515 /* Capability has dependancy on other capability */
#define IDS_TWCC_DENIED            9516 /* File System operation is denied (file is protected) */
#define IDS_TWCC_FILEEXISTS        9517 /* Operation failed because file already exists. */
#define IDS_TWCC_FILENOTFOUND      9518 /* File not found */
#define IDS_TWCC_NOTEMPTY          9519 /* Operation failed because directory is not empty */
#define IDS_TWCC_PAPERJAM          9520  /* The feeder is jammed */
#define IDS_TWCC_PAPERDOUBLEFEED   9521  /* The feeder detected multiple pages */
#define IDS_TWCC_FILEWRITEERROR    9522  /* Error writing the file (meant for things like disk full conditions) */
#define IDS_TWCC_CHECKDEVICEONLINE 9523  /* The device went offline prior to or during this operation */

#define IDS_TWRC_ERRORSTART       9600
#define IDS_TWRC_SUCCESS          9600
#define IDS_TWRC_FAILURE          9601
#define IDS_TWRC_CHECKSTATUS      9602
#define IDS_TWRC_CANCEL           9603
#define IDS_TWRC_DSEVENT          9604
#define IDS_TWRC_NOTDSEVENT       9605
#define IDS_TWRC_XFERDONE         9606
#define IDS_TWRC_ENDOFLIST        9607
#define IDS_TWRC_INFONOTSUPPORTED 9608
#define IDS_TWRC_DATANOTAVAILABLE 9609
#define IDS_DTWAIN_APPTITLE       9700

#define IDS_TWCC_EXCEPTION      9999
//#define VS_VERSION_INFO     9000
#define IDS_DTWAINFUNCSTART     9001


#define DTW_CONTARRAY           8
#define DTW_CONTENUMERATION     16
#define DTW_CONTONEVALUE        32
#define DTW_CONTRANGE           64

#define DTW_FF_TIFF        0
#define DTW_FF_PICT        1
#define DTW_FF_BMP         2
#define DTW_FF_XBM         3
#define DTW_FF_JFIF        4
#define DTW_FF_FPX         5
#define DTW_FF_TIFFMULTI   6
#define DTW_FF_PNG         7
#define DTW_FF_SPIFF       8
#define DTW_FF_EXIF        9
#define DTW_BMP          DTW_FF_BMP
#define DTW_JPEG         DTW_FF_JFIF
#define DTW_PCX          10
#define DTW_TGA          11
#define DTW_TIFFLZW      12
#define DTW_TIFFNONE     DTW_FF_TIFF
#define DTW_TIFFG3       13
#define DTW_TIFFG4       14
#define DTW_GIF          15
#define DTW_PNG          DTW_FF_PNG

#define LTWRC_SUCCESS          0L
#define LTWRC_FAILURE          1L
#define LTWRC_CHECKSTATUS      2L
#define LTWRC_CANCEL           3L
#define LTWRC_DSEVENT          4L
#define LTWRC_NOTDSEVENT       5L
#define LTWRC_XFERDONE         6L
#define LTWRC_ENDOFLIST        7L
#define LTWRC_INFONOTSUPPORTED 8L
#define LTWRC_DATANOTAVAILABLE 9L

#define LTWCC_SUCCESS            0L /* It worked!                                */
#define LTWCC_BUMMER             1L /* Failure due to unknown causes             */
#define LTWCC_LOWMEMORY          2L /* Not enough memory to perform operation    */
#define LTWCC_NODS               3L /* No Data Source                            */
#define LTWCC_MAXCONNECTIONS     4L /* DS is connected to max possible applications      */
#define LTWCC_OPERATIONERROR     5L /* DS or DSM reported error, application shouldn't   */
#define LTWCC_BADCAP             6L /* Unknown capability                        */
#define LTWCC_BADPROTOCOL        9L /* Unrecognized MSG DG DAT combination       */
#define LTWCC_BADVALUE           10L /* Data parameter out of range              */
#define LTWCC_SEQERROR           11L /* DG DAT MSG out of expected sequence      */
#define LTWCC_BADDEST            12L /* Unknown destination Application/Source in DSM_Entry */
#define LTWCC_CAPUNSUPPORTED     13L /* Capability not supported by source            */
#define LTWCC_CAPBADOPERATION    14L /* Operation not supported by capability         */
#define LTWCC_CAPSEQERROR        15L /* Capability has dependancy on other capability */
#define LTWCC_DENIED             16L /* File System operation is denied (file is protected) */
#define LTWCC_FILEEXISTS         17L /* Operation failed because file already exists. */
#define LTWCC_FILENOTFOUND       18L /* File not found */
#define LTWCC_NOTEMPTY           19L /* Operation failed because directory is not empty */
#define LTWCC_PAPERJAM           20L  /* The feeder is jammed */
#define LTWCC_PAPERDOUBLEFEED    21L  /* The feeder detected multiple pages */
#define LTWCC_FILEWRITEERROR     22L  /* Error writing the file (meant for things like disk full conditions) */
#define LTWCC_CHECKDEVICEONLINE  23L  /* The device went offline prior to or during this operation */

#define IDC_DLGSELECTSOURCE      10000
#define IDC_LSTSOURCES           10001
#define IDC_SOURCETEXT           10002
#define IDC_STATIC               -1

// version string
#define IDS_DTWAIN_VERSIONSTRING_MAJOR      11000
#define IDS_DTWAIN_VERSIONSTRING_MINOR      11001
#define IDS_DTWAIN_VERSIONSTRING_SUBBUILD1  11002
#define IDS_DTWAIN_VERSIONSTRING_SUBBUILD2  11003

#endif
