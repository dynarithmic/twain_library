/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2024 Dynarithmic Software.

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

#ifndef DTWAIN_RESOLUTION_OPTIONS_HPP
#define DTWAIN_RESOLUTION_OPTIONS_HPP

#include <vector>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class resolution_options  
        {
            friend class options_base;
            double m_xResolution = (std::numeric_limits<double>::min)();
            double m_yResolution = (std::numeric_limits<double>::min)();
    
            public:
                resolution_options() = default;
                resolution_options& set_xresolution(double val) noexcept
                { m_xResolution = val; return *this; }

                resolution_options& set_yresolution(double val) noexcept
                { m_yResolution = val; return *this; }

                resolution_options& set_resolution(double x, double y) noexcept
                { m_xResolution = x; m_yResolution = y; return *this; }

                double get_xresolution() const noexcept { return m_xResolution; }
                double get_yresolution() const noexcept { return m_yResolution; }

                static const std::array<uint16_t, 2>& get_affected_caps() noexcept
                {
                    static std::array<uint16_t, 2> affected_caps = { ICAP_XRESOLUTION, ICAP_YRESOLUTION };
                    return affected_caps;
                }
        };
    }
}
#endif
