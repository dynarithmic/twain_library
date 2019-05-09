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
#ifndef CTLTMPL4_H_
#define CTLTMPL4_H_

#include "tr1defs.h"
#include <algorithm>

namespace dynarithmic
{
    template <class P1, class P2, class P3, class P4, class P5, class P6, class P7>
    struct CTL_ClassValues7 : public std::tuple<P1, P2, P3, P4, P5, P6, P7>
    {
        typedef CTL_ClassValues7<P1,P2,P3,P4,P5,P6,P7> Tuple7;
        CTL_ClassValues7(P1 a1, P2 a2, P3 a3, P4 a4, P5 a5, P6 a6, P7 a7) :
                std::tuple<P1, P2, P3, P4, P5, P6, P7>(a1, a2, a3, a4, a5, a6, a7), m_bValid(true) {}

        CTL_ClassValues7() : std::tuple<P1, P2, P3, P4, P5, P6, P7>(), m_bValid(true) {}
        bool IsValid() const { return m_bValid; }
        void SetValid(bool bSet=true) { m_bValid = bSet; }

        private:
             bool m_bValid;
    };
}
#endif
