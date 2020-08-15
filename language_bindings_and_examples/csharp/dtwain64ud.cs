using System.Runtime.InteropServices;

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
namespace Dynarithmic
{
    public class TwainAPI
    {
        public delegate int  DTwainCallback( int wParam, int lParam, int UserData );
        public delegate int  DTwainDibUpdateCallback( int Source, int pagenum, int dibHandle);
        public delegate int  DTwainCallback64( int wParam, int lParam, long UserData );
        public delegate void DTwainLoggerProc( [MarshalAs(UnmanagedType.LPTStr)] string lpszName );
        public const  int DTWAIN_FF_TIFF = 0;
        public const  int DTWAIN_FF_PICT = 1;
        public const  int DTWAIN_FF_BMP = 2;
        public const  int DTWAIN_FF_XBM = 3;
        public const  int DTWAIN_FF_JFIF = 4;
        public const  int DTWAIN_FF_FPX = 5;
        public const  int DTWAIN_FF_TIFFMULTI = 6;
        public const  int DTWAIN_FF_PNG = 7;
        public const  int DTWAIN_FF_SPIFF = 8;
        public const  int DTWAIN_FF_EXIF = 9;
        public const  int DTWAIN_FF_PDF = 10;
        public const  int DTWAIN_FF_JP2 = 11;
        public const  int DTWAIN_FF_JPX = 13;
        public const  int DTWAIN_FF_DEJAVU = 14;
        public const  int DTWAIN_FF_PDFA = 15;
        public const  int DTWAIN_FF_PDFA2 = 16;
        public const  int DTWAIN_CP_NONE = 0;
        public const  int DTWAIN_CP_PACKBITS = 1;
        public const  int DTWAIN_CP_GROUP31D = 2;
        public const  int DTWAIN_CP_GROUP31DEOL = 3;
        public const  int DTWAIN_CP_GROUP32D = 4;
        public const  int DTWAIN_CP_GROUP4 = 5;
        public const  int DTWAIN_CP_JPEG = 6;
        public const  int DTWAIN_CP_LZW = 7;
        public const  int DTWAIN_CP_JBIG = 8;
        public const  int DTWAIN_CP_PNG = 9;
        public const  int DTWAIN_CP_RLE4 = 10;
        public const  int DTWAIN_CP_RLE8 = 11;
        public const  int DTWAIN_CP_BITFIELDS = 12;
        public const  int DTWAIN_CP_ZIP = 13;
        public const  int DTWAIN_CP_JPEG2000 = 14;
        public const  int DTWAIN_FS_NONE = 0;
        public const  int DTWAIN_FS_A4LETTER = 1;
        public const  int DTWAIN_FS_B5LETTER = 2;
        public const  int DTWAIN_FS_USLETTER = 3;
        public const  int DTWAIN_FS_USLEGAL = 4;
        public const  int DTWAIN_FS_A5 = 5;
        public const  int DTWAIN_FS_B4 = 6;
        public const  int DTWAIN_FS_B6 = 7;
        public const  int DTWAIN_FS_USLEDGER = 9;
        public const  int DTWAIN_FS_USEXECUTIVE = 10;
        public const  int DTWAIN_FS_A3 = 11;
        public const  int DTWAIN_FS_B3 = 12;
        public const  int DTWAIN_FS_A6 = 13;
        public const  int DTWAIN_FS_C4 = 14;
        public const  int DTWAIN_FS_C5 = 15;
        public const  int DTWAIN_FS_C6 = 16;
        public const  int DTWAIN_FS_4A0 = 17;
        public const  int DTWAIN_FS_2A0 = 18;
        public const  int DTWAIN_FS_A0 = 19;
        public const  int DTWAIN_FS_A1 = 20;
        public const  int DTWAIN_FS_A2 = 21;
        public const  int DTWAIN_FS_A4 = DTWAIN_FS_A4LETTER;
        public const  int DTWAIN_FS_A7 = 22;
        public const  int DTWAIN_FS_A8 = 23;
        public const  int DTWAIN_FS_A9 = 24;
        public const  int DTWAIN_FS_A10 = 25;
        public const  int DTWAIN_FS_ISOB0 = 26;
        public const  int DTWAIN_FS_ISOB1 = 27;
        public const  int DTWAIN_FS_ISOB2 = 28;
        public const  int DTWAIN_FS_ISOB3 = DTWAIN_FS_B3;
        public const  int DTWAIN_FS_ISOB4 = DTWAIN_FS_B4;
        public const  int DTWAIN_FS_ISOB5 = 29;
        public const  int DTWAIN_FS_ISOB6 = DTWAIN_FS_B6;
        public const  int DTWAIN_FS_ISOB7 = 30;
        public const  int DTWAIN_FS_ISOB8 = 31;
        public const  int DTWAIN_FS_ISOB9 = 32;
        public const  int DTWAIN_FS_ISOB10 = 33;
        public const  int DTWAIN_FS_JISB0 = 34;
        public const  int DTWAIN_FS_JISB1 = 35;
        public const  int DTWAIN_FS_JISB2 = 36;
        public const  int DTWAIN_FS_JISB3 = 37;
        public const  int DTWAIN_FS_JISB4 = 38;
        public const  int DTWAIN_FS_JISB5 = DTWAIN_FS_B5LETTER;
        public const  int DTWAIN_FS_JISB6 = 39;
        public const  int DTWAIN_FS_JISB7 = 40;
        public const  int DTWAIN_FS_JISB8 = 41;
        public const  int DTWAIN_FS_JISB9 = 42;
        public const  int DTWAIN_FS_JISB10 = 43;
        public const  int DTWAIN_FS_C0 = 44;
        public const  int DTWAIN_FS_C1 = 45;
        public const  int DTWAIN_FS_C2 = 46;
        public const  int DTWAIN_FS_C3 = 47;
        public const  int DTWAIN_FS_C7 = 48;
        public const  int DTWAIN_FS_C8 = 49;
        public const  int DTWAIN_FS_C9 = 50;
        public const  int DTWAIN_FS_C10 = 51;
        public const  int DTWAIN_FS_USSTATEMENT = 52;
        public const  int DTWAIN_FS_BUSINESSCARD = 53;
        public const  int DTWAIN_ANYSUPPORT = (-1);
        public const  int DTWAIN_BMP = 100;
        public const  int DTWAIN_JPEG = 200;
        public const  int DTWAIN_PDF = 250;
        public const  int DTWAIN_PDFMULTI = 251;
        public const  int DTWAIN_PCX = 300;
        public const  int DTWAIN_DCX = 301;
        public const  int DTWAIN_TGA = 400;
        public const  int DTWAIN_TIFFLZW = 500;
        public const  int DTWAIN_TIFFNONE = 600;
        public const  int DTWAIN_TIFFG3 = 700;
        public const  int DTWAIN_TIFFG4 = 800;
        public const  int DTWAIN_TIFFPACKBITS = 801;
        public const  int DTWAIN_TIFFDEFLATE = 802;
        public const  int DTWAIN_TIFFJPEG = 803;
        public const  int DTWAIN_TIFFJBIG = 804;
        public const  int DTWAIN_TIFFPIXARLOG = 805;
        public const  int DTWAIN_TIFFNONEMULTI = 900;
        public const  int DTWAIN_TIFFG3MULTI = 901;
        public const  int DTWAIN_TIFFG4MULTI = 902;
        public const  int DTWAIN_TIFFPACKBITSMULTI = 903;
        public const  int DTWAIN_TIFFDEFLATEMULTI = 904;
        public const  int DTWAIN_TIFFJPEGMULTI = 905;
        public const  int DTWAIN_TIFFLZWMULTI = 906;
        public const  int DTWAIN_TIFFJBIGMULTI = 907;
        public const  int DTWAIN_TIFFPIXARLOGMULTI = 908;
        public const  int DTWAIN_WMF = 850;
        public const  int DTWAIN_EMF = 851;
        public const  int DTWAIN_GIF = 950;
        public const  int DTWAIN_PNG = 1000;
        public const  int DTWAIN_PSD = 2000;
        public const  int DTWAIN_JPEG2000 = 3000;
        public const  int DTWAIN_POSTSCRIPT1 = 4000;
        public const  int DTWAIN_POSTSCRIPT2 = 4001;
        public const  int DTWAIN_POSTSCRIPT3 = 4002;
        public const  int DTWAIN_POSTSCRIPT1MULTI = 4003;
        public const  int DTWAIN_POSTSCRIPT2MULTI = 4004;
        public const  int DTWAIN_POSTSCRIPT3MULTI = 4005;
        public const  int DTWAIN_TEXT = 6000;
        public const  int DTWAIN_TEXTMULTI = 6001;
        public const  int DTWAIN_TIFFMULTI = 7000;
        public const  int DTWAIN_ICO = 8000;
        public const  int DTWAIN_ICO_VISTA = 8001;
        public const  int DTWAIN_WBMP = 8500;
        public const  int DTWAIN_INCHES = 0;
        public const  int DTWAIN_CENTIMETERS = 1;
        public const  int DTWAIN_PICAS = 2;
        public const  int DTWAIN_POINTS = 3;
        public const  int DTWAIN_TWIPS = 4;
        public const  int DTWAIN_PIXELS = 5;
        public const  int DTWAIN_USENAME = 4;
        public const  int DTWAIN_USEPROMPT = 8;
        public const  int DTWAIN_USELONGNAME = 16;
        public const  int DTWAIN_USESOURCEMODE = 32;
        public const  int DTWAIN_USELIST = 64;
        public const  int DTWAIN_ARRAYANY = 1;
        public const  int DTWAIN_ArrayTypePTR = 1;
        public const  int DTWAIN_ARRAYLONG = 2;
        public const  int DTWAIN_ARRAYFLOAT = 3;
        public const  int DTWAIN_ARRAYHANDLE = 4;
        public const  int DTWAIN_ARRAYSOURCE = 5;
        public const  int DTWAIN_ARRAYSTRING = 6;
        public const  int DTWAIN_ARRAYFRAME = 7;
        public const  int DTWAIN_ARRAYBOOL = DTWAIN_ARRAYLONG;
        public const  int DTWAIN_ARRAYLONGSTRING = 8;
        public const  int DTWAIN_ARRAYUNICODESTRING = 9;
        public const  int DTWAIN_ARRAYLONG64 = 10;
        public const  int DTWAIN_ARRAYANSISTRING = 11;
        public const  int DTWAIN_ARRAYWIDESTRING = 12;
        public const  int DTWAIN_ARRAYTWFIX32 = 200;
        public const  int DTWAIN_ArrayTypeINVALID = 0;
        public const  int DTWAIN_ARRAYINT16 = 100;
        public const  int DTWAIN_ARRAYUINT16 = 110;
        public const  int DTWAIN_ARRAYUINT32 = 120;
        public const  int DTWAIN_ARRAYINT32 = 130;
        public const  int DTWAIN_ARRAYINT64 = 140;
        public const  int DTWAIN_RANGELONG = DTWAIN_ARRAYLONG;
        public const  int DTWAIN_RANGEFLOAT = DTWAIN_ARRAYFLOAT;
        public const  int DTWAIN_RANGEMIN = 0;
        public const  int DTWAIN_RANGEMAX = 1;
        public const  int DTWAIN_RANGESTEP = 2;
        public const  int DTWAIN_RANGEDEFAULT = 3;
        public const  int DTWAIN_RANGECURRENT = 4;
        public const  int DTWAIN_FRAMELEFT = 0;
        public const  int DTWAIN_FRAMETOP = 1;
        public const  int DTWAIN_FRAMERIGHT = 2;
        public const  int DTWAIN_FRAMEBOTTOM = 3;
        public const  int DTWAIN_FIX32WHOLE = 0;
        public const  int DTWAIN_FIX32FRAC = 1;
        public const  int DTWAIN_JC_NONE = 0;
        public const  int DTWAIN_JC_JSIC = 1;
        public const  int DTWAIN_JC_JSIS = 2;
        public const  int DTWAIN_JC_JSXC = 3;
        public const  int DTWAIN_JC_JSXS = 4;
        public const  int DTWAIN_CAPDATATYPE_UNKNOWN = (-10);
        public const  int DTWAIN_JCBP_JSIC = 5;
        public const  int DTWAIN_JCBP_JSIS = 6;
        public const  int DTWAIN_JCBP_JSXC = 7;
        public const  int DTWAIN_JCBP_JSXS = 8;
        public const  int DTWAIN_FEEDPAGEON = 1;
        public const  int DTWAIN_CLEARPAGEON = 2;
        public const  int DTWAIN_REWINDPAGEON = 4;
        public const  int DTWAIN_AppOwnsDib = 1;
        public const  int DTWAIN_SourceOwnsDib = 2;
        public const  int DTWAIN_CONTARRAY = 8;
        public const  int DTWAIN_CONTENUMERATION = 16;
        public const  int DTWAIN_CONTONEVALUE = 32;
        public const  int DTWAIN_CONTRANGE = 64;
        public const  int DTWAIN_CONTDEFAULT = 0;
        public const  int DTWAIN_CAPGET = 1;
        public const  int DTWAIN_CAPGETCURRENT = 2;
        public const  int DTWAIN_CAPGETDEFAULT = 3;
        public const  int DTWAIN_CAPSET = 6;
        public const  int DTWAIN_CAPRESET = 7;
        public const  int DTWAIN_CAPRESETALL = 8;
        public const  int DTWAIN_CAPSETCONSTRAINT = 9;
        public const  int DTWAIN_CAPSETAVAILABLE = 8;
        public const  int DTWAIN_CAPSETCURRENT = 16;
        public const  int DTWAIN_AREASET = DTWAIN_CAPSET;
        public const  int DTWAIN_AREARESET = DTWAIN_CAPRESET;
        public const  int DTWAIN_AREACURRENT = DTWAIN_CAPGETCURRENT;
        public const  int DTWAIN_AREADEFAULT = DTWAIN_CAPGETDEFAULT;
        public const  int DTWAIN_VER15 = 0;
        public const  int DTWAIN_VER16 = 1;
        public const  int DTWAIN_VER17 = 2;
        public const  int DTWAIN_VER18 = 3;
        public const  int DTWAIN_VER20 = 4;
        public const  int DTWAIN_VER21 = 5;
        public const  int DTWAIN_VER22 = 6;
        public const  int DTWAIN_ACQUIREALL = (-1);
        public const  int DTWAIN_MAXACQUIRE = (-1);
        public const  int DTWAIN_DX_NONE = 0;
        public const  int DTWAIN_DX_1PASSDUPLEX = 1;
        public const  int DTWAIN_DX_2PASSDUPLEX = 2;
        public const  int DTWAIN_PT_BW = 0;
        public const  int DTWAIN_PT_GRAY = 1;
        public const  int DTWAIN_PT_RGB = 2;
        public const  int DTWAIN_PT_PALETTE = 3;
        public const  int DTWAIN_PT_CMY = 4;
        public const  int DTWAIN_PT_CMYK = 5;
        public const  int DTWAIN_PT_YUV = 6;
        public const  int DTWAIN_PT_YUVK = 7;
        public const  int DTWAIN_PT_CIEXYZ = 8;
        public const  int DTWAIN_PT_DEFAULT = 1000;
        public const  int DTWAIN_CURRENT = (-2);
        public const  int DTWAIN_DEFAULT = (-1);
        public const  double DTWAIN_FLOATDEFAULT = (-9999.0);
        public const  int DTWAIN_CallbackERROR = 1;
        public const  int DTWAIN_CallbackMESSAGE = 2;
        public const  int DTWAIN_USENATIVE = 1;
        public const  int DTWAIN_USEBUFFERED = 2;
        public const  int DTWAIN_USECOMPRESSION = 4;
        public const  int DTWAIN_FAILURE1 = (-1);
        public const  int DTWAIN_FAILURE2 = (-2);
        public const  int DTWAIN_DELETEALL = (-1);
        public const  int DTWAIN_TN_ACQUIREDONE = 1000;
        public const  int DTWAIN_TN_ACQUIREFAILED = 1001;
        public const  int DTWAIN_TN_ACQUIRECANCELLED = 1002;
        public const  int DTWAIN_TN_ACQUIRESTARTED = 1003;
        public const  int DTWAIN_TN_PAGECONTINUE = 1004;
        public const  int DTWAIN_TN_PAGEFAILED = 1005;
        public const  int DTWAIN_TN_PAGECANCELLED = 1006;
        public const  int DTWAIN_TN_TRANSFERREADY = 1009;
        public const  int DTWAIN_TN_TRANSFERDONE = 1010;
        public const  int DTWAIN_TN_ACQUIREPAGEDONE = 1010;
        public const  int DTWAIN_TN_UICLOSING = 1011;
        public const  int DTWAIN_TN_UICLOSED = 1012;
        public const  int DTWAIN_TN_UIOPENED = 1013;
        public const  int DTWAIN_TN_UIOPENING = 1055;
        public const  int DTWAIN_TN_UIOPENFAILURE = 1060;
        public const  int DTWAIN_TN_CLIPTRANSFERDONE = 1014;
        public const  int DTWAIN_TN_INVALIDIMAGEFORMAT = 1015;
        public const  int DTWAIN_TN_ACQUIRETERMINATED = 1021;
        public const  int DTWAIN_TN_TRANSFERSTRIPREADY = 1022;
        public const  int DTWAIN_TN_TRANSFERSTRIPDONE = 1023;
        public const  int DTWAIN_TN_TRANSFERSTRIPFAILED = 1029;
        public const  int DTWAIN_TN_IMAGEINFOERROR = 1024;
        public const  int DTWAIN_TN_TRANSFERCANCELLED = 1030;
        public const  int DTWAIN_TN_FILESAVECANCELLED = 1031;
        public const  int DTWAIN_TN_FILESAVEOK = 1032;
        public const  int DTWAIN_TN_FILESAVEERROR = 1033;
        public const  int DTWAIN_TN_FILEPAGESAVEOK = 1034;
        public const  int DTWAIN_TN_FILEPAGESAVEERROR = 1035;
        public const  int DTWAIN_TN_PROCESSEDDIB = 1036;
        public const  int DTWAIN_TN_FEEDERLOADED = 1037;
        public const  int DTWAIN_TN_GENERALERROR = 1038;
        public const  int DTWAIN_TN_MANDUPFLIPPAGES = 1040;
        public const  int DTWAIN_TN_MANDUPSIDE1DONE = 1041;
        public const  int DTWAIN_TN_MANDUPSIDE2DONE = 1042;
        public const  int DTWAIN_TN_MANDUPPAGECOUNTERROR = 1043;
        public const  int DTWAIN_TN_MANDUPACQUIREDONE = 1044;
        public const  int DTWAIN_TN_MANDUPSIDE1START = 1045;
        public const  int DTWAIN_TN_MANDUPSIDE2START = 1046;
        public const  int DTWAIN_TN_MANDUPMERGEERROR = 1047;
        public const  int DTWAIN_TN_MANDUPMEMORYERROR = 1048;
        public const  int DTWAIN_TN_MANDUPFILEERROR = 1049;
        public const  int DTWAIN_TN_MANDUPFILESAVEERROR = 1050;
        public const  int DTWAIN_TN_ENDOFJOBDETECTED = 1051;
        public const  int DTWAIN_TN_EOJDETECTED = 1051;
        public const  int DTWAIN_TN_EOJDETECTED_XFERDONE = 1052;
        public const  int DTWAIN_TN_QUERYPAGEDISCARD = 1053;
        public const  int DTWAIN_TN_PAGEDISCARDED = 1054;
        public const  int DTWAIN_TN_PROCESSDIBACCEPTED = 1055;
        public const  int DTWAIN_TN_PROCESSDIBFINALACCEPTED = 1056;
        public const  int DTWAIN_TN_DEVICEEVENT = 1100;
        public const  int DTWAIN_TN_TWAINPAGECANCELLED = 1105;
        public const  int DTWAIN_TN_TWAINPAGEFAILED = 1106;
        public const  int DTWAIN_TN_APPUPDATEDDIB = 1107;
        public const  int DTWAIN_TN_FILEPAGESAVING = 1110;
        public const  int DTWAIN_TN_EOJBEGINFILESAVE = 1112;
        public const  int DTWAIN_TN_EOJENDFILESAVE = 1113;
        public const  int DTWAIN_TN_CROPFAILED = 1120;
        public const  int DTWAIN_TN_PROCESSEDDIBFINAL = 1121;
        public const  int DTWAIN_TN_BLANKPAGEDETECTED1 = 1130;
        public const  int DTWAIN_TN_BLANKPAGEDETECTED2 = 1131;
        public const  int DTWAIN_TN_BLANKPAGEDETECTED3 = 1132;
        public const  int DTWAIN_TN_BLANKPAGEDISCARDED1 = 1133;
        public const  int DTWAIN_TN_BLANKPAGEDISCARDED2 = 1134;
        public const  int DTWAIN_TN_OCRTEXTRETRIEVED = 1140;
        public const  int DTWAIN_TN_QUERYOCRTEXT = 1141;
        public const  int DTWAIN_TN_PDFOCRREADY = 1142;
        public const  int DTWAIN_TN_PDFOCRDONE = 1143;
        public const  int DTWAIN_TN_PDFOCRERROR = 1144;
        public const  int DTWAIN_TN_SETCALLBACKINIT = 1150;
        public const  int DTWAIN_TN_SETCALLBACK64INIT = 1151;
        public const  int DTWAIN_TN_FILENAMECHANGING = 1160;
        public const  int DTWAIN_TN_FILENAMECHANGED = 1161;
	    public const  int DTWAIN_TN_PROCESSEDAUDIOFINAL = 1180;
	    public const  int DTWAIN_TN_PROCESSAUDIOFINALACCEPTED = 1181;
	    public const  int DTWAIN_TN_PROCESSEDAUDIOFILE = 1182;
        public const  int DTWAIN_PDFOCR_CLEANTEXT1 = 1;
        public const  int DTWAIN_PDFOCR_CLEANTEXT2 = 2;
        public const  int DTWAIN_MODAL = 0;
        public const  int DTWAIN_MODELESS = 1;
        public const  int DTWAIN_UIModeCLOSE = 0;
        public const  int DTWAIN_UIModeOPEN = 1;
        public const  int DTWAIN_REOPEN_SOURCE = 2;
        public const  int DTWAIN_ROUNDNEAREST = 0;
        public const  int DTWAIN_ROUNDUP = 1;
        public const  int DTWAIN_ROUNDDOWN = 2;
        public const  double DTWAIN_FLOATDELTA = (+1.0e-8);
        public const  int DTWAIN_OR_ROT0 = 0;
        public const  int DTWAIN_OR_ROT90 = 1;
        public const  int DTWAIN_OR_ROT180 = 2;
        public const  int DTWAIN_OR_ROT270 = 3;
        public const  int DTWAIN_OR_PORTRAIT = DTWAIN_OR_ROT0;
        public const  int DTWAIN_OR_LANDSCAPE = DTWAIN_OR_ROT270;
        public const  int DTWAIN_OR_ANYROTATION = (-1);
        public const  int DTWAIN_CO_GET = 0x0001;
        public const  int DTWAIN_CO_SET = 0x0002;
        public const  int DTWAIN_CO_GETDEFAULT = 0x0004;
        public const  int DTWAIN_CO_GETCURRENT = 0x0008;
        public const  int DTWAIN_CO_RESET = 0x0010;
        public const  int DTWAIN_CO_SETCONSTRAINT = 0x0020;
        public const  int DTWAIN_CO_CONSTRAINABLE = 0x0040;
        public const  int DTWAIN_CO_GETHELP = 0x0100;
        public const  int DTWAIN_CO_GETLABEL = 0x0200;
        public const  int DTWAIN_CO_GETLABELENUM = 0x0400;
        public const  int DTWAIN_CNTYAFGHANISTAN = 1001;
        public const  int DTWAIN_CNTYALGERIA = 213;
        public const  int DTWAIN_CNTYAMERICANSAMOA = 684;
        public const  int DTWAIN_CNTYANDORRA = 033;
        public const  int DTWAIN_CNTYANGOLA = 1002;
        public const  int DTWAIN_CNTYANGUILLA = 8090;
        public const  int DTWAIN_CNTYANTIGUA = 8091;
        public const  int DTWAIN_CNTYARGENTINA = 54;
        public const  int DTWAIN_CNTYARUBA = 297;
        public const  int DTWAIN_CNTYASCENSIONI = 247;
        public const  int DTWAIN_CNTYAUSTRALIA = 61;
        public const  int DTWAIN_CNTYAUSTRIA = 43;
        public const  int DTWAIN_CNTYBAHAMAS = 8092;
        public const  int DTWAIN_CNTYBAHRAIN = 973;
        public const  int DTWAIN_CNTYBANGLADESH = 880;
        public const  int DTWAIN_CNTYBARBADOS = 8093;
        public const  int DTWAIN_CNTYBELGIUM = 32;
        public const  int DTWAIN_CNTYBELIZE = 501;
        public const  int DTWAIN_CNTYBENIN = 229;
        public const  int DTWAIN_CNTYBERMUDA = 8094;
        public const  int DTWAIN_CNTYBHUTAN = 1003;
        public const  int DTWAIN_CNTYBOLIVIA = 591;
        public const  int DTWAIN_CNTYBOTSWANA = 267;
        public const  int DTWAIN_CNTYBRITAIN = 6;
        public const  int DTWAIN_CNTYBRITVIRGINIS = 8095;
        public const  int DTWAIN_CNTYBRAZIL = 55;
        public const  int DTWAIN_CNTYBRUNEI = 673;
        public const  int DTWAIN_CNTYBULGARIA = 359;
        public const  int DTWAIN_CNTYBURKINAFASO = 1004;
        public const  int DTWAIN_CNTYBURMA = 1005;
        public const  int DTWAIN_CNTYBURUNDI = 1006;
        public const  int DTWAIN_CNTYCAMAROON = 237;
        public const  int DTWAIN_CNTYCANADA = 2;
        public const  int DTWAIN_CNTYCAPEVERDEIS = 238;
        public const  int DTWAIN_CNTYCAYMANIS = 8096;
        public const  int DTWAIN_CNTYCENTRALAFREP = 1007;
        public const  int DTWAIN_CNTYCHAD = 1008;
        public const  int DTWAIN_CNTYCHILE = 56;
        public const  int DTWAIN_CNTYCHINA = 86;
        public const  int DTWAIN_CNTYCHRISTMASIS = 1009;
        public const  int DTWAIN_CNTYCOCOSIS = 1009;
        public const  int DTWAIN_CNTYCOLOMBIA = 57;
        public const  int DTWAIN_CNTYCOMOROS = 1010;
        public const  int DTWAIN_CNTYCONGO = 1011;
        public const  int DTWAIN_CNTYCOOKIS = 1012;
        public const  int DTWAIN_CNTYCOSTARICA = 506;
        public const  int DTWAIN_CNTYCUBA = 005;
        public const  int DTWAIN_CNTYCYPRUS = 357;
        public const  int DTWAIN_CNTYCZECHOSLOVAKIA = 42;
        public const  int DTWAIN_CNTYDENMARK = 45;
        public const  int DTWAIN_CNTYDJIBOUTI = 1013;
        public const  int DTWAIN_CNTYDOMINICA = 8097;
        public const  int DTWAIN_CNTYDOMINCANREP = 8098;
        public const  int DTWAIN_CNTYEASTERIS = 1014;
        public const  int DTWAIN_CNTYECUADOR = 593;
        public const  int DTWAIN_CNTYEGYPT = 20;
        public const  int DTWAIN_CNTYELSALVADOR = 503;
        public const  int DTWAIN_CNTYEQGUINEA = 1015;
        public const  int DTWAIN_CNTYETHIOPIA = 251;
        public const  int DTWAIN_CNTYFALKLANDIS = 1016;
        public const  int DTWAIN_CNTYFAEROEIS = 298;
        public const  int DTWAIN_CNTYFIJIISLANDS = 679;
        public const  int DTWAIN_CNTYFINLAND = 358;
        public const  int DTWAIN_CNTYFRANCE = 33;
        public const  int DTWAIN_CNTYFRANTILLES = 596;
        public const  int DTWAIN_CNTYFRGUIANA = 594;
        public const  int DTWAIN_CNTYFRPOLYNEISA = 689;
        public const  int DTWAIN_CNTYFUTANAIS = 1043;
        public const  int DTWAIN_CNTYGABON = 241;
        public const  int DTWAIN_CNTYGAMBIA = 220;
        public const  int DTWAIN_CNTYGERMANY = 49;
        public const  int DTWAIN_CNTYGHANA = 233;
        public const  int DTWAIN_CNTYGIBRALTER = 350;
        public const  int DTWAIN_CNTYGREECE = 30;
        public const  int DTWAIN_CNTYGREENLAND = 299;
        public const  int DTWAIN_CNTYGRENADA = 8099;
        public const  int DTWAIN_CNTYGRENEDINES = 8015;
        public const  int DTWAIN_CNTYGUADELOUPE = 590;
        public const  int DTWAIN_CNTYGUAM = 671;
        public const  int DTWAIN_CNTYGUANTANAMOBAY = 5399;
        public const  int DTWAIN_CNTYGUATEMALA = 502;
        public const  int DTWAIN_CNTYGUINEA = 224;
        public const  int DTWAIN_CNTYGUINEABISSAU = 1017;
        public const  int DTWAIN_CNTYGUYANA = 592;
        public const  int DTWAIN_CNTYHAITI = 509;
        public const  int DTWAIN_CNTYHONDURAS = 504;
        public const  int DTWAIN_CNTYHONGKONG = 852;
        public const  int DTWAIN_CNTYHUNGARY = 36;
        public const  int DTWAIN_CNTYICELAND = 354;
        public const  int DTWAIN_CNTYINDIA = 91;
        public const  int DTWAIN_CNTYINDONESIA = 62;
        public const  int DTWAIN_CNTYIRAN = 98;
        public const  int DTWAIN_CNTYIRAQ = 964;
        public const  int DTWAIN_CNTYIRELAND = 353;
        public const  int DTWAIN_CNTYISRAEL = 972;
        public const  int DTWAIN_CNTYITALY = 39;
        public const  int DTWAIN_CNTYIVORYCOAST = 225;
        public const  int DTWAIN_CNTYJAMAICA = 8010;
        public const  int DTWAIN_CNTYJAPAN = 81;
        public const  int DTWAIN_CNTYJORDAN = 962;
        public const  int DTWAIN_CNTYKENYA = 254;
        public const  int DTWAIN_CNTYKIRIBATI = 1018;
        public const  int DTWAIN_CNTYKOREA = 82;
        public const  int DTWAIN_CNTYKUWAIT = 965;
        public const  int DTWAIN_CNTYLAOS = 1019;
        public const  int DTWAIN_CNTYLEBANON = 1020;
        public const  int DTWAIN_CNTYLIBERIA = 231;
        public const  int DTWAIN_CNTYLIBYA = 218;
        public const  int DTWAIN_CNTYLIECHTENSTEIN = 41;
        public const  int DTWAIN_CNTYLUXENBOURG = 352;
        public const  int DTWAIN_CNTYMACAO = 853;
        public const  int DTWAIN_CNTYMADAGASCAR = 1021;
        public const  int DTWAIN_CNTYMALAWI = 265;
        public const  int DTWAIN_CNTYMALAYSIA = 60;
        public const  int DTWAIN_CNTYMALDIVES = 960;
        public const  int DTWAIN_CNTYMALI = 1022;
        public const  int DTWAIN_CNTYMALTA = 356;
        public const  int DTWAIN_CNTYMARSHALLIS = 692;
        public const  int DTWAIN_CNTYMAURITANIA = 1023;
        public const  int DTWAIN_CNTYMAURITIUS = 230;
        public const  int DTWAIN_CNTYMEXICO = 3;
        public const  int DTWAIN_CNTYMICRONESIA = 691;
        public const  int DTWAIN_CNTYMIQUELON = 508;
        public const  int DTWAIN_CNTYMONACO = 33;
        public const  int DTWAIN_CNTYMONGOLIA = 1024;
        public const  int DTWAIN_CNTYMONTSERRAT = 8011;
        public const  int DTWAIN_CNTYMOROCCO = 212;
        public const  int DTWAIN_CNTYMOZAMBIQUE = 1025;
        public const  int DTWAIN_CNTYNAMIBIA = 264;
        public const  int DTWAIN_CNTYNAURU = 1026;
        public const  int DTWAIN_CNTYNEPAL = 977;
        public const  int DTWAIN_CNTYNETHERLANDS = 31;
        public const  int DTWAIN_CNTYNETHANTILLES = 599;
        public const  int DTWAIN_CNTYNEVIS = 8012;
        public const  int DTWAIN_CNTYNEWCALEDONIA = 687;
        public const  int DTWAIN_CNTYNEWZEALAND = 64;
        public const  int DTWAIN_CNTYNICARAGUA = 505;
        public const  int DTWAIN_CNTYNIGER = 227;
        public const  int DTWAIN_CNTYNIGERIA = 234;
        public const  int DTWAIN_CNTYNIUE = 1027;
        public const  int DTWAIN_CNTYNORFOLKI = 1028;
        public const  int DTWAIN_CNTYNORWAY = 47;
        public const  int DTWAIN_CNTYOMAN = 968;
        public const  int DTWAIN_CNTYPAKISTAN = 92;
        public const  int DTWAIN_CNTYPALAU = 1029;
        public const  int DTWAIN_CNTYPANAMA = 507;
        public const  int DTWAIN_CNTYPARAGUAY = 595;
        public const  int DTWAIN_CNTYPERU = 51;
        public const  int DTWAIN_CNTYPHILLIPPINES = 63;
        public const  int DTWAIN_CNTYPITCAIRNIS = 1030;
        public const  int DTWAIN_CNTYPNEWGUINEA = 675;
        public const  int DTWAIN_CNTYPOLAND = 48;
        public const  int DTWAIN_CNTYPORTUGAL = 351;
        public const  int DTWAIN_CNTYQATAR = 974;
        public const  int DTWAIN_CNTYREUNIONI = 1031;
        public const  int DTWAIN_CNTYROMANIA = 40;
        public const  int DTWAIN_CNTYRWANDA = 250;
        public const  int DTWAIN_CNTYSAIPAN = 670;
        public const  int DTWAIN_CNTYSANMARINO = 39;
        public const  int DTWAIN_CNTYSAOTOME = 1033;
        public const  int DTWAIN_CNTYSAUDIARABIA = 966;
        public const  int DTWAIN_CNTYSENEGAL = 221;
        public const  int DTWAIN_CNTYSEYCHELLESIS = 1034;
        public const  int DTWAIN_CNTYSIERRALEONE = 1035;
        public const  int DTWAIN_CNTYSINGAPORE = 65;
        public const  int DTWAIN_CNTYSOLOMONIS = 1036;
        public const  int DTWAIN_CNTYSOMALI = 1037;
        public const  int DTWAIN_CNTYSOUTHAFRICA = 27;
        public const  int DTWAIN_CNTYSPAIN = 34;
        public const  int DTWAIN_CNTYSRILANKA = 94;
        public const  int DTWAIN_CNTYSTHELENA = 1032;
        public const  int DTWAIN_CNTYSTKITTS = 8013;
        public const  int DTWAIN_CNTYSTLUCIA = 8014;
        public const  int DTWAIN_CNTYSTPIERRE = 508;
        public const  int DTWAIN_CNTYSTVINCENT = 8015;
        public const  int DTWAIN_CNTYSUDAN = 1038;
        public const  int DTWAIN_CNTYSURINAME = 597;
        public const  int DTWAIN_CNTYSWAZILAND = 268;
        public const  int DTWAIN_CNTYSWEDEN = 46;
        public const  int DTWAIN_CNTYSWITZERLAND = 41;
        public const  int DTWAIN_CNTYSYRIA = 1039;
        public const  int DTWAIN_CNTYTAIWAN = 886;
        public const  int DTWAIN_CNTYTANZANIA = 255;
        public const  int DTWAIN_CNTYTHAILAND = 66;
        public const  int DTWAIN_CNTYTOBAGO = 8016;
        public const  int DTWAIN_CNTYTOGO = 228;
        public const  int DTWAIN_CNTYTONGAIS = 676;
        public const  int DTWAIN_CNTYTRINIDAD = 8016;
        public const  int DTWAIN_CNTYTUNISIA = 216;
        public const  int DTWAIN_CNTYTURKEY = 90;
        public const  int DTWAIN_CNTYTURKSCAICOS = 8017;
        public const  int DTWAIN_CNTYTUVALU = 1040;
        public const  int DTWAIN_CNTYUGANDA = 256;
        public const  int DTWAIN_CNTYUSSR = 7;
        public const  int DTWAIN_CNTYUAEMIRATES = 971;
        public const  int DTWAIN_CNTYUNITEDKINGDOM = 44;
        public const  int DTWAIN_CNTYUSA = 1;
        public const  int DTWAIN_CNTYURUGUAY = 598;
        public const  int DTWAIN_CNTYVANUATU = 1041;
        public const  int DTWAIN_CNTYVATICANCITY = 39;
        public const  int DTWAIN_CNTYVENEZUELA = 58;
        public const  int DTWAIN_CNTYWAKE = 1042;
        public const  int DTWAIN_CNTYWALLISIS = 1043;
        public const  int DTWAIN_CNTYWESTERNSAHARA = 1044;
        public const  int DTWAIN_CNTYWESTERNSAMOA = 1045;
        public const  int DTWAIN_CNTYYEMEN = 1046;
        public const  int DTWAIN_CNTYYUGOSLAVIA = 38;
        public const  int DTWAIN_CNTYZAIRE = 243;
        public const  int DTWAIN_CNTYZAMBIA = 260;
        public const  int DTWAIN_CNTYZIMBABWE = 263;
        public const  int DTWAIN_LANGDANISH = 0;
        public const  int DTWAIN_LANGDUTCH = 1;
        public const  int DTWAIN_LANGINTERNATIONALENGLISH = 2;
        public const  int DTWAIN_LANGFRENCHCANADIAN = 3;
        public const  int DTWAIN_LANGFINNISH = 4;
        public const  int DTWAIN_LANGFRENCH = 5;
        public const  int DTWAIN_LANGGERMAN = 6;
        public const  int DTWAIN_LANGICELANDIC = 7;
        public const  int DTWAIN_LANGITALIAN = 8;
        public const  int DTWAIN_LANGNORWEGIAN = 9;
        public const  int DTWAIN_LANGPORTUGUESE = 10;
        public const  int DTWAIN_LANGSPANISH = 11;
        public const  int DTWAIN_LANGSWEDISH = 12;
        public const  int DTWAIN_LANGUSAENGLISH = 13;
        public const  int DTWAIN_NO_ERROR = (0);
        public const  int DTWAIN_ERR_FIRST = (-1000);
        public const  int DTWAIN_ERR_BAD_HANDLE = (-1001);
        public const  int DTWAIN_ERR_BAD_SOURCE = (-1002);
        public const  int DTWAIN_ERR_BAD_ARRAY = (-1003);
        public const  int DTWAIN_ERR_WRONG_ARRAY_TYPE = (-1004);
        public const  int DTWAIN_ERR_INDEX_BOUNDS = (-1005);
        public const  int DTWAIN_ERR_OUT_OF_MEMORY = (-1006);
        public const  int DTWAIN_ERR_NULL_WINDOW = (-1007);
        public const  int DTWAIN_ERR_BAD_PIXTYPE = (-1008);
        public const  int DTWAIN_ERR_BAD_CONTAINER = (-1009);
        public const  int DTWAIN_ERR_NO_SESSION = (-1010);
        public const  int DTWAIN_ERR_BAD_ACQUIRE_NUM = (-1011);
        public const  int DTWAIN_ERR_BAD_CAP = (-1012);
        public const  int DTWAIN_ERR_CAP_NO_SUPPORT = (-1013);
        public const  int DTWAIN_ERR_TWAIN = (-1014);
        public const  int DTWAIN_ERR_HOOK_FAILED = (-1015);
        public const  int DTWAIN_ERR_BAD_FILENAME = (-1016);
        public const  int DTWAIN_ERR_EMPTY_ARRAY = (-1017);
        public const  int DTWAIN_ERR_FILE_FORMAT = (-1018);
        public const  int DTWAIN_ERR_BAD_DIB_PAGE = (-1019);
        public const  int DTWAIN_ERR_SOURCE_ACQUIRING = (-1020);
        public const  int DTWAIN_ERR_INVALID_PARAM = (-1021);
        public const  int DTWAIN_ERR_INVALID_RANGE = (-1022);
        public const  int DTWAIN_ERR_UI_ERROR = (-1023);
        public const  int DTWAIN_ERR_BAD_UNIT = (-1024);
        public const  int DTWAIN_ERR_LANGDLL_NOT_FOUND = (-1025);
        public const  int DTWAIN_ERR_SOURCE_NOT_OPEN = (-1026);
        public const  int DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED = (-1027);
        public const  int DTWAIN_ERR_UIONLY_NOT_SUPPORTED = (-1028);
        public const  int DTWAIN_ERR_UI_ALREADY_OPENED = (-1029);
        public const  int DTWAIN_ERR_CAPSET_NOSUPPORT = (-1030);
        public const  int DTWAIN_ERR_NO_FILE_XFER = (-1031);
        public const  int DTWAIN_ERR_INVALID_BITDEPTH = (-1032);
        public const  int DTWAIN_ERR_NO_CAPS_DEFINED = (-1033);
        public const  int DTWAIN_ERR_TILES_NOT_SUPPORTED = (-1034);
        public const  int DTWAIN_ERR_INVALID_DTWAIN_FRAME = (-1035);
        public const  int DTWAIN_ERR_LIMITED_VERSION = (-1036);
        public const  int DTWAIN_ERR_NO_FEEDER = (-1037);
        public const  int DTWAIN_ERR_NO_FEEDER_QUERY = (-1038);
        public const  int DTWAIN_ERR_EXCEPTION_ERROR = (-1039);
        public const  int DTWAIN_ERR_INVALID_STATE = (-1040);
        public const  int DTWAIN_ERR_UNSUPPORTED_EXTINFO = (-1041);
        public const  int DTWAIN_ERR_DLLRESOURCE_NOTFOUND = (-1042);
        public const  int DTWAIN_ERR_NOT_INITIALIZED = (-1043);
        public const  int DTWAIN_ERR_NO_SOURCES = (-1044);
        public const  int DTWAIN_ERR_TWAIN_NOT_INSTALLED = (-1045);
        public const  int DTWAIN_ERR_WRONG_THREAD = (-1046);
        public const  int DTWAIN_ERR_BAD_CAPTYPE = (-1047);
        public const  int DTWAIN_ERR_UNKNOWN_CAPDATATYPE = (-1048);
        public const  int DTWAIN_ERR_DEMO_NOFILETYPE = (-1049);
        public const  int DTWAIN_ERR_LAST_1 = DTWAIN_ERR_DEMO_NOFILETYPE;
        public const  int TWAIN_ERR_LOW_MEMORY = (-1100);
        public const  int TWAIN_ERR_FALSE_ALARM = (-1101);
        public const  int TWAIN_ERR_BUMMER = (-1102);
        public const  int TWAIN_ERR_NODATASOURCE = (-1103);
        public const  int TWAIN_ERR_MAXCONNECTIONS = (-1104);
        public const  int TWAIN_ERR_OPERATIONERROR = (-1105);
        public const  int TWAIN_ERR_BADCAPABILITY = (-1106);
        public const  int TWAIN_ERR_BADVALUE = (-1107);
        public const  int TWAIN_ERR_BADPROTOCOL = (-1108);
        public const  int TWAIN_ERR_SEQUENCEERROR = (-1109);
        public const  int TWAIN_ERR_BADDESTINATION = (-1110);
        public const  int TWAIN_ERR_CAPNOTSUPPORTED = (-1111);
        public const  int TWAIN_ERR_CAPBADOPERATION = (-1112);
        public const  int TWAIN_ERR_CAPSEQUENCEERROR = (-1113);
        public const  int TWAIN_ERR_FILEPROTECTEDERROR = (-1114);
        public const  int TWAIN_ERR_FILEEXISTERROR = (-1115);
        public const  int TWAIN_ERR_FILENOTFOUND = (-1116);
        public const  int TWAIN_ERR_DIRNOTEMPTY = (-1117);
        public const  int TWAIN_ERR_FEEDERJAMMED = (-1118);
        public const  int TWAIN_ERR_FEEDERMULTPAGES = (-1119);
        public const  int TWAIN_ERR_FEEDERWRITEERROR = (-1120);
        public const  int TWAIN_ERR_DEVICEOFFLINE = (-1121);
        public const  int TWAIN_ERR_NULL_CONTAINER = (-1122);
        public const  int TWAIN_ERR_INTERLOCK = (-1123);
        public const  int TWAIN_ERR_DAMAGEDCORNER = (-1124);
        public const  int TWAIN_ERR_FOCUSERROR = (-1125);
        public const  int TWAIN_ERR_DOCTOOLIGHT = (-1126);
        public const  int TWAIN_ERR_DOCTOODARK = (-1127);
        public const  int TWAIN_ERR_NOMEDIA = (-1128);
        public const  int DTWAIN_ERR_FILEXFERSTART = (-2000);
        public const  int DTWAIN_ERR_MEM = (-2001);
        public const  int DTWAIN_ERR_FILEOPEN = (-2002);
        public const  int DTWAIN_ERR_FILEREAD = (-2003);
        public const  int DTWAIN_ERR_FILEWRITE = (-2004);
        public const  int DTWAIN_ERR_BADPARAM = (-2005);
        public const  int DTWAIN_ERR_INVALIDBMP = (-2006);
        public const  int DTWAIN_ERR_BMPRLE = (-2007);
        public const  int DTWAIN_ERR_RESERVED1 = (-2008);
        public const  int DTWAIN_ERR_INVALIDJPG = (-2009);
        public const  int DTWAIN_ERR_DC = (-2010);
        public const  int DTWAIN_ERR_DIB = (-2011);
        public const  int DTWAIN_ERR_RESERVED2 = (-2012);
        public const  int DTWAIN_ERR_NORESOURCE = (-2013);
        public const  int DTWAIN_ERR_CALLBACKCANCEL = (-2014);
        public const  int DTWAIN_ERR_INVALIDPNG = (-2015);
        public const  int DTWAIN_ERR_PNGCREATE = (-2016);
        public const  int DTWAIN_ERR_INTERNAL = (-2017);
        public const  int DTWAIN_ERR_FONT = (-2018);
        public const  int DTWAIN_ERR_INTTIFF = (-2019);
        public const  int DTWAIN_ERR_INVALIDTIFF = (-2020);
        public const  int DTWAIN_ERR_NOTIFFLZW = (-2021);
        public const  int DTWAIN_ERR_INVALIDPCX = (-2022);
        public const  int DTWAIN_ERR_CREATEBMP = (-2023);
        public const  int DTWAIN_ERR_NOLINES = (-2024);
        public const  int DTWAIN_ERR_GETDIB = (-2025);
        public const  int DTWAIN_ERR_NODEVOP = (-2026);
        public const  int DTWAIN_ERR_INVALIDWMF = (-2027);
        public const  int DTWAIN_ERR_DEPTHMISMATCH = (-2028);
        public const  int DTWAIN_ERR_BITBLT = (-2029);
        public const  int DTWAIN_ERR_BUFTOOSMALL = (-2030);
        public const  int DTWAIN_ERR_TOOMANYCOLORS = (-2031);
        public const  int DTWAIN_ERR_INVALIDTGA = (-2032);
        public const  int DTWAIN_ERR_NOTGATHUMBNAIL = (-2033);
        public const  int DTWAIN_ERR_RESERVED3 = (-2034);
        public const  int DTWAIN_ERR_CREATEDIB = (-2035);
        public const  int DTWAIN_ERR_NOLZW = (-2036);
        public const  int DTWAIN_ERR_SELECTOBJ = (-2037);
        public const  int DTWAIN_ERR_BADMANAGER = (-2038);
        public const  int DTWAIN_ERR_OBSOLETE = (-2039);
        public const  int DTWAIN_ERR_CREATEDIBSECTION = (-2040);
        public const  int DTWAIN_ERR_SETWINMETAFILEBITS = (-2041);
        public const  int DTWAIN_ERR_GETWINMETAFILEBITS = (-2042);
        public const  int DTWAIN_ERR_PAXPWD = (-2043);
        public const  int DTWAIN_ERR_INVALIDPAX = (-2044);
        public const  int DTWAIN_ERR_NOSUPPORT = (-2045);
        public const  int DTWAIN_ERR_INVALIDPSD = (-2046);
        public const  int DTWAIN_ERR_PSDNOTSUPPORTED = (-2047);
        public const  int DTWAIN_ERR_DECRYPT = (-2048);
        public const  int DTWAIN_ERR_ENCRYPT = (-2049);
        public const  int DTWAIN_ERR_COMPRESSION = (-2050);
        public const  int DTWAIN_ERR_DECOMPRESSION = (-2051);
        public const  int DTWAIN_ERR_INVALIDTLA = (-2052);
        public const  int DTWAIN_ERR_INVALIDWBMP = (-2053);
        public const  int DTWAIN_ERR_NOTIFFTAG = (-2054);
        public const  int DTWAIN_ERR_NOLOCALSTORAGE = (-2055);
        public const  int DTWAIN_ERR_INVALIDEXIF = (-2056);
        public const  int DTWAIN_ERR_NOEXIFSTRING = (-2057);
        public const  int DTWAIN_ERR_TIFFDLL32NOTFOUND = (-2058);
        public const  int DTWAIN_ERR_TIFFDLL16NOTFOUND = (-2059);
        public const  int DTWAIN_ERR_PNGDLL16NOTFOUND = (-2060);
        public const  int DTWAIN_ERR_JPEGDLL16NOTFOUND = (-2061);
        public const  int DTWAIN_ERR_BADBITSPERPIXEL = (-2062);
        public const  int DTWAIN_ERR_TIFFDLL32INVALIDVER = (-2063);
        public const  int DTWAIN_ERR_PDFDLL32NOTFOUND = (-2064);
        public const  int DTWAIN_ERR_PDFDLL32INVALIDVER = (-2065);
        public const  int DTWAIN_ERR_JPEGDLL32NOTFOUND = (-2066);
        public const  int DTWAIN_ERR_JPEGDLL32INVALIDVER = (-2067);
        public const  int DTWAIN_ERR_PNGDLL32NOTFOUND = (-2068);
        public const  int DTWAIN_ERR_PNGDLL32INVALIDVER = (-2069);
        public const  int DTWAIN_ERR_J2KDLL32NOTFOUND = (-2070);
        public const  int DTWAIN_ERR_J2KDLL32INVALIDVER = (-2071);
        public const  int DTWAIN_ERR_MANDUPLEX_UNAVAILABLE = (-2072);
        public const  int DTWAIN_ERR_TIMEOUT = (-2073);
        public const  int DTWAIN_ERR_INVALIDICONFORMAT = (-2074);
        public const  int DTWAIN_ERR_TWAIN32DSMNOTFOUND = (-2075);
        public const  int DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND = (-2076);
        public const  int DTWAIN_TWAINSAVE_OK = (0);
        public const  int DTWAIN_ERR_TS_FIRST = (-2080);
        public const  int DTWAIN_ERR_TS_NOFILENAME = (-2081);
        public const  int DTWAIN_ERR_TS_NOTWAINSYS = (-2082);
        public const  int DTWAIN_ERR_TS_DEVICEFAILURE = (-2083);
        public const  int DTWAIN_ERR_TS_FILESAVEERROR = (-2084);
        public const  int DTWAIN_ERR_TS_COMMANDILLEGAL = (-2085);
        public const  int DTWAIN_ERR_TS_CANCELLED = (-2086);
        public const  int DTWAIN_ERR_TS_ACQUISITIONERROR = (-2087);
        public const  int DTWAIN_ERR_TS_INVALIDCOLORSPACE = (-2088);
        public const  int DTWAIN_ERR_TS_PDFNOTSUPPORTED = (-2089);
        public const  int DTWAIN_ERR_TS_NOTAVAILABLE = (-2090);
        public const  int DTWAIN_ERR_OCR_FIRST = (-2100);
        public const  int DTWAIN_ERR_OCR_INVALIDPAGENUM = (-2101);
        public const  int DTWAIN_ERR_OCR_INVALIDENGINE = (-2102);
        public const  int DTWAIN_ERR_OCR_NOTACTIVE = (-2103);
        public const  int DTWAIN_ERR_OCR_INVALIDFILETYPE = (-2104);
        public const  int DTWAIN_ERR_OCR_INVALIDPIXELTYPE = (-2105);
        public const  int DTWAIN_ERR_OCR_INVALIDBITDEPTH = (-2106);
        public const  int DTWAIN_ERR_OCR_RECOGNITIONERROR = (-2107);
        public const  int DTWAIN_ERR_OCR_LAST = (-2108);
        public const  int DTWAIN_ERR_LAST = DTWAIN_ERR_OCR_LAST;
        public const  int DTWAIN_DE_CHKAUTOCAPTURE = 1;
        public const  int DTWAIN_DE_CHKBATTERY = 2;
        public const  int DTWAIN_DE_CHKDEVICEONLINE = 4;
        public const  int DTWAIN_DE_CHKFLASH = 8;
        public const  int DTWAIN_DE_CHKPOWERSUPPLY = 16;
        public const  int DTWAIN_DE_CHKRESOLUTION = 32;
        public const  int DTWAIN_DE_DEVICEADDED = 64;
        public const  int DTWAIN_DE_DEVICEOFFLINE = 128;
        public const  int DTWAIN_DE_DEVICEREADY = 256;
        public const  int DTWAIN_DE_DEVICEREMOVED = 512;
        public const  int DTWAIN_DE_IMAGECAPTURED = 1024;
        public const  int DTWAIN_DE_IMAGEDELETED = 2048;
        public const  int DTWAIN_DE_PAPERDOUBLEFEED = 4096;
        public const  int DTWAIN_DE_PAPERJAM = 8192;
        public const  int DTWAIN_DE_LAMPFAILURE = 16384;
        public const  int DTWAIN_DE_POWERSAVE = 32768;
        public const  int DTWAIN_DE_POWERSAVENOTIFY = 65536;
        public const  int DTWAIN_DE_CUSTOMEVENTS = 0x8000;
        public const  int DTWAIN_GETDE_EVENT = 0;
        public const  int DTWAIN_GETDE_DEVNAME = 1;
        public const  int DTWAIN_GETDE_BATTERYMINUTES = 2;
        public const  int DTWAIN_GETDE_BATTERYPCT = 3;
        public const  int DTWAIN_GETDE_XRESOLUTION = 4;
        public const  int DTWAIN_GETDE_YRESOLUTION = 5;
        public const  int DTWAIN_GETDE_FLASHUSED = 6;
        public const  int DTWAIN_GETDE_AUTOCAPTURE = 7;
        public const  int DTWAIN_GETDE_TIMEBEFORECAPTURE = 8;
        public const  int DTWAIN_GETDE_TIMEBETWEENCAPTURES = 9;
        public const  int DTWAIN_GETDE_POWERSUPPLY = 10;
        public const  int DTWAIN_IMPRINTERTOPBEFORE = 1;
        public const  int DTWAIN_IMPRINTERTOPAFTER = 2;
        public const  int DTWAIN_IMPRINTERBOTTOMBEFORE = 4;
        public const  int DTWAIN_IMPRINTERBOTTOMAFTER = 8;
        public const  int DTWAIN_ENDORSERTOPBEFORE = 16;
        public const  int DTWAIN_ENDORSERTOPAFTER = 32;
        public const  int DTWAIN_ENDORSERBOTTOMBEFORE = 64;
        public const  int DTWAIN_ENDORSERBOTTOMAFTER = 128;
        public const  int DTWAIN_PM_SINGLESTRING = 0;
        public const  int DTWAIN_PM_MULTISTRING = 1;
        public const  int DTWAIN_PM_COMPOUNDSTRING = 2;
        public const  int DTWAIN_TWTY_INT8 = 0x0000;
        public const  int DTWAIN_TWTY_INT16 = 0x0001;
        public const  int DTWAIN_TWTY_INT32 = 0x0002;
        public const  int DTWAIN_TWTY_UINT8 = 0x0003;
        public const  int DTWAIN_TWTY_UINT16 = 0x0004;
        public const  int DTWAIN_TWTY_UINT32 = 0x0005;
        public const  int DTWAIN_TWTY_BOOL = 0x0006;
        public const  int DTWAIN_TWTY_FIX32 = 0x0007;
        public const  int DTWAIN_TWTY_FRAME = 0x0008;
        public const  int DTWAIN_TWTY_STR32 = 0x0009;
        public const  int DTWAIN_TWTY_STR64 = 0x000A;
        public const  int DTWAIN_TWTY_STR128 = 0x000B;
        public const  int DTWAIN_TWTY_STR255 = 0x000C;
        public const  int DTWAIN_TWTY_STR1024 = 0x000D;
        public const  int DTWAIN_TWTY_UNI512 = 0x000E;
        public const  int DTWAIN_EI_BARCODEX = 0x1200;
        public const  int DTWAIN_EI_BARCODEY = 0x1201;
        public const  int DTWAIN_EI_BARCODETEXT = 0x1202;
        public const  int DTWAIN_EI_BARCODETYPE = 0x1203;
        public const  int DTWAIN_EI_DESHADETOP = 0x1204;
        public const  int DTWAIN_EI_DESHADELEFT = 0x1205;
        public const  int DTWAIN_EI_DESHADEHEIGHT = 0x1206;
        public const  int DTWAIN_EI_DESHADEWIDTH = 0x1207;
        public const  int DTWAIN_EI_DESHADESIZE = 0x1208;
        public const  int DTWAIN_EI_SPECKLESREMOVED = 0x1209;
        public const  int DTWAIN_EI_HORZLINEXCOORD = 0x120A;
        public const  int DTWAIN_EI_HORZLINEYCOORD = 0x120B;
        public const  int DTWAIN_EI_HORZLINELENGTH = 0x120C;
        public const  int DTWAIN_EI_HORZLINETHICKNESS = 0x120D;
        public const  int DTWAIN_EI_VERTLINEXCOORD = 0x120E;
        public const  int DTWAIN_EI_VERTLINEYCOORD = 0x120F;
        public const  int DTWAIN_EI_VERTLINELENGTH = 0x1210;
        public const  int DTWAIN_EI_VERTLINETHICKNESS = 0x1211;
        public const  int DTWAIN_EI_PATCHCODE = 0x1212;
        public const  int DTWAIN_EI_ENDORSEDTEXT = 0x1213;
        public const  int DTWAIN_EI_FORMCONFIDENCE = 0x1214;
        public const  int DTWAIN_EI_FORMTEMPLATEMATCH = 0x1215;
        public const  int DTWAIN_EI_FORMTEMPLATEPAGEMATCH = 0x1216;
        public const  int DTWAIN_EI_FORMHORZDOCOFFSET = 0x1217;
        public const  int DTWAIN_EI_FORMVERTDOCOFFSET = 0x1218;
        public const  int DTWAIN_EI_BARCODECOUNT = 0x1219;
        public const  int DTWAIN_EI_BARCODECONFIDENCE = 0x121A;
        public const  int DTWAIN_EI_BARCODEROTATION = 0x121B;
        public const  int DTWAIN_EI_BARCODETEXTLENGTH = 0x121C;
        public const  int DTWAIN_EI_DESHADECOUNT = 0x121D;
        public const  int DTWAIN_EI_DESHADEBLACKCOUNTOLD = 0x121E;
        public const  int DTWAIN_EI_DESHADEBLACKCOUNTNEW = 0x121F;
        public const  int DTWAIN_EI_DESHADEBLACKRLMIN = 0x1220;
        public const  int DTWAIN_EI_DESHADEBLACKRLMAX = 0x1221;
        public const  int DTWAIN_EI_DESHADEWHITECOUNTOLD = 0x1222;
        public const  int DTWAIN_EI_DESHADEWHITECOUNTNEW = 0x1223;
        public const  int DTWAIN_EI_DESHADEWHITERLMIN = 0x1224;
        public const  int DTWAIN_EI_DESHADEWHITERLAVE = 0x1225;
        public const  int DTWAIN_EI_DESHADEWHITERLMAX = 0x1226;
        public const  int DTWAIN_EI_BLACKSPECKLESREMOVED = 0x1227;
        public const  int DTWAIN_EI_WHITESPECKLESREMOVED = 0x1228;
        public const  int DTWAIN_EI_HORZLINECOUNT = 0x1229;
        public const  int DTWAIN_EI_VERTLINECOUNT = 0x122A;
        public const  int DTWAIN_EI_DESKEWSTATUS = 0x122B;
        public const  int DTWAIN_EI_SKEWORIGINALANGLE = 0x122C;
        public const  int DTWAIN_EI_SKEWFINALANGLE = 0x122D;
        public const  int DTWAIN_EI_SKEWCONFIDENCE = 0x122E;
        public const  int DTWAIN_EI_SKEWWINDOWX1 = 0x122F;
        public const  int DTWAIN_EI_SKEWWINDOWY1 = 0x1230;
        public const  int DTWAIN_EI_SKEWWINDOWX2 = 0x1231;
        public const  int DTWAIN_EI_SKEWWINDOWY2 = 0x1232;
        public const  int DTWAIN_EI_SKEWWINDOWX3 = 0x1233;
        public const  int DTWAIN_EI_SKEWWINDOWY3 = 0x1234;
        public const  int DTWAIN_EI_SKEWWINDOWX4 = 0x1235;
        public const  int DTWAIN_EI_SKEWWINDOWY4 = 0x1236;
        public const  int DTWAIN_EI_BOOKNAME = 0x1238;
        public const  int DTWAIN_EI_CHAPTERNUMBER = 0x1239;
        public const  int DTWAIN_EI_DOCUMENTNUMBER = 0x123A;
        public const  int DTWAIN_EI_PAGENUMBER = 0x123B;
        public const  int DTWAIN_EI_CAMERA = 0x123C;
        public const  int DTWAIN_EI_FRAMENUMBER = 0x123D;
        public const  int DTWAIN_EI_FRAME = 0x123E;
        public const  int DTWAIN_EI_PIXELFLAVOR = 0x123F;
        public const  int DTWAIN_LOG_DECODE_SOURCE = 1;
        public const  int DTWAIN_LOG_DECODE_DEST = 2;
        public const  int DTWAIN_LOG_DECODE_TWMEMREF = 4;
        public const  int DTWAIN_LOG_DECODE_TWEVENT = 8;
        public const  int DTWAIN_LOG_USEFILE = 16;
        public const  int DTWAIN_LOG_CALLSTACK = 32;
        public const  int DTWAIN_LOG_USEWINDOW = 64;
        public const  int DTWAIN_LOG_SHOWEXCEPTIONS = 128;
        public const  int DTWAIN_LOG_ERRORMSGBOX = 256;
        public const  int DTWAIN_LOG_INITFAILURE = 512;
        public const  int DTWAIN_LOG_USEBUFFER = 1024;
        public const  int DTWAIN_LOG_FILEAPPEND = 2048;
        public const  int DTWAIN_LOG_DECODE_BITMAP = 4096;
        public const  int DTWAIN_LOG_NOCALLBACK = 8192;
        public const  int DTWAIN_LOG_WRITE = 16384;
        public const  int DTWAIN_LOG_USECRLF = 32768;
        public const  uint DTWAIN_LOG_ALL = 0xFFFFF7FF;
        public const  uint DTWAIN_LOG_ALL_APPEND = 0xFFFFFFFF;
        public const  int DTWAINGCD_RETURNHANDLE = 1;
        public const  int DTWAINGCD_COPYDATA = 2;
        public const  int DTWAIN_BYPOSITION = 0;
        public const  int DTWAIN_BYID = 1;
        public const  int DTWAINSCD_USEHANDLE = 1;
        public const  int DTWAINSCD_USEDATA = 2;
        public const  int DTWAIN_PAGEFAIL_RETRY = 1;
        public const  int DTWAIN_PAGEFAIL_TERMINATE = 2;
        public const  int DTWAIN_MAXRETRY_ATTEMPTS = 3;
        public const  int DTWAIN_RETRY_FOREVER = (-1);
        public const  int DTWAIN_PDF_NOSCALING = 128;
        public const  int DTWAIN_PDF_FITPAGE = 256;
        public const  int DTWAIN_PDF_VARIABLEPAGESIZE = 512;
        public const  int DTWAIN_PDF_CUSTOMSIZE = 1024;
        public const  int DTWAIN_PDF_USECOMPRESSION = 2048;
        public const  int DTWAIN_PDF_CUSTOMSCALE = 4096;
        public const  int DTWAIN_PDF_PIXELSPERMETERSIZE = 8192;
        public const  int DTWAIN_PDF_ALLOWPRINTING = 2052;
        public const  int DTWAIN_PDF_ALLOWMOD = 8;
        public const  int DTWAIN_PDF_ALLOWCOPY = 16;
        public const  int DTWAIN_PDF_ALLOWMODANNOTATIONS = 32;
        public const  int DTWAIN_PDF_ALLOWFILLIN = 256;
        public const  int DTWAIN_PDF_ALLOWEXTRACTION = 512;
        public const  int DTWAIN_PDF_ALLOWASSEMBLY = 1024;
        public const  int DTWAIN_PDF_ALLOWDEGRADEDPRINTING = 4;
        public const  int DTWAIN_PDF_PORTRAIT = 0;
        public const  int DTWAIN_PDF_LANDSCAPE = 1;
        public const  int DTWAIN_PS_REGULAR = 0;
        public const  int DTWAIN_PS_ENCAPSULATED = 1;
        public const  int DTWAIN_BP_AUTODISCARD_NONE = 0;
        public const  int DTWAIN_BP_AUTODISCARD_IMMEDIATE = 1;
        public const  int DTWAIN_BP_AUTODISCARD_AFTERPROCESS = 2;
        public const  uint DTWAIN_BP_AUTODISCARD_ANY = 0xFFFF;
        public const  int DTWAIN_LP_REFLECTIVE = 0;
        public const  int DTWAIN_LP_TRANSMISSIVE = 1;
        public const  int DTWAIN_LS_RED = 0;
        public const  int DTWAIN_LS_GREEN = 1;
        public const  int DTWAIN_LS_BLUE = 2;
        public const  int DTWAIN_LS_NONE = 3;
        public const  int DTWAIN_LS_WHITE = 4;
        public const  int DTWAIN_LS_UV = 5;
        public const  int DTWAIN_LS_IR = 6;
        public const  int DTWAIN_DLG_SORTNAMES = 1;
        public const  int DTWAIN_DLG_CENTER = 2;
        public const  int DTWAIN_DLG_CENTER_SCREEN = 4;
        public const  int DTWAIN_DLG_USETEMPLATE = 8;
        public const  int DTWAIN_DLG_CLEAR_PARAMS = 16;
        public const  int DTWAIN_DLG_HORIZONTALSCROLL = 32;
        public const  int DTWAIN_RES_ENGLISH = 0;
        public const  int DTWAIN_RES_FRENCH = 1;
        public const  int DTWAIN_RES_SPANISH = 2;
        public const  int DTWAIN_RES_DUTCH = 3;
        public const  int DTWAIN_RES_GERMAN = 4;
        public const  int DTWAIN_RES_ITALIAN = 5;
        public const  int DTWAIN_AL_ALARM = 0;
        public const  int DTWAIN_AL_FEEDERERROR = 1;
        public const  int DTWAIN_AL_FEEDERWARNING = 2;
        public const  int DTWAIN_AL_BARCODE = 3;
        public const  int DTWAIN_AL_DOUBLEFEED = 4;
        public const  int DTWAIN_AL_JAM = 5;
        public const  int DTWAIN_AL_PATCHCODE = 6;
        public const  int DTWAIN_AL_POWER = 7;
        public const  int DTWAIN_AL_SKEW = 8;
        public const  int DTWAIN_FT_CAMERA = 0;
        public const  int DTWAIN_FT_CAMERATOP = 1;
        public const  int DTWAIN_FT_CAMERABOTTOM = 2;
        public const  int DTWAIN_FT_CAMERAPREVIEW = 3;
        public const  int DTWAIN_FT_DOMAIN = 4;
        public const  int DTWAIN_FT_HOST = 5;
        public const  int DTWAIN_FT_DIRECTORY = 6;
        public const  int DTWAIN_FT_IMAGE = 7;
        public const  int DTWAIN_FT_UNKNOWN = 8;
        public const  int DTWAIN_NF_NONE = 0;
        public const  int DTWAIN_NF_AUTO = 1;
        public const  int DTWAIN_NF_LONEPIXEL = 2;
        public const  int DTWAIN_NF_MAJORITYRULE = 3;
        public const  int DTWAIN_CB_AUTO = 0;
        public const  int DTWAIN_CB_CLEAR = 1;
        public const  int DTWAIN_CB_NOCLEAR = 2;
        public const  int DTWAIN_FA_NONE = 0;
        public const  int DTWAIN_FA_LEFT = 1;
        public const  int DTWAIN_FA_CENTER = 2;
        public const  int DTWAIN_FA_RIGHT = 3;
        public const  int DTWAIN_PF_CHOCOLATE = 0;
        public const  int DTWAIN_PF_VANILLA = 1;
        public const  int DTWAIN_FO_FIRSTPAGEFIRST = 0;
        public const  int DTWAIN_FO_LASTPAGEFIRST = 1;
        public const  int DTWAIN_INCREMENT_STATIC = 0;
        public const  int DTWAIN_INCREMENT_DYNAMIC = 1;
        public const  int DTWAIN_INCREMENT_DEFAULT = -1;
        public const  int DTWAIN_MANDUP_FACEUPTOPPAGE = 0;
        public const  int DTWAIN_MANDUP_FACEUPBOTTOMPAGE = 1;
        public const  int DTWAIN_MANDUP_FACEDOWNTOPPAGE = 2;
        public const  int DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE = 3;
        public const  int DTWAIN_FILESAVE_DEFAULT = 0;
        public const  int DTWAIN_FILESAVE_UICLOSE = 1;
        public const  int DTWAIN_FILESAVE_SOURCECLOSE = 2;
        public const  int DTWAIN_FILESAVE_ENDACQUIRE = 3;
        public const  int DTWAIN_FILESAVE_MANUALSAVE = 4;
        public const  int DTWAIN_FILESAVE_SAVEINCOMPLETE = 128;
        public const  int DTWAIN_MANDUP_SCANOK = 1;
        public const  int DTWAIN_MANDUP_SIDE1RESCAN = 2;
        public const  int DTWAIN_MANDUP_SIDE2RESCAN = 3;
        public const  int DTWAIN_MANDUP_RESCANALL = 4;
        public const  int DTWAIN_MANDUP_PAGEMISSING = 5;
        public const  int DTWAIN_DEMODLL_VERSION = 0x00000001;
        public const  int DTWAIN_UNLICENSED_VERSION = 0x00000002;
        public const  int DTWAIN_COMPANY_VERSION = 0x00000004;
        public const  int DTWAIN_GENERAL_VERSION = 0x00000008;
        public const  int DTWAIN_DEVELOP_VERSION = 0x00000010;
        public const  int DTWAIN_JAVA_VERSION = 0x00000020;
        public const  int DTWAIN_TOOLKIT_VERSION = 0x00000040;
        public const  int DTWAIN_LIMITEDDLL_VERSION = 0x00000080;
        public const  int DTWAIN_STATICLIB_VERSION = 0x00000100;
        public const  int DTWAIN_STATICLIB_STDCALL_VERSION = 0x00000200;
        public const  int DTWAIN_PDF_VERSION = 0x00010000;
        public const  int DTWAIN_TWAINSAVE_VERSION = 0x00020000;
        public const  int DTWAIN_OCR_VERSION = 0x00040000;
        public const  int DTWAIN_BARCODE_VERSION = 0x00080000;
        public const  int DTWAIN_ACTIVEX_VERSION = 0x00100000;
        public const  int DTWAIN_32BIT_VERSION = 0x00200000;
        public const  int DTWAIN_64BIT_VERSION = 0x00400000;
        public const  int DTWAIN_UNICODE_VERSION = 0x00800000;
        public const  int DTWAINOCR_RETURNHANDLE = 1;
        public const  int DTWAINOCR_COPYDATA = 2;
        public const  int DTWAIN_OCRINFO_CHAR = 0;
        public const  int DTWAIN_OCRINFO_CHARXPOS = 1;
        public const  int DTWAIN_OCRINFO_CHARYPOS = 2;
        public const  int DTWAIN_OCRINFO_CHARXWIDTH = 3;
        public const  int DTWAIN_OCRINFO_CHARYWIDTH = 4;
        public const  int DTWAIN_OCRINFO_CHARCONFIDENCE = 5;
        public const  int DTWAIN_OCRINFO_PAGENUM = 6;
        public const  int DTWAIN_OCRINFO_OCRENGINE = 7;
        public const  int DTWAIN_OCRINFO_TEXTLENGTH = 8;
        public const  int DTWAIN_PDFPAGETYPE_COLOR = 0;
        public const  int DTWAIN_PDFPAGETYPE_BW = 1;
        public const  int DTWAIN_TWAINDSM_LEGACY = 1;
        public const  int DTWAIN_TWAINDSM_VERSION2 = 2;
        public const  int DTWAIN_TWAINDSM_LATESTVERSION = 4;
        public const  int DTWAIN_TWAINDSMSEARCH_NOTFOUND = (-1);
        public const  int DTWAIN_TWAINDSMSEARCH_WSO = 0;
        public const  int DTWAIN_TWAINDSMSEARCH_WOS = 1;
        public const  int DTWAIN_TWAINDSMSEARCH_SWO = 2;
        public const  int DTWAIN_TWAINDSMSEARCH_SOW = 3;
        public const  int DTWAIN_TWAINDSMSEARCH_OWS = 4;
        public const  int DTWAIN_TWAINDSMSEARCH_OSW = 5;
        public const  int DTWAIN_TWAINDSMSEARCH_W = 6;
        public const  int DTWAIN_TWAINDSMSEARCH_S = 7;
        public const  int DTWAIN_TWAINDSMSEARCH_O = 8;
        public const  int DTWAIN_TWAINDSMSEARCH_WS = 9;
        public const  int DTWAIN_TWAINDSMSEARCH_WO = 10;
        public const  int DTWAIN_TWAINDSMSEARCH_SW = 11;
        public const  int DTWAIN_TWAINDSMSEARCH_SO = 12;
        public const  int DTWAIN_TWAINDSMSEARCH_OW = 13;
        public const  int DTWAIN_TWAINDSMSEARCH_OS = 14;
        public const  int DTWAIN_PDFPOLARITY_POSITIVE = 1;
        public const  int DTWAIN_PDFPOLARITY_NEGATIVE = 2;
        public const  int DTWAIN_TWPF_NORMAL = 0;
        public const  int DTWAIN_TWPF_BOLD = 1;
        public const  int DTWAIN_TWPF_ITALIC = 2;
        public const  int DTWAIN_TWPF_LARGESIZE = 3;
        public const  int DTWAIN_TWPF_SMALLSIZE = 4;
        public const  int DTWAIN_TWCT_PAGE = 0;
        public const  int DTWAIN_TWCT_PATCH1 = 1;
        public const  int DTWAIN_TWCT_PATCH2 = 2;
        public const  int DTWAIN_TWCT_PATCH3 = 3;
        public const  int DTWAIN_TWCT_PATCH4 = 4;
        public const  int DTWAIN_TWCT_PATCHT = 5;
        public const  int DTWAIN_TWCT_PATCH6 = 6;
        public const  int DTWAIN_CV_CAPCUSTOMBASE = 0x8000;
        public const  int DTWAIN_CV_CAPXFERCOUNT = 0x0001;
        public const  int DTWAIN_CV_ICAPCOMPRESSION = 0x0100;
        public const  int DTWAIN_CV_ICAPPIXELTYPE = 0x0101;
        public const  int DTWAIN_CV_ICAPUNITS = 0x0102;
        public const  int DTWAIN_CV_ICAPXFERMECH = 0x0103;
        public const  int DTWAIN_CV_CAPAUTHOR = 0x1000;
        public const  int DTWAIN_CV_CAPCAPTION = 0x1001;
        public const  int DTWAIN_CV_CAPFEEDERENABLED = 0x1002;
        public const  int DTWAIN_CV_CAPFEEDERLOADED = 0x1003;
        public const  int DTWAIN_CV_CAPTIMEDATE = 0x1004;
        public const  int DTWAIN_CV_CAPSUPPORTEDCAPS = 0x1005;
        public const  int DTWAIN_CV_CAPEXTENDEDCAPS = 0x1006;
        public const  int DTWAIN_CV_CAPAUTOFEED = 0x1007;
        public const  int DTWAIN_CV_CAPCLEARPAGE = 0x1008;
        public const  int DTWAIN_CV_CAPFEEDPAGE = 0x1009;
        public const  int DTWAIN_CV_CAPREWINDPAGE = 0x100a;
        public const  int DTWAIN_CV_CAPINDICATORS = 0x100b;
        public const  int DTWAIN_CV_CAPSUPPORTEDCAPSEXT = 0x100c;
        public const  int DTWAIN_CV_CAPPAPERDETECTABLE = 0x100d;
        public const  int DTWAIN_CV_CAPUICONTROLLABLE = 0x100e;
        public const  int DTWAIN_CV_CAPDEVICEONLINE = 0x100f;
        public const  int DTWAIN_CV_CAPAUTOSCAN = 0x1010;
        public const  int DTWAIN_CV_CAPTHUMBNAILSENABLED = 0x1011;
        public const  int DTWAIN_CV_CAPDUPLEX = 0x1012;
        public const  int DTWAIN_CV_CAPDUPLEXENABLED = 0x1013;
        public const  int DTWAIN_CV_CAPENABLEDSUIONLY = 0x1014;
        public const  int DTWAIN_CV_CAPCUSTOMDSDATA = 0x1015;
        public const  int DTWAIN_CV_CAPENDORSER = 0x1016;
        public const  int DTWAIN_CV_CAPJOBCONTROL = 0x1017;
        public const  int DTWAIN_CV_CAPALARMS = 0x1018;
        public const  int DTWAIN_CV_CAPALARMVOLUME = 0x1019;
        public const  int DTWAIN_CV_CAPAUTOMATICCAPTURE = 0x101a;
        public const  int DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE = 0x101b;
        public const  int DTWAIN_CV_CAPTIMEBETWEENCAPTURES = 0x101c;
        public const  int DTWAIN_CV_CAPCLEARBUFFERS = 0x101d;
        public const  int DTWAIN_CV_CAPMAXBATCHBUFFERS = 0x101e;
        public const  int DTWAIN_CV_CAPDEVICETIMEDATE = 0x101f;
        public const  int DTWAIN_CV_CAPPOWERSUPPLY = 0x1020;
        public const  int DTWAIN_CV_CAPCAMERAPREVIEWUI = 0x1021;
        public const  int DTWAIN_CV_CAPDEVICEEVENT = 0x1022;
        public const  int DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE = 0x1023;
        public const  int DTWAIN_CV_CAPSERIALNUMBER = 0x1024;
        public const  int DTWAIN_CV_CAPFILESYSTEM = 0x1025;
        public const  int DTWAIN_CV_CAPPRINTER = 0x1026;
        public const  int DTWAIN_CV_CAPPRINTERENABLED = 0x1027;
        public const  int DTWAIN_CV_CAPPRINTERINDEX = 0x1028;
        public const  int DTWAIN_CV_CAPPRINTERMODE = 0x1029;
        public const  int DTWAIN_CV_CAPPRINTERSTRING = 0x102a;
        public const  int DTWAIN_CV_CAPPRINTERSUFFIX = 0x102b;
        public const  int DTWAIN_CV_CAPLANGUAGE = 0x102c;
        public const  int DTWAIN_CV_CAPFEEDERALIGNMENT = 0x102d;
        public const  int DTWAIN_CV_CAPFEEDERORDER = 0x102e;
        public const  int DTWAIN_CV_CAPPAPERBINDING = 0x102f;
        public const  int DTWAIN_CV_CAPREACQUIREALLOWED = 0x1030;
        public const  int DTWAIN_CV_CAPPASSTHRU = 0x1031;
        public const  int DTWAIN_CV_CAPBATTERYMINUTES = 0x1032;
        public const  int DTWAIN_CV_CAPBATTERYPERCENTAGE = 0x1033;
        public const  int DTWAIN_CV_CAPPOWERDOWNTIME = 0x1034;
        public const  int DTWAIN_CV_CAPSEGMENTED = 0x1035;
        public const  int DTWAIN_CV_CAPCAMERAENABLED = 0x1036;
        public const  int DTWAIN_CV_CAPCAMERAORDER = 0x1037;
        public const  int DTWAIN_CV_CAPMICRENABLED = 0x1038;
        public const  int DTWAIN_CV_CAPFEEDERPREP = 0x1039;
        public const  int DTWAIN_CV_CAPFEEDERPOCKET = 0x103a;
        public const  int DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM = 0x103b;
        public const  int DTWAIN_CV_CAPCUSTOMINTERFACEGUID = 0x103c;
        public const  int DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE = 0x103d;
        public const  int DTWAIN_CV_CAPSUPPORTEDDATS = 0x103e;
        public const  int DTWAIN_CV_CAPDOUBLEFEEDDETECTION = 0x103f;
        public const  int DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH = 0x1040;
        public const  int DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY = 0x1041;
        public const  int DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE = 0x1042;
        public const  int DTWAIN_CV_CAPPAPERHANDLING = 0x1043;
        public const  int DTWAIN_CV_CAPINDICATORSMODE = 0x1044;
        public const  int DTWAIN_CV_CAPPRINTERVERTICALOFFSET = 0x1045;
        public const  int DTWAIN_CV_CAPPOWERSAVETIME = 0x1046;
        public const  int DTWAIN_CV_CAPPRINTERCHARROTATION = 0x1047;
        public const  int DTWAIN_CV_CAPPRINTERFONTSTYLE = 0x1048;
        public const  int DTWAIN_CV_CAPPRINTERINDEXLEADCHAR = 0x1049;
        public const  int DTWAIN_CV_CAPPRINTERINDEXMAXVALUE = 0x104A;
        public const  int DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS = 0x104B;
        public const  int DTWAIN_CV_CAPPRINTERINDEXSTEP = 0x104C;
        public const  int DTWAIN_CV_CAPPRINTERINDEXTRIGGER = 0x104D;
        public const  int DTWAIN_CV_CAPPRINTERSTRINGPREVIEW = 0x104E;
        public const  int DTWAIN_CV_ICAPAUTOBRIGHT = 0x1100;
        public const  int DTWAIN_CV_ICAPBRIGHTNESS = 0x1101;
        public const  int DTWAIN_CV_ICAPCONTRAST = 0x1103;
        public const  int DTWAIN_CV_ICAPCUSTHALFTONE = 0x1104;
        public const  int DTWAIN_CV_ICAPEXPOSURETIME = 0x1105;
        public const  int DTWAIN_CV_ICAPFILTER = 0x1106;
        public const  int DTWAIN_CV_ICAPFLASHUSED = 0x1107;
        public const  int DTWAIN_CV_ICAPGAMMA = 0x1108;
        public const  int DTWAIN_CV_ICAPHALFTONES = 0x1109;
        public const  int DTWAIN_CV_ICAPHIGHLIGHT = 0x110a;
        public const  int DTWAIN_CV_ICAPIMAGEFILEFORMAT = 0x110c;
        public const  int DTWAIN_CV_ICAPLAMPSTATE = 0x110d;
        public const  int DTWAIN_CV_ICAPLIGHTSOURCE = 0x110e;
        public const  int DTWAIN_CV_ICAPORIENTATION = 0x1110;
        public const  int DTWAIN_CV_ICAPPHYSICALWIDTH = 0x1111;
        public const  int DTWAIN_CV_ICAPPHYSICALHEIGHT = 0x1112;
        public const  int DTWAIN_CV_ICAPSHADOW = 0x1113;
        public const  int DTWAIN_CV_ICAPFRAMES = 0x1114;
        public const  int DTWAIN_CV_ICAPXNATIVERESOLUTION = 0x1116;
        public const  int DTWAIN_CV_ICAPYNATIVERESOLUTION = 0x1117;
        public const  int DTWAIN_CV_ICAPXRESOLUTION = 0x1118;
        public const  int DTWAIN_CV_ICAPYRESOLUTION = 0x1119;
        public const  int DTWAIN_CV_ICAPMAXFRAMES = 0x111a;
        public const  int DTWAIN_CV_ICAPTILES = 0x111b;
        public const  int DTWAIN_CV_ICAPBITORDER = 0x111c;
        public const  int DTWAIN_CV_ICAPCCITTKFACTOR = 0x111d;
        public const  int DTWAIN_CV_ICAPLIGHTPATH = 0x111e;
        public const  int DTWAIN_CV_ICAPPIXELFLAVOR = 0x111f;
        public const  int DTWAIN_CV_ICAPPLANARCHUNKY = 0x1120;
        public const  int DTWAIN_CV_ICAPROTATION = 0x1121;
        public const  int DTWAIN_CV_ICAPSUPPORTEDSIZES = 0x1122;
        public const  int DTWAIN_CV_ICAPTHRESHOLD = 0x1123;
        public const  int DTWAIN_CV_ICAPXSCALING = 0x1124;
        public const  int DTWAIN_CV_ICAPYSCALING = 0x1125;
        public const  int DTWAIN_CV_ICAPBITORDERCODES = 0x1126;
        public const  int DTWAIN_CV_ICAPPIXELFLAVORCODES = 0x1127;
        public const  int DTWAIN_CV_ICAPJPEGPIXELTYPE = 0x1128;
        public const  int DTWAIN_CV_ICAPTIMEFILL = 0x112a;
        public const  int DTWAIN_CV_ICAPBITDEPTH = 0x112b;
        public const  int DTWAIN_CV_ICAPBITDEPTHREDUCTION = 0x112c;
        public const  int DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE = 0x112d;
        public const  int DTWAIN_CV_ICAPIMAGEDATASET = 0x112e;
        public const  int DTWAIN_CV_ICAPEXTIMAGEINFO = 0x112f;
        public const  int DTWAIN_CV_ICAPMINIMUMHEIGHT = 0x1130;
        public const  int DTWAIN_CV_ICAPMINIMUMWIDTH = 0x1131;
        public const  int DTWAIN_CV_ICAPAUTOBORDERDETECTION = 0x1132;
        public const  int DTWAIN_CV_ICAPAUTODESKEW = 0x1133;
        public const  int DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES = 0x1134;
        public const  int DTWAIN_CV_ICAPAUTOROTATE = 0x1135;
        public const  int DTWAIN_CV_ICAPFLIPROTATION = 0x1136;
        public const  int DTWAIN_CV_ICAPBARCODEDETECTIONENABLED = 0x1137;
        public const  int DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES = 0x1138;
        public const  int DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES = 0x1139;
        public const  int DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES = 0x113a;
        public const  int DTWAIN_CV_ICAPBARCODESEARCHMODE = 0x113b;
        public const  int DTWAIN_CV_ICAPBARCODEMAXRETRIES = 0x113c;
        public const  int DTWAIN_CV_ICAPBARCODETIMEOUT = 0x113d;
        public const  int DTWAIN_CV_ICAPZOOMFACTOR = 0x113e;
        public const  int DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED = 0x113f;
        public const  int DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES = 0x1140;
        public const  int DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES = 0x1141;
        public const  int DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES = 0x1142;
        public const  int DTWAIN_CV_ICAPPATCHCODESEARCHMODE = 0x1143;
        public const  int DTWAIN_CV_ICAPPATCHCODEMAXRETRIES = 0x1144;
        public const  int DTWAIN_CV_ICAPPATCHCODETIMEOUT = 0x1145;
        public const  int DTWAIN_CV_ICAPFLASHUSED2 = 0x1146;
        public const  int DTWAIN_CV_ICAPIMAGEFILTER = 0x1147;
        public const  int DTWAIN_CV_ICAPNOISEFILTER = 0x1148;
        public const  int DTWAIN_CV_ICAPOVERSCAN = 0x1149;
        public const  int DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION = 0x1150;
        public const  int DTWAIN_CV_ICAPAUTOMATICDESKEW = 0x1151;
        public const  int DTWAIN_CV_ICAPAUTOMATICROTATE = 0x1152;
        public const  int DTWAIN_CV_ICAPJPEGQUALITY = 0x1153;
        public const  int DTWAIN_CV_ICAPFEEDERTYPE = 0x1154;
        public const  int DTWAIN_CV_ICAPICCPROFILE = 0x1155;
        public const  int DTWAIN_CV_ICAPAUTOSIZE = 0x1156;
        public const  int DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME = 0x1157;
        public const  int DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION = 0x1158;
        public const  int DTWAIN_CV_ICAPAUTOMATICCOLORENABLED = 0x1159;
        public const  int DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE = 0x115a;
        public const  int DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED = 0x115b;
        public const  int DTWAIN_CV_ICAPIMAGEMERGE = 0x115c;
        public const  int DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD = 0x115d;
        public const  int DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO = 0x115e;
        public const  int DTWAIN_CV_ICAPFILMTYPE = 0x115f;
        public const  int DTWAIN_CV_ICAPMIRROR = 0x1160;
        public const  int DTWAIN_CV_ICAPJPEGSUBSAMPLING = 0x1161;
        public const  int DTWAIN_CV_ACAPAUDIOFILEFORMAT = 0x1201;
        public const  int DTWAIN_CV_ACAPXFERMECH = 0x1202;
        public const  int DTWAIN_CFMCV_CAPCFMSTART = 2048;
        public const  int DTWAIN_CFMCV_CAPDUPLEXSCANNER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+10);
        public const  int DTWAIN_CFMCV_CAPDUPLEXENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+11);
        public const  int DTWAIN_CFMCV_CAPSCANNERNAME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+12);
        public const  int DTWAIN_CFMCV_CAPSINGLEPASS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+13);
        public const  int DTWAIN_CFMCV_CAPERRHANDLING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+20);
        public const  int DTWAIN_CFMCV_CAPFEEDERSTATUS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+21);
        public const  int DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+22);
        public const  int DTWAIN_CFMCV_CAPFEEDWAITTIME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+23);
        public const  int DTWAIN_CFMCV_ICAPWHITEBALANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+24);
        public const  int DTWAIN_CFMCV_ICAPAUTOBINARY = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+25);
        public const  int DTWAIN_CFMCV_ICAPIMAGESEPARATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+26);
        public const  int DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+27);
        public const  int DTWAIN_CFMCV_ICAPIMAGEEMPHASIS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+28);
        public const  int DTWAIN_CFMCV_ICAPOUTLINING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+29);
        public const  int DTWAIN_CFMCV_ICAPDYNTHRESHOLD = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+30);
        public const  int DTWAIN_CFMCV_ICAPVARIANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+31);
        public const  int DTWAIN_CFMCV_CAPENDORSERAVAILABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+32);
        public const  int DTWAIN_CFMCV_CAPENDORSERENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+33);
        public const  int DTWAIN_CFMCV_CAPENDORSERCHARSET = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+34);
        public const  int DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+35);
        public const  int DTWAIN_CFMCV_CAPENDORSERSTRING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+36);
        public const  int DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+48);
        public const  int DTWAIN_CFMCV_ICAPSMOOTHINGMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+49);
        public const  int DTWAIN_CFMCV_ICAPFILTERMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+50);
        public const  int DTWAIN_CFMCV_ICAPGRADATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+51);
        public const  int DTWAIN_CFMCV_ICAPMIRROR = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+52);
        public const  int DTWAIN_CFMCV_ICAPEASYSCANMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+53);
        public const  int DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+54);
        public const  int DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+55);
        public const  int DTWAIN_CFMCV_CAPDUPLEXPAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+56);
        public const  int DTWAIN_CFMCV_ICAPINVERTIMAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+57);
        public const  int DTWAIN_CFMCV_ICAPSPECKLEREMOVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+58);
        public const  int DTWAIN_CFMCV_ICAPUSMFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+59);
        public const  int DTWAIN_CFMCV_ICAPNOISEFILTERCFM = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+60);
        public const  int DTWAIN_CFMCV_ICAPDESCREENING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+61);
        public const  int DTWAIN_CFMCV_ICAPQUALITYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+62);
        public const  int DTWAIN_CFMCV_ICAPBINARYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+63);
        public const  int DTWAIN_OCRCV_IMAGEFILEFORMAT = 0x1000;
        public const  int DTWAIN_OCRCV_DESKEW = 0x1001;
        public const  int DTWAIN_OCRCV_DESHADE = 0x1002;
        public const  int DTWAIN_OCRCV_ORIENTATION = 0x1003;
        public const  int DTWAIN_OCRCV_NOISEREMOVE = 0x1004;
        public const  int DTWAIN_OCRCV_LINEREMOVE = 0x1005;
        public const  int DTWAIN_OCRCV_INVERTPAGE = 0x1006;
        public const  int DTWAIN_OCRCV_INVERTZONES = 0x1007;
        public const  int DTWAIN_OCRCV_LINEREJECT = 0x1008;
        public const  int DTWAIN_OCRCV_CHARACTERREJECT = 0x1009;
        public const  int DTWAIN_OCRCV_ERRORREPORTMODE = 0x1010;
        public const  int DTWAIN_OCRCV_ERRORREPORTFILE = 0x1011;
        public const  int DTWAIN_OCRCV_PIXELTYPE = 0x1012;
        public const  int DTWAIN_OCRCV_BITDEPTH = 0x1013;
        public const  int DTWAIN_OCRCV_RETURNCHARINFO = 0x1014;
        public const  int DTWAIN_OCRCV_NATIVEFILEFORMAT = 0x1015;
        public const  int DTWAIN_OCRCV_MPNATIVEFILEFORMAT = 0x1016;
        public const  int DTWAIN_OCRCV_SUPPORTEDCAPS = 0x1017;
        public const  int DTWAIN_OCRCV_DISABLECHARACTERS = 0x1018;
        public const  int DTWAIN_OCRCV_REMOVECONTROLCHARS = 0x1019;
        public const  int DTWAIN_OCRORIENT_OFF = 0;
        public const  int DTWAIN_OCRORIENT_AUTO = 1;
        public const  int DTWAIN_OCRORIENT_90 = 2;
        public const  int DTWAIN_OCRORIENT_180 = 3;
        public const  int DTWAIN_OCRORIENT_270 = 4;
        public const  int DTWAIN_OCRIMAGEFORMAT_AUTO = 10000;
        public const  int DTWAIN_OCRERROR_MODENONE = 0;
        public const  int DTWAIN_OCRERROR_SHOWMSGBOX = 1;
        public const  int DTWAIN_OCRERROR_WRITEFILE = 2;
        public const  int DTWAIN_PDFTEXT_ALLPAGES = 0x00000001;
        public const  int DTWAIN_PDFTEXT_EVENPAGES = 0x00000002;
        public const  int DTWAIN_PDFTEXT_ODDPAGES = 0x00000004;
        public const  int DTWAIN_PDFTEXT_FIRSTPAGE = 0x00000008;
        public const  int DTWAIN_PDFTEXT_LASTPAGE = 0x00000010;
        public const  int DTWAIN_PDFTEXT_CURRENTPAGE = 0x00000020;
        public const  int DTWAIN_PDFTEXT_DISABLED = 0x00000040;
        public const  int DTWAIN_PDFTEXT_TOPLEFT = 0x00000100;
        public const  int DTWAIN_PDFTEXT_TOPRIGHT = 0x00000200;
        public const  int DTWAIN_PDFTEXT_HORIZCENTER = 0x00000400;
        public const  int DTWAIN_PDFTEXT_VERTCENTER = 0x00000800;
        public const  int DTWAIN_PDFTEXT_BOTTOMLEFT = 0x00001000;
        public const  int DTWAIN_PDFTEXT_BOTTOMRIGHT = 0x00002000;
        public const  int DTWAIN_PDFTEXT_BOTTOMCENTER = 0x00004000;
        public const  int DTWAIN_PDFTEXT_TOPCENTER = 0x00008000;
        public const  int DTWAIN_PDFTEXT_XCENTER = 0x00010000;
        public const  int DTWAIN_PDFTEXT_YCENTER = 0x00020000;
        public const  int DTWAIN_PDFTEXT_NOSCALING = 0x00100000;
        public const  int DTWAIN_PDFTEXT_NOCHARSPACING = 0x00200000;
        public const  int DTWAIN_PDFTEXT_NOWORDSPACING = 0x00400000;
        public const  int DTWAIN_PDFTEXT_NOSTROKEWIDTH = 0x00800000;
        public const  int DTWAIN_PDFTEXT_NORENDERMODE = 0x01000000;
        public const  int DTWAIN_PDFTEXT_NORGBCOLOR = 0x02000000;
        public const  int DTWAIN_PDFTEXT_NOFONTSIZE = 0x04000000;
        public const  int DTWAIN_PDFTEXT_NOABSPOSITION = 0x08000000;
        public const  uint DTWAIN_PDFTEXT_IGNOREALL = 0xFFF00000;
        public const  int DTWAIN_FONT_COURIER = 0;
        public const  int DTWAIN_FONT_COURIERBOLD = 1;
        public const  int DTWAIN_FONT_COURIERBOLDOBLIQUE = 2;
        public const  int DTWAIN_FONT_COURIEROBLIQUE = 3;
        public const  int DTWAIN_FONT_HELVETICA = 4;
        public const  int DTWAIN_FONT_HELVETICABOLD = 5;
        public const  int DTWAIN_FONT_HELVETICABOLDOBLIQUE = 6;
        public const  int DTWAIN_FONT_HELVETICAOBLIQUE = 7;
        public const  int DTWAIN_FONT_TIMESBOLD = 8;
        public const  int DTWAIN_FONT_TIMESBOLDITALIC = 9;
        public const  int DTWAIN_FONT_TIMESROMAN = 10;
        public const  int DTWAIN_FONT_TIMESITALIC = 11;
        public const  int DTWAIN_FONT_SYMBOL = 12;
        public const  int DTWAIN_FONT_ZAPFDINGBATS = 13;
        public const  int DTWAIN_PDFRENDER_FILL = 0;
        public const  int DTWAIN_PDFRENDER_STROKE = 1;
        public const  int DTWAIN_PDFRENDER_FILLSTROKE = 2;
        public const  int DTWAIN_PDFRENDER_INVISIBLE = 3;
        public const  int DTWAIN_PDFTEXTELEMENT_SCALINGXY = 0;
        public const  int DTWAIN_PDFTEXTELEMENT_FONTHEIGHT = 1;
        public const  int DTWAIN_PDFTEXTELEMENT_WORDSPACING = 2;
        public const  int DTWAIN_PDFTEXTELEMENT_POSITION = 3;
        public const  int DTWAIN_PDFTEXTELEMENT_COLOR = 4;
        public const  int DTWAIN_PDFTEXTELEMENT_STROKEWIDTH = 5;
        public const  int DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS = 6;
        public const  int DTWAIN_PDFTEXTELEMENT_FONTNAME = 7;
        public const  int DTWAIN_PDFTEXTELEMENT_TEXT = 8;
        public const  int DTWAIN_PDFTEXTELEMENT_RENDERMODE = 9;
        public const  int DTWAIN_PDFTEXTELEMENT_CHARSPACING = 10;
        public const  int DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE = 11;
        public const  int DTWAIN_PDFTEXTELEMENT_LEADING = 12;
        public const  int DTWAIN_PDFTEXTELEMENT_SCALING = 13;
        public const  int DTWAIN_PDFTEXTELEMENT_TEXTLENGTH = 14;
        public const  int DTWAIN_PDFTEXTELEMENT_SKEWANGLES = 15;
        public const  int DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER = 16;
        public const  int DTWAIN_PDFTEXTTRANSFORM_TSRK = 0;
        public const  int DTWAIN_PDFTEXTTRANSFORM_TSKR = 1;
        public const  int DTWAIN_PDFTEXTTRANSFORM_TKSR = 2;
        public const  int DTWAIN_PDFTEXTTRANSFORM_TKRS = 3;
        public const  int DTWAIN_PDFTEXTTRANSFORM_TRSK = 4;
        public const  int DTWAIN_PDFTEXTTRANSFORM_TRKS = 5;
        public const  int DTWAIN_PDFTEXTTRANSFORM_STRK = 6;
        public const  int DTWAIN_PDFTEXTTRANSFORM_STKR = 7;
        public const  int DTWAIN_PDFTEXTTRANSFORM_SKTR = 8;
        public const  int DTWAIN_PDFTEXTTRANSFORM_SKRT = 9;
        public const  int DTWAIN_PDFTEXTTRANSFORM_SRTK = 10;
        public const  int DTWAIN_PDFTEXTTRANSFORM_SRKT = 11;
        public const  int DTWAIN_PDFTEXTTRANSFORM_RSTK = 12;
        public const  int DTWAIN_PDFTEXTTRANSFORM_RSKT = 13;
        public const  int DTWAIN_PDFTEXTTRANSFORM_RTSK = 14;
        public const  int DTWAIN_PDFTEXTTRANSFORM_RTKT = 15;
        public const  int DTWAIN_PDFTEXTTRANSFORM_RKST = 16;
        public const  int DTWAIN_PDFTEXTTRANSFORM_RKTS = 17;
        public const  int DTWAIN_PDFTEXTTRANSFORM_KSTR = 18;
        public const  int DTWAIN_PDFTEXTTRANSFORM_KSRT = 19;
        public const  int DTWAIN_PDFTEXTTRANSFORM_KRST = 20;
        public const  int DTWAIN_PDFTEXTTRANSFORM_KRTS = 21;
        public const  int DTWAIN_PDFTEXTTRANSFORM_KTSR = 22;
        public const  int DTWAIN_PDFTEXTTRANSFORM_KTRS = 23;
        public const  int DTWAIN_PDFTEXTTRANFORM_LAST = DTWAIN_PDFTEXTTRANSFORM_KTRS;
		public const string DTWAIN_LIBRARY = "DTWAIN64UD.DLL";
		
        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_AcquireBuffered(long Source,int PixelType,int nMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireBufferedEx(long Source,int PixelType,int nMaxPages,int bShowUI,int bCloseSource,long Acquisitions,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireFile(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpszFile,int lFileType,int lFileFlags,int PixelType,int lMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireFileA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpszFile,int lFileType,int lFileFlags,int PixelType,int lMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireFileEx(long Source,long aFileNames,int lFileType,int lFileFlags,int PixelType,int lMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireFileW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpszFile,int lFileType,int lFileFlags,int PixelType,int lMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_AcquireNative(long Source,int PixelType,int nMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireNativeEx(long Source,int PixelType,int nMaxPages,int bShowUI,int bCloseSource,long Acquisitions,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireAudioNative(long Source,int nMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireAudioFile(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpszFile,int lFileFlags,int lMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireAudioFileA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpszFile, int lFileFlags,int lMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireAudioFileW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpszFile,int lFileFlags,int lMaxPages,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AcquireAudioNativeEx(long Source,int nMaxPages,int bShowUI,int bCloseSource,int Acquisitions,ref int pStatus);


        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_AcquireToClipboard(long Source,int PixelType,int nMaxPages,int nTransferMode,int bDiscardDibs,int bShowUI,int bCloseSource,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AddPDFText(long Source,[MarshalAs(UnmanagedType.LPTStr)] string szText,int xPos,int yPos,[MarshalAs(UnmanagedType.LPTStr)] string fontName,double fontSize,int colorRGB,int renderMode,double scaling,double charSpacing,double wordSpacing,int strokeWidth,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AddPDFTextA(long Source,[MarshalAs(UnmanagedType.LPStr)] string szText,int xPos,int yPos,[MarshalAs(UnmanagedType.LPStr)] string fontName,double fontSize,int colorRGB,int renderMode,double scaling,double charSpacing,double wordSpacing,int strokeWidth,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AddPDFTextW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string szText,int xPos,int yPos,[MarshalAs(UnmanagedType.LPWStr)] string fontName,double fontSize,int colorRGB,int renderMode,double scaling,double charSpacing,double wordSpacing,int strokeWidth,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_AppHandlesExceptions(int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddANSIString(long pArray,[MarshalAs(UnmanagedType.LPStr)] string Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddANSIStringN(long pArray,[MarshalAs(UnmanagedType.LPStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddFloat(long pArray,double Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddFloatN(long pArray,double Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddLong(long pArray,int Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddLongN(long pArray,int Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddString(long pArray,[MarshalAs(UnmanagedType.LPTStr)] string Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddStringA(long pArray,[MarshalAs(UnmanagedType.LPStr)] string Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddStringN(long pArray,[MarshalAs(UnmanagedType.LPTStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddStringNA(long pArray,[MarshalAs(UnmanagedType.LPStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddStringNW(long pArray,[MarshalAs(UnmanagedType.LPWStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddStringW(long pArray,[MarshalAs(UnmanagedType.LPWStr)] string Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddWideString(long pArray,[MarshalAs(UnmanagedType.LPWStr)] string Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayAddWideStringN(long pArray,[MarshalAs(UnmanagedType.LPWStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_ArrayConvertFix32ToFloat(long Fix32Array);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_ArrayConvertFloatToFix32(long FloatArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayCopy(long Source,long Dest);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_ArrayCreate(int nEnumType,int nInitialSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_ArrayCreateCopy(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_ArrayCreateFromCap(long Source,int lCapType,int lSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_ArrayCreateFromLongs(ref int pCArray,int nSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayDestroy(long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayDestroyFrames(long FrameArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayFindANSIString(long pArray,[MarshalAs(UnmanagedType.LPStr)] string pString);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayFindFloat(long pArray,double Val,double Tolerance);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayFindLong(long pArray,int Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayFindString(long pArray,[MarshalAs(UnmanagedType.LPTStr)] string pString);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayFindStringA(long pArray,[MarshalAs(UnmanagedType.LPStr)] string pString);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayFindStringW(long pArray,[MarshalAs(UnmanagedType.LPWStr)] string pString);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayFindWideString(long pArray,[MarshalAs(UnmanagedType.LPWStr)] string pString);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_ArrayFrameGetFrameAt(long FrameArray,int nWhere);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetAtANSIString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetAtFloat(long pArray,int nWhere,ref double pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetAtLong(long pArray,int nWhere,ref int pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetAtString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetAtStringA(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetAtStringW(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetAtWideString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetCount(long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetMaxStringLength(long a);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetStringLength(long a,int nWhichString);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayGetType(long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtANSIString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)] string pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtANSIStringN(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtFloat(long pArray,int nWhere,double pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtFloatN(long pArray,int nWhere,double Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtLong(long pArray,int nWhere,int pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtLongN(long pArray,int nWhere,int pVal,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPTStr)] string pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtStringA(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)] string pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtStringN(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPTStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtStringNA(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtStringNW(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtStringW(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)] string pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtWideString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)] string pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayInsertAtWideStringN(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)] string Val,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayRemoveAll(long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayRemoveAt(long pArray,int nWhere);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayRemoveAtN(long pArray,int nWhere,int num);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArrayResize(long pArray,int NewSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArraySetAtANSIString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)] string pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArraySetAtFloat(long pArray,int nWhere,double pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArraySetAtLong(long pArray,int nWhere,int pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArraySetAtString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPTStr)] string pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArraySetAtStringA(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPStr)] string pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArraySetAtStringW(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)] string pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ArraySetAtWideString(long pArray,int nWhere,[MarshalAs(UnmanagedType.LPWStr)] string pStr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_CallCallback(int wParam,int lParam,int UserData);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ClearBuffers(long Source,int ClearBuffer);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ClearErrorBuffer();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ClearPDFText(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ClearPage(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_CloseSource(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_CloseSourceUI(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_CreateAcquisitionArray();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_CreatePDFTextElement(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_DestroyAcquisitionArray(long aAcq,int bDestroyData);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_DestroyPDFTextElement(long TextElement);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_DisableAppWindow(int hWnd,int bDisable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableAutoBorderDetect(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableAutoBright(long Source,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableAutoDeskew(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableAutoFeed(long Source,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableAutoRotate(long Source,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableAutoScan(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableDuplex(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableFeeder(long Source,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableIndicator(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableJobFileHandling(long Source,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableLamp(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableMsgNotify(int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnablePatchDetect(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnablePrinter(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnableThumbnail(long Source,int bEnable);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EndThread(long DLLHandle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EndTwainSession();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumAlarms(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumBitDepths(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumBottomCameras(long Source,ref long Cameras);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumBrightnessValues(long Source,ref long pArray,int bExpandIfRange);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumCameras(long Source,ref long Cameras);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumCompressionTypes(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumContrastValues(long Source,ref long pArray,int bExpandIfRange);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumCustomCaps(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumExtImageInfoTypes(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumExtendedCaps(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumFileXferFormats(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumHighlightValues(long Source,ref long pArray,int bExpandIfRange);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumJobControls(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumLightPaths(long Source,ref long LightPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumLightSources(long Source,ref long LightSources);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumMaxBuffers(long Source,ref long pMaxBufs,int bExpandRange);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumNoiseFilters(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumOCRInterfaces(ref long OCRInterfaces);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumOCRSupportedCaps(long Engine,ref long SupportedCaps);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumOrientations(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPaperSizes(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPatchMaxPriorities(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPatchMaxRetries(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPatchPriorities(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPatchSearchModes(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPatchTimeOutValues(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPixelTypes(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumPrinterStringModes(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumResolutionValues(long Source,ref long pArray,int bExpandIfRange);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumSourceUnits(long Source,ref long lpArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumSources(ref long lpArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumSupportedCaps(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumSupportedCapsEx(long Source,int MaxCustomBase,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumThresholdValues(long Source,ref long pArray,int bExpandIfRange);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumTopCameras(long Source,ref long Cameras);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumTwainPrinters(long Source,ref int lpAvailPrinters);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_EnumTwainPrintersArray(long Source,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ExecuteOCR(long Engine,[MarshalAs(UnmanagedType.LPTStr)] string szFileName,int nStartPage,int nEndPage);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ExecuteOCRA(long Engine,[MarshalAs(UnmanagedType.LPStr)] string szFileName,int nStartPage,int nEndPage);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ExecuteOCRW(long Engine,[MarshalAs(UnmanagedType.LPWStr)] string szFileName,int nStartPage,int nEndPage);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FeedPage(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FlipBitmap(long hDib);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FlushAcquiredPages(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ForceAcquireBitDepth(long Source,int BitDepth);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_FrameCreate(double Left,double Top,double Right,double Bottom);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_FrameCreateString([MarshalAs(UnmanagedType.LPTStr)] string Left,[MarshalAs(UnmanagedType.LPTStr)] string Top,[MarshalAs(UnmanagedType.LPTStr)] string Right,[MarshalAs(UnmanagedType.LPTStr)] string Bottom);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameDestroy(long Frame);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameGetAll(long Frame,ref double Left,ref double Top,ref double Right,ref double Bottom);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameGetAllString(long Frame,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Left,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Top,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Right,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Bottom);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameGetValue(long Frame,int nWhich,ref double Value);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameGetValueString(long Frame,int nWhich,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Value);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameIsValid(long Frame);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameSetAll(long Frame,double Left,double Top,double Right,double Bottom);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameSetAllString(long Frame,[MarshalAs(UnmanagedType.LPTStr)] string Left,[MarshalAs(UnmanagedType.LPTStr)] string Top,[MarshalAs(UnmanagedType.LPTStr)] string Right,[MarshalAs(UnmanagedType.LPTStr)] string Bottom);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameSetValue(long Frame,int nWhich,double Value);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FrameSetValueString(long Frame,int nWhich,[MarshalAs(UnmanagedType.LPTStr)] string Value);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_FreeExtImageInfo(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAcquireArea(long Source,int lGetType,ref long FloatEnum);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAcquireArea2(long Source,ref double left,ref double top,ref double right,ref double bottom,ref int lpUnit);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAcquireArea2String(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder left,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder top,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder right,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder bottom,ref int Unit);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAcquireArea2StringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder left,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder top,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder right,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder bottom,ref int Unit);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAcquireArea2StringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder left,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder top,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder right,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder bottom,ref int Unit);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetAcquireStripBuffer(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAcquireStripData(long Source,ref int lpCompression,ref int lpBytesPerRow,ref int lpColumns,ref int lpRows,ref int XOffset,ref int YOffset,ref int lpBytesWritten);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAcquireStripSizes(long Source,ref int lpMin,ref int lpMax,ref int lpPreferred);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetAcquiredImage(long aAcq,int nWhichAcq,int nWhichDib);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetAcquiredImageArray(long aAcq,int nWhichAcq);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAlarmVolume(long Source,ref int lpVolume);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAppInfo([MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szVerStr,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szManu,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProdFam,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProdName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAppInfoA([MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szVerStr,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szManu,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProdFam,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProdName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAppInfoW([MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szVerStr,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szManu,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProdFam,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProdName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAuthor(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAuthorA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetAuthorW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBatteryMinutes(long Source,ref int lpMinutes);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBatteryPercent(long Source,ref int lpPercent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBitDepth(long Source,ref int BitDepth,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBlankPageAutoDetection(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBrightness(long Source,ref double Brightness);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBrightnessString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Brightness);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBrightnessStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetBrightnessStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern DTwainCallback DTWAIN_GetCallback();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern DTwainCallback64 DTWAIN_GetCallback64();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapArrayType(long Source,int nCap);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapContainer(long Source,int nCap,int lCapType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapContainerEx(int nCap,int bSetContainer,ref long ConTypes);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapDataType(long Source,int nCap);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapFromName([MarshalAs(UnmanagedType.LPTStr)] string szName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapFromNameA([MarshalAs(UnmanagedType.LPStr)] string szName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapFromNameW([MarshalAs(UnmanagedType.LPWStr)] string szName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapOperations(long Source,int lCapability,ref int lpOps);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapValues(long Source,int lCap,int lGetType,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapValuesEx(long Source,int lCap,int lGetType,int lContainerType,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCapValuesEx2(long Source,int lCap,int lGetType,int lContainerType,int nDataType,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCaption(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Caption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCaptionA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Caption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCaptionW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Caption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCompressionSize(long Source,ref int lBytes);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCompressionType(long Source,ref int lpCompression,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetContrast(long Source,ref double Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetContrastString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetContrastStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetContrastStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCountry();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetCurrentAcquiredImage(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCurrentFileName(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szName,int MaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCurrentFileNameA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szName,int MaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCurrentFileNameW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szName,int MaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCurrentPageNum(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetCurrentRetryCount(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetCustomDSData(long Source,ref int Data,int dSize,ref int pActualSize,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDSMFullName(int DSMType,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szDLLName,int nMaxLen,ref int pWhichSearch);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDSMFullNameA(int DSMType,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szDLLName,int nMaxLen,ref int pWhichSearch);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDSMFullNameW(int DSMType,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szDLLName,int nMaxLen,ref int pWhichSearch);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDSMSearchOrder();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetDTWAINHandle();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDeviceEvent(long Source,ref int lpEvent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDeviceEventEx(long Source,ref int lpEvent,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDeviceNotifications(long Source,ref int DevEvents);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDeviceTimeDate(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDeviceTimeDateA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDeviceTimeDateW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetDuplexType(long Source,ref int lpDupType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorBuffer(ref long ArrayBuffer);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorBufferThreshold();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorString(int lError,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder lpszBuffer,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorStringA(int lError,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder lpszBuffer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorStringW(int lError,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder lpszBuffer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtCapFromName([MarshalAs(UnmanagedType.LPTStr)] string szName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtCapFromNameA([MarshalAs(UnmanagedType.LPStr)] string szName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtCapFromNameW([MarshalAs(UnmanagedType.LPWStr)] string szName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtImageInfo(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtImageInfoData(long Source,int nWhich,ref long Data);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtNameFromCap(int nValue,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szValue,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtNameFromCapA(int nValue,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szValue,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetExtNameFromCapW(int nValue,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szValue,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetFeederAlignment(long Source,ref int lpAlignment);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetFeederFuncs(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetFeederOrder(long Source,ref int lpOrder);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetHighlight(long Source,ref double Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetHighlightString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetHighlightStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetHighlightStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetImageInfo(long Source,ref double lpXResolution,ref double lpYResolution,ref int lpWidth,ref int lpLength,ref int lpNumSamples,ref long lpBitsPerSample,ref int lpBitsPerPixel,ref int lpPlanar,ref int lpPixelType,ref int lpCompression);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetImageInfoString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder lpXResolution,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder lpYResolution,ref int lpWidth,ref int lpLength,ref int lpNumSamples,ref long lpBitsPerSample,ref int lpBitsPerPixel,ref int lpPlanar,ref int lpPixelType,ref int lpCompression);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetImageInfoStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder lpXResolution,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder lpYResolution,ref int lpWidth,ref int lpLength,ref int lpNumSamples,ref long lpBitsPerSample,ref int lpBitsPerPixel,ref int lpPlanar,ref int lpPixelType,ref int lpCompression);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetImageInfoStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder lpXResolution,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder lpYResolution,ref int lpWidth,ref int lpLength,ref int lpNumSamples,ref long lpBitsPerSample,ref int lpBitsPerPixel,ref int lpPlanar,ref int lpPixelType,ref int lpCompression);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetJobControl(long Source,ref int pJobControl,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetJpegValues(long Source,ref int pQuality,ref int Progressive);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetLanguage();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetLastError();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetLightPath(long Source,ref int lpLightPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetLightSources(long Source,ref long LightSources);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetManualDuplexCount(long Source,ref int pSide1,ref int pSide2);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetMaxAcquisitions(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetMaxBuffers(long Source,ref int pMaxBuf);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetMaxPagesToAcquire(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetMaxRetryAttempts(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNameFromCap(int nCapValue,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szValue,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNameFromCapA(int nCapValue,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szValue,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNameFromCapW(int nCapValue,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szValue,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNoiseFilter(long Source,ref int lpNoiseFilter);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNumAcquiredImages(long aAcq,int nWhich);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNumAcquisitions(long aAcq);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRCapValues(long Engine,int OCRCapValue,int nGetType,ref long CapValues);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRLastError(long Engine);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRManufacturer(long Engine,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szManufacturer,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRManufacturerA(long Engine,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szManufacturer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRManufacturerW(long Engine,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szManufacturer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRProductFamily(long Engine,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProductFamily,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRProductFamilyA(long Engine,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProductFamily,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRProductFamilyW(long Engine,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProductFamily,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRProductName(long Engine,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProductName,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRProductNameA(long Engine,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProductName,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRProductNameW(long Engine,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProductName,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetOCRText(long Engine,int nPageNo,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Data,int dSize,ref int pActualSize,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetOCRTextA(long Engine,int nPageNo,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Data,int dSize,ref int pActualSize,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRTextInfoFloat(int OCRTextInfo,int nCharPos,int nWhichItem,ref double pInfo);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRTextInfoFloatEx(int OCRTextInfo,int nWhichItem,ref double pInfo,int bufSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRTextInfoHandle(long Engine,int nPageNo);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRTextInfoLong(int OCRTextInfo,int nCharPos,int nWhichItem,ref int pInfo);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRTextInfoLongEx(int OCRTextInfo,int nWhichItem,ref int pInfo,int bufSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetOCRTextW(long Engine,int nPageNo,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Data,int dSize,ref int pActualSize,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRVersionInfo(long Engine,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder buffer,int maxBufSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRVersionInfoA(long Engine,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder buffer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRVersionInfoW(long Engine,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder buffer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOrientation(long Source,ref int lpOrient,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFTextElementFloat(long TextElement,ref double val1,ref double val2,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFTextElementLong(long TextElement,ref int val1,ref int val2,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFTextElementString(long TextElement,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szData,int maxLen,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFTextElementStringA(long TextElement,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szData,int maxLen,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFTextElementStringW(long TextElement,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szData,int maxLen,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFType1FontName(int FontVal,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szFont,int nChars);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFType1FontNameA(int FontVal,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szFont,int nChars);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPDFType1FontNameW(int FontVal,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szFont,int nChars);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPaperSize(long Source,ref int lpPaperSize,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPatchMaxPriorities(long Source,ref int pMaxPriorities,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPatchMaxRetries(long Source,ref int pMaxRetries,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPatchPriorities(long Source,ref long SearchPriorities);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPatchSearchMode(long Source,ref int pSearchMode,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPatchTimeOut(long Source,ref int pTimeOut,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPixelFlavor(long Source,ref int lpPixelFlavor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPixelType(long Source,ref int PixelType,ref int BitDepth,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPrinter(long Source,ref int lpPrinter,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPrinterStartNumber(long Source,ref int nStart);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPrinterStringMode(long Source,ref int PrinterMode,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPrinterStrings(long Source,ref long ArrayString);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPrinterSuffixString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Suffix,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPrinterSuffixStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Suffix,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetPrinterSuffixStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Suffix,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetRegisteredMsg();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetResolution(long Source,ref double Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetResolutionString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetResolutionStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetResolutionStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetRotation(long Source,ref double Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetRotationString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetRotationStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetRotationStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSaveFileName(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder fileName,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSaveFileNameA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder fileName,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSaveFileNameW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder fileName,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_GetSourceAcquisitions(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceManufacturer(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProduct,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceManufacturerA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceManufacturerW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductFamily(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProduct,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductFamilyA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductFamilyW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductName(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProduct,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductNameA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductNameW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceUnit(long Source,ref int lpUnit);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceVersionInfo(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szProduct,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceVersionInfoA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceVersionInfoW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szProduct,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceVersionNumber(long Source,ref int pMajor,ref int pMinor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetStaticLibVersion();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTempFileDirectory([MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szFilePath,int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTempFileDirectoryA([MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szFilePath,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTempFileDirectoryW([MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szFilePath,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetThreshold(long Source,ref double Threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetThresholdString(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder Threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetThresholdStringA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder Threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetThresholdStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder Threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTimeDate(long Source,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTimeDateA(long Source,[MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTimeDateW(long Source,[MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTwainAvailability();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTwainHwnd();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTwainMode();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTwainTimeout();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersion(ref int lpMajor,ref int lpMinor,ref int lpVersionType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionEx(ref int lMajor,ref int lMinor,ref int lVersionType,ref int lPatchLevel);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionInfo([MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionInfoA([MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionInfoW([MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionString([MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionStringA([MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionStringW([MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetShortVersionString([MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetShortVersionStringA([MarshalAs(UnmanagedType.LPStr)]System.Text.StringBuilder lpszVer,int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetShortVersionStringW([MarshalAs(UnmanagedType.LPWStr)]System.Text.StringBuilder lpszVer,int nLength);


        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_InitExtImageInfo(int Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_InitOCRInterface();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAcquiring();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoBorderDetectEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoBorderDetectSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoBrightEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoBrightSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoDeskewEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoDeskewSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoFeedEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoFeedSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoRotateEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsAutoScanEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsBlankPageDetectionOn(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsCapSupported(long Source,int lCapability);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsCompressionSupported(long Source,int Compression);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsCustomDSDataSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDIBBlank(long hDib,double threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDIBBlankString(long hDib,[MarshalAs(UnmanagedType.LPTStr)] string threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDIBBlankStringA(long hDib,[MarshalAs(UnmanagedType.LPStr)] string threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDIBBlankStringW(long hDib,[MarshalAs(UnmanagedType.LPWStr)] string threshold);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDeviceEventSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDeviceOnLine(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDuplexEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsDuplexSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsExtImageInfoSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsFeederEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsFeederLoaded(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsFeederSensitive(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsFeederSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsFileSystemSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsFileXferSupported(long Source,int lFileType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsIndicatorEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsIndicatorSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsInitialized();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsJPEGSupported();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsJobControlSupported(long Source,int JobControl);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsLampEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsLampSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsLightPathSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsLightSourceSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsMaxBuffersSupported(long Source,int MaxBuf);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsMsgNotifyEnabled();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsOCREngineActivated(long OCREngine);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsOrientationSupported(long Source,int Orientation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPDFSupported();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPNGSupported();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPaperDetectable(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPaperSizeSupported(long Source,int PaperSize);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPatchCapsSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPatchDetectEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPatchSupported(long Source,int PatchCode);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPixelTypeSupported(long Source,int PixelType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPrinterEnabled(long Source,int Printer);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsPrinterSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsSessionEnabled();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsSkipImageInfoError(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsSourceAcquiring(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsSourceOpen(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsTIFFSupported();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsThumbnailEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsThumbnailSupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsTwainAvailable();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsUIControllable(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsUIEnabled(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_IsUIOnlySupported(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_LoadCustomStringResources([MarshalAs(UnmanagedType.LPTStr)] string sLangDLL);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_LoadCustomStringResourcesA([MarshalAs(UnmanagedType.LPStr)] string sLangDLL);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_LoadCustomStringResourcesW([MarshalAs(UnmanagedType.LPWStr)] string sLangDLL);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_LoadLanguageResource(int nLanguage);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_LogMessage([MarshalAs(UnmanagedType.LPTStr)] string message);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_LogMessageA([MarshalAs(UnmanagedType.LPStr)] string message);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_LogMessageW([MarshalAs(UnmanagedType.LPWStr)] string message);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_OpenSource(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_OpenSourcesOnSelect(int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_RangeCreate(int nEnumType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_RangeCreateFromCap(long Source,int lCapType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeDestroy(long pSource);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeExpand(long pSource,ref long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetAllFloat(long pArray,ref double pVariantLow,ref double pVariantUp,ref double pVariantStep,ref double pVariantDefault,ref double pVariantCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetAllFloatString(long pArray,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder dLow,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder dUp,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder dStep,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder dDefault,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder dCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetAllLong(long pArray,ref int pVariantLow,ref int pVariantUp,ref int pVariantStep,ref int pVariantDefault,ref int pVariantCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetCount(long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetExpValueFloat(long pArray,int lPos,ref double pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetExpValueFloatString(long pArray,int lPos,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetExpValueLong(long pArray,int lPos,ref int pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetPosFloat(long pArray,double Val,ref int pPos);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetPosFloatString(long pArray,[MarshalAs(UnmanagedType.LPTStr)] string Val,ref int pPos);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetPosLong(long pArray,int Value,ref int pPos);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetValueFloat(long pArray,int nWhich,ref double pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetValueFloatString(long pArray,int nWhich,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeGetValueLong(long pArray,int nWhich,ref int pVal);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeIsValid(long Range,ref int pStatus);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeNearestValueFloat(long pArray,double dIn,ref double pOut,int RoundType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeNearestValueFloatString(long pArray,[MarshalAs(UnmanagedType.LPTStr)] string dIn,[MarshalAs(UnmanagedType.LPTStr)]System.Text.StringBuilder pOut,int RoundType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeNearestValueLong(long pArray,int lIn,ref int pOut,int RoundType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeSetAllFloat(long pArray,double dLow,double dUp,double dStep,double dDefault,double dCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeSetAllFloatString(long pArray,[MarshalAs(UnmanagedType.LPTStr)] string dLow,[MarshalAs(UnmanagedType.LPTStr)] string dUp,[MarshalAs(UnmanagedType.LPTStr)] string dStep,[MarshalAs(UnmanagedType.LPTStr)] string dDefault,[MarshalAs(UnmanagedType.LPTStr)] string dCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeSetAllLong(long pArray,int lLow,int lUp,int lStep,int lDefault,int lCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeSetValueFloat(long pArray,int nWhich,double Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeSetValueFloatString(long pArray,int nWhich,[MarshalAs(UnmanagedType.LPTStr)] string Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RangeSetValueLong(long pArray,int nWhich,int Val);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ResetPDFTextElement(long TextElement);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_RewindPage(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectDefaultOCREngine();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectDefaultSource();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectOCREngine();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectOCREngineByName([MarshalAs(UnmanagedType.LPTStr)] string lpszName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectOCREngineByNameA([MarshalAs(UnmanagedType.LPStr)] string lpszName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectOCREngineByNameW([MarshalAs(UnmanagedType.LPWStr)] string lpszName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectSource();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectSource2(int hWndParent,[MarshalAs(UnmanagedType.LPTStr)] string szTitle,int xPos,int yPos,int nOptions);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectSource2A(int hWndParent,[MarshalAs(UnmanagedType.LPStr)] string szTitle,int xPos,int yPos,int nOptions);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectSource2W(int hWndParent,[MarshalAs(UnmanagedType.LPWStr)] string szTitle,int xPos,int yPos,int nOptions);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectSourceByName([MarshalAs(UnmanagedType.LPTStr)] string lpszName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectSourceByNameA([MarshalAs(UnmanagedType.LPStr)] string lpszName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SelectSourceByNameW([MarshalAs(UnmanagedType.LPWStr)] string lpszName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireArea(long Source,int lSetType,long FloatEnum,long ActualEnum);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireArea2(long Source,double left,double top,double right,double bottom,int lUnit,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireArea2String(long Source,[MarshalAs(UnmanagedType.LPTStr)] string left,[MarshalAs(UnmanagedType.LPTStr)] string top,[MarshalAs(UnmanagedType.LPTStr)] string right,[MarshalAs(UnmanagedType.LPTStr)] string bottom,int lUnit,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireArea2StringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string left,[MarshalAs(UnmanagedType.LPStr)] string top,[MarshalAs(UnmanagedType.LPStr)] string right,[MarshalAs(UnmanagedType.LPStr)] string bottom,int lUnit,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireArea2StringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string left,[MarshalAs(UnmanagedType.LPWStr)] string top,[MarshalAs(UnmanagedType.LPWStr)] string right,[MarshalAs(UnmanagedType.LPWStr)] string bottom,int lUnit,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireImageNegative(long Source,int IsNegative);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireImageScale(long Source,double xscale,double yscale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireImageScaleString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string xscale,[MarshalAs(UnmanagedType.LPTStr)] string yscale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireImageScaleStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string xscale,[MarshalAs(UnmanagedType.LPStr)] string yscale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireImageScaleStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string xscale,[MarshalAs(UnmanagedType.LPWStr)] string yscale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAcquireStripBuffer(long Source,long hMem);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAlarmVolume(long Source,int Volume);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAlarms(long Source,long Alarms);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAllCapsToDefault(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAppInfo([MarshalAs(UnmanagedType.LPTStr)] string szVerStr,[MarshalAs(UnmanagedType.LPTStr)] string szManu,[MarshalAs(UnmanagedType.LPTStr)] string szProdFam,[MarshalAs(UnmanagedType.LPTStr)] string szProdName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAppInfoA([MarshalAs(UnmanagedType.LPStr)] string szVerStr,[MarshalAs(UnmanagedType.LPStr)] string szManu,[MarshalAs(UnmanagedType.LPStr)] string szProdFam,[MarshalAs(UnmanagedType.LPStr)] string szProdName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAppInfoW([MarshalAs(UnmanagedType.LPWStr)] string szVerStr,[MarshalAs(UnmanagedType.LPWStr)] string szManu,[MarshalAs(UnmanagedType.LPWStr)] string szProdFam,[MarshalAs(UnmanagedType.LPWStr)] string szProdName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAuthor(long Source,[MarshalAs(UnmanagedType.LPTStr)] string szAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAuthorA(long Source,[MarshalAs(UnmanagedType.LPStr)] string szAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAuthorW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string szAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAvailablePrinters(long Source,int lpAvailPrinters);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetAvailablePrintersArray(long Source,long AvailPrinters);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBitDepth(long Source,int BitDepth,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBlankPageDetection(long Source,double threshold,int autodetect_option,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBlankPageDetectionString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string threshold,int autodetect_option,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBlankPageDetectionStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string threshold,int autodetect_option,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBlankPageDetectionStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string threshold,int autodetect_option,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBrightness(long Source,double Brightness);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBrightnessString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Brightness);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBrightnessStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetBrightnessStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern DTwainCallback DTWAIN_SetCallback(DTwainCallback Fn,int UserData);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern DTwainCallback64 DTWAIN_SetCallback64(DTwainCallback64 Fn,long UserData);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCamera(long Source,[MarshalAs(UnmanagedType.LPTStr)] string szCamera);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCameraA(long Source,[MarshalAs(UnmanagedType.LPStr)] string szCamera);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCameraW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string szCamera);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCapValues(long Source,int lCap,int lSetType,long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCapValuesEx(long Source,int lCap,int lSetType,int lContainerType,long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCapValuesEx2(long Source,int lCap,int lSetType,int lContainerType,int nDataType,long pArray);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCaption(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Caption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCaptionA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Caption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCaptionW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Caption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCompressionType(long Source,int lCompression,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetContrast(long Source,double Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetContrastString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetContrastStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetContrastStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Contrast);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCountry(int nCountry);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCurrentRetryCount(long Source,int nCount);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetCustomDSData(long Source,long hData,ref int Data,int dSize,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetDSMSearchOrder(int SearchPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetDefaultSource(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetDeviceNotifications(long Source,int DevEvents);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetDeviceTimeDate(long Source,[MarshalAs(UnmanagedType.LPTStr)] string szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetDeviceTimeDateA(long Source,[MarshalAs(UnmanagedType.LPStr)] string szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetDeviceTimeDateW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string szTimeDate);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetEOJDetectValue(long Source,int nValue);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetErrorBufferThreshold(int nErrors);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetFeederAlignment(long Source,int lpAlignment);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetFeederOrder(long Source,int lOrder);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetFileAutoIncrement(long Source,int Increment,int bResetOnAcquire,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetFileSavePos(int hWndParent,[MarshalAs(UnmanagedType.LPTStr)] string szTitle,int xPos,int yPos,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetFileSavePosA(int hWndParent,[MarshalAs(UnmanagedType.LPStr)] string szTitle,int xPos,int yPos,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetFileSavePosW(int hWndParent,[MarshalAs(UnmanagedType.LPWStr)] string szTitle,int xPos,int yPos,int nFlags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetFileXferFormat(long Source,int lFileType,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetHighlight(long Source,double Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetHighlightString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetHighlightStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetHighlightStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Highlight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetJobControl(long Source,int JobControl,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetJpegValues(long Source,int Quality,int Progressive);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetLanguage(int nLanguage);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetLightPath(long Source,int LightPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetLightPathEx(long Source,long LightPaths);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetLightSources(long Source,long LightSources);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetLoggerCallback(DTwainLoggerProc logProc);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetManualDuplexMode(long Source,int Flags,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetMaxAcquisitions(long Source,int MaxAcquires);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetMaxBuffers(long Source,int MaxBuf);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetMaxRetryAttempts(long Source,int nAttempts);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetMultipageScanMode(long Source,int ScanType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetNoiseFilter(long Source,int NoiseFilter);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetOCRCapValues(long Engine,int OCRCapValue,int SetType,long CapValues);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetOrientation(long Source,int Orient,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFASCIICompression(long Source,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFAuthor(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFAuthorA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFAuthorW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpAuthor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFCompression(long Source,int bCompression);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFCreator(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpCreator);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFCreatorA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpCreator);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFCreatorW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpCreator);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFEncryption(long Source,int bUseEncryption,[MarshalAs(UnmanagedType.LPTStr)] string lpszUser,[MarshalAs(UnmanagedType.LPTStr)] string lpszOwner,int Permissions,int UseStrongEncryption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFEncryptionA(long Source,int bUseEncryption,[MarshalAs(UnmanagedType.LPStr)] string lpszUser,[MarshalAs(UnmanagedType.LPStr)] string lpszOwner,int Permissions,int UseStrongEncryption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFEncryptionW(long Source,int bUseEncryption,[MarshalAs(UnmanagedType.LPWStr)] string lpszUser,[MarshalAs(UnmanagedType.LPWStr)] string lpszOwner,int Permissions,int UseStrongEncryption);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFJpegQuality(long Source,int Quality);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFKeywords(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpKeyWords);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFKeywordsA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpKeyWords);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFKeywordsW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpKeyWords);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFOCRConversion(long Engine,int PageType,int FileType,int PixelType,int BitDepth,int Options);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFOCRMode(long Source,int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFOrientation(long Source,int lPOrientation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageScale(long Source,int nOptions,double xScale,double yScale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageScaleString(long Source,int nOptions,[MarshalAs(UnmanagedType.LPTStr)] string xScale,[MarshalAs(UnmanagedType.LPTStr)] string yScale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageScaleStringA(long Source,int nOptions,[MarshalAs(UnmanagedType.LPStr)] string xScale,[MarshalAs(UnmanagedType.LPStr)] string yScale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageScaleStringW(long Source,int nOptions,[MarshalAs(UnmanagedType.LPWStr)] string xScale,[MarshalAs(UnmanagedType.LPWStr)] string yScale);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageSize(long Source,int PageSize,double CustomWidth,double CustomHeight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageSizeString(long Source,int PageSize,[MarshalAs(UnmanagedType.LPTStr)] string CustomWidth,[MarshalAs(UnmanagedType.LPTStr)] string CustomHeight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageSizeStringA(long Source,int PageSize,[MarshalAs(UnmanagedType.LPStr)] string CustomWidth,[MarshalAs(UnmanagedType.LPStr)] string CustomHeight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFPageSizeStringW(long Source,int PageSize,[MarshalAs(UnmanagedType.LPWStr)] string CustomWidth,[MarshalAs(UnmanagedType.LPWStr)] string CustomHeight);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFProducer(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpProducer);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFProducerA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpProducer);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFProducerW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpProducer);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFSubject(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpSubject);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFSubjectA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpSubject);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFSubjectW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpSubject);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFTextElementFloat(long TextElement,double val1,double val2,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFTextElementLong(long TextElement,int val1,int val2,int Flags);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFTitle(long Source,[MarshalAs(UnmanagedType.LPTStr)] string lpTitle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFTitleA(long Source,[MarshalAs(UnmanagedType.LPStr)] string lpTitle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPDFTitleW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string lpTitle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPaperSize(long Source,int PaperSize,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPatchMaxPriorities(long Source,int nMaxSearchRetries);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPatchMaxRetries(long Source,int nMaxRetries);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPatchPriorities(long Source,long SearchPriorities);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPatchSearchMode(long Source,int nSearchMode);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPatchTimeOut(long Source,int TimeOutValue);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPixelFlavor(long Source,int PixelFlavor);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPixelType(long Source,int PixelType,int BitDepth,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPostScriptTitle(long Source,[MarshalAs(UnmanagedType.LPTStr)] string szTitle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPostScriptTitleA(long Source,[MarshalAs(UnmanagedType.LPStr)] string szTitle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPostScriptTitleW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string szTitle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPostScriptType(long Source,int PSType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPrinter(long Source,int Printer,int bCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPrinterStartNumber(long Source,int nStart);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPrinterStringMode(long Source,int PrinterMode,int bSetCurrent);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPrinterStrings(long Source,long ArrayString,ref int pNumStrings);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPrinterSuffixString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Suffix);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPrinterSuffixStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Suffix);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetPrinterSuffixStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Suffix);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetQueryCapSupport(int bSet);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetResolution(long Source,double Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetResolutionString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetResolutionStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetResolutionStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Resolution);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetRotation(long Source,double Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetRotationString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetRotationStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetRotationStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Rotation);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetSaveFileName(long Source,[MarshalAs(UnmanagedType.LPTStr)] string fileName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetSaveFileNameA(long Source,[MarshalAs(UnmanagedType.LPStr)] string fileName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetSaveFileNameW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetSourceUnit(long Source,int lpUnit);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTIFFCompressType(long Source,int Setting);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTIFFInvert(long Source,int Setting);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTempFileDirectory([MarshalAs(UnmanagedType.LPTStr)] string szFilePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTempFileDirectoryA([MarshalAs(UnmanagedType.LPStr)] string szFilePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTempFileDirectoryW([MarshalAs(UnmanagedType.LPWStr)] string szFilePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetThreshold(long Source,double Threshold,int bSetBithDepthReduction);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetThresholdString(long Source,[MarshalAs(UnmanagedType.LPTStr)] string Threshold,int bSetBitDepthReduction);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetThresholdStringA(long Source,[MarshalAs(UnmanagedType.LPStr)] string Threshold,int bSetBitDepthReduction);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetThresholdStringW(long Source,[MarshalAs(UnmanagedType.LPWStr)] string Threshold,int bSetBitDepthReduction);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTwainDialogFont(System.IntPtr hFont);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTwainDSM(int DSMType);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTwainLog(int LogFlags,[MarshalAs(UnmanagedType.LPTStr)] string lpszLogFile);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTwainLogA(int LogFlags,[MarshalAs(UnmanagedType.LPStr)] string lpszLogFile);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTwainLogW(int LogFlags,[MarshalAs(UnmanagedType.LPWStr)] string lpszLogFile);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTwainMode(int lAcquireMode);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SetTwainTimeout(int milliseconds);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern DTwainDibUpdateCallback DTWAIN_SetUpdateDibProc(DTwainDibUpdateCallback DibProc);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ShowUIOnly(long Source);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_ShutdownOCREngine(long OCREngine);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SkipImageInfoError(long Source,int bSkip);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_StartThread(long DLLHandle);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_StartTwainSession(int hWndMsg,[MarshalAs(UnmanagedType.LPTStr)] string lpszDLLName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_StartTwainSessionA(int hWndMsg,[MarshalAs(UnmanagedType.LPStr)] string lpszDLLName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_StartTwainSessionW(int hWndMsg,[MarshalAs(UnmanagedType.LPWStr)] string lpszDLLName);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_SysDestroy();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitialize();

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeEx([MarshalAs(UnmanagedType.LPTStr)] string szINIPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeEx2([MarshalAs(UnmanagedType.LPTStr)] string szINIPath,[MarshalAs(UnmanagedType.LPTStr)] string szImageDLLPath,[MarshalAs(UnmanagedType.LPTStr)] string szLangResourcePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeEx2A([MarshalAs(UnmanagedType.LPStr)] string szINIPath,[MarshalAs(UnmanagedType.LPStr)] string szImageDLLPath,[MarshalAs(UnmanagedType.LPStr)] string szLangResourcePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeEx2W([MarshalAs(UnmanagedType.LPWStr)] string szINIPath,[MarshalAs(UnmanagedType.LPWStr)] string szImageDLLPath,[MarshalAs(UnmanagedType.LPWStr)] string szLangResourcePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeExA([MarshalAs(UnmanagedType.LPStr)] string szINIPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeExW([MarshalAs(UnmanagedType.LPWStr)] string szINIPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeLib(long hInstance);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeLibEx(long hInstance,[MarshalAs(UnmanagedType.LPTStr)] string szINIPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeLibEx2(long hInstance,[MarshalAs(UnmanagedType.LPTStr)] string szINIPath,[MarshalAs(UnmanagedType.LPTStr)] string szImageDLLPath,[MarshalAs(UnmanagedType.LPTStr)] string szLangResourcePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeLibEx2A(long hInstance,[MarshalAs(UnmanagedType.LPStr)] string szINIPath,[MarshalAs(UnmanagedType.LPStr)] string szImageDLLPath,[MarshalAs(UnmanagedType.LPStr)] string szLangResourcePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeLibEx2W(long hInstance,[MarshalAs(UnmanagedType.LPWStr)] string szINIPath,[MarshalAs(UnmanagedType.LPWStr)] string szImageDLLPath,[MarshalAs(UnmanagedType.LPWStr)] string szLangResourcePath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeLibExA(long hInstance,[MarshalAs(UnmanagedType.LPStr)] string szINIPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern long DTWAIN_SysInitializeLibExW(long hInstance,[MarshalAs(UnmanagedType.LPWStr)] string szINIPath);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_TwainSave([MarshalAs(UnmanagedType.LPTStr)] string cmd);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_TwainSaveA([MarshalAs(UnmanagedType.LPStr)] string cmd);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_TwainSaveW([MarshalAs(UnmanagedType.LPWStr)] string cmd);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern void DTWAIN_X([MarshalAs(UnmanagedType.LPTStr)] string s);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern void DTWAIN_XA([MarshalAs(UnmanagedType.LPStr)] string s);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern void DTWAIN_XW([MarshalAs(UnmanagedType.LPWStr)] string s);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorString(int nError, System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNameFromCap(int nCapValue, System.LongPtr sPtr, int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTempFileDirectory(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionString(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetShortVersionString(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionInfo(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceManufacturer(long Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductFamily(long Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductName(long Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceVersionInfo(long Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRErrorString(long Engine, int nError, System.LongPtr lpszVer, int nLength);
        [DllImport("DTWAIN32.DLL", CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorStringA(int nError, System.IntPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNameFromCapA(int nCapValue, System.LongPtr sPtr, int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionStringA(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetShortVersionStringA(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTempFileDirectoryA(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionInfoA(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceManufacturerA(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductFamilyA(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductNameA(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceVersionInfoA(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Ansi,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRErrorStringA(int Engine, int nError, System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetErrorStringW(int nError, System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_GetNameFromCapW(int nCapValue, System.LongPtr sPtr, int nMaxLen);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionStringW(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetShortVersionStringW(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetTempFileDirectoryW(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetVersionInfoW(System.LongPtr lpszVer, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceManufacturerW(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductFamilyW(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceProductNameW(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetSourceVersionInfoW(int Source, System.LongPtr sPtr, int nLength);

        [DllImport(DTWAIN_LIBRARY, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DTWAIN_GetOCRErrorStringW(int Engine, int nError, System.LongPtr lpszVer, int nLength);
        
        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Auto,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_StartTwainSession(int hWndMsg, System.IntPtr sPtr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Ansi,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_StartTwainSessionA(int hWndMsg, System.IntPtr sPtr);

        [DllImport(DTWAIN_LIBRARY, CharSet=CharSet.Unicode,
        ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int DTWAIN_StartTwainSessionW(int hWndMsg, System.IntPtr sPtr);

    }
}
