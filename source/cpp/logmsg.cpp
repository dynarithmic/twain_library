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

For more information, the license file named LICENSE that is located in the root
directory of the DTWAIN installation covers the restrictions under the LGPL license.
Please read this file before deploying or distributing any application using DTWAIN.
*/
#include "date/date.h"
#include <string.h>
#include <time.h>
#include <stdarg.h>
#include <stdio.h>
#include <sstream>
#include <stdio.h>
#ifdef _WIN32
#include <windows.h>
#include <mmSystem.h>
#include <tchar.h>
#else
#include <dlfcn.h>
#endif
#include <string>
#include "logmsg.h"
#include "ctlobstr.h"
#include "ctlfileutils.h"
#include <boost/log/trivial.hpp>

using namespace dynarithmic;
using namespace date;
using namespace std::chrono;

/////////////////////////////////////////////////////////////////////////////

namespace dynarithmic
{
    CTL_String CBaseLogger::getTime()
    {
        std::ostringstream strm;
        auto tp = std::chrono::system_clock::now();
        strm << "[" << tp << "]";
        return strm.str();
    }

    void CBaseLogger::generic_outstream(std::ostream& os, const CTL_String& msg)
    {
        os << msg << '\n';
    }

    void StdCout_Logger::trace(const CTL_String& msg)
    {
        CTL_String total = getTime() + msg;
        BOOST_LOG_TRIVIAL(trace) << total.c_str();
    }

    #ifdef _WIN32
    void DebugMonitor_Logger::trace(const CTL_String& msg) { OutputDebugStringA((getTime() + msg).c_str()); }
    #else
    void DebugMonitor_Logger::trace(const CTL_String& msg) { generic_outstream(std::cout, getTime() + msg + "\n"); }
    #endif

    File_Logger::File_Logger(const LPCSTR filename, bool bAppend/* = false*/)
    {
        if (bAppend)
            m_ostr.open(filename, std::ios::app);
        else
            m_ostr.open(filename);
    }

    void File_Logger::trace(const CTL_String& msg)
    {
        if (m_ostr)
            generic_outstream(m_ostr, getTime() + msg);
    }
}


CLogSystem::CLogSystem() : m_bEnable(false), m_bPrintTime(false), m_bPrintAppName(false), m_bFileOpenedOK(false), m_bErrorDisplayed(false)
{}

/////////////////////////////////////////////////////////////////////////////

CLogSystem::~CLogSystem()
{
}

/////////////////////////////////////////////////////////////////////////////

void CLogSystem::GetModuleName(HINSTANCE hInst)
{
    // get application path and name
    #ifdef WIN32
    TCHAR buf[_MAX_PATH+1];
    GetModuleFileName(hInst, buf, _MAX_PATH);
    CTL_StringType appDir = GetBaseDir(buf);
    m_csAppName = GetBaseName(buf);
#else
    // code for Linux using dladdr
    #endif
}

void CLogSystem::InitLogger(int loggerType, LPCTSTR pOutputFilename, HINSTANCE hInst, bool bAppend)
{
    GetModuleName(hInst);
    switch (loggerType )
    {
        case CONSOLE_LOGGING:
            app_logger_map[CONSOLE_LOGGING] = std::make_shared<dynarithmic::StdCout_Logger>();
        break;
        case DEBUG_WINDOW_LOGGING:
            app_logger_map[DEBUG_WINDOW_LOGGING] = std::make_shared<DebugMonitor_Logger>();
        break;
        case FILE_LOGGING:
            app_logger_map[FILE_LOGGING] = std::make_shared<File_Logger>(StringConversion::Convert_NativePtr_To_Ansi(pOutputFilename).c_str(), bAppend);
        break;
    }
    m_bEnable = true;
}

void  CLogSystem::InitConsoleLogging(HINSTANCE hInst)
{
#ifdef _WIN32
    AllocConsole();
    freopen("CON", "w", stdout);
#endif
    InitLogger(CONSOLE_LOGGING, nullptr, hInst, false);
}

void  CLogSystem::InitDebugWindowLogging(HINSTANCE hInst)
{
    InitLogger(DEBUG_WINDOW_LOGGING, nullptr, hInst, false);
}

void CLogSystem::InitFileLogging(LPCTSTR pOutputFilename, HINSTANCE hInst, bool bAppend)
{
    if (pOutputFilename)
        InitLogger(FILE_LOGGING, pOutputFilename, hInst, bAppend);
}

void CLogSystem::PrintBanner(bool bStarted)
{
    if ( bStarted )
    StatusOutFast(_T("****** Log started ******\n"));
    else
        StatusOutFast(_T("****** Log ended ******\n"));
}

/////////////////////////////////////////////////////////////////////////////

void CLogSystem::Enable(bool bEnable)
{
    m_bEnable = bEnable;
}

/////////////////////////////////////////////////////////////////////////////
bool CLogSystem::StatusOutFast(LPCTSTR fmt)
{
    if (!m_bEnable)
        return true;

    WriteOnDemand(StringConversion::Convert_NativePtr_To_Ansi(fmt));
    return true;
}

bool CLogSystem::WriteOnDemand(const CTL_String& fmt)
{
    for (auto& m : app_logger_map)
        (m.second)->trace(fmt);
    return true;
}

bool CLogSystem::Flush()
{
    return WriteOnDemand("");
}
/////////////////////////////////////////////////////////////////////////////

CTL_StringType CLogSystem::GetBaseName(const CTL_StringType &path)
{
    CTL_StringArrayType rArray;
    StringWrapper::SplitPath(path, rArray);
    return rArray[StringWrapper::NAME_POS];
}

/////////////////////////////////////////////////////////////////////////////

CTL_StringType CLogSystem::GetBaseDir(const CTL_StringType & path)
{
    CTL_StringArrayType rArray;
    StringWrapper::SplitPath(path, rArray);
    return rArray[StringWrapper::DIRECTORY_POS];
}

void CLogSystem::OutputDebugStringFull(const CTL_StringType& s)
{
    for (auto& m : app_logger_map)
        (m.second)->trace(StringConversion::Convert_Native_To_Ansi(s));
}

CTL_StringType CLogSystem::GetDebugStringFull(const CTL_StringType& s)
{
    CTL_StringStreamType strm;
    if ( m_csAppName.empty() )
        m_csAppName = _T("Unknown App");
    strm << m_csAppName << _T(" : [") << system_clock::now() << _T("] : ") << s;
    return strm.str();
}
