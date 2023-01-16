/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2022 Dynarithmic Software.

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
#ifndef DTWAIN_PAGES_OPTIONS_HPP
#define DTWAIN_PAGES_OPTIONS_HPP

#include <array>
#include <dynarithmic/twain/types/twain_frame.hpp>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class pages_options
        {
            using papersize_value = supportedsizes_value;
            friend class options_base;
            segmented_value::value_type m_SegmentedValue;
            twain_frame<double> m_Frame;
            uint16_t m_MaxFrames;
            papersize_value::value_type m_SupportedSize;
    
            public:
                pages_options() : m_SegmentedValue(segmented_value::none), 
                                                    m_MaxFrames(0),
                                                    m_SupportedSize(papersize_value::default_val) {}

                pages_options& set_segmented(segmented_value::value_type sv) 
                { m_SegmentedValue = sv; return *this; }

                pages_options& set_maxframes(uint16_t val)
                { m_MaxFrames = val; return *this; }

                pages_options& set_frame(const twain_frame<double>& tf)
                { m_Frame = tf; return *this; }

                pages_options& set_supportedsize(papersize_value::value_type ps)
                { m_SupportedSize = ps; return *this; }

                segmented_value::value_type get_segmented() const { return m_SegmentedValue; }
                uint16_t get_maxframes() const { return m_MaxFrames; }
                twain_frame<double> get_frame() const { return m_Frame; }
                papersize_value::value_type get_supportedsize() const { return m_SupportedSize; }

                const std::array<uint16_t, 4>& get_affected_caps()
                {
                    static std::array<uint16_t, 4> affected_caps = { CAP_SEGMENTED,
                                                                    ICAP_FRAMES,
                                                                    ICAP_MAXFRAMES,
                                                                    ICAP_SUPPORTEDSIZES };
                    return affected_caps;
                }

        };
    }
}
#endif
