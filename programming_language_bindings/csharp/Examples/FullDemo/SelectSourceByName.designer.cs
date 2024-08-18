namespace TWAINDemo
{
    partial class SelectSourceByNameBox
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
            this.textSourceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textSourceName
            // 
            this.textSourceName.Location = new System.Drawing.Point(103, 12);
            this.textSourceName.Name = "textSourceName";
            this.textSourceName.Size = new System.Drawing.Size(212, 20);
            this.textSourceName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source Name:";
            // 
            // OK_button
            // 
            this.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_button.Location = new System.Drawing.Point(71, 54);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(75, 23);
            this.OK_button.TabIndex = 2;
            this.OK_button.Text = "OK";
            this.OK_button.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(188, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SelectSourceByNameBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 91);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textSourceName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SelectSourceByNameBox";
            this.Text = "Select Source By Name";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textSourceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Button button2;
    }
}