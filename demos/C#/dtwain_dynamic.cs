    /*
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
    */
    #pragma warning disable 0649
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.IO;

    namespace Dynarithmic
    {
        using DTWAIN_SOURCE = System.IntPtr;
        using DTWAIN_ARRAY = System.IntPtr;
        using DTWAIN_RANGE = System.IntPtr;
        using DTWAIN_FRAME = System.IntPtr;
        using DTWAIN_PDFTEXTELEMENT = System.IntPtr;
        using DTWAIN_HANDLE = System.IntPtr;
        using NULL_HANDLE = System.IntPtr;
        using DTWAIN_IDENTITY = System.IntPtr;
        using DTWAIN_OCRENGINE = System.IntPtr;
        using DTWAIN_OCRTEXTINFOHANDLE = System.IntPtr;
        using TW_UINT16 = System.UInt16;
        using TW_UINT32 = System.UInt32;
        using TW_BOOL = System.UInt16;
        using DTWAIN_MEMORY_PTR = System.IntPtr;
        using ULONG64 = System.UInt64;
        using LONG64 = System.Int64;
        using LONGLONG = System.Int64;
        using DWORD = System.UInt32;
        using LONG = System.Int32;
        using HINSTANCE = System.IntPtr;
        using HWND = System.IntPtr;
        using DTWAIN_FLOAT = System.Double;
        using HANDLE = System.IntPtr;
        using HFONT = System.IntPtr;

        [AttributeUsage(AttributeTargets.Field)]
        public sealed class DTWAINNativeFunctionAttribute : Attribute
        {
            public string EntryPoint { get; }
        
            public DTWAINNativeFunctionAttribute(string entryPoint)
            {
                EntryPoint = entryPoint;
            }
        }

        public abstract class DTWAINNativeLibraryBase : IDisposable
        {
            private IntPtr _module;
            private string _sysErrorMessage;

            public string GetErrorMessage() { return _sysErrorMessage; }
            protected DTWAINNativeLibraryBase(string dllPath)
            {
                _sysErrorMessage = "";
                string[] dll32 = { "dtwain32u.dll", "dtwain32ud.dll" };
                string[] dll64 = { "dtwain64u.dll", "dtwain64ud.dll" };
                string filename = Path.GetFileName(dllPath).ToLower();

                int index1 = Array.IndexOf(dll32, filename);
                int index2 = Array.IndexOf(dll64, filename);

                bool is32bit = (IntPtr.Size == 4);
                bool is64bit = (IntPtr.Size == 8);

                string joinedNames = default;
                if (is32bit)
                    joinedNames = string.Join(", ", dll32);
                else
                    joinedNames = string.Join(", ", dll64);

                if (index1 == -1 && index2 == -1)
                    _sysErrorMessage = "DTWAIN DLL file name " + filename +
                        " is not valid (must be one of the following: [" + joinedNames + "])";
                if (!String.IsNullOrEmpty(_sysErrorMessage) && is64bit && index1 != -1)
                _sysErrorMessage = "Cannot load 32-bit DTWAIN DLL in a 64-bit process";

            if (!String.IsNullOrEmpty(_sysErrorMessage) && is32bit && index2 != -1)
                _sysErrorMessage = "Cannot load 64-bit DTWAIN DLL in a 32-bit process";
            if (String.IsNullOrEmpty(_sysErrorMessage))
            {
                _module = LoadLibrary(dllPath);
                if (_module == IntPtr.Zero)
                    _sysErrorMessage = "Failed to load " + dllPath + ", error=" + Marshal.GetLastWin32Error();
                else
                {
                    try
                    {
                        BindFunctions();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        private void BindFunctions()
        {
            var fields = GetType().GetFields(
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public);

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<DTWAINNativeFunctionAttribute>();
                if (attr == null)
                    continue;

                IntPtr proc = GetProcAddress(_module, attr.EntryPoint);
                if (proc == IntPtr.Zero)
                    throw new EntryPointNotFoundException(attr.EntryPoint);

                Delegate del = Marshal.GetDelegateForFunctionPointer(
                    proc, field.FieldType);

                field.SetValue(this, del);
            }
        }

        public void Dispose()
        {
            if (_module != IntPtr.Zero)
            {
                FreeLibrary(_module);
                _module = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        ~DTWAINNativeLibraryBase() => Dispose();

        // ---------------- Win32 ----------------

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);
    }

    public sealed class TwainAPI : DTWAINNativeLibraryBase
    {
        public const int DTWAIN_FF_TIFF = 0;
        public const int DTWAIN_FF_PICT = 1;
        public const int DTWAIN_FF_BMP = 2;
        public const int DTWAIN_FF_XBM = 3;
        public const int DTWAIN_FF_JFIF = 4;
        public const int DTWAIN_FF_FPX = 5;
        public const int DTWAIN_FF_TIFFMULTI = 6;
        public const int DTWAIN_FF_PNG = 7;
        public const int DTWAIN_FF_SPIFF = 8;
        public const int DTWAIN_FF_EXIF = 9;
        public const int DTWAIN_FF_PDF = 10;
        public const int DTWAIN_FF_JP2 = 11;
        public const int DTWAIN_FF_JPX = 13;
        public const int DTWAIN_FF_DEJAVU = 14;
        public const int DTWAIN_FF_PDFA = 15;
        public const int DTWAIN_FF_PDFA2 = 16;
        public const int DTWAIN_FF_PDFRASTER = 17;
        public const int DTWAIN_CP_NONE = 0;
        public const int DTWAIN_CP_PACKBITS = 1;
        public const int DTWAIN_CP_GROUP31D = 2;
        public const int DTWAIN_CP_GROUP31DEOL = 3;
        public const int DTWAIN_CP_GROUP32D = 4;
        public const int DTWAIN_CP_GROUP4 = 5;
        public const int DTWAIN_CP_JPEG = 6;
        public const int DTWAIN_CP_LZW = 7;
        public const int DTWAIN_CP_JBIG = 8;
        public const int DTWAIN_CP_PNG = 9;
        public const int DTWAIN_CP_RLE4 = 10;
        public const int DTWAIN_CP_RLE8 = 11;
        public const int DTWAIN_CP_BITFIELDS = 12;
        public const int DTWAIN_CP_ZIP = 13;
        public const int DTWAIN_CP_JPEG2000 = 14;
        public const int DTWAIN_FS_NONE = 0;
        public const int DTWAIN_FS_A4LETTER = 1;
        public const int DTWAIN_FS_B5LETTER = 2;
        public const int DTWAIN_FS_USLETTER = 3;
        public const int DTWAIN_FS_USLEGAL = 4;
        public const int DTWAIN_FS_A5 = 5;
        public const int DTWAIN_FS_B4 = 6;
        public const int DTWAIN_FS_B6 = 7;
        public const int DTWAIN_FS_USLEDGER = 9;
        public const int DTWAIN_FS_USEXECUTIVE = 10;
        public const int DTWAIN_FS_A3 = 11;
        public const int DTWAIN_FS_B3 = 12;
        public const int DTWAIN_FS_A6 = 13;
        public const int DTWAIN_FS_C4 = 14;
        public const int DTWAIN_FS_C5 = 15;
        public const int DTWAIN_FS_C6 = 16;
        public const int DTWAIN_FS_4A0 = 17;
        public const int DTWAIN_FS_2A0 = 18;
        public const int DTWAIN_FS_A0 = 19;
        public const int DTWAIN_FS_A1 = 20;
        public const int DTWAIN_FS_A2 = 21;
        public const int DTWAIN_FS_A4 = DTWAIN_FS_A4LETTER;
        public const int DTWAIN_FS_A7 = 22;
        public const int DTWAIN_FS_A8 = 23;
        public const int DTWAIN_FS_A9 = 24;
        public const int DTWAIN_FS_A10 = 25;
        public const int DTWAIN_FS_ISOB0 = 26;
        public const int DTWAIN_FS_ISOB1 = 27;
        public const int DTWAIN_FS_ISOB2 = 28;
        public const int DTWAIN_FS_ISOB3 = DTWAIN_FS_B3;
        public const int DTWAIN_FS_ISOB4 = DTWAIN_FS_B4;
        public const int DTWAIN_FS_ISOB5 = 29;
        public const int DTWAIN_FS_ISOB6 = DTWAIN_FS_B6;
        public const int DTWAIN_FS_ISOB7 = 30;
        public const int DTWAIN_FS_ISOB8 = 31;
        public const int DTWAIN_FS_ISOB9 = 32;
        public const int DTWAIN_FS_ISOB10 = 33;
        public const int DTWAIN_FS_JISB0 = 34;
        public const int DTWAIN_FS_JISB1 = 35;
        public const int DTWAIN_FS_JISB2 = 36;
        public const int DTWAIN_FS_JISB3 = 37;
        public const int DTWAIN_FS_JISB4 = 38;
        public const int DTWAIN_FS_JISB5 = DTWAIN_FS_B5LETTER;
        public const int DTWAIN_FS_JISB6 = 39;
        public const int DTWAIN_FS_JISB7 = 40;
        public const int DTWAIN_FS_JISB8 = 41;
        public const int DTWAIN_FS_JISB9 = 42;
        public const int DTWAIN_FS_JISB10 = 43;
        public const int DTWAIN_FS_C0 = 44;
        public const int DTWAIN_FS_C1 = 45;
        public const int DTWAIN_FS_C2 = 46;
        public const int DTWAIN_FS_C3 = 47;
        public const int DTWAIN_FS_C7 = 48;
        public const int DTWAIN_FS_C8 = 49;
        public const int DTWAIN_FS_C9 = 50;
        public const int DTWAIN_FS_C10 = 51;
        public const int DTWAIN_FS_USSTATEMENT = 52;
        public const int DTWAIN_FS_BUSINESSCARD = 53;
        public const int DTWAIN_ANYSUPPORT = (-1);
        public const int DTWAIN_BMP = 100;
        public const int DTWAIN_JPEG = 200;
        public const int DTWAIN_PDF = 250;
        public const int DTWAIN_PDFMULTI = 251;
        public const int DTWAIN_PCX = 300;
        public const int DTWAIN_DCX = 301;
        public const int DTWAIN_TGA = 400;
        public const int DTWAIN_TIFFLZW = 500;
        public const int DTWAIN_TIFFNONE = 600;
        public const int DTWAIN_TIFFG3 = 700;
        public const int DTWAIN_TIFFG4 = 800;
        public const int DTWAIN_TIFFPACKBITS = 801;
        public const int DTWAIN_TIFFDEFLATE = 802;
        public const int DTWAIN_TIFFJPEG = 803;
        public const int DTWAIN_TIFFJBIG = 804;
        public const int DTWAIN_TIFFPIXARLOG = 805;
        public const int DTWAIN_TIFFNONEMULTI = 900;
        public const int DTWAIN_TIFFG3MULTI = 901;
        public const int DTWAIN_TIFFG4MULTI = 902;
        public const int DTWAIN_TIFFPACKBITSMULTI = 903;
        public const int DTWAIN_TIFFDEFLATEMULTI = 904;
        public const int DTWAIN_TIFFJPEGMULTI = 905;
        public const int DTWAIN_TIFFLZWMULTI = 906;
        public const int DTWAIN_TIFFJBIGMULTI = 907;
        public const int DTWAIN_TIFFPIXARLOGMULTI = 908;
        public const int DTWAIN_WMF = 850;
        public const int DTWAIN_EMF = 851;
        public const int DTWAIN_GIF = 950;
        public const int DTWAIN_PNG = 1000;
        public const int DTWAIN_PSD = 2000;
        public const int DTWAIN_JPEG2000 = 3000;
        public const int DTWAIN_POSTSCRIPT1 = 4000;
        public const int DTWAIN_POSTSCRIPT2 = 4001;
        public const int DTWAIN_POSTSCRIPT3 = 4002;
        public const int DTWAIN_POSTSCRIPT1MULTI = 4003;
        public const int DTWAIN_POSTSCRIPT2MULTI = 4004;
        public const int DTWAIN_POSTSCRIPT3MULTI = 4005;
        public const int DTWAIN_TEXT = 6000;
        public const int DTWAIN_TEXTMULTI = 6001;
        public const int DTWAIN_TIFFMULTI = 7000;
        public const int DTWAIN_ICO = 8000;
        public const int DTWAIN_ICO_VISTA = 8001;
        public const int DTWAIN_ICO_RESIZED = 8002;
        public const int DTWAIN_WBMP = 8500;
        public const int DTWAIN_WEBP = 8501;
        public const int DTWAIN_PPM = 10000;
        public const int DTWAIN_WBMP_RESIZED = 11000;
        public const int DTWAIN_TGA_RLE = 11001;
        public const int DTWAIN_BMP_RLE = 11002;
        public const int DTWAIN_BIGTIFFLZW = 11003;
        public const int DTWAIN_BIGTIFFLZWMULTI = 11004;
        public const int DTWAIN_BIGTIFFNONE = 11005;
        public const int DTWAIN_BIGTIFFNONEMULTI = 11006;
        public const int DTWAIN_BIGTIFFPACKBITS = 11007;
        public const int DTWAIN_BIGTIFFPACKBITSMULTI = 11008;
        public const int DTWAIN_BIGTIFFDEFLATE = 11009;
        public const int DTWAIN_BIGTIFFDEFLATEMULTI = 11010;
        public const int DTWAIN_BIGTIFFG3 = 11011;
        public const int DTWAIN_BIGTIFFG3MULTI = 11012;
        public const int DTWAIN_BIGTIFFG4 = 11013;
        public const int DTWAIN_BIGTIFFG4MULTI = 11014;
        public const int DTWAIN_BIGTIFFJPEG = 11015;
        public const int DTWAIN_BIGTIFFJPEGMULTI = 11016;
        public const int DTWAIN_JPEGXR = 12000;
        public const int DTWAIN_INCHES = 0;
        public const int DTWAIN_CENTIMETERS = 1;
        public const int DTWAIN_PICAS = 2;
        public const int DTWAIN_POINTS = 3;
        public const int DTWAIN_TWIPS = 4;
        public const int DTWAIN_PIXELS = 5;
        public const int DTWAIN_MILLIMETERS = 6;
        public const int DTWAIN_USENAME = 16;
        public const int DTWAIN_USEPROMPT = 32;
        public const int DTWAIN_USELONGNAME = 64;
        public const int DTWAIN_USESOURCEMODE = 128;
        public const int DTWAIN_USELIST = 256;
        public const int DTWAIN_CREATE_DIRECTORY = 512;
        public const int DTWAIN_CREATEDIRECTORY = DTWAIN_CREATE_DIRECTORY;
        public const int DTWAIN_ARRAYANY = 1;
        public const int DTWAIN_ArrayTypePTR = 1;
        public const int DTWAIN_ARRAYLONG = 2;
        public const int DTWAIN_ARRAYFLOAT = 3;
        public const int DTWAIN_ARRAYHANDLE = 4;
        public const int DTWAIN_ARRAYSOURCE = 5;
        public const int DTWAIN_ARRAYSTRING = 6;
        public const int DTWAIN_ARRAYFRAME = 7;
        public const int DTWAIN_ARRAYBOOL = DTWAIN_ARRAYLONG;
        public const int DTWAIN_ARRAYLONGSTRING = 8;
        public const int DTWAIN_ARRAYUNICODESTRING = 9;
        public const int DTWAIN_ARRAYLONG64 = 10;
        public const int DTWAIN_ARRAYANSISTRING = 11;
        public const int DTWAIN_ARRAYWIDESTRING = 12;
        public const int DTWAIN_ARRAYTWFIX32 = 200;
        public const int DTWAIN_ArrayTypeINVALID = 0;
        public const int DTWAIN_ARRAYINT16 = 100;
        public const int DTWAIN_ARRAYUINT16 = 110;
        public const int DTWAIN_ARRAYUINT32 = 120;
        public const int DTWAIN_ARRAYINT32 = 130;
        public const int DTWAIN_ARRAYINT64 = 140;
        public const int DTWAIN_ARRAYUINT64 = 150;
        public const int DTWAIN_RANGELONG = DTWAIN_ARRAYLONG;
        public const int DTWAIN_RANGEFLOAT = DTWAIN_ARRAYFLOAT;
        public const int DTWAIN_RANGEMIN = 0;
        public const int DTWAIN_RANGEMAX = 1;
        public const int DTWAIN_RANGESTEP = 2;
        public const int DTWAIN_RANGEDEFAULT = 3;
        public const int DTWAIN_RANGECURRENT = 4;
        public const int DTWAIN_FRAMELEFT = 0;
        public const int DTWAIN_FRAMETOP = 1;
        public const int DTWAIN_FRAMERIGHT = 2;
        public const int DTWAIN_FRAMEBOTTOM = 3;
        public const int DTWAIN_FIX32WHOLE = 0;
        public const int DTWAIN_FIX32FRAC = 1;
        public const int DTWAIN_JC_NONE = 0;
        public const int DTWAIN_JC_JSIC = 1;
        public const int DTWAIN_JC_JSIS = 2;
        public const int DTWAIN_JC_JSXC = 3;
        public const int DTWAIN_JC_JSXS = 4;
        public const int DTWAIN_CAPDATATYPE_UNKNOWN = (-10);
        public const int DTWAIN_JCBP_JSIC = 5;
        public const int DTWAIN_JCBP_JSIS = 6;
        public const int DTWAIN_JCBP_JSXC = 7;
        public const int DTWAIN_JCBP_JSXS = 8;
        public const int DTWAIN_FEEDPAGEON = 1;
        public const int DTWAIN_CLEARPAGEON = 2;
        public const int DTWAIN_REWINDPAGEON = 4;
        public const int DTWAIN_AppOwnsDib = 1;
        public const int DTWAIN_SourceOwnsDib = 2;
        public const int DTWAIN_CONTARRAY = 8;
        public const int DTWAIN_CONTENUMERATION = 16;
        public const int DTWAIN_CONTONEVALUE = 32;
        public const int DTWAIN_CONTRANGE = 64;
        public const int DTWAIN_CONTDEFAULT = 0;
        public const int DTWAIN_CAPGET = 1;
        public const int DTWAIN_CAPGETCURRENT = 2;
        public const int DTWAIN_CAPGETDEFAULT = 3;
        public const int DTWAIN_CAPSET = 6;
        public const int DTWAIN_CAPRESET = 7;
        public const int DTWAIN_CAPRESETALL = 8;
        public const int DTWAIN_CAPSETCONSTRAINT = 9;
        public const int DTWAIN_CAPSETAVAILABLE = 8;
        public const int DTWAIN_CAPSETCURRENT = 16;
        public const int DTWAIN_CAPGETHELP = 9;
        public const int DTWAIN_CAPGETLABEL = 10;
        public const int DTWAIN_CAPGETLABELENUM = 11;
        public const int DTWAIN_AREASET = DTWAIN_CAPSET;
        public const int DTWAIN_AREARESET = DTWAIN_CAPRESET;
        public const int DTWAIN_AREACURRENT = DTWAIN_CAPGETCURRENT;
        public const int DTWAIN_AREADEFAULT = DTWAIN_CAPGETDEFAULT;
        public const int DTWAIN_VER15 = 0;
        public const int DTWAIN_VER16 = 1;
        public const int DTWAIN_VER17 = 2;
        public const int DTWAIN_VER18 = 3;
        public const int DTWAIN_VER20 = 4;
        public const int DTWAIN_VER21 = 5;
        public const int DTWAIN_VER22 = 6;
        public const int DTWAIN_ACQUIREALL = (-1);
        public const int DTWAIN_MAXACQUIRE = (-1);
        public const int DTWAIN_DX_NONE = 0;
        public const int DTWAIN_DX_1PASSDUPLEX = 1;
        public const int DTWAIN_DX_2PASSDUPLEX = 2;
        public const int DTWAIN_PT_BW = 0;
        public const int DTWAIN_PT_GRAY = 1;
        public const int DTWAIN_PT_RGB = 2;
        public const int DTWAIN_PT_PALETTE = 3;
        public const int DTWAIN_PT_CMY = 4;
        public const int DTWAIN_PT_CMYK = 5;
        public const int DTWAIN_PT_YUV = 6;
        public const int DTWAIN_PT_YUVK = 7;
        public const int DTWAIN_PT_CIEXYZ = 8;
        public const int DTWAIN_PT_DEFAULT = 1000;
        public const int DTWAIN_CURRENT = (-2);
        public const int DTWAIN_DEFAULT = (-1);
        public const double DTWAIN_FLOATDEFAULT = (-9999.0);
        public const int DTWAIN_CallbackERROR = 1;
        public const int DTWAIN_CallbackMESSAGE = 2;
        public const int DTWAIN_USENATIVE = 1;
        public const int DTWAIN_USEBUFFERED = 2;
        public const int DTWAIN_USECOMPRESSION = 4;
        public const int DTWAIN_USEMEMFILE = 8;
        public const int DTWAIN_FAILURE1 = (-1);
        public const int DTWAIN_FAILURE2 = (-2);
        public const int DTWAIN_DELETEALL = (-1);
        public const int DTWAIN_TN_ACQUIREDONE = 1000;
        public const int DTWAIN_TN_ACQUIREFAILED = 1001;
        public const int DTWAIN_TN_ACQUIRECANCELLED = 1002;
        public const int DTWAIN_TN_ACQUIRESTARTED = 1003;
        public const int DTWAIN_TN_PAGECONTINUE = 1004;
        public const int DTWAIN_TN_PAGEFAILED = 1005;
        public const int DTWAIN_TN_PAGECANCELLED = 1006;
        public const int DTWAIN_TN_TRANSFERREADY = 1009;
        public const int DTWAIN_TN_TRANSFERDONE = 1010;
        public const int DTWAIN_TN_ACQUIREPAGEDONE = 1010;
        public const int DTWAIN_TN_UICLOSING = 3000;
        public const int DTWAIN_TN_UICLOSED = 3001;
        public const int DTWAIN_TN_UIOPENED = 3002;
        public const int DTWAIN_TN_UIOPENING = 3003;
        public const int DTWAIN_TN_UIOPENFAILURE = 3004;
        public const int DTWAIN_TN_CLIPTRANSFERDONE = 1014;
        public const int DTWAIN_TN_INVALIDIMAGEFORMAT = 1015;
        public const int DTWAIN_TN_ACQUIRETERMINATED = 1021;
        public const int DTWAIN_TN_TRANSFERSTRIPREADY = 1022;
        public const int DTWAIN_TN_TRANSFERSTRIPDONE = 1023;
        public const int DTWAIN_TN_TRANSFERSTRIPFAILED = 1029;
        public const int DTWAIN_TN_IMAGEINFOERROR = 1024;
        public const int DTWAIN_TN_TRANSFERCANCELLED = 1030;
        public const int DTWAIN_TN_FILESAVECANCELLED = 1031;
        public const int DTWAIN_TN_FILESAVEOK = 1032;
        public const int DTWAIN_TN_FILESAVEERROR = 1033;
        public const int DTWAIN_TN_FILEPAGESAVEOK = 1034;
        public const int DTWAIN_TN_FILEPAGESAVEERROR = 1035;
        public const int DTWAIN_TN_PROCESSEDDIB = 1036;
        public const int DTWAIN_TN_FEEDERLOADED = 1037;
        public const int DTWAIN_TN_GENERALERROR = 1038;
        public const int DTWAIN_TN_MANDUPFLIPPAGES = 1040;
        public const int DTWAIN_TN_MANDUPSIDE1DONE = 1041;
        public const int DTWAIN_TN_MANDUPSIDE2DONE = 1042;
        public const int DTWAIN_TN_MANDUPPAGECOUNTERROR = 1043;
        public const int DTWAIN_TN_MANDUPACQUIREDONE = 1044;
        public const int DTWAIN_TN_MANDUPSIDE1START = 1045;
        public const int DTWAIN_TN_MANDUPSIDE2START = 1046;
        public const int DTWAIN_TN_MANDUPMERGEERROR = 1047;
        public const int DTWAIN_TN_MANDUPMEMORYERROR = 1048;
        public const int DTWAIN_TN_MANDUPFILEERROR = 1049;
        public const int DTWAIN_TN_MANDUPFILESAVEERROR = 1050;
        public const int DTWAIN_TN_ENDOFJOBDETECTED = 1051;
        public const int DTWAIN_TN_EOJDETECTED = 1051;
        public const int DTWAIN_TN_EOJDETECTED_XFERDONE = 1052;
        public const int DTWAIN_TN_QUERYPAGEDISCARD = 1053;
        public const int DTWAIN_TN_PAGEDISCARDED = 1054;
        public const int DTWAIN_TN_PROCESSDIBACCEPTED = 1055;
        public const int DTWAIN_TN_PROCESSDIBFINALACCEPTED = 1056;
        public const int DTWAIN_TN_CLOSEDIBFAILED = 1057;
        public const int DTWAIN_TN_INVALID_TWAINDSM2_BITMAP = 1058;
        public const int DTWAIN_TN_IMAGE_RESAMPLE_FAILURE = 1059;
        public const int DTWAIN_TN_DEVICEEVENT = 1100;
        public const int DTWAIN_TN_TWAINPAGECANCELLED = 1105;
        public const int DTWAIN_TN_TWAINPAGEFAILED = 1106;
        public const int DTWAIN_TN_APPUPDATEDDIB = 1107;
        public const int DTWAIN_TN_FILEPAGESAVING = 1110;
        public const int DTWAIN_TN_EOJBEGINFILESAVE = 1112;
        public const int DTWAIN_TN_EOJENDFILESAVE = 1113;
        public const int DTWAIN_TN_CROPFAILED = 1120;
        public const int DTWAIN_TN_PROCESSEDDIBFINAL = 1121;
        public const int DTWAIN_TN_BLANKPAGEDETECTED1 = 1130;
        public const int DTWAIN_TN_BLANKPAGEDETECTED2 = 1131;
        public const int DTWAIN_TN_BLANKPAGEDETECTED3 = 1132;
        public const int DTWAIN_TN_BLANKPAGEDISCARDED1 = 1133;
        public const int DTWAIN_TN_BLANKPAGEDISCARDED2 = 1134;
        public const int DTWAIN_TN_OCRTEXTRETRIEVED = 1140;
        public const int DTWAIN_TN_QUERYOCRTEXT = 1141;
        public const int DTWAIN_TN_PDFOCRREADY = 1142;
        public const int DTWAIN_TN_PDFOCRDONE = 1143;
        public const int DTWAIN_TN_PDFOCRERROR = 1144;
        public const int DTWAIN_TN_SETCALLBACKINIT = 1150;
        public const int DTWAIN_TN_SETCALLBACK64INIT = 1151;
        public const int DTWAIN_TN_FILENAMECHANGING = 1160;
        public const int DTWAIN_TN_FILENAMECHANGED = 1161;
        public const int DTWAIN_TN_PROCESSEDAUDIOFINAL = 1180;
        public const int DTWAIN_TN_PROCESSAUDIOFINALACCEPTED = 1181;
        public const int DTWAIN_TN_PROCESSEDAUDIOFILE = 1182;
        public const int DTWAIN_TN_TWAINTRIPLETBEGIN = 1183;
        public const int DTWAIN_TN_TWAINTRIPLETEND = 1184;
        public const int DTWAIN_TN_FEEDERNOTLOADED = 1201;
        public const int DTWAIN_TN_FEEDERTIMEOUT = 1202;
        public const int DTWAIN_TN_FEEDERNOTENABLED = 1203;
        public const int DTWAIN_TN_FEEDERNOTSUPPORTED = 1204;
        public const int DTWAIN_TN_FEEDERTOFLATBED = 1205;
        public const int DTWAIN_TN_PREACQUIRESTART = 1206;
        public const int DTWAIN_TN_TRANSFERTILEREADY = 1300;
        public const int DTWAIN_TN_TRANSFERTILEDONE = 1301;
        public const int DTWAIN_TN_FILECOMPRESSTYPEMISMATCH = 1302;
        public const int DTWAIN_PDFOCR_CLEANTEXT1 = 1;
        public const int DTWAIN_PDFOCR_CLEANTEXT2 = 2;
        public const int DTWAIN_MODAL = 0;
        public const int DTWAIN_MODELESS = 1;
        public const int DTWAIN_UIModeCLOSE = 0;
        public const int DTWAIN_UIModeOPEN = 1;
        public const int DTWAIN_REOPEN_SOURCE = 2;
        public const int DTWAIN_ROUNDNEAREST = 0;
        public const int DTWAIN_ROUNDUP = 1;
        public const int DTWAIN_ROUNDDOWN = 2;
        public const double DTWAIN_FLOATDELTA = (+1.0e-8);
        public const int DTWAIN_OR_ROT0 = 0;
        public const int DTWAIN_OR_ROT90 = 1;
        public const int DTWAIN_OR_ROT180 = 2;
        public const int DTWAIN_OR_ROT270 = 3;
        public const int DTWAIN_OR_PORTRAIT = DTWAIN_OR_ROT0;
        public const int DTWAIN_OR_LANDSCAPE = DTWAIN_OR_ROT270;
        public const int DTWAIN_OR_ANYROTATION = (-1);
        public const int DTWAIN_CO_GET = 0x0001;
        public const int DTWAIN_CO_SET = 0x0002;
        public const int DTWAIN_CO_GETDEFAULT = 0x0004;
        public const int DTWAIN_CO_GETCURRENT = 0x0008;
        public const int DTWAIN_CO_RESET = 0x0010;
        public const int DTWAIN_CO_SETCONSTRAINT = 0x0020;
        public const int DTWAIN_CO_CONSTRAINABLE = 0x0040;
        public const int DTWAIN_CO_GETHELP = 0x0100;
        public const int DTWAIN_CO_GETLABEL = 0x0200;
        public const int DTWAIN_CO_GETLABELENUM = 0x0400;
        public const int DTWAIN_CNTYAFGHANISTAN = 1001;
        public const int DTWAIN_CNTYALGERIA = 213;
        public const int DTWAIN_CNTYAMERICANSAMOA = 684;
        public const int DTWAIN_CNTYANDORRA = 33;
        public const int DTWAIN_CNTYANGOLA = 1002;
        public const int DTWAIN_CNTYANGUILLA = 8090;
        public const int DTWAIN_CNTYANTIGUA = 8091;
        public const int DTWAIN_CNTYARGENTINA = 54;
        public const int DTWAIN_CNTYARUBA = 297;
        public const int DTWAIN_CNTYASCENSIONI = 247;
        public const int DTWAIN_CNTYAUSTRALIA = 61;
        public const int DTWAIN_CNTYAUSTRIA = 43;
        public const int DTWAIN_CNTYBAHAMAS = 8092;
        public const int DTWAIN_CNTYBAHRAIN = 973;
        public const int DTWAIN_CNTYBANGLADESH = 880;
        public const int DTWAIN_CNTYBARBADOS = 8093;
        public const int DTWAIN_CNTYBELGIUM = 32;
        public const int DTWAIN_CNTYBELIZE = 501;
        public const int DTWAIN_CNTYBENIN = 229;
        public const int DTWAIN_CNTYBERMUDA = 8094;
        public const int DTWAIN_CNTYBHUTAN = 1003;
        public const int DTWAIN_CNTYBOLIVIA = 591;
        public const int DTWAIN_CNTYBOTSWANA = 267;
        public const int DTWAIN_CNTYBRITAIN = 6;
        public const int DTWAIN_CNTYBRITVIRGINIS = 8095;
        public const int DTWAIN_CNTYBRAZIL = 55;
        public const int DTWAIN_CNTYBRUNEI = 673;
        public const int DTWAIN_CNTYBULGARIA = 359;
        public const int DTWAIN_CNTYBURKINAFASO = 1004;
        public const int DTWAIN_CNTYBURMA = 1005;
        public const int DTWAIN_CNTYBURUNDI = 1006;
        public const int DTWAIN_CNTYCAMAROON = 237;
        public const int DTWAIN_CNTYCANADA = 2;
        public const int DTWAIN_CNTYCAPEVERDEIS = 238;
        public const int DTWAIN_CNTYCAYMANIS = 8096;
        public const int DTWAIN_CNTYCENTRALAFREP = 1007;
        public const int DTWAIN_CNTYCHAD = 1008;
        public const int DTWAIN_CNTYCHILE = 56;
        public const int DTWAIN_CNTYCHINA = 86;
        public const int DTWAIN_CNTYCHRISTMASIS = 1009;
        public const int DTWAIN_CNTYCOCOSIS = 1009;
        public const int DTWAIN_CNTYCOLOMBIA = 57;
        public const int DTWAIN_CNTYCOMOROS = 1010;
        public const int DTWAIN_CNTYCONGO = 1011;
        public const int DTWAIN_CNTYCOOKIS = 1012;
        public const int DTWAIN_CNTYCOSTARICA = 506;
        public const int DTWAIN_CNTYCUBA = 5;
        public const int DTWAIN_CNTYCYPRUS = 357;
        public const int DTWAIN_CNTYCZECHOSLOVAKIA = 42;
        public const int DTWAIN_CNTYDENMARK = 45;
        public const int DTWAIN_CNTYDJIBOUTI = 1013;
        public const int DTWAIN_CNTYDOMINICA = 8097;
        public const int DTWAIN_CNTYDOMINCANREP = 8098;
        public const int DTWAIN_CNTYEASTERIS = 1014;
        public const int DTWAIN_CNTYECUADOR = 593;
        public const int DTWAIN_CNTYEGYPT = 20;
        public const int DTWAIN_CNTYELSALVADOR = 503;
        public const int DTWAIN_CNTYEQGUINEA = 1015;
        public const int DTWAIN_CNTYETHIOPIA = 251;
        public const int DTWAIN_CNTYFALKLANDIS = 1016;
        public const int DTWAIN_CNTYFAEROEIS = 298;
        public const int DTWAIN_CNTYFIJIISLANDS = 679;
        public const int DTWAIN_CNTYFINLAND = 358;
        public const int DTWAIN_CNTYFRANCE = 33;
        public const int DTWAIN_CNTYFRANTILLES = 596;
        public const int DTWAIN_CNTYFRGUIANA = 594;
        public const int DTWAIN_CNTYFRPOLYNEISA = 689;
        public const int DTWAIN_CNTYFUTANAIS = 1043;
        public const int DTWAIN_CNTYGABON = 241;
        public const int DTWAIN_CNTYGAMBIA = 220;
        public const int DTWAIN_CNTYGERMANY = 49;
        public const int DTWAIN_CNTYGHANA = 233;
        public const int DTWAIN_CNTYGIBRALTER = 350;
        public const int DTWAIN_CNTYGREECE = 30;
        public const int DTWAIN_CNTYGREENLAND = 299;
        public const int DTWAIN_CNTYGRENADA = 8099;
        public const int DTWAIN_CNTYGRENEDINES = 8015;
        public const int DTWAIN_CNTYGUADELOUPE = 590;
        public const int DTWAIN_CNTYGUAM = 671;
        public const int DTWAIN_CNTYGUANTANAMOBAY = 5399;
        public const int DTWAIN_CNTYGUATEMALA = 502;
        public const int DTWAIN_CNTYGUINEA = 224;
        public const int DTWAIN_CNTYGUINEABISSAU = 1017;
        public const int DTWAIN_CNTYGUYANA = 592;
        public const int DTWAIN_CNTYHAITI = 509;
        public const int DTWAIN_CNTYHONDURAS = 504;
        public const int DTWAIN_CNTYHONGKONG = 852;
        public const int DTWAIN_CNTYHUNGARY = 36;
        public const int DTWAIN_CNTYICELAND = 354;
        public const int DTWAIN_CNTYINDIA = 91;
        public const int DTWAIN_CNTYINDONESIA = 62;
        public const int DTWAIN_CNTYIRAN = 98;
        public const int DTWAIN_CNTYIRAQ = 964;
        public const int DTWAIN_CNTYIRELAND = 353;
        public const int DTWAIN_CNTYISRAEL = 972;
        public const int DTWAIN_CNTYITALY = 39;
        public const int DTWAIN_CNTYIVORYCOAST = 225;
        public const int DTWAIN_CNTYJAMAICA = 8010;
        public const int DTWAIN_CNTYJAPAN = 81;
        public const int DTWAIN_CNTYJORDAN = 962;
        public const int DTWAIN_CNTYKENYA = 254;
        public const int DTWAIN_CNTYKIRIBATI = 1018;
        public const int DTWAIN_CNTYKOREA = 82;
        public const int DTWAIN_CNTYKUWAIT = 965;
        public const int DTWAIN_CNTYLAOS = 1019;
        public const int DTWAIN_CNTYLEBANON = 1020;
        public const int DTWAIN_CNTYLIBERIA = 231;
        public const int DTWAIN_CNTYLIBYA = 218;
        public const int DTWAIN_CNTYLIECHTENSTEIN = 41;
        public const int DTWAIN_CNTYLUXENBOURG = 352;
        public const int DTWAIN_CNTYMACAO = 853;
        public const int DTWAIN_CNTYMADAGASCAR = 1021;
        public const int DTWAIN_CNTYMALAWI = 265;
        public const int DTWAIN_CNTYMALAYSIA = 60;
        public const int DTWAIN_CNTYMALDIVES = 960;
        public const int DTWAIN_CNTYMALI = 1022;
        public const int DTWAIN_CNTYMALTA = 356;
        public const int DTWAIN_CNTYMARSHALLIS = 692;
        public const int DTWAIN_CNTYMAURITANIA = 1023;
        public const int DTWAIN_CNTYMAURITIUS = 230;
        public const int DTWAIN_CNTYMEXICO = 3;
        public const int DTWAIN_CNTYMICRONESIA = 691;
        public const int DTWAIN_CNTYMIQUELON = 508;
        public const int DTWAIN_CNTYMONACO = 33;
        public const int DTWAIN_CNTYMONGOLIA = 1024;
        public const int DTWAIN_CNTYMONTSERRAT = 8011;
        public const int DTWAIN_CNTYMOROCCO = 212;
        public const int DTWAIN_CNTYMOZAMBIQUE = 1025;
        public const int DTWAIN_CNTYNAMIBIA = 264;
        public const int DTWAIN_CNTYNAURU = 1026;
        public const int DTWAIN_CNTYNEPAL = 977;
        public const int DTWAIN_CNTYNETHERLANDS = 31;
        public const int DTWAIN_CNTYNETHANTILLES = 599;
        public const int DTWAIN_CNTYNEVIS = 8012;
        public const int DTWAIN_CNTYNEWCALEDONIA = 687;
        public const int DTWAIN_CNTYNEWZEALAND = 64;
        public const int DTWAIN_CNTYNICARAGUA = 505;
        public const int DTWAIN_CNTYNIGER = 227;
        public const int DTWAIN_CNTYNIGERIA = 234;
        public const int DTWAIN_CNTYNIUE = 1027;
        public const int DTWAIN_CNTYNORFOLKI = 1028;
        public const int DTWAIN_CNTYNORWAY = 47;
        public const int DTWAIN_CNTYOMAN = 968;
        public const int DTWAIN_CNTYPAKISTAN = 92;
        public const int DTWAIN_CNTYPALAU = 1029;
        public const int DTWAIN_CNTYPANAMA = 507;
        public const int DTWAIN_CNTYPARAGUAY = 595;
        public const int DTWAIN_CNTYPERU = 51;
        public const int DTWAIN_CNTYPHILLIPPINES = 63;
        public const int DTWAIN_CNTYPITCAIRNIS = 1030;
        public const int DTWAIN_CNTYPNEWGUINEA = 675;
        public const int DTWAIN_CNTYPOLAND = 48;
        public const int DTWAIN_CNTYPORTUGAL = 351;
        public const int DTWAIN_CNTYQATAR = 974;
        public const int DTWAIN_CNTYREUNIONI = 1031;
        public const int DTWAIN_CNTYROMANIA = 40;
        public const int DTWAIN_CNTYRWANDA = 250;
        public const int DTWAIN_CNTYSAIPAN = 670;
        public const int DTWAIN_CNTYSANMARINO = 39;
        public const int DTWAIN_CNTYSAOTOME = 1033;
        public const int DTWAIN_CNTYSAUDIARABIA = 966;
        public const int DTWAIN_CNTYSENEGAL = 221;
        public const int DTWAIN_CNTYSEYCHELLESIS = 1034;
        public const int DTWAIN_CNTYSIERRALEONE = 1035;
        public const int DTWAIN_CNTYSINGAPORE = 65;
        public const int DTWAIN_CNTYSOLOMONIS = 1036;
        public const int DTWAIN_CNTYSOMALI = 1037;
        public const int DTWAIN_CNTYSOUTHAFRICA = 27;
        public const int DTWAIN_CNTYSPAIN = 34;
        public const int DTWAIN_CNTYSRILANKA = 94;
        public const int DTWAIN_CNTYSTHELENA = 1032;
        public const int DTWAIN_CNTYSTKITTS = 8013;
        public const int DTWAIN_CNTYSTLUCIA = 8014;
        public const int DTWAIN_CNTYSTPIERRE = 508;
        public const int DTWAIN_CNTYSTVINCENT = 8015;
        public const int DTWAIN_CNTYSUDAN = 1038;
        public const int DTWAIN_CNTYSURINAME = 597;
        public const int DTWAIN_CNTYSWAZILAND = 268;
        public const int DTWAIN_CNTYSWEDEN = 46;
        public const int DTWAIN_CNTYSWITZERLAND = 41;
        public const int DTWAIN_CNTYSYRIA = 1039;
        public const int DTWAIN_CNTYTAIWAN = 886;
        public const int DTWAIN_CNTYTANZANIA = 255;
        public const int DTWAIN_CNTYTHAILAND = 66;
        public const int DTWAIN_CNTYTOBAGO = 8016;
        public const int DTWAIN_CNTYTOGO = 228;
        public const int DTWAIN_CNTYTONGAIS = 676;
        public const int DTWAIN_CNTYTRINIDAD = 8016;
        public const int DTWAIN_CNTYTUNISIA = 216;
        public const int DTWAIN_CNTYTURKEY = 90;
        public const int DTWAIN_CNTYTURKSCAICOS = 8017;
        public const int DTWAIN_CNTYTUVALU = 1040;
        public const int DTWAIN_CNTYUGANDA = 256;
        public const int DTWAIN_CNTYUSSR = 7;
        public const int DTWAIN_CNTYUAEMIRATES = 971;
        public const int DTWAIN_CNTYUNITEDKINGDOM = 44;
        public const int DTWAIN_CNTYUSA = 1;
        public const int DTWAIN_CNTYURUGUAY = 598;
        public const int DTWAIN_CNTYVANUATU = 1041;
        public const int DTWAIN_CNTYVATICANCITY = 39;
        public const int DTWAIN_CNTYVENEZUELA = 58;
        public const int DTWAIN_CNTYWAKE = 1042;
        public const int DTWAIN_CNTYWALLISIS = 1043;
        public const int DTWAIN_CNTYWESTERNSAHARA = 1044;
        public const int DTWAIN_CNTYWESTERNSAMOA = 1045;
        public const int DTWAIN_CNTYYEMEN = 1046;
        public const int DTWAIN_CNTYYUGOSLAVIA = 38;
        public const int DTWAIN_CNTYZAIRE = 243;
        public const int DTWAIN_CNTYZAMBIA = 260;
        public const int DTWAIN_CNTYZIMBABWE = 263;
        public const int DTWAIN_LANGDANISH = 0;
        public const int DTWAIN_LANGDUTCH = 1;
        public const int DTWAIN_LANGINTERNATIONALENGLISH = 2;
        public const int DTWAIN_LANGFRENCHCANADIAN = 3;
        public const int DTWAIN_LANGFINNISH = 4;
        public const int DTWAIN_LANGFRENCH = 5;
        public const int DTWAIN_LANGGERMAN = 6;
        public const int DTWAIN_LANGICELANDIC = 7;
        public const int DTWAIN_LANGITALIAN = 8;
        public const int DTWAIN_LANGNORWEGIAN = 9;
        public const int DTWAIN_LANGPORTUGUESE = 10;
        public const int DTWAIN_LANGSPANISH = 11;
        public const int DTWAIN_LANGSWEDISH = 12;
        public const int DTWAIN_LANGUSAENGLISH = 13;
        public const int DTWAIN_NO_ERROR = (0);
        public const int DTWAIN_ERR_FIRST = (-1000);
        public const int DTWAIN_ERR_BAD_HANDLE = (-1001);
        public const int DTWAIN_ERR_BAD_SOURCE = (-1002);
        public const int DTWAIN_ERR_BAD_ARRAY = (-1003);
        public const int DTWAIN_ERR_WRONG_ARRAY_TYPE = (-1004);
        public const int DTWAIN_ERR_INDEX_BOUNDS = (-1005);
        public const int DTWAIN_ERR_OUT_OF_MEMORY = (-1006);
        public const int DTWAIN_ERR_NULL_WINDOW = (-1007);
        public const int DTWAIN_ERR_BAD_PIXTYPE = (-1008);
        public const int DTWAIN_ERR_BAD_CONTAINER = (-1009);
        public const int DTWAIN_ERR_NO_SESSION = (-1010);
        public const int DTWAIN_ERR_BAD_ACQUIRE_NUM = (-1011);
        public const int DTWAIN_ERR_BAD_CAP = (-1012);
        public const int DTWAIN_ERR_CAP_NO_SUPPORT = (-1013);
        public const int DTWAIN_ERR_TWAIN = (-1014);
        public const int DTWAIN_ERR_HOOK_FAILED = (-1015);
        public const int DTWAIN_ERR_BAD_FILENAME = (-1016);
        public const int DTWAIN_ERR_EMPTY_ARRAY = (-1017);
        public const int DTWAIN_ERR_FILE_FORMAT = (-1018);
        public const int DTWAIN_ERR_BAD_DIB_PAGE = (-1019);
        public const int DTWAIN_ERR_SOURCE_ACQUIRING = (-1020);
        public const int DTWAIN_ERR_INVALID_PARAM = (-1021);
        public const int DTWAIN_ERR_INVALID_RANGE = (-1022);
        public const int DTWAIN_ERR_UI_ERROR = (-1023);
        public const int DTWAIN_ERR_BAD_UNIT = (-1024);
        public const int DTWAIN_ERR_LANGDLL_NOT_FOUND = (-1025);
        public const int DTWAIN_ERR_SOURCE_NOT_OPEN = (-1026);
        public const int DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED = (-1027);
        public const int DTWAIN_ERR_UIONLY_NOT_SUPPORTED = (-1028);
        public const int DTWAIN_ERR_UI_ALREADY_OPENED = (-1029);
        public const int DTWAIN_ERR_CAPSET_NOSUPPORT = (-1030);
        public const int DTWAIN_ERR_NO_FILE_XFER = (-1031);
        public const int DTWAIN_ERR_INVALID_BITDEPTH = (-1032);
        public const int DTWAIN_ERR_NO_CAPS_DEFINED = (-1033);
        public const int DTWAIN_ERR_TILES_NOT_SUPPORTED = (-1034);
        public const int DTWAIN_ERR_INVALID_DTWAIN_FRAME = (-1035);
        public const int DTWAIN_ERR_LIMITED_VERSION = (-1036);
        public const int DTWAIN_ERR_NO_FEEDER = (-1037);
        public const int DTWAIN_ERR_NO_FEEDER_QUERY = (-1038);
        public const int DTWAIN_ERR_EXCEPTION_ERROR = (-1039);
        public const int DTWAIN_ERR_INVALID_STATE = (-1040);
        public const int DTWAIN_ERR_UNSUPPORTED_EXTINFO = (-1041);
        public const int DTWAIN_ERR_DLLRESOURCE_NOTFOUND = (-1042);
        public const int DTWAIN_ERR_NOT_INITIALIZED = (-1043);
        public const int DTWAIN_ERR_NO_SOURCES = (-1044);
        public const int DTWAIN_ERR_TWAIN_NOT_INSTALLED = (-1045);
        public const int DTWAIN_ERR_WRONG_THREAD = (-1046);
        public const int DTWAIN_ERR_BAD_CAPTYPE = (-1047);
        public const int DTWAIN_ERR_UNKNOWN_CAPDATATYPE = (-1048);
        public const int DTWAIN_ERR_DEMO_NOFILETYPE = (-1049);
        public const int DTWAIN_ERR_SOURCESELECTION_CANCELED = (-1050);
        public const int DTWAIN_ERR_RESOURCES_NOT_FOUND = (-1051);
        public const int DTWAIN_ERR_STRINGTYPE_MISMATCH = (-1052);
        public const int DTWAIN_ERR_ARRAYTYPE_MISMATCH = (-1053);
        public const int DTWAIN_ERR_SOURCENAME_NOTINSTALLED = (-1054);
        public const int DTWAIN_ERR_NO_MEMFILE_XFER = (-1055);
        public const int DTWAIN_ERR_AREA_ARRAY_TOO_SMALL = (-1056);
        public const int DTWAIN_ERR_LOG_CREATE_ERROR = (-1057);
        public const int DTWAIN_ERR_FILESYSTEM_NOT_SUPPORTED = (-1058);
        public const int DTWAIN_ERR_TILEMODE_NOTSET = (-1059);
        public const int DTWAIN_ERR_INI32_NOT_FOUND = (-1060);
        public const int DTWAIN_ERR_INI64_NOT_FOUND = (-1061);
        public const int DTWAIN_ERR_CRC_CHECK = (-1062);
        public const int DTWAIN_ERR_RESOURCES_BAD_VERSION = (-1063);
        public const int DTWAIN_ERR_WIN32_ERROR = (-1064);
        public const int DTWAIN_ERR_STRINGID_NOTFOUND = (-1065);
        public const int DTWAIN_ERR_RESOURCES_DUPLICATEID_FOUND = (-1066);
        public const int DTWAIN_ERR_UNAVAILABLE_EXTINFO = (-1067);
        public const int DTWAIN_ERR_TWAINDSM2_BADBITMAP = (-1068);
        public const int DTWAIN_ERR_ACQUISITION_CANCELED = (-1069);
        public const int DTWAIN_ERR_IMAGE_RESAMPLED = (-1070);
        public const int DTWAIN_ERR_UNKNOWN_TWAIN_RC = (-1071);
        public const int DTWAIN_ERR_UNKNOWN_TWAIN_CC = (-1072);
        public const int DTWAIN_ERR_RESOURCES_DATA_EXCEPTION = (-1073);
        public const int DTWAIN_ERR_AUDIO_TRANSFER_NOTSUPPORTED = (-1074);
        public const int DTWAIN_ERR_FEEDER_COMPLIANCY = (-1075);
        public const int DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY1 = (-1076);
        public const int DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY2 = (-1077);
        public const int DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY1 = (-1078);
        public const int DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY2 = (-1079);
        public const int DTWAIN_ERR_ICAPBITDEPTH_COMPLIANCY1 = (-1080);
        public const int DTWAIN_ERR_XFERMECH_COMPLIANCY = (-1081);
        public const int DTWAIN_ERR_STANDARDCAPS_COMPLIANCY = (-1082);
        public const int DTWAIN_ERR_EXTIMAGEINFO_DATATYPE_MISMATCH = (-1083);
        public const int DTWAIN_ERR_EXTIMAGEINFO_RETRIEVAL = (-1084);
        public const int DTWAIN_ERR_RANGE_OUTOFBOUNDS = (-1085);
        public const int DTWAIN_ERR_RANGE_STEPISZERO = (-1086);
        public const int DTWAIN_ERR_BLANKNAMEDETECTED = (-1087);
        public const int DTWAIN_ERR_FEEDER_NOPAPERSENSOR = (-1088);
        public const int TWAIN_ERR_LOW_MEMORY = (-1100);
        public const int TWAIN_ERR_FALSE_ALARM = (-1101);
        public const int TWAIN_ERR_BUMMER = (-1102);
        public const int TWAIN_ERR_NODATASOURCE = (-1103);
        public const int TWAIN_ERR_MAXCONNECTIONS = (-1104);
        public const int TWAIN_ERR_OPERATIONERROR = (-1105);
        public const int TWAIN_ERR_BADCAPABILITY = (-1106);
        public const int TWAIN_ERR_BADVALUE = (-1107);
        public const int TWAIN_ERR_BADPROTOCOL = (-1108);
        public const int TWAIN_ERR_SEQUENCEERROR = (-1109);
        public const int TWAIN_ERR_BADDESTINATION = (-1110);
        public const int TWAIN_ERR_CAPNOTSUPPORTED = (-1111);
        public const int TWAIN_ERR_CAPBADOPERATION = (-1112);
        public const int TWAIN_ERR_CAPSEQUENCEERROR = (-1113);
        public const int TWAIN_ERR_FILEPROTECTEDERROR = (-1114);
        public const int TWAIN_ERR_FILEEXISTERROR = (-1115);
        public const int TWAIN_ERR_FILENOTFOUND = (-1116);
        public const int TWAIN_ERR_DIRNOTEMPTY = (-1117);
        public const int TWAIN_ERR_FEEDERJAMMED = (-1118);
        public const int TWAIN_ERR_FEEDERMULTPAGES = (-1119);
        public const int TWAIN_ERR_FEEDERWRITEERROR = (-1120);
        public const int TWAIN_ERR_DEVICEOFFLINE = (-1121);
        public const int TWAIN_ERR_NULL_CONTAINER = (-1122);
        public const int TWAIN_ERR_INTERLOCK = (-1123);
        public const int TWAIN_ERR_DAMAGEDCORNER = (-1124);
        public const int TWAIN_ERR_FOCUSERROR = (-1125);
        public const int TWAIN_ERR_DOCTOOLIGHT = (-1126);
        public const int TWAIN_ERR_DOCTOODARK = (-1127);
        public const int TWAIN_ERR_NOMEDIA = (-1128);
        public const int DTWAIN_ERR_FILEXFERSTART = (-2000);
        public const int DTWAIN_ERR_MEM = (-2001);
        public const int DTWAIN_ERR_FILEOPEN = (-2002);
        public const int DTWAIN_ERR_FILEREAD = (-2003);
        public const int DTWAIN_ERR_FILEWRITE = (-2004);
        public const int DTWAIN_ERR_BADPARAM = (-2005);
        public const int DTWAIN_ERR_INVALIDBMP = (-2006);
        public const int DTWAIN_ERR_BMPRLE = (-2007);
        public const int DTWAIN_ERR_RESERVED1 = (-2008);
        public const int DTWAIN_ERR_INVALIDJPG = (-2009);
        public const int DTWAIN_ERR_DC = (-2010);
        public const int DTWAIN_ERR_DIB = (-2011);
        public const int DTWAIN_ERR_RESERVED2 = (-2012);
        public const int DTWAIN_ERR_NORESOURCE = (-2013);
        public const int DTWAIN_ERR_CALLBACKCANCEL = (-2014);
        public const int DTWAIN_ERR_INVALIDPNG = (-2015);
        public const int DTWAIN_ERR_PNGCREATE = (-2016);
        public const int DTWAIN_ERR_INTERNAL = (-2017);
        public const int DTWAIN_ERR_FONT = (-2018);
        public const int DTWAIN_ERR_INTTIFF = (-2019);
        public const int DTWAIN_ERR_INVALIDTIFF = (-2020);
        public const int DTWAIN_ERR_NOTIFFLZW = (-2021);
        public const int DTWAIN_ERR_INVALIDPCX = (-2022);
        public const int DTWAIN_ERR_CREATEBMP = (-2023);
        public const int DTWAIN_ERR_NOLINES = (-2024);
        public const int DTWAIN_ERR_GETDIB = (-2025);
        public const int DTWAIN_ERR_NODEVOP = (-2026);
        public const int DTWAIN_ERR_INVALIDWMF = (-2027);
        public const int DTWAIN_ERR_DEPTHMISMATCH = (-2028);
        public const int DTWAIN_ERR_BITBLT = (-2029);
        public const int DTWAIN_ERR_BUFTOOSMALL = (-2030);
        public const int DTWAIN_ERR_TOOMANYCOLORS = (-2031);
        public const int DTWAIN_ERR_INVALIDTGA = (-2032);
        public const int DTWAIN_ERR_NOTGATHUMBNAIL = (-2033);
        public const int DTWAIN_ERR_RESERVED3 = (-2034);
        public const int DTWAIN_ERR_CREATEDIB = (-2035);
        public const int DTWAIN_ERR_NOLZW = (-2036);
        public const int DTWAIN_ERR_SELECTOBJ = (-2037);
        public const int DTWAIN_ERR_BADMANAGER = (-2038);
        public const int DTWAIN_ERR_OBSOLETE = (-2039);
        public const int DTWAIN_ERR_CREATEDIBSECTION = (-2040);
        public const int DTWAIN_ERR_SETWINMETAFILEBITS = (-2041);
        public const int DTWAIN_ERR_GETWINMETAFILEBITS = (-2042);
        public const int DTWAIN_ERR_PAXPWD = (-2043);
        public const int DTWAIN_ERR_INVALIDPAX = (-2044);
        public const int DTWAIN_ERR_NOSUPPORT = (-2045);
        public const int DTWAIN_ERR_INVALIDPSD = (-2046);
        public const int DTWAIN_ERR_PSDNOTSUPPORTED = (-2047);
        public const int DTWAIN_ERR_DECRYPT = (-2048);
        public const int DTWAIN_ERR_ENCRYPT = (-2049);
        public const int DTWAIN_ERR_COMPRESSION = (-2050);
        public const int DTWAIN_ERR_DECOMPRESSION = (-2051);
        public const int DTWAIN_ERR_INVALIDTLA = (-2052);
        public const int DTWAIN_ERR_INVALIDWBMP = (-2053);
        public const int DTWAIN_ERR_NOTIFFTAG = (-2054);
        public const int DTWAIN_ERR_NOLOCALSTORAGE = (-2055);
        public const int DTWAIN_ERR_INVALIDEXIF = (-2056);
        public const int DTWAIN_ERR_NOEXIFSTRING = (-2057);
        public const int DTWAIN_ERR_TIFFDLL32NOTFOUND = (-2058);
        public const int DTWAIN_ERR_TIFFDLL16NOTFOUND = (-2059);
        public const int DTWAIN_ERR_PNGDLL16NOTFOUND = (-2060);
        public const int DTWAIN_ERR_JPEGDLL16NOTFOUND = (-2061);
        public const int DTWAIN_ERR_BADBITSPERPIXEL = (-2062);
        public const int DTWAIN_ERR_TIFFDLL32INVALIDVER = (-2063);
        public const int DTWAIN_ERR_PDFDLL32NOTFOUND = (-2064);
        public const int DTWAIN_ERR_PDFDLL32INVALIDVER = (-2065);
        public const int DTWAIN_ERR_JPEGDLL32NOTFOUND = (-2066);
        public const int DTWAIN_ERR_JPEGDLL32INVALIDVER = (-2067);
        public const int DTWAIN_ERR_PNGDLL32NOTFOUND = (-2068);
        public const int DTWAIN_ERR_PNGDLL32INVALIDVER = (-2069);
        public const int DTWAIN_ERR_J2KDLL32NOTFOUND = (-2070);
        public const int DTWAIN_ERR_J2KDLL32INVALIDVER = (-2071);
        public const int DTWAIN_ERR_MANDUPLEX_UNAVAILABLE = (-2072);
        public const int DTWAIN_ERR_TIMEOUT = (-2073);
        public const int DTWAIN_ERR_INVALIDICONFORMAT = (-2074);
        public const int DTWAIN_ERR_TWAIN32DSMNOTFOUND = (-2075);
        public const int DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND = (-2076);
        public const int DTWAIN_ERR_INVALID_DIRECTORY = (-2077);
        public const int DTWAIN_ERR_CREATE_DIRECTORY = (-2078);
        public const int DTWAIN_ERR_OCRLIBRARY_NOTFOUND = (-2079);
        public const int DTWAIN_TWAINSAVE_OK = (0);
        public const int DTWAIN_ERR_TS_FIRST = (-2080);
        public const int DTWAIN_ERR_TS_NOFILENAME = (-2081);
        public const int DTWAIN_ERR_TS_NOTWAINSYS = (-2082);
        public const int DTWAIN_ERR_TS_DEVICEFAILURE = (-2083);
        public const int DTWAIN_ERR_TS_FILESAVEERROR = (-2084);
        public const int DTWAIN_ERR_TS_COMMANDILLEGAL = (-2085);
        public const int DTWAIN_ERR_TS_CANCELLED = (-2086);
        public const int DTWAIN_ERR_TS_ACQUISITIONERROR = (-2087);
        public const int DTWAIN_ERR_TS_INVALIDCOLORSPACE = (-2088);
        public const int DTWAIN_ERR_TS_PDFNOTSUPPORTED = (-2089);
        public const int DTWAIN_ERR_TS_NOTAVAILABLE = (-2090);
        public const int DTWAIN_ERR_OCR_FIRST = (-2100);
        public const int DTWAIN_ERR_OCR_INVALIDPAGENUM = (-2101);
        public const int DTWAIN_ERR_OCR_INVALIDENGINE = (-2102);
        public const int DTWAIN_ERR_OCR_NOTACTIVE = (-2103);
        public const int DTWAIN_ERR_OCR_INVALIDFILETYPE = (-2104);
        public const int DTWAIN_ERR_OCR_INVALIDPIXELTYPE = (-2105);
        public const int DTWAIN_ERR_OCR_INVALIDBITDEPTH = (-2106);
        public const int DTWAIN_ERR_OCR_RECOGNITIONERROR = (-2107);
        public const int DTWAIN_ERR_OCR_LAST = (-2108);
        public const int DTWAIN_ERR_LAST = DTWAIN_ERR_OCR_LAST;
        public const int DTWAIN_ERR_SOURCE_COULD_NOT_OPEN = (-2500);
        public const int DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE = (-2501);
        public const int DTWAIN_ERR_IMAGEINFO_INVALID = (-2502);
        public const int DTWAIN_ERR_WRITEDATA_TOFILE = (-2503);
        public const int DTWAIN_ERR_OPERATION_NOTSUPPORTED = (-2504);
        public const int DTWAIN_DE_CHKAUTOCAPTURE = 1;
        public const int DTWAIN_DE_CHKBATTERY = 2;
        public const int DTWAIN_DE_CHKDEVICEONLINE = 4;
        public const int DTWAIN_DE_CHKFLASH = 8;
        public const int DTWAIN_DE_CHKPOWERSUPPLY = 16;
        public const int DTWAIN_DE_CHKRESOLUTION = 32;
        public const int DTWAIN_DE_DEVICEADDED = 64;
        public const int DTWAIN_DE_DEVICEOFFLINE = 128;
        public const int DTWAIN_DE_DEVICEREADY = 256;
        public const int DTWAIN_DE_DEVICEREMOVED = 512;
        public const int DTWAIN_DE_IMAGECAPTURED = 1024;
        public const int DTWAIN_DE_IMAGEDELETED = 2048;
        public const int DTWAIN_DE_PAPERDOUBLEFEED = 4096;
        public const int DTWAIN_DE_PAPERJAM = 8192;
        public const int DTWAIN_DE_LAMPFAILURE = 16384;
        public const int DTWAIN_DE_POWERSAVE = 32768;
        public const int DTWAIN_DE_POWERSAVENOTIFY = 65536;
        public const int DTWAIN_DE_CUSTOMEVENTS = 0x8000;
        public const int DTWAIN_GETDE_EVENT = 0;
        public const int DTWAIN_GETDE_DEVNAME = 1;
        public const int DTWAIN_GETDE_BATTERYMINUTES = 2;
        public const int DTWAIN_GETDE_BATTERYPCT = 3;
        public const int DTWAIN_GETDE_XRESOLUTION = 4;
        public const int DTWAIN_GETDE_YRESOLUTION = 5;
        public const int DTWAIN_GETDE_FLASHUSED = 6;
        public const int DTWAIN_GETDE_AUTOCAPTURE = 7;
        public const int DTWAIN_GETDE_TIMEBEFORECAPTURE = 8;
        public const int DTWAIN_GETDE_TIMEBETWEENCAPTURES = 9;
        public const int DTWAIN_GETDE_POWERSUPPLY = 10;
        public const int DTWAIN_IMPRINTERTOPBEFORE = 1;
        public const int DTWAIN_IMPRINTERTOPAFTER = 2;
        public const int DTWAIN_IMPRINTERBOTTOMBEFORE = 4;
        public const int DTWAIN_IMPRINTERBOTTOMAFTER = 8;
        public const int DTWAIN_ENDORSERTOPBEFORE = 16;
        public const int DTWAIN_ENDORSERTOPAFTER = 32;
        public const int DTWAIN_ENDORSERBOTTOMBEFORE = 64;
        public const int DTWAIN_ENDORSERBOTTOMAFTER = 128;
        public const int DTWAIN_PM_SINGLESTRING = 0;
        public const int DTWAIN_PM_MULTISTRING = 1;
        public const int DTWAIN_PM_COMPOUNDSTRING = 2;
        public const int DTWAIN_TWTY_INT8 = 0x0000;
        public const int DTWAIN_TWTY_INT16 = 0x0001;
        public const int DTWAIN_TWTY_INT32 = 0x0002;
        public const int DTWAIN_TWTY_UINT8 = 0x0003;
        public const int DTWAIN_TWTY_UINT16 = 0x0004;
        public const int DTWAIN_TWTY_UINT32 = 0x0005;
        public const int DTWAIN_TWTY_BOOL = 0x0006;
        public const int DTWAIN_TWTY_FIX32 = 0x0007;
        public const int DTWAIN_TWTY_FRAME = 0x0008;
        public const int DTWAIN_TWTY_STR32 = 0x0009;
        public const int DTWAIN_TWTY_STR64 = 0x000A;
        public const int DTWAIN_TWTY_STR128 = 0x000B;
        public const int DTWAIN_TWTY_STR255 = 0x000C;
        public const int DTWAIN_TWTY_STR1024 = 0x000D;
        public const int DTWAIN_TWTY_UNI512 = 0x000E;
        public const int DTWAIN_EI_BARCODEX = 0x1200;
        public const int DTWAIN_EI_BARCODEY = 0x1201;
        public const int DTWAIN_EI_BARCODETEXT = 0x1202;
        public const int DTWAIN_EI_BARCODETYPE = 0x1203;
        public const int DTWAIN_EI_DESHADETOP = 0x1204;
        public const int DTWAIN_EI_DESHADELEFT = 0x1205;
        public const int DTWAIN_EI_DESHADEHEIGHT = 0x1206;
        public const int DTWAIN_EI_DESHADEWIDTH = 0x1207;
        public const int DTWAIN_EI_DESHADESIZE = 0x1208;
        public const int DTWAIN_EI_SPECKLESREMOVED = 0x1209;
        public const int DTWAIN_EI_HORZLINEXCOORD = 0x120A;
        public const int DTWAIN_EI_HORZLINEYCOORD = 0x120B;
        public const int DTWAIN_EI_HORZLINELENGTH = 0x120C;
        public const int DTWAIN_EI_HORZLINETHICKNESS = 0x120D;
        public const int DTWAIN_EI_VERTLINEXCOORD = 0x120E;
        public const int DTWAIN_EI_VERTLINEYCOORD = 0x120F;
        public const int DTWAIN_EI_VERTLINELENGTH = 0x1210;
        public const int DTWAIN_EI_VERTLINETHICKNESS = 0x1211;
        public const int DTWAIN_EI_PATCHCODE = 0x1212;
        public const int DTWAIN_EI_ENDORSEDTEXT = 0x1213;
        public const int DTWAIN_EI_FORMCONFIDENCE = 0x1214;
        public const int DTWAIN_EI_FORMTEMPLATEMATCH = 0x1215;
        public const int DTWAIN_EI_FORMTEMPLATEPAGEMATCH = 0x1216;
        public const int DTWAIN_EI_FORMHORZDOCOFFSET = 0x1217;
        public const int DTWAIN_EI_FORMVERTDOCOFFSET = 0x1218;
        public const int DTWAIN_EI_BARCODECOUNT = 0x1219;
        public const int DTWAIN_EI_BARCODECONFIDENCE = 0x121A;
        public const int DTWAIN_EI_BARCODEROTATION = 0x121B;
        public const int DTWAIN_EI_BARCODETEXTLENGTH = 0x121C;
        public const int DTWAIN_EI_DESHADECOUNT = 0x121D;
        public const int DTWAIN_EI_DESHADEBLACKCOUNTOLD = 0x121E;
        public const int DTWAIN_EI_DESHADEBLACKCOUNTNEW = 0x121F;
        public const int DTWAIN_EI_DESHADEBLACKRLMIN = 0x1220;
        public const int DTWAIN_EI_DESHADEBLACKRLMAX = 0x1221;
        public const int DTWAIN_EI_DESHADEWHITECOUNTOLD = 0x1222;
        public const int DTWAIN_EI_DESHADEWHITECOUNTNEW = 0x1223;
        public const int DTWAIN_EI_DESHADEWHITERLMIN = 0x1224;
        public const int DTWAIN_EI_DESHADEWHITERLAVE = 0x1225;
        public const int DTWAIN_EI_DESHADEWHITERLMAX = 0x1226;
        public const int DTWAIN_EI_BLACKSPECKLESREMOVED = 0x1227;
        public const int DTWAIN_EI_WHITESPECKLESREMOVED = 0x1228;
        public const int DTWAIN_EI_HORZLINECOUNT = 0x1229;
        public const int DTWAIN_EI_VERTLINECOUNT = 0x122A;
        public const int DTWAIN_EI_DESKEWSTATUS = 0x122B;
        public const int DTWAIN_EI_SKEWORIGINALANGLE = 0x122C;
        public const int DTWAIN_EI_SKEWFINALANGLE = 0x122D;
        public const int DTWAIN_EI_SKEWCONFIDENCE = 0x122E;
        public const int DTWAIN_EI_SKEWWINDOWX1 = 0x122F;
        public const int DTWAIN_EI_SKEWWINDOWY1 = 0x1230;
        public const int DTWAIN_EI_SKEWWINDOWX2 = 0x1231;
        public const int DTWAIN_EI_SKEWWINDOWY2 = 0x1232;
        public const int DTWAIN_EI_SKEWWINDOWX3 = 0x1233;
        public const int DTWAIN_EI_SKEWWINDOWY3 = 0x1234;
        public const int DTWAIN_EI_SKEWWINDOWX4 = 0x1235;
        public const int DTWAIN_EI_SKEWWINDOWY4 = 0x1236;
        public const int DTWAIN_EI_BOOKNAME = 0x1238;
        public const int DTWAIN_EI_CHAPTERNUMBER = 0x1239;
        public const int DTWAIN_EI_DOCUMENTNUMBER = 0x123A;
        public const int DTWAIN_EI_PAGENUMBER = 0x123B;
        public const int DTWAIN_EI_CAMERA = 0x123C;
        public const int DTWAIN_EI_FRAMENUMBER = 0x123D;
        public const int DTWAIN_EI_FRAME = 0x123E;
        public const int DTWAIN_EI_PIXELFLAVOR = 0x123F;
        public const int DTWAIN_EI_ICCPROFILE = 0x1240;
        public const int DTWAIN_EI_LASTSEGMENT = 0x1241;
        public const int DTWAIN_EI_SEGMENTNUMBER = 0x1242;
        public const int DTWAIN_EI_MAGDATA = 0x1243;
        public const int DTWAIN_EI_MAGTYPE = 0x1244;
        public const int DTWAIN_EI_PAGESIDE = 0x1245;
        public const int DTWAIN_EI_FILESYSTEMSOURCE = 0x1246;
        public const int DTWAIN_EI_IMAGEMERGED = 0x1247;
        public const int DTWAIN_EI_MAGDATALENGTH = 0x1248;
        public const int DTWAIN_EI_PAPERCOUNT = 0x1249;
        public const int DTWAIN_EI_PRINTERTEXT = 0x124A;
        public const int DTWAIN_EI_TWAINDIRECTMETADATA = 0x124B;
        public const int DTWAIN_EI_IAFIELDA_VALUE = 0x124C;
        public const int DTWAIN_EI_IAFIELDB_VALUE = 0x124D;
        public const int DTWAIN_EI_IAFIELDC_VALUE = 0x124E;
        public const int DTWAIN_EI_IAFIELDD_VALUE = 0x124F;
        public const int DTWAIN_EI_IAFIELDE_VALUE = 0x1250;
        public const int DTWAIN_EI_IALEVEL = 0x1251;
        public const int DTWAIN_EI_PRINTER = 0x1252;
        public const int DTWAIN_EI_BARCODETEXT2 = 0x1253;
        public const uint DTWAIN_LOG_DECODE_SOURCE = 0x1      ;
        public const uint DTWAIN_LOG_DECODE_DEST = 0x2        ;
        public const uint DTWAIN_LOG_DECODE_TWMEMREF = 0x4    ;
        public const uint DTWAIN_LOG_DECODE_TWEVENT = 0x8     ;
        public const uint DTWAIN_LOG_CALLSTACK = 0x10         ;
        public const uint DTWAIN_LOG_ISTWAINMSG = 0x20        ;
        public const uint DTWAIN_LOG_INITFAILURE = 0x40       ;
        public const uint DTWAIN_LOG_LOWLEVELTWAIN = 0x80     ;
        public const uint DTWAIN_LOG_DECODE_BITMAP = 0x100    ;
        public const uint DTWAIN_LOG_NOTIFICATIONS = 0x200    ;
        public const uint DTWAIN_LOG_MISCELLANEOUS = 0x400    ;
        public const uint DTWAIN_LOG_DTWAINERRORS = 0x800     ;
        public const uint DTWAIN_LOG_USEFILE = 0x10000        ;
        public const uint DTWAIN_LOG_SHOWEXCEPTIONS = 0x20000 ;
        public const uint DTWAIN_LOG_ERRORMSGBOX = 0x40000    ;
        public const uint DTWAIN_LOG_USEBUFFER = 0x80000      ;
        public const uint DTWAIN_LOG_FILEAPPEND = 0x100000    ;
        public const uint DTWAIN_LOG_USECALLBACK = 0x200000   ;
        public const uint DTWAIN_LOG_USECRLF = 0x400000       ;
        public const uint DTWAIN_LOG_CONSOLE = 0x800000       ;
        public const uint DTWAIN_LOG_DEBUGMONITOR = 0x1000000 ;
        public const uint DTWAIN_LOG_USEWINDOW = 0x2000000    ;
        public const uint DTWAIN_LOG_CREATEDIRECTORY = 0x04000000;
        public const uint DTWAIN_LOG_CONSOLEWITHHANDLER = (0x08000000 | DTWAIN_LOG_CONSOLE);
        public const uint DTWAIN_LOG_ALL = (DTWAIN_LOG_DECODE_SOURCE | DTWAIN_LOG_DECODE_DEST | DTWAIN_LOG_DECODE_TWEVENT | DTWAIN_LOG_DECODE_TWMEMREF | DTWAIN_LOG_CALLSTACK | DTWAIN_LOG_ISTWAINMSG | DTWAIN_LOG_INITFAILURE | DTWAIN_LOG_LOWLEVELTWAIN | DTWAIN_LOG_NOTIFICATIONS | DTWAIN_LOG_MISCELLANEOUS | DTWAIN_LOG_DTWAINERRORS | DTWAIN_LOG_DECODE_BITMAP);
        public const uint DTWAIN_LOG_ALL_APPEND = 0xFFFFFFFFU;
        public const uint DTWAIN_TEMPDIR_CREATEDIRECTORY = DTWAIN_LOG_CREATEDIRECTORY;
        public const int DTWAINGCD_RETURNHANDLE = 1;
        public const int DTWAINGCD_COPYDATA = 2;
        public const int DTWAIN_BYPOSITION = 0;
        public const int DTWAIN_BYID = 1;
        public const int DTWAINSCD_USEHANDLE = 1;
        public const int DTWAINSCD_USEDATA = 2;
        public const int DTWAIN_PAGEFAIL_RETRY = 1;
        public const int DTWAIN_PAGEFAIL_TERMINATE = 2;
        public const int DTWAIN_MAXRETRY_ATTEMPTS = 3;
        public const int DTWAIN_RETRY_FOREVER = (-1);
        public const int DTWAIN_PDF_NOSCALING = 128;
        public const int DTWAIN_PDF_FITPAGE = 256;
        public const int DTWAIN_PDF_VARIABLEPAGESIZE = 512;
        public const int DTWAIN_PDF_CUSTOMSIZE = 1024;
        public const int DTWAIN_PDF_USECOMPRESSION = 2048;
        public const int DTWAIN_PDF_CUSTOMSCALE = 4096;
        public const int DTWAIN_PDF_PIXELSPERMETERSIZE = 8192;
        public const uint DTWAIN_PDF_ALLOWPRINTING = 2052;
        public const uint DTWAIN_PDF_ALLOWMOD = 8;
        public const uint DTWAIN_PDF_ALLOWCOPY = 16;
        public const uint DTWAIN_PDF_ALLOWMODANNOTATIONS = 32;
        public const uint DTWAIN_PDF_ALLOWFILLIN = 256;
        public const uint DTWAIN_PDF_ALLOWEXTRACTION = 512;
        public const uint DTWAIN_PDF_ALLOWASSEMBLY = 1024;
        public const uint DTWAIN_PDF_ALLOWDEGRADEDPRINTING = 4;
        public const uint DTWAIN_PDF_ALLOWALL = 0xFFFFFFFCU;
        public const uint DTWAIN_PDF_ALLOWANYMOD = (DTWAIN_PDF_ALLOWMOD | DTWAIN_PDF_ALLOWFILLIN | DTWAIN_PDF_ALLOWMODANNOTATIONS | DTWAIN_PDF_ALLOWASSEMBLY);
        public const uint DTWAIN_PDF_ALLOWANYPRINTING = (DTWAIN_PDF_ALLOWPRINTING | DTWAIN_PDF_ALLOWDEGRADEDPRINTING);
        public const int DTWAIN_PDF_PORTRAIT = 0;
        public const int DTWAIN_PDF_LANDSCAPE = 1;
        public const int DTWAIN_PS_REGULAR = 0;
        public const int DTWAIN_PS_ENCAPSULATED = 1;
        public const int DTWAIN_BP_AUTODISCARD_NONE = 0;
        public const int DTWAIN_BP_AUTODISCARD_IMMEDIATE = 1;
        public const int DTWAIN_BP_AUTODISCARD_AFTERPROCESS = 2;
        public const int DTWAIN_BP_DETECTORIGINAL = 1;
        public const int DTWAIN_BP_DETECTADJUSTED = 2;
        public const int DTWAIN_BP_DETECTALL = (DTWAIN_BP_DETECTORIGINAL | DTWAIN_BP_DETECTADJUSTED);
        public const int DTWAIN_BP_DISABLE = (-2);
        public const int DTWAIN_BP_AUTO = (-1);
        public const uint DTWAIN_BP_AUTODISCARD_ANY = 0xFFFF;
        public const int DTWAIN_LP_REFLECTIVE = 0;
        public const int DTWAIN_LP_TRANSMISSIVE = 1;
        public const int DTWAIN_LS_RED = 0;
        public const int DTWAIN_LS_GREEN = 1;
        public const int DTWAIN_LS_BLUE = 2;
        public const int DTWAIN_LS_NONE = 3;
        public const int DTWAIN_LS_WHITE = 4;
        public const int DTWAIN_LS_UV = 5;
        public const int DTWAIN_LS_IR = 6;
        public const int DTWAIN_DLG_SORTNAMES = 1;
        public const int DTWAIN_DLG_CENTER = 2;
        public const int DTWAIN_DLG_CENTER_SCREEN = 4;
        public const int DTWAIN_DLG_USETEMPLATE = 8;
        public const int DTWAIN_DLG_CLEAR_PARAMS = 16;
        public const int DTWAIN_DLG_HORIZONTALSCROLL = 32;
        public const int DTWAIN_DLG_USEINCLUDENAMES = 64;
        public const int DTWAIN_DLG_USEEXCLUDENAMES = 128;
        public const int DTWAIN_DLG_USENAMEMAPPING = 256;
        public const int DTWAIN_DLG_TOPMOSTWINDOW = 1024;
        public const int DTWAIN_DLG_OPENONSELECT = 2048;
        public const int DTWAIN_DLG_NOOPENONSELECT = 4096;
        public const int DTWAIN_DLG_HIGHLIGHTFIRST = 8192;
        public const int DTWAIN_DLG_SAVELASTSCREENPOS = 16384;
        public const int DTWAIN_RES_ENGLISH = 0;
        public const int DTWAIN_RES_FRENCH = 1;
        public const int DTWAIN_RES_SPANISH = 2;
        public const int DTWAIN_RES_DUTCH = 3;
        public const int DTWAIN_RES_GERMAN = 4;
        public const int DTWAIN_RES_ITALIAN = 5;
        public const int DTWAIN_AL_ALARM = 0;
        public const int DTWAIN_AL_FEEDERERROR = 1;
        public const int DTWAIN_AL_FEEDERWARNING = 2;
        public const int DTWAIN_AL_BARCODE = 3;
        public const int DTWAIN_AL_DOUBLEFEED = 4;
        public const int DTWAIN_AL_JAM = 5;
        public const int DTWAIN_AL_PATCHCODE = 6;
        public const int DTWAIN_AL_POWER = 7;
        public const int DTWAIN_AL_SKEW = 8;
        public const int DTWAIN_FT_CAMERA = 0;
        public const int DTWAIN_FT_CAMERATOP = 1;
        public const int DTWAIN_FT_CAMERABOTTOM = 2;
        public const int DTWAIN_FT_CAMERAPREVIEW = 3;
        public const int DTWAIN_FT_DOMAIN = 4;
        public const int DTWAIN_FT_HOST = 5;
        public const int DTWAIN_FT_DIRECTORY = 6;
        public const int DTWAIN_FT_IMAGE = 7;
        public const int DTWAIN_FT_UNKNOWN = 8;
        public const int DTWAIN_NF_NONE = 0;
        public const int DTWAIN_NF_AUTO = 1;
        public const int DTWAIN_NF_LONEPIXEL = 2;
        public const int DTWAIN_NF_MAJORITYRULE = 3;
        public const int DTWAIN_CB_AUTO = 0;
        public const int DTWAIN_CB_CLEAR = 1;
        public const int DTWAIN_CB_NOCLEAR = 2;
        public const int DTWAIN_FA_NONE = 0;
        public const int DTWAIN_FA_LEFT = 1;
        public const int DTWAIN_FA_CENTER = 2;
        public const int DTWAIN_FA_RIGHT = 3;
        public const int DTWAIN_PF_CHOCOLATE = 0;
        public const int DTWAIN_PF_VANILLA = 1;
        public const int DTWAIN_FO_FIRSTPAGEFIRST = 0;
        public const int DTWAIN_FO_LASTPAGEFIRST = 1;
        public const int DTWAIN_INCREMENT_STATIC = 0;
        public const int DTWAIN_INCREMENT_DYNAMIC = 1;
        public const int DTWAIN_INCREMENT_DEFAULT = -1;
        public const int DTWAIN_MANDUP_FACEUPTOPPAGE = 0;
        public const int DTWAIN_MANDUP_FACEUPBOTTOMPAGE = 1;
        public const int DTWAIN_MANDUP_FACEDOWNTOPPAGE = 2;
        public const int DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE = 3;
        public const int DTWAIN_FILESAVE_DEFAULT = 0;
        public const int DTWAIN_FILESAVE_UICLOSE = 1;
        public const int DTWAIN_FILESAVE_SOURCECLOSE = 2;
        public const int DTWAIN_FILESAVE_ENDACQUIRE = 3;
        public const int DTWAIN_FILESAVE_MANUALSAVE = 4;
        public const int DTWAIN_FILESAVE_SAVEINCOMPLETE = 128;
        public const int DTWAIN_MANDUP_SCANOK = 1;
        public const int DTWAIN_MANDUP_SIDE1RESCAN = 2;
        public const int DTWAIN_MANDUP_SIDE2RESCAN = 3;
        public const int DTWAIN_MANDUP_RESCANALL = 4;
        public const int DTWAIN_MANDUP_PAGEMISSING = 5;
        public const int DTWAIN_DEMODLL_VERSION = 0x00000001;
        public const int DTWAIN_UNLICENSED_VERSION = 0x00000002;
        public const int DTWAIN_COMPANY_VERSION = 0x00000004;
        public const int DTWAIN_GENERAL_VERSION = 0x00000008;
        public const int DTWAIN_DEVELOP_VERSION = 0x00000010;
        public const int DTWAIN_JAVA_VERSION = 0x00000020;
        public const int DTWAIN_TOOLKIT_VERSION = 0x00000040;
        public const int DTWAIN_LIMITEDDLL_VERSION = 0x00000080;
        public const int DTWAIN_STATICLIB_VERSION = 0x00000100;
        public const int DTWAIN_STATICLIB_STDCALL_VERSION = 0x00000200;
        public const int DTWAIN_PDF_VERSION = 0x00010000;
        public const int DTWAIN_TWAINSAVE_VERSION = 0x00020000;
        public const int DTWAIN_OCR_VERSION = 0x00040000;
        public const int DTWAIN_BARCODE_VERSION = 0x00080000;
        public const int DTWAIN_ACTIVEX_VERSION = 0x00100000;
        public const int DTWAIN_32BIT_VERSION = 0x00200000;
        public const int DTWAIN_64BIT_VERSION = 0x00400000;
        public const int DTWAIN_UNICODE_VERSION = 0x00800000;
        public const int DTWAIN_OPENSOURCE_VERSION = 0x01000000;
        public const int DTWAIN_CALLSTACK_LOGGING = 0x02000000;
        public const int DTWAIN_CALLSTACK_LOGGING_PLUS = 0x04000000;
        public const int DTWAINOCR_RETURNHANDLE = 1;
        public const int DTWAINOCR_COPYDATA = 2;
        public const int DTWAIN_OCRINFO_CHAR = 0;
        public const int DTWAIN_OCRINFO_CHARXPOS = 1;
        public const int DTWAIN_OCRINFO_CHARYPOS = 2;
        public const int DTWAIN_OCRINFO_CHARXWIDTH = 3;
        public const int DTWAIN_OCRINFO_CHARYWIDTH = 4;
        public const int DTWAIN_OCRINFO_CHARCONFIDENCE = 5;
        public const int DTWAIN_OCRINFO_PAGENUM = 6;
        public const int DTWAIN_OCRINFO_OCRENGINE = 7;
        public const int DTWAIN_OCRINFO_TEXTLENGTH = 8;
        public const int DTWAIN_PDFPAGETYPE_COLOR = 0;
        public const int DTWAIN_PDFPAGETYPE_BW = 1;
        public const int DTWAIN_TWAINDSM_LEGACY = 1;
        public const int DTWAIN_TWAINDSM_VERSION2 = 2;
        public const int DTWAIN_TWAINDSM_LATESTVERSION = 4;
        public const int DTWAIN_TWAINDSMSEARCH_NOTFOUND = (-1);
        public const int DTWAIN_TWAINDSMSEARCH_WSO = 0;
        public const int DTWAIN_TWAINDSMSEARCH_WOS = 1;
        public const int DTWAIN_TWAINDSMSEARCH_SWO = 2;
        public const int DTWAIN_TWAINDSMSEARCH_SOW = 3;
        public const int DTWAIN_TWAINDSMSEARCH_OWS = 4;
        public const int DTWAIN_TWAINDSMSEARCH_OSW = 5;
        public const int DTWAIN_TWAINDSMSEARCH_W = 6;
        public const int DTWAIN_TWAINDSMSEARCH_S = 7;
        public const int DTWAIN_TWAINDSMSEARCH_O = 8;
        public const int DTWAIN_TWAINDSMSEARCH_WS = 9;
        public const int DTWAIN_TWAINDSMSEARCH_WO = 10;
        public const int DTWAIN_TWAINDSMSEARCH_SW = 11;
        public const int DTWAIN_TWAINDSMSEARCH_SO = 12;
        public const int DTWAIN_TWAINDSMSEARCH_OW = 13;
        public const int DTWAIN_TWAINDSMSEARCH_OS = 14;
        public const int DTWAIN_TWAINDSMSEARCH_C = 15;
        public const int DTWAIN_TWAINDSMSEARCH_U = 16;
        public const int DTWAIN_PDFPOLARITY_POSITIVE = 1;
        public const int DTWAIN_PDFPOLARITY_NEGATIVE = 2;
        public const int DTWAIN_TWPF_NORMAL = 0;
        public const int DTWAIN_TWPF_BOLD = 1;
        public const int DTWAIN_TWPF_ITALIC = 2;
        public const int DTWAIN_TWPF_LARGESIZE = 3;
        public const int DTWAIN_TWPF_SMALLSIZE = 4;
        public const int DTWAIN_TWCT_PAGE = 0;
        public const int DTWAIN_TWCT_PATCH1 = 1;
        public const int DTWAIN_TWCT_PATCH2 = 2;
        public const int DTWAIN_TWCT_PATCH3 = 3;
        public const int DTWAIN_TWCT_PATCH4 = 4;
        public const int DTWAIN_TWCT_PATCH5 = 5;
        public const int DTWAIN_TWCT_PATCH6 = 6;
        public const int DTWAIN_AUTOSIZE_NONE = 0;
        public const int DTWAIN_CV_CAPCUSTOMBASE = 0x8000;
        public const int DTWAIN_CV_CAPXFERCOUNT = 0x0001;
        public const int DTWAIN_CV_ICAPCOMPRESSION = 0x0100;
        public const int DTWAIN_CV_ICAPPIXELTYPE = 0x0101;
        public const int DTWAIN_CV_ICAPUNITS = 0x0102;
        public const int DTWAIN_CV_ICAPXFERMECH = 0x0103;
        public const int DTWAIN_CV_CAPAUTHOR = 0x1000;
        public const int DTWAIN_CV_CAPCAPTION = 0x1001;
        public const int DTWAIN_CV_CAPFEEDERENABLED = 0x1002;
        public const int DTWAIN_CV_CAPFEEDERLOADED = 0x1003;
        public const int DTWAIN_CV_CAPTIMEDATE = 0x1004;
        public const int DTWAIN_CV_CAPSUPPORTEDCAPS = 0x1005;
        public const int DTWAIN_CV_CAPEXTENDEDCAPS = 0x1006;
        public const int DTWAIN_CV_CAPAUTOFEED = 0x1007;
        public const int DTWAIN_CV_CAPCLEARPAGE = 0x1008;
        public const int DTWAIN_CV_CAPFEEDPAGE = 0x1009;
        public const int DTWAIN_CV_CAPREWINDPAGE = 0x100a;
        public const int DTWAIN_CV_CAPINDICATORS = 0x100b;
        public const int DTWAIN_CV_CAPSUPPORTEDCAPSEXT = 0x100c;
        public const int DTWAIN_CV_CAPPAPERDETECTABLE = 0x100d;
        public const int DTWAIN_CV_CAPUICONTROLLABLE = 0x100e;
        public const int DTWAIN_CV_CAPDEVICEONLINE = 0x100f;
        public const int DTWAIN_CV_CAPAUTOSCAN = 0x1010;
        public const int DTWAIN_CV_CAPTHUMBNAILSENABLED = 0x1011;
        public const int DTWAIN_CV_CAPDUPLEX = 0x1012;
        public const int DTWAIN_CV_CAPDUPLEXENABLED = 0x1013;
        public const int DTWAIN_CV_CAPENABLEDSUIONLY = 0x1014;
        public const int DTWAIN_CV_CAPCUSTOMDSDATA = 0x1015;
        public const int DTWAIN_CV_CAPENDORSER = 0x1016;
        public const int DTWAIN_CV_CAPJOBCONTROL = 0x1017;
        public const int DTWAIN_CV_CAPALARMS = 0x1018;
        public const int DTWAIN_CV_CAPALARMVOLUME = 0x1019;
        public const int DTWAIN_CV_CAPAUTOMATICCAPTURE = 0x101a;
        public const int DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE = 0x101b;
        public const int DTWAIN_CV_CAPTIMEBETWEENCAPTURES = 0x101c;
        public const int DTWAIN_CV_CAPCLEARBUFFERS = 0x101d;
        public const int DTWAIN_CV_CAPMAXBATCHBUFFERS = 0x101e;
        public const int DTWAIN_CV_CAPDEVICETIMEDATE = 0x101f;
        public const int DTWAIN_CV_CAPPOWERSUPPLY = 0x1020;
        public const int DTWAIN_CV_CAPCAMERAPREVIEWUI = 0x1021;
        public const int DTWAIN_CV_CAPDEVICEEVENT = 0x1022;
        public const int DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE = 0x1023;
        public const int DTWAIN_CV_CAPSERIALNUMBER = 0x1024;
        public const int DTWAIN_CV_CAPFILESYSTEM = 0x1025;
        public const int DTWAIN_CV_CAPPRINTER = 0x1026;
        public const int DTWAIN_CV_CAPPRINTERENABLED = 0x1027;
        public const int DTWAIN_CV_CAPPRINTERINDEX = 0x1028;
        public const int DTWAIN_CV_CAPPRINTERMODE = 0x1029;
        public const int DTWAIN_CV_CAPPRINTERSTRING = 0x102a;
        public const int DTWAIN_CV_CAPPRINTERSUFFIX = 0x102b;
        public const int DTWAIN_CV_CAPLANGUAGE = 0x102c;
        public const int DTWAIN_CV_CAPFEEDERALIGNMENT = 0x102d;
        public const int DTWAIN_CV_CAPFEEDERORDER = 0x102e;
        public const int DTWAIN_CV_CAPPAPERBINDING = 0x102f;
        public const int DTWAIN_CV_CAPREACQUIREALLOWED = 0x1030;
        public const int DTWAIN_CV_CAPPASSTHRU = 0x1031;
        public const int DTWAIN_CV_CAPBATTERYMINUTES = 0x1032;
        public const int DTWAIN_CV_CAPBATTERYPERCENTAGE = 0x1033;
        public const int DTWAIN_CV_CAPPOWERDOWNTIME = 0x1034;
        public const int DTWAIN_CV_CAPSEGMENTED = 0x1035;
        public const int DTWAIN_CV_CAPCAMERAENABLED = 0x1036;
        public const int DTWAIN_CV_CAPCAMERAORDER = 0x1037;
        public const int DTWAIN_CV_CAPMICRENABLED = 0x1038;
        public const int DTWAIN_CV_CAPFEEDERPREP = 0x1039;
        public const int DTWAIN_CV_CAPFEEDERPOCKET = 0x103a;
        public const int DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM = 0x103b;
        public const int DTWAIN_CV_CAPCUSTOMINTERFACEGUID = 0x103c;
        public const int DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE = 0x103d;
        public const int DTWAIN_CV_CAPSUPPORTEDDATS = 0x103e;
        public const int DTWAIN_CV_CAPDOUBLEFEEDDETECTION = 0x103f;
        public const int DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH = 0x1040;
        public const int DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY = 0x1041;
        public const int DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE = 0x1042;
        public const int DTWAIN_CV_CAPPAPERHANDLING = 0x1043;
        public const int DTWAIN_CV_CAPINDICATORSMODE = 0x1044;
        public const int DTWAIN_CV_CAPPRINTERVERTICALOFFSET = 0x1045;
        public const int DTWAIN_CV_CAPPOWERSAVETIME = 0x1046;
        public const int DTWAIN_CV_CAPPRINTERCHARROTATION = 0x1047;
        public const int DTWAIN_CV_CAPPRINTERFONTSTYLE = 0x1048;
        public const int DTWAIN_CV_CAPPRINTERINDEXLEADCHAR = 0x1049;
        public const int DTWAIN_CV_CAPIMAGEADDRESSENABLED = 0x1050;
        public const int DTWAIN_CV_CAPIAFIELDA_LEVEL = 0x1051;
        public const int DTWAIN_CV_CAPIAFIELDB_LEVEL = 0x1052;
        public const int DTWAIN_CV_CAPIAFIELDC_LEVEL = 0x1053;
        public const int DTWAIN_CV_CAPIAFIELDD_LEVEL = 0x1054;
        public const int DTWAIN_CV_CAPIAFIELDE_LEVEL = 0x1055;
        public const int DTWAIN_CV_CAPIAFIELDA_PRINTFORMAT = 0x1056;
        public const int DTWAIN_CV_CAPIAFIELDB_PRINTFORMAT = 0x1057;
        public const int DTWAIN_CV_CAPIAFIELDC_PRINTFORMAT = 0x1058;
        public const int DTWAIN_CV_CAPIAFIELDD_PRINTFORMAT = 0x1059;
        public const int DTWAIN_CV_CAPIAFIELDE_PRINTFORMAT = 0x105A;
        public const int DTWAIN_CV_CAPIAFIELDA_VALUE = 0x105B;
        public const int DTWAIN_CV_CAPIAFIELDB_VALUE = 0x105C;
        public const int DTWAIN_CV_CAPIAFIELDC_VALUE = 0x105D;
        public const int DTWAIN_CV_CAPIAFIELDD_VALUE = 0x105E;
        public const int DTWAIN_CV_CAPIAFIELDE_VALUE = 0x105F;
        public const int DTWAIN_CV_CAPIAFIELDA_LASTPAGE = 0x1060;
        public const int DTWAIN_CV_CAPIAFIELDB_LASTPAGE = 0x1061;
        public const int DTWAIN_CV_CAPIAFIELDC_LASTPAGE = 0x1062;
        public const int DTWAIN_CV_CAPIAFIELDD_LASTPAGE = 0x1063;
        public const int DTWAIN_CV_CAPIAFIELDE_LASTPAGE = 0x1064;
        public const int DTWAIN_CV_CAPPRINTERINDEXMAXVALUE = 0x104A;
        public const int DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS = 0x104B;
        public const int DTWAIN_CV_CAPPRINTERINDEXSTEP = 0x104C;
        public const int DTWAIN_CV_CAPPRINTERINDEXTRIGGER = 0x104D;
        public const int DTWAIN_CV_CAPPRINTERSTRINGPREVIEW = 0x104E;
        public const int DTWAIN_CV_ICAPAUTOBRIGHT = 0x1100;
        public const int DTWAIN_CV_ICAPBRIGHTNESS = 0x1101;
        public const int DTWAIN_CV_ICAPCONTRAST = 0x1103;
        public const int DTWAIN_CV_ICAPCUSTHALFTONE = 0x1104;
        public const int DTWAIN_CV_ICAPEXPOSURETIME = 0x1105;
        public const int DTWAIN_CV_ICAPFILTER = 0x1106;
        public const int DTWAIN_CV_ICAPFLASHUSED = 0x1107;
        public const int DTWAIN_CV_ICAPGAMMA = 0x1108;
        public const int DTWAIN_CV_ICAPHALFTONES = 0x1109;
        public const int DTWAIN_CV_ICAPHIGHLIGHT = 0x110a;
        public const int DTWAIN_CV_ICAPIMAGEFILEFORMAT = 0x110c;
        public const int DTWAIN_CV_ICAPLAMPSTATE = 0x110d;
        public const int DTWAIN_CV_ICAPLIGHTSOURCE = 0x110e;
        public const int DTWAIN_CV_ICAPORIENTATION = 0x1110;
        public const int DTWAIN_CV_ICAPPHYSICALWIDTH = 0x1111;
        public const int DTWAIN_CV_ICAPPHYSICALHEIGHT = 0x1112;
        public const int DTWAIN_CV_ICAPSHADOW = 0x1113;
        public const int DTWAIN_CV_ICAPFRAMES = 0x1114;
        public const int DTWAIN_CV_ICAPXNATIVERESOLUTION = 0x1116;
        public const int DTWAIN_CV_ICAPYNATIVERESOLUTION = 0x1117;
        public const int DTWAIN_CV_ICAPXRESOLUTION = 0x1118;
        public const int DTWAIN_CV_ICAPYRESOLUTION = 0x1119;
        public const int DTWAIN_CV_ICAPMAXFRAMES = 0x111a;
        public const int DTWAIN_CV_ICAPTILES = 0x111b;
        public const int DTWAIN_CV_ICAPBITORDER = 0x111c;
        public const int DTWAIN_CV_ICAPCCITTKFACTOR = 0x111d;
        public const int DTWAIN_CV_ICAPLIGHTPATH = 0x111e;
        public const int DTWAIN_CV_ICAPPIXELFLAVOR = 0x111f;
        public const int DTWAIN_CV_ICAPPLANARCHUNKY = 0x1120;
        public const int DTWAIN_CV_ICAPROTATION = 0x1121;
        public const int DTWAIN_CV_ICAPSUPPORTEDSIZES = 0x1122;
        public const int DTWAIN_CV_ICAPTHRESHOLD = 0x1123;
        public const int DTWAIN_CV_ICAPXSCALING = 0x1124;
        public const int DTWAIN_CV_ICAPYSCALING = 0x1125;
        public const int DTWAIN_CV_ICAPBITORDERCODES = 0x1126;
        public const int DTWAIN_CV_ICAPPIXELFLAVORCODES = 0x1127;
        public const int DTWAIN_CV_ICAPJPEGPIXELTYPE = 0x1128;
        public const int DTWAIN_CV_ICAPTIMEFILL = 0x112a;
        public const int DTWAIN_CV_ICAPBITDEPTH = 0x112b;
        public const int DTWAIN_CV_ICAPBITDEPTHREDUCTION = 0x112c;
        public const int DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE = 0x112d;
        public const int DTWAIN_CV_ICAPIMAGEDATASET = 0x112e;
        public const int DTWAIN_CV_ICAPEXTIMAGEINFO = 0x112f;
        public const int DTWAIN_CV_ICAPMINIMUMHEIGHT = 0x1130;
        public const int DTWAIN_CV_ICAPMINIMUMWIDTH = 0x1131;
        public const int DTWAIN_CV_ICAPAUTOBORDERDETECTION = 0x1132;
        public const int DTWAIN_CV_ICAPAUTODESKEW = 0x1133;
        public const int DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES = 0x1134;
        public const int DTWAIN_CV_ICAPAUTOROTATE = 0x1135;
        public const int DTWAIN_CV_ICAPFLIPROTATION = 0x1136;
        public const int DTWAIN_CV_ICAPBARCODEDETECTIONENABLED = 0x1137;
        public const int DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES = 0x1138;
        public const int DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES = 0x1139;
        public const int DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES = 0x113a;
        public const int DTWAIN_CV_ICAPBARCODESEARCHMODE = 0x113b;
        public const int DTWAIN_CV_ICAPBARCODEMAXRETRIES = 0x113c;
        public const int DTWAIN_CV_ICAPBARCODETIMEOUT = 0x113d;
        public const int DTWAIN_CV_ICAPZOOMFACTOR = 0x113e;
        public const int DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED = 0x113f;
        public const int DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES = 0x1140;
        public const int DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES = 0x1141;
        public const int DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES = 0x1142;
        public const int DTWAIN_CV_ICAPPATCHCODESEARCHMODE = 0x1143;
        public const int DTWAIN_CV_ICAPPATCHCODEMAXRETRIES = 0x1144;
        public const int DTWAIN_CV_ICAPPATCHCODETIMEOUT = 0x1145;
        public const int DTWAIN_CV_ICAPFLASHUSED2 = 0x1146;
        public const int DTWAIN_CV_ICAPIMAGEFILTER = 0x1147;
        public const int DTWAIN_CV_ICAPNOISEFILTER = 0x1148;
        public const int DTWAIN_CV_ICAPOVERSCAN = 0x1149;
        public const int DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION = 0x1150;
        public const int DTWAIN_CV_ICAPAUTOMATICDESKEW = 0x1151;
        public const int DTWAIN_CV_ICAPAUTOMATICROTATE = 0x1152;
        public const int DTWAIN_CV_ICAPJPEGQUALITY = 0x1153;
        public const int DTWAIN_CV_ICAPFEEDERTYPE = 0x1154;
        public const int DTWAIN_CV_ICAPICCPROFILE = 0x1155;
        public const int DTWAIN_CV_ICAPAUTOSIZE = 0x1156;
        public const int DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME = 0x1157;
        public const int DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION = 0x1158;
        public const int DTWAIN_CV_ICAPAUTOMATICCOLORENABLED = 0x1159;
        public const int DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE = 0x115a;
        public const int DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED = 0x115b;
        public const int DTWAIN_CV_ICAPIMAGEMERGE = 0x115c;
        public const int DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD = 0x115d;
        public const int DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO = 0x115e;
        public const int DTWAIN_CV_ICAPFILMTYPE = 0x115f;
        public const int DTWAIN_CV_ICAPMIRROR = 0x1160;
        public const int DTWAIN_CV_ICAPJPEGSUBSAMPLING = 0x1161;
        public const int DTWAIN_CV_ACAPAUDIOFILEFORMAT = 0x1201;
        public const int DTWAIN_CV_ACAPXFERMECH = 0x1202;
        public const int DTWAIN_CFMCV_CAPCFMSTART = 2048;
        public const int DTWAIN_CFMCV_CAPDUPLEXSCANNER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+10);
        public const int DTWAIN_CFMCV_CAPDUPLEXENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+11);
        public const int DTWAIN_CFMCV_CAPSCANNERNAME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+12);
        public const int DTWAIN_CFMCV_CAPSINGLEPASS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+13);
        public const int DTWAIN_CFMCV_CAPERRHANDLING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+20);
        public const int DTWAIN_CFMCV_CAPFEEDERSTATUS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+21);
        public const int DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+22);
        public const int DTWAIN_CFMCV_CAPFEEDWAITTIME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+23);
        public const int DTWAIN_CFMCV_ICAPWHITEBALANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+24);
        public const int DTWAIN_CFMCV_ICAPAUTOBINARY = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+25);
        public const int DTWAIN_CFMCV_ICAPIMAGESEPARATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+26);
        public const int DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+27);
        public const int DTWAIN_CFMCV_ICAPIMAGEEMPHASIS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+28);
        public const int DTWAIN_CFMCV_ICAPOUTLINING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+29);
        public const int DTWAIN_CFMCV_ICAPDYNTHRESHOLD = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+30);
        public const int DTWAIN_CFMCV_ICAPVARIANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+31);
        public const int DTWAIN_CFMCV_CAPENDORSERAVAILABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+32);
        public const int DTWAIN_CFMCV_CAPENDORSERENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+33);
        public const int DTWAIN_CFMCV_CAPENDORSERCHARSET = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+34);
        public const int DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+35);
        public const int DTWAIN_CFMCV_CAPENDORSERSTRING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+36);
        public const int DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+48);
        public const int DTWAIN_CFMCV_ICAPSMOOTHINGMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+49);
        public const int DTWAIN_CFMCV_ICAPFILTERMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+50);
        public const int DTWAIN_CFMCV_ICAPGRADATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+51);
        public const int DTWAIN_CFMCV_ICAPMIRROR = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+52);
        public const int DTWAIN_CFMCV_ICAPEASYSCANMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+53);
        public const int DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+54);
        public const int DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+55);
        public const int DTWAIN_CFMCV_CAPDUPLEXPAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+56);
        public const int DTWAIN_CFMCV_ICAPINVERTIMAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+57);
        public const int DTWAIN_CFMCV_ICAPSPECKLEREMOVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+58);
        public const int DTWAIN_CFMCV_ICAPUSMFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+59);
        public const int DTWAIN_CFMCV_ICAPNOISEFILTERCFM = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+60);
        public const int DTWAIN_CFMCV_ICAPDESCREENING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+61);
        public const int DTWAIN_CFMCV_ICAPQUALITYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+62);
        public const int DTWAIN_CFMCV_ICAPBINARYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+63);
        public const int DTWAIN_OCRCV_IMAGEFILEFORMAT = 0x1000;
        public const int DTWAIN_OCRCV_DESKEW = 0x1001;
        public const int DTWAIN_OCRCV_DESHADE = 0x1002;
        public const int DTWAIN_OCRCV_ORIENTATION = 0x1003;
        public const int DTWAIN_OCRCV_NOISEREMOVE = 0x1004;
        public const int DTWAIN_OCRCV_LINEREMOVE = 0x1005;
        public const int DTWAIN_OCRCV_INVERTPAGE = 0x1006;
        public const int DTWAIN_OCRCV_INVERTZONES = 0x1007;
        public const int DTWAIN_OCRCV_LINEREJECT = 0x1008;
        public const int DTWAIN_OCRCV_CHARACTERREJECT = 0x1009;
        public const int DTWAIN_OCRCV_ERRORREPORTMODE = 0x1010;
        public const int DTWAIN_OCRCV_ERRORREPORTFILE = 0x1011;
        public const int DTWAIN_OCRCV_PIXELTYPE = 0x1012;
        public const int DTWAIN_OCRCV_BITDEPTH = 0x1013;
        public const int DTWAIN_OCRCV_RETURNCHARINFO = 0x1014;
        public const int DTWAIN_OCRCV_NATIVEFILEFORMAT = 0x1015;
        public const int DTWAIN_OCRCV_MPNATIVEFILEFORMAT = 0x1016;
        public const int DTWAIN_OCRCV_SUPPORTEDCAPS = 0x1017;
        public const int DTWAIN_OCRCV_DISABLECHARACTERS = 0x1018;
        public const int DTWAIN_OCRCV_REMOVECONTROLCHARS = 0x1019;
        public const int DTWAIN_OCRORIENT_OFF = 0;
        public const int DTWAIN_OCRORIENT_AUTO = 1;
        public const int DTWAIN_OCRORIENT_90 = 2;
        public const int DTWAIN_OCRORIENT_180 = 3;
        public const int DTWAIN_OCRORIENT_270 = 4;
        public const int DTWAIN_OCRIMAGEFORMAT_AUTO = 10000;
        public const int DTWAIN_OCRERROR_MODENONE = 0;
        public const int DTWAIN_OCRERROR_SHOWMSGBOX = 1;
        public const int DTWAIN_OCRERROR_WRITEFILE = 2;
        public const int DTWAIN_PDFTEXT_ALLPAGES = 0x00000001;
        public const int DTWAIN_PDFTEXT_EVENPAGES = 0x00000002;
        public const int DTWAIN_PDFTEXT_ODDPAGES = 0x00000004;
        public const int DTWAIN_PDFTEXT_FIRSTPAGE = 0x00000008;
        public const int DTWAIN_PDFTEXT_LASTPAGE = 0x00000010;
        public const int DTWAIN_PDFTEXT_CURRENTPAGE = 0x00000020;
        public const int DTWAIN_PDFTEXT_DISABLED = 0x00000040;
        public const int DTWAIN_PDFTEXT_TOPLEFT = 0x00000100;
        public const int DTWAIN_PDFTEXT_TOPRIGHT = 0x00000200;
        public const int DTWAIN_PDFTEXT_HORIZCENTER = 0x00000400;
        public const int DTWAIN_PDFTEXT_VERTCENTER = 0x00000800;
        public const int DTWAIN_PDFTEXT_BOTTOMLEFT = 0x00001000;
        public const int DTWAIN_PDFTEXT_BOTTOMRIGHT = 0x00002000;
        public const int DTWAIN_PDFTEXT_BOTTOMCENTER = 0x00004000;
        public const int DTWAIN_PDFTEXT_TOPCENTER = 0x00008000;
        public const int DTWAIN_PDFTEXT_XCENTER = 0x00010000;
        public const int DTWAIN_PDFTEXT_YCENTER = 0x00020000;
        public const int DTWAIN_PDFTEXT_NOSCALING = 0x00100000;
        public const int DTWAIN_PDFTEXT_NOCHARSPACING = 0x00200000;
        public const int DTWAIN_PDFTEXT_NOWORDSPACING = 0x00400000;
        public const int DTWAIN_PDFTEXT_NOSTROKEWIDTH = 0x00800000;
        public const int DTWAIN_PDFTEXT_NORENDERMODE = 0x01000000;
        public const int DTWAIN_PDFTEXT_NORGBCOLOR = 0x02000000;
        public const int DTWAIN_PDFTEXT_NOFONTSIZE = 0x04000000;
        public const int DTWAIN_PDFTEXT_NOABSPOSITION = 0x08000000;
        public const int DTWAIN_PDFTEXT_NOROTATION = 0x10000000;
        public const int DTWAIN_PDFTEXT_NOSKEWING = 0x20000000;
        public const int DTWAIN_PDFTEXT_NOSCALINGXY = 0x40000000;
        public const uint DTWAIN_PDFTEXT_IGNOREALL = 0xFFF00000U;
        public const int DTWAIN_FONT_COURIER = 0;
        public const int DTWAIN_FONT_COURIERBOLD = 1;
        public const int DTWAIN_FONT_COURIERBOLDOBLIQUE = 2;
        public const int DTWAIN_FONT_COURIEROBLIQUE = 3;
        public const int DTWAIN_FONT_HELVETICA = 4;
        public const int DTWAIN_FONT_HELVETICABOLD = 5;
        public const int DTWAIN_FONT_HELVETICABOLDOBLIQUE = 6;
        public const int DTWAIN_FONT_HELVETICAOBLIQUE = 7;
        public const int DTWAIN_FONT_TIMESBOLD = 8;
        public const int DTWAIN_FONT_TIMESBOLDITALIC = 9;
        public const int DTWAIN_FONT_TIMESROMAN = 10;
        public const int DTWAIN_FONT_TIMESITALIC = 11;
        public const int DTWAIN_FONT_SYMBOL = 12;
        public const int DTWAIN_FONT_ZAPFDINGBATS = 13;
        public const int DTWAIN_PDFRENDER_FILL = 0;
        public const int DTWAIN_PDFRENDER_STROKE = 1;
        public const int DTWAIN_PDFRENDER_FILLSTROKE = 2;
        public const int DTWAIN_PDFRENDER_INVISIBLE = 3;
        public const int DTWAIN_PDFTEXTELEMENT_SCALINGXY = 0;
        public const int DTWAIN_PDFTEXTELEMENT_FONTHEIGHT = 1;
        public const int DTWAIN_PDFTEXTELEMENT_WORDSPACING = 2;
        public const int DTWAIN_PDFTEXTELEMENT_POSITION = 3;
        public const int DTWAIN_PDFTEXTELEMENT_COLOR = 4;
        public const int DTWAIN_PDFTEXTELEMENT_STROKEWIDTH = 5;
        public const int DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS = 6;
        public const int DTWAIN_PDFTEXTELEMENT_FONTNAME = 7;
        public const int DTWAIN_PDFTEXTELEMENT_TEXT = 8;
        public const int DTWAIN_PDFTEXTELEMENT_RENDERMODE = 9;
        public const int DTWAIN_PDFTEXTELEMENT_CHARSPACING = 10;
        public const int DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE = 11;
        public const int DTWAIN_PDFTEXTELEMENT_LEADING = 12;
        public const int DTWAIN_PDFTEXTELEMENT_SCALING = 13;
        public const int DTWAIN_PDFTEXTELEMENT_TEXTLENGTH = 14;
        public const int DTWAIN_PDFTEXTELEMENT_SKEWANGLES = 15;
        public const int DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER = 16;
        public const int DTWAIN_PDFTEXTTRANSFORM_SRK = 0;
        public const int DTWAIN_PDFTEXTTRANSFORM_SKR = 1;
        public const int DTWAIN_PDFTEXTTRANSFORM_KSR = 2;
        public const int DTWAIN_PDFTEXTTRANSFORM_KRS = 3;
        public const int DTWAIN_PDFTEXTTRANSFORM_RSK = 4;
        public const int DTWAIN_PDFTEXTTRANSFORM_RKS = 5;
        public const int DTWAIN_PDFTEXTTRANFORM_LAST = DTWAIN_PDFTEXTTRANSFORM_RKS;
        public const int DTWAIN_TWDF_ULTRASONIC = 0;
        public const int DTWAIN_TWDF_BYLENGTH = 1;
        public const int DTWAIN_TWDF_INFRARED = 2;
        public const int DTWAIN_TWAS_NONE = 0;
        public const int DTWAIN_TWAS_AUTO = 1;
        public const int DTWAIN_TWAS_CURRENT = 2;
        public const int DTWAIN_TWFR_BOOK = 0;
        public const int DTWAIN_TWFR_FANFOLD = 1;
        public const int DTWAIN_CONSTANT_TWPT = 0 ;
        public const int DTWAIN_CONSTANT_TWUN = 1 ;
        public const int DTWAIN_CONSTANT_TWCY = 2 ;
        public const int DTWAIN_CONSTANT_TWAL = 3 ;
        public const int DTWAIN_CONSTANT_TWAS = 4 ;
        public const int DTWAIN_CONSTANT_TWBCOR = 5 ;
        public const int DTWAIN_CONSTANT_TWBD = 6 ;
        public const int DTWAIN_CONSTANT_TWBO = 7 ;
        public const int DTWAIN_CONSTANT_TWBP = 8 ;
        public const int DTWAIN_CONSTANT_TWBR = 9 ;
        public const int DTWAIN_CONSTANT_TWBT = 10;
        public const int DTWAIN_CONSTANT_TWCP = 11;
        public const int DTWAIN_CONSTANT_TWCS = 12;
        public const int DTWAIN_CONSTANT_TWDE = 13;
        public const int DTWAIN_CONSTANT_TWDR = 14;
        public const int DTWAIN_CONSTANT_TWDSK = 15;
        public const int DTWAIN_CONSTANT_TWDX = 16;
        public const int DTWAIN_CONSTANT_TWFA = 17;
        public const int DTWAIN_CONSTANT_TWFE = 18;
        public const int DTWAIN_CONSTANT_TWFF = 19;
        public const int DTWAIN_CONSTANT_TWFL = 20;
        public const int DTWAIN_CONSTANT_TWFO = 21;
        public const int DTWAIN_CONSTANT_TWFP = 22;
        public const int DTWAIN_CONSTANT_TWFR = 23;
        public const int DTWAIN_CONSTANT_TWFT = 24;
        public const int DTWAIN_CONSTANT_TWFY = 25;
        public const int DTWAIN_CONSTANT_TWIA = 26;
        public const int DTWAIN_CONSTANT_TWIC = 27;
        public const int DTWAIN_CONSTANT_TWIF = 28;
        public const int DTWAIN_CONSTANT_TWIM = 29;
        public const int DTWAIN_CONSTANT_TWJC = 30;
        public const int DTWAIN_CONSTANT_TWJQ = 31;
        public const int DTWAIN_CONSTANT_TWLP = 32;
        public const int DTWAIN_CONSTANT_TWLS = 33;
        public const int DTWAIN_CONSTANT_TWMD = 34;
        public const int DTWAIN_CONSTANT_TWNF = 35;
        public const int DTWAIN_CONSTANT_TWOR = 36;
        public const int DTWAIN_CONSTANT_TWOV = 37;
        public const int DTWAIN_CONSTANT_TWPA = 38;
        public const int DTWAIN_CONSTANT_TWPC = 39;
        public const int DTWAIN_CONSTANT_TWPCH = 40;
        public const int DTWAIN_CONSTANT_TWPF = 41;
        public const int DTWAIN_CONSTANT_TWPM = 42;
        public const int DTWAIN_CONSTANT_TWPR = 43;
        public const int DTWAIN_CONSTANT_TWPF2 = 44;
        public const int DTWAIN_CONSTANT_TWCT = 45;
        public const int DTWAIN_CONSTANT_TWPS = 46;
        public const int DTWAIN_CONSTANT_TWSS = 47;
        public const int DTWAIN_CONSTANT_TWPH = 48;
        public const int DTWAIN_CONSTANT_TWCI = 49;
        public const int DTWAIN_CONSTANT_FONTNAME = 50;
        public const int DTWAIN_CONSTANT_TWEI = 51;
        public const int DTWAIN_CONSTANT_TWEJ = 52;
        public const int DTWAIN_CONSTANT_TWCC = 53;
        public const int DTWAIN_CONSTANT_TWQC = 54;
        public const int DTWAIN_CONSTANT_TWRC = 55;
        public const int DTWAIN_CONSTANT_MSG = 56;
        public const int DTWAIN_CONSTANT_TWLG = 57;
        public const int DTWAIN_CONSTANT_DLLINFO = 58;
        public const int DTWAIN_CONSTANT_DG = 59;
        public const int DTWAIN_CONSTANT_DAT = 60;
        public const int DTWAIN_CONSTANT_DF = 61;
        public const int DTWAIN_CONSTANT_TWTY = 62;
        public const int DTWAIN_CONSTANT_TWCB = 63;
        public const int DTWAIN_CONSTANT_TWAF = 64;
        public const int DTWAIN_CONSTANT_TWFS = 65;
        public const int DTWAIN_CONSTANT_TWJS = 66;
        public const int DTWAIN_CONSTANT_TWMR = 67;
        public const int DTWAIN_CONSTANT_TWDP = 68;
        public const int DTWAIN_CONSTANT_TWUS = 69;
        public const int DTWAIN_CONSTANT_TWDF = 70;
        public const int DTWAIN_CONSTANT_TWFM = 71;
        public const int DTWAIN_CONSTANT_TWSG = 72;
        public const int DTWAIN_CONSTANT_DTWAIN_TN = 73;
        public const int DTWAIN_CONSTANT_TWON = 74;
        public const int DTWAIN_CONSTANT_TWMF = 75;
        public const int DTWAIN_CONSTANT_TWSX = 76;
        public const int DTWAIN_CONSTANT_CAP = 77;
        public const int DTWAIN_CONSTANT_ICAP = 78;
        public const int DTWAIN_CONSTANT_DTWAIN_CONT = 79;
        public const int DTWAIN_CONSTANT_CAPCODE_MAP = 80;
        public const int DTWAIN_CONSTANT_ACAP = 81;
        public const int DTWAIN_USERRES_START = 20000;
        public const int DTWAIN_USERRES_MAXSIZE = 8192;
        public const int DTWAIN_APIHANDLEOK = 1;
        public const int DTWAIN_TWAINSESSIONOK = 2;
        public const int DTWAIN_PDF_AES128 = 1;
        public const int DTWAIN_PDF_AES256 = 2;
        public const int DTWAIN_FEEDER_TERMINATE = 1;
        public const int DTWAIN_FEEDER_USEFLATBED = 2;
        // string type constants
        // these include room for the strings and a null char
        public enum TWSTR : int
        {
            STR32 = 34,
            STR64 = 66,
            STR128 = 130,
            STR255 = 256,
            STR1024 = 1026,
            UNI512 = 512
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_VERSION
        {
            public TW_UINT16 MajorNum;                          // Major revision number of the software
            public TW_UINT16 MinorNum;                          // Incremental revision number of the software
            public TW_UINT16 Language;                          // e.g. TWLG_SWISSFRENCH
            public TW_UINT16 Country;                           // e.g. TWCY_SWITZERLAND

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)TWSTR.STR32)]
            public string Info;                                 // e.g. "1.0b3 Beta release"
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class TW_IDENTITY
        {
            public TW_UINT32 Id;                                // Unique number.  In Windows, application hWnd
            public TW_VERSION Version;                          // Identifies the piece of code
            public TW_UINT16 ProtocolMajor;                     // Application and DS must set to TWON_PROTOCOLMAJOR
            public TW_UINT16 ProtocolMinor;                     // Application and DS must set to TWON_PROTOCOLMINOR
            public TW_UINT32 SupportedGroups;                   // Bit field OR combination of DG_ constants

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)TWSTR.STR32)]
            public string Manufacturer = string.Empty;                         // Manufacturer name, e.g. "Hewlett-Packard"

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)TWSTR.STR32)]
            public string ProductFamily = string.Empty;                        // Product family name, e.g. "ScanJet"

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)TWSTR.STR32)]
            public string ProductName = string.Empty;                          // Product name, e.g. "ScanJet Plus"
        }

        public struct POINT
        {
            public System.Int32 x;
            public System.Int32 Y;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct MSG
        {
            public System.IntPtr hwnd;
            public System.UInt32 message;
            public System.UIntPtr wParam;
            public System.UIntPtr lParam;
            public System.UInt32 time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OpenFileName
        {
            public int structSize = 0;
            public System.IntPtr dlgOwner = System.IntPtr.Zero;
            public System.IntPtr instance = System.IntPtr.Zero;
            public string filter = string.Empty;
            public string customFilter = string.Empty;
            public int maxCustFilter = 0;
            public int filterIndex = 0;
            public string file = string.Empty;
            public int maxFile = 0;
            public string fileTitle = string.Empty;
            public int maxFileTitle = 0;
            public string initialDir = string.Empty;
            public string title = string.Empty;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;
            public string defExt = string.Empty;
            public System.IntPtr custData = System.IntPtr.Zero;
            public System.IntPtr hook = System.IntPtr.Zero;
            public string templateName = string.Empty;
            public System.IntPtr reservedPtr = System.IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }

        public delegate nint DTwainCallback(nint wParam, nint lParam, nint UserData);
        public delegate nint DTwainCallback64(nint wParam, nint lParam, long UserData);
        public delegate nint DTwainErrorProc(int param1, int param2);
        public delegate nint DTwainErrorProc64(int param1, long param2);
        public delegate nint DTwainLoggerProcA([MarshalAs(UnmanagedType.LPStr)] string lpszName, long UserData);
        public delegate nint DTwainLoggerProcW([MarshalAs(UnmanagedType.LPWStr)] string lpszName, long UserData);
        public delegate DTWAIN_HANDLE DTwainDIBUpdateProc(DTWAIN_SOURCE source, int currentImage, DTWAIN_HANDLE DibData);
        public delegate nint DTwainLoggerProc([MarshalAs(UnmanagedType.LPTStr)] string lpszName, long UserData);

        public delegate int DTWAIN_AcquireAudioFileDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int lFileFlags, int lMaxClips, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireAudioFileADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpszFile, int lFileFlags, int lNumClips, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireAudioFileWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpszFile, int lFileFlags, int lNumClips, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate DTWAIN_ARRAY DTWAIN_AcquireAudioNativeDelegate(DTWAIN_SOURCE Source, int nMaxAudioClips, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireAudioNativeExDelegate(DTWAIN_SOURCE Source, int nMaxAudioClips, int bShowUI, int bCloseSource, DTWAIN_ARRAY Acquisitions, ref int pStatus);
        public delegate DTWAIN_ARRAY DTWAIN_AcquireBufferedDelegate(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireBufferedExDelegate(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, DTWAIN_ARRAY Acquisitions, ref int pStatus);
        public delegate int DTWAIN_AcquireFileDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireFileADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpszFile, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireFileExDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY aFileNames, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireFileWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpszFile, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate DTWAIN_ARRAY DTWAIN_AcquireNativeDelegate(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AcquireNativeExDelegate(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, DTWAIN_ARRAY Acquisitions, ref int pStatus);
        public delegate DTWAIN_ARRAY DTWAIN_AcquireToClipboardDelegate(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int nTransferMode, int bDiscardDibs, int bShowUI, int bCloseSource, ref int pStatus);
        public delegate int DTWAIN_AddExtImageInfoQueryDelegate(DTWAIN_SOURCE Source, int ExtImageInfo);
        public delegate int DTWAIN_AddFileToAppendDelegate([MarshalAs(UnmanagedType.LPTStr)] string szFile);
        public delegate int DTWAIN_AddFileToAppendADelegate([MarshalAs(UnmanagedType.LPStr)] string szFile);
        public delegate int DTWAIN_AddFileToAppendWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szFile);
        public delegate int DTWAIN_AddPDFTextDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string fontName, DTWAIN_FLOAT fontSize, int colorRGB, int renderMode, DTWAIN_FLOAT scaling, DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing, DTWAIN_FLOAT strokeWidth, uint Flags);
        public delegate int DTWAIN_AddPDFTextADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string fontName, DTWAIN_FLOAT fontSize, int colorRGB, int renderMode, DTWAIN_FLOAT scaling, DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing, DTWAIN_FLOAT strokeWidth, uint Flags);
        public delegate int DTWAIN_AddPDFTextElementDelegate(DTWAIN_SOURCE Source, DTWAIN_PDFTEXTELEMENT TextElement);
        public delegate int DTWAIN_AddPDFTextStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string fontName, [MarshalAs(UnmanagedType.LPTStr)] string fontSize, int colorRGB, int renderMode, [MarshalAs(UnmanagedType.LPTStr)] string scaling, [MarshalAs(UnmanagedType.LPTStr)] string charSpacing, [MarshalAs(UnmanagedType.LPTStr)] string wordSpacing, [MarshalAs(UnmanagedType.LPTStr)] string strokeWidth, uint Flags);
        public delegate int DTWAIN_AddPDFTextStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string fontName, [MarshalAs(UnmanagedType.LPStr)] string fontSize, int colorRGB, int renderMode, [MarshalAs(UnmanagedType.LPStr)] string scaling, [MarshalAs(UnmanagedType.LPStr)] string charSpacing, [MarshalAs(UnmanagedType.LPStr)] string wordSpacing, [MarshalAs(UnmanagedType.LPStr)] string strokeWidth, uint Flags);
        public delegate int DTWAIN_AddPDFTextStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string fontName, [MarshalAs(UnmanagedType.LPWStr)] string fontSize, int colorRGB, int renderMode, [MarshalAs(UnmanagedType.LPWStr)] string scaling, [MarshalAs(UnmanagedType.LPWStr)] string charSpacing, [MarshalAs(UnmanagedType.LPWStr)] string wordSpacing, [MarshalAs(UnmanagedType.LPWStr)] string strokeWidth, uint Flags);
        public delegate int DTWAIN_AddPDFTextWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string fontName, DTWAIN_FLOAT fontSize, int colorRGB, int renderMode, DTWAIN_FLOAT scaling, DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing, DTWAIN_FLOAT strokeWidth, uint Flags);
        public delegate HANDLE DTWAIN_AllocateMemoryDelegate(uint memSize);
        public delegate HANDLE DTWAIN_AllocateMemory64Delegate(ULONG64 memSize);
        public delegate HANDLE DTWAIN_AllocateMemoryExDelegate(uint memSize);
        public delegate int DTWAIN_AppHandlesExceptionsDelegate(int bSet);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayANSIStringToFloatDelegate(DTWAIN_ARRAY StringArray);
        public delegate int DTWAIN_ArrayAddDelegate(DTWAIN_ARRAY pArray, System.IntPtr pVariant);
        public delegate int DTWAIN_ArrayAddANSIStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val);
        public delegate int DTWAIN_ArrayAddANSIStringNDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, int num);
        public delegate int DTWAIN_ArrayAddFloatDelegate(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val);
        public delegate int DTWAIN_ArrayAddFloatNDelegate(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, int num);
        public delegate int DTWAIN_ArrayAddFloatStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val);
        public delegate int DTWAIN_ArrayAddFloatStringADelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val);
        public delegate int DTWAIN_ArrayAddFloatStringNDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num);
        public delegate int DTWAIN_ArrayAddFloatStringNADelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, int num);
        public delegate int DTWAIN_ArrayAddFloatStringNWDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num);
        public delegate int DTWAIN_ArrayAddFloatStringWDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val);
        public delegate int DTWAIN_ArrayAddFrameDelegate(DTWAIN_ARRAY pArray, DTWAIN_FRAME frame);
        public delegate int DTWAIN_ArrayAddFrameNDelegate(DTWAIN_ARRAY pArray, DTWAIN_FRAME frame, int num);
        public delegate int DTWAIN_ArrayAddLongDelegate(DTWAIN_ARRAY pArray, int Val);
        public delegate int DTWAIN_ArrayAddLong64Delegate(DTWAIN_ARRAY pArray, LONG64 Val);
        public delegate int DTWAIN_ArrayAddLong64NDelegate(DTWAIN_ARRAY pArray, LONG64 Val, int num);
        public delegate int DTWAIN_ArrayAddLongNDelegate(DTWAIN_ARRAY pArray, int Val, int num);
        public delegate int DTWAIN_ArrayAddNDelegate(DTWAIN_ARRAY pArray, System.IntPtr pVariant, int num);
        public delegate int DTWAIN_ArrayAddStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val);
        public delegate int DTWAIN_ArrayAddStringADelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val);
        public delegate int DTWAIN_ArrayAddStringNDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num);
        public delegate int DTWAIN_ArrayAddStringNADelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, int num);
        public delegate int DTWAIN_ArrayAddStringNWDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num);
        public delegate int DTWAIN_ArrayAddStringWDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val);
        public delegate int DTWAIN_ArrayAddWideStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val);
        public delegate int DTWAIN_ArrayAddWideStringNDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayConvertFix32ToFloatDelegate(DTWAIN_ARRAY Fix32Array);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayConvertFloatToFix32Delegate(DTWAIN_ARRAY FloatArray);
        public delegate int DTWAIN_ArrayCopyDelegate(DTWAIN_ARRAY Source, DTWAIN_ARRAY Dest);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayCreateDelegate(int nEnumType, int nInitialSize);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayCreateCopyDelegate(DTWAIN_ARRAY Source);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayCreateFromCapDelegate(DTWAIN_SOURCE Source, int lCapType, int lSize);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayCreateFromLong64sDelegate(ref long pCArray, int nSize);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayCreateFromLongsDelegate(ref int pCArray, int nSize);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayCreateFromRealsDelegate(ref DTWAIN_FLOAT pCArray, int nSize);
        public delegate int DTWAIN_ArrayDestroyDelegate(DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_ArrayDestroyFramesDelegate(DTWAIN_ARRAY FrameArray);
        public delegate int DTWAIN_ArrayFindDelegate(DTWAIN_ARRAY pArray, System.IntPtr pVariant);
        public delegate int DTWAIN_ArrayFindANSIStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string pString);
        public delegate int DTWAIN_ArrayFindFloatDelegate(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, DTWAIN_FLOAT Tolerance);
        public delegate int DTWAIN_ArrayFindFloatStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, [MarshalAs(UnmanagedType.LPTStr)] string Tolerance);
        public delegate int DTWAIN_ArrayFindFloatStringADelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, [MarshalAs(UnmanagedType.LPStr)] string Tolerance);
        public delegate int DTWAIN_ArrayFindFloatStringWDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, [MarshalAs(UnmanagedType.LPWStr)] string Tolerance);
        public delegate int DTWAIN_ArrayFindLongDelegate(DTWAIN_ARRAY pArray, int Val);
        public delegate int DTWAIN_ArrayFindLong64Delegate(DTWAIN_ARRAY pArray, LONG64 Val);
        public delegate int DTWAIN_ArrayFindStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string pString);
        public delegate int DTWAIN_ArrayFindStringADelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string pString);
        public delegate int DTWAIN_ArrayFindStringWDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string pString);
        public delegate int DTWAIN_ArrayFindWideStringDelegate(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string pString);
        public delegate int DTWAIN_ArrayFix32GetAtDelegate(DTWAIN_ARRAY aFix32, int lPos, ref int Whole, ref int Frac);
        public delegate int DTWAIN_ArrayFix32SetAtDelegate(DTWAIN_ARRAY aFix32, int lPos, int Whole, int Frac);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayFloatToANSIStringDelegate(DTWAIN_ARRAY FloatArray);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayFloatToStringDelegate(DTWAIN_ARRAY FloatArray);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayFloatToWideStringDelegate(DTWAIN_ARRAY FloatArray);
        public delegate int DTWAIN_ArrayGetAtDelegate(DTWAIN_ARRAY pArray, int nWhere, System.IntPtr pVariant);
        public delegate int DTWAIN_ArrayGetAtANSIStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder pStr);
        public delegate int DTWAIN_ArrayGetAtFloatDelegate(DTWAIN_ARRAY pArray, int nWhere, ref DTWAIN_FLOAT pVal);
        public delegate int DTWAIN_ArrayGetAtFloatStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Val);
        public delegate int DTWAIN_ArrayGetAtFloatStringADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Val);
        public delegate int DTWAIN_ArrayGetAtFloatStringWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Val);
        public delegate int DTWAIN_ArrayGetAtFrameDelegate(DTWAIN_ARRAY FrameArray, int nWhere, ref DTWAIN_FLOAT pleft, ref DTWAIN_FLOAT ptop, ref DTWAIN_FLOAT pright, ref DTWAIN_FLOAT pbottom);
        public delegate int DTWAIN_ArrayGetAtFrameExDelegate(DTWAIN_ARRAY FrameArray, int nWhere, DTWAIN_FRAME Frame);
        public delegate int DTWAIN_ArrayGetAtFrameStringDelegate(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder bottom);
        public delegate int DTWAIN_ArrayGetAtFrameStringADelegate(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder bottom);
        public delegate int DTWAIN_ArrayGetAtFrameStringWDelegate(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder bottom);
        public delegate int DTWAIN_ArrayGetAtLongDelegate(DTWAIN_ARRAY pArray, int nWhere, ref int pVal);
        public delegate int DTWAIN_ArrayGetAtLong64Delegate(DTWAIN_ARRAY pArray, int nWhere, ref long pVal);
        public delegate int DTWAIN_ArrayGetAtSourceDelegate(DTWAIN_ARRAY pArray, int nWhere, ref DTWAIN_SOURCE ppSource);
        public delegate DTWAIN_SOURCE DTWAIN_ArrayGetAtSourceExDelegate(DTWAIN_ARRAY pArray, int nWhere);
        public delegate int DTWAIN_ArrayGetAtStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pStr);
        public delegate int DTWAIN_ArrayGetAtStringADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder pStr);
        public delegate int DTWAIN_ArrayGetAtStringWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pStr);
        public delegate int DTWAIN_ArrayGetAtWideStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pStr);
        public delegate System.IntPtr DTWAIN_ArrayGetBufferDelegate(DTWAIN_ARRAY pArray, int nPos);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayGetCapValuesDelegate(DTWAIN_SOURCE Source, int lCap, int lGetType);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayGetCapValuesExDelegate(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayGetCapValuesEx2Delegate(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType, int nDataType);
        public delegate int DTWAIN_ArrayGetCountDelegate(DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_ArrayGetMaxStringLengthDelegate(DTWAIN_ARRAY a);
        public delegate int DTWAIN_ArrayGetSourceAtDelegate(DTWAIN_ARRAY pArray, int nWhere, ref DTWAIN_SOURCE ppSource);
        public delegate int DTWAIN_ArrayGetStringLengthDelegate(DTWAIN_ARRAY a, int nWhichString);
        public delegate int DTWAIN_ArrayGetTypeDelegate(DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayInitDelegate();
        public delegate int DTWAIN_ArrayInsertAtDelegate(DTWAIN_ARRAY pArray, int nWhere, System.IntPtr pVariant);
        public delegate int DTWAIN_ArrayInsertAtANSIStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pVal);
        public delegate int DTWAIN_ArrayInsertAtANSIStringNDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val, int num);
        public delegate int DTWAIN_ArrayInsertAtFloatDelegate(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FLOAT pVal);
        public delegate int DTWAIN_ArrayInsertAtFloatNDelegate(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FLOAT Val, int num);
        public delegate int DTWAIN_ArrayInsertAtFloatStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val);
        public delegate int DTWAIN_ArrayInsertAtFloatStringADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val);
        public delegate int DTWAIN_ArrayInsertAtFloatStringNDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num);
        public delegate int DTWAIN_ArrayInsertAtFloatStringNADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val, int num);
        public delegate int DTWAIN_ArrayInsertAtFloatStringNWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num);
        public delegate int DTWAIN_ArrayInsertAtFloatStringWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val);
        public delegate int DTWAIN_ArrayInsertAtFrameDelegate(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FRAME frame);
        public delegate int DTWAIN_ArrayInsertAtFrameNDelegate(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FRAME frame, int num);
        public delegate int DTWAIN_ArrayInsertAtLongDelegate(DTWAIN_ARRAY pArray, int nWhere, int pVal);
        public delegate int DTWAIN_ArrayInsertAtLong64Delegate(DTWAIN_ARRAY pArray, int nWhere, LONG64 Val);
        public delegate int DTWAIN_ArrayInsertAtLong64NDelegate(DTWAIN_ARRAY pArray, int nWhere, LONG64 Val, int num);
        public delegate int DTWAIN_ArrayInsertAtLongNDelegate(DTWAIN_ARRAY pArray, int nWhere, int pVal, int num);
        public delegate int DTWAIN_ArrayInsertAtNDelegate(DTWAIN_ARRAY pArray, int nWhere, System.IntPtr pVariant, int num);
        public delegate int DTWAIN_ArrayInsertAtStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string pVal);
        public delegate int DTWAIN_ArrayInsertAtStringADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pVal);
        public delegate int DTWAIN_ArrayInsertAtStringNDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num);
        public delegate int DTWAIN_ArrayInsertAtStringNADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val, int num);
        public delegate int DTWAIN_ArrayInsertAtStringNWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num);
        public delegate int DTWAIN_ArrayInsertAtStringWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pVal);
        public delegate int DTWAIN_ArrayInsertAtWideStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pVal);
        public delegate int DTWAIN_ArrayInsertAtWideStringNDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num);
        public delegate int DTWAIN_ArrayRemoveAllDelegate(DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_ArrayRemoveAtDelegate(DTWAIN_ARRAY pArray, int nWhere);
        public delegate int DTWAIN_ArrayRemoveAtNDelegate(DTWAIN_ARRAY pArray, int nWhere, int num);
        public delegate int DTWAIN_ArrayResizeDelegate(DTWAIN_ARRAY pArray, int NewSize);
        public delegate int DTWAIN_ArraySetAtDelegate(DTWAIN_ARRAY pArray, int lPos, System.IntPtr pVariant);
        public delegate int DTWAIN_ArraySetAtANSIStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pStr);
        public delegate int DTWAIN_ArraySetAtFloatDelegate(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FLOAT pVal);
        public delegate int DTWAIN_ArraySetAtFloatStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val);
        public delegate int DTWAIN_ArraySetAtFloatStringADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val);
        public delegate int DTWAIN_ArraySetAtFloatStringWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val);
        public delegate int DTWAIN_ArraySetAtFrameDelegate(DTWAIN_ARRAY FrameArray, int nWhere, DTWAIN_FLOAT left, DTWAIN_FLOAT top, DTWAIN_FLOAT right, DTWAIN_FLOAT bottom);
        public delegate int DTWAIN_ArraySetAtFrameExDelegate(DTWAIN_ARRAY FrameArray, int nWhere, DTWAIN_FRAME Frame);
        public delegate int DTWAIN_ArraySetAtFrameStringDelegate(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string left, [MarshalAs(UnmanagedType.LPTStr)] string top, [MarshalAs(UnmanagedType.LPTStr)] string right, [MarshalAs(UnmanagedType.LPTStr)] string bottom);
        public delegate int DTWAIN_ArraySetAtFrameStringADelegate(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string left, [MarshalAs(UnmanagedType.LPStr)] string top, [MarshalAs(UnmanagedType.LPStr)] string right, [MarshalAs(UnmanagedType.LPStr)] string bottom);
        public delegate int DTWAIN_ArraySetAtFrameStringWDelegate(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string left, [MarshalAs(UnmanagedType.LPWStr)] string top, [MarshalAs(UnmanagedType.LPWStr)] string right, [MarshalAs(UnmanagedType.LPWStr)] string bottom);
        public delegate int DTWAIN_ArraySetAtLongDelegate(DTWAIN_ARRAY pArray, int nWhere, int pVal);
        public delegate int DTWAIN_ArraySetAtLong64Delegate(DTWAIN_ARRAY pArray, int nWhere, LONG64 Val);
        public delegate int DTWAIN_ArraySetAtStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string pStr);
        public delegate int DTWAIN_ArraySetAtStringADelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pStr);
        public delegate int DTWAIN_ArraySetAtStringWDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pStr);
        public delegate int DTWAIN_ArraySetAtWideStringDelegate(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pStr);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayStringToFloatDelegate(DTWAIN_ARRAY StringArray);
        public delegate DTWAIN_ARRAY DTWAIN_ArrayWideStringToFloatDelegate(DTWAIN_ARRAY StringArray);
        public delegate int DTWAIN_CallCallbackDelegate(int wParam, int lParam, int UserData);
        public delegate int DTWAIN_CallCallback64Delegate(int wParam, int lParam, LONGLONG UserData);
        public delegate int DTWAIN_CallDSMProcDelegate(DTWAIN_IDENTITY AppID, DTWAIN_IDENTITY SourceId, int lDG, int lDAT, int lMSG, System.IntPtr pData);
        public delegate int DTWAIN_CheckHandlesDelegate(int bCheck);
        public delegate int DTWAIN_ClearBuffersDelegate(DTWAIN_SOURCE Source, int ClearBuffer);
        public delegate int DTWAIN_ClearErrorBufferDelegate();
        public delegate int DTWAIN_ClearPDFTextElementsDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_ClearPageDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_CloseSourceDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_CloseSourceUIDelegate(DTWAIN_SOURCE Source);
        public delegate HANDLE DTWAIN_ConvertDIBToBitmapDelegate(HANDLE hDib, HANDLE hPalette);
        public delegate HANDLE DTWAIN_ConvertDIBToFullBitmapDelegate(HANDLE hDib, int isBMP);
        public delegate HANDLE DTWAIN_ConvertToAPIStringDelegate([MarshalAs(UnmanagedType.LPTStr)] string lpOrigString);
        public delegate HANDLE DTWAIN_ConvertToAPIStringADelegate([MarshalAs(UnmanagedType.LPStr)] string lpOrigString);
        public delegate int DTWAIN_ConvertToAPIStringExDelegate([MarshalAs(UnmanagedType.LPTStr)] string lpOrigString, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpOutString, int nSize);
        public delegate int DTWAIN_ConvertToAPIStringExADelegate([MarshalAs(UnmanagedType.LPStr)] string lpOrigString, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpOutString, int nSize);
        public delegate int DTWAIN_ConvertToAPIStringExWDelegate([MarshalAs(UnmanagedType.LPWStr)] string lpOrigString, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpOutString, int nSize);
        public delegate HANDLE DTWAIN_ConvertToAPIStringWDelegate([MarshalAs(UnmanagedType.LPWStr)] string lpOrigString);
        public delegate DTWAIN_ARRAY DTWAIN_CreateAcquisitionArrayDelegate();
        public delegate DTWAIN_PDFTEXTELEMENT DTWAIN_CreatePDFTextElementDelegate();
        public delegate DTWAIN_PDFTEXTELEMENT DTWAIN_CreatePDFTextElementCopyDelegate(DTWAIN_PDFTEXTELEMENT TextElement);
        public delegate int DTWAIN_DeleteDIBDelegate(HANDLE hDib);
        public delegate int DTWAIN_DestroyAcquisitionArrayDelegate(DTWAIN_ARRAY aAcq, int bDestroyData);
        public delegate int DTWAIN_DestroyPDFTextElementDelegate(DTWAIN_PDFTEXTELEMENT TextElement);
        public delegate int DTWAIN_DisableAppWindowDelegate(HWND hWnd, int bDisable);
        public delegate int DTWAIN_EnableAutoBorderDetectDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableAutoBrightDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_EnableAutoDeskewDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableAutoFeedDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_EnableAutoRotateDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_EnableAutoScanDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableAutomaticSenseMediumDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_EnableDuplexDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableFeederDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_EnableIndicatorDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableJobFileHandlingDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_EnableLampDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableMsgNotifyDelegate(int bSet);
        public delegate int DTWAIN_EnablePatchDetectDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnablePeekMessageLoopDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_EnablePrinterDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableThumbnailDelegate(DTWAIN_SOURCE Source, int bEnable);
        public delegate int DTWAIN_EnableTripletsNotifyDelegate(int bSet);
        public delegate int DTWAIN_EndThreadDelegate(DTWAIN_HANDLE DLLHandle);
        public delegate int DTWAIN_EndTwainSessionDelegate();
        public delegate int DTWAIN_EnumAlarmVolumesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int expandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumAlarmVolumesExDelegate(DTWAIN_SOURCE Source, int expandIfRange);
        public delegate int DTWAIN_EnumAlarmsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumAlarmsExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumAudioXferMechsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumAudioXferMechsExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumAutoFeedValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumAutoFeedValuesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumAutomaticCapturesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumAutomaticCapturesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumAutomaticSenseMediumDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumAutomaticSenseMediumExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumBitDepthsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_EnumBitDepthsExDelegate(DTWAIN_SOURCE Source, int PixelType, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumBitDepthsEx2Delegate(DTWAIN_SOURCE Source, int PixelType);
        public delegate int DTWAIN_EnumBottomCamerasDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY Cameras);
        public delegate DTWAIN_ARRAY DTWAIN_EnumBottomCamerasExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumBrightnessValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumBrightnessValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumCamerasDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY Cameras);
        public delegate int DTWAIN_EnumCamerasExDelegate(DTWAIN_SOURCE Source, int nWhichCamera, ref DTWAIN_ARRAY Cameras);
        public delegate DTWAIN_ARRAY DTWAIN_EnumCamerasEx2Delegate(DTWAIN_SOURCE Source);
        public delegate DTWAIN_ARRAY DTWAIN_EnumCamerasEx3Delegate(DTWAIN_SOURCE Source, int nWhichCamera);
        public delegate int DTWAIN_EnumCompressionTypesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumCompressionTypesExDelegate(DTWAIN_SOURCE Source);
        public delegate DTWAIN_ARRAY DTWAIN_EnumCompressionTypesEx2Delegate(DTWAIN_SOURCE Source, int lFileType, int bUseBufferedMode);
        public delegate int DTWAIN_EnumContrastValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumContrastValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumCustomCapsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumCustomCapsEx2Delegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumDoubleFeedDetectLengthsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumDoubleFeedDetectLengthsExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumDoubleFeedDetectValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumDoubleFeedDetectValuesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumExtImageInfoTypesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumExtImageInfoTypesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumExtendedCapsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_EnumExtendedCapsExDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumExtendedCapsEx2Delegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumFileTypeBitsPerPixelDelegate(int FileType, ref DTWAIN_ARRAY Array);
        public delegate int DTWAIN_EnumFileXferFormatsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumFileXferFormatsExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumHalftonesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumHalftonesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumHighlightValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumHighlightValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumJobControlsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumJobControlsExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumLightPathsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY LightPath);
        public delegate DTWAIN_ARRAY DTWAIN_EnumLightPathsExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumLightSourcesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY LightSources);
        public delegate DTWAIN_ARRAY DTWAIN_EnumLightSourcesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumMaxBuffersDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pMaxBufs, int bExpandRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumMaxBuffersExDelegate(DTWAIN_SOURCE Source, int bExpandRange);
        public delegate int DTWAIN_EnumNoiseFiltersDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumNoiseFiltersExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumOCRInterfacesDelegate(ref DTWAIN_ARRAY OCRInterfaces);
        public delegate int DTWAIN_EnumOCRSupportedCapsDelegate(DTWAIN_OCRENGINE Engine, ref DTWAIN_ARRAY SupportedCaps);
        public delegate int DTWAIN_EnumOrientationsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumOrientationsExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumOverscanValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumOverscanValuesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPaperSizesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPaperSizesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPatchCodesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY PCodes);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPatchCodesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPatchMaxPrioritiesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPatchMaxPrioritiesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPatchMaxRetriesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPatchMaxRetriesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPatchPrioritiesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPatchPrioritiesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPatchSearchModesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPatchSearchModesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPatchTimeOutValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPatchTimeOutValuesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPixelTypesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPixelTypesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumPrinterStringModesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumPrinterStringModesExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumResolutionValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumResolutionValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumShadowValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumShadowValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumSourceUnitsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY lpArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumSourceUnitsExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumSourceValuesDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string capName, ref DTWAIN_ARRAY values, int bExpandIfRange);
        public delegate int DTWAIN_EnumSourceValuesADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string capName, ref DTWAIN_ARRAY values, int bExpandIfRange);
        public delegate int DTWAIN_EnumSourceValuesWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string capName, ref DTWAIN_ARRAY values, int bExpandIfRange);
        public delegate int DTWAIN_EnumSourcesDelegate(ref DTWAIN_ARRAY lpArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumSourcesExDelegate();
        public delegate int DTWAIN_EnumSupportedCapsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_EnumSupportedCapsExDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumSupportedCapsEx2Delegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumSupportedExtImageInfoDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumSupportedExtImageInfoExDelegate(DTWAIN_SOURCE Source);
        public delegate DTWAIN_ARRAY DTWAIN_EnumSupportedFileTypesDelegate();
        public delegate DTWAIN_ARRAY DTWAIN_EnumSupportedMultiPageFileTypesDelegate();
        public delegate DTWAIN_ARRAY DTWAIN_EnumSupportedSinglePageFileTypesDelegate();
        public delegate int DTWAIN_EnumThresholdValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumThresholdValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumTopCamerasDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY Cameras);
        public delegate DTWAIN_ARRAY DTWAIN_EnumTopCamerasExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumTwainPrintersDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY lpAvailPrinters);
        public delegate int DTWAIN_EnumTwainPrintersArrayDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_EnumTwainPrintersArrayExDelegate(DTWAIN_SOURCE Source);
        public delegate DTWAIN_ARRAY DTWAIN_EnumTwainPrintersExDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_EnumXResolutionValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumXResolutionValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_EnumYResolutionValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange);
        public delegate DTWAIN_ARRAY DTWAIN_EnumYResolutionValuesExDelegate(DTWAIN_SOURCE Source, int bExpandIfRange);
        public delegate int DTWAIN_ExecuteOCRDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] string szFileName, int nStartPage, int nEndPage);
        public delegate int DTWAIN_ExecuteOCRADelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] string szFileName, int nStartPage, int nEndPage);
        public delegate int DTWAIN_ExecuteOCRWDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] string szFileName, int nStartPage, int nEndPage);
        public delegate int DTWAIN_FeedPageDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_FlipBitmapDelegate(HANDLE hDib);
        public delegate int DTWAIN_FlushAcquiredPagesDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_ForceAcquireBitDepthDelegate(DTWAIN_SOURCE Source, int BitDepth);
        public delegate int DTWAIN_ForceScanOnNoUIDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate DTWAIN_FRAME DTWAIN_FrameCreateDelegate(DTWAIN_FLOAT Left, DTWAIN_FLOAT Top, DTWAIN_FLOAT Right, DTWAIN_FLOAT Bottom);
        public delegate DTWAIN_FRAME DTWAIN_FrameCreateStringDelegate([MarshalAs(UnmanagedType.LPTStr)] string Left, [MarshalAs(UnmanagedType.LPTStr)] string Top, [MarshalAs(UnmanagedType.LPTStr)] string Right, [MarshalAs(UnmanagedType.LPTStr)] string Bottom);
        public delegate DTWAIN_FRAME DTWAIN_FrameCreateStringADelegate([MarshalAs(UnmanagedType.LPStr)] string Left, [MarshalAs(UnmanagedType.LPStr)] string Top, [MarshalAs(UnmanagedType.LPStr)] string Right, [MarshalAs(UnmanagedType.LPStr)] string Bottom);
        public delegate DTWAIN_FRAME DTWAIN_FrameCreateStringWDelegate([MarshalAs(UnmanagedType.LPWStr)] string Left, [MarshalAs(UnmanagedType.LPWStr)] string Top, [MarshalAs(UnmanagedType.LPWStr)] string Right, [MarshalAs(UnmanagedType.LPWStr)] string Bottom);
        public delegate int DTWAIN_FrameDestroyDelegate(DTWAIN_FRAME Frame);
        public delegate int DTWAIN_FrameGetAllDelegate(DTWAIN_FRAME Frame, ref DTWAIN_FLOAT Left, ref DTWAIN_FLOAT Top, ref DTWAIN_FLOAT Right, ref DTWAIN_FLOAT Bottom);
        public delegate int DTWAIN_FrameGetAllStringDelegate(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Left, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Top, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Right, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Bottom);
        public delegate int DTWAIN_FrameGetAllStringADelegate(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Left, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Top, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Right, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Bottom);
        public delegate int DTWAIN_FrameGetAllStringWDelegate(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Left, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Top, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Right, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Bottom);
        public delegate int DTWAIN_FrameGetValueDelegate(DTWAIN_FRAME Frame, int nWhich, ref DTWAIN_FLOAT Value);
        public delegate int DTWAIN_FrameGetValueStringDelegate(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Value);
        public delegate int DTWAIN_FrameGetValueStringADelegate(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Value);
        public delegate int DTWAIN_FrameGetValueStringWDelegate(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Value);
        public delegate int DTWAIN_FrameIsValidDelegate(DTWAIN_FRAME Frame);
        public delegate int DTWAIN_FrameSetAllDelegate(DTWAIN_FRAME Frame, DTWAIN_FLOAT Left, DTWAIN_FLOAT Top, DTWAIN_FLOAT Right, DTWAIN_FLOAT Bottom);
        public delegate int DTWAIN_FrameSetAllStringDelegate(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPTStr)] string Left, [MarshalAs(UnmanagedType.LPTStr)] string Top, [MarshalAs(UnmanagedType.LPTStr)] string Right, [MarshalAs(UnmanagedType.LPTStr)] string Bottom);
        public delegate int DTWAIN_FrameSetAllStringADelegate(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPStr)] string Left, [MarshalAs(UnmanagedType.LPStr)] string Top, [MarshalAs(UnmanagedType.LPStr)] string Right, [MarshalAs(UnmanagedType.LPStr)] string Bottom);
        public delegate int DTWAIN_FrameSetAllStringWDelegate(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPWStr)] string Left, [MarshalAs(UnmanagedType.LPWStr)] string Top, [MarshalAs(UnmanagedType.LPWStr)] string Right, [MarshalAs(UnmanagedType.LPWStr)] string Bottom);
        public delegate int DTWAIN_FrameSetValueDelegate(DTWAIN_FRAME Frame, int nWhich, DTWAIN_FLOAT Value);
        public delegate int DTWAIN_FrameSetValueStringDelegate(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] string Value);
        public delegate int DTWAIN_FrameSetValueStringADelegate(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPStr)] string Value);
        public delegate int DTWAIN_FrameSetValueStringWDelegate(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] string Value);
        public delegate int DTWAIN_FreeExtImageInfoDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_FreeMemoryDelegate(HANDLE h);
        public delegate int DTWAIN_FreeMemoryExDelegate(HANDLE h);
        public delegate int DTWAIN_GetAPIHandleStatusDelegate(DTWAIN_HANDLE pHandle);
        public delegate int DTWAIN_GetAcquireAreaDelegate(DTWAIN_SOURCE Source, int lGetType, ref DTWAIN_ARRAY FloatEnum);
        public delegate int DTWAIN_GetAcquireArea2Delegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT left, ref DTWAIN_FLOAT top, ref DTWAIN_FLOAT right, ref DTWAIN_FLOAT bottom, ref int lpUnit);
        public delegate int DTWAIN_GetAcquireArea2StringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder bottom, ref int Unit);
        public delegate int DTWAIN_GetAcquireArea2StringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder bottom, ref int Unit);
        public delegate int DTWAIN_GetAcquireArea2StringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder bottom, ref int Unit);
        public delegate DTWAIN_ARRAY DTWAIN_GetAcquireAreaExDelegate(DTWAIN_SOURCE Source, int lGetType);
        public delegate int DTWAIN_GetAcquireMetricsDelegate(DTWAIN_SOURCE source, ref int ImageCount, ref int SheetCount);
        public delegate HANDLE DTWAIN_GetAcquireStripBufferDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetAcquireStripDataDelegate(DTWAIN_SOURCE Source, ref int lpCompression, ref DWORD lpBytesPerRow, ref DWORD lpColumns, ref DWORD lpRows, ref DWORD XOffset, ref DWORD YOffset, ref DWORD lpBytesWritten);
        public delegate int DTWAIN_GetAcquireStripSizesDelegate(DTWAIN_SOURCE Source, ref DWORD lpMin, ref DWORD lpMax, ref DWORD lpPreferred);
        public delegate HANDLE DTWAIN_GetAcquiredImageDelegate(DTWAIN_ARRAY aAcq, int nWhichAcq, int nWhichDib);
        public delegate DTWAIN_ARRAY DTWAIN_GetAcquiredImageArrayDelegate(DTWAIN_ARRAY aAcq, int nWhichAcq);
        public delegate int DTWAIN_GetActiveDSMPathDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetActiveDSMPathADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetActiveDSMPathWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetActiveDSMVersionInfoDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szDLLInfo, int nMaxLen);
        public delegate int DTWAIN_GetActiveDSMVersionInfoADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetActiveDSMVersionInfoWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetAlarmVolumeDelegate(DTWAIN_SOURCE Source, ref int lpVolume);
        public delegate DTWAIN_ARRAY DTWAIN_GetAllSourceDibsDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetAppInfoDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szVerStr, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szManu, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProdFam, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProdName);
        public delegate int DTWAIN_GetAppInfoADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szVerStr, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szManu, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProdFam, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProdName);
        public delegate int DTWAIN_GetAppInfoWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szVerStr, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szManu, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProdFam, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProdName);
        public delegate int DTWAIN_GetAuthorDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szAuthor);
        public delegate int DTWAIN_GetAuthorADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szAuthor);
        public delegate int DTWAIN_GetAuthorWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szAuthor);
        public delegate int DTWAIN_GetBatteryMinutesDelegate(DTWAIN_SOURCE Source, ref int lpMinutes);
        public delegate int DTWAIN_GetBatteryPercentDelegate(DTWAIN_SOURCE Source, ref int lpPercent);
        public delegate int DTWAIN_GetBitDepthDelegate(DTWAIN_SOURCE Source, ref int BitDepth, int bCurrent);
        public delegate int DTWAIN_GetBlankPageAutoDetectionDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetBrightnessDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Brightness);
        public delegate int DTWAIN_GetBrightnessStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Brightness);
        public delegate int DTWAIN_GetBrightnessStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Contrast);
        public delegate int DTWAIN_GetBrightnessStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Contrast);
        public delegate HANDLE DTWAIN_GetBufferedTransferInfoDelegate(DTWAIN_SOURCE Source, ref DWORD Compression, ref DWORD BytesPerRow, ref DWORD Columns, ref DWORD Rows, ref DWORD XOffset, ref DWORD YOffset, ref DWORD Flags, ref DWORD BytesWritten, ref DWORD MemoryLength);
        public delegate DTwainCallback DTWAIN_GetCallbackDelegate();
        public delegate DTwainCallback64 DTWAIN_GetCallback64Delegate();
        public delegate int DTWAIN_GetCapArrayTypeDelegate(DTWAIN_SOURCE Source, int nCap);
        public delegate int DTWAIN_GetCapContainerDelegate(DTWAIN_SOURCE Source, int nCap, int lCapType);
        public delegate int DTWAIN_GetCapContainerExDelegate(int nCap, int bSetContainer, ref DTWAIN_ARRAY ConTypes);
        public delegate DTWAIN_ARRAY DTWAIN_GetCapContainerEx2Delegate(int nCap, int bSetContainer);
        public delegate int DTWAIN_GetCapDataTypeDelegate(DTWAIN_SOURCE Source, int nCap);
        public delegate int DTWAIN_GetCapFromNameDelegate([MarshalAs(UnmanagedType.LPTStr)] string szName);
        public delegate int DTWAIN_GetCapFromNameADelegate([MarshalAs(UnmanagedType.LPStr)] string szName);
        public delegate int DTWAIN_GetCapFromNameWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szName);
        public delegate int DTWAIN_GetCapOperationsDelegate(DTWAIN_SOURCE Source, int lCapability, ref int lpOps);
        public delegate int DTWAIN_GetCapValuesDelegate(DTWAIN_SOURCE Source, int lCap, int lGetType, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_GetCapValuesExDelegate(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_GetCapValuesEx2Delegate(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType, int nDataType, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_GetCaptionDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Caption);
        public delegate int DTWAIN_GetCaptionADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Caption);
        public delegate int DTWAIN_GetCaptionWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Caption);
        public delegate int DTWAIN_GetCompressionSizeDelegate(DTWAIN_SOURCE Source, ref int lBytes);
        public delegate int DTWAIN_GetCompressionTypeDelegate(DTWAIN_SOURCE Source, ref int lpCompression, int bCurrent);
        public delegate int DTWAIN_GetConditionCodeStringDelegate(int lError, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetConditionCodeStringADelegate(int lError, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetConditionCodeStringWDelegate(int lError, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetContrastDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Contrast);
        public delegate int DTWAIN_GetContrastStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Contrast);
        public delegate int DTWAIN_GetContrastStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Contrast);
        public delegate int DTWAIN_GetContrastStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Contrast);
        public delegate int DTWAIN_GetCountryDelegate();
        public delegate HANDLE DTWAIN_GetCurrentAcquiredImageDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetCurrentFileNameDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szName, int MaxLen);
        public delegate int DTWAIN_GetCurrentFileNameADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szName, int MaxLen);
        public delegate int DTWAIN_GetCurrentFileNameWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szName, int MaxLen);
        public delegate int DTWAIN_GetCurrentPageNumDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetCurrentRetryCountDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetCurrentTwainTripletDelegate([In, Out] TW_IDENTITY pAppID, [In, Out] TW_IDENTITY pSourceID, ref int lpDG, ref int lpDAT, ref int lpMsg, ref long lpMemRef);
        public delegate HANDLE DTWAIN_GetCustomDSDataDelegate(DTWAIN_SOURCE Source, byte[] Data, uint dSize, ref DWORD pActualSize, int nFlags);
        public delegate int DTWAIN_GetDSMFullNameDelegate(int DSMType, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szDLLName, int nMaxLen, ref int pWhichSearch);
        public delegate int DTWAIN_GetDSMFullNameADelegate(int DSMType, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szDLLName, int nMaxLen, ref int pWhichSearch);
        public delegate int DTWAIN_GetDSMFullNameWDelegate(int DSMType, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szDLLName, int nMaxLen, ref int pWhichSearch);
        public delegate int DTWAIN_GetDSMSearchOrderDelegate();
        public delegate DTWAIN_HANDLE DTWAIN_GetDTWAINHandleDelegate();
        public delegate int DTWAIN_GetDeviceEventDelegate(DTWAIN_SOURCE Source, ref int lpEvent);
        public delegate int DTWAIN_GetDeviceEventExDelegate(DTWAIN_SOURCE Source, ref int lpEvent, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_GetDeviceEventInfoDelegate(DTWAIN_SOURCE Source, int nWhichInfo, System.IntPtr pValue);
        public delegate int DTWAIN_GetDeviceNotificationsDelegate(DTWAIN_SOURCE Source, ref int DevEvents);
        public delegate int DTWAIN_GetDeviceTimeDateDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szTimeDate);
        public delegate int DTWAIN_GetDeviceTimeDateADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szTimeDate);
        public delegate int DTWAIN_GetDeviceTimeDateWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szTimeDate);
        public delegate int DTWAIN_GetDoubleFeedDetectLengthDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Value, int bCurrent);
        public delegate int DTWAIN_GetDoubleFeedDetectValuesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_GetDuplexTypeDelegate(DTWAIN_SOURCE Source, ref int lpDupType);
        public delegate int DTWAIN_GetErrorBufferDelegate(ref DTWAIN_ARRAY ArrayBuffer);
        public delegate int DTWAIN_GetErrorBufferThresholdDelegate();
        public delegate DTwainErrorProc DTWAIN_GetErrorCallbackDelegate();
        public delegate DTwainErrorProc64 DTWAIN_GetErrorCallback64Delegate();
        public delegate int DTWAIN_GetErrorStringDelegate(int lError, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetErrorStringADelegate(int lError, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nLength);
        public delegate int DTWAIN_GetErrorStringWDelegate(int lError, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nLength);
        public delegate int DTWAIN_GetExtCapFromNameDelegate([MarshalAs(UnmanagedType.LPTStr)] string szName);
        public delegate int DTWAIN_GetExtCapFromNameADelegate([MarshalAs(UnmanagedType.LPStr)] string szName);
        public delegate int DTWAIN_GetExtCapFromNameWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szName);
        public delegate int DTWAIN_GetExtImageInfoDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetExtImageInfoDataDelegate(DTWAIN_SOURCE Source, int nWhich, ref DTWAIN_ARRAY Data);
        public delegate DTWAIN_ARRAY DTWAIN_GetExtImageInfoDataExDelegate(DTWAIN_SOURCE Source, int nWhich);
        public delegate int DTWAIN_GetExtImageInfoItemDelegate(DTWAIN_SOURCE Source, int nWhich, ref int InfoID, ref int NumItems, ref int Type);
        public delegate int DTWAIN_GetExtImageInfoItemExDelegate(DTWAIN_SOURCE Source, int nWhich, ref int InfoID, ref int NumItems, ref int Type, ref int ReturnCode);
        public delegate int DTWAIN_GetExtNameFromCapDelegate(int nValue, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szValue, int nMaxLen);
        public delegate int DTWAIN_GetExtNameFromCapADelegate(int nValue, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szValue, int nLength);
        public delegate int DTWAIN_GetExtNameFromCapWDelegate(int nValue, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szValue, int nLength);
        public delegate int DTWAIN_GetFeederAlignmentDelegate(DTWAIN_SOURCE Source, ref int lpAlignment);
        public delegate int DTWAIN_GetFeederFuncsDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetFeederOrderDelegate(DTWAIN_SOURCE Source, ref int lpOrder);
        public delegate int DTWAIN_GetFeederWaitTimeDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetFileCompressionTypeDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetFileSavePageCountDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetFileTypeExtensionsDelegate(int nType, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszName, int nLength);
        public delegate int DTWAIN_GetFileTypeExtensionsADelegate(int nType, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszName, int nLength);
        public delegate int DTWAIN_GetFileTypeExtensionsWDelegate(int nType, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszName, int nLength);
        public delegate int DTWAIN_GetFileTypeNameDelegate(int nType, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszName, int nLength);
        public delegate int DTWAIN_GetFileTypeNameADelegate(int nType, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszName, int nLength);
        public delegate int DTWAIN_GetFileTypeNameWDelegate(int nType, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszName, int nLength);
        public delegate int DTWAIN_GetHalftoneDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpHalftone, int GetType);
        public delegate int DTWAIN_GetHalftoneADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpHalftone, int GetType);
        public delegate int DTWAIN_GetHalftoneWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpHalftone, int GetType);
        public delegate int DTWAIN_GetHighlightDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Highlight);
        public delegate int DTWAIN_GetHighlightStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Highlight);
        public delegate int DTWAIN_GetHighlightStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Highlight);
        public delegate int DTWAIN_GetHighlightStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Highlight);
        public delegate int DTWAIN_GetImageInfoDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT lpXResolution, ref DTWAIN_FLOAT lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression);
        public delegate int DTWAIN_GetImageInfoStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpXResolution, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression);
        public delegate int DTWAIN_GetImageInfoStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpXResolution, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression);
        public delegate int DTWAIN_GetImageInfoStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpXResolution, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression);
        public delegate int DTWAIN_GetJobControlDelegate(DTWAIN_SOURCE Source, ref int pJobControl, int bCurrent);
        public delegate int DTWAIN_GetJpegValuesDelegate(DTWAIN_SOURCE Source, ref int pQuality, ref int Progressive);
        public delegate int DTWAIN_GetJpegXRValuesDelegate(DTWAIN_SOURCE Source, ref int pQuality, ref int Progressive);
        public delegate int DTWAIN_GetLanguageDelegate();
        public delegate int DTWAIN_GetLastErrorDelegate();
        public delegate int DTWAIN_GetLibraryPathDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetLibraryPathADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetLibraryPathWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetLightPathDelegate(DTWAIN_SOURCE Source, ref int lpLightPath);
        public delegate int DTWAIN_GetLightSourceDelegate(DTWAIN_SOURCE Source, ref int LightSource);
        public delegate int DTWAIN_GetLightSourcesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY LightSources);
        public delegate DTWAIN_ARRAY DTWAIN_GetLightSourcesExDelegate(DTWAIN_SOURCE Source);
        public delegate DTwainLoggerProc DTWAIN_GetLoggerCallbackDelegate();
        public delegate DTwainLoggerProcA DTWAIN_GetLoggerCallbackADelegate();
        public delegate DTwainLoggerProcW DTWAIN_GetLoggerCallbackWDelegate();
        public delegate int DTWAIN_GetManualDuplexCountDelegate(DTWAIN_SOURCE Source, ref int pSide1, ref int pSide2);
        public delegate int DTWAIN_GetMaxAcquisitionsDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetMaxBuffersDelegate(DTWAIN_SOURCE Source, ref int pMaxBuf);
        public delegate int DTWAIN_GetMaxPagesToAcquireDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetMaxRetryAttemptsDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetNameFromCapDelegate(int nCapValue, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szValue, int nMaxLen);
        public delegate int DTWAIN_GetNameFromCapADelegate(int nCapValue, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szValue, int nLength);
        public delegate int DTWAIN_GetNameFromCapWDelegate(int nCapValue, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szValue, int nLength);
        public delegate int DTWAIN_GetNoiseFilterDelegate(DTWAIN_SOURCE Source, ref int lpNoiseFilter);
        public delegate int DTWAIN_GetNumAcquiredImagesDelegate(DTWAIN_ARRAY aAcq, int nWhich);
        public delegate int DTWAIN_GetNumAcquisitionsDelegate(DTWAIN_ARRAY aAcq);
        public delegate int DTWAIN_GetOCRCapValuesDelegate(DTWAIN_OCRENGINE Engine, int OCRCapValue, int GetType, ref DTWAIN_ARRAY CapValues);
        public delegate int DTWAIN_GetOCRErrorStringDelegate(DTWAIN_OCRENGINE Engine, int lError, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetOCRErrorStringADelegate(DTWAIN_OCRENGINE Engine, int lError, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetOCRErrorStringWDelegate(DTWAIN_OCRENGINE Engine, int lError, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetOCRLastErrorDelegate(DTWAIN_OCRENGINE Engine);
        public delegate int DTWAIN_GetOCRMajorMinorVersionDelegate(DTWAIN_OCRENGINE Engine, ref int lpMajor, ref int lpMinor);
        public delegate int DTWAIN_GetOCRManufacturerDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szManufacturer, int nMaxLen);
        public delegate int DTWAIN_GetOCRManufacturerADelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szManufacturer, int nLength);
        public delegate int DTWAIN_GetOCRManufacturerWDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szManufacturer, int nLength);
        public delegate int DTWAIN_GetOCRProductFamilyDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProductFamily, int nMaxLen);
        public delegate int DTWAIN_GetOCRProductFamilyADelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProductFamily, int nLength);
        public delegate int DTWAIN_GetOCRProductFamilyWDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProductFamily, int nLength);
        public delegate int DTWAIN_GetOCRProductNameDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProductName, int nMaxLen);
        public delegate int DTWAIN_GetOCRProductNameADelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProductName, int nLength);
        public delegate int DTWAIN_GetOCRProductNameWDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProductName, int nLength);
        public delegate HANDLE DTWAIN_GetOCRTextDelegate(DTWAIN_OCRENGINE Engine, int nPageNo, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Data, int dSize, ref int pActualSize, int nFlags);
        public delegate HANDLE DTWAIN_GetOCRTextADelegate(DTWAIN_OCRENGINE Engine, int nPageNo, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Data, int dSize, ref int pActualSize, int nFlags);
        public delegate int DTWAIN_GetOCRTextInfoFloatDelegate(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nCharPos, int nWhichItem, ref DTWAIN_FLOAT pInfo);
        public delegate int DTWAIN_GetOCRTextInfoFloatExDelegate(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nWhichItem, ref DTWAIN_FLOAT pInfo, int bufSize);
        public delegate DTWAIN_OCRTEXTINFOHANDLE DTWAIN_GetOCRTextInfoHandleDelegate(DTWAIN_OCRENGINE Engine, int nPageNo);
        public delegate int DTWAIN_GetOCRTextInfoLongDelegate(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nCharPos, int nWhichItem, ref int pInfo);
        public delegate int DTWAIN_GetOCRTextInfoLongExDelegate(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nWhichItem, ref int pInfo, int bufSize);
        public delegate HANDLE DTWAIN_GetOCRTextWDelegate(DTWAIN_OCRENGINE Engine, int nPageNo, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Data, int dSize, ref int pActualSize, int nFlags);
        public delegate int DTWAIN_GetOCRVersionInfoDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder buffer, int maxBufSize);
        public delegate int DTWAIN_GetOCRVersionInfoADelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder buffer, int nLength);
        public delegate int DTWAIN_GetOCRVersionInfoWDelegate(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder buffer, int nLength);
        public delegate int DTWAIN_GetOrientationDelegate(DTWAIN_SOURCE Source, ref int lpOrient, int bCurrent);
        public delegate int DTWAIN_GetOverscanDelegate(DTWAIN_SOURCE Source, ref int lpOverscan, int bCurrent);
        public delegate int DTWAIN_GetPDFTextElementFloatDelegate(DTWAIN_PDFTEXTELEMENT TextElement, ref DTWAIN_FLOAT val1, ref DTWAIN_FLOAT val2, int Flags);
        public delegate int DTWAIN_GetPDFTextElementLongDelegate(DTWAIN_PDFTEXTELEMENT TextElement, ref int val1, ref int val2, int Flags);
        public delegate int DTWAIN_GetPDFTextElementStringDelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szData, int maxLen, int Flags);
        public delegate int DTWAIN_GetPDFTextElementStringADelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szData, int maxLen, int Flags);
        public delegate int DTWAIN_GetPDFTextElementStringWDelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szData, int maxLen, int Flags);
        public delegate int DTWAIN_GetPDFType1FontNameDelegate(int FontVal, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szFont, int nChars);
        public delegate int DTWAIN_GetPDFType1FontNameADelegate(int FontVal, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szFont, int nChars);
        public delegate int DTWAIN_GetPDFType1FontNameWDelegate(int FontVal, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szFont, int nChars);
        public delegate int DTWAIN_GetPaperSizeDelegate(DTWAIN_SOURCE Source, ref int lpPaperSize, int bCurrent);
        public delegate int DTWAIN_GetPaperSizeNameDelegate(int paperNumber, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder outName, int nSize);
        public delegate int DTWAIN_GetPaperSizeNameADelegate(int paperNumber, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder outName, int nSize);
        public delegate int DTWAIN_GetPaperSizeNameWDelegate(int paperNumber, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder outName, int nSize);
        public delegate int DTWAIN_GetPatchMaxPrioritiesDelegate(DTWAIN_SOURCE Source, ref int pMaxPriorities, int bCurrent);
        public delegate int DTWAIN_GetPatchMaxRetriesDelegate(DTWAIN_SOURCE Source, ref int pMaxRetries, int bCurrent);
        public delegate int DTWAIN_GetPatchPrioritiesDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY SearchPriorities);
        public delegate int DTWAIN_GetPatchSearchModeDelegate(DTWAIN_SOURCE Source, ref int pSearchMode, int bCurrent);
        public delegate int DTWAIN_GetPatchTimeOutDelegate(DTWAIN_SOURCE Source, ref int pTimeOut, int bCurrent);
        public delegate int DTWAIN_GetPixelFlavorDelegate(DTWAIN_SOURCE Source, ref int lpPixelFlavor);
        public delegate int DTWAIN_GetPixelTypeDelegate(DTWAIN_SOURCE Source, ref int PixelType, ref int BitDepth, int bCurrent);
        public delegate int DTWAIN_GetPrinterDelegate(DTWAIN_SOURCE Source, ref int lpPrinter, int bCurrent);
        public delegate int DTWAIN_GetPrinterStartNumberDelegate(DTWAIN_SOURCE Source, ref int nStart);
        public delegate int DTWAIN_GetPrinterStringModeDelegate(DTWAIN_SOURCE Source, ref int PrinterMode, int bCurrent);
        public delegate int DTWAIN_GetPrinterStringsDelegate(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY ArrayString);
        public delegate int DTWAIN_GetPrinterSuffixStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Suffix, int nMaxLen);
        public delegate int DTWAIN_GetPrinterSuffixStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Suffix, int nLength);
        public delegate int DTWAIN_GetPrinterSuffixStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Suffix, int nLength);
        public delegate int DTWAIN_GetRegisteredMsgDelegate();
        public delegate int DTWAIN_GetResolutionDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Resolution);
        public delegate int DTWAIN_GetResolutionStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetResolutionStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetResolutionStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetResourceStringDelegate(int ResourceID, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetResourceStringADelegate(int ResourceID, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetResourceStringWDelegate(int ResourceID, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetRotationDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Rotation);
        public delegate int DTWAIN_GetRotationStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Rotation);
        public delegate int DTWAIN_GetRotationStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Rotation);
        public delegate int DTWAIN_GetRotationStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Rotation);
        public delegate int DTWAIN_GetSaveFileNameDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder fName, int nMaxLen);
        public delegate int DTWAIN_GetSaveFileNameADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder fName, int nMaxLen);
        public delegate int DTWAIN_GetSaveFileNameWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder fName, int nMaxLen);
        public delegate int DTWAIN_GetSessionDetailsDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh);
        public delegate int DTWAIN_GetSessionDetailsADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh);
        public delegate int DTWAIN_GetSessionDetailsWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh);
        public delegate int DTWAIN_GetShadowDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Shadow);
        public delegate int DTWAIN_GetShadowStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Shadow);
        public delegate int DTWAIN_GetShadowStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Shadow);
        public delegate int DTWAIN_GetShadowStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Shadow);
        public delegate int DTWAIN_GetShortVersionStringDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetShortVersionStringADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetShortVersionStringWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate DTWAIN_ARRAY DTWAIN_GetSourceAcquisitionsDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_GetSourceDetailsDelegate([MarshalAs(UnmanagedType.LPTStr)] string szSources, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh);
        public delegate int DTWAIN_GetSourceDetailsADelegate([MarshalAs(UnmanagedType.LPStr)] string szSources, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh);
        public delegate int DTWAIN_GetSourceDetailsWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szSources, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh);
        public delegate DTWAIN_IDENTITY DTWAIN_GetSourceIDDelegate(DTWAIN_SOURCE Source);
        public delegate TW_IDENTITY DTWAIN_GetSourceIDExDelegate(DTWAIN_SOURCE Source, [In, Out] TW_IDENTITY pIdentity);
        public delegate int DTWAIN_GetSourceManufacturerDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen);
        public delegate int DTWAIN_GetSourceManufacturerADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceManufacturerWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceProductFamilyDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen);
        public delegate int DTWAIN_GetSourceProductFamilyADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceProductFamilyWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceProductNameDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen);
        public delegate int DTWAIN_GetSourceProductNameADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceProductNameWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceUnitDelegate(DTWAIN_SOURCE Source, ref int lpUnit);
        public delegate int DTWAIN_GetSourceVersionInfoDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen);
        public delegate int DTWAIN_GetSourceVersionInfoADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceVersionInfoWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength);
        public delegate int DTWAIN_GetSourceVersionNumberDelegate(DTWAIN_SOURCE Source, ref int pMajor, ref int pMinor);
        public delegate int DTWAIN_GetStaticLibVersionDelegate();
        public delegate int DTWAIN_GetTempFileDirectoryDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szFilePath, int nMaxLen);
        public delegate int DTWAIN_GetTempFileDirectoryADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szFilePath, int nLength);
        public delegate int DTWAIN_GetTempFileDirectoryWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szFilePath, int nLength);
        public delegate int DTWAIN_GetThresholdDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Threshold);
        public delegate int DTWAIN_GetThresholdStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Threshold);
        public delegate int DTWAIN_GetThresholdStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Threshold);
        public delegate int DTWAIN_GetThresholdStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Threshold);
        public delegate int DTWAIN_GetTimeDateDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szTimeDate);
        public delegate int DTWAIN_GetTimeDateADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szTimeDate);
        public delegate int DTWAIN_GetTimeDateWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szTimeDate);
        public delegate DTWAIN_IDENTITY DTWAIN_GetTwainAppIDDelegate();
        public delegate TW_IDENTITY DTWAIN_GetTwainAppIDExDelegate([In, Out] TW_IDENTITY pIdentity);
        public delegate int DTWAIN_GetTwainAvailabilityDelegate();
        public delegate int DTWAIN_GetTwainAvailabilityExDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder directories, int nMaxLen);
        public delegate int DTWAIN_GetTwainAvailabilityExADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szDirectories, int nLength);
        public delegate int DTWAIN_GetTwainAvailabilityExWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szDirectories, int nLength);
        public delegate int DTWAIN_GetTwainCountryNameDelegate(int countryId, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szName);
        public delegate int DTWAIN_GetTwainCountryNameADelegate(int countryId, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szName);
        public delegate int DTWAIN_GetTwainCountryNameWDelegate(int countryId, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szName);
        public delegate int DTWAIN_GetTwainCountryValueDelegate([MarshalAs(UnmanagedType.LPTStr)] string country);
        public delegate int DTWAIN_GetTwainCountryValueADelegate([MarshalAs(UnmanagedType.LPStr)] string country);
        public delegate int DTWAIN_GetTwainCountryValueWDelegate([MarshalAs(UnmanagedType.LPWStr)] string country);
        public delegate HWND DTWAIN_GetTwainHwndDelegate();
        public delegate int DTWAIN_GetTwainIDFromNameDelegate([MarshalAs(UnmanagedType.LPTStr)] string lpszBuffer);
        public delegate int DTWAIN_GetTwainIDFromNameADelegate([MarshalAs(UnmanagedType.LPStr)] string lpszBuffer);
        public delegate int DTWAIN_GetTwainIDFromNameWDelegate([MarshalAs(UnmanagedType.LPWStr)] string lpszBuffer);
        public delegate int DTWAIN_GetTwainLanguageNameDelegate(int nameId, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szName);
        public delegate int DTWAIN_GetTwainLanguageNameADelegate(int lang, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szName);
        public delegate int DTWAIN_GetTwainLanguageNameWDelegate(int lang, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szName);
        public delegate int DTWAIN_GetTwainLanguageValueDelegate([MarshalAs(UnmanagedType.LPTStr)] string szName);
        public delegate int DTWAIN_GetTwainLanguageValueADelegate([MarshalAs(UnmanagedType.LPStr)] string lang);
        public delegate int DTWAIN_GetTwainLanguageValueWDelegate([MarshalAs(UnmanagedType.LPWStr)] string lang);
        public delegate int DTWAIN_GetTwainModeDelegate();
        public delegate int DTWAIN_GetTwainNameFromConstantDelegate(int lConstantType, int lTwainConstant, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszOut, int nSize);
        public delegate int DTWAIN_GetTwainNameFromConstantADelegate(int lConstantType, int lTwainConstant, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszOut, int nSize);
        public delegate int DTWAIN_GetTwainNameFromConstantWDelegate(int lConstantType, int lTwainConstant, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszOut, int nSize);
        public delegate int DTWAIN_GetTwainStringNameDelegate(int category, int TwainID, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetTwainStringNameADelegate(int category, int TwainID, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetTwainStringNameWDelegate(int category, int TwainID, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetTwainTimeoutDelegate();
        public delegate int DTWAIN_GetVersionDelegate(ref int lpMajor, ref int lpMinor, ref int lpVersionType);
        public delegate int DTWAIN_GetVersionCopyrightDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszApp, int nLength);
        public delegate int DTWAIN_GetVersionCopyrightADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszApp, int nLength);
        public delegate int DTWAIN_GetVersionCopyrightWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszApp, int nLength);
        public delegate int DTWAIN_GetVersionExDelegate(ref int lMajor, ref int lMinor, ref int lVersionType, ref int lPatchLevel);
        public delegate int DTWAIN_GetVersionInfoDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetVersionInfoADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetVersionInfoWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetVersionStringDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetVersionStringADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetVersionStringWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength);
        public delegate int DTWAIN_GetWindowsVersionInfoDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetWindowsVersionInfoADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetWindowsVersionInfoWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen);
        public delegate int DTWAIN_GetXResolutionDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Resolution);
        public delegate int DTWAIN_GetXResolutionStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetXResolutionStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetXResolutionStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetYResolutionDelegate(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Resolution);
        public delegate int DTWAIN_GetYResolutionStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetYResolutionStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_GetYResolutionStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Resolution);
        public delegate int DTWAIN_InitExtImageInfoDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_InitImageFileAppendDelegate([MarshalAs(UnmanagedType.LPTStr)] string szFile, int fType);
        public delegate int DTWAIN_InitImageFileAppendADelegate([MarshalAs(UnmanagedType.LPStr)] string szFile, int fType);
        public delegate int DTWAIN_InitImageFileAppendWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szFile, int fType);
        public delegate int DTWAIN_InitOCRInterfaceDelegate();
        public delegate int DTWAIN_IsAcquiringDelegate();
        public delegate int DTWAIN_IsAudioXferSupportedDelegate(DTWAIN_SOURCE Source, int supportVal);
        public delegate int DTWAIN_IsAutoBorderDetectEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoBorderDetectSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoBrightEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoBrightSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoDeskewEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoDeskewSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoFeedEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoFeedSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoRotateEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoRotateSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutoScanEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutomaticSenseMediumEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsAutomaticSenseMediumSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsBlankPageDetectionOnDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsBufferedTileModeOnDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsBufferedTileModeSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsCapSupportedDelegate(DTWAIN_SOURCE Source, int lCapability);
        public delegate int DTWAIN_IsCompressionSupportedDelegate(DTWAIN_SOURCE Source, int Compression);
        public delegate int DTWAIN_IsCustomDSDataSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsDIBBlankDelegate(HANDLE hDib, DTWAIN_FLOAT threshold);
        public delegate int DTWAIN_IsDIBBlankStringDelegate(HANDLE hDib, [MarshalAs(UnmanagedType.LPTStr)] string threshold);
        public delegate int DTWAIN_IsDIBBlankStringADelegate(HANDLE hDib, [MarshalAs(UnmanagedType.LPStr)] string threshold);
        public delegate int DTWAIN_IsDIBBlankStringWDelegate(HANDLE hDib, [MarshalAs(UnmanagedType.LPWStr)] string threshold);
        public delegate int DTWAIN_IsDeviceEventSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsDeviceOnLineDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsDoubleFeedDetectLengthSupportedDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT value);
        public delegate int DTWAIN_IsDoubleFeedDetectSupportedDelegate(DTWAIN_SOURCE Source, int SupportVal);
        public delegate int DTWAIN_IsDoublePageCountOnDuplexDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsDuplexEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsDuplexSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsExtImageInfoSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsFeederEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsFeederLoadedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsFeederSensitiveDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsFeederSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsFileSystemSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsFileXferSupportedDelegate(DTWAIN_SOURCE Source, int lFileType);
        public delegate int DTWAIN_IsIAFieldALastPageSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldALevelSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldAPrintFormatSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldAValueSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldBLastPageSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldBLevelSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldBPrintFormatSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldBValueSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldCLastPageSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldCLevelSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldCPrintFormatSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldCValueSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldDLastPageSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldDLevelSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldDPrintFormatSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldDValueSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldELastPageSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldELevelSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldEPrintFormatSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIAFieldEValueSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsImageAddressingSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIndicatorEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsIndicatorSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsInitializedDelegate();
        public delegate int DTWAIN_IsJobControlSupportedDelegate(DTWAIN_SOURCE Source, int JobControl);
        public delegate int DTWAIN_IsLampEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsLampSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsLightPathSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsLightSourceSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsMaxBuffersSupportedDelegate(DTWAIN_SOURCE Source, int MaxBuf);
        public delegate int DTWAIN_IsMemFileXferSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsMsgNotifyEnabledDelegate();
        public delegate int DTWAIN_IsNotifyTripletsEnabledDelegate();
        public delegate int DTWAIN_IsOCREngineActivatedDelegate(DTWAIN_OCRENGINE OCREngine);
        public delegate int DTWAIN_IsOpenSourcesOnSelectDelegate();
        public delegate int DTWAIN_IsOrientationSupportedDelegate(DTWAIN_SOURCE Source, int Orientation);
        public delegate int DTWAIN_IsOverscanSupportedDelegate(DTWAIN_SOURCE Source, int SupportValue);
        public delegate int DTWAIN_IsPaperDetectableDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsPaperSizeSupportedDelegate(DTWAIN_SOURCE Source, int PaperSize);
        public delegate int DTWAIN_IsPatchCapsSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsPatchDetectEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsPatchSupportedDelegate(DTWAIN_SOURCE Source, int PatchCode);
        public delegate int DTWAIN_IsPeekMessageLoopEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsPixelTypeSupportedDelegate(DTWAIN_SOURCE Source, int PixelType);
        public delegate int DTWAIN_IsPrinterEnabledDelegate(DTWAIN_SOURCE Source, int Printer);
        public delegate int DTWAIN_IsPrinterSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsRotationSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsSessionEnabledDelegate();
        public delegate int DTWAIN_IsSkipImageInfoErrorDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsSourceAcquiringDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsSourceAcquiringExDelegate(DTWAIN_SOURCE Source, int bUIOnly);
        public delegate int DTWAIN_IsSourceInUIOnlyModeDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsSourceOpenDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsSourceSelectedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsSourceValidDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsThumbnailEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsThumbnailSupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsTwainAvailableDelegate();
        public delegate int DTWAIN_IsTwainAvailableExDelegate([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder directories, int nMaxLen);
        public delegate int DTWAIN_IsTwainAvailableExADelegate([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder directories, int nMaxLen);
        public delegate int DTWAIN_IsTwainAvailableExWDelegate([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder directories, int nMaxLen);
        public delegate int DTWAIN_IsTwainMsgDelegate(ref POINT pMsg);
        public delegate int DTWAIN_IsUIControllableDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsUIEnabledDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_IsUIOnlySupportedDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_LoadCustomStringResourcesDelegate([MarshalAs(UnmanagedType.LPTStr)] string sLangDLL);
        public delegate int DTWAIN_LoadCustomStringResourcesADelegate([MarshalAs(UnmanagedType.LPStr)] string sLangDLL);
        public delegate int DTWAIN_LoadCustomStringResourcesExDelegate([MarshalAs(UnmanagedType.LPTStr)] string sLangDLL, int bClear);
        public delegate int DTWAIN_LoadCustomStringResourcesExADelegate([MarshalAs(UnmanagedType.LPStr)] string sLangDLL, int bClear);
        public delegate int DTWAIN_LoadCustomStringResourcesExWDelegate([MarshalAs(UnmanagedType.LPWStr)] string sLangDLL, int bClear);
        public delegate int DTWAIN_LoadCustomStringResourcesWDelegate([MarshalAs(UnmanagedType.LPWStr)] string sLangDLL);
        public delegate int DTWAIN_LoadLanguageResourceDelegate(int nLanguage);
        public delegate DTWAIN_MEMORY_PTR DTWAIN_LockMemoryDelegate(HANDLE h);
        public delegate DTWAIN_MEMORY_PTR DTWAIN_LockMemoryExDelegate(HANDLE h);
        public delegate int DTWAIN_LogMessageDelegate([MarshalAs(UnmanagedType.LPTStr)] string message);
        public delegate int DTWAIN_LogMessageADelegate([MarshalAs(UnmanagedType.LPStr)] string message);
        public delegate int DTWAIN_LogMessageWDelegate([MarshalAs(UnmanagedType.LPWStr)] string message);
        public delegate int DTWAIN_MakeRGBDelegate(int red, int green, int blue);
        public delegate int DTWAIN_OpenSourceDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_OpenSourcesOnSelectDelegate(int bSet);
        public delegate DTWAIN_RANGE DTWAIN_RangeCreateDelegate(int nEnumType);
        public delegate DTWAIN_RANGE DTWAIN_RangeCreateFromCapDelegate(DTWAIN_SOURCE Source, int lCapType);
        public delegate int DTWAIN_RangeDestroyDelegate(DTWAIN_RANGE pSource);
        public delegate int DTWAIN_RangeExpandDelegate(DTWAIN_RANGE pSource, ref DTWAIN_ARRAY pArray);
        public delegate DTWAIN_ARRAY DTWAIN_RangeExpandExDelegate(DTWAIN_RANGE Range);
        public delegate int DTWAIN_RangeGetAllDelegate(DTWAIN_RANGE pArray, System.IntPtr pVariantLow, System.IntPtr pVariantUp, System.IntPtr pVariantStep, System.IntPtr pVariantDefault, System.IntPtr pVariantCurrent);
        public delegate int DTWAIN_RangeGetAllFloatDelegate(DTWAIN_RANGE pArray, ref DTWAIN_FLOAT pVariantLow, ref DTWAIN_FLOAT pVariantUp, ref DTWAIN_FLOAT pVariantStep, ref DTWAIN_FLOAT pVariantDefault, ref DTWAIN_FLOAT pVariantCurrent);
        public delegate int DTWAIN_RangeGetAllFloatStringDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dLow, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dUp, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dStep, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dDefault, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dCurrent);
        public delegate int DTWAIN_RangeGetAllFloatStringADelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dLow, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dUp, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dStep, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dDefault, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dCurrent);
        public delegate int DTWAIN_RangeGetAllFloatStringWDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dLow, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dUp, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dStep, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dDefault, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dCurrent);
        public delegate int DTWAIN_RangeGetAllLongDelegate(DTWAIN_RANGE pArray, ref int pVariantLow, ref int pVariantUp, ref int pVariantStep, ref int pVariantDefault, ref int pVariantCurrent);
        public delegate int DTWAIN_RangeGetCountDelegate(DTWAIN_RANGE pArray);
        public delegate int DTWAIN_RangeGetExpValueDelegate(DTWAIN_RANGE pArray, int lPos, System.IntPtr pVariant);
        public delegate int DTWAIN_RangeGetExpValueFloatDelegate(DTWAIN_RANGE pArray, int lPos, ref DTWAIN_FLOAT pVal);
        public delegate int DTWAIN_RangeGetExpValueFloatStringDelegate(DTWAIN_RANGE pArray, int lPos, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pVal);
        public delegate int DTWAIN_RangeGetExpValueFloatStringADelegate(DTWAIN_RANGE pArray, int lPos, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder pVal);
        public delegate int DTWAIN_RangeGetExpValueFloatStringWDelegate(DTWAIN_RANGE pArray, int lPos, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pVal);
        public delegate int DTWAIN_RangeGetExpValueLongDelegate(DTWAIN_RANGE pArray, int lPos, ref int pVal);
        public delegate int DTWAIN_RangeGetNearestValueDelegate(DTWAIN_RANGE pArray, System.IntPtr pVariantIn, System.IntPtr pVariantOut, int RoundType);
        public delegate int DTWAIN_RangeGetNearestValueFloatDelegate(DTWAIN_RANGE pArray, DTWAIN_FLOAT dIn, ref DTWAIN_FLOAT pOut, int RoundType);
        public delegate int DTWAIN_RangeGetNearestValueFloatStringDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] string dIn, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pOut, int RoundType);
        public delegate int DTWAIN_RangeGetNearestValueFloatStringADelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] string dIn, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dOut, int RoundType);
        public delegate int DTWAIN_RangeGetNearestValueFloatStringWDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] string dIn, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dOut, int RoundType);
        public delegate int DTWAIN_RangeGetNearestValueLongDelegate(DTWAIN_RANGE pArray, int lIn, ref int pOut, int RoundType);
        public delegate int DTWAIN_RangeGetPosDelegate(DTWAIN_RANGE pArray, System.IntPtr pVariant, ref int pPos);
        public delegate int DTWAIN_RangeGetPosFloatDelegate(DTWAIN_RANGE pArray, DTWAIN_FLOAT Val, ref int pPos);
        public delegate int DTWAIN_RangeGetPosFloatStringDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, ref int pPos);
        public delegate int DTWAIN_RangeGetPosFloatStringADelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, ref int pPos);
        public delegate int DTWAIN_RangeGetPosFloatStringWDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, ref int pPos);
        public delegate int DTWAIN_RangeGetPosLongDelegate(DTWAIN_RANGE pArray, int Value, ref int pPos);
        public delegate int DTWAIN_RangeGetValueDelegate(DTWAIN_RANGE pArray, int nWhich, System.IntPtr pVariant);
        public delegate int DTWAIN_RangeGetValueFloatDelegate(DTWAIN_RANGE pArray, int nWhich, ref DTWAIN_FLOAT pVal);
        public delegate int DTWAIN_RangeGetValueFloatStringDelegate(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pVal);
        public delegate int DTWAIN_RangeGetValueFloatStringADelegate(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dValue);
        public delegate int DTWAIN_RangeGetValueFloatStringWDelegate(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dValue);
        public delegate int DTWAIN_RangeGetValueLongDelegate(DTWAIN_RANGE pArray, int nWhich, ref int pVal);
        public delegate int DTWAIN_RangeIsValidDelegate(DTWAIN_RANGE Range, ref int pStatus);
        public delegate int DTWAIN_RangeSetAllDelegate(DTWAIN_RANGE pArray, System.IntPtr pVariantLow, System.IntPtr pVariantUp, System.IntPtr pVariantStep, System.IntPtr pVariantDefault, System.IntPtr pVariantCurrent);
        public delegate int DTWAIN_RangeSetAllFloatDelegate(DTWAIN_RANGE pArray, DTWAIN_FLOAT dLow, DTWAIN_FLOAT dUp, DTWAIN_FLOAT dStep, DTWAIN_FLOAT dDefault, DTWAIN_FLOAT dCurrent);
        public delegate int DTWAIN_RangeSetAllFloatStringDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] string dLow, [MarshalAs(UnmanagedType.LPTStr)] string dUp, [MarshalAs(UnmanagedType.LPTStr)] string dStep, [MarshalAs(UnmanagedType.LPTStr)] string dDefault, [MarshalAs(UnmanagedType.LPTStr)] string dCurrent);
        public delegate int DTWAIN_RangeSetAllFloatStringADelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] string dLow, [MarshalAs(UnmanagedType.LPStr)] string dUp, [MarshalAs(UnmanagedType.LPStr)] string dStep, [MarshalAs(UnmanagedType.LPStr)] string dDefault, [MarshalAs(UnmanagedType.LPStr)] string dCurrent);
        public delegate int DTWAIN_RangeSetAllFloatStringWDelegate(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] string dLow, [MarshalAs(UnmanagedType.LPWStr)] string dUp, [MarshalAs(UnmanagedType.LPWStr)] string dStep, [MarshalAs(UnmanagedType.LPWStr)] string dDefault, [MarshalAs(UnmanagedType.LPWStr)] string dCurrent);
        public delegate int DTWAIN_RangeSetAllLongDelegate(DTWAIN_RANGE pArray, int lLow, int lUp, int lStep, int lDefault, int lCurrent);
        public delegate int DTWAIN_RangeSetValueDelegate(DTWAIN_RANGE pArray, int nWhich, System.IntPtr pVal);
        public delegate int DTWAIN_RangeSetValueFloatDelegate(DTWAIN_RANGE pArray, int nWhich, DTWAIN_FLOAT Val);
        public delegate int DTWAIN_RangeSetValueFloatStringDelegate(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] string Val);
        public delegate int DTWAIN_RangeSetValueFloatStringADelegate(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPStr)] string dValue);
        public delegate int DTWAIN_RangeSetValueFloatStringWDelegate(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] string dValue);
        public delegate int DTWAIN_RangeSetValueLongDelegate(DTWAIN_RANGE pArray, int nWhich, int Val);
        public delegate int DTWAIN_RemovePDFTextElementDelegate(DTWAIN_SOURCE Source, DTWAIN_PDFTEXTELEMENT TextElement);
        public delegate int DTWAIN_ResetPDFTextElementDelegate(DTWAIN_PDFTEXTELEMENT TextElement);
        public delegate int DTWAIN_RewindPageDelegate(DTWAIN_SOURCE Source);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectDefaultOCREngineDelegate();
        public delegate DTWAIN_SOURCE DTWAIN_SelectDefaultSourceDelegate();
        public delegate DTWAIN_SOURCE DTWAIN_SelectDefaultSourceWithOpenDelegate(int bOpen);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngineDelegate();
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2Delegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, int nOptions);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2ADelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, int nOptions);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2ExDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string szIncludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szExcludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szNameMapping, int nOptions);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2ExADelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPStr)] string szNameMapping, int nOptions);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2ExWDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szNameMapping, int nOptions);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2WDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, int nOptions);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngineByNameDelegate([MarshalAs(UnmanagedType.LPTStr)] string lpszName);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngineByNameADelegate([MarshalAs(UnmanagedType.LPStr)] string lpszName);
        public delegate DTWAIN_OCRENGINE DTWAIN_SelectOCREngineByNameWDelegate([MarshalAs(UnmanagedType.LPWStr)] string lpszName);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceDelegate();
        public delegate DTWAIN_SOURCE DTWAIN_SelectSource2Delegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, int nOptions);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSource2ADelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, int nOptions);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSource2ExDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string szIncludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szExcludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szNameMapping, int nOptions);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSource2ExADelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPStr)] string szNameMapping, int nOptions);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSource2ExWDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szNameMapping, int nOptions);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSource2WDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, int nOptions);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceByNameDelegate([MarshalAs(UnmanagedType.LPTStr)] string lpszName);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceByNameADelegate([MarshalAs(UnmanagedType.LPStr)] string lpszName);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceByNameWDelegate([MarshalAs(UnmanagedType.LPWStr)] string lpszName);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceByNameWithOpenDelegate([MarshalAs(UnmanagedType.LPTStr)] string lpszName, int bOpen);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceByNameWithOpenADelegate([MarshalAs(UnmanagedType.LPStr)] string lpszName, int bOpen);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceByNameWithOpenWDelegate([MarshalAs(UnmanagedType.LPWStr)] string lpszName, int bOpen);
        public delegate DTWAIN_SOURCE DTWAIN_SelectSourceWithOpenDelegate(int bOpen);
        public delegate int DTWAIN_SetAcquireAreaDelegate(DTWAIN_SOURCE Source, int lSetType, DTWAIN_ARRAY FloatEnum, DTWAIN_ARRAY ActualEnum);
        public delegate int DTWAIN_SetAcquireArea2Delegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT left, DTWAIN_FLOAT top, DTWAIN_FLOAT right, DTWAIN_FLOAT bottom, int lUnit, int Flags);
        public delegate int DTWAIN_SetAcquireArea2StringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string left, [MarshalAs(UnmanagedType.LPTStr)] string top, [MarshalAs(UnmanagedType.LPTStr)] string right, [MarshalAs(UnmanagedType.LPTStr)] string bottom, int lUnit, int Flags);
        public delegate int DTWAIN_SetAcquireArea2StringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string left, [MarshalAs(UnmanagedType.LPStr)] string top, [MarshalAs(UnmanagedType.LPStr)] string right, [MarshalAs(UnmanagedType.LPStr)] string bottom, int lUnit, int Flags);
        public delegate int DTWAIN_SetAcquireArea2StringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string left, [MarshalAs(UnmanagedType.LPWStr)] string top, [MarshalAs(UnmanagedType.LPWStr)] string right, [MarshalAs(UnmanagedType.LPWStr)] string bottom, int lUnit, int Flags);
        public delegate int DTWAIN_SetAcquireImageNegativeDelegate(DTWAIN_SOURCE Source, int IsNegative);
        public delegate int DTWAIN_SetAcquireImageScaleDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT xscale, DTWAIN_FLOAT yscale);
        public delegate int DTWAIN_SetAcquireImageScaleStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string xscale, [MarshalAs(UnmanagedType.LPTStr)] string yscale);
        public delegate int DTWAIN_SetAcquireImageScaleStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string xscale, [MarshalAs(UnmanagedType.LPStr)] string yscale);
        public delegate int DTWAIN_SetAcquireImageScaleStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string xscale, [MarshalAs(UnmanagedType.LPWStr)] string yscale);
        public delegate int DTWAIN_SetAcquireStripBufferDelegate(DTWAIN_SOURCE Source, HANDLE hMem);
        public delegate int DTWAIN_SetAcquireStripSizeDelegate(DTWAIN_SOURCE Source, uint StripSize);
        public delegate int DTWAIN_SetAlarmVolumeDelegate(DTWAIN_SOURCE Source, int Volume);
        public delegate int DTWAIN_SetAlarmsDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY Alarms);
        public delegate int DTWAIN_SetAllCapsToDefaultDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_SetAppInfoDelegate([MarshalAs(UnmanagedType.LPTStr)] string szVerStr, [MarshalAs(UnmanagedType.LPTStr)] string szManu, [MarshalAs(UnmanagedType.LPTStr)] string szProdFam, [MarshalAs(UnmanagedType.LPTStr)] string szProdName);
        public delegate int DTWAIN_SetAppInfoADelegate([MarshalAs(UnmanagedType.LPStr)] string szVerStr, [MarshalAs(UnmanagedType.LPStr)] string szManu, [MarshalAs(UnmanagedType.LPStr)] string szProdFam, [MarshalAs(UnmanagedType.LPStr)] string szProdName);
        public delegate int DTWAIN_SetAppInfoWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szVerStr, [MarshalAs(UnmanagedType.LPWStr)] string szManu, [MarshalAs(UnmanagedType.LPWStr)] string szProdFam, [MarshalAs(UnmanagedType.LPWStr)] string szProdName);
        public delegate int DTWAIN_SetAuthorDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szAuthor);
        public delegate int DTWAIN_SetAuthorADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szAuthor);
        public delegate int DTWAIN_SetAuthorWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szAuthor);
        public delegate int DTWAIN_SetAvailablePrintersDelegate(DTWAIN_SOURCE Source, int lpAvailPrinters);
        public delegate int DTWAIN_SetAvailablePrintersArrayDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY AvailPrinters);
        public delegate int DTWAIN_SetBitDepthDelegate(DTWAIN_SOURCE Source, int BitDepth, int bSetCurrent);
        public delegate int DTWAIN_SetBlankPageDetectionDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT threshold, int discard_option, int bSet);
        public delegate int DTWAIN_SetBlankPageDetectionExDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT threshold, int autodetect, int detectOpts, int bSet);
        public delegate int DTWAIN_SetBlankPageDetectionExStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string threshold, int autodetect_option, int detectOpts, int bSet);
        public delegate int DTWAIN_SetBlankPageDetectionExStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string threshold, int autodetect_option, int detectOpts, int bSet);
        public delegate int DTWAIN_SetBlankPageDetectionExStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string threshold, int autodetect_option, int detectOpts, int bSet);
        public delegate int DTWAIN_SetBlankPageDetectionStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string threshold, int autodetect_option, int bSet);
        public delegate int DTWAIN_SetBlankPageDetectionStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string threshold, int autodetect_option, int bSet);
        public delegate int DTWAIN_SetBlankPageDetectionStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string threshold, int autodetect_option, int bSet);
        public delegate int DTWAIN_SetBrightnessDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Brightness);
        public delegate int DTWAIN_SetBrightnessStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Brightness);
        public delegate int DTWAIN_SetBrightnessStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Contrast);
        public delegate int DTWAIN_SetBrightnessStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Contrast);
        public delegate int DTWAIN_SetBufferedTileModeDelegate(DTWAIN_SOURCE Source, int bTileMode);
        public delegate DTwainCallback DTWAIN_SetCallbackDelegate(DTwainCallback Fn, int UserData);
        public delegate DTwainCallback64 DTWAIN_SetCallback64Delegate(DTwainCallback64 Fn, long UserData);
        public delegate int DTWAIN_SetCameraDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szCamera);
        public delegate int DTWAIN_SetCameraADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szCamera);
        public delegate int DTWAIN_SetCameraWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szCamera);
        public delegate int DTWAIN_SetCapValuesDelegate(DTWAIN_SOURCE Source, int lCap, int lSetType, DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_SetCapValuesExDelegate(DTWAIN_SOURCE Source, int lCap, int lSetType, int lContainerType, DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_SetCapValuesEx2Delegate(DTWAIN_SOURCE Source, int lCap, int lSetType, int lContainerType, int nDataType, DTWAIN_ARRAY pArray);
        public delegate int DTWAIN_SetCaptionDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Caption);
        public delegate int DTWAIN_SetCaptionADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Caption);
        public delegate int DTWAIN_SetCaptionWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Caption);
        public delegate int DTWAIN_SetCompressionTypeDelegate(DTWAIN_SOURCE Source, int lCompression, int bSetCurrent);
        public delegate int DTWAIN_SetContrastDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Contrast);
        public delegate int DTWAIN_SetContrastStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Contrast);
        public delegate int DTWAIN_SetContrastStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Contrast);
        public delegate int DTWAIN_SetContrastStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Contrast);
        public delegate int DTWAIN_SetCountryDelegate(int nCountry);
        public delegate int DTWAIN_SetCurrentRetryCountDelegate(DTWAIN_SOURCE Source, int nCount);
        public delegate int DTWAIN_SetCustomDSDataDelegate(DTWAIN_SOURCE Source, HANDLE hData, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U8, SizeParamIndex = 3)] byte[] Data, uint dSize, int nFlags);
        public delegate int DTWAIN_SetDSMSearchOrderDelegate(int SearchPath);
        public delegate int DTWAIN_SetDSMSearchOrderExDelegate([MarshalAs(UnmanagedType.LPTStr)] string SearchOrder, [MarshalAs(UnmanagedType.LPTStr)] string UserPath);
        public delegate int DTWAIN_SetDSMSearchOrderExADelegate([MarshalAs(UnmanagedType.LPStr)] string SearchOrder, [MarshalAs(UnmanagedType.LPStr)] string szUserPath);
        public delegate int DTWAIN_SetDSMSearchOrderExWDelegate([MarshalAs(UnmanagedType.LPWStr)] string SearchOrder, [MarshalAs(UnmanagedType.LPWStr)] string szUserPath);
        public delegate int DTWAIN_SetDefaultSourceDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_SetDeviceNotificationsDelegate(DTWAIN_SOURCE Source, int DevEvents);
        public delegate int DTWAIN_SetDeviceTimeDateDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szTimeDate);
        public delegate int DTWAIN_SetDeviceTimeDateADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szTimeDate);
        public delegate int DTWAIN_SetDeviceTimeDateWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szTimeDate);
        public delegate int DTWAIN_SetDoubleFeedDetectLengthDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Value);
        public delegate int DTWAIN_SetDoubleFeedDetectLengthStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string value);
        public delegate int DTWAIN_SetDoubleFeedDetectLengthStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szLength);
        public delegate int DTWAIN_SetDoubleFeedDetectLengthStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szLength);
        public delegate int DTWAIN_SetDoubleFeedDetectValuesDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY prray);
        public delegate int DTWAIN_SetDoublePageCountOnDuplexDelegate(DTWAIN_SOURCE Source, int bDoubleCount);
        public delegate int DTWAIN_SetEOJDetectValueDelegate(DTWAIN_SOURCE Source, int nValue);
        public delegate int DTWAIN_SetErrorBufferThresholdDelegate(int nErrors);
        public delegate int DTWAIN_SetErrorCallbackDelegate(DTwainErrorProc proc, int UserData);
        public delegate int DTWAIN_SetErrorCallback64Delegate(DTwainErrorProc64 proc, long UserData64);
        public delegate int DTWAIN_SetFeederAlignmentDelegate(DTWAIN_SOURCE Source, int lpAlignment);
        public delegate int DTWAIN_SetFeederOrderDelegate(DTWAIN_SOURCE Source, int lOrder);
        public delegate int DTWAIN_SetFeederWaitTimeDelegate(DTWAIN_SOURCE Source, int waitTime, int flags);
        public delegate int DTWAIN_SetFileAutoIncrementDelegate(DTWAIN_SOURCE Source, int Increment, int bResetOnAcquire, int bSet);
        public delegate int DTWAIN_SetFileCompressionTypeDelegate(DTWAIN_SOURCE Source, int lCompression, int bIsCustom);
        public delegate int DTWAIN_SetFileSavePosDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, int nFlags);
        public delegate int DTWAIN_SetFileSavePosADelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, int nFlags);
        public delegate int DTWAIN_SetFileSavePosWDelegate(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, int nFlags);
        public delegate int DTWAIN_SetFileXferFormatDelegate(DTWAIN_SOURCE Source, int lFileType, int bSetCurrent);
        public delegate int DTWAIN_SetHalftoneDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpHalftone);
        public delegate int DTWAIN_SetHalftoneADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpHalftone);
        public delegate int DTWAIN_SetHalftoneWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpHalftone);
        public delegate int DTWAIN_SetHighlightDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Highlight);
        public delegate int DTWAIN_SetHighlightStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Highlight);
        public delegate int DTWAIN_SetHighlightStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Highlight);
        public delegate int DTWAIN_SetHighlightStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Highlight);
        public delegate int DTWAIN_SetJobControlDelegate(DTWAIN_SOURCE Source, int JobControl, int bSetCurrent);
        public delegate int DTWAIN_SetJpegValuesDelegate(DTWAIN_SOURCE Source, int Quality, int Progressive);
        public delegate int DTWAIN_SetJpegXRValuesDelegate(DTWAIN_SOURCE Source, int Quality, int Progressive);
        public delegate int DTWAIN_SetLanguageDelegate(int nLanguage);
        public delegate int DTWAIN_SetLastErrorDelegate(int nError);
        public delegate int DTWAIN_SetLightPathDelegate(DTWAIN_SOURCE Source, int LightPath);
        public delegate int DTWAIN_SetLightPathExDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY LightPaths);
        public delegate int DTWAIN_SetLightSourceDelegate(DTWAIN_SOURCE Source, int LightSource);
        public delegate int DTWAIN_SetLightSourcesDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY LightSources);
        public delegate int DTWAIN_SetLoggerCallbackDelegate(DTwainLoggerProc logProc, long UserData);
        public delegate int DTWAIN_SetLoggerCallbackADelegate(DTwainLoggerProcA logProc, long UserData);
        public delegate int DTWAIN_SetLoggerCallbackWDelegate(DTwainLoggerProcW logProc, long UserData);
        public delegate int DTWAIN_SetManualDuplexModeDelegate(DTWAIN_SOURCE Source, int Flags, int bSet);
        public delegate int DTWAIN_SetMaxAcquisitionsDelegate(DTWAIN_SOURCE Source, int MaxAcquires);
        public delegate int DTWAIN_SetMaxBuffersDelegate(DTWAIN_SOURCE Source, int MaxBuf);
        public delegate int DTWAIN_SetMaxRetryAttemptsDelegate(DTWAIN_SOURCE Source, int nAttempts);
        public delegate int DTWAIN_SetMultipageScanModeDelegate(DTWAIN_SOURCE Source, int ScanType);
        public delegate int DTWAIN_SetNoiseFilterDelegate(DTWAIN_SOURCE Source, int NoiseFilter);
        public delegate int DTWAIN_SetOCRCapValuesDelegate(DTWAIN_OCRENGINE Engine, int OCRCapValue, int SetType, DTWAIN_ARRAY CapValues);
        public delegate int DTWAIN_SetOrientationDelegate(DTWAIN_SOURCE Source, int Orient, int bSetCurrent);
        public delegate int DTWAIN_SetOverscanDelegate(DTWAIN_SOURCE Source, int Value, int bSetCurrent);
        public delegate int DTWAIN_SetPDFAESEncryptionDelegate(DTWAIN_SOURCE Source, int nWhichEncryption, int bUseAES);
        public delegate int DTWAIN_SetPDFASCIICompressionDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_SetPDFAuthorDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpAuthor);
        public delegate int DTWAIN_SetPDFAuthorADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpAuthor);
        public delegate int DTWAIN_SetPDFAuthorWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpAuthor);
        public delegate int DTWAIN_SetPDFCompressionDelegate(DTWAIN_SOURCE Source, int bCompression);
        public delegate int DTWAIN_SetPDFCreatorDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpCreator);
        public delegate int DTWAIN_SetPDFCreatorADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpCreator);
        public delegate int DTWAIN_SetPDFCreatorWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpCreator);
        public delegate int DTWAIN_SetPDFEncryptionDelegate(DTWAIN_SOURCE Source, int bUseEncryption, [MarshalAs(UnmanagedType.LPTStr)] string lpszUser, [MarshalAs(UnmanagedType.LPTStr)] string lpszOwner, uint Permissions, int UseStrongEncryption);
        public delegate int DTWAIN_SetPDFEncryptionADelegate(DTWAIN_SOURCE Source, int bUseEncryption, [MarshalAs(UnmanagedType.LPStr)] string lpszUser, [MarshalAs(UnmanagedType.LPStr)] string lpszOwner, uint Permissions, int UseStrongEncryption);
        public delegate int DTWAIN_SetPDFEncryptionWDelegate(DTWAIN_SOURCE Source, int bUseEncryption, [MarshalAs(UnmanagedType.LPWStr)] string lpszUser, [MarshalAs(UnmanagedType.LPWStr)] string lpszOwner, uint Permissions, int UseStrongEncryption);
        public delegate int DTWAIN_SetPDFJpegQualityDelegate(DTWAIN_SOURCE Source, int Quality);
        public delegate int DTWAIN_SetPDFKeywordsDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpKeyWords);
        public delegate int DTWAIN_SetPDFKeywordsADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpKeyWords);
        public delegate int DTWAIN_SetPDFKeywordsWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpKeyWords);
        public delegate int DTWAIN_SetPDFOCRConversionDelegate(DTWAIN_OCRENGINE Engine, int PageType, int FileType, int PixelType, int BitDepth, int Options);
        public delegate int DTWAIN_SetPDFOCRModeDelegate(DTWAIN_SOURCE Source, int bSet);
        public delegate int DTWAIN_SetPDFOrientationDelegate(DTWAIN_SOURCE Source, int lPOrientation);
        public delegate int DTWAIN_SetPDFPageScaleDelegate(DTWAIN_SOURCE Source, int nOptions, DTWAIN_FLOAT xScale, DTWAIN_FLOAT yScale);
        public delegate int DTWAIN_SetPDFPageScaleStringDelegate(DTWAIN_SOURCE Source, int nOptions, [MarshalAs(UnmanagedType.LPTStr)] string xScale, [MarshalAs(UnmanagedType.LPTStr)] string yScale);
        public delegate int DTWAIN_SetPDFPageScaleStringADelegate(DTWAIN_SOURCE Source, int nOptions, [MarshalAs(UnmanagedType.LPStr)] string xScale, [MarshalAs(UnmanagedType.LPStr)] string yScale);
        public delegate int DTWAIN_SetPDFPageScaleStringWDelegate(DTWAIN_SOURCE Source, int nOptions, [MarshalAs(UnmanagedType.LPWStr)] string xScale, [MarshalAs(UnmanagedType.LPWStr)] string yScale);
        public delegate int DTWAIN_SetPDFPageSizeDelegate(DTWAIN_SOURCE Source, int PageSize, DTWAIN_FLOAT CustomWidth, DTWAIN_FLOAT CustomHeight);
        public delegate int DTWAIN_SetPDFPageSizeStringDelegate(DTWAIN_SOURCE Source, int PageSize, [MarshalAs(UnmanagedType.LPTStr)] string CustomWidth, [MarshalAs(UnmanagedType.LPTStr)] string CustomHeight);
        public delegate int DTWAIN_SetPDFPageSizeStringADelegate(DTWAIN_SOURCE Source, int PageSize, [MarshalAs(UnmanagedType.LPStr)] string CustomWidth, [MarshalAs(UnmanagedType.LPStr)] string CustomHeight);
        public delegate int DTWAIN_SetPDFPageSizeStringWDelegate(DTWAIN_SOURCE Source, int PageSize, [MarshalAs(UnmanagedType.LPWStr)] string CustomWidth, [MarshalAs(UnmanagedType.LPWStr)] string CustomHeight);
        public delegate int DTWAIN_SetPDFPolarityDelegate(DTWAIN_SOURCE Source, int Polarity);
        public delegate int DTWAIN_SetPDFProducerDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpProducer);
        public delegate int DTWAIN_SetPDFProducerADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpProducer);
        public delegate int DTWAIN_SetPDFProducerWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpProducer);
        public delegate int DTWAIN_SetPDFSubjectDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpSubject);
        public delegate int DTWAIN_SetPDFSubjectADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpSubject);
        public delegate int DTWAIN_SetPDFSubjectWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpSubject);
        public delegate int DTWAIN_SetPDFTextElementFloatDelegate(DTWAIN_PDFTEXTELEMENT TextElement, DTWAIN_FLOAT val1, DTWAIN_FLOAT val2, int Flags);
        public delegate int DTWAIN_SetPDFTextElementFloatStringDelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPTStr)] string val1, [MarshalAs(UnmanagedType.LPTStr)] string val2, int Flags);
        public delegate int DTWAIN_SetPDFTextElementFloatStringADelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPStr)] string val1, [MarshalAs(UnmanagedType.LPStr)] string val2, int Flags);
        public delegate int DTWAIN_SetPDFTextElementFloatStringWDelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPWStr)] string val1, [MarshalAs(UnmanagedType.LPWStr)] string val2, int Flags);
        public delegate int DTWAIN_SetPDFTextElementLongDelegate(DTWAIN_PDFTEXTELEMENT TextElement, int val1, int val2, int Flags);
        public delegate int DTWAIN_SetPDFTextElementStringDelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPTStr)] string val1, int Flags);
        public delegate int DTWAIN_SetPDFTextElementStringADelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPStr)] string szString, int Flags);
        public delegate int DTWAIN_SetPDFTextElementStringWDelegate(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPWStr)] string szString, int Flags);
        public delegate int DTWAIN_SetPDFTitleDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpTitle);
        public delegate int DTWAIN_SetPDFTitleADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpTitle);
        public delegate int DTWAIN_SetPDFTitleWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpTitle);
        public delegate int DTWAIN_SetPaperSizeDelegate(DTWAIN_SOURCE Source, int PaperSize, int bSetCurrent);
        public delegate int DTWAIN_SetPatchMaxPrioritiesDelegate(DTWAIN_SOURCE Source, int nMaxSearchRetries);
        public delegate int DTWAIN_SetPatchMaxRetriesDelegate(DTWAIN_SOURCE Source, int nMaxRetries);
        public delegate int DTWAIN_SetPatchPrioritiesDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY SearchPriorities);
        public delegate int DTWAIN_SetPatchSearchModeDelegate(DTWAIN_SOURCE Source, int nSearchMode);
        public delegate int DTWAIN_SetPatchTimeOutDelegate(DTWAIN_SOURCE Source, int TimeOutValue);
        public delegate int DTWAIN_SetPixelFlavorDelegate(DTWAIN_SOURCE Source, int PixelFlavor);
        public delegate int DTWAIN_SetPixelTypeDelegate(DTWAIN_SOURCE Source, int PixelType, int BitDepth, int bSetCurrent);
        public delegate int DTWAIN_SetPostScriptTitleDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szTitle);
        public delegate int DTWAIN_SetPostScriptTitleADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szTitle);
        public delegate int DTWAIN_SetPostScriptTitleWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szTitle);
        public delegate int DTWAIN_SetPostScriptTypeDelegate(DTWAIN_SOURCE Source, int PSType);
        public delegate int DTWAIN_SetPrinterDelegate(DTWAIN_SOURCE Source, int Printer, int bCurrent);
        public delegate int DTWAIN_SetPrinterExDelegate(DTWAIN_SOURCE Source, int Printer, int bCurrent);
        public delegate int DTWAIN_SetPrinterStartNumberDelegate(DTWAIN_SOURCE Source, int nStart);
        public delegate int DTWAIN_SetPrinterStringModeDelegate(DTWAIN_SOURCE Source, int PrinterMode, int bSetCurrent);
        public delegate int DTWAIN_SetPrinterStringsDelegate(DTWAIN_SOURCE Source, DTWAIN_ARRAY ArrayString, ref int pNumStrings);
        public delegate int DTWAIN_SetPrinterSuffixStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Suffix);
        public delegate int DTWAIN_SetPrinterSuffixStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Suffix);
        public delegate int DTWAIN_SetPrinterSuffixStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Suffix);
        public delegate int DTWAIN_SetQueryCapSupportDelegate(int bSet);
        public delegate int DTWAIN_SetResolutionDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Resolution);
        public delegate int DTWAIN_SetResolutionStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Resolution);
        public delegate int DTWAIN_SetResolutionStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Resolution);
        public delegate int DTWAIN_SetResolutionStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Resolution);
        public delegate int DTWAIN_SetResourcePathDelegate([MarshalAs(UnmanagedType.LPTStr)] string ResourcePath);
        public delegate int DTWAIN_SetResourcePathADelegate([MarshalAs(UnmanagedType.LPStr)] string ResourcePath);
        public delegate int DTWAIN_SetResourcePathWDelegate([MarshalAs(UnmanagedType.LPWStr)] string ResourcePath);
        public delegate int DTWAIN_SetRotationDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Rotation);
        public delegate int DTWAIN_SetRotationStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Rotation);
        public delegate int DTWAIN_SetRotationStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Rotation);
        public delegate int DTWAIN_SetRotationStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Rotation);
        public delegate int DTWAIN_SetSaveFileNameDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string fName);
        public delegate int DTWAIN_SetSaveFileNameADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string fName);
        public delegate int DTWAIN_SetSaveFileNameWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string fName);
        public delegate int DTWAIN_SetShadowDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Shadow);
        public delegate int DTWAIN_SetShadowStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Shadow);
        public delegate int DTWAIN_SetShadowStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Shadow);
        public delegate int DTWAIN_SetShadowStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Shadow);
        public delegate int DTWAIN_SetSourceUnitDelegate(DTWAIN_SOURCE Source, int Unit);
        public delegate int DTWAIN_SetTIFFCompressTypeDelegate(DTWAIN_SOURCE Source, int Setting);
        public delegate int DTWAIN_SetTIFFInvertDelegate(DTWAIN_SOURCE Source, int Setting);
        public delegate int DTWAIN_SetTempFileDirectoryDelegate([MarshalAs(UnmanagedType.LPTStr)] string szFilePath);
        public delegate int DTWAIN_SetTempFileDirectoryADelegate([MarshalAs(UnmanagedType.LPStr)] string szFilePath);
        public delegate int DTWAIN_SetTempFileDirectoryExDelegate([MarshalAs(UnmanagedType.LPTStr)] string szFilePath, int CreationFlags);
        public delegate int DTWAIN_SetTempFileDirectoryExADelegate([MarshalAs(UnmanagedType.LPStr)] string szFilePath, int CreationFlags);
        public delegate int DTWAIN_SetTempFileDirectoryExWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szFilePath, int CreationFlags);
        public delegate int DTWAIN_SetTempFileDirectoryWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szFilePath);
        public delegate int DTWAIN_SetThresholdDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT Threshold, int bSetBithDepthReduction);
        public delegate int DTWAIN_SetThresholdStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Threshold, int bSetBitDepthReduction);
        public delegate int DTWAIN_SetThresholdStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Threshold, int bSetBitDepthReduction);
        public delegate int DTWAIN_SetThresholdStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Threshold, int bSetBitDepthReduction);
        public delegate int DTWAIN_SetTwainDSMDelegate(int DSMType);
        public delegate int DTWAIN_SetTwainLogDelegate(uint LogFlags, [MarshalAs(UnmanagedType.LPTStr)] string lpszLogFile);
        public delegate int DTWAIN_SetTwainLogADelegate(uint LogFlags, [MarshalAs(UnmanagedType.LPStr)] string lpszLogFile);
        public delegate int DTWAIN_SetTwainLogWDelegate(uint LogFlags, [MarshalAs(UnmanagedType.LPWStr)] string lpszLogFile);
        public delegate int DTWAIN_SetTwainModeDelegate(int lAcquireMode);
        public delegate int DTWAIN_SetTwainTimeoutDelegate(int milliseconds);
        public delegate DTwainDIBUpdateProc DTWAIN_SetUpdateDibProcDelegate(DTwainDIBUpdateProc DibProc);
        public delegate int DTWAIN_SetXResolutionDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT xResolution);
        public delegate int DTWAIN_SetXResolutionStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Resolution);
        public delegate int DTWAIN_SetXResolutionStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Resolution);
        public delegate int DTWAIN_SetXResolutionStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Resolution);
        public delegate int DTWAIN_SetYResolutionDelegate(DTWAIN_SOURCE Source, DTWAIN_FLOAT yResolution);
        public delegate int DTWAIN_SetYResolutionStringDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Resolution);
        public delegate int DTWAIN_SetYResolutionStringADelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Resolution);
        public delegate int DTWAIN_SetYResolutionStringWDelegate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Resolution);
        public delegate int DTWAIN_ShowUIOnlyDelegate(DTWAIN_SOURCE Source);
        public delegate int DTWAIN_ShutdownOCREngineDelegate(DTWAIN_OCRENGINE OCREngine);
        public delegate int DTWAIN_SkipImageInfoErrorDelegate(DTWAIN_SOURCE Source, int bSkip);
        public delegate int DTWAIN_StartThreadDelegate(DTWAIN_HANDLE DLLHandle);
        public delegate int DTWAIN_StartTwainSessionDelegate(HWND hWndMsg, [MarshalAs(UnmanagedType.LPTStr)] string lpszDLLName);
        public delegate int DTWAIN_StartTwainSessionADelegate(HWND hWndMsg, [MarshalAs(UnmanagedType.LPStr)] string lpszDLLName);
        public delegate int DTWAIN_StartTwainSessionWDelegate(HWND hWndMsg, [MarshalAs(UnmanagedType.LPWStr)] string lpszDLLName);
        public delegate int DTWAIN_SysDestroyDelegate();
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeDelegate();
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeExDelegate([MarshalAs(UnmanagedType.LPTStr)] string szINIPath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeEx2Delegate([MarshalAs(UnmanagedType.LPTStr)] string szINIPath, [MarshalAs(UnmanagedType.LPTStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPTStr)] string szLangResourcePath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeEx2ADelegate([MarshalAs(UnmanagedType.LPStr)] string szINIPath, [MarshalAs(UnmanagedType.LPStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPStr)] string szLangResourcePath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeEx2WDelegate([MarshalAs(UnmanagedType.LPWStr)] string szINIPath, [MarshalAs(UnmanagedType.LPWStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPWStr)] string szLangResourcePath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeExADelegate([MarshalAs(UnmanagedType.LPStr)] string szINIPath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeExWDelegate([MarshalAs(UnmanagedType.LPWStr)] string szINIPath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeLibDelegate(HINSTANCE hInstance);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeLibExDelegate(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPTStr)] string szINIPath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeLibEx2Delegate(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPTStr)] string szINIPath, [MarshalAs(UnmanagedType.LPTStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPTStr)] string szLangResourcePath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeLibEx2ADelegate(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPStr)] string szINIPath, [MarshalAs(UnmanagedType.LPStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPStr)] string szLangResourcePath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeLibEx2WDelegate(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPWStr)] string szINIPath, [MarshalAs(UnmanagedType.LPWStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPWStr)] string szLangResourcePath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeLibExADelegate(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPStr)] string szINIPath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeLibExWDelegate(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPWStr)] string szINIPath);
        public delegate DTWAIN_HANDLE DTWAIN_SysInitializeNoBlockingDelegate();
        public delegate DTWAIN_ARRAY DTWAIN_TestGetCapDelegate(DTWAIN_SOURCE Source, int lCapability);
        public delegate int DTWAIN_UnlockMemoryDelegate(HANDLE h);
        public delegate int DTWAIN_UnlockMemoryExDelegate(HANDLE h);
        public delegate int DTWAIN_UseMultipleThreadsDelegate(int bSet);

        public TwainAPI(string dllPath)
            : base(dllPath) 
        {
            string errMsg= GetErrorMessage();
            bool isEmpty = String.IsNullOrEmpty(errMsg);
            if (!isEmpty)
            {
                if (errMsg.StartsWith("Failed"))
                    throw new DllNotFoundException(errMsg);
                throw new ArgumentOutOfRangeException(errMsg);
            }
        }

        [DTWAINNativeFunction("DTWAIN_AcquireAudioFile")]
        private readonly DTWAIN_AcquireAudioFileDelegate  _DTWAIN_AcquireAudioFile;

        [DTWAINNativeFunction("DTWAIN_AcquireAudioFileA")]
        private readonly DTWAIN_AcquireAudioFileADelegate  _DTWAIN_AcquireAudioFileA;

        [DTWAINNativeFunction("DTWAIN_AcquireAudioFileW")]
        private readonly DTWAIN_AcquireAudioFileWDelegate  _DTWAIN_AcquireAudioFileW;

        [DTWAINNativeFunction("DTWAIN_AcquireAudioNative")]
        private readonly DTWAIN_AcquireAudioNativeDelegate  _DTWAIN_AcquireAudioNative;

        [DTWAINNativeFunction("DTWAIN_AcquireAudioNativeEx")]
        private readonly DTWAIN_AcquireAudioNativeExDelegate  _DTWAIN_AcquireAudioNativeEx;

        [DTWAINNativeFunction("DTWAIN_AcquireBuffered")]
        private readonly DTWAIN_AcquireBufferedDelegate  _DTWAIN_AcquireBuffered;

        [DTWAINNativeFunction("DTWAIN_AcquireBufferedEx")]
        private readonly DTWAIN_AcquireBufferedExDelegate  _DTWAIN_AcquireBufferedEx;

        [DTWAINNativeFunction("DTWAIN_AcquireFile")]
        private readonly DTWAIN_AcquireFileDelegate  _DTWAIN_AcquireFile;

        [DTWAINNativeFunction("DTWAIN_AcquireFileA")]
        private readonly DTWAIN_AcquireFileADelegate  _DTWAIN_AcquireFileA;

        [DTWAINNativeFunction("DTWAIN_AcquireFileEx")]
        private readonly DTWAIN_AcquireFileExDelegate  _DTWAIN_AcquireFileEx;

        [DTWAINNativeFunction("DTWAIN_AcquireFileW")]
        private readonly DTWAIN_AcquireFileWDelegate  _DTWAIN_AcquireFileW;

        [DTWAINNativeFunction("DTWAIN_AcquireNative")]
        private readonly DTWAIN_AcquireNativeDelegate  _DTWAIN_AcquireNative;

        [DTWAINNativeFunction("DTWAIN_AcquireNativeEx")]
        private readonly DTWAIN_AcquireNativeExDelegate  _DTWAIN_AcquireNativeEx;

        [DTWAINNativeFunction("DTWAIN_AcquireToClipboard")]
        private readonly DTWAIN_AcquireToClipboardDelegate  _DTWAIN_AcquireToClipboard;

        [DTWAINNativeFunction("DTWAIN_AddExtImageInfoQuery")]
        private readonly DTWAIN_AddExtImageInfoQueryDelegate  _DTWAIN_AddExtImageInfoQuery;

        [DTWAINNativeFunction("DTWAIN_AddFileToAppend")]
        private readonly DTWAIN_AddFileToAppendDelegate  _DTWAIN_AddFileToAppend;

        [DTWAINNativeFunction("DTWAIN_AddFileToAppendA")]
        private readonly DTWAIN_AddFileToAppendADelegate  _DTWAIN_AddFileToAppendA;

        [DTWAINNativeFunction("DTWAIN_AddFileToAppendW")]
        private readonly DTWAIN_AddFileToAppendWDelegate  _DTWAIN_AddFileToAppendW;

        [DTWAINNativeFunction("DTWAIN_AddPDFText")]
        private readonly DTWAIN_AddPDFTextDelegate  _DTWAIN_AddPDFText;

        [DTWAINNativeFunction("DTWAIN_AddPDFTextA")]
        private readonly DTWAIN_AddPDFTextADelegate  _DTWAIN_AddPDFTextA;

        [DTWAINNativeFunction("DTWAIN_AddPDFTextElement")]
        private readonly DTWAIN_AddPDFTextElementDelegate  _DTWAIN_AddPDFTextElement;

        [DTWAINNativeFunction("DTWAIN_AddPDFTextString")]
        private readonly DTWAIN_AddPDFTextStringDelegate  _DTWAIN_AddPDFTextString;

        [DTWAINNativeFunction("DTWAIN_AddPDFTextStringA")]
        private readonly DTWAIN_AddPDFTextStringADelegate  _DTWAIN_AddPDFTextStringA;

        [DTWAINNativeFunction("DTWAIN_AddPDFTextStringW")]
        private readonly DTWAIN_AddPDFTextStringWDelegate  _DTWAIN_AddPDFTextStringW;

        [DTWAINNativeFunction("DTWAIN_AddPDFTextW")]
        private readonly DTWAIN_AddPDFTextWDelegate  _DTWAIN_AddPDFTextW;

        [DTWAINNativeFunction("DTWAIN_AllocateMemory")]
        private readonly DTWAIN_AllocateMemoryDelegate  _DTWAIN_AllocateMemory;

        [DTWAINNativeFunction("DTWAIN_AllocateMemory64")]
        private readonly DTWAIN_AllocateMemory64Delegate  _DTWAIN_AllocateMemory64;

        [DTWAINNativeFunction("DTWAIN_AllocateMemoryEx")]
        private readonly DTWAIN_AllocateMemoryExDelegate  _DTWAIN_AllocateMemoryEx;

        [DTWAINNativeFunction("DTWAIN_AppHandlesExceptions")]
        private readonly DTWAIN_AppHandlesExceptionsDelegate  _DTWAIN_AppHandlesExceptions;

        [DTWAINNativeFunction("DTWAIN_ArrayANSIStringToFloat")]
        private readonly DTWAIN_ArrayANSIStringToFloatDelegate  _DTWAIN_ArrayANSIStringToFloat;

        [DTWAINNativeFunction("DTWAIN_ArrayAdd")]
        private readonly DTWAIN_ArrayAddDelegate  _DTWAIN_ArrayAdd;

        [DTWAINNativeFunction("DTWAIN_ArrayAddANSIString")]
        private readonly DTWAIN_ArrayAddANSIStringDelegate  _DTWAIN_ArrayAddANSIString;

        [DTWAINNativeFunction("DTWAIN_ArrayAddANSIStringN")]
        private readonly DTWAIN_ArrayAddANSIStringNDelegate  _DTWAIN_ArrayAddANSIStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloat")]
        private readonly DTWAIN_ArrayAddFloatDelegate  _DTWAIN_ArrayAddFloat;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloatN")]
        private readonly DTWAIN_ArrayAddFloatNDelegate  _DTWAIN_ArrayAddFloatN;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloatString")]
        private readonly DTWAIN_ArrayAddFloatStringDelegate  _DTWAIN_ArrayAddFloatString;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloatStringA")]
        private readonly DTWAIN_ArrayAddFloatStringADelegate  _DTWAIN_ArrayAddFloatStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloatStringN")]
        private readonly DTWAIN_ArrayAddFloatStringNDelegate  _DTWAIN_ArrayAddFloatStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloatStringNA")]
        private readonly DTWAIN_ArrayAddFloatStringNADelegate  _DTWAIN_ArrayAddFloatStringNA;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloatStringNW")]
        private readonly DTWAIN_ArrayAddFloatStringNWDelegate  _DTWAIN_ArrayAddFloatStringNW;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFloatStringW")]
        private readonly DTWAIN_ArrayAddFloatStringWDelegate  _DTWAIN_ArrayAddFloatStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFrame")]
        private readonly DTWAIN_ArrayAddFrameDelegate  _DTWAIN_ArrayAddFrame;

        [DTWAINNativeFunction("DTWAIN_ArrayAddFrameN")]
        private readonly DTWAIN_ArrayAddFrameNDelegate  _DTWAIN_ArrayAddFrameN;

        [DTWAINNativeFunction("DTWAIN_ArrayAddLong")]
        private readonly DTWAIN_ArrayAddLongDelegate  _DTWAIN_ArrayAddLong;

        [DTWAINNativeFunction("DTWAIN_ArrayAddLong64")]
        private readonly DTWAIN_ArrayAddLong64Delegate  _DTWAIN_ArrayAddLong64;

        [DTWAINNativeFunction("DTWAIN_ArrayAddLong64N")]
        private readonly DTWAIN_ArrayAddLong64NDelegate  _DTWAIN_ArrayAddLong64N;

        [DTWAINNativeFunction("DTWAIN_ArrayAddLongN")]
        private readonly DTWAIN_ArrayAddLongNDelegate  _DTWAIN_ArrayAddLongN;

        [DTWAINNativeFunction("DTWAIN_ArrayAddN")]
        private readonly DTWAIN_ArrayAddNDelegate  _DTWAIN_ArrayAddN;

        [DTWAINNativeFunction("DTWAIN_ArrayAddString")]
        private readonly DTWAIN_ArrayAddStringDelegate  _DTWAIN_ArrayAddString;

        [DTWAINNativeFunction("DTWAIN_ArrayAddStringA")]
        private readonly DTWAIN_ArrayAddStringADelegate  _DTWAIN_ArrayAddStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayAddStringN")]
        private readonly DTWAIN_ArrayAddStringNDelegate  _DTWAIN_ArrayAddStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayAddStringNA")]
        private readonly DTWAIN_ArrayAddStringNADelegate  _DTWAIN_ArrayAddStringNA;

        [DTWAINNativeFunction("DTWAIN_ArrayAddStringNW")]
        private readonly DTWAIN_ArrayAddStringNWDelegate  _DTWAIN_ArrayAddStringNW;

        [DTWAINNativeFunction("DTWAIN_ArrayAddStringW")]
        private readonly DTWAIN_ArrayAddStringWDelegate  _DTWAIN_ArrayAddStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayAddWideString")]
        private readonly DTWAIN_ArrayAddWideStringDelegate  _DTWAIN_ArrayAddWideString;

        [DTWAINNativeFunction("DTWAIN_ArrayAddWideStringN")]
        private readonly DTWAIN_ArrayAddWideStringNDelegate  _DTWAIN_ArrayAddWideStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayConvertFix32ToFloat")]
        private readonly DTWAIN_ArrayConvertFix32ToFloatDelegate  _DTWAIN_ArrayConvertFix32ToFloat;

        [DTWAINNativeFunction("DTWAIN_ArrayConvertFloatToFix32")]
        private readonly DTWAIN_ArrayConvertFloatToFix32Delegate  _DTWAIN_ArrayConvertFloatToFix32;

        [DTWAINNativeFunction("DTWAIN_ArrayCopy")]
        private readonly DTWAIN_ArrayCopyDelegate  _DTWAIN_ArrayCopy;

        [DTWAINNativeFunction("DTWAIN_ArrayCreate")]
        private readonly DTWAIN_ArrayCreateDelegate  _DTWAIN_ArrayCreate;

        [DTWAINNativeFunction("DTWAIN_ArrayCreateCopy")]
        private readonly DTWAIN_ArrayCreateCopyDelegate  _DTWAIN_ArrayCreateCopy;

        [DTWAINNativeFunction("DTWAIN_ArrayCreateFromCap")]
        private readonly DTWAIN_ArrayCreateFromCapDelegate  _DTWAIN_ArrayCreateFromCap;

        [DTWAINNativeFunction("DTWAIN_ArrayCreateFromLong64s")]
        private readonly DTWAIN_ArrayCreateFromLong64sDelegate  _DTWAIN_ArrayCreateFromLong64s;

        [DTWAINNativeFunction("DTWAIN_ArrayCreateFromLongs")]
        private readonly DTWAIN_ArrayCreateFromLongsDelegate  _DTWAIN_ArrayCreateFromLongs;

        [DTWAINNativeFunction("DTWAIN_ArrayCreateFromReals")]
        private readonly DTWAIN_ArrayCreateFromRealsDelegate  _DTWAIN_ArrayCreateFromReals;

        [DTWAINNativeFunction("DTWAIN_ArrayDestroy")]
        private readonly DTWAIN_ArrayDestroyDelegate  _DTWAIN_ArrayDestroy;

        [DTWAINNativeFunction("DTWAIN_ArrayDestroyFrames")]
        private readonly DTWAIN_ArrayDestroyFramesDelegate  _DTWAIN_ArrayDestroyFrames;

        [DTWAINNativeFunction("DTWAIN_ArrayFind")]
        private readonly DTWAIN_ArrayFindDelegate  _DTWAIN_ArrayFind;

        [DTWAINNativeFunction("DTWAIN_ArrayFindANSIString")]
        private readonly DTWAIN_ArrayFindANSIStringDelegate  _DTWAIN_ArrayFindANSIString;

        [DTWAINNativeFunction("DTWAIN_ArrayFindFloat")]
        private readonly DTWAIN_ArrayFindFloatDelegate  _DTWAIN_ArrayFindFloat;

        [DTWAINNativeFunction("DTWAIN_ArrayFindFloatString")]
        private readonly DTWAIN_ArrayFindFloatStringDelegate  _DTWAIN_ArrayFindFloatString;

        [DTWAINNativeFunction("DTWAIN_ArrayFindFloatStringA")]
        private readonly DTWAIN_ArrayFindFloatStringADelegate  _DTWAIN_ArrayFindFloatStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayFindFloatStringW")]
        private readonly DTWAIN_ArrayFindFloatStringWDelegate  _DTWAIN_ArrayFindFloatStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayFindLong")]
        private readonly DTWAIN_ArrayFindLongDelegate  _DTWAIN_ArrayFindLong;

        [DTWAINNativeFunction("DTWAIN_ArrayFindLong64")]
        private readonly DTWAIN_ArrayFindLong64Delegate  _DTWAIN_ArrayFindLong64;

        [DTWAINNativeFunction("DTWAIN_ArrayFindString")]
        private readonly DTWAIN_ArrayFindStringDelegate  _DTWAIN_ArrayFindString;

        [DTWAINNativeFunction("DTWAIN_ArrayFindStringA")]
        private readonly DTWAIN_ArrayFindStringADelegate  _DTWAIN_ArrayFindStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayFindStringW")]
        private readonly DTWAIN_ArrayFindStringWDelegate  _DTWAIN_ArrayFindStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayFindWideString")]
        private readonly DTWAIN_ArrayFindWideStringDelegate  _DTWAIN_ArrayFindWideString;

        [DTWAINNativeFunction("DTWAIN_ArrayFix32GetAt")]
        private readonly DTWAIN_ArrayFix32GetAtDelegate  _DTWAIN_ArrayFix32GetAt;

        [DTWAINNativeFunction("DTWAIN_ArrayFix32SetAt")]
        private readonly DTWAIN_ArrayFix32SetAtDelegate  _DTWAIN_ArrayFix32SetAt;

        [DTWAINNativeFunction("DTWAIN_ArrayFloatToANSIString")]
        private readonly DTWAIN_ArrayFloatToANSIStringDelegate  _DTWAIN_ArrayFloatToANSIString;

        [DTWAINNativeFunction("DTWAIN_ArrayFloatToString")]
        private readonly DTWAIN_ArrayFloatToStringDelegate  _DTWAIN_ArrayFloatToString;

        [DTWAINNativeFunction("DTWAIN_ArrayFloatToWideString")]
        private readonly DTWAIN_ArrayFloatToWideStringDelegate  _DTWAIN_ArrayFloatToWideString;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAt")]
        private readonly DTWAIN_ArrayGetAtDelegate  _DTWAIN_ArrayGetAt;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtANSIString")]
        private readonly DTWAIN_ArrayGetAtANSIStringDelegate  _DTWAIN_ArrayGetAtANSIString;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFloat")]
        private readonly DTWAIN_ArrayGetAtFloatDelegate  _DTWAIN_ArrayGetAtFloat;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFloatString")]
        private readonly DTWAIN_ArrayGetAtFloatStringDelegate  _DTWAIN_ArrayGetAtFloatString;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFloatStringA")]
        private readonly DTWAIN_ArrayGetAtFloatStringADelegate  _DTWAIN_ArrayGetAtFloatStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFloatStringW")]
        private readonly DTWAIN_ArrayGetAtFloatStringWDelegate  _DTWAIN_ArrayGetAtFloatStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFrame")]
        private readonly DTWAIN_ArrayGetAtFrameDelegate  _DTWAIN_ArrayGetAtFrame;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFrameEx")]
        private readonly DTWAIN_ArrayGetAtFrameExDelegate  _DTWAIN_ArrayGetAtFrameEx;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFrameString")]
        private readonly DTWAIN_ArrayGetAtFrameStringDelegate  _DTWAIN_ArrayGetAtFrameString;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFrameStringA")]
        private readonly DTWAIN_ArrayGetAtFrameStringADelegate  _DTWAIN_ArrayGetAtFrameStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtFrameStringW")]
        private readonly DTWAIN_ArrayGetAtFrameStringWDelegate  _DTWAIN_ArrayGetAtFrameStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtLong")]
        private readonly DTWAIN_ArrayGetAtLongDelegate  _DTWAIN_ArrayGetAtLong;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtLong64")]
        private readonly DTWAIN_ArrayGetAtLong64Delegate  _DTWAIN_ArrayGetAtLong64;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtSource")]
        private readonly DTWAIN_ArrayGetAtSourceDelegate  _DTWAIN_ArrayGetAtSource;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtSourceEx")]
        private readonly DTWAIN_ArrayGetAtSourceExDelegate  _DTWAIN_ArrayGetAtSourceEx;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtString")]
        private readonly DTWAIN_ArrayGetAtStringDelegate  _DTWAIN_ArrayGetAtString;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtStringA")]
        private readonly DTWAIN_ArrayGetAtStringADelegate  _DTWAIN_ArrayGetAtStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtStringW")]
        private readonly DTWAIN_ArrayGetAtStringWDelegate  _DTWAIN_ArrayGetAtStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayGetAtWideString")]
        private readonly DTWAIN_ArrayGetAtWideStringDelegate  _DTWAIN_ArrayGetAtWideString;

        [DTWAINNativeFunction("DTWAIN_ArrayGetBuffer")]
        private readonly DTWAIN_ArrayGetBufferDelegate  _DTWAIN_ArrayGetBuffer;

        [DTWAINNativeFunction("DTWAIN_ArrayGetCapValues")]
        private readonly DTWAIN_ArrayGetCapValuesDelegate  _DTWAIN_ArrayGetCapValues;

        [DTWAINNativeFunction("DTWAIN_ArrayGetCapValuesEx")]
        private readonly DTWAIN_ArrayGetCapValuesExDelegate  _DTWAIN_ArrayGetCapValuesEx;

        [DTWAINNativeFunction("DTWAIN_ArrayGetCapValuesEx2")]
        private readonly DTWAIN_ArrayGetCapValuesEx2Delegate  _DTWAIN_ArrayGetCapValuesEx2;

        [DTWAINNativeFunction("DTWAIN_ArrayGetCount")]
        private readonly DTWAIN_ArrayGetCountDelegate  _DTWAIN_ArrayGetCount;

        [DTWAINNativeFunction("DTWAIN_ArrayGetMaxStringLength")]
        private readonly DTWAIN_ArrayGetMaxStringLengthDelegate  _DTWAIN_ArrayGetMaxStringLength;

        [DTWAINNativeFunction("DTWAIN_ArrayGetSourceAt")]
        private readonly DTWAIN_ArrayGetSourceAtDelegate  _DTWAIN_ArrayGetSourceAt;

        [DTWAINNativeFunction("DTWAIN_ArrayGetStringLength")]
        private readonly DTWAIN_ArrayGetStringLengthDelegate  _DTWAIN_ArrayGetStringLength;

        [DTWAINNativeFunction("DTWAIN_ArrayGetType")]
        private readonly DTWAIN_ArrayGetTypeDelegate  _DTWAIN_ArrayGetType;

        [DTWAINNativeFunction("DTWAIN_ArrayInit")]
        private readonly DTWAIN_ArrayInitDelegate  _DTWAIN_ArrayInit;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAt")]
        private readonly DTWAIN_ArrayInsertAtDelegate  _DTWAIN_ArrayInsertAt;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtANSIString")]
        private readonly DTWAIN_ArrayInsertAtANSIStringDelegate  _DTWAIN_ArrayInsertAtANSIString;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtANSIStringN")]
        private readonly DTWAIN_ArrayInsertAtANSIStringNDelegate  _DTWAIN_ArrayInsertAtANSIStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloat")]
        private readonly DTWAIN_ArrayInsertAtFloatDelegate  _DTWAIN_ArrayInsertAtFloat;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloatN")]
        private readonly DTWAIN_ArrayInsertAtFloatNDelegate  _DTWAIN_ArrayInsertAtFloatN;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloatString")]
        private readonly DTWAIN_ArrayInsertAtFloatStringDelegate  _DTWAIN_ArrayInsertAtFloatString;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloatStringA")]
        private readonly DTWAIN_ArrayInsertAtFloatStringADelegate  _DTWAIN_ArrayInsertAtFloatStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloatStringN")]
        private readonly DTWAIN_ArrayInsertAtFloatStringNDelegate  _DTWAIN_ArrayInsertAtFloatStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloatStringNA")]
        private readonly DTWAIN_ArrayInsertAtFloatStringNADelegate  _DTWAIN_ArrayInsertAtFloatStringNA;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloatStringNW")]
        private readonly DTWAIN_ArrayInsertAtFloatStringNWDelegate  _DTWAIN_ArrayInsertAtFloatStringNW;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFloatStringW")]
        private readonly DTWAIN_ArrayInsertAtFloatStringWDelegate  _DTWAIN_ArrayInsertAtFloatStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFrame")]
        private readonly DTWAIN_ArrayInsertAtFrameDelegate  _DTWAIN_ArrayInsertAtFrame;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtFrameN")]
        private readonly DTWAIN_ArrayInsertAtFrameNDelegate  _DTWAIN_ArrayInsertAtFrameN;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtLong")]
        private readonly DTWAIN_ArrayInsertAtLongDelegate  _DTWAIN_ArrayInsertAtLong;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtLong64")]
        private readonly DTWAIN_ArrayInsertAtLong64Delegate  _DTWAIN_ArrayInsertAtLong64;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtLong64N")]
        private readonly DTWAIN_ArrayInsertAtLong64NDelegate  _DTWAIN_ArrayInsertAtLong64N;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtLongN")]
        private readonly DTWAIN_ArrayInsertAtLongNDelegate  _DTWAIN_ArrayInsertAtLongN;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtN")]
        private readonly DTWAIN_ArrayInsertAtNDelegate  _DTWAIN_ArrayInsertAtN;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtString")]
        private readonly DTWAIN_ArrayInsertAtStringDelegate  _DTWAIN_ArrayInsertAtString;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtStringA")]
        private readonly DTWAIN_ArrayInsertAtStringADelegate  _DTWAIN_ArrayInsertAtStringA;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtStringN")]
        private readonly DTWAIN_ArrayInsertAtStringNDelegate  _DTWAIN_ArrayInsertAtStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtStringNA")]
        private readonly DTWAIN_ArrayInsertAtStringNADelegate  _DTWAIN_ArrayInsertAtStringNA;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtStringNW")]
        private readonly DTWAIN_ArrayInsertAtStringNWDelegate  _DTWAIN_ArrayInsertAtStringNW;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtStringW")]
        private readonly DTWAIN_ArrayInsertAtStringWDelegate  _DTWAIN_ArrayInsertAtStringW;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtWideString")]
        private readonly DTWAIN_ArrayInsertAtWideStringDelegate  _DTWAIN_ArrayInsertAtWideString;

        [DTWAINNativeFunction("DTWAIN_ArrayInsertAtWideStringN")]
        private readonly DTWAIN_ArrayInsertAtWideStringNDelegate  _DTWAIN_ArrayInsertAtWideStringN;

        [DTWAINNativeFunction("DTWAIN_ArrayRemoveAll")]
        private readonly DTWAIN_ArrayRemoveAllDelegate  _DTWAIN_ArrayRemoveAll;

        [DTWAINNativeFunction("DTWAIN_ArrayRemoveAt")]
        private readonly DTWAIN_ArrayRemoveAtDelegate  _DTWAIN_ArrayRemoveAt;

        [DTWAINNativeFunction("DTWAIN_ArrayRemoveAtN")]
        private readonly DTWAIN_ArrayRemoveAtNDelegate  _DTWAIN_ArrayRemoveAtN;

        [DTWAINNativeFunction("DTWAIN_ArrayResize")]
        private readonly DTWAIN_ArrayResizeDelegate  _DTWAIN_ArrayResize;

        [DTWAINNativeFunction("DTWAIN_ArraySetAt")]
        private readonly DTWAIN_ArraySetAtDelegate  _DTWAIN_ArraySetAt;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtANSIString")]
        private readonly DTWAIN_ArraySetAtANSIStringDelegate  _DTWAIN_ArraySetAtANSIString;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFloat")]
        private readonly DTWAIN_ArraySetAtFloatDelegate  _DTWAIN_ArraySetAtFloat;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFloatString")]
        private readonly DTWAIN_ArraySetAtFloatStringDelegate  _DTWAIN_ArraySetAtFloatString;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFloatStringA")]
        private readonly DTWAIN_ArraySetAtFloatStringADelegate  _DTWAIN_ArraySetAtFloatStringA;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFloatStringW")]
        private readonly DTWAIN_ArraySetAtFloatStringWDelegate  _DTWAIN_ArraySetAtFloatStringW;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFrame")]
        private readonly DTWAIN_ArraySetAtFrameDelegate  _DTWAIN_ArraySetAtFrame;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFrameEx")]
        private readonly DTWAIN_ArraySetAtFrameExDelegate  _DTWAIN_ArraySetAtFrameEx;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFrameString")]
        private readonly DTWAIN_ArraySetAtFrameStringDelegate  _DTWAIN_ArraySetAtFrameString;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFrameStringA")]
        private readonly DTWAIN_ArraySetAtFrameStringADelegate  _DTWAIN_ArraySetAtFrameStringA;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtFrameStringW")]
        private readonly DTWAIN_ArraySetAtFrameStringWDelegate  _DTWAIN_ArraySetAtFrameStringW;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtLong")]
        private readonly DTWAIN_ArraySetAtLongDelegate  _DTWAIN_ArraySetAtLong;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtLong64")]
        private readonly DTWAIN_ArraySetAtLong64Delegate  _DTWAIN_ArraySetAtLong64;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtString")]
        private readonly DTWAIN_ArraySetAtStringDelegate  _DTWAIN_ArraySetAtString;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtStringA")]
        private readonly DTWAIN_ArraySetAtStringADelegate  _DTWAIN_ArraySetAtStringA;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtStringW")]
        private readonly DTWAIN_ArraySetAtStringWDelegate  _DTWAIN_ArraySetAtStringW;

        [DTWAINNativeFunction("DTWAIN_ArraySetAtWideString")]
        private readonly DTWAIN_ArraySetAtWideStringDelegate  _DTWAIN_ArraySetAtWideString;

        [DTWAINNativeFunction("DTWAIN_ArrayStringToFloat")]
        private readonly DTWAIN_ArrayStringToFloatDelegate  _DTWAIN_ArrayStringToFloat;

        [DTWAINNativeFunction("DTWAIN_ArrayWideStringToFloat")]
        private readonly DTWAIN_ArrayWideStringToFloatDelegate  _DTWAIN_ArrayWideStringToFloat;

        [DTWAINNativeFunction("DTWAIN_CallCallback")]
        private readonly DTWAIN_CallCallbackDelegate  _DTWAIN_CallCallback;

        [DTWAINNativeFunction("DTWAIN_CallCallback64")]
        private readonly DTWAIN_CallCallback64Delegate  _DTWAIN_CallCallback64;

        [DTWAINNativeFunction("DTWAIN_CallDSMProc")]
        private readonly DTWAIN_CallDSMProcDelegate  _DTWAIN_CallDSMProc;

        [DTWAINNativeFunction("DTWAIN_CheckHandles")]
        private readonly DTWAIN_CheckHandlesDelegate  _DTWAIN_CheckHandles;

        [DTWAINNativeFunction("DTWAIN_ClearBuffers")]
        private readonly DTWAIN_ClearBuffersDelegate  _DTWAIN_ClearBuffers;

        [DTWAINNativeFunction("DTWAIN_ClearErrorBuffer")]
        private readonly DTWAIN_ClearErrorBufferDelegate  _DTWAIN_ClearErrorBuffer;

        [DTWAINNativeFunction("DTWAIN_ClearPDFTextElements")]
        private readonly DTWAIN_ClearPDFTextElementsDelegate  _DTWAIN_ClearPDFTextElements;

        [DTWAINNativeFunction("DTWAIN_ClearPage")]
        private readonly DTWAIN_ClearPageDelegate  _DTWAIN_ClearPage;

        [DTWAINNativeFunction("DTWAIN_CloseSource")]
        private readonly DTWAIN_CloseSourceDelegate  _DTWAIN_CloseSource;

        [DTWAINNativeFunction("DTWAIN_CloseSourceUI")]
        private readonly DTWAIN_CloseSourceUIDelegate  _DTWAIN_CloseSourceUI;

        [DTWAINNativeFunction("DTWAIN_ConvertDIBToBitmap")]
        private readonly DTWAIN_ConvertDIBToBitmapDelegate  _DTWAIN_ConvertDIBToBitmap;

        [DTWAINNativeFunction("DTWAIN_ConvertDIBToFullBitmap")]
        private readonly DTWAIN_ConvertDIBToFullBitmapDelegate  _DTWAIN_ConvertDIBToFullBitmap;

        [DTWAINNativeFunction("DTWAIN_ConvertToAPIString")]
        private readonly DTWAIN_ConvertToAPIStringDelegate  _DTWAIN_ConvertToAPIString;

        [DTWAINNativeFunction("DTWAIN_ConvertToAPIStringA")]
        private readonly DTWAIN_ConvertToAPIStringADelegate  _DTWAIN_ConvertToAPIStringA;

        [DTWAINNativeFunction("DTWAIN_ConvertToAPIStringEx")]
        private readonly DTWAIN_ConvertToAPIStringExDelegate  _DTWAIN_ConvertToAPIStringEx;

        [DTWAINNativeFunction("DTWAIN_ConvertToAPIStringExA")]
        private readonly DTWAIN_ConvertToAPIStringExADelegate  _DTWAIN_ConvertToAPIStringExA;

        [DTWAINNativeFunction("DTWAIN_ConvertToAPIStringExW")]
        private readonly DTWAIN_ConvertToAPIStringExWDelegate  _DTWAIN_ConvertToAPIStringExW;

        [DTWAINNativeFunction("DTWAIN_ConvertToAPIStringW")]
        private readonly DTWAIN_ConvertToAPIStringWDelegate  _DTWAIN_ConvertToAPIStringW;

        [DTWAINNativeFunction("DTWAIN_CreateAcquisitionArray")]
        private readonly DTWAIN_CreateAcquisitionArrayDelegate  _DTWAIN_CreateAcquisitionArray;

        [DTWAINNativeFunction("DTWAIN_CreatePDFTextElement")]
        private readonly DTWAIN_CreatePDFTextElementDelegate  _DTWAIN_CreatePDFTextElement;

        [DTWAINNativeFunction("DTWAIN_CreatePDFTextElementCopy")]
        private readonly DTWAIN_CreatePDFTextElementCopyDelegate  _DTWAIN_CreatePDFTextElementCopy;

        [DTWAINNativeFunction("DTWAIN_DeleteDIB")]
        private readonly DTWAIN_DeleteDIBDelegate  _DTWAIN_DeleteDIB;

        [DTWAINNativeFunction("DTWAIN_DestroyAcquisitionArray")]
        private readonly DTWAIN_DestroyAcquisitionArrayDelegate  _DTWAIN_DestroyAcquisitionArray;

        [DTWAINNativeFunction("DTWAIN_DestroyPDFTextElement")]
        private readonly DTWAIN_DestroyPDFTextElementDelegate  _DTWAIN_DestroyPDFTextElement;

        [DTWAINNativeFunction("DTWAIN_DisableAppWindow")]
        private readonly DTWAIN_DisableAppWindowDelegate  _DTWAIN_DisableAppWindow;

        [DTWAINNativeFunction("DTWAIN_EnableAutoBorderDetect")]
        private readonly DTWAIN_EnableAutoBorderDetectDelegate  _DTWAIN_EnableAutoBorderDetect;

        [DTWAINNativeFunction("DTWAIN_EnableAutoBright")]
        private readonly DTWAIN_EnableAutoBrightDelegate  _DTWAIN_EnableAutoBright;

        [DTWAINNativeFunction("DTWAIN_EnableAutoDeskew")]
        private readonly DTWAIN_EnableAutoDeskewDelegate  _DTWAIN_EnableAutoDeskew;

        [DTWAINNativeFunction("DTWAIN_EnableAutoFeed")]
        private readonly DTWAIN_EnableAutoFeedDelegate  _DTWAIN_EnableAutoFeed;

        [DTWAINNativeFunction("DTWAIN_EnableAutoRotate")]
        private readonly DTWAIN_EnableAutoRotateDelegate  _DTWAIN_EnableAutoRotate;

        [DTWAINNativeFunction("DTWAIN_EnableAutoScan")]
        private readonly DTWAIN_EnableAutoScanDelegate  _DTWAIN_EnableAutoScan;

        [DTWAINNativeFunction("DTWAIN_EnableAutomaticSenseMedium")]
        private readonly DTWAIN_EnableAutomaticSenseMediumDelegate  _DTWAIN_EnableAutomaticSenseMedium;

        [DTWAINNativeFunction("DTWAIN_EnableDuplex")]
        private readonly DTWAIN_EnableDuplexDelegate  _DTWAIN_EnableDuplex;

        [DTWAINNativeFunction("DTWAIN_EnableFeeder")]
        private readonly DTWAIN_EnableFeederDelegate  _DTWAIN_EnableFeeder;

        [DTWAINNativeFunction("DTWAIN_EnableIndicator")]
        private readonly DTWAIN_EnableIndicatorDelegate  _DTWAIN_EnableIndicator;

        [DTWAINNativeFunction("DTWAIN_EnableJobFileHandling")]
        private readonly DTWAIN_EnableJobFileHandlingDelegate  _DTWAIN_EnableJobFileHandling;

        [DTWAINNativeFunction("DTWAIN_EnableLamp")]
        private readonly DTWAIN_EnableLampDelegate  _DTWAIN_EnableLamp;

        [DTWAINNativeFunction("DTWAIN_EnableMsgNotify")]
        private readonly DTWAIN_EnableMsgNotifyDelegate  _DTWAIN_EnableMsgNotify;

        [DTWAINNativeFunction("DTWAIN_EnablePatchDetect")]
        private readonly DTWAIN_EnablePatchDetectDelegate  _DTWAIN_EnablePatchDetect;

        [DTWAINNativeFunction("DTWAIN_EnablePeekMessageLoop")]
        private readonly DTWAIN_EnablePeekMessageLoopDelegate  _DTWAIN_EnablePeekMessageLoop;

        [DTWAINNativeFunction("DTWAIN_EnablePrinter")]
        private readonly DTWAIN_EnablePrinterDelegate  _DTWAIN_EnablePrinter;

        [DTWAINNativeFunction("DTWAIN_EnableThumbnail")]
        private readonly DTWAIN_EnableThumbnailDelegate  _DTWAIN_EnableThumbnail;

        [DTWAINNativeFunction("DTWAIN_EnableTripletsNotify")]
        private readonly DTWAIN_EnableTripletsNotifyDelegate  _DTWAIN_EnableTripletsNotify;

        [DTWAINNativeFunction("DTWAIN_EndThread")]
        private readonly DTWAIN_EndThreadDelegate  _DTWAIN_EndThread;

        [DTWAINNativeFunction("DTWAIN_EndTwainSession")]
        private readonly DTWAIN_EndTwainSessionDelegate  _DTWAIN_EndTwainSession;

        [DTWAINNativeFunction("DTWAIN_EnumAlarmVolumes")]
        private readonly DTWAIN_EnumAlarmVolumesDelegate  _DTWAIN_EnumAlarmVolumes;

        [DTWAINNativeFunction("DTWAIN_EnumAlarmVolumesEx")]
        private readonly DTWAIN_EnumAlarmVolumesExDelegate  _DTWAIN_EnumAlarmVolumesEx;

        [DTWAINNativeFunction("DTWAIN_EnumAlarms")]
        private readonly DTWAIN_EnumAlarmsDelegate  _DTWAIN_EnumAlarms;

        [DTWAINNativeFunction("DTWAIN_EnumAlarmsEx")]
        private readonly DTWAIN_EnumAlarmsExDelegate  _DTWAIN_EnumAlarmsEx;

        [DTWAINNativeFunction("DTWAIN_EnumAudioXferMechs")]
        private readonly DTWAIN_EnumAudioXferMechsDelegate  _DTWAIN_EnumAudioXferMechs;

        [DTWAINNativeFunction("DTWAIN_EnumAudioXferMechsEx")]
        private readonly DTWAIN_EnumAudioXferMechsExDelegate  _DTWAIN_EnumAudioXferMechsEx;

        [DTWAINNativeFunction("DTWAIN_EnumAutoFeedValues")]
        private readonly DTWAIN_EnumAutoFeedValuesDelegate  _DTWAIN_EnumAutoFeedValues;

        [DTWAINNativeFunction("DTWAIN_EnumAutoFeedValuesEx")]
        private readonly DTWAIN_EnumAutoFeedValuesExDelegate  _DTWAIN_EnumAutoFeedValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumAutomaticCaptures")]
        private readonly DTWAIN_EnumAutomaticCapturesDelegate  _DTWAIN_EnumAutomaticCaptures;

        [DTWAINNativeFunction("DTWAIN_EnumAutomaticCapturesEx")]
        private readonly DTWAIN_EnumAutomaticCapturesExDelegate  _DTWAIN_EnumAutomaticCapturesEx;

        [DTWAINNativeFunction("DTWAIN_EnumAutomaticSenseMedium")]
        private readonly DTWAIN_EnumAutomaticSenseMediumDelegate  _DTWAIN_EnumAutomaticSenseMedium;

        [DTWAINNativeFunction("DTWAIN_EnumAutomaticSenseMediumEx")]
        private readonly DTWAIN_EnumAutomaticSenseMediumExDelegate  _DTWAIN_EnumAutomaticSenseMediumEx;

        [DTWAINNativeFunction("DTWAIN_EnumBitDepths")]
        private readonly DTWAIN_EnumBitDepthsDelegate  _DTWAIN_EnumBitDepths;

        [DTWAINNativeFunction("DTWAIN_EnumBitDepthsEx")]
        private readonly DTWAIN_EnumBitDepthsExDelegate  _DTWAIN_EnumBitDepthsEx;

        [DTWAINNativeFunction("DTWAIN_EnumBitDepthsEx2")]
        private readonly DTWAIN_EnumBitDepthsEx2Delegate  _DTWAIN_EnumBitDepthsEx2;

        [DTWAINNativeFunction("DTWAIN_EnumBottomCameras")]
        private readonly DTWAIN_EnumBottomCamerasDelegate  _DTWAIN_EnumBottomCameras;

        [DTWAINNativeFunction("DTWAIN_EnumBottomCamerasEx")]
        private readonly DTWAIN_EnumBottomCamerasExDelegate  _DTWAIN_EnumBottomCamerasEx;

        [DTWAINNativeFunction("DTWAIN_EnumBrightnessValues")]
        private readonly DTWAIN_EnumBrightnessValuesDelegate  _DTWAIN_EnumBrightnessValues;

        [DTWAINNativeFunction("DTWAIN_EnumBrightnessValuesEx")]
        private readonly DTWAIN_EnumBrightnessValuesExDelegate  _DTWAIN_EnumBrightnessValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumCameras")]
        private readonly DTWAIN_EnumCamerasDelegate  _DTWAIN_EnumCameras;

        [DTWAINNativeFunction("DTWAIN_EnumCamerasEx")]
        private readonly DTWAIN_EnumCamerasExDelegate  _DTWAIN_EnumCamerasEx;

        [DTWAINNativeFunction("DTWAIN_EnumCamerasEx2")]
        private readonly DTWAIN_EnumCamerasEx2Delegate  _DTWAIN_EnumCamerasEx2;

        [DTWAINNativeFunction("DTWAIN_EnumCamerasEx3")]
        private readonly DTWAIN_EnumCamerasEx3Delegate  _DTWAIN_EnumCamerasEx3;

        [DTWAINNativeFunction("DTWAIN_EnumCompressionTypes")]
        private readonly DTWAIN_EnumCompressionTypesDelegate  _DTWAIN_EnumCompressionTypes;

        [DTWAINNativeFunction("DTWAIN_EnumCompressionTypesEx")]
        private readonly DTWAIN_EnumCompressionTypesExDelegate  _DTWAIN_EnumCompressionTypesEx;

        [DTWAINNativeFunction("DTWAIN_EnumCompressionTypesEx2")]
        private readonly DTWAIN_EnumCompressionTypesEx2Delegate  _DTWAIN_EnumCompressionTypesEx2;

        [DTWAINNativeFunction("DTWAIN_EnumContrastValues")]
        private readonly DTWAIN_EnumContrastValuesDelegate  _DTWAIN_EnumContrastValues;

        [DTWAINNativeFunction("DTWAIN_EnumContrastValuesEx")]
        private readonly DTWAIN_EnumContrastValuesExDelegate  _DTWAIN_EnumContrastValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumCustomCaps")]
        private readonly DTWAIN_EnumCustomCapsDelegate  _DTWAIN_EnumCustomCaps;

        [DTWAINNativeFunction("DTWAIN_EnumCustomCapsEx2")]
        private readonly DTWAIN_EnumCustomCapsEx2Delegate  _DTWAIN_EnumCustomCapsEx2;

        [DTWAINNativeFunction("DTWAIN_EnumDoubleFeedDetectLengths")]
        private readonly DTWAIN_EnumDoubleFeedDetectLengthsDelegate  _DTWAIN_EnumDoubleFeedDetectLengths;

        [DTWAINNativeFunction("DTWAIN_EnumDoubleFeedDetectLengthsEx")]
        private readonly DTWAIN_EnumDoubleFeedDetectLengthsExDelegate  _DTWAIN_EnumDoubleFeedDetectLengthsEx;

        [DTWAINNativeFunction("DTWAIN_EnumDoubleFeedDetectValues")]
        private readonly DTWAIN_EnumDoubleFeedDetectValuesDelegate  _DTWAIN_EnumDoubleFeedDetectValues;

        [DTWAINNativeFunction("DTWAIN_EnumDoubleFeedDetectValuesEx")]
        private readonly DTWAIN_EnumDoubleFeedDetectValuesExDelegate  _DTWAIN_EnumDoubleFeedDetectValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumExtImageInfoTypes")]
        private readonly DTWAIN_EnumExtImageInfoTypesDelegate  _DTWAIN_EnumExtImageInfoTypes;

        [DTWAINNativeFunction("DTWAIN_EnumExtImageInfoTypesEx")]
        private readonly DTWAIN_EnumExtImageInfoTypesExDelegate  _DTWAIN_EnumExtImageInfoTypesEx;

        [DTWAINNativeFunction("DTWAIN_EnumExtendedCaps")]
        private readonly DTWAIN_EnumExtendedCapsDelegate  _DTWAIN_EnumExtendedCaps;

        [DTWAINNativeFunction("DTWAIN_EnumExtendedCapsEx")]
        private readonly DTWAIN_EnumExtendedCapsExDelegate  _DTWAIN_EnumExtendedCapsEx;

        [DTWAINNativeFunction("DTWAIN_EnumExtendedCapsEx2")]
        private readonly DTWAIN_EnumExtendedCapsEx2Delegate  _DTWAIN_EnumExtendedCapsEx2;

        [DTWAINNativeFunction("DTWAIN_EnumFileTypeBitsPerPixel")]
        private readonly DTWAIN_EnumFileTypeBitsPerPixelDelegate  _DTWAIN_EnumFileTypeBitsPerPixel;

        [DTWAINNativeFunction("DTWAIN_EnumFileXferFormats")]
        private readonly DTWAIN_EnumFileXferFormatsDelegate  _DTWAIN_EnumFileXferFormats;

        [DTWAINNativeFunction("DTWAIN_EnumFileXferFormatsEx")]
        private readonly DTWAIN_EnumFileXferFormatsExDelegate  _DTWAIN_EnumFileXferFormatsEx;

        [DTWAINNativeFunction("DTWAIN_EnumHalftones")]
        private readonly DTWAIN_EnumHalftonesDelegate  _DTWAIN_EnumHalftones;

        [DTWAINNativeFunction("DTWAIN_EnumHalftonesEx")]
        private readonly DTWAIN_EnumHalftonesExDelegate  _DTWAIN_EnumHalftonesEx;

        [DTWAINNativeFunction("DTWAIN_EnumHighlightValues")]
        private readonly DTWAIN_EnumHighlightValuesDelegate  _DTWAIN_EnumHighlightValues;

        [DTWAINNativeFunction("DTWAIN_EnumHighlightValuesEx")]
        private readonly DTWAIN_EnumHighlightValuesExDelegate  _DTWAIN_EnumHighlightValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumJobControls")]
        private readonly DTWAIN_EnumJobControlsDelegate  _DTWAIN_EnumJobControls;

        [DTWAINNativeFunction("DTWAIN_EnumJobControlsEx")]
        private readonly DTWAIN_EnumJobControlsExDelegate  _DTWAIN_EnumJobControlsEx;

        [DTWAINNativeFunction("DTWAIN_EnumLightPaths")]
        private readonly DTWAIN_EnumLightPathsDelegate  _DTWAIN_EnumLightPaths;

        [DTWAINNativeFunction("DTWAIN_EnumLightPathsEx")]
        private readonly DTWAIN_EnumLightPathsExDelegate  _DTWAIN_EnumLightPathsEx;

        [DTWAINNativeFunction("DTWAIN_EnumLightSources")]
        private readonly DTWAIN_EnumLightSourcesDelegate  _DTWAIN_EnumLightSources;

        [DTWAINNativeFunction("DTWAIN_EnumLightSourcesEx")]
        private readonly DTWAIN_EnumLightSourcesExDelegate  _DTWAIN_EnumLightSourcesEx;

        [DTWAINNativeFunction("DTWAIN_EnumMaxBuffers")]
        private readonly DTWAIN_EnumMaxBuffersDelegate  _DTWAIN_EnumMaxBuffers;

        [DTWAINNativeFunction("DTWAIN_EnumMaxBuffersEx")]
        private readonly DTWAIN_EnumMaxBuffersExDelegate  _DTWAIN_EnumMaxBuffersEx;

        [DTWAINNativeFunction("DTWAIN_EnumNoiseFilters")]
        private readonly DTWAIN_EnumNoiseFiltersDelegate  _DTWAIN_EnumNoiseFilters;

        [DTWAINNativeFunction("DTWAIN_EnumNoiseFiltersEx")]
        private readonly DTWAIN_EnumNoiseFiltersExDelegate  _DTWAIN_EnumNoiseFiltersEx;

        [DTWAINNativeFunction("DTWAIN_EnumOCRInterfaces")]
        private readonly DTWAIN_EnumOCRInterfacesDelegate  _DTWAIN_EnumOCRInterfaces;

        [DTWAINNativeFunction("DTWAIN_EnumOCRSupportedCaps")]
        private readonly DTWAIN_EnumOCRSupportedCapsDelegate  _DTWAIN_EnumOCRSupportedCaps;

        [DTWAINNativeFunction("DTWAIN_EnumOrientations")]
        private readonly DTWAIN_EnumOrientationsDelegate  _DTWAIN_EnumOrientations;

        [DTWAINNativeFunction("DTWAIN_EnumOrientationsEx")]
        private readonly DTWAIN_EnumOrientationsExDelegate  _DTWAIN_EnumOrientationsEx;

        [DTWAINNativeFunction("DTWAIN_EnumOverscanValues")]
        private readonly DTWAIN_EnumOverscanValuesDelegate  _DTWAIN_EnumOverscanValues;

        [DTWAINNativeFunction("DTWAIN_EnumOverscanValuesEx")]
        private readonly DTWAIN_EnumOverscanValuesExDelegate  _DTWAIN_EnumOverscanValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPaperSizes")]
        private readonly DTWAIN_EnumPaperSizesDelegate  _DTWAIN_EnumPaperSizes;

        [DTWAINNativeFunction("DTWAIN_EnumPaperSizesEx")]
        private readonly DTWAIN_EnumPaperSizesExDelegate  _DTWAIN_EnumPaperSizesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPatchCodes")]
        private readonly DTWAIN_EnumPatchCodesDelegate  _DTWAIN_EnumPatchCodes;

        [DTWAINNativeFunction("DTWAIN_EnumPatchCodesEx")]
        private readonly DTWAIN_EnumPatchCodesExDelegate  _DTWAIN_EnumPatchCodesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPatchMaxPriorities")]
        private readonly DTWAIN_EnumPatchMaxPrioritiesDelegate  _DTWAIN_EnumPatchMaxPriorities;

        [DTWAINNativeFunction("DTWAIN_EnumPatchMaxPrioritiesEx")]
        private readonly DTWAIN_EnumPatchMaxPrioritiesExDelegate  _DTWAIN_EnumPatchMaxPrioritiesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPatchMaxRetries")]
        private readonly DTWAIN_EnumPatchMaxRetriesDelegate  _DTWAIN_EnumPatchMaxRetries;

        [DTWAINNativeFunction("DTWAIN_EnumPatchMaxRetriesEx")]
        private readonly DTWAIN_EnumPatchMaxRetriesExDelegate  _DTWAIN_EnumPatchMaxRetriesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPatchPriorities")]
        private readonly DTWAIN_EnumPatchPrioritiesDelegate  _DTWAIN_EnumPatchPriorities;

        [DTWAINNativeFunction("DTWAIN_EnumPatchPrioritiesEx")]
        private readonly DTWAIN_EnumPatchPrioritiesExDelegate  _DTWAIN_EnumPatchPrioritiesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPatchSearchModes")]
        private readonly DTWAIN_EnumPatchSearchModesDelegate  _DTWAIN_EnumPatchSearchModes;

        [DTWAINNativeFunction("DTWAIN_EnumPatchSearchModesEx")]
        private readonly DTWAIN_EnumPatchSearchModesExDelegate  _DTWAIN_EnumPatchSearchModesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPatchTimeOutValues")]
        private readonly DTWAIN_EnumPatchTimeOutValuesDelegate  _DTWAIN_EnumPatchTimeOutValues;

        [DTWAINNativeFunction("DTWAIN_EnumPatchTimeOutValuesEx")]
        private readonly DTWAIN_EnumPatchTimeOutValuesExDelegate  _DTWAIN_EnumPatchTimeOutValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPixelTypes")]
        private readonly DTWAIN_EnumPixelTypesDelegate  _DTWAIN_EnumPixelTypes;

        [DTWAINNativeFunction("DTWAIN_EnumPixelTypesEx")]
        private readonly DTWAIN_EnumPixelTypesExDelegate  _DTWAIN_EnumPixelTypesEx;

        [DTWAINNativeFunction("DTWAIN_EnumPrinterStringModes")]
        private readonly DTWAIN_EnumPrinterStringModesDelegate  _DTWAIN_EnumPrinterStringModes;

        [DTWAINNativeFunction("DTWAIN_EnumPrinterStringModesEx")]
        private readonly DTWAIN_EnumPrinterStringModesExDelegate  _DTWAIN_EnumPrinterStringModesEx;

        [DTWAINNativeFunction("DTWAIN_EnumResolutionValues")]
        private readonly DTWAIN_EnumResolutionValuesDelegate  _DTWAIN_EnumResolutionValues;

        [DTWAINNativeFunction("DTWAIN_EnumResolutionValuesEx")]
        private readonly DTWAIN_EnumResolutionValuesExDelegate  _DTWAIN_EnumResolutionValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumShadowValues")]
        private readonly DTWAIN_EnumShadowValuesDelegate  _DTWAIN_EnumShadowValues;

        [DTWAINNativeFunction("DTWAIN_EnumShadowValuesEx")]
        private readonly DTWAIN_EnumShadowValuesExDelegate  _DTWAIN_EnumShadowValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumSourceUnits")]
        private readonly DTWAIN_EnumSourceUnitsDelegate  _DTWAIN_EnumSourceUnits;

        [DTWAINNativeFunction("DTWAIN_EnumSourceUnitsEx")]
        private readonly DTWAIN_EnumSourceUnitsExDelegate  _DTWAIN_EnumSourceUnitsEx;

        [DTWAINNativeFunction("DTWAIN_EnumSourceValues")]
        private readonly DTWAIN_EnumSourceValuesDelegate  _DTWAIN_EnumSourceValues;

        [DTWAINNativeFunction("DTWAIN_EnumSourceValuesA")]
        private readonly DTWAIN_EnumSourceValuesADelegate  _DTWAIN_EnumSourceValuesA;

        [DTWAINNativeFunction("DTWAIN_EnumSourceValuesW")]
        private readonly DTWAIN_EnumSourceValuesWDelegate  _DTWAIN_EnumSourceValuesW;

        [DTWAINNativeFunction("DTWAIN_EnumSources")]
        private readonly DTWAIN_EnumSourcesDelegate  _DTWAIN_EnumSources;

        [DTWAINNativeFunction("DTWAIN_EnumSourcesEx")]
        private readonly DTWAIN_EnumSourcesExDelegate  _DTWAIN_EnumSourcesEx;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedCaps")]
        private readonly DTWAIN_EnumSupportedCapsDelegate  _DTWAIN_EnumSupportedCaps;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedCapsEx")]
        private readonly DTWAIN_EnumSupportedCapsExDelegate  _DTWAIN_EnumSupportedCapsEx;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedCapsEx2")]
        private readonly DTWAIN_EnumSupportedCapsEx2Delegate  _DTWAIN_EnumSupportedCapsEx2;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedExtImageInfo")]
        private readonly DTWAIN_EnumSupportedExtImageInfoDelegate  _DTWAIN_EnumSupportedExtImageInfo;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedExtImageInfoEx")]
        private readonly DTWAIN_EnumSupportedExtImageInfoExDelegate  _DTWAIN_EnumSupportedExtImageInfoEx;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedFileTypes")]
        private readonly DTWAIN_EnumSupportedFileTypesDelegate  _DTWAIN_EnumSupportedFileTypes;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedMultiPageFileTypes")]
        private readonly DTWAIN_EnumSupportedMultiPageFileTypesDelegate  _DTWAIN_EnumSupportedMultiPageFileTypes;

        [DTWAINNativeFunction("DTWAIN_EnumSupportedSinglePageFileTypes")]
        private readonly DTWAIN_EnumSupportedSinglePageFileTypesDelegate  _DTWAIN_EnumSupportedSinglePageFileTypes;

        [DTWAINNativeFunction("DTWAIN_EnumThresholdValues")]
        private readonly DTWAIN_EnumThresholdValuesDelegate  _DTWAIN_EnumThresholdValues;

        [DTWAINNativeFunction("DTWAIN_EnumThresholdValuesEx")]
        private readonly DTWAIN_EnumThresholdValuesExDelegate  _DTWAIN_EnumThresholdValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumTopCameras")]
        private readonly DTWAIN_EnumTopCamerasDelegate  _DTWAIN_EnumTopCameras;

        [DTWAINNativeFunction("DTWAIN_EnumTopCamerasEx")]
        private readonly DTWAIN_EnumTopCamerasExDelegate  _DTWAIN_EnumTopCamerasEx;

        [DTWAINNativeFunction("DTWAIN_EnumTwainPrinters")]
        private readonly DTWAIN_EnumTwainPrintersDelegate  _DTWAIN_EnumTwainPrinters;

        [DTWAINNativeFunction("DTWAIN_EnumTwainPrintersArray")]
        private readonly DTWAIN_EnumTwainPrintersArrayDelegate  _DTWAIN_EnumTwainPrintersArray;

        [DTWAINNativeFunction("DTWAIN_EnumTwainPrintersArrayEx")]
        private readonly DTWAIN_EnumTwainPrintersArrayExDelegate  _DTWAIN_EnumTwainPrintersArrayEx;

        [DTWAINNativeFunction("DTWAIN_EnumTwainPrintersEx")]
        private readonly DTWAIN_EnumTwainPrintersExDelegate  _DTWAIN_EnumTwainPrintersEx;

        [DTWAINNativeFunction("DTWAIN_EnumXResolutionValues")]
        private readonly DTWAIN_EnumXResolutionValuesDelegate  _DTWAIN_EnumXResolutionValues;

        [DTWAINNativeFunction("DTWAIN_EnumXResolutionValuesEx")]
        private readonly DTWAIN_EnumXResolutionValuesExDelegate  _DTWAIN_EnumXResolutionValuesEx;

        [DTWAINNativeFunction("DTWAIN_EnumYResolutionValues")]
        private readonly DTWAIN_EnumYResolutionValuesDelegate  _DTWAIN_EnumYResolutionValues;

        [DTWAINNativeFunction("DTWAIN_EnumYResolutionValuesEx")]
        private readonly DTWAIN_EnumYResolutionValuesExDelegate  _DTWAIN_EnumYResolutionValuesEx;

        [DTWAINNativeFunction("DTWAIN_ExecuteOCR")]
        private readonly DTWAIN_ExecuteOCRDelegate  _DTWAIN_ExecuteOCR;

        [DTWAINNativeFunction("DTWAIN_ExecuteOCRA")]
        private readonly DTWAIN_ExecuteOCRADelegate  _DTWAIN_ExecuteOCRA;

        [DTWAINNativeFunction("DTWAIN_ExecuteOCRW")]
        private readonly DTWAIN_ExecuteOCRWDelegate  _DTWAIN_ExecuteOCRW;

        [DTWAINNativeFunction("DTWAIN_FeedPage")]
        private readonly DTWAIN_FeedPageDelegate  _DTWAIN_FeedPage;

        [DTWAINNativeFunction("DTWAIN_FlipBitmap")]
        private readonly DTWAIN_FlipBitmapDelegate  _DTWAIN_FlipBitmap;

        [DTWAINNativeFunction("DTWAIN_FlushAcquiredPages")]
        private readonly DTWAIN_FlushAcquiredPagesDelegate  _DTWAIN_FlushAcquiredPages;

        [DTWAINNativeFunction("DTWAIN_ForceAcquireBitDepth")]
        private readonly DTWAIN_ForceAcquireBitDepthDelegate  _DTWAIN_ForceAcquireBitDepth;

        [DTWAINNativeFunction("DTWAIN_ForceScanOnNoUI")]
        private readonly DTWAIN_ForceScanOnNoUIDelegate  _DTWAIN_ForceScanOnNoUI;

        [DTWAINNativeFunction("DTWAIN_FrameCreate")]
        private readonly DTWAIN_FrameCreateDelegate  _DTWAIN_FrameCreate;

        [DTWAINNativeFunction("DTWAIN_FrameCreateString")]
        private readonly DTWAIN_FrameCreateStringDelegate  _DTWAIN_FrameCreateString;

        [DTWAINNativeFunction("DTWAIN_FrameCreateStringA")]
        private readonly DTWAIN_FrameCreateStringADelegate  _DTWAIN_FrameCreateStringA;

        [DTWAINNativeFunction("DTWAIN_FrameCreateStringW")]
        private readonly DTWAIN_FrameCreateStringWDelegate  _DTWAIN_FrameCreateStringW;

        [DTWAINNativeFunction("DTWAIN_FrameDestroy")]
        private readonly DTWAIN_FrameDestroyDelegate  _DTWAIN_FrameDestroy;

        [DTWAINNativeFunction("DTWAIN_FrameGetAll")]
        private readonly DTWAIN_FrameGetAllDelegate  _DTWAIN_FrameGetAll;

        [DTWAINNativeFunction("DTWAIN_FrameGetAllString")]
        private readonly DTWAIN_FrameGetAllStringDelegate  _DTWAIN_FrameGetAllString;

        [DTWAINNativeFunction("DTWAIN_FrameGetAllStringA")]
        private readonly DTWAIN_FrameGetAllStringADelegate  _DTWAIN_FrameGetAllStringA;

        [DTWAINNativeFunction("DTWAIN_FrameGetAllStringW")]
        private readonly DTWAIN_FrameGetAllStringWDelegate  _DTWAIN_FrameGetAllStringW;

        [DTWAINNativeFunction("DTWAIN_FrameGetValue")]
        private readonly DTWAIN_FrameGetValueDelegate  _DTWAIN_FrameGetValue;

        [DTWAINNativeFunction("DTWAIN_FrameGetValueString")]
        private readonly DTWAIN_FrameGetValueStringDelegate  _DTWAIN_FrameGetValueString;

        [DTWAINNativeFunction("DTWAIN_FrameGetValueStringA")]
        private readonly DTWAIN_FrameGetValueStringADelegate  _DTWAIN_FrameGetValueStringA;

        [DTWAINNativeFunction("DTWAIN_FrameGetValueStringW")]
        private readonly DTWAIN_FrameGetValueStringWDelegate  _DTWAIN_FrameGetValueStringW;

        [DTWAINNativeFunction("DTWAIN_FrameIsValid")]
        private readonly DTWAIN_FrameIsValidDelegate  _DTWAIN_FrameIsValid;

        [DTWAINNativeFunction("DTWAIN_FrameSetAll")]
        private readonly DTWAIN_FrameSetAllDelegate  _DTWAIN_FrameSetAll;

        [DTWAINNativeFunction("DTWAIN_FrameSetAllString")]
        private readonly DTWAIN_FrameSetAllStringDelegate  _DTWAIN_FrameSetAllString;

        [DTWAINNativeFunction("DTWAIN_FrameSetAllStringA")]
        private readonly DTWAIN_FrameSetAllStringADelegate  _DTWAIN_FrameSetAllStringA;

        [DTWAINNativeFunction("DTWAIN_FrameSetAllStringW")]
        private readonly DTWAIN_FrameSetAllStringWDelegate  _DTWAIN_FrameSetAllStringW;

        [DTWAINNativeFunction("DTWAIN_FrameSetValue")]
        private readonly DTWAIN_FrameSetValueDelegate  _DTWAIN_FrameSetValue;

        [DTWAINNativeFunction("DTWAIN_FrameSetValueString")]
        private readonly DTWAIN_FrameSetValueStringDelegate  _DTWAIN_FrameSetValueString;

        [DTWAINNativeFunction("DTWAIN_FrameSetValueStringA")]
        private readonly DTWAIN_FrameSetValueStringADelegate  _DTWAIN_FrameSetValueStringA;

        [DTWAINNativeFunction("DTWAIN_FrameSetValueStringW")]
        private readonly DTWAIN_FrameSetValueStringWDelegate  _DTWAIN_FrameSetValueStringW;

        [DTWAINNativeFunction("DTWAIN_FreeExtImageInfo")]
        private readonly DTWAIN_FreeExtImageInfoDelegate  _DTWAIN_FreeExtImageInfo;

        [DTWAINNativeFunction("DTWAIN_FreeMemory")]
        private readonly DTWAIN_FreeMemoryDelegate  _DTWAIN_FreeMemory;

        [DTWAINNativeFunction("DTWAIN_FreeMemoryEx")]
        private readonly DTWAIN_FreeMemoryExDelegate  _DTWAIN_FreeMemoryEx;

        [DTWAINNativeFunction("DTWAIN_GetAPIHandleStatus")]
        private readonly DTWAIN_GetAPIHandleStatusDelegate  _DTWAIN_GetAPIHandleStatus;

        [DTWAINNativeFunction("DTWAIN_GetAcquireArea")]
        private readonly DTWAIN_GetAcquireAreaDelegate  _DTWAIN_GetAcquireArea;

        [DTWAINNativeFunction("DTWAIN_GetAcquireArea2")]
        private readonly DTWAIN_GetAcquireArea2Delegate  _DTWAIN_GetAcquireArea2;

        [DTWAINNativeFunction("DTWAIN_GetAcquireArea2String")]
        private readonly DTWAIN_GetAcquireArea2StringDelegate  _DTWAIN_GetAcquireArea2String;

        [DTWAINNativeFunction("DTWAIN_GetAcquireArea2StringA")]
        private readonly DTWAIN_GetAcquireArea2StringADelegate  _DTWAIN_GetAcquireArea2StringA;

        [DTWAINNativeFunction("DTWAIN_GetAcquireArea2StringW")]
        private readonly DTWAIN_GetAcquireArea2StringWDelegate  _DTWAIN_GetAcquireArea2StringW;

        [DTWAINNativeFunction("DTWAIN_GetAcquireAreaEx")]
        private readonly DTWAIN_GetAcquireAreaExDelegate  _DTWAIN_GetAcquireAreaEx;

        [DTWAINNativeFunction("DTWAIN_GetAcquireMetrics")]
        private readonly DTWAIN_GetAcquireMetricsDelegate  _DTWAIN_GetAcquireMetrics;

        [DTWAINNativeFunction("DTWAIN_GetAcquireStripBuffer")]
        private readonly DTWAIN_GetAcquireStripBufferDelegate  _DTWAIN_GetAcquireStripBuffer;

        [DTWAINNativeFunction("DTWAIN_GetAcquireStripData")]
        private readonly DTWAIN_GetAcquireStripDataDelegate  _DTWAIN_GetAcquireStripData;

        [DTWAINNativeFunction("DTWAIN_GetAcquireStripSizes")]
        private readonly DTWAIN_GetAcquireStripSizesDelegate  _DTWAIN_GetAcquireStripSizes;

        [DTWAINNativeFunction("DTWAIN_GetAcquiredImage")]
        private readonly DTWAIN_GetAcquiredImageDelegate  _DTWAIN_GetAcquiredImage;

        [DTWAINNativeFunction("DTWAIN_GetAcquiredImageArray")]
        private readonly DTWAIN_GetAcquiredImageArrayDelegate  _DTWAIN_GetAcquiredImageArray;

        [DTWAINNativeFunction("DTWAIN_GetActiveDSMPath")]
        private readonly DTWAIN_GetActiveDSMPathDelegate  _DTWAIN_GetActiveDSMPath;

        [DTWAINNativeFunction("DTWAIN_GetActiveDSMPathA")]
        private readonly DTWAIN_GetActiveDSMPathADelegate  _DTWAIN_GetActiveDSMPathA;

        [DTWAINNativeFunction("DTWAIN_GetActiveDSMPathW")]
        private readonly DTWAIN_GetActiveDSMPathWDelegate  _DTWAIN_GetActiveDSMPathW;

        [DTWAINNativeFunction("DTWAIN_GetActiveDSMVersionInfo")]
        private readonly DTWAIN_GetActiveDSMVersionInfoDelegate  _DTWAIN_GetActiveDSMVersionInfo;

        [DTWAINNativeFunction("DTWAIN_GetActiveDSMVersionInfoA")]
        private readonly DTWAIN_GetActiveDSMVersionInfoADelegate  _DTWAIN_GetActiveDSMVersionInfoA;

        [DTWAINNativeFunction("DTWAIN_GetActiveDSMVersionInfoW")]
        private readonly DTWAIN_GetActiveDSMVersionInfoWDelegate  _DTWAIN_GetActiveDSMVersionInfoW;

        [DTWAINNativeFunction("DTWAIN_GetAlarmVolume")]
        private readonly DTWAIN_GetAlarmVolumeDelegate  _DTWAIN_GetAlarmVolume;

        [DTWAINNativeFunction("DTWAIN_GetAllSourceDibs")]
        private readonly DTWAIN_GetAllSourceDibsDelegate  _DTWAIN_GetAllSourceDibs;

        [DTWAINNativeFunction("DTWAIN_GetAppInfo")]
        private readonly DTWAIN_GetAppInfoDelegate  _DTWAIN_GetAppInfo;

        [DTWAINNativeFunction("DTWAIN_GetAppInfoA")]
        private readonly DTWAIN_GetAppInfoADelegate  _DTWAIN_GetAppInfoA;

        [DTWAINNativeFunction("DTWAIN_GetAppInfoW")]
        private readonly DTWAIN_GetAppInfoWDelegate  _DTWAIN_GetAppInfoW;

        [DTWAINNativeFunction("DTWAIN_GetAuthor")]
        private readonly DTWAIN_GetAuthorDelegate  _DTWAIN_GetAuthor;

        [DTWAINNativeFunction("DTWAIN_GetAuthorA")]
        private readonly DTWAIN_GetAuthorADelegate  _DTWAIN_GetAuthorA;

        [DTWAINNativeFunction("DTWAIN_GetAuthorW")]
        private readonly DTWAIN_GetAuthorWDelegate  _DTWAIN_GetAuthorW;

        [DTWAINNativeFunction("DTWAIN_GetBatteryMinutes")]
        private readonly DTWAIN_GetBatteryMinutesDelegate  _DTWAIN_GetBatteryMinutes;

        [DTWAINNativeFunction("DTWAIN_GetBatteryPercent")]
        private readonly DTWAIN_GetBatteryPercentDelegate  _DTWAIN_GetBatteryPercent;

        [DTWAINNativeFunction("DTWAIN_GetBitDepth")]
        private readonly DTWAIN_GetBitDepthDelegate  _DTWAIN_GetBitDepth;

        [DTWAINNativeFunction("DTWAIN_GetBlankPageAutoDetection")]
        private readonly DTWAIN_GetBlankPageAutoDetectionDelegate  _DTWAIN_GetBlankPageAutoDetection;

        [DTWAINNativeFunction("DTWAIN_GetBrightness")]
        private readonly DTWAIN_GetBrightnessDelegate  _DTWAIN_GetBrightness;

        [DTWAINNativeFunction("DTWAIN_GetBrightnessString")]
        private readonly DTWAIN_GetBrightnessStringDelegate  _DTWAIN_GetBrightnessString;

        [DTWAINNativeFunction("DTWAIN_GetBrightnessStringA")]
        private readonly DTWAIN_GetBrightnessStringADelegate  _DTWAIN_GetBrightnessStringA;

        [DTWAINNativeFunction("DTWAIN_GetBrightnessStringW")]
        private readonly DTWAIN_GetBrightnessStringWDelegate  _DTWAIN_GetBrightnessStringW;

        [DTWAINNativeFunction("DTWAIN_GetBufferedTransferInfo")]
        private readonly DTWAIN_GetBufferedTransferInfoDelegate  _DTWAIN_GetBufferedTransferInfo;

        [DTWAINNativeFunction("DTWAIN_GetCallback")]
        private readonly DTWAIN_GetCallbackDelegate  _DTWAIN_GetCallback;

        [DTWAINNativeFunction("DTWAIN_GetCallback64")]
        private readonly DTWAIN_GetCallback64Delegate  _DTWAIN_GetCallback64;

        [DTWAINNativeFunction("DTWAIN_GetCapArrayType")]
        private readonly DTWAIN_GetCapArrayTypeDelegate  _DTWAIN_GetCapArrayType;

        [DTWAINNativeFunction("DTWAIN_GetCapContainer")]
        private readonly DTWAIN_GetCapContainerDelegate  _DTWAIN_GetCapContainer;

        [DTWAINNativeFunction("DTWAIN_GetCapContainerEx")]
        private readonly DTWAIN_GetCapContainerExDelegate  _DTWAIN_GetCapContainerEx;

        [DTWAINNativeFunction("DTWAIN_GetCapContainerEx2")]
        private readonly DTWAIN_GetCapContainerEx2Delegate  _DTWAIN_GetCapContainerEx2;

        [DTWAINNativeFunction("DTWAIN_GetCapDataType")]
        private readonly DTWAIN_GetCapDataTypeDelegate  _DTWAIN_GetCapDataType;

        [DTWAINNativeFunction("DTWAIN_GetCapFromName")]
        private readonly DTWAIN_GetCapFromNameDelegate  _DTWAIN_GetCapFromName;

        [DTWAINNativeFunction("DTWAIN_GetCapFromNameA")]
        private readonly DTWAIN_GetCapFromNameADelegate  _DTWAIN_GetCapFromNameA;

        [DTWAINNativeFunction("DTWAIN_GetCapFromNameW")]
        private readonly DTWAIN_GetCapFromNameWDelegate  _DTWAIN_GetCapFromNameW;

        [DTWAINNativeFunction("DTWAIN_GetCapOperations")]
        private readonly DTWAIN_GetCapOperationsDelegate  _DTWAIN_GetCapOperations;

        [DTWAINNativeFunction("DTWAIN_GetCapValues")]
        private readonly DTWAIN_GetCapValuesDelegate  _DTWAIN_GetCapValues;

        [DTWAINNativeFunction("DTWAIN_GetCapValuesEx")]
        private readonly DTWAIN_GetCapValuesExDelegate  _DTWAIN_GetCapValuesEx;

        [DTWAINNativeFunction("DTWAIN_GetCapValuesEx2")]
        private readonly DTWAIN_GetCapValuesEx2Delegate  _DTWAIN_GetCapValuesEx2;

        [DTWAINNativeFunction("DTWAIN_GetCaption")]
        private readonly DTWAIN_GetCaptionDelegate  _DTWAIN_GetCaption;

        [DTWAINNativeFunction("DTWAIN_GetCaptionA")]
        private readonly DTWAIN_GetCaptionADelegate  _DTWAIN_GetCaptionA;

        [DTWAINNativeFunction("DTWAIN_GetCaptionW")]
        private readonly DTWAIN_GetCaptionWDelegate  _DTWAIN_GetCaptionW;

        [DTWAINNativeFunction("DTWAIN_GetCompressionSize")]
        private readonly DTWAIN_GetCompressionSizeDelegate  _DTWAIN_GetCompressionSize;

        [DTWAINNativeFunction("DTWAIN_GetCompressionType")]
        private readonly DTWAIN_GetCompressionTypeDelegate  _DTWAIN_GetCompressionType;

        [DTWAINNativeFunction("DTWAIN_GetConditionCodeString")]
        private readonly DTWAIN_GetConditionCodeStringDelegate  _DTWAIN_GetConditionCodeString;

        [DTWAINNativeFunction("DTWAIN_GetConditionCodeStringA")]
        private readonly DTWAIN_GetConditionCodeStringADelegate  _DTWAIN_GetConditionCodeStringA;

        [DTWAINNativeFunction("DTWAIN_GetConditionCodeStringW")]
        private readonly DTWAIN_GetConditionCodeStringWDelegate  _DTWAIN_GetConditionCodeStringW;

        [DTWAINNativeFunction("DTWAIN_GetContrast")]
        private readonly DTWAIN_GetContrastDelegate  _DTWAIN_GetContrast;

        [DTWAINNativeFunction("DTWAIN_GetContrastString")]
        private readonly DTWAIN_GetContrastStringDelegate  _DTWAIN_GetContrastString;

        [DTWAINNativeFunction("DTWAIN_GetContrastStringA")]
        private readonly DTWAIN_GetContrastStringADelegate  _DTWAIN_GetContrastStringA;

        [DTWAINNativeFunction("DTWAIN_GetContrastStringW")]
        private readonly DTWAIN_GetContrastStringWDelegate  _DTWAIN_GetContrastStringW;

        [DTWAINNativeFunction("DTWAIN_GetCountry")]
        private readonly DTWAIN_GetCountryDelegate  _DTWAIN_GetCountry;

        [DTWAINNativeFunction("DTWAIN_GetCurrentAcquiredImage")]
        private readonly DTWAIN_GetCurrentAcquiredImageDelegate  _DTWAIN_GetCurrentAcquiredImage;

        [DTWAINNativeFunction("DTWAIN_GetCurrentFileName")]
        private readonly DTWAIN_GetCurrentFileNameDelegate  _DTWAIN_GetCurrentFileName;

        [DTWAINNativeFunction("DTWAIN_GetCurrentFileNameA")]
        private readonly DTWAIN_GetCurrentFileNameADelegate  _DTWAIN_GetCurrentFileNameA;

        [DTWAINNativeFunction("DTWAIN_GetCurrentFileNameW")]
        private readonly DTWAIN_GetCurrentFileNameWDelegate  _DTWAIN_GetCurrentFileNameW;

        [DTWAINNativeFunction("DTWAIN_GetCurrentPageNum")]
        private readonly DTWAIN_GetCurrentPageNumDelegate  _DTWAIN_GetCurrentPageNum;

        [DTWAINNativeFunction("DTWAIN_GetCurrentRetryCount")]
        private readonly DTWAIN_GetCurrentRetryCountDelegate  _DTWAIN_GetCurrentRetryCount;

        [DTWAINNativeFunction("DTWAIN_GetCurrentTwainTriplet")]
        private readonly DTWAIN_GetCurrentTwainTripletDelegate  _DTWAIN_GetCurrentTwainTriplet;

        [DTWAINNativeFunction("DTWAIN_GetCustomDSData")]
        private readonly DTWAIN_GetCustomDSDataDelegate  _DTWAIN_GetCustomDSData;

        [DTWAINNativeFunction("DTWAIN_GetDSMFullName")]
        private readonly DTWAIN_GetDSMFullNameDelegate  _DTWAIN_GetDSMFullName;

        [DTWAINNativeFunction("DTWAIN_GetDSMFullNameA")]
        private readonly DTWAIN_GetDSMFullNameADelegate  _DTWAIN_GetDSMFullNameA;

        [DTWAINNativeFunction("DTWAIN_GetDSMFullNameW")]
        private readonly DTWAIN_GetDSMFullNameWDelegate  _DTWAIN_GetDSMFullNameW;

        [DTWAINNativeFunction("DTWAIN_GetDSMSearchOrder")]
        private readonly DTWAIN_GetDSMSearchOrderDelegate  _DTWAIN_GetDSMSearchOrder;

        [DTWAINNativeFunction("DTWAIN_GetDTWAINHandle")]
        private readonly DTWAIN_GetDTWAINHandleDelegate  _DTWAIN_GetDTWAINHandle;

        [DTWAINNativeFunction("DTWAIN_GetDeviceEvent")]
        private readonly DTWAIN_GetDeviceEventDelegate  _DTWAIN_GetDeviceEvent;

        [DTWAINNativeFunction("DTWAIN_GetDeviceEventEx")]
        private readonly DTWAIN_GetDeviceEventExDelegate  _DTWAIN_GetDeviceEventEx;

        [DTWAINNativeFunction("DTWAIN_GetDeviceEventInfo")]
        private readonly DTWAIN_GetDeviceEventInfoDelegate  _DTWAIN_GetDeviceEventInfo;

        [DTWAINNativeFunction("DTWAIN_GetDeviceNotifications")]
        private readonly DTWAIN_GetDeviceNotificationsDelegate  _DTWAIN_GetDeviceNotifications;

        [DTWAINNativeFunction("DTWAIN_GetDeviceTimeDate")]
        private readonly DTWAIN_GetDeviceTimeDateDelegate  _DTWAIN_GetDeviceTimeDate;

        [DTWAINNativeFunction("DTWAIN_GetDeviceTimeDateA")]
        private readonly DTWAIN_GetDeviceTimeDateADelegate  _DTWAIN_GetDeviceTimeDateA;

        [DTWAINNativeFunction("DTWAIN_GetDeviceTimeDateW")]
        private readonly DTWAIN_GetDeviceTimeDateWDelegate  _DTWAIN_GetDeviceTimeDateW;

        [DTWAINNativeFunction("DTWAIN_GetDoubleFeedDetectLength")]
        private readonly DTWAIN_GetDoubleFeedDetectLengthDelegate  _DTWAIN_GetDoubleFeedDetectLength;

        [DTWAINNativeFunction("DTWAIN_GetDoubleFeedDetectValues")]
        private readonly DTWAIN_GetDoubleFeedDetectValuesDelegate  _DTWAIN_GetDoubleFeedDetectValues;

        [DTWAINNativeFunction("DTWAIN_GetDuplexType")]
        private readonly DTWAIN_GetDuplexTypeDelegate  _DTWAIN_GetDuplexType;

        [DTWAINNativeFunction("DTWAIN_GetErrorBuffer")]
        private readonly DTWAIN_GetErrorBufferDelegate  _DTWAIN_GetErrorBuffer;

        [DTWAINNativeFunction("DTWAIN_GetErrorBufferThreshold")]
        private readonly DTWAIN_GetErrorBufferThresholdDelegate  _DTWAIN_GetErrorBufferThreshold;

        [DTWAINNativeFunction("DTWAIN_GetErrorCallback")]
        private readonly DTWAIN_GetErrorCallbackDelegate  _DTWAIN_GetErrorCallback;

        [DTWAINNativeFunction("DTWAIN_GetErrorCallback64")]
        private readonly DTWAIN_GetErrorCallback64Delegate  _DTWAIN_GetErrorCallback64;

        [DTWAINNativeFunction("DTWAIN_GetErrorString")]
        private readonly DTWAIN_GetErrorStringDelegate  _DTWAIN_GetErrorString;

        [DTWAINNativeFunction("DTWAIN_GetErrorStringA")]
        private readonly DTWAIN_GetErrorStringADelegate  _DTWAIN_GetErrorStringA;

        [DTWAINNativeFunction("DTWAIN_GetErrorStringW")]
        private readonly DTWAIN_GetErrorStringWDelegate  _DTWAIN_GetErrorStringW;

        [DTWAINNativeFunction("DTWAIN_GetExtCapFromName")]
        private readonly DTWAIN_GetExtCapFromNameDelegate  _DTWAIN_GetExtCapFromName;

        [DTWAINNativeFunction("DTWAIN_GetExtCapFromNameA")]
        private readonly DTWAIN_GetExtCapFromNameADelegate  _DTWAIN_GetExtCapFromNameA;

        [DTWAINNativeFunction("DTWAIN_GetExtCapFromNameW")]
        private readonly DTWAIN_GetExtCapFromNameWDelegate  _DTWAIN_GetExtCapFromNameW;

        [DTWAINNativeFunction("DTWAIN_GetExtImageInfo")]
        private readonly DTWAIN_GetExtImageInfoDelegate  _DTWAIN_GetExtImageInfo;

        [DTWAINNativeFunction("DTWAIN_GetExtImageInfoData")]
        private readonly DTWAIN_GetExtImageInfoDataDelegate  _DTWAIN_GetExtImageInfoData;

        [DTWAINNativeFunction("DTWAIN_GetExtImageInfoDataEx")]
        private readonly DTWAIN_GetExtImageInfoDataExDelegate  _DTWAIN_GetExtImageInfoDataEx;

        [DTWAINNativeFunction("DTWAIN_GetExtImageInfoItem")]
        private readonly DTWAIN_GetExtImageInfoItemDelegate  _DTWAIN_GetExtImageInfoItem;

        [DTWAINNativeFunction("DTWAIN_GetExtImageInfoItemEx")]
        private readonly DTWAIN_GetExtImageInfoItemExDelegate  _DTWAIN_GetExtImageInfoItemEx;

        [DTWAINNativeFunction("DTWAIN_GetExtNameFromCap")]
        private readonly DTWAIN_GetExtNameFromCapDelegate  _DTWAIN_GetExtNameFromCap;

        [DTWAINNativeFunction("DTWAIN_GetExtNameFromCapA")]
        private readonly DTWAIN_GetExtNameFromCapADelegate  _DTWAIN_GetExtNameFromCapA;

        [DTWAINNativeFunction("DTWAIN_GetExtNameFromCapW")]
        private readonly DTWAIN_GetExtNameFromCapWDelegate  _DTWAIN_GetExtNameFromCapW;

        [DTWAINNativeFunction("DTWAIN_GetFeederAlignment")]
        private readonly DTWAIN_GetFeederAlignmentDelegate  _DTWAIN_GetFeederAlignment;

        [DTWAINNativeFunction("DTWAIN_GetFeederFuncs")]
        private readonly DTWAIN_GetFeederFuncsDelegate  _DTWAIN_GetFeederFuncs;

        [DTWAINNativeFunction("DTWAIN_GetFeederOrder")]
        private readonly DTWAIN_GetFeederOrderDelegate  _DTWAIN_GetFeederOrder;

        [DTWAINNativeFunction("DTWAIN_GetFeederWaitTime")]
        private readonly DTWAIN_GetFeederWaitTimeDelegate  _DTWAIN_GetFeederWaitTime;

        [DTWAINNativeFunction("DTWAIN_GetFileCompressionType")]
        private readonly DTWAIN_GetFileCompressionTypeDelegate  _DTWAIN_GetFileCompressionType;

        [DTWAINNativeFunction("DTWAIN_GetFileSavePageCount")]
        private readonly DTWAIN_GetFileSavePageCountDelegate  _DTWAIN_GetFileSavePageCount;

        [DTWAINNativeFunction("DTWAIN_GetFileTypeExtensions")]
        private readonly DTWAIN_GetFileTypeExtensionsDelegate  _DTWAIN_GetFileTypeExtensions;

        [DTWAINNativeFunction("DTWAIN_GetFileTypeExtensionsA")]
        private readonly DTWAIN_GetFileTypeExtensionsADelegate  _DTWAIN_GetFileTypeExtensionsA;

        [DTWAINNativeFunction("DTWAIN_GetFileTypeExtensionsW")]
        private readonly DTWAIN_GetFileTypeExtensionsWDelegate  _DTWAIN_GetFileTypeExtensionsW;

        [DTWAINNativeFunction("DTWAIN_GetFileTypeName")]
        private readonly DTWAIN_GetFileTypeNameDelegate  _DTWAIN_GetFileTypeName;

        [DTWAINNativeFunction("DTWAIN_GetFileTypeNameA")]
        private readonly DTWAIN_GetFileTypeNameADelegate  _DTWAIN_GetFileTypeNameA;

        [DTWAINNativeFunction("DTWAIN_GetFileTypeNameW")]
        private readonly DTWAIN_GetFileTypeNameWDelegate  _DTWAIN_GetFileTypeNameW;

        [DTWAINNativeFunction("DTWAIN_GetHalftone")]
        private readonly DTWAIN_GetHalftoneDelegate  _DTWAIN_GetHalftone;

        [DTWAINNativeFunction("DTWAIN_GetHalftoneA")]
        private readonly DTWAIN_GetHalftoneADelegate  _DTWAIN_GetHalftoneA;

        [DTWAINNativeFunction("DTWAIN_GetHalftoneW")]
        private readonly DTWAIN_GetHalftoneWDelegate  _DTWAIN_GetHalftoneW;

        [DTWAINNativeFunction("DTWAIN_GetHighlight")]
        private readonly DTWAIN_GetHighlightDelegate  _DTWAIN_GetHighlight;

        [DTWAINNativeFunction("DTWAIN_GetHighlightString")]
        private readonly DTWAIN_GetHighlightStringDelegate  _DTWAIN_GetHighlightString;

        [DTWAINNativeFunction("DTWAIN_GetHighlightStringA")]
        private readonly DTWAIN_GetHighlightStringADelegate  _DTWAIN_GetHighlightStringA;

        [DTWAINNativeFunction("DTWAIN_GetHighlightStringW")]
        private readonly DTWAIN_GetHighlightStringWDelegate  _DTWAIN_GetHighlightStringW;

        [DTWAINNativeFunction("DTWAIN_GetImageInfo")]
        private readonly DTWAIN_GetImageInfoDelegate  _DTWAIN_GetImageInfo;

        [DTWAINNativeFunction("DTWAIN_GetImageInfoString")]
        private readonly DTWAIN_GetImageInfoStringDelegate  _DTWAIN_GetImageInfoString;

        [DTWAINNativeFunction("DTWAIN_GetImageInfoStringA")]
        private readonly DTWAIN_GetImageInfoStringADelegate  _DTWAIN_GetImageInfoStringA;

        [DTWAINNativeFunction("DTWAIN_GetImageInfoStringW")]
        private readonly DTWAIN_GetImageInfoStringWDelegate  _DTWAIN_GetImageInfoStringW;

        [DTWAINNativeFunction("DTWAIN_GetJobControl")]
        private readonly DTWAIN_GetJobControlDelegate  _DTWAIN_GetJobControl;

        [DTWAINNativeFunction("DTWAIN_GetJpegValues")]
        private readonly DTWAIN_GetJpegValuesDelegate  _DTWAIN_GetJpegValues;

        [DTWAINNativeFunction("DTWAIN_GetJpegXRValues")]
        private readonly DTWAIN_GetJpegXRValuesDelegate  _DTWAIN_GetJpegXRValues;

        [DTWAINNativeFunction("DTWAIN_GetLanguage")]
        private readonly DTWAIN_GetLanguageDelegate  _DTWAIN_GetLanguage;

        [DTWAINNativeFunction("DTWAIN_GetLastError")]
        private readonly DTWAIN_GetLastErrorDelegate  _DTWAIN_GetLastError;

        [DTWAINNativeFunction("DTWAIN_GetLibraryPath")]
        private readonly DTWAIN_GetLibraryPathDelegate  _DTWAIN_GetLibraryPath;

        [DTWAINNativeFunction("DTWAIN_GetLibraryPathA")]
        private readonly DTWAIN_GetLibraryPathADelegate  _DTWAIN_GetLibraryPathA;

        [DTWAINNativeFunction("DTWAIN_GetLibraryPathW")]
        private readonly DTWAIN_GetLibraryPathWDelegate  _DTWAIN_GetLibraryPathW;

        [DTWAINNativeFunction("DTWAIN_GetLightPath")]
        private readonly DTWAIN_GetLightPathDelegate  _DTWAIN_GetLightPath;

        [DTWAINNativeFunction("DTWAIN_GetLightSource")]
        private readonly DTWAIN_GetLightSourceDelegate  _DTWAIN_GetLightSource;

        [DTWAINNativeFunction("DTWAIN_GetLightSources")]
        private readonly DTWAIN_GetLightSourcesDelegate  _DTWAIN_GetLightSources;

        [DTWAINNativeFunction("DTWAIN_GetLightSourcesEx")]
        private readonly DTWAIN_GetLightSourcesExDelegate  _DTWAIN_GetLightSourcesEx;

        [DTWAINNativeFunction("DTWAIN_GetLoggerCallback")]
        private readonly DTWAIN_GetLoggerCallbackDelegate  _DTWAIN_GetLoggerCallback;

        [DTWAINNativeFunction("DTWAIN_GetLoggerCallbackA")]
        private readonly DTWAIN_GetLoggerCallbackADelegate  _DTWAIN_GetLoggerCallbackA;

        [DTWAINNativeFunction("DTWAIN_GetLoggerCallbackW")]
        private readonly DTWAIN_GetLoggerCallbackWDelegate  _DTWAIN_GetLoggerCallbackW;

        [DTWAINNativeFunction("DTWAIN_GetManualDuplexCount")]
        private readonly DTWAIN_GetManualDuplexCountDelegate  _DTWAIN_GetManualDuplexCount;

        [DTWAINNativeFunction("DTWAIN_GetMaxAcquisitions")]
        private readonly DTWAIN_GetMaxAcquisitionsDelegate  _DTWAIN_GetMaxAcquisitions;

        [DTWAINNativeFunction("DTWAIN_GetMaxBuffers")]
        private readonly DTWAIN_GetMaxBuffersDelegate  _DTWAIN_GetMaxBuffers;

        [DTWAINNativeFunction("DTWAIN_GetMaxPagesToAcquire")]
        private readonly DTWAIN_GetMaxPagesToAcquireDelegate  _DTWAIN_GetMaxPagesToAcquire;

        [DTWAINNativeFunction("DTWAIN_GetMaxRetryAttempts")]
        private readonly DTWAIN_GetMaxRetryAttemptsDelegate  _DTWAIN_GetMaxRetryAttempts;

        [DTWAINNativeFunction("DTWAIN_GetNameFromCap")]
        private readonly DTWAIN_GetNameFromCapDelegate  _DTWAIN_GetNameFromCap;

        [DTWAINNativeFunction("DTWAIN_GetNameFromCapA")]
        private readonly DTWAIN_GetNameFromCapADelegate  _DTWAIN_GetNameFromCapA;

        [DTWAINNativeFunction("DTWAIN_GetNameFromCapW")]
        private readonly DTWAIN_GetNameFromCapWDelegate  _DTWAIN_GetNameFromCapW;

        [DTWAINNativeFunction("DTWAIN_GetNoiseFilter")]
        private readonly DTWAIN_GetNoiseFilterDelegate  _DTWAIN_GetNoiseFilter;

        [DTWAINNativeFunction("DTWAIN_GetNumAcquiredImages")]
        private readonly DTWAIN_GetNumAcquiredImagesDelegate  _DTWAIN_GetNumAcquiredImages;

        [DTWAINNativeFunction("DTWAIN_GetNumAcquisitions")]
        private readonly DTWAIN_GetNumAcquisitionsDelegate  _DTWAIN_GetNumAcquisitions;

        [DTWAINNativeFunction("DTWAIN_GetOCRCapValues")]
        private readonly DTWAIN_GetOCRCapValuesDelegate  _DTWAIN_GetOCRCapValues;

        [DTWAINNativeFunction("DTWAIN_GetOCRErrorString")]
        private readonly DTWAIN_GetOCRErrorStringDelegate  _DTWAIN_GetOCRErrorString;

        [DTWAINNativeFunction("DTWAIN_GetOCRErrorStringA")]
        private readonly DTWAIN_GetOCRErrorStringADelegate  _DTWAIN_GetOCRErrorStringA;

        [DTWAINNativeFunction("DTWAIN_GetOCRErrorStringW")]
        private readonly DTWAIN_GetOCRErrorStringWDelegate  _DTWAIN_GetOCRErrorStringW;

        [DTWAINNativeFunction("DTWAIN_GetOCRLastError")]
        private readonly DTWAIN_GetOCRLastErrorDelegate  _DTWAIN_GetOCRLastError;

        [DTWAINNativeFunction("DTWAIN_GetOCRMajorMinorVersion")]
        private readonly DTWAIN_GetOCRMajorMinorVersionDelegate  _DTWAIN_GetOCRMajorMinorVersion;

        [DTWAINNativeFunction("DTWAIN_GetOCRManufacturer")]
        private readonly DTWAIN_GetOCRManufacturerDelegate  _DTWAIN_GetOCRManufacturer;

        [DTWAINNativeFunction("DTWAIN_GetOCRManufacturerA")]
        private readonly DTWAIN_GetOCRManufacturerADelegate  _DTWAIN_GetOCRManufacturerA;

        [DTWAINNativeFunction("DTWAIN_GetOCRManufacturerW")]
        private readonly DTWAIN_GetOCRManufacturerWDelegate  _DTWAIN_GetOCRManufacturerW;

        [DTWAINNativeFunction("DTWAIN_GetOCRProductFamily")]
        private readonly DTWAIN_GetOCRProductFamilyDelegate  _DTWAIN_GetOCRProductFamily;

        [DTWAINNativeFunction("DTWAIN_GetOCRProductFamilyA")]
        private readonly DTWAIN_GetOCRProductFamilyADelegate  _DTWAIN_GetOCRProductFamilyA;

        [DTWAINNativeFunction("DTWAIN_GetOCRProductFamilyW")]
        private readonly DTWAIN_GetOCRProductFamilyWDelegate  _DTWAIN_GetOCRProductFamilyW;

        [DTWAINNativeFunction("DTWAIN_GetOCRProductName")]
        private readonly DTWAIN_GetOCRProductNameDelegate  _DTWAIN_GetOCRProductName;

        [DTWAINNativeFunction("DTWAIN_GetOCRProductNameA")]
        private readonly DTWAIN_GetOCRProductNameADelegate  _DTWAIN_GetOCRProductNameA;

        [DTWAINNativeFunction("DTWAIN_GetOCRProductNameW")]
        private readonly DTWAIN_GetOCRProductNameWDelegate  _DTWAIN_GetOCRProductNameW;

        [DTWAINNativeFunction("DTWAIN_GetOCRText")]
        private readonly DTWAIN_GetOCRTextDelegate  _DTWAIN_GetOCRText;

        [DTWAINNativeFunction("DTWAIN_GetOCRTextA")]
        private readonly DTWAIN_GetOCRTextADelegate  _DTWAIN_GetOCRTextA;

        [DTWAINNativeFunction("DTWAIN_GetOCRTextInfoFloat")]
        private readonly DTWAIN_GetOCRTextInfoFloatDelegate  _DTWAIN_GetOCRTextInfoFloat;

        [DTWAINNativeFunction("DTWAIN_GetOCRTextInfoFloatEx")]
        private readonly DTWAIN_GetOCRTextInfoFloatExDelegate  _DTWAIN_GetOCRTextInfoFloatEx;

        [DTWAINNativeFunction("DTWAIN_GetOCRTextInfoHandle")]
        private readonly DTWAIN_GetOCRTextInfoHandleDelegate  _DTWAIN_GetOCRTextInfoHandle;

        [DTWAINNativeFunction("DTWAIN_GetOCRTextInfoLong")]
        private readonly DTWAIN_GetOCRTextInfoLongDelegate  _DTWAIN_GetOCRTextInfoLong;

        [DTWAINNativeFunction("DTWAIN_GetOCRTextInfoLongEx")]
        private readonly DTWAIN_GetOCRTextInfoLongExDelegate  _DTWAIN_GetOCRTextInfoLongEx;

        [DTWAINNativeFunction("DTWAIN_GetOCRTextW")]
        private readonly DTWAIN_GetOCRTextWDelegate  _DTWAIN_GetOCRTextW;

        [DTWAINNativeFunction("DTWAIN_GetOCRVersionInfo")]
        private readonly DTWAIN_GetOCRVersionInfoDelegate  _DTWAIN_GetOCRVersionInfo;

        [DTWAINNativeFunction("DTWAIN_GetOCRVersionInfoA")]
        private readonly DTWAIN_GetOCRVersionInfoADelegate  _DTWAIN_GetOCRVersionInfoA;

        [DTWAINNativeFunction("DTWAIN_GetOCRVersionInfoW")]
        private readonly DTWAIN_GetOCRVersionInfoWDelegate  _DTWAIN_GetOCRVersionInfoW;

        [DTWAINNativeFunction("DTWAIN_GetOrientation")]
        private readonly DTWAIN_GetOrientationDelegate  _DTWAIN_GetOrientation;

        [DTWAINNativeFunction("DTWAIN_GetOverscan")]
        private readonly DTWAIN_GetOverscanDelegate  _DTWAIN_GetOverscan;

        [DTWAINNativeFunction("DTWAIN_GetPDFTextElementFloat")]
        private readonly DTWAIN_GetPDFTextElementFloatDelegate  _DTWAIN_GetPDFTextElementFloat;

        [DTWAINNativeFunction("DTWAIN_GetPDFTextElementLong")]
        private readonly DTWAIN_GetPDFTextElementLongDelegate  _DTWAIN_GetPDFTextElementLong;

        [DTWAINNativeFunction("DTWAIN_GetPDFTextElementString")]
        private readonly DTWAIN_GetPDFTextElementStringDelegate  _DTWAIN_GetPDFTextElementString;

        [DTWAINNativeFunction("DTWAIN_GetPDFTextElementStringA")]
        private readonly DTWAIN_GetPDFTextElementStringADelegate  _DTWAIN_GetPDFTextElementStringA;

        [DTWAINNativeFunction("DTWAIN_GetPDFTextElementStringW")]
        private readonly DTWAIN_GetPDFTextElementStringWDelegate  _DTWAIN_GetPDFTextElementStringW;

        [DTWAINNativeFunction("DTWAIN_GetPDFType1FontName")]
        private readonly DTWAIN_GetPDFType1FontNameDelegate  _DTWAIN_GetPDFType1FontName;

        [DTWAINNativeFunction("DTWAIN_GetPDFType1FontNameA")]
        private readonly DTWAIN_GetPDFType1FontNameADelegate  _DTWAIN_GetPDFType1FontNameA;

        [DTWAINNativeFunction("DTWAIN_GetPDFType1FontNameW")]
        private readonly DTWAIN_GetPDFType1FontNameWDelegate  _DTWAIN_GetPDFType1FontNameW;

        [DTWAINNativeFunction("DTWAIN_GetPaperSize")]
        private readonly DTWAIN_GetPaperSizeDelegate  _DTWAIN_GetPaperSize;

        [DTWAINNativeFunction("DTWAIN_GetPaperSizeName")]
        private readonly DTWAIN_GetPaperSizeNameDelegate  _DTWAIN_GetPaperSizeName;

        [DTWAINNativeFunction("DTWAIN_GetPaperSizeNameA")]
        private readonly DTWAIN_GetPaperSizeNameADelegate  _DTWAIN_GetPaperSizeNameA;

        [DTWAINNativeFunction("DTWAIN_GetPaperSizeNameW")]
        private readonly DTWAIN_GetPaperSizeNameWDelegate  _DTWAIN_GetPaperSizeNameW;

        [DTWAINNativeFunction("DTWAIN_GetPatchMaxPriorities")]
        private readonly DTWAIN_GetPatchMaxPrioritiesDelegate  _DTWAIN_GetPatchMaxPriorities;

        [DTWAINNativeFunction("DTWAIN_GetPatchMaxRetries")]
        private readonly DTWAIN_GetPatchMaxRetriesDelegate  _DTWAIN_GetPatchMaxRetries;

        [DTWAINNativeFunction("DTWAIN_GetPatchPriorities")]
        private readonly DTWAIN_GetPatchPrioritiesDelegate  _DTWAIN_GetPatchPriorities;

        [DTWAINNativeFunction("DTWAIN_GetPatchSearchMode")]
        private readonly DTWAIN_GetPatchSearchModeDelegate  _DTWAIN_GetPatchSearchMode;

        [DTWAINNativeFunction("DTWAIN_GetPatchTimeOut")]
        private readonly DTWAIN_GetPatchTimeOutDelegate  _DTWAIN_GetPatchTimeOut;

        [DTWAINNativeFunction("DTWAIN_GetPixelFlavor")]
        private readonly DTWAIN_GetPixelFlavorDelegate  _DTWAIN_GetPixelFlavor;

        [DTWAINNativeFunction("DTWAIN_GetPixelType")]
        private readonly DTWAIN_GetPixelTypeDelegate  _DTWAIN_GetPixelType;

        [DTWAINNativeFunction("DTWAIN_GetPrinter")]
        private readonly DTWAIN_GetPrinterDelegate  _DTWAIN_GetPrinter;

        [DTWAINNativeFunction("DTWAIN_GetPrinterStartNumber")]
        private readonly DTWAIN_GetPrinterStartNumberDelegate  _DTWAIN_GetPrinterStartNumber;

        [DTWAINNativeFunction("DTWAIN_GetPrinterStringMode")]
        private readonly DTWAIN_GetPrinterStringModeDelegate  _DTWAIN_GetPrinterStringMode;

        [DTWAINNativeFunction("DTWAIN_GetPrinterStrings")]
        private readonly DTWAIN_GetPrinterStringsDelegate  _DTWAIN_GetPrinterStrings;

        [DTWAINNativeFunction("DTWAIN_GetPrinterSuffixString")]
        private readonly DTWAIN_GetPrinterSuffixStringDelegate  _DTWAIN_GetPrinterSuffixString;

        [DTWAINNativeFunction("DTWAIN_GetPrinterSuffixStringA")]
        private readonly DTWAIN_GetPrinterSuffixStringADelegate  _DTWAIN_GetPrinterSuffixStringA;

        [DTWAINNativeFunction("DTWAIN_GetPrinterSuffixStringW")]
        private readonly DTWAIN_GetPrinterSuffixStringWDelegate  _DTWAIN_GetPrinterSuffixStringW;

        [DTWAINNativeFunction("DTWAIN_GetRegisteredMsg")]
        private readonly DTWAIN_GetRegisteredMsgDelegate  _DTWAIN_GetRegisteredMsg;

        [DTWAINNativeFunction("DTWAIN_GetResolution")]
        private readonly DTWAIN_GetResolutionDelegate  _DTWAIN_GetResolution;

        [DTWAINNativeFunction("DTWAIN_GetResolutionString")]
        private readonly DTWAIN_GetResolutionStringDelegate  _DTWAIN_GetResolutionString;

        [DTWAINNativeFunction("DTWAIN_GetResolutionStringA")]
        private readonly DTWAIN_GetResolutionStringADelegate  _DTWAIN_GetResolutionStringA;

        [DTWAINNativeFunction("DTWAIN_GetResolutionStringW")]
        private readonly DTWAIN_GetResolutionStringWDelegate  _DTWAIN_GetResolutionStringW;

        [DTWAINNativeFunction("DTWAIN_GetResourceString")]
        private readonly DTWAIN_GetResourceStringDelegate  _DTWAIN_GetResourceString;

        [DTWAINNativeFunction("DTWAIN_GetResourceStringA")]
        private readonly DTWAIN_GetResourceStringADelegate  _DTWAIN_GetResourceStringA;

        [DTWAINNativeFunction("DTWAIN_GetResourceStringW")]
        private readonly DTWAIN_GetResourceStringWDelegate  _DTWAIN_GetResourceStringW;

        [DTWAINNativeFunction("DTWAIN_GetRotation")]
        private readonly DTWAIN_GetRotationDelegate  _DTWAIN_GetRotation;

        [DTWAINNativeFunction("DTWAIN_GetRotationString")]
        private readonly DTWAIN_GetRotationStringDelegate  _DTWAIN_GetRotationString;

        [DTWAINNativeFunction("DTWAIN_GetRotationStringA")]
        private readonly DTWAIN_GetRotationStringADelegate  _DTWAIN_GetRotationStringA;

        [DTWAINNativeFunction("DTWAIN_GetRotationStringW")]
        private readonly DTWAIN_GetRotationStringWDelegate  _DTWAIN_GetRotationStringW;

        [DTWAINNativeFunction("DTWAIN_GetSaveFileName")]
        private readonly DTWAIN_GetSaveFileNameDelegate  _DTWAIN_GetSaveFileName;

        [DTWAINNativeFunction("DTWAIN_GetSaveFileNameA")]
        private readonly DTWAIN_GetSaveFileNameADelegate  _DTWAIN_GetSaveFileNameA;

        [DTWAINNativeFunction("DTWAIN_GetSaveFileNameW")]
        private readonly DTWAIN_GetSaveFileNameWDelegate  _DTWAIN_GetSaveFileNameW;

        [DTWAINNativeFunction("DTWAIN_GetSessionDetails")]
        private readonly DTWAIN_GetSessionDetailsDelegate  _DTWAIN_GetSessionDetails;

        [DTWAINNativeFunction("DTWAIN_GetSessionDetailsA")]
        private readonly DTWAIN_GetSessionDetailsADelegate  _DTWAIN_GetSessionDetailsA;

        [DTWAINNativeFunction("DTWAIN_GetSessionDetailsW")]
        private readonly DTWAIN_GetSessionDetailsWDelegate  _DTWAIN_GetSessionDetailsW;

        [DTWAINNativeFunction("DTWAIN_GetShadow")]
        private readonly DTWAIN_GetShadowDelegate  _DTWAIN_GetShadow;

        [DTWAINNativeFunction("DTWAIN_GetShadowString")]
        private readonly DTWAIN_GetShadowStringDelegate  _DTWAIN_GetShadowString;

        [DTWAINNativeFunction("DTWAIN_GetShadowStringA")]
        private readonly DTWAIN_GetShadowStringADelegate  _DTWAIN_GetShadowStringA;

        [DTWAINNativeFunction("DTWAIN_GetShadowStringW")]
        private readonly DTWAIN_GetShadowStringWDelegate  _DTWAIN_GetShadowStringW;

        [DTWAINNativeFunction("DTWAIN_GetShortVersionString")]
        private readonly DTWAIN_GetShortVersionStringDelegate  _DTWAIN_GetShortVersionString;

        [DTWAINNativeFunction("DTWAIN_GetShortVersionStringA")]
        private readonly DTWAIN_GetShortVersionStringADelegate  _DTWAIN_GetShortVersionStringA;

        [DTWAINNativeFunction("DTWAIN_GetShortVersionStringW")]
        private readonly DTWAIN_GetShortVersionStringWDelegate  _DTWAIN_GetShortVersionStringW;

        [DTWAINNativeFunction("DTWAIN_GetSourceAcquisitions")]
        private readonly DTWAIN_GetSourceAcquisitionsDelegate  _DTWAIN_GetSourceAcquisitions;

        [DTWAINNativeFunction("DTWAIN_GetSourceDetails")]
        private readonly DTWAIN_GetSourceDetailsDelegate  _DTWAIN_GetSourceDetails;

        [DTWAINNativeFunction("DTWAIN_GetSourceDetailsA")]
        private readonly DTWAIN_GetSourceDetailsADelegate  _DTWAIN_GetSourceDetailsA;

        [DTWAINNativeFunction("DTWAIN_GetSourceDetailsW")]
        private readonly DTWAIN_GetSourceDetailsWDelegate  _DTWAIN_GetSourceDetailsW;

        [DTWAINNativeFunction("DTWAIN_GetSourceID")]
        private readonly DTWAIN_GetSourceIDDelegate  _DTWAIN_GetSourceID;

        [DTWAINNativeFunction("DTWAIN_GetSourceIDEx")]
        private readonly DTWAIN_GetSourceIDExDelegate  _DTWAIN_GetSourceIDEx;

        [DTWAINNativeFunction("DTWAIN_GetSourceManufacturer")]
        private readonly DTWAIN_GetSourceManufacturerDelegate  _DTWAIN_GetSourceManufacturer;

        [DTWAINNativeFunction("DTWAIN_GetSourceManufacturerA")]
        private readonly DTWAIN_GetSourceManufacturerADelegate  _DTWAIN_GetSourceManufacturerA;

        [DTWAINNativeFunction("DTWAIN_GetSourceManufacturerW")]
        private readonly DTWAIN_GetSourceManufacturerWDelegate  _DTWAIN_GetSourceManufacturerW;

        [DTWAINNativeFunction("DTWAIN_GetSourceProductFamily")]
        private readonly DTWAIN_GetSourceProductFamilyDelegate  _DTWAIN_GetSourceProductFamily;

        [DTWAINNativeFunction("DTWAIN_GetSourceProductFamilyA")]
        private readonly DTWAIN_GetSourceProductFamilyADelegate  _DTWAIN_GetSourceProductFamilyA;

        [DTWAINNativeFunction("DTWAIN_GetSourceProductFamilyW")]
        private readonly DTWAIN_GetSourceProductFamilyWDelegate  _DTWAIN_GetSourceProductFamilyW;

        [DTWAINNativeFunction("DTWAIN_GetSourceProductName")]
        private readonly DTWAIN_GetSourceProductNameDelegate  _DTWAIN_GetSourceProductName;

        [DTWAINNativeFunction("DTWAIN_GetSourceProductNameA")]
        private readonly DTWAIN_GetSourceProductNameADelegate  _DTWAIN_GetSourceProductNameA;

        [DTWAINNativeFunction("DTWAIN_GetSourceProductNameW")]
        private readonly DTWAIN_GetSourceProductNameWDelegate  _DTWAIN_GetSourceProductNameW;

        [DTWAINNativeFunction("DTWAIN_GetSourceUnit")]
        private readonly DTWAIN_GetSourceUnitDelegate  _DTWAIN_GetSourceUnit;

        [DTWAINNativeFunction("DTWAIN_GetSourceVersionInfo")]
        private readonly DTWAIN_GetSourceVersionInfoDelegate  _DTWAIN_GetSourceVersionInfo;

        [DTWAINNativeFunction("DTWAIN_GetSourceVersionInfoA")]
        private readonly DTWAIN_GetSourceVersionInfoADelegate  _DTWAIN_GetSourceVersionInfoA;

        [DTWAINNativeFunction("DTWAIN_GetSourceVersionInfoW")]
        private readonly DTWAIN_GetSourceVersionInfoWDelegate  _DTWAIN_GetSourceVersionInfoW;

        [DTWAINNativeFunction("DTWAIN_GetSourceVersionNumber")]
        private readonly DTWAIN_GetSourceVersionNumberDelegate  _DTWAIN_GetSourceVersionNumber;

        [DTWAINNativeFunction("DTWAIN_GetStaticLibVersion")]
        private readonly DTWAIN_GetStaticLibVersionDelegate  _DTWAIN_GetStaticLibVersion;

        [DTWAINNativeFunction("DTWAIN_GetTempFileDirectory")]
        private readonly DTWAIN_GetTempFileDirectoryDelegate  _DTWAIN_GetTempFileDirectory;

        [DTWAINNativeFunction("DTWAIN_GetTempFileDirectoryA")]
        private readonly DTWAIN_GetTempFileDirectoryADelegate  _DTWAIN_GetTempFileDirectoryA;

        [DTWAINNativeFunction("DTWAIN_GetTempFileDirectoryW")]
        private readonly DTWAIN_GetTempFileDirectoryWDelegate  _DTWAIN_GetTempFileDirectoryW;

        [DTWAINNativeFunction("DTWAIN_GetThreshold")]
        private readonly DTWAIN_GetThresholdDelegate  _DTWAIN_GetThreshold;

        [DTWAINNativeFunction("DTWAIN_GetThresholdString")]
        private readonly DTWAIN_GetThresholdStringDelegate  _DTWAIN_GetThresholdString;

        [DTWAINNativeFunction("DTWAIN_GetThresholdStringA")]
        private readonly DTWAIN_GetThresholdStringADelegate  _DTWAIN_GetThresholdStringA;

        [DTWAINNativeFunction("DTWAIN_GetThresholdStringW")]
        private readonly DTWAIN_GetThresholdStringWDelegate  _DTWAIN_GetThresholdStringW;

        [DTWAINNativeFunction("DTWAIN_GetTimeDate")]
        private readonly DTWAIN_GetTimeDateDelegate  _DTWAIN_GetTimeDate;

        [DTWAINNativeFunction("DTWAIN_GetTimeDateA")]
        private readonly DTWAIN_GetTimeDateADelegate  _DTWAIN_GetTimeDateA;

        [DTWAINNativeFunction("DTWAIN_GetTimeDateW")]
        private readonly DTWAIN_GetTimeDateWDelegate  _DTWAIN_GetTimeDateW;

        [DTWAINNativeFunction("DTWAIN_GetTwainAppID")]
        private readonly DTWAIN_GetTwainAppIDDelegate  _DTWAIN_GetTwainAppID;

        [DTWAINNativeFunction("DTWAIN_GetTwainAppIDEx")]
        private readonly DTWAIN_GetTwainAppIDExDelegate  _DTWAIN_GetTwainAppIDEx;

        [DTWAINNativeFunction("DTWAIN_GetTwainAvailability")]
        private readonly DTWAIN_GetTwainAvailabilityDelegate  _DTWAIN_GetTwainAvailability;

        [DTWAINNativeFunction("DTWAIN_GetTwainAvailabilityEx")]
        private readonly DTWAIN_GetTwainAvailabilityExDelegate  _DTWAIN_GetTwainAvailabilityEx;

        [DTWAINNativeFunction("DTWAIN_GetTwainAvailabilityExA")]
        private readonly DTWAIN_GetTwainAvailabilityExADelegate  _DTWAIN_GetTwainAvailabilityExA;

        [DTWAINNativeFunction("DTWAIN_GetTwainAvailabilityExW")]
        private readonly DTWAIN_GetTwainAvailabilityExWDelegate  _DTWAIN_GetTwainAvailabilityExW;

        [DTWAINNativeFunction("DTWAIN_GetTwainCountryName")]
        private readonly DTWAIN_GetTwainCountryNameDelegate  _DTWAIN_GetTwainCountryName;

        [DTWAINNativeFunction("DTWAIN_GetTwainCountryNameA")]
        private readonly DTWAIN_GetTwainCountryNameADelegate  _DTWAIN_GetTwainCountryNameA;

        [DTWAINNativeFunction("DTWAIN_GetTwainCountryNameW")]
        private readonly DTWAIN_GetTwainCountryNameWDelegate  _DTWAIN_GetTwainCountryNameW;

        [DTWAINNativeFunction("DTWAIN_GetTwainCountryValue")]
        private readonly DTWAIN_GetTwainCountryValueDelegate  _DTWAIN_GetTwainCountryValue;

        [DTWAINNativeFunction("DTWAIN_GetTwainCountryValueA")]
        private readonly DTWAIN_GetTwainCountryValueADelegate  _DTWAIN_GetTwainCountryValueA;

        [DTWAINNativeFunction("DTWAIN_GetTwainCountryValueW")]
        private readonly DTWAIN_GetTwainCountryValueWDelegate  _DTWAIN_GetTwainCountryValueW;

        [DTWAINNativeFunction("DTWAIN_GetTwainHwnd")]
        private readonly DTWAIN_GetTwainHwndDelegate  _DTWAIN_GetTwainHwnd;

        [DTWAINNativeFunction("DTWAIN_GetTwainIDFromName")]
        private readonly DTWAIN_GetTwainIDFromNameDelegate  _DTWAIN_GetTwainIDFromName;

        [DTWAINNativeFunction("DTWAIN_GetTwainIDFromNameA")]
        private readonly DTWAIN_GetTwainIDFromNameADelegate  _DTWAIN_GetTwainIDFromNameA;

        [DTWAINNativeFunction("DTWAIN_GetTwainIDFromNameW")]
        private readonly DTWAIN_GetTwainIDFromNameWDelegate  _DTWAIN_GetTwainIDFromNameW;

        [DTWAINNativeFunction("DTWAIN_GetTwainLanguageName")]
        private readonly DTWAIN_GetTwainLanguageNameDelegate  _DTWAIN_GetTwainLanguageName;

        [DTWAINNativeFunction("DTWAIN_GetTwainLanguageNameA")]
        private readonly DTWAIN_GetTwainLanguageNameADelegate  _DTWAIN_GetTwainLanguageNameA;

        [DTWAINNativeFunction("DTWAIN_GetTwainLanguageNameW")]
        private readonly DTWAIN_GetTwainLanguageNameWDelegate  _DTWAIN_GetTwainLanguageNameW;

        [DTWAINNativeFunction("DTWAIN_GetTwainLanguageValue")]
        private readonly DTWAIN_GetTwainLanguageValueDelegate  _DTWAIN_GetTwainLanguageValue;

        [DTWAINNativeFunction("DTWAIN_GetTwainLanguageValueA")]
        private readonly DTWAIN_GetTwainLanguageValueADelegate  _DTWAIN_GetTwainLanguageValueA;

        [DTWAINNativeFunction("DTWAIN_GetTwainLanguageValueW")]
        private readonly DTWAIN_GetTwainLanguageValueWDelegate  _DTWAIN_GetTwainLanguageValueW;

        [DTWAINNativeFunction("DTWAIN_GetTwainMode")]
        private readonly DTWAIN_GetTwainModeDelegate  _DTWAIN_GetTwainMode;

        [DTWAINNativeFunction("DTWAIN_GetTwainNameFromConstant")]
        private readonly DTWAIN_GetTwainNameFromConstantDelegate  _DTWAIN_GetTwainNameFromConstant;

        [DTWAINNativeFunction("DTWAIN_GetTwainNameFromConstantA")]
        private readonly DTWAIN_GetTwainNameFromConstantADelegate  _DTWAIN_GetTwainNameFromConstantA;

        [DTWAINNativeFunction("DTWAIN_GetTwainNameFromConstantW")]
        private readonly DTWAIN_GetTwainNameFromConstantWDelegate  _DTWAIN_GetTwainNameFromConstantW;

        [DTWAINNativeFunction("DTWAIN_GetTwainStringName")]
        private readonly DTWAIN_GetTwainStringNameDelegate  _DTWAIN_GetTwainStringName;

        [DTWAINNativeFunction("DTWAIN_GetTwainStringNameA")]
        private readonly DTWAIN_GetTwainStringNameADelegate  _DTWAIN_GetTwainStringNameA;

        [DTWAINNativeFunction("DTWAIN_GetTwainStringNameW")]
        private readonly DTWAIN_GetTwainStringNameWDelegate  _DTWAIN_GetTwainStringNameW;

        [DTWAINNativeFunction("DTWAIN_GetTwainTimeout")]
        private readonly DTWAIN_GetTwainTimeoutDelegate  _DTWAIN_GetTwainTimeout;

        [DTWAINNativeFunction("DTWAIN_GetVersion")]
        private readonly DTWAIN_GetVersionDelegate  _DTWAIN_GetVersion;

        [DTWAINNativeFunction("DTWAIN_GetVersionCopyright")]
        private readonly DTWAIN_GetVersionCopyrightDelegate  _DTWAIN_GetVersionCopyright;

        [DTWAINNativeFunction("DTWAIN_GetVersionCopyrightA")]
        private readonly DTWAIN_GetVersionCopyrightADelegate  _DTWAIN_GetVersionCopyrightA;

        [DTWAINNativeFunction("DTWAIN_GetVersionCopyrightW")]
        private readonly DTWAIN_GetVersionCopyrightWDelegate  _DTWAIN_GetVersionCopyrightW;

        [DTWAINNativeFunction("DTWAIN_GetVersionEx")]
        private readonly DTWAIN_GetVersionExDelegate  _DTWAIN_GetVersionEx;

        [DTWAINNativeFunction("DTWAIN_GetVersionInfo")]
        private readonly DTWAIN_GetVersionInfoDelegate  _DTWAIN_GetVersionInfo;

        [DTWAINNativeFunction("DTWAIN_GetVersionInfoA")]
        private readonly DTWAIN_GetVersionInfoADelegate  _DTWAIN_GetVersionInfoA;

        [DTWAINNativeFunction("DTWAIN_GetVersionInfoW")]
        private readonly DTWAIN_GetVersionInfoWDelegate  _DTWAIN_GetVersionInfoW;

        [DTWAINNativeFunction("DTWAIN_GetVersionString")]
        private readonly DTWAIN_GetVersionStringDelegate  _DTWAIN_GetVersionString;

        [DTWAINNativeFunction("DTWAIN_GetVersionStringA")]
        private readonly DTWAIN_GetVersionStringADelegate  _DTWAIN_GetVersionStringA;

        [DTWAINNativeFunction("DTWAIN_GetVersionStringW")]
        private readonly DTWAIN_GetVersionStringWDelegate  _DTWAIN_GetVersionStringW;

        [DTWAINNativeFunction("DTWAIN_GetWindowsVersionInfo")]
        private readonly DTWAIN_GetWindowsVersionInfoDelegate  _DTWAIN_GetWindowsVersionInfo;

        [DTWAINNativeFunction("DTWAIN_GetWindowsVersionInfoA")]
        private readonly DTWAIN_GetWindowsVersionInfoADelegate  _DTWAIN_GetWindowsVersionInfoA;

        [DTWAINNativeFunction("DTWAIN_GetWindowsVersionInfoW")]
        private readonly DTWAIN_GetWindowsVersionInfoWDelegate  _DTWAIN_GetWindowsVersionInfoW;

        [DTWAINNativeFunction("DTWAIN_GetXResolution")]
        private readonly DTWAIN_GetXResolutionDelegate  _DTWAIN_GetXResolution;

        [DTWAINNativeFunction("DTWAIN_GetXResolutionString")]
        private readonly DTWAIN_GetXResolutionStringDelegate  _DTWAIN_GetXResolutionString;

        [DTWAINNativeFunction("DTWAIN_GetXResolutionStringA")]
        private readonly DTWAIN_GetXResolutionStringADelegate  _DTWAIN_GetXResolutionStringA;

        [DTWAINNativeFunction("DTWAIN_GetXResolutionStringW")]
        private readonly DTWAIN_GetXResolutionStringWDelegate  _DTWAIN_GetXResolutionStringW;

        [DTWAINNativeFunction("DTWAIN_GetYResolution")]
        private readonly DTWAIN_GetYResolutionDelegate  _DTWAIN_GetYResolution;

        [DTWAINNativeFunction("DTWAIN_GetYResolutionString")]
        private readonly DTWAIN_GetYResolutionStringDelegate  _DTWAIN_GetYResolutionString;

        [DTWAINNativeFunction("DTWAIN_GetYResolutionStringA")]
        private readonly DTWAIN_GetYResolutionStringADelegate  _DTWAIN_GetYResolutionStringA;

        [DTWAINNativeFunction("DTWAIN_GetYResolutionStringW")]
        private readonly DTWAIN_GetYResolutionStringWDelegate  _DTWAIN_GetYResolutionStringW;

        [DTWAINNativeFunction("DTWAIN_InitExtImageInfo")]
        private readonly DTWAIN_InitExtImageInfoDelegate  _DTWAIN_InitExtImageInfo;

        [DTWAINNativeFunction("DTWAIN_InitImageFileAppend")]
        private readonly DTWAIN_InitImageFileAppendDelegate  _DTWAIN_InitImageFileAppend;

        [DTWAINNativeFunction("DTWAIN_InitImageFileAppendA")]
        private readonly DTWAIN_InitImageFileAppendADelegate  _DTWAIN_InitImageFileAppendA;

        [DTWAINNativeFunction("DTWAIN_InitImageFileAppendW")]
        private readonly DTWAIN_InitImageFileAppendWDelegate  _DTWAIN_InitImageFileAppendW;

        [DTWAINNativeFunction("DTWAIN_InitOCRInterface")]
        private readonly DTWAIN_InitOCRInterfaceDelegate  _DTWAIN_InitOCRInterface;

        [DTWAINNativeFunction("DTWAIN_IsAcquiring")]
        private readonly DTWAIN_IsAcquiringDelegate  _DTWAIN_IsAcquiring;

        [DTWAINNativeFunction("DTWAIN_IsAudioXferSupported")]
        private readonly DTWAIN_IsAudioXferSupportedDelegate  _DTWAIN_IsAudioXferSupported;

        [DTWAINNativeFunction("DTWAIN_IsAutoBorderDetectEnabled")]
        private readonly DTWAIN_IsAutoBorderDetectEnabledDelegate  _DTWAIN_IsAutoBorderDetectEnabled;

        [DTWAINNativeFunction("DTWAIN_IsAutoBorderDetectSupported")]
        private readonly DTWAIN_IsAutoBorderDetectSupportedDelegate  _DTWAIN_IsAutoBorderDetectSupported;

        [DTWAINNativeFunction("DTWAIN_IsAutoBrightEnabled")]
        private readonly DTWAIN_IsAutoBrightEnabledDelegate  _DTWAIN_IsAutoBrightEnabled;

        [DTWAINNativeFunction("DTWAIN_IsAutoBrightSupported")]
        private readonly DTWAIN_IsAutoBrightSupportedDelegate  _DTWAIN_IsAutoBrightSupported;

        [DTWAINNativeFunction("DTWAIN_IsAutoDeskewEnabled")]
        private readonly DTWAIN_IsAutoDeskewEnabledDelegate  _DTWAIN_IsAutoDeskewEnabled;

        [DTWAINNativeFunction("DTWAIN_IsAutoDeskewSupported")]
        private readonly DTWAIN_IsAutoDeskewSupportedDelegate  _DTWAIN_IsAutoDeskewSupported;

        [DTWAINNativeFunction("DTWAIN_IsAutoFeedEnabled")]
        private readonly DTWAIN_IsAutoFeedEnabledDelegate  _DTWAIN_IsAutoFeedEnabled;

        [DTWAINNativeFunction("DTWAIN_IsAutoFeedSupported")]
        private readonly DTWAIN_IsAutoFeedSupportedDelegate  _DTWAIN_IsAutoFeedSupported;

        [DTWAINNativeFunction("DTWAIN_IsAutoRotateEnabled")]
        private readonly DTWAIN_IsAutoRotateEnabledDelegate  _DTWAIN_IsAutoRotateEnabled;

        [DTWAINNativeFunction("DTWAIN_IsAutoRotateSupported")]
        private readonly DTWAIN_IsAutoRotateSupportedDelegate  _DTWAIN_IsAutoRotateSupported;

        [DTWAINNativeFunction("DTWAIN_IsAutoScanEnabled")]
        private readonly DTWAIN_IsAutoScanEnabledDelegate  _DTWAIN_IsAutoScanEnabled;

        [DTWAINNativeFunction("DTWAIN_IsAutomaticSenseMediumEnabled")]
        private readonly DTWAIN_IsAutomaticSenseMediumEnabledDelegate  _DTWAIN_IsAutomaticSenseMediumEnabled;

        [DTWAINNativeFunction("DTWAIN_IsAutomaticSenseMediumSupported")]
        private readonly DTWAIN_IsAutomaticSenseMediumSupportedDelegate  _DTWAIN_IsAutomaticSenseMediumSupported;

        [DTWAINNativeFunction("DTWAIN_IsBlankPageDetectionOn")]
        private readonly DTWAIN_IsBlankPageDetectionOnDelegate  _DTWAIN_IsBlankPageDetectionOn;

        [DTWAINNativeFunction("DTWAIN_IsBufferedTileModeOn")]
        private readonly DTWAIN_IsBufferedTileModeOnDelegate  _DTWAIN_IsBufferedTileModeOn;

        [DTWAINNativeFunction("DTWAIN_IsBufferedTileModeSupported")]
        private readonly DTWAIN_IsBufferedTileModeSupportedDelegate  _DTWAIN_IsBufferedTileModeSupported;

        [DTWAINNativeFunction("DTWAIN_IsCapSupported")]
        private readonly DTWAIN_IsCapSupportedDelegate  _DTWAIN_IsCapSupported;

        [DTWAINNativeFunction("DTWAIN_IsCompressionSupported")]
        private readonly DTWAIN_IsCompressionSupportedDelegate  _DTWAIN_IsCompressionSupported;

        [DTWAINNativeFunction("DTWAIN_IsCustomDSDataSupported")]
        private readonly DTWAIN_IsCustomDSDataSupportedDelegate  _DTWAIN_IsCustomDSDataSupported;

        [DTWAINNativeFunction("DTWAIN_IsDIBBlank")]
        private readonly DTWAIN_IsDIBBlankDelegate  _DTWAIN_IsDIBBlank;

        [DTWAINNativeFunction("DTWAIN_IsDIBBlankString")]
        private readonly DTWAIN_IsDIBBlankStringDelegate  _DTWAIN_IsDIBBlankString;

        [DTWAINNativeFunction("DTWAIN_IsDIBBlankStringA")]
        private readonly DTWAIN_IsDIBBlankStringADelegate  _DTWAIN_IsDIBBlankStringA;

        [DTWAINNativeFunction("DTWAIN_IsDIBBlankStringW")]
        private readonly DTWAIN_IsDIBBlankStringWDelegate  _DTWAIN_IsDIBBlankStringW;

        [DTWAINNativeFunction("DTWAIN_IsDeviceEventSupported")]
        private readonly DTWAIN_IsDeviceEventSupportedDelegate  _DTWAIN_IsDeviceEventSupported;

        [DTWAINNativeFunction("DTWAIN_IsDeviceOnLine")]
        private readonly DTWAIN_IsDeviceOnLineDelegate  _DTWAIN_IsDeviceOnLine;

        [DTWAINNativeFunction("DTWAIN_IsDoubleFeedDetectLengthSupported")]
        private readonly DTWAIN_IsDoubleFeedDetectLengthSupportedDelegate  _DTWAIN_IsDoubleFeedDetectLengthSupported;

        [DTWAINNativeFunction("DTWAIN_IsDoubleFeedDetectSupported")]
        private readonly DTWAIN_IsDoubleFeedDetectSupportedDelegate  _DTWAIN_IsDoubleFeedDetectSupported;

        [DTWAINNativeFunction("DTWAIN_IsDoublePageCountOnDuplex")]
        private readonly DTWAIN_IsDoublePageCountOnDuplexDelegate  _DTWAIN_IsDoublePageCountOnDuplex;

        [DTWAINNativeFunction("DTWAIN_IsDuplexEnabled")]
        private readonly DTWAIN_IsDuplexEnabledDelegate  _DTWAIN_IsDuplexEnabled;

        [DTWAINNativeFunction("DTWAIN_IsDuplexSupported")]
        private readonly DTWAIN_IsDuplexSupportedDelegate  _DTWAIN_IsDuplexSupported;

        [DTWAINNativeFunction("DTWAIN_IsExtImageInfoSupported")]
        private readonly DTWAIN_IsExtImageInfoSupportedDelegate  _DTWAIN_IsExtImageInfoSupported;

        [DTWAINNativeFunction("DTWAIN_IsFeederEnabled")]
        private readonly DTWAIN_IsFeederEnabledDelegate  _DTWAIN_IsFeederEnabled;

        [DTWAINNativeFunction("DTWAIN_IsFeederLoaded")]
        private readonly DTWAIN_IsFeederLoadedDelegate  _DTWAIN_IsFeederLoaded;

        [DTWAINNativeFunction("DTWAIN_IsFeederSensitive")]
        private readonly DTWAIN_IsFeederSensitiveDelegate  _DTWAIN_IsFeederSensitive;

        [DTWAINNativeFunction("DTWAIN_IsFeederSupported")]
        private readonly DTWAIN_IsFeederSupportedDelegate  _DTWAIN_IsFeederSupported;

        [DTWAINNativeFunction("DTWAIN_IsFileSystemSupported")]
        private readonly DTWAIN_IsFileSystemSupportedDelegate  _DTWAIN_IsFileSystemSupported;

        [DTWAINNativeFunction("DTWAIN_IsFileXferSupported")]
        private readonly DTWAIN_IsFileXferSupportedDelegate  _DTWAIN_IsFileXferSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldALastPageSupported")]
        private readonly DTWAIN_IsIAFieldALastPageSupportedDelegate  _DTWAIN_IsIAFieldALastPageSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldALevelSupported")]
        private readonly DTWAIN_IsIAFieldALevelSupportedDelegate  _DTWAIN_IsIAFieldALevelSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldAPrintFormatSupported")]
        private readonly DTWAIN_IsIAFieldAPrintFormatSupportedDelegate  _DTWAIN_IsIAFieldAPrintFormatSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldAValueSupported")]
        private readonly DTWAIN_IsIAFieldAValueSupportedDelegate  _DTWAIN_IsIAFieldAValueSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldBLastPageSupported")]
        private readonly DTWAIN_IsIAFieldBLastPageSupportedDelegate  _DTWAIN_IsIAFieldBLastPageSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldBLevelSupported")]
        private readonly DTWAIN_IsIAFieldBLevelSupportedDelegate  _DTWAIN_IsIAFieldBLevelSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldBPrintFormatSupported")]
        private readonly DTWAIN_IsIAFieldBPrintFormatSupportedDelegate  _DTWAIN_IsIAFieldBPrintFormatSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldBValueSupported")]
        private readonly DTWAIN_IsIAFieldBValueSupportedDelegate  _DTWAIN_IsIAFieldBValueSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldCLastPageSupported")]
        private readonly DTWAIN_IsIAFieldCLastPageSupportedDelegate  _DTWAIN_IsIAFieldCLastPageSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldCLevelSupported")]
        private readonly DTWAIN_IsIAFieldCLevelSupportedDelegate  _DTWAIN_IsIAFieldCLevelSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldCPrintFormatSupported")]
        private readonly DTWAIN_IsIAFieldCPrintFormatSupportedDelegate  _DTWAIN_IsIAFieldCPrintFormatSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldCValueSupported")]
        private readonly DTWAIN_IsIAFieldCValueSupportedDelegate  _DTWAIN_IsIAFieldCValueSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldDLastPageSupported")]
        private readonly DTWAIN_IsIAFieldDLastPageSupportedDelegate  _DTWAIN_IsIAFieldDLastPageSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldDLevelSupported")]
        private readonly DTWAIN_IsIAFieldDLevelSupportedDelegate  _DTWAIN_IsIAFieldDLevelSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldDPrintFormatSupported")]
        private readonly DTWAIN_IsIAFieldDPrintFormatSupportedDelegate  _DTWAIN_IsIAFieldDPrintFormatSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldDValueSupported")]
        private readonly DTWAIN_IsIAFieldDValueSupportedDelegate  _DTWAIN_IsIAFieldDValueSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldELastPageSupported")]
        private readonly DTWAIN_IsIAFieldELastPageSupportedDelegate  _DTWAIN_IsIAFieldELastPageSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldELevelSupported")]
        private readonly DTWAIN_IsIAFieldELevelSupportedDelegate  _DTWAIN_IsIAFieldELevelSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldEPrintFormatSupported")]
        private readonly DTWAIN_IsIAFieldEPrintFormatSupportedDelegate  _DTWAIN_IsIAFieldEPrintFormatSupported;

        [DTWAINNativeFunction("DTWAIN_IsIAFieldEValueSupported")]
        private readonly DTWAIN_IsIAFieldEValueSupportedDelegate  _DTWAIN_IsIAFieldEValueSupported;

        [DTWAINNativeFunction("DTWAIN_IsImageAddressingSupported")]
        private readonly DTWAIN_IsImageAddressingSupportedDelegate  _DTWAIN_IsImageAddressingSupported;

        [DTWAINNativeFunction("DTWAIN_IsIndicatorEnabled")]
        private readonly DTWAIN_IsIndicatorEnabledDelegate  _DTWAIN_IsIndicatorEnabled;

        [DTWAINNativeFunction("DTWAIN_IsIndicatorSupported")]
        private readonly DTWAIN_IsIndicatorSupportedDelegate  _DTWAIN_IsIndicatorSupported;

        [DTWAINNativeFunction("DTWAIN_IsInitialized")]
        private readonly DTWAIN_IsInitializedDelegate  _DTWAIN_IsInitialized;

        [DTWAINNativeFunction("DTWAIN_IsJobControlSupported")]
        private readonly DTWAIN_IsJobControlSupportedDelegate  _DTWAIN_IsJobControlSupported;

        [DTWAINNativeFunction("DTWAIN_IsLampEnabled")]
        private readonly DTWAIN_IsLampEnabledDelegate  _DTWAIN_IsLampEnabled;

        [DTWAINNativeFunction("DTWAIN_IsLampSupported")]
        private readonly DTWAIN_IsLampSupportedDelegate  _DTWAIN_IsLampSupported;

        [DTWAINNativeFunction("DTWAIN_IsLightPathSupported")]
        private readonly DTWAIN_IsLightPathSupportedDelegate  _DTWAIN_IsLightPathSupported;

        [DTWAINNativeFunction("DTWAIN_IsLightSourceSupported")]
        private readonly DTWAIN_IsLightSourceSupportedDelegate  _DTWAIN_IsLightSourceSupported;

        [DTWAINNativeFunction("DTWAIN_IsMaxBuffersSupported")]
        private readonly DTWAIN_IsMaxBuffersSupportedDelegate  _DTWAIN_IsMaxBuffersSupported;

        [DTWAINNativeFunction("DTWAIN_IsMemFileXferSupported")]
        private readonly DTWAIN_IsMemFileXferSupportedDelegate  _DTWAIN_IsMemFileXferSupported;

        [DTWAINNativeFunction("DTWAIN_IsMsgNotifyEnabled")]
        private readonly DTWAIN_IsMsgNotifyEnabledDelegate  _DTWAIN_IsMsgNotifyEnabled;

        [DTWAINNativeFunction("DTWAIN_IsNotifyTripletsEnabled")]
        private readonly DTWAIN_IsNotifyTripletsEnabledDelegate  _DTWAIN_IsNotifyTripletsEnabled;

        [DTWAINNativeFunction("DTWAIN_IsOCREngineActivated")]
        private readonly DTWAIN_IsOCREngineActivatedDelegate  _DTWAIN_IsOCREngineActivated;

        [DTWAINNativeFunction("DTWAIN_IsOpenSourcesOnSelect")]
        private readonly DTWAIN_IsOpenSourcesOnSelectDelegate  _DTWAIN_IsOpenSourcesOnSelect;

        [DTWAINNativeFunction("DTWAIN_IsOrientationSupported")]
        private readonly DTWAIN_IsOrientationSupportedDelegate  _DTWAIN_IsOrientationSupported;

        [DTWAINNativeFunction("DTWAIN_IsOverscanSupported")]
        private readonly DTWAIN_IsOverscanSupportedDelegate  _DTWAIN_IsOverscanSupported;

        [DTWAINNativeFunction("DTWAIN_IsPaperDetectable")]
        private readonly DTWAIN_IsPaperDetectableDelegate  _DTWAIN_IsPaperDetectable;

        [DTWAINNativeFunction("DTWAIN_IsPaperSizeSupported")]
        private readonly DTWAIN_IsPaperSizeSupportedDelegate  _DTWAIN_IsPaperSizeSupported;

        [DTWAINNativeFunction("DTWAIN_IsPatchCapsSupported")]
        private readonly DTWAIN_IsPatchCapsSupportedDelegate  _DTWAIN_IsPatchCapsSupported;

        [DTWAINNativeFunction("DTWAIN_IsPatchDetectEnabled")]
        private readonly DTWAIN_IsPatchDetectEnabledDelegate  _DTWAIN_IsPatchDetectEnabled;

        [DTWAINNativeFunction("DTWAIN_IsPatchSupported")]
        private readonly DTWAIN_IsPatchSupportedDelegate  _DTWAIN_IsPatchSupported;

        [DTWAINNativeFunction("DTWAIN_IsPeekMessageLoopEnabled")]
        private readonly DTWAIN_IsPeekMessageLoopEnabledDelegate  _DTWAIN_IsPeekMessageLoopEnabled;

        [DTWAINNativeFunction("DTWAIN_IsPixelTypeSupported")]
        private readonly DTWAIN_IsPixelTypeSupportedDelegate  _DTWAIN_IsPixelTypeSupported;

        [DTWAINNativeFunction("DTWAIN_IsPrinterEnabled")]
        private readonly DTWAIN_IsPrinterEnabledDelegate  _DTWAIN_IsPrinterEnabled;

        [DTWAINNativeFunction("DTWAIN_IsPrinterSupported")]
        private readonly DTWAIN_IsPrinterSupportedDelegate  _DTWAIN_IsPrinterSupported;

        [DTWAINNativeFunction("DTWAIN_IsRotationSupported")]
        private readonly DTWAIN_IsRotationSupportedDelegate  _DTWAIN_IsRotationSupported;

        [DTWAINNativeFunction("DTWAIN_IsSessionEnabled")]
        private readonly DTWAIN_IsSessionEnabledDelegate  _DTWAIN_IsSessionEnabled;

        [DTWAINNativeFunction("DTWAIN_IsSkipImageInfoError")]
        private readonly DTWAIN_IsSkipImageInfoErrorDelegate  _DTWAIN_IsSkipImageInfoError;

        [DTWAINNativeFunction("DTWAIN_IsSourceAcquiring")]
        private readonly DTWAIN_IsSourceAcquiringDelegate  _DTWAIN_IsSourceAcquiring;

        [DTWAINNativeFunction("DTWAIN_IsSourceAcquiringEx")]
        private readonly DTWAIN_IsSourceAcquiringExDelegate  _DTWAIN_IsSourceAcquiringEx;

        [DTWAINNativeFunction("DTWAIN_IsSourceInUIOnlyMode")]
        private readonly DTWAIN_IsSourceInUIOnlyModeDelegate  _DTWAIN_IsSourceInUIOnlyMode;

        [DTWAINNativeFunction("DTWAIN_IsSourceOpen")]
        private readonly DTWAIN_IsSourceOpenDelegate  _DTWAIN_IsSourceOpen;

        [DTWAINNativeFunction("DTWAIN_IsSourceSelected")]
        private readonly DTWAIN_IsSourceSelectedDelegate  _DTWAIN_IsSourceSelected;

        [DTWAINNativeFunction("DTWAIN_IsSourceValid")]
        private readonly DTWAIN_IsSourceValidDelegate  _DTWAIN_IsSourceValid;

        [DTWAINNativeFunction("DTWAIN_IsThumbnailEnabled")]
        private readonly DTWAIN_IsThumbnailEnabledDelegate  _DTWAIN_IsThumbnailEnabled;

        [DTWAINNativeFunction("DTWAIN_IsThumbnailSupported")]
        private readonly DTWAIN_IsThumbnailSupportedDelegate  _DTWAIN_IsThumbnailSupported;

        [DTWAINNativeFunction("DTWAIN_IsTwainAvailable")]
        private readonly DTWAIN_IsTwainAvailableDelegate  _DTWAIN_IsTwainAvailable;

        [DTWAINNativeFunction("DTWAIN_IsTwainAvailableEx")]
        private readonly DTWAIN_IsTwainAvailableExDelegate  _DTWAIN_IsTwainAvailableEx;

        [DTWAINNativeFunction("DTWAIN_IsTwainAvailableExA")]
        private readonly DTWAIN_IsTwainAvailableExADelegate  _DTWAIN_IsTwainAvailableExA;

        [DTWAINNativeFunction("DTWAIN_IsTwainAvailableExW")]
        private readonly DTWAIN_IsTwainAvailableExWDelegate  _DTWAIN_IsTwainAvailableExW;

        [DTWAINNativeFunction("DTWAIN_IsTwainMsg")]
        private readonly DTWAIN_IsTwainMsgDelegate  _DTWAIN_IsTwainMsg;

        [DTWAINNativeFunction("DTWAIN_IsUIControllable")]
        private readonly DTWAIN_IsUIControllableDelegate  _DTWAIN_IsUIControllable;

        [DTWAINNativeFunction("DTWAIN_IsUIEnabled")]
        private readonly DTWAIN_IsUIEnabledDelegate  _DTWAIN_IsUIEnabled;

        [DTWAINNativeFunction("DTWAIN_IsUIOnlySupported")]
        private readonly DTWAIN_IsUIOnlySupportedDelegate  _DTWAIN_IsUIOnlySupported;

        [DTWAINNativeFunction("DTWAIN_LoadCustomStringResources")]
        private readonly DTWAIN_LoadCustomStringResourcesDelegate  _DTWAIN_LoadCustomStringResources;

        [DTWAINNativeFunction("DTWAIN_LoadCustomStringResourcesA")]
        private readonly DTWAIN_LoadCustomStringResourcesADelegate  _DTWAIN_LoadCustomStringResourcesA;

        [DTWAINNativeFunction("DTWAIN_LoadCustomStringResourcesEx")]
        private readonly DTWAIN_LoadCustomStringResourcesExDelegate  _DTWAIN_LoadCustomStringResourcesEx;

        [DTWAINNativeFunction("DTWAIN_LoadCustomStringResourcesExA")]
        private readonly DTWAIN_LoadCustomStringResourcesExADelegate  _DTWAIN_LoadCustomStringResourcesExA;

        [DTWAINNativeFunction("DTWAIN_LoadCustomStringResourcesExW")]
        private readonly DTWAIN_LoadCustomStringResourcesExWDelegate  _DTWAIN_LoadCustomStringResourcesExW;

        [DTWAINNativeFunction("DTWAIN_LoadCustomStringResourcesW")]
        private readonly DTWAIN_LoadCustomStringResourcesWDelegate  _DTWAIN_LoadCustomStringResourcesW;

        [DTWAINNativeFunction("DTWAIN_LoadLanguageResource")]
        private readonly DTWAIN_LoadLanguageResourceDelegate  _DTWAIN_LoadLanguageResource;

        [DTWAINNativeFunction("DTWAIN_LockMemory")]
        private readonly DTWAIN_LockMemoryDelegate  _DTWAIN_LockMemory;

        [DTWAINNativeFunction("DTWAIN_LockMemoryEx")]
        private readonly DTWAIN_LockMemoryExDelegate  _DTWAIN_LockMemoryEx;

        [DTWAINNativeFunction("DTWAIN_LogMessage")]
        private readonly DTWAIN_LogMessageDelegate  _DTWAIN_LogMessage;

        [DTWAINNativeFunction("DTWAIN_LogMessageA")]
        private readonly DTWAIN_LogMessageADelegate  _DTWAIN_LogMessageA;

        [DTWAINNativeFunction("DTWAIN_LogMessageW")]
        private readonly DTWAIN_LogMessageWDelegate  _DTWAIN_LogMessageW;

        [DTWAINNativeFunction("DTWAIN_MakeRGB")]
        private readonly DTWAIN_MakeRGBDelegate  _DTWAIN_MakeRGB;

        [DTWAINNativeFunction("DTWAIN_OpenSource")]
        private readonly DTWAIN_OpenSourceDelegate  _DTWAIN_OpenSource;

        [DTWAINNativeFunction("DTWAIN_OpenSourcesOnSelect")]
        private readonly DTWAIN_OpenSourcesOnSelectDelegate  _DTWAIN_OpenSourcesOnSelect;

        [DTWAINNativeFunction("DTWAIN_RangeCreate")]
        private readonly DTWAIN_RangeCreateDelegate  _DTWAIN_RangeCreate;

        [DTWAINNativeFunction("DTWAIN_RangeCreateFromCap")]
        private readonly DTWAIN_RangeCreateFromCapDelegate  _DTWAIN_RangeCreateFromCap;

        [DTWAINNativeFunction("DTWAIN_RangeDestroy")]
        private readonly DTWAIN_RangeDestroyDelegate  _DTWAIN_RangeDestroy;

        [DTWAINNativeFunction("DTWAIN_RangeExpand")]
        private readonly DTWAIN_RangeExpandDelegate  _DTWAIN_RangeExpand;

        [DTWAINNativeFunction("DTWAIN_RangeExpandEx")]
        private readonly DTWAIN_RangeExpandExDelegate  _DTWAIN_RangeExpandEx;

        [DTWAINNativeFunction("DTWAIN_RangeGetAll")]
        private readonly DTWAIN_RangeGetAllDelegate  _DTWAIN_RangeGetAll;

        [DTWAINNativeFunction("DTWAIN_RangeGetAllFloat")]
        private readonly DTWAIN_RangeGetAllFloatDelegate  _DTWAIN_RangeGetAllFloat;

        [DTWAINNativeFunction("DTWAIN_RangeGetAllFloatString")]
        private readonly DTWAIN_RangeGetAllFloatStringDelegate  _DTWAIN_RangeGetAllFloatString;

        [DTWAINNativeFunction("DTWAIN_RangeGetAllFloatStringA")]
        private readonly DTWAIN_RangeGetAllFloatStringADelegate  _DTWAIN_RangeGetAllFloatStringA;

        [DTWAINNativeFunction("DTWAIN_RangeGetAllFloatStringW")]
        private readonly DTWAIN_RangeGetAllFloatStringWDelegate  _DTWAIN_RangeGetAllFloatStringW;

        [DTWAINNativeFunction("DTWAIN_RangeGetAllLong")]
        private readonly DTWAIN_RangeGetAllLongDelegate  _DTWAIN_RangeGetAllLong;

        [DTWAINNativeFunction("DTWAIN_RangeGetCount")]
        private readonly DTWAIN_RangeGetCountDelegate  _DTWAIN_RangeGetCount;

        [DTWAINNativeFunction("DTWAIN_RangeGetExpValue")]
        private readonly DTWAIN_RangeGetExpValueDelegate  _DTWAIN_RangeGetExpValue;

        [DTWAINNativeFunction("DTWAIN_RangeGetExpValueFloat")]
        private readonly DTWAIN_RangeGetExpValueFloatDelegate  _DTWAIN_RangeGetExpValueFloat;

        [DTWAINNativeFunction("DTWAIN_RangeGetExpValueFloatString")]
        private readonly DTWAIN_RangeGetExpValueFloatStringDelegate  _DTWAIN_RangeGetExpValueFloatString;

        [DTWAINNativeFunction("DTWAIN_RangeGetExpValueFloatStringA")]
        private readonly DTWAIN_RangeGetExpValueFloatStringADelegate  _DTWAIN_RangeGetExpValueFloatStringA;

        [DTWAINNativeFunction("DTWAIN_RangeGetExpValueFloatStringW")]
        private readonly DTWAIN_RangeGetExpValueFloatStringWDelegate  _DTWAIN_RangeGetExpValueFloatStringW;

        [DTWAINNativeFunction("DTWAIN_RangeGetExpValueLong")]
        private readonly DTWAIN_RangeGetExpValueLongDelegate  _DTWAIN_RangeGetExpValueLong;

        [DTWAINNativeFunction("DTWAIN_RangeGetNearestValue")]
        private readonly DTWAIN_RangeGetNearestValueDelegate  _DTWAIN_RangeGetNearestValue;

        [DTWAINNativeFunction("DTWAIN_RangeGetNearestValueFloat")]
        private readonly DTWAIN_RangeGetNearestValueFloatDelegate  _DTWAIN_RangeGetNearestValueFloat;

        [DTWAINNativeFunction("DTWAIN_RangeGetNearestValueFloatString")]
        private readonly DTWAIN_RangeGetNearestValueFloatStringDelegate  _DTWAIN_RangeGetNearestValueFloatString;

        [DTWAINNativeFunction("DTWAIN_RangeGetNearestValueFloatStringA")]
        private readonly DTWAIN_RangeGetNearestValueFloatStringADelegate  _DTWAIN_RangeGetNearestValueFloatStringA;

        [DTWAINNativeFunction("DTWAIN_RangeGetNearestValueFloatStringW")]
        private readonly DTWAIN_RangeGetNearestValueFloatStringWDelegate  _DTWAIN_RangeGetNearestValueFloatStringW;

        [DTWAINNativeFunction("DTWAIN_RangeGetNearestValueLong")]
        private readonly DTWAIN_RangeGetNearestValueLongDelegate  _DTWAIN_RangeGetNearestValueLong;

        [DTWAINNativeFunction("DTWAIN_RangeGetPos")]
        private readonly DTWAIN_RangeGetPosDelegate  _DTWAIN_RangeGetPos;

        [DTWAINNativeFunction("DTWAIN_RangeGetPosFloat")]
        private readonly DTWAIN_RangeGetPosFloatDelegate  _DTWAIN_RangeGetPosFloat;

        [DTWAINNativeFunction("DTWAIN_RangeGetPosFloatString")]
        private readonly DTWAIN_RangeGetPosFloatStringDelegate  _DTWAIN_RangeGetPosFloatString;

        [DTWAINNativeFunction("DTWAIN_RangeGetPosFloatStringA")]
        private readonly DTWAIN_RangeGetPosFloatStringADelegate  _DTWAIN_RangeGetPosFloatStringA;

        [DTWAINNativeFunction("DTWAIN_RangeGetPosFloatStringW")]
        private readonly DTWAIN_RangeGetPosFloatStringWDelegate  _DTWAIN_RangeGetPosFloatStringW;

        [DTWAINNativeFunction("DTWAIN_RangeGetPosLong")]
        private readonly DTWAIN_RangeGetPosLongDelegate  _DTWAIN_RangeGetPosLong;

        [DTWAINNativeFunction("DTWAIN_RangeGetValue")]
        private readonly DTWAIN_RangeGetValueDelegate  _DTWAIN_RangeGetValue;

        [DTWAINNativeFunction("DTWAIN_RangeGetValueFloat")]
        private readonly DTWAIN_RangeGetValueFloatDelegate  _DTWAIN_RangeGetValueFloat;

        [DTWAINNativeFunction("DTWAIN_RangeGetValueFloatString")]
        private readonly DTWAIN_RangeGetValueFloatStringDelegate  _DTWAIN_RangeGetValueFloatString;

        [DTWAINNativeFunction("DTWAIN_RangeGetValueFloatStringA")]
        private readonly DTWAIN_RangeGetValueFloatStringADelegate  _DTWAIN_RangeGetValueFloatStringA;

        [DTWAINNativeFunction("DTWAIN_RangeGetValueFloatStringW")]
        private readonly DTWAIN_RangeGetValueFloatStringWDelegate  _DTWAIN_RangeGetValueFloatStringW;

        [DTWAINNativeFunction("DTWAIN_RangeGetValueLong")]
        private readonly DTWAIN_RangeGetValueLongDelegate  _DTWAIN_RangeGetValueLong;

        [DTWAINNativeFunction("DTWAIN_RangeIsValid")]
        private readonly DTWAIN_RangeIsValidDelegate  _DTWAIN_RangeIsValid;

        [DTWAINNativeFunction("DTWAIN_RangeSetAll")]
        private readonly DTWAIN_RangeSetAllDelegate  _DTWAIN_RangeSetAll;

        [DTWAINNativeFunction("DTWAIN_RangeSetAllFloat")]
        private readonly DTWAIN_RangeSetAllFloatDelegate  _DTWAIN_RangeSetAllFloat;

        [DTWAINNativeFunction("DTWAIN_RangeSetAllFloatString")]
        private readonly DTWAIN_RangeSetAllFloatStringDelegate  _DTWAIN_RangeSetAllFloatString;

        [DTWAINNativeFunction("DTWAIN_RangeSetAllFloatStringA")]
        private readonly DTWAIN_RangeSetAllFloatStringADelegate  _DTWAIN_RangeSetAllFloatStringA;

        [DTWAINNativeFunction("DTWAIN_RangeSetAllFloatStringW")]
        private readonly DTWAIN_RangeSetAllFloatStringWDelegate  _DTWAIN_RangeSetAllFloatStringW;

        [DTWAINNativeFunction("DTWAIN_RangeSetAllLong")]
        private readonly DTWAIN_RangeSetAllLongDelegate  _DTWAIN_RangeSetAllLong;

        [DTWAINNativeFunction("DTWAIN_RangeSetValue")]
        private readonly DTWAIN_RangeSetValueDelegate  _DTWAIN_RangeSetValue;

        [DTWAINNativeFunction("DTWAIN_RangeSetValueFloat")]
        private readonly DTWAIN_RangeSetValueFloatDelegate  _DTWAIN_RangeSetValueFloat;

        [DTWAINNativeFunction("DTWAIN_RangeSetValueFloatString")]
        private readonly DTWAIN_RangeSetValueFloatStringDelegate  _DTWAIN_RangeSetValueFloatString;

        [DTWAINNativeFunction("DTWAIN_RangeSetValueFloatStringA")]
        private readonly DTWAIN_RangeSetValueFloatStringADelegate  _DTWAIN_RangeSetValueFloatStringA;

        [DTWAINNativeFunction("DTWAIN_RangeSetValueFloatStringW")]
        private readonly DTWAIN_RangeSetValueFloatStringWDelegate  _DTWAIN_RangeSetValueFloatStringW;

        [DTWAINNativeFunction("DTWAIN_RangeSetValueLong")]
        private readonly DTWAIN_RangeSetValueLongDelegate  _DTWAIN_RangeSetValueLong;

        [DTWAINNativeFunction("DTWAIN_RemovePDFTextElement")]
        private readonly DTWAIN_RemovePDFTextElementDelegate  _DTWAIN_RemovePDFTextElement;

        [DTWAINNativeFunction("DTWAIN_ResetPDFTextElement")]
        private readonly DTWAIN_ResetPDFTextElementDelegate  _DTWAIN_ResetPDFTextElement;

        [DTWAINNativeFunction("DTWAIN_RewindPage")]
        private readonly DTWAIN_RewindPageDelegate  _DTWAIN_RewindPage;

        [DTWAINNativeFunction("DTWAIN_SelectDefaultOCREngine")]
        private readonly DTWAIN_SelectDefaultOCREngineDelegate  _DTWAIN_SelectDefaultOCREngine;

        [DTWAINNativeFunction("DTWAIN_SelectDefaultSource")]
        private readonly DTWAIN_SelectDefaultSourceDelegate  _DTWAIN_SelectDefaultSource;

        [DTWAINNativeFunction("DTWAIN_SelectDefaultSourceWithOpen")]
        private readonly DTWAIN_SelectDefaultSourceWithOpenDelegate  _DTWAIN_SelectDefaultSourceWithOpen;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngine")]
        private readonly DTWAIN_SelectOCREngineDelegate  _DTWAIN_SelectOCREngine;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngine2")]
        private readonly DTWAIN_SelectOCREngine2Delegate  _DTWAIN_SelectOCREngine2;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngine2A")]
        private readonly DTWAIN_SelectOCREngine2ADelegate  _DTWAIN_SelectOCREngine2A;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngine2Ex")]
        private readonly DTWAIN_SelectOCREngine2ExDelegate  _DTWAIN_SelectOCREngine2Ex;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngine2ExA")]
        private readonly DTWAIN_SelectOCREngine2ExADelegate  _DTWAIN_SelectOCREngine2ExA;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngine2ExW")]
        private readonly DTWAIN_SelectOCREngine2ExWDelegate  _DTWAIN_SelectOCREngine2ExW;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngine2W")]
        private readonly DTWAIN_SelectOCREngine2WDelegate  _DTWAIN_SelectOCREngine2W;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngineByName")]
        private readonly DTWAIN_SelectOCREngineByNameDelegate  _DTWAIN_SelectOCREngineByName;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngineByNameA")]
        private readonly DTWAIN_SelectOCREngineByNameADelegate  _DTWAIN_SelectOCREngineByNameA;

        [DTWAINNativeFunction("DTWAIN_SelectOCREngineByNameW")]
        private readonly DTWAIN_SelectOCREngineByNameWDelegate  _DTWAIN_SelectOCREngineByNameW;

        [DTWAINNativeFunction("DTWAIN_SelectSource")]
        private readonly DTWAIN_SelectSourceDelegate  _DTWAIN_SelectSource;

        [DTWAINNativeFunction("DTWAIN_SelectSource2")]
        private readonly DTWAIN_SelectSource2Delegate  _DTWAIN_SelectSource2;

        [DTWAINNativeFunction("DTWAIN_SelectSource2A")]
        private readonly DTWAIN_SelectSource2ADelegate  _DTWAIN_SelectSource2A;

        [DTWAINNativeFunction("DTWAIN_SelectSource2Ex")]
        private readonly DTWAIN_SelectSource2ExDelegate  _DTWAIN_SelectSource2Ex;

        [DTWAINNativeFunction("DTWAIN_SelectSource2ExA")]
        private readonly DTWAIN_SelectSource2ExADelegate  _DTWAIN_SelectSource2ExA;

        [DTWAINNativeFunction("DTWAIN_SelectSource2ExW")]
        private readonly DTWAIN_SelectSource2ExWDelegate  _DTWAIN_SelectSource2ExW;

        [DTWAINNativeFunction("DTWAIN_SelectSource2W")]
        private readonly DTWAIN_SelectSource2WDelegate  _DTWAIN_SelectSource2W;

        [DTWAINNativeFunction("DTWAIN_SelectSourceByName")]
        private readonly DTWAIN_SelectSourceByNameDelegate  _DTWAIN_SelectSourceByName;

        [DTWAINNativeFunction("DTWAIN_SelectSourceByNameA")]
        private readonly DTWAIN_SelectSourceByNameADelegate  _DTWAIN_SelectSourceByNameA;

        [DTWAINNativeFunction("DTWAIN_SelectSourceByNameW")]
        private readonly DTWAIN_SelectSourceByNameWDelegate  _DTWAIN_SelectSourceByNameW;

        [DTWAINNativeFunction("DTWAIN_SelectSourceByNameWithOpen")]
        private readonly DTWAIN_SelectSourceByNameWithOpenDelegate  _DTWAIN_SelectSourceByNameWithOpen;

        [DTWAINNativeFunction("DTWAIN_SelectSourceByNameWithOpenA")]
        private readonly DTWAIN_SelectSourceByNameWithOpenADelegate  _DTWAIN_SelectSourceByNameWithOpenA;

        [DTWAINNativeFunction("DTWAIN_SelectSourceByNameWithOpenW")]
        private readonly DTWAIN_SelectSourceByNameWithOpenWDelegate  _DTWAIN_SelectSourceByNameWithOpenW;

        [DTWAINNativeFunction("DTWAIN_SelectSourceWithOpen")]
        private readonly DTWAIN_SelectSourceWithOpenDelegate  _DTWAIN_SelectSourceWithOpen;

        [DTWAINNativeFunction("DTWAIN_SetAcquireArea")]
        private readonly DTWAIN_SetAcquireAreaDelegate  _DTWAIN_SetAcquireArea;

        [DTWAINNativeFunction("DTWAIN_SetAcquireArea2")]
        private readonly DTWAIN_SetAcquireArea2Delegate  _DTWAIN_SetAcquireArea2;

        [DTWAINNativeFunction("DTWAIN_SetAcquireArea2String")]
        private readonly DTWAIN_SetAcquireArea2StringDelegate  _DTWAIN_SetAcquireArea2String;

        [DTWAINNativeFunction("DTWAIN_SetAcquireArea2StringA")]
        private readonly DTWAIN_SetAcquireArea2StringADelegate  _DTWAIN_SetAcquireArea2StringA;

        [DTWAINNativeFunction("DTWAIN_SetAcquireArea2StringW")]
        private readonly DTWAIN_SetAcquireArea2StringWDelegate  _DTWAIN_SetAcquireArea2StringW;

        [DTWAINNativeFunction("DTWAIN_SetAcquireImageNegative")]
        private readonly DTWAIN_SetAcquireImageNegativeDelegate  _DTWAIN_SetAcquireImageNegative;

        [DTWAINNativeFunction("DTWAIN_SetAcquireImageScale")]
        private readonly DTWAIN_SetAcquireImageScaleDelegate  _DTWAIN_SetAcquireImageScale;

        [DTWAINNativeFunction("DTWAIN_SetAcquireImageScaleString")]
        private readonly DTWAIN_SetAcquireImageScaleStringDelegate  _DTWAIN_SetAcquireImageScaleString;

        [DTWAINNativeFunction("DTWAIN_SetAcquireImageScaleStringA")]
        private readonly DTWAIN_SetAcquireImageScaleStringADelegate  _DTWAIN_SetAcquireImageScaleStringA;

        [DTWAINNativeFunction("DTWAIN_SetAcquireImageScaleStringW")]
        private readonly DTWAIN_SetAcquireImageScaleStringWDelegate  _DTWAIN_SetAcquireImageScaleStringW;

        [DTWAINNativeFunction("DTWAIN_SetAcquireStripBuffer")]
        private readonly DTWAIN_SetAcquireStripBufferDelegate  _DTWAIN_SetAcquireStripBuffer;

        [DTWAINNativeFunction("DTWAIN_SetAcquireStripSize")]
        private readonly DTWAIN_SetAcquireStripSizeDelegate  _DTWAIN_SetAcquireStripSize;

        [DTWAINNativeFunction("DTWAIN_SetAlarmVolume")]
        private readonly DTWAIN_SetAlarmVolumeDelegate  _DTWAIN_SetAlarmVolume;

        [DTWAINNativeFunction("DTWAIN_SetAlarms")]
        private readonly DTWAIN_SetAlarmsDelegate  _DTWAIN_SetAlarms;

        [DTWAINNativeFunction("DTWAIN_SetAllCapsToDefault")]
        private readonly DTWAIN_SetAllCapsToDefaultDelegate  _DTWAIN_SetAllCapsToDefault;

        [DTWAINNativeFunction("DTWAIN_SetAppInfo")]
        private readonly DTWAIN_SetAppInfoDelegate  _DTWAIN_SetAppInfo;

        [DTWAINNativeFunction("DTWAIN_SetAppInfoA")]
        private readonly DTWAIN_SetAppInfoADelegate  _DTWAIN_SetAppInfoA;

        [DTWAINNativeFunction("DTWAIN_SetAppInfoW")]
        private readonly DTWAIN_SetAppInfoWDelegate  _DTWAIN_SetAppInfoW;

        [DTWAINNativeFunction("DTWAIN_SetAuthor")]
        private readonly DTWAIN_SetAuthorDelegate  _DTWAIN_SetAuthor;

        [DTWAINNativeFunction("DTWAIN_SetAuthorA")]
        private readonly DTWAIN_SetAuthorADelegate  _DTWAIN_SetAuthorA;

        [DTWAINNativeFunction("DTWAIN_SetAuthorW")]
        private readonly DTWAIN_SetAuthorWDelegate  _DTWAIN_SetAuthorW;

        [DTWAINNativeFunction("DTWAIN_SetAvailablePrinters")]
        private readonly DTWAIN_SetAvailablePrintersDelegate  _DTWAIN_SetAvailablePrinters;

        [DTWAINNativeFunction("DTWAIN_SetAvailablePrintersArray")]
        private readonly DTWAIN_SetAvailablePrintersArrayDelegate  _DTWAIN_SetAvailablePrintersArray;

        [DTWAINNativeFunction("DTWAIN_SetBitDepth")]
        private readonly DTWAIN_SetBitDepthDelegate  _DTWAIN_SetBitDepth;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetection")]
        private readonly DTWAIN_SetBlankPageDetectionDelegate  _DTWAIN_SetBlankPageDetection;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetectionEx")]
        private readonly DTWAIN_SetBlankPageDetectionExDelegate  _DTWAIN_SetBlankPageDetectionEx;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetectionExString")]
        private readonly DTWAIN_SetBlankPageDetectionExStringDelegate  _DTWAIN_SetBlankPageDetectionExString;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetectionExStringA")]
        private readonly DTWAIN_SetBlankPageDetectionExStringADelegate  _DTWAIN_SetBlankPageDetectionExStringA;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetectionExStringW")]
        private readonly DTWAIN_SetBlankPageDetectionExStringWDelegate  _DTWAIN_SetBlankPageDetectionExStringW;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetectionString")]
        private readonly DTWAIN_SetBlankPageDetectionStringDelegate  _DTWAIN_SetBlankPageDetectionString;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetectionStringA")]
        private readonly DTWAIN_SetBlankPageDetectionStringADelegate  _DTWAIN_SetBlankPageDetectionStringA;

        [DTWAINNativeFunction("DTWAIN_SetBlankPageDetectionStringW")]
        private readonly DTWAIN_SetBlankPageDetectionStringWDelegate  _DTWAIN_SetBlankPageDetectionStringW;

        [DTWAINNativeFunction("DTWAIN_SetBrightness")]
        private readonly DTWAIN_SetBrightnessDelegate  _DTWAIN_SetBrightness;

        [DTWAINNativeFunction("DTWAIN_SetBrightnessString")]
        private readonly DTWAIN_SetBrightnessStringDelegate  _DTWAIN_SetBrightnessString;

        [DTWAINNativeFunction("DTWAIN_SetBrightnessStringA")]
        private readonly DTWAIN_SetBrightnessStringADelegate  _DTWAIN_SetBrightnessStringA;

        [DTWAINNativeFunction("DTWAIN_SetBrightnessStringW")]
        private readonly DTWAIN_SetBrightnessStringWDelegate  _DTWAIN_SetBrightnessStringW;

        [DTWAINNativeFunction("DTWAIN_SetBufferedTileMode")]
        private readonly DTWAIN_SetBufferedTileModeDelegate  _DTWAIN_SetBufferedTileMode;

        [DTWAINNativeFunction("DTWAIN_SetCallback")]
        private readonly DTWAIN_SetCallbackDelegate  _DTWAIN_SetCallback;

        [DTWAINNativeFunction("DTWAIN_SetCallback64")]
        private readonly DTWAIN_SetCallback64Delegate  _DTWAIN_SetCallback64;

        [DTWAINNativeFunction("DTWAIN_SetCamera")]
        private readonly DTWAIN_SetCameraDelegate  _DTWAIN_SetCamera;

        [DTWAINNativeFunction("DTWAIN_SetCameraA")]
        private readonly DTWAIN_SetCameraADelegate  _DTWAIN_SetCameraA;

        [DTWAINNativeFunction("DTWAIN_SetCameraW")]
        private readonly DTWAIN_SetCameraWDelegate  _DTWAIN_SetCameraW;

        [DTWAINNativeFunction("DTWAIN_SetCapValues")]
        private readonly DTWAIN_SetCapValuesDelegate  _DTWAIN_SetCapValues;

        [DTWAINNativeFunction("DTWAIN_SetCapValuesEx")]
        private readonly DTWAIN_SetCapValuesExDelegate  _DTWAIN_SetCapValuesEx;

        [DTWAINNativeFunction("DTWAIN_SetCapValuesEx2")]
        private readonly DTWAIN_SetCapValuesEx2Delegate  _DTWAIN_SetCapValuesEx2;

        [DTWAINNativeFunction("DTWAIN_SetCaption")]
        private readonly DTWAIN_SetCaptionDelegate  _DTWAIN_SetCaption;

        [DTWAINNativeFunction("DTWAIN_SetCaptionA")]
        private readonly DTWAIN_SetCaptionADelegate  _DTWAIN_SetCaptionA;

        [DTWAINNativeFunction("DTWAIN_SetCaptionW")]
        private readonly DTWAIN_SetCaptionWDelegate  _DTWAIN_SetCaptionW;

        [DTWAINNativeFunction("DTWAIN_SetCompressionType")]
        private readonly DTWAIN_SetCompressionTypeDelegate  _DTWAIN_SetCompressionType;

        [DTWAINNativeFunction("DTWAIN_SetContrast")]
        private readonly DTWAIN_SetContrastDelegate  _DTWAIN_SetContrast;

        [DTWAINNativeFunction("DTWAIN_SetContrastString")]
        private readonly DTWAIN_SetContrastStringDelegate  _DTWAIN_SetContrastString;

        [DTWAINNativeFunction("DTWAIN_SetContrastStringA")]
        private readonly DTWAIN_SetContrastStringADelegate  _DTWAIN_SetContrastStringA;

        [DTWAINNativeFunction("DTWAIN_SetContrastStringW")]
        private readonly DTWAIN_SetContrastStringWDelegate  _DTWAIN_SetContrastStringW;

        [DTWAINNativeFunction("DTWAIN_SetCountry")]
        private readonly DTWAIN_SetCountryDelegate  _DTWAIN_SetCountry;

        [DTWAINNativeFunction("DTWAIN_SetCurrentRetryCount")]
        private readonly DTWAIN_SetCurrentRetryCountDelegate  _DTWAIN_SetCurrentRetryCount;

        [DTWAINNativeFunction("DTWAIN_SetCustomDSData")]
        private readonly DTWAIN_SetCustomDSDataDelegate  _DTWAIN_SetCustomDSData;

        [DTWAINNativeFunction("DTWAIN_SetDSMSearchOrder")]
        private readonly DTWAIN_SetDSMSearchOrderDelegate  _DTWAIN_SetDSMSearchOrder;

        [DTWAINNativeFunction("DTWAIN_SetDSMSearchOrderEx")]
        private readonly DTWAIN_SetDSMSearchOrderExDelegate  _DTWAIN_SetDSMSearchOrderEx;

        [DTWAINNativeFunction("DTWAIN_SetDSMSearchOrderExA")]
        private readonly DTWAIN_SetDSMSearchOrderExADelegate  _DTWAIN_SetDSMSearchOrderExA;

        [DTWAINNativeFunction("DTWAIN_SetDSMSearchOrderExW")]
        private readonly DTWAIN_SetDSMSearchOrderExWDelegate  _DTWAIN_SetDSMSearchOrderExW;

        [DTWAINNativeFunction("DTWAIN_SetDefaultSource")]
        private readonly DTWAIN_SetDefaultSourceDelegate  _DTWAIN_SetDefaultSource;

        [DTWAINNativeFunction("DTWAIN_SetDeviceNotifications")]
        private readonly DTWAIN_SetDeviceNotificationsDelegate  _DTWAIN_SetDeviceNotifications;

        [DTWAINNativeFunction("DTWAIN_SetDeviceTimeDate")]
        private readonly DTWAIN_SetDeviceTimeDateDelegate  _DTWAIN_SetDeviceTimeDate;

        [DTWAINNativeFunction("DTWAIN_SetDeviceTimeDateA")]
        private readonly DTWAIN_SetDeviceTimeDateADelegate  _DTWAIN_SetDeviceTimeDateA;

        [DTWAINNativeFunction("DTWAIN_SetDeviceTimeDateW")]
        private readonly DTWAIN_SetDeviceTimeDateWDelegate  _DTWAIN_SetDeviceTimeDateW;

        [DTWAINNativeFunction("DTWAIN_SetDoubleFeedDetectLength")]
        private readonly DTWAIN_SetDoubleFeedDetectLengthDelegate  _DTWAIN_SetDoubleFeedDetectLength;

        [DTWAINNativeFunction("DTWAIN_SetDoubleFeedDetectLengthString")]
        private readonly DTWAIN_SetDoubleFeedDetectLengthStringDelegate  _DTWAIN_SetDoubleFeedDetectLengthString;

        [DTWAINNativeFunction("DTWAIN_SetDoubleFeedDetectLengthStringA")]
        private readonly DTWAIN_SetDoubleFeedDetectLengthStringADelegate  _DTWAIN_SetDoubleFeedDetectLengthStringA;

        [DTWAINNativeFunction("DTWAIN_SetDoubleFeedDetectLengthStringW")]
        private readonly DTWAIN_SetDoubleFeedDetectLengthStringWDelegate  _DTWAIN_SetDoubleFeedDetectLengthStringW;

        [DTWAINNativeFunction("DTWAIN_SetDoubleFeedDetectValues")]
        private readonly DTWAIN_SetDoubleFeedDetectValuesDelegate  _DTWAIN_SetDoubleFeedDetectValues;

        [DTWAINNativeFunction("DTWAIN_SetDoublePageCountOnDuplex")]
        private readonly DTWAIN_SetDoublePageCountOnDuplexDelegate  _DTWAIN_SetDoublePageCountOnDuplex;

        [DTWAINNativeFunction("DTWAIN_SetEOJDetectValue")]
        private readonly DTWAIN_SetEOJDetectValueDelegate  _DTWAIN_SetEOJDetectValue;

        [DTWAINNativeFunction("DTWAIN_SetErrorBufferThreshold")]
        private readonly DTWAIN_SetErrorBufferThresholdDelegate  _DTWAIN_SetErrorBufferThreshold;

        [DTWAINNativeFunction("DTWAIN_SetErrorCallback")]
        private readonly DTWAIN_SetErrorCallbackDelegate  _DTWAIN_SetErrorCallback;

        [DTWAINNativeFunction("DTWAIN_SetErrorCallback64")]
        private readonly DTWAIN_SetErrorCallback64Delegate  _DTWAIN_SetErrorCallback64;

        [DTWAINNativeFunction("DTWAIN_SetFeederAlignment")]
        private readonly DTWAIN_SetFeederAlignmentDelegate  _DTWAIN_SetFeederAlignment;

        [DTWAINNativeFunction("DTWAIN_SetFeederOrder")]
        private readonly DTWAIN_SetFeederOrderDelegate  _DTWAIN_SetFeederOrder;

        [DTWAINNativeFunction("DTWAIN_SetFeederWaitTime")]
        private readonly DTWAIN_SetFeederWaitTimeDelegate  _DTWAIN_SetFeederWaitTime;

        [DTWAINNativeFunction("DTWAIN_SetFileAutoIncrement")]
        private readonly DTWAIN_SetFileAutoIncrementDelegate  _DTWAIN_SetFileAutoIncrement;

        [DTWAINNativeFunction("DTWAIN_SetFileCompressionType")]
        private readonly DTWAIN_SetFileCompressionTypeDelegate  _DTWAIN_SetFileCompressionType;

        [DTWAINNativeFunction("DTWAIN_SetFileSavePos")]
        private readonly DTWAIN_SetFileSavePosDelegate  _DTWAIN_SetFileSavePos;

        [DTWAINNativeFunction("DTWAIN_SetFileSavePosA")]
        private readonly DTWAIN_SetFileSavePosADelegate  _DTWAIN_SetFileSavePosA;

        [DTWAINNativeFunction("DTWAIN_SetFileSavePosW")]
        private readonly DTWAIN_SetFileSavePosWDelegate  _DTWAIN_SetFileSavePosW;

        [DTWAINNativeFunction("DTWAIN_SetFileXferFormat")]
        private readonly DTWAIN_SetFileXferFormatDelegate  _DTWAIN_SetFileXferFormat;

        [DTWAINNativeFunction("DTWAIN_SetHalftone")]
        private readonly DTWAIN_SetHalftoneDelegate  _DTWAIN_SetHalftone;

        [DTWAINNativeFunction("DTWAIN_SetHalftoneA")]
        private readonly DTWAIN_SetHalftoneADelegate  _DTWAIN_SetHalftoneA;

        [DTWAINNativeFunction("DTWAIN_SetHalftoneW")]
        private readonly DTWAIN_SetHalftoneWDelegate  _DTWAIN_SetHalftoneW;

        [DTWAINNativeFunction("DTWAIN_SetHighlight")]
        private readonly DTWAIN_SetHighlightDelegate  _DTWAIN_SetHighlight;

        [DTWAINNativeFunction("DTWAIN_SetHighlightString")]
        private readonly DTWAIN_SetHighlightStringDelegate  _DTWAIN_SetHighlightString;

        [DTWAINNativeFunction("DTWAIN_SetHighlightStringA")]
        private readonly DTWAIN_SetHighlightStringADelegate  _DTWAIN_SetHighlightStringA;

        [DTWAINNativeFunction("DTWAIN_SetHighlightStringW")]
        private readonly DTWAIN_SetHighlightStringWDelegate  _DTWAIN_SetHighlightStringW;

        [DTWAINNativeFunction("DTWAIN_SetJobControl")]
        private readonly DTWAIN_SetJobControlDelegate  _DTWAIN_SetJobControl;

        [DTWAINNativeFunction("DTWAIN_SetJpegValues")]
        private readonly DTWAIN_SetJpegValuesDelegate  _DTWAIN_SetJpegValues;

        [DTWAINNativeFunction("DTWAIN_SetJpegXRValues")]
        private readonly DTWAIN_SetJpegXRValuesDelegate  _DTWAIN_SetJpegXRValues;

        [DTWAINNativeFunction("DTWAIN_SetLanguage")]
        private readonly DTWAIN_SetLanguageDelegate  _DTWAIN_SetLanguage;

        [DTWAINNativeFunction("DTWAIN_SetLastError")]
        private readonly DTWAIN_SetLastErrorDelegate  _DTWAIN_SetLastError;

        [DTWAINNativeFunction("DTWAIN_SetLightPath")]
        private readonly DTWAIN_SetLightPathDelegate  _DTWAIN_SetLightPath;

        [DTWAINNativeFunction("DTWAIN_SetLightPathEx")]
        private readonly DTWAIN_SetLightPathExDelegate  _DTWAIN_SetLightPathEx;

        [DTWAINNativeFunction("DTWAIN_SetLightSource")]
        private readonly DTWAIN_SetLightSourceDelegate  _DTWAIN_SetLightSource;

        [DTWAINNativeFunction("DTWAIN_SetLightSources")]
        private readonly DTWAIN_SetLightSourcesDelegate  _DTWAIN_SetLightSources;

        [DTWAINNativeFunction("DTWAIN_SetLoggerCallback")]
        private readonly DTWAIN_SetLoggerCallbackDelegate  _DTWAIN_SetLoggerCallback;

        [DTWAINNativeFunction("DTWAIN_SetLoggerCallbackA")]
        private readonly DTWAIN_SetLoggerCallbackADelegate  _DTWAIN_SetLoggerCallbackA;

        [DTWAINNativeFunction("DTWAIN_SetLoggerCallbackW")]
        private readonly DTWAIN_SetLoggerCallbackWDelegate  _DTWAIN_SetLoggerCallbackW;

        [DTWAINNativeFunction("DTWAIN_SetManualDuplexMode")]
        private readonly DTWAIN_SetManualDuplexModeDelegate  _DTWAIN_SetManualDuplexMode;

        [DTWAINNativeFunction("DTWAIN_SetMaxAcquisitions")]
        private readonly DTWAIN_SetMaxAcquisitionsDelegate  _DTWAIN_SetMaxAcquisitions;

        [DTWAINNativeFunction("DTWAIN_SetMaxBuffers")]
        private readonly DTWAIN_SetMaxBuffersDelegate  _DTWAIN_SetMaxBuffers;

        [DTWAINNativeFunction("DTWAIN_SetMaxRetryAttempts")]
        private readonly DTWAIN_SetMaxRetryAttemptsDelegate  _DTWAIN_SetMaxRetryAttempts;

        [DTWAINNativeFunction("DTWAIN_SetMultipageScanMode")]
        private readonly DTWAIN_SetMultipageScanModeDelegate  _DTWAIN_SetMultipageScanMode;

        [DTWAINNativeFunction("DTWAIN_SetNoiseFilter")]
        private readonly DTWAIN_SetNoiseFilterDelegate  _DTWAIN_SetNoiseFilter;

        [DTWAINNativeFunction("DTWAIN_SetOCRCapValues")]
        private readonly DTWAIN_SetOCRCapValuesDelegate  _DTWAIN_SetOCRCapValues;

        [DTWAINNativeFunction("DTWAIN_SetOrientation")]
        private readonly DTWAIN_SetOrientationDelegate  _DTWAIN_SetOrientation;

        [DTWAINNativeFunction("DTWAIN_SetOverscan")]
        private readonly DTWAIN_SetOverscanDelegate  _DTWAIN_SetOverscan;

        [DTWAINNativeFunction("DTWAIN_SetPDFAESEncryption")]
        private readonly DTWAIN_SetPDFAESEncryptionDelegate  _DTWAIN_SetPDFAESEncryption;

        [DTWAINNativeFunction("DTWAIN_SetPDFASCIICompression")]
        private readonly DTWAIN_SetPDFASCIICompressionDelegate  _DTWAIN_SetPDFASCIICompression;

        [DTWAINNativeFunction("DTWAIN_SetPDFAuthor")]
        private readonly DTWAIN_SetPDFAuthorDelegate  _DTWAIN_SetPDFAuthor;

        [DTWAINNativeFunction("DTWAIN_SetPDFAuthorA")]
        private readonly DTWAIN_SetPDFAuthorADelegate  _DTWAIN_SetPDFAuthorA;

        [DTWAINNativeFunction("DTWAIN_SetPDFAuthorW")]
        private readonly DTWAIN_SetPDFAuthorWDelegate  _DTWAIN_SetPDFAuthorW;

        [DTWAINNativeFunction("DTWAIN_SetPDFCompression")]
        private readonly DTWAIN_SetPDFCompressionDelegate  _DTWAIN_SetPDFCompression;

        [DTWAINNativeFunction("DTWAIN_SetPDFCreator")]
        private readonly DTWAIN_SetPDFCreatorDelegate  _DTWAIN_SetPDFCreator;

        [DTWAINNativeFunction("DTWAIN_SetPDFCreatorA")]
        private readonly DTWAIN_SetPDFCreatorADelegate  _DTWAIN_SetPDFCreatorA;

        [DTWAINNativeFunction("DTWAIN_SetPDFCreatorW")]
        private readonly DTWAIN_SetPDFCreatorWDelegate  _DTWAIN_SetPDFCreatorW;

        [DTWAINNativeFunction("DTWAIN_SetPDFEncryption")]
        private readonly DTWAIN_SetPDFEncryptionDelegate  _DTWAIN_SetPDFEncryption;

        [DTWAINNativeFunction("DTWAIN_SetPDFEncryptionA")]
        private readonly DTWAIN_SetPDFEncryptionADelegate  _DTWAIN_SetPDFEncryptionA;

        [DTWAINNativeFunction("DTWAIN_SetPDFEncryptionW")]
        private readonly DTWAIN_SetPDFEncryptionWDelegate  _DTWAIN_SetPDFEncryptionW;

        [DTWAINNativeFunction("DTWAIN_SetPDFJpegQuality")]
        private readonly DTWAIN_SetPDFJpegQualityDelegate  _DTWAIN_SetPDFJpegQuality;

        [DTWAINNativeFunction("DTWAIN_SetPDFKeywords")]
        private readonly DTWAIN_SetPDFKeywordsDelegate  _DTWAIN_SetPDFKeywords;

        [DTWAINNativeFunction("DTWAIN_SetPDFKeywordsA")]
        private readonly DTWAIN_SetPDFKeywordsADelegate  _DTWAIN_SetPDFKeywordsA;

        [DTWAINNativeFunction("DTWAIN_SetPDFKeywordsW")]
        private readonly DTWAIN_SetPDFKeywordsWDelegate  _DTWAIN_SetPDFKeywordsW;

        [DTWAINNativeFunction("DTWAIN_SetPDFOCRConversion")]
        private readonly DTWAIN_SetPDFOCRConversionDelegate  _DTWAIN_SetPDFOCRConversion;

        [DTWAINNativeFunction("DTWAIN_SetPDFOCRMode")]
        private readonly DTWAIN_SetPDFOCRModeDelegate  _DTWAIN_SetPDFOCRMode;

        [DTWAINNativeFunction("DTWAIN_SetPDFOrientation")]
        private readonly DTWAIN_SetPDFOrientationDelegate  _DTWAIN_SetPDFOrientation;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageScale")]
        private readonly DTWAIN_SetPDFPageScaleDelegate  _DTWAIN_SetPDFPageScale;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageScaleString")]
        private readonly DTWAIN_SetPDFPageScaleStringDelegate  _DTWAIN_SetPDFPageScaleString;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageScaleStringA")]
        private readonly DTWAIN_SetPDFPageScaleStringADelegate  _DTWAIN_SetPDFPageScaleStringA;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageScaleStringW")]
        private readonly DTWAIN_SetPDFPageScaleStringWDelegate  _DTWAIN_SetPDFPageScaleStringW;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageSize")]
        private readonly DTWAIN_SetPDFPageSizeDelegate  _DTWAIN_SetPDFPageSize;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageSizeString")]
        private readonly DTWAIN_SetPDFPageSizeStringDelegate  _DTWAIN_SetPDFPageSizeString;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageSizeStringA")]
        private readonly DTWAIN_SetPDFPageSizeStringADelegate  _DTWAIN_SetPDFPageSizeStringA;

        [DTWAINNativeFunction("DTWAIN_SetPDFPageSizeStringW")]
        private readonly DTWAIN_SetPDFPageSizeStringWDelegate  _DTWAIN_SetPDFPageSizeStringW;

        [DTWAINNativeFunction("DTWAIN_SetPDFPolarity")]
        private readonly DTWAIN_SetPDFPolarityDelegate  _DTWAIN_SetPDFPolarity;

        [DTWAINNativeFunction("DTWAIN_SetPDFProducer")]
        private readonly DTWAIN_SetPDFProducerDelegate  _DTWAIN_SetPDFProducer;

        [DTWAINNativeFunction("DTWAIN_SetPDFProducerA")]
        private readonly DTWAIN_SetPDFProducerADelegate  _DTWAIN_SetPDFProducerA;

        [DTWAINNativeFunction("DTWAIN_SetPDFProducerW")]
        private readonly DTWAIN_SetPDFProducerWDelegate  _DTWAIN_SetPDFProducerW;

        [DTWAINNativeFunction("DTWAIN_SetPDFSubject")]
        private readonly DTWAIN_SetPDFSubjectDelegate  _DTWAIN_SetPDFSubject;

        [DTWAINNativeFunction("DTWAIN_SetPDFSubjectA")]
        private readonly DTWAIN_SetPDFSubjectADelegate  _DTWAIN_SetPDFSubjectA;

        [DTWAINNativeFunction("DTWAIN_SetPDFSubjectW")]
        private readonly DTWAIN_SetPDFSubjectWDelegate  _DTWAIN_SetPDFSubjectW;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementFloat")]
        private readonly DTWAIN_SetPDFTextElementFloatDelegate  _DTWAIN_SetPDFTextElementFloat;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementFloatString")]
        private readonly DTWAIN_SetPDFTextElementFloatStringDelegate  _DTWAIN_SetPDFTextElementFloatString;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementFloatStringA")]
        private readonly DTWAIN_SetPDFTextElementFloatStringADelegate  _DTWAIN_SetPDFTextElementFloatStringA;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementFloatStringW")]
        private readonly DTWAIN_SetPDFTextElementFloatStringWDelegate  _DTWAIN_SetPDFTextElementFloatStringW;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementLong")]
        private readonly DTWAIN_SetPDFTextElementLongDelegate  _DTWAIN_SetPDFTextElementLong;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementString")]
        private readonly DTWAIN_SetPDFTextElementStringDelegate  _DTWAIN_SetPDFTextElementString;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementStringA")]
        private readonly DTWAIN_SetPDFTextElementStringADelegate  _DTWAIN_SetPDFTextElementStringA;

        [DTWAINNativeFunction("DTWAIN_SetPDFTextElementStringW")]
        private readonly DTWAIN_SetPDFTextElementStringWDelegate  _DTWAIN_SetPDFTextElementStringW;

        [DTWAINNativeFunction("DTWAIN_SetPDFTitle")]
        private readonly DTWAIN_SetPDFTitleDelegate  _DTWAIN_SetPDFTitle;

        [DTWAINNativeFunction("DTWAIN_SetPDFTitleA")]
        private readonly DTWAIN_SetPDFTitleADelegate  _DTWAIN_SetPDFTitleA;

        [DTWAINNativeFunction("DTWAIN_SetPDFTitleW")]
        private readonly DTWAIN_SetPDFTitleWDelegate  _DTWAIN_SetPDFTitleW;

        [DTWAINNativeFunction("DTWAIN_SetPaperSize")]
        private readonly DTWAIN_SetPaperSizeDelegate  _DTWAIN_SetPaperSize;

        [DTWAINNativeFunction("DTWAIN_SetPatchMaxPriorities")]
        private readonly DTWAIN_SetPatchMaxPrioritiesDelegate  _DTWAIN_SetPatchMaxPriorities;

        [DTWAINNativeFunction("DTWAIN_SetPatchMaxRetries")]
        private readonly DTWAIN_SetPatchMaxRetriesDelegate  _DTWAIN_SetPatchMaxRetries;

        [DTWAINNativeFunction("DTWAIN_SetPatchPriorities")]
        private readonly DTWAIN_SetPatchPrioritiesDelegate  _DTWAIN_SetPatchPriorities;

        [DTWAINNativeFunction("DTWAIN_SetPatchSearchMode")]
        private readonly DTWAIN_SetPatchSearchModeDelegate  _DTWAIN_SetPatchSearchMode;

        [DTWAINNativeFunction("DTWAIN_SetPatchTimeOut")]
        private readonly DTWAIN_SetPatchTimeOutDelegate  _DTWAIN_SetPatchTimeOut;

        [DTWAINNativeFunction("DTWAIN_SetPixelFlavor")]
        private readonly DTWAIN_SetPixelFlavorDelegate  _DTWAIN_SetPixelFlavor;

        [DTWAINNativeFunction("DTWAIN_SetPixelType")]
        private readonly DTWAIN_SetPixelTypeDelegate  _DTWAIN_SetPixelType;

        [DTWAINNativeFunction("DTWAIN_SetPostScriptTitle")]
        private readonly DTWAIN_SetPostScriptTitleDelegate  _DTWAIN_SetPostScriptTitle;

        [DTWAINNativeFunction("DTWAIN_SetPostScriptTitleA")]
        private readonly DTWAIN_SetPostScriptTitleADelegate  _DTWAIN_SetPostScriptTitleA;

        [DTWAINNativeFunction("DTWAIN_SetPostScriptTitleW")]
        private readonly DTWAIN_SetPostScriptTitleWDelegate  _DTWAIN_SetPostScriptTitleW;

        [DTWAINNativeFunction("DTWAIN_SetPostScriptType")]
        private readonly DTWAIN_SetPostScriptTypeDelegate  _DTWAIN_SetPostScriptType;

        [DTWAINNativeFunction("DTWAIN_SetPrinter")]
        private readonly DTWAIN_SetPrinterDelegate  _DTWAIN_SetPrinter;

        [DTWAINNativeFunction("DTWAIN_SetPrinterEx")]
        private readonly DTWAIN_SetPrinterExDelegate  _DTWAIN_SetPrinterEx;

        [DTWAINNativeFunction("DTWAIN_SetPrinterStartNumber")]
        private readonly DTWAIN_SetPrinterStartNumberDelegate  _DTWAIN_SetPrinterStartNumber;

        [DTWAINNativeFunction("DTWAIN_SetPrinterStringMode")]
        private readonly DTWAIN_SetPrinterStringModeDelegate  _DTWAIN_SetPrinterStringMode;

        [DTWAINNativeFunction("DTWAIN_SetPrinterStrings")]
        private readonly DTWAIN_SetPrinterStringsDelegate  _DTWAIN_SetPrinterStrings;

        [DTWAINNativeFunction("DTWAIN_SetPrinterSuffixString")]
        private readonly DTWAIN_SetPrinterSuffixStringDelegate  _DTWAIN_SetPrinterSuffixString;

        [DTWAINNativeFunction("DTWAIN_SetPrinterSuffixStringA")]
        private readonly DTWAIN_SetPrinterSuffixStringADelegate  _DTWAIN_SetPrinterSuffixStringA;

        [DTWAINNativeFunction("DTWAIN_SetPrinterSuffixStringW")]
        private readonly DTWAIN_SetPrinterSuffixStringWDelegate  _DTWAIN_SetPrinterSuffixStringW;

        [DTWAINNativeFunction("DTWAIN_SetQueryCapSupport")]
        private readonly DTWAIN_SetQueryCapSupportDelegate  _DTWAIN_SetQueryCapSupport;

        [DTWAINNativeFunction("DTWAIN_SetResolution")]
        private readonly DTWAIN_SetResolutionDelegate  _DTWAIN_SetResolution;

        [DTWAINNativeFunction("DTWAIN_SetResolutionString")]
        private readonly DTWAIN_SetResolutionStringDelegate  _DTWAIN_SetResolutionString;

        [DTWAINNativeFunction("DTWAIN_SetResolutionStringA")]
        private readonly DTWAIN_SetResolutionStringADelegate  _DTWAIN_SetResolutionStringA;

        [DTWAINNativeFunction("DTWAIN_SetResolutionStringW")]
        private readonly DTWAIN_SetResolutionStringWDelegate  _DTWAIN_SetResolutionStringW;

        [DTWAINNativeFunction("DTWAIN_SetResourcePath")]
        private readonly DTWAIN_SetResourcePathDelegate  _DTWAIN_SetResourcePath;

        [DTWAINNativeFunction("DTWAIN_SetResourcePathA")]
        private readonly DTWAIN_SetResourcePathADelegate  _DTWAIN_SetResourcePathA;

        [DTWAINNativeFunction("DTWAIN_SetResourcePathW")]
        private readonly DTWAIN_SetResourcePathWDelegate  _DTWAIN_SetResourcePathW;

        [DTWAINNativeFunction("DTWAIN_SetRotation")]
        private readonly DTWAIN_SetRotationDelegate  _DTWAIN_SetRotation;

        [DTWAINNativeFunction("DTWAIN_SetRotationString")]
        private readonly DTWAIN_SetRotationStringDelegate  _DTWAIN_SetRotationString;

        [DTWAINNativeFunction("DTWAIN_SetRotationStringA")]
        private readonly DTWAIN_SetRotationStringADelegate  _DTWAIN_SetRotationStringA;

        [DTWAINNativeFunction("DTWAIN_SetRotationStringW")]
        private readonly DTWAIN_SetRotationStringWDelegate  _DTWAIN_SetRotationStringW;

        [DTWAINNativeFunction("DTWAIN_SetSaveFileName")]
        private readonly DTWAIN_SetSaveFileNameDelegate  _DTWAIN_SetSaveFileName;

        [DTWAINNativeFunction("DTWAIN_SetSaveFileNameA")]
        private readonly DTWAIN_SetSaveFileNameADelegate  _DTWAIN_SetSaveFileNameA;

        [DTWAINNativeFunction("DTWAIN_SetSaveFileNameW")]
        private readonly DTWAIN_SetSaveFileNameWDelegate  _DTWAIN_SetSaveFileNameW;

        [DTWAINNativeFunction("DTWAIN_SetShadow")]
        private readonly DTWAIN_SetShadowDelegate  _DTWAIN_SetShadow;

        [DTWAINNativeFunction("DTWAIN_SetShadowString")]
        private readonly DTWAIN_SetShadowStringDelegate  _DTWAIN_SetShadowString;

        [DTWAINNativeFunction("DTWAIN_SetShadowStringA")]
        private readonly DTWAIN_SetShadowStringADelegate  _DTWAIN_SetShadowStringA;

        [DTWAINNativeFunction("DTWAIN_SetShadowStringW")]
        private readonly DTWAIN_SetShadowStringWDelegate  _DTWAIN_SetShadowStringW;

        [DTWAINNativeFunction("DTWAIN_SetSourceUnit")]
        private readonly DTWAIN_SetSourceUnitDelegate  _DTWAIN_SetSourceUnit;

        [DTWAINNativeFunction("DTWAIN_SetTIFFCompressType")]
        private readonly DTWAIN_SetTIFFCompressTypeDelegate  _DTWAIN_SetTIFFCompressType;

        [DTWAINNativeFunction("DTWAIN_SetTIFFInvert")]
        private readonly DTWAIN_SetTIFFInvertDelegate  _DTWAIN_SetTIFFInvert;

        [DTWAINNativeFunction("DTWAIN_SetTempFileDirectory")]
        private readonly DTWAIN_SetTempFileDirectoryDelegate  _DTWAIN_SetTempFileDirectory;

        [DTWAINNativeFunction("DTWAIN_SetTempFileDirectoryA")]
        private readonly DTWAIN_SetTempFileDirectoryADelegate  _DTWAIN_SetTempFileDirectoryA;

        [DTWAINNativeFunction("DTWAIN_SetTempFileDirectoryEx")]
        private readonly DTWAIN_SetTempFileDirectoryExDelegate  _DTWAIN_SetTempFileDirectoryEx;

        [DTWAINNativeFunction("DTWAIN_SetTempFileDirectoryExA")]
        private readonly DTWAIN_SetTempFileDirectoryExADelegate  _DTWAIN_SetTempFileDirectoryExA;

        [DTWAINNativeFunction("DTWAIN_SetTempFileDirectoryExW")]
        private readonly DTWAIN_SetTempFileDirectoryExWDelegate  _DTWAIN_SetTempFileDirectoryExW;

        [DTWAINNativeFunction("DTWAIN_SetTempFileDirectoryW")]
        private readonly DTWAIN_SetTempFileDirectoryWDelegate  _DTWAIN_SetTempFileDirectoryW;

        [DTWAINNativeFunction("DTWAIN_SetThreshold")]
        private readonly DTWAIN_SetThresholdDelegate  _DTWAIN_SetThreshold;

        [DTWAINNativeFunction("DTWAIN_SetThresholdString")]
        private readonly DTWAIN_SetThresholdStringDelegate  _DTWAIN_SetThresholdString;

        [DTWAINNativeFunction("DTWAIN_SetThresholdStringA")]
        private readonly DTWAIN_SetThresholdStringADelegate  _DTWAIN_SetThresholdStringA;

        [DTWAINNativeFunction("DTWAIN_SetThresholdStringW")]
        private readonly DTWAIN_SetThresholdStringWDelegate  _DTWAIN_SetThresholdStringW;

        [DTWAINNativeFunction("DTWAIN_SetTwainDSM")]
        private readonly DTWAIN_SetTwainDSMDelegate  _DTWAIN_SetTwainDSM;

        [DTWAINNativeFunction("DTWAIN_SetTwainLog")]
        private readonly DTWAIN_SetTwainLogDelegate  _DTWAIN_SetTwainLog;

        [DTWAINNativeFunction("DTWAIN_SetTwainLogA")]
        private readonly DTWAIN_SetTwainLogADelegate  _DTWAIN_SetTwainLogA;

        [DTWAINNativeFunction("DTWAIN_SetTwainLogW")]
        private readonly DTWAIN_SetTwainLogWDelegate  _DTWAIN_SetTwainLogW;

        [DTWAINNativeFunction("DTWAIN_SetTwainMode")]
        private readonly DTWAIN_SetTwainModeDelegate  _DTWAIN_SetTwainMode;

        [DTWAINNativeFunction("DTWAIN_SetTwainTimeout")]
        private readonly DTWAIN_SetTwainTimeoutDelegate  _DTWAIN_SetTwainTimeout;

        [DTWAINNativeFunction("DTWAIN_SetUpdateDibProc")]
        private readonly DTWAIN_SetUpdateDibProcDelegate  _DTWAIN_SetUpdateDibProc;

        [DTWAINNativeFunction("DTWAIN_SetXResolution")]
        private readonly DTWAIN_SetXResolutionDelegate  _DTWAIN_SetXResolution;

        [DTWAINNativeFunction("DTWAIN_SetXResolutionString")]
        private readonly DTWAIN_SetXResolutionStringDelegate  _DTWAIN_SetXResolutionString;

        [DTWAINNativeFunction("DTWAIN_SetXResolutionStringA")]
        private readonly DTWAIN_SetXResolutionStringADelegate  _DTWAIN_SetXResolutionStringA;

        [DTWAINNativeFunction("DTWAIN_SetXResolutionStringW")]
        private readonly DTWAIN_SetXResolutionStringWDelegate  _DTWAIN_SetXResolutionStringW;

        [DTWAINNativeFunction("DTWAIN_SetYResolution")]
        private readonly DTWAIN_SetYResolutionDelegate  _DTWAIN_SetYResolution;

        [DTWAINNativeFunction("DTWAIN_SetYResolutionString")]
        private readonly DTWAIN_SetYResolutionStringDelegate  _DTWAIN_SetYResolutionString;

        [DTWAINNativeFunction("DTWAIN_SetYResolutionStringA")]
        private readonly DTWAIN_SetYResolutionStringADelegate  _DTWAIN_SetYResolutionStringA;

        [DTWAINNativeFunction("DTWAIN_SetYResolutionStringW")]
        private readonly DTWAIN_SetYResolutionStringWDelegate  _DTWAIN_SetYResolutionStringW;

        [DTWAINNativeFunction("DTWAIN_ShowUIOnly")]
        private readonly DTWAIN_ShowUIOnlyDelegate  _DTWAIN_ShowUIOnly;

        [DTWAINNativeFunction("DTWAIN_ShutdownOCREngine")]
        private readonly DTWAIN_ShutdownOCREngineDelegate  _DTWAIN_ShutdownOCREngine;

        [DTWAINNativeFunction("DTWAIN_SkipImageInfoError")]
        private readonly DTWAIN_SkipImageInfoErrorDelegate  _DTWAIN_SkipImageInfoError;

        [DTWAINNativeFunction("DTWAIN_StartThread")]
        private readonly DTWAIN_StartThreadDelegate  _DTWAIN_StartThread;

        [DTWAINNativeFunction("DTWAIN_StartTwainSession")]
        private readonly DTWAIN_StartTwainSessionDelegate  _DTWAIN_StartTwainSession;

        [DTWAINNativeFunction("DTWAIN_StartTwainSessionA")]
        private readonly DTWAIN_StartTwainSessionADelegate  _DTWAIN_StartTwainSessionA;

        [DTWAINNativeFunction("DTWAIN_StartTwainSessionW")]
        private readonly DTWAIN_StartTwainSessionWDelegate  _DTWAIN_StartTwainSessionW;

        [DTWAINNativeFunction("DTWAIN_SysDestroy")]
        private readonly DTWAIN_SysDestroyDelegate  _DTWAIN_SysDestroy;

        [DTWAINNativeFunction("DTWAIN_SysInitialize")]
        private readonly DTWAIN_SysInitializeDelegate  _DTWAIN_SysInitialize;

        [DTWAINNativeFunction("DTWAIN_SysInitializeEx")]
        private readonly DTWAIN_SysInitializeExDelegate  _DTWAIN_SysInitializeEx;

        [DTWAINNativeFunction("DTWAIN_SysInitializeEx2")]
        private readonly DTWAIN_SysInitializeEx2Delegate  _DTWAIN_SysInitializeEx2;

        [DTWAINNativeFunction("DTWAIN_SysInitializeEx2A")]
        private readonly DTWAIN_SysInitializeEx2ADelegate  _DTWAIN_SysInitializeEx2A;

        [DTWAINNativeFunction("DTWAIN_SysInitializeEx2W")]
        private readonly DTWAIN_SysInitializeEx2WDelegate  _DTWAIN_SysInitializeEx2W;

        [DTWAINNativeFunction("DTWAIN_SysInitializeExA")]
        private readonly DTWAIN_SysInitializeExADelegate  _DTWAIN_SysInitializeExA;

        [DTWAINNativeFunction("DTWAIN_SysInitializeExW")]
        private readonly DTWAIN_SysInitializeExWDelegate  _DTWAIN_SysInitializeExW;

        [DTWAINNativeFunction("DTWAIN_SysInitializeLib")]
        private readonly DTWAIN_SysInitializeLibDelegate  _DTWAIN_SysInitializeLib;

        [DTWAINNativeFunction("DTWAIN_SysInitializeLibEx")]
        private readonly DTWAIN_SysInitializeLibExDelegate  _DTWAIN_SysInitializeLibEx;

        [DTWAINNativeFunction("DTWAIN_SysInitializeLibEx2")]
        private readonly DTWAIN_SysInitializeLibEx2Delegate  _DTWAIN_SysInitializeLibEx2;

        [DTWAINNativeFunction("DTWAIN_SysInitializeLibEx2A")]
        private readonly DTWAIN_SysInitializeLibEx2ADelegate  _DTWAIN_SysInitializeLibEx2A;

        [DTWAINNativeFunction("DTWAIN_SysInitializeLibEx2W")]
        private readonly DTWAIN_SysInitializeLibEx2WDelegate  _DTWAIN_SysInitializeLibEx2W;

        [DTWAINNativeFunction("DTWAIN_SysInitializeLibExA")]
        private readonly DTWAIN_SysInitializeLibExADelegate  _DTWAIN_SysInitializeLibExA;

        [DTWAINNativeFunction("DTWAIN_SysInitializeLibExW")]
        private readonly DTWAIN_SysInitializeLibExWDelegate  _DTWAIN_SysInitializeLibExW;

        [DTWAINNativeFunction("DTWAIN_SysInitializeNoBlocking")]
        private readonly DTWAIN_SysInitializeNoBlockingDelegate  _DTWAIN_SysInitializeNoBlocking;

        [DTWAINNativeFunction("DTWAIN_TestGetCap")]
        private readonly DTWAIN_TestGetCapDelegate  _DTWAIN_TestGetCap;

        [DTWAINNativeFunction("DTWAIN_UnlockMemory")]
        private readonly DTWAIN_UnlockMemoryDelegate  _DTWAIN_UnlockMemory;

        [DTWAINNativeFunction("DTWAIN_UnlockMemoryEx")]
        private readonly DTWAIN_UnlockMemoryExDelegate  _DTWAIN_UnlockMemoryEx;

        [DTWAINNativeFunction("DTWAIN_UseMultipleThreads")]
        private readonly DTWAIN_UseMultipleThreadsDelegate  _DTWAIN_UseMultipleThreads;
        public  int DTWAIN_AcquireAudioFile(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int lFileFlags, int lMaxClips, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireAudioFile(Source, lpszFile, lFileFlags, lMaxClips, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireAudioFileA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpszFile, int lFileFlags, int lNumClips, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireAudioFileA(Source, lpszFile, lFileFlags, lNumClips, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireAudioFileW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpszFile, int lFileFlags, int lNumClips, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireAudioFileW(Source, lpszFile, lFileFlags, lNumClips, bShowUI, bCloseSource, ref pStatus);

        public  DTWAIN_ARRAY DTWAIN_AcquireAudioNative(DTWAIN_SOURCE Source, int nMaxAudioClips, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireAudioNative(Source, nMaxAudioClips, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireAudioNativeEx(DTWAIN_SOURCE Source, int nMaxAudioClips, int bShowUI, int bCloseSource, DTWAIN_ARRAY Acquisitions, ref int pStatus)
        => _DTWAIN_AcquireAudioNativeEx(Source, nMaxAudioClips, bShowUI, bCloseSource, Acquisitions, ref pStatus);

        public  DTWAIN_ARRAY DTWAIN_AcquireBuffered(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireBuffered(Source, PixelType, nMaxPages, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireBufferedEx(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, DTWAIN_ARRAY Acquisitions, ref int pStatus)
        => _DTWAIN_AcquireBufferedEx(Source, PixelType, nMaxPages, bShowUI, bCloseSource, Acquisitions, ref pStatus);

        public  int DTWAIN_AcquireFile(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireFile(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireFileA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpszFile, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireFileA(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireFileEx(DTWAIN_SOURCE Source, DTWAIN_ARRAY aFileNames, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireFileEx(Source, aFileNames, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireFileW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpszFile, int lFileType, int lFileFlags, int PixelType, int lMaxPages, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireFileW(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, ref pStatus);

        public  DTWAIN_ARRAY DTWAIN_AcquireNative(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireNative(Source, PixelType, nMaxPages, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AcquireNativeEx(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int bShowUI, int bCloseSource, DTWAIN_ARRAY Acquisitions, ref int pStatus)
        => _DTWAIN_AcquireNativeEx(Source, PixelType, nMaxPages, bShowUI, bCloseSource, Acquisitions, ref pStatus);

        public  DTWAIN_ARRAY DTWAIN_AcquireToClipboard(DTWAIN_SOURCE Source, int PixelType, int nMaxPages, int nTransferMode, int bDiscardDibs, int bShowUI, int bCloseSource, ref int pStatus)
        => _DTWAIN_AcquireToClipboard(Source, PixelType, nMaxPages, nTransferMode, bDiscardDibs, bShowUI, bCloseSource, ref pStatus);

        public  int DTWAIN_AddExtImageInfoQuery(DTWAIN_SOURCE Source, int ExtImageInfo)
        => _DTWAIN_AddExtImageInfoQuery(Source, ExtImageInfo);

        public  int DTWAIN_AddFileToAppend([MarshalAs(UnmanagedType.LPTStr)] string szFile)
        => _DTWAIN_AddFileToAppend(szFile);

        public  int DTWAIN_AddFileToAppendA([MarshalAs(UnmanagedType.LPStr)] string szFile)
        => _DTWAIN_AddFileToAppendA(szFile);

        public  int DTWAIN_AddFileToAppendW([MarshalAs(UnmanagedType.LPWStr)] string szFile)
        => _DTWAIN_AddFileToAppendW(szFile);

        public  int DTWAIN_AddPDFText(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string fontName, DTWAIN_FLOAT fontSize, int colorRGB, int renderMode, DTWAIN_FLOAT scaling, DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing, DTWAIN_FLOAT strokeWidth, uint Flags)
        => _DTWAIN_AddPDFText(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);

        public  int DTWAIN_AddPDFTextA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string fontName, DTWAIN_FLOAT fontSize, int colorRGB, int renderMode, DTWAIN_FLOAT scaling, DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing, DTWAIN_FLOAT strokeWidth, uint Flags)
        => _DTWAIN_AddPDFTextA(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);

        public  int DTWAIN_AddPDFTextElement(DTWAIN_SOURCE Source, DTWAIN_PDFTEXTELEMENT TextElement)
        => _DTWAIN_AddPDFTextElement(Source, TextElement);

        public  int DTWAIN_AddPDFTextString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string fontName, [MarshalAs(UnmanagedType.LPTStr)] string fontSize, int colorRGB, int renderMode, [MarshalAs(UnmanagedType.LPTStr)] string scaling, [MarshalAs(UnmanagedType.LPTStr)] string charSpacing, [MarshalAs(UnmanagedType.LPTStr)] string wordSpacing, [MarshalAs(UnmanagedType.LPTStr)] string strokeWidth, uint Flags)
        => _DTWAIN_AddPDFTextString(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);

        public  int DTWAIN_AddPDFTextStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string fontName, [MarshalAs(UnmanagedType.LPStr)] string fontSize, int colorRGB, int renderMode, [MarshalAs(UnmanagedType.LPStr)] string scaling, [MarshalAs(UnmanagedType.LPStr)] string charSpacing, [MarshalAs(UnmanagedType.LPStr)] string wordSpacing, [MarshalAs(UnmanagedType.LPStr)] string strokeWidth, uint Flags)
        => _DTWAIN_AddPDFTextStringA(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);

        public  int DTWAIN_AddPDFTextStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string fontName, [MarshalAs(UnmanagedType.LPWStr)] string fontSize, int colorRGB, int renderMode, [MarshalAs(UnmanagedType.LPWStr)] string scaling, [MarshalAs(UnmanagedType.LPWStr)] string charSpacing, [MarshalAs(UnmanagedType.LPWStr)] string wordSpacing, [MarshalAs(UnmanagedType.LPWStr)] string strokeWidth, uint Flags)
        => _DTWAIN_AddPDFTextStringW(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);

        public  int DTWAIN_AddPDFTextW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szText, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string fontName, DTWAIN_FLOAT fontSize, int colorRGB, int renderMode, DTWAIN_FLOAT scaling, DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing, DTWAIN_FLOAT strokeWidth, uint Flags)
        => _DTWAIN_AddPDFTextW(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);

        public  HANDLE DTWAIN_AllocateMemory(uint memSize)
        => _DTWAIN_AllocateMemory(memSize);

        public  HANDLE DTWAIN_AllocateMemory64(ULONG64 memSize)
        => _DTWAIN_AllocateMemory64(memSize);

        public  HANDLE DTWAIN_AllocateMemoryEx(uint memSize)
        => _DTWAIN_AllocateMemoryEx(memSize);

        public  int DTWAIN_AppHandlesExceptions(int bSet)
        => _DTWAIN_AppHandlesExceptions(bSet);

        public  DTWAIN_ARRAY DTWAIN_ArrayANSIStringToFloat(DTWAIN_ARRAY StringArray)
        => _DTWAIN_ArrayANSIStringToFloat(StringArray);

        public  int DTWAIN_ArrayAdd(DTWAIN_ARRAY pArray, System.IntPtr pVariant)
        => _DTWAIN_ArrayAdd(pArray, pVariant);

        public  int DTWAIN_ArrayAddANSIString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val)
        => _DTWAIN_ArrayAddANSIString(pArray, Val);

        public  int DTWAIN_ArrayAddANSIStringN(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, int num)
        => _DTWAIN_ArrayAddANSIStringN(pArray, Val, num);

        public  int DTWAIN_ArrayAddFloat(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val)
        => _DTWAIN_ArrayAddFloat(pArray, Val);

        public  int DTWAIN_ArrayAddFloatN(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, int num)
        => _DTWAIN_ArrayAddFloatN(pArray, Val, num);

        public  int DTWAIN_ArrayAddFloatString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val)
        => _DTWAIN_ArrayAddFloatString(pArray, Val);

        public  int DTWAIN_ArrayAddFloatStringA(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val)
        => _DTWAIN_ArrayAddFloatStringA(pArray, Val);

        public  int DTWAIN_ArrayAddFloatStringN(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num)
        => _DTWAIN_ArrayAddFloatStringN(pArray, Val, num);

        public  int DTWAIN_ArrayAddFloatStringNA(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, int num)
        => _DTWAIN_ArrayAddFloatStringNA(pArray, Val, num);

        public  int DTWAIN_ArrayAddFloatStringNW(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num)
        => _DTWAIN_ArrayAddFloatStringNW(pArray, Val, num);

        public  int DTWAIN_ArrayAddFloatStringW(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val)
        => _DTWAIN_ArrayAddFloatStringW(pArray, Val);

        public  int DTWAIN_ArrayAddFrame(DTWAIN_ARRAY pArray, DTWAIN_FRAME frame)
        => _DTWAIN_ArrayAddFrame(pArray, frame);

        public  int DTWAIN_ArrayAddFrameN(DTWAIN_ARRAY pArray, DTWAIN_FRAME frame, int num)
        => _DTWAIN_ArrayAddFrameN(pArray, frame, num);

        public  int DTWAIN_ArrayAddLong(DTWAIN_ARRAY pArray, int Val)
        => _DTWAIN_ArrayAddLong(pArray, Val);

        public  int DTWAIN_ArrayAddLong64(DTWAIN_ARRAY pArray, LONG64 Val)
        => _DTWAIN_ArrayAddLong64(pArray, Val);

        public  int DTWAIN_ArrayAddLong64N(DTWAIN_ARRAY pArray, LONG64 Val, int num)
        => _DTWAIN_ArrayAddLong64N(pArray, Val, num);

        public  int DTWAIN_ArrayAddLongN(DTWAIN_ARRAY pArray, int Val, int num)
        => _DTWAIN_ArrayAddLongN(pArray, Val, num);

        public  int DTWAIN_ArrayAddN(DTWAIN_ARRAY pArray, System.IntPtr pVariant, int num)
        => _DTWAIN_ArrayAddN(pArray, pVariant, num);

        public  int DTWAIN_ArrayAddString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val)
        => _DTWAIN_ArrayAddString(pArray, Val);

        public  int DTWAIN_ArrayAddStringA(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val)
        => _DTWAIN_ArrayAddStringA(pArray, Val);

        public  int DTWAIN_ArrayAddStringN(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num)
        => _DTWAIN_ArrayAddStringN(pArray, Val, num);

        public  int DTWAIN_ArrayAddStringNA(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, int num)
        => _DTWAIN_ArrayAddStringNA(pArray, Val, num);

        public  int DTWAIN_ArrayAddStringNW(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num)
        => _DTWAIN_ArrayAddStringNW(pArray, Val, num);

        public  int DTWAIN_ArrayAddStringW(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val)
        => _DTWAIN_ArrayAddStringW(pArray, Val);

        public  int DTWAIN_ArrayAddWideString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val)
        => _DTWAIN_ArrayAddWideString(pArray, Val);

        public  int DTWAIN_ArrayAddWideStringN(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num)
        => _DTWAIN_ArrayAddWideStringN(pArray, Val, num);

        public  DTWAIN_ARRAY DTWAIN_ArrayConvertFix32ToFloat(DTWAIN_ARRAY Fix32Array)
        => _DTWAIN_ArrayConvertFix32ToFloat(Fix32Array);

        public  DTWAIN_ARRAY DTWAIN_ArrayConvertFloatToFix32(DTWAIN_ARRAY FloatArray)
        => _DTWAIN_ArrayConvertFloatToFix32(FloatArray);

        public  int DTWAIN_ArrayCopy(DTWAIN_ARRAY Source, DTWAIN_ARRAY Dest)
        => _DTWAIN_ArrayCopy(Source, Dest);

        public  DTWAIN_ARRAY DTWAIN_ArrayCreate(int nEnumType, int nInitialSize)
        => _DTWAIN_ArrayCreate(nEnumType, nInitialSize);

        public  DTWAIN_ARRAY DTWAIN_ArrayCreateCopy(DTWAIN_ARRAY Source)
        => _DTWAIN_ArrayCreateCopy(Source);

        public  DTWAIN_ARRAY DTWAIN_ArrayCreateFromCap(DTWAIN_SOURCE Source, int lCapType, int lSize)
        => _DTWAIN_ArrayCreateFromCap(Source, lCapType, lSize);

        public  DTWAIN_ARRAY DTWAIN_ArrayCreateFromLong64s(ref long pCArray, int nSize)
        => _DTWAIN_ArrayCreateFromLong64s(ref pCArray, nSize);

        public  DTWAIN_ARRAY DTWAIN_ArrayCreateFromLongs(ref int pCArray, int nSize)
        => _DTWAIN_ArrayCreateFromLongs(ref pCArray, nSize);

        public  DTWAIN_ARRAY DTWAIN_ArrayCreateFromReals(ref DTWAIN_FLOAT pCArray, int nSize)
        => _DTWAIN_ArrayCreateFromReals(ref pCArray, nSize);

        public  int DTWAIN_ArrayDestroy(DTWAIN_ARRAY pArray)
        => _DTWAIN_ArrayDestroy(pArray);

        public  int DTWAIN_ArrayDestroyFrames(DTWAIN_ARRAY FrameArray)
        => _DTWAIN_ArrayDestroyFrames(FrameArray);

        public  int DTWAIN_ArrayFind(DTWAIN_ARRAY pArray, System.IntPtr pVariant)
        => _DTWAIN_ArrayFind(pArray, pVariant);

        public  int DTWAIN_ArrayFindANSIString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string pString)
        => _DTWAIN_ArrayFindANSIString(pArray, pString);

        public  int DTWAIN_ArrayFindFloat(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, DTWAIN_FLOAT Tolerance)
        => _DTWAIN_ArrayFindFloat(pArray, Val, Tolerance);

        public  int DTWAIN_ArrayFindFloatString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, [MarshalAs(UnmanagedType.LPTStr)] string Tolerance)
        => _DTWAIN_ArrayFindFloatString(pArray, Val, Tolerance);

        public  int DTWAIN_ArrayFindFloatStringA(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, [MarshalAs(UnmanagedType.LPStr)] string Tolerance)
        => _DTWAIN_ArrayFindFloatStringA(pArray, Val, Tolerance);

        public  int DTWAIN_ArrayFindFloatStringW(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, [MarshalAs(UnmanagedType.LPWStr)] string Tolerance)
        => _DTWAIN_ArrayFindFloatStringW(pArray, Val, Tolerance);

        public  int DTWAIN_ArrayFindLong(DTWAIN_ARRAY pArray, int Val)
        => _DTWAIN_ArrayFindLong(pArray, Val);

        public  int DTWAIN_ArrayFindLong64(DTWAIN_ARRAY pArray, LONG64 Val)
        => _DTWAIN_ArrayFindLong64(pArray, Val);

        public  int DTWAIN_ArrayFindString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPTStr)] string pString)
        => _DTWAIN_ArrayFindString(pArray, pString);

        public  int DTWAIN_ArrayFindStringA(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPStr)] string pString)
        => _DTWAIN_ArrayFindStringA(pArray, pString);

        public  int DTWAIN_ArrayFindStringW(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string pString)
        => _DTWAIN_ArrayFindStringW(pArray, pString);

        public  int DTWAIN_ArrayFindWideString(DTWAIN_ARRAY pArray, [MarshalAs(UnmanagedType.LPWStr)] string pString)
        => _DTWAIN_ArrayFindWideString(pArray, pString);

        public  int DTWAIN_ArrayFix32GetAt(DTWAIN_ARRAY aFix32, int lPos, ref int Whole, ref int Frac)
        => _DTWAIN_ArrayFix32GetAt(aFix32, lPos, ref Whole, ref Frac);

        public  int DTWAIN_ArrayFix32SetAt(DTWAIN_ARRAY aFix32, int lPos, int Whole, int Frac)
        => _DTWAIN_ArrayFix32SetAt(aFix32, lPos, Whole, Frac);

        public  DTWAIN_ARRAY DTWAIN_ArrayFloatToANSIString(DTWAIN_ARRAY FloatArray)
        => _DTWAIN_ArrayFloatToANSIString(FloatArray);

        public  DTWAIN_ARRAY DTWAIN_ArrayFloatToString(DTWAIN_ARRAY FloatArray)
        => _DTWAIN_ArrayFloatToString(FloatArray);

        public  DTWAIN_ARRAY DTWAIN_ArrayFloatToWideString(DTWAIN_ARRAY FloatArray)
        => _DTWAIN_ArrayFloatToWideString(FloatArray);

        public  int DTWAIN_ArrayGetAt(DTWAIN_ARRAY pArray, int nWhere, System.IntPtr pVariant)
        => _DTWAIN_ArrayGetAt(pArray, nWhere, pVariant);

        public  int DTWAIN_ArrayGetAtANSIString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder pStr)
        => _DTWAIN_ArrayGetAtANSIString(pArray, nWhere, pStr);

        public  int DTWAIN_ArrayGetAtFloat(DTWAIN_ARRAY pArray, int nWhere, ref DTWAIN_FLOAT pVal)
        => _DTWAIN_ArrayGetAtFloat(pArray, nWhere, ref pVal);

        public  int DTWAIN_ArrayGetAtFloatString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Val)
        => _DTWAIN_ArrayGetAtFloatString(pArray, nWhere, Val);

        public  int DTWAIN_ArrayGetAtFloatStringA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Val)
        => _DTWAIN_ArrayGetAtFloatStringA(pArray, nWhere, Val);

        public  int DTWAIN_ArrayGetAtFloatStringW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Val)
        => _DTWAIN_ArrayGetAtFloatStringW(pArray, nWhere, Val);

        public  int DTWAIN_ArrayGetAtFrame(DTWAIN_ARRAY FrameArray, int nWhere, ref DTWAIN_FLOAT pleft, ref DTWAIN_FLOAT ptop, ref DTWAIN_FLOAT pright, ref DTWAIN_FLOAT pbottom)
        => _DTWAIN_ArrayGetAtFrame(FrameArray, nWhere, ref pleft, ref ptop, ref pright, ref pbottom);

        public  int DTWAIN_ArrayGetAtFrameEx(DTWAIN_ARRAY FrameArray, int nWhere, DTWAIN_FRAME Frame)
        => _DTWAIN_ArrayGetAtFrameEx(FrameArray, nWhere, Frame);

        public  int DTWAIN_ArrayGetAtFrameString(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder bottom)
        => _DTWAIN_ArrayGetAtFrameString(FrameArray, nWhere, left, top, right, bottom);

        public  int DTWAIN_ArrayGetAtFrameStringA(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder bottom)
        => _DTWAIN_ArrayGetAtFrameStringA(FrameArray, nWhere, left, top, right, bottom);

        public  int DTWAIN_ArrayGetAtFrameStringW(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder bottom)
        => _DTWAIN_ArrayGetAtFrameStringW(FrameArray, nWhere, left, top, right, bottom);

        public  int DTWAIN_ArrayGetAtLong(DTWAIN_ARRAY pArray, int nWhere, ref int pVal)
        => _DTWAIN_ArrayGetAtLong(pArray, nWhere, ref pVal);

        public  int DTWAIN_ArrayGetAtLong64(DTWAIN_ARRAY pArray, int nWhere, ref long pVal)
        => _DTWAIN_ArrayGetAtLong64(pArray, nWhere, ref pVal);

        public  int DTWAIN_ArrayGetAtSource(DTWAIN_ARRAY pArray, int nWhere, ref DTWAIN_SOURCE ppSource)
        => _DTWAIN_ArrayGetAtSource(pArray, nWhere, ref ppSource);

        public  DTWAIN_SOURCE DTWAIN_ArrayGetAtSourceEx(DTWAIN_ARRAY pArray, int nWhere)
        => _DTWAIN_ArrayGetAtSourceEx(pArray, nWhere);

        public  int DTWAIN_ArrayGetAtString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pStr)
        => _DTWAIN_ArrayGetAtString(pArray, nWhere, pStr);

        public  int DTWAIN_ArrayGetAtStringA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder pStr)
        => _DTWAIN_ArrayGetAtStringA(pArray, nWhere, pStr);

        public  int DTWAIN_ArrayGetAtStringW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pStr)
        => _DTWAIN_ArrayGetAtStringW(pArray, nWhere, pStr);

        public  int DTWAIN_ArrayGetAtWideString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pStr)
        => _DTWAIN_ArrayGetAtWideString(pArray, nWhere, pStr);

        public  System.IntPtr DTWAIN_ArrayGetBuffer(DTWAIN_ARRAY pArray, int nPos)
        => _DTWAIN_ArrayGetBuffer(pArray, nPos);

        public  DTWAIN_ARRAY DTWAIN_ArrayGetCapValues(DTWAIN_SOURCE Source, int lCap, int lGetType)
        => _DTWAIN_ArrayGetCapValues(Source, lCap, lGetType);

        public  DTWAIN_ARRAY DTWAIN_ArrayGetCapValuesEx(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType)
        => _DTWAIN_ArrayGetCapValuesEx(Source, lCap, lGetType, lContainerType);

        public  DTWAIN_ARRAY DTWAIN_ArrayGetCapValuesEx2(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType, int nDataType)
        => _DTWAIN_ArrayGetCapValuesEx2(Source, lCap, lGetType, lContainerType, nDataType);

        public  int DTWAIN_ArrayGetCount(DTWAIN_ARRAY pArray)
        => _DTWAIN_ArrayGetCount(pArray);

        public  int DTWAIN_ArrayGetMaxStringLength(DTWAIN_ARRAY a)
        => _DTWAIN_ArrayGetMaxStringLength(a);

        public  int DTWAIN_ArrayGetSourceAt(DTWAIN_ARRAY pArray, int nWhere, ref DTWAIN_SOURCE ppSource)
        => _DTWAIN_ArrayGetSourceAt(pArray, nWhere, ref ppSource);

        public  int DTWAIN_ArrayGetStringLength(DTWAIN_ARRAY a, int nWhichString)
        => _DTWAIN_ArrayGetStringLength(a, nWhichString);

        public  int DTWAIN_ArrayGetType(DTWAIN_ARRAY pArray)
        => _DTWAIN_ArrayGetType(pArray);

        public  DTWAIN_ARRAY DTWAIN_ArrayInit()
        => _DTWAIN_ArrayInit();

        public  int DTWAIN_ArrayInsertAt(DTWAIN_ARRAY pArray, int nWhere, System.IntPtr pVariant)
        => _DTWAIN_ArrayInsertAt(pArray, nWhere, pVariant);

        public  int DTWAIN_ArrayInsertAtANSIString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pVal)
        => _DTWAIN_ArrayInsertAtANSIString(pArray, nWhere, pVal);

        public  int DTWAIN_ArrayInsertAtANSIStringN(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtANSIStringN(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtFloat(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FLOAT pVal)
        => _DTWAIN_ArrayInsertAtFloat(pArray, nWhere, pVal);

        public  int DTWAIN_ArrayInsertAtFloatN(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FLOAT Val, int num)
        => _DTWAIN_ArrayInsertAtFloatN(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtFloatString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val)
        => _DTWAIN_ArrayInsertAtFloatString(pArray, nWhere, Val);

        public  int DTWAIN_ArrayInsertAtFloatStringA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val)
        => _DTWAIN_ArrayInsertAtFloatStringA(pArray, nWhere, Val);

        public  int DTWAIN_ArrayInsertAtFloatStringN(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtFloatStringN(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtFloatStringNA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtFloatStringNA(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtFloatStringNW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtFloatStringNW(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtFloatStringW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val)
        => _DTWAIN_ArrayInsertAtFloatStringW(pArray, nWhere, Val);

        public  int DTWAIN_ArrayInsertAtFrame(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FRAME frame)
        => _DTWAIN_ArrayInsertAtFrame(pArray, nWhere, frame);

        public  int DTWAIN_ArrayInsertAtFrameN(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FRAME frame, int num)
        => _DTWAIN_ArrayInsertAtFrameN(pArray, nWhere, frame, num);

        public  int DTWAIN_ArrayInsertAtLong(DTWAIN_ARRAY pArray, int nWhere, int pVal)
        => _DTWAIN_ArrayInsertAtLong(pArray, nWhere, pVal);

        public  int DTWAIN_ArrayInsertAtLong64(DTWAIN_ARRAY pArray, int nWhere, LONG64 Val)
        => _DTWAIN_ArrayInsertAtLong64(pArray, nWhere, Val);

        public  int DTWAIN_ArrayInsertAtLong64N(DTWAIN_ARRAY pArray, int nWhere, LONG64 Val, int num)
        => _DTWAIN_ArrayInsertAtLong64N(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtLongN(DTWAIN_ARRAY pArray, int nWhere, int pVal, int num)
        => _DTWAIN_ArrayInsertAtLongN(pArray, nWhere, pVal, num);

        public  int DTWAIN_ArrayInsertAtN(DTWAIN_ARRAY pArray, int nWhere, System.IntPtr pVariant, int num)
        => _DTWAIN_ArrayInsertAtN(pArray, nWhere, pVariant, num);

        public  int DTWAIN_ArrayInsertAtString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string pVal)
        => _DTWAIN_ArrayInsertAtString(pArray, nWhere, pVal);

        public  int DTWAIN_ArrayInsertAtStringA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pVal)
        => _DTWAIN_ArrayInsertAtStringA(pArray, nWhere, pVal);

        public  int DTWAIN_ArrayInsertAtStringN(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtStringN(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtStringNA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtStringNA(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtStringNW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtStringNW(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayInsertAtStringW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pVal)
        => _DTWAIN_ArrayInsertAtStringW(pArray, nWhere, pVal);

        public  int DTWAIN_ArrayInsertAtWideString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pVal)
        => _DTWAIN_ArrayInsertAtWideString(pArray, nWhere, pVal);

        public  int DTWAIN_ArrayInsertAtWideStringN(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val, int num)
        => _DTWAIN_ArrayInsertAtWideStringN(pArray, nWhere, Val, num);

        public  int DTWAIN_ArrayRemoveAll(DTWAIN_ARRAY pArray)
        => _DTWAIN_ArrayRemoveAll(pArray);

        public  int DTWAIN_ArrayRemoveAt(DTWAIN_ARRAY pArray, int nWhere)
        => _DTWAIN_ArrayRemoveAt(pArray, nWhere);

        public  int DTWAIN_ArrayRemoveAtN(DTWAIN_ARRAY pArray, int nWhere, int num)
        => _DTWAIN_ArrayRemoveAtN(pArray, nWhere, num);

        public  int DTWAIN_ArrayResize(DTWAIN_ARRAY pArray, int NewSize)
        => _DTWAIN_ArrayResize(pArray, NewSize);

        public  int DTWAIN_ArraySetAt(DTWAIN_ARRAY pArray, int lPos, System.IntPtr pVariant)
        => _DTWAIN_ArraySetAt(pArray, lPos, pVariant);

        public  int DTWAIN_ArraySetAtANSIString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pStr)
        => _DTWAIN_ArraySetAtANSIString(pArray, nWhere, pStr);

        public  int DTWAIN_ArraySetAtFloat(DTWAIN_ARRAY pArray, int nWhere, DTWAIN_FLOAT pVal)
        => _DTWAIN_ArraySetAtFloat(pArray, nWhere, pVal);

        public  int DTWAIN_ArraySetAtFloatString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string Val)
        => _DTWAIN_ArraySetAtFloatString(pArray, nWhere, Val);

        public  int DTWAIN_ArraySetAtFloatStringA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string Val)
        => _DTWAIN_ArraySetAtFloatStringA(pArray, nWhere, Val);

        public  int DTWAIN_ArraySetAtFloatStringW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string Val)
        => _DTWAIN_ArraySetAtFloatStringW(pArray, nWhere, Val);

        public  int DTWAIN_ArraySetAtFrame(DTWAIN_ARRAY FrameArray, int nWhere, DTWAIN_FLOAT left, DTWAIN_FLOAT top, DTWAIN_FLOAT right, DTWAIN_FLOAT bottom)
        => _DTWAIN_ArraySetAtFrame(FrameArray, nWhere, left, top, right, bottom);

        public  int DTWAIN_ArraySetAtFrameEx(DTWAIN_ARRAY FrameArray, int nWhere, DTWAIN_FRAME Frame)
        => _DTWAIN_ArraySetAtFrameEx(FrameArray, nWhere, Frame);

        public  int DTWAIN_ArraySetAtFrameString(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string left, [MarshalAs(UnmanagedType.LPTStr)] string top, [MarshalAs(UnmanagedType.LPTStr)] string right, [MarshalAs(UnmanagedType.LPTStr)] string bottom)
        => _DTWAIN_ArraySetAtFrameString(FrameArray, nWhere, left, top, right, bottom);

        public  int DTWAIN_ArraySetAtFrameStringA(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string left, [MarshalAs(UnmanagedType.LPStr)] string top, [MarshalAs(UnmanagedType.LPStr)] string right, [MarshalAs(UnmanagedType.LPStr)] string bottom)
        => _DTWAIN_ArraySetAtFrameStringA(FrameArray, nWhere, left, top, right, bottom);

        public  int DTWAIN_ArraySetAtFrameStringW(DTWAIN_ARRAY FrameArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string left, [MarshalAs(UnmanagedType.LPWStr)] string top, [MarshalAs(UnmanagedType.LPWStr)] string right, [MarshalAs(UnmanagedType.LPWStr)] string bottom)
        => _DTWAIN_ArraySetAtFrameStringW(FrameArray, nWhere, left, top, right, bottom);

        public  int DTWAIN_ArraySetAtLong(DTWAIN_ARRAY pArray, int nWhere, int pVal)
        => _DTWAIN_ArraySetAtLong(pArray, nWhere, pVal);

        public  int DTWAIN_ArraySetAtLong64(DTWAIN_ARRAY pArray, int nWhere, LONG64 Val)
        => _DTWAIN_ArraySetAtLong64(pArray, nWhere, Val);

        public  int DTWAIN_ArraySetAtString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPTStr)] string pStr)
        => _DTWAIN_ArraySetAtString(pArray, nWhere, pStr);

        public  int DTWAIN_ArraySetAtStringA(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPStr)] string pStr)
        => _DTWAIN_ArraySetAtStringA(pArray, nWhere, pStr);

        public  int DTWAIN_ArraySetAtStringW(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pStr)
        => _DTWAIN_ArraySetAtStringW(pArray, nWhere, pStr);

        public  int DTWAIN_ArraySetAtWideString(DTWAIN_ARRAY pArray, int nWhere, [MarshalAs(UnmanagedType.LPWStr)] string pStr)
        => _DTWAIN_ArraySetAtWideString(pArray, nWhere, pStr);

        public  DTWAIN_ARRAY DTWAIN_ArrayStringToFloat(DTWAIN_ARRAY StringArray)
        => _DTWAIN_ArrayStringToFloat(StringArray);

        public  DTWAIN_ARRAY DTWAIN_ArrayWideStringToFloat(DTWAIN_ARRAY StringArray)
        => _DTWAIN_ArrayWideStringToFloat(StringArray);

        public  int DTWAIN_CallCallback(int wParam, int lParam, int UserData)
        => _DTWAIN_CallCallback(wParam, lParam, UserData);

        public  int DTWAIN_CallCallback64(int wParam, int lParam, LONGLONG UserData)
        => _DTWAIN_CallCallback64(wParam, lParam, UserData);

        public  int DTWAIN_CallDSMProc(DTWAIN_IDENTITY AppID, DTWAIN_IDENTITY SourceId, int lDG, int lDAT, int lMSG, System.IntPtr pData)
        => _DTWAIN_CallDSMProc(AppID, SourceId, lDG, lDAT, lMSG, pData);

        public  int DTWAIN_CheckHandles(int bCheck)
        => _DTWAIN_CheckHandles(bCheck);

        public  int DTWAIN_ClearBuffers(DTWAIN_SOURCE Source, int ClearBuffer)
        => _DTWAIN_ClearBuffers(Source, ClearBuffer);

        public  int DTWAIN_ClearErrorBuffer()
        => _DTWAIN_ClearErrorBuffer();

        public  int DTWAIN_ClearPDFTextElements(DTWAIN_SOURCE Source)
        => _DTWAIN_ClearPDFTextElements(Source);

        public  int DTWAIN_ClearPage(DTWAIN_SOURCE Source)
        => _DTWAIN_ClearPage(Source);

        public  int DTWAIN_CloseSource(DTWAIN_SOURCE Source)
        => _DTWAIN_CloseSource(Source);

        public  int DTWAIN_CloseSourceUI(DTWAIN_SOURCE Source)
        => _DTWAIN_CloseSourceUI(Source);

        public  HANDLE DTWAIN_ConvertDIBToBitmap(HANDLE hDib, HANDLE hPalette)
        => _DTWAIN_ConvertDIBToBitmap(hDib, hPalette);

        public  HANDLE DTWAIN_ConvertDIBToFullBitmap(HANDLE hDib, int isBMP)
        => _DTWAIN_ConvertDIBToFullBitmap(hDib, isBMP);

        public  HANDLE DTWAIN_ConvertToAPIString([MarshalAs(UnmanagedType.LPTStr)] string lpOrigString)
        => _DTWAIN_ConvertToAPIString(lpOrigString);

        public  HANDLE DTWAIN_ConvertToAPIStringA([MarshalAs(UnmanagedType.LPStr)] string lpOrigString)
        => _DTWAIN_ConvertToAPIStringA(lpOrigString);

        public  int DTWAIN_ConvertToAPIStringEx([MarshalAs(UnmanagedType.LPTStr)] string lpOrigString, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpOutString, int nSize)
        => _DTWAIN_ConvertToAPIStringEx(lpOrigString, lpOutString, nSize);

        public  int DTWAIN_ConvertToAPIStringExA([MarshalAs(UnmanagedType.LPStr)] string lpOrigString, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpOutString, int nSize)
        => _DTWAIN_ConvertToAPIStringExA(lpOrigString, lpOutString, nSize);

        public  int DTWAIN_ConvertToAPIStringExW([MarshalAs(UnmanagedType.LPWStr)] string lpOrigString, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpOutString, int nSize)
        => _DTWAIN_ConvertToAPIStringExW(lpOrigString, lpOutString, nSize);

        public  HANDLE DTWAIN_ConvertToAPIStringW([MarshalAs(UnmanagedType.LPWStr)] string lpOrigString)
        => _DTWAIN_ConvertToAPIStringW(lpOrigString);

        public  DTWAIN_ARRAY DTWAIN_CreateAcquisitionArray()
        => _DTWAIN_CreateAcquisitionArray();

        public  DTWAIN_PDFTEXTELEMENT DTWAIN_CreatePDFTextElement()
        => _DTWAIN_CreatePDFTextElement();

        public  DTWAIN_PDFTEXTELEMENT DTWAIN_CreatePDFTextElementCopy(DTWAIN_PDFTEXTELEMENT TextElement)
        => _DTWAIN_CreatePDFTextElementCopy(TextElement);

        public  int DTWAIN_DeleteDIB(HANDLE hDib)
        => _DTWAIN_DeleteDIB(hDib);

        public  int DTWAIN_DestroyAcquisitionArray(DTWAIN_ARRAY aAcq, int bDestroyData)
        => _DTWAIN_DestroyAcquisitionArray(aAcq, bDestroyData);

        public  int DTWAIN_DestroyPDFTextElement(DTWAIN_PDFTEXTELEMENT TextElement)
        => _DTWAIN_DestroyPDFTextElement(TextElement);

        public  int DTWAIN_DisableAppWindow(HWND hWnd, int bDisable)
        => _DTWAIN_DisableAppWindow(hWnd, bDisable);

        public  int DTWAIN_EnableAutoBorderDetect(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnableAutoBorderDetect(Source, bEnable);

        public  int DTWAIN_EnableAutoBright(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_EnableAutoBright(Source, bSet);

        public  int DTWAIN_EnableAutoDeskew(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnableAutoDeskew(Source, bEnable);

        public  int DTWAIN_EnableAutoFeed(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_EnableAutoFeed(Source, bSet);

        public  int DTWAIN_EnableAutoRotate(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_EnableAutoRotate(Source, bSet);

        public  int DTWAIN_EnableAutoScan(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnableAutoScan(Source, bEnable);

        public  int DTWAIN_EnableAutomaticSenseMedium(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_EnableAutomaticSenseMedium(Source, bSet);

        public  int DTWAIN_EnableDuplex(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnableDuplex(Source, bEnable);

        public  int DTWAIN_EnableFeeder(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_EnableFeeder(Source, bSet);

        public  int DTWAIN_EnableIndicator(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnableIndicator(Source, bEnable);

        public  int DTWAIN_EnableJobFileHandling(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_EnableJobFileHandling(Source, bSet);

        public  int DTWAIN_EnableLamp(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnableLamp(Source, bEnable);

        public  int DTWAIN_EnableMsgNotify(int bSet)
        => _DTWAIN_EnableMsgNotify(bSet);

        public  int DTWAIN_EnablePatchDetect(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnablePatchDetect(Source, bEnable);

        public  int DTWAIN_EnablePeekMessageLoop(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_EnablePeekMessageLoop(Source, bSet);

        public  int DTWAIN_EnablePrinter(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnablePrinter(Source, bEnable);

        public  int DTWAIN_EnableThumbnail(DTWAIN_SOURCE Source, int bEnable)
        => _DTWAIN_EnableThumbnail(Source, bEnable);

        public  int DTWAIN_EnableTripletsNotify(int bSet)
        => _DTWAIN_EnableTripletsNotify(bSet);

        public  int DTWAIN_EndThread(DTWAIN_HANDLE DLLHandle)
        => _DTWAIN_EndThread(DLLHandle);

        public  int DTWAIN_EndTwainSession()
        => _DTWAIN_EndTwainSession();

        public  int DTWAIN_EnumAlarmVolumes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int expandIfRange)
        => _DTWAIN_EnumAlarmVolumes(Source, ref pArray, expandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumAlarmVolumesEx(DTWAIN_SOURCE Source, int expandIfRange)
        => _DTWAIN_EnumAlarmVolumesEx(Source, expandIfRange);

        public  int DTWAIN_EnumAlarms(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumAlarms(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumAlarmsEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumAlarmsEx(Source);

        public  int DTWAIN_EnumAudioXferMechs(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumAudioXferMechs(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumAudioXferMechsEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumAudioXferMechsEx(Source);

        public  int DTWAIN_EnumAutoFeedValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumAutoFeedValues(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumAutoFeedValuesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumAutoFeedValuesEx(Source);

        public  int DTWAIN_EnumAutomaticCaptures(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumAutomaticCaptures(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumAutomaticCapturesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumAutomaticCapturesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumAutomaticSenseMedium(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumAutomaticSenseMedium(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumAutomaticSenseMediumEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumAutomaticSenseMediumEx(Source);

        public  int DTWAIN_EnumBitDepths(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumBitDepths(Source, ref pArray);

        public  int DTWAIN_EnumBitDepthsEx(DTWAIN_SOURCE Source, int PixelType, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumBitDepthsEx(Source, PixelType, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumBitDepthsEx2(DTWAIN_SOURCE Source, int PixelType)
        => _DTWAIN_EnumBitDepthsEx2(Source, PixelType);

        public  int DTWAIN_EnumBottomCameras(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY Cameras)
        => _DTWAIN_EnumBottomCameras(Source, ref Cameras);

        public  DTWAIN_ARRAY DTWAIN_EnumBottomCamerasEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumBottomCamerasEx(Source);

        public  int DTWAIN_EnumBrightnessValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumBrightnessValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumBrightnessValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumBrightnessValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumCameras(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY Cameras)
        => _DTWAIN_EnumCameras(Source, ref Cameras);

        public  int DTWAIN_EnumCamerasEx(DTWAIN_SOURCE Source, int nWhichCamera, ref DTWAIN_ARRAY Cameras)
        => _DTWAIN_EnumCamerasEx(Source, nWhichCamera, ref Cameras);

        public  DTWAIN_ARRAY DTWAIN_EnumCamerasEx2(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumCamerasEx2(Source);

        public  DTWAIN_ARRAY DTWAIN_EnumCamerasEx3(DTWAIN_SOURCE Source, int nWhichCamera)
        => _DTWAIN_EnumCamerasEx3(Source, nWhichCamera);

        public  int DTWAIN_EnumCompressionTypes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumCompressionTypes(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumCompressionTypesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumCompressionTypesEx(Source);

        public  DTWAIN_ARRAY DTWAIN_EnumCompressionTypesEx2(DTWAIN_SOURCE Source, int lFileType, int bUseBufferedMode)
        => _DTWAIN_EnumCompressionTypesEx2(Source, lFileType, bUseBufferedMode);

        public  int DTWAIN_EnumContrastValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumContrastValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumContrastValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumContrastValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumCustomCaps(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumCustomCaps(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumCustomCapsEx2(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumCustomCapsEx2(Source);

        public  int DTWAIN_EnumDoubleFeedDetectLengths(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumDoubleFeedDetectLengths(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumDoubleFeedDetectLengthsEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumDoubleFeedDetectLengthsEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumDoubleFeedDetectValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumDoubleFeedDetectValues(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumDoubleFeedDetectValuesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumDoubleFeedDetectValuesEx(Source);

        public  int DTWAIN_EnumExtImageInfoTypes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumExtImageInfoTypes(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumExtImageInfoTypesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumExtImageInfoTypesEx(Source);

        public  int DTWAIN_EnumExtendedCaps(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumExtendedCaps(Source, ref pArray);

        public  int DTWAIN_EnumExtendedCapsEx(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumExtendedCapsEx(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumExtendedCapsEx2(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumExtendedCapsEx2(Source);

        public  int DTWAIN_EnumFileTypeBitsPerPixel(int FileType, ref DTWAIN_ARRAY Array)
        => _DTWAIN_EnumFileTypeBitsPerPixel(FileType, ref Array);

        public  int DTWAIN_EnumFileXferFormats(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumFileXferFormats(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumFileXferFormatsEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumFileXferFormatsEx(Source);

        public  int DTWAIN_EnumHalftones(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumHalftones(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumHalftonesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumHalftonesEx(Source);

        public  int DTWAIN_EnumHighlightValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumHighlightValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumHighlightValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumHighlightValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumJobControls(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumJobControls(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumJobControlsEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumJobControlsEx(Source);

        public  int DTWAIN_EnumLightPaths(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY LightPath)
        => _DTWAIN_EnumLightPaths(Source, ref LightPath);

        public  DTWAIN_ARRAY DTWAIN_EnumLightPathsEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumLightPathsEx(Source);

        public  int DTWAIN_EnumLightSources(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY LightSources)
        => _DTWAIN_EnumLightSources(Source, ref LightSources);

        public  DTWAIN_ARRAY DTWAIN_EnumLightSourcesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumLightSourcesEx(Source);

        public  int DTWAIN_EnumMaxBuffers(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pMaxBufs, int bExpandRange)
        => _DTWAIN_EnumMaxBuffers(Source, ref pMaxBufs, bExpandRange);

        public  DTWAIN_ARRAY DTWAIN_EnumMaxBuffersEx(DTWAIN_SOURCE Source, int bExpandRange)
        => _DTWAIN_EnumMaxBuffersEx(Source, bExpandRange);

        public  int DTWAIN_EnumNoiseFilters(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumNoiseFilters(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumNoiseFiltersEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumNoiseFiltersEx(Source);

        public  int DTWAIN_EnumOCRInterfaces(ref DTWAIN_ARRAY OCRInterfaces)
        => _DTWAIN_EnumOCRInterfaces(ref OCRInterfaces);

        public  int DTWAIN_EnumOCRSupportedCaps(DTWAIN_OCRENGINE Engine, ref DTWAIN_ARRAY SupportedCaps)
        => _DTWAIN_EnumOCRSupportedCaps(Engine, ref SupportedCaps);

        public  int DTWAIN_EnumOrientations(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumOrientations(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumOrientationsEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumOrientationsEx(Source);

        public  int DTWAIN_EnumOverscanValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumOverscanValues(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumOverscanValuesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumOverscanValuesEx(Source);

        public  int DTWAIN_EnumPaperSizes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPaperSizes(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPaperSizesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPaperSizesEx(Source);

        public  int DTWAIN_EnumPatchCodes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY PCodes)
        => _DTWAIN_EnumPatchCodes(Source, ref PCodes);

        public  DTWAIN_ARRAY DTWAIN_EnumPatchCodesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPatchCodesEx(Source);

        public  int DTWAIN_EnumPatchMaxPriorities(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPatchMaxPriorities(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPatchMaxPrioritiesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPatchMaxPrioritiesEx(Source);

        public  int DTWAIN_EnumPatchMaxRetries(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPatchMaxRetries(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPatchMaxRetriesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPatchMaxRetriesEx(Source);

        public  int DTWAIN_EnumPatchPriorities(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPatchPriorities(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPatchPrioritiesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPatchPrioritiesEx(Source);

        public  int DTWAIN_EnumPatchSearchModes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPatchSearchModes(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPatchSearchModesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPatchSearchModesEx(Source);

        public  int DTWAIN_EnumPatchTimeOutValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPatchTimeOutValues(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPatchTimeOutValuesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPatchTimeOutValuesEx(Source);

        public  int DTWAIN_EnumPixelTypes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPixelTypes(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPixelTypesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPixelTypesEx(Source);

        public  int DTWAIN_EnumPrinterStringModes(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumPrinterStringModes(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumPrinterStringModesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumPrinterStringModesEx(Source);

        public  int DTWAIN_EnumResolutionValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumResolutionValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumResolutionValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumResolutionValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumShadowValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumShadowValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumShadowValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumShadowValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumSourceUnits(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY lpArray)
        => _DTWAIN_EnumSourceUnits(Source, ref lpArray);

        public  DTWAIN_ARRAY DTWAIN_EnumSourceUnitsEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumSourceUnitsEx(Source);

        public  int DTWAIN_EnumSourceValues(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string capName, ref DTWAIN_ARRAY values, int bExpandIfRange)
        => _DTWAIN_EnumSourceValues(Source, capName, ref values, bExpandIfRange);

        public  int DTWAIN_EnumSourceValuesA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string capName, ref DTWAIN_ARRAY values, int bExpandIfRange)
        => _DTWAIN_EnumSourceValuesA(Source, capName, ref values, bExpandIfRange);

        public  int DTWAIN_EnumSourceValuesW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string capName, ref DTWAIN_ARRAY values, int bExpandIfRange)
        => _DTWAIN_EnumSourceValuesW(Source, capName, ref values, bExpandIfRange);

        public  int DTWAIN_EnumSources(ref DTWAIN_ARRAY lpArray)
        => _DTWAIN_EnumSources(ref lpArray);

        public  DTWAIN_ARRAY DTWAIN_EnumSourcesEx()
        => _DTWAIN_EnumSourcesEx();

        public  int DTWAIN_EnumSupportedCaps(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumSupportedCaps(Source, ref pArray);

        public  int DTWAIN_EnumSupportedCapsEx(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumSupportedCapsEx(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumSupportedCapsEx2(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumSupportedCapsEx2(Source);

        public  int DTWAIN_EnumSupportedExtImageInfo(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumSupportedExtImageInfo(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumSupportedExtImageInfoEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumSupportedExtImageInfoEx(Source);

        public  DTWAIN_ARRAY DTWAIN_EnumSupportedFileTypes()
        => _DTWAIN_EnumSupportedFileTypes();

        public  DTWAIN_ARRAY DTWAIN_EnumSupportedMultiPageFileTypes()
        => _DTWAIN_EnumSupportedMultiPageFileTypes();

        public  DTWAIN_ARRAY DTWAIN_EnumSupportedSinglePageFileTypes()
        => _DTWAIN_EnumSupportedSinglePageFileTypes();

        public  int DTWAIN_EnumThresholdValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumThresholdValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumThresholdValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumThresholdValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumTopCameras(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY Cameras)
        => _DTWAIN_EnumTopCameras(Source, ref Cameras);

        public  DTWAIN_ARRAY DTWAIN_EnumTopCamerasEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumTopCamerasEx(Source);

        public  int DTWAIN_EnumTwainPrinters(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY lpAvailPrinters)
        => _DTWAIN_EnumTwainPrinters(Source, ref lpAvailPrinters);

        public  int DTWAIN_EnumTwainPrintersArray(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_EnumTwainPrintersArray(Source, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_EnumTwainPrintersArrayEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumTwainPrintersArrayEx(Source);

        public  DTWAIN_ARRAY DTWAIN_EnumTwainPrintersEx(DTWAIN_SOURCE Source)
        => _DTWAIN_EnumTwainPrintersEx(Source);

        public  int DTWAIN_EnumXResolutionValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumXResolutionValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumXResolutionValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumXResolutionValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_EnumYResolutionValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray, int bExpandIfRange)
        => _DTWAIN_EnumYResolutionValues(Source, ref pArray, bExpandIfRange);

        public  DTWAIN_ARRAY DTWAIN_EnumYResolutionValuesEx(DTWAIN_SOURCE Source, int bExpandIfRange)
        => _DTWAIN_EnumYResolutionValuesEx(Source, bExpandIfRange);

        public  int DTWAIN_ExecuteOCR(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] string szFileName, int nStartPage, int nEndPage)
        => _DTWAIN_ExecuteOCR(Engine, szFileName, nStartPage, nEndPage);

        public  int DTWAIN_ExecuteOCRA(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] string szFileName, int nStartPage, int nEndPage)
        => _DTWAIN_ExecuteOCRA(Engine, szFileName, nStartPage, nEndPage);

        public  int DTWAIN_ExecuteOCRW(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] string szFileName, int nStartPage, int nEndPage)
        => _DTWAIN_ExecuteOCRW(Engine, szFileName, nStartPage, nEndPage);

        public  int DTWAIN_FeedPage(DTWAIN_SOURCE Source)
        => _DTWAIN_FeedPage(Source);

        public  int DTWAIN_FlipBitmap(HANDLE hDib)
        => _DTWAIN_FlipBitmap(hDib);

        public  int DTWAIN_FlushAcquiredPages(DTWAIN_SOURCE Source)
        => _DTWAIN_FlushAcquiredPages(Source);

        public  int DTWAIN_ForceAcquireBitDepth(DTWAIN_SOURCE Source, int BitDepth)
        => _DTWAIN_ForceAcquireBitDepth(Source, BitDepth);

        public  int DTWAIN_ForceScanOnNoUI(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_ForceScanOnNoUI(Source, bSet);

        public  DTWAIN_FRAME DTWAIN_FrameCreate(DTWAIN_FLOAT Left, DTWAIN_FLOAT Top, DTWAIN_FLOAT Right, DTWAIN_FLOAT Bottom)
        => _DTWAIN_FrameCreate(Left, Top, Right, Bottom);

        public  DTWAIN_FRAME DTWAIN_FrameCreateString([MarshalAs(UnmanagedType.LPTStr)] string Left, [MarshalAs(UnmanagedType.LPTStr)] string Top, [MarshalAs(UnmanagedType.LPTStr)] string Right, [MarshalAs(UnmanagedType.LPTStr)] string Bottom)
        => _DTWAIN_FrameCreateString(Left, Top, Right, Bottom);

        public  DTWAIN_FRAME DTWAIN_FrameCreateStringA([MarshalAs(UnmanagedType.LPStr)] string Left, [MarshalAs(UnmanagedType.LPStr)] string Top, [MarshalAs(UnmanagedType.LPStr)] string Right, [MarshalAs(UnmanagedType.LPStr)] string Bottom)
        => _DTWAIN_FrameCreateStringA(Left, Top, Right, Bottom);

        public  DTWAIN_FRAME DTWAIN_FrameCreateStringW([MarshalAs(UnmanagedType.LPWStr)] string Left, [MarshalAs(UnmanagedType.LPWStr)] string Top, [MarshalAs(UnmanagedType.LPWStr)] string Right, [MarshalAs(UnmanagedType.LPWStr)] string Bottom)
        => _DTWAIN_FrameCreateStringW(Left, Top, Right, Bottom);

        public  int DTWAIN_FrameDestroy(DTWAIN_FRAME Frame)
        => _DTWAIN_FrameDestroy(Frame);

        public  int DTWAIN_FrameGetAll(DTWAIN_FRAME Frame, ref DTWAIN_FLOAT Left, ref DTWAIN_FLOAT Top, ref DTWAIN_FLOAT Right, ref DTWAIN_FLOAT Bottom)
        => _DTWAIN_FrameGetAll(Frame, ref Left, ref Top, ref Right, ref Bottom);

        public  int DTWAIN_FrameGetAllString(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Left, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Top, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Right, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Bottom)
        => _DTWAIN_FrameGetAllString(Frame, Left, Top, Right, Bottom);

        public  int DTWAIN_FrameGetAllStringA(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Left, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Top, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Right, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Bottom)
        => _DTWAIN_FrameGetAllStringA(Frame, Left, Top, Right, Bottom);

        public  int DTWAIN_FrameGetAllStringW(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Left, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Top, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Right, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Bottom)
        => _DTWAIN_FrameGetAllStringW(Frame, Left, Top, Right, Bottom);

        public  int DTWAIN_FrameGetValue(DTWAIN_FRAME Frame, int nWhich, ref DTWAIN_FLOAT Value)
        => _DTWAIN_FrameGetValue(Frame, nWhich, ref Value);

        public  int DTWAIN_FrameGetValueString(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Value)
        => _DTWAIN_FrameGetValueString(Frame, nWhich, Value);

        public  int DTWAIN_FrameGetValueStringA(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Value)
        => _DTWAIN_FrameGetValueStringA(Frame, nWhich, Value);

        public  int DTWAIN_FrameGetValueStringW(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Value)
        => _DTWAIN_FrameGetValueStringW(Frame, nWhich, Value);

        public  int DTWAIN_FrameIsValid(DTWAIN_FRAME Frame)
        => _DTWAIN_FrameIsValid(Frame);

        public  int DTWAIN_FrameSetAll(DTWAIN_FRAME Frame, DTWAIN_FLOAT Left, DTWAIN_FLOAT Top, DTWAIN_FLOAT Right, DTWAIN_FLOAT Bottom)
        => _DTWAIN_FrameSetAll(Frame, Left, Top, Right, Bottom);

        public  int DTWAIN_FrameSetAllString(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPTStr)] string Left, [MarshalAs(UnmanagedType.LPTStr)] string Top, [MarshalAs(UnmanagedType.LPTStr)] string Right, [MarshalAs(UnmanagedType.LPTStr)] string Bottom)
        => _DTWAIN_FrameSetAllString(Frame, Left, Top, Right, Bottom);

        public  int DTWAIN_FrameSetAllStringA(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPStr)] string Left, [MarshalAs(UnmanagedType.LPStr)] string Top, [MarshalAs(UnmanagedType.LPStr)] string Right, [MarshalAs(UnmanagedType.LPStr)] string Bottom)
        => _DTWAIN_FrameSetAllStringA(Frame, Left, Top, Right, Bottom);

        public  int DTWAIN_FrameSetAllStringW(DTWAIN_FRAME Frame, [MarshalAs(UnmanagedType.LPWStr)] string Left, [MarshalAs(UnmanagedType.LPWStr)] string Top, [MarshalAs(UnmanagedType.LPWStr)] string Right, [MarshalAs(UnmanagedType.LPWStr)] string Bottom)
        => _DTWAIN_FrameSetAllStringW(Frame, Left, Top, Right, Bottom);

        public  int DTWAIN_FrameSetValue(DTWAIN_FRAME Frame, int nWhich, DTWAIN_FLOAT Value)
        => _DTWAIN_FrameSetValue(Frame, nWhich, Value);

        public  int DTWAIN_FrameSetValueString(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] string Value)
        => _DTWAIN_FrameSetValueString(Frame, nWhich, Value);

        public  int DTWAIN_FrameSetValueStringA(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPStr)] string Value)
        => _DTWAIN_FrameSetValueStringA(Frame, nWhich, Value);

        public  int DTWAIN_FrameSetValueStringW(DTWAIN_FRAME Frame, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] string Value)
        => _DTWAIN_FrameSetValueStringW(Frame, nWhich, Value);

        public  int DTWAIN_FreeExtImageInfo(DTWAIN_SOURCE Source)
        => _DTWAIN_FreeExtImageInfo(Source);

        public  int DTWAIN_FreeMemory(HANDLE h)
        => _DTWAIN_FreeMemory(h);

        public  int DTWAIN_FreeMemoryEx(HANDLE h)
        => _DTWAIN_FreeMemoryEx(h);

        public  int DTWAIN_GetAPIHandleStatus(DTWAIN_HANDLE pHandle)
        => _DTWAIN_GetAPIHandleStatus(pHandle);

        public  int DTWAIN_GetAcquireArea(DTWAIN_SOURCE Source, int lGetType, ref DTWAIN_ARRAY FloatEnum)
        => _DTWAIN_GetAcquireArea(Source, lGetType, ref FloatEnum);

        public  int DTWAIN_GetAcquireArea2(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT left, ref DTWAIN_FLOAT top, ref DTWAIN_FLOAT right, ref DTWAIN_FLOAT bottom, ref int lpUnit)
        => _DTWAIN_GetAcquireArea2(Source, ref left, ref top, ref right, ref bottom, ref lpUnit);

        public  int DTWAIN_GetAcquireArea2String(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder bottom, ref int Unit)
        => _DTWAIN_GetAcquireArea2String(Source, left, top, right, bottom, ref Unit);

        public  int DTWAIN_GetAcquireArea2StringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder bottom, ref int Unit)
        => _DTWAIN_GetAcquireArea2StringA(Source, left, top, right, bottom, ref Unit);

        public  int DTWAIN_GetAcquireArea2StringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder left, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder top, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder right, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder bottom, ref int Unit)
        => _DTWAIN_GetAcquireArea2StringW(Source, left, top, right, bottom, ref Unit);

        public  DTWAIN_ARRAY DTWAIN_GetAcquireAreaEx(DTWAIN_SOURCE Source, int lGetType)
        => _DTWAIN_GetAcquireAreaEx(Source, lGetType);

        public  int DTWAIN_GetAcquireMetrics(DTWAIN_SOURCE source, ref int ImageCount, ref int SheetCount)
        => _DTWAIN_GetAcquireMetrics(source, ref ImageCount, ref SheetCount);

        public  HANDLE DTWAIN_GetAcquireStripBuffer(DTWAIN_SOURCE Source)
        => _DTWAIN_GetAcquireStripBuffer(Source);

        public  int DTWAIN_GetAcquireStripData(DTWAIN_SOURCE Source, ref int lpCompression, ref DWORD lpBytesPerRow, ref DWORD lpColumns, ref DWORD lpRows, ref DWORD XOffset, ref DWORD YOffset, ref DWORD lpBytesWritten)
        => _DTWAIN_GetAcquireStripData(Source, ref lpCompression, ref lpBytesPerRow, ref lpColumns, ref lpRows, ref XOffset, ref YOffset, ref lpBytesWritten);

        public  int DTWAIN_GetAcquireStripSizes(DTWAIN_SOURCE Source, ref DWORD lpMin, ref DWORD lpMax, ref DWORD lpPreferred)
        => _DTWAIN_GetAcquireStripSizes(Source, ref lpMin, ref lpMax, ref lpPreferred);

        public  HANDLE DTWAIN_GetAcquiredImage(DTWAIN_ARRAY aAcq, int nWhichAcq, int nWhichDib)
        => _DTWAIN_GetAcquiredImage(aAcq, nWhichAcq, nWhichDib);

        public  DTWAIN_ARRAY DTWAIN_GetAcquiredImageArray(DTWAIN_ARRAY aAcq, int nWhichAcq)
        => _DTWAIN_GetAcquiredImageArray(aAcq, nWhichAcq);

        public  int DTWAIN_GetActiveDSMPath([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetActiveDSMPath(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetActiveDSMPathA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetActiveDSMPathA(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetActiveDSMPathW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetActiveDSMPathW(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetActiveDSMVersionInfo([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szDLLInfo, int nMaxLen)
        => _DTWAIN_GetActiveDSMVersionInfo(szDLLInfo, nMaxLen);

        public  int DTWAIN_GetActiveDSMVersionInfoA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetActiveDSMVersionInfoA(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetActiveDSMVersionInfoW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetActiveDSMVersionInfoW(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetAlarmVolume(DTWAIN_SOURCE Source, ref int lpVolume)
        => _DTWAIN_GetAlarmVolume(Source, ref lpVolume);

        public  DTWAIN_ARRAY DTWAIN_GetAllSourceDibs(DTWAIN_SOURCE Source)
        => _DTWAIN_GetAllSourceDibs(Source);

        public  int DTWAIN_GetAppInfo([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szVerStr, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szManu, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProdFam, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProdName)
        => _DTWAIN_GetAppInfo(szVerStr, szManu, szProdFam, szProdName);

        public  int DTWAIN_GetAppInfoA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szVerStr, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szManu, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProdFam, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProdName)
        => _DTWAIN_GetAppInfoA(szVerStr, szManu, szProdFam, szProdName);

        public  int DTWAIN_GetAppInfoW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szVerStr, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szManu, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProdFam, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProdName)
        => _DTWAIN_GetAppInfoW(szVerStr, szManu, szProdFam, szProdName);

        public  int DTWAIN_GetAuthor(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szAuthor)
        => _DTWAIN_GetAuthor(Source, szAuthor);

        public  int DTWAIN_GetAuthorA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szAuthor)
        => _DTWAIN_GetAuthorA(Source, szAuthor);

        public  int DTWAIN_GetAuthorW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szAuthor)
        => _DTWAIN_GetAuthorW(Source, szAuthor);

        public  int DTWAIN_GetBatteryMinutes(DTWAIN_SOURCE Source, ref int lpMinutes)
        => _DTWAIN_GetBatteryMinutes(Source, ref lpMinutes);

        public  int DTWAIN_GetBatteryPercent(DTWAIN_SOURCE Source, ref int lpPercent)
        => _DTWAIN_GetBatteryPercent(Source, ref lpPercent);

        public  int DTWAIN_GetBitDepth(DTWAIN_SOURCE Source, ref int BitDepth, int bCurrent)
        => _DTWAIN_GetBitDepth(Source, ref BitDepth, bCurrent);

        public  int DTWAIN_GetBlankPageAutoDetection(DTWAIN_SOURCE Source)
        => _DTWAIN_GetBlankPageAutoDetection(Source);

        public  int DTWAIN_GetBrightness(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Brightness)
        => _DTWAIN_GetBrightness(Source, ref Brightness);

        public  int DTWAIN_GetBrightnessString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Brightness)
        => _DTWAIN_GetBrightnessString(Source, Brightness);

        public  int DTWAIN_GetBrightnessStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Contrast)
        => _DTWAIN_GetBrightnessStringA(Source, Contrast);

        public  int DTWAIN_GetBrightnessStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Contrast)
        => _DTWAIN_GetBrightnessStringW(Source, Contrast);

        public  HANDLE DTWAIN_GetBufferedTransferInfo(DTWAIN_SOURCE Source, ref DWORD Compression, ref DWORD BytesPerRow, ref DWORD Columns, ref DWORD Rows, ref DWORD XOffset, ref DWORD YOffset, ref DWORD Flags, ref DWORD BytesWritten, ref DWORD MemoryLength)
        => _DTWAIN_GetBufferedTransferInfo(Source, ref Compression, ref BytesPerRow, ref Columns, ref Rows, ref XOffset, ref YOffset, ref Flags, ref BytesWritten, ref MemoryLength);

        public  DTwainCallback DTWAIN_GetCallback()
        => _DTWAIN_GetCallback();

        public  DTwainCallback64 DTWAIN_GetCallback64()
        => _DTWAIN_GetCallback64();

        public  int DTWAIN_GetCapArrayType(DTWAIN_SOURCE Source, int nCap)
        => _DTWAIN_GetCapArrayType(Source, nCap);

        public  int DTWAIN_GetCapContainer(DTWAIN_SOURCE Source, int nCap, int lCapType)
        => _DTWAIN_GetCapContainer(Source, nCap, lCapType);

        public  int DTWAIN_GetCapContainerEx(int nCap, int bSetContainer, ref DTWAIN_ARRAY ConTypes)
        => _DTWAIN_GetCapContainerEx(nCap, bSetContainer, ref ConTypes);

        public  DTWAIN_ARRAY DTWAIN_GetCapContainerEx2(int nCap, int bSetContainer)
        => _DTWAIN_GetCapContainerEx2(nCap, bSetContainer);

        public  int DTWAIN_GetCapDataType(DTWAIN_SOURCE Source, int nCap)
        => _DTWAIN_GetCapDataType(Source, nCap);

        public  int DTWAIN_GetCapFromName([MarshalAs(UnmanagedType.LPTStr)] string szName)
        => _DTWAIN_GetCapFromName(szName);

        public  int DTWAIN_GetCapFromNameA([MarshalAs(UnmanagedType.LPStr)] string szName)
        => _DTWAIN_GetCapFromNameA(szName);

        public  int DTWAIN_GetCapFromNameW([MarshalAs(UnmanagedType.LPWStr)] string szName)
        => _DTWAIN_GetCapFromNameW(szName);

        public  int DTWAIN_GetCapOperations(DTWAIN_SOURCE Source, int lCapability, ref int lpOps)
        => _DTWAIN_GetCapOperations(Source, lCapability, ref lpOps);

        public  int DTWAIN_GetCapValues(DTWAIN_SOURCE Source, int lCap, int lGetType, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_GetCapValues(Source, lCap, lGetType, ref pArray);

        public  int DTWAIN_GetCapValuesEx(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_GetCapValuesEx(Source, lCap, lGetType, lContainerType, ref pArray);

        public  int DTWAIN_GetCapValuesEx2(DTWAIN_SOURCE Source, int lCap, int lGetType, int lContainerType, int nDataType, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_GetCapValuesEx2(Source, lCap, lGetType, lContainerType, nDataType, ref pArray);

        public  int DTWAIN_GetCaption(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Caption)
        => _DTWAIN_GetCaption(Source, Caption);

        public  int DTWAIN_GetCaptionA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Caption)
        => _DTWAIN_GetCaptionA(Source, Caption);

        public  int DTWAIN_GetCaptionW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Caption)
        => _DTWAIN_GetCaptionW(Source, Caption);

        public  int DTWAIN_GetCompressionSize(DTWAIN_SOURCE Source, ref int lBytes)
        => _DTWAIN_GetCompressionSize(Source, ref lBytes);

        public  int DTWAIN_GetCompressionType(DTWAIN_SOURCE Source, ref int lpCompression, int bCurrent)
        => _DTWAIN_GetCompressionType(Source, ref lpCompression, bCurrent);

        public  int DTWAIN_GetConditionCodeString(int lError, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetConditionCodeString(lError, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetConditionCodeStringA(int lError, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetConditionCodeStringA(lError, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetConditionCodeStringW(int lError, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetConditionCodeStringW(lError, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetContrast(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Contrast)
        => _DTWAIN_GetContrast(Source, ref Contrast);

        public  int DTWAIN_GetContrastString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Contrast)
        => _DTWAIN_GetContrastString(Source, Contrast);

        public  int DTWAIN_GetContrastStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Contrast)
        => _DTWAIN_GetContrastStringA(Source, Contrast);

        public  int DTWAIN_GetContrastStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Contrast)
        => _DTWAIN_GetContrastStringW(Source, Contrast);

        public  int DTWAIN_GetCountry()
        => _DTWAIN_GetCountry();

        public  HANDLE DTWAIN_GetCurrentAcquiredImage(DTWAIN_SOURCE Source)
        => _DTWAIN_GetCurrentAcquiredImage(Source);

        public  int DTWAIN_GetCurrentFileName(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szName, int MaxLen)
        => _DTWAIN_GetCurrentFileName(Source, szName, MaxLen);

        public  int DTWAIN_GetCurrentFileNameA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szName, int MaxLen)
        => _DTWAIN_GetCurrentFileNameA(Source, szName, MaxLen);

        public  int DTWAIN_GetCurrentFileNameW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szName, int MaxLen)
        => _DTWAIN_GetCurrentFileNameW(Source, szName, MaxLen);

        public  int DTWAIN_GetCurrentPageNum(DTWAIN_SOURCE Source)
        => _DTWAIN_GetCurrentPageNum(Source);

        public  int DTWAIN_GetCurrentRetryCount(DTWAIN_SOURCE Source)
        => _DTWAIN_GetCurrentRetryCount(Source);

        public  int DTWAIN_GetCurrentTwainTriplet([In, Out] TW_IDENTITY pAppID, [In, Out] TW_IDENTITY pSourceID, ref int lpDG, ref int lpDAT, ref int lpMsg, ref long lpMemRef)
        => _DTWAIN_GetCurrentTwainTriplet(pAppID, pSourceID, ref lpDG, ref lpDAT, ref lpMsg, ref lpMemRef);

        public  HANDLE DTWAIN_GetCustomDSData(DTWAIN_SOURCE Source, byte[] Data, uint dSize, ref DWORD pActualSize, int nFlags)
        => _DTWAIN_GetCustomDSData(Source, Data, dSize, ref pActualSize, nFlags);

        public  int DTWAIN_GetDSMFullName(int DSMType, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szDLLName, int nMaxLen, ref int pWhichSearch)
        => _DTWAIN_GetDSMFullName(DSMType, szDLLName, nMaxLen, ref pWhichSearch);

        public  int DTWAIN_GetDSMFullNameA(int DSMType, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szDLLName, int nMaxLen, ref int pWhichSearch)
        => _DTWAIN_GetDSMFullNameA(DSMType, szDLLName, nMaxLen, ref pWhichSearch);

        public  int DTWAIN_GetDSMFullNameW(int DSMType, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szDLLName, int nMaxLen, ref int pWhichSearch)
        => _DTWAIN_GetDSMFullNameW(DSMType, szDLLName, nMaxLen, ref pWhichSearch);

        public  int DTWAIN_GetDSMSearchOrder()
        => _DTWAIN_GetDSMSearchOrder();

        public  DTWAIN_HANDLE DTWAIN_GetDTWAINHandle()
        => _DTWAIN_GetDTWAINHandle();

        public  int DTWAIN_GetDeviceEvent(DTWAIN_SOURCE Source, ref int lpEvent)
        => _DTWAIN_GetDeviceEvent(Source, ref lpEvent);

        public  int DTWAIN_GetDeviceEventEx(DTWAIN_SOURCE Source, ref int lpEvent, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_GetDeviceEventEx(Source, ref lpEvent, ref pArray);

        public  int DTWAIN_GetDeviceEventInfo(DTWAIN_SOURCE Source, int nWhichInfo, System.IntPtr pValue)
        => _DTWAIN_GetDeviceEventInfo(Source, nWhichInfo, pValue);

        public  int DTWAIN_GetDeviceNotifications(DTWAIN_SOURCE Source, ref int DevEvents)
        => _DTWAIN_GetDeviceNotifications(Source, ref DevEvents);

        public  int DTWAIN_GetDeviceTimeDate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szTimeDate)
        => _DTWAIN_GetDeviceTimeDate(Source, szTimeDate);

        public  int DTWAIN_GetDeviceTimeDateA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szTimeDate)
        => _DTWAIN_GetDeviceTimeDateA(Source, szTimeDate);

        public  int DTWAIN_GetDeviceTimeDateW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szTimeDate)
        => _DTWAIN_GetDeviceTimeDateW(Source, szTimeDate);

        public  int DTWAIN_GetDoubleFeedDetectLength(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Value, int bCurrent)
        => _DTWAIN_GetDoubleFeedDetectLength(Source, ref Value, bCurrent);

        public  int DTWAIN_GetDoubleFeedDetectValues(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_GetDoubleFeedDetectValues(Source, ref pArray);

        public  int DTWAIN_GetDuplexType(DTWAIN_SOURCE Source, ref int lpDupType)
        => _DTWAIN_GetDuplexType(Source, ref lpDupType);

        public  int DTWAIN_GetErrorBuffer(ref DTWAIN_ARRAY ArrayBuffer)
        => _DTWAIN_GetErrorBuffer(ref ArrayBuffer);

        public  int DTWAIN_GetErrorBufferThreshold()
        => _DTWAIN_GetErrorBufferThreshold();

        public  DTwainErrorProc DTWAIN_GetErrorCallback()
        => _DTWAIN_GetErrorCallback();

        public  DTwainErrorProc64 DTWAIN_GetErrorCallback64()
        => _DTWAIN_GetErrorCallback64();

        public  int DTWAIN_GetErrorString(int lError, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetErrorString(lError, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetErrorStringA(int lError, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nLength)
        => _DTWAIN_GetErrorStringA(lError, lpszBuffer, nLength);

        public  int DTWAIN_GetErrorStringW(int lError, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nLength)
        => _DTWAIN_GetErrorStringW(lError, lpszBuffer, nLength);

        public  int DTWAIN_GetExtCapFromName([MarshalAs(UnmanagedType.LPTStr)] string szName)
        => _DTWAIN_GetExtCapFromName(szName);

        public  int DTWAIN_GetExtCapFromNameA([MarshalAs(UnmanagedType.LPStr)] string szName)
        => _DTWAIN_GetExtCapFromNameA(szName);

        public  int DTWAIN_GetExtCapFromNameW([MarshalAs(UnmanagedType.LPWStr)] string szName)
        => _DTWAIN_GetExtCapFromNameW(szName);

        public  int DTWAIN_GetExtImageInfo(DTWAIN_SOURCE Source)
        => _DTWAIN_GetExtImageInfo(Source);

        public  int DTWAIN_GetExtImageInfoData(DTWAIN_SOURCE Source, int nWhich, ref DTWAIN_ARRAY Data)
        => _DTWAIN_GetExtImageInfoData(Source, nWhich, ref Data);

        public  DTWAIN_ARRAY DTWAIN_GetExtImageInfoDataEx(DTWAIN_SOURCE Source, int nWhich)
        => _DTWAIN_GetExtImageInfoDataEx(Source, nWhich);

        public  int DTWAIN_GetExtImageInfoItem(DTWAIN_SOURCE Source, int nWhich, ref int InfoID, ref int NumItems, ref int Type)
        => _DTWAIN_GetExtImageInfoItem(Source, nWhich, ref InfoID, ref NumItems, ref Type);

        public  int DTWAIN_GetExtImageInfoItemEx(DTWAIN_SOURCE Source, int nWhich, ref int InfoID, ref int NumItems, ref int Type, ref int ReturnCode)
        => _DTWAIN_GetExtImageInfoItemEx(Source, nWhich, ref InfoID, ref NumItems, ref Type, ref ReturnCode);

        public  int DTWAIN_GetExtNameFromCap(int nValue, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szValue, int nMaxLen)
        => _DTWAIN_GetExtNameFromCap(nValue, szValue, nMaxLen);

        public  int DTWAIN_GetExtNameFromCapA(int nValue, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szValue, int nLength)
        => _DTWAIN_GetExtNameFromCapA(nValue, szValue, nLength);

        public  int DTWAIN_GetExtNameFromCapW(int nValue, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szValue, int nLength)
        => _DTWAIN_GetExtNameFromCapW(nValue, szValue, nLength);

        public  int DTWAIN_GetFeederAlignment(DTWAIN_SOURCE Source, ref int lpAlignment)
        => _DTWAIN_GetFeederAlignment(Source, ref lpAlignment);

        public  int DTWAIN_GetFeederFuncs(DTWAIN_SOURCE Source)
        => _DTWAIN_GetFeederFuncs(Source);

        public  int DTWAIN_GetFeederOrder(DTWAIN_SOURCE Source, ref int lpOrder)
        => _DTWAIN_GetFeederOrder(Source, ref lpOrder);

        public  int DTWAIN_GetFeederWaitTime(DTWAIN_SOURCE Source)
        => _DTWAIN_GetFeederWaitTime(Source);

        public  int DTWAIN_GetFileCompressionType(DTWAIN_SOURCE Source)
        => _DTWAIN_GetFileCompressionType(Source);

        public  int DTWAIN_GetFileSavePageCount(DTWAIN_SOURCE Source)
        => _DTWAIN_GetFileSavePageCount(Source);

        public  int DTWAIN_GetFileTypeExtensions(int nType, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszName, int nLength)
        => _DTWAIN_GetFileTypeExtensions(nType, lpszName, nLength);

        public  int DTWAIN_GetFileTypeExtensionsA(int nType, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszName, int nLength)
        => _DTWAIN_GetFileTypeExtensionsA(nType, lpszName, nLength);

        public  int DTWAIN_GetFileTypeExtensionsW(int nType, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszName, int nLength)
        => _DTWAIN_GetFileTypeExtensionsW(nType, lpszName, nLength);

        public  int DTWAIN_GetFileTypeName(int nType, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszName, int nLength)
        => _DTWAIN_GetFileTypeName(nType, lpszName, nLength);

        public  int DTWAIN_GetFileTypeNameA(int nType, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszName, int nLength)
        => _DTWAIN_GetFileTypeNameA(nType, lpszName, nLength);

        public  int DTWAIN_GetFileTypeNameW(int nType, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszName, int nLength)
        => _DTWAIN_GetFileTypeNameW(nType, lpszName, nLength);

        public  int DTWAIN_GetHalftone(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpHalftone, int GetType)
        => _DTWAIN_GetHalftone(Source, lpHalftone, GetType);

        public  int DTWAIN_GetHalftoneA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpHalftone, int GetType)
        => _DTWAIN_GetHalftoneA(Source, lpHalftone, GetType);

        public  int DTWAIN_GetHalftoneW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpHalftone, int GetType)
        => _DTWAIN_GetHalftoneW(Source, lpHalftone, GetType);

        public  int DTWAIN_GetHighlight(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Highlight)
        => _DTWAIN_GetHighlight(Source, ref Highlight);

        public  int DTWAIN_GetHighlightString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Highlight)
        => _DTWAIN_GetHighlightString(Source, Highlight);

        public  int DTWAIN_GetHighlightStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Highlight)
        => _DTWAIN_GetHighlightStringA(Source, Highlight);

        public  int DTWAIN_GetHighlightStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Highlight)
        => _DTWAIN_GetHighlightStringW(Source, Highlight);

        public  int DTWAIN_GetImageInfo(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT lpXResolution, ref DTWAIN_FLOAT lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression)
        => _DTWAIN_GetImageInfo(Source, ref lpXResolution, ref lpYResolution, ref lpWidth, ref lpLength, ref lpNumSamples, ref lpBitsPerSample, ref lpBitsPerPixel, ref lpPlanar, ref lpPixelType, ref lpCompression);

        public  int DTWAIN_GetImageInfoString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpXResolution, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression)
        => _DTWAIN_GetImageInfoString(Source, lpXResolution, lpYResolution, ref lpWidth, ref lpLength, ref lpNumSamples, ref lpBitsPerSample, ref lpBitsPerPixel, ref lpPlanar, ref lpPixelType, ref lpCompression);

        public  int DTWAIN_GetImageInfoStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpXResolution, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression)
        => _DTWAIN_GetImageInfoStringA(Source, lpXResolution, lpYResolution, ref lpWidth, ref lpLength, ref lpNumSamples, ref lpBitsPerSample, ref lpBitsPerPixel, ref lpPlanar, ref lpPixelType, ref lpCompression);

        public  int DTWAIN_GetImageInfoStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpXResolution, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpYResolution, ref int lpWidth, ref int lpLength, ref int lpNumSamples, ref DTWAIN_ARRAY lpBitsPerSample, ref int lpBitsPerPixel, ref int lpPlanar, ref int lpPixelType, ref int lpCompression)
        => _DTWAIN_GetImageInfoStringW(Source, lpXResolution, lpYResolution, ref lpWidth, ref lpLength, ref lpNumSamples, ref lpBitsPerSample, ref lpBitsPerPixel, ref lpPlanar, ref lpPixelType, ref lpCompression);

        public  int DTWAIN_GetJobControl(DTWAIN_SOURCE Source, ref int pJobControl, int bCurrent)
        => _DTWAIN_GetJobControl(Source, ref pJobControl, bCurrent);

        public  int DTWAIN_GetJpegValues(DTWAIN_SOURCE Source, ref int pQuality, ref int Progressive)
        => _DTWAIN_GetJpegValues(Source, ref pQuality, ref Progressive);

        public  int DTWAIN_GetJpegXRValues(DTWAIN_SOURCE Source, ref int pQuality, ref int Progressive)
        => _DTWAIN_GetJpegXRValues(Source, ref pQuality, ref Progressive);

        public  int DTWAIN_GetLanguage()
        => _DTWAIN_GetLanguage();

        public  int DTWAIN_GetLastError()
        => _DTWAIN_GetLastError();

        public  int DTWAIN_GetLibraryPath([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetLibraryPath(lpszVer, nLength);

        public  int DTWAIN_GetLibraryPathA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetLibraryPathA(lpszVer, nLength);

        public  int DTWAIN_GetLibraryPathW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetLibraryPathW(lpszVer, nLength);

        public  int DTWAIN_GetLightPath(DTWAIN_SOURCE Source, ref int lpLightPath)
        => _DTWAIN_GetLightPath(Source, ref lpLightPath);

        public  int DTWAIN_GetLightSource(DTWAIN_SOURCE Source, ref int LightSource)
        => _DTWAIN_GetLightSource(Source, ref LightSource);

        public  int DTWAIN_GetLightSources(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY LightSources)
        => _DTWAIN_GetLightSources(Source, ref LightSources);

        public  DTWAIN_ARRAY DTWAIN_GetLightSourcesEx(DTWAIN_SOURCE Source)
        => _DTWAIN_GetLightSourcesEx(Source);

        public  DTwainLoggerProc DTWAIN_GetLoggerCallback()
        => _DTWAIN_GetLoggerCallback();

        public  DTwainLoggerProcA DTWAIN_GetLoggerCallbackA()
        => _DTWAIN_GetLoggerCallbackA();

        public  DTwainLoggerProcW DTWAIN_GetLoggerCallbackW()
        => _DTWAIN_GetLoggerCallbackW();

        public  int DTWAIN_GetManualDuplexCount(DTWAIN_SOURCE Source, ref int pSide1, ref int pSide2)
        => _DTWAIN_GetManualDuplexCount(Source, ref pSide1, ref pSide2);

        public  int DTWAIN_GetMaxAcquisitions(DTWAIN_SOURCE Source)
        => _DTWAIN_GetMaxAcquisitions(Source);

        public  int DTWAIN_GetMaxBuffers(DTWAIN_SOURCE Source, ref int pMaxBuf)
        => _DTWAIN_GetMaxBuffers(Source, ref pMaxBuf);

        public  int DTWAIN_GetMaxPagesToAcquire(DTWAIN_SOURCE Source)
        => _DTWAIN_GetMaxPagesToAcquire(Source);

        public  int DTWAIN_GetMaxRetryAttempts(DTWAIN_SOURCE Source)
        => _DTWAIN_GetMaxRetryAttempts(Source);

        public  int DTWAIN_GetNameFromCap(int nCapValue, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szValue, int nMaxLen)
        => _DTWAIN_GetNameFromCap(nCapValue, szValue, nMaxLen);

        public  int DTWAIN_GetNameFromCapA(int nCapValue, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szValue, int nLength)
        => _DTWAIN_GetNameFromCapA(nCapValue, szValue, nLength);

        public  int DTWAIN_GetNameFromCapW(int nCapValue, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szValue, int nLength)
        => _DTWAIN_GetNameFromCapW(nCapValue, szValue, nLength);

        public  int DTWAIN_GetNoiseFilter(DTWAIN_SOURCE Source, ref int lpNoiseFilter)
        => _DTWAIN_GetNoiseFilter(Source, ref lpNoiseFilter);

        public  int DTWAIN_GetNumAcquiredImages(DTWAIN_ARRAY aAcq, int nWhich)
        => _DTWAIN_GetNumAcquiredImages(aAcq, nWhich);

        public  int DTWAIN_GetNumAcquisitions(DTWAIN_ARRAY aAcq)
        => _DTWAIN_GetNumAcquisitions(aAcq);

        public  int DTWAIN_GetOCRCapValues(DTWAIN_OCRENGINE Engine, int OCRCapValue, int GetType, ref DTWAIN_ARRAY CapValues)
        => _DTWAIN_GetOCRCapValues(Engine, OCRCapValue, GetType, ref CapValues);

        public  int DTWAIN_GetOCRErrorString(DTWAIN_OCRENGINE Engine, int lError, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetOCRErrorString(Engine, lError, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetOCRErrorStringA(DTWAIN_OCRENGINE Engine, int lError, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetOCRErrorStringA(Engine, lError, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetOCRErrorStringW(DTWAIN_OCRENGINE Engine, int lError, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetOCRErrorStringW(Engine, lError, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetOCRLastError(DTWAIN_OCRENGINE Engine)
        => _DTWAIN_GetOCRLastError(Engine);

        public  int DTWAIN_GetOCRMajorMinorVersion(DTWAIN_OCRENGINE Engine, ref int lpMajor, ref int lpMinor)
        => _DTWAIN_GetOCRMajorMinorVersion(Engine, ref lpMajor, ref lpMinor);

        public  int DTWAIN_GetOCRManufacturer(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szManufacturer, int nMaxLen)
        => _DTWAIN_GetOCRManufacturer(Engine, szManufacturer, nMaxLen);

        public  int DTWAIN_GetOCRManufacturerA(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szManufacturer, int nLength)
        => _DTWAIN_GetOCRManufacturerA(Engine, szManufacturer, nLength);

        public  int DTWAIN_GetOCRManufacturerW(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szManufacturer, int nLength)
        => _DTWAIN_GetOCRManufacturerW(Engine, szManufacturer, nLength);

        public  int DTWAIN_GetOCRProductFamily(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProductFamily, int nMaxLen)
        => _DTWAIN_GetOCRProductFamily(Engine, szProductFamily, nMaxLen);

        public  int DTWAIN_GetOCRProductFamilyA(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProductFamily, int nLength)
        => _DTWAIN_GetOCRProductFamilyA(Engine, szProductFamily, nLength);

        public  int DTWAIN_GetOCRProductFamilyW(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProductFamily, int nLength)
        => _DTWAIN_GetOCRProductFamilyW(Engine, szProductFamily, nLength);

        public  int DTWAIN_GetOCRProductName(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProductName, int nMaxLen)
        => _DTWAIN_GetOCRProductName(Engine, szProductName, nMaxLen);

        public  int DTWAIN_GetOCRProductNameA(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProductName, int nLength)
        => _DTWAIN_GetOCRProductNameA(Engine, szProductName, nLength);

        public  int DTWAIN_GetOCRProductNameW(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProductName, int nLength)
        => _DTWAIN_GetOCRProductNameW(Engine, szProductName, nLength);

        public  HANDLE DTWAIN_GetOCRText(DTWAIN_OCRENGINE Engine, int nPageNo, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Data, int dSize, ref int pActualSize, int nFlags)
        => _DTWAIN_GetOCRText(Engine, nPageNo, Data, dSize, ref pActualSize, nFlags);

        public  HANDLE DTWAIN_GetOCRTextA(DTWAIN_OCRENGINE Engine, int nPageNo, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Data, int dSize, ref int pActualSize, int nFlags)
        => _DTWAIN_GetOCRTextA(Engine, nPageNo, Data, dSize, ref pActualSize, nFlags);

        public  int DTWAIN_GetOCRTextInfoFloat(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nCharPos, int nWhichItem, ref DTWAIN_FLOAT pInfo)
        => _DTWAIN_GetOCRTextInfoFloat(OCRTextInfo, nCharPos, nWhichItem, ref pInfo);

        public  int DTWAIN_GetOCRTextInfoFloatEx(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nWhichItem, ref DTWAIN_FLOAT pInfo, int bufSize)
        => _DTWAIN_GetOCRTextInfoFloatEx(OCRTextInfo, nWhichItem, ref pInfo, bufSize);

        public  DTWAIN_OCRTEXTINFOHANDLE DTWAIN_GetOCRTextInfoHandle(DTWAIN_OCRENGINE Engine, int nPageNo)
        => _DTWAIN_GetOCRTextInfoHandle(Engine, nPageNo);

        public  int DTWAIN_GetOCRTextInfoLong(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nCharPos, int nWhichItem, ref int pInfo)
        => _DTWAIN_GetOCRTextInfoLong(OCRTextInfo, nCharPos, nWhichItem, ref pInfo);

        public  int DTWAIN_GetOCRTextInfoLongEx(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, int nWhichItem, ref int pInfo, int bufSize)
        => _DTWAIN_GetOCRTextInfoLongEx(OCRTextInfo, nWhichItem, ref pInfo, bufSize);

        public  HANDLE DTWAIN_GetOCRTextW(DTWAIN_OCRENGINE Engine, int nPageNo, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Data, int dSize, ref int pActualSize, int nFlags)
        => _DTWAIN_GetOCRTextW(Engine, nPageNo, Data, dSize, ref pActualSize, nFlags);

        public  int DTWAIN_GetOCRVersionInfo(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder buffer, int maxBufSize)
        => _DTWAIN_GetOCRVersionInfo(Engine, buffer, maxBufSize);

        public  int DTWAIN_GetOCRVersionInfoA(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder buffer, int nLength)
        => _DTWAIN_GetOCRVersionInfoA(Engine, buffer, nLength);

        public  int DTWAIN_GetOCRVersionInfoW(DTWAIN_OCRENGINE Engine, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder buffer, int nLength)
        => _DTWAIN_GetOCRVersionInfoW(Engine, buffer, nLength);

        public  int DTWAIN_GetOrientation(DTWAIN_SOURCE Source, ref int lpOrient, int bCurrent)
        => _DTWAIN_GetOrientation(Source, ref lpOrient, bCurrent);

        public  int DTWAIN_GetOverscan(DTWAIN_SOURCE Source, ref int lpOverscan, int bCurrent)
        => _DTWAIN_GetOverscan(Source, ref lpOverscan, bCurrent);

        public  int DTWAIN_GetPDFTextElementFloat(DTWAIN_PDFTEXTELEMENT TextElement, ref DTWAIN_FLOAT val1, ref DTWAIN_FLOAT val2, int Flags)
        => _DTWAIN_GetPDFTextElementFloat(TextElement, ref val1, ref val2, Flags);

        public  int DTWAIN_GetPDFTextElementLong(DTWAIN_PDFTEXTELEMENT TextElement, ref int val1, ref int val2, int Flags)
        => _DTWAIN_GetPDFTextElementLong(TextElement, ref val1, ref val2, Flags);

        public  int DTWAIN_GetPDFTextElementString(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szData, int maxLen, int Flags)
        => _DTWAIN_GetPDFTextElementString(TextElement, szData, maxLen, Flags);

        public  int DTWAIN_GetPDFTextElementStringA(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szData, int maxLen, int Flags)
        => _DTWAIN_GetPDFTextElementStringA(TextElement, szData, maxLen, Flags);

        public  int DTWAIN_GetPDFTextElementStringW(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szData, int maxLen, int Flags)
        => _DTWAIN_GetPDFTextElementStringW(TextElement, szData, maxLen, Flags);

        public  int DTWAIN_GetPDFType1FontName(int FontVal, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szFont, int nChars)
        => _DTWAIN_GetPDFType1FontName(FontVal, szFont, nChars);

        public  int DTWAIN_GetPDFType1FontNameA(int FontVal, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szFont, int nChars)
        => _DTWAIN_GetPDFType1FontNameA(FontVal, szFont, nChars);

        public  int DTWAIN_GetPDFType1FontNameW(int FontVal, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szFont, int nChars)
        => _DTWAIN_GetPDFType1FontNameW(FontVal, szFont, nChars);

        public  int DTWAIN_GetPaperSize(DTWAIN_SOURCE Source, ref int lpPaperSize, int bCurrent)
        => _DTWAIN_GetPaperSize(Source, ref lpPaperSize, bCurrent);

        public  int DTWAIN_GetPaperSizeName(int paperNumber, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder outName, int nSize)
        => _DTWAIN_GetPaperSizeName(paperNumber, outName, nSize);

        public  int DTWAIN_GetPaperSizeNameA(int paperNumber, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder outName, int nSize)
        => _DTWAIN_GetPaperSizeNameA(paperNumber, outName, nSize);

        public  int DTWAIN_GetPaperSizeNameW(int paperNumber, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder outName, int nSize)
        => _DTWAIN_GetPaperSizeNameW(paperNumber, outName, nSize);

        public  int DTWAIN_GetPatchMaxPriorities(DTWAIN_SOURCE Source, ref int pMaxPriorities, int bCurrent)
        => _DTWAIN_GetPatchMaxPriorities(Source, ref pMaxPriorities, bCurrent);

        public  int DTWAIN_GetPatchMaxRetries(DTWAIN_SOURCE Source, ref int pMaxRetries, int bCurrent)
        => _DTWAIN_GetPatchMaxRetries(Source, ref pMaxRetries, bCurrent);

        public  int DTWAIN_GetPatchPriorities(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY SearchPriorities)
        => _DTWAIN_GetPatchPriorities(Source, ref SearchPriorities);

        public  int DTWAIN_GetPatchSearchMode(DTWAIN_SOURCE Source, ref int pSearchMode, int bCurrent)
        => _DTWAIN_GetPatchSearchMode(Source, ref pSearchMode, bCurrent);

        public  int DTWAIN_GetPatchTimeOut(DTWAIN_SOURCE Source, ref int pTimeOut, int bCurrent)
        => _DTWAIN_GetPatchTimeOut(Source, ref pTimeOut, bCurrent);

        public  int DTWAIN_GetPixelFlavor(DTWAIN_SOURCE Source, ref int lpPixelFlavor)
        => _DTWAIN_GetPixelFlavor(Source, ref lpPixelFlavor);

        public  int DTWAIN_GetPixelType(DTWAIN_SOURCE Source, ref int PixelType, ref int BitDepth, int bCurrent)
        => _DTWAIN_GetPixelType(Source, ref PixelType, ref BitDepth, bCurrent);

        public  int DTWAIN_GetPrinter(DTWAIN_SOURCE Source, ref int lpPrinter, int bCurrent)
        => _DTWAIN_GetPrinter(Source, ref lpPrinter, bCurrent);

        public  int DTWAIN_GetPrinterStartNumber(DTWAIN_SOURCE Source, ref int nStart)
        => _DTWAIN_GetPrinterStartNumber(Source, ref nStart);

        public  int DTWAIN_GetPrinterStringMode(DTWAIN_SOURCE Source, ref int PrinterMode, int bCurrent)
        => _DTWAIN_GetPrinterStringMode(Source, ref PrinterMode, bCurrent);

        public  int DTWAIN_GetPrinterStrings(DTWAIN_SOURCE Source, ref DTWAIN_ARRAY ArrayString)
        => _DTWAIN_GetPrinterStrings(Source, ref ArrayString);

        public  int DTWAIN_GetPrinterSuffixString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Suffix, int nMaxLen)
        => _DTWAIN_GetPrinterSuffixString(Source, Suffix, nMaxLen);

        public  int DTWAIN_GetPrinterSuffixStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Suffix, int nLength)
        => _DTWAIN_GetPrinterSuffixStringA(Source, Suffix, nLength);

        public  int DTWAIN_GetPrinterSuffixStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Suffix, int nLength)
        => _DTWAIN_GetPrinterSuffixStringW(Source, Suffix, nLength);

        public  int DTWAIN_GetRegisteredMsg()
        => _DTWAIN_GetRegisteredMsg();

        public  int DTWAIN_GetResolution(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Resolution)
        => _DTWAIN_GetResolution(Source, ref Resolution);

        public  int DTWAIN_GetResolutionString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetResolutionString(Source, Resolution);

        public  int DTWAIN_GetResolutionStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetResolutionStringA(Source, Resolution);

        public  int DTWAIN_GetResolutionStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetResolutionStringW(Source, Resolution);

        public  int DTWAIN_GetResourceString(int ResourceID, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetResourceString(ResourceID, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetResourceStringA(int ResourceID, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetResourceStringA(ResourceID, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetResourceStringW(int ResourceID, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetResourceStringW(ResourceID, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetRotation(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Rotation)
        => _DTWAIN_GetRotation(Source, ref Rotation);

        public  int DTWAIN_GetRotationString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Rotation)
        => _DTWAIN_GetRotationString(Source, Rotation);

        public  int DTWAIN_GetRotationStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Rotation)
        => _DTWAIN_GetRotationStringA(Source, Rotation);

        public  int DTWAIN_GetRotationStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Rotation)
        => _DTWAIN_GetRotationStringW(Source, Rotation);

        public  int DTWAIN_GetSaveFileName(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder fName, int nMaxLen)
        => _DTWAIN_GetSaveFileName(Source, fName, nMaxLen);

        public  int DTWAIN_GetSaveFileNameA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder fName, int nMaxLen)
        => _DTWAIN_GetSaveFileNameA(Source, fName, nMaxLen);

        public  int DTWAIN_GetSaveFileNameW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder fName, int nMaxLen)
        => _DTWAIN_GetSaveFileNameW(Source, fName, nMaxLen);

        public  int DTWAIN_GetSessionDetails([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh)
        => _DTWAIN_GetSessionDetails(szBuf, nSize, indentFactor, bRefresh);

        public  int DTWAIN_GetSessionDetailsA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh)
        => _DTWAIN_GetSessionDetailsA(szBuf, nSize, indentFactor, bRefresh);

        public  int DTWAIN_GetSessionDetailsW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh)
        => _DTWAIN_GetSessionDetailsW(szBuf, nSize, indentFactor, bRefresh);

        public  int DTWAIN_GetShadow(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Shadow)
        => _DTWAIN_GetShadow(Source, ref Shadow);

        public  int DTWAIN_GetShadowString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Shadow)
        => _DTWAIN_GetShadowString(Source, Shadow);

        public  int DTWAIN_GetShadowStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Shadow)
        => _DTWAIN_GetShadowStringA(Source, Shadow);

        public  int DTWAIN_GetShadowStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Shadow)
        => _DTWAIN_GetShadowStringW(Source, Shadow);

        public  int DTWAIN_GetShortVersionString([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetShortVersionString(lpszVer, nLength);

        public  int DTWAIN_GetShortVersionStringA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetShortVersionStringA(lpszVer, nLength);

        public  int DTWAIN_GetShortVersionStringW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetShortVersionStringW(lpszVer, nLength);

        public  DTWAIN_ARRAY DTWAIN_GetSourceAcquisitions(DTWAIN_SOURCE Source)
        => _DTWAIN_GetSourceAcquisitions(Source);

        public  int DTWAIN_GetSourceDetails([MarshalAs(UnmanagedType.LPTStr)] string szSources, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh)
        => _DTWAIN_GetSourceDetails(szSources, szBuf, nSize, indentFactor, bRefresh);

        public  int DTWAIN_GetSourceDetailsA([MarshalAs(UnmanagedType.LPStr)] string szSources, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh)
        => _DTWAIN_GetSourceDetailsA(szSources, szBuf, nSize, indentFactor, bRefresh);

        public  int DTWAIN_GetSourceDetailsW([MarshalAs(UnmanagedType.LPWStr)] string szSources, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szBuf, int nSize, int indentFactor, int bRefresh)
        => _DTWAIN_GetSourceDetailsW(szSources, szBuf, nSize, indentFactor, bRefresh);

        public  DTWAIN_IDENTITY DTWAIN_GetSourceID(DTWAIN_SOURCE Source)
        => _DTWAIN_GetSourceID(Source);

        public  TW_IDENTITY DTWAIN_GetSourceIDEx(DTWAIN_SOURCE Source, [In, Out] TW_IDENTITY pIdentity)
        => _DTWAIN_GetSourceIDEx(Source, pIdentity);

        public  int DTWAIN_GetSourceManufacturer(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen)
        => _DTWAIN_GetSourceManufacturer(Source, szProduct, nMaxLen);

        public  int DTWAIN_GetSourceManufacturerA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceManufacturerA(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceManufacturerW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceManufacturerW(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceProductFamily(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen)
        => _DTWAIN_GetSourceProductFamily(Source, szProduct, nMaxLen);

        public  int DTWAIN_GetSourceProductFamilyA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceProductFamilyA(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceProductFamilyW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceProductFamilyW(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceProductName(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen)
        => _DTWAIN_GetSourceProductName(Source, szProduct, nMaxLen);

        public  int DTWAIN_GetSourceProductNameA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceProductNameA(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceProductNameW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceProductNameW(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceUnit(DTWAIN_SOURCE Source, ref int lpUnit)
        => _DTWAIN_GetSourceUnit(Source, ref lpUnit);

        public  int DTWAIN_GetSourceVersionInfo(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szProduct, int nMaxLen)
        => _DTWAIN_GetSourceVersionInfo(Source, szProduct, nMaxLen);

        public  int DTWAIN_GetSourceVersionInfoA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceVersionInfoA(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceVersionInfoW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szProduct, int nLength)
        => _DTWAIN_GetSourceVersionInfoW(Source, szProduct, nLength);

        public  int DTWAIN_GetSourceVersionNumber(DTWAIN_SOURCE Source, ref int pMajor, ref int pMinor)
        => _DTWAIN_GetSourceVersionNumber(Source, ref pMajor, ref pMinor);

        public  int DTWAIN_GetStaticLibVersion()
        => _DTWAIN_GetStaticLibVersion();

        public  int DTWAIN_GetTempFileDirectory([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szFilePath, int nMaxLen)
        => _DTWAIN_GetTempFileDirectory(szFilePath, nMaxLen);

        public  int DTWAIN_GetTempFileDirectoryA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szFilePath, int nLength)
        => _DTWAIN_GetTempFileDirectoryA(szFilePath, nLength);

        public  int DTWAIN_GetTempFileDirectoryW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szFilePath, int nLength)
        => _DTWAIN_GetTempFileDirectoryW(szFilePath, nLength);

        public  int DTWAIN_GetThreshold(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Threshold)
        => _DTWAIN_GetThreshold(Source, ref Threshold);

        public  int DTWAIN_GetThresholdString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Threshold)
        => _DTWAIN_GetThresholdString(Source, Threshold);

        public  int DTWAIN_GetThresholdStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Threshold)
        => _DTWAIN_GetThresholdStringA(Source, Threshold);

        public  int DTWAIN_GetThresholdStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Threshold)
        => _DTWAIN_GetThresholdStringW(Source, Threshold);

        public  int DTWAIN_GetTimeDate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szTimeDate)
        => _DTWAIN_GetTimeDate(Source, szTimeDate);

        public  int DTWAIN_GetTimeDateA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szTimeDate)
        => _DTWAIN_GetTimeDateA(Source, szTimeDate);

        public  int DTWAIN_GetTimeDateW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szTimeDate)
        => _DTWAIN_GetTimeDateW(Source, szTimeDate);

        public  DTWAIN_IDENTITY DTWAIN_GetTwainAppID()
        => _DTWAIN_GetTwainAppID();

        public  TW_IDENTITY DTWAIN_GetTwainAppIDEx([In, Out] TW_IDENTITY pIdentity)
        => _DTWAIN_GetTwainAppIDEx(pIdentity);

        public  int DTWAIN_GetTwainAvailability()
        => _DTWAIN_GetTwainAvailability();

        public  int DTWAIN_GetTwainAvailabilityEx([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder directories, int nMaxLen)
        => _DTWAIN_GetTwainAvailabilityEx(directories, nMaxLen);

        public  int DTWAIN_GetTwainAvailabilityExA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szDirectories, int nLength)
        => _DTWAIN_GetTwainAvailabilityExA(szDirectories, nLength);

        public  int DTWAIN_GetTwainAvailabilityExW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szDirectories, int nLength)
        => _DTWAIN_GetTwainAvailabilityExW(szDirectories, nLength);

        public  int DTWAIN_GetTwainCountryName(int countryId, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szName)
        => _DTWAIN_GetTwainCountryName(countryId, szName);

        public  int DTWAIN_GetTwainCountryNameA(int countryId, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szName)
        => _DTWAIN_GetTwainCountryNameA(countryId, szName);

        public  int DTWAIN_GetTwainCountryNameW(int countryId, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szName)
        => _DTWAIN_GetTwainCountryNameW(countryId, szName);

        public  int DTWAIN_GetTwainCountryValue([MarshalAs(UnmanagedType.LPTStr)] string country)
        => _DTWAIN_GetTwainCountryValue(country);

        public  int DTWAIN_GetTwainCountryValueA([MarshalAs(UnmanagedType.LPStr)] string country)
        => _DTWAIN_GetTwainCountryValueA(country);

        public  int DTWAIN_GetTwainCountryValueW([MarshalAs(UnmanagedType.LPWStr)] string country)
        => _DTWAIN_GetTwainCountryValueW(country);

        public  HWND DTWAIN_GetTwainHwnd()
        => _DTWAIN_GetTwainHwnd();

        public  int DTWAIN_GetTwainIDFromName([MarshalAs(UnmanagedType.LPTStr)] string lpszBuffer)
        => _DTWAIN_GetTwainIDFromName(lpszBuffer);

        public  int DTWAIN_GetTwainIDFromNameA([MarshalAs(UnmanagedType.LPStr)] string lpszBuffer)
        => _DTWAIN_GetTwainIDFromNameA(lpszBuffer);

        public  int DTWAIN_GetTwainIDFromNameW([MarshalAs(UnmanagedType.LPWStr)] string lpszBuffer)
        => _DTWAIN_GetTwainIDFromNameW(lpszBuffer);

        public  int DTWAIN_GetTwainLanguageName(int nameId, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder szName)
        => _DTWAIN_GetTwainLanguageName(nameId, szName);

        public  int DTWAIN_GetTwainLanguageNameA(int lang, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szName)
        => _DTWAIN_GetTwainLanguageNameA(lang, szName);

        public  int DTWAIN_GetTwainLanguageNameW(int lang, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder szName)
        => _DTWAIN_GetTwainLanguageNameW(lang, szName);

        public  int DTWAIN_GetTwainLanguageValue([MarshalAs(UnmanagedType.LPTStr)] string szName)
        => _DTWAIN_GetTwainLanguageValue(szName);

        public  int DTWAIN_GetTwainLanguageValueA([MarshalAs(UnmanagedType.LPStr)] string lang)
        => _DTWAIN_GetTwainLanguageValueA(lang);

        public  int DTWAIN_GetTwainLanguageValueW([MarshalAs(UnmanagedType.LPWStr)] string lang)
        => _DTWAIN_GetTwainLanguageValueW(lang);

        public  int DTWAIN_GetTwainMode()
        => _DTWAIN_GetTwainMode();

        public  int DTWAIN_GetTwainNameFromConstant(int lConstantType, int lTwainConstant, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszOut, int nSize)
        => _DTWAIN_GetTwainNameFromConstant(lConstantType, lTwainConstant, lpszOut, nSize);

        public  int DTWAIN_GetTwainNameFromConstantA(int lConstantType, int lTwainConstant, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszOut, int nSize)
        => _DTWAIN_GetTwainNameFromConstantA(lConstantType, lTwainConstant, lpszOut, nSize);

        public  int DTWAIN_GetTwainNameFromConstantW(int lConstantType, int lTwainConstant, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszOut, int nSize)
        => _DTWAIN_GetTwainNameFromConstantW(lConstantType, lTwainConstant, lpszOut, nSize);

        public  int DTWAIN_GetTwainStringName(int category, int TwainID, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetTwainStringName(category, TwainID, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetTwainStringNameA(int category, int TwainID, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetTwainStringNameA(category, TwainID, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetTwainStringNameW(int category, int TwainID, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetTwainStringNameW(category, TwainID, lpszBuffer, nMaxLen);

        public  int DTWAIN_GetTwainTimeout()
        => _DTWAIN_GetTwainTimeout();

        public  int DTWAIN_GetVersion(ref int lpMajor, ref int lpMinor, ref int lpVersionType)
        => _DTWAIN_GetVersion(ref lpMajor, ref lpMinor, ref lpVersionType);

        public  int DTWAIN_GetVersionCopyright([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszApp, int nLength)
        => _DTWAIN_GetVersionCopyright(lpszApp, nLength);

        public  int DTWAIN_GetVersionCopyrightA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszApp, int nLength)
        => _DTWAIN_GetVersionCopyrightA(lpszApp, nLength);

        public  int DTWAIN_GetVersionCopyrightW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszApp, int nLength)
        => _DTWAIN_GetVersionCopyrightW(lpszApp, nLength);

        public  int DTWAIN_GetVersionEx(ref int lMajor, ref int lMinor, ref int lVersionType, ref int lPatchLevel)
        => _DTWAIN_GetVersionEx(ref lMajor, ref lMinor, ref lVersionType, ref lPatchLevel);

        public  int DTWAIN_GetVersionInfo([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetVersionInfo(lpszVer, nLength);

        public  int DTWAIN_GetVersionInfoA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetVersionInfoA(lpszVer, nLength);

        public  int DTWAIN_GetVersionInfoW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetVersionInfoW(lpszVer, nLength);

        public  int DTWAIN_GetVersionString([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetVersionString(lpszVer, nLength);

        public  int DTWAIN_GetVersionStringA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetVersionStringA(lpszVer, nLength);

        public  int DTWAIN_GetVersionStringW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszVer, int nLength)
        => _DTWAIN_GetVersionStringW(lpszVer, nLength);

        public  int DTWAIN_GetWindowsVersionInfo([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetWindowsVersionInfo(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetWindowsVersionInfoA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetWindowsVersionInfoA(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetWindowsVersionInfoW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lpszBuffer, int nMaxLen)
        => _DTWAIN_GetWindowsVersionInfoW(lpszBuffer, nMaxLen);

        public  int DTWAIN_GetXResolution(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Resolution)
        => _DTWAIN_GetXResolution(Source, ref Resolution);

        public  int DTWAIN_GetXResolutionString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetXResolutionString(Source, Resolution);

        public  int DTWAIN_GetXResolutionStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetXResolutionStringA(Source, Resolution);

        public  int DTWAIN_GetXResolutionStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetXResolutionStringW(Source, Resolution);

        public  int DTWAIN_GetYResolution(DTWAIN_SOURCE Source, ref DTWAIN_FLOAT Resolution)
        => _DTWAIN_GetYResolution(Source, ref Resolution);

        public  int DTWAIN_GetYResolutionString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetYResolutionString(Source, Resolution);

        public  int DTWAIN_GetYResolutionStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetYResolutionStringA(Source, Resolution);

        public  int DTWAIN_GetYResolutionStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder Resolution)
        => _DTWAIN_GetYResolutionStringW(Source, Resolution);

        public  int DTWAIN_InitExtImageInfo(DTWAIN_SOURCE Source)
        => _DTWAIN_InitExtImageInfo(Source);

        public  int DTWAIN_InitImageFileAppend([MarshalAs(UnmanagedType.LPTStr)] string szFile, int fType)
        => _DTWAIN_InitImageFileAppend(szFile, fType);

        public  int DTWAIN_InitImageFileAppendA([MarshalAs(UnmanagedType.LPStr)] string szFile, int fType)
        => _DTWAIN_InitImageFileAppendA(szFile, fType);

        public  int DTWAIN_InitImageFileAppendW([MarshalAs(UnmanagedType.LPWStr)] string szFile, int fType)
        => _DTWAIN_InitImageFileAppendW(szFile, fType);

        public  int DTWAIN_InitOCRInterface()
        => _DTWAIN_InitOCRInterface();

        public  int DTWAIN_IsAcquiring()
        => _DTWAIN_IsAcquiring();

        public  int DTWAIN_IsAudioXferSupported(DTWAIN_SOURCE Source, int supportVal)
        => _DTWAIN_IsAudioXferSupported(Source, supportVal);

        public  int DTWAIN_IsAutoBorderDetectEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoBorderDetectEnabled(Source);

        public  int DTWAIN_IsAutoBorderDetectSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoBorderDetectSupported(Source);

        public  int DTWAIN_IsAutoBrightEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoBrightEnabled(Source);

        public  int DTWAIN_IsAutoBrightSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoBrightSupported(Source);

        public  int DTWAIN_IsAutoDeskewEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoDeskewEnabled(Source);

        public  int DTWAIN_IsAutoDeskewSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoDeskewSupported(Source);

        public  int DTWAIN_IsAutoFeedEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoFeedEnabled(Source);

        public  int DTWAIN_IsAutoFeedSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoFeedSupported(Source);

        public  int DTWAIN_IsAutoRotateEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoRotateEnabled(Source);

        public  int DTWAIN_IsAutoRotateSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoRotateSupported(Source);

        public  int DTWAIN_IsAutoScanEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutoScanEnabled(Source);

        public  int DTWAIN_IsAutomaticSenseMediumEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutomaticSenseMediumEnabled(Source);

        public  int DTWAIN_IsAutomaticSenseMediumSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsAutomaticSenseMediumSupported(Source);

        public  int DTWAIN_IsBlankPageDetectionOn(DTWAIN_SOURCE Source)
        => _DTWAIN_IsBlankPageDetectionOn(Source);

        public  int DTWAIN_IsBufferedTileModeOn(DTWAIN_SOURCE Source)
        => _DTWAIN_IsBufferedTileModeOn(Source);

        public  int DTWAIN_IsBufferedTileModeSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsBufferedTileModeSupported(Source);

        public  int DTWAIN_IsCapSupported(DTWAIN_SOURCE Source, int lCapability)
        => _DTWAIN_IsCapSupported(Source, lCapability);

        public  int DTWAIN_IsCompressionSupported(DTWAIN_SOURCE Source, int Compression)
        => _DTWAIN_IsCompressionSupported(Source, Compression);

        public  int DTWAIN_IsCustomDSDataSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsCustomDSDataSupported(Source);

        public  int DTWAIN_IsDIBBlank(HANDLE hDib, DTWAIN_FLOAT threshold)
        => _DTWAIN_IsDIBBlank(hDib, threshold);

        public  int DTWAIN_IsDIBBlankString(HANDLE hDib, [MarshalAs(UnmanagedType.LPTStr)] string threshold)
        => _DTWAIN_IsDIBBlankString(hDib, threshold);

        public  int DTWAIN_IsDIBBlankStringA(HANDLE hDib, [MarshalAs(UnmanagedType.LPStr)] string threshold)
        => _DTWAIN_IsDIBBlankStringA(hDib, threshold);

        public  int DTWAIN_IsDIBBlankStringW(HANDLE hDib, [MarshalAs(UnmanagedType.LPWStr)] string threshold)
        => _DTWAIN_IsDIBBlankStringW(hDib, threshold);

        public  int DTWAIN_IsDeviceEventSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsDeviceEventSupported(Source);

        public  int DTWAIN_IsDeviceOnLine(DTWAIN_SOURCE Source)
        => _DTWAIN_IsDeviceOnLine(Source);

        public  int DTWAIN_IsDoubleFeedDetectLengthSupported(DTWAIN_SOURCE Source, DTWAIN_FLOAT value)
        => _DTWAIN_IsDoubleFeedDetectLengthSupported(Source, value);

        public  int DTWAIN_IsDoubleFeedDetectSupported(DTWAIN_SOURCE Source, int SupportVal)
        => _DTWAIN_IsDoubleFeedDetectSupported(Source, SupportVal);

        public  int DTWAIN_IsDoublePageCountOnDuplex(DTWAIN_SOURCE Source)
        => _DTWAIN_IsDoublePageCountOnDuplex(Source);

        public  int DTWAIN_IsDuplexEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsDuplexEnabled(Source);

        public  int DTWAIN_IsDuplexSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsDuplexSupported(Source);

        public  int DTWAIN_IsExtImageInfoSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsExtImageInfoSupported(Source);

        public  int DTWAIN_IsFeederEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsFeederEnabled(Source);

        public  int DTWAIN_IsFeederLoaded(DTWAIN_SOURCE Source)
        => _DTWAIN_IsFeederLoaded(Source);

        public  int DTWAIN_IsFeederSensitive(DTWAIN_SOURCE Source)
        => _DTWAIN_IsFeederSensitive(Source);

        public  int DTWAIN_IsFeederSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsFeederSupported(Source);

        public  int DTWAIN_IsFileSystemSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsFileSystemSupported(Source);

        public  int DTWAIN_IsFileXferSupported(DTWAIN_SOURCE Source, int lFileType)
        => _DTWAIN_IsFileXferSupported(Source, lFileType);

        public  int DTWAIN_IsIAFieldALastPageSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldALastPageSupported(Source);

        public  int DTWAIN_IsIAFieldALevelSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldALevelSupported(Source);

        public  int DTWAIN_IsIAFieldAPrintFormatSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldAPrintFormatSupported(Source);

        public  int DTWAIN_IsIAFieldAValueSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldAValueSupported(Source);

        public  int DTWAIN_IsIAFieldBLastPageSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldBLastPageSupported(Source);

        public  int DTWAIN_IsIAFieldBLevelSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldBLevelSupported(Source);

        public  int DTWAIN_IsIAFieldBPrintFormatSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldBPrintFormatSupported(Source);

        public  int DTWAIN_IsIAFieldBValueSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldBValueSupported(Source);

        public  int DTWAIN_IsIAFieldCLastPageSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldCLastPageSupported(Source);

        public  int DTWAIN_IsIAFieldCLevelSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldCLevelSupported(Source);

        public  int DTWAIN_IsIAFieldCPrintFormatSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldCPrintFormatSupported(Source);

        public  int DTWAIN_IsIAFieldCValueSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldCValueSupported(Source);

        public  int DTWAIN_IsIAFieldDLastPageSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldDLastPageSupported(Source);

        public  int DTWAIN_IsIAFieldDLevelSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldDLevelSupported(Source);

        public  int DTWAIN_IsIAFieldDPrintFormatSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldDPrintFormatSupported(Source);

        public  int DTWAIN_IsIAFieldDValueSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldDValueSupported(Source);

        public  int DTWAIN_IsIAFieldELastPageSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldELastPageSupported(Source);

        public  int DTWAIN_IsIAFieldELevelSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldELevelSupported(Source);

        public  int DTWAIN_IsIAFieldEPrintFormatSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldEPrintFormatSupported(Source);

        public  int DTWAIN_IsIAFieldEValueSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIAFieldEValueSupported(Source);

        public  int DTWAIN_IsImageAddressingSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsImageAddressingSupported(Source);

        public  int DTWAIN_IsIndicatorEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIndicatorEnabled(Source);

        public  int DTWAIN_IsIndicatorSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsIndicatorSupported(Source);

        public  int DTWAIN_IsInitialized()
        => _DTWAIN_IsInitialized();

        public  int DTWAIN_IsJobControlSupported(DTWAIN_SOURCE Source, int JobControl)
        => _DTWAIN_IsJobControlSupported(Source, JobControl);

        public  int DTWAIN_IsLampEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsLampEnabled(Source);

        public  int DTWAIN_IsLampSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsLampSupported(Source);

        public  int DTWAIN_IsLightPathSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsLightPathSupported(Source);

        public  int DTWAIN_IsLightSourceSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsLightSourceSupported(Source);

        public  int DTWAIN_IsMaxBuffersSupported(DTWAIN_SOURCE Source, int MaxBuf)
        => _DTWAIN_IsMaxBuffersSupported(Source, MaxBuf);

        public  int DTWAIN_IsMemFileXferSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsMemFileXferSupported(Source);

        public  int DTWAIN_IsMsgNotifyEnabled()
        => _DTWAIN_IsMsgNotifyEnabled();

        public  int DTWAIN_IsNotifyTripletsEnabled()
        => _DTWAIN_IsNotifyTripletsEnabled();

        public  int DTWAIN_IsOCREngineActivated(DTWAIN_OCRENGINE OCREngine)
        => _DTWAIN_IsOCREngineActivated(OCREngine);

        public  int DTWAIN_IsOpenSourcesOnSelect()
        => _DTWAIN_IsOpenSourcesOnSelect();

        public  int DTWAIN_IsOrientationSupported(DTWAIN_SOURCE Source, int Orientation)
        => _DTWAIN_IsOrientationSupported(Source, Orientation);

        public  int DTWAIN_IsOverscanSupported(DTWAIN_SOURCE Source, int SupportValue)
        => _DTWAIN_IsOverscanSupported(Source, SupportValue);

        public  int DTWAIN_IsPaperDetectable(DTWAIN_SOURCE Source)
        => _DTWAIN_IsPaperDetectable(Source);

        public  int DTWAIN_IsPaperSizeSupported(DTWAIN_SOURCE Source, int PaperSize)
        => _DTWAIN_IsPaperSizeSupported(Source, PaperSize);

        public  int DTWAIN_IsPatchCapsSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsPatchCapsSupported(Source);

        public  int DTWAIN_IsPatchDetectEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsPatchDetectEnabled(Source);

        public  int DTWAIN_IsPatchSupported(DTWAIN_SOURCE Source, int PatchCode)
        => _DTWAIN_IsPatchSupported(Source, PatchCode);

        public  int DTWAIN_IsPeekMessageLoopEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsPeekMessageLoopEnabled(Source);

        public  int DTWAIN_IsPixelTypeSupported(DTWAIN_SOURCE Source, int PixelType)
        => _DTWAIN_IsPixelTypeSupported(Source, PixelType);

        public  int DTWAIN_IsPrinterEnabled(DTWAIN_SOURCE Source, int Printer)
        => _DTWAIN_IsPrinterEnabled(Source, Printer);

        public  int DTWAIN_IsPrinterSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsPrinterSupported(Source);

        public  int DTWAIN_IsRotationSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsRotationSupported(Source);

        public  int DTWAIN_IsSessionEnabled()
        => _DTWAIN_IsSessionEnabled();

        public  int DTWAIN_IsSkipImageInfoError(DTWAIN_SOURCE Source)
        => _DTWAIN_IsSkipImageInfoError(Source);

        public  int DTWAIN_IsSourceAcquiring(DTWAIN_SOURCE Source)
        => _DTWAIN_IsSourceAcquiring(Source);

        public  int DTWAIN_IsSourceAcquiringEx(DTWAIN_SOURCE Source, int bUIOnly)
        => _DTWAIN_IsSourceAcquiringEx(Source, bUIOnly);

        public  int DTWAIN_IsSourceInUIOnlyMode(DTWAIN_SOURCE Source)
        => _DTWAIN_IsSourceInUIOnlyMode(Source);

        public  int DTWAIN_IsSourceOpen(DTWAIN_SOURCE Source)
        => _DTWAIN_IsSourceOpen(Source);

        public  int DTWAIN_IsSourceSelected(DTWAIN_SOURCE Source)
        => _DTWAIN_IsSourceSelected(Source);

        public  int DTWAIN_IsSourceValid(DTWAIN_SOURCE Source)
        => _DTWAIN_IsSourceValid(Source);

        public  int DTWAIN_IsThumbnailEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsThumbnailEnabled(Source);

        public  int DTWAIN_IsThumbnailSupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsThumbnailSupported(Source);

        public  int DTWAIN_IsTwainAvailable()
        => _DTWAIN_IsTwainAvailable();

        public  int DTWAIN_IsTwainAvailableEx([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder directories, int nMaxLen)
        => _DTWAIN_IsTwainAvailableEx(directories, nMaxLen);

        public  int DTWAIN_IsTwainAvailableExA([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder directories, int nMaxLen)
        => _DTWAIN_IsTwainAvailableExA(directories, nMaxLen);

        public  int DTWAIN_IsTwainAvailableExW([MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder directories, int nMaxLen)
        => _DTWAIN_IsTwainAvailableExW(directories, nMaxLen);

        public  int DTWAIN_IsTwainMsg(ref POINT pMsg)
        => _DTWAIN_IsTwainMsg(ref pMsg);

        public  int DTWAIN_IsUIControllable(DTWAIN_SOURCE Source)
        => _DTWAIN_IsUIControllable(Source);

        public  int DTWAIN_IsUIEnabled(DTWAIN_SOURCE Source)
        => _DTWAIN_IsUIEnabled(Source);

        public  int DTWAIN_IsUIOnlySupported(DTWAIN_SOURCE Source)
        => _DTWAIN_IsUIOnlySupported(Source);

        public  int DTWAIN_LoadCustomStringResources([MarshalAs(UnmanagedType.LPTStr)] string sLangDLL)
        => _DTWAIN_LoadCustomStringResources(sLangDLL);

        public  int DTWAIN_LoadCustomStringResourcesA([MarshalAs(UnmanagedType.LPStr)] string sLangDLL)
        => _DTWAIN_LoadCustomStringResourcesA(sLangDLL);

        public  int DTWAIN_LoadCustomStringResourcesEx([MarshalAs(UnmanagedType.LPTStr)] string sLangDLL, int bClear)
        => _DTWAIN_LoadCustomStringResourcesEx(sLangDLL, bClear);

        public  int DTWAIN_LoadCustomStringResourcesExA([MarshalAs(UnmanagedType.LPStr)] string sLangDLL, int bClear)
        => _DTWAIN_LoadCustomStringResourcesExA(sLangDLL, bClear);

        public  int DTWAIN_LoadCustomStringResourcesExW([MarshalAs(UnmanagedType.LPWStr)] string sLangDLL, int bClear)
        => _DTWAIN_LoadCustomStringResourcesExW(sLangDLL, bClear);

        public  int DTWAIN_LoadCustomStringResourcesW([MarshalAs(UnmanagedType.LPWStr)] string sLangDLL)
        => _DTWAIN_LoadCustomStringResourcesW(sLangDLL);

        public  int DTWAIN_LoadLanguageResource(int nLanguage)
        => _DTWAIN_LoadLanguageResource(nLanguage);

        public  DTWAIN_MEMORY_PTR DTWAIN_LockMemory(HANDLE h)
        => _DTWAIN_LockMemory(h);

        public  DTWAIN_MEMORY_PTR DTWAIN_LockMemoryEx(HANDLE h)
        => _DTWAIN_LockMemoryEx(h);

        public  int DTWAIN_LogMessage([MarshalAs(UnmanagedType.LPTStr)] string message)
        => _DTWAIN_LogMessage(message);

        public  int DTWAIN_LogMessageA([MarshalAs(UnmanagedType.LPStr)] string message)
        => _DTWAIN_LogMessageA(message);

        public  int DTWAIN_LogMessageW([MarshalAs(UnmanagedType.LPWStr)] string message)
        => _DTWAIN_LogMessageW(message);

        public  int DTWAIN_MakeRGB(int red, int green, int blue)
        => _DTWAIN_MakeRGB(red, green, blue);

        public  int DTWAIN_OpenSource(DTWAIN_SOURCE Source)
        => _DTWAIN_OpenSource(Source);

        public  int DTWAIN_OpenSourcesOnSelect(int bSet)
        => _DTWAIN_OpenSourcesOnSelect(bSet);

        public  DTWAIN_RANGE DTWAIN_RangeCreate(int nEnumType)
        => _DTWAIN_RangeCreate(nEnumType);

        public  DTWAIN_RANGE DTWAIN_RangeCreateFromCap(DTWAIN_SOURCE Source, int lCapType)
        => _DTWAIN_RangeCreateFromCap(Source, lCapType);

        public  int DTWAIN_RangeDestroy(DTWAIN_RANGE pSource)
        => _DTWAIN_RangeDestroy(pSource);

        public  int DTWAIN_RangeExpand(DTWAIN_RANGE pSource, ref DTWAIN_ARRAY pArray)
        => _DTWAIN_RangeExpand(pSource, ref pArray);

        public  DTWAIN_ARRAY DTWAIN_RangeExpandEx(DTWAIN_RANGE Range)
        => _DTWAIN_RangeExpandEx(Range);

        public  int DTWAIN_RangeGetAll(DTWAIN_RANGE pArray, System.IntPtr pVariantLow, System.IntPtr pVariantUp, System.IntPtr pVariantStep, System.IntPtr pVariantDefault, System.IntPtr pVariantCurrent)
        => _DTWAIN_RangeGetAll(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent);

        public  int DTWAIN_RangeGetAllFloat(DTWAIN_RANGE pArray, ref DTWAIN_FLOAT pVariantLow, ref DTWAIN_FLOAT pVariantUp, ref DTWAIN_FLOAT pVariantStep, ref DTWAIN_FLOAT pVariantDefault, ref DTWAIN_FLOAT pVariantCurrent)
        => _DTWAIN_RangeGetAllFloat(pArray, ref pVariantLow, ref pVariantUp, ref pVariantStep, ref pVariantDefault, ref pVariantCurrent);

        public  int DTWAIN_RangeGetAllFloatString(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dLow, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dUp, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dStep, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dDefault, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder dCurrent)
        => _DTWAIN_RangeGetAllFloatString(pArray, dLow, dUp, dStep, dDefault, dCurrent);

        public  int DTWAIN_RangeGetAllFloatStringA(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dLow, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dUp, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dStep, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dDefault, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dCurrent)
        => _DTWAIN_RangeGetAllFloatStringA(pArray, dLow, dUp, dStep, dDefault, dCurrent);

        public  int DTWAIN_RangeGetAllFloatStringW(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dLow, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dUp, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dStep, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dDefault, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dCurrent)
        => _DTWAIN_RangeGetAllFloatStringW(pArray, dLow, dUp, dStep, dDefault, dCurrent);

        public  int DTWAIN_RangeGetAllLong(DTWAIN_RANGE pArray, ref int pVariantLow, ref int pVariantUp, ref int pVariantStep, ref int pVariantDefault, ref int pVariantCurrent)
        => _DTWAIN_RangeGetAllLong(pArray, ref pVariantLow, ref pVariantUp, ref pVariantStep, ref pVariantDefault, ref pVariantCurrent);

        public  int DTWAIN_RangeGetCount(DTWAIN_RANGE pArray)
        => _DTWAIN_RangeGetCount(pArray);

        public  int DTWAIN_RangeGetExpValue(DTWAIN_RANGE pArray, int lPos, System.IntPtr pVariant)
        => _DTWAIN_RangeGetExpValue(pArray, lPos, pVariant);

        public  int DTWAIN_RangeGetExpValueFloat(DTWAIN_RANGE pArray, int lPos, ref DTWAIN_FLOAT pVal)
        => _DTWAIN_RangeGetExpValueFloat(pArray, lPos, ref pVal);

        public  int DTWAIN_RangeGetExpValueFloatString(DTWAIN_RANGE pArray, int lPos, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pVal)
        => _DTWAIN_RangeGetExpValueFloatString(pArray, lPos, pVal);

        public  int DTWAIN_RangeGetExpValueFloatStringA(DTWAIN_RANGE pArray, int lPos, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder pVal)
        => _DTWAIN_RangeGetExpValueFloatStringA(pArray, lPos, pVal);

        public  int DTWAIN_RangeGetExpValueFloatStringW(DTWAIN_RANGE pArray, int lPos, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pVal)
        => _DTWAIN_RangeGetExpValueFloatStringW(pArray, lPos, pVal);

        public  int DTWAIN_RangeGetExpValueLong(DTWAIN_RANGE pArray, int lPos, ref int pVal)
        => _DTWAIN_RangeGetExpValueLong(pArray, lPos, ref pVal);

        public  int DTWAIN_RangeGetNearestValue(DTWAIN_RANGE pArray, System.IntPtr pVariantIn, System.IntPtr pVariantOut, int RoundType)
        => _DTWAIN_RangeGetNearestValue(pArray, pVariantIn, pVariantOut, RoundType);

        public  int DTWAIN_RangeGetNearestValueFloat(DTWAIN_RANGE pArray, DTWAIN_FLOAT dIn, ref DTWAIN_FLOAT pOut, int RoundType)
        => _DTWAIN_RangeGetNearestValueFloat(pArray, dIn, ref pOut, RoundType);

        public  int DTWAIN_RangeGetNearestValueFloatString(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] string dIn, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pOut, int RoundType)
        => _DTWAIN_RangeGetNearestValueFloatString(pArray, dIn, pOut, RoundType);

        public  int DTWAIN_RangeGetNearestValueFloatStringA(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] string dIn, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dOut, int RoundType)
        => _DTWAIN_RangeGetNearestValueFloatStringA(pArray, dIn, dOut, RoundType);

        public  int DTWAIN_RangeGetNearestValueFloatStringW(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] string dIn, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dOut, int RoundType)
        => _DTWAIN_RangeGetNearestValueFloatStringW(pArray, dIn, dOut, RoundType);

        public  int DTWAIN_RangeGetNearestValueLong(DTWAIN_RANGE pArray, int lIn, ref int pOut, int RoundType)
        => _DTWAIN_RangeGetNearestValueLong(pArray, lIn, ref pOut, RoundType);

        public  int DTWAIN_RangeGetPos(DTWAIN_RANGE pArray, System.IntPtr pVariant, ref int pPos)
        => _DTWAIN_RangeGetPos(pArray, pVariant, ref pPos);

        public  int DTWAIN_RangeGetPosFloat(DTWAIN_RANGE pArray, DTWAIN_FLOAT Val, ref int pPos)
        => _DTWAIN_RangeGetPosFloat(pArray, Val, ref pPos);

        public  int DTWAIN_RangeGetPosFloatString(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] string Val, ref int pPos)
        => _DTWAIN_RangeGetPosFloatString(pArray, Val, ref pPos);

        public  int DTWAIN_RangeGetPosFloatStringA(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] string Val, ref int pPos)
        => _DTWAIN_RangeGetPosFloatStringA(pArray, Val, ref pPos);

        public  int DTWAIN_RangeGetPosFloatStringW(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] string Val, ref int pPos)
        => _DTWAIN_RangeGetPosFloatStringW(pArray, Val, ref pPos);

        public  int DTWAIN_RangeGetPosLong(DTWAIN_RANGE pArray, int Value, ref int pPos)
        => _DTWAIN_RangeGetPosLong(pArray, Value, ref pPos);

        public  int DTWAIN_RangeGetValue(DTWAIN_RANGE pArray, int nWhich, System.IntPtr pVariant)
        => _DTWAIN_RangeGetValue(pArray, nWhich, pVariant);

        public  int DTWAIN_RangeGetValueFloat(DTWAIN_RANGE pArray, int nWhich, ref DTWAIN_FLOAT pVal)
        => _DTWAIN_RangeGetValueFloat(pArray, nWhich, ref pVal);

        public  int DTWAIN_RangeGetValueFloatString(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder pVal)
        => _DTWAIN_RangeGetValueFloatString(pArray, nWhich, pVal);

        public  int DTWAIN_RangeGetValueFloatStringA(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder dValue)
        => _DTWAIN_RangeGetValueFloatStringA(pArray, nWhich, dValue);

        public  int DTWAIN_RangeGetValueFloatStringW(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder dValue)
        => _DTWAIN_RangeGetValueFloatStringW(pArray, nWhich, dValue);

        public  int DTWAIN_RangeGetValueLong(DTWAIN_RANGE pArray, int nWhich, ref int pVal)
        => _DTWAIN_RangeGetValueLong(pArray, nWhich, ref pVal);

        public  int DTWAIN_RangeIsValid(DTWAIN_RANGE Range, ref int pStatus)
        => _DTWAIN_RangeIsValid(Range, ref pStatus);

        public  int DTWAIN_RangeSetAll(DTWAIN_RANGE pArray, System.IntPtr pVariantLow, System.IntPtr pVariantUp, System.IntPtr pVariantStep, System.IntPtr pVariantDefault, System.IntPtr pVariantCurrent)
        => _DTWAIN_RangeSetAll(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent);

        public  int DTWAIN_RangeSetAllFloat(DTWAIN_RANGE pArray, DTWAIN_FLOAT dLow, DTWAIN_FLOAT dUp, DTWAIN_FLOAT dStep, DTWAIN_FLOAT dDefault, DTWAIN_FLOAT dCurrent)
        => _DTWAIN_RangeSetAllFloat(pArray, dLow, dUp, dStep, dDefault, dCurrent);

        public  int DTWAIN_RangeSetAllFloatString(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPTStr)] string dLow, [MarshalAs(UnmanagedType.LPTStr)] string dUp, [MarshalAs(UnmanagedType.LPTStr)] string dStep, [MarshalAs(UnmanagedType.LPTStr)] string dDefault, [MarshalAs(UnmanagedType.LPTStr)] string dCurrent)
        => _DTWAIN_RangeSetAllFloatString(pArray, dLow, dUp, dStep, dDefault, dCurrent);

        public  int DTWAIN_RangeSetAllFloatStringA(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPStr)] string dLow, [MarshalAs(UnmanagedType.LPStr)] string dUp, [MarshalAs(UnmanagedType.LPStr)] string dStep, [MarshalAs(UnmanagedType.LPStr)] string dDefault, [MarshalAs(UnmanagedType.LPStr)] string dCurrent)
        => _DTWAIN_RangeSetAllFloatStringA(pArray, dLow, dUp, dStep, dDefault, dCurrent);

        public  int DTWAIN_RangeSetAllFloatStringW(DTWAIN_RANGE pArray, [MarshalAs(UnmanagedType.LPWStr)] string dLow, [MarshalAs(UnmanagedType.LPWStr)] string dUp, [MarshalAs(UnmanagedType.LPWStr)] string dStep, [MarshalAs(UnmanagedType.LPWStr)] string dDefault, [MarshalAs(UnmanagedType.LPWStr)] string dCurrent)
        => _DTWAIN_RangeSetAllFloatStringW(pArray, dLow, dUp, dStep, dDefault, dCurrent);

        public  int DTWAIN_RangeSetAllLong(DTWAIN_RANGE pArray, int lLow, int lUp, int lStep, int lDefault, int lCurrent)
        => _DTWAIN_RangeSetAllLong(pArray, lLow, lUp, lStep, lDefault, lCurrent);

        public  int DTWAIN_RangeSetValue(DTWAIN_RANGE pArray, int nWhich, System.IntPtr pVal)
        => _DTWAIN_RangeSetValue(pArray, nWhich, pVal);

        public  int DTWAIN_RangeSetValueFloat(DTWAIN_RANGE pArray, int nWhich, DTWAIN_FLOAT Val)
        => _DTWAIN_RangeSetValueFloat(pArray, nWhich, Val);

        public  int DTWAIN_RangeSetValueFloatString(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPTStr)] string Val)
        => _DTWAIN_RangeSetValueFloatString(pArray, nWhich, Val);

        public  int DTWAIN_RangeSetValueFloatStringA(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPStr)] string dValue)
        => _DTWAIN_RangeSetValueFloatStringA(pArray, nWhich, dValue);

        public  int DTWAIN_RangeSetValueFloatStringW(DTWAIN_RANGE pArray, int nWhich, [MarshalAs(UnmanagedType.LPWStr)] string dValue)
        => _DTWAIN_RangeSetValueFloatStringW(pArray, nWhich, dValue);

        public  int DTWAIN_RangeSetValueLong(DTWAIN_RANGE pArray, int nWhich, int Val)
        => _DTWAIN_RangeSetValueLong(pArray, nWhich, Val);

        public  int DTWAIN_RemovePDFTextElement(DTWAIN_SOURCE Source, DTWAIN_PDFTEXTELEMENT TextElement)
        => _DTWAIN_RemovePDFTextElement(Source, TextElement);

        public  int DTWAIN_ResetPDFTextElement(DTWAIN_PDFTEXTELEMENT TextElement)
        => _DTWAIN_ResetPDFTextElement(TextElement);

        public  int DTWAIN_RewindPage(DTWAIN_SOURCE Source)
        => _DTWAIN_RewindPage(Source);

        public  DTWAIN_OCRENGINE DTWAIN_SelectDefaultOCREngine()
        => _DTWAIN_SelectDefaultOCREngine();

        public  DTWAIN_SOURCE DTWAIN_SelectDefaultSource()
        => _DTWAIN_SelectDefaultSource();

        public  DTWAIN_SOURCE DTWAIN_SelectDefaultSourceWithOpen(int bOpen)
        => _DTWAIN_SelectDefaultSourceWithOpen(bOpen);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngine()
        => _DTWAIN_SelectOCREngine();

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, int nOptions)
        => _DTWAIN_SelectOCREngine2(hWndParent, szTitle, xPos, yPos, nOptions);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2A(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, int nOptions)
        => _DTWAIN_SelectOCREngine2A(hWndParent, szTitle, xPos, yPos, nOptions);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2Ex(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string szIncludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szExcludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szNameMapping, int nOptions)
        => _DTWAIN_SelectOCREngine2Ex(hWndParent, szTitle, xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2ExA(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPStr)] string szNameMapping, int nOptions)
        => _DTWAIN_SelectOCREngine2ExA(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2ExW(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szNameMapping, int nOptions)
        => _DTWAIN_SelectOCREngine2ExW(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngine2W(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, int nOptions)
        => _DTWAIN_SelectOCREngine2W(hWndParent, szTitle, xPos, yPos, nOptions);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngineByName([MarshalAs(UnmanagedType.LPTStr)] string lpszName)
        => _DTWAIN_SelectOCREngineByName(lpszName);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngineByNameA([MarshalAs(UnmanagedType.LPStr)] string lpszName)
        => _DTWAIN_SelectOCREngineByNameA(lpszName);

        public  DTWAIN_OCRENGINE DTWAIN_SelectOCREngineByNameW([MarshalAs(UnmanagedType.LPWStr)] string lpszName)
        => _DTWAIN_SelectOCREngineByNameW(lpszName);

        public  DTWAIN_SOURCE DTWAIN_SelectSource()
        => _DTWAIN_SelectSource();

        public  DTWAIN_SOURCE DTWAIN_SelectSource2(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, int nOptions)
        => _DTWAIN_SelectSource2(hWndParent, szTitle, xPos, yPos, nOptions);

        public  DTWAIN_SOURCE DTWAIN_SelectSource2A(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, int nOptions)
        => _DTWAIN_SelectSource2A(hWndParent, szTitle, xPos, yPos, nOptions);

        public  DTWAIN_SOURCE DTWAIN_SelectSource2Ex(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPTStr)] string szIncludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szExcludeFilter, [MarshalAs(UnmanagedType.LPTStr)] string szNameMapping, int nOptions)
        => _DTWAIN_SelectSource2Ex(hWndParent, szTitle, xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions);

        public  DTWAIN_SOURCE DTWAIN_SelectSource2ExA(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPStr)] string szNameMapping, int nOptions)
        => _DTWAIN_SelectSource2ExA(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);

        public  DTWAIN_SOURCE DTWAIN_SelectSource2ExW(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, [MarshalAs(UnmanagedType.LPWStr)] string szIncludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szExcludeNames, [MarshalAs(UnmanagedType.LPWStr)] string szNameMapping, int nOptions)
        => _DTWAIN_SelectSource2ExW(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);

        public  DTWAIN_SOURCE DTWAIN_SelectSource2W(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, int nOptions)
        => _DTWAIN_SelectSource2W(hWndParent, szTitle, xPos, yPos, nOptions);

        public  DTWAIN_SOURCE DTWAIN_SelectSourceByName([MarshalAs(UnmanagedType.LPTStr)] string lpszName)
        => _DTWAIN_SelectSourceByName(lpszName);

        public  DTWAIN_SOURCE DTWAIN_SelectSourceByNameA([MarshalAs(UnmanagedType.LPStr)] string lpszName)
        => _DTWAIN_SelectSourceByNameA(lpszName);

        public  DTWAIN_SOURCE DTWAIN_SelectSourceByNameW([MarshalAs(UnmanagedType.LPWStr)] string lpszName)
        => _DTWAIN_SelectSourceByNameW(lpszName);

        public  DTWAIN_SOURCE DTWAIN_SelectSourceByNameWithOpen([MarshalAs(UnmanagedType.LPTStr)] string lpszName, int bOpen)
        => _DTWAIN_SelectSourceByNameWithOpen(lpszName, bOpen);

        public  DTWAIN_SOURCE DTWAIN_SelectSourceByNameWithOpenA([MarshalAs(UnmanagedType.LPStr)] string lpszName, int bOpen)
        => _DTWAIN_SelectSourceByNameWithOpenA(lpszName, bOpen);

        public  DTWAIN_SOURCE DTWAIN_SelectSourceByNameWithOpenW([MarshalAs(UnmanagedType.LPWStr)] string lpszName, int bOpen)
        => _DTWAIN_SelectSourceByNameWithOpenW(lpszName, bOpen);

        public  DTWAIN_SOURCE DTWAIN_SelectSourceWithOpen(int bOpen)
        => _DTWAIN_SelectSourceWithOpen(bOpen);

        public  int DTWAIN_SetAcquireArea(DTWAIN_SOURCE Source, int lSetType, DTWAIN_ARRAY FloatEnum, DTWAIN_ARRAY ActualEnum)
        => _DTWAIN_SetAcquireArea(Source, lSetType, FloatEnum, ActualEnum);

        public  int DTWAIN_SetAcquireArea2(DTWAIN_SOURCE Source, DTWAIN_FLOAT left, DTWAIN_FLOAT top, DTWAIN_FLOAT right, DTWAIN_FLOAT bottom, int lUnit, int Flags)
        => _DTWAIN_SetAcquireArea2(Source, left, top, right, bottom, lUnit, Flags);

        public  int DTWAIN_SetAcquireArea2String(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string left, [MarshalAs(UnmanagedType.LPTStr)] string top, [MarshalAs(UnmanagedType.LPTStr)] string right, [MarshalAs(UnmanagedType.LPTStr)] string bottom, int lUnit, int Flags)
        => _DTWAIN_SetAcquireArea2String(Source, left, top, right, bottom, lUnit, Flags);

        public  int DTWAIN_SetAcquireArea2StringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string left, [MarshalAs(UnmanagedType.LPStr)] string top, [MarshalAs(UnmanagedType.LPStr)] string right, [MarshalAs(UnmanagedType.LPStr)] string bottom, int lUnit, int Flags)
        => _DTWAIN_SetAcquireArea2StringA(Source, left, top, right, bottom, lUnit, Flags);

        public  int DTWAIN_SetAcquireArea2StringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string left, [MarshalAs(UnmanagedType.LPWStr)] string top, [MarshalAs(UnmanagedType.LPWStr)] string right, [MarshalAs(UnmanagedType.LPWStr)] string bottom, int lUnit, int Flags)
        => _DTWAIN_SetAcquireArea2StringW(Source, left, top, right, bottom, lUnit, Flags);

        public  int DTWAIN_SetAcquireImageNegative(DTWAIN_SOURCE Source, int IsNegative)
        => _DTWAIN_SetAcquireImageNegative(Source, IsNegative);

        public  int DTWAIN_SetAcquireImageScale(DTWAIN_SOURCE Source, DTWAIN_FLOAT xscale, DTWAIN_FLOAT yscale)
        => _DTWAIN_SetAcquireImageScale(Source, xscale, yscale);

        public  int DTWAIN_SetAcquireImageScaleString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string xscale, [MarshalAs(UnmanagedType.LPTStr)] string yscale)
        => _DTWAIN_SetAcquireImageScaleString(Source, xscale, yscale);

        public  int DTWAIN_SetAcquireImageScaleStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string xscale, [MarshalAs(UnmanagedType.LPStr)] string yscale)
        => _DTWAIN_SetAcquireImageScaleStringA(Source, xscale, yscale);

        public  int DTWAIN_SetAcquireImageScaleStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string xscale, [MarshalAs(UnmanagedType.LPWStr)] string yscale)
        => _DTWAIN_SetAcquireImageScaleStringW(Source, xscale, yscale);

        public  int DTWAIN_SetAcquireStripBuffer(DTWAIN_SOURCE Source, HANDLE hMem)
        => _DTWAIN_SetAcquireStripBuffer(Source, hMem);

        public  int DTWAIN_SetAcquireStripSize(DTWAIN_SOURCE Source, uint StripSize)
        => _DTWAIN_SetAcquireStripSize(Source, StripSize);

        public  int DTWAIN_SetAlarmVolume(DTWAIN_SOURCE Source, int Volume)
        => _DTWAIN_SetAlarmVolume(Source, Volume);

        public  int DTWAIN_SetAlarms(DTWAIN_SOURCE Source, DTWAIN_ARRAY Alarms)
        => _DTWAIN_SetAlarms(Source, Alarms);

        public  int DTWAIN_SetAllCapsToDefault(DTWAIN_SOURCE Source)
        => _DTWAIN_SetAllCapsToDefault(Source);

        public  int DTWAIN_SetAppInfo([MarshalAs(UnmanagedType.LPTStr)] string szVerStr, [MarshalAs(UnmanagedType.LPTStr)] string szManu, [MarshalAs(UnmanagedType.LPTStr)] string szProdFam, [MarshalAs(UnmanagedType.LPTStr)] string szProdName)
        => _DTWAIN_SetAppInfo(szVerStr, szManu, szProdFam, szProdName);

        public  int DTWAIN_SetAppInfoA([MarshalAs(UnmanagedType.LPStr)] string szVerStr, [MarshalAs(UnmanagedType.LPStr)] string szManu, [MarshalAs(UnmanagedType.LPStr)] string szProdFam, [MarshalAs(UnmanagedType.LPStr)] string szProdName)
        => _DTWAIN_SetAppInfoA(szVerStr, szManu, szProdFam, szProdName);

        public  int DTWAIN_SetAppInfoW([MarshalAs(UnmanagedType.LPWStr)] string szVerStr, [MarshalAs(UnmanagedType.LPWStr)] string szManu, [MarshalAs(UnmanagedType.LPWStr)] string szProdFam, [MarshalAs(UnmanagedType.LPWStr)] string szProdName)
        => _DTWAIN_SetAppInfoW(szVerStr, szManu, szProdFam, szProdName);

        public  int DTWAIN_SetAuthor(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szAuthor)
        => _DTWAIN_SetAuthor(Source, szAuthor);

        public  int DTWAIN_SetAuthorA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szAuthor)
        => _DTWAIN_SetAuthorA(Source, szAuthor);

        public  int DTWAIN_SetAuthorW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szAuthor)
        => _DTWAIN_SetAuthorW(Source, szAuthor);

        public  int DTWAIN_SetAvailablePrinters(DTWAIN_SOURCE Source, int lpAvailPrinters)
        => _DTWAIN_SetAvailablePrinters(Source, lpAvailPrinters);

        public  int DTWAIN_SetAvailablePrintersArray(DTWAIN_SOURCE Source, DTWAIN_ARRAY AvailPrinters)
        => _DTWAIN_SetAvailablePrintersArray(Source, AvailPrinters);

        public  int DTWAIN_SetBitDepth(DTWAIN_SOURCE Source, int BitDepth, int bSetCurrent)
        => _DTWAIN_SetBitDepth(Source, BitDepth, bSetCurrent);

        public  int DTWAIN_SetBlankPageDetection(DTWAIN_SOURCE Source, DTWAIN_FLOAT threshold, int discard_option, int bSet)
        => _DTWAIN_SetBlankPageDetection(Source, threshold, discard_option, bSet);

        public  int DTWAIN_SetBlankPageDetectionEx(DTWAIN_SOURCE Source, DTWAIN_FLOAT threshold, int autodetect, int detectOpts, int bSet)
        => _DTWAIN_SetBlankPageDetectionEx(Source, threshold, autodetect, detectOpts, bSet);

        public  int DTWAIN_SetBlankPageDetectionExString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string threshold, int autodetect_option, int detectOpts, int bSet)
        => _DTWAIN_SetBlankPageDetectionExString(Source, threshold, autodetect_option, detectOpts, bSet);

        public  int DTWAIN_SetBlankPageDetectionExStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string threshold, int autodetect_option, int detectOpts, int bSet)
        => _DTWAIN_SetBlankPageDetectionExStringA(Source, threshold, autodetect_option, detectOpts, bSet);

        public  int DTWAIN_SetBlankPageDetectionExStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string threshold, int autodetect_option, int detectOpts, int bSet)
        => _DTWAIN_SetBlankPageDetectionExStringW(Source, threshold, autodetect_option, detectOpts, bSet);

        public  int DTWAIN_SetBlankPageDetectionString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string threshold, int autodetect_option, int bSet)
        => _DTWAIN_SetBlankPageDetectionString(Source, threshold, autodetect_option, bSet);

        public  int DTWAIN_SetBlankPageDetectionStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string threshold, int autodetect_option, int bSet)
        => _DTWAIN_SetBlankPageDetectionStringA(Source, threshold, autodetect_option, bSet);

        public  int DTWAIN_SetBlankPageDetectionStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string threshold, int autodetect_option, int bSet)
        => _DTWAIN_SetBlankPageDetectionStringW(Source, threshold, autodetect_option, bSet);

        public  int DTWAIN_SetBrightness(DTWAIN_SOURCE Source, DTWAIN_FLOAT Brightness)
        => _DTWAIN_SetBrightness(Source, Brightness);

        public  int DTWAIN_SetBrightnessString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Brightness)
        => _DTWAIN_SetBrightnessString(Source, Brightness);

        public  int DTWAIN_SetBrightnessStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Contrast)
        => _DTWAIN_SetBrightnessStringA(Source, Contrast);

        public  int DTWAIN_SetBrightnessStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Contrast)
        => _DTWAIN_SetBrightnessStringW(Source, Contrast);

        public  int DTWAIN_SetBufferedTileMode(DTWAIN_SOURCE Source, int bTileMode)
        => _DTWAIN_SetBufferedTileMode(Source, bTileMode);

        public  DTwainCallback DTWAIN_SetCallback(DTwainCallback Fn, int UserData)
        => _DTWAIN_SetCallback(Fn, UserData);

        public  DTwainCallback64 DTWAIN_SetCallback64(DTwainCallback64 Fn, long UserData)
        => _DTWAIN_SetCallback64(Fn, UserData);

        public  int DTWAIN_SetCamera(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szCamera)
        => _DTWAIN_SetCamera(Source, szCamera);

        public  int DTWAIN_SetCameraA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szCamera)
        => _DTWAIN_SetCameraA(Source, szCamera);

        public  int DTWAIN_SetCameraW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szCamera)
        => _DTWAIN_SetCameraW(Source, szCamera);

        public  int DTWAIN_SetCapValues(DTWAIN_SOURCE Source, int lCap, int lSetType, DTWAIN_ARRAY pArray)
        => _DTWAIN_SetCapValues(Source, lCap, lSetType, pArray);

        public  int DTWAIN_SetCapValuesEx(DTWAIN_SOURCE Source, int lCap, int lSetType, int lContainerType, DTWAIN_ARRAY pArray)
        => _DTWAIN_SetCapValuesEx(Source, lCap, lSetType, lContainerType, pArray);

        public  int DTWAIN_SetCapValuesEx2(DTWAIN_SOURCE Source, int lCap, int lSetType, int lContainerType, int nDataType, DTWAIN_ARRAY pArray)
        => _DTWAIN_SetCapValuesEx2(Source, lCap, lSetType, lContainerType, nDataType, pArray);

        public  int DTWAIN_SetCaption(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Caption)
        => _DTWAIN_SetCaption(Source, Caption);

        public  int DTWAIN_SetCaptionA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Caption)
        => _DTWAIN_SetCaptionA(Source, Caption);

        public  int DTWAIN_SetCaptionW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Caption)
        => _DTWAIN_SetCaptionW(Source, Caption);

        public  int DTWAIN_SetCompressionType(DTWAIN_SOURCE Source, int lCompression, int bSetCurrent)
        => _DTWAIN_SetCompressionType(Source, lCompression, bSetCurrent);

        public  int DTWAIN_SetContrast(DTWAIN_SOURCE Source, DTWAIN_FLOAT Contrast)
        => _DTWAIN_SetContrast(Source, Contrast);

        public  int DTWAIN_SetContrastString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Contrast)
        => _DTWAIN_SetContrastString(Source, Contrast);

        public  int DTWAIN_SetContrastStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Contrast)
        => _DTWAIN_SetContrastStringA(Source, Contrast);

        public  int DTWAIN_SetContrastStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Contrast)
        => _DTWAIN_SetContrastStringW(Source, Contrast);

        public  int DTWAIN_SetCountry(int nCountry)
        => _DTWAIN_SetCountry(nCountry);

        public  int DTWAIN_SetCurrentRetryCount(DTWAIN_SOURCE Source, int nCount)
        => _DTWAIN_SetCurrentRetryCount(Source, nCount);

        public  int DTWAIN_SetCustomDSData(DTWAIN_SOURCE Source, HANDLE hData, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U8, SizeParamIndex = 3)] byte[] Data, uint dSize, int nFlags)
        => _DTWAIN_SetCustomDSData(Source, hData, Data, dSize, nFlags);

        public  int DTWAIN_SetDSMSearchOrder(int SearchPath)
        => _DTWAIN_SetDSMSearchOrder(SearchPath);

        public  int DTWAIN_SetDSMSearchOrderEx([MarshalAs(UnmanagedType.LPTStr)] string SearchOrder, [MarshalAs(UnmanagedType.LPTStr)] string UserPath)
        => _DTWAIN_SetDSMSearchOrderEx(SearchOrder, UserPath);

        public  int DTWAIN_SetDSMSearchOrderExA([MarshalAs(UnmanagedType.LPStr)] string SearchOrder, [MarshalAs(UnmanagedType.LPStr)] string szUserPath)
        => _DTWAIN_SetDSMSearchOrderExA(SearchOrder, szUserPath);

        public  int DTWAIN_SetDSMSearchOrderExW([MarshalAs(UnmanagedType.LPWStr)] string SearchOrder, [MarshalAs(UnmanagedType.LPWStr)] string szUserPath)
        => _DTWAIN_SetDSMSearchOrderExW(SearchOrder, szUserPath);

        public  int DTWAIN_SetDefaultSource(DTWAIN_SOURCE Source)
        => _DTWAIN_SetDefaultSource(Source);

        public  int DTWAIN_SetDeviceNotifications(DTWAIN_SOURCE Source, int DevEvents)
        => _DTWAIN_SetDeviceNotifications(Source, DevEvents);

        public  int DTWAIN_SetDeviceTimeDate(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szTimeDate)
        => _DTWAIN_SetDeviceTimeDate(Source, szTimeDate);

        public  int DTWAIN_SetDeviceTimeDateA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szTimeDate)
        => _DTWAIN_SetDeviceTimeDateA(Source, szTimeDate);

        public  int DTWAIN_SetDeviceTimeDateW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szTimeDate)
        => _DTWAIN_SetDeviceTimeDateW(Source, szTimeDate);

        public  int DTWAIN_SetDoubleFeedDetectLength(DTWAIN_SOURCE Source, DTWAIN_FLOAT Value)
        => _DTWAIN_SetDoubleFeedDetectLength(Source, Value);

        public  int DTWAIN_SetDoubleFeedDetectLengthString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string value)
        => _DTWAIN_SetDoubleFeedDetectLengthString(Source, value);

        public  int DTWAIN_SetDoubleFeedDetectLengthStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szLength)
        => _DTWAIN_SetDoubleFeedDetectLengthStringA(Source, szLength);

        public  int DTWAIN_SetDoubleFeedDetectLengthStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szLength)
        => _DTWAIN_SetDoubleFeedDetectLengthStringW(Source, szLength);

        public  int DTWAIN_SetDoubleFeedDetectValues(DTWAIN_SOURCE Source, DTWAIN_ARRAY prray)
        => _DTWAIN_SetDoubleFeedDetectValues(Source, prray);

        public  int DTWAIN_SetDoublePageCountOnDuplex(DTWAIN_SOURCE Source, int bDoubleCount)
        => _DTWAIN_SetDoublePageCountOnDuplex(Source, bDoubleCount);

        public  int DTWAIN_SetEOJDetectValue(DTWAIN_SOURCE Source, int nValue)
        => _DTWAIN_SetEOJDetectValue(Source, nValue);

        public  int DTWAIN_SetErrorBufferThreshold(int nErrors)
        => _DTWAIN_SetErrorBufferThreshold(nErrors);

        public  int DTWAIN_SetErrorCallback(DTwainErrorProc proc, int UserData)
        => _DTWAIN_SetErrorCallback(proc, UserData);

        public  int DTWAIN_SetErrorCallback64(DTwainErrorProc64 proc, long UserData64)
        => _DTWAIN_SetErrorCallback64(proc, UserData64);

        public  int DTWAIN_SetFeederAlignment(DTWAIN_SOURCE Source, int lpAlignment)
        => _DTWAIN_SetFeederAlignment(Source, lpAlignment);

        public  int DTWAIN_SetFeederOrder(DTWAIN_SOURCE Source, int lOrder)
        => _DTWAIN_SetFeederOrder(Source, lOrder);

        public  int DTWAIN_SetFeederWaitTime(DTWAIN_SOURCE Source, int waitTime, int flags)
        => _DTWAIN_SetFeederWaitTime(Source, waitTime, flags);

        public  int DTWAIN_SetFileAutoIncrement(DTWAIN_SOURCE Source, int Increment, int bResetOnAcquire, int bSet)
        => _DTWAIN_SetFileAutoIncrement(Source, Increment, bResetOnAcquire, bSet);

        public  int DTWAIN_SetFileCompressionType(DTWAIN_SOURCE Source, int lCompression, int bIsCustom)
        => _DTWAIN_SetFileCompressionType(Source, lCompression, bIsCustom);

        public  int DTWAIN_SetFileSavePos(HWND hWndParent, [MarshalAs(UnmanagedType.LPTStr)] string szTitle, int xPos, int yPos, int nFlags)
        => _DTWAIN_SetFileSavePos(hWndParent, szTitle, xPos, yPos, nFlags);

        public  int DTWAIN_SetFileSavePosA(HWND hWndParent, [MarshalAs(UnmanagedType.LPStr)] string szTitle, int xPos, int yPos, int nFlags)
        => _DTWAIN_SetFileSavePosA(hWndParent, szTitle, xPos, yPos, nFlags);

        public  int DTWAIN_SetFileSavePosW(HWND hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string szTitle, int xPos, int yPos, int nFlags)
        => _DTWAIN_SetFileSavePosW(hWndParent, szTitle, xPos, yPos, nFlags);

        public  int DTWAIN_SetFileXferFormat(DTWAIN_SOURCE Source, int lFileType, int bSetCurrent)
        => _DTWAIN_SetFileXferFormat(Source, lFileType, bSetCurrent);

        public  int DTWAIN_SetHalftone(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpHalftone)
        => _DTWAIN_SetHalftone(Source, lpHalftone);

        public  int DTWAIN_SetHalftoneA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpHalftone)
        => _DTWAIN_SetHalftoneA(Source, lpHalftone);

        public  int DTWAIN_SetHalftoneW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpHalftone)
        => _DTWAIN_SetHalftoneW(Source, lpHalftone);

        public  int DTWAIN_SetHighlight(DTWAIN_SOURCE Source, DTWAIN_FLOAT Highlight)
        => _DTWAIN_SetHighlight(Source, Highlight);

        public  int DTWAIN_SetHighlightString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Highlight)
        => _DTWAIN_SetHighlightString(Source, Highlight);

        public  int DTWAIN_SetHighlightStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Highlight)
        => _DTWAIN_SetHighlightStringA(Source, Highlight);

        public  int DTWAIN_SetHighlightStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Highlight)
        => _DTWAIN_SetHighlightStringW(Source, Highlight);

        public  int DTWAIN_SetJobControl(DTWAIN_SOURCE Source, int JobControl, int bSetCurrent)
        => _DTWAIN_SetJobControl(Source, JobControl, bSetCurrent);

        public  int DTWAIN_SetJpegValues(DTWAIN_SOURCE Source, int Quality, int Progressive)
        => _DTWAIN_SetJpegValues(Source, Quality, Progressive);

        public  int DTWAIN_SetJpegXRValues(DTWAIN_SOURCE Source, int Quality, int Progressive)
        => _DTWAIN_SetJpegXRValues(Source, Quality, Progressive);

        public  int DTWAIN_SetLanguage(int nLanguage)
        => _DTWAIN_SetLanguage(nLanguage);

        public  int DTWAIN_SetLastError(int nError)
        => _DTWAIN_SetLastError(nError);

        public  int DTWAIN_SetLightPath(DTWAIN_SOURCE Source, int LightPath)
        => _DTWAIN_SetLightPath(Source, LightPath);

        public  int DTWAIN_SetLightPathEx(DTWAIN_SOURCE Source, DTWAIN_ARRAY LightPaths)
        => _DTWAIN_SetLightPathEx(Source, LightPaths);

        public  int DTWAIN_SetLightSource(DTWAIN_SOURCE Source, int LightSource)
        => _DTWAIN_SetLightSource(Source, LightSource);

        public  int DTWAIN_SetLightSources(DTWAIN_SOURCE Source, DTWAIN_ARRAY LightSources)
        => _DTWAIN_SetLightSources(Source, LightSources);

        public  int DTWAIN_SetLoggerCallback(DTwainLoggerProc logProc, long UserData)
        => _DTWAIN_SetLoggerCallback(logProc, UserData);

        public  int DTWAIN_SetLoggerCallbackA(DTwainLoggerProcA logProc, long UserData)
        => _DTWAIN_SetLoggerCallbackA(logProc, UserData);

        public  int DTWAIN_SetLoggerCallbackW(DTwainLoggerProcW logProc, long UserData)
        => _DTWAIN_SetLoggerCallbackW(logProc, UserData);

        public  int DTWAIN_SetManualDuplexMode(DTWAIN_SOURCE Source, int Flags, int bSet)
        => _DTWAIN_SetManualDuplexMode(Source, Flags, bSet);

        public  int DTWAIN_SetMaxAcquisitions(DTWAIN_SOURCE Source, int MaxAcquires)
        => _DTWAIN_SetMaxAcquisitions(Source, MaxAcquires);

        public  int DTWAIN_SetMaxBuffers(DTWAIN_SOURCE Source, int MaxBuf)
        => _DTWAIN_SetMaxBuffers(Source, MaxBuf);

        public  int DTWAIN_SetMaxRetryAttempts(DTWAIN_SOURCE Source, int nAttempts)
        => _DTWAIN_SetMaxRetryAttempts(Source, nAttempts);

        public  int DTWAIN_SetMultipageScanMode(DTWAIN_SOURCE Source, int ScanType)
        => _DTWAIN_SetMultipageScanMode(Source, ScanType);

        public  int DTWAIN_SetNoiseFilter(DTWAIN_SOURCE Source, int NoiseFilter)
        => _DTWAIN_SetNoiseFilter(Source, NoiseFilter);

        public  int DTWAIN_SetOCRCapValues(DTWAIN_OCRENGINE Engine, int OCRCapValue, int SetType, DTWAIN_ARRAY CapValues)
        => _DTWAIN_SetOCRCapValues(Engine, OCRCapValue, SetType, CapValues);

        public  int DTWAIN_SetOrientation(DTWAIN_SOURCE Source, int Orient, int bSetCurrent)
        => _DTWAIN_SetOrientation(Source, Orient, bSetCurrent);

        public  int DTWAIN_SetOverscan(DTWAIN_SOURCE Source, int Value, int bSetCurrent)
        => _DTWAIN_SetOverscan(Source, Value, bSetCurrent);

        public  int DTWAIN_SetPDFAESEncryption(DTWAIN_SOURCE Source, int nWhichEncryption, int bUseAES)
        => _DTWAIN_SetPDFAESEncryption(Source, nWhichEncryption, bUseAES);

        public  int DTWAIN_SetPDFASCIICompression(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_SetPDFASCIICompression(Source, bSet);

        public  int DTWAIN_SetPDFAuthor(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpAuthor)
        => _DTWAIN_SetPDFAuthor(Source, lpAuthor);

        public  int DTWAIN_SetPDFAuthorA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpAuthor)
        => _DTWAIN_SetPDFAuthorA(Source, lpAuthor);

        public  int DTWAIN_SetPDFAuthorW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpAuthor)
        => _DTWAIN_SetPDFAuthorW(Source, lpAuthor);

        public  int DTWAIN_SetPDFCompression(DTWAIN_SOURCE Source, int bCompression)
        => _DTWAIN_SetPDFCompression(Source, bCompression);

        public  int DTWAIN_SetPDFCreator(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpCreator)
        => _DTWAIN_SetPDFCreator(Source, lpCreator);

        public  int DTWAIN_SetPDFCreatorA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpCreator)
        => _DTWAIN_SetPDFCreatorA(Source, lpCreator);

        public  int DTWAIN_SetPDFCreatorW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpCreator)
        => _DTWAIN_SetPDFCreatorW(Source, lpCreator);

        public  int DTWAIN_SetPDFEncryption(DTWAIN_SOURCE Source, int bUseEncryption, [MarshalAs(UnmanagedType.LPTStr)] string lpszUser, [MarshalAs(UnmanagedType.LPTStr)] string lpszOwner, uint Permissions, int UseStrongEncryption)
        => _DTWAIN_SetPDFEncryption(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption);

        public  int DTWAIN_SetPDFEncryptionA(DTWAIN_SOURCE Source, int bUseEncryption, [MarshalAs(UnmanagedType.LPStr)] string lpszUser, [MarshalAs(UnmanagedType.LPStr)] string lpszOwner, uint Permissions, int UseStrongEncryption)
        => _DTWAIN_SetPDFEncryptionA(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption);

        public  int DTWAIN_SetPDFEncryptionW(DTWAIN_SOURCE Source, int bUseEncryption, [MarshalAs(UnmanagedType.LPWStr)] string lpszUser, [MarshalAs(UnmanagedType.LPWStr)] string lpszOwner, uint Permissions, int UseStrongEncryption)
        => _DTWAIN_SetPDFEncryptionW(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption);

        public  int DTWAIN_SetPDFJpegQuality(DTWAIN_SOURCE Source, int Quality)
        => _DTWAIN_SetPDFJpegQuality(Source, Quality);

        public  int DTWAIN_SetPDFKeywords(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpKeyWords)
        => _DTWAIN_SetPDFKeywords(Source, lpKeyWords);

        public  int DTWAIN_SetPDFKeywordsA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpKeyWords)
        => _DTWAIN_SetPDFKeywordsA(Source, lpKeyWords);

        public  int DTWAIN_SetPDFKeywordsW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpKeyWords)
        => _DTWAIN_SetPDFKeywordsW(Source, lpKeyWords);

        public  int DTWAIN_SetPDFOCRConversion(DTWAIN_OCRENGINE Engine, int PageType, int FileType, int PixelType, int BitDepth, int Options)
        => _DTWAIN_SetPDFOCRConversion(Engine, PageType, FileType, PixelType, BitDepth, Options);

        public  int DTWAIN_SetPDFOCRMode(DTWAIN_SOURCE Source, int bSet)
        => _DTWAIN_SetPDFOCRMode(Source, bSet);

        public  int DTWAIN_SetPDFOrientation(DTWAIN_SOURCE Source, int lPOrientation)
        => _DTWAIN_SetPDFOrientation(Source, lPOrientation);

        public  int DTWAIN_SetPDFPageScale(DTWAIN_SOURCE Source, int nOptions, DTWAIN_FLOAT xScale, DTWAIN_FLOAT yScale)
        => _DTWAIN_SetPDFPageScale(Source, nOptions, xScale, yScale);

        public  int DTWAIN_SetPDFPageScaleString(DTWAIN_SOURCE Source, int nOptions, [MarshalAs(UnmanagedType.LPTStr)] string xScale, [MarshalAs(UnmanagedType.LPTStr)] string yScale)
        => _DTWAIN_SetPDFPageScaleString(Source, nOptions, xScale, yScale);

        public  int DTWAIN_SetPDFPageScaleStringA(DTWAIN_SOURCE Source, int nOptions, [MarshalAs(UnmanagedType.LPStr)] string xScale, [MarshalAs(UnmanagedType.LPStr)] string yScale)
        => _DTWAIN_SetPDFPageScaleStringA(Source, nOptions, xScale, yScale);

        public  int DTWAIN_SetPDFPageScaleStringW(DTWAIN_SOURCE Source, int nOptions, [MarshalAs(UnmanagedType.LPWStr)] string xScale, [MarshalAs(UnmanagedType.LPWStr)] string yScale)
        => _DTWAIN_SetPDFPageScaleStringW(Source, nOptions, xScale, yScale);

        public  int DTWAIN_SetPDFPageSize(DTWAIN_SOURCE Source, int PageSize, DTWAIN_FLOAT CustomWidth, DTWAIN_FLOAT CustomHeight)
        => _DTWAIN_SetPDFPageSize(Source, PageSize, CustomWidth, CustomHeight);

        public  int DTWAIN_SetPDFPageSizeString(DTWAIN_SOURCE Source, int PageSize, [MarshalAs(UnmanagedType.LPTStr)] string CustomWidth, [MarshalAs(UnmanagedType.LPTStr)] string CustomHeight)
        => _DTWAIN_SetPDFPageSizeString(Source, PageSize, CustomWidth, CustomHeight);

        public  int DTWAIN_SetPDFPageSizeStringA(DTWAIN_SOURCE Source, int PageSize, [MarshalAs(UnmanagedType.LPStr)] string CustomWidth, [MarshalAs(UnmanagedType.LPStr)] string CustomHeight)
        => _DTWAIN_SetPDFPageSizeStringA(Source, PageSize, CustomWidth, CustomHeight);

        public  int DTWAIN_SetPDFPageSizeStringW(DTWAIN_SOURCE Source, int PageSize, [MarshalAs(UnmanagedType.LPWStr)] string CustomWidth, [MarshalAs(UnmanagedType.LPWStr)] string CustomHeight)
        => _DTWAIN_SetPDFPageSizeStringW(Source, PageSize, CustomWidth, CustomHeight);

        public  int DTWAIN_SetPDFPolarity(DTWAIN_SOURCE Source, int Polarity)
        => _DTWAIN_SetPDFPolarity(Source, Polarity);

        public  int DTWAIN_SetPDFProducer(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpProducer)
        => _DTWAIN_SetPDFProducer(Source, lpProducer);

        public  int DTWAIN_SetPDFProducerA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpProducer)
        => _DTWAIN_SetPDFProducerA(Source, lpProducer);

        public  int DTWAIN_SetPDFProducerW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpProducer)
        => _DTWAIN_SetPDFProducerW(Source, lpProducer);

        public  int DTWAIN_SetPDFSubject(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpSubject)
        => _DTWAIN_SetPDFSubject(Source, lpSubject);

        public  int DTWAIN_SetPDFSubjectA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpSubject)
        => _DTWAIN_SetPDFSubjectA(Source, lpSubject);

        public  int DTWAIN_SetPDFSubjectW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpSubject)
        => _DTWAIN_SetPDFSubjectW(Source, lpSubject);

        public  int DTWAIN_SetPDFTextElementFloat(DTWAIN_PDFTEXTELEMENT TextElement, DTWAIN_FLOAT val1, DTWAIN_FLOAT val2, int Flags)
        => _DTWAIN_SetPDFTextElementFloat(TextElement, val1, val2, Flags);

        public  int DTWAIN_SetPDFTextElementFloatString(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPTStr)] string val1, [MarshalAs(UnmanagedType.LPTStr)] string val2, int Flags)
        => _DTWAIN_SetPDFTextElementFloatString(TextElement, val1, val2, Flags);

        public  int DTWAIN_SetPDFTextElementFloatStringA(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPStr)] string val1, [MarshalAs(UnmanagedType.LPStr)] string val2, int Flags)
        => _DTWAIN_SetPDFTextElementFloatStringA(TextElement, val1, val2, Flags);

        public  int DTWAIN_SetPDFTextElementFloatStringW(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPWStr)] string val1, [MarshalAs(UnmanagedType.LPWStr)] string val2, int Flags)
        => _DTWAIN_SetPDFTextElementFloatStringW(TextElement, val1, val2, Flags);

        public  int DTWAIN_SetPDFTextElementLong(DTWAIN_PDFTEXTELEMENT TextElement, int val1, int val2, int Flags)
        => _DTWAIN_SetPDFTextElementLong(TextElement, val1, val2, Flags);

        public  int DTWAIN_SetPDFTextElementString(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPTStr)] string val1, int Flags)
        => _DTWAIN_SetPDFTextElementString(TextElement, val1, Flags);

        public  int DTWAIN_SetPDFTextElementStringA(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPStr)] string szString, int Flags)
        => _DTWAIN_SetPDFTextElementStringA(TextElement, szString, Flags);

        public  int DTWAIN_SetPDFTextElementStringW(DTWAIN_PDFTEXTELEMENT TextElement, [MarshalAs(UnmanagedType.LPWStr)] string szString, int Flags)
        => _DTWAIN_SetPDFTextElementStringW(TextElement, szString, Flags);

        public  int DTWAIN_SetPDFTitle(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string lpTitle)
        => _DTWAIN_SetPDFTitle(Source, lpTitle);

        public  int DTWAIN_SetPDFTitleA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string lpTitle)
        => _DTWAIN_SetPDFTitleA(Source, lpTitle);

        public  int DTWAIN_SetPDFTitleW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string lpTitle)
        => _DTWAIN_SetPDFTitleW(Source, lpTitle);

        public  int DTWAIN_SetPaperSize(DTWAIN_SOURCE Source, int PaperSize, int bSetCurrent)
        => _DTWAIN_SetPaperSize(Source, PaperSize, bSetCurrent);

        public  int DTWAIN_SetPatchMaxPriorities(DTWAIN_SOURCE Source, int nMaxSearchRetries)
        => _DTWAIN_SetPatchMaxPriorities(Source, nMaxSearchRetries);

        public  int DTWAIN_SetPatchMaxRetries(DTWAIN_SOURCE Source, int nMaxRetries)
        => _DTWAIN_SetPatchMaxRetries(Source, nMaxRetries);

        public  int DTWAIN_SetPatchPriorities(DTWAIN_SOURCE Source, DTWAIN_ARRAY SearchPriorities)
        => _DTWAIN_SetPatchPriorities(Source, SearchPriorities);

        public  int DTWAIN_SetPatchSearchMode(DTWAIN_SOURCE Source, int nSearchMode)
        => _DTWAIN_SetPatchSearchMode(Source, nSearchMode);

        public  int DTWAIN_SetPatchTimeOut(DTWAIN_SOURCE Source, int TimeOutValue)
        => _DTWAIN_SetPatchTimeOut(Source, TimeOutValue);

        public  int DTWAIN_SetPixelFlavor(DTWAIN_SOURCE Source, int PixelFlavor)
        => _DTWAIN_SetPixelFlavor(Source, PixelFlavor);

        public  int DTWAIN_SetPixelType(DTWAIN_SOURCE Source, int PixelType, int BitDepth, int bSetCurrent)
        => _DTWAIN_SetPixelType(Source, PixelType, BitDepth, bSetCurrent);

        public  int DTWAIN_SetPostScriptTitle(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string szTitle)
        => _DTWAIN_SetPostScriptTitle(Source, szTitle);

        public  int DTWAIN_SetPostScriptTitleA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string szTitle)
        => _DTWAIN_SetPostScriptTitleA(Source, szTitle);

        public  int DTWAIN_SetPostScriptTitleW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string szTitle)
        => _DTWAIN_SetPostScriptTitleW(Source, szTitle);

        public  int DTWAIN_SetPostScriptType(DTWAIN_SOURCE Source, int PSType)
        => _DTWAIN_SetPostScriptType(Source, PSType);

        public  int DTWAIN_SetPrinter(DTWAIN_SOURCE Source, int Printer, int bCurrent)
        => _DTWAIN_SetPrinter(Source, Printer, bCurrent);

        public  int DTWAIN_SetPrinterEx(DTWAIN_SOURCE Source, int Printer, int bCurrent)
        => _DTWAIN_SetPrinterEx(Source, Printer, bCurrent);

        public  int DTWAIN_SetPrinterStartNumber(DTWAIN_SOURCE Source, int nStart)
        => _DTWAIN_SetPrinterStartNumber(Source, nStart);

        public  int DTWAIN_SetPrinterStringMode(DTWAIN_SOURCE Source, int PrinterMode, int bSetCurrent)
        => _DTWAIN_SetPrinterStringMode(Source, PrinterMode, bSetCurrent);

        public  int DTWAIN_SetPrinterStrings(DTWAIN_SOURCE Source, DTWAIN_ARRAY ArrayString, ref int pNumStrings)
        => _DTWAIN_SetPrinterStrings(Source, ArrayString, ref pNumStrings);

        public  int DTWAIN_SetPrinterSuffixString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Suffix)
        => _DTWAIN_SetPrinterSuffixString(Source, Suffix);

        public  int DTWAIN_SetPrinterSuffixStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Suffix)
        => _DTWAIN_SetPrinterSuffixStringA(Source, Suffix);

        public  int DTWAIN_SetPrinterSuffixStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Suffix)
        => _DTWAIN_SetPrinterSuffixStringW(Source, Suffix);

        public  int DTWAIN_SetQueryCapSupport(int bSet)
        => _DTWAIN_SetQueryCapSupport(bSet);

        public  int DTWAIN_SetResolution(DTWAIN_SOURCE Source, DTWAIN_FLOAT Resolution)
        => _DTWAIN_SetResolution(Source, Resolution);

        public  int DTWAIN_SetResolutionString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Resolution)
        => _DTWAIN_SetResolutionString(Source, Resolution);

        public  int DTWAIN_SetResolutionStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Resolution)
        => _DTWAIN_SetResolutionStringA(Source, Resolution);

        public  int DTWAIN_SetResolutionStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Resolution)
        => _DTWAIN_SetResolutionStringW(Source, Resolution);

        public  int DTWAIN_SetResourcePath([MarshalAs(UnmanagedType.LPTStr)] string ResourcePath)
        => _DTWAIN_SetResourcePath(ResourcePath);

        public  int DTWAIN_SetResourcePathA([MarshalAs(UnmanagedType.LPStr)] string ResourcePath)
        => _DTWAIN_SetResourcePathA(ResourcePath);

        public  int DTWAIN_SetResourcePathW([MarshalAs(UnmanagedType.LPWStr)] string ResourcePath)
        => _DTWAIN_SetResourcePathW(ResourcePath);

        public  int DTWAIN_SetRotation(DTWAIN_SOURCE Source, DTWAIN_FLOAT Rotation)
        => _DTWAIN_SetRotation(Source, Rotation);

        public  int DTWAIN_SetRotationString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Rotation)
        => _DTWAIN_SetRotationString(Source, Rotation);

        public  int DTWAIN_SetRotationStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Rotation)
        => _DTWAIN_SetRotationStringA(Source, Rotation);

        public  int DTWAIN_SetRotationStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Rotation)
        => _DTWAIN_SetRotationStringW(Source, Rotation);

        public  int DTWAIN_SetSaveFileName(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string fName)
        => _DTWAIN_SetSaveFileName(Source, fName);

        public  int DTWAIN_SetSaveFileNameA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string fName)
        => _DTWAIN_SetSaveFileNameA(Source, fName);

        public  int DTWAIN_SetSaveFileNameW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string fName)
        => _DTWAIN_SetSaveFileNameW(Source, fName);

        public  int DTWAIN_SetShadow(DTWAIN_SOURCE Source, DTWAIN_FLOAT Shadow)
        => _DTWAIN_SetShadow(Source, Shadow);

        public  int DTWAIN_SetShadowString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Shadow)
        => _DTWAIN_SetShadowString(Source, Shadow);

        public  int DTWAIN_SetShadowStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Shadow)
        => _DTWAIN_SetShadowStringA(Source, Shadow);

        public  int DTWAIN_SetShadowStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Shadow)
        => _DTWAIN_SetShadowStringW(Source, Shadow);

        public  int DTWAIN_SetSourceUnit(DTWAIN_SOURCE Source, int Unit)
        => _DTWAIN_SetSourceUnit(Source, Unit);

        public  int DTWAIN_SetTIFFCompressType(DTWAIN_SOURCE Source, int Setting)
        => _DTWAIN_SetTIFFCompressType(Source, Setting);

        public  int DTWAIN_SetTIFFInvert(DTWAIN_SOURCE Source, int Setting)
        => _DTWAIN_SetTIFFInvert(Source, Setting);

        public  int DTWAIN_SetTempFileDirectory([MarshalAs(UnmanagedType.LPTStr)] string szFilePath)
        => _DTWAIN_SetTempFileDirectory(szFilePath);

        public  int DTWAIN_SetTempFileDirectoryA([MarshalAs(UnmanagedType.LPStr)] string szFilePath)
        => _DTWAIN_SetTempFileDirectoryA(szFilePath);

        public  int DTWAIN_SetTempFileDirectoryEx([MarshalAs(UnmanagedType.LPTStr)] string szFilePath, int CreationFlags)
        => _DTWAIN_SetTempFileDirectoryEx(szFilePath, CreationFlags);

        public  int DTWAIN_SetTempFileDirectoryExA([MarshalAs(UnmanagedType.LPStr)] string szFilePath, int CreationFlags)
        => _DTWAIN_SetTempFileDirectoryExA(szFilePath, CreationFlags);

        public  int DTWAIN_SetTempFileDirectoryExW([MarshalAs(UnmanagedType.LPWStr)] string szFilePath, int CreationFlags)
        => _DTWAIN_SetTempFileDirectoryExW(szFilePath, CreationFlags);

        public  int DTWAIN_SetTempFileDirectoryW([MarshalAs(UnmanagedType.LPWStr)] string szFilePath)
        => _DTWAIN_SetTempFileDirectoryW(szFilePath);

        public  int DTWAIN_SetThreshold(DTWAIN_SOURCE Source, DTWAIN_FLOAT Threshold, int bSetBithDepthReduction)
        => _DTWAIN_SetThreshold(Source, Threshold, bSetBithDepthReduction);

        public  int DTWAIN_SetThresholdString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Threshold, int bSetBitDepthReduction)
        => _DTWAIN_SetThresholdString(Source, Threshold, bSetBitDepthReduction);

        public  int DTWAIN_SetThresholdStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Threshold, int bSetBitDepthReduction)
        => _DTWAIN_SetThresholdStringA(Source, Threshold, bSetBitDepthReduction);

        public  int DTWAIN_SetThresholdStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Threshold, int bSetBitDepthReduction)
        => _DTWAIN_SetThresholdStringW(Source, Threshold, bSetBitDepthReduction);

        public  int DTWAIN_SetTwainDSM(int DSMType)
        => _DTWAIN_SetTwainDSM(DSMType);

        public  int DTWAIN_SetTwainLog(uint LogFlags, [MarshalAs(UnmanagedType.LPTStr)] string lpszLogFile)
        => _DTWAIN_SetTwainLog(LogFlags, lpszLogFile);

        public  int DTWAIN_SetTwainLogA(uint LogFlags, [MarshalAs(UnmanagedType.LPStr)] string lpszLogFile)
        => _DTWAIN_SetTwainLogA(LogFlags, lpszLogFile);

        public  int DTWAIN_SetTwainLogW(uint LogFlags, [MarshalAs(UnmanagedType.LPWStr)] string lpszLogFile)
        => _DTWAIN_SetTwainLogW(LogFlags, lpszLogFile);

        public  int DTWAIN_SetTwainMode(int lAcquireMode)
        => _DTWAIN_SetTwainMode(lAcquireMode);

        public  int DTWAIN_SetTwainTimeout(int milliseconds)
        => _DTWAIN_SetTwainTimeout(milliseconds);

        public  DTwainDIBUpdateProc DTWAIN_SetUpdateDibProc(DTwainDIBUpdateProc DibProc)
        => _DTWAIN_SetUpdateDibProc(DibProc);

        public  int DTWAIN_SetXResolution(DTWAIN_SOURCE Source, DTWAIN_FLOAT xResolution)
        => _DTWAIN_SetXResolution(Source, xResolution);

        public  int DTWAIN_SetXResolutionString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Resolution)
        => _DTWAIN_SetXResolutionString(Source, Resolution);

        public  int DTWAIN_SetXResolutionStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Resolution)
        => _DTWAIN_SetXResolutionStringA(Source, Resolution);

        public  int DTWAIN_SetXResolutionStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Resolution)
        => _DTWAIN_SetXResolutionStringW(Source, Resolution);

        public  int DTWAIN_SetYResolution(DTWAIN_SOURCE Source, DTWAIN_FLOAT yResolution)
        => _DTWAIN_SetYResolution(Source, yResolution);

        public  int DTWAIN_SetYResolutionString(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPTStr)] string Resolution)
        => _DTWAIN_SetYResolutionString(Source, Resolution);

        public  int DTWAIN_SetYResolutionStringA(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPStr)] string Resolution)
        => _DTWAIN_SetYResolutionStringA(Source, Resolution);

        public  int DTWAIN_SetYResolutionStringW(DTWAIN_SOURCE Source, [MarshalAs(UnmanagedType.LPWStr)] string Resolution)
        => _DTWAIN_SetYResolutionStringW(Source, Resolution);

        public  int DTWAIN_ShowUIOnly(DTWAIN_SOURCE Source)
        => _DTWAIN_ShowUIOnly(Source);

        public  int DTWAIN_ShutdownOCREngine(DTWAIN_OCRENGINE OCREngine)
        => _DTWAIN_ShutdownOCREngine(OCREngine);

        public  int DTWAIN_SkipImageInfoError(DTWAIN_SOURCE Source, int bSkip)
        => _DTWAIN_SkipImageInfoError(Source, bSkip);

        public  int DTWAIN_StartThread(DTWAIN_HANDLE DLLHandle)
        => _DTWAIN_StartThread(DLLHandle);

        public  int DTWAIN_StartTwainSession(HWND hWndMsg, [MarshalAs(UnmanagedType.LPTStr)] string lpszDLLName)
        => _DTWAIN_StartTwainSession(hWndMsg, lpszDLLName);

        public  int DTWAIN_StartTwainSessionA(HWND hWndMsg, [MarshalAs(UnmanagedType.LPStr)] string lpszDLLName)
        => _DTWAIN_StartTwainSessionA(hWndMsg, lpszDLLName);

        public  int DTWAIN_StartTwainSessionW(HWND hWndMsg, [MarshalAs(UnmanagedType.LPWStr)] string lpszDLLName)
        => _DTWAIN_StartTwainSessionW(hWndMsg, lpszDLLName);

        public  int DTWAIN_SysDestroy()
        => _DTWAIN_SysDestroy();

        public  DTWAIN_HANDLE DTWAIN_SysInitialize()
        => _DTWAIN_SysInitialize();

        public  DTWAIN_HANDLE DTWAIN_SysInitializeEx([MarshalAs(UnmanagedType.LPTStr)] string szINIPath)
        => _DTWAIN_SysInitializeEx(szINIPath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeEx2([MarshalAs(UnmanagedType.LPTStr)] string szINIPath, [MarshalAs(UnmanagedType.LPTStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPTStr)] string szLangResourcePath)
        => _DTWAIN_SysInitializeEx2(szINIPath, szImageDLLPath, szLangResourcePath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeEx2A([MarshalAs(UnmanagedType.LPStr)] string szINIPath, [MarshalAs(UnmanagedType.LPStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPStr)] string szLangResourcePath)
        => _DTWAIN_SysInitializeEx2A(szINIPath, szImageDLLPath, szLangResourcePath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeEx2W([MarshalAs(UnmanagedType.LPWStr)] string szINIPath, [MarshalAs(UnmanagedType.LPWStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPWStr)] string szLangResourcePath)
        => _DTWAIN_SysInitializeEx2W(szINIPath, szImageDLLPath, szLangResourcePath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeExA([MarshalAs(UnmanagedType.LPStr)] string szINIPath)
        => _DTWAIN_SysInitializeExA(szINIPath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeExW([MarshalAs(UnmanagedType.LPWStr)] string szINIPath)
        => _DTWAIN_SysInitializeExW(szINIPath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeLib(HINSTANCE hInstance)
        => _DTWAIN_SysInitializeLib(hInstance);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeLibEx(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPTStr)] string szINIPath)
        => _DTWAIN_SysInitializeLibEx(hInstance, szINIPath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeLibEx2(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPTStr)] string szINIPath, [MarshalAs(UnmanagedType.LPTStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPTStr)] string szLangResourcePath)
        => _DTWAIN_SysInitializeLibEx2(hInstance, szINIPath, szImageDLLPath, szLangResourcePath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeLibEx2A(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPStr)] string szINIPath, [MarshalAs(UnmanagedType.LPStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPStr)] string szLangResourcePath)
        => _DTWAIN_SysInitializeLibEx2A(hInstance, szINIPath, szImageDLLPath, szLangResourcePath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeLibEx2W(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPWStr)] string szINIPath, [MarshalAs(UnmanagedType.LPWStr)] string szImageDLLPath, [MarshalAs(UnmanagedType.LPWStr)] string szLangResourcePath)
        => _DTWAIN_SysInitializeLibEx2W(hInstance, szINIPath, szImageDLLPath, szLangResourcePath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeLibExA(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPStr)] string szINIPath)
        => _DTWAIN_SysInitializeLibExA(hInstance, szINIPath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeLibExW(HINSTANCE hInstance, [MarshalAs(UnmanagedType.LPWStr)] string szINIPath)
        => _DTWAIN_SysInitializeLibExW(hInstance, szINIPath);

        public  DTWAIN_HANDLE DTWAIN_SysInitializeNoBlocking()
        => _DTWAIN_SysInitializeNoBlocking();

        public  DTWAIN_ARRAY DTWAIN_TestGetCap(DTWAIN_SOURCE Source, int lCapability)
        => _DTWAIN_TestGetCap(Source, lCapability);

        public  int DTWAIN_UnlockMemory(HANDLE h)
        => _DTWAIN_UnlockMemory(h);

        public  int DTWAIN_UnlockMemoryEx(HANDLE h)
        => _DTWAIN_UnlockMemoryEx(h);

        public  int DTWAIN_UseMultipleThreads(int bSet)
        => _DTWAIN_UseMultipleThreads(bSet);
    }
}
