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
    public partial class AboutDlg : Form
    {
        public AboutDlg()
        {
            InitializeComponent();
        }

        private void AboutDlg_Load(object sender, EventArgs e)
        {
            int nChars = TwainAPI.DTWAIN_GetVersionInfo(IntPtr.Zero, -1);
            StringBuilder szInfo = new StringBuilder(nChars);
            TwainAPI.DTWAIN_GetVersionInfo(szInfo, nChars);
            edInfo.Text = szInfo.ToString().Replace("\n", "\r\n");
        }
    }
}
