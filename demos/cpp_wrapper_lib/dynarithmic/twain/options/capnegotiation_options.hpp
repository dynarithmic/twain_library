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

#ifndef DTWAIN_CAPNEGOTIATION_OPTIONS_HPP
#define DTWAIN_CAPNEGOTIATION_OPTIONS_HPP

#include <vector>
#include <algorithm>
#include <iterator>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class capnegotiation_options
        {
            friend class options_base;
            std::vector<uint16_t> m_vExtendedCaps;
            bool m_bSetExtendedCaps;
    
            public:
                capnegotiation_options() : m_bSetExtendedCaps(false ){}
                capnegotiation_options& enable(bool bSet= true)
                { m_bSetExtendedCaps = bSet; return *this; }

                std::vector<uint16_t> get_extendedcaps() const { return m_vExtendedCaps; }
                bool is_enabled() const { return m_bSetExtendedCaps; }

                template <typename Container=std::vector<uint16_t>>
                capnegotiation_options& set_extendedcaps(const Container& c)
                {
                    m_vExtendedCaps.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vExtendedCaps));
                    return *this;
                }

                static const std::array<uint16_t, 3>& get_affected_caps()
                {
                    static std::array<uint16_t, 3> affected_caps = { CAP_EXTENDEDCAPS, CAP_SUPPORTEDCAPS, CAP_SUPPORTEDDATS };
                    return affected_caps;
                }

        };
    }
}
#endif
