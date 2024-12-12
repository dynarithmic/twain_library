/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2025 Dynarithmic Software.

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
#ifndef DTWAINC_H
#define DTWAINC_H

#include "dtwtype.h"

/* Callback functions */
typedef void (*DTWAIN_CALLBACK)(DTWAIN_HANDLE, DTWAIN_SOURCE, WPARAM, LPARAM);

/* Callback function definition for DTWAIN_SetCallback */
#ifdef _WIN64
typedef LRESULT (CALLBACK *DTWAIN_CALLBACK_PROC)(WPARAM, LPARAM, LONG_PTR);
#else
typedef LRESULT (CALLBACK *DTWAIN_CALLBACK_PROC)(WPARAM, LPARAM, LONG);
#endif
typedef LRESULT (CALLBACK *DTWAIN_CALLBACK_PROC64)(WPARAM, LPARAM, LONGLONG);

/* Callback function to allow application to change DIB that is acquired */
typedef HANDLE (CALLBACK *DTWAIN_DIBUPDATE_PROC)(DTWAIN_SOURCE, LONG, HANDLE);

/* Callback to allow sending of log message to application */
typedef LRESULT (CALLBACK *DTWAIN_LOGGER_PROC)(LPCTSTR, DTWAIN_LONG64);
typedef LRESULT (CALLBACK *DTWAIN_LOGGER_PROCA)(LPCSTR, DTWAIN_LONG64);
typedef LRESULT (CALLBACK *DTWAIN_LOGGER_PROCW)(LPCWSTR, DTWAIN_LONG64);

/* Callback to allow only sending the error code to application */
typedef LRESULT(CALLBACK *DTWAIN_ERROR_PROC)(LONG, LONG);
typedef LRESULT(CALLBACK *DTWAIN_ERROR_PROC64)(LONG, LONG64);
#endif
