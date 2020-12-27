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
#include "ctltr010.h"
#include "ctltwmgr.h"

using namespace dynarithmic;

CTL_CapabilityTriplet::CTL_CapabilityTriplet(CTL_ITwainSession *pSession,
                                             CTL_ITwainSource* pSource,
                                             TW_UINT16 nMsg,
                                             TW_UINT16 TwainType,
                                             bool bReading)
                       :  CTL_TwainTriplet(),
                            m_Capability(),
                            m_bReading(bReading),
                            m_bTesting(false),
                            m_bSupported(true),
                            m_nItemType(TwainType),
                            m_bGetDefaultType(false)
{
    SetSessionPtr( pSession );
    SetSourcePtr( pSource );

    // Get the app manager's AppID
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();
    if ( pMgr && pMgr->IsValidTwainSession( pSession ))
    {
        if ( pSource )
        {
            Init( pSession->GetAppIDPtr(), pSource->GetSourceIDPtr(),
                    DG_CONTROL, DAT_CAPABILITY, nMsg,
                    (TW_MEMREF)&m_Capability);
            SetAlive (true);
        }
    }
}

CTL_CapabilityTriplet::~CTL_CapabilityTriplet()
{
    RemoveAllTypeObjects();
}

void CTL_CapabilityTriplet::RemoveAllTypeObjects()
{
    m_ObArray.clear();
 }

bool CTL_CapabilityTriplet::IsCapabilitySupported()
{
    bool oldTesting = m_bTesting;
    m_bTesting = true;
    // Call base class
    TW_UINT16 rc = Execute();
    m_bTesting = oldTesting;
    // Return FALSE if capability is not supported
    if ( rc != TWRC_SUCCESS )
        return false;

    return true;
}


TW_UINT16 CTL_CapabilityTriplet::Execute()
{
    RemoveAllTypeObjects();
    // Call base class
    TW_UINT16 rc = CTL_TwainTriplet::Execute();

    if ( rc != TWRC_SUCCESS )
    {
        m_bSupported = FALSE;

        TW_UINT16 ccode =
            CTL_TwainAppMgr::GetConditionCode(GetSessionPtr(), GetSourcePtr(), rc);
        if ( ccode != TWCC_SUCCESS )
        {
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
        }
        return rc;
    }

    // Determine return type
    // Get the pointer to the data returned and let it decode
    // if the capability is to be read
    if ( m_bTesting ) // Don't decode if in test mode
    {
        if ( m_Capability.hContainer)
        {
            if ( m_Capability.Cap >= CAP_CUSTOMBASE )
            {
                // Need to specially decode this to determine ItemType
                // Get pointer to data
                void *pCapData = (void *)CTL_TwainDLLHandle::s_TwainMemoryFunc->LockMemory(m_Capability.hContainer);
                pTW_ONEVALUE pValOne;

                // dereference to a TW_ONEVALUE structure.  Don't really
                // care if item is not really TW_ONEVALUE since first
                // item in structure is the same for all types (the item type)
                pValOne = (pTW_ONEVALUE) pCapData;

                // Get item type
                TW_UINT16 nItemType = pValOne->ItemType;

                SetItemType(nItemType);
            }

            if ( CTL_TwainDLLHandle::s_TwainMemoryFunc == &CTL_TwainDLLHandle::s_TwainLegacyFunc)
            {
                #ifdef _WIN32
                UINT nCount = GlobalFlags(m_Capability.hContainer) & GMEM_LOCKCOUNT;
                for ( UINT i = 0; i < nCount; i++ )
                      GlobalUnlock(m_Capability.hContainer);
                GlobalFree( m_Capability.hContainer );
                #endif
            }
            else
            {
                CTL_TwainDLLHandle::s_TwainMemoryFunc->UnlockMemory(m_Capability.hContainer);
                CTL_TwainDLLHandle::s_TwainMemoryFunc->FreeMemory(m_Capability.hContainer);
            }
        }
    }
    else
    if ( m_bReading )
    {
        if ( !m_Capability.hContainer )
        {
            // No capability data
            rc = 1;
            CTL_TwainAppMgr::ProcessConditionCodeError(-TWAIN_ERR_NULL_CONTAINER);
            return rc;
        }

        try
        {
            TW_MEMREF p = CTL_TwainDLLHandle::s_TwainMemoryFunc->LockMemory( m_Capability.hContainer );
            Decode(p);
            CTL_TwainDLLHandle::s_TwainMemoryFunc->UnlockMemory(m_Capability.hContainer);
            CTL_TwainDLLHandle::s_TwainMemoryFunc->FreeMemory( m_Capability.hContainer ); // Test
        }
        catch(...)
        {

        }
    }
    return rc;
}

void CTL_CapabilityTriplet::Decode( void * /*p*/ )
{}

bool CTL_CapabilityTriplet::IsReading() const
{
    return m_bReading;
}

void CTL_CapabilityTriplet::SetTestMode( bool bSet/* = true */)
{
    m_bTesting = bSet;
}


bool CTL_CapabilityTriplet::IsTesting() const
{
    return m_bTesting;
}

TW_CAPABILITY*  CTL_CapabilityTriplet::GetCapabilityBuffer()
{
    return &m_Capability;
}


TW_UINT16 CTL_CapabilityTriplet::GetItemSize( TW_UINT16 nTwainItem )
{
    switch (nTwainItem)
    {
        case TWTY_INT8:
            return sizeof( TW_INT8 );
        case TWTY_INT16:
            return sizeof( TW_INT16 );
        case TWTY_INT32:
            return sizeof( TW_INT32 );
        case TWTY_UINT8:
            return sizeof( TW_UINT8 );
        case TWTY_UINT16:
            return sizeof( TW_UINT16 );
        case TWTY_UINT32:
            return sizeof( TW_UINT32 );
        case TWTY_BOOL:
            return sizeof( TW_BOOL );
        case TWTY_FIX32:
            return sizeof( TW_FIX32 );
        case TWTY_FRAME:
            return sizeof( TW_FRAME );
        case TWTY_STR32:
            return sizeof( TW_STR32 );
        case TWTY_STR64:
            return sizeof( TW_STR64 );
        case TWTY_STR128:
            return sizeof( TW_STR128 );
        case TWTY_STR255:
            return sizeof( TW_STR255 );
        case TWTY_STR1024:
            return sizeof( TW_STR1024);
        case TWTY_UNI512:
            return sizeof( TW_UNI512);
        case TWTY_HANDLE:
            return sizeof(TW_HANDLE);
    }
    return 0;
}

CTL_TwainTypeArray* CTL_CapabilityTriplet::GetTwainTypeArray()
{
    return &m_ObArray;
}
////////////////////////////////////////////////////////////////////////////
float CTL_CapabilityTriplet::Twain32ToFloat( TW_FIX32 Fix32 )
{
    float  fval;
    fval = (float)Fix32.Whole + (float)Fix32.Frac / (float)65536.0;
    return fval;
}


void CTL_CapabilityTriplet::FloatToTwain32( float fnum, TW_FIX32 & Fix32_value )
{
    TW_BOOL sign = (fnum < 0)?TRUE:FALSE;
    TW_INT32 value = (TW_INT32) (fnum * 65536.0 + (sign?(-0.5):0.5));
    Fix32_value.Whole = static_cast<TW_INT16>(value >> 16);
    Fix32_value.Frac = (TW_UINT16)(value & 0x0000ffffL);
}


bool CTL_CapabilityTriplet::IsSupported()
{
    return m_bSupported;
}
