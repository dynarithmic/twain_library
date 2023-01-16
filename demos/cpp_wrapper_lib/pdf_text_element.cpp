/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2023 Dynarithmic Software.

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
#include <dtwain.h>
#include <dynarithmic/twain/pdf/pdf_text_element.hpp>
#include <dynarithmic/twain/source/twain_source.hpp>

namespace dynarithmic
{
    namespace twain
    {
        bool pdf_text_element::add_text(twain_source& ts)
        {
            DTWAIN_SOURCE src = ts.get_source();
            LONG flags = 0;

            if (std::find(m_vIgnoreflags.begin(), m_vIgnoreflags.end(), pdf_pageignore_flag::ignorenone) == m_vIgnoreflags.end())
            {
                for (auto fv : m_vIgnoreflags)
                    flags |= static_cast<LONG>(fv);
            }
            flags |= m_printpage;

            BOOL retval = DTWAIN_AddPDFTextA(src,
                m_text.c_str(),
                m_position.first,
                m_position.second,
                m_font.c_str(),
                m_fontsize,
                m_color,
                m_rendermode,
                m_scaling,
                m_charspacing,
                m_wordspacing,
                m_strokewidth,
                flags);
            return retval ? true : false;
        }
    }
}