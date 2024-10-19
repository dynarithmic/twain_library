<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomLanguageDlg
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
        Me.button2 = New System.Windows.Forms.Button()
        Me.OK_button = New System.Windows.Forms.Button()
        Me.label1 = New System.Windows.Forms.Label()
        Me.textCustomLanguageName = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'button2
        '
        Me.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button2.Location = New System.Drawing.Point(234, 84)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(75, 23)
        Me.button2.TabIndex = 13
        Me.button2.Text = "Cancel"
        Me.button2.UseVisualStyleBackColor = True
        '
        'OK_button
        '
        Me.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_button.Location = New System.Drawing.Point(117, 84)
        Me.OK_button.Name = "OK_button"
        Me.OK_button.Size = New System.Drawing.Size(75, 23)
        Me.OK_button.TabIndex = 12
        Me.OK_button.Text = "OK"
        Me.OK_button.UseVisualStyleBackColor = True
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(11, 20)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(127, 13)
        Me.label1.TabIndex = 11
        Me.label1.Text = "Custom Language Name:"
        '
        'textCustomLanguageName
        '
        Me.textCustomLanguageName.Location = New System.Drawing.Point(142, 17)
        Me.textCustomLanguageName.Name = "textCustomLanguageName"
        Me.textCustomLanguageName.Size = New System.Drawing.Size(253, 20)
        Me.textCustomLanguageName.TabIndex = 10
        '
        'CustomLanguageDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(426, 127)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.OK_button)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.textCustomLanguageName)
        Me.Name = "CustomLanguageDlg"
        Me.Text = "Custom Language"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents button2 As Windows.Forms.Button
    Private WithEvents OK_button As Windows.Forms.Button
    Private WithEvents label1 As Windows.Forms.Label
    Private WithEvents textCustomLanguageName As Windows.Forms.TextBox
End Class
