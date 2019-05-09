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
#include <cstring>
#include "dtwain.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "ctlsupport.h"

using namespace dynarithmic;

///////////////////////////////////////////////////////////////////////////
bool dynarithmic::GetSupportString(DTWAIN_SOURCE Source, LPTSTR sz, LONG nLen, LONG Cap, LONG GetType)
{
    if ( nLen > 0 )
        sz[0] = _T('\0');
    DTWAIN_ARRAY Array = 0;
    bool bRet = DTWAIN_GetCapValues(Source, Cap, GetType, &Array)?true:false;
    DTWAINArrayLL_RAII raii(Array);
    CTL_StringType sVal;
    if ( bRet )
    {
        EnumeratorFunctionImpl::EnumeratorGetAt(Array, 0, &sVal);
        CopyInfoToCString(sVal,sz,nLen);
    }
    return bRet;
}


bool dynarithmic::EnumSupported(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG Cap)
{
    bool bRet = DTWAIN_GetCapValues(Source, Cap, DTWAIN_CAPGET, pArray)?true:false;
    return bRet;
}


bool dynarithmic::SetSupportArray(DTWAIN_SOURCE Source, DTWAIN_ARRAY Array, LONG Cap)
{
    return DTWAIN_SetCapValues(Source, Cap, DTWAIN_CAPSETAVAILABLE, Array)?true:false;
}

bool dynarithmic::GetSupportArray(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Array, LONG Cap, LONG GetType/*=DTWAIN_CAPGET*/)
{
    return DTWAIN_GetCapValues(Source, Cap, GetType, Array)?true:false;
}

LONG dynarithmic::CheckEnabled(DTWAIN_SOURCE Source, LONG CapVal)
{
    LONG IsEnabled = 0;
    GetSupport(Source, &IsEnabled, CapVal);
    return IsEnabled;
}

