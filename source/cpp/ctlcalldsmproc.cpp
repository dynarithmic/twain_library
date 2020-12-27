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
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

TWAIN_IDENTITY DLLENTRY_DEF DTWAIN_GetTwainAppID()
{
    LOG_FUNC_ENTRY_PARAMS(())
    if (!DTWAIN_IsSessionEnabled())
         LOG_FUNC_EXIT_PARAMS(NULL);
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    CTL_ITwainSession *pSession = (CTL_ITwainSession *)pHandle->m_Session;
    TW_IDENTITY *pIdentity = pSession->GetAppIDPtr();
    LOG_FUNC_EXIT_PARAMS(((TWAIN_IDENTITY)pIdentity))
    CATCH_BLOCK(TWAIN_IDENTITY(0))
}

TWAIN_IDENTITY DLLENTRY_DEF DTWAIN_GetSourceID(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_ITwainSource *pSource = VerifySourceHandle(GetDTWAINHandle_Internal(), Source);
    TWAIN_IDENTITY Id = {};
    if ( pSource )
        Id = (TWAIN_IDENTITY)pSource->GetSourceIDPtr();
    LOG_FUNC_EXIT_PARAMS(Id)
    CATCH_BLOCK(TWAIN_IDENTITY())
}

LONG DLLENTRY_DEF DTWAIN_CallDSMProc(TWAIN_IDENTITY AppID, TWAIN_IDENTITY SourceId, LONG lDG, LONG lDAT, LONG lMSG, LPVOID pData)
{
    LOG_FUNC_ENTRY_PARAMS((AppID, SourceId, lDG, lDAT, lMSG, pData))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, -1L, FUNC_MACRO);

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!DTWAIN_IsSessionEnabled()); }, DTWAIN_ERR_NO_SESSION, -1L, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!AppID && !SourceId); }, DTWAIN_ERR_INVALID_PARAM, -1L, FUNC_MACRO);
    LONG Ret = CTL_TwainAppMgr::CallDSMEntryProc((TW_IDENTITY*)AppID,
                                                (TW_IDENTITY*)SourceId,
                                                (TW_UINT32)lDG,
                                                (TW_UINT16)lDAT,
                                                (TW_UINT16)lMSG,
                                                (TW_MEMREF)pData);

    LOG_FUNC_EXIT_PARAMS(Ret)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}
