using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dynarithmic;
using DTWAIN_ARRAY = System.IntPtr;
using DTWAIN_SOURCE = System.IntPtr;
// using DTWAIN_ARRAY = System.Long; // Use this if compiling for 64-bits
// using DTWAIN_SOURCE = System.Long;


namespace TWAINDemo
{
    public partial class CustomSelectSource : Form
    {
        private bool sourceSelected;
        private string sSourceName;
        public bool IsSourceSelected() { return sourceSelected; }
        public string GetSourceName() { return sSourceName; }

        public CustomSelectSource()
        {
            InitializeComponent();
        }

        private void CustomSelectSource_Load(object sender, EventArgs e)
        {
            sourceSelected = false;
            DTWAIN_ARRAY SourceArray = IntPtr.Zero;
            TwainAPI.DTWAIN_EnumSources(ref SourceArray);
            int nCount = TwainAPI.DTWAIN_ArrayGetCount(SourceArray);
            if ( nCount <= 0 )
                Close();

            // Display the sources
            DTWAIN_SOURCE CurSource = IntPtr.Zero;
            for (int i = 0; i < nCount; ++i)
            {
                StringBuilder szName = new StringBuilder(256);
                TwainAPI.DTWAIN_ArrayGetSourceAt(SourceArray, i, ref CurSource);
                TwainAPI.DTWAIN_GetSourceProductName(CurSource, szName, 255);
                listSources.Items.Add(szName.ToString());
            }
            listSources.SelectedIndex = 0;
            // Display Info about sources
            string sText = nCount.ToString() + " TWAIN Source(s) Available for Selection";
            editSourceInfo.Text = sText;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            sSourceName = listSources.SelectedItem.ToString();
            sourceSelected = true;
        }
    }
}
