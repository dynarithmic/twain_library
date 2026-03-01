Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
Imports Dynarithmic

Public Class TestCapDlg

    Private m_Source As System.IntPtr
    Private capToTest As String
    Private capToTestAsInt As Integer
    Private Shared ReadOnly allGetTypes As String() = {"MSG_GET", "MSG_GETCURRENT", "MSG_GETDEFAULT"}
    Private Shared ReadOnly allContainerTypes As String() = {"TW_ONEVALUE", "TW_ARRAY", "TW_ENUMERATION", "TW_RANGE"}
    Private Shared ReadOnly allContainerTypesID As Int32() = {DTWAINAPI.DTWAIN_CONTONEVALUE, DTWAINAPI.DTWAIN_CONTARRAY,
                                                              DTWAINAPI.DTWAIN_CONTENUMERATION,
                                                              DTWAINAPI.DTWAIN_CONTRANGE}
    Private Shared ReadOnly allDataTypes As String() = {"TWTY_INT8", "TWTY_INT16", "TWTY_INT32", "TWTY_UINT8", "TWTY_UINT16",
                            "TWTY_UINT32", "TWTY_BOOL", "TWTY_FIX32", "TWTY_FRAME", "TWTY_STR32",
                            "TWTY_STR64", "TWTY_STR128", "TWTY_STR255", "TWTY_STR1024", "TWTY_UNI512",
                            "TWTY_HANDLE"}
    Private Shared ReadOnly allRangeNames As String() = {"Minimum: ", "Maximum: ", "Step: ", "Default: ", "Current: "}

    Private Shared ReadOnly allSetTypes As String() = {"MSG_SET", "MSG_RESET", "MSG_SETCONSTRAINT"}


    Public Sub New(ByVal item As System.IntPtr, theCapToTest As String)
        InitializeComponent() ' This call is required by the Windows Form Designer.
        m_Source = item
        capToTest = theCapToTest
        capToTestAsInt = VB_FullDemo.DTWAINAPI.DTWAIN_GetCapFromName(capToTest)
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TestCapDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "Test Capability (" & capToTest & ")"
        InitTestControls()
    End Sub

    Private Sub InitTestControls()
        cmbGetTypes.Items.AddRange(allGetTypes)
        cmbContainer.Items.AddRange(allContainerTypes)
        cmbDataType.Items.AddRange(allDataTypes)
        cmbSetTypes.Items.AddRange(allSetTypes)
        cmbContainerSet.Items.AddRange(allContainerTypes)
        cmbDataTypeSet.Items.AddRange(allDataTypes)
        SetTestSelection("MSG_GET")
        SetTestSelection2("MSG_SET")
    End Sub

    Private Function FindItemByStringName(comboBox As ComboBox, itemName As String) As Integer
        For i As Integer = 0 To comboBox.Items.Count - 1
            If comboBox.Items(i).ToString() = itemName Then
                Return i
            End If
        Next i
        Return -1 ' Item Not found
    End Function

    Private Function GetBestMatchingContainer(container As Integer) As String
        For i As Integer = 0 To allContainerTypesID.Length - 1
            If ((container And allContainerTypesID(i)) <> 0) Then
                Return allContainerTypes(i)
            End If
        Next i
        Return "TW_ONEVALUE"
    End Function


    Private Sub SetTestSelection(ByVal typeOfGet As String)
        ' Position the cmbGetTypes combo to "typeOfGet"
        Dim nPos As Integer = FindItemByStringName(cmbGetTypes, typeOfGet)
        cmbGetTypes.SelectedIndex = nPos

        ' Get the equivalent "MSG_GETxxxxx" type matching the one passed in */
        Dim nID As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetConstantFromTwainName(typeOfGet)

        ' Choose the container type for the capability 
        Dim bestContainer As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetCapContainer(m_Source, capToTestAsInt, nID)

        ' Could be more than one container defined for this cap, so get the "best" one
        Dim sBestContainer As String = GetBestMatchingContainer(bestContainer)

        ' Position the cmbContainer control to the name equal to sBestContainer
        nPos = FindItemByStringName(cmbContainer, sBestContainer)
        If nPos <> -1 Then
            cmbContainer.SelectedIndex = nPos
        Else
            cmbContainer.SelectedIndex = 0
        End If

        ' Get the data type 
        Dim bestDataType As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetCapDataType(m_Source, capToTestAsInt)

        ' Get the name of the data type
        Dim szBestDataType As StringBuilder = New StringBuilder(100)
        VB_FullDemo.DTWAINAPI.DTWAIN_GetTwainNameFromConstant(DTWAINAPI.DTWAIN_CONSTANT_TWTY, bestDataType, szBestDataType, 100)

        ' Now position the cmbDataType combo to the name
        nPos = FindItemByStringName(cmbDataType, szBestDataType.ToString())
        If nPos <> -1 Then
            cmbDataType.SelectedIndex = nPos
        Else
            cmbDataType.SelectedIndex = 0
        End If
    End Sub

    Private Function SetTestSelection2(setType As String) As Integer
        Dim controlsToToggle As Control() =
        {
            lblSetOperation,
            lblSetContainer,
            lblSetDataType,
            cmbSetTypes,
            cmbContainerSet,
            cmbDataTypeSet,
            btnSetRevert,
            btnTestSet,
            lblInput,
            lblResultsSet,
            editInputData,
            lstResultsSet
        }

        Dim capOpts As Integer = 0
        ' Determine if setting the capability values for this cap Is supported 
        If VB_FullDemo.DTWAINAPI.DTWAIN_GetCapOperations(m_Source, capToTestAsInt, capOpts) = 1 Then
            ' Turn off "Set" controls if the capability does Not support the set operation 
            If (capOpts And DTWAINAPI.DTWAIN_CO_SET) = 0 Then
                For Each ctrl As Control In controlsToToggle
                    ctrl.Enabled = False
                Next
                Return capOpts ' Nothing else to do, since this capability does Not support having it set
            End If
        End If

        ' Position the cmbGetTypes combo to "setType"
        Dim nPos As Integer = FindItemByStringName(cmbSetTypes, setType)
        cmbSetTypes.SelectedIndex = nPos

        ' Get the equivalent MSG_SETxxxx type matching the one passed in 
        Dim nID As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetConstantFromTwainName(setType)

        ' Choose the container type for the capability 
        Dim bestContainer As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetCapContainer(m_Source, capToTestAsInt, nID)
        If bestContainer = 0 Then
            cmbContainerSet.SelectedIndex = 0
        Else
            ' Could have multiple "Set" TWAIN container types, so choose one
            Dim sBestContainer As String = GetBestMatchingContainer(bestContainer)

            nPos = FindItemByStringName(cmbContainerSet, sBestContainer)
            If nPos = -1 Then
                cmbContainerSet.SelectedIndex = 0
            Else
                cmbContainerSet.SelectedIndex = nPos
            End If
        End If

        ' Get the data type 
        Dim bestDataType As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetCapDataType(m_Source, capToTestAsInt)
        Dim szBestDataType As StringBuilder = New StringBuilder(100)

        ' Position cmbDataTypeSet to the data type
        VB_FullDemo.DTWAINAPI.DTWAIN_GetTwainNameFromConstant(DTWAINAPI.DTWAIN_CONSTANT_TWTY, bestDataType, szBestDataType, 100)
        nPos = FindItemByStringName(cmbDataTypeSet, szBestDataType.ToString())
        If nPos = -1 Then
            cmbDataTypeSet.SelectedIndex = 0
        Else
            cmbDataTypeSet.SelectedIndex = nPos
        End If
        Return capOpts
    End Function

    Private Sub TestSetCap()
        lstResultsSet.Items.Clear()
        ' Get the set type
        Dim szSetType As String = cmbSetTypes.SelectedItem.ToString()
        Dim nSetType As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetConstantFromTwainName(szSetType)
        ' Get the container type 
        Dim nCurSel As Integer = cmbContainerSet.SelectedIndex
        Dim nContainerType As Integer = allContainerTypesID(nCurSel)
        ' Get the data type 
        Dim szDataType As String = cmbDataTypeSet.SelectedItem.ToString()
        Dim nDataType As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetConstantFromTwainName(szDataType)
        ' Get the input 
        Dim szInput As String = editInputData.Text
        szInput.Replace("\r\n", " ")
        Dim aValues As System.IntPtr = IntPtr.Zero
        ' Parse the input, depending on the data type 
        Dim arrayType As Integer = DTWAINAPI.DTWAIN_ARRAYLONG
        Select Case nDataType
            Case DTWAINAPI.DTWAIN_TWTY_STR32,
                 DTWAINAPI.DTWAIN_TWTY_STR64,
                 DTWAINAPI.DTWAIN_TWTY_STR128,
                 DTWAINAPI.DTWAIN_TWTY_STR255,
                 DTWAINAPI.DTWAIN_TWTY_STR1024
                arrayType = DTWAINAPI.DTWAIN_ARRAYANSISTRING

            Case DTWAINAPI.DTWAIN_TWTY_FIX32
                arrayType = DTWAINAPI.DTWAIN_ARRAYFLOAT

            Case DTWAINAPI.DTWAIN_TWTY_FRAME
                arrayType = DTWAINAPI.DTWAIN_ARRAYFRAME
        End Select

        If String.IsNullOrEmpty(szInput) Then
            szInput = " "
        End If

        Dim items As String() = szInput.Split(" ")
        If arrayType = DTWAINAPI.DTWAIN_ARRAYFRAME Then
            Dim frameValues As Double() = {0, 0, 0, 0}
            For i As Integer = 0 To items.Length - 1
                frameValues(i) = SafeToDouble(items(i))
            Next i
            aValues = VB_FullDemo.DTWAINAPI.DTWAIN_FrameCreate(frameValues(0), frameValues(1), frameValues(2),
                                                               frameValues(3))
        Else
            aValues = VB_FullDemo.DTWAINAPI.DTWAIN_ArrayCreate(arrayType, 0)
            If arrayType = DTWAINAPI.DTWAIN_ARRAYANSISTRING Then
                VB_FullDemo.DTWAINAPI.DTWAIN_ArrayAddANSIString(aValues, szInput)
            Else
                Select Case nDataType
                    Case DTWAINAPI.DTWAIN_TWTY_BOOL
                        Dim value As Integer = BoolStringToInt(items(0))
                        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayAddLong(aValues, value)

                    Case DTWAINAPI.DTWAIN_TWTY_INT8,
                        DTWAINAPI.DTWAIN_TWTY_INT16,
                        DTWAINAPI.DTWAIN_TWTY_INT32,
                        DTWAINAPI.DTWAIN_TWTY_UINT8,
                        DTWAINAPI.DTWAIN_TWTY_UINT16,
                        DTWAINAPI.DTWAIN_TWTY_UINT32
                        ' First see if the string Is a TWAIN known value
                        Dim value As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetConstantFromTwainName(items(0))
                        If value <> Integer.MinValue Then
                            VB_FullDemo.DTWAINAPI.DTWAIN_ArrayAddLong(aValues, value)
                        ElseIf String.IsNullOrWhiteSpace(items(0)) Then
                            VB_FullDemo.DTWAINAPI.DTWAIN_ArrayAddLong(aValues, 0)
                        Else
                            VB_FullDemo.DTWAINAPI.DTWAIN_ArrayAddLong(aValues, SafeToInt32(items(0)))
                        End If
                    Case DTWAINAPI.DTWAIN_TWTY_FIX32
                        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayAddFloat(aValues, SafeToDouble(items(0)))
                End Select
            End If
        End If
        ' Call the capability function 
        Dim ret As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_SetCapValuesEx2(m_Source, capToTestAsInt, nSetType, nContainerType,
                                                    nDataType, aValues)
        Dim lastError As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetLastError()
        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayDestroy(aValues)
        If ret = 1 Then
            lstResultsSet.Items.Add("Ok")
        Else
            Dim szErr As StringBuilder = New StringBuilder(8192)
            VB_FullDemo.DTWAINAPI.DTWAIN_GetErrorString(lastError, szErr, 8192)
            lstResultsSet.Items.Add("Error")
            lstResultsSet.Items.Add(szErr.ToString())
        End If
    End Sub

    Private Sub btnStartTest_Click(sender As Object, e As EventArgs) Handles btnStartTest.Click
        TestGetCap()
    End Sub

    Private Sub TestGetCap()
        lstResults.Items.Clear()

        ' Get the get type, container, And data type 
        Dim szGetType As String = cmbGetTypes.SelectedItem.ToString()
        Dim nGetType As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetConstantFromTwainName(szGetType)

        ' Get the container type 
        Dim nCurSel As Integer = cmbContainer.SelectedIndex
        Dim nContainerType As Integer = allContainerTypesID(nCurSel)

        ' Get the data type 
        Dim szDataType As String = cmbDataType.SelectedItem.ToString()
        Dim nDataType As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetConstantFromTwainName(szDataType)

        ' Get the translation (if it exists) for the cap return values 
        Dim nTranslationID As Integer = -1
        Dim bGotID As Integer = 0
        Dim bTranslate As Integer = 1
        Dim bIsCapNameSupported As Boolean = (capToTestAsInt = DTWAINAPI.DTWAIN_CV_CAPSUPPORTEDCAPS) Or
                                             (capToTestAsInt = DTWAINAPI.DTWAIN_CV_CAPEXTENDEDCAPS) Or
                                             (capToTestAsInt = DTWAINAPI.DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE)
        Dim bIsCustomCap As Boolean = capToTestAsInt >= DTWAINAPI.DTWAIN_CV_CAPCUSTOMBASE
        If Not bIsCapNameSupported And Not bIsCustomCap Then
            ' Look in name mapping to see if the cap values do Not need translation 
            Dim nValue As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetTwainNameFromConstant(DTWAINAPI.DTWAIN_CONSTANT_CAPCODE_NOMNEMONIC,
                                                                    capToTestAsInt, Nothing, 0)
            If nValue = DTWAINAPI.DTWAIN_FAILURE1 Then
                bTranslate = 1
            Else
                bTranslate = 0
            End If
            If bTranslate = 1 Then
                Dim szID As StringBuilder = New StringBuilder(100)
                ' Get the TWAIN constant name mapping, given the capability value 
                nTranslationID = -1
                bGotID = VB_FullDemo.DTWAINAPI.DTWAIN_GetTwainNameFromConstant(DTWAINAPI.DTWAIN_CONSTANT_CAPCODE_MAP, capToTestAsInt, szID, 100)
                If bGotID > 0 Then
                    nTranslationID = Convert.ToInt32(szID.ToString())
                End If

                ' If the name Is equal to the cap value, then the ID was really Not found 
                If nTranslationID <> capToTestAsInt Then
                    bGotID = 1
                Else
                    bGotID = 0
                End If
            End If
        End If

        ' Call the capability function 
        Dim values As System.IntPtr = IntPtr.Zero
        Dim ret As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_GetCapValuesEx2(m_Source, capToTestAsInt, nGetType, nContainerType, nDataType, values)
        If ret = 1 Then
            lblTestGetResults.Text = "Success"
            Dim szValues As StringBuilder = New StringBuilder(1024)

            ' Display the results in the list box 
            Dim numItems As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetCount(values)
            Dim nArrayType As Integer = VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetType(values)
            For i As Integer = 0 To numItems - 1
                szValues.Clear()
                If i >= 1000 Then
                    lstResults.Items.Add("~ Number of values exceeded 1000 ... ~")
                    Exit For
                End If

                Select Case nArrayType
                    ' Display long values.  This includes boolean TRUE And FALSE
                    Case DTWAINAPI.DTWAIN_ARRAYLONG
                        Dim lVal As Integer = 0
                        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetAtLong(values, i, lVal)
                        ' The CAP_SUPPORTEDDATS Is special in that the values
                        ' represent DG And DAT names, where the DG Is in the hi-word
                        '' of the value, And the DAT Is the low-word of the value.
                        If capToTestAsInt = DTWAINAPI.DTWAIN_CV_CAPSUPPORTEDDATS Then
                            Dim hiWord As Integer = lVal >> 16
                            Dim loWord As Integer = lVal And &HFFFF
                            Dim szTemp As StringBuilder = New StringBuilder(30)
                            Dim szTemp2 As StringBuilder = New StringBuilder(30)
                            VB_FullDemo.DTWAINAPI.DTWAIN_GetTwainNameFromConstant(DTWAINAPI.DTWAIN_CONSTANT_DG, hiWord, szTemp, 30)
                            VB_FullDemo.DTWAINAPI.DTWAIN_GetTwainNameFromConstant(DTWAINAPI.DTWAIN_CONSTANT_DAT, loWord, szTemp2, 30)
                            szValues.AppendFormat("{0} / {1}", szTemp.ToString(), szTemp2.ToString())
                        Else
                            If bIsCapNameSupported Then
                                VB_FullDemo.DTWAINAPI.DTWAIN_GetNameFromCap(lVal, szValues, 256)
                            ElseIf nDataType = DTWAINAPI.DTWAIN_TWTY_BOOL Then
                                If lVal = 1 Then
                                    szValues.AppendFormat("TRUE")
                                Else
                                    szValues.AppendFormat("FALSE")
                                End If
                            Else
                                ' This Is for the special ICAP_SUPPORTEDCAPS, ICAP_EXTENDEDCAPS, And
                                ' ICAP_SUPPORTEDCAPSSEGMENTUNIQUE capabilities.  We display the name
                                ' Not the value
                                If bGotID = 1 Then
                                    VB_FullDemo.DTWAINAPI.DTWAIN_GetTwainNameFromConstant(nTranslationID, lVal, szValues, 256)
                                Else
                                    Dim sPrefix As String = ""
                                    ' Check if this Is a range.  If so, there will always be 5 values
                                    ' where item i Is either Minimum, Maximum, Step, Default, Or Current
                                    Dim isRange As Boolean = (cmbContainer.SelectedItem.ToString() = "TW_RANGE")
                                    If isRange Then
                                        sPrefix = allRangeNames(i)
                                    End If
                                    szValues.AppendFormat("{0}{1}", sPrefix, lVal)
                                End If
                            End If
                        End If
                        lstResults.Items.Add(szValues.ToString())

                    Case DTWAINAPI.DTWAIN_ARRAYFLOAT
                        Dim dVal As Double = 0
                        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetAtFloat(values, i, dVal)
                        Dim sPrefix As String = ""
                        ' Check if this Is a range.  If so, there will always be 5 values
                        ' where item i Is either Minimum, Maximum, Step, Default, Or Current
                        Dim isRange As Boolean = (cmbContainer.SelectedItem.ToString() = "TW_RANGE")
                        If isRange Then
                            sPrefix = allRangeNames(i)
                        End If
                        szValues.AppendFormat("{0}{1:F2}", sPrefix, dVal)
                        lstResults.Items.Add(szValues.ToString())


                    Case DTWAINAPI.DTWAIN_ARRAYANSISTRING
                        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetAtANSIString(values, i, szValues)
                        lstResults.Items.Add(szValues.ToString())

                    Case DTWAINAPI.DTWAIN_ARRAYFRAME
                        Dim Left, Top, Right, Bottom As Double
                        VB_FullDemo.DTWAINAPI.DTWAIN_ArrayGetAtFrame(values, i, Left, Top, Right, Bottom)
                        szValues.AppendFormat("Left: {0}  Top: {1}  Right: {2}  Bottom: {3}",
                                                Left, Top, Right, Bottom)
                        lstResults.Items.Add(szValues.ToString())
                End Select
            Next i
            VB_FullDemo.DTWAINAPI.DTWAIN_ArrayDestroy(values)
        Else
            lblTestGetResults.Text = "Error"
        End If
    End Sub

    Private Function SafeToDouble(s As String) As Double
        Dim v As Double
        Dim retValue As Boolean = Double.TryParse(s, NumberStyles.Float Or NumberStyles.AllowThousands, CultureInfo.InvariantCulture, v)
        If retValue Then
            Return v
        End If

        Return 0.0
    End Function

    Private Function SafeToInt32(s As String) As Integer
        Dim v As Integer
        Dim ret As Boolean = Integer.TryParse(s, v)
        If ret Then
            Return v
        End If
        Return 0
    End Function


    Private Function BoolStringToInt(value As String) As Integer
        If String.IsNullOrWhiteSpace(value) Then
            Return 0
        End If

        value = value.Trim()
        If value = "1" Then
            Return 1
        End If

        If value = "0" Then
            Return 0
        End If

        If value.Equals("TRUE", StringComparison.OrdinalIgnoreCase) Then
            Return 1
        End If

        If value.Equals("FALSE", StringComparison.OrdinalIgnoreCase) Then
            Return 0
        End If

        Return 0
    End Function

    Private Sub cmbGetTypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbGetTypes.SelectedIndexChanged
        SetTestSelection(cmbGetTypes.SelectedItem.ToString())
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        SetTestSelection("MSG_GET")
    End Sub

    Private Sub btnTestSet_Click(sender As Object, e As EventArgs) Handles btnTestSet.Click
        TestSetCap()
    End Sub

    Private Sub btnSetRevert_Click(sender As Object, e As EventArgs) Handles btnSetRevert.Click
        SetTestSelection2("MSG_SET")
    End Sub

    Private Sub cmbSetTypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSetTypes.SelectedIndexChanged
        btnTestSet.Enabled = True
        Dim capOpts As Integer
        capOpts = SetTestSelection2(cmbSetTypes.SelectedItem.ToString())
        ' This Is the MSG_RESET 
        If cmbSetTypes.SelectedIndex = 1 Then
            EnableSetCapWindows(False)
        Else
            EnableSetCapWindows(True)
        End If
        ' Now test the MSG_SETCONSTRAINT
        If cmbSetTypes.SelectedIndex = 2 Then
            If (capOpts And DTWAINAPI.DTWAIN_CO_SETCONSTRAINT) = 0 Then
                EnableSetCapWindows(False)
                btnTestSet.Enabled = False
            End If
        End If
    End Sub

    Private Sub EnableSetCapWindows(bEnable As Boolean)
        Dim controlsToToggle As Control() =
        {
            lblSetContainer,
            lblSetDataType,
            cmbContainerSet,
            cmbDataTypeSet,
            lblInput,
            lblResultsSet,
            editInputData,
            lstResultsSet
        }
        For Each ctrl As Control In controlsToToggle
            ctrl.Enabled = bEnable
        Next
    End Sub

    Private Sub TestCapDlg_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        VB_FullDemo.DTWAINAPI.DTWAIN_SetAllCapsToDefault(m_Source)
    End Sub
End Class
