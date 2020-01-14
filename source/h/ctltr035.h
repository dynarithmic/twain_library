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
#ifndef CTLTR035_H_
#define CTLTR035_H_

#include "ctltr013.h"

namespace dynarithmic
{
    class CTL_CapabilityQueryTriplet : public CTL_CapabilityGetOneValTriplet
    {
        public:
            CTL_CapabilityQueryTriplet(CTL_ITwainSession *pSession,
                                       CTL_ITwainSource *pSource,
                                       TW_UINT16 gCap,
                                       TW_UINT16 TwainType=0xFFFF);
            TW_UINT16       Execute();

            bool            IsGet() const
                            { return (m_lCapSupport & TWQC_GET)?true:false; }

            bool            IsGetDefault() const
                            { return (m_lCapSupport & TWQC_GETDEFAULT)?true:false; }

            bool            IsGetCurrent() const
                            { return (m_lCapSupport & TWQC_GETCURRENT) ? true : false; }

            bool            IsSet() const
                            { return (m_lCapSupport & TWQC_SET)?true:false; }

            bool            IsReset() const
                            { return (m_lCapSupport & TWQC_RESET)?true:false; }

            bool            IsSetConstraint() const
                            { return (m_lCapSupport & TWQC_SETCONSTRAINT) ? true : false;}

            bool            IsAnySupport() const
                            { return m_lCapSupport?true:false; }

            UINT            GetSupport() const { return (UINT)m_lCapSupport; }

        protected:
            virtual bool    GetValue( void *pData, size_t nWhere=0 );
            virtual bool    EnumCapValues( void *pCapData );

        private:
            LONG            m_lCapSupport;
    };
}
#endif
