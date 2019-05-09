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

    For more information, the license file LICENSE.TXT that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef PCXFUN32_H_
#define PCXFUN32_H_

#ifndef WINBIT32_H
#include "winbit32.h"
#endif

#ifndef WINBIT32_H
    #include "winbit32.h"
#endif

#include <vector>
#include <fstream>
#include <memory>

#define DCXHEADER_ID 0x3ADE68B1
namespace dynarithmic
{
    struct PCXHEAD
    {
        char manufacturer;
        char version;
        char encoding;
        char bits;
        short int xmin,ymin;
        short int xmax,ymax;
        short int hres;
        short int vres;
        char palette[48];
        char reserved;
        char colour_planes;
        short int bytes_per_line;
        short int palette_type;
        char filler[58];
    };

    struct DCXHEADER
    {
        DWORD Id;
        DWORD nOffsets[1024];
    };

    struct DCXINFO
    {
        DCXHEADER DCXHeader;
        std::unique_ptr<std::ofstream> fh;
        int nCurrentPage;
        int nCurrentOffset;
    };

    class CPCXImageHandler : public CDibInterface
    {
        public:
            CPCXImageHandler(DTWAINImageInfoEx &ImageInfoEx) : m_ImageInfoEx(ImageInfoEx) {}
            // Virtual interface
            virtual CTL_String GetFileExtension() const  override;
            virtual HANDLE  GetFileInformation(LPCSTR path) override;
            virtual int     WriteGraphicFile(CTL_ImageIOHandler* pThis, LPCTSTR path, HANDLE bitmap, void *pUserInfo=NULL) override;
            virtual int     WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE *pImage2, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 cpal, RGBQUAD *pPal, void *pUserInfo=NULL) override;

            virtual void SetMultiPageStatus(DibMultiPageStruct *pStruct) override;
            virtual void GetMultiPageStatus(DibMultiPageStruct *pStruct) override;

        protected:
            void DestroyAllObjects();
            bool OpenOutputFile(LPCTSTR pFileName);
            bool CloseOutputFile();
            WORD PCXWriteLine(LPSTR p, std::ofstream& fh,int n);

        private:
            bool m_bWriteOk;
            std::vector<CHAR> m_plinebuffer;
            std::vector<CHAR> m_pextrabuffer;
            DCXINFO *m_pDCXInfo;
            std::unique_ptr<std::ofstream> m_hFile;
//            HANDLE m_hFile;
            DTWAINImageInfoEx m_ImageInfoEx;
    };
}
#endif

