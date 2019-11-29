Imports System.Windows.Forms

Public Class LogFileSelectionDlg

    Private sFileName As String
    Private nWhichOption As Integer

    Public Sub New(ByVal item As Integer)
        nWhichOption = 1
        InitializeComponent() ' This call is required by the Windows Form Designer.
    End Sub

    Public Function GetDebugOption() As Integer
        Return nWhichOption
    End Function

    Public Function GetFileName() As String
        Return sFileName
    End Function

    Private Sub radioLogToFile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioLogToFile.CheckedChanged
        edFileName.Enabled = radioLogToFile.Checked
    End Sub

    Private Sub OKbutton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKbutton.Click
        If radioLogToFile.Checked Then
            sFileName = edFileName.Text
            nWhichOption = 2
        ElseIf radioLogDebugMonitor.Checked Then
            nWhichOption = 3
        End If
    End Sub

    Private Sub LogFileSelectionDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
