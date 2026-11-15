Option Infer On
Imports System.Drawing
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Diagnostics
Imports Dynarithmic ' Root namespace is set to blank for this project


Public Class VB_FullDemo
    Inherits Form

#Region " Windows Form Designer generated code "

    Private Const PDF_NoEncryption_Value As Integer = 0
    Private Const PDF_40BitRC4_Encryption_Value As Integer = 1
    Private Const PDF_128BitRC4_Encryption_Value As Integer = 2
    Private Const PDF_128BitAES_Encryption_Value As Integer = 3
    Private Const PDF_256BitAES_Encryption_Value As Integer = 4

    Private Dim fileDriverMap As Dictionary(Of Integer, MenuItem) = New Dictionary(Of Integer, MenuItem)

    Friend WithEvents MenuItem6 As MenuItem
    Friend WithEvents PDF_NoEncryption As MenuItem
    Friend WithEvents PDF_ASCII85 As MenuItem
    Friend WithEvents PDF_40BitRC4_Encryption As MenuItem
    Friend WithEvents PDF_128BitRC4_Encryption As MenuItem
    Friend WithEvents PDF_128BitAES_Encryption As MenuItem
    Friend WithEvents PDF_256BitAES_Encryption As MenuItem
    Private Const PDF_ASCII85_Value As Integer = 5
    Friend WithEvents PNG As MenuItem
    Friend WithEvents PostScript_Level1 As MenuItem
    Friend WithEvents PostScript_Level2 As MenuItem
    Friend WithEvents PostScript_Level3 As MenuItem
    Friend WithEvents SVG As MenuItem
    Friend WithEvents SVGZ As MenuItem
    Friend WithEvents TGA As MenuItem
    Friend WithEvents TGA_RLE As MenuItem
    Friend WithEvents MenuItem8 As MenuItem
    Friend WithEvents Tiff_NoCompression As MenuItem
    Friend WithEvents Tiff_Group3 As MenuItem
    Friend WithEvents Tiff_Group4 As MenuItem
    Friend WithEvents Tiff_Flate As MenuItem
    Friend WithEvents Tiff_JPEG As MenuItem
    Friend WithEvents Tiff_LZW As MenuItem
    Friend WithEvents Tiff_Packbits As MenuItem
    Friend WithEvents WEBP As MenuItem
    Friend WithEvents WMF As MenuItem
    Friend WithEvents WBMP As MenuItem
    Friend WithEvents FF_BMP As MenuItem
    Friend WithEvents FF_JFIF As MenuItem
    Friend WithEvents FF_TIFF As MenuItem
    Friend WithEvents FF_TIFFMULTI As MenuItem
    Friend WithEvents FF_PNG As MenuItem
    Friend WithEvents FF_PDF As MenuItem
    Friend WithEvents FF_PDFA As MenuItem
    Friend WithEvents FF_PDFA2 As MenuItem
    Friend WithEvents FF_PDFRASTER As MenuItem
    Friend WithEvents FF_FPX As MenuItem
    Friend WithEvents FF_EXIF As MenuItem
    Friend WithEvents FF_SPIFF As MenuItem
    Friend WithEvents FF_XBM As MenuItem
    Friend WithEvents FF_PICT As MenuItem
    Friend WithEvents FF_JP2 As MenuItem
    Friend WithEvents FF_JPX As MenuItem
    Friend WithEvents FF_DEJAVU As MenuItem
    Friend WithEvents UseGetMessage As MenuItem
    Friend WithEvents menuCantonese As MenuItem
    Friend WithEvents menuTurkish As MenuItem
    Friend WithEvents TEXTOCR As MenuItem
    Friend WithEvents MenuItem12 As MenuItem
    Public Shared stamp_pdf As Boolean = True
    
    Public Sub New()
        MyBase.New()
        thisObject = Me
        InitializeComponent()
        InitializeFileDriverMap()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If dllExists = True Then
                DTWAINAPI.DTWAIN_SysDestroy()
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End If
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents ExitDemo As System.Windows.Forms.MenuItem
    Friend WithEvents SelectSource As System.Windows.Forms.MenuItem
    Friend WithEvents SelectSourceByName As System.Windows.Forms.MenuItem
    Friend WithEvents SelectDefaultSource As System.Windows.Forms.MenuItem
    Friend WithEvents SelectSourceCustom As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents SourceProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents CloseSource As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents AcquireNative As System.Windows.Forms.MenuItem
    Friend WithEvents AcquireBuffered As System.Windows.Forms.MenuItem
    Friend WithEvents AcquireFile As System.Windows.Forms.MenuItem
    Friend WithEvents AcquireFileUsingDriver As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents ShowPreview As System.Windows.Forms.MenuItem
    Friend WithEvents UseSourceUI As System.Windows.Forms.MenuItem
    Friend WithEvents DiscardBlankPages As System.Windows.Forms.MenuItem
    Friend WithEvents LoggingOptions As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents DTWAINVersion As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As MenuItem
    Friend WithEvents menuDutch As MenuItem
    Friend WithEvents menuEnglish As MenuItem
    Friend WithEvents menuFrench As MenuItem
    Friend WithEvents menuGerman As MenuItem
    Friend WithEvents menuItalian As MenuItem
    Friend WithEvents menuPortuguese As MenuItem
    Friend WithEvents menuRomanian As MenuItem
    Friend WithEvents menuRussian As MenuItem
    Friend WithEvents menuSimplifiedChinese As MenuItem
    Friend WithEvents menuSpanish As MenuItem
    Friend WithEvents menuCustomLanguage As MenuItem
    Friend WithEvents menuTraditionalChinese As MenuItem
    Friend WithEvents menuGreek As MenuItem
    Friend WithEvents menuJapanese As MenuItem
    Friend WithEvents menuKorean As MenuItem
    Friend WithEvents ShowBarcodeInfo As MenuItem
    Friend WithEvents MenuItem4 As MenuItem
    Friend WithEvents BigTiff_NoCompression As MenuItem
    Friend WithEvents BigTiff_Group3 As MenuItem
    Friend WithEvents BigTiff_Group4 As MenuItem
    Friend WithEvents BigTIFF_Flate As MenuItem
    Friend WithEvents BigTIFF_JPEG As MenuItem
    Friend WithEvents BigTIFF_LZW As MenuItem
    Friend WithEvents BigTIFF_Packbits As MenuItem
    Friend WithEvents BMP As MenuItem
    Friend WithEvents BMP_RLE As MenuItem
    Friend WithEvents DCX As MenuItem
    Friend WithEvents EMF As MenuItem
    Friend WithEvents GIF As MenuItem
    Friend WithEvents ICO As MenuItem
    Friend WithEvents ICOVISTA As MenuItem
    Friend WithEvents JPEG As MenuItem
    Friend WithEvents JPEG2000 As MenuItem
    Friend WithEvents JPEGXL As MenuItem
    Friend WithEvents JPEGXR As MenuItem
    Friend WithEvents PSD As MenuItem
    Friend WithEvents PCX As MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.SelectSource = New System.Windows.Forms.MenuItem()
        Me.SelectSourceByName = New System.Windows.Forms.MenuItem()
        Me.SelectDefaultSource = New System.Windows.Forms.MenuItem()
        Me.SelectSourceCustom = New System.Windows.Forms.MenuItem()
        Me.MenuItem5 = New System.Windows.Forms.MenuItem()
        Me.SourceProperties = New System.Windows.Forms.MenuItem()
        Me.MenuItem7 = New System.Windows.Forms.MenuItem()
        Me.CloseSource = New System.Windows.Forms.MenuItem()
        Me.ExitDemo = New System.Windows.Forms.MenuItem()
        Me.MenuItem9 = New System.Windows.Forms.MenuItem()
        Me.AcquireNative = New System.Windows.Forms.MenuItem()
        Me.AcquireBuffered = New System.Windows.Forms.MenuItem()
        Me.AcquireFile = New System.Windows.Forms.MenuItem()
        Me.MenuItem4 = New System.Windows.Forms.MenuItem()
        Me.BigTiff_NoCompression = New System.Windows.Forms.MenuItem()
        Me.BigTiff_Group3 = New System.Windows.Forms.MenuItem()
        Me.BigTiff_Group4 = New System.Windows.Forms.MenuItem()
        Me.BigTIFF_Flate = New System.Windows.Forms.MenuItem()
        Me.BigTIFF_JPEG = New System.Windows.Forms.MenuItem()
        Me.BigTIFF_LZW = New System.Windows.Forms.MenuItem()
        Me.BigTIFF_Packbits = New System.Windows.Forms.MenuItem()
        Me.BMP = New System.Windows.Forms.MenuItem()
        Me.BMP_RLE = New System.Windows.Forms.MenuItem()
        Me.DCX = New System.Windows.Forms.MenuItem()
        Me.EMF = New System.Windows.Forms.MenuItem()
        Me.GIF = New System.Windows.Forms.MenuItem()
        Me.ICO = New System.Windows.Forms.MenuItem()
        Me.ICOVISTA = New System.Windows.Forms.MenuItem()
        Me.JPEG = New System.Windows.Forms.MenuItem()
        Me.JPEG2000 = New System.Windows.Forms.MenuItem()
        Me.JPEGXL = New System.Windows.Forms.MenuItem()
        Me.JPEGXR = New System.Windows.Forms.MenuItem()
        Me.PSD = New System.Windows.Forms.MenuItem()
        Me.PCX = New System.Windows.Forms.MenuItem()
        Me.MenuItem6 = New System.Windows.Forms.MenuItem()
        Me.PDF_NoEncryption = New System.Windows.Forms.MenuItem()
        Me.PDF_ASCII85 = New System.Windows.Forms.MenuItem()
        Me.PDF_40BitRC4_Encryption = New System.Windows.Forms.MenuItem()
        Me.PDF_128BitRC4_Encryption = New System.Windows.Forms.MenuItem()
        Me.PDF_128BitAES_Encryption = New System.Windows.Forms.MenuItem()
        Me.PDF_256BitAES_Encryption = New System.Windows.Forms.MenuItem()
        Me.PNG = New System.Windows.Forms.MenuItem()
        Me.PostScript_Level1 = New System.Windows.Forms.MenuItem()
        Me.PostScript_Level2 = New System.Windows.Forms.MenuItem()
        Me.PostScript_Level3 = New System.Windows.Forms.MenuItem()
        Me.SVG = New System.Windows.Forms.MenuItem()
        Me.SVGZ = New System.Windows.Forms.MenuItem()
        Me.TGA = New System.Windows.Forms.MenuItem()
        Me.TGA_RLE = New System.Windows.Forms.MenuItem()
        Me.TEXTOCR = New System.Windows.Forms.MenuItem()
        Me.MenuItem8 = New System.Windows.Forms.MenuItem()
        Me.Tiff_NoCompression = New System.Windows.Forms.MenuItem()
        Me.Tiff_Group3 = New System.Windows.Forms.MenuItem()
        Me.Tiff_Group4 = New System.Windows.Forms.MenuItem()
        Me.Tiff_Flate = New System.Windows.Forms.MenuItem()
        Me.Tiff_JPEG = New System.Windows.Forms.MenuItem()
        Me.Tiff_LZW = New System.Windows.Forms.MenuItem()
        Me.Tiff_Packbits = New System.Windows.Forms.MenuItem()
        Me.WEBP = New System.Windows.Forms.MenuItem()
        Me.WMF = New System.Windows.Forms.MenuItem()
        Me.WBMP = New System.Windows.Forms.MenuItem()
        Me.AcquireFileUsingDriver = New System.Windows.Forms.MenuItem()
        Me.FF_BMP = New System.Windows.Forms.MenuItem()
        Me.FF_JFIF = New System.Windows.Forms.MenuItem()
        Me.FF_TIFF = New System.Windows.Forms.MenuItem()
        Me.FF_TIFFMULTI = New System.Windows.Forms.MenuItem()
        Me.FF_PNG = New System.Windows.Forms.MenuItem()
        Me.FF_PDF = New System.Windows.Forms.MenuItem()
        Me.FF_PDFA = New System.Windows.Forms.MenuItem()
        Me.FF_PDFA2 = New System.Windows.Forms.MenuItem()
        Me.FF_PDFRASTER = New System.Windows.Forms.MenuItem()
        Me.FF_FPX = New System.Windows.Forms.MenuItem()
        Me.FF_EXIF = New System.Windows.Forms.MenuItem()
        Me.FF_SPIFF = New System.Windows.Forms.MenuItem()
        Me.FF_XBM = New System.Windows.Forms.MenuItem()
        Me.FF_PICT = New System.Windows.Forms.MenuItem()
        Me.FF_JP2 = New System.Windows.Forms.MenuItem()
        Me.FF_JPX = New System.Windows.Forms.MenuItem()
        Me.FF_DEJAVU = New System.Windows.Forms.MenuItem()
        Me.MenuItem14 = New System.Windows.Forms.MenuItem()
        Me.ShowPreview = New System.Windows.Forms.MenuItem()
        Me.UseSourceUI = New System.Windows.Forms.MenuItem()
        Me.DiscardBlankPages = New System.Windows.Forms.MenuItem()
        Me.ShowBarcodeInfo = New System.Windows.Forms.MenuItem()
        Me.UseGetMessage = New System.Windows.Forms.MenuItem()
        Me.MenuItem10 = New System.Windows.Forms.MenuItem()
        Me.LoggingOptions = New System.Windows.Forms.MenuItem()
        Me.MenuItem3 = New System.Windows.Forms.MenuItem()
        Me.menuCantonese = New System.Windows.Forms.MenuItem()
        Me.menuDutch = New System.Windows.Forms.MenuItem()
        Me.menuEnglish = New System.Windows.Forms.MenuItem()
        Me.menuFrench = New System.Windows.Forms.MenuItem()
        Me.menuGerman = New System.Windows.Forms.MenuItem()
        Me.menuGreek = New System.Windows.Forms.MenuItem()
        Me.menuItalian = New System.Windows.Forms.MenuItem()
        Me.menuJapanese = New System.Windows.Forms.MenuItem()
        Me.menuKorean = New System.Windows.Forms.MenuItem()
        Me.menuPortuguese = New System.Windows.Forms.MenuItem()
        Me.menuRomanian = New System.Windows.Forms.MenuItem()
        Me.menuRussian = New System.Windows.Forms.MenuItem()
        Me.menuSimplifiedChinese = New System.Windows.Forms.MenuItem()
        Me.menuSpanish = New System.Windows.Forms.MenuItem()
        Me.menuTurkish = New System.Windows.Forms.MenuItem()
        Me.menuTraditionalChinese = New System.Windows.Forms.MenuItem()
        Me.menuCustomLanguage = New System.Windows.Forms.MenuItem()
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.DTWAINVersion = New System.Windows.Forms.MenuItem()
        Me.MenuItem12 = New System.Windows.Forms.MenuItem()
        Me.SuspendLayout
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem9, Me.MenuItem10, Me.MenuItem3, Me.MenuItem2})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.SelectSource, Me.SelectSourceByName, Me.SelectDefaultSource, Me.SelectSourceCustom, Me.MenuItem5, Me.SourceProperties, Me.MenuItem7, Me.CloseSource, Me.ExitDemo})
        Me.MenuItem1.Text = "&Source Selection Test"
        '
        'SelectSource
        '
        Me.SelectSource.Index = 0
        Me.SelectSource.Text = "Select Source..."
        '
        'SelectSourceByName
        '
        Me.SelectSourceByName.Index = 1
        Me.SelectSourceByName.Text = "Select Source By Name..."
        '
        'SelectDefaultSource
        '
        Me.SelectDefaultSource.Index = 2
        Me.SelectDefaultSource.Text = "Select Default Source..."
        '
        'SelectSourceCustom
        '
        Me.SelectSourceCustom.Index = 3
        Me.SelectSourceCustom.Text = "Select Source Custom..."
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 4
        Me.MenuItem5.Text = "-"
        '
        'SourceProperties
        '
        Me.SourceProperties.Index = 5
        Me.SourceProperties.Text = "Source Properties..."
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 6
        Me.MenuItem7.Text = "-"
        '
        'CloseSource
        '
        Me.CloseSource.Index = 7
        Me.CloseSource.Text = "Close Source..."
        '
        'ExitDemo
        '
        Me.ExitDemo.Index = 8
        Me.ExitDemo.Text = "Exit Demo"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 1
        Me.MenuItem9.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.AcquireNative, Me.AcquireBuffered, Me.AcquireFile, Me.AcquireFileUsingDriver, Me.MenuItem14, Me.ShowPreview, Me.UseSourceUI, Me.DiscardBlankPages, Me.ShowBarcodeInfo, Me.MenuItem12, Me.UseGetMessage})
        Me.MenuItem9.Text = "&Acquire Test"
        '
        'AcquireNative
        '
        Me.AcquireNative.Index = 0
        Me.AcquireNative.Text = "Acquire Native..."
        '
        'AcquireBuffered
        '
        Me.AcquireBuffered.Index = 1
        Me.AcquireBuffered.Text = "Acquire Buffered..."
        '
        'AcquireFile
        '
        Me.AcquireFile.Index = 2
        Me.AcquireFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem4, Me.BMP, Me.BMP_RLE, Me.DCX, Me.EMF, Me.GIF, Me.ICO, Me.ICOVISTA, Me.JPEG, Me.JPEG2000, Me.JPEGXL, Me.JPEGXR, Me.PSD, Me.PCX, Me.MenuItem6, Me.PNG, Me.PostScript_Level1, Me.PostScript_Level2, Me.PostScript_Level3, Me.SVG, Me.SVGZ, Me.TGA, Me.TGA_RLE, Me.TEXTOCR, Me.MenuItem8, Me.WEBP, Me.WMF, Me.WBMP})
        Me.AcquireFile.Text = "Acquire File..."
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 0
        Me.MenuItem4.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.BigTiff_NoCompression, Me.BigTiff_Group3, Me.BigTiff_Group4, Me.BigTIFF_Flate, Me.BigTIFF_JPEG, Me.BigTIFF_LZW, Me.BigTIFF_Packbits})
        Me.MenuItem4.Text = "BigTIFF"
        '
        'BigTiff_NoCompression
        '
        Me.BigTiff_NoCompression.Index = 0
        Me.BigTiff_NoCompression.Text = "No compression"
        '
        'BigTiff_Group3
        '
        Me.BigTiff_Group3.Index = 1
        Me.BigTiff_Group3.Text = "Group 3"
        '
        'BigTiff_Group4
        '
        Me.BigTiff_Group4.Index = 2
        Me.BigTiff_Group4.Text = "Group 4"
        '
        'BigTIFF_Flate
        '
        Me.BigTIFF_Flate.Index = 3
        Me.BigTIFF_Flate.Text = "Flate"
        '
        'BigTIFF_JPEG
        '
        Me.BigTIFF_JPEG.Index = 4
        Me.BigTIFF_JPEG.Text = "JPEG"
        '
        'BigTIFF_LZW
        '
        Me.BigTIFF_LZW.Index = 5
        Me.BigTIFF_LZW.Text = "LZW"
        '
        'BigTIFF_Packbits
        '
        Me.BigTIFF_Packbits.Index = 6
        Me.BigTIFF_Packbits.Text = "Packbits"
        '
        'BMP
        '
        Me.BMP.Index = 1
        Me.BMP.Text = "BMP"
        '
        'BMP_RLE
        '
        Me.BMP_RLE.Index = 2
        Me.BMP_RLE.Text = "BMP RLE"
        '
        'DCX
        '
        Me.DCX.Index = 3
        Me.DCX.Text = "DCX"
        '
        'EMF
        '
        Me.EMF.Index = 4
        Me.EMF.Text = "Enhanced Meta File (EMF)"
        '
        'GIF
        '
        Me.GIF.Index = 5
        Me.GIF.Text = "GIF"
        '
        'ICO
        '
        Me.ICO.Index = 6
        Me.ICO.Text = "ICO"
        '
        'ICOVISTA
        '
        Me.ICOVISTA.Index = 7
        Me.ICOVISTA.Text = "ICO-Vista"
        '
        'JPEG
        '
        Me.JPEG.Index = 8
        Me.JPEG.Text = "JPEG"
        '
        'JPEG2000
        '
        Me.JPEG2000.Index = 9
        Me.JPEG2000.Text = "JPEG-2000"
        '
        'JPEGXL
        '
        Me.JPEGXL.Index = 10
        Me.JPEGXL.Text = "JPEG-XL"
        '
        'JPEGXR
        '
        Me.JPEGXR.Index = 11
        Me.JPEGXR.Text = "JPEG-XR"
        '
        'PSD
        '
        Me.PSD.Index = 12
        Me.PSD.Text = "Paintshop (PSD)"
        '
        'PCX
        '
        Me.PCX.Index = 13
        Me.PCX.Text = "PCX"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 14
        Me.MenuItem6.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.PDF_NoEncryption, Me.PDF_ASCII85, Me.PDF_40BitRC4_Encryption, Me.PDF_128BitRC4_Encryption, Me.PDF_128BitAES_Encryption, Me.PDF_256BitAES_Encryption})
        Me.MenuItem6.Text = "PDF"
        '
        'PDF_NoEncryption
        '
        Me.PDF_NoEncryption.Index = 0
        Me.PDF_NoEncryption.Text = "No Encryption"
        '
        'PDF_ASCII85
        '
        Me.PDF_ASCII85.Index = 1
        Me.PDF_ASCII85.Text = "ASCII85 Encoding"
        '
        'PDF_40BitRC4_Encryption
        '
        Me.PDF_40BitRC4_Encryption.Index = 2
        Me.PDF_40BitRC4_Encryption.Text = "40-bit RC4 Encryption"
        '
        'PDF_128BitRC4_Encryption
        '
        Me.PDF_128BitRC4_Encryption.Index = 3
        Me.PDF_128BitRC4_Encryption.Text = "128-bit RC4 Encryption"
        '
        'PDF_128BitAES_Encryption
        '
        Me.PDF_128BitAES_Encryption.Index = 4
        Me.PDF_128BitAES_Encryption.Text = "128-bit AES Encryption"
        '
        'PDF_256BitAES_Encryption
        '
        Me.PDF_256BitAES_Encryption.Index = 5
        Me.PDF_256BitAES_Encryption.Text = "256-bit AES Encryption"
        '
        'PNG
        '
        Me.PNG.Index = 15
        Me.PNG.Text = "PNG"
        '
        'PostScript_Level1
        '
        Me.PostScript_Level1.Index = 16
        Me.PostScript_Level1.Text = "PostScript (Level 1)"
        '
        'PostScript_Level2
        '
        Me.PostScript_Level2.Index = 17
        Me.PostScript_Level2.Text = "PostScript (Level 2)"
        '
        'PostScript_Level3
        '
        Me.PostScript_Level3.Index = 18
        Me.PostScript_Level3.Text = "PostScript (Level 3)"
        '
        'SVG
        '
        Me.SVG.Index = 19
        Me.SVG.Text = "SVG"
        '
        'SVGZ
        '
        Me.SVGZ.Index = 20
        Me.SVGZ.Text = "SVGZ"
        '
        'TGA
        '
        Me.TGA.Index = 21
        Me.TGA.Text = "Targa (TGA)"
        '
        'TGA_RLE
        '
        Me.TGA_RLE.Index = 22
        Me.TGA_RLE.Text = "Targa RLE"
        '
        'TEXTOCR
        '
        Me.TEXTOCR.Index = 23
        Me.TEXTOCR.Text = "Text"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 24
        Me.MenuItem8.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.Tiff_NoCompression, Me.Tiff_Group3, Me.Tiff_Group4, Me.Tiff_Flate, Me.Tiff_JPEG, Me.Tiff_LZW, Me.Tiff_Packbits})
        Me.MenuItem8.Text = "TIFF"
        '
        'Tiff_NoCompression
        '
        Me.Tiff_NoCompression.Index = 0
        Me.Tiff_NoCompression.Text = "No compression"
        '
        'Tiff_Group3
        '
        Me.Tiff_Group3.Index = 1
        Me.Tiff_Group3.Text = "Group 3"
        '
        'Tiff_Group4
        '
        Me.Tiff_Group4.Index = 2
        Me.Tiff_Group4.Text = "Group 4"
        '
        'Tiff_Flate
        '
        Me.Tiff_Flate.Index = 3
        Me.Tiff_Flate.Text = "Flate"
        '
        'Tiff_JPEG
        '
        Me.Tiff_JPEG.Index = 4
        Me.Tiff_JPEG.Text = "JPEG"
        '
        'Tiff_LZW
        '
        Me.Tiff_LZW.Index = 5
        Me.Tiff_LZW.Text = "LZW"
        '
        'Tiff_Packbits
        '
        Me.Tiff_Packbits.Index = 6
        Me.Tiff_Packbits.Text = "Packbits"
        '
        'WEBP
        '
        Me.WEBP.Index = 25
        Me.WEBP.Text = "WebP"
        '
        'WMF
        '
        Me.WMF.Index = 26
        Me.WMF.Text = "Windows Meta File (WMF)"
        '
        'WBMP
        '
        Me.WBMP.Index = 27
        Me.WBMP.Text = "Wireless Bitmap (WBMP)"
        '
        'AcquireFileUsingDriver
        '
        Me.AcquireFileUsingDriver.Index = 3
        Me.AcquireFileUsingDriver.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.FF_BMP, Me.FF_JFIF, Me.FF_TIFF, Me.FF_TIFFMULTI, Me.FF_PNG, Me.FF_PDF, Me.FF_PDFA, Me.FF_PDFA2, Me.FF_PDFRASTER, Me.FF_FPX, Me.FF_EXIF, Me.FF_SPIFF, Me.FF_XBM, Me.FF_PICT, Me.FF_JP2, Me.FF_JPX, Me.FF_DEJAVU})
        Me.AcquireFileUsingDriver.Text = "Acquire File Using Driver..."
        '
        'FF_BMP
        '
        Me.FF_BMP.Index = 0
        Me.FF_BMP.Text = "Windows BMP"
        '
        'FF_JFIF
        '
        Me.FF_JFIF.Index = 1
        Me.FF_JFIF.Text = "JFIF (JPEG)"
        '
        'FF_TIFF
        '
        Me.FF_TIFF.Index = 2
        Me.FF_TIFF.Text = "TIFF"
        '
        'FF_TIFFMULTI
        '
        Me.FF_TIFFMULTI.Index = 3
        Me.FF_TIFFMULTI.Text = "TIFF (Multipage)"
        '
        'FF_PNG
        '
        Me.FF_PNG.Index = 4
        Me.FF_PNG.Text = "PNG"
        '
        'FF_PDF
        '
        Me.FF_PDF.Index = 5
        Me.FF_PDF.Text = "PDF"
        '
        'FF_PDFA
        '
        Me.FF_PDFA.Index = 6
        Me.FF_PDFA.Text = "PDF/A"
        '
        'FF_PDFA2
        '
        Me.FF_PDFA2.Index = 7
        Me.FF_PDFA2.Text = "PDF/A2"
        '
        'FF_PDFRASTER
        '
        Me.FF_PDFRASTER.Index = 8
        Me.FF_PDFRASTER.Text = "PDFRASTER"
        '
        'FF_FPX
        '
        Me.FF_FPX.Index = 9
        Me.FF_FPX.Text = "FlashPix (FPX)"
        '
        'FF_EXIF
        '
        Me.FF_EXIF.Index = 10
        Me.FF_EXIF.Text = "EXIF"
        '
        'FF_SPIFF
        '
        Me.FF_SPIFF.Index = 11
        Me.FF_SPIFF.Text = "SPIFF"
        '
        'FF_XBM
        '
        Me.FF_XBM.Index = 12
        Me.FF_XBM.Text = "XBM"
        '
        'FF_PICT
        '
        Me.FF_PICT.Index = 13
        Me.FF_PICT.Text = "Macintosh PICT"
        '
        'FF_JP2
        '
        Me.FF_JP2.Index = 14
        Me.FF_JP2.Text = "JP2"
        '
        'FF_JPX
        '
        Me.FF_JPX.Index = 15
        Me.FF_JPX.Text = "JPX"
        '
        'FF_DEJAVU
        '
        Me.FF_DEJAVU.Index = 16
        Me.FF_DEJAVU.Text = "DEJAVU"
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 4
        Me.MenuItem14.Text = "-"
        '
        'ShowPreview
        '
        Me.ShowPreview.Checked = true
        Me.ShowPreview.Index = 5
        Me.ShowPreview.Text = "Show Preview"
        '
        'UseSourceUI
        '
        Me.UseSourceUI.Checked = true
        Me.UseSourceUI.Index = 6
        Me.UseSourceUI.Text = "Use Source UI"
        '
        'DiscardBlankPages
        '
        Me.DiscardBlankPages.Index = 7
        Me.DiscardBlankPages.Text = "Discard Blank Pages"
        '
        'ShowBarcodeInfo
        '
        Me.ShowBarcodeInfo.Index = 8
        Me.ShowBarcodeInfo.Text = "Show Barcode Information"
        '
        'UseGetMessage
        '
        Me.UseGetMessage.Index = 10
        Me.UseGetMessage.Text = "Use GetMessage TWAIN Loop"
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 2
        Me.MenuItem10.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.LoggingOptions})
        Me.MenuItem10.Text = "&TWAIN Logging"
        '
        'LoggingOptions
        '
        Me.LoggingOptions.Index = 0
        Me.LoggingOptions.Text = "Logging Options..."
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 3
        Me.MenuItem3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuCantonese, Me.menuDutch, Me.menuEnglish, Me.menuFrench, Me.menuGerman, Me.menuGreek, Me.menuItalian, Me.menuJapanese, Me.menuKorean, Me.menuPortuguese, Me.menuRomanian, Me.menuRussian, Me.menuSimplifiedChinese, Me.menuSpanish, Me.menuTurkish, Me.menuTraditionalChinese, Me.menuCustomLanguage})
        Me.MenuItem3.Text = "Language"
        '
        'menuCantonese
        '
        Me.menuCantonese.Index = 0
        Me.menuCantonese.Text = "Cantonese"
        '
        'menuDutch
        '
        Me.menuDutch.Index = 1
        Me.menuDutch.Text = "Dutch"
        '
        'menuEnglish
        '
        Me.menuEnglish.Index = 2
        Me.menuEnglish.Text = "English"
        '
        'menuFrench
        '
        Me.menuFrench.Index = 3
        Me.menuFrench.Text = "French"
        '
        'menuGerman
        '
        Me.menuGerman.Index = 4
        Me.menuGerman.Text = "German"
        '
        'menuGreek
        '
        Me.menuGreek.Index = 5
        Me.menuGreek.Text = "Greek"
        '
        'menuItalian
        '
        Me.menuItalian.Index = 6
        Me.menuItalian.Text = "Italian"
        '
        'menuJapanese
        '
        Me.menuJapanese.Index = 7
        Me.menuJapanese.Text = "Japanese"
        '
        'menuKorean
        '
        Me.menuKorean.Index = 8
        Me.menuKorean.Text = "Korean"
        '
        'menuPortuguese
        '
        Me.menuPortuguese.Index = 9
        Me.menuPortuguese.Text = "Portuguese"
        '
        'menuRomanian
        '
        Me.menuRomanian.Index = 10
        Me.menuRomanian.Text = "Romanian"
        '
        'menuRussian
        '
        Me.menuRussian.Index = 11
        Me.menuRussian.Text = "Russian"
        '
        'menuSimplifiedChinese
        '
        Me.menuSimplifiedChinese.Index = 12
        Me.menuSimplifiedChinese.Text = "Simplified Chinese"
        '
        'menuSpanish
        '
        Me.menuSpanish.Index = 13
        Me.menuSpanish.Text = "Spanish"
        '
        'menuTurkish
        '
        Me.menuTurkish.Index = 14
        Me.menuTurkish.Text = "Turkish"
        '
        'menuTraditionalChinese
        '
        Me.menuTraditionalChinese.Index = 15
        Me.menuTraditionalChinese.Text = "Traditional Chinese"
        '
        'menuCustomLanguage
        '
        Me.menuCustomLanguage.Index = 16
        Me.menuCustomLanguage.Text = "Cusstom Language..."
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 4
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.DTWAINVersion})
        Me.MenuItem2.Text = "Help"
        '
        'DTWAINVersion
        '
        Me.DTWAINVersion.Index = 0
        Me.DTWAINVersion.Text = "DTWAIN Version..."
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 9
        Me.MenuItem12.Text = "-"
        '
        'VB_FullDemo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(664, 621)
        Me.Menu = Me.MainMenu1
        Me.Name = "VB_FullDemo"
        Me.Text = "DTWAIN VB .NET Example"
        Me.ResumeLayout(false)

End Sub

#End Region

    Private TwainOK As Integer
    Private TwainHandle As IntPtr
    Private Shared SelectedSource As IntPtr
    Private sOrigTitle As String
    Private Shared thisObject As VB_FullDemo
    Private dllExists As Boolean
    Private Shared textElement As IntPtr
    Private Shared pdfPageCount As Integer
    Private Shared cb As DTWAINAPI.DTwainCallback = New DTWAINAPI.DTwainCallback(AddressOf Callbackfn)
    Public Shared DTWAINAPI As Dynarithmic.DTWAINAPI = Nothing


    Private Sub VB_FullDemo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SelectSource.Enabled = False
        dllExists = True
        sOrigTitle = Me.Text

        ' Load the 32-bit or 64-bit DLL, depending on the application type
        Try
            If IntPtr.Size = 8 Then
                DTWAINAPI = New DTWAINAPI("dtwain64u.dll")
            Else
                DTWAINAPI = New DTWAINAPI("dtwain32u.dll")
            End If
        Catch ex As System.DllNotFoundException
            MessageBox.Show(ex.Message)
            dllExists = False
            Dispose()
        End Try
        TwainOK = DTWAINAPI.DTWAIN_IsTwainAvailable()
        SelectedSource = 0
        If TwainOK <> 0 Then
            TwainHandle = DTWAINAPI.DTWAIN_SysInitialize()
            Me.SelectSource.Enabled = True
            If TwainHandle <> 0 Then
                DTWAINAPI.DTWAIN_EnableMsgNotify(1)
                DTWAINAPI.DTWAIN_SetCallback(cb, 0)
                DTWAINAPI.DTWAIN_InitOCRInterface()
                textElement = DTWAINAPI.DTWAIN_CreatePDFTextElement
                DTWAINAPI.DTWAIN_SetPDFTextElementLong(textElement, 100, 100, DTWAINAPI.DTWAIN_PDFTEXTELEMENT_POSITION)
                DTWAINAPI.DTWAIN_SetPDFTextElementLong(textElement, DTWAINAPI.DTWAIN_PDFTEXT_ALLPAGES, 0, DTWAINAPI.DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS)
                DTWAINAPI.DTWAIN_SetPDFTextElementFloat(textElement, 25, 0, DTWAINAPI.DTWAIN_PDFTEXTELEMENT_FONTHEIGHT)
            Else
                Application.Exit()
            End If
        End If
        EnableSourceItems(False)
    End Sub
    Public Shared Function Callbackfn(wparam As IntPtr, lparam As IntPtr, userval As IntPtr) As IntPtr
        Select Case wparam
            Case DTWAINAPI.DTWAIN_TN_ACQUIRESTARTED
                pdfPageCount = 1
                Exit Select

            Case DTWAINAPI.DTWAIN_TN_FILEPAGESAVING
                If stamp_pdf Then
                    Dim text As String
                    text = "Page " & pdfPageCount
                    DTWAINAPI.DTWAIN_SetPDFTextElementString(textElement, text, DTWAINAPI.DTWAIN_PDFTEXTELEMENT_TEXT)
                    pdfPageCount = pdfPageCount + 1
                End If
                Exit Select

            Case DTWAINAPI.DTWAIN_TN_QUERYPAGEDISCARD
                If thisObject.ShowPreview.Checked Then
                    Dim sDIBDlg As New DibDisplayerDlg2(DTWAINAPI.DTWAIN_GetCurrentAcquiredImage(SelectedSource))
                    If sDIBDlg.ShowDialog() = DialogResult.Cancel Then
                        Return 0
                    End If
                End If
            Case DTWAINAPI.DTWAIN_TN_TRANSFERDONE
                If thisObject.ShowBarcodeInfo.Checked Then
                    Dim sBarcodeDlg = New BarCodesDlg(SelectedSource)
                    sBarcodeDlg.ShowDialog()
                End If
                Exit Select
        End Select
        Return 1
    End Function

    Private Sub InitializeFileDriverMap()
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_BMP, FF_BMP)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_JFIF, FF_JFIF)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_TIFF, FF_TIFF)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_TIFFMULTI, FF_TIFFMULTI)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_PNG, FF_PNG)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_PDF, FF_PDF)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_PDFA, FF_PDFA)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_PDFA2, FF_PDFA2)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_PDFRASTER, FF_PDFRASTER)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_FPX, FF_FPX)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_EXIF, FF_EXIF)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_SPIFF, FF_SPIFF)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_XBM, FF_XBM)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_PICT, FF_PICT)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_JP2, FF_JP2)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_JPX, FF_JPX)
        fileDriverMap.Add(DTWAINAPI.DTWAIN_FF_DEJAVU, FF_DEJAVU)
    End Sub

    Private Sub EnableFileDriverItems(bEnable As Boolean)
        For Each entry In fileDriverMap
            entry.Value.Enabled =
                bEnable AndAlso
                (DTWAINAPI.DTWAIN_IsFileXferSupported(SelectedSource, entry.Key) = 1)
        Next
    End Sub

    Private Sub SetCaptionToSourceName()
        Dim sourceName As New StringBuilder(256)
        Dim sTitle As New StringBuilder
        sTitle.Append(sOrigTitle)
        If SelectedSource <> 0 Then
            DTWAINAPI.DTWAIN_GetSourceProductName(SelectedSource, sourceName, 255)
            sTitle.Append(" - ")
            sTitle.Append(sourceName)
            Text = sTitle.ToString
        Else
            Text = sOrigTitle
        End If
    End Sub

    Private Sub SelectTheSource(nWhich As Long)
        Dim nReturn As Long
        If SelectedSource <> 0 Then
            nReturn = MessageBox.Show("For this demo, only one Source can be opened.  Close current Source?", "DTWAIN Message", MessageBoxButtons.YesNo)
            If nReturn = DialogResult.Yes Then
                DTWAINAPI.DTWAIN_CloseSource(SelectedSource)
                SelectedSource = 0
            Else
                Return
            End If
        End If
        Enabled = False
        Select Case nWhich
            Case 0
                SelectedSource = DTWAINAPI.DTWAIN_SelectSource2(IntPtr.Zero, Nothing, 0, 0,
                                                                DTWAINAPI.DTWAIN_DLG_CENTER_CURRENT_MONITOR Or 
                                                                DTWAINAPI.DTWAIN_DLG_TOPMOSTWINDOW Or DTWAINAPI.DTWAIN_DLG_SORTNAMES)
            Case 1
                Dim objSelectSourceByName = New SelectSourceByName()
                Dim nResult As DialogResult = objSelectSourceByName.ShowDialog()
                If nResult = DialogResult.OK Then
                    SelectedSource = DTWAINAPI.DTWAIN_SelectSourceByName(objSelectSourceByName.GetText())
                End If
            Case 2
                SelectedSource = DTWAINAPI.DTWAIN_SelectDefaultSource
            Case 3
                Dim customSourceDlg As New CustomSelectSource()
                Dim dResult As DialogResult = customSourceDlg.ShowDialog()
                If dResult = DialogResult.OK Then
                    SelectedSource = DTWAINAPI.DTWAIN_SelectSourceByName(customSourceDlg.GetSourceName())
                End If
        End Select
        Enabled = True
        If SelectedSource <> 0 Then
            If DTWAINAPI.DTWAIN_OpenSource(SelectedSource) <> 0 Then
                DTWAINAPI.DTWAIN_EnableBarcodeDetection(SelectedSource, 1)
                DTWAINAPI.DTWAIN_AddPDFTextElement(SelectedSource, textElement)
                DTWAINAPI.DTWAIN_EnableFeeder(SelectedSource, 1)
                SetCaptionToSourceName()
                EnableSourceItems(True)
                EnableBarcodeItems(True)
                EnableFileDriverItems(True)
                SetGetMessageLoopProcessing()
                Return
            Else
                MessageBox.Show("Error Opening selected Source", "TWAIN Error", MessageBoxButtons.OK)
                SetCaptionToSourceName()
                EnableSourceItems(False)
                EnableBarcodeItems(False)
            End If
        Else
            Dim lastError As Integer
            lastError = DTWAINAPI.DTWAIN_GetLastError()
            If lastError = DTWAINAPI.DTWAIN_ERR_SOURCESELECTION_CANCELED Then
                MessageBox.Show("Source selection canceled", "TWAIN Info", MessageBoxButtons.OK)
            Else
                Dim szErr = New StringBuilder(100)
                DTWAINAPI.DTWAIN_GetErrorString(lastError, szErr, 100)
                MessageBox.Show("Error Selecting and/or opening Source.\r\n" + szErr.ToString(), "TWAIN Error", MessageBoxButtons.OK)
            End If
            SetCaptionToSourceName()
            EnableSourceItems(False)
        End If
    End Sub
    Private Sub EnableBarcodeItems(bEnable As Boolean)
        Dim supported As Integer = DTWAINAPI.DTWAIN_IsExtImageInfoSupported(SelectedSource)
        If bEnable And supported = 1 Then
            ShowBarcodeInfo.Enabled = True
        Else
            ShowBarcodeInfo.Enabled = False
            If supported = 0 Then
                ShowBarcodeInfo.Checked = False
            End If
        End If
    End Sub

    Private Sub SelectSource_Click(sender As System.Object, e As EventArgs) Handles SelectSource.Click
        SelectTheSource(0)
        Focus()
    End Sub

    Public Function GetImageFromClipboard() As Image
        If Not Clipboard.GetDataObject() Is Nothing Then
            Dim dobj As IDataObject = Clipboard.GetDataObject()
            If dobj.GetDataPresent(DataFormats.Bitmap) Then
                Dim img_obj As Object = dobj.GetData(DataFormats.Bitmap)
                Return CType(img_obj, Bitmap)
            End If
        End If
    End Function


    Private Sub ExitApp_Click(sender As System.Object, e As EventArgs) Handles ExitDemo.Click
        Dispose(True)
    End Sub

    Private Sub SourceProperties_Click(sender As System.Object, e As EventArgs) Handles SourceProperties.Click
        If SelectedSource <> 0 Then
            Dim sPropDlg As SourcePropertiesDlg
            sPropDlg = New SourcePropertiesDlg(SelectedSource)
            sPropDlg.ShowDialog()
        End If
    End Sub
    Private Sub GenericAcquire(nWhich As Integer)
        If SelectedSource <> 0 Then
            Dim isChecked As Integer
            Dim isUI As Integer
            isChecked = 0
            isUI = 0
            If DiscardBlankPages.Checked Then
                isChecked = 1
            End If
            If UseSourceUI.Checked Then
                isUI = 1
            End If

            DTWAINAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5, DTWAINAPI.DTWAIN_BP_AUTODISCARD_ANY, isChecked)
            Dim acquireArray As IntPtr = DTWAINAPI.DTWAIN_CreateAcquisitionArray()
            Enabled = False
            Dim status = 0
            Dim returnVal = 0
            If nWhich = 0 Then
                returnVal = DTWAINAPI.DTWAIN_AcquireNativeEx(SelectedSource, DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, isUI, 0, acquireArray, status)
            Else
                returnVal = DTWAINAPI.DTWAIN_AcquireBufferedEx(SelectedSource, DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, isUI, 0, acquireArray, status)
            End If

            If returnVal = 0 Then
                If status = DTWAINAPI.DTWAIN_TN_ACQUIRECANCELLED Then
                    MessageBox.Show("No Images acquired", "TWAIN Information")
                Else
                    Dim errorVal As Integer = DTWAINAPI.DTWAIN_GetLastError()
                    Dim errorString = New StringBuilder(256)
                    DTWAINAPI.DTWAIN_GetErrorString(errorVal, errorString, 255)
                    MessageBox.Show(errorString.ToString(), "TWAIN Information")
                End If
                Enabled = True
                Return
            End If

            Dim nCount As Integer = DTWAINAPI.DTWAIN_GetNumAcquisitions(acquireArray)
            If nCount = 0 Then
                MessageBox.Show("No Images acquired", "TWAIN Information")
                Enabled = True
                Return
            End If

            Dim sDIBDlg = New DibDisplayerDlg(acquireArray)
            sDIBDlg.ShowDialog()
            Enabled = True
        End If
    End Sub

    Private Sub AcquireNative_Click(sender As System.Object, e As EventArgs) Handles AcquireNative.Click
        GenericAcquire(0)
    End Sub
    Private Sub AcquireBuffered_Click(sender As System.Object, e As EventArgs) Handles AcquireBuffered.Click
        GenericAcquire(1)
    End Sub
    Private Sub AcquireToFile(nWhich As Integer, fileType As Integer)
        If SelectedSource <> 0 Then
            Dim status = 0
            Dim bError = 0
            Dim FileFlags As Long = 0
            Dim tFileName = ""
            Select Case nWhich
                Case 0
                    FileFlags = DTWAINAPI.DTWAIN_USELONGNAME Or DTWAINAPI.DTWAIN_USENATIVE
                    DTWAINAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5, CInt(DTWAINAPI.DTWAIN_BP_AUTODISCARD_ANY), IsDiscardPages())
                Exit Select  
                Case 1
                    FileFlags = DTWAINAPI.DTWAIN_USESOURCEMODE Or DTWAINAPI.DTWAIN_USELONGNAME
                    DTWAINAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5, CInt(DTWAINAPI.DTWAIN_BP_AUTODISCARD_ANY), IsDiscardPages())
                Exit Select  
            End Select  
            Dim fDlg As New FileTypeDlg()
            Dim response As DialogResult = fDlg.ShowDialog(Me)
            If response = DialogResult.Cancel Then
                Return
            End If
            tFileName = fDlg.GetFileName()
            If tFileName.Length = 0 Then
                MessageBox.Show("File name cannot be blank", "File name error")
                Return
            End If

            ' Use default 
            ' Get all pages 
            ' Close Source when UI is closed 
            Enabled = False
            bError = DTWAINAPI.DTWAIN_AcquireFile(SelectedSource, tFileName, fileType, CInt(FileFlags + DTWAINAPI.DTWAIN_CREATE_DIRECTORY), DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, IsSourceUI(), 1, status)
            Enabled = True

            If bError = 0 Then
                If status = DTWAINAPI.DTWAIN_TN_ACQUIRECANCELLED Then
                    MessageBox.Show("No Images acquired", "TWAIN Information")
                Else
                    Dim errorVal As Integer = DTWAINAPI.DTWAIN_GetLastError()
                    Dim errorString = New StringBuilder(256)
                    DTWAINAPI.DTWAIN_GetErrorString(errorVal, errorString, 255)
                    MessageBox.Show(errorString.ToString(), "TWAIN Information")
                End If
                Return
            End If
            If DTWAINAPI.DTWAIN_GetFileSavePageCount(SelectedSource) = 0 Then
                MessageBox.Show("No files were saved")
            Else
                MessageBox.Show("Image file saved successfully")
            End If
        End If
    End Sub

    Private Sub SelectSourceByName_Click(sender As System.Object, e As EventArgs) Handles SelectSourceByName.Click
        SelectTheSource(1)
        Focus()
    End Sub

    Private Sub SelectSourceCustom_Click(sender As System.Object, e As EventArgs) Handles SelectSourceCustom.Click
        SelectTheSource(3)
        Focus()
    End Sub

    Private Sub SelectDefaultSource_Click(sender As System.Object, e As EventArgs) Handles SelectDefaultSource.Click
        SelectTheSource(2)
        Focus()
    End Sub

    Private Sub EnableSourceItems(bEnable As Boolean)
        SourceProperties.Enabled = bEnable
        CloseSource.Enabled = bEnable
        AcquireNative.Enabled = bEnable
        AcquireBuffered.Enabled = bEnable
        AcquireFile.Enabled = bEnable
        AcquireFileUsingDriver.Enabled = bEnable
    End Sub

    Private Sub CloseSource_Click(sender As System.Object, e As EventArgs) Handles CloseSource.Click
        If SelectedSource <> 0 Then
            DTWAINAPI.DTWAIN_CloseSource(SelectedSource)
            SelectedSource = 0
            SetCaptionToSourceName()
            EnableSourceItems(False)
            EnableFileDriverItems(false)
        End If
    End Sub

    Private Function IsDiscardPages() As Integer
        If DiscardBlankPages.Enabled Then
            Return 1
        End If
        Return 0
    End Function

    Private Function IsSourceUI() As Integer
        If UseSourceUI.Checked Then
            Return 1
        End If
        Return 0
    End Function

    Private Sub UseSourceUI_Click(sender As System.Object, e As EventArgs) Handles UseSourceUI.Click
        UseSourceUI.Checked = Not UseSourceUI.Checked
    End Sub

    Private Sub LoggingOptions_Click(sender As System.Object, e As EventArgs) Handles LoggingOptions.Click
        Dim LogFlags As Long = DTWAINAPI.DTWAIN_LOG_ALL And Not DTWAINAPI.DTWAIN_LOG_ERRORMSGBOX
        Dim logDlg As New LogFileSelectionDlg(1)
        Dim nResult As DialogResult = logDlg.ShowDialog()
        If nResult = DialogResult.OK Then
            Dim debugOption As Integer = logDlg.GetDebugOption()
            DTWAINAPI.DTWAIN_SetTwainLog(0, "")
            Select Case debugOption
                Case 0
                    Exit Select
                Case 1
                    Exit Select
                Case 2
                    DTWAINAPI.DTWAIN_SetTwainLog(LogFlags Or DTWAINAPI.DTWAIN_LOG_USEFILE, logDlg.GetFileName())
                    Exit Select
                Case 3
                    DTWAINAPI.DTWAIN_SetTwainLog(LogFlags Or DTWAINAPI.DTWAIN_LOG_CONSOLE, "")
                    Exit Select
            End Select
        End If
    End Sub

    Private Sub ShowPreview_Click(sender As System.Object, e As EventArgs) Handles ShowPreview.Click
        ShowPreview.Checked = Not ShowPreview.Checked
    End Sub

    Private Sub DiscardBlankPages_Click(sender As System.Object, e As EventArgs) Handles DiscardBlankPages.Click
        DiscardBlankPages.Checked = Not DiscardBlankPages.Checked
    End Sub

    Private Sub DTWAINVersion_Click(sender As System.Object, e As EventArgs) Handles DTWAINVersion.Click
        Dim aDlg As New AboutDlg()
        aDlg.ShowDialog()
    End Sub

    Private Sub LoadLanguage(lang As String)
        Dim retVal As Integer
        retVal = DTWAINAPI.DTWAIN_LoadCustomStringResources(lang)
        If retVal = 0 Then
            MessageBox.Show("Error loading custom resource " + lang)
        Else
            MessageBox.Show("Language " + " loaded successfully.  Select a Source or choose Logging/Log To Console to see the results")
        End If
    End Sub
    Private Sub menuCantonese_Click(sender As Object, e As EventArgs) Handles menuCantonese.Click
        LoadLanguage("cantonese")
    End Sub
    Private Sub MenuDutch_Click(sender As Object, e As EventArgs) Handles menuDutch.Click
        LoadLanguage("dutch")
    End Sub

    Private Sub menuEnglish_Click(sender As Object, e As EventArgs) Handles menuEnglish.Click
        LoadLanguage("english")
    End Sub

    Private Sub menuFrench_Click(sender As Object, e As EventArgs) Handles menuFrench.Click
        LoadLanguage("french")
    End Sub

    Private Sub menuGerman_Click(sender As Object, e As EventArgs) Handles menuGerman.Click
        LoadLanguage("german")
    End Sub

    Private Sub menuItalian_Click(sender As Object, e As EventArgs) Handles menuItalian.Click
        LoadLanguage("italian")
    End Sub

    Private Sub menuPortuguese_Click(sender As Object, e As EventArgs) Handles menuPortuguese.Click
        LoadLanguage("portuguese")
    End Sub

    Private Sub menuRomanian_Click(sender As Object, e As EventArgs) Handles menuRomanian.Click
        LoadLanguage("romanian")
    End Sub

    Private Sub menuRussian_Click(sender As Object, e As EventArgs) Handles menuRussian.Click
        LoadLanguage("russian")
    End Sub

    Private Sub menuSimplifiedChinese_Click(sender As Object, e As EventArgs) Handles menuSimplifiedChinese.Click
        LoadLanguage("simplified_chinese")
    End Sub

    Private Sub menuSpanish_Click(sender As Object, e As EventArgs) Handles menuSpanish.Click
        LoadLanguage("spanish")
    End Sub
    Private Sub menuTurkish_Click(sender As Object, e As EventArgs) Handles menuTurkish.Click
        LoadLanguage("turkish")
    End Sub

    Private Sub menuCustomLanguage_Click(sender As Object, e As EventArgs) Handles menuCustomLanguage.Click
        Dim objCustomLanguage = New CustomLanguageDlg()
        Dim nResult As DialogResult = objCustomLanguage.ShowDialog()
        If nResult = DialogResult.OK Then
            LoadLanguage(objCustomLanguage.GetText())
        End If
    End Sub
    Private Sub menuTraditionalChinese_Click(sender As Object, e As EventArgs) Handles menuTraditionalChinese.Click
        LoadLanguage("traditional_chinese")
    End Sub
    Private Sub menuGreek_Click(sender As Object, e As EventArgs) Handles menuGreek.Click
        LoadLanguage("greek")
    End Sub
    Private Sub menuJapanese_Click(sender As Object, e As EventArgs) Handles menuJapanese.Click
        LoadLanguage("japanese")
    End Sub
    Private Sub menuKorean_Click(sender As Object, e As EventArgs) Handles menuKorean.Click
        LoadLanguage("korean")
    End Sub
    Private Sub ShowBarcodeInfo_Click(sender As Object, e As EventArgs) Handles ShowBarcodeInfo.Click
        ShowBarcodeInfo.Checked = Not ShowBarcodeInfo.Checked
    End Sub

    Private Sub BigTiff_NoCompression_Click(sender As Object, e As EventArgs) Handles BigTiff_NoCompression.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BIGTIFFNONEMULTI)
    End Sub

    Private Sub BigTiff_Group3_Click(sender As Object, e As EventArgs) Handles BigTiff_Group3.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BIGTIFFG3MULTI)
    End Sub

    Private Sub BigTiff_Group4_Click(sender As Object, e As EventArgs) Handles BigTiff_Group4.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BIGTIFFG4MULTI)
    End Sub

    Private Sub BigTIFF_Flate_Click(sender As Object, e As EventArgs) Handles BigTIFF_Flate.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BIGTIFFDEFLATEMULTI)
    End Sub

    Private Sub BigTIFF_JPEG_Click(sender As Object, e As EventArgs) Handles BigTIFF_JPEG.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BIGTIFFJPEGMULTI)
    End Sub

    Private Sub BigTIFF_LZW_Click(sender As Object, e As EventArgs) Handles BigTIFF_LZW.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BIGTIFFLZWMULTI)
    End Sub

    Private Sub BigTIFF_Packbits_Click(sender As Object, e As EventArgs) Handles BigTIFF_Packbits.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BIGTIFFPACKBITSMULTI)
    End Sub

    Private Sub BMP_Click(sender As Object, e As EventArgs) Handles BMP.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BMP)
    End Sub

    Private Sub BMP_RLE_Click(sender As Object, e As EventArgs) Handles BMP_RLE.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_BMP_RLE)
    End Sub

    Private Sub DCX_Click(sender As Object, e As EventArgs) Handles DCX.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_DCX)
    End Sub

    Private Sub EMF_Click(sender As Object, e As EventArgs) Handles EMF.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_EMF)
    End Sub

    Private Sub GIF_Click(sender As Object, e As EventArgs) Handles GIF.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_GIF)
    End Sub

    Private Sub ICO_Click(sender As Object, e As EventArgs) Handles ICO.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_ICO)
    End Sub

    Private Sub ICOVISTA_Click(sender As Object, e As EventArgs) Handles ICOVISTA.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_ICO_VISTA)
    End Sub

    Private Sub JPEG_Click(sender As Object, e As EventArgs) Handles JPEG.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_JPEG)
    End Sub

    Private Sub JPEG2000_Click(sender As Object, e As EventArgs) Handles JPEG2000.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_JPEG2000)
    End Sub

    Private Sub JPEGXL_Click(sender As Object, e As EventArgs) Handles JPEGXL.Click
        Dim isSupported As Integer = DTWAINAPI.DTWAIN_IsFileTypeSupported(DTWAINAPI.DTWAIN_JPEGXL)
        If isSupported = 0 Then
            MessageBox.Show("JPEG-XL library not found or one or more JPEG-XL dependencies were not found. Please install JPEG-XL components now and select JPEG-XL again...")
            Return
        Else
            AcquireToFile(0, DTWAINAPI.DTWAIN_JPEGXL)
        End If
    End Sub

    Private Sub JPEGXR_Click(sender As Object, e As EventArgs) Handles JPEGXR.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_JPEGXR)
    End Sub

    Private Sub PSD_Click(sender As Object, e As EventArgs) Handles PSD.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_PSD)
    End Sub

    Private Sub PCX_Click(sender As Object, e As EventArgs) Handles PCX.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_PCX)
    End Sub
    Private Function IsPDFEncrypted(value As Integer) As Boolean
        Select Case value
            Case PDF_128BitAES_Encryption_Value:
                return True
            Case PDF_256BitAES_Encryption_Value:
                return True
            Case PDF_40BitRC4_Encryption_Value:
                return True
            Case PDF_128BitRC4_Encryption_Value:
                return True
        End Select
        Return False
    End Function

    Private Sub Do_PDF_Acquisition(encryptionType As Integer)
        Dim pdfEncrypted As Boolean = IsPDFEncrypted(encryptionType)
        Dim optionsDlg As new PDFOptionsDlg(pdfEncrypted)
        optionsDlg.ShowDialog(Me)
        Dim optionsData As PDFOptionsDlg.PDFOptionsData = optionsDlg.GetPDFOptions()
        DTWAINAPI.DTWAIN_SetPDFAuthor(SelectedSource, optionsData.sPDFAuthor)
        DTWAINAPI.DTWAIN_SetPDFCreator(SelectedSource, optionsData.sPDFCreator)
        DTWAINAPI.DTWAIN_SetPDFKeywords(SelectedSource, optionsData.sPDFKeywords)
        DTWAINAPI.DTWAIN_SetPDFTitle(SelectedSource, optionsData.sPDFTitle)
        DTWAINAPI.DTWAIN_SetPDFSubject(SelectedSource, optionsData.sPDFSubject)

        Dim useStrong = 0
        If pdfEncrypted Then
            Select Case encryptionType
                Case PDF_128BitAES_Encryption_Value:
                    useStrong = 1
                Case PDF_256BitAES_Encryption_Value:
                    useStrong = 1
                Case PDF_128BitRC4_Encryption_Value:
                    useStrong = 1
                Exit Select
            End Select
            DTWAINAPI.DTWAIN_SetPDFEncryption(SelectedSource, 1, optionsData.sUserPassword, optionsData.sOwnerPassword,
                                             DTWAINAPI.DTWAIN_PDF_ALLOWALL, useStrong)
            If encryptionType = PDF_128BitAES_Encryption_Value Then
                DTWAINAPI.DTWAIN_SetPDFAESEncryption(SelectedSource, DTWAINAPI.DTWAIN_PDF_AES128, 1)
            ElseIf encryptionType = PDF_256BitAES_Encryption_Value Then
                DTWAINAPI.DTWAIN_SetPDFAESEncryption(SelectedSource, DTWAINAPI.DTWAIN_PDF_AES256, 1)
            End If
        Else If encryptionType = PDF_ASCII85_Value Then
            DTWAINAPI.DTWAIN_SetPDFASCIICompression(SelectedSource, 1)
        End If
        stamp_pdf = optionsData.stamp_pdf
        AcquireToFile(0, DTWAINAPI.DTWAIN_PDFMULTI)
    End Sub

    Private Sub PDF_NoEncryption_Click(sender As Object, e As EventArgs) Handles PDF_NoEncryption.Click
        Do_PDF_Acquisition(PDF_NoEncryption_Value)
    End Sub

    Private Sub PDF_ASCII85_Click(sender As Object, e As EventArgs) Handles PDF_ASCII85.Click
        Do_PDF_Acquisition(PDF_ASCII85_Value)
    End Sub

    Private Sub PDF_40BitRC4_Encryption_Click(sender As Object, e As EventArgs) Handles PDF_40BitRC4_Encryption.Click
        Do_PDF_Acquisition(PDF_40BitRC4_Encryption_Value)
    End Sub

    Private Sub PDF_128BitRC4_Encryption_Click(sender As Object, e As EventArgs) Handles PDF_128BitRC4_Encryption.Click
        Do_PDF_Acquisition(PDF_128BitRC4_Encryption_Value)
    End Sub

    Private Sub PDF_128BitAES_Encryption_Click(sender As Object, e As EventArgs) Handles PDF_128BitAES_Encryption.Click
        Do_PDF_Acquisition(PDF_128BitAES_Encryption_Value)
    End Sub

    Private Sub PDF_256BitAES_Encryption_Click(sender As Object, e As EventArgs) Handles PDF_256BitAES_Encryption.Click
        Do_PDF_Acquisition(PDF_256BitAES_Encryption_Value)
    End Sub

    Private Sub PNG_Click(sender As Object, e As EventArgs) Handles PNG.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_PNG)
    End Sub

    Private Sub PostScript_Level1_Click(sender As Object, e As EventArgs) Handles PostScript_Level1.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_POSTSCRIPT1MULTI)
    End Sub

    Private Sub PostScript_Level2_Click(sender As Object, e As EventArgs) Handles PostScript_Level2.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_POSTSCRIPT2MULTI)
    End Sub

    Private Sub PostScript_Level3_Click(sender As Object, e As EventArgs) Handles PostScript_Level3.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_POSTSCRIPT3MULTI)
    End Sub

    Private Sub SVG_Click(sender As Object, e As EventArgs) Handles SVG.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_SVG)
    End Sub

    Private Sub SVGZ_Click(sender As Object, e As EventArgs) Handles SVGZ.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_SVGZ)
    End Sub

    Private Sub TGA_Click(sender As Object, e As EventArgs) Handles TGA.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TGA)
    End Sub

    Private Sub TGA_RLE_Click(sender As Object, e As EventArgs) Handles TGA_RLE.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TGA_RLE)
    End Sub

    Private Sub Tiff_NoCompression_Click(sender As Object, e As EventArgs) Handles Tiff_NoCompression.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TIFFNONEMULTI)
    End Sub

    Private Sub Tiff_Group3_Click(sender As Object, e As EventArgs) Handles Tiff_Group3.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TIFFG3MULTI)
    End Sub

    Private Sub Tiff_Group4_Click(sender As Object, e As EventArgs) Handles Tiff_Group4.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TIFFG4MULTI)
    End Sub

    Private Sub Tiff_Flate_Click(sender As Object, e As EventArgs) Handles Tiff_Flate.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TIFFDEFLATEMULTI)
    End Sub

    Private Sub Tiff_JPEG_Click(sender As Object, e As EventArgs) Handles Tiff_JPEG.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TIFFJPEGMULTI)
    End Sub

    Private Sub Tiff_LZW_Click(sender As Object, e As EventArgs) Handles Tiff_LZW.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TIFFLZWMULTI)
    End Sub

    Private Sub Tiff_Packbits_Click(sender As Object, e As EventArgs) Handles Tiff_Packbits.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_TIFFPACKBITSMULTI)
    End Sub

    Private Sub WEBP_Click(sender As Object, e As EventArgs) Handles WEBP.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_WEBP)
    End Sub

    Private Sub WMF_Click(sender As Object, e As EventArgs) Handles WMF.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_WMF)
    End Sub

    Private Sub WBMP_Click(sender As Object, e As EventArgs) Handles WBMP.Click
        AcquireToFile(0, DTWAINAPI.DTWAIN_WBMP)
    End Sub

    Private Sub FF_BMP_Click(sender As Object, e As EventArgs) Handles FF_BMP.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_BMP)
    End Sub

    Private Sub FF_JFIF_Click(sender As Object, e As EventArgs) Handles FF_JFIF.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_JFIF)
    End Sub

    Private Sub FF_TIFF_Click(sender As Object, e As EventArgs) Handles FF_TIFF.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_TIFF)
    End Sub

    Private Sub FF_TIFFMULTI_Click(sender As Object, e As EventArgs) Handles FF_TIFFMULTI.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_TIFFMULTI)
    End Sub

    Private Sub FF_PNG_Click(sender As Object, e As EventArgs) Handles FF_PNG.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_PNG)
    End Sub

    Private Sub FF_PDF_Click(sender As Object, e As EventArgs) Handles FF_PDF.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_PDF)
    End Sub

    Private Sub FF_PDFA_Click(sender As Object, e As EventArgs) Handles FF_PDFA.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_PDFA)
    End Sub

    Private Sub FF_PDFA2_Click(sender As Object, e As EventArgs) Handles FF_PDFA2.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_PDFA2)
    End Sub

    Private Sub FF_PDFRASTER_Click(sender As Object, e As EventArgs) Handles FF_PDFRASTER.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_PDFRASTER)
    End Sub

    Private Sub FF_FPX_Click(sender As Object, e As EventArgs) Handles FF_FPX.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_FPX)
    End Sub

    Private Sub FF_EXIF_Click(sender As Object, e As EventArgs) Handles FF_EXIF.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_EXIF)
    End Sub

    Private Sub FF_SPIFF_Click(sender As Object, e As EventArgs) Handles FF_SPIFF.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_SPIFF)
    End Sub

    Private Sub FF_XBM_Click(sender As Object, e As EventArgs) Handles FF_XBM.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_XBM)
    End Sub

    Private Sub FF_PICT_Click(sender As Object, e As EventArgs) Handles FF_PICT.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_PICT)
    End Sub

    Private Sub FF_JP2_Click(sender As Object, e As EventArgs) Handles FF_JP2.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_JP2)
    End Sub

    Private Sub FF_JPX_Click(sender As Object, e As EventArgs) Handles FF_JPX.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_JPX)
    End Sub

    Private Sub FF_DEJAVU_Click(sender As Object, e As EventArgs) Handles FF_DEJAVU.Click
        AcquireToFile(1, DTWAINAPI.DTWAIN_FF_DEJAVU)
    End Sub

    Private Sub TEXTOCR_Click(sender As Object, e As EventArgs) Handles TEXTOCR.Click
        Dim isSupported As Integer = DTWAINAPI.DTWAIN_IsFileTypeSupported(DTWAINAPI.DTWAIN_TEXTMULTI)
        If isSupported = 0 Then
            MessageBox.Show("Text mode requires OCR engine installed", "OCR Error")
            Return
        Else
            AcquireToFile(0, DTWAINAPI.DTWAIN_TEXTMULTI)
        End If
    End Sub

    Private Sub SetGetMessageLoopProcessing()
        Dim valueToUse As Integer
        If UseGetMessage.Checked Then
            valueToUse = 0
        Else
            valueToUse = 1
        End If
        DTWAINAPI.DTWAIN_EnablePeekMessageLoop(SelectedSource, valueToUse)
    End Sub

    Private Sub UseGetMessage_Click(sender As Object, e As EventArgs) Handles UseGetMessage.Click
        UseGetMessage.Checked = Not UseGetMessage.Checked
        SetGetMessageLoopProcessing()
    End Sub
End Class
