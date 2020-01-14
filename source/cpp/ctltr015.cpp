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

#include "ctltr015.h"
#include "ctlobtyp.h"

using namespace dynarithmic;

CTL_CapabilityGetRangeTriplet::CTL_CapabilityGetRangeTriplet(
                                    CTL_ITwainSession *pSession,
                                    CTL_ITwainSource* pSource,
                                    CTL_EnumGetType gType,
                                    TW_UINT16   gCap,
                                    TW_UINT16 TwainDataType=0xFFFF)

                       :  CTL_CapabilityGetTriplet(pSession,
                                                   pSource,
                                                   gType,
                                                   gCap,
                                                   TwainDataType),
												m_FirstVal{}, m_LastVal{}, m_StepVal{}, m_DefaultVal{}, m_CurrentVal{}, m_nNumRangeItems{}
{
}


void CTL_CapabilityGetRangeTriplet::Decode( void *pCapData )
{
    EnumCapValues( pCapData );
}


bool CTL_CapabilityGetRangeTriplet::EnumCapValues( void *pCapData )
{
    CTL_TwainTypeObPtr pOb;
    pTW_RANGE pRange;

    RemoveAllTypeObjects();

    CTL_TwainTypeArray *pArray = GetTwainTypeArray();

    // dereference to a TW_RANGE structure
    pRange = (pTW_RANGE) pCapData;

    // get item type
    int nItemType = GetEffectiveItemType(pRange->ItemType);

    // get min value

    // Create a new object for this item
    pOb = std::make_shared<CTL_TwainTypeOb>( static_cast<TW_UINT16>(sizeof(TW_RANGE)), false );

    // Copy Data to pOb
    pOb->CopyData( (void *)pRange );

    // Store this object in object array
    pArray->push_back( pOb );

    // Get range stats
    if ( nItemType == TWTY_FIX32 )
    {
        pTW_FIX32   pFixFirst;
        pTW_FIX32   pFixLast;
        pTW_FIX32   pFixStep;
        pTW_FIX32   pFixDefVal;
        pTW_FIX32   pFixCurVal;

        pFixFirst = (pTW_FIX32)&pRange->MinValue;
        pFixLast  = (pTW_FIX32)&pRange->MaxValue;
        pFixStep  = (pTW_FIX32)&pRange->StepSize;
        pFixDefVal = (pTW_FIX32)&pRange->DefaultValue;
        pFixCurVal = (pTW_FIX32)&pRange->CurrentValue;

        m_FirstVal.fval  = Twain32ToFloat( *pFixFirst );
        m_LastVal.fval   = Twain32ToFloat( *pFixLast  );
        m_StepVal.fval   = Twain32ToFloat( *pFixStep  );
        m_CurrentVal.fval = Twain32ToFloat( *pFixCurVal );
        m_DefaultVal.fval = Twain32ToFloat( *pFixDefVal );
    }
    else
    {
        m_FirstVal.ival = pRange->MinValue;
        m_LastVal.ival  = pRange->MaxValue;
        m_StepVal.ival  = pRange->StepSize;
        m_CurrentVal.ival = pRange->CurrentValue;
        m_DefaultVal.ival = pRange->DefaultValue;
    }

    m_nNumRangeItems = GetNumItems();
    return true;
}


pTW_RANGE CTL_CapabilityGetRangeTriplet::GetRangePtr()
{
    pTW_RANGE pRange;
    CTL_TwainTypeArray *pArray = GetTwainTypeArray();
    if ( pArray->size() == 0 )
        return NULL;
    CTL_TwainTypeOb *pOb = pArray->front().get();
    pRange = static_cast<pTW_RANGE>(pOb->GetDataRaw());
    return pRange;
}


TW_UINT16 CTL_CapabilityGetRangeTriplet::GetDataType()
{
    pTW_RANGE pRange;
    pRange = GetRangePtr();
    if ( !pRange )
        return (TW_UINT16)-1;
    return pRange->ItemType;
}


size_t CTL_CapabilityGetRangeTriplet::GetNumItems()
{
    return 5;
}

bool CTL_CapabilityGetRangeTriplet::GetValue(void *pData, size_t nWhichVal)
{
    if ( nWhichVal >= m_nNumRangeItems )
        return false;
    int nDataType = GetDataType();
    if ( nDataType == TWTY_FIX32 )
    {
        double *pFloat = (double *)pData;
        switch ((CTL_EnumTwainRange)nWhichVal)
        {
            case TwainRange_MIN:
                *pFloat = m_FirstVal.fval;
            break;
            case TwainRange_MAX:
                *pFloat = m_LastVal.fval;
            break;
            case TwainRange_STEP:
                *pFloat = m_StepVal.fval;
            break;
            case TwainRange_DEFAULT:
                *pFloat = m_DefaultVal.fval;
            break;
            case TwainRange_CURRENT:
                *pFloat = m_CurrentVal.fval;
            break;
            default:
                return false;
        }
        return true;
    }

    pTW_UINT32 pInt32;
    pInt32 = (pTW_UINT32)pData;
    switch ((CTL_EnumTwainRange)nWhichVal)
    {
        case TwainRange_MIN:
            *pInt32 = m_FirstVal.ival;
        break;
        case TwainRange_MAX:
            *pInt32 = m_LastVal.ival;
        break;
        case TwainRange_STEP:
            *pInt32 = m_StepVal.ival;
        break;
        case TwainRange_DEFAULT:
            *pInt32 = m_DefaultVal.ival;
        break;
        case TwainRange_CURRENT:
            *pInt32 = m_CurrentVal.ival;
        break;
        default:
            return false;
    }
    return true;
}



