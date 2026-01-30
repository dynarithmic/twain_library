using System;
using System.Windows.Forms;
using Dynarithmic;
using DTWAIN_SOURCE = System.IntPtr;
using DTWAIN_ARRAY = System.IntPtr;
using System.Text;

namespace TWAINDemo
{
    public partial class BarCodesDlg : Form
    {
        public BarCodesDlg()
        {
            InitializeComponent();
        }
        private DTWAIN_SOURCE m_Source;

        public BarCodesDlg(DTWAIN_SOURCE theSource)
        {
            m_Source = theSource;
            InitializeComponent();
        }
        private void BarCodesDlg_Load(object sender, EventArgs e)
        {
            int bOk = DTwainDemo.TwainAPI.DTWAIN_InitExtImageInfo(m_Source);
            if (bOk == 0)
            {
                MessageBox.Show("Cannot load bar code information.  Extended Information error.");
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            DTWAIN_ARRAY aText = IntPtr.Zero;
            DTWAIN_ARRAY aType = IntPtr.Zero;
            DTWAIN_ARRAY aCount = IntPtr.Zero;

            // Get the bar code count 
            bOk = DTwainDemo.TwainAPI.DTWAIN_GetExtImageInfoData(m_Source, TwainAPI.DTWAIN_EI_BARCODECOUNT, ref aCount);
            if (bOk == 1 && DTwainDemo.TwainAPI.DTWAIN_ArrayGetCount(aCount) > 0)
            {
                int barCount = 0;
                DTwainDemo.TwainAPI.DTWAIN_ArrayGetAtLong(aCount, 0, ref barCount);
                DTwainDemo.TwainAPI.DTWAIN_ArrayDestroy(aCount);
                if (barCount == 0)
                {
                    MessageBox.Show("No bar codes found.");
                    this.DialogResult = DialogResult.Cancel;

                    // Release the memory 
                    DTwainDemo.TwainAPI.DTWAIN_FreeExtImageInfo(m_Source);
                    return;
                }

                StringBuilder szOrigText = new StringBuilder();
                szOrigText.AppendFormat("Bar Code Count: {0}\r\n\r\n", barCount);

                // Get all of the bar code texts and the bar code types 
                bOk = DTwainDemo.TwainAPI.DTWAIN_GetExtImageInfoData(m_Source, TwainAPI.DTWAIN_EI_BARCODETEXT, ref aText);
                bOk = DTwainDemo.TwainAPI.DTWAIN_GetExtImageInfoData(m_Source, TwainAPI.DTWAIN_EI_BARCODETYPE, ref aType);
                if (bOk == 1)
                {
                    // Display each bar code text and type
                    StringBuilder oneString = new StringBuilder(1024);
                    for (int i = 0; i < barCount; ++i)
                    {
                        szOrigText.AppendFormat("Bar Code {0}:\r\n", i + 1);

                        // Get the bar code text associated with bar code i + 1 
                        DTwainDemo.TwainAPI.DTWAIN_ArrayGetAtANSIString(aText, i, oneString);
                        szOrigText.AppendFormat("     Text: {0}\r\n", oneString.ToString());
                        int nType = 0;
                        DTwainDemo.TwainAPI.DTWAIN_ArrayGetAtLong(aType, i, ref nType);
                        StringBuilder szType = new StringBuilder(100);

                        // Translate the bar code type to a string defined by the TWAIN specification
                        DTwainDemo.TwainAPI.DTWAIN_GetTwainNameFromConstantEx(TwainAPI.DTWAIN_CONSTANT_TWBT, nType, szType, 100);
                        szOrigText.AppendFormat("     Type: {0}\r\n\r\n", szType.ToString());
                    }

                    // Set the edit control to the text 
                    this.txtBarCodes.Text = szOrigText.ToString();
                }
                DTwainDemo.TwainAPI.DTWAIN_ArrayDestroy(aText);
                DTwainDemo.TwainAPI.DTWAIN_ArrayDestroy(aType);
            }
        }
    }
}
