/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
 #ifdef _WIN32
CTL_StringType GetTwainDirFullNameEx(LPCTSTR strTwainDLLName,
									bool bLeaveLoaded = false,
									boost::dll::shared_library *pModule = nullptr);

CTL_StringType GetTwainDirFullName(LPCTSTR strTwainDLLName, 
                                    LPLONG pWhichSearch, 
                                    bool bLeaveLoaded = false, 
                                    boost::dll::shared_library *pModule = nullptr)
{
	static std::unordered_map<LONG, CTL_String> searchOrderMap = {
		{DTWAIN_TWAINDSMSEARCH_WSO,"WSO"},
		{ DTWAIN_TWAINDSMSEARCH_WOS,"WOS" },
		{ DTWAIN_TWAINDSMSEARCH_SWO,"SWO" },
        { DTWAIN_TWAINDSMSEARCH_SOW,"SOW" },
        { DTWAIN_TWAINDSMSEARCH_OWS,"OWS" },
        { DTWAIN_TWAINDSMSEARCH_OSW,"OSW" },
        { DTWAIN_TWAINDSMSEARCH_W,"W" },
		{ DTWAIN_TWAINDSMSEARCH_S,"S" },
		{ DTWAIN_TWAINDSMSEARCH_O,"O" },
		{ DTWAIN_TWAINDSMSEARCH_WS,"WS" },
		{ DTWAIN_TWAINDSMSEARCH_WO,"WO" },
		{ DTWAIN_TWAINDSMSEARCH_SW,"SW" },
		{ DTWAIN_TWAINDSMSEARCH_SO,"SO" },
		{ DTWAIN_TWAINDSMSEARCH_OW,"OW" },
		{ DTWAIN_TWAINDSMSEARCH_OS,"OS" } };

	auto iter = searchOrderMap.find(CTL_TwainDLLHandle::s_TwainDSMSearchOrder);
	if (iter != searchOrderMap.end())
	{
		CTL_TwainDLLHandle::s_TwainDSMSearchOrderStr = StringConversion::Convert_Ansi_To_Native(iter->second);
		return GetTwainDirFullNameEx(strTwainDLLName, bLeaveLoaded, pModule);
	}
    // This will completely use the Ex version of finding the directory
    return GetTwainDirFullNameEx(strTwainDLLName, bLeaveLoaded, pModule);
}

CTL_StringType GetTwainDirFullNameEx(LPCTSTR strTwainDLLName,
									bool bLeaveLoaded,
									boost::dll::shared_library *pModule)
{
    static constexpr int WinDirPos = 0;
    static constexpr int SysDirPos = 1;
    static constexpr int SysPathPos = 2;
    static constexpr int CurDirPos = 3;
    static constexpr int UserDefPos = 4;

    // make sure we get only the file name.  If a directory path
    // is given in the strTwainDLLName argument, it is ignored.
    StringWrapper::StringArrayType fComponents;
    StringWrapper::SplitPath(strTwainDLLName, fComponents);
    CTL_StringType fName = fComponents[StringWrapper::NAME_POS] + fComponents[StringWrapper::EXTENSION_POS];

    // we first search the Windows directory.
    // if TWAIN isn't found there, check system directory.
    // if not there, then use the Windows path search logic
    std::set<CTL_StringType> strSet;
	static std::unordered_map<StringWrapper::traits_type::char_type, int> searchMap = { 
		{StringConversion::Convert_Ansi_To_Native("C")[0],CurDirPos},
		{StringConversion::Convert_Ansi_To_Native("W")[0],WinDirPos},
		{StringConversion::Convert_Ansi_To_Native("S")[0],SysDirPos},
		{StringConversion::Convert_Ansi_To_Native("O")[0],SysPathPos},
        {StringConversion::Convert_Ansi_To_Native("U")[0],UserDefPos },
     };

    std::vector<CTL_StringType> dirNames(searchMap.size());

    dirNames[WinDirPos] = StringWrapper::GetWindowsDirectory();
    dirNames[SysDirPos] = StringWrapper::GetSystemDirectory();
    dirNames[SysPathPos] = CTL_StringType(_T(""));
	dirNames[CurDirPos] = StringWrapper::SplitPath(GetDTWAINDLLPath())[StringWrapper::DIRECTORY_POS];
    dirNames[UserDefPos] = StringWrapper::SplitPath(CTL_TwainDLLHandle::s_TwainDSMUserDirectory)[StringWrapper::DIRECTORY_POS];

	CTL_StringType curSearchOrder = CTL_TwainDLLHandle::s_TwainDSMSearchOrderStr;
    CTL_StringType fNameTotal;
    bool bFoundTwain = false;
	int minSize = (std::min)(dirNames.size(), curSearchOrder.size());
	for (int i = 0; i < minSize && !bFoundTwain; ++i)
    {
        // skip this search if -1 is given
		int nCurDir = searchMap[curSearchOrder[i]];
        if (nCurDir == -1)
            continue;

        // only do this if we haven't checked the directory
        CTL_StringType dirNameToUse = dirNames[nCurDir];
        if (strSet.find(dirNameToUse) != strSet.end())
            continue;

        // record that we are trying this directory
        strSet.insert(dirNameToUse);

        if (!dirNameToUse.empty())
            fNameTotal = StringWrapper::AddBackslashToDirectory(dirNameToUse) + fName;
        else
            fNameTotal = fName;
        #ifdef _WIN32
        UINT nOldError = ::SetErrorMode(SEM_NOOPENFILEERRORBOX);
        #endif

        boost::dll::shared_library libloader;
        boost::system::error_code ec;
        libloader.load(fNameTotal, ec); 

        #ifdef _WIN32
        ::SetErrorMode(nOldError);
        #endif

        if (ec != boost::system::errc::success)
            continue;

        // Try to load the source manager
        DSMENTRYPROC lpDSMEntry = nullptr;
        try {
            lpDSMEntry = dtwain_library_loader<DSMENTRYPROC>::get_func_ptr(libloader.native(), "DSM_Entry");
        }
        catch (boost::exception&)
        {

        }

        if (lpDSMEntry)
        {
            // We need the full module name
            fNameTotal = StringConversion::Convert_Ansi_To_Native(libloader.location().string());
            if (!bLeaveLoaded)
                // Unload the library
                libloader.unload();
            if (pModule)
            {
                if (bLeaveLoaded)
                    *pModule = libloader;
            }
            return fNameTotal;
        }
        libloader.unload();
    }
    return CTL_StringType(_T(""));
}

#endif