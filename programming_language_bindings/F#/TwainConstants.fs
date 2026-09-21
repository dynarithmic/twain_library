(*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2026 Dynarithmic Software.

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
*)

namespace Dynarithmic

[<RequireQualifiedAccess>]
module TwainConstants =
    [<Literal>]
    let TWON_PROTOCOLMINOR = 5
    [<Literal>]
    let TWON_PROTOCOLMAJOR = 2
    [<Literal>]
    let TWON_ARRAY = 3
    [<Literal>]
    let TWON_ENUMERATION = 4
    [<Literal>]
    let TWON_ONEVALUE = 5
    [<Literal>]
    let TWON_RANGE = 6
    [<Literal>]
    let TWON_ICONID = 962
    [<Literal>]
    let TWON_DSMID = 461
    [<Literal>]
    let TWON_DSMCODEID = 63
    [<Literal>]
    let TWON_DONTCARE8 = 0xff
    [<Literal>]
    let TWON_DONTCARE16 = 0xffff
    [<Literal>]
    let TWON_DONTCARE32 = 0xffffffffu
    [<Literal>]
    let TWMF_APPOWNS = 0x0001
    [<Literal>]
    let TWMF_DSMOWNS = 0x0002
    [<Literal>]
    let TWMF_DSOWNS = 0x0004
    [<Literal>]
    let TWMF_POINTER = 0x0008
    [<Literal>]
    let TWMF_HANDLE = 0x0010
    [<Literal>]
    let TWTY_INT8 = 0x0000
    [<Literal>]
    let TWTY_INT16 = 0x0001
    [<Literal>]
    let TWTY_INT32 = 0x0002
    [<Literal>]
    let TWTY_UINT8 = 0x0003
    [<Literal>]
    let TWTY_UINT16 = 0x0004
    [<Literal>]
    let TWTY_UINT32 = 0x0005
    [<Literal>]
    let TWTY_BOOL = 0x0006
    [<Literal>]
    let TWTY_FIX32 = 0x0007
    [<Literal>]
    let TWTY_FRAME = 0x0008
    [<Literal>]
    let TWTY_STR32 = 0x0009
    [<Literal>]
    let TWTY_STR64 = 0x000a
    [<Literal>]
    let TWTY_STR128 = 0x000b
    [<Literal>]
    let TWTY_STR255 = 0x000c
    [<Literal>]
    let TWTY_HANDLE = 0x000f
    [<Literal>]
    let TWAL_ALARM = 0
    [<Literal>]
    let TWAL_FEEDERERROR = 1
    [<Literal>]
    let TWAL_FEEDERWARNING = 2
    [<Literal>]
    let TWAL_BARCODE = 3
    [<Literal>]
    let TWAL_DOUBLEFEED = 4
    [<Literal>]
    let TWAL_JAM = 5
    [<Literal>]
    let TWAL_PATCHCODE = 6
    [<Literal>]
    let TWAL_POWER = 7
    [<Literal>]
    let TWAL_SKEW = 8
    [<Literal>]
    let TWAS_NONE = 0
    [<Literal>]
    let TWAS_AUTO = 1
    [<Literal>]
    let TWAS_CURRENT = 2
    [<Literal>]
    let TWBCOR_ROT0 = 0
    [<Literal>]
    let TWBCOR_ROT90 = 1
    [<Literal>]
    let TWBCOR_ROT180 = 2
    [<Literal>]
    let TWBCOR_ROT270 = 3
    [<Literal>]
    let TWBCOR_ROTX = 4
    [<Literal>]
    let TWBD_HORZ = 0
    [<Literal>]
    let TWBD_VERT = 1
    [<Literal>]
    let TWBD_HORZVERT = 2
    [<Literal>]
    let TWBD_VERTHORZ = 3
    [<Literal>]
    let TWBO_LSBFIRST = 0
    [<Literal>]
    let TWBO_MSBFIRST = 1
    [<Literal>]
    let TWBP_DISABLE = -2
    [<Literal>]
    let TWBP_AUTO = -1
    [<Literal>]
    let TWBR_THRESHOLD = 0
    [<Literal>]
    let TWBR_HALFTONE = 1
    [<Literal>]
    let TWBR_CUSTHALFTONE = 2
    [<Literal>]
    let TWBR_DIFFUSION = 3
    [<Literal>]
    let TWBR_DYNAMICTHRESHOLD = 4
    [<Literal>]
    let TWBT_3OF9 = 0
    [<Literal>]
    let TWBT_2OF5INTERLEAVED = 1
    [<Literal>]
    let TWBT_2OF5NONINTERLEAVED = 2
    [<Literal>]
    let TWBT_CODE93 = 3
    [<Literal>]
    let TWBT_CODE128 = 4
    [<Literal>]
    let TWBT_UCC128 = 5
    [<Literal>]
    let TWBT_CODABAR = 6
    [<Literal>]
    let TWBT_UPCA = 7
    [<Literal>]
    let TWBT_UPCE = 8
    [<Literal>]
    let TWBT_EAN8 = 9
    [<Literal>]
    let TWBT_EAN13 = 10
    [<Literal>]
    let TWBT_POSTNET = 11
    [<Literal>]
    let TWBT_PDF417 = 12
    [<Literal>]
    let TWBT_2OF5INDUSTRIAL = 13
    [<Literal>]
    let TWBT_2OF5MATRIX = 14
    [<Literal>]
    let TWBT_2OF5DATALOGIC = 15
    [<Literal>]
    let TWBT_2OF5IATA = 16
    [<Literal>]
    let TWBT_3OF9FULLASCII = 17
    [<Literal>]
    let TWBT_CODABARWITHSTARTSTOP = 18
    [<Literal>]
    let TWBT_MAXICODE = 19
    [<Literal>]
    let TWBT_QRCODE = 20
    [<Literal>]
    let TWCP_NONE = 0
    [<Literal>]
    let TWCP_PACKBITS = 1
    [<Literal>]
    let TWCP_GROUP31D = 2
    [<Literal>]
    let TWCP_GROUP31DEOL = 3
    [<Literal>]
    let TWCP_GROUP32D = 4
    [<Literal>]
    let TWCP_GROUP4 = 5
    [<Literal>]
    let TWCP_JPEG = 6
    [<Literal>]
    let TWCP_LZW = 7
    [<Literal>]
    let TWCP_JBIG = 8
    [<Literal>]
    let TWCP_PNG = 9
    [<Literal>]
    let TWCP_RLE4 = 10
    [<Literal>]
    let TWCP_RLE8 = 11
    [<Literal>]
    let TWCP_BITFIELDS = 12
    [<Literal>]
    let TWCP_ZIP = 13
    [<Literal>]
    let TWCP_JPEG2000 = 14
    [<Literal>]
    let TWCS_BOTH = 0
    [<Literal>]
    let TWCS_TOP = 1
    [<Literal>]
    let TWCS_BOTTOM = 2
    [<Literal>]
    let TWDE_CUSTOMEVENTS = 0x8000
    [<Literal>]
    let TWDE_CHECKAUTOMATICCAPTURE = 0
    [<Literal>]
    let TWDE_CHECKBATTERY = 1
    [<Literal>]
    let TWDE_CHECKDEVICEONLINE = 2
    [<Literal>]
    let TWDE_CHECKFLASH = 3
    [<Literal>]
    let TWDE_CHECKPOWERSUPPLY = 4
    [<Literal>]
    let TWDE_CHECKRESOLUTION = 5
    [<Literal>]
    let TWDE_DEVICEADDED = 6
    [<Literal>]
    let TWDE_DEVICEOFFLINE = 7
    [<Literal>]
    let TWDE_DEVICEREADY = 8
    [<Literal>]
    let TWDE_DEVICEREMOVED = 9
    [<Literal>]
    let TWDE_IMAGECAPTURED = 10
    [<Literal>]
    let TWDE_IMAGEDELETED = 11
    [<Literal>]
    let TWDE_PAPERDOUBLEFEED = 12
    [<Literal>]
    let TWDE_PAPERJAM = 13
    [<Literal>]
    let TWDE_LAMPFAILURE = 14
    [<Literal>]
    let TWDE_POWERSAVE = 15
    [<Literal>]
    let TWDE_POWERSAVENOTIFY = 16
    [<Literal>]
    let TWDR_GET = 1
    [<Literal>]
    let TWDR_SET = 2
    [<Literal>]
    let TWDSK_SUCCESS = 0
    [<Literal>]
    let TWDSK_REPORTONLY = 1
    [<Literal>]
    let TWDSK_FAIL = 2
    [<Literal>]
    let TWDSK_DISABLED = 3
    [<Literal>]
    let TWDX_NONE = 0
    [<Literal>]
    let TWDX_1PASSDUPLEX = 1
    [<Literal>]
    let TWDX_2PASSDUPLEX = 2
    [<Literal>]
    let TWFA_NONE = 0
    [<Literal>]
    let TWFA_LEFT = 1
    [<Literal>]
    let TWFA_CENTER = 2
    [<Literal>]
    let TWFA_RIGHT = 3
    [<Literal>]
    let TWFE_GENERAL = 0
    [<Literal>]
    let TWFE_PHOTO = 1
    [<Literal>]
    let TWFF_TIFF = 0
    [<Literal>]
    let TWFF_PICT = 1
    [<Literal>]
    let TWFF_BMP = 2
    [<Literal>]
    let TWFF_XBM = 3
    [<Literal>]
    let TWFF_JFIF = 4
    [<Literal>]
    let TWFF_FPX = 5
    [<Literal>]
    let TWFF_TIFFMULTI = 6
    [<Literal>]
    let TWFF_PNG = 7
    [<Literal>]
    let TWFF_SPIFF = 8
    [<Literal>]
    let TWFF_EXIF = 9
    [<Literal>]
    let TWFF_PDF = 10
    [<Literal>]
    let TWFF_JP2 = 11
    [<Literal>]
    let TWFF_JPX = 13
    [<Literal>]
    let TWFF_DEJAVU = 14
    [<Literal>]
    let TWFF_PDFA = 15
    [<Literal>]
    let TWFF_PDFA2 = 16
    [<Literal>]
    let TWFF_PDFRASTER = 17
    [<Literal>]
    let TWFL_NONE = 0
    [<Literal>]
    let TWFL_OFF = 1
    [<Literal>]
    let TWFL_ON = 2
    [<Literal>]
    let TWFL_AUTO = 3
    [<Literal>]
    let TWFL_REDEYE = 4
    [<Literal>]
    let TWFO_FIRSTPAGEFIRST = 0
    [<Literal>]
    let TWFO_LASTPAGEFIRST = 1
    [<Literal>]
    let TWFP_POCKETERROR = 0
    [<Literal>]
    let TWFP_POCKET1 = 1
    [<Literal>]
    let TWFP_POCKET2 = 2
    [<Literal>]
    let TWFP_POCKET3 = 3
    [<Literal>]
    let TWFP_POCKET4 = 4
    [<Literal>]
    let TWFP_POCKET5 = 5
    [<Literal>]
    let TWFP_POCKET6 = 6
    [<Literal>]
    let TWFP_POCKET7 = 7
    [<Literal>]
    let TWFP_POCKET8 = 8
    [<Literal>]
    let TWFP_POCKET9 = 9
    [<Literal>]
    let TWFP_POCKET10 = 10
    [<Literal>]
    let TWFP_POCKET11 = 11
    [<Literal>]
    let TWFP_POCKET12 = 12
    [<Literal>]
    let TWFP_POCKET13 = 13
    [<Literal>]
    let TWFP_POCKET14 = 14
    [<Literal>]
    let TWFP_POCKET15 = 15
    [<Literal>]
    let TWFP_POCKET16 = 16
    [<Literal>]
    let TWFR_BOOK = 0
    [<Literal>]
    let TWFR_FANFOLD = 1
    [<Literal>]
    let TWFT_RED = 0
    [<Literal>]
    let TWFT_GREEN = 1
    [<Literal>]
    let TWFT_BLUE = 2
    [<Literal>]
    let TWFT_NONE = 3
    [<Literal>]
    let TWFT_WHITE = 4
    [<Literal>]
    let TWFT_CYAN = 5
    [<Literal>]
    let TWFT_MAGENTA = 6
    [<Literal>]
    let TWFT_YELLOW = 7
    [<Literal>]
    let TWFT_BLACK = 8
    [<Literal>]
    let TWFY_CAMERA = 0
    [<Literal>]
    let TWFY_CAMERATOP = 1
    [<Literal>]
    let TWFY_CAMERABOTTOM = 2
    [<Literal>]
    let TWFY_CAMERAPREVIEW = 3
    [<Literal>]
    let TWFY_DOMAIN = 4
    [<Literal>]
    let TWFY_HOST = 5
    [<Literal>]
    let TWFY_DIRECTORY = 6
    [<Literal>]
    let TWFY_IMAGE = 7
    [<Literal>]
    let TWFY_UNKNOWN = 8
    [<Literal>]
    let TWIA_UNUSED = 0
    [<Literal>]
    let TWIA_FIXED = 1
    [<Literal>]
    let TWIA_LEVEL1 = 2
    [<Literal>]
    let TWIA_LEVEL2 = 3
    [<Literal>]
    let TWIA_LEVEL3 = 4
    [<Literal>]
    let TWIA_LEVEL4 = 5
    [<Literal>]
    let TWIC_NONE = 0
    [<Literal>]
    let TWIC_LINK = 1
    [<Literal>]
    let TWIC_EMBED = 2
    [<Literal>]
    let TWIF_NONE = 0
    [<Literal>]
    let TWIF_AUTO = 1
    [<Literal>]
    let TWIF_LOWPASS = 2
    [<Literal>]
    let TWIF_BANDPASS = 3
    [<Literal>]
    let TWIF_HIGHPASS = 4
    [<Literal>]
    let TWIF_TEXT = TWIF_BANDPASS
    [<Literal>]
    let TWIF_FINELINE = TWIF_HIGHPASS
    [<Literal>]
    let TWIM_NONE = 0
    [<Literal>]
    let TWIM_FRONTONTOP = 1
    [<Literal>]
    let TWIM_FRONTONBOTTOM = 2
    [<Literal>]
    let TWIM_FRONTONLEFT = 3
    [<Literal>]
    let TWIM_FRONTONRIGHT = 4
    [<Literal>]
    let TWJC_NONE = 0
    [<Literal>]
    let TWJC_JSIC = 1
    [<Literal>]
    let TWJC_JSIS = 2
    [<Literal>]
    let TWJC_JSXC = 3
    [<Literal>]
    let TWJC_JSXS = 4
    [<Literal>]
    let TWJQ_UNKNOWN = -4
    [<Literal>]
    let TWJQ_LOW = -3
    [<Literal>]
    let TWJQ_MEDIUM = -2
    [<Literal>]
    let TWJQ_HIGH = -1
    [<Literal>]
    let TWLP_REFLECTIVE = 0
    [<Literal>]
    let TWLP_TRANSMISSIVE = 1
    [<Literal>]
    let TWLS_RED = 0
    [<Literal>]
    let TWLS_GREEN = 1
    [<Literal>]
    let TWLS_BLUE = 2
    [<Literal>]
    let TWLS_NONE = 3
    [<Literal>]
    let TWLS_WHITE = 4
    [<Literal>]
    let TWLS_UV = 5
    [<Literal>]
    let TWLS_IR = 6
    [<Literal>]
    let TWMD_MICR = 0
    [<Literal>]
    let TWMD_RAW = 1
    [<Literal>]
    let TWMD_INVALID = 2
    [<Literal>]
    let TWNF_NONE = 0
    [<Literal>]
    let TWNF_AUTO = 1
    [<Literal>]
    let TWNF_LONEPIXEL = 2
    [<Literal>]
    let TWNF_MAJORITYRULE = 3
    [<Literal>]
    let TWOR_ROT0 = 0
    [<Literal>]
    let TWOR_ROT90 = 1
    [<Literal>]
    let TWOR_ROT180 = 2
    [<Literal>]
    let TWOR_ROT270 = 3
    [<Literal>]
    let TWOR_PORTRAIT = TWOR_ROT0
    [<Literal>]
    let TWOR_LANDSCAPE = TWOR_ROT270
    [<Literal>]
    let TWOR_AUTO = 4
    [<Literal>]
    let TWOR_AUTOTEXT = 5
    [<Literal>]
    let TWOR_AUTOPICTURE = 6
    [<Literal>]
    let TWOV_NONE = 0
    [<Literal>]
    let TWOV_AUTO = 1
    [<Literal>]
    let TWOV_TOPBOTTOM = 2
    [<Literal>]
    let TWOV_LEFTRIGHT = 3
    [<Literal>]
    let TWOV_ALL = 4
    [<Literal>]
    let TWPA_RGB = 0
    [<Literal>]
    let TWPA_GRAY = 1
    [<Literal>]
    let TWPA_CMY = 2
    [<Literal>]
    let TWPC_CHUNKY = 0
    [<Literal>]
    let TWPC_PLANAR = 1
    [<Literal>]
    let TWPCH_PATCH1 = 0
    [<Literal>]
    let TWPCH_PATCH2 = 1
    [<Literal>]
    let TWPCH_PATCH3 = 2
    [<Literal>]
    let TWPCH_PATCH4 = 3
    [<Literal>]
    let TWPCH_PATCH6 = 4
    [<Literal>]
    let TWPCH_PATCHT = 5
    [<Literal>]
    let TWPF_CHOCOLATE = 0
    [<Literal>]
    let TWPF_VANILLA = 1
    [<Literal>]
    let TWPM_SINGLESTRING = 0
    [<Literal>]
    let TWPM_MULTISTRING = 1
    [<Literal>]
    let TWPM_COMPOUNDSTRING = 2
    [<Literal>]
    let TWPM_IMAGEADDRESSSTRING = 3
    [<Literal>]
    let TWPR_IMPRINTERTOPBEFORE = 0
    [<Literal>]
    let TWPR_IMPRINTERTOPAFTER = 1
    [<Literal>]
    let TWPR_IMPRINTERBOTTOMBEFORE = 2
    [<Literal>]
    let TWPR_IMPRINTERBOTTOMAFTER = 3
    [<Literal>]
    let TWPR_ENDORSERTOPBEFORE = 4
    [<Literal>]
    let TWPR_ENDORSERTOPAFTER = 5
    [<Literal>]
    let TWPR_ENDORSERBOTTOMBEFORE = 6
    [<Literal>]
    let TWPR_ENDORSERBOTTOMAFTER = 7
    [<Literal>]
    let TWPF_NORMAL = 0
    [<Literal>]
    let TWPF_BOLD = 1
    [<Literal>]
    let TWPF_ITALIC = 2
    [<Literal>]
    let TWPF_LARGESIZE = 3
    [<Literal>]
    let TWPF_SMALLSIZE = 4
    [<Literal>]
    let TWCT_PAGE = 0
    [<Literal>]
    let TWCT_PATCH1 = 1
    [<Literal>]
    let TWCT_PATCH2 = 2
    [<Literal>]
    let TWCT_PATCH3 = 3
    [<Literal>]
    let TWCT_PATCH4 = 4
    [<Literal>]
    let TWCT_PATCHT = 5
    [<Literal>]
    let TWCT_PATCH6 = 6
    [<Literal>]
    let TWPS_EXTERNAL = 0
    [<Literal>]
    let TWPS_BATTERY = 1
    [<Literal>]
    let TWPT_BW = 0
    [<Literal>]
    let TWPT_GRAY = 1
    [<Literal>]
    let TWPT_RGB = 2
    [<Literal>]
    let TWPT_PALETTE = 3
    [<Literal>]
    let TWPT_CMY = 4
    [<Literal>]
    let TWPT_CMYK = 5
    [<Literal>]
    let TWPT_YUV = 6
    [<Literal>]
    let TWPT_YUVK = 7
    [<Literal>]
    let TWPT_CIEXYZ = 8
    [<Literal>]
    let TWPT_LAB = 9
    [<Literal>]
    let TWPT_SRGB = 10
    [<Literal>]
    let TWPT_SCRGB = 11
    [<Literal>]
    let TWPT_INFRARED = 16
    [<Literal>]
    let TWSG_NONE = 0
    [<Literal>]
    let TWSG_AUTO = 1
    [<Literal>]
    let TWSG_MANUAL = 2
    [<Literal>]
    let TWFM_POSITIVE = 0
    [<Literal>]
    let TWFM_NEGATIVE = 1
    [<Literal>]
    let TWDF_ULTRASONIC = 0
    [<Literal>]
    let TWDF_BYLENGTH = 1
    [<Literal>]
    let TWDF_INFRARED = 2
    [<Literal>]
    let TWUS_LOW = 0
    [<Literal>]
    let TWUS_MEDIUM = 1
    [<Literal>]
    let TWUS_HIGH = 2
    [<Literal>]
    let TWDP_STOP = 0
    [<Literal>]
    let TWDP_STOPANDWAIT = 1
    [<Literal>]
    let TWDP_SOUND = 2
    [<Literal>]
    let TWDP_DONOTIMPRINT = 3
    [<Literal>]
    let TWMR_NONE = 0
    [<Literal>]
    let TWMR_VERTICAL = 1
    [<Literal>]
    let TWMR_HORIZONTAL = 2
    [<Literal>]
    let TWJS_444YCBCR = 0
    [<Literal>]
    let TWJS_444RGB = 1
    [<Literal>]
    let TWJS_422 = 2
    [<Literal>]
    let TWJS_421 = 3
    [<Literal>]
    let TWJS_411 = 4
    [<Literal>]
    let TWJS_420 = 5
    [<Literal>]
    let TWJS_410 = 6
    [<Literal>]
    let TWJS_311 = 7
    [<Literal>]
    let TWPH_NORMAL = 0
    [<Literal>]
    let TWPH_FRAGILE = 1
    [<Literal>]
    let TWPH_THICK = 2
    [<Literal>]
    let TWPH_TRIFOLD = 3
    [<Literal>]
    let TWPH_PHOTOGRAPH = 4
    [<Literal>]
    let TWCI_INFO = 0
    [<Literal>]
    let TWCI_WARNING = 1
    [<Literal>]
    let TWCI_ERROR = 2
    [<Literal>]
    let TWCI_WARMUP = 3
    [<Literal>]
    let TWSS_NONE = 0
    [<Literal>]
    let TWSS_A4 = 1
    [<Literal>]
    let TWSS_JISB5 = 2
    [<Literal>]
    let TWSS_USLETTER = 3
    [<Literal>]
    let TWSS_USLEGAL = 4
    [<Literal>]
    let TWSS_A5 = 5
    [<Literal>]
    let TWSS_ISOB4 = 6
    [<Literal>]
    let TWSS_ISOB6 = 7
    [<Literal>]
    let TWSS_USLEDGER = 9
    [<Literal>]
    let TWSS_USEXECUTIVE = 10
    [<Literal>]
    let TWSS_A3 = 11
    [<Literal>]
    let TWSS_ISOB3 = 12
    [<Literal>]
    let TWSS_A6 = 13
    [<Literal>]
    let TWSS_C4 = 14
    [<Literal>]
    let TWSS_C5 = 15
    [<Literal>]
    let TWSS_C6 = 16
    [<Literal>]
    let TWSS_4A0 = 17
    [<Literal>]
    let TWSS_2A0 = 18
    [<Literal>]
    let TWSS_A0 = 19
    [<Literal>]
    let TWSS_A1 = 20
    [<Literal>]
    let TWSS_A2 = 21
    [<Literal>]
    let TWSS_A7 = 22
    [<Literal>]
    let TWSS_A8 = 23
    [<Literal>]
    let TWSS_A9 = 24
    [<Literal>]
    let TWSS_A10 = 25
    [<Literal>]
    let TWSS_ISOB0 = 26
    [<Literal>]
    let TWSS_ISOB1 = 27
    [<Literal>]
    let TWSS_ISOB2 = 28
    [<Literal>]
    let TWSS_ISOB5 = 29
    [<Literal>]
    let TWSS_ISOB7 = 30
    [<Literal>]
    let TWSS_ISOB8 = 31
    [<Literal>]
    let TWSS_ISOB9 = 32
    [<Literal>]
    let TWSS_ISOB10 = 33
    [<Literal>]
    let TWSS_JISB0 = 34
    [<Literal>]
    let TWSS_JISB1 = 35
    [<Literal>]
    let TWSS_JISB2 = 36
    [<Literal>]
    let TWSS_JISB3 = 37
    [<Literal>]
    let TWSS_JISB4 = 38
    [<Literal>]
    let TWSS_JISB6 = 39
    [<Literal>]
    let TWSS_JISB7 = 40
    [<Literal>]
    let TWSS_JISB8 = 41
    [<Literal>]
    let TWSS_JISB9 = 42
    [<Literal>]
    let TWSS_JISB10 = 43
    [<Literal>]
    let TWSS_C0 = 44
    [<Literal>]
    let TWSS_C1 = 45
    [<Literal>]
    let TWSS_C2 = 46
    [<Literal>]
    let TWSS_C3 = 47
    [<Literal>]
    let TWSS_C7 = 48
    [<Literal>]
    let TWSS_C8 = 49
    [<Literal>]
    let TWSS_C9 = 50
    [<Literal>]
    let TWSS_C10 = 51
    [<Literal>]
    let TWSS_USSTATEMENT = 52
    [<Literal>]
    let TWSS_BUSINESSCARD = 53
    [<Literal>]
    let TWSS_MAXSIZE = 54
    [<Literal>]
    let TWSX_NATIVE = 0
    [<Literal>]
    let TWSX_FILE = 1
    [<Literal>]
    let TWSX_MEMORY = 2
    [<Literal>]
    let TWSX_MEMFILE = 4
    [<Literal>]
    let TWUN_INCHES = 0
    [<Literal>]
    let TWUN_CENTIMETERS = 1
    [<Literal>]
    let TWUN_PICAS = 2
    [<Literal>]
    let TWUN_POINTS = 3
    [<Literal>]
    let TWUN_TWIPS = 4
    [<Literal>]
    let TWUN_PIXELS = 5
    [<Literal>]
    let TWUN_MILLIMETERS = 6
    [<Literal>]
    let TWCY_AFGHANISTAN = 1001
    [<Literal>]
    let TWCY_ALGERIA = 213
    [<Literal>]
    let TWCY_AMERICANSAMOA = 684
    [<Literal>]
    let TWCY_ANDORRA = 33
    [<Literal>]
    let TWCY_ANGOLA = 1002
    [<Literal>]
    let TWCY_ANGUILLA = 8090
    [<Literal>]
    let TWCY_ANTIGUA = 8091
    [<Literal>]
    let TWCY_ARGENTINA = 54
    [<Literal>]
    let TWCY_ARUBA = 297
    [<Literal>]
    let TWCY_ASCENSIONI = 247
    [<Literal>]
    let TWCY_AUSTRALIA = 61
    [<Literal>]
    let TWCY_AUSTRIA = 43
    [<Literal>]
    let TWCY_BAHAMAS = 8092
    [<Literal>]
    let TWCY_BAHRAIN = 973
    [<Literal>]
    let TWCY_BANGLADESH = 880
    [<Literal>]
    let TWCY_BARBADOS = 8093
    [<Literal>]
    let TWCY_BELGIUM = 32
    [<Literal>]
    let TWCY_BELIZE = 501
    [<Literal>]
    let TWCY_BENIN = 229
    [<Literal>]
    let TWCY_BERMUDA = 8094
    [<Literal>]
    let TWCY_BHUTAN = 1003
    [<Literal>]
    let TWCY_BOLIVIA = 591
    [<Literal>]
    let TWCY_BOTSWANA = 267
    [<Literal>]
    let TWCY_BRITAIN = 6
    [<Literal>]
    let TWCY_BRITVIRGINIS = 8095
    [<Literal>]
    let TWCY_BRAZIL = 55
    [<Literal>]
    let TWCY_BRUNEI = 673
    [<Literal>]
    let TWCY_BULGARIA = 359
    [<Literal>]
    let TWCY_BURKINAFASO = 1004
    [<Literal>]
    let TWCY_BURMA = 1005
    [<Literal>]
    let TWCY_BURUNDI = 1006
    [<Literal>]
    let TWCY_CAMAROON = 237
    [<Literal>]
    let TWCY_CANADA = 2
    [<Literal>]
    let TWCY_CAPEVERDEIS = 238
    [<Literal>]
    let TWCY_CAYMANIS = 8096
    [<Literal>]
    let TWCY_CENTRALAFREP = 1007
    [<Literal>]
    let TWCY_CHAD = 1008
    [<Literal>]
    let TWCY_CHILE = 56
    [<Literal>]
    let TWCY_CHINA = 86
    [<Literal>]
    let TWCY_CHRISTMASIS = 1009
    [<Literal>]
    let TWCY_COCOSIS = 1009
    [<Literal>]
    let TWCY_COLOMBIA = 57
    [<Literal>]
    let TWCY_COMOROS = 1010
    [<Literal>]
    let TWCY_CONGO = 1011
    [<Literal>]
    let TWCY_COOKIS = 1012
    [<Literal>]
    let TWCY_COSTARICA = 506
    [<Literal>]
    let TWCY_CUBA = 5
    [<Literal>]
    let TWCY_CYPRUS = 357
    [<Literal>]
    let TWCY_CZECHOSLOVAKIA = 42
    [<Literal>]
    let TWCY_DENMARK = 45
    [<Literal>]
    let TWCY_DJIBOUTI = 1013
    [<Literal>]
    let TWCY_DOMINICA = 8097
    [<Literal>]
    let TWCY_DOMINCANREP = 8098
    [<Literal>]
    let TWCY_EASTERIS = 1014
    [<Literal>]
    let TWCY_ECUADOR = 593
    [<Literal>]
    let TWCY_EGYPT = 20
    [<Literal>]
    let TWCY_ELSALVADOR = 503
    [<Literal>]
    let TWCY_EQGUINEA = 1015
    [<Literal>]
    let TWCY_ETHIOPIA = 251
    [<Literal>]
    let TWCY_FALKLANDIS = 1016
    [<Literal>]
    let TWCY_FAEROEIS = 298
    [<Literal>]
    let TWCY_FIJIISLANDS = 679
    [<Literal>]
    let TWCY_FINLAND = 358
    [<Literal>]
    let TWCY_FRANCE = 33
    [<Literal>]
    let TWCY_FRANTILLES = 596
    [<Literal>]
    let TWCY_FRGUIANA = 594
    [<Literal>]
    let TWCY_FRPOLYNEISA = 689
    [<Literal>]
    let TWCY_FUTANAIS = 1043
    [<Literal>]
    let TWCY_GABON = 241
    [<Literal>]
    let TWCY_GAMBIA = 220
    [<Literal>]
    let TWCY_GERMANY = 49
    [<Literal>]
    let TWCY_GHANA = 233
    [<Literal>]
    let TWCY_GIBRALTER = 350
    [<Literal>]
    let TWCY_GREECE = 30
    [<Literal>]
    let TWCY_GREENLAND = 299
    [<Literal>]
    let TWCY_GRENADA = 8099
    [<Literal>]
    let TWCY_GRENEDINES = 8015
    [<Literal>]
    let TWCY_GUADELOUPE = 590
    [<Literal>]
    let TWCY_GUAM = 671
    [<Literal>]
    let TWCY_GUANTANAMOBAY = 5399
    [<Literal>]
    let TWCY_GUATEMALA = 502
    [<Literal>]
    let TWCY_GUINEA = 224
    [<Literal>]
    let TWCY_GUINEABISSAU = 1017
    [<Literal>]
    let TWCY_GUYANA = 592
    [<Literal>]
    let TWCY_HAITI = 509
    [<Literal>]
    let TWCY_HONDURAS = 504
    [<Literal>]
    let TWCY_HONGKONG = 852
    [<Literal>]
    let TWCY_HUNGARY = 36
    [<Literal>]
    let TWCY_ICELAND = 354
    [<Literal>]
    let TWCY_INDIA = 91
    [<Literal>]
    let TWCY_INDONESIA = 62
    [<Literal>]
    let TWCY_IRAN = 98
    [<Literal>]
    let TWCY_IRAQ = 964
    [<Literal>]
    let TWCY_IRELAND = 353
    [<Literal>]
    let TWCY_ISRAEL = 972
    [<Literal>]
    let TWCY_ITALY = 39
    [<Literal>]
    let TWCY_IVORYCOAST = 225
    [<Literal>]
    let TWCY_JAMAICA = 8010
    [<Literal>]
    let TWCY_JAPAN = 81
    [<Literal>]
    let TWCY_JORDAN = 962
    [<Literal>]
    let TWCY_KENYA = 254
    [<Literal>]
    let TWCY_KIRIBATI = 1018
    [<Literal>]
    let TWCY_KOREA = 82
    [<Literal>]
    let TWCY_KUWAIT = 965
    [<Literal>]
    let TWCY_LAOS = 1019
    [<Literal>]
    let TWCY_LEBANON = 1020
    [<Literal>]
    let TWCY_LIBERIA = 231
    [<Literal>]
    let TWCY_LIBYA = 218
    [<Literal>]
    let TWCY_LIECHTENSTEIN = 41
    [<Literal>]
    let TWCY_LUXENBOURG = 352
    [<Literal>]
    let TWCY_MACAO = 853
    [<Literal>]
    let TWCY_MADAGASCAR = 1021
    [<Literal>]
    let TWCY_MALAWI = 265
    [<Literal>]
    let TWCY_MALAYSIA = 60
    [<Literal>]
    let TWCY_MALDIVES = 960
    [<Literal>]
    let TWCY_MALI = 1022
    [<Literal>]
    let TWCY_MALTA = 356
    [<Literal>]
    let TWCY_MARSHALLIS = 692
    [<Literal>]
    let TWCY_MAURITANIA = 1023
    [<Literal>]
    let TWCY_MAURITIUS = 230
    [<Literal>]
    let TWCY_MEXICO = 3
    [<Literal>]
    let TWCY_MICRONESIA = 691
    [<Literal>]
    let TWCY_MIQUELON = 508
    [<Literal>]
    let TWCY_MONACO = 33
    [<Literal>]
    let TWCY_MONGOLIA = 1024
    [<Literal>]
    let TWCY_MONTSERRAT = 8011
    [<Literal>]
    let TWCY_MOROCCO = 212
    [<Literal>]
    let TWCY_MOZAMBIQUE = 1025
    [<Literal>]
    let TWCY_NAMIBIA = 264
    [<Literal>]
    let TWCY_NAURU = 1026
    [<Literal>]
    let TWCY_NEPAL = 977
    [<Literal>]
    let TWCY_NETHERLANDS = 31
    [<Literal>]
    let TWCY_NETHANTILLES = 599
    [<Literal>]
    let TWCY_NEVIS = 8012
    [<Literal>]
    let TWCY_NEWCALEDONIA = 687
    [<Literal>]
    let TWCY_NEWZEALAND = 64
    [<Literal>]
    let TWCY_NICARAGUA = 505
    [<Literal>]
    let TWCY_NIGER = 227
    [<Literal>]
    let TWCY_NIGERIA = 234
    [<Literal>]
    let TWCY_NIUE = 1027
    [<Literal>]
    let TWCY_NORFOLKI = 1028
    [<Literal>]
    let TWCY_NORWAY = 47
    [<Literal>]
    let TWCY_OMAN = 968
    [<Literal>]
    let TWCY_PAKISTAN = 92
    [<Literal>]
    let TWCY_PALAU = 1029
    [<Literal>]
    let TWCY_PANAMA = 507
    [<Literal>]
    let TWCY_PARAGUAY = 595
    [<Literal>]
    let TWCY_PERU = 51
    [<Literal>]
    let TWCY_PHILLIPPINES = 63
    [<Literal>]
    let TWCY_PITCAIRNIS = 1030
    [<Literal>]
    let TWCY_PNEWGUINEA = 675
    [<Literal>]
    let TWCY_POLAND = 48
    [<Literal>]
    let TWCY_PORTUGAL = 351
    [<Literal>]
    let TWCY_QATAR = 974
    [<Literal>]
    let TWCY_REUNIONI = 1031
    [<Literal>]
    let TWCY_ROMANIA = 40
    [<Literal>]
    let TWCY_RWANDA = 250
    [<Literal>]
    let TWCY_SAIPAN = 670
    [<Literal>]
    let TWCY_SANMARINO = 39
    [<Literal>]
    let TWCY_SAOTOME = 1033
    [<Literal>]
    let TWCY_SAUDIARABIA = 966
    [<Literal>]
    let TWCY_SENEGAL = 221
    [<Literal>]
    let TWCY_SEYCHELLESIS = 1034
    [<Literal>]
    let TWCY_SIERRALEONE = 1035
    [<Literal>]
    let TWCY_SINGAPORE = 65
    [<Literal>]
    let TWCY_SOLOMONIS = 1036
    [<Literal>]
    let TWCY_SOMALI = 1037
    [<Literal>]
    let TWCY_SOUTHAFRICA = 27
    [<Literal>]
    let TWCY_SPAIN = 34
    [<Literal>]
    let TWCY_SRILANKA = 94
    [<Literal>]
    let TWCY_STHELENA = 1032
    [<Literal>]
    let TWCY_STKITTS = 8013
    [<Literal>]
    let TWCY_STLUCIA = 8014
    [<Literal>]
    let TWCY_STPIERRE = 508
    [<Literal>]
    let TWCY_STVINCENT = 8015
    [<Literal>]
    let TWCY_SUDAN = 1038
    [<Literal>]
    let TWCY_SURINAME = 597
    [<Literal>]
    let TWCY_SWAZILAND = 268
    [<Literal>]
    let TWCY_SWEDEN = 46
    [<Literal>]
    let TWCY_SWITZERLAND = 41
    [<Literal>]
    let TWCY_SYRIA = 1039
    [<Literal>]
    let TWCY_TAIWAN = 886
    [<Literal>]
    let TWCY_TANZANIA = 255
    [<Literal>]
    let TWCY_THAILAND = 66
    [<Literal>]
    let TWCY_TOBAGO = 8016
    [<Literal>]
    let TWCY_TOGO = 228
    [<Literal>]
    let TWCY_TONGAIS = 676
    [<Literal>]
    let TWCY_TRINIDAD = 8016
    [<Literal>]
    let TWCY_TUNISIA = 216
    [<Literal>]
    let TWCY_TURKEY = 90
    [<Literal>]
    let TWCY_TURKSCAICOS = 8017
    [<Literal>]
    let TWCY_TUVALU = 1040
    [<Literal>]
    let TWCY_UGANDA = 256
    [<Literal>]
    let TWCY_USSR = 7
    [<Literal>]
    let TWCY_UAEMIRATES = 971
    [<Literal>]
    let TWCY_UNITEDKINGDOM = 44
    [<Literal>]
    let TWCY_USA = 1
    [<Literal>]
    let TWCY_URUGUAY = 598
    [<Literal>]
    let TWCY_VANUATU = 1041
    [<Literal>]
    let TWCY_VATICANCITY = 39
    [<Literal>]
    let TWCY_VENEZUELA = 58
    [<Literal>]
    let TWCY_WAKE = 1042
    [<Literal>]
    let TWCY_WALLISIS = 1043
    [<Literal>]
    let TWCY_WESTERNSAHARA = 1044
    [<Literal>]
    let TWCY_WESTERNSAMOA = 1045
    [<Literal>]
    let TWCY_YEMEN = 1046
    [<Literal>]
    let TWCY_YUGOSLAVIA = 38
    [<Literal>]
    let TWCY_ZAIRE = 243
    [<Literal>]
    let TWCY_ZAMBIA = 260
    [<Literal>]
    let TWCY_ZIMBABWE = 263
    [<Literal>]
    let TWCY_ALBANIA = 355
    [<Literal>]
    let TWCY_ARMENIA = 374
    [<Literal>]
    let TWCY_AZERBAIJAN = 994
    [<Literal>]
    let TWCY_BELARUS = 375
    [<Literal>]
    let TWCY_BOSNIAHERZGO = 387
    [<Literal>]
    let TWCY_CAMBODIA = 855
    [<Literal>]
    let TWCY_CROATIA = 385
    [<Literal>]
    let TWCY_CZECHREPUBLIC = 420
    [<Literal>]
    let TWCY_DIEGOGARCIA = 246
    [<Literal>]
    let TWCY_ERITREA = 291
    [<Literal>]
    let TWCY_ESTONIA = 372
    [<Literal>]
    let TWCY_GEORGIA = 995
    [<Literal>]
    let TWCY_LATVIA = 371
    [<Literal>]
    let TWCY_LESOTHO = 266
    [<Literal>]
    let TWCY_LITHUANIA = 370
    [<Literal>]
    let TWCY_MACEDONIA = 389
    [<Literal>]
    let TWCY_MAYOTTEIS = 269
    [<Literal>]
    let TWCY_MOLDOVA = 373
    [<Literal>]
    let TWCY_MYANMAR = 95
    [<Literal>]
    let TWCY_NORTHKOREA = 850
    [<Literal>]
    let TWCY_PUERTORICO = 787
    [<Literal>]
    let TWCY_RUSSIA = 7
    [<Literal>]
    let TWCY_SERBIA = 381
    [<Literal>]
    let TWCY_SLOVAKIA = 421
    [<Literal>]
    let TWCY_SLOVENIA = 386
    [<Literal>]
    let TWCY_SOUTHKOREA = 82
    [<Literal>]
    let TWCY_UKRAINE = 380
    [<Literal>]
    let TWCY_USVIRGINIS = 340
    [<Literal>]
    let TWCY_VIETNAM = 84
    [<Literal>]
    let TWLG_USERLOCALE = -1
    [<Literal>]
    let TWLG_DAN = 0
    [<Literal>]
    let TWLG_DUT = 1
    [<Literal>]
    let TWLG_ENG = 2
    [<Literal>]
    let TWLG_FCF = 3
    [<Literal>]
    let TWLG_FIN = 4
    [<Literal>]
    let TWLG_FRN = 5
    [<Literal>]
    let TWLG_GER = 6
    [<Literal>]
    let TWLG_ICE = 7
    [<Literal>]
    let TWLG_ITN = 8
    [<Literal>]
    let TWLG_NOR = 9
    [<Literal>]
    let TWLG_POR = 10
    [<Literal>]
    let TWLG_SPA = 11
    [<Literal>]
    let TWLG_SWE = 12
    [<Literal>]
    let TWLG_USA = 13
    [<Literal>]
    let TWLG_AFRIKAANS = 14
    [<Literal>]
    let TWLG_ALBANIA = 15
    [<Literal>]
    let TWLG_ARABIC = 16
    [<Literal>]
    let TWLG_ARABIC_ALGERIA = 17
    [<Literal>]
    let TWLG_ARABIC_BAHRAIN = 18
    [<Literal>]
    let TWLG_ARABIC_EGYPT = 19
    [<Literal>]
    let TWLG_ARABIC_IRAQ = 20
    [<Literal>]
    let TWLG_ARABIC_JORDAN = 21
    [<Literal>]
    let TWLG_ARABIC_KUWAIT = 22
    [<Literal>]
    let TWLG_ARABIC_LEBANON = 23
    [<Literal>]
    let TWLG_ARABIC_LIBYA = 24
    [<Literal>]
    let TWLG_ARABIC_MOROCCO = 25
    [<Literal>]
    let TWLG_ARABIC_OMAN = 26
    [<Literal>]
    let TWLG_ARABIC_QATAR = 27
    [<Literal>]
    let TWLG_ARABIC_SAUDIARABIA = 28
    [<Literal>]
    let TWLG_ARABIC_SYRIA = 29
    [<Literal>]
    let TWLG_ARABIC_TUNISIA = 30
    [<Literal>]
    let TWLG_ARABIC_UAE = 31
    [<Literal>]
    let TWLG_ARABIC_YEMEN = 32
    [<Literal>]
    let TWLG_BASQUE = 33
    [<Literal>]
    let TWLG_BYELORUSSIAN = 34
    [<Literal>]
    let TWLG_BULGARIAN = 35
    [<Literal>]
    let TWLG_CATALAN = 36
    [<Literal>]
    let TWLG_CHINESE = 37
    [<Literal>]
    let TWLG_CHINESE_HONGKONG = 38
    [<Literal>]
    let TWLG_CHINESE_PRC = 39
    [<Literal>]
    let TWLG_CHINESE_SINGAPORE = 40
    [<Literal>]
    let TWLG_CHINESE_SIMPLIFIED = 41
    [<Literal>]
    let TWLG_CHINESE_TAIWAN = 42
    [<Literal>]
    let TWLG_CHINESE_TRADITIONAL = 43
    [<Literal>]
    let TWLG_CROATIA = 44
    [<Literal>]
    let TWLG_CZECH = 45
    [<Literal>]
    let TWLG_DANISH = TWLG_DAN
    [<Literal>]
    let TWLG_DUTCH = TWLG_DUT
    [<Literal>]
    let TWLG_DUTCH_BELGIAN = 46
    [<Literal>]
    let TWLG_ENGLISH = TWLG_ENG
    [<Literal>]
    let TWLG_ENGLISH_AUSTRALIAN = 47
    [<Literal>]
    let TWLG_ENGLISH_CANADIAN = 48
    [<Literal>]
    let TWLG_ENGLISH_IRELAND = 49
    [<Literal>]
    let TWLG_ENGLISH_NEWZEALAND = 50
    [<Literal>]
    let TWLG_ENGLISH_SOUTHAFRICA = 51
    [<Literal>]
    let TWLG_ENGLISH_UK = 52
    [<Literal>]
    let TWLG_ENGLISH_USA = TWLG_USA
    [<Literal>]
    let TWLG_ESTONIAN = 53
    [<Literal>]
    let TWLG_FAEROESE = 54
    [<Literal>]
    let TWLG_FARSI = 55
    [<Literal>]
    let TWLG_FINNISH = TWLG_FIN
    [<Literal>]
    let TWLG_FRENCH = TWLG_FRN
    [<Literal>]
    let TWLG_FRENCH_BELGIAN = 56
    [<Literal>]
    let TWLG_FRENCH_CANADIAN = TWLG_FCF
    [<Literal>]
    let TWLG_FRENCH_LUXEMBOURG = 57
    [<Literal>]
    let TWLG_FRENCH_SWISS = 58
    [<Literal>]
    let TWLG_GERMAN = TWLG_GER
    [<Literal>]
    let TWLG_GERMAN_AUSTRIAN = 59
    [<Literal>]
    let TWLG_GERMAN_LUXEMBOURG = 60
    [<Literal>]
    let TWLG_GERMAN_LIECHTENSTEIN = 61
    [<Literal>]
    let TWLG_GERMAN_SWISS = 62
    [<Literal>]
    let TWLG_GREEK = 63
    [<Literal>]
    let TWLG_HEBREW = 64
    [<Literal>]
    let TWLG_HUNGARIAN = 65
    [<Literal>]
    let TWLG_ICELANDIC = TWLG_ICE
    [<Literal>]
    let TWLG_INDONESIAN = 66
    [<Literal>]
    let TWLG_ITALIAN = TWLG_ITN
    [<Literal>]
    let TWLG_ITALIAN_SWISS = 67
    [<Literal>]
    let TWLG_JAPANESE = 68
    [<Literal>]
    let TWLG_KOREAN = 69
    [<Literal>]
    let TWLG_KOREAN_JOHAB = 70
    [<Literal>]
    let TWLG_LATVIAN = 71
    [<Literal>]
    let TWLG_LITHUANIAN = 72
    [<Literal>]
    let TWLG_NORWEGIAN = TWLG_NOR
    [<Literal>]
    let TWLG_NORWEGIAN_BOKMAL = 73
    [<Literal>]
    let TWLG_NORWEGIAN_NYNORSK = 74
    [<Literal>]
    let TWLG_POLISH = 75
    [<Literal>]
    let TWLG_PORTUGUESE = TWLG_POR
    [<Literal>]
    let TWLG_PORTUGUESE_BRAZIL = 76
    [<Literal>]
    let TWLG_ROMANIAN = 77
    [<Literal>]
    let TWLG_RUSSIAN = 78
    [<Literal>]
    let TWLG_SERBIAN_LATIN = 79
    [<Literal>]
    let TWLG_SLOVAK = 80
    [<Literal>]
    let TWLG_SLOVENIAN = 81
    [<Literal>]
    let TWLG_SPANISH = TWLG_SPA
    [<Literal>]
    let TWLG_SPANISH_MEXICAN = 82
    [<Literal>]
    let TWLG_SPANISH_MODERN = 83
    [<Literal>]
    let TWLG_SWEDISH = TWLG_SWE
    [<Literal>]
    let TWLG_THAI = 84
    [<Literal>]
    let TWLG_TURKISH = 85
    [<Literal>]
    let TWLG_UKRANIAN = 86
    [<Literal>]
    let TWLG_ASSAMESE = 87
    [<Literal>]
    let TWLG_BENGALI = 88
    [<Literal>]
    let TWLG_BIHARI = 89
    [<Literal>]
    let TWLG_BODO = 90
    [<Literal>]
    let TWLG_DOGRI = 91
    [<Literal>]
    let TWLG_GUJARATI = 92
    [<Literal>]
    let TWLG_HARYANVI = 93
    [<Literal>]
    let TWLG_HINDI = 94
    [<Literal>]
    let TWLG_KANNADA = 95
    [<Literal>]
    let TWLG_KASHMIRI = 96
    [<Literal>]
    let TWLG_MALAYALAM = 97
    [<Literal>]
    let TWLG_MARATHI = 98
    [<Literal>]
    let TWLG_MARWARI = 99
    [<Literal>]
    let TWLG_MEGHALAYAN = 100
    [<Literal>]
    let TWLG_MIZO = 101
    [<Literal>]
    let TWLG_NAGA = 102
    [<Literal>]
    let TWLG_ORISSI = 103
    [<Literal>]
    let TWLG_PUNJABI = 104
    [<Literal>]
    let TWLG_PUSHTU = 105
    [<Literal>]
    let TWLG_SERBIAN_CYRILLIC = 106
    [<Literal>]
    let TWLG_SIKKIMI = 107
    [<Literal>]
    let TWLG_SWEDISH_FINLAND = 108
    [<Literal>]
    let TWLG_TAMIL = 109
    [<Literal>]
    let TWLG_TELUGU = 110
    [<Literal>]
    let TWLG_TRIPURI = 111
    [<Literal>]
    let TWLG_URDU = 112
    [<Literal>]
    let TWLG_VIETNAMESE = 113
    [<Literal>]
    let DG_CONTROL = 0x0001
    [<Literal>]
    let DG_IMAGE = 0x0002
    [<Literal>]
    let DG_AUDIO = 0x0004
    [<Literal>]
    let DF_DSM2 = 0x10000000
    [<Literal>]
    let DF_APP2 = 0x20000000
    [<Literal>]
    let DF_DS2 = 0x40000000
    [<Literal>]
    let DG_MASK = 0xFFFF
    [<Literal>]
    let DAT_NULL = 0x0000
    [<Literal>]
    let DAT_CUSTOMBASE = 0x8000
    [<Literal>]
    let DAT_CAPABILITY = 0x0001
    [<Literal>]
    let DAT_EVENT = 0x0002
    [<Literal>]
    let DAT_IDENTITY = 0x0003
    [<Literal>]
    let DAT_PARENT = 0x0004
    [<Literal>]
    let DAT_PENDINGXFERS = 0x0005
    [<Literal>]
    let DAT_SETUPMEMXFER = 0x0006
    [<Literal>]
    let DAT_SETUPFILEXFER = 0x0007
    [<Literal>]
    let DAT_STATUS = 0x0008
    [<Literal>]
    let DAT_USERINTERFACE = 0x0009
    [<Literal>]
    let DAT_XFERGROUP = 0x000a
    [<Literal>]
    let DAT_CUSTOMDSDATA = 0x000c
    [<Literal>]
    let DAT_DEVICEEVENT = 0x000d
    [<Literal>]
    let DAT_FILESYSTEM = 0x000e
    [<Literal>]
    let DAT_PASSTHRU = 0x000f
    [<Literal>]
    let DAT_CALLBACK = 0x0010
    [<Literal>]
    let DAT_STATUSUTF8 = 0x0011
    [<Literal>]
    let DAT_CALLBACK2 = 0x0012
    [<Literal>]
    let DAT_METRICS = 0x0013
    [<Literal>]
    let DAT_TWAINDIRECT = 0x0014
    [<Literal>]
    let DAT_IMAGEINFO = 0x0101
    [<Literal>]
    let DAT_IMAGELAYOUT = 0x0102
    [<Literal>]
    let DAT_IMAGEMEMXFER = 0x0103
    [<Literal>]
    let DAT_IMAGENATIVEXFER = 0x0104
    [<Literal>]
    let DAT_IMAGEFILEXFER = 0x0105
    [<Literal>]
    let DAT_CIECOLOR = 0x0106
    [<Literal>]
    let DAT_GRAYRESPONSE = 0x0107
    [<Literal>]
    let DAT_RGBRESPONSE = 0x0108
    [<Literal>]
    let DAT_JPEGCOMPRESSION = 0x0109
    [<Literal>]
    let DAT_PALETTE8 = 0x010a
    [<Literal>]
    let DAT_EXTIMAGEINFO = 0x010b
    [<Literal>]
    let DAT_FILTER = 0x010c
    [<Literal>]
    let DAT_AUDIOFILEXFER = 0x0201
    [<Literal>]
    let DAT_AUDIOINFO = 0x0202
    [<Literal>]
    let DAT_AUDIONATIVEXFER = 0x0203
    [<Literal>]
    let DAT_ICCPROFILE = 0x0401
    [<Literal>]
    let DAT_IMAGEMEMFILEXFER = 0x0402
    [<Literal>]
    let DAT_ENTRYPOINT = 0x0403
    [<Literal>]
    let MSG_NULL = 0x0000
    [<Literal>]
    let MSG_CUSTOMBASE = 0x8000
    [<Literal>]
    let MSG_GET = 0x0001
    [<Literal>]
    let MSG_GETCURRENT = 0x0002
    [<Literal>]
    let MSG_GETDEFAULT = 0x0003
    [<Literal>]
    let MSG_GETFIRST = 0x0004
    [<Literal>]
    let MSG_GETNEXT = 0x0005
    [<Literal>]
    let MSG_SET = 0x0006
    [<Literal>]
    let MSG_RESET = 0x0007
    [<Literal>]
    let MSG_QUERYSUPPORT = 0x0008
    [<Literal>]
    let MSG_GETHELP = 0x0009
    [<Literal>]
    let MSG_GETLABEL = 0x000a
    [<Literal>]
    let MSG_GETLABELENUM = 0x000b
    [<Literal>]
    let MSG_SETCONSTRAINT = 0x000c
    [<Literal>]
    let MSG_XFERREADY = 0x0101
    [<Literal>]
    let MSG_CLOSEDSREQ = 0x0102
    [<Literal>]
    let MSG_CLOSEDSOK = 0x0103
    [<Literal>]
    let MSG_DEVICEEVENT = 0x0104
    [<Literal>]
    let MSG_OPENDSM = 0x0301
    [<Literal>]
    let MSG_CLOSEDSM = 0x0302
    [<Literal>]
    let MSG_OPENDS = 0x0401
    [<Literal>]
    let MSG_CLOSEDS = 0x0402
    [<Literal>]
    let MSG_USERSELECT = 0x0403
    [<Literal>]
    let MSG_DISABLEDS = 0x0501
    [<Literal>]
    let MSG_ENABLEDS = 0x0502
    [<Literal>]
    let MSG_ENABLEDSUIONLY = 0x0503
    [<Literal>]
    let MSG_PROCESSEVENT = 0x0601
    [<Literal>]
    let MSG_ENDXFER = 0x0701
    [<Literal>]
    let MSG_STOPFEEDER = 0x0702
    [<Literal>]
    let MSG_CHANGEDIRECTORY = 0x0801
    [<Literal>]
    let MSG_CREATEDIRECTORY = 0x0802
    [<Literal>]
    let MSG_DELETE = 0x0803
    [<Literal>]
    let MSG_FORMATMEDIA = 0x0804
    [<Literal>]
    let MSG_GETCLOSE = 0x0805
    [<Literal>]
    let MSG_GETFIRSTFILE = 0x0806
    [<Literal>]
    let MSG_GETINFO = 0x0807
    [<Literal>]
    let MSG_GETNEXTFILE = 0x0808
    [<Literal>]
    let MSG_RENAME = 0x0809
    [<Literal>]
    let MSG_COPY = 0x080A
    [<Literal>]
    let MSG_AUTOMATICCAPTUREDIRECTORY = 0x080B
    [<Literal>]
    let MSG_PASSTHRU = 0x0901
    [<Literal>]
    let MSG_REGISTER_CALLBACK = 0x0902
    [<Literal>]
    let MSG_RESETALL = 0x0A01
    [<Literal>]
    let MSG_SETTASK = 0x0B01
    [<Literal>]
    let CAP_CUSTOMBASE = 0x8000
    [<Literal>]
    let CAP_XFERCOUNT = 0x0001
    [<Literal>]
    let ICAP_COMPRESSION = 0x0100
    [<Literal>]
    let ICAP_PIXELTYPE = 0x0101
    [<Literal>]
    let ICAP_UNITS = 0x0102
    [<Literal>]
    let ICAP_XFERMECH = 0x0103
    [<Literal>]
    let CAP_AUTHOR = 0x1000
    [<Literal>]
    let CAP_CAPTION = 0x1001
    [<Literal>]
    let CAP_FEEDERENABLED = 0x1002
    [<Literal>]
    let CAP_FEEDERLOADED = 0x1003
    [<Literal>]
    let CAP_TIMEDATE = 0x1004
    [<Literal>]
    let CAP_SUPPORTEDCAPS = 0x1005
    [<Literal>]
    let CAP_EXTENDEDCAPS = 0x1006
    [<Literal>]
    let CAP_AUTOFEED = 0x1007
    [<Literal>]
    let CAP_CLEARPAGE = 0x1008
    [<Literal>]
    let CAP_FEEDPAGE = 0x1009
    [<Literal>]
    let CAP_REWINDPAGE = 0x100a
    [<Literal>]
    let CAP_INDICATORS = 0x100b
    [<Literal>]
    let CAP_PAPERDETECTABLE = 0x100d
    [<Literal>]
    let CAP_UICONTROLLABLE = 0x100e
    [<Literal>]
    let CAP_DEVICEONLINE = 0x100f
    [<Literal>]
    let CAP_AUTOSCAN = 0x1010
    [<Literal>]
    let CAP_THUMBNAILSENABLED = 0x1011
    [<Literal>]
    let CAP_DUPLEX = 0x1012
    [<Literal>]
    let CAP_DUPLEXENABLED = 0x1013
    [<Literal>]
    let CAP_ENABLEDSUIONLY = 0x1014
    [<Literal>]
    let CAP_CUSTOMDSDATA = 0x1015
    [<Literal>]
    let CAP_ENDORSER = 0x1016
    [<Literal>]
    let CAP_JOBCONTROL = 0x1017
    [<Literal>]
    let CAP_ALARMS = 0x1018
    [<Literal>]
    let CAP_ALARMVOLUME = 0x1019
    [<Literal>]
    let CAP_AUTOMATICCAPTURE = 0x101a
    [<Literal>]
    let CAP_TIMEBEFOREFIRSTCAPTURE = 0x101b
    [<Literal>]
    let CAP_TIMEBETWEENCAPTURES = 0x101c
    [<Literal>]
    let CAP_MAXBATCHBUFFERS = 0x101e
    [<Literal>]
    let CAP_DEVICETIMEDATE = 0x101f
    [<Literal>]
    let CAP_POWERSUPPLY = 0x1020
    [<Literal>]
    let CAP_CAMERAPREVIEWUI = 0x1021
    [<Literal>]
    let CAP_DEVICEEVENT = 0x1022
    [<Literal>]
    let CAP_SERIALNUMBER = 0x1024
    [<Literal>]
    let CAP_PRINTER = 0x1026
    [<Literal>]
    let CAP_PRINTERENABLED = 0x1027
    [<Literal>]
    let CAP_PRINTERINDEX = 0x1028
    [<Literal>]
    let CAP_PRINTERMODE = 0x1029
    [<Literal>]
    let CAP_PRINTERSTRING = 0x102a
    [<Literal>]
    let CAP_PRINTERSUFFIX = 0x102b
    [<Literal>]
    let CAP_LANGUAGE = 0x102c
    [<Literal>]
    let CAP_FEEDERALIGNMENT = 0x102d
    [<Literal>]
    let CAP_FEEDERORDER = 0x102e
    [<Literal>]
    let CAP_REACQUIREALLOWED = 0x1030
    [<Literal>]
    let CAP_BATTERYMINUTES = 0x1032
    [<Literal>]
    let CAP_BATTERYPERCENTAGE = 0x1033
    [<Literal>]
    let CAP_CAMERASIDE = 0x1034
    [<Literal>]
    let CAP_SEGMENTED = 0x1035
    [<Literal>]
    let CAP_CAMERAENABLED = 0x1036
    [<Literal>]
    let CAP_CAMERAORDER = 0x1037
    [<Literal>]
    let CAP_MICRENABLED = 0x1038
    [<Literal>]
    let CAP_FEEDERPREP = 0x1039
    [<Literal>]
    let CAP_FEEDERPOCKET = 0x103a
    [<Literal>]
    let CAP_AUTOMATICSENSEMEDIUM = 0x103b
    [<Literal>]
    let CAP_CUSTOMINTERFACEGUID = 0x103c
    [<Literal>]
    let CAP_SUPPORTEDCAPSSEGMENTUNIQUE = 0x103d
    [<Literal>]
    let CAP_SUPPORTEDDATS = 0x103e
    [<Literal>]
    let CAP_DOUBLEFEEDDETECTION = 0x103f
    [<Literal>]
    let CAP_DOUBLEFEEDDETECTIONLENGTH = 0x1040
    [<Literal>]
    let CAP_DOUBLEFEEDDETECTIONSENSITIVITY = 0x1041
    [<Literal>]
    let CAP_DOUBLEFEEDDETECTIONRESPONSE = 0x1042
    [<Literal>]
    let CAP_PAPERHANDLING = 0x1043
    [<Literal>]
    let CAP_INDICATORSMODE = 0x1044
    [<Literal>]
    let CAP_PRINTERVERTICALOFFSET = 0x1045
    [<Literal>]
    let CAP_POWERSAVETIME = 0x1046
    [<Literal>]
    let CAP_PRINTERCHARROTATION = 0x1047
    [<Literal>]
    let CAP_PRINTERFONTSTYLE = 0x1048
    [<Literal>]
    let CAP_PRINTERINDEXLEADCHAR = 0x1049
    [<Literal>]
    let CAP_PRINTERINDEXMAXVALUE = 0x104A
    [<Literal>]
    let CAP_PRINTERINDEXNUMDIGITS = 0x104B
    [<Literal>]
    let CAP_PRINTERINDEXSTEP = 0x104C
    [<Literal>]
    let CAP_PRINTERINDEXTRIGGER = 0x104D
    [<Literal>]
    let CAP_PRINTERSTRINGPREVIEW = 0x104E
    [<Literal>]
    let CAP_SHEETCOUNT = 0x104F
    [<Literal>]
    let CAP_IMAGEADDRESSENABLED = 0x1050
    [<Literal>]
    let CAP_IAFIELDA_LEVEL = 0x1051
    [<Literal>]
    let CAP_IAFIELDB_LEVEL = 0x1052
    [<Literal>]
    let CAP_IAFIELDC_LEVEL = 0x1053
    [<Literal>]
    let CAP_IAFIELDD_LEVEL = 0x1054
    [<Literal>]
    let CAP_IAFIELDE_LEVEL = 0x1055
    [<Literal>]
    let CAP_IAFIELDA_PRINTFORMAT = 0x1056
    [<Literal>]
    let CAP_IAFIELDB_PRINTFORMAT = 0x1057
    [<Literal>]
    let CAP_IAFIELDC_PRINTFORMAT = 0x1058
    [<Literal>]
    let CAP_IAFIELDD_PRINTFORMAT = 0x1059
    [<Literal>]
    let CAP_IAFIELDE_PRINTFORMAT = 0x105A
    [<Literal>]
    let CAP_IAFIELDA_VALUE = 0x105B
    [<Literal>]
    let CAP_IAFIELDB_VALUE = 0x105C
    [<Literal>]
    let CAP_IAFIELDC_VALUE = 0x105D
    [<Literal>]
    let CAP_IAFIELDD_VALUE = 0x105E
    [<Literal>]
    let CAP_IAFIELDE_VALUE = 0x105F
    [<Literal>]
    let CAP_IAFIELDA_LASTPAGE = 0x1060
    [<Literal>]
    let CAP_IAFIELDB_LASTPAGE = 0x1061
    [<Literal>]
    let CAP_IAFIELDC_LASTPAGE = 0x1062
    [<Literal>]
    let CAP_IAFIELDD_LASTPAGE = 0x1063
    [<Literal>]
    let CAP_IAFIELDE_LASTPAGE = 0x1064
    [<Literal>]
    let ICAP_AUTOBRIGHT = 0x1100
    [<Literal>]
    let ICAP_BRIGHTNESS = 0x1101
    [<Literal>]
    let ICAP_CONTRAST = 0x1103
    [<Literal>]
    let ICAP_CUSTHALFTONE = 0x1104
    [<Literal>]
    let ICAP_EXPOSURETIME = 0x1105
    [<Literal>]
    let ICAP_FILTER = 0x1106
    [<Literal>]
    let ICAP_FLASHUSED = 0x1107
    [<Literal>]
    let ICAP_GAMMA = 0x1108
    [<Literal>]
    let ICAP_HALFTONES = 0x1109
    [<Literal>]
    let ICAP_HIGHLIGHT = 0x110a
    [<Literal>]
    let ICAP_IMAGEFILEFORMAT = 0x110c
    [<Literal>]
    let ICAP_LAMPSTATE = 0x110d
    [<Literal>]
    let ICAP_LIGHTSOURCE = 0x110e
    [<Literal>]
    let ICAP_ORIENTATION = 0x1110
    [<Literal>]
    let ICAP_PHYSICALWIDTH = 0x1111
    [<Literal>]
    let ICAP_PHYSICALHEIGHT = 0x1112
    [<Literal>]
    let ICAP_SHADOW = 0x1113
    [<Literal>]
    let ICAP_FRAMES = 0x1114
    [<Literal>]
    let ICAP_XNATIVERESOLUTION = 0x1116
    [<Literal>]
    let ICAP_YNATIVERESOLUTION = 0x1117
    [<Literal>]
    let ICAP_XRESOLUTION = 0x1118
    [<Literal>]
    let ICAP_YRESOLUTION = 0x1119
    [<Literal>]
    let ICAP_MAXFRAMES = 0x111a
    [<Literal>]
    let ICAP_TILES = 0x111b
    [<Literal>]
    let ICAP_BITORDER = 0x111c
    [<Literal>]
    let ICAP_CCITTKFACTOR = 0x111d
    [<Literal>]
    let ICAP_LIGHTPATH = 0x111e
    [<Literal>]
    let ICAP_PIXELFLAVOR = 0x111f
    [<Literal>]
    let ICAP_PLANARCHUNKY = 0x1120
    [<Literal>]
    let ICAP_ROTATION = 0x1121
    [<Literal>]
    let ICAP_SUPPORTEDSIZES = 0x1122
    [<Literal>]
    let ICAP_THRESHOLD = 0x1123
    [<Literal>]
    let ICAP_XSCALING = 0x1124
    [<Literal>]
    let ICAP_YSCALING = 0x1125
    [<Literal>]
    let ICAP_BITORDERCODES = 0x1126
    [<Literal>]
    let ICAP_PIXELFLAVORCODES = 0x1127
    [<Literal>]
    let ICAP_JPEGPIXELTYPE = 0x1128
    [<Literal>]
    let ICAP_TIMEFILL = 0x112a
    [<Literal>]
    let ICAP_BITDEPTH = 0x112b
    [<Literal>]
    let ICAP_BITDEPTHREDUCTION = 0x112c
    [<Literal>]
    let ICAP_UNDEFINEDIMAGESIZE = 0x112d
    [<Literal>]
    let ICAP_IMAGEDATASET = 0x112e
    [<Literal>]
    let ICAP_EXTIMAGEINFO = 0x112f
    [<Literal>]
    let ICAP_MINIMUMHEIGHT = 0x1130
    [<Literal>]
    let ICAP_MINIMUMWIDTH = 0x1131
    [<Literal>]
    let ICAP_AUTODISCARDBLANKPAGES = 0x1134
    [<Literal>]
    let ICAP_FLIPROTATION = 0x1136
    [<Literal>]
    let ICAP_BARCODEDETECTIONENABLED = 0x1137
    [<Literal>]
    let ICAP_SUPPORTEDBARCODETYPES = 0x1138
    [<Literal>]
    let ICAP_BARCODEMAXSEARCHPRIORITIES = 0x1139
    [<Literal>]
    let ICAP_BARCODESEARCHPRIORITIES = 0x113a
    [<Literal>]
    let ICAP_BARCODESEARCHMODE = 0x113b
    [<Literal>]
    let ICAP_BARCODEMAXRETRIES = 0x113c
    [<Literal>]
    let ICAP_BARCODETIMEOUT = 0x113d
    [<Literal>]
    let ICAP_ZOOMFACTOR = 0x113e
    [<Literal>]
    let ICAP_PATCHCODEDETECTIONENABLED = 0x113f
    [<Literal>]
    let ICAP_SUPPORTEDPATCHCODETYPES = 0x1140
    [<Literal>]
    let ICAP_PATCHCODEMAXSEARCHPRIORITIES = 0x1141
    [<Literal>]
    let ICAP_PATCHCODESEARCHPRIORITIES = 0x1142
    [<Literal>]
    let ICAP_PATCHCODESEARCHMODE = 0x1143
    [<Literal>]
    let ICAP_PATCHCODEMAXRETRIES = 0x1144
    [<Literal>]
    let ICAP_PATCHCODETIMEOUT = 0x1145
    [<Literal>]
    let ICAP_FLASHUSED2 = 0x1146
    [<Literal>]
    let ICAP_IMAGEFILTER = 0x1147
    [<Literal>]
    let ICAP_NOISEFILTER = 0x1148
    [<Literal>]
    let ICAP_OVERSCAN = 0x1149
    [<Literal>]
    let ICAP_AUTOMATICBORDERDETECTION = 0x1150
    [<Literal>]
    let ICAP_AUTOMATICDESKEW = 0x1151
    [<Literal>]
    let ICAP_AUTOMATICROTATE = 0x1152
    [<Literal>]
    let ICAP_JPEGQUALITY = 0x1153
    [<Literal>]
    let ICAP_FEEDERTYPE = 0x1154
    [<Literal>]
    let ICAP_ICCPROFILE = 0x1155
    [<Literal>]
    let ICAP_AUTOSIZE = 0x1156
    [<Literal>]
    let ICAP_AUTOMATICCROPUSESFRAME = 0x1157
    [<Literal>]
    let ICAP_AUTOMATICLENGTHDETECTION = 0x1158
    [<Literal>]
    let ICAP_AUTOMATICCOLORENABLED = 0x1159
    [<Literal>]
    let ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE = 0x115a
    [<Literal>]
    let ICAP_COLORMANAGEMENTENABLED = 0x115b
    [<Literal>]
    let ICAP_IMAGEMERGE = 0x115c
    [<Literal>]
    let ICAP_IMAGEMERGEHEIGHTTHRESHOLD = 0x115d
    [<Literal>]
    let ICAP_SUPPORTEDEXTIMAGEINFO = 0x115e
    [<Literal>]
    let ICAP_FILMTYPE = 0x115f
    [<Literal>]
    let ICAP_MIRROR = 0x1160
    [<Literal>]
    let ICAP_JPEGSUBSAMPLING = 0x1161
    [<Literal>]
    let ACAP_XFERMECH = 0x1202
    [<Literal>]
    let TWEI_BARCODEX = 0x1200
    [<Literal>]
    let TWEI_BARCODEY = 0x1201
    [<Literal>]
    let TWEI_BARCODETEXT = 0x1202
    [<Literal>]
    let TWEI_BARCODETYPE = 0x1203
    [<Literal>]
    let TWEI_DESHADETOP = 0x1204
    [<Literal>]
    let TWEI_DESHADELEFT = 0x1205
    [<Literal>]
    let TWEI_DESHADEHEIGHT = 0x1206
    [<Literal>]
    let TWEI_DESHADEWIDTH = 0x1207
    [<Literal>]
    let TWEI_DESHADESIZE = 0x1208
    [<Literal>]
    let TWEI_SPECKLESREMOVED = 0x1209
    [<Literal>]
    let TWEI_HORZLINEXCOORD = 0x120A
    [<Literal>]
    let TWEI_HORZLINEYCOORD = 0x120B
    [<Literal>]
    let TWEI_HORZLINELENGTH = 0x120C
    [<Literal>]
    let TWEI_HORZLINETHICKNESS = 0x120D
    [<Literal>]
    let TWEI_VERTLINEXCOORD = 0x120E
    [<Literal>]
    let TWEI_VERTLINEYCOORD = 0x120F
    [<Literal>]
    let TWEI_VERTLINELENGTH = 0x1210
    [<Literal>]
    let TWEI_VERTLINETHICKNESS = 0x1211
    [<Literal>]
    let TWEI_PATCHCODE = 0x1212
    [<Literal>]
    let TWEI_ENDORSEDTEXT = 0x1213
    [<Literal>]
    let TWEI_FORMCONFIDENCE = 0x1214
    [<Literal>]
    let TWEI_FORMTEMPLATEMATCH = 0x1215
    [<Literal>]
    let TWEI_FORMTEMPLATEPAGEMATCH = 0x1216
    [<Literal>]
    let TWEI_FORMHORZDOCOFFSET = 0x1217
    [<Literal>]
    let TWEI_FORMVERTDOCOFFSET = 0x1218
    [<Literal>]
    let TWEI_BARCODECOUNT = 0x1219
    [<Literal>]
    let TWEI_BARCODECONFIDENCE = 0x121A
    [<Literal>]
    let TWEI_BARCODEROTATION = 0x121B
    [<Literal>]
    let TWEI_BARCODETEXTLENGTH = 0x121C
    [<Literal>]
    let TWEI_DESHADECOUNT = 0x121D
    [<Literal>]
    let TWEI_DESHADEBLACKCOUNTOLD = 0x121E
    [<Literal>]
    let TWEI_DESHADEBLACKCOUNTNEW = 0x121F
    [<Literal>]
    let TWEI_DESHADEBLACKRLMIN = 0x1220
    [<Literal>]
    let TWEI_DESHADEBLACKRLMAX = 0x1221
    [<Literal>]
    let TWEI_DESHADEWHITECOUNTOLD = 0x1222
    [<Literal>]
    let TWEI_DESHADEWHITECOUNTNEW = 0x1223
    [<Literal>]
    let TWEI_DESHADEWHITERLMIN = 0x1224
    [<Literal>]
    let TWEI_DESHADEWHITERLAVE = 0x1225
    [<Literal>]
    let TWEI_DESHADEWHITERLMAX = 0x1226
    [<Literal>]
    let TWEI_BLACKSPECKLESREMOVED = 0x1227
    [<Literal>]
    let TWEI_WHITESPECKLESREMOVED = 0x1228
    [<Literal>]
    let TWEI_HORZLINECOUNT = 0x1229
    [<Literal>]
    let TWEI_VERTLINECOUNT = 0x122A
    [<Literal>]
    let TWEI_DESKEWSTATUS = 0x122B
    [<Literal>]
    let TWEI_SKEWORIGINALANGLE = 0x122C
    [<Literal>]
    let TWEI_SKEWFINALANGLE = 0x122D
    [<Literal>]
    let TWEI_SKEWCONFIDENCE = 0x122E
    [<Literal>]
    let TWEI_SKEWWINDOWX1 = 0x122F
    [<Literal>]
    let TWEI_SKEWWINDOWY1 = 0x1230
    [<Literal>]
    let TWEI_SKEWWINDOWX2 = 0x1231
    [<Literal>]
    let TWEI_SKEWWINDOWY2 = 0x1232
    [<Literal>]
    let TWEI_SKEWWINDOWX3 = 0x1233
    [<Literal>]
    let TWEI_SKEWWINDOWY3 = 0x1234
    [<Literal>]
    let TWEI_SKEWWINDOWX4 = 0x1235
    [<Literal>]
    let TWEI_SKEWWINDOWY4 = 0x1236
    [<Literal>]
    let TWEI_BOOKNAME = 0x1238
    [<Literal>]
    let TWEI_CHAPTERNUMBER = 0x1239
    [<Literal>]
    let TWEI_DOCUMENTNUMBER = 0x123A
    [<Literal>]
    let TWEI_PAGENUMBER = 0x123B
    [<Literal>]
    let TWEI_CAMERA = 0x123C
    [<Literal>]
    let TWEI_FRAMENUMBER = 0x123D
    [<Literal>]
    let TWEI_FRAME = 0x123E
    [<Literal>]
    let TWEI_PIXELFLAVOR = 0x123F
    [<Literal>]
    let TWEI_ICCPROFILE = 0x1240
    [<Literal>]
    let TWEI_LASTSEGMENT = 0x1241
    [<Literal>]
    let TWEI_SEGMENTNUMBER = 0x1242
    [<Literal>]
    let TWEI_MAGDATA = 0x1243
    [<Literal>]
    let TWEI_MAGTYPE = 0x1244
    [<Literal>]
    let TWEI_PAGESIDE = 0x1245
    [<Literal>]
    let TWEI_FILESYSTEMSOURCE = 0x1246
    [<Literal>]
    let TWEI_IMAGEMERGED = 0x1247
    [<Literal>]
    let TWEI_MAGDATALENGTH = 0x1248
    [<Literal>]
    let TWEI_PAPERCOUNT = 0x1249
    [<Literal>]
    let TWEI_PRINTERTEXT = 0x124A
    [<Literal>]
    let TWEI_TWAINDIRECTMETADATA = 0x124B
    [<Literal>]
    let TWEI_IAFIELDA_VALUE = 0x124C
    [<Literal>]
    let TWEI_IAFIELDB_VALUE = 0x124D
    [<Literal>]
    let TWEI_IAFIELDC_VALUE = 0x124E
    [<Literal>]
    let TWEI_IAFIELDD_VALUE = 0x124F
    [<Literal>]
    let TWEI_IAFIELDE_VALUE = 0x1250
    [<Literal>]
    let TWEI_IALEVEL = 0x1251
    [<Literal>]
    let TWEI_PRINTER = 0x1252
    [<Literal>]
    let TWEI_BARCODETEXT2 = 0x1253
    [<Literal>]
    let TWEJ_NONE = 0x0000
    [<Literal>]
    let TWEJ_MIDSEPARATOR = 0x0001
    [<Literal>]
    let TWEJ_PATCH1 = 0x0002
    [<Literal>]
    let TWEJ_PATCH2 = 0x0003
    [<Literal>]
    let TWEJ_PATCH3 = 0x0004
    [<Literal>]
    let TWEJ_PATCH4 = 0x0005
    [<Literal>]
    let TWEJ_PATCH6 = 0x0006
    [<Literal>]
    let TWEJ_PATCHT = 0x0007
    [<Literal>]
    let TWRC_CUSTOMBASE = 0x8000
    [<Literal>]
    let TWRC_SUCCESS = 0
    [<Literal>]
    let TWRC_FAILURE = 1
    [<Literal>]
    let TWRC_CHECKSTATUS = 2
    [<Literal>]
    let TWRC_CANCEL = 3
    [<Literal>]
    let TWRC_DSEVENT = 4
    [<Literal>]
    let TWRC_NOTDSEVENT = 5
    [<Literal>]
    let TWRC_XFERDONE = 6
    [<Literal>]
    let TWRC_ENDOFLIST = 7
    [<Literal>]
    let TWRC_INFONOTSUPPORTED = 8
    [<Literal>]
    let TWRC_DATANOTAVAILABLE = 9
    [<Literal>]
    let TWRC_BUSY = 10
    [<Literal>]
    let TWRC_SCANNERLOCKED = 11
    [<Literal>]
    let TWCC_CUSTOMBASE = 0x8000
    [<Literal>]
    let TWCC_SUCCESS = 0
    [<Literal>]
    let TWCC_BUMMER = 1
    [<Literal>]
    let TWCC_LOWMEMORY = 2
    [<Literal>]
    let TWCC_NODS = 3
    [<Literal>]
    let TWCC_MAXCONNECTIONS = 4
    [<Literal>]
    let TWCC_OPERATIONERROR = 5
    [<Literal>]
    let TWCC_BADCAP = 6
    [<Literal>]
    let TWCC_BADPROTOCOL = 9
    [<Literal>]
    let TWCC_BADVALUE = 10
    [<Literal>]
    let TWCC_SEQERROR = 11
    [<Literal>]
    let TWCC_BADDEST = 12
    [<Literal>]
    let TWCC_CAPUNSUPPORTED = 13
    [<Literal>]
    let TWCC_CAPBADOPERATION = 14
    [<Literal>]
    let TWCC_CAPSEQERROR = 15
    [<Literal>]
    let TWCC_DENIED = 16
    [<Literal>]
    let TWCC_FILEEXISTS = 17
    [<Literal>]
    let TWCC_FILENOTFOUND = 18
    [<Literal>]
    let TWCC_NOTEMPTY = 19
    [<Literal>]
    let TWCC_PAPERJAM = 20
    [<Literal>]
    let TWCC_PAPERDOUBLEFEED = 21
    [<Literal>]
    let TWCC_FILEWRITEERROR = 22
    [<Literal>]
    let TWCC_CHECKDEVICEONLINE = 23
    [<Literal>]
    let TWCC_INTERLOCK = 24
    [<Literal>]
    let TWCC_DAMAGEDCORNER = 25
    [<Literal>]
    let TWCC_FOCUSERROR = 26
    [<Literal>]
    let TWCC_DOCTOOLIGHT = 27
    [<Literal>]
    let TWCC_DOCTOODARK = 28
    [<Literal>]
    let TWCC_NOMEDIA = 29
    [<Literal>]
    let TWQC_GET = 0x0001
    [<Literal>]
    let TWQC_SET = 0x0002
    [<Literal>]
    let TWQC_GETDEFAULT = 0x0004
    [<Literal>]
    let TWQC_GETCURRENT = 0x0008
    [<Literal>]
    let TWQC_RESET = 0x0010
    [<Literal>]
    let TWQC_SETCONSTRAINT = 0x0020
    [<Literal>]
    let TWQC_GETHELP = 0x0100
    [<Literal>]
    let TWQC_GETLABEL = 0x0200
    [<Literal>]
    let TWQC_GETLABELENUM = 0x0400
    [<Literal>]
    let TWTY_STR1024 = 0x000d
    [<Literal>]
    let TWTY_UNI512 = 0x000e
    [<Literal>]
    let TWFF_JPN = 12
    [<Literal>]
    let DAT_TWUNKIDENTITY = 0x000b
    [<Literal>]
    let DAT_SETUPFILEXFER2 = 0x0301
    [<Literal>]
    let CAP_CLEARBUFFERS = 0x101d
    [<Literal>]
    let CAP_SUPPORTEDCAPSEXT = 0x100c
    [<Literal>]
    let CAP_PAGEMULTIPLEACQUIRE = 0x1023
    [<Literal>]
    let CAP_PAPERBINDING = 0x102f
    [<Literal>]
    let CAP_PASSTHRU = 0x1031
    [<Literal>]
    let CAP_POWERDOWNTIME = 0x1034
    [<Literal>]
    let ACAP_AUDIOFILEFORMAT = 0x1201
    [<Literal>]
    let MSG_CHECKSTATUS = 0x0201
    [<Literal>]
    let MSG_INVOKE_CALLBACK = 0x0903
    [<Literal>]
    let TWQC_CONSTRAINABLE = 0x0040
    [<Literal>]
    let TWSX_FILE2 = 3
    [<Literal>]
    let TWFS_FILESYSTEM = 0
    [<Literal>]
    let TWFS_RECURSIVEDELETE = 1
    [<Literal>]
    let TWPT_SRGB64 = 11
    [<Literal>]
    let TWPT_BGR = 12
    [<Literal>]
    let TWPT_CIELAB = 13
    [<Literal>]
    let TWPT_CIELUV = 14
    [<Literal>]
    let TWPT_YCBCR = 15
    [<Literal>]
    let TWSS_B = 8
    [<Literal>]
    let TWSS_A4LETTER = TWSS_A4
    [<Literal>]
    let TWSS_B3 = TWSS_ISOB3
    [<Literal>]
    let TWSS_B4 = TWSS_ISOB4
    [<Literal>]
    let TWSS_B6 = TWSS_ISOB6
    [<Literal>]
    let TWSS_B5LETTER = TWSS_JISB5
    [<Literal>]
    let TWAF_WAV = 0
    [<Literal>]
    let TWAF_AIFF = 1
    [<Literal>]
    let TWAF_AU = 3
    [<Literal>]
    let TWAF_SND = 4
    [<Literal>]
    let TWCB_AUTO = 0
    [<Literal>]
    let TWCB_CLEAR = 1
    [<Literal>]
    let TWCB_NOCLEAR = 2
