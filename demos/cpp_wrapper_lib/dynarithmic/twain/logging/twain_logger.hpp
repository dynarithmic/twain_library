/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2022 Dynarithmic Software.

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
#ifndef DTWAIN_TWAIN_LOGGER_HPP
#define DTWAIN_TWAIN_LOGGER_HPP

#include <string>
#include <array>
#include <functional>
#include <dtwain.h>

namespace dynarithmic
{
    namespace twain
    {
        enum class logger_destination
        {
            tofile = DTWAIN_LOG_USEFILE,
            toconsole = DTWAIN_LOG_CONSOLE,
            todebug = DTWAIN_LOG_DEBUGMONITOR,
            tofile_and_console = tofile | toconsole,
            tofile_and_debug = tofile | todebug,
            toconsole_and_debug = toconsole | todebug,
            toall = tofile | todebug | toconsole,
            tonone = 0
        };

        enum class logger_verbosity
        {
            verbose0 = 0,
            verbose1 = 1,
            verbose2 = 2,
            verbose3 = 3,
            verbose4 = 4
        };

        class twain_logger
        {
            logger_destination m_log_destination = logger_destination::tonone;
            logger_verbosity m_log_verbosity = logger_verbosity::verbose4;
            std::string m_log_filename;
            std::array<LONG, 5> m_verbose_settings;
            bool m_bEnabled = false;
            bool m_bCustomEnabled = false;
            std::function<void(const char*)> m_customFunc;
            public:
                typedef std::function<void(const char*)> custom_function_type;
                twain_logger() : m_log_verbosity(logger_verbosity::verbose1)
                {
                    m_verbose_settings[0] = 0;
                    m_verbose_settings[1] = DTWAIN_LOG_CALLSTACK;
                    m_verbose_settings[2] = m_verbose_settings[1] | DTWAIN_LOG_LOWLEVELTWAIN | DTWAIN_LOG_DECODE_DEST | DTWAIN_LOG_DECODE_SOURCE;
                    m_verbose_settings[3] = m_verbose_settings[2] | DTWAIN_LOG_DECODE_TWMEMREF;
                    m_verbose_settings[4] = m_verbose_settings[3] | DTWAIN_LOG_DECODE_TWEVENT;
                }

                twain_logger& enable(bool bEnable = true) { m_bEnabled = bEnable; return *this; }
                twain_logger& enable_custom(bool bEnable = true) { m_bCustomEnabled = bEnable; return *this; }
                twain_logger& set_destination(logger_destination ld) { m_log_destination = ld; return *this; }
                logger_destination get_destination() const { return m_log_destination; }
                twain_logger& set_verbosity(logger_verbosity lv) { m_log_verbosity = lv; return *this; }
                logger_verbosity get_verbosity() const { return m_log_verbosity; }
                long  get_verbosity_aslong() const { return m_verbose_settings[static_cast<LONG>(m_log_verbosity)];}
                twain_logger& set_filename(const std::string& filename) { m_log_filename = filename; return *this; }
                std::string get_filename() const { return m_log_filename; }
                bool is_enabled() const { return m_bEnabled; }
                bool is_custom_enabled() const { return m_bCustomEnabled; }
                twain_logger& set_custom_function(custom_function_type fn) { m_customFunc = fn; return *this; }
                custom_function_type get_custom_function() const { return m_customFunc; }

                /// Logs a custom error message to the logging system
                /// 
                /// This function will pass a message to the logging system.  The message will show up in the log with time stamp.
                /// @param[in] msg Message that will be logged to the logging system.
                /// @returns **true** if the message was sent to the logging system successfully, **false** otherwise.
                static bool log_custom_message(std::string msg) { return DTWAIN_LogMessageA(msg.c_str()) ? true : false; }
        };
    }
}
#endif
