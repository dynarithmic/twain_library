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
#define MC_NO_CPP
#include "ctltr013.h"
#include "ctlobtyp.h"

using namespace dynarithmic;

CTL_CapabilityGetOneValTriplet::CTL_CapabilityGetOneValTriplet(
                                    CTL_ITwainSession *pSession,
                                    CTL_ITwainSource* pSource,
                                    CTL_EnumGetType gType,
                                    TW_UINT16 gCap,
                                    TW_UINT16 TwainDataType=0xFFFF)

                       :  CTL_CapabilityGetTriplet(pSession,
                                                   pSource,
                                                   gType,
                                                   gCap,
                                                   TwainDataType)
{
}


bool CTL_CapabilityGetOneValTriplet::EnumCapValues( void *pCapData )
{
    pTW_ONEVALUE pValOne;

    // dereference to a TW_ENUMERATION structure
    pValOne = (pTW_ONEVALUE) pCapData;

    // Get item type
    int nItemType = GetEffectiveItemType(pValOne->ItemType);

    // Get sizeof each item in enumeration
    int nItemSize = GetItemSize( pValOne->ItemType );

    // Unknown item type.  Do error condition here??
    if ( nItemSize == 0 )
        return false;

    CTL_TwainTypeObPtr pOb;

    RemoveAllTypeObjects();

    CTL_TwainTypeArray *pArray = GetTwainTypeArray();

    // Create a new object for this item
    if ( nItemType == TWTY_FIX32 )
        pOb = std::make_shared<CTL_TwainTypeOb>( static_cast<TW_UINT16>(sizeof( double )), false );
    else
        pOb = std::make_shared<CTL_TwainTypeOb>( pValOne->ItemType );

    if ( nItemType == TWTY_FIX32 )
    {
        pTW_FIX32 p = (pTW_FIX32)&pValOne->Item;
        double fFix = Twain32ToFloat( *p );
        pOb->CopyData( &fFix );
    }
    else
    {
        // Copy Data to pOb
        pOb->CopyData( (void *)&pValOne->Item );
    }

    // Store this object in object array
    pArray->push_back( pOb );
    return true;
}

size_t CTL_CapabilityGetOneValTriplet::GetNumItems()
{
    return 1;
}

bool CTL_CapabilityGetOneValTriplet::GetValue(void *pData, size_t /*nWhere*/)
{
    CTL_TwainTypeArray *pArray = GetTwainTypeArray();
    if ( pArray->empty() )
        return false;
    CTL_TwainTypeOb *pOb = pArray->front().get();
    if ( !pOb )
        return false;

    pOb->GetData( pData );
    return true;
}
