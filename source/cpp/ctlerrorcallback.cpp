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

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetErrorCallback(DTWAIN_ERROR_PROC logProc, LONG UserData)
{
    LOG_FUNC_ENTRY_PARAMS((logProc, UserData))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    pHandle->m_pErrorProcFn = logProc;
    pHandle->m_lErrorProcUserData = UserData;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false);
}

DTWAIN_ERROR_PROC DLLENTRY_DEF DTWAIN_GetErrorCallback(VOID_PROTOTYPE)
{
    LOG_FUNC_ENTRY_PARAMS(())
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    DTWAIN_ERROR_PROC theProc = pHandle->m_pErrorProcFn;
    LOG_FUNC_EXIT_PARAMS(theProc)
    CATCH_BLOCK(DTWAIN_ERROR_PROC(0))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetErrorCallback64(DTWAIN_ERROR_PROC64 logProc, LONG64 UserData)
{
    LOG_FUNC_ENTRY_PARAMS((logProc, UserData))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    pHandle->m_pErrorProcFn64 = logProc;
    pHandle->m_lErrorProcUserData64 = UserData;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false);
}

DTWAIN_ERROR_PROC64 DLLENTRY_DEF DTWAIN_GetErrorCallback64(VOID_PROTOTYPE)
{
    LOG_FUNC_ENTRY_PARAMS(())
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    DTWAIN_ERROR_PROC64 theProc = pHandle->m_pErrorProcFn64;
    LOG_FUNC_EXIT_PARAMS(theProc)
    CATCH_BLOCK(DTWAIN_ERROR_PROC64(0))
}

