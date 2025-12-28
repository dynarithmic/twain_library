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
#ifndef DTWAIN_TWAIN_ERROR_DETAILS_HPP
#define DTWAIN_TWAIN_ERROR_DETAILS_HPP

#include <deque>
#include <cstdint>

namespace dynarithmic
{
    namespace twain
    {
        class error_logger
        {
            std::deque<int32_t> m_dequeErrors;
            std::size_t buffer_size;

            public:
                typedef std::deque<int32_t> container_type;
                error_logger(std::size_t buf = 0) : buffer_size(buf) {}
                error_logger& set_maxsize(std::size_t sz) { buffer_size = sz; return *this; }
                const container_type& get_errors() const { return m_dequeErrors; }

                void add_error(int32_t val)
                {
                    if (buffer_size > 0)
                    {
                        if (m_dequeErrors.size() == buffer_size)
                            m_dequeErrors.pop_back();
                    }
                    m_dequeErrors.push_front(val);
                }

                void clear() { m_dequeErrors.clear(); }
        };
    }
}
#endif
