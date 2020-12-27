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
#include "ctltr037.h"
#include "ctltwmgr.h"

using namespace dynarithmic;
/////////////////////////////////////////////////////////////////////////
CTL_FileSystemTriplet::CTL_FileSystemTriplet(CTL_ITwainSession *pSession,
                                         CTL_ITwainSource* pSource,
                                         TW_UINT16 nMsg)
                                         : CTL_TwainTriplet(), m_FileSystem()
{
    SetSessionPtr(pSession);
    SetSourcePtr( pSource );

    // Get the app manager's AppID
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();
    if ( pMgr && pMgr->IsValidTwainSession( pSession ))
    {
        if ( pSource )
        {
            Init( pSession->GetAppIDPtr(),
                  pSource->GetSourceIDPtr(),
                  DG_CONTROL,
                  DAT_FILESYSTEM,
                  nMsg,
                  (TW_MEMREF)((pTW_FILESYSTEM)&m_FileSystem));
            SetAlive (true);
        }
    }
}

TW_UINT16 CTL_FileSystemTriplet::SelectAutoCaptureDirectory( const CTL_StringType& sDir )
{
    return ChangeDirectoryHelper(sDir, MSG_AUTOMATICCAPTUREDIRECTORY);
}

TW_UINT16 CTL_FileSystemTriplet::ChangeDirectory(const CTL_StringType& sDir)
{
   return ChangeDirectoryHelper(sDir, MSG_CHANGEDIRECTORY);
}

TW_UINT16 CTL_FileSystemTriplet::CreateDirectory(const CTL_StringType& sDir)
{
    return ChangeDirectoryHelper(sDir, MSG_CREATEDIRECTORY);
}

TW_UINT16 CTL_FileSystemTriplet::CopyFile(const CTL_StringType& sInput, const CTL_StringType& sOutput)
{
    strcpy(m_FileSystem.InputName, StringConversion::Convert_Native_To_Ansi(sInput).c_str());
    strcpy(m_FileSystem.OutputName, StringConversion::Convert_Native_To_Ansi(sOutput).c_str());
    return ExecuteIt(MSG_COPY);
}

TW_UINT16 CTL_FileSystemTriplet::ChangeDirectoryHelper(const CTL_StringType& sDir, TW_UINT16 Msg)
{
    strcpy(m_FileSystem.InputName, StringConversion::Convert_Native_To_Ansi(sDir).c_str());
    return ExecuteIt(Msg);
}

TW_UINT16 CTL_FileSystemTriplet::DeleteFile(const CTL_StringType& sDir, bool bRecursive)
{
    strcpy(m_FileSystem.InputName, StringConversion::Convert_Native_To_Ansi(sDir).c_str());
    m_FileSystem.Recursive = bRecursive?TRUE:FALSE;
    return ExecuteIt(MSG_DELETE);
}

TW_UINT16 CTL_FileSystemTriplet::FormatMedia(const CTL_StringType& sDir)
{
    strcpy(m_FileSystem.InputName, StringConversion::Convert_Native_To_Ansi(sDir).c_str());
    return ExecuteIt(MSG_FORMATMEDIA);
}

TW_UINT16 CTL_FileSystemTriplet::GetFirstFile()
{
    return ExecuteIt(MSG_GETFIRSTFILE);
}

TW_UINT16 CTL_FileSystemTriplet::GetNextFile(TW_MEMREF Context)
{
    // Assume that this will be done after GETFIRSTFILE or previous GETNEXTFILE
    m_FileSystem.Context = Context;
    return ExecuteIt(MSG_GETNEXTFILE);
}

TW_UINT16 CTL_FileSystemTriplet::GetInfo(const CTL_StringType& sDir)
{
    strcpy(m_FileSystem.InputName, StringConversion::Convert_Native_To_Ansi(sDir).c_str());
    return ExecuteIt(MSG_GETINFO);
}

TW_UINT16 CTL_FileSystemTriplet::GetClose()
{
    // Assume that this will be done after GETFIRSTFILE or previous GETNEXTFILE
    return ExecuteIt(MSG_GETCLOSE);
}

void CTL_FileSystemTriplet::GetTWFileSystem(TW_FILESYSTEM& FS)
{
    FS = m_FileSystem;
}

TW_UINT16 CTL_FileSystemTriplet::Rename(const CTL_StringType& sInput, const CTL_StringType& sOutput)
{
    // Assume that this will be done after GETFIRSTFILE or previous GETNEXTFILE
    strcpy(m_FileSystem.InputName, StringConversion::Convert_Native_To_Ansi(sInput).c_str());
    strcpy(m_FileSystem.OutputName, StringConversion::Convert_Native_To_Ansi(sOutput).c_str());
    return ExecuteIt(MSG_RENAME);
}

TW_UINT16 CTL_FileSystemTriplet::ExecuteIt(TW_UINT16 Msg)
{
    std::get<4>(GetTripletArgs()) = Msg;
    return Execute();
}

