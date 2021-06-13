using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
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
            int nCount = TwainAPI.DTWAIN_GetNumAcquisitions(AcquireArray);
            for (int i = 1; i <= nCount; ++i)
                this.cmbAcquisition.Items.Add(i.ToString());
            this.cmbAcquisition.SelectedIndex = 0;

            // Display the bitmap
            DisplayTheDib();
        }

        private void EnablePageButtons()
        {
            int nCount = TwainAPI.DTWAIN_GetNumAcquiredImages(AcquireArray, nCurrentAcquisition);
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
            return Bitmap.FromHbitmap(TwainAPI.DTWAIN_ConvertDIBToBitmap(pDIB, System.IntPtr.Zero), System.IntPtr.Zero);
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            --nCurDib;
            DisplayTheDib();
        }

        // Displays the DIB bitmap for acquisition nCurrentAcquisition, page nCurDib
        private void DisplayTheDib()
        {
            DTWAIN_HANDLE dib = TwainAPI.DTWAIN_GetAcquiredImage(AcquireArray, nCurrentAcquisition, nCurDib);
            this.dibBox.Image = BitmapFromDIB(dib);
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
            int acquisitionCount = TwainAPI.DTWAIN_GetNumAcquisitions(AcquireArray);
            for (int i = 0; i < acquisitionCount; ++i)
            {
                int imageCount = TwainAPI.DTWAIN_GetNumAcquiredImages(AcquireArray, i);
                for (int j = 0; j < imageCount; ++j)
                {
                    DTWAIN_HANDLE handle = TwainAPI.DTWAIN_GetAcquiredImage(AcquireArray, i, j);
                    TwainAPI.GlobalUnlock(handle);
                    TwainAPI.GlobalFree(handle);
                }
            }
        }
    }
}
