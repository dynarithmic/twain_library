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
#ifndef CTLTR031_H_
#define CTLTR031_H_

#include <vector>
#include "ctltr026.h"
#include "ctliface.h"


namespace dynarithmic
{
    class CTL_ImageMemXferTriplet : public CTL_ImageXferTriplet
    {
        public:
            CTL_ImageMemXferTriplet(CTL_ITwainSession *pSession,
                                    CTL_ITwainSource* pSource,
                                    HANDLE hDib,
                                    TW_UINT32 nFlags,
                                    TW_UINT16 nPixelType,
                                    TW_UINT32 nNumBytes,
                                    TW_UINT16 nCompression=TWCP_NONE);

            TW_UINT16           Execute();
            ~CTL_ImageMemXferTriplet();

        private:
            void InitXferBuffer();

            TW_IMAGEMEMXFER m_ImageMemXferBuffer;
            TW_MEMORY       m_TempMemory;
            HANDLE          m_DibStrip;
            unsigned char TW_HUGE * m_ptrDib;
            unsigned char TW_HUGE * m_ptrOrig;
            TW_UINT16       m_nPixelType;
            TW_UINT32       m_nCurDibSize;
            TW_UINT16       m_nCompression;
            LONG            m_nCompressPos;
            unsigned char TW_HUGE * m_ptrTempDib;
            HANDLE          m_origDibHandle;
            CTL_TwainDynMemoryHandler m_dynMemoryHandler;
    };
}
#endif


