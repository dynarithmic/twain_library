using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TWAINDemo
{
    public partial class LogFileSelectionDlg : Form
    {
        private string sFileName;
        private int nWhichOption;

        public int GetDebugOption()
        {
            return nWhichOption;
        }

        public string GetFileName()
        {
            return sFileName;
        }

        public LogFileSelectionDlg()
        {
            nWhichOption = 1;
            InitializeComponent();
        }

        private void radioLogToFile_CheckedChanged(object sender, EventArgs e)
        {
            edFileName.Enabled = radioLogToFile.Checked;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (radioLogToFile.Checked)
            {
                sFileName = edFileName.Text;
                nWhichOption = 2;
            }
            else
            if (radioLogDebugMonitor.Checked)
                nWhichOption = 3;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
        }
    }
}
