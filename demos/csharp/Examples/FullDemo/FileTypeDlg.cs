using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dynarithmic;

namespace TWAINDemo
{
    public partial class FileTypeDlg : Form
    {
        private int selectedFileType;
        private string selectedFileName;

        public int GetFileType()
        {
            return selectedFileType;
        }

        public string GetFileName()
        {
            return selectedFileName;
        }

        private class AllTypes
        {
            public string fType;
            public int DTWAINType;
            public string defFileName;
            public AllTypes(string s1, int dType, string s2) { fType = s1; DTWAINType = dType; defFileName = s2; }
        }

        private AllTypes[] g_allTypes = {new AllTypes("BMP File", TwainAPI.DTWAIN_BMP, "test.bmp"),
                                 new AllTypes("BMP RLE File",TwainAPI.DTWAIN_BMP_RLE, "testrle.bmp"),
                                 new AllTypes("PCX File",TwainAPI.DTWAIN_PCX, "test.pcx"),
                                 new AllTypes("Multi-page DCX File",TwainAPI.DTWAIN_DCX, "test.dcx"),
                                 new AllTypes("Enhanced Meta File (EMF)",TwainAPI.DTWAIN_EMF, "test.emf"),
                                 new AllTypes("GIF File", TwainAPI.DTWAIN_GIF, "test.gif"),
                                 new AllTypes("JPEG File", TwainAPI.DTWAIN_JPEG, "test.jpg"),
                                 new AllTypes("JPEG-2000 File", TwainAPI.DTWAIN_JPEG2000, "test.jp2"),
                                 new AllTypes("Adobe PDF File", TwainAPI.DTWAIN_PDFMULTI,"test.pdf"),
                                 new AllTypes("Postscript Level 1 File", TwainAPI.DTWAIN_POSTSCRIPT1MULTI, "test.ps"),
                                 new AllTypes("Postscript Level 2 File", TwainAPI.DTWAIN_POSTSCRIPT2MULTI, "test.ps"),
                                 new AllTypes("PNG File", TwainAPI.DTWAIN_PNG, "test.png"),
                                 new AllTypes("Adobe Paintshop (PSD) File", TwainAPI.DTWAIN_PSD, "test.psd"),
                                 new AllTypes("Text File", TwainAPI.DTWAIN_TEXTMULTI, "test.txt"),
                                 new AllTypes("TIFF (No compression)", TwainAPI.DTWAIN_TIFFNONEMULTI, "test.tif"),
                                 new AllTypes("TIFF (CCITT Group 3)", TwainAPI.DTWAIN_TIFFG3MULTI, "test.tif"),
                                 new AllTypes("TIFF (CCITT Group 4)", TwainAPI.DTWAIN_TIFFG4MULTI, "test.tif"),
                                 new AllTypes("TIFF (JPEG compression)", TwainAPI.DTWAIN_TIFFJPEGMULTI, "test.tif"),
                                 new AllTypes("TIFF (Packbits)", TwainAPI.DTWAIN_TIFFPACKBITSMULTI, "test.tif"),
                                 new AllTypes("TIFF (Flate compression)", TwainAPI.DTWAIN_TIFFDEFLATEMULTI, "test.tif"),
                                 new AllTypes("TIFF (LZW compression)", TwainAPI.DTWAIN_TIFFLZWMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (No compression)", TwainAPI.DTWAIN_BIGTIFFNONEMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (CCITT Group 3)", TwainAPI.DTWAIN_BIGTIFFG3MULTI, "test.tif"),
                                 new AllTypes("BigTIFF (CCITT Group 4)", TwainAPI.DTWAIN_BIGTIFFG4MULTI, "test.tif"),
                                 new AllTypes("BigTIFF (JPEG compression)", TwainAPI.DTWAIN_BIGTIFFJPEGMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (Packbits)", TwainAPI.DTWAIN_BIGTIFFPACKBITSMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (Flate compression)", TwainAPI.DTWAIN_BIGTIFFDEFLATEMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (LZW compression)", TwainAPI.DTWAIN_BIGTIFFLZWMULTI, "test.tif"),
                                 new AllTypes("Targa (TGA) File", TwainAPI.DTWAIN_TGA, "test.tga"),
                                 new AllTypes("Targa RLE (TGA) File",TwainAPI.DTWAIN_TGA_RLE, "testrle.tga"),
                                 new AllTypes("Windows Meta File (WMF)", TwainAPI.DTWAIN_WMF, "test.wmf"),
                                 new AllTypes("Windows ICON File (ICO)", TwainAPI.DTWAIN_ICO_RESIZED, "test.ico"),
                                 new AllTypes("Windows ICON File- Vista compatible (ICO)", TwainAPI.DTWAIN_ICO_VISTA, "test.ico"),
                                 new AllTypes("Wireless Bitmap File (WBMP)", TwainAPI.DTWAIN_WBMP_RESIZED, "test.wbmp")};


        public FileTypeDlg()
        {
            selectedFileType = TwainAPI.DTWAIN_BMP;
            selectedFileName = "test.bmp";
            InitializeComponent();
        }

        private void FileTypeDlg_Load(object sender, EventArgs e)
        {
            int nTypes = g_allTypes.Length;
            for (int i = 0; i < nTypes; ++i)
                cmbFileType.Items.Add(g_allTypes[i].fType);
            cmbFileType.SelectedIndex = 0;
            edFileName.Text = g_allTypes[0].defFileName;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            int nCurSel = cmbFileType.SelectedIndex;
            selectedFileType = g_allTypes[nCurSel].DTWAINType;
            selectedFileName = edFileName.Text;
        }

        private void cmbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nCurSel = cmbFileType.SelectedIndex;
            edFileName.Text = g_allTypes[nCurSel].defFileName;
        }
    }
}
