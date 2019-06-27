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

For more information, the license file named LICENSE that is located in the root
directory of the DTWAIN installation covers the restrictions under the LGPL license.
Please read this file before deploying or distributing any application using DTWAIN.
*/
#ifdef _MSC_VER
#pragma warning (disable:4786)
#endif
#include <vector>
#include <pdflib32.h>
#include <dtwainpdf.h>
using namespace dynarithmic;

#define FUNCCONVENTION CALLBACK

void* FUNCCONVENTION dynarithmic::DTWLIB_PDFGetNewDocument()
{
    return new PdfDocument;
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFReleaseDocument(void *pDoc)
{
    delete (PdfDocument *)pDoc;
}

BOOL FUNCCONVENTION dynarithmic::DTWLIB_PDFOpenNewFile(void* pDoc, LPCTSTR szFile)
{
    return ((PdfDocument *)pDoc)->OpenNewPDFFile(szFile);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetCompression(void* pDoc,  bool bCompress)
{
    ((PdfDocument *)pDoc)->SetCompression(bCompress);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetNameField(void* pDoc, LONG nWhich, LPCSTR szName)
{
    PdfDocument *p = (PdfDocument *)pDoc;
    switch (nWhich)
    {
        case PDF_AUTHOR:
            p->SetAuthor( szName );
        break;
        case PDF_PRODUCER:
            p->SetProducer( szName );
        break;
        case PDF_TITLE:
            p->SetTitle( szName );
        break;
        case PDF_KEYWORDS:
            p->SetKeywords( szName );
        break;
        case PDF_SUBJECT:
            p->SetSubject( szName );
        break;
        case PDF_MEDIABOX:
            p->SetMediaBox( szName );
        break;
        case PDF_CREATOR:
            p->SetCreator(szName);
        break;
    }
}

BOOL FUNCCONVENTION dynarithmic::DTWLIB_PDFStartCreation(void *pDoc)
{
    return ((PdfDocument *)pDoc)->StartPDFCreation();
}

BOOL FUNCCONVENTION dynarithmic::DTWLIB_PDFEndCreation(void *pDoc)
{
    return ((PdfDocument *)pDoc)->EndPDFCreation();
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetImageType(void *pDoc, LONG iType)
{
    ((PdfDocument *)pDoc)->SetImageType(iType);
}

BOOL FUNCCONVENTION dynarithmic::DTWLIB_PDFWritePage(void *pDoc, LPCTSTR szPath)
{
    return ((PdfDocument *)pDoc)->WritePage(szPath);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetLongField(void *pDoc, LONG nWhich, LONG nValue)
{
    PdfDocument *p = (PdfDocument *)pDoc;
    switch (nWhich)
    {
        case PDF_ORIENTATION:
            p->SetOrientation(nValue);
        break;

        case PDF_MEDIABOX:
            p->SetMediaBox(nValue);
        break;

        case PDF_SCALETYPE:
            p->SetScaleType(nValue);
        break;
    }
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetScaling(void *pDoc, double xscale, double yscale)
{
    ((PdfDocument *)pDoc)->SetScaling(xscale, yscale);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetThumbnailFile(void *pDoc, LPCTSTR szPath)
{
    ((PdfDocument *)pDoc)->SetThumbnailFile(szPath);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetDPI(void *pDoc, LONG dpi)
{
    ((PdfDocument *)pDoc)->SetDPI( dpi );
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetEncryption(void *pDoc, LPCTSTR szOwnerPass,
                                          LPCTSTR szUserPass, LONG Permissions,
                                          bool bUseStrongEncrypt,
										  bool bUseAESEncryption)
{
    ((PdfDocument *)pDoc)->SetEncryption(szOwnerPass?szOwnerPass:_T(""),
                                         szUserPass?szUserPass:_T(""),
                                         Permissions,
                                         bUseStrongEncrypt,
										 bUseAESEncryption);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFGetDLLVersion(LPLONG lMajor, LPLONG lMinor, LPLONG lPatch)
{
    *lMajor = 1;
    *lMinor = 9;
    *lPatch = 0;
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetASCIICompression(void *pDoc, bool bSetCompression)
{
    ((PdfDocument *)pDoc)->SetASCIICompression(bSetCompression);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetNoCompression(void *pDoc, bool bSetCompression)
{
	((PdfDocument *)pDoc)->SetNoCompression(bSetCompression);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetSearchableText(void *pDoc, LPCSTR text)
{
    ((PdfDocument *)pDoc)->SetSearchableText(text);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFAddPageText(void* pDoc, PDFTextElement* pElement) 
{
/*    PDFTextElement element;

    element.m_text = szText;
    element.xpos = xPos;
    element.ypos = yPos;
    element.m_font.m_fontName = fontName;
    element.fontSize = fontSize;
    element.colorRGB = colorRGB;
    element.renderMode = renderMode;
    element.strokeWidth = strokeWidth;
    element.scaling = scaling;
    element.wordSpacing = wordSpacing;
    element.charSpacing = charSpacing;
    element.displayFlags = flags;
	element.scalingX = scalingX;
	element.scalingY = scalingY;
*/
    ((PdfDocument *)pDoc)->AddTextElement(pElement);
}

void FUNCCONVENTION dynarithmic::DTWLIB_PDFSetPolarity(void *pDoc, LONG Polarity)
{
    ((PdfDocument *)pDoc)->SetPolarity( Polarity );
}
