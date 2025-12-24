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
    public partial class CustomLanguageDlg : Form
    {
        public CustomLanguageDlg()
        {
            InitializeComponent();
        }

        private void textLanguageName_TextChanged(object sender, EventArgs e)
        {
        }
        public string GetText()
        {
            return this.textLanguageName.Text;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
        }
    }
}
