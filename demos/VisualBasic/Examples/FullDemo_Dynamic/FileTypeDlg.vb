Imports System.Windows.Forms

Public Class FileTypeDlg
    Private selectedFileName As String


    Public Sub New()
        selectedFileName = "test.bmp"
        InitializeComponent()
    End Sub
    Public Function GetFileName() As String
        Return selectedFileName
    End Function

    Private Sub FileTypeDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub OKbutton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKbutton.Click
        selectedFileName = edFileName.Text
    End Sub
End Class
