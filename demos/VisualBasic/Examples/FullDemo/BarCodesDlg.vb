Imports System.Text
Imports System.Windows.Forms

Public Class BarCodesDlg
    Private m_Source As System.IntPtr

    Public Sub New(ByVal item As System.IntPtr)
        InitializeComponent() ' This call is required by the Windows Form Designer.
        m_Source = item
    End Sub

    Private Sub BarCodesDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim bOk As Integer = DTWAINAPI.DTWAIN_InitExtImageInfo(m_Source)
        If bOk = 0 Then
            MessageBox.Show("Cannot load bar code information.  Extended Information error.")
            Me.DialogResult = DialogResult.Cancel
            Return
        End If

        Dim aText As System.IntPtr = IntPtr.Zero
        Dim aType As System.IntPtr = IntPtr.Zero
        Dim aCount As System.IntPtr = IntPtr.Zero

        ' Get the bar code count 
        bOk = DTWAINAPI.DTWAIN_GetExtImageInfoData(m_Source, DTWAINAPI.DTWAIN_EI_BARCODECOUNT, aCount)
        If bOk = 1 And DTWAINAPI.DTWAIN_ArrayGetCount(aCount) > 0 Then
            Dim barCount As Integer = 0
            DTWAINAPI.DTWAIN_ArrayGetAtLong(aCount, 0, barCount)
            DTWAINAPI.DTWAIN_ArrayDestroy(aCount)
            If barCount = 0 Then
                MessageBox.Show("No bar codes found.")
                Me.DialogResult = DialogResult.Cancel

                ' Release the memory 
                DTWAINAPI.DTWAIN_FreeExtImageInfo(m_Source)
                Return
            End If

            Dim szOrigText As StringBuilder = New StringBuilder()
            szOrigText.AppendFormat("Bar Code Count: {0}\r\n\r\n", barCount)

            ' Get all of the bar code texts And the bar code types 
            bOk = DTWAINAPI.DTWAIN_GetExtImageInfoData(m_Source, DTWAINAPI.DTWAIN_EI_BARCODETEXT, aText)
            bOk = DTWAINAPI.DTWAIN_GetExtImageInfoData(m_Source, DTWAINAPI.DTWAIN_EI_BARCODETYPE, aType)
            If bOk = 1 Then
                ' Display each bar code text And type
                Dim oneString As StringBuilder = New StringBuilder(1024)
                For i As Integer = 0 To barCount - 1
                    szOrigText.AppendFormat("Bar Code {0}:\r\n", i + 1)

                    ' Get the bar code text associated with bar code i + 1 
                    DTWAINAPI.DTWAIN_ArrayGetAtANSIString(aText, i, oneString)
                    szOrigText.AppendFormat("     Text: {0}\r\n", oneString.ToString())
                    Dim nType As Integer = 0
                    DTWAINAPI.DTWAIN_ArrayGetAtLong(aType, i, nType)
                    Dim szType As StringBuilder = New StringBuilder(100)

                    ' Translate the bar code type to a string defined by the TWAIN specification
                    DTWAINAPI.DTWAIN_GetTwainNameFromConstantEx(DTWAINAPI.DTWAIN_CONSTANT_TWBT, nType, szType, 100)
                    szOrigText.AppendFormat("     Type: {0}\r\n\r\n", szType.ToString())
                Next i

                ' Set the edit control to the text 
                txtBarCodes.Text = szOrigText.ToString().Replace("\r\n", vbCrLf)
            End If

            DTWAINAPI.DTWAIN_ArrayDestroy(aText)
            DTWAINAPI.DTWAIN_ArrayDestroy(aType)
        End If
    End Sub
End Class
