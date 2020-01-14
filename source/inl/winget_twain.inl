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
 #ifdef _WIN32
CTL_StringType GetTwainDirFullName(LPCTSTR strTwainDLLName, 
                                    LPLONG pWhichSearch, 
                                    bool bLeaveLoaded = false, 
                                    boost::dll::shared_library *pModule = nullptr)
{
    // make sure we get only the file name.  If a directory path
    // is given in the strTwainDLLName argument, it is ignored.
    StringWrapper::StringArrayType fComponents;
    StringWrapper::SplitPath(strTwainDLLName, fComponents);
    CTL_StringType fName = fComponents[StringWrapper::NAME_POS] + fComponents[StringWrapper::EXTENSION_POS];

    // we first search the Windows directory.
    // if TWAIN isn't found there, check system directory.
    // if not there, then use the Windows path search logic
    CTL_StringType dirNames[3];
    std::set<CTL_StringType> strSet;
    dirNames[0] = StringWrapper::GetWindowsDirectory();
    dirNames[1] = StringWrapper::GetSystemDirectory();
    dirNames[2] = CTL_StringType(_T(""));
    const int whichSearch[] = { DTWAIN_TWAINDSMSEARCH_W, DTWAIN_TWAINDSMSEARCH_S, DTWAIN_TWAINDSMSEARCH_O };

    const int sSearchOrder[15][3] = { { 0,1,2 }, //WSO
    { 0,2,1 },   //WOS
    { 1,0,2 },   //SWO
    { 1,2,0 },   //SOW
    { 2,0,1 },   //OWS
    { 2,1,0 },   //OSW
    { 0,-1,-1 }, //W
    { 1,-1,-1 }, //S
    { 2,-1,-1 }, //O
    { 0,1,-1 },  //WS
    { 0,2,-1 },  //WO
    { 1,0,-1 },  //SW
    { 1,2,-1 },  //SO
    { 2,0,-1 },  //OW
    { 2,1,-1 } }; //OS

    int curSearchOrder = CTL_TwainDLLHandle::s_TwainDSMSearchOrder;

    CTL_StringType fNameTotal;
    bool bFoundTwain = false;
    if (pWhichSearch)
        *pWhichSearch = DTWAIN_TWAINDSMSEARCH_NOTFOUND;
    for (int i = 0; i < 3 && !bFoundTwain; ++i)
    {
        // skip this search if -1 is given
        int nCurDir = sSearchOrder[curSearchOrder][i];
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
            if (pWhichSearch)
                *pWhichSearch = whichSearch[nCurDir];
            return fNameTotal;
        }
        libloader.unload();
    }
    return CTL_StringType(_T(""));
}
#endif