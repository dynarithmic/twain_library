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

    For more information, the license file named LICENSE that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef CTLTR016_H_
#define CTLTR016_H_
#include <vector>
#include "ctltr010.h"
#include "ctlenum.h"
namespace dynarithmic
{
    class CTL_CapabilitySetTripletBase : public CTL_CapabilityTriplet
    {
        public:
            CTL_EnumSetType           CapSetType() const;
            CTL_EnumCapability        CapToSet() const;
            TW_UINT16                 GetTwainType() const;

            CTL_CapabilitySetTripletBase(CTL_ITwainSession *pSession,
                                         CTL_ITwainSource* pSource,
                                         CTL_EnumSetType sType,
                                         TW_UINT16      sCap,
                                         TW_UINT16 TwainType);

        protected:
            virtual TW_UINT16       GetContainerTypeSize() { return 0; }
            virtual size_t          GetAggregateSize() { return 0; }
            virtual TW_UINT16       GetContainerType() { return 0; }

            void * PreEncode();
            TW_UINT16 PostEncode(TW_UINT16);

            void EncodeOneValue(pTW_ONEVALUE pVal, void *pData);

            void EncodeRange(pTW_RANGE pVal,
                            void *pData1,
                            void *pData2,
                            void *pData3);

            void EncodeEnumValue(pTW_ENUMERATION pArray,
                                int valuePos,
                                size_t nItemSize,
                                void *pData);

            void EncodeArrayValue(pTW_ARRAY pArray,
                                  size_t valuePos,
                                  void *pData);

        private:
            CTL_EnumSetType         m_gType;
            CTL_EnumCapability      m_gCap;
            TW_UINT16               m_nTwainType;
    };


    template <class T>
    class CTL_CapabilitySetTriplet : public CTL_CapabilitySetTripletBase
    {
        public:
            CTL_CapabilitySetTriplet(CTL_ITwainSession *pSession,
                                     CTL_ITwainSource* pSource,
                                     CTL_EnumSetType sType,
                                     TW_UINT16  sCap,
                                     TW_UINT16 TwainType,
                                     const std::vector<T> & rArray);

            TW_UINT16                Execute();

        protected:
            virtual bool Encode( const std::vector<T>& /*rArray*/, void * /*pMemBlock*/) { return false; }

        private:
            std::vector<T>              m_Array;
    };

    #ifndef USE_EXPLICIT_TEMPLATE_INSTANTIATIONS
    #include "../inl/ctltr016.inl"
    #endif

    class CTL_CapabilityResetTriplet : public CTL_CapabilityTriplet
    {
        public:

            CTL_CapabilityResetTriplet(CTL_ITwainSession *pSession,
                                       CTL_ITwainSource* pSource,
                                       CTL_EnumCapability sCap,
                                       TW_UINT16 SetType = CTL_SetTypeRESET
                                       );
    };

    class CTL_CapabilityResetAllTriplet : public CTL_CapabilityResetTriplet
    {
        public:
            CTL_CapabilityResetAllTriplet(CTL_ITwainSession *pSession,
                CTL_ITwainSource* pSource,
                CTL_EnumCapability sCap) : CTL_CapabilityResetTriplet(pSession, pSource, sCap, CTL_SetTypeRESETALL) {}
    };
}
#endif
