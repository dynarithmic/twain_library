using System;
using System.Text;
using System.Windows.Forms;
using Dynarithmic;

namespace TWAINDemo
{
    public partial class PDFOptionsDlg : Form
    {
        public class PDFOptionsData
        {
            public string sPDFProducer = "";
            public string sPDFCreator = "";
            public string sPDFKeywords = "";
            public string sPDFTitle = "";
            public string sPDFSubject = "";
            public string sPDFAuthor = "";
            public string sUserPassword = "";
            public string sOwnerPassword = "";
            public bool stamp_pdf = false;
        }

        bool hasPasswords = false;
        PDFOptionsData pdfOptions = new PDFOptionsData();

        public PDFOptionsDlg(bool passWords)
        {
            hasPasswords = passWords;
            InitializeComponent();
        }

        public PDFOptionsData GetPDFOptions()
        {
            return pdfOptions;
        }

        private void PDFOptionsDlg_Load(object sender, EventArgs e)
        {
            StringBuilder szBuf = new StringBuilder(256);
            DTwainDemo.TwainAPI.DTWAIN_GetResourceString(TwainAPI.DTWAIN_RESOURCE_COPYRIGHT, szBuf, 256);
            txtPDFProducer.Text = szBuf.ToString();
            txtPDFCreator.Text = "(None)";
            txtPDFKeywords.Text = "(None)";
            txtPDFTitle.Text = "(None)";
            txtPDFSubject.Text = "(None)";
            txtPDFAuthor.Text = "(None)";
            staticUserPassword.Enabled = hasPasswords;
            staticOwnerPassword.Enabled = hasPasswords;
            txtUserPassword.Enabled = hasPasswords;
            txtOwnerPassword.Enabled = hasPasswords;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            pdfOptions.sPDFTitle = txtPDFTitle.Text;
            pdfOptions.sPDFKeywords = txtPDFKeywords.Text;
            pdfOptions.sPDFCreator = txtPDFCreator.Text;
            pdfOptions.sPDFProducer = txtPDFProducer.Text;
            pdfOptions.sPDFSubject = txtPDFSubject.Text;
            pdfOptions.sPDFAuthor = txtPDFAuthor.Text;
            pdfOptions.sUserPassword = txtUserPassword.Text;
            pdfOptions.sOwnerPassword = txtOwnerPassword.Text;
            pdfOptions.stamp_pdf = chkPDFStampPages.Checked;
        }
    }
}
