using System;
using System.ComponentModel;
using System.Windows.Forms;
using Dynarithmic;
using System.Text;

// For 32-bit apps, use these definitions
using DTWAIN_ARRAY = System.IntPtr;
using DTWAIN_BOOL = System.Int32;
using DTWAIN_FLOAT = System.Double;
using DTWAIN_FRAME = System.IntPtr;
using HANDLE = System.IntPtr;
using DTWAIN_IDENTITY = System.Int32;
using DTWAIN_OCRENGINE = System.IntPtr;
using DTWAIN_LONG = System.Int32;
using DTWAIN_LONG64 = System.Int64;
using DTWAIN_PDFTEXTELEMENT = System.IntPtr;
using DTWAIN_RANGE = System.IntPtr;
using DTWAIN_SOURCE = System.IntPtr;
using DTWAIN_HANDLE = System.IntPtr;
using FullDemo;
using System.Collections.Generic;

namespace TWAINDemo
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class DTwainDemo : System.Windows.Forms.Form
    {
        private System.Windows.Forms.MenuItem SelectSource;
        private System.Windows.Forms.MenuItem SelectSourceByNameBox;
        private IContainer components;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItem;
        private MenuItem SelectDefaultSource;
        private MenuItem SelectSourceCustom;
        private MenuItem menuItem3;
        private MenuItem SourceProperties;
        private MenuItem menuItem5;
        private MenuItem CloseSource;
        private MenuItem menuItem7;
        private MenuItem ExitDemo;

        private DTWAIN_SOURCE SelectedSource = IntPtr.Zero;
        private MenuItem menuItem1;
        private MenuItem AcquireNative;
        private MenuItem AcquireBuffered;
        private MenuItem AcquireFile;
        private MenuItem menuItem4;
        private MenuItem UseSourceUI;
        private MenuItem DiscardBlankPages;
        private MenuItem TwainLogging;
        private MenuItem LoggingOptions;
        private MenuItem AcquireFileUsingDevice;
        private MenuItem menuItem2;
        private MenuItem About;
        private MenuItem menuItem6;
        private MenuItem idlang_dutch;
        private MenuItem idlang_english;
        private MenuItem idlang_french;
        private MenuItem idlang_german;
        private MenuItem idlang_italian;
        private MenuItem idlang_portuguese;
        private MenuItem idlang_romanian;
        private MenuItem idlang_russian;
        private MenuItem idlang_simplified_chinese;
        private MenuItem idlang_spanish;
        private MenuItem idlang_custom;
        private String sOrigTitle;
        private Boolean initialized;
        private TwainAPI.DTwainCallback theCallback;
        private DTWAIN_PDFTEXTELEMENT textElement;
        private MenuItem idlang_greek;
        private MenuItem idlang_korean;
        private MenuItem idlang_japanese;
        private MenuItem idlang_traditional_chinese;
        private MenuItem ShowPreview;
        private MenuItem ShowBarcodeInfo;
        private MenuItem menuItem8;
        private MenuItem BigTiff_NoCompression;
        private MenuItem BigTiff_Group3;
        private MenuItem BigTiff_Group4;
        private MenuItem BigTiff_Flate;
        private MenuItem BigTiff_JPEG;
        private MenuItem BigTiff_LZW;
        private MenuItem BigTiff_Packbits;
        private MenuItem BMP;
        private MenuItem BMP_RLE;
        private MenuItem DCX;
        private MenuItem EMF;
        private MenuItem GIF;
        private MenuItem ICO;
        private MenuItem ICOV;
        private MenuItem JPEG;
        private MenuItem JPEG2000;
        private MenuItem JPEGXL;
        private MenuItem JPEGXR;
        private MenuItem PSD;
        private MenuItem PCX;
        private MenuItem PDF;
        private MenuItem PDF_NoEncryption;
        private MenuItem PDF_ASCII85;
        private MenuItem PDF_40BitRC4_Encryption;
        private MenuItem PDF_128BitRC4_Encryption;
        private MenuItem PDF_128BitAES_Encryption;
        private MenuItem PDF_256BitAES_Encryption;
        private int pdf_page_count = 1;
        private bool stamp_pdf = false;
        private const int PDF_NoEncryption_Value = 0;
        private const int PDF_40BitRC4_Encryption_Value = 1;
        private const int PDF_128BitRC4_Encryption_Value = 2;
        private const int PDF_128BitAES_Encryption_Value = 3;
        private const int PDF_256BitAES_Encryption_Value = 4;
        private MenuItem PNG;
        private MenuItem PostScript_Level1;
        private MenuItem PostScript_Level2;
        private MenuItem PostScript_Level3;
        private MenuItem SVG;
        private MenuItem SVGZ;
        private MenuItem TGA;
        private MenuItem TGARLE;
        private MenuItem TEXTOCR;
        private MenuItem menuItem9;
        private MenuItem Tiff_NoCompression;
        private MenuItem Tiff_Group3;
        private MenuItem Tiff_Group4;
        private MenuItem Tiff_Flate;
        private MenuItem Tiff_JPEG;
        private MenuItem Tiff_LZW;
        private MenuItem Tiff_Packbits;
        private MenuItem WEBP;
        private MenuItem WMF;
        private MenuItem WBMP;
        private MenuItem FF_BMP;
        private const int PDF_ASCII85_Value = 5;
        private MenuItem FF_JFIF;
        private MenuItem FF_TIFF;
        private MenuItem FF_TIFFMULTI;
        private MenuItem FF_PNG;
        private MenuItem FF_PDF;
        private MenuItem FF_PDFA;
        private MenuItem FF_PDFA2;
        private MenuItem FF_PDFRASTER;
        private MenuItem FF_FPX;
        private MenuItem FF_EXIF;
        private MenuItem FF_SPIFF;
        private MenuItem FF_XBM;
        private MenuItem FF_PICT;
        private MenuItem FF_JP2;
        private MenuItem FF_JPX;
        private MenuItem FF_DEJAVU;
        private MenuItem UseGetMessage;
        private MenuItem menuItem10;
        private MenuItem idlang_cantonese;
        private MenuItem idlang_turkish;
        private Dictionary<int, MenuItem> fileDriverMap = new Dictionary<int, MenuItem>();

#if _X64
        public long CallbackProc(long wParam, long lParam, long UserData)
#else
        public int CallbackProc(int wParam, int lParam, int UserData)
        #endif
        {
            switch (wParam)
            {
                case TwainAPI.DTWAIN_TN_ACQUIRESTARTED:
                    pdf_page_count = 1;
                    break;

                // If this is a PDF file this code will put a page stamp on this page 
                case TwainAPI.DTWAIN_TN_FILEPAGESAVING:
                    if (stamp_pdf)
                    {
                        string text = "Page " + pdf_page_count;
                        ++pdf_page_count;
                        TwainAPI.DTWAIN_SetPDFTextElementString(textElement, text, TwainAPI.DTWAIN_PDFTEXTELEMENT_TEXT);
                    }
                    break;

                case TwainAPI.DTWAIN_TN_QUERYPAGEDISCARD:
                    if (ShowPreview.Checked)
                    {
                        DIBDisplayerDlg2 sDIBDlg = new DIBDisplayerDlg2(TwainAPI.DTWAIN_GetCurrentAcquiredImage(SelectedSource));
                        if (sDIBDlg.ShowDialog(this) == DialogResult.Cancel)
                            return 0;
                    }
                    break;

                case TwainAPI.DTWAIN_TN_TRANSFERDONE:
                {
                    if (ShowBarcodeInfo.Checked)
                    {
                        BarCodesDlg sBarcodeDlg = new BarCodesDlg(SelectedSource);
                        sBarcodeDlg.ShowDialog(this);
                    }
                }
                break;
            }
            return 1;
        }

        public DTwainDemo()
        {
            initialized = false;
            InitializeComponent();
            InitializeFileDriverMap();

            sOrigTitle = this.Text;
            DTWAIN_HANDLE handle = TwainAPI.DTWAIN_SysInitialize();
            if (handle == IntPtr.Zero)
            {
                initialized = false;
                return;
            }
            else
            {
                initialized = true;
                SelectedSource = IntPtr.Zero;
                if (TwainAPI.DTWAIN_IsTwainAvailable() == 0)
                {
                    SelectSource.Enabled = false;
                    SelectSourceByNameBox.Enabled = false;
                }
            }
            theCallback += CallbackProc;
            TwainAPI.DTWAIN_EnableMsgNotify(1);
            TwainAPI.DTWAIN_SetCallback(theCallback, 0);
            TwainAPI.DTWAIN_InitOCRInterface();
            textElement = TwainAPI.DTWAIN_CreatePDFTextElement();
            TwainAPI.DTWAIN_SetPDFTextElementLong(textElement, 100, 100, TwainAPI.DTWAIN_PDFTEXTELEMENT_POSITION);
            TwainAPI.DTWAIN_SetPDFTextElementLong(textElement, TwainAPI.DTWAIN_PDFTEXT_ALLPAGES, 0, TwainAPI.DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS);
            TwainAPI.DTWAIN_SetPDFTextElementFloat(textElement, 25, 0, TwainAPI.DTWAIN_PDFTEXTELEMENT_FONTHEIGHT);
        }

        public Boolean InitializedOk() { return initialized; }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            TwainAPI.DTWAIN_SysDestroy();
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem = new System.Windows.Forms.MenuItem();
            this.SelectSource = new System.Windows.Forms.MenuItem();
            this.SelectSourceByNameBox = new System.Windows.Forms.MenuItem();
            this.SelectDefaultSource = new System.Windows.Forms.MenuItem();
            this.SelectSourceCustom = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.SourceProperties = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.CloseSource = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.ExitDemo = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.AcquireNative = new System.Windows.Forms.MenuItem();
            this.AcquireBuffered = new System.Windows.Forms.MenuItem();
            this.AcquireFile = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.BigTiff_NoCompression = new System.Windows.Forms.MenuItem();
            this.BigTiff_Group3 = new System.Windows.Forms.MenuItem();
            this.BigTiff_Group4 = new System.Windows.Forms.MenuItem();
            this.BigTiff_Flate = new System.Windows.Forms.MenuItem();
            this.BigTiff_JPEG = new System.Windows.Forms.MenuItem();
            this.BigTiff_LZW = new System.Windows.Forms.MenuItem();
            this.BigTiff_Packbits = new System.Windows.Forms.MenuItem();
            this.BMP = new System.Windows.Forms.MenuItem();
            this.BMP_RLE = new System.Windows.Forms.MenuItem();
            this.DCX = new System.Windows.Forms.MenuItem();
            this.EMF = new System.Windows.Forms.MenuItem();
            this.GIF = new System.Windows.Forms.MenuItem();
            this.ICO = new System.Windows.Forms.MenuItem();
            this.ICOV = new System.Windows.Forms.MenuItem();
            this.JPEG = new System.Windows.Forms.MenuItem();
            this.JPEG2000 = new System.Windows.Forms.MenuItem();
            this.JPEGXL = new System.Windows.Forms.MenuItem();
            this.JPEGXR = new System.Windows.Forms.MenuItem();
            this.PSD = new System.Windows.Forms.MenuItem();
            this.PCX = new System.Windows.Forms.MenuItem();
            this.PDF = new System.Windows.Forms.MenuItem();
            this.PDF_NoEncryption = new System.Windows.Forms.MenuItem();
            this.PDF_ASCII85 = new System.Windows.Forms.MenuItem();
            this.PDF_40BitRC4_Encryption = new System.Windows.Forms.MenuItem();
            this.PDF_128BitRC4_Encryption = new System.Windows.Forms.MenuItem();
            this.PDF_128BitAES_Encryption = new System.Windows.Forms.MenuItem();
            this.PDF_256BitAES_Encryption = new System.Windows.Forms.MenuItem();
            this.PNG = new System.Windows.Forms.MenuItem();
            this.PostScript_Level1 = new System.Windows.Forms.MenuItem();
            this.PostScript_Level2 = new System.Windows.Forms.MenuItem();
            this.PostScript_Level3 = new System.Windows.Forms.MenuItem();
            this.SVG = new System.Windows.Forms.MenuItem();
            this.SVGZ = new System.Windows.Forms.MenuItem();
            this.TGA = new System.Windows.Forms.MenuItem();
            this.TGARLE = new System.Windows.Forms.MenuItem();
            this.TEXTOCR = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.Tiff_NoCompression = new System.Windows.Forms.MenuItem();
            this.Tiff_Group3 = new System.Windows.Forms.MenuItem();
            this.Tiff_Group4 = new System.Windows.Forms.MenuItem();
            this.Tiff_Flate = new System.Windows.Forms.MenuItem();
            this.Tiff_JPEG = new System.Windows.Forms.MenuItem();
            this.Tiff_LZW = new System.Windows.Forms.MenuItem();
            this.Tiff_Packbits = new System.Windows.Forms.MenuItem();
            this.WEBP = new System.Windows.Forms.MenuItem();
            this.WMF = new System.Windows.Forms.MenuItem();
            this.WBMP = new System.Windows.Forms.MenuItem();
            this.AcquireFileUsingDevice = new System.Windows.Forms.MenuItem();
            this.FF_BMP = new System.Windows.Forms.MenuItem();
            this.FF_JFIF = new System.Windows.Forms.MenuItem();
            this.FF_TIFF = new System.Windows.Forms.MenuItem();
            this.FF_TIFFMULTI = new System.Windows.Forms.MenuItem();
            this.FF_PNG = new System.Windows.Forms.MenuItem();
            this.FF_PDF = new System.Windows.Forms.MenuItem();
            this.FF_PDFA = new System.Windows.Forms.MenuItem();
            this.FF_PDFA2 = new System.Windows.Forms.MenuItem();
            this.FF_PDFRASTER = new System.Windows.Forms.MenuItem();
            this.FF_FPX = new System.Windows.Forms.MenuItem();
            this.FF_EXIF = new System.Windows.Forms.MenuItem();
            this.FF_SPIFF = new System.Windows.Forms.MenuItem();
            this.FF_XBM = new System.Windows.Forms.MenuItem();
            this.FF_PICT = new System.Windows.Forms.MenuItem();
            this.FF_JP2 = new System.Windows.Forms.MenuItem();
            this.FF_JPX = new System.Windows.Forms.MenuItem();
            this.FF_DEJAVU = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.ShowPreview = new System.Windows.Forms.MenuItem();
            this.UseSourceUI = new System.Windows.Forms.MenuItem();
            this.DiscardBlankPages = new System.Windows.Forms.MenuItem();
            this.ShowBarcodeInfo = new System.Windows.Forms.MenuItem();
            this.UseGetMessage = new System.Windows.Forms.MenuItem();
            this.TwainLogging = new System.Windows.Forms.MenuItem();
            this.LoggingOptions = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.idlang_dutch = new System.Windows.Forms.MenuItem();
            this.idlang_english = new System.Windows.Forms.MenuItem();
            this.idlang_french = new System.Windows.Forms.MenuItem();
            this.idlang_german = new System.Windows.Forms.MenuItem();
            this.idlang_greek = new System.Windows.Forms.MenuItem();
            this.idlang_italian = new System.Windows.Forms.MenuItem();
            this.idlang_japanese = new System.Windows.Forms.MenuItem();
            this.idlang_korean = new System.Windows.Forms.MenuItem();
            this.idlang_portuguese = new System.Windows.Forms.MenuItem();
            this.idlang_romanian = new System.Windows.Forms.MenuItem();
            this.idlang_russian = new System.Windows.Forms.MenuItem();
            this.idlang_simplified_chinese = new System.Windows.Forms.MenuItem();
            this.idlang_spanish = new System.Windows.Forms.MenuItem();
            this.idlang_traditional_chinese = new System.Windows.Forms.MenuItem();
            this.idlang_custom = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.About = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.idlang_cantonese = new System.Windows.Forms.MenuItem();
            this.idlang_turkish = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem,
            this.menuItem1,
            this.TwainLogging,
            this.menuItem6,
            this.menuItem2});
            // 
            // menuItem
            // 
            this.menuItem.Index = 0;
            this.menuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SelectSource,
            this.SelectSourceByNameBox,
            this.SelectDefaultSource,
            this.SelectSourceCustom,
            this.menuItem3,
            this.SourceProperties,
            this.menuItem5,
            this.CloseSource,
            this.menuItem7,
            this.ExitDemo});
            this.menuItem.Text = "&Source Selection Test";
            // 
            // SelectSource
            // 
            this.SelectSource.Index = 0;
            this.SelectSource.Text = "Select Source...";
            this.SelectSource.Click += new System.EventHandler(this.SelectSource_Click);
            // 
            // SelectSourceByNameBox
            // 
            this.SelectSourceByNameBox.Index = 1;
            this.SelectSourceByNameBox.Text = "Select Source By Name...";
            this.SelectSourceByNameBox.Click += new System.EventHandler(this.SelectSourceByName_Click);
            // 
            // SelectDefaultSource
            // 
            this.SelectDefaultSource.Index = 2;
            this.SelectDefaultSource.Text = "Select Default Source...";
            this.SelectDefaultSource.Click += new System.EventHandler(this.SelectDefaultSource_Click);
            // 
            // SelectSourceCustom
            // 
            this.SelectSourceCustom.Index = 3;
            this.SelectSourceCustom.Text = "Select Source Custom...";
            this.SelectSourceCustom.Click += new System.EventHandler(this.SelectSourceCustom_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 4;
            this.menuItem3.Text = "-";
            // 
            // SourceProperties
            // 
            this.SourceProperties.Index = 5;
            this.SourceProperties.Text = "Source Properties...";
            this.SourceProperties.Click += new System.EventHandler(this.SourceProperties_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 6;
            this.menuItem5.Text = "-";
            // 
            // CloseSource
            // 
            this.CloseSource.Index = 7;
            this.CloseSource.Text = "Close Source...";
            this.CloseSource.Click += new System.EventHandler(this.CloseSource_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 8;
            this.menuItem7.Text = "-";
            // 
            // ExitDemo
            // 
            this.ExitDemo.Index = 9;
            this.ExitDemo.Text = "Exit Demo";
            this.ExitDemo.Click += new System.EventHandler(this.ExitDemo_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.AcquireNative,
            this.AcquireBuffered,
            this.AcquireFile,
            this.AcquireFileUsingDevice,
            this.menuItem4,
            this.ShowPreview,
            this.UseSourceUI,
            this.DiscardBlankPages,
            this.ShowBarcodeInfo,
            this.menuItem10,
            this.UseGetMessage});
            this.menuItem1.Text = "&Acquire Test";
            // 
            // AcquireNative
            // 
            this.AcquireNative.Index = 0;
            this.AcquireNative.Text = "Acquire Native...";
            this.AcquireNative.Click += new System.EventHandler(this.AcquireNative_Click);
            // 
            // AcquireBuffered
            // 
            this.AcquireBuffered.Index = 1;
            this.AcquireBuffered.Text = "Acquire Buffered...";
            this.AcquireBuffered.Click += new System.EventHandler(this.AcquireBuffered_Click);
            // 
            // AcquireFile
            // 
            this.AcquireFile.Index = 2;
            this.AcquireFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem8,
            this.BMP,
            this.BMP_RLE,
            this.DCX,
            this.EMF,
            this.GIF,
            this.ICO,
            this.ICOV,
            this.JPEG,
            this.JPEG2000,
            this.JPEGXL,
            this.JPEGXR,
            this.PSD,
            this.PCX,
            this.PDF,
            this.PNG,
            this.PostScript_Level1,
            this.PostScript_Level2,
            this.PostScript_Level3,
            this.SVG,
            this.SVGZ,
            this.TGA,
            this.TGARLE,
            this.TEXTOCR,
            this.menuItem9,
            this.WEBP,
            this.WMF,
            this.WBMP});
            this.AcquireFile.Text = "Acquire File...";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 0;
            this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.BigTiff_NoCompression,
            this.BigTiff_Group3,
            this.BigTiff_Group4,
            this.BigTiff_Flate,
            this.BigTiff_JPEG,
            this.BigTiff_LZW,
            this.BigTiff_Packbits});
            this.menuItem8.Text = "Big TIFF";
            // 
            // BigTiff_NoCompression
            // 
            this.BigTiff_NoCompression.Index = 0;
            this.BigTiff_NoCompression.Text = "No compression";
            this.BigTiff_NoCompression.Click += new System.EventHandler(this.BigTiff_NoCompression_Click);
            // 
            // BigTiff_Group3
            // 
            this.BigTiff_Group3.Index = 1;
            this.BigTiff_Group3.Text = "Group 3";
            this.BigTiff_Group3.Click += new System.EventHandler(this.BigTiff_Group3_Click);
            // 
            // BigTiff_Group4
            // 
            this.BigTiff_Group4.Index = 2;
            this.BigTiff_Group4.Text = "Group 4";
            this.BigTiff_Group4.Click += new System.EventHandler(this.BigTiff_Group4_Click);
            // 
            // BigTiff_Flate
            // 
            this.BigTiff_Flate.Index = 3;
            this.BigTiff_Flate.Text = "Flate";
            this.BigTiff_Flate.Click += new System.EventHandler(this.BigTiff_Flate_Click);
            // 
            // BigTiff_JPEG
            // 
            this.BigTiff_JPEG.Index = 4;
            this.BigTiff_JPEG.Text = "JPEG";
            this.BigTiff_JPEG.Click += new System.EventHandler(this.BigTiff_JPEG_Click);
            // 
            // BigTiff_LZW
            // 
            this.BigTiff_LZW.Index = 5;
            this.BigTiff_LZW.Text = "LZW";
            this.BigTiff_LZW.Click += new System.EventHandler(this.BigTiff_LZW_Click);
            // 
            // BigTiff_Packbits
            // 
            this.BigTiff_Packbits.Index = 6;
            this.BigTiff_Packbits.Text = "Packbits";
            this.BigTiff_Packbits.Click += new System.EventHandler(this.BigTiff_Packbits_Click);
            // 
            // BMP
            // 
            this.BMP.Index = 1;
            this.BMP.Text = "BMP";
            this.BMP.Click += new System.EventHandler(this.BMP_Click);
            // 
            // BMP_RLE
            // 
            this.BMP_RLE.Index = 2;
            this.BMP_RLE.Text = "BMP RLE";
            this.BMP_RLE.Click += new System.EventHandler(this.BMP_RLE_Click);
            // 
            // DCX
            // 
            this.DCX.Index = 3;
            this.DCX.Text = "DCX";
            this.DCX.Click += new System.EventHandler(this.DCX_Click);
            // 
            // EMF
            // 
            this.EMF.Index = 4;
            this.EMF.Text = "Enhanced Meta File (EMF)";
            this.EMF.Click += new System.EventHandler(this.EMF_Click);
            // 
            // GIF
            // 
            this.GIF.Index = 5;
            this.GIF.Text = "GIF";
            this.GIF.Click += new System.EventHandler(this.GIF_Click);
            // 
            // ICO
            // 
            this.ICO.Index = 6;
            this.ICO.Text = "Windows ICON (ICO)";
            this.ICO.Click += new System.EventHandler(this.ICO_Click);
            // 
            // ICOV
            // 
            this.ICOV.Index = 7;
            this.ICOV.Text = "Windows Vista ICON (ICOV)";
            this.ICOV.Click += new System.EventHandler(this.ICOV_Click);
            // 
            // JPEG
            // 
            this.JPEG.Index = 8;
            this.JPEG.Text = "JPEG";
            this.JPEG.Click += new System.EventHandler(this.JPEG_Click);
            // 
            // JPEG2000
            // 
            this.JPEG2000.Index = 9;
            this.JPEG2000.Text = "JPEG-2000";
            this.JPEG2000.Click += new System.EventHandler(this.JPEG2000_Click);
            // 
            // JPEGXL
            // 
            this.JPEGXL.Index = 10;
            this.JPEGXL.Text = "JPEG-XL";
            this.JPEGXL.Click += new System.EventHandler(this.JPEGXL_Click);
            // 
            // JPEGXR
            // 
            this.JPEGXR.Index = 11;
            this.JPEGXR.Text = "JPEG-XR";
            this.JPEGXR.Click += new System.EventHandler(this.JPEGXR_Click);
            // 
            // PSD
            // 
            this.PSD.Index = 12;
            this.PSD.Text = "Paintshop (PSD)";
            this.PSD.Click += new System.EventHandler(this.PSD_Click);
            // 
            // PCX
            // 
            this.PCX.Index = 13;
            this.PCX.Text = "PCX";
            this.PCX.Click += new System.EventHandler(this.PCX_Click);
            // 
            // PDF
            // 
            this.PDF.Index = 14;
            this.PDF.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.PDF_NoEncryption,
            this.PDF_ASCII85,
            this.PDF_40BitRC4_Encryption,
            this.PDF_128BitRC4_Encryption,
            this.PDF_128BitAES_Encryption,
            this.PDF_256BitAES_Encryption});
            this.PDF.Text = "PDF";
            // 
            // PDF_NoEncryption
            // 
            this.PDF_NoEncryption.Index = 0;
            this.PDF_NoEncryption.Text = "No Encryption";
            this.PDF_NoEncryption.Click += new System.EventHandler(this.PDF_NoEncryption_Click);
            // 
            // PDF_ASCII85
            // 
            this.PDF_ASCII85.Index = 1;
            this.PDF_ASCII85.Text = "ASCII85 Encoding";
            this.PDF_ASCII85.Click += new System.EventHandler(this.PDF_ASCII85_Click);
            // 
            // PDF_40BitRC4_Encryption
            // 
            this.PDF_40BitRC4_Encryption.Index = 2;
            this.PDF_40BitRC4_Encryption.Text = "40-bit RC4 Encryption";
            this.PDF_40BitRC4_Encryption.Click += new System.EventHandler(this.PDF_40BitRC4_Encryption_Click);
            // 
            // PDF_128BitRC4_Encryption
            // 
            this.PDF_128BitRC4_Encryption.Index = 3;
            this.PDF_128BitRC4_Encryption.Text = "128-bit RC4 Encryption";
            this.PDF_128BitRC4_Encryption.Click += new System.EventHandler(this.PDF_128BitRC4_Encryption_Click);
            // 
            // PDF_128BitAES_Encryption
            // 
            this.PDF_128BitAES_Encryption.Index = 4;
            this.PDF_128BitAES_Encryption.Text = "128-bit AES Encryption";
            this.PDF_128BitAES_Encryption.Click += new System.EventHandler(this.PDF_128BitAES_Encryption_Click);
            // 
            // PDF_256BitAES_Encryption
            // 
            this.PDF_256BitAES_Encryption.Index = 5;
            this.PDF_256BitAES_Encryption.Text = "256-bit AES Encryption";
            this.PDF_256BitAES_Encryption.Click += new System.EventHandler(this.PDF_256BitAES_Encryption_Click);
            // 
            // PNG
            // 
            this.PNG.Index = 15;
            this.PNG.Text = "PNG";
            this.PNG.Click += new System.EventHandler(this.PNG_Click);
            // 
            // PostScript_Level1
            // 
            this.PostScript_Level1.Index = 16;
            this.PostScript_Level1.Text = "PostScript (Level 1)";
            this.PostScript_Level1.Click += new System.EventHandler(this.PostScript_Level1_Click);
            // 
            // PostScript_Level2
            // 
            this.PostScript_Level2.Index = 17;
            this.PostScript_Level2.Text = "PostScript (Level 2)";
            this.PostScript_Level2.Click += new System.EventHandler(this.PostScript_Level2_Click);
            // 
            // PostScript_Level3
            // 
            this.PostScript_Level3.Index = 18;
            this.PostScript_Level3.Text = "PostScript (Level 3)";
            this.PostScript_Level3.Click += new System.EventHandler(this.PostScript_Level3_Click);
            // 
            // SVG
            // 
            this.SVG.Index = 19;
            this.SVG.Text = "SVG";
            this.SVG.Click += new System.EventHandler(this.SVG_Click);
            // 
            // SVGZ
            // 
            this.SVGZ.Index = 20;
            this.SVGZ.Text = "SVGZ";
            this.SVGZ.Click += new System.EventHandler(this.SVGZ_Click);
            // 
            // TGA
            // 
            this.TGA.Index = 21;
            this.TGA.Text = "Targa (TGA)";
            this.TGA.Click += new System.EventHandler(this.TGA_Click);
            // 
            // TGARLE
            // 
            this.TGARLE.Index = 22;
            this.TGARLE.Text = "Targa RLE";
            this.TGARLE.Click += new System.EventHandler(this.TGARLE_Click);
            // 
            // TEXTOCR
            // 
            this.TEXTOCR.Index = 23;
            this.TEXTOCR.Text = "Text (OCR)";
            this.TEXTOCR.Click += new System.EventHandler(this.TEXTOCR_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 24;
            this.menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Tiff_NoCompression,
            this.Tiff_Group3,
            this.Tiff_Group4,
            this.Tiff_Flate,
            this.Tiff_JPEG,
            this.Tiff_LZW,
            this.Tiff_Packbits});
            this.menuItem9.Text = "TIFF";
            // 
            // Tiff_NoCompression
            // 
            this.Tiff_NoCompression.Index = 0;
            this.Tiff_NoCompression.Text = "No Compression";
            this.Tiff_NoCompression.Click += new System.EventHandler(this.Tiff_NoCompression_Click);
            // 
            // Tiff_Group3
            // 
            this.Tiff_Group3.Index = 1;
            this.Tiff_Group3.Text = "Group 3";
            this.Tiff_Group3.Click += new System.EventHandler(this.Tiff_Group3_Click);
            // 
            // Tiff_Group4
            // 
            this.Tiff_Group4.Index = 2;
            this.Tiff_Group4.Text = "Group 4";
            this.Tiff_Group4.Click += new System.EventHandler(this.Tiff_Group4_Click);
            // 
            // Tiff_Flate
            // 
            this.Tiff_Flate.Index = 3;
            this.Tiff_Flate.Text = "Flate";
            this.Tiff_Flate.Click += new System.EventHandler(this.Tiff_Flate_Click);
            // 
            // Tiff_JPEG
            // 
            this.Tiff_JPEG.Index = 4;
            this.Tiff_JPEG.Text = "JPEG";
            this.Tiff_JPEG.Click += new System.EventHandler(this.Tiff_JPEG_Click);
            // 
            // Tiff_LZW
            // 
            this.Tiff_LZW.Index = 5;
            this.Tiff_LZW.Text = "LZW";
            this.Tiff_LZW.Click += new System.EventHandler(this.Tiff_LZW_Click);
            // 
            // Tiff_Packbits
            // 
            this.Tiff_Packbits.Index = 6;
            this.Tiff_Packbits.Text = "Packbits";
            this.Tiff_Packbits.Click += new System.EventHandler(this.Tiff_Packbits_Click);
            // 
            // WEBP
            // 
            this.WEBP.Index = 25;
            this.WEBP.Text = "WebP";
            this.WEBP.Click += new System.EventHandler(this.WEBP_Click);
            // 
            // WMF
            // 
            this.WMF.Index = 26;
            this.WMF.Text = "Windows Meta File (WMF)";
            this.WMF.Click += new System.EventHandler(this.WMF_Click);
            // 
            // WBMP
            // 
            this.WBMP.Index = 27;
            this.WBMP.Text = "Wireless Bitmap (WBMP)";
            this.WBMP.Click += new System.EventHandler(this.WBMP_Click);
            // 
            // AcquireFileUsingDevice
            // 
            this.AcquireFileUsingDevice.Index = 3;
            this.AcquireFileUsingDevice.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FF_BMP,
            this.FF_JFIF,
            this.FF_TIFF,
            this.FF_TIFFMULTI,
            this.FF_PNG,
            this.FF_PDF,
            this.FF_PDFA,
            this.FF_PDFA2,
            this.FF_PDFRASTER,
            this.FF_FPX,
            this.FF_EXIF,
            this.FF_SPIFF,
            this.FF_XBM,
            this.FF_PICT,
            this.FF_JP2,
            this.FF_JPX,
            this.FF_DEJAVU});
            this.AcquireFileUsingDevice.Text = "Acquire File (Use Source File Driver) ...";
            // 
            // FF_BMP
            // 
            this.FF_BMP.Index = 0;
            this.FF_BMP.Tag = "2";
            this.FF_BMP.Text = "Windows BMP";
            this.FF_BMP.Click += new System.EventHandler(this.FF_BMP_Click);
            // 
            // FF_JFIF
            // 
            this.FF_JFIF.Index = 1;
            this.FF_JFIF.Text = "JFIF (JPEG)";
            this.FF_JFIF.Click += new System.EventHandler(this.FF_JFIF_Click);
            // 
            // FF_TIFF
            // 
            this.FF_TIFF.Index = 2;
            this.FF_TIFF.Text = "TIFF";
            this.FF_TIFF.Click += new System.EventHandler(this.FF_TIFF_Click);
            // 
            // FF_TIFFMULTI
            // 
            this.FF_TIFFMULTI.Index = 3;
            this.FF_TIFFMULTI.Text = "TIFF (Multipage)";
            this.FF_TIFFMULTI.Click += new System.EventHandler(this.FF_TIFFMULTI_Click);
            // 
            // FF_PNG
            // 
            this.FF_PNG.Index = 4;
            this.FF_PNG.Text = "PNG";
            this.FF_PNG.Click += new System.EventHandler(this.FF_PNG_Click);
            // 
            // FF_PDF
            // 
            this.FF_PDF.Index = 5;
            this.FF_PDF.Text = "PDF";
            this.FF_PDF.Click += new System.EventHandler(this.FF_PDF_Click);
            // 
            // FF_PDFA
            // 
            this.FF_PDFA.Index = 6;
            this.FF_PDFA.Text = "PDF/A";
            this.FF_PDFA.Click += new System.EventHandler(this.FF_PDFA_Click);
            // 
            // FF_PDFA2
            // 
            this.FF_PDFA2.Index = 7;
            this.FF_PDFA2.Text = "PDF/A2";
            this.FF_PDFA2.Click += new System.EventHandler(this.FF_PDFA2_Click);
            // 
            // FF_PDFRASTER
            // 
            this.FF_PDFRASTER.Index = 8;
            this.FF_PDFRASTER.Text = "PDFRASTER";
            this.FF_PDFRASTER.Click += new System.EventHandler(this.FF_PDFRASTER_Click);
            // 
            // FF_FPX
            // 
            this.FF_FPX.Index = 9;
            this.FF_FPX.Text = "FlashPix (FPX)";
            this.FF_FPX.Click += new System.EventHandler(this.FF_FPX_Click);
            // 
            // FF_EXIF
            // 
            this.FF_EXIF.Index = 10;
            this.FF_EXIF.Text = "EXIF";
            this.FF_EXIF.Click += new System.EventHandler(this.FF_EXIF_Click);
            // 
            // FF_SPIFF
            // 
            this.FF_SPIFF.Index = 11;
            this.FF_SPIFF.Text = "SPIFF";
            this.FF_SPIFF.Click += new System.EventHandler(this.FF_SPIFF_Click);
            // 
            // FF_XBM
            // 
            this.FF_XBM.Index = 12;
            this.FF_XBM.Text = "XBM";
            this.FF_XBM.Click += new System.EventHandler(this.FF_XBM_Click);
            // 
            // FF_PICT
            // 
            this.FF_PICT.Index = 13;
            this.FF_PICT.Text = "Macintosh PICT";
            this.FF_PICT.Click += new System.EventHandler(this.FF_PICT_Click);
            // 
            // FF_JP2
            // 
            this.FF_JP2.Index = 14;
            this.FF_JP2.Text = "JP2";
            this.FF_JP2.Click += new System.EventHandler(this.FF_JP2_Click);
            // 
            // FF_JPX
            // 
            this.FF_JPX.Index = 15;
            this.FF_JPX.Text = "JPX";
            this.FF_JPX.Click += new System.EventHandler(this.FF_JPX_Click);
            // 
            // FF_DEJAVU
            // 
            this.FF_DEJAVU.Index = 16;
            this.FF_DEJAVU.Text = "DEJAVU";
            this.FF_DEJAVU.Click += new System.EventHandler(this.FF_DEJAVU_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Text = "-";
            // 
            // ShowPreview
            // 
            this.ShowPreview.Checked = true;
            this.ShowPreview.Index = 5;
            this.ShowPreview.Text = "Show Preview";
            this.ShowPreview.Click += new System.EventHandler(this.ShowPreview_Click);
            // 
            // UseSourceUI
            // 
            this.UseSourceUI.Checked = true;
            this.UseSourceUI.Index = 6;
            this.UseSourceUI.Text = "Use Source UI";
            this.UseSourceUI.Click += new System.EventHandler(this.UseSourceUI_Click);
            // 
            // DiscardBlankPages
            // 
            this.DiscardBlankPages.Index = 7;
            this.DiscardBlankPages.Text = "Discard Blank Pages";
            this.DiscardBlankPages.Click += new System.EventHandler(this.DiscardBlankPages_Click);
            // 
            // ShowBarcodeInfo
            // 
            this.ShowBarcodeInfo.Index = 8;
            this.ShowBarcodeInfo.Text = "Show Barcode Information";
            this.ShowBarcodeInfo.Click += new System.EventHandler(this.ShowBarcodeInfo_Click);
            // 
            // UseGetMessage
            // 
            this.UseGetMessage.Index = 10;
            this.UseGetMessage.Text = "Use GetMessage TWAIN Loop";
            this.UseGetMessage.Click += new System.EventHandler(this.UseGetMessage_Click);
            // 
            // TwainLogging
            // 
            this.TwainLogging.Index = 2;
            this.TwainLogging.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.LoggingOptions});
            this.TwainLogging.Text = "&TWAIN Logging";
            // 
            // LoggingOptions
            // 
            this.LoggingOptions.Index = 0;
            this.LoggingOptions.Text = "Logging Options...";
            this.LoggingOptions.Click += new System.EventHandler(this.LoggingOptions_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 3;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.idlang_cantonese,
            this.idlang_dutch,
            this.idlang_english,
            this.idlang_french,
            this.idlang_german,
            this.idlang_greek,
            this.idlang_italian,
            this.idlang_japanese,
            this.idlang_korean,
            this.idlang_portuguese,
            this.idlang_romanian,
            this.idlang_russian,
            this.idlang_simplified_chinese,
            this.idlang_spanish,
            this.idlang_traditional_chinese,
            this.idlang_turkish,
            this.idlang_custom});
            this.menuItem6.Text = "Language";
            // 
            // idlang_dutch
            // 
            this.idlang_dutch.Index = 1;
            this.idlang_dutch.Text = "Dutch";
            this.idlang_dutch.Click += new System.EventHandler(this.idlang_dutch_Click);
            // 
            // idlang_english
            // 
            this.idlang_english.Index = 2;
            this.idlang_english.Text = "English";
            this.idlang_english.Click += new System.EventHandler(this.idlang_english_Click);
            // 
            // idlang_french
            // 
            this.idlang_french.Index = 3;
            this.idlang_french.Text = "French";
            this.idlang_french.Click += new System.EventHandler(this.idlang_french_Click);
            // 
            // idlang_german
            // 
            this.idlang_german.Index = 4;
            this.idlang_german.Text = "German";
            this.idlang_german.Click += new System.EventHandler(this.idlang_german_Click);
            // 
            // idlang_greek
            // 
            this.idlang_greek.Index = 5;
            this.idlang_greek.Text = "Greek";
            this.idlang_greek.Click += new System.EventHandler(this.idlang_greek_Click);
            // 
            // idlang_italian
            // 
            this.idlang_italian.Index = 6;
            this.idlang_italian.Text = "Italian";
            this.idlang_italian.Click += new System.EventHandler(this.idlang_italian_Click);
            // 
            // idlang_japanese
            // 
            this.idlang_japanese.Index = 7;
            this.idlang_japanese.Text = "Japanese";
            this.idlang_japanese.Click += new System.EventHandler(this.idlang_japanese_Click);
            // 
            // idlang_korean
            // 
            this.idlang_korean.Index = 8;
            this.idlang_korean.Text = "Korean";
            this.idlang_korean.Click += new System.EventHandler(this.idlang_korean_Click);
            // 
            // idlang_portuguese
            // 
            this.idlang_portuguese.Index = 9;
            this.idlang_portuguese.Text = "Portuguese";
            this.idlang_portuguese.Click += new System.EventHandler(this.idlang_portuguese_Click);
            // 
            // idlang_romanian
            // 
            this.idlang_romanian.Index = 10;
            this.idlang_romanian.Text = "Romanian";
            this.idlang_romanian.Click += new System.EventHandler(this.idlang_romanian_Click);
            // 
            // idlang_russian
            // 
            this.idlang_russian.Index = 11;
            this.idlang_russian.Text = "Russian";
            this.idlang_russian.Click += new System.EventHandler(this.idlang_russian_Click);
            // 
            // idlang_simplified_chinese
            // 
            this.idlang_simplified_chinese.Index = 12;
            this.idlang_simplified_chinese.Text = "Simplified Chinese";
            this.idlang_simplified_chinese.Click += new System.EventHandler(this.idlang_simplified_chinese_Click);
            // 
            // idlang_spanish
            // 
            this.idlang_spanish.Index = 13;
            this.idlang_spanish.Text = "Spanish";
            this.idlang_spanish.Click += new System.EventHandler(this.idlang_spanish_Click);
            // 
            // idlang_traditional_chinese
            // 
            this.idlang_traditional_chinese.Index = 14;
            this.idlang_traditional_chinese.Text = "Traditional Chinese";
            this.idlang_traditional_chinese.Click += new System.EventHandler(this.idlang_traditional_chinese_Click);
            // 
            // idlang_custom
            // 
            this.idlang_custom.Index = 16;
            this.idlang_custom.Text = "Custom Language...";
            this.idlang_custom.Click += new System.EventHandler(this.idlang_custom_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 4;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.About});
            this.menuItem2.Text = "Help";
            // 
            // About
            // 
            this.About.Index = 0;
            this.About.Text = "About...";
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 9;
            this.menuItem10.Text = "-";
            // 
            // idlang_cantonese
            // 
            this.idlang_cantonese.Index = 0;
            this.idlang_cantonese.Text = "Cantonese";
            this.idlang_cantonese.Click += new System.EventHandler(this.idlang_cantonese_Click);
            // 
            // idlang_turkish
            // 
            this.idlang_turkish.Index = 15;
            this.idlang_turkish.Text = "Turkish";
            this.idlang_turkish.Click += new System.EventHandler(this.idlang_turkish_Click);
            // 
            // DTwainDemo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(577, 421);
            this.Menu = this.mainMenu;
            this.Name = "DTwainDemo";
            this.Text = "DTWAIN C# Demo Program";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            DTwainDemo theDemo = new DTwainDemo();
            if (theDemo.InitializedOk())
                Application.Run(theDemo);
        }

        
        private void SelectSource_Click(object sender, System.EventArgs e)
        {
            SelectTheSource(0);
            Focus();
        }

        private void SelectSourceByName_Click(object sender, System.EventArgs e)
        {
            SelectTheSource(1);
            Focus();
        }

        private void SelectDefaultSource_Click(object sender, EventArgs e)
        {
            SelectTheSource(2);
            Focus();
        }

        private void SelectSourceCustom_Click(object sender, EventArgs e)
        {
            SelectTheSource(3);
            Focus();
        }

        private void SelectTheSource(int nWhich)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                DialogResult nReturn;
                nReturn = MessageBox.Show("For this demo, only one Source can be opened.  Close current Source?",
                                                 "DTWAIN Message", MessageBoxButtons.YesNo);
                if (nReturn == DialogResult.Yes)
                {
                    TwainAPI.DTWAIN_CloseSource(SelectedSource);
                    SelectedSource = IntPtr.Zero;
                    // EnableSourceItems( FALSE );
                }
                else
                    return;
            }

            DTWAIN_ARRAY pArray = System.IntPtr.Zero;
            if (TwainAPI.DTWAIN_EnumSources(ref pArray) > 0)
            {
                DTWAIN_SOURCE src = IntPtr.Zero;
                int nSources = TwainAPI.DTWAIN_ArrayGetCount(pArray);
                StringBuilder szBuf = new StringBuilder(256);
                for (int i = 0; i < nSources; ++i)
                {
                    TwainAPI.DTWAIN_ArrayGetSourceAt(pArray, i, ref src);
                    TwainAPI.DTWAIN_GetSourceProductName(src, szBuf, 256);
                    Console.WriteLine("The source name is " + szBuf);
                }
            }


            this.Enabled = false;  // disable the main form
            switch (nWhich)
            {
                case 0:
                    // Select the source
                    SelectedSource = TwainAPI.DTWAIN_SelectSource2(IntPtr.Zero, null, 0,0, 
                            TwainAPI.DTWAIN_DLG_CENTER_CURRENT_MONITOR  | TwainAPI.DTWAIN_DLG_HIGHLIGHTFIRST | TwainAPI.DTWAIN_DLG_SORTNAMES
                            | TwainAPI.DTWAIN_DLG_TOPMOSTWINDOW);
                break;

                case 1:
                    SelectSourceByNameBox objSelectSourceByName = new SelectSourceByNameBox();
                    DialogResult nResult = objSelectSourceByName.ShowDialog(this);
                    if (nResult == DialogResult.OK)
                    {
                        SelectedSource = TwainAPI.DTWAIN_SelectSourceByName(objSelectSourceByName.GetText());
                    }
                break;

                case 2:
                    SelectedSource = TwainAPI.DTWAIN_SelectDefaultSource();
                break;

                case 3:
                    CustomSelectSource customSourceDlg = new CustomSelectSource();
                    DialogResult dResult = customSourceDlg.ShowDialog(this);
                    if (dResult == DialogResult.OK)
                        SelectedSource = TwainAPI.DTWAIN_SelectSourceByName(customSourceDlg.GetSourceName());
                break;
            }
            this.Enabled = true;  // re-enable the main form

            if (SelectedSource != IntPtr.Zero)
            {
                if (TwainAPI.DTWAIN_OpenSource(SelectedSource) != 0)
                {
                    TwainAPI.DTWAIN_EnableBarcodeDetection(SelectedSource, 1);
                    TwainAPI.DTWAIN_AddPDFTextElement(SelectedSource, textElement);
                    TwainAPI.DTWAIN_EnableFeeder(SelectedSource, 1);
                    SetCaptionToSourceName();
                    EnableSourceItems(true);
                    EnableBarcodeItems(true);
                    EnableFileDriverItems(true);
                    SetGetMessageLoopProcessing();
                    return;
                }
                else
                {
                    MessageBox.Show("Error Opening Source", "TWAIN Error", MessageBoxButtons.OK);
                    SetCaptionToSourceName();
                    EnableSourceItems(false);
                    EnableBarcodeItems(false);
                }
            }
            else
            {
                int lastError = TwainAPI.DTWAIN_GetLastError();
                if (lastError == TwainAPI.DTWAIN_ERR_SOURCESELECTION_CANCELED)
                    MessageBox.Show("Source selection canceled", "TWAIN Info", MessageBoxButtons.OK);
                else
                {
                    StringBuilder szErr = new StringBuilder(100);
                    TwainAPI.DTWAIN_GetErrorString(lastError, szErr, 100);
                    MessageBox.Show("Error Selecting and/or opening Source.\r\n" + szErr.ToString(), "TWAIN Error", MessageBoxButtons.OK);
                }
                SetCaptionToSourceName();
                EnableSourceItems(false);
            }
        }
        private void EnableSourceItems(bool bEnable)
        {
            SourceProperties.Enabled = bEnable;
            CloseSource.Enabled = bEnable;
            AcquireNative.Enabled = bEnable;
            AcquireBuffered.Enabled = bEnable;
            AcquireFile.Enabled = bEnable;
            AcquireFileUsingDevice.Enabled = bEnable;
        }

        private void EnableBarcodeItems(bool bEnable)
        {
            int supported = TwainAPI.DTWAIN_IsExtImageInfoSupported(SelectedSource);
            if (bEnable && supported == 1)
                ShowBarcodeInfo.Enabled = true;
            else
            {
                ShowBarcodeInfo.Enabled = false;
                if (supported == 0)
                    ShowBarcodeInfo.Checked = false;
            }
        }

        private void InitializeFileDriverMap()
        {
            fileDriverMap[TwainAPI.DTWAIN_FF_BMP] = FF_BMP;
            fileDriverMap[TwainAPI.DTWAIN_FF_JFIF] = FF_JFIF;
            fileDriverMap[TwainAPI.DTWAIN_FF_TIFF] = FF_TIFF;
            fileDriverMap[TwainAPI.DTWAIN_FF_TIFFMULTI] = FF_TIFFMULTI;
            fileDriverMap[TwainAPI.DTWAIN_FF_PNG] = FF_PNG;
            fileDriverMap[TwainAPI.DTWAIN_FF_PDF] = FF_PDF;
            fileDriverMap[TwainAPI.DTWAIN_FF_PDFA] = FF_PDFA;
            fileDriverMap[TwainAPI.DTWAIN_FF_PDFA2] = FF_PDFA2;
            fileDriverMap[TwainAPI.DTWAIN_FF_PDFRASTER] = FF_PDFRASTER;
            fileDriverMap[TwainAPI.DTWAIN_FF_FPX] = FF_FPX;
            fileDriverMap[TwainAPI.DTWAIN_FF_EXIF] = FF_EXIF;
            fileDriverMap[TwainAPI.DTWAIN_FF_SPIFF] = FF_SPIFF;
            fileDriverMap[TwainAPI.DTWAIN_FF_XBM] = FF_XBM;
            fileDriverMap[TwainAPI.DTWAIN_FF_PICT] = FF_PICT;
            fileDriverMap[TwainAPI.DTWAIN_FF_JP2] = FF_JP2;
            fileDriverMap[TwainAPI.DTWAIN_FF_JPX] = FF_JPX;
            fileDriverMap[TwainAPI.DTWAIN_FF_DEJAVU] = FF_DEJAVU;
        }

        private void EnableFileDriverItems(bool bEnable)
        {
            if ( !bEnable )    
            {
                foreach (var entry in fileDriverMap)
                {
                    entry.Value.Enabled = false;
                }
            }
            else
            {
                foreach (var entry in fileDriverMap)
                {
                    entry.Value.Enabled = TwainAPI.DTWAIN_IsFileXferSupported(SelectedSource, entry.Key) == 1?true:false;
                }
            }
        }

        private void SetCaptionToSourceName()
        {
            StringBuilder szSourceName = new StringBuilder(256);
            string sTitle = sOrigTitle;
            if (SelectedSource != IntPtr.Zero)
            {
                TwainAPI.DTWAIN_GetSourceProductName(SelectedSource, szSourceName, 255);
                sTitle += " - ";
                sTitle += szSourceName;
                this.Text = sTitle;
            }
            else
                this.Text = sOrigTitle;
        }

        private void SourceProperties_Click(object sender, EventArgs e)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                SourcePropsDlg sPropDlg = new SourcePropsDlg(SelectedSource);
                sPropDlg.ShowDialog(this);
            }
        }

        private void CloseSource_Click(object sender, EventArgs e)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                TwainAPI.DTWAIN_CloseSource(SelectedSource);
                SelectedSource = IntPtr.Zero;
                SetCaptionToSourceName();
                EnableSourceItems(false);
                EnableFileDriverItems(false);
            }
        }

        private void ExitDemo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GenericAcquire(int nWhich)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                TwainAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5,
                                                      (int)TwainAPI.DTWAIN_BP_AUTODISCARD_ANY,
                                                      DiscardBlankPages.Checked ? 1 : 0);

                DTWAIN_ARRAY acquireArray = TwainAPI.DTWAIN_CreateAcquisitionArray();
                this.Enabled = false;
                int status = 0;
                int retVal = 0;
                if (nWhich == 0)
                    retVal = TwainAPI.DTWAIN_AcquireNativeEx(SelectedSource, TwainAPI.DTWAIN_PT_DEFAULT,
                         TwainAPI.DTWAIN_ACQUIREALL, UseSourceUI.Checked ? 1 : 0, 0, acquireArray, ref status);
                else
                    retVal = TwainAPI.DTWAIN_AcquireBufferedEx(SelectedSource, TwainAPI.DTWAIN_PT_DEFAULT,
                         TwainAPI.DTWAIN_ACQUIREALL, UseSourceUI.Checked ? 1 : 0, 0, acquireArray, ref status);
                if ( retVal == 0)
                {
                    int lastError = TwainAPI.DTWAIN_GetLastError();
                    if (status == TwainAPI.DTWAIN_TN_ACQUIRECANCELLED)
                        MessageBox.Show("No Images Acquired", "TWAIN Information");
                    else
                    { 
                        StringBuilder errMsg = new StringBuilder(256);
                        TwainAPI.DTWAIN_GetErrorString(lastError, errMsg, 256);
                        MessageBox.Show(errMsg.ToString(), "TWAIN Error");
                    }
                    this.Enabled = true;
                    return;
                }

                if (TwainAPI.DTWAIN_ArrayGetCount(acquireArray) == 0)
                {
                    MessageBox.Show("No Images Acquired", "");
                    this.Enabled = true;
                    return;
                }

                // Display the DIBS
                //...
                DIBDisplayerDlg sDIBDlg = new DIBDisplayerDlg(acquireArray);
                sDIBDlg.ShowDialog(this);
                this.Enabled = true;
            }
        }
        private void AcquireNative_Click(object sender, EventArgs e)
        {
            GenericAcquire(0);
        }

        private void AcquireBuffered_Click(object sender, EventArgs e)
        {
            GenericAcquire(1);
        }

        private void AcquireToFile(int nWhich, int nWhichFormat)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                int status = 0;
                int bError = 0;
                long FileFlags = 0;
                string tFileName = "";
                int fileType = 0;
                FileTypeDlg fDlg = new FileTypeDlg();
                if (nWhich == 0)
                {
                    FileFlags = TwainAPI.DTWAIN_USELONGNAME | TwainAPI.DTWAIN_USENATIVE | TwainAPI.DTWAIN_CREATE_DIRECTORY;
                    TwainAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5,
                                                          (int)TwainAPI.DTWAIN_BP_AUTODISCARD_ANY,
                                                          DiscardBlankPages.Checked ? 1 : 0);
                }
                else
                {
                    FileFlags = TwainAPI.DTWAIN_USESOURCEMODE | TwainAPI.DTWAIN_USELONGNAME;
                }
                DialogResult res = fDlg.ShowDialog(this);
                if (res == DialogResult.Cancel)
                    return;
                tFileName = fDlg.GetFileName();
                if ( tFileName.Length == 0 )
                {
                    MessageBox.Show("File name cannot be blank", "File name error");
                    return;
                }
                fileType = nWhichFormat; 
                this.Enabled = true;
                bError = TwainAPI.DTWAIN_AcquireFile(SelectedSource,
                                     tFileName,
                                     fileType,
                                     (int)FileFlags,
                                     TwainAPI.DTWAIN_PT_DEFAULT, /* Use default */
                                     TwainAPI.DTWAIN_ACQUIREALL, /* Get all pages */
                                     UseSourceUI.Checked ? 1 : 0,
                                     1,/* Close Source when UI is closed */
                                     ref status
                                    );

                if (bError == 0)
                {
                    int lastError = TwainAPI.DTWAIN_GetLastError();
                    if (status == TwainAPI.DTWAIN_TN_ACQUIRECANCELLED)
                        MessageBox.Show("No Images Acquired", "TWAIN Information");
                    else
                    {
                        StringBuilder errMsg = new StringBuilder(256);
                        TwainAPI.DTWAIN_GetErrorString(lastError, errMsg, 256);
                        MessageBox.Show(errMsg.ToString(), "TWAIN Error");
                    }
                    this.Enabled = true;
                    return;
                }
                else
                if (status == TwainAPI.DTWAIN_TN_ACQUIREDONE)
                    MessageBox.Show("Image file saved successfully");
                else
                if (TwainAPI.DTWAIN_GetFileSavePageCount(SelectedSource) == 0)
                    MessageBox.Show("No Images acquired");
                else
                    MessageBox.Show("The acquisition returned a status of " + status.ToString());
                this.Enabled = true;
            }
        }
        private void UseSourceUI_Click(object sender, EventArgs e)
        {
            UseSourceUI.Checked = !UseSourceUI.Checked;
        }

        private void DiscardBlankPages_Click(object sender, EventArgs e)
        {
            DiscardBlankPages.Checked = !DiscardBlankPages.Checked;
        }

        private void LoggingOptions_Click(object sender, EventArgs e)
        {
            uint LogFlags = TwainAPI.DTWAIN_LOG_ALL & ~(TwainAPI.DTWAIN_LOG_ISTWAINMSG | TwainAPI.DTWAIN_LOG_USEFILE | TwainAPI.DTWAIN_LOG_DEBUGMONITOR | TwainAPI.DTWAIN_LOG_CONSOLE);
            LogFileSelectionDlg logDlg = new LogFileSelectionDlg();
            DialogResult nResult = logDlg.ShowDialog(this);
            if (nResult == DialogResult.OK)
            {
                int debugOption = logDlg.GetDebugOption();
                TwainAPI.DTWAIN_SetTwainLog(0, "");
                switch (debugOption)
                {
                    case 0:
                    case 1:
                    break;
                    case 2:
                        TwainAPI.DTWAIN_SetTwainLog(LogFlags | TwainAPI.DTWAIN_LOG_USEFILE, logDlg.GetFileName());
                    break;
                    case 3:
                        TwainAPI.DTWAIN_SetTwainLog(LogFlags | TwainAPI.DTWAIN_LOG_DEBUGMONITOR, "");
                    break;
                    case 4:
                        TwainAPI.DTWAIN_SetTwainLog(LogFlags | TwainAPI.DTWAIN_LOG_CONSOLEWITHHANDLER, "");
                    break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EnableSourceItems(false);
        }

        private void About_Click(object sender, EventArgs e)
        {
            AboutDlg aDlg = new AboutDlg();
            aDlg.ShowDialog(this);
        }

        private void idlang_dutch_Click(object sender, EventArgs e)
        {
            load_language("dutch");
        }

        private void idlang_english_Click(object sender, EventArgs e)
        {
            load_language("english");
        }

        private void idlang_french_Click(object sender, EventArgs e)
        {
            load_language("french");
        }

        private void idlang_german_Click(object sender, EventArgs e)
        {
            load_language("german");
        }

        private void idlang_italian_Click(object sender, EventArgs e)
        {
            load_language("italian");
        }

        private void idlang_portuguese_Click(object sender, EventArgs e)
        {
            load_language("portuguese");
        }

        private void idlang_romanian_Click(object sender, EventArgs e)
        {
            load_language("romanian");
        }

        private void idlang_russian_Click(object sender, EventArgs e)
        {
            load_language("russian");
        }

        private void idlang_simplified_chinese_Click(object sender, EventArgs e)
        {
            load_language("simplified_chinese");
        }

        private void idlang_spanish_Click(object sender, EventArgs e)
        {
            load_language("spanish");
        }

        private void idlang_custom_Click(object sender, EventArgs e)
        {
            CustomLanguageDlg objCustomLanguage = new CustomLanguageDlg();
            DialogResult nResult = objCustomLanguage.ShowDialog(this);
            if (nResult == DialogResult.OK)
            {
                load_language(objCustomLanguage.GetText());
            }
        }

        private void load_language(string language)
        {
            int retVal = TwainAPI.DTWAIN_LoadCustomStringResources(language);
            if (retVal == 0)
                MessageBox.Show("Could not load language resource " + language, "Error", MessageBoxButtons.OK);
            else
                MessageBox.Show("Language " + " loaded successfully.  Select a Source or choose Logging/Log To Console to see the results");
        }

        private void idlang_greek_Click(object sender, EventArgs e)
        {
            load_language("greek");
        }

        private void idlang_korean_Click(object sender, EventArgs e)
        {
            load_language("korean");
        }

        private void idlang_japanese_Click(object sender, EventArgs e)
        {
            load_language("japanese");
        }

        private void idlang_traditional_chinese_Click(object sender, EventArgs e)
        {
            load_language("traditional_chinese");
        }

        private void ShowPreview_Click(object sender, EventArgs e)
        {
            ShowPreview.Checked = !ShowPreview.Checked;
        }

        private void ShowBarcodeInfo_Click(object sender, EventArgs e)
        {
            ShowBarcodeInfo.Checked = !ShowBarcodeInfo.Checked;
        }

        private void BigTiff_NoCompression_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BIGTIFFNONEMULTI);
        }

        private void BigTiff_Group3_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BIGTIFFG3MULTI);
        }

        private void BigTiff_Group4_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BIGTIFFG4MULTI);
        }

        private void BigTiff_Flate_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BIGTIFFDEFLATEMULTI);
        }

        private void BigTiff_JPEG_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BIGTIFFJPEGMULTI);
        }

        private void BigTiff_LZW_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BIGTIFFLZWMULTI);
        }

        private void BigTiff_Packbits_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BIGTIFFPACKBITSMULTI);
        }

        private void BMP_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BMP);
        }

        private void BMP_RLE_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_BMP_RLE);
        }

        private void DCX_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_DCX);
        }

        private void EMF_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_EMF);
        }

        private void GIF_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_GIF);
        }

        private void ICO_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_ICO);
        }

        private void ICOV_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_ICO_VISTA);
        }

        private void JPEG_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_JPEG);
        }

        private void JPEG2000_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_JPEG2000);
        }

        private void JPEGXL_Click(object sender, EventArgs e)
        {
            int isSupported = TwainAPI.DTWAIN_IsFileTypeSupported(TwainAPI.DTWAIN_JPEGXL);
            if ( isSupported == 0 )
            {
                MessageBox.Show("JPEG-XL library not found or one or more JPEG-XL dependencies were not found. Please install JPEG-XL components now and select JPEG-XL again...");
            }
            else
                AcquireToFile(0, TwainAPI.DTWAIN_JPEGXL);
        }

        private void JPEGXR_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_JPEGXR);
        }

        private void PSD_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_PSD);
        }

        private void PCX_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_PCX);
        }

        private void Do_PDF_Acquisition(int encryptionType)
        {
            bool pdfEncrypted = IsPDFEncrypted(encryptionType);
            PDFOptionsDlg optionsDlg = new PDFOptionsDlg(pdfEncrypted);
            optionsDlg.ShowDialog(this);
            PDFOptionsDlg.PDFOptionsData optionsData = optionsDlg.GetPDFOptions();

            TwainAPI.DTWAIN_SetPDFAuthor(SelectedSource, optionsData.sPDFAuthor);
            TwainAPI.DTWAIN_SetPDFCreator(SelectedSource, optionsData.sPDFCreator);
            TwainAPI.DTWAIN_SetPDFKeywords(SelectedSource, optionsData.sPDFKeywords);
            TwainAPI.DTWAIN_SetPDFTitle(SelectedSource, optionsData.sPDFTitle);
            TwainAPI.DTWAIN_SetPDFSubject(SelectedSource, optionsData.sPDFSubject);

            bool useStrong = false;
            if (pdfEncrypted)
            {
                switch (encryptionType)
                {
                    case PDF_128BitRC4_Encryption_Value:
                    case PDF_128BitAES_Encryption_Value:
                    case PDF_256BitAES_Encryption_Value:
                        useStrong = true;
                        break;
                }
                TwainAPI.DTWAIN_SetPDFEncryption(SelectedSource, 1, optionsData.sUserPassword, optionsData.sOwnerPassword,
                                                 TwainAPI.DTWAIN_PDF_ALLOWALL, useStrong ? 1 : 0);
                if (encryptionType == PDF_128BitAES_Encryption_Value)
                    TwainAPI.DTWAIN_SetPDFAESEncryption(SelectedSource, TwainAPI.DTWAIN_PDF_AES128, 1);
                else
                if (encryptionType == PDF_256BitAES_Encryption_Value)
                    TwainAPI.DTWAIN_SetPDFAESEncryption(SelectedSource, TwainAPI.DTWAIN_PDF_AES256, 1);
            }
            else
            if (encryptionType == PDF_ASCII85_Value)
            {
                TwainAPI.DTWAIN_SetPDFASCIICompression(SelectedSource, 1);
            }

            stamp_pdf = optionsData.stamp_pdf;
            AcquireToFile(0, TwainAPI.DTWAIN_PDFMULTI);
        }
        private bool IsPDFEncrypted(int value)
        {
            switch (value)
            {
                case PDF_128BitAES_Encryption_Value:
                case PDF_256BitAES_Encryption_Value:
                case PDF_40BitRC4_Encryption_Value:
                case PDF_128BitRC4_Encryption_Value:
                    return true;
            }
            return false;
        }
        private void PDF_NoEncryption_Click(object sender, EventArgs e)
        {
            Do_PDF_Acquisition(PDF_NoEncryption_Value);
        }

        private void PDF_ASCII85_Click(object sender, EventArgs e)
        {
            Do_PDF_Acquisition(PDF_ASCII85_Value);
        }

        private void PDF_40BitRC4_Encryption_Click(object sender, EventArgs e)
        {
            Do_PDF_Acquisition(PDF_40BitRC4_Encryption_Value);
        }

        private void PDF_128BitRC4_Encryption_Click(object sender, EventArgs e)
        {
            Do_PDF_Acquisition(PDF_128BitRC4_Encryption_Value);
        }

        private void PDF_128BitAES_Encryption_Click(object sender, EventArgs e)
        {
            Do_PDF_Acquisition(PDF_128BitAES_Encryption_Value);
        }

        private void PDF_256BitAES_Encryption_Click(object sender, EventArgs e)
        {
            Do_PDF_Acquisition(PDF_256BitAES_Encryption_Value);
        }

        private void PNG_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_PNG);
        }

        private void PostScript_Level1_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_POSTSCRIPT1MULTI);
        }

        private void PostScript_Level2_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_POSTSCRIPT2MULTI);
        }

        private void PostScript_Level3_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_POSTSCRIPT3MULTI);
        }

        private void SVG_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_SVG);
        }

        private void SVGZ_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_SVGZ);
        }

        private void TGA_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TGA);
        }

        private void TGARLE_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TGA_RLE);
        }

        private void TEXTOCR_Click(object sender, EventArgs e)
        {
            if (TwainAPI.DTWAIN_IsFileTypeSupported(TwainAPI.DTWAIN_TEXTMULTI) == 0)
            {
                MessageBox.Show("Text mode requires OCR engine installed", "No OCR engines detected");
                return;
            }
            AcquireToFile(0, TwainAPI.DTWAIN_TEXTMULTI);
        }

        private void Tiff_NoCompression_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TIFFNONEMULTI);
        }

        private void Tiff_Group3_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TIFFG3MULTI);
        }

        private void Tiff_Group4_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TIFFG4MULTI);
        }

        private void Tiff_Flate_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TIFFDEFLATEMULTI);
        }

        private void Tiff_JPEG_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TIFFJPEGMULTI);
        }

        private void Tiff_LZW_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TIFFLZWMULTI);
        }

        private void Tiff_Packbits_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_TIFFPACKBITSMULTI);
        }

        private void WEBP_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_WEBP);
        }

        private void WMF_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_WMF);
        }

        private void WBMP_Click(object sender, EventArgs e)
        {
            AcquireToFile(0, TwainAPI.DTWAIN_WBMP);
        }

        private void FF_BMP_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_BMP);
        }

        private void FF_JFIF_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_JFIF);
        }

        private void FF_TIFF_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_TIFF);
        }

        private void FF_TIFFMULTI_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_TIFFMULTI);
        }

        private void FF_PNG_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_PNG);
        }

        private void FF_PDF_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_PDF);
        }

        private void FF_PDFA_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_PDFA);
        }

        private void FF_PDFA2_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_PDFA2);
        }

        private void FF_PDFRASTER_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_PDFRASTER);
        }

        private void FF_FPX_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_FPX);
        }

        private void FF_EXIF_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_EXIF);
        }

        private void FF_SPIFF_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_SPIFF);
        }

        private void FF_XBM_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_XBM);
        }

        private void FF_PICT_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_PICT);
        }

        private void FF_JP2_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_JP2);
        }

        private void FF_JPX_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_JPX);
        }

        private void FF_DEJAVU_Click(object sender, EventArgs e)
        {
            AcquireToFile(1, TwainAPI.DTWAIN_FF_DEJAVU);
        }

        private void SetGetMessageLoopProcessing()
        {
            int valueToUse = 0;
            if (UseGetMessage.Checked)
                valueToUse = 0;
            else
                valueToUse = 1;
            TwainAPI.DTWAIN_EnablePeekMessageLoop(SelectedSource, valueToUse);
        }
        private void UseGetMessage_Click(object sender, EventArgs e)
        {
            UseGetMessage.Checked = !UseGetMessage.Checked;
            SetGetMessageLoopProcessing();
        }

        private void idlang_cantonese_Click(object sender, EventArgs e)
        {
            load_language("cantonese");
        }

        private void idlang_turkish_Click(object sender, EventArgs e)
        {
            load_language("turkish");
        }
    }
}
