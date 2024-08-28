using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dynarithmic;

using DTWAIN_SOURCE = System.IntPtr;
using DTWAIN_ARRAY = System.IntPtr;

namespace TWAINDemo
{
    public partial class SourcePropsDlg : Form
    {
        public SourcePropsDlg()
        {
            InitializeComponent();
        }

        private DTWAIN_SOURCE m_Source;

        public SourcePropsDlg(DTWAIN_SOURCE theSource)
        {
            m_Source = theSource;
            InitializeComponent();
        }

        private void SourcePropsDlg_Load(object sender, EventArgs e)
        {
            StringBuilder szInfo = new StringBuilder(256);
            TwainAPI.DTWAIN_GetSourceProductName(m_Source, szInfo, 255);
            this.edProductName.Text = szInfo.ToString();
            TwainAPI.DTWAIN_GetSourceProductFamily(m_Source, szInfo, 255);
            this.edFamilyName.Text = szInfo.ToString();
            TwainAPI.DTWAIN_GetSourceManufacturer(m_Source, szInfo, 255);
            this.edManufacturer.Text = szInfo.ToString();
            TwainAPI.DTWAIN_GetSourceVersionInfo(m_Source, szInfo, 255);
            this.edVersionInfo.Text = szInfo.ToString();

            int lMajor = 0, lMinor = 0;
            TwainAPI.DTWAIN_GetSourceVersionNumber(m_Source, ref lMajor, ref lMinor);
            string sVersion = lMajor.ToString() + "." + lMinor.ToString();
            this.edVersion.Text = sVersion;

            DTWAIN_ARRAY AllCaps = IntPtr.Zero;
            int Val = 0;
            TwainAPI.DTWAIN_EnumSupportedCaps(m_Source, ref AllCaps);
            int nSize = TwainAPI.DTWAIN_ArrayGetCount(AllCaps);
            for (int i = 0; i < nSize; ++i)
            {
                // get the cap value
                TwainAPI.DTWAIN_ArrayGetAtLong(AllCaps, i, ref Val);

                // get the name from the cap
                TwainAPI.DTWAIN_GetNameFromCap(Val, szInfo, 255);
                this.listCaps.Items.Add(szInfo.ToString());
            }

            this.edTotalCaps.Text = nSize.ToString();
            TwainAPI.DTWAIN_EnumCustomCaps(m_Source, ref AllCaps);
            this.edCustomCaps.Text = TwainAPI.DTWAIN_ArrayGetCount(AllCaps).ToString();
            TwainAPI.DTWAIN_EnumExtendedCaps(m_Source, ref AllCaps);
            this.edExtendedCaps.Text = TwainAPI.DTWAIN_ArrayGetCount(AllCaps).ToString();

            int customDSLength = 0;
            Encoding enc8 = Encoding.UTF8;
            TwainAPI.DTWAIN_GetCustomDSData(m_Source, IntPtr.Zero, 0, ref customDSLength, TwainAPI.DTWAINGCD_COPYDATA);
            byte [] szCustomData = new byte[customDSLength];
            TwainAPI.DTWAIN_GetCustomDSData(m_Source, szCustomData, customDSLength, ref customDSLength, TwainAPI.DTWAINGCD_COPYDATA);
            this.txtDSData.Text = enc8.GetString(szCustomData, 0, customDSLength);
        }
    }
}
