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
#ifndef TIFFUN32_H_
#define TIFFUN32_H_

// These #include files are for the TIFFLIB DLL library
    #include "winbit32.h"

// Includes stuff from the LIBTIFF library
#include "FreeImage.h"
#include <stdio.h>
namespace dynarithmic
{
    class CTIFFImageHandler : public CDibInterface
    {
            enum { TiffFormatLZW=500,
                   TiffFormatNONE=600,
                   TiffFormatGROUP3=700,
                   TiffFormatGROUP4=800,
                   TiffFormatPACKBITS=801,
                   TiffFormatDEFLATE=802,
                   TiffFormatJPEG=803,
                   TiffFormatPIXARLOG=805,
                   TiffFormatNONEMULTI = 900,
                   TiffFormatGROUP3MULTI = 901,
                   TiffFormatGROUP4MULTI  = 902,
                   TiffFormatPACKBITSMULTI = 903,
                   TiffFormatDEFLATEMULTI = 904,
                   TiffFormatJPEGMULTI = 905,
                   TiffFormatLZWMULTI = 906,
                   TiffFormatPIXARLOGMULTI=908
     };

        private:
            UINT32 m_nFormat;
            DTWAINImageInfoEx m_ImageInfoEx;
            TCHAR m_FileName[255];
            bool   m_bWriteOk;
            bool    m_nError;

            static CTL_StringType s_AppInfo;

        public:
            CTIFFImageHandler(UINT32 nFormat, DTWAINImageInfoEx& ImageInfoEx) :
                m_nFormat(nFormat),
                m_ImageInfoEx(ImageInfoEx),
                m_bWriteOk(false),
                m_nError(false)
                {
                    m_MultiPageStruct.pUserData = nullptr;
                    m_MultiPageStruct.Stage = 0;
                }

            // Virtual interface
            virtual CTL_String GetFileExtension() const;
            virtual HANDLE  GetFileInformation(LPCSTR path) ;
            virtual int     WriteGraphicFile(CTL_ImageIOHandler* ptrHandler, LPCTSTR path, HANDLE bitmap, void *pUserInfo=NULL) override;
            virtual int     WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE *pImage2, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 cpal, RGBQUAD *pPal, void *pUserInfo=NULL) override;

            bool OpenOutputFile(LPCTSTR pFileName);
            bool CloseOutputFile();
            void SetMultiPageStatus(DibMultiPageStruct *pStruct);
            void GetMultiPageStatus(DibMultiPageStruct *pStruct);
            LONG GetErrorCode() const { return m_nError; }
            int  Tiff2PS(LPCTSTR szFileIn, LPCTSTR szFileOut, LONG PSType,
                         LPCTSTR szTitle, bool IsEncapsulated);
            virtual void DestroyAllObjects();

        protected:
            int ProcessCompressionType(fipImage& im, unsigned long&);
    };
}
#endif
