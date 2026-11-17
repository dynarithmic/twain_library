Imports System
Imports System.Windows.Forms

Partial Public Class GeneralOptionsDlg
    Public Class GeneralOptionsData
        Public sJXLPath As String = ""
    End Class

    Private gOptionsData As New GeneralOptionsData()

    ' The parameterless constructor is useful for the Windows Forms designer.
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(gData As GeneralOptionsData)
        Me.New()
        gOptionsData = gData
    End Sub

    Private Sub txtJXLPath_TextChanged(sender As Object, e As EventArgs) Handles txtJXLPath.TextChanged
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        gOptionsData.sJXLPath = txtJXLPath.Text
    End Sub

    ' Preserve the original C# method name, including its spelling.
    Public Function GetGeneraoOptionsData() As GeneralOptionsData
        Return gOptionsData
    End Function

    Private Sub GeneralOptionsDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtJXLPath.Text = gOptionsData.sJXLPath
    End Sub
End Class
