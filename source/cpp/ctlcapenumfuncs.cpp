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
#include "ctltrall.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "ctltmpl5.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

/////////////////////////////////////////////////////////////////////////
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumCustomCapsEx2(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    DTWAIN_ARRAY pArray = 0;
    DTWAIN_EnumCustomCaps(Source, &pArray);
    LOG_FUNC_EXIT_PARAMS(pArray)
    CATCH_BLOCK(DTWAIN_ARRAY(nullptr))
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumSupportedCaps(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Array)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Array))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return !CTL_TwainAppMgr::IsSourceOpen(p); },
        DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    if (Array)
    {
        if (EnumeratorFunctionImpl::EnumeratorIsValid(*Array))
            EnumeratorFunctionImpl::ClearEnumerator(*Array);
    }

    DTWAIN_ARRAY ThisArray = 0;
    ThisArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 0);
    DTWAINArrayLL_RAII arr(ThisArray);
    auto vCaps = EnumeratorVectorPtr<LONG>(ThisArray);

    if (p && ThisArray)
    {
        if (p->RetrievedAllCaps())
        {
            // Check if this source has had capabilities negotiated and tested
            CTL_StringType strProdName;
            strProdName = p->GetProductName();
            int nWhere;
            FindFirstValue(strProdName, &pHandle->m_aSourceCapInfo, &nWhere);

            if (nWhere != -1) // Already negotiated
            {
                // Get the cap array values
                CTL_SourceCapInfo Info = pHandle->m_aSourceCapInfo[nWhere];
                CTL_CapInfoArray *pCapInfoArray = std::get<1>(Info).get();
                std::for_each(pCapInfoArray->begin(), pCapInfoArray->end(), [&vCaps](const CTL_CapInfoArray::value_type& CapInfo)
                                { vCaps->push_back(static_cast<int>(std::get<0>(CapInfo))); });
                *Array = DTWAIN_ArrayCreateCopy(ThisArray);
                LOG_FUNC_EXIT_PARAMS(true)
            }
        }

        // First time, so let's go
        DTWAIN_ARRAY pDTWAINArray = ThisArray;
        EnumeratorFunctionImpl::ClearEnumerator(pDTWAINArray);
        CTL_TwainCapArray rArray;

        // loop through all capabilities
        CTL_TwainAppMgr::GetCapabilities(p, rArray);

        // copy caps to our DTWAIN array
        std::copy(rArray.begin(), rArray.end(), std::back_inserter(*vCaps));
        vCaps->erase(std::remove(vCaps->begin(), vCaps->end(), 0), vCaps->end());

        // Cache this information and set source's flag that all caps were retrieved
        DTWAIN_CacheCapabilityInfo(p, pHandle, vCaps);
        p->SetRetrievedAllCaps(true);
        bool bFound = !vCaps->empty();
        *Array = DTWAIN_ArrayCreateCopy(ThisArray);
        if (bFound)
            LOG_FUNC_EXIT_PARAMS(true)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumExtendedCaps(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Array)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Array))
    DTWAIN_BOOL retVal = DTWAIN_EnumExtendedCapsEx(Source, Array);
    LOG_FUNC_EXIT_PARAMS(retVal)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumCustomCaps(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Array)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Array))
    if (!DTWAIN_EnumSupportedCapsEx(Source, Array))
    LOG_FUNC_EXIT_PARAMS(false)
    DTWAIN_ARRAY ThisArray = (DTWAIN_ARRAY)(*Array);

    auto vCaps = EnumeratorVectorPtr<LONG>(ThisArray);
    if (vCaps)
        vCaps->erase(std::remove_if(vCaps->begin(), vCaps->end(), [&](LONG nCap) { return static_cast<unsigned>(nCap) < DTWAIN_CV_CAPCUSTOMBASE;}), vCaps->end());
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCapOperations(DTWAIN_SOURCE Source, LONG lCapability, LPLONG  lpOps)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCapability, lpOps))
    if (!DTWAIN_IsCapSupported(Source, lCapability))
        LOG_FUNC_EXIT_PARAMS(false)

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    CTL_CapInfo CapInfo = GetCapInfo(pHandle, p, (TW_UINT16)lCapability);
    *lpOps = std::get<4>(CapInfo);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAllCapsToDefault(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);

    if (!p)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (!CTL_TwainAppMgr::IsSourceOpen(p)); },
        DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);
    {
        DTWAIN_ARRAY a = 0;
        DTWAINArrayPtr_RAII arr(&a);
        DTWAIN_EnumSupportedCaps(Source, &a);
    }

    CTL_CapInfoArrayPtr pArray = GetCapInfoArray(pHandle, p);

    vector<int> Array;
    Array.push_back(0);
    for_each(pArray->begin(), pArray->end(), [&](CTL_CapInfoArray::value_type& InfoVal)
    {
        CTL_CapInfo Info = InfoVal.second;
        if (p->IsCapNegotiableInState((TW_UINT16)std::get<0>(Info), p->GetState()))
        {
            if (!SetCapabilityValues(p, std::get<0>(Info), CTL_SetTypeRESET, (UINT)TwainContainer_ONEVALUE, 0,Array)) {}
        }
    });

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

