<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SourcePropertiesDlg
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        Me.label8 = New System.Windows.Forms.Label()
        Me.label7 = New System.Windows.Forms.Label()
        Me.label6 = New System.Windows.Forms.Label()
        Me.listCaps = New System.Windows.Forms.ListBox()
        Me.edExtendedCaps = New System.Windows.Forms.TextBox()
        Me.edCustomCaps = New System.Windows.Forms.TextBox()
        Me.edTotalCaps = New System.Windows.Forms.TextBox()
        Me.OK_button = New System.Windows.Forms.Button()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.edVersion = New System.Windows.Forms.TextBox()
        Me.edVersionInfo = New System.Windows.Forms.TextBox()
        Me.edManufacturer = New System.Windows.Forms.TextBox()
        Me.edFamilyName = New System.Windows.Forms.TextBox()
        Me.edProductName = New System.Windows.Forms.TextBox()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.txtDSData = New System.Windows.Forms.TextBox()
        Me.groupBox2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.label8)
        Me.groupBox2.Controls.Add(Me.label7)
        Me.groupBox2.Controls.Add(Me.label6)
        Me.groupBox2.Controls.Add(Me.listCaps)
        Me.groupBox2.Controls.Add(Me.edExtendedCaps)
        Me.groupBox2.Controls.Add(Me.edCustomCaps)
        Me.groupBox2.Controls.Add(Me.edTotalCaps)
        Me.groupBox2.Location = New System.Drawing.Point(19, 198)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(394, 220)
        Me.groupBox2.TabIndex = 5
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Capability Info"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(243, 112)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(82, 13)
        Me.label8.TabIndex = 1
        Me.label8.Text = "Extended Caps:"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(253, 72)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(72, 13)
        Me.label7.TabIndex = 1
        Me.label7.Text = "Custom Caps:"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(264, 32)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(61, 13)
        Me.label6.TabIndex = 1
        Me.label6.Text = "Total Caps:"
        '
        'listCaps
        '
        Me.listCaps.FormattingEnabled = True
        Me.listCaps.HorizontalScrollbar = True
        Me.listCaps.Location = New System.Drawing.Point(10, 20)
        Me.listCaps.Name = "listCaps"
        Me.listCaps.Size = New System.Drawing.Size(217, 186)
        Me.listCaps.Sorted = True
        Me.listCaps.TabIndex = 0
        '
        'edExtendedCaps
        '
        Me.edExtendedCaps.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edExtendedCaps.Location = New System.Drawing.Point(331, 113)
        Me.edExtendedCaps.Name = "edExtendedCaps"
        Me.edExtendedCaps.ReadOnly = True
        Me.edExtendedCaps.Size = New System.Drawing.Size(42, 13)
        Me.edExtendedCaps.TabIndex = 1
        Me.edExtendedCaps.TabStop = False
        '
        'edCustomCaps
        '
        Me.edCustomCaps.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edCustomCaps.Location = New System.Drawing.Point(331, 72)
        Me.edCustomCaps.Name = "edCustomCaps"
        Me.edCustomCaps.ReadOnly = True
        Me.edCustomCaps.Size = New System.Drawing.Size(42, 13)
        Me.edCustomCaps.TabIndex = 1
        Me.edCustomCaps.TabStop = False
        '
        'edTotalCaps
        '
        Me.edTotalCaps.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edTotalCaps.Location = New System.Drawing.Point(331, 32)
        Me.edTotalCaps.Name = "edTotalCaps"
        Me.edTotalCaps.ReadOnly = True
        Me.edTotalCaps.Size = New System.Drawing.Size(42, 13)
        Me.edTotalCaps.TabIndex = 1
        Me.edTotalCaps.TabStop = False
        '
        'OK_button
        '
        Me.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_button.Location = New System.Drawing.Point(337, 424)
        Me.OK_button.Name = "OK_button"
        Me.OK_button.Size = New System.Drawing.Size(75, 23)
        Me.OK_button.TabIndex = 4
        Me.OK_button.Text = "OK"
        Me.OK_button.UseVisualStyleBackColor = True
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.edVersion)
        Me.groupBox1.Controls.Add(Me.edVersionInfo)
        Me.groupBox1.Controls.Add(Me.edManufacturer)
        Me.groupBox1.Controls.Add(Me.edFamilyName)
        Me.groupBox1.Controls.Add(Me.edProductName)
        Me.groupBox1.Controls.Add(Me.label5)
        Me.groupBox1.Controls.Add(Me.label4)
        Me.groupBox1.Controls.Add(Me.label3)
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Location = New System.Drawing.Point(19, 13)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(394, 173)
        Me.groupBox1.TabIndex = 3
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "General Info"
        '
        'edVersion
        '
        Me.edVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edVersion.Location = New System.Drawing.Point(91, 144)
        Me.edVersion.Name = "edVersion"
        Me.edVersion.ReadOnly = True
        Me.edVersion.Size = New System.Drawing.Size(282, 13)
        Me.edVersion.TabIndex = 1
        Me.edVersion.TabStop = False
        '
        'edVersionInfo
        '
        Me.edVersionInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edVersionInfo.Location = New System.Drawing.Point(91, 113)
        Me.edVersionInfo.Name = "edVersionInfo"
        Me.edVersionInfo.ReadOnly = True
        Me.edVersionInfo.Size = New System.Drawing.Size(282, 13)
        Me.edVersionInfo.TabIndex = 1
        Me.edVersionInfo.TabStop = False
        '
        'edManufacturer
        '
        Me.edManufacturer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edManufacturer.Location = New System.Drawing.Point(91, 82)
        Me.edManufacturer.Name = "edManufacturer"
        Me.edManufacturer.ReadOnly = True
        Me.edManufacturer.Size = New System.Drawing.Size(282, 13)
        Me.edManufacturer.TabIndex = 1
        Me.edManufacturer.TabStop = False
        '
        'edFamilyName
        '
        Me.edFamilyName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edFamilyName.Location = New System.Drawing.Point(91, 51)
        Me.edFamilyName.Name = "edFamilyName"
        Me.edFamilyName.ReadOnly = True
        Me.edFamilyName.Size = New System.Drawing.Size(282, 13)
        Me.edFamilyName.TabIndex = 1
        Me.edFamilyName.TabStop = False
        '
        'edProductName
        '
        Me.edProductName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edProductName.Location = New System.Drawing.Point(91, 19)
        Me.edProductName.Name = "edProductName"
        Me.edProductName.ReadOnly = True
        Me.edProductName.Size = New System.Drawing.Size(282, 13)
        Me.edProductName.TabIndex = 1
        Me.edProductName.TabStop = False
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(40, 144)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(45, 13)
        Me.label5.TabIndex = 0
        Me.label5.Text = "Version:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(19, 113)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(66, 13)
        Me.label4.TabIndex = 0
        Me.label4.Text = "Version Info:"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(12, 82)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(73, 13)
        Me.label3.TabIndex = 0
        Me.label3.Text = "Manufacturer:"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(15, 51)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(70, 13)
        Me.label2.TabIndex = 0
        Me.label2.Text = "Family Name:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(7, 20)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(78, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Product Name:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtDSData)
        Me.GroupBox3.Controls.Add(Me.TextBox1)
        Me.GroupBox3.Controls.Add(Me.TextBox2)
        Me.GroupBox3.Controls.Add(Me.TextBox3)
        Me.GroupBox3.Location = New System.Drawing.Point(419, 13)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(317, 405)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Custom Data"
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Location = New System.Drawing.Point(331, 113)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(42, 13)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.TabStop = False
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Location = New System.Drawing.Point(331, 72)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(42, 13)
        Me.TextBox2.TabIndex = 1
        Me.TextBox2.TabStop = False
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Location = New System.Drawing.Point(331, 32)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(42, 13)
        Me.TextBox3.TabIndex = 1
        Me.TextBox3.TabStop = False
        '
        'txtDSData
        '
        Me.txtDSData.Location = New System.Drawing.Point(7, 19)
        Me.txtDSData.Multiline = True
        Me.txtDSData.Name = "txtDSData"
        Me.txtDSData.ReadOnly = True
        Me.txtDSData.Size = New System.Drawing.Size(304, 380)
        Me.txtDSData.TabIndex = 2
        '
        'SourcePropertiesDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 469)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.OK_button)
        Me.Controls.Add(Me.groupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SourcePropertiesDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Source Properties"
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents listCaps As System.Windows.Forms.ListBox
    Private WithEvents edExtendedCaps As System.Windows.Forms.TextBox
    Private WithEvents edCustomCaps As System.Windows.Forms.TextBox
    Private WithEvents edTotalCaps As System.Windows.Forms.TextBox
    Private WithEvents OK_button As System.Windows.Forms.Button
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents edVersion As System.Windows.Forms.TextBox
    Private WithEvents edVersionInfo As System.Windows.Forms.TextBox
    Private WithEvents edManufacturer As System.Windows.Forms.TextBox
    Private WithEvents edFamilyName As System.Windows.Forms.TextBox
    Private WithEvents edProductName As System.Windows.Forms.TextBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents GroupBox3 As Windows.Forms.GroupBox
    Friend WithEvents txtDSData As Windows.Forms.TextBox
    Private WithEvents TextBox1 As Windows.Forms.TextBox
    Private WithEvents TextBox2 As Windows.Forms.TextBox
    Private WithEvents TextBox3 As Windows.Forms.TextBox
End Class
