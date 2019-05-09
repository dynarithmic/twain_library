/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2019 Dynarithmic Software.

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
#include <cstring>
#include <stdio.h>
#include <stdlib.h>
#include <algorithm>
#include <string>
#include <boost/filesystem.hpp>
#include <boost/format.hpp>
#include "ctltwmgr.h"
#include "ctltrall.h"
#include "ctlres.h"
#include "twilres.h"
#include "dtwinverex.h"
#include "ctlobstr.h"
#include "errorcheck.h"

using namespace boost::filesystem;
using namespace dynarithmic;

bool CreateDirectoryTree(LPCTSTR lpszPath, DWORD* /*lasterror*/)
{
    CTL_StringType thePath;
    CTL_StringArrayType pathInfo;
    StringWrapper::SplitPath(lpszPath, pathInfo);

    CTL_StringArrayType dirs;
    StringWrapper::Tokenize(pathInfo[StringWrapper::DIRECTORY_POS], _T("\\/"), dirs);
    thePath = pathInfo[StringWrapper::DRIVE_POS] + StringWrapper::Join(dirs,_T("\\"));
    return true;
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTempFileDirectoryEx(LPCTSTR szFilePath, LONG CreationFlags)
{
    LOG_FUNC_ENTRY_PARAMS((szFilePath, CreationFlags))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    if (CreationFlags == 0)
    {
        path p(szFilePath);

        if (exists(p))
        {
           if (is_directory(p))
           {
               auto retVal = p.native();
               if (!retVal.empty() && *retVal.rbegin() != boost::filesystem::path::preferred_separator)
                   retVal += boost::filesystem::path::preferred_separator;
               CTL_TwainDLLHandle::s_TempFilePath = CTL_StringType(retVal.begin(), retVal.end());
               LOG_FUNC_EXIT_PARAMS(true)
            }
            else
               DTWAIN_Check_Error_Condition_0_Ex(pHandle, []{ return false;}, DTWAIN_ERR_FILEOPEN, false, FUNC_MACRO);
        }
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTempFileDirectory(LPCTSTR szFilePath)
{
    LOG_FUNC_ENTRY_PARAMS((szFilePath))
    DTWAIN_BOOL bRetval = DTWAIN_SetTempFileDirectoryEx(szFilePath, 0);
    LOG_FUNC_EXIT_PARAMS(bRetval)
    CATCH_BLOCK(false)
}

LONG DLLENTRY_DEF DTWAIN_GetTempFileDirectory(LPTSTR szFilePath, LONG nMaxLen)
{
    LOG_FUNC_ENTRY_PARAMS((szFilePath, nMaxLen))
    LONG nRealLen = CopyInfoToCString(GetDTWAINTempFilePath(), szFilePath, nMaxLen);
    LOG_FUNC_EXIT_PARAMS(nRealLen)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}
