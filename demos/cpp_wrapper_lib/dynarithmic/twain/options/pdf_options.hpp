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

#ifndef DTWAIN_PDF_OPTIONS_HPP
#define DTWAIN_PDF_OPTIONS_HPP

#include <vector>
#include <string>
#include <random>
#include <utility>

#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/types/twain_capbasics.hpp>
namespace dynarithmic
{
    namespace twain
    {
        using papersize_value = supportedsizes_value;
        class pdf_options
        {
            public:
                using pdf_custom_scale = std::pair<double, double>;
                using pdf_page_size = std::pair<double, double>;

                enum class pdf_orientation
                {
                   landscape = DTWAIN_PDF_LANDSCAPE,
                   portrait = DTWAIN_PDF_PORTRAIT
                };

                enum class pdf_page_scale
                {
                  none = DTWAIN_PDF_NOSCALING,
                  fitpage = DTWAIN_PDF_FITPAGE,
                  custom = DTWAIN_PDF_CUSTOMSCALE
                };

                enum class pdf_permission
                {
                   print = DTWAIN_PDF_ALLOWPRINTING,
                   modify = DTWAIN_PDF_ALLOWMOD,
                   copy = DTWAIN_PDF_ALLOWCOPY,
                   modifyannotations = DTWAIN_PDF_ALLOWMODANNOTATIONS,
                   fillin = DTWAIN_PDF_ALLOWFILLIN,
                   extract = DTWAIN_PDF_ALLOWEXTRACTION,
                   assembly = DTWAIN_PDF_ALLOWASSEMBLY,
                   degradedprint = DTWAIN_PDF_ALLOWDEGRADEDPRINTING,
                   all = print | modify | copy | modifyannotations | fillin | extract | assembly | degradedprint
                };

                enum class pdf_paper_size_custom
                {
                    none = 0,
                    custom = DTWAIN_PDF_CUSTOMSIZE,
                    pixelspermeter = DTWAIN_PDF_PIXELSPERMETERSIZE,
                    variable = DTWAIN_PDF_VARIABLEPAGESIZE
                };
        
            private:
                std::string m_author;
                std::string m_creator;
                std::string m_producer;
                std::string m_keywords;
                std::string m_subject;
                std::string m_title;
                bool m_useASCII;
                int m_jpegQuality;
                pdf_orientation m_orientation;

            public:
                class pdf_encryption_options
                {
                    std::string m_user_password;
                    std::string m_owner_password;
                    bool m_useStrong;
                    bool m_useEncryption;
                    bool m_useAESEncryption;
                    bool m_bAutoGenPassword;
                    int32_t m_permissions;
                
                    pdf_encryption_options& set_password(std::string& pwdtoset, const std::string& pwdString, int nWhich)
                    {
                        if (m_bAutoGenPassword && nWhich == 0)
                            pwdtoset = get_random_password();
                        else
                            pwdtoset = pwdString;
                        return *this;
                    }

                    static std::string generate_random()
                    {
                        char s[100] = {};
                        struct rnd_gen
                        {
                            rnd_gen() 
                            {}

                            char operator()() const
                            {
                                static const char range[] = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&()_+=-{}[];:'\\\",<.>/?`~";
                                static const size_t len = std::size(range);
                                static std::random_device rand_dev;
                                static std::mt19937 generator(rand_dev());
                                std::uniform_int_distribution<int>  distr(0, len - 1);
                                auto val = distr(generator);
                                return range[val];
                            }
                        };
                        std::generate_n(s, 32, rnd_gen());
                        return std::string(s, 32);
                    }

                    public:
                       pdf_encryption_options() : m_useStrong(true), m_useEncryption(false), m_useAESEncryption(false), m_bAutoGenPassword(false), m_permissions(0) {}
                       pdf_encryption_options& use_encryption(bool bSet, bool useStrong = true) { m_useEncryption = bSet; m_useStrong = useStrong; return *this; }
                       pdf_encryption_options& use_AES_encryption(bool bSet) { m_useAESEncryption = bSet; return *this; }
                       pdf_encryption_options& use_strong_encryption(bool bSet = true) { m_useEncryption = true; m_useStrong = bSet; return *this; }
                       pdf_encryption_options& use_autogen_password(bool bSet = true) { m_bAutoGenPassword = true; m_useStrong = bSet; return *this; }
                       pdf_encryption_options& set_user_password(const std::string& pwd = "") { return set_password(m_user_password, pwd, 0);}
                       pdf_encryption_options& set_owner_password(const std::string& pwd = "") { return set_password(m_owner_password, pwd, 1); }

                       static std::string get_random_password() { return generate_random(); }

                       template <typename Container = std::vector<pdf_permission>>
                           pdf_encryption_options& set_permissions(const Container& ctr)
                       {       
                           auto iter = ctr.begin();
                           while (iter != ctr.end())
                           {
                              m_permissions |= static_cast<LONG>(*iter);
                              std::advance(iter, 1);
                           }
                           return *this;
                       }
       
                       std::string get_user_password() const { return m_user_password; }
                       std::string get_owner_password() const { return m_owner_password; }
                       bool is_use_encryption() const { return m_useEncryption; }
                       bool is_use_strong_encryption() const { return m_useStrong; }
                       bool is_use_AES_encryption() const { return m_useAESEncryption; }
                       bool is_use_autogen_password() const { return m_bAutoGenPassword; }

                       template <typename C = std::vector<pdf_permission>> 
                       C get_permissions() const 
                       {
                            C retVal;
                            static pdf_permission allPerms[] = {pdf_permission::print,
                                                                pdf_permission::modify,
                                                                pdf_permission::copy,
                                                                pdf_permission::modifyannotations,
                                                                pdf_permission::fillin,
                                                                pdf_permission::extract,
                                                                pdf_permission::assembly,
                                                                pdf_permission::degradedprint};
                            static int allPermsSize = sizeof(allPerms) / sizeof(allPerms[0]);
                            std::copy_if(allPerms, allPerms + allPermsSize, std::inserter(retVal, retVal.end()), [&](pdf_permission& p)
                                          { return static_cast<int32_t>(p) & m_permissions; });
                            return retVal;
                       }

                       int32_t get_permissions_int() const { return m_permissions; }
                       bool is_permission_set(pdf_permission perm) const { return (m_permissions & static_cast<int32_t>(perm)) ? true : false; }
                };

                pdf_encryption_options m_encryptOptions;

                class pdf_page_size_options
                {
                    public:
                        using custom_size_type = std::pair<uint32_t, uint32_t>;
                    private:
                        papersize_value::value_type m_pagesize;
                        custom_size_type m_pagesize_custom;
                        pdf_paper_size_custom m_size_opt;

                    public:
                        static constexpr uint32_t default_size = (std::numeric_limits<uint32_t>::max)();
                        pdf_page_size_options() : m_pagesize(papersize_value::USLETTER),
                                                  m_pagesize_custom({ default_size, default_size }),
                                                  m_size_opt(pdf_paper_size_custom::none) {}

                        pdf_page_size_options& set_custom_size(uint32_t width, uint32_t height) { m_pagesize_custom = { width, height }; return *this; }
                        pdf_page_size_options& set_page_size(papersize_value::value_type ps) { m_pagesize = ps; return *this; }
                        pdf_page_size_options& set_custom_option(pdf_paper_size_custom ps) { m_size_opt = ps; return *this; }
                        pdf_page_size_options& reset_sizes() { *this = pdf_page_size_options(); return *this; }

                        capability_type::supportedsizes_type get_page_size() const { if (!is_custom_size_used()) return m_pagesize; return papersize_value::CUSTOM; }
                        pdf_paper_size_custom get_custom_option() const { return m_size_opt; }
                        custom_size_type get_custom_size() const { return m_pagesize_custom; }
                        bool is_custom_size_used() const { return m_pagesize_custom.first != default_size && m_pagesize_custom.second != default_size; }
                };

                pdf_page_size_options m_size_options;

                class pdf_page_scale_options
                {
                    public:
                        using custom_scale_type = std::pair<double, double>;
                    private:
                        pdf_page_scale m_pagescale;
                        custom_scale_type m_pagescale_custom;
                    public:
                        static constexpr double default_scale = (std::numeric_limits<double>::max)();

                        pdf_page_scale_options() : m_pagescale(pdf_page_scale::none),
                                                   m_pagescale_custom({ default_scale, default_scale }) {}

                        pdf_page_scale_options& set_custom_scale(double xScale, double yScale) { m_pagescale_custom = { xScale, yScale }; return *this; }
                        pdf_page_scale_options& set_page_scale(pdf_page_scale ps) { m_pagescale = ps; return *this; }
                        pdf_page_scale_options& reset_scaling() { *this = pdf_page_scale_options(); return *this; }

                        pdf_page_scale get_page_scale() const { return m_pagescale; }
                        custom_scale_type get_custom_scale() const { return m_pagescale_custom; }
                        bool is_custom_scale_used() const { return m_pagescale_custom.first != default_scale && m_pagescale_custom.second != default_scale; }
                };

                pdf_page_scale_options m_scale_options;
        
                pdf_options() : m_useASCII(false), m_jpegQuality(70), m_orientation(pdf_orientation::portrait) {}
                pdf_options& set_author(const std::string& value) { m_author = value; return *this; }
                pdf_options& set_creator(const std::string& value) { m_creator = value; return *this; }
                pdf_options& set_producer(const std::string& value) { m_producer = value; return *this; }
                pdf_options& set_keywords(const std::string& value) { m_keywords = value; return *this; }
                pdf_options& set_subject(const std::string& value) { m_subject = value; return *this; }
                pdf_options& set_title(const std::string& value) { m_title = value; return *this; }
                pdf_options& set_use_ASCII(bool bSet) { m_useASCII = bSet; return *this; }
                pdf_options& set_jpeg_quality(int quality) { m_jpegQuality = quality; return *this; }
                pdf_options& set_orientation(pdf_orientation orient) { m_orientation = orient; return *this; }

                pdf_encryption_options& get_encryption_options() { return m_encryptOptions; }
                pdf_page_size_options& get_page_size_options() { return m_size_options; }
                pdf_page_scale_options& get_page_scale_options() { return m_scale_options; }

                std::string get_author() const { return m_author; }
                std::string get_creator() const { return m_creator; }
                std::string get_producer() const { return m_producer; }
                std::string get_keywords() const { return m_keywords; }
                std::string get_subject() const { return m_subject; }
                std::string get_title() const { return m_title; }
                bool is_use_ASCII() const { return m_useASCII; }
                int get_jpeg_quality() const { return m_jpegQuality; }
                pdf_orientation get_orientation() const { return m_orientation; }
        };
    }
}
#endif
