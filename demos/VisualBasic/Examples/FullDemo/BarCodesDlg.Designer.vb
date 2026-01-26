<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BarCodesDlg
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
        Me.OkButton = New System.Windows.Forms.Button()
        Me.txtBarCodes = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'OkButton
        '
        Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OkButton.Location = New System.Drawing.Point(213, 388)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 4
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'txtBarCodes
        '
        Me.txtBarCodes.Location = New System.Drawing.Point(12, 12)
        Me.txtBarCodes.Multiline = True
        Me.txtBarCodes.Name = "txtBarCodes"
        Me.txtBarCodes.ReadOnly = True
        Me.txtBarCodes.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtBarCodes.Size = New System.Drawing.Size(482, 370)
        Me.txtBarCodes.TabIndex = 3
        Me.txtBarCodes.TabStop = False
        '
        'BarCodesDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(519, 429)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.txtBarCodes)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BarCodesDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BarcCodesDlg"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents OkButton As Windows.Forms.Button
    Private WithEvents txtBarCodes As Windows.Forms.TextBox
End Class
