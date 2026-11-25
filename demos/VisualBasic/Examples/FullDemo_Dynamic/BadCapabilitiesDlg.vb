Imports System
Imports System.Text
Imports System.Windows.Forms
Imports Dynarithmic

' The project Root Namespace supplies TWAINDemo when configured that way.
Partial Public Class BadCapabilitiesDlg

    ' Used by the Windows Forms Designer.
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(theSource As IntPtr)
        Me.New()
        LoadBadCapabilities(theSource)
    End Sub

    Private Sub LoadBadCapabilities(theSource As IntPtr)
        Dim sTextData As New StringBuilder(256)
        VB_FullDemo.DTWAINAPI.DTWAIN_GetSourceProductName(theSource, sTextData, 256)
        Me.Text = sTextData.ToString()

        Dim capArray As IntPtr = IntPtr.Zero
        VB_FullDemo.DTWAINAPI.DTWAIN_EnumSupportedCaps(theSource, capArray)
        Dim nCapCount As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetCount(capArray)
        Dim nCapValue As Integer = 0
        Dim bOneBadCap As Boolean = False

        For nIndex As Integer = 0 To nCapCount - 1
            VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetAtLong(capArray, nIndex, nCapValue)
            VB_FullDemo.DTWAINAPI.DTWAIN_GetNameFromCap(nCapValue, sTextData, 255)
            Dim bestContainer As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetCapContainer(theSource, nCapValue, TwainConstants.MSG_GET)
            If bestContainer = 0 Then
                listBadCaps.Items.Add(sTextData.ToString())
                bOneBadCap = True
            End If
        Next

        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayDestroy(capArray)
        If Not bOneBadCap Then
            listBadCaps.Items.Add("No Bad Capabilities detected")
        End If
    End Sub
End Class
