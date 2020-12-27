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
//#define MC_NO_CPP
#include <math.h>
#include <stdio.h>
#include <cstring>
#include <algorithm>
#include <boost/format.hpp>
#include "ctltwmgr.h"
#include "ctltrall.h"
#include "dtwain_resource_constants.h"
#include "ctltmpl5.h"
#include "ctlreg.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"

typedef LPVOID* LPLPVOID;

using namespace std;
using namespace dynarithmic;

static LONG IsValidRangeArray( DTWAIN_ARRAY pArray );
static LONG IsValidAcqArray( DTWAIN_ARRAY pArray );

static DTWAINFrameInternal* GetDTWAINFramePtr(DTWAINFrameInternal* pPtr)
{
        return (StringTraits::StringCompare(pPtr->s_id.data(), DTWAINFrameInternalGUID)==0)?pPtr:nullptr;
    }

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayInit()
{
    LOG_FUNC_ENTRY_PARAMS(())
    LOG_FUNC_EXIT_PARAMS(NULL)
    CATCH_BLOCK(DTWAIN_ARRAY())
}

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayCreate( LONG nEnumType, LONG nInitialSize )
{
    LOG_FUNC_ENTRY_PARAMS((nEnumType, nInitialSize))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);
    int nStatus;
    DTWAIN_ARRAY Array = 0;

    LONG nInterpret = nEnumType;
    switch (nEnumType)
    {
        case DTWAIN_ARRAYINT16:
        case DTWAIN_ARRAYINT32:
        case DTWAIN_ARRAYUINT16:
        case DTWAIN_ARRAYUINT32:
            nEnumType = DTWAIN_ARRAYLONG;
        break;
    }

    LPVOID ptr = EnumeratorFunctionImpl::GetNewEnumerator((CTL_EnumeratorType )nEnumType, &nStatus, nInterpret, nInitialSize);

    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
        [&] { return ptr == 0; }, DTWAIN_ERR_WRONG_ARRAY_TYPE, (DTWAIN_ARRAY)0, FUNC_MACRO);

    Array = ptr;

    LOG_FUNC_EXIT_PARAMS(Array)
    CATCH_BLOCK(DTWAIN_ARRAY())
}

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayCreateFromCap(DTWAIN_SOURCE Source, LONG lCapType, LONG lSize)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCapType, lSize))
    LONG lType = DTWAIN_GetCapArrayType(Source, lCapType);
    if ( lType == DTWAIN_FAILURE1 )
        LOG_FUNC_EXIT_PARAMS(NULL)
    DTWAIN_ARRAY Array = DTWAIN_ArrayCreate(lType, lSize);
    LOG_FUNC_EXIT_PARAMS(Array)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

LONG DLLENTRY_DEF DTWAIN_ArrayGetType(DTWAIN_ARRAY pArray)
{
    LOG_FUNC_ENTRY_PARAMS((pArray))
    LONG Ret = DTWAIN_ArrayType(pArray, FALSE);
    LOG_FUNC_EXIT_PARAMS(Ret)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG dynarithmic::DTWAIN_ArrayType(DTWAIN_ARRAY pArray, bool bGetReal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, bGetReal))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, DTWAIN_FAILURE1, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                                      [&] { return !EnumeratorFunctionImpl::EnumeratorIsValid(pArray); },
                                      DTWAIN_ERR_WRONG_ARRAY_TYPE, DTWAIN_FAILURE1, FUNC_MACRO);
    LONG Ret = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    LOG_FUNC_EXIT_PARAMS(Ret)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayCreateCopy( DTWAIN_ARRAY Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    DTWAIN_ARRAY Dest = 0;
    LONG lType = DTWAIN_ArrayGetType(Source);
    if ( lType == DTWAIN_FAILURE1 )
        LOG_FUNC_EXIT_PARAMS(NULL)
    Dest = DTWAIN_ArrayCreate(lType, 0);
    if ( !Dest )
        LOG_FUNC_EXIT_PARAMS(NULL)
    // Copy the arrays
    EnumeratorFunctionImpl::CopyArraysFromEnumerator(Dest, Source, EnumeratorFunctionImpl::GetEnumeratorType(Source));
    LOG_FUNC_EXIT_PARAMS(Dest)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayCopy(DTWAIN_ARRAY Source, DTWAIN_ARRAY Dest)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Dest))
        // Instance of class that takes an array that takes a handle (pointer)
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                                      [&] { return !EnumeratorFunctionImpl::EnumeratorIsValid(Source) ||
                                                   !EnumeratorFunctionImpl::EnumeratorIsValid(Dest); },
                                      DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG lType1 = DTWAIN_ArrayGetType(Source);
    LONG lType2 = DTWAIN_ArrayGetType(Dest);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return lType1 != lType2; },
                                     DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    // Copy the arrays
    EnumeratorFunctionImpl::CopyArraysFromEnumerator(Dest, Source, EnumeratorFunctionImpl::GetEnumeratorType(Source));
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL  DLLENTRY_DEF DTWAIN_ArrayAddN( DTWAIN_ARRAY pArray, LPVOID pVariant, LONG num )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariant, num))

        // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return ( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );},
                                    DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    EnumeratorFunctionImpl::EnumeratorAddValue(pArray, pVariant, num);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddLongN( DTWAIN_ARRAY pArray, LONG Val, LONG num )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAddN(pArray, &Val,num );
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false);
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddLong64N( DTWAIN_ARRAY pArray, LONG64 Val, LONG num )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAddN(pArray, &Val,num );
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false);
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddFloatN( DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, LONG num )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAddN(pArray, &Val,num );
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false);
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddStringN( DTWAIN_ARRAY pArray, LPCTSTR Val, LONG num )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAddN(pArray, (LPVOID)Val,num );
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false);
}

DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_ArrayAddWideStringN(DTWAIN_ARRAY pArray, LPCWSTR Val, LONG num )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAddN(pArray, (LPVOID)Val,num );
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false);
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddANSIStringN(DTWAIN_ARRAY pArray, LPCSTR Val, LONG num )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAddN(pArray, (LPVOID)Val,num );
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false);
}

DTWAIN_BOOL  DLLENTRY_DEF DTWAIN_ArrayAdd( DTWAIN_ARRAY pArray, LPVOID pVariant )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariant))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
            [&] {return ( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );}, DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    bool bAdded = false;
    switch (eType)
    {
        case CTL_EnumeratorStringType:
        {
            CTL_StringType sVal = (LPCTSTR)pVariant;
            bAdded = EnumeratorFunctionImpl::EnumeratorAddValue(pArray, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
        }
        break;
        case CTL_EnumeratorANSIStringType:
        {
            CTL_String sVal = (LPCSTR)pVariant;
            bAdded = EnumeratorFunctionImpl::EnumeratorAddValue(pArray, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
        }
        break;
        case CTL_EnumeratorWideStringType:
        {
            CTL_WString sVal = (LPCWSTR)pVariant;
            bAdded = EnumeratorFunctionImpl::EnumeratorAddValue(pArray, &sVal);
        }
        break;
        default:
            bAdded = EnumeratorFunctionImpl::EnumeratorAddValue(pArray, pVariant); //ArrayAddInternal(pArray, pVariant, 1);
    }
    LOG_FUNC_EXIT_PARAMS(bAdded)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddLong( DTWAIN_ARRAY pArray, LONG Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAdd(pArray, &Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddLong64( DTWAIN_ARRAY pArray, LONG64 Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAdd(pArray, &Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddFloat(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAdd(pArray, &Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddString(DTWAIN_ARRAY pArray, LPCTSTR Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAdd(pArray, (LPVOID)(Val));
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddWideString(DTWAIN_ARRAY pArray, LPCWSTR Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAdd(pArray, (LPVOID)(Val));
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayAddANSIString(DTWAIN_ARRAY pArray, LPCSTR Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayAdd(pArray, (LPVOID)(Val));
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_ARRAY  DLLENTRY_DEF  DTWAIN_ArrayCreateFromLongs(LPLONG pCArray, LONG nSize)
{
    LOG_FUNC_ENTRY_PARAMS((pCArray, nSize))
    DTWAIN_ARRAY Dest = 0;
    Dest = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 0);
    if ( !Dest )
        LOG_FUNC_EXIT_PARAMS(NULL)
    ENUMERATOR_INSERT_RANGE(Dest, 0, pCArray, nSize, int)
    LOG_FUNC_EXIT_PARAMS(Dest)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

DTWAIN_ARRAY  DLLENTRY_DEF  DTWAIN_ArrayCreateFromLong64s(LONG64* pCArray, LONG nSize)
{
    LOG_FUNC_ENTRY_PARAMS((pCArray, nSize))
    DTWAIN_ARRAY Dest = 0;
    Dest = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG64, nSize);
    if ( !Dest )
        LOG_FUNC_EXIT_PARAMS(NULL)
    ENUMERATOR_INSERT_RANGE(Dest, 0, pCArray, nSize, LONG64)
    LOG_FUNC_EXIT_PARAMS(Dest)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

DTWAIN_ARRAY  DLLENTRY_DEF  DTWAIN_ArrayCreateFromReals(double* pCArray, LONG nSize)
{
    LOG_FUNC_ENTRY_PARAMS((pCArray, nSize))
    DTWAIN_ARRAY Dest = 0;
    Dest = DTWAIN_ArrayCreate(DTWAIN_ARRAYFLOAT, nSize);
    if ( !Dest )
        LOG_FUNC_EXIT_PARAMS(NULL)
    ENUMERATOR_INSERT_RANGE(Dest, 0, pCArray, nSize, double)
    LOG_FUNC_EXIT_PARAMS(Dest)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_ARRAY  DLLENTRY_DEF  DTWAIN_ArrayCreateFromStrings(LPCTSTR* pCArray, LONG nSize)
{
    LOG_FUNC_ENTRY_PARAMS((pCArray, nSize))
    DTWAIN_ARRAY Dest = 0;
    Dest = DTWAIN_ArrayCreate(DTWAIN_ARRAYSTRING, nSize);
    if ( !Dest )
        LOG_FUNC_EXIT_PARAMS(NULL)
    CTL_StringArrayType tempArray;
    for (LONG i = 0; i < nSize; ++i)
    {
        tempArray.push_back(*pCArray);
        ++pCArray;
    }

    ENUMERATOR_INSERT_RANGE_STRING(Dest, 0, tempArray, nSize, CTL_StringType)
    LOG_FUNC_EXIT_PARAMS(Dest)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayRemoveAll( DTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((pArray))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                                    [&] {return (!EnumeratorFunctionImpl::EnumeratorIsValid(pArray)); },
                                    DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);
    EnumeratorFunctionImpl::ClearEnumerator(pArray);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayRemoveAt(  DTWAIN_ARRAY pArray, LONG nWhere)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                                    [&] {return (!EnumeratorFunctionImpl::EnumeratorIsValid(pArray)); },
                                    DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);
    LONG Count = (LONG)EnumeratorFunctionImpl::EnumeratorGetCount(pArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || nWhere >= Count);},
                                        DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);
    EnumeratorFunctionImpl::EnumeratorRemoveAt(pArray, nWhere);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayRemoveAtN(  DTWAIN_ARRAY pArray, LONG nWhere, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, num))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                                        [&] {return (!EnumeratorFunctionImpl::EnumeratorIsValid(pArray)); },
                                        DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG Count = (LONG)EnumeratorFunctionImpl::EnumeratorGetCount(pArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || nWhere >= Count);},
                                DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);
    EnumeratorFunctionImpl::EnumeratorRemoveAt(pArray, nWhere, num);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayInsertAt( DTWAIN_ARRAY pArray, LONG nWhere, LPVOID pVariant)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVariant))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                                        [&] {return (!EnumeratorFunctionImpl::EnumeratorIsValid(pArray)); },
                                        DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG Count = (LONG)EnumeratorFunctionImpl::EnumeratorGetCount(pArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || nWhere >= Count); }, DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);
    // Do something special for strings
    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    switch (eType )
    {
        case CTL_EnumeratorStringType:
        {
            CTL_StringType sVal = (LPCTSTR)pVariant;
            EnumeratorFunctionImpl::EnumeratorInsertAt(pArray, nWhere, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
        }
        break;
        case CTL_EnumeratorANSIStringType:
        {
            CTL_String sVal = (LPCSTR)pVariant;
            EnumeratorFunctionImpl::EnumeratorInsertAt(pArray, nWhere, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
        }
        break;
        case CTL_EnumeratorWideStringType:
        {
            CTL_WString sVal = (LPCWSTR)pVariant;
            EnumeratorFunctionImpl::EnumeratorInsertAt(pArray, nWhere, &sVal);
        }
        break;
        default:
            EnumeratorFunctionImpl::EnumeratorInsertAt(pArray, nWhere, pVariant);
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLong(DTWAIN_ARRAY pArray, LONG nWhere, LONG Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAt(pArray,nWhere,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLong64(DTWAIN_ARRAY pArray, LONG nWhere, LONG64 Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAt(pArray,nWhere,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtFloat(DTWAIN_ARRAY pArray, LONG nWhere,  DTWAIN_FLOAT Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAt(pArray,nWhere,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPCTSTR pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVal))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAt(pArray,nWhere,(LPVOID)pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtWideString(DTWAIN_ARRAY pArray, LONG nWhere, LPCWSTR pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVal))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAt(pArray,nWhere,(LPVOID)pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtANSIString(DTWAIN_ARRAY pArray, LONG nWhere, LPCSTR pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVal))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAt(pArray,nWhere,(LPVOID)pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayDestroy( DTWAIN_ARRAY pArray)
{
    LOG_FUNC_ENTRY_PARAMS((pArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
                                        [&] {return (!EnumeratorFunctionImpl::EnumeratorIsValid(pArray)); },
                                        DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    EnumeratorFunctionImpl::EnumeratorDestroy(pArray);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

LONG DLLENTRY_DEF DTWAIN_ArrayGetCount( DTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((pArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, -1L, FUNC_MACRO);
    // Check if array exists

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return ( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );},
                              DTWAIN_ERR_WRONG_ARRAY_TYPE, DTWAIN_FAILURE1, FUNC_MACRO);

    LONG Ret = EnumeratorFunctionImpl::EnumeratorGetCount(pArray);
    LOG_FUNC_EXIT_PARAMS(Ret)
    CATCH_BLOCK(0)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayResize(DTWAIN_ARRAY Array, LONG NewSize)
{
    LOG_FUNC_ENTRY_PARAMS((Array, NewSize))

    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] {return ( !EnumeratorFunctionImpl::EnumeratorIsValid( Array ) );},
                                      DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    EnumeratorFunctionImpl::ResizeEnumerator(Array, NewSize);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayGetAt( DTWAIN_ARRAY pArray, LONG nWhere, LPVOID pVariant)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVariant))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return ( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );},
                                DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(pArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (Count <= 0 || nWhere >= Count);},
                                DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);

    // Do something special for strings
    DTWAIN_BOOL bRet = FALSE;
    switch (EnumeratorFunctionImpl::GetEnumeratorType(pArray))
    {
        case CTL_EnumeratorStringType:
            bRet = DTWAIN_ArrayGetAtString(pArray, nWhere, (LPTSTR)pVariant);
        break;
        case CTL_EnumeratorWideStringType:
            bRet = DTWAIN_ArrayGetAtWideString(pArray, nWhere, (LPWSTR)pVariant);
        break;
        case CTL_EnumeratorANSIStringType:
            bRet = DTWAIN_ArrayGetAtANSIString(pArray, nWhere, (LPSTR)pVariant);
        break;
        // Do something special for frames
        case CTL_EnumeratorDTWAINFrameType:
        {
            auto vValues = EnumeratorVectorPtr<DTWAINFrameInternal>(pArray);
            LPLPVOID pVariant2 = (LPLPVOID)pVariant;
            *pVariant2 = &(*vValues)[nWhere];
        }
        break;
        default:
            bRet = EnumeratorFunctionImpl::EnumeratorGetAt(pArray, nWhere, pVariant);
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF  DTWAIN_ArrayGetAtLong(DTWAIN_ARRAY pArray, LONG nWhere, LPLONG pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVal))
    DTWAIN_BOOL bRet = DTWAIN_ArrayGetAt(pArray,nWhere,pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF  DTWAIN_ArrayGetAtLong64(DTWAIN_ARRAY pArray, LONG nWhere, LONG64* pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVal))
    DTWAIN_BOOL bRet = DTWAIN_ArrayGetAt(pArray,nWhere,pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF  DTWAIN_ArrayGetAtFloat(DTWAIN_ARRAY pArray, LONG nWhere, LPDTWAIN_FLOAT pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVal))
    DTWAIN_BOOL bRet = DTWAIN_ArrayGetAt(pArray,nWhere,pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

template <typename T, typename U>
static DTWAIN_BOOL StringGetter(DTWAIN_ARRAY pArray, LONG nWhere, T& sVal, U pStr, bool bNullTerminate=true)
{
    DTWAIN_BOOL bRet = EnumeratorFunctionImpl::EnumeratorGetAt(pArray, nWhere, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
    if (bRet)
    {
        std::copy(sVal.begin(), sVal.end(), pStr);
        if (bNullTerminate)
            pStr[sVal.length()] = typename T::value_type('\0');
    }
    return bRet;
}

template <typename T, typename U>
static T StringPtrGetter(DTWAIN_ARRAY pArray, LONG nWhere)
{
    auto vValues = EnumeratorVectorPtr<U>(pArray);
    if (vValues && !vValues->empty())
    {
        if ( nWhere >= 0 && nWhere < (LONG)vValues->size() )
            return (*vValues)[nWhere].c_str();
    }
    return NULL;
}

DTWAIN_BOOL    DLLENTRY_DEF  DTWAIN_ArrayGetAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPTSTR pStr)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pStr))
    CTL_StringType sVal;
    DTWAIN_BOOL bRet = StringGetter(pArray, nWhere, sVal, pStr);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

LPCTSTR DLLENTRY_DEF  DTWAIN_ArrayGetAtStringPtr(DTWAIN_ARRAY pArray, LONG nWhere)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere))
    LPCTSTR bRet = StringPtrGetter<LPCTSTR, CTL_StringType>(pArray, nWhere);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(LPCTSTR(0))
}

LPCWSTR DLLENTRY_DEF  DTWAIN_ArrayGetAtWideStringPtr(DTWAIN_ARRAY pArray, LONG nWhere)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere))
    LPCWSTR bRet = StringPtrGetter<LPCWSTR, CTL_WString>(pArray, nWhere);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(LPCWSTR(0))
}

LPCSTR DLLENTRY_DEF  DTWAIN_ArrayGetAtANSIStringPtr(DTWAIN_ARRAY pArray, LONG nWhere)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere))
    LPCSTR bRet = StringPtrGetter<LPCSTR, CTL_String>(pArray, nWhere);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(LPCSTR(0))
}

DTWAIN_BOOL DLLENTRY_DEF   DTWAIN_ArrayGetAtWideString(DTWAIN_ARRAY pArray, LONG nWhere, LPWSTR pStr)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pStr))
    CTL_WString sVal;
    DTWAIN_BOOL bRet = StringGetter(pArray, nWhere, sVal, pStr);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF   DTWAIN_ArrayGetAtANSIString(DTWAIN_ARRAY pArray, LONG nWhere, LPSTR pStr)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pStr))
    CTL_String sVal;
    DTWAIN_BOOL bRet = StringGetter(pArray, nWhere, sVal, pStr);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

LONG  DLLENTRY_DEF DTWAIN_ArrayFind( DTWAIN_ARRAY pArray, LPVOID pVariant )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariant))
    LONG FoundPos = -1;
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, -1L, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] {return ( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );},
                                      DTWAIN_ERR_WRONG_ARRAY_TYPE, -1, FUNC_MACRO);

    // Get correct array type
    FoundPos = EnumeratorFunctionImpl::EnumeratorFindValue(pArray, pVariant);
    LOG_FUNC_EXIT_PARAMS(FoundPos)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG DLLENTRY_DEF DTWAIN_ArrayFindLong( DTWAIN_ARRAY pArray, LONG Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    LONG lRet = DTWAIN_ArrayFind(pArray, &Val);
    LOG_FUNC_EXIT_PARAMS(lRet)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}


LONG DLLENTRY_DEF DTWAIN_ArrayFindLong64( DTWAIN_ARRAY pArray, LONG64 Val )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val))
    LONG lRet = DTWAIN_ArrayFind(pArray, &Val);
    LOG_FUNC_EXIT_PARAMS(lRet)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG DLLENTRY_DEF DTWAIN_ArrayFindFloat( DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, DTWAIN_FLOAT Tolerance )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, Tolerance))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, -1L, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( pArray, CTL_EnumeratorDoubleType ) );}, DTWAIN_ERR_WRONG_ARRAY_TYPE, -1, FUNC_MACRO);

    LONG lRet = EnumeratorFunctionImpl::EnumeratorFindValue(pArray, Val, Tolerance);
    LOG_FUNC_EXIT_PARAMS(lRet)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayInsertAtN( DTWAIN_ARRAY pArray, LONG nWhere, LPVOID pVariant, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pVariant, num))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] {return( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );},
                              DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(pArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (Count <= 0 || nWhere >= Count);},
                                DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);

    EnumeratorFunctionImpl::EnumeratorInsertAt(pArray, nWhere, num, pVariant);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLongN(DTWAIN_ARRAY pArray, LONG nWhere, LONG Val, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAtN(pArray,nWhere,&Val,num);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLong64N(DTWAIN_ARRAY pArray, LONG nWhere, LONG64 Val, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAtN(pArray,nWhere,&Val,num);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtFloatN(DTWAIN_ARRAY pArray, LONG nWhere, DTWAIN_FLOAT Val, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAtN(pArray,nWhere,&Val,num);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtStringN(DTWAIN_ARRAY pArray, LONG nWhere, LPCTSTR Val, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAtN(pArray,nWhere,(LPVOID)Val,num);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtWideStringN(DTWAIN_ARRAY pArray, LONG nWhere, LPCWSTR Val, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAtN(pArray,nWhere,(LPVOID)Val,num);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtANSIStringN(DTWAIN_ARRAY pArray, LONG nWhere, LPCSTR Val, LONG num)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val, num))
    DTWAIN_BOOL bRet = DTWAIN_ArrayInsertAtN(pArray,nWhere,(LPVOID)Val,num);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


LONG DLLENTRY_DEF DTWAIN_ArrayFindString( DTWAIN_ARRAY pArray, LPCTSTR pString )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pString))
    LONG lRet = DTWAIN_ArrayFind(pArray, (LPVOID)pString);
    LOG_FUNC_EXIT_PARAMS(lRet)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG DLLENTRY_DEF DTWAIN_ArrayFindWideString( DTWAIN_ARRAY pArray, LPCWSTR pString )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pString))
    LONG lRet = DTWAIN_ArrayFind(pArray, (LPVOID)pString);
    LOG_FUNC_EXIT_PARAMS(lRet)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG DLLENTRY_DEF DTWAIN_ArrayFindANSIString( DTWAIN_ARRAY pArray, LPCSTR pString )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pString))
    LONG lRet = DTWAIN_ArrayFind(pArray, (LPVOID)pString);
    LOG_FUNC_EXIT_PARAMS(lRet)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

DTWAIN_BOOL  DLLENTRY_DEF DTWAIN_ArraySetAt( DTWAIN_ARRAY pArray, LONG lPos, LPVOID pVariant )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lPos, pVariant))
        // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return ( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );},
                                        DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(pArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] {return (Count <= 0 || lPos >= Count);}, DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);

    int enumType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    switch (enumType)
    {
        // Do something special for strings
        case CTL_EnumeratorStringType:
        {
            CTL_StringType sVal = (LPCTSTR)pVariant;
            EnumeratorFunctionImpl::EnumeratorSetAt(pArray, lPos, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
        }
        break;
        case CTL_EnumeratorWideStringType:
        {
            CTL_WString sVal = (LPCWSTR)pVariant;
            EnumeratorFunctionImpl::EnumeratorSetAt(pArray, lPos, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
        }
        break;
        case CTL_EnumeratorANSIStringType:
        {
            CTL_String sVal = (LPCSTR)pVariant;
            EnumeratorFunctionImpl::EnumeratorSetAt(pArray, lPos, &sVal); //ArrayAddInternal(pArray, pVariant, 1);
        }
        break;

        default:
            EnumeratorFunctionImpl::EnumeratorSetAt(pArray, lPos, pVariant);
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtLong(DTWAIN_ARRAY pArray, LONG nWhere, LONG Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArraySetAt(pArray,nWhere,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtLong64(DTWAIN_ARRAY pArray, LONG nWhere, LONG64 Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArraySetAt(pArray,nWhere,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtFloat(DTWAIN_ARRAY pArray, LONG nWhere, DTWAIN_FLOAT Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, Val))
    DTWAIN_BOOL bRet = DTWAIN_ArraySetAt(pArray,nWhere,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPCTSTR pStr)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pStr))
    DTWAIN_BOOL bRet = DTWAIN_ArraySetAt(pArray,nWhere,(LPVOID)pStr);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtWideString(DTWAIN_ARRAY pArray, LONG nWhere, LPCWSTR pStr)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pStr))
    DTWAIN_BOOL bRet = DTWAIN_ArraySetAt(pArray,nWhere,(LPVOID)pStr);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtANSIString(DTWAIN_ARRAY pArray, LONG nWhere, LPCSTR pStr)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhere, pStr))
    DTWAIN_BOOL bRet = DTWAIN_ArraySetAt(pArray,nWhere,(LPVOID)pStr);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

LPVOID DLLENTRY_DEF DTWAIN_ArrayGetBuffer( DTWAIN_ARRAY pArray, LONG nOffset )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nOffset))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, NULL, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return ( !EnumeratorFunctionImpl::EnumeratorIsValid( pArray ) );},
        DTWAIN_ERR_WRONG_ARRAY_TYPE, NULL, FUNC_MACRO);

    // Get correct array type
    LPVOID v = EnumeratorFunctionImpl::EnumeratorGetBuffer(pArray, nOffset);
    LOG_FUNC_EXIT_PARAMS(v)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

// Implement DTWAIN_RANGE functions!!!!!!!!!!

/* Range functions */
DTWAIN_RANGE DLLENTRY_DEF DTWAIN_RangeCreate(LONG nEnumType)
{
    LOG_FUNC_ENTRY_PARAMS((nEnumType))
    DTWAIN_RANGE Array = (DTWAIN_RANGE)DTWAIN_ArrayCreate( nEnumType, 5 );
    if ( !Array )
        LOG_FUNC_EXIT_PARAMS(NULL)
    LOG_FUNC_EXIT_PARAMS(Array)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_RANGE DLLENTRY_DEF DTWAIN_RangeCreateFromCap(DTWAIN_SOURCE Source, LONG lCapType)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCapType))
    DTWAIN_RANGE Array = (DTWAIN_RANGE)DTWAIN_ArrayCreateFromCap(Source, lCapType, 5 );
    if ( !Array )
        LOG_FUNC_EXIT_PARAMS(NULL)
    LOG_FUNC_EXIT_PARAMS(Array)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeDestroy(DTWAIN_RANGE Range)
{
    LOG_FUNC_ENTRY_PARAMS((Range))
    DTWAIN_BOOL bRet = DTWAIN_ArrayDestroy((DTWAIN_ARRAY)Range);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeIsValid(DTWAIN_RANGE Range, LPLONG pStatus )
{
    LOG_FUNC_ENTRY_PARAMS((Range, pStatus))
    LONG lCurStatus;
    if ( (lCurStatus = IsValidRangeArray(Range)) != 1 )
    {
        if ( pStatus )
            *pStatus = lCurStatus;
        LOG_FUNC_EXIT_PARAMS(false)
    }
    if (pStatus)
        *pStatus = 1;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

static LONG IsValidRangeArray( DTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((pArray))

    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, DTWAIN_ERR_BAD_HANDLE, FUNC_MACRO);

    // Check if array is a valid type for ranges
    if ( !EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, CTL_EnumeratorIntType) && !EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, CTL_EnumeratorDoubleType))
        LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_WRONG_ARRAY_TYPE)

    // Check if the array has at least 5 elements
    if ( DTWAIN_ArrayGetCount( pArray ) < 5 )
        LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_BAD_ARRAY)

    // Check if (low < high) and 0 < step < (high - low)
    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    if ( eType == CTL_EnumeratorIntType )
    {
        LONG lLow, lUp, lStep;
        int *pVals = (int *)EnumeratorFunctionImpl::EnumeratorGetBuffer(pArray, 0);
        lLow = pVals[0];
        lUp = pVals[1];
        lStep = pVals[2];

        LOG_FUNC_VALUES_EX((lLow, lUp, lStep))

        if ( lLow > lUp )
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_INVALID_RANGE)
        if ( lStep < 0 )
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_INVALID_RANGE)
         if ( lStep == 0 && lLow < lUp )
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_INVALID_RANGE)
    }
    else
    if (eType == CTL_EnumeratorDoubleType )
    {
        double dLow, dUp, dStep;
        double *pVals = (double*)EnumeratorFunctionImpl::EnumeratorGetBuffer(pArray, 0);
        dLow = pVals[0];
        dUp = pVals[1];
        dStep = pVals[2];
        LOG_FUNC_VALUES_EX((dLow, dUp, dStep))
        if ( dLow > dUp )
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_INVALID_RANGE)
        if ( dStep < 0 )
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_INVALID_RANGE)
        if ( FLOAT_CLOSE(dStep,0.0) && dLow < dUp )
            LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_INVALID_RANGE)
    }
    LOG_FUNC_EXIT_PARAMS(1)
    CATCH_BLOCK(0)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeSetValue( DTWAIN_RANGE pArray, LONG nWhich, LPVOID pVariant )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, pVariant))
    LONG lResult = IsValidRangeArray( pArray );
    if ( lResult != 1 && lResult != DTWAIN_ERR_INVALID_RANGE)
        LOG_FUNC_EXIT_PARAMS(false)
    DTWAIN_BOOL bRet = DTWAIN_ArraySetAt((DTWAIN_ARRAY)pArray, nWhich, pVariant);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetValueLong( DTWAIN_RANGE pArray, LONG nWhich,LONG Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, Val))
    DTWAIN_BOOL bRet = DTWAIN_RangeSetValue(pArray,nWhich,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetValueFloat( DTWAIN_RANGE pArray, LONG nWhich,DTWAIN_FLOAT Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, Val))
    DTWAIN_BOOL bRet = DTWAIN_RangeSetValue(pArray,nWhich,&Val);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetValueFloatString( DTWAIN_RANGE pArray, LONG nWhich, LPCTSTR Val)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, Val))
    double d = StringWrapper::ToDouble(Val);
    DTWAIN_BOOL bRet = DTWAIN_RangeSetValueFloat(pArray,nWhich, d);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetValue( DTWAIN_RANGE pArray, LONG nWhich, LPVOID pVariant )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, pVariant))
    LONG lResult = IsValidRangeArray( pArray );
    if ( lResult != 1 && lResult != DTWAIN_ERR_INVALID_RANGE)
        LOG_FUNC_EXIT_PARAMS(false)
    DTWAIN_BOOL bRet = DTWAIN_ArrayGetAt((DTWAIN_ARRAY)pArray, nWhich, pVariant);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetValueLong( DTWAIN_RANGE pArray, LONG nWhich,LPLONG pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, pVal))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetValue(pArray,nWhich,pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetValueFloat( DTWAIN_RANGE pArray, LONG nWhich, LPDTWAIN_FLOAT pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, pVal))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetValue(pArray,nWhich,pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF   DTWAIN_RangeGetValueFloatString( DTWAIN_RANGE pArray, LONG nWhich, LPTSTR pVal)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, nWhich, pVal))
    double d;
    DTWAIN_BOOL bRet = DTWAIN_RangeGetValueFloat(pArray, nWhich, &d);
    if ( bRet )
    {
        CTL_StringStreamType strm;
        strm << BOOST_FORMAT(_T("%1%")) % d;
        StringWrapper::SafeStrcpy(pVal, strm.str().c_str());
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

struct ArrayLongTraits
{
    static void SetAt(DTWAIN_ARRAY a, LONG nWhere, LONG value)
    {
        auto& vValues = EnumeratorVector<int>(a);
        vValues[nWhere] = value;
    }
    typedef LONG value_type;
};

struct ArrayFloatTraits
{
    static void SetAt(DTWAIN_ARRAY a, LONG nWhere, DTWAIN_FLOAT value)
    {
        auto& vValues = EnumeratorVector<double>(a);
        vValues[nWhere] = value;
    }
    typedef DTWAIN_FLOAT value_type;
};

template <typename Traits>
void DTWAIN_RangeSetter(DTWAIN_ARRAY a,
                        LPVOID valueLow,
                        LPVOID valueUp,
                        LPVOID valueStep,
                        LPVOID valueCurrent,
                        LPVOID pDefault)
{
    Traits::SetAt(a, DTWAIN_RANGEMIN, *(typename Traits::value_type*)valueLow);
    Traits::SetAt(a, DTWAIN_RANGEMAX, *(typename Traits::value_type*)valueUp);
    Traits::SetAt(a, DTWAIN_RANGESTEP, *(typename Traits::value_type*)valueStep);

    if ( pDefault )
        Traits::SetAt(a, DTWAIN_RANGEDEFAULT, *(typename Traits::value_type*)pDefault);
    else
        Traits::SetAt(a, DTWAIN_RANGEDEFAULT, (typename Traits::value_type)0);

    if (valueCurrent)
        Traits::SetAt(a, DTWAIN_RANGECURRENT, *(typename Traits::value_type*)valueCurrent);
    else
        Traits::SetAt(a, DTWAIN_RANGECURRENT, (typename Traits::value_type)0);
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeSetAll( DTWAIN_RANGE pArray, LPVOID pVariantLow,
                                            LPVOID pVariantUp, LPVOID pVariantStep,
                                            LPVOID pDefault, LPVOID pCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariantLow,pVariantUp, pVariantStep,pDefault, pCurrent ))

    LONG lResult = IsValidRangeArray( pArray );
    if ( lResult != 1 && lResult != DTWAIN_ERR_INVALID_RANGE)
        LOG_FUNC_EXIT_PARAMS(false)

    if ( DTWAIN_ArrayGetType(pArray) == DTWAIN_ARRAYLONG )
        DTWAIN_RangeSetter<ArrayLongTraits>(pArray, pVariantLow, pVariantUp, pVariantStep, pCurrent, pDefault);
    else
        DTWAIN_RangeSetter<ArrayFloatTraits>(pArray, pVariantLow, pVariantUp, pVariantStep, pCurrent, pDefault);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetAllLong( DTWAIN_RANGE pArray, LONG lLow,
                                                        LONG lUp, LONG lStep,
                                                        LONG lDefault,
                                                        LONG lCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lLow,lUp,lStep,lDefault,lCurrent ))
    DTWAIN_BOOL bRet = DTWAIN_RangeSetAll(pArray,&lLow,&lUp,&lStep,&lDefault,&lCurrent);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetAllFloat( DTWAIN_RANGE pArray, DTWAIN_FLOAT dLow,
                                                         DTWAIN_FLOAT dUp, DTWAIN_FLOAT dStep,
                                                         DTWAIN_FLOAT dDefault,
                                                         DTWAIN_FLOAT dCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, dLow, dUp,dStep,dDefault,dCurrent ))
    DTWAIN_BOOL bRet = DTWAIN_RangeSetAll(pArray,&dLow,&dUp,&dStep,&dDefault,&dCurrent);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetAllFloatString( DTWAIN_RANGE pArray, LPCTSTR dLow,
                                                                LPCTSTR dUp, LPCTSTR dStep, LPCTSTR dDefault,LPCTSTR dCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, dLow, dUp,dStep,dDefault,dCurrent ))
    double d[5];
    LPCTSTR* vals[] = {&dLow, &dUp, &dStep, &dDefault, &dCurrent};
    for (int i = 0; i < 5; ++i )
      d[i] = StringWrapper::ToDouble(*vals[i]);
    DTWAIN_BOOL bRet = DTWAIN_RangeSetAllFloat(pArray, d[0], d[1], d[2], d[3], d[4]);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetAll( DTWAIN_RANGE pArray, LPVOID pVariantLow,
                                            LPVOID pVariantUp, LPVOID pVariantStep,
                                            LPVOID pDefault, LPVOID pCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariantLow,pVariantUp, pVariantStep,pDefault, pCurrent ))
    LONG lResult = IsValidRangeArray( pArray );
    if ( lResult != 1 && lResult != DTWAIN_ERR_INVALID_RANGE)
        LOG_FUNC_EXIT_PARAMS(false)
    if ( pVariantLow )
        DTWAIN_ArrayGetAt(pArray, DTWAIN_RANGEMIN, pVariantLow);
    if ( pVariantUp )
        DTWAIN_ArrayGetAt(pArray, DTWAIN_RANGEMAX, pVariantUp);
    if ( pVariantStep )
        DTWAIN_ArrayGetAt(pArray, DTWAIN_RANGESTEP, pVariantStep);
    if ( pDefault )
        DTWAIN_ArrayGetAt(pArray, DTWAIN_RANGEDEFAULT, pDefault);
    if ( pCurrent )
        DTWAIN_ArrayGetAt(pArray, DTWAIN_RANGECURRENT, pCurrent);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetAllLong( DTWAIN_RANGE pArray, LPLONG lLow,
                                                        LPLONG lUp, LPLONG lStep,
                                                        LPLONG lDefault,
                                                        LPLONG lCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lLow, lUp, lStep, lDefault, lCurrent ))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetAll(pArray,lLow,lUp,lStep,lDefault,lCurrent);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetAllFloat( DTWAIN_RANGE pArray, LPDTWAIN_FLOAT dLow,
                                                         LPDTWAIN_FLOAT dUp, LPDTWAIN_FLOAT dStep,
                                                         LPDTWAIN_FLOAT dDefault,
                                                         LPDTWAIN_FLOAT dCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, dLow, dUp, dStep, dDefault, dCurrent ))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetAll(pArray,dLow,dUp,dStep,dDefault,dCurrent);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetAllFloatString( DTWAIN_RANGE pArray, LPTSTR dLow,
                                                         LPTSTR dUp, LPTSTR dStep,
                                                         LPTSTR dDefault,
                                                         LPTSTR dCurrent )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, dLow, dUp, dStep, dDefault, dCurrent ))
    double d[5];
    DTWAIN_BOOL bRet = DTWAIN_RangeGetAllFloat(pArray, &d[0], &d[1], &d[2], &d[3], &d[4]);
    if ( bRet )
    {
        LPTSTR* vals[] = { &dLow, &dUp, &dStep, &dDefault, &dCurrent };
        CTL_StringStreamType strm;
        for (int i = 0; i < 5; ++i )
        {
            strm << BOOST_FORMAT(_T("%1%")) % d[i];
            StringWrapper::SafeStrcpy(*vals[i], strm.str().c_str());
            strm.str(_T(""));
        }
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

LONG DLLENTRY_DEF DTWAIN_RangeGetCount( DTWAIN_RANGE pArray )
{
    LOG_FUNC_ENTRY_PARAMS((pArray))
    LONG lError;
    if (( lError = IsValidRangeArray( (DTWAIN_ARRAY)pArray )) < 0 )
        LOG_FUNC_EXIT_PARAMS(DTWAIN_FAILURE1)

    // Check if (low < high) and 0 < step < (high - low)
    LONG nNumItems;
    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    if ( eType == CTL_EnumeratorIntType )
    {
        LONG lLow, lUp, lStep, lDef, lCur;
        DTWAIN_RangeGetAll( pArray, &lLow, &lUp, &lStep, &lDef, &lCur );
        nNumItems = abs(lUp - lLow) / lStep + 1;
    }
    else
    {
        double dLow, dUp, dStep, dDef, dCur;
        DTWAIN_RangeGetAll( pArray, &dLow, &dUp, &dStep, &dDef, &dCur );
        nNumItems = static_cast<LONG>((float)(fabs(dUp - dLow) / dStep) + 1);
    }
    LOG_FUNC_EXIT_PARAMS(nNumItems)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetExpValue( DTWAIN_RANGE pArray, LONG lPos, LPVOID pVariant )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lPos, pVariant))
    LONG lError;
    if (( lError = IsValidRangeArray( (DTWAIN_ARRAY)pArray )) < 0 )
        LOG_FUNC_EXIT_PARAMS(false)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (lPos < 0);}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    // Check if (low < high) and 0 < step < (high - low)
    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    if ( eType == CTL_EnumeratorIntType )
    {
        LONG *pLong;
        pLong = (LONG *)pVariant;
        LONG lLow, lUp, lStep, lDef, lCur;
        DTWAIN_RangeGetAll( pArray, &lLow, &lUp, &lStep, &lDef, &lCur );
        *pLong = lLow + ( lStep * lPos );
    }
    else
    {
        double *pFloat = (double *)pVariant;
        double dLow, dUp, dStep, dDef, dCur;
        DTWAIN_RangeGetAll( pArray, &dLow, &dUp, &dStep, &dDef, &dCur );
        *pFloat = dLow + ( dStep * lPos );
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetExpValueLong( DTWAIN_RANGE pArray, LONG lPos, LPLONG pVal )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lPos, pVal))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetExpValue(pArray,lPos,pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetExpValueFloat( DTWAIN_RANGE pArray, LONG lPos, LPDTWAIN_FLOAT pVal )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lPos, pVal))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetExpValue(pArray,lPos,pVal);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetExpValueFloatString( DTWAIN_RANGE pArray, LONG lPos, LPTSTR pVal )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lPos, pVal))
    double d;
    DTWAIN_BOOL bRet = DTWAIN_RangeGetExpValueFloat(pArray,lPos,&d);
    if ( bRet )
    {
        CTL_StringStreamType strm;
        strm << BOOST_FORMAT(_T("%1%")) % d;
        StringWrapper::SafeStrcpy(pVal, strm.str().c_str());
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetPos( DTWAIN_RANGE pArray, LPVOID pVariant, LPLONG pPos )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariant, pPos))
    LONG lError;
    if (( lError = IsValidRangeArray( (DTWAIN_ARRAY)pArray )) < 0 )
        LOG_FUNC_EXIT_PARAMS(false)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return (!pVariant || !pPos);}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    // Check if (low < high) and 0 < step < (high - low)
    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);
    if ( eType == CTL_EnumeratorIntType )
    {
        LONG *pLong;
        pLong = (LONG *)pVariant;
        LONG lLow, lUp, lStep, lDef, lCur;
        DTWAIN_RangeGetAll( pArray, &lLow, &lUp, &lStep, &lDef, &lCur );
        if ( lStep == 0 )
            LOG_FUNC_EXIT_PARAMS(false)
        *pPos = (*pLong - lLow) / lStep;
    }
    else
    {
        double *pFloat = (double *)pVariant;
        double dLow, dUp, dStep, dDef, dCur;
        DTWAIN_RangeGetAll( pArray, &dLow, &dUp, &dStep, &dDef, &dCur );
        if ( FLOAT_CLOSE(0.0, dStep))
            LOG_FUNC_EXIT_PARAMS(false)
        *pPos = static_cast<LONG>((*pFloat - dLow) / dStep);
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetPosFloat( DTWAIN_RANGE pArray, DTWAIN_FLOAT pVariant, LPLONG pPos )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariant, pPos))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetPos(pArray, &pVariant, pPos);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetPosFloatString( DTWAIN_RANGE pArray, LPCTSTR Val, LPLONG pPos )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Val, pPos))
    double d = StringWrapper::ToDouble(Val);
    DTWAIN_BOOL bRet = DTWAIN_RangeGetPosFloat(pArray, d, pPos);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetPosLong( DTWAIN_RANGE pArray, LONG Value, LPLONG pPos )
{
    LOG_FUNC_ENTRY_PARAMS((pArray, Value, pPos))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetPos(pArray, &Value, pPos);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeExpand(DTWAIN_RANGE Range, LPDTWAIN_ARRAY Array)
{
    LOG_FUNC_ENTRY_PARAMS((Range, Array))
    LONG lError;
    if (( lError = IsValidRangeArray( (DTWAIN_ARRAY)Range )) < 0 )
        LOG_FUNC_EXIT_PARAMS(false)

        // Check if array exists
        // Instance of class that takes an array that takes a handle (pointer)
    LONG nArrayType = DTWAIN_ArrayGetType(Range);
    LONG lCount = DTWAIN_RangeGetCount( Range );

    DTWAIN_ARRAY pDest = DTWAIN_ArrayCreate( nArrayType, lCount );
    if ( !pDest )
        LOG_FUNC_EXIT_PARAMS(false)

    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(Range);

    if ( eType == CTL_EnumeratorIntType )
    {
        LONG i;
        LONG lLow, lUp, lStep, lDef, lCur;
        DTWAIN_RangeGetAll( Range, &lLow, &lUp, &lStep, &lDef, &lCur );
        auto& pArrayBuf = EnumeratorVector<int>(pDest);
        i = 0;
        std::transform(pArrayBuf.begin(), pArrayBuf.end(), pArrayBuf.begin(), [&](int /*n*/)
        {
            int retVal = static_cast<int>(lLow + (lStep * i));
            ++i;
            return retVal;
        });
    }
    else
    {
        LONG i = 0;
        double dLow, dUp, dStep, dDef, dCur;
        DTWAIN_RangeGetAll( Range, &dLow, &dUp, &dStep, &dDef, &dCur );

        auto& pArrayBuf = EnumeratorVector<double>(pDest);
        std::transform(pArrayBuf.begin(), pArrayBuf.end(), pArrayBuf.begin(), [&](double /*d*/)
        {
            double dValue = dLow + (dStep * i);
            ++i;
            return dValue;
        });
    }
    // Destroy the user arra if one is supplied
    if (EnumeratorFunctionImpl::EnumeratorIsValidNoCheck(*Array))
        EnumeratorFunctionImpl::EnumeratorDestroy(*Array);

    //    DTWAIN_ArrayDestroy((DTWAIN_ARRAY)*Array);
    *Array = pDest;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetNearestValue( DTWAIN_RANGE pArray, LPVOID pVariantIn,
                                                     LPVOID pVariantOut, LONG RoundType)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, pVariantIn, pVariantOut, RoundType))
    LONG lError;
    if (( lError = IsValidRangeArray( (DTWAIN_ARRAY)pArray )) < 0 )
    LOG_FUNC_EXIT_PARAMS(false)

    CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(pArray);

    if ( eType == CTL_EnumeratorIntType )
    {
        LONG lLow, lUp, lStep, lDef, lCur;

        // Get the values
        DTWAIN_RangeGetAll( pArray, &lLow, &lUp, &lStep, &lDef, &lCur );

        // Get the value passed in
        LONG lInVal = *(LONG *)pVariantIn;
        LONG *pOutVal = (LONG *)pVariantOut;

        // return immediately if step is 0
        if ( lStep == 0 )
        {
            *pOutVal = lLow;
            LOG_FUNC_EXIT_PARAMS(true)
        }

        // Check if value passed in is out of bounds
        if ( lInVal < lLow )
        {
            *pOutVal = lLow;
            LOG_FUNC_EXIT_PARAMS(true)
        }
        else
        if ( lInVal > lUp )
        {
            *pOutVal = lUp;
            LOG_FUNC_EXIT_PARAMS(true)
        }

        LONG Remainder;
        LONG Dividend;

        // Get the nearest value to *pVariantIn;
        // First get the bias value from 0
        LONG lBias = 0;
        if ( lLow != 0 )
            lBias = -lLow;

        lInVal += lBias;

        Remainder = abs(lInVal % lStep);
        Dividend = lInVal / lStep;

        if ( Remainder == 0 )
        {
            *pOutVal = lInVal - lBias;
            LOG_FUNC_EXIT_PARAMS(true)
        }

        // Check if round to lowest or highest valid value
        if ( RoundType == DTWAIN_ROUNDNEAREST )
        {
            if ( Remainder >= abs(lStep) / 2 )
                RoundType = DTWAIN_ROUNDUP;
            else
                RoundType = DTWAIN_ROUNDDOWN;
        }

        if ( RoundType == DTWAIN_ROUNDDOWN )
            *pOutVal = (Dividend * lStep) - lBias;
        else
            if ( RoundType == DTWAIN_ROUNDUP )
                *pOutVal = ((Dividend + 1) * lStep) - lBias;
        LOG_FUNC_EXIT_PARAMS(true)
    }
    else
    if ( eType == CTL_EnumeratorDoubleType )
    {
        double dLow, dUp, dStep, dDef, dCur;

        // Get the values
        DTWAIN_RangeGetAll( pArray, &dLow, &dUp, &dStep, &dDef, &dCur );

        double Remainder;
        double Dividend;


        // Get the value passed in
        double dInVal = *(double *)pVariantIn;
        double *pOutVal = (double *)pVariantOut;

        // Check if value passed in is out of bounds
        if (FLOAT_CLOSE(dLow, dInVal) ||
            FLOAT_CLOSE(0.0, dStep) ||
            (dInVal < dLow))
        {
            *pOutVal = dLow;
            LOG_FUNC_EXIT_PARAMS(true)
        }
        else
        if (FLOAT_CLOSE(dUp, dInVal) || (dInVal > dUp))
        {
            *pOutVal = dUp;
            LOG_FUNC_EXIT_PARAMS(true)
        }

        //        if (FLOAT_CLOSE(0.0, dStep))

        // Get the nearest value to *pVariantIn;
        // First get the bias value from 0
        double dBias = 0;
        if ( dLow != 0 )
            dBias = -dLow;

        dInVal += dBias;
        Remainder = fabs(fmod(dInVal, dStep));
        Dividend = (double)(LONG)(dInVal / dStep);

        if ( FLOAT_CLOSE(Remainder,0.0 ))
        {
            *pOutVal = dInVal - dBias;
            LOG_FUNC_EXIT_PARAMS(true)
        }

        // Check if round to lowest or highest valid value
        if ( RoundType == DTWAIN_ROUNDNEAREST )
        {
            if ( Remainder >= fabs(dStep) / 2.0 )
                RoundType = DTWAIN_ROUNDUP;
            else
                RoundType = DTWAIN_ROUNDDOWN;
        }
        if ( RoundType == DTWAIN_ROUNDDOWN )
            *pOutVal = Dividend * dStep - dBias;
        else
            if ( RoundType == DTWAIN_ROUNDUP )
                *pOutVal = (Dividend + 1.0) * dStep - dBias;
        LOG_FUNC_EXIT_PARAMS(true)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeNearestValueLong( DTWAIN_RANGE pArray, LONG lIn,
                                                              LPLONG pOut, LONG RoundType)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, lIn, pOut, RoundType))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetNearestValue(pArray,&lIn,pOut,RoundType);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeNearestValueFloat( DTWAIN_RANGE pArray, DTWAIN_FLOAT dIn,
                                                             LPDTWAIN_FLOAT pOut, LONG RoundType)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, dIn, pOut, RoundType))
    DTWAIN_BOOL bRet = DTWAIN_RangeGetNearestValue(pArray,&dIn,pOut,RoundType);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeNearestValueFloatString( DTWAIN_RANGE pArray, LPCTSTR dIn,
                                                                      LPTSTR pOut, LONG RoundType)
{
    LOG_FUNC_ENTRY_PARAMS((pArray, dIn, pOut, RoundType))
    double d = StringWrapper::ToDouble(dIn);
    double dOut;
    DTWAIN_BOOL bRet = DTWAIN_RangeNearestValueFloat(pArray, d, &dOut,RoundType);
    if ( bRet )
    {
        CTL_StringStreamType strm;
        strm << BOOST_FORMAT(_T("%1%")) % dOut;
        StringWrapper::SafeStrcpy(pOut, strm.str().c_str());
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

///////////////////////////////////////////////// Frame functions //////////////////////////////////////////
DTWAIN_FRAME DLLENTRY_DEF DTWAIN_FrameCreate(DTWAIN_FLOAT Left, DTWAIN_FLOAT Top, DTWAIN_FLOAT Right, DTWAIN_FLOAT Bottom)
{
    LOG_FUNC_ENTRY_PARAMS((Left, Top, Right, Bottom))
    CTL_TwainDLLHandle::s_EnumeratorFactory->m_AvailableFrameValues.push_back(DTWAINFrameInternal(Left, Top, Right, Bottom));
    DTWAIN_FRAME Frame = &CTL_TwainDLLHandle::s_EnumeratorFactory->m_AvailableFrameValues.back();
    LOG_FUNC_EXIT_PARAMS((DTWAIN_FRAME)Frame)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFrameGetAt(DTWAIN_ARRAY FrameArray, LONG nWhere,
                                                LPDTWAIN_FLOAT pleft, LPDTWAIN_FLOAT ptop,
                                                LPDTWAIN_FLOAT pright, LPDTWAIN_FLOAT pbottom )
{
    LOG_FUNC_ENTRY_PARAMS((FrameArray, nWhere, pleft, ptop, pright, pbottom))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] {return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( FrameArray, CTL_EnumeratorDTWAINFrameType));},
            DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(FrameArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || nWhere >= Count);},
                                        DTWAIN_ERR_INDEX_BOUNDS, NULL, FUNC_MACRO);

    DTWAINFrameInternal& theFrame = EnumeratorVector<DTWAINFrameInternal>(FrameArray)[nWhere];
    if ( pleft )
        *pleft = theFrame.m_FrameComponent[DTWAIN_FRAMELEFT];
    if ( ptop )
        *ptop = theFrame.m_FrameComponent[DTWAIN_FRAMETOP];
    if ( pright )
        *pright = theFrame.m_FrameComponent[DTWAIN_FRAMERIGHT];
    if ( pbottom )
        *pbottom = theFrame.m_FrameComponent[DTWAIN_FRAMEBOTTOM];

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_FRAME DLLENTRY_DEF DTWAIN_ArrayFrameGetFrameAt(DTWAIN_ARRAY FrameArray, LONG nWhere )
{
    LOG_FUNC_ENTRY_PARAMS((FrameArray, nWhere))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);

    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( FrameArray, CTL_EnumeratorDTWAINFrameType));},
    DTWAIN_ERR_WRONG_ARRAY_TYPE, NULL, FUNC_MACRO);

    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(FrameArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || nWhere >= Count); },
                                    DTWAIN_ERR_INDEX_BOUNDS, NULL, FUNC_MACRO);

    auto& vValues = EnumeratorVector<DTWAINFrameInternal>(FrameArray);
    DTWAIN_FRAME theFrame = &vValues[nWhere];
    LOG_FUNC_EXIT_PARAMS(theFrame)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFrameSetAt(DTWAIN_ARRAY FrameArray, LONG nWhere, DTWAIN_FLOAT left, DTWAIN_FLOAT top, DTWAIN_FLOAT right, DTWAIN_FLOAT bottom )
{
    LOG_FUNC_ENTRY_PARAMS((FrameArray, nWhere, left, top, right, bottom))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( FrameArray, CTL_EnumeratorDTWAINFrameType));},
                DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(FrameArray);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || nWhere >= Count); },
                                    DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);

    DTWAINFrameInternal& theFrame = EnumeratorVector<DTWAINFrameInternal>(FrameArray)[nWhere];
    theFrame.m_FrameComponent[DTWAIN_FRAMELEFT] = left;
    theFrame.m_FrameComponent[DTWAIN_FRAMETOP] = top;
    theFrame.m_FrameComponent[DTWAIN_FRAMERIGHT] = right;
    theFrame.m_FrameComponent[DTWAIN_FRAMEBOTTOM] = bottom;

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameIsValid(DTWAIN_FRAME Frame)
{
    LOG_FUNC_ENTRY_PARAMS((Frame))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    DTWAINFrameInternal* pPtr = (DTWAINFrameInternal*)Frame;
    pPtr = GetDTWAINFramePtr(pPtr);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return !pPtr;},
                                      DTWAIN_ERR_INVALID_DTWAIN_FRAME, false, FUNC_MACRO);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameDestroy(DTWAIN_FRAME Frame)
{
    LOG_FUNC_ENTRY_PARAMS((Frame))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    DTWAINFrameInternal* pPtr = (DTWAINFrameInternal*)Frame;
    pPtr = GetDTWAINFramePtr(pPtr);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !pPtr;},
                                      DTWAIN_ERR_INVALID_DTWAIN_FRAME, false, FUNC_MACRO);

    DTWAINFrameList::iterator it = std::find(
        CTL_TwainDLLHandle::s_EnumeratorFactory->m_AvailableFrameValues.begin(),
        CTL_TwainDLLHandle::s_EnumeratorFactory->m_AvailableFrameValues.end(), *pPtr);

    if ( it != CTL_TwainDLLHandle::s_EnumeratorFactory->m_AvailableFrameValues.end())
        CTL_TwainDLLHandle::s_EnumeratorFactory->m_AvailableFrameValues.erase(it);

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameSetAll(DTWAIN_FRAME Frame,DTWAIN_FLOAT Left,
                                            DTWAIN_FLOAT Top, DTWAIN_FLOAT Right,
                                            DTWAIN_FLOAT Bottom)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, Left, Top, Right, Bottom))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    DTWAINFrameInternal* pPtr = (DTWAINFrameInternal*)Frame;
    pPtr = GetDTWAINFramePtr(pPtr);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !pPtr; },
        DTWAIN_ERR_INVALID_DTWAIN_FRAME, false, FUNC_MACRO);
    pPtr->m_FrameComponent[DTWAIN_FRAMELEFT] = Left;
    pPtr->m_FrameComponent[DTWAIN_FRAMETOP] = Top;
    pPtr->m_FrameComponent[DTWAIN_FRAMERIGHT] = Right;
    pPtr->m_FrameComponent[DTWAIN_FRAMEBOTTOM] = Bottom;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameGetAll(DTWAIN_FRAME Frame, LPDTWAIN_FLOAT Left,
                                            LPDTWAIN_FLOAT Top, LPDTWAIN_FLOAT Right,
                                            LPDTWAIN_FLOAT Bottom)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, Left, Top, Right, Bottom))

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    DTWAINFrameInternal* pPtr = (DTWAINFrameInternal*)Frame;
    pPtr = GetDTWAINFramePtr(pPtr);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !pPtr; },
        DTWAIN_ERR_INVALID_DTWAIN_FRAME, false, FUNC_MACRO);

    *Left =   pPtr->m_FrameComponent[DTWAIN_FRAMELEFT];
    *Top =    pPtr->m_FrameComponent[DTWAIN_FRAMETOP];
    *Right =  pPtr->m_FrameComponent[DTWAIN_FRAMERIGHT];
    *Bottom = pPtr->m_FrameComponent[DTWAIN_FRAMEBOTTOM];
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameGetValue(DTWAIN_FRAME Frame, LONG nWhich, LPDTWAIN_FLOAT Value)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, nWhich, Value))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    bool bCheck = (nWhich == DTWAIN_FRAMETOP || nWhich == DTWAIN_FRAMELEFT || nWhich == DTWAIN_FRAMERIGHT || nWhich == DTWAIN_FRAMEBOTTOM);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] {return !bCheck;}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    DTWAINFrameInternal* pPtr = (DTWAINFrameInternal*)Frame;
    pPtr = GetDTWAINFramePtr(pPtr);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !pPtr; },
        DTWAIN_ERR_INVALID_DTWAIN_FRAME, false, FUNC_MACRO);
    *Value = pPtr->m_FrameComponent[nWhich];
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameSetValue(DTWAIN_FRAME Frame, LONG nWhich, DTWAIN_FLOAT Value)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, nWhich, Value))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    bool bCheck = (nWhich == DTWAIN_FRAMETOP || nWhich == DTWAIN_FRAMELEFT || nWhich == DTWAIN_FRAMERIGHT || nWhich == DTWAIN_FRAMEBOTTOM);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return !bCheck;}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    DTWAINFrameInternal* pPtr = (DTWAINFrameInternal*)Frame;
    pPtr = GetDTWAINFramePtr(pPtr);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return !pPtr;},
                                      DTWAIN_ERR_INVALID_DTWAIN_FRAME, false, FUNC_MACRO);
    pPtr->m_FrameComponent[nWhich] = Value;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}


DTWAIN_FRAME DLLENTRY_DEF DTWAIN_FrameCreateString(LPCTSTR Left, LPCTSTR Top, LPCTSTR Right, LPCTSTR Bottom)
{
    LOG_FUNC_ENTRY_PARAMS((Left, Top, Right, Bottom))
    double leftD    = StringWrapper::ToDouble(Left);
    double topD     = StringWrapper::ToDouble(Top);
    double rightD   = StringWrapper::ToDouble(Right);
    double bottomD  = StringWrapper::ToDouble(Bottom);
    DTWAIN_FRAME newFrame = DTWAIN_FrameCreate(leftD, topD, rightD, bottomD);
    LOG_FUNC_EXIT_PARAMS(newFrame)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameSetAllString(DTWAIN_FRAME Frame, LPCTSTR Left, LPCTSTR Top, LPCTSTR Right, LPCTSTR Bottom)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, Left, Top, Right, Bottom))
    double leftD    = StringWrapper::ToDouble(Left);
    double topD     = StringWrapper::ToDouble(Top);
    double rightD   = StringWrapper::ToDouble(Right);
    double bottomD  = StringWrapper::ToDouble(Bottom);
    DTWAIN_BOOL bRet = DTWAIN_FrameSetAll(Frame, leftD, topD, rightD, bottomD);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameGetAllString(DTWAIN_FRAME Frame, LPTSTR Left, LPTSTR Top, LPTSTR Right, LPTSTR Bottom)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, Left, Top, Right, Bottom))
    double d[4];
    DTWAIN_BOOL bRet = DTWAIN_FrameGetAll(Frame, &d[0], &d[1], &d[2], &d[3]);
    if ( !bRet )
        LOG_FUNC_EXIT_PARAMS(bRet)
    LPTSTR* vals[] = {&Left, &Top, &Right, &Bottom};
    CTL_StringStreamType strm;
    for (int i = 0; i < 4; ++i )
    {
        strm << BOOST_FORMAT(_T("%1%")) % d[i];
        StringWrapper::SafeStrcpy(*vals[i], strm.str().c_str());
        strm.str(_T(""));
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameGetValueString(DTWAIN_FRAME Frame, LONG nWhich, LPTSTR Value)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, nWhich, Value))
    double d;
    DTWAIN_BOOL bRet = DTWAIN_FrameGetValue(Frame, nWhich, &d);
    if ( !bRet )
        LOG_FUNC_EXIT_PARAMS(bRet)
    CTL_StringStreamType strm;
    strm << BOOST_FORMAT(_T("%1%")) % d;
    StringWrapper::SafeStrcpy(Value, strm.str().c_str());
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FrameSetValueString(DTWAIN_FRAME Frame, LONG nWhich, LPCTSTR Value)
{
    LOG_FUNC_ENTRY_PARAMS((Frame, nWhich, Value))
    double d = StringWrapper::ToDouble(Value);
    DTWAIN_BOOL bRet = DTWAIN_FrameSetValue(Frame, nWhich, d);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayDestroyFrames(DTWAIN_ARRAY FrameArray)
{
    LOG_FUNC_ENTRY_PARAMS((FrameArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
    [&] { return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( FrameArray, CTL_EnumeratorDTWAINFrameType ) );},
    DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    // Remove DTWAIN_FRAMES from the container of known frames
    auto vValues = EnumeratorVectorPtr<DTWAIN_FRAME>(FrameArray);

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return ( vValues == NULL );}, DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    for_each(vValues->begin(), vValues->end(), DTWAIN_FrameDestroy);

    // Now destroy the enumerator
    EnumeratorFunctionImpl::EnumeratorDestroy(FrameArray);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}
////////////////////////////// TW_FIX32 functions //////////////////////////////////////////////
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFix32SetAt(DTWAIN_ARRAY aFix32, DTWAIN_LONG lPos, DTWAIN_LONG Whole, DTWAIN_LONG Frac)
{
    LOG_FUNC_ENTRY_PARAMS((aFix32, lPos, Whole, Frac))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
    [&]{return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( aFix32, CTL_EnumeratorTWFIX32Type ));},
                                        DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    // check for out of bounds size
    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(aFix32);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || lPos >= Count);}, DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);

    TW_FIX32 temp = {(TW_INT16)Whole, (TW_UINT16)Frac};
    EnumeratorFunctionImpl::EnumeratorSetAt(aFix32, lPos, &temp);

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFix32GetAt(DTWAIN_ARRAY aFix32, DTWAIN_LONG lPos, LPLONG Whole, LPLONG Frac)
{
    LOG_FUNC_ENTRY_PARAMS((aFix32, lPos, Whole, Frac))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
        [&] {return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( aFix32, CTL_EnumeratorTWFIX32Type ));},
        DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    // check for out of bounds size
    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(aFix32);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (Count <= 0 || lPos >= Count);}, DTWAIN_ERR_INDEX_BOUNDS, false, FUNC_MACRO);

    TW_FIX32 tempF;
    EnumeratorFunctionImpl::EnumeratorGetAt(aFix32, lPos, &tempF);
    if ( Whole )
        *Whole = tempF.Whole;
    if ( Frac )
        *Frac = tempF.Frac;

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

static TW_FIX32 FloatToFix32(double d)
{
    TW_FIX32 f32;
    CTL_CapabilityTriplet::FloatToTwain32( (float)d, f32 );
    return f32;
}

static double Fix32ToFloat(const TW_FIX32& fix32)
{
    return CTL_CapabilityTriplet::Twain32ToFloat( fix32 );
}

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayConvertFloatToFix32(DTWAIN_ARRAY FloatArray)
{
    LOG_FUNC_ENTRY_PARAMS((FloatArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
    [&]{return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( FloatArray, CTL_EnumeratorDoubleType ));},
                                      DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    int nStatus;
    // get count
    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(FloatArray);

    // create a new enumerator
    LPVOID aFix32 = EnumeratorFunctionImpl::GetNewEnumerator(CTL_EnumeratorTWFIX32Type, &nStatus, CTL_EnumeratorTWFIX32Type, Count);

    // get the underlying vectors
    auto& vIn = EnumeratorVector<double>(FloatArray);
    auto& vOut = EnumeratorVector<TW_FIX32>(aFix32);

    // call transform to create array of TW_FIX32 values
    std::transform(vIn.begin(), vIn.end(), vOut.begin(), FloatToFix32);

    // remove the old array
    EnumeratorFunctionImpl::EnumeratorDestroy(FloatArray);

    LOG_FUNC_EXIT_PARAMS(aFix32)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}


DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayConvertFix32ToFloat(DTWAIN_ARRAY Fix32Array)
{
    LOG_FUNC_ENTRY_PARAMS((Fix32Array))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle,
        [&]{return ( !EnumeratorFunctionImpl::EnumeratorIsValidEx( Fix32Array, CTL_EnumeratorTWFIX32Type ));},
        DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    int nStatus;
    LONG Count = EnumeratorFunctionImpl::EnumeratorGetCount(Fix32Array);
    LPVOID aFloat = EnumeratorFunctionImpl::GetNewEnumerator(CTL_EnumeratorDoubleType, &nStatus, CTL_EnumeratorDoubleType, Count);
    auto& vIn = EnumeratorVector<TW_FIX32>(Fix32Array);
    auto& vOut = EnumeratorVector<double>(aFloat);
    std::transform(vIn.begin(), vIn.end(), vOut.begin(), Fix32ToFloat);

    // remove the old array
    EnumeratorFunctionImpl::EnumeratorDestroy(Fix32Array);

    LOG_FUNC_EXIT_PARAMS(aFloat)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

bool dynarithmic::DTWAINFRAMEToTWFRAME(DTWAIN_FRAME pDdtwil, pTW_FRAME pTwain)
{
    double Val[4];
    pTW_FIX32 pVal[4];
    pVal[0] = &pTwain->Left;
    pVal[1] = &pTwain->Top;
    pVal[2] = &pTwain->Right;
    pVal[3] = &pTwain->Bottom;

    if ( !DTWAIN_FrameGetAll(pDdtwil, (LPDTWAIN_FLOAT)&Val[0], (LPDTWAIN_FLOAT)&Val[1], (LPDTWAIN_FLOAT)&Val[2], (LPDTWAIN_FLOAT)&Val[3]))
        return false;
    for ( int i = 0; i < 4; i++ )
        CTL_CapabilityTriplet::FloatToTwain32( (float)Val[i], *pVal[i] );
    return true;
}

bool dynarithmic::TWFRAMEToDTWAINFRAME(const TW_FRAME& pTwain, DTWAIN_FRAME pDdtwil)
{
    double ValOut[4];
    TW_FIX32 ValIn[4];
    ValIn[0] = pTwain.Left;
    ValIn[1] = pTwain.Top;
    ValIn[2] = pTwain.Right;
    ValIn[3] = pTwain.Bottom;

    std::transform(ValIn, ValIn + 4, ValOut, CTL_CapabilityTriplet::Twain32ToFloat);
    if ( !DTWAIN_FrameSetAll(pDdtwil, ValOut[0], ValOut[1], ValOut[2], ValOut[3]) )
        return false;
    return true;
}

static LONG IsValidAcqArray( DTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((pArray))
    // Instance of class that takes an array that takes a handle (pointer)
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, DTWAIN_ERR_BAD_HANDLE, NULL);

    // Check if array is a valid type for ranges
    if ( !EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, CTL_EnumeratorPtrType) )
       LOG_FUNC_EXIT_PARAMS(DTWAIN_ERR_WRONG_ARRAY_TYPE)
    LOG_FUNC_EXIT_PARAMS(1)
    CATCH_BLOCK(0)
}

LONG DLLENTRY_DEF DTWAIN_GetNumAcquisitions(DTWAIN_ARRAY aAcq)
{
    LOG_FUNC_ENTRY_PARAMS((aAcq))
    LONG lError;
    if (( lError = IsValidAcqArray( aAcq )) < 0 )
        LOG_FUNC_EXIT_PARAMS(lError)
    LONG Ret = EnumeratorFunctionImpl::EnumeratorGetCount( aAcq );
    LOG_FUNC_EXIT_PARAMS(Ret)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG DLLENTRY_DEF DTWAIN_GetNumAcquiredImages( DTWAIN_ARRAY aAcq, LONG nWhich )
{
    LOG_FUNC_ENTRY_PARAMS((aAcq, nWhich))
    LONG lError;
    if (( lError = IsValidAcqArray( aAcq )) < 0 )
        LOG_FUNC_EXIT_PARAMS(lError)

    LONG lCount = EnumeratorFunctionImpl::EnumeratorGetCount( aAcq );
    if ( nWhich >= lCount )
        LOG_FUNC_EXIT_PARAMS(DTWAIN_FAILURE1)

    DTWAIN_ARRAY aDib = 0;
    EnumeratorFunctionImpl::EnumeratorGetAt( aAcq, nWhich, &aDib);
    lCount = EnumeratorFunctionImpl::EnumeratorGetCount( aDib );
    LOG_FUNC_EXIT_PARAMS(lCount)
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

HANDLE DLLENTRY_DEF DTWAIN_GetAcquiredImage( DTWAIN_ARRAY aAcq, LONG nWhichAcq, LONG nWhichDib )
{
    LOG_FUNC_ENTRY_PARAMS((aAcq, nWhichAcq, nWhichDib))
    if ( nWhichDib < 0 )
            LOG_FUNC_EXIT_PARAMS(NULL)
    int nDibs = DTWAIN_GetNumAcquiredImages( aAcq, nWhichAcq );
    if ( nDibs == DTWAIN_FAILURE1 || nWhichDib >= nDibs )
        LOG_FUNC_EXIT_PARAMS(NULL)
    DTWAIN_ARRAY aDib = 0;
    EnumeratorFunctionImpl::EnumeratorGetAt( aAcq, nWhichAcq, &aDib );
    HANDLE hDib = NULL;
    EnumeratorFunctionImpl::EnumeratorGetAt( aDib, nWhichDib, &hDib );
    LOG_FUNC_EXIT_PARAMS(hDib)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_GetAcquiredImageArray(DTWAIN_ARRAY aAcq, LONG nWhichAcq)
{
    LOG_FUNC_ENTRY_PARAMS((aAcq, nWhichAcq))
    DTWAIN_ARRAY aCopy = 0;
    int nDibs = DTWAIN_GetNumAcquiredImages( aAcq, nWhichAcq );
    if ( nDibs == DTWAIN_FAILURE1 )
        LOG_FUNC_EXIT_PARAMS(NULL)

    //use a copy
    DTWAIN_ARRAY aDib = 0;
    DTWAIN_ArrayGetAt( aAcq, nWhichAcq, &aDib );
    aCopy = DTWAIN_ArrayCreateCopy(aDib);
    LOG_FUNC_EXIT_PARAMS(aCopy)
    CATCH_BLOCK(DTWAIN_ARRAY(NULL))
}

template <typename T>
static LONG ArrayStringLength_Internal(DTWAIN_ARRAY theArray, LONG nWhichString)
{
    auto vValues = EnumeratorVectorPtr<T>(theArray);
    if ( !vValues )
        return DTWAIN_ERR_INDEX_BOUNDS;
    size_t nCount = vValues->size();
    if ( nWhichString < 0 || static_cast<size_t>(nWhichString) > nCount )
        return DTWAIN_ERR_INDEX_BOUNDS;
    return static_cast<LONG>((*vValues)[nWhichString].size());
}

template <typename T>
struct stringLengthComparer
{
  bool operator()(const T& s1, const T& s2) const
  { return s1.size() < s2.size(); }
};

template <typename T>
static LONG ArrayMaxStringLength_Internal(DTWAIN_ARRAY theArray)
{
    auto vValues = EnumeratorVectorPtr<T>(theArray);
    if ( !vValues )
       return 0;

    auto it = std::max_element(vValues->begin(), vValues->end(), stringLengthComparer<T>());
    return static_cast<LONG>(it->size());
}

LONG DLLENTRY_DEF DTWAIN_ArrayGetStringLength(DTWAIN_ARRAY theArray, LONG nWhichString)
{
    LOG_FUNC_ENTRY_PARAMS((theArray, nWhichString))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    // check if array is a string array

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    bool bGoodType1 = EnumeratorFunctionImpl::EnumeratorIsValidEx(theArray, CTL_EnumeratorStringType);
    bool bGoodType2 = EnumeratorFunctionImpl::EnumeratorIsValidEx(theArray, CTL_EnumeratorWideStringType);
    bool bGoodType3 = EnumeratorFunctionImpl::EnumeratorIsValidEx(theArray, CTL_EnumeratorANSIStringType);
    bool bGoodType = bGoodType1 || bGoodType2 || bGoodType3;

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return !bGoodType;},
                               DTWAIN_ERR_WRONG_ARRAY_TYPE, DTWAIN_ERR_WRONG_ARRAY_TYPE, FUNC_MACRO);

    LONG retValue;
    if ( bGoodType1 )
        retValue = ArrayStringLength_Internal<CTL_StringType>(theArray, nWhichString);
    else
    if ( bGoodType2 )
        retValue = ArrayStringLength_Internal<CTL_WString>(theArray, nWhichString);
    else
        retValue = ArrayStringLength_Internal<CTL_String>(theArray, nWhichString);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return retValue == DTWAIN_ERR_INDEX_BOUNDS; },
        DTWAIN_ERR_INDEX_BOUNDS, DTWAIN_ERR_INDEX_BOUNDS, FUNC_MACRO);
    LOG_FUNC_EXIT_PARAMS(retValue)
    CATCH_BLOCK(0)
}

LONG DLLENTRY_DEF DTWAIN_ArrayGetMaxStringLength(DTWAIN_ARRAY theArray)
{
    LOG_FUNC_ENTRY_PARAMS((theArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    // check if array is a string array

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    bool bGoodType1 = EnumeratorFunctionImpl::EnumeratorIsValidEx(theArray, CTL_EnumeratorStringType);
    bool bGoodType2 = EnumeratorFunctionImpl::EnumeratorIsValidEx(theArray, CTL_EnumeratorWideStringType);
    bool bGoodType3 = EnumeratorFunctionImpl::EnumeratorIsValidEx(theArray, CTL_EnumeratorANSIStringType);
    bool bGoodType = bGoodType1 || bGoodType2 || bGoodType3;

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] {return !bGoodType;} ,
                                     DTWAIN_ERR_WRONG_ARRAY_TYPE, DTWAIN_ERR_WRONG_ARRAY_TYPE, FUNC_MACRO);

    LONG retValue;
    if ( bGoodType1 )
        retValue = ArrayMaxStringLength_Internal<CTL_StringType>(theArray);
    else
    if ( bGoodType2 )
        retValue = ArrayMaxStringLength_Internal<CTL_WString>(theArray);
    else
        retValue = ArrayMaxStringLength_Internal<CTL_String>(theArray);

    // Check if array exists
    LOG_FUNC_EXIT_PARAMS(retValue)
    CATCH_BLOCK(0)
}

void CTL_TwainDLLHandle::RemoveAllEnumerators()
{
    CTL_TwainDLLHandle::s_EnumeratorFactory.reset();
}
