﻿Imports System.Windows.Forms

Public Class FileTypeDlg

    Private Class AllTypes
        Public fType As String
        Public DTWAINType As Integer
        Public defFileName As String
        Public Sub New(ByVal s1 As String, ByVal dType As Integer, ByVal s2 As String)
            fType = s1
            DTWAINType = dType
            defFileName = s2
        End Sub
    End Class

    Private selectedFileType As Integer
    Private selectedFileName As String


    Public Sub New()
        selectedFileType = DTWAINAPI.DTWAIN_BMP
        selectedFileName = "test.bmp"
        InitializeComponent()
    End Sub

    Public Function GetFileType() As Integer
        Return selectedFileType
    End Function

    Public Function GetFileName() As String
        Return selectedFileName
    End Function

    Private g_allTypes As AllTypes() = {New AllTypes("BMP File", DTWAINAPI.DTWAIN_BMP, "test.bmp"), New AllTypes("Multi-page DCX File", DTWAINAPI.DTWAIN_DCX, "test.dcx"), New AllTypes("Enhanced Meta File (EMF)", DTWAINAPI.DTWAIN_EMF, "test.emf"), New AllTypes("GIF File", DTWAINAPI.DTWAIN_GIF, "test.gif"), New AllTypes("JPEG File", DTWAINAPI.DTWAIN_JPEG, "test.jpg"), New AllTypes("JPEG-2000 File", DTWAINAPI.DTWAIN_JPEG2000, "test.jp2"), _
     New AllTypes("Adobe PDF File", DTWAINAPI.DTWAIN_PDFMULTI, "test.pdf"), New AllTypes("Postscript Level 1 File", DTWAINAPI.DTWAIN_POSTSCRIPT1MULTI, "test.ps"), New AllTypes("Postscript Level 2 File", DTWAINAPI.DTWAIN_POSTSCRIPT2MULTI, "test.ps"), New AllTypes("PNG File", DTWAINAPI.DTWAIN_PNG, "test.png"), New AllTypes("Adobe Paintshop (PSD) File", DTWAINAPI.DTWAIN_PSD, "test.psd"), New AllTypes("Text File", DTWAINAPI.DTWAIN_TEXTMULTI, "test.txt"), _
     New AllTypes("TIFF (No compression)", DTWAINAPI.DTWAIN_TIFFNONEMULTI, "test.tif"), New AllTypes("TIFF (CCITT Group 3)", DTWAINAPI.DTWAIN_TIFFG3MULTI, "test.tif"), New AllTypes("TIFF (CCITT Group 4)", DTWAINAPI.DTWAIN_TIFFG4MULTI, "test.tif"), New AllTypes("TIFF (JPEG compression)", DTWAINAPI.DTWAIN_TIFFJPEGMULTI, "test.tif"), New AllTypes("TIFF (Packbits)", DTWAINAPI.DTWAIN_TIFFPACKBITSMULTI, "test.tif"), New AllTypes("TIFF (Flate compression)", DTWAINAPI.DTWAIN_TIFFDEFLATEMULTI, "test.tif"), _
     New AllTypes("TIFF (LZW compression)", DTWAINAPI.DTWAIN_TIFFLZWMULTI, "test.tif"), New AllTypes("Targa (TGA) File", DTWAINAPI.DTWAIN_TGA, "test.tga"), New AllTypes("Windows Meta File (WMF)", DTWAINAPI.DTWAIN_WMF, "test.wmf"), New AllTypes("Windows ICON File (ICO)", DTWAINAPI.DTWAIN_ICO, "test.ico"), New AllTypes("Windows ICON File- Vista compatible (ICO)", DTWAINAPI.DTWAIN_ICO_VISTA, "test.ico"), New AllTypes("Wireless Bitmap File (WBMP)", DTWAINAPI.DTWAIN_WBMP, "test.wbmp")}

    Private Sub cmbFileType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFileType.SelectedIndexChanged
        Dim nCurSel As Integer = cmbFileType.SelectedIndex
        edFileName.Text = g_allTypes(nCurSel).defFileName
    End Sub

    Private Sub FileTypeDlg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim nTypes As Integer = g_allTypes.Length
        For i As Integer = 0 To nTypes - 1
            cmbFileType.Items.Add(g_allTypes(i).fType)
        Next
        cmbFileType.SelectedIndex = 0
        edFileName.Text = g_allTypes(0).defFileName
    End Sub

    Private Sub OKbutton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKbutton.Click
        Dim nCurSel As Integer = cmbFileType.SelectedIndex
        selectedFileType = g_allTypes(nCurSel).DTWAINType
        selectedFileName = edFileName.Text
    End Sub
End Class
