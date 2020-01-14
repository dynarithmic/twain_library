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
#ifndef CAPSTRUCT_H_
#define CAPSTRUCT_H_

#include <unordered_map>
#include "dtwdecl.h"
namespace dynarithmic
{
    // Define the cap info structure used
    class CTL_CapStruct
    {
        public:
            CTL_CapStruct() : m_nDataType(0), m_nGetContainer(0), m_nSetContainer(0) {}
            UINT       m_nDataType;
            UINT       m_nGetContainer;
            UINT       m_nSetContainer;
            CTL_String m_strCapName;
            operator CTL_String();
    };

    typedef std::unordered_map<TW_UINT16, CTL_CapStruct> CTL_GeneralCapInfo;
}
#endif
