<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogFileSelectionDlg
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
        Me.edFileName = New System.Windows.Forms.TextBox
        Me.OKbutton = New System.Windows.Forms.Button
        Me.radioLogDebugMonitor = New System.Windows.Forms.RadioButton
        Me.radioLogToFile = New System.Windows.Forms.RadioButton
        Me.radioNoLogging = New System.Windows.Forms.RadioButton
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'edFileName
        '
        Me.edFileName.Enabled = False
        Me.edFileName.Location = New System.Drawing.Point(118, 73)
        Me.edFileName.Name = "edFileName"
        Me.edFileName.Size = New System.Drawing.Size(240, 20)
        Me.edFileName.TabIndex = 8
        Me.edFileName.TabStop = False
        '
        'OKbutton
        '
        Me.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKbutton.Location = New System.Drawing.Point(81, 147)
        Me.OKbutton.Name = "OKbutton"
        Me.OKbutton.Size = New System.Drawing.Size(75, 23)
        Me.OKbutton.TabIndex = 11
        Me.OKbutton.Text = "&OK"
        Me.OKbutton.UseVisualStyleBackColor = True
        '
        'radioLogDebugMonitor
        '
        Me.radioLogDebugMonitor.AutoSize = True
        Me.radioLogDebugMonitor.Location = New System.Drawing.Point(35, 99)
        Me.radioLogDebugMonitor.Name = "radioLogDebugMonitor"
        Me.radioLogDebugMonitor.Size = New System.Drawing.Size(128, 17)
        Me.radioLogDebugMonitor.TabIndex = 9
        Me.radioLogDebugMonitor.TabStop = True
        Me.radioLogDebugMonitor.Text = "Log to Debug Monitor"
        Me.radioLogDebugMonitor.UseVisualStyleBackColor = True
        '
        'radioLogToFile
        '
        Me.radioLogToFile.AutoSize = True
        Me.radioLogToFile.Location = New System.Drawing.Point(35, 73)
        Me.radioLogToFile.Name = "radioLogToFile"
        Me.radioLogToFile.Size = New System.Drawing.Size(77, 17)
        Me.radioLogToFile.TabIndex = 7
        Me.radioLogToFile.TabStop = True
        Me.radioLogToFile.Text = "Log to File:"
        Me.radioLogToFile.UseVisualStyleBackColor = True
        '
        'radioNoLogging
        '
        Me.radioNoLogging.AutoSize = True
        Me.radioNoLogging.Location = New System.Drawing.Point(35, 50)
        Me.radioNoLogging.Name = "radioNoLogging"
        Me.radioNoLogging.Size = New System.Drawing.Size(80, 17)
        Me.radioNoLogging.TabIndex = 6
        Me.radioNoLogging.TabStop = True
        Me.radioNoLogging.Text = "No Logging"
        Me.radioNoLogging.UseVisualStyleBackColor = True
        '
        'groupBox1
        '
        Me.groupBox1.Location = New System.Drawing.Point(15, 20)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(351, 112)
        Me.groupBox1.TabIndex = 10
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Logging Options"
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.Location = New System.Drawing.Point(199, 147)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "&Cancel"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LogFileSelectionDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(397, 189)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.edFileName)
        Me.Controls.Add(Me.OKbutton)
        Me.Controls.Add(Me.radioLogDebugMonitor)
        Me.Controls.Add(Me.radioLogToFile)
        Me.Controls.Add(Me.radioNoLogging)
        Me.Controls.Add(Me.groupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LogFileSelectionDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Logging Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents edFileName As System.Windows.Forms.TextBox
    '    Private WithEvents Cancelbutton As System.Windows.Forms.Button
    Private WithEvents OKbutton As System.Windows.Forms.Button
    Private WithEvents radioLogDebugMonitor As System.Windows.Forms.RadioButton
    Private WithEvents radioLogToFile As System.Windows.Forms.RadioButton
    Private WithEvents radioNoLogging As System.Windows.Forms.RadioButton
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
