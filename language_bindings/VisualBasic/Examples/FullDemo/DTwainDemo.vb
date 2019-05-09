Imports System.Drawing
Imports System
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Diagnostics

Public Class DTwainDemo
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        thisObject = Me
        InitializeComponent()
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
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.SelectSource = New System.Windows.Forms.MenuItem
        Me.SelectSourceByName = New System.Windows.Forms.MenuItem
        Me.SelectDefaultSource = New System.Windows.Forms.MenuItem
        Me.SelectSourceCustom = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.SourceProperties = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.CloseSource = New System.Windows.Forms.MenuItem
        Me.ExitDemo = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.AcquireNative = New System.Windows.Forms.MenuItem
        Me.AcquireBuffered = New System.Windows.Forms.MenuItem
        Me.AcquireFile = New System.Windows.Forms.MenuItem
        Me.AcquireFileUsingDriver = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.ShowPreview = New System.Windows.Forms.MenuItem
        Me.UseSourceUI = New System.Windows.Forms.MenuItem
        Me.DiscardBlankPages = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.LoggingOptions = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.DTWAINVersion = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem9, Me.MenuItem10, Me.MenuItem2})
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
        Me.MenuItem9.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.AcquireNative, Me.AcquireBuffered, Me.AcquireFile, Me.AcquireFileUsingDriver, Me.MenuItem14, Me.ShowPreview, Me.UseSourceUI, Me.DiscardBlankPages})
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
        Me.AcquireFile.Text = "Acquire File..."
        '
        'AcquireFileUsingDriver
        '
        Me.AcquireFileUsingDriver.Index = 3
        Me.AcquireFileUsingDriver.Text = "Acquire File Using Driver..."
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 4
        Me.MenuItem14.Text = "-"
        '
        'ShowPreview
        '
        Me.ShowPreview.Checked = True
        Me.ShowPreview.Index = 5
        Me.ShowPreview.Text = "Show Preview"
        '
        'UseSourceUI
        '
        Me.UseSourceUI.Checked = True
        Me.UseSourceUI.Index = 6
        Me.UseSourceUI.Text = "Use Source UI"
        '
        'DiscardBlankPages
        '
        Me.DiscardBlankPages.Index = 7
        Me.DiscardBlankPages.Text = "Discard Blank Pages"
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
        'MenuItem2
        '
        Me.MenuItem2.Index = 3
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.DTWAINVersion})
        Me.MenuItem2.Text = "Help"
        '
        'DTWAINVersion
        '
        Me.DTWAINVersion.Index = 0
        Me.DTWAINVersion.Text = "DTWAIN Version..."
        '
        'DTwainDemo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(664, 641)
        Me.Menu = Me.MainMenu1
        Me.Name = "DTwainDemo"
        Me.Text = "DTWAIN VB .NET Example"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private TwainOK As Integer
    Private SelectedSource As Integer
    Private sOrigTitle As String
    Private Shared thisObject As DTwainDemo
    Private dllExists As Boolean
    Private Shared cb As DTWAINAPI.DTWAINCallback = New DTWAINAPI.DTWAINCallback(AddressOf callbackfn)


    Private Sub DTwainDemo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SelectSource.Enabled = False
        dllExists = True
        sOrigTitle = Me.Text
        Try
            MessageBox.Show("To run this demo, please make sure that DTWAIN32U.DLL is located in either the executable directory, or in a directory specified on your system path.")
            TwainOK = DTWAINAPI.DTWAIN_IsTwainAvailable()
        Catch ex As System.DllNotFoundException
            MessageBox.Show("Could not find or load DTWAIN32U.DLL")
            dllExists = False
            Dispose()
        End Try
        SelectedSource = 0
        If TwainOK = 1 Then
            TwainOK = DTWAINAPI.DTWAIN_SysInitialize()
            Me.SelectSource.Enabled = True
            If TwainOK Then
                DTWAINAPI.DTWAIN_EnableMsgNotify(1)
                DTWAINAPI.DTWAIN_SetCallback(cb, 0)
            End If
        End If
        EnableSourceItems(False)
    End Sub

    Public Shared Function callbackfn(ByVal wparam As Integer, ByVal lparam As Integer, ByVal userval As Integer) As Integer
        Select Case wparam
            Case DTWAINAPI.DTWAIN_TN_QUERYPAGEDISCARD
                If thisObject.ShowPreview.Checked Then
                    Dim sDIBDlg As New DibDisplayerDlg2(DTWAINAPI.DTWAIN_GetCurrentAcquiredImage(lparam))
                    If sDIBDlg.ShowDialog() = DialogResult.Cancel Then
                        Return 0
                    End If
                End If
                Exit Select
        End Select
        Return 1
    End Function

    Private Sub SetCaptionToSourceName()
        Dim SourceName As String
        Dim sTitle As String
        SourceName = Space$(256)
        sTitle = sOrigTitle
        If SelectedSource <> 0 Then
            DTWAINAPI.DTWAIN_GetSourceProductName(SelectedSource, SourceName, 255)
            sTitle += " - "
            sTitle += SourceName
            Me.Text = sTitle
        Else
            Me.Text = sOrigTitle
        End If
    End Sub

    Private Sub SelectTheSource(ByVal nWhich As Long)
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
        Me.Enabled = False
        Select Case nWhich
            Case 0
                SelectedSource = DTWAINAPI.DTWAIN_SelectSource
            Case 1
                Dim objSelectSourceByName As SelectSourceByName = New SelectSourceByName()
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
        Me.Enabled = True
        If SelectedSource <> 0 Then
            If DTWAINAPI.DTWAIN_OpenSource(SelectedSource) <> 0 Then
                DTWAINAPI.DTWAIN_EnableFeeder(SelectedSource, 1)
                SetCaptionToSourceName()
                EnableSourceItems(True)
                Return
            Else
                MessageBox.Show("Error Selecting Source", "TWAIN Error", MessageBoxButtons.OK)
                EnableSourceItems(False)
            End If
        End If
    End Sub

    Private Sub SelectSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectSource.Click
        SelectTheSource(0)
        Focus()
    End Sub

    Private Sub Acquire_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Enabled = False
        Dim Status As Integer
        If SelectedSource <> 0 Then
            If DTWAINAPI.DTWAIN_AcquireToClipboard(SelectedSource, DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, DTWAINAPI.DTWAIN_USENATIVE, 1, 1, 0, Status) Then
                'setting clipboard data to picturebox 
                Me.Focus()
            End If
        End If
        Me.Enabled = True
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


    Private Sub ExitApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitDemo.Click
        Dispose(True)
    End Sub

    Private Sub AcquireToFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Dlg As New FileTypeDlg()
        Dlg.ShowDialog()
        Dim FileName As String = Dlg.GetFileName()
        Dim FileType As Integer = Dlg.GetFileType()
        If FileType = -1 Then
            Return
        End If
        Me.Enabled = False
        Dim Status As Integer
        If SelectedSource <> 0 Then
            If DTWAINAPI.DTWAIN_AcquireFile(SelectedSource, FileName, FileType, DTWAINAPI.DTWAIN_USENATIVE + DTWAINAPI.DTWAIN_USELONGNAME, DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, 1, 0, Status) Then
                MsgBox(FileName + " has been created")
            End If
        End If
        Me.Enabled = True
    End Sub

    Private Sub SourceProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourceProperties.Click
        If SelectedSource <> 0 Then
            Dim sPropDlg As SourcePropertiesDlg
            sPropDlg = New SourcePropertiesDlg(SelectedSource)
            sPropDlg.ShowDialog()
        End If
    End Sub

    Private Sub AcquireNative_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcquireNative.Click
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
            Dim acquireArray As Integer = DTWAINAPI.DTWAIN_CreateAcquisitionArray()
            Me.Enabled = False
            Dim status As Integer = 0
            If DTWAINAPI.DTWAIN_AcquireNativeEx(SelectedSource, DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, isUI, 0, acquireArray, status) = 0 Then
                MessageBox.Show("Acquisition Failed", "TWAIN Error")
                Return
            End If
            If DTWAINAPI.DTWAIN_ArrayGetCount(acquireArray) = 0 Then
                MessageBox.Show("No Images Acquired", "")
                Return
            End If

            Dim sDIBDlg As DibDisplayerDlg = New DibDisplayerDlg(acquireArray)
            sDIBDlg.ShowDialog()
            DTWAINAPI.DTWAIN_DestroyAcquisitionArray(acquireArray, 0)
            Me.Enabled = True
        End If
    End Sub

    Private Sub SelectSourceByName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectSourceByName.Click
        SelectTheSource(1)
        Focus()
    End Sub

    Private Sub SelectSourceCustom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectSourceCustom.Click
        SelectTheSource(3)
        Focus()
    End Sub

    Private Sub SelectDefaultSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectDefaultSource.Click
        SelectTheSource(2)
        Focus()
    End Sub

    Private Sub EnableSourceItems(ByVal bEnable As Boolean)
        SourceProperties.Enabled = bEnable
        CloseSource.Enabled = bEnable
        AcquireNative.Enabled = bEnable
        AcquireBuffered.Enabled = bEnable
        AcquireFile.Enabled = bEnable
        AcquireFileUsingDriver.Enabled = bEnable
    End Sub

    Private Sub CloseSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseSource.Click
        If SelectedSource <> 0 Then
            DTWAINAPI.DTWAIN_CloseSource(SelectedSource)
            SelectedSource = 0
            SetCaptionToSourceName()
            EnableSourceItems(False)
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

    Private Sub AcquireBuffered_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcquireBuffered.Click
        If SelectedSource <> 0 Then
            DTWAINAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5, CInt(DTWAINAPI.DTWAIN_BP_AUTODISCARD_ANY), IsDiscardPages())
            Dim acquireArray As Integer = DTWAINAPI.DTWAIN_CreateAcquisitionArray()
            Me.Enabled = False
            Dim status As Integer = 0
            Dim IsUIChecked As Integer = 0
            If UseSourceUI.Checked Then
                IsUIChecked = 1
            End If
            If DTWAINAPI.DTWAIN_AcquireBufferedEx(SelectedSource, DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, IsUIChecked, 0, acquireArray, _
             status) = 0 Then
                MessageBox.Show("Acquisition Failed", "TWAIN Error")
                Return
            End If

            If DTWAINAPI.DTWAIN_ArrayGetCount(acquireArray) = 0 Then
                MessageBox.Show("No Images Acquired", "")
                Return
            End If

            ' Display the DIBS
            '...
            Dim sDIBDlg As New DibDisplayerDlg(acquireArray)
            sDIBDlg.ShowDialog()
            DTWAINAPI.DTWAIN_DestroyAcquisitionArray(acquireArray, 0)
            Me.Enabled = True
        End If
    End Sub

    Private Sub AcquireToFile(ByVal nWhich As Integer)
        If SelectedSource <> 0 Then
            Dim status As Integer = 0
            Dim bError As Integer = 0
            Dim FileFlags As Long = 0
            Dim tFileName As String = ""
            Dim fileType As Integer = 0
            Select Case nWhich
                Case 0
                    FileFlags = DTWAINAPI.DTWAIN_USELONGNAME Or DTWAINAPI.DTWAIN_USENATIVE
                    DTWAINAPI.DTWAIN_SetBlankPageDetection(SelectedSource, 98.5, CInt(DTWAINAPI.DTWAIN_BP_AUTODISCARD_ANY), IsDiscardPages())
                    Dim fDlg As New FileTypeDlg()
                    fDlg.ShowDialog()
                    tFileName = fDlg.GetFileName()
                    Dim szSourceName As New StringBuilder(tFileName)
                    fileType = fDlg.GetFileType()
                    Exit Select

                Case 1
                    If DTWAINAPI.DTWAIN_IsFileXferSupported(SelectedSource, DTWAINAPI.DTWAIN_ANYSUPPORT) = 0 Then
                        MessageBox.Show("Sorry.  The selected driver does not have built-in file transfer support.")
                        Return
                    End If
                    If DTWAINAPI.DTWAIN_IsFileXferSupported(SelectedSource, DTWAINAPI.DTWAIN_FF_BMP) = 0 Then
                        Dim sText As String = "Sorry.  This demo program only supports built-in BMP file transfers." & vbCr & vbLf
                        sText += "However, the DTWAIN library will support all built-in formats if your driver" & vbCr & vbLf
                        sText += "supports other formats."
                        MessageBox.Show(sText)
                        Return
                    End If
                    FileFlags = DTWAINAPI.DTWAIN_USESOURCEMODE Or DTWAINAPI.DTWAIN_USELONGNAME
                    fileType = DTWAINAPI.DTWAIN_FF_BMP
                    tFileName = ".\IMAGE.BMP"
                    MessageBox.Show("The name of the image file that will be saved is IMAGE.BMP" & vbLf)
                    Exit Select
            End Select

            ' Use default 
            ' Get all pages 
            ' Close Source when UI is closed 
            bError = DTWAINAPI.DTWAIN_AcquireFile(SelectedSource, tFileName, fileType, CInt(FileFlags), DTWAINAPI.DTWAIN_PT_DEFAULT, DTWAINAPI.DTWAIN_ACQUIREALL, IsSourceUI(), 1, status)

            If bError = 0 Then
                MessageBox.Show("Error acquiring or saving file.")
            ElseIf status = DTWAINAPI.DTWAIN_TN_ACQUIREDONE Then
                MessageBox.Show("Image file saved successfully")
            Else
                MessageBox.Show("The acquisition returned a status of " & status.ToString())
            End If
        End If
    End Sub

    Private Sub AcquireFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcquireFile.Click
        AcquireToFile(0)
    End Sub

    Private Sub UseSourceUI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseSourceUI.Click
        UseSourceUI.Checked = Not UseSourceUI.Checked
    End Sub

    Private Sub AcquireFileUsingDriver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcquireFileUsingDriver.Click
        AcquireToFile(1)
    End Sub

    Private Sub LoggingOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoggingOptions.Click
        Dim LogFlags As Long = DTWAINAPI.DTWAIN_LOG_ALL And Not DTWAINAPI.DTWAIN_LOG_ERRORMSGBOX
        Dim logDlg As New LogFileSelectionDlg(1)
        Dim nResult As DialogResult = logDlg.ShowDialog()
        If nResult = DialogResult.OK Then
            Dim debugOption As Integer = logDlg.GetDebugOption()
            Select Case debugOption
                Case 0
                    Exit Select
                Case 1
                    DTWAINAPI.DTWAIN_SetTwainLog(0, "")
                    Exit Select
                Case 2
                    DTWAINAPI.DTWAIN_SetTwainLog(CInt(LogFlags Or DTWAINAPI.DTWAIN_LOG_USEFILE), logDlg.GetFileName())
                    Exit Select
                Case 3
                    DTWAINAPI.DTWAIN_SetTwainLog(CInt(LogFlags And Not DTWAINAPI.DTWAIN_LOG_USEFILE), "")
                    MessageBox.Show("The DebugView debug monitor will start...")
                    Process.Start("DbgView.exe")
                    Exit Select
            End Select
        End If
    End Sub

    Private Sub ShowPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowPreview.Click
        ShowPreview.Checked = Not ShowPreview.Checked
    End Sub

    Private Sub DiscardBlankPages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscardBlankPages.Click
        DiscardBlankPages.Checked = Not DiscardBlankPages.Checked
    End Sub

    Private Sub DTWAINVersion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DTWAINVersion.Click
        Dim aDlg As New AboutDlg()
        aDlg.ShowDialog()
    End Sub
End Class
