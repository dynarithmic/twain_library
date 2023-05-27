Imports System.Windows.Forms
Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Collections.Generic

Public Class DibDisplayerDlg
    Declare Auto Function GlobalLock Lib "kernel32.DLL" (ByVal handle As Integer) As Integer
    Declare Auto Function GlobalUnlock Lib "kernel32.dll" (ByVal handle As IntPtr) As Integer
    Declare Unicode Function GdipCreateBitmapFromGdiDib Lib "GdiPlus.dll" (ByVal pBIH As IntPtr, ByVal pPix As IntPtr, ByRef pBitmap As IntPtr) As Integer
    Declare Auto Function DeleteObject Lib "gdi32.dll" (hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    Private AcquireArray As System.IntPtr
    Private nCurrentAcquisition As Integer
    Private nCurDib As Integer
    Private Structure DibInfo
        Public acquisition As Integer
        Public pageNum As Integer
    End Structure

    Private DibDictionary As Dictionary(Of DibInfo, Bitmap) = New Dictionary(Of DibInfo, Bitmap)

    Public Sub New(ByVal item As System.IntPtr)
        InitializeComponent() ' This call is required by the Windows Form Designer.
        AcquireArray = item
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DisplayTheDib()
        Dim keyCurrent As New DibInfo()
        Dim dib As System.IntPtr
        keyCurrent.pageNum = nCurDib
        keyCurrent.acquisition = nCurrentAcquisition
        If DibDictionary.ContainsKey(keyCurrent) Then
            Me.dibBox.Image = DibDictionary.Item(keyCurrent)
        Else
            dib = DTWAINAPI.DTWAIN_GetAcquiredImage(AcquireArray, nCurrentAcquisition, nCurDib)
            If dib <> 0 Then
                Me.dibBox.Image = BitmapFromDIB(dib)
                DibDictionary.Add(keyCurrent, Me.dibBox.Image)
            End If
        End If
        EnablePageButtons()
    End Sub

    Private Shared Function BitmapFromDIB(ByVal pDIB As IntPtr) As Bitmap
        Return Bitmap.FromHbitmap(DTWAINAPI.DTWAIN_ConvertDIBToBitmap(pDIB, System.IntPtr.Zero), System.IntPtr.Zero)
    End Function

    Private Sub EnablePageButtons()
        Dim nCount As Integer = DTWAINAPI.DTWAIN_GetNumAcquiredImages(AcquireArray, nCurrentAcquisition)
        Me.buttonNext.Enabled = (nCurDib < nCount - 1)
        Me.buttonPrev.Enabled = (nCurDib > 0)

        If nCount = 0 Then
        Else
            Dim sDib As Integer = nCurDib + 1
            Me.edPageCurrent.Text = sDib.ToString()
            Me.edPageTotal.Text = nCount.ToString()
        End If
    End Sub
    Private Sub DibDisplayerDlg_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        For Each pair As KeyValuePair(Of DibInfo, Bitmap) In DibDictionary
            DeleteObject(pair.Value.GetHbitmap())
        Next
        DTWAINAPI.DTWAIN_DestroyAcquisitionArray(AcquireArray, 1)
    End Sub

    Private Sub DibDisplayerDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        nCurrentAcquisition = 0
        nCurDib = 0
        Dim nCount As Integer = DTWAINAPI.DTWAIN_GetNumAcquisitions(AcquireArray)
        For i As Integer = 1 To nCount
            Me.cmbAcquisition.Items.Add(i.ToString())
            Me.cmbAcquisition.SelectedIndex = 0
        Next i
        DisplayTheDib()
    End Sub

    Private Sub buttonNext_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNext.Click
        nCurDib += 1
        DisplayTheDib()
    End Sub

    Private Sub buttonPrev_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonPrev.Click
        nCurDib -= 1
        DisplayTheDib()
    End Sub

    Private Sub cmbAcquisition_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcquisition.SelectedIndexChanged
        If Me.cmbAcquisition.SelectedIndex <> nCurrentAcquisition Then
            nCurrentAcquisition = Me.cmbAcquisition.SelectedIndex
            nCurDib = 0
            DisplayTheDib()
        End If
    End Sub
End Class
