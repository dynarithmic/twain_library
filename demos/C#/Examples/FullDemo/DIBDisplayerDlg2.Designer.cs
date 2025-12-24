
namespace TWAINDemo
{
    partial class DIBDisplayerDlg2
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dibBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dibBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(94, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Keep";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(251, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Discard";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // dibBox2
            // 
            this.dibBox2.Location = new System.Drawing.Point(11, 51);
            this.dibBox2.Name = "dibBox2";
            this.dibBox2.Size = new System.Drawing.Size(387, 511);
            this.dibBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dibBox2.TabIndex = 2;
            this.dibBox2.TabStop = false;
            // 
            // DIBDisplayerDlg2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 656);
            this.Controls.Add(this.dibBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "DIBDisplayerDlg2";
            this.Text = "Keep or Discard Image?";
            this.Load += new System.EventHandler(this.DIBDisplayerDlg2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dibBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox dibBox2;
    }
}