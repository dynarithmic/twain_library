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
#include <boost/format.hpp>
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "ctltr025.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetImageInfoString(DTWAIN_SOURCE Source,
                                                LPTSTR XResolution,
                                                LPTSTR YResolution,
                                                LPLONG Width,
                                                LPLONG Length,
                                                LPLONG NumSamples,
                                                LPDTWAIN_ARRAY BitsPerSample,
                                                LPLONG BitsPerPixel,
                                                LPLONG Planar,
                                                LPLONG PixelType,
                                                LPLONG Compression)
{
    LOG_FUNC_ENTRY_PARAMS((Source, XResolution, YResolution, Width, Length, NumSamples, BitsPerSample, BitsPerPixel, Planar, PixelType, Compression))

    DTWAIN_FLOAT tempX;
    DTWAIN_FLOAT tempY;
    DTWAIN_BOOL retVal = DTWAIN_GetImageInfo(Source, &tempX, &tempY, Width, Length, NumSamples, BitsPerSample, BitsPerPixel, Planar, PixelType, Compression);
    if (retVal)
    {
        CTL_StringStreamType strm;
        strm << BOOST_FORMAT(_T("%1%")) % tempX;
        StringWrapper::SafeStrcpy(XResolution, strm.str().c_str());
        strm.str(_T(""));
        strm << BOOST_FORMAT(_T("%1%")) % tempY;
        StringWrapper::SafeStrcpy(YResolution, strm.str().c_str());
    }
    LOG_FUNC_EXIT_PARAMS(retVal)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetImageInfo(DTWAIN_SOURCE Source,
                                            LPDTWAIN_FLOAT XResolution,
                                            LPDTWAIN_FLOAT YResolution,
                                            LPLONG Width,
                                            LPLONG Length,
                                            LPLONG NumSamples,
                                            LPDTWAIN_ARRAY BitsPerSample,
                                            LPLONG BitsPerPixel,
                                            LPLONG Planar,
                                            LPLONG PixelType,
                                            LPLONG Compression)
{
    LOG_FUNC_ENTRY_PARAMS((Source, XResolution, YResolution, Width, Length, NumSamples, BitsPerSample,BitsPerPixel, Planar, PixelType, Compression))

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);

    if (!p)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen(p)); },
    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    CTL_ImageInfoTriplet II(pHandle->m_Session, p);

    if (!CTL_TwainAppMgr::GetImageInfo(p, &II))
        LOG_FUNC_EXIT_PARAMS(false)

        // Get the image information
    TW_IMAGEINFO *pInfo = II.GetImageInfoBuffer();
    if (XResolution)
        *XResolution = (DTWAIN_FLOAT)CTL_CapabilityTriplet::Twain32ToFloat(pInfo->XResolution);
    if (YResolution)
        *YResolution = (DTWAIN_FLOAT)CTL_CapabilityTriplet::Twain32ToFloat(pInfo->YResolution);
    if (Width)
        *Width = pInfo->ImageWidth;
    if (Length)
        *Length = pInfo->ImageLength;
    if (NumSamples)
        *NumSamples = pInfo->SamplesPerPixel;
    if (BitsPerPixel)
        *BitsPerPixel = pInfo->BitsPerPixel;


    if (BitsPerSample)
    {
        DTWAIN_ARRAY Array = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 8);
        auto& vValues = EnumeratorVector<LONG>(Array);
        TW_INT16* pStart = &pInfo->BitsPerSample[0];
        TW_INT16* pEnd = &pInfo->BitsPerSample[8];
        std::copy(pStart, pEnd, vValues.begin());
        *BitsPerSample = Array;
    }

    if (Planar)
        *Planar = pInfo->Planar;

    if (PixelType)
        *PixelType = pInfo->PixelType;

    if (Compression)
        *Compression = pInfo->Compression;

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}
