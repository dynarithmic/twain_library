using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Dynarithmic;

namespace TWAINDemo
{
    public partial class DIBDisplayerDlg2 : Form
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        System.IntPtr theDib = IntPtr.Zero;
        Bitmap curBMP;
        public DIBDisplayerDlg2(System.IntPtr item)
        {
            InitializeComponent();
            theDib = item;
        }

        private void DisplayTheDib()
        {
            this.dibBox2.Image = Bitmap.FromHbitmap(DTwainDemo.TwainAPI.DTWAIN_ConvertDIBToBitmap(theDib, System.IntPtr.Zero), System.IntPtr.Zero);
            curBMP = (Bitmap)this.dibBox2.Image;
        }

        private void DIBDisplayerDlg2_Load(object sender, EventArgs e)
        {
            DisplayTheDib();
        }

        private void DIBDispalyerDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeleteObject(curBMP.GetHbitmap());
        }
    }
}
