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
#ifndef DTWAIN_IMPRINTER_INFO_HPP
#define DTWAIN_IMPRINTER_INFO_HPP

#include <vector>
#include <string>
#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/source/twain_source_base.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class imprinter_info
        {
            std::vector<capability_type::endorser_type> m_vEndorser;
            std::vector<printertype_value::value_type> m_vPrinter;
            std::vector<capability_type::printercharrotation_type> m_vCharRotation;
            std::vector<capability_type::printerenabled_type> m_vPrinterEnabled;
            std::vector<fontstyle_value::value_type> m_vFontStyle;
            std::vector<capability_type::printerindex_type> m_vPrinterIndex;
            std::vector<std::string> m_vPrinterLeadChar;
            std::vector<capability_type::printerindexmaxvalue_type> m_vMaxValue;
            std::vector<capability_type::printerindexnumdigits_type> m_vNumDigits;
            std::vector<capability_type::printerindexstep_type> m_vIndexStep;
            std::vector<indextrigger_value::value_type> m_vIndexTrigger;
            std::vector<stringmode_value::value_type> m_vPrinterMode;
            std::vector<std::string> m_vPrinterString;
            std::vector<std::string> m_vPrinterStringPreview;
            std::string m_PrinterSuffix;
            std::vector<double> m_vPrinterVerticalOffset;

            public:
                imprinter_info() {}
                imprinter_info(twain_source_base& ts) 
                {
                    get_info(ts);
                }

                bool get_info(twain_source_base& ts)
                {
                    auto& capInterface = ts.get_capability_interface();
                    m_vEndorser = capInterface.get_endorser();
                    m_vPrinter = capInterface.get_printer();
                    m_vCharRotation = capInterface.get_printercharrotation();
                    m_vPrinterEnabled = capInterface.get_printerenabled();
                    m_vFontStyle = capInterface.get_printerfontstyle();
                    m_vPrinterIndex = capInterface.get_printerindex();
                    m_vPrinterLeadChar = capInterface.get_printerindexleadchar();
                    m_vMaxValue = capInterface.get_printerindexmaxvalue();
                    m_vNumDigits = capInterface.get_printerindexnumdigits();
                    m_vIndexStep = capInterface.get_printerindexstep();
                    m_vIndexTrigger = capInterface.get_printerindextrigger();
                    m_vPrinterMode = capInterface.get_printermode();
                    m_vPrinterString = capInterface.get_printerstring();
                    m_vPrinterStringPreview = capInterface.get_printerstringpreview();
                    auto v  = capInterface.get_printersuffix();
                    if (!v.empty())
                        m_PrinterSuffix = v.front();
                    m_vPrinterVerticalOffset = capInterface.get_printerverticaloffset();
                    return true;
                }

                bool is_supported() const { return !m_vEndorser.empty() || !m_vPrinter.empty(); }
                std::vector<capability_type::endorser_type> get_endorser() const { return m_vEndorser; }
                std::vector<printertype_value::value_type> get_printer() const { return m_vPrinter; }
                std::vector<capability_type::printercharrotation_type> get_printercharrotation() const { return m_vCharRotation; }
                std::vector<capability_type::printerenabled_type> get_printerenabled() const { return m_vPrinterEnabled; }
                std::vector<fontstyle_value::value_type> get_printerfontstyle() const { return m_vFontStyle; }
                std::vector<capability_type::printerindex_type> get_printerindex() const { return m_vPrinterIndex; }
                std::vector<std::string> get_printerindexleadchar() const { return m_vPrinterLeadChar; }
                std::vector<capability_type::printerindexmaxvalue_type> get_printerindexmaxvalue() const { return m_vMaxValue; }
                std::vector<capability_type::printerindexnumdigits_type> get_printerindexnumdigits() const { return m_vNumDigits; }
                std::vector<capability_type::printerindexstep_type> get_printerindexstep() const { return m_vIndexStep; }
                std::vector<indextrigger_value::value_type> get_printerindextrigger() const { return m_vIndexTrigger; }
                std::vector<stringmode_value::value_type> get_printermode() const { return m_vPrinterMode; }
                std::vector<std::string> get_printerstring() const { return m_vPrinterString; }
                std::vector<std::string> get_printerstringpreview() const { return m_vPrinterStringPreview; }
                std::string get_printersuffix() const { return m_PrinterSuffix; }
                std::vector<double> get_printerverticaloffset() const { return m_vPrinterVerticalOffset; }
        };
    }
}
#endif
