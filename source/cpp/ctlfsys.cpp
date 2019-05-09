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
#include "dtwain.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "ctltr037.h"
#include "errorcheck.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

#define GET_FIRST           0
#define GET_NEXT            1
#define GET_CLOSE           2
#define GET_FILE_TYPE       3
#define CHANGE_DIRECTORY    4
#define CREATE_DIRECTORY    5
#define COPY_DIRECTORY      6
#define RENAME_DIRECTORY    7
#define DELETE_DIRECTORY    8
#define FORMAT_MEDIA        9

static DTWAIN_BOOL FSDirectory(CTL_ITwainSource* pSource, LPCTSTR sDir, LONG nWhich);
static LONG FSGetFile(CTL_ITwainSource* pSource, LPTSTR sDir, LPLONG FSHandle, LONG nWhich);
static DTWAIN_BOOL FSGetFileInfo(CTL_ITwainSource* Source, LPCTSTR szFileName, TW_FILESYSTEM* pFS);
static bool FSGetCameras(CTL_ITwainSource *pSource, LPDTWAIN_ARRAY Cameras, TW_UINT16 CameraType );
static bool GetResults(CTL_ITwainSource* pSource, CTL_FileSystemTriplet* pFS, TW_UINT16 rc);
static DTWAIN_BOOL FSFileOp(CTL_ITwainSource* pSource, LPCTSTR sInput, LPCTSTR sOutput, LONG nWhich);
static DTWAIN_BOOL FSFileOp2(CTL_ITwainSource* pSource, LPCTSTR sInput, DTWAIN_BOOL bRecursive, LONG nWhich);

typedef bool (*WALKFSPROC)(TW_FILESYSTEM* p, LPARAM UserParam);
static bool WalkFileSystem(WALKFSPROC pProc, CTL_ITwainSource* pSource, LPCTSTR szDir, LPARAM UserParam);
static bool EnumCameraProc(TW_FILESYSTEM* p, LPARAM UserParam);

/**********************************************************************************/
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsFileSystemSupported(DTWAIN_SOURCE Source )
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex( pHandle, false, FUNC_MACRO);
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&]{ return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = FSDirectory(pSource, _T("/"), CHANGE_DIRECTORY);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

/**********************************************************************************/
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSChangeDirectory(DTWAIN_SOURCE Source, LPCTSTR szNewDir)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szNewDir))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] {return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    // Get session pointer
    DTWAIN_BOOL bRet = FSDirectory(pSource, szNewDir, CHANGE_DIRECTORY);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}
/**********************************************************************************/
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSGetFirstFile(DTWAIN_SOURCE Source, LPTSTR szDir, LPLONG FSHandle)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szDir, FSHandle))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !FSHandle;}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    // Get session pointer
    DTWAIN_BOOL bRet = FSGetFile(pSource, szDir, FSHandle, GET_FIRST);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSGetNextFile(DTWAIN_SOURCE Source, LPTSTR szDir, LPLONG FSHandle)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szDir, FSHandle))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !FSHandle;}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    // Get session pointer
    DTWAIN_BOOL bRet = FSGetFile(pSource, szDir, FSHandle, GET_NEXT);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSGetClose(DTWAIN_SOURCE Source, LPLONG FSHandle)
{
    LOG_FUNC_ENTRY_PARAMS((Source, FSHandle))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !FSHandle;}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    // Get session pointer
    DTWAIN_BOOL bRet = FSGetFile(pSource, NULL, FSHandle, GET_CLOSE);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSGetFileType(DTWAIN_SOURCE Source, LPCTSTR szFileName, LPLONG pFileType)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szFileName, pFileType))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !pFileType;}, DTWAIN_ERR_INVALID_PARAM,false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    TW_FILESYSTEM *pFS = pSource->GetFileSystem();
    DTWAIN_BOOL bRet = FSGetFileInfo(pSource, szFileName, pFS);
    if ( bRet )
        *pFileType = pFS->FileType;
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSCreateDirectory(DTWAIN_SOURCE Source, LPCTSTR szNewDir)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szNewDir))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !szNewDir;}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = FSDirectory(pSource, szNewDir, CREATE_DIRECTORY);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)

}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSCopy(DTWAIN_SOURCE Source, LPCTSTR szInput, LPCTSTR szOutput)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szInput, szOutput))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&]{return !szInput || !szOutput;}, DTWAIN_ERR_INVALID_PARAM,
                                      false, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = FSFileOp(pSource, szInput, szOutput, COPY_DIRECTORY);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)

}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSRename(DTWAIN_SOURCE Source, LPCTSTR szInput, LPCTSTR szOutput)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szInput, szOutput))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !szInput || !szOutput;}, DTWAIN_ERR_INVALID_PARAM,
                                      false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen( pSource );},
                                        DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = FSFileOp(pSource, szInput, szOutput, RENAME_DIRECTORY);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)

}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSDelete(DTWAIN_SOURCE Source, LPCTSTR szInput,
                                         DTWAIN_BOOL bRecursive)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szInput, bRecursive))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !szInput; },
                                        DTWAIN_ERR_INVALID_PARAM,false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen(pSource); },
                                    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = FSFileOp2(pSource, szInput, bRecursive, DELETE_DIRECTORY);

    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)

}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FSFormat(DTWAIN_SOURCE Source, LPCTSTR szDir)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szDir))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !szDir; }, DTWAIN_ERR_INVALID_PARAM,
                                     false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen(pSource); },
                                      DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = FSDirectory(pSource, szDir, FORMAT_MEDIA);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)

}

struct CameraStruct {
    DTWAIN_ARRAY aCameras;
    TW_UINT16 CameraType;
};

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumTopCameras(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Cameras)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Cameras))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !Cameras; }, DTWAIN_ERR_INVALID_PARAM,
        false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen(pSource); },
        DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    FSGetCameras( pSource, Cameras, DTWAIN_FT_CAMERATOP );

   LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumBottomCameras(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Cameras)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Cameras))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !Cameras; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen(pSource); }, DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    FSGetCameras( pSource, Cameras, DTWAIN_FT_CAMERABOTTOM );

   LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumCameras(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Cameras)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Cameras))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !Cameras; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !CTL_TwainAppMgr::IsSourceOpen(pSource); }, DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    FSGetCameras( pSource, Cameras, DTWAIN_FT_CAMERA );

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCamera(DTWAIN_SOURCE Source, LPCTSTR szCamera)
{
    LOG_FUNC_ENTRY_PARAMS((Source, szCamera))
    DTWAIN_BOOL bRet = DTWAIN_FSChangeDirectory(Source, szCamera);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


bool FSGetCameras(CTL_ITwainSource *pSource, LPDTWAIN_ARRAY Cameras, TW_UINT16 CameraType )
{

    // Change to the root directory
    DTWAIN_ARRAY aCameras = 0;
    aCameras = DTWAIN_ArrayCreate(DTWAIN_ARRAYSTRING, 0);
    CameraStruct CS;
    CS.aCameras = aCameras;
    CS.CameraType = CameraType;
    WalkFileSystem(EnumCameraProc, pSource, _T("/"), (LPARAM)&CS);
    *Cameras = CS.aCameras;
    return true;
}

bool EnumCameraProc(TW_FILESYSTEM* p, LPARAM UserParam)
{
    CameraStruct *pCS = (CameraStruct *)UserParam;
    if ( p->FileType == pCS->CameraType )
        DTWAIN_ArrayAdd(pCS->aCameras, p->OutputName);
    return true;
}

bool WalkFileSystem(WALKFSPROC pProc, CTL_ITwainSource* pSource, LPCTSTR szStart, LPARAM UserParam)
{
    // Change the directory
    DTWAIN_BOOL bRes;
    vector<TCHAR> szCurDir(256, 0);
    bRes = DTWAIN_FSChangeDirectory(pSource, szStart);
    LONG Context;
    StringWrapper::SafeStrcpy(&szCurDir[0], szStart);
    TW_FILESYSTEM *pFS = pSource->GetFileSystem();
    TW_UINT16 FileType;

    // Get the first file in the directory
    DTWAIN_FSGetFirstFile( pSource, &szCurDir[0], &Context );
    while ( bRes )
    {
        FileType = static_cast<TW_UINT16>(pFS->FileType);
        if ( !(*pProc)(pFS, UserParam))
            return true;
        switch ( FileType )
        {
            case TWFY_DIRECTORY:
                bRes = WalkFileSystem(pProc, pSource, &szCurDir[0], UserParam);
                if ( bRes )
                {
                    DTWAIN_FSGetClose( pSource, (LPLONG)pFS->Context );
                    return bRes?true:false;
                }
            break;
        }
        bRes = DTWAIN_FSGetNextFile( pSource, &szCurDir[0], &Context);
    }

    DTWAIN_FSGetClose(pSource, &Context);
    return TRUE;
}

DTWAIN_BOOL FSGetFileInfo(CTL_ITwainSource* pSource, LPCTSTR szFileName, TW_FILESYSTEM* pFS)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    CTL_FileSystemTriplet FS(pSession, pSource);
    TW_UINT16 rc = FS.GetInfo(szFileName);
    switch(rc)
    {
        case TWRC_SUCCESS:
            FS.GetTWFileSystem(*pFS);
            return TRUE;
        break;

        case TWRC_FAILURE:
        {
            TW_UINT16 cc = CTL_TwainAppMgr::GetConditionCode(pSession, pSource);
            CTL_TwainAppMgr::ProcessConditionCodeError(cc);
            return FALSE;
        }
        break;

        default:
            return FALSE;
    }
    return FALSE;
}

DTWAIN_BOOL FSDirectory(CTL_ITwainSource* pSource, LPCTSTR sDir, LONG nWhich)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    CTL_FileSystemTriplet FS(pSession, pSource);
    TW_UINT16 rc = 0;
    switch(nWhich)
    {
        case CHANGE_DIRECTORY:
            rc = FS.ChangeDirectory(sDir);
        break;

        case CREATE_DIRECTORY:
            rc = FS.CreateDirectory(sDir);
        break;

        case FORMAT_MEDIA:
            rc = FS.FormatMedia(sDir);
        break;
    }

    return GetResults(pSource, &FS, rc);
}


DTWAIN_BOOL FSFileOp(CTL_ITwainSource* pSource, LPCTSTR sInput, LPCTSTR sOutput, LONG nWhich)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    CTL_FileSystemTriplet FS(pSession, pSource);
    TW_UINT16 rc = 0;
    switch(nWhich)
    {
        case COPY_DIRECTORY:
            rc = FS.CopyFile(sInput, sOutput);
        break;

        case RENAME_DIRECTORY:
            rc = FS.Rename(sInput, sOutput);
        break;
    }

    return GetResults(pSource, &FS, rc);
}


DTWAIN_BOOL FSFileOp2(CTL_ITwainSource* pSource, LPCTSTR sInput, DTWAIN_BOOL bRecursive, LONG nWhich)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    CTL_FileSystemTriplet FS(pSession, pSource);
    TW_UINT16 rc = 0;
    switch(nWhich)
    {
        case DELETE_DIRECTORY:
            rc = FS.DeleteFile(sInput, bRecursive?true:false);
        break;
    }
    return GetResults(pSource, &FS, rc);
}


LONG FSGetFile(CTL_ITwainSource* pSource, LPTSTR sDir, LPLONG FSHandle, LONG nWhich)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    CTL_FileSystemTriplet FS(pSession, pSource);
    TW_UINT16 rc = 0;
    switch(nWhich)
    {
        case GET_FIRST:
            rc = FS.GetFirstFile();
        break;

        case GET_NEXT:
            rc = FS.GetNextFile((TW_MEMREF)(*FSHandle));
        break;

        case GET_CLOSE:
            rc = FS.GetClose();
        break;
    }

    bool bRet = GetResults(pSource, &FS, rc);
    if ( bRet )
    {
        TW_FILESYSTEM *pFS = pSource->GetFileSystem();
        *FSHandle = (LONG_PTR)(pFS->Context);
        if ( nWhich != GET_CLOSE)
            StringWrapper::SafeStrcpy(sDir, StringConversion::Convert_AnsiPtr_To_Native(pFS->OutputName).c_str());
    }
    return bRet;
}

bool GetResults(CTL_ITwainSource* pSource, CTL_FileSystemTriplet* pFST, TW_UINT16 rc)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    switch (rc)
    {
        case TWRC_SUCCESS:
        {
            TW_FILESYSTEM *pFS = pSource->GetFileSystem();
            pFST->GetTWFileSystem(*pFS);
            return true;
        }
        break;

        case TWRC_FAILURE:
        {
            TW_UINT16 cc = CTL_TwainAppMgr::GetConditionCode(pSession, pSource);
            CTL_TwainAppMgr::ProcessConditionCodeError(cc);
            return false;
        }
        break;

        default:
            return false;
    }
    return false;
}
