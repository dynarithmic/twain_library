<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileTypeDlg
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
        Me.OKbutton = New System.Windows.Forms.Button
        Me.edFileName = New System.Windows.Forms.TextBox
        Me.cmbFileType = New System.Windows.Forms.ComboBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'OKbutton
        '
        Me.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKbutton.Location = New System.Drawing.Point(97, 183)
        Me.OKbutton.Name = "OKbutton"
        Me.OKbutton.Size = New System.Drawing.Size(75, 30)
        Me.OKbutton.TabIndex = 8
        Me.OKbutton.Text = "OK"
        Me.OKbutton.UseVisualStyleBackColor = True
        '
        'edFileName
        '
        Me.edFileName.Location = New System.Drawing.Point(20, 133)
        Me.edFileName.Name = "edFileName"
        Me.edFileName.Size = New System.Drawing.Size(223, 20)
        Me.edFileName.TabIndex = 7
        '
        'cmbFileType
        '
        Me.cmbFileType.FormattingEnabled = True
        Me.cmbFileType.Location = New System.Drawing.Point(18, 35)
        Me.cmbFileType.Name = "cmbFileType"
        Me.cmbFileType.Size = New System.Drawing.Size(225, 21)
        Me.cmbFileType.TabIndex = 6
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(17, 116)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(96, 13)
        Me.label2.TabIndex = 4
        Me.label2.Text = "Choose File Name:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(15, 18)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(92, 13)
        Me.label1.TabIndex = 5
        Me.label1.Text = "Choose File Type:"
        '
        'FileTypeDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(265, 229)
        Me.Controls.Add(Me.OKbutton)
        Me.Controls.Add(Me.edFileName)
        Me.Controls.Add(Me.cmbFileType)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FileTypeDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DTWAIN File Types"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents OKbutton As System.Windows.Forms.Button
    Private WithEvents edFileName As System.Windows.Forms.TextBox
    Private WithEvents cmbFileType As System.Windows.Forms.ComboBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label

End Class
