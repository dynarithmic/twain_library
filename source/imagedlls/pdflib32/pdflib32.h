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
