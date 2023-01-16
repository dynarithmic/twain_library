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
#ifndef DTWAIN_PAPERHANDLING_OPTIONS_HPP
#define DTWAIN_PAPERHANDLING_OPTIONS_HPP

#include <vector>
#include <iterator>
#include <algorithm>
#include <array>
#include <dynarithmic/twain/types/twain_types.hpp>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class paperhandling_options
        {
            friend class options_base;
            bool m_bAutoFeed;
            bool m_bDuplexEnabled;
            feederalignment_value::value_type m_FeederAlignment;
            bool m_bFeederEnabled;
            feederorder_value::value_type m_FeederOrder;
            std::vector<feederpocket_value::value_type> m_vFeederPocket;
            bool m_bFeederPrep;
            std::vector<paperhandling_value::value_type> m_vPaperHandling;
            feedertype_value::value_type m_FeederType;
            int m_feeder_waittime;
            feedermode_value m_FeederMode;
            manualduplexmode_value m_DuplexModeValue;
            static constexpr int wait_infinite = -1;

            public:
                paperhandling_options() :
                                        m_bAutoFeed(true),
                                        m_bDuplexEnabled(false),
                                        m_bFeederEnabled(false),
                                        m_FeederAlignment(feederalignment_value::none),
                                        m_FeederOrder(feederorder_value::default_val),
                                        m_bFeederPrep(false),
                                        m_feeder_waittime(0),
                                        m_FeederMode(feedermode_value::feeder),
                                        m_DuplexModeValue(manualduplexmode_value::none),
                                        m_FeederType(feedertype_value::default_val) {}

                paperhandling_options& enable_autofeed(bool bSet)
                { m_bAutoFeed = bSet; return *this; }

                paperhandling_options& enable_duplex(bool bSet)
                { m_bDuplexEnabled = bSet; return *this; }

                paperhandling_options& set_feederalignment(feederalignment_value::value_type fa)
                { m_FeederAlignment = fa; return *this; }

                paperhandling_options& enable_feeder(bool bSet)
                { m_bFeederEnabled = bSet; return *this; }

                paperhandling_options& set_feederwait(int val)
                { m_feeder_waittime = val; return *this; }

                paperhandling_options& set_feederorder(feederorder_value::value_type fv)
                { m_FeederOrder = fv; return *this; }

                paperhandling_options& enable_feederprep(bool bSet)
                { m_bFeederPrep = bSet; return *this; }

                paperhandling_options& set_feedertype(feedertype_value::value_type fv)
                { m_FeederType = fv; return *this; }

                paperhandling_options& set_feedermode(feedermode_value fm)
                { m_FeederMode = fm; return *this; }

                paperhandling_options& set_duplexmode(manualduplexmode_value dm)
                { m_DuplexModeValue = dm; return *this; }

                bool is_autofeed_enabled() const { return m_bAutoFeed; }
                bool is_duplex_enabled() const { return m_bDuplexEnabled; }
                feederalignment_value::value_type get_feederalignment() const { return m_FeederAlignment; }
                bool is_feeder_enabled() const { return m_bFeederEnabled; }
                int get_feederwait() const { return m_feeder_waittime; }
                std::vector<feederpocket_value::value_type> get_feederpocket() const { return m_vFeederPocket; }
                feederorder_value::value_type get_feederorder() const { return m_FeederOrder; }
                bool is_feederprep_enabled() const { return m_bFeederPrep; }
                std::vector<paperhandling_value::value_type> get_paperhandling() const { return m_vPaperHandling; }
                feedertype_value::value_type get_feedertype() const { return m_FeederType; }
                feedermode_value get_feedermode() const { return m_FeederMode;  }
                manualduplexmode_value get_manualduplexmode() const { return m_DuplexModeValue; }

                static const std::array<uint16_t, 16>& get_affected_caps()
                {
                    static std::array<uint16_t, 16> affected_caps = { CAP_AUTOFEED,
                                                                        CAP_CLEARPAGE,
                                                                        CAP_DUPLEX,
                                                                        CAP_DUPLEXENABLED,
                                                                        CAP_FEEDERALIGNMENT,
                                                                        CAP_FEEDERENABLED,
                                                                        CAP_FEEDERLOADED,
                                                                        CAP_FEEDERORDER,
                                                                        CAP_FEEDERPOCKET,
                                                                        CAP_FEEDERPREP,
                                                                        CAP_FEEDPAGE,
                                                                        CAP_PAPERDETECTABLE,
                                                                        CAP_PAPERHANDLING,
                                                                        CAP_REACQUIREALLOWED,
                                                                        CAP_REWINDPAGE,
                                                                        ICAP_FEEDERTYPE };
                    return affected_caps;
                }


                template <typename Container=std::vector<paperhandling_value::value_type>>
                paperhandling_options& set_paperhandling(const Container& c)
                {
                    m_vPaperHandling.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vPaperHandling));
                    return *this;
                }

                template <typename Container=std::vector<feederpocket_value::value_type>>
                paperhandling_options& set_feederpocket(const Container& c)
                {
                    m_vFeederPocket.clear();
                    std::copy(c.begin(), c.end(), std::back_inserter(m_vFeederPocket));
                    return *this;
                }
        };
    }
}
#endif
