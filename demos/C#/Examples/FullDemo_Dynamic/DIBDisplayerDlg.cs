using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Dynarithmic;
using DTWAIN_ARRAY = System.IntPtr;
using DTWAIN_HANDLE = System.IntPtr;

namespace TWAINDemo
{
    public partial class DIBDisplayerDlg : Form
    {
        private DTWAIN_ARRAY AcquireArray;
        private int nCurrentAcquisition;
        private int nCurDib;

        public struct DibInfo
        {
            public int acquisition;
            public int pageNum;
        }

        private Dictionary<DibInfo, Bitmap> DibDictionary = new Dictionary<DibInfo, Bitmap>();

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        public DIBDisplayerDlg(DTWAIN_ARRAY AcqArray)
        {
            AcquireArray = AcqArray;
            InitializeComponent();
        }

        private void DIBDisplayerDlg_Load(object sender, EventArgs e)
        {
            // Set up acquisition combo box
            nCurrentAcquisition = 0;
            nCurDib = 0;
            int nCount = DTwainDemo.api.DTWAIN_GetNumAcquisitions(AcquireArray);
            for (int i = 1; i <= nCount; ++i)
                this.cmbAcquisition.Items.Add(i.ToString());
            this.cmbAcquisition.SelectedIndex = 0;

            // Display the bitmap
            DisplayTheDib();
        }

        private void EnablePageButtons()
        {
            int nCount = DTwainDemo.api.DTWAIN_GetNumAcquiredImages(AcquireArray, nCurrentAcquisition);
            this.buttonNext.Enabled = (nCurDib < nCount - 1);
            this.buttonPrev.Enabled = (nCurDib > 0);

            if (nCount == 0)
            {
            }
            else
            {
                int sDib = nCurDib + 1;
                this.edPageCurrent.Text = sDib.ToString();
                this.edPageTotal.Text = nCount.ToString();
            }
        }

        private static Bitmap BitmapFromDIB(IntPtr pDIB)
        {
            Bitmap theBitmap = Bitmap.FromHbitmap(DTwainDemo.api.DTWAIN_ConvertDIBToBitmap(pDIB, System.IntPtr.Zero), System.IntPtr.Zero);
            return theBitmap;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            --nCurDib;
            DisplayTheDib();
        }

        // Displays the DIB bitmap for acquisition nCurrentAcquisition, page nCurDib
        private void DisplayTheDib()
        {
            DibInfo keyCurrent = new DibInfo();
            keyCurrent.acquisition = nCurrentAcquisition;
            keyCurrent.pageNum = nCurDib;
            Bitmap theBitmap;
            if (DibDictionary.ContainsKey(keyCurrent))
            {
                theBitmap = DibDictionary[keyCurrent];
                this.dibBox.Image = theBitmap;
            }
            else
            {
                DTWAIN_HANDLE dibToUse = DTwainDemo.api.DTWAIN_GetAcquiredImage(AcquireArray, nCurrentAcquisition, nCurDib);
                if (dibToUse == System.IntPtr.Zero)
                {
                    MessageBox.Show("Image was discarded or not available", "DTWAIN Message", MessageBoxButtons.OK);
                    return;
                }
                this.dibBox.Image = BitmapFromDIB(dibToUse);
                DibDictionary.Add(keyCurrent, (Bitmap)this.dibBox.Image);
            }
            EnablePageButtons();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            ++nCurDib;
            DisplayTheDib();
        }

        private void cmbAcquisition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbAcquisition.SelectedIndex != nCurrentAcquisition)
            {
                nCurrentAcquisition = this.cmbAcquisition.SelectedIndex;
                nCurDib = 0;
                DisplayTheDib();
            }
        }

        private void DIBDisplayreDlg_Leave(object sender, EventArgs e)
        {
        }

        private void DIBDispalyerDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (KeyValuePair<DibInfo, Bitmap> pair in DibDictionary)
                DeleteObject(pair.Value.GetHbitmap());
            DTwainDemo.api.DTWAIN_DestroyAcquisitionArray(AcquireArray, 1);
        }
    }
}
