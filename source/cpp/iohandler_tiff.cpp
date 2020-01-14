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
#include <sstream>
#include "ctldib.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "ctlfileutils.h"
#include "tiff.h"
using namespace dynarithmic;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
int CTL_TiffIOHandler::WriteBitmap(LPCTSTR szFile, bool /*bOpenFile*/, int /*fhFile*/, LONG64 MultiStage)
{
    DibMultiPageStruct *s = (DibMultiPageStruct *)MultiStage;
    HANDLE hDib = NULL;

    // Check if this is the first page
    CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Writing TIFF or Postscript file\n")));

    // Get the current TIFF type from the Source
    if ( m_ImageInfoEx.theSource &&
        !m_ImageInfoEx.IsPDF &&
        !m_ImageInfoEx.IsPostscript &&
        !m_ImageInfoEx.IsPostscriptMultipage &&
        !m_ImageInfoEx.IsOCRTempImage)
        m_nFormat = m_ImageInfoEx.theSource->GetAcquireFileType();

    if ( !s || s->Stage == DIB_MULTI_FIRST )
    {
        // Check for the Postscript option
        if ( m_ImageInfoEx.IsPostscript )
        {
            CTL_StringType szTempPath;
            // This is a postscript save, so
            // create a temp file
            szTempPath = GetDTWAINTempFilePath();
            if ( szTempPath.empty() )
                return DTWAIN_ERR_FILEWRITE;
            szTempPath += StringWrapper::GetGUID() +  _T("TIF");

            CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Temporary Image File is ")) + szTempPath + CTL_StringType(_T("\n")));

            // OK, now remember that the file we are writing is a TIF file, and this is
            // the file that is created first
            sActualFileName = szTempPath;
            sPostscriptName = szFile;
        }
        else
        {
            // Just a normal TIFF file, no Postscript
            sActualFileName = szFile;

            // Attempt to delete the file
            if ( !delete_file(sActualFileName.c_str()) )
                CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Could not delete existing file ")) + sActualFileName.c_str() + CTL_StringType(_T("\n")));
        }
    }

    bool bNotLastFile = false;
    if ( !s )
        bNotLastFile = true;
    else
        if ( s->Stage != DIB_MULTI_LAST )
            bNotLastFile = true;
    if ( bNotLastFile ) //!s || (s && s->Stage != DIB_MULTI_LAST ))
    {
        CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Retrieving DIB:\n")));
        if ( !m_pDib )
        {
            CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Dib not found!\n")));
            return DTWAIN_ERR_DIB;
        }
        hDib = m_pDib->GetHandle();
        if ( !hDib )
        {
            CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Dib handle not found!\n")));
            return DTWAIN_ERR_DIB;
        }
    }
    //    int nRes = m_pDib->GetResolution();
    int nLibTiff;
    switch( m_nFormat )
    {
    case CTL_TwainDib::TiffFormatLZW:
    case CTL_TwainDib::TiffFormatLZWMULTI:
        nLibTiff = COMPRESSION_LZW;  // LZW compression is suspended
        //            nLibTiff = COMPRESSION_NONE;
        break;

    case CTL_TwainDib::TiffFormatNONE:
    case CTL_TwainDib::TiffFormatNONEMULTI:
        nLibTiff = COMPRESSION_NONE;
        break;

    case CTL_TwainDib::TiffFormatGROUP3:
    case CTL_TwainDib::TiffFormatGROUP3MULTI:
        nLibTiff = COMPRESSION_CCITTFAX3;
        break;

    case CTL_TwainDib::TiffFormatGROUP4:
    case CTL_TwainDib::TiffFormatGROUP4MULTI:
        nLibTiff = COMPRESSION_CCITTFAX4;
        break;

    case CTL_TwainDib::TiffFormatPACKBITS:
    case CTL_TwainDib::TiffFormatPACKBITSMULTI:
        nLibTiff = COMPRESSION_PACKBITS;
        break;

    case CTL_TwainDib::TiffFormatDEFLATE:
    case CTL_TwainDib::TiffFormatDEFLATEMULTI:
        nLibTiff = COMPRESSION_DEFLATE;
        break;

    case CTL_TwainDib::TiffFormatJPEG:
    case CTL_TwainDib::TiffFormatJPEGMULTI:
        nLibTiff = COMPRESSION_JPEG;
        break;

    case CTL_TwainDib::TiffFormatPIXARLOG:
    case CTL_TwainDib::TiffFormatPIXARLOGMULTI:
        nLibTiff = COMPRESSION_PIXARLOG;
        break;

    default:
            return DTWAIN_ERR_INVALID_BITDEPTH;
    }

    if ( !IsValidBitDepth(m_nFormat, m_pDib->GetBitsPerPixel()) )
        return DTWAIN_ERR_INVALID_BITDEPTH;

    CTIFFImageHandler TIFFHandler(nLibTiff, m_ImageInfoEx);

    if ( MultiStage )
    {
        TIFFHandler.SetMultiPageStatus(s);
    }
    int retval;
    if ( bNotLastFile )
    {
        SetNumPagesWritten(GetNumPagesWritten()+1);
        CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Writing TIFF / PS page\n")));
        retval = TIFFHandler.WriteGraphicFile(this, sActualFileName.c_str(), hDib);
        if ( retval != 0 )
            SetPagesOK(false);
        else
            SetOnePageWritten(true);
        CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Writing TIFF / PS page\n")));
        CTL_StringStreamType strm;
        strm << _T("Return from writing intermediate image = ") << retval << _T("\n");
        CTL_TwainAppMgr::WriteLogInfo(strm.str());
    }
    else
    {
        // Close the multi-page TIFF file
        CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Closing TIFF / PS file\n")));
        retval = TIFFHandler.WriteImage(this,0,0,0,0,0,NULL);
        if ( !AllPagesOK() )
        {
            if ( !IsOnePageWritten() )
            {
                delete_file( sActualFileName.c_str() );
                retval = DTWAIN_ERR_FILEXFERSTART - DTWAIN_ERR_FILEWRITE;
            }
        }
        CTL_StringStreamType strm;
        strm << _T("Return from writing last image = ") << retval << _T("\n");
        CTL_TwainAppMgr::WriteLogInfo(strm.str());
    }
    if ( (!s || s->Stage == DIB_MULTI_LAST) && (retval == 0) )
    {
        // Convert the TIFF file to Postscript if necessary
        if ( m_ImageInfoEx.IsPostscript )
        {
            // This will have to call the routine to convert
            LONG Level;
            switch(m_ImageInfoEx.PostscriptType)
            {
            case DTWAIN_POSTSCRIPT1:
            case DTWAIN_POSTSCRIPT1MULTI:
                Level = 1;
                break;

            case DTWAIN_POSTSCRIPT2:
            case DTWAIN_POSTSCRIPT2MULTI:
                Level = 2;
                break;
            default:
                Level = 3;
                break;
            }
            CTL_StringType sTitle;
            sTitle = m_ImageInfoEx.PSTitle;
            if ( sTitle.empty() )
                sTitle = _T("DTWAIN Postscript");
            retval = TIFFHandler.Tiff2PS(sActualFileName.c_str(), sPostscriptName.c_str(),Level,
                sTitle.c_str(), m_ImageInfoEx.PSType==DTWAIN_PS_ENCAPSULATED);

            if ( retval == -1 )
                retval = DTWAIN_ERR_FILEWRITE;
            delete_file( sActualFileName.c_str());
        }
    }
    if ( s )
        TIFFHandler.GetMultiPageStatus(s);

    return retval;
}
