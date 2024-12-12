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

#include <dynarithmic/twain/session/twain_characteristics.hpp>
#include <unordered_map>
#include <string>

namespace dynarithmic
{
    namespace twain
    {
        twain_characteristics& twain_characteristics::set_dsm_search_order(int search_order) noexcept
        {
            static const std::unordered_map<int, std::string> sMap = { {0,"WSO"},
                                                                  { 1,"WOS"},
                                                                  { 2,"SWO"},
                                                                  { 3,"SOW" },
                                                                  { 4,"OWS" },
                                                                  { 5,"OSW" },
                                                                  { 6,"W" },
                                                                  { 7,"S" },
                                                                  { 8,"O" },
                                                                  { 9,"WS" },
                                                                  { 10,"WO" },
                                                                  { 11,"SW" },
                                                                  { 12,"SO" },
                                                                  { 13,"OW" },
                                                                  { 14,"OS" } };
            auto iter = sMap.find(search_order);
            if (iter != sMap.end())
                set_dsm_search_order(iter->second, "");
            return *this;
        }
    }
}

