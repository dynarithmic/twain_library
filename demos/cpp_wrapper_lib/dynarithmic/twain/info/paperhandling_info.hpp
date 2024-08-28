/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2024 Dynarithmic Software.

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
#ifndef DTWAIN_PAPERHANDLING_INFO_HPP
#define DTWAIN_PAPERHANDLING_INFO_HPP

#include <vector>
#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/types/twain_capbasics.hpp>
namespace dynarithmic
{
    namespace twain
    {
        class twain_source;
        class paperhandling_info
        {
            std::vector<capability_type::autofeed_type> m_vAutoFeed;
            std::vector<capability_type::clearpage_type> m_vClearPage;
            duplextype_value::value_type m_Duplex;
            std::vector<capability_type::duplexenabled_type> m_vDuplexEnabled;
            std::vector<feederalignment_value::value_type> m_vFeederAlignment;
            std::vector<capability_type::feederenabled_type> m_vFeederEnabled;
            std::vector<feederorder_value::value_type> m_vFeederOrder;
            std::vector<capability_type::feederloaded_type> m_vFeederLoaded;
            std::vector<feederpocket_value::value_type> m_vFeederPocket;
            std::vector<capability_type::feederprep_type> m_vFeederPrep;
            std::vector<capability_type::feedpage_type> m_vFeedPage;
            std::vector<capability_type::paperdetectable_type> m_vPaperDetectable;
            std::vector<paperhandling_value::value_type> m_vPaperHandling;
            std::vector<capability_type::reacquireallowed_type> m_vReacquireAllowed;
            std::vector<capability_type::rewindpage_type> m_vRewindPage;
            std::vector<feedertype_value::value_type> m_vFeederType;
            bool m_bFeederSupported;

        public:
            paperhandling_info() : m_Duplex(duplextype_value::none), m_bFeederSupported(false) {}
            paperhandling_info(twain_source& ts) : m_Duplex(duplextype_value::none), m_bFeederSupported(false) 
            {
                get_info(ts);
            }

            std::vector<capability_type::autofeed_type> get_autofeed() const { return m_vAutoFeed;  }
            std::vector<capability_type::clearpage_type> get_clearpage() const { return m_vClearPage; }
            duplextype_value::value_type get_duplex() const { return m_Duplex; }
            std::vector<capability_type::duplexenabled_type> get_duplexenabled() const { return m_vDuplexEnabled; }
            std::vector<feederalignment_value::value_type> get_feederalignment() const { return m_vFeederAlignment; }
            std::vector<capability_type::feederenabled_type> get_feederenabled() const { return m_vFeederEnabled; }
            std::vector<feederorder_value::value_type> get_feederorder() const { return m_vFeederOrder; }
            std::vector<capability_type::feederloaded_type> get_feederloaded() const { return m_vFeederLoaded; }
            std::vector<feederpocket_value::value_type> get_feederpocket() const { return m_vFeederPocket; }
            std::vector<capability_type::feederprep_type> get_feederprep() const { return m_vFeederPrep; }
            std::vector<capability_type::feedpage_type> get_feedpage() const { return m_vFeedPage; }
            std::vector<capability_type::paperdetectable_type> get_paperdetectable() const { return m_vPaperDetectable; }
            std::vector<paperhandling_value::value_type> get_paperhandling() const { return m_vPaperHandling; }
            std::vector<capability_type::reacquireallowed_type> get_reacquireallowed() const { return m_vReacquireAllowed; }
            std::vector<capability_type::rewindpage_type> get_rewindpage() const { return m_vRewindPage; }
            std::vector<feedertype_value::value_type> get_feedertype() const { return m_vFeederType; }
            bool is_feedersupported() const { return m_bFeederSupported; }

            // This is a "live" capability.  Can change depending on external factors.
            bool is_feederloaded(twain_source& ts) const;
            bool get_info(twain_source& ts);
        };
    }
}
#endif
