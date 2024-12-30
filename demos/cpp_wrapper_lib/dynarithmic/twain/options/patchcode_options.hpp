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

#ifndef DTWAIN_PATCHCODE_OPTIONS_HPP
#define DTWAIN_PATCHCODE_OPTIONS_HPP

#include <vector>
#include <array>
#include <iterator>
#include <algorithm>

#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class patchcode_options
        {
            friend class options_base;
            bool m_bDetectionEnabled;
            uint32_t m_MaxRetries;
            uint32_t m_MaxSearchPriorities;
            patchcodesearchmode_value::value_type m_SearchMode;
            std::vector<patchcodetype_value::value_type> m_vSearchPriority;
            uint32_t m_TimeOut;

            public:

                patchcode_options() : m_bDetectionEnabled(false),
                                                        m_MaxRetries(0),
                                                        m_MaxSearchPriorities(0),
                                                        m_SearchMode(patchcodesearchmode_value::default_val),
                                                        m_TimeOut(0) {}

                patchcode_options& enable(bool bSet) 
                { m_bDetectionEnabled = bSet; return *this; }

                patchcode_options& set_maxretries(uint32_t num)
                { m_MaxRetries = num; return *this; }

                patchcode_options& set_maxsearchpriorities(uint32_t num)
                { m_MaxSearchPriorities = num; return *this; }

                patchcode_options& set_searchmode(patchcodesearchmode_value::value_type sm)
                { m_SearchMode = sm; return *this; }

                patchcode_options& set_timeout(uint32_t num)
                { m_TimeOut = num; return *this; }

                bool is_enabled() const { return m_bDetectionEnabled; }
                uint32_t get_maxretries() const { return m_MaxRetries; }
                uint32_t get_maxsearchpriorities() const { return m_MaxSearchPriorities; }
                patchcodesearchmode_value::value_type get_searchmode() const { return m_SearchMode; }
                std::vector<patchcodetype_value::value_type>& get_searchpriorities() { return m_vSearchPriority; }
                uint32_t get_timeout() const { return m_TimeOut; }

                template <typename Container=std::vector<patchcodetype_value::value_type>>
                patchcode_options& set_searchpriorities(const Container& c)
                {
                    m_vSearchPriority.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vSearchPriority));
                    return *this;
                }

                static const std::array<uint16_t, 7>& get_affected_caps()
                {
                    static std::array<uint16_t, 7> affected_caps = { ICAP_PATCHCODEDETECTIONENABLED,
                                                                    ICAP_SUPPORTEDPATCHCODETYPES,
                                                                    ICAP_PATCHCODEMAXSEARCHPRIORITIES,
                                                                    ICAP_PATCHCODESEARCHPRIORITIES,
                                                                    ICAP_PATCHCODESEARCHMODE,
                                                                    ICAP_PATCHCODEMAXRETRIES,
                                                                    ICAP_PATCHCODETIMEOUT
                                                                    };
                    return affected_caps;
                }

        };
    }
}
#endif
