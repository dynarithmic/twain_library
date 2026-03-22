#include <tchar.h>
#include <windowsx.h>
#include <stdio.h>
#include <string.h>
#include "dtwain.h"
#include "dtwdemo.h"
#include "stringutils.h"
#include "SourceProperties.h"

#ifdef _MSC_VER
#pragma warning (disable : 4996)
#endif

// This is the Resource ID for the "Error" string (see twainresourcestring_english.txt)
#define RESOURCE_ERROR_TEXT 3016

extern DTWAIN_SOURCE g_CurrentSource;
extern HINSTANCE g_hInstance;

static TCHAR* g_AllContainerTypes[] = { _T("TW_ARRAY"), _T("TW_ENUMERATION"), _T("TW_ONEVALUE"), _T("TW_RANGE") };
static LONG g_AllContainerTypesID[] = { DTWAIN_CONTARRAY, DTWAIN_CONTENUMERATION, DTWAIN_CONTONEVALUE, DTWAIN_CONTRANGE };
static TCHAR* g_AllGetTypes[] = { _T("MSG_GET"), _T("MSG_GETCURRENT"), _T("MSG_GETDEFAULT") };
static TCHAR* g_AllDataTypes[] = { _T("TWTY_INT8"), _T("TWTY_INT16"), _T("TWTY_INT32"), _T("TWTY_UINT8"),_T("TWTY_UINT16"),
                                _T("TWTY_UINT32"), _T("TWTY_BOOL"), _T("TWTY_FIX32"), _T("TWTY_FRAME"), _T("TWTY_STR32"),
                                _T("TWTY_STR64"), _T("TWTY_STR128"), _T("TWTY_STR255"), _T("TWTY_STR1024"), _T("TWTY_UNI512"),
                                _T("TWTY_HANDLE") };
static TCHAR* g_AllSetTypes[] = { _T("MSG_SET"), _T("MSG_RESET"), _T("MSG_SETCONSTRAINT") };
static char g_szInput[32767];

static LRESULT CALLBACK DisplayTestCapProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
static void EnableSetCapWindows(HWND hWnd, int bEnable);
static void SetTestSelection(HWND hWnd, TCHAR* getType, int capValue);
static LONG SetTestSelection2(HWND hWnd, TCHAR* setType, int capValue);
static void TestGetCap(HWND hWnd, LONG capValue);
static void TestSetCap(HWND hWnd, LONG capValue);
static LONG InitTestControls(HWND hWnd, const char* szName);
static void RefreshCustomDSData(HWND hWndDSData);

LRESULT CALLBACK DisplaySourcePropsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_INITDIALOG:
    {
        TCHAR szBuf[256];
        char szBufName[256];
        LONG nMajor, nMinor;
        DTWAIN_ARRAY CapArray = 0;
        LONG nCapCount;
        LONG nIndex;
        LONG nCapValue;
        HWND hWndName = GetDlgItem(hDlg, IDC_edProductName);
        HWND hWndFamily = GetDlgItem(hDlg, IDC_edFamilyName);
        HWND hWndManu = GetDlgItem(hDlg, IDC_edManufacturer);
        HWND hWndVerInfo = GetDlgItem(hDlg, IDC_edVersionInfo);
        HWND hWndVersion = GetDlgItem(hDlg, IDC_edVersion);
        HWND hWndCaps = GetDlgItem(hDlg, IDC_lstCapabilities);
        HWND hWndNumCaps = GetDlgItem(hDlg, IDC_edTotalCaps);
        HWND hWndNumCustomCaps = GetDlgItem(hDlg, IDC_edCustomCaps);
        HWND hWndNumExtendedCaps = GetDlgItem(hDlg, IDC_edExtendedCaps);
        HWND hWndDSData = GetDlgItem(hDlg, IDC_edDSData);
        HWND hWndJSONDetails = GetDlgItem(hDlg, IDC_edJSONDetails);
        HWND hWndShowUIOnly = GetDlgItem(hDlg, IDC_btnShowUIIOnly);
        int maxTextLength = 0;
        int curStringLength;
        HDC hdcList = GetDC(hWndCaps);
        SIZE textSize;
        DTWAIN_GetSourceProductNameA(g_CurrentSource, szBufName, 255);
        SetWindowTextA(hWndName, szBufName);

        DTWAIN_GetSourceProductFamily(g_CurrentSource, szBuf, 255);
        SetWindowText(hWndFamily, szBuf);

        DTWAIN_GetSourceManufacturer(g_CurrentSource, szBuf, 255);
        SetWindowText(hWndManu, szBuf);

        DTWAIN_GetSourceVersionInfo(g_CurrentSource, szBuf, 255);
        SetWindowText(hWndVerInfo, szBuf);

        DTWAIN_GetSourceVersionNumber(g_CurrentSource, &nMajor, &nMinor);
        wsprintf(szBuf, _T("%d.%d"), nMajor, nMinor);
        SetWindowText(hWndVersion, szBuf);

        CheckDlgButton(hDlg, IDC_chkResetCapsOnClose, BST_CHECKED);
        DTWAIN_EnumSupportedCaps(g_CurrentSource, &CapArray);
        nCapCount = DTWAIN_ArrayGetCount(CapArray);
        for (nIndex = 0; nIndex < nCapCount; nIndex++)
        {
            DTWAIN_ArrayGetAt(CapArray, nIndex, &nCapValue);
            DTWAIN_GetNameFromCap(nCapValue, szBuf, 255);
            SendMessage(hWndCaps, LB_ADDSTRING, 0, (LPARAM)szBuf);
            curStringLength = lstrlen(szBuf);
            GetTextExtentPoint32(hdcList, szBuf, curStringLength, &textSize);
            if (textSize.cx > maxTextLength)
                maxTextLength = textSize.cx;
        }
        ReleaseDC(hWndCaps, hdcList);
        SendMessage(hWndCaps, LB_SETHORIZONTALEXTENT, maxTextLength, 0);
        SendMessage(hWndCaps, LB_SETCURSEL, 0, 0);

        wsprintf(szBuf, _T("%d"), nCapCount);
        SetWindowText(hWndNumCaps, szBuf);

        DTWAIN_ARRAY testArray;
        DTWAIN_EnumCustomCaps(g_CurrentSource, &testArray);
        wsprintf(szBuf, _T("%d"), (int)DTWAIN_ArrayGetCount(testArray));
        SetWindowText(hWndNumCustomCaps, szBuf);
        DTWAIN_ArrayDestroy(testArray);

        DTWAIN_EnumExtendedCaps(g_CurrentSource, &CapArray);
        wsprintf(szBuf, _T("%d"), (int)DTWAIN_ArrayGetCount(CapArray));
        SetWindowText(hWndNumExtendedCaps, szBuf);

        RefreshCustomDSData(hWndDSData);

        /* Get JSON details of the Source */
        {
            LONG numChars = DTWAIN_GetSourceDetailsA(szBufName, NULL, 0, 2, TRUE);
            if (numChars > 0)
            {
                BYTE* szData = NULL;
                szData = malloc(numChars + 1);
                if (szData)
                {
                    /* Fill the memory with 0 */
                    memset(szData, 0, numChars + 1);
                    DTWAIN_GetSourceDetailsA(szBufName, szData, numChars, 2, FALSE);

                    /* Edit controls need \r\n and not \n new lines. */
                    HANDLE h = DTWAIN_ConvertToAPIStringA(szData);
                    if (h)
                    {
                        LPCSTR pData = GlobalLock(h);
                        SetWindowTextA(hWndJSONDetails, pData);
                        GlobalUnlock(h);
                        GlobalFree(h);
                    }
                    free(szData);
                }
            }
        }
        DTWAIN_ArrayDestroy(CapArray);
        // Get whether Source supports "Show UI Only"
        EnableWindow(hWndShowUIOnly, DTWAIN_IsUIOnlySupported(g_CurrentSource));
        return TRUE;
    }

    case WM_COMMAND:
    {
        int nControl = LOWORD(wParam);
        int nNotification = HIWORD(wParam);
        HWND hCheckBox = GetDlgItem(hDlg, IDC_chkResetCapsOnClose);
        switch (nControl)
        {
            /* Quit the dialog */
        case IDOK:
        case IDCANCEL:
            if (DTWAIN_IsSourceAcquiringEx(g_CurrentSource, TRUE))
            {
                MessageBoxA(NULL, "You must close the Source user interface before leaving this dialog", "Information", MB_ICONSTOP);
                return FALSE;
            }
            /* User may have done a lot of capability testing, 
            so make sure we reset all the caps to default when we return if requested */
            LRESULT checkState = SendMessage(hCheckBox, BM_GETCHECK, 0, 0);
            if (checkState == BST_CHECKED)
                DTWAIN_SetAllCapsToDefault(g_CurrentSource);
            EndDialog(hDlg, 1);
            break;
        case IDC_btnTestCap:
        {
            char szCap[100];
            LRESULT nCurSel = SendMessage(GetDlgItem(hDlg, IDC_lstCapabilities), LB_GETCURSEL, 0, 0);
            SendMessageA(GetDlgItem(hDlg, IDC_lstCapabilities), LB_GETTEXT, nCurSel, (LPARAM)szCap);
            DisplayTestCapDlg(hDlg, szCap);
        }
        break;
        case IDC_btnResetCapabilities:
            DTWAIN_SetAllCapsToDefault(g_CurrentSource);
            break;
        case IDC_btnShowUIIOnly:
        {
            HWND hWndShowUIOnly = GetDlgItem(hDlg, IDC_btnShowUIIOnly);
            HWND hWndDSData = GetDlgItem(hDlg, IDC_edDSData);
            EnableWindow(hWndShowUIOnly, FALSE);
            DTWAIN_ShowUIOnly(g_CurrentSource);
            EnableWindow(hWndShowUIOnly, TRUE);
            RefreshCustomDSData(hWndDSData);
        }
        break;
        case IDC_btnRefreshShowUIOnly:
        {
            HWND hWndDSData = GetDlgItem(hDlg, IDC_edDSData);
            RefreshCustomDSData(hWndDSData);
        }
        break;
        }
    }
    break;
    }
    return FALSE;
}

void DisplayTestCapDlg(HWND parent, const char* szCapName)
{
    int capValue = DTWAIN_GetCapFromNameA(szCapName);
    DialogBoxParam(g_hInstance, (LPCTSTR)IDD_dlgTestCap, parent, (DLGPROC)DisplayTestCapProc, (LPARAM)(capValue));
}

LRESULT CALLBACK DisplayTestCapProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    static LONG curCapValue;
    LONG capOpts = 0;
    switch (message)
    {
    case WM_INITDIALOG:
    {
        int capValue = (int)lParam;
        char szName[100];
        DTWAIN_GetNameFromCapA(capValue, szName, 100);
        char szTitle[256];
        strcpy(szTitle, "Test Capability (");
        strcat(szTitle, szName);
        strcat(szTitle, ")");
        SetWindowTextA(hDlg, szTitle);
        curCapValue = InitTestControls(hDlg, szName);
        return TRUE;
    }
    break;
    case WM_COMMAND:
    {
        int nControl = LOWORD(wParam);
        int nNotification = HIWORD(wParam);

        switch (nControl)
        {
        case IDC_cmbGetTypes:
        {
            if (nNotification == CBN_SELCHANGE)
            {
                TCHAR szGetType[100];
                LRESULT nCurSel = SendMessage(GetDlgItem(hDlg, IDC_cmbGetTypes), CB_GETCURSEL, 0, 0);
                SendMessage(GetDlgItem(hDlg, IDC_cmbGetTypes), CB_GETLBTEXT, nCurSel, (LPARAM)szGetType);
                SetTestSelection(hDlg, szGetType, curCapValue);
            }
        }
        break;

        case IDC_cmbSetTypes:
        {
            if (nNotification == CBN_SELCHANGE)
            {
                HWND hWndTestSet = GetDlgItem(hDlg, IDC_btnTestSet);
                EnableWindow(hWndTestSet, TRUE);
                TCHAR szGetType[100];
                /* This is the MSG_RESET */
                LRESULT nCurSel = SendMessage(GetDlgItem(hDlg, IDC_cmbSetTypes), CB_GETCURSEL, 0, 0);
                SendMessage(GetDlgItem(hDlg, IDC_cmbSetTypes), CB_GETLBTEXT, nCurSel, (LPARAM)szGetType);
                capOpts = SetTestSelection2(hDlg, szGetType, curCapValue);
                if (nCurSel == 1)
                {
                    EnableSetCapWindows(hDlg, FALSE);
                    break;
                }
                else
                    EnableSetCapWindows(hDlg, TRUE);

                /* Now test for MSG_SETCONSTRAINT */
                if (nCurSel == 2)
                {
                    if (!(capOpts & DTWAIN_CO_SETCONSTRAINT))
                    {
                        // Disable controls for constraint, including test button
                        EnableSetCapWindows(hDlg, FALSE);
                        EnableWindow(hWndTestSet, FALSE);
                    }
                }
            }
        }
        break;

        case IDC_btnTest:
            TestGetCap(hDlg, curCapValue);
            break;

        case IDC_btnReset:
        {
            /* Get the Get type*/
            TCHAR szGetType[100];
            LRESULT nCurSel = SendMessage(GetDlgItem(hDlg, IDC_cmbGetTypes), CB_GETCURSEL, 0, 0);
            SendMessage(GetDlgItem(hDlg, IDC_cmbGetTypes), CB_GETLBTEXT, nCurSel, (LPARAM)szGetType);
            SetTestSelection(hDlg, szGetType, curCapValue);
        }
        break;

        case IDC_btnResetSet:
        {
            /* Get the Get type*/
            TCHAR szSetType[100];
            LRESULT nCurSel = SendMessage(GetDlgItem(hDlg, IDC_cmbSetTypes), CB_GETCURSEL, 0, 0);
            SendMessage(GetDlgItem(hDlg, IDC_cmbSetTypes), CB_GETLBTEXT, nCurSel, (LPARAM)szSetType);
            SetTestSelection2(hDlg, szSetType, curCapValue);
        }
        break;

        case IDC_btnTestSet:
            TestSetCap(hDlg, curCapValue);
            break;

            /* Quit the dialog */
        case IDOK:
        {
            EndDialog(hDlg, 1);
        }
        break;
        case IDCANCEL:
            EndDialog(hDlg, LOWORD(wParam));
            return TRUE;
            break;
        }
    }
    break;
    }
    return FALSE;
}

void EnableSetCapWindows(HWND hWnd, int bEnable)
{
    HWND hWndStatic3 = GetDlgItem(hWnd, IDC_staticContainer);
    HWND hWndContainerTypesSet = GetDlgItem(hWnd, IDC_cmbContainerSet);
    HWND hWndStatic2 = GetDlgItem(hWnd, IDC_staticDataType);
    HWND hWndDataTypesSet = GetDlgItem(hWnd, IDC_cmbDataTypeSet);
    HWND hWndStatic5 = GetDlgItem(hWnd, IDC_staticInput);
    HWND hWndInput = GetDlgItem(hWnd, IDC_edSetInput);
    HWND hWndStatic4 = GetDlgItem(hWnd, IDC_staticResults);
    HWND hWndSetResults = GetDlgItem(hWnd, IDC_lstResultsSet);

    HWND allWindows[] = { hWndStatic3, hWndContainerTypesSet, hWndStatic2,
                          hWndDataTypesSet, hWndStatic5, hWndInput, hWndStatic4,
                          hWndSetResults };
    const int sz = sizeof(allWindows) / sizeof(allWindows[0]);
    int i = 0;
    for (i = 0; i < sz; ++i)
        EnableWindow(allWindows[i], bEnable);
}


LONG InitTestControls(HWND hWnd, const char* szName)
{
    HWND hWndGetTypes = GetDlgItem(hWnd, IDC_cmbGetTypes);
    HWND hWndContainerTypes = GetDlgItem(hWnd, IDC_cmbContainer);
    HWND hWndDataTypes = GetDlgItem(hWnd, IDC_cmbDataType);

    HWND hWndSetTypes = GetDlgItem(hWnd, IDC_cmbSetTypes);
    HWND hWndContainerTypesSet = GetDlgItem(hWnd, IDC_cmbContainerSet);
    HWND hWndDataTypesSet = GetDlgItem(hWnd, IDC_cmbDataTypeSet);
    HWND hWndInput = GetDlgItem(hWnd, IDC_edSetInput);
    HWND hWndTestSet = GetDlgItem(hWnd, IDC_btnTestSet);
    HWND hWndResetSet = GetDlgItem(hWnd, IDC_btnResetSet);
    HWND hWndSetResults = GetDlgItem(hWnd, IDC_lstResultsSet);

    LONG capValue = DTWAIN_GetCapFromNameA(szName);
    if (capValue == -1)
        return -1;

    int i = 0;
    int numGetTypes = sizeof(g_AllGetTypes) / sizeof(g_AllGetTypes[0]);
    for (i = 0; i < numGetTypes; ++i)
        SendMessage(hWndGetTypes, CB_ADDSTRING, 0, (LPARAM)g_AllGetTypes[i]);

    int numContainerTypes = sizeof(g_AllContainerTypes) / sizeof(g_AllContainerTypes[0]);
    for (i = 0; i < numContainerTypes; ++i)
        SendMessage(hWndContainerTypes, CB_ADDSTRING, 0, (LPARAM)g_AllContainerTypes[i]);

    int numDataTypes = sizeof(g_AllDataTypes) / sizeof(g_AllDataTypes[0]);
    for (i = 0; i < numDataTypes; ++i)
        SendMessage(hWndDataTypes, CB_ADDSTRING, 0, (LPARAM)g_AllDataTypes[i]);

    int numSetTypes = sizeof(g_AllSetTypes) / sizeof(g_AllSetTypes[0]);
    for (i = 0; i < numSetTypes; ++i)
        SendMessage(hWndSetTypes, CB_ADDSTRING, 0, (LPARAM)g_AllSetTypes[i]);

    for (i = 0; i < numContainerTypes; ++i)
        SendMessage(hWndContainerTypesSet, CB_ADDSTRING, 0, (LPARAM)g_AllContainerTypes[i]);

    for (i = 0; i < numDataTypes; ++i)
        SendMessage(hWndDataTypesSet, CB_ADDSTRING, 0, (LPARAM)g_AllDataTypes[i]);

    SetTestSelection(hWnd, _T("MSG_GET"), capValue);
    SetTestSelection2(hWnd, _T("MSG_SET"), capValue);
    return capValue;
}

void SetTestSelection(HWND hWnd, TCHAR* getType, int capValue)
{
    HWND hWndGetTypes = GetDlgItem(hWnd, IDC_cmbGetTypes);
    HWND hWndContainerTypes = GetDlgItem(hWnd, IDC_cmbContainer);
    HWND hWndDataTypes = GetDlgItem(hWnd, IDC_cmbDataType);

    int nPos = ComboBox_FindString(hWndGetTypes, -1, getType);
    SendMessage(hWndGetTypes, CB_SETCURSEL, nPos, 0);

    /* Get the equivalent MSG_GET type matching the one passed in */
    LONG nID = DTWAIN_GetConstantFromTwainName(getType);

    /* Choose the best container type for the capability */
    LONG bestContainer = DTWAIN_GetCapContainer(g_CurrentSource, capValue, nID);

    TCHAR szBestContainer[100];
    DTWAIN_GetTwainNameFromConstant(DTWAIN_CONSTANT_DTWAINCONT_TWAINCONT, bestContainer, szBestContainer, 100);

    nPos = ComboBox_FindString(hWndContainerTypes, -1, szBestContainer);
    if (nPos != CB_ERR)
        SendMessage(hWndContainerTypes, CB_SETCURSEL, nPos, 0);

    /* Choose the data type */
    LONG bestDataType = DTWAIN_GetCapDataType(g_CurrentSource, capValue);

    TCHAR szBestDataType[100];
    DTWAIN_GetTwainNameFromConstant(DTWAIN_CONSTANT_TWTY, bestDataType, szBestDataType, 100);

    nPos = ComboBox_FindString(hWndDataTypes, -1, szBestDataType);
    if (nPos != CB_ERR)
        SendMessage(hWndDataTypes, CB_SETCURSEL, nPos, 0);
}

LONG SetTestSelection2(HWND hWnd, TCHAR* setType, int capValue)
{
    HWND hWndSetTypes = GetDlgItem(hWnd, IDC_cmbSetTypes);
    HWND hWndContainerTypesSet = GetDlgItem(hWnd, IDC_cmbContainerSet);
    HWND hWndDataTypesSet = GetDlgItem(hWnd, IDC_cmbDataTypeSet);
    HWND hWndInput = GetDlgItem(hWnd, IDC_edSetInput);
    HWND hWndTestSet = GetDlgItem(hWnd, IDC_btnTestSet);
    HWND hWndResetSet = GetDlgItem(hWnd, IDC_btnResetSet);
    HWND hWndSetResults = GetDlgItem(hWnd, IDC_lstResultsSet);
    HWND hWndStatic1 = GetDlgItem(hWnd, IDC_staticSetOperation);
    HWND hWndStatic2 = GetDlgItem(hWnd, IDC_staticDataType);
    HWND hWndStatic3 = GetDlgItem(hWnd, IDC_staticContainer);
    HWND hWndStatic4 = GetDlgItem(hWnd, IDC_staticResults);
    HWND hWndStatic5 = GetDlgItem(hWnd, IDC_staticInput);

    SendMessage(hWndSetResults, LB_SETHORIZONTALEXTENT, 1000, 0);

    HWND allWindows[] = {
            hWndSetTypes, hWndContainerTypesSet, hWndDataTypesSet, hWndInput,hWndTestSet,
            hWndResetSet, hWndSetResults, hWndStatic1 , hWndStatic2 , hWndStatic3 , hWndStatic4, hWndStatic5 };

    int nPos = ComboBox_FindString(hWndSetTypes, -1, setType);
    SendMessage(hWndSetTypes, CB_SETCURSEL, nPos, 0);

    /* Get the equivalent MSG_SET type matching the one passed in */
    LONG nID = DTWAIN_GetConstantFromTwainName(setType);

    /* Choose the best container type for the capability */
    LONG bestContainer = DTWAIN_GetCapContainer(g_CurrentSource, capValue, nID);

    TCHAR szBestContainer[100];
    DTWAIN_GetTwainNameFromConstant(DTWAIN_CONSTANT_DTWAINCONT_TWAINCONT, bestContainer, szBestContainer, 100);

    nPos = ComboBox_FindString(hWndContainerTypesSet, -1, szBestContainer);
    if (nPos != CB_ERR)
        SendMessage(hWndContainerTypesSet, CB_SETCURSEL, nPos, 0);
    else
        SendMessage(hWndContainerTypesSet, CB_SETCURSEL, 0, 0);

    /* Choose the data type */
    LONG bestDataType = DTWAIN_GetCapDataType(g_CurrentSource, capValue);

    TCHAR szBestDataType[100];
    DTWAIN_GetTwainNameFromConstant(DTWAIN_CONSTANT_TWTY, bestDataType, szBestDataType, 100);

    nPos = ComboBox_FindString(hWndDataTypesSet, -1, szBestDataType);
    if (nPos != CB_ERR)
        SendMessage(hWndDataTypesSet, CB_SETCURSEL, nPos, 0);

    LONG capOpts = 0;
    /* Determine if setting the capability values for this cap is supported */
    if (DTWAIN_GetCapOperations(g_CurrentSource, capValue, &capOpts))
    {
        /* Turn off "Set" controls if the capability does not support
           the set operation */
        if (!(capOpts & DTWAIN_CO_SET))
        {
            const int numWindows = sizeof(allWindows) / sizeof(allWindows[0]);
            int i = 0;
            for (i = 0; i < numWindows; ++i)
                EnableWindow(allWindows[i], FALSE);
        }
    }
    return capOpts;
}

void TestGetCap(HWND hWnd, LONG capValue)
{
    HWND hWndGetTypes = GetDlgItem(hWnd, IDC_cmbGetTypes);
    HWND hWndContainerTypes = GetDlgItem(hWnd, IDC_cmbContainer);
    HWND hWndDataTypes = GetDlgItem(hWnd, IDC_cmbDataType);
    HWND hWndResults = GetDlgItem(hWnd, IDC_lstResults);
    HWND hWndStaticResults = GetDlgItem(hWnd, IDC_staticTestGetResults);

    SendMessage(hWndResults, LB_RESETCONTENT, 0, 0);

    /* Get the get type, container, and data type */
    TCHAR szGetType[100];
    LRESULT nCurSel = SendMessage(hWndGetTypes, CB_GETCURSEL, 0, 0);
    SendMessage(hWndGetTypes, CB_GETLBTEXT, nCurSel, (LPARAM)szGetType);
    LONG nGetType = DTWAIN_GetConstantFromTwainName(szGetType);

    /* Get the container type */
    nCurSel = SendMessage(hWndContainerTypes, CB_GETCURSEL, 0, 0);
    LONG nContainerType = g_AllContainerTypesID[nCurSel];

    /* Get the data type */
    TCHAR szDataType[100];
    nCurSel = SendMessage(hWndDataTypes, CB_GETCURSEL, 0, 0);
    SendMessage(hWndDataTypes, CB_GETLBTEXT, nCurSel, (LPARAM)szDataType);
    LONG nDataType = DTWAIN_GetConstantFromTwainName(szDataType);

    /* Get the translation (if it exists) for the cap return values */
    LONG nTranslationID = -1;
    BOOL bGotID = FALSE;
    BOOL bTranslate = TRUE;
    BOOL bIsCapNameSupported =
        (capValue == CAP_SUPPORTEDCAPS ||
            capValue == CAP_EXTENDEDCAPS ||
            capValue == CAP_SUPPORTEDCAPSSEGMENTUNIQUE);
    BOOL bIsCustomCap = (capValue >= CAP_CUSTOMBASE);
    if (!bIsCapNameSupported && !bIsCustomCap)
    {
        /* Look in name mapping to see if the cap values do not need translation */
        bTranslate =
            (DTWAIN_GetTwainNameFromConstantA(DTWAIN_CONSTANT_CAPCODE_NOMNEMONIC, capValue, NULL, 0) == DTWAIN_FAILURE1);
        if (bTranslate)
        {
            /* Get the TWAIN constant name mapping, given the capability value */
            char szTranslationID[100];
            nTranslationID = -1;

            bGotID = DTWAIN_GetTwainNameFromConstantA(DTWAIN_CONSTANT_CAPCODE_MAP, capValue, szTranslationID, 100);
            if (bGotID)
                nTranslationID = atoi(szTranslationID);

            /* If the name is equal to the cap value, then the ID was really not found */
            bGotID = (nTranslationID != capValue);
        }
    }

    /* Choose the data type */
    LONG bestDataType = DTWAIN_GetCapDataType(g_CurrentSource, capValue);

    /* Call the capability function */
    DTWAIN_ARRAY values;
    LONG ret = DTWAIN_GetCapValuesEx2(g_CurrentSource, capValue, nGetType, nContainerType, nDataType, &values);
    if (ret)
    {
        SendMessageA(hWndStaticResults, WM_SETTEXT, 0, (LPARAM)"Success");
        char szValues[1024];
        /* Display the results in the list box */
        LONG numItems = DTWAIN_ArrayGetCount(values);
        LONG nArrayType = DTWAIN_ArrayGetType(values);
        const char* rangeName[] = { "Minimum", "Maximum", "Step", "Default", "Current" };
        LONG i = 0;
        for (i = 0; i < numItems; ++i)
        {
            BOOL bGotValue = TRUE;
            if (i >= 1000)
            {
                SendMessageA(hWndResults, LB_ADDSTRING, 0, (LPARAM)"~ Number of values exceeded 1000 ... ~");
                break;
            }
            switch (nArrayType)
            {
                case DTWAIN_ARRAYLONG:
                {
                    LONG lVal;
                    BOOL isRange = (nContainerType == DTWAIN_CONTRANGE);
                    const char* rangeFormatU = "%s%u";
                    const char* rangeFormatD = "%s%d";
                    const char* rangeNameToPrint = "";
                    if (isRange)
                    {
                        rangeFormatU = "%s: %u";
                        rangeFormatD = "%s: %d";
                        rangeNameToPrint = rangeName[i];
                    }

                    DTWAIN_ArrayGetAtLong(values, i, &lVal);
                    if (capValue == CAP_SUPPORTEDDATS)
                    {
                        char szHi[100];
                        char szLo[100];
                        int hiWord = lVal >> 16;
                        int loWord = lVal & 0x0000FFFF;
                        DTWAIN_GetTwainNameFromConstantExA(DTWAIN_CONSTANT_DG, hiWord, szHi, 100);
                        DTWAIN_GetTwainNameFromConstantExA(DTWAIN_CONSTANT_DAT, loWord, szLo, 100);
                        sprintf(szValues, "%s / %s", szHi, szLo);
                    }
                    else
                    if (bIsCapNameSupported)
                        DTWAIN_GetNameFromCapA(lVal, szValues, 256);
                    else
                    if (nDataType == TWTY_BOOL)
                        sprintf(szValues, "%s", lVal == 1 ? "TRUE" : "FALSE");
                    else
                    if (bGotID)
                    {
                        char szTempBuf[100];
                        if (nDataType == TWTY_INT32 || nDataType == TWTY_INT16)
                            sprintf(szTempBuf, "%d", lVal);
                        else
                            sprintf(szTempBuf, "%u", lVal);
                        DTWAIN_GetTwainNameFromConstantExA(nTranslationID, lVal, szValues, 256);

                        // Name does not really exist
                        if (strcmp(szTempBuf, szValues) == 0)
                        {
                            bGotValue = FALSE;
                            szValues[0] = 0;
                        }
                    }

                    if (!bGotValue && szValues[0] == 0)
                    {
                        if (nDataType == TWTY_UINT32)
                            sprintf(szValues, rangeFormatU, rangeNameToPrint, (TW_UINT32)lVal);
                        else
                            sprintf(szValues, rangeFormatD, rangeNameToPrint, lVal);
                    }
                    SendMessageA(hWndResults, LB_ADDSTRING, 0, (LPARAM)szValues);
                }
                break;

                case DTWAIN_ARRAYFLOAT:
                {
                    double dVal;
                    DTWAIN_ArrayGetAtFloat(values, i, &dVal);
                    if (nContainerType == DTWAIN_CONTRANGE)
                        sprintf(szValues, "%s: %lf", rangeName[i], dVal);
                    else
                        sprintf(szValues, "%lf", dVal);
                    SendMessageA(hWndResults, LB_ADDSTRING, 0, (LPARAM)szValues);
                }
                break;

                case DTWAIN_ARRAYANSISTRING:
                {
                    DTWAIN_ArrayGetAtANSIString(values, i, szValues);
                    SendMessageA(hWndResults, LB_ADDSTRING, 0, (LPARAM)szValues);
                }
                break;

                case DTWAIN_ARRAYFRAME:
                {
                    double left, top, right, bottom;
                    DTWAIN_ArrayGetAtFrame(values, i, &left, &top, &right, &bottom);
                    sprintf(szValues, "Left: %lf  Top: %lf  Right: %lf  Bottom: %lf", left, top, right, bottom);
                    SendMessageA(hWndResults, LB_ADDSTRING, 0, (LPARAM)szValues);
                }
                break;
            }
        }
        DTWAIN_ArrayDestroy(values);
    }
    else
    {
        SendMessageA(hWndStaticResults, WM_SETTEXT, 0, (LPARAM)"Error");
    }
}

typedef struct ParserInfo
{
    DTWAIN_ARRAY theArray;
    int dataType;
    int counter;
} PARSERINFO;

int ParseTokenCallback(const char* line, void* userData)
{
    PARSERINFO* theInfo = (PARSERINFO*)userData;
    switch (theInfo->dataType)
    {
    case TWTY_BOOL:
    {
        int value = StringToBoolInt(line);
        DTWAIN_ArrayAddLong(theInfo->theArray, value);
    }
    break;
    case TWTY_INT8:
    case TWTY_INT16:
    case TWTY_INT32:
    {
        // First see if the string is a TWAIN known value
        int value = DTWAIN_GetConstantFromTwainNameA(line);
        if (value != INT_MIN)
            DTWAIN_ArrayAddLong(theInfo->theArray, value);
        else
        {
            int value = 0;
            char* end;
            value = strtol(line, &end, 10);
            DTWAIN_ArrayAddLong(theInfo->theArray, value);
        }
    }
    break;

    case TWTY_UINT8:
    case TWTY_UINT16:
    case TWTY_UINT32:
    {
        // First see if the string is a TWAIN known value
        int value = DTWAIN_GetConstantFromTwainNameA(line);
        if (value != INT_MIN)
            DTWAIN_ArrayAddLong(theInfo->theArray, value);
        else
        {
            int value = 0;
            char* end;
            value = strtoul(line, &end, 10);
            DTWAIN_ArrayAddLong(theInfo->theArray, value);
        }
    }
    break;

    case TWTY_STR32:
    case TWTY_STR64:
    case TWTY_STR128:
    case TWTY_STR255:
    case TWTY_STR1024:
    {
        DTWAIN_ArrayAddANSIString(theInfo->theArray, line);
    }
    break;

    case TWTY_FIX32:
    {
        double value = 0;
        char* end;
        value = strtod(line, &end);
        DTWAIN_ArrayAddFloat(theInfo->theArray, value);
    }
    break;

    case TWTY_FRAME:
    {
        double value = 0;
        char* end;
        value = strtod(line, &end);
        DTWAIN_FrameSetValue(theInfo->theArray, theInfo->counter, value);
        ++theInfo->counter;
        if (theInfo->counter == 4)
            return 0;
    }
    break;
    }
    return 1;
}

void TestSetCap(HWND hWnd, LONG capValue)
{
    HWND hWndSetTypes = GetDlgItem(hWnd, IDC_cmbSetTypes);
    HWND hWndContainerTypes = GetDlgItem(hWnd, IDC_cmbContainerSet);
    HWND hWndDataTypes = GetDlgItem(hWnd, IDC_cmbDataTypeSet);
    HWND hWndResults = GetDlgItem(hWnd, IDC_lstResultsSet);
    HWND hWndInput = GetDlgItem(hWnd, IDC_edSetInput);

    SendMessage(hWndResults, LB_RESETCONTENT, 0, 0);

    /* Get the set type, container, and data type */
    TCHAR szSetType[100];
    LRESULT nCurSel = SendMessage(hWndSetTypes, CB_GETCURSEL, 0, 0);
    SendMessage(hWndSetTypes, CB_GETLBTEXT, nCurSel, (LPARAM)szSetType);
    LONG nSetType = DTWAIN_GetConstantFromTwainName(szSetType);

    /* Get the container type */
    nCurSel = SendMessage(hWndContainerTypes, CB_GETCURSEL, 0, 0);
    LONG nContainerType = g_AllContainerTypesID[nCurSel];

    /* Get the data type */
    TCHAR szDataType[100];
    nCurSel = SendMessage(hWndDataTypes, CB_GETCURSEL, 0, 0);
    SendMessage(hWndDataTypes, CB_GETLBTEXT, nCurSel, (LPARAM)szDataType);
    LONG nDataType = DTWAIN_GetConstantFromTwainName(szDataType);

    /* Get the input */
    GetWindowTextA(hWndInput, g_szInput, 32767);

    DTWAIN_ARRAY aValues = 0;
    /* Parse the input, depending on the data type */
    int arrayType = DTWAIN_ARRAYLONG;
    switch (nDataType)
    {
    case TWTY_STR32:
    case TWTY_STR64:
    case TWTY_STR128:
    case TWTY_STR255:
    case TWTY_STR1024:
    {
        arrayType = DTWAIN_ARRAYANSISTRING;
    }
    break;

    case TWTY_FIX32:
    {
        arrayType = DTWAIN_ARRAYFLOAT;
    }
    break;

    case TWTY_FRAME:
    {
        arrayType = DTWAIN_ARRAYFRAME;
    }
    break;
    }

    PARSERINFO pInfo;
    pInfo.counter = 0;
    if (arrayType == DTWAIN_ARRAYFRAME)
        aValues = DTWAIN_FrameCreate(0, 0, 0, 0);
    else
        aValues = DTWAIN_ArrayCreate(arrayType, 0);
    pInfo.dataType = nDataType;
    pInfo.theArray = aValues;
    ParseTextBySpaces(g_szInput, &ParseTokenCallback, arrayType == DTWAIN_ARRAYANSISTRING, &pInfo);

    /* Call the capability function */
    LONG ret = DTWAIN_SetCapValuesEx2(g_CurrentSource, capValue, nSetType, nContainerType, nDataType, aValues);
    LONG last_error = DTWAIN_GetLastError();
    DTWAIN_ArrayDestroy(aValues);

    if (ret)
        SendMessageA(hWndResults, LB_ADDSTRING, 0, (LPARAM)"Ok");
    else
    {
        /* Error occurred while setting the capability
         These messages assume that the error text and strings
         are UTF-8 converted to UTF-16 internally by DTWAIN when using
         the Unicode version of DTWAIN.  */
        wchar_t szErrMessage[8192];
        wchar_t szErrorText[100];

        /* Get the error from the DTWAIN_SetCapValues function.This is in UTF16 - format */
        DTWAIN_GetErrorStringW(last_error, szErrMessage, 8192);

        /* Get the resource string for the string "Error".This is in UTF16 - format */
        DTWAIN_GetResourceStringW(RESOURCE_ERROR_TEXT, szErrorText, 100);

        /* Display results */
        SendMessageW(hWndResults, LB_ADDSTRING, 0, (LPARAM)szErrorText);
        SendMessageW(hWndResults, LB_ADDSTRING, 0, (LPARAM)szErrMessage);
    }
}

void RefreshCustomDSData(HWND hWndDSData)
{
    SetWindowTextA(hWndDSData, "");
    BYTE* szData = NULL;
    LONG actualSize;
    /* First, get the size of the Source's custom DS data */
    HANDLE h = DTWAIN_GetCustomDSData(g_CurrentSource, NULL, 0, &actualSize, DTWAINGCD_COPYDATA);
    if (h)
    {
        /* Allocate memory for the data.  We add an extra byte,
           since the data is not guaranteed to be null-terminated */
        szData = malloc((actualSize + 1) * sizeof(BYTE));
        if (szData)
        {
            /* Fill the memory with 0 */
            memset(szData, 0, actualSize + 1);

            /* Second call actually gets the data */
            DTWAIN_GetCustomDSData(g_CurrentSource, szData, actualSize, &actualSize, DTWAINGCD_COPYDATA);
            SetWindowTextA(hWndDSData, szData);
            free(szData);
        }
    }
}