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

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsUIControllable(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    // Open the source (if source is closed)
    bool bSourceOpen = false;
    if (CTL_TwainAppMgr::IsSourceOpen(pSource))
        bSourceOpen = true;
    else
    if (!CTL_TwainAppMgr::OpenSource(pHandle->m_Session, pSource))
       LOG_FUNC_EXIT_PARAMS(false)
    bool bOk = false;

    // Check if capability UICONTROLLABLE is supported
    if (DTWAIN_IsCapSupported(Source, DTWAIN_CV_CAPUICONTROLLABLE))
    {
        // Get the capability value
        DTWAIN_ARRAY CapArray = 0;
        DTWAIN_GetCapValuesEx(Source, DTWAIN_CV_CAPUICONTROLLABLE, DTWAIN_CAPGET, DTWAIN_CONTONEVALUE, &CapArray);
        DTWAINArrayLL_RAII arr(CapArray);
        bOk = (EnumeratorVector<LONG>(CapArray)[0]) ? true : false;
    }
    else
    {
        // Source UI must be tested
        bOk = CTL_TwainAppMgr::ShowUserInterface(pSource, true);
    }
    // Close source if opened in this function
    if (!bSourceOpen)
        ::DTWAIN_CloseSource(Source);
    LOG_FUNC_EXIT_PARAMS(bOk ? TRUE : FALSE)
    CATCH_BLOCK(false)
}
