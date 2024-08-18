<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DibDisplayerDlg2
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
        Me.dibBox2 = New System.Windows.Forms.PictureBox
        Me.button2 = New System.Windows.Forms.Button
        Me.button1 = New System.Windows.Forms.Button
        CType(Me.dibBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dibBox2
        '
        Me.dibBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dibBox2.Location = New System.Drawing.Point(16, 36)
        Me.dibBox2.Name = "dibBox2"
        Me.dibBox2.Size = New System.Drawing.Size(456, 429)
        Me.dibBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.dibBox2.TabIndex = 6
        Me.dibBox2.TabStop = False
        '
        'button2
        '
        Me.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button2.Location = New System.Drawing.Point(300, 8)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(75, 23)
        Me.button2.TabIndex = 5
        Me.button2.Text = "Discard"
        Me.button2.UseVisualStyleBackColor = True
        '
        'button1
        '
        Me.button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button1.Location = New System.Drawing.Point(129, 9)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(75, 23)
        Me.button1.TabIndex = 4
        Me.button1.Text = "Keep"
        Me.button1.UseVisualStyleBackColor = True
        '
        'DibDisplayerDlg2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 481)
        Me.Controls.Add(Me.dibBox2)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DibDisplayerDlg2"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Keep or Discard Current Image?"
        CType(Me.dibBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents dibBox2 As System.Windows.Forms.PictureBox
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents button1 As System.Windows.Forms.Button

End Class
