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
#ifndef DTWAIN_INFO_BASE_HPP
#define DTWAIN_INFO_BASE_HPP

#include <algorithm>
#include <iterator>
#include <unordered_set>

#include <dynarithmic/twain/capability_interface.hpp>
#include <dynarithmic/twain/types/twain_frame.hpp>
#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/source/twain_source.hpp>
#include <dynarithmic/twain/info/file_transfer_info.hpp>
#include <dynarithmic/twain/source/twain_source_base.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class info_base
        {
            public:
                static file_transfer_info get_file_transfer_info(twain_source_base& ts)
                {
                    file_transfer_info finfo;
                    auto& ci = ts.get_capability_interface();
                    auto vFormat = ci.get_cap_values(ICAP_IMAGEFILEFORMAT);
                    std::copy(vFormat.begin(), vFormat.end(), std::inserter(finfo.all_file_types, finfo.all_file_types.end()));
                    return finfo;
                }
        };
    }
}
#endif
