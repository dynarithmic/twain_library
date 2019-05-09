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
#include "ctlobtyp.h"
#include "ctltr010.h"
#include "ctliface.h"

using namespace dynarithmic;
CTL_TwainTypeOb::CTL_TwainTypeOb( TW_UINT16 nType, bool bGetTypeSize/*=true*/ ) : m_nSize(0), m_nType(nType), m_pData(nullptr), m_hGlobal(nullptr)
{
    if ( bGetTypeSize )
        m_nSize = CTL_CapabilityTriplet::GetItemSize( nType );
    else
        m_nSize = nType;
    m_hGlobal = CTL_TwainDLLHandle::s_TwainMemoryFunc->AllocateMemory(m_nSize);
    m_pData = NULL;
    if ( m_hGlobal )
        m_pData = CTL_TwainDLLHandle::s_TwainMemoryFunc->LockMemory(m_hGlobal);
}


CTL_TwainTypeOb::~CTL_TwainTypeOb()
{
    CTL_TwainDLLHandle::s_TwainMemoryFunc->UnlockMemory(m_hGlobal);
    CTL_TwainDLLHandle::s_TwainMemoryFunc->FreeMemory(m_hGlobal);
}

// No check for size!!!
void CTL_TwainTypeOb::CopyData( void *pData )
{
        memcpy( m_pData, pData, m_nSize );
}

void CTL_TwainTypeOb::GetData( void *pData)
{
    if ( m_pData )
        memcpy( pData, m_pData, m_nSize );
}


void *CTL_TwainTypeOb::GetDataRaw() const
{
    return m_pData;
}

int CTL_TwainTypeOb::GetDataSize() const
{
    return m_nSize;
}

int CTL_TwainTypeOb::GetDataType() const
{
    return m_nType;
}

/////////////////////////////////////////////////////////////////////////////







