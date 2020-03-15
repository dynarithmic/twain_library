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
/*********************************************************************/

#ifndef CLogSystem_H
#define CLogSystem_H
#include "ctlobstr.h"
#include <deque>
#include <vector>
#include <memory>
#include <unordered_map>
#include <fstream>
#include <iostream>
#include <iomanip>
#include <chrono>
#include <sstream>
/////////////////////////////////////////////////////////////////////////////
namespace dynarithmic
{
    class CBaseLogger
    {
        protected:
            static CTL_String getTime();
        public:
            virtual void trace(const CTL_String& msg) = 0;
            void generic_outstream(std::ostream& os, const CTL_String& msg);
    };

    class StdCout_Logger : public CBaseLogger
    {
        public:
            void trace(const CTL_String& msg) override;
    };

    class DebugMonitor_Logger : public CBaseLogger
    {
        public:
            void trace(const CTL_String& msg) override;
    };

    class File_Logger : public CBaseLogger
    {
        CTL_String m_fileName;
        std::ofstream m_ostr;
        public:
            File_Logger(const LPCSTR filename, bool bAppend = false);
            void trace(const CTL_String& msg) override;
    };

    class CLogSystem
    {
    public:
            enum {FILE_LOGGING, DEBUG_WINDOW_LOGGING, CONSOLE_LOGGING};

       /////////////////////////////////////////////////////////////////////////////
            std::unordered_map<int, std::shared_ptr<CBaseLogger>> app_logger_map;
        CLogSystem();
       ~CLogSystem();

       /////////////////////////////////////////////////////////////////////////////
           void     InitFileLogging(LPCTSTR pOutputFilename, HINSTANCE hInst, bool bAppend);
           void     InitConsoleLogging(HINSTANCE hInst); // adds console.
           void     InitDebugWindowLogging(HINSTANCE hInst); // adds win debug logging.

       /////////////////////////////////////////////////////////////////////////////
       // output text, just like TRACE or printf
       bool     StatusOutFast(LPCTSTR fmt);


       /////////////////////////////////////////////////////////////////////////////
       // turn it on or off
       void     Enable(bool bEnable);

       /////////////////////////////////////////////////////////////////////////////
       // timestamp each line?
       void     PrintTime(bool b) {m_bPrintTime = b;}

       /////////////////////////////////////////////////////////////////////////////
       // print the application name?
       void     PrintAppName(bool b) {m_bPrintAppName = b;}

       /////////////////////////////////////////////////////////////////////////////
       // override the default app name, which is the name the EXE (minus the ".exe")
       void     SetAppName(LPCTSTR pName) {m_csAppName = pName;}

       bool     Flush();

           void     PrintBanner(bool bStarted = true);

       CTL_StringType GetAppName() const {return m_csAppName;}
       void OutputDebugStringFull(const CTL_StringType& s);
       CTL_StringType GetDebugStringFull(const CTL_StringType& s);

    protected:
        CTL_StringType  m_csAppName;
        CTL_StringType  m_csFileName;

       /////////////////////////////////////////////////////////////////////////////
       // controlling stuff
        bool     m_bEnable;
        bool     m_bPrintTime;
        bool     m_bPrintAppName;
        bool     m_bFileOpenedOK;
        bool     m_bErrorDisplayed;

       /////////////////////////////////////////////////////////////////////////////
       // string utils
       CTL_StringType GetBaseDir(const CTL_StringType & path);
       CTL_StringType GetBaseName(const CTL_StringType & path);
           void GetModuleName(HINSTANCE hInst);
        bool WriteOnDemand(const CTL_String& fmt);

           private:
               void InitLogger(int loggerType, LPCTSTR pOutputFilename, HINSTANCE hInst, bool bAppend);
    };
}
#endif

