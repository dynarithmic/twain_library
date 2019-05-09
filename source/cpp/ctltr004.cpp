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
#include "ctltr004.h"
#include "ctltwses.h"
using namespace dynarithmic;
CTL_SelectSourceDlgTriplet::CTL_SelectSourceDlgTriplet(
                            CTL_ITwainSession *pSession,
                            LPCTSTR pProduct/*=NULL*/) :
              CTL_SourceTriplet( pSession, pProduct, MSG_USERSELECT )
{
}


TW_UINT16 CTL_SelectSourceDlgTriplet::Execute()
{
    TW_UINT16 rc = CTL_SourceTriplet::Execute();
    CTL_ITwainSession *pSession;
    CTL_ITwainSource* pCurSource;

    pSession = GetSessionPtr();
    pCurSource = GetSourcePtr();

    switch ( rc )
    {
        case TWRC_SUCCESS:
        {
            CTL_ITwainSource* pSource;

            // Check if source exists
            pSource = pSession->Find( pCurSource );
            if ( !pSource )
            {
                pSession->AddTwainSource( pCurSource );
                pSession->SetSelectedSource( pCurSource );
            }
            else
            {
                pSession->SetSelectedSource( pSource );
                pCurSource->SetActive(FALSE);
                CTL_ITwainSource::Destroy(pCurSource);
            }
        }
        break;

        case TWRC_FAILURE:
        case TWRC_CANCEL:
            CTL_ITwainSource::Destroy(pCurSource);
        break;
    }
    return rc;
}
