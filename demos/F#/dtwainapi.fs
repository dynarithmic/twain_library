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

module dtwainapi

open System
open System.Runtime.InteropServices
open System.Text
open System.IO

type DTWAIN_ARRAY = IntPtr
type DTWAIN_SOURCE = IntPtr
type DTWAIN_RANGE = IntPtr
type DTWAIN_FRAME = IntPtr
type DTWAIN_OCRENGINE = IntPtr
type DTWAIN_OCRTEXTINFOHANDLE = IntPtr
type DTWAIN_PDFTEXTELEMENT = IntPtr
type DTWAIN_HANDLE = IntPtr
type DTWAIN_BOOL = Int32
type BOOL = Int32
type LONG = Int32
type DTWAIN_FLOAT = double
type DTWAIN_LONG = Int32
type DTWAIN_LONG64 = Int64
type LPVOID = IntPtr
type HWND       = IntPtr
type HINSTANCE = IntPtr
type HANDLE = IntPtr
type DTWAIN_MEMORY_PTR = IntPtr
type TWAIN_IDENTITY = IntPtr
type DWORD = UInt32
type ULONG64 = UInt64
type LONGLONG = Int64
type LONG64 = Int64
type DTWAIN_IDENTITY = IntPtr

[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_CALLBACK_PROC64 = delegate of nativeint * nativeint * int64 -> nativeint
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetCallback64Delegate = delegate of DTWAIN_CALLBACK_PROC64 * int64 -> nativeint

[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_CALLBACK_PROC = delegate of nativeint * nativeint * nativeint -> nativeint
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetCallbackDelegate = delegate of DTWAIN_CALLBACK_PROC * int32 -> nativeint

[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_DIBUPDATE_PROC = delegate of DTWAIN_SOURCE * int32 * IntPtr -> IntPtr
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetUpdateDibProcDelegate = delegate of DTWAIN_DIBUPDATE_PROC -> nativeint

[<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
type DTWAIN_LOGGER_PROC = delegate of string * int64 -> nativeint
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetLoggerCallbackDelegate = delegate of DTWAIN_LOGGER_PROC * int64 -> nativeint

[<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
type DTWAIN_LOGGER_PROCA = delegate of string * int64 -> nativeint
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetLoggerCallbackADelegate = delegate of DTWAIN_LOGGER_PROCA * int64 -> nativeint

[<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
type DTWAIN_LOGGER_PROCW = delegate of string * int64 -> nativeint
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetLoggerCallbackWDelegate = delegate of DTWAIN_LOGGER_PROCW * int64 -> nativeint

[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_ERROR_PROC = delegate of int * int -> nativeint
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetErrorCallbackDelegate = delegate of DTWAIN_ERROR_PROC * int32 -> nativeint

[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_ERROR_PROC64 = delegate of int * int64 -> nativeint
[<UnmanagedFunctionPointer(CallingConvention.StdCall)>]
type DTWAIN_SetErrorCallback64Delegate = delegate of DTWAIN_ERROR_PROC64 * int64 -> nativeint

[<Struct>]
[<StructLayout(LayoutKind.Sequential)>]
type MSG =
    {
        mutable hwnd    : IntPtr
        mutable message : uint32
        mutable wParam  : IntPtr
        mutable lParam  : IntPtr
        mutable time    : uint32
        mutable pt      : POINT
    }
    
and [<Struct; StructLayout(LayoutKind.Sequential)>] POINT =
    {
        mutable x : int32
        mutable y : int32
    }

module NativeMethods =
    [<DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)>]
    extern IntPtr LoadLibrary(string lpFileName)
    [<DllImport("kernel32.dll", SetLastError = true)>]
    extern bool FreeLibrary(IntPtr hModule)
    [<DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)>]
    extern IntPtr GetProcAddress(IntPtr hModule, string procName)
    [<DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)>]
    extern uint32 FormatMessage(uint32 dwFlags,IntPtr lpSource,uint32 dwMessageId,
                                uint32 dwLanguageId,StringBuilder lpBuffer,uint32 nSize,IntPtr Arguments)

module TwainAPI =
    [<Struct; StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)>]
    type TW_VERSION =
        {
            MajorNum : uint16
            MinorNum : uint16
            Language : uint16
            Country  : uint16
            [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)>]
            Info     : string
        }

    [<Struct; StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)>]
    type TW_IDENTITY =
        {
            Id              : uint32
            Version         : TW_VERSION
            ProtocolMajor   : uint16
            ProtocolMinor   : uint16
            SupportedGroups : uint32
            [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)>]
            Manufacturer    : string
            [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)>]
            ProductFamily   : string
            [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)>]
            ProductName     : string
        }

    let public DTWAIN_FF_TIFF = 0
    let public DTWAIN_FF_PICT = 1
    let public DTWAIN_FF_BMP = 2
    let public DTWAIN_FF_XBM = 3
    let public DTWAIN_FF_JFIF = 4
    let public DTWAIN_FF_FPX = 5
    let public DTWAIN_FF_TIFFMULTI = 6
    let public DTWAIN_FF_PNG = 7
    let public DTWAIN_FF_SPIFF = 8
    let public DTWAIN_FF_EXIF = 9
    let public DTWAIN_FF_PDF = 10
    let public DTWAIN_FF_JP2 = 11
    let public DTWAIN_FF_JPX = 13
    let public DTWAIN_FF_DEJAVU = 14
    let public DTWAIN_FF_PDFA = 15
    let public DTWAIN_FF_PDFA2 = 16
    let public DTWAIN_FF_PDFRASTER = 17
    let public DTWAIN_CP_NONE = 0
    let public DTWAIN_CP_PACKBITS = 1
    let public DTWAIN_CP_GROUP31D = 2
    let public DTWAIN_CP_GROUP31DEOL = 3
    let public DTWAIN_CP_GROUP32D = 4
    let public DTWAIN_CP_GROUP4 = 5
    let public DTWAIN_CP_JPEG = 6
    let public DTWAIN_CP_LZW = 7
    let public DTWAIN_CP_JBIG = 8
    let public DTWAIN_CP_PNG = 9
    let public DTWAIN_CP_RLE4 = 10
    let public DTWAIN_CP_RLE8 = 11
    let public DTWAIN_CP_BITFIELDS = 12
    let public DTWAIN_CP_ZIP = 13
    let public DTWAIN_CP_JPEG2000 = 14
    let public DTWAIN_FS_NONE = 0
    let public DTWAIN_FS_A4LETTER = 1
    let public DTWAIN_FS_B5LETTER = 2
    let public DTWAIN_FS_USLETTER = 3
    let public DTWAIN_FS_USLEGAL = 4
    let public DTWAIN_FS_A5 = 5
    let public DTWAIN_FS_B4 = 6
    let public DTWAIN_FS_B6 = 7
    let public DTWAIN_FS_USLEDGER = 9
    let public DTWAIN_FS_USEXECUTIVE = 10
    let public DTWAIN_FS_A3 = 11
    let public DTWAIN_FS_B3 = 12
    let public DTWAIN_FS_A6 = 13
    let public DTWAIN_FS_C4 = 14
    let public DTWAIN_FS_C5 = 15
    let public DTWAIN_FS_C6 = 16
    let public DTWAIN_FS_4A0 = 17
    let public DTWAIN_FS_2A0 = 18
    let public DTWAIN_FS_A0 = 19
    let public DTWAIN_FS_A1 = 20
    let public DTWAIN_FS_A2 = 21
    let public DTWAIN_FS_A4 = DTWAIN_FS_A4LETTER
    let public DTWAIN_FS_A7 = 22
    let public DTWAIN_FS_A8 = 23
    let public DTWAIN_FS_A9 = 24
    let public DTWAIN_FS_A10 = 25
    let public DTWAIN_FS_ISOB0 = 26
    let public DTWAIN_FS_ISOB1 = 27
    let public DTWAIN_FS_ISOB2 = 28
    let public DTWAIN_FS_ISOB3 = DTWAIN_FS_B3
    let public DTWAIN_FS_ISOB4 = DTWAIN_FS_B4
    let public DTWAIN_FS_ISOB5 = 29
    let public DTWAIN_FS_ISOB6 = DTWAIN_FS_B6
    let public DTWAIN_FS_ISOB7 = 30
    let public DTWAIN_FS_ISOB8 = 31
    let public DTWAIN_FS_ISOB9 = 32
    let public DTWAIN_FS_ISOB10 = 33
    let public DTWAIN_FS_JISB0 = 34
    let public DTWAIN_FS_JISB1 = 35
    let public DTWAIN_FS_JISB2 = 36
    let public DTWAIN_FS_JISB3 = 37
    let public DTWAIN_FS_JISB4 = 38
    let public DTWAIN_FS_JISB5 = DTWAIN_FS_B5LETTER
    let public DTWAIN_FS_JISB6 = 39
    let public DTWAIN_FS_JISB7 = 40
    let public DTWAIN_FS_JISB8 = 41
    let public DTWAIN_FS_JISB9 = 42
    let public DTWAIN_FS_JISB10 = 43
    let public DTWAIN_FS_C0 = 44
    let public DTWAIN_FS_C1 = 45
    let public DTWAIN_FS_C2 = 46
    let public DTWAIN_FS_C3 = 47
    let public DTWAIN_FS_C7 = 48
    let public DTWAIN_FS_C8 = 49
    let public DTWAIN_FS_C9 = 50
    let public DTWAIN_FS_C10 = 51
    let public DTWAIN_FS_USSTATEMENT = 52
    let public DTWAIN_FS_BUSINESSCARD = 53
    let public DTWAIN_ANYSUPPORT = (-1)
    let public DTWAIN_BMP = 100
    let public DTWAIN_JPEG = 200
    let public DTWAIN_PDF = 250
    let public DTWAIN_PDFMULTI = 251
    let public DTWAIN_PCX = 300
    let public DTWAIN_DCX = 301
    let public DTWAIN_TGA = 400
    let public DTWAIN_TIFFLZW = 500
    let public DTWAIN_TIFFNONE = 600
    let public DTWAIN_TIFFG3 = 700
    let public DTWAIN_TIFFG4 = 800
    let public DTWAIN_TIFFPACKBITS = 801
    let public DTWAIN_TIFFDEFLATE = 802
    let public DTWAIN_TIFFJPEG = 803
    let public DTWAIN_TIFFJBIG = 804
    let public DTWAIN_TIFFPIXARLOG = 805
    let public DTWAIN_TIFFNONEMULTI = 900
    let public DTWAIN_TIFFG3MULTI = 901
    let public DTWAIN_TIFFG4MULTI = 902
    let public DTWAIN_TIFFPACKBITSMULTI = 903
    let public DTWAIN_TIFFDEFLATEMULTI = 904
    let public DTWAIN_TIFFJPEGMULTI = 905
    let public DTWAIN_TIFFLZWMULTI = 906
    let public DTWAIN_TIFFJBIGMULTI = 907
    let public DTWAIN_TIFFPIXARLOGMULTI = 908
    let public DTWAIN_WMF = 850
    let public DTWAIN_EMF = 851
    let public DTWAIN_GIF = 950
    let public DTWAIN_PNG = 1000
    let public DTWAIN_PSD = 2000
    let public DTWAIN_JPEG2000 = 3000
    let public DTWAIN_POSTSCRIPT1 = 4000
    let public DTWAIN_POSTSCRIPT2 = 4001
    let public DTWAIN_POSTSCRIPT3 = 4002
    let public DTWAIN_POSTSCRIPT1MULTI = 4003
    let public DTWAIN_POSTSCRIPT2MULTI = 4004
    let public DTWAIN_POSTSCRIPT3MULTI = 4005
    let public DTWAIN_TEXT = 6000
    let public DTWAIN_TEXTMULTI = 6001
    let public DTWAIN_TIFFMULTI = 7000
    let public DTWAIN_ICO = 8000
    let public DTWAIN_ICO_VISTA = 8001
    let public DTWAIN_ICO_RESIZED = 8002
    let public DTWAIN_WBMP = 8500
    let public DTWAIN_WEBP = 8501
    let public DTWAIN_PPM = 10000
    let public DTWAIN_WBMP_RESIZED = 11000
    let public DTWAIN_TGA_RLE = 11001
    let public DTWAIN_BMP_RLE = 11002
    let public DTWAIN_BIGTIFFLZW = 11003
    let public DTWAIN_BIGTIFFLZWMULTI = 11004
    let public DTWAIN_BIGTIFFNONE = 11005
    let public DTWAIN_BIGTIFFNONEMULTI = 11006
    let public DTWAIN_BIGTIFFPACKBITS = 11007
    let public DTWAIN_BIGTIFFPACKBITSMULTI = 11008
    let public DTWAIN_BIGTIFFDEFLATE = 11009
    let public DTWAIN_BIGTIFFDEFLATEMULTI = 11010
    let public DTWAIN_BIGTIFFG3 = 11011
    let public DTWAIN_BIGTIFFG3MULTI = 11012
    let public DTWAIN_BIGTIFFG4 = 11013
    let public DTWAIN_BIGTIFFG4MULTI = 11014
    let public DTWAIN_BIGTIFFJPEG = 11015
    let public DTWAIN_BIGTIFFJPEGMULTI = 11016
    let public DTWAIN_JPEGXR = 12000
    let public DTWAIN_INCHES = 0
    let public DTWAIN_CENTIMETERS = 1
    let public DTWAIN_PICAS = 2
    let public DTWAIN_POINTS = 3
    let public DTWAIN_TWIPS = 4
    let public DTWAIN_PIXELS = 5
    let public DTWAIN_MILLIMETERS = 6
    let public DTWAIN_USENAME = 16
    let public DTWAIN_USEPROMPT = 32
    let public DTWAIN_USELONGNAME = 64
    let public DTWAIN_USESOURCEMODE = 128
    let public DTWAIN_USELIST = 256
    let public DTWAIN_CREATE_DIRECTORY = 512
    let public DTWAIN_CREATEDIRECTORY = DTWAIN_CREATE_DIRECTORY
    let public DTWAIN_ARRAYANY = 1
    let public DTWAIN_ArrayTypePTR = 1
    let public DTWAIN_ARRAYLONG = 2
    let public DTWAIN_ARRAYFLOAT = 3
    let public DTWAIN_ARRAYHANDLE = 4
    let public DTWAIN_ARRAYSOURCE = 5
    let public DTWAIN_ARRAYSTRING = 6
    let public DTWAIN_ARRAYFRAME = 7
    let public DTWAIN_ARRAYBOOL = DTWAIN_ARRAYLONG
    let public DTWAIN_ARRAYLONGSTRING = 8
    let public DTWAIN_ARRAYUNICODESTRING = 9
    let public DTWAIN_ARRAYLONG64 = 10
    let public DTWAIN_ARRAYANSISTRING = 11
    let public DTWAIN_ARRAYWIDESTRING = 12
    let public DTWAIN_ARRAYTWFIX32 = 200
    let public DTWAIN_ArrayTypeINVALID = 0
    let public DTWAIN_ARRAYINT16 = 100
    let public DTWAIN_ARRAYUINT16 = 110
    let public DTWAIN_ARRAYUINT32 = 120
    let public DTWAIN_ARRAYINT32 = 130
    let public DTWAIN_ARRAYINT64 = 140
    let public DTWAIN_ARRAYUINT64 = 150
    let public DTWAIN_RANGELONG = DTWAIN_ARRAYLONG
    let public DTWAIN_RANGEFLOAT = DTWAIN_ARRAYFLOAT
    let public DTWAIN_RANGEMIN = 0
    let public DTWAIN_RANGEMAX = 1
    let public DTWAIN_RANGESTEP = 2
    let public DTWAIN_RANGEDEFAULT = 3
    let public DTWAIN_RANGECURRENT = 4
    let public DTWAIN_FRAMELEFT = 0
    let public DTWAIN_FRAMETOP = 1
    let public DTWAIN_FRAMERIGHT = 2
    let public DTWAIN_FRAMEBOTTOM = 3
    let public DTWAIN_FIX32WHOLE = 0
    let public DTWAIN_FIX32FRAC = 1
    let public DTWAIN_JC_NONE = 0
    let public DTWAIN_JC_JSIC = 1
    let public DTWAIN_JC_JSIS = 2
    let public DTWAIN_JC_JSXC = 3
    let public DTWAIN_JC_JSXS = 4
    let public DTWAIN_CAPDATATYPE_UNKNOWN = (-10)
    let public DTWAIN_JCBP_JSIC = 5
    let public DTWAIN_JCBP_JSIS = 6
    let public DTWAIN_JCBP_JSXC = 7
    let public DTWAIN_JCBP_JSXS = 8
    let public DTWAIN_FEEDPAGEON = 1
    let public DTWAIN_CLEARPAGEON = 2
    let public DTWAIN_REWINDPAGEON = 4
    let public DTWAIN_AppOwnsDib = 1
    let public DTWAIN_SourceOwnsDib = 2
    let public DTWAIN_CONTARRAY = 8
    let public DTWAIN_CONTENUMERATION = 16
    let public DTWAIN_CONTONEVALUE = 32
    let public DTWAIN_CONTRANGE = 64
    let public DTWAIN_CONTDEFAULT = 0
    let public DTWAIN_CAPGET = 1
    let public DTWAIN_CAPGETCURRENT = 2
    let public DTWAIN_CAPGETDEFAULT = 3
    let public DTWAIN_CAPSET = 6
    let public DTWAIN_CAPRESET = 7
    let public DTWAIN_CAPRESETALL = 8
    let public DTWAIN_CAPSETCONSTRAINT = 9
    let public DTWAIN_CAPSETAVAILABLE = 8
    let public DTWAIN_CAPSETCURRENT = 16
    let public DTWAIN_CAPGETHELP = 9
    let public DTWAIN_CAPGETLABEL = 10
    let public DTWAIN_CAPGETLABELENUM = 11
    let public DTWAIN_AREASET = DTWAIN_CAPSET
    let public DTWAIN_AREARESET = DTWAIN_CAPRESET
    let public DTWAIN_AREACURRENT = DTWAIN_CAPGETCURRENT
    let public DTWAIN_AREADEFAULT = DTWAIN_CAPGETDEFAULT
    let public DTWAIN_VER15 = 0
    let public DTWAIN_VER16 = 1
    let public DTWAIN_VER17 = 2
    let public DTWAIN_VER18 = 3
    let public DTWAIN_VER20 = 4
    let public DTWAIN_VER21 = 5
    let public DTWAIN_VER22 = 6
    let public DTWAIN_ACQUIREALL = (-1)
    let public DTWAIN_MAXACQUIRE = (-1)
    let public DTWAIN_DX_NONE = 0
    let public DTWAIN_DX_1PASSDUPLEX = 1
    let public DTWAIN_DX_2PASSDUPLEX = 2
    let public DTWAIN_PT_BW = 0
    let public DTWAIN_PT_GRAY = 1
    let public DTWAIN_PT_RGB = 2
    let public DTWAIN_PT_PALETTE = 3
    let public DTWAIN_PT_CMY = 4
    let public DTWAIN_PT_CMYK = 5
    let public DTWAIN_PT_YUV = 6
    let public DTWAIN_PT_YUVK = 7
    let public DTWAIN_PT_CIEXYZ = 8
    let public DTWAIN_PT_DEFAULT = 1000
    let public DTWAIN_CURRENT = (-2)
    let public DTWAIN_DEFAULT = (-1)
    let public DTWAIN_FLOATDEFAULT = (-9999.0)
    let public DTWAIN_CallbackERROR = 1
    let public DTWAIN_CallbackMESSAGE = 2
    let public DTWAIN_USENATIVE = 1
    let public DTWAIN_USEBUFFERED = 2
    let public DTWAIN_USECOMPRESSION = 4
    let public DTWAIN_USEMEMFILE = 8
    let public DTWAIN_FAILURE1 = (-1)
    let public DTWAIN_FAILURE2 = (-2)
    let public DTWAIN_DELETEALL = (-1)
    let public DTWAIN_TN_ACQUIREDONE = 1000
    let public DTWAIN_TN_ACQUIREFAILED = 1001
    let public DTWAIN_TN_ACQUIRECANCELLED = 1002
    let public DTWAIN_TN_ACQUIRESTARTED = 1003
    let public DTWAIN_TN_PAGECONTINUE = 1004
    let public DTWAIN_TN_PAGEFAILED = 1005
    let public DTWAIN_TN_PAGECANCELLED = 1006
    let public DTWAIN_TN_TRANSFERREADY = 1009
    let public DTWAIN_TN_TRANSFERDONE = 1010
    let public DTWAIN_TN_ACQUIREPAGEDONE = 1010
    let public DTWAIN_TN_UICLOSING = 3000
    let public DTWAIN_TN_UICLOSED = 3001
    let public DTWAIN_TN_UIOPENED = 3002
    let public DTWAIN_TN_UIOPENING = 3003
    let public DTWAIN_TN_UIOPENFAILURE = 3004
    let public DTWAIN_TN_CLIPTRANSFERDONE = 1014
    let public DTWAIN_TN_INVALIDIMAGEFORMAT = 1015
    let public DTWAIN_TN_ACQUIRETERMINATED = 1021
    let public DTWAIN_TN_TRANSFERSTRIPREADY = 1022
    let public DTWAIN_TN_TRANSFERSTRIPDONE = 1023
    let public DTWAIN_TN_TRANSFERSTRIPFAILED = 1029
    let public DTWAIN_TN_IMAGEINFOERROR = 1024
    let public DTWAIN_TN_TRANSFERCANCELLED = 1030
    let public DTWAIN_TN_FILESAVECANCELLED = 1031
    let public DTWAIN_TN_FILESAVEOK = 1032
    let public DTWAIN_TN_FILESAVEERROR = 1033
    let public DTWAIN_TN_FILEPAGESAVEOK = 1034
    let public DTWAIN_TN_FILEPAGESAVEERROR = 1035
    let public DTWAIN_TN_PROCESSEDDIB = 1036
    let public DTWAIN_TN_FEEDERLOADED = 1037
    let public DTWAIN_TN_GENERALERROR = 1038
    let public DTWAIN_TN_MANDUPFLIPPAGES = 1040
    let public DTWAIN_TN_MANDUPSIDE1DONE = 1041
    let public DTWAIN_TN_MANDUPSIDE2DONE = 1042
    let public DTWAIN_TN_MANDUPPAGECOUNTERROR = 1043
    let public DTWAIN_TN_MANDUPACQUIREDONE = 1044
    let public DTWAIN_TN_MANDUPSIDE1START = 1045
    let public DTWAIN_TN_MANDUPSIDE2START = 1046
    let public DTWAIN_TN_MANDUPMERGEERROR = 1047
    let public DTWAIN_TN_MANDUPMEMORYERROR = 1048
    let public DTWAIN_TN_MANDUPFILEERROR = 1049
    let public DTWAIN_TN_MANDUPFILESAVEERROR = 1050
    let public DTWAIN_TN_ENDOFJOBDETECTED = 1051
    let public DTWAIN_TN_EOJDETECTED = 1051
    let public DTWAIN_TN_EOJDETECTED_XFERDONE = 1052
    let public DTWAIN_TN_QUERYPAGEDISCARD = 1053
    let public DTWAIN_TN_PAGEDISCARDED = 1054
    let public DTWAIN_TN_PROCESSDIBACCEPTED = 1055
    let public DTWAIN_TN_PROCESSDIBFINALACCEPTED = 1056
    let public DTWAIN_TN_CLOSEDIBFAILED = 1057
    let public DTWAIN_TN_INVALID_TWAINDSM2_BITMAP = 1058
    let public DTWAIN_TN_IMAGE_RESAMPLE_FAILURE = 1059
    let public DTWAIN_TN_DEVICEEVENT = 1100
    let public DTWAIN_TN_TWAINPAGECANCELLED = 1105
    let public DTWAIN_TN_TWAINPAGEFAILED = 1106
    let public DTWAIN_TN_APPUPDATEDDIB = 1107
    let public DTWAIN_TN_FILEPAGESAVING = 1110
    let public DTWAIN_TN_EOJBEGINFILESAVE = 1112
    let public DTWAIN_TN_EOJENDFILESAVE = 1113
    let public DTWAIN_TN_CROPFAILED = 1120
    let public DTWAIN_TN_PROCESSEDDIBFINAL = 1121
    let public DTWAIN_TN_BLANKPAGEDETECTED1 = 1130
    let public DTWAIN_TN_BLANKPAGEDETECTED2 = 1131
    let public DTWAIN_TN_BLANKPAGEDETECTED3 = 1132
    let public DTWAIN_TN_BLANKPAGEDISCARDED1 = 1133
    let public DTWAIN_TN_BLANKPAGEDISCARDED2 = 1134
    let public DTWAIN_TN_OCRTEXTRETRIEVED = 1140
    let public DTWAIN_TN_QUERYOCRTEXT = 1141
    let public DTWAIN_TN_PDFOCRREADY = 1142
    let public DTWAIN_TN_PDFOCRDONE = 1143
    let public DTWAIN_TN_PDFOCRERROR = 1144
    let public DTWAIN_TN_SETCALLBACKINIT = 1150
    let public DTWAIN_TN_SETCALLBACK64INIT = 1151
    let public DTWAIN_TN_FILENAMECHANGING = 1160
    let public DTWAIN_TN_FILENAMECHANGED = 1161
    let public DTWAIN_TN_PROCESSEDAUDIOFINAL = 1180
    let public DTWAIN_TN_PROCESSAUDIOFINALACCEPTED = 1181
    let public DTWAIN_TN_PROCESSEDAUDIOFILE = 1182
    let public DTWAIN_TN_TWAINTRIPLETBEGIN = 1183
    let public DTWAIN_TN_TWAINTRIPLETEND = 1184
    let public DTWAIN_TN_FEEDERNOTLOADED = 1201
    let public DTWAIN_TN_FEEDERTIMEOUT = 1202
    let public DTWAIN_TN_FEEDERNOTENABLED = 1203
    let public DTWAIN_TN_FEEDERNOTSUPPORTED = 1204
    let public DTWAIN_TN_FEEDERTOFLATBED = 1205
    let public DTWAIN_TN_PREACQUIRESTART = 1206
    let public DTWAIN_TN_TRANSFERTILEREADY = 1300
    let public DTWAIN_TN_TRANSFERTILEDONE = 1301
    let public DTWAIN_TN_FILECOMPRESSTYPEMISMATCH = 1302
    let public DTWAIN_PDFOCR_CLEANTEXT1 = 1
    let public DTWAIN_PDFOCR_CLEANTEXT2 = 2
    let public DTWAIN_MODAL = 0
    let public DTWAIN_MODELESS = 1
    let public DTWAIN_UIModeCLOSE = 0
    let public DTWAIN_UIModeOPEN = 1
    let public DTWAIN_REOPEN_SOURCE = 2
    let public DTWAIN_ROUNDNEAREST = 0
    let public DTWAIN_ROUNDUP = 1
    let public DTWAIN_ROUNDDOWN = 2
    let public DTWAIN_FLOATDELTA = (+1.0e-8)
    let public DTWAIN_OR_ROT0 = 0
    let public DTWAIN_OR_ROT90 = 1
    let public DTWAIN_OR_ROT180 = 2
    let public DTWAIN_OR_ROT270 = 3
    let public DTWAIN_OR_PORTRAIT = DTWAIN_OR_ROT0
    let public DTWAIN_OR_LANDSCAPE = DTWAIN_OR_ROT270
    let public DTWAIN_OR_ANYROTATION = (-1)
    let public DTWAIN_CO_GET = 0x0001
    let public DTWAIN_CO_SET = 0x0002
    let public DTWAIN_CO_GETDEFAULT = 0x0004
    let public DTWAIN_CO_GETCURRENT = 0x0008
    let public DTWAIN_CO_RESET = 0x0010
    let public DTWAIN_CO_SETCONSTRAINT = 0x0020
    let public DTWAIN_CO_CONSTRAINABLE = 0x0040
    let public DTWAIN_CO_GETHELP = 0x0100
    let public DTWAIN_CO_GETLABEL = 0x0200
    let public DTWAIN_CO_GETLABELENUM = 0x0400
    let public DTWAIN_CNTYAFGHANISTAN = 1001
    let public DTWAIN_CNTYALGERIA = 213
    let public DTWAIN_CNTYAMERICANSAMOA = 684
    let public DTWAIN_CNTYANDORRA = 33
    let public DTWAIN_CNTYANGOLA = 1002
    let public DTWAIN_CNTYANGUILLA = 8090
    let public DTWAIN_CNTYANTIGUA = 8091
    let public DTWAIN_CNTYARGENTINA = 54
    let public DTWAIN_CNTYARUBA = 297
    let public DTWAIN_CNTYASCENSIONI = 247
    let public DTWAIN_CNTYAUSTRALIA = 61
    let public DTWAIN_CNTYAUSTRIA = 43
    let public DTWAIN_CNTYBAHAMAS = 8092
    let public DTWAIN_CNTYBAHRAIN = 973
    let public DTWAIN_CNTYBANGLADESH = 880
    let public DTWAIN_CNTYBARBADOS = 8093
    let public DTWAIN_CNTYBELGIUM = 32
    let public DTWAIN_CNTYBELIZE = 501
    let public DTWAIN_CNTYBENIN = 229
    let public DTWAIN_CNTYBERMUDA = 8094
    let public DTWAIN_CNTYBHUTAN = 1003
    let public DTWAIN_CNTYBOLIVIA = 591
    let public DTWAIN_CNTYBOTSWANA = 267
    let public DTWAIN_CNTYBRITAIN = 6
    let public DTWAIN_CNTYBRITVIRGINIS = 8095
    let public DTWAIN_CNTYBRAZIL = 55
    let public DTWAIN_CNTYBRUNEI = 673
    let public DTWAIN_CNTYBULGARIA = 359
    let public DTWAIN_CNTYBURKINAFASO = 1004
    let public DTWAIN_CNTYBURMA = 1005
    let public DTWAIN_CNTYBURUNDI = 1006
    let public DTWAIN_CNTYCAMAROON = 237
    let public DTWAIN_CNTYCANADA = 2
    let public DTWAIN_CNTYCAPEVERDEIS = 238
    let public DTWAIN_CNTYCAYMANIS = 8096
    let public DTWAIN_CNTYCENTRALAFREP = 1007
    let public DTWAIN_CNTYCHAD = 1008
    let public DTWAIN_CNTYCHILE = 56
    let public DTWAIN_CNTYCHINA = 86
    let public DTWAIN_CNTYCHRISTMASIS = 1009
    let public DTWAIN_CNTYCOCOSIS = 1009
    let public DTWAIN_CNTYCOLOMBIA = 57
    let public DTWAIN_CNTYCOMOROS = 1010
    let public DTWAIN_CNTYCONGO = 1011
    let public DTWAIN_CNTYCOOKIS = 1012
    let public DTWAIN_CNTYCOSTARICA = 506
    let public DTWAIN_CNTYCUBA = 5
    let public DTWAIN_CNTYCYPRUS = 357
    let public DTWAIN_CNTYCZECHOSLOVAKIA = 42
    let public DTWAIN_CNTYDENMARK = 45
    let public DTWAIN_CNTYDJIBOUTI = 1013
    let public DTWAIN_CNTYDOMINICA = 8097
    let public DTWAIN_CNTYDOMINCANREP = 8098
    let public DTWAIN_CNTYEASTERIS = 1014
    let public DTWAIN_CNTYECUADOR = 593
    let public DTWAIN_CNTYEGYPT = 20
    let public DTWAIN_CNTYELSALVADOR = 503
    let public DTWAIN_CNTYEQGUINEA = 1015
    let public DTWAIN_CNTYETHIOPIA = 251
    let public DTWAIN_CNTYFALKLANDIS = 1016
    let public DTWAIN_CNTYFAEROEIS = 298
    let public DTWAIN_CNTYFIJIISLANDS = 679
    let public DTWAIN_CNTYFINLAND = 358
    let public DTWAIN_CNTYFRANCE = 33
    let public DTWAIN_CNTYFRANTILLES = 596
    let public DTWAIN_CNTYFRGUIANA = 594
    let public DTWAIN_CNTYFRPOLYNEISA = 689
    let public DTWAIN_CNTYFUTANAIS = 1043
    let public DTWAIN_CNTYGABON = 241
    let public DTWAIN_CNTYGAMBIA = 220
    let public DTWAIN_CNTYGERMANY = 49
    let public DTWAIN_CNTYGHANA = 233
    let public DTWAIN_CNTYGIBRALTER = 350
    let public DTWAIN_CNTYGREECE = 30
    let public DTWAIN_CNTYGREENLAND = 299
    let public DTWAIN_CNTYGRENADA = 8099
    let public DTWAIN_CNTYGRENEDINES = 8015
    let public DTWAIN_CNTYGUADELOUPE = 590
    let public DTWAIN_CNTYGUAM = 671
    let public DTWAIN_CNTYGUANTANAMOBAY = 5399
    let public DTWAIN_CNTYGUATEMALA = 502
    let public DTWAIN_CNTYGUINEA = 224
    let public DTWAIN_CNTYGUINEABISSAU = 1017
    let public DTWAIN_CNTYGUYANA = 592
    let public DTWAIN_CNTYHAITI = 509
    let public DTWAIN_CNTYHONDURAS = 504
    let public DTWAIN_CNTYHONGKONG = 852
    let public DTWAIN_CNTYHUNGARY = 36
    let public DTWAIN_CNTYICELAND = 354
    let public DTWAIN_CNTYINDIA = 91
    let public DTWAIN_CNTYINDONESIA = 62
    let public DTWAIN_CNTYIRAN = 98
    let public DTWAIN_CNTYIRAQ = 964
    let public DTWAIN_CNTYIRELAND = 353
    let public DTWAIN_CNTYISRAEL = 972
    let public DTWAIN_CNTYITALY = 39
    let public DTWAIN_CNTYIVORYCOAST = 225
    let public DTWAIN_CNTYJAMAICA = 8010
    let public DTWAIN_CNTYJAPAN = 81
    let public DTWAIN_CNTYJORDAN = 962
    let public DTWAIN_CNTYKENYA = 254
    let public DTWAIN_CNTYKIRIBATI = 1018
    let public DTWAIN_CNTYKOREA = 82
    let public DTWAIN_CNTYKUWAIT = 965
    let public DTWAIN_CNTYLAOS = 1019
    let public DTWAIN_CNTYLEBANON = 1020
    let public DTWAIN_CNTYLIBERIA = 231
    let public DTWAIN_CNTYLIBYA = 218
    let public DTWAIN_CNTYLIECHTENSTEIN = 41
    let public DTWAIN_CNTYLUXENBOURG = 352
    let public DTWAIN_CNTYMACAO = 853
    let public DTWAIN_CNTYMADAGASCAR = 1021
    let public DTWAIN_CNTYMALAWI = 265
    let public DTWAIN_CNTYMALAYSIA = 60
    let public DTWAIN_CNTYMALDIVES = 960
    let public DTWAIN_CNTYMALI = 1022
    let public DTWAIN_CNTYMALTA = 356
    let public DTWAIN_CNTYMARSHALLIS = 692
    let public DTWAIN_CNTYMAURITANIA = 1023
    let public DTWAIN_CNTYMAURITIUS = 230
    let public DTWAIN_CNTYMEXICO = 3
    let public DTWAIN_CNTYMICRONESIA = 691
    let public DTWAIN_CNTYMIQUELON = 508
    let public DTWAIN_CNTYMONACO = 33
    let public DTWAIN_CNTYMONGOLIA = 1024
    let public DTWAIN_CNTYMONTSERRAT = 8011
    let public DTWAIN_CNTYMOROCCO = 212
    let public DTWAIN_CNTYMOZAMBIQUE = 1025
    let public DTWAIN_CNTYNAMIBIA = 264
    let public DTWAIN_CNTYNAURU = 1026
    let public DTWAIN_CNTYNEPAL = 977
    let public DTWAIN_CNTYNETHERLANDS = 31
    let public DTWAIN_CNTYNETHANTILLES = 599
    let public DTWAIN_CNTYNEVIS = 8012
    let public DTWAIN_CNTYNEWCALEDONIA = 687
    let public DTWAIN_CNTYNEWZEALAND = 64
    let public DTWAIN_CNTYNICARAGUA = 505
    let public DTWAIN_CNTYNIGER = 227
    let public DTWAIN_CNTYNIGERIA = 234
    let public DTWAIN_CNTYNIUE = 1027
    let public DTWAIN_CNTYNORFOLKI = 1028
    let public DTWAIN_CNTYNORWAY = 47
    let public DTWAIN_CNTYOMAN = 968
    let public DTWAIN_CNTYPAKISTAN = 92
    let public DTWAIN_CNTYPALAU = 1029
    let public DTWAIN_CNTYPANAMA = 507
    let public DTWAIN_CNTYPARAGUAY = 595
    let public DTWAIN_CNTYPERU = 51
    let public DTWAIN_CNTYPHILLIPPINES = 63
    let public DTWAIN_CNTYPITCAIRNIS = 1030
    let public DTWAIN_CNTYPNEWGUINEA = 675
    let public DTWAIN_CNTYPOLAND = 48
    let public DTWAIN_CNTYPORTUGAL = 351
    let public DTWAIN_CNTYQATAR = 974
    let public DTWAIN_CNTYREUNIONI = 1031
    let public DTWAIN_CNTYROMANIA = 40
    let public DTWAIN_CNTYRWANDA = 250
    let public DTWAIN_CNTYSAIPAN = 670
    let public DTWAIN_CNTYSANMARINO = 39
    let public DTWAIN_CNTYSAOTOME = 1033
    let public DTWAIN_CNTYSAUDIARABIA = 966
    let public DTWAIN_CNTYSENEGAL = 221
    let public DTWAIN_CNTYSEYCHELLESIS = 1034
    let public DTWAIN_CNTYSIERRALEONE = 1035
    let public DTWAIN_CNTYSINGAPORE = 65
    let public DTWAIN_CNTYSOLOMONIS = 1036
    let public DTWAIN_CNTYSOMALI = 1037
    let public DTWAIN_CNTYSOUTHAFRICA = 27
    let public DTWAIN_CNTYSPAIN = 34
    let public DTWAIN_CNTYSRILANKA = 94
    let public DTWAIN_CNTYSTHELENA = 1032
    let public DTWAIN_CNTYSTKITTS = 8013
    let public DTWAIN_CNTYSTLUCIA = 8014
    let public DTWAIN_CNTYSTPIERRE = 508
    let public DTWAIN_CNTYSTVINCENT = 8015
    let public DTWAIN_CNTYSUDAN = 1038
    let public DTWAIN_CNTYSURINAME = 597
    let public DTWAIN_CNTYSWAZILAND = 268
    let public DTWAIN_CNTYSWEDEN = 46
    let public DTWAIN_CNTYSWITZERLAND = 41
    let public DTWAIN_CNTYSYRIA = 1039
    let public DTWAIN_CNTYTAIWAN = 886
    let public DTWAIN_CNTYTANZANIA = 255
    let public DTWAIN_CNTYTHAILAND = 66
    let public DTWAIN_CNTYTOBAGO = 8016
    let public DTWAIN_CNTYTOGO = 228
    let public DTWAIN_CNTYTONGAIS = 676
    let public DTWAIN_CNTYTRINIDAD = 8016
    let public DTWAIN_CNTYTUNISIA = 216
    let public DTWAIN_CNTYTURKEY = 90
    let public DTWAIN_CNTYTURKSCAICOS = 8017
    let public DTWAIN_CNTYTUVALU = 1040
    let public DTWAIN_CNTYUGANDA = 256
    let public DTWAIN_CNTYUSSR = 7
    let public DTWAIN_CNTYUAEMIRATES = 971
    let public DTWAIN_CNTYUNITEDKINGDOM = 44
    let public DTWAIN_CNTYUSA = 1
    let public DTWAIN_CNTYURUGUAY = 598
    let public DTWAIN_CNTYVANUATU = 1041
    let public DTWAIN_CNTYVATICANCITY = 39
    let public DTWAIN_CNTYVENEZUELA = 58
    let public DTWAIN_CNTYWAKE = 1042
    let public DTWAIN_CNTYWALLISIS = 1043
    let public DTWAIN_CNTYWESTERNSAHARA = 1044
    let public DTWAIN_CNTYWESTERNSAMOA = 1045
    let public DTWAIN_CNTYYEMEN = 1046
    let public DTWAIN_CNTYYUGOSLAVIA = 38
    let public DTWAIN_CNTYZAIRE = 243
    let public DTWAIN_CNTYZAMBIA = 260
    let public DTWAIN_CNTYZIMBABWE = 263
    let public DTWAIN_LANGDANISH = 0
    let public DTWAIN_LANGDUTCH = 1
    let public DTWAIN_LANGINTERNATIONALENGLISH = 2
    let public DTWAIN_LANGFRENCHCANADIAN = 3
    let public DTWAIN_LANGFINNISH = 4
    let public DTWAIN_LANGFRENCH = 5
    let public DTWAIN_LANGGERMAN = 6
    let public DTWAIN_LANGICELANDIC = 7
    let public DTWAIN_LANGITALIAN = 8
    let public DTWAIN_LANGNORWEGIAN = 9
    let public DTWAIN_LANGPORTUGUESE = 10
    let public DTWAIN_LANGSPANISH = 11
    let public DTWAIN_LANGSWEDISH = 12
    let public DTWAIN_LANGUSAENGLISH = 13
    let public DTWAIN_NO_ERROR = (0)
    let public DTWAIN_ERR_FIRST = (-1000)
    let public DTWAIN_ERR_BAD_HANDLE = (-1001)
    let public DTWAIN_ERR_BAD_SOURCE = (-1002)
    let public DTWAIN_ERR_BAD_ARRAY = (-1003)
    let public DTWAIN_ERR_WRONG_ARRAY_TYPE = (-1004)
    let public DTWAIN_ERR_INDEX_BOUNDS = (-1005)
    let public DTWAIN_ERR_OUT_OF_MEMORY = (-1006)
    let public DTWAIN_ERR_NULL_WINDOW = (-1007)
    let public DTWAIN_ERR_BAD_PIXTYPE = (-1008)
    let public DTWAIN_ERR_BAD_CONTAINER = (-1009)
    let public DTWAIN_ERR_NO_SESSION = (-1010)
    let public DTWAIN_ERR_BAD_ACQUIRE_NUM = (-1011)
    let public DTWAIN_ERR_BAD_CAP = (-1012)
    let public DTWAIN_ERR_CAP_NO_SUPPORT = (-1013)
    let public DTWAIN_ERR_TWAIN = (-1014)
    let public DTWAIN_ERR_HOOK_FAILED = (-1015)
    let public DTWAIN_ERR_BAD_FILENAME = (-1016)
    let public DTWAIN_ERR_EMPTY_ARRAY = (-1017)
    let public DTWAIN_ERR_FILE_FORMAT = (-1018)
    let public DTWAIN_ERR_BAD_DIB_PAGE = (-1019)
    let public DTWAIN_ERR_SOURCE_ACQUIRING = (-1020)
    let public DTWAIN_ERR_INVALID_PARAM = (-1021)
    let public DTWAIN_ERR_INVALID_RANGE = (-1022)
    let public DTWAIN_ERR_UI_ERROR = (-1023)
    let public DTWAIN_ERR_BAD_UNIT = (-1024)
    let public DTWAIN_ERR_LANGDLL_NOT_FOUND = (-1025)
    let public DTWAIN_ERR_SOURCE_NOT_OPEN = (-1026)
    let public DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED = (-1027)
    let public DTWAIN_ERR_UIONLY_NOT_SUPPORTED = (-1028)
    let public DTWAIN_ERR_UI_ALREADY_OPENED = (-1029)
    let public DTWAIN_ERR_CAPSET_NOSUPPORT = (-1030)
    let public DTWAIN_ERR_NO_FILE_XFER = (-1031)
    let public DTWAIN_ERR_INVALID_BITDEPTH = (-1032)
    let public DTWAIN_ERR_NO_CAPS_DEFINED = (-1033)
    let public DTWAIN_ERR_TILES_NOT_SUPPORTED = (-1034)
    let public DTWAIN_ERR_INVALID_DTWAIN_FRAME = (-1035)
    let public DTWAIN_ERR_LIMITED_VERSION = (-1036)
    let public DTWAIN_ERR_NO_FEEDER = (-1037)
    let public DTWAIN_ERR_NO_FEEDER_QUERY = (-1038)
    let public DTWAIN_ERR_EXCEPTION_ERROR = (-1039)
    let public DTWAIN_ERR_INVALID_STATE = (-1040)
    let public DTWAIN_ERR_UNSUPPORTED_EXTINFO = (-1041)
    let public DTWAIN_ERR_DLLRESOURCE_NOTFOUND = (-1042)
    let public DTWAIN_ERR_NOT_INITIALIZED = (-1043)
    let public DTWAIN_ERR_NO_SOURCES = (-1044)
    let public DTWAIN_ERR_TWAIN_NOT_INSTALLED = (-1045)
    let public DTWAIN_ERR_WRONG_THREAD = (-1046)
    let public DTWAIN_ERR_BAD_CAPTYPE = (-1047)
    let public DTWAIN_ERR_UNKNOWN_CAPDATATYPE = (-1048)
    let public DTWAIN_ERR_DEMO_NOFILETYPE = (-1049)
    let public DTWAIN_ERR_SOURCESELECTION_CANCELED = (-1050)
    let public DTWAIN_ERR_RESOURCES_NOT_FOUND = (-1051)
    let public DTWAIN_ERR_STRINGTYPE_MISMATCH = (-1052)
    let public DTWAIN_ERR_ARRAYTYPE_MISMATCH = (-1053)
    let public DTWAIN_ERR_SOURCENAME_NOTINSTALLED = (-1054)
    let public DTWAIN_ERR_NO_MEMFILE_XFER = (-1055)
    let public DTWAIN_ERR_AREA_ARRAY_TOO_SMALL = (-1056)
    let public DTWAIN_ERR_LOG_CREATE_ERROR = (-1057)
    let public DTWAIN_ERR_FILESYSTEM_NOT_SUPPORTED = (-1058)
    let public DTWAIN_ERR_TILEMODE_NOTSET = (-1059)
    let public DTWAIN_ERR_INI32_NOT_FOUND = (-1060)
    let public DTWAIN_ERR_INI64_NOT_FOUND = (-1061)
    let public DTWAIN_ERR_CRC_CHECK = (-1062)
    let public DTWAIN_ERR_RESOURCES_BAD_VERSION = (-1063)
    let public DTWAIN_ERR_WIN32_ERROR = (-1064)
    let public DTWAIN_ERR_STRINGID_NOTFOUND = (-1065)
    let public DTWAIN_ERR_RESOURCES_DUPLICATEID_FOUND = (-1066)
    let public DTWAIN_ERR_UNAVAILABLE_EXTINFO = (-1067)
    let public DTWAIN_ERR_TWAINDSM2_BADBITMAP = (-1068)
    let public DTWAIN_ERR_ACQUISITION_CANCELED = (-1069)
    let public DTWAIN_ERR_IMAGE_RESAMPLED = (-1070)
    let public DTWAIN_ERR_UNKNOWN_TWAIN_RC = (-1071)
    let public DTWAIN_ERR_UNKNOWN_TWAIN_CC = (-1072)
    let public DTWAIN_ERR_RESOURCES_DATA_EXCEPTION = (-1073)
    let public DTWAIN_ERR_AUDIO_TRANSFER_NOTSUPPORTED = (-1074)
    let public DTWAIN_ERR_FEEDER_COMPLIANCY = (-1075)
    let public DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY1 = (-1076)
    let public DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY2 = (-1077)
    let public DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY1 = (-1078)
    let public DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY2 = (-1079)
    let public DTWAIN_ERR_ICAPBITDEPTH_COMPLIANCY1 = (-1080)
    let public DTWAIN_ERR_XFERMECH_COMPLIANCY = (-1081)
    let public DTWAIN_ERR_STANDARDCAPS_COMPLIANCY = (-1082)
    let public DTWAIN_ERR_EXTIMAGEINFO_DATATYPE_MISMATCH = (-1083)
    let public DTWAIN_ERR_EXTIMAGEINFO_RETRIEVAL = (-1084)
    let public DTWAIN_ERR_RANGE_OUTOFBOUNDS = (-1085)
    let public DTWAIN_ERR_RANGE_STEPISZERO = (-1086)
    let public DTWAIN_ERR_BLANKNAMEDETECTED = (-1087)
    let public DTWAIN_ERR_FEEDER_NOPAPERSENSOR = (-1088)
    let public TWAIN_ERR_LOW_MEMORY = (-1100)
    let public TWAIN_ERR_FALSE_ALARM = (-1101)
    let public TWAIN_ERR_BUMMER = (-1102)
    let public TWAIN_ERR_NODATASOURCE = (-1103)
    let public TWAIN_ERR_MAXCONNECTIONS = (-1104)
    let public TWAIN_ERR_OPERATIONERROR = (-1105)
    let public TWAIN_ERR_BADCAPABILITY = (-1106)
    let public TWAIN_ERR_BADVALUE = (-1107)
    let public TWAIN_ERR_BADPROTOCOL = (-1108)
    let public TWAIN_ERR_SEQUENCEERROR = (-1109)
    let public TWAIN_ERR_BADDESTINATION = (-1110)
    let public TWAIN_ERR_CAPNOTSUPPORTED = (-1111)
    let public TWAIN_ERR_CAPBADOPERATION = (-1112)
    let public TWAIN_ERR_CAPSEQUENCEERROR = (-1113)
    let public TWAIN_ERR_FILEPROTECTEDERROR = (-1114)
    let public TWAIN_ERR_FILEEXISTERROR = (-1115)
    let public TWAIN_ERR_FILENOTFOUND = (-1116)
    let public TWAIN_ERR_DIRNOTEMPTY = (-1117)
    let public TWAIN_ERR_FEEDERJAMMED = (-1118)
    let public TWAIN_ERR_FEEDERMULTPAGES = (-1119)
    let public TWAIN_ERR_FEEDERWRITEERROR = (-1120)
    let public TWAIN_ERR_DEVICEOFFLINE = (-1121)
    let public TWAIN_ERR_NULL_CONTAINER = (-1122)
    let public TWAIN_ERR_INTERLOCK = (-1123)
    let public TWAIN_ERR_DAMAGEDCORNER = (-1124)
    let public TWAIN_ERR_FOCUSERROR = (-1125)
    let public TWAIN_ERR_DOCTOOLIGHT = (-1126)
    let public TWAIN_ERR_DOCTOODARK = (-1127)
    let public TWAIN_ERR_NOMEDIA = (-1128)
    let public DTWAIN_ERR_FILEXFERSTART = (-2000)
    let public DTWAIN_ERR_MEM = (-2001)
    let public DTWAIN_ERR_FILEOPEN = (-2002)
    let public DTWAIN_ERR_FILEREAD = (-2003)
    let public DTWAIN_ERR_FILEWRITE = (-2004)
    let public DTWAIN_ERR_BADPARAM = (-2005)
    let public DTWAIN_ERR_INVALIDBMP = (-2006)
    let public DTWAIN_ERR_BMPRLE = (-2007)
    let public DTWAIN_ERR_RESERVED1 = (-2008)
    let public DTWAIN_ERR_INVALIDJPG = (-2009)
    let public DTWAIN_ERR_DC = (-2010)
    let public DTWAIN_ERR_DIB = (-2011)
    let public DTWAIN_ERR_RESERVED2 = (-2012)
    let public DTWAIN_ERR_NORESOURCE = (-2013)
    let public DTWAIN_ERR_CALLBACKCANCEL = (-2014)
    let public DTWAIN_ERR_INVALIDPNG = (-2015)
    let public DTWAIN_ERR_PNGCREATE = (-2016)
    let public DTWAIN_ERR_INTERNAL = (-2017)
    let public DTWAIN_ERR_FONT = (-2018)
    let public DTWAIN_ERR_INTTIFF = (-2019)
    let public DTWAIN_ERR_INVALIDTIFF = (-2020)
    let public DTWAIN_ERR_NOTIFFLZW = (-2021)
    let public DTWAIN_ERR_INVALIDPCX = (-2022)
    let public DTWAIN_ERR_CREATEBMP = (-2023)
    let public DTWAIN_ERR_NOLINES = (-2024)
    let public DTWAIN_ERR_GETDIB = (-2025)
    let public DTWAIN_ERR_NODEVOP = (-2026)
    let public DTWAIN_ERR_INVALIDWMF = (-2027)
    let public DTWAIN_ERR_DEPTHMISMATCH = (-2028)
    let public DTWAIN_ERR_BITBLT = (-2029)
    let public DTWAIN_ERR_BUFTOOSMALL = (-2030)
    let public DTWAIN_ERR_TOOMANYCOLORS = (-2031)
    let public DTWAIN_ERR_INVALIDTGA = (-2032)
    let public DTWAIN_ERR_NOTGATHUMBNAIL = (-2033)
    let public DTWAIN_ERR_RESERVED3 = (-2034)
    let public DTWAIN_ERR_CREATEDIB = (-2035)
    let public DTWAIN_ERR_NOLZW = (-2036)
    let public DTWAIN_ERR_SELECTOBJ = (-2037)
    let public DTWAIN_ERR_BADMANAGER = (-2038)
    let public DTWAIN_ERR_OBSOLETE = (-2039)
    let public DTWAIN_ERR_CREATEDIBSECTION = (-2040)
    let public DTWAIN_ERR_SETWINMETAFILEBITS = (-2041)
    let public DTWAIN_ERR_GETWINMETAFILEBITS = (-2042)
    let public DTWAIN_ERR_PAXPWD = (-2043)
    let public DTWAIN_ERR_INVALIDPAX = (-2044)
    let public DTWAIN_ERR_NOSUPPORT = (-2045)
    let public DTWAIN_ERR_INVALIDPSD = (-2046)
    let public DTWAIN_ERR_PSDNOTSUPPORTED = (-2047)
    let public DTWAIN_ERR_DECRYPT = (-2048)
    let public DTWAIN_ERR_ENCRYPT = (-2049)
    let public DTWAIN_ERR_COMPRESSION = (-2050)
    let public DTWAIN_ERR_DECOMPRESSION = (-2051)
    let public DTWAIN_ERR_INVALIDTLA = (-2052)
    let public DTWAIN_ERR_INVALIDWBMP = (-2053)
    let public DTWAIN_ERR_NOTIFFTAG = (-2054)
    let public DTWAIN_ERR_NOLOCALSTORAGE = (-2055)
    let public DTWAIN_ERR_INVALIDEXIF = (-2056)
    let public DTWAIN_ERR_NOEXIFSTRING = (-2057)
    let public DTWAIN_ERR_TIFFDLL32NOTFOUND = (-2058)
    let public DTWAIN_ERR_TIFFDLL16NOTFOUND = (-2059)
    let public DTWAIN_ERR_PNGDLL16NOTFOUND = (-2060)
    let public DTWAIN_ERR_JPEGDLL16NOTFOUND = (-2061)
    let public DTWAIN_ERR_BADBITSPERPIXEL = (-2062)
    let public DTWAIN_ERR_TIFFDLL32INVALIDVER = (-2063)
    let public DTWAIN_ERR_PDFDLL32NOTFOUND = (-2064)
    let public DTWAIN_ERR_PDFDLL32INVALIDVER = (-2065)
    let public DTWAIN_ERR_JPEGDLL32NOTFOUND = (-2066)
    let public DTWAIN_ERR_JPEGDLL32INVALIDVER = (-2067)
    let public DTWAIN_ERR_PNGDLL32NOTFOUND = (-2068)
    let public DTWAIN_ERR_PNGDLL32INVALIDVER = (-2069)
    let public DTWAIN_ERR_J2KDLL32NOTFOUND = (-2070)
    let public DTWAIN_ERR_J2KDLL32INVALIDVER = (-2071)
    let public DTWAIN_ERR_MANDUPLEX_UNAVAILABLE = (-2072)
    let public DTWAIN_ERR_TIMEOUT = (-2073)
    let public DTWAIN_ERR_INVALIDICONFORMAT = (-2074)
    let public DTWAIN_ERR_TWAIN32DSMNOTFOUND = (-2075)
    let public DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND = (-2076)
    let public DTWAIN_ERR_INVALID_DIRECTORY = (-2077)
    let public DTWAIN_ERR_CREATE_DIRECTORY = (-2078)
    let public DTWAIN_ERR_OCRLIBRARY_NOTFOUND = (-2079)
    let public DTWAIN_TWAINSAVE_OK = (0)
    let public DTWAIN_ERR_TS_FIRST = (-2080)
    let public DTWAIN_ERR_TS_NOFILENAME = (-2081)
    let public DTWAIN_ERR_TS_NOTWAINSYS = (-2082)
    let public DTWAIN_ERR_TS_DEVICEFAILURE = (-2083)
    let public DTWAIN_ERR_TS_FILESAVEERROR = (-2084)
    let public DTWAIN_ERR_TS_COMMANDILLEGAL = (-2085)
    let public DTWAIN_ERR_TS_CANCELLED = (-2086)
    let public DTWAIN_ERR_TS_ACQUISITIONERROR = (-2087)
    let public DTWAIN_ERR_TS_INVALIDCOLORSPACE = (-2088)
    let public DTWAIN_ERR_TS_PDFNOTSUPPORTED = (-2089)
    let public DTWAIN_ERR_TS_NOTAVAILABLE = (-2090)
    let public DTWAIN_ERR_OCR_FIRST = (-2100)
    let public DTWAIN_ERR_OCR_INVALIDPAGENUM = (-2101)
    let public DTWAIN_ERR_OCR_INVALIDENGINE = (-2102)
    let public DTWAIN_ERR_OCR_NOTACTIVE = (-2103)
    let public DTWAIN_ERR_OCR_INVALIDFILETYPE = (-2104)
    let public DTWAIN_ERR_OCR_INVALIDPIXELTYPE = (-2105)
    let public DTWAIN_ERR_OCR_INVALIDBITDEPTH = (-2106)
    let public DTWAIN_ERR_OCR_RECOGNITIONERROR = (-2107)
    let public DTWAIN_ERR_OCR_LAST = (-2108)
    let public DTWAIN_ERR_LAST = DTWAIN_ERR_OCR_LAST
    let public DTWAIN_ERR_SOURCE_COULD_NOT_OPEN = (-2500)
    let public DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE = (-2501)
    let public DTWAIN_ERR_IMAGEINFO_INVALID = (-2502)
    let public DTWAIN_ERR_WRITEDATA_TOFILE = (-2503)
    let public DTWAIN_ERR_OPERATION_NOTSUPPORTED = (-2504)
    let public DTWAIN_DE_CHKAUTOCAPTURE = 1
    let public DTWAIN_DE_CHKBATTERY = 2
    let public DTWAIN_DE_CHKDEVICEONLINE = 4
    let public DTWAIN_DE_CHKFLASH = 8
    let public DTWAIN_DE_CHKPOWERSUPPLY = 16
    let public DTWAIN_DE_CHKRESOLUTION = 32
    let public DTWAIN_DE_DEVICEADDED = 64
    let public DTWAIN_DE_DEVICEOFFLINE = 128
    let public DTWAIN_DE_DEVICEREADY = 256
    let public DTWAIN_DE_DEVICEREMOVED = 512
    let public DTWAIN_DE_IMAGECAPTURED = 1024
    let public DTWAIN_DE_IMAGEDELETED = 2048
    let public DTWAIN_DE_PAPERDOUBLEFEED = 4096
    let public DTWAIN_DE_PAPERJAM = 8192
    let public DTWAIN_DE_LAMPFAILURE = 16384
    let public DTWAIN_DE_POWERSAVE = 32768
    let public DTWAIN_DE_POWERSAVENOTIFY = 65536
    let public DTWAIN_DE_CUSTOMEVENTS = 0x8000
    let public DTWAIN_GETDE_EVENT = 0
    let public DTWAIN_GETDE_DEVNAME = 1
    let public DTWAIN_GETDE_BATTERYMINUTES = 2
    let public DTWAIN_GETDE_BATTERYPCT = 3
    let public DTWAIN_GETDE_XRESOLUTION = 4
    let public DTWAIN_GETDE_YRESOLUTION = 5
    let public DTWAIN_GETDE_FLASHUSED = 6
    let public DTWAIN_GETDE_AUTOCAPTURE = 7
    let public DTWAIN_GETDE_TIMEBEFORECAPTURE = 8
    let public DTWAIN_GETDE_TIMEBETWEENCAPTURES = 9
    let public DTWAIN_GETDE_POWERSUPPLY = 10
    let public DTWAIN_IMPRINTERTOPBEFORE = 1
    let public DTWAIN_IMPRINTERTOPAFTER = 2
    let public DTWAIN_IMPRINTERBOTTOMBEFORE = 4
    let public DTWAIN_IMPRINTERBOTTOMAFTER = 8
    let public DTWAIN_ENDORSERTOPBEFORE = 16
    let public DTWAIN_ENDORSERTOPAFTER = 32
    let public DTWAIN_ENDORSERBOTTOMBEFORE = 64
    let public DTWAIN_ENDORSERBOTTOMAFTER = 128
    let public DTWAIN_PM_SINGLESTRING = 0
    let public DTWAIN_PM_MULTISTRING = 1
    let public DTWAIN_PM_COMPOUNDSTRING = 2
    let public DTWAIN_TWTY_INT8 = 0x0000
    let public DTWAIN_TWTY_INT16 = 0x0001
    let public DTWAIN_TWTY_INT32 = 0x0002
    let public DTWAIN_TWTY_UINT8 = 0x0003
    let public DTWAIN_TWTY_UINT16 = 0x0004
    let public DTWAIN_TWTY_UINT32 = 0x0005
    let public DTWAIN_TWTY_BOOL = 0x0006
    let public DTWAIN_TWTY_FIX32 = 0x0007
    let public DTWAIN_TWTY_FRAME = 0x0008
    let public DTWAIN_TWTY_STR32 = 0x0009
    let public DTWAIN_TWTY_STR64 = 0x000A
    let public DTWAIN_TWTY_STR128 = 0x000B
    let public DTWAIN_TWTY_STR255 = 0x000C
    let public DTWAIN_TWTY_STR1024 = 0x000D
    let public DTWAIN_TWTY_UNI512 = 0x000E
    let public DTWAIN_EI_BARCODEX = 0x1200
    let public DTWAIN_EI_BARCODEY = 0x1201
    let public DTWAIN_EI_BARCODETEXT = 0x1202
    let public DTWAIN_EI_BARCODETYPE = 0x1203
    let public DTWAIN_EI_DESHADETOP = 0x1204
    let public DTWAIN_EI_DESHADELEFT = 0x1205
    let public DTWAIN_EI_DESHADEHEIGHT = 0x1206
    let public DTWAIN_EI_DESHADEWIDTH = 0x1207
    let public DTWAIN_EI_DESHADESIZE = 0x1208
    let public DTWAIN_EI_SPECKLESREMOVED = 0x1209
    let public DTWAIN_EI_HORZLINEXCOORD = 0x120A
    let public DTWAIN_EI_HORZLINEYCOORD = 0x120B
    let public DTWAIN_EI_HORZLINELENGTH = 0x120C
    let public DTWAIN_EI_HORZLINETHICKNESS = 0x120D
    let public DTWAIN_EI_VERTLINEXCOORD = 0x120E
    let public DTWAIN_EI_VERTLINEYCOORD = 0x120F
    let public DTWAIN_EI_VERTLINELENGTH = 0x1210
    let public DTWAIN_EI_VERTLINETHICKNESS = 0x1211
    let public DTWAIN_EI_PATCHCODE = 0x1212
    let public DTWAIN_EI_ENDORSEDTEXT = 0x1213
    let public DTWAIN_EI_FORMCONFIDENCE = 0x1214
    let public DTWAIN_EI_FORMTEMPLATEMATCH = 0x1215
    let public DTWAIN_EI_FORMTEMPLATEPAGEMATCH = 0x1216
    let public DTWAIN_EI_FORMHORZDOCOFFSET = 0x1217
    let public DTWAIN_EI_FORMVERTDOCOFFSET = 0x1218
    let public DTWAIN_EI_BARCODECOUNT = 0x1219
    let public DTWAIN_EI_BARCODECONFIDENCE = 0x121A
    let public DTWAIN_EI_BARCODEROTATION = 0x121B
    let public DTWAIN_EI_BARCODETEXTLENGTH = 0x121C
    let public DTWAIN_EI_DESHADECOUNT = 0x121D
    let public DTWAIN_EI_DESHADEBLACKCOUNTOLD = 0x121E
    let public DTWAIN_EI_DESHADEBLACKCOUNTNEW = 0x121F
    let public DTWAIN_EI_DESHADEBLACKRLMIN = 0x1220
    let public DTWAIN_EI_DESHADEBLACKRLMAX = 0x1221
    let public DTWAIN_EI_DESHADEWHITECOUNTOLD = 0x1222
    let public DTWAIN_EI_DESHADEWHITECOUNTNEW = 0x1223
    let public DTWAIN_EI_DESHADEWHITERLMIN = 0x1224
    let public DTWAIN_EI_DESHADEWHITERLAVE = 0x1225
    let public DTWAIN_EI_DESHADEWHITERLMAX = 0x1226
    let public DTWAIN_EI_BLACKSPECKLESREMOVED = 0x1227
    let public DTWAIN_EI_WHITESPECKLESREMOVED = 0x1228
    let public DTWAIN_EI_HORZLINECOUNT = 0x1229
    let public DTWAIN_EI_VERTLINECOUNT = 0x122A
    let public DTWAIN_EI_DESKEWSTATUS = 0x122B
    let public DTWAIN_EI_SKEWORIGINALANGLE = 0x122C
    let public DTWAIN_EI_SKEWFINALANGLE = 0x122D
    let public DTWAIN_EI_SKEWCONFIDENCE = 0x122E
    let public DTWAIN_EI_SKEWWINDOWX1 = 0x122F
    let public DTWAIN_EI_SKEWWINDOWY1 = 0x1230
    let public DTWAIN_EI_SKEWWINDOWX2 = 0x1231
    let public DTWAIN_EI_SKEWWINDOWY2 = 0x1232
    let public DTWAIN_EI_SKEWWINDOWX3 = 0x1233
    let public DTWAIN_EI_SKEWWINDOWY3 = 0x1234
    let public DTWAIN_EI_SKEWWINDOWX4 = 0x1235
    let public DTWAIN_EI_SKEWWINDOWY4 = 0x1236
    let public DTWAIN_EI_BOOKNAME = 0x1238
    let public DTWAIN_EI_CHAPTERNUMBER = 0x1239
    let public DTWAIN_EI_DOCUMENTNUMBER = 0x123A
    let public DTWAIN_EI_PAGENUMBER = 0x123B
    let public DTWAIN_EI_CAMERA = 0x123C
    let public DTWAIN_EI_FRAMENUMBER = 0x123D
    let public DTWAIN_EI_FRAME = 0x123E
    let public DTWAIN_EI_PIXELFLAVOR = 0x123F
    let public DTWAIN_EI_ICCPROFILE = 0x1240
    let public DTWAIN_EI_LASTSEGMENT = 0x1241
    let public DTWAIN_EI_SEGMENTNUMBER = 0x1242
    let public DTWAIN_EI_MAGDATA = 0x1243
    let public DTWAIN_EI_MAGTYPE = 0x1244
    let public DTWAIN_EI_PAGESIDE = 0x1245
    let public DTWAIN_EI_FILESYSTEMSOURCE = 0x1246
    let public DTWAIN_EI_IMAGEMERGED = 0x1247
    let public DTWAIN_EI_MAGDATALENGTH = 0x1248
    let public DTWAIN_EI_PAPERCOUNT = 0x1249
    let public DTWAIN_EI_PRINTERTEXT = 0x124A
    let public DTWAIN_EI_TWAINDIRECTMETADATA = 0x124B
    let public DTWAIN_EI_IAFIELDA_VALUE = 0x124C
    let public DTWAIN_EI_IAFIELDB_VALUE = 0x124D
    let public DTWAIN_EI_IAFIELDC_VALUE = 0x124E
    let public DTWAIN_EI_IAFIELDD_VALUE = 0x124F
    let public DTWAIN_EI_IAFIELDE_VALUE = 0x1250
    let public DTWAIN_EI_IALEVEL = 0x1251
    let public DTWAIN_EI_PRINTER = 0x1252
    let public DTWAIN_EI_BARCODETEXT2 = 0x1253
    let public DTWAIN_LOG_DECODE_SOURCE = 0x1
    let public DTWAIN_LOG_DECODE_DEST = 0x2
    let public DTWAIN_LOG_DECODE_TWMEMREF = 0x4
    let public DTWAIN_LOG_DECODE_TWEVENT = 0x8
    let public DTWAIN_LOG_CALLSTACK = 0x10
    let public DTWAIN_LOG_ISTWAINMSG = 0x20
    let public DTWAIN_LOG_INITFAILURE = 0x40
    let public DTWAIN_LOG_LOWLEVELTWAIN = 0x80
    let public DTWAIN_LOG_DECODE_BITMAP = 0x100
    let public DTWAIN_LOG_NOTIFICATIONS = 0x200
    let public DTWAIN_LOG_MISCELLANEOUS = 0x400
    let public DTWAIN_LOG_DTWAINERRORS = 0x800
    let public DTWAIN_LOG_USEFILE = 0x10000
    let public DTWAIN_LOG_SHOWEXCEPTIONS = 0x20000
    let public DTWAIN_LOG_ERRORMSGBOX = 0x40000
    let public DTWAIN_LOG_USEBUFFER = 0x80000
    let public DTWAIN_LOG_FILEAPPEND = 0x100000
    let public DTWAIN_LOG_USECALLBACK = 0x200000
    let public DTWAIN_LOG_USECRLF = 0x400000
    let public DTWAIN_LOG_CONSOLE = 0x800000
    let public DTWAIN_LOG_DEBUGMONITOR = 0x1000000
    let public DTWAIN_LOG_USEWINDOW = 0x2000000
    let public DTWAIN_LOG_CREATEDIRECTORY = 0x04000000
    let public DTWAIN_LOG_CONSOLEWITHHANDLER = (0x08000000 ||| DTWAIN_LOG_CONSOLE)
    let public DTWAIN_LOG_ALL = (DTWAIN_LOG_DECODE_SOURCE ||| DTWAIN_LOG_DECODE_DEST ||| DTWAIN_LOG_DECODE_TWEVENT ||| DTWAIN_LOG_DECODE_TWMEMREF ||| DTWAIN_LOG_CALLSTACK ||| DTWAIN_LOG_ISTWAINMSG ||| DTWAIN_LOG_INITFAILURE ||| DTWAIN_LOG_LOWLEVELTWAIN ||| DTWAIN_LOG_NOTIFICATIONS ||| DTWAIN_LOG_MISCELLANEOUS ||| DTWAIN_LOG_DTWAINERRORS ||| DTWAIN_LOG_DECODE_BITMAP)
    let public DTWAIN_LOG_ALL_APPEND = 0xFFFFFFFF
    let public DTWAIN_TEMPDIR_CREATEDIRECTORY = DTWAIN_LOG_CREATEDIRECTORY
    let public DTWAINGCD_RETURNHANDLE = 1
    let public DTWAINGCD_COPYDATA = 2
    let public DTWAIN_BYPOSITION = 0
    let public DTWAIN_BYID = 1
    let public DTWAINSCD_USEHANDLE = 1
    let public DTWAINSCD_USEDATA = 2
    let public DTWAIN_PAGEFAIL_RETRY = 1
    let public DTWAIN_PAGEFAIL_TERMINATE = 2
    let public DTWAIN_MAXRETRY_ATTEMPTS = 3
    let public DTWAIN_RETRY_FOREVER = (-1)
    let public DTWAIN_PDF_NOSCALING = 128
    let public DTWAIN_PDF_FITPAGE = 256
    let public DTWAIN_PDF_VARIABLEPAGESIZE = 512
    let public DTWAIN_PDF_CUSTOMSIZE = 1024
    let public DTWAIN_PDF_USECOMPRESSION = 2048
    let public DTWAIN_PDF_CUSTOMSCALE = 4096
    let public DTWAIN_PDF_PIXELSPERMETERSIZE = 8192
    let public DTWAIN_PDF_ALLOWPRINTING = 2052
    let public DTWAIN_PDF_ALLOWMOD = 8
    let public DTWAIN_PDF_ALLOWCOPY = 16
    let public DTWAIN_PDF_ALLOWMODANNOTATIONS = 32
    let public DTWAIN_PDF_ALLOWFILLIN = 256
    let public DTWAIN_PDF_ALLOWEXTRACTION = 512
    let public DTWAIN_PDF_ALLOWASSEMBLY = 1024
    let public DTWAIN_PDF_ALLOWDEGRADEDPRINTING = 4
    let public DTWAIN_PDF_ALLOWALL = 0xFFFFFFFC
    let public DTWAIN_PDF_ALLOWANYMOD = (DTWAIN_PDF_ALLOWMOD ||| DTWAIN_PDF_ALLOWFILLIN ||| DTWAIN_PDF_ALLOWMODANNOTATIONS ||| DTWAIN_PDF_ALLOWASSEMBLY)
    let public DTWAIN_PDF_ALLOWANYPRINTING = (DTWAIN_PDF_ALLOWPRINTING ||| DTWAIN_PDF_ALLOWDEGRADEDPRINTING)
    let public DTWAIN_PDF_PORTRAIT = 0
    let public DTWAIN_PDF_LANDSCAPE = 1
    let public DTWAIN_PS_REGULAR = 0
    let public DTWAIN_PS_ENCAPSULATED = 1
    let public DTWAIN_BP_AUTODISCARD_NONE = 0
    let public DTWAIN_BP_AUTODISCARD_IMMEDIATE = 1
    let public DTWAIN_BP_AUTODISCARD_AFTERPROCESS = 2
    let public DTWAIN_BP_DETECTORIGINAL = 1
    let public DTWAIN_BP_DETECTADJUSTED = 2
    let public DTWAIN_BP_DETECTALL = (DTWAIN_BP_DETECTORIGINAL ||| DTWAIN_BP_DETECTADJUSTED)
    let public DTWAIN_BP_DISABLE = (-2)
    let public DTWAIN_BP_AUTO = (-1)
    let public DTWAIN_BP_AUTODISCARD_ANY = 0xFFFF
    let public DTWAIN_LP_REFLECTIVE = 0
    let public DTWAIN_LP_TRANSMISSIVE = 1
    let public DTWAIN_LS_RED = 0
    let public DTWAIN_LS_GREEN = 1
    let public DTWAIN_LS_BLUE = 2
    let public DTWAIN_LS_NONE = 3
    let public DTWAIN_LS_WHITE = 4
    let public DTWAIN_LS_UV = 5
    let public DTWAIN_LS_IR = 6
    let public DTWAIN_DLG_SORTNAMES = 1
    let public DTWAIN_DLG_CENTER = 2
    let public DTWAIN_DLG_CENTER_SCREEN = 4
    let public DTWAIN_DLG_USETEMPLATE = 8
    let public DTWAIN_DLG_CLEAR_PARAMS = 16
    let public DTWAIN_DLG_HORIZONTALSCROLL = 32
    let public DTWAIN_DLG_USEINCLUDENAMES = 64
    let public DTWAIN_DLG_USEEXCLUDENAMES = 128
    let public DTWAIN_DLG_USENAMEMAPPING = 256
    let public DTWAIN_DLG_TOPMOSTWINDOW = 1024
    let public DTWAIN_DLG_OPENONSELECT = 2048
    let public DTWAIN_DLG_NOOPENONSELECT = 4096
    let public DTWAIN_DLG_HIGHLIGHTFIRST = 8192
    let public DTWAIN_DLG_SAVELASTSCREENPOS = 16384
    let public DTWAIN_RES_ENGLISH = 0
    let public DTWAIN_RES_FRENCH = 1
    let public DTWAIN_RES_SPANISH = 2
    let public DTWAIN_RES_DUTCH = 3
    let public DTWAIN_RES_GERMAN = 4
    let public DTWAIN_RES_ITALIAN = 5
    let public DTWAIN_AL_ALARM = 0
    let public DTWAIN_AL_FEEDERERROR = 1
    let public DTWAIN_AL_FEEDERWARNING = 2
    let public DTWAIN_AL_BARCODE = 3
    let public DTWAIN_AL_DOUBLEFEED = 4
    let public DTWAIN_AL_JAM = 5
    let public DTWAIN_AL_PATCHCODE = 6
    let public DTWAIN_AL_POWER = 7
    let public DTWAIN_AL_SKEW = 8
    let public DTWAIN_FT_CAMERA = 0
    let public DTWAIN_FT_CAMERATOP = 1
    let public DTWAIN_FT_CAMERABOTTOM = 2
    let public DTWAIN_FT_CAMERAPREVIEW = 3
    let public DTWAIN_FT_DOMAIN = 4
    let public DTWAIN_FT_HOST = 5
    let public DTWAIN_FT_DIRECTORY = 6
    let public DTWAIN_FT_IMAGE = 7
    let public DTWAIN_FT_UNKNOWN = 8
    let public DTWAIN_NF_NONE = 0
    let public DTWAIN_NF_AUTO = 1
    let public DTWAIN_NF_LONEPIXEL = 2
    let public DTWAIN_NF_MAJORITYRULE = 3
    let public DTWAIN_CB_AUTO = 0
    let public DTWAIN_CB_CLEAR = 1
    let public DTWAIN_CB_NOCLEAR = 2
    let public DTWAIN_FA_NONE = 0
    let public DTWAIN_FA_LEFT = 1
    let public DTWAIN_FA_CENTER = 2
    let public DTWAIN_FA_RIGHT = 3
    let public DTWAIN_PF_CHOCOLATE = 0
    let public DTWAIN_PF_VANILLA = 1
    let public DTWAIN_FO_FIRSTPAGEFIRST = 0
    let public DTWAIN_FO_LASTPAGEFIRST = 1
    let public DTWAIN_INCREMENT_STATIC = 0
    let public DTWAIN_INCREMENT_DYNAMIC = 1
    let public DTWAIN_INCREMENT_DEFAULT = -1
    let public DTWAIN_MANDUP_FACEUPTOPPAGE = 0
    let public DTWAIN_MANDUP_FACEUPBOTTOMPAGE = 1
    let public DTWAIN_MANDUP_FACEDOWNTOPPAGE = 2
    let public DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE = 3
    let public DTWAIN_FILESAVE_DEFAULT = 0
    let public DTWAIN_FILESAVE_UICLOSE = 1
    let public DTWAIN_FILESAVE_SOURCECLOSE = 2
    let public DTWAIN_FILESAVE_ENDACQUIRE = 3
    let public DTWAIN_FILESAVE_MANUALSAVE = 4
    let public DTWAIN_FILESAVE_SAVEINCOMPLETE = 128
    let public DTWAIN_MANDUP_SCANOK = 1
    let public DTWAIN_MANDUP_SIDE1RESCAN = 2
    let public DTWAIN_MANDUP_SIDE2RESCAN = 3
    let public DTWAIN_MANDUP_RESCANALL = 4
    let public DTWAIN_MANDUP_PAGEMISSING = 5
    let public DTWAIN_DEMODLL_VERSION = 0x00000001
    let public DTWAIN_UNLICENSED_VERSION = 0x00000002
    let public DTWAIN_COMPANY_VERSION = 0x00000004
    let public DTWAIN_GENERAL_VERSION = 0x00000008
    let public DTWAIN_DEVELOP_VERSION = 0x00000010
    let public DTWAIN_JAVA_VERSION = 0x00000020
    let public DTWAIN_TOOLKIT_VERSION = 0x00000040
    let public DTWAIN_LIMITEDDLL_VERSION = 0x00000080
    let public DTWAIN_STATICLIB_VERSION = 0x00000100
    let public DTWAIN_STATICLIB_STDCALL_VERSION = 0x00000200
    let public DTWAIN_PDF_VERSION = 0x00010000
    let public DTWAIN_TWAINSAVE_VERSION = 0x00020000
    let public DTWAIN_OCR_VERSION = 0x00040000
    let public DTWAIN_BARCODE_VERSION = 0x00080000
    let public DTWAIN_ACTIVEX_VERSION = 0x00100000
    let public DTWAIN_32BIT_VERSION = 0x00200000
    let public DTWAIN_64BIT_VERSION = 0x00400000
    let public DTWAIN_UNICODE_VERSION = 0x00800000
    let public DTWAIN_OPENSOURCE_VERSION = 0x01000000
    let public DTWAIN_CALLSTACK_LOGGING = 0x02000000
    let public DTWAIN_CALLSTACK_LOGGING_PLUS = 0x04000000
    let public DTWAINOCR_RETURNHANDLE = 1
    let public DTWAINOCR_COPYDATA = 2
    let public DTWAIN_OCRINFO_CHAR = 0
    let public DTWAIN_OCRINFO_CHARXPOS = 1
    let public DTWAIN_OCRINFO_CHARYPOS = 2
    let public DTWAIN_OCRINFO_CHARXWIDTH = 3
    let public DTWAIN_OCRINFO_CHARYWIDTH = 4
    let public DTWAIN_OCRINFO_CHARCONFIDENCE = 5
    let public DTWAIN_OCRINFO_PAGENUM = 6
    let public DTWAIN_OCRINFO_OCRENGINE = 7
    let public DTWAIN_OCRINFO_TEXTLENGTH = 8
    let public DTWAIN_PDFPAGETYPE_COLOR = 0
    let public DTWAIN_PDFPAGETYPE_BW = 1
    let public DTWAIN_TWAINDSM_LEGACY = 1
    let public DTWAIN_TWAINDSM_VERSION2 = 2
    let public DTWAIN_TWAINDSM_LATESTVERSION = 4
    let public DTWAIN_TWAINDSMSEARCH_NOTFOUND = (-1)
    let public DTWAIN_TWAINDSMSEARCH_WSO = 0
    let public DTWAIN_TWAINDSMSEARCH_WOS = 1
    let public DTWAIN_TWAINDSMSEARCH_SWO = 2
    let public DTWAIN_TWAINDSMSEARCH_SOW = 3
    let public DTWAIN_TWAINDSMSEARCH_OWS = 4
    let public DTWAIN_TWAINDSMSEARCH_OSW = 5
    let public DTWAIN_TWAINDSMSEARCH_W = 6
    let public DTWAIN_TWAINDSMSEARCH_S = 7
    let public DTWAIN_TWAINDSMSEARCH_O = 8
    let public DTWAIN_TWAINDSMSEARCH_WS = 9
    let public DTWAIN_TWAINDSMSEARCH_WO = 10
    let public DTWAIN_TWAINDSMSEARCH_SW = 11
    let public DTWAIN_TWAINDSMSEARCH_SO = 12
    let public DTWAIN_TWAINDSMSEARCH_OW = 13
    let public DTWAIN_TWAINDSMSEARCH_OS = 14
    let public DTWAIN_TWAINDSMSEARCH_C = 15
    let public DTWAIN_TWAINDSMSEARCH_U = 16
    let public DTWAIN_PDFPOLARITY_POSITIVE = 1
    let public DTWAIN_PDFPOLARITY_NEGATIVE = 2
    let public DTWAIN_TWPF_NORMAL = 0
    let public DTWAIN_TWPF_BOLD = 1
    let public DTWAIN_TWPF_ITALIC = 2
    let public DTWAIN_TWPF_LARGESIZE = 3
    let public DTWAIN_TWPF_SMALLSIZE = 4
    let public DTWAIN_TWCT_PAGE = 0
    let public DTWAIN_TWCT_PATCH1 = 1
    let public DTWAIN_TWCT_PATCH2 = 2
    let public DTWAIN_TWCT_PATCH3 = 3
    let public DTWAIN_TWCT_PATCH4 = 4
    let public DTWAIN_TWCT_PATCH5 = 5
    let public DTWAIN_TWCT_PATCH6 = 6
    let public DTWAIN_AUTOSIZE_NONE = 0
    let public DTWAIN_CV_CAPCUSTOMBASE = 0x8000
    let public DTWAIN_CV_CAPXFERCOUNT = 0x0001
    let public DTWAIN_CV_ICAPCOMPRESSION = 0x0100
    let public DTWAIN_CV_ICAPPIXELTYPE = 0x0101
    let public DTWAIN_CV_ICAPUNITS = 0x0102
    let public DTWAIN_CV_ICAPXFERMECH = 0x0103
    let public DTWAIN_CV_CAPAUTHOR = 0x1000
    let public DTWAIN_CV_CAPCAPTION = 0x1001
    let public DTWAIN_CV_CAPFEEDERENABLED = 0x1002
    let public DTWAIN_CV_CAPFEEDERLOADED = 0x1003
    let public DTWAIN_CV_CAPTIMEDATE = 0x1004
    let public DTWAIN_CV_CAPSUPPORTEDCAPS = 0x1005
    let public DTWAIN_CV_CAPEXTENDEDCAPS = 0x1006
    let public DTWAIN_CV_CAPAUTOFEED = 0x1007
    let public DTWAIN_CV_CAPCLEARPAGE = 0x1008
    let public DTWAIN_CV_CAPFEEDPAGE = 0x1009
    let public DTWAIN_CV_CAPREWINDPAGE = 0x100a
    let public DTWAIN_CV_CAPINDICATORS = 0x100b
    let public DTWAIN_CV_CAPSUPPORTEDCAPSEXT = 0x100c
    let public DTWAIN_CV_CAPPAPERDETECTABLE = 0x100d
    let public DTWAIN_CV_CAPUICONTROLLABLE = 0x100e
    let public DTWAIN_CV_CAPDEVICEONLINE = 0x100f
    let public DTWAIN_CV_CAPAUTOSCAN = 0x1010
    let public DTWAIN_CV_CAPTHUMBNAILSENABLED = 0x1011
    let public DTWAIN_CV_CAPDUPLEX = 0x1012
    let public DTWAIN_CV_CAPDUPLEXENABLED = 0x1013
    let public DTWAIN_CV_CAPENABLEDSUIONLY = 0x1014
    let public DTWAIN_CV_CAPCUSTOMDSDATA = 0x1015
    let public DTWAIN_CV_CAPENDORSER = 0x1016
    let public DTWAIN_CV_CAPJOBCONTROL = 0x1017
    let public DTWAIN_CV_CAPALARMS = 0x1018
    let public DTWAIN_CV_CAPALARMVOLUME = 0x1019
    let public DTWAIN_CV_CAPAUTOMATICCAPTURE = 0x101a
    let public DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE = 0x101b
    let public DTWAIN_CV_CAPTIMEBETWEENCAPTURES = 0x101c
    let public DTWAIN_CV_CAPCLEARBUFFERS = 0x101d
    let public DTWAIN_CV_CAPMAXBATCHBUFFERS = 0x101e
    let public DTWAIN_CV_CAPDEVICETIMEDATE = 0x101f
    let public DTWAIN_CV_CAPPOWERSUPPLY = 0x1020
    let public DTWAIN_CV_CAPCAMERAPREVIEWUI = 0x1021
    let public DTWAIN_CV_CAPDEVICEEVENT = 0x1022
    let public DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE = 0x1023
    let public DTWAIN_CV_CAPSERIALNUMBER = 0x1024
    let public DTWAIN_CV_CAPFILESYSTEM = 0x1025
    let public DTWAIN_CV_CAPPRINTER = 0x1026
    let public DTWAIN_CV_CAPPRINTERENABLED = 0x1027
    let public DTWAIN_CV_CAPPRINTERINDEX = 0x1028
    let public DTWAIN_CV_CAPPRINTERMODE = 0x1029
    let public DTWAIN_CV_CAPPRINTERSTRING = 0x102a
    let public DTWAIN_CV_CAPPRINTERSUFFIX = 0x102b
    let public DTWAIN_CV_CAPLANGUAGE = 0x102c
    let public DTWAIN_CV_CAPFEEDERALIGNMENT = 0x102d
    let public DTWAIN_CV_CAPFEEDERORDER = 0x102e
    let public DTWAIN_CV_CAPPAPERBINDING = 0x102f
    let public DTWAIN_CV_CAPREACQUIREALLOWED = 0x1030
    let public DTWAIN_CV_CAPPASSTHRU = 0x1031
    let public DTWAIN_CV_CAPBATTERYMINUTES = 0x1032
    let public DTWAIN_CV_CAPBATTERYPERCENTAGE = 0x1033
    let public DTWAIN_CV_CAPPOWERDOWNTIME = 0x1034
    let public DTWAIN_CV_CAPSEGMENTED = 0x1035
    let public DTWAIN_CV_CAPCAMERAENABLED = 0x1036
    let public DTWAIN_CV_CAPCAMERAORDER = 0x1037
    let public DTWAIN_CV_CAPMICRENABLED = 0x1038
    let public DTWAIN_CV_CAPFEEDERPREP = 0x1039
    let public DTWAIN_CV_CAPFEEDERPOCKET = 0x103a
    let public DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM = 0x103b
    let public DTWAIN_CV_CAPCUSTOMINTERFACEGUID = 0x103c
    let public DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE = 0x103d
    let public DTWAIN_CV_CAPSUPPORTEDDATS = 0x103e
    let public DTWAIN_CV_CAPDOUBLEFEEDDETECTION = 0x103f
    let public DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH = 0x1040
    let public DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY = 0x1041
    let public DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE = 0x1042
    let public DTWAIN_CV_CAPPAPERHANDLING = 0x1043
    let public DTWAIN_CV_CAPINDICATORSMODE = 0x1044
    let public DTWAIN_CV_CAPPRINTERVERTICALOFFSET = 0x1045
    let public DTWAIN_CV_CAPPOWERSAVETIME = 0x1046
    let public DTWAIN_CV_CAPPRINTERCHARROTATION = 0x1047
    let public DTWAIN_CV_CAPPRINTERFONTSTYLE = 0x1048
    let public DTWAIN_CV_CAPPRINTERINDEXLEADCHAR = 0x1049
    let public DTWAIN_CV_CAPIMAGEADDRESSENABLED = 0x1050
    let public DTWAIN_CV_CAPIAFIELDA_LEVEL = 0x1051
    let public DTWAIN_CV_CAPIAFIELDB_LEVEL = 0x1052
    let public DTWAIN_CV_CAPIAFIELDC_LEVEL = 0x1053
    let public DTWAIN_CV_CAPIAFIELDD_LEVEL = 0x1054
    let public DTWAIN_CV_CAPIAFIELDE_LEVEL = 0x1055
    let public DTWAIN_CV_CAPIAFIELDA_PRINTFORMAT = 0x1056
    let public DTWAIN_CV_CAPIAFIELDB_PRINTFORMAT = 0x1057
    let public DTWAIN_CV_CAPIAFIELDC_PRINTFORMAT = 0x1058
    let public DTWAIN_CV_CAPIAFIELDD_PRINTFORMAT = 0x1059
    let public DTWAIN_CV_CAPIAFIELDE_PRINTFORMAT = 0x105A
    let public DTWAIN_CV_CAPIAFIELDA_VALUE = 0x105B
    let public DTWAIN_CV_CAPIAFIELDB_VALUE = 0x105C
    let public DTWAIN_CV_CAPIAFIELDC_VALUE = 0x105D
    let public DTWAIN_CV_CAPIAFIELDD_VALUE = 0x105E
    let public DTWAIN_CV_CAPIAFIELDE_VALUE = 0x105F
    let public DTWAIN_CV_CAPIAFIELDA_LASTPAGE = 0x1060
    let public DTWAIN_CV_CAPIAFIELDB_LASTPAGE = 0x1061
    let public DTWAIN_CV_CAPIAFIELDC_LASTPAGE = 0x1062
    let public DTWAIN_CV_CAPIAFIELDD_LASTPAGE = 0x1063
    let public DTWAIN_CV_CAPIAFIELDE_LASTPAGE = 0x1064
    let public DTWAIN_CV_CAPPRINTERINDEXMAXVALUE = 0x104A
    let public DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS = 0x104B
    let public DTWAIN_CV_CAPPRINTERINDEXSTEP = 0x104C
    let public DTWAIN_CV_CAPPRINTERINDEXTRIGGER = 0x104D
    let public DTWAIN_CV_CAPPRINTERSTRINGPREVIEW = 0x104E
    let public DTWAIN_CV_ICAPAUTOBRIGHT = 0x1100
    let public DTWAIN_CV_ICAPBRIGHTNESS = 0x1101
    let public DTWAIN_CV_ICAPCONTRAST = 0x1103
    let public DTWAIN_CV_ICAPCUSTHALFTONE = 0x1104
    let public DTWAIN_CV_ICAPEXPOSURETIME = 0x1105
    let public DTWAIN_CV_ICAPFILTER = 0x1106
    let public DTWAIN_CV_ICAPFLASHUSED = 0x1107
    let public DTWAIN_CV_ICAPGAMMA = 0x1108
    let public DTWAIN_CV_ICAPHALFTONES = 0x1109
    let public DTWAIN_CV_ICAPHIGHLIGHT = 0x110a
    let public DTWAIN_CV_ICAPIMAGEFILEFORMAT = 0x110c
    let public DTWAIN_CV_ICAPLAMPSTATE = 0x110d
    let public DTWAIN_CV_ICAPLIGHTSOURCE = 0x110e
    let public DTWAIN_CV_ICAPORIENTATION = 0x1110
    let public DTWAIN_CV_ICAPPHYSICALWIDTH = 0x1111
    let public DTWAIN_CV_ICAPPHYSICALHEIGHT = 0x1112
    let public DTWAIN_CV_ICAPSHADOW = 0x1113
    let public DTWAIN_CV_ICAPFRAMES = 0x1114
    let public DTWAIN_CV_ICAPXNATIVERESOLUTION = 0x1116
    let public DTWAIN_CV_ICAPYNATIVERESOLUTION = 0x1117
    let public DTWAIN_CV_ICAPXRESOLUTION = 0x1118
    let public DTWAIN_CV_ICAPYRESOLUTION = 0x1119
    let public DTWAIN_CV_ICAPMAXFRAMES = 0x111a
    let public DTWAIN_CV_ICAPTILES = 0x111b
    let public DTWAIN_CV_ICAPBITORDER = 0x111c
    let public DTWAIN_CV_ICAPCCITTKFACTOR = 0x111d
    let public DTWAIN_CV_ICAPLIGHTPATH = 0x111e
    let public DTWAIN_CV_ICAPPIXELFLAVOR = 0x111f
    let public DTWAIN_CV_ICAPPLANARCHUNKY = 0x1120
    let public DTWAIN_CV_ICAPROTATION = 0x1121
    let public DTWAIN_CV_ICAPSUPPORTEDSIZES = 0x1122
    let public DTWAIN_CV_ICAPTHRESHOLD = 0x1123
    let public DTWAIN_CV_ICAPXSCALING = 0x1124
    let public DTWAIN_CV_ICAPYSCALING = 0x1125
    let public DTWAIN_CV_ICAPBITORDERCODES = 0x1126
    let public DTWAIN_CV_ICAPPIXELFLAVORCODES = 0x1127
    let public DTWAIN_CV_ICAPJPEGPIXELTYPE = 0x1128
    let public DTWAIN_CV_ICAPTIMEFILL = 0x112a
    let public DTWAIN_CV_ICAPBITDEPTH = 0x112b
    let public DTWAIN_CV_ICAPBITDEPTHREDUCTION = 0x112c
    let public DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE = 0x112d
    let public DTWAIN_CV_ICAPIMAGEDATASET = 0x112e
    let public DTWAIN_CV_ICAPEXTIMAGEINFO = 0x112f
    let public DTWAIN_CV_ICAPMINIMUMHEIGHT = 0x1130
    let public DTWAIN_CV_ICAPMINIMUMWIDTH = 0x1131
    let public DTWAIN_CV_ICAPAUTOBORDERDETECTION = 0x1132
    let public DTWAIN_CV_ICAPAUTODESKEW = 0x1133
    let public DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES = 0x1134
    let public DTWAIN_CV_ICAPAUTOROTATE = 0x1135
    let public DTWAIN_CV_ICAPFLIPROTATION = 0x1136
    let public DTWAIN_CV_ICAPBARCODEDETECTIONENABLED = 0x1137
    let public DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES = 0x1138
    let public DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES = 0x1139
    let public DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES = 0x113a
    let public DTWAIN_CV_ICAPBARCODESEARCHMODE = 0x113b
    let public DTWAIN_CV_ICAPBARCODEMAXRETRIES = 0x113c
    let public DTWAIN_CV_ICAPBARCODETIMEOUT = 0x113d
    let public DTWAIN_CV_ICAPZOOMFACTOR = 0x113e
    let public DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED = 0x113f
    let public DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES = 0x1140
    let public DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES = 0x1141
    let public DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES = 0x1142
    let public DTWAIN_CV_ICAPPATCHCODESEARCHMODE = 0x1143
    let public DTWAIN_CV_ICAPPATCHCODEMAXRETRIES = 0x1144
    let public DTWAIN_CV_ICAPPATCHCODETIMEOUT = 0x1145
    let public DTWAIN_CV_ICAPFLASHUSED2 = 0x1146
    let public DTWAIN_CV_ICAPIMAGEFILTER = 0x1147
    let public DTWAIN_CV_ICAPNOISEFILTER = 0x1148
    let public DTWAIN_CV_ICAPOVERSCAN = 0x1149
    let public DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION = 0x1150
    let public DTWAIN_CV_ICAPAUTOMATICDESKEW = 0x1151
    let public DTWAIN_CV_ICAPAUTOMATICROTATE = 0x1152
    let public DTWAIN_CV_ICAPJPEGQUALITY = 0x1153
    let public DTWAIN_CV_ICAPFEEDERTYPE = 0x1154
    let public DTWAIN_CV_ICAPICCPROFILE = 0x1155
    let public DTWAIN_CV_ICAPAUTOSIZE = 0x1156
    let public DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME = 0x1157
    let public DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION = 0x1158
    let public DTWAIN_CV_ICAPAUTOMATICCOLORENABLED = 0x1159
    let public DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE = 0x115a
    let public DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED = 0x115b
    let public DTWAIN_CV_ICAPIMAGEMERGE = 0x115c
    let public DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD = 0x115d
    let public DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO = 0x115e
    let public DTWAIN_CV_ICAPFILMTYPE = 0x115f
    let public DTWAIN_CV_ICAPMIRROR = 0x1160
    let public DTWAIN_CV_ICAPJPEGSUBSAMPLING = 0x1161
    let public DTWAIN_CV_ACAPAUDIOFILEFORMAT = 0x1201
    let public DTWAIN_CV_ACAPXFERMECH = 0x1202
    let public DTWAIN_CFMCV_CAPCFMSTART = 2048
    let public DTWAIN_CFMCV_CAPDUPLEXSCANNER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+10)
    let public DTWAIN_CFMCV_CAPDUPLEXENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+11)
    let public DTWAIN_CFMCV_CAPSCANNERNAME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+12)
    let public DTWAIN_CFMCV_CAPSINGLEPASS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+13)
    let public DTWAIN_CFMCV_CAPERRHANDLING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+20)
    let public DTWAIN_CFMCV_CAPFEEDERSTATUS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+21)
    let public DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+22)
    let public DTWAIN_CFMCV_CAPFEEDWAITTIME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+23)
    let public DTWAIN_CFMCV_ICAPWHITEBALANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+24)
    let public DTWAIN_CFMCV_ICAPAUTOBINARY = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+25)
    let public DTWAIN_CFMCV_ICAPIMAGESEPARATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+26)
    let public DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+27)
    let public DTWAIN_CFMCV_ICAPIMAGEEMPHASIS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+28)
    let public DTWAIN_CFMCV_ICAPOUTLINING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+29)
    let public DTWAIN_CFMCV_ICAPDYNTHRESHOLD = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+30)
    let public DTWAIN_CFMCV_ICAPVARIANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+31)
    let public DTWAIN_CFMCV_CAPENDORSERAVAILABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+32)
    let public DTWAIN_CFMCV_CAPENDORSERENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+33)
    let public DTWAIN_CFMCV_CAPENDORSERCHARSET = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+34)
    let public DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+35)
    let public DTWAIN_CFMCV_CAPENDORSERSTRING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+36)
    let public DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+48)
    let public DTWAIN_CFMCV_ICAPSMOOTHINGMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+49)
    let public DTWAIN_CFMCV_ICAPFILTERMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+50)
    let public DTWAIN_CFMCV_ICAPGRADATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+51)
    let public DTWAIN_CFMCV_ICAPMIRROR = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+52)
    let public DTWAIN_CFMCV_ICAPEASYSCANMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+53)
    let public DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+54)
    let public DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+55)
    let public DTWAIN_CFMCV_CAPDUPLEXPAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+56)
    let public DTWAIN_CFMCV_ICAPINVERTIMAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+57)
    let public DTWAIN_CFMCV_ICAPSPECKLEREMOVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+58)
    let public DTWAIN_CFMCV_ICAPUSMFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+59)
    let public DTWAIN_CFMCV_ICAPNOISEFILTERCFM = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+60)
    let public DTWAIN_CFMCV_ICAPDESCREENING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+61)
    let public DTWAIN_CFMCV_ICAPQUALITYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+62)
    let public DTWAIN_CFMCV_ICAPBINARYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+63)
    let public DTWAIN_OCRCV_IMAGEFILEFORMAT = 0x1000
    let public DTWAIN_OCRCV_DESKEW = 0x1001
    let public DTWAIN_OCRCV_DESHADE = 0x1002
    let public DTWAIN_OCRCV_ORIENTATION = 0x1003
    let public DTWAIN_OCRCV_NOISEREMOVE = 0x1004
    let public DTWAIN_OCRCV_LINEREMOVE = 0x1005
    let public DTWAIN_OCRCV_INVERTPAGE = 0x1006
    let public DTWAIN_OCRCV_INVERTZONES = 0x1007
    let public DTWAIN_OCRCV_LINEREJECT = 0x1008
    let public DTWAIN_OCRCV_CHARACTERREJECT = 0x1009
    let public DTWAIN_OCRCV_ERRORREPORTMODE = 0x1010
    let public DTWAIN_OCRCV_ERRORREPORTFILE = 0x1011
    let public DTWAIN_OCRCV_PIXELTYPE = 0x1012
    let public DTWAIN_OCRCV_BITDEPTH = 0x1013
    let public DTWAIN_OCRCV_RETURNCHARINFO = 0x1014
    let public DTWAIN_OCRCV_NATIVEFILEFORMAT = 0x1015
    let public DTWAIN_OCRCV_MPNATIVEFILEFORMAT = 0x1016
    let public DTWAIN_OCRCV_SUPPORTEDCAPS = 0x1017
    let public DTWAIN_OCRCV_DISABLECHARACTERS = 0x1018
    let public DTWAIN_OCRCV_REMOVECONTROLCHARS = 0x1019
    let public DTWAIN_OCRORIENT_OFF = 0
    let public DTWAIN_OCRORIENT_AUTO = 1
    let public DTWAIN_OCRORIENT_90 = 2
    let public DTWAIN_OCRORIENT_180 = 3
    let public DTWAIN_OCRORIENT_270 = 4
    let public DTWAIN_OCRIMAGEFORMAT_AUTO = 10000
    let public DTWAIN_OCRERROR_MODENONE = 0
    let public DTWAIN_OCRERROR_SHOWMSGBOX = 1
    let public DTWAIN_OCRERROR_WRITEFILE = 2
    let public DTWAIN_PDFTEXT_ALLPAGES = 0x00000001
    let public DTWAIN_PDFTEXT_EVENPAGES = 0x00000002
    let public DTWAIN_PDFTEXT_ODDPAGES = 0x00000004
    let public DTWAIN_PDFTEXT_FIRSTPAGE = 0x00000008
    let public DTWAIN_PDFTEXT_LASTPAGE = 0x00000010
    let public DTWAIN_PDFTEXT_CURRENTPAGE = 0x00000020
    let public DTWAIN_PDFTEXT_DISABLED = 0x00000040
    let public DTWAIN_PDFTEXT_TOPLEFT = 0x00000100
    let public DTWAIN_PDFTEXT_TOPRIGHT = 0x00000200
    let public DTWAIN_PDFTEXT_HORIZCENTER = 0x00000400
    let public DTWAIN_PDFTEXT_VERTCENTER = 0x00000800
    let public DTWAIN_PDFTEXT_BOTTOMLEFT = 0x00001000
    let public DTWAIN_PDFTEXT_BOTTOMRIGHT = 0x00002000
    let public DTWAIN_PDFTEXT_BOTTOMCENTER = 0x00004000
    let public DTWAIN_PDFTEXT_TOPCENTER = 0x00008000
    let public DTWAIN_PDFTEXT_XCENTER = 0x00010000
    let public DTWAIN_PDFTEXT_YCENTER = 0x00020000
    let public DTWAIN_PDFTEXT_NOSCALING = 0x00100000
    let public DTWAIN_PDFTEXT_NOCHARSPACING = 0x00200000
    let public DTWAIN_PDFTEXT_NOWORDSPACING = 0x00400000
    let public DTWAIN_PDFTEXT_NOSTROKEWIDTH = 0x00800000
    let public DTWAIN_PDFTEXT_NORENDERMODE = 0x01000000
    let public DTWAIN_PDFTEXT_NORGBCOLOR = 0x02000000
    let public DTWAIN_PDFTEXT_NOFONTSIZE = 0x04000000
    let public DTWAIN_PDFTEXT_NOABSPOSITION = 0x08000000
    let public DTWAIN_PDFTEXT_IGNOREALL = 0xFFF00000
    let public DTWAIN_FONT_COURIER = 0
    let public DTWAIN_FONT_COURIERBOLD = 1
    let public DTWAIN_FONT_COURIERBOLDOBLIQUE = 2
    let public DTWAIN_FONT_COURIEROBLIQUE = 3
    let public DTWAIN_FONT_HELVETICA = 4
    let public DTWAIN_FONT_HELVETICABOLD = 5
    let public DTWAIN_FONT_HELVETICABOLDOBLIQUE = 6
    let public DTWAIN_FONT_HELVETICAOBLIQUE = 7
    let public DTWAIN_FONT_TIMESBOLD = 8
    let public DTWAIN_FONT_TIMESBOLDITALIC = 9
    let public DTWAIN_FONT_TIMESROMAN = 10
    let public DTWAIN_FONT_TIMESITALIC = 11
    let public DTWAIN_FONT_SYMBOL = 12
    let public DTWAIN_FONT_ZAPFDINGBATS = 13
    let public DTWAIN_PDFRENDER_FILL = 0
    let public DTWAIN_PDFRENDER_STROKE = 1
    let public DTWAIN_PDFRENDER_FILLSTROKE = 2
    let public DTWAIN_PDFRENDER_INVISIBLE = 3
    let public DTWAIN_PDFTEXTELEMENT_SCALINGXY = 0
    let public DTWAIN_PDFTEXTELEMENT_FONTHEIGHT = 1
    let public DTWAIN_PDFTEXTELEMENT_WORDSPACING = 2
    let public DTWAIN_PDFTEXTELEMENT_POSITION = 3
    let public DTWAIN_PDFTEXTELEMENT_COLOR = 4
    let public DTWAIN_PDFTEXTELEMENT_STROKEWIDTH = 5
    let public DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS = 6
    let public DTWAIN_PDFTEXTELEMENT_FONTNAME = 7
    let public DTWAIN_PDFTEXTELEMENT_TEXT = 8
    let public DTWAIN_PDFTEXTELEMENT_RENDERMODE = 9
    let public DTWAIN_PDFTEXTELEMENT_CHARSPACING = 10
    let public DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE = 11
    let public DTWAIN_PDFTEXTELEMENT_LEADING = 12
    let public DTWAIN_PDFTEXTELEMENT_SCALING = 13
    let public DTWAIN_PDFTEXTELEMENT_TEXTLENGTH = 14
    let public DTWAIN_PDFTEXTELEMENT_SKEWANGLES = 15
    let public DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER = 16
    let public DTWAIN_PDFTEXTTRANSFORM_TSRK = 0
    let public DTWAIN_PDFTEXTTRANSFORM_TSKR = 1
    let public DTWAIN_PDFTEXTTRANSFORM_TKSR = 2
    let public DTWAIN_PDFTEXTTRANSFORM_TKRS = 3
    let public DTWAIN_PDFTEXTTRANSFORM_TRSK = 4
    let public DTWAIN_PDFTEXTTRANSFORM_TRKS = 5
    let public DTWAIN_PDFTEXTTRANSFORM_STRK = 6
    let public DTWAIN_PDFTEXTTRANSFORM_STKR = 7
    let public DTWAIN_PDFTEXTTRANSFORM_SKTR = 8
    let public DTWAIN_PDFTEXTTRANSFORM_SKRT = 9
    let public DTWAIN_PDFTEXTTRANSFORM_SRTK = 10
    let public DTWAIN_PDFTEXTTRANSFORM_SRKT = 11
    let public DTWAIN_PDFTEXTTRANSFORM_RSTK = 12
    let public DTWAIN_PDFTEXTTRANSFORM_RSKT = 13
    let public DTWAIN_PDFTEXTTRANSFORM_RTSK = 14
    let public DTWAIN_PDFTEXTTRANSFORM_RTKT = 15
    let public DTWAIN_PDFTEXTTRANSFORM_RKST = 16
    let public DTWAIN_PDFTEXTTRANSFORM_RKTS = 17
    let public DTWAIN_PDFTEXTTRANSFORM_KSTR = 18
    let public DTWAIN_PDFTEXTTRANSFORM_KSRT = 19
    let public DTWAIN_PDFTEXTTRANSFORM_KRST = 20
    let public DTWAIN_PDFTEXTTRANSFORM_KRTS = 21
    let public DTWAIN_PDFTEXTTRANSFORM_KTSR = 22
    let public DTWAIN_PDFTEXTTRANSFORM_KTRS = 23
    let public DTWAIN_PDFTEXTTRANFORM_LAST = DTWAIN_PDFTEXTTRANSFORM_KTRS
    let public DTWAIN_TWDF_ULTRASONIC = 0
    let public DTWAIN_TWDF_BYLENGTH = 1
    let public DTWAIN_TWDF_INFRARED = 2
    let public DTWAIN_TWAS_NONE = 0
    let public DTWAIN_TWAS_AUTO = 1
    let public DTWAIN_TWAS_CURRENT = 2
    let public DTWAIN_TWFR_BOOK = 0
    let public DTWAIN_TWFR_FANFOLD = 1
    let public DTWAIN_CONSTANT_TWPT = 0
    let public DTWAIN_CONSTANT_TWUN = 1
    let public DTWAIN_CONSTANT_TWCY = 2
    let public DTWAIN_CONSTANT_TWAL = 3
    let public DTWAIN_CONSTANT_TWAS = 4
    let public DTWAIN_CONSTANT_TWBCOR = 5
    let public DTWAIN_CONSTANT_TWBD = 6
    let public DTWAIN_CONSTANT_TWBO = 7
    let public DTWAIN_CONSTANT_TWBP = 8
    let public DTWAIN_CONSTANT_TWBR = 9
    let public DTWAIN_CONSTANT_TWBT = 10
    let public DTWAIN_CONSTANT_TWCP = 11
    let public DTWAIN_CONSTANT_TWCS = 12
    let public DTWAIN_CONSTANT_TWDE = 13
    let public DTWAIN_CONSTANT_TWDR = 14
    let public DTWAIN_CONSTANT_TWDSK = 15
    let public DTWAIN_CONSTANT_TWDX = 16
    let public DTWAIN_CONSTANT_TWFA = 17
    let public DTWAIN_CONSTANT_TWFE = 18
    let public DTWAIN_CONSTANT_TWFF = 19
    let public DTWAIN_CONSTANT_TWFL = 20
    let public DTWAIN_CONSTANT_TWFO = 21
    let public DTWAIN_CONSTANT_TWFP = 22
    let public DTWAIN_CONSTANT_TWFR = 23
    let public DTWAIN_CONSTANT_TWFT = 24
    let public DTWAIN_CONSTANT_TWFY = 25
    let public DTWAIN_CONSTANT_TWIA = 26
    let public DTWAIN_CONSTANT_TWIC = 27
    let public DTWAIN_CONSTANT_TWIF = 28
    let public DTWAIN_CONSTANT_TWIM = 29
    let public DTWAIN_CONSTANT_TWJC = 30
    let public DTWAIN_CONSTANT_TWJQ = 31
    let public DTWAIN_CONSTANT_TWLP = 32
    let public DTWAIN_CONSTANT_TWLS = 33
    let public DTWAIN_CONSTANT_TWMD = 34
    let public DTWAIN_CONSTANT_TWNF = 35
    let public DTWAIN_CONSTANT_TWOR = 36
    let public DTWAIN_CONSTANT_TWOV = 37
    let public DTWAIN_CONSTANT_TWPA = 38
    let public DTWAIN_CONSTANT_TWPC = 39
    let public DTWAIN_CONSTANT_TWPCH = 40
    let public DTWAIN_CONSTANT_TWPF = 41
    let public DTWAIN_CONSTANT_TWPM = 42
    let public DTWAIN_CONSTANT_TWPR = 43
    let public DTWAIN_CONSTANT_TWPF2 = 44
    let public DTWAIN_CONSTANT_TWCT = 45
    let public DTWAIN_CONSTANT_TWPS = 46
    let public DTWAIN_CONSTANT_TWSS = 47
    let public DTWAIN_CONSTANT_TWPH = 48
    let public DTWAIN_CONSTANT_TWCI = 49
    let public DTWAIN_CONSTANT_FONTNAME = 50
    let public DTWAIN_CONSTANT_TWEI = 51
    let public DTWAIN_CONSTANT_TWEJ = 52
    let public DTWAIN_CONSTANT_TWCC = 53
    let public DTWAIN_CONSTANT_TWQC = 54
    let public DTWAIN_CONSTANT_TWRC = 55
    let public DTWAIN_CONSTANT_MSG = 56
    let public DTWAIN_CONSTANT_TWLG = 57
    let public DTWAIN_CONSTANT_DLLINFO = 58
    let public DTWAIN_CONSTANT_DG = 59
    let public DTWAIN_CONSTANT_DAT = 60
    let public DTWAIN_CONSTANT_DF = 61
    let public DTWAIN_CONSTANT_TWTY = 62
    let public DTWAIN_CONSTANT_TWCB = 63
    let public DTWAIN_CONSTANT_TWAF = 64
    let public DTWAIN_CONSTANT_TWFS = 65
    let public DTWAIN_CONSTANT_TWJS = 66
    let public DTWAIN_CONSTANT_TWMR = 67
    let public DTWAIN_CONSTANT_TWDP = 68
    let public DTWAIN_CONSTANT_TWUS = 69
    let public DTWAIN_CONSTANT_TWDF = 70
    let public DTWAIN_CONSTANT_TWFM = 71
    let public DTWAIN_CONSTANT_TWSG = 72
    let public DTWAIN_CONSTANT_DTWAIN_TN = 73
    let public DTWAIN_CONSTANT_TWON = 74
    let public DTWAIN_CONSTANT_TWMF = 75
    let public DTWAIN_CONSTANT_TWSX = 76
    let public DTWAIN_CONSTANT_CAP = 77
    let public DTWAIN_CONSTANT_ICAP = 78
    let public DTWAIN_CONSTANT_DTWAIN_CONT = 79
    let public DTWAIN_CONSTANT_CAPCODE_MAP = 80
    let public DTWAIN_CONSTANT_ACAP = 81
    let public DTWAIN_USERRES_START = 20000
    let public DTWAIN_USERRES_MAXSIZE = 8192
    let public DTWAIN_APIHANDLEOK = 1
    let public DTWAIN_TWAINSESSIONOK = 2
    let public DTWAIN_PDF_AES128 = 1
    let public DTWAIN_PDF_AES256 = 2
    let public DTWAIN_FEEDER_TERMINATE = 1
    let public DTWAIN_FEEDER_USEFLATBED = 2

    // Public state exposed after successful Load
    let mutable private IsLoaded = false

    /// Returns true if the DTWAIN DLL has been successfully loaded and initialized
    let IsDLLInitialized() = IsLoaded

    module private DynamicDll =
        let mutable private hModule = IntPtr.Zero

        let rawUnload() =
            if hModule <> IntPtr.Zero then
                let ok = NativeMethods.FreeLibrary(hModule)
                if ok then hModule <- IntPtr.Zero

        let ensureNotLoaded() =
            if hModule <> IntPtr.Zero then
                failwith "DTWAIN DLL already loaded"

        let ensureLoaded() =
            if hModule = IntPtr.Zero then
                failwith "DTWAIN DLL not loaded. Call TwainAPI.Load first."

        let private getWin32ErrorMessage (errorCode: int) : string =
            let FORMAT_MESSAGE_FROM_SYSTEM = 0x00000100u
            let FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200u
            let flags = FORMAT_MESSAGE_FROM_SYSTEM ||| FORMAT_MESSAGE_IGNORE_INSERTS

            let buffer = Text.StringBuilder(1024)
            let len = NativeMethods.FormatMessage(
                    flags, IntPtr.Zero, uint32 errorCode, 0u, buffer, uint32 buffer.Capacity, IntPtr.Zero)

            if len > 0u then
                buffer.ToString().Trim()
            else
                sprintf "Unknown error (0x%08X)" errorCode

        let bind<'T when 'T :> Delegate> (name: string) : 'T =
            ensureLoaded()
            let ptr = NativeMethods.GetProcAddress(hModule, name)
            if ptr = IntPtr.Zero then
                // Don't fail immediately — some functions may be optional
                // But for critical ones, you can failwith here
                Marshal.GetDelegateForFunctionPointer<'T>(IntPtr.Zero) // returns null delegate
            else
                Marshal.GetDelegateForFunctionPointer<'T>(ptr)

        let rawLoad dllPath =
            hModule <- NativeMethods.LoadLibrary(dllPath)
            if hModule = IntPtr.Zero then
                let err = Marshal.GetLastWin32Error()
                let message = getWin32ErrorMessage err
                // This is the key: include both code and message
                failwithf "Failed to load DLL '%s': %s (Win32 error code: %d / 0x%08X)"
                    dllPath message err err

        // Public internals (only visible inside TwainAPI)
        let LoadModule = rawLoad
        let UnloadModule = rawUnload
        let Bind = bind
        let ModuleHandle = hModule

    // Delegate types
    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireAudioFileDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_AcquireAudioFileADelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_AcquireAudioFileWDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireAudioNativeDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireAudioNativeExDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL * DTWAIN_BOOL * DTWAIN_ARRAY * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireBufferedDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireBufferedExDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * DTWAIN_ARRAY * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireFileDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_AcquireFileADelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireFileExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY * LONG * LONG * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_AcquireFileWDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireNativeDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireNativeExDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * DTWAIN_ARRAY * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AcquireToClipboardDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * LONG * DTWAIN_BOOL * DTWAIN_BOOL * DTWAIN_BOOL * int byref -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AddExtImageInfoQueryDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AddFileToAppendDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_AddFileToAppendADelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_AddFileToAppendWDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AddPDFTextDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * string * DTWAIN_FLOAT * LONG * LONG * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * LONG * DWORD -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_AddPDFTextADelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * string * DTWAIN_FLOAT * LONG * LONG * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * LONG * DWORD -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AddPDFTextExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_PDFTEXTELEMENT * DWORD -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_AddPDFTextWDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * string * DTWAIN_FLOAT * LONG * LONG * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * LONG * DWORD -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AllocateMemoryDelegate = delegate of DWORD -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AllocateMemory64Delegate = delegate of ULONG64 -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AllocateMemoryExDelegate = delegate of DWORD -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_AppHandlesExceptionsDelegate = delegate of DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayANSIStringToFloatDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddDelegate = delegate of DTWAIN_ARRAY * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayAddANSIStringDelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayAddANSIStringNDelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddFloatDelegate = delegate of DTWAIN_ARRAY * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddFloatNDelegate = delegate of DTWAIN_ARRAY * DTWAIN_FLOAT * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddFloatStringDelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayAddFloatStringADelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddFloatStringNDelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayAddFloatStringNADelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayAddFloatStringNWDelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayAddFloatStringWDelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddFrameDelegate = delegate of DTWAIN_ARRAY * DTWAIN_FRAME -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddFrameNDelegate = delegate of DTWAIN_ARRAY * DTWAIN_FRAME * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddLongDelegate = delegate of DTWAIN_ARRAY * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddLong64Delegate = delegate of DTWAIN_ARRAY * LONG64 -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddLong64NDelegate = delegate of DTWAIN_ARRAY * LONG64 * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddLongNDelegate = delegate of DTWAIN_ARRAY * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddNDelegate = delegate of DTWAIN_ARRAY * LPVOID * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddStringDelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayAddStringADelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayAddStringNDelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayAddStringNADelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayAddStringNWDelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayAddStringWDelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayAddWideStringDelegate = delegate of DTWAIN_ARRAY * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayAddWideStringNDelegate = delegate of DTWAIN_ARRAY * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayConvertFix32ToFloatDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayConvertFloatToFix32Delegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayCopyDelegate = delegate of DTWAIN_ARRAY * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayCreateDelegate = delegate of LONG * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayCreateCopyDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayCreateFromCapDelegate = delegate of DTWAIN_SOURCE * LONG * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayCreateFromLong64sDelegate = delegate of Int64 byref * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayCreateFromLongsDelegate = delegate of int byref * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayCreateFromRealsDelegate = delegate of DTWAIN_FLOAT byref * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayDestroyDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayDestroyFramesDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFindDelegate = delegate of DTWAIN_ARRAY * LPVOID -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayFindANSIStringDelegate = delegate of DTWAIN_ARRAY * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFindFloatDelegate = delegate of DTWAIN_ARRAY * DTWAIN_FLOAT * DTWAIN_FLOAT -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFindFloatStringDelegate = delegate of DTWAIN_ARRAY * string * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayFindFloatStringADelegate = delegate of DTWAIN_ARRAY * string * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayFindFloatStringWDelegate = delegate of DTWAIN_ARRAY * string * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFindLongDelegate = delegate of DTWAIN_ARRAY * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFindLong64Delegate = delegate of DTWAIN_ARRAY * LONG64 -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFindStringDelegate = delegate of DTWAIN_ARRAY * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayFindStringADelegate = delegate of DTWAIN_ARRAY * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayFindStringWDelegate = delegate of DTWAIN_ARRAY * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayFindWideStringDelegate = delegate of DTWAIN_ARRAY * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFix32GetAtDelegate = delegate of DTWAIN_ARRAY * int * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFix32SetAtDelegate = delegate of DTWAIN_ARRAY * int * int * int -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFloatToANSIStringDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFloatToStringDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayFloatToWideStringDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtDelegate = delegate of DTWAIN_ARRAY * LONG * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayGetAtANSIStringDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtFloatDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtFloatStringDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayGetAtFloatStringADelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayGetAtFloatStringWDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtFrameDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtFrameExDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FRAME -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtFrameStringDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayGetAtFrameStringADelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayGetAtFrameStringWDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtLongDelegate = delegate of DTWAIN_ARRAY * LONG * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtLong64Delegate = delegate of DTWAIN_ARRAY * LONG * Int64 byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtSourceDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_SOURCE byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetAtStringDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayGetAtStringADelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayGetAtStringWDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayGetAtWideStringDelegate = delegate of DTWAIN_ARRAY * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetBufferDelegate = delegate of DTWAIN_ARRAY * LONG -> LPVOID

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetCapValuesDelegate = delegate of DTWAIN_SOURCE * LONG * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetCapValuesExDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetCapValuesEx2Delegate = delegate of DTWAIN_SOURCE * LONG * LONG * LONG * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetCountDelegate = delegate of DTWAIN_ARRAY -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetMaxStringLengthDelegate = delegate of DTWAIN_ARRAY -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetSourceAtDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_SOURCE byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetStringLengthDelegate = delegate of DTWAIN_ARRAY * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayGetTypeDelegate = delegate of DTWAIN_ARRAY -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInitDelegate = delegate of unit -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtDelegate = delegate of DTWAIN_ARRAY * LONG * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayInsertAtANSIStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayInsertAtANSIStringNDelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtFloatDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtFloatNDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FLOAT * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtFloatStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayInsertAtFloatStringADelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtFloatStringNDelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayInsertAtFloatStringNADelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayInsertAtFloatStringNWDelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayInsertAtFloatStringWDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtFrameDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FRAME -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtFrameNDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FRAME * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtLongDelegate = delegate of DTWAIN_ARRAY * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtLong64Delegate = delegate of DTWAIN_ARRAY * LONG * LONG64 -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtLong64NDelegate = delegate of DTWAIN_ARRAY * LONG * LONG64 * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtLongNDelegate = delegate of DTWAIN_ARRAY * LONG * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtNDelegate = delegate of DTWAIN_ARRAY * LONG * LPVOID * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayInsertAtStringADelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayInsertAtStringNDelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArrayInsertAtStringNADelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayInsertAtStringNWDelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayInsertAtStringWDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayInsertAtWideStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArrayInsertAtWideStringNDelegate = delegate of DTWAIN_ARRAY * LONG * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayRemoveAllDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayRemoveAtDelegate = delegate of DTWAIN_ARRAY * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayRemoveAtNDelegate = delegate of DTWAIN_ARRAY * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayResizeDelegate = delegate of DTWAIN_ARRAY * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtDelegate = delegate of DTWAIN_ARRAY * LONG * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArraySetAtANSIStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtFloatDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtFloatStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArraySetAtFloatStringADelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArraySetAtFloatStringWDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtFrameDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtFrameExDelegate = delegate of DTWAIN_ARRAY * LONG * DTWAIN_FRAME -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtFrameStringDelegate = delegate of DTWAIN_ARRAY * LONG * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArraySetAtFrameStringADelegate = delegate of DTWAIN_ARRAY * LONG * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArraySetAtFrameStringWDelegate = delegate of DTWAIN_ARRAY * LONG * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtLongDelegate = delegate of DTWAIN_ARRAY * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtLong64Delegate = delegate of DTWAIN_ARRAY * LONG * LONG64 -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArraySetAtStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ArraySetAtStringADelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArraySetAtStringWDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ArraySetAtWideStringDelegate = delegate of DTWAIN_ARRAY * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayStringToFloatDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ArrayWideStringToFloatDelegate = delegate of DTWAIN_ARRAY -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CallCallbackDelegate = delegate of nativeint * nativeint * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CallCallback64Delegate = delegate of nativeint * nativeint * LONGLONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CallDSMProcDelegate = delegate of DTWAIN_IDENTITY * DTWAIN_IDENTITY * LONG * LONG * LONG * LPVOID -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CheckHandlesDelegate = delegate of DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ClearBuffersDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ClearErrorBufferDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ClearPDFTextDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ClearPageDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CloseSourceDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CloseSourceUIDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ConvertDIBToBitmapDelegate = delegate of HANDLE * HANDLE -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ConvertDIBToFullBitmapDelegate = delegate of HANDLE * DTWAIN_BOOL -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ConvertToAPIStringDelegate = delegate of string -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ConvertToAPIStringADelegate = delegate of string -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ConvertToAPIStringExDelegate = delegate of string * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ConvertToAPIStringExADelegate = delegate of string * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ConvertToAPIStringExWDelegate = delegate of string * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ConvertToAPIStringWDelegate = delegate of string -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CreateAcquisitionArrayDelegate = delegate of unit -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_CreatePDFTextElementDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_PDFTEXTELEMENT

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_DeleteDIBDelegate = delegate of HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_DestroyAcquisitionArrayDelegate = delegate of DTWAIN_ARRAY * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_DestroyPDFTextElementDelegate = delegate of DTWAIN_PDFTEXTELEMENT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_DisableAppWindowDelegate = delegate of HWND * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableAutoBorderDetectDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableAutoBrightDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableAutoDeskewDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableAutoFeedDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableAutoRotateDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableAutoScanDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableAutomaticSenseMediumDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableDuplexDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableFeederDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableIndicatorDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableJobFileHandlingDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableLampDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableMsgNotifyDelegate = delegate of DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnablePatchDetectDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnablePeekMessageLoopDelegate = delegate of DTWAIN_SOURCE * BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnablePrinterDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableThumbnailDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnableTripletsNotifyDelegate = delegate of DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EndThreadDelegate = delegate of DTWAIN_HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EndTwainSessionDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAlarmVolumesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAlarmVolumesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAlarmsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAlarmsExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAudioXferMechsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAudioXferMechsExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAutoFeedValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAutoFeedValuesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAutomaticCapturesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAutomaticCapturesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAutomaticSenseMediumDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumAutomaticSenseMediumExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumBitDepthsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumBitDepthsExDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumBitDepthsEx2Delegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumBottomCamerasDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumBottomCamerasExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumBrightnessValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumBrightnessValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCamerasDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCamerasExDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCamerasEx2Delegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCamerasEx3Delegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCompressionTypesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCompressionTypesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCompressionTypesEx2Delegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumContrastValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumContrastValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCustomCapsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumCustomCapsEx2Delegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumDoubleFeedDetectLengthsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumDoubleFeedDetectLengthsExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumDoubleFeedDetectValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumDoubleFeedDetectValuesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumExtImageInfoTypesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumExtImageInfoTypesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumExtendedCapsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumExtendedCapsExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumExtendedCapsEx2Delegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumFileTypeBitsPerPixelDelegate = delegate of LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumFileXferFormatsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumFileXferFormatsExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumHalftonesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumHalftonesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumHighlightValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumHighlightValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumJobControlsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumJobControlsExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumLightPathsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumLightPathsExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumLightSourcesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumLightSourcesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumMaxBuffersDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumMaxBuffersExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumNoiseFiltersDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumNoiseFiltersExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumOCRInterfacesDelegate = delegate of DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumOCRSupportedCapsDelegate = delegate of DTWAIN_OCRENGINE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumOrientationsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumOrientationsExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumOverscanValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumOverscanValuesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPaperSizesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPaperSizesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchCodesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchCodesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchMaxPrioritiesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchMaxPrioritiesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchMaxRetriesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchMaxRetriesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchPrioritiesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchPrioritiesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchSearchModesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchSearchModesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchTimeOutValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPatchTimeOutValuesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPixelTypesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPixelTypesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPrinterStringModesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumPrinterStringModesExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumResolutionValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumResolutionValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumShadowValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumShadowValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSourceUnitsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSourceUnitsExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSourceValuesDelegate = delegate of DTWAIN_SOURCE * string * DTWAIN_ARRAY byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_EnumSourceValuesADelegate = delegate of DTWAIN_SOURCE * string * DTWAIN_ARRAY byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_EnumSourceValuesWDelegate = delegate of DTWAIN_SOURCE * string * DTWAIN_ARRAY byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSourcesDelegate = delegate of DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSourcesExDelegate = delegate of unit -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedCapsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedCapsExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedCapsEx2Delegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedExtImageInfoDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedExtImageInfoExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedFileTypesDelegate = delegate of unit -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedMultiPageFileTypesDelegate = delegate of unit -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumSupportedSinglePageFileTypesDelegate = delegate of unit -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumThresholdValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumThresholdValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumTopCamerasDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumTopCamerasExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumTwainPrintersDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumTwainPrintersArrayDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumTwainPrintersArrayExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumTwainPrintersExDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumXResolutionValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumXResolutionValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumYResolutionValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref * DTWAIN_BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_EnumYResolutionValuesExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ExecuteOCRDelegate = delegate of DTWAIN_OCRENGINE * string * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_ExecuteOCRADelegate = delegate of DTWAIN_OCRENGINE * string * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_ExecuteOCRWDelegate = delegate of DTWAIN_OCRENGINE * string * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FeedPageDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FlipBitmapDelegate = delegate of HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FlushAcquiredPagesDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ForceAcquireBitDepthDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ForceScanOnNoUIDelegate = delegate of DTWAIN_SOURCE * BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameCreateDelegate = delegate of DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT -> DTWAIN_FRAME

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameCreateStringDelegate = delegate of string * string * string * string -> DTWAIN_FRAME

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_FrameCreateStringADelegate = delegate of string * string * string * string -> DTWAIN_FRAME

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_FrameCreateStringWDelegate = delegate of string * string * string * string -> DTWAIN_FRAME

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameDestroyDelegate = delegate of DTWAIN_FRAME -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameGetAllDelegate = delegate of DTWAIN_FRAME * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameGetAllStringDelegate = delegate of DTWAIN_FRAME * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_FrameGetAllStringADelegate = delegate of DTWAIN_FRAME * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_FrameGetAllStringWDelegate = delegate of DTWAIN_FRAME * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameGetValueDelegate = delegate of DTWAIN_FRAME * LONG * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameGetValueStringDelegate = delegate of DTWAIN_FRAME * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_FrameGetValueStringADelegate = delegate of DTWAIN_FRAME * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_FrameGetValueStringWDelegate = delegate of DTWAIN_FRAME * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameIsValidDelegate = delegate of DTWAIN_FRAME -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameSetAllDelegate = delegate of DTWAIN_FRAME * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameSetAllStringDelegate = delegate of DTWAIN_FRAME * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_FrameSetAllStringADelegate = delegate of DTWAIN_FRAME * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_FrameSetAllStringWDelegate = delegate of DTWAIN_FRAME * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameSetValueDelegate = delegate of DTWAIN_FRAME * LONG * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FrameSetValueStringDelegate = delegate of DTWAIN_FRAME * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_FrameSetValueStringADelegate = delegate of DTWAIN_FRAME * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_FrameSetValueStringWDelegate = delegate of DTWAIN_FRAME * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FreeExtImageInfoDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FreeMemoryDelegate = delegate of HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_FreeMemoryExDelegate = delegate of HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAPIHandleStatusDelegate = delegate of DTWAIN_HANDLE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireAreaDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireArea2Delegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireArea2StringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetAcquireArea2StringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetAcquireArea2StringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireAreaExDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireMetricsDelegate = delegate of DTWAIN_SOURCE * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireStripBufferDelegate = delegate of DTWAIN_SOURCE -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireStripDataDelegate = delegate of DTWAIN_SOURCE * int byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquireStripSizesDelegate = delegate of DTWAIN_SOURCE * DWORD byref * DWORD byref * DWORD byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquiredImageDelegate = delegate of DTWAIN_ARRAY * LONG * LONG -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAcquiredImageArrayDelegate = delegate of DTWAIN_ARRAY * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetActiveDSMPathDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetActiveDSMPathADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetActiveDSMPathWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetActiveDSMVersionInfoDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetActiveDSMVersionInfoADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetActiveDSMVersionInfoWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAlarmVolumeDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAllSourceDibsDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAppInfoDelegate = delegate of System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetAppInfoADelegate = delegate of System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetAppInfoWDelegate = delegate of System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetAuthorDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetAuthorADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetAuthorWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetBatteryMinutesDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetBatteryPercentDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetBitDepthDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetBlankPageAutoDetectionDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetBrightnessDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetBrightnessStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetBrightnessStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetBrightnessStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetBufferedTransferInfoDelegate = delegate of DTWAIN_SOURCE * DWORD byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref * DWORD byref -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCallbackDelegate = delegate of unit -> DTWAIN_CALLBACK_PROC

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCallback64Delegate = delegate of unit -> DTWAIN_CALLBACK_PROC64

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapArrayTypeDelegate = delegate of DTWAIN_SOURCE * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapContainerDelegate = delegate of DTWAIN_SOURCE * LONG * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapContainerExDelegate = delegate of LONG * DTWAIN_BOOL * DTWAIN_ARRAY byref -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapContainerEx2Delegate = delegate of LONG * DTWAIN_BOOL -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapDataTypeDelegate = delegate of DTWAIN_SOURCE * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapFromNameDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetCapFromNameADelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetCapFromNameWDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapOperationsDelegate = delegate of DTWAIN_SOURCE * LONG * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapValuesDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapValuesExDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCapValuesEx2Delegate = delegate of DTWAIN_SOURCE * LONG * LONG * LONG * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCaptionDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetCaptionADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetCaptionWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCompressionSizeDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCompressionTypeDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetConditionCodeStringDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetConditionCodeStringADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetConditionCodeStringWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetContrastDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetContrastStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetContrastStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetContrastStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCountryDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCurrentAcquiredImageDelegate = delegate of DTWAIN_SOURCE -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCurrentFileNameDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetCurrentFileNameADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetCurrentFileNameWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCurrentPageNumDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCurrentRetryCountDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCurrentTwainTripletDelegate = delegate of TW_IDENTITY byref * TW_IDENTITY byref * int byref * int byref * int byref * Int64 byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetCustomDSDataDelegate = delegate of DTWAIN_SOURCE * byte[] * DWORD * DWORD byref * LONG -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDSMFullNameDelegate = delegate of LONG * System.Text.StringBuilder * LONG * int byref -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetDSMFullNameADelegate = delegate of LONG * System.Text.StringBuilder * LONG * int byref -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetDSMFullNameWDelegate = delegate of LONG * System.Text.StringBuilder * LONG * int byref -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDSMSearchOrderDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDTWAINHandleDelegate = delegate of unit -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDeviceEventDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDeviceEventExDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDeviceEventInfoDelegate = delegate of DTWAIN_SOURCE * LONG * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDeviceNotificationsDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDeviceTimeDateDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetDeviceTimeDateADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetDeviceTimeDateWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDoubleFeedDetectLengthDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDoubleFeedDetectValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetDuplexTypeDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetErrorBufferDelegate = delegate of DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetErrorBufferThresholdDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetErrorCallbackDelegate = delegate of unit -> DTWAIN_ERROR_PROC

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetErrorCallback64Delegate = delegate of unit -> DTWAIN_ERROR_PROC64

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetErrorStringDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetErrorStringADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetErrorStringWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetExtCapFromNameDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetExtCapFromNameADelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetExtCapFromNameWDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetExtImageInfoDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetExtImageInfoDataDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetExtImageInfoDataExDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetExtImageInfoItemDelegate = delegate of DTWAIN_SOURCE * LONG * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetExtImageInfoItemExDelegate = delegate of DTWAIN_SOURCE * LONG * int byref * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetExtNameFromCapDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetExtNameFromCapADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetExtNameFromCapWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetFeederAlignmentDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetFeederFuncsDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetFeederOrderDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetFeederWaitTimeDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetFileCompressionTypeDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetFileTypeExtensionsDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetFileTypeExtensionsADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetFileTypeExtensionsWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetFileTypeNameDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetFileTypeNameADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetFileTypeNameWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetHalftoneDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetHalftoneADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetHalftoneWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetHighlightDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetHighlightStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetHighlightStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetHighlightStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetImageInfoDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * int byref * int byref * int byref * DTWAIN_ARRAY byref * int byref * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetImageInfoStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * System.Text.StringBuilder * int byref * int byref * int byref * DTWAIN_ARRAY byref * int byref * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetImageInfoStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * System.Text.StringBuilder * int byref * int byref * int byref * DTWAIN_ARRAY byref * int byref * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetImageInfoStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * System.Text.StringBuilder * int byref * int byref * int byref * DTWAIN_ARRAY byref * int byref * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetJobControlDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetJpegValuesDelegate = delegate of DTWAIN_SOURCE * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetJpegXRValuesDelegate = delegate of DTWAIN_SOURCE * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLanguageDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLastErrorDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLibraryPathDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetLibraryPathADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetLibraryPathWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLightPathDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLightSourceDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLightSourcesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLoggerCallbackDelegate = delegate of unit -> DTWAIN_LOGGER_PROC

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLoggerCallbackADelegate = delegate of unit -> DTWAIN_LOGGER_PROCA

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetLoggerCallbackWDelegate = delegate of unit -> DTWAIN_LOGGER_PROCW

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetManualDuplexCountDelegate = delegate of DTWAIN_SOURCE * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetMaxAcquisitionsDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetMaxBuffersDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetMaxPagesToAcquireDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetMaxRetryAttemptsDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetNameFromCapDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetNameFromCapADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetNameFromCapWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetNoiseFilterDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetNumAcquiredImagesDelegate = delegate of DTWAIN_ARRAY * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetNumAcquisitionsDelegate = delegate of DTWAIN_ARRAY -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRCapValuesDelegate = delegate of DTWAIN_OCRENGINE * LONG * LONG * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRErrorStringDelegate = delegate of DTWAIN_OCRENGINE * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetOCRErrorStringADelegate = delegate of DTWAIN_OCRENGINE * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetOCRErrorStringWDelegate = delegate of DTWAIN_OCRENGINE * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRLastErrorDelegate = delegate of DTWAIN_OCRENGINE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRMajorMinorVersionDelegate = delegate of DTWAIN_OCRENGINE * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRManufacturerDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetOCRManufacturerADelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetOCRManufacturerWDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRProductFamilyDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetOCRProductFamilyADelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetOCRProductFamilyWDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRProductNameDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetOCRProductNameADelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetOCRProductNameWDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRTextDelegate = delegate of DTWAIN_OCRENGINE * LONG * System.Text.StringBuilder * LONG * int byref * LONG -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetOCRTextADelegate = delegate of DTWAIN_OCRENGINE * LONG * System.Text.StringBuilder * LONG * int byref * LONG -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRTextInfoFloatDelegate = delegate of DTWAIN_OCRTEXTINFOHANDLE * LONG * LONG * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRTextInfoFloatExDelegate = delegate of DTWAIN_OCRTEXTINFOHANDLE * LONG * DTWAIN_FLOAT byref * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRTextInfoHandleDelegate = delegate of DTWAIN_OCRENGINE * LONG -> DTWAIN_OCRTEXTINFOHANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRTextInfoLongDelegate = delegate of DTWAIN_OCRTEXTINFOHANDLE * LONG * LONG * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRTextInfoLongExDelegate = delegate of DTWAIN_OCRTEXTINFOHANDLE * LONG * int byref * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetOCRTextWDelegate = delegate of DTWAIN_OCRENGINE * LONG * System.Text.StringBuilder * LONG * int byref * LONG -> HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOCRVersionInfoDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetOCRVersionInfoADelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetOCRVersionInfoWDelegate = delegate of DTWAIN_OCRENGINE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOrientationDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetOverscanDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPDFTextElementFloatDelegate = delegate of DTWAIN_PDFTEXTELEMENT * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPDFTextElementLongDelegate = delegate of DTWAIN_PDFTEXTELEMENT * int byref * int byref * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPDFTextElementStringDelegate = delegate of DTWAIN_PDFTEXTELEMENT * System.Text.StringBuilder * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetPDFTextElementStringADelegate = delegate of DTWAIN_PDFTEXTELEMENT * System.Text.StringBuilder * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetPDFTextElementStringWDelegate = delegate of DTWAIN_PDFTEXTELEMENT * System.Text.StringBuilder * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPDFType1FontNameDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetPDFType1FontNameADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetPDFType1FontNameWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPaperSizeDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPaperSizeNameDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetPaperSizeNameADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetPaperSizeNameWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPatchMaxPrioritiesDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPatchMaxRetriesDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPatchPrioritiesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPatchSearchModeDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPatchTimeOutDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPixelFlavorDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPixelTypeDelegate = delegate of DTWAIN_SOURCE * int byref * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPrinterDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPrinterStartNumberDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPrinterStringModeDelegate = delegate of DTWAIN_SOURCE * int byref * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPrinterStringsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetPrinterSuffixStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetPrinterSuffixStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetPrinterSuffixStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetRegisteredMsgDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetResolutionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetResolutionStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetResolutionStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetResolutionStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetResourceStringDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetResourceStringADelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetResourceStringWDelegate = delegate of LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetRotationDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetRotationStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetRotationStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetRotationStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSaveFileNameDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetSaveFileNameADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetSaveFileNameWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSavedFilesCountDelegate = delegate of DTWAIN_SOURCE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSessionDetailsDelegate = delegate of System.Text.StringBuilder * LONG * LONG * BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetSessionDetailsADelegate = delegate of System.Text.StringBuilder * LONG * LONG * BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetSessionDetailsWDelegate = delegate of System.Text.StringBuilder * LONG * LONG * BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetShadowDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetShadowStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetShadowStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetShadowStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetShortVersionStringDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetShortVersionStringADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetShortVersionStringWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceAcquisitionsDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceDetailsDelegate = delegate of string * System.Text.StringBuilder * LONG * LONG * BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetSourceDetailsADelegate = delegate of string * System.Text.StringBuilder * LONG * LONG * BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetSourceDetailsWDelegate = delegate of string * System.Text.StringBuilder * LONG * LONG * BOOL -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceIDDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_IDENTITY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceIDExDelegate = delegate of DTWAIN_SOURCE * TW_IDENTITY byref -> TW_IDENTITY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceManufacturerDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetSourceManufacturerADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetSourceManufacturerWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceProductFamilyDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetSourceProductFamilyADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetSourceProductFamilyWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceProductNameDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetSourceProductNameADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetSourceProductNameWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceUnitDelegate = delegate of DTWAIN_SOURCE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceVersionInfoDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetSourceVersionInfoADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetSourceVersionInfoWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetSourceVersionNumberDelegate = delegate of DTWAIN_SOURCE * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetStaticLibVersionDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTempFileDirectoryDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTempFileDirectoryADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTempFileDirectoryWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetThresholdDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetThresholdStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetThresholdStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetThresholdStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTimeDateDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTimeDateADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTimeDateWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainAppIDDelegate = delegate of unit -> DTWAIN_IDENTITY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainAppIDExDelegate = delegate of TW_IDENTITY byref -> TW_IDENTITY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainAvailabilityDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainAvailabilityExDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainAvailabilityExADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainAvailabilityExWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainCountryNameDelegate = delegate of LONG * System.Text.StringBuilder -> BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainCountryNameADelegate = delegate of LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainCountryNameWDelegate = delegate of LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainCountryValueDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainCountryValueADelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainCountryValueWDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainHwndDelegate = delegate of unit -> HWND

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainIDFromNameDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainIDFromNameADelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainIDFromNameWDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainLanguageNameDelegate = delegate of LONG * System.Text.StringBuilder -> BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainLanguageNameADelegate = delegate of LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainLanguageNameWDelegate = delegate of LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainLanguageValueDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainLanguageValueADelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainLanguageValueWDelegate = delegate of string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainModeDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainNameFromConstantDelegate = delegate of LONG * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainNameFromConstantADelegate = delegate of LONG * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainNameFromConstantWDelegate = delegate of LONG * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainStringNameDelegate = delegate of LONG * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetTwainStringNameADelegate = delegate of LONG * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetTwainStringNameWDelegate = delegate of LONG * LONG * System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetTwainTimeoutDelegate = delegate of unit -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetVersionDelegate = delegate of int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetVersionCopyrightDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetVersionCopyrightADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetVersionCopyrightWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetVersionExDelegate = delegate of int byref * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetVersionInfoDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetVersionInfoADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetVersionInfoWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetVersionStringDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetVersionStringADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetVersionStringWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetWindowsVersionInfoDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetWindowsVersionInfoADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetWindowsVersionInfoWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetXResolutionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetXResolutionStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetXResolutionStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetXResolutionStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetYResolutionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_GetYResolutionStringDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_GetYResolutionStringADelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_GetYResolutionStringWDelegate = delegate of DTWAIN_SOURCE * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_InitExtImageInfoDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_InitImageFileAppendDelegate = delegate of string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_InitImageFileAppendADelegate = delegate of string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_InitImageFileAppendWDelegate = delegate of string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_InitOCRInterfaceDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAcquiringDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAudioXferSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoBorderDetectEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoBorderDetectSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoBrightEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoBrightSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoDeskewEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoDeskewSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoFeedEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoFeedSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoRotateEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoRotateSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutoScanEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutomaticSenseMediumEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsAutomaticSenseMediumSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsBlankPageDetectionOnDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsBufferedTileModeOnDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsBufferedTileModeSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsCapSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsCompressionSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsCustomDSDataSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDIBBlankDelegate = delegate of HANDLE * DTWAIN_FLOAT -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDIBBlankStringDelegate = delegate of HANDLE * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_IsDIBBlankStringADelegate = delegate of HANDLE * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_IsDIBBlankStringWDelegate = delegate of HANDLE * string -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDeviceEventSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDeviceOnLineDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDoubleFeedDetectLengthSupportedDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDoubleFeedDetectSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDoublePageCountOnDuplexDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDuplexEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsDuplexSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsExtImageInfoSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsFeederEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsFeederLoadedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsFeederSensitiveDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsFeederSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsFileSystemSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsFileXferSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldALastPageSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldALevelSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldAPrintFormatSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldAValueSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldBLastPageSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldBLevelSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldBPrintFormatSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldBValueSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldCLastPageSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldCLevelSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldCPrintFormatSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldCValueSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldDLastPageSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldDLevelSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldDPrintFormatSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldDValueSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldELastPageSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldELevelSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldEPrintFormatSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIAFieldEValueSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsImageAddressingSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIndicatorEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsIndicatorSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsInitializedDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsJPEGSupportedDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsJobControlSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsLampEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsLampSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsLightPathSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsLightSourceSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsMaxBuffersSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsMemFileXferSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsMsgNotifyEnabledDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsNotifyTripletsEnabledDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsOCREngineActivatedDelegate = delegate of DTWAIN_OCRENGINE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsOpenSourcesOnSelectDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsOrientationSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsOverscanSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPDFSupportedDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPNGSupportedDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPaperDetectableDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPaperSizeSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPatchCapsSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPatchDetectEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPatchSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPeekMessageLoopEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPixelTypeSupportedDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPrinterEnabledDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsPrinterSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsRotationSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSessionEnabledDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSkipImageInfoErrorDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSourceAcquiringDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSourceAcquiringExDelegate = delegate of DTWAIN_SOURCE * BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSourceInUIOnlyModeDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSourceOpenDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSourceSelectedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsSourceValidDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsTIFFSupportedDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsThumbnailEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsThumbnailSupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsTwainAvailableDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsTwainAvailableExDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_IsTwainAvailableExADelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_IsTwainAvailableExWDelegate = delegate of System.Text.StringBuilder * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsTwainMsgDelegate = delegate of MSG byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsUIControllableDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsUIEnabledDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_IsUIOnlySupportedDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_LoadCustomStringResourcesDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_LoadCustomStringResourcesADelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_LoadCustomStringResourcesExDelegate = delegate of string * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_LoadCustomStringResourcesExADelegate = delegate of string * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_LoadCustomStringResourcesExWDelegate = delegate of string * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_LoadCustomStringResourcesWDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_LoadLanguageResourceDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_LockMemoryDelegate = delegate of HANDLE -> DTWAIN_MEMORY_PTR

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_LockMemoryExDelegate = delegate of HANDLE -> DTWAIN_MEMORY_PTR

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_LogMessageDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_LogMessageADelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_LogMessageWDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_MakeRGBDelegate = delegate of LONG * LONG * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_OpenSourceDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_OpenSourcesOnSelectDelegate = delegate of DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeCreateDelegate = delegate of LONG -> DTWAIN_RANGE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeCreateFromCapDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_RANGE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeDestroyDelegate = delegate of DTWAIN_RANGE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeExpandDelegate = delegate of DTWAIN_RANGE * DTWAIN_ARRAY byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeExpandExDelegate = delegate of DTWAIN_RANGE -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetAllDelegate = delegate of DTWAIN_RANGE * LPVOID * LPVOID * LPVOID * LPVOID * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetAllFloatDelegate = delegate of DTWAIN_RANGE * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetAllFloatStringDelegate = delegate of DTWAIN_RANGE * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_RangeGetAllFloatStringADelegate = delegate of DTWAIN_RANGE * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_RangeGetAllFloatStringWDelegate = delegate of DTWAIN_RANGE * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetAllLongDelegate = delegate of DTWAIN_RANGE * int byref * int byref * int byref * int byref * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetCountDelegate = delegate of DTWAIN_RANGE -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetExpValueDelegate = delegate of DTWAIN_RANGE * LONG * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetExpValueFloatDelegate = delegate of DTWAIN_RANGE * LONG * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetExpValueFloatStringDelegate = delegate of DTWAIN_RANGE * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_RangeGetExpValueFloatStringADelegate = delegate of DTWAIN_RANGE * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_RangeGetExpValueFloatStringWDelegate = delegate of DTWAIN_RANGE * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetExpValueLongDelegate = delegate of DTWAIN_RANGE * LONG * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetNearestValueDelegate = delegate of DTWAIN_RANGE * LPVOID * LPVOID * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetPosDelegate = delegate of DTWAIN_RANGE * LPVOID * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetPosFloatDelegate = delegate of DTWAIN_RANGE * DTWAIN_FLOAT * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetPosFloatStringDelegate = delegate of DTWAIN_RANGE * string * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_RangeGetPosFloatStringADelegate = delegate of DTWAIN_RANGE * string * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_RangeGetPosFloatStringWDelegate = delegate of DTWAIN_RANGE * string * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetPosLongDelegate = delegate of DTWAIN_RANGE * LONG * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetValueDelegate = delegate of DTWAIN_RANGE * LONG * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetValueFloatDelegate = delegate of DTWAIN_RANGE * LONG * DTWAIN_FLOAT byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetValueFloatStringDelegate = delegate of DTWAIN_RANGE * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_RangeGetValueFloatStringADelegate = delegate of DTWAIN_RANGE * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_RangeGetValueFloatStringWDelegate = delegate of DTWAIN_RANGE * LONG * System.Text.StringBuilder -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeGetValueLongDelegate = delegate of DTWAIN_RANGE * LONG * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeIsValidDelegate = delegate of DTWAIN_RANGE * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeNearestValueFloatDelegate = delegate of DTWAIN_RANGE * DTWAIN_FLOAT * DTWAIN_FLOAT byref * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeNearestValueFloatStringDelegate = delegate of DTWAIN_RANGE * string * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_RangeNearestValueFloatStringADelegate = delegate of DTWAIN_RANGE * string * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_RangeNearestValueFloatStringWDelegate = delegate of DTWAIN_RANGE * string * System.Text.StringBuilder * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeNearestValueLongDelegate = delegate of DTWAIN_RANGE * LONG * int byref * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetAllDelegate = delegate of DTWAIN_RANGE * LPVOID * LPVOID * LPVOID * LPVOID * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetAllFloatDelegate = delegate of DTWAIN_RANGE * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetAllFloatStringDelegate = delegate of DTWAIN_RANGE * string * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_RangeSetAllFloatStringADelegate = delegate of DTWAIN_RANGE * string * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_RangeSetAllFloatStringWDelegate = delegate of DTWAIN_RANGE * string * string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetAllLongDelegate = delegate of DTWAIN_RANGE * LONG * LONG * LONG * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetValueDelegate = delegate of DTWAIN_RANGE * LONG * LPVOID -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetValueFloatDelegate = delegate of DTWAIN_RANGE * LONG * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetValueFloatStringDelegate = delegate of DTWAIN_RANGE * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_RangeSetValueFloatStringADelegate = delegate of DTWAIN_RANGE * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_RangeSetValueFloatStringWDelegate = delegate of DTWAIN_RANGE * LONG * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RangeSetValueLongDelegate = delegate of DTWAIN_RANGE * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ResetPDFTextElementDelegate = delegate of DTWAIN_PDFTEXTELEMENT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_RewindPageDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectDefaultOCREngineDelegate = delegate of unit -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectDefaultSourceDelegate = delegate of unit -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectDefaultSourceWithOpenDelegate = delegate of DTWAIN_BOOL -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectOCREngineDelegate = delegate of unit -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectOCREngine2Delegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SelectOCREngine2ADelegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectOCREngine2ExDelegate = delegate of HWND * string * LONG * LONG * string * string * string * LONG -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SelectOCREngine2ExADelegate = delegate of HWND * string * LONG * LONG * string * string * string * LONG -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SelectOCREngine2ExWDelegate = delegate of HWND * string * LONG * LONG * string * string * string * LONG -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SelectOCREngine2WDelegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectOCREngineByNameDelegate = delegate of string -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SelectOCREngineByNameADelegate = delegate of string -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SelectOCREngineByNameWDelegate = delegate of string -> DTWAIN_OCRENGINE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectSourceDelegate = delegate of unit -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectSource2Delegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SelectSource2ADelegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectSource2ExDelegate = delegate of HWND * string * LONG * LONG * string * string * string * LONG -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SelectSource2ExADelegate = delegate of HWND * string * LONG * LONG * string * string * string * LONG -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SelectSource2ExWDelegate = delegate of HWND * string * LONG * LONG * string * string * string * LONG -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SelectSource2WDelegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectSourceByNameDelegate = delegate of string -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SelectSourceByNameADelegate = delegate of string -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SelectSourceByNameWDelegate = delegate of string -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectSourceByNameWithOpenDelegate = delegate of string * DTWAIN_BOOL -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SelectSourceByNameWithOpenADelegate = delegate of string * DTWAIN_BOOL -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SelectSourceByNameWithOpenWDelegate = delegate of string * DTWAIN_BOOL -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SelectSourceWithOpenDelegate = delegate of DTWAIN_BOOL -> DTWAIN_SOURCE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireAreaDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_ARRAY * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireArea2Delegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * DTWAIN_FLOAT * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireArea2StringDelegate = delegate of DTWAIN_SOURCE * string * string * string * string * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetAcquireArea2StringADelegate = delegate of DTWAIN_SOURCE * string * string * string * string * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetAcquireArea2StringWDelegate = delegate of DTWAIN_SOURCE * string * string * string * string * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireImageNegativeDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireImageScaleDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireImageScaleStringDelegate = delegate of DTWAIN_SOURCE * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetAcquireImageScaleStringADelegate = delegate of DTWAIN_SOURCE * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetAcquireImageScaleStringWDelegate = delegate of DTWAIN_SOURCE * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireStripBufferDelegate = delegate of DTWAIN_SOURCE * HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAcquireStripSizeDelegate = delegate of DTWAIN_SOURCE * DWORD -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAlarmVolumeDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAlarmsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAllCapsToDefaultDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAppInfoDelegate = delegate of string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetAppInfoADelegate = delegate of string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetAppInfoWDelegate = delegate of string * string * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAuthorDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetAuthorADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetAuthorWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAvailablePrintersDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetAvailablePrintersArrayDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBitDepthDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBlankPageDetectionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBlankPageDetectionExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT * LONG * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBlankPageDetectionExStringDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetBlankPageDetectionExStringADelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetBlankPageDetectionExStringWDelegate = delegate of DTWAIN_SOURCE * string * LONG * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBlankPageDetectionStringDelegate = delegate of DTWAIN_SOURCE * string * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetBlankPageDetectionStringADelegate = delegate of DTWAIN_SOURCE * string * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetBlankPageDetectionStringWDelegate = delegate of DTWAIN_SOURCE * string * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBrightnessDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBrightnessStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetBrightnessStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetBrightnessStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetBufferedTileModeDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCameraDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetCameraADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetCameraWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCapValuesDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCapValuesExDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * LONG * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCapValuesEx2Delegate = delegate of DTWAIN_SOURCE * LONG * LONG * LONG * LONG * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCaptionDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetCaptionADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetCaptionWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCompressionTypeDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetContrastDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetContrastStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetContrastStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetContrastStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCountryDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCurrentRetryCountDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetCustomDSDataDelegate = delegate of DTWAIN_SOURCE * HANDLE * byte[] * DWORD * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDSMSearchOrderDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDSMSearchOrderExDelegate = delegate of string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetDSMSearchOrderExADelegate = delegate of string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetDSMSearchOrderExWDelegate = delegate of string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDefaultSourceDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDeviceNotificationsDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDeviceTimeDateDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetDeviceTimeDateADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetDeviceTimeDateWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDoubleFeedDetectLengthDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDoubleFeedDetectLengthStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetDoubleFeedDetectLengthStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetDoubleFeedDetectLengthStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDoubleFeedDetectValuesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetDoublePageCountOnDuplexDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetEOJDetectValueDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetErrorBufferThresholdDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetFeederAlignmentDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetFeederOrderDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetFeederWaitTimeDelegate = delegate of DTWAIN_SOURCE * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetFileAutoIncrementDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetFileCompressionTypeDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetFileSavePosDelegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetFileSavePosADelegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetFileSavePosWDelegate = delegate of HWND * string * LONG * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetFileXferFormatDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetHalftoneDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetHalftoneADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetHalftoneWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetHighlightDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetHighlightStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetHighlightStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetHighlightStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetJobControlDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetJpegValuesDelegate = delegate of DTWAIN_SOURCE * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetJpegXRValuesDelegate = delegate of DTWAIN_SOURCE * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetLanguageDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetLastErrorDelegate = delegate of LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetLightPathDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetLightPathExDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetLightSourceDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetLightSourcesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetManualDuplexModeDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetMaxAcquisitionsDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetMaxBuffersDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetMaxRetryAttemptsDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetMultipageScanModeDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetNoiseFilterDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetOCRCapValuesDelegate = delegate of DTWAIN_OCRENGINE * LONG * LONG * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetOrientationDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetOverscanDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFAESEncryptionDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFASCIICompressionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFAuthorDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFAuthorADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFAuthorWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFCompressionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFCreatorDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFCreatorADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFCreatorWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFEncryptionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL * string * string * DWORD * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFEncryptionADelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL * string * string * DWORD * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFEncryptionWDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL * string * string * DWORD * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFJpegQualityDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFKeywordsDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFKeywordsADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFKeywordsWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFOCRConversionDelegate = delegate of DTWAIN_OCRENGINE * LONG * LONG * LONG * LONG * LONG -> LONG

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFOCRModeDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFOrientationDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFPageScaleDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_FLOAT * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFPageScaleStringDelegate = delegate of DTWAIN_SOURCE * LONG * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFPageScaleStringADelegate = delegate of DTWAIN_SOURCE * LONG * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFPageScaleStringWDelegate = delegate of DTWAIN_SOURCE * LONG * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFPageSizeDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_FLOAT * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFPageSizeStringDelegate = delegate of DTWAIN_SOURCE * LONG * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFPageSizeStringADelegate = delegate of DTWAIN_SOURCE * LONG * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFPageSizeStringWDelegate = delegate of DTWAIN_SOURCE * LONG * string * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFPolarityDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFProducerDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFProducerADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFProducerWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFSubjectDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFSubjectADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFSubjectWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFTextElementFloatDelegate = delegate of DTWAIN_PDFTEXTELEMENT * DTWAIN_FLOAT * DTWAIN_FLOAT * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFTextElementLongDelegate = delegate of DTWAIN_PDFTEXTELEMENT * LONG * LONG * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFTextElementStringDelegate = delegate of DTWAIN_PDFTEXTELEMENT * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFTextElementStringADelegate = delegate of DTWAIN_PDFTEXTELEMENT * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFTextElementStringWDelegate = delegate of DTWAIN_PDFTEXTELEMENT * string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPDFTitleDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPDFTitleADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPDFTitleWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPaperSizeDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPatchMaxPrioritiesDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPatchMaxRetriesDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPatchPrioritiesDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPatchSearchModeDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPatchTimeOutDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPixelFlavorDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPixelTypeDelegate = delegate of DTWAIN_SOURCE * LONG * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPostScriptTitleDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPostScriptTitleADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPostScriptTitleWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPostScriptTypeDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPrinterDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPrinterExDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPrinterStartNumberDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPrinterStringModeDelegate = delegate of DTWAIN_SOURCE * LONG * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPrinterStringsDelegate = delegate of DTWAIN_SOURCE * DTWAIN_ARRAY * int byref -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetPrinterSuffixStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetPrinterSuffixStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetPrinterSuffixStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetQueryCapSupportDelegate = delegate of DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetResolutionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetResolutionStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetResolutionStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetResolutionStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetResourcePathDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetResourcePathADelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetResourcePathWDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetRotationDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetRotationStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetRotationStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetRotationStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetSaveFileNameDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetSaveFileNameADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetSaveFileNameWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetShadowDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetShadowStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetShadowStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetShadowStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetSourceUnitDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTIFFCompressTypeDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTIFFInvertDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTempFileDirectoryDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetTempFileDirectoryADelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTempFileDirectoryExDelegate = delegate of string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetTempFileDirectoryExADelegate = delegate of string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetTempFileDirectoryExWDelegate = delegate of string * LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetTempFileDirectoryWDelegate = delegate of string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetThresholdDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetThresholdStringDelegate = delegate of DTWAIN_SOURCE * string * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetThresholdStringADelegate = delegate of DTWAIN_SOURCE * string * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetThresholdStringWDelegate = delegate of DTWAIN_SOURCE * string * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTwainDSMDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTwainLogDelegate = delegate of DWORD * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetTwainLogADelegate = delegate of DWORD * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetTwainLogWDelegate = delegate of DWORD * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTwainModeDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetTwainTimeoutDelegate = delegate of LONG -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetXResolutionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetXResolutionStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetXResolutionStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetXResolutionStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetYResolutionDelegate = delegate of DTWAIN_SOURCE * DTWAIN_FLOAT -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SetYResolutionStringDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SetYResolutionStringADelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SetYResolutionStringWDelegate = delegate of DTWAIN_SOURCE * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ShowUIOnlyDelegate = delegate of DTWAIN_SOURCE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_ShutdownOCREngineDelegate = delegate of DTWAIN_OCRENGINE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SkipImageInfoErrorDelegate = delegate of DTWAIN_SOURCE * DTWAIN_BOOL -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_StartThreadDelegate = delegate of DTWAIN_HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_StartTwainSessionDelegate = delegate of HWND * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_StartTwainSessionADelegate = delegate of HWND * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_StartTwainSessionWDelegate = delegate of HWND * string -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysDestroyDelegate = delegate of unit -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysInitializeDelegate = delegate of unit -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysInitializeExDelegate = delegate of string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysInitializeEx2Delegate = delegate of string * string * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SysInitializeEx2ADelegate = delegate of string * string * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SysInitializeEx2WDelegate = delegate of string * string * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SysInitializeExADelegate = delegate of string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SysInitializeExWDelegate = delegate of string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysInitializeLibDelegate = delegate of HINSTANCE -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysInitializeLibExDelegate = delegate of HINSTANCE * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysInitializeLibEx2Delegate = delegate of HINSTANCE * string * string * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SysInitializeLibEx2ADelegate = delegate of HINSTANCE * string * string * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SysInitializeLibEx2WDelegate = delegate of HINSTANCE * string * string * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)>]
    type DTWAIN_SysInitializeLibExADelegate = delegate of HINSTANCE * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)>]
    type DTWAIN_SysInitializeLibExWDelegate = delegate of HINSTANCE * string -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_SysInitializeNoBlockingDelegate = delegate of unit -> DTWAIN_HANDLE

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_TestGetCapDelegate = delegate of DTWAIN_SOURCE * LONG -> DTWAIN_ARRAY

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_UnlockMemoryDelegate = delegate of HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_UnlockMemoryExDelegate = delegate of HANDLE -> DTWAIN_BOOL

    [<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)>]
    type DTWAIN_UseMultipleThreadsDelegate = delegate of DTWAIN_BOOL -> DTWAIN_BOOL
    let private AcquireAudioFile = lazy (DynamicDll.Bind "DTWAIN_AcquireAudioFile" : DTWAIN_AcquireAudioFileDelegate)
    let private AcquireAudioFileA = lazy (DynamicDll.Bind "DTWAIN_AcquireAudioFileA" : DTWAIN_AcquireAudioFileADelegate)
    let private AcquireAudioFileW = lazy (DynamicDll.Bind "DTWAIN_AcquireAudioFileW" : DTWAIN_AcquireAudioFileWDelegate)
    let private AcquireAudioNative = lazy (DynamicDll.Bind "DTWAIN_AcquireAudioNative" : DTWAIN_AcquireAudioNativeDelegate)
    let private AcquireAudioNativeEx = lazy (DynamicDll.Bind "DTWAIN_AcquireAudioNativeEx" : DTWAIN_AcquireAudioNativeExDelegate)
    let private AcquireBuffered = lazy (DynamicDll.Bind "DTWAIN_AcquireBuffered" : DTWAIN_AcquireBufferedDelegate)
    let private AcquireBufferedEx = lazy (DynamicDll.Bind "DTWAIN_AcquireBufferedEx" : DTWAIN_AcquireBufferedExDelegate)
    let private AcquireFile = lazy (DynamicDll.Bind "DTWAIN_AcquireFile" : DTWAIN_AcquireFileDelegate)
    let private AcquireFileA = lazy (DynamicDll.Bind "DTWAIN_AcquireFileA" : DTWAIN_AcquireFileADelegate)
    let private AcquireFileEx = lazy (DynamicDll.Bind "DTWAIN_AcquireFileEx" : DTWAIN_AcquireFileExDelegate)
    let private AcquireFileW = lazy (DynamicDll.Bind "DTWAIN_AcquireFileW" : DTWAIN_AcquireFileWDelegate)
    let private AcquireNative = lazy (DynamicDll.Bind "DTWAIN_AcquireNative" : DTWAIN_AcquireNativeDelegate)
    let private AcquireNativeEx = lazy (DynamicDll.Bind "DTWAIN_AcquireNativeEx" : DTWAIN_AcquireNativeExDelegate)
    let private AcquireToClipboard = lazy (DynamicDll.Bind "DTWAIN_AcquireToClipboard" : DTWAIN_AcquireToClipboardDelegate)
    let private AddExtImageInfoQuery = lazy (DynamicDll.Bind "DTWAIN_AddExtImageInfoQuery" : DTWAIN_AddExtImageInfoQueryDelegate)
    let private AddFileToAppend = lazy (DynamicDll.Bind "DTWAIN_AddFileToAppend" : DTWAIN_AddFileToAppendDelegate)
    let private AddFileToAppendA = lazy (DynamicDll.Bind "DTWAIN_AddFileToAppendA" : DTWAIN_AddFileToAppendADelegate)
    let private AddFileToAppendW = lazy (DynamicDll.Bind "DTWAIN_AddFileToAppendW" : DTWAIN_AddFileToAppendWDelegate)
    let private AddPDFText = lazy (DynamicDll.Bind "DTWAIN_AddPDFText" : DTWAIN_AddPDFTextDelegate)
    let private AddPDFTextA = lazy (DynamicDll.Bind "DTWAIN_AddPDFTextA" : DTWAIN_AddPDFTextADelegate)
    let private AddPDFTextEx = lazy (DynamicDll.Bind "DTWAIN_AddPDFTextEx" : DTWAIN_AddPDFTextExDelegate)
    let private AddPDFTextW = lazy (DynamicDll.Bind "DTWAIN_AddPDFTextW" : DTWAIN_AddPDFTextWDelegate)
    let private AllocateMemory = lazy (DynamicDll.Bind "DTWAIN_AllocateMemory" : DTWAIN_AllocateMemoryDelegate)
    let private AllocateMemory64 = lazy (DynamicDll.Bind "DTWAIN_AllocateMemory64" : DTWAIN_AllocateMemory64Delegate)
    let private AllocateMemoryEx = lazy (DynamicDll.Bind "DTWAIN_AllocateMemoryEx" : DTWAIN_AllocateMemoryExDelegate)
    let private AppHandlesExceptions = lazy (DynamicDll.Bind "DTWAIN_AppHandlesExceptions" : DTWAIN_AppHandlesExceptionsDelegate)
    let private ArrayANSIStringToFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayANSIStringToFloat" : DTWAIN_ArrayANSIStringToFloatDelegate)
    let private ArrayAdd = lazy (DynamicDll.Bind "DTWAIN_ArrayAdd" : DTWAIN_ArrayAddDelegate)
    let private ArrayAddANSIString = lazy (DynamicDll.Bind "DTWAIN_ArrayAddANSIString" : DTWAIN_ArrayAddANSIStringDelegate)
    let private ArrayAddANSIStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddANSIStringN" : DTWAIN_ArrayAddANSIStringNDelegate)
    let private ArrayAddFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloat" : DTWAIN_ArrayAddFloatDelegate)
    let private ArrayAddFloatN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloatN" : DTWAIN_ArrayAddFloatNDelegate)
    let private ArrayAddFloatString = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloatString" : DTWAIN_ArrayAddFloatStringDelegate)
    let private ArrayAddFloatStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloatStringA" : DTWAIN_ArrayAddFloatStringADelegate)
    let private ArrayAddFloatStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloatStringN" : DTWAIN_ArrayAddFloatStringNDelegate)
    let private ArrayAddFloatStringNA = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloatStringNA" : DTWAIN_ArrayAddFloatStringNADelegate)
    let private ArrayAddFloatStringNW = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloatStringNW" : DTWAIN_ArrayAddFloatStringNWDelegate)
    let private ArrayAddFloatStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFloatStringW" : DTWAIN_ArrayAddFloatStringWDelegate)
    let private ArrayAddFrame = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFrame" : DTWAIN_ArrayAddFrameDelegate)
    let private ArrayAddFrameN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddFrameN" : DTWAIN_ArrayAddFrameNDelegate)
    let private ArrayAddLong = lazy (DynamicDll.Bind "DTWAIN_ArrayAddLong" : DTWAIN_ArrayAddLongDelegate)
    let private ArrayAddLong64 = lazy (DynamicDll.Bind "DTWAIN_ArrayAddLong64" : DTWAIN_ArrayAddLong64Delegate)
    let private ArrayAddLong64N = lazy (DynamicDll.Bind "DTWAIN_ArrayAddLong64N" : DTWAIN_ArrayAddLong64NDelegate)
    let private ArrayAddLongN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddLongN" : DTWAIN_ArrayAddLongNDelegate)
    let private ArrayAddN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddN" : DTWAIN_ArrayAddNDelegate)
    let private ArrayAddString = lazy (DynamicDll.Bind "DTWAIN_ArrayAddString" : DTWAIN_ArrayAddStringDelegate)
    let private ArrayAddStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayAddStringA" : DTWAIN_ArrayAddStringADelegate)
    let private ArrayAddStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddStringN" : DTWAIN_ArrayAddStringNDelegate)
    let private ArrayAddStringNA = lazy (DynamicDll.Bind "DTWAIN_ArrayAddStringNA" : DTWAIN_ArrayAddStringNADelegate)
    let private ArrayAddStringNW = lazy (DynamicDll.Bind "DTWAIN_ArrayAddStringNW" : DTWAIN_ArrayAddStringNWDelegate)
    let private ArrayAddStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayAddStringW" : DTWAIN_ArrayAddStringWDelegate)
    let private ArrayAddWideString = lazy (DynamicDll.Bind "DTWAIN_ArrayAddWideString" : DTWAIN_ArrayAddWideStringDelegate)
    let private ArrayAddWideStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayAddWideStringN" : DTWAIN_ArrayAddWideStringNDelegate)
    let private ArrayConvertFix32ToFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayConvertFix32ToFloat" : DTWAIN_ArrayConvertFix32ToFloatDelegate)
    let private ArrayConvertFloatToFix32 = lazy (DynamicDll.Bind "DTWAIN_ArrayConvertFloatToFix32" : DTWAIN_ArrayConvertFloatToFix32Delegate)
    let private ArrayCopy = lazy (DynamicDll.Bind "DTWAIN_ArrayCopy" : DTWAIN_ArrayCopyDelegate)
    let private ArrayCreate = lazy (DynamicDll.Bind "DTWAIN_ArrayCreate" : DTWAIN_ArrayCreateDelegate)
    let private ArrayCreateCopy = lazy (DynamicDll.Bind "DTWAIN_ArrayCreateCopy" : DTWAIN_ArrayCreateCopyDelegate)
    let private ArrayCreateFromCap = lazy (DynamicDll.Bind "DTWAIN_ArrayCreateFromCap" : DTWAIN_ArrayCreateFromCapDelegate)
    let private ArrayCreateFromLong64s = lazy (DynamicDll.Bind "DTWAIN_ArrayCreateFromLong64s" : DTWAIN_ArrayCreateFromLong64sDelegate)
    let private ArrayCreateFromLongs = lazy (DynamicDll.Bind "DTWAIN_ArrayCreateFromLongs" : DTWAIN_ArrayCreateFromLongsDelegate)
    let private ArrayCreateFromReals = lazy (DynamicDll.Bind "DTWAIN_ArrayCreateFromReals" : DTWAIN_ArrayCreateFromRealsDelegate)
    let private ArrayDestroy = lazy (DynamicDll.Bind "DTWAIN_ArrayDestroy" : DTWAIN_ArrayDestroyDelegate)
    let private ArrayDestroyFrames = lazy (DynamicDll.Bind "DTWAIN_ArrayDestroyFrames" : DTWAIN_ArrayDestroyFramesDelegate)
    let private ArrayFind = lazy (DynamicDll.Bind "DTWAIN_ArrayFind" : DTWAIN_ArrayFindDelegate)
    let private ArrayFindANSIString = lazy (DynamicDll.Bind "DTWAIN_ArrayFindANSIString" : DTWAIN_ArrayFindANSIStringDelegate)
    let private ArrayFindFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayFindFloat" : DTWAIN_ArrayFindFloatDelegate)
    let private ArrayFindFloatString = lazy (DynamicDll.Bind "DTWAIN_ArrayFindFloatString" : DTWAIN_ArrayFindFloatStringDelegate)
    let private ArrayFindFloatStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayFindFloatStringA" : DTWAIN_ArrayFindFloatStringADelegate)
    let private ArrayFindFloatStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayFindFloatStringW" : DTWAIN_ArrayFindFloatStringWDelegate)
    let private ArrayFindLong = lazy (DynamicDll.Bind "DTWAIN_ArrayFindLong" : DTWAIN_ArrayFindLongDelegate)
    let private ArrayFindLong64 = lazy (DynamicDll.Bind "DTWAIN_ArrayFindLong64" : DTWAIN_ArrayFindLong64Delegate)
    let private ArrayFindString = lazy (DynamicDll.Bind "DTWAIN_ArrayFindString" : DTWAIN_ArrayFindStringDelegate)
    let private ArrayFindStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayFindStringA" : DTWAIN_ArrayFindStringADelegate)
    let private ArrayFindStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayFindStringW" : DTWAIN_ArrayFindStringWDelegate)
    let private ArrayFindWideString = lazy (DynamicDll.Bind "DTWAIN_ArrayFindWideString" : DTWAIN_ArrayFindWideStringDelegate)
    let private ArrayFix32GetAt = lazy (DynamicDll.Bind "DTWAIN_ArrayFix32GetAt" : DTWAIN_ArrayFix32GetAtDelegate)
    let private ArrayFix32SetAt = lazy (DynamicDll.Bind "DTWAIN_ArrayFix32SetAt" : DTWAIN_ArrayFix32SetAtDelegate)
    let private ArrayFloatToANSIString = lazy (DynamicDll.Bind "DTWAIN_ArrayFloatToANSIString" : DTWAIN_ArrayFloatToANSIStringDelegate)
    let private ArrayFloatToString = lazy (DynamicDll.Bind "DTWAIN_ArrayFloatToString" : DTWAIN_ArrayFloatToStringDelegate)
    let private ArrayFloatToWideString = lazy (DynamicDll.Bind "DTWAIN_ArrayFloatToWideString" : DTWAIN_ArrayFloatToWideStringDelegate)
    let private ArrayGetAt = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAt" : DTWAIN_ArrayGetAtDelegate)
    let private ArrayGetAtANSIString = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtANSIString" : DTWAIN_ArrayGetAtANSIStringDelegate)
    let private ArrayGetAtFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFloat" : DTWAIN_ArrayGetAtFloatDelegate)
    let private ArrayGetAtFloatString = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFloatString" : DTWAIN_ArrayGetAtFloatStringDelegate)
    let private ArrayGetAtFloatStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFloatStringA" : DTWAIN_ArrayGetAtFloatStringADelegate)
    let private ArrayGetAtFloatStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFloatStringW" : DTWAIN_ArrayGetAtFloatStringWDelegate)
    let private ArrayGetAtFrame = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFrame" : DTWAIN_ArrayGetAtFrameDelegate)
    let private ArrayGetAtFrameEx = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFrameEx" : DTWAIN_ArrayGetAtFrameExDelegate)
    let private ArrayGetAtFrameString = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFrameString" : DTWAIN_ArrayGetAtFrameStringDelegate)
    let private ArrayGetAtFrameStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFrameStringA" : DTWAIN_ArrayGetAtFrameStringADelegate)
    let private ArrayGetAtFrameStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtFrameStringW" : DTWAIN_ArrayGetAtFrameStringWDelegate)
    let private ArrayGetAtLong = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtLong" : DTWAIN_ArrayGetAtLongDelegate)
    let private ArrayGetAtLong64 = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtLong64" : DTWAIN_ArrayGetAtLong64Delegate)
    let private ArrayGetAtSource = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtSource" : DTWAIN_ArrayGetAtSourceDelegate)
    let private ArrayGetAtString = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtString" : DTWAIN_ArrayGetAtStringDelegate)
    let private ArrayGetAtStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtStringA" : DTWAIN_ArrayGetAtStringADelegate)
    let private ArrayGetAtStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtStringW" : DTWAIN_ArrayGetAtStringWDelegate)
    let private ArrayGetAtWideString = lazy (DynamicDll.Bind "DTWAIN_ArrayGetAtWideString" : DTWAIN_ArrayGetAtWideStringDelegate)
    let private ArrayGetBuffer = lazy (DynamicDll.Bind "DTWAIN_ArrayGetBuffer" : DTWAIN_ArrayGetBufferDelegate)
    let private ArrayGetCapValues = lazy (DynamicDll.Bind "DTWAIN_ArrayGetCapValues" : DTWAIN_ArrayGetCapValuesDelegate)
    let private ArrayGetCapValuesEx = lazy (DynamicDll.Bind "DTWAIN_ArrayGetCapValuesEx" : DTWAIN_ArrayGetCapValuesExDelegate)
    let private ArrayGetCapValuesEx2 = lazy (DynamicDll.Bind "DTWAIN_ArrayGetCapValuesEx2" : DTWAIN_ArrayGetCapValuesEx2Delegate)
    let private ArrayGetCount = lazy (DynamicDll.Bind "DTWAIN_ArrayGetCount" : DTWAIN_ArrayGetCountDelegate)
    let private ArrayGetMaxStringLength = lazy (DynamicDll.Bind "DTWAIN_ArrayGetMaxStringLength" : DTWAIN_ArrayGetMaxStringLengthDelegate)
    let private ArrayGetSourceAt = lazy (DynamicDll.Bind "DTWAIN_ArrayGetSourceAt" : DTWAIN_ArrayGetSourceAtDelegate)
    let private ArrayGetStringLength = lazy (DynamicDll.Bind "DTWAIN_ArrayGetStringLength" : DTWAIN_ArrayGetStringLengthDelegate)
    let private ArrayGetType = lazy (DynamicDll.Bind "DTWAIN_ArrayGetType" : DTWAIN_ArrayGetTypeDelegate)
    let private ArrayInit = lazy (DynamicDll.Bind "DTWAIN_ArrayInit" : DTWAIN_ArrayInitDelegate)
    let private ArrayInsertAt = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAt" : DTWAIN_ArrayInsertAtDelegate)
    let private ArrayInsertAtANSIString = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtANSIString" : DTWAIN_ArrayInsertAtANSIStringDelegate)
    let private ArrayInsertAtANSIStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtANSIStringN" : DTWAIN_ArrayInsertAtANSIStringNDelegate)
    let private ArrayInsertAtFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloat" : DTWAIN_ArrayInsertAtFloatDelegate)
    let private ArrayInsertAtFloatN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloatN" : DTWAIN_ArrayInsertAtFloatNDelegate)
    let private ArrayInsertAtFloatString = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloatString" : DTWAIN_ArrayInsertAtFloatStringDelegate)
    let private ArrayInsertAtFloatStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloatStringA" : DTWAIN_ArrayInsertAtFloatStringADelegate)
    let private ArrayInsertAtFloatStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloatStringN" : DTWAIN_ArrayInsertAtFloatStringNDelegate)
    let private ArrayInsertAtFloatStringNA = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloatStringNA" : DTWAIN_ArrayInsertAtFloatStringNADelegate)
    let private ArrayInsertAtFloatStringNW = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloatStringNW" : DTWAIN_ArrayInsertAtFloatStringNWDelegate)
    let private ArrayInsertAtFloatStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFloatStringW" : DTWAIN_ArrayInsertAtFloatStringWDelegate)
    let private ArrayInsertAtFrame = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFrame" : DTWAIN_ArrayInsertAtFrameDelegate)
    let private ArrayInsertAtFrameN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtFrameN" : DTWAIN_ArrayInsertAtFrameNDelegate)
    let private ArrayInsertAtLong = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtLong" : DTWAIN_ArrayInsertAtLongDelegate)
    let private ArrayInsertAtLong64 = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtLong64" : DTWAIN_ArrayInsertAtLong64Delegate)
    let private ArrayInsertAtLong64N = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtLong64N" : DTWAIN_ArrayInsertAtLong64NDelegate)
    let private ArrayInsertAtLongN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtLongN" : DTWAIN_ArrayInsertAtLongNDelegate)
    let private ArrayInsertAtN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtN" : DTWAIN_ArrayInsertAtNDelegate)
    let private ArrayInsertAtString = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtString" : DTWAIN_ArrayInsertAtStringDelegate)
    let private ArrayInsertAtStringA = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtStringA" : DTWAIN_ArrayInsertAtStringADelegate)
    let private ArrayInsertAtStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtStringN" : DTWAIN_ArrayInsertAtStringNDelegate)
    let private ArrayInsertAtStringNA = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtStringNA" : DTWAIN_ArrayInsertAtStringNADelegate)
    let private ArrayInsertAtStringNW = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtStringNW" : DTWAIN_ArrayInsertAtStringNWDelegate)
    let private ArrayInsertAtStringW = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtStringW" : DTWAIN_ArrayInsertAtStringWDelegate)
    let private ArrayInsertAtWideString = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtWideString" : DTWAIN_ArrayInsertAtWideStringDelegate)
    let private ArrayInsertAtWideStringN = lazy (DynamicDll.Bind "DTWAIN_ArrayInsertAtWideStringN" : DTWAIN_ArrayInsertAtWideStringNDelegate)
    let private ArrayRemoveAll = lazy (DynamicDll.Bind "DTWAIN_ArrayRemoveAll" : DTWAIN_ArrayRemoveAllDelegate)
    let private ArrayRemoveAt = lazy (DynamicDll.Bind "DTWAIN_ArrayRemoveAt" : DTWAIN_ArrayRemoveAtDelegate)
    let private ArrayRemoveAtN = lazy (DynamicDll.Bind "DTWAIN_ArrayRemoveAtN" : DTWAIN_ArrayRemoveAtNDelegate)
    let private ArrayResize = lazy (DynamicDll.Bind "DTWAIN_ArrayResize" : DTWAIN_ArrayResizeDelegate)
    let private ArraySetAt = lazy (DynamicDll.Bind "DTWAIN_ArraySetAt" : DTWAIN_ArraySetAtDelegate)
    let private ArraySetAtANSIString = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtANSIString" : DTWAIN_ArraySetAtANSIStringDelegate)
    let private ArraySetAtFloat = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFloat" : DTWAIN_ArraySetAtFloatDelegate)
    let private ArraySetAtFloatString = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFloatString" : DTWAIN_ArraySetAtFloatStringDelegate)
    let private ArraySetAtFloatStringA = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFloatStringA" : DTWAIN_ArraySetAtFloatStringADelegate)
    let private ArraySetAtFloatStringW = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFloatStringW" : DTWAIN_ArraySetAtFloatStringWDelegate)
    let private ArraySetAtFrame = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFrame" : DTWAIN_ArraySetAtFrameDelegate)
    let private ArraySetAtFrameEx = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFrameEx" : DTWAIN_ArraySetAtFrameExDelegate)
    let private ArraySetAtFrameString = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFrameString" : DTWAIN_ArraySetAtFrameStringDelegate)
    let private ArraySetAtFrameStringA = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFrameStringA" : DTWAIN_ArraySetAtFrameStringADelegate)
    let private ArraySetAtFrameStringW = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtFrameStringW" : DTWAIN_ArraySetAtFrameStringWDelegate)
    let private ArraySetAtLong = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtLong" : DTWAIN_ArraySetAtLongDelegate)
    let private ArraySetAtLong64 = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtLong64" : DTWAIN_ArraySetAtLong64Delegate)
    let private ArraySetAtString = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtString" : DTWAIN_ArraySetAtStringDelegate)
    let private ArraySetAtStringA = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtStringA" : DTWAIN_ArraySetAtStringADelegate)
    let private ArraySetAtStringW = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtStringW" : DTWAIN_ArraySetAtStringWDelegate)
    let private ArraySetAtWideString = lazy (DynamicDll.Bind "DTWAIN_ArraySetAtWideString" : DTWAIN_ArraySetAtWideStringDelegate)
    let private ArrayStringToFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayStringToFloat" : DTWAIN_ArrayStringToFloatDelegate)
    let private ArrayWideStringToFloat = lazy (DynamicDll.Bind "DTWAIN_ArrayWideStringToFloat" : DTWAIN_ArrayWideStringToFloatDelegate)
    let private CallCallback = lazy (DynamicDll.Bind "DTWAIN_CallCallback" : DTWAIN_CallCallbackDelegate)
    let private CallCallback64 = lazy (DynamicDll.Bind "DTWAIN_CallCallback64" : DTWAIN_CallCallback64Delegate)
    let private CallDSMProc = lazy (DynamicDll.Bind "DTWAIN_CallDSMProc" : DTWAIN_CallDSMProcDelegate)
    let private CheckHandles = lazy (DynamicDll.Bind "DTWAIN_CheckHandles" : DTWAIN_CheckHandlesDelegate)
    let private ClearBuffers = lazy (DynamicDll.Bind "DTWAIN_ClearBuffers" : DTWAIN_ClearBuffersDelegate)
    let private ClearErrorBuffer = lazy (DynamicDll.Bind "DTWAIN_ClearErrorBuffer" : DTWAIN_ClearErrorBufferDelegate)
    let private ClearPDFText = lazy (DynamicDll.Bind "DTWAIN_ClearPDFText" : DTWAIN_ClearPDFTextDelegate)
    let private ClearPage = lazy (DynamicDll.Bind "DTWAIN_ClearPage" : DTWAIN_ClearPageDelegate)
    let private CloseSource = lazy (DynamicDll.Bind "DTWAIN_CloseSource" : DTWAIN_CloseSourceDelegate)
    let private CloseSourceUI = lazy (DynamicDll.Bind "DTWAIN_CloseSourceUI" : DTWAIN_CloseSourceUIDelegate)
    let private ConvertDIBToBitmap = lazy (DynamicDll.Bind "DTWAIN_ConvertDIBToBitmap" : DTWAIN_ConvertDIBToBitmapDelegate)
    let private ConvertDIBToFullBitmap = lazy (DynamicDll.Bind "DTWAIN_ConvertDIBToFullBitmap" : DTWAIN_ConvertDIBToFullBitmapDelegate)
    let private ConvertToAPIString = lazy (DynamicDll.Bind "DTWAIN_ConvertToAPIString" : DTWAIN_ConvertToAPIStringDelegate)
    let private ConvertToAPIStringA = lazy (DynamicDll.Bind "DTWAIN_ConvertToAPIStringA" : DTWAIN_ConvertToAPIStringADelegate)
    let private ConvertToAPIStringEx = lazy (DynamicDll.Bind "DTWAIN_ConvertToAPIStringEx" : DTWAIN_ConvertToAPIStringExDelegate)
    let private ConvertToAPIStringExA = lazy (DynamicDll.Bind "DTWAIN_ConvertToAPIStringExA" : DTWAIN_ConvertToAPIStringExADelegate)
    let private ConvertToAPIStringExW = lazy (DynamicDll.Bind "DTWAIN_ConvertToAPIStringExW" : DTWAIN_ConvertToAPIStringExWDelegate)
    let private ConvertToAPIStringW = lazy (DynamicDll.Bind "DTWAIN_ConvertToAPIStringW" : DTWAIN_ConvertToAPIStringWDelegate)
    let private CreateAcquisitionArray = lazy (DynamicDll.Bind "DTWAIN_CreateAcquisitionArray" : DTWAIN_CreateAcquisitionArrayDelegate)
    let private CreatePDFTextElement = lazy (DynamicDll.Bind "DTWAIN_CreatePDFTextElement" : DTWAIN_CreatePDFTextElementDelegate)
    let private DeleteDIB = lazy (DynamicDll.Bind "DTWAIN_DeleteDIB" : DTWAIN_DeleteDIBDelegate)
    let private DestroyAcquisitionArray = lazy (DynamicDll.Bind "DTWAIN_DestroyAcquisitionArray" : DTWAIN_DestroyAcquisitionArrayDelegate)
    let private DestroyPDFTextElement = lazy (DynamicDll.Bind "DTWAIN_DestroyPDFTextElement" : DTWAIN_DestroyPDFTextElementDelegate)
    let private DisableAppWindow = lazy (DynamicDll.Bind "DTWAIN_DisableAppWindow" : DTWAIN_DisableAppWindowDelegate)
    let private EnableAutoBorderDetect = lazy (DynamicDll.Bind "DTWAIN_EnableAutoBorderDetect" : DTWAIN_EnableAutoBorderDetectDelegate)
    let private EnableAutoBright = lazy (DynamicDll.Bind "DTWAIN_EnableAutoBright" : DTWAIN_EnableAutoBrightDelegate)
    let private EnableAutoDeskew = lazy (DynamicDll.Bind "DTWAIN_EnableAutoDeskew" : DTWAIN_EnableAutoDeskewDelegate)
    let private EnableAutoFeed = lazy (DynamicDll.Bind "DTWAIN_EnableAutoFeed" : DTWAIN_EnableAutoFeedDelegate)
    let private EnableAutoRotate = lazy (DynamicDll.Bind "DTWAIN_EnableAutoRotate" : DTWAIN_EnableAutoRotateDelegate)
    let private EnableAutoScan = lazy (DynamicDll.Bind "DTWAIN_EnableAutoScan" : DTWAIN_EnableAutoScanDelegate)
    let private EnableAutomaticSenseMedium = lazy (DynamicDll.Bind "DTWAIN_EnableAutomaticSenseMedium" : DTWAIN_EnableAutomaticSenseMediumDelegate)
    let private EnableDuplex = lazy (DynamicDll.Bind "DTWAIN_EnableDuplex" : DTWAIN_EnableDuplexDelegate)
    let private EnableFeeder = lazy (DynamicDll.Bind "DTWAIN_EnableFeeder" : DTWAIN_EnableFeederDelegate)
    let private EnableIndicator = lazy (DynamicDll.Bind "DTWAIN_EnableIndicator" : DTWAIN_EnableIndicatorDelegate)
    let private EnableJobFileHandling = lazy (DynamicDll.Bind "DTWAIN_EnableJobFileHandling" : DTWAIN_EnableJobFileHandlingDelegate)
    let private EnableLamp = lazy (DynamicDll.Bind "DTWAIN_EnableLamp" : DTWAIN_EnableLampDelegate)
    let private EnableMsgNotify = lazy (DynamicDll.Bind "DTWAIN_EnableMsgNotify" : DTWAIN_EnableMsgNotifyDelegate)
    let private EnablePatchDetect = lazy (DynamicDll.Bind "DTWAIN_EnablePatchDetect" : DTWAIN_EnablePatchDetectDelegate)
    let private EnablePeekMessageLoop = lazy (DynamicDll.Bind "DTWAIN_EnablePeekMessageLoop" : DTWAIN_EnablePeekMessageLoopDelegate)
    let private EnablePrinter = lazy (DynamicDll.Bind "DTWAIN_EnablePrinter" : DTWAIN_EnablePrinterDelegate)
    let private EnableThumbnail = lazy (DynamicDll.Bind "DTWAIN_EnableThumbnail" : DTWAIN_EnableThumbnailDelegate)
    let private EnableTripletsNotify = lazy (DynamicDll.Bind "DTWAIN_EnableTripletsNotify" : DTWAIN_EnableTripletsNotifyDelegate)
    let private EndThread = lazy (DynamicDll.Bind "DTWAIN_EndThread" : DTWAIN_EndThreadDelegate)
    let private EndTwainSession = lazy (DynamicDll.Bind "DTWAIN_EndTwainSession" : DTWAIN_EndTwainSessionDelegate)
    let private EnumAlarmVolumes = lazy (DynamicDll.Bind "DTWAIN_EnumAlarmVolumes" : DTWAIN_EnumAlarmVolumesDelegate)
    let private EnumAlarmVolumesEx = lazy (DynamicDll.Bind "DTWAIN_EnumAlarmVolumesEx" : DTWAIN_EnumAlarmVolumesExDelegate)
    let private EnumAlarms = lazy (DynamicDll.Bind "DTWAIN_EnumAlarms" : DTWAIN_EnumAlarmsDelegate)
    let private EnumAlarmsEx = lazy (DynamicDll.Bind "DTWAIN_EnumAlarmsEx" : DTWAIN_EnumAlarmsExDelegate)
    let private EnumAudioXferMechs = lazy (DynamicDll.Bind "DTWAIN_EnumAudioXferMechs" : DTWAIN_EnumAudioXferMechsDelegate)
    let private EnumAudioXferMechsEx = lazy (DynamicDll.Bind "DTWAIN_EnumAudioXferMechsEx" : DTWAIN_EnumAudioXferMechsExDelegate)
    let private EnumAutoFeedValues = lazy (DynamicDll.Bind "DTWAIN_EnumAutoFeedValues" : DTWAIN_EnumAutoFeedValuesDelegate)
    let private EnumAutoFeedValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumAutoFeedValuesEx" : DTWAIN_EnumAutoFeedValuesExDelegate)
    let private EnumAutomaticCaptures = lazy (DynamicDll.Bind "DTWAIN_EnumAutomaticCaptures" : DTWAIN_EnumAutomaticCapturesDelegate)
    let private EnumAutomaticCapturesEx = lazy (DynamicDll.Bind "DTWAIN_EnumAutomaticCapturesEx" : DTWAIN_EnumAutomaticCapturesExDelegate)
    let private EnumAutomaticSenseMedium = lazy (DynamicDll.Bind "DTWAIN_EnumAutomaticSenseMedium" : DTWAIN_EnumAutomaticSenseMediumDelegate)
    let private EnumAutomaticSenseMediumEx = lazy (DynamicDll.Bind "DTWAIN_EnumAutomaticSenseMediumEx" : DTWAIN_EnumAutomaticSenseMediumExDelegate)
    let private EnumBitDepths = lazy (DynamicDll.Bind "DTWAIN_EnumBitDepths" : DTWAIN_EnumBitDepthsDelegate)
    let private EnumBitDepthsEx = lazy (DynamicDll.Bind "DTWAIN_EnumBitDepthsEx" : DTWAIN_EnumBitDepthsExDelegate)
    let private EnumBitDepthsEx2 = lazy (DynamicDll.Bind "DTWAIN_EnumBitDepthsEx2" : DTWAIN_EnumBitDepthsEx2Delegate)
    let private EnumBottomCameras = lazy (DynamicDll.Bind "DTWAIN_EnumBottomCameras" : DTWAIN_EnumBottomCamerasDelegate)
    let private EnumBottomCamerasEx = lazy (DynamicDll.Bind "DTWAIN_EnumBottomCamerasEx" : DTWAIN_EnumBottomCamerasExDelegate)
    let private EnumBrightnessValues = lazy (DynamicDll.Bind "DTWAIN_EnumBrightnessValues" : DTWAIN_EnumBrightnessValuesDelegate)
    let private EnumBrightnessValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumBrightnessValuesEx" : DTWAIN_EnumBrightnessValuesExDelegate)
    let private EnumCameras = lazy (DynamicDll.Bind "DTWAIN_EnumCameras" : DTWAIN_EnumCamerasDelegate)
    let private EnumCamerasEx = lazy (DynamicDll.Bind "DTWAIN_EnumCamerasEx" : DTWAIN_EnumCamerasExDelegate)
    let private EnumCamerasEx2 = lazy (DynamicDll.Bind "DTWAIN_EnumCamerasEx2" : DTWAIN_EnumCamerasEx2Delegate)
    let private EnumCamerasEx3 = lazy (DynamicDll.Bind "DTWAIN_EnumCamerasEx3" : DTWAIN_EnumCamerasEx3Delegate)
    let private EnumCompressionTypes = lazy (DynamicDll.Bind "DTWAIN_EnumCompressionTypes" : DTWAIN_EnumCompressionTypesDelegate)
    let private EnumCompressionTypesEx = lazy (DynamicDll.Bind "DTWAIN_EnumCompressionTypesEx" : DTWAIN_EnumCompressionTypesExDelegate)
    let private EnumCompressionTypesEx2 = lazy (DynamicDll.Bind "DTWAIN_EnumCompressionTypesEx2" : DTWAIN_EnumCompressionTypesEx2Delegate)
    let private EnumContrastValues = lazy (DynamicDll.Bind "DTWAIN_EnumContrastValues" : DTWAIN_EnumContrastValuesDelegate)
    let private EnumContrastValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumContrastValuesEx" : DTWAIN_EnumContrastValuesExDelegate)
    let private EnumCustomCaps = lazy (DynamicDll.Bind "DTWAIN_EnumCustomCaps" : DTWAIN_EnumCustomCapsDelegate)
    let private EnumCustomCapsEx2 = lazy (DynamicDll.Bind "DTWAIN_EnumCustomCapsEx2" : DTWAIN_EnumCustomCapsEx2Delegate)
    let private EnumDoubleFeedDetectLengths = lazy (DynamicDll.Bind "DTWAIN_EnumDoubleFeedDetectLengths" : DTWAIN_EnumDoubleFeedDetectLengthsDelegate)
    let private EnumDoubleFeedDetectLengthsEx = lazy (DynamicDll.Bind "DTWAIN_EnumDoubleFeedDetectLengthsEx" : DTWAIN_EnumDoubleFeedDetectLengthsExDelegate)
    let private EnumDoubleFeedDetectValues = lazy (DynamicDll.Bind "DTWAIN_EnumDoubleFeedDetectValues" : DTWAIN_EnumDoubleFeedDetectValuesDelegate)
    let private EnumDoubleFeedDetectValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumDoubleFeedDetectValuesEx" : DTWAIN_EnumDoubleFeedDetectValuesExDelegate)
    let private EnumExtImageInfoTypes = lazy (DynamicDll.Bind "DTWAIN_EnumExtImageInfoTypes" : DTWAIN_EnumExtImageInfoTypesDelegate)
    let private EnumExtImageInfoTypesEx = lazy (DynamicDll.Bind "DTWAIN_EnumExtImageInfoTypesEx" : DTWAIN_EnumExtImageInfoTypesExDelegate)
    let private EnumExtendedCaps = lazy (DynamicDll.Bind "DTWAIN_EnumExtendedCaps" : DTWAIN_EnumExtendedCapsDelegate)
    let private EnumExtendedCapsEx = lazy (DynamicDll.Bind "DTWAIN_EnumExtendedCapsEx" : DTWAIN_EnumExtendedCapsExDelegate)
    let private EnumExtendedCapsEx2 = lazy (DynamicDll.Bind "DTWAIN_EnumExtendedCapsEx2" : DTWAIN_EnumExtendedCapsEx2Delegate)
    let private EnumFileTypeBitsPerPixel = lazy (DynamicDll.Bind "DTWAIN_EnumFileTypeBitsPerPixel" : DTWAIN_EnumFileTypeBitsPerPixelDelegate)
    let private EnumFileXferFormats = lazy (DynamicDll.Bind "DTWAIN_EnumFileXferFormats" : DTWAIN_EnumFileXferFormatsDelegate)
    let private EnumFileXferFormatsEx = lazy (DynamicDll.Bind "DTWAIN_EnumFileXferFormatsEx" : DTWAIN_EnumFileXferFormatsExDelegate)
    let private EnumHalftones = lazy (DynamicDll.Bind "DTWAIN_EnumHalftones" : DTWAIN_EnumHalftonesDelegate)
    let private EnumHalftonesEx = lazy (DynamicDll.Bind "DTWAIN_EnumHalftonesEx" : DTWAIN_EnumHalftonesExDelegate)
    let private EnumHighlightValues = lazy (DynamicDll.Bind "DTWAIN_EnumHighlightValues" : DTWAIN_EnumHighlightValuesDelegate)
    let private EnumHighlightValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumHighlightValuesEx" : DTWAIN_EnumHighlightValuesExDelegate)
    let private EnumJobControls = lazy (DynamicDll.Bind "DTWAIN_EnumJobControls" : DTWAIN_EnumJobControlsDelegate)
    let private EnumJobControlsEx = lazy (DynamicDll.Bind "DTWAIN_EnumJobControlsEx" : DTWAIN_EnumJobControlsExDelegate)
    let private EnumLightPaths = lazy (DynamicDll.Bind "DTWAIN_EnumLightPaths" : DTWAIN_EnumLightPathsDelegate)
    let private EnumLightPathsEx = lazy (DynamicDll.Bind "DTWAIN_EnumLightPathsEx" : DTWAIN_EnumLightPathsExDelegate)
    let private EnumLightSources = lazy (DynamicDll.Bind "DTWAIN_EnumLightSources" : DTWAIN_EnumLightSourcesDelegate)
    let private EnumLightSourcesEx = lazy (DynamicDll.Bind "DTWAIN_EnumLightSourcesEx" : DTWAIN_EnumLightSourcesExDelegate)
    let private EnumMaxBuffers = lazy (DynamicDll.Bind "DTWAIN_EnumMaxBuffers" : DTWAIN_EnumMaxBuffersDelegate)
    let private EnumMaxBuffersEx = lazy (DynamicDll.Bind "DTWAIN_EnumMaxBuffersEx" : DTWAIN_EnumMaxBuffersExDelegate)
    let private EnumNoiseFilters = lazy (DynamicDll.Bind "DTWAIN_EnumNoiseFilters" : DTWAIN_EnumNoiseFiltersDelegate)
    let private EnumNoiseFiltersEx = lazy (DynamicDll.Bind "DTWAIN_EnumNoiseFiltersEx" : DTWAIN_EnumNoiseFiltersExDelegate)
    let private EnumOCRInterfaces = lazy (DynamicDll.Bind "DTWAIN_EnumOCRInterfaces" : DTWAIN_EnumOCRInterfacesDelegate)
    let private EnumOCRSupportedCaps = lazy (DynamicDll.Bind "DTWAIN_EnumOCRSupportedCaps" : DTWAIN_EnumOCRSupportedCapsDelegate)
    let private EnumOrientations = lazy (DynamicDll.Bind "DTWAIN_EnumOrientations" : DTWAIN_EnumOrientationsDelegate)
    let private EnumOrientationsEx = lazy (DynamicDll.Bind "DTWAIN_EnumOrientationsEx" : DTWAIN_EnumOrientationsExDelegate)
    let private EnumOverscanValues = lazy (DynamicDll.Bind "DTWAIN_EnumOverscanValues" : DTWAIN_EnumOverscanValuesDelegate)
    let private EnumOverscanValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumOverscanValuesEx" : DTWAIN_EnumOverscanValuesExDelegate)
    let private EnumPaperSizes = lazy (DynamicDll.Bind "DTWAIN_EnumPaperSizes" : DTWAIN_EnumPaperSizesDelegate)
    let private EnumPaperSizesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPaperSizesEx" : DTWAIN_EnumPaperSizesExDelegate)
    let private EnumPatchCodes = lazy (DynamicDll.Bind "DTWAIN_EnumPatchCodes" : DTWAIN_EnumPatchCodesDelegate)
    let private EnumPatchCodesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPatchCodesEx" : DTWAIN_EnumPatchCodesExDelegate)
    let private EnumPatchMaxPriorities = lazy (DynamicDll.Bind "DTWAIN_EnumPatchMaxPriorities" : DTWAIN_EnumPatchMaxPrioritiesDelegate)
    let private EnumPatchMaxPrioritiesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPatchMaxPrioritiesEx" : DTWAIN_EnumPatchMaxPrioritiesExDelegate)
    let private EnumPatchMaxRetries = lazy (DynamicDll.Bind "DTWAIN_EnumPatchMaxRetries" : DTWAIN_EnumPatchMaxRetriesDelegate)
    let private EnumPatchMaxRetriesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPatchMaxRetriesEx" : DTWAIN_EnumPatchMaxRetriesExDelegate)
    let private EnumPatchPriorities = lazy (DynamicDll.Bind "DTWAIN_EnumPatchPriorities" : DTWAIN_EnumPatchPrioritiesDelegate)
    let private EnumPatchPrioritiesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPatchPrioritiesEx" : DTWAIN_EnumPatchPrioritiesExDelegate)
    let private EnumPatchSearchModes = lazy (DynamicDll.Bind "DTWAIN_EnumPatchSearchModes" : DTWAIN_EnumPatchSearchModesDelegate)
    let private EnumPatchSearchModesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPatchSearchModesEx" : DTWAIN_EnumPatchSearchModesExDelegate)
    let private EnumPatchTimeOutValues = lazy (DynamicDll.Bind "DTWAIN_EnumPatchTimeOutValues" : DTWAIN_EnumPatchTimeOutValuesDelegate)
    let private EnumPatchTimeOutValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPatchTimeOutValuesEx" : DTWAIN_EnumPatchTimeOutValuesExDelegate)
    let private EnumPixelTypes = lazy (DynamicDll.Bind "DTWAIN_EnumPixelTypes" : DTWAIN_EnumPixelTypesDelegate)
    let private EnumPixelTypesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPixelTypesEx" : DTWAIN_EnumPixelTypesExDelegate)
    let private EnumPrinterStringModes = lazy (DynamicDll.Bind "DTWAIN_EnumPrinterStringModes" : DTWAIN_EnumPrinterStringModesDelegate)
    let private EnumPrinterStringModesEx = lazy (DynamicDll.Bind "DTWAIN_EnumPrinterStringModesEx" : DTWAIN_EnumPrinterStringModesExDelegate)
    let private EnumResolutionValues = lazy (DynamicDll.Bind "DTWAIN_EnumResolutionValues" : DTWAIN_EnumResolutionValuesDelegate)
    let private EnumResolutionValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumResolutionValuesEx" : DTWAIN_EnumResolutionValuesExDelegate)
    let private EnumShadowValues = lazy (DynamicDll.Bind "DTWAIN_EnumShadowValues" : DTWAIN_EnumShadowValuesDelegate)
    let private EnumShadowValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumShadowValuesEx" : DTWAIN_EnumShadowValuesExDelegate)
    let private EnumSourceUnits = lazy (DynamicDll.Bind "DTWAIN_EnumSourceUnits" : DTWAIN_EnumSourceUnitsDelegate)
    let private EnumSourceUnitsEx = lazy (DynamicDll.Bind "DTWAIN_EnumSourceUnitsEx" : DTWAIN_EnumSourceUnitsExDelegate)
    let private EnumSourceValues = lazy (DynamicDll.Bind "DTWAIN_EnumSourceValues" : DTWAIN_EnumSourceValuesDelegate)
    let private EnumSourceValuesA = lazy (DynamicDll.Bind "DTWAIN_EnumSourceValuesA" : DTWAIN_EnumSourceValuesADelegate)
    let private EnumSourceValuesW = lazy (DynamicDll.Bind "DTWAIN_EnumSourceValuesW" : DTWAIN_EnumSourceValuesWDelegate)
    let private EnumSources = lazy (DynamicDll.Bind "DTWAIN_EnumSources" : DTWAIN_EnumSourcesDelegate)
    let private EnumSourcesEx = lazy (DynamicDll.Bind "DTWAIN_EnumSourcesEx" : DTWAIN_EnumSourcesExDelegate)
    let private EnumSupportedCaps = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedCaps" : DTWAIN_EnumSupportedCapsDelegate)
    let private EnumSupportedCapsEx = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedCapsEx" : DTWAIN_EnumSupportedCapsExDelegate)
    let private EnumSupportedCapsEx2 = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedCapsEx2" : DTWAIN_EnumSupportedCapsEx2Delegate)
    let private EnumSupportedExtImageInfo = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedExtImageInfo" : DTWAIN_EnumSupportedExtImageInfoDelegate)
    let private EnumSupportedExtImageInfoEx = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedExtImageInfoEx" : DTWAIN_EnumSupportedExtImageInfoExDelegate)
    let private EnumSupportedFileTypes = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedFileTypes" : DTWAIN_EnumSupportedFileTypesDelegate)
    let private EnumSupportedMultiPageFileTypes = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedMultiPageFileTypes" : DTWAIN_EnumSupportedMultiPageFileTypesDelegate)
    let private EnumSupportedSinglePageFileTypes = lazy (DynamicDll.Bind "DTWAIN_EnumSupportedSinglePageFileTypes" : DTWAIN_EnumSupportedSinglePageFileTypesDelegate)
    let private EnumThresholdValues = lazy (DynamicDll.Bind "DTWAIN_EnumThresholdValues" : DTWAIN_EnumThresholdValuesDelegate)
    let private EnumThresholdValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumThresholdValuesEx" : DTWAIN_EnumThresholdValuesExDelegate)
    let private EnumTopCameras = lazy (DynamicDll.Bind "DTWAIN_EnumTopCameras" : DTWAIN_EnumTopCamerasDelegate)
    let private EnumTopCamerasEx = lazy (DynamicDll.Bind "DTWAIN_EnumTopCamerasEx" : DTWAIN_EnumTopCamerasExDelegate)
    let private EnumTwainPrinters = lazy (DynamicDll.Bind "DTWAIN_EnumTwainPrinters" : DTWAIN_EnumTwainPrintersDelegate)
    let private EnumTwainPrintersArray = lazy (DynamicDll.Bind "DTWAIN_EnumTwainPrintersArray" : DTWAIN_EnumTwainPrintersArrayDelegate)
    let private EnumTwainPrintersArrayEx = lazy (DynamicDll.Bind "DTWAIN_EnumTwainPrintersArrayEx" : DTWAIN_EnumTwainPrintersArrayExDelegate)
    let private EnumTwainPrintersEx = lazy (DynamicDll.Bind "DTWAIN_EnumTwainPrintersEx" : DTWAIN_EnumTwainPrintersExDelegate)
    let private EnumXResolutionValues = lazy (DynamicDll.Bind "DTWAIN_EnumXResolutionValues" : DTWAIN_EnumXResolutionValuesDelegate)
    let private EnumXResolutionValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumXResolutionValuesEx" : DTWAIN_EnumXResolutionValuesExDelegate)
    let private EnumYResolutionValues = lazy (DynamicDll.Bind "DTWAIN_EnumYResolutionValues" : DTWAIN_EnumYResolutionValuesDelegate)
    let private EnumYResolutionValuesEx = lazy (DynamicDll.Bind "DTWAIN_EnumYResolutionValuesEx" : DTWAIN_EnumYResolutionValuesExDelegate)
    let private ExecuteOCR = lazy (DynamicDll.Bind "DTWAIN_ExecuteOCR" : DTWAIN_ExecuteOCRDelegate)
    let private ExecuteOCRA = lazy (DynamicDll.Bind "DTWAIN_ExecuteOCRA" : DTWAIN_ExecuteOCRADelegate)
    let private ExecuteOCRW = lazy (DynamicDll.Bind "DTWAIN_ExecuteOCRW" : DTWAIN_ExecuteOCRWDelegate)
    let private FeedPage = lazy (DynamicDll.Bind "DTWAIN_FeedPage" : DTWAIN_FeedPageDelegate)
    let private FlipBitmap = lazy (DynamicDll.Bind "DTWAIN_FlipBitmap" : DTWAIN_FlipBitmapDelegate)
    let private FlushAcquiredPages = lazy (DynamicDll.Bind "DTWAIN_FlushAcquiredPages" : DTWAIN_FlushAcquiredPagesDelegate)
    let private ForceAcquireBitDepth = lazy (DynamicDll.Bind "DTWAIN_ForceAcquireBitDepth" : DTWAIN_ForceAcquireBitDepthDelegate)
    let private ForceScanOnNoUI = lazy (DynamicDll.Bind "DTWAIN_ForceScanOnNoUI" : DTWAIN_ForceScanOnNoUIDelegate)
    let private FrameCreate = lazy (DynamicDll.Bind "DTWAIN_FrameCreate" : DTWAIN_FrameCreateDelegate)
    let private FrameCreateString = lazy (DynamicDll.Bind "DTWAIN_FrameCreateString" : DTWAIN_FrameCreateStringDelegate)
    let private FrameCreateStringA = lazy (DynamicDll.Bind "DTWAIN_FrameCreateStringA" : DTWAIN_FrameCreateStringADelegate)
    let private FrameCreateStringW = lazy (DynamicDll.Bind "DTWAIN_FrameCreateStringW" : DTWAIN_FrameCreateStringWDelegate)
    let private FrameDestroy = lazy (DynamicDll.Bind "DTWAIN_FrameDestroy" : DTWAIN_FrameDestroyDelegate)
    let private FrameGetAll = lazy (DynamicDll.Bind "DTWAIN_FrameGetAll" : DTWAIN_FrameGetAllDelegate)
    let private FrameGetAllString = lazy (DynamicDll.Bind "DTWAIN_FrameGetAllString" : DTWAIN_FrameGetAllStringDelegate)
    let private FrameGetAllStringA = lazy (DynamicDll.Bind "DTWAIN_FrameGetAllStringA" : DTWAIN_FrameGetAllStringADelegate)
    let private FrameGetAllStringW = lazy (DynamicDll.Bind "DTWAIN_FrameGetAllStringW" : DTWAIN_FrameGetAllStringWDelegate)
    let private FrameGetValue = lazy (DynamicDll.Bind "DTWAIN_FrameGetValue" : DTWAIN_FrameGetValueDelegate)
    let private FrameGetValueString = lazy (DynamicDll.Bind "DTWAIN_FrameGetValueString" : DTWAIN_FrameGetValueStringDelegate)
    let private FrameGetValueStringA = lazy (DynamicDll.Bind "DTWAIN_FrameGetValueStringA" : DTWAIN_FrameGetValueStringADelegate)
    let private FrameGetValueStringW = lazy (DynamicDll.Bind "DTWAIN_FrameGetValueStringW" : DTWAIN_FrameGetValueStringWDelegate)
    let private FrameIsValid = lazy (DynamicDll.Bind "DTWAIN_FrameIsValid" : DTWAIN_FrameIsValidDelegate)
    let private FrameSetAll = lazy (DynamicDll.Bind "DTWAIN_FrameSetAll" : DTWAIN_FrameSetAllDelegate)
    let private FrameSetAllString = lazy (DynamicDll.Bind "DTWAIN_FrameSetAllString" : DTWAIN_FrameSetAllStringDelegate)
    let private FrameSetAllStringA = lazy (DynamicDll.Bind "DTWAIN_FrameSetAllStringA" : DTWAIN_FrameSetAllStringADelegate)
    let private FrameSetAllStringW = lazy (DynamicDll.Bind "DTWAIN_FrameSetAllStringW" : DTWAIN_FrameSetAllStringWDelegate)
    let private FrameSetValue = lazy (DynamicDll.Bind "DTWAIN_FrameSetValue" : DTWAIN_FrameSetValueDelegate)
    let private FrameSetValueString = lazy (DynamicDll.Bind "DTWAIN_FrameSetValueString" : DTWAIN_FrameSetValueStringDelegate)
    let private FrameSetValueStringA = lazy (DynamicDll.Bind "DTWAIN_FrameSetValueStringA" : DTWAIN_FrameSetValueStringADelegate)
    let private FrameSetValueStringW = lazy (DynamicDll.Bind "DTWAIN_FrameSetValueStringW" : DTWAIN_FrameSetValueStringWDelegate)
    let private FreeExtImageInfo = lazy (DynamicDll.Bind "DTWAIN_FreeExtImageInfo" : DTWAIN_FreeExtImageInfoDelegate)
    let private FreeMemory = lazy (DynamicDll.Bind "DTWAIN_FreeMemory" : DTWAIN_FreeMemoryDelegate)
    let private FreeMemoryEx = lazy (DynamicDll.Bind "DTWAIN_FreeMemoryEx" : DTWAIN_FreeMemoryExDelegate)
    let private GetAPIHandleStatus = lazy (DynamicDll.Bind "DTWAIN_GetAPIHandleStatus" : DTWAIN_GetAPIHandleStatusDelegate)
    let private GetAcquireArea = lazy (DynamicDll.Bind "DTWAIN_GetAcquireArea" : DTWAIN_GetAcquireAreaDelegate)
    let private GetAcquireArea2 = lazy (DynamicDll.Bind "DTWAIN_GetAcquireArea2" : DTWAIN_GetAcquireArea2Delegate)
    let private GetAcquireArea2String = lazy (DynamicDll.Bind "DTWAIN_GetAcquireArea2String" : DTWAIN_GetAcquireArea2StringDelegate)
    let private GetAcquireArea2StringA = lazy (DynamicDll.Bind "DTWAIN_GetAcquireArea2StringA" : DTWAIN_GetAcquireArea2StringADelegate)
    let private GetAcquireArea2StringW = lazy (DynamicDll.Bind "DTWAIN_GetAcquireArea2StringW" : DTWAIN_GetAcquireArea2StringWDelegate)
    let private GetAcquireAreaEx = lazy (DynamicDll.Bind "DTWAIN_GetAcquireAreaEx" : DTWAIN_GetAcquireAreaExDelegate)
    let private GetAcquireMetrics = lazy (DynamicDll.Bind "DTWAIN_GetAcquireMetrics" : DTWAIN_GetAcquireMetricsDelegate)
    let private GetAcquireStripBuffer = lazy (DynamicDll.Bind "DTWAIN_GetAcquireStripBuffer" : DTWAIN_GetAcquireStripBufferDelegate)
    let private GetAcquireStripData = lazy (DynamicDll.Bind "DTWAIN_GetAcquireStripData" : DTWAIN_GetAcquireStripDataDelegate)
    let private GetAcquireStripSizes = lazy (DynamicDll.Bind "DTWAIN_GetAcquireStripSizes" : DTWAIN_GetAcquireStripSizesDelegate)
    let private GetAcquiredImage = lazy (DynamicDll.Bind "DTWAIN_GetAcquiredImage" : DTWAIN_GetAcquiredImageDelegate)
    let private GetAcquiredImageArray = lazy (DynamicDll.Bind "DTWAIN_GetAcquiredImageArray" : DTWAIN_GetAcquiredImageArrayDelegate)
    let private GetActiveDSMPath = lazy (DynamicDll.Bind "DTWAIN_GetActiveDSMPath" : DTWAIN_GetActiveDSMPathDelegate)
    let private GetActiveDSMPathA = lazy (DynamicDll.Bind "DTWAIN_GetActiveDSMPathA" : DTWAIN_GetActiveDSMPathADelegate)
    let private GetActiveDSMPathW = lazy (DynamicDll.Bind "DTWAIN_GetActiveDSMPathW" : DTWAIN_GetActiveDSMPathWDelegate)
    let private GetActiveDSMVersionInfo = lazy (DynamicDll.Bind "DTWAIN_GetActiveDSMVersionInfo" : DTWAIN_GetActiveDSMVersionInfoDelegate)
    let private GetActiveDSMVersionInfoA = lazy (DynamicDll.Bind "DTWAIN_GetActiveDSMVersionInfoA" : DTWAIN_GetActiveDSMVersionInfoADelegate)
    let private GetActiveDSMVersionInfoW = lazy (DynamicDll.Bind "DTWAIN_GetActiveDSMVersionInfoW" : DTWAIN_GetActiveDSMVersionInfoWDelegate)
    let private GetAlarmVolume = lazy (DynamicDll.Bind "DTWAIN_GetAlarmVolume" : DTWAIN_GetAlarmVolumeDelegate)
    let private GetAllSourceDibs = lazy (DynamicDll.Bind "DTWAIN_GetAllSourceDibs" : DTWAIN_GetAllSourceDibsDelegate)
    let private GetAppInfo = lazy (DynamicDll.Bind "DTWAIN_GetAppInfo" : DTWAIN_GetAppInfoDelegate)
    let private GetAppInfoA = lazy (DynamicDll.Bind "DTWAIN_GetAppInfoA" : DTWAIN_GetAppInfoADelegate)
    let private GetAppInfoW = lazy (DynamicDll.Bind "DTWAIN_GetAppInfoW" : DTWAIN_GetAppInfoWDelegate)
    let private GetAuthor = lazy (DynamicDll.Bind "DTWAIN_GetAuthor" : DTWAIN_GetAuthorDelegate)
    let private GetAuthorA = lazy (DynamicDll.Bind "DTWAIN_GetAuthorA" : DTWAIN_GetAuthorADelegate)
    let private GetAuthorW = lazy (DynamicDll.Bind "DTWAIN_GetAuthorW" : DTWAIN_GetAuthorWDelegate)
    let private GetBatteryMinutes = lazy (DynamicDll.Bind "DTWAIN_GetBatteryMinutes" : DTWAIN_GetBatteryMinutesDelegate)
    let private GetBatteryPercent = lazy (DynamicDll.Bind "DTWAIN_GetBatteryPercent" : DTWAIN_GetBatteryPercentDelegate)
    let private GetBitDepth = lazy (DynamicDll.Bind "DTWAIN_GetBitDepth" : DTWAIN_GetBitDepthDelegate)
    let private GetBlankPageAutoDetection = lazy (DynamicDll.Bind "DTWAIN_GetBlankPageAutoDetection" : DTWAIN_GetBlankPageAutoDetectionDelegate)
    let private GetBrightness = lazy (DynamicDll.Bind "DTWAIN_GetBrightness" : DTWAIN_GetBrightnessDelegate)
    let private GetBrightnessString = lazy (DynamicDll.Bind "DTWAIN_GetBrightnessString" : DTWAIN_GetBrightnessStringDelegate)
    let private GetBrightnessStringA = lazy (DynamicDll.Bind "DTWAIN_GetBrightnessStringA" : DTWAIN_GetBrightnessStringADelegate)
    let private GetBrightnessStringW = lazy (DynamicDll.Bind "DTWAIN_GetBrightnessStringW" : DTWAIN_GetBrightnessStringWDelegate)
    let private GetBufferedTransferInfo = lazy (DynamicDll.Bind "DTWAIN_GetBufferedTransferInfo" : DTWAIN_GetBufferedTransferInfoDelegate)
    let private GetCallback = lazy (DynamicDll.Bind "DTWAIN_GetCallback" : DTWAIN_GetCallbackDelegate)
    let private GetCallback64 = lazy (DynamicDll.Bind "DTWAIN_GetCallback64" : DTWAIN_GetCallback64Delegate)
    let private GetCapArrayType = lazy (DynamicDll.Bind "DTWAIN_GetCapArrayType" : DTWAIN_GetCapArrayTypeDelegate)
    let private GetCapContainer = lazy (DynamicDll.Bind "DTWAIN_GetCapContainer" : DTWAIN_GetCapContainerDelegate)
    let private GetCapContainerEx = lazy (DynamicDll.Bind "DTWAIN_GetCapContainerEx" : DTWAIN_GetCapContainerExDelegate)
    let private GetCapContainerEx2 = lazy (DynamicDll.Bind "DTWAIN_GetCapContainerEx2" : DTWAIN_GetCapContainerEx2Delegate)
    let private GetCapDataType = lazy (DynamicDll.Bind "DTWAIN_GetCapDataType" : DTWAIN_GetCapDataTypeDelegate)
    let private GetCapFromName = lazy (DynamicDll.Bind "DTWAIN_GetCapFromName" : DTWAIN_GetCapFromNameDelegate)
    let private GetCapFromNameA = lazy (DynamicDll.Bind "DTWAIN_GetCapFromNameA" : DTWAIN_GetCapFromNameADelegate)
    let private GetCapFromNameW = lazy (DynamicDll.Bind "DTWAIN_GetCapFromNameW" : DTWAIN_GetCapFromNameWDelegate)
    let private GetCapOperations = lazy (DynamicDll.Bind "DTWAIN_GetCapOperations" : DTWAIN_GetCapOperationsDelegate)
    let private GetCapValues = lazy (DynamicDll.Bind "DTWAIN_GetCapValues" : DTWAIN_GetCapValuesDelegate)
    let private GetCapValuesEx = lazy (DynamicDll.Bind "DTWAIN_GetCapValuesEx" : DTWAIN_GetCapValuesExDelegate)
    let private GetCapValuesEx2 = lazy (DynamicDll.Bind "DTWAIN_GetCapValuesEx2" : DTWAIN_GetCapValuesEx2Delegate)
    let private GetCaption = lazy (DynamicDll.Bind "DTWAIN_GetCaption" : DTWAIN_GetCaptionDelegate)
    let private GetCaptionA = lazy (DynamicDll.Bind "DTWAIN_GetCaptionA" : DTWAIN_GetCaptionADelegate)
    let private GetCaptionW = lazy (DynamicDll.Bind "DTWAIN_GetCaptionW" : DTWAIN_GetCaptionWDelegate)
    let private GetCompressionSize = lazy (DynamicDll.Bind "DTWAIN_GetCompressionSize" : DTWAIN_GetCompressionSizeDelegate)
    let private GetCompressionType = lazy (DynamicDll.Bind "DTWAIN_GetCompressionType" : DTWAIN_GetCompressionTypeDelegate)
    let private GetConditionCodeString = lazy (DynamicDll.Bind "DTWAIN_GetConditionCodeString" : DTWAIN_GetConditionCodeStringDelegate)
    let private GetConditionCodeStringA = lazy (DynamicDll.Bind "DTWAIN_GetConditionCodeStringA" : DTWAIN_GetConditionCodeStringADelegate)
    let private GetConditionCodeStringW = lazy (DynamicDll.Bind "DTWAIN_GetConditionCodeStringW" : DTWAIN_GetConditionCodeStringWDelegate)
    let private GetContrast = lazy (DynamicDll.Bind "DTWAIN_GetContrast" : DTWAIN_GetContrastDelegate)
    let private GetContrastString = lazy (DynamicDll.Bind "DTWAIN_GetContrastString" : DTWAIN_GetContrastStringDelegate)
    let private GetContrastStringA = lazy (DynamicDll.Bind "DTWAIN_GetContrastStringA" : DTWAIN_GetContrastStringADelegate)
    let private GetContrastStringW = lazy (DynamicDll.Bind "DTWAIN_GetContrastStringW" : DTWAIN_GetContrastStringWDelegate)
    let private GetCountry = lazy (DynamicDll.Bind "DTWAIN_GetCountry" : DTWAIN_GetCountryDelegate)
    let private GetCurrentAcquiredImage = lazy (DynamicDll.Bind "DTWAIN_GetCurrentAcquiredImage" : DTWAIN_GetCurrentAcquiredImageDelegate)
    let private GetCurrentFileName = lazy (DynamicDll.Bind "DTWAIN_GetCurrentFileName" : DTWAIN_GetCurrentFileNameDelegate)
    let private GetCurrentFileNameA = lazy (DynamicDll.Bind "DTWAIN_GetCurrentFileNameA" : DTWAIN_GetCurrentFileNameADelegate)
    let private GetCurrentFileNameW = lazy (DynamicDll.Bind "DTWAIN_GetCurrentFileNameW" : DTWAIN_GetCurrentFileNameWDelegate)
    let private GetCurrentPageNum = lazy (DynamicDll.Bind "DTWAIN_GetCurrentPageNum" : DTWAIN_GetCurrentPageNumDelegate)
    let private GetCurrentRetryCount = lazy (DynamicDll.Bind "DTWAIN_GetCurrentRetryCount" : DTWAIN_GetCurrentRetryCountDelegate)
    let private GetCurrentTwainTriplet = lazy (DynamicDll.Bind "DTWAIN_GetCurrentTwainTriplet" : DTWAIN_GetCurrentTwainTripletDelegate)
    let private GetCustomDSData = lazy (DynamicDll.Bind "DTWAIN_GetCustomDSData" : DTWAIN_GetCustomDSDataDelegate)
    let private GetDSMFullName = lazy (DynamicDll.Bind "DTWAIN_GetDSMFullName" : DTWAIN_GetDSMFullNameDelegate)
    let private GetDSMFullNameA = lazy (DynamicDll.Bind "DTWAIN_GetDSMFullNameA" : DTWAIN_GetDSMFullNameADelegate)
    let private GetDSMFullNameW = lazy (DynamicDll.Bind "DTWAIN_GetDSMFullNameW" : DTWAIN_GetDSMFullNameWDelegate)
    let private GetDSMSearchOrder = lazy (DynamicDll.Bind "DTWAIN_GetDSMSearchOrder" : DTWAIN_GetDSMSearchOrderDelegate)
    let private GetDTWAINHandle = lazy (DynamicDll.Bind "DTWAIN_GetDTWAINHandle" : DTWAIN_GetDTWAINHandleDelegate)
    let private GetDeviceEvent = lazy (DynamicDll.Bind "DTWAIN_GetDeviceEvent" : DTWAIN_GetDeviceEventDelegate)
    let private GetDeviceEventEx = lazy (DynamicDll.Bind "DTWAIN_GetDeviceEventEx" : DTWAIN_GetDeviceEventExDelegate)
    let private GetDeviceEventInfo = lazy (DynamicDll.Bind "DTWAIN_GetDeviceEventInfo" : DTWAIN_GetDeviceEventInfoDelegate)
    let private GetDeviceNotifications = lazy (DynamicDll.Bind "DTWAIN_GetDeviceNotifications" : DTWAIN_GetDeviceNotificationsDelegate)
    let private GetDeviceTimeDate = lazy (DynamicDll.Bind "DTWAIN_GetDeviceTimeDate" : DTWAIN_GetDeviceTimeDateDelegate)
    let private GetDeviceTimeDateA = lazy (DynamicDll.Bind "DTWAIN_GetDeviceTimeDateA" : DTWAIN_GetDeviceTimeDateADelegate)
    let private GetDeviceTimeDateW = lazy (DynamicDll.Bind "DTWAIN_GetDeviceTimeDateW" : DTWAIN_GetDeviceTimeDateWDelegate)
    let private GetDoubleFeedDetectLength = lazy (DynamicDll.Bind "DTWAIN_GetDoubleFeedDetectLength" : DTWAIN_GetDoubleFeedDetectLengthDelegate)
    let private GetDoubleFeedDetectValues = lazy (DynamicDll.Bind "DTWAIN_GetDoubleFeedDetectValues" : DTWAIN_GetDoubleFeedDetectValuesDelegate)
    let private GetDuplexType = lazy (DynamicDll.Bind "DTWAIN_GetDuplexType" : DTWAIN_GetDuplexTypeDelegate)
    let private GetErrorBuffer = lazy (DynamicDll.Bind "DTWAIN_GetErrorBuffer" : DTWAIN_GetErrorBufferDelegate)
    let private GetErrorBufferThreshold = lazy (DynamicDll.Bind "DTWAIN_GetErrorBufferThreshold" : DTWAIN_GetErrorBufferThresholdDelegate)
    let private GetErrorCallback = lazy (DynamicDll.Bind "DTWAIN_GetErrorCallback" : DTWAIN_GetErrorCallbackDelegate)
    let private GetErrorCallback64 = lazy (DynamicDll.Bind "DTWAIN_GetErrorCallback64" : DTWAIN_GetErrorCallback64Delegate)
    let private GetErrorString = lazy (DynamicDll.Bind "DTWAIN_GetErrorString" : DTWAIN_GetErrorStringDelegate)
    let private GetErrorStringA = lazy (DynamicDll.Bind "DTWAIN_GetErrorStringA" : DTWAIN_GetErrorStringADelegate)
    let private GetErrorStringW = lazy (DynamicDll.Bind "DTWAIN_GetErrorStringW" : DTWAIN_GetErrorStringWDelegate)
    let private GetExtCapFromName = lazy (DynamicDll.Bind "DTWAIN_GetExtCapFromName" : DTWAIN_GetExtCapFromNameDelegate)
    let private GetExtCapFromNameA = lazy (DynamicDll.Bind "DTWAIN_GetExtCapFromNameA" : DTWAIN_GetExtCapFromNameADelegate)
    let private GetExtCapFromNameW = lazy (DynamicDll.Bind "DTWAIN_GetExtCapFromNameW" : DTWAIN_GetExtCapFromNameWDelegate)
    let private GetExtImageInfo = lazy (DynamicDll.Bind "DTWAIN_GetExtImageInfo" : DTWAIN_GetExtImageInfoDelegate)
    let private GetExtImageInfoData = lazy (DynamicDll.Bind "DTWAIN_GetExtImageInfoData" : DTWAIN_GetExtImageInfoDataDelegate)
    let private GetExtImageInfoDataEx = lazy (DynamicDll.Bind "DTWAIN_GetExtImageInfoDataEx" : DTWAIN_GetExtImageInfoDataExDelegate)
    let private GetExtImageInfoItem = lazy (DynamicDll.Bind "DTWAIN_GetExtImageInfoItem" : DTWAIN_GetExtImageInfoItemDelegate)
    let private GetExtImageInfoItemEx = lazy (DynamicDll.Bind "DTWAIN_GetExtImageInfoItemEx" : DTWAIN_GetExtImageInfoItemExDelegate)
    let private GetExtNameFromCap = lazy (DynamicDll.Bind "DTWAIN_GetExtNameFromCap" : DTWAIN_GetExtNameFromCapDelegate)
    let private GetExtNameFromCapA = lazy (DynamicDll.Bind "DTWAIN_GetExtNameFromCapA" : DTWAIN_GetExtNameFromCapADelegate)
    let private GetExtNameFromCapW = lazy (DynamicDll.Bind "DTWAIN_GetExtNameFromCapW" : DTWAIN_GetExtNameFromCapWDelegate)
    let private GetFeederAlignment = lazy (DynamicDll.Bind "DTWAIN_GetFeederAlignment" : DTWAIN_GetFeederAlignmentDelegate)
    let private GetFeederFuncs = lazy (DynamicDll.Bind "DTWAIN_GetFeederFuncs" : DTWAIN_GetFeederFuncsDelegate)
    let private GetFeederOrder = lazy (DynamicDll.Bind "DTWAIN_GetFeederOrder" : DTWAIN_GetFeederOrderDelegate)
    let private GetFeederWaitTime = lazy (DynamicDll.Bind "DTWAIN_GetFeederWaitTime" : DTWAIN_GetFeederWaitTimeDelegate)
    let private GetFileCompressionType = lazy (DynamicDll.Bind "DTWAIN_GetFileCompressionType" : DTWAIN_GetFileCompressionTypeDelegate)
    let private GetFileTypeExtensions = lazy (DynamicDll.Bind "DTWAIN_GetFileTypeExtensions" : DTWAIN_GetFileTypeExtensionsDelegate)
    let private GetFileTypeExtensionsA = lazy (DynamicDll.Bind "DTWAIN_GetFileTypeExtensionsA" : DTWAIN_GetFileTypeExtensionsADelegate)
    let private GetFileTypeExtensionsW = lazy (DynamicDll.Bind "DTWAIN_GetFileTypeExtensionsW" : DTWAIN_GetFileTypeExtensionsWDelegate)
    let private GetFileTypeName = lazy (DynamicDll.Bind "DTWAIN_GetFileTypeName" : DTWAIN_GetFileTypeNameDelegate)
    let private GetFileTypeNameA = lazy (DynamicDll.Bind "DTWAIN_GetFileTypeNameA" : DTWAIN_GetFileTypeNameADelegate)
    let private GetFileTypeNameW = lazy (DynamicDll.Bind "DTWAIN_GetFileTypeNameW" : DTWAIN_GetFileTypeNameWDelegate)
    let private GetHalftone = lazy (DynamicDll.Bind "DTWAIN_GetHalftone" : DTWAIN_GetHalftoneDelegate)
    let private GetHalftoneA = lazy (DynamicDll.Bind "DTWAIN_GetHalftoneA" : DTWAIN_GetHalftoneADelegate)
    let private GetHalftoneW = lazy (DynamicDll.Bind "DTWAIN_GetHalftoneW" : DTWAIN_GetHalftoneWDelegate)
    let private GetHighlight = lazy (DynamicDll.Bind "DTWAIN_GetHighlight" : DTWAIN_GetHighlightDelegate)
    let private GetHighlightString = lazy (DynamicDll.Bind "DTWAIN_GetHighlightString" : DTWAIN_GetHighlightStringDelegate)
    let private GetHighlightStringA = lazy (DynamicDll.Bind "DTWAIN_GetHighlightStringA" : DTWAIN_GetHighlightStringADelegate)
    let private GetHighlightStringW = lazy (DynamicDll.Bind "DTWAIN_GetHighlightStringW" : DTWAIN_GetHighlightStringWDelegate)
    let private GetImageInfo = lazy (DynamicDll.Bind "DTWAIN_GetImageInfo" : DTWAIN_GetImageInfoDelegate)
    let private GetImageInfoString = lazy (DynamicDll.Bind "DTWAIN_GetImageInfoString" : DTWAIN_GetImageInfoStringDelegate)
    let private GetImageInfoStringA = lazy (DynamicDll.Bind "DTWAIN_GetImageInfoStringA" : DTWAIN_GetImageInfoStringADelegate)
    let private GetImageInfoStringW = lazy (DynamicDll.Bind "DTWAIN_GetImageInfoStringW" : DTWAIN_GetImageInfoStringWDelegate)
    let private GetJobControl = lazy (DynamicDll.Bind "DTWAIN_GetJobControl" : DTWAIN_GetJobControlDelegate)
    let private GetJpegValues = lazy (DynamicDll.Bind "DTWAIN_GetJpegValues" : DTWAIN_GetJpegValuesDelegate)
    let private GetJpegXRValues = lazy (DynamicDll.Bind "DTWAIN_GetJpegXRValues" : DTWAIN_GetJpegXRValuesDelegate)
    let private GetLanguage = lazy (DynamicDll.Bind "DTWAIN_GetLanguage" : DTWAIN_GetLanguageDelegate)
    let private GetLastError = lazy (DynamicDll.Bind "DTWAIN_GetLastError" : DTWAIN_GetLastErrorDelegate)
    let private GetLibraryPath = lazy (DynamicDll.Bind "DTWAIN_GetLibraryPath" : DTWAIN_GetLibraryPathDelegate)
    let private GetLibraryPathA = lazy (DynamicDll.Bind "DTWAIN_GetLibraryPathA" : DTWAIN_GetLibraryPathADelegate)
    let private GetLibraryPathW = lazy (DynamicDll.Bind "DTWAIN_GetLibraryPathW" : DTWAIN_GetLibraryPathWDelegate)
    let private GetLightPath = lazy (DynamicDll.Bind "DTWAIN_GetLightPath" : DTWAIN_GetLightPathDelegate)
    let private GetLightSource = lazy (DynamicDll.Bind "DTWAIN_GetLightSource" : DTWAIN_GetLightSourceDelegate)
    let private GetLightSources = lazy (DynamicDll.Bind "DTWAIN_GetLightSources" : DTWAIN_GetLightSourcesDelegate)
    let private GetLoggerCallback = lazy (DynamicDll.Bind "DTWAIN_GetLoggerCallback" : DTWAIN_GetLoggerCallbackDelegate)
    let private GetLoggerCallbackA = lazy (DynamicDll.Bind "DTWAIN_GetLoggerCallbackA" : DTWAIN_GetLoggerCallbackADelegate)
    let private GetLoggerCallbackW = lazy (DynamicDll.Bind "DTWAIN_GetLoggerCallbackW" : DTWAIN_GetLoggerCallbackWDelegate)
    let private GetManualDuplexCount = lazy (DynamicDll.Bind "DTWAIN_GetManualDuplexCount" : DTWAIN_GetManualDuplexCountDelegate)
    let private GetMaxAcquisitions = lazy (DynamicDll.Bind "DTWAIN_GetMaxAcquisitions" : DTWAIN_GetMaxAcquisitionsDelegate)
    let private GetMaxBuffers = lazy (DynamicDll.Bind "DTWAIN_GetMaxBuffers" : DTWAIN_GetMaxBuffersDelegate)
    let private GetMaxPagesToAcquire = lazy (DynamicDll.Bind "DTWAIN_GetMaxPagesToAcquire" : DTWAIN_GetMaxPagesToAcquireDelegate)
    let private GetMaxRetryAttempts = lazy (DynamicDll.Bind "DTWAIN_GetMaxRetryAttempts" : DTWAIN_GetMaxRetryAttemptsDelegate)
    let private GetNameFromCap = lazy (DynamicDll.Bind "DTWAIN_GetNameFromCap" : DTWAIN_GetNameFromCapDelegate)
    let private GetNameFromCapA = lazy (DynamicDll.Bind "DTWAIN_GetNameFromCapA" : DTWAIN_GetNameFromCapADelegate)
    let private GetNameFromCapW = lazy (DynamicDll.Bind "DTWAIN_GetNameFromCapW" : DTWAIN_GetNameFromCapWDelegate)
    let private GetNoiseFilter = lazy (DynamicDll.Bind "DTWAIN_GetNoiseFilter" : DTWAIN_GetNoiseFilterDelegate)
    let private GetNumAcquiredImages = lazy (DynamicDll.Bind "DTWAIN_GetNumAcquiredImages" : DTWAIN_GetNumAcquiredImagesDelegate)
    let private GetNumAcquisitions = lazy (DynamicDll.Bind "DTWAIN_GetNumAcquisitions" : DTWAIN_GetNumAcquisitionsDelegate)
    let private GetOCRCapValues = lazy (DynamicDll.Bind "DTWAIN_GetOCRCapValues" : DTWAIN_GetOCRCapValuesDelegate)
    let private GetOCRErrorString = lazy (DynamicDll.Bind "DTWAIN_GetOCRErrorString" : DTWAIN_GetOCRErrorStringDelegate)
    let private GetOCRErrorStringA = lazy (DynamicDll.Bind "DTWAIN_GetOCRErrorStringA" : DTWAIN_GetOCRErrorStringADelegate)
    let private GetOCRErrorStringW = lazy (DynamicDll.Bind "DTWAIN_GetOCRErrorStringW" : DTWAIN_GetOCRErrorStringWDelegate)
    let private GetOCRLastError = lazy (DynamicDll.Bind "DTWAIN_GetOCRLastError" : DTWAIN_GetOCRLastErrorDelegate)
    let private GetOCRMajorMinorVersion = lazy (DynamicDll.Bind "DTWAIN_GetOCRMajorMinorVersion" : DTWAIN_GetOCRMajorMinorVersionDelegate)
    let private GetOCRManufacturer = lazy (DynamicDll.Bind "DTWAIN_GetOCRManufacturer" : DTWAIN_GetOCRManufacturerDelegate)
    let private GetOCRManufacturerA = lazy (DynamicDll.Bind "DTWAIN_GetOCRManufacturerA" : DTWAIN_GetOCRManufacturerADelegate)
    let private GetOCRManufacturerW = lazy (DynamicDll.Bind "DTWAIN_GetOCRManufacturerW" : DTWAIN_GetOCRManufacturerWDelegate)
    let private GetOCRProductFamily = lazy (DynamicDll.Bind "DTWAIN_GetOCRProductFamily" : DTWAIN_GetOCRProductFamilyDelegate)
    let private GetOCRProductFamilyA = lazy (DynamicDll.Bind "DTWAIN_GetOCRProductFamilyA" : DTWAIN_GetOCRProductFamilyADelegate)
    let private GetOCRProductFamilyW = lazy (DynamicDll.Bind "DTWAIN_GetOCRProductFamilyW" : DTWAIN_GetOCRProductFamilyWDelegate)
    let private GetOCRProductName = lazy (DynamicDll.Bind "DTWAIN_GetOCRProductName" : DTWAIN_GetOCRProductNameDelegate)
    let private GetOCRProductNameA = lazy (DynamicDll.Bind "DTWAIN_GetOCRProductNameA" : DTWAIN_GetOCRProductNameADelegate)
    let private GetOCRProductNameW = lazy (DynamicDll.Bind "DTWAIN_GetOCRProductNameW" : DTWAIN_GetOCRProductNameWDelegate)
    let private GetOCRText = lazy (DynamicDll.Bind "DTWAIN_GetOCRText" : DTWAIN_GetOCRTextDelegate)
    let private GetOCRTextA = lazy (DynamicDll.Bind "DTWAIN_GetOCRTextA" : DTWAIN_GetOCRTextADelegate)
    let private GetOCRTextInfoFloat = lazy (DynamicDll.Bind "DTWAIN_GetOCRTextInfoFloat" : DTWAIN_GetOCRTextInfoFloatDelegate)
    let private GetOCRTextInfoFloatEx = lazy (DynamicDll.Bind "DTWAIN_GetOCRTextInfoFloatEx" : DTWAIN_GetOCRTextInfoFloatExDelegate)
    let private GetOCRTextInfoHandle = lazy (DynamicDll.Bind "DTWAIN_GetOCRTextInfoHandle" : DTWAIN_GetOCRTextInfoHandleDelegate)
    let private GetOCRTextInfoLong = lazy (DynamicDll.Bind "DTWAIN_GetOCRTextInfoLong" : DTWAIN_GetOCRTextInfoLongDelegate)
    let private GetOCRTextInfoLongEx = lazy (DynamicDll.Bind "DTWAIN_GetOCRTextInfoLongEx" : DTWAIN_GetOCRTextInfoLongExDelegate)
    let private GetOCRTextW = lazy (DynamicDll.Bind "DTWAIN_GetOCRTextW" : DTWAIN_GetOCRTextWDelegate)
    let private GetOCRVersionInfo = lazy (DynamicDll.Bind "DTWAIN_GetOCRVersionInfo" : DTWAIN_GetOCRVersionInfoDelegate)
    let private GetOCRVersionInfoA = lazy (DynamicDll.Bind "DTWAIN_GetOCRVersionInfoA" : DTWAIN_GetOCRVersionInfoADelegate)
    let private GetOCRVersionInfoW = lazy (DynamicDll.Bind "DTWAIN_GetOCRVersionInfoW" : DTWAIN_GetOCRVersionInfoWDelegate)
    let private GetOrientation = lazy (DynamicDll.Bind "DTWAIN_GetOrientation" : DTWAIN_GetOrientationDelegate)
    let private GetOverscan = lazy (DynamicDll.Bind "DTWAIN_GetOverscan" : DTWAIN_GetOverscanDelegate)
    let private GetPDFTextElementFloat = lazy (DynamicDll.Bind "DTWAIN_GetPDFTextElementFloat" : DTWAIN_GetPDFTextElementFloatDelegate)
    let private GetPDFTextElementLong = lazy (DynamicDll.Bind "DTWAIN_GetPDFTextElementLong" : DTWAIN_GetPDFTextElementLongDelegate)
    let private GetPDFTextElementString = lazy (DynamicDll.Bind "DTWAIN_GetPDFTextElementString" : DTWAIN_GetPDFTextElementStringDelegate)
    let private GetPDFTextElementStringA = lazy (DynamicDll.Bind "DTWAIN_GetPDFTextElementStringA" : DTWAIN_GetPDFTextElementStringADelegate)
    let private GetPDFTextElementStringW = lazy (DynamicDll.Bind "DTWAIN_GetPDFTextElementStringW" : DTWAIN_GetPDFTextElementStringWDelegate)
    let private GetPDFType1FontName = lazy (DynamicDll.Bind "DTWAIN_GetPDFType1FontName" : DTWAIN_GetPDFType1FontNameDelegate)
    let private GetPDFType1FontNameA = lazy (DynamicDll.Bind "DTWAIN_GetPDFType1FontNameA" : DTWAIN_GetPDFType1FontNameADelegate)
    let private GetPDFType1FontNameW = lazy (DynamicDll.Bind "DTWAIN_GetPDFType1FontNameW" : DTWAIN_GetPDFType1FontNameWDelegate)
    let private GetPaperSize = lazy (DynamicDll.Bind "DTWAIN_GetPaperSize" : DTWAIN_GetPaperSizeDelegate)
    let private GetPaperSizeName = lazy (DynamicDll.Bind "DTWAIN_GetPaperSizeName" : DTWAIN_GetPaperSizeNameDelegate)
    let private GetPaperSizeNameA = lazy (DynamicDll.Bind "DTWAIN_GetPaperSizeNameA" : DTWAIN_GetPaperSizeNameADelegate)
    let private GetPaperSizeNameW = lazy (DynamicDll.Bind "DTWAIN_GetPaperSizeNameW" : DTWAIN_GetPaperSizeNameWDelegate)
    let private GetPatchMaxPriorities = lazy (DynamicDll.Bind "DTWAIN_GetPatchMaxPriorities" : DTWAIN_GetPatchMaxPrioritiesDelegate)
    let private GetPatchMaxRetries = lazy (DynamicDll.Bind "DTWAIN_GetPatchMaxRetries" : DTWAIN_GetPatchMaxRetriesDelegate)
    let private GetPatchPriorities = lazy (DynamicDll.Bind "DTWAIN_GetPatchPriorities" : DTWAIN_GetPatchPrioritiesDelegate)
    let private GetPatchSearchMode = lazy (DynamicDll.Bind "DTWAIN_GetPatchSearchMode" : DTWAIN_GetPatchSearchModeDelegate)
    let private GetPatchTimeOut = lazy (DynamicDll.Bind "DTWAIN_GetPatchTimeOut" : DTWAIN_GetPatchTimeOutDelegate)
    let private GetPixelFlavor = lazy (DynamicDll.Bind "DTWAIN_GetPixelFlavor" : DTWAIN_GetPixelFlavorDelegate)
    let private GetPixelType = lazy (DynamicDll.Bind "DTWAIN_GetPixelType" : DTWAIN_GetPixelTypeDelegate)
    let private GetPrinter = lazy (DynamicDll.Bind "DTWAIN_GetPrinter" : DTWAIN_GetPrinterDelegate)
    let private GetPrinterStartNumber = lazy (DynamicDll.Bind "DTWAIN_GetPrinterStartNumber" : DTWAIN_GetPrinterStartNumberDelegate)
    let private GetPrinterStringMode = lazy (DynamicDll.Bind "DTWAIN_GetPrinterStringMode" : DTWAIN_GetPrinterStringModeDelegate)
    let private GetPrinterStrings = lazy (DynamicDll.Bind "DTWAIN_GetPrinterStrings" : DTWAIN_GetPrinterStringsDelegate)
    let private GetPrinterSuffixString = lazy (DynamicDll.Bind "DTWAIN_GetPrinterSuffixString" : DTWAIN_GetPrinterSuffixStringDelegate)
    let private GetPrinterSuffixStringA = lazy (DynamicDll.Bind "DTWAIN_GetPrinterSuffixStringA" : DTWAIN_GetPrinterSuffixStringADelegate)
    let private GetPrinterSuffixStringW = lazy (DynamicDll.Bind "DTWAIN_GetPrinterSuffixStringW" : DTWAIN_GetPrinterSuffixStringWDelegate)
    let private GetRegisteredMsg = lazy (DynamicDll.Bind "DTWAIN_GetRegisteredMsg" : DTWAIN_GetRegisteredMsgDelegate)
    let private GetResolution = lazy (DynamicDll.Bind "DTWAIN_GetResolution" : DTWAIN_GetResolutionDelegate)
    let private GetResolutionString = lazy (DynamicDll.Bind "DTWAIN_GetResolutionString" : DTWAIN_GetResolutionStringDelegate)
    let private GetResolutionStringA = lazy (DynamicDll.Bind "DTWAIN_GetResolutionStringA" : DTWAIN_GetResolutionStringADelegate)
    let private GetResolutionStringW = lazy (DynamicDll.Bind "DTWAIN_GetResolutionStringW" : DTWAIN_GetResolutionStringWDelegate)
    let private GetResourceString = lazy (DynamicDll.Bind "DTWAIN_GetResourceString" : DTWAIN_GetResourceStringDelegate)
    let private GetResourceStringA = lazy (DynamicDll.Bind "DTWAIN_GetResourceStringA" : DTWAIN_GetResourceStringADelegate)
    let private GetResourceStringW = lazy (DynamicDll.Bind "DTWAIN_GetResourceStringW" : DTWAIN_GetResourceStringWDelegate)
    let private GetRotation = lazy (DynamicDll.Bind "DTWAIN_GetRotation" : DTWAIN_GetRotationDelegate)
    let private GetRotationString = lazy (DynamicDll.Bind "DTWAIN_GetRotationString" : DTWAIN_GetRotationStringDelegate)
    let private GetRotationStringA = lazy (DynamicDll.Bind "DTWAIN_GetRotationStringA" : DTWAIN_GetRotationStringADelegate)
    let private GetRotationStringW = lazy (DynamicDll.Bind "DTWAIN_GetRotationStringW" : DTWAIN_GetRotationStringWDelegate)
    let private GetSaveFileName = lazy (DynamicDll.Bind "DTWAIN_GetSaveFileName" : DTWAIN_GetSaveFileNameDelegate)
    let private GetSaveFileNameA = lazy (DynamicDll.Bind "DTWAIN_GetSaveFileNameA" : DTWAIN_GetSaveFileNameADelegate)
    let private GetSaveFileNameW = lazy (DynamicDll.Bind "DTWAIN_GetSaveFileNameW" : DTWAIN_GetSaveFileNameWDelegate)
    let private GetSavedFilesCount = lazy (DynamicDll.Bind "DTWAIN_GetSavedFilesCount" : DTWAIN_GetSavedFilesCountDelegate)
    let private GetSessionDetails = lazy (DynamicDll.Bind "DTWAIN_GetSessionDetails" : DTWAIN_GetSessionDetailsDelegate)
    let private GetSessionDetailsA = lazy (DynamicDll.Bind "DTWAIN_GetSessionDetailsA" : DTWAIN_GetSessionDetailsADelegate)
    let private GetSessionDetailsW = lazy (DynamicDll.Bind "DTWAIN_GetSessionDetailsW" : DTWAIN_GetSessionDetailsWDelegate)
    let private GetShadow = lazy (DynamicDll.Bind "DTWAIN_GetShadow" : DTWAIN_GetShadowDelegate)
    let private GetShadowString = lazy (DynamicDll.Bind "DTWAIN_GetShadowString" : DTWAIN_GetShadowStringDelegate)
    let private GetShadowStringA = lazy (DynamicDll.Bind "DTWAIN_GetShadowStringA" : DTWAIN_GetShadowStringADelegate)
    let private GetShadowStringW = lazy (DynamicDll.Bind "DTWAIN_GetShadowStringW" : DTWAIN_GetShadowStringWDelegate)
    let private GetShortVersionString = lazy (DynamicDll.Bind "DTWAIN_GetShortVersionString" : DTWAIN_GetShortVersionStringDelegate)
    let private GetShortVersionStringA = lazy (DynamicDll.Bind "DTWAIN_GetShortVersionStringA" : DTWAIN_GetShortVersionStringADelegate)
    let private GetShortVersionStringW = lazy (DynamicDll.Bind "DTWAIN_GetShortVersionStringW" : DTWAIN_GetShortVersionStringWDelegate)
    let private GetSourceAcquisitions = lazy (DynamicDll.Bind "DTWAIN_GetSourceAcquisitions" : DTWAIN_GetSourceAcquisitionsDelegate)
    let private GetSourceDetails = lazy (DynamicDll.Bind "DTWAIN_GetSourceDetails" : DTWAIN_GetSourceDetailsDelegate)
    let private GetSourceDetailsA = lazy (DynamicDll.Bind "DTWAIN_GetSourceDetailsA" : DTWAIN_GetSourceDetailsADelegate)
    let private GetSourceDetailsW = lazy (DynamicDll.Bind "DTWAIN_GetSourceDetailsW" : DTWAIN_GetSourceDetailsWDelegate)
    let private GetSourceID = lazy (DynamicDll.Bind "DTWAIN_GetSourceID" : DTWAIN_GetSourceIDDelegate)
    let private GetSourceIDEx = lazy (DynamicDll.Bind "DTWAIN_GetSourceIDEx" : DTWAIN_GetSourceIDExDelegate)
    let private GetSourceManufacturer = lazy (DynamicDll.Bind "DTWAIN_GetSourceManufacturer" : DTWAIN_GetSourceManufacturerDelegate)
    let private GetSourceManufacturerA = lazy (DynamicDll.Bind "DTWAIN_GetSourceManufacturerA" : DTWAIN_GetSourceManufacturerADelegate)
    let private GetSourceManufacturerW = lazy (DynamicDll.Bind "DTWAIN_GetSourceManufacturerW" : DTWAIN_GetSourceManufacturerWDelegate)
    let private GetSourceProductFamily = lazy (DynamicDll.Bind "DTWAIN_GetSourceProductFamily" : DTWAIN_GetSourceProductFamilyDelegate)
    let private GetSourceProductFamilyA = lazy (DynamicDll.Bind "DTWAIN_GetSourceProductFamilyA" : DTWAIN_GetSourceProductFamilyADelegate)
    let private GetSourceProductFamilyW = lazy (DynamicDll.Bind "DTWAIN_GetSourceProductFamilyW" : DTWAIN_GetSourceProductFamilyWDelegate)
    let private GetSourceProductName = lazy (DynamicDll.Bind "DTWAIN_GetSourceProductName" : DTWAIN_GetSourceProductNameDelegate)
    let private GetSourceProductNameA = lazy (DynamicDll.Bind "DTWAIN_GetSourceProductNameA" : DTWAIN_GetSourceProductNameADelegate)
    let private GetSourceProductNameW = lazy (DynamicDll.Bind "DTWAIN_GetSourceProductNameW" : DTWAIN_GetSourceProductNameWDelegate)
    let private GetSourceUnit = lazy (DynamicDll.Bind "DTWAIN_GetSourceUnit" : DTWAIN_GetSourceUnitDelegate)
    let private GetSourceVersionInfo = lazy (DynamicDll.Bind "DTWAIN_GetSourceVersionInfo" : DTWAIN_GetSourceVersionInfoDelegate)
    let private GetSourceVersionInfoA = lazy (DynamicDll.Bind "DTWAIN_GetSourceVersionInfoA" : DTWAIN_GetSourceVersionInfoADelegate)
    let private GetSourceVersionInfoW = lazy (DynamicDll.Bind "DTWAIN_GetSourceVersionInfoW" : DTWAIN_GetSourceVersionInfoWDelegate)
    let private GetSourceVersionNumber = lazy (DynamicDll.Bind "DTWAIN_GetSourceVersionNumber" : DTWAIN_GetSourceVersionNumberDelegate)
    let private GetStaticLibVersion = lazy (DynamicDll.Bind "DTWAIN_GetStaticLibVersion" : DTWAIN_GetStaticLibVersionDelegate)
    let private GetTempFileDirectory = lazy (DynamicDll.Bind "DTWAIN_GetTempFileDirectory" : DTWAIN_GetTempFileDirectoryDelegate)
    let private GetTempFileDirectoryA = lazy (DynamicDll.Bind "DTWAIN_GetTempFileDirectoryA" : DTWAIN_GetTempFileDirectoryADelegate)
    let private GetTempFileDirectoryW = lazy (DynamicDll.Bind "DTWAIN_GetTempFileDirectoryW" : DTWAIN_GetTempFileDirectoryWDelegate)
    let private GetThreshold = lazy (DynamicDll.Bind "DTWAIN_GetThreshold" : DTWAIN_GetThresholdDelegate)
    let private GetThresholdString = lazy (DynamicDll.Bind "DTWAIN_GetThresholdString" : DTWAIN_GetThresholdStringDelegate)
    let private GetThresholdStringA = lazy (DynamicDll.Bind "DTWAIN_GetThresholdStringA" : DTWAIN_GetThresholdStringADelegate)
    let private GetThresholdStringW = lazy (DynamicDll.Bind "DTWAIN_GetThresholdStringW" : DTWAIN_GetThresholdStringWDelegate)
    let private GetTimeDate = lazy (DynamicDll.Bind "DTWAIN_GetTimeDate" : DTWAIN_GetTimeDateDelegate)
    let private GetTimeDateA = lazy (DynamicDll.Bind "DTWAIN_GetTimeDateA" : DTWAIN_GetTimeDateADelegate)
    let private GetTimeDateW = lazy (DynamicDll.Bind "DTWAIN_GetTimeDateW" : DTWAIN_GetTimeDateWDelegate)
    let private GetTwainAppID = lazy (DynamicDll.Bind "DTWAIN_GetTwainAppID" : DTWAIN_GetTwainAppIDDelegate)
    let private GetTwainAppIDEx = lazy (DynamicDll.Bind "DTWAIN_GetTwainAppIDEx" : DTWAIN_GetTwainAppIDExDelegate)
    let private GetTwainAvailability = lazy (DynamicDll.Bind "DTWAIN_GetTwainAvailability" : DTWAIN_GetTwainAvailabilityDelegate)
    let private GetTwainAvailabilityEx = lazy (DynamicDll.Bind "DTWAIN_GetTwainAvailabilityEx" : DTWAIN_GetTwainAvailabilityExDelegate)
    let private GetTwainAvailabilityExA = lazy (DynamicDll.Bind "DTWAIN_GetTwainAvailabilityExA" : DTWAIN_GetTwainAvailabilityExADelegate)
    let private GetTwainAvailabilityExW = lazy (DynamicDll.Bind "DTWAIN_GetTwainAvailabilityExW" : DTWAIN_GetTwainAvailabilityExWDelegate)
    let private GetTwainCountryName = lazy (DynamicDll.Bind "DTWAIN_GetTwainCountryName" : DTWAIN_GetTwainCountryNameDelegate)
    let private GetTwainCountryNameA = lazy (DynamicDll.Bind "DTWAIN_GetTwainCountryNameA" : DTWAIN_GetTwainCountryNameADelegate)
    let private GetTwainCountryNameW = lazy (DynamicDll.Bind "DTWAIN_GetTwainCountryNameW" : DTWAIN_GetTwainCountryNameWDelegate)
    let private GetTwainCountryValue = lazy (DynamicDll.Bind "DTWAIN_GetTwainCountryValue" : DTWAIN_GetTwainCountryValueDelegate)
    let private GetTwainCountryValueA = lazy (DynamicDll.Bind "DTWAIN_GetTwainCountryValueA" : DTWAIN_GetTwainCountryValueADelegate)
    let private GetTwainCountryValueW = lazy (DynamicDll.Bind "DTWAIN_GetTwainCountryValueW" : DTWAIN_GetTwainCountryValueWDelegate)
    let private GetTwainHwnd = lazy (DynamicDll.Bind "DTWAIN_GetTwainHwnd" : DTWAIN_GetTwainHwndDelegate)
    let private GetTwainIDFromName = lazy (DynamicDll.Bind "DTWAIN_GetTwainIDFromName" : DTWAIN_GetTwainIDFromNameDelegate)
    let private GetTwainIDFromNameA = lazy (DynamicDll.Bind "DTWAIN_GetTwainIDFromNameA" : DTWAIN_GetTwainIDFromNameADelegate)
    let private GetTwainIDFromNameW = lazy (DynamicDll.Bind "DTWAIN_GetTwainIDFromNameW" : DTWAIN_GetTwainIDFromNameWDelegate)
    let private GetTwainLanguageName = lazy (DynamicDll.Bind "DTWAIN_GetTwainLanguageName" : DTWAIN_GetTwainLanguageNameDelegate)
    let private GetTwainLanguageNameA = lazy (DynamicDll.Bind "DTWAIN_GetTwainLanguageNameA" : DTWAIN_GetTwainLanguageNameADelegate)
    let private GetTwainLanguageNameW = lazy (DynamicDll.Bind "DTWAIN_GetTwainLanguageNameW" : DTWAIN_GetTwainLanguageNameWDelegate)
    let private GetTwainLanguageValue = lazy (DynamicDll.Bind "DTWAIN_GetTwainLanguageValue" : DTWAIN_GetTwainLanguageValueDelegate)
    let private GetTwainLanguageValueA = lazy (DynamicDll.Bind "DTWAIN_GetTwainLanguageValueA" : DTWAIN_GetTwainLanguageValueADelegate)
    let private GetTwainLanguageValueW = lazy (DynamicDll.Bind "DTWAIN_GetTwainLanguageValueW" : DTWAIN_GetTwainLanguageValueWDelegate)
    let private GetTwainMode = lazy (DynamicDll.Bind "DTWAIN_GetTwainMode" : DTWAIN_GetTwainModeDelegate)
    let private GetTwainNameFromConstant = lazy (DynamicDll.Bind "DTWAIN_GetTwainNameFromConstant" : DTWAIN_GetTwainNameFromConstantDelegate)
    let private GetTwainNameFromConstantA = lazy (DynamicDll.Bind "DTWAIN_GetTwainNameFromConstantA" : DTWAIN_GetTwainNameFromConstantADelegate)
    let private GetTwainNameFromConstantW = lazy (DynamicDll.Bind "DTWAIN_GetTwainNameFromConstantW" : DTWAIN_GetTwainNameFromConstantWDelegate)
    let private GetTwainStringName = lazy (DynamicDll.Bind "DTWAIN_GetTwainStringName" : DTWAIN_GetTwainStringNameDelegate)
    let private GetTwainStringNameA = lazy (DynamicDll.Bind "DTWAIN_GetTwainStringNameA" : DTWAIN_GetTwainStringNameADelegate)
    let private GetTwainStringNameW = lazy (DynamicDll.Bind "DTWAIN_GetTwainStringNameW" : DTWAIN_GetTwainStringNameWDelegate)
    let private GetTwainTimeout = lazy (DynamicDll.Bind "DTWAIN_GetTwainTimeout" : DTWAIN_GetTwainTimeoutDelegate)
    let private GetVersion = lazy (DynamicDll.Bind "DTWAIN_GetVersion" : DTWAIN_GetVersionDelegate)
    let private GetVersionCopyright = lazy (DynamicDll.Bind "DTWAIN_GetVersionCopyright" : DTWAIN_GetVersionCopyrightDelegate)
    let private GetVersionCopyrightA = lazy (DynamicDll.Bind "DTWAIN_GetVersionCopyrightA" : DTWAIN_GetVersionCopyrightADelegate)
    let private GetVersionCopyrightW = lazy (DynamicDll.Bind "DTWAIN_GetVersionCopyrightW" : DTWAIN_GetVersionCopyrightWDelegate)
    let private GetVersionEx = lazy (DynamicDll.Bind "DTWAIN_GetVersionEx" : DTWAIN_GetVersionExDelegate)
    let private GetVersionInfo = lazy (DynamicDll.Bind "DTWAIN_GetVersionInfo" : DTWAIN_GetVersionInfoDelegate)
    let private GetVersionInfoA = lazy (DynamicDll.Bind "DTWAIN_GetVersionInfoA" : DTWAIN_GetVersionInfoADelegate)
    let private GetVersionInfoW = lazy (DynamicDll.Bind "DTWAIN_GetVersionInfoW" : DTWAIN_GetVersionInfoWDelegate)
    let private GetVersionString = lazy (DynamicDll.Bind "DTWAIN_GetVersionString" : DTWAIN_GetVersionStringDelegate)
    let private GetVersionStringA = lazy (DynamicDll.Bind "DTWAIN_GetVersionStringA" : DTWAIN_GetVersionStringADelegate)
    let private GetVersionStringW = lazy (DynamicDll.Bind "DTWAIN_GetVersionStringW" : DTWAIN_GetVersionStringWDelegate)
    let private GetWindowsVersionInfo = lazy (DynamicDll.Bind "DTWAIN_GetWindowsVersionInfo" : DTWAIN_GetWindowsVersionInfoDelegate)
    let private GetWindowsVersionInfoA = lazy (DynamicDll.Bind "DTWAIN_GetWindowsVersionInfoA" : DTWAIN_GetWindowsVersionInfoADelegate)
    let private GetWindowsVersionInfoW = lazy (DynamicDll.Bind "DTWAIN_GetWindowsVersionInfoW" : DTWAIN_GetWindowsVersionInfoWDelegate)
    let private GetXResolution = lazy (DynamicDll.Bind "DTWAIN_GetXResolution" : DTWAIN_GetXResolutionDelegate)
    let private GetXResolutionString = lazy (DynamicDll.Bind "DTWAIN_GetXResolutionString" : DTWAIN_GetXResolutionStringDelegate)
    let private GetXResolutionStringA = lazy (DynamicDll.Bind "DTWAIN_GetXResolutionStringA" : DTWAIN_GetXResolutionStringADelegate)
    let private GetXResolutionStringW = lazy (DynamicDll.Bind "DTWAIN_GetXResolutionStringW" : DTWAIN_GetXResolutionStringWDelegate)
    let private GetYResolution = lazy (DynamicDll.Bind "DTWAIN_GetYResolution" : DTWAIN_GetYResolutionDelegate)
    let private GetYResolutionString = lazy (DynamicDll.Bind "DTWAIN_GetYResolutionString" : DTWAIN_GetYResolutionStringDelegate)
    let private GetYResolutionStringA = lazy (DynamicDll.Bind "DTWAIN_GetYResolutionStringA" : DTWAIN_GetYResolutionStringADelegate)
    let private GetYResolutionStringW = lazy (DynamicDll.Bind "DTWAIN_GetYResolutionStringW" : DTWAIN_GetYResolutionStringWDelegate)
    let private InitExtImageInfo = lazy (DynamicDll.Bind "DTWAIN_InitExtImageInfo" : DTWAIN_InitExtImageInfoDelegate)
    let private InitImageFileAppend = lazy (DynamicDll.Bind "DTWAIN_InitImageFileAppend" : DTWAIN_InitImageFileAppendDelegate)
    let private InitImageFileAppendA = lazy (DynamicDll.Bind "DTWAIN_InitImageFileAppendA" : DTWAIN_InitImageFileAppendADelegate)
    let private InitImageFileAppendW = lazy (DynamicDll.Bind "DTWAIN_InitImageFileAppendW" : DTWAIN_InitImageFileAppendWDelegate)
    let private InitOCRInterface = lazy (DynamicDll.Bind "DTWAIN_InitOCRInterface" : DTWAIN_InitOCRInterfaceDelegate)
    let private IsAcquiring = lazy (DynamicDll.Bind "DTWAIN_IsAcquiring" : DTWAIN_IsAcquiringDelegate)
    let private IsAudioXferSupported = lazy (DynamicDll.Bind "DTWAIN_IsAudioXferSupported" : DTWAIN_IsAudioXferSupportedDelegate)
    let private IsAutoBorderDetectEnabled = lazy (DynamicDll.Bind "DTWAIN_IsAutoBorderDetectEnabled" : DTWAIN_IsAutoBorderDetectEnabledDelegate)
    let private IsAutoBorderDetectSupported = lazy (DynamicDll.Bind "DTWAIN_IsAutoBorderDetectSupported" : DTWAIN_IsAutoBorderDetectSupportedDelegate)
    let private IsAutoBrightEnabled = lazy (DynamicDll.Bind "DTWAIN_IsAutoBrightEnabled" : DTWAIN_IsAutoBrightEnabledDelegate)
    let private IsAutoBrightSupported = lazy (DynamicDll.Bind "DTWAIN_IsAutoBrightSupported" : DTWAIN_IsAutoBrightSupportedDelegate)
    let private IsAutoDeskewEnabled = lazy (DynamicDll.Bind "DTWAIN_IsAutoDeskewEnabled" : DTWAIN_IsAutoDeskewEnabledDelegate)
    let private IsAutoDeskewSupported = lazy (DynamicDll.Bind "DTWAIN_IsAutoDeskewSupported" : DTWAIN_IsAutoDeskewSupportedDelegate)
    let private IsAutoFeedEnabled = lazy (DynamicDll.Bind "DTWAIN_IsAutoFeedEnabled" : DTWAIN_IsAutoFeedEnabledDelegate)
    let private IsAutoFeedSupported = lazy (DynamicDll.Bind "DTWAIN_IsAutoFeedSupported" : DTWAIN_IsAutoFeedSupportedDelegate)
    let private IsAutoRotateEnabled = lazy (DynamicDll.Bind "DTWAIN_IsAutoRotateEnabled" : DTWAIN_IsAutoRotateEnabledDelegate)
    let private IsAutoRotateSupported = lazy (DynamicDll.Bind "DTWAIN_IsAutoRotateSupported" : DTWAIN_IsAutoRotateSupportedDelegate)
    let private IsAutoScanEnabled = lazy (DynamicDll.Bind "DTWAIN_IsAutoScanEnabled" : DTWAIN_IsAutoScanEnabledDelegate)
    let private IsAutomaticSenseMediumEnabled = lazy (DynamicDll.Bind "DTWAIN_IsAutomaticSenseMediumEnabled" : DTWAIN_IsAutomaticSenseMediumEnabledDelegate)
    let private IsAutomaticSenseMediumSupported = lazy (DynamicDll.Bind "DTWAIN_IsAutomaticSenseMediumSupported" : DTWAIN_IsAutomaticSenseMediumSupportedDelegate)
    let private IsBlankPageDetectionOn = lazy (DynamicDll.Bind "DTWAIN_IsBlankPageDetectionOn" : DTWAIN_IsBlankPageDetectionOnDelegate)
    let private IsBufferedTileModeOn = lazy (DynamicDll.Bind "DTWAIN_IsBufferedTileModeOn" : DTWAIN_IsBufferedTileModeOnDelegate)
    let private IsBufferedTileModeSupported = lazy (DynamicDll.Bind "DTWAIN_IsBufferedTileModeSupported" : DTWAIN_IsBufferedTileModeSupportedDelegate)
    let private IsCapSupported = lazy (DynamicDll.Bind "DTWAIN_IsCapSupported" : DTWAIN_IsCapSupportedDelegate)
    let private IsCompressionSupported = lazy (DynamicDll.Bind "DTWAIN_IsCompressionSupported" : DTWAIN_IsCompressionSupportedDelegate)
    let private IsCustomDSDataSupported = lazy (DynamicDll.Bind "DTWAIN_IsCustomDSDataSupported" : DTWAIN_IsCustomDSDataSupportedDelegate)
    let private IsDIBBlank = lazy (DynamicDll.Bind "DTWAIN_IsDIBBlank" : DTWAIN_IsDIBBlankDelegate)
    let private IsDIBBlankString = lazy (DynamicDll.Bind "DTWAIN_IsDIBBlankString" : DTWAIN_IsDIBBlankStringDelegate)
    let private IsDIBBlankStringA = lazy (DynamicDll.Bind "DTWAIN_IsDIBBlankStringA" : DTWAIN_IsDIBBlankStringADelegate)
    let private IsDIBBlankStringW = lazy (DynamicDll.Bind "DTWAIN_IsDIBBlankStringW" : DTWAIN_IsDIBBlankStringWDelegate)
    let private IsDeviceEventSupported = lazy (DynamicDll.Bind "DTWAIN_IsDeviceEventSupported" : DTWAIN_IsDeviceEventSupportedDelegate)
    let private IsDeviceOnLine = lazy (DynamicDll.Bind "DTWAIN_IsDeviceOnLine" : DTWAIN_IsDeviceOnLineDelegate)
    let private IsDoubleFeedDetectLengthSupported = lazy (DynamicDll.Bind "DTWAIN_IsDoubleFeedDetectLengthSupported" : DTWAIN_IsDoubleFeedDetectLengthSupportedDelegate)
    let private IsDoubleFeedDetectSupported = lazy (DynamicDll.Bind "DTWAIN_IsDoubleFeedDetectSupported" : DTWAIN_IsDoubleFeedDetectSupportedDelegate)
    let private IsDoublePageCountOnDuplex = lazy (DynamicDll.Bind "DTWAIN_IsDoublePageCountOnDuplex" : DTWAIN_IsDoublePageCountOnDuplexDelegate)
    let private IsDuplexEnabled = lazy (DynamicDll.Bind "DTWAIN_IsDuplexEnabled" : DTWAIN_IsDuplexEnabledDelegate)
    let private IsDuplexSupported = lazy (DynamicDll.Bind "DTWAIN_IsDuplexSupported" : DTWAIN_IsDuplexSupportedDelegate)
    let private IsExtImageInfoSupported = lazy (DynamicDll.Bind "DTWAIN_IsExtImageInfoSupported" : DTWAIN_IsExtImageInfoSupportedDelegate)
    let private IsFeederEnabled = lazy (DynamicDll.Bind "DTWAIN_IsFeederEnabled" : DTWAIN_IsFeederEnabledDelegate)
    let private IsFeederLoaded = lazy (DynamicDll.Bind "DTWAIN_IsFeederLoaded" : DTWAIN_IsFeederLoadedDelegate)
    let private IsFeederSensitive = lazy (DynamicDll.Bind "DTWAIN_IsFeederSensitive" : DTWAIN_IsFeederSensitiveDelegate)
    let private IsFeederSupported = lazy (DynamicDll.Bind "DTWAIN_IsFeederSupported" : DTWAIN_IsFeederSupportedDelegate)
    let private IsFileSystemSupported = lazy (DynamicDll.Bind "DTWAIN_IsFileSystemSupported" : DTWAIN_IsFileSystemSupportedDelegate)
    let private IsFileXferSupported = lazy (DynamicDll.Bind "DTWAIN_IsFileXferSupported" : DTWAIN_IsFileXferSupportedDelegate)
    let private IsIAFieldALastPageSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldALastPageSupported" : DTWAIN_IsIAFieldALastPageSupportedDelegate)
    let private IsIAFieldALevelSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldALevelSupported" : DTWAIN_IsIAFieldALevelSupportedDelegate)
    let private IsIAFieldAPrintFormatSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldAPrintFormatSupported" : DTWAIN_IsIAFieldAPrintFormatSupportedDelegate)
    let private IsIAFieldAValueSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldAValueSupported" : DTWAIN_IsIAFieldAValueSupportedDelegate)
    let private IsIAFieldBLastPageSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldBLastPageSupported" : DTWAIN_IsIAFieldBLastPageSupportedDelegate)
    let private IsIAFieldBLevelSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldBLevelSupported" : DTWAIN_IsIAFieldBLevelSupportedDelegate)
    let private IsIAFieldBPrintFormatSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldBPrintFormatSupported" : DTWAIN_IsIAFieldBPrintFormatSupportedDelegate)
    let private IsIAFieldBValueSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldBValueSupported" : DTWAIN_IsIAFieldBValueSupportedDelegate)
    let private IsIAFieldCLastPageSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldCLastPageSupported" : DTWAIN_IsIAFieldCLastPageSupportedDelegate)
    let private IsIAFieldCLevelSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldCLevelSupported" : DTWAIN_IsIAFieldCLevelSupportedDelegate)
    let private IsIAFieldCPrintFormatSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldCPrintFormatSupported" : DTWAIN_IsIAFieldCPrintFormatSupportedDelegate)
    let private IsIAFieldCValueSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldCValueSupported" : DTWAIN_IsIAFieldCValueSupportedDelegate)
    let private IsIAFieldDLastPageSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldDLastPageSupported" : DTWAIN_IsIAFieldDLastPageSupportedDelegate)
    let private IsIAFieldDLevelSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldDLevelSupported" : DTWAIN_IsIAFieldDLevelSupportedDelegate)
    let private IsIAFieldDPrintFormatSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldDPrintFormatSupported" : DTWAIN_IsIAFieldDPrintFormatSupportedDelegate)
    let private IsIAFieldDValueSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldDValueSupported" : DTWAIN_IsIAFieldDValueSupportedDelegate)
    let private IsIAFieldELastPageSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldELastPageSupported" : DTWAIN_IsIAFieldELastPageSupportedDelegate)
    let private IsIAFieldELevelSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldELevelSupported" : DTWAIN_IsIAFieldELevelSupportedDelegate)
    let private IsIAFieldEPrintFormatSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldEPrintFormatSupported" : DTWAIN_IsIAFieldEPrintFormatSupportedDelegate)
    let private IsIAFieldEValueSupported = lazy (DynamicDll.Bind "DTWAIN_IsIAFieldEValueSupported" : DTWAIN_IsIAFieldEValueSupportedDelegate)
    let private IsImageAddressingSupported = lazy (DynamicDll.Bind "DTWAIN_IsImageAddressingSupported" : DTWAIN_IsImageAddressingSupportedDelegate)
    let private IsIndicatorEnabled = lazy (DynamicDll.Bind "DTWAIN_IsIndicatorEnabled" : DTWAIN_IsIndicatorEnabledDelegate)
    let private IsIndicatorSupported = lazy (DynamicDll.Bind "DTWAIN_IsIndicatorSupported" : DTWAIN_IsIndicatorSupportedDelegate)
    let private IsInitialized = lazy (DynamicDll.Bind "DTWAIN_IsInitialized" : DTWAIN_IsInitializedDelegate)
    let private IsJPEGSupported = lazy (DynamicDll.Bind "DTWAIN_IsJPEGSupported" : DTWAIN_IsJPEGSupportedDelegate)
    let private IsJobControlSupported = lazy (DynamicDll.Bind "DTWAIN_IsJobControlSupported" : DTWAIN_IsJobControlSupportedDelegate)
    let private IsLampEnabled = lazy (DynamicDll.Bind "DTWAIN_IsLampEnabled" : DTWAIN_IsLampEnabledDelegate)
    let private IsLampSupported = lazy (DynamicDll.Bind "DTWAIN_IsLampSupported" : DTWAIN_IsLampSupportedDelegate)
    let private IsLightPathSupported = lazy (DynamicDll.Bind "DTWAIN_IsLightPathSupported" : DTWAIN_IsLightPathSupportedDelegate)
    let private IsLightSourceSupported = lazy (DynamicDll.Bind "DTWAIN_IsLightSourceSupported" : DTWAIN_IsLightSourceSupportedDelegate)
    let private IsMaxBuffersSupported = lazy (DynamicDll.Bind "DTWAIN_IsMaxBuffersSupported" : DTWAIN_IsMaxBuffersSupportedDelegate)
    let private IsMemFileXferSupported = lazy (DynamicDll.Bind "DTWAIN_IsMemFileXferSupported" : DTWAIN_IsMemFileXferSupportedDelegate)
    let private IsMsgNotifyEnabled = lazy (DynamicDll.Bind "DTWAIN_IsMsgNotifyEnabled" : DTWAIN_IsMsgNotifyEnabledDelegate)
    let private IsNotifyTripletsEnabled = lazy (DynamicDll.Bind "DTWAIN_IsNotifyTripletsEnabled" : DTWAIN_IsNotifyTripletsEnabledDelegate)
    let private IsOCREngineActivated = lazy (DynamicDll.Bind "DTWAIN_IsOCREngineActivated" : DTWAIN_IsOCREngineActivatedDelegate)
    let private IsOpenSourcesOnSelect = lazy (DynamicDll.Bind "DTWAIN_IsOpenSourcesOnSelect" : DTWAIN_IsOpenSourcesOnSelectDelegate)
    let private IsOrientationSupported = lazy (DynamicDll.Bind "DTWAIN_IsOrientationSupported" : DTWAIN_IsOrientationSupportedDelegate)
    let private IsOverscanSupported = lazy (DynamicDll.Bind "DTWAIN_IsOverscanSupported" : DTWAIN_IsOverscanSupportedDelegate)
    let private IsPDFSupported = lazy (DynamicDll.Bind "DTWAIN_IsPDFSupported" : DTWAIN_IsPDFSupportedDelegate)
    let private IsPNGSupported = lazy (DynamicDll.Bind "DTWAIN_IsPNGSupported" : DTWAIN_IsPNGSupportedDelegate)
    let private IsPaperDetectable = lazy (DynamicDll.Bind "DTWAIN_IsPaperDetectable" : DTWAIN_IsPaperDetectableDelegate)
    let private IsPaperSizeSupported = lazy (DynamicDll.Bind "DTWAIN_IsPaperSizeSupported" : DTWAIN_IsPaperSizeSupportedDelegate)
    let private IsPatchCapsSupported = lazy (DynamicDll.Bind "DTWAIN_IsPatchCapsSupported" : DTWAIN_IsPatchCapsSupportedDelegate)
    let private IsPatchDetectEnabled = lazy (DynamicDll.Bind "DTWAIN_IsPatchDetectEnabled" : DTWAIN_IsPatchDetectEnabledDelegate)
    let private IsPatchSupported = lazy (DynamicDll.Bind "DTWAIN_IsPatchSupported" : DTWAIN_IsPatchSupportedDelegate)
    let private IsPeekMessageLoopEnabled = lazy (DynamicDll.Bind "DTWAIN_IsPeekMessageLoopEnabled" : DTWAIN_IsPeekMessageLoopEnabledDelegate)
    let private IsPixelTypeSupported = lazy (DynamicDll.Bind "DTWAIN_IsPixelTypeSupported" : DTWAIN_IsPixelTypeSupportedDelegate)
    let private IsPrinterEnabled = lazy (DynamicDll.Bind "DTWAIN_IsPrinterEnabled" : DTWAIN_IsPrinterEnabledDelegate)
    let private IsPrinterSupported = lazy (DynamicDll.Bind "DTWAIN_IsPrinterSupported" : DTWAIN_IsPrinterSupportedDelegate)
    let private IsRotationSupported = lazy (DynamicDll.Bind "DTWAIN_IsRotationSupported" : DTWAIN_IsRotationSupportedDelegate)
    let private IsSessionEnabled = lazy (DynamicDll.Bind "DTWAIN_IsSessionEnabled" : DTWAIN_IsSessionEnabledDelegate)
    let private IsSkipImageInfoError = lazy (DynamicDll.Bind "DTWAIN_IsSkipImageInfoError" : DTWAIN_IsSkipImageInfoErrorDelegate)
    let private IsSourceAcquiring = lazy (DynamicDll.Bind "DTWAIN_IsSourceAcquiring" : DTWAIN_IsSourceAcquiringDelegate)
    let private IsSourceAcquiringEx = lazy (DynamicDll.Bind "DTWAIN_IsSourceAcquiringEx" : DTWAIN_IsSourceAcquiringExDelegate)
    let private IsSourceInUIOnlyMode = lazy (DynamicDll.Bind "DTWAIN_IsSourceInUIOnlyMode" : DTWAIN_IsSourceInUIOnlyModeDelegate)
    let private IsSourceOpen = lazy (DynamicDll.Bind "DTWAIN_IsSourceOpen" : DTWAIN_IsSourceOpenDelegate)
    let private IsSourceSelected = lazy (DynamicDll.Bind "DTWAIN_IsSourceSelected" : DTWAIN_IsSourceSelectedDelegate)
    let private IsSourceValid = lazy (DynamicDll.Bind "DTWAIN_IsSourceValid" : DTWAIN_IsSourceValidDelegate)
    let private IsTIFFSupported = lazy (DynamicDll.Bind "DTWAIN_IsTIFFSupported" : DTWAIN_IsTIFFSupportedDelegate)
    let private IsThumbnailEnabled = lazy (DynamicDll.Bind "DTWAIN_IsThumbnailEnabled" : DTWAIN_IsThumbnailEnabledDelegate)
    let private IsThumbnailSupported = lazy (DynamicDll.Bind "DTWAIN_IsThumbnailSupported" : DTWAIN_IsThumbnailSupportedDelegate)
    let private IsTwainAvailable = lazy (DynamicDll.Bind "DTWAIN_IsTwainAvailable" : DTWAIN_IsTwainAvailableDelegate)
    let private IsTwainAvailableEx = lazy (DynamicDll.Bind "DTWAIN_IsTwainAvailableEx" : DTWAIN_IsTwainAvailableExDelegate)
    let private IsTwainAvailableExA = lazy (DynamicDll.Bind "DTWAIN_IsTwainAvailableExA" : DTWAIN_IsTwainAvailableExADelegate)
    let private IsTwainAvailableExW = lazy (DynamicDll.Bind "DTWAIN_IsTwainAvailableExW" : DTWAIN_IsTwainAvailableExWDelegate)
    let private IsTwainMsg = lazy (DynamicDll.Bind "DTWAIN_IsTwainMsg" : DTWAIN_IsTwainMsgDelegate)
    let private IsUIControllable = lazy (DynamicDll.Bind "DTWAIN_IsUIControllable" : DTWAIN_IsUIControllableDelegate)
    let private IsUIEnabled = lazy (DynamicDll.Bind "DTWAIN_IsUIEnabled" : DTWAIN_IsUIEnabledDelegate)
    let private IsUIOnlySupported = lazy (DynamicDll.Bind "DTWAIN_IsUIOnlySupported" : DTWAIN_IsUIOnlySupportedDelegate)
    let private LoadCustomStringResources = lazy (DynamicDll.Bind "DTWAIN_LoadCustomStringResources" : DTWAIN_LoadCustomStringResourcesDelegate)
    let private LoadCustomStringResourcesA = lazy (DynamicDll.Bind "DTWAIN_LoadCustomStringResourcesA" : DTWAIN_LoadCustomStringResourcesADelegate)
    let private LoadCustomStringResourcesEx = lazy (DynamicDll.Bind "DTWAIN_LoadCustomStringResourcesEx" : DTWAIN_LoadCustomStringResourcesExDelegate)
    let private LoadCustomStringResourcesExA = lazy (DynamicDll.Bind "DTWAIN_LoadCustomStringResourcesExA" : DTWAIN_LoadCustomStringResourcesExADelegate)
    let private LoadCustomStringResourcesExW = lazy (DynamicDll.Bind "DTWAIN_LoadCustomStringResourcesExW" : DTWAIN_LoadCustomStringResourcesExWDelegate)
    let private LoadCustomStringResourcesW = lazy (DynamicDll.Bind "DTWAIN_LoadCustomStringResourcesW" : DTWAIN_LoadCustomStringResourcesWDelegate)
    let private LoadLanguageResource = lazy (DynamicDll.Bind "DTWAIN_LoadLanguageResource" : DTWAIN_LoadLanguageResourceDelegate)
    let private LockMemory = lazy (DynamicDll.Bind "DTWAIN_LockMemory" : DTWAIN_LockMemoryDelegate)
    let private LockMemoryEx = lazy (DynamicDll.Bind "DTWAIN_LockMemoryEx" : DTWAIN_LockMemoryExDelegate)
    let private LogMessage = lazy (DynamicDll.Bind "DTWAIN_LogMessage" : DTWAIN_LogMessageDelegate)
    let private LogMessageA = lazy (DynamicDll.Bind "DTWAIN_LogMessageA" : DTWAIN_LogMessageADelegate)
    let private LogMessageW = lazy (DynamicDll.Bind "DTWAIN_LogMessageW" : DTWAIN_LogMessageWDelegate)
    let private MakeRGB = lazy (DynamicDll.Bind "DTWAIN_MakeRGB" : DTWAIN_MakeRGBDelegate)
    let private OpenSource = lazy (DynamicDll.Bind "DTWAIN_OpenSource" : DTWAIN_OpenSourceDelegate)
    let private OpenSourcesOnSelect = lazy (DynamicDll.Bind "DTWAIN_OpenSourcesOnSelect" : DTWAIN_OpenSourcesOnSelectDelegate)
    let private RangeCreate = lazy (DynamicDll.Bind "DTWAIN_RangeCreate" : DTWAIN_RangeCreateDelegate)
    let private RangeCreateFromCap = lazy (DynamicDll.Bind "DTWAIN_RangeCreateFromCap" : DTWAIN_RangeCreateFromCapDelegate)
    let private RangeDestroy = lazy (DynamicDll.Bind "DTWAIN_RangeDestroy" : DTWAIN_RangeDestroyDelegate)
    let private RangeExpand = lazy (DynamicDll.Bind "DTWAIN_RangeExpand" : DTWAIN_RangeExpandDelegate)
    let private RangeExpandEx = lazy (DynamicDll.Bind "DTWAIN_RangeExpandEx" : DTWAIN_RangeExpandExDelegate)
    let private RangeGetAll = lazy (DynamicDll.Bind "DTWAIN_RangeGetAll" : DTWAIN_RangeGetAllDelegate)
    let private RangeGetAllFloat = lazy (DynamicDll.Bind "DTWAIN_RangeGetAllFloat" : DTWAIN_RangeGetAllFloatDelegate)
    let private RangeGetAllFloatString = lazy (DynamicDll.Bind "DTWAIN_RangeGetAllFloatString" : DTWAIN_RangeGetAllFloatStringDelegate)
    let private RangeGetAllFloatStringA = lazy (DynamicDll.Bind "DTWAIN_RangeGetAllFloatStringA" : DTWAIN_RangeGetAllFloatStringADelegate)
    let private RangeGetAllFloatStringW = lazy (DynamicDll.Bind "DTWAIN_RangeGetAllFloatStringW" : DTWAIN_RangeGetAllFloatStringWDelegate)
    let private RangeGetAllLong = lazy (DynamicDll.Bind "DTWAIN_RangeGetAllLong" : DTWAIN_RangeGetAllLongDelegate)
    let private RangeGetCount = lazy (DynamicDll.Bind "DTWAIN_RangeGetCount" : DTWAIN_RangeGetCountDelegate)
    let private RangeGetExpValue = lazy (DynamicDll.Bind "DTWAIN_RangeGetExpValue" : DTWAIN_RangeGetExpValueDelegate)
    let private RangeGetExpValueFloat = lazy (DynamicDll.Bind "DTWAIN_RangeGetExpValueFloat" : DTWAIN_RangeGetExpValueFloatDelegate)
    let private RangeGetExpValueFloatString = lazy (DynamicDll.Bind "DTWAIN_RangeGetExpValueFloatString" : DTWAIN_RangeGetExpValueFloatStringDelegate)
    let private RangeGetExpValueFloatStringA = lazy (DynamicDll.Bind "DTWAIN_RangeGetExpValueFloatStringA" : DTWAIN_RangeGetExpValueFloatStringADelegate)
    let private RangeGetExpValueFloatStringW = lazy (DynamicDll.Bind "DTWAIN_RangeGetExpValueFloatStringW" : DTWAIN_RangeGetExpValueFloatStringWDelegate)
    let private RangeGetExpValueLong = lazy (DynamicDll.Bind "DTWAIN_RangeGetExpValueLong" : DTWAIN_RangeGetExpValueLongDelegate)
    let private RangeGetNearestValue = lazy (DynamicDll.Bind "DTWAIN_RangeGetNearestValue" : DTWAIN_RangeGetNearestValueDelegate)
    let private RangeGetPos = lazy (DynamicDll.Bind "DTWAIN_RangeGetPos" : DTWAIN_RangeGetPosDelegate)
    let private RangeGetPosFloat = lazy (DynamicDll.Bind "DTWAIN_RangeGetPosFloat" : DTWAIN_RangeGetPosFloatDelegate)
    let private RangeGetPosFloatString = lazy (DynamicDll.Bind "DTWAIN_RangeGetPosFloatString" : DTWAIN_RangeGetPosFloatStringDelegate)
    let private RangeGetPosFloatStringA = lazy (DynamicDll.Bind "DTWAIN_RangeGetPosFloatStringA" : DTWAIN_RangeGetPosFloatStringADelegate)
    let private RangeGetPosFloatStringW = lazy (DynamicDll.Bind "DTWAIN_RangeGetPosFloatStringW" : DTWAIN_RangeGetPosFloatStringWDelegate)
    let private RangeGetPosLong = lazy (DynamicDll.Bind "DTWAIN_RangeGetPosLong" : DTWAIN_RangeGetPosLongDelegate)
    let private RangeGetValue = lazy (DynamicDll.Bind "DTWAIN_RangeGetValue" : DTWAIN_RangeGetValueDelegate)
    let private RangeGetValueFloat = lazy (DynamicDll.Bind "DTWAIN_RangeGetValueFloat" : DTWAIN_RangeGetValueFloatDelegate)
    let private RangeGetValueFloatString = lazy (DynamicDll.Bind "DTWAIN_RangeGetValueFloatString" : DTWAIN_RangeGetValueFloatStringDelegate)
    let private RangeGetValueFloatStringA = lazy (DynamicDll.Bind "DTWAIN_RangeGetValueFloatStringA" : DTWAIN_RangeGetValueFloatStringADelegate)
    let private RangeGetValueFloatStringW = lazy (DynamicDll.Bind "DTWAIN_RangeGetValueFloatStringW" : DTWAIN_RangeGetValueFloatStringWDelegate)
    let private RangeGetValueLong = lazy (DynamicDll.Bind "DTWAIN_RangeGetValueLong" : DTWAIN_RangeGetValueLongDelegate)
    let private RangeIsValid = lazy (DynamicDll.Bind "DTWAIN_RangeIsValid" : DTWAIN_RangeIsValidDelegate)
    let private RangeNearestValueFloat = lazy (DynamicDll.Bind "DTWAIN_RangeNearestValueFloat" : DTWAIN_RangeNearestValueFloatDelegate)
    let private RangeNearestValueFloatString = lazy (DynamicDll.Bind "DTWAIN_RangeNearestValueFloatString" : DTWAIN_RangeNearestValueFloatStringDelegate)
    let private RangeNearestValueFloatStringA = lazy (DynamicDll.Bind "DTWAIN_RangeNearestValueFloatStringA" : DTWAIN_RangeNearestValueFloatStringADelegate)
    let private RangeNearestValueFloatStringW = lazy (DynamicDll.Bind "DTWAIN_RangeNearestValueFloatStringW" : DTWAIN_RangeNearestValueFloatStringWDelegate)
    let private RangeNearestValueLong = lazy (DynamicDll.Bind "DTWAIN_RangeNearestValueLong" : DTWAIN_RangeNearestValueLongDelegate)
    let private RangeSetAll = lazy (DynamicDll.Bind "DTWAIN_RangeSetAll" : DTWAIN_RangeSetAllDelegate)
    let private RangeSetAllFloat = lazy (DynamicDll.Bind "DTWAIN_RangeSetAllFloat" : DTWAIN_RangeSetAllFloatDelegate)
    let private RangeSetAllFloatString = lazy (DynamicDll.Bind "DTWAIN_RangeSetAllFloatString" : DTWAIN_RangeSetAllFloatStringDelegate)
    let private RangeSetAllFloatStringA = lazy (DynamicDll.Bind "DTWAIN_RangeSetAllFloatStringA" : DTWAIN_RangeSetAllFloatStringADelegate)
    let private RangeSetAllFloatStringW = lazy (DynamicDll.Bind "DTWAIN_RangeSetAllFloatStringW" : DTWAIN_RangeSetAllFloatStringWDelegate)
    let private RangeSetAllLong = lazy (DynamicDll.Bind "DTWAIN_RangeSetAllLong" : DTWAIN_RangeSetAllLongDelegate)
    let private RangeSetValue = lazy (DynamicDll.Bind "DTWAIN_RangeSetValue" : DTWAIN_RangeSetValueDelegate)
    let private RangeSetValueFloat = lazy (DynamicDll.Bind "DTWAIN_RangeSetValueFloat" : DTWAIN_RangeSetValueFloatDelegate)
    let private RangeSetValueFloatString = lazy (DynamicDll.Bind "DTWAIN_RangeSetValueFloatString" : DTWAIN_RangeSetValueFloatStringDelegate)
    let private RangeSetValueFloatStringA = lazy (DynamicDll.Bind "DTWAIN_RangeSetValueFloatStringA" : DTWAIN_RangeSetValueFloatStringADelegate)
    let private RangeSetValueFloatStringW = lazy (DynamicDll.Bind "DTWAIN_RangeSetValueFloatStringW" : DTWAIN_RangeSetValueFloatStringWDelegate)
    let private RangeSetValueLong = lazy (DynamicDll.Bind "DTWAIN_RangeSetValueLong" : DTWAIN_RangeSetValueLongDelegate)
    let private ResetPDFTextElement = lazy (DynamicDll.Bind "DTWAIN_ResetPDFTextElement" : DTWAIN_ResetPDFTextElementDelegate)
    let private RewindPage = lazy (DynamicDll.Bind "DTWAIN_RewindPage" : DTWAIN_RewindPageDelegate)
    let private SelectDefaultOCREngine = lazy (DynamicDll.Bind "DTWAIN_SelectDefaultOCREngine" : DTWAIN_SelectDefaultOCREngineDelegate)
    let private SelectDefaultSource = lazy (DynamicDll.Bind "DTWAIN_SelectDefaultSource" : DTWAIN_SelectDefaultSourceDelegate)
    let private SelectDefaultSourceWithOpen = lazy (DynamicDll.Bind "DTWAIN_SelectDefaultSourceWithOpen" : DTWAIN_SelectDefaultSourceWithOpenDelegate)
    let private SelectOCREngine = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngine" : DTWAIN_SelectOCREngineDelegate)
    let private SelectOCREngine2 = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngine2" : DTWAIN_SelectOCREngine2Delegate)
    let private SelectOCREngine2A = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngine2A" : DTWAIN_SelectOCREngine2ADelegate)
    let private SelectOCREngine2Ex = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngine2Ex" : DTWAIN_SelectOCREngine2ExDelegate)
    let private SelectOCREngine2ExA = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngine2ExA" : DTWAIN_SelectOCREngine2ExADelegate)
    let private SelectOCREngine2ExW = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngine2ExW" : DTWAIN_SelectOCREngine2ExWDelegate)
    let private SelectOCREngine2W = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngine2W" : DTWAIN_SelectOCREngine2WDelegate)
    let private SelectOCREngineByName = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngineByName" : DTWAIN_SelectOCREngineByNameDelegate)
    let private SelectOCREngineByNameA = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngineByNameA" : DTWAIN_SelectOCREngineByNameADelegate)
    let private SelectOCREngineByNameW = lazy (DynamicDll.Bind "DTWAIN_SelectOCREngineByNameW" : DTWAIN_SelectOCREngineByNameWDelegate)
    let private SelectSource = lazy (DynamicDll.Bind "DTWAIN_SelectSource" : DTWAIN_SelectSourceDelegate)
    let private SelectSource2 = lazy (DynamicDll.Bind "DTWAIN_SelectSource2" : DTWAIN_SelectSource2Delegate)
    let private SelectSource2A = lazy (DynamicDll.Bind "DTWAIN_SelectSource2A" : DTWAIN_SelectSource2ADelegate)
    let private SelectSource2Ex = lazy (DynamicDll.Bind "DTWAIN_SelectSource2Ex" : DTWAIN_SelectSource2ExDelegate)
    let private SelectSource2ExA = lazy (DynamicDll.Bind "DTWAIN_SelectSource2ExA" : DTWAIN_SelectSource2ExADelegate)
    let private SelectSource2ExW = lazy (DynamicDll.Bind "DTWAIN_SelectSource2ExW" : DTWAIN_SelectSource2ExWDelegate)
    let private SelectSource2W = lazy (DynamicDll.Bind "DTWAIN_SelectSource2W" : DTWAIN_SelectSource2WDelegate)
    let private SelectSourceByName = lazy (DynamicDll.Bind "DTWAIN_SelectSourceByName" : DTWAIN_SelectSourceByNameDelegate)
    let private SelectSourceByNameA = lazy (DynamicDll.Bind "DTWAIN_SelectSourceByNameA" : DTWAIN_SelectSourceByNameADelegate)
    let private SelectSourceByNameW = lazy (DynamicDll.Bind "DTWAIN_SelectSourceByNameW" : DTWAIN_SelectSourceByNameWDelegate)
    let private SelectSourceByNameWithOpen = lazy (DynamicDll.Bind "DTWAIN_SelectSourceByNameWithOpen" : DTWAIN_SelectSourceByNameWithOpenDelegate)
    let private SelectSourceByNameWithOpenA = lazy (DynamicDll.Bind "DTWAIN_SelectSourceByNameWithOpenA" : DTWAIN_SelectSourceByNameWithOpenADelegate)
    let private SelectSourceByNameWithOpenW = lazy (DynamicDll.Bind "DTWAIN_SelectSourceByNameWithOpenW" : DTWAIN_SelectSourceByNameWithOpenWDelegate)
    let private SelectSourceWithOpen = lazy (DynamicDll.Bind "DTWAIN_SelectSourceWithOpen" : DTWAIN_SelectSourceWithOpenDelegate)
    let private SetAcquireArea = lazy (DynamicDll.Bind "DTWAIN_SetAcquireArea" : DTWAIN_SetAcquireAreaDelegate)
    let private SetAcquireArea2 = lazy (DynamicDll.Bind "DTWAIN_SetAcquireArea2" : DTWAIN_SetAcquireArea2Delegate)
    let private SetAcquireArea2String = lazy (DynamicDll.Bind "DTWAIN_SetAcquireArea2String" : DTWAIN_SetAcquireArea2StringDelegate)
    let private SetAcquireArea2StringA = lazy (DynamicDll.Bind "DTWAIN_SetAcquireArea2StringA" : DTWAIN_SetAcquireArea2StringADelegate)
    let private SetAcquireArea2StringW = lazy (DynamicDll.Bind "DTWAIN_SetAcquireArea2StringW" : DTWAIN_SetAcquireArea2StringWDelegate)
    let private SetAcquireImageNegative = lazy (DynamicDll.Bind "DTWAIN_SetAcquireImageNegative" : DTWAIN_SetAcquireImageNegativeDelegate)
    let private SetAcquireImageScale = lazy (DynamicDll.Bind "DTWAIN_SetAcquireImageScale" : DTWAIN_SetAcquireImageScaleDelegate)
    let private SetAcquireImageScaleString = lazy (DynamicDll.Bind "DTWAIN_SetAcquireImageScaleString" : DTWAIN_SetAcquireImageScaleStringDelegate)
    let private SetAcquireImageScaleStringA = lazy (DynamicDll.Bind "DTWAIN_SetAcquireImageScaleStringA" : DTWAIN_SetAcquireImageScaleStringADelegate)
    let private SetAcquireImageScaleStringW = lazy (DynamicDll.Bind "DTWAIN_SetAcquireImageScaleStringW" : DTWAIN_SetAcquireImageScaleStringWDelegate)
    let private SetAcquireStripBuffer = lazy (DynamicDll.Bind "DTWAIN_SetAcquireStripBuffer" : DTWAIN_SetAcquireStripBufferDelegate)
    let private SetAcquireStripSize = lazy (DynamicDll.Bind "DTWAIN_SetAcquireStripSize" : DTWAIN_SetAcquireStripSizeDelegate)
    let private SetAlarmVolume = lazy (DynamicDll.Bind "DTWAIN_SetAlarmVolume" : DTWAIN_SetAlarmVolumeDelegate)
    let private SetAlarms = lazy (DynamicDll.Bind "DTWAIN_SetAlarms" : DTWAIN_SetAlarmsDelegate)
    let private SetAllCapsToDefault = lazy (DynamicDll.Bind "DTWAIN_SetAllCapsToDefault" : DTWAIN_SetAllCapsToDefaultDelegate)
    let private SetAppInfo = lazy (DynamicDll.Bind "DTWAIN_SetAppInfo" : DTWAIN_SetAppInfoDelegate)
    let private SetAppInfoA = lazy (DynamicDll.Bind "DTWAIN_SetAppInfoA" : DTWAIN_SetAppInfoADelegate)
    let private SetAppInfoW = lazy (DynamicDll.Bind "DTWAIN_SetAppInfoW" : DTWAIN_SetAppInfoWDelegate)
    let private SetAuthor = lazy (DynamicDll.Bind "DTWAIN_SetAuthor" : DTWAIN_SetAuthorDelegate)
    let private SetAuthorA = lazy (DynamicDll.Bind "DTWAIN_SetAuthorA" : DTWAIN_SetAuthorADelegate)
    let private SetAuthorW = lazy (DynamicDll.Bind "DTWAIN_SetAuthorW" : DTWAIN_SetAuthorWDelegate)
    let private SetAvailablePrinters = lazy (DynamicDll.Bind "DTWAIN_SetAvailablePrinters" : DTWAIN_SetAvailablePrintersDelegate)
    let private SetAvailablePrintersArray = lazy (DynamicDll.Bind "DTWAIN_SetAvailablePrintersArray" : DTWAIN_SetAvailablePrintersArrayDelegate)
    let private SetBitDepth = lazy (DynamicDll.Bind "DTWAIN_SetBitDepth" : DTWAIN_SetBitDepthDelegate)
    let private SetBlankPageDetection = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetection" : DTWAIN_SetBlankPageDetectionDelegate)
    let private SetBlankPageDetectionEx = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetectionEx" : DTWAIN_SetBlankPageDetectionExDelegate)
    let private SetBlankPageDetectionExString = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetectionExString" : DTWAIN_SetBlankPageDetectionExStringDelegate)
    let private SetBlankPageDetectionExStringA = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetectionExStringA" : DTWAIN_SetBlankPageDetectionExStringADelegate)
    let private SetBlankPageDetectionExStringW = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetectionExStringW" : DTWAIN_SetBlankPageDetectionExStringWDelegate)
    let private SetBlankPageDetectionString = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetectionString" : DTWAIN_SetBlankPageDetectionStringDelegate)
    let private SetBlankPageDetectionStringA = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetectionStringA" : DTWAIN_SetBlankPageDetectionStringADelegate)
    let private SetBlankPageDetectionStringW = lazy (DynamicDll.Bind "DTWAIN_SetBlankPageDetectionStringW" : DTWAIN_SetBlankPageDetectionStringWDelegate)
    let private SetBrightness = lazy (DynamicDll.Bind "DTWAIN_SetBrightness" : DTWAIN_SetBrightnessDelegate)
    let private SetBrightnessString = lazy (DynamicDll.Bind "DTWAIN_SetBrightnessString" : DTWAIN_SetBrightnessStringDelegate)
    let private SetBrightnessStringA = lazy (DynamicDll.Bind "DTWAIN_SetBrightnessStringA" : DTWAIN_SetBrightnessStringADelegate)
    let private SetBrightnessStringW = lazy (DynamicDll.Bind "DTWAIN_SetBrightnessStringW" : DTWAIN_SetBrightnessStringWDelegate)
    let private SetBufferedTileMode = lazy (DynamicDll.Bind "DTWAIN_SetBufferedTileMode" : DTWAIN_SetBufferedTileModeDelegate)
    let private SetCamera = lazy (DynamicDll.Bind "DTWAIN_SetCamera" : DTWAIN_SetCameraDelegate)
    let private SetCameraA = lazy (DynamicDll.Bind "DTWAIN_SetCameraA" : DTWAIN_SetCameraADelegate)
    let private SetCameraW = lazy (DynamicDll.Bind "DTWAIN_SetCameraW" : DTWAIN_SetCameraWDelegate)
    let private SetCapValues = lazy (DynamicDll.Bind "DTWAIN_SetCapValues" : DTWAIN_SetCapValuesDelegate)
    let private SetCapValuesEx = lazy (DynamicDll.Bind "DTWAIN_SetCapValuesEx" : DTWAIN_SetCapValuesExDelegate)
    let private SetCapValuesEx2 = lazy (DynamicDll.Bind "DTWAIN_SetCapValuesEx2" : DTWAIN_SetCapValuesEx2Delegate)
    let private SetCaption = lazy (DynamicDll.Bind "DTWAIN_SetCaption" : DTWAIN_SetCaptionDelegate)
    let private SetCaptionA = lazy (DynamicDll.Bind "DTWAIN_SetCaptionA" : DTWAIN_SetCaptionADelegate)
    let private SetCaptionW = lazy (DynamicDll.Bind "DTWAIN_SetCaptionW" : DTWAIN_SetCaptionWDelegate)
    let private SetCompressionType = lazy (DynamicDll.Bind "DTWAIN_SetCompressionType" : DTWAIN_SetCompressionTypeDelegate)
    let private SetContrast = lazy (DynamicDll.Bind "DTWAIN_SetContrast" : DTWAIN_SetContrastDelegate)
    let private SetContrastString = lazy (DynamicDll.Bind "DTWAIN_SetContrastString" : DTWAIN_SetContrastStringDelegate)
    let private SetContrastStringA = lazy (DynamicDll.Bind "DTWAIN_SetContrastStringA" : DTWAIN_SetContrastStringADelegate)
    let private SetContrastStringW = lazy (DynamicDll.Bind "DTWAIN_SetContrastStringW" : DTWAIN_SetContrastStringWDelegate)
    let private SetCountry = lazy (DynamicDll.Bind "DTWAIN_SetCountry" : DTWAIN_SetCountryDelegate)
    let private SetCurrentRetryCount = lazy (DynamicDll.Bind "DTWAIN_SetCurrentRetryCount" : DTWAIN_SetCurrentRetryCountDelegate)
    let private SetCustomDSData = lazy (DynamicDll.Bind "DTWAIN_SetCustomDSData" : DTWAIN_SetCustomDSDataDelegate)
    let private SetDSMSearchOrder = lazy (DynamicDll.Bind "DTWAIN_SetDSMSearchOrder" : DTWAIN_SetDSMSearchOrderDelegate)
    let private SetDSMSearchOrderEx = lazy (DynamicDll.Bind "DTWAIN_SetDSMSearchOrderEx" : DTWAIN_SetDSMSearchOrderExDelegate)
    let private SetDSMSearchOrderExA = lazy (DynamicDll.Bind "DTWAIN_SetDSMSearchOrderExA" : DTWAIN_SetDSMSearchOrderExADelegate)
    let private SetDSMSearchOrderExW = lazy (DynamicDll.Bind "DTWAIN_SetDSMSearchOrderExW" : DTWAIN_SetDSMSearchOrderExWDelegate)
    let private SetDefaultSource = lazy (DynamicDll.Bind "DTWAIN_SetDefaultSource" : DTWAIN_SetDefaultSourceDelegate)
    let private SetDeviceNotifications = lazy (DynamicDll.Bind "DTWAIN_SetDeviceNotifications" : DTWAIN_SetDeviceNotificationsDelegate)
    let private SetDeviceTimeDate = lazy (DynamicDll.Bind "DTWAIN_SetDeviceTimeDate" : DTWAIN_SetDeviceTimeDateDelegate)
    let private SetDeviceTimeDateA = lazy (DynamicDll.Bind "DTWAIN_SetDeviceTimeDateA" : DTWAIN_SetDeviceTimeDateADelegate)
    let private SetDeviceTimeDateW = lazy (DynamicDll.Bind "DTWAIN_SetDeviceTimeDateW" : DTWAIN_SetDeviceTimeDateWDelegate)
    let private SetDoubleFeedDetectLength = lazy (DynamicDll.Bind "DTWAIN_SetDoubleFeedDetectLength" : DTWAIN_SetDoubleFeedDetectLengthDelegate)
    let private SetDoubleFeedDetectLengthString = lazy (DynamicDll.Bind "DTWAIN_SetDoubleFeedDetectLengthString" : DTWAIN_SetDoubleFeedDetectLengthStringDelegate)
    let private SetDoubleFeedDetectLengthStringA = lazy (DynamicDll.Bind "DTWAIN_SetDoubleFeedDetectLengthStringA" : DTWAIN_SetDoubleFeedDetectLengthStringADelegate)
    let private SetDoubleFeedDetectLengthStringW = lazy (DynamicDll.Bind "DTWAIN_SetDoubleFeedDetectLengthStringW" : DTWAIN_SetDoubleFeedDetectLengthStringWDelegate)
    let private SetDoubleFeedDetectValues = lazy (DynamicDll.Bind "DTWAIN_SetDoubleFeedDetectValues" : DTWAIN_SetDoubleFeedDetectValuesDelegate)
    let private SetDoublePageCountOnDuplex = lazy (DynamicDll.Bind "DTWAIN_SetDoublePageCountOnDuplex" : DTWAIN_SetDoublePageCountOnDuplexDelegate)
    let private SetEOJDetectValue = lazy (DynamicDll.Bind "DTWAIN_SetEOJDetectValue" : DTWAIN_SetEOJDetectValueDelegate)
    let private SetErrorBufferThreshold = lazy (DynamicDll.Bind "DTWAIN_SetErrorBufferThreshold" : DTWAIN_SetErrorBufferThresholdDelegate)
    let private SetFeederAlignment = lazy (DynamicDll.Bind "DTWAIN_SetFeederAlignment" : DTWAIN_SetFeederAlignmentDelegate)
    let private SetFeederOrder = lazy (DynamicDll.Bind "DTWAIN_SetFeederOrder" : DTWAIN_SetFeederOrderDelegate)
    let private SetFeederWaitTime = lazy (DynamicDll.Bind "DTWAIN_SetFeederWaitTime" : DTWAIN_SetFeederWaitTimeDelegate)
    let private SetFileAutoIncrement = lazy (DynamicDll.Bind "DTWAIN_SetFileAutoIncrement" : DTWAIN_SetFileAutoIncrementDelegate)
    let private SetFileCompressionType = lazy (DynamicDll.Bind "DTWAIN_SetFileCompressionType" : DTWAIN_SetFileCompressionTypeDelegate)
    let private SetFileSavePos = lazy (DynamicDll.Bind "DTWAIN_SetFileSavePos" : DTWAIN_SetFileSavePosDelegate)
    let private SetFileSavePosA = lazy (DynamicDll.Bind "DTWAIN_SetFileSavePosA" : DTWAIN_SetFileSavePosADelegate)
    let private SetFileSavePosW = lazy (DynamicDll.Bind "DTWAIN_SetFileSavePosW" : DTWAIN_SetFileSavePosWDelegate)
    let private SetFileXferFormat = lazy (DynamicDll.Bind "DTWAIN_SetFileXferFormat" : DTWAIN_SetFileXferFormatDelegate)
    let private SetHalftone = lazy (DynamicDll.Bind "DTWAIN_SetHalftone" : DTWAIN_SetHalftoneDelegate)
    let private SetHalftoneA = lazy (DynamicDll.Bind "DTWAIN_SetHalftoneA" : DTWAIN_SetHalftoneADelegate)
    let private SetHalftoneW = lazy (DynamicDll.Bind "DTWAIN_SetHalftoneW" : DTWAIN_SetHalftoneWDelegate)
    let private SetHighlight = lazy (DynamicDll.Bind "DTWAIN_SetHighlight" : DTWAIN_SetHighlightDelegate)
    let private SetHighlightString = lazy (DynamicDll.Bind "DTWAIN_SetHighlightString" : DTWAIN_SetHighlightStringDelegate)
    let private SetHighlightStringA = lazy (DynamicDll.Bind "DTWAIN_SetHighlightStringA" : DTWAIN_SetHighlightStringADelegate)
    let private SetHighlightStringW = lazy (DynamicDll.Bind "DTWAIN_SetHighlightStringW" : DTWAIN_SetHighlightStringWDelegate)
    let private SetJobControl = lazy (DynamicDll.Bind "DTWAIN_SetJobControl" : DTWAIN_SetJobControlDelegate)
    let private SetJpegValues = lazy (DynamicDll.Bind "DTWAIN_SetJpegValues" : DTWAIN_SetJpegValuesDelegate)
    let private SetJpegXRValues = lazy (DynamicDll.Bind "DTWAIN_SetJpegXRValues" : DTWAIN_SetJpegXRValuesDelegate)
    let private SetLanguage = lazy (DynamicDll.Bind "DTWAIN_SetLanguage" : DTWAIN_SetLanguageDelegate)
    let private SetLastError = lazy (DynamicDll.Bind "DTWAIN_SetLastError" : DTWAIN_SetLastErrorDelegate)
    let private SetLightPath = lazy (DynamicDll.Bind "DTWAIN_SetLightPath" : DTWAIN_SetLightPathDelegate)
    let private SetLightPathEx = lazy (DynamicDll.Bind "DTWAIN_SetLightPathEx" : DTWAIN_SetLightPathExDelegate)
    let private SetLightSource = lazy (DynamicDll.Bind "DTWAIN_SetLightSource" : DTWAIN_SetLightSourceDelegate)
    let private SetLightSources = lazy (DynamicDll.Bind "DTWAIN_SetLightSources" : DTWAIN_SetLightSourcesDelegate)
    let private SetManualDuplexMode = lazy (DynamicDll.Bind "DTWAIN_SetManualDuplexMode" : DTWAIN_SetManualDuplexModeDelegate)
    let private SetMaxAcquisitions = lazy (DynamicDll.Bind "DTWAIN_SetMaxAcquisitions" : DTWAIN_SetMaxAcquisitionsDelegate)
    let private SetMaxBuffers = lazy (DynamicDll.Bind "DTWAIN_SetMaxBuffers" : DTWAIN_SetMaxBuffersDelegate)
    let private SetMaxRetryAttempts = lazy (DynamicDll.Bind "DTWAIN_SetMaxRetryAttempts" : DTWAIN_SetMaxRetryAttemptsDelegate)
    let private SetMultipageScanMode = lazy (DynamicDll.Bind "DTWAIN_SetMultipageScanMode" : DTWAIN_SetMultipageScanModeDelegate)
    let private SetNoiseFilter = lazy (DynamicDll.Bind "DTWAIN_SetNoiseFilter" : DTWAIN_SetNoiseFilterDelegate)
    let private SetOCRCapValues = lazy (DynamicDll.Bind "DTWAIN_SetOCRCapValues" : DTWAIN_SetOCRCapValuesDelegate)
    let private SetOrientation = lazy (DynamicDll.Bind "DTWAIN_SetOrientation" : DTWAIN_SetOrientationDelegate)
    let private SetOverscan = lazy (DynamicDll.Bind "DTWAIN_SetOverscan" : DTWAIN_SetOverscanDelegate)
    let private SetPDFAESEncryption = lazy (DynamicDll.Bind "DTWAIN_SetPDFAESEncryption" : DTWAIN_SetPDFAESEncryptionDelegate)
    let private SetPDFASCIICompression = lazy (DynamicDll.Bind "DTWAIN_SetPDFASCIICompression" : DTWAIN_SetPDFASCIICompressionDelegate)
    let private SetPDFAuthor = lazy (DynamicDll.Bind "DTWAIN_SetPDFAuthor" : DTWAIN_SetPDFAuthorDelegate)
    let private SetPDFAuthorA = lazy (DynamicDll.Bind "DTWAIN_SetPDFAuthorA" : DTWAIN_SetPDFAuthorADelegate)
    let private SetPDFAuthorW = lazy (DynamicDll.Bind "DTWAIN_SetPDFAuthorW" : DTWAIN_SetPDFAuthorWDelegate)
    let private SetPDFCompression = lazy (DynamicDll.Bind "DTWAIN_SetPDFCompression" : DTWAIN_SetPDFCompressionDelegate)
    let private SetPDFCreator = lazy (DynamicDll.Bind "DTWAIN_SetPDFCreator" : DTWAIN_SetPDFCreatorDelegate)
    let private SetPDFCreatorA = lazy (DynamicDll.Bind "DTWAIN_SetPDFCreatorA" : DTWAIN_SetPDFCreatorADelegate)
    let private SetPDFCreatorW = lazy (DynamicDll.Bind "DTWAIN_SetPDFCreatorW" : DTWAIN_SetPDFCreatorWDelegate)
    let private SetPDFEncryption = lazy (DynamicDll.Bind "DTWAIN_SetPDFEncryption" : DTWAIN_SetPDFEncryptionDelegate)
    let private SetPDFEncryptionA = lazy (DynamicDll.Bind "DTWAIN_SetPDFEncryptionA" : DTWAIN_SetPDFEncryptionADelegate)
    let private SetPDFEncryptionW = lazy (DynamicDll.Bind "DTWAIN_SetPDFEncryptionW" : DTWAIN_SetPDFEncryptionWDelegate)
    let private SetPDFJpegQuality = lazy (DynamicDll.Bind "DTWAIN_SetPDFJpegQuality" : DTWAIN_SetPDFJpegQualityDelegate)
    let private SetPDFKeywords = lazy (DynamicDll.Bind "DTWAIN_SetPDFKeywords" : DTWAIN_SetPDFKeywordsDelegate)
    let private SetPDFKeywordsA = lazy (DynamicDll.Bind "DTWAIN_SetPDFKeywordsA" : DTWAIN_SetPDFKeywordsADelegate)
    let private SetPDFKeywordsW = lazy (DynamicDll.Bind "DTWAIN_SetPDFKeywordsW" : DTWAIN_SetPDFKeywordsWDelegate)
    let private SetPDFOCRConversion = lazy (DynamicDll.Bind "DTWAIN_SetPDFOCRConversion" : DTWAIN_SetPDFOCRConversionDelegate)
    let private SetPDFOCRMode = lazy (DynamicDll.Bind "DTWAIN_SetPDFOCRMode" : DTWAIN_SetPDFOCRModeDelegate)
    let private SetPDFOrientation = lazy (DynamicDll.Bind "DTWAIN_SetPDFOrientation" : DTWAIN_SetPDFOrientationDelegate)
    let private SetPDFPageScale = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageScale" : DTWAIN_SetPDFPageScaleDelegate)
    let private SetPDFPageScaleString = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageScaleString" : DTWAIN_SetPDFPageScaleStringDelegate)
    let private SetPDFPageScaleStringA = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageScaleStringA" : DTWAIN_SetPDFPageScaleStringADelegate)
    let private SetPDFPageScaleStringW = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageScaleStringW" : DTWAIN_SetPDFPageScaleStringWDelegate)
    let private SetPDFPageSize = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageSize" : DTWAIN_SetPDFPageSizeDelegate)
    let private SetPDFPageSizeString = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageSizeString" : DTWAIN_SetPDFPageSizeStringDelegate)
    let private SetPDFPageSizeStringA = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageSizeStringA" : DTWAIN_SetPDFPageSizeStringADelegate)
    let private SetPDFPageSizeStringW = lazy (DynamicDll.Bind "DTWAIN_SetPDFPageSizeStringW" : DTWAIN_SetPDFPageSizeStringWDelegate)
    let private SetPDFPolarity = lazy (DynamicDll.Bind "DTWAIN_SetPDFPolarity" : DTWAIN_SetPDFPolarityDelegate)
    let private SetPDFProducer = lazy (DynamicDll.Bind "DTWAIN_SetPDFProducer" : DTWAIN_SetPDFProducerDelegate)
    let private SetPDFProducerA = lazy (DynamicDll.Bind "DTWAIN_SetPDFProducerA" : DTWAIN_SetPDFProducerADelegate)
    let private SetPDFProducerW = lazy (DynamicDll.Bind "DTWAIN_SetPDFProducerW" : DTWAIN_SetPDFProducerWDelegate)
    let private SetPDFSubject = lazy (DynamicDll.Bind "DTWAIN_SetPDFSubject" : DTWAIN_SetPDFSubjectDelegate)
    let private SetPDFSubjectA = lazy (DynamicDll.Bind "DTWAIN_SetPDFSubjectA" : DTWAIN_SetPDFSubjectADelegate)
    let private SetPDFSubjectW = lazy (DynamicDll.Bind "DTWAIN_SetPDFSubjectW" : DTWAIN_SetPDFSubjectWDelegate)
    let private SetPDFTextElementFloat = lazy (DynamicDll.Bind "DTWAIN_SetPDFTextElementFloat" : DTWAIN_SetPDFTextElementFloatDelegate)
    let private SetPDFTextElementLong = lazy (DynamicDll.Bind "DTWAIN_SetPDFTextElementLong" : DTWAIN_SetPDFTextElementLongDelegate)
    let private SetPDFTextElementString = lazy (DynamicDll.Bind "DTWAIN_SetPDFTextElementString" : DTWAIN_SetPDFTextElementStringDelegate)
    let private SetPDFTextElementStringA = lazy (DynamicDll.Bind "DTWAIN_SetPDFTextElementStringA" : DTWAIN_SetPDFTextElementStringADelegate)
    let private SetPDFTextElementStringW = lazy (DynamicDll.Bind "DTWAIN_SetPDFTextElementStringW" : DTWAIN_SetPDFTextElementStringWDelegate)
    let private SetPDFTitle = lazy (DynamicDll.Bind "DTWAIN_SetPDFTitle" : DTWAIN_SetPDFTitleDelegate)
    let private SetPDFTitleA = lazy (DynamicDll.Bind "DTWAIN_SetPDFTitleA" : DTWAIN_SetPDFTitleADelegate)
    let private SetPDFTitleW = lazy (DynamicDll.Bind "DTWAIN_SetPDFTitleW" : DTWAIN_SetPDFTitleWDelegate)
    let private SetPaperSize = lazy (DynamicDll.Bind "DTWAIN_SetPaperSize" : DTWAIN_SetPaperSizeDelegate)
    let private SetPatchMaxPriorities = lazy (DynamicDll.Bind "DTWAIN_SetPatchMaxPriorities" : DTWAIN_SetPatchMaxPrioritiesDelegate)
    let private SetPatchMaxRetries = lazy (DynamicDll.Bind "DTWAIN_SetPatchMaxRetries" : DTWAIN_SetPatchMaxRetriesDelegate)
    let private SetPatchPriorities = lazy (DynamicDll.Bind "DTWAIN_SetPatchPriorities" : DTWAIN_SetPatchPrioritiesDelegate)
    let private SetPatchSearchMode = lazy (DynamicDll.Bind "DTWAIN_SetPatchSearchMode" : DTWAIN_SetPatchSearchModeDelegate)
    let private SetPatchTimeOut = lazy (DynamicDll.Bind "DTWAIN_SetPatchTimeOut" : DTWAIN_SetPatchTimeOutDelegate)
    let private SetPixelFlavor = lazy (DynamicDll.Bind "DTWAIN_SetPixelFlavor" : DTWAIN_SetPixelFlavorDelegate)
    let private SetPixelType = lazy (DynamicDll.Bind "DTWAIN_SetPixelType" : DTWAIN_SetPixelTypeDelegate)
    let private SetPostScriptTitle = lazy (DynamicDll.Bind "DTWAIN_SetPostScriptTitle" : DTWAIN_SetPostScriptTitleDelegate)
    let private SetPostScriptTitleA = lazy (DynamicDll.Bind "DTWAIN_SetPostScriptTitleA" : DTWAIN_SetPostScriptTitleADelegate)
    let private SetPostScriptTitleW = lazy (DynamicDll.Bind "DTWAIN_SetPostScriptTitleW" : DTWAIN_SetPostScriptTitleWDelegate)
    let private SetPostScriptType = lazy (DynamicDll.Bind "DTWAIN_SetPostScriptType" : DTWAIN_SetPostScriptTypeDelegate)
    let private SetPrinter = lazy (DynamicDll.Bind "DTWAIN_SetPrinter" : DTWAIN_SetPrinterDelegate)
    let private SetPrinterEx = lazy (DynamicDll.Bind "DTWAIN_SetPrinterEx" : DTWAIN_SetPrinterExDelegate)
    let private SetPrinterStartNumber = lazy (DynamicDll.Bind "DTWAIN_SetPrinterStartNumber" : DTWAIN_SetPrinterStartNumberDelegate)
    let private SetPrinterStringMode = lazy (DynamicDll.Bind "DTWAIN_SetPrinterStringMode" : DTWAIN_SetPrinterStringModeDelegate)
    let private SetPrinterStrings = lazy (DynamicDll.Bind "DTWAIN_SetPrinterStrings" : DTWAIN_SetPrinterStringsDelegate)
    let private SetPrinterSuffixString = lazy (DynamicDll.Bind "DTWAIN_SetPrinterSuffixString" : DTWAIN_SetPrinterSuffixStringDelegate)
    let private SetPrinterSuffixStringA = lazy (DynamicDll.Bind "DTWAIN_SetPrinterSuffixStringA" : DTWAIN_SetPrinterSuffixStringADelegate)
    let private SetPrinterSuffixStringW = lazy (DynamicDll.Bind "DTWAIN_SetPrinterSuffixStringW" : DTWAIN_SetPrinterSuffixStringWDelegate)
    let private SetQueryCapSupport = lazy (DynamicDll.Bind "DTWAIN_SetQueryCapSupport" : DTWAIN_SetQueryCapSupportDelegate)
    let private SetResolution = lazy (DynamicDll.Bind "DTWAIN_SetResolution" : DTWAIN_SetResolutionDelegate)
    let private SetResolutionString = lazy (DynamicDll.Bind "DTWAIN_SetResolutionString" : DTWAIN_SetResolutionStringDelegate)
    let private SetResolutionStringA = lazy (DynamicDll.Bind "DTWAIN_SetResolutionStringA" : DTWAIN_SetResolutionStringADelegate)
    let private SetResolutionStringW = lazy (DynamicDll.Bind "DTWAIN_SetResolutionStringW" : DTWAIN_SetResolutionStringWDelegate)
    let private SetResourcePath = lazy (DynamicDll.Bind "DTWAIN_SetResourcePath" : DTWAIN_SetResourcePathDelegate)
    let private SetResourcePathA = lazy (DynamicDll.Bind "DTWAIN_SetResourcePathA" : DTWAIN_SetResourcePathADelegate)
    let private SetResourcePathW = lazy (DynamicDll.Bind "DTWAIN_SetResourcePathW" : DTWAIN_SetResourcePathWDelegate)
    let private SetRotation = lazy (DynamicDll.Bind "DTWAIN_SetRotation" : DTWAIN_SetRotationDelegate)
    let private SetRotationString = lazy (DynamicDll.Bind "DTWAIN_SetRotationString" : DTWAIN_SetRotationStringDelegate)
    let private SetRotationStringA = lazy (DynamicDll.Bind "DTWAIN_SetRotationStringA" : DTWAIN_SetRotationStringADelegate)
    let private SetRotationStringW = lazy (DynamicDll.Bind "DTWAIN_SetRotationStringW" : DTWAIN_SetRotationStringWDelegate)
    let private SetSaveFileName = lazy (DynamicDll.Bind "DTWAIN_SetSaveFileName" : DTWAIN_SetSaveFileNameDelegate)
    let private SetSaveFileNameA = lazy (DynamicDll.Bind "DTWAIN_SetSaveFileNameA" : DTWAIN_SetSaveFileNameADelegate)
    let private SetSaveFileNameW = lazy (DynamicDll.Bind "DTWAIN_SetSaveFileNameW" : DTWAIN_SetSaveFileNameWDelegate)
    let private SetShadow = lazy (DynamicDll.Bind "DTWAIN_SetShadow" : DTWAIN_SetShadowDelegate)
    let private SetShadowString = lazy (DynamicDll.Bind "DTWAIN_SetShadowString" : DTWAIN_SetShadowStringDelegate)
    let private SetShadowStringA = lazy (DynamicDll.Bind "DTWAIN_SetShadowStringA" : DTWAIN_SetShadowStringADelegate)
    let private SetShadowStringW = lazy (DynamicDll.Bind "DTWAIN_SetShadowStringW" : DTWAIN_SetShadowStringWDelegate)
    let private SetSourceUnit = lazy (DynamicDll.Bind "DTWAIN_SetSourceUnit" : DTWAIN_SetSourceUnitDelegate)
    let private SetTIFFCompressType = lazy (DynamicDll.Bind "DTWAIN_SetTIFFCompressType" : DTWAIN_SetTIFFCompressTypeDelegate)
    let private SetTIFFInvert = lazy (DynamicDll.Bind "DTWAIN_SetTIFFInvert" : DTWAIN_SetTIFFInvertDelegate)
    let private SetTempFileDirectory = lazy (DynamicDll.Bind "DTWAIN_SetTempFileDirectory" : DTWAIN_SetTempFileDirectoryDelegate)
    let private SetTempFileDirectoryA = lazy (DynamicDll.Bind "DTWAIN_SetTempFileDirectoryA" : DTWAIN_SetTempFileDirectoryADelegate)
    let private SetTempFileDirectoryEx = lazy (DynamicDll.Bind "DTWAIN_SetTempFileDirectoryEx" : DTWAIN_SetTempFileDirectoryExDelegate)
    let private SetTempFileDirectoryExA = lazy (DynamicDll.Bind "DTWAIN_SetTempFileDirectoryExA" : DTWAIN_SetTempFileDirectoryExADelegate)
    let private SetTempFileDirectoryExW = lazy (DynamicDll.Bind "DTWAIN_SetTempFileDirectoryExW" : DTWAIN_SetTempFileDirectoryExWDelegate)
    let private SetTempFileDirectoryW = lazy (DynamicDll.Bind "DTWAIN_SetTempFileDirectoryW" : DTWAIN_SetTempFileDirectoryWDelegate)
    let private SetThreshold = lazy (DynamicDll.Bind "DTWAIN_SetThreshold" : DTWAIN_SetThresholdDelegate)
    let private SetThresholdString = lazy (DynamicDll.Bind "DTWAIN_SetThresholdString" : DTWAIN_SetThresholdStringDelegate)
    let private SetThresholdStringA = lazy (DynamicDll.Bind "DTWAIN_SetThresholdStringA" : DTWAIN_SetThresholdStringADelegate)
    let private SetThresholdStringW = lazy (DynamicDll.Bind "DTWAIN_SetThresholdStringW" : DTWAIN_SetThresholdStringWDelegate)
    let private SetTwainDSM = lazy (DynamicDll.Bind "DTWAIN_SetTwainDSM" : DTWAIN_SetTwainDSMDelegate)
    let private SetTwainLog = lazy (DynamicDll.Bind "DTWAIN_SetTwainLog" : DTWAIN_SetTwainLogDelegate)
    let private SetTwainLogA = lazy (DynamicDll.Bind "DTWAIN_SetTwainLogA" : DTWAIN_SetTwainLogADelegate)
    let private SetTwainLogW = lazy (DynamicDll.Bind "DTWAIN_SetTwainLogW" : DTWAIN_SetTwainLogWDelegate)
    let private SetTwainMode = lazy (DynamicDll.Bind "DTWAIN_SetTwainMode" : DTWAIN_SetTwainModeDelegate)
    let private SetTwainTimeout = lazy (DynamicDll.Bind "DTWAIN_SetTwainTimeout" : DTWAIN_SetTwainTimeoutDelegate)
    let private SetXResolution = lazy (DynamicDll.Bind "DTWAIN_SetXResolution" : DTWAIN_SetXResolutionDelegate)
    let private SetXResolutionString = lazy (DynamicDll.Bind "DTWAIN_SetXResolutionString" : DTWAIN_SetXResolutionStringDelegate)
    let private SetXResolutionStringA = lazy (DynamicDll.Bind "DTWAIN_SetXResolutionStringA" : DTWAIN_SetXResolutionStringADelegate)
    let private SetXResolutionStringW = lazy (DynamicDll.Bind "DTWAIN_SetXResolutionStringW" : DTWAIN_SetXResolutionStringWDelegate)
    let private SetYResolution = lazy (DynamicDll.Bind "DTWAIN_SetYResolution" : DTWAIN_SetYResolutionDelegate)
    let private SetYResolutionString = lazy (DynamicDll.Bind "DTWAIN_SetYResolutionString" : DTWAIN_SetYResolutionStringDelegate)
    let private SetYResolutionStringA = lazy (DynamicDll.Bind "DTWAIN_SetYResolutionStringA" : DTWAIN_SetYResolutionStringADelegate)
    let private SetYResolutionStringW = lazy (DynamicDll.Bind "DTWAIN_SetYResolutionStringW" : DTWAIN_SetYResolutionStringWDelegate)
    let private ShowUIOnly = lazy (DynamicDll.Bind "DTWAIN_ShowUIOnly" : DTWAIN_ShowUIOnlyDelegate)
    let private ShutdownOCREngine = lazy (DynamicDll.Bind "DTWAIN_ShutdownOCREngine" : DTWAIN_ShutdownOCREngineDelegate)
    let private SkipImageInfoError = lazy (DynamicDll.Bind "DTWAIN_SkipImageInfoError" : DTWAIN_SkipImageInfoErrorDelegate)
    let private StartThread = lazy (DynamicDll.Bind "DTWAIN_StartThread" : DTWAIN_StartThreadDelegate)
    let private StartTwainSession = lazy (DynamicDll.Bind "DTWAIN_StartTwainSession" : DTWAIN_StartTwainSessionDelegate)
    let private StartTwainSessionA = lazy (DynamicDll.Bind "DTWAIN_StartTwainSessionA" : DTWAIN_StartTwainSessionADelegate)
    let private StartTwainSessionW = lazy (DynamicDll.Bind "DTWAIN_StartTwainSessionW" : DTWAIN_StartTwainSessionWDelegate)
    let private SysDestroy = lazy (DynamicDll.Bind "DTWAIN_SysDestroy" : DTWAIN_SysDestroyDelegate)
    let private SysInitialize = lazy (DynamicDll.Bind "DTWAIN_SysInitialize" : DTWAIN_SysInitializeDelegate)
    let private SysInitializeEx = lazy (DynamicDll.Bind "DTWAIN_SysInitializeEx" : DTWAIN_SysInitializeExDelegate)
    let private SysInitializeEx2 = lazy (DynamicDll.Bind "DTWAIN_SysInitializeEx2" : DTWAIN_SysInitializeEx2Delegate)
    let private SysInitializeEx2A = lazy (DynamicDll.Bind "DTWAIN_SysInitializeEx2A" : DTWAIN_SysInitializeEx2ADelegate)
    let private SysInitializeEx2W = lazy (DynamicDll.Bind "DTWAIN_SysInitializeEx2W" : DTWAIN_SysInitializeEx2WDelegate)
    let private SysInitializeExA = lazy (DynamicDll.Bind "DTWAIN_SysInitializeExA" : DTWAIN_SysInitializeExADelegate)
    let private SysInitializeExW = lazy (DynamicDll.Bind "DTWAIN_SysInitializeExW" : DTWAIN_SysInitializeExWDelegate)
    let private SysInitializeLib = lazy (DynamicDll.Bind "DTWAIN_SysInitializeLib" : DTWAIN_SysInitializeLibDelegate)
    let private SysInitializeLibEx = lazy (DynamicDll.Bind "DTWAIN_SysInitializeLibEx" : DTWAIN_SysInitializeLibExDelegate)
    let private SysInitializeLibEx2 = lazy (DynamicDll.Bind "DTWAIN_SysInitializeLibEx2" : DTWAIN_SysInitializeLibEx2Delegate)
    let private SysInitializeLibEx2A = lazy (DynamicDll.Bind "DTWAIN_SysInitializeLibEx2A" : DTWAIN_SysInitializeLibEx2ADelegate)
    let private SysInitializeLibEx2W = lazy (DynamicDll.Bind "DTWAIN_SysInitializeLibEx2W" : DTWAIN_SysInitializeLibEx2WDelegate)
    let private SysInitializeLibExA = lazy (DynamicDll.Bind "DTWAIN_SysInitializeLibExA" : DTWAIN_SysInitializeLibExADelegate)
    let private SysInitializeLibExW = lazy (DynamicDll.Bind "DTWAIN_SysInitializeLibExW" : DTWAIN_SysInitializeLibExWDelegate)
    let private SysInitializeNoBlocking = lazy (DynamicDll.Bind "DTWAIN_SysInitializeNoBlocking" : DTWAIN_SysInitializeNoBlockingDelegate)
    let private TestGetCap = lazy (DynamicDll.Bind "DTWAIN_TestGetCap" : DTWAIN_TestGetCapDelegate)
    let private UnlockMemory = lazy (DynamicDll.Bind "DTWAIN_UnlockMemory" : DTWAIN_UnlockMemoryDelegate)
    let private UnlockMemoryEx = lazy (DynamicDll.Bind "DTWAIN_UnlockMemoryEx" : DTWAIN_UnlockMemoryExDelegate)
    let private UseMultipleThreads = lazy (DynamicDll.Bind "DTWAIN_UseMultipleThreads" : DTWAIN_UseMultipleThreadsDelegate)
    /// Loads the DTWAIN DLL and performs post-load business logic
    let Load (dllPath: string) =
        if IsLoaded then
            failwith "TwainAPI already initialized"
        DynamicDll.LoadModule dllPath
        try
            IsLoaded <- true
        with
        | ex ->
            // Clean up on failure
            DynamicDll.UnloadModule()
            IsLoaded <- false
            reraise()

    /// Unloads the DLL and resets state
    let Unload() =
        if IsLoaded then
            DynamicDll.UnloadModule()
            IsLoaded <- false

    let DTWAIN_AcquireAudioFile (source: DTWAIN_SOURCE) (lpszfile: string) (lfileflags: LONG) (lmaxclips: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireAudioFile.Value.Invoke(source, lpszfile, lfileflags, lmaxclips, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireAudioFileA (source: DTWAIN_SOURCE) (lpszfile: string) (lfileflags: LONG) (lnumclips: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireAudioFileA.Value.Invoke(source, lpszfile, lfileflags, lnumclips, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireAudioFileW (source: DTWAIN_SOURCE) (lpszfile: string) (lfileflags: LONG) (lnumclips: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireAudioFileW.Value.Invoke(source, lpszfile, lfileflags, lnumclips, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireAudioNative (source: DTWAIN_SOURCE) (nmaxaudioclips: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireAudioNative.Value.Invoke(source, nmaxaudioclips, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireAudioNativeEx (source: DTWAIN_SOURCE) (nmaxaudioclips: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (acquisitions: DTWAIN_ARRAY) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireAudioNativeEx.Value.Invoke(source, nmaxaudioclips, bshowui, bclosesource, acquisitions, &pstatus)

    let DTWAIN_AcquireBuffered (source: DTWAIN_SOURCE) (pixeltype: LONG) (nmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireBuffered.Value.Invoke(source, pixeltype, nmaxpages, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireBufferedEx (source: DTWAIN_SOURCE) (pixeltype: LONG) (nmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (acquisitions: DTWAIN_ARRAY) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireBufferedEx.Value.Invoke(source, pixeltype, nmaxpages, bshowui, bclosesource, acquisitions, &pstatus)

    let DTWAIN_AcquireFile (source: DTWAIN_SOURCE) (lpszfile: string) (lfiletype: LONG) (lfileflags: LONG) (pixeltype: LONG) (lmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireFile.Value.Invoke(source, lpszfile, lfiletype, lfileflags, pixeltype, lmaxpages, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireFileA (source: DTWAIN_SOURCE) (lpszfile: string) (lfiletype: LONG) (lfileflags: LONG) (pixeltype: LONG) (lmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireFileA.Value.Invoke(source, lpszfile, lfiletype, lfileflags, pixeltype, lmaxpages, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireFileEx (source: DTWAIN_SOURCE) (afilenames: DTWAIN_ARRAY) (lfiletype: LONG) (lfileflags: LONG) (pixeltype: LONG) (lmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireFileEx.Value.Invoke(source, afilenames, lfiletype, lfileflags, pixeltype, lmaxpages, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireFileW (source: DTWAIN_SOURCE) (lpszfile: string) (lfiletype: LONG) (lfileflags: LONG) (pixeltype: LONG) (lmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireFileW.Value.Invoke(source, lpszfile, lfiletype, lfileflags, pixeltype, lmaxpages, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireNative (source: DTWAIN_SOURCE) (pixeltype: LONG) (nmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireNative.Value.Invoke(source, pixeltype, nmaxpages, bshowui, bclosesource, &pstatus)

    let DTWAIN_AcquireNativeEx (source: DTWAIN_SOURCE) (pixeltype: LONG) (nmaxpages: LONG) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (acquisitions: DTWAIN_ARRAY) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireNativeEx.Value.Invoke(source, pixeltype, nmaxpages, bshowui, bclosesource, acquisitions, &pstatus)

    let DTWAIN_AcquireToClipboard (source: DTWAIN_SOURCE) (pixeltype: LONG) (nmaxpages: LONG) (ntransfermode: LONG) (bdiscarddibs: DTWAIN_BOOL) (bshowui: DTWAIN_BOOL) (bclosesource: DTWAIN_BOOL) (pstatus: int byref) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AcquireToClipboard.Value.Invoke(source, pixeltype, nmaxpages, ntransfermode, bdiscarddibs, bshowui, bclosesource, &pstatus)

    let DTWAIN_AddExtImageInfoQuery (source: DTWAIN_SOURCE) (extimageinfo: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddExtImageInfoQuery.Value.Invoke(source, extimageinfo)

    let DTWAIN_AddFileToAppend (szfile: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddFileToAppend.Value.Invoke(szfile)

    let DTWAIN_AddFileToAppendA (szfile: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddFileToAppendA.Value.Invoke(szfile)

    let DTWAIN_AddFileToAppendW (szfile: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddFileToAppendW.Value.Invoke(szfile)

    let DTWAIN_AddPDFText (source: DTWAIN_SOURCE) (sztext: string) (xpos: LONG) (ypos: LONG) (fontname: string) (fontsize: DTWAIN_FLOAT) (colorrgb: LONG) (rendermode: LONG) (scaling: DTWAIN_FLOAT) (charspacing: DTWAIN_FLOAT) (wordspacing: DTWAIN_FLOAT) (strokewidth: LONG) (flags: DWORD) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddPDFText.Value.Invoke(source, sztext, xpos, ypos, fontname, fontsize, colorrgb, rendermode, scaling, charspacing, wordspacing, strokewidth, flags)

    let DTWAIN_AddPDFTextA (source: DTWAIN_SOURCE) (sztext: string) (xpos: LONG) (ypos: LONG) (fontname: string) (fontsize: DTWAIN_FLOAT) (colorrgb: LONG) (rendermode: LONG) (scaling: DTWAIN_FLOAT) (charspacing: DTWAIN_FLOAT) (wordspacing: DTWAIN_FLOAT) (strokewidth: LONG) (flags: DWORD) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddPDFTextA.Value.Invoke(source, sztext, xpos, ypos, fontname, fontsize, colorrgb, rendermode, scaling, charspacing, wordspacing, strokewidth, flags)

    let DTWAIN_AddPDFTextEx (source: DTWAIN_SOURCE) (textelement: DTWAIN_PDFTEXTELEMENT) (flags: DWORD) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddPDFTextEx.Value.Invoke(source, textelement, flags)

    let DTWAIN_AddPDFTextW (source: DTWAIN_SOURCE) (sztext: string) (xpos: LONG) (ypos: LONG) (fontname: string) (fontsize: DTWAIN_FLOAT) (colorrgb: LONG) (rendermode: LONG) (scaling: DTWAIN_FLOAT) (charspacing: DTWAIN_FLOAT) (wordspacing: DTWAIN_FLOAT) (strokewidth: LONG) (flags: DWORD) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AddPDFTextW.Value.Invoke(source, sztext, xpos, ypos, fontname, fontsize, colorrgb, rendermode, scaling, charspacing, wordspacing, strokewidth, flags)

    let DTWAIN_AllocateMemory (memsize: DWORD) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AllocateMemory.Value.Invoke(memsize)

    let DTWAIN_AllocateMemory64 (memsize: ULONG64) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AllocateMemory64.Value.Invoke(memsize)

    let DTWAIN_AllocateMemoryEx (memsize: DWORD) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AllocateMemoryEx.Value.Invoke(memsize)

    let DTWAIN_AppHandlesExceptions (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        AppHandlesExceptions.Value.Invoke(bset)

    let DTWAIN_ArrayANSIStringToFloat (stringarray: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayANSIStringToFloat.Value.Invoke(stringarray)

    let DTWAIN_ArrayAdd (parray: DTWAIN_ARRAY) (pvariant: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAdd.Value.Invoke(parray, pvariant)

    let DTWAIN_ArrayAddANSIString (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddANSIString.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddANSIStringN (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddANSIStringN.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddFloat (parray: DTWAIN_ARRAY) (val1: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloat.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddFloatN (parray: DTWAIN_ARRAY) (val1: DTWAIN_FLOAT) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloatN.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddFloatString (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloatString.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddFloatStringA (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloatStringA.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddFloatStringN (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloatStringN.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddFloatStringNA (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloatStringNA.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddFloatStringNW (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloatStringNW.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddFloatStringW (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFloatStringW.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddFrame (parray: DTWAIN_ARRAY) (frame: DTWAIN_FRAME) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFrame.Value.Invoke(parray, frame)

    let DTWAIN_ArrayAddFrameN (parray: DTWAIN_ARRAY) (frame: DTWAIN_FRAME) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddFrameN.Value.Invoke(parray, frame, num)

    let DTWAIN_ArrayAddLong (parray: DTWAIN_ARRAY) (val1: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddLong.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddLong64 (parray: DTWAIN_ARRAY) (val1: LONG64) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddLong64.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddLong64N (parray: DTWAIN_ARRAY) (val1: LONG64) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddLong64N.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddLongN (parray: DTWAIN_ARRAY) (val1: LONG) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddLongN.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddN (parray: DTWAIN_ARRAY) (pvariant: LPVOID) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddN.Value.Invoke(parray, pvariant, num)

    let DTWAIN_ArrayAddString (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddString.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddStringA (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddStringA.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddStringN (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddStringN.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddStringNA (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddStringNA.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddStringNW (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddStringNW.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayAddStringW (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddStringW.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddWideString (parray: DTWAIN_ARRAY) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddWideString.Value.Invoke(parray, val1)

    let DTWAIN_ArrayAddWideStringN (parray: DTWAIN_ARRAY) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayAddWideStringN.Value.Invoke(parray, val1, num)

    let DTWAIN_ArrayConvertFix32ToFloat (fix32array: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayConvertFix32ToFloat.Value.Invoke(fix32array)

    let DTWAIN_ArrayConvertFloatToFix32 (floatarray: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayConvertFloatToFix32.Value.Invoke(floatarray)

    let DTWAIN_ArrayCopy (source: DTWAIN_ARRAY) (dest: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayCopy.Value.Invoke(source, dest)

    let DTWAIN_ArrayCreate (nenumtype: LONG) (ninitialsize: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayCreate.Value.Invoke(nenumtype, ninitialsize)

    let DTWAIN_ArrayCreateCopy (source: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayCreateCopy.Value.Invoke(source)

    let DTWAIN_ArrayCreateFromCap (source: DTWAIN_SOURCE) (lcaptype: LONG) (lsize: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayCreateFromCap.Value.Invoke(source, lcaptype, lsize)

    let DTWAIN_ArrayCreateFromLong64s (pcarray: Int64 byref) (nsize: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayCreateFromLong64s.Value.Invoke(&pcarray, nsize)

    let DTWAIN_ArrayCreateFromLongs (pcarray: int byref) (nsize: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayCreateFromLongs.Value.Invoke(&pcarray, nsize)

    let DTWAIN_ArrayCreateFromReals (pcarray: DTWAIN_FLOAT byref) (nsize: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayCreateFromReals.Value.Invoke(&pcarray, nsize)

    let DTWAIN_ArrayDestroy (parray: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayDestroy.Value.Invoke(parray)

    let DTWAIN_ArrayDestroyFrames (framearray: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayDestroyFrames.Value.Invoke(framearray)

    let DTWAIN_ArrayFind (parray: DTWAIN_ARRAY) (pvariant: LPVOID) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFind.Value.Invoke(parray, pvariant)

    let DTWAIN_ArrayFindANSIString (parray: DTWAIN_ARRAY) (pstring: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindANSIString.Value.Invoke(parray, pstring)

    let DTWAIN_ArrayFindFloat (parray: DTWAIN_ARRAY) (val1: DTWAIN_FLOAT) (tolerance: DTWAIN_FLOAT) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindFloat.Value.Invoke(parray, val1, tolerance)

    let DTWAIN_ArrayFindFloatString (parray: DTWAIN_ARRAY) (val1: string) (tolerance: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindFloatString.Value.Invoke(parray, val1, tolerance)

    let DTWAIN_ArrayFindFloatStringA (parray: DTWAIN_ARRAY) (val1: string) (tolerance: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindFloatStringA.Value.Invoke(parray, val1, tolerance)

    let DTWAIN_ArrayFindFloatStringW (parray: DTWAIN_ARRAY) (val1: string) (tolerance: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindFloatStringW.Value.Invoke(parray, val1, tolerance)

    let DTWAIN_ArrayFindLong (parray: DTWAIN_ARRAY) (val1: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindLong.Value.Invoke(parray, val1)

    let DTWAIN_ArrayFindLong64 (parray: DTWAIN_ARRAY) (val1: LONG64) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindLong64.Value.Invoke(parray, val1)

    let DTWAIN_ArrayFindString (parray: DTWAIN_ARRAY) (pstring: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindString.Value.Invoke(parray, pstring)

    let DTWAIN_ArrayFindStringA (parray: DTWAIN_ARRAY) (pstring: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindStringA.Value.Invoke(parray, pstring)

    let DTWAIN_ArrayFindStringW (parray: DTWAIN_ARRAY) (pstring: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindStringW.Value.Invoke(parray, pstring)

    let DTWAIN_ArrayFindWideString (parray: DTWAIN_ARRAY) (pstring: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFindWideString.Value.Invoke(parray, pstring)

    let DTWAIN_ArrayFix32GetAt (afix32: DTWAIN_ARRAY) (lpos: int) (whole: int byref) (frac: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFix32GetAt.Value.Invoke(afix32, lpos, &whole, &frac)

    let DTWAIN_ArrayFix32SetAt (afix32: DTWAIN_ARRAY) (lpos: int) (whole: int) (frac: int) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFix32SetAt.Value.Invoke(afix32, lpos, whole, frac)

    let DTWAIN_ArrayFloatToANSIString (floatarray: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFloatToANSIString.Value.Invoke(floatarray)

    let DTWAIN_ArrayFloatToString (floatarray: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFloatToString.Value.Invoke(floatarray)

    let DTWAIN_ArrayFloatToWideString (floatarray: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayFloatToWideString.Value.Invoke(floatarray)

    let DTWAIN_ArrayGetAt (parray: DTWAIN_ARRAY) (nwhere: LONG) (pvariant: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAt.Value.Invoke(parray, nwhere, pvariant)

    let DTWAIN_ArrayGetAtANSIString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtANSIString.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArrayGetAtFloat (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFloat.Value.Invoke(parray, nwhere, &pval)

    let DTWAIN_ArrayGetAtFloatString (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFloatString.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArrayGetAtFloatStringA (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFloatStringA.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArrayGetAtFloatStringW (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFloatStringW.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArrayGetAtFrame (framearray: DTWAIN_ARRAY) (nwhere: LONG) (pleft: DTWAIN_FLOAT byref) (ptop: DTWAIN_FLOAT byref) (pright: DTWAIN_FLOAT byref) (pbottom: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFrame.Value.Invoke(framearray, nwhere, &pleft, &ptop, &pright, &pbottom)

    let DTWAIN_ArrayGetAtFrameEx (framearray: DTWAIN_ARRAY) (nwhere: LONG) (frame: DTWAIN_FRAME) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFrameEx.Value.Invoke(framearray, nwhere, frame)

    let DTWAIN_ArrayGetAtFrameString (framearray: DTWAIN_ARRAY) (nwhere: LONG) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFrameString.Value.Invoke(framearray, nwhere, left, top, right, bottom)

    let DTWAIN_ArrayGetAtFrameStringA (framearray: DTWAIN_ARRAY) (nwhere: LONG) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFrameStringA.Value.Invoke(framearray, nwhere, left, top, right, bottom)

    let DTWAIN_ArrayGetAtFrameStringW (framearray: DTWAIN_ARRAY) (nwhere: LONG) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtFrameStringW.Value.Invoke(framearray, nwhere, left, top, right, bottom)

    let DTWAIN_ArrayGetAtLong (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtLong.Value.Invoke(parray, nwhere, &pval)

    let DTWAIN_ArrayGetAtLong64 (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: Int64 byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtLong64.Value.Invoke(parray, nwhere, &pval)

    let DTWAIN_ArrayGetAtSource (parray: DTWAIN_ARRAY) (nwhere: LONG) (ppsource: DTWAIN_SOURCE byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtSource.Value.Invoke(parray, nwhere, &ppsource)

    let DTWAIN_ArrayGetAtString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtString.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArrayGetAtStringA (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtStringA.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArrayGetAtStringW (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtStringW.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArrayGetAtWideString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetAtWideString.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArrayGetBuffer (parray: DTWAIN_ARRAY) (npos: LONG) : LPVOID =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetBuffer.Value.Invoke(parray, npos)

    let DTWAIN_ArrayGetCapValues (source: DTWAIN_SOURCE) (lcap: LONG) (lgettype: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetCapValues.Value.Invoke(source, lcap, lgettype)

    let DTWAIN_ArrayGetCapValuesEx (source: DTWAIN_SOURCE) (lcap: LONG) (lgettype: LONG) (lcontainertype: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetCapValuesEx.Value.Invoke(source, lcap, lgettype, lcontainertype)

    let DTWAIN_ArrayGetCapValuesEx2 (source: DTWAIN_SOURCE) (lcap: LONG) (lgettype: LONG) (lcontainertype: LONG) (ndatatype: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetCapValuesEx2.Value.Invoke(source, lcap, lgettype, lcontainertype, ndatatype)

    let DTWAIN_ArrayGetCount (parray: DTWAIN_ARRAY) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetCount.Value.Invoke(parray)

    let DTWAIN_ArrayGetMaxStringLength (a: DTWAIN_ARRAY) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetMaxStringLength.Value.Invoke(a)

    let DTWAIN_ArrayGetSourceAt (parray: DTWAIN_ARRAY) (nwhere: LONG) (ppsource: DTWAIN_SOURCE byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetSourceAt.Value.Invoke(parray, nwhere, &ppsource)

    let DTWAIN_ArrayGetStringLength (a: DTWAIN_ARRAY) (nwhichstring: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetStringLength.Value.Invoke(a, nwhichstring)

    let DTWAIN_ArrayGetType (parray: DTWAIN_ARRAY) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayGetType.Value.Invoke(parray)

    let DTWAIN_ArrayInit() : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInit.Value.Invoke()

    let DTWAIN_ArrayInsertAt (parray: DTWAIN_ARRAY) (nwhere: LONG) (pvariant: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAt.Value.Invoke(parray, nwhere, pvariant)

    let DTWAIN_ArrayInsertAtANSIString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtANSIString.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArrayInsertAtANSIStringN (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtANSIStringN.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtFloat (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloat.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArrayInsertAtFloatN (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: DTWAIN_FLOAT) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloatN.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtFloatString (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloatString.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArrayInsertAtFloatStringA (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloatStringA.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArrayInsertAtFloatStringN (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloatStringN.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtFloatStringNA (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloatStringNA.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtFloatStringNW (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloatStringNW.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtFloatStringW (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFloatStringW.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArrayInsertAtFrame (parray: DTWAIN_ARRAY) (nwhere: LONG) (frame: DTWAIN_FRAME) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFrame.Value.Invoke(parray, nwhere, frame)

    let DTWAIN_ArrayInsertAtFrameN (parray: DTWAIN_ARRAY) (nwhere: LONG) (frame: DTWAIN_FRAME) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtFrameN.Value.Invoke(parray, nwhere, frame, num)

    let DTWAIN_ArrayInsertAtLong (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtLong.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArrayInsertAtLong64 (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: LONG64) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtLong64.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArrayInsertAtLong64N (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: LONG64) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtLong64N.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtLongN (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: LONG) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtLongN.Value.Invoke(parray, nwhere, pval, num)

    let DTWAIN_ArrayInsertAtN (parray: DTWAIN_ARRAY) (nwhere: LONG) (pvariant: LPVOID) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtN.Value.Invoke(parray, nwhere, pvariant, num)

    let DTWAIN_ArrayInsertAtString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtString.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArrayInsertAtStringA (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtStringA.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArrayInsertAtStringN (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtStringN.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtStringNA (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtStringNA.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtStringNW (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtStringNW.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayInsertAtStringW (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtStringW.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArrayInsertAtWideString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtWideString.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArrayInsertAtWideStringN (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayInsertAtWideStringN.Value.Invoke(parray, nwhere, val1, num)

    let DTWAIN_ArrayRemoveAll (parray: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayRemoveAll.Value.Invoke(parray)

    let DTWAIN_ArrayRemoveAt (parray: DTWAIN_ARRAY) (nwhere: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayRemoveAt.Value.Invoke(parray, nwhere)

    let DTWAIN_ArrayRemoveAtN (parray: DTWAIN_ARRAY) (nwhere: LONG) (num: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayRemoveAtN.Value.Invoke(parray, nwhere, num)

    let DTWAIN_ArrayResize (parray: DTWAIN_ARRAY) (newsize: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayResize.Value.Invoke(parray, newsize)

    let DTWAIN_ArraySetAt (parray: DTWAIN_ARRAY) (lpos: LONG) (pvariant: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAt.Value.Invoke(parray, lpos, pvariant)

    let DTWAIN_ArraySetAtANSIString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtANSIString.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArraySetAtFloat (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFloat.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArraySetAtFloatString (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFloatString.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArraySetAtFloatStringA (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFloatStringA.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArraySetAtFloatStringW (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFloatStringW.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArraySetAtFrame (framearray: DTWAIN_ARRAY) (nwhere: LONG) (left: DTWAIN_FLOAT) (top: DTWAIN_FLOAT) (right: DTWAIN_FLOAT) (bottom: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFrame.Value.Invoke(framearray, nwhere, left, top, right, bottom)

    let DTWAIN_ArraySetAtFrameEx (framearray: DTWAIN_ARRAY) (nwhere: LONG) (frame: DTWAIN_FRAME) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFrameEx.Value.Invoke(framearray, nwhere, frame)

    let DTWAIN_ArraySetAtFrameString (framearray: DTWAIN_ARRAY) (nwhere: LONG) (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFrameString.Value.Invoke(framearray, nwhere, left, top, right, bottom)

    let DTWAIN_ArraySetAtFrameStringA (framearray: DTWAIN_ARRAY) (nwhere: LONG) (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFrameStringA.Value.Invoke(framearray, nwhere, left, top, right, bottom)

    let DTWAIN_ArraySetAtFrameStringW (framearray: DTWAIN_ARRAY) (nwhere: LONG) (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtFrameStringW.Value.Invoke(framearray, nwhere, left, top, right, bottom)

    let DTWAIN_ArraySetAtLong (parray: DTWAIN_ARRAY) (nwhere: LONG) (pval: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtLong.Value.Invoke(parray, nwhere, pval)

    let DTWAIN_ArraySetAtLong64 (parray: DTWAIN_ARRAY) (nwhere: LONG) (val1: LONG64) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtLong64.Value.Invoke(parray, nwhere, val1)

    let DTWAIN_ArraySetAtString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtString.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArraySetAtStringA (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtStringA.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArraySetAtStringW (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtStringW.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArraySetAtWideString (parray: DTWAIN_ARRAY) (nwhere: LONG) (pstr: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArraySetAtWideString.Value.Invoke(parray, nwhere, pstr)

    let DTWAIN_ArrayStringToFloat (stringarray: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayStringToFloat.Value.Invoke(stringarray)

    let DTWAIN_ArrayWideStringToFloat (stringarray: DTWAIN_ARRAY) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ArrayWideStringToFloat.Value.Invoke(stringarray)

    let DTWAIN_CallCallback (wparam: nativeint) (lparam: nativeint) (userdata: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CallCallback.Value.Invoke(wparam, lparam, userdata)

    let DTWAIN_CallCallback64 (wparam: nativeint) (lparam: nativeint) (userdata: LONGLONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CallCallback64.Value.Invoke(wparam, lparam, userdata)

    let DTWAIN_CallDSMProc (appid: DTWAIN_IDENTITY) (sourceid: DTWAIN_IDENTITY) (ldg: LONG) (ldat: LONG) (lmsg: LONG) (pdata: LPVOID) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CallDSMProc.Value.Invoke(appid, sourceid, ldg, ldat, lmsg, pdata)

    let DTWAIN_CheckHandles (bcheck: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CheckHandles.Value.Invoke(bcheck)

    let DTWAIN_ClearBuffers (source: DTWAIN_SOURCE) (clearbuffer: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ClearBuffers.Value.Invoke(source, clearbuffer)

    let DTWAIN_ClearErrorBuffer() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ClearErrorBuffer.Value.Invoke()

    let DTWAIN_ClearPDFText (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ClearPDFText.Value.Invoke(source)

    let DTWAIN_ClearPage (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ClearPage.Value.Invoke(source)

    let DTWAIN_CloseSource (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CloseSource.Value.Invoke(source)

    let DTWAIN_CloseSourceUI (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CloseSourceUI.Value.Invoke(source)

    let DTWAIN_ConvertDIBToBitmap (hdib: HANDLE) (hpalette: HANDLE) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertDIBToBitmap.Value.Invoke(hdib, hpalette)

    let DTWAIN_ConvertDIBToFullBitmap (hdib: HANDLE) (isbmp: DTWAIN_BOOL) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertDIBToFullBitmap.Value.Invoke(hdib, isbmp)

    let DTWAIN_ConvertToAPIString (lporigstring: string) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertToAPIString.Value.Invoke(lporigstring)

    let DTWAIN_ConvertToAPIStringA (lporigstring: string) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertToAPIStringA.Value.Invoke(lporigstring)

    let DTWAIN_ConvertToAPIStringEx (lporigstring: string) (lpoutstring: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertToAPIStringEx.Value.Invoke(lporigstring, lpoutstring, nsize)

    let DTWAIN_ConvertToAPIStringExA (lporigstring: string) (lpoutstring: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertToAPIStringExA.Value.Invoke(lporigstring, lpoutstring, nsize)

    let DTWAIN_ConvertToAPIStringExW (lporigstring: string) (lpoutstring: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertToAPIStringExW.Value.Invoke(lporigstring, lpoutstring, nsize)

    let DTWAIN_ConvertToAPIStringW (lporigstring: string) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ConvertToAPIStringW.Value.Invoke(lporigstring)

    let DTWAIN_CreateAcquisitionArray() : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CreateAcquisitionArray.Value.Invoke()

    let DTWAIN_CreatePDFTextElement (source: DTWAIN_SOURCE) : DTWAIN_PDFTEXTELEMENT =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        CreatePDFTextElement.Value.Invoke(source)

    let DTWAIN_DeleteDIB (hdib: HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        DeleteDIB.Value.Invoke(hdib)

    let DTWAIN_DestroyAcquisitionArray (aacq: DTWAIN_ARRAY) (bdestroydata: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        DestroyAcquisitionArray.Value.Invoke(aacq, bdestroydata)

    let DTWAIN_DestroyPDFTextElement (textelement: DTWAIN_PDFTEXTELEMENT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        DestroyPDFTextElement.Value.Invoke(textelement)

    let DTWAIN_DisableAppWindow (hwnd: HWND) (bdisable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        DisableAppWindow.Value.Invoke(hwnd, bdisable)

    let DTWAIN_EnableAutoBorderDetect (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableAutoBorderDetect.Value.Invoke(source, benable)

    let DTWAIN_EnableAutoBright (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableAutoBright.Value.Invoke(source, bset)

    let DTWAIN_EnableAutoDeskew (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableAutoDeskew.Value.Invoke(source, benable)

    let DTWAIN_EnableAutoFeed (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableAutoFeed.Value.Invoke(source, bset)

    let DTWAIN_EnableAutoRotate (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableAutoRotate.Value.Invoke(source, bset)

    let DTWAIN_EnableAutoScan (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableAutoScan.Value.Invoke(source, benable)

    let DTWAIN_EnableAutomaticSenseMedium (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableAutomaticSenseMedium.Value.Invoke(source, bset)

    let DTWAIN_EnableDuplex (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableDuplex.Value.Invoke(source, benable)

    let DTWAIN_EnableFeeder (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableFeeder.Value.Invoke(source, bset)

    let DTWAIN_EnableIndicator (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableIndicator.Value.Invoke(source, benable)

    let DTWAIN_EnableJobFileHandling (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableJobFileHandling.Value.Invoke(source, bset)

    let DTWAIN_EnableLamp (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableLamp.Value.Invoke(source, benable)

    let DTWAIN_EnableMsgNotify (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableMsgNotify.Value.Invoke(bset)

    let DTWAIN_EnablePatchDetect (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnablePatchDetect.Value.Invoke(source, benable)

    let DTWAIN_EnablePeekMessageLoop (source: DTWAIN_SOURCE) (bset: BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnablePeekMessageLoop.Value.Invoke(source, bset)

    let DTWAIN_EnablePrinter (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnablePrinter.Value.Invoke(source, benable)

    let DTWAIN_EnableThumbnail (source: DTWAIN_SOURCE) (benable: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableThumbnail.Value.Invoke(source, benable)

    let DTWAIN_EnableTripletsNotify (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnableTripletsNotify.Value.Invoke(bset)

    let DTWAIN_EndThread (dllhandle: DTWAIN_HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EndThread.Value.Invoke(dllhandle)

    let DTWAIN_EndTwainSession() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EndTwainSession.Value.Invoke()

    let DTWAIN_EnumAlarmVolumes (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (expandifrange: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAlarmVolumes.Value.Invoke(source, &parray, expandifrange)

    let DTWAIN_EnumAlarmVolumesEx (source: DTWAIN_SOURCE) (expandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAlarmVolumesEx.Value.Invoke(source, expandifrange)

    let DTWAIN_EnumAlarms (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAlarms.Value.Invoke(source, &parray)

    let DTWAIN_EnumAlarmsEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAlarmsEx.Value.Invoke(source)

    let DTWAIN_EnumAudioXferMechs (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAudioXferMechs.Value.Invoke(source, &parray)

    let DTWAIN_EnumAudioXferMechsEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAudioXferMechsEx.Value.Invoke(source)

    let DTWAIN_EnumAutoFeedValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAutoFeedValues.Value.Invoke(source, &parray)

    let DTWAIN_EnumAutoFeedValuesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAutoFeedValuesEx.Value.Invoke(source)

    let DTWAIN_EnumAutomaticCaptures (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAutomaticCaptures.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumAutomaticCapturesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAutomaticCapturesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumAutomaticSenseMedium (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAutomaticSenseMedium.Value.Invoke(source, &parray)

    let DTWAIN_EnumAutomaticSenseMediumEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumAutomaticSenseMediumEx.Value.Invoke(source)

    let DTWAIN_EnumBitDepths (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumBitDepths.Value.Invoke(source, &parray)

    let DTWAIN_EnumBitDepthsEx (source: DTWAIN_SOURCE) (pixeltype: LONG) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumBitDepthsEx.Value.Invoke(source, pixeltype, &parray)

    let DTWAIN_EnumBitDepthsEx2 (source: DTWAIN_SOURCE) (pixeltype: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumBitDepthsEx2.Value.Invoke(source, pixeltype)

    let DTWAIN_EnumBottomCameras (source: DTWAIN_SOURCE) (cameras: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumBottomCameras.Value.Invoke(source, &cameras)

    let DTWAIN_EnumBottomCamerasEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumBottomCamerasEx.Value.Invoke(source)

    let DTWAIN_EnumBrightnessValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumBrightnessValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumBrightnessValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumBrightnessValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumCameras (source: DTWAIN_SOURCE) (cameras: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCameras.Value.Invoke(source, &cameras)

    let DTWAIN_EnumCamerasEx (source: DTWAIN_SOURCE) (nwhichcamera: LONG) (cameras: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCamerasEx.Value.Invoke(source, nwhichcamera, &cameras)

    let DTWAIN_EnumCamerasEx2 (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCamerasEx2.Value.Invoke(source)

    let DTWAIN_EnumCamerasEx3 (source: DTWAIN_SOURCE) (nwhichcamera: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCamerasEx3.Value.Invoke(source, nwhichcamera)

    let DTWAIN_EnumCompressionTypes (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCompressionTypes.Value.Invoke(source, &parray)

    let DTWAIN_EnumCompressionTypesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCompressionTypesEx.Value.Invoke(source)

    let DTWAIN_EnumCompressionTypesEx2 (source: DTWAIN_SOURCE) (lfiletype: LONG) (busebufferedmode: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCompressionTypesEx2.Value.Invoke(source, lfiletype, busebufferedmode)

    let DTWAIN_EnumContrastValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumContrastValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumContrastValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumContrastValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumCustomCaps (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCustomCaps.Value.Invoke(source, &parray)

    let DTWAIN_EnumCustomCapsEx2 (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumCustomCapsEx2.Value.Invoke(source)

    let DTWAIN_EnumDoubleFeedDetectLengths (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumDoubleFeedDetectLengths.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumDoubleFeedDetectLengthsEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumDoubleFeedDetectLengthsEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumDoubleFeedDetectValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumDoubleFeedDetectValues.Value.Invoke(source, &parray)

    let DTWAIN_EnumDoubleFeedDetectValuesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumDoubleFeedDetectValuesEx.Value.Invoke(source)

    let DTWAIN_EnumExtImageInfoTypes (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumExtImageInfoTypes.Value.Invoke(source, &parray)

    let DTWAIN_EnumExtImageInfoTypesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumExtImageInfoTypesEx.Value.Invoke(source)

    let DTWAIN_EnumExtendedCaps (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumExtendedCaps.Value.Invoke(source, &parray)

    let DTWAIN_EnumExtendedCapsEx (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumExtendedCapsEx.Value.Invoke(source, &parray)

    let DTWAIN_EnumExtendedCapsEx2 (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumExtendedCapsEx2.Value.Invoke(source)

    let DTWAIN_EnumFileTypeBitsPerPixel (filetype: LONG) (array: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumFileTypeBitsPerPixel.Value.Invoke(filetype, &array)

    let DTWAIN_EnumFileXferFormats (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumFileXferFormats.Value.Invoke(source, &parray)

    let DTWAIN_EnumFileXferFormatsEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumFileXferFormatsEx.Value.Invoke(source)

    let DTWAIN_EnumHalftones (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumHalftones.Value.Invoke(source, &parray)

    let DTWAIN_EnumHalftonesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumHalftonesEx.Value.Invoke(source)

    let DTWAIN_EnumHighlightValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumHighlightValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumHighlightValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumHighlightValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumJobControls (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumJobControls.Value.Invoke(source, &parray)

    let DTWAIN_EnumJobControlsEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumJobControlsEx.Value.Invoke(source)

    let DTWAIN_EnumLightPaths (source: DTWAIN_SOURCE) (lightpath: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumLightPaths.Value.Invoke(source, &lightpath)

    let DTWAIN_EnumLightPathsEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumLightPathsEx.Value.Invoke(source)

    let DTWAIN_EnumLightSources (source: DTWAIN_SOURCE) (lightsources: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumLightSources.Value.Invoke(source, &lightsources)

    let DTWAIN_EnumLightSourcesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumLightSourcesEx.Value.Invoke(source)

    let DTWAIN_EnumMaxBuffers (source: DTWAIN_SOURCE) (pmaxbufs: DTWAIN_ARRAY byref) (bexpandrange: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumMaxBuffers.Value.Invoke(source, &pmaxbufs, bexpandrange)

    let DTWAIN_EnumMaxBuffersEx (source: DTWAIN_SOURCE) (bexpandrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumMaxBuffersEx.Value.Invoke(source, bexpandrange)

    let DTWAIN_EnumNoiseFilters (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumNoiseFilters.Value.Invoke(source, &parray)

    let DTWAIN_EnumNoiseFiltersEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumNoiseFiltersEx.Value.Invoke(source)

    let DTWAIN_EnumOCRInterfaces (ocrinterfaces: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumOCRInterfaces.Value.Invoke(&ocrinterfaces)

    let DTWAIN_EnumOCRSupportedCaps (engine: DTWAIN_OCRENGINE) (supportedcaps: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumOCRSupportedCaps.Value.Invoke(engine, &supportedcaps)

    let DTWAIN_EnumOrientations (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumOrientations.Value.Invoke(source, &parray)

    let DTWAIN_EnumOrientationsEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumOrientationsEx.Value.Invoke(source)

    let DTWAIN_EnumOverscanValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumOverscanValues.Value.Invoke(source, &parray)

    let DTWAIN_EnumOverscanValuesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumOverscanValuesEx.Value.Invoke(source)

    let DTWAIN_EnumPaperSizes (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPaperSizes.Value.Invoke(source, &parray)

    let DTWAIN_EnumPaperSizesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPaperSizesEx.Value.Invoke(source)

    let DTWAIN_EnumPatchCodes (source: DTWAIN_SOURCE) (pcodes: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchCodes.Value.Invoke(source, &pcodes)

    let DTWAIN_EnumPatchCodesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchCodesEx.Value.Invoke(source)

    let DTWAIN_EnumPatchMaxPriorities (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchMaxPriorities.Value.Invoke(source, &parray)

    let DTWAIN_EnumPatchMaxPrioritiesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchMaxPrioritiesEx.Value.Invoke(source)

    let DTWAIN_EnumPatchMaxRetries (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchMaxRetries.Value.Invoke(source, &parray)

    let DTWAIN_EnumPatchMaxRetriesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchMaxRetriesEx.Value.Invoke(source)

    let DTWAIN_EnumPatchPriorities (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchPriorities.Value.Invoke(source, &parray)

    let DTWAIN_EnumPatchPrioritiesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchPrioritiesEx.Value.Invoke(source)

    let DTWAIN_EnumPatchSearchModes (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchSearchModes.Value.Invoke(source, &parray)

    let DTWAIN_EnumPatchSearchModesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchSearchModesEx.Value.Invoke(source)

    let DTWAIN_EnumPatchTimeOutValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchTimeOutValues.Value.Invoke(source, &parray)

    let DTWAIN_EnumPatchTimeOutValuesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPatchTimeOutValuesEx.Value.Invoke(source)

    let DTWAIN_EnumPixelTypes (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPixelTypes.Value.Invoke(source, &parray)

    let DTWAIN_EnumPixelTypesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPixelTypesEx.Value.Invoke(source)

    let DTWAIN_EnumPrinterStringModes (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPrinterStringModes.Value.Invoke(source, &parray)

    let DTWAIN_EnumPrinterStringModesEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumPrinterStringModesEx.Value.Invoke(source)

    let DTWAIN_EnumResolutionValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumResolutionValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumResolutionValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumResolutionValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumShadowValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumShadowValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumShadowValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumShadowValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumSourceUnits (source: DTWAIN_SOURCE) (lparray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSourceUnits.Value.Invoke(source, &lparray)

    let DTWAIN_EnumSourceUnitsEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSourceUnitsEx.Value.Invoke(source)

    let DTWAIN_EnumSourceValues (source: DTWAIN_SOURCE) (capname: string) (values: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSourceValues.Value.Invoke(source, capname, &values, bexpandifrange)

    let DTWAIN_EnumSourceValuesA (source: DTWAIN_SOURCE) (capname: string) (values: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSourceValuesA.Value.Invoke(source, capname, &values, bexpandifrange)

    let DTWAIN_EnumSourceValuesW (source: DTWAIN_SOURCE) (capname: string) (values: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSourceValuesW.Value.Invoke(source, capname, &values, bexpandifrange)

    let DTWAIN_EnumSources (lparray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSources.Value.Invoke(&lparray)

    let DTWAIN_EnumSourcesEx() : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSourcesEx.Value.Invoke()

    let DTWAIN_EnumSupportedCaps (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedCaps.Value.Invoke(source, &parray)

    let DTWAIN_EnumSupportedCapsEx (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedCapsEx.Value.Invoke(source, &parray)

    let DTWAIN_EnumSupportedCapsEx2 (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedCapsEx2.Value.Invoke(source)

    let DTWAIN_EnumSupportedExtImageInfo (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedExtImageInfo.Value.Invoke(source, &parray)

    let DTWAIN_EnumSupportedExtImageInfoEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedExtImageInfoEx.Value.Invoke(source)

    let DTWAIN_EnumSupportedFileTypes() : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedFileTypes.Value.Invoke()

    let DTWAIN_EnumSupportedMultiPageFileTypes() : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedMultiPageFileTypes.Value.Invoke()

    let DTWAIN_EnumSupportedSinglePageFileTypes() : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumSupportedSinglePageFileTypes.Value.Invoke()

    let DTWAIN_EnumThresholdValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumThresholdValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumThresholdValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumThresholdValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumTopCameras (source: DTWAIN_SOURCE) (cameras: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumTopCameras.Value.Invoke(source, &cameras)

    let DTWAIN_EnumTopCamerasEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumTopCamerasEx.Value.Invoke(source)

    let DTWAIN_EnumTwainPrinters (source: DTWAIN_SOURCE) (lpavailprinters: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumTwainPrinters.Value.Invoke(source, &lpavailprinters)

    let DTWAIN_EnumTwainPrintersArray (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumTwainPrintersArray.Value.Invoke(source, &parray)

    let DTWAIN_EnumTwainPrintersArrayEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumTwainPrintersArrayEx.Value.Invoke(source)

    let DTWAIN_EnumTwainPrintersEx (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumTwainPrintersEx.Value.Invoke(source)

    let DTWAIN_EnumXResolutionValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumXResolutionValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumXResolutionValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumXResolutionValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_EnumYResolutionValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) (bexpandifrange: DTWAIN_BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumYResolutionValues.Value.Invoke(source, &parray, bexpandifrange)

    let DTWAIN_EnumYResolutionValuesEx (source: DTWAIN_SOURCE) (bexpandifrange: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        EnumYResolutionValuesEx.Value.Invoke(source, bexpandifrange)

    let DTWAIN_ExecuteOCR (engine: DTWAIN_OCRENGINE) (szfilename: string) (nstartpage: LONG) (nendpage: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ExecuteOCR.Value.Invoke(engine, szfilename, nstartpage, nendpage)

    let DTWAIN_ExecuteOCRA (engine: DTWAIN_OCRENGINE) (szfilename: string) (nstartpage: LONG) (nendpage: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ExecuteOCRA.Value.Invoke(engine, szfilename, nstartpage, nendpage)

    let DTWAIN_ExecuteOCRW (engine: DTWAIN_OCRENGINE) (szfilename: string) (nstartpage: LONG) (nendpage: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ExecuteOCRW.Value.Invoke(engine, szfilename, nstartpage, nendpage)

    let DTWAIN_FeedPage (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FeedPage.Value.Invoke(source)

    let DTWAIN_FlipBitmap (hdib: HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FlipBitmap.Value.Invoke(hdib)

    let DTWAIN_FlushAcquiredPages (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FlushAcquiredPages.Value.Invoke(source)

    let DTWAIN_ForceAcquireBitDepth (source: DTWAIN_SOURCE) (bitdepth: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ForceAcquireBitDepth.Value.Invoke(source, bitdepth)

    let DTWAIN_ForceScanOnNoUI (source: DTWAIN_SOURCE) (bset: BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ForceScanOnNoUI.Value.Invoke(source, bset)

    let DTWAIN_FrameCreate (left: DTWAIN_FLOAT) (top: DTWAIN_FLOAT) (right: DTWAIN_FLOAT) (bottom: DTWAIN_FLOAT) : DTWAIN_FRAME =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameCreate.Value.Invoke(left, top, right, bottom)

    let DTWAIN_FrameCreateString (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_FRAME =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameCreateString.Value.Invoke(left, top, right, bottom)

    let DTWAIN_FrameCreateStringA (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_FRAME =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameCreateStringA.Value.Invoke(left, top, right, bottom)

    let DTWAIN_FrameCreateStringW (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_FRAME =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameCreateStringW.Value.Invoke(left, top, right, bottom)

    let DTWAIN_FrameDestroy (frame: DTWAIN_FRAME) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameDestroy.Value.Invoke(frame)

    let DTWAIN_FrameGetAll (frame: DTWAIN_FRAME) (left: DTWAIN_FLOAT byref) (top: DTWAIN_FLOAT byref) (right: DTWAIN_FLOAT byref) (bottom: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetAll.Value.Invoke(frame, &left, &top, &right, &bottom)

    let DTWAIN_FrameGetAllString (frame: DTWAIN_FRAME) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetAllString.Value.Invoke(frame, left, top, right, bottom)

    let DTWAIN_FrameGetAllStringA (frame: DTWAIN_FRAME) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetAllStringA.Value.Invoke(frame, left, top, right, bottom)

    let DTWAIN_FrameGetAllStringW (frame: DTWAIN_FRAME) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetAllStringW.Value.Invoke(frame, left, top, right, bottom)

    let DTWAIN_FrameGetValue (frame: DTWAIN_FRAME) (nwhich: LONG) (value: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetValue.Value.Invoke(frame, nwhich, &value)

    let DTWAIN_FrameGetValueString (frame: DTWAIN_FRAME) (nwhich: LONG) (value: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetValueString.Value.Invoke(frame, nwhich, value)

    let DTWAIN_FrameGetValueStringA (frame: DTWAIN_FRAME) (nwhich: LONG) (value: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetValueStringA.Value.Invoke(frame, nwhich, value)

    let DTWAIN_FrameGetValueStringW (frame: DTWAIN_FRAME) (nwhich: LONG) (value: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameGetValueStringW.Value.Invoke(frame, nwhich, value)

    let DTWAIN_FrameIsValid (frame: DTWAIN_FRAME) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameIsValid.Value.Invoke(frame)

    let DTWAIN_FrameSetAll (frame: DTWAIN_FRAME) (left: DTWAIN_FLOAT) (top: DTWAIN_FLOAT) (right: DTWAIN_FLOAT) (bottom: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetAll.Value.Invoke(frame, left, top, right, bottom)

    let DTWAIN_FrameSetAllString (frame: DTWAIN_FRAME) (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetAllString.Value.Invoke(frame, left, top, right, bottom)

    let DTWAIN_FrameSetAllStringA (frame: DTWAIN_FRAME) (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetAllStringA.Value.Invoke(frame, left, top, right, bottom)

    let DTWAIN_FrameSetAllStringW (frame: DTWAIN_FRAME) (left: string) (top: string) (right: string) (bottom: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetAllStringW.Value.Invoke(frame, left, top, right, bottom)

    let DTWAIN_FrameSetValue (frame: DTWAIN_FRAME) (nwhich: LONG) (value: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetValue.Value.Invoke(frame, nwhich, value)

    let DTWAIN_FrameSetValueString (frame: DTWAIN_FRAME) (nwhich: LONG) (value: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetValueString.Value.Invoke(frame, nwhich, value)

    let DTWAIN_FrameSetValueStringA (frame: DTWAIN_FRAME) (nwhich: LONG) (value: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetValueStringA.Value.Invoke(frame, nwhich, value)

    let DTWAIN_FrameSetValueStringW (frame: DTWAIN_FRAME) (nwhich: LONG) (value: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FrameSetValueStringW.Value.Invoke(frame, nwhich, value)

    let DTWAIN_FreeExtImageInfo (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FreeExtImageInfo.Value.Invoke(source)

    let DTWAIN_FreeMemory (h: HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FreeMemory.Value.Invoke(h)

    let DTWAIN_FreeMemoryEx (h: HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        FreeMemoryEx.Value.Invoke(h)

    let DTWAIN_GetAPIHandleStatus (phandle: DTWAIN_HANDLE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAPIHandleStatus.Value.Invoke(phandle)

    let DTWAIN_GetAcquireArea (source: DTWAIN_SOURCE) (lgettype: LONG) (floatenum: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireArea.Value.Invoke(source, lgettype, &floatenum)

    let DTWAIN_GetAcquireArea2 (source: DTWAIN_SOURCE) (left: DTWAIN_FLOAT byref) (top: DTWAIN_FLOAT byref) (right: DTWAIN_FLOAT byref) (bottom: DTWAIN_FLOAT byref) (lpunit: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireArea2.Value.Invoke(source, &left, &top, &right, &bottom, &lpunit)

    let DTWAIN_GetAcquireArea2String (source: DTWAIN_SOURCE) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) (unit: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireArea2String.Value.Invoke(source, left, top, right, bottom, &unit)

    let DTWAIN_GetAcquireArea2StringA (source: DTWAIN_SOURCE) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) (unit: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireArea2StringA.Value.Invoke(source, left, top, right, bottom, &unit)

    let DTWAIN_GetAcquireArea2StringW (source: DTWAIN_SOURCE) (left: System.Text.StringBuilder) (top: System.Text.StringBuilder) (right: System.Text.StringBuilder) (bottom: System.Text.StringBuilder) (unit: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireArea2StringW.Value.Invoke(source, left, top, right, bottom, &unit)

    let DTWAIN_GetAcquireAreaEx (source: DTWAIN_SOURCE) (lgettype: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireAreaEx.Value.Invoke(source, lgettype)

    let DTWAIN_GetAcquireMetrics (source: DTWAIN_SOURCE) (imagecount: int byref) (sheetcount: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireMetrics.Value.Invoke(source, &imagecount, &sheetcount)

    let DTWAIN_GetAcquireStripBuffer (source: DTWAIN_SOURCE) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireStripBuffer.Value.Invoke(source)

    let DTWAIN_GetAcquireStripData (source: DTWAIN_SOURCE) (lpcompression: int byref) (lpbytesperrow: DWORD byref) (lpcolumns: DWORD byref) (lprows: DWORD byref) (xoffset: DWORD byref) (yoffset: DWORD byref) (lpbyteswritten: DWORD byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireStripData.Value.Invoke(source, &lpcompression, &lpbytesperrow, &lpcolumns, &lprows, &xoffset, &yoffset, &lpbyteswritten)

    let DTWAIN_GetAcquireStripSizes (source: DTWAIN_SOURCE) (lpmin: DWORD byref) (lpmax: DWORD byref) (lppreferred: DWORD byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquireStripSizes.Value.Invoke(source, &lpmin, &lpmax, &lppreferred)

    let DTWAIN_GetAcquiredImage (aacq: DTWAIN_ARRAY) (nwhichacq: LONG) (nwhichdib: LONG) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquiredImage.Value.Invoke(aacq, nwhichacq, nwhichdib)

    let DTWAIN_GetAcquiredImageArray (aacq: DTWAIN_ARRAY) (nwhichacq: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAcquiredImageArray.Value.Invoke(aacq, nwhichacq)

    let DTWAIN_GetActiveDSMPath (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetActiveDSMPath.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetActiveDSMPathA (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetActiveDSMPathA.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetActiveDSMPathW (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetActiveDSMPathW.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetActiveDSMVersionInfo (szdllinfo: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetActiveDSMVersionInfo.Value.Invoke(szdllinfo, nmaxlen)

    let DTWAIN_GetActiveDSMVersionInfoA (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetActiveDSMVersionInfoA.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetActiveDSMVersionInfoW (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetActiveDSMVersionInfoW.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetAlarmVolume (source: DTWAIN_SOURCE) (lpvolume: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAlarmVolume.Value.Invoke(source, &lpvolume)

    let DTWAIN_GetAllSourceDibs (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAllSourceDibs.Value.Invoke(source)

    let DTWAIN_GetAppInfo (szverstr: System.Text.StringBuilder) (szmanu: System.Text.StringBuilder) (szprodfam: System.Text.StringBuilder) (szprodname: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAppInfo.Value.Invoke(szverstr, szmanu, szprodfam, szprodname)

    let DTWAIN_GetAppInfoA (szverstr: System.Text.StringBuilder) (szmanu: System.Text.StringBuilder) (szprodfam: System.Text.StringBuilder) (szprodname: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAppInfoA.Value.Invoke(szverstr, szmanu, szprodfam, szprodname)

    let DTWAIN_GetAppInfoW (szverstr: System.Text.StringBuilder) (szmanu: System.Text.StringBuilder) (szprodfam: System.Text.StringBuilder) (szprodname: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAppInfoW.Value.Invoke(szverstr, szmanu, szprodfam, szprodname)

    let DTWAIN_GetAuthor (source: DTWAIN_SOURCE) (szauthor: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAuthor.Value.Invoke(source, szauthor)

    let DTWAIN_GetAuthorA (source: DTWAIN_SOURCE) (szauthor: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAuthorA.Value.Invoke(source, szauthor)

    let DTWAIN_GetAuthorW (source: DTWAIN_SOURCE) (szauthor: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetAuthorW.Value.Invoke(source, szauthor)

    let DTWAIN_GetBatteryMinutes (source: DTWAIN_SOURCE) (lpminutes: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBatteryMinutes.Value.Invoke(source, &lpminutes)

    let DTWAIN_GetBatteryPercent (source: DTWAIN_SOURCE) (lppercent: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBatteryPercent.Value.Invoke(source, &lppercent)

    let DTWAIN_GetBitDepth (source: DTWAIN_SOURCE) (bitdepth: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBitDepth.Value.Invoke(source, &bitdepth, bcurrent)

    let DTWAIN_GetBlankPageAutoDetection (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBlankPageAutoDetection.Value.Invoke(source)

    let DTWAIN_GetBrightness (source: DTWAIN_SOURCE) (brightness: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBrightness.Value.Invoke(source, &brightness)

    let DTWAIN_GetBrightnessString (source: DTWAIN_SOURCE) (brightness: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBrightnessString.Value.Invoke(source, brightness)

    let DTWAIN_GetBrightnessStringA (source: DTWAIN_SOURCE) (contrast: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBrightnessStringA.Value.Invoke(source, contrast)

    let DTWAIN_GetBrightnessStringW (source: DTWAIN_SOURCE) (contrast: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBrightnessStringW.Value.Invoke(source, contrast)

    let DTWAIN_GetBufferedTransferInfo (source: DTWAIN_SOURCE) (compression: DWORD byref) (bytesperrow: DWORD byref) (columns: DWORD byref) (rows: DWORD byref) (xoffset: DWORD byref) (yoffset: DWORD byref) (flags: DWORD byref) (byteswritten: DWORD byref) (memorylength: DWORD byref) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetBufferedTransferInfo.Value.Invoke(source, &compression, &bytesperrow, &columns, &rows, &xoffset, &yoffset, &flags, &byteswritten, &memorylength)

    let DTWAIN_GetCallback() : DTWAIN_CALLBACK_PROC =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCallback.Value.Invoke()

    let DTWAIN_GetCallback64() : DTWAIN_CALLBACK_PROC64 =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCallback64.Value.Invoke()

    let DTWAIN_GetCapArrayType (source: DTWAIN_SOURCE) (ncap: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapArrayType.Value.Invoke(source, ncap)

    let DTWAIN_GetCapContainer (source: DTWAIN_SOURCE) (ncap: LONG) (lcaptype: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapContainer.Value.Invoke(source, ncap, lcaptype)

    let DTWAIN_GetCapContainerEx (ncap: LONG) (bsetcontainer: DTWAIN_BOOL) (contypes: DTWAIN_ARRAY byref) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapContainerEx.Value.Invoke(ncap, bsetcontainer, &contypes)

    let DTWAIN_GetCapContainerEx2 (ncap: LONG) (bsetcontainer: DTWAIN_BOOL) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapContainerEx2.Value.Invoke(ncap, bsetcontainer)

    let DTWAIN_GetCapDataType (source: DTWAIN_SOURCE) (ncap: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapDataType.Value.Invoke(source, ncap)

    let DTWAIN_GetCapFromName (szname: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapFromName.Value.Invoke(szname)

    let DTWAIN_GetCapFromNameA (szname: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapFromNameA.Value.Invoke(szname)

    let DTWAIN_GetCapFromNameW (szname: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapFromNameW.Value.Invoke(szname)

    let DTWAIN_GetCapOperations (source: DTWAIN_SOURCE) (lcapability: LONG) (lpops: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapOperations.Value.Invoke(source, lcapability, &lpops)

    let DTWAIN_GetCapValues (source: DTWAIN_SOURCE) (lcap: LONG) (lgettype: LONG) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapValues.Value.Invoke(source, lcap, lgettype, &parray)

    let DTWAIN_GetCapValuesEx (source: DTWAIN_SOURCE) (lcap: LONG) (lgettype: LONG) (lcontainertype: LONG) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapValuesEx.Value.Invoke(source, lcap, lgettype, lcontainertype, &parray)

    let DTWAIN_GetCapValuesEx2 (source: DTWAIN_SOURCE) (lcap: LONG) (lgettype: LONG) (lcontainertype: LONG) (ndatatype: LONG) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCapValuesEx2.Value.Invoke(source, lcap, lgettype, lcontainertype, ndatatype, &parray)

    let DTWAIN_GetCaption (source: DTWAIN_SOURCE) (caption: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCaption.Value.Invoke(source, caption)

    let DTWAIN_GetCaptionA (source: DTWAIN_SOURCE) (caption: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCaptionA.Value.Invoke(source, caption)

    let DTWAIN_GetCaptionW (source: DTWAIN_SOURCE) (caption: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCaptionW.Value.Invoke(source, caption)

    let DTWAIN_GetCompressionSize (source: DTWAIN_SOURCE) (lbytes: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCompressionSize.Value.Invoke(source, &lbytes)

    let DTWAIN_GetCompressionType (source: DTWAIN_SOURCE) (lpcompression: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCompressionType.Value.Invoke(source, &lpcompression, bcurrent)

    let DTWAIN_GetConditionCodeString (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetConditionCodeString.Value.Invoke(lerror, lpszbuffer, nmaxlen)

    let DTWAIN_GetConditionCodeStringA (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetConditionCodeStringA.Value.Invoke(lerror, lpszbuffer, nmaxlen)

    let DTWAIN_GetConditionCodeStringW (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetConditionCodeStringW.Value.Invoke(lerror, lpszbuffer, nmaxlen)

    let DTWAIN_GetContrast (source: DTWAIN_SOURCE) (contrast: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetContrast.Value.Invoke(source, &contrast)

    let DTWAIN_GetContrastString (source: DTWAIN_SOURCE) (contrast: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetContrastString.Value.Invoke(source, contrast)

    let DTWAIN_GetContrastStringA (source: DTWAIN_SOURCE) (contrast: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetContrastStringA.Value.Invoke(source, contrast)

    let DTWAIN_GetContrastStringW (source: DTWAIN_SOURCE) (contrast: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetContrastStringW.Value.Invoke(source, contrast)

    let DTWAIN_GetCountry() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCountry.Value.Invoke()

    let DTWAIN_GetCurrentAcquiredImage (source: DTWAIN_SOURCE) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCurrentAcquiredImage.Value.Invoke(source)

    let DTWAIN_GetCurrentFileName (source: DTWAIN_SOURCE) (szname: System.Text.StringBuilder) (maxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCurrentFileName.Value.Invoke(source, szname, maxlen)

    let DTWAIN_GetCurrentFileNameA (source: DTWAIN_SOURCE) (szname: System.Text.StringBuilder) (maxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCurrentFileNameA.Value.Invoke(source, szname, maxlen)

    let DTWAIN_GetCurrentFileNameW (source: DTWAIN_SOURCE) (szname: System.Text.StringBuilder) (maxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCurrentFileNameW.Value.Invoke(source, szname, maxlen)

    let DTWAIN_GetCurrentPageNum (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCurrentPageNum.Value.Invoke(source)

    let DTWAIN_GetCurrentRetryCount (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCurrentRetryCount.Value.Invoke(source)

    let DTWAIN_GetCurrentTwainTriplet (pappid: TW_IDENTITY byref) (psourceid: TW_IDENTITY byref) (lpdg: int byref) (lpdat: int byref) (lpmsg: int byref) (lpmemref: Int64 byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCurrentTwainTriplet.Value.Invoke(&pappid, &psourceid, &lpdg, &lpdat, &lpmsg, &lpmemref)

    let DTWAIN_GetCustomDSData (source: DTWAIN_SOURCE) (data: byte[]) (dsize: DWORD) (pactualsize: DWORD byref) (nflags: LONG) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetCustomDSData.Value.Invoke(source, data, dsize, &pactualsize, nflags)

    let DTWAIN_GetDSMFullName (dsmtype: LONG) (szdllname: System.Text.StringBuilder) (nmaxlen: LONG) (pwhichsearch: int byref) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDSMFullName.Value.Invoke(dsmtype, szdllname, nmaxlen, &pwhichsearch)

    let DTWAIN_GetDSMFullNameA (dsmtype: LONG) (szdllname: System.Text.StringBuilder) (nmaxlen: LONG) (pwhichsearch: int byref) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDSMFullNameA.Value.Invoke(dsmtype, szdllname, nmaxlen, &pwhichsearch)

    let DTWAIN_GetDSMFullNameW (dsmtype: LONG) (szdllname: System.Text.StringBuilder) (nmaxlen: LONG) (pwhichsearch: int byref) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDSMFullNameW.Value.Invoke(dsmtype, szdllname, nmaxlen, &pwhichsearch)

    let DTWAIN_GetDSMSearchOrder() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDSMSearchOrder.Value.Invoke()

    let DTWAIN_GetDTWAINHandle() : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDTWAINHandle.Value.Invoke()

    let DTWAIN_GetDeviceEvent (source: DTWAIN_SOURCE) (lpevent: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDeviceEvent.Value.Invoke(source, &lpevent)

    let DTWAIN_GetDeviceEventEx (source: DTWAIN_SOURCE) (lpevent: int byref) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDeviceEventEx.Value.Invoke(source, &lpevent, &parray)

    let DTWAIN_GetDeviceEventInfo (source: DTWAIN_SOURCE) (nwhichinfo: LONG) (pvalue: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDeviceEventInfo.Value.Invoke(source, nwhichinfo, pvalue)

    let DTWAIN_GetDeviceNotifications (source: DTWAIN_SOURCE) (devevents: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDeviceNotifications.Value.Invoke(source, &devevents)

    let DTWAIN_GetDeviceTimeDate (source: DTWAIN_SOURCE) (sztimedate: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDeviceTimeDate.Value.Invoke(source, sztimedate)

    let DTWAIN_GetDeviceTimeDateA (source: DTWAIN_SOURCE) (sztimedate: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDeviceTimeDateA.Value.Invoke(source, sztimedate)

    let DTWAIN_GetDeviceTimeDateW (source: DTWAIN_SOURCE) (sztimedate: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDeviceTimeDateW.Value.Invoke(source, sztimedate)

    let DTWAIN_GetDoubleFeedDetectLength (source: DTWAIN_SOURCE) (value: DTWAIN_FLOAT byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDoubleFeedDetectLength.Value.Invoke(source, &value, bcurrent)

    let DTWAIN_GetDoubleFeedDetectValues (source: DTWAIN_SOURCE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDoubleFeedDetectValues.Value.Invoke(source, &parray)

    let DTWAIN_GetDuplexType (source: DTWAIN_SOURCE) (lpduptype: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetDuplexType.Value.Invoke(source, &lpduptype)

    let DTWAIN_GetErrorBuffer (arraybuffer: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetErrorBuffer.Value.Invoke(&arraybuffer)

    let DTWAIN_GetErrorBufferThreshold() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetErrorBufferThreshold.Value.Invoke()

    let DTWAIN_GetErrorCallback() : DTWAIN_ERROR_PROC =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetErrorCallback.Value.Invoke()

    let DTWAIN_GetErrorCallback64() : DTWAIN_ERROR_PROC64 =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetErrorCallback64.Value.Invoke()

    let DTWAIN_GetErrorString (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetErrorString.Value.Invoke(lerror, lpszbuffer, nmaxlen)

    let DTWAIN_GetErrorStringA (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetErrorStringA.Value.Invoke(lerror, lpszbuffer, nlength)

    let DTWAIN_GetErrorStringW (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetErrorStringW.Value.Invoke(lerror, lpszbuffer, nlength)

    let DTWAIN_GetExtCapFromName (szname: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtCapFromName.Value.Invoke(szname)

    let DTWAIN_GetExtCapFromNameA (szname: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtCapFromNameA.Value.Invoke(szname)

    let DTWAIN_GetExtCapFromNameW (szname: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtCapFromNameW.Value.Invoke(szname)

    let DTWAIN_GetExtImageInfo (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtImageInfo.Value.Invoke(source)

    let DTWAIN_GetExtImageInfoData (source: DTWAIN_SOURCE) (nwhich: LONG) (data: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtImageInfoData.Value.Invoke(source, nwhich, &data)

    let DTWAIN_GetExtImageInfoDataEx (source: DTWAIN_SOURCE) (nwhich: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtImageInfoDataEx.Value.Invoke(source, nwhich)

    let DTWAIN_GetExtImageInfoItem (source: DTWAIN_SOURCE) (nwhich: LONG) (infoid: int byref) (numitems: int byref) (type1: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtImageInfoItem.Value.Invoke(source, nwhich, &infoid, &numitems, &type1)

    let DTWAIN_GetExtImageInfoItemEx (source: DTWAIN_SOURCE) (nwhich: LONG) (infoid: int byref) (numitems: int byref) (type1: int byref) (returncode: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtImageInfoItemEx.Value.Invoke(source, nwhich, &infoid, &numitems, &type1, &returncode)

    let DTWAIN_GetExtNameFromCap (nvalue: LONG) (szvalue: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtNameFromCap.Value.Invoke(nvalue, szvalue, nmaxlen)

    let DTWAIN_GetExtNameFromCapA (nvalue: LONG) (szvalue: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtNameFromCapA.Value.Invoke(nvalue, szvalue, nlength)

    let DTWAIN_GetExtNameFromCapW (nvalue: LONG) (szvalue: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetExtNameFromCapW.Value.Invoke(nvalue, szvalue, nlength)

    let DTWAIN_GetFeederAlignment (source: DTWAIN_SOURCE) (lpalignment: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFeederAlignment.Value.Invoke(source, &lpalignment)

    let DTWAIN_GetFeederFuncs (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFeederFuncs.Value.Invoke(source)

    let DTWAIN_GetFeederOrder (source: DTWAIN_SOURCE) (lporder: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFeederOrder.Value.Invoke(source, &lporder)

    let DTWAIN_GetFeederWaitTime (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFeederWaitTime.Value.Invoke(source)

    let DTWAIN_GetFileCompressionType (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFileCompressionType.Value.Invoke(source)

    let DTWAIN_GetFileTypeExtensions (ntype: LONG) (lpszname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFileTypeExtensions.Value.Invoke(ntype, lpszname, nlength)

    let DTWAIN_GetFileTypeExtensionsA (ntype: LONG) (lpszname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFileTypeExtensionsA.Value.Invoke(ntype, lpszname, nlength)

    let DTWAIN_GetFileTypeExtensionsW (ntype: LONG) (lpszname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFileTypeExtensionsW.Value.Invoke(ntype, lpszname, nlength)

    let DTWAIN_GetFileTypeName (ntype: LONG) (lpszname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFileTypeName.Value.Invoke(ntype, lpszname, nlength)

    let DTWAIN_GetFileTypeNameA (ntype: LONG) (lpszname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFileTypeNameA.Value.Invoke(ntype, lpszname, nlength)

    let DTWAIN_GetFileTypeNameW (ntype: LONG) (lpszname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetFileTypeNameW.Value.Invoke(ntype, lpszname, nlength)

    let DTWAIN_GetHalftone (source: DTWAIN_SOURCE) (lphalftone: System.Text.StringBuilder) (gettype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetHalftone.Value.Invoke(source, lphalftone, gettype)

    let DTWAIN_GetHalftoneA (source: DTWAIN_SOURCE) (lphalftone: System.Text.StringBuilder) (gettype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetHalftoneA.Value.Invoke(source, lphalftone, gettype)

    let DTWAIN_GetHalftoneW (source: DTWAIN_SOURCE) (lphalftone: System.Text.StringBuilder) (gettype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetHalftoneW.Value.Invoke(source, lphalftone, gettype)

    let DTWAIN_GetHighlight (source: DTWAIN_SOURCE) (highlight: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetHighlight.Value.Invoke(source, &highlight)

    let DTWAIN_GetHighlightString (source: DTWAIN_SOURCE) (highlight: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetHighlightString.Value.Invoke(source, highlight)

    let DTWAIN_GetHighlightStringA (source: DTWAIN_SOURCE) (highlight: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetHighlightStringA.Value.Invoke(source, highlight)

    let DTWAIN_GetHighlightStringW (source: DTWAIN_SOURCE) (highlight: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetHighlightStringW.Value.Invoke(source, highlight)

    let DTWAIN_GetImageInfo (source: DTWAIN_SOURCE) (lpxresolution: DTWAIN_FLOAT byref) (lpyresolution: DTWAIN_FLOAT byref) (lpwidth: int byref) (lplength: int byref) (lpnumsamples: int byref) (lpbitspersample: DTWAIN_ARRAY byref) (lpbitsperpixel: int byref) (lpplanar: int byref) (lppixeltype: int byref) (lpcompression: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetImageInfo.Value.Invoke(source, &lpxresolution, &lpyresolution, &lpwidth, &lplength, &lpnumsamples, &lpbitspersample, &lpbitsperpixel, &lpplanar, &lppixeltype, &lpcompression)

    let DTWAIN_GetImageInfoString (source: DTWAIN_SOURCE) (lpxresolution: System.Text.StringBuilder) (lpyresolution: System.Text.StringBuilder) (lpwidth: int byref) (lplength: int byref) (lpnumsamples: int byref) (lpbitspersample: DTWAIN_ARRAY byref) (lpbitsperpixel: int byref) (lpplanar: int byref) (lppixeltype: int byref) (lpcompression: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetImageInfoString.Value.Invoke(source, lpxresolution, lpyresolution, &lpwidth, &lplength, &lpnumsamples, &lpbitspersample, &lpbitsperpixel, &lpplanar, &lppixeltype, &lpcompression)

    let DTWAIN_GetImageInfoStringA (source: DTWAIN_SOURCE) (lpxresolution: System.Text.StringBuilder) (lpyresolution: System.Text.StringBuilder) (lpwidth: int byref) (lplength: int byref) (lpnumsamples: int byref) (lpbitspersample: DTWAIN_ARRAY byref) (lpbitsperpixel: int byref) (lpplanar: int byref) (lppixeltype: int byref) (lpcompression: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetImageInfoStringA.Value.Invoke(source, lpxresolution, lpyresolution, &lpwidth, &lplength, &lpnumsamples, &lpbitspersample, &lpbitsperpixel, &lpplanar, &lppixeltype, &lpcompression)

    let DTWAIN_GetImageInfoStringW (source: DTWAIN_SOURCE) (lpxresolution: System.Text.StringBuilder) (lpyresolution: System.Text.StringBuilder) (lpwidth: int byref) (lplength: int byref) (lpnumsamples: int byref) (lpbitspersample: DTWAIN_ARRAY byref) (lpbitsperpixel: int byref) (lpplanar: int byref) (lppixeltype: int byref) (lpcompression: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetImageInfoStringW.Value.Invoke(source, lpxresolution, lpyresolution, &lpwidth, &lplength, &lpnumsamples, &lpbitspersample, &lpbitsperpixel, &lpplanar, &lppixeltype, &lpcompression)

    let DTWAIN_GetJobControl (source: DTWAIN_SOURCE) (pjobcontrol: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetJobControl.Value.Invoke(source, &pjobcontrol, bcurrent)

    let DTWAIN_GetJpegValues (source: DTWAIN_SOURCE) (pquality: int byref) (progressive: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetJpegValues.Value.Invoke(source, &pquality, &progressive)

    let DTWAIN_GetJpegXRValues (source: DTWAIN_SOURCE) (pquality: int byref) (progressive: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetJpegXRValues.Value.Invoke(source, &pquality, &progressive)

    let DTWAIN_GetLanguage() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLanguage.Value.Invoke()

    let DTWAIN_GetLastError() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLastError.Value.Invoke()

    let DTWAIN_GetLibraryPath (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLibraryPath.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetLibraryPathA (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLibraryPathA.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetLibraryPathW (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLibraryPathW.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetLightPath (source: DTWAIN_SOURCE) (lplightpath: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLightPath.Value.Invoke(source, &lplightpath)

    let DTWAIN_GetLightSource (source: DTWAIN_SOURCE) (lightsource: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLightSource.Value.Invoke(source, &lightsource)

    let DTWAIN_GetLightSources (source: DTWAIN_SOURCE) (lightsources: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLightSources.Value.Invoke(source, &lightsources)

    let DTWAIN_GetLoggerCallback() : DTWAIN_LOGGER_PROC =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLoggerCallback.Value.Invoke()

    let DTWAIN_GetLoggerCallbackA() : DTWAIN_LOGGER_PROCA =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLoggerCallbackA.Value.Invoke()

    let DTWAIN_GetLoggerCallbackW() : DTWAIN_LOGGER_PROCW =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetLoggerCallbackW.Value.Invoke()

    let DTWAIN_GetManualDuplexCount (source: DTWAIN_SOURCE) (pside1: int byref) (pside2: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetManualDuplexCount.Value.Invoke(source, &pside1, &pside2)

    let DTWAIN_GetMaxAcquisitions (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetMaxAcquisitions.Value.Invoke(source)

    let DTWAIN_GetMaxBuffers (source: DTWAIN_SOURCE) (pmaxbuf: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetMaxBuffers.Value.Invoke(source, &pmaxbuf)

    let DTWAIN_GetMaxPagesToAcquire (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetMaxPagesToAcquire.Value.Invoke(source)

    let DTWAIN_GetMaxRetryAttempts (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetMaxRetryAttempts.Value.Invoke(source)

    let DTWAIN_GetNameFromCap (ncapvalue: LONG) (szvalue: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetNameFromCap.Value.Invoke(ncapvalue, szvalue, nmaxlen)

    let DTWAIN_GetNameFromCapA (ncapvalue: LONG) (szvalue: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetNameFromCapA.Value.Invoke(ncapvalue, szvalue, nlength)

    let DTWAIN_GetNameFromCapW (ncapvalue: LONG) (szvalue: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetNameFromCapW.Value.Invoke(ncapvalue, szvalue, nlength)

    let DTWAIN_GetNoiseFilter (source: DTWAIN_SOURCE) (lpnoisefilter: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetNoiseFilter.Value.Invoke(source, &lpnoisefilter)

    let DTWAIN_GetNumAcquiredImages (aacq: DTWAIN_ARRAY) (nwhich: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetNumAcquiredImages.Value.Invoke(aacq, nwhich)

    let DTWAIN_GetNumAcquisitions (aacq: DTWAIN_ARRAY) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetNumAcquisitions.Value.Invoke(aacq)

    let DTWAIN_GetOCRCapValues (engine: DTWAIN_OCRENGINE) (ocrcapvalue: LONG) (gettype: LONG) (capvalues: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRCapValues.Value.Invoke(engine, ocrcapvalue, gettype, &capvalues)

    let DTWAIN_GetOCRErrorString (engine: DTWAIN_OCRENGINE) (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRErrorString.Value.Invoke(engine, lerror, lpszbuffer, nmaxlen)

    let DTWAIN_GetOCRErrorStringA (engine: DTWAIN_OCRENGINE) (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRErrorStringA.Value.Invoke(engine, lerror, lpszbuffer, nmaxlen)

    let DTWAIN_GetOCRErrorStringW (engine: DTWAIN_OCRENGINE) (lerror: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRErrorStringW.Value.Invoke(engine, lerror, lpszbuffer, nmaxlen)

    let DTWAIN_GetOCRLastError (engine: DTWAIN_OCRENGINE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRLastError.Value.Invoke(engine)

    let DTWAIN_GetOCRMajorMinorVersion (engine: DTWAIN_OCRENGINE) (lpmajor: int byref) (lpminor: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRMajorMinorVersion.Value.Invoke(engine, &lpmajor, &lpminor)

    let DTWAIN_GetOCRManufacturer (engine: DTWAIN_OCRENGINE) (szmanufacturer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRManufacturer.Value.Invoke(engine, szmanufacturer, nmaxlen)

    let DTWAIN_GetOCRManufacturerA (engine: DTWAIN_OCRENGINE) (szmanufacturer: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRManufacturerA.Value.Invoke(engine, szmanufacturer, nlength)

    let DTWAIN_GetOCRManufacturerW (engine: DTWAIN_OCRENGINE) (szmanufacturer: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRManufacturerW.Value.Invoke(engine, szmanufacturer, nlength)

    let DTWAIN_GetOCRProductFamily (engine: DTWAIN_OCRENGINE) (szproductfamily: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRProductFamily.Value.Invoke(engine, szproductfamily, nmaxlen)

    let DTWAIN_GetOCRProductFamilyA (engine: DTWAIN_OCRENGINE) (szproductfamily: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRProductFamilyA.Value.Invoke(engine, szproductfamily, nlength)

    let DTWAIN_GetOCRProductFamilyW (engine: DTWAIN_OCRENGINE) (szproductfamily: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRProductFamilyW.Value.Invoke(engine, szproductfamily, nlength)

    let DTWAIN_GetOCRProductName (engine: DTWAIN_OCRENGINE) (szproductname: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRProductName.Value.Invoke(engine, szproductname, nmaxlen)

    let DTWAIN_GetOCRProductNameA (engine: DTWAIN_OCRENGINE) (szproductname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRProductNameA.Value.Invoke(engine, szproductname, nlength)

    let DTWAIN_GetOCRProductNameW (engine: DTWAIN_OCRENGINE) (szproductname: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRProductNameW.Value.Invoke(engine, szproductname, nlength)

    let DTWAIN_GetOCRText (engine: DTWAIN_OCRENGINE) (npageno: LONG) (data: System.Text.StringBuilder) (dsize: LONG) (pactualsize: int byref) (nflags: LONG) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRText.Value.Invoke(engine, npageno, data, dsize, &pactualsize, nflags)

    let DTWAIN_GetOCRTextA (engine: DTWAIN_OCRENGINE) (npageno: LONG) (data: System.Text.StringBuilder) (dsize: LONG) (pactualsize: int byref) (nflags: LONG) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRTextA.Value.Invoke(engine, npageno, data, dsize, &pactualsize, nflags)

    let DTWAIN_GetOCRTextInfoFloat (ocrtextinfo: DTWAIN_OCRTEXTINFOHANDLE) (ncharpos: LONG) (nwhichitem: LONG) (pinfo: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRTextInfoFloat.Value.Invoke(ocrtextinfo, ncharpos, nwhichitem, &pinfo)

    let DTWAIN_GetOCRTextInfoFloatEx (ocrtextinfo: DTWAIN_OCRTEXTINFOHANDLE) (nwhichitem: LONG) (pinfo: DTWAIN_FLOAT byref) (bufsize: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRTextInfoFloatEx.Value.Invoke(ocrtextinfo, nwhichitem, &pinfo, bufsize)

    let DTWAIN_GetOCRTextInfoHandle (engine: DTWAIN_OCRENGINE) (npageno: LONG) : DTWAIN_OCRTEXTINFOHANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRTextInfoHandle.Value.Invoke(engine, npageno)

    let DTWAIN_GetOCRTextInfoLong (ocrtextinfo: DTWAIN_OCRTEXTINFOHANDLE) (ncharpos: LONG) (nwhichitem: LONG) (pinfo: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRTextInfoLong.Value.Invoke(ocrtextinfo, ncharpos, nwhichitem, &pinfo)

    let DTWAIN_GetOCRTextInfoLongEx (ocrtextinfo: DTWAIN_OCRTEXTINFOHANDLE) (nwhichitem: LONG) (pinfo: int byref) (bufsize: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRTextInfoLongEx.Value.Invoke(ocrtextinfo, nwhichitem, &pinfo, bufsize)

    let DTWAIN_GetOCRTextW (engine: DTWAIN_OCRENGINE) (npageno: LONG) (data: System.Text.StringBuilder) (dsize: LONG) (pactualsize: int byref) (nflags: LONG) : HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRTextW.Value.Invoke(engine, npageno, data, dsize, &pactualsize, nflags)

    let DTWAIN_GetOCRVersionInfo (engine: DTWAIN_OCRENGINE) (buffer: System.Text.StringBuilder) (maxbufsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRVersionInfo.Value.Invoke(engine, buffer, maxbufsize)

    let DTWAIN_GetOCRVersionInfoA (engine: DTWAIN_OCRENGINE) (buffer: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRVersionInfoA.Value.Invoke(engine, buffer, nlength)

    let DTWAIN_GetOCRVersionInfoW (engine: DTWAIN_OCRENGINE) (buffer: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOCRVersionInfoW.Value.Invoke(engine, buffer, nlength)

    let DTWAIN_GetOrientation (source: DTWAIN_SOURCE) (lporient: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOrientation.Value.Invoke(source, &lporient, bcurrent)

    let DTWAIN_GetOverscan (source: DTWAIN_SOURCE) (lpoverscan: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetOverscan.Value.Invoke(source, &lpoverscan, bcurrent)

    let DTWAIN_GetPDFTextElementFloat (textelement: DTWAIN_PDFTEXTELEMENT) (val1: DTWAIN_FLOAT byref) (val2: DTWAIN_FLOAT byref) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFTextElementFloat.Value.Invoke(textelement, &val1, &val2, flags)

    let DTWAIN_GetPDFTextElementLong (textelement: DTWAIN_PDFTEXTELEMENT) (val1: int byref) (val2: int byref) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFTextElementLong.Value.Invoke(textelement, &val1, &val2, flags)

    let DTWAIN_GetPDFTextElementString (textelement: DTWAIN_PDFTEXTELEMENT) (szdata: System.Text.StringBuilder) (maxlen: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFTextElementString.Value.Invoke(textelement, szdata, maxlen, flags)

    let DTWAIN_GetPDFTextElementStringA (textelement: DTWAIN_PDFTEXTELEMENT) (szdata: System.Text.StringBuilder) (maxlen: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFTextElementStringA.Value.Invoke(textelement, szdata, maxlen, flags)

    let DTWAIN_GetPDFTextElementStringW (textelement: DTWAIN_PDFTEXTELEMENT) (szdata: System.Text.StringBuilder) (maxlen: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFTextElementStringW.Value.Invoke(textelement, szdata, maxlen, flags)

    let DTWAIN_GetPDFType1FontName (fontval: LONG) (szfont: System.Text.StringBuilder) (nchars: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFType1FontName.Value.Invoke(fontval, szfont, nchars)

    let DTWAIN_GetPDFType1FontNameA (fontval: LONG) (szfont: System.Text.StringBuilder) (nchars: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFType1FontNameA.Value.Invoke(fontval, szfont, nchars)

    let DTWAIN_GetPDFType1FontNameW (fontval: LONG) (szfont: System.Text.StringBuilder) (nchars: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPDFType1FontNameW.Value.Invoke(fontval, szfont, nchars)

    let DTWAIN_GetPaperSize (source: DTWAIN_SOURCE) (lppapersize: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPaperSize.Value.Invoke(source, &lppapersize, bcurrent)

    let DTWAIN_GetPaperSizeName (papernumber: LONG) (outname: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPaperSizeName.Value.Invoke(papernumber, outname, nsize)

    let DTWAIN_GetPaperSizeNameA (papernumber: LONG) (outname: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPaperSizeNameA.Value.Invoke(papernumber, outname, nsize)

    let DTWAIN_GetPaperSizeNameW (papernumber: LONG) (outname: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPaperSizeNameW.Value.Invoke(papernumber, outname, nsize)

    let DTWAIN_GetPatchMaxPriorities (source: DTWAIN_SOURCE) (pmaxpriorities: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPatchMaxPriorities.Value.Invoke(source, &pmaxpriorities, bcurrent)

    let DTWAIN_GetPatchMaxRetries (source: DTWAIN_SOURCE) (pmaxretries: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPatchMaxRetries.Value.Invoke(source, &pmaxretries, bcurrent)

    let DTWAIN_GetPatchPriorities (source: DTWAIN_SOURCE) (searchpriorities: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPatchPriorities.Value.Invoke(source, &searchpriorities)

    let DTWAIN_GetPatchSearchMode (source: DTWAIN_SOURCE) (psearchmode: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPatchSearchMode.Value.Invoke(source, &psearchmode, bcurrent)

    let DTWAIN_GetPatchTimeOut (source: DTWAIN_SOURCE) (ptimeout: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPatchTimeOut.Value.Invoke(source, &ptimeout, bcurrent)

    let DTWAIN_GetPixelFlavor (source: DTWAIN_SOURCE) (lppixelflavor: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPixelFlavor.Value.Invoke(source, &lppixelflavor)

    let DTWAIN_GetPixelType (source: DTWAIN_SOURCE) (pixeltype: int byref) (bitdepth: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPixelType.Value.Invoke(source, &pixeltype, &bitdepth, bcurrent)

    let DTWAIN_GetPrinter (source: DTWAIN_SOURCE) (lpprinter: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPrinter.Value.Invoke(source, &lpprinter, bcurrent)

    let DTWAIN_GetPrinterStartNumber (source: DTWAIN_SOURCE) (nstart: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPrinterStartNumber.Value.Invoke(source, &nstart)

    let DTWAIN_GetPrinterStringMode (source: DTWAIN_SOURCE) (printermode: int byref) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPrinterStringMode.Value.Invoke(source, &printermode, bcurrent)

    let DTWAIN_GetPrinterStrings (source: DTWAIN_SOURCE) (arraystring: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPrinterStrings.Value.Invoke(source, &arraystring)

    let DTWAIN_GetPrinterSuffixString (source: DTWAIN_SOURCE) (suffix: System.Text.StringBuilder) (nmaxlen: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPrinterSuffixString.Value.Invoke(source, suffix, nmaxlen)

    let DTWAIN_GetPrinterSuffixStringA (source: DTWAIN_SOURCE) (suffix: System.Text.StringBuilder) (nlength: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPrinterSuffixStringA.Value.Invoke(source, suffix, nlength)

    let DTWAIN_GetPrinterSuffixStringW (source: DTWAIN_SOURCE) (suffix: System.Text.StringBuilder) (nlength: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetPrinterSuffixStringW.Value.Invoke(source, suffix, nlength)

    let DTWAIN_GetRegisteredMsg() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetRegisteredMsg.Value.Invoke()

    let DTWAIN_GetResolution (source: DTWAIN_SOURCE) (resolution: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetResolution.Value.Invoke(source, &resolution)

    let DTWAIN_GetResolutionString (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetResolutionString.Value.Invoke(source, resolution)

    let DTWAIN_GetResolutionStringA (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetResolutionStringA.Value.Invoke(source, resolution)

    let DTWAIN_GetResolutionStringW (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetResolutionStringW.Value.Invoke(source, resolution)

    let DTWAIN_GetResourceString (resourceid: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetResourceString.Value.Invoke(resourceid, lpszbuffer, nmaxlen)

    let DTWAIN_GetResourceStringA (resourceid: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetResourceStringA.Value.Invoke(resourceid, lpszbuffer, nmaxlen)

    let DTWAIN_GetResourceStringW (resourceid: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetResourceStringW.Value.Invoke(resourceid, lpszbuffer, nmaxlen)

    let DTWAIN_GetRotation (source: DTWAIN_SOURCE) (rotation: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetRotation.Value.Invoke(source, &rotation)

    let DTWAIN_GetRotationString (source: DTWAIN_SOURCE) (rotation: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetRotationString.Value.Invoke(source, rotation)

    let DTWAIN_GetRotationStringA (source: DTWAIN_SOURCE) (rotation: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetRotationStringA.Value.Invoke(source, rotation)

    let DTWAIN_GetRotationStringW (source: DTWAIN_SOURCE) (rotation: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetRotationStringW.Value.Invoke(source, rotation)

    let DTWAIN_GetSaveFileName (source: DTWAIN_SOURCE) (fname: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSaveFileName.Value.Invoke(source, fname, nmaxlen)

    let DTWAIN_GetSaveFileNameA (source: DTWAIN_SOURCE) (fname: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSaveFileNameA.Value.Invoke(source, fname, nmaxlen)

    let DTWAIN_GetSaveFileNameW (source: DTWAIN_SOURCE) (fname: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSaveFileNameW.Value.Invoke(source, fname, nmaxlen)

    let DTWAIN_GetSavedFilesCount (source: DTWAIN_SOURCE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSavedFilesCount.Value.Invoke(source)

    let DTWAIN_GetSessionDetails (szbuf: System.Text.StringBuilder) (nsize: LONG) (indentfactor: LONG) (brefresh: BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSessionDetails.Value.Invoke(szbuf, nsize, indentfactor, brefresh)

    let DTWAIN_GetSessionDetailsA (szbuf: System.Text.StringBuilder) (nsize: LONG) (indentfactor: LONG) (brefresh: BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSessionDetailsA.Value.Invoke(szbuf, nsize, indentfactor, brefresh)

    let DTWAIN_GetSessionDetailsW (szbuf: System.Text.StringBuilder) (nsize: LONG) (indentfactor: LONG) (brefresh: BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSessionDetailsW.Value.Invoke(szbuf, nsize, indentfactor, brefresh)

    let DTWAIN_GetShadow (source: DTWAIN_SOURCE) (shadow: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetShadow.Value.Invoke(source, &shadow)

    let DTWAIN_GetShadowString (source: DTWAIN_SOURCE) (shadow: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetShadowString.Value.Invoke(source, shadow)

    let DTWAIN_GetShadowStringA (source: DTWAIN_SOURCE) (shadow: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetShadowStringA.Value.Invoke(source, shadow)

    let DTWAIN_GetShadowStringW (source: DTWAIN_SOURCE) (shadow: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetShadowStringW.Value.Invoke(source, shadow)

    let DTWAIN_GetShortVersionString (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetShortVersionString.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetShortVersionStringA (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetShortVersionStringA.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetShortVersionStringW (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetShortVersionStringW.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetSourceAcquisitions (source: DTWAIN_SOURCE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceAcquisitions.Value.Invoke(source)

    let DTWAIN_GetSourceDetails (szsources: string) (szbuf: System.Text.StringBuilder) (nsize: LONG) (indentfactor: LONG) (brefresh: BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceDetails.Value.Invoke(szsources, szbuf, nsize, indentfactor, brefresh)

    let DTWAIN_GetSourceDetailsA (szsources: string) (szbuf: System.Text.StringBuilder) (nsize: LONG) (indentfactor: LONG) (brefresh: BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceDetailsA.Value.Invoke(szsources, szbuf, nsize, indentfactor, brefresh)

    let DTWAIN_GetSourceDetailsW (szsources: string) (szbuf: System.Text.StringBuilder) (nsize: LONG) (indentfactor: LONG) (brefresh: BOOL) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceDetailsW.Value.Invoke(szsources, szbuf, nsize, indentfactor, brefresh)

    let DTWAIN_GetSourceID (source: DTWAIN_SOURCE) : DTWAIN_IDENTITY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceID.Value.Invoke(source)

    let DTWAIN_GetSourceIDEx (source: DTWAIN_SOURCE) (pidentity: TW_IDENTITY byref) : TW_IDENTITY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceIDEx.Value.Invoke(source, &pidentity)

    let DTWAIN_GetSourceManufacturer (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceManufacturer.Value.Invoke(source, szproduct, nmaxlen)

    let DTWAIN_GetSourceManufacturerA (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceManufacturerA.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceManufacturerW (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceManufacturerW.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceProductFamily (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceProductFamily.Value.Invoke(source, szproduct, nmaxlen)

    let DTWAIN_GetSourceProductFamilyA (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceProductFamilyA.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceProductFamilyW (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceProductFamilyW.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceProductName (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceProductName.Value.Invoke(source, szproduct, nmaxlen)

    let DTWAIN_GetSourceProductNameA (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceProductNameA.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceProductNameW (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceProductNameW.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceUnit (source: DTWAIN_SOURCE) (lpunit: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceUnit.Value.Invoke(source, &lpunit)

    let DTWAIN_GetSourceVersionInfo (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceVersionInfo.Value.Invoke(source, szproduct, nmaxlen)

    let DTWAIN_GetSourceVersionInfoA (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceVersionInfoA.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceVersionInfoW (source: DTWAIN_SOURCE) (szproduct: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceVersionInfoW.Value.Invoke(source, szproduct, nlength)

    let DTWAIN_GetSourceVersionNumber (source: DTWAIN_SOURCE) (pmajor: int byref) (pminor: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetSourceVersionNumber.Value.Invoke(source, &pmajor, &pminor)

    let DTWAIN_GetStaticLibVersion() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetStaticLibVersion.Value.Invoke()

    let DTWAIN_GetTempFileDirectory (szfilepath: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTempFileDirectory.Value.Invoke(szfilepath, nmaxlen)

    let DTWAIN_GetTempFileDirectoryA (szfilepath: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTempFileDirectoryA.Value.Invoke(szfilepath, nlength)

    let DTWAIN_GetTempFileDirectoryW (szfilepath: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTempFileDirectoryW.Value.Invoke(szfilepath, nlength)

    let DTWAIN_GetThreshold (source: DTWAIN_SOURCE) (threshold: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetThreshold.Value.Invoke(source, &threshold)

    let DTWAIN_GetThresholdString (source: DTWAIN_SOURCE) (threshold: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetThresholdString.Value.Invoke(source, threshold)

    let DTWAIN_GetThresholdStringA (source: DTWAIN_SOURCE) (threshold: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetThresholdStringA.Value.Invoke(source, threshold)

    let DTWAIN_GetThresholdStringW (source: DTWAIN_SOURCE) (threshold: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetThresholdStringW.Value.Invoke(source, threshold)

    let DTWAIN_GetTimeDate (source: DTWAIN_SOURCE) (sztimedate: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTimeDate.Value.Invoke(source, sztimedate)

    let DTWAIN_GetTimeDateA (source: DTWAIN_SOURCE) (sztimedate: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTimeDateA.Value.Invoke(source, sztimedate)

    let DTWAIN_GetTimeDateW (source: DTWAIN_SOURCE) (sztimedate: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTimeDateW.Value.Invoke(source, sztimedate)

    let DTWAIN_GetTwainAppID() : DTWAIN_IDENTITY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainAppID.Value.Invoke()

    let DTWAIN_GetTwainAppIDEx (pidentity: TW_IDENTITY byref) : TW_IDENTITY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainAppIDEx.Value.Invoke(&pidentity)

    let DTWAIN_GetTwainAvailability() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainAvailability.Value.Invoke()

    let DTWAIN_GetTwainAvailabilityEx (directories: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainAvailabilityEx.Value.Invoke(directories, nmaxlen)

    let DTWAIN_GetTwainAvailabilityExA (szdirectories: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainAvailabilityExA.Value.Invoke(szdirectories, nlength)

    let DTWAIN_GetTwainAvailabilityExW (szdirectories: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainAvailabilityExW.Value.Invoke(szdirectories, nlength)

    let DTWAIN_GetTwainCountryName (countryid: LONG) (szname: System.Text.StringBuilder) : BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainCountryName.Value.Invoke(countryid, szname)

    let DTWAIN_GetTwainCountryNameA (countryid: LONG) (szname: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainCountryNameA.Value.Invoke(countryid, szname)

    let DTWAIN_GetTwainCountryNameW (countryid: LONG) (szname: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainCountryNameW.Value.Invoke(countryid, szname)

    let DTWAIN_GetTwainCountryValue (country: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainCountryValue.Value.Invoke(country)

    let DTWAIN_GetTwainCountryValueA (country: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainCountryValueA.Value.Invoke(country)

    let DTWAIN_GetTwainCountryValueW (country: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainCountryValueW.Value.Invoke(country)

    let DTWAIN_GetTwainHwnd() : HWND =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainHwnd.Value.Invoke()

    let DTWAIN_GetTwainIDFromName (lpszbuffer: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainIDFromName.Value.Invoke(lpszbuffer)

    let DTWAIN_GetTwainIDFromNameA (lpszbuffer: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainIDFromNameA.Value.Invoke(lpszbuffer)

    let DTWAIN_GetTwainIDFromNameW (lpszbuffer: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainIDFromNameW.Value.Invoke(lpszbuffer)

    let DTWAIN_GetTwainLanguageName (nameid: LONG) (szname: System.Text.StringBuilder) : BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainLanguageName.Value.Invoke(nameid, szname)

    let DTWAIN_GetTwainLanguageNameA (lang: LONG) (szname: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainLanguageNameA.Value.Invoke(lang, szname)

    let DTWAIN_GetTwainLanguageNameW (lang: LONG) (szname: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainLanguageNameW.Value.Invoke(lang, szname)

    let DTWAIN_GetTwainLanguageValue (szname: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainLanguageValue.Value.Invoke(szname)

    let DTWAIN_GetTwainLanguageValueA (lang: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainLanguageValueA.Value.Invoke(lang)

    let DTWAIN_GetTwainLanguageValueW (lang: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainLanguageValueW.Value.Invoke(lang)

    let DTWAIN_GetTwainMode() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainMode.Value.Invoke()

    let DTWAIN_GetTwainNameFromConstant (lconstanttype: LONG) (ltwainconstant: LONG) (lpszout: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainNameFromConstant.Value.Invoke(lconstanttype, ltwainconstant, lpszout, nsize)

    let DTWAIN_GetTwainNameFromConstantA (lconstanttype: LONG) (ltwainconstant: LONG) (lpszout: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainNameFromConstantA.Value.Invoke(lconstanttype, ltwainconstant, lpszout, nsize)

    let DTWAIN_GetTwainNameFromConstantW (lconstanttype: LONG) (ltwainconstant: LONG) (lpszout: System.Text.StringBuilder) (nsize: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainNameFromConstantW.Value.Invoke(lconstanttype, ltwainconstant, lpszout, nsize)

    let DTWAIN_GetTwainStringName (category: LONG) (twainid: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainStringName.Value.Invoke(category, twainid, lpszbuffer, nmaxlen)

    let DTWAIN_GetTwainStringNameA (category: LONG) (twainid: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainStringNameA.Value.Invoke(category, twainid, lpszbuffer, nmaxlen)

    let DTWAIN_GetTwainStringNameW (category: LONG) (twainid: LONG) (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainStringNameW.Value.Invoke(category, twainid, lpszbuffer, nmaxlen)

    let DTWAIN_GetTwainTimeout() : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetTwainTimeout.Value.Invoke()

    let DTWAIN_GetVersion (lpmajor: int byref) (lpminor: int byref) (lpversiontype: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersion.Value.Invoke(&lpmajor, &lpminor, &lpversiontype)

    let DTWAIN_GetVersionCopyright (lpszapp: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionCopyright.Value.Invoke(lpszapp, nlength)

    let DTWAIN_GetVersionCopyrightA (lpszapp: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionCopyrightA.Value.Invoke(lpszapp, nlength)

    let DTWAIN_GetVersionCopyrightW (lpszapp: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionCopyrightW.Value.Invoke(lpszapp, nlength)

    let DTWAIN_GetVersionEx (lmajor: int byref) (lminor: int byref) (lversiontype: int byref) (lpatchlevel: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionEx.Value.Invoke(&lmajor, &lminor, &lversiontype, &lpatchlevel)

    let DTWAIN_GetVersionInfo (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionInfo.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetVersionInfoA (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionInfoA.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetVersionInfoW (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionInfoW.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetVersionString (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionString.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetVersionStringA (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionStringA.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetVersionStringW (lpszver: System.Text.StringBuilder) (nlength: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetVersionStringW.Value.Invoke(lpszver, nlength)

    let DTWAIN_GetWindowsVersionInfo (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetWindowsVersionInfo.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetWindowsVersionInfoA (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetWindowsVersionInfoA.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetWindowsVersionInfoW (lpszbuffer: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetWindowsVersionInfoW.Value.Invoke(lpszbuffer, nmaxlen)

    let DTWAIN_GetXResolution (source: DTWAIN_SOURCE) (resolution: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetXResolution.Value.Invoke(source, &resolution)

    let DTWAIN_GetXResolutionString (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetXResolutionString.Value.Invoke(source, resolution)

    let DTWAIN_GetXResolutionStringA (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetXResolutionStringA.Value.Invoke(source, resolution)

    let DTWAIN_GetXResolutionStringW (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetXResolutionStringW.Value.Invoke(source, resolution)

    let DTWAIN_GetYResolution (source: DTWAIN_SOURCE) (resolution: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetYResolution.Value.Invoke(source, &resolution)

    let DTWAIN_GetYResolutionString (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetYResolutionString.Value.Invoke(source, resolution)

    let DTWAIN_GetYResolutionStringA (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetYResolutionStringA.Value.Invoke(source, resolution)

    let DTWAIN_GetYResolutionStringW (source: DTWAIN_SOURCE) (resolution: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        GetYResolutionStringW.Value.Invoke(source, resolution)

    let DTWAIN_InitExtImageInfo (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        InitExtImageInfo.Value.Invoke(source)

    let DTWAIN_InitImageFileAppend (szfile: string) (ftype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        InitImageFileAppend.Value.Invoke(szfile, ftype)

    let DTWAIN_InitImageFileAppendA (szfile: string) (ftype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        InitImageFileAppendA.Value.Invoke(szfile, ftype)

    let DTWAIN_InitImageFileAppendW (szfile: string) (ftype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        InitImageFileAppendW.Value.Invoke(szfile, ftype)

    let DTWAIN_InitOCRInterface() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        InitOCRInterface.Value.Invoke()

    let DTWAIN_IsAcquiring() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAcquiring.Value.Invoke()

    let DTWAIN_IsAudioXferSupported (source: DTWAIN_SOURCE) (supportval: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAudioXferSupported.Value.Invoke(source, supportval)

    let DTWAIN_IsAutoBorderDetectEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoBorderDetectEnabled.Value.Invoke(source)

    let DTWAIN_IsAutoBorderDetectSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoBorderDetectSupported.Value.Invoke(source)

    let DTWAIN_IsAutoBrightEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoBrightEnabled.Value.Invoke(source)

    let DTWAIN_IsAutoBrightSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoBrightSupported.Value.Invoke(source)

    let DTWAIN_IsAutoDeskewEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoDeskewEnabled.Value.Invoke(source)

    let DTWAIN_IsAutoDeskewSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoDeskewSupported.Value.Invoke(source)

    let DTWAIN_IsAutoFeedEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoFeedEnabled.Value.Invoke(source)

    let DTWAIN_IsAutoFeedSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoFeedSupported.Value.Invoke(source)

    let DTWAIN_IsAutoRotateEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoRotateEnabled.Value.Invoke(source)

    let DTWAIN_IsAutoRotateSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoRotateSupported.Value.Invoke(source)

    let DTWAIN_IsAutoScanEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutoScanEnabled.Value.Invoke(source)

    let DTWAIN_IsAutomaticSenseMediumEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutomaticSenseMediumEnabled.Value.Invoke(source)

    let DTWAIN_IsAutomaticSenseMediumSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsAutomaticSenseMediumSupported.Value.Invoke(source)

    let DTWAIN_IsBlankPageDetectionOn (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsBlankPageDetectionOn.Value.Invoke(source)

    let DTWAIN_IsBufferedTileModeOn (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsBufferedTileModeOn.Value.Invoke(source)

    let DTWAIN_IsBufferedTileModeSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsBufferedTileModeSupported.Value.Invoke(source)

    let DTWAIN_IsCapSupported (source: DTWAIN_SOURCE) (lcapability: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsCapSupported.Value.Invoke(source, lcapability)

    let DTWAIN_IsCompressionSupported (source: DTWAIN_SOURCE) (compression: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsCompressionSupported.Value.Invoke(source, compression)

    let DTWAIN_IsCustomDSDataSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsCustomDSDataSupported.Value.Invoke(source)

    let DTWAIN_IsDIBBlank (hdib: HANDLE) (threshold: DTWAIN_FLOAT) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDIBBlank.Value.Invoke(hdib, threshold)

    let DTWAIN_IsDIBBlankString (hdib: HANDLE) (threshold: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDIBBlankString.Value.Invoke(hdib, threshold)

    let DTWAIN_IsDIBBlankStringA (hdib: HANDLE) (threshold: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDIBBlankStringA.Value.Invoke(hdib, threshold)

    let DTWAIN_IsDIBBlankStringW (hdib: HANDLE) (threshold: string) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDIBBlankStringW.Value.Invoke(hdib, threshold)

    let DTWAIN_IsDeviceEventSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDeviceEventSupported.Value.Invoke(source)

    let DTWAIN_IsDeviceOnLine (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDeviceOnLine.Value.Invoke(source)

    let DTWAIN_IsDoubleFeedDetectLengthSupported (source: DTWAIN_SOURCE) (value: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDoubleFeedDetectLengthSupported.Value.Invoke(source, value)

    let DTWAIN_IsDoubleFeedDetectSupported (source: DTWAIN_SOURCE) (supportval: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDoubleFeedDetectSupported.Value.Invoke(source, supportval)

    let DTWAIN_IsDoublePageCountOnDuplex (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDoublePageCountOnDuplex.Value.Invoke(source)

    let DTWAIN_IsDuplexEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDuplexEnabled.Value.Invoke(source)

    let DTWAIN_IsDuplexSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsDuplexSupported.Value.Invoke(source)

    let DTWAIN_IsExtImageInfoSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsExtImageInfoSupported.Value.Invoke(source)

    let DTWAIN_IsFeederEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsFeederEnabled.Value.Invoke(source)

    let DTWAIN_IsFeederLoaded (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsFeederLoaded.Value.Invoke(source)

    let DTWAIN_IsFeederSensitive (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsFeederSensitive.Value.Invoke(source)

    let DTWAIN_IsFeederSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsFeederSupported.Value.Invoke(source)

    let DTWAIN_IsFileSystemSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsFileSystemSupported.Value.Invoke(source)

    let DTWAIN_IsFileXferSupported (source: DTWAIN_SOURCE) (lfiletype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsFileXferSupported.Value.Invoke(source, lfiletype)

    let DTWAIN_IsIAFieldALastPageSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldALastPageSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldALevelSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldALevelSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldAPrintFormatSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldAPrintFormatSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldAValueSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldAValueSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldBLastPageSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldBLastPageSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldBLevelSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldBLevelSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldBPrintFormatSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldBPrintFormatSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldBValueSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldBValueSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldCLastPageSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldCLastPageSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldCLevelSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldCLevelSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldCPrintFormatSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldCPrintFormatSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldCValueSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldCValueSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldDLastPageSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldDLastPageSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldDLevelSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldDLevelSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldDPrintFormatSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldDPrintFormatSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldDValueSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldDValueSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldELastPageSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldELastPageSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldELevelSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldELevelSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldEPrintFormatSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldEPrintFormatSupported.Value.Invoke(source)

    let DTWAIN_IsIAFieldEValueSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIAFieldEValueSupported.Value.Invoke(source)

    let DTWAIN_IsImageAddressingSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsImageAddressingSupported.Value.Invoke(source)

    let DTWAIN_IsIndicatorEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIndicatorEnabled.Value.Invoke(source)

    let DTWAIN_IsIndicatorSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsIndicatorSupported.Value.Invoke(source)

    let DTWAIN_IsInitialized() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsInitialized.Value.Invoke()

    let DTWAIN_IsJPEGSupported() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsJPEGSupported.Value.Invoke()

    let DTWAIN_IsJobControlSupported (source: DTWAIN_SOURCE) (jobcontrol: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsJobControlSupported.Value.Invoke(source, jobcontrol)

    let DTWAIN_IsLampEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsLampEnabled.Value.Invoke(source)

    let DTWAIN_IsLampSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsLampSupported.Value.Invoke(source)

    let DTWAIN_IsLightPathSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsLightPathSupported.Value.Invoke(source)

    let DTWAIN_IsLightSourceSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsLightSourceSupported.Value.Invoke(source)

    let DTWAIN_IsMaxBuffersSupported (source: DTWAIN_SOURCE) (maxbuf: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsMaxBuffersSupported.Value.Invoke(source, maxbuf)

    let DTWAIN_IsMemFileXferSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsMemFileXferSupported.Value.Invoke(source)

    let DTWAIN_IsMsgNotifyEnabled() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsMsgNotifyEnabled.Value.Invoke()

    let DTWAIN_IsNotifyTripletsEnabled() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsNotifyTripletsEnabled.Value.Invoke()

    let DTWAIN_IsOCREngineActivated (ocrengine: DTWAIN_OCRENGINE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsOCREngineActivated.Value.Invoke(ocrengine)

    let DTWAIN_IsOpenSourcesOnSelect() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsOpenSourcesOnSelect.Value.Invoke()

    let DTWAIN_IsOrientationSupported (source: DTWAIN_SOURCE) (orientation: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsOrientationSupported.Value.Invoke(source, orientation)

    let DTWAIN_IsOverscanSupported (source: DTWAIN_SOURCE) (supportvalue: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsOverscanSupported.Value.Invoke(source, supportvalue)

    let DTWAIN_IsPDFSupported() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPDFSupported.Value.Invoke()

    let DTWAIN_IsPNGSupported() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPNGSupported.Value.Invoke()

    let DTWAIN_IsPaperDetectable (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPaperDetectable.Value.Invoke(source)

    let DTWAIN_IsPaperSizeSupported (source: DTWAIN_SOURCE) (papersize: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPaperSizeSupported.Value.Invoke(source, papersize)

    let DTWAIN_IsPatchCapsSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPatchCapsSupported.Value.Invoke(source)

    let DTWAIN_IsPatchDetectEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPatchDetectEnabled.Value.Invoke(source)

    let DTWAIN_IsPatchSupported (source: DTWAIN_SOURCE) (patchcode: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPatchSupported.Value.Invoke(source, patchcode)

    let DTWAIN_IsPeekMessageLoopEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPeekMessageLoopEnabled.Value.Invoke(source)

    let DTWAIN_IsPixelTypeSupported (source: DTWAIN_SOURCE) (pixeltype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPixelTypeSupported.Value.Invoke(source, pixeltype)

    let DTWAIN_IsPrinterEnabled (source: DTWAIN_SOURCE) (printer: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPrinterEnabled.Value.Invoke(source, printer)

    let DTWAIN_IsPrinterSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsPrinterSupported.Value.Invoke(source)

    let DTWAIN_IsRotationSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsRotationSupported.Value.Invoke(source)

    let DTWAIN_IsSessionEnabled() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSessionEnabled.Value.Invoke()

    let DTWAIN_IsSkipImageInfoError (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSkipImageInfoError.Value.Invoke(source)

    let DTWAIN_IsSourceAcquiring (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSourceAcquiring.Value.Invoke(source)

    let DTWAIN_IsSourceAcquiringEx (source: DTWAIN_SOURCE) (buionly: BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSourceAcquiringEx.Value.Invoke(source, buionly)

    let DTWAIN_IsSourceInUIOnlyMode (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSourceInUIOnlyMode.Value.Invoke(source)

    let DTWAIN_IsSourceOpen (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSourceOpen.Value.Invoke(source)

    let DTWAIN_IsSourceSelected (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSourceSelected.Value.Invoke(source)

    let DTWAIN_IsSourceValid (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsSourceValid.Value.Invoke(source)

    let DTWAIN_IsTIFFSupported() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsTIFFSupported.Value.Invoke()

    let DTWAIN_IsThumbnailEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsThumbnailEnabled.Value.Invoke(source)

    let DTWAIN_IsThumbnailSupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsThumbnailSupported.Value.Invoke(source)

    let DTWAIN_IsTwainAvailable() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsTwainAvailable.Value.Invoke()

    let DTWAIN_IsTwainAvailableEx (directories: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsTwainAvailableEx.Value.Invoke(directories, nmaxlen)

    let DTWAIN_IsTwainAvailableExA (directories: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsTwainAvailableExA.Value.Invoke(directories, nmaxlen)

    let DTWAIN_IsTwainAvailableExW (directories: System.Text.StringBuilder) (nmaxlen: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsTwainAvailableExW.Value.Invoke(directories, nmaxlen)

    let DTWAIN_IsTwainMsg (pmsg: MSG byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsTwainMsg.Value.Invoke(&pmsg)

    let DTWAIN_IsUIControllable (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsUIControllable.Value.Invoke(source)

    let DTWAIN_IsUIEnabled (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsUIEnabled.Value.Invoke(source)

    let DTWAIN_IsUIOnlySupported (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        IsUIOnlySupported.Value.Invoke(source)

    let DTWAIN_LoadCustomStringResources (slangdll: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LoadCustomStringResources.Value.Invoke(slangdll)

    let DTWAIN_LoadCustomStringResourcesA (slangdll: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LoadCustomStringResourcesA.Value.Invoke(slangdll)

    let DTWAIN_LoadCustomStringResourcesEx (slangdll: string) (bclear: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LoadCustomStringResourcesEx.Value.Invoke(slangdll, bclear)

    let DTWAIN_LoadCustomStringResourcesExA (slangdll: string) (bclear: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LoadCustomStringResourcesExA.Value.Invoke(slangdll, bclear)

    let DTWAIN_LoadCustomStringResourcesExW (slangdll: string) (bclear: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LoadCustomStringResourcesExW.Value.Invoke(slangdll, bclear)

    let DTWAIN_LoadCustomStringResourcesW (slangdll: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LoadCustomStringResourcesW.Value.Invoke(slangdll)

    let DTWAIN_LoadLanguageResource (nlanguage: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LoadLanguageResource.Value.Invoke(nlanguage)

    let DTWAIN_LockMemory (h: HANDLE) : DTWAIN_MEMORY_PTR =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LockMemory.Value.Invoke(h)

    let DTWAIN_LockMemoryEx (h: HANDLE) : DTWAIN_MEMORY_PTR =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LockMemoryEx.Value.Invoke(h)

    let DTWAIN_LogMessage (message: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LogMessage.Value.Invoke(message)

    let DTWAIN_LogMessageA (message: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LogMessageA.Value.Invoke(message)

    let DTWAIN_LogMessageW (message: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        LogMessageW.Value.Invoke(message)

    let DTWAIN_MakeRGB (red: LONG) (green: LONG) (blue: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        MakeRGB.Value.Invoke(red, green, blue)

    let DTWAIN_OpenSource (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        OpenSource.Value.Invoke(source)

    let DTWAIN_OpenSourcesOnSelect (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        OpenSourcesOnSelect.Value.Invoke(bset)

    let DTWAIN_RangeCreate (nenumtype: LONG) : DTWAIN_RANGE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeCreate.Value.Invoke(nenumtype)

    let DTWAIN_RangeCreateFromCap (source: DTWAIN_SOURCE) (lcaptype: LONG) : DTWAIN_RANGE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeCreateFromCap.Value.Invoke(source, lcaptype)

    let DTWAIN_RangeDestroy (psource: DTWAIN_RANGE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeDestroy.Value.Invoke(psource)

    let DTWAIN_RangeExpand (psource: DTWAIN_RANGE) (parray: DTWAIN_ARRAY byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeExpand.Value.Invoke(psource, &parray)

    let DTWAIN_RangeExpandEx (range: DTWAIN_RANGE) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeExpandEx.Value.Invoke(range)

    let DTWAIN_RangeGetAll (parray: DTWAIN_RANGE) (pvariantlow: LPVOID) (pvariantup: LPVOID) (pvariantstep: LPVOID) (pvariantdefault: LPVOID) (pvariantcurrent: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetAll.Value.Invoke(parray, pvariantlow, pvariantup, pvariantstep, pvariantdefault, pvariantcurrent)

    let DTWAIN_RangeGetAllFloat (parray: DTWAIN_RANGE) (pvariantlow: DTWAIN_FLOAT byref) (pvariantup: DTWAIN_FLOAT byref) (pvariantstep: DTWAIN_FLOAT byref) (pvariantdefault: DTWAIN_FLOAT byref) (pvariantcurrent: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetAllFloat.Value.Invoke(parray, &pvariantlow, &pvariantup, &pvariantstep, &pvariantdefault, &pvariantcurrent)

    let DTWAIN_RangeGetAllFloatString (parray: DTWAIN_RANGE) (dlow: System.Text.StringBuilder) (dup: System.Text.StringBuilder) (dstep: System.Text.StringBuilder) (ddefault: System.Text.StringBuilder) (dcurrent: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetAllFloatString.Value.Invoke(parray, dlow, dup, dstep, ddefault, dcurrent)

    let DTWAIN_RangeGetAllFloatStringA (parray: DTWAIN_RANGE) (dlow: System.Text.StringBuilder) (dup: System.Text.StringBuilder) (dstep: System.Text.StringBuilder) (ddefault: System.Text.StringBuilder) (dcurrent: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetAllFloatStringA.Value.Invoke(parray, dlow, dup, dstep, ddefault, dcurrent)

    let DTWAIN_RangeGetAllFloatStringW (parray: DTWAIN_RANGE) (dlow: System.Text.StringBuilder) (dup: System.Text.StringBuilder) (dstep: System.Text.StringBuilder) (ddefault: System.Text.StringBuilder) (dcurrent: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetAllFloatStringW.Value.Invoke(parray, dlow, dup, dstep, ddefault, dcurrent)

    let DTWAIN_RangeGetAllLong (parray: DTWAIN_RANGE) (pvariantlow: int byref) (pvariantup: int byref) (pvariantstep: int byref) (pvariantdefault: int byref) (pvariantcurrent: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetAllLong.Value.Invoke(parray, &pvariantlow, &pvariantup, &pvariantstep, &pvariantdefault, &pvariantcurrent)

    let DTWAIN_RangeGetCount (parray: DTWAIN_RANGE) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetCount.Value.Invoke(parray)

    let DTWAIN_RangeGetExpValue (parray: DTWAIN_RANGE) (lpos: LONG) (pvariant: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetExpValue.Value.Invoke(parray, lpos, pvariant)

    let DTWAIN_RangeGetExpValueFloat (parray: DTWAIN_RANGE) (lpos: LONG) (pval: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetExpValueFloat.Value.Invoke(parray, lpos, &pval)

    let DTWAIN_RangeGetExpValueFloatString (parray: DTWAIN_RANGE) (lpos: LONG) (pval: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetExpValueFloatString.Value.Invoke(parray, lpos, pval)

    let DTWAIN_RangeGetExpValueFloatStringA (parray: DTWAIN_RANGE) (lpos: LONG) (pval: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetExpValueFloatStringA.Value.Invoke(parray, lpos, pval)

    let DTWAIN_RangeGetExpValueFloatStringW (parray: DTWAIN_RANGE) (lpos: LONG) (pval: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetExpValueFloatStringW.Value.Invoke(parray, lpos, pval)

    let DTWAIN_RangeGetExpValueLong (parray: DTWAIN_RANGE) (lpos: LONG) (pval: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetExpValueLong.Value.Invoke(parray, lpos, &pval)

    let DTWAIN_RangeGetNearestValue (parray: DTWAIN_RANGE) (pvariantin: LPVOID) (pvariantout: LPVOID) (roundtype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetNearestValue.Value.Invoke(parray, pvariantin, pvariantout, roundtype)

    let DTWAIN_RangeGetPos (parray: DTWAIN_RANGE) (pvariant: LPVOID) (ppos: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetPos.Value.Invoke(parray, pvariant, &ppos)

    let DTWAIN_RangeGetPosFloat (parray: DTWAIN_RANGE) (val1: DTWAIN_FLOAT) (ppos: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetPosFloat.Value.Invoke(parray, val1, &ppos)

    let DTWAIN_RangeGetPosFloatString (parray: DTWAIN_RANGE) (val1: string) (ppos: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetPosFloatString.Value.Invoke(parray, val1, &ppos)

    let DTWAIN_RangeGetPosFloatStringA (parray: DTWAIN_RANGE) (val1: string) (ppos: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetPosFloatStringA.Value.Invoke(parray, val1, &ppos)

    let DTWAIN_RangeGetPosFloatStringW (parray: DTWAIN_RANGE) (val1: string) (ppos: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetPosFloatStringW.Value.Invoke(parray, val1, &ppos)

    let DTWAIN_RangeGetPosLong (parray: DTWAIN_RANGE) (value: LONG) (ppos: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetPosLong.Value.Invoke(parray, value, &ppos)

    let DTWAIN_RangeGetValue (parray: DTWAIN_RANGE) (nwhich: LONG) (pvariant: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetValue.Value.Invoke(parray, nwhich, pvariant)

    let DTWAIN_RangeGetValueFloat (parray: DTWAIN_RANGE) (nwhich: LONG) (pval: DTWAIN_FLOAT byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetValueFloat.Value.Invoke(parray, nwhich, &pval)

    let DTWAIN_RangeGetValueFloatString (parray: DTWAIN_RANGE) (nwhich: LONG) (pval: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetValueFloatString.Value.Invoke(parray, nwhich, pval)

    let DTWAIN_RangeGetValueFloatStringA (parray: DTWAIN_RANGE) (nwhich: LONG) (dvalue: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetValueFloatStringA.Value.Invoke(parray, nwhich, dvalue)

    let DTWAIN_RangeGetValueFloatStringW (parray: DTWAIN_RANGE) (nwhich: LONG) (dvalue: System.Text.StringBuilder) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetValueFloatStringW.Value.Invoke(parray, nwhich, dvalue)

    let DTWAIN_RangeGetValueLong (parray: DTWAIN_RANGE) (nwhich: LONG) (pval: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeGetValueLong.Value.Invoke(parray, nwhich, &pval)

    let DTWAIN_RangeIsValid (range: DTWAIN_RANGE) (pstatus: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeIsValid.Value.Invoke(range, &pstatus)

    let DTWAIN_RangeNearestValueFloat (parray: DTWAIN_RANGE) (din: DTWAIN_FLOAT) (pout: DTWAIN_FLOAT byref) (roundtype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeNearestValueFloat.Value.Invoke(parray, din, &pout, roundtype)

    let DTWAIN_RangeNearestValueFloatString (parray: DTWAIN_RANGE) (din: string) (pout: System.Text.StringBuilder) (roundtype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeNearestValueFloatString.Value.Invoke(parray, din, pout, roundtype)

    let DTWAIN_RangeNearestValueFloatStringA (parray: DTWAIN_RANGE) (din: string) (dout: System.Text.StringBuilder) (roundtype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeNearestValueFloatStringA.Value.Invoke(parray, din, dout, roundtype)

    let DTWAIN_RangeNearestValueFloatStringW (parray: DTWAIN_RANGE) (din: string) (dout: System.Text.StringBuilder) (roundtype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeNearestValueFloatStringW.Value.Invoke(parray, din, dout, roundtype)

    let DTWAIN_RangeNearestValueLong (parray: DTWAIN_RANGE) (lin: LONG) (pout: int byref) (roundtype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeNearestValueLong.Value.Invoke(parray, lin, &pout, roundtype)

    let DTWAIN_RangeSetAll (parray: DTWAIN_RANGE) (pvariantlow: LPVOID) (pvariantup: LPVOID) (pvariantstep: LPVOID) (pvariantdefault: LPVOID) (pvariantcurrent: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetAll.Value.Invoke(parray, pvariantlow, pvariantup, pvariantstep, pvariantdefault, pvariantcurrent)

    let DTWAIN_RangeSetAllFloat (parray: DTWAIN_RANGE) (dlow: DTWAIN_FLOAT) (dup: DTWAIN_FLOAT) (dstep: DTWAIN_FLOAT) (ddefault: DTWAIN_FLOAT) (dcurrent: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetAllFloat.Value.Invoke(parray, dlow, dup, dstep, ddefault, dcurrent)

    let DTWAIN_RangeSetAllFloatString (parray: DTWAIN_RANGE) (dlow: string) (dup: string) (dstep: string) (ddefault: string) (dcurrent: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetAllFloatString.Value.Invoke(parray, dlow, dup, dstep, ddefault, dcurrent)

    let DTWAIN_RangeSetAllFloatStringA (parray: DTWAIN_RANGE) (dlow: string) (dup: string) (dstep: string) (ddefault: string) (dcurrent: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetAllFloatStringA.Value.Invoke(parray, dlow, dup, dstep, ddefault, dcurrent)

    let DTWAIN_RangeSetAllFloatStringW (parray: DTWAIN_RANGE) (dlow: string) (dup: string) (dstep: string) (ddefault: string) (dcurrent: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetAllFloatStringW.Value.Invoke(parray, dlow, dup, dstep, ddefault, dcurrent)

    let DTWAIN_RangeSetAllLong (parray: DTWAIN_RANGE) (llow: LONG) (lup: LONG) (lstep: LONG) (ldefault: LONG) (lcurrent: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetAllLong.Value.Invoke(parray, llow, lup, lstep, ldefault, lcurrent)

    let DTWAIN_RangeSetValue (parray: DTWAIN_RANGE) (nwhich: LONG) (pval: LPVOID) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetValue.Value.Invoke(parray, nwhich, pval)

    let DTWAIN_RangeSetValueFloat (parray: DTWAIN_RANGE) (nwhich: LONG) (val1: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetValueFloat.Value.Invoke(parray, nwhich, val1)

    let DTWAIN_RangeSetValueFloatString (parray: DTWAIN_RANGE) (nwhich: LONG) (val1: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetValueFloatString.Value.Invoke(parray, nwhich, val1)

    let DTWAIN_RangeSetValueFloatStringA (parray: DTWAIN_RANGE) (nwhich: LONG) (dvalue: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetValueFloatStringA.Value.Invoke(parray, nwhich, dvalue)

    let DTWAIN_RangeSetValueFloatStringW (parray: DTWAIN_RANGE) (nwhich: LONG) (dvalue: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetValueFloatStringW.Value.Invoke(parray, nwhich, dvalue)

    let DTWAIN_RangeSetValueLong (parray: DTWAIN_RANGE) (nwhich: LONG) (val1: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RangeSetValueLong.Value.Invoke(parray, nwhich, val1)

    let DTWAIN_ResetPDFTextElement (textelement: DTWAIN_PDFTEXTELEMENT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ResetPDFTextElement.Value.Invoke(textelement)

    let DTWAIN_RewindPage (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        RewindPage.Value.Invoke(source)

    let DTWAIN_SelectDefaultOCREngine() : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectDefaultOCREngine.Value.Invoke()

    let DTWAIN_SelectDefaultSource() : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectDefaultSource.Value.Invoke()

    let DTWAIN_SelectDefaultSourceWithOpen (bopen: DTWAIN_BOOL) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectDefaultSourceWithOpen.Value.Invoke(bopen)

    let DTWAIN_SelectOCREngine() : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngine.Value.Invoke()

    let DTWAIN_SelectOCREngine2 (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (noptions: LONG) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngine2.Value.Invoke(hwndparent, sztitle, xpos, ypos, noptions)

    let DTWAIN_SelectOCREngine2A (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (noptions: LONG) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngine2A.Value.Invoke(hwndparent, sztitle, xpos, ypos, noptions)

    let DTWAIN_SelectOCREngine2Ex (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (szincludefilter: string) (szexcludefilter: string) (sznamemapping: string) (noptions: LONG) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngine2Ex.Value.Invoke(hwndparent, sztitle, xpos, ypos, szincludefilter, szexcludefilter, sznamemapping, noptions)

    let DTWAIN_SelectOCREngine2ExA (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (szincludenames: string) (szexcludenames: string) (sznamemapping: string) (noptions: LONG) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngine2ExA.Value.Invoke(hwndparent, sztitle, xpos, ypos, szincludenames, szexcludenames, sznamemapping, noptions)

    let DTWAIN_SelectOCREngine2ExW (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (szincludenames: string) (szexcludenames: string) (sznamemapping: string) (noptions: LONG) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngine2ExW.Value.Invoke(hwndparent, sztitle, xpos, ypos, szincludenames, szexcludenames, sznamemapping, noptions)

    let DTWAIN_SelectOCREngine2W (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (noptions: LONG) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngine2W.Value.Invoke(hwndparent, sztitle, xpos, ypos, noptions)

    let DTWAIN_SelectOCREngineByName (lpszname: string) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngineByName.Value.Invoke(lpszname)

    let DTWAIN_SelectOCREngineByNameA (lpszname: string) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngineByNameA.Value.Invoke(lpszname)

    let DTWAIN_SelectOCREngineByNameW (lpszname: string) : DTWAIN_OCRENGINE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectOCREngineByNameW.Value.Invoke(lpszname)

    let DTWAIN_SelectSource() : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSource.Value.Invoke()

    let DTWAIN_SelectSource2 (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (noptions: LONG) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSource2.Value.Invoke(hwndparent, sztitle, xpos, ypos, noptions)

    let DTWAIN_SelectSource2A (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (noptions: LONG) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSource2A.Value.Invoke(hwndparent, sztitle, xpos, ypos, noptions)

    let DTWAIN_SelectSource2Ex (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (szincludefilter: string) (szexcludefilter: string) (sznamemapping: string) (noptions: LONG) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSource2Ex.Value.Invoke(hwndparent, sztitle, xpos, ypos, szincludefilter, szexcludefilter, sznamemapping, noptions)

    let DTWAIN_SelectSource2ExA (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (szincludenames: string) (szexcludenames: string) (sznamemapping: string) (noptions: LONG) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSource2ExA.Value.Invoke(hwndparent, sztitle, xpos, ypos, szincludenames, szexcludenames, sznamemapping, noptions)

    let DTWAIN_SelectSource2ExW (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (szincludenames: string) (szexcludenames: string) (sznamemapping: string) (noptions: LONG) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSource2ExW.Value.Invoke(hwndparent, sztitle, xpos, ypos, szincludenames, szexcludenames, sznamemapping, noptions)

    let DTWAIN_SelectSource2W (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (noptions: LONG) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSource2W.Value.Invoke(hwndparent, sztitle, xpos, ypos, noptions)

    let DTWAIN_SelectSourceByName (lpszname: string) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSourceByName.Value.Invoke(lpszname)

    let DTWAIN_SelectSourceByNameA (lpszname: string) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSourceByNameA.Value.Invoke(lpszname)

    let DTWAIN_SelectSourceByNameW (lpszname: string) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSourceByNameW.Value.Invoke(lpszname)

    let DTWAIN_SelectSourceByNameWithOpen (lpszname: string) (bopen: DTWAIN_BOOL) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSourceByNameWithOpen.Value.Invoke(lpszname, bopen)

    let DTWAIN_SelectSourceByNameWithOpenA (lpszname: string) (bopen: DTWAIN_BOOL) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSourceByNameWithOpenA.Value.Invoke(lpszname, bopen)

    let DTWAIN_SelectSourceByNameWithOpenW (lpszname: string) (bopen: DTWAIN_BOOL) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSourceByNameWithOpenW.Value.Invoke(lpszname, bopen)

    let DTWAIN_SelectSourceWithOpen (bopen: DTWAIN_BOOL) : DTWAIN_SOURCE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SelectSourceWithOpen.Value.Invoke(bopen)

    let DTWAIN_SetAcquireArea (source: DTWAIN_SOURCE) (lsettype: LONG) (floatenum: DTWAIN_ARRAY) (actualenum: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireArea.Value.Invoke(source, lsettype, floatenum, actualenum)

    let DTWAIN_SetAcquireArea2 (source: DTWAIN_SOURCE) (left: DTWAIN_FLOAT) (top: DTWAIN_FLOAT) (right: DTWAIN_FLOAT) (bottom: DTWAIN_FLOAT) (lunit: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireArea2.Value.Invoke(source, left, top, right, bottom, lunit, flags)

    let DTWAIN_SetAcquireArea2String (source: DTWAIN_SOURCE) (left: string) (top: string) (right: string) (bottom: string) (lunit: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireArea2String.Value.Invoke(source, left, top, right, bottom, lunit, flags)

    let DTWAIN_SetAcquireArea2StringA (source: DTWAIN_SOURCE) (left: string) (top: string) (right: string) (bottom: string) (lunit: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireArea2StringA.Value.Invoke(source, left, top, right, bottom, lunit, flags)

    let DTWAIN_SetAcquireArea2StringW (source: DTWAIN_SOURCE) (left: string) (top: string) (right: string) (bottom: string) (lunit: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireArea2StringW.Value.Invoke(source, left, top, right, bottom, lunit, flags)

    let DTWAIN_SetAcquireImageNegative (source: DTWAIN_SOURCE) (isnegative: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireImageNegative.Value.Invoke(source, isnegative)

    let DTWAIN_SetAcquireImageScale (source: DTWAIN_SOURCE) (xscale: DTWAIN_FLOAT) (yscale: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireImageScale.Value.Invoke(source, xscale, yscale)

    let DTWAIN_SetAcquireImageScaleString (source: DTWAIN_SOURCE) (xscale: string) (yscale: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireImageScaleString.Value.Invoke(source, xscale, yscale)

    let DTWAIN_SetAcquireImageScaleStringA (source: DTWAIN_SOURCE) (xscale: string) (yscale: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireImageScaleStringA.Value.Invoke(source, xscale, yscale)

    let DTWAIN_SetAcquireImageScaleStringW (source: DTWAIN_SOURCE) (xscale: string) (yscale: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireImageScaleStringW.Value.Invoke(source, xscale, yscale)

    let DTWAIN_SetAcquireStripBuffer (source: DTWAIN_SOURCE) (hmem: HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireStripBuffer.Value.Invoke(source, hmem)

    let DTWAIN_SetAcquireStripSize (source: DTWAIN_SOURCE) (stripsize: DWORD) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAcquireStripSize.Value.Invoke(source, stripsize)

    let DTWAIN_SetAlarmVolume (source: DTWAIN_SOURCE) (volume: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAlarmVolume.Value.Invoke(source, volume)

    let DTWAIN_SetAlarms (source: DTWAIN_SOURCE) (alarms: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAlarms.Value.Invoke(source, alarms)

    let DTWAIN_SetAllCapsToDefault (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAllCapsToDefault.Value.Invoke(source)

    let DTWAIN_SetAppInfo (szverstr: string) (szmanu: string) (szprodfam: string) (szprodname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAppInfo.Value.Invoke(szverstr, szmanu, szprodfam, szprodname)

    let DTWAIN_SetAppInfoA (szverstr: string) (szmanu: string) (szprodfam: string) (szprodname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAppInfoA.Value.Invoke(szverstr, szmanu, szprodfam, szprodname)

    let DTWAIN_SetAppInfoW (szverstr: string) (szmanu: string) (szprodfam: string) (szprodname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAppInfoW.Value.Invoke(szverstr, szmanu, szprodfam, szprodname)

    let DTWAIN_SetAuthor (source: DTWAIN_SOURCE) (szauthor: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAuthor.Value.Invoke(source, szauthor)

    let DTWAIN_SetAuthorA (source: DTWAIN_SOURCE) (szauthor: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAuthorA.Value.Invoke(source, szauthor)

    let DTWAIN_SetAuthorW (source: DTWAIN_SOURCE) (szauthor: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAuthorW.Value.Invoke(source, szauthor)

    let DTWAIN_SetAvailablePrinters (source: DTWAIN_SOURCE) (lpavailprinters: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAvailablePrinters.Value.Invoke(source, lpavailprinters)

    let DTWAIN_SetAvailablePrintersArray (source: DTWAIN_SOURCE) (availprinters: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetAvailablePrintersArray.Value.Invoke(source, availprinters)

    let DTWAIN_SetBitDepth (source: DTWAIN_SOURCE) (bitdepth: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBitDepth.Value.Invoke(source, bitdepth, bsetcurrent)

    let DTWAIN_SetBlankPageDetection (source: DTWAIN_SOURCE) (threshold: DTWAIN_FLOAT) (discard_option: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetection.Value.Invoke(source, threshold, discard_option, bset)

    let DTWAIN_SetBlankPageDetectionEx (source: DTWAIN_SOURCE) (threshold: DTWAIN_FLOAT) (autodetect: LONG) (detectopts: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetectionEx.Value.Invoke(source, threshold, autodetect, detectopts, bset)

    let DTWAIN_SetBlankPageDetectionExString (source: DTWAIN_SOURCE) (threshold: string) (autodetect_option: LONG) (detectopts: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetectionExString.Value.Invoke(source, threshold, autodetect_option, detectopts, bset)

    let DTWAIN_SetBlankPageDetectionExStringA (source: DTWAIN_SOURCE) (threshold: string) (autodetect_option: LONG) (detectopts: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetectionExStringA.Value.Invoke(source, threshold, autodetect_option, detectopts, bset)

    let DTWAIN_SetBlankPageDetectionExStringW (source: DTWAIN_SOURCE) (threshold: string) (autodetect_option: LONG) (detectopts: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetectionExStringW.Value.Invoke(source, threshold, autodetect_option, detectopts, bset)

    let DTWAIN_SetBlankPageDetectionString (source: DTWAIN_SOURCE) (threshold: string) (autodetect_option: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetectionString.Value.Invoke(source, threshold, autodetect_option, bset)

    let DTWAIN_SetBlankPageDetectionStringA (source: DTWAIN_SOURCE) (threshold: string) (autodetect_option: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetectionStringA.Value.Invoke(source, threshold, autodetect_option, bset)

    let DTWAIN_SetBlankPageDetectionStringW (source: DTWAIN_SOURCE) (threshold: string) (autodetect_option: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBlankPageDetectionStringW.Value.Invoke(source, threshold, autodetect_option, bset)

    let DTWAIN_SetBrightness (source: DTWAIN_SOURCE) (brightness: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBrightness.Value.Invoke(source, brightness)

    let DTWAIN_SetBrightnessString (source: DTWAIN_SOURCE) (brightness: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBrightnessString.Value.Invoke(source, brightness)

    let DTWAIN_SetBrightnessStringA (source: DTWAIN_SOURCE) (contrast: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBrightnessStringA.Value.Invoke(source, contrast)

    let DTWAIN_SetBrightnessStringW (source: DTWAIN_SOURCE) (contrast: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBrightnessStringW.Value.Invoke(source, contrast)

    let DTWAIN_SetBufferedTileMode (source: DTWAIN_SOURCE) (btilemode: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetBufferedTileMode.Value.Invoke(source, btilemode)

    let DTWAIN_SetCamera (source: DTWAIN_SOURCE) (szcamera: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCamera.Value.Invoke(source, szcamera)

    let DTWAIN_SetCameraA (source: DTWAIN_SOURCE) (szcamera: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCameraA.Value.Invoke(source, szcamera)

    let DTWAIN_SetCameraW (source: DTWAIN_SOURCE) (szcamera: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCameraW.Value.Invoke(source, szcamera)

    let DTWAIN_SetCapValues (source: DTWAIN_SOURCE) (lcap: LONG) (lsettype: LONG) (parray: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCapValues.Value.Invoke(source, lcap, lsettype, parray)

    let DTWAIN_SetCapValuesEx (source: DTWAIN_SOURCE) (lcap: LONG) (lsettype: LONG) (lcontainertype: LONG) (parray: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCapValuesEx.Value.Invoke(source, lcap, lsettype, lcontainertype, parray)

    let DTWAIN_SetCapValuesEx2 (source: DTWAIN_SOURCE) (lcap: LONG) (lsettype: LONG) (lcontainertype: LONG) (ndatatype: LONG) (parray: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCapValuesEx2.Value.Invoke(source, lcap, lsettype, lcontainertype, ndatatype, parray)

    let DTWAIN_SetCaption (source: DTWAIN_SOURCE) (caption: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCaption.Value.Invoke(source, caption)

    let DTWAIN_SetCaptionA (source: DTWAIN_SOURCE) (caption: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCaptionA.Value.Invoke(source, caption)

    let DTWAIN_SetCaptionW (source: DTWAIN_SOURCE) (caption: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCaptionW.Value.Invoke(source, caption)

    let DTWAIN_SetCompressionType (source: DTWAIN_SOURCE) (lcompression: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCompressionType.Value.Invoke(source, lcompression, bsetcurrent)

    let DTWAIN_SetContrast (source: DTWAIN_SOURCE) (contrast: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetContrast.Value.Invoke(source, contrast)

    let DTWAIN_SetContrastString (source: DTWAIN_SOURCE) (contrast: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetContrastString.Value.Invoke(source, contrast)

    let DTWAIN_SetContrastStringA (source: DTWAIN_SOURCE) (contrast: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetContrastStringA.Value.Invoke(source, contrast)

    let DTWAIN_SetContrastStringW (source: DTWAIN_SOURCE) (contrast: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetContrastStringW.Value.Invoke(source, contrast)

    let DTWAIN_SetCountry (ncountry: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCountry.Value.Invoke(ncountry)

    let DTWAIN_SetCurrentRetryCount (source: DTWAIN_SOURCE) (ncount: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCurrentRetryCount.Value.Invoke(source, ncount)

    let DTWAIN_SetCustomDSData (source: DTWAIN_SOURCE) (hdata: HANDLE) (data: byte[]) (dsize: DWORD) (nflags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetCustomDSData.Value.Invoke(source, hdata, data, dsize, nflags)

    let DTWAIN_SetDSMSearchOrder (searchpath: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDSMSearchOrder.Value.Invoke(searchpath)

    let DTWAIN_SetDSMSearchOrderEx (searchorder: string) (userpath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDSMSearchOrderEx.Value.Invoke(searchorder, userpath)

    let DTWAIN_SetDSMSearchOrderExA (searchorder: string) (szuserpath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDSMSearchOrderExA.Value.Invoke(searchorder, szuserpath)

    let DTWAIN_SetDSMSearchOrderExW (searchorder: string) (szuserpath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDSMSearchOrderExW.Value.Invoke(searchorder, szuserpath)

    let DTWAIN_SetDefaultSource (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDefaultSource.Value.Invoke(source)

    let DTWAIN_SetDeviceNotifications (source: DTWAIN_SOURCE) (devevents: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDeviceNotifications.Value.Invoke(source, devevents)

    let DTWAIN_SetDeviceTimeDate (source: DTWAIN_SOURCE) (sztimedate: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDeviceTimeDate.Value.Invoke(source, sztimedate)

    let DTWAIN_SetDeviceTimeDateA (source: DTWAIN_SOURCE) (sztimedate: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDeviceTimeDateA.Value.Invoke(source, sztimedate)

    let DTWAIN_SetDeviceTimeDateW (source: DTWAIN_SOURCE) (sztimedate: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDeviceTimeDateW.Value.Invoke(source, sztimedate)

    let DTWAIN_SetDoubleFeedDetectLength (source: DTWAIN_SOURCE) (value: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDoubleFeedDetectLength.Value.Invoke(source, value)

    let DTWAIN_SetDoubleFeedDetectLengthString (source: DTWAIN_SOURCE) (value: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDoubleFeedDetectLengthString.Value.Invoke(source, value)

    let DTWAIN_SetDoubleFeedDetectLengthStringA (source: DTWAIN_SOURCE) (szlength: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDoubleFeedDetectLengthStringA.Value.Invoke(source, szlength)

    let DTWAIN_SetDoubleFeedDetectLengthStringW (source: DTWAIN_SOURCE) (szlength: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDoubleFeedDetectLengthStringW.Value.Invoke(source, szlength)

    let DTWAIN_SetDoubleFeedDetectValues (source: DTWAIN_SOURCE) (prray: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDoubleFeedDetectValues.Value.Invoke(source, prray)

    let DTWAIN_SetDoublePageCountOnDuplex (source: DTWAIN_SOURCE) (bdoublecount: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetDoublePageCountOnDuplex.Value.Invoke(source, bdoublecount)

    let DTWAIN_SetEOJDetectValue (source: DTWAIN_SOURCE) (nvalue: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetEOJDetectValue.Value.Invoke(source, nvalue)

    let DTWAIN_SetErrorBufferThreshold (nerrors: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetErrorBufferThreshold.Value.Invoke(nerrors)

    let DTWAIN_SetFeederAlignment (source: DTWAIN_SOURCE) (lpalignment: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFeederAlignment.Value.Invoke(source, lpalignment)

    let DTWAIN_SetFeederOrder (source: DTWAIN_SOURCE) (lorder: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFeederOrder.Value.Invoke(source, lorder)

    let DTWAIN_SetFeederWaitTime (source: DTWAIN_SOURCE) (waittime: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFeederWaitTime.Value.Invoke(source, waittime, flags)

    let DTWAIN_SetFileAutoIncrement (source: DTWAIN_SOURCE) (increment: LONG) (bresetonacquire: DTWAIN_BOOL) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFileAutoIncrement.Value.Invoke(source, increment, bresetonacquire, bset)

    let DTWAIN_SetFileCompressionType (source: DTWAIN_SOURCE) (lcompression: LONG) (biscustom: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFileCompressionType.Value.Invoke(source, lcompression, biscustom)

    let DTWAIN_SetFileSavePos (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (nflags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFileSavePos.Value.Invoke(hwndparent, sztitle, xpos, ypos, nflags)

    let DTWAIN_SetFileSavePosA (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (nflags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFileSavePosA.Value.Invoke(hwndparent, sztitle, xpos, ypos, nflags)

    let DTWAIN_SetFileSavePosW (hwndparent: HWND) (sztitle: string) (xpos: LONG) (ypos: LONG) (nflags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFileSavePosW.Value.Invoke(hwndparent, sztitle, xpos, ypos, nflags)

    let DTWAIN_SetFileXferFormat (source: DTWAIN_SOURCE) (lfiletype: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetFileXferFormat.Value.Invoke(source, lfiletype, bsetcurrent)

    let DTWAIN_SetHalftone (source: DTWAIN_SOURCE) (lphalftone: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetHalftone.Value.Invoke(source, lphalftone)

    let DTWAIN_SetHalftoneA (source: DTWAIN_SOURCE) (lphalftone: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetHalftoneA.Value.Invoke(source, lphalftone)

    let DTWAIN_SetHalftoneW (source: DTWAIN_SOURCE) (lphalftone: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetHalftoneW.Value.Invoke(source, lphalftone)

    let DTWAIN_SetHighlight (source: DTWAIN_SOURCE) (highlight: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetHighlight.Value.Invoke(source, highlight)

    let DTWAIN_SetHighlightString (source: DTWAIN_SOURCE) (highlight: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetHighlightString.Value.Invoke(source, highlight)

    let DTWAIN_SetHighlightStringA (source: DTWAIN_SOURCE) (highlight: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetHighlightStringA.Value.Invoke(source, highlight)

    let DTWAIN_SetHighlightStringW (source: DTWAIN_SOURCE) (highlight: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetHighlightStringW.Value.Invoke(source, highlight)

    let DTWAIN_SetJobControl (source: DTWAIN_SOURCE) (jobcontrol: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetJobControl.Value.Invoke(source, jobcontrol, bsetcurrent)

    let DTWAIN_SetJpegValues (source: DTWAIN_SOURCE) (quality: LONG) (progressive: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetJpegValues.Value.Invoke(source, quality, progressive)

    let DTWAIN_SetJpegXRValues (source: DTWAIN_SOURCE) (quality: LONG) (progressive: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetJpegXRValues.Value.Invoke(source, quality, progressive)

    let DTWAIN_SetLanguage (nlanguage: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetLanguage.Value.Invoke(nlanguage)

    let DTWAIN_SetLastError (nerror: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetLastError.Value.Invoke(nerror)

    let DTWAIN_SetLightPath (source: DTWAIN_SOURCE) (lightpath: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetLightPath.Value.Invoke(source, lightpath)

    let DTWAIN_SetLightPathEx (source: DTWAIN_SOURCE) (lightpaths: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetLightPathEx.Value.Invoke(source, lightpaths)

    let DTWAIN_SetLightSource (source: DTWAIN_SOURCE) (lightsource: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetLightSource.Value.Invoke(source, lightsource)

    let DTWAIN_SetLightSources (source: DTWAIN_SOURCE) (lightsources: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetLightSources.Value.Invoke(source, lightsources)

    let DTWAIN_SetManualDuplexMode (source: DTWAIN_SOURCE) (flags: LONG) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetManualDuplexMode.Value.Invoke(source, flags, bset)

    let DTWAIN_SetMaxAcquisitions (source: DTWAIN_SOURCE) (maxacquires: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetMaxAcquisitions.Value.Invoke(source, maxacquires)

    let DTWAIN_SetMaxBuffers (source: DTWAIN_SOURCE) (maxbuf: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetMaxBuffers.Value.Invoke(source, maxbuf)

    let DTWAIN_SetMaxRetryAttempts (source: DTWAIN_SOURCE) (nattempts: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetMaxRetryAttempts.Value.Invoke(source, nattempts)

    let DTWAIN_SetMultipageScanMode (source: DTWAIN_SOURCE) (scantype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetMultipageScanMode.Value.Invoke(source, scantype)

    let DTWAIN_SetNoiseFilter (source: DTWAIN_SOURCE) (noisefilter: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetNoiseFilter.Value.Invoke(source, noisefilter)

    let DTWAIN_SetOCRCapValues (engine: DTWAIN_OCRENGINE) (ocrcapvalue: LONG) (settype: LONG) (capvalues: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetOCRCapValues.Value.Invoke(engine, ocrcapvalue, settype, capvalues)

    let DTWAIN_SetOrientation (source: DTWAIN_SOURCE) (orient: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetOrientation.Value.Invoke(source, orient, bsetcurrent)

    let DTWAIN_SetOverscan (source: DTWAIN_SOURCE) (value: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetOverscan.Value.Invoke(source, value, bsetcurrent)

    let DTWAIN_SetPDFAESEncryption (source: DTWAIN_SOURCE) (nwhichencryption: LONG) (buseaes: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFAESEncryption.Value.Invoke(source, nwhichencryption, buseaes)

    let DTWAIN_SetPDFASCIICompression (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFASCIICompression.Value.Invoke(source, bset)

    let DTWAIN_SetPDFAuthor (source: DTWAIN_SOURCE) (lpauthor: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFAuthor.Value.Invoke(source, lpauthor)

    let DTWAIN_SetPDFAuthorA (source: DTWAIN_SOURCE) (lpauthor: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFAuthorA.Value.Invoke(source, lpauthor)

    let DTWAIN_SetPDFAuthorW (source: DTWAIN_SOURCE) (lpauthor: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFAuthorW.Value.Invoke(source, lpauthor)

    let DTWAIN_SetPDFCompression (source: DTWAIN_SOURCE) (bcompression: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFCompression.Value.Invoke(source, bcompression)

    let DTWAIN_SetPDFCreator (source: DTWAIN_SOURCE) (lpcreator: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFCreator.Value.Invoke(source, lpcreator)

    let DTWAIN_SetPDFCreatorA (source: DTWAIN_SOURCE) (lpcreator: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFCreatorA.Value.Invoke(source, lpcreator)

    let DTWAIN_SetPDFCreatorW (source: DTWAIN_SOURCE) (lpcreator: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFCreatorW.Value.Invoke(source, lpcreator)

    let DTWAIN_SetPDFEncryption (source: DTWAIN_SOURCE) (buseencryption: DTWAIN_BOOL) (lpszuser: string) (lpszowner: string) (permissions: DWORD) (usestrongencryption: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFEncryption.Value.Invoke(source, buseencryption, lpszuser, lpszowner, permissions, usestrongencryption)

    let DTWAIN_SetPDFEncryptionA (source: DTWAIN_SOURCE) (buseencryption: DTWAIN_BOOL) (lpszuser: string) (lpszowner: string) (permissions: DWORD) (usestrongencryption: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFEncryptionA.Value.Invoke(source, buseencryption, lpszuser, lpszowner, permissions, usestrongencryption)

    let DTWAIN_SetPDFEncryptionW (source: DTWAIN_SOURCE) (buseencryption: DTWAIN_BOOL) (lpszuser: string) (lpszowner: string) (permissions: DWORD) (usestrongencryption: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFEncryptionW.Value.Invoke(source, buseencryption, lpszuser, lpszowner, permissions, usestrongencryption)

    let DTWAIN_SetPDFJpegQuality (source: DTWAIN_SOURCE) (quality: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFJpegQuality.Value.Invoke(source, quality)

    let DTWAIN_SetPDFKeywords (source: DTWAIN_SOURCE) (lpkeywords: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFKeywords.Value.Invoke(source, lpkeywords)

    let DTWAIN_SetPDFKeywordsA (source: DTWAIN_SOURCE) (lpkeywords: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFKeywordsA.Value.Invoke(source, lpkeywords)

    let DTWAIN_SetPDFKeywordsW (source: DTWAIN_SOURCE) (lpkeywords: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFKeywordsW.Value.Invoke(source, lpkeywords)

    let DTWAIN_SetPDFOCRConversion (engine: DTWAIN_OCRENGINE) (pagetype: LONG) (filetype: LONG) (pixeltype: LONG) (bitdepth: LONG) (options: LONG) : LONG =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFOCRConversion.Value.Invoke(engine, pagetype, filetype, pixeltype, bitdepth, options)

    let DTWAIN_SetPDFOCRMode (source: DTWAIN_SOURCE) (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFOCRMode.Value.Invoke(source, bset)

    let DTWAIN_SetPDFOrientation (source: DTWAIN_SOURCE) (lporientation: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFOrientation.Value.Invoke(source, lporientation)

    let DTWAIN_SetPDFPageScale (source: DTWAIN_SOURCE) (noptions: LONG) (xscale: DTWAIN_FLOAT) (yscale: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageScale.Value.Invoke(source, noptions, xscale, yscale)

    let DTWAIN_SetPDFPageScaleString (source: DTWAIN_SOURCE) (noptions: LONG) (xscale: string) (yscale: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageScaleString.Value.Invoke(source, noptions, xscale, yscale)

    let DTWAIN_SetPDFPageScaleStringA (source: DTWAIN_SOURCE) (noptions: LONG) (xscale: string) (yscale: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageScaleStringA.Value.Invoke(source, noptions, xscale, yscale)

    let DTWAIN_SetPDFPageScaleStringW (source: DTWAIN_SOURCE) (noptions: LONG) (xscale: string) (yscale: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageScaleStringW.Value.Invoke(source, noptions, xscale, yscale)

    let DTWAIN_SetPDFPageSize (source: DTWAIN_SOURCE) (pagesize: LONG) (customwidth: DTWAIN_FLOAT) (customheight: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageSize.Value.Invoke(source, pagesize, customwidth, customheight)

    let DTWAIN_SetPDFPageSizeString (source: DTWAIN_SOURCE) (pagesize: LONG) (customwidth: string) (customheight: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageSizeString.Value.Invoke(source, pagesize, customwidth, customheight)

    let DTWAIN_SetPDFPageSizeStringA (source: DTWAIN_SOURCE) (pagesize: LONG) (customwidth: string) (customheight: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageSizeStringA.Value.Invoke(source, pagesize, customwidth, customheight)

    let DTWAIN_SetPDFPageSizeStringW (source: DTWAIN_SOURCE) (pagesize: LONG) (customwidth: string) (customheight: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPageSizeStringW.Value.Invoke(source, pagesize, customwidth, customheight)

    let DTWAIN_SetPDFPolarity (source: DTWAIN_SOURCE) (polarity: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFPolarity.Value.Invoke(source, polarity)

    let DTWAIN_SetPDFProducer (source: DTWAIN_SOURCE) (lpproducer: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFProducer.Value.Invoke(source, lpproducer)

    let DTWAIN_SetPDFProducerA (source: DTWAIN_SOURCE) (lpproducer: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFProducerA.Value.Invoke(source, lpproducer)

    let DTWAIN_SetPDFProducerW (source: DTWAIN_SOURCE) (lpproducer: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFProducerW.Value.Invoke(source, lpproducer)

    let DTWAIN_SetPDFSubject (source: DTWAIN_SOURCE) (lpsubject: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFSubject.Value.Invoke(source, lpsubject)

    let DTWAIN_SetPDFSubjectA (source: DTWAIN_SOURCE) (lpsubject: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFSubjectA.Value.Invoke(source, lpsubject)

    let DTWAIN_SetPDFSubjectW (source: DTWAIN_SOURCE) (lpsubject: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFSubjectW.Value.Invoke(source, lpsubject)

    let DTWAIN_SetPDFTextElementFloat (textelement: DTWAIN_PDFTEXTELEMENT) (val1: DTWAIN_FLOAT) (val2: DTWAIN_FLOAT) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTextElementFloat.Value.Invoke(textelement, val1, val2, flags)

    let DTWAIN_SetPDFTextElementLong (textelement: DTWAIN_PDFTEXTELEMENT) (val1: LONG) (val2: LONG) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTextElementLong.Value.Invoke(textelement, val1, val2, flags)

    let DTWAIN_SetPDFTextElementString (textelement: DTWAIN_PDFTEXTELEMENT) (val1: string) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTextElementString.Value.Invoke(textelement, val1, flags)

    let DTWAIN_SetPDFTextElementStringA (textelement: DTWAIN_PDFTEXTELEMENT) (szstring: string) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTextElementStringA.Value.Invoke(textelement, szstring, flags)

    let DTWAIN_SetPDFTextElementStringW (textelement: DTWAIN_PDFTEXTELEMENT) (szstring: string) (flags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTextElementStringW.Value.Invoke(textelement, szstring, flags)

    let DTWAIN_SetPDFTitle (source: DTWAIN_SOURCE) (lptitle: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTitle.Value.Invoke(source, lptitle)

    let DTWAIN_SetPDFTitleA (source: DTWAIN_SOURCE) (lptitle: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTitleA.Value.Invoke(source, lptitle)

    let DTWAIN_SetPDFTitleW (source: DTWAIN_SOURCE) (lptitle: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPDFTitleW.Value.Invoke(source, lptitle)

    let DTWAIN_SetPaperSize (source: DTWAIN_SOURCE) (papersize: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPaperSize.Value.Invoke(source, papersize, bsetcurrent)

    let DTWAIN_SetPatchMaxPriorities (source: DTWAIN_SOURCE) (nmaxsearchretries: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPatchMaxPriorities.Value.Invoke(source, nmaxsearchretries)

    let DTWAIN_SetPatchMaxRetries (source: DTWAIN_SOURCE) (nmaxretries: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPatchMaxRetries.Value.Invoke(source, nmaxretries)

    let DTWAIN_SetPatchPriorities (source: DTWAIN_SOURCE) (searchpriorities: DTWAIN_ARRAY) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPatchPriorities.Value.Invoke(source, searchpriorities)

    let DTWAIN_SetPatchSearchMode (source: DTWAIN_SOURCE) (nsearchmode: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPatchSearchMode.Value.Invoke(source, nsearchmode)

    let DTWAIN_SetPatchTimeOut (source: DTWAIN_SOURCE) (timeoutvalue: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPatchTimeOut.Value.Invoke(source, timeoutvalue)

    let DTWAIN_SetPixelFlavor (source: DTWAIN_SOURCE) (pixelflavor: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPixelFlavor.Value.Invoke(source, pixelflavor)

    let DTWAIN_SetPixelType (source: DTWAIN_SOURCE) (pixeltype: LONG) (bitdepth: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPixelType.Value.Invoke(source, pixeltype, bitdepth, bsetcurrent)

    let DTWAIN_SetPostScriptTitle (source: DTWAIN_SOURCE) (sztitle: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPostScriptTitle.Value.Invoke(source, sztitle)

    let DTWAIN_SetPostScriptTitleA (source: DTWAIN_SOURCE) (sztitle: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPostScriptTitleA.Value.Invoke(source, sztitle)

    let DTWAIN_SetPostScriptTitleW (source: DTWAIN_SOURCE) (sztitle: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPostScriptTitleW.Value.Invoke(source, sztitle)

    let DTWAIN_SetPostScriptType (source: DTWAIN_SOURCE) (pstype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPostScriptType.Value.Invoke(source, pstype)

    let DTWAIN_SetPrinter (source: DTWAIN_SOURCE) (printer: LONG) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinter.Value.Invoke(source, printer, bcurrent)

    let DTWAIN_SetPrinterEx (source: DTWAIN_SOURCE) (printer: LONG) (bcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinterEx.Value.Invoke(source, printer, bcurrent)

    let DTWAIN_SetPrinterStartNumber (source: DTWAIN_SOURCE) (nstart: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinterStartNumber.Value.Invoke(source, nstart)

    let DTWAIN_SetPrinterStringMode (source: DTWAIN_SOURCE) (printermode: LONG) (bsetcurrent: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinterStringMode.Value.Invoke(source, printermode, bsetcurrent)

    let DTWAIN_SetPrinterStrings (source: DTWAIN_SOURCE) (arraystring: DTWAIN_ARRAY) (pnumstrings: int byref) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinterStrings.Value.Invoke(source, arraystring, &pnumstrings)

    let DTWAIN_SetPrinterSuffixString (source: DTWAIN_SOURCE) (suffix: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinterSuffixString.Value.Invoke(source, suffix)

    let DTWAIN_SetPrinterSuffixStringA (source: DTWAIN_SOURCE) (suffix: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinterSuffixStringA.Value.Invoke(source, suffix)

    let DTWAIN_SetPrinterSuffixStringW (source: DTWAIN_SOURCE) (suffix: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetPrinterSuffixStringW.Value.Invoke(source, suffix)

    let DTWAIN_SetQueryCapSupport (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetQueryCapSupport.Value.Invoke(bset)

    let DTWAIN_SetResolution (source: DTWAIN_SOURCE) (resolution: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetResolution.Value.Invoke(source, resolution)

    let DTWAIN_SetResolutionString (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetResolutionString.Value.Invoke(source, resolution)

    let DTWAIN_SetResolutionStringA (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetResolutionStringA.Value.Invoke(source, resolution)

    let DTWAIN_SetResolutionStringW (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetResolutionStringW.Value.Invoke(source, resolution)

    let DTWAIN_SetResourcePath (resourcepath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetResourcePath.Value.Invoke(resourcepath)

    let DTWAIN_SetResourcePathA (resourcepath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetResourcePathA.Value.Invoke(resourcepath)

    let DTWAIN_SetResourcePathW (resourcepath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetResourcePathW.Value.Invoke(resourcepath)

    let DTWAIN_SetRotation (source: DTWAIN_SOURCE) (rotation: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetRotation.Value.Invoke(source, rotation)

    let DTWAIN_SetRotationString (source: DTWAIN_SOURCE) (rotation: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetRotationString.Value.Invoke(source, rotation)

    let DTWAIN_SetRotationStringA (source: DTWAIN_SOURCE) (rotation: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetRotationStringA.Value.Invoke(source, rotation)

    let DTWAIN_SetRotationStringW (source: DTWAIN_SOURCE) (rotation: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetRotationStringW.Value.Invoke(source, rotation)

    let DTWAIN_SetSaveFileName (source: DTWAIN_SOURCE) (fname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetSaveFileName.Value.Invoke(source, fname)

    let DTWAIN_SetSaveFileNameA (source: DTWAIN_SOURCE) (fname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetSaveFileNameA.Value.Invoke(source, fname)

    let DTWAIN_SetSaveFileNameW (source: DTWAIN_SOURCE) (fname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetSaveFileNameW.Value.Invoke(source, fname)

    let DTWAIN_SetShadow (source: DTWAIN_SOURCE) (shadow: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetShadow.Value.Invoke(source, shadow)

    let DTWAIN_SetShadowString (source: DTWAIN_SOURCE) (shadow: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetShadowString.Value.Invoke(source, shadow)

    let DTWAIN_SetShadowStringA (source: DTWAIN_SOURCE) (shadow: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetShadowStringA.Value.Invoke(source, shadow)

    let DTWAIN_SetShadowStringW (source: DTWAIN_SOURCE) (shadow: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetShadowStringW.Value.Invoke(source, shadow)

    let DTWAIN_SetSourceUnit (source: DTWAIN_SOURCE) (unit: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetSourceUnit.Value.Invoke(source, unit)

    let DTWAIN_SetTIFFCompressType (source: DTWAIN_SOURCE) (setting: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTIFFCompressType.Value.Invoke(source, setting)

    let DTWAIN_SetTIFFInvert (source: DTWAIN_SOURCE) (setting: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTIFFInvert.Value.Invoke(source, setting)

    let DTWAIN_SetTempFileDirectory (szfilepath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTempFileDirectory.Value.Invoke(szfilepath)

    let DTWAIN_SetTempFileDirectoryA (szfilepath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTempFileDirectoryA.Value.Invoke(szfilepath)

    let DTWAIN_SetTempFileDirectoryEx (szfilepath: string) (creationflags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTempFileDirectoryEx.Value.Invoke(szfilepath, creationflags)

    let DTWAIN_SetTempFileDirectoryExA (szfilepath: string) (creationflags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTempFileDirectoryExA.Value.Invoke(szfilepath, creationflags)

    let DTWAIN_SetTempFileDirectoryExW (szfilepath: string) (creationflags: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTempFileDirectoryExW.Value.Invoke(szfilepath, creationflags)

    let DTWAIN_SetTempFileDirectoryW (szfilepath: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTempFileDirectoryW.Value.Invoke(szfilepath)

    let DTWAIN_SetThreshold (source: DTWAIN_SOURCE) (threshold: DTWAIN_FLOAT) (bsetbithdepthreduction: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetThreshold.Value.Invoke(source, threshold, bsetbithdepthreduction)

    let DTWAIN_SetThresholdString (source: DTWAIN_SOURCE) (threshold: string) (bsetbitdepthreduction: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetThresholdString.Value.Invoke(source, threshold, bsetbitdepthreduction)

    let DTWAIN_SetThresholdStringA (source: DTWAIN_SOURCE) (threshold: string) (bsetbitdepthreduction: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetThresholdStringA.Value.Invoke(source, threshold, bsetbitdepthreduction)

    let DTWAIN_SetThresholdStringW (source: DTWAIN_SOURCE) (threshold: string) (bsetbitdepthreduction: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetThresholdStringW.Value.Invoke(source, threshold, bsetbitdepthreduction)

    let DTWAIN_SetTwainDSM (dsmtype: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTwainDSM.Value.Invoke(dsmtype)

    let DTWAIN_SetTwainLog (logflags: DWORD) (lpszlogfile: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTwainLog.Value.Invoke(logflags, lpszlogfile)

    let DTWAIN_SetTwainLogA (logflags: DWORD) (lpszlogfile: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTwainLogA.Value.Invoke(logflags, lpszlogfile)

    let DTWAIN_SetTwainLogW (logflags: DWORD) (lpszlogfile: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTwainLogW.Value.Invoke(logflags, lpszlogfile)

    let DTWAIN_SetTwainMode (lacquiremode: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTwainMode.Value.Invoke(lacquiremode)

    let DTWAIN_SetTwainTimeout (milliseconds: LONG) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetTwainTimeout.Value.Invoke(milliseconds)

    let DTWAIN_SetXResolution (source: DTWAIN_SOURCE) (xresolution: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetXResolution.Value.Invoke(source, xresolution)

    let DTWAIN_SetXResolutionString (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetXResolutionString.Value.Invoke(source, resolution)

    let DTWAIN_SetXResolutionStringA (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetXResolutionStringA.Value.Invoke(source, resolution)

    let DTWAIN_SetXResolutionStringW (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetXResolutionStringW.Value.Invoke(source, resolution)

    let DTWAIN_SetYResolution (source: DTWAIN_SOURCE) (yresolution: DTWAIN_FLOAT) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetYResolution.Value.Invoke(source, yresolution)

    let DTWAIN_SetYResolutionString (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetYResolutionString.Value.Invoke(source, resolution)

    let DTWAIN_SetYResolutionStringA (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetYResolutionStringA.Value.Invoke(source, resolution)

    let DTWAIN_SetYResolutionStringW (source: DTWAIN_SOURCE) (resolution: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SetYResolutionStringW.Value.Invoke(source, resolution)

    let DTWAIN_ShowUIOnly (source: DTWAIN_SOURCE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ShowUIOnly.Value.Invoke(source)

    let DTWAIN_ShutdownOCREngine (ocrengine: DTWAIN_OCRENGINE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        ShutdownOCREngine.Value.Invoke(ocrengine)

    let DTWAIN_SkipImageInfoError (source: DTWAIN_SOURCE) (bskip: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SkipImageInfoError.Value.Invoke(source, bskip)

    let DTWAIN_StartThread (dllhandle: DTWAIN_HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        StartThread.Value.Invoke(dllhandle)

    let DTWAIN_StartTwainSession (hwndmsg: HWND) (lpszdllname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        StartTwainSession.Value.Invoke(hwndmsg, lpszdllname)

    let DTWAIN_StartTwainSessionA (hwndmsg: HWND) (lpszdllname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        StartTwainSessionA.Value.Invoke(hwndmsg, lpszdllname)

    let DTWAIN_StartTwainSessionW (hwndmsg: HWND) (lpszdllname: string) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        StartTwainSessionW.Value.Invoke(hwndmsg, lpszdllname)

    let DTWAIN_SysDestroy() : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysDestroy.Value.Invoke()

    let DTWAIN_SysInitialize() : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitialize.Value.Invoke()

    let DTWAIN_SysInitializeEx (szinipath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeEx.Value.Invoke(szinipath)

    let DTWAIN_SysInitializeEx2 (szinipath: string) (szimagedllpath: string) (szlangresourcepath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeEx2.Value.Invoke(szinipath, szimagedllpath, szlangresourcepath)

    let DTWAIN_SysInitializeEx2A (szinipath: string) (szimagedllpath: string) (szlangresourcepath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeEx2A.Value.Invoke(szinipath, szimagedllpath, szlangresourcepath)

    let DTWAIN_SysInitializeEx2W (szinipath: string) (szimagedllpath: string) (szlangresourcepath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeEx2W.Value.Invoke(szinipath, szimagedllpath, szlangresourcepath)

    let DTWAIN_SysInitializeExA (szinipath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeExA.Value.Invoke(szinipath)

    let DTWAIN_SysInitializeExW (szinipath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeExW.Value.Invoke(szinipath)

    let DTWAIN_SysInitializeLib (hinstance: HINSTANCE) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeLib.Value.Invoke(hinstance)

    let DTWAIN_SysInitializeLibEx (hinstance: HINSTANCE) (szinipath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeLibEx.Value.Invoke(hinstance, szinipath)

    let DTWAIN_SysInitializeLibEx2 (hinstance: HINSTANCE) (szinipath: string) (szimagedllpath: string) (szlangresourcepath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeLibEx2.Value.Invoke(hinstance, szinipath, szimagedllpath, szlangresourcepath)

    let DTWAIN_SysInitializeLibEx2A (hinstance: HINSTANCE) (szinipath: string) (szimagedllpath: string) (szlangresourcepath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeLibEx2A.Value.Invoke(hinstance, szinipath, szimagedllpath, szlangresourcepath)

    let DTWAIN_SysInitializeLibEx2W (hinstance: HINSTANCE) (szinipath: string) (szimagedllpath: string) (szlangresourcepath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeLibEx2W.Value.Invoke(hinstance, szinipath, szimagedllpath, szlangresourcepath)

    let DTWAIN_SysInitializeLibExA (hinstance: HINSTANCE) (szinipath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeLibExA.Value.Invoke(hinstance, szinipath)

    let DTWAIN_SysInitializeLibExW (hinstance: HINSTANCE) (szinipath: string) : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeLibExW.Value.Invoke(hinstance, szinipath)

    let DTWAIN_SysInitializeNoBlocking() : DTWAIN_HANDLE =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        SysInitializeNoBlocking.Value.Invoke()

    let DTWAIN_TestGetCap (source: DTWAIN_SOURCE) (lcapability: LONG) : DTWAIN_ARRAY =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        TestGetCap.Value.Invoke(source, lcapability)

    let DTWAIN_UnlockMemory (h: HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        UnlockMemory.Value.Invoke(h)

    let DTWAIN_UnlockMemoryEx (h: HANDLE) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        UnlockMemoryEx.Value.Invoke(h)

    let DTWAIN_UseMultipleThreads (bset: DTWAIN_BOOL) : DTWAIN_BOOL =
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        UseMultipleThreads.Value.Invoke(bset)
    let mutable private pinnedCallback_SetCallback : DTWAIN_CALLBACK_PROC option = None

    let private SetCallback : Lazy<DTWAIN_SetCallbackDelegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetCallback")

    let DTWAIN_SetCallback(fn: DTWAIN_CALLBACK_PROC) (userdata: LONG) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetCallback <- Some fn
        SetCallback.Value.Invoke(fn, userdata)

    let mutable private pinnedCallback_SetCallback64 : DTWAIN_CALLBACK_PROC64 option = None

    let private SetCallback64 : Lazy<DTWAIN_SetCallback64Delegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetCallback64")

    let DTWAIN_SetCallback64(fn: DTWAIN_CALLBACK_PROC64) (userdata: DTWAIN_LONG64) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetCallback64 <- Some fn
        SetCallback64.Value.Invoke(fn, userdata)

    let mutable private pinnedCallback_SetErrorCallback : DTWAIN_ERROR_PROC option = None

    let private SetErrorCallback : Lazy<DTWAIN_SetErrorCallbackDelegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetErrorCallback")

    let DTWAIN_SetErrorCallback(proc: DTWAIN_ERROR_PROC) (userdata: LONG) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetErrorCallback <- Some proc
        SetErrorCallback.Value.Invoke(proc, userdata)

    let mutable private pinnedCallback_SetErrorCallback64 : DTWAIN_ERROR_PROC64 option = None

    let private SetErrorCallback64 : Lazy<DTWAIN_SetErrorCallback64Delegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetErrorCallback64")

    let DTWAIN_SetErrorCallback64(proc: DTWAIN_ERROR_PROC64) (userdata64: DTWAIN_LONG64) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetErrorCallback64 <- Some proc
        SetErrorCallback64.Value.Invoke(proc, userdata64)

    let mutable private pinnedCallback_SetLoggerCallback : DTWAIN_LOGGER_PROC option = None

    let private SetLoggerCallback : Lazy<DTWAIN_SetLoggerCallbackDelegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetLoggerCallback")

    let DTWAIN_SetLoggerCallback(logproc: DTWAIN_LOGGER_PROC) (userdata: DTWAIN_LONG64) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetLoggerCallback <- Some logproc
        SetLoggerCallback.Value.Invoke(logproc, userdata)

    let mutable private pinnedCallback_SetLoggerCallbackA : DTWAIN_LOGGER_PROCA option = None

    let private SetLoggerCallbackA : Lazy<DTWAIN_SetLoggerCallbackADelegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetLoggerCallbackA")

    let DTWAIN_SetLoggerCallbackA(logproc: DTWAIN_LOGGER_PROCA) (userdata: DTWAIN_LONG64) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetLoggerCallbackA <- Some logproc
        SetLoggerCallbackA.Value.Invoke(logproc, userdata)

    let mutable private pinnedCallback_SetLoggerCallbackW : DTWAIN_LOGGER_PROCW option = None

    let private SetLoggerCallbackW : Lazy<DTWAIN_SetLoggerCallbackWDelegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetLoggerCallbackW")

    let DTWAIN_SetLoggerCallbackW(logproc: DTWAIN_LOGGER_PROCW) (userdata: DTWAIN_LONG64) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetLoggerCallbackW <- Some logproc
        SetLoggerCallbackW.Value.Invoke(logproc, userdata)

    let mutable private pinnedCallback_SetUpdateDibProc : DTWAIN_DIBUPDATE_PROC option = None

    let private SetUpdateDibProc : Lazy<DTWAIN_SetUpdateDibProcDelegate> = 
        lazy (DynamicDll. Bind "DTWAIN_SetUpdateDibProc")

    let DTWAIN_SetUpdateDibProc(dibproc: DTWAIN_DIBUPDATE_PROC) : nativeint = 
        if not IsLoaded then failwith "Call TwainAPI.Load first"
        pinnedCallback_SetUpdateDibProc <- Some dibproc
        SetUpdateDibProc.Value.Invoke(dibproc)
