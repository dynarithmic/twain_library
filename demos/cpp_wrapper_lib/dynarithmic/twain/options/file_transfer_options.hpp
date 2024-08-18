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
#ifndef DTWAIN_FILE_TRANSFER_OPTIONS_HPP
#define DTWAIN_FILE_TRANSFER_OPTIONS_HPP

#include <string>
#include <unordered_set>
#include <dynarithmic/twain/twain_values.hpp>

// Class that controls the naming of image files when generated
namespace dynarithmic
{
    namespace twain
    {
        class filename_increment_options
        {
            bool m_bEnable = false;
            int increment_value = 1;
            bool m_resetOnStartup = false;

            public:
                filename_increment_options& enable(bool b = true)
                { m_bEnable = b; return *this; }
            
                filename_increment_options& set_increment(int value)
                { increment_value = value; return *this; }
            
                filename_increment_options& use_reset_count(bool bSet = true)
                { m_resetOnStartup = bSet; return *this; }
            
                bool is_enabled() const { return m_bEnable; }
                int get_increment() const { return increment_value; }
                bool is_reset_count_used() const { return m_resetOnStartup; }
        };

        class file_transfer_options
        {
            std::string m_filename;
            filetype_value::value_type m_file_type;
            file_transfer_flags m_transferFlags;
            bool m_bMultiPage;
            bool m_bAutoCreateDirectory;
            filename_increment_options m_filename_increment_options;
            multipage_save_options m_multipage_save_options;

            public:
                file_transfer_options() :
                                    m_filename("temp.bmp"),
                                    m_file_type(filetype_value::bmp),
                                    m_transferFlags(file_transfer_flags::use_name),
                                    m_bMultiPage(false),
                                    m_bAutoCreateDirectory(true)
                {}

                file_transfer_options& set_type(filetype_value::value_type ft)
                { m_file_type = ft; return *this; }

                file_transfer_options& set_name(const std::string& name)
                { m_filename = name; return *this; }

                filename_increment_options& get_filename_increment_options() { return m_filename_increment_options; }
                multipage_save_options& get_multipage_save_options() { return m_multipage_save_options; }

                filetype_value::value_type get_type() const { return m_file_type; }
                std::string get_name() const { return m_filename; }

                bool can_multipage() const { return m_bMultiPage && file_type_info::get_multipage_type(m_file_type); }
                long get_multipage_type() const { return file_type_info::get_multipage_type(m_file_type); }

                file_transfer_options& set_transfer_flags(file_transfer_flags ftf)
                { m_transferFlags = ftf; return *this; }

                file_transfer_flags get_transfer_flags() const { return m_transferFlags; }
                file_transfer_options& set_multi_page(bool bSet)
                { m_bMultiPage = bSet; return *this; }

                file_transfer_options& enable_autocreate_directory(bool bEnable=true)
                { m_bAutoCreateDirectory = bEnable; return *this; }

                bool is_autocreate_directory() const { return m_bAutoCreateDirectory; }
        };

        class file_transfer_options_ex
        {
            private:
                friend class options_base;
                std::unordered_set<filetype_value::value_type> all_file_types;
                DTWAIN_SOURCE m_twain_source;
            
            public:
                file_transfer_options_ex() : m_twain_source(nullptr) {}
                bool is_supported() const { return !all_file_types.empty(); }
                bool is_supported(filetype_value::value_type ft) const { return all_file_types.find(ft) != all_file_types.end(); }
        };
    }
}   
#endif
