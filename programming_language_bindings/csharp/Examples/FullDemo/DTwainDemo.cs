using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Dynarithmic;
using System.Text;
using System.Diagnostics;

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
    
/*  Use this instead of above for 64-bit compilation
    using DTWAIN_ARRAY = System.Int64;
    using DTWAIN_BOOL = System.Int32;
    using DTWAIN_FLOAT = System.Double;
    using DTWAIN_FRAME = System.Int64;
    using HANDLE = System.Int64;
    using DTWAIN_IDENTITY = System.Int64;
    using DTWAIN_OCRENGINE = System.Int64;
    using DTWAIN_LONG = System.Int32;
    using DTWAIN_LONG64 = System.Int64;
    using DTWAIN_PDFTEXTELEMENT = System.Int64;
    using DTWAIN_RANGE = System.Int64;
    using DTWAIN_SOURCE = System.Int64;
*/

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
        private String sOrigTitle;

		public DTwainDemo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
            sOrigTitle = this.Text;
			TwainAPI.DTWAIN_SysInitialize();
            SelectedSource = IntPtr.Zero;
			if ( TwainAPI.DTWAIN_IsTwainAvailable() == 0)
			{
				SelectSource.Enabled = false;
				SelectSourceByNameBox.Enabled = false;
			}
		}

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
            System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
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
            this.SelectSource.Enabled = true;
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
            // menuItem2
            // 
            this.menuItem2.Index = 3;
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
            this.ClientSize = new System.Drawing.Size(577, 441);
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
			Application.Run(new DTwainDemo());
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

            this.Enabled = false;  // disable the main form
            switch (nWhich)
            {
                case 0:
                    // Select the source
                    SelectedSource = TwainAPI.DTWAIN_SelectSource();
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
                    MessageBox.Show("Error Opening Source", "TWAIN Error", MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("Error Selecting Source", "TWAIN Error", MessageBoxButtons.OK);
            EnableSourceItems(false);
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
                SourcePropertiesDlg sPropDlg = new SourcePropertiesDlg(SelectedSource);
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

        private void AcquireNative_Click(object sender, EventArgs e)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                TwainAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5,
                                                      (int)TwainAPI.DTWAIN_BP_AUTODISCARD_ANY, 
                                                      DiscardBlankPages.Checked?1:0);

                DTWAIN_ARRAY acquireArray = TwainAPI.DTWAIN_CreateAcquisitionArray();
                this.Enabled = false;
                int status = 0;
                if ( TwainAPI.DTWAIN_AcquireNativeEx(SelectedSource, TwainAPI.DTWAIN_PT_DEFAULT,
                     TwainAPI.DTWAIN_ACQUIREALL, UseSourceUI.Checked ? 1 : 0, 0, acquireArray, ref status) == 0)
                {
                    MessageBox.Show("Acquisition Failed", "TWAIN Error");
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
                TwainAPI.DTWAIN_DestroyAcquisitionArray(acquireArray, 0);
                this.Enabled = true;
            }
        }

        private void AcquireBuffered_Click(object sender, EventArgs e)
        {
            if (SelectedSource != IntPtr.Zero)
            {
                TwainAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5,
                                                      (int)TwainAPI.DTWAIN_BP_AUTODISCARD_ANY,
                                                      DiscardBlankPages.Checked ? 1 : 0);

                DTWAIN_ARRAY acquireArray = TwainAPI.DTWAIN_CreateAcquisitionArray();
                this.Enabled = false;
                int status = 0;
                if (TwainAPI.DTWAIN_AcquireBufferedEx(SelectedSource, TwainAPI.DTWAIN_PT_DEFAULT,
                     TwainAPI.DTWAIN_ACQUIREALL, UseSourceUI.Checked ? 1 : 0, 0, acquireArray, ref status) == 0)
                {
                    MessageBox.Show("Acquisition Failed", "TWAIN Error");
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
                TwainAPI.DTWAIN_DestroyAcquisitionArray(acquireArray, 0);
                this.Enabled = true;
            }
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
                        FileFlags = TwainAPI.DTWAIN_USELONGNAME | TwainAPI.DTWAIN_USENATIVE;
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
                    MessageBox.Show("Error acquiring or saving file.");
                else
                if (status == TwainAPI.DTWAIN_TN_ACQUIREDONE)
                    MessageBox.Show("Image file saved successfully");
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
            long LogFlags = TwainAPI.DTWAIN_LOG_ALL & ~TwainAPI.DTWAIN_LOG_ERRORMSGBOX;
            LogFileSelectionDlg logDlg = new LogFileSelectionDlg();
            DialogResult nResult = logDlg.ShowDialog();
            if (nResult == DialogResult.OK)
            {
                int debugOption = logDlg.GetDebugOption();
                switch (debugOption)
                {
                    case 0:
                        break;
                    case 1:
                        TwainAPI.DTWAIN_SetTwainLog(0, "");
                    break;
                    case 2:
                        TwainAPI.DTWAIN_SetTwainLog((int)(LogFlags | TwainAPI.DTWAIN_LOG_USEFILE), logDlg.GetFileName());
                    break;
                    case 3:
                        TwainAPI.DTWAIN_SetTwainLog((int)(LogFlags & ~TwainAPI.DTWAIN_LOG_USEFILE), "");
                        MessageBox.Show("The DebugView debug monitor will start...");
                        Process.Start("DbgView.exe");
                        this.Enabled = true;
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

	}
}
