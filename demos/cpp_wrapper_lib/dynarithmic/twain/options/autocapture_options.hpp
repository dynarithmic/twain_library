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

#ifndef DTWAIN_AUTOCAPTURE_OPTIONS_HPP
#define DTWAIN_AUTOCAPTURE_OPTIONS_HPP

#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class autocapture_options
        {
            friend class options_base;
            capability_type::automaticcapture_type m_NumImages;
            capability_type::timebeforefirstcapture_type m_TimeBefore;
            capability_type::timebetweencaptures_type m_TimeBetween;

            public:
                autocapture_options() : m_NumImages(0), m_TimeBefore(0), m_TimeBetween(0) {}

                autocapture_options& set_num_images(capability_type::automaticcapture_type numImages)
                { m_NumImages = numImages; return *this; }

                autocapture_options& set_timebeforefirstcapture(capability_type::timebeforefirstcapture_type num)
                { m_TimeBefore = num; return *this; }

                autocapture_options& set_timebetweencaptures(capability_type::timebetweencaptures_type num)
                { m_TimeBetween = num; return *this; }

                int32_t get_num_images() const { return m_NumImages; }
                capability_type::timebeforefirstcapture_type get_timebeforefirstcapture() const { return m_TimeBefore; }
                capability_type::timebetweencaptures_type get_timebetweencaptures() const { return m_TimeBetween; }

                static const std::array<uint16_t, 3>& get_affected_caps()
                {
                    static std::array<uint16_t, 3> affected_caps = {CAP_AUTOMATICCAPTURE, CAP_TIMEBEFOREFIRSTCAPTURE, CAP_TIMEBETWEENCAPTURES};
                    return affected_caps;
                }

        };
    }
}   
#endif
