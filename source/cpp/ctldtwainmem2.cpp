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
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

// DTWAIN 2.0 memory related functions
HANDLE  DLLENTRY_DEF DTWAIN_AllocateMemory(LONG memSize)
{
    LOG_FUNC_ENTRY_PARAMS((memSize))
    HANDLE h = ImageMemoryHandler::GlobalAlloc(GHND, memSize);
    LOG_FUNC_EXIT_PARAMS(h)
    CATCH_BLOCK(HANDLE())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FreeMemory(HANDLE h)
{
    LOG_FUNC_ENTRY_PARAMS((h))
    DTWAIN_BOOL bRet = ImageMemoryHandler::GlobalFree(h) ? TRUE : FALSE;
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_MEMORY_PTR DLLENTRY_DEF DTWAIN_LockMemory(HANDLE h)
{
    LOG_FUNC_ENTRY_PARAMS((h))
    DTWAIN_MEMORY_PTR ptr = ImageMemoryHandler::GlobalLock(h);
    LOG_FUNC_EXIT_PARAMS(ptr)
    CATCH_BLOCK(DTWAIN_MEMORY_PTR())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_UnlockMemory(HANDLE h)
{
    LOG_FUNC_ENTRY_PARAMS((h))
    DTWAIN_BOOL bRet = ImageMemoryHandler::GlobalUnlock(h);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

HANDLE  DLLENTRY_DEF DTWAIN_AllocateMemoryEx(LONG memSize)
{
    LOG_FUNC_ENTRY_PARAMS((memSize))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);
    HANDLE h = NULL;
    if (CTL_TwainDLLHandle::s_TwainMemoryFunc)
        h = CTL_TwainDLLHandle::s_TwainMemoryFunc->AllocateMemory(memSize);
    LOG_FUNC_EXIT_PARAMS(h)
    CATCH_BLOCK(HANDLE())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FreeMemoryEx(HANDLE h)
{
    LOG_FUNC_ENTRY_PARAMS((h))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    DTWAIN_BOOL bRet = FALSE;
    if (CTL_TwainDLLHandle::s_TwainMemoryFunc)
    {
        CTL_TwainDLLHandle::s_TwainMemoryFunc->FreeMemory(h);
        bRet = TRUE;
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_MEMORY_PTR DLLENTRY_DEF DTWAIN_LockMemoryEx(HANDLE h)
{
    LOG_FUNC_ENTRY_PARAMS((h))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);
    DTWAIN_MEMORY_PTR ptr = NULL;
    if (CTL_TwainDLLHandle::s_TwainMemoryFunc)
        ptr = CTL_TwainDLLHandle::s_TwainMemoryFunc->LockMemory(h);
    LOG_FUNC_EXIT_PARAMS(ptr)
    CATCH_BLOCK(DTWAIN_MEMORY_PTR())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_UnlockMemoryEx(HANDLE h)
{
    LOG_FUNC_ENTRY_PARAMS((h))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    DTWAIN_BOOL bRet = FALSE;
    if (CTL_TwainDLLHandle::s_TwainMemoryFunc)
    {
        CTL_TwainDLLHandle::s_TwainMemoryFunc->UnlockMemory(h);
        bRet = TRUE;
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}
