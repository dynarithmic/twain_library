using System;
using System.Windows.Forms;
using Dynarithmic;

namespace TWAINDemo
{
    public partial class GeneralOptionsDlg : Form
    {
        public class GeneralOptionsData
        {
            public string sJXLPath = "";
        }
        private GeneralOptionsData generalOptionsData = new GeneralOptionsData();

        public GeneralOptionsDlg(GeneralOptionsData gData)
        {
            generalOptionsData = gData;
            InitializeComponent();
        }

        private void txtJXLPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            generalOptionsData.sJXLPath = txtJXLPath.Text;
        }

        public GeneralOptionsData GetGeneraoOptionsData()
        {
            return generalOptionsData;
        }

        private void GeneralOptionsDlg_Load(object sender, EventArgs e)
        {
            txtJXLPath.Text = generalOptionsData.sJXLPath;
        }
    }
}
