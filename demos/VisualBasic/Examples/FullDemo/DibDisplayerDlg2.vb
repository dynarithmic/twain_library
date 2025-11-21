Imports System.Drawing
Imports System.Runtime.InteropServices

Public Class DibDisplayerDlg2
    Private theDib As System.IntPtr
    Private curBMP As Bitmap
    Declare Auto Function DeleteObject Lib "gdi32.dll" (hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean


    Public Sub New(ByVal item As System.IntPtr)
        InitializeComponent() ' This call is required by the Windows Form Designer.
        theDib = item
    End Sub

    Private Sub DisplayTheDib()
        Me.dibBox2.Image = Bitmap.FromHbitmap(DTWAINAPI.DTWAIN_ConvertDIBToBitmap(theDib, System.IntPtr.Zero), System.IntPtr.Zero)
        curBMP = Me.dibBox2.Image
    End Sub

    Private Sub DibDisplayerDlg2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DisplayTheDib()
    End Sub

    Private Sub DibDisplayerDlg2_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        DeleteObject(curBMP.GetHbitmap())
    End Sub

End Class

