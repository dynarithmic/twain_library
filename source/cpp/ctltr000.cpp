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
#include "ctltr000.h"
#include "ctltr007.h"
#include "ctltwses.h"
#include "ctltwmgr.h"
#include "dtwain_resource_constants.h"
using namespace dynarithmic;
///////////////// Open Data source manager triplet /////////////////////////
CTL_TwainCloseSMTriplet::CTL_TwainCloseSMTriplet(CTL_ITwainSession *pSession) :
                        CTL_TwainSMTriplet(pSession, MSG_CLOSEDSM, IDS_ErrSourceMgrClose)
{
}

CTL_TwainOpenSMTriplet::CTL_TwainOpenSMTriplet(CTL_ITwainSession *pSession) :
                        CTL_TwainSMTriplet(pSession, MSG_OPENDSM, IDS_ErrSourceMgrOpen)
{
}

TW_UINT16 CTL_TwainOpenSMTriplet::Execute()
{
    return CTL_TwainSMTriplet::Execute();
}

CTL_TwainSMTriplet::CTL_TwainSMTriplet(CTL_ITwainSession *pSession, TW_UINT16 nMsg, int nErr) : m_nDSMVersion(0)
{
    // Get the app manager's AppID
    m_nErr = nErr;
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();
    if ( pMgr && pMgr->IsValidTwainSession( pSession ))
    {
         Init( pSession->GetAppIDPtr(), NULL, DG_CONTROL, DAT_PARENT,
                nMsg, (TW_MEMREF)pSession->GetWindowHandlePtr() );
         SetAlive (true);
    }
}

int CTL_TwainSMTriplet::GetDSMVersion() const
{
    if ( std::get<0>(GetTripletArgs())->SupportedGroups & DF_DSM2 )
        return DTWAIN_TWAINDSM_VERSION2;
    return DTWAIN_TWAINDSM_LEGACY;
}

TW_UINT16 CTL_TwainSMTriplet::Execute()
{
    TW_UINT16 rc = CTL_TwainTriplet::Execute();
    if ( rc != TWRC_SUCCESS )
    {
        CTL_ConditionCodeTriplet CC(GetSessionPtr(), GetSourcePtr());
        if ( CC.Execute() == TWRC_SUCCESS )
            CTL_TwainAppMgr::ProcessConditionCodeError(CC.GetConditionCode());
        DTWAIN_ERROR_CONDITION(m_nErr, TWRC_FAILURE);
    }
    return TWRC_SUCCESS;
}
