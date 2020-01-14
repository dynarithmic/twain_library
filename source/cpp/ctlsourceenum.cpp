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

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumSources(LPDTWAIN_ARRAY Array)
{
    LOG_FUNC_ENTRY_PARAMS((Array))
        DTWAIN_ARRAY aSource = NULL;
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    // Terminate if Array is NULL )
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !Array; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    aSource = DTWAIN_ArrayCreate(DTWAIN_ARRAYSOURCE, 0);
    if (!aSource)
        LOG_FUNC_EXIT_PARAMS(false)
    DTWAIN_ARRAY pDTWAINArray = aSource;

    EnumeratorFunctionImpl::ClearEnumerator(pDTWAINArray);

    CTL_TwainSourceArray SourceArray;

    // Start a session if not already started
    if (!pHandle->m_bSessionAllocated)
    {
        if (!DTWAIN_StartTwainSession(NULL, NULL))
            LOG_FUNC_EXIT_PARAMS(false)
    }

    struct EnumAddValue {
        DTWAIN_ARRAY m_Arr; EnumAddValue(DTWAIN_ARRAY Arr) : m_Arr(Arr) {}
        void operator()(CTL_ITwainSource* ptr) { EnumeratorFunctionImpl::EnumeratorAddValue(m_Arr, &ptr); }
    };

    CTL_TwainAppMgr::EnumSources(pHandle->m_Session, SourceArray);
    for_each(SourceArray.begin(), SourceArray.end(), [&](CTL_ITwainSource* ptr) {EnumeratorFunctionImpl::EnumeratorAddValue(pDTWAINArray, &ptr);});
    *Array = aSource;
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}
