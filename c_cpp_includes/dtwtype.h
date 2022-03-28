/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
#ifndef DTWTYPE_H
#define DTWTYPE_H

#ifdef _WIN32
    #include <windows.h>
#else
    #include "dtwain_standard_defs.h"
#endif

/* DTWAIN Types */
typedef void *   DTWAIN_HANDLE;
typedef void *   DTWAIN_ARRAY;
typedef DTWAIN_ARRAY  DTWAIN_RANGE;
typedef void *   DTWAIN_FRAME;
typedef DTWAIN_ARRAY  DTWAIN_FIX32;
typedef void *   DTWAIN_SOURCE;
typedef LONG        DTWAIN_ACQUIRE;
typedef LONG        DTWAIN_LONG;
typedef LONGLONG    DTWAIN_LONG64;
typedef DTWAIN_LONG64*  LPLONG64;
typedef HANDLE      DTWAIN_DIB;
typedef double      DTWAIN_FLOAT;
typedef char        DTWAIN_STRING[1026];
typedef void*       DTWAIN_PDFTEXTELEMENT;
typedef void*       DTWAIN_MEMORY_PTR;

/* Added for TWAIN 1.9 */
typedef char        DTWAIN_LONGSTRING[1026];
    typedef unsigned short DTWAIN_UNICODESTRING[512];

typedef DTWAIN_ARRAY FAR* LPDTWAIN_ARRAY;
typedef LONG        DTWAIN_BOOL;
typedef DTWAIN_FLOAT FAR * LPDTWAIN_FLOAT;
typedef void *   TWAIN_IDENTITY;
typedef TWAIN_IDENTITY DTWAIN_IDENTITY;
typedef DTWAIN_SOURCE           DTWAIN_OCRENGINE;
typedef DTWAIN_SOURCE           DTWAIN_OCRTEXTINFOHANDLE;
#define DTWAIN_ARRAYOCRENGINE   DTWAIN_ARRAYSOURCE
typedef void*   DTWAIN_OCRTEXTINFO;

#ifdef __cplusplus
  #define VOID_PROTOTYPE
#else
  #define VOID_PROTOTYPE void
#endif

#endif
