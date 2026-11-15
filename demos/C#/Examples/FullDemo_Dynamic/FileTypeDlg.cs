using System;
using System.Windows.Forms;
using Dynarithmic;

namespace TWAINDemo
{
    public partial class FileTypeDlg : Form
    {
        private int selectedFileType;
        private string selectedFileName;

        public int GetFileType()
        {
            return selectedFileType;
        }

        public string GetFileName()
        {
            return selectedFileName;
        }


        public FileTypeDlg()
        {
            selectedFileType = TwainAPI.DTWAIN_BMP;
            selectedFileName = "test.bmp";
            InitializeComponent();
        }

        private void FileTypeDlg_Load(object sender, EventArgs e)
        {
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            selectedFileName = edFileName.Text;
        }
    }
}
