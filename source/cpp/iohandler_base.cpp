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
#include "ctldib.h"
#include "ctliface.h"
#include "ctltwmgr.h"

using namespace dynarithmic;

std::unordered_map<LONG, std::vector<int>> CTL_ImageIOHandler::s_supportedBitDepths;

CTL_ImageIOHandler::CTL_ImageIOHandler() : pMultiDibData(NULL), m_nPage(0),
m_bAllWritten(true), m_bOnePageWritten(false), bytesleft(0), nextbyte(0), bytebuffer{}, bittable{}, masktable{}
{
    m_pDib = NULL;

}

CTL_ImageIOHandler::CTL_ImageIOHandler( CTL_TwainDib *pDib ): pMultiDibData(NULL), m_nPage(0),
m_bAllWritten(true), m_bOnePageWritten(false), bytesleft(0), nextbyte(0), bytebuffer{}, bittable{}, masktable{}
{
    m_pDib = pDib;
}

CTL_ImageIOHandler::~CTL_ImageIOHandler()
{
}

void CTL_ImageIOHandler::SetMultiDibInfo(const DibMultiPageStruct &s)
{
    m_DibMultiPageStruct = s;
}

DibMultiPageStruct CTL_ImageIOHandler::GetMultiDibInfo() const
{
    return m_DibMultiPageStruct;
}

void CTL_ImageIOHandler::resetbuffer()
{
    bytesleft=0;
}

bool CTL_ImageIOHandler::IsValidBitDepth(LONG FileType, LONG bitDepth)
{
    auto it = s_supportedBitDepths.find(FileType);
    if (it != s_supportedBitDepths.end())
    {
        auto it2 = std::find(it->second.begin(), it->second.end(), bitDepth);
        if (it2 == it->second.end())
            return false;
    }
    return true;
}

int CTL_ImageIOHandler::SaveToFile(HANDLE hDib, LPCTSTR szFile, FREE_IMAGE_FORMAT fmt, int flags,
                                   UINT unitOfMeasure, const std::pair<LONG, LONG>& res, 
									const std::tuple<double, double, double, double>& multiplier_pr)
{
    #ifdef _WIN32
    fipImage fw;
    if (!fipImageUtility::copyFromHandle(fw, hDib))
        return 1;
    fipWinImage_RAII raii(&fw);
    #else
        fipImage fw;
        fipMemoryIO memIO((BYTE *)hDib, 0);
    fw.loadFromMemory(FIF_TIFF, memIO, flags);
    #endif

    double multiplier = 39.37 * std::get<0>(multiplier_pr);
    if (unitOfMeasure == DTWAIN_CENTIMETERS)
        multiplier = 100.0 * std::get<1>(multiplier_pr);

    fw.setHorizontalResolution(res.first * multiplier + std::get<2>(multiplier_pr));
    fw.setVerticalResolution(res.second * multiplier + std::get<3>(multiplier_pr));

    fipTag fp;
    fp.setKeyValue("Comment", StringConversion::Convert_Native_To_Ansi(dynarithmic::GetVersionString()).c_str());
    fw.setMetadata(FIMD_COMMENTS, "Comment", fp);
    return fw.save(fmt, StringConversion::Convert_Native_To_Ansi(szFile).c_str(), flags) ? 0 : 1;
}
