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
	#elif _MSC_VER >= 1920
		#pragma message ("Microsoft Visual Studio 2019 compiler defined")
    #endif
#endif

#ifndef _MSC_VER
    #pragma message("Unsupported compiler being used to compile DTWAIN")
#endif

#if defined (UNICODE) || defined (_UNICODE)
    #pragma message ("DTWAIN Library using Unicode is active")
#else
    #pragma message ("DTWAIN Library using ANSI/MBCS is active")
#endif

#if defined(DTWAIN_STDCALL) || !defined(DTWAIN_LIB)
    #undef CALLCONVENTION_DEF
    #define CALLCONVENTION_DEF __stdcall
    #pragma message ("DTWAIN Using __stdcall calling convention")
#else
    #undef CALLCONVENTION_DEF
    #define CALLCONVENTION_DEF __cdecl
    #pragma message ("DTWAIN Using __cdecl calling convention")
#endif

#if defined(DTWAIN_LIB)
    #define DLLENTRY_DEF CALLCONVENTION_DEF
    #define EXPORTDEF
    #define DLLEXORTDEF
    #define CALLBACK_DEF  __stdcall
    #if defined (WIN64) || defined (_WIN64)
        #pragma message("DTWAIN static LIB for Win64")
        #define IMGFUNC_DEF  __stdcall
    #else
    #if defined(WIN32) || defined(_WIN32)
         #pragma message("DTWAIN static LIB for Win32")
        #define IMGFUNC_DEF  __stdcall
    #else
         #pragma message("DTWAIN static LIB for Win16")
        #define IMGFUNC_DEF  FAR PASCAL
    #endif
    #endif
#else
    #undef CALLCONVENTION_DEF
    #define CALLCONVENTION_DEF __stdcall
    #define DECLSPEC_DEF
    #define CALLBACK_DEF DECLSPEC_DEF __stdcall
    #if defined(WIN32) || defined(_WIN32) || defined (WIN64) || defined(_WIN64)
        #if defined(DTWAIN_DLL) || defined(DTWAIN_OCX) || defined(DTWAIN_VB)
            #define DLLENTRY_DEF DECLSPEC_DEF CALLCONVENTION_DEF
            #define IMGFUNC_DEF  __stdcall
            #if defined(WIN64) || defined (_WIN64)
                #pragma message("Building 64-bit DTWAIN DLL")
            #else
            #pragma message("Building 32-bit DTWAIN DLL, OCX or VB version")
            #endif
        #else
            #define DLLENTRY_DEF __stdcall
            #define IMGFUNC_DEF __declspec(dllexport) CALLCONVENTION_DEF
            #if defined (WIN64) || defined(_WIN64)
                #pragma message("Including 64-bit DTWAIN DLL definitions")
            #else
            #pragma message("Including 32-bit DTWAIN DLL definitions")
    #endif
        #endif
    #else
        #if defined(DTWAIN_DLL) || defined(DTWAIN_OCX) || defined(DTWAIN_VB)
            #define DLLENTRY_DEF __export FAR PASCAL
            #define IMGFUNC_DEF FAR PASCAL
            #pragma message("Building 16-bit DTWAIN DLL, OCX or VB version")
        #else
            #define DLLENTRY_DEF FAR PASCAL
            #define IMGFUNC_DEF __export FAR PASCAL
            #pragma message("Including 16-bit DTWAIN DLL definitions")
        #endif
    #endif
#endif
#endif
#define HUGEDEF
typedef unsigned char HUGEDEF* HUGEPTR_CHAR;
#endif
