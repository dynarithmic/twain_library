Public Class CustomLanguageDlg
    Private Sub CustomLanguageDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Public Function GetText() As String
        Return Me.textCustomLanguageName.Text
    End Function
End Class
