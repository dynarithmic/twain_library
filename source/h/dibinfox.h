/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2018 Dynarithmic Software.

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

    For more information, the license file LICENSE.TXT that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef DIBINFOX_H
#define DIBINFOX_H
//...
#include <string>
#include "imagefun/dtwpdf/pdffont_basic.h"
#include <vector>
#include <list>
#include "ctlobstr.h"
namespace dynarithmic
{
    class CTL_ITwainSource;
    class CTL_ITwainSession;

    #define PDFAUTHORKEY    _T("Author")
    #define PDFPRODUCERKEY  _T("Producer")
    #define PDFKEYWORDSKEY  _T("Keywords")
    #define PDFTITLEKEY     _T("Title")
    #define PDFSUBJECTKEY   _T("Subject")
    #define PSTITLEKEY      _T("PSTitle")
    #define PDFOWNERPASSKEY _T("OwnerPassword")
    #define PDFUSERPASSKEY  _T("UserPassword")
    #define PDFCREATORKEY    _T("Creator")
    #define PDFORIENTATIONKEY _T("Orientation")
    #define PDFSCALINGKEY       _T("Scaling")
    #define PDFCOMPRESSIONKEY   _T("Compression")
    #define PDFASCIICOMPRESSKEY _T("ASCIICompression")
    #define PSTYPEKEY       _T("PSType")
    #define PDFPERMISSIONSKEY   _T("Permissions")
    #define PDFJPEGQUALITYKEY   _T("JPEGQuality")
    #define PDFOCRMODE     _T("OCRMode")
    #define PDFTEXTELEMENTKEY _T("TextElement")
    #define PDFPOLARITYKEY _T("Polarity")
    #define PDFAESKEY   _T("AESEncryption")

    struct DTWAINImageOCRPDFInfo
    {
        LONG colorTypeBW;
        LONG colorTypeColor;
        LONG colorTypeGray;
        LONG bppBW;
        LONG bppColor;
        LONG bppGray;
        DTWAINImageOCRPDFInfo() :
            colorTypeBW(DTWAIN_TIFFNONE),
            colorTypeColor(DTWAIN_JPEG),
            colorTypeGray(DTWAIN_BMP),
            bppBW(1), bppColor(8), bppGray(4)
            { }
    };

    struct DTWAINImageInfoEx
    {
        LONG PostscriptType;
        bool IsPostscript;
        bool IsPDF;
        bool IsPostscriptMultipage;
        LONG PDFScaleType;
        bool bIsPDFEncrypted;
        bool bIsAESEncrypted;
        bool bUseStrongEncryption;
        bool PDFUseASCIICompression;
		bool PDFUseNoCompression;
        LONG PDFPermissions;
        LONG PSType;
        LONG UnitOfMeasure;
        LONG ResolutionX;
        LONG ResolutionY;
        LONG nPDFJpegQuality;
        bool IsImageFileCreated;
        CTL_ITwainSource* theSource;
        LONG PDFOrientation;
        bool IsOCRTempImage;
        bool IsOCRUsedForPDF;
        bool IsVistaIcon;
        LONG nPDFPolarity;
        CTL_ITwainSession* theSession;
        bool IsSearchableTextOnPage;

        LONG nJpegQuality;
        bool bProgressiveJpeg;
        LONG PDFPageSize;
        bool PDFUseCompression;
        bool PDFUseThumbnail;
        LONG  PhotoMetric;
        CTL_StringType PDFAuthor;
        CTL_StringType PDFSubject;
        CTL_StringType PDFProducer;
        CTL_StringType PDFKeywords;
        CTL_StringType PDFTitle;
        CTL_StringType PSTitle;
        CTL_StringType PDFCreator;
        CTL_StringType szImageFileName;
        CTL_StringType PDFUserPassword;
        CTL_StringType PDFOwnerPassword;
        DTWAIN_FLOAT PDFThumbnailScale[2];
        DTWAIN_FLOAT PDFCustomSize[2];
        DTWAIN_FLOAT PDFCustomScale[2];
        CTL_TEXTELEMENTPTRLIST PDFTextElementList;
        CTL_SEARCHABLETEXTRANGE PDFSearchableTextRange;
        DTWAINImageOCRPDFInfo PDFOCRInfo;

        // Vista ICONs
        DTWAINImageInfoEx() :
			theSession{},
			PhotoMetric{},
            PostscriptType(0),
            IsPostscript(false),
            IsPDF(false),
            IsPostscriptMultipage(false),
            PDFScaleType(DTWAIN_PDF_FITPAGE),
            bIsPDFEncrypted(false),
            bIsAESEncrypted(false),
            bUseStrongEncryption(false),
            PDFUseASCIICompression(false),
            PDFPermissions(0),
            PSType(0),
            UnitOfMeasure(DTWAIN_INCHES),
            ResolutionX(100),
            ResolutionY(100),
            nPDFJpegQuality(75),
            IsImageFileCreated(false),
            theSource(nullptr),
            PDFOrientation(DTWAIN_PDF_PORTRAIT),
            IsOCRTempImage(false),
            IsOCRUsedForPDF(false),
            IsVistaIcon(false),
            nPDFPolarity(DTWAIN_PDFPOLARITY_POSITIVE),
            IsSearchableTextOnPage(false),
            nJpegQuality(100),
            bProgressiveJpeg(false),
            PDFPageSize(DTWAIN_PDF_NOSCALING),
			PDFUseCompression(true),
			PDFUseNoCompression(false),
            PDFUseThumbnail(false)
            {
                PDFThumbnailScale[0] =
                PDFThumbnailScale[1] =
                PDFCustomSize[0] =
                PDFCustomSize[1] =
                PDFCustomScale[0] =
                PDFCustomScale[1] = 0.0;
            }
    };
}
#endif
