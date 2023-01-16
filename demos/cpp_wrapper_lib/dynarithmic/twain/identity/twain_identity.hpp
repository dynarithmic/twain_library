/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2020 Dynarithmic Software.

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
// TWAIN application info (used by both the TWAIN Data Source Manager and Data Source
#ifndef DTWAIN_TWAIN_IDENTITY_HPP
#define DTWAIN_TWAIN_IDENTITY_HPP

#include <string>
#include <cstring>
#include <algorithm>
#include <sstream>
#include <numeric>
#include "twain.h"

namespace dynarithmic
{
    namespace twain
    {
        class twain_identity
        {
        private:
            template <int N>
            static void copy_util(const std::string& s, char(&dest)[N], size_t num)
            {
                const auto minval = (std::min)(num, s.size());
                std::copy_n(s.begin(), minval, dest);
                dest[minval] = 0;
            }

            TW_IDENTITY m_identity;

        public:
            explicit twain_identity(TW_IDENTITY& t) : m_identity(t) {}
            explicit twain_identity(TW_IDENTITY* t) : m_identity(t ? *t : TW_IDENTITY()) {}

            twain_identity() : m_identity()
            {
                set_version_info("<?>");
                set_manufacturer("<?>");
                set_product_family("<?>");
                set_product_name("<?>");
                set_language(TWLG_USA);
                set_country(TWCY_USA);
            }

            twain_identity& operator =(TW_IDENTITY& t) noexcept { m_identity = t; return *this; }
            twain_identity& operator =(TW_IDENTITY* t) noexcept { m_identity = t ? *t : TW_IDENTITY(); return *this; }

            twain_identity& set_protocol_major(uint16_t val) { m_identity.ProtocolMajor = val; return *this; }
            twain_identity& set_protocol_minor(uint16_t val) { m_identity.ProtocolMinor = val; return *this; }
            twain_identity& set_supported_groups(uint32_t val) { m_identity.SupportedGroups = val; return *this; }
            twain_identity& set_manufacturer(const std::string& s) { copy_util(s, m_identity.Manufacturer, 32U); return *this; }
            twain_identity& set_product_family(const std::string& s) { copy_util(s, m_identity.ProductFamily, 32U); return *this; }
            twain_identity& set_product_name(const std::string& s) { copy_util(s, m_identity.ProductName, 32U); return *this; }
            twain_identity& set_version_info(const std::string& s) { copy_util(s, m_identity.Version.Info, 32U); return *this; }

            twain_identity& set_major_num(uint16_t val) { m_identity.Version.MajorNum = val; return *this; }
            twain_identity& set_minor_num(uint16_t val) { m_identity.Version.MinorNum = val; return *this; }
            twain_identity& set_language(uint16_t val) { m_identity.Version.Language = val; return *this; }
            twain_identity& set_country(uint16_t val) { m_identity.Version.Country = val; return *this; }

            uint16_t get_protocol_major() const     {  return m_identity.ProtocolMajor; }   
            uint16_t get_protocol_minor() const     {  return m_identity.ProtocolMinor; }   
            uint32_t get_supported_groups() const   {  return m_identity.SupportedGroups; } 
            std::string get_manufacturer() const    {  return m_identity.Manufacturer; }    
            std::string get_product_family() const  {  return m_identity.ProductFamily; }   
            std::string get_product_name() const    {  return m_identity.ProductName; }     
            uint16_t get_major_num() const          {  return m_identity.Version.MajorNum; }
            uint16_t get_minor_num() const          {  return m_identity.Version.MinorNum; }
            uint16_t get_language() const           {  return m_identity.Version.Language; }
            uint16_t get_country() const            {  return m_identity.Version.Country; } 
            std::string get_version_info() const    {  return m_identity.Version.Info; }    
            static std::string get_supported_groups_string(uint32_t sgroups)
            {
                static const uint32_t dgroups[] = { DG_CONTROL, DG_IMAGE, DG_AUDIO, DF_DSM2, DF_APP2, DF_DS2 };
                static const char * dgroupsStr[] = { "DG_CONTROL", "DG_IMAGE", "DG_AUDIO", "DF_DSM2", "DF_APP2", "DF_DS2" };
                std::string ret;
                int i = 0;
                for (auto g : dgroups)
                {
                    if (sgroups & g)
                    {
                        if (i > 0)
                            ret += ",";
                        ret += dgroupsStr[i];
                    }
                    ++i;
                }
                if (ret.empty())
                    return "<unknown>";
                return ret;
            }

            TW_IDENTITY& get_identity()             { return m_identity; };
            explicit operator const TW_IDENTITY*()
            {
                TW_IDENTITY test = {};
                if (std::memcmp(&m_identity, &test, sizeof(TW_IDENTITY)) == 0) 
                    return nullptr; 
                return &m_identity;
            }

            std::string to_json() const 
            {
                std::vector<std::string> jComponents;
                std::vector<std::string> jVerComponents;
                std::stringstream jstrm;
                jstrm << "\"protocol-major\":" << m_identity.ProtocolMajor;
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"protocol-minor\":" << m_identity.ProtocolMinor;
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"supported-groups\":\"" << get_supported_groups_string(m_identity.SupportedGroups) << "\"";
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"manufacturer\":\"" << m_identity.Manufacturer << "\"";
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"product-family\":\"" << m_identity.ProductFamily << "\"";
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"product-name\":\"" << m_identity.ProductName << "\"";
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"version-majornum\":" << m_identity.Version.MajorNum;
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"version-minornum\":" << m_identity.Version.MinorNum;
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"version-language\":" << m_identity.Version.Language;
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"version-country\":" << m_identity.Version.Country;
                jComponents.push_back(jstrm.str());
                jstrm.str("");
                jstrm << "\"version-info\":\"" << m_identity.Version.Info << "\"";
                jComponents.push_back(jstrm.str());

                std::string s1 = std::accumulate(jComponents.begin(), jComponents.end(), std::string(),
                                       [](const std::string& total, const std::string& cur) { return total + "," + cur; }) + "}";
                if (s1[0] == ',')
                    s1 = s1.substr(1);
                jstrm.str("");
                jstrm << "{\"device-name\":\"" << m_identity.ProductName << "\", \"twain-identity\":{" << s1 << "}";
                return jstrm.str();
            }
        };
    }
}
#endif
