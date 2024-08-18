namespace TWAINDemo
{
    partial class LogFileSelectionDlg
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
            this.radioNoLogging = new System.Windows.Forms.RadioButton();
            this.radioLogToFile = new System.Windows.Forms.RadioButton();
            this.radioLogDebugMonitor = new System.Windows.Forms.RadioButton();
            this.OKbutton = new System.Windows.Forms.Button();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.edFileName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // radioNoLogging
            // 
            this.radioNoLogging.AutoSize = true;
            this.radioNoLogging.Location = new System.Drawing.Point(22, 42);
            this.radioNoLogging.Name = "radioNoLogging";
            this.radioNoLogging.Size = new System.Drawing.Size(80, 17);
            this.radioNoLogging.TabIndex = 0;
            this.radioNoLogging.TabStop = true;
            this.radioNoLogging.Text = "No Logging";
            this.radioNoLogging.UseVisualStyleBackColor = true;
            // 
            // radioLogToFile
            // 
            this.radioLogToFile.AutoSize = true;
            this.radioLogToFile.Location = new System.Drawing.Point(22, 65);
            this.radioLogToFile.Name = "radioLogToFile";
            this.radioLogToFile.Size = new System.Drawing.Size(77, 17);
            this.radioLogToFile.TabIndex = 1;
            this.radioLogToFile.TabStop = true;
            this.radioLogToFile.Text = "Log to File:";
            this.radioLogToFile.UseVisualStyleBackColor = true;
            this.radioLogToFile.CheckedChanged += new System.EventHandler(this.radioLogToFile_CheckedChanged);
            // 
            // radioLogDebugMonitor
            // 
            this.radioLogDebugMonitor.AutoSize = true;
            this.radioLogDebugMonitor.Location = new System.Drawing.Point(22, 91);
            this.radioLogDebugMonitor.Name = "radioLogDebugMonitor";
            this.radioLogDebugMonitor.Size = new System.Drawing.Size(128, 17);
            this.radioLogDebugMonitor.TabIndex = 3;
            this.radioLogDebugMonitor.TabStop = true;
            this.radioLogDebugMonitor.Text = "Log to Debug Monitor";
            this.radioLogDebugMonitor.UseVisualStyleBackColor = true;
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbutton.Location = new System.Drawing.Point(68, 139);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 4;
            this.OKbutton.Text = "&OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Location = new System.Drawing.Point(208, 139);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.Cancelbutton.TabIndex = 5;
            this.Cancelbutton.Text = "&Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // edFileName
            // 
            this.edFileName.Enabled = false;
            this.edFileName.Location = new System.Drawing.Point(105, 65);
            this.edFileName.Name = "edFileName";
            this.edFileName.Size = new System.Drawing.Size(240, 20);
            this.edFileName.TabIndex = 2;
            this.edFileName.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(2, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 112);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logging Options";
            // 
            // LogFileSelectionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 175);
            this.Controls.Add(this.edFileName);
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.radioLogDebugMonitor);
            this.Controls.Add(this.radioLogToFile);
            this.Controls.Add(this.radioNoLogging);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "LogFileSelectionDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logging Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioNoLogging;
        private System.Windows.Forms.RadioButton radioLogToFile;
        private System.Windows.Forms.RadioButton radioLogDebugMonitor;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.TextBox edFileName;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}