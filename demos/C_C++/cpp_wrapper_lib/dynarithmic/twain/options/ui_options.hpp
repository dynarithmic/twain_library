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

#ifndef DTWAIN_UI_OPTIONS_HPP
#define DTWAIN_UI_OPTIONS_HPP

#include <vector>
#include <iterator>
#include <algorithm>
#include <array>

#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class userinterface_options
        {
            friend class options_base;
            bool m_bShowUI;
            bool m_bShowUI_Only;
            bool m_bShowIndicators;
            std::vector<indicatormode_value::value_type> m_vIndicatorMode;

            public:

                template <typename Container=std::vector<indicatormode_value::value_type>>
                userinterface_options& set_indicator_mode(const Container& c)
                {
                    m_vIndicatorMode.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vIndicatorMode));
                    return *this;
                }

                userinterface_options() : m_bShowUI(true), m_bShowUI_Only(false), m_bShowIndicators(true) {}
                userinterface_options& show(bool bShow) { m_bShowUI = bShow; return *this; }
                userinterface_options& show_onlyui(bool bShow) { m_bShowUI_Only = bShow; return *this; }
                userinterface_options& show_indicators(bool bShow) { m_bShowIndicators = bShow; return *this; }

                bool is_shown() const { return m_bShowUI;  }
                bool is_uionly_shown() const { return m_bShowUI_Only; }
                bool indicators_shown() const { return m_bShowIndicators; }
                std::vector<indicatormode_value::value_type> get_indicator_modes() const { return m_vIndicatorMode; }

                const std::array<uint16_t, 7>& get_affected_caps()
                {
                    static std::array<uint16_t, 7> affected_caps = { CAP_CAMERAPREVIEWUI,
                                                                    CAP_CUSTOMDSDATA,
                                                                    CAP_CUSTOMINTERFACEGUID,
                                                                    CAP_ENABLEDSUIONLY,
                                                                    CAP_INDICATORS,
                                                                    CAP_INDICATORSMODE,
                                                                    CAP_UICONTROLLABLE};
                    return affected_caps;
                }
        };
    }
}
#endif
