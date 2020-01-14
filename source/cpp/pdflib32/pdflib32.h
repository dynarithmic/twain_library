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

For more information, the license file named LICENSE that is located in the root
directory of the DTWAIN installation covers the restrictions under the LGPL license.
Please read this file before deploying or distributing any application using DTWAIN.
*/
#ifndef PDFLIB32_H
#define PDFLIB32_H
#include <winconst.h>
#include <pdfconst.h>
#include "pdffont_basic.h"
#ifdef PDFLIB_INTERNAL
    #define FUNCCONVENTION CALLBACK
#else
    #define FUNCCONVENTION DLLENTRY_DEF
#endif

namespace dynarithmic
{
    #ifdef __cplusplus
    extern "C"
    {
    #endif
        LPVOID FUNCCONVENTION DTWLIB_PDFGetNewDocument();
        BOOL FUNCCONVENTION DTWLIB_PDFOpenNewFile(void* pDoc, LPCTSTR szFile);
        void FUNCCONVENTION DTWLIB_PDFSetCompression(void* pDoc, bool bCompress);
        void FUNCCONVENTION DTWLIB_PDFSetNameField(void* pDoc, LONG nWhich, LPCSTR szName);
        BOOL FUNCCONVENTION DTWLIB_PDFStartCreation(void *pDoc);
        BOOL FUNCCONVENTION DTWLIB_PDFEndCreation(void *pDoc);
        void FUNCCONVENTION DTWLIB_PDFSetImageType(void *pDoc, LONG iType);
        void FUNCCONVENTION DTWLIB_PDFSetLongField(void *pDoc, LONG nWhich, LONG nValue);
        BOOL FUNCCONVENTION DTWLIB_PDFWritePage(void *pDoc, LPCTSTR szPath);
        void FUNCCONVENTION DTWLIB_PDFSetScaling(void *pDoc, double xscale, double yscale);
        void FUNCCONVENTION DTWLIB_PDFReleaseDocument(void *pDoc); 
        void FUNCCONVENTION DTWLIB_PDFSetThumbnailFile(void *pDoc, LPCTSTR szPath );
        void FUNCCONVENTION DTWLIB_PDFSetDPI(void *pDoc, LONG dpi);
        void FUNCCONVENTION DTWLIB_PDFSetEncryption(void *pDoc, LPCTSTR szOwnerPass,
                                                  LPCTSTR szUserPass, LONG Permissions,
                                                  bool bUseStrongEncrypt,
											      bool bUseAESEncryption);

        LONG FUNCCONVENTION DTWLIB_PSWriteFile(LPCTSTR szFileIn,
                                             LPCTSTR szFileOut,
                                             LONG PSType,
                                             LPCTSTR szTitle,
                                             bool bUseEncapsulated);
    
        void FUNCCONVENTION DTWLIB_PDFGetDLLVersion(LPLONG lMajor, LPLONG lMinor, LPLONG lPatch);
        void FUNCCONVENTION DTWLIB_PDFSetASCIICompression(void *pDoc, bool bCompression);
		void FUNCCONVENTION DTWLIB_PDFSetNoCompression(void *pDoc, bool bCompression);
		void FUNCCONVENTION DTWLIB_PDFSetSearchableText(void *pDoc, LPCSTR text);
        void FUNCCONVENTION DTWLIB_PDFAddPageText(void *pDoc, PDFTextElement* pElement); /*LPCSTR szTest, LONG xPos, LONG yPos, 
                                                  LPCSTR fontName, double fontSize, LONG colorRGB,
                                                  LONG renderMode, double scaling, double charSpacing,
                                                  double wordSpacing,
                                                  LONG riseValue, LONG flags, double scalingX, double scalingY);*/
        void FUNCCONVENTION DTWLIB_PDFSetPolarity(void *pDoc, LONG Polarity);                                              
    #ifdef __cplusplus
    }
    #endif
}
#endif
