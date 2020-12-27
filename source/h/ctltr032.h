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
#ifndef CTLTR032_H_
#define CTLTR032_H_

#include "ctltr010.h"
namespace dynarithmic
{
    class CTL_ImageLayoutTriplet : public CTL_TwainTriplet
    {
        public:
            CTL_ImageLayoutTriplet(CTL_ITwainSession *pSession,
                                   CTL_ITwainSource* pSource,
                                   TW_UINT16 GetSetType);

            double      GetLeft() const;
            double      GetRight() const;
            double      GetTop() const;
            double      GetBottom() const;
            TW_UINT32   GetDocumentNumber() const;
            TW_UINT32   GetPageNumber() const;
            TW_UINT32   GetFrameNumber() const;
            TW_FRAME    GetFrame() const;
            TW_IMAGELAYOUT* GetImageLayout() { return &m_ImageLayout; }

        private:
            TW_IMAGELAYOUT  m_ImageLayout;
    };


    class CTL_ImageSetLayoutTriplet : public CTL_ImageLayoutTriplet
    {
        public:
            CTL_ImageSetLayoutTriplet(CTL_ITwainSession *pSession,
                                   CTL_ITwainSource* pSource,
                                   const CTL_RealArray &rArray,
                                   TW_UINT16   SetType);
    };
}
#endif


