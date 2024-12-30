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

#ifndef DTWAIN_IMAGEINFORMATION_OPTIONS_HPP
#define DTWAIN_IMAGEINFORMATION_OPTIONS_HPP

#include <string>
#include <vector>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class imageinformation_options
        {
            friend class options_base;
            std::string m_sAuthor;
            std::string m_sCaption;
            bool m_bExtImageInfo;
    
            public:

                imageinformation_options() : m_bExtImageInfo(false) {}
                imageinformation_options& set_author(const std::string& s)
                { m_sAuthor = s; return *this; }

                imageinformation_options& set_caption(const std::string& s)
                { m_sCaption = s; return *this; }

                imageinformation_options& set_extimageinfo(bool bSet)
                { m_bExtImageInfo = bSet; return *this; }

                std::string get_author() const
                { return m_sAuthor; }

                std::string get_caption() const
                { return m_sCaption; }

                bool get_extimageinfo() const
                { return m_bExtImageInfo;}

                static const std::array<uint16_t, 5>& get_affected_caps()
                {
                    static std::array<uint16_t, 5> affected_caps = { CAP_AUTHOR,
                                                                    CAP_CAPTION,
                                                                    CAP_TIMEDATE,
                                                                    ICAP_EXTIMAGEINFO,
                                                                    ICAP_SUPPORTEDEXTIMAGEINFO};
                    return affected_caps;
                }

        };
    }
}
#endif
