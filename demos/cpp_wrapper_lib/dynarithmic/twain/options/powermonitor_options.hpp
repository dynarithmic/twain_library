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
#ifndef DTWAIN_POWERMONITOR_OPTIONS_HPP
#define DTWAIN_POWERMONITOR_OPTIONS_HPP

#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class powermonitor_options
        {
            friend class options_base;
            capability_type::powersavetime_type m_powersavetime;
            static constexpr int32_t default_val = INT_MIN;
            public:
                powermonitor_options() : m_powersavetime(default_val) {}
                powermonitor_options& set_powersavetime(capability_type::powersavetime_type p) { m_powersavetime = p; return *this; }
                capability_type::powersavetime_type get_powersavetime() const { return m_powersavetime; }

                static const std::array<uint16_t, 4>& get_affected_caps()
                {
                    static std::array<uint16_t, 4> affected_caps = { CAP_BATTERYMINUTES,
                                                                     CAP_BATTERYPERCENTAGE,
                                                                     CAP_POWERSAVETIME,
                                                                    CAP_POWERSUPPLY };
                    return affected_caps;
                }
        };
    }
}
#endif
