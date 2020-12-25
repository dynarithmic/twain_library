/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2020 Dynarithmic Software.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
    OF THIRD PARTY RIGHTS.
 */
#include "ctltwmgr.h"
#include "ctltmpl5.h"
#include "dtwain_resource_constants.h"
#include "sourceselectopts.h"
#include "errorcheck.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

LONG DLLENTRY_DEF DTWAIN_SetTwainDialogFont(HFONT font)
{
	LOG_FUNC_ENTRY_PARAMS((font))
	CTL_TwainDLLHandle::s_DialogFont = font;
	LOG_FUNC_EXIT_PARAMS(1)
	CATCH_BLOCK(0)
}

static LRESULT CALLBACK DisplayTwainDlgProc(HWND, UINT, WPARAM, LPARAM);
static CTL_StringType GetTwainDlgTextFromResource(int nID, size_t& status);
static void DisplayLocalString(HWND hWnd, int nID, int ResID);

typedef DTWAIN_SOURCE(*SourceFn)(const SourceSelectionOptions&);
static std::unordered_map<int, SourceFn> SourcefnMap = { { SELECTSOURCE, dynarithmic::DTWAIN_LLSelectSource },
{ SELECTDEFAULTSOURCE, dynarithmic::DTWAIN_LLSelectDefaultSource },
{ SELECTSOURCEBYNAME, dynarithmic::DTWAIN_LLSelectSourceByName },
{ SELECTSOURCE2, dynarithmic::DTWAIN_LLSelectSource2 } };

static LONG OpenSourceInternal(DTWAIN_SOURCE Source)
{
    auto* pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    auto p = static_cast<CTL_ITwainSource *>(Source);
    if (p)
        p->SetSelected(true);
    if (pHandle->m_bOpenSourceOnSelect)
    {
        DTWAIN_BOOL retval = DTWAIN_OpenSource(Source);
        if (retval != TRUE)
        {
            LONG err = DTWAIN_GetLastError();
            DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !retval; }, err, err, FUNC_MACRO);
        }
    }
    return DTWAIN_NO_ERROR;
}

static DTWAIN_SOURCE SelectAndOpenSource(const SourceSelectionOptions& opts)
{
    DTWAIN_SOURCE Source = SourceSelect(opts);
    if (Source)
    {
        LONG retVal = OpenSourceInternal(Source);
        if (retVal != DTWAIN_NO_ERROR)
            return NULL;
    }
    return Source;
}

DTWAIN_SOURCE DLLENTRY_DEF DTWAIN_SelectSource()
{
    LOG_FUNC_ENTRY_PARAMS(())
    DTWAIN_SOURCE Source = SelectAndOpenSource(SourceSelectionOptions());
    LOG_FUNC_EXIT_PARAMS(Source);
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_SOURCE DLLENTRY_DEF DTWAIN_SelectSource2(HWND hWndParent, LPCTSTR szTitle, LONG xPos, LONG yPos, LONG nOptions)
{
    LOG_FUNC_ENTRY_PARAMS((hWndParent, szTitle, xPos, yPos, nOptions))
    DTWAIN_SOURCE Source = SelectAndOpenSource(SourceSelectionOptions(SELECTSOURCE2, NULL, hWndParent, szTitle, 
                                               xPos, yPos, NULL, NULL, NULL, nOptions));
    LOG_FUNC_EXIT_PARAMS(Source);
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_SOURCE DLLENTRY_DEF DTWAIN_SelectSource2Ex(HWND hWndParent,
                                                  LPCTSTR szTitle,
                                                  LONG xPos,
                                                  LONG yPos,
                                                  LPCTSTR szIncludeFilter,
                                                  LPCTSTR szExcludeFilter,
                                                  LPCTSTR szNameMapping,
                                                  LONG nOptions)
{
    LOG_FUNC_ENTRY_PARAMS((hWndParent, szTitle, xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions))
    DTWAIN_SOURCE Source = SelectAndOpenSource(SourceSelectionOptions(SELECTSOURCE2, NULL, hWndParent, szTitle, 
                                               xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions));
    LOG_FUNC_EXIT_PARAMS(Source);
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_SOURCE DLLENTRY_DEF DTWAIN_SelectDefaultSource()
{
    LOG_FUNC_ENTRY_PARAMS(())
    DTWAIN_SOURCE Source = SelectAndOpenSource(SourceSelectionOptions(SELECTDEFAULTSOURCE));
    LOG_FUNC_EXIT_PARAMS(Source);
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_SOURCE DLLENTRY_DEF DTWAIN_SelectSourceByName(LPCTSTR szProduct)
{
    LOG_FUNC_ENTRY_PARAMS((szProduct))
    DTWAIN_SOURCE Source = SelectAndOpenSource(SourceSelectionOptions(SELECTSOURCEBYNAME, szProduct));
    LOG_FUNC_EXIT_PARAMS(Source);
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsSourceSelected(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, FALSE, FUNC_MACRO);
    CTL_ITwainSource *p = VerifySourceHandle(GetDTWAINHandle_Internal(), Source);
    DTWAIN_BOOL bRet = FALSE;
    if ( p )
        bRet = p->IsSelected();
    LOG_FUNC_EXIT_PARAMS(bRet);
    CATCH_BLOCK(FALSE)
}

DTWAIN_SOURCE dynarithmic::SourceSelect(const SourceSelectionOptions& options)
{
    LOG_FUNC_ENTRY_PARAMS((options))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);

    // Start a session if not already started by app
    if (!pHandle->m_bSessionAllocated)
    {
        if (!DTWAIN_StartTwainSession(NULL, NULL))
            LOG_FUNC_EXIT_PARAMS((DTWAIN_SOURCE)NULL)
    }

    // Do a minimal check for TWAIN here
    DTWAIN_SOURCE pSource = SourcefnMap[options.nWhich](options);

    if (!pSource)
        LOG_FUNC_EXIT_PARAMS((DTWAIN_SOURCE)NULL)

        // Open and close the source to initialize capability structure
        // Return a dead source.  This allows closing of the source without
        // destroying the source info
        CTL_ITwainSource *pRealSource = (CTL_ITwainSource *)pSource;
    DTWAIN_SOURCE pDead = NULL;

    DTWAIN_ARRAY pDTWAINArray = 0;
    bool bFound = false;

    if (!DTWAIN_EnumSources(&pDTWAINArray))
    {
            DTWAIN_EndTwainSession();
        LOG_FUNC_EXIT_PARAMS(NULL)
    }

    DTWAINArrayLL_RAII arr(pDTWAINArray);
    auto vSources = EnumeratorVectorPtr<CTL_ITwainSourcePtr>(pDTWAINArray);

    if (vSources)
    {
        CTL_StringType sName = pRealSource->GetProductName();
        auto pDeadIt = std::find_if(vSources->begin(), vSources->end(),
            [&](CTL_ITwainSource* pSource){ return pSource->GetProductName() == sName; });

        if (pDeadIt != vSources->end())
        {
            pDead = *pDeadIt;
            bFound = true;
        }
        if (bFound)
        {
            if (pSource && (pRealSource != pDead))
            {
                CTL_ITwainSession *pSession;
                pSession = CTL_TwainAppMgr::GetCurrentSession();
                pSession->DestroyOneSource(pRealSource);
            }
            LOG_FUNC_EXIT_PARAMS(pDead)
        }
    }
        DTWAIN_EndTwainSession();
    LOG_FUNC_EXIT_PARAMS((DTWAIN_SOURCE)NULL)
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_SOURCE dynarithmic::DTWAIN_LLSelectSource(const SourceSelectionOptions& /*opt*/)
{
    LOG_FUNC_ENTRY_PARAMS(())
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, NULL, FUNC_MACRO);
    // Select a source from the source dialog
    // Bring TWAIN window to top
    const CTL_ITwainSource *pSource = CTL_TwainAppMgr::SelectSourceDlg( pHandle->m_Session );
    // Check if a source was selected
    LOG_FUNC_EXIT_PARAMS((DTWAIN_SOURCE)pSource)
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_SOURCE dynarithmic::DTWAIN_LLSelectSource2(const SourceSelectionOptions& opts)
{
    #ifndef _WIN32
    return DTWAIN_LLSelectSource(opts);
    #else
    LOG_FUNC_ENTRY_PARAMS((opts))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    // Get the resource for the Twain dialog
    HGLOBAL hglb = LoadResource(CTL_TwainDLLHandle::s_DLLInstance,
                               (HRSRC)FindResource(CTL_TwainDLLHandle::s_DLLInstance,
                               MAKEINTRESOURCE(10000), RT_DIALOG));
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&]{ return !hglb;}, DTWAIN_ERR_NULL_WINDOW, NULL, FUNC_MACRO);

    LPDLGTEMPLATE lpTemplate = static_cast<LPDLGTEMPLATE>(LockResource(hglb));

    SelectStruct selectStruct;
    selectStruct.CS.xpos = opts.xPos;
    selectStruct.CS.ypos = opts.yPos;
    selectStruct.CS.nOptions = opts.nOptions;
    selectStruct.CS.hWndParent = opts.hWndParent;
    selectStruct.m_pbSourcesOnSelect = &pHandle->m_bOpenSourceOnSelect;
    selectStruct.nItems = 0;
    if ( opts.szTitle )
        selectStruct.CS.sTitle = opts.szTitle;
    else
    {
        size_t status = 0;
        CTL_TwainAppMgr::WriteLogInfo(_T("Retrieving TWAIN Dialog Resources...\n"));
        selectStruct.CS.sTitle = GetTwainDlgTextFromResource(IDS_SELECT_SOURCE_TEXT, status);
        CTL_TwainAppMgr::WriteLogInfo(_T("Retrieved TWAIN Dialog Resources successfully...\n"));
        if ( !status )
            selectStruct.CS.sTitle = _T("Select Source");
    }

    if (opts.szIncludeNames)
        StringWrapper::Tokenize(opts.szIncludeNames, _T("|"), selectStruct.CS.aIncludeNames);
    if (opts.szExcludeNames)
        StringWrapper::Tokenize(opts.szExcludeNames, _T("|"), selectStruct.CS.aExcludeNames);
    if (opts.szNameMapping)
    {
        std::vector<CTL_StringType> mapPairs;
        StringWrapper::Tokenize(opts.szNameMapping, _T("|"), mapPairs);
        for (auto& m : mapPairs)
        {
            std::vector<CTL_StringType> onePair;
            StringWrapper::Tokenize(m, _T("="), onePair);
            if (onePair.size() == 2)
                selectStruct.CS.mapNames.insert({ onePair.front(), onePair.back() });
        }
    }

    DTWAIN_SOURCE Source;
    CTL_TwainAppMgr::WriteLogInfo(_T("Displaying TWAIN Dialog...\n"));
    INT_PTR bRet = DialogBoxIndirectParam(CTL_TwainDLLHandle::s_DLLInstance, lpTemplate, opts.hWndParent,
                          reinterpret_cast<DLGPROC>(DisplayTwainDlgProc), reinterpret_cast<LPARAM>(&selectStruct));
    if ( bRet == -1 )
        LOG_FUNC_EXIT_PARAMS(NULL)

    // See if cancel was selected
    if (selectStruct.SourceName.empty() || selectStruct.nItems == 0 )
    {
        CTL_TwainAppMgr::SetError(DTWAIN_ERR_SOURCESELECTION_CANCELED);
        LOG_FUNC_EXIT_PARAMS(NULL)
    }

    // Could be a mapped name, so need to check
    CTL_StringType actualSourceName = selectStruct.SourceName;
    if (opts.nOptions & DTWAIN_DLG_USENAMEMAPPING)
    {
        auto iter = std::find_if(selectStruct.CS.mapNames.begin(), selectStruct.CS.mapNames.end(), 
                                 [&](auto& pr)
                                { return pr.second == actualSourceName;}
                                );
        if (iter != selectStruct.CS.mapNames.end())
            actualSourceName = iter->first;
    }
    Source = DTWAIN_SelectSourceByName(actualSourceName.c_str());

    // Set the default Source
    DTWAIN_SetDefaultSource(Source);

    LOG_FUNC_EXIT_PARAMS(Source)
    CATCH_BLOCK(DTWAIN_SOURCE(0))
    #endif
}

DTWAIN_SOURCE dynarithmic::DTWAIN_LLSelectSourceByName( const SourceSelectionOptions& opts)
{
    LOG_FUNC_ENTRY_PARAMS((opts))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, NULL, FUNC_MACRO);
    // Select a source from the source dialog
    const CTL_ITwainSource *pSource = CTL_TwainAppMgr::SelectSource( pHandle->m_Session, opts.szProduct);
    // Check if a source was selected
    LOG_FUNC_EXIT_PARAMS(static_cast<DTWAIN_SOURCE>(const_cast<CTL_ITwainSource *>(pSource)))
        CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_SOURCE dynarithmic::DTWAIN_LLSelectDefaultSource(const SourceSelectionOptions& /*opts*/)
{
    LOG_FUNC_ENTRY_PARAMS(())
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, NULL, FUNC_MACRO);
    const CTL_ITwainSource* pSource = CTL_TwainAppMgr::GetDefaultSource(pHandle->m_Session);
    DTWAIN_SOURCE Source = static_cast<DTWAIN_SOURCE>(const_cast<CTL_ITwainSource *>(pSource));
    LOG_FUNC_EXIT_PARAMS(Source)
    CATCH_BLOCK(DTWAIN_SOURCE(0))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetDefaultSource(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_ITwainSource *p = VerifySourceHandle(GetDTWAINHandle_Internal(), Source);
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    bool bRetval = false;
    if (p)
    {
        bRetval = CTL_TwainAppMgr::SetDefaultSource(pHandle->m_Session, p);
        LOG_FUNC_EXIT_PARAMS(bRetval)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

#ifdef _WIN32
/////////////////////////////////////////////////////////////////////////////////
/// DTWAIN TWAIN Dialog procedure
CTL_StringType GetTwainDlgTextFromResource(int nID, size_t& status)
{
        size_t resSize;
    status = 0;
    resSize = GetResourceString(nID, NULL, 0);
        if (resSize > 0)
        {
            std::vector<TCHAR> buffer(resSize);
        status = GetResourceString(nID, &buffer[0], (LONG)resSize);
            return &buffer[0];
        }
    return _T("");
}

static bool ByCX(const SIZE& sz1, const SIZE& sz2)
{ return sz1.cx > sz2.cx; }

static BOOL CALLBACK ChildEnumFontProc(HWND hWnd, LPARAM lParam)
{
	SendMessage(hWnd, WM_SETFONT, (WPARAM)lParam, 0);
	return TRUE;
}

static std::vector<CTL_StringType> AdjustSourceNames(std::vector<CTL_StringType> vSourceNames, CustomPlacement CS)
{
    if (vSourceNames.empty())
        return {};

    bool doExclude = (CS.nOptions & DTWAIN_DLG_USEEXCLUDENAMES)?true:false;
    bool doInclude = (CS.nOptions & DTWAIN_DLG_USEINCLUDENAMES)?true : false;
    bool doMapping = (CS.nOptions & DTWAIN_DLG_USENAMEMAPPING)?true : false;

    if (!doInclude && !doExclude && !doMapping)
        return vSourceNames;

    std::vector<CTL_StringType> vReturn = vSourceNames;

    for (auto& sName : vSourceNames)
        sName = StringWrapper::TrimAll(sName);

    if (doInclude)
    {
        for (auto& sName : CS.aIncludeNames)
            sName = StringWrapper::TrimAll(sName);
        std::vector<CTL_StringType> vReturn2;
        std::sort(vSourceNames.begin(), vSourceNames.end());
        std::sort(CS.aIncludeNames.begin(), CS.aIncludeNames.end());
        std::set_intersection(vSourceNames.begin(), vSourceNames.end(),
                              CS.aIncludeNames.begin(), CS.aIncludeNames.end(), std::back_inserter(vReturn2));
        if (!vReturn2.empty())
            vSourceNames = vReturn2;
    }

    vReturn = vSourceNames;

    if (doExclude)
    {
        for (auto& sName : CS.aExcludeNames)
            sName = StringWrapper::TrimAll(sName);
        std::vector<CTL_StringType> vReturn2;
        std::sort(vSourceNames.begin(), vSourceNames.end());
        std::sort(CS.aExcludeNames.begin(), CS.aExcludeNames.end());
        std::set_difference(vSourceNames.begin(), vSourceNames.end(),
                            CS.aExcludeNames.begin(), CS.aExcludeNames.end(), std::back_inserter(vReturn2));
        if (!vReturn2.empty())
            vReturn = vReturn2;
    }

    if (doMapping)
    {
        std::vector<CTL_StringType> vMapped;
        for (auto& sName : vReturn)
        {
            auto iter = CS.mapNames.find(sName);
            if (iter != CS.mapNames.end())
                vMapped.push_back(iter->second);
            else
                vMapped.push_back(sName);
        }
        vReturn = vMapped;
    }
    return vReturn;
}

LRESULT CALLBACK DisplayTwainDlgProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    static SelectStruct *pS;
    static bool bOpenTemp;
    switch (message)
    {
    case WM_INITDIALOG:
    {
			if (CTL_TwainDLLHandle::s_DialogFont)
			{
				SendMessage(hWnd, WM_SETFONT, (WPARAM)CTL_TwainDLLHandle::s_DialogFont, 0);
				EnumChildWindows(hWnd, ChildEnumFontProc, (LPARAM)CTL_TwainDLLHandle::s_DialogFont);
			}

        HWND lstSources;
        CTL_TwainAppMgr::WriteLogInfo(_T("Initializing TWAIN Dialog...\n"));
        pS = (SelectStruct*)lParam;

        bOpenTemp = *pS->m_pbSourcesOnSelect;

        if (pS->CS.nOptions & DTWAIN_DLG_CENTER_SCREEN)
            CenterWindow(hWnd, NULL);
        else
            if (pS->CS.nOptions & DTWAIN_DLG_CENTER)
                CenterWindow(hWnd, GetParent(hWnd));
            else
                ::SetWindowPos(hWnd, NULL, pS->CS.xpos, pS->CS.ypos, 0, 0, SWP_NOSIZE);
        lstSources = GetDlgItem(hWnd, IDC_LSTSOURCES);

        // Set the title
        ::SetWindowText(hWnd, pS->CS.sTitle.c_str());

        // Fill the list box with the sources
        DTWAIN_ARRAY Array = 0;
        DTWAIN_EnumSources(&Array);
        DTWAINArrayLL_RAII arr(Array);

        auto vValues = EnumeratorVectorPtr<CTL_ITwainSourcePtr>(Array);

        int nCount;
        if (!vValues)
            nCount = 0;
        else
				nCount = static_cast<int>(vValues->size());
        pS->nItems = nCount;
        if (nCount == 0)
        {
            HWND hWndSelect = GetDlgItem(hWnd, IDOK);
            if (hWndSelect)
                EnableWindow(hWndSelect, FALSE);
            CTL_TwainAppMgr::WriteLogInfo(_T("Finished Adding names to TWAIN dialog...\n"));

            // Display the local strings if they are available:
            DisplayLocalString(hWnd, IDOK, IDS_SELECT_TEXT);
            DisplayLocalString(hWnd, IDCANCEL, IDS_CANCEL_TEXT);
            DisplayLocalString(hWnd, IDC_SOURCETEXT, IDS_SOURCES_TEXT);
            ::SetFocus(hWnd);
            CTL_TwainAppMgr::WriteLogInfo(_T("Finished Initializing TWAIN Dialog...\n"));
            return TRUE;
        }

        // Get the default Source
        // Turn off default open temporarily
        *pS->m_pbSourcesOnSelect = false;
        CTL_TwainAppMgr::WriteLogInfo(_T("Initializing TWAIN Dialog -- Retrieving default TWAIN Source...\n"));
        DTWAIN_SOURCE DefSource = DTWAIN_SelectDefaultSource();
        std::vector<TCHAR> DefName;
        if (DefSource)
        {
            LONG nBytes = DTWAIN_GetSourceProductName(DefSource, NULL, 0);
            if (nBytes > 0)
            {
                DefName.resize(nBytes);
					DTWAIN_GetSourceProductName(DefSource, DefName.data(), nBytes);
                CTL_TwainAppMgr::WriteLogInfo(_T("Initializing TWAIN Dialog -- Retrieved default TWAIN Source name...\n"));
            }
        }

        if (DefName.empty())
            CTL_TwainAppMgr::WriteLogInfo(_T("The TWAIN default name has no characters...\n"));
        else
        {
            CTL_StringStreamType strm;
            strm << _T("The default TWAIN source is ") << &DefName[0] << _T("...\n");
            CTL_TwainAppMgr::WriteLogInfo(strm.str());
        }

        vector<SIZE> TextExtents;
        HDC hdcList = NULL;
        if (pS->CS.nOptions & DTWAIN_DLG_HORIZONTALSCROLL)
            hdcList = GetDC(lstSources);

            // Get all the source names
            vector<CTL_StringType> vSourceNames;
            TCHAR ProdName[256];

            std::transform(vValues->begin(), vValues->end(), std::back_inserter(vSourceNames),
                           [&](CTL_ITwainSourcePtr ptr)
                            { DTWAIN_GetSourceProductName(ptr, ProdName, 255); return ProdName; }
                          );


            // Remove and rename sources depending on the options
            auto vNewSourceNames = AdjustSourceNames(vSourceNames, pS->CS);

			for (auto& sName : vNewSourceNames )
			{
            SIZE szType;
            if (hdcList)
            {
                    auto cstr = sName.c_str();
					::GetTextExtentPoint32(hdcList, cstr, static_cast<int>(_tcslen(cstr)), &szType);
                TextExtents.push_back(szType);
            }
        }

        if (hdcList)
        {
            ReleaseDC(lstSources, hdcList);
            sort(TextExtents.begin(), TextExtents.end(), ByCX);
        }

        // Sort the names
        if (pS->CS.nOptions & DTWAIN_DLG_SORTNAMES)
        {
            CTL_TwainAppMgr::WriteLogInfo(_T("Initializing TWAIN Dialog -- Sorting TWAIN Source names...\n"));
				sort(vSourceNames.begin(), vSourceNames.end());
        }

        LRESULT index;
        LRESULT DefIndex = 0;
        CTL_StringStreamType strm;
        CTL_TwainAppMgr::WriteLogInfo(_T("Initializing TWAIN Dialog -- Adding names to dialog...\n"));
			strm << _T("TWAIN found ") << vNewSourceNames.size() << _T(" source names to add to TWAIN dialog...\n");
        strm << _T("The TWAIN dialog window handle to add names is ") << lstSources << _T("\n");
        CTL_TwainAppMgr::WriteLogInfo(strm.str());
			for (auto& sName : vNewSourceNames)
        {
            strm.str(_T(""));
				strm << _T("The TWAIN name being added is ") << sName << _T("\n");
            CTL_TwainAppMgr::WriteLogInfo(strm.str());
				index = SendMessage(lstSources, LB_ADDSTRING, 0, (LPARAM)sName.c_str());
            CTL_TwainAppMgr::WriteLogInfo(_T("LB_ADDSTRING was sent to TWAIN dialog...\n"));
            CTL_TwainAppMgr::WriteLogInfo(_T("TWAIN now comparing names...\n"));
            if (!DefName.empty())
            {
					if (_tcscmp(sName.c_str(), (LPCTSTR)&DefName[0]) == 0)
                    DefIndex = index;
            }
            CTL_TwainAppMgr::WriteLogInfo(_T("TWAIN now finished comparing names...\n"));
        }
        if (DefName.empty())
            DefIndex = 0;

			if (!TextExtents.empty())
        {
            SendMessage(lstSources, LB_SETCURSEL, DefIndex, 0);
            if (pS->CS.nOptions & DTWAIN_DLG_HORIZONTALSCROLL)
                SendMessage(lstSources, LB_SETHORIZONTALEXTENT, TextExtents[0].cx, 0);
        }
            else
                SendMessage(lstSources, LB_SETCURSEL, 0, 0);

        CTL_TwainAppMgr::WriteLogInfo(_T("Finished Adding names to TWAIN dialog...\n"));

        // Display the local strings if they are available:
        DisplayLocalString(hWnd, IDOK, IDS_SELECT_TEXT);
        DisplayLocalString(hWnd, IDCANCEL, IDS_CANCEL_TEXT);
        DisplayLocalString(hWnd, IDC_SOURCETEXT, IDS_SOURCES_TEXT);
        ::SetFocus(hWnd);
        CTL_TwainAppMgr::WriteLogInfo(_T("Finished Initializing TWAIN Dialog...\n"));
        return TRUE;
    }

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK)
        {
            HWND lstSources = GetDlgItem(hWnd, IDC_LSTSOURCES);
            TCHAR sz[255];
            LRESULT nSel = SendMessage(lstSources, LB_GETCURSEL, 0, 0);
            SendMessage(lstSources, LB_GETTEXT, nSel, (LPARAM)sz);
            pS->SourceName = sz;
            *pS->m_pbSourcesOnSelect = bOpenTemp;
            EndDialog(hWnd, LOWORD(wParam));
            return TRUE;
        }
        else
            if (LOWORD(wParam) == IDCANCEL)
            {
                pS->SourceName.clear();
                *pS->m_pbSourcesOnSelect = bOpenTemp;
                EndDialog(hWnd, LOWORD(wParam));
                return TRUE;
            }
        break;
    }
    return FALSE;
}

void DisplayLocalString(HWND hWnd, int nID, int resID)
{
    CTL_StringType sText;
    size_t status = 0;
        sText = GetTwainDlgTextFromResource(resID, status);
        if (status)
        {
            HWND hWndControl = GetDlgItem(hWnd, nID);
            if (hWndControl)
                SetWindowText(hWndControl, sText.c_str());
    }
}
#endif
