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
#include "dtwain.h"
#include "ctliface.h"
#include "ctltwmgr.h"
using namespace dynarithmic;

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_InitImageFileAppend(LPCTSTR szFile, LONG fType)
{
    LOG_FUNC_ENTRY_PARAMS((szFile, fType ))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    if ( !IsDLLHandleValid( pHandle, FALSE ) )
        LOG_FUNC_EXIT_PARAMS(false)
    if ( !pHandle->m_pDummySource )
        pHandle->m_pDummySource = CTL_ITwainSource::Create(NULL, _T("DTWAIN DummySource"));

    // Test destruction here
    CTL_ITwainSource::Destroy(pHandle->m_pDummySource);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(DTWAIN_ERR_BAD_HANDLE)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddFileToAppend(LPCTSTR /* szFile*/) { return FALSE; }
