Imports System.Text
Imports System.Windows.Forms

Public Class SourcePropertiesDlg

    Private m_Source As System.IntPtr

    Public Sub New(ByVal item As System.IntPtr)
        InitializeComponent() ' This call is required by the Windows Form Designer.
        m_Source = item
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SourcePropertiesDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim szInfo As New System.Text.StringBuilder(256)
        Dim szInfoName As New System.Text.StringBuilder(256)
        DTWAINAPI.DTWAIN_GetSourceProductName(m_Source, szInfoName, 255)
        Me.edProductName.Text = szInfoName.ToString()
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

        Dim AllCaps As System.IntPtr
        Dim Val As Integer
        DTWAINAPI.DTWAIN_EnumSupportedCaps(m_Source, AllCaps)
        Dim nSize As Integer
        nSize = DTWAINAPI.DTWAIN_ArrayGetCount(AllCaps)
        For i As Integer = 0 To nSize - 1
            DTWAINAPI.DTWAIN_ArrayGetAtLong(AllCaps, i, Val)
            DTWAINAPI.DTWAIN_GetNameFromCap(Val, szInfo, 255)
            Me.listCaps.Items.Add(szInfo.ToString())
        Next i

        listCaps.SetSelected(0, True)
        Me.edTotalCaps.Text = nSize.ToString()
        DTWAINAPI.DTWAIN_EnumCustomCaps(m_Source, AllCaps)
        Me.edCustomCaps.Text = DTWAINAPI.DTWAIN_ArrayGetCount(AllCaps).ToString()
        DTWAINAPI.DTWAIN_EnumExtendedCaps(m_Source, AllCaps)
        Me.edExtendedCaps.Text = DTWAINAPI.DTWAIN_ArrayGetCount(AllCaps).ToString()

        RefreshCustomDSData()

        Dim jsonLength As Integer
        Dim sName As String
        sName = szInfoName.ToString()
        jsonLength = DTWAINAPI.DTWAIN_GetSourceDetails(sName, Nothing, 0, 2, 1)
        szInfo = New StringBuilder(jsonLength)
        DTWAINAPI.DTWAIN_GetSourceDetails(sName, szInfo, jsonLength, 2, 1)

        ' Convert string to one with /r/n, since these are the types of strings for edit controls
        Dim sNewData As String = szInfo.ToString().Replace(vbLf, vbCrLf)
        Me.txtJSON.Text = sNewData
    End Sub
    Private Sub btnTestCap_Click(sender As Object, e As EventArgs) Handles btnTestCap.Click
        Dim sTestCapDlg As TestCapDlg = New TestCapDlg(m_Source, listCaps.SelectedItem.ToString())
        sTestCapDlg.ShowDialog(Me)
    End Sub
    Private Sub btnResetAllCaps_Click(sender As Object, e As EventArgs) Handles btnResetAllCaps.Click
        DTWAINAPI.DTWAIN_SetAllCapsToDefault(m_Source)
    End Sub

    Private Sub btnShowUIOnly_Click(sender As Object, e As EventArgs) Handles btnShowUIOnly.Click
        btnShowUIOnly.Enabled = False
        DTWAINAPI.DTWAIN_ShowUIOnly(m_Source)
        btnShowUIOnly.Enabled = True
        RefreshCustomDSData()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RefreshCustomDSData()
    End Sub

    Private Sub RefreshCustomDSData()
        Dim customDSLength As UInteger
        Dim enc8 As Encoding = Encoding.UTF8
        DTWAINAPI.DTWAIN_GetCustomDSData(m_Source, Nothing, 0, customDSLength, DTWAINAPI.DTWAINGCD_COPYDATA)
        Dim szCustomData(customDSLength) As Byte
        DTWAINAPI.DTWAIN_GetCustomDSData(m_Source, szCustomData, customDSLength, customDSLength, DTWAINAPI.DTWAINGCD_COPYDATA)
        Dim contents As String
        contents = enc8.GetString(szCustomData, 0, customDSLength)
        Me.txtDSData.Text = contents
    End Sub
    Private Sub SourcePropertiesDlg_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If DTWAINAPI.DTWAIN_IsSourceAcquiringEx(m_Source, 1) Then
            MessageBox.Show("You must close the Source user interface before leaving this dialog")
            e.Cancel = True
        End If
    End Sub
End Class
