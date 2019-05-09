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
#include <cstdlib>
#include <cstdio>
#include <cstring>
#include <sstream>
#include <boost/scope_exit.hpp>
#include <functional>
#include "ctldib.h"
#include "ctltwmgr.h"
#include "ctltrall.h"
#include "ctltmpl5.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"

using namespace std;
using namespace dynarithmic;
bool dynarithmic::IsIntCapType(TW_UINT16 nCap)
{
    return  nCap == TWTY_INT16 ||
        nCap == TWTY_INT32 ||
        nCap == TWTY_BOOL ||
        nCap == TWTY_INT8 ||
        nCap == TWTY_UINT8 ||
        nCap == TWTY_UINT16 ||
        nCap == TWTY_UINT32;
}

bool dynarithmic::IsFloatCapType(TW_UINT16 nCap)
{ return nCap == TWTY_FIX32; }

bool dynarithmic::IsStringCapType(TW_UINT16 nCap)
{
    return nCap == TWTY_STR32 ||
        nCap == TWTY_STR64 ||
        nCap == TWTY_STR128 ||
        nCap == TWTY_STR255 ||
        nCap == TWTY_STR1024;
}

bool dynarithmic::IsFrameCapType(TW_UINT16 nCap)
{ return nCap == TWTY_FRAME; }

static DTWAIN_BOOL DTWAIN_GetCapValuesEx_Internal( DTWAIN_SOURCE Source, TW_UINT16 lCap,
                                                   LONG lGetType, LONG lContainerType,
                                                   LONG nDataType, LPDTWAIN_ARRAY pArray,
                                                   bool bOverrideDataType );

// Overload for string caps
static int GetMultiCapValues(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source,
                        DTWAIN_ARRAY pArray,
                        CTL_EnumeratorType,
                        TW_UINT16 nCap,
                        CTL_EnumGetType GetType,
                        CTL_StringType AValue,
                        TW_UINT16 TwainDataType,
                        UINT nContainerVal/* = 0*/,
                        bool bUseContainer/* = false */,
                        CTL_String dummy = CTL_String()
                        );

// overload for TW_FRAME caps
static int GetMultiCapValues(DTWAIN_HANDLE DLLHandle,
                        DTWAIN_SOURCE Source,
                        DTWAIN_ARRAY pArray,
                        CTL_EnumeratorType EnumType,
                        TW_UINT16 nCap,
                        CTL_EnumGetType GetType,
                        DTWAIN_FRAME frm,
                        TW_UINT16 TwainDataType,
                        UINT nContainerVal,
                        bool bUseContainer,
                        TW_FRAME dummy);

template <typename T, typename ConvertTo=T>
struct NullGetCapConverter
{
    static ConvertTo Convert(T value) { return value; }
};

template <typename T, typename ConvertTo = T>
struct NullSetCapConverter
{
    static ConvertTo convert(T value, DTWAIN_ARRAY) { return value; }
};

struct StringGetCapConverter
{
    static CTL_StringType Convert(CTL_String& value)
    { return StringConversion::Convert_Ansi_To_Native(value); }
};

struct StringSetCapConverter
{
    static CTL_String convert(CTL_StringType& value, DTWAIN_ARRAY)
    {
        return StringConversion::Convert_Native_To_Ansi(value);
    }
};

struct FrameGetCapConverter
{
    static DTWAIN_FRAME Convert(const TW_FRAME& fValue)
    {
        DTWAIN_FRAME TFrame = DTWAIN_FrameCreate(0, 0, 0, 0);
        if (TFrame)
        {
            TWFRAMEToDTWAINFRAME(fValue, TFrame);
            return TFrame;
        }
        return NULL;
    }
};

struct FrameSetCapConverter
{
    static TW_FRAME convert(DTWAINFrameInternal& /*fValue*/, DTWAIN_ARRAY pArray)
    {
        TW_FRAME Frame;
        DTWAIN_FRAME TranslatedFrame;
        TranslatedFrame = DTWAIN_ArrayFrameGetFrameAt(pArray, 0);
        DTWAINFRAMEToTWFRAME(TranslatedFrame, &Frame);
        return Frame;
    }
};

template <typename DataType,
          typename ConvertTo = DataType,
          typename ConverterFn=NullGetCapConverter<DataType, ConvertTo> >
static DTWAIN_ARRAY performGetCap(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source,
                           TW_UINT16 lCap, LONG /*nDataType*/, LONG lContainerType,
                           LONG lGetType, LONG overrideDataType, CTL_EnumeratorType eType,
                           LONG oneCapFlag=0)
{
    DataType dValue;
    DTWAIN_ARRAY ThisArray = DTWAIN_ArrayCreateFromCap(Source, lCap, 0);

    if (!ThisArray)
        return NULL;

    DTWAINArray_RAII raii(ThisArray);

    DTWAIN_ARRAY pDTWAINArray = ThisArray;
    EnumeratorFunctionImpl::ClearEnumerator(pDTWAINArray);
    int bOk = 0;
    if (lContainerType == DTWAIN_CONTONEVALUE)
    {
        bOk = GetOneCapValue<DataType>(DLLHandle,
                                        Source,
                                        (UINT)lCap,
                                        (CTL_EnumGetType)lGetType,
                                        oneCapFlag,
                                        &dValue,
                                        static_cast<TW_UINT16>(overrideDataType));
        if (!bOk)
            return NULL;
        ConvertTo convValue = ConverterFn::Convert(dValue);
        EnumeratorFunctionImpl::EnumeratorAddValue(pDTWAINArray, &convValue);
    }
    else
    {
        bOk = GetMultiCapValues(DLLHandle,
                                Source,
                                pDTWAINArray,
                                eType,
                                (UINT)lCap,
                                (CTL_EnumGetType)lGetType,
                                ConvertTo(),
                                static_cast<TW_UINT16>(overrideDataType),
                                (UINT)lContainerType,
                                true,
                                DataType());
        if (!bOk)
            return NULL;
    }
    raii.Disconnect();
    return ThisArray;
}


template <typename DataType,
          typename TwainType,
          typename ConvertFrom = DataType,
          typename ConverterFn = NullSetCapConverter<DataType, ConvertFrom> >
static bool performSetCap(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source, TW_UINT16 lCap, DTWAIN_ARRAY pArray,
                          LONG lContainerType, LONG lSetType, LONG nDTWAIN_ArrayType, CTL_EnumeratorType eType,
                          LONG TwainTypeValue)
{
    bool bOk = false;
    DTWAIN_ARRAY pDTWAINArray = pArray;
    bool isArrayOk = (EnumeratorFunctionImpl::GetEnumeratorType(pArray) == nDTWAIN_ArrayType);
    if (lSetType != DTWAIN_CAPRESET)
    {
       if ( !isArrayOk && CTL_CapabilityTriplet::IsCapOperationSet(lSetType) )
            return false;
    }

    bool isResetOp = CTL_CapabilityTriplet::IsCapOperationReset(lSetType);
    if (lContainerType == DTWAIN_CONTONEVALUE || isResetOp)
    {
        DataType lValue;

        if (isResetOp)
            lValue = DataType();
        else
        {
            ConvertFrom lVal;
            EnumeratorFunctionImpl::EnumeratorGetAt(pDTWAINArray, 0, &lVal);
            lValue = ConverterFn::convert(lVal, pDTWAINArray);
        }

        bOk = SetOneCapValue(DLLHandle, Source, static_cast<TW_UINT16>(lCap), (CTL_EnumSetType)lSetType, lValue, static_cast<TW_UINT16>(TwainTypeValue))?true:false;
        return bOk;
    }
    else
    {
        bOk = SetMultiCapValues<DataType, ConvertFrom, ConverterFn>
            (DLLHandle, Source, pArray, eType, (UINT)lCap, (CTL_EnumSetType)lSetType, (UINT)lContainerType, true, static_cast<TW_UINT16>(TwainTypeValue))?true:false;

    }
    return bOk;
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCapValues( DTWAIN_SOURCE Source, LONG lCap, LONG lGetType, LPDTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCap, lGetType, pArray))
    DTWAIN_BOOL bRet = DTWAIN_GetCapValuesEx(Source, lCap, lGetType, DTWAIN_CONTDEFAULT, pArray);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


// Gets capability values.  This function does not test if the capability exists, or if the container type is valid.  Use
// with caution!!
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCapValuesEx( DTWAIN_SOURCE Source, LONG lCap, LONG lGetType, LONG lContainerType,
    LPDTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCap, lGetType, lContainerType, pArray))
    DTWAIN_BOOL bRet = FALSE;
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle((DTWAIN_HANDLE)pHandle, Source);

    if( p )
    {
        LONG nDataType = DTWAIN_GetCapDataType(Source, (CTL_EnumCapability)lCap);
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&]{ return (nDataType < 0);}, nDataType, false, FUNC_MACRO);
        bRet = DTWAIN_GetCapValuesEx_Internal(Source, (TW_UINT16)lCap, lGetType, DTWAIN_CONTDEFAULT, nDataType, pArray, false);
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

// Gets capability values.  This function does not test if the capability exists, or if the container type is valid.  Use
// with caution!!
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCapValuesEx2( DTWAIN_SOURCE Source, LONG lCap, LONG lGetType, LONG lContainerType,
                                                 LONG nDataType, LPDTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCap, lGetType, lContainerType, nDataType,pArray))

    DTWAIN_BOOL bRet = DTWAIN_GetCapValuesEx_Internal(Source, (TW_UINT16)lCap, lGetType, lContainerType, nDataType, pArray, true);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DTWAIN_GetCapValuesEx_Internal( DTWAIN_SOURCE Source, TW_UINT16 lCap, LONG lGetType, LONG lContainerType,
                                                         LONG nDataType, LPDTWAIN_ARRAY pArray, bool bOverrideDataType )
{
    bool bEnumeratorExists = false;
    LOG_FUNC_ENTRY_PARAMS((Source, lCap, lGetType, lContainerType, nDataType, pArray,  bOverrideDataType?TRUE:FALSE))

    DTWAIN_HANDLE DLLHandle = GetDTWAINHandle_Internal();
    CTL_ITwainSource *p = VerifySourceHandle(DLLHandle, Source);

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(DLLHandle);

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !p;}, DTWAIN_ERR_BAD_SOURCE, false, FUNC_MACRO);

    bEnumeratorExists = EnumeratorFunctionImpl::EnumeratorClearExisting(pArray);

    LONG overrideDataType = nDataType;
    if ( bOverrideDataType )
        overrideDataType = 0xFFFF;

    if( !DTWAIN_IsCapSupported(Source, lCap) )
        LOG_FUNC_EXIT_PARAMS(false)

    if( !p->IsCapNegotiableInState((TW_UINT16)lCap, p->GetState()) )
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_ARRAY ThisArray=0;
    DTWAINArrayPtr_RAII arr(&ThisArray);
    if( p )
    {
        if( nDataType == 0xFFFF )
            LOG_FUNC_EXIT_PARAMS(false)

        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (nDataType == DTWAIN_CAPDATATYPE_UNKNOWN);},
                                          DTWAIN_ERR_UNKNOWN_CAPDATATYPE, false, FUNC_MACRO);

        if( lContainerType == DTWAIN_CONTDEFAULT )
            lContainerType = DTWAIN_GetCapContainer(Source, lCap, lGetType);

        if (IsIntCapType(static_cast<TW_UINT16>(nDataType)))
        {
            ThisArray = performGetCap<LONG>(pHandle, Source, lCap, nDataType, lContainerType, lGetType, overrideDataType, CTL_EnumeratorIntType);
            if ( !ThisArray )
                LOG_FUNC_EXIT_PARAMS(false)
        }
        else
        if (IsFloatCapType(static_cast<TW_UINT16>(nDataType)))
        {
            ThisArray = performGetCap<double>(pHandle, Source, lCap, nDataType, lContainerType, lGetType, overrideDataType, CTL_EnumeratorDoubleType);
            if (!ThisArray)
                LOG_FUNC_EXIT_PARAMS(false)
        }
        else
        if ( IsStringCapType(static_cast<TW_UINT16>(nDataType)))
        {
            ThisArray = performGetCap<CTL_String, CTL_StringType, StringGetCapConverter>
                        (pHandle, Source, lCap, nDataType, lContainerType, lGetType, overrideDataType, CTL_EnumeratorStringType);
            if (!ThisArray)
                LOG_FUNC_EXIT_PARAMS(false)
        }
        else
        if ( IsFrameCapType(static_cast<TW_UINT16>(nDataType)))
        {
            ThisArray = performGetCap<TW_FRAME, DTWAIN_FRAME, FrameGetCapConverter>
                (pHandle, Source, lCap, nDataType, lContainerType, lGetType, overrideDataType, CTL_EnumeratorTWFrameType);
            if (!ThisArray)
                LOG_FUNC_EXIT_PARAMS(false)
        }
        else
            LOG_FUNC_EXIT_PARAMS(false)
    }
    arr.Disconnect();
    if ( bEnumeratorExists )
        EnumeratorFunctionImpl::EnumeratorDestroy(*pArray);
    *pArray = ThisArray;
    DumpArrayContents(*pArray, lCap);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

// Sets capability values.  This function does not test if the
// capability exists, or if the container type is valid.  Use
// with caution!!
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCapValues( DTWAIN_SOURCE Source, LONG lCap, LONG lSetType, DTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCap, lSetType, pArray))
    DTWAIN_BOOL bRet = DTWAIN_SetCapValuesEx(Source, lCap, lSetType, DTWAIN_CONTDEFAULT, pArray);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCapValuesEx2( DTWAIN_SOURCE Source, LONG lCap, LONG lSetType, LONG lContainerType,
                                                LONG nDataType, DTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCap, lSetType, lContainerType, nDataType, pArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    bool bOk = false;
    if( p )
    {
        // Check if data type matches array type
        bool bFoundType = false;
        if ( !CTL_CapabilityTriplet::IsCapOperationReset(lSetType) )
        {
            LONG DTwainArrayType = DTWAIN_ArrayGetType( pArray );
            CTL_MAPLONGTOVECTORLONG::iterator it1 =
                pHandle->m_mapDTWAINArrayToTwainType.find(DTwainArrayType);
            if ( it1 != pHandle->m_mapDTWAINArrayToTwainType.end() )
            {
                // Search the array for the Twain Type
                std::vector<LONG>::iterator it2 =
                    std::find(it1->second.begin(), it1->second.end(), nDataType);
                if ( it2 != it1->second.end())
                    bFoundType = true;
            }
            DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !bFoundType;}, DTWAIN_ERR_BAD_ARRAY, false, FUNC_MACRO);
        }
        if( nDataType == 0xFFFF )
            LOG_FUNC_EXIT_PARAMS(false)

        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (nDataType == DTWAIN_CAPDATATYPE_UNKNOWN);}, DTWAIN_ERR_UNKNOWN_CAPDATATYPE, false, FUNC_MACRO);

        LONG TestContainer;

        if( lContainerType == DTWAIN_CONTDEFAULT )
            TestContainer = DTWAIN_GetCapContainer(Source, lCap, lSetType);
        else
            TestContainer = lContainerType;

        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (TestContainer == 0 && !CTL_CapabilityTriplet::IsCapOperationReset(lSetType));}, DTWAIN_ERR_CAPSET_NOSUPPORT, false, FUNC_MACRO);

        if( lContainerType == DTWAIN_CONTDEFAULT )
            lContainerType = TestContainer;

        if( lSetType == DTWAIN_CAPSETAVAILABLE || lSetType == DTWAIN_CAPSETCURRENT )
            lSetType = DTWAIN_CAPSET;

        DumpArrayContents(pArray, lCap);

        if (IsIntCapType(static_cast<TW_UINT16>(nDataType)))
            bOk = performSetCap<LONG, TW_UINT32>(pHandle, Source, static_cast<TW_UINT16>(lCap), pArray, lContainerType, lSetType, DTWAIN_ARRAYLONG, CTL_EnumeratorIntType, nDataType);
        else
        if (IsFloatCapType(static_cast<TW_UINT16>(nDataType)))
            bOk = performSetCap<double, double>(pHandle, Source, static_cast<TW_UINT16>(lCap), pArray, lContainerType, lSetType, DTWAIN_ARRAYFLOAT, CTL_EnumeratorDoubleType, nDataType);
        else
        if ( IsStringCapType(static_cast<TW_UINT16>(nDataType)))
            bOk = performSetCap<CTL_String, CTL_StringType, CTL_StringType, StringSetCapConverter>
                          (pHandle, Source, static_cast<TW_UINT16>(lCap), pArray, lContainerType, lSetType, DTWAIN_ARRAYSTRING, CTL_EnumeratorStringType, nDataType);
        else
        if ( IsFrameCapType(static_cast<TW_UINT16>(nDataType)))
        {
            bOk = performSetCap<TW_FRAME, TW_FRAME, DTWAINFrameInternal, FrameSetCapConverter>
            (pHandle, Source, static_cast<TW_UINT16>(lCap), pArray, lContainerType, lSetType, DTWAIN_ARRAYFRAME, CTL_EnumeratorTWFrameType, nDataType);
        }
    }
    LOG_FUNC_EXIT_PARAMS(bOk)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCapValuesEx( DTWAIN_SOURCE Source, LONG lCap, LONG lSetType, LONG lContainerType,
                                                DTWAIN_ARRAY pArray )
{
    LOG_FUNC_ENTRY_PARAMS((Source, lCap, lSetType, lContainerType, pArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    DTWAIN_BOOL bRet = FALSE;
    if( p )
    {
        LONG nDataType = DTWAIN_GetCapDataType(Source, (CTL_EnumCapability)lCap);
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return (nDataType < 0);} , DTWAIN_ERR_BAD_CAP, false, FUNC_MACRO);
        bRet = DTWAIN_SetCapValuesEx2(Source, lCap, lSetType, lContainerType, nDataType, pArray);
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

/////////////////////////////////////////////////////////////
// Custom cap data functions
#include "ctltr036.h"

HANDLE DLLENTRY_DEF DTWAIN_GetCustomDSData( DTWAIN_SOURCE Source, LPBYTE Data, LONG dSize, LPLONG pActualSize, LONG nFlags )
{
    LOG_FUNC_ENTRY_PARAMS((Source, Data, dSize, pActualSize, nFlags))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    bool bSupported = DTWAIN_IsCapSupported(Source, CAP_CUSTOMDSDATA)?true:false;

    if( !bSupported )
        LOG_FUNC_EXIT_PARAMS(NULL);

    // Call TWAIN to get the custom data
    CTL_ITwainSession *pSession = p->GetTwainSession();
    CTL_CustomDSTriplet DST(pSession, p, MSG_GET);

    // Get the custom data
    int ret = DST.Execute();

    // return if TWAIN cannot complete this request
    if( ret != TWRC_SUCCESS )
        LOG_FUNC_EXIT_PARAMS(NULL);

    // Copy actual size data to parameter
    if( pActualSize )
        * pActualSize = DST.GetDataSize();
    int localActualSize = DST.GetDataSize();

    // Get the returned handle from TWAIN
    HANDLE h = DST.GetData();

    // Return the handle if that is all user wants to do
    if( nFlags & DTWAINGCD_RETURNHANDLE )
        LOG_FUNC_EXIT_PARAMS(h);

    // Copy data to user's data area.
    if( nFlags & DTWAINGCD_COPYDATA )
    {
        int nMinCopy;
        char *pData = static_cast<char *>(ImageMemoryHandler::GlobalLock(h));

        if( dSize == -1 )
            nMinCopy = localActualSize;
        nMinCopy = max(min(dSize, (LONG)localActualSize), (LONG)0);

        memcpy(Data, pData, nMinCopy);
        ImageMemoryHandler::GlobalUnlock(h);
        ImageMemoryHandler::GlobalFree(h);
        LOG_FUNC_EXIT_PARAMS(HANDLE(1));
    }

    LOG_FUNC_EXIT_PARAMS(h);
    CATCH_BLOCK(HANDLE())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCustomDSData( DTWAIN_SOURCE Source, HANDLE hData, LPCBYTE Data, LONG dSize, LONG nFlags )
{
    LOG_FUNC_ENTRY_PARAMS((Source, hData, Data, dSize, nFlags))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    bool bSupported = DTWAIN_IsCapSupported(Source, CAP_CUSTOMDSDATA)?true:false;

    if( !bSupported )
        LOG_FUNC_EXIT_PARAMS(false);


    // Set up triplet for CUSTOMDSDATA call
    CTL_ITwainSession *pSession = p->GetTwainSession();
    CTL_CustomDSTriplet DST(pSession, p, MSG_SET);

    // Check what options the user wants to do
    char *pData = NULL;

    // Set data to the handle passed in
    if( nFlags & DTWAINSCD_USEHANDLE )
        DST.SetData(hData, dSize);
    else

    // Check if dSize is -1
    if( dSize == -1 )
    {
        if( !DTWAIN_GetCustomDSData(Source, NULL, 0, &dSize, DTWAINGCD_COPYDATA) )
            LOG_FUNC_EXIT_PARAMS(false)
    }

    // Set data to the data passed in
    if( nFlags & DTWAINSCD_USEDATA )
    {
        // Allocate local copy of handle
        pData = (char *)ImageMemoryHandler::GlobalAllocPr(GMEM_DDESHARE, dSize);
        memcpy(pData, Data, dSize);
        DST.SetData(ImageMemoryHandler::GlobalHandle(pData), dSize);
    }

    // Call TWAIN
    int ret = DST.Execute();

    // Release local handle if data used
    if( nFlags & DTWAINSCD_USEDATA )
        ImageMemoryHandler::GlobalFreePr(pData);

    // return TRUE or FALSE depending on return code of TWAIN
    if( ret != TWRC_SUCCESS )
        LOG_FUNC_EXIT_PARAMS(false);
    LOG_FUNC_EXIT_PARAMS(true);
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireStripSizes( DTWAIN_SOURCE Source, LPLONG lpMin, LPLONG lpMax,
    LPLONG lpPreferred )
{
    LOG_FUNC_ENTRY_PARAMS((Source, lpMin, lpMax, lpPreferred))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    // See if Source is opened

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return (!CTL_TwainAppMgr::IsSourceOpen(p) );}, DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);
    TW_SETUPMEMXFER Xfer;
    bool bRet = CTL_TwainAppMgr::GetMemXferValues(p, &Xfer)?true:false;

    if( bRet )
    {
        if( lpMin )
            * lpMin = Xfer.MinBufSize;

        if( lpMax )
            * lpMax = Xfer.MaxBufSize;

        if( lpPreferred )
            * lpPreferred = Xfer.Preferred;
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireStripBuffer(DTWAIN_SOURCE Source, HANDLE hMem)
{
    LOG_FUNC_ENTRY_PARAMS((Source,hMem))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );
    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex (pHandle, [&] { return (!CTL_TwainAppMgr::IsSourceOpen( p ));},
                DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    if ( !hMem )
    {
        p->SetUserStripBuffer(NULL);
        LOG_FUNC_EXIT_PARAMS(true)
    }
    else
    {
        SIZE_T dSize = ImageMemoryHandler::GlobalSize( hMem );
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return !dSize; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
        p->SetUserStripBuffer(hMem);
        p->SetUserStripBufSize(dSize);
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireStripSize(DTWAIN_SOURCE Source, LONG StripSize)
{
    LOG_FUNC_ENTRY_PARAMS((Source, StripSize))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex (pHandle, [&]{return (!CTL_TwainAppMgr::IsSourceOpen( p ));}, DTWAIN_ERR_SOURCE_NOT_OPEN, false,
                                        FUNC_MACRO);

    LONG MinSize, MaxSize;
    if ( StripSize == 0 )
    {
        p->SetUserStripBufSize(StripSize);
        LOG_FUNC_EXIT_PARAMS(true)
    }

    if ( DTWAIN_GetAcquireStripSizes(Source, &MinSize, &MaxSize, NULL) )
    {
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (StripSize < MinSize || StripSize > MaxSize );},
                                                    DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    }
    else
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, []{ return true;}, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    p->SetUserStripBufSize(StripSize);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

HANDLE DLLENTRY_DEF DTWAIN_GetAcquireStripBuffer(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );
    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex (pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen( p ));},DTWAIN_ERR_SOURCE_NOT_OPEN, NULL, FUNC_MACRO);
    HANDLE h = p->GetUserStripBuffer();
    LOG_FUNC_EXIT_PARAMS(h)
    CATCH_BLOCK(HANDLE())
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireStripData(DTWAIN_SOURCE Source, LPLONG lpCompression, LPLONG lpBytesPerRow,
                                                    LPLONG lpColumns, LPLONG lpRows, LPLONG XOffset,
                                                    LPLONG YOffset, LPLONG lpBytesWritten)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lpCompression, lpBytesPerRow,lpColumns, lpRows, XOffset,YOffset, lpBytesWritten))

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex (pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen( p ));},DTWAIN_ERR_SOURCE_NOT_OPEN, false,
                                       FUNC_MACRO);

    TW_IMAGEMEMXFER* pBuffer = p->GetBufferStripData();

    if ( lpCompression )
        *lpCompression = pBuffer->Compression;
    if ( lpBytesPerRow)
        *lpBytesPerRow = pBuffer->BytesPerRow;
    if ( lpColumns )
        *lpColumns = pBuffer->Columns;
    if ( lpRows )
        *lpRows = pBuffer->Rows;
    if ( XOffset )
        *XOffset = pBuffer->XOffset;
    if ( YOffset )
        *YOffset = pBuffer->YOffset;
    if ( lpBytesWritten)
        *lpBytesWritten = pBuffer->BytesWritten;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

/* These functions can only be used in State 7   (when DTWAIN_TN_TRANSFERDONE notification is sent).
   This means that only languages that can utilize DTWAIN_SetCallback or can intercept Window's
   messages can use these functions. */


/* Return all the supported ExtImageInfo types.  This function is useful if your app
   wants to know what types of Extended Image Information is supported by the Source.
   This function does not need DTWAIN_InitExtImageInfo to execute correctly.  */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumExtImageInfoTypes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Array)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Array))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    if ( !DTWAIN_IsExtImageInfoSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    CTL_IntArray r;
    DTWAIN_ARRAY ThisArray=0;
    if ( p->EnumExtImageInfo(r) )
    {
        size_t nCount = r.size();
        ThisArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, (LONG)nCount);
        auto& vValues = EnumeratorVector<int>(ThisArray);
        std::copy(r.begin(), r.end(), vValues.begin());
        *Array = ThisArray;
        return TRUE;
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

/* Initialize the extimageinfo interface.  This must be called first! */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_InitExtImageInfo(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );
    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen( p ));},DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (p->GetState() != SOURCE_STATE_TRANSFERRING);},DTWAIN_ERR_INVALID_STATE, false, FUNC_MACRO);

    p->InitExtImageInfo(0);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

/* Application adds an item to query the image information.  Before getting the Extended
   Image Information, the application will call DTWAIN_AddExtImageInfoQuery multiple times,
   each time for each Image Information desired  */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddExtImageInfoQuery(DTWAIN_SOURCE Source, LONG ExtImageInfo)
{
    LOG_FUNC_ENTRY_PARAMS((Source, ExtImageInfo))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );
    if ( !DTWAIN_IsExtImageInfoSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (p->GetState() != SOURCE_STATE_TRANSFERRING);},DTWAIN_ERR_INVALID_STATE, false, FUNC_MACRO);
    TW_INFO Info;
    Info.InfoID = static_cast<TW_UINT16>(ExtImageInfo);
    p->AddExtImageInfo( Info );
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

/* This function actualy initiates the querying of the ext image information.  This function
   will query the TWAIN Source.  If your TWAIN Source has bugs, this will be where any problem
   will exist */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetExtImageInfo(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    if ( !DTWAIN_IsExtImageInfoSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&] { return (p->GetState() != SOURCE_STATE_TRANSFERRING);},DTWAIN_ERR_INVALID_STATE, false, FUNC_MACRO);

    p->GetExtImageInfo(TRUE);
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

/* This returns the information pertaining to a certain item in the list.  The application
   will call this for each information retrieved from the Source.  This function does not
   return the actual data, only the information as to the number of items, data type, etc.
   that the Source reports for the data item.  Use DTWAIN_GetExtImageInfoData to get the
   data */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetExtImageInfoItem(DTWAIN_SOURCE Source,
                                                    LONG nWhich,
                                                    LPLONG InfoID,
                                                    LPLONG NumItems,
                                                    LPLONG Type)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nWhich, InfoID, NumItems, Type))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    if ( !DTWAIN_IsExtImageInfoSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (p->GetState() != SOURCE_STATE_TRANSFERRING);},
                DTWAIN_ERR_INVALID_STATE, false, FUNC_MACRO);

    TW_INFO Info = p->GetExtImageInfoItem(nWhich, DTWAIN_BYID);
    if ( InfoID )
        *InfoID = Info.InfoID;
    if ( NumItems )
        *NumItems = Info.NumItems;
    if ( Type )
        *Type = Info.ItemType;

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

/* This returns the data that the Source returned when the item is queried.  Application
   must make sure that the LPVOID passed in fits the data that is returned from the Source.
   Use DTWAIN_GetExtImageInfoItem to determine the type of data.   */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetExtImageInfoData(DTWAIN_SOURCE Source, LONG nWhich, LPDTWAIN_ARRAY Data)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nWhich, Data))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    if ( !DTWAIN_IsExtImageInfoSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (p->GetState() != SOURCE_STATE_TRANSFERRING);},DTWAIN_ERR_INVALID_STATE,
                                      false, FUNC_MACRO);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return !Data;}, DTWAIN_ERR_BAD_ARRAY, false, FUNC_MACRO);
    // Create the array that corresponds with the correct type
    DTWAIN_ARRAY A = DTWAIN_ArrayCreate(CTL_TwainAppMgr::ExtImageInfoArrayType(nWhich), 0);
    if ( !A )
    {
    // Check if array exists
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return !A;}, DTWAIN_ERR_BAD_ARRAY, false, FUNC_MACRO);
    }

    TW_INFO Info = p->GetExtImageInfoItem(nWhich, DTWAIN_BYID);

    LONG Count = Info.NumItems;
    DTWAIN_ArrayResize(A, Count);
    for ( int i = 0; i < Count; ++i )
    {
        if ( DTWAIN_ArrayGetType(A) == DTWAIN_ARRAYSTRING)
        {
            std::vector<char> Temp;
            size_t ItemSize;
            if ( p->GetExtImageInfoData(nWhich, DTWAIN_BYID, i, NULL, &ItemSize) )
            {
                p->GetExtImageInfoData(nWhich, DTWAIN_BYID, i, &Temp[0], NULL);
                DTWAIN_ArraySetAt(A, i, &Temp[0]);
            }
        }
        else
            p->GetExtImageInfoData(nWhich, DTWAIN_BYID, i, DTWAIN_ArrayGetBuffer(A, i));
    }
    *Data = A;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

/* Uninitializes the Extended Image information interface.  This also must be called  */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FreeExtImageInfo(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle( pHandle, Source );

    if ( !DTWAIN_IsExtImageInfoSupported(Source))
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (p->GetState() != SOURCE_STATE_TRANSFERRING);},DTWAIN_ERR_INVALID_STATE, false, FUNC_MACRO);
    p->DestroyExtImageInfo();
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

template <typename T>
struct StreamerImpl
{
    static void streamMe(CTL_OutputBaseStreamType* strm, size_t* pCur, T& val)
    { *strm << _T("Array[") << *pCur << _T("] = ") << val << _T("\n"); }
};

struct StreamerImplFrame
{
    CTL_OutputBaseStreamType* m_pStrm;
    size_t* m_pCurItem;
    StreamerImplFrame(CTL_OutputBaseStreamType* strm, size_t* curItem) : m_pStrm(strm), m_pCurItem(curItem) { *curItem = 0; }
    void operator()(DTWAINFrameInternal& pPtr)
    {
        *m_pStrm << _T("Array[") << *m_pCurItem << _T("] = {left=") <<
                    pPtr.m_FrameComponent[0] << _T(", top=") <<
                    pPtr.m_FrameComponent[1] << _T(", right=") <<
                    pPtr.m_FrameComponent[2] << _T(", bottom=") <<
                    pPtr.m_FrameComponent[3] << _T("}\n");
        ++(*m_pCurItem);
    }
};

template <typename T, typename StreamFn = StreamerImpl<T> >
struct oStreamer
{

    CTL_OutputBaseStreamType* m_pStrm;
    size_t* m_pCurItem;
    oStreamer(CTL_OutputBaseStreamType* strm, size_t* curItem) : m_pStrm(strm), m_pCurItem(curItem) { *curItem = 0; }
    void operator()(T& n)
    {
        StreamFn::streamMe(m_pStrm, m_pCurItem, n);
        ++(*m_pCurItem);
    }
};

template <typename T>
static void genericDumper(DTWAIN_ARRAY Array)
{
    // Get the array contents as a vector
    auto vCaps = EnumeratorVectorPtr<T>(Array);

    if ( !vCaps )
        return;

    CTL_StringStreamType strm;
    size_t n;

    std::for_each(vCaps->begin(), vCaps->end(), oStreamer<T>(&strm, &n));
    CTL_TwainAppMgr::WriteLogInfo( strm.str() );
}

static void DumpArrayLONG(DTWAIN_ARRAY Array, LONG lCap)
{
    if ( lCap != DTWAIN_CV_CAPSUPPORTEDCAPS )
        genericDumper<LONG>(Array);

    else
    {
        // Get the array contents as a vector
        auto vCaps = EnumeratorVectorPtr<LONG>(Array);

        if ( !vCaps )
            return;

        CTL_StringStreamType strm;
        size_t n;
        strm << _T("\n");

        // if the cap is for supported caps, then output the strings.
        // vector of names
        std::vector<CTL_StringType> CapNames;

        // get the vector of cap names given cap number
        std::transform(vCaps->begin(), vCaps->end(), std::back_inserter(CapNames), [] (LONG n)
                                    {return StringConversion::Convert_Ansi_To_Native(CTL_TwainAppMgr::GetCapNameFromCap(n));});

        // stream the cap information from the cap names
        std::for_each(CapNames.begin(), CapNames.end(), oStreamer<CTL_StringType>(&strm, &n));
        CTL_TwainAppMgr::WriteLogInfo( strm.str() );
    }
}

static void DumpArrayFLOAT(DTWAIN_ARRAY Array)
{
    genericDumper<double>(Array);
}

static void DumpArrayString(DTWAIN_ARRAY Array)
{
    genericDumper<CTL_StringType>(Array);
}


static void DumpArrayFrame(DTWAIN_ARRAY Array)
{
    auto vCaps = EnumeratorVectorPtr<DTWAINFrameInternal>(Array);
    if ( !vCaps )
        return;
    size_t n;
    CTL_StringStreamType strm;
    std::for_each(vCaps->begin(), vCaps->end(), StreamerImplFrame(&strm, &n));
    CTL_TwainAppMgr::WriteLogInfo( strm.str() );
}

void dynarithmic::DumpArrayContents(DTWAIN_ARRAY Array, LONG lCap)
{
    if ( CTL_TwainDLLHandle::s_lErrorFilterFlags == 0 )
        return;

    CTL_StringType szBuf;
    // This dumps contents of array to log file
    {
        // Turn off the error logging flags temporarily
        {
            DTWAINScopedLogController sLogger(0);
            if (!Array)
            {
                szBuf = _T("DTWAIN_ARRAY is NULL\n");
                // Turn on the error logging flags
                CTL_TwainAppMgr::WriteLogInfo(szBuf);
                return;
            }
        }

        LONG nCount = EnumeratorFunctionImpl::EnumeratorGetCount(Array);
        CTL_StringStreamType strm;
        strm << _T("Dumping contents of DTWAIN_ARRAY ") << Array << _T("\nNumber of elements: ") << nCount << _T("\n");
        szBuf = strm.str();
    }

    CTL_TwainAppMgr::WriteLogInfo(szBuf);

    // determine the type
    LONG nType = DTWAIN_ArrayGetType(Array);
    switch (nType)
    {
        case DTWAIN_ARRAYLONG:
            DumpArrayLONG(Array, lCap);
            break;

        case DTWAIN_ARRAYFLOAT:
            DumpArrayFLOAT(Array);
            break;

        case DTWAIN_ARRAYSTRING:
            DumpArrayString(Array);
            break;

        case DTWAIN_ARRAYFRAME:
            DumpArrayFrame(Array);
            break;
    }
}


int GetMultiCapValues(DTWAIN_HANDLE DLLHandle,
                       DTWAIN_SOURCE Source,
                       DTWAIN_ARRAY pArray,
                       CTL_EnumeratorType EnumType,
                       TW_UINT16 nCap,
                       CTL_EnumGetType GetType,
                       CTL_StringType cStr,
                       TW_UINT16 TwainDataType,
                       UINT nContainerVal,
                       bool bUseContainer,
                       CTL_String )
{
    return GetMultiCapValuesImpl<CTL_String, CTL_StringType, CTL_EnumeratorType, VectorAdderFn2>::GetMultiCapValues
        (DLLHandle, Source, pArray, EnumType, nCap, GetType, cStr, TwainDataType, nContainerVal, bUseContainer, CTL_String());
}

int GetMultiCapValues(DTWAIN_HANDLE DLLHandle,
                      DTWAIN_SOURCE Source,
                      DTWAIN_ARRAY pArray,
                      CTL_EnumeratorType,
                      TW_UINT16 nCap,
                      CTL_EnumGetType GetType,
                      DTWAIN_FRAME,
                      TW_UINT16 TwainDataType,
                      UINT nContainerVal,
                      bool,
                      TW_FRAME )
{
    // Create a TW_FRAME array
    DTWAIN_ARRAY FrameArray = DTWAIN_ArrayCreate(CTL_EnumeratorTWFrameType, 0);
    if ( !FrameArray )
        return 0;
    DTWAINArrayLL_RAII fArr(FrameArray);

    TW_FRAME fValue{ {0,0},{0,0},{0,0},{0,0} };
    int bOk = GetMultiCapValues(DLLHandle, Source, FrameArray, CTL_EnumeratorTWFrameType, (UINT)nCap, (CTL_EnumGetType)GetType, fValue,
                            static_cast<TW_UINT16>(TwainDataType), (UINT)nContainerVal, true, TW_FRAME());

    if (!bOk)
        return 0;

    int nSize = DTWAIN_ArrayGetCount(FrameArray);

    for (int i = 0; i < nSize; i++)
    {
        DTWAIN_FRAME DTWAINFrame = DTWAIN_FrameCreate(0, 0, 0, 0);
        DTWAINFrame_RAII raii(DTWAINFrame);
        auto& FrameV = EnumeratorVector<TW_FRAME>(FrameArray);
        TWFRAMEToDTWAINFRAME(FrameV[i], DTWAINFrame);
        EnumeratorFunctionImpl::EnumeratorAddValue(pArray, DTWAINFrame);
    }
    return 1;
}

