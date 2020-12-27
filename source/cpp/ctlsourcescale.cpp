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

static bool SetImageScale(CTL_ITwainSource *p, DTWAIN_FLOAT xscale, DTWAIN_FLOAT yscale)
{
    p->SetImageScale(xscale, yscale, true);
    return true;
}

///////////////////////////////////////////////////////////////////////

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireImageScaleString(DTWAIN_SOURCE Source, LPCTSTR xscale, LPCTSTR yscale)
{
    LOG_FUNC_ENTRY_PARAMS((Source, xscale, yscale))
    DTWAIN_FLOAT xValue = StringWrapper::ToDouble(xscale);
    DTWAIN_FLOAT yValue = StringWrapper::ToDouble(yscale);
    DTWAIN_BOOL retVal = DTWAIN_SetAcquireImageScale(Source, xValue, yValue);
    LOG_FUNC_EXIT_PARAMS(retVal)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireImageNegative(DTWAIN_SOURCE Source, DTWAIN_BOOL IsNegative)
{
    LOG_FUNC_ENTRY_PARAMS((Source, IsNegative))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(false)
    p->SetImageNegative(IsNegative ? true : false);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireImageScale(DTWAIN_SOURCE Source, DTWAIN_FLOAT  xscale, DTWAIN_FLOAT  yscale)
{
    LOG_FUNC_ENTRY_PARAMS((Source, xscale, yscale))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen(p)); },
    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = SetImageScale(p, xscale, yscale);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}
