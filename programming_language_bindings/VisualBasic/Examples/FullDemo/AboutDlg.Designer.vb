<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutDlg
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
        Me.edInfo = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'OKbutton
        '
        Me.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKbutton.Location = New System.Drawing.Point(137, 279)
        Me.OKbutton.Name = "OKbutton"
        Me.OKbutton.Size = New System.Drawing.Size(75, 23)
        Me.OKbutton.TabIndex = 7
        Me.OKbutton.Text = "OK"
        Me.OKbutton.UseVisualStyleBackColor = True
        '
        'edInfo
        '
        Me.edInfo.BackColor = System.Drawing.SystemColors.Control
        Me.edInfo.Location = New System.Drawing.Point(23, 72)
        Me.edInfo.Multiline = True
        Me.edInfo.Name = "edInfo"
        Me.edInfo.ReadOnly = True
        Me.edInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.edInfo.Size = New System.Drawing.Size(301, 201)
        Me.edInfo.TabIndex = 6
        Me.edInfo.TabStop = False
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(23, 46)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(75, 13)
        Me.label2.TabIndex = 5
        Me.label2.Text = "DTWAIN Info:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(20, 8)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(231, 13)
        Me.label1.TabIndex = 4
        Me.label1.Text = "DynaRithmic TWAIN Library Visual Basic Demo"
        '
        'AboutDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(340, 317)
        Me.Controls.Add(Me.OKbutton)
        Me.Controls.Add(Me.edInfo)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DTWAIN Visual Basic Demo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents OKbutton As System.Windows.Forms.Button
    Private WithEvents edInfo As System.Windows.Forms.TextBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label

End Class
