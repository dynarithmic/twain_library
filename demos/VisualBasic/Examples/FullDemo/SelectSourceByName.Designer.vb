<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectSourceByName
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
        Me.label1 = New System.Windows.Forms.Label
        Me.textSourceName = New System.Windows.Forms.TextBox
        Me.button2 = New System.Windows.Forms.Button
        Me.OK_button = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(30, 29)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(75, 13)
        Me.label1.TabIndex = 5
        Me.label1.Text = "Source Name:"
        '
        'textSourceName
        '
        Me.textSourceName.Location = New System.Drawing.Point(111, 26)
        Me.textSourceName.Name = "textSourceName"
        Me.textSourceName.Size = New System.Drawing.Size(253, 20)
        Me.textSourceName.TabIndex = 4
        '
        'button2
        '
        Me.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button2.Location = New System.Drawing.Point(237, 68)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(75, 23)
        Me.button2.TabIndex = 9
        Me.button2.Text = "Cancel"
        Me.button2.UseVisualStyleBackColor = True
        '
        'OK_button
        '
        Me.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_button.Location = New System.Drawing.Point(120, 68)
        Me.OK_button.Name = "OK_button"
        Me.OK_button.Size = New System.Drawing.Size(75, 23)
        Me.OK_button.TabIndex = 8
        Me.OK_button.Text = "OK"
        Me.OK_button.UseVisualStyleBackColor = True
        '
        'SelectSourceByName
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(431, 123)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.OK_button)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.textSourceName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectSourceByName"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Source By Name"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents textSourceName As System.Windows.Forms.TextBox
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents OK_button As System.Windows.Forms.Button

End Class
