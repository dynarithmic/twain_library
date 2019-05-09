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
#include <boost/format.hpp>
#include "ctliface.h"
#include "ctltwmgr.h"
#include "twilres.h"
#include "dtwdecl.h"

using namespace std;
using namespace dynarithmic;

static void LogExceptionToConsole(LPCTSTR fname);

CTL_StringType dynarithmic::CTL_LogFunctionCallA(LPCSTR pFuncName, int nWhich, LPCSTR pOptionalString/* = NULL*/)
{
    CTL_StringType ret;
    if (pOptionalString)
        ret = CTL_LogFunctionCall(StringConversion::Convert_AnsiPtr_To_Native(pFuncName).c_str(),
                                  nWhich, StringConversion::Convert_AnsiPtr_To_Native(pOptionalString).c_str());
    else
        ret = CTL_LogFunctionCall(StringConversion::Convert_AnsiPtr_To_Native(pFuncName).c_str(), nWhich );
    return ret;
}

CTL_StringType dynarithmic::CTL_LogFunctionCall(LPCTSTR pFuncName, int nWhich, LPCTSTR pString/*=NULL*/)
{
    if (CTL_TwainDLLHandle::s_lErrorFilterFlags == 0 )
         return CTL_StringType();
    static int nIndent = 0;
    CTL_StringType s;
    CTL_StringType s2;
    if ( pString )
        s2 = pString;
    if ( nWhich != LOG_NO_INDENT )
    {
        if ( nWhich == 0 || nWhich == LOG_INDENT_IN)
        {
            CTL_StringType sTemp(nIndent, _T(' '));
            s = sTemp + (CTL_StringType)_T("===>>>") + CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_ENTERTEXT] + _T(" ");
            nIndent += 3;
        }
        else
        {
            nIndent -= 3;
            nIndent = max(0, nIndent);
            CTL_StringType sTemp(nIndent, _T(' '));
            s = sTemp + (CTL_StringType)_T("<<<===") + CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_EXITTEXT] + _T(" ");
        }
    }
    else
    {
        CTL_StringType sTemp(nIndent, _T(' '));
        s = sTemp;
    }
    if ( !pString )
        s += pFuncName;
    else
        s += s2;
    if ( nWhich != LOG_INDENT_IN && nWhich != LOG_INDENT_OUT)
    {
        if (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_USEFILE)
        {
            if (!CTL_TwainDLLHandle::s_appLog.StatusOutFast( s.c_str() ) )
                CTL_TwainDLLHandle::s_appLog.OutputDebugStringFull(s.c_str());
        }
        else
        {
            if ( CTL_TwainDLLHandle::s_lErrorFilterFlags )
                CTL_TwainDLLHandle::s_appLog.OutputDebugStringFull(s.c_str());
        }
    }
    return s;
}

void dynarithmic::LogExceptionErrorA(LPCSTR fname)
{
    LogExceptionError(StringConversion::Convert_AnsiPtr_To_Native(fname).c_str());
}

void dynarithmic::LogExceptionError(LPCTSTR fname)
{
    if ( CTL_TwainDLLHandle::s_lErrorFilterFlags == 0 )
         return;
    try {
       CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
       if ( pHandle )
            pHandle->m_lLastError = DTWAIN_ERR_EXCEPTION_ERROR;
       CTL_StringStreamType output;

       output << _T("**** DTWAIN ") << CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_EXCEPTERRORTEXT] <<  _T(" ****.  ") <<
                 CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_MODULETEXT] << _T(": ") <<  fname;

       CTL_StringType s = output.str();
       if (!(CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_USEFILE))
            s += _T("\n");
       CTL_TwainAppMgr::WriteLogInfo(s, true);  // flush all writes to the log file
       if ( CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_SHOWEXCEPTIONS)
           LogExceptionToConsole(fname);
    }
    catch(...)
    {
        LogExceptionToConsole(fname);
    }
}

void LogExceptionToConsole(LPCTSTR fname)
{
    try
    {
        #ifdef UNICODE
            std::wostringstream strm;
            strm << boost::wformat(_T("**** DTWAIN %1% ****.  %2%: %3%\n")) %
                CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_EXCEPTERRORTEXT].c_str() %
                CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_MODULETEXT].c_str() % fname;
        #else
            std::ostringstream strm;
            strm << boost::format("**** DTWAIN %1% ****.  %2%: %3%\n") %
                CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_EXCEPTERRORTEXT].c_str() %
                CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_MODULETEXT].c_str() % fname;
        #endif
        #ifdef _WIN32
           ::MessageBox(NULL, strm.str().c_str(), CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_EXCEPTERRORTEXT].c_str(), MB_ICONSTOP);
        #else
        #ifdef _UNICODE
           std::wcout << strm.str() << L'\n';
        #else
           std::cout << strm.str() << '\n';
        #endif
        #endif
    }
    catch (...) {}  // can't really do anything
}
