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
#include <cstring>
#include "ctltr007.h"
#include "ctltwses.h"
#include "ctltwsrc.h"
#include "ctltwmgr.h"
using namespace dynarithmic;
CTL_ConditionCodeTriplet::CTL_ConditionCodeTriplet(CTL_ITwainSession *pSession,
                                      const CTL_ITwainSource* pSource/* = NULL*/) :
                                    CTL_TwainTriplet(), m_Status{}
{
    SetSourcePtr((CTL_ITwainSource*)pSource);
    SetSessionPtr(pSession);

    // Get the app manager's AppID
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();

    if ( pMgr && pMgr->IsValidTwainSession( pSession ))
    {
        if ( pSource )
            Init( pSession->GetAppIDPtr(), *pSource, DG_CONTROL, DAT_STATUS,
                  MSG_GET, (TW_MEMREF)&m_Status );
        else
            Init( pSession->GetAppIDPtr(), NULL, DG_CONTROL, DAT_STATUS,
                  MSG_GET, (TW_MEMREF)&m_Status );

        SetAlive (true);
    }
}


TW_UINT16 CTL_ConditionCodeTriplet::GetConditionCode() const
{
    return m_Status.ConditionCode;
}
