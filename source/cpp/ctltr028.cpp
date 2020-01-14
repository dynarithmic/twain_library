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
#include <algorithm>
#include "ctltwmgr.h"
#include "ctltrall.h"
#include "ctltmpl5.h"

using namespace dynarithmic;

CTL_SetupFileXferTriplet::CTL_SetupFileXferTriplet(CTL_ITwainSession *pSession,
                                                   CTL_ITwainSource* pSource,
                                                   TW_UINT16 GetSetType,
                                                   CTL_TwainFileFormatEnum FileFormat,
                                                   const CTL_StringType& strFileName
                                                   )
                                                   : CTL_TwainTriplet(), m_SetupFileXfer()
{
    SetSessionPtr( pSession );
    SetSourcePtr( pSource );
    StringWrapperA::SafeStrcpy( m_SetupFileXfer.FileName,
                                StringConversion::Convert_Native_To_Ansi(strFileName).c_str(),
                                sizeof(m_SetupFileXfer.FileName) - 1 );

    m_SetupFileXfer.Format = (TW_UINT16)FileFormat;

    // Get the app manager's AppID
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();

    if ( pMgr && pMgr->IsValidTwainSession( pSession ))
    {
        if ( pSource )
        {
            Init( pSession->GetAppIDPtr(),
                  pSource->GetSourceIDPtr(),
                  DG_CONTROL,
                  DAT_SETUPFILEXFER,
                  (TW_UINT16)GetSetType,
                  (TW_MEMREF)&m_SetupFileXfer);

            SetAlive (true);
        }
    }

    // Set the capability map if this is a set type
    bool bIsSet = IsMSGSetType();
    if (bIsSet)
    {
        m_capMap[DTWAIN_CV_ICAPJPEGPIXELTYPE] = TWPT_BW;
        m_capMap[DTWAIN_CV_ICAPJPEGQUALITY] = TWJQ_MEDIUM;
        m_capMap[DTWAIN_CV_ICAPCOMPRESSION] = TWCP_NONE;
        m_capMap[DTWAIN_CV_ICAPJPEGSUBSAMPLING] = TWCP_NONE;
    }
}

struct CapGetter
{
    CTL_ITwainSource *m_pSource;
    CapGetter(CTL_ITwainSource* pSource) : m_pSource(pSource) {}
    void operator()(CTL_SetupFileXferTriplet::FileXferCapMap::value_type& v)
    { CTL_TwainAppMgr::GetCurrentOneCapValue(m_pSource, &v.second, v.first, static_cast<TW_UINT16>(CTL_TwainAppMgr::GetGeneralCapInfo(v.first).m_nDataType)); }
};

struct CapSetter
{
    CTL_ITwainSource *m_pSource;
    CapSetter(CTL_ITwainSource* pSource) : m_pSource(pSource) {}
    void operator()(CTL_SetupFileXferTriplet::FileXferCapMap::value_type& v)
    { SetOneCapValue(m_pSource, v.first, CTL_SetTypeSET, v.second, static_cast<TW_UINT16>(CTL_TwainAppMgr::GetGeneralCapInfo(v.first).m_nDataType)); }
};

TW_UINT16 CTL_SetupFileXferTriplet::Execute()
{
    bool bIsSet = IsMSGSetType();

    if (bIsSet)
    {
        // Get the current cap values for each cap we need to know about before setting the file transfer
        CapGetter cg(GetSourcePtr());
        std::for_each(m_capMap.begin(), m_capMap.end(), cg);
    }

    // set up the file xfer
    TW_UINT16 rc = CTL_TwainTriplet::Execute();

    // if successful we now set the trailing capabilities we need for file xfer
    if (rc == TWRC_SUCCESS && bIsSet )
    {
        CapSetter cs(GetSourcePtr());
        std::for_each(m_capMap.begin(), m_capMap.end(), cs);
    }
    return rc;
}
