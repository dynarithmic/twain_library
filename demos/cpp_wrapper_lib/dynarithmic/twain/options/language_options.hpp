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

#ifndef DTWAIN_LANGUAGE_OPTIONS_HPP
#define DTWAIN_LANGUAGE_OPTIONS_HPP

#include <vector>
#include <array>

#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class language_options
        {
            friend class options_base;
            language_value::value_type m_Language;
    
            public:
                language_options() : m_Language(language_value::default_val) {}
                language_options& set_language(language_value::value_type lv) { m_Language = lv; return *this; }
                language_value::value_type get_language() const { return m_Language; }

                static const std::array<uint16_t, 1>& get_affected_caps()
                {
                    static std::array<uint16_t, 1> affected_caps = { CAP_LANGUAGE };
                    return affected_caps;
                }

        };
    }
}
#endif
