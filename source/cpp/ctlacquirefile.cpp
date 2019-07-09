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
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "sourceacquireopts.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

static bool AcquireFileHelper(SourceAcquireOptions& opts);

DTWAIN_BOOL       DLLENTRY_DEF DTWAIN_AcquireFileEx(DTWAIN_SOURCE Source,
                                                    DTWAIN_ARRAY aFileNames,
                                                    LONG     lFileType,
                                                    LONG     lFileFlags,
                                                    LONG     PixelType,
                                                    LONG     lMaxPages,
                                                    DTWAIN_BOOL bShowUI,
                                                    DTWAIN_BOOL bCloseSource,
                                                    LPLONG pStatus)
{
    LOG_FUNC_ENTRY_PARAMS((Source, aFileNames, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI,
        bCloseSource, pStatus))
    LONG Count, Type;
    bool bRetval = true;
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    if (aFileNames)
    {
        auto vValues = EnumeratorVectorPtr<CTL_StringType>(aFileNames);
        if (vValues)
            Count = static_cast<LONG>(vValues->size());
        else
            Count = 0;
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return (Count <= 0); }, DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);

        Type = EnumeratorFunctionImpl::GetEnumeratorType(aFileNames);
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return (Type != DTWAIN_ARRAYSTRING); }, DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);
    }
    else
        bRetval = false;
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !bRetval; }, DTWAIN_ERR_BAD_ARRAY, false, FUNC_MACRO);

    SourceAcquireOptions opts = SourceAcquireOptions().setHandle(GetDTWAINHandle_Internal()).setSource(Source).setFileType(lFileType).setFileFlags(lFileFlags | DTWAIN_USELIST).
                setFileList(aFileNames).setPixelType(PixelType).setMaxPages(lMaxPages).setShowUI(bShowUI ? true : false).
                setRemainOpen(!(bCloseSource ? true : false));

    bRetval = AcquireFileHelper(opts);
    if (pStatus)
        *pStatus = opts.getStatus();
    LOG_FUNC_EXIT_PARAMS(bRetval)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL       DLLENTRY_DEF DTWAIN_AcquireFile(DTWAIN_SOURCE Source,
                                                LPCTSTR   lpszFile,
                                                LONG     lFileType,
                                                LONG     lFileFlags,
                                                LONG     PixelType,
                                                LONG     lMaxPages,
                                                DTWAIN_BOOL bShowUI,
                                                DTWAIN_BOOL bCloseSource,
                                                LPLONG pStatus)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus))
    lFileFlags &= ~DTWAIN_USELIST;
    SourceAcquireOptions opts = SourceAcquireOptions().setHandle(GetDTWAINHandle_Internal()).setSource(Source).setFileName(lpszFile).setFileType(lFileType).setFileFlags(lFileFlags).setPixelType(PixelType).
        setMaxPages(lMaxPages).setShowUI(bShowUI ? true : false).setRemainOpen(!(bCloseSource ? true : false)).setAcquireType(ACQUIREFILE);
    bool bRetval = AcquireFileHelper(opts);
    if (pStatus)
        *pStatus = opts.getStatus();
    LOG_FUNC_EXIT_PARAMS(bRetval)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetFileAutoIncrement(DTWAIN_SOURCE Source, LONG nValue, DTWAIN_BOOL bResetOnAcquire, DTWAIN_BOOL bSet)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nValue, bResetOnAcquire, bSet))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    pSource->SetFileAutoIncrement(bSet ? true : false, nValue);
    pSource->SetFileAutoIncrementFlags(bResetOnAcquire ? DTWAIN_INCREMENT_DYNAMIC : DTWAIN_INCREMENT_DEFAULT);
    /*    if ( nInitial < -1 )
    nInitial = 0;                                    */
    pSource->SetFileAutoIncrementBase(0); //nInitial );
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_ACQUIRE dynarithmic::DTWAIN_LLAcquireFile(SourceAcquireOptions& opts)
{
    LOG_FUNC_ENTRY_PARAMS((opts))
    DTWAIN_ARRAY FileList = opts.getFileList();
    if (FileList)
        opts.setFileFlags(opts.getFileFlags() | DTWAIN_USELIST);
    opts.setActualAcquireType(TWAINAcquireType_File);
    DTWAIN_ACQUIRE Ret = LLAcquireImage(opts);
    LOG_FUNC_EXIT_PARAMS(Ret)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

static bool AcquireFileHelper(SourceAcquireOptions& opts)
{
    LOG_FUNC_ENTRY_PARAMS((opts))
    DTWAIN_ARRAY aDibs = 0;
    CTL_ITwainSource *pSource = VerifySourceHandle(GetDTWAINHandle_Internal(), opts.getSource());

    // Check if file type requires a loaded DLL
    DumpArrayContents(opts.getFileList(), 0);
    opts.setAcquireType(ACQUIREFILE);
	opts.setDiscardDibs(true); // make sure we remove acquired dibs for file handling
    aDibs = SourceAcquire(opts);
    if (opts.getStatus() < 0 && !aDibs)
    {
        LOG_FUNC_EXIT_PARAMS(false)
    }

    bool bRetval = false;
    if (aDibs)
    {
        bRetval = TRUE;
        auto vDibs = EnumeratorVectorPtr<LPVOID>(aDibs);
        if (vDibs)
            for_each(begin(*vDibs), end(*vDibs), EnumeratorFunctionImpl::EnumeratorDestroy);
        pSource->ResetAcquisitionAttempts(nullptr);
        if (EnumeratorFunctionImpl::EnumeratorIsValid(aDibs))
            EnumeratorFunctionImpl::EnumeratorDestroy(aDibs);
    }

    if (DTWAIN_GetTwainMode() == DTWAIN_MODAL)
    {
        if (!aDibs)
            bRetval = false;
        else
            if (opts.getStatus() == DTWAIN_TN_ACQUIREDONE)
                bRetval = true;
    }
    else
    if (DTWAIN_GetTwainMode() == DTWAIN_MODELESS)
        pSource->m_pUserPtr = nullptr;

    LOG_FUNC_EXIT_PARAMS(bRetval)
    CATCH_BLOCK(false)
}

