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
#ifndef DTWAIN_COLOR_OPTIONS_HPP
#define DTWAIN_COLOR_OPTIONS_HPP

#include <vector>
#include <iterator>
#include <algorithm>
#include <array>
#include <climits>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class color_options
        {
            public:
                using gamma_type = double;
                static constexpr gamma_type disable_gamma = (std::numeric_limits<gamma_type>::min)();

            private:
                friend class options_base;
                bool m_bColorManagementEnabled;
                std::vector<filter_value::value_type> m_vFilterValue;
                double m_Gamma;
                iccprofile_value::value_type m_ICCProfileValue;
                planarchunky_value::value_type m_PlanarChunkyValue;
    
            public:
                color_options() : m_bColorManagementEnabled(true),
                                                 m_Gamma(disable_gamma),
                                                 m_ICCProfileValue(iccprofile_value::default_val),
                                                 m_PlanarChunkyValue(planarchunky_value::default_val) {}

                color_options& enable(bool bSet=true) 
                { m_bColorManagementEnabled = bSet; return *this; }

                color_options& set_gamma(gamma_type val)
                { m_Gamma = val; return *this; }

                color_options& set_iccprofile(iccprofile_value::value_type val)
                { m_ICCProfileValue = val; return *this; }

                color_options& set_planarchunky(planarchunky_value::value_type val)
                { m_PlanarChunkyValue = val; return *this; }

                bool is_enabled() const { return m_bColorManagementEnabled; }
                std::vector<filter_value::value_type> get_filter() const { return m_vFilterValue; }
                gamma_type get_gamma() const { return m_Gamma; }
                iccprofile_value::value_type get_iccprofile() const { return m_ICCProfileValue; }
                planarchunky_value::value_type get_planarchunky() const { return m_PlanarChunkyValue; }

                template <typename Container=std::vector<filter_value::value_type>>
                color_options& set_filter(const Container& c)
                {
                    m_vFilterValue.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vFilterValue));
                    return *this;
                }

                static const std::array<uint16_t, 5>& get_affected_caps()
                {
                    static std::array<uint16_t, 5> affected_caps = {ICAP_COLORMANAGEMENTENABLED,
                                                                    ICAP_FILTER,
                                                                    ICAP_GAMMA,
                                                                    ICAP_ICCPROFILE,
                                                                    ICAP_PLANARCHUNKY };
                    return affected_caps;
                }
        };
    }
}
#endif
