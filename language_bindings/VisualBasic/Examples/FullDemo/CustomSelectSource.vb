Imports System.Windows.Forms

Public Class CustomSelectSource
    Private sourceSelected As Boolean
    Private sSourceName As String
    Public Function IsSourceSelected() As Boolean
        Return sourceSelected
    End Function
    Public Function GetSourceName() As String
        Return sSourceName
    End Function

    Private Sub CustomSelectSource_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        sourceSelected = False
        Dim SourceArray As Integer = 0
        DTWAINAPI.DTWAIN_EnumSources(SourceArray)
        Dim nCount As Integer = DTWAINAPI.DTWAIN_ArrayGetCount(SourceArray)
        If nCount <= 0 Then
            Close()
        End If

        ' Display the sources
        Dim CurSource As Integer = 0
        For i As Integer = 0 To nCount - 1
            Dim szName As String
            szName = Space$(256)
            DTWAINAPI.DTWAIN_ArrayGetAtLong(SourceArray, i, CurSource)
            DTWAINAPI.DTWAIN_GetSourceProductName(CurSource, szName, 255)
            listSources.Items.Add(szName.ToString())
        Next
        listSources.SelectedIndex = 0
        ' Display Info about sources
        Dim sText As String = nCount.ToString() & " TWAIN Source(s) Available for Selection"
        editSourceInfo.Text = sText
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        sSourceName = listSources.SelectedItem.ToString()
        sourceSelected = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        sourceSelected = False
        Me.Close()
    End Sub
End Class
