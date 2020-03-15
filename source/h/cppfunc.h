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
#ifndef CPPFUNC_H_
#define CPPFUNC_H_

#include "dtwain_retail_def.h"
#include <funcmac.h>
#define LOG_INDENT_CONSOLE 0
#define LOG_NO_INDENT   1
#define LOG_INDENT_IN   2
#define LOG_INDENT_OUT  3

#define NAG_FOR_LICENSE (0)

#define THROW_EXCEPTION \
    { if ( CTL_TwainDLLHandle::s_bThrowExceptions )  DTWAIN_InternalThrowException(); }

     #define STRING_PARAM_LIST(x) _T(#x)

#ifndef DTWAIN_LEAN_AND_MEAN
    #ifndef DTWAIN_NO_LOGGING
        #define TRY_BLOCK try {

        #define LOG_FUNC_STRING(x) \
            if ( CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK) \
            CTL_LogFunctionCall(_T(""), LOG_INDENT_CONSOLE, _T(#x));

        #define LOG_FUNC_VALUES(x) \
            if ( CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK) \
            CTL_LogFunctionCall(_T(""), LOG_INDENT_CONSOLE, (x));

        #define LOG_FUNC_ENTRY_PARAMS(argVals) \
            TRY_BLOCK \
            if (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK) \
            CTL_TwainAppMgr::WriteLogInfo(CTL_LogFunctionCallA(FUNC_MACRO,LOG_INDENT_IN) +  ParamOutputter(_T(#argVals)).outputParam argVals.getString());

        #define LOG_FUNC_EXIT_PARAMS(x) { \
            if (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK) \
            CTL_TwainAppMgr::WriteLogInfo(CTL_LogFunctionCallA(FUNC_MACRO, LOG_INDENT_OUT) + ParamOutputter(_T(""), true).outputParam(x).getString()); \
            return(x); \
                }

        #define LOG_FUNC_VALUES_EX(argvals) { \
            if (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK) \
            CTL_TwainAppMgr::WriteLogInfo(CTL_LogFunctionCall(_T(""),LOG_INDENT_IN) + ParamOutputter(_T(#argvals)).outputParam argvals.getString()); \
        }

        #define CATCH_BLOCK(type) \
                } \
                catch(decltype(type) var) { return var; }\
                catch(...) {\
                LogExceptionErrorA(FUNC_MACRO); \
                THROW_EXCEPTION \
                return(type); \
                }
        #else
            #define TRY_BLOCK try {
            #define LOG_FUNC_STRING(x)

            #define LOG_FUNC_VALUES(x)

            #define LOG_FUNC_ENTRY_PARAMS(argVals) TRY_BLOCK

            #define LOG_FUNC_ENTRY_PARAMS_NO_CHECK(argvals) TRY_BLOCK

            #define LOG_FUNC_EXIT_PARAMS(x) { return(x); }

            #define LOG_FUNC_VALUES_EX(argvals)

            #define CATCH_BLOCK(type) \
                } \
                catch(decltype(type) var) { return var; }\
                catch(...) {\
                THROW_EXCEPTION \
                return(type); \
                }
        #endif
#else
    #define TRY_BLOCK

    #define LOG_FUNC_STRING(x)

    #define LOG_FUNC_VALUES(x)

    #define LOG_FUNC_VALUES_EX(x, argtype)

    #define LOG_FUNC_EXIT_PARAMS(x, argtype) { return (x); }

    #define CATCH_BLOCK(type)

    #define LOG_FUNC_ENTRY_PARAMS_NO_CHECK(argvals)

#endif
#endif

