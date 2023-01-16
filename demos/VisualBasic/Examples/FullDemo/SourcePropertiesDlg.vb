Imports System.Windows.Forms

Public Class SourcePropertiesDlg
    Private m_Source As Integer

    Public Sub New(ByVal item As Long)
        InitializeComponent() ' This call is required by the Windows Form Designer.
        m_Source = item
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SourcePropertiesDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim szInfo As New System.Text.StringBuilder(256)
        DTWAINAPI.DTWAIN_GetSourceProductName(m_Source, szInfo, 255)
        Me.edProductName.Text = szInfo.ToString()
        DTWAINAPI.DTWAIN_GetSourceProductFamily(m_Source, szInfo, 255)
        Me.edFamilyName.Text = szInfo.ToString()
        DTWAINAPI.DTWAIN_GetSourceManufacturer(m_Source, szInfo, 255)
        Me.edManufacturer.Text = szInfo.ToString()
        DTWAINAPI.DTWAIN_GetSourceVersionInfo(m_Source, szInfo, 255)
        Me.edVersionInfo.Text = szInfo.ToString()

        Dim lMajor As Integer
        Dim lMinor As Integer
        lMajor = 0
        lMinor = 0
        DTWAINAPI.DTWAIN_GetSourceVersionNumber(m_Source, lMajor, lMinor)
        Dim sVersion As String
        sVersion = lMajor.ToString() + "." + lMinor.ToString()
        Me.edVersion.Text = sVersion

        Dim AllCaps As Integer
        Dim Val As Integer
        DTWAINAPI.DTWAIN_EnumSupportedCaps(m_Source, AllCaps)
        Dim nSize As Integer
        nSize = DTWAINAPI.DTWAIN_ArrayGetCount(AllCaps)
        For i As Integer = 0 To nSize - 1
            DTWAINAPI.DTWAIN_ArrayGetAtLong(AllCaps, i, Val)
            DTWAINAPI.DTWAIN_GetNameFromCap(Val, szInfo, 255)
            Me.listCaps.Items.Add(szInfo.ToString())
        Next i

        Me.edTotalCaps.Text = nSize.ToString()
        DTWAINAPI.DTWAIN_EnumCustomCaps(m_Source, AllCaps)
        Me.edCustomCaps.Text = DTWAINAPI.DTWAIN_ArrayGetCount(AllCaps).ToString()
        DTWAINAPI.DTWAIN_EnumExtendedCaps(m_Source, AllCaps)
        Me.edExtendedCaps.Text = DTWAINAPI.DTWAIN_ArrayGetCount(AllCaps).ToString()
    End Sub
End Class
