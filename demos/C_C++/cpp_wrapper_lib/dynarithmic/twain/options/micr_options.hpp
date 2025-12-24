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

#ifndef DTWAIN_MICR_OPTIONS_HPP
#define DTWAIN_MICR_OPTIONS_HPP

#include <vector>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class micr_options
        {
            friend class options_base;
            bool m_bMicrEnabled;
    
            public:
                micr_options() : m_bMicrEnabled(false) {}
                micr_options& enable(bool bSet = true) { m_bMicrEnabled = bSet; return *this; }

                bool is_enabled() const { return m_bMicrEnabled; }

                static const std::array<uint16_t, 1>& get_affected_caps()
                {
                    static std::array<uint16_t, 1> affected_caps = { CAP_MICRENABLED };
                    return affected_caps;
                }
        };
    }
}
#endif
