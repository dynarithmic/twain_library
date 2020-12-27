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
#include "ctltmpl5.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

CTL_CapInfoArrayPtr dynarithmic::GetCapInfoArray(CTL_TwainDLLHandle* pHandle, CTL_ITwainSource *p)
{
    // Check if the capability is supported
    CTL_StringType strProdName = p->GetProductName();

    // Find where capability setting is
    int nWhere;
    FindFirstValue(strProdName, &pHandle->m_aSourceCapInfo, &nWhere);
    if (nWhere == -1)
        return CTL_CapInfoArrayPtr();

    // Get the cap array values
    CTL_SourceCapInfo Info = pHandle->m_aSourceCapInfo[nWhere];
    CTL_CapInfoArrayPtr pArray = std::get<1>(Info);
    return pArray;
}

CTL_CapInfo dynarithmic::GetCapInfo(CTL_TwainDLLHandle* pHandle, CTL_ITwainSource *p, TW_UINT16 nCap)
{
    CTL_CapInfoArrayPtr pArray = GetCapInfoArray(pHandle, p);
    CTL_CapInfo CapInfo;
    if (!pArray)
    {
        CapInfo.SetValid(false);
        return CapInfo;
    }
    auto iter = pArray->find(static_cast<TW_UINT16>(nCap));
    if (iter != pArray->end())
    {
        CapInfo = iter->second;
        CapInfo.SetValid(true);
        return CapInfo;
    }
    CapInfo.SetValid(false);
    return CapInfo;
}
