<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomSelectSource
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
        Me.editSourceInfo = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnSelect = New System.Windows.Forms.Button
        Me.listSources = New System.Windows.Forms.ListBox
        Me.label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'editSourceInfo
        '
        Me.editSourceInfo.BackColor = System.Drawing.SystemColors.Control
        Me.editSourceInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.editSourceInfo.Enabled = False
        Me.editSourceInfo.Location = New System.Drawing.Point(18, 224)
        Me.editSourceInfo.Name = "editSourceInfo"
        Me.editSourceInfo.Size = New System.Drawing.Size(270, 13)
        Me.editSourceInfo.TabIndex = 8
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(296, 74)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSelect
        '
        Me.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSelect.Location = New System.Drawing.Point(295, 45)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(75, 23)
        Me.btnSelect.TabIndex = 6
        Me.btnSelect.Text = "&Select"
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'listSources
        '
        Me.listSources.FormattingEnabled = True
        Me.listSources.Location = New System.Drawing.Point(18, 32)
        Me.listSources.Name = "listSources"
        Me.listSources.Size = New System.Drawing.Size(242, 186)
        Me.listSources.Sorted = True
        Me.listSources.TabIndex = 5
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(15, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(114, 13)
        Me.label1.TabIndex = 4
        Me.label1.Text = "Sorted Source Names:"
        '
        'CustomSelectSource
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 238)
        Me.Controls.Add(Me.editSourceInfo)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.listSources)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CustomSelectSource"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CustomSelectSource"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents editSourceInfo As System.Windows.Forms.TextBox
    Private WithEvents btnCancel As System.Windows.Forms.Button
    Private WithEvents btnSelect As System.Windows.Forms.Button
    Public WithEvents listSources As System.Windows.Forms.ListBox
    Private WithEvents label1 As System.Windows.Forms.Label

End Class
