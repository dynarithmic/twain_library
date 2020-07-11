REM
REM This file is part of the Dynarithmic TWAIN Library (DTWAIN).                          
REM Copyright (c) 2002-2019 Dynarithmic Software.                                         
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

REM ******************************************************************************
REM DTWAIN.BAS Declaration statements for Visual Basic .NET programs
Rem         This is the translation of the DTWAIN.H include file
Rem *****************************************************************************

REM  The DTWAIN VB .NET class is called DTWAINAPI
Class DTWAINAPI
    Structure DTWAIN_POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure

    Structure WinMsg
        Dim hwnd As Integer
        Dim message As Integer
        Dim wParam As Integer
        Dim lParam As Integer
        Dim time As Integer
        Dim pt As DTWAIN_POINTAPI
    End Structure

    Public Const DTWAIN_FF_TIFF As Integer  = 0
    Public Const DTWAIN_FF_PICT As Integer  = 1
    Public Const DTWAIN_FF_BMP As Integer  = 2
    Public Const DTWAIN_FF_XBM As Integer  = 3
    Public Const DTWAIN_FF_JFIF As Integer  = 4
    Public Const DTWAIN_FF_FPX As Integer  = 5
    Public Const DTWAIN_FF_TIFFMULTI As Integer  = 6
    Public Const DTWAIN_FF_PNG As Integer  = 7
    Public Const DTWAIN_FF_SPIFF As Integer  = 8
    Public Const DTWAIN_FF_EXIF As Integer  = 9
    Public Const DTWAIN_FF_PDF As Integer  = 10
    Public Const DTWAIN_FF_JP2 As Integer  = 11
    Public Const DTWAIN_FF_JPX As Integer  = 13
    Public Const DTWAIN_FF_DEJAVU As Integer  = 14
    Public Const DTWAIN_FF_PDFA As Integer  = 15
    Public Const DTWAIN_FF_PDFA2 As Integer  = 16
    Public Const DTWAIN_CP_NONE As Integer  = 0
    Public Const DTWAIN_CP_PACKBITS As Integer  = 1
    Public Const DTWAIN_CP_GROUP31D As Integer  = 2
    Public Const DTWAIN_CP_GROUP31DEOL As Integer  = 3
    Public Const DTWAIN_CP_GROUP32D As Integer  = 4
    Public Const DTWAIN_CP_GROUP4 As Integer  = 5
    Public Const DTWAIN_CP_JPEG As Integer  = 6
    Public Const DTWAIN_CP_LZW As Integer  = 7
    Public Const DTWAIN_CP_JBIG As Integer  = 8
    Public Const DTWAIN_CP_PNG As Integer  = 9
    Public Const DTWAIN_CP_RLE4 As Integer  = 10
    Public Const DTWAIN_CP_RLE8 As Integer  = 11
    Public Const DTWAIN_CP_BITFIELDS As Integer  = 12
    Public Const DTWAIN_CP_ZIP As Integer  = 13
    Public Const DTWAIN_CP_JPEG2000 As Integer  = 14
    Public Const DTWAIN_FS_NONE As Integer  = 0
    Public Const DTWAIN_FS_A4LETTER As Integer  = 1
    Public Const DTWAIN_FS_B5LETTER As Integer  = 2
    Public Const DTWAIN_FS_USLETTER As Integer  = 3
    Public Const DTWAIN_FS_USLEGAL As Integer  = 4
    Public Const DTWAIN_FS_A5 As Integer  = 5
    Public Const DTWAIN_FS_B4 As Integer  = 6
    Public Const DTWAIN_FS_B6 As Integer  = 7
    Public Const DTWAIN_FS_USLEDGER As Integer  = 9
    Public Const DTWAIN_FS_USEXECUTIVE As Integer  = 10
    Public Const DTWAIN_FS_A3 As Integer  = 11
    Public Const DTWAIN_FS_B3 As Integer  = 12
    Public Const DTWAIN_FS_A6 As Integer  = 13
    Public Const DTWAIN_FS_C4 As Integer  = 14
    Public Const DTWAIN_FS_C5 As Integer  = 15
    Public Const DTWAIN_FS_C6 As Integer  = 16
    Public Const DTWAIN_FS_4A0 As Integer  = 17
    Public Const DTWAIN_FS_2A0 As Integer  = 18
    Public Const DTWAIN_FS_A0 As Integer  = 19
    Public Const DTWAIN_FS_A1 As Integer  = 20
    Public Const DTWAIN_FS_A2 As Integer  = 21
    Public Const DTWAIN_FS_A4 As Integer  = DTWAIN_FS_A4LETTER
    Public Const DTWAIN_FS_A7 As Integer  = 22
    Public Const DTWAIN_FS_A8 As Integer  = 23
    Public Const DTWAIN_FS_A9 As Integer  = 24
    Public Const DTWAIN_FS_A10 As Integer  = 25
    Public Const DTWAIN_FS_ISOB0 As Integer  = 26
    Public Const DTWAIN_FS_ISOB1 As Integer  = 27
    Public Const DTWAIN_FS_ISOB2 As Integer  = 28
    Public Const DTWAIN_FS_ISOB3 As Integer  = DTWAIN_FS_B3
    Public Const DTWAIN_FS_ISOB4 As Integer  = DTWAIN_FS_B4
    Public Const DTWAIN_FS_ISOB5 As Integer  = 29
    Public Const DTWAIN_FS_ISOB6 As Integer  = DTWAIN_FS_B6
    Public Const DTWAIN_FS_ISOB7 As Integer  = 30
    Public Const DTWAIN_FS_ISOB8 As Integer  = 31
    Public Const DTWAIN_FS_ISOB9 As Integer  = 32
    Public Const DTWAIN_FS_ISOB10 As Integer  = 33
    Public Const DTWAIN_FS_JISB0 As Integer  = 34
    Public Const DTWAIN_FS_JISB1 As Integer  = 35
    Public Const DTWAIN_FS_JISB2 As Integer  = 36
    Public Const DTWAIN_FS_JISB3 As Integer  = 37
    Public Const DTWAIN_FS_JISB4 As Integer  = 38
    Public Const DTWAIN_FS_JISB5 As Integer  = DTWAIN_FS_B5LETTER
    Public Const DTWAIN_FS_JISB6 As Integer  = 39
    Public Const DTWAIN_FS_JISB7 As Integer  = 40
    Public Const DTWAIN_FS_JISB8 As Integer  = 41
    Public Const DTWAIN_FS_JISB9 As Integer  = 42
    Public Const DTWAIN_FS_JISB10 As Integer  = 43
    Public Const DTWAIN_FS_C0 As Integer  = 44
    Public Const DTWAIN_FS_C1 As Integer  = 45
    Public Const DTWAIN_FS_C2 As Integer  = 46
    Public Const DTWAIN_FS_C3 As Integer  = 47
    Public Const DTWAIN_FS_C7 As Integer  = 48
    Public Const DTWAIN_FS_C8 As Integer  = 49
    Public Const DTWAIN_FS_C9 As Integer  = 50
    Public Const DTWAIN_FS_C10 As Integer  = 51
    Public Const DTWAIN_FS_USSTATEMENT As Integer  = 52
    Public Const DTWAIN_FS_BUSINESSCARD As Integer  = 53
    Public Const DTWAIN_ANYSUPPORT As Integer  = (-1)
    Public Const DTWAIN_BMP As Integer  = 100
    Public Const DTWAIN_JPEG As Integer  = 200
    Public Const DTWAIN_PDF As Integer  = 250
    Public Const DTWAIN_PDFMULTI As Integer  = 251
    Public Const DTWAIN_PCX As Integer  = 300
    Public Const DTWAIN_DCX As Integer  = 301
    Public Const DTWAIN_TGA As Integer  = 400
    Public Const DTWAIN_TIFFLZW As Integer  = 500
    Public Const DTWAIN_TIFFNONE As Integer  = 600
    Public Const DTWAIN_TIFFG3 As Integer  = 700
    Public Const DTWAIN_TIFFG4 As Integer  = 800
    Public Const DTWAIN_TIFFPACKBITS As Integer  = 801
    Public Const DTWAIN_TIFFDEFLATE As Integer  = 802
    Public Const DTWAIN_TIFFJPEG As Integer  = 803
    Public Const DTWAIN_TIFFJBIG As Integer  = 804
    Public Const DTWAIN_TIFFPIXARLOG As Integer  = 805
    Public Const DTWAIN_TIFFNONEMULTI As Integer  = 900
    Public Const DTWAIN_TIFFG3MULTI As Integer  = 901
    Public Const DTWAIN_TIFFG4MULTI As Integer  = 902
    Public Const DTWAIN_TIFFPACKBITSMULTI As Integer  = 903
    Public Const DTWAIN_TIFFDEFLATEMULTI As Integer  = 904
    Public Const DTWAIN_TIFFJPEGMULTI As Integer  = 905
    Public Const DTWAIN_TIFFLZWMULTI As Integer  = 906
    Public Const DTWAIN_TIFFJBIGMULTI As Integer  = 907
    Public Const DTWAIN_TIFFPIXARLOGMULTI As Integer  = 908
    Public Const DTWAIN_WMF As Integer  = 850
    Public Const DTWAIN_EMF As Integer  = 851
    Public Const DTWAIN_GIF As Integer  = 950
    Public Const DTWAIN_PNG As Integer  = 1000
    Public Const DTWAIN_PSD As Integer  = 2000
    Public Const DTWAIN_JPEG2000 As Integer  = 3000
    Public Const DTWAIN_POSTSCRIPT1 As Integer  = 4000
    Public Const DTWAIN_POSTSCRIPT2 As Integer  = 4001
    Public Const DTWAIN_POSTSCRIPT3 As Integer  = 4002
    Public Const DTWAIN_POSTSCRIPT1MULTI As Integer  = 4003
    Public Const DTWAIN_POSTSCRIPT2MULTI As Integer  = 4004
    Public Const DTWAIN_POSTSCRIPT3MULTI As Integer  = 4005
    Public Const DTWAIN_TEXT As Integer  = 6000
    Public Const DTWAIN_TEXTMULTI As Integer  = 6001
    Public Const DTWAIN_TIFFMULTI As Integer  = 7000
    Public Const DTWAIN_ICO As Integer  = 8000
    Public Const DTWAIN_ICO_VISTA As Integer  = 8001
    Public Const DTWAIN_WBMP As Integer  = 8500
    Public Const DTWAIN_INCHES As Integer  = 0
    Public Const DTWAIN_CENTIMETERS As Integer  = 1
    Public Const DTWAIN_PICAS As Integer  = 2
    Public Const DTWAIN_POINTS As Integer  = 3
    Public Const DTWAIN_TWIPS As Integer  = 4
    Public Const DTWAIN_PIXELS As Integer  = 5
    Public Const DTWAIN_MILLIMETERS As Integer  = 6
    Public Const DTWAIN_USENAME As Integer  = 4
    Public Const DTWAIN_USEPROMPT As Integer  = 8
    Public Const DTWAIN_USELONGNAME As Integer  = 16
    Public Const DTWAIN_USESOURCEMODE As Integer  = 32
    Public Const DTWAIN_USELIST As Integer  = 64
    Public Const DTWAIN_ARRAYANY As Integer  = 1
    Public Const DTWAIN_ArrayTypePTR As Integer  = 1
    Public Const DTWAIN_ARRAYLONG As Integer  = 2
    Public Const DTWAIN_ARRAYFLOAT As Integer  = 3
    Public Const DTWAIN_ARRAYHANDLE As Integer  = 4
    Public Const DTWAIN_ARRAYSOURCE As Integer  = 5
    Public Const DTWAIN_ARRAYSTRING As Integer  = 6
    Public Const DTWAIN_ARRAYFRAME As Integer  = 7
    Public Const DTWAIN_ARRAYBOOL As Integer  = DTWAIN_ARRAYLONG
    Public Const DTWAIN_ARRAYLONGSTRING As Integer  = 8
    Public Const DTWAIN_ARRAYUNICODESTRING As Integer  = 9
    Public Const DTWAIN_ARRAYLONG64 As Integer  = 10
    Public Const DTWAIN_ARRAYANSISTRING As Integer  = 11
    Public Const DTWAIN_ARRAYWIDESTRING As Integer  = 12
    Public Const DTWAIN_ARRAYTWFIX32 As Integer  = 200
    Public Const DTWAIN_ArrayTypeINVALID As Integer  = 0
    Public Const DTWAIN_ARRAYINT16 As Integer  = 100
    Public Const DTWAIN_ARRAYUINT16 As Integer  = 110
    Public Const DTWAIN_ARRAYUINT32 As Integer  = 120
    Public Const DTWAIN_ARRAYINT32 As Integer  = 130
    Public Const DTWAIN_ARRAYINT64 As Integer  = 140
    Public Const DTWAIN_RANGELONG As Integer  = DTWAIN_ARRAYLONG
    Public Const DTWAIN_RANGEFLOAT As Integer  = DTWAIN_ARRAYFLOAT
    Public Const DTWAIN_RANGEMIN As Integer  = 0
    Public Const DTWAIN_RANGEMAX As Integer  = 1
    Public Const DTWAIN_RANGESTEP As Integer  = 2
    Public Const DTWAIN_RANGEDEFAULT As Integer  = 3
    Public Const DTWAIN_RANGECURRENT As Integer  = 4
    Public Const DTWAIN_FRAMELEFT As Integer  = 0
    Public Const DTWAIN_FRAMETOP As Integer  = 1
    Public Const DTWAIN_FRAMERIGHT As Integer  = 2
    Public Const DTWAIN_FRAMEBOTTOM As Integer  = 3
    Public Const DTWAIN_FIX32WHOLE As Integer  = 0
    Public Const DTWAIN_FIX32FRAC As Integer  = 1
    Public Const DTWAIN_JC_NONE As Integer  = 0
    Public Const DTWAIN_JC_JSIC As Integer  = 1
    Public Const DTWAIN_JC_JSIS As Integer  = 2
    Public Const DTWAIN_JC_JSXC As Integer  = 3
    Public Const DTWAIN_JC_JSXS As Integer  = 4
    Public Const DTWAIN_CAPDATATYPE_UNKNOWN As Integer  = (-10)
    Public Const DTWAIN_JCBP_JSIC As Integer  = 5
    Public Const DTWAIN_JCBP_JSIS As Integer  = 6
    Public Const DTWAIN_JCBP_JSXC As Integer  = 7
    Public Const DTWAIN_JCBP_JSXS As Integer  = 8
    Public Const DTWAIN_FEEDPAGEON As Integer  = 1
    Public Const DTWAIN_CLEARPAGEON As Integer  = 2
    Public Const DTWAIN_REWINDPAGEON As Integer  = 4
    Public Const DTWAIN_AppOwnsDib As Integer  = 1
    Public Const DTWAIN_SourceOwnsDib As Integer  = 2
    Public Const DTWAIN_CONTARRAY As Integer  = 8
    Public Const DTWAIN_CONTENUMERATION As Integer  = 16
    Public Const DTWAIN_CONTONEVALUE As Integer  = 32
    Public Const DTWAIN_CONTRANGE As Integer  = 64
    Public Const DTWAIN_CONTDEFAULT As Integer  = 0
    Public Const DTWAIN_CAPGET As Integer  = 1
    Public Const DTWAIN_CAPGETCURRENT As Integer  = 2
    Public Const DTWAIN_CAPGETDEFAULT As Integer  = 3
    Public Const DTWAIN_CAPSET As Integer  = 6
    Public Const DTWAIN_CAPRESET As Integer  = 7
    Public Const DTWAIN_CAPRESETALL As Integer  = 8
    Public Const DTWAIN_CAPSETCONSTRAINT As Integer  = 9
    Public Const DTWAIN_CAPSETAVAILABLE As Integer  = 8
    Public Const DTWAIN_CAPSETCURRENT As Integer  = 16
    Public Const DTWAIN_AREASET As Integer  = DTWAIN_CAPSET
    Public Const DTWAIN_AREARESET As Integer  = DTWAIN_CAPRESET
    Public Const DTWAIN_AREACURRENT As Integer  = DTWAIN_CAPGETCURRENT
    Public Const DTWAIN_AREADEFAULT As Integer  = DTWAIN_CAPGETDEFAULT
    Public Const DTWAIN_VER15 As Integer  = 0
    Public Const DTWAIN_VER16 As Integer  = 1
    Public Const DTWAIN_VER17 As Integer  = 2
    Public Const DTWAIN_VER18 As Integer  = 3
    Public Const DTWAIN_VER20 As Integer  = 4
    Public Const DTWAIN_VER21 As Integer  = 5
    Public Const DTWAIN_VER22 As Integer  = 6
    Public Const DTWAIN_ACQUIREALL As Integer  = (-1)
    Public Const DTWAIN_MAXACQUIRE As Integer  = (-1)
    Public Const DTWAIN_DX_NONE As Integer  = 0
    Public Const DTWAIN_DX_1PASSDUPLEX As Integer  = 1
    Public Const DTWAIN_DX_2PASSDUPLEX As Integer  = 2
    Public Const DTWAIN_PT_BW As Integer  = 0
    Public Const DTWAIN_PT_GRAY As Integer  = 1
    Public Const DTWAIN_PT_RGB As Integer  = 2
    Public Const DTWAIN_PT_PALETTE As Integer  = 3
    Public Const DTWAIN_PT_CMY As Integer  = 4
    Public Const DTWAIN_PT_CMYK As Integer  = 5
    Public Const DTWAIN_PT_YUV As Integer  = 6
    Public Const DTWAIN_PT_YUVK As Integer  = 7
    Public Const DTWAIN_PT_CIEXYZ As Integer  = 8
    Public Const DTWAIN_PT_DEFAULT As Integer  = 1000
    Public Const DTWAIN_CURRENT As Integer  = (-2)
    Public Const DTWAIN_DEFAULT As Integer  = (-1)
    Public Const DTWAIN_FLOATDEFAULT As Double  = (-9999.0)
    Public Const DTWAIN_CallbackERROR As Integer  = 1
    Public Const DTWAIN_CallbackMESSAGE As Integer  = 2
    Public Const DTWAIN_USENATIVE As Integer  = 1
    Public Const DTWAIN_USEBUFFERED As Integer  = 2
    Public Const DTWAIN_USECOMPRESSION As Integer  = 4
    Public Const DTWAIN_FAILURE1 As Integer  = (-1)
    Public Const DTWAIN_FAILURE2 As Integer  = (-2)
    Public Const DTWAIN_DELETEALL As Integer  = (-1)
    Public Const DTWAIN_TN_ACQUIREDONE As Integer  = 1000
    Public Const DTWAIN_TN_ACQUIREFAILED As Integer  = 1001
    Public Const DTWAIN_TN_ACQUIRECANCELLED As Integer  = 1002
    Public Const DTWAIN_TN_ACQUIRESTARTED As Integer  = 1003
    Public Const DTWAIN_TN_PAGECONTINUE As Integer  = 1004
    Public Const DTWAIN_TN_PAGEFAILED As Integer  = 1005
    Public Const DTWAIN_TN_PAGECANCELLED As Integer  = 1006
    Public Const DTWAIN_TN_TRANSFERREADY As Integer  = 1009
    Public Const DTWAIN_TN_TRANSFERDONE As Integer  = 1010
    Public Const DTWAIN_TN_ACQUIREPAGEDONE As Integer  = 1010
    Public Const DTWAIN_TN_UICLOSING As Integer  = 1011
    Public Const DTWAIN_TN_UICLOSED As Integer  = 1012
    Public Const DTWAIN_TN_UIOPENED As Integer  = 1013
    Public Const DTWAIN_TN_UIOPENING As Integer  = 1055
    Public Const DTWAIN_TN_UIOPENFAILURE As Integer  = 1060
    Public Const DTWAIN_TN_CLIPTRANSFERDONE As Integer  = 1014
    Public Const DTWAIN_TN_INVALIDIMAGEFORMAT As Integer  = 1015
    Public Const DTWAIN_TN_ACQUIRETERMINATED As Integer  = 1021
    Public Const DTWAIN_TN_TRANSFERSTRIPREADY As Integer  = 1022
    Public Const DTWAIN_TN_TRANSFERSTRIPDONE As Integer  = 1023
    Public Const DTWAIN_TN_TRANSFERSTRIPFAILED As Integer  = 1029
    Public Const DTWAIN_TN_IMAGEINFOERROR As Integer  = 1024
    Public Const DTWAIN_TN_TRANSFERCANCELLED As Integer  = 1030
    Public Const DTWAIN_TN_FILESAVECANCELLED As Integer  = 1031
    Public Const DTWAIN_TN_FILESAVEOK As Integer  = 1032
    Public Const DTWAIN_TN_FILESAVEERROR As Integer  = 1033
    Public Const DTWAIN_TN_FILEPAGESAVEOK As Integer  = 1034
    Public Const DTWAIN_TN_FILEPAGESAVEERROR As Integer  = 1035
    Public Const DTWAIN_TN_PROCESSEDDIB As Integer  = 1036
    Public Const DTWAIN_TN_FEEDERLOADED As Integer  = 1037
    Public Const DTWAIN_TN_GENERALERROR As Integer  = 1038
    Public Const DTWAIN_TN_MANDUPFLIPPAGES As Integer  = 1040
    Public Const DTWAIN_TN_MANDUPSIDE1DONE As Integer  = 1041
    Public Const DTWAIN_TN_MANDUPSIDE2DONE As Integer  = 1042
    Public Const DTWAIN_TN_MANDUPPAGECOUNTERROR As Integer  = 1043
    Public Const DTWAIN_TN_MANDUPACQUIREDONE As Integer  = 1044
    Public Const DTWAIN_TN_MANDUPSIDE1START As Integer  = 1045
    Public Const DTWAIN_TN_MANDUPSIDE2START As Integer  = 1046
    Public Const DTWAIN_TN_MANDUPMERGEERROR As Integer  = 1047
    Public Const DTWAIN_TN_MANDUPMEMORYERROR As Integer  = 1048
    Public Const DTWAIN_TN_MANDUPFILEERROR As Integer  = 1049
    Public Const DTWAIN_TN_MANDUPFILESAVEERROR As Integer  = 1050
    Public Const DTWAIN_TN_ENDOFJOBDETECTED As Integer  = 1051
    Public Const DTWAIN_TN_EOJDETECTED As Integer  = 1051
    Public Const DTWAIN_TN_EOJDETECTED_XFERDONE As Integer  = 1052
    Public Const DTWAIN_TN_QUERYPAGEDISCARD As Integer  = 1053
    Public Const DTWAIN_TN_PAGEDISCARDED As Integer  = 1054
    Public Const DTWAIN_TN_PROCESSDIBACCEPTED As Integer  = 1055
    Public Const DTWAIN_TN_PROCESSDIBFINALACCEPTED As Integer  = 1056
    Public Const DTWAIN_TN_DEVICEEVENT As Integer  = 1100
    Public Const DTWAIN_TN_TWAINPAGECANCELLED As Integer  = 1105
    Public Const DTWAIN_TN_TWAINPAGEFAILED As Integer  = 1106
    Public Const DTWAIN_TN_APPUPDATEDDIB As Integer  = 1107
    Public Const DTWAIN_TN_FILEPAGESAVING As Integer  = 1110
    Public Const DTWAIN_TN_EOJBEGINFILESAVE As Integer  = 1112
    Public Const DTWAIN_TN_EOJENDFILESAVE As Integer  = 1113
    Public Const DTWAIN_TN_CROPFAILED As Integer  = 1120
    Public Const DTWAIN_TN_PROCESSEDDIBFINAL As Integer  = 1121
    Public Const DTWAIN_TN_BLANKPAGEDETECTED1 As Integer  = 1130
    Public Const DTWAIN_TN_BLANKPAGEDETECTED2 As Integer  = 1131
    Public Const DTWAIN_TN_BLANKPAGEDETECTED3 As Integer  = 1132
    Public Const DTWAIN_TN_BLANKPAGEDISCARDED1 As Integer  = 1133
    Public Const DTWAIN_TN_BLANKPAGEDISCARDED2 As Integer  = 1134
    Public Const DTWAIN_TN_OCRTEXTRETRIEVED As Integer  = 1140
    Public Const DTWAIN_TN_QUERYOCRTEXT As Integer  = 1141
    Public Const DTWAIN_TN_PDFOCRREADY As Integer  = 1142
    Public Const DTWAIN_TN_PDFOCRDONE As Integer  = 1143
    Public Const DTWAIN_TN_PDFOCRERROR As Integer  = 1144
    Public Const DTWAIN_TN_SETCALLBACKINIT As Integer  = 1150
    Public Const DTWAIN_TN_SETCALLBACK64INIT As Integer  = 1151
    Public Const DTWAIN_TN_FILENAMECHANGING As Integer  = 1160
    Public Const DTWAIN_TN_FILENAMECHANGED As Integer  = 1161
    Public Const DTWAIN_PDFOCR_CLEANTEXT1 As Integer  = 1
    Public Const DTWAIN_PDFOCR_CLEANTEXT2 As Integer  = 2
    Public Const DTWAIN_MODAL As Integer  = 0
    Public Const DTWAIN_MODELESS As Integer  = 1
    Public Const DTWAIN_UIModeCLOSE As Integer  = 0
    Public Const DTWAIN_UIModeOPEN As Integer  = 1
    Public Const DTWAIN_REOPEN_SOURCE As Integer  = 2
    Public Const DTWAIN_ROUNDNEAREST As Integer  = 0
    Public Const DTWAIN_ROUNDUP As Integer  = 1
    Public Const DTWAIN_ROUNDDOWN As Integer  = 2
    Public Const DTWAIN_FLOATDELTA As Double  = 1.0E-08
    Public Const DTWAIN_OR_ROT0 As Integer  = 0
    Public Const DTWAIN_OR_ROT90 As Integer  = 1
    Public Const DTWAIN_OR_ROT180 As Integer  = 2
    Public Const DTWAIN_OR_ROT270 As Integer  = 3
    Public Const DTWAIN_OR_PORTRAIT As Integer  = DTWAIN_OR_ROT0
    Public Const DTWAIN_OR_LANDSCAPE As Integer  = DTWAIN_OR_ROT270
    Public Const DTWAIN_OR_ANYROTATION As Integer  = (-1)
    Public Const DTWAIN_CO_GET As Integer  = &H0001
    Public Const DTWAIN_CO_SET As Integer  = &H0002
    Public Const DTWAIN_CO_GETDEFAULT As Integer  = &H0004
    Public Const DTWAIN_CO_GETCURRENT As Integer  = &H0008
    Public Const DTWAIN_CO_RESET As Integer  = &H0010
    Public Const DTWAIN_CO_SETCONSTRAINT As Integer  = &H0020
    Public Const DTWAIN_CO_CONSTRAINABLE As Integer  = &H0040
    Public Const DTWAIN_CO_GETHELP As Integer  = &H0100
    Public Const DTWAIN_CO_GETLABEL As Integer  = &H0200
    Public Const DTWAIN_CO_GETLABELENUM As Integer  = &H0400
    Public Const DTWAIN_CNTYAFGHANISTAN As Integer  = 1001
    Public Const DTWAIN_CNTYALGERIA As Integer  = 213
    Public Const DTWAIN_CNTYAMERICANSAMOA As Integer  = 684
    Public Const DTWAIN_CNTYANDORRA As Integer  = 033
    Public Const DTWAIN_CNTYANGOLA As Integer  = 1002
    Public Const DTWAIN_CNTYANGUILLA As Integer  = 8090
    Public Const DTWAIN_CNTYANTIGUA As Integer  = 8091
    Public Const DTWAIN_CNTYARGENTINA As Integer  = 54
    Public Const DTWAIN_CNTYARUBA As Integer  = 297
    Public Const DTWAIN_CNTYASCENSIONI As Integer  = 247
    Public Const DTWAIN_CNTYAUSTRALIA As Integer  = 61
    Public Const DTWAIN_CNTYAUSTRIA As Integer  = 43
    Public Const DTWAIN_CNTYBAHAMAS As Integer  = 8092
    Public Const DTWAIN_CNTYBAHRAIN As Integer  = 973
    Public Const DTWAIN_CNTYBANGLADESH As Integer  = 880
    Public Const DTWAIN_CNTYBARBADOS As Integer  = 8093
    Public Const DTWAIN_CNTYBELGIUM As Integer  = 32
    Public Const DTWAIN_CNTYBELIZE As Integer  = 501
    Public Const DTWAIN_CNTYBENIN As Integer  = 229
    Public Const DTWAIN_CNTYBERMUDA As Integer  = 8094
    Public Const DTWAIN_CNTYBHUTAN As Integer  = 1003
    Public Const DTWAIN_CNTYBOLIVIA As Integer  = 591
    Public Const DTWAIN_CNTYBOTSWANA As Integer  = 267
    Public Const DTWAIN_CNTYBRITAIN As Integer  = 6
    Public Const DTWAIN_CNTYBRITVIRGINIS As Integer  = 8095
    Public Const DTWAIN_CNTYBRAZIL As Integer  = 55
    Public Const DTWAIN_CNTYBRUNEI As Integer  = 673
    Public Const DTWAIN_CNTYBULGARIA As Integer  = 359
    Public Const DTWAIN_CNTYBURKINAFASO As Integer  = 1004
    Public Const DTWAIN_CNTYBURMA As Integer  = 1005
    Public Const DTWAIN_CNTYBURUNDI As Integer  = 1006
    Public Const DTWAIN_CNTYCAMAROON As Integer  = 237
    Public Const DTWAIN_CNTYCANADA As Integer  = 2
    Public Const DTWAIN_CNTYCAPEVERDEIS As Integer  = 238
    Public Const DTWAIN_CNTYCAYMANIS As Integer  = 8096
    Public Const DTWAIN_CNTYCENTRALAFREP As Integer  = 1007
    Public Const DTWAIN_CNTYCHAD As Integer  = 1008
    Public Const DTWAIN_CNTYCHILE As Integer  = 56
    Public Const DTWAIN_CNTYCHINA As Integer  = 86
    Public Const DTWAIN_CNTYCHRISTMASIS As Integer  = 1009
    Public Const DTWAIN_CNTYCOCOSIS As Integer  = 1009
    Public Const DTWAIN_CNTYCOLOMBIA As Integer  = 57
    Public Const DTWAIN_CNTYCOMOROS As Integer  = 1010
    Public Const DTWAIN_CNTYCONGO As Integer  = 1011
    Public Const DTWAIN_CNTYCOOKIS As Integer  = 1012
    Public Const DTWAIN_CNTYCOSTARICA As Integer  = 506
    Public Const DTWAIN_CNTYCUBA As Integer  = 005
    Public Const DTWAIN_CNTYCYPRUS As Integer  = 357
    Public Const DTWAIN_CNTYCZECHOSLOVAKIA As Integer  = 42
    Public Const DTWAIN_CNTYDENMARK As Integer  = 45
    Public Const DTWAIN_CNTYDJIBOUTI As Integer  = 1013
    Public Const DTWAIN_CNTYDOMINICA As Integer  = 8097
    Public Const DTWAIN_CNTYDOMINCANREP As Integer  = 8098
    Public Const DTWAIN_CNTYEASTERIS As Integer  = 1014
    Public Const DTWAIN_CNTYECUADOR As Integer  = 593
    Public Const DTWAIN_CNTYEGYPT As Integer  = 20
    Public Const DTWAIN_CNTYELSALVADOR As Integer  = 503
    Public Const DTWAIN_CNTYEQGUINEA As Integer  = 1015
    Public Const DTWAIN_CNTYETHIOPIA As Integer  = 251
    Public Const DTWAIN_CNTYFALKLANDIS As Integer  = 1016
    Public Const DTWAIN_CNTYFAEROEIS As Integer  = 298
    Public Const DTWAIN_CNTYFIJIISLANDS As Integer  = 679
    Public Const DTWAIN_CNTYFINLAND As Integer  = 358
    Public Const DTWAIN_CNTYFRANCE As Integer  = 33
    Public Const DTWAIN_CNTYFRANTILLES As Integer  = 596
    Public Const DTWAIN_CNTYFRGUIANA As Integer  = 594
    Public Const DTWAIN_CNTYFRPOLYNEISA As Integer  = 689
    Public Const DTWAIN_CNTYFUTANAIS As Integer  = 1043
    Public Const DTWAIN_CNTYGABON As Integer  = 241
    Public Const DTWAIN_CNTYGAMBIA As Integer  = 220
    Public Const DTWAIN_CNTYGERMANY As Integer  = 49
    Public Const DTWAIN_CNTYGHANA As Integer  = 233
    Public Const DTWAIN_CNTYGIBRALTER As Integer  = 350
    Public Const DTWAIN_CNTYGREECE As Integer  = 30
    Public Const DTWAIN_CNTYGREENLAND As Integer  = 299
    Public Const DTWAIN_CNTYGRENADA As Integer  = 8099
    Public Const DTWAIN_CNTYGRENEDINES As Integer  = 8015
    Public Const DTWAIN_CNTYGUADELOUPE As Integer  = 590
    Public Const DTWAIN_CNTYGUAM As Integer  = 671
    Public Const DTWAIN_CNTYGUANTANAMOBAY As Integer  = 5399
    Public Const DTWAIN_CNTYGUATEMALA As Integer  = 502
    Public Const DTWAIN_CNTYGUINEA As Integer  = 224
    Public Const DTWAIN_CNTYGUINEABISSAU As Integer  = 1017
    Public Const DTWAIN_CNTYGUYANA As Integer  = 592
    Public Const DTWAIN_CNTYHAITI As Integer  = 509
    Public Const DTWAIN_CNTYHONDURAS As Integer  = 504
    Public Const DTWAIN_CNTYHONGKONG As Integer  = 852
    Public Const DTWAIN_CNTYHUNGARY As Integer  = 36
    Public Const DTWAIN_CNTYICELAND As Integer  = 354
    Public Const DTWAIN_CNTYINDIA As Integer  = 91
    Public Const DTWAIN_CNTYINDONESIA As Integer  = 62
    Public Const DTWAIN_CNTYIRAN As Integer  = 98
    Public Const DTWAIN_CNTYIRAQ As Integer  = 964
    Public Const DTWAIN_CNTYIRELAND As Integer  = 353
    Public Const DTWAIN_CNTYISRAEL As Integer  = 972
    Public Const DTWAIN_CNTYITALY As Integer  = 39
    Public Const DTWAIN_CNTYIVORYCOAST As Integer  = 225
    Public Const DTWAIN_CNTYJAMAICA As Integer  = 8010
    Public Const DTWAIN_CNTYJAPAN As Integer  = 81
    Public Const DTWAIN_CNTYJORDAN As Integer  = 962
    Public Const DTWAIN_CNTYKENYA As Integer  = 254
    Public Const DTWAIN_CNTYKIRIBATI As Integer  = 1018
    Public Const DTWAIN_CNTYKOREA As Integer  = 82
    Public Const DTWAIN_CNTYKUWAIT As Integer  = 965
    Public Const DTWAIN_CNTYLAOS As Integer  = 1019
    Public Const DTWAIN_CNTYLEBANON As Integer  = 1020
    Public Const DTWAIN_CNTYLIBERIA As Integer  = 231
    Public Const DTWAIN_CNTYLIBYA As Integer  = 218
    Public Const DTWAIN_CNTYLIECHTENSTEIN As Integer  = 41
    Public Const DTWAIN_CNTYLUXENBOURG As Integer  = 352
    Public Const DTWAIN_CNTYMACAO As Integer  = 853
    Public Const DTWAIN_CNTYMADAGASCAR As Integer  = 1021
    Public Const DTWAIN_CNTYMALAWI As Integer  = 265
    Public Const DTWAIN_CNTYMALAYSIA As Integer  = 60
    Public Const DTWAIN_CNTYMALDIVES As Integer  = 960
    Public Const DTWAIN_CNTYMALI As Integer  = 1022
    Public Const DTWAIN_CNTYMALTA As Integer  = 356
    Public Const DTWAIN_CNTYMARSHALLIS As Integer  = 692
    Public Const DTWAIN_CNTYMAURITANIA As Integer  = 1023
    Public Const DTWAIN_CNTYMAURITIUS As Integer  = 230
    Public Const DTWAIN_CNTYMEXICO As Integer  = 3
    Public Const DTWAIN_CNTYMICRONESIA As Integer  = 691
    Public Const DTWAIN_CNTYMIQUELON As Integer  = 508
    Public Const DTWAIN_CNTYMONACO As Integer  = 33
    Public Const DTWAIN_CNTYMONGOLIA As Integer  = 1024
    Public Const DTWAIN_CNTYMONTSERRAT As Integer  = 8011
    Public Const DTWAIN_CNTYMOROCCO As Integer  = 212
    Public Const DTWAIN_CNTYMOZAMBIQUE As Integer  = 1025
    Public Const DTWAIN_CNTYNAMIBIA As Integer  = 264
    Public Const DTWAIN_CNTYNAURU As Integer  = 1026
    Public Const DTWAIN_CNTYNEPAL As Integer  = 977
    Public Const DTWAIN_CNTYNETHERLANDS As Integer  = 31
    Public Const DTWAIN_CNTYNETHANTILLES As Integer  = 599
    Public Const DTWAIN_CNTYNEVIS As Integer  = 8012
    Public Const DTWAIN_CNTYNEWCALEDONIA As Integer  = 687
    Public Const DTWAIN_CNTYNEWZEALAND As Integer  = 64
    Public Const DTWAIN_CNTYNICARAGUA As Integer  = 505
    Public Const DTWAIN_CNTYNIGER As Integer  = 227
    Public Const DTWAIN_CNTYNIGERIA As Integer  = 234
    Public Const DTWAIN_CNTYNIUE As Integer  = 1027
    Public Const DTWAIN_CNTYNORFOLKI As Integer  = 1028
    Public Const DTWAIN_CNTYNORWAY As Integer  = 47
    Public Const DTWAIN_CNTYOMAN As Integer  = 968
    Public Const DTWAIN_CNTYPAKISTAN As Integer  = 92
    Public Const DTWAIN_CNTYPALAU As Integer  = 1029
    Public Const DTWAIN_CNTYPANAMA As Integer  = 507
    Public Const DTWAIN_CNTYPARAGUAY As Integer  = 595
    Public Const DTWAIN_CNTYPERU As Integer  = 51
    Public Const DTWAIN_CNTYPHILLIPPINES As Integer  = 63
    Public Const DTWAIN_CNTYPITCAIRNIS As Integer  = 1030
    Public Const DTWAIN_CNTYPNEWGUINEA As Integer  = 675
    Public Const DTWAIN_CNTYPOLAND As Integer  = 48
    Public Const DTWAIN_CNTYPORTUGAL As Integer  = 351
    Public Const DTWAIN_CNTYQATAR As Integer  = 974
    Public Const DTWAIN_CNTYREUNIONI As Integer  = 1031
    Public Const DTWAIN_CNTYROMANIA As Integer  = 40
    Public Const DTWAIN_CNTYRWANDA As Integer  = 250
    Public Const DTWAIN_CNTYSAIPAN As Integer  = 670
    Public Const DTWAIN_CNTYSANMARINO As Integer  = 39
    Public Const DTWAIN_CNTYSAOTOME As Integer  = 1033
    Public Const DTWAIN_CNTYSAUDIARABIA As Integer  = 966
    Public Const DTWAIN_CNTYSENEGAL As Integer  = 221
    Public Const DTWAIN_CNTYSEYCHELLESIS As Integer  = 1034
    Public Const DTWAIN_CNTYSIERRALEONE As Integer  = 1035
    Public Const DTWAIN_CNTYSINGAPORE As Integer  = 65
    Public Const DTWAIN_CNTYSOLOMONIS As Integer  = 1036
    Public Const DTWAIN_CNTYSOMALI As Integer  = 1037
    Public Const DTWAIN_CNTYSOUTHAFRICA As Integer  = 27
    Public Const DTWAIN_CNTYSPAIN As Integer  = 34
    Public Const DTWAIN_CNTYSRILANKA As Integer  = 94
    Public Const DTWAIN_CNTYSTHELENA As Integer  = 1032
    Public Const DTWAIN_CNTYSTKITTS As Integer  = 8013
    Public Const DTWAIN_CNTYSTLUCIA As Integer  = 8014
    Public Const DTWAIN_CNTYSTPIERRE As Integer  = 508
    Public Const DTWAIN_CNTYSTVINCENT As Integer  = 8015
    Public Const DTWAIN_CNTYSUDAN As Integer  = 1038
    Public Const DTWAIN_CNTYSURINAME As Integer  = 597
    Public Const DTWAIN_CNTYSWAZILAND As Integer  = 268
    Public Const DTWAIN_CNTYSWEDEN As Integer  = 46
    Public Const DTWAIN_CNTYSWITZERLAND As Integer  = 41
    Public Const DTWAIN_CNTYSYRIA As Integer  = 1039
    Public Const DTWAIN_CNTYTAIWAN As Integer  = 886
    Public Const DTWAIN_CNTYTANZANIA As Integer  = 255
    Public Const DTWAIN_CNTYTHAILAND As Integer  = 66
    Public Const DTWAIN_CNTYTOBAGO As Integer  = 8016
    Public Const DTWAIN_CNTYTOGO As Integer  = 228
    Public Const DTWAIN_CNTYTONGAIS As Integer  = 676
    Public Const DTWAIN_CNTYTRINIDAD As Integer  = 8016
    Public Const DTWAIN_CNTYTUNISIA As Integer  = 216
    Public Const DTWAIN_CNTYTURKEY As Integer  = 90
    Public Const DTWAIN_CNTYTURKSCAICOS As Integer  = 8017
    Public Const DTWAIN_CNTYTUVALU As Integer  = 1040
    Public Const DTWAIN_CNTYUGANDA As Integer  = 256
    Public Const DTWAIN_CNTYUSSR As Integer  = 7
    Public Const DTWAIN_CNTYUAEMIRATES As Integer  = 971
    Public Const DTWAIN_CNTYUNITEDKINGDOM As Integer  = 44
    Public Const DTWAIN_CNTYUSA As Integer  = 1
    Public Const DTWAIN_CNTYURUGUAY As Integer  = 598
    Public Const DTWAIN_CNTYVANUATU As Integer  = 1041
    Public Const DTWAIN_CNTYVATICANCITY As Integer  = 39
    Public Const DTWAIN_CNTYVENEZUELA As Integer  = 58
    Public Const DTWAIN_CNTYWAKE As Integer  = 1042
    Public Const DTWAIN_CNTYWALLISIS As Integer  = 1043
    Public Const DTWAIN_CNTYWESTERNSAHARA As Integer  = 1044
    Public Const DTWAIN_CNTYWESTERNSAMOA As Integer  = 1045
    Public Const DTWAIN_CNTYYEMEN As Integer  = 1046
    Public Const DTWAIN_CNTYYUGOSLAVIA As Integer  = 38
    Public Const DTWAIN_CNTYZAIRE As Integer  = 243
    Public Const DTWAIN_CNTYZAMBIA As Integer  = 260
    Public Const DTWAIN_CNTYZIMBABWE As Integer  = 263
    Public Const DTWAIN_LANGDANISH As Integer  = 0
    Public Const DTWAIN_LANGDUTCH As Integer  = 1
    Public Const DTWAIN_LANGINTERNATIONALENGLISH As Integer  = 2
    Public Const DTWAIN_LANGFRENCHCANADIAN As Integer  = 3
    Public Const DTWAIN_LANGFINNISH As Integer  = 4
    Public Const DTWAIN_LANGFRENCH As Integer  = 5
    Public Const DTWAIN_LANGGERMAN As Integer  = 6
    Public Const DTWAIN_LANGICELANDIC As Integer  = 7
    Public Const DTWAIN_LANGITALIAN As Integer  = 8
    Public Const DTWAIN_LANGNORWEGIAN As Integer  = 9
    Public Const DTWAIN_LANGPORTUGUESE As Integer  = 10
    Public Const DTWAIN_LANGSPANISH As Integer  = 11
    Public Const DTWAIN_LANGSWEDISH As Integer  = 12
    Public Const DTWAIN_LANGUSAENGLISH As Integer  = 13
    Public Const DTWAIN_NO_ERROR As Integer  = (0)
    Public Const DTWAIN_ERR_FIRST As Integer  = (-1000)
    Public Const DTWAIN_ERR_BAD_HANDLE As Integer  = (-1001)
    Public Const DTWAIN_ERR_BAD_SOURCE As Integer  = (-1002)
    Public Const DTWAIN_ERR_BAD_ARRAY As Integer  = (-1003)
    Public Const DTWAIN_ERR_WRONG_ARRAY_TYPE As Integer  = (-1004)
    Public Const DTWAIN_ERR_INDEX_BOUNDS As Integer  = (-1005)
    Public Const DTWAIN_ERR_OUT_OF_MEMORY As Integer  = (-1006)
    Public Const DTWAIN_ERR_NULL_WINDOW As Integer  = (-1007)
    Public Const DTWAIN_ERR_BAD_PIXTYPE As Integer  = (-1008)
    Public Const DTWAIN_ERR_BAD_CONTAINER As Integer  = (-1009)
    Public Const DTWAIN_ERR_NO_SESSION As Integer  = (-1010)
    Public Const DTWAIN_ERR_BAD_ACQUIRE_NUM As Integer  = (-1011)
    Public Const DTWAIN_ERR_BAD_CAP As Integer  = (-1012)
    Public Const DTWAIN_ERR_CAP_NO_SUPPORT As Integer  = (-1013)
    Public Const DTWAIN_ERR_TWAIN As Integer  = (-1014)
    Public Const DTWAIN_ERR_HOOK_FAILED As Integer  = (-1015)
    Public Const DTWAIN_ERR_BAD_FILENAME As Integer  = (-1016)
    Public Const DTWAIN_ERR_EMPTY_ARRAY As Integer  = (-1017)
    Public Const DTWAIN_ERR_FILE_FORMAT As Integer  = (-1018)
    Public Const DTWAIN_ERR_BAD_DIB_PAGE As Integer  = (-1019)
    Public Const DTWAIN_ERR_SOURCE_ACQUIRING As Integer  = (-1020)
    Public Const DTWAIN_ERR_INVALID_PARAM As Integer  = (-1021)
    Public Const DTWAIN_ERR_INVALID_RANGE As Integer  = (-1022)
    Public Const DTWAIN_ERR_UI_ERROR As Integer  = (-1023)
    Public Const DTWAIN_ERR_BAD_UNIT As Integer  = (-1024)
    Public Const DTWAIN_ERR_LANGDLL_NOT_FOUND As Integer  = (-1025)
    Public Const DTWAIN_ERR_SOURCE_NOT_OPEN As Integer  = (-1026)
    Public Const DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED As Integer  = (-1027)
    Public Const DTWAIN_ERR_UIONLY_NOT_SUPPORTED As Integer  = (-1028)
    Public Const DTWAIN_ERR_UI_ALREADY_OPENED As Integer  = (-1029)
    Public Const DTWAIN_ERR_CAPSET_NOSUPPORT As Integer  = (-1030)
    Public Const DTWAIN_ERR_NO_FILE_XFER As Integer  = (-1031)
    Public Const DTWAIN_ERR_INVALID_BITDEPTH As Integer  = (-1032)
    Public Const DTWAIN_ERR_NO_CAPS_DEFINED As Integer  = (-1033)
    Public Const DTWAIN_ERR_TILES_NOT_SUPPORTED As Integer  = (-1034)
    Public Const DTWAIN_ERR_INVALID_DTWAIN_FRAME As Integer  = (-1035)
    Public Const DTWAIN_ERR_LIMITED_VERSION As Integer  = (-1036)
    Public Const DTWAIN_ERR_NO_FEEDER As Integer  = (-1037)
    Public Const DTWAIN_ERR_NO_FEEDER_QUERY As Integer  = (-1038)
    Public Const DTWAIN_ERR_EXCEPTION_ERROR As Integer  = (-1039)
    Public Const DTWAIN_ERR_INVALID_STATE As Integer  = (-1040)
    Public Const DTWAIN_ERR_UNSUPPORTED_EXTINFO As Integer  = (-1041)
    Public Const DTWAIN_ERR_DLLRESOURCE_NOTFOUND As Integer  = (-1042)
    Public Const DTWAIN_ERR_NOT_INITIALIZED As Integer  = (-1043)
    Public Const DTWAIN_ERR_NO_SOURCES As Integer  = (-1044)
    Public Const DTWAIN_ERR_TWAIN_NOT_INSTALLED As Integer  = (-1045)
    Public Const DTWAIN_ERR_WRONG_THREAD As Integer  = (-1046)
    Public Const DTWAIN_ERR_BAD_CAPTYPE As Integer  = (-1047)
    Public Const DTWAIN_ERR_UNKNOWN_CAPDATATYPE As Integer  = (-1048)
    Public Const DTWAIN_ERR_DEMO_NOFILETYPE As Integer  = (-1049)
    Public Const DTWAIN_ERR_LAST_1 As Integer  = DTWAIN_ERR_DEMO_NOFILETYPE
    Public Const TWAIN_ERR_LOW_MEMORY As Integer  = (-1100)
    Public Const TWAIN_ERR_FALSE_ALARM As Integer  = (-1101)
    Public Const TWAIN_ERR_BUMMER As Integer  = (-1102)
    Public Const TWAIN_ERR_NODATASOURCE As Integer  = (-1103)
    Public Const TWAIN_ERR_MAXCONNECTIONS As Integer  = (-1104)
    Public Const TWAIN_ERR_OPERATIONERROR As Integer  = (-1105)
    Public Const TWAIN_ERR_BADCAPABILITY As Integer  = (-1106)
    Public Const TWAIN_ERR_BADVALUE As Integer  = (-1107)
    Public Const TWAIN_ERR_BADPROTOCOL As Integer  = (-1108)
    Public Const TWAIN_ERR_SEQUENCEERROR As Integer  = (-1109)
    Public Const TWAIN_ERR_BADDESTINATION As Integer  = (-1110)
    Public Const TWAIN_ERR_CAPNOTSUPPORTED As Integer  = (-1111)
    Public Const TWAIN_ERR_CAPBADOPERATION As Integer  = (-1112)
    Public Const TWAIN_ERR_CAPSEQUENCEERROR As Integer  = (-1113)
    Public Const TWAIN_ERR_FILEPROTECTEDERROR As Integer  = (-1114)
    Public Const TWAIN_ERR_FILEEXISTERROR As Integer  = (-1115)
    Public Const TWAIN_ERR_FILENOTFOUND As Integer  = (-1116)
    Public Const TWAIN_ERR_DIRNOTEMPTY As Integer  = (-1117)
    Public Const TWAIN_ERR_FEEDERJAMMED As Integer  = (-1118)
    Public Const TWAIN_ERR_FEEDERMULTPAGES As Integer  = (-1119)
    Public Const TWAIN_ERR_FEEDERWRITEERROR As Integer  = (-1120)
    Public Const TWAIN_ERR_DEVICEOFFLINE As Integer  = (-1121)
    Public Const TWAIN_ERR_NULL_CONTAINER As Integer  = (-1122)
    Public Const TWAIN_ERR_INTERLOCK As Integer  = (-1123)
    Public Const TWAIN_ERR_DAMAGEDCORNER As Integer  = (-1124)
    Public Const TWAIN_ERR_FOCUSERROR As Integer  = (-1125)
    Public Const TWAIN_ERR_DOCTOOLIGHT As Integer  = (-1126)
    Public Const TWAIN_ERR_DOCTOODARK As Integer  = (-1127)
    Public Const TWAIN_ERR_NOMEDIA As Integer  = (-1128)
    Public Const DTWAIN_ERR_FILEXFERSTART As Integer  = (-2000)
    Public Const DTWAIN_ERR_MEM As Integer  = (-2001)
    Public Const DTWAIN_ERR_FILEOPEN As Integer  = (-2002)
    Public Const DTWAIN_ERR_FILEREAD As Integer  = (-2003)
    Public Const DTWAIN_ERR_FILEWRITE As Integer  = (-2004)
    Public Const DTWAIN_ERR_BADPARAM As Integer  = (-2005)
    Public Const DTWAIN_ERR_INVALIDBMP As Integer  = (-2006)
    Public Const DTWAIN_ERR_BMPRLE As Integer  = (-2007)
    Public Const DTWAIN_ERR_RESERVED1 As Integer  = (-2008)
    Public Const DTWAIN_ERR_INVALIDJPG As Integer  = (-2009)
    Public Const DTWAIN_ERR_DC As Integer  = (-2010)
    Public Const DTWAIN_ERR_DIB As Integer  = (-2011)
    Public Const DTWAIN_ERR_RESERVED2 As Integer  = (-2012)
    Public Const DTWAIN_ERR_NORESOURCE As Integer  = (-2013)
    Public Const DTWAIN_ERR_CALLBACKCANCEL As Integer  = (-2014)
    Public Const DTWAIN_ERR_INVALIDPNG As Integer  = (-2015)
    Public Const DTWAIN_ERR_PNGCREATE As Integer  = (-2016)
    Public Const DTWAIN_ERR_INTERNAL As Integer  = (-2017)
    Public Const DTWAIN_ERR_FONT As Integer  = (-2018)
    Public Const DTWAIN_ERR_INTTIFF As Integer  = (-2019)
    Public Const DTWAIN_ERR_INVALIDTIFF As Integer  = (-2020)
    Public Const DTWAIN_ERR_NOTIFFLZW As Integer  = (-2021)
    Public Const DTWAIN_ERR_INVALIDPCX As Integer  = (-2022)
    Public Const DTWAIN_ERR_CREATEBMP As Integer  = (-2023)
    Public Const DTWAIN_ERR_NOLINES As Integer  = (-2024)
    Public Const DTWAIN_ERR_GETDIB As Integer  = (-2025)
    Public Const DTWAIN_ERR_NODEVOP As Integer  = (-2026)
    Public Const DTWAIN_ERR_INVALIDWMF As Integer  = (-2027)
    Public Const DTWAIN_ERR_DEPTHMISMATCH As Integer  = (-2028)
    Public Const DTWAIN_ERR_BITBLT As Integer  = (-2029)
    Public Const DTWAIN_ERR_BUFTOOSMALL As Integer  = (-2030)
    Public Const DTWAIN_ERR_TOOMANYCOLORS As Integer  = (-2031)
    Public Const DTWAIN_ERR_INVALIDTGA As Integer  = (-2032)
    Public Const DTWAIN_ERR_NOTGATHUMBNAIL As Integer  = (-2033)
    Public Const DTWAIN_ERR_RESERVED3 As Integer  = (-2034)
    Public Const DTWAIN_ERR_CREATEDIB As Integer  = (-2035)
    Public Const DTWAIN_ERR_NOLZW As Integer  = (-2036)
    Public Const DTWAIN_ERR_SELECTOBJ As Integer  = (-2037)
    Public Const DTWAIN_ERR_BADMANAGER As Integer  = (-2038)
    Public Const DTWAIN_ERR_OBSOLETE As Integer  = (-2039)
    Public Const DTWAIN_ERR_CREATEDIBSECTION As Integer  = (-2040)
    Public Const DTWAIN_ERR_SETWINMETAFILEBITS As Integer  = (-2041)
    Public Const DTWAIN_ERR_GETWINMETAFILEBITS As Integer  = (-2042)
    Public Const DTWAIN_ERR_PAXPWD As Integer  = (-2043)
    Public Const DTWAIN_ERR_INVALIDPAX As Integer  = (-2044)
    Public Const DTWAIN_ERR_NOSUPPORT As Integer  = (-2045)
    Public Const DTWAIN_ERR_INVALIDPSD As Integer  = (-2046)
    Public Const DTWAIN_ERR_PSDNOTSUPPORTED As Integer  = (-2047)
    Public Const DTWAIN_ERR_DECRYPT As Integer  = (-2048)
    Public Const DTWAIN_ERR_ENCRYPT As Integer  = (-2049)
    Public Const DTWAIN_ERR_COMPRESSION As Integer  = (-2050)
    Public Const DTWAIN_ERR_DECOMPRESSION As Integer  = (-2051)
    Public Const DTWAIN_ERR_INVALIDTLA As Integer  = (-2052)
    Public Const DTWAIN_ERR_INVALIDWBMP As Integer  = (-2053)
    Public Const DTWAIN_ERR_NOTIFFTAG As Integer  = (-2054)
    Public Const DTWAIN_ERR_NOLOCALSTORAGE As Integer  = (-2055)
    Public Const DTWAIN_ERR_INVALIDEXIF As Integer  = (-2056)
    Public Const DTWAIN_ERR_NOEXIFSTRING As Integer  = (-2057)
    Public Const DTWAIN_ERR_TIFFDLL32NOTFOUND As Integer  = (-2058)
    Public Const DTWAIN_ERR_TIFFDLL16NOTFOUND As Integer  = (-2059)
    Public Const DTWAIN_ERR_PNGDLL16NOTFOUND As Integer  = (-2060)
    Public Const DTWAIN_ERR_JPEGDLL16NOTFOUND As Integer  = (-2061)
    Public Const DTWAIN_ERR_BADBITSPERPIXEL As Integer  = (-2062)
    Public Const DTWAIN_ERR_TIFFDLL32INVALIDVER As Integer  = (-2063)
    Public Const DTWAIN_ERR_PDFDLL32NOTFOUND As Integer  = (-2064)
    Public Const DTWAIN_ERR_PDFDLL32INVALIDVER As Integer  = (-2065)
    Public Const DTWAIN_ERR_JPEGDLL32NOTFOUND As Integer  = (-2066)
    Public Const DTWAIN_ERR_JPEGDLL32INVALIDVER As Integer  = (-2067)
    Public Const DTWAIN_ERR_PNGDLL32NOTFOUND As Integer  = (-2068)
    Public Const DTWAIN_ERR_PNGDLL32INVALIDVER As Integer  = (-2069)
    Public Const DTWAIN_ERR_J2KDLL32NOTFOUND As Integer  = (-2070)
    Public Const DTWAIN_ERR_J2KDLL32INVALIDVER As Integer  = (-2071)
    Public Const DTWAIN_ERR_MANDUPLEX_UNAVAILABLE As Integer  = (-2072)
    Public Const DTWAIN_ERR_TIMEOUT As Integer  = (-2073)
    Public Const DTWAIN_ERR_INVALIDICONFORMAT As Integer  = (-2074)
    Public Const DTWAIN_ERR_TWAIN32DSMNOTFOUND As Integer  = (-2075)
    Public Const DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND As Integer  = (-2076)
    Public Const DTWAIN_TWAINSAVE_OK As Integer  = (0)
    Public Const DTWAIN_ERR_TS_FIRST As Integer  = (-2080)
    Public Const DTWAIN_ERR_TS_NOFILENAME As Integer  = (-2081)
    Public Const DTWAIN_ERR_TS_NOTWAINSYS As Integer  = (-2082)
    Public Const DTWAIN_ERR_TS_DEVICEFAILURE As Integer  = (-2083)
    Public Const DTWAIN_ERR_TS_FILESAVEERROR As Integer  = (-2084)
    Public Const DTWAIN_ERR_TS_COMMANDILLEGAL As Integer  = (-2085)
    Public Const DTWAIN_ERR_TS_CANCELLED As Integer  = (-2086)
    Public Const DTWAIN_ERR_TS_ACQUISITIONERROR As Integer  = (-2087)
    Public Const DTWAIN_ERR_TS_INVALIDCOLORSPACE As Integer  = (-2088)
    Public Const DTWAIN_ERR_TS_PDFNOTSUPPORTED As Integer  = (-2089)
    Public Const DTWAIN_ERR_TS_NOTAVAILABLE As Integer  = (-2090)
    Public Const DTWAIN_ERR_OCR_FIRST As Integer  = (-2100)
    Public Const DTWAIN_ERR_OCR_INVALIDPAGENUM As Integer  = (-2101)
    Public Const DTWAIN_ERR_OCR_INVALIDENGINE As Integer  = (-2102)
    Public Const DTWAIN_ERR_OCR_NOTACTIVE As Integer  = (-2103)
    Public Const DTWAIN_ERR_OCR_INVALIDFILETYPE As Integer  = (-2104)
    Public Const DTWAIN_ERR_OCR_INVALIDPIXELTYPE As Integer  = (-2105)
    Public Const DTWAIN_ERR_OCR_INVALIDBITDEPTH As Integer  = (-2106)
    Public Const DTWAIN_ERR_OCR_RECOGNITIONERROR As Integer  = (-2107)
    Public Const DTWAIN_ERR_OCR_LAST As Integer  = (-2108)
    Public Const DTWAIN_ERR_LAST As Integer  = DTWAIN_ERR_OCR_LAST
    Public Const DTWAIN_DE_CHKAUTOCAPTURE As Integer  = 1
    Public Const DTWAIN_DE_CHKBATTERY As Integer  = 2
    Public Const DTWAIN_DE_CHKDEVICEONLINE As Integer  = 4
    Public Const DTWAIN_DE_CHKFLASH As Integer  = 8
    Public Const DTWAIN_DE_CHKPOWERSUPPLY As Integer  = 16
    Public Const DTWAIN_DE_CHKRESOLUTION As Integer  = 32
    Public Const DTWAIN_DE_DEVICEADDED As Integer  = 64
    Public Const DTWAIN_DE_DEVICEOFFLINE As Integer  = 128
    Public Const DTWAIN_DE_DEVICEREADY As Integer  = 256
    Public Const DTWAIN_DE_DEVICEREMOVED As Integer  = 512
    Public Const DTWAIN_DE_IMAGECAPTURED As Integer  = 1024
    Public Const DTWAIN_DE_IMAGEDELETED As Integer  = 2048
    Public Const DTWAIN_DE_PAPERDOUBLEFEED As Integer  = 4096
    Public Const DTWAIN_DE_PAPERJAM As Integer  = 8192
    Public Const DTWAIN_DE_LAMPFAILURE As Integer  = 16384
    Public Const DTWAIN_DE_POWERSAVE As Integer  = 32768
    Public Const DTWAIN_DE_POWERSAVENOTIFY As Integer  = 65536
    Public Const DTWAIN_DE_CUSTOMEVENTS As Integer  = &H8000
    Public Const DTWAIN_GETDE_EVENT As Integer  = 0
    Public Const DTWAIN_GETDE_DEVNAME As Integer  = 1
    Public Const DTWAIN_GETDE_BATTERYMINUTES As Integer  = 2
    Public Const DTWAIN_GETDE_BATTERYPCT As Integer  = 3
    Public Const DTWAIN_GETDE_XRESOLUTION As Integer  = 4
    Public Const DTWAIN_GETDE_YRESOLUTION As Integer  = 5
    Public Const DTWAIN_GETDE_FLASHUSED As Integer  = 6
    Public Const DTWAIN_GETDE_AUTOCAPTURE As Integer  = 7
    Public Const DTWAIN_GETDE_TIMEBEFORECAPTURE As Integer  = 8
    Public Const DTWAIN_GETDE_TIMEBETWEENCAPTURES As Integer  = 9
    Public Const DTWAIN_GETDE_POWERSUPPLY As Integer  = 10
    Public Const DTWAIN_IMPRINTERTOPBEFORE As Integer  = 1
    Public Const DTWAIN_IMPRINTERTOPAFTER As Integer  = 2
    Public Const DTWAIN_IMPRINTERBOTTOMBEFORE As Integer  = 4
    Public Const DTWAIN_IMPRINTERBOTTOMAFTER As Integer  = 8
    Public Const DTWAIN_ENDORSERTOPBEFORE As Integer  = 16
    Public Const DTWAIN_ENDORSERTOPAFTER As Integer  = 32
    Public Const DTWAIN_ENDORSERBOTTOMBEFORE As Integer  = 64
    Public Const DTWAIN_ENDORSERBOTTOMAFTER As Integer  = 128
    Public Const DTWAIN_PM_SINGLESTRING As Integer  = 0
    Public Const DTWAIN_PM_MULTISTRING As Integer  = 1
    Public Const DTWAIN_PM_COMPOUNDSTRING As Integer  = 2
    Public Const DTWAIN_TWTY_INT8 As Integer  = &H0000
    Public Const DTWAIN_TWTY_INT16 As Integer  = &H0001
    Public Const DTWAIN_TWTY_INT32 As Integer  = &H0002
    Public Const DTWAIN_TWTY_UINT8 As Integer  = &H0003
    Public Const DTWAIN_TWTY_UINT16 As Integer  = &H0004
    Public Const DTWAIN_TWTY_UINT32 As Integer  = &H0005
    Public Const DTWAIN_TWTY_BOOL As Integer  = &H0006
    Public Const DTWAIN_TWTY_FIX32 As Integer  = &H0007
    Public Const DTWAIN_TWTY_FRAME As Integer  = &H0008
    Public Const DTWAIN_TWTY_STR32 As Integer  = &H0009
    Public Const DTWAIN_TWTY_STR64 As Integer  = &H000A
    Public Const DTWAIN_TWTY_STR128 As Integer  = &H000B
    Public Const DTWAIN_TWTY_STR255 As Integer  = &H000C
    Public Const DTWAIN_TWTY_STR1024 As Integer  = &H000D
    Public Const DTWAIN_TWTY_UNI512 As Integer  = &H000E
    Public Const DTWAIN_EI_BARCODEX As Integer  = &H1200
    Public Const DTWAIN_EI_BARCODEY As Integer  = &H1201
    Public Const DTWAIN_EI_BARCODETEXT As Integer  = &H1202
    Public Const DTWAIN_EI_BARCODETYPE As Integer  = &H1203
    Public Const DTWAIN_EI_DESHADETOP As Integer  = &H1204
    Public Const DTWAIN_EI_DESHADELEFT As Integer  = &H1205
    Public Const DTWAIN_EI_DESHADEHEIGHT As Integer  = &H1206
    Public Const DTWAIN_EI_DESHADEWIDTH As Integer  = &H1207
    Public Const DTWAIN_EI_DESHADESIZE As Integer  = &H1208
    Public Const DTWAIN_EI_SPECKLESREMOVED As Integer  = &H1209
    Public Const DTWAIN_EI_HORZLINEXCOORD As Integer  = &H120A
    Public Const DTWAIN_EI_HORZLINEYCOORD As Integer  = &H120B
    Public Const DTWAIN_EI_HORZLINELENGTH As Integer  = &H120C
    Public Const DTWAIN_EI_HORZLINETHICKNESS As Integer  = &H120D
    Public Const DTWAIN_EI_VERTLINEXCOORD As Integer  = &H120E
    Public Const DTWAIN_EI_VERTLINEYCOORD As Integer  = &H120F
    Public Const DTWAIN_EI_VERTLINELENGTH As Integer  = &H1210
    Public Const DTWAIN_EI_VERTLINETHICKNESS As Integer  = &H1211
    Public Const DTWAIN_EI_PATCHCODE As Integer  = &H1212
    Public Const DTWAIN_EI_ENDORSEDTEXT As Integer  = &H1213
    Public Const DTWAIN_EI_FORMCONFIDENCE As Integer  = &H1214
    Public Const DTWAIN_EI_FORMTEMPLATEMATCH As Integer  = &H1215
    Public Const DTWAIN_EI_FORMTEMPLATEPAGEMATCH As Integer  = &H1216
    Public Const DTWAIN_EI_FORMHORZDOCOFFSET As Integer  = &H1217
    Public Const DTWAIN_EI_FORMVERTDOCOFFSET As Integer  = &H1218
    Public Const DTWAIN_EI_BARCODECOUNT As Integer  = &H1219
    Public Const DTWAIN_EI_BARCODECONFIDENCE As Integer  = &H121A
    Public Const DTWAIN_EI_BARCODEROTATION As Integer  = &H121B
    Public Const DTWAIN_EI_BARCODETEXTLENGTH As Integer  = &H121C
    Public Const DTWAIN_EI_DESHADECOUNT As Integer  = &H121D
    Public Const DTWAIN_EI_DESHADEBLACKCOUNTOLD As Integer  = &H121E
    Public Const DTWAIN_EI_DESHADEBLACKCOUNTNEW As Integer  = &H121F
    Public Const DTWAIN_EI_DESHADEBLACKRLMIN As Integer  = &H1220
    Public Const DTWAIN_EI_DESHADEBLACKRLMAX As Integer  = &H1221
    Public Const DTWAIN_EI_DESHADEWHITECOUNTOLD As Integer  = &H1222
    Public Const DTWAIN_EI_DESHADEWHITECOUNTNEW As Integer  = &H1223
    Public Const DTWAIN_EI_DESHADEWHITERLMIN As Integer  = &H1224
    Public Const DTWAIN_EI_DESHADEWHITERLAVE As Integer  = &H1225
    Public Const DTWAIN_EI_DESHADEWHITERLMAX As Integer  = &H1226
    Public Const DTWAIN_EI_BLACKSPECKLESREMOVED As Integer  = &H1227
    Public Const DTWAIN_EI_WHITESPECKLESREMOVED As Integer  = &H1228
    Public Const DTWAIN_EI_HORZLINECOUNT As Integer  = &H1229
    Public Const DTWAIN_EI_VERTLINECOUNT As Integer  = &H122A
    Public Const DTWAIN_EI_DESKEWSTATUS As Integer  = &H122B
    Public Const DTWAIN_EI_SKEWORIGINALANGLE As Integer  = &H122C
    Public Const DTWAIN_EI_SKEWFINALANGLE As Integer  = &H122D
    Public Const DTWAIN_EI_SKEWCONFIDENCE As Integer  = &H122E
    Public Const DTWAIN_EI_SKEWWINDOWX1 As Integer  = &H122F
    Public Const DTWAIN_EI_SKEWWINDOWY1 As Integer  = &H1230
    Public Const DTWAIN_EI_SKEWWINDOWX2 As Integer  = &H1231
    Public Const DTWAIN_EI_SKEWWINDOWY2 As Integer  = &H1232
    Public Const DTWAIN_EI_SKEWWINDOWX3 As Integer  = &H1233
    Public Const DTWAIN_EI_SKEWWINDOWY3 As Integer  = &H1234
    Public Const DTWAIN_EI_SKEWWINDOWX4 As Integer  = &H1235
    Public Const DTWAIN_EI_SKEWWINDOWY4 As Integer  = &H1236
    Public Const DTWAIN_EI_BOOKNAME As Integer  = &H1238
    Public Const DTWAIN_EI_CHAPTERNUMBER As Integer  = &H1239
    Public Const DTWAIN_EI_DOCUMENTNUMBER As Integer  = &H123A
    Public Const DTWAIN_EI_PAGENUMBER As Integer  = &H123B
    Public Const DTWAIN_EI_CAMERA As Integer  = &H123C
    Public Const DTWAIN_EI_FRAMENUMBER As Integer  = &H123D
    Public Const DTWAIN_EI_FRAME As Integer  = &H123E
    Public Const DTWAIN_EI_PIXELFLAVOR As Integer  = &H123F
    Public Const DTWAIN_EI_ICCPROFILE As Integer  = &H1240
    Public Const DTWAIN_EI_LASTSEGMENT As Integer  = &H1241
    Public Const DTWAIN_EI_SEGMENTNUMBER As Integer  = &H1242
    Public Const DTWAIN_EI_MAGDATA As Integer  = &H1243
    Public Const DTWAIN_EI_MAGTYPE As Integer  = &H1244
    Public Const DTWAIN_EI_PAGESIDE As Integer  = &H1245
    Public Const DTWAIN_EI_FILESYSTEMSOURCE As Integer  = &H1246
    Public Const DTWAIN_EI_IMAGEMERGED As Integer  = &H1247
    Public Const DTWAIN_EI_MAGDATALENGTH As Integer  = &H1248
    Public Const DTWAIN_EI_PAPERCOUNT As Integer  = &H1249
    Public Const DTWAIN_EI_PRINTERTEXT As Integer  = &H124A
    Public Const DTWAIN_LOG_DECODE_SOURCE As Integer  = 1
    Public Const DTWAIN_LOG_DECODE_DEST As Integer  = 2
    Public Const DTWAIN_LOG_DECODE_TWMEMREF As Integer  = 4
    Public Const DTWAIN_LOG_DECODE_TWEVENT As Integer  = 8
    Public Const DTWAIN_LOG_USEFILE As Integer  = 16
    Public Const DTWAIN_LOG_CALLSTACK As Integer  = 32
    Public Const DTWAIN_LOG_USEWINDOW As Integer  = 64
    Public Const DTWAIN_LOG_SHOWEXCEPTIONS As Integer  = 128
    Public Const DTWAIN_LOG_ERRORMSGBOX As Integer  = 256
    Public Const DTWAIN_LOG_INITFAILURE As Integer  = 512
    Public Const DTWAIN_LOG_USEBUFFER As Integer  = 1024
    Public Const DTWAIN_LOG_FILEAPPEND As Integer  = 2048
    Public Const DTWAIN_LOG_DECODE_BITMAP As Integer  = 4096
    Public Const DTWAIN_LOG_NOCALLBACK As Integer  = 8192
    Public Const DTWAIN_LOG_WRITE As Integer  = 16384
    Public Const DTWAIN_LOG_USECRLF As Integer  = 32768
    Public Const DTWAIN_LOG_ALL As Integer  = &HFFFFF7FF
    Public Const DTWAIN_LOG_ALL_APPEND As Integer  = &HFFFFFFFF
    Public Const DTWAINGCD_RETURNHANDLE As Integer  = 1
    Public Const DTWAINGCD_COPYDATA As Integer  = 2
    Public Const DTWAIN_BYPOSITION As Integer  = 0
    Public Const DTWAIN_BYID As Integer  = 1
    Public Const DTWAINSCD_USEHANDLE As Integer  = 1
    Public Const DTWAINSCD_USEDATA As Integer  = 2
    Public Const DTWAIN_PAGEFAIL_RETRY As Integer  = 1
    Public Const DTWAIN_PAGEFAIL_TERMINATE As Integer  = 2
    Public Const DTWAIN_MAXRETRY_ATTEMPTS As Integer  = 3
    Public Const DTWAIN_RETRY_FOREVER As Integer  = (-1)
    Public Const DTWAIN_PDF_NOSCALING As Integer  = 128
    Public Const DTWAIN_PDF_FITPAGE As Integer  = 256
    Public Const DTWAIN_PDF_VARIABLEPAGESIZE As Integer  = 512
    Public Const DTWAIN_PDF_CUSTOMSIZE As Integer  = 1024
    Public Const DTWAIN_PDF_USECOMPRESSION As Integer  = 2048
    Public Const DTWAIN_PDF_CUSTOMSCALE As Integer  = 4096
    Public Const DTWAIN_PDF_PIXELSPERMETERSIZE As Integer  = 8192
    Public Const DTWAIN_PDF_ALLOWPRINTING As Integer  = 2052
    Public Const DTWAIN_PDF_ALLOWMOD As Integer  = 8
    Public Const DTWAIN_PDF_ALLOWCOPY As Integer  = 16
    Public Const DTWAIN_PDF_ALLOWMODANNOTATIONS As Integer  = 32
    Public Const DTWAIN_PDF_ALLOWFILLIN As Integer  = 256
    Public Const DTWAIN_PDF_ALLOWEXTRACTION As Integer  = 512
    Public Const DTWAIN_PDF_ALLOWASSEMBLY As Integer  = 1024
    Public Const DTWAIN_PDF_ALLOWDEGRADEDPRINTING As Integer  = 4
    Public Const DTWAIN_PDF_PORTRAIT As Integer  = 0
    Public Const DTWAIN_PDF_LANDSCAPE As Integer  = 1
    Public Const DTWAIN_PS_REGULAR As Integer  = 0
    Public Const DTWAIN_PS_ENCAPSULATED As Integer  = 1
    Public Const DTWAIN_BP_AUTODISCARD_NONE As Integer  = 0
    Public Const DTWAIN_BP_AUTODISCARD_IMMEDIATE As Integer  = 1
    Public Const DTWAIN_BP_AUTODISCARD_AFTERPROCESS As Integer  = 2
    Public Const DTWAIN_BP_AUTODISCARD_ANY As Integer  = &HFFFF
    Public Const DTWAIN_LP_REFLECTIVE As Integer  = 0
    Public Const DTWAIN_LP_TRANSMISSIVE As Integer  = 1
    Public Const DTWAIN_LS_RED As Integer  = 0
    Public Const DTWAIN_LS_GREEN As Integer  = 1
    Public Const DTWAIN_LS_BLUE As Integer  = 2
    Public Const DTWAIN_LS_NONE As Integer  = 3
    Public Const DTWAIN_LS_WHITE As Integer  = 4
    Public Const DTWAIN_LS_UV As Integer  = 5
    Public Const DTWAIN_LS_IR As Integer  = 6
    Public Const DTWAIN_DLG_SORTNAMES As Integer  = 1
    Public Const DTWAIN_DLG_CENTER As Integer  = 2
    Public Const DTWAIN_DLG_CENTER_SCREEN As Integer  = 4
    Public Const DTWAIN_DLG_USETEMPLATE As Integer  = 8
    Public Const DTWAIN_DLG_CLEAR_PARAMS As Integer  = 16
    Public Const DTWAIN_DLG_HORIZONTALSCROLL As Integer  = 32
    Public Const DTWAIN_RES_ENGLISH As Integer  = 0
    Public Const DTWAIN_RES_FRENCH As Integer  = 1
    Public Const DTWAIN_RES_SPANISH As Integer  = 2
    Public Const DTWAIN_RES_DUTCH As Integer  = 3
    Public Const DTWAIN_RES_GERMAN As Integer  = 4
    Public Const DTWAIN_RES_ITALIAN As Integer  = 5
    Public Const DTWAIN_AL_ALARM As Integer  = 0
    Public Const DTWAIN_AL_FEEDERERROR As Integer  = 1
    Public Const DTWAIN_AL_FEEDERWARNING As Integer  = 2
    Public Const DTWAIN_AL_BARCODE As Integer  = 3
    Public Const DTWAIN_AL_DOUBLEFEED As Integer  = 4
    Public Const DTWAIN_AL_JAM As Integer  = 5
    Public Const DTWAIN_AL_PATCHCODE As Integer  = 6
    Public Const DTWAIN_AL_POWER As Integer  = 7
    Public Const DTWAIN_AL_SKEW As Integer  = 8
    Public Const DTWAIN_FT_CAMERA As Integer  = 0
    Public Const DTWAIN_FT_CAMERATOP As Integer  = 1
    Public Const DTWAIN_FT_CAMERABOTTOM As Integer  = 2
    Public Const DTWAIN_FT_CAMERAPREVIEW As Integer  = 3
    Public Const DTWAIN_FT_DOMAIN As Integer  = 4
    Public Const DTWAIN_FT_HOST As Integer  = 5
    Public Const DTWAIN_FT_DIRECTORY As Integer  = 6
    Public Const DTWAIN_FT_IMAGE As Integer  = 7
    Public Const DTWAIN_FT_UNKNOWN As Integer  = 8
    Public Const DTWAIN_NF_NONE As Integer  = 0
    Public Const DTWAIN_NF_AUTO As Integer  = 1
    Public Const DTWAIN_NF_LONEPIXEL As Integer  = 2
    Public Const DTWAIN_NF_MAJORITYRULE As Integer  = 3
    Public Const DTWAIN_CB_AUTO As Integer  = 0
    Public Const DTWAIN_CB_CLEAR As Integer  = 1
    Public Const DTWAIN_CB_NOCLEAR As Integer  = 2
    Public Const DTWAIN_FA_NONE As Integer  = 0
    Public Const DTWAIN_FA_LEFT As Integer  = 1
    Public Const DTWAIN_FA_CENTER As Integer  = 2
    Public Const DTWAIN_FA_RIGHT As Integer  = 3
    Public Const DTWAIN_PF_CHOCOLATE As Integer  = 0
    Public Const DTWAIN_PF_VANILLA As Integer  = 1
    Public Const DTWAIN_FO_FIRSTPAGEFIRST As Integer  = 0
    Public Const DTWAIN_FO_LASTPAGEFIRST As Integer  = 1
    Public Const DTWAIN_INCREMENT_STATIC As Integer  = 0
    Public Const DTWAIN_INCREMENT_DYNAMIC As Integer  = 1
    Public Const DTWAIN_INCREMENT_DEFAULT As Integer  = -1
    Public Const DTWAIN_MANDUP_FACEUPTOPPAGE As Integer  = 0
    Public Const DTWAIN_MANDUP_FACEUPBOTTOMPAGE As Integer  = 1
    Public Const DTWAIN_MANDUP_FACEDOWNTOPPAGE As Integer  = 2
    Public Const DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE As Integer  = 3
    Public Const DTWAIN_FILESAVE_DEFAULT As Integer  = 0
    Public Const DTWAIN_FILESAVE_UICLOSE As Integer  = 1
    Public Const DTWAIN_FILESAVE_SOURCECLOSE As Integer  = 2
    Public Const DTWAIN_FILESAVE_ENDACQUIRE As Integer  = 3
    Public Const DTWAIN_FILESAVE_MANUALSAVE As Integer  = 4
    Public Const DTWAIN_FILESAVE_SAVEINCOMPLETE As Integer  = 128
    Public Const DTWAIN_MANDUP_SCANOK As Integer  = 1
    Public Const DTWAIN_MANDUP_SIDE1RESCAN As Integer  = 2
    Public Const DTWAIN_MANDUP_SIDE2RESCAN As Integer  = 3
    Public Const DTWAIN_MANDUP_RESCANALL As Integer  = 4
    Public Const DTWAIN_MANDUP_PAGEMISSING As Integer  = 5
    Public Const DTWAIN_DEMODLL_VERSION As Integer  = &H00000001
    Public Const DTWAIN_UNLICENSED_VERSION As Integer  = &H00000002
    Public Const DTWAIN_COMPANY_VERSION As Integer  = &H00000004
    Public Const DTWAIN_GENERAL_VERSION As Integer  = &H00000008
    Public Const DTWAIN_DEVELOP_VERSION As Integer  = &H00000010
    Public Const DTWAIN_JAVA_VERSION As Integer  = &H00000020
    Public Const DTWAIN_TOOLKIT_VERSION As Integer  = &H00000040
    Public Const DTWAIN_LIMITEDDLL_VERSION As Integer  = &H00000080
    Public Const DTWAIN_STATICLIB_VERSION As Integer  = &H00000100
    Public Const DTWAIN_STATICLIB_STDCALL_VERSION As Integer  = &H00000200
    Public Const DTWAIN_PDF_VERSION As Integer  = &H00010000
    Public Const DTWAIN_TWAINSAVE_VERSION As Integer  = &H00020000
    Public Const DTWAIN_OCR_VERSION As Integer  = &H00040000
    Public Const DTWAIN_BARCODE_VERSION As Integer  = &H00080000
    Public Const DTWAIN_ACTIVEX_VERSION As Integer  = &H00100000
    Public Const DTWAIN_32BIT_VERSION As Integer  = &H00200000
    Public Const DTWAIN_64BIT_VERSION As Integer  = &H00400000
    Public Const DTWAIN_UNICODE_VERSION As Integer  = &H00800000
    Public Const DTWAIN_OPENSOURCE_VERSION As Integer  = &H01000000
    Public Const DTWAINOCR_RETURNHANDLE As Integer  = 1
    Public Const DTWAINOCR_COPYDATA As Integer  = 2
    Public Const DTWAIN_OCRINFO_CHAR As Integer  = 0
    Public Const DTWAIN_OCRINFO_CHARXPOS As Integer  = 1
    Public Const DTWAIN_OCRINFO_CHARYPOS As Integer  = 2
    Public Const DTWAIN_OCRINFO_CHARXWIDTH As Integer  = 3
    Public Const DTWAIN_OCRINFO_CHARYWIDTH As Integer  = 4
    Public Const DTWAIN_OCRINFO_CHARCONFIDENCE As Integer  = 5
    Public Const DTWAIN_OCRINFO_PAGENUM As Integer  = 6
    Public Const DTWAIN_OCRINFO_OCRENGINE As Integer  = 7
    Public Const DTWAIN_OCRINFO_TEXTLENGTH As Integer  = 8
    Public Const DTWAIN_PDFPAGETYPE_COLOR As Integer  = 0
    Public Const DTWAIN_PDFPAGETYPE_BW As Integer  = 1
    Public Const DTWAIN_TWAINDSM_LEGACY As Integer  = 1
    Public Const DTWAIN_TWAINDSM_VERSION2 As Integer  = 2
    Public Const DTWAIN_TWAINDSM_LATESTVERSION As Integer  = 4
    Public Const DTWAIN_TWAINDSMSEARCH_NOTFOUND As Integer  = (-1)
    Public Const DTWAIN_TWAINDSMSEARCH_WSO As Integer  = 0
    Public Const DTWAIN_TWAINDSMSEARCH_WOS As Integer  = 1
    Public Const DTWAIN_TWAINDSMSEARCH_SWO As Integer  = 2
    Public Const DTWAIN_TWAINDSMSEARCH_SOW As Integer  = 3
    Public Const DTWAIN_TWAINDSMSEARCH_OWS As Integer  = 4
    Public Const DTWAIN_TWAINDSMSEARCH_OSW As Integer  = 5
    Public Const DTWAIN_TWAINDSMSEARCH_W As Integer  = 6
    Public Const DTWAIN_TWAINDSMSEARCH_S As Integer  = 7
    Public Const DTWAIN_TWAINDSMSEARCH_O As Integer  = 8
    Public Const DTWAIN_TWAINDSMSEARCH_WS As Integer  = 9
    Public Const DTWAIN_TWAINDSMSEARCH_WO As Integer  = 10
    Public Const DTWAIN_TWAINDSMSEARCH_SW As Integer  = 11
    Public Const DTWAIN_TWAINDSMSEARCH_SO As Integer  = 12
    Public Const DTWAIN_TWAINDSMSEARCH_OW As Integer  = 13
    Public Const DTWAIN_TWAINDSMSEARCH_OS As Integer  = 14
    Public Const DTWAIN_PDFPOLARITY_POSITIVE As Integer  = 1
    Public Const DTWAIN_PDFPOLARITY_NEGATIVE As Integer  = 2
    Public Const DTWAIN_TWPF_NORMAL As Integer  = 0
    Public Const DTWAIN_TWPF_BOLD As Integer  = 1
    Public Const DTWAIN_TWPF_ITALIC As Integer  = 2
    Public Const DTWAIN_TWPF_LARGESIZE As Integer  = 3
    Public Const DTWAIN_TWPF_SMALLSIZE As Integer  = 4
    Public Const DTWAIN_TWCT_PAGE As Integer  = 0
    Public Const DTWAIN_TWCT_PATCH1 As Integer  = 1
    Public Const DTWAIN_TWCT_PATCH2 As Integer  = 2
    Public Const DTWAIN_TWCT_PATCH3 As Integer  = 3
    Public Const DTWAIN_TWCT_PATCH4 As Integer  = 4
    Public Const DTWAIN_TWCT_PATCH5 As Integer  = 5
    Public Const DTWAIN_TWCT_PATCH6 As Integer  = 6
    Public Const DTWAIN_TWDF_ULTRASONIC As Integer  = 0
    Public Const DTWAIN_TWDF_BYLENGTH As Integer  = 1
    Public Const DTWAIN_TWDF_INFRARED As Integer  = 2
    Public Const DTWAIN_CV_CAPCUSTOMBASE As Integer  = &H8000
    Public Const DTWAIN_CV_CAPXFERCOUNT As Integer  = &H0001
    Public Const DTWAIN_CV_ICAPCOMPRESSION As Integer  = &H0100
    Public Const DTWAIN_CV_ICAPPIXELTYPE As Integer  = &H0101
    Public Const DTWAIN_CV_ICAPUNITS As Integer  = &H0102
    Public Const DTWAIN_CV_ICAPXFERMECH As Integer  = &H0103
    Public Const DTWAIN_CV_CAPAUTHOR As Integer  = &H1000
    Public Const DTWAIN_CV_CAPCAPTION As Integer  = &H1001
    Public Const DTWAIN_CV_CAPFEEDERENABLED As Integer  = &H1002
    Public Const DTWAIN_CV_CAPFEEDERLOADED As Integer  = &H1003
    Public Const DTWAIN_CV_CAPTIMEDATE As Integer  = &H1004
    Public Const DTWAIN_CV_CAPSUPPORTEDCAPS As Integer  = &H1005
    Public Const DTWAIN_CV_CAPEXTENDEDCAPS As Integer  = &H1006
    Public Const DTWAIN_CV_CAPAUTOFEED As Integer  = &H1007
    Public Const DTWAIN_CV_CAPCLEARPAGE As Integer  = &H1008
    Public Const DTWAIN_CV_CAPFEEDPAGE As Integer  = &H1009
    Public Const DTWAIN_CV_CAPREWINDPAGE As Integer  = &H100a
    Public Const DTWAIN_CV_CAPINDICATORS As Integer  = &H100b
    Public Const DTWAIN_CV_CAPSUPPORTEDCAPSEXT As Integer  = &H100c
    Public Const DTWAIN_CV_CAPPAPERDETECTABLE As Integer  = &H100d
    Public Const DTWAIN_CV_CAPUICONTROLLABLE As Integer  = &H100e
    Public Const DTWAIN_CV_CAPDEVICEONLINE As Integer  = &H100f
    Public Const DTWAIN_CV_CAPAUTOSCAN As Integer  = &H1010
    Public Const DTWAIN_CV_CAPTHUMBNAILSENABLED As Integer  = &H1011
    Public Const DTWAIN_CV_CAPDUPLEX As Integer  = &H1012
    Public Const DTWAIN_CV_CAPDUPLEXENABLED As Integer  = &H1013
    Public Const DTWAIN_CV_CAPENABLEDSUIONLY As Integer  = &H1014
    Public Const DTWAIN_CV_CAPCUSTOMDSDATA As Integer  = &H1015
    Public Const DTWAIN_CV_CAPENDORSER As Integer  = &H1016
    Public Const DTWAIN_CV_CAPJOBCONTROL As Integer  = &H1017
    Public Const DTWAIN_CV_CAPALARMS As Integer  = &H1018
    Public Const DTWAIN_CV_CAPALARMVOLUME As Integer  = &H1019
    Public Const DTWAIN_CV_CAPAUTOMATICCAPTURE As Integer  = &H101a
    Public Const DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE As Integer  = &H101b
    Public Const DTWAIN_CV_CAPTIMEBETWEENCAPTURES As Integer  = &H101c
    Public Const DTWAIN_CV_CAPCLEARBUFFERS As Integer  = &H101d
    Public Const DTWAIN_CV_CAPMAXBATCHBUFFERS As Integer  = &H101e
    Public Const DTWAIN_CV_CAPDEVICETIMEDATE As Integer  = &H101f
    Public Const DTWAIN_CV_CAPPOWERSUPPLY As Integer  = &H1020
    Public Const DTWAIN_CV_CAPCAMERAPREVIEWUI As Integer  = &H1021
    Public Const DTWAIN_CV_CAPDEVICEEVENT As Integer  = &H1022
    Public Const DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE As Integer  = &H1023
    Public Const DTWAIN_CV_CAPSERIALNUMBER As Integer  = &H1024
    Public Const DTWAIN_CV_CAPFILESYSTEM As Integer  = &H1025
    Public Const DTWAIN_CV_CAPPRINTER As Integer  = &H1026
    Public Const DTWAIN_CV_CAPPRINTERENABLED As Integer  = &H1027
    Public Const DTWAIN_CV_CAPPRINTERINDEX As Integer  = &H1028
    Public Const DTWAIN_CV_CAPPRINTERMODE As Integer  = &H1029
    Public Const DTWAIN_CV_CAPPRINTERSTRING As Integer  = &H102a
    Public Const DTWAIN_CV_CAPPRINTERSUFFIX As Integer  = &H102b
    Public Const DTWAIN_CV_CAPLANGUAGE As Integer  = &H102c
    Public Const DTWAIN_CV_CAPFEEDERALIGNMENT As Integer  = &H102d
    Public Const DTWAIN_CV_CAPFEEDERORDER As Integer  = &H102e
    Public Const DTWAIN_CV_CAPPAPERBINDING As Integer  = &H102f
    Public Const DTWAIN_CV_CAPREACQUIREALLOWED As Integer  = &H1030
    Public Const DTWAIN_CV_CAPPASSTHRU As Integer  = &H1031
    Public Const DTWAIN_CV_CAPBATTERYMINUTES As Integer  = &H1032
    Public Const DTWAIN_CV_CAPBATTERYPERCENTAGE As Integer  = &H1033
    Public Const DTWAIN_CV_CAPPOWERDOWNTIME As Integer  = &H1034
    Public Const DTWAIN_CV_CAPSEGMENTED As Integer  = &H1035
    Public Const DTWAIN_CV_CAPCAMERAENABLED As Integer  = &H1036
    Public Const DTWAIN_CV_CAPCAMERAORDER As Integer  = &H1037
    Public Const DTWAIN_CV_CAPMICRENABLED As Integer  = &H1038
    Public Const DTWAIN_CV_CAPFEEDERPREP As Integer  = &H1039
    Public Const DTWAIN_CV_CAPFEEDERPOCKET As Integer  = &H103a
    Public Const DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM As Integer  = &H103b
    Public Const DTWAIN_CV_CAPCUSTOMINTERFACEGUID As Integer  = &H103c
    Public Const DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE As Integer  = &H103d
    Public Const DTWAIN_CV_CAPSUPPORTEDDATS As Integer  = &H103e
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTION As Integer  = &H103f
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH As Integer  = &H1040
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY As Integer  = &H1041
    Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE As Integer  = &H1042
    Public Const DTWAIN_CV_CAPPAPERHANDLING As Integer  = &H1043
    Public Const DTWAIN_CV_CAPINDICATORSMODE As Integer  = &H1044
    Public Const DTWAIN_CV_CAPPRINTERVERTICALOFFSET As Integer  = &H1045
    Public Const DTWAIN_CV_CAPPOWERSAVETIME As Integer  = &H1046
    Public Const DTWAIN_CV_CAPPRINTERCHARROTATION As Integer  = &H1047
    Public Const DTWAIN_CV_CAPPRINTERFONTSTYLE As Integer  = &H1048
    Public Const DTWAIN_CV_CAPPRINTERINDEXLEADCHAR As Integer  = &H1049
    Public Const DTWAIN_CV_CAPPRINTERINDEXMAXVALUE As Integer  = &H104A
    Public Const DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS As Integer  = &H104B
    Public Const DTWAIN_CV_CAPPRINTERINDEXSTEP As Integer  = &H104C
    Public Const DTWAIN_CV_CAPPRINTERINDEXTRIGGER As Integer  = &H104D
    Public Const DTWAIN_CV_CAPPRINTERSTRINGPREVIEW As Integer  = &H104E
    Public Const DTWAIN_CV_ICAPAUTOBRIGHT As Integer  = &H1100
    Public Const DTWAIN_CV_ICAPBRIGHTNESS As Integer  = &H1101
    Public Const DTWAIN_CV_ICAPCONTRAST As Integer  = &H1103
    Public Const DTWAIN_CV_ICAPCUSTHALFTONE As Integer  = &H1104
    Public Const DTWAIN_CV_ICAPEXPOSURETIME As Integer  = &H1105
    Public Const DTWAIN_CV_ICAPFILTER As Integer  = &H1106
    Public Const DTWAIN_CV_ICAPFLASHUSED As Integer  = &H1107
    Public Const DTWAIN_CV_ICAPGAMMA As Integer  = &H1108
    Public Const DTWAIN_CV_ICAPHALFTONES As Integer  = &H1109
    Public Const DTWAIN_CV_ICAPHIGHLIGHT As Integer  = &H110a
    Public Const DTWAIN_CV_ICAPIMAGEFILEFORMAT As Integer  = &H110c
    Public Const DTWAIN_CV_ICAPLAMPSTATE As Integer  = &H110d
    Public Const DTWAIN_CV_ICAPLIGHTSOURCE As Integer  = &H110e
    Public Const DTWAIN_CV_ICAPORIENTATION As Integer  = &H1110
    Public Const DTWAIN_CV_ICAPPHYSICALWIDTH As Integer  = &H1111
    Public Const DTWAIN_CV_ICAPPHYSICALHEIGHT As Integer  = &H1112
    Public Const DTWAIN_CV_ICAPSHADOW As Integer  = &H1113
    Public Const DTWAIN_CV_ICAPFRAMES As Integer  = &H1114
    Public Const DTWAIN_CV_ICAPXNATIVERESOLUTION As Integer  = &H1116
    Public Const DTWAIN_CV_ICAPYNATIVERESOLUTION As Integer  = &H1117
    Public Const DTWAIN_CV_ICAPXRESOLUTION As Integer  = &H1118
    Public Const DTWAIN_CV_ICAPYRESOLUTION As Integer  = &H1119
    Public Const DTWAIN_CV_ICAPMAXFRAMES As Integer  = &H111a
    Public Const DTWAIN_CV_ICAPTILES As Integer  = &H111b
    Public Const DTWAIN_CV_ICAPBITORDER As Integer  = &H111c
    Public Const DTWAIN_CV_ICAPCCITTKFACTOR As Integer  = &H111d
    Public Const DTWAIN_CV_ICAPLIGHTPATH As Integer  = &H111e
    Public Const DTWAIN_CV_ICAPPIXELFLAVOR As Integer  = &H111f
    Public Const DTWAIN_CV_ICAPPLANARCHUNKY As Integer  = &H1120
    Public Const DTWAIN_CV_ICAPROTATION As Integer  = &H1121
    Public Const DTWAIN_CV_ICAPSUPPORTEDSIZES As Integer  = &H1122
    Public Const DTWAIN_CV_ICAPTHRESHOLD As Integer  = &H1123
    Public Const DTWAIN_CV_ICAPXSCALING As Integer  = &H1124
    Public Const DTWAIN_CV_ICAPYSCALING As Integer  = &H1125
    Public Const DTWAIN_CV_ICAPBITORDERCODES As Integer  = &H1126
    Public Const DTWAIN_CV_ICAPPIXELFLAVORCODES As Integer  = &H1127
    Public Const DTWAIN_CV_ICAPJPEGPIXELTYPE As Integer  = &H1128
    Public Const DTWAIN_CV_ICAPTIMEFILL As Integer  = &H112a
    Public Const DTWAIN_CV_ICAPBITDEPTH As Integer  = &H112b
    Public Const DTWAIN_CV_ICAPBITDEPTHREDUCTION As Integer  = &H112c
    Public Const DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE As Integer  = &H112d
    Public Const DTWAIN_CV_ICAPIMAGEDATASET As Integer  = &H112e
    Public Const DTWAIN_CV_ICAPEXTIMAGEINFO As Integer  = &H112f
    Public Const DTWAIN_CV_ICAPMINIMUMHEIGHT As Integer  = &H1130
    Public Const DTWAIN_CV_ICAPMINIMUMWIDTH As Integer  = &H1131
    Public Const DTWAIN_CV_ICAPAUTOBORDERDETECTION As Integer  = &H1132
    Public Const DTWAIN_CV_ICAPAUTODESKEW As Integer  = &H1133
    Public Const DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES As Integer  = &H1134
    Public Const DTWAIN_CV_ICAPAUTOROTATE As Integer  = &H1135
    Public Const DTWAIN_CV_ICAPFLIPROTATION As Integer  = &H1136
    Public Const DTWAIN_CV_ICAPBARCODEDETECTIONENABLED As Integer  = &H1137
    Public Const DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES As Integer  = &H1138
    Public Const DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES As Integer  = &H1139
    Public Const DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES As Integer  = &H113a
    Public Const DTWAIN_CV_ICAPBARCODESEARCHMODE As Integer  = &H113b
    Public Const DTWAIN_CV_ICAPBARCODEMAXRETRIES As Integer  = &H113c
    Public Const DTWAIN_CV_ICAPBARCODETIMEOUT As Integer  = &H113d
    Public Const DTWAIN_CV_ICAPZOOMFACTOR As Integer  = &H113e
    Public Const DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED As Integer  = &H113f
    Public Const DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES As Integer  = &H1140
    Public Const DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES As Integer  = &H1141
    Public Const DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES As Integer  = &H1142
    Public Const DTWAIN_CV_ICAPPATCHCODESEARCHMODE As Integer  = &H1143
    Public Const DTWAIN_CV_ICAPPATCHCODEMAXRETRIES As Integer  = &H1144
    Public Const DTWAIN_CV_ICAPPATCHCODETIMEOUT As Integer  = &H1145
    Public Const DTWAIN_CV_ICAPFLASHUSED2 As Integer  = &H1146
    Public Const DTWAIN_CV_ICAPIMAGEFILTER As Integer  = &H1147
    Public Const DTWAIN_CV_ICAPNOISEFILTER As Integer  = &H1148
    Public Const DTWAIN_CV_ICAPOVERSCAN As Integer  = &H1149
    Public Const DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION As Integer  = &H1150
    Public Const DTWAIN_CV_ICAPAUTOMATICDESKEW As Integer  = &H1151
    Public Const DTWAIN_CV_ICAPAUTOMATICROTATE As Integer  = &H1152
    Public Const DTWAIN_CV_ICAPJPEGQUALITY As Integer  = &H1153
    Public Const DTWAIN_CV_ICAPFEEDERTYPE As Integer  = &H1154
    Public Const DTWAIN_CV_ICAPICCPROFILE As Integer  = &H1155
    Public Const DTWAIN_CV_ICAPAUTOSIZE As Integer  = &H1156
    Public Const DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME As Integer  = &H1157
    Public Const DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION As Integer  = &H1158
    Public Const DTWAIN_CV_ICAPAUTOMATICCOLORENABLED As Integer  = &H1159
    Public Const DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE As Integer  = &H115a
    Public Const DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED As Integer  = &H115b
    Public Const DTWAIN_CV_ICAPIMAGEMERGE As Integer  = &H115c
    Public Const DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD As Integer  = &H115d
    Public Const DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO As Integer  = &H115e
    Public Const DTWAIN_CV_ICAPFILMTYPE As Integer  = &H115f
    Public Const DTWAIN_CV_ICAPMIRROR As Integer  = &H1160
    Public Const DTWAIN_CV_ICAPJPEGSUBSAMPLING As Integer  = &H1161
    Public Const DTWAIN_CV_ACAPAUDIOFILEFORMAT As Integer  = &H1201
    Public Const DTWAIN_CV_ACAPXFERMECH As Integer  = &H1202
    Public Const DTWAIN_CFMCV_CAPCFMSTART As Integer  = 2048
    Public Const DTWAIN_CFMCV_CAPDUPLEXSCANNER As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+10)
    Public Const DTWAIN_CFMCV_CAPDUPLEXENABLE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+11)
    Public Const DTWAIN_CFMCV_CAPSCANNERNAME As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+12)
    Public Const DTWAIN_CFMCV_CAPSINGLEPASS As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+13)
    Public Const DTWAIN_CFMCV_CAPERRHANDLING As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+20)
    Public Const DTWAIN_CFMCV_CAPFEEDERSTATUS As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+21)
    Public Const DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+22)
    Public Const DTWAIN_CFMCV_CAPFEEDWAITTIME As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+23)
    Public Const DTWAIN_CFMCV_ICAPWHITEBALANCE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+24)
    Public Const DTWAIN_CFMCV_ICAPAUTOBINARY As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+25)
    Public Const DTWAIN_CFMCV_ICAPIMAGESEPARATION As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+26)
    Public Const DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+27)
    Public Const DTWAIN_CFMCV_ICAPIMAGEEMPHASIS As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+28)
    Public Const DTWAIN_CFMCV_ICAPOUTLINING As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+29)
    Public Const DTWAIN_CFMCV_ICAPDYNTHRESHOLD As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+30)
    Public Const DTWAIN_CFMCV_ICAPVARIANCE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+31)
    Public Const DTWAIN_CFMCV_CAPENDORSERAVAILABLE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+32)
    Public Const DTWAIN_CFMCV_CAPENDORSERENABLE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+33)
    Public Const DTWAIN_CFMCV_CAPENDORSERCHARSET As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+34)
    Public Const DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+35)
    Public Const DTWAIN_CFMCV_CAPENDORSERSTRING As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+36)
    Public Const DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+48)
    Public Const DTWAIN_CFMCV_ICAPSMOOTHINGMODE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+49)
    Public Const DTWAIN_CFMCV_ICAPFILTERMODE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+50)
    Public Const DTWAIN_CFMCV_ICAPGRADATION As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+51)
    Public Const DTWAIN_CFMCV_ICAPMIRROR As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+52)
    Public Const DTWAIN_CFMCV_ICAPEASYSCANMODE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+53)
    Public Const DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+54)
    Public Const DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+55)
    Public Const DTWAIN_CFMCV_CAPDUPLEXPAGE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+56)
    Public Const DTWAIN_CFMCV_ICAPINVERTIMAGE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+57)
    Public Const DTWAIN_CFMCV_ICAPSPECKLEREMOVE As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+58)
    Public Const DTWAIN_CFMCV_ICAPUSMFILTER As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+59)
    Public Const DTWAIN_CFMCV_ICAPNOISEFILTERCFM As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+60)
    Public Const DTWAIN_CFMCV_ICAPDESCREENING As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+61)
    Public Const DTWAIN_CFMCV_ICAPQUALITYFILTER As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+62)
    Public Const DTWAIN_CFMCV_ICAPBINARYFILTER As Integer  = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+63)
    Public Const DTWAIN_OCRCV_IMAGEFILEFORMAT As Integer  = &H1000
    Public Const DTWAIN_OCRCV_DESKEW As Integer  = &H1001
    Public Const DTWAIN_OCRCV_DESHADE As Integer  = &H1002
    Public Const DTWAIN_OCRCV_ORIENTATION As Integer  = &H1003
    Public Const DTWAIN_OCRCV_NOISEREMOVE As Integer  = &H1004
    Public Const DTWAIN_OCRCV_LINEREMOVE As Integer  = &H1005
    Public Const DTWAIN_OCRCV_INVERTPAGE As Integer  = &H1006
    Public Const DTWAIN_OCRCV_INVERTZONES As Integer  = &H1007
    Public Const DTWAIN_OCRCV_LINEREJECT As Integer  = &H1008
    Public Const DTWAIN_OCRCV_CHARACTERREJECT As Integer  = &H1009
    Public Const DTWAIN_OCRCV_ERRORREPORTMODE As Integer  = &H1010
    Public Const DTWAIN_OCRCV_ERRORREPORTFILE As Integer  = &H1011
    Public Const DTWAIN_OCRCV_PIXELTYPE As Integer  = &H1012
    Public Const DTWAIN_OCRCV_BITDEPTH As Integer  = &H1013
    Public Const DTWAIN_OCRCV_RETURNCHARINFO As Integer  = &H1014
    Public Const DTWAIN_OCRCV_NATIVEFILEFORMAT As Integer  = &H1015
    Public Const DTWAIN_OCRCV_MPNATIVEFILEFORMAT As Integer  = &H1016
    Public Const DTWAIN_OCRCV_SUPPORTEDCAPS As Integer  = &H1017
    Public Const DTWAIN_OCRCV_DISABLECHARACTERS As Integer  = &H1018
    Public Const DTWAIN_OCRCV_REMOVECONTROLCHARS As Integer  = &H1019
    Public Const DTWAIN_OCRORIENT_OFF As Integer  = 0
    Public Const DTWAIN_OCRORIENT_AUTO As Integer  = 1
    Public Const DTWAIN_OCRORIENT_90 As Integer  = 2
    Public Const DTWAIN_OCRORIENT_180 As Integer  = 3
    Public Const DTWAIN_OCRORIENT_270 As Integer  = 4
    Public Const DTWAIN_OCRIMAGEFORMAT_AUTO As Integer  = 10000
    Public Const DTWAIN_OCRERROR_MODENONE As Integer  = 0
    Public Const DTWAIN_OCRERROR_SHOWMSGBOX As Integer  = 1
    Public Const DTWAIN_OCRERROR_WRITEFILE As Integer  = 2
    Public Const DTWAIN_PDFTEXT_ALLPAGES As Integer  = &H00000001
    Public Const DTWAIN_PDFTEXT_EVENPAGES As Integer  = &H00000002
    Public Const DTWAIN_PDFTEXT_ODDPAGES As Integer  = &H00000004
    Public Const DTWAIN_PDFTEXT_FIRSTPAGE As Integer  = &H00000008
    Public Const DTWAIN_PDFTEXT_LASTPAGE As Integer  = &H00000010
    Public Const DTWAIN_PDFTEXT_CURRENTPAGE As Integer  = &H00000020
    Public Const DTWAIN_PDFTEXT_DISABLED As Integer  = &H00000040
    Public Const DTWAIN_PDFTEXT_TOPLEFT As Integer  = &H00000100
    Public Const DTWAIN_PDFTEXT_TOPRIGHT As Integer  = &H00000200
    Public Const DTWAIN_PDFTEXT_HORIZCENTER As Integer  = &H00000400
    Public Const DTWAIN_PDFTEXT_VERTCENTER As Integer  = &H00000800
    Public Const DTWAIN_PDFTEXT_BOTTOMLEFT As Integer  = &H00001000
    Public Const DTWAIN_PDFTEXT_BOTTOMRIGHT As Integer  = &H00002000
    Public Const DTWAIN_PDFTEXT_BOTTOMCENTER As Integer  = &H00004000
    Public Const DTWAIN_PDFTEXT_TOPCENTER As Integer  = &H00008000
    Public Const DTWAIN_PDFTEXT_XCENTER As Integer  = &H00010000
    Public Const DTWAIN_PDFTEXT_YCENTER As Integer  = &H00020000
    Public Const DTWAIN_PDFTEXT_NOSCALING As Integer  = &H00100000
    Public Const DTWAIN_PDFTEXT_NOCHARSPACING As Integer  = &H00200000
    Public Const DTWAIN_PDFTEXT_NOWORDSPACING As Integer  = &H00400000
    Public Const DTWAIN_PDFTEXT_NOSTROKEWIDTH As Integer  = &H00800000
    Public Const DTWAIN_PDFTEXT_NORENDERMODE As Integer  = &H01000000
    Public Const DTWAIN_PDFTEXT_NORGBCOLOR As Integer  = &H02000000
    Public Const DTWAIN_PDFTEXT_NOFONTSIZE As Integer  = &H04000000
    Public Const DTWAIN_PDFTEXT_NOABSPOSITION As Integer  = &H08000000
    Public Const DTWAIN_PDFTEXT_IGNOREALL As Integer  = &HFFF00000
    Public Const DTWAIN_FONT_COURIER As Integer  = 0
    Public Const DTWAIN_FONT_COURIERBOLD As Integer  = 1
    Public Const DTWAIN_FONT_COURIERBOLDOBLIQUE As Integer  = 2
    Public Const DTWAIN_FONT_COURIEROBLIQUE As Integer  = 3
    Public Const DTWAIN_FONT_HELVETICA As Integer  = 4
    Public Const DTWAIN_FONT_HELVETICABOLD As Integer  = 5
    Public Const DTWAIN_FONT_HELVETICABOLDOBLIQUE As Integer  = 6
    Public Const DTWAIN_FONT_HELVETICAOBLIQUE As Integer  = 7
    Public Const DTWAIN_FONT_TIMESBOLD As Integer  = 8
    Public Const DTWAIN_FONT_TIMESBOLDITALIC As Integer  = 9
    Public Const DTWAIN_FONT_TIMESROMAN As Integer  = 10
    Public Const DTWAIN_FONT_TIMESITALIC As Integer  = 11
    Public Const DTWAIN_FONT_SYMBOL As Integer  = 12
    Public Const DTWAIN_FONT_ZAPFDINGBATS As Integer  = 13
    Public Const DTWAIN_PDFRENDER_FILL As Integer  = 0
    Public Const DTWAIN_PDFRENDER_STROKE As Integer  = 1
    Public Const DTWAIN_PDFRENDER_FILLSTROKE As Integer  = 2
    Public Const DTWAIN_PDFRENDER_INVISIBLE As Integer  = 3
    Public Const DTWAIN_PDFTEXTELEMENT_SCALINGXY As Integer  = 0
    Public Const DTWAIN_PDFTEXTELEMENT_FONTHEIGHT As Integer  = 1
    Public Const DTWAIN_PDFTEXTELEMENT_WORDSPACING As Integer  = 2
    Public Const DTWAIN_PDFTEXTELEMENT_POSITION As Integer  = 3
    Public Const DTWAIN_PDFTEXTELEMENT_COLOR As Integer  = 4
    Public Const DTWAIN_PDFTEXTELEMENT_STROKEWIDTH As Integer  = 5
    Public Const DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS As Integer  = 6
    Public Const DTWAIN_PDFTEXTELEMENT_FONTNAME As Integer  = 7
    Public Const DTWAIN_PDFTEXTELEMENT_TEXT As Integer  = 8
    Public Const DTWAIN_PDFTEXTELEMENT_RENDERMODE As Integer  = 9
    Public Const DTWAIN_PDFTEXTELEMENT_CHARSPACING As Integer  = 10
    Public Const DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE As Integer  = 11
    Public Const DTWAIN_PDFTEXTELEMENT_LEADING As Integer  = 12
    Public Const DTWAIN_PDFTEXTELEMENT_SCALING As Integer  = 13
    Public Const DTWAIN_PDFTEXTELEMENT_TEXTLENGTH As Integer  = 14
    Public Const DTWAIN_PDFTEXTELEMENT_SKEWANGLES As Integer  = 15
    Public Const DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER As Integer  = 16
    Public Const DTWAIN_PDFTEXTTRANSFORM_TSRK As Integer  = 0
    Public Const DTWAIN_PDFTEXTTRANSFORM_TSKR As Integer  = 1
    Public Const DTWAIN_PDFTEXTTRANSFORM_TKSR As Integer  = 2
    Public Const DTWAIN_PDFTEXTTRANSFORM_TKRS As Integer  = 3
    Public Const DTWAIN_PDFTEXTTRANSFORM_TRSK As Integer  = 4
    Public Const DTWAIN_PDFTEXTTRANSFORM_TRKS As Integer  = 5
    Public Const DTWAIN_PDFTEXTTRANSFORM_STRK As Integer  = 6
    Public Const DTWAIN_PDFTEXTTRANSFORM_STKR As Integer  = 7
    Public Const DTWAIN_PDFTEXTTRANSFORM_SKTR As Integer  = 8
    Public Const DTWAIN_PDFTEXTTRANSFORM_SKRT As Integer  = 9
    Public Const DTWAIN_PDFTEXTTRANSFORM_SRTK As Integer  = 10
    Public Const DTWAIN_PDFTEXTTRANSFORM_SRKT As Integer  = 11
    Public Const DTWAIN_PDFTEXTTRANSFORM_RSTK As Integer  = 12
    Public Const DTWAIN_PDFTEXTTRANSFORM_RSKT As Integer  = 13
    Public Const DTWAIN_PDFTEXTTRANSFORM_RTSK As Integer  = 14
    Public Const DTWAIN_PDFTEXTTRANSFORM_RTKT As Integer  = 15
    Public Const DTWAIN_PDFTEXTTRANSFORM_RKST As Integer  = 16
    Public Const DTWAIN_PDFTEXTTRANSFORM_RKTS As Integer  = 17
    Public Const DTWAIN_PDFTEXTTRANSFORM_KSTR As Integer  = 18
    Public Const DTWAIN_PDFTEXTTRANSFORM_KSRT As Integer  = 19
    Public Const DTWAIN_PDFTEXTTRANSFORM_KRST As Integer  = 20
    Public Const DTWAIN_PDFTEXTTRANSFORM_KRTS As Integer  = 21
    Public Const DTWAIN_PDFTEXTTRANSFORM_KTSR As Integer  = 22
    Public Const DTWAIN_PDFTEXTTRANSFORM_KTRS As Integer  = 23
    Public Const DTWAIN_PDFTEXTTRANFORM_LAST As Integer  = DTWAIN_PDFTEXTTRANSFORM_KTRS
    Public Delegate Function DTWAINCallback(ByVal WParam As Integer, ByVal LParam As Integer, ByVal UserData As Integer) As Integer
    Public Delegate Function DTWAINDibUpdateCallback(ByVal TheSource As Integer, ByVal PageNum As Integer, ByVal DibHandle As Integer ) As Integer
    Public Delegate Function DTWAINCallback64(ByVal WParam As Integer, ByVal LParam As Integer, ByVal UserData As Long) As Integer

    Public Delegate Function DTWAINLoggerCallback(ByVal sMessage As String) As Integer

    Declare Ansi Function DTWAIN_AcquireFileA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpszFile As String, ByVal lFileType As Integer, ByVal lFileFlags As Integer, ByVal PixelType As Integer, ByVal lMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Ansi Function DTWAIN_AddPDFTextA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szText As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal fontName As String, ByVal fontSize As Double, ByVal colorRGB As Integer, ByVal renderMode As Integer, ByVal scaling As Double, ByVal charSpacing As Double, ByVal wordSpacing As Double, ByVal strokeWidth As Integer, ByVal Flags As Integer) As Integer
    Declare Ansi Function DTWAIN_ArrayAddStringA Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String) As Integer
    Declare Ansi Function DTWAIN_ArrayAddStringNA Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Ansi Function DTWAIN_ArrayFindStringA Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal pString As String) As Integer
    Declare Ansi Function DTWAIN_ArrayGetAtStringA Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtStringA Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As String) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtStringNA Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Ansi Function DTWAIN_ArraySetAtStringA Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Ansi Function DTWAIN_ExecuteOCRA Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szFileName As String, ByVal nStartPage As Integer, ByVal nEndPage As Integer) As Integer
    Declare Ansi Function DTWAIN_GetAcquireArea2StringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByRef lpUnit As Integer) As Integer
    Declare Ansi Function DTWAIN_GetAppInfoA Lib "DTWAIN32.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Ansi Function DTWAIN_GetAuthorA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szAuthor As String) As Integer
    Declare Ansi Function DTWAIN_GetBrightnessStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_GetCapFromNameA Lib "DTWAIN32.DLL" (ByVal szName As String) As Integer
    Declare Ansi Function DTWAIN_GetCaptionA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Caption As String) As Integer
    Declare Ansi Function DTWAIN_GetContrastStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_GetCurrentFileNameA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szName As String, ByVal MaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetDSMFullNameA Lib "DTWAIN32.DLL" (ByVal DSMType As Integer, ByVal szDLLName As String, ByVal nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
    Declare Ansi Function DTWAIN_GetDeviceTimeDateA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Ansi Function DTWAIN_GetErrorStringA Lib "DTWAIN32.DLL" (ByVal lError As Integer, ByVal lpszBuffer As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetExtCapFromNameA Lib "DTWAIN32.DLL" (ByVal szName As String) As Integer
    Declare Ansi Function DTWAIN_GetExtNameFromCapA Lib "DTWAIN32.DLL" (ByVal nValue As Integer, ByVal szValue As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetImageInfoStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpXResolution As String, ByVal lpYResolution As String, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Integer, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Ansi Function DTWAIN_GetNameFromCapA Lib "DTWAIN32.DLL" (ByVal nCapValue As Integer, ByVal szValue As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRManufacturerA Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szManufacturer As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRProductFamilyA Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szProductFamily As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRProductNameA Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szProductName As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRTextA Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal nPageNo As Integer, ByVal Data As String, ByVal dSize As Integer, ByRef pActualSize As Integer, ByVal nFlags As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRVersionInfoA Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal buffer As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetPDFTextElementStringA Lib "DTWAIN32.DLL" (ByVal TextElement As Integer, ByVal szData As String, ByVal maxLen As Integer, ByVal Flags As Integer) As Integer
    Declare Ansi Function DTWAIN_GetPDFType1FontNameA Lib "DTWAIN32.DLL" (ByVal FontVal As Integer, ByVal szFont As String, ByVal nChars As Integer) As Integer
    Declare Ansi Function DTWAIN_GetPrinterSuffixStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Suffix As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetResolutionStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Resolution As String) As Integer
    Declare Ansi Function DTWAIN_GetRotationStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Rotation As String) As Integer
    Declare Ansi Function DTWAIN_GetSourceManufacturerA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductFamilyA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductNameA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceVersionInfoA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTempFileDirectoryA Lib "DTWAIN32.DLL" (ByVal szFilePath As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTimeDateA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Ansi Function DTWAIN_GetVersionInfoA Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionStringA Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Ansi Function DTWAIN_IsDIBBlankStringA Lib "DTWAIN32.DLL" (ByVal hDib As Integer, ByVal threshold As String) As Integer
    Declare Ansi Function DTWAIN_LoadCustomStringResourcesA Lib "DTWAIN32.DLL" (ByVal sLangDLL As String) As Integer
    Declare Ansi Function DTWAIN_LogMessageA Lib "DTWAIN32.DLL" (ByVal message As String) As Integer
    Declare Ansi Function DTWAIN_SelectOCREngineByNameA Lib "DTWAIN32.DLL" (ByVal lpszName As String) As Integer
    Declare Ansi Function DTWAIN_SelectSource2A Lib "DTWAIN32.DLL" (ByVal hWndParent As IntPtr, ByVal szTitle As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal nOptions As Integer) As Integer
    Declare Ansi Function DTWAIN_SelectSourceByNameA Lib "DTWAIN32.DLL" (ByVal lpszName As String) As Integer
    Declare Ansi Function DTWAIN_SetAcquireArea2StringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByVal lUnit As Integer, ByVal Flags As Integer) As Integer
    Declare Ansi Function DTWAIN_SetAcquireImageScaleStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal xscale As String, ByVal yscale As String) As Integer
    Declare Ansi Function DTWAIN_SetAppInfoA Lib "DTWAIN32.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Ansi Function DTWAIN_SetAuthorA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szAuthor As String) As Integer
    Declare Ansi Function DTWAIN_SetBlankPageDetectionStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal threshold As String, ByVal autodetect_option As Integer, ByVal bSet As Integer) As Integer
    Declare Ansi Function DTWAIN_SetBrightnessStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_SetCameraA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szCamera As String) As Integer
    Declare Ansi Function DTWAIN_SetCaptionA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Caption As String) As Integer
    Declare Ansi Function DTWAIN_SetContrastStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_SetDeviceTimeDateA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Ansi Function DTWAIN_SetFileSavePosA Lib "DTWAIN32.DLL" (ByVal hWndParent As IntPtr, ByVal szTitle As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal nFlags As Integer) As Integer
    Declare Ansi Function DTWAIN_SetPDFAuthorA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpAuthor As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFCreatorA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpCreator As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFEncryptionA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bUseEncryption As Integer, ByVal lpszUser As String, ByVal lpszOwner As String, ByVal Permissions As Integer, ByVal UseStrongEncryption As Integer) As Integer
    Declare Ansi Function DTWAIN_SetPDFKeywordsA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpKeyWords As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFPageScaleStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nOptions As Integer, ByVal xScale As String, ByVal yScale As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFPageSizeStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PageSize As Integer, ByVal CustomWidth As String, ByVal CustomHeight As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFProducerA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpProducer As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFSubjectA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpSubject As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFTitleA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpTitle As String) As Integer
    Declare Ansi Function DTWAIN_SetPostScriptTitleA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTitle As String) As Integer
    Declare Ansi Function DTWAIN_SetPrinterSuffixStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Suffix As String) As Integer
    Declare Ansi Function DTWAIN_SetResolutionStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Resolution As String) As Integer
    Declare Ansi Function DTWAIN_SetRotationStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Rotation As String) As Integer
    Declare Ansi Function DTWAIN_SetTempFileDirectoryA Lib "DTWAIN32.DLL" (ByVal szFilePath As String) As Integer
    Declare Ansi Function DTWAIN_SetTwainLogA Lib "DTWAIN32.DLL" (ByVal LogFlags As Integer, ByVal lpszLogFile As String) As Integer
    Declare Ansi Function DTWAIN_StartTwainSessionA Lib "DTWAIN32.DLL" (ByVal hWndMsg As IntPtr, ByVal lpszDLLName As String) As Integer
    Declare Ansi Function DTWAIN_SysInitializeEx2A Lib "DTWAIN32.DLL" (ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Integer
    Declare Ansi Function DTWAIN_SysInitializeExA Lib "DTWAIN32.DLL" (ByVal szINIPath As String) As Integer
    Declare Ansi Function DTWAIN_SysInitializeLibEx2A Lib "DTWAIN32.DLL" (ByVal hInstance As Integer, ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Integer
    Declare Ansi Function DTWAIN_SysInitializeLibExA Lib "DTWAIN32.DLL" (ByVal hInstance As Integer, ByVal szINIPath As String) As Integer
    Declare Ansi Function DTWAIN_TwainSaveA Lib "DTWAIN32.DLL" (ByVal cmd As String) As Integer
    Declare Ansi Sub DTWAIN_XA Lib "DTWAIN32.DLL" (ByVal s As String) 
    Declare Auto Function DTWAIN_AcquireBuffered Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelType As Integer, ByVal nMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireBufferedEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelType As Integer, ByVal nMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByVal Acquisitions As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireFile Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpszFile As String, ByVal lFileType As Integer, ByVal lFileFlags As Integer, ByVal PixelType As Integer, ByVal lMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireFileEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal aFileNames As Integer, ByVal lFileType As Integer, ByVal lFileFlags As Integer, ByVal PixelType As Integer, ByVal lMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireNative Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelType As Integer, ByVal nMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireNativeEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelType As Integer, ByVal nMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByVal Acquisitions As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireToClipboard Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelType As Integer, ByVal nMaxPages As Integer, ByVal nTransferMode As Integer, ByVal bDiscardDibs As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AddPDFText Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szText As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal fontName As String, ByVal fontSize As Double, ByVal colorRGB As Integer, ByVal renderMode As Integer, ByVal scaling As Double, ByVal charSpacing As Double, ByVal wordSpacing As Double, ByVal strokeWidth As Integer, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_AppHandlesExceptions Lib "DTWAIN32.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddANSIString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddANSIStringN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As Double) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloatN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As Double, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddLongN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As Integer, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddStringN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddWideString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddWideStringN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayConvertFix32ToFloat Lib "DTWAIN32.DLL" (ByVal Fix32Array As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayConvertFloatToFix32 Lib "DTWAIN32.DLL" (ByVal FloatArray As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayCopy Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Dest As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayCreate Lib "DTWAIN32.DLL" (ByVal nEnumType As Integer, ByVal nInitialSize As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayCreateCopy Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayCreateFromCap Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCapType As Integer, ByVal lSize As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayCreateFromLongs Lib "DTWAIN32.DLL" (ByRef pCArray As Integer, ByVal nSize As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayDestroy Lib "DTWAIN32.DLL" (ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayDestroyFrames Lib "DTWAIN32.DLL" (ByVal FrameArray As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayFindANSIString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFindFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As Double, ByVal Tolerance As Double) As Integer
    Declare Auto Function DTWAIN_ArrayFindLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayFindString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFindWideString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFix32GetAt Lib "DTWAIN32.DLL" (ByVal aFix32 As Integer, ByVal lPos As Integer, ByRef Whole As Integer, ByRef Frac As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayFix32SetAt Lib "DTWAIN32.DLL" (ByVal aFix32 As Integer, ByVal lPos As Integer, ByVal Whole As Integer, ByVal Frac As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayFrameGetFrameAt Lib "DTWAIN32.DLL" (ByVal FrameArray As Integer, ByVal nWhere As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtANSIString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByRef pVal As Double) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtWideString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArrayGetBuffer Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nPos As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetCount Lib "DTWAIN32.DLL" (ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetMaxStringLength Lib "DTWAIN32.DLL" (ByVal a As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetStringLength Lib "DTWAIN32.DLL" (ByVal a As Integer, ByVal nWhichString As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetType Lib "DTWAIN32.DLL" (ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtANSIString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtANSIStringN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As Double) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloatN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal Val As Double, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLongN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As Integer, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtStringN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtWideString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtWideStringN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAll Lib "DTWAIN32.DLL" (ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAt Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAtN Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal num As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayResize Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal NewSize As Integer) As Integer
    Declare Auto Function DTWAIN_ArraySetAtANSIString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArraySetAtFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As Double) As Integer
    Declare Auto Function DTWAIN_ArraySetAtLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As Integer) As Integer
    Declare Auto Function DTWAIN_ArraySetAtString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArraySetAtWideString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_CallCallback Lib "DTWAIN32.DLL" (ByVal wParam As Integer, ByVal lParam As Integer, ByVal UserData As Integer) As Integer
    Declare Auto Function DTWAIN_ClearBuffers Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal ClearBuffer As Integer) As Integer
    Declare Auto Function DTWAIN_ClearErrorBuffer Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_ClearPDFText Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_ClearPage Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_CloseSource Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_CloseSourceUI Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_CreateAcquisitionArray Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_CreatePDFTextElement Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_DestroyAcquisitionArray Lib "DTWAIN32.DLL" (ByVal aAcq As Integer, ByVal bDestroyData As Integer) As Integer
    Declare Auto Function DTWAIN_DestroyPDFTextElement Lib "DTWAIN32.DLL" (ByVal TextElement As Integer) As Integer
    Declare Auto Function DTWAIN_DisableAppWindow Lib "DTWAIN32.DLL" (ByVal hWnd As IntPtr, ByVal bDisable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoBorderDetect Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoBright Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoDeskew Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoFeed Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoRotate Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoScan Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableDuplex Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableFeeder Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableIndicator Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableJobFileHandling Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableLamp Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableMsgNotify Lib "DTWAIN32.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnablePatchDetect Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnablePrinter Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableThumbnail Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EndThread Lib "DTWAIN32.DLL" (ByVal DLLHandle As Integer) As Integer
    Declare Auto Function DTWAIN_EndTwainSession Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_EnumAlarms Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumBitDepths Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumBottomCameras Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Cameras As Integer) As Integer
    Declare Auto Function DTWAIN_EnumBrightnessValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumCameras Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Cameras As Integer) As Integer
    Declare Auto Function DTWAIN_EnumCompressionTypes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumContrastValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumCustomCaps Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumExtImageInfoTypes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumExtendedCaps Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumFileXferFormats Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumHighlightValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumJobControls Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumLightPaths Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef LightPath As Integer) As Integer
    Declare Auto Function DTWAIN_EnumLightSources Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef LightSources As Integer) As Integer
    Declare Auto Function DTWAIN_EnumMaxBuffers Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pMaxBufs As Integer, ByVal bExpandRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumNoiseFilters Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumOCRInterfaces Lib "DTWAIN32.DLL" (ByRef OCRInterfaces As Integer) As Integer
    Declare Auto Function DTWAIN_EnumOCRSupportedCaps Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByRef SupportedCaps As Integer) As Integer
    Declare Auto Function DTWAIN_EnumOrientations Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumOverscanValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPaperSizes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPatchMaxPriorities Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPatchMaxRetries Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPatchPriorities Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPatchSearchModes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPatchTimeOutValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPixelTypes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumPrinterStringModes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumResolutionValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumSourceUnits Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumSources Lib "DTWAIN32.DLL" (ByRef lpArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumSupportedCaps Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumSupportedCapsEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal MaxCustomBase As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_EnumThresholdValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumTopCameras Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Cameras As Integer) As Integer
    Declare Auto Function DTWAIN_EnumTwainPrinters Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpAvailPrinters As Integer) As Integer
    Declare Auto Function DTWAIN_EnumTwainPrintersArray Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_ExecuteOCR Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szFileName As String, ByVal nStartPage As Integer, ByVal nEndPage As Integer) As Integer
    Declare Auto Function DTWAIN_FeedPage Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_FlipBitmap Lib "DTWAIN32.DLL" (ByVal hDib As Integer) As Integer
    Declare Auto Function DTWAIN_FlushAcquiredPages Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_ForceAcquireBitDepth Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal BitDepth As Integer) As Integer
    Declare Auto Function DTWAIN_FrameCreate Lib "DTWAIN32.DLL" (ByVal Left As Double, ByVal Top As Double, ByVal Right As Double, ByVal Bottom As Double) As Integer
    Declare Auto Function DTWAIN_FrameCreateString Lib "DTWAIN32.DLL" (ByVal Left As String, ByVal Top As String, ByVal Right As String, ByVal Bottom As String) As Integer
    Declare Auto Function DTWAIN_FrameDestroy Lib "DTWAIN32.DLL" (ByVal Frame As Integer) As Integer
    Declare Auto Function DTWAIN_FrameGetAll Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByRef Left As Double, ByRef Top As Double, ByRef Right As Double, ByRef Bottom As Double) As Integer
    Declare Auto Function DTWAIN_FrameGetAllString Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByVal Left As String, ByVal Top As String, ByVal Right As String, ByVal Bottom As String) As Integer
    Declare Auto Function DTWAIN_FrameGetValue Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByVal nWhich As Integer, ByRef Value As Double) As Integer
    Declare Auto Function DTWAIN_FrameGetValueString Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByVal nWhich As Integer, ByVal Value As String) As Integer
    Declare Auto Function DTWAIN_FrameIsValid Lib "DTWAIN32.DLL" (ByVal Frame As Integer) As Integer
    Declare Auto Function DTWAIN_FrameSetAll Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByVal Left As Double, ByVal Top As Double, ByVal Right As Double, ByVal Bottom As Double) As Integer
    Declare Auto Function DTWAIN_FrameSetAllString Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByVal Left As String, ByVal Top As String, ByVal Right As String, ByVal Bottom As String) As Integer
    Declare Auto Function DTWAIN_FrameSetValue Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByVal nWhich As Integer, ByVal Value As Double) As Integer
    Declare Auto Function DTWAIN_FrameSetValueString Lib "DTWAIN32.DLL" (ByVal Frame As Integer, ByVal nWhich As Integer, ByVal Value As String) As Integer
    Declare Auto Function DTWAIN_FreeExtImageInfo Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lGetType As Integer, ByRef FloatEnum As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea2 Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef left As Double, ByRef top As Double, ByRef right As Double, ByRef bottom As Double, ByRef lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea2String Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByRef lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireStripBuffer Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireStripData Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpCompression As Integer, ByRef lpBytesPerRow As Integer, ByRef lpColumns As Integer, ByRef lpRows As Integer, ByRef XOffset As Integer, ByRef YOffset As Integer, ByRef lpBytesWritten As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireStripSizes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpMin As Integer, ByRef lpMax As Integer, ByRef lpPreferred As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquiredImage Lib "DTWAIN32.DLL" (ByVal aAcq As Integer, ByVal nWhichAcq As Integer, ByVal nWhichDib As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquiredImageArray Lib "DTWAIN32.DLL" (ByVal aAcq As Integer, ByVal nWhichAcq As Integer) As Integer
    Declare Auto Function DTWAIN_GetAlarmVolume Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpVolume As Integer) As Integer
    Declare Auto Function DTWAIN_GetAppInfo Lib "DTWAIN32.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Auto Function DTWAIN_GetAuthor Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szAuthor As String) As Integer
    Declare Auto Function DTWAIN_GetBatteryMinutes Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpMinutes As Integer) As Integer
    Declare Auto Function DTWAIN_GetBatteryPercent Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpPercent As Integer) As Integer
    Declare Auto Function DTWAIN_GetBitDepth Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef BitDepth As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetBlankPageAutoDetection Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetBrightness Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Brightness As Double) As Integer
    Declare Auto Function DTWAIN_GetBrightnessString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Brightness As String) As Integer
    Declare Auto Function DTWAIN_GetCallback Lib  "DTWAIN32.DLL" () As DTWAINCallback
    Declare Auto Function DTWAIN_GetCallback64 Lib  "DTWAIN32.DLL" () As DTWAINCallback64
    Declare Auto Function DTWAIN_GetCapArrayType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nCap As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapContainer Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nCap As Integer, ByVal lCapType As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapContainerEx Lib "DTWAIN32.DLL" (ByVal nCap As Integer, ByVal bSetContainer As Integer, ByRef ConTypes As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapDataType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nCap As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapFromName Lib "DTWAIN32.DLL" (ByVal szName As String) As Integer
    Declare Auto Function DTWAIN_GetCapOperations Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCapability As Integer, ByRef lpOps As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCap As Integer, ByVal lGetType As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapValuesEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCap As Integer, ByVal lGetType As Integer, ByVal lContainerType As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapValuesEx2 Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCap As Integer, ByVal lGetType As Integer, ByVal lContainerType As Integer, ByVal nDataType As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_GetCaption Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Caption As String) As Integer
    Declare Auto Function DTWAIN_GetCompressionSize Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lBytes As Integer) As Integer
    Declare Auto Function DTWAIN_GetCompressionType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpCompression As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetContrast Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Contrast As Double) As Integer
    Declare Auto Function DTWAIN_GetContrastString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Auto Function DTWAIN_GetCountry Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetCurrentAcquiredImage Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetCurrentFileName Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szName As String, ByVal MaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetCurrentPageNum Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetCurrentRetryCount Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetCustomDSData Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Data As Integer, ByVal dSize As Integer, ByRef pActualSize As Integer, ByVal nFlags As Integer) As Integer
    Declare Auto Function DTWAIN_GetDSMFullName Lib "DTWAIN32.DLL" (ByVal DSMType As Integer, ByVal szDLLName As String, ByVal nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
    Declare Auto Function DTWAIN_GetDSMSearchOrder Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetDTWAINHandle Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetDeviceEvent Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpEvent As Integer) As Integer
    Declare Auto Function DTWAIN_GetDeviceEventEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpEvent As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_GetDeviceNotifications Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef DevEvents As Integer) As Integer
    Declare Auto Function DTWAIN_GetDeviceTimeDate Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Auto Function DTWAIN_GetDuplexType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpDupType As Integer) As Integer
    Declare Auto Function DTWAIN_GetErrorBuffer Lib "DTWAIN32.DLL" (ByRef ArrayBuffer As Integer) As Integer
    Declare Auto Function DTWAIN_GetErrorBufferThreshold Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetErrorString Lib "DTWAIN32.DLL" (ByVal lError As Integer, ByVal lpszBuffer As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetExtCapFromName Lib "DTWAIN32.DLL" (ByVal szName As String) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfo Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfoData Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nWhich As Integer, ByRef Data As Integer) As Integer
    Declare Auto Function DTWAIN_GetExtNameFromCap Lib "DTWAIN32.DLL" (ByVal nValue As Integer, ByVal szValue As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetFeederAlignment Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpAlignment As Integer) As Integer
    Declare Auto Function DTWAIN_GetFeederFuncs Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetFeederOrder Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpOrder As Integer) As Integer
    Declare Auto Function DTWAIN_GetHighlight Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Highlight As Double) As Integer
    Declare Auto Function DTWAIN_GetHighlightString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_GetHighlightStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_GetHighlightStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_GetImageInfo Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpXResolution As Double, ByRef lpYResolution As Double, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Integer, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Auto Function DTWAIN_GetImageInfoString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpXResolution As String, ByVal lpYResolution As String, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Integer, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Auto Function DTWAIN_GetJobControl Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pJobControl As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetJpegValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
    Declare Auto Function DTWAIN_GetLanguage Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetLastError Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetLightPath Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpLightPath As Integer) As Integer
    Declare Auto Function DTWAIN_GetLightSources Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef LightSources As Integer) As Integer
    Declare Auto Function DTWAIN_GetManualDuplexCount Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pSide1 As Integer, ByRef pSide2 As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxAcquisitions Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxBuffers Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pMaxBuf As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxPagesToAcquire Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxRetryAttempts Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetNameFromCap Lib "DTWAIN32.DLL" (ByVal nCapValue As Integer, ByVal szValue As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetNoiseFilter Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpNoiseFilter As Integer) As Integer
    Declare Auto Function DTWAIN_GetNumAcquiredImages Lib "DTWAIN32.DLL" (ByVal aAcq As Integer, ByVal nWhich As Integer) As Integer
    Declare Auto Function DTWAIN_GetNumAcquisitions Lib "DTWAIN32.DLL" (ByVal aAcq As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRCapValues Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal OCRCapValue As Integer, ByVal nGetType As Integer, ByRef CapValues As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRLastError Lib "DTWAIN32.DLL" (ByVal Engine As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRManufacturer Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szManufacturer As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRProductFamily Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szProductFamily As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRProductName Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szProductName As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRText Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal nPageNo As Integer, ByVal Data As String, ByVal dSize As Integer, ByRef pActualSize As Integer, ByVal nFlags As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoFloat Lib "DTWAIN32.DLL" (ByVal OCRTextInfo As Integer, ByVal nCharPos As Integer, ByVal nWhichItem As Integer, ByRef pInfo As Double) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoFloatEx Lib "DTWAIN32.DLL" (ByVal OCRTextInfo As Integer, ByVal nWhichItem As Integer, ByRef pInfo As Double, ByVal bufSize As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoHandle Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal nPageNo As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoLong Lib "DTWAIN32.DLL" (ByVal OCRTextInfo As Integer, ByVal nCharPos As Integer, ByVal nWhichItem As Integer, ByRef pInfo As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoLongEx Lib "DTWAIN32.DLL" (ByVal OCRTextInfo As Integer, ByVal nWhichItem As Integer, ByRef pInfo As Integer, ByVal bufSize As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRVersionInfo Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal buffer As String, ByVal maxBufSize As Integer) As Integer
    Declare Auto Function DTWAIN_GetOrientation Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpOrient As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetOverscan Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpOverscan As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementFloat Lib "DTWAIN32.DLL" (ByVal TextElement As Integer, ByRef val1 As Double, ByRef val2 As Double, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementLong Lib "DTWAIN32.DLL" (ByVal TextElement As Integer, ByRef val1 As Integer, ByRef val2 As Integer, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementString Lib "DTWAIN32.DLL" (ByVal TextElement As Integer, ByVal szData As String, ByVal maxLen As Integer, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFType1FontName Lib "DTWAIN32.DLL" (ByVal FontVal As Integer, ByVal szFont As String, ByVal nChars As Integer) As Integer
    Declare Auto Function DTWAIN_GetPaperSize Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpPaperSize As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchMaxPriorities Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pMaxPriorities As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchMaxRetries Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pMaxRetries As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchPriorities Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef SearchPriorities As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchSearchMode Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pSearchMode As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchTimeOut Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pTimeOut As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPixelFlavor Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpPixelFlavor As Integer) As Integer
    Declare Auto Function DTWAIN_GetPixelType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef PixelType As Integer, ByRef BitDepth As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinter Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpPrinter As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStartNumber Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef nStart As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStringMode Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef PrinterMode As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStrings Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef ArrayString As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterSuffixString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Suffix As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetRegisteredMsg Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetResolution Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Resolution As Double) As Integer
    Declare Auto Function DTWAIN_GetResolutionString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Resolution As String) As Integer
    Declare Auto Function DTWAIN_GetRotation Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Rotation As Double) As Integer
    Declare Auto Function DTWAIN_GetRotationString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Rotation As String) As Integer
    Declare Auto Function DTWAIN_GetSaveFileName Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal fileName As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSaveFileNameA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal fileName As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSaveFileNameW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal fileName As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceAcquisitions Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceID Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceManufacturer Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductFamily Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductName Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceUnit Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionInfo Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionNumber Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef pMajor As Integer, ByRef pMinor As Integer) As Integer
    Declare Auto Function DTWAIN_GetStaticLibVersion Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTempFileDirectory Lib "DTWAIN32.DLL" (ByVal szFilePath As String, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetThreshold Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByRef Threshold As Double) As Integer
    Declare Auto Function DTWAIN_GetThresholdString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Threshold As String) As Integer
    Declare Auto Function DTWAIN_GetThresholdStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Threshold As String) As Integer
    Declare Auto Function DTWAIN_GetThresholdStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Threshold As String) As Integer
    Declare Auto Function DTWAIN_GetTimeDate Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Auto Function DTWAIN_GetTwainAppID Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTwainAvailability Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTwainHwnd Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTwainMode Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTwainTimeout Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_GetVersion Lib "DTWAIN32.DLL" (ByRef lpMajor As Integer, ByRef lpMinor As Integer, ByRef lpVersionType As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionEx Lib "DTWAIN32.DLL" (ByRef lMajor As Integer, ByRef lMinor As Integer, ByRef lVersionType As Integer, ByRef lPatchLevel As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionInfo Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionString Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Auto Function DTWAIN_InitExtImageInfo Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_InitOCRInterface Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsAcquiring Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsAutoBorderDetectEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoBorderDetectSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoBrightEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoBrightSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoDeskewEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoDeskewSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoFeedEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoFeedSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoRotateEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoRotateSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsAutoScanEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsBlankPageDetectionOn Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsCapSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCapability As Integer) As Integer
    Declare Auto Function DTWAIN_IsCompressionSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Compression As Integer) As Integer
    Declare Auto Function DTWAIN_IsCustomDSDataSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsDIBBlank Lib "DTWAIN32.DLL" (ByVal hDib As Integer, ByVal threshold As Double) As Integer
    Declare Auto Function DTWAIN_IsDIBBlankString Lib "DTWAIN32.DLL" (ByVal hDib As Integer, ByVal threshold As String) As Integer
    Declare Auto Function DTWAIN_IsDeviceEventSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsDeviceOnLine Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsDuplexEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsDuplexSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsExtImageInfoSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsFeederEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsFeederLoaded Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsFeederSensitive Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsFeederSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsFileSystemSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsFileXferSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lFileType As Integer) As Integer
    Declare Auto Function DTWAIN_IsIndicatorEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsIndicatorSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsInitialized Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsJPEGSupported Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsJobControlSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal JobControl As Integer) As Integer
    Declare Auto Function DTWAIN_IsLampEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsLampSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsLightPathSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsLightSourceSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsMaxBuffersSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal MaxBuf As Integer) As Integer
    Declare Auto Function DTWAIN_IsMsgNotifyEnabled Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsOCREngineActivated Lib "DTWAIN32.DLL" (ByVal OCREngine As Integer) As Integer
    Declare Auto Function DTWAIN_IsOrientationSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Orientation As Integer) As Integer
    Declare Auto Function DTWAIN_IsOverscanSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal SupportValue As Integer) As Integer
    Declare Auto Function DTWAIN_IsPDFSupported Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsPNGSupported Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsPaperDetectable Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsPaperSizeSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PaperSize As Integer) As Integer
    Declare Auto Function DTWAIN_IsPatchCapsSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsPatchDetectEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsPatchSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PatchCode As Integer) As Integer
    Declare Auto Function DTWAIN_IsPixelTypeSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelType As Integer) As Integer
    Declare Auto Function DTWAIN_IsPrinterEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Printer As Integer) As Integer
    Declare Auto Function DTWAIN_IsPrinterSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsSessionEnabled Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsSkipImageInfoError Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsSourceAcquiring Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsSourceOpen Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsTIFFSupported Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsThumbnailEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsThumbnailSupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsTwainAvailable Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_IsTwainMsg Lib "DTWAIN32.DLL" (ByVal pMsg As WinMsg) As Integer
    Declare Auto Function DTWAIN_IsUIControllable Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsUIEnabled Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_IsUIOnlySupported Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_LoadCustomStringResources Lib "DTWAIN32.DLL" (ByVal sLangDLL As String) As Integer
    Declare Auto Function DTWAIN_LoadLanguageResource Lib "DTWAIN32.DLL" (ByVal nLanguage As Integer) As Integer
    Declare Auto Function DTWAIN_LogMessage Lib "DTWAIN32.DLL" (ByVal message As String) As Integer
    Declare Auto Function DTWAIN_OpenSource Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_OpenSourcesOnSelect Lib "DTWAIN32.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_RangeCreate Lib "DTWAIN32.DLL" (ByVal nEnumType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeCreateFromCap Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCapType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeDestroy Lib "DTWAIN32.DLL" (ByVal pSource As Integer) As Integer
    Declare Auto Function DTWAIN_RangeExpand Lib "DTWAIN32.DLL" (ByVal pSource As Integer, ByRef pArray As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetAllFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByRef pVariantLow As Double, ByRef pVariantUp As Double, ByRef pVariantStep As Double, ByRef pVariantDefault As Double, ByRef pVariantCurrent As Double) As Integer
    Declare Auto Function DTWAIN_RangeGetAllFloatString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal dLow As String, ByVal dUp As String, ByVal dStep As String, ByVal dDefault As String, ByVal dCurrent As String) As Integer
    Declare Auto Function DTWAIN_RangeGetAllLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByRef pVariantLow As Integer, ByRef pVariantUp As Integer, ByRef pVariantStep As Integer, ByRef pVariantDefault As Integer, ByRef pVariantCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetCount Lib "DTWAIN32.DLL" (ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal lPos As Integer, ByRef pVal As Double) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueFloatString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal lPos As Integer, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal lPos As Integer, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As Double, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosFloatString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Value As Integer, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetValueFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhich As Integer, ByRef pVal As Double) As Integer
    Declare Auto Function DTWAIN_RangeGetValueFloatString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhich As Integer, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_RangeGetValueLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhich As Integer, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_RangeIsValid Lib "DTWAIN32.DLL" (ByVal Range As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal dIn As Double, ByRef pOut As Double, ByVal RoundType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueFloatString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal dIn As String, ByVal pOut As String, ByVal RoundType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal lIn As Integer, ByRef pOut As Integer, ByVal RoundType As Integer) As Integer
    Declare Auto Function DTWAIN_RangeSetAllFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal dLow As Double, ByVal dUp As Double, ByVal dStep As Double, ByVal dDefault As Double, ByVal dCurrent As Double) As Integer
    Declare Auto Function DTWAIN_RangeSetAllFloatString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal dLow As String, ByVal dUp As String, ByVal dStep As String, ByVal dDefault As String, ByVal dCurrent As String) As Integer
    Declare Auto Function DTWAIN_RangeSetAllLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal lLow As Integer, ByVal lUp As Integer, ByVal lStep As Integer, ByVal lDefault As Integer, ByVal lCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_RangeSetValueFloat Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhich As Integer, ByVal Val As Double) As Integer
    Declare Auto Function DTWAIN_RangeSetValueFloatString Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhich As Integer, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_RangeSetValueLong Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhich As Integer, ByVal Val As Integer) As Integer
    Declare Auto Function DTWAIN_ResetPDFTextElement Lib "DTWAIN32.DLL" (ByVal TextElement As Integer) As Integer
    Declare Auto Function DTWAIN_RewindPage Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_SelectDefaultOCREngine Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_SelectDefaultSource Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_SelectOCREngine Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_SelectOCREngineByName Lib "DTWAIN32.DLL" (ByVal lpszName As String) As Integer
    Declare Auto Function DTWAIN_SelectSource Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_SelectSource2 Lib "DTWAIN32.DLL" (ByVal hWndParent As IntPtr, ByVal szTitle As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal nOptions As Integer) As Integer
    Declare Auto Function DTWAIN_SelectSourceByName Lib "DTWAIN32.DLL" (ByVal lpszName As String) As Integer
    Declare Auto Function DTWAIN_SetAcquireArea Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lSetType As Integer, ByVal FloatEnum As Integer, ByVal ActualEnum As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireArea2 Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal left As Double, ByVal top As Double, ByVal right As Double, ByVal bottom As Double, ByVal lUnit As Integer, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireArea2String Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByVal lUnit As Integer, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageNegative Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal IsNegative As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageScale Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal xscale As Double, ByVal yscale As Double) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageScaleString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal xscale As String, ByVal yscale As String) As Integer
    Declare Auto Function DTWAIN_SetAcquireStripBuffer Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal hMem As Integer) As Integer
    Declare Auto Function DTWAIN_SetAlarmVolume Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Volume As Integer) As Integer
    Declare Auto Function DTWAIN_SetAlarms Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Alarms As Integer) As Integer
    Declare Auto Function DTWAIN_SetAllCapsToDefault Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_SetAppInfo Lib "DTWAIN32.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Auto Function DTWAIN_SetAuthor Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szAuthor As String) As Integer
    Declare Auto Function DTWAIN_SetAvailablePrinters Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpAvailPrinters As Integer) As Integer
    Declare Auto Function DTWAIN_SetAvailablePrintersArray Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal AvailPrinters As Integer) As Integer
    Declare Auto Function DTWAIN_SetBitDepth Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal BitDepth As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetection Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal threshold As Double, ByVal autodetect_option As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetectionString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal threshold As String, ByVal autodetect_option As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBrightness Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Brightness As Double) As Integer
    Declare Auto Function DTWAIN_SetBrightnessString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Brightness As String) As Integer
    Declare Auto Function DTWAIN_SetCallback Lib "DTWAIN32.DLL" (ByVal Fn As DTWAINCallback, ByVal UserData As Integer) As DTWAINCallback
    Declare Auto Function DTWAIN_SetCallback64 Lib "DTWAIN32.DLL" (ByVal Fn As DTWAINCallback64, ByVal UserData As Long) As DTWAINCallback64
    Declare Auto Function DTWAIN_SetCamera Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szCamera As String) As Integer
    Declare Auto Function DTWAIN_SetCapValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCap As Integer, ByVal lSetType As Integer, ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_SetCapValuesEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCap As Integer, ByVal lSetType As Integer, ByVal lContainerType As Integer, ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_SetCapValuesEx2 Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCap As Integer, ByVal lSetType As Integer, ByVal lContainerType As Integer, ByVal nDataType As Integer, ByVal pArray As Integer) As Integer
    Declare Auto Function DTWAIN_SetCaption Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Caption As String) As Integer
    Declare Auto Function DTWAIN_SetCompressionType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lCompression As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetContrast Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As Double) As Integer
    Declare Auto Function DTWAIN_SetContrastString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Auto Function DTWAIN_SetCountry Lib "DTWAIN32.DLL" (ByVal nCountry As Integer) As Integer
    Declare Auto Function DTWAIN_SetCurrentRetryCount Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nCount As Integer) As Integer
    Declare Auto Function DTWAIN_SetCustomDSData Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal hData As Integer, ByRef Data As Integer, ByVal dSize As Integer, ByVal nFlags As Integer) As Integer
    Declare Auto Function DTWAIN_SetDSMSearchOrder Lib "DTWAIN32.DLL" (ByVal SearchPath As Integer) As Integer
    Declare Auto Function DTWAIN_SetDefaultSource Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_SetDeviceNotifications Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal DevEvents As Integer) As Integer
    Declare Auto Function DTWAIN_SetDeviceTimeDate Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Auto Function DTWAIN_SetEOJDetectValue Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nValue As Integer) As Integer
    Declare Auto Function DTWAIN_SetErrorBufferThreshold Lib "DTWAIN32.DLL" (ByVal nErrors As Integer) As Integer
    Declare Auto Function DTWAIN_SetFeederAlignment Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpAlignment As Integer) As Integer
    Declare Auto Function DTWAIN_SetFeederOrder Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lOrder As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileAutoIncrement Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Increment As Integer, ByVal bResetOnAcquire As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileSavePos Lib "DTWAIN32.DLL" (ByVal hWndParent As IntPtr, ByVal szTitle As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal nFlags As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileXferFormat Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lFileType As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetHighlight Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Highlight As Double) As Integer
    Declare Auto Function DTWAIN_SetHighlightString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_SetHighlightStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_SetHighlightStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_SetJobControl Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal JobControl As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetJpegValues Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Quality As Integer, ByVal Progressive As Integer) As Integer
    Declare Auto Function DTWAIN_SetLanguage Lib "DTWAIN32.DLL" (ByVal nLanguage As Integer) As Integer
    Declare Auto Function DTWAIN_SetLightPath Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal LightPath As Integer) As Integer
    Declare Auto Function DTWAIN_SetLightPathEx Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal LightPaths As Integer) As Integer
    Declare Auto Function DTWAIN_SetLightSources Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal LightSources As Integer) As Integer
    Declare Auto Function DTWAIN_SetManualDuplexMode Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Flags As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetMaxAcquisitions Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal MaxAcquires As Integer) As Integer
    Declare Auto Function DTWAIN_SetMaxBuffers Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal MaxBuf As Integer) As Integer
    Declare Auto Function DTWAIN_SetMaxRetryAttempts Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nAttempts As Integer) As Integer
    Declare Auto Function DTWAIN_SetMultipageScanMode Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal ScanType As Integer) As Integer
    Declare Auto Function DTWAIN_SetNoiseFilter Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal NoiseFilter As Integer) As Integer
    Declare Auto Function DTWAIN_SetOCRCapValues Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal OCRCapValue As Integer, ByVal SetType As Integer, ByVal CapValues As Integer) As Integer
    Declare Auto Function DTWAIN_SetOrientation Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Orient As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetOverscan Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Value As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFASCIICompression Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFAuthor Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpAuthor As String) As Integer
    Declare Auto Function DTWAIN_SetPDFCompression Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bCompression As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFCreator Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpCreator As String) As Integer
    Declare Auto Function DTWAIN_SetPDFEncryption Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bUseEncryption As Integer, ByVal lpszUser As String, ByVal lpszOwner As String, ByVal Permissions As Integer, ByVal UseStrongEncryption As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFJpegQuality Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Quality As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFKeywords Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpKeyWords As String) As Integer
    Declare Auto Function DTWAIN_SetPDFOCRConversion Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal PageType As Integer, ByVal FileType As Integer, ByVal PixelType As Integer, ByVal BitDepth As Integer, ByVal Options As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFOCRMode Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFOrientation Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lPOrientation As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFPageScale Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nOptions As Integer, ByVal xScale As Double, ByVal yScale As Double) As Integer
    Declare Auto Function DTWAIN_SetPDFPageScaleString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nOptions As Integer, ByVal xScale As String, ByVal yScale As String) As Integer
    Declare Auto Function DTWAIN_SetPDFPageSize Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PageSize As Integer, ByVal CustomWidth As Double, ByVal CustomHeight As Double) As Integer
    Declare Auto Function DTWAIN_SetPDFPageSizeString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PageSize As Integer, ByVal CustomWidth As String, ByVal CustomHeight As String) As Integer
    Declare Auto Function DTWAIN_SetPDFProducer Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpProducer As String) As Integer
    Declare Auto Function DTWAIN_SetPDFSubject Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpSubject As String) As Integer
    Declare Auto Function DTWAIN_SetPDFTextElementFloat Lib "DTWAIN32.DLL" (ByVal TextElement As Integer, ByVal val1 As Double, ByVal val2 As Double, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFTextElementLong Lib "DTWAIN32.DLL" (ByVal TextElement As Integer, ByVal val1 As Integer, ByVal val2 As Integer, ByVal Flags As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFTitle Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpTitle As String) As Integer
    Declare Auto Function DTWAIN_SetPaperSize Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PaperSize As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchMaxPriorities Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nMaxSearchRetries As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchMaxRetries Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nMaxRetries As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchPriorities Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal SearchPriorities As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchSearchMode Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nSearchMode As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchTimeOut Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal TimeOutValue As Integer) As Integer
    Declare Auto Function DTWAIN_SetPixelFlavor Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelFlavor As Integer) As Integer
    Declare Auto Function DTWAIN_SetPixelType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PixelType As Integer, ByVal BitDepth As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPostScriptTitle Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTitle As String) As Integer
    Declare Auto Function DTWAIN_SetPostScriptType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PSType As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinter Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Printer As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStartNumber Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nStart As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStringMode Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PrinterMode As Integer, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStrings Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal ArrayString As Integer, ByRef pNumStrings As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterSuffixString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Suffix As String) As Integer
    Declare Auto Function DTWAIN_SetQueryCapSupport Lib "DTWAIN32.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetResolution Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Resolution As Double) As Integer
    Declare Auto Function DTWAIN_SetResolutionString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Resolution As String) As Integer
    Declare Auto Function DTWAIN_SetRotation Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Rotation As Double) As Integer
    Declare Auto Function DTWAIN_SetRotationString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Rotation As String) As Integer
    Declare Auto Function DTWAIN_SetSaveFileName Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal fileName As String) As Integer
    Declare Auto Function DTWAIN_SetSaveFileNameA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal fileName As String) As Integer
    Declare Auto Function DTWAIN_SetSaveFileNameW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal fileName As String) As Integer
    Declare Auto Function DTWAIN_SetSourceUnit Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_SetTIFFCompressType Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Setting As Integer) As Integer
    Declare Auto Function DTWAIN_SetTIFFInvert Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Setting As Integer) As Integer
    Declare Auto Function DTWAIN_SetTempFileDirectory Lib "DTWAIN32.DLL" (ByVal szFilePath As String) As Integer
    Declare Auto Function DTWAIN_SetThreshold Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Threshold As Double, ByVal bSetBithDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetThresholdString Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Threshold As String, ByVal bSetBitDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetThresholdStringA Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Threshold As String, ByVal bSetBitDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetThresholdStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Threshold As String, ByVal bSetBitDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainDialogFont Lib "DTWAIN32.DLL" (ByVal hFont As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainDSM Lib "DTWAIN32.DLL" (ByVal DSMType As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainLog Lib "DTWAIN32.DLL" (ByVal LogFlags As Integer, ByVal lpszLogFile As String) As Integer
    Declare Auto Function DTWAIN_SetTwainMode Lib "DTWAIN32.DLL" (ByVal lAcquireMode As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainTimeout Lib "DTWAIN32.DLL" (ByVal milliseconds As Integer) As Integer
    Declare Auto Function DTWAIN_SetUpdateDibProc Lib "DTWAIN32.DLL" (ByVal DibProc As DTWAINDibUpdateCallback) As DTWAINDibUpdateCallback
    Declare Auto Function DTWAIN_ShowUIOnly Lib "DTWAIN32.DLL" (ByVal Source As Integer) As Integer
    Declare Auto Function DTWAIN_ShutdownOCREngine Lib "DTWAIN32.DLL" (ByVal OCREngine As Integer) As Integer
    Declare Auto Function DTWAIN_SkipImageInfoError Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bSkip As Integer) As Integer
    Declare Auto Function DTWAIN_StartThread Lib "DTWAIN32.DLL" (ByVal DLLHandle As Integer) As Integer
    Declare Auto Function DTWAIN_StartTwainSession Lib "DTWAIN32.DLL" (ByVal hWndMsg As IntPtr, ByVal lpszDLLName As String) As Integer
    Declare Auto Function DTWAIN_SysDestroy Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_SysInitialize Lib  "DTWAIN32.DLL" () As Integer
    Declare Auto Function DTWAIN_SysInitializeEx Lib "DTWAIN32.DLL" (ByVal szINIPath As String) As Integer
    Declare Auto Function DTWAIN_SysInitializeEx2 Lib "DTWAIN32.DLL" (ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Integer
    Declare Auto Function DTWAIN_SysInitializeLib Lib "DTWAIN32.DLL" (ByVal hInstance As Integer) As Integer
    Declare Auto Function DTWAIN_SysInitializeLibEx Lib "DTWAIN32.DLL" (ByVal hInstance As Integer, ByVal szINIPath As String) As Integer
    Declare Auto Function DTWAIN_SysInitializeLibEx2 Lib "DTWAIN32.DLL" (ByVal hInstance As Integer, ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Integer
    Declare Auto Function DTWAIN_TwainSave Lib "DTWAIN32.DLL" (ByVal cmd As String) As Integer
    Declare Auto Sub DTWAIN_X Lib "DTWAIN32.DLL" (ByVal s As String) 
    Declare Unicode Function DTWAIN_AcquireFileW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpszFile As String, ByVal lFileType As Integer, ByVal lFileFlags As Integer, ByVal PixelType As Integer, ByVal lMaxPages As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Unicode Function DTWAIN_AddPDFTextW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szText As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal fontName As String, ByVal fontSize As Double, ByVal colorRGB As Integer, ByVal renderMode As Integer, ByVal scaling As Double, ByVal charSpacing As Double, ByVal wordSpacing As Double, ByVal strokeWidth As Integer, ByVal Flags As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayAddStringNW Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayAddStringW Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal Val As String) As Integer
    Declare Unicode Function DTWAIN_ArrayFindStringW Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal pString As String) As Integer
    Declare Unicode Function DTWAIN_ArrayGetAtStringW Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtStringNW Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal Val As String, ByVal num As Integer) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtStringW Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pVal As String) As Integer
    Declare Unicode Function DTWAIN_ArraySetAtStringW Lib "DTWAIN32.DLL" (ByVal pArray As Integer, ByVal nWhere As Integer, ByVal pStr As String) As Integer
    Declare Unicode Function DTWAIN_ExecuteOCRW Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szFileName As String, ByVal nStartPage As Integer, ByVal nEndPage As Integer) As Integer
    Declare Unicode Function DTWAIN_GetAcquireArea2StringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByRef lpUnit As Integer) As Integer
    Declare Unicode Function DTWAIN_GetAppInfoW Lib "DTWAIN32.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Unicode Function DTWAIN_GetAuthorW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szAuthor As String) As Integer
    Declare Unicode Function DTWAIN_GetBrightnessStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_GetCapFromNameW Lib "DTWAIN32.DLL" (ByVal szName As String) As Integer
    Declare Unicode Function DTWAIN_GetCaptionW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Caption As String) As Integer
    Declare Unicode Function DTWAIN_GetContrastStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_GetCurrentFileNameW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szName As String, ByVal MaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetDSMFullNameW Lib "DTWAIN32.DLL" (ByVal DSMType As Integer, ByVal szDLLName As String, ByVal nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
    Declare Unicode Function DTWAIN_GetDeviceTimeDateW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Unicode Function DTWAIN_GetErrorStringW Lib "DTWAIN32.DLL" (ByVal lError As Integer, ByVal lpszBuffer As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetExtCapFromNameW Lib "DTWAIN32.DLL" (ByVal szName As String) As Integer
    Declare Unicode Function DTWAIN_GetExtNameFromCapW Lib "DTWAIN32.DLL" (ByVal nValue As Integer, ByVal szValue As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetImageInfoStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpXResolution As String, ByVal lpYResolution As String, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Integer, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Unicode Function DTWAIN_GetNameFromCapW Lib "DTWAIN32.DLL" (ByVal nCapValue As Integer, ByVal szValue As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRManufacturerW Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szManufacturer As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRProductFamilyW Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szProductFamily As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRProductNameW Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal szProductName As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRTextW Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal nPageNo As Integer, ByVal Data As String, ByVal dSize As Integer, ByRef pActualSize As Integer, ByVal nFlags As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRVersionInfoW Lib "DTWAIN32.DLL" (ByVal Engine As Integer, ByVal buffer As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetPDFTextElementStringW Lib "DTWAIN32.DLL" (ByVal TextElement As Integer, ByVal szData As String, ByVal maxLen As Integer, ByVal Flags As Integer) As Integer
    Declare Unicode Function DTWAIN_GetPDFType1FontNameW Lib "DTWAIN32.DLL" (ByVal FontVal As Integer, ByVal szFont As String, ByVal nChars As Integer) As Integer
    Declare Unicode Function DTWAIN_GetPrinterSuffixStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Suffix As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetResolutionStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Resolution As String) As Integer
    Declare Unicode Function DTWAIN_GetRotationStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Rotation As String) As Integer
    Declare Unicode Function DTWAIN_GetSourceManufacturerW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductFamilyW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductNameW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceVersionInfoW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szProduct As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTempFileDirectoryW Lib "DTWAIN32.DLL" (ByVal szFilePath As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTimeDateW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Unicode Function DTWAIN_GetVersionInfoW Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionStringW Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_IsDIBBlankStringW Lib "DTWAIN32.DLL" (ByVal hDib As Integer, ByVal threshold As String) As Integer
    Declare Unicode Function DTWAIN_LoadCustomStringResourcesW Lib "DTWAIN32.DLL" (ByVal sLangDLL As String) As Integer
    Declare Unicode Function DTWAIN_LogMessageW Lib "DTWAIN32.DLL" (ByVal message As String) As Integer
    Declare Unicode Function DTWAIN_SelectOCREngineByNameW Lib "DTWAIN32.DLL" (ByVal lpszName As String) As Integer
    Declare Unicode Function DTWAIN_SelectSource2W Lib "DTWAIN32.DLL" (ByVal hWndParent As IntPtr, ByVal szTitle As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal nOptions As Integer) As Integer
    Declare Unicode Function DTWAIN_SelectSourceByNameW Lib "DTWAIN32.DLL" (ByVal lpszName As String) As Integer
    Declare Unicode Function DTWAIN_SetAcquireArea2StringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByVal lUnit As Integer, ByVal Flags As Integer) As Integer
    Declare Unicode Function DTWAIN_SetAcquireImageScaleStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal xscale As String, ByVal yscale As String) As Integer
    Declare Unicode Function DTWAIN_SetAppInfoW Lib "DTWAIN32.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Unicode Function DTWAIN_SetAuthorW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szAuthor As String) As Integer
    Declare Unicode Function DTWAIN_SetBlankPageDetectionStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal threshold As String, ByVal autodetect_option As Integer, ByVal bSet As Integer) As Integer
    Declare Unicode Function DTWAIN_SetBrightnessStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_SetCameraW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szCamera As String) As Integer
    Declare Unicode Function DTWAIN_SetCaptionW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Caption As String) As Integer
    Declare Unicode Function DTWAIN_SetContrastStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_SetDeviceTimeDateW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTimeDate As String) As Integer
    Declare Unicode Function DTWAIN_SetFileSavePosW Lib "DTWAIN32.DLL" (ByVal hWndParent As IntPtr, ByVal szTitle As String, ByVal xPos As Integer, ByVal yPos As Integer, ByVal nFlags As Integer) As Integer
    Declare Unicode Function DTWAIN_SetPDFAuthorW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpAuthor As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFCreatorW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpCreator As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFEncryptionW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal bUseEncryption As Integer, ByVal lpszUser As String, ByVal lpszOwner As String, ByVal Permissions As Integer, ByVal UseStrongEncryption As Integer) As Integer
    Declare Unicode Function DTWAIN_SetPDFKeywordsW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpKeyWords As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFPageScaleStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal nOptions As Integer, ByVal xScale As String, ByVal yScale As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFPageSizeStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal PageSize As Integer, ByVal CustomWidth As String, ByVal CustomHeight As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFProducerW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpProducer As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFSubjectW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpSubject As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFTitleW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal lpTitle As String) As Integer
    Declare Unicode Function DTWAIN_SetPostScriptTitleW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal szTitle As String) As Integer
    Declare Unicode Function DTWAIN_SetPrinterSuffixStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Suffix As String) As Integer
    Declare Unicode Function DTWAIN_SetResolutionStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Resolution As String) As Integer
    Declare Unicode Function DTWAIN_SetRotationStringW Lib "DTWAIN32.DLL" (ByVal Source As Integer, ByVal Rotation As String) As Integer
    Declare Unicode Function DTWAIN_SetTempFileDirectoryW Lib "DTWAIN32.DLL" (ByVal szFilePath As String) As Integer
    Declare Unicode Function DTWAIN_SetTwainLogW Lib "DTWAIN32.DLL" (ByVal LogFlags As Integer, ByVal lpszLogFile As String) As Integer
    Declare Unicode Function DTWAIN_StartTwainSessionW Lib "DTWAIN32.DLL" (ByVal hWndMsg As IntPtr, ByVal lpszDLLName As String) As Integer
    Declare Unicode Function DTWAIN_SysInitializeEx2W Lib "DTWAIN32.DLL" (ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Integer
    Declare Unicode Function DTWAIN_SysInitializeExW Lib "DTWAIN32.DLL" (ByVal szINIPath As String) As Integer
    Declare Unicode Function DTWAIN_SysInitializeLibEx2W Lib "DTWAIN32.DLL" (ByVal hInstance As Integer, ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Integer
    Declare Unicode Function DTWAIN_SysInitializeLibExW Lib "DTWAIN32.DLL" (ByVal hInstance As Integer, ByVal szINIPath As String) As Integer
    Declare Unicode Function DTWAIN_TwainSaveW Lib "DTWAIN32.DLL" (ByVal cmd As String) As Integer
    Declare Unicode Sub DTWAIN_XW Lib "DTWAIN32.DLL" (ByVal s As String) 
    Declare Auto Function DTWAIN_GetNameFromCap Lib "DTWAIN32.DLL" (ByVal nCapValue As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetTempFileDirectory Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetErrorString Lib "DTWAIN32.DLL" (ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionString Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionInfo Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceManufacturer Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductFamily Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductName Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionInfo Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRErrorString Lib "DTWAIN32.DLL" (ByVal nEngine As Integer, ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetDSMFullName Lib "DTWAIN32.DLL" (ByVal DSMType As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer, ByRef pStatus As Integer) As Integer
    Declare Ansi Function DTWAIN_GetNameFromCapA Lib "DTWAIN32.DLL" (ByVal nCapValue As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTempFileDirectoryA Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetErrorStringA Lib "DTWAIN32.DLL" (ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionStringA Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionInfoA Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceManufacturerA Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductFamilyA Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductNameA Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceVersionInfoA Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRErrorStringA Lib "DTWAIN32.DLL" (ByVal nEngine As Integer, ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetDSMFullNameA Lib "DTWAIN32.DLL" (ByVal DSMType As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer, ByRef pStatus As Integer) As Integer
    Declare Unicode Function DTWAIN_GetNameFromCapW Lib "DTWAIN32.DLL" (ByVal nCapValue As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTempFileDirectoryW Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetErrorStringW Lib "DTWAIN32.DLL" (ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionStringW Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionInfoW Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceManufacturerW Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductFamilyW Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductNameW Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceVersionInfoW Lib "DTWAIN32.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRErrorStringW Lib "DTWAIN32.DLL" (ByVal nEngine As Integer, ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetDSMFullNameW Lib "DTWAIN32.DLL" (ByVal DSMType As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_StartTwainSession Lib "DTWAIN32.DLL" (ByVal hWndMsg As IntPtr, ByVal szValue As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_StartTwainSessionA Lib "DTWAIN32.DLL" (ByVal hWndMsg As IntPtr, ByVal szValue As System.IntPtr) As Integer
    Declare Unicode Function DTWAIN_StartTwainSessionW Lib "DTWAIN32.DLL" (ByVal hWndMsg As IntPtr, ByVal szValue As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_GetShortVersionStringA Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetShortVersionString Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Unicode Function DTWAIN_GetShortVersionStringW Lib "DTWAIN32.DLL" (ByVal lpszVer As String, ByVal nLength As Integer) As Integer
    Declare Auto Function DTWAIN_GetShortVersionString Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetShortVersionStringA Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetShortVersionStringW Lib "DTWAIN32.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer

End Class
