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
            TwainAPI.DTWAIN_GetSourceProductName(theSource, sTextData, 256);
            Text = sTextData.ToString();

            DTWAIN_ARRAY CapArray = IntPtr.Zero;
            TwainAPI.DTWAIN_EnumSupportedCaps(theSource, ref CapArray);
            int nCapCount = TwainAPI.DTWAIN_ArrayGetCount(CapArray);
            int nCapValue = 0;
            bool bOneBadCap = false;
            for (int nIndex = 0; nIndex < nCapCount; nIndex++)
            {
                TwainAPI.DTWAIN_ArrayGetAtLong(CapArray, nIndex, ref nCapValue);
                TwainAPI.DTWAIN_GetNameFromCap(nCapValue, sTextData, 255);
                int bestContainer = TwainAPI.DTWAIN_GetCapContainer(theSource, nCapValue, TwainConstants.MSG_GET);
                if (bestContainer == 0)
                {
                    listBadCaps.Items.Add(sTextData.ToString());
                    bOneBadCap = true;
                }
            }
            TwainAPI.DTWAIN_ArrayDestroy(CapArray);
            if (!bOneBadCap)
                listBadCaps.Items.Add("No Bad Capabilities detected");
        }
    }

}
