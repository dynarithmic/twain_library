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
#ifndef CTLTR011_H_
#define CTLTR011_H_
#include "ctltr010.h"
namespace dynarithmic
{
    class CTL_CapabilityGetTriplet : public CTL_CapabilityTriplet
    {
        public:
            CTL_CapabilityGetTriplet(CTL_ITwainSession      *pSession,
                                     CTL_ITwainSource       *pSource,
                                     CTL_EnumGetType  gType,
                                     TW_UINT16 gCap,
                                     TW_UINT16 TwainDataType=0xFFFF);

            virtual size_t          GetNumItems() {return 1;}

            virtual bool            GetValue( void * /*pData*/, size_t /*nWhere*/ =0 ){ return false; }
            CTL_EnumContainer       GetSupportedContainer() const { return m_nContainerToUse; }
            TW_UINT16               Execute();


        protected:
            CTL_EnumGetType         CapRetrievalType() const;
            CTL_EnumCapability      CapToRetrieve()    const;

            virtual bool            EnumCapValues( void * /*pCapData*/) { return false; }

            void    Decode(void *p);

            TW_UINT16 GetEffectiveItemType(TW_UINT16 curDataType) const;

        private:
            TW_UINT16               m_gCap;
            CTL_EnumGetType         m_gType;
            CTL_EnumContainer       m_nContainerToUse;
    };
}
#endif
