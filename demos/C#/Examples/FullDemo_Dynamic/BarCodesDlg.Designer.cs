
namespace TWAINDemo
{
    partial class BarCodesDlg
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
            this.txtBarCodes = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBarCodes
            // 
            this.txtBarCodes.Location = new System.Drawing.Point(22, 27);
            this.txtBarCodes.Multiline = true;
            this.txtBarCodes.Name = "txtBarCodes";
            this.txtBarCodes.ReadOnly = true;
            this.txtBarCodes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBarCodes.Size = new System.Drawing.Size(482, 370);
            this.txtBarCodes.TabIndex = 0;
            this.txtBarCodes.TabStop = false;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(218, 403);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // BarCodesDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 450);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.txtBarCodes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "BarCodesDlg";
            this.Text = "Bar Codes Found";
            this.Load += new System.EventHandler(this.BarCodesDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBarCodes;
        private System.Windows.Forms.Button OkButton;
    }
}