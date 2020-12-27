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
#include "ctltr036.h"
#include "ctltwmgr.h"

using namespace dynarithmic;
/////////////////////////////////////////////////////////////////////////
CTL_CustomDSTriplet::CTL_CustomDSTriplet(CTL_ITwainSession *pSession,
                                         CTL_ITwainSource* pSource,
                                         TW_UINT16 nMsg)
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
                  DAT_CUSTOMDSDATA,
                  nMsg,
                  (TW_MEMREF)((pTW_CUSTOMDSDATA)&m_CustomDSData));
            SetAlive (true);
        }
    }
}

TW_UINT16 CTL_CustomDSTriplet::Execute()
{
    TW_UINT16 rc = CTL_TwainTriplet::Execute();
    return rc;
}

TW_UINT32 CTL_CustomDSTriplet::GetDataSize() const
{
    return m_CustomDSData.InfoLength;
}

HANDLE CTL_CustomDSTriplet::GetData() const
{
    return m_CustomDSData.hData;
}

void CTL_CustomDSTriplet::SetDataSize(TW_UINT32 nSize)
{
    m_CustomDSData.InfoLength = nSize;
}

TW_UINT16 CTL_CustomDSTriplet::SetData(HANDLE hData, int /*nSize*/)
{
    m_CustomDSData.hData = hData;
    return Execute();
}





