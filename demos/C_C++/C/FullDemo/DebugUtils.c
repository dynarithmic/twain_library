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
#if defined(_MSC_VER)
    #define _CRT_SECURE_NO_WARNINGS
#endif

#include <wchar.h>
#include "DebugUtils.h"
#include "dtwain.h"

#if defined(_MSC_VER)
    #define DTWAIN_DBG_THREAD_LOCAL __declspec(thread)
#else
    #define DTWAIN_DBG_THREAD_LOCAL _Thread_local
#endif

#define DTWAIN_DBG_MAX_ITEMS         256
#define DTWAIN_DBG_MAX_ANSI_CHARS    512
#define DTWAIN_DBG_MAX_WIDE_CHARS    512
#define DTWAIN_DBG_SOURCE_JSON_CHARS 4096


static DTWAIN_DBG_THREAD_LOCAL char g_astring_buffers[DTWAIN_DBG_MAX_ITEMS][DTWAIN_DBG_MAX_ANSI_CHARS];
static DTWAIN_DBG_THREAD_LOCAL const char* g_astring_ptrs[DTWAIN_DBG_MAX_ITEMS];

static DTWAIN_DBG_THREAD_LOCAL wchar_t g_widestring_buffers[DTWAIN_DBG_MAX_ITEMS][DTWAIN_DBG_MAX_WIDE_CHARS];
static DTWAIN_DBG_THREAD_LOCAL const wchar_t* g_widestring_ptrs[DTWAIN_DBG_MAX_ITEMS];

static DTWAIN_DBG_THREAD_LOCAL wchar_t g_source_name_buffers[DTWAIN_DBG_MAX_ITEMS][DTWAIN_DBG_MAX_WIDE_CHARS];
static DTWAIN_DBG_THREAD_LOCAL const wchar_t* g_source_name_ptrs[DTWAIN_DBG_MAX_ITEMS];

static DTWAIN_DBG_THREAD_LOCAL DTWAIN_ARRAY_DEBUG_VIEW g_nested_views[DTWAIN_DBG_MAX_ITEMS];
static DTWAIN_DBG_THREAD_LOCAL DTWAIN_FRAME_DEBUG_ITEM g_frame_items[DTWAIN_DBG_MAX_ITEMS];

/* Source JSON buffers/views */
static DTWAIN_DBG_THREAD_LOCAL char g_source_json_buffers[DTWAIN_DBG_MAX_ITEMS][DTWAIN_DBG_SOURCE_JSON_CHARS];
static DTWAIN_DBG_THREAD_LOCAL DTWAIN_SOURCE_DEBUG_VIEW g_source_views[DTWAIN_DBG_MAX_ITEMS];

static void dtwain_dbg_copy_str(char* dest, size_t destCount, const char* src)
{
    size_t i;
    if (!dest || destCount == 0)
        return;
    if (!src)
        src = "<null>";
    for (i = 0; i + 1 < destCount && src[i] != 0; ++i)
        dest[i] = src[i];
    dest[i] = 0;
}

static void dtwain_dbg_copy_wstr(wchar_t* dest, size_t destCount, const wchar_t* src)
{
    size_t i;
    if (!dest || destCount == 0)
        return;
    if (!src)
        src = L"<null>";
    for (i = 0; i + 1 < destCount && src[i] != 0; ++i)
        dest[i] = src[i];
    dest[i] = 0;
}

static DTWAIN_SOURCE_DEBUG_VIEW DTWAIN_CreateSourceDebugViewAt(DTWAIN_SOURCE s, int slot)
{
    DTWAIN_SOURCE_DEBUG_VIEW v;
    char* buf;

    if (slot < 0 || slot >= DTWAIN_DBG_MAX_ITEMS)
    {
        v.json = "<invalid slot>";
        return v;
    }

    buf = g_source_json_buffers[slot];
    v.json = buf;
    buf[0] = '\0';

    if (!s)
    {
        strcpy(buf, "<null DTWAIN_SOURCE>");
        return v;
    }

    if (DTWAIN_GetAllSourceInfoA(s, buf, 2, DTWAIN_DBG_SOURCE_JSON_CHARS) == 0 || buf[0] == '\0')
        strcpy(buf, "<unable to retrieve source info>");

    return v;
}

DTWAIN_SOURCE_DEBUG_VIEW DTWAIN_CreateSourceDebugView(DTWAIN_SOURCE s)
{
    return DTWAIN_CreateSourceDebugViewAt(s, 0);
}

DTWAIN_ARRAY_DEBUG_VIEW DTWAIN_CreateArrayDebugView(DTWAIN_ARRAY a)
{
    DTWAIN_ARRAY_DEBUG_VIEW v;
    long i, visibleCount;
    LONG arrType;

    memset(&v, 0, sizeof(v));

    if (!a)
        return v;

    v.count = DTWAIN_ArrayGetCount(a);
    visibleCount = v.count;
    if (visibleCount > DTWAIN_DBG_MAX_ITEMS)
        visibleCount = DTWAIN_DBG_MAX_ITEMS;

    arrType = DTWAIN_ArrayGetType(a);

    switch (arrType)
    {
    case DTWAIN_ARRAYLONG:
        v.type = DTWAIN_DBG_ARRAY_LONG;
        v.long_data = (const LONG*)DTWAIN_ArrayGetBuffer(a, 0);
        break;

    case DTWAIN_ARRAYLONG64:
        v.type = DTWAIN_DBG_ARRAY_LONGLONG;
        v.longlong_data = (const LONGLONG*)DTWAIN_ArrayGetBuffer(a, 0);
        break;

    case DTWAIN_ARRAYFLOAT:
        v.type = DTWAIN_DBG_ARRAY_FLOAT;
        v.float_data = (const double*)DTWAIN_ArrayGetBuffer(a, 0);
        break;

    case DTWAIN_ARRAYANSISTRING:
        v.type = DTWAIN_DBG_ARRAY_ASTRING;
        for (i = 0; i < visibleCount; ++i)
        {
            const char* s = DTWAIN_ArrayGetAtANSIStringPtr(a, i);
            dtwain_dbg_copy_str(g_astring_buffers[i], DTWAIN_DBG_MAX_ANSI_CHARS, s);
            g_astring_ptrs[i] = g_astring_buffers[i];
        }
        v.count = visibleCount;
        v.astring_ptrs = g_astring_ptrs;
        break;

    case DTWAIN_ARRAYWIDESTRING:
        v.type = DTWAIN_DBG_ARRAY_WIDESTRING;
        for (i = 0; i < visibleCount; ++i)
        {
            const wchar_t* s = DTWAIN_ArrayGetAtWideStringPtr(a, i);
            dtwain_dbg_copy_wstr(g_widestring_buffers[i], DTWAIN_DBG_MAX_WIDE_CHARS, s);
            g_widestring_ptrs[i] = g_widestring_buffers[i];
        }
        v.count = visibleCount;
        v.widestring_ptrs = g_widestring_ptrs;
        break;

    case DTWAIN_ARRAYSTRING:
    {
#if defined(UNICODE) || defined(_UNICODE)
        v.type = DTWAIN_DBG_ARRAY_WIDESTRING;
        for (i = 0; i < visibleCount; ++i)
        {
            const wchar_t* s = DTWAIN_ArrayGetAtWideStringPtr(a, i);
            dtwain_dbg_copy_wstr(g_widestring_buffers[i], DTWAIN_DBG_MAX_WIDE_CHARS, s);
            g_widestring_ptrs[i] = g_widestring_buffers[i];
        }
        v.count = visibleCount;
        v.widestring_ptrs = g_widestring_ptrs;
#else
        v.type = DTWAIN_DBG_ARRAY_ASTRING;
        for (i = 0; i < visibleCount; ++i)
        {
            const char* s = DTWAIN_ArrayGetAtANSIStringPtr(a, i);
            dtwain_dbg_copy_str(g_astring_buffers[i], DTWAIN_DBG_MAX_ANSI_CHARS, s);
            g_astring_ptrs[i] = g_astring_buffers[i];
        }
        v.count = visibleCount;
        v.astring_ptrs = g_astring_ptrs;
#endif
        break;
    }

    case DTWAIN_ARRAYFRAME:
    {
        v.type = DTWAIN_DBG_ARRAY_FRAME;

        for (i = 0; i < visibleCount; ++i)
        {
            double left = 0.0, top = 0.0, right = 0.0, bottom = 0.0;

            DTWAIN_ArrayGetAtFrame(a, i, &left, &top, &right, &bottom);

            g_frame_items[i].left = left;
            g_frame_items[i].top = top;
            g_frame_items[i].right = right;
            g_frame_items[i].bottom = bottom;
        }

        v.count = visibleCount;
        v.frame_items = g_frame_items;
        break;
    }

    case DTWAIN_ARRAYSOURCE:
        v.type = DTWAIN_DBG_ARRAY_SOURCE;
        for (i = 0; i < visibleCount; ++i)
        {
            DTWAIN_SOURCE src = ((DTWAIN_SOURCE*)DTWAIN_ArrayGetBuffer(a, 0))[i];
            g_source_views[i] = DTWAIN_CreateSourceDebugViewAt(src, (int)i);
        }
        v.count = visibleCount;
        v.source_views = g_source_views;
        break;

    case DTWAIN_ARRAYOFHANDLEARRAYS:
        v.type = DTWAIN_DBG_ARRAY_ARRAY;
        for (i = 0; i < visibleCount; ++i)
        {
            DTWAIN_ARRAY child = ((DTWAIN_ARRAY*)DTWAIN_ArrayGetBuffer(a, 0))[i];
            g_nested_views[i] = DTWAIN_CreateArrayDebugView(child);
        }
        v.count = visibleCount;
        v.nested_views = g_nested_views;
        break;

    case DTWAIN_ARRAYHANDLE:
        v.type = DTWAIN_DBG_ARRAY_HANDLE;
        v.handle_data = (const void**)DTWAIN_ArrayGetBuffer(a, 0);
        break;

    default:
        v.type = DTWAIN_DBG_ARRAY_UNKNOWN;
        break;
    }

    return v;
}

