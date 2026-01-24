
namespace TWAINDemo
{
    partial class TestCapDlg
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbGetTypes = new System.Windows.Forms.ComboBox();
            this.cmbContainer = new System.Windows.Forms.ComboBox();
            this.cmbDataType = new System.Windows.Forms.ComboBox();
            this.btnStartTest = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lstResultsSet = new System.Windows.Forms.ListBox();
            this.lblResultsSet = new System.Windows.Forms.Label();
            this.btnSetRevert = new System.Windows.Forms.Button();
            this.btnTestSet = new System.Windows.Forms.Button();
            this.cmbDataTypeSet = new System.Windows.Forms.ComboBox();
            this.cmbContainerSet = new System.Windows.Forms.ComboBox();
            this.cmbSetTypes = new System.Windows.Forms.ComboBox();
            this.lblSetDataType = new System.Windows.Forms.Label();
            this.lblSetContainer = new System.Windows.Forms.Label();
            this.lblSetOperation = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.editInputData = new System.Windows.Forms.TextBox();
            this.lblTestGetResults = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(403, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Note: Testing using non-default container or data type may have undesirable resul" +
    "ts.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Get Operation:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(157, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Container:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(318, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Data Type:";
            // 
            // cmbGetTypes
            // 
            this.cmbGetTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGetTypes.FormattingEnabled = true;
            this.cmbGetTypes.Location = new System.Drawing.Point(17, 55);
            this.cmbGetTypes.Name = "cmbGetTypes";
            this.cmbGetTypes.Size = new System.Drawing.Size(134, 21);
            this.cmbGetTypes.TabIndex = 4;
            this.cmbGetTypes.SelectedIndexChanged += new System.EventHandler(this.cmbGetTypes_SelectedIndexChanged);
            // 
            // cmbContainer
            // 
            this.cmbContainer.FormattingEnabled = true;
            this.cmbContainer.Location = new System.Drawing.Point(157, 55);
            this.cmbContainer.Name = "cmbContainer";
            this.cmbContainer.Size = new System.Drawing.Size(145, 21);
            this.cmbContainer.TabIndex = 5;
            // 
            // cmbDataType
            // 
            this.cmbDataType.FormattingEnabled = true;
            this.cmbDataType.Location = new System.Drawing.Point(318, 55);
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.Size = new System.Drawing.Size(121, 21);
            this.cmbDataType.TabIndex = 6;
            // 
            // btnStartTest
            // 
            this.btnStartTest.Location = new System.Drawing.Point(240, 88);
            this.btnStartTest.Name = "btnStartTest";
            this.btnStartTest.Size = new System.Drawing.Size(75, 23);
            this.btnStartTest.TabIndex = 7;
            this.btnStartTest.Text = "Test";
            this.btnStartTest.UseVisualStyleBackColor = true;
            this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(451, 55);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Revert";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Results:";
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(17, 128);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(509, 95);
            this.lstResults.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.AllowDrop = true;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(240, 447);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(14, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(526, 2);
            this.label6.TabIndex = 12;
            // 
            // lstResultsSet
            // 
            this.lstResultsSet.FormattingEnabled = true;
            this.lstResultsSet.HorizontalExtent = 500;
            this.lstResultsSet.HorizontalScrollbar = true;
            this.lstResultsSet.Location = new System.Drawing.Point(313, 353);
            this.lstResultsSet.Name = "lstResultsSet";
            this.lstResultsSet.Size = new System.Drawing.Size(213, 82);
            this.lstResultsSet.TabIndex = 22;
            // 
            // lblResultsSet
            // 
            this.lblResultsSet.AutoSize = true;
            this.lblResultsSet.Location = new System.Drawing.Point(312, 337);
            this.lblResultsSet.Name = "lblResultsSet";
            this.lblResultsSet.Size = new System.Drawing.Size(45, 13);
            this.lblResultsSet.TabIndex = 21;
            this.lblResultsSet.Text = "Results:";
            // 
            // btnSetRevert
            // 
            this.btnSetRevert.Location = new System.Drawing.Point(451, 280);
            this.btnSetRevert.Name = "btnSetRevert";
            this.btnSetRevert.Size = new System.Drawing.Size(75, 23);
            this.btnSetRevert.TabIndex = 20;
            this.btnSetRevert.Text = "Revert";
            this.btnSetRevert.UseVisualStyleBackColor = true;
            this.btnSetRevert.Click += new System.EventHandler(this.btnSetRevert_Click);
            // 
            // btnTestSet
            // 
            this.btnTestSet.Location = new System.Drawing.Point(240, 313);
            this.btnTestSet.Name = "btnTestSet";
            this.btnTestSet.Size = new System.Drawing.Size(75, 23);
            this.btnTestSet.TabIndex = 19;
            this.btnTestSet.Text = "Test";
            this.btnTestSet.UseVisualStyleBackColor = true;
            this.btnTestSet.Click += new System.EventHandler(this.btnTestSet_Click);
            // 
            // cmbDataTypeSet
            // 
            this.cmbDataTypeSet.FormattingEnabled = true;
            this.cmbDataTypeSet.Location = new System.Drawing.Point(318, 280);
            this.cmbDataTypeSet.Name = "cmbDataTypeSet";
            this.cmbDataTypeSet.Size = new System.Drawing.Size(121, 21);
            this.cmbDataTypeSet.TabIndex = 18;
            // 
            // cmbContainerSet
            // 
            this.cmbContainerSet.FormattingEnabled = true;
            this.cmbContainerSet.Location = new System.Drawing.Point(157, 280);
            this.cmbContainerSet.Name = "cmbContainerSet";
            this.cmbContainerSet.Size = new System.Drawing.Size(145, 21);
            this.cmbContainerSet.TabIndex = 17;
            // 
            // cmbSetTypes
            // 
            this.cmbSetTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSetTypes.FormattingEnabled = true;
            this.cmbSetTypes.Location = new System.Drawing.Point(17, 280);
            this.cmbSetTypes.Name = "cmbSetTypes";
            this.cmbSetTypes.Size = new System.Drawing.Size(134, 21);
            this.cmbSetTypes.TabIndex = 16;
            this.cmbSetTypes.SelectedIndexChanged += new System.EventHandler(this.cmbSetTypes_SelectedIndexChanged);
            // 
            // lblSetDataType
            // 
            this.lblSetDataType.AutoSize = true;
            this.lblSetDataType.Location = new System.Drawing.Point(318, 263);
            this.lblSetDataType.Name = "lblSetDataType";
            this.lblSetDataType.Size = new System.Drawing.Size(60, 13);
            this.lblSetDataType.TabIndex = 15;
            this.lblSetDataType.Text = "Data Type:";
            // 
            // lblSetContainer
            // 
            this.lblSetContainer.AutoSize = true;
            this.lblSetContainer.Location = new System.Drawing.Point(157, 263);
            this.lblSetContainer.Name = "lblSetContainer";
            this.lblSetContainer.Size = new System.Drawing.Size(55, 13);
            this.lblSetContainer.TabIndex = 14;
            this.lblSetContainer.Text = "Container:";
            // 
            // lblSetOperation
            // 
            this.lblSetOperation.AutoSize = true;
            this.lblSetOperation.Location = new System.Drawing.Point(17, 263);
            this.lblSetOperation.Name = "lblSetOperation";
            this.lblSetOperation.Size = new System.Drawing.Size(75, 13);
            this.lblSetOperation.TabIndex = 13;
            this.lblSetOperation.Text = "Set Operation:";
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(22, 337);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(146, 13);
            this.lblInput.TabIndex = 23;
            this.lblInput.Text = "Input (One data item per line):";
            // 
            // editInputData
            // 
            this.editInputData.Location = new System.Drawing.Point(17, 353);
            this.editInputData.Multiline = true;
            this.editInputData.Name = "editInputData";
            this.editInputData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editInputData.Size = new System.Drawing.Size(208, 84);
            this.editInputData.TabIndex = 24;
            // 
            // lblTestGetResults
            // 
            this.lblTestGetResults.AutoSize = true;
            this.lblTestGetResults.Location = new System.Drawing.Point(318, 94);
            this.lblTestGetResults.Name = "lblTestGetResults";
            this.lblTestGetResults.Size = new System.Drawing.Size(0, 13);
            this.lblTestGetResults.TabIndex = 25;
            // 
            // TestCapDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 482);
            this.Controls.Add(this.lblTestGetResults);
            this.Controls.Add(this.editInputData);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.lstResultsSet);
            this.Controls.Add(this.lblResultsSet);
            this.Controls.Add(this.btnSetRevert);
            this.Controls.Add(this.btnTestSet);
            this.Controls.Add(this.cmbDataTypeSet);
            this.Controls.Add(this.cmbContainerSet);
            this.Controls.Add(this.cmbSetTypes);
            this.Controls.Add(this.lblSetDataType);
            this.Controls.Add(this.lblSetContainer);
            this.Controls.Add(this.lblSetOperation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnStartTest);
            this.Controls.Add(this.cmbDataType);
            this.Controls.Add(this.cmbContainer);
            this.Controls.Add(this.cmbGetTypes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TestCapDlg";
            this.Text = "TestCapDlg";
            this.Load += new System.EventHandler(this.TestCapDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbGetTypes;
        private System.Windows.Forms.ComboBox cmbContainer;
        private System.Windows.Forms.ComboBox cmbDataType;
        private System.Windows.Forms.Button btnStartTest;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lstResultsSet;
        private System.Windows.Forms.Label lblResultsSet;
        private System.Windows.Forms.Button btnSetRevert;
        private System.Windows.Forms.Button btnTestSet;
        private System.Windows.Forms.ComboBox cmbDataTypeSet;
        private System.Windows.Forms.ComboBox cmbContainerSet;
        private System.Windows.Forms.ComboBox cmbSetTypes;
        private System.Windows.Forms.Label lblSetDataType;
        private System.Windows.Forms.Label lblSetContainer;
        private System.Windows.Forms.Label lblSetOperation;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.TextBox editInputData;
        private System.Windows.Forms.Label lblTestGetResults;
    }
}