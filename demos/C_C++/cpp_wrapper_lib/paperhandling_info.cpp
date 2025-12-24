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
#include <dynarithmic/twain/info/paperhandling_info.hpp>
#include <dynarithmic/twain/dtwain_twain.hpp>
#include <dynarithmic/twain/capability_interface/capability_interface.hpp>
#include <dynarithmic/twain/source/twain_source.hpp>
namespace dynarithmic
{
    namespace twain
    {
        // This is a "live" capability.  Can change depending on external factors.
        bool paperhandling_info::is_feederloaded(twain_source& ts) const
        {
            const auto& v = ts.get_capability_interface().get_cap_values(CAP_FEEDERLOADED);
            if (v.empty())
                return false;
            return v.front() ? true : false;
        }

        bool paperhandling_info::get_info(twain_source& ts)
        {
            *this = {};
            auto& capInterface = ts.get_capability_interface();
            m_vAutoFeed = capInterface.get_cap_values<std::vector<CAP_AUTOFEED_::value_type>>(CAP_AUTOFEED);
            m_vClearPage = capInterface.get_cap_values<std::vector<CAP_CLEARPAGE_::value_type>>(CAP_CLEARPAGE);
            m_vDuplexEnabled = capInterface.get_cap_values<std::vector<CAP_DUPLEXENABLED_::value_type>>(CAP_DUPLEXENABLED);
            m_vFeederAlignment = capInterface.get_cap_values<std::vector<CAP_FEEDERALIGNMENT_::value_type>>(CAP_FEEDERALIGNMENT);
            m_vFeederEnabled = capInterface.get_cap_values<std::vector<CAP_FEEDERENABLED_::value_type>>(CAP_FEEDERENABLED);
            m_vFeederOrder = capInterface.get_cap_values<std::vector<CAP_FEEDERORDER_::value_type>>(CAP_FEEDERORDER);
            m_vFeederLoaded = capInterface.get_cap_values<std::vector<CAP_FEEDERLOADED_::value_type>>(CAP_FEEDERLOADED);
            m_vFeederPocket = capInterface.get_cap_values<std::vector<CAP_FEEDERPOCKET_::value_type>>(CAP_FEEDERPOCKET);
            m_vFeederPrep = capInterface.get_cap_values<std::vector<CAP_FEEDERPREP_::value_type>>(CAP_FEEDERPREP);
            m_vFeedPage = capInterface.get_cap_values<std::vector<CAP_FEEDPAGE_::value_type>>(CAP_FEEDPAGE);
            m_vPaperDetectable = capInterface.get_cap_values<std::vector<CAP_PAPERDETECTABLE_::value_type>>(CAP_PAPERDETECTABLE);
            m_vPaperHandling = capInterface.get_cap_values<std::vector<CAP_PAPERHANDLING_::value_type>>(CAP_PAPERHANDLING);
            m_vReacquireAllowed = capInterface.get_cap_values<std::vector<CAP_REACQUIREALLOWED_::value_type>>(CAP_REACQUIREALLOWED);
            m_vRewindPage = capInterface.get_cap_values<std::vector<CAP_REWINDPAGE_::value_type>>(CAP_REWINDPAGE);
            m_vFeederType = capInterface.get_cap_values<std::vector<ICAP_FEEDERTYPE_::value_type>>(ICAP_FEEDERTYPE);
            if (capInterface.is_cap_supported(CAP_DUPLEX))
                m_Duplex = capInterface.get_cap_values<std::vector<CAP_DUPLEX_::value_type>>(CAP_DUPLEX).front();
            if (!capInterface.is_cap_supported(CAP_FEEDERENABLED))
                m_bFeederSupported = false;
            else
            {
                auto tempVal = capInterface.get_cap_values<std::vector<CAP_FEEDERENABLED_::value_type>>(CAP_FEEDERENABLED, capability_interface::get_current());
                if (!tempVal.empty())
                {
                    if (tempVal.front() == true)
                        m_bFeederSupported = true;
                    else
                    {
                        capInterface.set_cap_values<std::vector<CAP_FEEDERENABLED_::value_type >>({ true }, CAP_FEEDERENABLED);
                        if (capInterface.get_last_error().error_code == DTWAIN_NO_ERROR)
                        {
                            capInterface.set_cap_values<std::vector<CAP_FEEDERENABLED_::value_type >>({ tempVal.front() }, CAP_FEEDERENABLED);
                            m_bFeederSupported = true;
                        }
                        else
                            m_bFeederSupported = false;
                    }
                }
            }
            return true;
        }
    }
}

