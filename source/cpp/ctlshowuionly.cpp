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
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "ctltr040.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

// This function allows the user to only show the UI
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ShowUIOnly(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle,[&]{return DTWAIN_IsSourceAcquiring(Source); },
        DTWAIN_ERR_SOURCE_ACQUIRING, false, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_0_Ex(pHandle,[&]{return pSource->IsUIOpen(); },
        DTWAIN_ERR_UI_ALREADY_OPENED, false, FUNC_MACRO);

    // Open the source (if source is closed)
    bool bCloseSource = false;
    bool bIsSourceOpen = DTWAIN_IsSourceOpen(Source)?true:false;

    if (!bIsSourceOpen && (DTWAIN_GetTwainMode() == DTWAIN_MODAL))
    {
        bCloseSource = true;
        if (!DTWAIN_OpenSource(Source))
            LOG_FUNC_EXIT_PARAMS(false)
    }
    else
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !bIsSourceOpen; }, DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    // Check if capability is supported
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !DTWAIN_IsCapSupported(Source, DTWAIN_CV_CAPENABLEDSUIONLY); },
        DTWAIN_ERR_UIONLY_NOT_SUPPORTED, false, FUNC_MACRO);

    // Start a thread depending on Twain Mode.
    if (DTWAIN_GetTwainMode() == DTWAIN_MODELESS) // No thread
    {
        DTWAIN_BOOL bRet2 = CTL_TwainAppMgr::ShowUserInterface(pSource, false, true);
        LOG_FUNC_EXIT_PARAMS(bRet2)
    }

    else
    {
        // Set up a "worker thread" until we get confirmation on success or failure
        if (true)
        {
            // TWAIN 1.x loop implementation
            TwainMessageLoopV1 v1Impl(pHandle);

            // TWAIN 2.x loop implementation
            TwainMessageLoopV2 v2Impl(pHandle);

            // default to version 1
            TwainMessageLoopImpl* pImpl = &v1Impl;

            // check for version 2 implementation
            if (CTL_TwainAppMgr::IsVersion2DSMUsed())
            {
                // assign the callback procedure
                CTL_TwainDLLHandle::s_TwainCallbackSet = false;
                CTL_DSMCallbackTripletRegister callbackSetter(CTL_TwainAppMgr::GetCurrentSession(),
                    pSource,
                    &TwainMessageLoopV2::TwainVersion2MsgProc);
                if (callbackSetter.Execute() == TWRC_SUCCESS)
                    pImpl = &v2Impl;
            }

            // show the interface -- this is where we may get a message right away in the loop
            CTL_TwainAppMgr::ShowUserInterface(pSource, false, true);

            // perform the TWAIN loop now.
            pImpl->PerformMessageLoop(pSource, true);
        }
    }

    // Close the source if opened artificially
    if (bCloseSource)
        DTWAIN_CloseSource(Source);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ForceScanOnNoUI(DTWAIN_SOURCE Source, BOOL bSet)
{
    LOG_FUNC_ENTRY_PARAMS((Source, bSet))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

        // return the file name that would be acquired
    pSource->SetForceScanOnNoUI(bSet ? true : false);
    LOG_FUNC_EXIT_PARAMS(true)
   CATCH_BLOCK(false);
}

