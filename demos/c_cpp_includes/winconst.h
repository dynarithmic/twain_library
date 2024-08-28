/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2024 Dynarithmic Software.

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
#ifndef WINCONST_H
#define WINCONST_H

#ifndef _WIN32
    #include "dtwain_standard_defs.h"
#else
#ifndef __WIN32__
  #ifndef WIN32
    #define WINDOWS_16
  #else
    #define __WIN32__
  #endif
#endif

#if defined (WIN32) || defined (_WIN64)
    #define DllExport       __declspec( dllexport )
    //#define HUGEDEF
    #define DLLEXPORTDEF    DllExport
    #define EXPORTDEF
#else
#ifdef WINDOWS_16
    //#define HUGEDEF         huge
    #define DLLEXPORTDEF
    #define EXPORTDEF       _export
    #endif
#endif
#endif

#ifdef DLLENTRY_DEF
    #undef DLLENTRY_DEF
#endif

#ifdef IMGFUNC_DEF
    #undef IMGFUNC_DEF
#endif

#define CALLCONVENTION_DEF

#ifdef _MSC_VER
    #if _MSC_VER < 1800
        #error("Compiler must be Visual Studio 2013 or greater")
    #elif _MSC_VER == 1800
    #pragma message ("Microsoft Visual Studio 2013 compiler defined")
    #elif _MSC_VER == 1900
        #pragma message ("Microsoft Visual Studio 2015 compiler defined")
    #elif _MSC_VER >= 1910 && _MSC_VER < 1920
        #pragma message ("Microsoft Visual Studio 2017 compiler defined")
    #elif _MSC_VER >= 1920 && _MSC_VER < 1930
        #pragma message ("Microsoft Visual Studio 2019 compiler defined")
    #elif _MSC_VER >= 1930
        #pragma message ("Microsoft Visual Studio 2022 (or greater) compiler defined")
    #endif
#endif

#ifdef _MSC_VER
    #ifdef BUILDING_DTWAINDLL
        #if _MSC_VER < 1920
            #error("Visual C++ compiler must be Visual Studio 2019 or greater to build DTWAIN")
        #endif
        #if (__cplusplus < 201703L)
            #error("Visual C++ Compiler must use C++17 standard or greaater to build DTWAIN")
        #endif
    #endif
#endif


#ifdef _MSC_VER
    #if defined (UNICODE) || defined (_UNICODE)
        #pragma message ("DTWAIN Library using Unicode is active")
    #else
        #pragma message ("DTWAIN Library using ANSI/MBCS is active")
    #endif
#endif

#if defined(DTWAIN_STDCALL) || !defined(DTWAIN_LIB)
    #undef CALLCONVENTION_DEF
    #define CALLCONVENTION_DEF __stdcall
    #ifdef _MSC_VER
        #pragma message ("DTWAIN Using __stdcall calling convention")
    #endif
#else
    #undef CALLCONVENTION_DEF
    #define CALLCONVENTION_DEF __cdecl
    #ifdef _MSC_VER
        #pragma message ("DTWAIN Using __cdecl calling convention")
    #endif
#endif

#ifdef _MSC_VER
    #ifdef BUILDING_DTWAINDLL
        #ifdef _DEBUG
            #pragma message ("DTWAIN Debug Library building...")
        #else
            #pragma message ("DTWAIN Release Library building...")
        #endif
    #endif
#endif

#undef CALLCONVENTION_DEF
#define CALLCONVENTION_DEF __stdcall
#define DECLSPEC_DEF
#define CALLBACK_DEF DECLSPEC_DEF __stdcall
#if defined(WIN32) || defined(_WIN32) || defined (WIN64) || defined(_WIN64)
    #ifdef DTWAIN_DLL
        #define DLLENTRY_DEF DECLSPEC_DEF CALLCONVENTION_DEF
        #define IMGFUNC_DEF  __stdcall
        #ifdef _MSC_VER
            #if defined(WIN64) || defined (_WIN64)
                #pragma message("Building 64-bit DTWAIN DLL")
            #else
                #pragma message("Building 32-bit DTWAIN DLL")
            #endif
        #endif
    #else
        #define DLLENTRY_DEF __stdcall
        #define IMGFUNC_DEF __declspec(dllexport) CALLCONVENTION_DEF
        #ifdef _MSC_VER
            #if defined (WIN64) || defined(_WIN64)
                #pragma message("Including 64-bit DTWAIN DLL definitions")
            #else
                #pragma message("Including 32-bit DTWAIN DLL definitions")
            #endif
        #endif
    #endif
#endif
#define HUGEDEF
typedef unsigned char HUGEDEF* HUGEPTR_CHAR;
#endif
