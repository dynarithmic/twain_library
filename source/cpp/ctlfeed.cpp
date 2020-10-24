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
#include <cstring>
#include "dtwain.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "ctlsupport.h"
#include <algorithm>
#include "errorcheck.h"

using namespace dynarithmic;

typedef void (CTL_ITwainSource::*SetFunc)(bool);
typedef bool (CTL_ITwainSource::*IsEnabledFunc)() const;

static bool EnableFeederFunc(DTWAIN_SOURCE Source, LONG lCap, CTL_ITwainSource* p, SetFunc Func,
                             bool bSet);
static bool IsFeederEnabledFunc(DTWAIN_SOURCE Source, IsEnabledFunc Func);
static bool ExecuteFeederState5Func(DTWAIN_SOURCE Source, LONG lCap);
static VOID CALLBACK ThisTimerProc(HWND hwnd, UINT uMsg, ULONG idEvent,DWORD dwTime);

DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_IsFeederSupported(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))

    // Check if feeder has been enabled.  If so, then device has a feeder
    DTWAIN_ARRAY arr = 0;
    BOOL bOk = DTWAIN_GetCapValues(Source, DTWAIN_CV_CAPFEEDERENABLED, DTWAIN_CAPGETCURRENT, &arr);
    DTWAINArrayLL_RAII a(arr);
    if (!bOk)
        LOG_FUNC_EXIT_PARAMS(false)

    LONG val;
    DTWAIN_ArrayGetAtLong(arr, 0, &val);
    if (val == 1)
        LOG_FUNC_EXIT_PARAMS(true);

    // Enable the feeder temporarily. 
    BOOL bRet = DTWAIN_SetCapValues(Source, DTWAIN_CV_CAPFEEDERENABLED, DTWAIN_CAPSET, arr);
    if (!bRet)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_ArraySetAtLong(arr, 0, 0);
    bRet = DTWAIN_SetCapValues(Source, DTWAIN_CV_CAPFEEDERENABLED, DTWAIN_CAPSET, arr);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_IsFeederLoaded(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    if ( DTWAIN_IsFeederSupported(Source) )
    {
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
        DTWAIN_BOOL bReturn;
        DTWAIN_ARRAY a = 0;
        bReturn = DTWAIN_GetCapValues(Source, DTWAIN_CV_CAPFEEDERLOADED, DTWAIN_CAPGET, &a);
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !bReturn;}, DTWAIN_ERR_NO_FEEDER_QUERY, false, FUNC_MACRO);
        DTWAINArrayLL_RAII arr(a);
        auto vFeeder = EnumeratorVectorPtr<LONG>(a);
        LONG Val = 0;
        if ( vFeeder && !vFeeder->empty() )
            Val = (*vFeeder)[0];
        LOG_FUNC_EXIT_PARAMS(Val)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

//*********************************************************************************************
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_IsFeederEnabled(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    if (DTWAIN_IsFeederSupported(Source))
    {
        DTWAIN_BOOL bRet = IsFeederEnabledFunc(Source, &CTL_ITwainSource::IsFeederEnabledMode);
        LOG_FUNC_EXIT_PARAMS(bRet)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_IsAutoFeedEnabled(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    if (DTWAIN_IsAutoFeedSupported(Source))
    {
        DTWAIN_BOOL bRet = IsFeederEnabledFunc(Source, &CTL_ITwainSource::GetAutoFeedMode);
        LOG_FUNC_EXIT_PARAMS(bRet)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

bool IsFeederEnabledFunc(DTWAIN_SOURCE Source, IsEnabledFunc Func)
{
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );
    bool bRet = false;
    if (p)
        return bRet = (p->*Func)();
    return false;
}
//*********************************************************************************************
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_EnableFeeder(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet)
{
    LOG_FUNC_ENTRY_PARAMS((Source, bSet))
    // Check if feeder supported
    if ( !DTWAIN_IsFeederSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    // Call general function to enable feeder
    bool bRet = EnableFeederFunc(Source, DTWAIN_CV_CAPFEEDERENABLED, p,
                            &CTL_ITwainSource::SetFeederEnableMode, bSet?true:false);

    // Enable the feeder state 5 and 6 functionality here
    if ( bSet )
    {
        DTWAIN_ARRAY A = 0;
        LONG Caps[] = { DTWAIN_CV_CAPFEEDPAGE, DTWAIN_CV_CAPREWINDPAGE, DTWAIN_CV_CAPCLEARPAGE };
        if (!DTWAIN_IsCapSupported(Source, DTWAIN_CV_CAPEXTENDEDCAPS))
            LOG_FUNC_EXIT_PARAMS(bRet)
        DTWAIN_BOOL bOk = DTWAIN_GetCapValues( Source, DTWAIN_CV_CAPEXTENDEDCAPS, DTWAIN_CAPGETCURRENT, &A);
        if ( bOk && A )
        {
            DTWAINArrayLL_RAII arr(A);
            auto& vCaps = EnumeratorVector<LONG>(A);
            size_t oldSize = vCaps.size();
            std::for_each(std::begin(Caps), std::end(Caps), [&](LONG n)
            {
                if (p->IsExtendedCapNegotiable(n) && std::find(vCaps.begin(), vCaps.end(), n) == vCaps.end())
                    vCaps.push_back(n);
            });
            if (vCaps.size() > oldSize)
                DTWAIN_SetCapValues( Source, DTWAIN_CV_CAPEXTENDEDCAPS, DTWAIN_CAPSET, A);
        }
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableAutoFeed(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet)
{
    LOG_FUNC_ENTRY_PARAMS((Source, bSet))
    // Check if feeder supported
    if ( !DTWAIN_IsAutoFeedSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    bool bRet = EnableFeederFunc(Source, DTWAIN_CV_CAPAUTOFEED, p,
                        &CTL_ITwainSource::SetAutoFeedMode, bSet?true:false);
    // Call general function to enable feeder
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

bool EnableFeederFunc(DTWAIN_SOURCE Source, LONG lCap, CTL_ITwainSource* p,
                      SetFunc Func, bool bSet)
{

    // Set the capability value
    DTWAIN_ARRAY a = 0;

    // Check the current value
    DTWAIN_BOOL bReturn = DTWAIN_GetCapValues(Source, lCap, DTWAIN_CAPGETCURRENT, &a);
    if ( !bReturn )
        return false;
    DTWAINArrayLL_RAII arr(a);
    auto vFeeder = EnumeratorVectorPtr<LONG>(a);

    if ( !vFeeder || vFeeder->empty() )
        return false;

    bool Val = ((*vFeeder)[0])?true:false;
    bool bValue=false;
    if ( bSet != 0 )
        bValue = 1;

    // Already set, so return
    if ( bValue == Val )
    {
        (p->*Func)(bValue);
        return true;
    }

    // Set here
    (*vFeeder)[0] = bValue;
    bReturn = DTWAIN_SetCapValues( Source, lCap, DTWAIN_CAPSETCURRENT, a)?true:false;
    if ( bReturn )
        (p->*Func)(bValue);
    else
        (p->*Func)(FALSE);
    return true;
}
//*********************************************************************************************

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsFeederSensitive(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    if ( !DTWAIN_IsFeederEnabled(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    if ( DTWAIN_IsCapSupported(Source, DTWAIN_CV_CAPFEEDERLOADED) )
        LOG_FUNC_EXIT_PARAMS(true)

    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}
//*********************************************************************************************
// Only called in State 5
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_FeedPage(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    if ( !DTWAIN_IsFeederEnabled(Source) )
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_BOOL bRet = ExecuteFeederState5Func(Source, DTWAIN_CV_CAPFEEDPAGE)?1:0;
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_RewindPage(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    if ( !DTWAIN_IsFeederEnabled(Source) )
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_BOOL bRet = ExecuteFeederState5Func(Source, DTWAIN_CV_CAPREWINDPAGE)?1:0;
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_ClearPage(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    if ( !DTWAIN_IsFeederEnabled(Source) )
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_BOOL bRet = ExecuteFeederState5Func(Source, DTWAIN_CV_CAPCLEARPAGE)?1:0;
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

LONG DLLENTRY_DEF DTWAIN_GetFeederFuncs(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    LONG Caps[] = { DTWAIN_CV_CAPFEEDPAGE, DTWAIN_CV_CAPCLEARPAGE, DTWAIN_CV_CAPREWINDPAGE };
    LONG ReturnVal = 0;
    for ( int i = 0; i < 3; i++ )
    {
        if (DTWAIN_IsCapSupported(Source,Caps[i]))
            ReturnVal |= (i << 1);
    }
    LOG_FUNC_EXIT_PARAMS(ReturnVal)
    CATCH_BLOCK(0)
}

bool ExecuteFeederState5Func(DTWAIN_SOURCE Source, LONG lCap)
{
    if ( !DTWAIN_IsCapSupported(Source, lCap) )
        return false;

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    VerifySourceHandle( pHandle, Source );
    DTWAIN_ARRAY a = 0;
    a = DTWAIN_ArrayCreateFromCap(NULL,lCap,0);
    if ( !a )
        return false;
    DTWAINArrayLL_RAII aRAII(a);
    auto& vCaps = EnumeratorVector<LONG>(a);
    vCaps.push_back(true);
    DTWAIN_SetCapValues(Source, lCap, DTWAIN_CAPSET, a);
    return false;
}

///////////////////////////////////////////////////////////
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableAutoFeedNotify(LONG Latency, DTWAIN_BOOL bEnable)
{
    LOG_FUNC_ENTRY_PARAMS((Latency, bEnable))
    #ifdef _WIN32
    if ( bEnable )
    {
        CTL_TwainDLLHandle::s_nTimerID = ::SetTimer(NULL, 0, (UINT)Latency, (TIMERPROC)ThisTimerProc);
        CTL_TwainDLLHandle::s_bTimerIDSet = true;
    }
    else
    {
        ::KillTimer(NULL, CTL_TwainDLLHandle::s_nTimerID);
        CTL_TwainDLLHandle::s_bTimerIDSet = false;
    }
    #endif
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

VOID CALLBACK ThisTimerProc(HWND, UINT, ULONG idEvent, DWORD)
{
    #ifdef _WIN32
    // Make sure that user set a callback
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_CALLBACK_PROC pFn = DTWAIN_GetCallback();
    if ( !pFn )
        return;

    if ( idEvent == CTL_TwainDLLHandle::s_nTimerID )
    {
        // Check if any open source supports feeder
        auto it = CTL_TwainDLLHandle::s_aFeederSources.begin();
        auto it2 = CTL_TwainDLLHandle::s_aFeederSources.end();

        while ( it != it2 )
        {
            if ( DTWAIN_IsFeederLoaded( *it ) &&
                 DTWAIN_IsFeederSensitive( *it ) &&
                 !DTWAIN_IsSourceAcquiring( *it))
            {
                (*pFn)(DTWAIN_TN_FEEDERLOADED, (LPARAM)(*it), pHandle->m_lCallbackData);
            }
            ++it;
        }
    }
    #endif
}
