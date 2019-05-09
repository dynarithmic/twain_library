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

static LONG GetCustomCapDataType(DTWAIN_SOURCE Source, TW_UINT16 nCap);

LONG DLLENTRY_DEF DTWAIN_GetCapContainerEx(LONG nCap, DTWAIN_BOOL bSetContainer, LPDTWAIN_ARRAY pArray)
{
    LOG_FUNC_ENTRY_PARAMS((nCap, bSetContainer, pArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, 0, FUNC_MACRO);
    // Check if array is of the correct type
    if (pArray)
    {
        //DTWAIN_ArrayDestroy(*pArray);
        *pArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 0);
        if (*pArray == NULL)
            LOG_FUNC_EXIT_PARAMS(0L)
    }
    DTWAIN_ARRAY pDTWAINArray = 0;
    if (pArray)
        pDTWAINArray = *pArray;

    LONG lValue;
    if (nCap < CAP_CUSTOMBASE)
    {
        lValue = CTL_TwainAppMgr::GetContainerTypesFromCap((CTL_EnumCapability)nCap, bSetContainer?true:false);
        if (pDTWAINArray)
        {
            for (int i = 1; i <= 16; i++)
            {
                if (lValue & (1 << (i - 1)))
                    EnumeratorFunctionImpl::EnumeratorAddValue(pDTWAINArray, &i);
            }
        }
    }
    LOG_FUNC_EXIT_PARAMS(0xFFFFFFFF)
    CATCH_BLOCK(0)
}

LONG DLLENTRY_DEF DTWAIN_GetCapContainer(DTWAIN_SOURCE Source, LONG nCap, LONG lCapType)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nCap, lCapType))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, 0, FUNC_MACRO);
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(0L)

    CTL_CapInfoArrayPtr pArray;
    DTWAIN_BOOL bCapSupported = DTWAIN_IsCapSupported(Source, nCap);
    if (!bCapSupported)
        LOG_FUNC_EXIT_PARAMS(0L)
    pArray = GetCapInfoArray(pHandle, p);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !pArray; }, DTWAIN_ERR_NO_CAPS_DEFINED, 0L, FUNC_MACRO);

    // Get the cap array values
    auto iter = pArray->find(static_cast<TW_UINT16>(nCap));
    if (iter != pArray->end())
    {
        CTL_CapInfo CapInfo = iter->second;
        switch (lCapType)
        {
            case DTWAIN_CAPGET:
                LOG_FUNC_EXIT_PARAMS((LONG)std::get<1>(CapInfo))

            case DTWAIN_CAPGETCURRENT:
                LOG_FUNC_EXIT_PARAMS((LONG)std::get<5>(CapInfo))

            case DTWAIN_CAPGETDEFAULT:
                LOG_FUNC_EXIT_PARAMS((LONG)std::get<6>(CapInfo))

            case DTWAIN_CAPSET:
            case DTWAIN_CAPSETAVAILABLE:
            case DTWAIN_CAPSETCURRENT:
            case DTWAIN_CAPSETCONSTRAINT:
            {
                LONG Value = (LONG)std::get<2>(CapInfo);
                LONG bResult1 = 0, bResult2 = 0;
                if (Value == 0)
                    LOG_FUNC_EXIT_PARAMS(0)
                LONG nHold = 0;
                if (lCapType == DTWAIN_CAPSETAVAILABLE)
                    lCapType = DTWAIN_CAPSETCURRENT;

                if (lCapType == DTWAIN_CAPSETCURRENT)
                    nHold = std::get<5>(CapInfo);
                if (/* lCapType == DTWAIN_CAPSETCURRENT ||*/
                    lCapType == DTWAIN_CAPSET)
                    bResult1 = ((Value & TwainContainer_ONEVALUE) || (Value & TwainContainer_ARRAY));
                else
                    bResult2 = ((Value & TwainContainer_ENUMERATION) || (Value & TwainContainer_RANGE) || (Value & TwainContainer_ARRAY));
                if (!bResult1 && !bResult2 && !nHold)
                    LOG_FUNC_EXIT_PARAMS(0)
                if (!bResult1 && !bResult2 && nHold)
                    LOG_FUNC_EXIT_PARAMS(nHold)

                if (Value)
                {
                    // Check container for CAPGET
                    LONG GetContainer = (LONG)std::get<1>(CapInfo);

                    // Return if containers are the same
                    if (lCapType == DTWAIN_CAPSETCURRENT ||
                        lCapType == DTWAIN_CAPSET)
                    {
                        // Check if container for get is RANGE
                        // Return TW_ONEVALUE, since you can't set multiple
                        // values in the range with SET or SETCURRENT
                        if (GetContainer == TwainContainer_RANGE)
                            LOG_FUNC_EXIT_PARAMS(TwainContainer_ONEVALUE)

                        if (bResult1 == 0)
                            LOG_FUNC_EXIT_PARAMS(GetContainer)
                        if (Value & GetContainer &&
                            (GetContainer == TwainContainer_ONEVALUE || GetContainer == TwainContainer_ARRAY))
                            LOG_FUNC_EXIT_PARAMS(GetContainer)
                        else
                            LOG_FUNC_EXIT_PARAMS(TwainContainer_ONEVALUE)
                    }
                    else
                    {
                        if (bResult2 == 0)
                            LOG_FUNC_EXIT_PARAMS(GetContainer)
                        if (Value & GetContainer &&
                            (GetContainer == TwainContainer_ENUMERATION || GetContainer == TwainContainer_RANGE))
                                LOG_FUNC_EXIT_PARAMS(GetContainer)
                        else
                            LOG_FUNC_EXIT_PARAMS(Value & ~GetContainer)
                    }
                    // Set container does not match get container :-(
                    // No choice but to return other container type and hope
                    // that Source handles wrong container gracefully.
                    LOG_FUNC_EXIT_PARAMS(Value & ~GetContainer)
                }
            }
            break;

            default:
                LOG_FUNC_EXIT_PARAMS(DTWAIN_CONTONEVALUE)
        }
    }
    LOG_FUNC_EXIT_PARAMS(0L)
    CATCH_BLOCK(0)
}

LONG DLLENTRY_DEF DTWAIN_GetCapDataType(DTWAIN_SOURCE Source, LONG nCap)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nCap))
    TW_UINT16 nThisCap = (TW_UINT16)nCap;
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, -1L, FUNC_MACRO);
    if (nThisCap >= CAP_CUSTOMBASE)
    {
        if (Source == NULL)
            LOG_FUNC_EXIT_PARAMS(DTWAIN_FAILURE1)
        LONG nDataType = GetCustomCapDataType(Source, nThisCap);
        LOG_FUNC_EXIT_PARAMS(nDataType)
    }
    UINT nDataType = CTL_TwainAppMgr::GetDataTypeFromCap((CTL_EnumCapability)nCap);
    if (nDataType == 0xFFFF)
        LOG_FUNC_EXIT_PARAMS(DTWAIN_FAILURE1)
    LOG_FUNC_EXIT_PARAMS((LONG)nDataType)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG GetCustomCapDataType(DTWAIN_SOURCE Source, TW_UINT16 nCap)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nCap))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, 0, FUNC_MACRO);
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_BAD_SOURCE)

    if (!p->IsCapInSupportedList(static_cast<TW_UINT16>(nCap)))
    {
        // Try getting it the slow way
        if (!CTL_TwainAppMgr::IsCapabilitySupported(p, nCap))
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_CAP_NO_SUPPORT)
            p->AddCapToSupportedList(static_cast<TW_UINT16>(nCap));
    }

    CTL_CapInfoArrayPtr pArray;
    DTWAIN_CacheCapabilityInfo(p, pHandle, nCap);
    pArray = GetCapInfoArray(pHandle, p);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !pArray; }, DTWAIN_ERR_NO_CAPS_DEFINED, 0L, FUNC_MACRO);

    // Get the cap array values
    CTL_CapInfo CapInfo;
    auto iter = pArray->find(static_cast<TW_UINT16>(nCap));
    if (iter != pArray->end())
    {
        CapInfo = iter->second;
        LONG nValue = (LONG)std::get<3>(CapInfo);
        if (nValue == DTWAIN_CAPDATATYPE_UNKNOWN)
            nValue = DTWAIN_ERR_UNKNOWN_CAPDATATYPE;
        LOG_FUNC_EXIT_PARAMS(nValue) // Capability data type value
    }
    LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_UNKNOWN_CAPDATATYPE)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG DLLENTRY_DEF DTWAIN_GetCapArrayType(DTWAIN_SOURCE Source, LONG nCap)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nCap))
    LONG lDataType = DTWAIN_GetCapDataType(Source, nCap);
    if (lDataType == DTWAIN_FAILURE1)
        LOG_FUNC_EXIT_PARAMS(DTWAIN_FAILURE1)
    UINT nDataType = lDataType;
    switch (nDataType)
    {
        case TWTY_STR32:
        case TWTY_STR64:
        case TWTY_STR128:
        case TWTY_STR255:
        case TWTY_STR1024:
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ARRAYSTRING)

        case TWTY_FRAME:
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ARRAYFRAME)

        case TWTY_FIX32:
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ARRAYFLOAT)

        default:
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ARRAYLONG)
    }
    LOG_FUNC_EXIT_PARAMS(DTWAIN_FAILURE1)// DTWAIN_ArrayTypeINVALID;
    CATCH_BLOCK(DTWAIN_FAILURE1)
}
