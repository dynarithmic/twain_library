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
#ifndef FUNCMAC_H
#define FUNCMAC_H
    #define UNUSED_PARAM(expr) do { (void)(expr); } while (0)
    #include "dtwain_standard_defs.h"
    #ifndef DTWAIN_LEAN_AND_MEAN
        #ifdef FUNCTION_MACRO_DEFINED
            #pragma message("   Compiler defines __FUNCTION__")
            #define FUNC_MACRO   __FUNCTION__
            #define FUNC_HEADER(x)
        #else
        #ifdef FUNC_MACRO_DEFINED
            #pragma message("Compiler defines __FUNC__")
            #define FUNC_MACRO   __FUNC__
            #define FUNC_HEADER(x)
        #else
            #pragma message("Compiler does not define __FUNCTION__ or __FUNC__")
            #define FUNC_HEADER(x) static const TCHAR funcname_[] = _T(#x);
            #define __FUNC__    (funcname_)
            #define FUNC_MACRO  __FUNC__
            #define FUNC_MACRO_DEFINED
        #endif
        #endif
    #else
        #define FUNC_HEADER(x)
    #endif
#endif
