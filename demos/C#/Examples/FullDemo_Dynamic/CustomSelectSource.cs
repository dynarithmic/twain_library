using System;
using System.Text;
using System.Windows.Forms;
using DTWAIN_ARRAY = System.IntPtr;
using DTWAIN_SOURCE = System.IntPtr;

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
            DTwainDemo.TwainAPI.DTWAIN_EnumSources(ref SourceArray);
            int nCount = DTwainDemo.TwainAPI.DTWAIN_ArrayGetCount(SourceArray);
            if (nCount <= 0)
                Close();

            // Display the sources
            DTWAIN_SOURCE CurSource = IntPtr.Zero;
            for (int i = 0; i < nCount; ++i)
            {
                StringBuilder szName = new StringBuilder(256);
                DTwainDemo.TwainAPI.DTWAIN_ArrayGetSourceAt(SourceArray, i, ref CurSource);
                DTwainDemo.TwainAPI.DTWAIN_GetSourceProductName(CurSource, szName, 255);
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
