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
#ifndef CTLTR036_H_
#define CTLTR036_H_

#include "ctltr010.h"
namespace dynarithmic
{
    class CTL_CustomDSTriplet : public CTL_TwainTriplet
    {
        public:
            CTL_CustomDSTriplet(CTL_ITwainSession *pSession,
                                   CTL_ITwainSource* pSource,
                                   TW_UINT16 nMsg);
            TW_UINT16   Execute();
            bool        IsSuccessful() const;
            TW_UINT32   GetDataSize() const;
            HANDLE      GetData() const;
            void        SetDataSize(TW_UINT32 nSize);
            TW_UINT16   SetData(HANDLE hData, int nSize);

        private:
            TW_CUSTOMDSDATA     m_CustomDSData;
    };
}
#endif
