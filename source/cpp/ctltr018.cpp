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
#include <cstring>
#include "ctltr018.h"

#ifdef USE_EXPLICIT_TEMPLATE_INSTANTIATIONS
    #include "ctltr018.inl"
    template  CTL_CapabilitySetRangeTriplet<int>;
    template  CTL_CapabilitySetRangeTriplet<long>;
    template  CTL_CapabilitySetRangeTriplet<unsigned int>;
    template  CTL_CapabilitySetRangeTriplet<unsigned long>;
    template  CTL_CapabilitySetRangeTriplet<unsigned short>;
    template  CTL_CapabilitySetRangeTriplet<double>;
    template  CTL_CapabilitySetRangeTriplet<CTL_String>;
    template  CTL_CapabilitySetRangeTriplet<char *>;
    template  CTL_CapabilitySetRangeTriplet<TW_FRAME>;
#endif

