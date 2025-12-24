/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2020 Dynarithmic Software.

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

#ifndef PDF_TEXT_ELEMENT_HPP
#define PDF_TEXT_ELEMENT_HPP

#include <string>
#include <utility>
#include <cstdlib>
#include <vector>
#include <cstdint>
#include <cstring>
#include <dtwpdft.h>

namespace dynarithmic
{
    namespace twain
    {
        class twain_source;
        struct pdf_rendermode_value
        {
            static constexpr int32_t fill = DTWAIN_PDFRENDER_FILL;
            static constexpr int32_t stroke = DTWAIN_PDFRENDER_STROKE;
            static constexpr int32_t fillstroke = DTWAIN_PDFRENDER_FILLSTROKE;
            static constexpr int32_t invisible = DTWAIN_PDFRENDER_INVISIBLE;
        };

        struct pdf_printpage_value
        {
            static constexpr int32_t allpages = DTWAIN_PDFTEXT_ALLPAGES;
            static constexpr int32_t evenpages = DTWAIN_PDFTEXT_EVENPAGES;
            static constexpr int32_t oddpages = DTWAIN_PDFTEXT_ODDPAGES;
            static constexpr int32_t firstpage = DTWAIN_PDFTEXT_FIRSTPAGE;
            static constexpr int32_t lastpage = DTWAIN_PDFTEXT_LASTPAGE;
            static constexpr int32_t currentpage = DTWAIN_PDFTEXT_CURRENTPAGE;
        };

        enum class pdf_pageignore_flag : uint32_t
        {
            ignorenone  = 0,
            scaling     = DTWAIN_PDFTEXT_NOSCALING,
            charspacing = DTWAIN_PDFTEXT_NOCHARSPACING,
            wordspacing = DTWAIN_PDFTEXT_NOWORDSPACING,
            rendermode  = DTWAIN_PDFTEXT_NORENDERMODE,
            rgbcolor    = DTWAIN_PDFTEXT_NORGBCOLOR,
            fontsize    = DTWAIN_PDFTEXT_NOFONTSIZE,
            ignoreall   = DTWAIN_PDFTEXT_IGNOREALL
        };

        class pdf_text_element
        {
            std::string m_text;
            std::pair<int32_t, int32_t> m_position;
            std::string m_font;
            int m_fontsize;
            uint32_t m_color;
            int32_t m_rendermode;
            double m_scaling;
            double m_charspacing;
            double m_wordspacing;
            uint32_t m_strokewidth;
            int32_t m_printpage;
            std::vector<pdf_pageignore_flag> m_vIgnoreflags;

            static constexpr const char * fontnames[] = {
                                                        "Courier",
                                                        "Courier-Bold",
                                                        "Courier-BoldOblique",
                                                        "Courier-Oblique",
                                                        "Helvetica",
                                                        "Helvetica-Bold",
                                                        "Helvetica-BoldOblique",
                                                        "Helvetica-Oblique",
                                                        "Times-Bold",
                                                        "Times-BoldItalic",
                                                        "Times-Roman",
                                                        "Times-Italic",
                                                        "Symbol",
                                                        "ZapfDingbats"
                                                        };

            public:
                typedef std::vector<pdf_pageignore_flag> pdf_ignoreflag_container;
                using position_type = std::pair<int32_t, int32_t>;

                pdf_text_element() : m_position{},
                                     m_font("Helvetica"),
                                     m_fontsize(10), 
                                     m_color(0),
                                     m_rendermode(pdf_rendermode_value::fill),
                                     m_scaling(100.0),
                                     m_charspacing(0),
                                     m_wordspacing(0),
                                     m_strokewidth(0),
                                     m_printpage(pdf_printpage_value::currentpage)
                {}

                static bool is_supported_font(const char* fontName)
                {
                    for (auto str : fontnames)
                    {
                        if (strcmp(str, fontName) == 0)
                            return true;
                    }
                    return false;
                }

                pdf_text_element& set_text(const std::string& s) { m_text = s; return *this; }
                pdf_text_element& set_position(position_type p) { m_position = p; return *this; }
                pdf_text_element& set_xposition(int32_t x) { m_position = { x, m_position.second }; return *this; }
                pdf_text_element& set_yposition(int32_t y) { m_position = { m_position.first, y }; return *this; }
                pdf_text_element& set_font(const std::string& s) { m_font = s; return *this; }
                pdf_text_element& set_color(uint32_t color) { m_color = color; return *this; }
                pdf_text_element& set_fontsize(int sz) { m_fontsize = sz; return *this; }
                pdf_text_element& set_rendermode(int32_t rm) { m_rendermode = rm; return *this; }
                pdf_text_element& set_scaling(double val) { m_scaling = val; return *this; }
                pdf_text_element& set_charspacing(double val) { m_charspacing = val; return *this; }
                pdf_text_element& set_wordspacing(double val) { m_wordspacing = val; return *this; }
                pdf_text_element& set_strokewidth(uint32_t val) { m_strokewidth = val; return *this; }
                pdf_text_element& set_whichpages(int32_t whichpage) { m_printpage = whichpage; return *this; }
                pdf_text_element& set_ignoreflags(const pdf_ignoreflag_container& vectFlags) { m_vIgnoreflags = vectFlags; return *this; }

                std::string get_text() const { return m_text; }
                position_type get_position() const { return m_position; } 
                int32_t get_xposition() const { return m_position.first;  }
                int32_t get_yposition() const { return m_position.second; }
                std::string get_font() const  { return m_font; }
                uint32_t get_color() const { return m_color; }
                int get_fontsize() const { return m_fontsize; }
                int32_t get_rendermode() const { return m_rendermode; }
                double get_scaling() const { return m_scaling; }
                double get_charspacing() const { return m_charspacing; }
                double get_wordspacing() const { return m_wordspacing; }
                uint32_t get_strokewidth() const { return m_strokewidth; }
                int32_t get_whichpages() const { return m_printpage; }
                pdf_ignoreflag_container get_ignoreflags() const { return m_vIgnoreflags; }
                pdf_ignoreflag_container& get_ignoreflags_ref() { return m_vIgnoreflags; }

                bool add_text(twain_source& ts);
        };
    }
}
#endif
