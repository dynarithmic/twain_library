Imports System.Windows.Forms
Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.InteropServices

Public Class DibDisplayerDlg2
    Private theDib As Integer
    Declare Auto Function GlobalLock Lib "kernel32.DLL" (ByVal handle As Integer) As Integer
    Declare Auto Function GlobalUnlock Lib "kernel32.dll" (ByVal handle As IntPtr) As Integer
    Declare Unicode Function GdipCreateBitmapFromGdiDib Lib "GdiPlus.dll" (ByVal pBIH As IntPtr, ByVal pPix As IntPtr, ByRef pBitmap As IntPtr) As Integer

    Public Sub New(ByVal item As Integer)
        InitializeComponent() ' This call is required by the Windows Form Designer.
        theDib = item
    End Sub

    Private Sub DisplayTheDib()
        Dim dibPtr As IntPtr = GlobalLock(theDib)
        Me.dibBox2.Image = BitmapFromDIB(dibPtr)
        GlobalUnlock(dibPtr)
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

    Private Sub DibDisplayerDlg2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DisplayTheDib()
    End Sub
End Class
