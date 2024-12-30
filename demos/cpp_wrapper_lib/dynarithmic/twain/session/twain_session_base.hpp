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
#ifndef DTWAIN_TWAIN_SESSION_BASE_HPP
#define DTWAIN_TWAIN_SESSION_BASE_HPP

#include <string>
#include <unordered_map>
#include <vector>
#include <memory>
#include <algorithm>
#include <iterator>

#include <dynarithmic/twain/identity/twain_identity.hpp>
#include <dynarithmic/twain/logging/twain_logger.hpp>
namespace dynarithmic {
namespace twain {
    class twain_source;
    class twain_logger;
    class twain_callback;
    class error_logger;
    class twain_identity;

    /**
    The twain_session_base class serves as the base class to twain_session.
    */
    class twain_session_base
    {
        public:
            using twain_app_info = twain_identity;
            struct supported_filetype_info
            {
                friend class twain_session;
                private:
                    filetype_value::value_type m_filetype;
                    std::string m_filetypename;
                    std::vector<std::string> m_fileExtensions;
                public:
                    filetype_value::value_type get_type() const { return m_filetype; }
                    std::string get_name() const { return m_filetypename; }

                    template <typename Container = std::vector<std::string>>
                    Container get_extensions() const
                    {
                        Container c;
                        std::copy(m_fileExtensions.begin(), m_fileExtensions.end(), std::inserter(c, c.end()));
                        return c;
                    }
            };

            using source_basic_info = twain_app_info;
            using logger_type = std::pair<twain_session_base*, std::unique_ptr<twain_logger>>;
            using callback_map_type = std::unordered_map<twain_source*, std::unique_ptr<twain_callback>>;
            using error_logger_func = std::function<void(LONG)>;

        protected:
            bool m_bStarted = false;
            std::string m_dsm_path;
            std::string m_long_name;
            std::string m_short_name;
            std::string m_dtwain_path;
            DTWAIN_HANDLE m_Handle = nullptr;
            logger_type m_logger;
            callback_map_type m_mapcallback;
            mutable std::vector<source_basic_info> m_source_cache;
            error_logger_func m_error_logger_func;
            std::unordered_map<std::string, std::string> m_source_detail_map;

        public:
            twain_session_base() = default;
            ~twain_session_base() = default;
            logger_type& get_logger() noexcept { return m_logger; }
            callback_map_type& get_callback_map() noexcept { return m_mapcallback; }
            error_logger_func& get_error_logger_func() { return m_error_logger_func; }
            DTWAIN_HANDLE get_handle() const { return m_Handle; }
            void log_error(int32_t err)
            {
                if (m_error_logger_func)
                    m_error_logger_func(err);
            }
    };
}
}
#endif
