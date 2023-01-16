Imports System.Windows.Forms
Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.InteropServices

Public Structure BITMAPINFOHEADER
    Public biSize As Int32
    Public biWidth As Int32
    Public biHeight As Int32
    Public biPlanes As Int16
    Public biBitCount As Int16
    Public biCompression As Int32
    Public biSizeImage As Int32
    Public biXPelsperMeter As Int32
    Public biYPelsPerMeter As Int32
    Public biClrUsed As Int32
    Public biClrImportant As Int32
End Structure

Public Class DibDisplayerDlg
    Declare Auto Function GlobalLock Lib "kernel32.DLL" (ByVal handle As Integer) As Integer
    Declare Auto Function GlobalUnlock Lib "kernel32.dll" (ByVal handle As IntPtr) As Integer
    Declare Unicode Function GdipCreateBitmapFromGdiDib Lib "GdiPlus.dll" (ByVal pBIH As IntPtr, ByVal pPix As IntPtr, ByRef pBitmap As IntPtr) As Integer

    Private AcquireArray As Integer
    Private nCurrentAcquisition As Integer
    Private nCurDib As Integer

    Public Sub New(ByVal item As Integer)
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
        Dim dib As Integer = DTWAINAPI.DTWAIN_GetAcquiredImage(AcquireArray, nCurrentAcquisition, nCurDib)
        If dib <> 0 Then
            Dim dibPtr As IntPtr = GlobalLock(dib)
            Me.dibBox.Image = BitmapFromDIB(dibPtr)
            GlobalUnlock(dibPtr)
        End If
        EnablePageButtons()
    End Sub

    Private Shared Function BitmapFromDIB(ByVal pDIB As IntPtr) As Bitmap
        'get pointer to bitmap header info       
        Dim pPix As IntPtr = GetPixelInfo(pDIB)

        'Call external GDI method
        Dim mi As MethodInfo = GetType(Bitmap).GetMethod("FromGDIplus", BindingFlags.[Static] Or BindingFlags.NonPublic)
        If mi Is Nothing Then
            Return Nothing
        End If

        ' Initialize memory pointer where Bitmap will be saved
        Dim pBmp As IntPtr = IntPtr.Zero

        'Call external method that saves bitmap into pointer
        Dim status As Integer = GdipCreateBitmapFromGdiDib(pDIB, pPix, pBmp)

        'If success return bitmap, if failed return null
        If (status = 0) AndAlso (pBmp <> IntPtr.Zero) Then
            Return DirectCast(mi.Invoke(Nothing, New Object() {pBmp}), Bitmap)
        Else
            Return Nothing
        End If
    End Function
            
    Private Shared Function GetPixelInfo(ByVal bmpPtr As IntPtr) As IntPtr
        Dim bmi As BITMAPINFOHEADER = DirectCast(Marshal.PtrToStructure(bmpPtr, GetType(BITMAPINFOHEADER)), BITMAPINFOHEADER)

        If bmi.biSizeImage = 0 Then
            bmi.biSizeImage = CUInt(((((bmi.biWidth * bmi.biBitCount) + 31) And Not 31) >> 3) * bmi.biHeight)
        End If

        Dim p As Integer = CInt(bmi.biClrUsed)
        If (p = 0) AndAlso (bmi.biBitCount <= 8) Then
            p = 1 << bmi.biBitCount
        End If
        p = (p * 4) + CInt(bmi.biSize) + CInt(bmpPtr)
        Return New IntPtr(p)
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
