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
 #ifdef _MSC_VER
#pragma warning (disable : 4786)
#endif
#include <cstring>
#include <stdio.h>
#include <stdlib.h>
#include "tiffun32.h"
#include "pdflib32.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "ctlfileutils.h"
#include "tiff.h"

using namespace dynarithmic;

CTL_StringType CTIFFImageHandler::s_AppInfo;

CTL_String CTIFFImageHandler::GetFileExtension() const
{
    return "TIF";
}

HANDLE CTIFFImageHandler::GetFileInformation(LPCSTR /*path*/)
{
    return NULL;
}

void CTIFFImageHandler::SetMultiPageStatus(DibMultiPageStruct *pStruct)
{

    if ( pStruct )
        m_MultiPageStruct = *pStruct;
}


void CTIFFImageHandler::GetMultiPageStatus(DibMultiPageStruct *pStruct)
{
    *pStruct = m_MultiPageStruct;
}

bool CTIFFImageHandler::OpenOutputFile(LPCTSTR pFileName)
{
    StringTraits::StringCopy(m_FileName, pFileName);
    SetError(0);
    return true;
}

bool CTIFFImageHandler::CloseOutputFile()
{
    return true;
}

void CTIFFImageHandler::DestroyAllObjects()
{
    FIMULTIBITMAP *fp = reinterpret_cast<FIMULTIBITMAP*>(m_MultiPageStruct.pUserData);
    if ( fp )
        m_nError = FreeImage_CloseMultiBitmap(fp, 0)?0:1;
}

int CTIFFImageHandler::WriteGraphicFile(CTL_ImageIOHandler* ptrHandler, LPCTSTR path, HANDLE bitmap, void *pUserInfo/*=NULL*/)
{
    std::unordered_map<int, int> compressionFlags = {{COMPRESSION_PACKBITS, TIFF_PACKBITS},
                                                     {COMPRESSION_DEFLATE, TIFF_DEFLATE},
                                                     {COMPRESSION_NONE, TIFF_NONE},
                                                     {COMPRESSION_CCITTFAX3, TIFF_CCITTFAX3},
                                                     {COMPRESSION_CCITTFAX4, TIFF_CCITTFAX4},
                                                     {COMPRESSION_LZW, TIFF_LZW},
                                                     {COMPRESSION_JPEG, TIFF_JPEG}
    };

    // Check if this is first page of multi-page TIFF or
    // if only a single page TIFF
    if (m_MultiPageStruct.Stage == 0 || m_MultiPageStruct.Stage == DIB_MULTI_FIRST)
    {
		FIMULTIBITMAP *fp = nullptr;
		CTL_String fname = StringConversion::Convert_NativePtr_To_Ansi(path);
		{
			std::ofstream ofs(fname.c_str());
			if (!ofs)
				return DTWAIN_ERR_FILEOPEN;
		}
        if (m_MultiPageStruct.Stage != 0)
        {
		fp = FreeImage_OpenMultiBitmap(FIF_TIFF, fname.c_str(), true, false, false, 0);
        if ( !fp )
            return DTWAIN_ERR_FILEOPEN;
        }
        m_MultiPageStruct.pUserData = fp;
        }
        else
        if (m_MultiPageStruct.Stage == DIB_MULTI_LAST)
        {
            m_bWriteOk = TRUE;
        return DTWAIN_NO_ERROR;
    }

    FIMULTIBITMAP *fp = reinterpret_cast<FIMULTIBITMAP*>(m_MultiPageStruct.pUserData);
    fipImage im;
    fipImageUtility::copyFromHandle(im, bitmap);
    fipWinImage_RAII raii(&im);
    unsigned long compression;
    int retVal = ProcessCompressionType(im, compression);
    if (retVal != 0)
        return retVal;

    double factor = GetScaleFactorPerInch(m_ImageInfoEx.UnitOfMeasure);
    switch (m_ImageInfoEx.UnitOfMeasure)
    {
        case DTWAIN_CENTIMETERS:
            factor = 1.0;
    }
    im.setHorizontalResolution((m_ImageInfoEx.ResolutionX * factor) / 2.54);
    im.setVerticalResolution((m_ImageInfoEx.ResolutionY * factor) / 2.54);

    fipTag ft;
    ft.setKeyValue("Comment", StringConversion::Convert_Native_To_Ansi(dynarithmic::GetVersionString()).c_str());
    im.setMetadata(FIMD_COMMENTS, "Comment", ft);

    if (m_MultiPageStruct.Stage == 0)
    {
        auto retVal2 = im.save(FIF_TIFF, StringConversion::Convert_Native_To_Ansi(path).c_str(), compressionFlags[compression]);
        if (retVal2 == 1)
            return DTWAIN_NO_ERROR;
        return DTWAIN_ERR_FILEWRITE;
    }

    FreeImage_AppendPageEx(fp, im, compressionFlags[compression]);
    return 0;
}

int CTIFFImageHandler::ProcessCompressionType(fipImage& im, unsigned long& compression)
{
    long samplesperpixel = 0;
    long bitspersample = 0;
    long photometric = 0;
    compression = 0;
    bool bOk = false;
    auto bits = im.getBitsPerPixel();

    switch(bits)
    {
        case 1:
            samplesperpixel = 1;
            bitspersample = 1;
            photometric = PHOTOMETRIC_MINISWHITE;
            if ( m_nFormat == COMPRESSION_NONE ||
                 m_nFormat == COMPRESSION_LZW ||
                 m_nFormat == COMPRESSION_PACKBITS ||
                 m_nFormat == COMPRESSION_CCITTRLE  ||
                 m_nFormat == COMPRESSION_CCITTFAX3 ||
                 m_nFormat == COMPRESSION_CCITTFAX4 ||
                 m_nFormat == COMPRESSION_JBIG ||
                 m_nFormat == COMPRESSION_PIXARLOG ||
                 m_nFormat == COMPRESSION_DEFLATE  )
            {
                compression = m_nFormat;
                bOk = true;
            }
        break;
        case 4:
        case 8:
        case 14:
        case 16:
            samplesperpixel = 1;
            bitspersample = bits;
            photometric = PHOTOMETRIC_PALETTE;
            if ( m_nFormat == COMPRESSION_NONE ||
                 m_nFormat == COMPRESSION_LZW ||
                 m_nFormat == COMPRESSION_PACKBITS ||
                 m_nFormat == COMPRESSION_JPEG ||
                 m_nFormat == COMPRESSION_DEFLATE ||
                 m_nFormat == COMPRESSION_JBIG ||
                 m_nFormat == COMPRESSION_PIXARLOG)
            {
                compression = m_nFormat;
                bOk = true;
            }

            if (m_nFormat == COMPRESSION_JPEG)
                photometric = PHOTOMETRIC_RGB;
            break;
        case 24:
        case 32:
            samplesperpixel = 3;
            bitspersample = 8;
            photometric = PHOTOMETRIC_RGB;
            if ( m_nFormat == COMPRESSION_NONE ||
                 m_nFormat == COMPRESSION_LZW ||
                 m_nFormat == COMPRESSION_PACKBITS ||
                 m_nFormat == COMPRESSION_JPEG ||
                 m_nFormat == COMPRESSION_JBIG ||
                 m_nFormat == COMPRESSION_PIXARLOG ||
                 m_nFormat == COMPRESSION_DEFLATE )
            {
                compression = m_nFormat;
                bOk = true;
            }
            break;
    }
    if ( !bOk )
        return DTWAIN_ERR_BADBITSPERPIXEL;
    return 0;
}

int CTIFFImageHandler::WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE *pImage2, UINT32 wid, UINT32 ht,
                                  UINT32 bpp, UINT32 /*nColors*/, RGBQUAD * /*pPal*/, void * /*pUserInfo*/)
{
    DestroyAllObjects();
    return m_nError;
}

int CTIFFImageHandler::Tiff2PS(LPCTSTR szFileIn, LPCTSTR szFileOut, LONG PSType,
                               LPCTSTR szTitle, bool PSEncapsulated)
{
    return DTWLIB_PSWriteFile(szFileIn, szFileOut, PSType, szTitle, PSEncapsulated);
}