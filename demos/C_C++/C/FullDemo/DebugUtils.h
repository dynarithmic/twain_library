/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2026 Dynarithmic Software.

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
#ifndef DEBUGUTILS_H
#define DEBUGUTILS_H

#include "dtwaindefs.h"
#include "dtwain_standard_defs.h"
#include "dtwtype.h"

#ifdef __cplusplus
extern "C" {
#endif
    enum
    {
        DTWAIN_DBG_ARRAY_UNKNOWN = 0,
        DTWAIN_DBG_ARRAY_LONG = DTWAIN_ARRAYLONG,
        DTWAIN_DBG_ARRAY_LONGLONG = DTWAIN_ARRAYLONG64,
        DTWAIN_DBG_ARRAY_FLOAT = DTWAIN_ARRAYFLOAT,
        DTWAIN_DBG_ARRAY_ASTRING = DTWAIN_ARRAYANSISTRING,
        DTWAIN_DBG_ARRAY_WIDESTRING = DTWAIN_ARRAYWIDESTRING,
        DTWAIN_DBG_ARRAY_SOURCE = DTWAIN_ARRAYSOURCE,
        DTWAIN_DBG_ARRAY_ARRAY = DTWAIN_ARRAYOFHANDLEARRAYS,
        DTWAIN_DBG_ARRAY_HANDLE = DTWAIN_ARRAYHANDLE,
        DTWAIN_DBG_ARRAY_FRAME = DTWAIN_ARRAYFRAME
    };

    typedef struct tagDTWAIN_FRAME_DEBUG_ITEM
    {
        double left;
        double top;
        double right;
        double bottom;
    } DTWAIN_FRAME_DEBUG_ITEM;

    typedef struct tagDTWAIN_SOURCE_DEBUG_VIEW
    {
        const char* json;
    } DTWAIN_SOURCE_DEBUG_VIEW;

    typedef struct tagDTWAIN_ARRAY_DEBUG_VIEW
    {
        long count;
        long type;

        const LONG* long_data;
        const LONGLONG* longlong_data;
        const double* float_data;

        const char** astring_ptrs;
        const wchar_t** widestring_ptrs;

        const struct tagDTWAIN_SOURCE_DEBUG_VIEW* source_views;
        const struct tagDTWAIN_ARRAY_DEBUG_VIEW* nested_views;

        const void** handle_data;
        const struct tagDTWAIN_FRAME_DEBUG_ITEM* frame_items;
    } DTWAIN_ARRAY_DEBUG_VIEW;

    DTWAIN_ARRAY_DEBUG_VIEW DTWAIN_CreateArrayDebugView(DTWAIN_ARRAY a);
    DTWAIN_SOURCE_DEBUG_VIEW DTWAIN_CreateSourceDebugView(DTWAIN_SOURCE s);

    #define DTWAIN_DEBUG_VIEW(a) DTWAIN_CreateArrayDebugView((a))
    #define DTWAIN_DEBUG_SOURCE_VIEW(s) DTWAIN_CreateSourceDebugView((s))

#ifdef __cplusplus
}
#endif
#endif
