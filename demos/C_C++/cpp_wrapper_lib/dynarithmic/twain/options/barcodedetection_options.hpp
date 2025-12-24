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
#ifndef DTWAIN_BARCODE_OPTIONS_HPP
#define DTWAIN_BARCODE_OPTIONS_HPP

#include <vector>
#include <array>
#include <iterator>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class barcodedetection_options
        {
            friend class options_base;
            bool m_bDetectionEnabled;
            capability_type::barcodemaxretries_type  m_MaxRetries;
            capability_type::barcodemaxsearchpriorities_type m_MaxSearchPriorities;
            barcodesearchmode_value::value_type m_SearchMode;
            std::vector<barcodetype_value::value_type> m_vSearchPriority;
            capability_type::barcodetimeout_type m_TimeOut;

            public:
                barcodedetection_options() : m_bDetectionEnabled(false),
                                                        m_MaxRetries(0),
                                                        m_MaxSearchPriorities(0),
                                                        m_SearchMode(barcodesearchmode_value::default_val),
                                                        m_TimeOut(0) {}

                barcodedetection_options& enable(bool bSet)  noexcept
                { m_bDetectionEnabled = bSet; return *this; }

                barcodedetection_options& set_maxretries(capability_type::barcodemaxretries_type num) noexcept
                { m_MaxRetries = num; return *this; }

                barcodedetection_options& set_maxsearchpriorities(capability_type::barcodemaxsearchpriorities_type num) noexcept
                { m_MaxSearchPriorities = num; return *this; }

                barcodedetection_options& set_searchmode(barcodesearchmode_value::value_type sm) noexcept
                { m_SearchMode = sm; return *this; }

                barcodedetection_options& set_timeout(capability_type::barcodetimeout_type num) noexcept
                { m_TimeOut = num; return *this; }

                bool is_enabled() const noexcept { return m_bDetectionEnabled; }
                capability_type::barcodemaxretries_type get_maxretries() const noexcept { return m_MaxRetries; }
                capability_type::barcodemaxsearchpriorities_type get_maxsearchpriorities() const noexcept { return m_MaxSearchPriorities; }
                barcodesearchmode_value::value_type get_searchmode() const noexcept { return m_SearchMode; }
                std::vector<barcodetype_value::value_type>& get_searchpriorities() noexcept { return m_vSearchPriority; }
                capability_type::barcodetimeout_type get_timeout() const noexcept { return m_TimeOut; }

                template <typename Container=std::vector<barcodetype_value::value_type>>
                barcodedetection_options& set_searchpriorities(const Container& c)
                {
                    m_vSearchPriority.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vSearchPriority));
                    return *this;
                }

                static const std::array<uint16_t, 7>& get_affected_caps() noexcept
                {
                    static std::array<uint16_t, 7> affected_caps = { ICAP_BARCODEDETECTIONENABLED,
                                                                    ICAP_SUPPORTEDBARCODETYPES,
                                                                    ICAP_BARCODEMAXRETRIES,
                                                                    ICAP_BARCODEMAXSEARCHPRIORITIES,
                                                                    ICAP_BARCODESEARCHMODE,
                                                                    ICAP_BARCODESEARCHPRIORITIES,
                                                                    ICAP_BARCODETIMEOUT };
                    return affected_caps;
                }

        };
    }
}
#endif
