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
#include "errorcheck.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetMultipageScanMode(DTWAIN_SOURCE Source, LONG ScanType)
{
    LOG_FUNC_ENTRY_PARAMS((Source, ScanType))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    bool bSaveIncomplete = (ScanType & DTWAIN_FILESAVE_SAVEINCOMPLETE) ? true : false;

    // remove the DTWAIN_FILESAVE_INCOMPLETE mask
    ScanType = ScanType &~DTWAIN_FILESAVE_SAVEINCOMPLETE;

    if (ScanType != DTWAIN_FILESAVE_DEFAULT &&
        ScanType != DTWAIN_FILESAVE_SOURCECLOSE &&
        ScanType != DTWAIN_FILESAVE_UICLOSE &&
        ScanType != DTWAIN_FILESAVE_ENDACQUIRE &&
        ScanType != DTWAIN_FILESAVE_MANUALSAVE)
        ScanType = DTWAIN_FILESAVE_DEFAULT;
    // Flush any pages
    if (pSource->IsMultiPageModeContinuous())
        pSource->ProcessMultipageFile();

    // Set the scan mode
    pSource->SetMultiPageScanMode(ScanType);

    // Set the flag to save if file is incomplete (multipage scan cancelled)
    pSource->SetFileIncompleteSaveMode(bSaveIncomplete);

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FlushAcquiredPages(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)
    if (pSource->IsMultiPageModeContinuous())
        pSource->ProcessMultipageFile();

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}
