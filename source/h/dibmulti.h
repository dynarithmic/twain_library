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
#ifndef _DIBMULTI_H
#define _DIBMULTI_H

#define DIB_MULTI_FIRST     1
#define DIB_MULTI_NEXT      2
#define DIB_MULTI_LAST      3

#include <string>
#include "ctlobstr.h"
namespace dynarithmic
{
    struct DibMultiPageStruct
    {
        int Stage;
        int Page;
        void *pUserData;
        CTL_StringType strName;
        DibMultiPageStruct() : Stage(0), Page(0), pUserData(NULL) { }
    };
}
#endif
