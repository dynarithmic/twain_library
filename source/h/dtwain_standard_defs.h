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
#ifndef DTWAIN_STANDARD_DEFS_H
#define DTWAIN_STANDARD_DEFS_H

#ifdef _WIN32
    #include <windows.h>
    #include <shlwapi.h>
    #include <stdint.h>
    #include <tchar.h>
#else
    #include <stdint.h>
    #define DECLARE_HANDLE(name) struct name##__{int unused;}; typedef struct name##__ *name
    typedef void VOID;
    typedef unsigned short WORD;
    typedef WORD *LPWORD;
    typedef uint32_t UINT;
    typedef unsigned long ULONG32;
    typedef int32_t  LONG;
    typedef uint32_t DWORD;
    typedef int64_t  LONG64;
    typedef long long LONGLONG;
    typedef char *   LPSTR;
    typedef const char * LPCSTR;
    typedef wchar_t* LPWSTR;
    typedef const wchar_t* LPCWSTR;
    typedef char CHAR;
    typedef unsigned char BYTE;
    typedef unsigned char* LPBYTE;
    typedef void * HANDLE;
    typedef void * HMODULE;
    typedef void * LPVOID;
    typedef const void * LPCVOID;
    typedef UINT   UINT32;
    typedef LONG    BOOL;
    typedef LONG* LPLONG;
    typedef int64_t LONG_PTR;
    typedef LONG_PTR LRESULT;
    typedef HANDLE  HWND;
    typedef HANDLE  HINSTANCE;
    typedef HANDLE  HGLOBAL;
    typedef HANDLE  HBITMAP;
    typedef HANDLE  HDC;
    typedef unsigned short  SHORT;
    typedef unsigned long ULONG;
    typedef int64_t  DWORD_PTR;
    typedef unsigned char UCHAR;
    typedef DWORD_PTR ULONG_PTR;
    typedef ULONG_PTR   SIZE_T;
    typedef int64_t LONG_PTR;
    struct OPENFILENAME {};
    typedef OPENFILENAME* LPOPENFILENAME;
    DECLARE_HANDLE(HHOOK);
    DECLARE_HANDLE(HRGN);
    DECLARE_HANDLE(HGDIOBJ);
    DECLARE_HANDLE(HPALETTE);
    #ifdef UNICODE
    typedef wchar_t TCHAR;
        typedef LPWSTR LPTSTR;
        typedef LPCWSTR LPCTSTR;
    #define _T(x) L##x
    #else
    typedef char TCHAR;
        typedef LPSTR LPTSTR;
        typedef LPCSTR LPCTSTR;
        #define _T(x) x
    #endif
    typedef uint64_t UINT_PTR;
    typedef uint64_t WPARAM;
    typedef uint64_t LPARAM;
    #define FAR
    #define CALLBACK
    #define DLLENTRY_DEF
    #define GPTR 0
    #define GHND 0
    #define GMEM_SHARE 0
    #define GMEM_ZEROINIT 1
    #define MAKEWORD(a, b)      ((WORD)(((BYTE)(((DWORD_PTR)(a)) & 0xff)) | ((WORD)((BYTE)(((DWORD_PTR)(b)) & 0xff))) << 8))
    #define MAKELONG(a, b)      ((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
    #define LOWORD(l)           ((WORD)(((DWORD_PTR)(l)) & 0xffff))
    #define HIWORD(l)           ((WORD)((((DWORD_PTR)(l)) >> 16) & 0xffff))
    #define LOBYTE(w)           ((BYTE)(((DWORD_PTR)(w)) & 0xff))
    #define HIBYTE(w)           ((BYTE)((((DWORD_PTR)(w)) >> 8) & 0xff))
    typedef struct tagPOINT
    {
        LONG  x;
        LONG  y;
    } POINT, *PPOINT, *LPPOINT;

    typedef struct tagMSG
    {
        HWND hwnd;
        UINT message;
        WPARAM wParam;
        LPARAM lParam;
        DWORD time;
        POINT pt;
    }   MSG, *LPMSG;

    typedef struct tagRECT
    {
        LONG    left;
        LONG    top;
        LONG    right;
        LONG    bottom;
    } RECT, *PRECT, *LPRECT;
#endif

#endif
