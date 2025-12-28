using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TWAINDemo
{
    public partial class SelectSourceByNameBox : Form
    {
        public string GetText()
        {
            return this.textSourceName.Text;
        }

        public SelectSourceByNameBox()
        {
            InitializeComponent();
        }

        private void SelectSourceByNameBox_Load(object sender, EventArgs e)
        {
        }
    }
}
