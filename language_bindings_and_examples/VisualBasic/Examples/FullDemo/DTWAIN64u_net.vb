REM
REM ** Copyright (c) 2001-2012  DynaRithmic Software
REM
REM ** Permission to use, copy, modify and distribute this file for any purpose is hereby
REM ** granted without fee, provided that (i) the above copyright notices and
REM ** this permission notice appear in all copies of the software and related
REM ** documentation, and (ii) the name of DynaRithmic Software may not be used
REM ** in any advertising or publicity relating to the software without the
REM ** specific, prior written permission of DynaRithmic Software.
REM
REM ** THE SOFTWARE IS PROVIDED "AS-IS" AND WITHOUT WARRANTY OF ANY KIND,
REM ** EXPRESS, IMPLIED OR OTHERWISE, INCLUDING WITHOUT LIMITATION, ANY
REM ** WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
REM
REM ** IN NO EVENT SHALL DYNARITHMIC SOFTWARE BE LIABLE FOR ANY SPECIAL, INCIDENTAL,
REM ** INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY KIND, OR ANY DAMAGES WHATSOEVER
REM ** RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER OR NOT ADVISED OF THE
REM ** POSSIBILITY OF DAMAGE, AND ON ANY THEORY OF LIABILITY, ARISING OUT OF OR IN
REM ** CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
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
    Public Const DTWAIN_TWCT_PATCHT As Integer  = 5
    Public Const DTWAIN_TWCT_PATCH6 As Integer  = 6
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
    Public Const DTWAIN_CV_ACAPAUDIOFILEFORMAT As Integer  = &H1201
    Public Const DTWAIN_CV_ACAPXFERMECH As Integer  = &H1202
    Public Const DTWAIN_CV_CAPPRINTERCHARROTATION As Integer  = &H1047
    Public Const DTWAIN_CV_CAPPRINTERFONTSTYLE As Integer  = &H1048
    Public Const DTWAIN_CV_CAPPRINTERINDEXLEADCHAR As Integer  = &H1049
    Public Const DTWAIN_CV_CAPPRINTERINDEXMAXVALUE As Integer  = &H104A
    Public Const DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS As Integer  = &H104B
    Public Const DTWAIN_CV_CAPPRINTERINDEXSTEP As Integer  = &H104C
    Public Const DTWAIN_CV_CAPPRINTERINDEXTRIGGER As Integer  = &H104D
    Public Const DTWAIN_CV_CAPPRINTERSTRINGPREVIEW As Integer  = &H104E
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

    Declare Ansi Function DTWAIN_AcquireFileA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpszFile As String, ByVal lFileType As Long, ByVal lFileFlags As Long, ByVal PixelType As Long, ByVal lMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Ansi Function DTWAIN_AddPDFTextA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szText As String, ByVal xPos As Long, ByVal yPos As Long, ByVal fontName As String, ByVal fontSize As Double, ByVal colorRGB As Long, ByVal renderMode As Long, ByVal scaling As Double, ByVal charSpacing As Double, ByVal wordSpacing As Double, ByVal strokeWidth As Long, ByVal Flags As Long) As Integer
    Declare Ansi Function DTWAIN_ArrayAddStringA Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String) As Integer
    Declare Ansi Function DTWAIN_ArrayAddStringNA Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Ansi Function DTWAIN_ArrayFindStringA Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal pString As String) As Integer
    Declare Ansi Function DTWAIN_ArrayGetAtStringA Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtStringA Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As String) As Integer
    Declare Ansi Function DTWAIN_ArrayInsertAtStringNA Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Ansi Function DTWAIN_ArraySetAtStringA Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Ansi Function DTWAIN_ExecuteOCRA Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szFileName As String, ByVal nStartPage As Long, ByVal nEndPage As Long) As Integer
    Declare Ansi Function DTWAIN_GetAcquireArea2StringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByRef Unit As Integer) As Integer
    Declare Ansi Function DTWAIN_GetAppInfoA Lib "DTWAIN64U.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Ansi Function DTWAIN_GetAuthorA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szAuthor As String) As Integer
    Declare Ansi Function DTWAIN_GetBrightnessStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_GetCapFromNameA Lib "DTWAIN64U.DLL" (ByVal szName As String) As Integer
    Declare Ansi Function DTWAIN_GetCaptionA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Caption As String) As Integer
    Declare Ansi Function DTWAIN_GetContrastStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_GetCurrentFileNameA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szName As String, ByVal MaxLen As Long) As Integer
    Declare Ansi Function DTWAIN_GetDSMFullNameA Lib "DTWAIN64U.DLL" (ByVal DSMType As Long, ByVal szDLLName As String, ByVal nMaxLen As Long, ByRef pWhichSearch As Integer) As Integer
    Declare Ansi Function DTWAIN_GetDeviceTimeDateA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Ansi Function DTWAIN_GetErrorStringA Lib "DTWAIN64U.DLL" (ByVal lError As Long, ByVal lpszBuffer As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetExtCapFromNameA Lib "DTWAIN64U.DLL" (ByVal szName As String) As Integer
    Declare Ansi Function DTWAIN_GetExtNameFromCapA Lib "DTWAIN64U.DLL" (ByVal nValue As Long, ByVal szValue As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetImageInfoStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpXResolution As String, ByVal lpYResolution As String, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Long, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Ansi Function DTWAIN_GetNameFromCapA Lib "DTWAIN64U.DLL" (ByVal nCapValue As Long, ByVal szValue As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetOCRManufacturerA Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szManufacturer As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetOCRProductFamilyA Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szProductFamily As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetOCRProductNameA Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szProductName As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetOCRTextA Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal nPageNo As Long, ByVal Data As String, ByVal dSize As Long, ByRef pActualSize As Integer, ByVal nFlags As Long) As Long
    Declare Ansi Function DTWAIN_GetOCRVersionInfoA Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal buffer As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetPDFTextElementStringA Lib "DTWAIN64U.DLL" (ByVal TextElement As Long, ByVal szData As String, ByVal maxLen As Long, ByVal Flags As Long) As Integer
    Declare Ansi Function DTWAIN_GetPDFType1FontNameA Lib "DTWAIN64U.DLL" (ByVal FontVal As Long, ByVal szFont As String, ByVal nChars As Long) As Integer
    Declare Ansi Function DTWAIN_GetPrinterSuffixStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Suffix As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetResolutionStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Resolution As String) As Integer
    Declare Ansi Function DTWAIN_GetRotationStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Rotation As String) As Integer
    Declare Ansi Function DTWAIN_GetSourceManufacturerA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductFamilyA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductNameA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetSourceVersionInfoA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetTempFileDirectoryA Lib "DTWAIN64U.DLL" (ByVal szFilePath As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetTimeDateA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Ansi Function DTWAIN_GetVersionInfoA Lib "DTWAIN64U.DLL" (ByVal lpszVer As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_GetVersionStringA Lib "DTWAIN64U.DLL" (ByVal lpszVer As String, ByVal nLength As Long) As Integer
    Declare Ansi Function DTWAIN_IsDIBBlankStringA Lib "DTWAIN64U.DLL" (ByVal hDib As Long, ByVal threshold As String) As Integer
    Declare Ansi Function DTWAIN_LoadCustomStringResourcesA Lib "DTWAIN64U.DLL" (ByVal sLangDLL As String) As Integer
    Declare Ansi Function DTWAIN_LogMessageA Lib "DTWAIN64U.DLL" (ByVal message As String) As Integer
    Declare Ansi Function DTWAIN_SelectOCREngineByNameA Lib "DTWAIN64U.DLL" (ByVal lpszName As String) As Long
    Declare Ansi Function DTWAIN_SelectSource2A Lib "DTWAIN64U.DLL" (ByVal hWndParent As LongPtr, ByVal szTitle As String, ByVal xPos As Long, ByVal yPos As Long, ByVal nOptions As Long) As Long
    Declare Ansi Function DTWAIN_SelectSourceByNameA Lib "DTWAIN64U.DLL" (ByVal lpszName As String) As Long
    Declare Ansi Function DTWAIN_SetAcquireArea2StringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByVal lUnit As Long, ByVal Flags As Long) As Integer
    Declare Ansi Function DTWAIN_SetAcquireImageScaleStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal xscale As String, ByVal yscale As String) As Integer
    Declare Ansi Function DTWAIN_SetAppInfoA Lib "DTWAIN64U.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Ansi Function DTWAIN_SetAuthorA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szAuthor As String) As Integer
    Declare Ansi Function DTWAIN_SetBlankPageDetectionStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal threshold As String, ByVal autodetect_option As Long, ByVal bSet As Integer) As Integer
    Declare Ansi Function DTWAIN_SetBrightnessStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_SetCameraA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szCamera As String) As Integer
    Declare Ansi Function DTWAIN_SetCaptionA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Caption As String) As Integer
    Declare Ansi Function DTWAIN_SetContrastStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Ansi Function DTWAIN_SetDeviceTimeDateA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Ansi Function DTWAIN_SetFileSavePosA Lib "DTWAIN64U.DLL" (ByVal hWndParent As LongPtr, ByVal szTitle As String, ByVal xPos As Long, ByVal yPos As Long, ByVal nFlags As Long) As Integer
    Declare Ansi Function DTWAIN_SetPDFAuthorA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpAuthor As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFCreatorA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpCreator As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFEncryptionA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bUseEncryption As Integer, ByVal lpszUser As String, ByVal lpszOwner As String, ByVal Permissions As Long, ByVal UseStrongEncryption As Integer) As Integer
    Declare Ansi Function DTWAIN_SetPDFKeywordsA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpKeyWords As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFPageScaleStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nOptions As Long, ByVal xScale As String, ByVal yScale As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFPageSizeStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PageSize As Long, ByVal CustomWidth As String, ByVal CustomHeight As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFProducerA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpProducer As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFSubjectA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpSubject As String) As Integer
    Declare Ansi Function DTWAIN_SetPDFTitleA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpTitle As String) As Integer
    Declare Ansi Function DTWAIN_SetPostScriptTitleA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTitle As String) As Integer
    Declare Ansi Function DTWAIN_SetPrinterSuffixStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Suffix As String) As Integer
    Declare Ansi Function DTWAIN_SetResolutionStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Resolution As String) As Integer
    Declare Ansi Function DTWAIN_SetRotationStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Rotation As String) As Integer
    Declare Ansi Function DTWAIN_SetTempFileDirectoryA Lib "DTWAIN64U.DLL" (ByVal szFilePath As String) As Integer
    Declare Ansi Function DTWAIN_SetTwainLogA Lib "DTWAIN64U.DLL" (ByVal LogFlags As Long, ByVal lpszLogFile As String) As Integer
    Declare Ansi Function DTWAIN_StartTwainSessionA Lib "DTWAIN64U.DLL" (ByVal hWndMsg As LongPtr, ByVal lpszDLLName As String) As Integer
    Declare Ansi Function DTWAIN_SysInitializeEx2A Lib "DTWAIN64U.DLL" (ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Long
    Declare Ansi Function DTWAIN_SysInitializeExA Lib "DTWAIN64U.DLL" (ByVal szINIPath As String) As Long
    Declare Ansi Function DTWAIN_SysInitializeLibEx2A Lib "DTWAIN64U.DLL" (ByVal hInstance As Long, ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Long
    Declare Ansi Function DTWAIN_SysInitializeLibExA Lib "DTWAIN64U.DLL" (ByVal hInstance As Long, ByVal szINIPath As String) As Long
    Declare Ansi Function DTWAIN_TwainSaveA Lib "DTWAIN64U.DLL" (ByVal cmd As String) As Integer
    Declare Ansi Sub DTWAIN_XA Lib "DTWAIN64U.DLL" (ByVal s As String) 
    Declare Auto Function DTWAIN_AcquireBuffered Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelType As Long, ByVal nMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Long
    Declare Auto Function DTWAIN_AcquireBufferedEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelType As Long, ByVal nMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByVal Acquisitions As Long, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireFile Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpszFile As String, ByVal lFileType As Long, ByVal lFileFlags As Long, ByVal PixelType As Long, ByVal lMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireFileEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal aFileNames As Long, ByVal lFileType As Long, ByVal lFileFlags As Long, ByVal PixelType As Long, ByVal lMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireNative Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelType As Long, ByVal nMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Long
    Declare Auto Function DTWAIN_AcquireNativeEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelType As Long, ByVal nMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByVal Acquisitions As Long, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_AcquireToClipboard Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelType As Long, ByVal nMaxPages As Long, ByVal nTransferMode As Long, ByVal bDiscardDibs As Integer, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Long
    Declare Auto Function DTWAIN_AddPDFText Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szText As String, ByVal xPos As Long, ByVal yPos As Long, ByVal fontName As String, ByVal fontSize As Double, ByVal colorRGB As Long, ByVal renderMode As Long, ByVal scaling As Double, ByVal charSpacing As Double, ByVal wordSpacing As Double, ByVal strokeWidth As Long, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_AppHandlesExceptions Lib "DTWAIN64U.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayAddANSIString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddANSIStringN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As Double) As Integer
    Declare Auto Function DTWAIN_ArrayAddFloatN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As Double, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayAddLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As Long) As Integer
    Declare Auto Function DTWAIN_ArrayAddLongN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As Long, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayAddString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddStringN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayAddWideString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_ArrayAddWideStringN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayConvertFix32ToFloat Lib "DTWAIN64U.DLL" (ByVal Fix32Array As Long) As Long
    Declare Auto Function DTWAIN_ArrayConvertFloatToFix32 Lib "DTWAIN64U.DLL" (ByVal FloatArray As Long) As Long
    Declare Auto Function DTWAIN_ArrayCopy Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Dest As Long) As Integer
    Declare Auto Function DTWAIN_ArrayCreate Lib "DTWAIN64U.DLL" (ByVal nEnumType As Long, ByVal nInitialSize As Long) As Long
    Declare Auto Function DTWAIN_ArrayCreateCopy Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Long
    Declare Auto Function DTWAIN_ArrayCreateFromCap Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCapType As Long, ByVal lSize As Long) As Long
    Declare Auto Function DTWAIN_ArrayCreateFromLongs Lib "DTWAIN64U.DLL" (ByRef pCArray As Integer, ByVal nSize As Long) As Long
    Declare Auto Function DTWAIN_ArrayDestroy Lib "DTWAIN64U.DLL" (ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_ArrayDestroyFrames Lib "DTWAIN64U.DLL" (ByVal FrameArray As Long) As Integer
    Declare Auto Function DTWAIN_ArrayFindANSIString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFindFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As Double, ByVal Tolerance As Double) As Integer
    Declare Auto Function DTWAIN_ArrayFindLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As Long) As Integer
    Declare Auto Function DTWAIN_ArrayFindString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFindWideString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal pString As String) As Integer
    Declare Auto Function DTWAIN_ArrayFrameGetFrameAt Lib "DTWAIN64U.DLL" (ByVal FrameArray As Long, ByVal nWhere As Long) As Long
    Declare Auto Function DTWAIN_ArrayGetAtANSIString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByRef pVal As Double) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArrayGetAtWideString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArrayGetBuffer Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nPos As Long) As Long
    Declare Auto Function DTWAIN_ArrayGetCount Lib "DTWAIN64U.DLL" (ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_ArrayGetMaxStringLength Lib "DTWAIN64U.DLL" (ByVal a As Long) As Integer
    Declare Auto Function DTWAIN_ArrayGetStringLength Lib "DTWAIN64U.DLL" (ByVal a As Long, ByVal nWhichString As Long) As Integer
    Declare Auto Function DTWAIN_ArrayGetType Lib "DTWAIN64U.DLL" (ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtANSIString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtANSIStringN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As Double) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtFloatN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal Val As Double, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As Long) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtLongN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As Long, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtStringN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtWideString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_ArrayInsertAtWideStringN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAll Lib "DTWAIN64U.DLL" (ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAt Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long) As Integer
    Declare Auto Function DTWAIN_ArrayRemoveAtN Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal num As Long) As Integer
    Declare Auto Function DTWAIN_ArrayResize Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal NewSize As Long) As Integer
    Declare Auto Function DTWAIN_ArraySetAtANSIString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArraySetAtFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As Double) As Integer
    Declare Auto Function DTWAIN_ArraySetAtLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As Long) As Integer
    Declare Auto Function DTWAIN_ArraySetAtString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_ArraySetAtWideString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Auto Function DTWAIN_CallCallback Lib "DTWAIN64U.DLL" (ByVal wParam As Long, ByVal lParam As Long, ByVal UserData As Long) As Integer
    Declare Auto Function DTWAIN_ClearBuffers Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal ClearBuffer As Long) As Integer
    Declare Auto Function DTWAIN_ClearErrorBuffer Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_ClearPDFText Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_ClearPage Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_CloseSource Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_CloseSourceUI Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_CreateAcquisitionArray Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_CreatePDFTextElement Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Long
    Declare Auto Function DTWAIN_DestroyAcquisitionArray Lib "DTWAIN64U.DLL" (ByVal aAcq As Long, ByVal bDestroyData As Integer) As Integer
    Declare Auto Function DTWAIN_DestroyPDFTextElement Lib "DTWAIN64U.DLL" (ByVal TextElement As Long) As Integer
    Declare Auto Function DTWAIN_DisableAppWindow Lib "DTWAIN64U.DLL" (ByVal hWnd As LongPtr, ByVal bDisable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoBorderDetect Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoBright Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoDeskew Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoFeed Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoRotate Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableAutoScan Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableDuplex Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableFeeder Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableIndicator Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableJobFileHandling Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnableLamp Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableMsgNotify Lib "DTWAIN64U.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_EnablePatchDetect Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnablePrinter Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EnableThumbnail Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bEnable As Integer) As Integer
    Declare Auto Function DTWAIN_EndThread Lib "DTWAIN64U.DLL" (ByVal DLLHandle As Long) As Integer
    Declare Auto Function DTWAIN_EndTwainSession Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_EnumAlarms Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumBitDepths Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumBottomCameras Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Cameras As Long) As Integer
    Declare Auto Function DTWAIN_EnumBrightnessValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumCameras Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Cameras As Long) As Integer
    Declare Auto Function DTWAIN_EnumCompressionTypes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumContrastValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumCustomCaps Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumExtImageInfoTypes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumExtendedCaps Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumFileXferFormats Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumHighlightValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumJobControls Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumLightPaths Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef LightPath As Long) As Integer
    Declare Auto Function DTWAIN_EnumLightSources Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef LightSources As Long) As Integer
    Declare Auto Function DTWAIN_EnumMaxBuffers Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pMaxBufs As Long, ByVal bExpandRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumNoiseFilters Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumOCRInterfaces Lib "DTWAIN64U.DLL" (ByRef OCRInterfaces As Long) As Integer
    Declare Auto Function DTWAIN_EnumOCRSupportedCaps Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByRef SupportedCaps As Long) As Integer
    Declare Auto Function DTWAIN_EnumOrientations Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPaperSizes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPatchMaxPriorities Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPatchMaxRetries Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPatchPriorities Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPatchSearchModes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPatchTimeOutValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPixelTypes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumPrinterStringModes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumResolutionValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumSourceUnits Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumSources Lib "DTWAIN64U.DLL" (ByRef lpArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumSupportedCaps Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumSupportedCapsEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal MaxCustomBase As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_EnumThresholdValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long, ByVal bExpandIfRange As Integer) As Integer
    Declare Auto Function DTWAIN_EnumTopCameras Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Cameras As Long) As Integer
    Declare Auto Function DTWAIN_EnumTwainPrinters Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpAvailPrinters As Integer) As Integer
    Declare Auto Function DTWAIN_EnumTwainPrintersArray Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_ExecuteOCR Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szFileName As String, ByVal nStartPage As Long, ByVal nEndPage As Long) As Integer
    Declare Auto Function DTWAIN_FeedPage Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_FlipBitmap Lib "DTWAIN64U.DLL" (ByVal hDib As Long) As Integer
    Declare Auto Function DTWAIN_FlushAcquiredPages Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_ForceAcquireBitDepth Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal BitDepth As Long) As Integer
    Declare Auto Function DTWAIN_FrameCreate Lib "DTWAIN64U.DLL" (ByVal Left As Double, ByVal Top As Double, ByVal Right As Double, ByVal Bottom As Double) As Long
    Declare Auto Function DTWAIN_FrameCreateString Lib "DTWAIN64U.DLL" (ByVal Left As String, ByVal Top As String, ByVal Right As String, ByVal Bottom As String) As Long
    Declare Auto Function DTWAIN_FrameDestroy Lib "DTWAIN64U.DLL" (ByVal Frame As Long) As Integer
    Declare Auto Function DTWAIN_FrameGetAll Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByRef Left As Double, ByRef Top As Double, ByRef Right As Double, ByRef Bottom As Double) As Integer
    Declare Auto Function DTWAIN_FrameGetAllString Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByVal Left As String, ByVal Top As String, ByVal Right As String, ByVal Bottom As String) As Integer
    Declare Auto Function DTWAIN_FrameGetValue Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByVal nWhich As Long, ByRef Value As Double) As Integer
    Declare Auto Function DTWAIN_FrameGetValueString Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByVal nWhich As Long, ByVal Value As String) As Integer
    Declare Auto Function DTWAIN_FrameIsValid Lib "DTWAIN64U.DLL" (ByVal Frame As Long) As Integer
    Declare Auto Function DTWAIN_FrameSetAll Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByVal Left As Double, ByVal Top As Double, ByVal Right As Double, ByVal Bottom As Double) As Integer
    Declare Auto Function DTWAIN_FrameSetAllString Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByVal Left As String, ByVal Top As String, ByVal Right As String, ByVal Bottom As String) As Integer
    Declare Auto Function DTWAIN_FrameSetValue Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByVal nWhich As Long, ByVal Value As Double) As Integer
    Declare Auto Function DTWAIN_FrameSetValueString Lib "DTWAIN64U.DLL" (ByVal Frame As Long, ByVal nWhich As Long, ByVal Value As String) As Integer
    Declare Auto Function DTWAIN_FreeExtImageInfo Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lGetType As Long, ByRef FloatEnum As Long) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea2 Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef left As Double, ByRef top As Double, ByRef right As Double, ByRef bottom As Double, ByRef lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireArea2String Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByRef Unit As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireStripBuffer Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Long
    Declare Auto Function DTWAIN_GetAcquireStripData Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpCompression As Integer, ByRef lpBytesPerRow As Integer, ByRef lpColumns As Integer, ByRef lpRows As Integer, ByRef XOffset As Integer, ByRef YOffset As Integer, ByRef lpBytesWritten As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquireStripSizes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpMin As Integer, ByRef lpMax As Integer, ByRef lpPreferred As Integer) As Integer
    Declare Auto Function DTWAIN_GetAcquiredImage Lib "DTWAIN64U.DLL" (ByVal aAcq As Long, ByVal nWhichAcq As Long, ByVal nWhichDib As Long) As Long
    Declare Auto Function DTWAIN_GetAcquiredImageArray Lib "DTWAIN64U.DLL" (ByVal aAcq As Long, ByVal nWhichAcq As Long) As Long
    Declare Auto Function DTWAIN_GetAlarmVolume Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpVolume As Integer) As Integer
    Declare Auto Function DTWAIN_GetAppInfo Lib "DTWAIN64U.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Auto Function DTWAIN_GetAuthor Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szAuthor As String) As Integer
    Declare Auto Function DTWAIN_GetBatteryMinutes Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpMinutes As Integer) As Integer
    Declare Auto Function DTWAIN_GetBatteryPercent Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpPercent As Integer) As Integer
    Declare Auto Function DTWAIN_GetBitDepth Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef BitDepth As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetBlankPageAutoDetection Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetBrightness Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Brightness As Double) As Integer
    Declare Auto Function DTWAIN_GetBrightnessString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Brightness As String) As Integer
    Declare Auto Function DTWAIN_GetCallback Lib  "DTWAIN64U.DLL" () As DTWAINCallback
    Declare Auto Function DTWAIN_GetCallback64 Lib  "DTWAIN64U.DLL" () As DTWAINCallback64
    Declare Auto Function DTWAIN_GetCapArrayType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nCap As Long) As Integer
    Declare Auto Function DTWAIN_GetCapContainer Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nCap As Long, ByVal lCapType As Long) As Integer
    Declare Auto Function DTWAIN_GetCapContainerEx Lib "DTWAIN64U.DLL" (ByVal nCap As Long, ByVal bSetContainer As Integer, ByRef ConTypes As Long) As Integer
    Declare Auto Function DTWAIN_GetCapDataType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nCap As Long) As Integer
    Declare Auto Function DTWAIN_GetCapFromName Lib "DTWAIN64U.DLL" (ByVal szName As String) As Integer
    Declare Auto Function DTWAIN_GetCapOperations Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCapability As Long, ByRef lpOps As Integer) As Integer
    Declare Auto Function DTWAIN_GetCapValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCap As Long, ByVal lGetType As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_GetCapValuesEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCap As Long, ByVal lGetType As Long, ByVal lContainerType As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_GetCapValuesEx2 Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCap As Long, ByVal lGetType As Long, ByVal lContainerType As Long, ByVal nDataType As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_GetCaption Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Caption As String) As Integer
    Declare Auto Function DTWAIN_GetCompressionSize Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lBytes As Integer) As Integer
    Declare Auto Function DTWAIN_GetCompressionType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpCompression As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetContrast Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Contrast As Double) As Integer
    Declare Auto Function DTWAIN_GetContrastString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Auto Function DTWAIN_GetCountry Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetCurrentAcquiredImage Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Long
    Declare Auto Function DTWAIN_GetCurrentFileName Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szName As String, ByVal MaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetCurrentPageNum Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetCurrentRetryCount Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetCustomDSData Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Data As Integer, ByVal dSize As Long, ByRef pActualSize As Integer, ByVal nFlags As Long) As Long
    Declare Auto Function DTWAIN_GetDSMFullName Lib "DTWAIN64U.DLL" (ByVal DSMType As Long, ByVal szDLLName As String, ByVal nMaxLen As Long, ByRef pWhichSearch As Integer) As Integer
    Declare Auto Function DTWAIN_GetDSMSearchOrder Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetDTWAINHandle Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_GetDeviceEvent Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpEvent As Integer) As Integer
    Declare Auto Function DTWAIN_GetDeviceEventEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpEvent As Integer, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_GetDeviceNotifications Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef DevEvents As Integer) As Integer
    Declare Auto Function DTWAIN_GetDeviceTimeDate Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Auto Function DTWAIN_GetDuplexType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpDupType As Integer) As Integer
    Declare Auto Function DTWAIN_GetErrorBuffer Lib "DTWAIN64U.DLL" (ByRef ArrayBuffer As Long) As Integer
    Declare Auto Function DTWAIN_GetErrorBufferThreshold Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetErrorString Lib "DTWAIN64U.DLL" (ByVal lError As Long, ByVal lpszBuffer As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetExtCapFromName Lib "DTWAIN64U.DLL" (ByVal szName As String) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfo Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetExtImageInfoData Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nWhich As Long, ByRef Data As Long) As Integer
    Declare Auto Function DTWAIN_GetExtNameFromCap Lib "DTWAIN64U.DLL" (ByVal nValue As Long, ByVal szValue As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetFeederAlignment Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpAlignment As Integer) As Integer
    Declare Auto Function DTWAIN_GetFeederFuncs Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetFeederOrder Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpOrder As Integer) As Integer
    Declare Auto Function DTWAIN_GetHighlight Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Highlight As Double) As Integer
    Declare Auto Function DTWAIN_GetHighlightString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_GetHighlightStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_GetHighlightStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_GetImageInfo Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpXResolution As Double, ByRef lpYResolution As Double, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Long, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Auto Function DTWAIN_GetImageInfoString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpXResolution As String, ByVal lpYResolution As String, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Long, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Auto Function DTWAIN_GetJobControl Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pJobControl As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetJpegValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
    Declare Auto Function DTWAIN_GetLanguage Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetLastError Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetLightPath Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpLightPath As Integer) As Integer
    Declare Auto Function DTWAIN_GetLightSources Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef LightSources As Long) As Integer
    Declare Auto Function DTWAIN_GetManualDuplexCount Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pSide1 As Integer, ByRef pSide2 As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxAcquisitions Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetMaxBuffers Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pMaxBuf As Integer) As Integer
    Declare Auto Function DTWAIN_GetMaxPagesToAcquire Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetMaxRetryAttempts Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_GetNameFromCap Lib "DTWAIN64U.DLL" (ByVal nCapValue As Long, ByVal szValue As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetNoiseFilter Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpNoiseFilter As Integer) As Integer
    Declare Auto Function DTWAIN_GetNumAcquiredImages Lib "DTWAIN64U.DLL" (ByVal aAcq As Long, ByVal nWhich As Long) As Integer
    Declare Auto Function DTWAIN_GetNumAcquisitions Lib "DTWAIN64U.DLL" (ByVal aAcq As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRCapValues Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal OCRCapValue As Long, ByVal nGetType As Long, ByRef CapValues As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRLastError Lib "DTWAIN64U.DLL" (ByVal Engine As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRManufacturer Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szManufacturer As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRProductFamily Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szProductFamily As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRProductName Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szProductName As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRText Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal nPageNo As Long, ByVal Data As String, ByVal dSize As Long, ByRef pActualSize As Integer, ByVal nFlags As Long) As Long
    Declare Auto Function DTWAIN_GetOCRTextInfoFloat Lib "DTWAIN64U.DLL" (ByVal OCRTextInfo As Long, ByVal nCharPos As Long, ByVal nWhichItem As Long, ByRef pInfo As Double) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoFloatEx Lib "DTWAIN64U.DLL" (ByVal OCRTextInfo As Long, ByVal nWhichItem As Long, ByRef pInfo As Double, ByVal bufSize As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoHandle Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal nPageNo As Long) As Long
    Declare Auto Function DTWAIN_GetOCRTextInfoLong Lib "DTWAIN64U.DLL" (ByVal OCRTextInfo As Long, ByVal nCharPos As Long, ByVal nWhichItem As Long, ByRef pInfo As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRTextInfoLongEx Lib "DTWAIN64U.DLL" (ByVal OCRTextInfo As Long, ByVal nWhichItem As Long, ByRef pInfo As Integer, ByVal bufSize As Long) As Integer
    Declare Auto Function DTWAIN_GetOCRVersionInfo Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal buffer As String, ByVal maxBufSize As Long) As Integer
    Declare Auto Function DTWAIN_GetOrientation Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpOrient As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementFloat Lib "DTWAIN64U.DLL" (ByVal TextElement As Long, ByRef val1 As Double, ByRef val2 As Double, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementLong Lib "DTWAIN64U.DLL" (ByVal TextElement As Long, ByRef val1 As Integer, ByRef val2 As Integer, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_GetPDFTextElementString Lib "DTWAIN64U.DLL" (ByVal TextElement As Long, ByVal szData As String, ByVal maxLen As Long, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_GetPDFType1FontName Lib "DTWAIN64U.DLL" (ByVal FontVal As Long, ByVal szFont As String, ByVal nChars As Long) As Integer
    Declare Auto Function DTWAIN_GetPaperSize Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpPaperSize As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchMaxPriorities Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pMaxPriorities As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchMaxRetries Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pMaxRetries As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchPriorities Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef SearchPriorities As Long) As Integer
    Declare Auto Function DTWAIN_GetPatchSearchMode Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pSearchMode As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPatchTimeOut Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pTimeOut As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPixelFlavor Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpPixelFlavor As Integer) As Integer
    Declare Auto Function DTWAIN_GetPixelType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef PixelType As Integer, ByRef BitDepth As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinter Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpPrinter As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStartNumber Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef nStart As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStringMode Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef PrinterMode As Integer, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_GetPrinterStrings Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef ArrayString As Long) As Integer
    Declare Auto Function DTWAIN_GetPrinterSuffixString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Suffix As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetRegisteredMsg Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetResolution Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Resolution As Double) As Integer
    Declare Auto Function DTWAIN_GetResolutionString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Resolution As String) As Integer
    Declare Auto Function DTWAIN_GetRotation Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Rotation As Double) As Integer
    Declare Auto Function DTWAIN_GetRotationString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Rotation As String) As Integer
    Declare Auto Function DTWAIN_GetSourceAcquisitions Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Long
    Declare Auto Function DTWAIN_GetSourceID Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Long
    Declare Auto Function DTWAIN_GetSourceManufacturer Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetSourceProductFamily Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetSourceProductName Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetSourceUnit Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef lpUnit As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionInfo Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionNumber Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef pMajor As Integer, ByRef pMinor As Integer) As Integer
    Declare Auto Function DTWAIN_GetStaticLibVersion Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTempFileDirectory Lib "DTWAIN64U.DLL" (ByVal szFilePath As String, ByVal nMaxLen As Long) As Integer
    Declare Auto Function DTWAIN_GetThreshold Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByRef Threshold As Double) As Integer
    Declare Auto Function DTWAIN_GetThresholdString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Threshold As String) As Integer
    Declare Auto Function DTWAIN_GetThresholdStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Threshold As String) As Integer
    Declare Auto Function DTWAIN_GetThresholdStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Threshold As String) As Integer
    Declare Auto Function DTWAIN_GetTimeDate Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Auto Function DTWAIN_GetTwainAppID Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_GetTwainAvailability Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTwainHwnd Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_GetTwainMode Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetTwainTimeout Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_GetVersion Lib "DTWAIN64U.DLL" (ByRef lpMajor As Integer, ByRef lpMinor As Integer, ByRef lpVersionType As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionEx Lib "DTWAIN64U.DLL" (ByRef lMajor As Integer, ByRef lMinor As Integer, ByRef lVersionType As Integer, ByRef lPatchLevel As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionInfo Lib "DTWAIN64U.DLL" (ByVal lpszVer As String, ByVal nLength As Long) As Integer
    Declare Auto Function DTWAIN_GetVersionString Lib "DTWAIN64U.DLL" (ByVal lpszVer As String, ByVal nLength As Long) As Integer
    Declare Auto Function DTWAIN_InitExtImageInfo Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_InitOCRInterface Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsAcquiring Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsAutoBorderDetectEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoBorderDetectSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoBrightEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoBrightSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoDeskewEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoDeskewSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoFeedEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoFeedSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoRotateEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsAutoScanEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsBlankPageDetectionOn Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsCapSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCapability As Long) As Integer
    Declare Auto Function DTWAIN_IsCompressionSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Compression As Long) As Integer
    Declare Auto Function DTWAIN_IsCustomDSDataSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsDIBBlank Lib "DTWAIN64U.DLL" (ByVal hDib As Long, ByVal threshold As Double) As Integer
    Declare Auto Function DTWAIN_IsDIBBlankString Lib "DTWAIN64U.DLL" (ByVal hDib As Long, ByVal threshold As String) As Integer
    Declare Auto Function DTWAIN_IsDeviceEventSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsDeviceOnLine Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsDuplexEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsDuplexSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsExtImageInfoSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsFeederEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsFeederLoaded Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsFeederSensitive Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsFeederSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsFileSystemSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsFileXferSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lFileType As Long) As Integer
    Declare Auto Function DTWAIN_IsIndicatorEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsIndicatorSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsInitialized Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsJPEGSupported Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsJobControlSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal JobControl As Long) As Integer
    Declare Auto Function DTWAIN_IsLampEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsLampSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsLightPathSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsLightSourceSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsMaxBuffersSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal MaxBuf As Long) As Integer
    Declare Auto Function DTWAIN_IsMsgNotifyEnabled Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsOCREngineActivated Lib "DTWAIN64U.DLL" (ByVal OCREngine As Long) As Integer
    Declare Auto Function DTWAIN_IsOrientationSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Orientation As Long) As Integer
    Declare Auto Function DTWAIN_IsPDFSupported Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsPNGSupported Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsPaperDetectable Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsPaperSizeSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PaperSize As Long) As Integer
    Declare Auto Function DTWAIN_IsPatchCapsSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsPatchDetectEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsPatchSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PatchCode As Long) As Integer
    Declare Auto Function DTWAIN_IsPixelTypeSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelType As Long) As Integer
    Declare Auto Function DTWAIN_IsPrinterEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Printer As Long) As Integer
    Declare Auto Function DTWAIN_IsPrinterSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsSessionEnabled Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsSkipImageInfoError Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsSourceAcquiring Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsSourceOpen Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsTIFFSupported Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsThumbnailEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsThumbnailSupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsTwainAvailable Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_IsTwainMsg Lib "DTWAIN64U.DLL" (ByVal pMsg As WinMsg) As Integer
    Declare Auto Function DTWAIN_IsUIControllable Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsUIEnabled Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_IsUIOnlySupported Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_LoadCustomStringResources Lib "DTWAIN64U.DLL" (ByVal sLangDLL As String) As Integer
    Declare Auto Function DTWAIN_LoadLanguageResource Lib "DTWAIN64U.DLL" (ByVal nLanguage As Long) As Integer
    Declare Auto Function DTWAIN_LogMessage Lib "DTWAIN64U.DLL" (ByVal message As String) As Integer
    Declare Auto Function DTWAIN_OpenSource Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_OpenSourcesOnSelect Lib "DTWAIN64U.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_RangeCreate Lib "DTWAIN64U.DLL" (ByVal nEnumType As Long) As Long
    Declare Auto Function DTWAIN_RangeCreateFromCap Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCapType As Long) As Long
    Declare Auto Function DTWAIN_RangeDestroy Lib "DTWAIN64U.DLL" (ByVal pSource As Long) As Integer
    Declare Auto Function DTWAIN_RangeExpand Lib "DTWAIN64U.DLL" (ByVal pSource As Long, ByRef pArray As Long) As Integer
    Declare Auto Function DTWAIN_RangeGetAllFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByRef pVariantLow As Double, ByRef pVariantUp As Double, ByRef pVariantStep As Double, ByRef pVariantDefault As Double, ByRef pVariantCurrent As Double) As Integer
    Declare Auto Function DTWAIN_RangeGetAllFloatString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal dLow As String, ByVal dUp As String, ByVal dStep As String, ByVal dDefault As String, ByVal dCurrent As String) As Integer
    Declare Auto Function DTWAIN_RangeGetAllLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByRef pVariantLow As Integer, ByRef pVariantUp As Integer, ByRef pVariantStep As Integer, ByRef pVariantDefault As Integer, ByRef pVariantCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetCount Lib "DTWAIN64U.DLL" (ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal lPos As Long, ByRef pVal As Double) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueFloatString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal lPos As Long, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_RangeGetExpValueLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal lPos As Long, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As Double, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosFloatString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetPosLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Value As Long, ByRef pPos As Integer) As Integer
    Declare Auto Function DTWAIN_RangeGetValueFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhich As Long, ByRef pVal As Double) As Integer
    Declare Auto Function DTWAIN_RangeGetValueFloatString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhich As Long, ByVal pVal As String) As Integer
    Declare Auto Function DTWAIN_RangeGetValueLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhich As Long, ByRef pVal As Integer) As Integer
    Declare Auto Function DTWAIN_RangeIsValid Lib "DTWAIN64U.DLL" (ByVal Range As Long, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal dIn As Double, ByRef pOut As Double, ByVal RoundType As Long) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueFloatString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal dIn As String, ByVal pOut As String, ByVal RoundType As Long) As Integer
    Declare Auto Function DTWAIN_RangeNearestValueLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal lIn As Long, ByRef pOut As Integer, ByVal RoundType As Long) As Integer
    Declare Auto Function DTWAIN_RangeSetAllFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal dLow As Double, ByVal dUp As Double, ByVal dStep As Double, ByVal dDefault As Double, ByVal dCurrent As Double) As Integer
    Declare Auto Function DTWAIN_RangeSetAllFloatString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal dLow As String, ByVal dUp As String, ByVal dStep As String, ByVal dDefault As String, ByVal dCurrent As String) As Integer
    Declare Auto Function DTWAIN_RangeSetAllLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal lLow As Long, ByVal lUp As Long, ByVal lStep As Long, ByVal lDefault As Long, ByVal lCurrent As Long) As Integer
    Declare Auto Function DTWAIN_RangeSetValueFloat Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhich As Long, ByVal Val As Double) As Integer
    Declare Auto Function DTWAIN_RangeSetValueFloatString Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhich As Long, ByVal Val As String) As Integer
    Declare Auto Function DTWAIN_RangeSetValueLong Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhich As Long, ByVal Val As Long) As Integer
    Declare Auto Function DTWAIN_ResetPDFTextElement Lib "DTWAIN64U.DLL" (ByVal TextElement As Long) As Integer
    Declare Auto Function DTWAIN_RewindPage Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_SelectDefaultOCREngine Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_SelectDefaultSource Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_SelectOCREngine Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_SelectOCREngineByName Lib "DTWAIN64U.DLL" (ByVal lpszName As String) As Long
    Declare Auto Function DTWAIN_SelectSource Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_SelectSource2 Lib "DTWAIN64U.DLL" (ByVal hWndParent As LongPtr, ByVal szTitle As String, ByVal xPos As Long, ByVal yPos As Long, ByVal nOptions As Long) As Long
    Declare Auto Function DTWAIN_SelectSourceByName Lib "DTWAIN64U.DLL" (ByVal lpszName As String) As Long
    Declare Auto Function DTWAIN_SetAcquireArea Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lSetType As Long, ByVal FloatEnum As Long, ByVal ActualEnum As Long) As Integer
    Declare Auto Function DTWAIN_SetAcquireArea2 Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal left As Double, ByVal top As Double, ByVal right As Double, ByVal bottom As Double, ByVal lUnit As Long, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_SetAcquireArea2String Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByVal lUnit As Long, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageNegative Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal IsNegative As Integer) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageScale Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal xscale As Double, ByVal yscale As Double) As Integer
    Declare Auto Function DTWAIN_SetAcquireImageScaleString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal xscale As String, ByVal yscale As String) As Integer
    Declare Auto Function DTWAIN_SetAcquireStripBuffer Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal hMem As Long) As Integer
    Declare Auto Function DTWAIN_SetAlarmVolume Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Volume As Long) As Integer
    Declare Auto Function DTWAIN_SetAlarms Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Alarms As Long) As Integer
    Declare Auto Function DTWAIN_SetAllCapsToDefault Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_SetAppInfo Lib "DTWAIN64U.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Auto Function DTWAIN_SetAuthor Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szAuthor As String) As Integer
    Declare Auto Function DTWAIN_SetAvailablePrinters Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpAvailPrinters As Long) As Integer
    Declare Auto Function DTWAIN_SetAvailablePrintersArray Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal AvailPrinters As Long) As Integer
    Declare Auto Function DTWAIN_SetBitDepth Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal BitDepth As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetection Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal threshold As Double, ByVal autodetect_option As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBlankPageDetectionString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal threshold As String, ByVal autodetect_option As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetBrightness Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Brightness As Double) As Integer
    Declare Auto Function DTWAIN_SetBrightnessString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Brightness As String) As Integer
    Declare Auto Function DTWAIN_SetCallback Lib "DTWAIN64U.DLL" (ByVal Fn As DTWAINCallback, ByVal UserData As Long) As DTWAINCallback
    Declare Auto Function DTWAIN_SetCallback64 Lib "DTWAIN64U.DLL" (ByVal Fn As DTWAINCallback64, ByVal UserData As Long) As DTWAINCallback64
    Declare Auto Function DTWAIN_SetCamera Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szCamera As String) As Integer
    Declare Auto Function DTWAIN_SetCapValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCap As Long, ByVal lSetType As Long, ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_SetCapValuesEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCap As Long, ByVal lSetType As Long, ByVal lContainerType As Long, ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_SetCapValuesEx2 Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCap As Long, ByVal lSetType As Long, ByVal lContainerType As Long, ByVal nDataType As Long, ByVal pArray As Long) As Integer
    Declare Auto Function DTWAIN_SetCaption Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Caption As String) As Integer
    Declare Auto Function DTWAIN_SetCompressionType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lCompression As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetContrast Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As Double) As Integer
    Declare Auto Function DTWAIN_SetContrastString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Auto Function DTWAIN_SetCountry Lib "DTWAIN64U.DLL" (ByVal nCountry As Long) As Integer
    Declare Auto Function DTWAIN_SetCurrentRetryCount Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nCount As Long) As Integer
    Declare Auto Function DTWAIN_SetCustomDSData Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal hData As Long, ByRef Data As Integer, ByVal dSize As Long, ByVal nFlags As Long) As Integer
    Declare Auto Function DTWAIN_SetDSMSearchOrder Lib "DTWAIN64U.DLL" (ByVal SearchPath As Long) As Integer
    Declare Auto Function DTWAIN_SetDefaultSource Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_SetDeviceNotifications Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal DevEvents As Long) As Integer
    Declare Auto Function DTWAIN_SetDeviceTimeDate Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Auto Function DTWAIN_SetEOJDetectValue Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nValue As Long) As Integer
    Declare Auto Function DTWAIN_SetErrorBufferThreshold Lib "DTWAIN64U.DLL" (ByVal nErrors As Long) As Integer
    Declare Auto Function DTWAIN_SetFeederAlignment Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpAlignment As Long) As Integer
    Declare Auto Function DTWAIN_SetFeederOrder Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lOrder As Long) As Integer
    Declare Auto Function DTWAIN_SetFileAutoIncrement Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Increment As Long, ByVal bResetOnAcquire As Integer, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetFileSavePos Lib "DTWAIN64U.DLL" (ByVal hWndParent As LongPtr, ByVal szTitle As String, ByVal xPos As Long, ByVal yPos As Long, ByVal nFlags As Long) As Integer
    Declare Auto Function DTWAIN_SetFileXferFormat Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lFileType As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetHighlight Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Highlight As Double) As Integer
    Declare Auto Function DTWAIN_SetHighlightString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_SetHighlightStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_SetHighlightStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Highlight As String) As Integer
    Declare Auto Function DTWAIN_SetJobControl Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal JobControl As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetJpegValues Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Quality As Long, ByVal Progressive As Long) As Integer
    Declare Auto Function DTWAIN_SetLanguage Lib "DTWAIN64U.DLL" (ByVal nLanguage As Long) As Integer
    Declare Auto Function DTWAIN_SetLightPath Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal LightPath As Long) As Integer
    Declare Auto Function DTWAIN_SetLightPathEx Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal LightPaths As Long) As Integer
    Declare Auto Function DTWAIN_SetLightSources Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal LightSources As Long) As Integer
    Declare Auto Function DTWAIN_SetManualDuplexMode Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Flags As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetMaxAcquisitions Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal MaxAcquires As Long) As Integer
    Declare Auto Function DTWAIN_SetMaxBuffers Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal MaxBuf As Long) As Integer
    Declare Auto Function DTWAIN_SetMaxRetryAttempts Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nAttempts As Long) As Integer
    Declare Auto Function DTWAIN_SetMultipageScanMode Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal ScanType As Long) As Integer
    Declare Auto Function DTWAIN_SetNoiseFilter Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal NoiseFilter As Long) As Integer
    Declare Auto Function DTWAIN_SetOCRCapValues Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal OCRCapValue As Long, ByVal SetType As Long, ByVal CapValues As Long) As Integer
    Declare Auto Function DTWAIN_SetOrientation Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Orient As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFASCIICompression Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFAuthor Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpAuthor As String) As Integer
    Declare Auto Function DTWAIN_SetPDFCompression Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bCompression As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFCreator Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpCreator As String) As Integer
    Declare Auto Function DTWAIN_SetPDFEncryption Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bUseEncryption As Integer, ByVal lpszUser As String, ByVal lpszOwner As String, ByVal Permissions As Long, ByVal UseStrongEncryption As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFJpegQuality Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Quality As Long) As Integer
    Declare Auto Function DTWAIN_SetPDFKeywords Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpKeyWords As String) As Integer
    Declare Auto Function DTWAIN_SetPDFOCRConversion Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal PageType As Long, ByVal FileType As Long, ByVal PixelType As Long, ByVal BitDepth As Long, ByVal Options As Long) As Integer
    Declare Auto Function DTWAIN_SetPDFOCRMode Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetPDFOrientation Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lPOrientation As Long) As Integer
    Declare Auto Function DTWAIN_SetPDFPageScale Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nOptions As Long, ByVal xScale As Double, ByVal yScale As Double) As Integer
    Declare Auto Function DTWAIN_SetPDFPageScaleString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nOptions As Long, ByVal xScale As String, ByVal yScale As String) As Integer
    Declare Auto Function DTWAIN_SetPDFPageSize Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PageSize As Long, ByVal CustomWidth As Double, ByVal CustomHeight As Double) As Integer
    Declare Auto Function DTWAIN_SetPDFPageSizeString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PageSize As Long, ByVal CustomWidth As String, ByVal CustomHeight As String) As Integer
    Declare Auto Function DTWAIN_SetPDFProducer Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpProducer As String) As Integer
    Declare Auto Function DTWAIN_SetPDFSubject Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpSubject As String) As Integer
    Declare Auto Function DTWAIN_SetPDFTextElementFloat Lib "DTWAIN64U.DLL" (ByVal TextElement As Long, ByVal val1 As Double, ByVal val2 As Double, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_SetPDFTextElementLong Lib "DTWAIN64U.DLL" (ByVal TextElement As Long, ByVal val1 As Long, ByVal val2 As Long, ByVal Flags As Long) As Integer
    Declare Auto Function DTWAIN_SetPDFTitle Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpTitle As String) As Integer
    Declare Auto Function DTWAIN_SetPaperSize Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PaperSize As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPatchMaxPriorities Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nMaxSearchRetries As Long) As Integer
    Declare Auto Function DTWAIN_SetPatchMaxRetries Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nMaxRetries As Long) As Integer
    Declare Auto Function DTWAIN_SetPatchPriorities Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal SearchPriorities As Long) As Integer
    Declare Auto Function DTWAIN_SetPatchSearchMode Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nSearchMode As Long) As Integer
    Declare Auto Function DTWAIN_SetPatchTimeOut Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal TimeOutValue As Long) As Integer
    Declare Auto Function DTWAIN_SetPixelFlavor Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelFlavor As Long) As Integer
    Declare Auto Function DTWAIN_SetPixelType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PixelType As Long, ByVal BitDepth As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPostScriptTitle Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTitle As String) As Integer
    Declare Auto Function DTWAIN_SetPostScriptType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PSType As Long) As Integer
    Declare Auto Function DTWAIN_SetPrinter Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Printer As Long, ByVal bCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStartNumber Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nStart As Long) As Integer
    Declare Auto Function DTWAIN_SetPrinterStringMode Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PrinterMode As Long, ByVal bSetCurrent As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterStrings Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal ArrayString As Long, ByRef pNumStrings As Integer) As Integer
    Declare Auto Function DTWAIN_SetPrinterSuffixString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Suffix As String) As Integer
    Declare Auto Function DTWAIN_SetQueryCapSupport Lib "DTWAIN64U.DLL" (ByVal bSet As Integer) As Integer
    Declare Auto Function DTWAIN_SetResolution Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Resolution As Double) As Integer
    Declare Auto Function DTWAIN_SetResolutionString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Resolution As String) As Integer
    Declare Auto Function DTWAIN_SetRotation Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Rotation As Double) As Integer
    Declare Auto Function DTWAIN_SetRotationString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Rotation As String) As Integer
    Declare Auto Function DTWAIN_SetSourceUnit Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpUnit As Long) As Integer
    Declare Auto Function DTWAIN_SetTIFFCompressType Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Setting As Long) As Integer
    Declare Auto Function DTWAIN_SetTIFFInvert Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Setting As Long) As Integer
    Declare Auto Function DTWAIN_SetTempFileDirectory Lib "DTWAIN64U.DLL" (ByVal szFilePath As String) As Integer
    Declare Auto Function DTWAIN_SetThreshold Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Threshold As Double, ByVal bSetBithDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetThresholdString Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Threshold As String, ByVal bSetBitDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetThresholdStringA Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Threshold As String, ByVal bSetBitDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetThresholdStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Threshold As String, ByVal bSetBitDepthReduction As Integer) As Integer
    Declare Auto Function DTWAIN_SetTwainDSM Lib "DTWAIN64U.DLL" (ByVal DSMType As Long) As Integer
    Declare Auto Function DTWAIN_SetTwainLog Lib "DTWAIN64U.DLL" (ByVal LogFlags As Long, ByVal lpszLogFile As String) As Integer
    Declare Auto Function DTWAIN_SetTwainMode Lib "DTWAIN64U.DLL" (ByVal lAcquireMode As Long) As Integer
    Declare Auto Function DTWAIN_SetTwainTimeout Lib "DTWAIN64U.DLL" (ByVal milliseconds As Long) As Integer
    Declare Auto Function DTWAIN_SetUpdateDibProc Lib "DTWAIN64U.DLL" (ByVal DibProc As DTWAINDibUpdateCallback) As DTWAINDibUpdateCallback
    Declare Auto Function DTWAIN_ShowUIOnly Lib "DTWAIN64U.DLL" (ByVal Source As Long) As Integer
    Declare Auto Function DTWAIN_ShutdownOCREngine Lib "DTWAIN64U.DLL" (ByVal OCREngine As Long) As Integer
    Declare Auto Function DTWAIN_SkipImageInfoError Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bSkip As Integer) As Integer
    Declare Auto Function DTWAIN_StartThread Lib "DTWAIN64U.DLL" (ByVal DLLHandle As Long) As Integer
    Declare Auto Function DTWAIN_StartTwainSession Lib "DTWAIN64U.DLL" (ByVal hWndMsg As LongPtr, ByVal lpszDLLName As String) As Integer
    Declare Auto Function DTWAIN_SysDestroy Lib  "DTWAIN64U.DLL" () As Integer
    Declare Auto Function DTWAIN_SysInitialize Lib  "DTWAIN64U.DLL" () As Long
    Declare Auto Function DTWAIN_SysInitializeEx Lib "DTWAIN64U.DLL" (ByVal szINIPath As String) As Long
    Declare Auto Function DTWAIN_SysInitializeEx2 Lib "DTWAIN64U.DLL" (ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Long
    Declare Auto Function DTWAIN_SysInitializeLib Lib "DTWAIN64U.DLL" (ByVal hInstance As Long) As Long
    Declare Auto Function DTWAIN_SysInitializeLibEx Lib "DTWAIN64U.DLL" (ByVal hInstance As Long, ByVal szINIPath As String) As Long
    Declare Auto Function DTWAIN_SysInitializeLibEx2 Lib "DTWAIN64U.DLL" (ByVal hInstance As Long, ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Long
    Declare Auto Function DTWAIN_TwainSave Lib "DTWAIN64U.DLL" (ByVal cmd As String) As Integer
    Declare Auto Sub DTWAIN_X Lib "DTWAIN64U.DLL" (ByVal s As String) 
    Declare Unicode Function DTWAIN_AcquireFileW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpszFile As String, ByVal lFileType As Long, ByVal lFileFlags As Long, ByVal PixelType As Long, ByVal lMaxPages As Long, ByVal bShowUI As Integer, ByVal bCloseSource As Integer, ByRef pStatus As Integer) As Integer
    Declare Unicode Function DTWAIN_AddPDFTextW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szText As String, ByVal xPos As Long, ByVal yPos As Long, ByVal fontName As String, ByVal fontSize As Double, ByVal colorRGB As Long, ByVal renderMode As Long, ByVal scaling As Double, ByVal charSpacing As Double, ByVal wordSpacing As Double, ByVal strokeWidth As Long, ByVal Flags As Long) As Integer
    Declare Unicode Function DTWAIN_ArrayAddStringNW Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Unicode Function DTWAIN_ArrayAddStringW Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal Val As String) As Integer
    Declare Unicode Function DTWAIN_ArrayFindStringW Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal pString As String) As Integer
    Declare Unicode Function DTWAIN_ArrayGetAtStringW Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtStringNW Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal Val As String, ByVal num As Long) As Integer
    Declare Unicode Function DTWAIN_ArrayInsertAtStringW Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pVal As String) As Integer
    Declare Unicode Function DTWAIN_ArraySetAtStringW Lib "DTWAIN64U.DLL" (ByVal pArray As Long, ByVal nWhere As Long, ByVal pStr As String) As Integer
    Declare Unicode Function DTWAIN_ExecuteOCRW Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szFileName As String, ByVal nStartPage As Long, ByVal nEndPage As Long) As Integer
    Declare Unicode Function DTWAIN_GetAcquireArea2StringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByRef Unit As Integer) As Integer
    Declare Unicode Function DTWAIN_GetAppInfoW Lib "DTWAIN64U.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Unicode Function DTWAIN_GetAuthorW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szAuthor As String) As Integer
    Declare Unicode Function DTWAIN_GetBrightnessStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_GetCapFromNameW Lib "DTWAIN64U.DLL" (ByVal szName As String) As Integer
    Declare Unicode Function DTWAIN_GetCaptionW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Caption As String) As Integer
    Declare Unicode Function DTWAIN_GetContrastStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_GetCurrentFileNameW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szName As String, ByVal MaxLen As Long) As Integer
    Declare Unicode Function DTWAIN_GetDSMFullNameW Lib "DTWAIN64U.DLL" (ByVal DSMType As Long, ByVal szDLLName As String, ByVal nMaxLen As Long, ByRef pWhichSearch As Integer) As Integer
    Declare Unicode Function DTWAIN_GetDeviceTimeDateW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Unicode Function DTWAIN_GetErrorStringW Lib "DTWAIN64U.DLL" (ByVal lError As Long, ByVal lpszBuffer As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetExtCapFromNameW Lib "DTWAIN64U.DLL" (ByVal szName As String) As Integer
    Declare Unicode Function DTWAIN_GetExtNameFromCapW Lib "DTWAIN64U.DLL" (ByVal nValue As Long, ByVal szValue As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetImageInfoStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpXResolution As String, ByVal lpYResolution As String, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As Long, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
    Declare Unicode Function DTWAIN_GetNameFromCapW Lib "DTWAIN64U.DLL" (ByVal nCapValue As Long, ByVal szValue As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetOCRManufacturerW Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szManufacturer As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetOCRProductFamilyW Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szProductFamily As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetOCRProductNameW Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal szProductName As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetOCRTextW Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal nPageNo As Long, ByVal Data As String, ByVal dSize As Long, ByRef pActualSize As Integer, ByVal nFlags As Long) As Long
    Declare Unicode Function DTWAIN_GetOCRVersionInfoW Lib "DTWAIN64U.DLL" (ByVal Engine As Long, ByVal buffer As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetPDFTextElementStringW Lib "DTWAIN64U.DLL" (ByVal TextElement As Long, ByVal szData As String, ByVal maxLen As Long, ByVal Flags As Long) As Integer
    Declare Unicode Function DTWAIN_GetPDFType1FontNameW Lib "DTWAIN64U.DLL" (ByVal FontVal As Long, ByVal szFont As String, ByVal nChars As Long) As Integer
    Declare Unicode Function DTWAIN_GetPrinterSuffixStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Suffix As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetResolutionStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Resolution As String) As Integer
    Declare Unicode Function DTWAIN_GetRotationStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Rotation As String) As Integer
    Declare Unicode Function DTWAIN_GetSourceManufacturerW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductFamilyW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductNameW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetSourceVersionInfoW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szProduct As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetTempFileDirectoryW Lib "DTWAIN64U.DLL" (ByVal szFilePath As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetTimeDateW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Unicode Function DTWAIN_GetVersionInfoW Lib "DTWAIN64U.DLL" (ByVal lpszVer As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_GetVersionStringW Lib "DTWAIN64U.DLL" (ByVal lpszVer As String, ByVal nLength As Long) As Integer
    Declare Unicode Function DTWAIN_IsDIBBlankStringW Lib "DTWAIN64U.DLL" (ByVal hDib As Long, ByVal threshold As String) As Integer
    Declare Unicode Function DTWAIN_LoadCustomStringResourcesW Lib "DTWAIN64U.DLL" (ByVal sLangDLL As String) As Integer
    Declare Unicode Function DTWAIN_LogMessageW Lib "DTWAIN64U.DLL" (ByVal message As String) As Integer
    Declare Unicode Function DTWAIN_SelectOCREngineByNameW Lib "DTWAIN64U.DLL" (ByVal lpszName As String) As Long
    Declare Unicode Function DTWAIN_SelectSource2W Lib "DTWAIN64U.DLL" (ByVal hWndParent As LongPtr, ByVal szTitle As String, ByVal xPos As Long, ByVal yPos As Long, ByVal nOptions As Long) As Long
    Declare Unicode Function DTWAIN_SelectSourceByNameW Lib "DTWAIN64U.DLL" (ByVal lpszName As String) As Long
    Declare Unicode Function DTWAIN_SetAcquireArea2StringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal left As String, ByVal top As String, ByVal right As String, ByVal bottom As String, ByVal lUnit As Long, ByVal Flags As Long) As Integer
    Declare Unicode Function DTWAIN_SetAcquireImageScaleStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal xscale As String, ByVal yscale As String) As Integer
    Declare Unicode Function DTWAIN_SetAppInfoW Lib "DTWAIN64U.DLL" (ByVal szVerStr As String, ByVal szManu As String, ByVal szProdFam As String, ByVal szProdName As String) As Integer
    Declare Unicode Function DTWAIN_SetAuthorW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szAuthor As String) As Integer
    Declare Unicode Function DTWAIN_SetBlankPageDetectionStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal threshold As String, ByVal autodetect_option As Long, ByVal bSet As Integer) As Integer
    Declare Unicode Function DTWAIN_SetBrightnessStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_SetCameraW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szCamera As String) As Integer
    Declare Unicode Function DTWAIN_SetCaptionW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Caption As String) As Integer
    Declare Unicode Function DTWAIN_SetContrastStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Contrast As String) As Integer
    Declare Unicode Function DTWAIN_SetDeviceTimeDateW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTimeDate As String) As Integer
    Declare Unicode Function DTWAIN_SetFileSavePosW Lib "DTWAIN64U.DLL" (ByVal hWndParent As LongPtr, ByVal szTitle As String, ByVal xPos As Long, ByVal yPos As Long, ByVal nFlags As Long) As Integer
    Declare Unicode Function DTWAIN_SetPDFAuthorW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpAuthor As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFCreatorW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpCreator As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFEncryptionW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal bUseEncryption As Integer, ByVal lpszUser As String, ByVal lpszOwner As String, ByVal Permissions As Long, ByVal UseStrongEncryption As Integer) As Integer
    Declare Unicode Function DTWAIN_SetPDFKeywordsW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpKeyWords As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFPageScaleStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal nOptions As Long, ByVal xScale As String, ByVal yScale As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFPageSizeStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal PageSize As Long, ByVal CustomWidth As String, ByVal CustomHeight As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFProducerW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpProducer As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFSubjectW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpSubject As String) As Integer
    Declare Unicode Function DTWAIN_SetPDFTitleW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal lpTitle As String) As Integer
    Declare Unicode Function DTWAIN_SetPostScriptTitleW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal szTitle As String) As Integer
    Declare Unicode Function DTWAIN_SetPrinterSuffixStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Suffix As String) As Integer
    Declare Unicode Function DTWAIN_SetResolutionStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Resolution As String) As Integer
    Declare Unicode Function DTWAIN_SetRotationStringW Lib "DTWAIN64U.DLL" (ByVal Source As Long, ByVal Rotation As String) As Integer
    Declare Unicode Function DTWAIN_SetTempFileDirectoryW Lib "DTWAIN64U.DLL" (ByVal szFilePath As String) As Integer
    Declare Unicode Function DTWAIN_SetTwainLogW Lib "DTWAIN64U.DLL" (ByVal LogFlags As Long, ByVal lpszLogFile As String) As Integer
    Declare Unicode Function DTWAIN_StartTwainSessionW Lib "DTWAIN64U.DLL" (ByVal hWndMsg As LongPtr, ByVal lpszDLLName As String) As Integer
    Declare Unicode Function DTWAIN_SysInitializeEx2W Lib "DTWAIN64U.DLL" (ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Long
    Declare Unicode Function DTWAIN_SysInitializeExW Lib "DTWAIN64U.DLL" (ByVal szINIPath As String) As Long
    Declare Unicode Function DTWAIN_SysInitializeLibEx2W Lib "DTWAIN64U.DLL" (ByVal hInstance As Long, ByVal szINIPath As String, ByVal szImageDLLPath As String, ByVal szLangResourcePath As String) As Long
    Declare Unicode Function DTWAIN_SysInitializeLibExW Lib "DTWAIN64U.DLL" (ByVal hInstance As Long, ByVal szINIPath As String) As Long
    Declare Unicode Function DTWAIN_TwainSaveW Lib "DTWAIN64U.DLL" (ByVal cmd As String) As Integer
    Declare Unicode Sub DTWAIN_XW Lib "DTWAIN64U.DLL" (ByVal s As String) 
    Declare Auto Function DTWAIN_GetNameFromCap Lib "DTWAIN64U.DLL" (ByVal nCapValue As Integer, ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetTempFileDirectory Lib "DTWAIN64U.DLL" (ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetErrorString Lib "DTWAIN64U.DLL" (ByVal nError As Integer, ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionString Lib "DTWAIN64U.DLL" (ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetVersionInfo Lib "DTWAIN64U.DLL" (ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceManufacturer Lib "DTWAIN64U.DLL" (ByVal nSource As Long, ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductFamily Lib "DTWAIN64U.DLL" (ByVal nSource As Long, ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceProductName Lib "DTWAIN64U.DLL" (ByVal nSource As Long, ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetSourceVersionInfo Lib "DTWAIN64U.DLL" (ByVal nSource As Long, ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetOCRErrorString Lib "DTWAIN64U.DLL" (ByVal nEngine As Long, ByVal nError As Integer, ByVal szValue As System.LongPtr, ByVal nMaxLen As Integer) As Integer
    Declare Auto Function DTWAIN_GetDSMFullName Lib "DTWAIN64U.DLL" (ByVal DSMType As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer, ByRef pStatus As Integer) As Integer
    Declare Ansi Function DTWAIN_GetNameFromCapA Lib "DTWAIN64U.DLL" (ByVal nCapValue As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetTempFileDirectoryA Lib "DTWAIN64U.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetErrorStringA Lib "DTWAIN64U.DLL" (ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionStringA Lib "DTWAIN64U.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetVersionInfoA Lib "DTWAIN64U.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceManufacturerA Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductFamilyA Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceProductNameA Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetSourceVersionInfoA Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetOCRErrorStringA Lib "DTWAIN64U.DLL" (ByVal nEngine As Integer, ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Ansi Function DTWAIN_GetDSMFullNameA Lib "DTWAIN64U.DLL" (ByVal DSMType As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer, ByRef pStatus As Integer) As Integer
    Declare Unicode Function DTWAIN_GetNameFromCapW Lib "DTWAIN64U.DLL" (ByVal nCapValue As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetTempFileDirectoryW Lib "DTWAIN64U.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetErrorStringW Lib "DTWAIN64U.DLL" (ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionStringW Lib "DTWAIN64U.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetVersionInfoW Lib "DTWAIN64U.DLL" (ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceManufacturerW Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductFamilyW Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceProductNameW Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetSourceVersionInfoW Lib "DTWAIN64U.DLL" (ByVal nSource As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetOCRErrorStringW Lib "DTWAIN64U.DLL" (ByVal nEngine As Integer, ByVal nError As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer) As Integer
    Declare Unicode Function DTWAIN_GetDSMFullNameW Lib "DTWAIN64U.DLL" (ByVal DSMType As Integer, ByVal szValue As System.IntPtr, ByVal nMaxLen As Integer, ByRef pStatus As Integer) As Integer
    Declare Auto Function DTWAIN_StartTwainSession Lib "DTWAIN64U.DLL" (ByVal hWndMsg As IntPtr, ByVal szValue As System.IntPtr) As Integer
    Declare Ansi Function DTWAIN_StartTwainSessionA Lib "DTWAIN64U.DLL" (ByVal hWndMsg As IntPtr, ByVal szValue As System.IntPtr) As Integer
    Declare Unicode Function DTWAIN_StartTwainSessionW Lib "DTWAIN64U.DLL" (ByVal hWndMsg As IntPtr, ByVal szValue As System.IntPtr) As Integer
    
End Class
