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
#include <dynarithmic/twain/info/imprinter_info.hpp>
#include <dynarithmic/twain/dtwain_twain.hpp>
#include <dynarithmic/twain/capability_interface/capability_interface.hpp>
#include <dynarithmic/twain/acquire_characteristics/acquire_characteristics.hpp>
#include <dynarithmic/twain/source/twain_source.hpp>
namespace dynarithmic
{
    namespace twain
    {
		bool imprinter_info::get_info(twain_source& ts)
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
			auto v = capInterface.get_printersuffix();
			if (!v.empty())
				m_PrinterSuffix = v.front();
			m_vPrinterVerticalOffset = capInterface.get_printerverticaloffset();
			return true;
		}
    }
}

