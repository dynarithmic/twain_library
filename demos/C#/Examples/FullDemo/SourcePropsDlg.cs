using System;
using System.Text;
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
            StringBuilder szNameInfo = new StringBuilder(256);
            TwainAPI.DTWAIN_GetSourceProductName(m_Source, szNameInfo, 255);
            edProductName.Text = szNameInfo.ToString();
            TwainAPI.DTWAIN_GetSourceProductFamily(m_Source, szInfo, 255);
            edFamilyName.Text = szInfo.ToString();
            TwainAPI.DTWAIN_GetSourceManufacturer(m_Source, szInfo, 255);
            edManufacturer.Text = szInfo.ToString();
            TwainAPI.DTWAIN_GetSourceVersionInfo(m_Source, szInfo, 255);
            edVersionInfo.Text = szInfo.ToString();

            int lMajor = 0, lMinor = 0;
            TwainAPI.DTWAIN_GetSourceVersionNumber(m_Source, ref lMajor, ref lMinor);
            string sVersion = lMajor.ToString() + "." + lMinor.ToString();
            edVersion.Text = sVersion;

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
                listCaps.Items.Add(szInfo.ToString());
            }

            listCaps.SetSelected(0, true);

            edTotalCaps.Text = nSize.ToString();
            TwainAPI.DTWAIN_EnumCustomCaps(m_Source, ref AllCaps);
            edCustomCaps.Text = TwainAPI.DTWAIN_ArrayGetCount(AllCaps).ToString();
            TwainAPI.DTWAIN_EnumExtendedCaps(m_Source, ref AllCaps);
            edExtendedCaps.Text = TwainAPI.DTWAIN_ArrayGetCount(AllCaps).ToString();

            RefreshCustomDSData();

            string sName = szNameInfo.ToString();
            int nBytes = TwainAPI.DTWAIN_GetSourceDetails(sName, null, 0, 2, 1);
            szInfo = new StringBuilder(nBytes);
            TwainAPI.DTWAIN_GetSourceDetails(sName, szInfo, nBytes, 2, 1);

            // Need to convert the JSON new lines to \r\n for edit controls
            txtJSON.Text = szInfo.ToString().Replace("\n", "\r\n");

            TwainAPI.DTWAIN_ArrayDestroy(AllCaps);

            btnShowUIOnly.Enabled = (TwainAPI.DTWAIN_IsUIOnlySupported(m_Source) == 1);
        }

        private void btnTestCap_Click(object sender, EventArgs e)
        {
            TestCapDlg sTestCapDlg = new TestCapDlg(m_Source, listCaps.SelectedItem.ToString());
            sTestCapDlg.ShowDialog(this);
        }
        private void btnResetAllCaps_Click(object sender, EventArgs e)
        {
            TwainAPI.DTWAIN_SetAllCapsToDefault(m_Source);
        }
        private void SourcePropsDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            TwainAPI.DTWAIN_SetAllCapsToDefault(m_Source);
        }

        private void btnShowUIOnly_Click(object sender, EventArgs e)
        {
            btnShowUIOnly.Enabled = false;
            OKbutton.Enabled = false;
            TwainAPI.DTWAIN_ShowUIOnly(m_Source);
            btnShowUIOnly.Enabled = true;
            OKbutton.Enabled = true;
            RefreshCustomDSData();
        }

        private void btnRefreshCustomData_Click(object sender, EventArgs e)
        {
            RefreshCustomDSData();
        }

        private void RefreshCustomDSData()
        {
            uint customDSLength = 0;
            Encoding enc8 = Encoding.UTF8;
            TwainAPI.DTWAIN_GetCustomDSData(m_Source, null, 0, ref customDSLength, TwainAPI.DTWAINGCD_COPYDATA);
            byte[] szCustomData = new byte[customDSLength];
            TwainAPI.DTWAIN_GetCustomDSData(m_Source, szCustomData, customDSLength, ref customDSLength, TwainAPI.DTWAINGCD_COPYDATA);
            txtDSData.Text = enc8.GetString(szCustomData, 0, (int)customDSLength);
        }
        private void SourcePropsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TwainAPI.DTWAIN_IsSourceAcquiringEx(m_Source, 1) == 1)
            {
                MessageBox.Show("You must close the Source user interface before leaving this dialog");
                e.Cancel = true;
            }
        }
        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (TwainAPI.DTWAIN_IsSourceAcquiringEx(m_Source, 1) == 1)
            {
                MessageBox.Show("You must close the Source user interface before leaving this dialog");
            }
            else
                Close();
        }
    }
}
