namespace TWAINDemo
{
    partial class CustomSelectSource
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
            this.listSources = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.editSourceInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sorted Source Names:";
            // 
            // listSources
            // 
            this.listSources.FormattingEnabled = true;
            this.listSources.Location = new System.Drawing.Point(16, 29);
            this.listSources.Name = "listSources";
            this.listSources.Size = new System.Drawing.Size(242, 186);
            this.listSources.Sorted = true;
            this.listSources.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Location = new System.Drawing.Point(293, 42);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "&Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(294, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // editSourceInfo
            // 
            this.editSourceInfo.BackColor = System.Drawing.SystemColors.Control;
            this.editSourceInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.editSourceInfo.Enabled = false;
            this.editSourceInfo.Location = new System.Drawing.Point(16, 221);
            this.editSourceInfo.Name = "editSourceInfo";
            this.editSourceInfo.Size = new System.Drawing.Size(270, 13);
            this.editSourceInfo.TabIndex = 3;
            // 
            // CustomSelectSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 260);
            this.Controls.Add(this.editSourceInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.listSources);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "CustomSelectSource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom Select Source Dialog";
            this.Load += new System.EventHandler(this.CustomSelectSource_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ListBox listSources;
        private System.Windows.Forms.TextBox editSourceInfo;
    }
}