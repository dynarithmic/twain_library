/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2018 Dynarithmic Software.

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

    For more information, the license file LICENSE.TXT that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef CTLCCErr_h_
#define CTLCCErr_h_

#include "ctlobstr.h"
#include "ctltwain.h"
namespace dynarithmic
{
    class CTL_CondCodeInfo
    {
        public:
            CTL_CondCodeInfo() : m_nCode(9999) { }
            CTL_CondCodeInfo(TW_UINT16 nCode, int nResourceID) :
                        m_nCode(nCode), m_nResourceID(nResourceID) {}
            TW_UINT16       m_nCode;
            int             m_nResourceID;
            bool        IsValidCode() { return (m_nCode != 9999); }
    };
}
#endif
