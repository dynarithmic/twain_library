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

        private AllTypes[] g_allTypes = {new AllTypes("BMP File", Dynarithmic.Constants.DTWAIN_BMP, "test.bmp"),
                                 new AllTypes("BMP RLE File",Dynarithmic.Constants.DTWAIN_BMP_RLE, "testrle.bmp"),
                                 new AllTypes("PCX File",Dynarithmic.Constants.DTWAIN_PCX, "test.pcx"),
                                 new AllTypes("Multi-page DCX File",Dynarithmic.Constants.DTWAIN_DCX, "test.dcx"),
                                 new AllTypes("Enhanced Meta File (EMF)",Dynarithmic.Constants.DTWAIN_EMF, "test.emf"),
                                 new AllTypes("GIF File", Dynarithmic.Constants.DTWAIN_GIF, "test.gif"),
                                 new AllTypes("JPEG File", Dynarithmic.Constants.DTWAIN_JPEG, "test.jpg"),
                                 new AllTypes("JPEG-2000 File", Dynarithmic.Constants.DTWAIN_JPEG2000, "test.jp2"),
                                 new AllTypes("JPEG-XR File", Dynarithmic.Constants.DTWAIN_JPEGXR, "test.jxr"),
                                 new AllTypes("Adobe PDF File", Dynarithmic.Constants.DTWAIN_PDFMULTI,"test.pdf"),
                                 new AllTypes("Postscript Level 1 File", Dynarithmic.Constants.DTWAIN_POSTSCRIPT1MULTI, "test.ps"),
                                 new AllTypes("Postscript Level 2 File", Dynarithmic.Constants.DTWAIN_POSTSCRIPT2MULTI, "test.ps"),
                                 new AllTypes("PNG File", Dynarithmic.Constants.DTWAIN_PNG, "test.png"),
                                 new AllTypes("Adobe Paintshop (PSD) File", Dynarithmic.Constants.DTWAIN_PSD, "test.psd"),
                                 new AllTypes("Text File", Dynarithmic.Constants.DTWAIN_TEXTMULTI, "test.txt"),
                                 new AllTypes("TIFF (No compression)", Dynarithmic.Constants.DTWAIN_TIFFNONEMULTI, "test.tif"),
                                 new AllTypes("TIFF (CCITT Group 3)", Dynarithmic.Constants.DTWAIN_TIFFG3MULTI, "test.tif"),
                                 new AllTypes("TIFF (CCITT Group 4)", Dynarithmic.Constants.DTWAIN_TIFFG4MULTI, "test.tif"),
                                 new AllTypes("TIFF (JPEG compression)", Dynarithmic.Constants.DTWAIN_TIFFJPEGMULTI, "test.tif"),
                                 new AllTypes("TIFF (Packbits)", Dynarithmic.Constants.DTWAIN_TIFFPACKBITSMULTI, "test.tif"),
                                 new AllTypes("TIFF (Flate compression)", Dynarithmic.Constants.DTWAIN_TIFFDEFLATEMULTI, "test.tif"),
                                 new AllTypes("TIFF (LZW compression)", Dynarithmic.Constants.DTWAIN_TIFFLZWMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (No compression)", Dynarithmic.Constants.DTWAIN_BIGTIFFNONEMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (CCITT Group 3)", Dynarithmic.Constants.DTWAIN_BIGTIFFG3MULTI, "test.tif"),
                                 new AllTypes("BigTIFF (CCITT Group 4)", Dynarithmic.Constants.DTWAIN_BIGTIFFG4MULTI, "test.tif"),
                                 new AllTypes("BigTIFF (JPEG compression)", Dynarithmic.Constants.DTWAIN_BIGTIFFJPEGMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (Packbits)", Dynarithmic.Constants.DTWAIN_BIGTIFFPACKBITSMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (Flate compression)", Dynarithmic.Constants.DTWAIN_BIGTIFFDEFLATEMULTI, "test.tif"),
                                 new AllTypes("BigTIFF (LZW compression)", Dynarithmic.Constants.DTWAIN_BIGTIFFLZWMULTI, "test.tif"),
                                 new AllTypes("Targa (TGA) File", Dynarithmic.Constants.DTWAIN_TGA, "test.tga"),
                                 new AllTypes("Targa RLE (TGA) File",Dynarithmic.Constants.DTWAIN_TGA_RLE, "testrle.tga"),
                                 new AllTypes("Windows Meta File (WMF)", Dynarithmic.Constants.DTWAIN_WMF, "test.wmf"),
                                 new AllTypes("Windows ICON File (ICO)", Dynarithmic.Constants.DTWAIN_ICO_RESIZED, "test.ico"),
                                 new AllTypes("Windows ICON File- Vista compatible (ICO)", Dynarithmic.Constants.DTWAIN_ICO_VISTA, "test.ico"),
                                 new AllTypes("Wireless Bitmap File (WBMP)", Dynarithmic.Constants.DTWAIN_WBMP_RESIZED, "test.wbmp")};


        public FileTypeDlg()
        {
            selectedFileType = Dynarithmic.Constants.DTWAIN_BMP;
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
