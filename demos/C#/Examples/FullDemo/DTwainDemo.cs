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

        #if _X64
        public long CallbackProc(long wParam, long lParam, long UserData)
        #else
        public int CallbackProc(int wParam, int lParam, int UserData)
        #endif
        {
            switch (wParam)
            {
                case TwainAPI.DTWAIN_TN_QUERYPAGEDISCARD:
                    DIBDisplayerDlg2 sDIBDlg = new DIBDisplayerDlg2(TwainAPI.DTWAIN_GetCurrentAcquiredImage(SelectedSource));
                    if (sDIBDlg.ShowDialog() == DialogResult.Cancel)
                        return 0;
                break;
            }
            return 1;
        }

        public DTwainDemo()
		{
            initialized = false;
            InitializeComponent();

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
            this.AcquireFileUsingDevice = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.UseSourceUI = new System.Windows.Forms.MenuItem();
            this.DiscardBlankPages = new System.Windows.Forms.MenuItem();
            this.TwainLogging = new System.Windows.Forms.MenuItem();
            this.LoggingOptions = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.idlang_dutch = new System.Windows.Forms.MenuItem();
            this.idlang_english = new System.Windows.Forms.MenuItem();
            this.idlang_french = new System.Windows.Forms.MenuItem();
            this.idlang_german = new System.Windows.Forms.MenuItem();
            this.idlang_italian = new System.Windows.Forms.MenuItem();
            this.idlang_portuguese = new System.Windows.Forms.MenuItem();
            this.idlang_romanian = new System.Windows.Forms.MenuItem();
            this.idlang_russian = new System.Windows.Forms.MenuItem();
            this.idlang_simplified_chinese = new System.Windows.Forms.MenuItem();
            this.idlang_spanish = new System.Windows.Forms.MenuItem();
            this.idlang_custom = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.About = new System.Windows.Forms.MenuItem();
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
            this.UseSourceUI,
            this.DiscardBlankPages});
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
            this.AcquireFile.Text = "Acquire File...";
            this.AcquireFile.Click += new System.EventHandler(this.AcquireFile_Click);
            // 
            // AcquireFileUsingDevice
            // 
            this.AcquireFileUsingDevice.Index = 3;
            this.AcquireFileUsingDevice.Text = "Acquire File Using Driver Transfer...";
            this.AcquireFileUsingDevice.Click += new System.EventHandler(this.AcquireFileUsingDevice_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Text = "-";
            // 
            // UseSourceUI
            // 
            this.UseSourceUI.Checked = true;
            this.UseSourceUI.Index = 5;
            this.UseSourceUI.Text = "Use Source UI";
            this.UseSourceUI.Click += new System.EventHandler(this.UseSourceUI_Click);
            // 
            // DiscardBlankPages
            // 
            this.DiscardBlankPages.Index = 6;
            this.DiscardBlankPages.Text = "Discard Blank Pages";
            this.DiscardBlankPages.Click += new System.EventHandler(this.DiscardBlankPages_Click);
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
            this.idlang_dutch,
            this.idlang_english,
            this.idlang_french,
            this.idlang_german,
            this.idlang_italian,
            this.idlang_portuguese,
            this.idlang_romanian,
            this.idlang_russian,
            this.idlang_simplified_chinese,
            this.idlang_spanish,
            this.idlang_custom});
            this.menuItem6.Text = "Language";
            // 
            // idlang_dutch
            // 
            this.idlang_dutch.Index = 0;
            this.idlang_dutch.Text = "Dutch";
            this.idlang_dutch.Click += new System.EventHandler(this.idlang_dutch_Click);
            // 
            // idlang_english
            // 
            this.idlang_english.Index = 1;
            this.idlang_english.Text = "English";
            this.idlang_english.Click += new System.EventHandler(this.idlang_english_Click);
            // 
            // idlang_french
            // 
            this.idlang_french.Index = 2;
            this.idlang_french.Text = "French";
            this.idlang_french.Click += new System.EventHandler(this.idlang_french_Click);
            // 
            // idlang_german
            // 
            this.idlang_german.Index = 3;
            this.idlang_german.Text = "German";
            this.idlang_german.Click += new System.EventHandler(this.idlang_german_Click);
            // 
            // idlang_italian
            // 
            this.idlang_italian.Index = 4;
            this.idlang_italian.Text = "Italian";
            this.idlang_italian.Click += new System.EventHandler(this.idlang_italian_Click);
            // 
            // idlang_portuguese
            // 
            this.idlang_portuguese.Index = 5;
            this.idlang_portuguese.Text = "Portuguese";
            this.idlang_portuguese.Click += new System.EventHandler(this.idlang_portuguese_Click);
            // 
            // idlang_romanian
            // 
            this.idlang_romanian.Index = 6;
            this.idlang_romanian.Text = "Romanian";
            this.idlang_romanian.Click += new System.EventHandler(this.idlang_romanian_Click);
            // 
            // idlang_russian
            // 
            this.idlang_russian.Index = 7;
            this.idlang_russian.Text = "Russian";
            this.idlang_russian.Click += new System.EventHandler(this.idlang_russian_Click);
            // 
            // idlang_simplified_chinese
            // 
            this.idlang_simplified_chinese.Index = 8;
            this.idlang_simplified_chinese.Text = "Simplified Chinese";
            this.idlang_simplified_chinese.Click += new System.EventHandler(this.idlang_simplified_chinese_Click);
            // 
            // idlang_spanish
            // 
            this.idlang_spanish.Index = 9;
            this.idlang_spanish.Text = "Spanish";
            this.idlang_spanish.Click += new System.EventHandler(this.idlang_spanish_Click);
            // 
            // idlang_custom
            // 
            this.idlang_custom.Index = 10;
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
                            TwainAPI.DTWAIN_DLG_CENTER_SCREEN  | TwainAPI.DTWAIN_DLG_HIGHLIGHTFIRST | TwainAPI.DTWAIN_DLG_SORTNAMES
                            | TwainAPI.DTWAIN_DLG_TOPMOSTWINDOW);
                break;

                case 1:
                    SelectSourceByNameBox objSelectSourceByName = new SelectSourceByNameBox();
                    DialogResult nResult = objSelectSourceByName.ShowDialog();
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
                    DialogResult dResult = customSourceDlg.ShowDialog();
                    if (dResult == DialogResult.OK)
                        SelectedSource = TwainAPI.DTWAIN_SelectSourceByName(customSourceDlg.GetSourceName());
                break;
            }
            this.Enabled = true;  // re-enable the main form

            if (SelectedSource != IntPtr.Zero)
            {
                if (TwainAPI.DTWAIN_OpenSource(SelectedSource) != 0)
                {
                    TwainAPI.DTWAIN_EnableFeeder(SelectedSource, 1);
                    SetCaptionToSourceName();
                    EnableSourceItems(true);
                    return;
                }
                else
                {
                    MessageBox.Show("Error Opening Source", "TWAIN Error", MessageBoxButtons.OK);
                    SetCaptionToSourceName();
                    EnableSourceItems(false);
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
                sPropDlg.ShowDialog();
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
                sDIBDlg.ShowDialog();
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

        private void AcquireFile_Click(object sender, EventArgs e)
        {
            AcquireToFile(0);
        }

        private void AcquireFileUsingDevice_Click(object sender, EventArgs e)
        {
            AcquireToFile(1);
        }

        private void AcquireToFile(int nWhich)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                int status = 0;
                int bError = 0;
                long FileFlags = 0;
                string tFileName = "";
                int fileType = 0;
                switch (nWhich)
                {
                    case 0:
                        FileFlags = TwainAPI.DTWAIN_USELONGNAME | TwainAPI.DTWAIN_USENATIVE | TwainAPI.DTWAIN_CREATE_DIRECTORY;
                        TwainAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5,
                                                              (int)TwainAPI.DTWAIN_BP_AUTODISCARD_ANY,
                                                              DiscardBlankPages.Checked ? 1 : 0);
                        FileTypeDlg fDlg = new FileTypeDlg();
                        fDlg.ShowDialog();
                        tFileName = fDlg.GetFileName();
                        StringBuilder szSourceName = new StringBuilder(tFileName);
                        fileType = fDlg.GetFileType();
                        this.Enabled = true;
                        break;

                    case 1:
                        if (TwainAPI.DTWAIN_IsFileXferSupported(SelectedSource, TwainAPI.DTWAIN_ANYSUPPORT) == 0)
                        {
                            MessageBox.Show("Sorry.  The selected driver does not have built-in file transfer support.");
                            return;
                        }
                        if (TwainAPI.DTWAIN_IsFileXferSupported(SelectedSource, TwainAPI.DTWAIN_FF_BMP) == 0)
                        {
                            string sText = "Sorry.  This demo program only supports built-in BMP file transfers.\r\n";
                            sText += "However, the DTWAIN library will support all built-in formats if your driver\r\n";
                            sText += "supports other formats.";
                            MessageBox.Show(sText);
                            this.Enabled = true;
                            return;
                        }
                        FileFlags = TwainAPI.DTWAIN_USESOURCEMODE | TwainAPI.DTWAIN_USELONGNAME;
                        fileType = TwainAPI.DTWAIN_FF_BMP;
                        tFileName = ".\\IMAGE.BMP";
                        MessageBox.Show("The name of the image file that will be saved is IMAGE.BMP\n");
                        break;
                }

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
            DialogResult nResult = logDlg.ShowDialog();
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
            aDlg.ShowDialog();
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
            DialogResult nResult = objCustomLanguage.ShowDialog();
            if (nResult == DialogResult.OK)
            {
                load_language(objCustomLanguage.GetText());
            }
        }

        private void load_language(string language)
        {
            int retVal = TwainAPI.DTWAIN_LoadCustomStringResourcesA(language);
            if (retVal == 0)
                MessageBox.Show("Could not load language resource " + language, "Error", MessageBoxButtons.OK);
            else
                MessageBox.Show("Language " + " loaded successfully.  Select a Source or choose Logging/Log To Console to see the results");
        }
    }
}
