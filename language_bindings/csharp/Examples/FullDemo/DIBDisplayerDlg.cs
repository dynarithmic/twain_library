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

using DTWAIN_ARRAY = System.Int32;


namespace TWAINDemo
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
       public uint biSize;
       public int biWidth;
       public int biHeight;
       public ushort biPlanes;
       public ushort biBitCount;
       public uint biCompression;
       public uint biSizeImage;
       public int biXPelsPerMeter;
       public int biYPelsPerMeter;
       public uint biClrUsed;
       public uint biClrImportant;

       public void Init()
       {
           biSize = (uint)Marshal.SizeOf(this);
       }
    }

    public partial class DIBDisplayerDlg : Form
    {
        [DllImport("GdiPlus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int GdipCreateBitmapFromGdiDib(IntPtr pBIH, IntPtr pPix, out IntPtr pBitmap);

        [DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GlobalLock(int handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GlobalUnlock(IntPtr handle);

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
            //get pointer to bitmap header info       
            IntPtr pPix = GetPixelInfo(pDIB);

            //Call external GDI method
            MethodInfo mi = typeof(Bitmap).GetMethod("FromGDIplus", BindingFlags.Static | BindingFlags.NonPublic);
            if (mi == null)
                return null;

            // Initialize memory pointer where Bitmap will be saved
            IntPtr pBmp = IntPtr.Zero;

            //Call external method that saves bitmap into pointer
            int status = GdipCreateBitmapFromGdiDib(pDIB, pPix, out pBmp);

            //If success return bitmap, if failed return null
            if ((status == 0) && (pBmp != IntPtr.Zero))
                return (Bitmap)mi.Invoke(null, new object[] { pBmp });
            else
                return null;
        }

        // THIS METHOD GETS THE POINTER TO THE BITMAP HEADER INFO
        private static IntPtr GetPixelInfo(IntPtr bmpPtr)
        {
           BITMAPINFOHEADER bmi = (BITMAPINFOHEADER)Marshal.PtrToStructure(bmpPtr, typeof(BITMAPINFOHEADER));

           if (bmi.biSizeImage == 0)
               bmi.biSizeImage = (uint)(((((bmi.biWidth * bmi.biBitCount) + 31) & ~31) >> 3) * bmi.biHeight);

           int p = (int)bmi.biClrUsed;
           if ((p == 0) && (bmi.biBitCount <= 8))
               p = 1 << bmi.biBitCount;
           p = (p * 4) + (int)bmi.biSize + (int)bmpPtr;
           return (IntPtr)p;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            --nCurDib;
            DisplayTheDib();
        }

        // Displays the DIB bitmap for acquisition nCurrentAcquisition, page nCurDib
        private void DisplayTheDib()
        {
            int dib = TwainAPI.DTWAIN_GetAcquiredImage(AcquireArray, nCurrentAcquisition, nCurDib);
            IntPtr dibPtr = GlobalLock(dib);
            this.dibBox.Image = BitmapFromDIB(dibPtr);
            GlobalUnlock(dibPtr);
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
    }
}
