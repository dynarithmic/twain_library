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
#include "ctltr014.h"
#include "ctlobtyp.h"

using namespace dynarithmic;

CTL_CapabilityGetArrayTriplet::CTL_CapabilityGetArrayTriplet(
                                    CTL_ITwainSession *pSession,
                                    CTL_ITwainSource* pSource,
                                    CTL_EnumGetType gType,
                                    TW_UINT16       gCap,
                                    TW_UINT16 TwainDataType=0xFFFF)

                       :  CTL_CapabilityGetTriplet(pSession,
                                                   pSource,
                                                   gType,
                                                   gCap,
                                                   TwainDataType), m_nNumItems(0)
{
}


bool CTL_CapabilityGetArrayTriplet::EnumCapValues( void *pCapData )
{
    pTW_ARRAY pValArray;
    size_t    nNumItems;

    // dereference to a TW_ARRAY structure
    pValArray = static_cast<pTW_ARRAY>(pCapData);

    // Get # of items in array
    nNumItems = static_cast<size_t>(pValArray->NumItems);

    TW_UINT16 nItemType = GetEffectiveItemType(pValArray->ItemType);

    // Get sizeof each item in enumeration
    TW_UINT16 nItemSize = GetItemSize( nItemType );

    // Unknown item type.  Do error condition here??
    if ( nItemSize == 0 )
        return false;

    CTL_TwainTypeObPtr pOb;

    RemoveAllTypeObjects();

    CTL_TwainTypeArray *pArray = GetTwainTypeArray();

    for ( TW_UINT16 nIndex = 0; nIndex < nNumItems; nIndex++ )
    {
        // Create a new object for this item
        if ( nItemType == TWTY_FIX32 )
            pOb = std::make_shared<CTL_TwainTypeOb>( static_cast<TW_UINT16>(sizeof( float )), false );
        else
            pOb = std::make_shared<CTL_TwainTypeOb>( nItemType );

        if ( nItemType == TWTY_FIX32 )
        {
            pTW_FIX32 p = (pTW_FIX32)&pValArray->ItemList[nIndex * nItemSize];
            float fFix = Twain32ToFloat( *p );
            pOb->CopyData( &fFix );
        }
        else
        {
            // Copy Data to pOb
            pOb->CopyData( (void *)&pValArray->ItemList[nIndex * nItemSize] );
        }
            // Store this object in object array
        pArray->push_back( pOb );
    }
    m_nNumItems = nNumItems;
    return true;
}


size_t CTL_CapabilityGetArrayTriplet::GetNumItems()
{
    return m_nNumItems;
}


bool CTL_CapabilityGetArrayTriplet::GetValue( void *pData, size_t nWhere )
{
    CTL_TwainTypeArray *pArray = GetTwainTypeArray();

    if ( nWhere >= m_nNumItems )
        return false;
    CTL_TwainTypeOb *pOb = (*pArray)[nWhere].get();
    if ( pOb )
    {
        pOb->GetData( pData );
        return true;
    }
    return false;
}
