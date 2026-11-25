using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTWAIN_SOURCE = System.IntPtr;
using DTWAIN_ARRAY = System.IntPtr;
using Dynarithmic;

namespace TWAINDemo
{
    public partial class BadCapabilitiesDlg : Form
    {
        public BadCapabilitiesDlg(DTWAIN_SOURCE theSource)
        {
            InitializeComponent();
            LoadBadCapabilities(theSource);
        }
        private void LoadBadCapabilities(DTWAIN_SOURCE theSource)
        {
            StringBuilder sTextData = new StringBuilder(256);
            DTwainDemo.TwainAPI.DTWAIN_GetSourceProductName(theSource, sTextData, 256);
            Text = sTextData.ToString();

            DTWAIN_ARRAY CapArray = IntPtr.Zero;
            DTwainDemo.TwainAPI.DTWAIN_EnumSupportedCaps(theSource, ref CapArray);
            int nCapCount = DTwainDemo.TwainAPI.DTWAIN_ArrayGetCount(CapArray);
            int nCapValue = 0;
            bool bOneBadCap = false;
            for (int nIndex = 0; nIndex < nCapCount; nIndex++)
            {
                DTwainDemo.TwainAPI.DTWAIN_ArrayGetAtLong(CapArray, nIndex, ref nCapValue);
                DTwainDemo.TwainAPI.DTWAIN_GetNameFromCap(nCapValue, sTextData, 255);
                int bestContainer = DTwainDemo.TwainAPI.DTWAIN_GetCapContainer(theSource, nCapValue, TwainConstants.MSG_GET);
                if (bestContainer == 0)
                {
                    listBadCaps.Items.Add(sTextData.ToString());
                    bOneBadCap = true;
                }
            }
            DTwainDemo.TwainAPI.DTWAIN_ArrayDestroy(CapArray);
            if (!bOneBadCap)
                listBadCaps.Items.Add("No Bad Capabilities detected");
        }
    }
}
