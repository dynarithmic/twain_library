<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PDFOptionsDlg
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

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

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.label1 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label6 = New System.Windows.Forms.Label()
        Me.staticUserPassword = New System.Windows.Forms.Label()
        Me.staticOwnerPassword = New System.Windows.Forms.Label()
        Me.txtPDFTitle = New System.Windows.Forms.TextBox()
        Me.txtPDFSubject = New System.Windows.Forms.TextBox()
        Me.txtPDFAuthor = New System.Windows.Forms.TextBox()
        Me.txtPDFCreator = New System.Windows.Forms.TextBox()
        Me.txtPDFProducer = New System.Windows.Forms.TextBox()
        Me.txtPDFKeywords = New System.Windows.Forms.TextBox()
        Me.txtUserPassword = New System.Windows.Forms.TextBox()
        Me.txtOwnerPassword = New System.Windows.Forms.TextBox()
        Me.chkPDFStampPages = New System.Windows.Forms.CheckBox()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(31, 23)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(30, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Title:"
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(31, 47)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(46, 13)
        Me.label2.TabIndex = 0
        Me.label2.Text = "Subject:"
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(31, 73)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(41, 13)
        Me.label3.TabIndex = 0
        Me.label3.Text = "Author:"
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(31, 100)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(44, 13)
        Me.label4.TabIndex = 0
        Me.label4.Text = "Creator:"
        Me.label5.AutoSize = True
        Me.label5.Enabled = False
        Me.label5.Location = New System.Drawing.Point(31, 128)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(53, 13)
        Me.label5.TabIndex = 0
        Me.label5.Text = "Producer:"
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(31, 155)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(56, 13)
        Me.label6.TabIndex = 0
        Me.label6.Text = "Keywords:"
        Me.staticUserPassword.AutoSize = True
        Me.staticUserPassword.Location = New System.Drawing.Point(31, 185)
        Me.staticUserPassword.Name = "staticUserPassword"
        Me.staticUserPassword.Size = New System.Drawing.Size(81, 13)
        Me.staticUserPassword.TabIndex = 0
        Me.staticUserPassword.Text = "User Password:"
        Me.staticOwnerPassword.AutoSize = True
        Me.staticOwnerPassword.Location = New System.Drawing.Point(31, 214)
        Me.staticOwnerPassword.Name = "staticOwnerPassword"
        Me.staticOwnerPassword.Size = New System.Drawing.Size(90, 13)
        Me.staticOwnerPassword.TabIndex = 0
        Me.staticOwnerPassword.Text = "Owner Password:"
        Me.txtPDFTitle.Location = New System.Drawing.Point(137, 23)
        Me.txtPDFTitle.Name = "txtPDFTitle"
        Me.txtPDFTitle.Size = New System.Drawing.Size(395, 20)
        Me.txtPDFTitle.TabIndex = 1
        Me.txtPDFSubject.Location = New System.Drawing.Point(137, 49)
        Me.txtPDFSubject.Name = "txtPDFSubject"
        Me.txtPDFSubject.Size = New System.Drawing.Size(395, 20)
        Me.txtPDFSubject.TabIndex = 2
        Me.txtPDFAuthor.Location = New System.Drawing.Point(137, 75)
        Me.txtPDFAuthor.Name = "txtPDFAuthor"
        Me.txtPDFAuthor.Size = New System.Drawing.Size(395, 20)
        Me.txtPDFAuthor.TabIndex = 3
        Me.txtPDFCreator.Location = New System.Drawing.Point(137, 101)
        Me.txtPDFCreator.Name = "txtPDFCreator"
        Me.txtPDFCreator.Size = New System.Drawing.Size(395, 20)
        Me.txtPDFCreator.TabIndex = 4
        Me.txtPDFProducer.Enabled = False
        Me.txtPDFProducer.Location = New System.Drawing.Point(137, 127)
        Me.txtPDFProducer.Name = "txtPDFProducer"
        Me.txtPDFProducer.ReadOnly = True
        Me.txtPDFProducer.Size = New System.Drawing.Size(395, 20)
        Me.txtPDFProducer.TabIndex = 1
        Me.txtPDFKeywords.Location = New System.Drawing.Point(137, 153)
        Me.txtPDFKeywords.Name = "txtPDFKeywords"
        Me.txtPDFKeywords.Size = New System.Drawing.Size(395, 20)
        Me.txtPDFKeywords.TabIndex = 5
        Me.txtUserPassword.Location = New System.Drawing.Point(137, 179)
        Me.txtUserPassword.Name = "txtUserPassword"
        Me.txtUserPassword.Size = New System.Drawing.Size(395, 20)
        Me.txtUserPassword.TabIndex = 6
        Me.txtOwnerPassword.Location = New System.Drawing.Point(137, 205)
        Me.txtOwnerPassword.Name = "txtOwnerPassword"
        Me.txtOwnerPassword.Size = New System.Drawing.Size(395, 20)
        Me.txtOwnerPassword.TabIndex = 7
        Me.chkPDFStampPages.AutoSize = True
        Me.chkPDFStampPages.Location = New System.Drawing.Point(41, 261)
        Me.chkPDFStampPages.Name = "chkPDFStampPages"
        Me.chkPDFStampPages.Size = New System.Drawing.Size(113, 17)
        Me.chkPDFStampPages.TabIndex = 8
        Me.chkPDFStampPages.Text = "Stamp PDF Pages"
        Me.chkPDFStampPages.UseVisualStyleBackColor = True
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(168, 305)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 9
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(316, 305)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 10
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 347)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.chkPDFStampPages)
        Me.Controls.Add(Me.txtOwnerPassword)
        Me.Controls.Add(Me.txtUserPassword)
        Me.Controls.Add(Me.txtPDFKeywords)
        Me.Controls.Add(Me.txtPDFProducer)
        Me.Controls.Add(Me.txtPDFCreator)
        Me.Controls.Add(Me.txtPDFAuthor)
        Me.Controls.Add(Me.txtPDFSubject)
        Me.Controls.Add(Me.txtPDFTitle)
        Me.Controls.Add(Me.staticOwnerPassword)
        Me.Controls.Add(Me.staticUserPassword)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Name = "PDFOptionsDlg"
        Me.Text = "PDF Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents label6 As System.Windows.Forms.Label
    Friend WithEvents staticUserPassword As System.Windows.Forms.Label
    Friend WithEvents staticOwnerPassword As System.Windows.Forms.Label
    Friend WithEvents txtPDFTitle As System.Windows.Forms.TextBox
    Friend WithEvents txtPDFSubject As System.Windows.Forms.TextBox
    Friend WithEvents txtPDFAuthor As System.Windows.Forms.TextBox
    Friend WithEvents txtPDFCreator As System.Windows.Forms.TextBox
    Friend WithEvents txtPDFProducer As System.Windows.Forms.TextBox
    Friend WithEvents txtPDFKeywords As System.Windows.Forms.TextBox
    Friend WithEvents txtUserPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtOwnerPassword As System.Windows.Forms.TextBox
    Friend WithEvents chkPDFStampPages As System.Windows.Forms.CheckBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
End Class
