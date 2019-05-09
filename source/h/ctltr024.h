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
#ifndef CTLTR024_H_
#define CTLTR024_H_

#include "ctltrp.h"
#include "ctltwses.h"
#include "ctlobtyp.h"

namespace dynarithmic
{
    class CTL_ImageTriplet : public CTL_TwainTriplet
    {
        public:
            CTL_ImageTriplet(CTL_ITwainSession *pSession,
                             CTL_ITwainSource *pSource);

        protected:
            void  InitVars(TW_UINT16 nType,
                           CTL_EnumGetType nGet,
                           void *pData);

            bool QueryAndRemoveDib(CTL_TwainAcquireEnum acquireType, CTL_TwainDibArray& pArray, size_t nWhich);

        private:
    };
}
#endif

