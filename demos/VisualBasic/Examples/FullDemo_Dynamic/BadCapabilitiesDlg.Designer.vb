<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BadCapabilitiesDlg
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.label1 = New System.Windows.Forms.Label()
        Me.listBadCaps = New System.Windows.Forms.ListBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 25)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(254, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Bad Capabilities (Unknown TWAIN Container Type):"
        '
        'listBadCaps
        '
        Me.listBadCaps.FormattingEnabled = True
        Me.listBadCaps.Location = New System.Drawing.Point(15, 42)
        Me.listBadCaps.Name = "listBadCaps"
        Me.listBadCaps.Size = New System.Drawing.Size(353, 186)
        Me.listBadCaps.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(164, 243)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'BadCapabilitiesDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 276)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.listBadCaps)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "BadCapabilitiesDlg"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents listBadCaps As System.Windows.Forms.ListBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
