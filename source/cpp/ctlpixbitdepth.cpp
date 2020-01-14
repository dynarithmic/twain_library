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
#include <cstring>
#include "dtwain.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
using namespace dynarithmic;

static DTWAIN_BOOL GetPixelType(DTWAIN_SOURCE Source, LPLONG PixelType, LPLONG BitDepth, LONG GetType);
static DTWAIN_BOOL DTWAIN_SetPixelTypeHelper(DTWAIN_SOURCE Source, LONG PixelType, LONG BitDepth, DTWAIN_BOOL bSetCurrent);

// Pixel Types and Bit depths
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPixelType(DTWAIN_SOURCE Source, LONG PixelType, LONG BitDepth, DTWAIN_BOOL bSetCurrent)
{
    LOG_FUNC_ENTRY_PARAMS((Source, PixelType, BitDepth, bSetCurrent))
    // reset the values first
    DTWAIN_BOOL bRet = TRUE;
    if (PixelType == DTWAIN_PT_DEFAULT && BitDepth == DTWAIN_DEFAULT)
    {
        bRet = DTWAIN_SetPixelTypeHelper(Source, PixelType, BitDepth, TRUE);
        LOG_FUNC_EXIT_PARAMS(bRet)
    }

    bRet = DTWAIN_SetPixelTypeHelper(Source, PixelType, -1, FALSE);
    // Now set the value if value needs to be set
    if ( bSetCurrent )
        bRet = DTWAIN_SetPixelTypeHelper(Source, PixelType, BitDepth, TRUE);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DTWAIN_SetPixelTypeHelper(DTWAIN_SOURCE Source, LONG PixelType, LONG BitDepth, DTWAIN_BOOL bSetCurrent)
{
    LOG_FUNC_ENTRY_PARAMS((Source, PixelType, BitDepth, bSetCurrent))

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    LONG SetType = DTWAIN_CAPSET;
    if ( !bSetCurrent )
        SetType = DTWAIN_CAPRESET;
    DTWAIN_ARRAY Array = DTWAIN_ArrayCreateFromCap( NULL, DTWAIN_CV_ICAPPIXELTYPE, 1);
    if ( !Array )
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAINArrayLL_RAII a(Array);

    auto& vValues = EnumeratorVector<LONG>(Array);
    LONG defaultBitDepth = -1;
    if (PixelType == DTWAIN_PT_DEFAULT)
        GetPixelType(Source, &PixelType, &defaultBitDepth, DTWAIN_CAPGETDEFAULT);

    vValues[0] = PixelType;

    DTWAIN_BOOL bRet = DTWAIN_SetCapValues(Source, DTWAIN_CV_ICAPPIXELTYPE, SetType, Array );
    if ( bRet )
    {
        // Set the source value in the cache
        CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
        pSource->SetCapCacheValue(DTWAIN_CV_ICAPPIXELTYPE, (double)PixelType, TRUE);

        // Test if bit depth is desired to be set
        DTWAIN_BOOL bSetBitDepth = TRUE;
        if (BitDepth == DTWAIN_DEFAULT)
        {
            if (defaultBitDepth == -1)
            {
                if (!DTWAIN_GetBitDepth(Source, &defaultBitDepth, TRUE))
                    bSetBitDepth = FALSE;
            }
            BitDepth = defaultBitDepth;
        }

        if ( bSetBitDepth )
        {
            DTWAIN_BOOL bBitRet = DTWAIN_SetBitDepth(Source, BitDepth, bSetBitDepth);
            if ( !bBitRet )
            {
                LOG_FUNC_EXIT_PARAMS(bBitRet)
            }
        }
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPixelType(DTWAIN_SOURCE Source, LPLONG PixelType, LPLONG BitDepth, DTWAIN_BOOL bCurrent)
{
    LOG_FUNC_ENTRY_PARAMS((Source, PixelType, BitDepth, bCurrent))
    LONG GetType = DTWAIN_CAPGETDEFAULT;
    if ( bCurrent )
        GetType = DTWAIN_CAPGETCURRENT;
    DTWAIN_BOOL bRet = GetPixelType(Source, PixelType, BitDepth, GetType);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL GetPixelType(DTWAIN_SOURCE Source, LPLONG PixelType, LPLONG BitDepth, LONG GetType)
{
    DTWAIN_ARRAY Array = 0;
    DTWAIN_BOOL bRet = DTWAIN_GetCapValues(Source, DTWAIN_CV_ICAPPIXELTYPE, GetType, &Array );
    if ( bRet )
    {
        DTWAINArrayLL_RAII arr(Array);
        auto vIn = EnumeratorVectorPtr<LONG>(Array);

        if ( vIn && !vIn->empty())
        {
            *PixelType = (*vIn)[0];
            // Check if bitdepth is to be retrieved
            if ( BitDepth )
                DTWAIN_GetBitDepth(Source, BitDepth, GetType);
        }
    }
    return bRet;
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetBitDepth(DTWAIN_SOURCE Source, LONG BitDepth, DTWAIN_BOOL bSetCurrent)
{
    LOG_FUNC_ENTRY_PARAMS((Source, BitDepth, bSetCurrent))
    LONG SetType = DTWAIN_CAPSET;
    if ( !bSetCurrent )
        SetType = DTWAIN_CAPRESET;
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_BOOL bRet = FALSE;
    DTWAIN_ARRAY Array = DTWAIN_ArrayCreateFromCap( NULL, DTWAIN_CV_ICAPBITDEPTH, 1);
    if ( !Array )
        LOG_FUNC_EXIT_PARAMS(false)
    DTWAINArrayLL_RAII a(Array);
    auto vIn = EnumeratorVectorPtr<LONG>(Array);
    if ( vIn && !vIn->empty())
    {
        (*vIn)[0] = BitDepth;
        bRet = DTWAIN_SetCapValues(Source, DTWAIN_CV_ICAPBITDEPTH, SetType, Array);
        if ( !bRet )
        {
            DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return true;},
                                DTWAIN_ERR_INVALID_BITDEPTH, false, FUNC_MACRO);
        }
        else
        {
          // Set the source value in the cache
            CTL_ITwainSource *pSource = VerifySourceHandle( pHandle, Source );
            pSource->SetCapCacheValue(DTWAIN_CV_ICAPBITDEPTH, (double)BitDepth, TRUE);
        }
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetBitDepth(DTWAIN_SOURCE Source, LPLONG BitDepth, DTWAIN_BOOL bCurrent)
{
    LOG_FUNC_ENTRY_PARAMS((Source, BitDepth, bCurrent))
    DTWAIN_ARRAY Array = 0;
    DTWAIN_BOOL bRet;
    LONG GetType = DTWAIN_CAPGETCURRENT;
    if ( !bCurrent )
        GetType = DTWAIN_CAPGETDEFAULT;
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !BitDepth;} ,
                                 DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    bRet = DTWAIN_GetCapValues(Source, DTWAIN_CV_ICAPBITDEPTH, GetType, &Array);
    DTWAINArrayLL_RAII arr(Array);
    if ( bRet && Array )
    {
        auto& vIn = EnumeratorVector<LONG>(Array);
        if ( !vIn.empty() )
            *BitDepth = vIn[0];
        else
            *BitDepth = DTWAIN_DEFAULT;
    }
    else
        *BitDepth = DTWAIN_DEFAULT;

    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPixelTypes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray)
{
    LOG_FUNC_ENTRY_PARAMS((Source, pArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle((DTWAIN_HANDLE)pHandle, Source);
    if (  p )
    {
        DTWAIN_ARRAY arr = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 0);
        auto& vIn = EnumeratorVector<LONG>(arr);
        const CTL_ITwainSource::CachedPixelTypeMap& theMap = p->GetPixelTypeMap();
        std::transform(theMap.begin(), theMap.end(), std::back_inserter(vIn), []
                (const CTL_ITwainSource::CachedPixelTypeMap::value_type& v) { return v.first; });
        *pArray = arr;
        LOG_FUNC_EXIT_PARAMS(true)
    }

    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumBitDepthsEx(DTWAIN_SOURCE Source, LONG PixelType, LPDTWAIN_ARRAY Array)
{
    LOG_FUNC_ENTRY_PARAMS((Source, PixelType, Array))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle((DTWAIN_HANDLE)pHandle, Source);
    if (  p && p->IsPixelTypeSupported(PixelType) )
    {
        DTWAIN_ARRAY arr = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 0);
        auto& vIn = EnumeratorVector<LONG>(arr);
        const CTL_ITwainSource::CachedPixelTypeMap& theMap = p->GetPixelTypeMap();
        const std::vector<int>& pBitDepths = theMap.find(PixelType)->second;
        std::copy(pBitDepths.begin(), pBitDepths.end(), std::back_inserter(vIn));
        *Array = arr;
        LOG_FUNC_EXIT_PARAMS(true)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPixelTypeSupported(DTWAIN_SOURCE Source, LONG PixelType)
{
    LOG_FUNC_ENTRY_PARAMS((Source, PixelType))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle((DTWAIN_HANDLE)pHandle, Source);
    if (  p )
    {
        DTWAIN_BOOL bRet = p->IsPixelTypeSupported(PixelType)?TRUE:FALSE;
        LOG_FUNC_EXIT_PARAMS(bRet)
    }

    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumFileTypeBitsPerPixel(LONG FileType, LPDTWAIN_ARRAY Array)
{
	LOG_FUNC_ENTRY_PARAMS((FileType, Array))
	CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

	// See if DLL Handle exists
	DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

	if (Array)
	{
		if (EnumeratorFunctionImpl::EnumeratorIsValid(*Array))
			EnumeratorFunctionImpl::ClearEnumerator(*Array);
	}

	DTWAIN_ARRAY ThisArray = 0;
	ThisArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 0);
	DTWAINArrayLL_RAII arr(ThisArray);
	auto& bppMap = CTL_ImageIOHandler::GetSupportedBPPMap();
	auto iter = bppMap.find(FileType);
	if (iter != bppMap.end())
		for_each(iter->second.begin(), iter->second.end(),
			[&](int val) {LONG lVal = val;  EnumeratorFunctionImpl::EnumeratorAddValue(ThisArray, &lVal); });
	*Array = ThisArray;
	arr.Disconnect();
	LOG_FUNC_EXIT_PARAMS(iter != bppMap.end())
	CATCH_BLOCK(FALSE)
}

LONG DLLENTRY_DEF DTWAIN_MakeRGB(LONG red, LONG green, LONG blue)
{
	LOG_FUNC_ENTRY_PARAMS((red, green, blue))
	LONG returnVal = RGB(red, green, blue);
	LOG_FUNC_EXIT_PARAMS(returnVal);
	CATCH_BLOCK(0)
}
