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
#ifndef CTLTR008_H_
#define CTLTR008_H_
#include "ctltrp.h"
#include "ctltwses.h"
namespace dynarithmic
{
    class CTL_ProcessEventTriplet : public CTL_TwainTriplet
    {
        public:
            CTL_ProcessEventTriplet(CTL_ITwainSession* pSession,
                                    const CTL_ITwainSource* pSource,
                                    MSG *pMsg,
                                    bool isDSM2);

            TW_UINT16       ExecuteEventHandler();
            bool            ResetTransfer(TW_UINT16 Msg=MSG_RESET);
            void            SetMessage(TW_UINT16 nMsg) { m_Event.TWMessage = nMsg;  }

        private:
            TW_EVENT   m_Event;
            bool       m_bDSM2Used;
            MSG        *m_pMsg;

    };
}
#endif

