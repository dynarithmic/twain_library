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
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

static DTWAIN_BOOL DTWAIN_CloseSourceUnconditional(CTL_TwainDLLHandle *pHandle, CTL_ITwainSource *pSource);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_CloseSource(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_ITwainSource *p = VerifySourceHandle(GetDTWAINHandle_Internal(), Source);
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    bool bRetval = false;
    if (p)
    {
        CTL_StringType sProductName = p->GetProductName();
        bRetval = DTWAIN_CloseSourceUnconditional(pHandle, p)?true:false;
        if (bRetval)
            pHandle->m_mapStringToSource.erase(sProductName);
    }
    LOG_FUNC_EXIT_PARAMS(bRetval)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_CloseSourceUI(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_ITwainSource *p = VerifySourceHandle(GetDTWAINHandle_Internal(), Source);
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    if (p)
    {
        CTL_ITwainSession *pSession = (CTL_ITwainSession *)pHandle->m_Session;
        CTL_TwainAppMgr::EndTwainUI(pSession, p);
        LOG_FUNC_EXIT_PARAMS(true)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DTWAIN_CloseSourceUnconditional(CTL_TwainDLLHandle *pHandle, CTL_ITwainSource *p)
{
    LOG_FUNC_ENTRY_PARAMS(())
    bool bRetval = false;

    if (p)
    {
        if (pHandle->m_nSourceCloseMode == DTWAIN_SourceCloseModeFORCE &&
            p->IsAcquireAttempt())
        {
            CTL_TwainAppMgr::DisableUserInterface(p);
            p->SetAcquireAttempt(false);
        }
        else
            DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return p->IsAcquireAttempt(); },
            DTWAIN_ERR_SOURCE_ACQUIRING, false, FUNC_MACRO);

        bRetval = CTL_TwainAppMgr::CloseSource(pHandle->m_Session, p)?true:false;
    }
    LOG_FUNC_EXIT_PARAMS(bRetval)
    CATCH_BLOCK(false)
}
