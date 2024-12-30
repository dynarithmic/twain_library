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

#ifndef DTWAIN_DOUBLEFEED_OPTIONS_HPP
#define DTWAIN_DOUBLEFEED_OPTIONS_HPP

#include <vector>
#include <iterator>
#include <algorithm>
#include <limits>

#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class doublefeed_options
        {
            public:
                using length_type = double;
                static constexpr length_type default_length = (std::numeric_limits<length_type>::min)();
            private:
                friend class options_base;
                bool m_bEnable;
                doublefeeddetection_value::value_type m_detection;
                double m_length;
                doublefeedsensitivity_value::value_type m_sensitivity;
                std::vector<doublefeedresponse_value::value_type> m_vResponses;

            public:
                doublefeed_options& set_responses(doublefeedresponse_value::value_type v)
                { 
                    std::vector<doublefeedresponse_value::value_type> vtemp {v};
                    return set_responses(vtemp); 
                }

                doublefeed_options() : m_bEnable(false),
                                       m_detection(doublefeeddetection_value::default_val),
                                       m_length((std::numeric_limits<double>::min)()),
                                       m_sensitivity(doublefeedsensitivity_value::default_val)
                {}

                doublefeed_options& enable(bool bEnable = true) { m_bEnable = bEnable; return *this; }
                bool is_enabled() const { return m_bEnable; }

                doublefeed_options& set_detection(doublefeeddetection_value::value_type dv) 
                { m_detection = dv; return *this; }

                doublefeed_options& set_length(length_type length)
                { m_length = length; return *this; }

                doublefeed_options& set_sensitivity(doublefeedsensitivity_value::value_type sensitivity)
                { m_sensitivity = sensitivity; return *this; }

                doublefeeddetection_value::value_type get_detection() const { return m_detection; }
                std::vector<doublefeedresponse_value::value_type> get_responses() const { return m_vResponses; }
                length_type get_length() const { return m_length; }
                doublefeedsensitivity_value::value_type get_sensitivity() const { return m_sensitivity; }

                template <typename Container=std::vector<doublefeedresponse_value::value_type>>
                doublefeed_options& set_responses(const Container &c)
                {
                    m_vResponses.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vResponses));
                    return *this;
                }

                static const std::array<uint16_t, 4>& get_affected_caps()
                {
                    static std::array<uint16_t, 4> affected_caps = { CAP_DOUBLEFEEDDETECTION, CAP_DOUBLEFEEDDETECTIONLENGTH,
                                                                     CAP_DOUBLEFEEDDETECTIONSENSITIVITY, CAP_DOUBLEFEEDDETECTIONRESPONSE };
                    return affected_caps;
                }
        };
    }
}
#endif
