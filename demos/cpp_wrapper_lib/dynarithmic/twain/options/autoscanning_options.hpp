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

#ifndef DTWAIN_AUTOSCANNING_OPTIONS_HPP
#define DTWAIN_AUTOSCANNING_OPTIONS_HPP

#include <vector>
#include <algorithm>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class autoscanning_options
        {
            bool m_bAutoScan;
            bool m_bCameraEnabled;
            std::vector<color_value::value_type> m_vCameraOrder;
            cameraside_value::value_type m_CameraSide;
            capability_type::maxbatchbuffers_type m_MaxBatchBuffers;
            friend class options_base;

            public:
                autoscanning_options() :  m_bAutoScan(true),
                                          m_bCameraEnabled(false),
                                          m_CameraSide(cameraside_value::both),
                                          m_MaxBatchBuffers(0) {}

                autoscanning_options& enable_autoscan(bool bSet=true)
                { m_bAutoScan = bSet; return *this; }

                autoscanning_options& enable_camerahandling(bool bSet=true)
                { m_bCameraEnabled = bSet; return *this; }

                autoscanning_options& set_cameraside(cameraside_value::value_type cv)
                { m_CameraSide = cv; return *this; }

                autoscanning_options& set_maxbatchbuffers(capability_type::maxbatchbuffers_type val)
                { m_MaxBatchBuffers = val; return *this; }

                bool is_autoscan_enabled() const { return m_bAutoScan; }
                bool is_camerahandling_enabled() const { return m_bCameraEnabled; }
                std::vector<color_value::value_type> get_cameraorder() const { return m_vCameraOrder; }
                cameraside_value::value_type get_cameraside() const { return m_CameraSide; }
                capability_type::maxbatchbuffers_type get_maxbatchbuffers() const { return m_MaxBatchBuffers; }

                const std::array<uint16_t, 5>& get_affected_caps()
                {
                    static std::array<uint16_t, 5> affected_caps = { CAP_AUTOSCAN,
                                                                    CAP_CAMERAENABLED,
                                                                    CAP_CAMERAORDER,
                                                                    CAP_CAMERASIDE,
                                                                    CAP_MAXBATCHBUFFERS};
                    return affected_caps;
                }

                template <typename Container=std::vector<color_value::value_type>>
                autoscanning_options& set_cameraorder(const Container& c)
                {
                    m_vCameraOrder.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vCameraOrder));
                    return *this;
                }
        };
    }
}
#endif
