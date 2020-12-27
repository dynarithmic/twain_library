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
#include "ctltr034.h"
#include "ctltwmgr.h"

using namespace dynarithmic;
CTL_DeviceEventTriplet::CTL_DeviceEventTriplet(CTL_ITwainSession *pSession,
                                               CTL_ITwainSource* pSource)
                                               :  CTL_TwainTriplet()
{
    SetSessionPtr(pSession);
    SetSourcePtr( pSource );

    // Get the app manager's AppID
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();
    if ( pMgr && pMgr->IsValidTwainSession( pSession ))
    {
        if ( pSource )
        {
            Init( pSession->GetAppIDPtr(),
                  pSource->GetSourceIDPtr(),
                  DG_CONTROL,
                  DAT_DEVICEEVENT,
                  MSG_GET,
                  (TW_MEMREF)((pTW_DEVICEEVENT)m_DeviceEvent));
            SetAlive (true);
        }
    }
    m_bPassed = false;
}

TW_UINT16 CTL_DeviceEventTriplet::Execute()
{
    m_bPassed = false;
    TW_UINT16 rc = CTL_TwainTriplet::Execute();
    if ( rc != TWRC_SUCCESS )
        // Process Condition code
        return rc;
    m_bPassed = true;
    return rc;
}

CTL_DeviceEvent CTL_DeviceEventTriplet::GetDeviceEvent() const
{
    return m_DeviceEvent;
}

bool CTL_DeviceEventTriplet::IsSuccessful() const
{
    return m_bPassed;
}
