Imports System.Windows.Forms

Public Class AboutDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AboutDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim nChars As Integer = DTWAINAPI.DTWAIN_GetVersionInfo(IntPtr.Zero, -1)
        Dim szInfo As New System.Text.StringBuilder(nChars)
        DTWAINAPI.DTWAIN_GetVersionInfo(szInfo, nChars)
        edInfo.Text = szInfo.ToString()
    End Sub
End Class
