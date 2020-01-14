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
#include "errorcheck.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_CheckHandles(DTWAIN_BOOL bCheck)
{
    LOG_FUNC_ENTRY_PARAMS((bCheck))
    CTL_TwainDLLHandle::s_bCheckHandles = bCheck ? true : false;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

CTL_ITwainSource* dynarithmic::VerifySourceHandle(DTWAIN_HANDLE DLLHandle,  DTWAIN_SOURCE Source )
{
    LOG_FUNC_ENTRY_PARAMS((DLLHandle, Source))
    CTL_ITwainSource *p;
    if ( !CTL_TwainDLLHandle::s_bCheckHandles )
        p = static_cast<CTL_ITwainSource *>(Source);
    else
    {
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(DLLHandle);
        // See if DLL Handle exists
        DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);
        p = static_cast<CTL_ITwainSource *>(Source);

        // Check if Source is valid
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return (!p ||
            (!CTL_TwainAppMgr::IsValidTwainSource(pHandle->m_Session, p))); },
            DTWAIN_ERR_BAD_SOURCE, NULL, FUNC_MACRO);
    }
    LOG_FUNC_EXIT_PARAMS(p)
    CATCH_BLOCK(static_cast<CTL_ITwainSource *>(NULL))
}

