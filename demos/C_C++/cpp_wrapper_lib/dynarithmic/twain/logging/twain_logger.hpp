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
#ifndef DTWAIN_TWAIN_LOGGER_HPP
#define DTWAIN_TWAIN_LOGGER_HPP

#include <string>
#include <array>
#include <functional>
#include <dynarithmic/twain/dtwain_twain.hpp>

namespace dynarithmic
{
    namespace twain
    {
        enum class logger_verbosity
        {
            verbose0 = 0,
            verbose1 = 1,
            verbose2 = 2,
            verbose3 = 3,
            verbose4 = 4
        };

        enum class logger_destination : uint32_t
        {
            to_none    = 1000,
            to_console = DTWAIN_LOG_CONSOLE,
            to_file    = DTWAIN_LOG_USEFILE,
            to_file_append = DTWAIN_LOG_FILEAPPEND,
            to_debug   = DTWAIN_LOG_DEBUGMONITOR,
            to_custom  = DTWAIN_LOG_USECALLBACK
        };

        class twain_logger
        {
            logger_verbosity m_log_verbosity = logger_verbosity::verbose4;
            std::array<LONG, 5> m_verbose_settings;
            bool m_bEnabled = false;
            std::string m_filename;
            uint32_t m_log_destination = static_cast<uint32_t>(logger_destination::to_custom)
                ;
            std::vector<std::function<void(twain_logger&, const char*)>> m_vCustomFuncs;
            public:
                typedef std::function<void(twain_logger&, const char*)> log_proc_type;
                virtual ~twain_logger() = default;
                virtual void log(const char* /*msg*/) {}

                twain_logger() : m_log_verbosity(logger_verbosity::verbose1)
                {
                    m_verbose_settings[0] = 0;
                    m_verbose_settings[1] = DTWAIN_LOG_CALLSTACK;
                    m_verbose_settings[2] = m_verbose_settings[1] | DTWAIN_LOG_LOWLEVELTWAIN | DTWAIN_LOG_DECODE_DEST | DTWAIN_LOG_DECODE_SOURCE;
                    m_verbose_settings[3] = m_verbose_settings[2] | DTWAIN_LOG_DECODE_TWMEMREF;
                    m_verbose_settings[4] = m_verbose_settings[3] | DTWAIN_LOG_DECODE_TWEVENT | DTWAIN_LOG_MISCELLANEOUS;
                }

                twain_logger& enable(bool bEnable = true) { m_bEnabled = bEnable; return *this; }
                twain_logger& set_verbosity(logger_verbosity lv) { m_log_verbosity = lv; return *this; }
                twain_logger& set_filename(std::string name) { m_filename = std::move(name); return *this; }
                std::string get_filename() const { return m_filename; }
                twain_logger& set_destination(logger_destination ld) 
                { 
                    m_log_destination |= static_cast<int>(ld); 
                    return *this; 
                }
                twain_logger& clear_all_destinations() { m_log_destination = 0; return *this;  }
                twain_logger& clear_destination(logger_destination ld) { m_log_destination = m_log_destination & ~static_cast<int>(ld); 
                                                    return *this; }
                bool is_toconsole() const { return m_log_destination & static_cast<int>(logger_destination::to_console); }
				bool is_tofile() const { return m_log_destination & static_cast<int>(logger_destination::to_file); }
				bool is_tofileappend() const { return m_log_destination & static_cast<int>(logger_destination::to_file_append); }
                bool is_todebug() const { return m_log_destination & static_cast<int>(logger_destination::to_debug); }
				bool is_tocustom() const { return m_log_destination & static_cast<int>(logger_destination::to_custom); }
                bool any_logging() const { return m_log_destination > 0; }
        
                uint32_t get_destination_aslong() const { return m_log_destination;  }
                logger_verbosity get_verbosity() const { return m_log_verbosity; }
                long  get_verbosity_aslong() const { return m_verbose_settings[static_cast<long>(m_log_verbosity)];}
                bool is_enabled() const { return m_bEnabled; }

                /// Logs a custom error message to the logging system
                /// 
                /// This function will pass a message to the logging system.  The message will show up in the log with time stamp.
                /// @param[in] msg Message that will be logged to the logging system.
                /// @returns **true** if the message was sent to the logging system successfully, **false** otherwise.
                static bool log_custom_message(std::string msg) { return API_INSTANCE DTWAIN_LogMessageA(msg.c_str()) ? true : false; }
        };
    }
}
#endif
