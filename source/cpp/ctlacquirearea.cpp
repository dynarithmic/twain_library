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

    For more information, the license file named LICENSE that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include <boost/format.hpp>
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

static bool GetImageSize(DTWAIN_SOURCE Source,
    LPDTWAIN_ARRAY FloatEnum,
    CTL_EnumGetType GetType);

static bool GetImageSize2(CTL_ITwainSource *p,
    LPDTWAIN_FLOAT left,
    LPDTWAIN_FLOAT top,
    LPDTWAIN_FLOAT right,
    LPDTWAIN_FLOAT bottom,
    LPLONG Unit);

static bool SetImageSize(DTWAIN_SOURCE Source,
    DTWAIN_ARRAY FloatEnum,
    DTWAIN_ARRAY ActualEnum,
    CTL_EnumSetType SetType);

static bool SetImageSize2(CTL_ITwainSource *p,
    DTWAIN_FLOAT left,
    DTWAIN_FLOAT top,
    DTWAIN_FLOAT right,
    DTWAIN_FLOAT bottom,
    LONG Unit,
    LONG flags);

static bool IsValidUnit(LONG Unit)
{
    return (Unit == DTWAIN_INCHES ||
        Unit == DTWAIN_CENTIMETERS ||
        Unit == DTWAIN_PICAS ||
        Unit == DTWAIN_POINTS ||
        Unit == DTWAIN_TWIPS ||
        Unit == DTWAIN_PIXELS);
}

///////////////////////////////////////////////////////////////////////
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireArea(DTWAIN_SOURCE Source, LONG lGetType, LPDTWAIN_ARRAY FloatEnum)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lGetType, FloatEnum))
    DTWAIN_BOOL bRet = GetImageSize(Source, FloatEnum, (CTL_EnumGetType)lGetType);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireArea(DTWAIN_SOURCE Source, LONG lSetType, DTWAIN_ARRAY FloatEnum, DTWAIN_ARRAY ActualEnum)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lSetType, FloatEnum, ActualEnum))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);

    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen(p)); },
    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = SetImageSize(Source, FloatEnum, ActualEnum,(CTL_EnumSetType)lSetType);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireArea2String(DTWAIN_SOURCE Source, LPCTSTR left, LPCTSTR top, LPCTSTR right, LPCTSTR bottom, LONG Unit, LONG flags)
{
    LOG_FUNC_ENTRY_PARAMS((Source, left, top, right, bottom, Unit, flags))
    DTWAIN_FLOAT val1 = StringWrapper::ToDouble(left);
    DTWAIN_FLOAT val2 = StringWrapper::ToDouble(top);
    DTWAIN_FLOAT val3 = StringWrapper::ToDouble(right);
    DTWAIN_FLOAT val4 = StringWrapper::ToDouble(bottom);
    DTWAIN_BOOL bRet = DTWAIN_SetAcquireArea2(Source, val1, val2, val3, val4, Unit, flags);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireArea2(DTWAIN_SOURCE Source, DTWAIN_FLOAT left, DTWAIN_FLOAT top, DTWAIN_FLOAT right, DTWAIN_FLOAT bottom, LONG Unit, LONG flags)
{
    LOG_FUNC_ENTRY_PARAMS((Source, left, top, right, bottom, Unit, flags))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen(p)); },
                                    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!IsValidUnit(Unit)); },
                                    DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    DTWAIN_BOOL bRet = SetImageSize2(p, left, top, right, bottom, Unit, flags);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireArea2String(DTWAIN_SOURCE Source, LPTSTR left, LPTSTR top, LPTSTR right, LPTSTR bottom, LPLONG Unit)
{
    LOG_FUNC_ENTRY_PARAMS((Source, left, top, right, bottom, Unit))
    DTWAIN_FLOAT val[4];
    LPTSTR* pStr[] = { &left, &top, &right, &bottom };
    DTWAIN_BOOL bRet = DTWAIN_GetAcquireArea2(Source, &val[0], &val[1], &val[2], &val[3], Unit);
    if (bRet)
    {
        CTL_StringStreamType strm;
        for (int i = 0; i < 4; ++i)
        {
            strm << BOOST_FORMAT(_T("%1%")) % val[i];
            StringWrapper::SafeStrcpy(*pStr[i], strm.str().c_str());
            strm.str(_T(""));
        }
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireArea2(DTWAIN_SOURCE Source, LPDTWAIN_FLOAT  left, LPDTWAIN_FLOAT  top, LPDTWAIN_FLOAT  right, LPDTWAIN_FLOAT  bottom, LPLONG Unit)
{
    LOG_FUNC_ENTRY_PARAMS((Source, left, top, right, bottom, Unit))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen(p)); },
    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = GetImageSize2(p, left, top, right, bottom, Unit);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

static bool GetImageSize(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY FloatArray, CTL_EnumGetType GetType)
{
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (p)
    {
        DTWAIN_ARRAY FloatEnum = DTWAIN_ArrayCreate(DTWAIN_ARRAYFLOAT, 4);
        if (!FloatEnum)
            return false;
        DTWAINArrayLL_RAII a(FloatEnum);

        CTL_RealArray Array;
        if (GetType == CTL_GetTypeGETCURRENT)
            GetType = CTL_GetTypeGET;

        bool bOk = CTL_TwainAppMgr::GetImageLayoutSize(p, Array, GetType);
        if (!bOk)
            return false;
        auto& vValues = EnumeratorVector<double>(FloatEnum);
        std::copy(Array.begin(), Array.end(), vValues.begin());
        DTWAIN_ARRAY Temp = DTWAIN_ArrayCreateCopy(FloatEnum);
        if  (EnumeratorFunctionImpl::EnumeratorIsValidNoCheck(*FloatArray))
            EnumeratorFunctionImpl::EnumeratorDestroy(*FloatArray);
        if (Temp)
            *FloatArray = Temp;
        if (Temp)
            return true;
    }
    return false;
}


static bool SetImageSize(DTWAIN_SOURCE Source, DTWAIN_ARRAY FloatEnum, DTWAIN_ARRAY ActualEnum, CTL_EnumSetType SetType)
{
    LOG_FUNC_ENTRY_PARAMS((Source, FloatEnum, ActualEnum, SetType))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (p)
    {
        DTWAIN_ARRAY pArray = FloatEnum;
        DTWAIN_ARRAY pArrayActual = DTWAIN_ArrayInit();
        DTWAIN_Check_Error_Condition_0_Ex(pHandle,
            [&] { return (!EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, CTL_EnumeratorDoubleType)); },
            DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

        auto vFloatEnum = EnumeratorVectorPtr<double>(FloatEnum);
        auto vActualEnum = EnumeratorVectorPtr<double>(ActualEnum);

        if (!vFloatEnum || vFloatEnum->size() < 4)
            LOG_FUNC_EXIT_PARAMS(false)

        if (ActualEnum != nullptr)
        {
            pArrayActual = ActualEnum;
            DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                [&] {return (!EnumeratorFunctionImpl::EnumeratorIsValidEx(pArrayActual, CTL_EnumeratorDoubleType)); },
                DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);
            vActualEnum->clear();
        }

        CTL_RealArray Array;
        CTL_RealArray rArray;
        std::copy(vFloatEnum->begin(), vFloatEnum->begin() + 4, std::back_inserter(Array));
        bool bOk = CTL_TwainAppMgr::SetImageLayoutSize(p, Array, rArray, SetType);
        if (bOk && pArrayActual)
            std::copy(rArray.begin(), rArray.begin() + 4, std::back_inserter(*vActualEnum));
        LOG_FUNC_EXIT_PARAMS(bOk)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}


static bool SetImageSize2(CTL_ITwainSource *p,
                            DTWAIN_FLOAT left,
                            DTWAIN_FLOAT top,
                            DTWAIN_FLOAT right,
                            DTWAIN_FLOAT bottom,
                            LONG Unit,
                            LONG flags)
{
    LOG_FUNC_ENTRY_PARAMS((p, left, top, right, bottom, Unit, flags))
    p->SetAlternateAcquireArea(left, top, right, bottom, flags, Unit, flags ? true : false);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

static bool GetImageSize2(CTL_ITwainSource *p,
                            LPDTWAIN_FLOAT left,
                            LPDTWAIN_FLOAT top,
                            LPDTWAIN_FLOAT right,
                            LPDTWAIN_FLOAT bottom,
                            LPLONG Unit)
{
    LOG_FUNC_ENTRY_PARAMS((p, left, top, right, bottom, Unit))
    FloatRect r;
    LONG flags;
    p->GetAlternateAcquireArea(r, *Unit, flags);
    if (!flags)
    {
        *Unit = -1;
        *left = -1.0;
        *right = -1.0;
        *top = -1.0;
        *bottom = -1.0;
    }
    else
    {
        *left = r.left;
        *top = r.top;
        *right = r.right;
        *bottom = r.bottom;
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}
