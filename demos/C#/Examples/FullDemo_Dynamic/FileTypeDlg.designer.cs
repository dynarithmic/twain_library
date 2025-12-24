namespace TWAINDemo
{
    partial class FileTypeDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFileType = new System.Windows.Forms.ComboBox();
            this.edFileName = new System.Windows.Forms.TextBox();
            this.OKbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose File Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Choose File Name:";
            // 
            // cmbFileType
            // 
            this.cmbFileType.FormattingEnabled = true;
            this.cmbFileType.Location = new System.Drawing.Point(16, 30);
            this.cmbFileType.Name = "cmbFileType";
            this.cmbFileType.Size = new System.Drawing.Size(225, 21);
            this.cmbFileType.TabIndex = 1;
            this.cmbFileType.SelectedIndexChanged += new System.EventHandler(this.cmbFileType_SelectedIndexChanged);
            // 
            // edFileName
            // 
            this.edFileName.Location = new System.Drawing.Point(18, 128);
            this.edFileName.Name = "edFileName";
            this.edFileName.Size = new System.Drawing.Size(223, 20);
            this.edFileName.TabIndex = 2;
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbutton.Location = new System.Drawing.Point(95, 178);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 30);
            this.OKbutton.TabIndex = 3;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // FileTypeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 243);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.edFileName);
            this.Controls.Add(this.cmbFileType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FileTypeDlg";
            this.Text = "FileTypeDlg";
            this.Load += new System.EventHandler(this.FileTypeDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbFileType;
        private System.Windows.Forms.TextBox edFileName;
        private System.Windows.Forms.Button OKbutton;
    }
}