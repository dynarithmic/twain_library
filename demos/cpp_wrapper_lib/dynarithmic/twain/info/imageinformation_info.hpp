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

#ifndef DTWAIN_IMAGEINFORMATION_INFO_HPP
#define DTWAIN_IMAGEINFORMATION_INFO_HPP

#include <string>
#include <vector>

#include <dynarithmic/twain/capability_interface.hpp>
#include <dynarithmic/twain/types/twain_capbasics.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class imageinformation_info
        {
            std::string m_sAuthor;
            std::string m_sCaption;
            std::string m_TimeDate;
            std::vector<capability_type::extimageinfo_type> m_vExtImageInfo;
            std::vector<capability_type::supportedextimageinfo_type> m_vSupportedExtImageInfo;

            public:

                std::string get_author() const { return m_sAuthor; }
                std::string get_caption() const { return m_sCaption; }
                std::string get_timedate() const { return m_TimeDate; }
                std::vector<capability_type::extimageinfo_type> get_extimageinfo() const { return m_vExtImageInfo; }
                std::vector<capability_type::supportedextimageinfo_type> get_supportedextimageinfo() const { return m_vSupportedExtImageInfo; }

                bool get(capability_interface& capInterface)
                {
                    *this = {};
                    if ( capInterface.is_author_supported())
                        m_sAuthor = capInterface.get_author().front();
                    if (capInterface.is_caption_supported())
                        m_sCaption = capInterface.get_caption().front();
                    if (capInterface.is_timedate_supported())
                        m_TimeDate = capInterface.get_timedate().front();
                    m_vExtImageInfo = capInterface.get_extimageinfo();
                    m_vSupportedExtImageInfo = capInterface.get_supportedextimageinfo();
                    return true;
                }

        };
    }
}
#endif
