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
#include "ctltr024.h"
#include "ctltwmgr.h"

using namespace dynarithmic;

CTL_ImageTriplet::CTL_ImageTriplet(CTL_ITwainSession *pSession,
                                   CTL_ITwainSource* pSource)

                       :  CTL_TwainTriplet()
{
    SetSessionPtr(pSession);
    SetSourcePtr(pSource);

    // Get the app manager's AppID
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();

    if ( pMgr && pMgr->IsValidTwainSession( pSession ))
        SetAlive(pSource?true:false);
}



void CTL_ImageTriplet::InitVars(TW_UINT16 nType,
                                CTL_EnumGetType nGetType,
                                void *pData)
{
    if ( IsAlive() )
    {
        Init( GetSessionPtr()->GetAppIDPtr(),
              GetSourcePtr()->GetSourceIDPtr(),
              DG_IMAGE,
              nType,
              (TW_UINT16)nGetType,
              (TW_MEMREF)pData);
    }
}


bool CTL_ImageTriplet::QueryAndRemoveDib(CTL_TwainAcquireEnum acquireType, CTL_TwainDibArray& pArray, size_t nWhich)
{
    bool bKeepPage = true;
    CTL_ITwainSession* pSession = GetSessionPtr();
    CTL_ITwainSource* pSource = GetSourcePtr();

    if (pSource->GetAcquireType() == acquireType)
    {
        bKeepPage = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_QUERYPAGEDISCARD, (LPARAM)pSource) ? true : false;
        // Keep the page
        if (!bKeepPage)
        {
            // throw this dib away (remove from the dib array)
            pArray.DeleteDibMemory(nWhich);
            pArray.RemoveDib(nWhich);
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_PAGEDISCARDED, (LPARAM)pSource);
        }
    }
    return bKeepPage;
}
