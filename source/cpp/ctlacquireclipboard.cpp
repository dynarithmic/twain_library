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
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "sourceacquireopts.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

DTWAIN_ARRAY  DLLENTRY_DEF DTWAIN_AcquireToClipboard(DTWAIN_SOURCE Source,LONG PixelType,LONG nMaxPages,LONG nTransferMode,DTWAIN_BOOL bDiscardDibs,DTWAIN_BOOL bShowUI,DTWAIN_BOOL bCloseSource,
                                                    LPLONG pStatus)
{
    LOG_FUNC_ENTRY_PARAMS((Source, PixelType, nMaxPages, nTransferMode, bDiscardDibs, bShowUI, bCloseSource, pStatus))
    DTWAIN_ARRAY aDibs = 0;
    SourceAcquireOptions opts = SourceAcquireOptions().setHandle(GetDTWAINHandle_Internal()).setSource(Source).setPixelType(PixelType).setMaxPages(nMaxPages).
        setTransferMode(nTransferMode).setShowUI(bShowUI ? true : false).setRemainOpen(!(bCloseSource ? true : false)).
        setAcquireType(ACQUIRECLIPBOARD).setDiscardDibs(bDiscardDibs ? true : false);

    aDibs = SourceAcquire(opts);
    if (pStatus)
        *pStatus = opts.getStatus();
    if (aDibs && bDiscardDibs)
    {
        DTWAINArrayLL_RAII arrAcq(aDibs);
        auto vValues = EnumeratorVectorPtr<LPVOID>(aDibs);
        DTWAIN_ARRAY aDibHandle = 0;
        LONG nCount = static_cast<LONG>(vValues->size()); //DTWAIN_ArrayGetCount( aDibs );
        for (LONG i = 0; i < nCount; i++)
        {
            aDibHandle = (*vValues)[i];
            {
                DTWAINArrayLL_RAII arr(aDibHandle);
                auto vDibs = EnumeratorVectorPtr<HANDLE>(aDibHandle);

                if (!vDibs)
                    continue;

                LONG nCount2 = static_cast<LONG>(vDibs->size());
                HANDLE hDib;
                // Leave last DIB.  This is the one in the clipboard!!
                LONG nLastDib = nCount2;
                if (i == nCount - 1)
                    nLastDib = nCount2 - 1;

                for (int j = 0; j < nLastDib; j++)
                {
                    hDib = DTWAIN_GetAcquiredImage(aDibs, i, j);
                    if (CTL_TwainDLLHandle::s_TwainMemoryFunc->LockMemory(hDib))
                    {
                        CTL_TwainDLLHandle::s_TwainMemoryFunc->UnlockMemory(hDib);
                        CTL_TwainDLLHandle::s_TwainMemoryFunc->FreeMemory(hDib);
                    }
                }
            }
        }
    }
    else
    if (!aDibs)
        LOG_FUNC_EXIT_PARAMS(NULL)

    if (!bDiscardDibs)
        LOG_FUNC_EXIT_PARAMS(aDibs)

    aDibs = (HANDLE)1;
    LOG_FUNC_EXIT_PARAMS(aDibs)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

DTWAIN_ACQUIRE dynarithmic::DTWAIN_LLAcquireToClipboard(SourceAcquireOptions& opts)
{
    LONG lTransferMode = opts.getTransferMode();
    if (lTransferMode != DTWAIN_USENATIVE && lTransferMode != DTWAIN_USEBUFFERED)
        lTransferMode = DTWAIN_USENATIVE;
    opts.setActualAcquireType(TWAINAcquireType_Clipboard).setTransferMode(lTransferMode);
    return LLAcquireImage(opts);
}
