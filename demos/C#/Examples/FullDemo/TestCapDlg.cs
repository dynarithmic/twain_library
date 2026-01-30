using System;
using System.Text;
using System.Windows.Forms;
using Dynarithmic;
using DTWAIN_SOURCE = System.IntPtr;
using DTWAIN_ARRAY = System.IntPtr;
using System.Globalization;

namespace TWAINDemo
{
    public partial class TestCapDlg : Form
    {
        private DTWAIN_SOURCE m_Source;
        private string capToTest;
        private int capToTestAsInt;
        private static string[] allGetTypes = { "MSG_GET", "MSG_GETCURRENT", "MSG_GETDEFAULT" };
        private static string[] allContainerTypes = { "TW_ONEVALUE", "TW_ARRAY", "TW_ENUMERATION", "TW_RANGE" };
        private static int[] allContainerTypesID = { TwainAPI.DTWAIN_CONTONEVALUE, TwainAPI.DTWAIN_CONTARRAY, TwainAPI.DTWAIN_CONTENUMERATION,
                                                      TwainAPI.DTWAIN_CONTRANGE };
        private static string [] allDataTypes = { "TWTY_INT8", "TWTY_INT16", "TWTY_INT32", "TWTY_UINT8","TWTY_UINT16",
                            "TWTY_UINT32", "TWTY_BOOL", "TWTY_FIX32", "TWTY_FRAME", "TWTY_STR32",
                            "TWTY_STR64", "TWTY_STR128", "TWTY_STR255", "TWTY_STR1024", "TWTY_UNI512",
                            "TWTY_HANDLE" };
        private static string[] allRangeNames= { "Minimum: ", "Maximum: ", "Step: ", "Default: ", "Current: " };

        private static string[] allSetTypes = { "MSG_SET", "MSG_RESET", "MSG_SETCONSTRAINT" };

        public TestCapDlg()
        {
            InitializeComponent();
        }

        public TestCapDlg(DTWAIN_SOURCE theSource, string thecapToTest)
        {
            m_Source = theSource;
            capToTest = thecapToTest;
            capToTestAsInt = TwainAPI.DTWAIN_GetCapFromName(capToTest);
            InitializeComponent();
        }

        private void TestCapDlg_Load(object sender, EventArgs e)
        {
            Text = "Test Capability (" + capToTest + ")";
            InitTestControls();
        }

        private int FindItemByStringName(ComboBox comboBox, string itemName)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                // Cast the item to a string and compare
                if (comboBox.Items[i].ToString() == itemName)
                {
                    return i; // Return the index of the matching item
                }
            }
            return -1; // Item not found
        }

        private int BoolStringToInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            value = value.Trim();

            if (value == "1")
                return 1;

            if (value == "0")
                return 0;

            if (value.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                return 1;

            if (value.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
                return 0;

            return 0; // default/fallback
        }

        private void InitTestControls()
        {
            cmbGetTypes.Items.AddRange(allGetTypes);
            cmbContainer.Items.AddRange(allContainerTypes);
            cmbDataType.Items.AddRange(allDataTypes);
            cmbSetTypes.Items.AddRange(allSetTypes);
            cmbContainerSet.Items.AddRange(allContainerTypes);
            cmbDataTypeSet.Items.AddRange(allDataTypes);
            SetTestSelection("MSG_GET");
            SetTestSelection2("MSG_SET");
        }

        private void SetTestSelection(string getType)
        {
            // Position the cmbGetTypes combo to "getType"
            int nPos = FindItemByStringName(cmbGetTypes, getType);
            cmbGetTypes.SelectedIndex = nPos;

            // Get the equivalent MSG_GET type matching the one passed in */
            int nID = TwainAPI.DTWAIN_GetConstantFromTwainName(getType);

            // Choose the best container type for the capability 
            int bestContainer = TwainAPI.DTWAIN_GetCapContainer(m_Source, capToTestAsInt, nID);

            // Could be more than one container defined for this cap, so get the "best" one
            string sBestContainer = GetBestMatchingContainer(bestContainer);

            // Position the cmbContainer control to the name equal to sBestContainer
            nPos = FindItemByStringName(cmbContainer, sBestContainer);
            if (nPos != -1)
                cmbContainer.SelectedIndex = nPos;
            else
                cmbContainer.SelectedIndex = 0;

            // Get the data type 
            int bestDataType = TwainAPI.DTWAIN_GetCapDataType(m_Source, capToTestAsInt);

            // Get the name of the data type
            StringBuilder szBestDataType= new StringBuilder(100);
            TwainAPI.DTWAIN_GetTwainNameFromConstant(TwainAPI.DTWAIN_CONSTANT_TWTY, bestDataType, szBestDataType, 100);

            // Now position the cmbDataType combo to the name
            nPos = FindItemByStringName(cmbDataType, szBestDataType.ToString());
            if (nPos != -1)
                cmbDataType.SelectedIndex = nPos;
            else
                cmbDataType.SelectedIndex = 0;
        }

        private void SetTestSelection2(string setType)
        {
            Control[] controlsToToggle =
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
            };

            int capOpts = 0;
            // Determine if setting the capability values for this cap is supported 
            if (TwainAPI.DTWAIN_GetCapOperations(m_Source, capToTestAsInt, ref capOpts) == 1)
            {
                // Turn off "Set" controls if the capability does not support the set operation 
                if ((capOpts & TwainAPI.DTWAIN_CO_SET) == 0)
                {
                    foreach (var ctrl in controlsToToggle)
                    {
                        ctrl.Enabled = false;
                    }
                    return; // Nothing else to do, since this capability does not support having it set
                }
            }

            // Position the cmbGetTypes combo to "setType"
            int nPos = FindItemByStringName(cmbSetTypes, setType);
            cmbSetTypes.SelectedIndex = nPos;

            // Get the equivalent MSG_SETxxxx type matching the one passed in 
            int nID = TwainAPI.DTWAIN_GetConstantFromTwainName(setType);

            // Choose the container type for the capability 
            int bestContainer = TwainAPI.DTWAIN_GetCapContainer(m_Source, capToTestAsInt, nID);
            if (bestContainer == 0)
            {
                cmbContainerSet.SelectedIndex = 0;
            }
            else
            {
                // Could have multiple "Set" TWAIN container types, so choose one
                string sBestContainer = GetBestMatchingContainer(bestContainer);

                nPos = FindItemByStringName(cmbContainerSet, sBestContainer);
                if ( nPos == -1 )
                    cmbContainerSet.SelectedIndex = 0;
                else
                    cmbContainerSet.SelectedIndex = nPos;
            }

            // Get the data type 
            int bestDataType = TwainAPI.DTWAIN_GetCapDataType(m_Source, capToTestAsInt);
            StringBuilder szBestDataType = new StringBuilder(100);

            // Position cmbDataTypeSet to the data type
            TwainAPI.DTWAIN_GetTwainNameFromConstant(TwainAPI.DTWAIN_CONSTANT_TWTY, bestDataType, szBestDataType, 100);
            nPos = FindItemByStringName(cmbDataTypeSet, szBestDataType.ToString());
            if ( nPos == -1 )
                cmbDataTypeSet.SelectedIndex = 0;
            else
                cmbDataTypeSet.SelectedIndex = nPos;
        }

        private string GetBestMatchingContainer(int container)
        {
            for (int i = 0; i < allContainerTypesID.Length; ++i)
            {
                if ((container & allContainerTypesID[i]) != 0)
                    return allContainerTypes[i];
            }
            return "TW_ONEVALUE";
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            TestGetCap();
        }

        private void TestGetCap()
        {
            lstResults.Items.Clear();

            // Get the get type, container, and data type 
            string szGetType = cmbGetTypes.SelectedItem.ToString();
            int nGetType = TwainAPI.DTWAIN_GetConstantFromTwainName(szGetType);

            // Get the container type 
            int nCurSel = cmbContainer.SelectedIndex;
            int nContainerType = allContainerTypesID[nCurSel];

            // Get the data type 
            string szDataType = cmbDataType.SelectedItem.ToString();
            int nDataType = TwainAPI.DTWAIN_GetConstantFromTwainName(szDataType);

            // Get the translation (if it exists) for the cap return values 
            int nTranslationID = -1;
            int bGotID = 0;
            int bTranslate = 1;
            bool bIsCapNameSupported = (capToTestAsInt == TwainAPI.DTWAIN_CV_CAPSUPPORTEDCAPS ||
                                        capToTestAsInt == TwainAPI.DTWAIN_CV_CAPEXTENDEDCAPS ||
                                        capToTestAsInt == TwainAPI.DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE);
            bool bIsCustomCap = (capToTestAsInt >= TwainAPI.DTWAIN_CV_CAPCUSTOMBASE);
            if (!bIsCapNameSupported && !bIsCustomCap)
            {
                // Look in name mapping to see if the cap values do not need translation 
                int nValue = TwainAPI.DTWAIN_GetTwainNameFromConstant(TwainAPI.DTWAIN_CONSTANT_CAPCODE_NOMNEMONIC, 
                                                                      capToTestAsInt, IntPtr.Zero, 0);
                bTranslate = (nValue == TwainAPI.DTWAIN_FAILURE1 ? 1 : 0);
                if ( bTranslate == 1 )
                {
                    StringBuilder szID = new StringBuilder(100);
                    // Get the TWAIN constant name mapping, given the capability value 
                    nTranslationID = -1;
                    bGotID = TwainAPI.DTWAIN_GetTwainNameFromConstant(
                            TwainAPI.DTWAIN_CONSTANT_CAPCODE_MAP, capToTestAsInt, szID, 100);
                    if (bGotID > 0)
                        nTranslationID = Convert.ToInt32(szID.ToString());
                    // If the name is equal to the cap value, then the ID was really not found 
                    bGotID = (nTranslationID != capToTestAsInt ? 1:0);
                }
            }

            // Call the capability function 
            DTWAIN_ARRAY values = IntPtr.Zero;
            int ret = TwainAPI.DTWAIN_GetCapValuesEx2(m_Source, capToTestAsInt, nGetType, nContainerType, nDataType, 
                                                      ref values);
            if (ret == 1)
            {
                lblTestGetResults.Text = "Success";
                StringBuilder szValues = new StringBuilder(1024);

                // Display the results in the list box 
                int numItems = TwainAPI.DTWAIN_ArrayGetCount(values);
                int nArrayType = TwainAPI.DTWAIN_ArrayGetType(values);
                for (int i = 0; i < numItems; ++i)
                {
                    szValues.Clear();
                    if (i >= 1000)
                    {
                        lstResults.Items.Add("~ Number of values exceeded 1000 ... ~");
                        break;
                    }
                    switch (nArrayType)
                    {
                        // Display long values.  This includes boolean TRUE and FALSE
                        case TwainAPI.DTWAIN_ARRAYLONG:
                        {
                            int lVal = 0;
                            TwainAPI.DTWAIN_ArrayGetAtLong(values, i, ref lVal);
                            // The CAP_SUPPORTEDDATS is special in that the values
                            // represent DG and DAT names, where the DG is in the hi-word
                            // of the value, and the DAT is the low-word of the value.
                            if (capToTestAsInt == TwainAPI.DTWAIN_CV_CAPSUPPORTEDDATS)
                            {
                                int hiWord = lVal >> 16;
                                int loWord = lVal & 0x0000FFFF;
                                StringBuilder szTemp = new StringBuilder(30);
                                StringBuilder szTemp2 = new StringBuilder(30);
                                TwainAPI.DTWAIN_GetTwainNameFromConstant(TwainAPI.DTWAIN_CONSTANT_DG, hiWord, szTemp, 30);
                                TwainAPI.DTWAIN_GetTwainNameFromConstant(TwainAPI.DTWAIN_CONSTANT_DAT, loWord, szTemp2, 30);
                                szValues.AppendFormat("{0} / {1}", szTemp.ToString(), szTemp2.ToString());
                            }
                            else
                            if (bIsCapNameSupported)
                                TwainAPI.DTWAIN_GetNameFromCap(lVal, szValues, 256);
                            else
                            if (nDataType == TwainAPI.DTWAIN_TWTY_BOOL)
                                szValues.AppendFormat("{0}", lVal == 1 ? "TRUE" : "FALSE");
                            else
                            // This is for the special ICAP_SUPPORTEDCAPS, ICAP_EXTENDEDCAPS, and
                            // ICAP_SUPPORTEDCAPSSEGMENTUNIQUE capabilities.  We display the name
                            // not the value
                            if (bGotID == 1)
                                TwainAPI.DTWAIN_GetTwainNameFromConstant(nTranslationID, lVal, szValues, 256);
                            else
                            {
                                string sPrefix = "";
                                // Check if this is a range.  If so, there will always be 5 values
                                // where item i is either Minimum, Maximum, Step, Default, or Current
                                bool isRange = (cmbContainer.SelectedItem.ToString() == "TW_RANGE");
                                if (isRange)
                                    sPrefix = allRangeNames[i];
                                szValues.AppendFormat("{0}{1}", sPrefix, lVal);
                            }
                            lstResults.Items.Add(szValues.ToString());
                        }
                        break;

                        case TwainAPI.DTWAIN_ARRAYFLOAT:
                        {
                            double dVal = 0;
                            TwainAPI.DTWAIN_ArrayGetAtFloat(values, i, ref dVal);
                            string sPrefix = "";
                            // Check if this is a range.  If so, there will always be 5 values
                            // where item i is either Minimum, Maximum, Step, Default, or Current
                            bool isRange = (cmbContainer.SelectedItem.ToString() == "TW_RANGE");
                            if (isRange)
                                sPrefix = allRangeNames[i];
                            szValues.AppendFormat("{0}{1:F2}", sPrefix, dVal);
                            lstResults.Items.Add(szValues.ToString());
                        }
                        break;

                        case TwainAPI.DTWAIN_ARRAYANSISTRING:
                        {
                            TwainAPI.DTWAIN_ArrayGetAtANSIString(values, i, szValues);
                            lstResults.Items.Add(szValues.ToString());
                        }
                        break;

                        case TwainAPI.DTWAIN_ARRAYFRAME:
                        {
                            double left = 0, top = 0, right = 0, bottom = 0;
                            TwainAPI.DTWAIN_ArrayGetAtFrame(values, i, ref left, ref top, ref right, ref bottom);
                            szValues.AppendFormat("Left: {0}  Top: {1}  Right: {2}  Bottom: {3}",
                                                    left, top, right, bottom);
                            lstResults.Items.Add(szValues.ToString());
                        }
                        break;
                    }
                }
                TwainAPI.DTWAIN_ArrayDestroy(values);
            }
            else
                lblTestGetResults.Text = "Error";
        }
        private void TestSetCap()
        {
            lstResultsSet.Items.Clear();
            // Get the set type
            string szSetType = cmbSetTypes.SelectedItem.ToString();
            int nSetType = TwainAPI.DTWAIN_GetConstantFromTwainName(szSetType);
            // Get the container type 
            int nCurSel = cmbContainerSet.SelectedIndex;
            int nContainerType = allContainerTypesID[nCurSel];
            // Get the data type 
            string szDataType = cmbDataTypeSet.SelectedItem.ToString();
            int nDataType = TwainAPI.DTWAIN_GetConstantFromTwainName(szDataType);
            // Get the input 
            string szInput = editInputData.Text;
            szInput.Replace("\r\n", " ");
            DTWAIN_ARRAY aValues = IntPtr.Zero;
            // Parse the input, depending on the data type 
            int arrayType = TwainAPI.DTWAIN_ARRAYLONG;
            switch (nDataType)
            {
                case TwainAPI.DTWAIN_TWTY_STR32:
                case TwainAPI.DTWAIN_TWTY_STR64:
                case TwainAPI.DTWAIN_TWTY_STR128:
                case TwainAPI.DTWAIN_TWTY_STR255:
                case TwainAPI.DTWAIN_TWTY_STR1024:
                {
                    arrayType = TwainAPI.DTWAIN_ARRAYANSISTRING;
                }
                break;
                case TwainAPI.DTWAIN_TWTY_FIX32:
                {
                    arrayType = TwainAPI.DTWAIN_ARRAYFLOAT;
                }
                break;
                case TwainAPI.DTWAIN_TWTY_FRAME:
                {
                    arrayType = TwainAPI.DTWAIN_ARRAYFRAME;
                }
                break;
            }
            if (string.IsNullOrEmpty(szInput))
                szInput = " ";
            string[] items = szInput.Split(' ');
            if (arrayType == TwainAPI.DTWAIN_ARRAYFRAME)
            {
                double[] frameValues = { 0, 0, 0, 0 };
                for (int i = 0; i < items.Length; ++i)
                {
                    frameValues[i] = SafeToDouble(items[i]);
                }
                aValues = TwainAPI.DTWAIN_FrameCreate(frameValues[0], frameValues[1], frameValues[2], frameValues[3]);
            }
            else
            { 
                aValues = TwainAPI.DTWAIN_ArrayCreate(arrayType, 0);
                if ( arrayType == TwainAPI.DTWAIN_ARRAYANSISTRING )
                {
                    TwainAPI.DTWAIN_ArrayAddANSIString(aValues, szInput);
                }
                else
                {
                    switch(nDataType)
                    {
                        case TwainAPI.DTWAIN_TWTY_BOOL:
                        {
                            int value = BoolStringToInt(items[0]);
                            TwainAPI.DTWAIN_ArrayAddLong(aValues, value);
                        }
                        break;
                        case TwainAPI.DTWAIN_TWTY_INT8:
                        case TwainAPI.DTWAIN_TWTY_INT16:
                        case TwainAPI.DTWAIN_TWTY_INT32:
                        case TwainAPI.DTWAIN_TWTY_UINT8:
                        case TwainAPI.DTWAIN_TWTY_UINT16:
                        case TwainAPI.DTWAIN_TWTY_UINT32:
                        {
                            // First see if the string is a TWAIN known value
                            int value = TwainAPI.DTWAIN_GetConstantFromTwainName(items[0]);
                            if (value != int.MinValue)
                                TwainAPI.DTWAIN_ArrayAddLong(aValues, value);
                            else
                            {
                                if (string.IsNullOrWhiteSpace(items[0]))
                                    TwainAPI.DTWAIN_ArrayAddLong(aValues, 0);
                                else
                                    TwainAPI.DTWAIN_ArrayAddLong(aValues, SafeToInt32(items[0]));
                            }
                        }
                        break;
                        case TwainAPI.DTWAIN_TWTY_FIX32:
                            TwainAPI.DTWAIN_ArrayAddFloat(aValues, SafeToDouble(items[0]));
                        break;
                    }
                }
            }
            // Call the capability function 
            int ret = TwainAPI.DTWAIN_SetCapValuesEx2(m_Source, capToTestAsInt, nSetType, nContainerType, 
                                                      nDataType, aValues);
            int lastError = TwainAPI.DTWAIN_GetLastError();
            TwainAPI.DTWAIN_ArrayDestroy(aValues);
            if (ret == 1)
                lstResultsSet.Items.Add("Ok");
            else
            {
                StringBuilder szErr = new StringBuilder(8192);
                TwainAPI.DTWAIN_GetErrorString(lastError, szErr, 8192);
                lstResultsSet.Items.Add("Error");
                lstResultsSet.Items.Add(szErr.ToString());
            }
        }

        private void cmbGetTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTestSelection(cmbGetTypes.SelectedItem.ToString());
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SetTestSelection("MSG_GET");
        }
        private void btnTestSet_Click(object sender, EventArgs e)
        {
            TestSetCap();
        }
        private void btnSetRevert_Click(object sender, EventArgs e)
        {
            SetTestSelection2("MSG_SET");
        }
        private void cmbSetTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This is the MSG_RESET 
            if ( cmbSetTypes.SelectedIndex == 1 )
                EnableSetCapWindows(false);
            else
                EnableSetCapWindows(true);
        }
        private void EnableSetCapWindows(bool bEnable)
        {
            Control[] controlsToToggle =
            {
                lblSetContainer,
                lblSetDataType,
                cmbContainerSet, 
                cmbDataTypeSet,
                lblInput,
                lblResultsSet,
                editInputData,
                lstResultsSet
            };
            foreach (var ctrl in controlsToToggle)
            {
                ctrl.Enabled = bEnable;
            }
        }

        public static int SafeToInt32(string s)
        {
            return int.TryParse(s, out var v) ? v : 0;
        }

        public static double SafeToDouble(string s)
        {
            return double.TryParse(
                s,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture,
                out var v)
                ? v
                : 0.0;
        }
    }
}
