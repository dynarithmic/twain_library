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
#include "dtwain.h"
#include "ctliface.h"
#include <twilres.h>
#include "ctltwmgr.h"
#include "Transym_OCRInterface.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#ifdef _MSC_VER
#pragma warning (disable:4505)
#endif
// OCR routines
static bool OCREngineExists(CTL_TwainDLLHandle* pHandle, OCREngine* pEngine);
//static bool NewOCRJob(OCREngine *pEngine, LPCSTR szFileName);
static bool OCRIsActive(const OCREngine* pEngine);
static LONG GetOCRTextSupport(OCREngine* pEngine, LONG fileType, LONG pixelType, LONG bitDepth);

typedef CTL_StringType(OCREngine::*OCRINFOFUNC)() const;

HANDLE DLLENTRY_DEF DTWAIN_GetOCRText(DTWAIN_OCRENGINE Engine,
    LONG nPageNo,
    LPTSTR Data,
    LONG dSize,
    LPLONG pActualSize,
    LONG nFlags)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, nPageNo, Data, dSize, pActualSize, nFlags))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    // Check if OCR is active
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCRIsActive(pEngine); }, DTWAIN_ERR_OCR_NOTACTIVE, NULL, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !pEngine->IsValidOCRPage(nPageNo); }, DTWAIN_ERR_OCR_INVALIDPAGENUM, NULL, FUNC_MACRO);

    CTL_StringType sText = pEngine->GetOCRText(nPageNo);

    // Copy actual size data to parameter
    if (pActualSize)
        *pActualSize = (long)sText.length();
    int localActualSize = (int)sText.length();

    // Return the handle if that is all user wants to do
    if (nFlags & DTWAINOCR_RETURNHANDLE)
    {
        HANDLE theHandle;
        TW_MEMREF hMem = CTL_TwainDLLHandle::s_TwainMemoryFunc->AllocateMemoryPtr(localActualSize*sizeof(TCHAR), &theHandle);
        memcpy(hMem, sText.c_str(), localActualSize*sizeof(TCHAR));
        CTL_TwainDLLHandle::s_TwainMemoryFunc->UnlockMemory(theHandle);
        LOG_FUNC_EXIT_PARAMS(theHandle);
    }
    else
        if (nFlags & DTWAINOCR_COPYDATA)
        {
            if (!Data)
            {
                // cache the info
                LOG_FUNC_EXIT_PARAMS(HANDLE(1));
            }
            int nMinCopy;

            if (dSize == -1)
                nMinCopy = localActualSize;
            nMinCopy = (std::max)((std::min)(dSize, (LONG)localActualSize), (LONG)0);

            memcpy(Data, sText.data(), nMinCopy * sizeof(TCHAR));
            LOG_FUNC_EXIT_PARAMS(HANDLE(1));
        }
    LOG_FUNC_EXIT_PARAMS(NULL);
    CATCH_BLOCK(HANDLE())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRCapValues(DTWAIN_OCRENGINE Engine,
    LONG OCRCapValue,
    LONG GetType,
    LPDTWAIN_ARRAY CapValues)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, OCRCapValue, GetType, CapValues))

        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    if (pEngine->IsCapSupported(OCRCapValue))
    {
        LONG nDataType = pEngine->GetCapDataType(OCRCapValue);
        if (nDataType == -1)
            LOG_FUNC_EXIT_PARAMS(false)
            switch (nDataType)
        {
            case DTWAIN_ARRAYLONG:
            {
                OCREngine::OCRLongArrayValues vals;
                pEngine->GetCapValues(OCRCapValue, GetType, vals);
                DTWAIN_ARRAY theArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, (LONG)vals.size());

                for (LONG i = 0; i < (LONG)vals.size(); ++i)
                    DTWAIN_ArraySetAtLong(theArray, i, vals[i]);
                *CapValues = theArray;
                LOG_FUNC_EXIT_PARAMS(true)
            }
            break;
            case DTWAIN_ARRAYSTRING:
            {
                OCREngine::OCRStringArrayValues vals;
                pEngine->GetCapValues(OCRCapValue, GetType, vals);
                DTWAIN_ARRAY theArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYSTRING, (LONG)vals.size());
                for (LONG i = 0; i < (LONG)vals.size(); ++i)
                    DTWAIN_ArraySetAtStringA(theArray, i, vals[i].c_str());
                *CapValues = theArray;
                LOG_FUNC_EXIT_PARAMS(true)
            }
            break;
        }
    }
    LOG_FUNC_EXIT_PARAMS(false)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetOCRCapValues(DTWAIN_OCRENGINE Engine,
    LONG OCRCapValue,
    LONG SetType,
    DTWAIN_ARRAY CapValues)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, OCRCapValue, SetType, CapValues))

        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    if (pEngine->IsCapSupported(OCRCapValue))
    {
        LONG nDataType = pEngine->GetCapDataType(OCRCapValue);
        if (nDataType == -1)
            LOG_FUNC_EXIT_PARAMS(false)
            switch (nDataType)
        {
            case DTWAIN_ARRAYLONG:
            {
                OCREngine::OCRLongArrayValues vals;
                LONG nCount = DTWAIN_ArrayGetCount(CapValues);
                if (nCount < 1)
                    LOG_FUNC_EXIT_PARAMS(false)
                    vals.resize(nCount);
                LONG *ArrayStart = (LONG*)DTWAIN_ArrayGetBuffer(CapValues, 0);
                std::copy(ArrayStart, ArrayStart + nCount, vals.begin());
                BOOL bRet = pEngine->SetCapValues(OCRCapValue, SetType, vals);
                LOG_FUNC_EXIT_PARAMS(bRet)
            }
            break;

            case DTWAIN_ARRAYSTRING:
            {
                OCREngine::OCRStringArrayValues vals;
                LONG nCount = DTWAIN_ArrayGetCount(CapValues);
                if (nCount < 1)
                    LOG_FUNC_EXIT_PARAMS(false)
                    vals.resize(nCount);
                char buffer[1024];
                for (LONG i = 0; i < nCount; ++i)
                {
                    DTWAIN_ArrayGetAtStringA(CapValues, i, buffer);
                    vals[i] = buffer;
                }
                BOOL bRet = pEngine->SetCapValues(OCRCapValue, SetType, vals);
                LOG_FUNC_EXIT_PARAMS(bRet)
            }
        }
    }
    LOG_FUNC_EXIT_PARAMS(false)
    CATCH_BLOCK(false)
}

//////////////////// OCR information functions /////////////////////////
static LONG GetOCRInfo(OCREngine *pEngine,
    OCRINFOFUNC pFunc,
    LPTSTR szInfo, LONG nMaxLen);

LONG GetOCRInfo(OCREngine *pEngine,
    OCRINFOFUNC pFunc,
    LPTSTR szInfo, LONG nMaxLen)
{
    CTL_StringType pName = (pEngine->*pFunc)();
    int nLen = (int)pName.length();
    if (szInfo == NULL)
        return (LONG)nLen;
    int nRealLen;
    nRealLen = (std::min)((int)nMaxLen, nLen);
    StringTraits::StringCopyN(szInfo, pName.c_str(), nRealLen);
    szInfo[nRealLen] = _T('\0');
    return nRealLen;
}

LONG   DLLENTRY_DEF DTWAIN_GetOCRManufacturer(DTWAIN_OCRENGINE Engine,
    LPTSTR szMan,
    LONG nMaxLen)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, szMan, nMaxLen))

        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, NULL, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    if (pEngine)
    {
        LONG Ret = GetOCRInfo(pEngine, (OCRINFOFUNC)&OCREngine::GetManufacturer,
            szMan, nMaxLen);
        LOG_FUNC_EXIT_PARAMS(Ret)
    }
    LOG_FUNC_EXIT_PARAMS(-1L)
        CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG   DLLENTRY_DEF DTWAIN_GetOCRProductFamily(DTWAIN_OCRENGINE Engine,
    LPTSTR szMan,
    LONG nMaxLen)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, szMan, nMaxLen))

        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, NULL, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    if (pEngine)
    {
        LONG Ret = GetOCRInfo(pEngine, (OCRINFOFUNC)&OCREngine::GetProductFamily, szMan, nMaxLen);
        LOG_FUNC_EXIT_PARAMS(Ret)
    }
    LOG_FUNC_EXIT_PARAMS(-1L)
        CATCH_BLOCK(DTWAIN_FAILURE1)
}

LONG   DLLENTRY_DEF DTWAIN_GetOCRProductName(DTWAIN_OCRENGINE Engine,
    LPTSTR szMan,
    LONG nMaxLen)
{
    if (szMan)
        szMan[0] = '\0';
    LOG_FUNC_ENTRY_PARAMS((Engine, szMan, nMaxLen))

        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, NULL, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    if (pEngine)
    {
        LONG Ret = GetOCRInfo(pEngine, (OCRINFOFUNC)&OCREngine::GetProductName, szMan, nMaxLen);
        LOG_FUNC_EXIT_PARAMS(Ret)
    }
    LOG_FUNC_EXIT_PARAMS(-1L)
        CATCH_BLOCK(DTWAIN_FAILURE1)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ExecuteOCR(DTWAIN_OCRENGINE Engine,
    LPCTSTR szFileName,
    LONG nStartPage,
    LONG nEndPage)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, szFileName, nStartPage, nEndPage))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);


    OCREngine *pEngine = (OCREngine*)Engine;

    // Check if OCR is active
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCRIsActive(pEngine); }, DTWAIN_ERR_OCR_NOTACTIVE, false, FUNC_MACRO);

    std::string sText;
    if (nStartPage > nEndPage)
        LOG_FUNC_EXIT_PARAMS(false);

    int stat;
    LONG nPages = pEngine->GetNumPagesInFile(szFileName, stat);
    if (nPages < 0)
    {
        std::string s = pEngine->GetReturnCodeString(stat);
        LOG_FUNC_EXIT_PARAMS(false);
    }
    if (nStartPage == -1)
    {
        nStartPage = 0;
        nEndPage = nPages - 1;
    }
    else
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return (nStartPage >= nPages); },
        DTWAIN_ERR_OCR_INVALIDPAGENUM, false, FUNC_MACRO);

    LONG minEndPage = (std::min)(nEndPage, nPages - 1);

    LONG curPage = nStartPage;
    pEngine->ClearCharacterInfoMap();
    while (curPage <= minEndPage)
    {
        // Reset that everything is OK so far
        pEngine->SetOkErrorCode();
        pEngine->SetCurrentPageNumber(curPage);
        LONG retCode = pEngine->StartOCR(szFileName);

        // Check for an error here
        if (!pEngine->IsReturnCodeOk(retCode))
        {
            // No good, we need to get out and report the error
            pEngine->SetLastError(retCode);
            DTWAIN_Check_Error_Condition_1_Ex(pHandle, [] { return 1; },
                DTWAIN_ERR_OCR_RECOGNITIONERROR, false, FUNC_MACRO);
        }
        ++curPage;
    }
    LOG_FUNC_EXIT_PARAMS(true);
    CATCH_BLOCK(false)
}

DTWAIN_OCRTEXTINFOHANDLE DLLENTRY_DEF DTWAIN_GetOCRTextInfoHandle(DTWAIN_OCRENGINE Engine, LONG nPageNo)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, nPageNo))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    // Check if OCR is active
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCRIsActive(pEngine); }, DTWAIN_ERR_OCR_NOTACTIVE, false, FUNC_MACRO);
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !pEngine->IsValidOCRPage(nPageNo); }, DTWAIN_ERR_OCR_INVALIDPAGENUM, NULL, FUNC_MACRO);

    // If nNumInfo is not NULL, fill it in with the number of items
    int status;
    std::vector<OCRCharacterInfo>& cInfo = pEngine->GetCharacterInfo(nPageNo, status);

    OCRCharacterInfo* pInfo = &cInfo[0];
    DTWAIN_OCRTEXTINFOHANDLE pReturn = (DTWAIN_OCRTEXTINFOHANDLE)pInfo;
    LOG_FUNC_EXIT_PARAMS(pReturn)
        CATCH_BLOCK(DTWAIN_OCRTEXTINFOHANDLE())
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoLong(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo,
    LONG nCharPos,
    LONG nWhichItem,
    LPLONG pInfo)
{
    LOG_FUNC_ENTRY_PARAMS((OCRTextInfo, nCharPos, nWhichItem, pInfo))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !pInfo; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    OCRCharacterInfo *cInfo = (OCRCharacterInfo*)OCRTextInfo;

    switch (nWhichItem)
    {
    case DTWAIN_OCRINFO_CHAR:
        *pInfo = cInfo->iChar[nCharPos];
        break;

    case DTWAIN_OCRINFO_CHARXPOS:
        *pInfo = cInfo->xPosition[nCharPos];
        break;

    case DTWAIN_OCRINFO_CHARYPOS:
        *pInfo = cInfo->yPosition[nCharPos];
        break;

    case DTWAIN_OCRINFO_CHARXWIDTH:
        *pInfo = cInfo->xWidth[nCharPos];
        break;

    case DTWAIN_OCRINFO_CHARYWIDTH:
        *pInfo = cInfo->yWidth[nCharPos];
        break;

    case DTWAIN_OCRINFO_PAGENUM:
        *pInfo = cInfo->nPage;
        break;

    case DTWAIN_OCRINFO_TEXTLENGTH:
        *pInfo = (LONG)cInfo->iChar.size();
        break;

    default:
    {
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [] { return 1; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    }
    break;
    }
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoFloat(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo,
    LONG nCharPos,
    LONG nWhichItem,
    LPDTWAIN_FLOAT pInfo)
{
    LOG_FUNC_ENTRY_PARAMS((OCRTextInfo, nCharPos, nWhichItem, pInfo))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !pInfo; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    OCRCharacterInfo *cInfo = (OCRCharacterInfo*)OCRTextInfo;

    switch (nWhichItem)
    {
    case DTWAIN_OCRINFO_CHARCONFIDENCE:
        *pInfo = cInfo->dConfidence[nCharPos];
        break;

    default:
    {
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [] { return TRUE; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    }
    break;

    }
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoLongEx(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo,
    LONG nWhichItem,
    LPLONG pInfo,
    LONG bufSize)
{

    LOG_FUNC_ENTRY_PARAMS((OCRTextInfo, nWhichItem, pInfo, bufSize))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !pInfo; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    OCRCharacterInfo *cInfo = (OCRCharacterInfo*)OCRTextInfo;

    std::vector<int>::iterator itStart;
    std::vector<int>::iterator itEnd;

    switch (nWhichItem)
    {
    case DTWAIN_OCRINFO_CHAR:
        itStart = cInfo->iChar.begin();
        itEnd = cInfo->iChar.end();
        break;

    case DTWAIN_OCRINFO_CHARXPOS:
        itStart = cInfo->xPosition.begin();
        itEnd = cInfo->xPosition.end();
        break;

    case DTWAIN_OCRINFO_CHARYPOS:
        itStart = cInfo->yPosition.begin();
        itEnd = cInfo->yPosition.end();
        break;

    case DTWAIN_OCRINFO_CHARXWIDTH:
        itStart = cInfo->xWidth.begin();
        itEnd = cInfo->xWidth.end();
        break;

    case DTWAIN_OCRINFO_CHARYWIDTH:
        itStart = cInfo->yWidth.begin();
        itEnd = cInfo->yWidth.end();
        break;

    default:
    {
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [] { return 1; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    }
    break;
    }
    LONG actualSize = 0;
    if (itStart != itEnd)
    {
        --itEnd;
        LONG realSize = (LONG)std::distance(itStart, itEnd) + 1;
        actualSize = (std::min)(bufSize, realSize);
    }
    std::copy(itStart, itStart + actualSize, pInfo);
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoFloatEx(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo,
    LONG nWhichItem,
    LPDTWAIN_FLOAT pInfo,
    LONG bufSize)
{
    LOG_FUNC_ENTRY_PARAMS((OCRTextInfo, nWhichItem, pInfo, bufSize))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !pInfo; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);

    OCRCharacterInfo *cInfo = (OCRCharacterInfo*)OCRTextInfo;
    LONG realSize = (LONG)std::distance(cInfo->dConfidence.begin(), cInfo->dConfidence.end());
    LONG actualSize = (std::min)(bufSize, realSize);
    switch (nWhichItem)
    {
    case DTWAIN_OCRINFO_CHARCONFIDENCE:
        std::copy(cInfo->dConfidence.begin(), cInfo->dConfidence.begin() + actualSize, pInfo);
        break;

    default:
    {
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [] { return 1; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    }
    break;

    }
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}

LONG DLLENTRY_DEF DTWAIN_SetPDFOCRConversion(DTWAIN_OCRENGINE Engine,
    LONG PageType, LONG FileType,
    LONG PixelType, LONG BitDepth, LONG Options)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, PageType, FileType, PixelType, BitDepth))

        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, NULL, FUNC_MACRO);

    // check if PageType is OK
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !(PageType == 0 || PageType == 1); },
        DTWAIN_ERR_INVALID_PARAM, NULL, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    // Check if BW format, pixel type, and bit depth are supported
    LONG bRet = GetOCRTextSupport(pEngine, FileType, PixelType, BitDepth);
    if (bRet == 0)
    {
        pEngine->m_OCRPDFInfo.FileType[PageType] = FileType;
        pEngine->m_OCRPDFInfo.PixelType[PageType] = PixelType;
        pEngine->m_OCRPDFInfo.BitDepth[PageType] = BitDepth;
        pEngine->SetBaseOption(OCROPTION_STORECLEANTEXT1, Options & OCROPTION_STORECLEANTEXT1);
        LOG_FUNC_EXIT_PARAMS(1)
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
        CATCH_BLOCK(-1L)
}

LONG GetOCRTextSupport(OCREngine* pEngine, LONG fileType, LONG pixelType, LONG bitDepth)
{
    // Get file type support
    OCREngine::OCRLongArrayValues vals;
    bool bOK = pEngine->GetCapValues(DTWAIN_OCRCV_IMAGEFILEFORMAT, DTWAIN_CAPGET, vals);
    if (!bOK)
        return DTWAIN_ERR_OCR_INVALIDFILETYPE;
    if (vals.size() < 1)
        return DTWAIN_ERR_OCR_INVALIDFILETYPE;
    OCREngine::OCRLongArrayValues::iterator it = std::find(vals.begin(), vals.end(), fileType);
    if (it == vals.end())
        return DTWAIN_ERR_OCR_INVALIDFILETYPE;

    // File type exists, so see if pixel type exists
    bOK = pEngine->GetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPGET, vals);
    if (!bOK)
        return DTWAIN_ERR_OCR_INVALIDPIXELTYPE;
    if (vals.size() < 1)
        return DTWAIN_ERR_OCR_INVALIDPIXELTYPE;
    it = std::find(vals.begin(), vals.end(), pixelType);
    if (it == vals.end())
        return DTWAIN_ERR_OCR_INVALIDPIXELTYPE;

    // Now select the pixel type, but remember the last one to reset it.
    pEngine->GetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPGETCURRENT, vals);
    LONG lastPixelType = vals[0];
    vals[0] = pixelType;
    pEngine->SetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPSET, vals);

    // Get the bit depths for this type
    bOK = pEngine->GetCapValues(DTWAIN_OCRCV_BITDEPTH, DTWAIN_CAPGET, vals);
    LONG retVal = 0;
    if (!bOK || vals.size() < 1)
        retVal = DTWAIN_ERR_OCR_INVALIDBITDEPTH;
    if (retVal == 0)
    {
        OCREngine::OCRLongArrayValues::iterator it2 = std::find(vals.begin(), vals.end(), bitDepth);
        if (it2 == vals.end())
            retVal = DTWAIN_ERR_OCR_INVALIDBITDEPTH;
    }

    // reset pixel type
    vals.resize(1);
    vals[0] = lastPixelType;
    pEngine->SetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPSET, vals);
    return retVal;
}

LONG   DLLENTRY_DEF DTWAIN_GetOCRVersionInfo(DTWAIN_OCRENGINE Engine, LPTSTR buffer, LONG maxBufSize)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, buffer, maxBufSize))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;
    CTL_StringType sVersion = pEngine->GetOCRVersionInfo();
    int nSize = (int)sVersion.length();
    if (buffer == NULL || maxBufSize < 1)
        LOG_FUNC_EXIT_PARAMS(nSize + 1)
        LONG nCharsToCopy = maxBufSize - 1;
    if (nSize < nCharsToCopy)
        nCharsToCopy = nSize;
    std::copy(sVersion.begin(),
        sVersion.begin() + nCharsToCopy, buffer);
    buffer[nCharsToCopy] = _T('\0');
    LOG_FUNC_EXIT_PARAMS(nSize + 1)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumOCRSupportedCaps(DTWAIN_OCRENGINE Engine,
    LPDTWAIN_ARRAY SupportedCaps)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, SupportedCaps))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    OCREngine::OCRLongArrayValues vals;
    pEngine->GetSupportedCaps(vals);
    DTWAIN_ARRAY theArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, (LONG)vals.size());
    if (!theArray)
        LOG_FUNC_EXIT_PARAMS(false)
        auto& vValues = EnumeratorVector<LONG>(theArray);
    std::copy(vals.begin(), vals.end(), vValues.begin());
    *SupportedCaps = theArray;
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}

bool OCREngineExists(CTL_TwainDLLHandle* pHandle, OCREngine* pEngine)
{
    return
        std::find_if(pHandle->m_OCRInterfaceArray.begin(),
        pHandle->m_OCRInterfaceArray.end(),
        SmartPointerFinder<OCREnginePtr>(pEngine)) != pHandle->m_OCRInterfaceArray.end();
}

bool OCRIsActive(const OCREngine* pEngine)
{
    return pEngine->IsActivated();
}
void dynarithmic::LoadOCRInterfaces(CTL_TwainDLLHandle *pHandle)
{
    pHandle->m_OCRProdNameToEngine.clear();

    // Load the Transym Interface
    OCREnginePtr pInterface;
    pInterface = std::make_shared<TransymOCR>();
    if (pInterface->IsInitialized())
        pHandle->m_OCRInterfaceArray.push_back(pInterface);

    // Add other engines here.

    // Call virtuals to set up the product names.
    OCRInterfaceContainer::iterator it = pHandle->m_OCRInterfaceArray.begin();
    while (it != pHandle->m_OCRInterfaceArray.end())
    {
        (*it)->SetOCRVersionIdentity();
        pHandle->m_OCRProdNameToEngine.insert(make_pair((*it)->GetProductName(), *it));
        ++it;
    }

    // Set first OCR engine to be the default engine
    if (!pHandle->m_OCRInterfaceArray.empty())
        pHandle->m_pOCRDefaultEngine = *(pHandle->m_OCRInterfaceArray.begin());
}

void dynarithmic::UnloadOCRInterfaces(CTL_TwainDLLHandle *pHandle)
{
    pHandle->m_OCRInterfaceArray.clear();
}

DTWAIN_OCRENGINE DLLENTRY_DEF DTWAIN_SelectOCREngineByName(LPCTSTR lpszName)
{
    LOG_FUNC_ENTRY_PARAMS((lpszName))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);

    // Get the OCR engine associated with the name
    OCRProductNameToEngineMap::iterator it = pHandle->m_OCRProdNameToEngine.find(lpszName);
    OCREnginePtr SelectedEngine;
	DTWAIN_OCRENGINE ocrEngine_ = nullptr;
	if (it != pHandle->m_OCRProdNameToEngine.end())
	{
		SelectedEngine = it->second;
		if (SelectedEngine)
		{
			pHandle->m_pOCRDefaultEngine = SelectedEngine;

			if (!SelectedEngine->IsActivated())
				SelectedEngine->StartupOCREngine();
			ocrEngine_ = SelectedEngine.get();
		}
	}
    LOG_FUNC_EXIT_PARAMS(ocrEngine_)
    CATCH_BLOCK(DTWAIN_OCRENGINE())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_InitOCRInterface()
{
    LOG_FUNC_ENTRY_PARAMS(())
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    if (pHandle->m_OCRInterfaceArray.empty())
        LoadOCRInterfaces(pHandle);

    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumOCRInterfaces(LPDTWAIN_ARRAY OCRArray)
{
    LOG_FUNC_ENTRY_PARAMS((OCRArray))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    if (pHandle->m_OCRInterfaceArray.empty())
        *OCRArray = NULL;
    else
    {
        DTWAIN_ARRAY theArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYOCRENGINE, (LONG)pHandle->m_OCRInterfaceArray.size());
        DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !theArray; }, DTWAIN_ERR_OUT_OF_MEMORY, false, FUNC_MACRO);
        DTWAIN_OCRENGINE TempE;
        for (OCRInterfaceContainer::size_type i = 0; i < pHandle->m_OCRInterfaceArray.size(); ++i)
        {
            TempE = (DTWAIN_OCRENGINE)pHandle->m_OCRInterfaceArray[i].get();
            DTWAIN_ArraySetAt(theArray, (LONG)i, &TempE);
        }
        *OCRArray = theArray;
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ShutdownOCREngine(DTWAIN_OCRENGINE Engine)
{
    LOG_FUNC_ENTRY_PARAMS((Engine))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;
    int status;
    pEngine->ShutdownOCR(status);
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_StartupOCREngine(DTWAIN_OCRENGINE Engine)
{
    LOG_FUNC_ENTRY_PARAMS((Engine))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, false, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;
    //    int status;
    if (!pEngine->IsActivated())
    {
        LONG bRet = pEngine->StartupOCREngine();
        if (bRet != 0)
            LOG_FUNC_EXIT_PARAMS(false)
            LOG_FUNC_EXIT_PARAMS(true)
    }
    LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
}

DTWAIN_OCRENGINE DLLENTRY_DEF DTWAIN_SelectDefaultOCREngine()
{
    LOG_FUNC_ENTRY_PARAMS(())
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, NULL, FUNC_MACRO);

    // Get the OCR engine associated with the name
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return (pHandle->m_OCRInterfaceArray.empty()); },
        DTWAIN_ERR_OCR_NOTACTIVE, 0, FUNC_MACRO);
    DTWAIN_OCRENGINE SelectedEngine = (DTWAIN_OCRENGINE)pHandle->m_pOCRDefaultEngine.get();

    OCREngine *pEngine = (OCREngine*)SelectedEngine;
    if (!pEngine->IsActivated())
        pEngine->StartupOCREngine();

    LOG_FUNC_EXIT_PARAMS(SelectedEngine)
        CATCH_BLOCK(DTWAIN_OCRENGINE())
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsOCREngineActivated(DTWAIN_OCRENGINE Engine)
{
    LOG_FUNC_ENTRY_PARAMS((Engine))

        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, NULL, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    LONG retVal = FALSE;
    if (pEngine)
    {
        bool bRet = pEngine->IsActivated();
        retVal = bRet ? TRUE : FALSE;
    }
    LOG_FUNC_EXIT_PARAMS(retVal)
        CATCH_BLOCK(false)
}


LONG DLLENTRY_DEF DTWAIN_GetOCRLastError(DTWAIN_OCRENGINE Engine)
{
    LOG_FUNC_ENTRY_PARAMS((Engine))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, false, FUNC_MACRO);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, NULL, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;
    if (pEngine)
    {
        LONG bRet = pEngine->GetLastError();
        LOG_FUNC_EXIT_PARAMS(bRet)
    }
    LOG_FUNC_EXIT_PARAMS(0)
        CATCH_BLOCK(0)
}

LONG DLLENTRY_DEF DTWAIN_GetOCRErrorString(DTWAIN_OCRENGINE Engine, LONG lError, LPTSTR lpszBuffer, LONG nMaxLen)
{
    LOG_FUNC_ENTRY_PARAMS((Engine, lError, lpszBuffer, nMaxLen))
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex(pHandle, -1L, NULL);

    // check if Engine exists
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return !OCREngineExists(pHandle, (OCREngine*)Engine); },
        DTWAIN_ERR_OCR_INVALIDENGINE, NULL, FUNC_MACRO);

    OCREngine *pEngine = (OCREngine*)Engine;

    if (pEngine)
    {
        if (lError < 0)
        {
            // This is a DTWAIN error, not an OCR specific error
            LONG retval = DTWAIN_GetErrorString(lError, lpszBuffer, nMaxLen);
            LOG_FUNC_EXIT_PARAMS(retval)
        }
        LONG nTotalBytes = CopyInfoToCString(pEngine->GetErrorString(lError), lpszBuffer, nMaxLen);
        LOG_FUNC_EXIT_PARAMS(nTotalBytes)
    }
    LOG_FUNC_EXIT_PARAMS(-1)
        CATCH_BLOCK(-1)
}

#ifdef _WIN32

bool NewOCRJob(OCREngine *pEngine, LPCTSTR szFileName)
{
    CTL_StringType s1 = pEngine->GetCachedFile();
    CTL_StringType s2 = szFileName;
    s1 = StringWrapper::TrimAll(s1);
    s1 = StringWrapper::MakeLowerCase(s1);
    s2 = StringWrapper::TrimAll(s2);
    s2 = StringWrapper::MakeLowerCase(s2);
    return s1 != s2;
}

LRESULT CALLBACK DisplayOCRDlgProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);


DTWAIN_OCRENGINE DLLENTRY_DEF DTWAIN_SelectOCREngine()
{
    LOG_FUNC_ENTRY_PARAMS(())
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    // See if DLL Handle exists
    DTWAIN_Check_Bad_Handle_Ex( pHandle, NULL, FUNC_MACRO);

    // Get the resource for the Twain dialog
    HGLOBAL hglb = LoadResource(CTL_TwainDLLHandle::s_DLLInstance,
                               (HRSRC)FindResource(CTL_TwainDLLHandle::s_DLLInstance,
                                MAKEINTRESOURCE(10000), RT_DIALOG));
    DTWAIN_Check_Error_Condition_1_Ex(pHandle, [&] { return  !hglb;}, DTWAIN_ERR_NULL_WINDOW, NULL, FUNC_MACRO);

    LPDLGTEMPLATE lpTemplate = static_cast<LPDLGTEMPLATE>(LockResource(hglb));

    SelectStruct S;
    S.CS.xpos = 0;
    S.CS.ypos = 0;
    S.CS.nOptions = DTWAIN_DLG_CENTER_SCREEN;
    S.CS.hWndParent = NULL;
    S.nItems = 0;
    S.CS.sTitle = _T("Select OCR Engine");
    INT_PTR bRet = DialogBoxIndirectParam(CTL_TwainDLLHandle::s_DLLInstance,
                                      lpTemplate, NULL,
                                      reinterpret_cast<DLGPROC>(DisplayOCRDlgProc),
                                      reinterpret_cast<LPARAM>(&S));
    if ( bRet == -1 )
        LOG_FUNC_EXIT_PARAMS(0)

    // See if cancel was selected
    if ( S.SourceName.empty() || S.nItems == 0 )
        LOG_FUNC_EXIT_PARAMS(0)

    DTWAIN_OCRENGINE SelectedEngine = DTWAIN_SelectOCREngineByName(S.SourceName.c_str());
    LOG_FUNC_EXIT_PARAMS((DTWAIN_OCRENGINE)SelectedEngine)
    CATCH_BLOCK(DTWAIN_OCRENGINE())
}



/////////////////////////////////////////////////////////////////////////////////
/// DTWAIN OCR Dialog procedure
static void DisplayLocalString(HWND hWnd, int nID, int resID);

LRESULT CALLBACK DisplayOCRDlgProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    static HWND lstSources;
    static SelectStruct *pS;
    switch (message)
    {
    case WM_INITDIALOG:
        {
            pS = (SelectStruct*)lParam;

            if ( pS->CS.nOptions & DTWAIN_DLG_CENTER_SCREEN )
                CenterWindow(hWnd, NULL);
            else
            if ( pS->CS.nOptions & DTWAIN_DLG_CENTER)
                CenterWindow(hWnd, GetParent(hWnd));
            else
                ::SetWindowPos(hWnd, NULL, pS->CS.xpos, pS->CS.ypos, 0, 0, SWP_NOSIZE);

            lstSources = GetDlgItem(hWnd, IDC_LSTSOURCES);

            // Set the title
            ::SetWindowText(hWnd, pS->CS.sTitle.c_str());

            // Fill the list box with the sources
            DTWAIN_ARRAY Array = 0;
            DTWAIN_EnumOCRInterfaces(&Array);
            DTWAINArrayLL_RAII raii(Array);
            int nCount = DTWAIN_ArrayGetCount(Array);
            pS->nItems = nCount;
            if ( nCount == 0 )
            {
                HWND hWndSelect = GetDlgItem(hWnd, IDOK);
                if ( hWndSelect )
                    EnableWindow(hWndSelect, FALSE);
                return TRUE;
            }

            TCHAR DefName[255] = {0};
            std::vector<CTL_StringType> SourceNames;
            SourceNames.reserve(nCount);
            int i;
            DTWAIN_OCRENGINE TempOCR;
            for ( i = 0; i < nCount; i++ )
            {
                DTWAIN_ArrayGetAt(Array, i, &TempOCR);
                TCHAR ProdName[256];
                DTWAIN_GetOCRProductName(TempOCR, ProdName, 255);
                SourceNames.push_back(ProdName);
            }

            // Sort the names
            if ( pS->CS.nOptions & DTWAIN_DLG_SORTNAMES)
                sort(SourceNames.begin(), SourceNames.end());

            LRESULT index;
            LRESULT DefIndex = 0;
            for ( i = 0; i < nCount; i++ )
            {
                index = SendMessage(lstSources, LB_ADDSTRING, 0, (LPARAM)SourceNames[i].c_str());
                if ( lstrcmp(SourceNames[i].c_str(), (LPCTSTR)DefName) == 0)
                    DefIndex = index;
            }
            if ( i > 0 )
                SendMessage(lstSources, LB_SETCURSEL, DefIndex, 0);

            // Display the local strings if they are available:
            DisplayLocalString(hWnd, IDOK, IDS_SELECT_TEXT);
            DisplayLocalString(hWnd, IDCANCEL, IDS_CANCEL_TEXT);
            DisplayLocalString(hWnd, IDC_SOURCETEXT, IDS_SOURCES_TEXT);
            return TRUE;
        }

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK)
        {
            TCHAR sz[255];
            LRESULT nSel = SendMessage(lstSources, LB_GETCURSEL, 0, 0);
            SendMessage(lstSources, LB_GETTEXT, nSel, (LPARAM)sz);
            pS->SourceName = sz;
            EndDialog(hWnd, LOWORD(wParam));
            return TRUE;
        }
        else
            if (LOWORD(wParam) == IDCANCEL)
            {
                pS->SourceName = _T("");
                EndDialog(hWnd, LOWORD(wParam));
                return TRUE;
            }
            break;
    }
    return FALSE;
}

static CTL_StringType GetTwainDlgTextFromResource(int nID, int& status)
{
    TCHAR buffer[256];
    status = (int)GetResourceString(nID, buffer, 255);
    return buffer;
}

static void DisplayLocalString(HWND hWnd, int nID, int resID)
{
    CTL_StringType sText;
        int status;
        sText = GetTwainDlgTextFromResource(resID, status);
        if ( status )
        {
            HWND hWndControl = GetDlgItem(hWnd, nID);
            if ( hWndControl )
                SetWindowText(hWndControl, sText.c_str());
    }
}

#endif
