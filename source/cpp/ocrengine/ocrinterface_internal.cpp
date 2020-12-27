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
#include "OCRInterface.h"
using namespace dynarithmic;

OCRCapInfo& OCREngine::GetOCRCapInfo(LONG nCap) { return m_AllCapValues[nCap]; }

OCREngine::OCREngine() : m_nLastOCRError(0), m_OCRIdentity{}, m_nCurrentPage(0), m_bIsActivated(false)
{
    m_mapOperations[DTWAIN_CAPGET] = DTWAIN_CO_GET;
    m_mapOperations[DTWAIN_CAPGETDEFAULT] = DTWAIN_CO_GETDEFAULT;
    m_mapOperations[DTWAIN_CAPGETCURRENT] = DTWAIN_CO_GETCURRENT;
    m_mapOperations[DTWAIN_CAPSET] = DTWAIN_CO_SET;
    m_mapOperations[DTWAIN_CAPRESET] = DTWAIN_CO_RESET;
}

OCREngine::~OCREngine() {}
bool OCREngine::IsInitialized() const { return false; }
bool OCREngine::SetOptions(OCRJobOptions& /*options*/) { return false; }
LONG OCREngine::StartOCR() { return false; }
LONG OCREngine::StartOCR(const CTL_StringType& /*filename*/) {return false;}
LONG OCREngine::GetLastError() const { return m_nLastOCRError; }
CTL_StringType OCREngine::GetErrorString(LONG /*errCode*/) { return _T(""); }
void OCREngine::SetOkErrorCode() { m_nLastOCRError = 0; }
void OCREngine::SetLastError(LONG errCode) { m_nLastOCRError = errCode; }
bool OCREngine::IsReturnCodeOk(LONG /*returnCode*/) { return true; }
std::string OCREngine::GetReturnCodeString(LONG /*returnCode*/) { return ""; }
bool OCREngine::SetFileType() { return false; }
bool OCREngine::SetFileName() { return false;}
CTL_StringType OCREngine::GetOCRVersionInfo() { return _T("Unknown OCR Engine"); }
bool OCREngine::SetOCRVersionIdentity(const OCRVersionIdentity& vIdentity) { m_OCRIdentity = vIdentity; return true;}
bool OCREngine::SetOCRVersionIdentity() { return false; }
CTL_StringType OCREngine::GetManufacturer() const { return m_OCRIdentity.Manufacturer;}
CTL_StringType OCREngine::GetProductFamily() const { return m_OCRIdentity.ProductFamily;}
CTL_StringType OCREngine::GetProductName() const { return m_OCRIdentity.ProductName;}
bool OCREngine::ShutdownOCR(int&) { return true; }
bool OCREngine::IsActivated() const { return m_bIsActivated; }
void OCREngine::SetActivated(bool bActive) { m_bIsActivated = bActive; }
LONG OCREngine::StartupOCREngine() { return 0; }

CTL_StringType OCREngine::GetCachedFile() const { return m_OCRCache.sOCRFileName; }
std::string OCREngine::GetCachedText() const { return m_OCRCache.sOCRText; }

void OCREngine::AddCharacterInfo(LONG nPage, const OCRCharacterInfo& cInfo)
{
    OCRCharacterInfoMap::iterator itMap = m_OCRCharMap.find(nPage);
    if ( itMap == m_OCRCharMap.end() )
        itMap = m_OCRCharMap.insert(make_pair(nPage, std::vector<OCRCharacterInfo>())).first;
    itMap->second.push_back(cInfo);
}

void OCREngine::SetBaseOption(int nWhichOption, bool bSet/*=true*/)
{
    if ( bSet)
        m_baseOptions.set(nWhichOption);
    else
        m_baseOptions.reset(nWhichOption);
}

void OCREngine::SetCachedFile(const CTL_StringType& filename) { m_OCRCache.sOCRFileName = filename; }
void OCREngine::SetCachedText(const std::string& sText) { m_OCRCache.sOCRText = sText; }

    // OCR capability functions similar to TWAIN protocol
bool OCREngine::GetCapValues(LONG nOCRCap, LONG CapType, OCRLongArrayValues& vals)
{
    auto it = m_AllCapValues.find(nOCRCap);
    if ( it != m_AllCapValues.end())
    {
        OCRCapInfo &CapInfo = it->second;
        if ( CapInfo.GetCapDataType() == DTWAIN_ARRAYLONG )
        {
            // Check if operation exists
            auto itOp = m_mapOperations.find(CapType);
            if ( itOp != m_mapOperations.end())
            {
                // Now check if cap supports these operations
                if (CapInfo.GetCapOperations() & itOp->second)
                {
                    // Cap is supported, and supports the operation
                    // so now call the process function
                    return ProcessGetCapValues(nOCRCap, CapType, vals);
                }
            }
        }
    }
    return false;
}

bool OCREngine::GetCapValues(LONG nOCRCap, LONG CapType, OCRStringArrayValues& vals)
{
    auto it = m_AllCapValues.find(nOCRCap);
    if ( it != m_AllCapValues.end())
    {
        OCRCapInfo &CapInfo = it->second;
        if ( CapInfo.GetCapDataType() == DTWAIN_ARRAYSTRING )
        {
            // Check if operation exists
            auto itOp = m_mapOperations.find(CapType);
            if ( itOp != m_mapOperations.end())
            {
                // Now check if cap supports these operations
                if (CapInfo.GetCapOperations() & itOp->second)
                {
                    // Cap is supported, and supports the operation
                    // so now call the process function
                    return ProcessGetCapValues(nOCRCap, CapType, vals);
                }
            }
        }
    }
    return false;
}


bool OCREngine::SetCapValues(LONG nOCRCap, LONG CapType, const OCRLongArrayValues& vals)
{
    auto it = m_AllCapValues.find(nOCRCap);
    if ( it != m_AllCapValues.end())
    {
        OCRCapInfo &CapInfo = it->second;
        if ( CapInfo.GetCapDataType() == DTWAIN_ARRAYLONG )
        {
            // Check if operation exists
            auto itOp = m_mapOperations.find(CapType);
            if ( itOp != m_mapOperations.end())
            {
                // Now check if cap supports these operations
                if (CapInfo.GetCapOperations() & itOp->second)
                {
                    // Cap is supported, and supports the operation
                    // so now call the process function
                    return ProcessSetCapValues(nOCRCap, CapType, vals);
                }
            }
        }
    }
    return false;
}

bool OCREngine::SetCapValues(LONG nOCRCap, LONG CapType, const OCRStringArrayValues& vals)
{
    auto it = m_AllCapValues.find(nOCRCap);
    if ( it != m_AllCapValues.end())
    {
        OCRCapInfo &CapInfo = it->second;
        if ( CapInfo.GetCapDataType() == DTWAIN_ARRAYSTRING )
        {
            // Check if operation exists
            auto itOp = m_mapOperations.find(CapType);
            if ( itOp != m_mapOperations.end())
            {
                // Now check if cap supports these operations
                if (CapInfo.GetCapOperations() & itOp->second)
                {
                    // Cap is supported, and supports the operation
                    // so now call the process function
                    return ProcessSetCapValues(nOCRCap, CapType, vals);
                }
            }
        }
    }
    return false;
}

bool OCREngine::IsCapSupported(LONG nOCRCap) const
{
    return m_AllCapValues.find(nOCRCap) != m_AllCapValues.end();
}

bool OCREngine::SetAllFileTypes(const FileTypeArray& allTypes)
{
    m_fileTypes = allTypes;
    return true;
}

OCREngine::FileTypeArray OCREngine::GetAllFileTypes() const { return m_fileTypes; }
void OCREngine::AddCapValue(LONG nCap, const OCRCapInfo& capInfo)
{
    m_AllCapValues.insert(std::make_pair(nCap, capInfo));
}

LONG OCREngine::GetCapDataType(LONG nOCRCap) const
{
    auto it = m_AllCapValues.find(nOCRCap);
    if ( it != m_AllCapValues.end())
    {
        const OCRCapInfo &CapInfo = it->second;
        return CapInfo.GetCapDataType();
    }
    return -1;
}

bool OCREngine::GetSupportedCaps(OCRLongArrayValues& vals) const
{
    vals.resize(0);
    auto it = m_AllCapValues.begin();
    while (it != m_AllCapValues.end())
    {
        vals.push_back(it->first);
        ++it;
    }
    return true;
}

bool OCREngine::GetBaseOption(int nWhichOption) const
{
    return m_baseOptions.test(nWhichOption);
}

bool OCREngine::ProcessGetCapValues(LONG, LONG, OCRLongArrayValues&) { return false; }
bool OCREngine::ProcessGetCapValues(LONG, LONG, OCRStringArrayValues&) { return false; }
bool OCREngine::ProcessSetCapValues(LONG, LONG, const OCRLongArrayValues&) { return false; }
bool OCREngine::ProcessSetCapValues(LONG, LONG, const OCRStringArrayValues&) { return false; }
void OCREngine::ClearCharacterInfoMap() { m_OCRCharMap.clear(); }
void OCREngine::SetCurrentPageNumber(int PageNo) { m_nCurrentPage = PageNo; }
int OCREngine::GetCurrentPageNumber() const { return m_nCurrentPage; }

std::vector<OCRCharacterInfo>& OCREngine::GetCharacterInfo(LONG nPage, int& status)
{
    OCRCharacterInfoMap::iterator it = m_OCRCharMap.find(nPage);
    status = -1;
    if ( it != m_OCRCharMap.end())
    {
        status = 0;
        return it->second;
    }
    return m_InvalidOCRCharInfo;
}

void OCREngine::SetPageTextMap(LONG nPage, const CTL_StringType& sData)
{
    m_OCRPageTextMap[nPage] = sData;
}

CTL_StringType OCREngine::GetOCRText(LONG nPage)
{
    OCRPageTextMap::iterator it = m_OCRPageTextMap.find(nPage);
    if ( it != m_OCRPageTextMap.end())
        return it->second;
    return _T("");
}

bool OCREngine::IsValidOCRPage(LONG nPage) const
{
    return
        m_OCRPageTextMap.find(nPage) != m_OCRPageTextMap.end();
}

bool OCREngine::SetPDFFileTypes(OCRPDFInfo::enumPDFColorType nWhich, LONG fileType, LONG pixelType, LONG bitDepth)
{
    m_OCRPDFInfo.FileType[nWhich] = fileType;
    m_OCRPDFInfo.BitDepth[nWhich] = bitDepth;
    m_OCRPDFInfo.PixelType[nWhich] = pixelType;
    return true;
}
