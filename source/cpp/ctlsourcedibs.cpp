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
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_GetSourceAcquisitions(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(NULL)
    DTWAIN_ARRAY Array = p->GetAcquisitionArray();
    if (!Array)
        LOG_FUNC_EXIT_PARAMS(NULL)
    LOG_FUNC_EXIT_PARAMS(Array)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

DTWAIN_BOOL dynarithmic::DTWAIN_GetAllSourceDibs(DTWAIN_SOURCE Source, DTWAIN_ARRAY pArray)
{
    LOG_FUNC_ENTRY_PARAMS((Source, pArray))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

        // Check if array is of the correct type
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (!EnumeratorFunctionImpl::EnumeratorIsValidEx(pArray, CTL_EnumeratorHandleType)); },
                                                    DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);
    DTWAIN_ARRAY pDTWAINArray = pArray;
    EnumeratorFunctionImpl::ClearEnumerator(pDTWAINArray);

    // Copy DIBs to the array
    int nCount = pSource->GetNumDibs();
    HANDLE hDib;
    for (int i = 0; i < nCount; i++)
    {
        hDib = pSource->GetDibHandle(i);
        if (hDib)
            DTWAIN_ArrayAdd(pArray, &hDib);
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAllSourceDibsEx(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY  pArray)
{
    LOG_FUNC_ENTRY_PARAMS((Source, pArray))
    DTWAIN_ARRAY A = 0;
    A = DTWAIN_ArrayCreate(DTWAIN_ARRAYHANDLE, 0);
    if (A)
    {
        DTWAIN_GetAllSourceDibs(Source, A);
        *pArray = A;
        LOG_FUNC_EXIT_PARAMS(true)
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

HANDLE DLLENTRY_DEF DTWAIN_GetCurrentAcquiredImage(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(NULL)

    int nCount = pSource->GetNumDibs();
    if (nCount == 0)
        LOG_FUNC_EXIT_PARAMS(NULL)
    LOG_FUNC_EXIT_PARAMS((HANDLE)pSource->GetDibHandle(nCount - 1))
    CATCH_BLOCK(HANDLE(0))
}

LONG DLLENTRY_DEF DTWAIN_GetCurrentPageNum(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(-1L)

        // return the file name that would be acquired
    LONG retval = (LONG)pSource->GetPendingImageNum();
    LOG_FUNC_EXIT_PARAMS(retval)
    CATCH_BLOCK(-1L);
}

DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_CreateAcquisitionArray()
{
    LOG_FUNC_ENTRY_PARAMS(())
    DTWAIN_ARRAY A = (DTWAIN_ARRAY)DTWAIN_ArrayCreate(DTWAIN_ArrayTypePTR, 0);
    LOG_FUNC_EXIT_PARAMS(A)
    CATCH_BLOCK(DTWAIN_ARRAY(0))
}

// class whose purpose is to destroy the image data array
struct NestedAcquisitionDestroyer
{
    bool m_bDestroyDibs;
    NestedAcquisitionDestroyer(bool bDestroyDibs) : m_bDestroyDibs(bDestroyDibs) {}

    void operator()(DTWAIN_ARRAY ImagesArray)
    {
        // we want this array destroyed when we're finished
        DTWAINArrayLL_RAII raii(ImagesArray);

        // Test if the DIB data should also be destroyed
        if (m_bDestroyDibs)
        {
            // get underlying vector of dibs
            auto& vHandles = EnumeratorVector<HANDLE>(ImagesArray);

            // for each dib, destroy the data
            std::for_each(vHandles.begin(), vHandles.end(), DestroyDibData);
        }
    }

    static void DestroyDibData(HANDLE hImageData)
    {
        #ifdef _WIN32
        UINT nCount = GlobalFlags(hImageData) & GMEM_LOCKCOUNT;
        #else
        UINT nCount = 1;
        #endif
        for (UINT k = 0; k < nCount; k++)
            ImageMemoryHandler::GlobalUnlock(hImageData);
        ImageMemoryHandler::GlobalFree(hImageData);
    }
};

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_DestroyAcquisitionArray(DTWAIN_ARRAY aAcq, DTWAIN_BOOL bDestroyDibs)
{
    LOG_FUNC_ENTRY_PARAMS((aAcq))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, NULL);

    // Check if array exists
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (!EnumeratorFunctionImpl::EnumeratorIsValid(aAcq)); }, DTWAIN_ERR_WRONG_ARRAY_TYPE, false, FUNC_MACRO);

    // Make sure this array is destroyed when we exit this function
    DTWAINArrayLL_RAII raiiMain(aAcq);

    // get instance of acquisition destroy class
    NestedAcquisitionDestroyer acqDestroyer(bDestroyDibs ? true : false);

    // underlying images array
    auto& vImagesArray = EnumeratorVector<LPVOID>(aAcq);

    // for each image array, destroy it
    std::for_each(vImagesArray.begin(), vImagesArray.end(), acqDestroyer);

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

static bool SetBitDepth(CTL_ITwainSource *p, LONG BitDepth);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ForceAcquireBitDepth(DTWAIN_SOURCE Source, LONG BitDepth)
{
    LOG_FUNC_ENTRY_PARAMS((Source, BitDepth))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    if (!p)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen(p)); },
                DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    DTWAIN_BOOL bRet = SetBitDepth(p, BitDepth);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

static bool SetBitDepth(CTL_ITwainSource *p, LONG BitDepth)
{
    if (BitDepth == 1 ||
        BitDepth == 4 ||
        BitDepth == 8 ||
        BitDepth == 24)
    {
        p->SetForcedImageBpp(BitDepth);
        return true;
    }
    else
        p->SetForcedImageBpp(0);
    return true;
}
