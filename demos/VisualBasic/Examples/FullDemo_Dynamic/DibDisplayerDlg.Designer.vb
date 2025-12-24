<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DibDisplayerDlg
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
        Me.label3 = New System.Windows.Forms.Label
        Me.edPageTotal = New System.Windows.Forms.TextBox
        Me.edPageCurrent = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.buttonNext = New System.Windows.Forms.Button
        Me.buttonPrev = New System.Windows.Forms.Button
        Me.cmbAcquisition = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.OkButton = New System.Windows.Forms.Button
        Me.dibBox = New System.Windows.Forms.PictureBox
        CType(Me.dibBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(435, 413)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(18, 13)
        Me.label3.TabIndex = 17
        Me.label3.Text = "Of"
        '
        'edPageTotal
        '
        Me.edPageTotal.BackColor = System.Drawing.SystemColors.Control
        Me.edPageTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edPageTotal.Enabled = False
        Me.edPageTotal.Location = New System.Drawing.Point(457, 414)
        Me.edPageTotal.Name = "edPageTotal"
        Me.edPageTotal.Size = New System.Drawing.Size(24, 13)
        Me.edPageTotal.TabIndex = 15
        Me.edPageTotal.TabStop = False
        '
        'edPageCurrent
        '
        Me.edPageCurrent.BackColor = System.Drawing.SystemColors.Control
        Me.edPageCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edPageCurrent.Enabled = False
        Me.edPageCurrent.Location = New System.Drawing.Point(404, 414)
        Me.edPageCurrent.Name = "edPageCurrent"
        Me.edPageCurrent.Size = New System.Drawing.Size(24, 13)
        Me.edPageCurrent.TabIndex = 16
        Me.edPageCurrent.TabStop = False
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(369, 413)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(32, 13)
        Me.label2.TabIndex = 14
        Me.label2.Text = "Page"
        '
        'buttonNext
        '
        Me.buttonNext.Location = New System.Drawing.Point(280, 410)
        Me.buttonNext.Name = "buttonNext"
        Me.buttonNext.Size = New System.Drawing.Size(75, 23)
        Me.buttonNext.TabIndex = 13
        Me.buttonNext.Text = "&Next"
        Me.buttonNext.UseVisualStyleBackColor = True
        '
        'buttonPrev
        '
        Me.buttonPrev.Location = New System.Drawing.Point(199, 410)
        Me.buttonPrev.Name = "buttonPrev"
        Me.buttonPrev.Size = New System.Drawing.Size(75, 23)
        Me.buttonPrev.TabIndex = 12
        Me.buttonPrev.Text = "&Previous"
        Me.buttonPrev.UseVisualStyleBackColor = True
        '
        'cmbAcquisition
        '
        Me.cmbAcquisition.FormattingEnabled = True
        Me.cmbAcquisition.Location = New System.Drawing.Point(89, 413)
        Me.cmbAcquisition.Name = "cmbAcquisition"
        Me.cmbAcquisition.Size = New System.Drawing.Size(71, 21)
        Me.cmbAcquisition.TabIndex = 11
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(21, 416)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(61, 13)
        Me.label1.TabIndex = 10
        Me.label1.Text = "Acquisition:"
        '
        'OkButton
        '
        Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OkButton.Location = New System.Drawing.Point(336, -46)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 9
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'dibBox
        '
        Me.dibBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dibBox.Location = New System.Drawing.Point(20, 9)
        Me.dibBox.Name = "dibBox"
        Me.dibBox.Size = New System.Drawing.Size(343, 391)
        Me.dibBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.dibBox.TabIndex = 8
        Me.dibBox.TabStop = False
        '
        'DibDisplayerDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(503, 461)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.edPageTotal)
        Me.Controls.Add(Me.edPageCurrent)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.buttonNext)
        Me.Controls.Add(Me.buttonPrev)
        Me.Controls.Add(Me.cmbAcquisition)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.dibBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DibDisplayerDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Image Displayer"
        CType(Me.dibBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents edPageTotal As System.Windows.Forms.TextBox
    Private WithEvents edPageCurrent As System.Windows.Forms.TextBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents buttonNext As System.Windows.Forms.Button
    Private WithEvents buttonPrev As System.Windows.Forms.Button
    Private WithEvents cmbAcquisition As System.Windows.Forms.ComboBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents OkButton As System.Windows.Forms.Button
    Private WithEvents dibBox As System.Windows.Forms.PictureBox

End Class
