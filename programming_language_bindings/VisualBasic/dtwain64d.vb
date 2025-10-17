REM
REM This file is part of the Dynarithmic TWAIN Library (DTWAIN).                          
REM Copyright (c) 2002-2025 Dynarithmic Software.                                         
REM                                                                                       
REM Licensed under the Apache License, Version 2.0 (the "License");                       
REM you may not use this file except in compliance with the License.                      
REM You may obtain a copy of the License at                                               
REM                                                                                       
REM     http://www.apache.org/licenses/LICENSE-2.0                                        
REM                                                                                       
REM Unless required by applicable law or agreed to in writing, software                   
REM distributed under the License is distributed on an "AS IS" BASIS,                     
REM WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.              
REM See the License for the specific language governing permissions and                   
REM limitations under the License.                                                        
REM                                                                                       
REM FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY                   
REM DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT 
REM OF THIRD PARTY RIGHTS.                                                                
REM

REM  The DTWAIN VB .NET class is called DTWAINAPI

Imports System.Runtime.InteropServices
Imports System.Text

Class DTWAINAPI
    Structure DTWAIN_POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=8)>
    Structure WinMsg
        Dim hwnd As Integer
        Dim message As Integer
        Dim wParam As Integer
        Dim lParam As Integer
        Dim time As Integer
        Dim pt As DTWAIN_POINTAPI
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=2)>
    Structure TW_VERSION
        Dim MajorNum As System.UInt16
        Dim MinorNum As System.UInt16
        Dim Language As System.UInt16
        Dim Country As System.UInt16

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
        Dim Info As String
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=2)>
    Structure TW_IDENTITY
        Dim Id As System.UInt32
        Dim Version As TW_VERSION
        Dim ProtocolMajor As System.UInt16
        Dim ProtocolMinor As System.UInt16
        Dim SupportedGroups As System.UInt32

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
        Dim Manufacturer As String

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
        Dim ProductFamily As String

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
        Dim ProductName As String
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Structure OPENFILENAME
        Dim lStructSize As Long
        Dim hwndOwner As Long
        Dim hInstance As Long
        Dim lpstrFilter As String
        Dim lpstrCustomFilter As String
        Dim nMaxCustFilter As Long
        Dim nFilterIndex As Long
        Dim lpstrFile As String
        Dim nMaxFile As Long
        Dim lpstrFileTitle As String
        Dim nMaxFileTitle As Long
        Dim lpstrInitialDir As String
        Dim lpstrTitle As String
        Dim flags As Long
        Dim nFileOffset As Integer
        Dim nFileExtension As Integer
        Dim lpstrDefExt As String
        Dim lCustData As Long
        Dim lpfnHook As Long
        Dim lpTemplateName As String
    End Structure

    Public Const DTWAIN_FF_TIFF As Integer = 0
    Public Const DTWAIN_FF_PICT As Integer = 1
    Public Const DTWAIN_FF_BMP As Integer = 2
    Public Const DTWAIN_FF_XBM As Integer = 3
    Public Const DTWAIN_FF_JFIF As Integer = 4
    Public Const DTWAIN_FF_FPX As Integer = 5
    Public Const DTWAIN_FF_TIFFMULTI As Integer = 6
    Public Const DTWAIN_FF_PNG As Integer = 7
    Public Const DTWAIN_FF_SPIFF As Integer = 8
    Public Const DTWAIN_FF_EXIF As Integer = 9
    Public Const DTWAIN_FF_PDF As Integer = 10
    Public Const DTWAIN_FF_JP2 As Integer = 11
    Public Const DTWAIN_FF_JPX As Integer = 13
    Public Const DTWAIN_FF_DEJAVU As Integer = 14
    Public Const DTWAIN_FF_PDFA As Integer = 15
    Public Const DTWAIN_FF_PDFA2 As Integer = 16
    Public Const DTWAIN_FF_PDFRASTER As Integer = 17
    Public Const DTWAIN_CP_NONE As Integer = 0
    Public Const DTWAIN_CP_PACKBITS As Integer = 1
    Public Const DTWAIN_CP_GROUP31D As Integer = 2
    Public Const DTWAIN_CP_GROUP31DEOL As Integer = 3
    Public Const DTWAIN_CP_GROUP32D As Integer = 4
    Public Const DTWAIN_CP_GROUP4 As Integer = 5
    Public Const DTWAIN_CP_JPEG As Integer = 6
    Public Const DTWAIN_CP_LZW As Integer = 7
    Public Const DTWAIN_CP_JBIG As Integer = 8
    Public Const DTWAIN_CP_PNG As Integer = 9
    Public Const DTWAIN_CP_RLE4 As Integer = 10
    Public Const DTWAIN_CP_RLE8 As Integer = 11
    Public Const DTWAIN_CP_BITFIELDS As Integer = 12
    Public Const DTWAIN_CP_ZIP As Integer = 13
    Public Const DTWAIN_CP_JPEG2000 As Integer = 14
    Public Const DTWAIN_FS_NONE As Integer = 0
    Public Const DTWAIN_FS_A4LETTER As Integer = 1
    Public Const DTWAIN_FS_B5LETTER As Integer = 2
    Public Const DTWAIN_FS_USLETTER As Integer = 3
    Public Const DTWAIN_FS_USLEGAL As Integer = 4
    Public Const DTWAIN_FS_A5 As Integer = 5
    Public Const DTWAIN_FS_B4 As Integer = 6
    Public Const DTWAIN_FS_B6 As Integer = 7
    Public Const DTWAIN_FS_USLEDGER As Integer = 9
    Public Const DTWAIN_FS_USEXECUTIVE As Integer = 10
    Public Const DTWAIN_FS_A3 As Integer = 11
    Public Const DTWAIN_FS_B3 As Integer = 12
    Public Const DTWAIN_FS_A6 As Integer = 13
    Public Const DTWAIN_FS_C4 As Integer = 14
    Public Const DTWAIN_FS_C5 As Integer = 15
    Public Const DTWAIN_FS_C6 As Integer = 16
    Public Const DTWAIN_FS_4A0 As Integer = 17
    Public Const DTWAIN_FS_2A0 As Integer = 18
    Public Const DTWAIN_FS_A0 As Integer = 19
    Public Const DTWAIN_FS_A1 As Integer = 20
    Public Const DTWAIN_FS_A2 As Integer = 21
    Public Const DTWAIN_FS_A4 As Integer = DTWAIN_FS_A4LETTER
    Public Const DTWAIN_FS_A7 As Integer = 22
    Public Const DTWAIN_FS_A8 As Integer = 23
    Public Const DTWAIN_FS_A9 As Integer = 24
    Public Const DTWAIN_FS_A10 As Integer = 25
    Public Const DTWAIN_FS_ISOB0 As Integer = 26
    Public Const DTWAIN_FS_ISOB1 As Integer = 27
    Public Const DTWAIN_FS_ISOB2 As Integer = 28
    Public Const DTWAIN_FS_ISOB3 As Integer = DTWAIN_FS_B3
    Public Const DTWAIN_FS_ISOB4 As Integer = DTWAIN_FS_B4
    Public Const DTWAIN_FS_ISOB5 As Integer = 29
    Public Const DTWAIN_FS_ISOB6 As Integer = DTWAIN_FS_B6
    Public Const DTWAIN_FS_ISOB7 As Integer = 30
    Public Const DTWAIN_FS_ISOB8 As Integer = 31
    Public Const DTWAIN_FS_ISOB9 As Integer = 32
    Public Const DTWAIN_FS_ISOB10 As Integer = 33
    Public Const DTWAIN_FS_JISB0 As Integer = 34
    Public Const DTWAIN_FS_JISB1 As Integer = 35
    Public Const DTWAIN_FS_JISB2 As Integer = 36
    Public Const DTWAIN_FS_JISB3 As Integer = 37
    Public Const DTWAIN_FS_JISB4 As Integer = 38
    Public Const DTWAIN_FS_JISB5 As Integer = DTWAIN_FS_B5LETTER
    Public Const DTWAIN_FS_JISB6 As Integer = 39
    Public Const DTWAIN_FS_JISB7 As Integer = 40
    Public Const DTWAIN_FS_JISB8 As Integer = 41
    Public Const DTWAIN_FS_JISB9 As Integer = 42
    Public Const DTWAIN_FS_JISB10 As Integer = 43
    Public Const DTWAIN_FS_C0 As Integer = 44
    Public Const DTWAIN_FS_C1 As Integer = 45
    Public Const DTWAIN_FS_C2 As Integer = 46
    Public Const DTWAIN_FS_C3 As Integer = 47
    Public Const DTWAIN_FS_C7 As Integer = 48
    Public Const DTWAIN_FS_C8 As Integer = 49
    Public Const DTWAIN_FS_C9 As Integer = 50
    Public Const DTWAIN_FS_C10 As Integer = 51
    Public Const DTWAIN_FS_USSTATEMENT As Integer = 52
    Public Const DTWAIN_FS_BUSINESSCARD As Integer = 53
    Public Const DTWAIN_ANYSUPPORT As Integer = (-1)
    Public Const DTWAIN_BMP As Integer = 100
    Public Const DTWAIN_JPEG As Integer = 200
    Public Const DTWAIN_PDF As Integer = 250
    Public Const DTWAIN_PDFMULTI As Integer = 251
    Public Const DTWAIN_PCX As Integer = 300
    Public Const DTWAIN_DCX As Integer = 301
    Public Const DTWAIN_TGA As Integer = 400
    Public Const DTWAIN_TIFFLZW As Integer = 500
    Public Const DTWAIN_TIFFNONE As Integer = 600
    Public Const DTWAIN_TIFFG3 As Integer = 700
    Public Const DTWAIN_TIFFG4 As Integer = 800
    Public Const DTWAIN_TIFFPACKBITS As Integer = 801
    Public Const DTWAIN_TIFFDEFLATE As Integer = 802
    Public Const DTWAIN_TIFFJPEG As Integer = 803
    Public Const DTWAIN_TIFFJBIG As Integer = 804
    Public Const DTWAIN_TIFFPIXARLOG As Integer = 805
    Public Const DTWAIN_TIFFNONEMULTI As Integer = 900
    Public Const DTWAIN_TIFFG3MULTI As Integer = 901
    Public Const DTWAIN_TIFFG4MULTI As Integer = 902
    Public Const DTWAIN_TIFFPACKBITSMULTI As Integer = 903
    Public Const DTWAIN_TIFFDEFLATEMULTI As Integer = 904
    Public Const DTWAIN_TIFFJPEGMULTI As Integer = 905
    Public Const DTWAIN_TIFFLZWMULTI As Integer = 906
    Public Const DTWAIN_TIFFJBIGMULTI As Integer = 907
    Public Const DTWAIN_TIFFPIXARLOGMULTI As Integer = 908
    Public Const DTWAIN_WMF As Integer = 850
    Public Const DTWAIN_EMF As Integer = 851
    Public Const DTWAIN_GIF As Integer = 950
    Public Const DTWAIN_PNG As Integer = 1000
    Public Const DTWAIN_PSD As Integer = 2000
    Public Const DTWAIN_JPEG2000 As Integer = 3000
    Public Const DTWAIN_POSTSCRIPT1 As Integer = 4000
    Public Const DTWAIN_POSTSCRIPT2 As Integer = 4001
    Public Const DTWAIN_POSTSCRIPT3 As Integer = 4002
    Public Const DTWAIN_POSTSCRIPT1MULTI As Integer = 4003
    Public Const DTWAIN_POSTSCRIPT2MULTI As Integer = 4004
    Public Const DTWAIN_POSTSCRIPT3MULTI As Integer = 4005
    Public Const DTWAIN_TEXT As Integer = 6000
    Public Const DTWAIN_TEXTMULTI As Integer = 6001
    Public Const DTWAIN_TIFFMULTI As Integer = 7000
    Public Const DTWAIN_ICO As Integer = 8000
    Public Const DTWAIN_ICO_VISTA As Integer = 8001
    Public Const DTWAIN_ICO_RESIZED As Integer = 8002
    Public Const DTWAIN_WBMP As Integer = 8500
    Public Const DTWAIN_WEBP As Integer = 8501
    Public Const DTWAIN_PPM As Integer = 10000
    Public Const DTWAIN_WBMP_RESIZED As Integer = 11000
    Public Const DTWAIN_TGA_RLE As Integer = 11001
    Public Const DTWAIN_BMP_RLE As Integer = 11002
    Public Const DTWAIN_BIGTIFFLZW As Integer = 11003
    Public Const DTWAIN_BIGTIFFLZWMULTI As Integer = 11004
    Public Const DTWAIN_BIGTIFFNONE As Integer = 11005
    Public Const DTWAIN_BIGTIFFNONEMULTI As Integer = 11006
    Public Const DTWAIN_BIGTIFFPACKBITS As Integer = 11007
    Public Const DTWAIN_BIGTIFFPACKBITSMULTI As Integer = 11008
    Public Const DTWAIN_BIGTIFFDEFLATE As Integer = 11009
    Public Const DTWAIN_BIGTIFFDEFLATEMULTI As Integer = 11010
    Public Const DTWAIN_BIGTIFFG3 As Integer = 11011
    Public Const DTWAIN_BIGTIFFG3MULTI As Integer = 11012
    Public Const DTWAIN_BIGTIFFG4 As Integer = 11013
    Public Const DTWAIN_BIGTIFFG4MULTI As Integer = 11014
    Public Const DTWAIN_BIGTIFFJPEG As Integer = 11015
    Public Const DTWAIN_BIGTIFFJPEGMULTI As Integer = 11016
    Public Const DTWAIN_JPEGXR As Integer = 12000
    Public Const DTWAIN_INCHES As Integer = 0
    Public Const DTWAIN_CENTIMETERS As Integer = 1
    Public Const DTWAIN_PICAS As Integer = 2
    Public Const DTWAIN_POINTS As Integer = 3
    Public Const DTWAIN_TWIPS As Integer = 4
    Public Const DTWAIN_PIXELS As Integer = 5
    Public Const DTWAIN_MILLIMETERS As Integer = 6
    Public Const DTWAIN_USENAME As Integer = 16
    Public Const DTWAIN_USEPROMPT As Integer = 32
    Public Const DTWAIN_USELONGNAME As Integer = 64
    Public Const DTWAIN_USESOURCEMODE As Integer = 128
    Public Const DTWAIN_USELIST As Integer = 256
    Public Const DTWAIN_CREATE_DIRECTORY As Integer = 512
    Public Const DTWAIN_CREATEDIRECTORY As Integer = DTWAIN_CREATE_DIRECTORY
    Public Const DTWAIN_ARRAYANY As Integer = 1
    Public Const DTWAIN_ArrayTypePTR As Integer = 1
    Public Const DTWAIN_ARRAYLONG As Integer = 2
    Public Const DTWAIN_ARRAYFLOAT As Integer = 3
    Public Const DTWAIN_ARRAYHANDLE As Integer = 4
    Public Const DTWAIN_ARRAYSOURCE As Integer = 5
    Public Const DTWAIN_ARRAYSTRING As Integer = 6
    Public Const DTWAIN_ARRAYFRAME As Integer = 7
    Public Const DTWAIN_ARRAYBOOL As Integer = DTWAIN_ARRAYLONG
    Public Const DTWAIN_ARRAYLONGSTRING As Integer = 8
    Public Const DTWAIN_ARRAYUNICODESTRING As Integer = 9
    Public Const DTWAIN_ARRAYLONG64 As Integer = 10
    Public Const DTWAIN_ARRAYANSISTRING As Integer = 11
    Public Const DTWAIN_ARRAYWIDESTRING As Integer = 12
    Public Const DTWAIN_ARRAYTWFIX32 As Integer = 200
    Public Const DTWAIN_ArrayTypeINVALID As Integer = 0
    Public Const DTWAIN_ARRAYINT16 As Integer = 100
    Public Const DTWAIN_ARRAYUINT16 As Integer = 110
    Public Const DTWAIN_ARRAYUINT32 As Integer = 120
    Public Const DTWAIN_ARRAYINT32 As Integer = 130
    Public Const DTWAIN_ARRAYINT64 As Integer = 140
    Public Const DTWAIN_ARRAYUINT64 As Integer = 150
    Public Const DTWAIN_RANGELONG As Integer = DTWAIN_ARRAYLONG
    Public Const DTWAIN_RANGEFLOAT As Integer = DTWAIN_ARRAYFLOAT
    Public Const DTWAIN_RANGEMIN As Integer = 0
    Public Const DTWAIN_RANGEMAX As Integer = 1
    Public Const DTWAIN_RANGESTEP As Integer = 2
    Public Const DTWAIN_RANGEDEFAULT As Integer = 3
    Public Const DTWAIN_RANGECURRENT As Integer = 4
    Public Const DTWAIN_FRAMELEFT As Integer = 0
    Public Const DTWAIN_FRAMETOP As Integer = 1
    Public Const DTWAIN_FRAMERIGHT As Integer = 2
    Public Const DTWAIN_FRAMEBOTTOM As Integer = 3
    Public Const DTWAIN_FIX32WHOLE As Integer = 0
    Public Const DTWAIN_FIX32FRAC As Integer = 1
    Public Const DTWAIN_JC_NONE As Integer = 0
    Public Const DTWAIN_JC_JSIC As Integer = 1
    Public Const DTWAIN_JC_JSIS As Integer = 2
    Public Const DTWAIN_JC_JSXC As Integer = 3
    Public Const DTWAIN_JC_JSXS As Integer = 4
    Public Const DTWAIN_CAPDATATYPE_UNKNOWN As Integer = (-10)
    Public Const DTWAIN_JCBP_JSIC As Integer = 5
    Public Const DTWAIN_JCBP_JSIS As Integer = 6
    Public Const DTWAIN_JCBP_JSXC As Integer = 7
    Public Const DTWAIN_JCBP_JSXS As Integer = 8
    Public Const DTWAIN_FEEDPAGEON As Integer = 1
    Public Const DTWAIN_CLEARPAGEON As Integer = 2
    Public Const DTWAIN_REWINDPAGEON As Integer = 4
    Public Const DTWAIN_AppOwnsDib As Integer = 1
    Public Const DTWAIN_SourceOwnsDib As Integer = 2
    Public Const DTWAIN_CONTARRAY As Integer = 8
    Public Const DTWAIN_CONTENUMERATION As Integer = 16
    Public Const DTWAIN_CONTONEVALUE As Integer = 32
    Public Const DTWAIN_CONTRANGE As Integer = 64
    Public Const DTWAIN_CONTDEFAULT As Integer = 0
    Public Const DTWAIN_CAPGET As Integer = 1
    Public Const DTWAIN_CAPGETCURRENT As Integer = 2
    Public Const DTWAIN_CAPGETDEFAULT As Integer = 3
    Public Const DTWAIN_CAPSET As Integer = 6
    Public Const DTWAIN_CAPRESET As Integer = 7
    Public Const DTWAIN_CAPRESETALL As Integer = 8
    Public Const DTWAIN_CAPSETCONSTRAINT As Integer = 9
    Public Const DTWAIN_CAPSETAVAILABLE As Integer = 8
    Public Const DTWAIN_CAPSETCURRENT As Integer = 16
    Public Const DTWAIN_CAPGETHELP As Integer = 9
    Public Const DTWAIN_CAPGETLABEL As Integer = 10
    Public Const DTWAIN_CAPGETLABELENUM As Integer = 11
    Public Const DTWAIN_AREASET As Integer = DTWAIN_CAPSET
    Public Const DTWAIN_AREARESET As Integer = DTWAIN_CAPRESET
    Public Const DTWAIN_AREACURRENT As Integer = DTWAIN_CAPGETCURRENT
    Public Const DTWAIN_AREADEFAULT As Integer = DTWAIN_CAPGETDEFAULT
    Public Const DTWAIN_VER15 As Integer = 0
    Public Const DTWAIN_VER16 As Integer = 1
    Public Const DTWAIN_VER17 As Integer = 2
    Public Const DTWAIN_VER18 As Integer = 3
    Public Const DTWAIN_VER20 As Integer = 4
    Public Const DTWAIN_VER21 As Integer = 5
    Public Const DTWAIN_VER22 As Integer = 6
    Public Const DTWAIN_ACQUIREALL As Integer = (-1)
    Public Const DTWAIN_MAXACQUIRE As Integer = (-1)
    Public Const DTWAIN_DX_NONE As Integer = 0
    Public Const DTWAIN_DX_1PASSDUPLEX As Integer = 1
    Public Const DTWAIN_DX_2PASSDUPLEX As Integer = 2
    Public Const DTWAIN_PT_BW As Integer = 0
    Public Const DTWAIN_PT_GRAY As Integer = 1
    Public Const DTWAIN_PT_RGB As Integer = 2
    Public Const DTWAIN_PT_PALETTE As Integer = 3
    Public Const DTWAIN_PT_CMY As Integer = 4
    Public Const DTWAIN_PT_CMYK As Integer = 5
    Public Const DTWAIN_PT_YUV As Integer = 6
    Public Const DTWAIN_PT_YUVK As Integer = 7
    Public Const DTWAIN_PT_CIEXYZ As Integer = 8
    Public Const DTWAIN_PT_DEFAULT As Integer = 1000
    Public Const DTWAIN_CURRENT As Integer = (-2)
    Public Const DTWAIN_DEFAULT As Integer = (-1)
    Public Const DTWAIN_FLOATDEFAULT As Double = (-9999.0)
    Public Const DTWAIN_CallbackERROR As Integer = 1
    Public Const DTWAIN_CallbackMESSAGE As Integer = 2
    Public Const DTWAIN_USENATIVE As Integer = 1
    Public Const DTWAIN_USEBUFFERED As Integer = 2
    Public Const DTWAIN_USECOMPRESSION As Integer = 4
    Public Const DTWAIN_USEMEMFILE As Integer = 8
    Public Const DTWAIN_FAILURE1 As Integer = (-1)
    Public Const DTWAIN_FAILURE2 As Integer = (-2)
    Public Const DTWAIN_DELETEALL As Integer = (-1)
    Public Const DTWAIN_TN_ACQUIREDONE As Integer = 1000
    Public Const DTWAIN_TN_ACQUIREFAILED As Integer = 1001
    Public Const DTWAIN_TN_ACQUIRECANCELLED As Integer = 1002
    Public Const DTWAIN_TN_ACQUIRESTARTED As Integer = 1003
    Public Const DTWAIN_TN_PAGECONTINUE As Integer = 1004
    Public Const DTWAIN_TN_PAGEFAILED As Integer = 1005
    Public Const DTWAIN_TN_PAGECANCELLED As Integer = 1006
    Public Const DTWAIN_TN_TRANSFERREADY As Integer = 1009
    Public Const DTWAIN_TN_TRANSFERDONE As Integer = 1010
    Public Const DTWAIN_TN_ACQUIREPAGEDONE As Integer = 1010
    Public Const DTWAIN_TN_UICLOSING As Integer = 3000
    Public Const DTWAIN_TN_UICLOSED As Integer = 3001
    Public Const DTWAIN_TN_UIOPENED As Integer = 3002
    Public Const DTWAIN_TN_UIOPENING As Integer = 3003
    Public Const DTWAIN_TN_UIOPENFAILURE As Integer = 3004
    Public Const DTWAIN_TN_CLIPTRANSFERDONE As Integer = 1014
    Public Const DTWAIN_TN_INVALIDIMAGEFORMAT As Integer = 1015
    Public Const DTWAIN_TN_ACQUIRETERMINATED As Integer = 1021
    Public Const DTWAIN_TN_TRANSFERSTRIPREADY As Integer = 1022
    Public Const DTWAIN_TN_TRANSFERSTRIPDONE As Integer = 1023
    Public Const DTWAIN_TN_TRANSFERSTRIPFAILED As Integer = 1029
    Public Const DTWAIN_TN_IMAGEINFOERROR As Integer = 1024
    Public Const DTWAIN_TN_TRANSFERCANCELLED As Integer = 1030
    Public Const DTWAIN_TN_FILESAVECANCELLED As Integer = 1031
    Public Const DTWAIN_TN_FILESAVEOK As Integer = 1032
    Public Const DTWAIN_TN_FILESAVEERROR As Integer = 1033
    Public Const DTWAIN_TN_FILEPAGESAVEOK As Integer = 1034
    Public Const DTWAIN_TN_FILEPAGESAVEERROR As Integer = 1035
    Public Const DTWAIN_TN_PROCESSEDDIB As Integer = 1036
    Public Const DTWAIN_TN_FEEDERLOADED As Integer = 1037
    Public Const DTWAIN_TN_GENERALERROR As Integer = 1038
    Public Const DTWAIN_TN_MANDUPFLIPPAGES As Integer = 1040
    Public Const DTWAIN_TN_MANDUPSIDE1DONE As Integer = 1041
    Public Const DTWAIN_TN_MANDUPSIDE2DONE As Integer = 1042
    Public Const DTWAIN_TN_MANDUPPAGECOUNTERROR As Integer = 1043
    Public Const DTWAIN_TN_MANDUPACQUIREDONE As Integer = 1044
    Public Const DTWAIN_TN_MANDUPSIDE1START As Integer = 1045
    Public Const DTWAIN_TN_MANDUPSIDE2START As Integer = 1046
    Public Const DTWAIN_TN_MANDUPMERGEERROR As Integer = 1047
    Public Const DTWAIN_TN_MANDUPMEMORYERROR As Integer = 1048
    Public Const DTWAIN_TN_MANDUPFILEERROR As Integer = 1049
    Public Const DTWAIN_TN_MANDUPFILESAVEERROR As Integer = 1050
    Public Const DTWAIN_TN_ENDOFJOBDETECTED As Integer = 1051
    Public Const DTWAIN_TN_EOJDETECTED As Integer = 1051
    Public Const DTWAIN_TN_EOJDETECTED_XFERDONE As Integer = 1052
    Public Const DTWAIN_TN_QUERYPAGEDISCARD As Integer = 1053
    Public Const DTWAIN_TN_PAGEDISCARDED As Integer = 1054
    Public Const DTWAIN_TN_PROCESSDIBACCEPTED As Integer = 1055
    Public Const DTWAIN_TN_PROCESSDIBFINALACCEPTED As Integer = 1056
    Public Const DTWAIN_TN_CLOSEDIBFAILED As Integer = 1057
    Public Const DTWAIN_TN_INVALID_TWAINDSM2_BITMAP As Integer = 1058
    Public Const DTWAIN_TN_IMAGE_RESAMPLE_FAILURE As Integer = 1059
    Public Const DTWAIN_TN_DEVICEEVENT As Integer = 1100
    Public Const DTWAIN_TN_TWAINPAGECANCELLED As Integer = 1105
    Public Const DTWAIN_TN_TWAINPAGEFAILED As Integer = 1106
    Public Const DTWAIN_TN_APPUPDATEDDIB As Integer = 1107
    Public Const DTWAIN_TN_FILEPAGESAVING As Integer = 1110
    Public Const DTWAIN_TN_EOJBEGINFILESAVE As Integer = 1112
    Public Const DTWAIN_TN_EOJENDFILESAVE As Integer = 1113
    Public Const DTWAIN_TN_CROPFAILED As Integer = 1120
    Public Const DTWAIN_TN_PROCESSEDDIBFINAL As Integer = 1121
    Public Const DTWAIN_TN_BLANKPAGEDETECTED1 As Integer = 1130
    Public Const DTWAIN_TN_BLANKPAGEDETECTED2 As Integer = 1131
    Public Const DTWAIN_TN_BLANKPAGEDETECTED3 As Integer = 1132
    Public Const DTWAIN_TN_BLANKPAGEDISCARDED1 As Integer = 1133
    Public Const DTWAIN_TN_BLANKPAGEDISCARDED2 As Integer = 1134
    Public Const DTWAIN_TN_OCRTEXTRETRIEVED As Integer = 1140
    Public Const DTWAIN_TN_QUERYOCRTEXT As Integer = 1141
    Public Const DTWAIN_TN_PDFOCRREADY As Integer = 1142
    Public Const DTWAIN_TN_PDFOCRDONE As Integer = 1143
    Public Const DTWAIN_TN_PDFOCRERROR As Integer = 1144
    Public Const DTWAIN_TN_SETCALLBACKINIT As Integer = 1150
    Public Const DTWAIN_TN_SETCALLBACK64INIT As Integer = 1151
    Public Const DTWAIN_TN_FILENAMECHANGING As Integer = 1160
    Public Const DTWAIN_TN_FILENAMECHANGED As Integer = 1161
    Public Const DTWAIN_TN_PROCESSEDAUDIOFINAL As Integer = 1180
    Public Const DTWAIN_TN_PROCESSAUDIOFINALACCEPTED As Integer = 1181
    Public Const DTWAIN_TN_PROCESSEDAUDIOFILE As Integer = 1182
    Public Const DTWAIN_TN_TWAINTRIPLETBEGIN As Integer = 1183
    Public Const DTWAIN_TN_TWAINTRIPLETEND As Integer = 1184
    Public Const DTWAIN_TN_FEEDERNOTLOADED As Integer = 1201
    Public Const DTWAIN_TN_FEEDERTIMEOUT As Integer = 1202
    Public Const DTWAIN_TN_FEEDERNOTENABLED As Integer = 1203
    Public Const DTWAIN_TN_FEEDERNOTSUPPORTED As Integer = 1204
    Public Const DTWAIN_TN_FEEDERTOFLATBED As Integer = 1205
    Public Const DTWAIN_TN_TRANSFERTILEREADY As Integer = 1300
    Public Const DTWAIN_TN_TRANSFERTILEDONE As Integer = 1301
    Public Const DTWAIN_TN_FILECOMPRESSTYPEMISMATCH As Integer = 1302
    Public Const DTWAIN_PDFOCR_CLEANTEXT1 As Integer = 1
    Public Const DTWAIN_PDFOCR_CLEANTEXT2 As Integer = 2
    Public Const DTWAIN_MODAL As Integer = 0
    Public Const DTWAIN_MODELESS As Integer = 1
    Public Const DTWAIN_UIModeCLOSE As Integer = 0
    Public Const DTWAIN_UIModeOPEN As Integer = 1
    Public Const DTWAIN_REOPEN_SOURCE As Integer = 2
    Public Const DTWAIN_ROUNDNEAREST As Integer = 0
    Public Const DTWAIN_ROUNDUP As Integer = 1
    Public Const DTWAIN_ROUNDDOWN As Integer = 2
    Public Const DTWAIN_FLOATDELTA As Double = (+1.0e-8)
    Public Const DTWAIN_OR_ROT0 As Integer = 0
    Public Const DTWAIN_OR_ROT90 As Integer = 1
    Public Const DTWAIN_OR_ROT180 As Integer = 2
    Public Const DTWAIN_OR_ROT270 As Integer = 3
    Public Const DTWAIN_OR_PORTRAIT As Integer = DTWAIN_OR_ROT0
    Public Const DTWAIN_OR_LANDSCAPE As Integer = DTWAIN_OR_ROT270
    Public Const DTWAIN_OR_ANYROTATION As Integer = (-1)
    Public Const DTWAIN_CO_GET As Integer = &H0001
    Public Const DTWAIN_CO_SET As Integer = &H0002
    Public Const DTWAIN_CO_GETDEFAULT As Integer = &H0004
    Public Const DTWAIN_CO_GETCURRENT As Integer = &H0008
    Public Const DTWAIN_CO_RESET As Integer = &H0010
    Public Const DTWAIN_CO_SETCONSTRAINT As Integer = &H0020
    Public Const DTWAIN_CO_CONSTRAINABLE As Integer = &H0040
    Public Const DTWAIN_CO_GETHELP As Integer = &H0100
    Public Const DTWAIN_CO_GETLABEL As Integer = &H0200
    Public Const DTWAIN_CO_GETLABELENUM As Integer = &H0400
    Public Const DTWAIN_CNTYAFGHANISTAN As Integer = 1001
    Public Const DTWAIN_CNTYALGERIA As Integer = 213
    Public Const DTWAIN_CNTYAMERICANSAMOA As Integer = 684
    Public Const DTWAIN_CNTYANDORRA As Integer = 33
    Public Const DTWAIN_CNTYANGOLA As Integer = 1002
    Public Const DTWAIN_CNTYANGUILLA As Integer = 8090
    Public Const DTWAIN_CNTYANTIGUA As Integer = 8091
    Public Const DTWAIN_CNTYARGENTINA As Integer = 54
    Public Const DTWAIN_CNTYARUBA As Integer = 297
    Public Const DTWAIN_CNTYASCENSIONI As Integer = 247
    Public Const DTWAIN_CNTYAUSTRALIA As Integer = 61
    Public Const DTWAIN_CNTYAUSTRIA As Integer = 43
    Public Const DTWAIN_CNTYBAHAMAS As Integer = 8092
    Public Const DTWAIN_CNTYBAHRAIN As Integer = 973
    Public Const DTWAIN_CNTYBANGLADESH As Integer = 880
    Public Const DTWAIN_CNTYBARBADOS As Integer = 8093
    Public Const DTWAIN_CNTYBELGIUM As Integer = 32
    Public Const DTWAIN_CNTYBELIZE As Integer = 501
    Public Const DTWAIN_CNTYBENIN As Integer = 229
    Public Const DTWAIN_CNTYBERMUDA As Integer = 8094
    Public Const DTWAIN_CNTYBHUTAN As Integer = 1003
    Public Const DTWAIN_CNTYBOLIVIA As Integer = 591
    Public Const DTWAIN_CNTYBOTSWANA As Integer = 267
    Public Const DTWAIN_CNTYBRITAIN As Integer = 6
    Public Const DTWAIN_CNTYBRITVIRGINIS As Integer = 8095
    Public Const DTWAIN_CNTYBRAZIL As Integer = 55
    Public Const DTWAIN_CNTYBRUNEI As Integer = 673
    Public Const DTWAIN_CNTYBULGARIA As Integer = 359
    Public Const DTWAIN_CNTYBURKINAFASO As Integer = 1004
    Public Const DTWAIN_CNTYBURMA As Integer = 1005
    Public Const DTWAIN_CNTYBURUNDI As Integer = 1006
    Public Const DTWAIN_CNTYCAMAROON As Integer = 237
    Public Const DTWAIN_CNTYCANADA As Integer = 2
    Public Const DTWAIN_CNTYCAPEVERDEIS As Integer = 238
    Public Const DTWAIN_CNTYCAYMANIS As Integer = 8096
    Public Const DTWAIN_CNTYCENTRALAFREP As Integer = 1007
    Public Const DTWAIN_CNTYCHAD As Integer = 1008
    Public Const DTWAIN_CNTYCHILE As Integer = 56
    Public Const DTWAIN_CNTYCHINA As Integer = 86
    Public Const DTWAIN_CNTYCHRISTMASIS As Integer = 1009
    Public Const DTWAIN_CNTYCOCOSIS As Integer = 1009
    Public Const DTWAIN_CNTYCOLOMBIA As Integer = 57
    Public Const DTWAIN_CNTYCOMOROS As Integer = 1010
    Public Const DTWAIN_CNTYCONGO As Integer = 1011
    Public Const DTWAIN_CNTYCOOKIS As Integer = 1012
    Public Const DTWAIN_CNTYCOSTARICA As Integer = 506
    Public Const DTWAIN_CNTYCUBA As Integer = 5
    Public Const DTWAIN_CNTYCYPRUS As Integer = 357
    Public Const DTWAIN_CNTYCZECHOSLOVAKIA As Integer = 42
    Public Const DTWAIN_CNTYDENMARK As Integer = 45
    Public Const DTWAIN_CNTYDJIBOUTI As Integer = 1013
    Public Const DTWAIN_CNTYDOMINICA As Integer = 8097
    Public Const DTWAIN_CNTYDOMINCANREP As Integer = 8098
    Public Const DTWAIN_CNTYEASTERIS As Integer = 1014
    Public Const DTWAIN_CNTYECUADOR As Integer = 593
    Public Const DTWAIN_CNTYEGYPT As Integer = 20
    Public Const DTWAIN_CNTYELSALVADOR As Integer = 503
    Public Const DTWAIN_CNTYEQGUINEA As Integer = 1015
    Public Const DTWAIN_CNTYETHIOPIA As Integer = 251
    Public Const DTWAIN_CNTYFALKLANDIS As Integer = 1016
    Public Const DTWAIN_CNTYFAEROEIS As Integer = 298
    Public Const DTWAIN_CNTYFIJIISLANDS As Integer = 679
    Public Const DTWAIN_CNTYFINLAND As Integer = 358
    Public Const DTWAIN_CNTYFRANCE As Integer = 33
    Public Const DTWAIN_CNTYFRANTILLES As Integer = 596
    Public Const DTWAIN_CNTYFRGUIANA As Integer = 594
    Public Const DTWAIN_CNTYFRPOLYNEISA As Integer = 689
    Public Const DTWAIN_CNTYFUTANAIS As Integer = 1043
    Public Const DTWAIN_CNTYGABON As Integer = 241
    Public Const DTWAIN_CNTYGAMBIA As Integer = 220
    Public Const DTWAIN_CNTYGERMANY As Integer = 49
    Public Const DTWAIN_CNTYGHANA As Integer = 233
    Public Const DTWAIN_CNTYGIBRALTER As Integer = 350
    Public Const DTWAIN_CNTYGREECE As Integer = 30
    Public Const DTWAIN_CNTYGREENLAND As Integer = 299
    Public Const DTWAIN_CNTYGRENADA As Integer = 8099
    Public Const DTWAIN_CNTYGRENEDINES As Integer = 8015
    Public Const DTWAIN_CNTYGUADELOUPE As Integer = 590
    Public Const DTWAIN_CNTYGUAM As Integer = 671
    Public Const DTWAIN_CNTYGUANTANAMOBAY As Integer = 5399
    Public Const DTWAIN_CNTYGUATEMALA As Integer = 502
    Public Const DTWAIN_CNTYGUINEA As Integer = 224
    Public Const DTWAIN_CNTYGUINEABISSAU As Integer = 1017
    Public Const DTWAIN_CNTYGUYANA As Integer = 592
    Public Const DTWAIN_CNTYHAITI As Integer = 509
    Public Const DTWAIN_CNTYHONDURAS As Integer = 504
    Public Const DTWAIN_CNTYHONGKONG As Integer = 852
    Public Const DTWAIN_CNTYHUNGARY As Integer = 36
    Public Const DTWAIN_CNTYICELAND As Integer = 354
    Public Const DTWAIN_CNTYINDIA As Integer = 91
    Public Const DTWAIN_CNTYINDONESIA As Integer = 62
    Public Const DTWAIN_CNTYIRAN As Integer = 98
    Public Const DTWAIN_CNTYIRAQ As Integer = 964
    Public Const DTWAIN_CNTYIRELAND As Integer = 353
    Public Const DTWAIN_CNTYISRAEL As Integer = 972
    Public Const DTWAIN_CNTYITALY As Integer = 39
    Public Const DTWAIN_CNTYIVORYCOAST As Integer = 225
    Public Const DTWAIN_CNTYJAMAICA As Integer = 8010
    Public Const DTWAIN_CNTYJAPAN As Integer = 81
    Public Const DTWAIN_CNTYJORDAN As Integer = 962
    Public Const DTWAIN_CNTYKENYA As Integer = 254
    Public Const DTWAIN_CNTYKIRIBATI As Integer = 1018
    Public Const DTWAIN_CNTYKOREA As Integer = 82
    Public Const DTWAIN_CNTYKUWAIT As Integer = 965
    Public Const DTWAIN_CNTYLAOS As Integer = 1019
    Public Const DTWAIN_CNTYLEBANON As Integer = 1020
    Public Const DTWAIN_CNTYLIBERIA As Integer = 231
    Public Const DTWAIN_CNTYLIBYA As Integer = 218
    Public Const DTWAIN_CNTYLIECHTENSTEIN As Integer = 41
    Public Const DTWAIN_CNTYLUXENBOURG As Integer = 352
    Public Const DTWAIN_CNTYMACAO As Integer = 853
    Public Const DTWAIN_CNTYMADAGASCAR As Integer = 1021
    Public Const DTWAIN_CNTYMALAWI As Integer = 265
    Public Const DTWAIN_CNTYMALAYSIA As Integer = 60
    Public Const DTWAIN_CNTYMALDIVES As Integer = 960
    Public Const DTWAIN_CNTYMALI As Integer = 1022
    Public Const DTWAIN_CNTYMALTA As Integer = 356
    Public Const DTWAIN_CNTYMARSHALLIS As Integer = 692
    Public Const DTWAIN_CNTYMAURITANIA As Integer = 1023
    Public Const DTWAIN_CNTYMAURITIUS As Integer = 230
    Public Const DTWAIN_CNTYMEXICO As Integer = 3
    Public Const DTWAIN_CNTYMICRONESIA As Integer = 691
    Public Const DTWAIN_CNTYMIQUELON As Integer = 508
    Public Const DTWAIN_CNTYMONACO As Integer = 33
    Public Const DTWAIN_CNTYMONGOLIA As Integer = 1024
    Public Const DTWAIN_CNTYMONTSERRAT As Integer = 8011
    Public Const DTWAIN_CNTYMOROCCO As Integer = 212
    Public Const DTWAIN_CNTYMOZAMBIQUE As Integer = 1025
    Public Const DTWAIN_CNTYNAMIBIA As Integer = 264
    Public Const DTWAIN_CNTYNAURU As Integer = 1026
    Public Const DTWAIN_CNTYNEPAL As Integer = 977
    Public Const DTWAIN_CNTYNETHERLANDS As Integer = 31
    Public Const DTWAIN_CNTYNETHANTILLES As Integer = 599
    Public Const DTWAIN_CNTYNEVIS As Integer = 8012
    Public Const DTWAIN_CNTYNEWCALEDONIA As Integer = 687
    Public Const DTWAIN_CNTYNEWZEALAND As Integer = 64
    Public Const DTWAIN_CNTYNICARAGUA As Integer = 505
    Public Const DTWAIN_CNTYNIGER As Integer = 227
    Public Const DTWAIN_CNTYNIGERIA As Integer = 234
    Public Const DTWAIN_CNTYNIUE As Integer = 1027
    Public Const DTWAIN_CNTYNORFOLKI As Integer = 1028
    Public Const DTWAIN_CNTYNORWAY As Integer = 47
    Public Const DTWAIN_CNTYOMAN As Integer = 968
    Public Const DTWAIN_CNTYPAKISTAN As Integer = 92
    Public Const DTWAIN_CNTYPALAU As Integer = 1029
    Public Const DTWAIN_CNTYPANAMA As Integer = 507
    Public Const DTWAIN_CNTYPARAGUAY As Integer = 595
    Public Const DTWAIN_CNTYPERU As Integer = 51
    Public Const DTWAIN_CNTYPHILLIPPINES As Integer = 63
    Public Const DTWAIN_CNTYPITCAIRNIS As Integer = 1030
    Public Const DTWAIN_CNTYPNEWGUINEA As Integer = 675
    Public Const DTWAIN_CNTYPOLAND As Integer = 48
    Public Const DTWAIN_CNTYPORTUGAL As Integer = 351
    Public Const DTWAIN_CNTYQATAR As Integer = 974
    Public Const DTWAIN_CNTYREUNIONI As Integer = 1031
    Public Const DTWAIN_CNTYROMANIA As Integer = 40
    Public Const DTWAIN_CNTYRWANDA As Integer = 250
    Public Const DTWAIN_CNTYSAIPAN As Integer = 670
    Public Const DTWAIN_CNTYSANMARINO As Integer = 39
    Public Const DTWAIN_CNTYSAOTOME As Integer = 1033
    Public Const DTWAIN_CNTYSAUDIARABIA As Integer = 966
    Public Const DTWAIN_CNTYSENEGAL As Integer = 221
    Public Const DTWAIN_CNTYSEYCHELLESIS As Integer = 1034
    Public Const DTWAIN_CNTYSIERRALEONE As Integer = 1035
    Public Const DTWAIN_CNTYSINGAPORE As Integer = 65
    Public Const DTWAIN_CNTYSOLOMONIS As Integer = 1036
    Public Const DTWAIN_CNTYSOMALI As Integer = 1037
    Public Const DTWAIN_CNTYSOUTHAFRICA As Integer = 27
    Public Const DTWAIN_CNTYSPAIN As Integer = 34
    Public Const DTWAIN_CNTYSRILANKA As Integer = 94
    Public Const DTWAIN_CNTYSTHELENA As Integer = 1032
    Public Const DTWAIN_CNTYSTKITTS As Integer = 8013
    Public Const DTWAIN_CNTYSTLUCIA As Integer = 8014
    Public Const DTWAIN_CNTYSTPIERRE As Integer = 508
    Public Const DTWAIN_CNTYSTVINCENT As Integer = 8015
    Public Const DTWAIN_CNTYSUDAN As Integer = 1038
    Public Const DTWAIN_CNTYSURINAME As Integer = 597
    Public Const DTWAIN_CNTYSWAZILAND As Integer = 268
    Public Const DTWAIN_CNTYSWEDEN As Integer = 46
    Public Const DTWAIN_CNTYSWITZERLAND As Integer = 41
    Public Const DTWAIN_CNTYSYRIA As Integer = 1039
    Public Const DTWAIN_CNTYTAIWAN As Integer = 886
    Public Const DTWAIN_CNTYTANZANIA As Integer = 255
    Public Const DTWAIN_CNTYTHAILAND As Integer = 66
    Public Const DTWAIN_CNTYTOBAGO As Integer = 8016
    Public Const DTWAIN_CNTYTOGO As Integer = 228
    Public Const DTWAIN_CNTYTONGAIS As Integer = 676
    Public Const DTWAIN_CNTYTRINIDAD As Integer = 8016
    Public Const DTWAIN_CNTYTUNISIA As Integer = 216
    Public Const DTWAIN_CNTYTURKEY As Integer = 90
    Public Const DTWAIN_CNTYTURKSCAICOS As Integer = 8017
    Public Const DTWAIN_CNTYTUVALU As Integer = 1040
    Public Const DTWAIN_CNTYUGANDA As Integer = 256
    Public Const DTWAIN_CNTYUSSR As Integer = 7
    Public Const DTWAIN_CNTYUAEMIRATES As Integer = 971
    Public Const DTWAIN_CNTYUNITEDKINGDOM As Integer = 44
    Public Const DTWAIN_CNTYUSA As Integer = 1
    Public Const DTWAIN_CNTYURUGUAY As Integer = 598
    Public Const DTWAIN_CNTYVANUATU As Integer = 1041
    Public Const DTWAIN_CNTYVATICANCITY As Integer = 39
    Public Const DTWAIN_CNTYVENEZUELA As Integer = 58
    Public Const DTWAIN_CNTYWAKE As Integer = 1042
    Public Const DTWAIN_CNTYWALLISIS As Integer = 1043
    Public Const DTWAIN_CNTYWESTERNSAHARA As Integer = 1044
    Public Const DTWAIN_CNTYWESTERNSAMOA As Integer = 1045
    Public Const DTWAIN_CNTYYEMEN As Integer = 1046
    Public Const DTWAIN_CNTYYUGOSLAVIA As Integer = 38
    Public Const DTWAIN_CNTYZAIRE As Integer = 243
    Public Const DTWAIN_CNTYZAMBIA As Integer = 260
    Public Const DTWAIN_CNTYZIMBABWE As Integer = 263
    Public Const DTWAIN_LANGDANISH As Integer = 0
    Public Const DTWAIN_LANGDUTCH As Integer = 1
    Public Const DTWAIN_LANGINTERNATIONALENGLISH As Integer = 2
    Public Const DTWAIN_LANGFRENCHCANADIAN As Integer = 3
    Public Const DTWAIN_LANGFINNISH As Integer = 4
    Public Const DTWAIN_LANGFRENCH As Integer = 5
    Public Const DTWAIN_LANGGERMAN As Integer = 6
    Public Const DTWAIN_LANGICELANDIC As Integer = 7
    Public Const DTWAIN_LANGITALIAN As Integer = 8
    Public Const DTWAIN_LANGNORWEGIAN As Integer = 9
    Public Const DTWAIN_LANGPORTUGUESE As Integer = 10
    Public Const DTWAIN_LANGSPANISH As Integer = 11
    Public Const DTWAIN_LANGSWEDISH As Integer = 12
    Public Const DTWAIN_LANGUSAENGLISH As Integer = 13
    Public Const DTWAIN_NO_ERROR As Integer = (0)
    Public Const DTWAIN_ERR_FIRST As Integer = (-1000)
    Public Const DTWAIN_ERR_BAD_HANDLE As Integer = (-1001)
    Public Const DTWAIN_ERR_BAD_SOURCE As Integer = (-1002)
    Public Const DTWAIN_ERR_BAD_ARRAY As Integer = (-1003)
    Public Const DTWAIN_ERR_WRONG_ARRAY_TYPE As Integer = (-1004)
    Public Const DTWAIN_ERR_INDEX_BOUNDS As Integer = (-1005)
    Public Const DTWAIN_ERR_OUT_OF_MEMORY As Integer = (-1006)
    Public Const DTWAIN_ERR_NULL_WINDOW As Integer = (-1007)
    Public Const DTWAIN_ERR_BAD_PIXTYPE As Integer = (-1008)
    Public Const DTWAIN_ERR_BAD_CONTAINER As Integer = (-1009)
    Public Const DTWAIN_ERR_NO_SESSION As Integer = (-1010)
    Public Const DTWAIN_ERR_BAD_ACQUIRE_NUM As Integer = (-1011)
    Public Const DTWAIN_ERR_BAD_CAP As Integer = (-1012)
    Public Const DTWAIN_ERR_CAP_NO_SUPPORT As Integer = (-1013)
    Public Const DTWAIN_ERR_TWAIN As Integer = (-1014)
    Public Const DTWAIN_ERR_HOOK_FAILED As Integer = (-1015)
    Public Const DTWAIN_ERR_BAD_FILENAME As Integer = (-1016)
    Public Const DTWAIN_ERR_EMPTY_ARRAY As Integer = (-1017)
    Public Const DTWAIN_ERR_FILE_FORMAT As Integer = (-1018)
    Public Const DTWAIN_ERR_BAD_DIB_PAGE As Integer = (-1019)
    Public Const DTWAIN_ERR_SOURCE_ACQUIRING As Integer = (-1020)
    Public Const DTWAIN_ERR_INVALID_PARAM As Integer = (-1021)
    Public Const DTWAIN_ERR_INVALID_RANGE As Integer = (-1022)
    Public Const DTWAIN_ERR_UI_ERROR As Integer = (-1023)
    Public Const DTWAIN_ERR_BAD_UNIT As Integer = (-1024)
    Public Const DTWAIN_ERR_LANGDLL_NOT_FOUND As Integer = (-1025)
    Public Const DTWAIN_ERR_SOURCE_NOT_OPEN As Integer = (-1026)
    Public Const DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED As Integer = (-1027)
    Public Const DTWAIN_ERR_UIONLY_NOT_SUPPORTED As Integer = (-1028)
    Public Const DTWAIN_ERR_UI_ALREADY_OPENED As Integer = (-1029)
    Public Const DTWAIN_ERR_CAPSET_NOSUPPORT As Integer = (-1030)
    Public Const DTWAIN_ERR_NO_FILE_XFER As Integer = (-1031)
    Public Const DTWAIN_ERR_INVALID_BITDEPTH As Integer = (-1032)
    Public Const DTWAIN_ERR_NO_CAPS_DEFINED As Integer = (-1033)
    Public Const DTWAIN_ERR_TILES_NOT_SUPPORTED As Integer = (-1034)
    Public Const DTWAIN_ERR_INVALID_DTWAIN_FRAME As Integer = (-1035)
    Public Const DTWAIN_ERR_LIMITED_VERSION As Integer = (-1036)
    Public Const DTWAIN_ERR_NO_FEEDER As Integer = (-1037)
    Public Const DTWAIN_ERR_NO_FEEDER_QUERY As Integer = (-1038)
    Public Const DTWAIN_ERR_EXCEPTION_ERROR As Integer = (-1039)
    Public Const DTWAIN_ERR_INVALID_STATE As Integer = (-1040)
    Public Const DTWAIN_ERR_UNSUPPORTED_EXTINFO As Integer = (-1041)
    Public Const DTWAIN_ERR_DLLRESOURCE_NOTFOUND As Integer = (-1042)
    Public Const DTWAIN_ERR_NOT_INITIALIZED As Integer = (-1043)
    Public Const DTWAIN_ERR_NO_SOURCES As Integer = (-1044)
    Public Const DTWAIN_ERR_TWAIN_NOT_INSTALLED As Integer = (-1045)
    Public Const DTWAIN_ERR_WRONG_THREAD As Integer = (-1046)
    Public Const DTWAIN_ERR_BAD_CAPTYPE As Integer = (-1047)
    Public Const DTWAIN_ERR_UNKNOWN_CAPDATATYPE As Integer = (-1048)
    Public Const DTWAIN_ERR_DEMO_NOFILETYPE As Integer = (-1049)
    Public Const DTWAIN_ERR_SOURCESELECTION_CANCELED As Integer = (-1050)
    Public Const DTWAIN_ERR_RESOURCES_NOT_FOUND As Integer = (-1051)
    Public Const DTWAIN_ERR_STRINGTYPE_MISMATCH As Integer = (-1052)
    Public Const DTWAIN_ERR_ARRAYTYPE_MISMATCH As Integer = (-1053)
    Public Const DTWAIN_ERR_SOURCENAME_NOTINSTALLED As Integer = (-1054)
    Public Const DTWAIN_ERR_NO_MEMFILE_XFER As Integer = (-1055)
    Public Const DTWAIN_ERR_AREA_ARRAY_TOO_SMALL As Integer = (-1056)
    Public Const DTWAIN_ERR_LOG_CREATE_ERROR As Integer = (-1057)
    Public Const DTWAIN_ERR_FILESYSTEM_NOT_SUPPORTED As Integer = (-1058)
    Public Const DTWAIN_ERR_TILEMODE_NOTSET As Integer = (-1059)
    Public Const DTWAIN_ERR_INI32_NOT_FOUND As Integer = (-1060)
    Public Const DTWAIN_ERR_INI64_NOT_FOUND As Integer = (-1061)
    Public Const DTWAIN_ERR_CRC_CHECK As Integer = (-1062)
    Public Const DTWAIN_ERR_RESOURCES_BAD_VERSION As Integer = (-1063)
    Public Const DTWAIN_ERR_WIN32_ERROR As Integer = (-1064)
    Public Const DTWAIN_ERR_STRINGID_NOTFOUND As Integer = (-1065)
    Public Const DTWAIN_ERR_RESOURCES_DUPLICATEID_FOUND As Integer = (-1066)
    Public Const DTWAIN_ERR_UNAVAILABLE_EXTINFO As Integer = (-1067)
    Public Const DTWAIN_ERR_TWAINDSM2_BADBITMAP As Integer = (-1068)
    Public Const DTWAIN_ERR_ACQUISITION_CANCELED As Integer = (-1069)
    Public Const DTWAIN_ERR_IMAGE_RESAMPLED As Integer = (-1070)
    Public Const DTWAIN_ERR_UNKNOWN_TWAIN_RC As Integer = (-1071)
    Public Const DTWAIN_ERR_UNKNOWN_TWAIN_CC As Integer = (-1072)
    Public Const DTWAIN_ERR_RESOURCES_DATA_EXCEPTION As Integer = (-1073)
    Public Const DTWAIN_ERR_AUDIO_TRANSFER_NOTSUPPORTED As Integer = (-1074)
    Public Const DTWAIN_ERR_FEEDER_COMPLIANCY As Integer = (-1075)
    Public Const DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY1 As Integer = (-1076)
    Public Const DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY2 As Integer = (-1077)
    Public Const DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY1 As Integer = (-1078)
    Public Const DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY2 As Integer = (-1079)
    Public Const DTWAIN_ERR_ICAPBITDEPTH_COMPLIANCY1 As Integer = (-1080)
    Public Const DTWAIN_ERR_XFERMECH_COMPLIANCY As Integer = (-1081)
    Public Const DTWAIN_ERR_STANDARDCAPS_COMPLIANCY As Integer = (-1082)
    Public Const DTWAIN_ERR_EXTIMAGEINFO_DATATYPE_MISMATCH As Integer = (-1083)
    Public Const DTWAIN_ERR_EXTIMAGEINFO_RETRIEVAL As Integer = (-1084)
    Public Const DTWAIN_ERR_RANGE_OUTOFBOUNDS As Integer = (-1085)
    Public Const DTWAIN_ERR_RANGE_STEPISZERO As Integer = (-1086)
    Public Const DTWAIN_ERR_BLANKNAMEDETECTED As Integer = (-1087)
    Public Const DTWAIN_ERR_FEEDER_NOPAPERSENSOR As Integer = (-1088)
    Public Const TWAIN_ERR_LOW_MEMORY As Integer = (-1100)
    Public Const TWAIN_ERR_FALSE_ALARM As Integer = (-1101)
    Public Const TWAIN_ERR_BUMMER As Integer = (-1102)
    Public Const TWAIN_ERR_NODATASOURCE As Integer = (-1103)
    Public Const TWAIN_ERR_MAXCONNECTIONS As Integer = (-1104)
    Public Const TWAIN_ERR_OPERATIONERROR As Integer = (-1105)
    Public Const TWAIN_ERR_BADCAPABILITY As Integer = (-1106)
    Public Const TWAIN_ERR_BADVALUE As Integer = (-1107)
    Public Const TWAIN_ERR_BADPROTOCOL As Integer = (-1108)
    Public Const TWAIN_ERR_SEQUENCEERROR As Integer = (-1109)
    Public Const TWAIN_ERR_BADDESTINATION As Integer = (-1110)
    Public Const TWAIN_ERR_CAPNOTSUPPORTED As Integer = (-1111)
    Public Const TWAIN_ERR_CAPBADOPERATION As Integer = (-1112)
    Public Const TWAIN_ERR_CAPSEQUENCEERROR As Integer = (-1113)
    Public Const TWAIN_ERR_FILEPROTECTEDERROR As Integer = (-1114)
    Public Const TWAIN_ERR_FILEEXISTERROR As Integer = (-1115)
    Public Const TWAIN_ERR_FILENOTFOUND As Integer = (-1116)
    Public Const TWAIN_ERR_DIRNOTEMPTY As Integer = (-1117)
    Public Const TWAIN_ERR_FEEDERJAMMED As Integer = (-1118)
    Public Const TWAIN_ERR_FEEDERMULTPAGES As Integer = (-1119)
    Public Const TWAIN_ERR_FEEDERWRITEERROR As Integer = (-1120)
    Public Const TWAIN_ERR_DEVICEOFFLINE As Integer = (-1121)
    Public Const TWAIN_ERR_NULL_CONTAINER As Integer = (-1122)
    Public Const TWAIN_ERR_INTERLOCK As Integer = (-1123)
    Public Const TWAIN_ERR_DAMAGEDCORNER As Integer = (-1124)
    Public Const TWAIN_ERR_FOCUSERROR As Integer = (-1125)
    Public Const TWAIN_ERR_DOCTOOLIGHT As Integer = (-1126)
    Public Const TWAIN_ERR_DOCTOODARK As Integer = (-1127)
    Public Const TWAIN_ERR_NOMEDIA As Integer = (-1128)
    Public Const DTWAIN_ERR_FILEXFERSTART As Integer = (-2000)
    Public Const DTWAIN_ERR_MEM As Integer = (-2001)
    Public Const DTWAIN_ERR_FILEOPEN As Integer = (-2002)
    Public Const DTWAIN_ERR_FILEREAD As Integer = (-2003)
    Public Const DTWAIN_ERR_FILEWRITE As Integer = (-2004)
    Public Const DTWAIN_ERR_BADPARAM As Integer = (-2005)
    Public Const DTWAIN_ERR_INVALIDBMP As Integer = (-2006)
    Public Const DTWAIN_ERR_BMPRLE As Integer = (-2007)
    Public Const DTWAIN_ERR_RESERVED1 As Integer = (-2008)
    Public Const DTWAIN_ERR_INVALIDJPG As Integer = (-2009)
    Public Const DTWAIN_ERR_DC As Integer = (-2010)
    Public Const DTWAIN_ERR_DIB As Integer = (-2011)
    Public Const DTWAIN_ERR_RESERVED2 As Integer = (-2012)
    Public Const DTWAIN_ERR_NORESOURCE As Integer = (-2013)
    Public Const DTWAIN_ERR_CALLBACKCANCEL As Integer = (-2014)
    Public Const DTWAIN_ERR_INVALIDPNG As Integer = (-2015)
    Public Const DTWAIN_ERR_PNGCREATE As Integer = (-2016)
    Public Const DTWAIN_ERR_INTERNAL As Integer = (-2017)
    Public Const DTWAIN_ERR_FONT As Integer = (-2018)
    Public Const DTWAIN_ERR_INTTIFF As Integer = (-2019)
    Public Const DTWAIN_ERR_INVALIDTIFF As Integer = (-2020)
    Public Const DTWAIN_ERR_NOTIFFLZW As Integer = (-2021)
    Public Const DTWAIN_ERR_INVALIDPCX As Integer = (-2022)
    Public Const DTWAIN_ERR_CREATEBMP As Integer = (-2023)
    Public Const DTWAIN_ERR_NOLINES As Integer = (-2024)
    Public Const DTWAIN_ERR_GETDIB As Integer = (-2025)
    Public Const DTWAIN_ERR_NODEVOP As Integer = (-2026)
    Public Const DTWAIN_ERR_INVALIDWMF As Integer = (-2027)
    Public Const DTWAIN_ERR_DEPTHMISMATCH As Integer = (-2028)
    Public Const DTWAIN_ERR_BITBLT As Integer = (-2029)
    Public Const DTWAIN_ERR_BUFTOOSMALL As Integer = (-2030)
    Public Const DTWAIN_ERR_TOOMANYCOLORS As Integer = (-2031)
    Public Const DTWAIN_ERR_INVALIDTGA As Integer = (-2032)
    Public Const DTWAIN_ERR_NOTGATHUMBNAIL As Integer = (-2033)
    Public Const DTWAIN_ERR_RESERVED3 As Integer = (-2034)
    Public Const DTWAIN_ERR_CREATEDIB As Integer = (-2035)
    Public Const DTWAIN_ERR_NOLZW As Integer = (-2036)
    Public Const DTWAIN_ERR_SELECTOBJ As Integer = (-2037)
    Public Const DTWAIN_ERR_BADMANAGER As Integer = (-2038)
    Public Const DTWAIN_ERR_OBSOLETE As Integer = (-2039)
    Public Const DTWAIN_ERR_CREATEDIBSECTION As Integer = (-2040)
    Public Const DTWAIN_ERR_SETWINMETAFILEBITS As Integer = (-2041)
    Public Const DTWAIN_ERR_GETWINMETAFILEBITS As Integer = (-2042)
    Public Const DTWAIN_ERR_PAXPWD As Integer = (-2043)
    Public Const DTWAIN_ERR_INVALIDPAX As Integer = (-2044)
    Public Const DTWAIN_ERR_NOSUPPORT As Integer = (-2045)
    Public Const DTWAIN_ERR_INVALIDPSD As Integer = (-2046)
    Public Const DTWAIN_ERR_PSDNOTSUPPORTED As Integer = (-2047)
    Public Const DTWAIN_ERR_DECRYPT As Integer = (-2048)
    Public Const DTWAIN_ERR_ENCRYPT As Integer = (-2049)
    Public Const DTWAIN_ERR_COMPRESSION As Integer = (-2050)
    Public Const DTWAIN_ERR_DECOMPRESSION As Integer = (-2051)
    Public Const DTWAIN_ERR_INVALIDTLA As Integer = (-2052)
    Public Const DTWAIN_ERR_INVALIDWBMP As Integer = (-2053)
    Public Const DTWAIN_ERR_NOTIFFTAG As Integer = (-2054)
    Public Const DTWAIN_ERR_NOLOCALSTORAGE As Integer = (-2055)
    Public Const DTWAIN_ERR_INVALIDEXIF As Integer = (-2056)
    Public Const DTWAIN_ERR_NOEXIFSTRING As Integer = (-2057)
    Public Const DTWAIN_ERR_TIFFDLL32NOTFOUND As Integer = (-2058)
    Public Const DTWAIN_ERR_TIFFDLL16NOTFOUND As Integer = (-2059)
    Public Const DTWAIN_ERR_PNGDLL16NOTFOUND As Integer = (-2060)
    Public Const DTWAIN_ERR_JPEGDLL16NOTFOUND As Integer = (-2061)
    Public Const DTWAIN_ERR_BADBITSPERPIXEL As Integer = (-2062)
    Public Const DTWAIN_ERR_TIFFDLL32INVALIDVER As Integer = (-2063)
    Public Const DTWAIN_ERR_PDFDLL32NOTFOUND As Integer = (-2064)
    Public Const DTWAIN_ERR_PDFDLL32INVALIDVER As Integer = (-2065)
    Public Const DTWAIN_ERR_JPEGDLL32NOTFOUND As Integer = (-2066)
    Public Const DTWAIN_ERR_JPEGDLL32INVALIDVER As Integer = (-2067)
    Public Const DTWAIN_ERR_PNGDLL32NOTFOUND As Integer = (-2068)
    Public Const DTWAIN_ERR_PNGDLL32INVALIDVER As Integer = (-2069)
    Public Const DTWAIN_ERR_J2KDLL32NOTFOUND As Integer = (-2070)
    Public Const DTWAIN_ERR_J2KDLL32INVALIDVER As Integer = (-2071)
    Public Const DTWAIN_ERR_MANDUPLEX_UNAVAILABLE As Integer = (-2072)
    Public Const DTWAIN_ERR_TIMEOUT As Integer = (-2073)
    Public Const DTWAIN_ERR_INVALIDICONFORMAT As Integer = (-2074)
    Public Const DTWAIN_ERR_TWAIN32DSMNOTFOUND As Integer = (-2075)
    Public Const DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND As Integer = (-2076)
    Public Const DTWAIN_ERR_INVALID_DIRECTORY As Integer = (-2077)
    Public Const DTWAIN_ERR_CREATE_DIRECTORY As Integer = (-2078)
    Public Const DTWAIN_ERR_OCRLIBRARY_NOTFOUND As Integer = (-2079)
    Public Const DTWAIN_TWAINSAVE_OK As Integer = (0)
    Public Const DTWAIN_ERR_TS_FIRST As Integer = (-2080)
    Public Const DTWAIN_ERR_TS_NOFILENAME As Integer = (-2081)
    Public Const DTWAIN_ERR_TS_NOTWAINSYS As Integer = (-2082)
    Public Const DTWAIN_ERR_TS_DEVICEFAILURE As Integer = (-2083)
    Public Const DTWAIN_ERR_TS_FILESAVEERROR As Integer = (-2084)
    Public Const DTWAIN_ERR_TS_COMMANDILLEGAL As Integer = (-2085)
    Public Const DTWAIN_ERR_TS_CANCELLED As Integer = (-2086)
    Public Const DTWAIN_ERR_TS_ACQUISITIONERROR As Integer = (-2087)
    Public Const DTWAIN_ERR_TS_INVALIDCOLORSPACE As Integer = (-2088)
    Public Const DTWAIN_ERR_TS_PDFNOTSUPPORTED As Integer = (-2089)
    Public Const DTWAIN_ERR_TS_NOTAVAILABLE As Integer = (-2090)
    Public Const DTWAIN_ERR_OCR_FIRST As Integer = (-2100)
    Public Const DTWAIN_ERR_OCR_INVALIDPAGENUM As Integer = (-2101)
    Public Const DTWAIN_ERR_OCR_INVALIDENGINE As Integer = (-2102)
    Public Const DTWAIN_ERR_OCR_NOTACTIVE As Integer = (-2103)
    Public Const DTWAIN_ERR_OCR_INVALIDFILETYPE As Integer = (-2104)
    Public Const DTWAIN_ERR_OCR_INVALIDPIXELTYPE As Integer = (-2105)
    Public Const DTWAIN_ERR_OCR_INVALIDBITDEPTH As Integer = (-2106)
    Public Const DTWAIN_ERR_OCR_RECOGNITIONERROR As Integer = (-2107)
    Public Const DTWAIN_ERR_OCR_LAST As Integer = (-2108)
    Public Const DTWAIN_ERR_LAST As Integer = DTWAIN_ERR_OCR_LAST
    Public Const DTWAIN_ERR_SOURCE_COULD_NOT_OPEN As Integer = (-2500)
    Public Const DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE As Integer = (-2501)
    Public Const DTWAIN_ERR_IMAGEINFO_INVALID As Integer = (-2502)
    Public Const DTWAIN_ERR_WRITEDATA_TOFILE As Integer = (-2503)
    Public Const DTWAIN_ERR_OPERATION_NOTSUPPORTED As Integer = (-2504)
    Public Const DTWAIN_DE_CHKAUTOCAPTURE As Integer = 1
    Public Const DTWAIN_DE_CHKBATTERY As Integer = 2
    Public Const DTWAIN_DE_CHKDEVICEONLINE As Integer = 4
    Public Const DTWAIN_DE_CHKFLASH As Integer = 8
    Public Const DTWAIN_DE_CHKPOWERSUPPLY As Integer = 16
    Public Const DTWAIN_DE_CHKRESOLUTION As Integer = 32
    Public Const DTWAIN_DE_DEVICEADDED As Integer = 64
    Public Const DTWAIN_DE_DEVICEOFFLINE As Integer = 128
    Public Const DTWAIN_DE_DEVICEREADY As Integer = 256
    Public Const DTWAIN_DE_DEVICEREMOVED As Integer = 512
    Public Const DTWAIN_DE_IMAGECAPTURED As Integer = 1024
    Public Const DTWAIN_DE_IMAGEDELETED As Integer = 2048
    Public Const DTWAIN_DE_PAPERDOUBLEFEED As Integer = 4096
    Public Const DTWAIN_DE_PAPERJAM As Integer = 8192
    Public Const DTWAIN_DE_LAMPFAILURE As Integer = 16384
    Public Const DTWAIN_DE_POWERSAVE As Integer = 32768
    Public Const DTWAIN_DE_POWERSAVENOTIFY As Integer = 65536
    Public Const DTWAIN_DE_CUSTOMEVENTS As Integer = &H8000
    Public Const DTWAIN_GETDE_EVENT As Integer = 0
    Public Const DTWAIN_GETDE_DEVNAME As Integer = 1
    Public Const DTWAIN_GETDE_BATTERYMINUTES As Integer = 2
    Public Const DTWAIN_GETDE_BATTERYPCT As Integer = 3
    Public Const DTWAIN_GETDE_XRESOLUTION As Integer = 4
    Public Const DTWAIN_GETDE_YRESOLUTION As Integer = 5
    Public Const DTWAIN_GETDE_FLASHUSED As Integer = 6
    Public Const DTWAIN_GETDE_AUTOCAPTURE As Integer = 7
    Public Const DTWAIN_GETDE_TIMEBEFORECAPTURE As Integer = 8
    Public Const DTWAIN_GETDE_TIMEBETWEENCAPTURES As Integer = 9
    Public Const DTWAIN_GETDE_POWERSUPPLY As Integer = 10
    Public Const DTWAIN_IMPRINTERTOPBEFORE As Integer = 1
    Public Const DTWAIN_IMPRINTERTOPAFTER As Integer = 2
    Public Const DTWAIN_IMPRINTERBOTTOMBEFORE As Integer = 4
    Public Const DTWAIN_IMPRINTERBOTTOMAFTER As Integer = 8
    Public Const DTWAIN_ENDORSERTOPBEFORE As Integer = 16
    Public Const DTWAIN_ENDORSERTOPAFTER As Integer = 32
    Public Const DTWAIN_ENDORSERBOTTOMBEFORE As Integer = 64
    Public Const DTWAIN_ENDORSERBOTTOMAFTER As Integer = 128
    Public Const DTWAIN_PM_SINGLESTRING As Integer = 0
    Public Const DTWAIN_PM_MULTISTRING As Integer = 1
    Public Const DTWAIN_PM_COMPOUNDSTRING As Integer = 2
    Public Const DTWAIN_TWTY_INT8 As Integer = &H0000
    Public Const DTWAIN_TWTY_INT16 As Integer = &H0001
    Public Const DTWAIN_TWTY_INT32 As Integer = &H0002
    Public Const DTWAIN_TWTY_UINT8 As Integer = &H0003
    Public Const DTWAIN_TWTY_UINT16 As Integer = &H0004
    Public Const DTWAIN_TWTY_UINT32 As Integer = &H0005
    Public Const DTWAIN_TWTY_BOOL As Integer = &H0006
    Public Const DTWAIN_TWTY_FIX32 As Integer = &H0007
    Public Const DTWAIN_TWTY_FRAME As Integer = &H0008
    Public Const DTWAIN_TWTY_STR32 As Integer = &H0009
    Public Const DTWAIN_TWTY_STR64 As Integer = &H000A
    Public Const DTWAIN_TWTY_STR128 As Integer = &H000B
    Public Const DTWAIN_TWTY_STR255 As Integer = &H000C
    Public Const DTWAIN_TWTY_STR1024 As Integer = &H000D
    Public Const DTWAIN_TWTY_UNI512 As Integer = &H000E
    Public Const DTWAIN_EI_BARCODEX As Integer = &H1200
    Public Const DTWAIN_EI_BARCODEY As Integer = &H1201
    Public Const DTWAIN_EI_BARCODETEXT As Integer = &H1202
    Public Const DTWAIN_EI_BARCODETYPE As Integer = &H1203
    Public Const DTWAIN_EI_DESHADETOP As Integer = &H1204
    Public Const DTWAIN_EI_DESHADELEFT As Integer = &H1205
    Public Const DTWAIN_EI_DESHADEHEIGHT As Integer = &H1206
    Public Const DTWAIN_EI_DESHADEWIDTH As Integer = &H1207
    Public Const DTWAIN_EI_DESHADESIZE As Integer = &H1208
    Public Const DTWAIN_EI_SPECKLESREMOVED As Integer = &H1209
    Public Const DTWAIN_EI_HORZLINEXCOORD As Integer = &H120A
    Public Const DTWAIN_EI_HORZLINEYCOORD As Integer = &H120B
    Public Const DTWAIN_EI_HORZLINELENGTH As Integer = &H120C
    Public Const DTWAIN_EI_HORZLINETHICKNESS As Integer = &H120D
    Public Const DTWAIN_EI_VERTLINEXCOORD As Integer = &H120E
    Public Const DTWAIN_EI_VERTLINEYCOORD As Integer = &H120F
    Public Const DTWAIN_EI_VERTLINELENGTH As Integer = &H1210
    Public Const DTWAIN_EI_VERTLINETHICKNESS As Integer = &H1211
    Public Const DTWAIN_EI_PATCHCODE As Integer = &H1212
    Public Const DTWAIN_EI_ENDORSEDTEXT As Integer = &H1213
    Public Const DTWAIN_EI_FORMCONFIDENCE As Integer = &H1214
    Public Const DTWAIN_EI_FORMTEMPLATEMATCH As Integer = &H1215
    Public Const DTWAIN_EI_FORMTEMPLATEPAGEMATCH As Integer = &H1216
    Public Const DTWAIN_EI_FORMHORZDOCOFFSET As Integer = &H1217
    Public Const DTWAIN_EI_FORMVERTDOCOFFSET As Integer = &H1218
    Public Const DTWAIN_EI_BARCODECOUNT As Integer = &H1219
    Public Const DTWAIN_EI_BARCODECONFIDENCE As Integer = &H121A
    Public Const DTWAIN_EI_BARCODEROTATION As Integer = &H121B
    Public Const DTWAIN_EI_BARCODETEXTLENGTH As Integer = &H121C
    Public Const DTWAIN_EI_DESHADECOUNT As Integer = &H121D
    Public Const DTWAIN_EI_DESHADEBLACKCOUNTOLD As Integer = &H121E
    Public Const DTWAIN_EI_DESHADEBLACKCOUNTNEW As Integer = &H121F
    Public Const DTWAIN_EI_DESHADEBLACKRLMIN As Integer = &H1220
    Public Const DTWAIN_EI_DESHADEBLACKRLMAX As Integer = &H1221
    Public Const DTWAIN_EI_DESHADEWHITECOUNTOLD As Integer = &H1222
    Public Const DTWAIN_EI_DESHADEWHITECOUNTNEW As Integer = &H1223
    Public Const DTWAIN_EI_DESHADEWHITERLMIN As Integer = &H1224
    Public Const DTWAIN_EI_DESHADEWHITERLAVE As Integer = &H1225
    Public Const DTWAIN_EI_DESHADEWHITERLMAX As Integer = &H1226
    Public Const DTWAIN_EI_BLACKSPECKLESREMOVED As Integer = &H1227
    Public Const DTWAIN_EI_WHITESPECKLESREMOVED As Integer = &H1228
    Public Const DTWAIN_EI_HORZLINECOUNT As Integer = &H1229
    Public Const DTWAIN_EI_VERTLINECOUNT As Integer = &H122A
    Public Const DTWAIN_EI_DESKEWSTATUS As Integer = &H122B
    Public Const DTWAIN_EI_SKEWORIGINALANGLE As Integer = &H122C
    Public Const DTWAIN_EI_SKEWFINALANGLE As Integer = &H122D
    Public Const DTWAIN_EI_SKEWCONFIDENCE As Integer = &H122E
    Public Const DTWAIN_EI_SKEWWINDOWX1 As Integer = &H122F
    Public Const DTWAIN_EI_SKEWWINDOWY1 As Integer = &H1230
    Public Const DTWAIN_EI_SKEWWINDOWX2 As Integer = &H1231
    Public Const DTWAIN_EI_SKEWWINDOWY2 As Integer = &H1232
    Public Const DTWAIN_EI_SKEWWINDOWX3 As Integer = &H1233
    Public Const DTWAIN_EI_SKEWWINDOWY3 As Integer = &H1234
    Public Const DTWAIN_EI_SKEWWINDOWX4 As Integer = &H1235
    Public Const DTWAIN_EI_SKEWWINDOWY4 As Integer = &H1236
    Public Const DTWAIN_EI_BOOKNAME As Integer = &H1238
    Public Const DTWAIN_EI_CHAPTERNUMBER As Integer = &H1239
    Public Const DTWAIN_EI_DOCUMENTNUMBER As Integer = &H123A
    Public Const DTWAIN_EI_PAGENUMBER As Integer = &H123B
    Public Const DTWAIN_EI_CAMERA As Integer = &H123C
    Public Const DTWAIN_EI_FRAMENUMBER As Integer = &H123D
    Public Const DTWAIN_EI_FRAME As Integer = &H123E
    Public Const DTWAIN_EI_PIXELFLAVOR As Integer = &H123F
    Public Const DTWAIN_EI_ICCPROFILE As Integer = &H1240
    Public Const DTWAIN_EI_LASTSEGMENT As Integer = &H1241
    Public Const DTWAIN_EI_SEGMENTNUMBER As Integer = &H1242
    Public Const DTWAIN_EI_MAGDATA As Integer = &H1243
    Public Const DTWAIN_EI_MAGTYPE As Integer = &H1244
    Public Const DTWAIN_EI_PAGESIDE As Integer = &H1245
    Public Const DTWAIN_EI_FILESYSTEMSOURCE As Integer = &H1246
    Public Const DTWAIN_EI_IMAGEMERGED As Integer = &H1247
    Public Const DTWAIN_EI_MAGDATALENGTH As Integer = &H1248
    Public Const DTWAIN_EI_PAPERCOUNT As Integer = &H1249
    Public Const DTWAIN_EI_PRINTERTEXT As Integer = &H124A
    Public Const DTWAIN_EI_TWAINDIRECTMETADATA As Integer = &H124B
    Public Const DTWAIN_EI_IAFIELDA_VALUE As Integer = &H124C
    Public Const DTWAIN_EI_IAFIELDB_VALUE As Integer = &H124D
    Public Const DTWAIN_EI_IAFIELDC_VALUE As Integer = &H124E
    Public Const DTWAIN_EI_IAFIELDD_VALUE As Integer = &H124F
    Public Const DTWAIN_EI_IAFIELDE_VALUE As Integer = &H1250
    Public Const DTWAIN_EI_IALEVEL As Integer = &H1251
    Public Const DTWAIN_EI_PRINTER As Integer = &H1252
    Public Const DTWAIN_EI_BARCODETEXT2 As Integer = &H1253
    Public Const DTWAIN_LOG_DECODE_SOURCE As UInteger = &H1
    Public Const DTWAIN_LOG_DECODE_DEST As UInteger = &H2
    Public Const DTWAIN_LOG_DECODE_TWMEMREF As UInteger = &H4
    Public Const DTWAIN_LOG_DECODE_TWEVENT As UInteger = &H8
    Public Const DTWAIN_LOG_CALLSTACK As UInteger = &H10
    Public Const DTWAIN_LOG_ISTWAINMSG As UInteger = &H20
    Public Const DTWAIN_LOG_INITFAILURE As UInteger = &H40
    Public Const DTWAIN_LOG_LOWLEVELTWAIN As UInteger = &H80
    Public Const DTWAIN_LOG_DECODE_BITMAP As UInteger = &H100
    Public Const DTWAIN_LOG_NOTIFICATIONS As UInteger = &H200
    Public Const DTWAIN_LOG_MISCELLANEOUS As UInteger = &H400
    Public Const DTWAIN_LOG_DTWAINERRORS As UInteger = &H800
    Public Const DTWAIN_LOG_USEFILE As UInteger = &H10000
    Public Const DTWAIN_LOG_SHOWEXCEPTIONS As UInteger = &H20000
    Public Const DTWAIN_LOG_ERRORMSGBOX As UInteger = &H40000
    Public Const DTWAIN_LOG_USEBUFFER As UInteger = &H80000
    Public Const DTWAIN_LOG_FILEAPPEND As UInteger = &H100000
    Public Const DTWAIN_LOG_USECALLBACK As UInteger = &H200000
    Public Const DTWAIN_LOG_USECRLF As UInteger = &H400000
    Public Const DTWAIN_LOG_CONSOLE As UInteger = &H800000
    Public Const DTWAIN_LOG_DEBUGMONITOR As UInteger = &H1000000
    Public Const DTWAIN_LOG_USEWINDOW As UInteger = &H2000000
    Public Const DTWAIN_LOG_CREATEDIRECTORY As UInteger = &H04000000
    Public Const DTWAIN_LOG_CONSOLEWITHHANDLER As UInteger = (&H08000000 Or DTWAIN_LOG_CONSOLE)
    Public Const DTWAIN_LOG_ALL As UInteger = (DTWAIN_LOG_DECODE_SOURCE Or DTWAIN_LOG_DECODE_DEST Or DTWAIN_LOG_DECODE_TWEVENT Or DTWAIN_LOG_DECODE_TWMEMREF Or DTWAIN_LOG_CALLSTACK Or DTWAIN_LOG_ISTWAINMSG Or DTWAIN_LOG_INITFAILURE Or DTWAIN_LOG_LOWLEVELTWAIN Or DTWAIN_LOG_NOTIFICATIONS Or DTWAIN_LOG_MISCELLANEOUS Or DTWAIN_LOG_DTWAINERRORS Or DTWAIN_LOG_DECODE_BITMAP)
    Public Const DTWAIN_LOG_ALL_APPEND As UInteger = &HFFFFFFFFUI
    Public Const DTWAIN_TEMPDIR_CREATEDIRECTORY As UInteger = DTWAIN_LOG_CREATEDIRECTORY
    Public Const DTWAINGCD_RETURNHANDLE As Integer = 1
    Public Const DTWAINGCD_COPYDATA As Integer = 2
    Public Const DTWAIN_BYPOSITION As Integer = 0
    Public Const DTWAIN_BYID As Integer = 1
    Public Const DTWAINSCD_USEHANDLE As Integer = 1
    Public Const DTWAINSCD_USEDATA As Integer = 2
    Public Const DTWAIN_PAGEFAIL_RETRY As Integer = 1
    Public Const DTWAIN_PAGEFAIL_TERMINATE As Integer = 2
    Public Const DTWAIN_MAXRETRY_ATTEMPTS As Integer = 3
    Public Const DTWAIN_RETRY_FOREVER As Integer = (-1)
    Public Const DTWAIN_PDF_NOSCALING As Integer = 128
    Public Const DTWAIN_PDF_FITPAGE As Integer = 256
    Public Const DTWAIN_PDF_VARIABLEPAGESIZE As Integer = 512
    Public Const DTWAIN_PDF_CUSTOMSIZE As Integer = 1024
    Public Const DTWAIN_PDF_USECOMPRESSION As Integer = 2048
    Public Const DTWAIN_PDF_CUSTOMSCALE As Integer = 4096
    Public Const DTWAIN_PDF_PIXELSPERMETERSIZE As Integer = 8192
    Public Const DTWAIN_PDF_ALLOWPRINTING As UInteger = 2052
    Public Const DTWAIN_PDF_ALLOWMOD As UInteger = 8
    Public Const DTWAIN_PDF_ALLOWCOPY As UInteger = 16
    Public Const DTWAIN_PDF_ALLOWMODANNOTATIONS As UInteger = 32
    Public Const DTWAIN_PDF_ALLOWFILLIN As UInteger = 256
    Public Const DTWAIN_PDF_ALLOWEXTRACTION As UInteger = 512
    Public Const DTWAIN_PDF_ALLOWASSEMBLY As UInteger = 1024
    Public Const DTWAIN_PDF_ALLOWDEGRADEDPRINTING As UInteger = 4
    Public Const DTWAIN_PDF_ALLOWALL As UInteger = &HFFFFFFFCUI
    Public Const DTWAIN_PDF_ALLOWANYMOD As UInteger = (DTWAIN_PDF_ALLOWMOD Or DTWAIN_PDF_ALLOWFILLIN Or DTWAIN_PDF_ALLOWMODANNOTATIONS Or DTWAIN_PDF_ALLOWASSEMBLY)
    Public Const DTWAIN_PDF_ALLOWANYPRINTING As UInteger = (DTWAIN_PDF_ALLOWPRINTING Or DTWAIN_PDF_ALLOWDEGRADEDPRINTING)
    Public Const DTWAIN_PDF_PORTRAIT As Integer = 0
    Public Const DTWAIN_PDF_LANDSCAPE As Integer = 1
    Public Const DTWAIN_PS_REGULAR As Integer = 0
    Public Const DTWAIN_PS_ENCAPSULATED As Integer = 1
    Public Const DTWAIN_BP_AUTODISCARD_NONE As Integer = 0
    Public Const DTWAIN_BP_AUTODISCARD_IMMEDIATE As Integer = 1
    Public Const DTWAIN_BP_AUTODISCARD_AFTERPROCESS As Integer = 2
    Public Const DTWAIN_BP_DETECTORIGINAL As Integer = 1
    Public Const DTWAIN_BP_DETECTADJUSTED As Integer = 2
    Public Const DTWAIN_BP_DETECTALL As Integer = (DTWAIN_BP_DETECTORIGINAL Or DTWAIN_BP_DETECTADJUSTED)
    Public Const DTWAIN_BP_DISABLE As Integer = (-2)
    Public Const DTWAIN_BP_AUTO As Integer = (-1)
    Public Const DTWAIN_BP_AUTODISCARD_ANY As UInteger = &HFFFF
    Public Const DTWAIN_LP_REFLECTIVE As Integer = 0
    Public Const DTWAIN_LP_TRANSMISSIVE As Integer = 1
    Public Const DTWAIN_LS_RED As Integer = 0
    Public Const DTWAIN_LS_GREEN As Integer = 1
    Public Const DTWAIN_LS_BLUE As Integer = 2
    Public Const DTWAIN_LS_NONE As Integer = 3
    Public Const DTWAIN_LS_WHITE As Integer = 4
    Public Const DTWAIN_LS_UV As Integer = 5
    Public Const DTWAIN_LS_IR As Integer = 6
    Public Const DTWAIN_DLG_SORTNAMES As Integer = 1
    Public Const DTWAIN_DLG_CENTER As Integer = 2
    Public Const DTWAIN_DLG_CENTER_SCREEN As Integer = 4
    Public Const DTWAIN_DLG_USETEMPLATE As Integer = 8
    Public Const DTWAIN_DLG_CLEAR_PARAMS As Integer = 16
    Public Const DTWAIN_DLG_HORIZONTALSCROLL As Integer = 32
    Public Const DTWAIN_DLG_USEINCLUDENAMES As Integer = 64
    Public Const DTWAIN_DLG_USEEXCLUDENAMES As Integer = 128
    Public Const DTWAIN_DLG_USENAMEMAPPING As Integer = 256
    Public Const DTWAIN_DLG_TOPMOSTWINDOW As Integer = 1024
    Public Const DTWAIN_DLG_OPENONSELECT As Integer = 2048
    Public Const DTWAIN_DLG_NOOPENONSELECT As Integer = 4096
    Public Const DTWAIN_DLG_HIGHLIGHTFIRST As Integer = 8192
    Public Const DTWAIN_RES_ENGLISH As Integer = 0
    Public Const DTWAIN_RES_FRENCH As Integer = 1
    Public Const DTWAIN_RES_SPANISH As Integer = 2
    Public Const DTWAIN_RES_DUTCH As Integer = 3
    Public Const DTWAIN_RES_GERMAN As Integer = 4
    Public Const DTWAIN_RES_ITALIAN As Integer = 5
    Public Const DTWAIN_AL_ALARM As Integer = 0
    Public Const DTWAIN_AL_FEEDERERROR As Integer = 1
    Public Const DTWAIN_AL_FEEDERWARNING As Integer = 2
    Public Const DTWAIN_AL_BARCODE As Integer = 3
    Public Const DTWAIN_AL_DOUBLEFEED As Integer = 4
    Public Const DTWAIN_AL_JAM As Integer = 5
    Public Const DTWAIN_AL_PATCHCODE As Integer = 6
    Public Const DTWAIN_AL_POWER As Integer = 7
    Public Const DTWAIN_AL_SKEW As Integer = 8
    Public Const DTWAIN_FT_CAMERA As Integer = 0
    Public Const DTWAIN_FT_CAMERATOP As Integer = 1
    Public Const DTWAIN_FT_CAMERABOTTOM As Integer = 2
    Public Const DTWAIN_FT_CAMERAPREVIEW As Integer = 3
    Public Const DTWAIN_FT_DOMAIN As Integer = 4
    Public Const DTWAIN_FT_HOST As Integer = 5
    Public Const DTWAIN_FT_DIRECTORY As Integer = 6
    Public Const DTWAIN_FT_IMAGE As Integer = 7
    Public Const DTWAIN_FT_UNKNOWN As Integer = 8
    Public Const DTWAIN_NF_NONE As Integer = 0
    Public Const DTWAIN_NF_AUTO As Integer = 1
    Public Const DTWAIN_NF_LONEPIXEL As Integer = 2
    Public Const DTWAIN_NF_MAJORITYRULE As Integer = 3
    Public Const DTWAIN_CB_AUTO As Integer = 0
    Public Const DTWAIN_CB_CLEAR As Integer = 1
    Public Const DTWAIN_CB_NOCLEAR As Integer = 2
    Public Const DTWAIN_FA_NONE As Integer = 0
    Public Const DTWAIN_FA_LEFT As Integer = 1
    Public Const DTWAIN_FA_CENTER As Integer = 2
    Public Const DTWAIN_FA_RIGHT As Integer = 3
    Public Const DTWAIN_PF_CHOCOLATE As Integer = 0
    Public Const DTWAIN_PF_VANILLA As Integer = 1
    Public Const DTWAIN_FO_FIRSTPAGEFIRST As Integer = 0
    Public Const DTWAIN_FO_LASTPAGEFIRST As Integer = 1
    Public Const DTWAIN_INCREMENT_STATIC As Integer = 0
    Public Const DTWAIN_INCREMENT_DYNAMIC As Integer = 1
    Public Const DTWAIN_INCREMENT_DEFAULT As Integer = -1
    Public Const DTWAIN_MANDUP_FACEUPTOPPAGE As Integer = 0
    Public Const DTWAIN_MANDUP_FACEUPBOTTOMPAGE As Integer = 1
    Public Const DTWAIN_MANDUP_FACEDOWNTOPPAGE As Integer = 2
    Public Const DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE As Integer = 3
    Public Const DTWAIN_FILESAVE_DEFAULT As Integer = 0
    Public Const DTWAIN_FILESAVE_UICLOSE As Integer = 1
    Public Const DTWAIN_FILESAVE_SOURCECLOSE As Integer = 2
    Public Const DTWAIN_FILESAVE_ENDACQUIRE As Integer = 3
    Public Const DTWAIN_FILESAVE_MANUALSAVE As Integer = 4
    Public Const DTWAIN_FILESAVE_SAVEINCOMPLETE As Integer = 128
    Public Const DTWAIN_MANDUP_SCANOK As Integer = 1
    Public Const DTWAIN_MANDUP_SIDE1RESCAN As Integer = 2
    Public Const DTWAIN_MANDUP_SIDE2RESCAN As Integer = 3
    Public Const DTWAIN_MANDUP_RESCANALL As Integer = 4
    Public Const DTWAIN_MANDUP_PAGEMISSING As Integer = 5
    Public Const DTWAIN_DEMODLL_VERSION As Integer = &H00000001
    Public Const DTWAIN_UNLICENSED_VERSION As Integer = &H00000002
    Public Const DTWAIN_COMPANY_VERSION As Integer = &H00000004
    Public Const DTWAIN_GENERAL_VERSION As Integer = &H00000008
    Public Const DTWAIN_DEVELOP_VERSION As Integer = &H00000010
    Public Const DTWAIN_JAVA_VERSION As Integer = &H00000020
    Public Const DTWAIN_TOOLKIT_VERSION As Integer = &H00000040
    Public Const DTWAIN_LIMITEDDLL_VERSION As Integer = &H00000080
    Public Const DTWAIN_STATICLIB_VERSION As Integer = &H00000100
    Public Const DTWAIN_STATICLIB_STDCALL_VERSION As Integer = &H00000200
    Public Const DTWAIN_PDF_VERSION As Integer = &H00010000
    Public Const DTWAIN_TWAINSAVE_VERSION As Integer = &H00020000
    Public Const DTWAIN_OCR_VERSION As Integer = &H00040000
    Public Const DTWAIN_BARCODE_VERSION As Integer = &H00080000
    Public Const DTWAIN_ACTIVEX_VERSION As Integer = &H00100000
    Public Const DTWAIN_32BIT_VERSION As Integer = &H00200000
    Public Const DTWAIN_64BIT_VERSION As Integer = &H00400000
    Public Const DTWAIN_UNICODE_VERSION As Integer = &H00800000
    Public Const DTWAIN_OPENSOURCE_VERSION As Integer = &H01000000
    Public Const DTWAIN_CALLSTACK_LOGGING As Integer = &H02000000
    Public Const DTWAIN_CALLSTACK_LOGGING_PLUS As Integer = &H04000000
    Public Const DTWAINOCR_RETURNHANDLE As Integer = 1
    Public Const DTWAINOCR_COPYDATA As Integer = 2
    Public Const DTWAIN_OCRINFO_CHAR As Integer = 0
    Public Const DTWAIN_OCRINFO_CHARXPOS As Integer = 1
    Public Const DTWAIN_OCRINFO_CHARYPOS As Integer = 2
    Public Const DTWAIN_OCRINFO_CHARXWIDTH As Integer = 3
    Public Const DTWAIN_OCRINFO_CHARYWIDTH As Integer = 4
    Public Const DTWAIN_OCRINFO_CHARCONFIDENCE As Integer = 5
    Public Const DTWAIN_OCRINFO_PAGENUM As Integer = 6
    Public Const DTWAIN_OCRINFO_OCRENGINE As Integer = 7
    Public Const DTWAIN_OCRINFO_TEXTLENGTH As Integer = 8
    Public Const DTWAIN_PDFPAGETYPE_COLOR As Integer = 0
    Public Const DTWAIN_PDFPAGETYPE_BW As Integer = 1
    Public Const DTWAIN_TWAINDSM_LEGACY As Integer = 1
    Public Const DTWAIN_TWAINDSM_VERSION2 As Integer = 2
    Public Const DTWAIN_TWAINDSM_LATESTVERSION As Integer = 4
    Public Const DTWAIN_TWAINDSMSEARCH_NOTFOUND As Integer = (-1)
    Public Const DTWAIN_TWAINDSMSEARCH_WSO As Integer = 0
    Public Const DTWAIN_TWAINDSMSEARCH_WOS As Integer = 1
    Public Const DTWAIN_TWAINDSMSEARCH_SWO As Integer = 2
    Public Const DTWAIN_TWAINDSMSEARCH_SOW As Integer = 3
    Public Const DTWAIN_TWAINDSMSEARCH_OWS As Integer = 4
    Public Const DTWAIN_TWAINDSMSEARCH_OSW As Integer = 5
    Public Const DTWAIN_TWAINDSMSEARCH_W As Integer = 6
    Public Const DTWAIN_TWAINDSMSEARCH_S As Integer = 7
    Public Const DTWAIN_TWAINDSMSEARCH_O As Integer = 8
    Public Const DTWAIN_TWAINDSMSEARCH_WS As Integer = 9
    Public Const DTWAIN_TWAINDSMSEARCH_WO As Integer = 10
    Public Const DTWAIN_TWAINDSMSEARCH_SW As Integer = 11
    Public Const DTWAIN_TWAINDSMSEARCH_SO As Integer = 12
    Public Const DTWAIN_TWAINDSMSEARCH_OW As Integer = 13
    Public Const DTWAIN_TWAINDSMSEARCH_OS As Integer = 14
    Public Const DTWAIN_TWAINDSMSEARCH_C As Integer = 15
    Public Const DTWAIN_TWAINDSMSEARCH_U As Integer = 16
    Public Const DTWAIN_PDFPOLARITY_POSITIVE As Integer = 1
    Public Const DTWAIN_PDFPOLARITY_NEGATIVE As Integer = 2
    Public Const DTWAIN_TWPF_NORMAL As Integer = 0
    Public Const DTWAIN_TWPF_BOLD As Integer = 1
    Public Const DTWAIN_TWPF_ITALIC As Integer = 2
    Public Const DTWAIN_TWPF_LARGESIZE As Integer = 3
    Public Const DTWAIN_TWPF_SMALLSIZE As Integer = 4
    Public Const DTWAIN_TWCT_PAGE As Integer = 0
    Public Const DTWAIN_TWCT_PATCH1 As Integer = 1
    Public Const DTWAIN_TWCT_PATCH2 As Integer = 2
    Public Const DTWAIN_TWCT_PATCH3 As Integer = 3
    Public Const DTWAIN_TWCT_PATCH4 As Integer = 4
    Public Const DTWAIN_TWCT_PATCH5 As Integer = 5
    Public Const DTWAIN_TWCT_PATCH6 As Integer = 6
    Public Const DTWAIN_AUTOSIZE_NONE As Integer = 0
    Public Const DTWAIN_CV_CAPCUSTOMBASE As Integer = &H8000
    Public Const DTWAIN_CV_CAPXFERCOUNT As Integer = &H0001
    Public Const DTWAIN_CV_ICAPCOMPRESSION As Integer = &H0100
    Public Const DTWAIN_CV_ICAPPIXELTYPE As Integer = &H0101
    Public Const DTWAIN_CV_ICAPUNITS As Integer = &H0102
    Public Const DTWAIN_CV_ICAPXFERMECH As Integer = &H0103
    Public Const DTWAIN_CV_CAPAUTHOR As Integer = &H1000
    Public Const DTWAIN_CV_CAPCAPTION As Integer = &H1001
    Public Const DTWAIN_CV_CAPFEEDERENABLED As Integer = &H1002
    Public Const DTWAIN_CV_CAPFEEDERLOADED As Integer = &H1003
    Public Const DTWAIN_CV_CAPTIMEDATE As Integer = &H1004
    Public Const DTWAIN_CV_CAPSUPPORTEDCAPS As Integer = &H1005
    Public Const DTWAIN_CV_CAPEXTENDEDCAPS As Integer = &H1006
    Public Const DTWAIN_CV_CAPAUTOFEED As Integer = &H1007
    Public Const DTWAIN_CV_CAPCLEARPAGE As Integer = &H1008
    Public Const DTWAIN_CV_CAPFEEDPAGE As Integer = &H1009
    Public Const DTWAIN_CV_CAPREWINDPAGE As Integer = &H100a
    Public Const DTWAIN_CV_CAPINDICATORS As Integer = &H100b
    Public Const DTWAIN_CV_CAPSUPPORTEDCAPSEXT As Integer = &H100c
    Public Const DTWAIN_CV_CAPPAPERDETECTABLE As Integer = &H100d
    Public Const DTWAIN_CV_CAPUICONTROLLABLE As Integer = &H100e
    Public Const DTWAIN_CV_CAPDEVICEONLINE As Integer = &H100f
    Public Const DTWAIN_CV_CAPAUTOSCAN As Integer = &H1010
    Public Const DTWAIN_CV_CAPTHUMBNAILSENABLED As Integer = &H1011
    Public Const DTWAIN_CV_CAPDUPLEX As Integer = &H1012
    Public Const DTWAIN_CV_CAPDUPLEXENABLED As Integer = &H1013
    Public Const DTWAIN_CV_CAPENABLEDSUIONLY As Integer = &H1014
    Public Const DTWAIN_CV_CAPCUSTOMDSDATA As Integer = &H1015
    Public Const DTWAIN_CV_CAPENDORSER As Integer = &H1016
    Public Const DTWAIN_CV_CAPJOBCONTROL As Integer = &H1017
    Public Const DTWAIN_CV_CAPALARMS As Integer = &H1018
    Public Const DTWAIN_CV_CAPALARMVOLUME As Integer = &H1019
    Public Const DTWAIN_CV_CAPAUTOMATICCAPTURE As Integer = &H101a
    Public Const DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE As Integer = &H101b
    Public Const DTWAIN_CV_CAPTIMEBETWEENCAPTURES As Integer = &H101c
    Public Const DTWAIN_CV_CAPCLEARBUFFERS As Integer = &H101d
    Public Const DTWAIN_CV_CAPMAXBATCHBUFFERS As Integer = &H101e
    Public Const DTWAIN_CV_CAPDEVICETIMEDATE As Integer = &H101f
    Public Const DTWAIN_CV_CAPPOWERSUPPLY As Integer = &H1020
    Public Const DTWAIN_CV_CAPCAMERAPREVIEWUI As Integer = &H1021
    Public Const DTWAIN_CV_CAPDEVICEEVENT As Integer = &H1022
    Public Const DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE As Integer = &H1023
    Public Const DTWAIN_CV_CAPSERIALNUMBER As Integer = &H1024
    Public Const DTWAIN_CV_CAPFILESYSTEM As Integer = &H1025
    Public Const DTWAIN_CV_CAPPRINTER As Integer = &H1026
    Public Const DTWAIN_CV_CAPPRINTERENABLED As Integer = &H1027
    Public Const DTWAIN_CV_CAPPRINTERINDEX As Integer = &H1028
    Public Const DTWAIN_CV_CAPPRINTERMODE As Integer = &H1029
    Public Const DTWAIN_CV_CAPPRINTERSTRING As Integer = &H102a
    Public Const DTWAIN_CV_CAPPRINTERSUFFIX As Integer = &H102b
    Public Const DTWAIN_CV_CAPLANGUAGE As Integer = &H102c
    Public Const DTWAIN_CV_CAPFEEDERALIGNMENT As Integer = &H102d
    Public Const DTWAIN_CV_CAPFEEDERORDER As Integer = &H102e
    Public Const DTWAIN_CV_CAPPAPERBINDING As Integer = &H102f
    Public Const DTWAIN_CV_CAPREACQUIREALLOWED As Integer = &H1030
    Public Const DTWAIN_CV_CAPPASSTHRU As Integer = &H1031
    Public Const DTWAIN_CV_CAPBATTERYMINUTES As Integer = &H1032
    Public Const DTWAIN_CV_CAPBATTERYPERCENTAGE As Integer = &H1033
    Public Const DTWAIN_CV_CAPPOWERDOWNTIME As Integer = &H1034
    Public Const DTWAIN_CV_CAPSEGMENTED As Integer = &H1035
    Public Const DTWAIN_CV_CAPCAMERAENABLED As Integer = &H1036
    Public Const DTWAIN_CV_CAPCAMERAORDER As Integer = &H1037
    Public Const DTWAIN_CV_CAPMICRENABLED As Integer = &H1038
    Public Const DTWAIN_CV_CAPFEEDERPREP As Integer = &H1039
    Public Const DTWAIN_CV_CAPFEEDERPOCKET As Integer = &H103a
    Public Const DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM As Integer = &H103b
    Public Const DTWAIN_CV_CAPCUSTOMINTERFACEGUID As Integer = &H103c
    Public Const DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE As Integer = &H103d
    Public Const DTWAIN_CV_CAPSUPPORTEDDATS As Integer = &H103e
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTION As Integer = &H103f
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH As Integer = &H1040
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY As Integer = &H1041
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE As Integer = &H1042
    Public Const DTWAIN_CV_CAPPAPERHANDLING As Integer = &H1043
    Public Const DTWAIN_CV_CAPINDICATORSMODE As Integer = &H1044
    Public Const DTWAIN_CV_CAPPRINTERVERTICALOFFSET As Integer = &H1045
    Public Const DTWAIN_CV_CAPPOWERSAVETIME As Integer = &H1046
    Public Const DTWAIN_CV_CAPPRINTERCHARROTATION As Integer = &H1047
    Public Const DTWAIN_CV_CAPPRINTERFONTSTYLE As Integer = &H1048
    Public Const DTWAIN_CV_CAPPRINTERINDEXLEADCHAR As Integer = &H1049
    Public Const DTWAIN_CV_CAPIMAGEADDRESSENABLED As Integer = &H1050
    Public Const DTWAIN_CV_CAPIAFIELDA_LEVEL As Integer = &H1051
    Public Const DTWAIN_CV_CAPIAFIELDB_LEVEL As Integer = &H1052
    Public Const DTWAIN_CV_CAPIAFIELDC_LEVEL As Integer = &H1053
    Public Const DTWAIN_CV_CAPIAFIELDD_LEVEL As Integer = &H1054
    Public Const DTWAIN_CV_CAPIAFIELDE_LEVEL As Integer = &H1055
    Public Const DTWAIN_CV_CAPIAFIELDA_PRINTFORMAT As Integer = &H1056
    Public Const DTWAIN_CV_CAPIAFIELDB_PRINTFORMAT As Integer = &H1057
    Public Const DTWAIN_CV_CAPIAFIELDC_PRINTFORMAT As Integer = &H1058
    Public Const DTWAIN_CV_CAPIAFIELDD_PRINTFORMAT As Integer = &H1059
    Public Const DTWAIN_CV_CAPIAFIELDE_PRINTFORMAT As Integer = &H105A
    Public Const DTWAIN_CV_CAPIAFIELDA_VALUE As Integer = &H105B
    Public Const DTWAIN_CV_CAPIAFIELDB_VALUE As Integer = &H105C
    Public Const DTWAIN_CV_CAPIAFIELDC_VALUE As Integer = &H105D
    Public Const DTWAIN_CV_CAPIAFIELDD_VALUE As Integer = &H105E
    Public Const DTWAIN_CV_CAPIAFIELDE_VALUE As Integer = &H105F
    Public Const DTWAIN_CV_CAPIAFIELDA_LASTPAGE As Integer = &H1060
    Public Const DTWAIN_CV_CAPIAFIELDB_LASTPAGE As Integer = &H1061
    Public Const DTWAIN_CV_CAPIAFIELDC_LASTPAGE As Integer = &H1062
    Public Const DTWAIN_CV_CAPIAFIELDD_LASTPAGE As Integer = &H1063
    Public Const DTWAIN_CV_CAPIAFIELDE_LASTPAGE As Integer = &H1064
    Public Const DTWAIN_CV_CAPPRINTERINDEXMAXVALUE As Integer = &H104A
    Public Const DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS As Integer = &H104B
    Public Const DTWAIN_CV_CAPPRINTERINDEXSTEP As Integer = &H104C
    Public Const DTWAIN_CV_CAPPRINTERINDEXTRIGGER As Integer = &H104D
    Public Const DTWAIN_CV_CAPPRINTERSTRINGPREVIEW As Integer = &H104E
    Public Const DTWAIN_CV_ICAPAUTOBRIGHT As Integer = &H1100
    Public Const DTWAIN_CV_ICAPBRIGHTNESS As Integer = &H1101
    Public Const DTWAIN_CV_ICAPCONTRAST As Integer = &H1103
    Public Const DTWAIN_CV_ICAPCUSTHALFTONE As Integer = &H1104
    Public Const DTWAIN_CV_ICAPEXPOSURETIME As Integer = &H1105
    Public Const DTWAIN_CV_ICAPFILTER As Integer = &H1106
    Public Const DTWAIN_CV_ICAPFLASHUSED As Integer = &H1107
    Public Const DTWAIN_CV_ICAPGAMMA As Integer = &H1108
    Public Const DTWAIN_CV_ICAPHALFTONES As Integer = &H1109
    Public Const DTWAIN_CV_ICAPHIGHLIGHT As Integer = &H110a
    Public Const DTWAIN_CV_ICAPIMAGEFILEFORMAT As Integer = &H110c
    Public Const DTWAIN_CV_ICAPLAMPSTATE As Integer = &H110d
    Public Const DTWAIN_CV_ICAPLIGHTSOURCE As Integer = &H110e
    Public Const DTWAIN_CV_ICAPORIENTATION As Integer = &H1110
    Public Const DTWAIN_CV_ICAPPHYSICALWIDTH As Integer = &H1111
    Public Const DTWAIN_CV_ICAPPHYSICALHEIGHT As Integer = &H1112
    Public Const DTWAIN_CV_ICAPSHADOW As Integer = &H1113
    Public Const DTWAIN_CV_ICAPFRAMES As Integer = &H1114
    Public Const DTWAIN_CV_ICAPXNATIVERESOLUTION As Integer = &H1116
    Public Const DTWAIN_CV_ICAPYNATIVERESOLUTION As Integer = &H1117
    Public Const DTWAIN_CV_ICAPXRESOLUTION As Integer = &H1118
    Public Const DTWAIN_CV_ICAPYRESOLUTION As Integer = &H1119
    Public Const DTWAIN_CV_ICAPMAXFRAMES As Integer = &H111a
    Public Const DTWAIN_CV_ICAPTILES As Integer = &H111b
    Public Const DTWAIN_CV_ICAPBITORDER As Integer = &H111c
    Public Const DTWAIN_CV_ICAPCCITTKFACTOR As Integer = &H111d
    Public Const DTWAIN_CV_ICAPLIGHTPATH As Integer = &H111e
    Public Const DTWAIN_CV_ICAPPIXELFLAVOR As Integer = &H111f
    Public Const DTWAIN_CV_ICAPPLANARCHUNKY As Integer = &H1120
    Public Const DTWAIN_CV_ICAPROTATION As Integer = &H1121
    Public Const DTWAIN_CV_ICAPSUPPORTEDSIZES As Integer = &H1122
    Public Const DTWAIN_CV_ICAPTHRESHOLD As Integer = &H1123
    Public Const DTWAIN_CV_ICAPXSCALING As Integer = &H1124
    Public Const DTWAIN_CV_ICAPYSCALING As Integer = &H1125
    Public Const DTWAIN_CV_ICAPBITORDERCODES As Integer = &H1126
    Public Const DTWAIN_CV_ICAPPIXELFLAVORCODES As Integer = &H1127
    Public Const DTWAIN_CV_ICAPJPEGPIXELTYPE As Integer = &H1128
    Public Const DTWAIN_CV_ICAPTIMEFILL As Integer = &H112a
    Public Const DTWAIN_CV_ICAPBITDEPTH As Integer = &H112b
    Public Const DTWAIN_CV_ICAPBITDEPTHREDUCTION As Integer = &H112c
    Public Const DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE As Integer = &H112d
    Public Const DTWAIN_CV_ICAPIMAGEDATASET As Integer = &H112e
    Public Const DTWAIN_CV_ICAPEXTIMAGEINFO As Integer = &H112f
    Public Const DTWAIN_CV_ICAPMINIMUMHEIGHT As Integer = &H1130
    Public Const DTWAIN_CV_ICAPMINIMUMWIDTH As Integer = &H1131
    Public Const DTWAIN_CV_ICAPAUTOBORDERDETECTION As Integer = &H1132
    Public Const DTWAIN_CV_ICAPAUTODESKEW As Integer = &H1133
    Public Const DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES As Integer = &H1134
    Public Const DTWAIN_CV_ICAPAUTOROTATE As Integer = &H1135
    Public Const DTWAIN_CV_ICAPFLIPROTATION As Integer = &H1136
    Public Const DTWAIN_CV_ICAPBARCODEDETECTIONENABLED As Integer = &H1137
    Public Const DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES As Integer = &H1138
    Public Const DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES As Integer = &H1139
    Public Const DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES As Integer = &H113a
    Public Const DTWAIN_CV_ICAPBARCODESEARCHMODE As Integer = &H113b
    Public Const DTWAIN_CV_ICAPBARCODEMAXRETRIES As Integer = &H113c
    Public Const DTWAIN_CV_ICAPBARCODETIMEOUT As Integer = &H113d
    Public Const DTWAIN_CV_ICAPZOOMFACTOR As Integer = &H113e
    Public Const DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED As Integer = &H113f
    Public Const DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES As Integer = &H1140
    Public Const DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES As Integer = &H1141
    Public Const DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES As Integer = &H1142
    Public Const DTWAIN_CV_ICAPPATCHCODESEARCHMODE As Integer = &H1143
    Public Const DTWAIN_CV_ICAPPATCHCODEMAXRETRIES As Integer = &H1144
    Public Const DTWAIN_CV_ICAPPATCHCODETIMEOUT As Integer = &H1145
    Public Const DTWAIN_CV_ICAPFLASHUSED2 As Integer = &H1146
    Public Const DTWAIN_CV_ICAPIMAGEFILTER As Integer = &H1147
    Public Const DTWAIN_CV_ICAPNOISEFILTER As Integer = &H1148
    Public Const DTWAIN_CV_ICAPOVERSCAN As Integer = &H1149
    Public Const DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION As Integer = &H1150
    Public Const DTWAIN_CV_ICAPAUTOMATICDESKEW As Integer = &H1151
    Public Const DTWAIN_CV_ICAPAUTOMATICROTATE As Integer = &H1152
    Public Const DTWAIN_CV_ICAPJPEGQUALITY As Integer = &H1153
    Public Const DTWAIN_CV_ICAPFEEDERTYPE As Integer = &H1154
    Public Const DTWAIN_CV_ICAPICCPROFILE As Integer = &H1155
    Public Const DTWAIN_CV_ICAPAUTOSIZE As Integer = &H1156
    Public Const DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME As Integer = &H1157
    Public Const DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION As Integer = &H1158
    Public Const DTWAIN_CV_ICAPAUTOMATICCOLORENABLED As Integer = &H1159
    Public Const DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE As Integer = &H115a
    Public Const DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED As Integer = &H115b
    Public Const DTWAIN_CV_ICAPIMAGEMERGE As Integer = &H115c
    Public Const DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD As Integer = &H115d
    Public Const DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO As Integer = &H115e
    Public Const DTWAIN_CV_ICAPFILMTYPE As Integer = &H115f
    Public Const DTWAIN_CV_ICAPMIRROR As Integer = &H1160
    Public Const DTWAIN_CV_ICAPJPEGSUBSAMPLING As Integer = &H1161
    Public Const DTWAIN_CV_ACAPAUDIOFILEFORMAT As Integer = &H1201
    Public Const DTWAIN_CV_ACAPXFERMECH As Integer = &H1202
    Public Const DTWAIN_CFMCV_CAPCFMSTART As Integer = 2048
    Public Const DTWAIN_CFMCV_CAPDUPLEXSCANNER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+10)
    Public Const DTWAIN_CFMCV_CAPDUPLEXENABLE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+11)
    Public Const DTWAIN_CFMCV_CAPSCANNERNAME As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+12)
    Public Const DTWAIN_CFMCV_CAPSINGLEPASS As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+13)
    Public Const DTWAIN_CFMCV_CAPERRHANDLING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+20)
    Public Const DTWAIN_CFMCV_CAPFEEDERSTATUS As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+21)
    Public Const DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+22)
    Public Const DTWAIN_CFMCV_CAPFEEDWAITTIME As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+23)
    Public Const DTWAIN_CFMCV_ICAPWHITEBALANCE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+24)
    Public Const DTWAIN_CFMCV_ICAPAUTOBINARY As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+25)
    Public Const DTWAIN_CFMCV_ICAPIMAGESEPARATION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+26)
    Public Const DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+27)
    Public Const DTWAIN_CFMCV_ICAPIMAGEEMPHASIS As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+28)
    Public Const DTWAIN_CFMCV_ICAPOUTLINING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+29)
    Public Const DTWAIN_CFMCV_ICAPDYNTHRESHOLD As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+30)
    Public Const DTWAIN_CFMCV_ICAPVARIANCE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+31)
    Public Const DTWAIN_CFMCV_CAPENDORSERAVAILABLE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+32)
    Public Const DTWAIN_CFMCV_CAPENDORSERENABLE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+33)
    Public Const DTWAIN_CFMCV_CAPENDORSERCHARSET As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+34)
    Public Const DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+35)
    Public Const DTWAIN_CFMCV_CAPENDORSERSTRING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+36)
    Public Const DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+48)
    Public Const DTWAIN_CFMCV_ICAPSMOOTHINGMODE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+49)
    Public Const DTWAIN_CFMCV_ICAPFILTERMODE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+50)
    Public Const DTWAIN_CFMCV_ICAPGRADATION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+51)
    Public Const DTWAIN_CFMCV_ICAPMIRROR As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+52)
    Public Const DTWAIN_CFMCV_ICAPEASYSCANMODE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+53)
    Public Const DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+54)
    Public Const DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+55)
    Public Const DTWAIN_CFMCV_CAPDUPLEXPAGE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+56)
    Public Const DTWAIN_CFMCV_ICAPINVERTIMAGE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+57)
    Public Const DTWAIN_CFMCV_ICAPSPECKLEREMOVE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+58)
    Public Const DTWAIN_CFMCV_ICAPUSMFILTER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+59)
    Public Const DTWAIN_CFMCV_ICAPNOISEFILTERCFM As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+60)
    Public Const DTWAIN_CFMCV_ICAPDESCREENING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+61)
    Public Const DTWAIN_CFMCV_ICAPQUALITYFILTER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+62)
    Public Const DTWAIN_CFMCV_ICAPBINARYFILTER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+63)
    Public Const DTWAIN_OCRCV_IMAGEFILEFORMAT As Integer = &H1000
    Public Const DTWAIN_OCRCV_DESKEW As Integer = &H1001
    Public Const DTWAIN_OCRCV_DESHADE As Integer = &H1002
    Public Const DTWAIN_OCRCV_ORIENTATION As Integer = &H1003
    Public Const DTWAIN_OCRCV_NOISEREMOVE As Integer = &H1004
    Public Const DTWAIN_OCRCV_LINEREMOVE As Integer = &H1005
    Public Const DTWAIN_OCRCV_INVERTPAGE As Integer = &H1006
    Public Const DTWAIN_OCRCV_INVERTZONES As Integer = &H1007
    Public Const DTWAIN_OCRCV_LINEREJECT As Integer = &H1008
    Public Const DTWAIN_OCRCV_CHARACTERREJECT As Integer = &H1009
    Public Const DTWAIN_OCRCV_ERRORREPORTMODE As Integer = &H1010
    Public Const DTWAIN_OCRCV_ERRORREPORTFILE As Integer = &H1011
    Public Const DTWAIN_OCRCV_PIXELTYPE As Integer = &H1012
    Public Const DTWAIN_OCRCV_BITDEPTH As Integer = &H1013
    Public Const DTWAIN_OCRCV_RETURNCHARINFO As Integer = &H1014
    Public Const DTWAIN_OCRCV_NATIVEFILEFORMAT As Integer = &H1015
    Public Const DTWAIN_OCRCV_MPNATIVEFILEFORMAT As Integer = &H1016
    Public Const DTWAIN_OCRCV_SUPPORTEDCAPS As Integer = &H1017
    Public Const DTWAIN_OCRCV_DISABLECHARACTERS As Integer = &H1018
    Public Const DTWAIN_OCRCV_REMOVECONTROLCHARS As Integer = &H1019
    Public Const DTWAIN_OCRORIENT_OFF As Integer = 0
    Public Const DTWAIN_OCRORIENT_AUTO As Integer = 1
    Public Const DTWAIN_OCRORIENT_90 As Integer = 2
    Public Const DTWAIN_OCRORIENT_180 As Integer = 3
    Public Const DTWAIN_OCRORIENT_270 As Integer = 4
    Public Const DTWAIN_OCRIMAGEFORMAT_AUTO As Integer = 10000
    Public Const DTWAIN_OCRERROR_MODENONE As Integer = 0
    Public Const DTWAIN_OCRERROR_SHOWMSGBOX As Integer = 1
    Public Const DTWAIN_OCRERROR_WRITEFILE As Integer = 2
    Public Const DTWAIN_PDFTEXT_ALLPAGES As UInteger = &H00000001UI
    Public Const DTWAIN_PDFTEXT_EVENPAGES As UInteger = &H00000002UI
    Public Const DTWAIN_PDFTEXT_ODDPAGES As UInteger = &H00000004UI
    Public Const DTWAIN_PDFTEXT_FIRSTPAGE As UInteger = &H00000008UI
    Public Const DTWAIN_PDFTEXT_LASTPAGE As UInteger = &H00000010UI
    Public Const DTWAIN_PDFTEXT_CURRENTPAGE As UInteger = &H00000020UI
    Public Const DTWAIN_PDFTEXT_DISABLED As UInteger = &H00000040UI
    Public Const DTWAIN_PDFTEXT_TOPLEFT As UInteger = &H00000100UI
    Public Const DTWAIN_PDFTEXT_TOPRIGHT As UInteger = &H00000200UI
    Public Const DTWAIN_PDFTEXT_HORIZCENTER As UInteger = &H00000400UI
    Public Const DTWAIN_PDFTEXT_VERTCENTER As UInteger = &H00000800UI
    Public Const DTWAIN_PDFTEXT_BOTTOMLEFT As UInteger = &H00001000UI
    Public Const DTWAIN_PDFTEXT_BOTTOMRIGHT As UInteger = &H00002000UI
    Public Const DTWAIN_PDFTEXT_BOTTOMCENTER As UInteger = &H00004000UI
    Public Const DTWAIN_PDFTEXT_TOPCENTER As UInteger = &H00008000UI
    Public Const DTWAIN_PDFTEXT_XCENTER As UInteger = &H00010000UI
    Public Const DTWAIN_PDFTEXT_YCENTER As UInteger = &H00020000UI
    Public Const DTWAIN_PDFTEXT_NOSCALING As UInteger = &H00100000UI
    Public Const DTWAIN_PDFTEXT_NOCHARSPACING As UInteger = &H00200000UI
    Public Const DTWAIN_PDFTEXT_NOWORDSPACING As UInteger = &H00400000UI
    Public Const DTWAIN_PDFTEXT_NOSTROKEWIDTH As UInteger = &H00800000UI
    Public Const DTWAIN_PDFTEXT_NORENDERMODE As UInteger = &H01000000UI
    Public Const DTWAIN_PDFTEXT_NORGBCOLOR As UInteger = &H02000000UI
    Public Const DTWAIN_PDFTEXT_NOFONTSIZE As UInteger = &H04000000UI
    Public Const DTWAIN_PDFTEXT_NOABSPOSITION As UInteger = &H08000000UI
    Public Const DTWAIN_PDFTEXT_IGNOREALL As UInteger = &HFFF00000UI
    Public Const DTWAIN_FONT_COURIER As Integer = 0
    Public Const DTWAIN_FONT_COURIERBOLD As Integer = 1
    Public Const DTWAIN_FONT_COURIERBOLDOBLIQUE As Integer = 2
    Public Const DTWAIN_FONT_COURIEROBLIQUE As Integer = 3
    Public Const DTWAIN_FONT_HELVETICA As Integer = 4
    Public Const DTWAIN_FONT_HELVETICABOLD As Integer = 5
    Public Const DTWAIN_FONT_HELVETICABOLDOBLIQUE As Integer = 6
    Public Const DTWAIN_FONT_HELVETICAOBLIQUE As Integer = 7
    Public Const DTWAIN_FONT_TIMESBOLD As Integer = 8
    Public Const DTWAIN_FONT_TIMESBOLDITALIC As Integer = 9
    Public Const DTWAIN_FONT_TIMESROMAN As Integer = 10
    Public Const DTWAIN_FONT_TIMESITALIC As Integer = 11
    Public Const DTWAIN_FONT_SYMBOL As Integer = 12
    Public Const DTWAIN_FONT_ZAPFDINGBATS As Integer = 13
    Public Const DTWAIN_PDFRENDER_FILL As Integer = 0
    Public Const DTWAIN_PDFRENDER_STROKE As Integer = 1
    Public Const DTWAIN_PDFRENDER_FILLSTROKE As Integer = 2
    Public Const DTWAIN_PDFRENDER_INVISIBLE As Integer = 3
    Public Const DTWAIN_PDFTEXTELEMENT_SCALINGXY As Integer = 0
    Public Const DTWAIN_PDFTEXTELEMENT_FONTHEIGHT As Integer = 1
    Public Const DTWAIN_PDFTEXTELEMENT_WORDSPACING As Integer = 2
    Public Const DTWAIN_PDFTEXTELEMENT_POSITION As Integer = 3
    Public Const DTWAIN_PDFTEXTELEMENT_COLOR As Integer = 4
    Public Const DTWAIN_PDFTEXTELEMENT_STROKEWIDTH As Integer = 5
    Public Const DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS As Integer = 6
    Public Const DTWAIN_PDFTEXTELEMENT_FONTNAME As Integer = 7
    Public Const DTWAIN_PDFTEXTELEMENT_TEXT As Integer = 8
    Public Const DTWAIN_PDFTEXTELEMENT_RENDERMODE As Integer = 9
    Public Const DTWAIN_PDFTEXTELEMENT_CHARSPACING As Integer = 10
    Public Const DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE As Integer = 11
    Public Const DTWAIN_PDFTEXTELEMENT_LEADING As Integer = 12
    Public Const DTWAIN_PDFTEXTELEMENT_SCALING As Integer = 13
    Public Const DTWAIN_PDFTEXTELEMENT_TEXTLENGTH As Integer = 14
    Public Const DTWAIN_PDFTEXTELEMENT_SKEWANGLES As Integer = 15
    Public Const DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER As Integer = 16
    Public Const DTWAIN_PDFTEXTTRANSFORM_TSRK As Integer = 0
    Public Const DTWAIN_PDFTEXTTRANSFORM_TSKR As Integer = 1
    Public Const DTWAIN_PDFTEXTTRANSFORM_TKSR As Integer = 2
    Public Const DTWAIN_PDFTEXTTRANSFORM_TKRS As Integer = 3
    Public Const DTWAIN_PDFTEXTTRANSFORM_TRSK As Integer = 4
    Public Const DTWAIN_PDFTEXTTRANSFORM_TRKS As Integer = 5
    Public Const DTWAIN_PDFTEXTTRANSFORM_STRK As Integer = 6
    Public Const DTWAIN_PDFTEXTTRANSFORM_STKR As Integer = 7
    Public Const DTWAIN_PDFTEXTTRANSFORM_SKTR As Integer = 8
    Public Const DTWAIN_PDFTEXTTRANSFORM_SKRT As Integer = 9
    Public Const DTWAIN_PDFTEXTTRANSFORM_SRTK As Integer = 10
    Public Const DTWAIN_PDFTEXTTRANSFORM_SRKT As Integer = 11
    Public Const DTWAIN_PDFTEXTTRANSFORM_RSTK As Integer = 12
    Public Const DTWAIN_PDFTEXTTRANSFORM_RSKT As Integer = 13
    Public Const DTWAIN_PDFTEXTTRANSFORM_RTSK As Integer = 14
    Public Const DTWAIN_PDFTEXTTRANSFORM_RTKT As Integer = 15
    Public Const DTWAIN_PDFTEXTTRANSFORM_RKST As Integer = 16
    Public Const DTWAIN_PDFTEXTTRANSFORM_RKTS As Integer = 17
    Public Const DTWAIN_PDFTEXTTRANSFORM_KSTR As Integer = 18
    Public Const DTWAIN_PDFTEXTTRANSFORM_KSRT As Integer = 19
    Public Const DTWAIN_PDFTEXTTRANSFORM_KRST As Integer = 20
    Public Const DTWAIN_PDFTEXTTRANSFORM_KRTS As Integer = 21
    Public Const DTWAIN_PDFTEXTTRANSFORM_KTSR As Integer = 22
    Public Const DTWAIN_PDFTEXTTRANSFORM_KTRS As Integer = 23
    Public Const DTWAIN_PDFTEXTTRANFORM_LAST As Integer = DTWAIN_PDFTEXTTRANSFORM_KTRS
    Public Const DTWAIN_TWDF_ULTRASONIC As Integer = 0
    Public Const DTWAIN_TWDF_BYLENGTH As Integer = 1
    Public Const DTWAIN_TWDF_INFRARED As Integer = 2
    Public Const DTWAIN_TWAS_NONE As Integer = 0
    Public Const DTWAIN_TWAS_AUTO As Integer = 1
    Public Const DTWAIN_TWAS_CURRENT As Integer = 2
    Public Const DTWAIN_TWFR_BOOK As Integer = 0
    Public Const DTWAIN_TWFR_FANFOLD As Integer = 1
    Public Const DTWAIN_CONSTANT_TWPT As Integer = 0
    Public Const DTWAIN_CONSTANT_TWUN As Integer = 1
    Public Const DTWAIN_CONSTANT_TWCY As Integer = 2
    Public Const DTWAIN_CONSTANT_TWAL As Integer = 3
    Public Const DTWAIN_CONSTANT_TWAS As Integer = 4
    Public Const DTWAIN_CONSTANT_TWBCOR As Integer = 5
    Public Const DTWAIN_CONSTANT_TWBD As Integer = 6
    Public Const DTWAIN_CONSTANT_TWBO As Integer = 7
    Public Const DTWAIN_CONSTANT_TWBP As Integer = 8
    Public Const DTWAIN_CONSTANT_TWBR As Integer = 9
    Public Const DTWAIN_CONSTANT_TWBT As Integer = 10
    Public Const DTWAIN_CONSTANT_TWCP As Integer = 11
    Public Const DTWAIN_CONSTANT_TWCS As Integer = 12
    Public Const DTWAIN_CONSTANT_TWDE As Integer = 13
    Public Const DTWAIN_CONSTANT_TWDR As Integer = 14
    Public Const DTWAIN_CONSTANT_TWDSK As Integer = 15
    Public Const DTWAIN_CONSTANT_TWDX As Integer = 16
    Public Const DTWAIN_CONSTANT_TWFA As Integer = 17
    Public Const DTWAIN_CONSTANT_TWFE As Integer = 18
    Public Const DTWAIN_CONSTANT_TWFF As Integer = 19
    Public Const DTWAIN_CONSTANT_TWFL As Integer = 20
    Public Const DTWAIN_CONSTANT_TWFO As Integer = 21
    Public Const DTWAIN_CONSTANT_TWFP As Integer = 22
    Public Const DTWAIN_CONSTANT_TWFR As Integer = 23
    Public Const DTWAIN_CONSTANT_TWFT As Integer = 24
    Public Const DTWAIN_CONSTANT_TWFY As Integer = 25
    Public Const DTWAIN_CONSTANT_TWIA As Integer = 26
    Public Const DTWAIN_CONSTANT_TWIC As Integer = 27
    Public Const DTWAIN_CONSTANT_TWIF As Integer = 28
    Public Const DTWAIN_CONSTANT_TWIM As Integer = 29
    Public Const DTWAIN_CONSTANT_TWJC As Integer = 30
    Public Const DTWAIN_CONSTANT_TWJQ As Integer = 31
    Public Const DTWAIN_CONSTANT_TWLP As Integer = 32
    Public Const DTWAIN_CONSTANT_TWLS As Integer = 33
    Public Const DTWAIN_CONSTANT_TWMD As Integer = 34
    Public Const DTWAIN_CONSTANT_TWNF As Integer = 35
    Public Const DTWAIN_CONSTANT_TWOR As Integer = 36
    Public Const DTWAIN_CONSTANT_TWOV As Integer = 37
    Public Const DTWAIN_CONSTANT_TWPA As Integer = 38
    Public Const DTWAIN_CONSTANT_TWPC As Integer = 39
    Public Const DTWAIN_CONSTANT_TWPCH As Integer = 40
    Public Const DTWAIN_CONSTANT_TWPF As Integer = 41
    Public Const DTWAIN_CONSTANT_TWPM As Integer = 42
    Public Const DTWAIN_CONSTANT_TWPR As Integer = 43
    Public Const DTWAIN_CONSTANT_TWPF2 As Integer = 44
    Public Const DTWAIN_CONSTANT_TWCT As Integer = 45
    Public Const DTWAIN_CONSTANT_TWPS As Integer = 46
    Public Const DTWAIN_CONSTANT_TWSS As Integer = 47
    Public Const DTWAIN_CONSTANT_TWPH As Integer = 48
    Public Const DTWAIN_CONSTANT_TWCI As Integer = 49
    Public Const DTWAIN_CONSTANT_FONTNAME As Integer = 50
    Public Const DTWAIN_CONSTANT_TWEI As Integer = 51
    Public Const DTWAIN_CONSTANT_TWEJ As Integer = 52
    Public Const DTWAIN_CONSTANT_TWCC As Integer = 53
    Public Const DTWAIN_CONSTANT_TWQC As Integer = 54
    Public Const DTWAIN_CONSTANT_TWRC As Integer = 55
    Public Const DTWAIN_CONSTANT_MSG As Integer = 56
    Public Const DTWAIN_CONSTANT_TWLG As Integer = 57
    Public Const DTWAIN_CONSTANT_DLLINFO As Integer = 58
    Public Const DTWAIN_CONSTANT_DG As Integer = 59
    Public Const DTWAIN_CONSTANT_DAT As Integer = 60
    Public Const DTWAIN_CONSTANT_DF As Integer = 61
    Public Const DTWAIN_CONSTANT_TWTY As Integer = 62
    Public Const DTWAIN_CONSTANT_TWCB As Integer = 63
    Public Const DTWAIN_CONSTANT_TWAF As Integer = 64
    Public Const DTWAIN_CONSTANT_TWFS As Integer = 65
    Public Const DTWAIN_CONSTANT_TWJS As Integer = 66
    Public Const DTWAIN_CONSTANT_TWMR As Integer = 67
    Public Const DTWAIN_CONSTANT_TWDP As Integer = 68
    Public Const DTWAIN_CONSTANT_TWUS As Integer = 69
    Public Const DTWAIN_CONSTANT_TWDF As Integer = 70
    Public Const DTWAIN_CONSTANT_TWFM As Integer = 71
    Public Const DTWAIN_CONSTANT_TWSG As Integer = 72
    Public Const DTWAIN_CONSTANT_DTWAIN_TN As Integer = 73
    Public Const DTWAIN_CONSTANT_TWON As Integer = 74
    Public Const DTWAIN_CONSTANT_TWMF As Integer = 75
    Public Const DTWAIN_CONSTANT_TWSX As Integer = 76
    Public Const DTWAIN_CONSTANT_CAP As Integer = 77
    Public Const DTWAIN_CONSTANT_ICAP As Integer = 78
    Public Const DTWAIN_CONSTANT_DTWAIN_CONT As Integer = 79
    Public Const DTWAIN_CONSTANT_CAPCODE_MAP As Integer = 80
    Public Const DTWAIN_USERRES_START As Integer = 20000
    Public Const DTWAIN_USERRES_MAXSIZE As Integer = 8192
    Public Const DTWAIN_APIHANDLEOK As Integer = 1
    Public Const DTWAIN_TWAINSESSIONOK As Integer = 2
    Public Const DTWAIN_PDF_AES128 As Integer = 1
    Public Const DTWAIN_PDF_AES256 As Integer = 2
    Public Const DTWAIN_FEEDER_TERMINATE As Integer = 1
    Public Const DTWAIN_FEEDER_USEFLATBED As Integer = 2
    Public Delegate Function DTwainCallback(WParam As Integer, LParam As Integer, UserData As Integer) As Integer
    Public Delegate Function DTwainCallback64(WParam As Integer, LParam As Integer, UserData As Long) As Integer
    Public Delegate Function DTwainErrorProc(param1 As Integer, param2 As Integer) As Integer
    Public Delegate Function DTwainErrorProc64(param1 As Integer, param2 As Long) As Integer
    Public Delegate Function DTwainLoggerProcA(<MarshalAs(UnmanagedType.LPStr)> lpszName As String, UserData As Long) As Integer
    Public Delegate Function DTwainLoggerProcW(<MarshalAs(UnmanagedType.LPWStr)> lpszName As String, UserData As Long) As Integer
    Public Delegate Function DTwainLoggerProc(<MarshalAs(UnmanagedType.LPTStr)> lpszName As String, UserData As Long) As Integer
    Public Delegate Function DTwainDIBUpdateProc(TheSource As System.IntPtr, currentImage As Integer, DibData As System.IntPtr) As System.IntPtr

    Declare Auto Function DTWAIN_AcquireAudioFile Lib "dtwain64d.dll" (Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lMaxClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Ansi Function DTWAIN_AcquireAudioFileA Lib "dtwain64d.dll" (Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lNumClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Unicode Function DTWAIN_AcquireAudioFileW Lib "dtwain64d.dll" (Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lNumClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireAudioNative Lib "dtwain64d.dll" (Source As System.IntPtr, nMaxAudioClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_AcquireAudioNativeEx Lib "dtwain64d.dll" (Source As System.IntPtr, nMaxAudioClips As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireBuffered Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_AcquireBufferedEx Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireFile Lib "dtwain64d.dll" (Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Ansi Function DTWAIN_AcquireFileA Lib "dtwain64d.dll" (Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireFileEx Lib "dtwain64d.dll" (Source As System.IntPtr, aFileNames As System.IntPtr, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Unicode Function DTWAIN_AcquireFileW Lib "dtwain64d.dll" (Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireNative Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_AcquireNativeEx Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireToClipboard Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, nTransferMode As Integer, bDiscardDibs As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_AddExtImageInfoQuery Lib "dtwain64d.dll" (Source As System.IntPtr, ExtImageInfo As Integer) As Integer
    Declare Auto Function DTWAIN_AddFileToAppend Lib "dtwain64d.dll" (szFile As String) As Integer
    Declare Ansi Function DTWAIN_AddFileToAppendA Lib "dtwain64d.dll" (szFile As String) As Integer
    Declare Unicode Function DTWAIN_AddFileToAppendW Lib "dtwain64d.dll" (szFile As String) As Integer
    Declare Auto Function DTWAIN_AddPDFText Lib "dtwain64d.dll" (Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
    Declare Ansi Function DTWAIN_AddPDFTextA Lib "dtwain64d.dll" (Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
    Declare Auto Function DTWAIN_AddPDFTextEx Lib "dtwain64d.dll" (Source As System.IntPtr, TextElement As System.IntPtr, Flags As UInteger) As Integer
    Declare Unicode Function DTWAIN_AddPDFTextW Lib "dtwain64d.dll" (Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
    Declare Auto Function DTWAIN_AllocateMemory Lib "dtwain64d.dll" (memSize As UInteger) As System.IntPtr
    Declare Auto Function DTWAIN_AllocateMemory64 Lib "dtwain64d.dll" (memSize As System.UInt64) As System.IntPtr
    Declare Auto Function DTWAIN_AllocateMemoryEx Lib "dtwain64d.dll" (memSize As UInteger) As System.IntPtr
    Declare Auto Function DTWAIN_AppHandlesExceptions Lib "dtwain64d.dll" (bSet As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayANSIStringToFloat Lib "dtwain64d.dll" (StringArray As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayAdd Lib "dtwain64d.dll" (pArray As System.IntPtr, pVariant As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_ArrayAddANSIString Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Ansi Function DTWAIN_ArrayAddANSIStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As System.Double) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloatN Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As System.Double, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Ansi Function DTWAIN_ArrayAddFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloatStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Ansi Function DTWAIN_ArrayAddFloatStringNA Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayAddFloatStringNW Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayAddFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddFrame Lib "dtwain64d.dll" (pArray As System.IntPtr, frame As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayAddFrameN Lib "dtwain64d.dll" (pArray As System.IntPtr, frame As System.IntPtr, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddLong Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddLong64 Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As System.Int64) As Integer
    Declare Auto Function DTWAIN_ArrayAddLong64N Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As System.Int64, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddLongN Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As Integer, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddN Lib "dtwain64d.dll" (pArray As System.IntPtr, pVariant As System.IntPtr, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddString Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Ansi Function DTWAIN_ArrayAddStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Ansi Function DTWAIN_ArrayAddStringNA Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayAddStringNW Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayAddStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Unicode Function DTWAIN_ArrayAddWideString Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String) As Integer
    Declare Unicode Function DTWAIN_ArrayAddWideStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayConvertFix32ToFloat Lib "dtwain64d.dll" (Fix32Array As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayConvertFloatToFix32 Lib "dtwain64d.dll" (FloatArray As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayCopy Lib "dtwain64d.dll" (Source As System.IntPtr, Dest As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayCreate Lib "dtwain64d.dll" (nEnumType As Integer, nInitialSize As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayCreateCopy Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayCreateFromCap Lib "dtwain64d.dll" (Source As System.IntPtr, lCapType As Integer, lSize As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayCreateFromLong64s Lib "dtwain64d.dll" (ByRef pCArray As System.Int64, nSize As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayCreateFromLongs Lib "dtwain64d.dll" (ByRef pCArray As Integer, nSize As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayCreateFromReals Lib "dtwain64d.dll" (ByRef pCArray As System.Double, nSize As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayDestroy Lib "dtwain64d.dll" (pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayDestroyFrames Lib "dtwain64d.dll" (FrameArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayFind Lib "dtwain64d.dll" (pArray As System.IntPtr, pVariant As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_ArrayFindANSIString Lib "dtwain64d.dll" (pArray As System.IntPtr, pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFindFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As System.Double, Tolerance As System.Double) As Integer
    Declare Auto Function DTWAIN_ArrayFindFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
    Declare Ansi Function DTWAIN_ArrayFindFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
    Declare Unicode Function DTWAIN_ArrayFindFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
    Declare Auto Function DTWAIN_ArrayFindLong Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayFindLong64 Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As System.Int64) As Integer
    Declare Auto Function DTWAIN_ArrayFindString Lib "dtwain64d.dll" (pArray As System.IntPtr, pString As String) As Integer
    Declare Ansi Function DTWAIN_ArrayFindStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, pString As String) As Integer
    Declare Unicode Function DTWAIN_ArrayFindStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, pString As String) As Integer
    Declare Unicode Function DTWAIN_ArrayFindWideString Lib "dtwain64d.dll" (pArray As System.IntPtr, pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFix32GetAt Lib "dtwain64d.dll" (aFix32 As System.IntPtr, lPos As Integer, ByRef Whole As Integer, ByRef Frac As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayFix32SetAt Lib "dtwain64d.dll" (aFix32 As System.IntPtr, lPos As Integer, Whole As Integer, Frac As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayFloatToANSIString Lib "dtwain64d.dll" (FloatArray As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayFloatToString Lib "dtwain64d.dll" (FloatArray As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayFloatToWideString Lib "dtwain64d.dll" (FloatArray As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayGetAt Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_ArrayGetAtANSIString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> pStr As StringBuilder) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, ByRef pVal As System.Double) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> Val As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_ArrayGetAtFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> Val As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_ArrayGetAtFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> Val As StringBuilder) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtFrame Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, ByRef pleft As System.Double, ByRef ptop As System.Double, ByRef pright As System.Double, ByRef pbottom As System.Double) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtFrameEx Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, Frame As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtFrameString Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> bottom As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_ArrayGetAtFrameStringA Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> bottom As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_ArrayGetAtFrameStringW Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> bottom As StringBuilder) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtLong Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtLong64 Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, ByRef pVal As System.Int64) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtSource Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, ByRef ppSource As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> pStr As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_ArrayGetAtStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> pStr As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_ArrayGetAtStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> pStr As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_ArrayGetAtWideString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> pStr As StringBuilder) As Integer
    Declare Auto Function DTWAIN_ArrayGetBuffer Lib "dtwain64d.dll" (pArray As System.IntPtr, nPos As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayGetCapValues Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lGetType As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayGetCapValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayGetCapValuesEx2 Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, nDataType As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayGetCount Lib "dtwain64d.dll" (pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayGetMaxStringLength Lib "dtwain64d.dll" (a As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayGetSourceAt Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, ByRef ppSource As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayGetStringLength Lib "dtwain64d.dll" (a As System.IntPtr, nWhichString As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetType Lib "dtwain64d.dll" (pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayInit Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_ArrayInsertAt Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtANSIString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtANSIStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As System.Double) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloatN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As System.Double, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloatStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtFloatStringNA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtFloatStringNW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFrame Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, frame As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFrameN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, frame As System.IntPtr, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLong Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLong64 Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As System.Int64) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLong64N Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As System.Int64, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLongN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As Integer, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtStringNA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtStringNW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtWideString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtWideStringN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAll Lib "dtwain64d.dll" (pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAt Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAtN Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayResize Lib "dtwain64d.dll" (pArray As System.IntPtr, NewSize As Integer) As Integer
    Declare Auto Function DTWAIN_ArraySetAt Lib "dtwain64d.dll" (pArray As System.IntPtr, lPos As Integer, pVariant As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_ArraySetAtANSIString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
    Declare Auto Function DTWAIN_ArraySetAtFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As System.Double) As Integer
    Declare Auto Function DTWAIN_ArraySetAtFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
    Declare Ansi Function DTWAIN_ArraySetAtFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
    Declare Unicode Function DTWAIN_ArraySetAtFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
    Declare Auto Function DTWAIN_ArraySetAtFrame Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, left As System.Double, top As System.Double, right As System.Double, bottom As System.Double) As Integer
    Declare Auto Function DTWAIN_ArraySetAtFrameEx Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, Frame As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ArraySetAtFrameString Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
    Declare Ansi Function DTWAIN_ArraySetAtFrameStringA Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
    Declare Unicode Function DTWAIN_ArraySetAtFrameStringW Lib "dtwain64d.dll" (FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
    Declare Auto Function DTWAIN_ArraySetAtLong Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pVal As Integer) As Integer
    Declare Auto Function DTWAIN_ArraySetAtLong64 Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, Val As System.Int64) As Integer
    Declare Auto Function DTWAIN_ArraySetAtString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
    Declare Ansi Function DTWAIN_ArraySetAtStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
    Declare Unicode Function DTWAIN_ArraySetAtStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
    Declare Unicode Function DTWAIN_ArraySetAtWideString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
    Declare Auto Function DTWAIN_ArrayStringToFloat Lib "dtwain64d.dll" (StringArray As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ArrayWideStringToFloat Lib "dtwain64d.dll" (StringArray As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_CallCallback Lib "dtwain64d.dll" (wParam As Integer, lParam As Integer, UserData As Integer) As Integer
    Declare Auto Function DTWAIN_CallCallback64 Lib "dtwain64d.dll" (wParam As Integer, lParam As Integer, UserData As System.Int64) As Integer
    Declare Auto Function DTWAIN_CallDSMProc Lib "dtwain64d.dll" (AppID As System.IntPtr, SourceId As System.IntPtr, lDG As Integer, lDAT As Integer, lMSG As Integer, pData As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_CheckHandles Lib "dtwain64d.dll" (bCheck As Integer) As Integer
    Declare Auto Function DTWAIN_ClearBuffers Lib "dtwain64d.dll" (Source As System.IntPtr, ClearBuffer As Integer) As Integer
    Declare Auto Function DTWAIN_ClearErrorBuffer Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_ClearPDFText Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ClearPage Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_CloseSource Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_CloseSourceUI Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ConvertDIBToBitmap Lib "dtwain64d.dll" (hDib As System.IntPtr, hPalette As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_ConvertDIBToFullBitmap Lib "dtwain64d.dll" (hDib As System.IntPtr, isBMP As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ConvertToAPIString Lib "dtwain64d.dll" (lpOrigString As String) As System.IntPtr
    Declare Ansi Function DTWAIN_ConvertToAPIStringA Lib "dtwain64d.dll" (lpOrigString As String) As System.IntPtr
    Declare Auto Function DTWAIN_ConvertToAPIStringEx Lib "dtwain64d.dll" (lpOrigString As String, <MarshalAs(UnmanagedType.LPTStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
    Declare Ansi Function DTWAIN_ConvertToAPIStringExA Lib "dtwain64d.dll" (lpOrigString As String, <MarshalAs(UnmanagedType.LPStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
    Declare Unicode Function DTWAIN_ConvertToAPIStringExW Lib "dtwain64d.dll" (lpOrigString As String, <MarshalAs(UnmanagedType.LPWStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
    Declare Unicode Function DTWAIN_ConvertToAPIStringW Lib "dtwain64d.dll" (lpOrigString As String) As System.IntPtr
    Declare Auto Function DTWAIN_CreateAcquisitionArray Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_CreatePDFTextElement Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_DeleteDIB Lib "dtwain64d.dll" (hDib As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_DestroyAcquisitionArray Lib "dtwain64d.dll" (aAcq As System.IntPtr, bDestroyData As Integer) As Integer
    Declare Auto Function DTWAIN_DestroyPDFTextElement Lib "dtwain64d.dll" (TextElement As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_DisableAppWindow Lib "dtwain64d.dll" (hWnd As System.IntPtr, bDisable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoBorderDetect Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoBright Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoDeskew Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoFeed Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoRotate Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoScan Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutomaticSenseMedium Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableDuplex Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableFeeder Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableIndicator Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableJobFileHandling Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableLamp Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableMsgNotify Lib "dtwain64d.dll" (bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnablePatchDetect Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnablePeekMessageLoop Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnablePrinter Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableThumbnail Lib "dtwain64d.dll" (Source As System.IntPtr, bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableTripletsNotify Lib "dtwain64d.dll" (bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EndThread Lib "dtwain64d.dll" (DLLHandle As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EndTwainSession Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_EnumAlarmVolumes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, expandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumAlarmVolumesEx Lib "dtwain64d.dll" (Source As System.IntPtr, expandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumAlarms Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumAlarmsEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumAudioXferMechs Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumAudioXferMechsEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumAutoFeedValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumAutoFeedValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumAutomaticCaptures Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumAutomaticCapturesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumAutomaticSenseMedium Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumAutomaticSenseMediumEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumBitDepths Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumBitDepthsEx Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumBitDepthsEx2 Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumBottomCameras Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumBottomCamerasEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumBrightnessValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumBrightnessValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumCameras Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumCamerasEx Lib "dtwain64d.dll" (Source As System.IntPtr, nWhichCamera As Integer, ByRef Cameras As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumCamerasEx2 Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumCamerasEx3 Lib "dtwain64d.dll" (Source As System.IntPtr, nWhichCamera As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumCompressionTypes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumCompressionTypesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumCompressionTypesEx2 Lib "dtwain64d.dll" (Source As System.IntPtr, lFileType As Integer, bUseBufferedMode As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumContrastValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumContrastValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumCustomCaps Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumCustomCapsEx2 Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumDoubleFeedDetectLengths Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumDoubleFeedDetectLengthsEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumDoubleFeedDetectValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumDoubleFeedDetectValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumExtImageInfoTypes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumExtImageInfoTypesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumExtendedCaps Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumExtendedCapsEx Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumExtendedCapsEx2 Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumFileTypeBitsPerPixel Lib "dtwain64d.dll" (FileType As Integer, ByRef Array As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumFileXferFormats Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumFileXferFormatsEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumHalftones Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumHalftonesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumHighlightValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumHighlightValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumJobControls Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumJobControlsEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumLightPaths Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef LightPath As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumLightPathsEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumLightSources Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef LightSources As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumLightSourcesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumMaxBuffers Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pMaxBufs As System.IntPtr, bExpandRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumMaxBuffersEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumNoiseFilters Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumNoiseFiltersEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumOCRInterfaces Lib "dtwain64d.dll" (ByRef OCRInterfaces As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumOCRSupportedCaps Lib "dtwain64d.dll" (Engine As System.IntPtr, ByRef SupportedCaps As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumOrientations Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumOrientationsEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumOverscanValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumOverscanValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPaperSizes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPaperSizesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPatchCodes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef PCodes As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPatchCodesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPatchMaxPriorities Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPatchMaxPrioritiesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPatchMaxRetries Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPatchMaxRetriesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPatchPriorities Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPatchPrioritiesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPatchSearchModes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPatchSearchModesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPatchTimeOutValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPatchTimeOutValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPixelTypes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPixelTypesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumPrinterStringModes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumPrinterStringModesEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumResolutionValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumResolutionValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumShadowValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumShadowValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumSourceUnits Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumSourceUnitsEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumSourceValues Lib "dtwain64d.dll" (Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Ansi Function DTWAIN_EnumSourceValuesA Lib "dtwain64d.dll" (Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Unicode Function DTWAIN_EnumSourceValuesW Lib "dtwain64d.dll" (Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumSources Lib "dtwain64d.dll" (ByRef lpArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumSourcesEx Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_EnumSupportedCaps Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumSupportedCapsEx Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumSupportedCapsEx2 Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumSupportedExtImageInfo Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumSupportedExtImageInfoEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumSupportedFileTypes Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_EnumSupportedMultiPageFileTypes Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_EnumSupportedSinglePageFileTypes Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_EnumThresholdValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumThresholdValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumTopCameras Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumTopCamerasEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumTwainPrinters Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpAvailPrinters As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumTwainPrintersArray Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_EnumTwainPrintersArrayEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumTwainPrintersEx Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_EnumXResolutionValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumXResolutionValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_EnumYResolutionValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumYResolutionValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_ExecuteOCR Lib "dtwain64d.dll" (Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
    Declare Ansi Function DTWAIN_ExecuteOCRA Lib "dtwain64d.dll" (Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
    Declare Unicode Function DTWAIN_ExecuteOCRW Lib "dtwain64d.dll" (Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
    Declare Auto Function DTWAIN_FeedPage Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_FlipBitmap Lib "dtwain64d.dll" (hDib As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_FlushAcquiredPages Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ForceAcquireBitDepth Lib "dtwain64d.dll" (Source As System.IntPtr, BitDepth As Integer) As Integer
    Declare Auto Function DTWAIN_ForceScanOnNoUI Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_FrameCreate Lib "dtwain64d.dll" (Left As System.Double, Top As System.Double, Right As System.Double, Bottom As System.Double) As System.IntPtr
    Declare Auto Function DTWAIN_FrameCreateString Lib "dtwain64d.dll" (Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
    Declare Ansi Function DTWAIN_FrameCreateStringA Lib "dtwain64d.dll" (Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
    Declare Unicode Function DTWAIN_FrameCreateStringW Lib "dtwain64d.dll" (Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
    Declare Auto Function DTWAIN_FrameDestroy Lib "dtwain64d.dll" (Frame As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_FrameGetAll Lib "dtwain64d.dll" (Frame As System.IntPtr, ByRef Left As System.Double, ByRef Top As System.Double, ByRef Right As System.Double, ByRef Bottom As System.Double) As Integer
    Declare Auto Function DTWAIN_FrameGetAllString Lib "dtwain64d.dll" (Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Bottom As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_FrameGetAllStringA Lib "dtwain64d.dll" (Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Bottom As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_FrameGetAllStringW Lib "dtwain64d.dll" (Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Bottom As StringBuilder) As Integer
    Declare Auto Function DTWAIN_FrameGetValue Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, ByRef Value As System.Double) As Integer
    Declare Auto Function DTWAIN_FrameGetValueString Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPTStr)> Value As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_FrameGetValueStringA Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPStr)> Value As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_FrameGetValueStringW Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPWStr)> Value As StringBuilder) As Integer
    Declare Auto Function DTWAIN_FrameIsValid Lib "dtwain64d.dll" (Frame As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_FrameSetAll Lib "dtwain64d.dll" (Frame As System.IntPtr, Left As System.Double, Top As System.Double, Right As System.Double, Bottom As System.Double) As Integer
    Declare Auto Function DTWAIN_FrameSetAllString Lib "dtwain64d.dll" (Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
    Declare Ansi Function DTWAIN_FrameSetAllStringA Lib "dtwain64d.dll" (Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
    Declare Unicode Function DTWAIN_FrameSetAllStringW Lib "dtwain64d.dll" (Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
    Declare Auto Function DTWAIN_FrameSetValue Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, Value As System.Double) As Integer
    Declare Auto Function DTWAIN_FrameSetValueString Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
    Declare Ansi Function DTWAIN_FrameSetValueStringA Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
    Declare Unicode Function DTWAIN_FrameSetValueStringW Lib "dtwain64d.dll" (Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
    Declare Auto Function DTWAIN_FreeExtImageInfo Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_FreeMemory Lib "dtwain64d.dll" (h As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_FreeMemoryEx Lib "dtwain64d.dll" (h As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetAPIHandleStatus Lib "dtwain64d.dll" (pHandle As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea Lib "dtwain64d.dll" (Source As System.IntPtr, lGetType As Integer, ByRef FloatEnum As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea2 Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef left As System.Double, ByRef top As System.Double, ByRef right As System.Double, ByRef bottom As System.Double, ByRef lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea2String Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
    Declare Ansi Function DTWAIN_GetAcquireArea2StringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
    Declare Unicode Function DTWAIN_GetAcquireArea2StringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireMetrics Lib "dtwain64d.dll" (source As System.IntPtr, ByRef ImageCount As Integer, ByRef SheetCount As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireStripBuffer Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_GetAcquireStripData Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpCompression As Integer, ByRef lpBytesPerRow As UInteger, ByRef lpColumns As UInteger, ByRef lpRows As UInteger, ByRef XOffset As UInteger, ByRef YOffset As UInteger, ByRef lpBytesWritten As UInteger) As Integer
    Declare Auto Function DTWAIN_GetAcquireStripSizes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpMin As UInteger, ByRef lpMax As UInteger, ByRef lpPreferred As UInteger) As Integer
    Declare Auto Function DTWAIN_GetAcquiredImage Lib "dtwain64d.dll" (aAcq As System.IntPtr, nWhichAcq As Integer, nWhichDib As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_GetAcquiredImageArray Lib "dtwain64d.dll" (aAcq As System.IntPtr, nWhichAcq As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_GetActiveDSMPath Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetActiveDSMPathA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetActiveDSMPathW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetActiveDSMVersionInfo Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> szDLLInfo As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetActiveDSMVersionInfoA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetActiveDSMVersionInfoW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetAlarmVolume Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpVolume As Integer) As Integer
    Declare Auto Function DTWAIN_GetAppInfo Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szProdName As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetAppInfoA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szProdName As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetAppInfoW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szProdName As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetAuthor Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szAuthor As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetAuthorA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szAuthor As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetAuthorW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szAuthor As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetBatteryMinutes Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpMinutes As Integer) As Integer
    Declare Auto Function DTWAIN_GetBatteryPercent Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpPercent As Integer) As Integer
    Declare Auto Function DTWAIN_GetBitDepth Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef BitDepth As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetBlankPageAutoDetection Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetBrightness Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Brightness As System.Double) As Integer
    Declare Auto Function DTWAIN_GetBrightnessString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Brightness As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetBrightnessStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Contrast As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetBrightnessStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Contrast As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetBufferedTransferInfo Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Compression As UInteger, ByRef BytesPerRow As UInteger, ByRef Columns As UInteger, ByRef Rows As UInteger, ByRef XOffset As UInteger, ByRef YOffset As UInteger, ByRef Flags As UInteger, ByRef BytesWritten As UInteger, ByRef MemoryLength As UInteger) As System.IntPtr
    Declare Auto Function DTWAIN_GetCallback Lib "dtwain64d.dll" () As DTwainCallback
    Declare Auto Function DTWAIN_GetCallback64 Lib "dtwain64d.dll" () As DTwainCallback64
    Declare Auto Function DTWAIN_GetCapArrayType Lib "dtwain64d.dll" (Source As System.IntPtr, nCap As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapContainer Lib "dtwain64d.dll" (Source As System.IntPtr, nCap As Integer, lCapType As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapContainerEx Lib "dtwain64d.dll" (nCap As Integer, bSetContainer As Integer, ByRef ConTypes As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetCapDataType Lib "dtwain64d.dll" (Source As System.IntPtr, nCap As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapFromName Lib "dtwain64d.dll" (szName As String) As Integer
    Declare Ansi Function DTWAIN_GetCapFromNameA Lib "dtwain64d.dll" (szName As String) As Integer
    Declare Unicode Function DTWAIN_GetCapFromNameW Lib "dtwain64d.dll" (szName As String) As Integer
    Declare Auto Function DTWAIN_GetCapOperations Lib "dtwain64d.dll" (Source As System.IntPtr, lCapability As Integer, ByRef lpOps As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapValues Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lGetType As Integer, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetCapValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetCapValuesEx2 Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, nDataType As Integer, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetCaption Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Caption As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetCaptionA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Caption As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetCaptionW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Caption As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetCompressionSize Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lBytes As Integer) As Integer
    Declare Auto Function DTWAIN_GetCompressionType Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpCompression As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetConditionCodeString Lib "dtwain64d.dll" (lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetConditionCodeStringA Lib "dtwain64d.dll" (lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetConditionCodeStringW Lib "dtwain64d.dll" (lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetContrast Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Contrast As System.Double) As Integer
    Declare Auto Function DTWAIN_GetContrastString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Contrast As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetContrastStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Contrast As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetContrastStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Contrast As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetCountry Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetCurrentAcquiredImage Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_GetCurrentFileName Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder, MaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetCurrentFileNameA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder, MaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetCurrentFileNameW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder, MaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetCurrentPageNum Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetCurrentRetryCount Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetCurrentTwainTriplet Lib "dtwain64d.dll" (ByRef pAppID As TW_IDENTITY, ByRef pSourceID As TW_IDENTITY, ByRef lpDG As Integer, ByRef lpDAT As Integer, ByRef lpMsg As Integer, ByRef lpMemRef As System.Int64) As Integer
    Declare Auto Function DTWAIN_GetCustomDSData Lib "dtwain64d.dll" (Source As System.IntPtr, Data As Byte(), dSize As UInteger, ByRef pActualSize As UInteger, nFlags As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_GetDSMFullName Lib "dtwain64d.dll" (DSMType As Integer, <MarshalAs(UnmanagedType.LPTStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
    Declare Ansi Function DTWAIN_GetDSMFullNameA Lib "dtwain64d.dll" (DSMType As Integer, <MarshalAs(UnmanagedType.LPStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
    Declare Unicode Function DTWAIN_GetDSMFullNameW Lib "dtwain64d.dll" (DSMType As Integer, <MarshalAs(UnmanagedType.LPWStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
    Declare Auto Function DTWAIN_GetDSMSearchOrder Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetDTWAINHandle Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_GetDeviceEvent Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpEvent As Integer) As Integer
    Declare Auto Function DTWAIN_GetDeviceEventEx Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpEvent As Integer, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetDeviceEventInfo Lib "dtwain64d.dll" (Source As System.IntPtr, nWhichInfo As Integer, pValue As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetDeviceNotifications Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef DevEvents As Integer) As Integer
    Declare Auto Function DTWAIN_GetDeviceTimeDate Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szTimeDate As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetDeviceTimeDateA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szTimeDate As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetDeviceTimeDateW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szTimeDate As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetDoubleFeedDetectLength Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Value As System.Double, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetDoubleFeedDetectValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetDuplexType Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpDupType As Integer) As Integer
    Declare Auto Function DTWAIN_GetErrorBuffer Lib "dtwain64d.dll" (ByRef ArrayBuffer As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetErrorBufferThreshold Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetErrorCallback Lib "dtwain64d.dll" () As DTwainErrorProc
    Declare Auto Function DTWAIN_GetErrorCallback64 Lib "dtwain64d.dll" () As DTwainErrorProc64
    Declare Auto Function DTWAIN_GetErrorString Lib "dtwain64d.dll" (lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetErrorStringA Lib "dtwain64d.dll" (lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetErrorStringW Lib "dtwain64d.dll" (lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetExtCapFromName Lib "dtwain64d.dll" (szName As String) As Integer
    Declare Ansi Function DTWAIN_GetExtCapFromNameA Lib "dtwain64d.dll" (szName As String) As Integer
    Declare Unicode Function DTWAIN_GetExtCapFromNameW Lib "dtwain64d.dll" (szName As String) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfo Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfoData Lib "dtwain64d.dll" (Source As System.IntPtr, nWhich As Integer, ByRef Data As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfoItem Lib "dtwain64d.dll" (Source As System.IntPtr, nWhich As Integer, ByRef InfoID As Integer, ByRef NumItems As Integer, ByRef Type As Integer) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfoItemEx Lib "dtwain64d.dll" (Source As System.IntPtr, nWhich As Integer, ByRef InfoID As Integer, ByRef NumItems As Integer, ByRef Type As Integer, ByRef ReturnCode As Integer) As Integer
    Declare Auto Function DTWAIN_GetExtNameFromCap Lib "dtwain64d.dll" (nValue As Integer, <MarshalAs(UnmanagedType.LPTStr)> szValue As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetExtNameFromCapA Lib "dtwain64d.dll" (nValue As Integer, <MarshalAs(UnmanagedType.LPStr)> szValue As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetExtNameFromCapW Lib "dtwain64d.dll" (nValue As Integer, <MarshalAs(UnmanagedType.LPWStr)> szValue As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetFeederAlignment Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpAlignment As Integer) As Integer
    Declare Auto Function DTWAIN_GetFeederFuncs Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetFeederOrder Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpOrder As Integer) As Integer
    Declare Auto Function DTWAIN_GetFeederWaitTime Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetFileCompressionType Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetFileTypeExtensions Lib "dtwain64d.dll" (nType As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszName As StringBuilder, nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetFileTypeExtensionsA Lib "dtwain64d.dll" (nType As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszName As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetFileTypeExtensionsW Lib "dtwain64d.dll" (nType As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszName As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetFileTypeName Lib "dtwain64d.dll" (nType As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszName As StringBuilder, nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetFileTypeNameA Lib "dtwain64d.dll" (nType As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszName As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetFileTypeNameW Lib "dtwain64d.dll" (nType As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszName As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetHalftone Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
    Declare Ansi Function DTWAIN_GetHalftoneA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
    Declare Unicode Function DTWAIN_GetHalftoneW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
    Declare Auto Function DTWAIN_GetHighlight Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Highlight As System.Double) As Integer
    Declare Auto Function DTWAIN_GetHighlightString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Highlight As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetHighlightStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Highlight As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetHighlightStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Highlight As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetImageInfo Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpXResolution As System.Double, ByRef lpYResolution As System.Double, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Auto Function DTWAIN_GetImageInfoString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Ansi Function DTWAIN_GetImageInfoStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Unicode Function DTWAIN_GetImageInfoStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Auto Function DTWAIN_GetJobControl Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pJobControl As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetJpegValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
    Declare Auto Function DTWAIN_GetJpegXRValues Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
    Declare Auto Function DTWAIN_GetLanguage Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetLastError Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetLibraryPath Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetLibraryPathA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetLibraryPathW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetLightPath Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpLightPath As Integer) As Integer
    Declare Auto Function DTWAIN_GetLightSource Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef LightSource As Integer) As Integer
    Declare Auto Function DTWAIN_GetLightSources Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef LightSources As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetLoggerCallback Lib "dtwain64d.dll" () As DTwainLoggerProc
    Declare Auto Function DTWAIN_GetLoggerCallbackA Lib "dtwain64d.dll" () As DTwainLoggerProcA
    Declare Auto Function DTWAIN_GetLoggerCallbackW Lib "dtwain64d.dll" () As DTwainLoggerProcW
    Declare Auto Function DTWAIN_GetManualDuplexCount Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pSide1 As Integer, ByRef pSide2 As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxAcquisitions Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetMaxBuffers Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pMaxBuf As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxPagesToAcquire Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetMaxRetryAttempts Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetNameFromCap Lib "dtwain64d.dll" (nCapValue As Integer, <MarshalAs(UnmanagedType.LPTStr)> szValue As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetNameFromCapA Lib "dtwain64d.dll" (nCapValue As Integer, <MarshalAs(UnmanagedType.LPStr)> szValue As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetNameFromCapW Lib "dtwain64d.dll" (nCapValue As Integer, <MarshalAs(UnmanagedType.LPWStr)> szValue As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetNoiseFilter Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpNoiseFilter As Integer) As Integer
    Declare Auto Function DTWAIN_GetNumAcquiredImages Lib "dtwain64d.dll" (aAcq As System.IntPtr, nWhich As Integer) As Integer
    Declare Auto Function DTWAIN_GetNumAcquisitions Lib "dtwain64d.dll" (aAcq As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetOCRCapValues Lib "dtwain64d.dll" (Engine As System.IntPtr, OCRCapValue As Integer, TypeOfGet As Integer, ByRef CapValues As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetOCRErrorString Lib "dtwain64d.dll" (Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRErrorStringA Lib "dtwain64d.dll" (Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRErrorStringW Lib "dtwain64d.dll" (Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRLastError Lib "dtwain64d.dll" (Engine As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetOCRMajorMinorVersion Lib "dtwain64d.dll" (Engine As System.IntPtr, ByRef lpMajor As Integer, ByRef lpMinor As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRManufacturer Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szManufacturer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRManufacturerA Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szManufacturer As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRManufacturerW Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szManufacturer As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRProductFamily Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProductFamily As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRProductFamilyA Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProductFamily As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRProductFamilyW Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProductFamily As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRProductName Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProductName As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRProductNameA Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProductName As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRProductNameW Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProductName As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRText Lib "dtwain64d.dll" (Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPTStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
    Declare Ansi Function DTWAIN_GetOCRTextA Lib "dtwain64d.dll" (Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_GetOCRTextInfoFloat Lib "dtwain64d.dll" (OCRTextInfo As System.IntPtr, nCharPos As Integer, nWhichItem As Integer, ByRef pInfo As System.Double) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoFloatEx Lib "dtwain64d.dll" (OCRTextInfo As System.IntPtr, nWhichItem As Integer, ByRef pInfo As System.Double, bufSize As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoHandle Lib "dtwain64d.dll" (Engine As System.IntPtr, nPageNo As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_GetOCRTextInfoLong Lib "dtwain64d.dll" (OCRTextInfo As System.IntPtr, nCharPos As Integer, nWhichItem As Integer, ByRef pInfo As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoLongEx Lib "dtwain64d.dll" (OCRTextInfo As System.IntPtr, nWhichItem As Integer, ByRef pInfo As Integer, bufSize As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRTextW Lib "dtwain64d.dll" (Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPWStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_GetOCRVersionInfo Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> buffer As StringBuilder, maxBufSize As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRVersionInfoA Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> buffer As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRVersionInfoW Lib "dtwain64d.dll" (Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> buffer As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetOrientation Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpOrient As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetOverscan Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpOverscan As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementFloat Lib "dtwain64d.dll" (TextElement As System.IntPtr, ByRef val1 As System.Double, ByRef val2 As System.Double, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementLong Lib "dtwain64d.dll" (TextElement As System.IntPtr, ByRef val1 As Integer, ByRef val2 As Integer, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementString Lib "dtwain64d.dll" (TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
    Declare Ansi Function DTWAIN_GetPDFTextElementStringA Lib "dtwain64d.dll" (TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
    Declare Unicode Function DTWAIN_GetPDFTextElementStringW Lib "dtwain64d.dll" (TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFType1FontName Lib "dtwain64d.dll" (FontVal As Integer, <MarshalAs(UnmanagedType.LPTStr)> szFont As StringBuilder, nChars As Integer) As Integer
    Declare Ansi Function DTWAIN_GetPDFType1FontNameA Lib "dtwain64d.dll" (FontVal As Integer, <MarshalAs(UnmanagedType.LPStr)> szFont As StringBuilder, nChars As Integer) As Integer
    Declare Unicode Function DTWAIN_GetPDFType1FontNameW Lib "dtwain64d.dll" (FontVal As Integer, <MarshalAs(UnmanagedType.LPWStr)> szFont As StringBuilder, nChars As Integer) As Integer
    Declare Auto Function DTWAIN_GetPaperSize Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpPaperSize As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPaperSizeName Lib "dtwain64d.dll" (paperNumber As Integer, <MarshalAs(UnmanagedType.LPTStr)> outName As StringBuilder, nSize As Integer) As Integer
    Declare Ansi Function DTWAIN_GetPaperSizeNameA Lib "dtwain64d.dll" (paperNumber As Integer, <MarshalAs(UnmanagedType.LPStr)> outName As StringBuilder, nSize As Integer) As Integer
    Declare Unicode Function DTWAIN_GetPaperSizeNameW Lib "dtwain64d.dll" (paperNumber As Integer, <MarshalAs(UnmanagedType.LPWStr)> outName As StringBuilder, nSize As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchMaxPriorities Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pMaxPriorities As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchMaxRetries Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pMaxRetries As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchPriorities Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef SearchPriorities As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetPatchSearchMode Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pSearchMode As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchTimeOut Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pTimeOut As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPixelFlavor Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpPixelFlavor As Integer) As Integer
    Declare Auto Function DTWAIN_GetPixelType Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef PixelType As Integer, ByRef BitDepth As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinter Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpPrinter As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStartNumber Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef nStart As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStringMode Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef PrinterMode As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStrings Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef ArrayString As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetPrinterSuffixString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Suffix As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetPrinterSuffixStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Suffix As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetPrinterSuffixStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Suffix As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetRegisteredMsg Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetResolution Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
    Declare Auto Function DTWAIN_GetResolutionString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetResolutionStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetResolutionStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetResourceString Lib "dtwain64d.dll" (ResourceID As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetResourceStringA Lib "dtwain64d.dll" (ResourceID As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetResourceStringW Lib "dtwain64d.dll" (ResourceID As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetRotation Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Rotation As System.Double) As Integer
    Declare Auto Function DTWAIN_GetRotationString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Rotation As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetRotationStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Rotation As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetRotationStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Rotation As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetSaveFileName Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSaveFileNameA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSaveFileNameW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSavedFilesCount Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_GetSessionDetails Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSessionDetailsA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSessionDetailsW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
    Declare Auto Function DTWAIN_GetShadow Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Shadow As System.Double) As Integer
    Declare Auto Function DTWAIN_GetShadowString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Shadow As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetShadowStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Shadow As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetShadowStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Shadow As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetShortVersionString Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetShortVersionStringA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetShortVersionStringW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceAcquisitions Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_GetSourceDetails Lib "dtwain64d.dll" (szSources As String, <MarshalAs(UnmanagedType.LPTStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceDetailsA Lib "dtwain64d.dll" (szSources As String, <MarshalAs(UnmanagedType.LPStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceDetailsW Lib "dtwain64d.dll" (szSources As String, <MarshalAs(UnmanagedType.LPWStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceID Lib "dtwain64d.dll" (Source As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_GetSourceIDEx Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pIdentity As TW_IDENTITY) As TW_IDENTITY
    Declare Auto Function DTWAIN_GetSourceManufacturer Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceManufacturerA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceManufacturerW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductFamily Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductFamilyA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductFamilyW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductName Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductNameA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductNameW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceUnit Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionInfo Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceVersionInfoA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceVersionInfoW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionNumber Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef pMajor As Integer, ByRef pMinor As Integer) As Integer
    Declare Auto Function DTWAIN_GetStaticLibVersion Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetTempFileDirectory Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> szFilePath As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTempFileDirectoryA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> szFilePath As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTempFileDirectoryW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> szFilePath As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetThreshold Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Threshold As System.Double) As Integer
    Declare Auto Function DTWAIN_GetThresholdString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Threshold As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetThresholdStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Threshold As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetThresholdStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Threshold As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetTimeDate Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szTimeDate As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetTimeDateA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szTimeDate As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetTimeDateW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szTimeDate As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetTwainAppID Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_GetTwainAppIDEx Lib "dtwain64d.dll" (ByRef pIdentity As TW_IDENTITY) As TW_IDENTITY
    Declare Auto Function DTWAIN_GetTwainAvailability Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetTwainAvailabilityEx Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTwainAvailabilityExA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> szDirectories As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTwainAvailabilityExW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> szDirectories As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetTwainCountryName Lib "dtwain64d.dll" (countryId As Integer, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetTwainCountryNameA Lib "dtwain64d.dll" (countryId As Integer, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetTwainCountryNameW Lib "dtwain64d.dll" (countryId As Integer, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetTwainCountryValue Lib "dtwain64d.dll" (country As String) As Integer
    Declare Ansi Function DTWAIN_GetTwainCountryValueA Lib "dtwain64d.dll" (country As String) As Integer
    Declare Unicode Function DTWAIN_GetTwainCountryValueW Lib "dtwain64d.dll" (country As String) As Integer
    Declare Auto Function DTWAIN_GetTwainHwnd Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_GetTwainIDFromName Lib "dtwain64d.dll" (lpszBuffer As String) As Integer
    Declare Ansi Function DTWAIN_GetTwainIDFromNameA Lib "dtwain64d.dll" (lpszBuffer As String) As Integer
    Declare Unicode Function DTWAIN_GetTwainIDFromNameW Lib "dtwain64d.dll" (lpszBuffer As String) As Integer
    Declare Auto Function DTWAIN_GetTwainLanguageName Lib "dtwain64d.dll" (nameId As Integer, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetTwainLanguageNameA Lib "dtwain64d.dll" (lang As Integer, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetTwainLanguageNameW Lib "dtwain64d.dll" (lang As Integer, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetTwainLanguageValue Lib "dtwain64d.dll" (szName As String) As Integer
    Declare Ansi Function DTWAIN_GetTwainLanguageValueA Lib "dtwain64d.dll" (lang As String) As Integer
    Declare Unicode Function DTWAIN_GetTwainLanguageValueW Lib "dtwain64d.dll" (lang As String) As Integer
    Declare Auto Function DTWAIN_GetTwainMode Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetTwainNameFromConstant Lib "dtwain64d.dll" (lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTwainNameFromConstantA Lib "dtwain64d.dll" (lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTwainNameFromConstantW Lib "dtwain64d.dll" (lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
    Declare Auto Function DTWAIN_GetTwainStringName Lib "dtwain64d.dll" (category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTwainStringNameA Lib "dtwain64d.dll" (category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTwainStringNameW Lib "dtwain64d.dll" (category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetTwainTimeout Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_GetVersion Lib "dtwain64d.dll" (ByRef lpMajor As Integer, ByRef lpMinor As Integer, ByRef lpVersionType As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionCopyright Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionCopyrightA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionCopyrightW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionEx Lib "dtwain64d.dll" (ByRef lMajor As Integer, ByRef lMinor As Integer, ByRef lVersionType As Integer, ByRef lPatchLevel As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionInfo Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionInfoA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionInfoW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionString Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionStringA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionStringW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetWindowsVersionInfo Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetWindowsVersionInfoA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetWindowsVersionInfoW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetXResolution Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
    Declare Auto Function DTWAIN_GetXResolutionString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetXResolutionStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetXResolutionStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
    Declare Auto Function DTWAIN_GetYResolution Lib "dtwain64d.dll" (Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
    Declare Auto Function DTWAIN_GetYResolutionString Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_GetYResolutionStringA Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_GetYResolutionStringW Lib "dtwain64d.dll" (Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
    Declare Auto Function DTWAIN_InitExtImageInfo Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_InitImageFileAppend Lib "dtwain64d.dll" (szFile As String, fType As Integer) As Integer
    Declare Ansi Function DTWAIN_InitImageFileAppendA Lib "dtwain64d.dll" (szFile As String, fType As Integer) As Integer
    Declare Unicode Function DTWAIN_InitImageFileAppendW Lib "dtwain64d.dll" (szFile As String, fType As Integer) As Integer
    Declare Auto Function DTWAIN_InitOCRInterface Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsAcquiring Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsAudioXferSupported Lib "dtwain64d.dll" (Source As System.IntPtr, supportVal As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoBorderDetectEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoBorderDetectSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoBrightEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoBrightSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoDeskewEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoDeskewSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoFeedEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoFeedSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoRotateEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoRotateSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutoScanEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutomaticSenseMediumEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsAutomaticSenseMediumSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsBlankPageDetectionOn Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsBufferedTileModeOn Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsBufferedTileModeSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsCapSupported Lib "dtwain64d.dll" (Source As System.IntPtr, lCapability As Integer) As Integer
    Declare Auto Function DTWAIN_IsCompressionSupported Lib "dtwain64d.dll" (Source As System.IntPtr, Compression As Integer) As Integer
    Declare Auto Function DTWAIN_IsCustomDSDataSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsDIBBlank Lib "dtwain64d.dll" (hDib As System.IntPtr, threshold As System.Double) As Integer
    Declare Auto Function DTWAIN_IsDIBBlankString Lib "dtwain64d.dll" (hDib As System.IntPtr, threshold As String) As Integer
    Declare Ansi Function DTWAIN_IsDIBBlankStringA Lib "dtwain64d.dll" (hDib As System.IntPtr, threshold As String) As Integer
    Declare Unicode Function DTWAIN_IsDIBBlankStringW Lib "dtwain64d.dll" (hDib As System.IntPtr, threshold As String) As Integer
    Declare Auto Function DTWAIN_IsDeviceEventSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsDeviceOnLine Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsDoubleFeedDetectLengthSupported Lib "dtwain64d.dll" (Source As System.IntPtr, value As System.Double) As Integer
    Declare Auto Function DTWAIN_IsDoubleFeedDetectSupported Lib "dtwain64d.dll" (Source As System.IntPtr, SupportVal As Integer) As Integer
    Declare Auto Function DTWAIN_IsDoublePageCountOnDuplex Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsDuplexEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsDuplexSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsExtImageInfoSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsFeederEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsFeederLoaded Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsFeederSensitive Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsFeederSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsFileSystemSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsFileXferSupported Lib "dtwain64d.dll" (Source As System.IntPtr, lFileType As Integer) As Integer
    Declare Auto Function DTWAIN_IsIAFieldALastPageSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldALevelSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldAPrintFormatSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldAValueSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldBLastPageSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldBLevelSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldBPrintFormatSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldBValueSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldCLastPageSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldCLevelSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldCPrintFormatSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldCValueSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldDLastPageSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldDLevelSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldDPrintFormatSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldDValueSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldELastPageSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldELevelSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldEPrintFormatSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIAFieldEValueSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsImageAddressingSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIndicatorEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsIndicatorSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsInitialized Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsJPEGSupported Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsJobControlSupported Lib "dtwain64d.dll" (Source As System.IntPtr, JobControl As Integer) As Integer
    Declare Auto Function DTWAIN_IsLampEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsLampSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsLightPathSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsLightSourceSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsMaxBuffersSupported Lib "dtwain64d.dll" (Source As System.IntPtr, MaxBuf As Integer) As Integer
    Declare Auto Function DTWAIN_IsMemFileXferSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsMsgNotifyEnabled Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsNotifyTripletsEnabled Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsOCREngineActivated Lib "dtwain64d.dll" (OCREngine As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsOpenSourcesOnSelect Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsOrientationSupported Lib "dtwain64d.dll" (Source As System.IntPtr, Orientation As Integer) As Integer
    Declare Auto Function DTWAIN_IsOverscanSupported Lib "dtwain64d.dll" (Source As System.IntPtr, SupportValue As Integer) As Integer
    Declare Auto Function DTWAIN_IsPDFSupported Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsPNGSupported Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsPaperDetectable Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsPaperSizeSupported Lib "dtwain64d.dll" (Source As System.IntPtr, PaperSize As Integer) As Integer
    Declare Auto Function DTWAIN_IsPatchCapsSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsPatchDetectEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsPatchSupported Lib "dtwain64d.dll" (Source As System.IntPtr, PatchCode As Integer) As Integer
    Declare Auto Function DTWAIN_IsPeekMessageLoopEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsPixelTypeSupported Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer) As Integer
    Declare Auto Function DTWAIN_IsPrinterEnabled Lib "dtwain64d.dll" (Source As System.IntPtr, Printer As Integer) As Integer
    Declare Auto Function DTWAIN_IsPrinterSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsRotationSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsSessionEnabled Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsSkipImageInfoError Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsSourceAcquiring Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsSourceAcquiringEx Lib "dtwain64d.dll" (Source As System.IntPtr, bUIOnly As Integer) As Integer
    Declare Auto Function DTWAIN_IsSourceInUIOnlyMode Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsSourceOpen Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsSourceSelected Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsSourceValid Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsTIFFSupported Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsThumbnailEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsThumbnailSupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsTwainAvailable Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_IsTwainAvailableEx Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPTStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_IsTwainAvailableExA Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_IsTwainAvailableExW Lib "dtwain64d.dll" (<MarshalAs(UnmanagedType.LPWStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_IsTwainMsg Lib "dtwain64d.dll" (ByRef pMsg As WinMsg) As Integer
    Declare Auto Function DTWAIN_IsUIControllable Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsUIEnabled Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_IsUIOnlySupported Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_LoadCustomStringResources Lib "dtwain64d.dll" (sLangDLL As String) As Integer
    Declare Ansi Function DTWAIN_LoadCustomStringResourcesA Lib "dtwain64d.dll" (sLangDLL As String) As Integer
    Declare Auto Function DTWAIN_LoadCustomStringResourcesEx Lib "dtwain64d.dll" (sLangDLL As String, bClear As Integer) As Integer
    Declare Ansi Function DTWAIN_LoadCustomStringResourcesExA Lib "dtwain64d.dll" (sLangDLL As String, bClear As Integer) As Integer
    Declare Unicode Function DTWAIN_LoadCustomStringResourcesExW Lib "dtwain64d.dll" (sLangDLL As String, bClear As Integer) As Integer
    Declare Unicode Function DTWAIN_LoadCustomStringResourcesW Lib "dtwain64d.dll" (sLangDLL As String) As Integer
    Declare Auto Function DTWAIN_LoadLanguageResource Lib "dtwain64d.dll" (nLanguage As Integer) As Integer
    Declare Auto Function DTWAIN_LockMemory Lib "dtwain64d.dll" (h As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_LockMemoryEx Lib "dtwain64d.dll" (h As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_LogMessage Lib "dtwain64d.dll" (message As String) As Integer
    Declare Ansi Function DTWAIN_LogMessageA Lib "dtwain64d.dll" (message As String) As Integer
    Declare Unicode Function DTWAIN_LogMessageW Lib "dtwain64d.dll" (message As String) As Integer
    Declare Auto Function DTWAIN_MakeRGB Lib "dtwain64d.dll" (red As Integer, green As Integer, blue As Integer) As Integer
    Declare Auto Function DTWAIN_OpenSource Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_OpenSourcesOnSelect Lib "dtwain64d.dll" (bSet As Integer) As Integer
    Declare Auto Function DTWAIN_RangeCreate Lib "dtwain64d.dll" (nEnumType As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_RangeCreateFromCap Lib "dtwain64d.dll" (Source As System.IntPtr, lCapType As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_RangeDestroy Lib "dtwain64d.dll" (pSource As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeExpand Lib "dtwain64d.dll" (pSource As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeExpandEx Lib "dtwain64d.dll" (Range As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_RangeGetAll Lib "dtwain64d.dll" (pArray As System.IntPtr, pVariantLow As System.IntPtr, pVariantUp As System.IntPtr, pVariantStep As System.IntPtr, pVariantDefault As System.IntPtr, pVariantCurrent As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeGetAllFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, ByRef pVariantLow As System.Double, ByRef pVariantUp As System.Double, ByRef pVariantStep As System.Double, ByRef pVariantDefault As System.Double, ByRef pVariantCurrent As System.Double) As Integer
    Declare Auto Function DTWAIN_RangeGetAllFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dCurrent As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_RangeGetAllFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dCurrent As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_RangeGetAllFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dCurrent As StringBuilder) As Integer
    Declare Auto Function DTWAIN_RangeGetAllLong Lib "dtwain64d.dll" (pArray As System.IntPtr, ByRef pVariantLow As Integer, ByRef pVariantUp As Integer, ByRef pVariantStep As Integer, ByRef pVariantDefault As Integer, ByRef pVariantCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetCount Lib "dtwain64d.dll" (pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValue Lib "dtwain64d.dll" (pArray As System.IntPtr, lPos As Integer, pVariant As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, lPos As Integer, ByRef pVal As System.Double) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPTStr)> pVal As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_RangeGetExpValueFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPStr)> pVal As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_RangeGetExpValueFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPWStr)> pVal As StringBuilder) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueLong Lib "dtwain64d.dll" (pArray As System.IntPtr, lPos As Integer, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetNearestValue Lib "dtwain64d.dll" (pArray As System.IntPtr, pVariantIn As System.IntPtr, pVariantOut As System.IntPtr, RoundType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPos Lib "dtwain64d.dll" (pArray As System.IntPtr, pVariant As System.IntPtr, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As System.Double, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
    Declare Ansi Function DTWAIN_RangeGetPosFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
    Declare Unicode Function DTWAIN_RangeGetPosFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosLong Lib "dtwain64d.dll" (pArray As System.IntPtr, Value As Integer, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetValue Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, pVariant As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeGetValueFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, ByRef pVal As System.Double) As Integer
    Declare Auto Function DTWAIN_RangeGetValueFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPTStr)> pVal As StringBuilder) As Integer
    Declare Ansi Function DTWAIN_RangeGetValueFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPStr)> dValue As StringBuilder) As Integer
    Declare Unicode Function DTWAIN_RangeGetValueFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPWStr)> dValue As StringBuilder) As Integer
    Declare Auto Function DTWAIN_RangeGetValueLong Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_RangeIsValid Lib "dtwain64d.dll" (Range As System.IntPtr, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, dIn As System.Double, ByRef pOut As System.Double, RoundType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPTStr)> pOut As StringBuilder, RoundType As Integer) As Integer
    Declare Ansi Function DTWAIN_RangeNearestValueFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPStr)> dOut As StringBuilder, RoundType As Integer) As Integer
    Declare Unicode Function DTWAIN_RangeNearestValueFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPWStr)> dOut As StringBuilder, RoundType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueLong Lib "dtwain64d.dll" (pArray As System.IntPtr, lIn As Integer, ByRef pOut As Integer, RoundType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeSetAll Lib "dtwain64d.dll" (pArray As System.IntPtr, pVariantLow As System.IntPtr, pVariantUp As System.IntPtr, pVariantStep As System.IntPtr, pVariantDefault As System.IntPtr, pVariantCurrent As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeSetAllFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, dLow As System.Double, dUp As System.Double, dStep As System.Double, dDefault As System.Double, dCurrent As System.Double) As Integer
    Declare Auto Function DTWAIN_RangeSetAllFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
    Declare Ansi Function DTWAIN_RangeSetAllFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
    Declare Unicode Function DTWAIN_RangeSetAllFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
    Declare Auto Function DTWAIN_RangeSetAllLong Lib "dtwain64d.dll" (pArray As System.IntPtr, lLow As Integer, lUp As Integer, lStep As Integer, lDefault As Integer, lCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_RangeSetValue Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, pVal As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RangeSetValueFloat Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, Val As System.Double) As Integer
    Declare Auto Function DTWAIN_RangeSetValueFloatString Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, Val As String) As Integer
    Declare Ansi Function DTWAIN_RangeSetValueFloatStringA Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, dValue As String) As Integer
    Declare Unicode Function DTWAIN_RangeSetValueFloatStringW Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, dValue As String) As Integer
    Declare Auto Function DTWAIN_RangeSetValueLong Lib "dtwain64d.dll" (pArray As System.IntPtr, nWhich As Integer, Val As Integer) As Integer
    Declare Auto Function DTWAIN_ResetPDFTextElement Lib "dtwain64d.dll" (TextElement As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_RewindPage Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SelectDefaultOCREngine Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_SelectDefaultSource Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_SelectDefaultSourceWithOpen Lib "dtwain64d.dll" (bOpen As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_SelectOCREngine Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_SelectOCREngine2 Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
    Declare Ansi Function DTWAIN_SelectOCREngine2A Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_SelectOCREngine2Ex Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeFilter As String, szExcludeFilter As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
    Declare Ansi Function DTWAIN_SelectOCREngine2ExA Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
    Declare Unicode Function DTWAIN_SelectOCREngine2ExW Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
    Declare Unicode Function DTWAIN_SelectOCREngine2W Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_SelectOCREngineByName Lib "dtwain64d.dll" (lpszName As String) As System.IntPtr
    Declare Ansi Function DTWAIN_SelectOCREngineByNameA Lib "dtwain64d.dll" (lpszName As String) As System.IntPtr
    Declare Unicode Function DTWAIN_SelectOCREngineByNameW Lib "dtwain64d.dll" (lpszName As String) As System.IntPtr
    Declare Auto Function DTWAIN_SelectSource Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_SelectSource2 Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
    Declare Ansi Function DTWAIN_SelectSource2A Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_SelectSource2Ex Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeFilter As String, szExcludeFilter As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
    Declare Ansi Function DTWAIN_SelectSource2ExA Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
    Declare Unicode Function DTWAIN_SelectSource2ExW Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
    Declare Unicode Function DTWAIN_SelectSource2W Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_SelectSourceByName Lib "dtwain64d.dll" (lpszName As String) As System.IntPtr
    Declare Ansi Function DTWAIN_SelectSourceByNameA Lib "dtwain64d.dll" (lpszName As String) As System.IntPtr
    Declare Unicode Function DTWAIN_SelectSourceByNameW Lib "dtwain64d.dll" (lpszName As String) As System.IntPtr
    Declare Auto Function DTWAIN_SelectSourceByNameWithOpen Lib "dtwain64d.dll" (lpszName As String, bOpen As Integer) As System.IntPtr
    Declare Ansi Function DTWAIN_SelectSourceByNameWithOpenA Lib "dtwain64d.dll" (lpszName As String, bOpen As Integer) As System.IntPtr
    Declare Unicode Function DTWAIN_SelectSourceByNameWithOpenW Lib "dtwain64d.dll" (lpszName As String, bOpen As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_SelectSourceWithOpen Lib "dtwain64d.dll" (bOpen As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_SetAcquireArea Lib "dtwain64d.dll" (Source As System.IntPtr, lSetType As Integer, FloatEnum As System.IntPtr, ActualEnum As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetAcquireArea2 Lib "dtwain64d.dll" (Source As System.IntPtr, left As System.Double, top As System.Double, right As System.Double, bottom As System.Double, lUnit As Integer, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireArea2String Lib "dtwain64d.dll" (Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
    Declare Ansi Function DTWAIN_SetAcquireArea2StringA Lib "dtwain64d.dll" (Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
    Declare Unicode Function DTWAIN_SetAcquireArea2StringW Lib "dtwain64d.dll" (Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageNegative Lib "dtwain64d.dll" (Source As System.IntPtr, IsNegative As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageScale Lib "dtwain64d.dll" (Source As System.IntPtr, xscale As System.Double, yscale As System.Double) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageScaleString Lib "dtwain64d.dll" (Source As System.IntPtr, xscale As String, yscale As String) As Integer
    Declare Ansi Function DTWAIN_SetAcquireImageScaleStringA Lib "dtwain64d.dll" (Source As System.IntPtr, xscale As String, yscale As String) As Integer
    Declare Unicode Function DTWAIN_SetAcquireImageScaleStringW Lib "dtwain64d.dll" (Source As System.IntPtr, xscale As String, yscale As String) As Integer
    Declare Auto Function DTWAIN_SetAcquireStripBuffer Lib "dtwain64d.dll" (Source As System.IntPtr, hMem As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetAcquireStripSize Lib "dtwain64d.dll" (Source As System.IntPtr, StripSize As UInteger) As Integer
    Declare Auto Function DTWAIN_SetAlarmVolume Lib "dtwain64d.dll" (Source As System.IntPtr, Volume As Integer) As Integer
    Declare Auto Function DTWAIN_SetAlarms Lib "dtwain64d.dll" (Source As System.IntPtr, Alarms As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetAllCapsToDefault Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetAppInfo Lib "dtwain64d.dll" (szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
    Declare Ansi Function DTWAIN_SetAppInfoA Lib "dtwain64d.dll" (szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
    Declare Unicode Function DTWAIN_SetAppInfoW Lib "dtwain64d.dll" (szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
    Declare Auto Function DTWAIN_SetAuthor Lib "dtwain64d.dll" (Source As System.IntPtr, szAuthor As String) As Integer
    Declare Ansi Function DTWAIN_SetAuthorA Lib "dtwain64d.dll" (Source As System.IntPtr, szAuthor As String) As Integer
    Declare Unicode Function DTWAIN_SetAuthorW Lib "dtwain64d.dll" (Source As System.IntPtr, szAuthor As String) As Integer
    Declare Auto Function DTWAIN_SetAvailablePrinters Lib "dtwain64d.dll" (Source As System.IntPtr, lpAvailPrinters As Integer) As Integer
    Declare Auto Function DTWAIN_SetAvailablePrintersArray Lib "dtwain64d.dll" (Source As System.IntPtr, AvailPrinters As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetBitDepth Lib "dtwain64d.dll" (Source As System.IntPtr, BitDepth As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetection Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As System.Double, discard_option As Integer, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetectionEx Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As System.Double, autodetect As Integer, detectOpts As Integer, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetectionExString Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
    Declare Ansi Function DTWAIN_SetBlankPageDetectionExStringA Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
    Declare Unicode Function DTWAIN_SetBlankPageDetectionExStringW Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetectionString Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
    Declare Ansi Function DTWAIN_SetBlankPageDetectionStringA Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
    Declare Unicode Function DTWAIN_SetBlankPageDetectionStringW Lib "dtwain64d.dll" (Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBrightness Lib "dtwain64d.dll" (Source As System.IntPtr, Brightness As System.Double) As Integer
    Declare Auto Function DTWAIN_SetBrightnessString Lib "dtwain64d.dll" (Source As System.IntPtr, Brightness As String) As Integer
    Declare Ansi Function DTWAIN_SetBrightnessStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Contrast As String) As Integer
    Declare Unicode Function DTWAIN_SetBrightnessStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Contrast As String) As Integer
    Declare Auto Function DTWAIN_SetBufferedTileMode Lib "dtwain64d.dll" (Source As System.IntPtr, bTileMode As Integer) As Integer
    Declare Auto Function DTWAIN_SetCallback Lib "dtwain64d.dll" (Fn As DTwainCallback, UserData As Integer) As DTwainCallback
    Declare Auto Function DTWAIN_SetCallback64 Lib "dtwain64d.dll" (Fn As DTwainCallback64, UserData As System.Int64) As DTwainCallback64
    Declare Auto Function DTWAIN_SetCamera Lib "dtwain64d.dll" (Source As System.IntPtr, szCamera As String) As Integer
    Declare Ansi Function DTWAIN_SetCameraA Lib "dtwain64d.dll" (Source As System.IntPtr, szCamera As String) As Integer
    Declare Unicode Function DTWAIN_SetCameraW Lib "dtwain64d.dll" (Source As System.IntPtr, szCamera As String) As Integer
    Declare Auto Function DTWAIN_SetCapValues Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lSetType As Integer, pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetCapValuesEx Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lSetType As Integer, lContainerType As Integer, pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetCapValuesEx2 Lib "dtwain64d.dll" (Source As System.IntPtr, lCap As Integer, lSetType As Integer, lContainerType As Integer, nDataType As Integer, pArray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetCaption Lib "dtwain64d.dll" (Source As System.IntPtr, Caption As String) As Integer
    Declare Ansi Function DTWAIN_SetCaptionA Lib "dtwain64d.dll" (Source As System.IntPtr, Caption As String) As Integer
    Declare Unicode Function DTWAIN_SetCaptionW Lib "dtwain64d.dll" (Source As System.IntPtr, Caption As String) As Integer
    Declare Auto Function DTWAIN_SetCompressionType Lib "dtwain64d.dll" (Source As System.IntPtr, lCompression As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetContrast Lib "dtwain64d.dll" (Source As System.IntPtr, Contrast As System.Double) As Integer
    Declare Auto Function DTWAIN_SetContrastString Lib "dtwain64d.dll" (Source As System.IntPtr, Contrast As String) As Integer
    Declare Ansi Function DTWAIN_SetContrastStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Contrast As String) As Integer
    Declare Unicode Function DTWAIN_SetContrastStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Contrast As String) As Integer
    Declare Auto Function DTWAIN_SetCountry Lib "dtwain64d.dll" (nCountry As Integer) As Integer
    Declare Auto Function DTWAIN_SetCurrentRetryCount Lib "dtwain64d.dll" (Source As System.IntPtr, nCount As Integer) As Integer
    Declare Auto Function DTWAIN_SetCustomDSData Lib "dtwain64d.dll" (Source As System.IntPtr, hData As System.IntPtr, <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U8, SizeParamIndex:=3)> Data As Byte(), dSize As UInteger, nFlags As Integer) As Integer
    Declare Auto Function DTWAIN_SetDSMSearchOrder Lib "dtwain64d.dll" (SearchPath As Integer) As Integer
    Declare Auto Function DTWAIN_SetDSMSearchOrderEx Lib "dtwain64d.dll" (SearchOrder As String, UserPath As String) As Integer
    Declare Ansi Function DTWAIN_SetDSMSearchOrderExA Lib "dtwain64d.dll" (SearchOrder As String, szUserPath As String) As Integer
    Declare Unicode Function DTWAIN_SetDSMSearchOrderExW Lib "dtwain64d.dll" (SearchOrder As String, szUserPath As String) As Integer
    Declare Auto Function DTWAIN_SetDefaultSource Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetDeviceNotifications Lib "dtwain64d.dll" (Source As System.IntPtr, DevEvents As Integer) As Integer
    Declare Auto Function DTWAIN_SetDeviceTimeDate Lib "dtwain64d.dll" (Source As System.IntPtr, szTimeDate As String) As Integer
    Declare Ansi Function DTWAIN_SetDeviceTimeDateA Lib "dtwain64d.dll" (Source As System.IntPtr, szTimeDate As String) As Integer
    Declare Unicode Function DTWAIN_SetDeviceTimeDateW Lib "dtwain64d.dll" (Source As System.IntPtr, szTimeDate As String) As Integer
    Declare Auto Function DTWAIN_SetDoubleFeedDetectLength Lib "dtwain64d.dll" (Source As System.IntPtr, Value As System.Double) As Integer
    Declare Auto Function DTWAIN_SetDoubleFeedDetectLengthString Lib "dtwain64d.dll" (Source As System.IntPtr, value As String) As Integer
    Declare Ansi Function DTWAIN_SetDoubleFeedDetectLengthStringA Lib "dtwain64d.dll" (Source As System.IntPtr, szLength As String) As Integer
    Declare Unicode Function DTWAIN_SetDoubleFeedDetectLengthStringW Lib "dtwain64d.dll" (Source As System.IntPtr, szLength As String) As Integer
    Declare Auto Function DTWAIN_SetDoubleFeedDetectValues Lib "dtwain64d.dll" (Source As System.IntPtr, prray As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetDoublePageCountOnDuplex Lib "dtwain64d.dll" (Source As System.IntPtr, bDoubleCount As Integer) As Integer
    Declare Auto Function DTWAIN_SetEOJDetectValue Lib "dtwain64d.dll" (Source As System.IntPtr, nValue As Integer) As Integer
    Declare Auto Function DTWAIN_SetErrorBufferThreshold Lib "dtwain64d.dll" (nErrors As Integer) As Integer
    Declare Auto Function DTWAIN_SetErrorCallback Lib "dtwain64d.dll" (proc As DTwainErrorProc, UserData As Integer) As Integer
    Declare Auto Function DTWAIN_SetErrorCallback64 Lib "dtwain64d.dll" (proc As DTwainErrorProc64, UserData64 As System.Int64) As Integer
    Declare Auto Function DTWAIN_SetFeederAlignment Lib "dtwain64d.dll" (Source As System.IntPtr, lpAlignment As Integer) As Integer
    Declare Auto Function DTWAIN_SetFeederOrder Lib "dtwain64d.dll" (Source As System.IntPtr, lOrder As Integer) As Integer
    Declare Auto Function DTWAIN_SetFeederWaitTime Lib "dtwain64d.dll" (Source As System.IntPtr, waitTime As Integer, flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileAutoIncrement Lib "dtwain64d.dll" (Source As System.IntPtr, Increment As Integer, bResetOnAcquire As Integer, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileCompressionType Lib "dtwain64d.dll" (Source As System.IntPtr, lCompression As Integer, bIsCustom As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileSavePos Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
    Declare Ansi Function DTWAIN_SetFileSavePosA Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
    Declare Unicode Function DTWAIN_SetFileSavePosW Lib "dtwain64d.dll" (hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileXferFormat Lib "dtwain64d.dll" (Source As System.IntPtr, lFileType As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetHalftone Lib "dtwain64d.dll" (Source As System.IntPtr, lpHalftone As String) As Integer
    Declare Ansi Function DTWAIN_SetHalftoneA Lib "dtwain64d.dll" (Source As System.IntPtr, lpHalftone As String) As Integer
    Declare Unicode Function DTWAIN_SetHalftoneW Lib "dtwain64d.dll" (Source As System.IntPtr, lpHalftone As String) As Integer
    Declare Auto Function DTWAIN_SetHighlight Lib "dtwain64d.dll" (Source As System.IntPtr, Highlight As System.Double) As Integer
    Declare Auto Function DTWAIN_SetHighlightString Lib "dtwain64d.dll" (Source As System.IntPtr, Highlight As String) As Integer
    Declare Ansi Function DTWAIN_SetHighlightStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Highlight As String) As Integer
    Declare Unicode Function DTWAIN_SetHighlightStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Highlight As String) As Integer
    Declare Auto Function DTWAIN_SetJobControl Lib "dtwain64d.dll" (Source As System.IntPtr, JobControl As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetJpegValues Lib "dtwain64d.dll" (Source As System.IntPtr, Quality As Integer, Progressive As Integer) As Integer
    Declare Auto Function DTWAIN_SetJpegXRValues Lib "dtwain64d.dll" (Source As System.IntPtr, Quality As Integer, Progressive As Integer) As Integer
    Declare Auto Function DTWAIN_SetLanguage Lib "dtwain64d.dll" (nLanguage As Integer) As Integer
    Declare Auto Function DTWAIN_SetLastError Lib "dtwain64d.dll" (nError As Integer) As Integer
    Declare Auto Function DTWAIN_SetLightPath Lib "dtwain64d.dll" (Source As System.IntPtr, LightPath As Integer) As Integer
    Declare Auto Function DTWAIN_SetLightPathEx Lib "dtwain64d.dll" (Source As System.IntPtr, LightPaths As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetLightSource Lib "dtwain64d.dll" (Source As System.IntPtr, LightSource As Integer) As Integer
    Declare Auto Function DTWAIN_SetLightSources Lib "dtwain64d.dll" (Source As System.IntPtr, LightSources As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetLoggerCallback Lib "dtwain64d.dll" (logProc As DTwainLoggerProc, UserData As System.Int64) As Integer
    Declare Auto Function DTWAIN_SetLoggerCallbackA Lib "dtwain64d.dll" (logProc As DTwainLoggerProcA, UserData As System.Int64) As Integer
    Declare Auto Function DTWAIN_SetLoggerCallbackW Lib "dtwain64d.dll" (logProc As DTwainLoggerProcW, UserData As System.Int64) As Integer
    Declare Auto Function DTWAIN_SetManualDuplexMode Lib "dtwain64d.dll" (Source As System.IntPtr, Flags As Integer, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetMaxAcquisitions Lib "dtwain64d.dll" (Source As System.IntPtr, MaxAcquires As Integer) As Integer
    Declare Auto Function DTWAIN_SetMaxBuffers Lib "dtwain64d.dll" (Source As System.IntPtr, MaxBuf As Integer) As Integer
    Declare Auto Function DTWAIN_SetMaxRetryAttempts Lib "dtwain64d.dll" (Source As System.IntPtr, nAttempts As Integer) As Integer
    Declare Auto Function DTWAIN_SetMultipageScanMode Lib "dtwain64d.dll" (Source As System.IntPtr, ScanType As Integer) As Integer
    Declare Auto Function DTWAIN_SetNoiseFilter Lib "dtwain64d.dll" (Source As System.IntPtr, NoiseFilter As Integer) As Integer
    Declare Auto Function DTWAIN_SetOCRCapValues Lib "dtwain64d.dll" (Engine As System.IntPtr, OCRCapValue As Integer, SetType As Integer, CapValues As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetOrientation Lib "dtwain64d.dll" (Source As System.IntPtr, Orient As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetOverscan Lib "dtwain64d.dll" (Source As System.IntPtr, Value As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFAESEncryption Lib "dtwain64d.dll" (Source As System.IntPtr, nWhichEncryption As Integer, bUseAES As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFASCIICompression Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFAuthor Lib "dtwain64d.dll" (Source As System.IntPtr, lpAuthor As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFAuthorA Lib "dtwain64d.dll" (Source As System.IntPtr, lpAuthor As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFAuthorW Lib "dtwain64d.dll" (Source As System.IntPtr, lpAuthor As String) As Integer
    Declare Auto Function DTWAIN_SetPDFCompression Lib "dtwain64d.dll" (Source As System.IntPtr, bCompression As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFCreator Lib "dtwain64d.dll" (Source As System.IntPtr, lpCreator As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFCreatorA Lib "dtwain64d.dll" (Source As System.IntPtr, lpCreator As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFCreatorW Lib "dtwain64d.dll" (Source As System.IntPtr, lpCreator As String) As Integer
    Declare Auto Function DTWAIN_SetPDFEncryption Lib "dtwain64d.dll" (Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
    Declare Ansi Function DTWAIN_SetPDFEncryptionA Lib "dtwain64d.dll" (Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
    Declare Unicode Function DTWAIN_SetPDFEncryptionW Lib "dtwain64d.dll" (Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFJpegQuality Lib "dtwain64d.dll" (Source As System.IntPtr, Quality As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFKeywords Lib "dtwain64d.dll" (Source As System.IntPtr, lpKeyWords As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFKeywordsA Lib "dtwain64d.dll" (Source As System.IntPtr, lpKeyWords As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFKeywordsW Lib "dtwain64d.dll" (Source As System.IntPtr, lpKeyWords As String) As Integer
    Declare Auto Function DTWAIN_SetPDFOCRConversion Lib "dtwain64d.dll" (Engine As System.IntPtr, PageType As Integer, FileType As Integer, PixelType As Integer, BitDepth As Integer, Options As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFOCRMode Lib "dtwain64d.dll" (Source As System.IntPtr, bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFOrientation Lib "dtwain64d.dll" (Source As System.IntPtr, lPOrientation As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFPageScale Lib "dtwain64d.dll" (Source As System.IntPtr, nOptions As Integer, xScale As System.Double, yScale As System.Double) As Integer
    Declare Auto Function DTWAIN_SetPDFPageScaleString Lib "dtwain64d.dll" (Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFPageScaleStringA Lib "dtwain64d.dll" (Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFPageScaleStringW Lib "dtwain64d.dll" (Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
    Declare Auto Function DTWAIN_SetPDFPageSize Lib "dtwain64d.dll" (Source As System.IntPtr, PageSize As Integer, CustomWidth As System.Double, CustomHeight As System.Double) As Integer
    Declare Auto Function DTWAIN_SetPDFPageSizeString Lib "dtwain64d.dll" (Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFPageSizeStringA Lib "dtwain64d.dll" (Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFPageSizeStringW Lib "dtwain64d.dll" (Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
    Declare Auto Function DTWAIN_SetPDFPolarity Lib "dtwain64d.dll" (Source As System.IntPtr, Polarity As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFProducer Lib "dtwain64d.dll" (Source As System.IntPtr, lpProducer As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFProducerA Lib "dtwain64d.dll" (Source As System.IntPtr, lpProducer As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFProducerW Lib "dtwain64d.dll" (Source As System.IntPtr, lpProducer As String) As Integer
    Declare Auto Function DTWAIN_SetPDFSubject Lib "dtwain64d.dll" (Source As System.IntPtr, lpSubject As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFSubjectA Lib "dtwain64d.dll" (Source As System.IntPtr, lpSubject As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFSubjectW Lib "dtwain64d.dll" (Source As System.IntPtr, lpSubject As String) As Integer
    Declare Auto Function DTWAIN_SetPDFTextElementFloat Lib "dtwain64d.dll" (TextElement As System.IntPtr, val1 As System.Double, val2 As System.Double, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFTextElementLong Lib "dtwain64d.dll" (TextElement As System.IntPtr, val1 As Integer, val2 As Integer, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFTextElementString Lib "dtwain64d.dll" (TextElement As System.IntPtr, val1 As String, Flags As Integer) As Integer
    Declare Ansi Function DTWAIN_SetPDFTextElementStringA Lib "dtwain64d.dll" (TextElement As System.IntPtr, szString As String, Flags As Integer) As Integer
    Declare Unicode Function DTWAIN_SetPDFTextElementStringW Lib "dtwain64d.dll" (TextElement As System.IntPtr, szString As String, Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFTitle Lib "dtwain64d.dll" (Source As System.IntPtr, lpTitle As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFTitleA Lib "dtwain64d.dll" (Source As System.IntPtr, lpTitle As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFTitleW Lib "dtwain64d.dll" (Source As System.IntPtr, lpTitle As String) As Integer
    Declare Auto Function DTWAIN_SetPaperSize Lib "dtwain64d.dll" (Source As System.IntPtr, PaperSize As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchMaxPriorities Lib "dtwain64d.dll" (Source As System.IntPtr, nMaxSearchRetries As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchMaxRetries Lib "dtwain64d.dll" (Source As System.IntPtr, nMaxRetries As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchPriorities Lib "dtwain64d.dll" (Source As System.IntPtr, SearchPriorities As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SetPatchSearchMode Lib "dtwain64d.dll" (Source As System.IntPtr, nSearchMode As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchTimeOut Lib "dtwain64d.dll" (Source As System.IntPtr, TimeOutValue As Integer) As Integer
    Declare Auto Function DTWAIN_SetPixelFlavor Lib "dtwain64d.dll" (Source As System.IntPtr, PixelFlavor As Integer) As Integer
    Declare Auto Function DTWAIN_SetPixelType Lib "dtwain64d.dll" (Source As System.IntPtr, PixelType As Integer, BitDepth As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPostScriptTitle Lib "dtwain64d.dll" (Source As System.IntPtr, szTitle As String) As Integer
    Declare Ansi Function DTWAIN_SetPostScriptTitleA Lib "dtwain64d.dll" (Source As System.IntPtr, szTitle As String) As Integer
    Declare Unicode Function DTWAIN_SetPostScriptTitleW Lib "dtwain64d.dll" (Source As System.IntPtr, szTitle As String) As Integer
    Declare Auto Function DTWAIN_SetPostScriptType Lib "dtwain64d.dll" (Source As System.IntPtr, PSType As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinter Lib "dtwain64d.dll" (Source As System.IntPtr, Printer As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterEx Lib "dtwain64d.dll" (Source As System.IntPtr, Printer As Integer, bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStartNumber Lib "dtwain64d.dll" (Source As System.IntPtr, nStart As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStringMode Lib "dtwain64d.dll" (Source As System.IntPtr, PrinterMode As Integer, bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStrings Lib "dtwain64d.dll" (Source As System.IntPtr, ArrayString As System.IntPtr, ByRef pNumStrings As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterSuffixString Lib "dtwain64d.dll" (Source As System.IntPtr, Suffix As String) As Integer
    Declare Ansi Function DTWAIN_SetPrinterSuffixStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Suffix As String) As Integer
    Declare Unicode Function DTWAIN_SetPrinterSuffixStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Suffix As String) As Integer
    Declare Auto Function DTWAIN_SetQueryCapSupport Lib "dtwain64d.dll" (bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetResolution Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As System.Double) As Integer
    Declare Auto Function DTWAIN_SetResolutionString Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Ansi Function DTWAIN_SetResolutionStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Unicode Function DTWAIN_SetResolutionStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Auto Function DTWAIN_SetResourcePath Lib "dtwain64d.dll" (ResourcePath As String) As Integer
    Declare Ansi Function DTWAIN_SetResourcePathA Lib "dtwain64d.dll" (ResourcePath As String) As Integer
    Declare Unicode Function DTWAIN_SetResourcePathW Lib "dtwain64d.dll" (ResourcePath As String) As Integer
    Declare Auto Function DTWAIN_SetRotation Lib "dtwain64d.dll" (Source As System.IntPtr, Rotation As System.Double) As Integer
    Declare Auto Function DTWAIN_SetRotationString Lib "dtwain64d.dll" (Source As System.IntPtr, Rotation As String) As Integer
    Declare Ansi Function DTWAIN_SetRotationStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Rotation As String) As Integer
    Declare Unicode Function DTWAIN_SetRotationStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Rotation As String) As Integer
    Declare Auto Function DTWAIN_SetSaveFileName Lib "dtwain64d.dll" (Source As System.IntPtr, fName As String) As Integer
    Declare Ansi Function DTWAIN_SetSaveFileNameA Lib "dtwain64d.dll" (Source As System.IntPtr, fName As String) As Integer
    Declare Unicode Function DTWAIN_SetSaveFileNameW Lib "dtwain64d.dll" (Source As System.IntPtr, fName As String) As Integer
    Declare Auto Function DTWAIN_SetShadow Lib "dtwain64d.dll" (Source As System.IntPtr, Shadow As System.Double) As Integer
    Declare Auto Function DTWAIN_SetShadowString Lib "dtwain64d.dll" (Source As System.IntPtr, Shadow As String) As Integer
    Declare Ansi Function DTWAIN_SetShadowStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Shadow As String) As Integer
    Declare Unicode Function DTWAIN_SetShadowStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Shadow As String) As Integer
    Declare Auto Function DTWAIN_SetSourceUnit Lib "dtwain64d.dll" (Source As System.IntPtr, Unit As Integer) As Integer
    Declare Auto Function DTWAIN_SetTIFFCompressType Lib "dtwain64d.dll" (Source As System.IntPtr, Setting As Integer) As Integer
    Declare Auto Function DTWAIN_SetTIFFInvert Lib "dtwain64d.dll" (Source As System.IntPtr, Setting As Integer) As Integer
    Declare Auto Function DTWAIN_SetTempFileDirectory Lib "dtwain64d.dll" (szFilePath As String) As Integer
    Declare Ansi Function DTWAIN_SetTempFileDirectoryA Lib "dtwain64d.dll" (szFilePath As String) As Integer
    Declare Auto Function DTWAIN_SetTempFileDirectoryEx Lib "dtwain64d.dll" (szFilePath As String, CreationFlags As Integer) As Integer
    Declare Ansi Function DTWAIN_SetTempFileDirectoryExA Lib "dtwain64d.dll" (szFilePath As String, CreationFlags As Integer) As Integer
    Declare Unicode Function DTWAIN_SetTempFileDirectoryExW Lib "dtwain64d.dll" (szFilePath As String, CreationFlags As Integer) As Integer
    Declare Unicode Function DTWAIN_SetTempFileDirectoryW Lib "dtwain64d.dll" (szFilePath As String) As Integer
    Declare Auto Function DTWAIN_SetThreshold Lib "dtwain64d.dll" (Source As System.IntPtr, Threshold As System.Double, bSetBithDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetThresholdString Lib "dtwain64d.dll" (Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
    Declare Ansi Function DTWAIN_SetThresholdStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
    Declare Unicode Function DTWAIN_SetThresholdStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainDSM Lib "dtwain64d.dll" (DSMType As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainLog Lib "dtwain64d.dll" (LogFlags As UInteger, lpszLogFile As String) As Integer
    Declare Ansi Function DTWAIN_SetTwainLogA Lib "dtwain64d.dll" (LogFlags As UInteger, lpszLogFile As String) As Integer
    Declare Unicode Function DTWAIN_SetTwainLogW Lib "dtwain64d.dll" (LogFlags As UInteger, lpszLogFile As String) As Integer
    Declare Auto Function DTWAIN_SetTwainMode Lib "dtwain64d.dll" (lAcquireMode As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainTimeout Lib "dtwain64d.dll" (milliseconds As Integer) As Integer
    Declare Auto Function DTWAIN_SetUpdateDibProc Lib "dtwain64d.dll" (DibProc As DTwainDIBUpdateProc) As DTwainDIBUpdateProc
    Declare Auto Function DTWAIN_SetXResolution Lib "dtwain64d.dll" (Source As System.IntPtr, xResolution As System.Double) As Integer
    Declare Auto Function DTWAIN_SetXResolutionString Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Ansi Function DTWAIN_SetXResolutionStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Unicode Function DTWAIN_SetXResolutionStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Auto Function DTWAIN_SetYResolution Lib "dtwain64d.dll" (Source As System.IntPtr, yResolution As System.Double) As Integer
    Declare Auto Function DTWAIN_SetYResolutionString Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Ansi Function DTWAIN_SetYResolutionStringA Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Unicode Function DTWAIN_SetYResolutionStringW Lib "dtwain64d.dll" (Source As System.IntPtr, Resolution As String) As Integer
    Declare Auto Function DTWAIN_ShowUIOnly Lib "dtwain64d.dll" (Source As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_ShutdownOCREngine Lib "dtwain64d.dll" (OCREngine As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_SkipImageInfoError Lib "dtwain64d.dll" (Source As System.IntPtr, bSkip As Integer) As Integer
    Declare Auto Function DTWAIN_StartThread Lib "dtwain64d.dll" (DLLHandle As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_StartTwainSession Lib "dtwain64d.dll" (hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
    Declare Ansi Function DTWAIN_StartTwainSessionA Lib "dtwain64d.dll" (hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
    Declare Unicode Function DTWAIN_StartTwainSessionW Lib "dtwain64d.dll" (hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
    Declare Auto Function DTWAIN_SysDestroy Lib "dtwain64d.dll" () As Integer
    Declare Auto Function DTWAIN_SysInitialize Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_SysInitializeEx Lib "dtwain64d.dll" (szINIPath As String) As System.IntPtr
    Declare Auto Function DTWAIN_SysInitializeEx2 Lib "dtwain64d.dll" (szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
    Declare Ansi Function DTWAIN_SysInitializeEx2A Lib "dtwain64d.dll" (szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
    Declare Unicode Function DTWAIN_SysInitializeEx2W Lib "dtwain64d.dll" (szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
    Declare Ansi Function DTWAIN_SysInitializeExA Lib "dtwain64d.dll" (szINIPath As String) As System.IntPtr
    Declare Unicode Function DTWAIN_SysInitializeExW Lib "dtwain64d.dll" (szINIPath As String) As System.IntPtr
    Declare Auto Function DTWAIN_SysInitializeLib Lib "dtwain64d.dll" (hInstance As System.IntPtr) As System.IntPtr
    Declare Auto Function DTWAIN_SysInitializeLibEx Lib "dtwain64d.dll" (hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
    Declare Auto Function DTWAIN_SysInitializeLibEx2 Lib "dtwain64d.dll" (hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
    Declare Ansi Function DTWAIN_SysInitializeLibEx2A Lib "dtwain64d.dll" (hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
    Declare Unicode Function DTWAIN_SysInitializeLibEx2W Lib "dtwain64d.dll" (hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
    Declare Ansi Function DTWAIN_SysInitializeLibExA Lib "dtwain64d.dll" (hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
    Declare Unicode Function DTWAIN_SysInitializeLibExW Lib "dtwain64d.dll" (hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
    Declare Auto Function DTWAIN_SysInitializeNoBlocking Lib "dtwain64d.dll" () As System.IntPtr
    Declare Auto Function DTWAIN_TestGetCap Lib "dtwain64d.dll" (Source As System.IntPtr, lCapability As Integer) As System.IntPtr
    Declare Auto Function DTWAIN_UnlockMemory Lib "dtwain64d.dll" (h As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_UnlockMemoryEx Lib "dtwain64d.dll" (h As System.IntPtr) As Integer
    Declare Auto Function DTWAIN_UseMultipleThreads Lib "dtwain64d.dll" (bSet As Integer) As Integer

End Class
