<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GeneralOptionsDlg
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
        Me.txtJXLPath = New System.Windows.Forms.TextBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(13, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(112, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "JPEG-XL Library Path:"
        '
        'txtJXLPath
        '
        Me.txtJXLPath.Location = New System.Drawing.Point(132, 13)
        Me.txtJXLPath.Name = "txtJXLPath"
        Me.txtJXLPath.Size = New System.Drawing.Size(325, 20)
        Me.txtJXLPath.TabIndex = 1
        '
        'btnOk
        '
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(148, 57)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(245, 57)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GeneralOptionsDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(469, 92)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtJXLPath)
        Me.Controls.Add(Me.label1)
        Me.Name = "GeneralOptionsDlg"
        Me.Text = "Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents txtJXLPath As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
