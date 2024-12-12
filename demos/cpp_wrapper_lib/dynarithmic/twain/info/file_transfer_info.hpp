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
#ifndef DTWAIN_FILE_TRANSFER_INFO_HPP
#define DTWAIN_FILE_TRANSFER_INFO_HPP

#include <unordered_set>
#include <dynarithmic/twain/twain_values.hpp>

// Class that controls the naming of image files when generated
namespace dynarithmic
{
    namespace twain
    {
        class file_transfer_info
        {
            private:
                friend class info_base;
                friend class twain_source;
                std::unordered_set<filetype_value::value_type> all_file_types;
            
            public:
                bool is_supported() const { return !all_file_types.empty(); }
                bool is_supported(filetype_value::value_type ft) const { return all_file_types.find(ft) != all_file_types.end(); }
        };
    }
}   
#endif
