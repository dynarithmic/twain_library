namespace TWAINDemo
{
    partial class DIBDisplayerDlg
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
            this.dibBox = new System.Windows.Forms.PictureBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAcquisition = new System.Windows.Forms.ComboBox();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.edPageCurrent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.edPageTotal = new System.Windows.Forms.TextBox();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.dibBox)).BeginInit();
            this.SuspendLayout();
            // 
            // dibBox
            // 
            this.dibBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dibBox.Location = new System.Drawing.Point(12, 3);
            this.dibBox.Name = "dibBox";
            this.dibBox.Size = new System.Drawing.Size(343, 391);
            this.dibBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dibBox.TabIndex = 0;
            this.dibBox.TabStop = false;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(361, 12);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 410);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Acquisition:";
            // 
            // cmbAcquisition
            // 
            this.cmbAcquisition.FormattingEnabled = true;
            this.cmbAcquisition.Location = new System.Drawing.Point(81, 407);
            this.cmbAcquisition.Name = "cmbAcquisition";
            this.cmbAcquisition.Size = new System.Drawing.Size(71, 21);
            this.cmbAcquisition.TabIndex = 3;
            this.cmbAcquisition.SelectedIndexChanged += new System.EventHandler(this.cmbAcquisition_SelectedIndexChanged);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Location = new System.Drawing.Point(191, 404);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(75, 23);
            this.buttonPrev.TabIndex = 4;
            this.buttonPrev.Text = "&Previous";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(272, 404);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.Text = "&Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(361, 407);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Page";
            // 
            // edPageCurrent
            // 
            this.edPageCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.edPageCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.edPageCurrent.Enabled = false;
            this.edPageCurrent.Location = new System.Drawing.Point(396, 408);
            this.edPageCurrent.Name = "edPageCurrent";
            this.edPageCurrent.Size = new System.Drawing.Size(24, 13);
            this.edPageCurrent.TabIndex = 6;
            this.edPageCurrent.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(427, 407);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Of";
            // 
            // edPageTotal
            // 
            this.edPageTotal.BackColor = System.Drawing.SystemColors.Control;
            this.edPageTotal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.edPageTotal.Enabled = false;
            this.edPageTotal.Location = new System.Drawing.Point(449, 408);
            this.edPageTotal.Name = "edPageTotal";
            this.edPageTotal.Size = new System.Drawing.Size(24, 13);
            this.edPageTotal.TabIndex = 6;
            this.edPageTotal.TabStop = false;
            // 
            // DIBDisplayerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 457);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edPageTotal);
            this.Controls.Add(this.edPageCurrent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.cmbAcquisition);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.dibBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "DIBDisplayerDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitmap Displayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DIBDispalyerDlg_FormClosing);
            this.Load += new System.EventHandler(this.DIBDisplayerDlg_Load);
            this.Leave += new System.EventHandler(this.DIBDisplayreDlg_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.dibBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox dibBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAcquisition;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edPageCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edPageTotal;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}