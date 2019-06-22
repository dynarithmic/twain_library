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
#ifndef PDFFUN32_H_
#define PDFFUN32_H_

#include <string>
#include <vector>

#ifndef WINBIT32_H
#include "winbit32.h"
#endif
#include "ctlobstr.h"
namespace dynarithmic
{
    typedef void * PdfDocument;

    typedef void* (CALLBACK *PDF_FUNC1)();
    typedef BOOL  (CALLBACK *PDF_FUNC2)(void* pDoc, LPCTSTR szFile);
    typedef void  (CALLBACK *PDF_FUNC3)(void* pDoc, bool bCompress);
    typedef void  (CALLBACK *PDF_FUNC4)(void* pDoc, LONG nWhich, LPCSTR szName);
    typedef BOOL  (CALLBACK *PDF_FUNC5)(void *pDoc);
    typedef BOOL  (CALLBACK *PDF_FUNC6)(void *pDoc);
    typedef void  (CALLBACK *PDF_FUNC7)(void *pDoc, LONG iType);
    typedef void  (CALLBACK *PDF_FUNC8)(void *pDoc, LONG nWhich, LONG nValue);
    typedef BOOL  (CALLBACK *PDF_FUNC9)(void *pDoc, LPCTSTR szPath);
    typedef void  (CALLBACK *PDF_FUNC10)(void *pDoc, double xscale, double yscale);
    typedef void  (CALLBACK *PDF_FUNC11)(void *pDoc);
    typedef void  (CALLBACK *PDF_FUNC12)(void *pDoc, LPCTSTR szFile);
    typedef void  (CALLBACK *PDF_FUNC13)(void *pDoc, LONG DPI);
    typedef void  (CALLBACK *PDF_FUNC14)(void *pDoc, LPCTSTR szOwnerPass,
                                              LPCTSTR szUserPass, LONG Permissions,
                                              bool bUseStrongEncrypt,
                                              bool bUseAESEncryption);
    typedef void  (CALLBACK *PDF_FUNC15)(void* pDoc, bool bCompress);
    typedef void  (CALLBACK *PDF_FUNC16)(void* pDoc, LPCSTR szText);
    typedef void  (CALLBACK *PDF_FUNC17)(void* pDoc, PDFTextElement* pElement);/*LPCSTR szText, LONG xPos, LONG yPos,
                                         LPCSTR fontName, DTWAIN_FLOAT fontSize, LONG colorRGB,
                                         LONG renderMode, double scaling,
                                         double charSpacing, double wordSpacing,
                                         LONG riseValue, LONG flags, double scalingX, double scalingY, double rotationAngle);*/
    typedef void  (CALLBACK *PDF_FUNC18)(void *pDoc, LONG Polarity);
	typedef void  (CALLBACK *PDF_FUNC19)(void* pDoc, bool bCompress);

    struct PDFINFO
    {
    //    PdfDocument *pPDFdoc;
        PdfDocument pPDFdoc;
        int nCurrentPage;
        bool IsFileOpened;
        bool IsPDFStarted;

        // PDF Information
        CTL_StringType sFileName;

        std:: string sAuthor;
        std:: string sProducer;
        std:: string sTitle;
        std:: string sSubject;
        std:: string sKeywords;
        std:: string sCreator;

        CTL_StringArrayType TempFileArray;
        DTWAINImageInfoEx ImageInfoEx;
    };
    #ifndef DTWAIN_LIMITED_VERSION
    class CPDFImageHandler : public CDibInterface
    {
        private:
            DTWAINImageInfoEx m_ImageInfoEx;

        public:
            CPDFImageHandler(const CTL_StringType& sFileName, DTWAINImageInfoEx &ImageInfoEx);

            bool LibraryIsLoaded() const { return s_bLibraryLoaded; }
            LONG GetErrorCode() const { return m_nError; }

            // Virtual interface
            virtual CTL_String GetFileExtension() const;
            virtual HANDLE  GetFileInformation(LPCSTR path) ;
            virtual int     WriteGraphicFile(CTL_ImageIOHandler* ptrHandler, LPCTSTR path, HANDLE bitmap, void *pUserInfo=NULL) override;
            virtual int     WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE *pImage2, UINT32 wid, UINT32 ht, UINT32 bpp, UINT32 cpal, RGBQUAD *pPal, void *pUserInfo=NULL) override;

            virtual void SetMultiPageStatus(DibMultiPageStruct *pStruct);
            virtual void GetMultiPageStatus(DibMultiPageStruct *pStruct);

            void SetAuthor(const std:: string& s) { m_sAuthor = s; }
            void SetProducer(const std:: string& s) { m_sProducer = s; }
            void SetTitle(const std:: string& s) { m_sTitle = s; }
            void SetSubject(const std:: string& s) { m_sSubject = s; }
            void SetKeywords(const std:: string& s) { m_sKeywords = s; }
            void SetCreator(const std:: string& s) {m_sCreator = s; }
            void SetImageType(int nWhich) { m_nImageType = nWhich; }
            int  GetImageType() const { return m_nImageType; }
            void SetThumbnailFile(const CTL_StringType& s) { m_sThumbnailFile = s; }
            void SetDPI(LONG dpi) { m_dpi = dpi; }
            LONG GetDPI() const { return m_dpi; }
            void SetSearchableText(const std:: string& s);
            void AddPDFTextElement(PDFTextElementPtr element) { m_ImageInfoEx.PDFTextElementList.push_back(element); }

            static void UnloadPDFLibrary() { s_bLibraryLoaded = false; }

        protected:
            bool OpenOutputFile(LPCTSTR pFileName);
            int  InitializePDFPage(PDFINFO *pPDFInfo, HANDLE bitmap);
            void RemoveAllImageFiles(PDFINFO *pPDFInfo);
            bool LoadPDFLibrary();

        private:

            CTL_StringType m_sFileName;
            std:: string m_sAuthor;
            std:: string m_sProducer;
            std:: string m_sTitle;
            std:: string m_sSubject;
            std:: string m_sKeywords;
            CTL_StringType  m_sThumbnailFile;
            std:: string m_sCreator;
            std:: string m_sSearchableText;

            int m_nImageType;
            LONG m_nError;
            LONG m_dpi;

            static PDF_FUNC1  m_pPDFGetNewDocument  ;
            static PDF_FUNC2  m_pPDFOpenNewFile     ;
            static PDF_FUNC3  m_pPDFSetCompression  ;
            static PDF_FUNC4  m_pPDFSetNameField    ;
            static PDF_FUNC5  m_pPDFStartCreation   ;
            static PDF_FUNC6  m_pPDFEndCreation     ;
            static PDF_FUNC7  m_pPDFSetImageType    ;
            static PDF_FUNC8  m_pPDFSetLongField    ;
            static PDF_FUNC9  m_pPDFWritePage       ;
            static PDF_FUNC10 m_pPDFSetScaling      ;
            static PDF_FUNC11 m_pPDFReleaseDocument ;
            static PDF_FUNC12 m_pPDFSetThumbnailFile;
            static PDF_FUNC13 m_pPDFSetDPI;
            static PDF_FUNC14 m_pPDFSetEncryption   ;
            static PDF_FUNC15 m_pPDFSetASCIICompression;
            static PDF_FUNC16 m_pPDFSetSearchableText;
            static PDF_FUNC17 m_pPDFAddPageText;
            static PDF_FUNC18 m_pPDFSetPolarity;
			static PDF_FUNC19 m_pPDFSetNoCompression;

            static bool s_bLibraryLoaded;
    };

    class CPSImageHandler : public CPDFImageHandler
    {
        public:
              CPSImageHandler(const CTL_StringType& sFileName, DTWAINImageInfoEx &ImageInfoEx) :
              CPDFImageHandler(sFileName, ImageInfoEx) { }

    };
}
#endif
#endif
