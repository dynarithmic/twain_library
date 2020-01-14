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
#ifndef CTLTMPL5_H_
#define CTLTMPL5_H_

#include <vector>
#include <algorithm>
#include <memory>
#include "tr1defs.h"
#include "enumeratorfuncs.h"
#include "ctltr013.h"
#include "ctltr014.h"
#include "ctltr016.h"
#include "ctltr017.h"
#include "ctltr018.h"
#include "ctltr019.h"
#include "ctltr020.h"
#define USE_NORMALSTRINGS  1
#define USE_LONGSTRINGS    2
#define USE_UNICODESTRINGS 4

namespace dynarithmic
{
    // Finds the array item that has a value of SearchVal
    template <class TypeInfo, class TypeArray>
    bool FindFirstValue( TypeInfo SearchVal, std::vector<TypeArray>* SearchArray, int *pWhere);
    CTL_CapInfo GetCapInfo( CTL_TwainDLLHandle* pHandle, CTL_ITwainSource *p,TW_UINT16 nCap);

    template <class T>
    bool    GetCapabilityValues( const CTL_ITwainSource *pSource,
                                TW_UINT16 nCap,
                                CTL_EnumGetType GetType,
                                UINT      nContainerTypes,
                                LONG    bUseStrings,
                                TW_UINT16 TwainDataType,
                                std::vector<T> &rArray
                              );
    template <class T>
    bool GetCapabilityValues( const CTL_ITwainSource *pSource,
                             TW_UINT16 nCap,
                             CTL_EnumGetType GetType,
                             UINT      nContainerTypes,
                             LONG      bUseStrings,
                             TW_UINT16 TwainDataType,
                             std::vector<T> &rArray
                            )
    {
        CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
        CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
        TW_UINT16 rc;
        rArray.erase(rArray.begin(), rArray.end());
        std::unique_ptr<CTL_CapabilityGetTriplet> pGetTriplet;

        if ( !CTL_TwainAppMgr::IsSourceOpen( pSource ) )
            return false;

        // Try the array version
        if ( nContainerTypes & TwainContainer_ARRAY )
        {
            pGetTriplet = std::make_unique<CTL_CapabilityGetArrayTriplet>(pSession,
                                                                          pTempSource,
                                                                          GetType,
                                                                          nCap,
                                                                          TwainDataType);
        }
        else
        if ( nContainerTypes & TwainContainer_ENUMERATION )
        {
            pGetTriplet = std::make_unique<CTL_CapabilityGetEnumTriplet>(pSession,
                                                                          pTempSource,
                                                                          GetType,
                                                                          nCap,
                                                                          TwainDataType);
        }
        else
        if ( nContainerTypes & TwainContainer_RANGE )
        {
            pGetTriplet = std::make_unique<CTL_CapabilityGetRangeTriplet>( pSession,
                                                                        pTempSource,
                                                                        GetType,
                                                                        nCap,
                                                                        TwainDataType);
        }
        else
        if ( nContainerTypes & TwainContainer_ONEVALUE )
        {
            pGetTriplet = std::make_unique<CTL_CapabilityGetOneValTriplet>( pSession,
                                                                           pTempSource,
                                                                           GetType,
                                                                           nCap,
                                                                           TwainDataType);
        }
        else
            return false;

        rc = pGetTriplet->Execute();
        if ( rc == TWRC_SUCCESS )
        {
            size_t nValues = pGetTriplet->GetNumItems();

            // Check if string type was actually used.
            // This may be the case of custom caps are used
            int nItemType = pGetTriplet->GetItemType();
            if ( nItemType == TWTY_STR32 ||
                 nItemType == TWTY_STR64  ||
                 nItemType == TWTY_STR128  ||
                 nItemType == TWTY_STR255 )
                 bUseStrings = USE_NORMALSTRINGS;
            else
            if ( nItemType == TWTY_STR1024)
                bUseStrings = USE_LONGSTRINGS;
            else
            if ( nItemType == TWTY_UNI512 )
                bUseStrings = USE_UNICODESTRINGS;

            for ( size_t i = 0; i < nValues; i++ )
            {
                T Value;
                if ( !bUseStrings )
                {
                    memset(&Value,0,sizeof(T));
                    pGetTriplet->GetValue( &Value, i );
                    rArray.push_back( Value );
                }
                else
                {
                    if ( bUseStrings & USE_NORMALSTRINGS )
                    {
                        TW_STR255 ThisString;
                        pGetTriplet->GetValue(ThisString, i );
                        CTL_StringArray *pArray = (CTL_StringArray *)&rArray;
                        pArray->push_back(ThisString);
                    }
                    else
                    if ( bUseStrings & USE_LONGSTRINGS )
                    {
                        TW_STR1024 ThisString;
                        pGetTriplet->GetValue(ThisString, i );
                        CTL_StringArray *pArray = (CTL_StringArray *)&rArray;
                        pArray->push_back(reinterpret_cast<LPSTR>(ThisString));
                    }
                    else
                    if ( bUseStrings & USE_UNICODESTRINGS )
                    {
    /*
                        TW_UNI512 ThisString;
                        pGetTriplet->GetValue(ThisString, i );
                        CTL_UnicodeStringArray *pArray = (CTL_UnicodeStringArray *)&rArray;
                        pArray->push_back(ThisString);
    */
                    }
                }
            }
            return true;
        }
        return false;
    }

    template <class T>
    bool SetCapabilityValues( const CTL_ITwainSource *pSource,
                             CTL_EnumCapability nCap,
                             CTL_EnumSetType SetType,
                             UINT      nContainerTypes,
                             TW_UINT16  nDataType,
                             std::vector<T> &rArray
                            );
    template <class T>
    bool SetCapabilityValues( const CTL_ITwainSource *pSource,
                             CTL_EnumCapability nCap,
                             CTL_EnumSetType SetType,
                             UINT      nContainerTypes,
                             TW_UINT16  nDataType,
                             std::vector<T> &rArray
                            )
    {
        CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
        CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
        TW_UINT16 rc;

        std::unique_ptr<CTL_CapabilityTriplet> pSetTriplet;

        // Check if this is a "Reset"
        if (CTL_CapabilityTriplet::IsCapOperationReset(SetType))
        {
            pSetTriplet = std::make_unique<CTL_CapabilityResetTriplet>(pSession,
                                                                     pTempSource,
                                                                     nCap,
                                                                     static_cast<TW_UINT16>(SetType ));
        }
        else
        // Try the array version
        if ( nContainerTypes & TwainContainer_ARRAY )
        {
            pSetTriplet = std::make_unique<CTL_CapabilitySetArrayTriplet<T>>(pSession,
                                                                            pTempSource,
                                                                            SetType,
                                                                            nCap,
                                                                            nDataType,
                                                                            rArray);
        }
        else
        if ( nContainerTypes & TwainContainer_ENUMERATION )
        {
            pSetTriplet = std::make_unique<CTL_CapabilitySetEnumTriplet<T>>( pSession,
                                                                              pTempSource,
                                                                              SetType,
                                                                              nCap,
                                                                              nDataType,
                                                                              rArray);
        }
        else
        if ( nContainerTypes & TwainContainer_RANGE )
        {
            pSetTriplet = std::make_unique<CTL_CapabilitySetRangeTriplet<T>>( pSession,
                                                                            pTempSource,
                                                                            SetType,
                                                                            nCap,
                                                                            nDataType,
                                                                            rArray);
        }
        else
        if ( nContainerTypes & TwainContainer_ONEVALUE )
        {
            pSetTriplet = std::make_unique<CTL_CapabilitySetOneValTriplet<T>>( pSession,
                                                                               pTempSource,
                                                                               SetType,
                                                                               nCap,
                                                                               nDataType,
                                                                               rArray);
        }
        else
            return false;

        rc = pSetTriplet->Execute();
        if ( rc == TWRC_SUCCESS )
            return true;
        return false;
    }

    template <class TwainType, class AssignType>
    bool GetOneCapValue( DTWAIN_HANDLE DLLHandle,
                        DTWAIN_SOURCE Source,
                        TW_UINT16 nCap,
                        CTL_EnumGetType GetType,
                        BOOL bUseStrings,
                        AssignType *pAssign,
                        TW_UINT16 TwainDataType
                        );

    template <class TwainType, class AssignType>
    bool GetOneCapValue( DTWAIN_HANDLE DLLHandle,
                        DTWAIN_SOURCE Source,
                        TW_UINT16 nCap,
                        CTL_EnumGetType GetType,
                        BOOL bUseStrings,
                        AssignType *pAssign,
                        TW_UINT16 TwainDataType
                        )
    {
        CTL_ITwainSource *p = VerifySourceHandle( DLLHandle, Source );

        if ( !p )
            return false;

        std::vector<TwainType> Array;
        int bOk = GetCapabilityValues( p,
                                        nCap,
                                        (CTL_EnumGetType)GetType,
                                        (UINT)TwainContainer_ONEVALUE,
                                        bUseStrings,
                                        TwainDataType,
                                        Array );
        if ( !bOk )
            return false;

        *pAssign = Array[0];
        return true;
    }


    template <class T>
    bool SetOneCapValue(CTL_ITwainSource* pSource, TW_UINT16 nCap, CTL_EnumSetType SetType, T dValue, TW_UINT16 nDataType);

    template <class T>
    bool SetOneCapValue(CTL_ITwainSource* pSource, TW_UINT16 nCap, CTL_EnumSetType SetType, T dValue, TW_UINT16 nDataType)
    {
        std::vector<T> Array;
        Array.push_back(dValue);
        return SetCapabilityValues(pSource, (CTL_EnumCapability)nCap, (CTL_EnumSetType)SetType, (UINT)TwainContainer_ONEVALUE, nDataType, Array)?true:false;
    }

    template <class T>
    bool SetOneCapValue(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source, TW_UINT16 nCap, CTL_EnumSetType SetType, T dValue, TW_UINT16 nDataType);

    template <class T>
    bool SetOneCapValue( DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source, TW_UINT16 nCap, CTL_EnumSetType SetType, T dValue,TW_UINT16 nDataType)
    {
        CTL_ITwainSource *p = VerifySourceHandle( DLLHandle, Source );
        if ( !p )
            return false;
        return SetOneCapValue(p, nCap, SetType, dValue, nDataType);
    }

    template <typename T>
    struct VectorAdderFn
    {
        static void AdderFn(std::vector<T>* ptr, T data)
        {
            ptr->push_back(data);
        }
    };

    struct VectorAdderFn2
    {
        static void AdderFn(std::vector<CTL_StringType>* ptr, CTL_String& data)
        {
            CTL_StringType sVal = StringConversion::Convert_Ansi_To_Native(data);
            ptr->push_back( sVal );
        }
    };

    template <class TwainType, class AssignType, class EnumeratorType>
    bool GetMultiCapValues( DTWAIN_HANDLE DLLHandle,
                           DTWAIN_SOURCE Source,
                           DTWAIN_ARRAY pArray,
                           CTL_EnumeratorType EnumType,
                           TW_UINT16 nCap,
                           CTL_EnumGetType GetType,
                           AssignType AValue,
                           TW_UINT16 TwainDataType,
                           UINT nContainerVal/* = 0*/,
                           bool bUseContainer/* = false*/,
                           TwainType TT = TwainType()
                         );


    template <class TwainType, class AssignType, class EnumeratorType, class Adder>
    struct GetMultiCapValuesImpl
    {
        static bool GetMultiCapValues( DTWAIN_HANDLE DLLHandle,
                                      DTWAIN_SOURCE Source,
                                      DTWAIN_ARRAY pArray,
                                      CTL_EnumeratorType EnumType,
                                      TW_UINT16 nCap,
                                      CTL_EnumGetType GetType,
                                      AssignType,
                                      TW_UINT16 TwainDataType,
                                      UINT nContainerVal,
                                      bool bUseContainer,
                                      TwainType TT = TwainType()
                                    )
        {
            UNUSED_PARAM(TT);
            CTL_ITwainSource *p = VerifySourceHandle( DLLHandle, Source );
            CTL_TwainDLLHandle *pHandle = (static_cast<CTL_TwainDLLHandle*>(DLLHandle));

            if ( !p )
                return false;

            // Check if array is of the correct type
            if ( !EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, EnumType ) )
                return false;
            std::vector<TwainType> Array;

            bool bOk = false;

            CTL_CapInfo Info;
            Info = GetCapInfo( pHandle, p, nCap );
            if ( !Info.IsValid() )
                return false;
            UINT nAll[3];
            UINT nContainer = std::get<1>(Info);

            size_t nMaxNum;
            if ( !bUseContainer )
            {
                nAll[0] = nContainer & TwainContainer_ENUMERATION;
                nAll[1] = nContainer & TwainContainer_ARRAY;
                nAll[2] = nContainer & TwainContainer_RANGE;
                nMaxNum = 3;
            }
            else
            {
                nAll[0] = nContainerVal;
                nMaxNum = 1;
            }

            size_t i;
            // Determine string type
            LONG StringType=0;
            switch (EnumType)
            {
                case CTL_EnumeratorStringType:
                    StringType = USE_NORMALSTRINGS;
                    break;

                case CTL_EnumeratorLongStringType:
                    StringType = USE_LONGSTRINGS;
                    break;

                case CTL_EnumeratorUnicodeStringType:
                    StringType = USE_UNICODESTRINGS;
                    break;
                default:
                    break;
            }

            for ( i = 0; i < nMaxNum; i++ )
            {
                bOk = GetCapabilityValues( p,
                    nCap,
                    (CTL_EnumGetType)GetType,
                    nAll[i],
                    StringType,
                    TwainDataType,
                    Array );
                if ( bOk )
                    break;
            }

            if ( !bOk )
                return false;

            // Populate the array
            size_t nSize = Array.size();
            auto  pVector = EnumeratorVectorPtr<AssignType>(pArray);
            for ( i = 0; i < nSize; i++)
                Adder::AdderFn(pVector, Array[i]);
            return true;
        }

    };

    template <class TwainType, class AssignType, class EnumeratorType>
    bool GetMultiCapValues( DTWAIN_HANDLE DLLHandle,
                           DTWAIN_SOURCE Source,
                           EnumeratorType *pArray,
                           CTL_EnumeratorType EnumType,
                           TW_UINT16 nCap,
                           CTL_EnumGetType GetType,
                           AssignType CValue,
                           TW_UINT16 TwainDataType,
                           UINT nContainerVal,
                           bool bUseContainer,
                           TwainType TT = TwainType()
                         )
    {
        return GetMultiCapValuesImpl<TwainType, AssignType, EnumeratorType, VectorAdderFn<AssignType> >::GetMultiCapValues
                                                    (DLLHandle, Source, pArray, EnumType, nCap,
                                                     GetType, CValue, TwainDataType,
                                                     nContainerVal, bUseContainer, TT);
    }

    template <class TwainType, class EnumeratorType>
    bool GetMultiCapValues_String( DTWAIN_HANDLE DLLHandle,
                          DTWAIN_SOURCE Source,
                          DTWAIN_ARRAY pArray,
                          CTL_EnumeratorType EnumType,
                          TW_UINT16 nCap,
                          CTL_EnumGetType GetType,
                          CTL_StringType cStr,
                          TW_UINT16 TwainDataType,
                          UINT nContainerVal,
                          bool bUseContainer
                          )
    {
        return GetMultiCapValuesImpl<CTL_String, CTL_StringType, EnumeratorType, VectorAdderFn2>::GetMultiCapValues
                                    (DLLHandle,
                                    Source,
                                    pArray,
                                    EnumType,
                                    nCap,
                                    GetType,
                                    cStr,
                                    TwainDataType,
                                    nContainerVal,
                                    bUseContainer,
                                    CTL_String());
    }

    template <typename T>
    struct DefaultNativeToTwainConverter
    {
        static T convert(T value, DTWAIN_ARRAY) { return value; }
    };

    struct StringNativeToTwainConverter
    {
        static CTL_String convert(CTL_StringType& value)
        { return StringConversion::Convert_Native_To_Ansi(value); }
    };

    /////////////////////////////////////////////////////////////////////////////////////////
    template <class NativeTwainType, class ArrayType>
    bool SetMultiCapValues( DTWAIN_HANDLE DLLHandle,
                           DTWAIN_SOURCE Source,
                           DTWAIN_ARRAY  pArray,
                           CTL_EnumeratorType EnumType,
                           UINT nCap,
                           CTL_EnumSetType SetType,
                           NativeTwainType TValue,
                           UINT nContainerVal,
                           bool bUseContainer,
                           TW_UINT16 OriginalTwainType,
                           ArrayType eType
                         );

    template <class OriginalType, class ArrayType, class ConvertedTwainType, class NativeToTwainConverter>
    bool SetMultiCapValuesImpl( DTWAIN_HANDLE DLLHandle,
                           DTWAIN_SOURCE Source,
                           DTWAIN_ARRAY pArray,
                           CTL_EnumeratorType EnumType,
                           UINT nCap,
                           CTL_EnumSetType SetType,
                           OriginalType TValue,
                           UINT nContainerVal,
                           bool bUseContainer,
                           TW_UINT16 OriginalTwainType,
                           ArrayType eType,
                           ConvertedTwainType cType
                         )
    {
        CTL_ITwainSource *p = VerifySourceHandle( DLLHandle, Source );
        CTL_TwainDLLHandle* pHandle = static_cast<CTL_TwainDLLHandle*>(DLLHandle);

        if ( !p )
            return false;

        // Check if array is of the correct type
        if ( !EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, EnumType ) )
            return false;

        // Create array of the twain type
        DTWAIN_ARRAY pDTWAINArray = pArray;
        std::vector<ConvertedTwainType> Array;
        OriginalType dValue;
        size_t nValues = EnumeratorFunctionImpl::EnumeratorGetCount(pDTWAINArray);
        int i;

        for ( i = 0; i < (int)nValues; i++ )
        {
            EnumeratorFunctionImpl::EnumeratorGetAt(pDTWAINArray, i, &dValue);
            ConvertedTwainType conv = NativeToTwainConverter::convert(dValue);
            Array.push_back( conv );
        }

        bool bOk = false;

        CTL_CapInfo Info;
        Info = GetCapInfo( pHandle, p, (TW_UINT16)nCap );
        if ( !Info.IsValid() )
            return false;
        UINT nAll[3];
        UINT nContainer = std::get<1>(Info);

        int nMaxNum;
        if ( !bUseContainer )
        {
            nAll[0] = nContainer & TwainContainer_ENUMERATION;
            nAll[1] = nContainer & TwainContainer_ARRAY;
            nAll[2] = nContainer & TwainContainer_RANGE;
            nMaxNum = 3;
        }
        else
        {
            nAll[0] = nContainerVal;
            nMaxNum = 1;
        }

        for ( i = 0; i < nMaxNum; i++ )
        {
            bOk = SetCapabilityValues(  p,
                                        (CTL_EnumCapability)nCap,
                                        SetType,
                                        nAll[i],
                                        OriginalTwainType,
                                       Array);
            if ( bOk )
               break;
        }

        return bOk;
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    template <class TwainType, class NativeType, class TwainConverter>
    bool SetMultiCapValues( DTWAIN_HANDLE DLLHandle,
                          DTWAIN_SOURCE Source,
                          DTWAIN_ARRAY pArray,
                          CTL_EnumeratorType EnumType,
                          UINT nCap,
                          CTL_EnumSetType SetType,
                          UINT nContainerVal,
                          bool bUseContainer,
                          TW_UINT16 OriginalTwainType
                          );

    template <class TwainType, class NativeType, class TwainConverter>
    bool SetMultiCapValues( DTWAIN_HANDLE DLLHandle,
                          DTWAIN_SOURCE Source,
                          DTWAIN_ARRAY  pArray,
                          CTL_EnumeratorType EnumType,
                          UINT nCap,
                          CTL_EnumSetType SetType,
                          UINT nContainerVal,
                          bool bUseContainer,
                          TW_UINT16 OriginalTwainType
                          )
    {
        CTL_ITwainSource *p = VerifySourceHandle( DLLHandle, Source );
        CTL_TwainDLLHandle*  pHandle = static_cast<CTL_TwainDLLHandle*>(DLLHandle);

        if ( !p )
            return false;

        // Check if array is of the correct type
        if ( !EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, EnumType ) )
            return false;

        // Create array of the twain type
        DTWAIN_ARRAY pDTWAINArray = pArray;
        std::vector<TwainType> Array;
        NativeType dValue;
        size_t nValues = EnumeratorFunctionImpl::EnumeratorGetCount(pDTWAINArray);
        int i;

        for ( i = 0; i < (int)nValues; i++ )
        {
            EnumeratorFunctionImpl::EnumeratorGetAt(pDTWAINArray, i, &dValue );
            Array.push_back( TwainConverter::convert(dValue, pDTWAINArray) );
        }

        bool bOk = false;

        CTL_CapInfo Info;
        Info = GetCapInfo( pHandle, p, (TW_UINT16)nCap );
        if ( !Info.IsValid() )
            return false;
        UINT nAll[3];
        UINT nContainer = std::get<1>(Info);

        int nMaxNum;
        if ( !bUseContainer )
        {
            nAll[0] = nContainer & TwainContainer_ENUMERATION;
            nAll[1] = nContainer & TwainContainer_ARRAY;
            nAll[2] = nContainer & TwainContainer_RANGE;
            nMaxNum = 3;
        }
        else
        {
            nAll[0] = nContainerVal;
            nMaxNum = 1;
        }

        for ( i = 0; i < nMaxNum; i++ )
        {
            bOk = SetCapabilityValues(  p,
                (CTL_EnumCapability)nCap,
                SetType,
                nAll[i],
                OriginalTwainType,
                Array);
            if ( bOk )
                break;
        }

        return bOk;
    }

    template <typename tupleType, typename TypeInfo, int nWhich=0>
    struct tupleFinder
    {
        TypeInfo tInfo;
        tupleFinder(TypeInfo t) : tInfo(t) {}
        bool operator()(const tupleType& tpl) const
        { return std::get<nWhich>(tpl) == tInfo; }
    };

    template <class TypeInfo, class TypeArray>
    bool FindFirstValue( TypeInfo SearchVal, std::vector<TypeArray> *pSearchArray, int *pWhere)
    {
        typename std::vector<TypeArray>::iterator it =
                std::find_if(pSearchArray->begin(), pSearchArray->end(), tupleFinder<TypeArray, TypeInfo, 0>(SearchVal));
        if ( it != pSearchArray->end())
        {
            if ( pWhere )
                *pWhere = static_cast<int>(std::distance(pSearchArray->begin(), it));
            return true;
        }
        if ( pWhere )
            *pWhere = -1;
        return false;
    }
}
#endif
