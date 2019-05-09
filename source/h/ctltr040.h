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
#ifndef CTLTR040_H_
#define CTLTR040_H_

#include "ctltrp.h"
namespace dynarithmic
{
    class CTL_ITwainSession;

    class CTL_DSMCallbackTriplet : public CTL_TwainTriplet
    {
        public:
            CTL_DSMCallbackTriplet(CTL_ITwainSession *pSession, CTL_ITwainSource* pSource, TW_UINT16 msg);
            void setDSMEntryProc(DSMENTRYPROC proc)
            {
                m_DSMEntryProc = proc;
                m_TWCallback.CallBackProc = reinterpret_cast<TW_MEMREF>(proc);
                m_TWCallback.RefCon = 0;
            }
            DSMENTRYPROC getEntryProc() { return m_DSMEntryProc; }
            TW_CALLBACK getCallback() const { return m_TWCallback; }

        private:
            DSMENTRYPROC m_DSMEntryProc;
            TW_CALLBACK m_TWCallback;
    };

    class CTL_DSMCallbackTripletRegister : public CTL_DSMCallbackTriplet
    {
        public:
            CTL_DSMCallbackTripletRegister(CTL_ITwainSession *pSession, CTL_ITwainSource* pSource, DSMENTRYPROC proc) :
                CTL_DSMCallbackTriplet(pSession, pSource, MSG_REGISTER_CALLBACK)
            { setDSMEntryProc(proc); }
    };
}
#endif
