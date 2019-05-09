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
#define MC_NO_CPP
#include "ctltr012.h"
#include "ctlobtyp.h"

using namespace dynarithmic;

CTL_CapabilityGetEnumTriplet::CTL_CapabilityGetEnumTriplet(
                                    CTL_ITwainSession *pSession,
                                    CTL_ITwainSource* pSource,
                                    CTL_EnumGetType gType,
                                    TW_UINT16       gCap,
                                    TW_UINT16 TwainDataType)

                       :  CTL_CapabilityGetTriplet(pSession,
                                                   pSource,
                                                   gType,
                                                   gCap,
                                                   TwainDataType)
{
}


bool CTL_CapabilityGetEnumTriplet::EnumCapValues( void *pCapData )
{
    pTW_ENUMERATION pValEnum;
    size_t          nNumItems;

    // dereference to a TW_ENUMERATION structure
    pValEnum = (pTW_ENUMERATION) pCapData;

    // Get # of items in enumeration
    nNumItems = static_cast<size_t>(pValEnum->NumItems);

    // Get item type
    TW_UINT16 nItemType = GetEffectiveItemType(pValEnum->ItemType);

    // Get sizeof each item in enumeration
    int nItemSize = GetItemSize( nItemType );

    // Unknown item type.  Do error condition here??
    if ( nItemSize == 0 )
        return false;

    // Set the item type for this object
    CTL_TwainTypeObPtr pOb;

    RemoveAllTypeObjects();

    CTL_TwainTypeArray *pArray = GetTwainTypeArray();

    for ( TW_UINT16 nIndex = 0; nIndex < nNumItems; nIndex++ )
    {
        if ( nItemType == TWTY_FIX32 )
            pOb = std::make_shared<CTL_TwainTypeOb>(static_cast<TW_UINT16>(sizeof( double )), false );
        else
            pOb = std::make_shared<CTL_TwainTypeOb>( nItemType );

        if ( nItemType == TWTY_FIX32 )
        {
            pTW_FIX32 p = (pTW_FIX32)&pValEnum->ItemList[nIndex * nItemSize];
            double fFix = (double)Twain32ToFloat( *p );
            pOb->CopyData( &fFix );
        }
        else
        {
            // Copy Data to pOb
            pOb->CopyData( (void *)&pValEnum->ItemList[nIndex * nItemSize] );
        }
        // Store this object in object array
        pArray->push_back( pOb );
    }
    m_nNumItems = nNumItems;
    return true;
}

size_t CTL_CapabilityGetEnumTriplet::GetNumItems()
{
    return m_nNumItems;
}


bool CTL_CapabilityGetEnumTriplet::GetValue( void *pData, size_t nWhere )
{
    CTL_TwainTypeArray *pArray = GetTwainTypeArray();

    if ( nWhere >= m_nNumItems )
        return false;
    CTL_TwainTypeObPtr pOb = pArray->at( nWhere );
    if ( pOb )
    {
        pOb->GetData( pData );
        return true;
    }
    return false;
}

