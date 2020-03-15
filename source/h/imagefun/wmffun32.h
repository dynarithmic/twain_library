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
#ifndef WMFFUN32_H
#define WMFFUN32_H
#include "winbit32.h"

// Aldus Placeable Header ===================================================
// Since we are a 32bit app, we have to be sure this structure compiles to
// be identical to a 16 bit app's version. To do this, we use the #pragma
// to adjust packing, we use a WORD for the hmf handle, and a SMALL_RECT
// for the bbox rectangle.
#if defined (__GCC__)
    #pragma pack (push, 1)
#else
    #pragma pack( push )
    #pragma pack( 2 )
#endif
namespace dynarithmic
{
    struct APMHEADER
    {
        UINT32      dwKey;
        WORD        hmf;
        SMALL_RECT  bbox;
        WORD        wInch;
        UINT32      dwReserved;
        WORD        wCheckSum;
    };

    typedef APMHEADER* PAPMHEADER;

    #pragma pack( pop )

    #define METAFILE_VERSION    1
    #define ALDUSKEY        0x9AC6CDD7
    //#define   ALDUSMFHEADERSIZE   APMHEADER   // Avoid sizeof is struct alignment >1

    // Alignment types
    typedef enum {
        AlignNone = -1,
        AlignDefault,
        AlignTopLeft,
        AlignTopCentre,
        AlignTopRight,
        AlignMiddleLeft,
        AlignMiddleCentre,
        AlignMiddleRight,
        AlignBottomLeft,
        AlignBottomCentre,
        AlignBottomRight,
        AlignStretch,
        AlignFit,
    } METAALIGNMENT;


    class CWMFImageHandler : public CDibInterface
    {
        public:
            CWMFImageHandler(int nWMFType=0) : m_nWMFType(nWMFType) { }
            // Virtual interface
            virtual CTL_String GetFileExtension() const override;
            virtual HANDLE  GetFileInformation(LPCSTR path) override;
            virtual int     WriteGraphicFile(CTL_ImageIOHandler *pHandler, LPCTSTR path, HANDLE bitmap, void *pUserInfo=NULL) override;
            virtual int     WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE *pImage2, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 cpal, RGBQUAD *pPal, void *pUserInfo = NULL) override;
        protected:
            int WriteEMF(BYTE *pImage2, UINT32 wid, UINT32 ht,
                         UINT32 bpp, UINT32 nColors, RGBQUAD *pPal, void *pUserInfo);
            int WriteWMF(BYTE *pImage2, UINT32 wid, UINT32 ht,
                         UINT32 bpp, UINT32 nColors, RGBQUAD *pPal, void *pUserInfo);
            WORD CalculateAPMCheckSum( APMHEADER apmfh );
        private:
            int m_nWMFType;
    };
}
#endif

