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
#ifdef _MSC_VER
#pragma warning (disable:4786)
#endif
#include "pdffun32.h"
#include <limits.h>
#undef min
#undef max
#include <sstream>
#include <sstream>
#include <string>
#include <cmath>
#include <pdflib32.h>
#include "ctliface.h"
#include "ctltwmgr.h"
#include "ctlfileutils.h"

#define FLOAT_DELTA  (+1.0e-8)
#define FLOAT_CLOSE(x,y) (fabs((x) - (y)) <= FLOAT_DELTA)

using namespace std;
using namespace dynarithmic;

bool CPDFImageHandler::s_bLibraryLoaded=false;
PDF_FUNC1  CPDFImageHandler::m_pPDFGetNewDocument  =NULL;
PDF_FUNC2  CPDFImageHandler::m_pPDFOpenNewFile     =NULL;
PDF_FUNC3  CPDFImageHandler::m_pPDFSetCompression  =NULL;
PDF_FUNC4  CPDFImageHandler::m_pPDFSetNameField    =NULL;
PDF_FUNC5  CPDFImageHandler::m_pPDFStartCreation   =NULL;
PDF_FUNC6  CPDFImageHandler::m_pPDFEndCreation     =NULL;
PDF_FUNC7  CPDFImageHandler::m_pPDFSetImageType    =NULL;
PDF_FUNC8  CPDFImageHandler::m_pPDFSetLongField    =NULL;
PDF_FUNC9  CPDFImageHandler::m_pPDFWritePage       =NULL;
PDF_FUNC10 CPDFImageHandler::m_pPDFSetScaling      =NULL;
PDF_FUNC11 CPDFImageHandler::m_pPDFReleaseDocument =NULL;
PDF_FUNC12 CPDFImageHandler::m_pPDFSetThumbnailFile=NULL;
PDF_FUNC13 CPDFImageHandler::m_pPDFSetDPI          =NULL;
PDF_FUNC14 CPDFImageHandler::m_pPDFSetEncryption   =NULL;
PDF_FUNC15 CPDFImageHandler::m_pPDFSetASCIICompression   =NULL;
PDF_FUNC16 CPDFImageHandler::m_pPDFSetSearchableText = NULL;
PDF_FUNC17 CPDFImageHandler::m_pPDFAddPageText = NULL;
PDF_FUNC18 CPDFImageHandler::m_pPDFSetPolarity = NULL;
PDF_FUNC19 CPDFImageHandler::m_pPDFSetNoCompression = NULL;

CPDFImageHandler::CPDFImageHandler(const CTL_StringType& sFileName, DTWAINImageInfoEx &ImageInfoEx) :
        CDibInterface(), m_ImageInfoEx(ImageInfoEx),
        m_sFileName(sFileName),
        m_sAuthor("(None)"),
        m_sProducer("(None)"),
        m_sTitle("(None)"),
        m_sSubject("(None)"),
        m_sKeywords("(None)"),
        m_sCreator("(None)"),
		m_nImageType(0)
{
    LoadPDFLibrary();
}

bool CPDFImageHandler::LoadPDFLibrary()
{
    m_pPDFGetNewDocument      =   DTWLIB_PDFGetNewDocument;
    m_pPDFOpenNewFile         =   DTWLIB_PDFOpenNewFile;
    m_pPDFSetCompression      =   DTWLIB_PDFSetCompression;
    m_pPDFSetNameField        =   DTWLIB_PDFSetNameField;
    m_pPDFStartCreation       =   DTWLIB_PDFStartCreation;
    m_pPDFEndCreation         =   DTWLIB_PDFEndCreation;
    m_pPDFSetImageType        =   DTWLIB_PDFSetImageType;
    m_pPDFSetLongField        =   DTWLIB_PDFSetLongField;
    m_pPDFWritePage           =   DTWLIB_PDFWritePage;
    m_pPDFSetScaling          =   DTWLIB_PDFSetScaling;
    m_pPDFReleaseDocument     =   DTWLIB_PDFReleaseDocument;
    m_pPDFSetThumbnailFile    =   DTWLIB_PDFSetThumbnailFile;
    m_pPDFSetDPI              =   DTWLIB_PDFSetDPI;
    m_pPDFSetEncryption       =   DTWLIB_PDFSetEncryption;
    m_pPDFSetASCIICompression =   DTWLIB_PDFSetASCIICompression;
	m_pPDFSetNoCompression 	  =	  DTWLIB_PDFSetNoCompression;
    m_pPDFSetSearchableText   =   DTWLIB_PDFSetSearchableText;
    m_pPDFAddPageText         =   DTWLIB_PDFAddPageText;
    m_pPDFSetPolarity         =   DTWLIB_PDFSetPolarity;

    m_nError = 0;
    s_bLibraryLoaded = true;
    return true;
}



CTL_String CPDFImageHandler::GetFileExtension() const
{
    return "PDF";
}

HANDLE CPDFImageHandler::GetFileInformation(LPCSTR /*path*/)
{
    return NULL;
}

bool CPDFImageHandler::OpenOutputFile(LPCTSTR /*pFileName*/)
{
    if ( !s_bLibraryLoaded )
    {
        CTL_TwainAppMgr::SetError( m_nError);
        return false;
    }
    return true;
}

int CPDFImageHandler::WriteGraphicFile(CTL_ImageIOHandler* ptrHandler, LPCTSTR path, HANDLE bitmap, void *pUserInfo)
{
    int retval = 0;
    if ( !s_bLibraryLoaded )
    {
        CTL_TwainAppMgr::SetError(m_nError);
        return m_nError;
    }

    PDFINFO *pPDFInfo = NULL;
    PdfDocumentPtr pDocument;


    if ( m_MultiPageStruct.Stage == DIB_MULTI_FIRST ||
         m_MultiPageStruct.Stage == 0 )
    {
        pPDFInfo = new PDFINFO;
        pPDFInfo->ImageInfoEx = m_ImageInfoEx;
        m_MultiPageStruct.pUserData = pPDFInfo;

        // Open the file, return if there is an error
        pDocument = m_pPDFGetNewDocument();

        if ( !pDocument || !m_pPDFOpenNewFile(pDocument, m_sFileName.c_str()) )
        {
            pPDFInfo->IsFileOpened = false;
            pPDFInfo->IsPDFStarted = false;
            if ( pDocument )
                m_pPDFReleaseDocument(pDocument);
            pPDFInfo->pPDFdoc = NULL;
            return DTWAIN_ERR_FILEWRITE;
        }

        pPDFInfo->IsFileOpened = true;
        pPDFInfo->nCurrentPage  = 1;
        m_MultiPageStruct.pUserData = pPDFInfo;
        pPDFInfo->sFileName     = m_sFileName;


        m_pPDFSetCompression(pDocument, false);
		m_pPDFSetNoCompression(pDocument, false);

        // Set the ASCII Hex compression
        m_pPDFSetASCIICompression(pDocument, pPDFInfo->ImageInfoEx.PDFUseASCIICompression);

		// turn on other compression flags in the PDF object
        if ( pPDFInfo->ImageInfoEx.PDFUseCompression )
			m_pPDFSetCompression(pDocument, true);   // Use Flate compression
		else
			m_pPDFSetNoCompression(pDocument, true); // Use no compression 

        m_pPDFSetNameField(pDocument, PDF_AUTHOR, m_sAuthor.c_str());
        m_pPDFSetNameField(pDocument, PDF_PRODUCER, m_sProducer.c_str());
        m_pPDFSetNameField(pDocument, PDF_TITLE, m_sTitle.c_str());
        m_pPDFSetNameField(pDocument, PDF_KEYWORDS, m_sKeywords.c_str());
        m_pPDFSetNameField(pDocument, PDF_SUBJECT, m_sSubject.c_str());
        m_pPDFSetNameField(pDocument, PDF_CREATOR, m_sCreator.c_str());

        if ( !m_pPDFStartCreation(pDocument) )
        {
            pPDFInfo->IsPDFStarted = false;
            m_pPDFReleaseDocument (pDocument);
            return DTWAIN_ERR_FILEWRITE;
        }

        m_pPDFSetPolarity(pDocument, pPDFInfo->ImageInfoEx.nPDFPolarity);
        // Test the encryption here
        if ( pPDFInfo->ImageInfoEx.bIsPDFEncrypted)
        {
            if ( m_pPDFSetEncryption )
            {
                m_pPDFSetEncryption(pDocument,
                                    pPDFInfo->ImageInfoEx.PDFOwnerPassword.c_str(),
                                    pPDFInfo->ImageInfoEx.PDFUserPassword.c_str(),
                                    pPDFInfo->ImageInfoEx.PDFPermissions,
                                    pPDFInfo->ImageInfoEx.bUseStrongEncryption?TRUE:false,
                                    pPDFInfo->ImageInfoEx.bIsAESEncrypted?TRUE:FALSE);
            }
        }

        pPDFInfo->IsPDFStarted = true;

        pPDFInfo->pPDFdoc = pDocument;
    }
    else
    if ( m_MultiPageStruct.Stage == DIB_MULTI_LAST )
    {
        pPDFInfo = (PDFINFO *)m_MultiPageStruct.pUserData;
        if ( 1 ) //pPDFInfo->IsFileOpened && pPDFInfo->IsPDFStarted )
        {
            try
            {
                if ( pPDFInfo->IsPDFStarted )
                    m_pPDFEndCreation(pPDFInfo->pPDFdoc);
                if ( pPDFInfo->pPDFdoc )
                {
                    m_pPDFReleaseDocument(pPDFInfo->pPDFdoc);
                    pPDFInfo->pPDFdoc = NULL;
                }

                retval = 0;
                if ( !pPDFInfo->IsPDFStarted ||
                     !pPDFInfo->IsFileOpened )
                     retval = DTWAIN_ERR_FILEWRITE;
                delete pPDFInfo;
                pPDFInfo = NULL;
                return retval;
            }
            catch(...)
            {
                if ( pPDFInfo->IsPDFStarted )
                    m_pPDFEndCreation(pPDFInfo->pPDFdoc);
                if ( pPDFInfo->pPDFdoc )
                {
                    m_pPDFReleaseDocument(pPDFInfo->pPDFdoc);
                    pPDFInfo->pPDFdoc = NULL;
                }
                // Delete the PDF file.  Something went wrong.
                if ( pPDFInfo )
                    delete pPDFInfo;
                return DTWAIN_ERR_FILEWRITE;
            }
        }
    }
    else
    if ( m_MultiPageStruct.Stage == DIB_MULTI_NEXT)
    {
        pPDFInfo = (PDFINFO *)m_MultiPageStruct.pUserData;
        pPDFInfo->nCurrentPage++;
        pPDFInfo->ImageInfoEx = *(static_cast<DTWAINImageInfoEx*>(pUserInfo));
        if ( !pPDFInfo->IsFileOpened || !pPDFInfo->IsPDFStarted )
            return DTWAIN_ERR_FILEWRITE;
    }

    // Initialize the page dimensions depending on the image information
    retval = InitializePDFPage(pPDFInfo, bitmap);
    // Set the thumbnail if used
    if ( pPDFInfo->ImageInfoEx.PDFUseThumbnail )
    {
        m_pPDFSetThumbnailFile(pPDFInfo->pPDFdoc, m_sThumbnailFile.c_str());
    }

    m_pPDFSetImageType(pPDFInfo->pPDFdoc, m_nImageType);
    if ( m_nImageType == 0 )
    {
        m_pPDFSetDPI(pPDFInfo->pPDFdoc, pPDFInfo->ImageInfoEx.ResolutionX);
    }

    // Set any other text to write (searchable text is included in this)
    if ( m_MultiPageStruct.Stage != DIB_MULTI_LAST || m_MultiPageStruct.Stage == 0 )
    {
        CTL_TEXTELEMENTPTRLIST::iterator it = pPDFInfo->ImageInfoEx.PDFTextElementList.begin();
        CTL_TEXTELEMENTPTRLIST::iterator it2 = pPDFInfo->ImageInfoEx.PDFTextElementList.end();
        while (it != it2 )
        {
            m_pPDFAddPageText(pPDFInfo->pPDFdoc, it->get());
            ++it;
        }
    }

    if (!m_pPDFWritePage(pPDFInfo->pPDFdoc, path))
    {
        delete_file(path);
        m_pPDFReleaseDocument(pPDFInfo->pPDFdoc);
        delete pPDFInfo;
        return DTWAIN_ERR_FILEWRITE;
    }

    // Add the file to the array for later deletion
    // delete the temporary file
    delete_file(path);

    if ( m_MultiPageStruct.Stage == 0)
    {
        m_pPDFEndCreation(pPDFInfo->pPDFdoc);
        m_pPDFReleaseDocument(pPDFInfo->pPDFdoc);
        RemoveAllImageFiles(pPDFInfo);
        delete pPDFInfo;
    }

    return retval;
}


int CPDFImageHandler::InitializePDFPage(PDFINFO *pPDFInfo, HANDLE bitmap)
{
    // Initialize the page.

    // Get the orientation
    int rotation = pPDFInfo->ImageInfoEx.PDFOrientation;
    string sDimensions;

    if ( rotation != DTWAIN_PDF_PORTRAIT &&
         rotation != DTWAIN_PDF_LANDSCAPE )
         rotation = DTWAIN_PDF_PORTRAIT;

    // Set the rotation
    m_pPDFSetLongField(pPDFInfo->pPDFdoc, PDF_ORIENTATION, rotation);


    // Check if normal paper size is specified
    if ( !(pPDFInfo->ImageInfoEx.PDFPageSize == DTWAIN_PDF_CUSTOMSIZE) &&
         !(pPDFInfo->ImageInfoEx.PDFPageSize == DTWAIN_PDF_VARIABLEPAGESIZE) &&
         !(pPDFInfo->ImageInfoEx.PDFPageSize == DTWAIN_PDF_PIXELSPERMETERSIZE))
    {
        // One of the default page sized (A4, USLETTER, etc.)
        m_pPDFSetLongField(pPDFInfo->pPDFdoc, PDF_MEDIABOX, pPDFInfo->ImageInfoEx.PDFPageSize);
    }
    else
    if ( pPDFInfo->ImageInfoEx.PDFPageSize == DTWAIN_PDF_CUSTOMSIZE )
    {
        // Dimensions specified by the user
        ostringstream sBuf;
        sBuf << "[0 0 " << (int)pPDFInfo->ImageInfoEx.PDFCustomSize[0] << " " << (int)pPDFInfo->ImageInfoEx.PDFCustomSize[1] << "]";
        sDimensions = sBuf.str();
        m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_MEDIABOX, sDimensions.c_str());
    }
    else
    if ( pPDFInfo->ImageInfoEx.PDFPageSize == DTWAIN_PDF_VARIABLEPAGESIZE )
    {
        m_pPDFSetLongField(pPDFInfo->pPDFdoc, PDF_MEDIABOX, -1);
    }
    else
    {
        // Determine the size of the page, given the DIB dimensions and bytes per meter
        LPBITMAPINFOHEADER pbi = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock(bitmap);

        double xInches = (double)pbi->biXPelsPerMeter / 39.37;
        double yInches = (double)pbi->biYPelsPerMeter / 39.37;
        double xWidth = (double)pbi->biWidth;
        double yHeight = (double)pbi->biHeight;

        if ( FLOAT_CLOSE(xInches,0.0) || FLOAT_CLOSE(yInches, 0.0) )
        {
            ImageMemoryHandler::GlobalUnlock(bitmap);
            return DTWAIN_ERR_BAD_DIB_PAGE; // this page cannot be created due to improper pels per meter
        }

        double widthInPoints = xWidth / xInches * 72.0;
        double heightInPoints = yHeight / yInches * 72.0;

        // Dimensions specified by the user
        ostringstream sBuf;
        sBuf << "[0 0 " << widthInPoints << " " << heightInPoints << "]";
        sDimensions = sBuf.str();

            m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_MEDIABOX, sDimensions.c_str());

        if ( CTL_TwainDLLHandle::s_lErrorFilterFlags )
        {
            CTL_StringType sOut = _T("PDF Computed media box: ");
            sOut += StringConversion::Convert_Ansi_To_Native(sDimensions);
            CTL_TwainAppMgr::WriteLogInfo(sOut);
        }
    }


    // This will set the scaling
    // Best fit overrides all scale types
    if ( pPDFInfo->ImageInfoEx.PDFScaleType == DTWAIN_PDF_FITPAGE )
    {

        m_pPDFSetLongField(pPDFInfo->pPDFdoc, PDF_SCALETYPE, DTWAIN_PDF_FITPAGE);

    }
    else
    if ( pPDFInfo->ImageInfoEx.PDFScaleType == DTWAIN_PDF_NOSCALING )
    {

        m_pPDFSetLongField(pPDFInfo->pPDFdoc, PDF_SCALETYPE, DTWAIN_PDF_NOSCALING);

    }
    else
    if ( pPDFInfo->ImageInfoEx.PDFScaleType == DTWAIN_PDF_CUSTOMSCALE )
    {

        m_pPDFSetLongField(pPDFInfo->pPDFdoc, PDF_SCALETYPE, DTWAIN_PDF_CUSTOMSCALE);

        m_pPDFSetScaling(pPDFInfo->pPDFdoc,pPDFInfo->ImageInfoEx.PDFCustomScale[0],
                                      pPDFInfo->ImageInfoEx.PDFCustomScale[1]);

    }


    m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_AUTHOR,   string("(" + StringConversion::Convert_Native_To_Ansi(pPDFInfo->ImageInfoEx.PDFAuthor) + ")").c_str());

    m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_PRODUCER, string("(" + StringConversion::Convert_Native_To_Ansi(pPDFInfo->ImageInfoEx.PDFProducer) + ")").c_str());

    m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_KEYWORDS, string("(" + StringConversion::Convert_Native_To_Ansi(pPDFInfo->ImageInfoEx.PDFKeywords) + ")").c_str());

    m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_TITLE,    string("(" + StringConversion::Convert_Native_To_Ansi(pPDFInfo->ImageInfoEx.PDFTitle) + ")").c_str());

    m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_SUBJECT,  string("(" + StringConversion::Convert_Native_To_Ansi(pPDFInfo->ImageInfoEx.PDFSubject) + ")").c_str());

    m_pPDFSetNameField(pPDFInfo->pPDFdoc, PDF_CREATOR,  string("(" + StringConversion::Convert_Native_To_Ansi(pPDFInfo->ImageInfoEx.PDFCreator) + ")").c_str());

    return 0;
}

void CPDFImageHandler::RemoveAllImageFiles(PDFINFO *pPDFInfo)
{
    CTL_StringArrayType::iterator it = pPDFInfo->TempFileArray.begin();
    while (it != pPDFInfo->TempFileArray.end())
    {
        delete_file((*it).c_str());
        ++it;
    }
}


void CPDFImageHandler::SetMultiPageStatus(DibMultiPageStruct *pStruct)
{

    if ( pStruct )
        m_MultiPageStruct = *pStruct;
}


void CPDFImageHandler::GetMultiPageStatus(DibMultiPageStruct *pStruct)
{
    *pStruct = m_MultiPageStruct;
}

int CPDFImageHandler::WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE * /*pImage2*/, UINT32 /*wid*/, UINT32 /*ht*/,
                                 UINT32 /*bpp*/, UINT32 /*nColors*/, RGBQUAD * /*pPal*/, void * /*pUserInfo*/)
{
    return 0;
}

void CPDFImageHandler::SetSearchableText(const std::string& sText)
{
    m_sSearchableText = sText;
}
