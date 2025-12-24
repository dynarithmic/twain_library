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
#ifndef DTWAIN_IMPRINTER_OPTIONS_HPP
#define DTWAIN_IMPRINTER_OPTIONS_HPP

#include <vector>
#include <string>
#include <iterator>
#include <algorithm>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class imprinter_options
        {
            friend class options_base;
            bool m_bEnable;
            std::vector<printertype_value::value_type> m_vPrinterToUse;
            std::vector<fontstyle_value::value_type> m_fontStyles;
            std::vector<std::string> m_printerStrings;
            std::vector<indextrigger_value::value_type> m_indexTriggers;
            std::string m_suffixString;
            capability_type::printerindex_type m_printerIndex;
            capability_type::printerindexmaxvalue_type m_printerMaxValue;
            capability_type::printercharrotation_type m_charRotation;
            capability_type::printerindexnumdigits_type m_printerNumDigits;
            capability_type::printerindexmaxvalue_type m_printerIndexStep;
            std::string m_printerLeadChar;
            stringmode_value::value_type m_stringMode;
            double m_vertical_offset;

            public:
                imprinter_options() : m_bEnable(false),
                    m_printerIndex(0),
                    m_printerMaxValue(INT_MAX),
                    m_charRotation(0),
                    m_printerNumDigits(0),
                    m_printerIndexStep(1),
                    m_stringMode(stringmode_value::single),
                    m_vertical_offset(0.0) {}

                imprinter_options& enable(bool bEnable=true)
                { m_bEnable = bEnable; return *this; }

                imprinter_options& set_index(capability_type::printerindex_type idx)
                { m_printerIndex = idx; return *this;}

                imprinter_options& set_charrotation(capability_type::printercharrotation_type rotation)
                { m_charRotation = rotation; return *this; }

                imprinter_options& set_printerleadchar(const std::string& s)
                { m_printerLeadChar = s; return *this; }

                imprinter_options& set_indexmaxvalue(capability_type::printerindexmaxvalue_type maxValue)
                { m_printerMaxValue = maxValue; return *this; }

                imprinter_options& set_indexnumdigits(capability_type::printerindexnumdigits_type numDigits)
                { m_printerNumDigits = numDigits; return *this; }

                imprinter_options& set_indexstep(capability_type::printerindexstep_type step)
                { m_printerIndexStep = step; return *this; }

                imprinter_options& set_mode(stringmode_value::value_type mode)
                { m_stringMode = mode; return *this; }

                imprinter_options& set_suffix(const std::string& s)
                { m_suffixString = s; return *this; }

                imprinter_options& set_verticaloffset(double offset)
                { m_vertical_offset = offset; return *this; }

                bool is_enabled() const { return m_bEnable; }
                capability_type::printerindex_type get_index() const { return m_printerIndex; }
                capability_type::printercharrotation_type get_charrotation() const { return m_charRotation; }
                std::string get_printerleadchar() const { return m_printerLeadChar; }
                capability_type::printerindexmaxvalue_type get_indexmaxvalue() const { return m_printerMaxValue; }
                capability_type::printerindexnumdigits_type get_indexnumdigits() const { return m_printerNumDigits; }
                capability_type::printerindexstep_type get_indexstep() const { return m_printerIndexStep; }
                stringmode_value::value_type get_mode() const { return m_stringMode; }
                std::string get_suffix() const { return m_suffixString; }
                double get_verticaloffset() const { return m_vertical_offset; }
                std::vector<printertype_value::value_type> get_printer() const { return m_vPrinterToUse; }

                template <typename Container=std::vector<indextrigger_value::value_type>>
                imprinter_options& set_indextrigger(const Container& c)
                {
                    m_indexTriggers.clear();
                    std::copy(std::begin(c), std::end(c), std::back_inserter(m_indexTriggers));
                    return *this;
                }   

                template <typename Container=std::vector<printertype_value::value_type>>
                imprinter_options& set_printer(const Container& c)
                {
                    m_vPrinterToUse.clear();
                    std::copy(std::begin(c), std::end(c), std::back_inserter(m_vPrinterToUse));
                    return *this;
                }

                template <typename Container=std::vector<std::string>>
                imprinter_options& set_string(const Container& c)
                {
                    m_printerStrings.clear();
                    std::copy(std::begin(c), std::end(c), std::back_inserter(m_printerStrings));
                    return *this;
                }

                template <typename Container=std::vector<fontstyle_value::value_type>>
                imprinter_options& set_fontstyles(const Container& c)
                {
                    m_fontStyles.clear();
                    std::copy(std::begin(c), std::end(c), std::back_inserter(m_fontStyles));
                    return *this;
                }   

                static const std::array<uint16_t, 16>& get_affected_caps()
                {
                    static std::array<uint16_t, 16> affected_caps = { CAP_ENDORSER,
                                                                    CAP_PRINTER,
                                                                    CAP_PRINTERCHARROTATION,
                                                                    CAP_PRINTERENABLED,
                                                                    CAP_PRINTERFONTSTYLE,
                                                                    CAP_PRINTERINDEX,
                                                                    CAP_PRINTERINDEXLEADCHAR,
                                                                    CAP_PRINTERINDEXMAXVALUE,
                                                                    CAP_PRINTERINDEXNUMDIGITS,
                                                                    CAP_PRINTERINDEXSTEP,
                                                                    CAP_PRINTERINDEXTRIGGER,
                                                                    CAP_PRINTERMODE,
                                                                    CAP_PRINTERSTRING,
                                                                    CAP_PRINTERSTRINGPREVIEW,
                                                                    CAP_PRINTERSUFFIX,
                                                                    CAP_PRINTERVERTICALOFFSET };
                    return affected_caps;
                }

        };
    }
}
#endif
