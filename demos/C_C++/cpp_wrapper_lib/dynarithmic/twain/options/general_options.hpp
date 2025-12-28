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

#ifndef DTWAIN_GENERAL_OPTIONS_HPP
#define DTWAIN_GENERAL_OPTIONS_HPP

#include <limits>
#include <cstdint>
#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/types/twain_types.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class general_options
        {
            public:
                static constexpr double default_val = (std::numeric_limits<double>::min)(); 
                static constexpr uint16_t default_int = (std::numeric_limits<uint16_t>::min)();

            private:
                transfer_type m_transfer_type;
                int m_nMaxPageCount;
                int m_nMaxAcquisitions;
                sourceaction_type m_SourceAction;
                color_value::value_type m_pixelType;

            public:
                general_options() : 
                            m_transfer_type(transfer_type::file_using_native),
                            m_nMaxPageCount(DTWAIN_MAXACQUIRE),
                            m_nMaxAcquisitions(default_int),
                            m_SourceAction(sourceaction_type::openafteracquire),
                            m_pixelType(color_value::default_color)
                            {}

                general_options& set_transfer_type(transfer_type t)
                { m_transfer_type = t; return *this; }

                general_options& set_max_page_count(int numPages)
                { m_nMaxPageCount = numPages; return *this; }

                general_options& set_max_acquisitions(int numAcqs)
                { m_nMaxAcquisitions = numAcqs; return *this; }

                general_options& set_source_action(sourceaction_type sa)
                { m_SourceAction = sa; return *this; }

                general_options& set_pixeltype(color_value::value_type pt)
                { m_pixelType = pt; return *this;}

                transfer_type get_transfer_type() const
                { return m_transfer_type; }

                int get_max_page_count() const
                { return m_nMaxPageCount; }

                int get_max_acquisitions() const 
                { return m_nMaxAcquisitions; }

                color_value::value_type get_pixel_type() const
                { return m_pixelType; }

                sourceaction_type get_source_action() const
                { return m_SourceAction; }
        };
    }
}
#endif
