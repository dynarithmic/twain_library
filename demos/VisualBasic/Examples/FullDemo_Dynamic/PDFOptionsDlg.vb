Imports System
Imports System.Text
Imports Dynarithmic

' Set the VB project Root Namespace to TWAINDemo, or adjust to match your project.
Public Partial Class PDFOptionsDlg
    Inherits Windows.Forms.Form
    Public Class PDFOptionsData
        Public sPDFProducer As String = ""
        Public sPDFCreator As String = ""
        Public sPDFKeywords As String = ""
        Public sPDFTitle As String = ""
        Public sPDFSubject As String = ""
        Public sPDFAuthor As String = ""
        Public sUserPassword As String = ""
        Public sOwnerPassword As String = ""
        Public stamp_pdf As Boolean = False
    End Class

    Private hasPasswords As Boolean = False
    Private pdfOptions As New PDFOptionsData()

    ' Parameterless constructor allows the Visual Studio designer to instantiate the form.
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal passWords As Boolean)
        Me.New()
        hasPasswords = passWords
    End Sub

    Public Function GetPDFOptions() As PDFOptionsData
        Return pdfOptions
    End Function

    Private Sub PDFOptionsDlg_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim szBuf As New StringBuilder(256)
        VB_FullDemo.DTWAINAPI.DTWAIN_GetResourceString(DTWAINAPI.DTWAIN_RESOURCE_COPYRIGHT, szBuf, 256)
        txtPDFProducer.Text = szBuf.ToString()
        txtPDFCreator.Text = "(None)"
        txtPDFKeywords.Text = "(None)"
        txtPDFTitle.Text = "(None)"
        txtPDFSubject.Text = "(None)"
        txtPDFAuthor.Text = "(None)"
        staticUserPassword.Enabled = hasPasswords
        staticOwnerPassword.Enabled = hasPasswords
        txtUserPassword.Enabled = hasPasswords
        txtOwnerPassword.Enabled = hasPasswords
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OKButton.Click
        pdfOptions.sPDFTitle = txtPDFTitle.Text
        pdfOptions.sPDFKeywords = txtPDFKeywords.Text
        pdfOptions.sPDFCreator = txtPDFCreator.Text
        pdfOptions.sPDFProducer = txtPDFProducer.Text
        pdfOptions.sPDFSubject = txtPDFSubject.Text
        pdfOptions.sPDFAuthor = txtPDFAuthor.Text
        pdfOptions.sUserPassword = txtUserPassword.Text
        pdfOptions.sOwnerPassword = txtOwnerPassword.Text
        pdfOptions.stamp_pdf = chkPDFStampPages.Checked
    End Sub
End Class
