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
#ifndef CTLTR028_H_
#define CTLTR028_H_

#include <unordered_map>
#include "ctltrp.h"

namespace dynarithmic
{
    class CTL_SetupFileXferTriplet : public CTL_TwainTriplet
    {
        public:
            CTL_SetupFileXferTriplet(CTL_ITwainSession *pSession,
                                    CTL_ITwainSource* pSource,
                                    TW_UINT16 GetSetType,
                                    CTL_TwainFileFormatEnum FileFormat,
                                    const CTL_StringType& strFileName);

            CTL_StringType  GetFileName() const { return StringConversion::Convert_AnsiPtr_To_Native(m_SetupFileXfer.FileName); }
            CTL_TwainFileFormatEnum  GetFileFormat() const { return (CTL_TwainFileFormatEnum) m_SetupFileXfer.Format; }
            TW_UINT16 Execute();
            void SetJPEGQuality(TW_INT16 quality) { m_capMap[DTWAIN_CV_ICAPJPEGQUALITY] = (LONG)quality; }
            void SetJPEGPixelType(TW_UINT16 pixType) { m_capMap[DTWAIN_CV_ICAPJPEGPIXELTYPE] = (LONG)pixType; }
            void SetCompression(TW_UINT16 nCompression) { m_capMap[DTWAIN_CV_ICAPCOMPRESSION] = nCompression; }
            typedef std::unordered_map<TW_UINT16, LONG> FileXferCapMap;

        private:
            FileXferCapMap          m_capMap;
            TW_SETUPFILEXFER        m_SetupFileXfer;

    };
}
#endif
