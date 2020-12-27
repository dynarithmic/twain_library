/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
#ifndef TXTFUN32_H_
#define TXTFUN32_H_

#include <memory>

#include "ctlobstr.h"
#include "winbit32.h"
#include "ocrinterface.h"
namespace dynarithmic
{
    class CTL_ImageIOHandler;
    class CTL_TwainDib;
    struct CTextPageInfo
    {
        int curPageNum;
        std::unique_ptr<std::ofstream> fh;
        CTL_StringType szTempFile;
        std::shared_ptr<CTL_ImageIOHandler> m_pOrigHandler;

        CTextPageInfo(int curPage) : curPageNum(curPage) { }
        ~CTextPageInfo();
    };

    class CTextImageHandler : public CDibInterface
    {
        public:
            CTextImageHandler(DTWAINImageInfoEx &ImageInfoEx, OCREngine* pEngine, LONG inputFormat,
                                CTL_TwainDib* pDib) :
                                     m_ImageInfoEx(ImageInfoEx),
                                     m_pOCREngine(pEngine),
                                     m_InputFormat(inputFormat),
                                     m_pDib(pDib) {}
            // Virtual interface
            virtual CTL_String GetFileExtension() const  override;
            virtual HANDLE  GetFileInformation(LPCSTR path)  override;
            virtual int     WriteGraphicFile(CTL_ImageIOHandler *pHandler, LPCTSTR path, HANDLE bitmap, void *pUserInfo = NULL)  override;
            virtual int     WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE *pImage2, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 cpal, RGBQUAD *pPal,
                                       void *pUserInfo = NULL)  override;

            virtual void SetMultiPageStatus(DibMultiPageStruct *pStruct);
            virtual void GetMultiPageStatus(DibMultiPageStruct *pStruct);

        protected:
            bool OpenOutputFile(LPCTSTR pFileName);
            bool CloseOutputFile();
            void DestroyAllObjects();
            int WriteTempFile();
            int SaveOCR();

        private:
            DTWAINImageInfoEx m_ImageInfoEx;
            OCREngine *m_pOCREngine;
            LONG m_InputFormat;
            CTL_TwainDib* m_pDib;
            std::unique_ptr<std::ofstream> m_fh;
            std::shared_ptr<CTextPageInfo> m_pTextPageInfo;
    };
}
#endif
