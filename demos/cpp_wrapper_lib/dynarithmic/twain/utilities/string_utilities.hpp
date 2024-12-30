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
#ifndef STRING_UTILITIES_HPP
#define STRING_UTILITIES_HPP

#include <string>
#include <sstream>
#include <numeric>

#ifdef _MSC_VER
    #pragma warning( push )  // Stores the current warning state for every warning.
    #pragma warning( disable:4996)
#endif

namespace dynarithmic
{
    namespace twain
    {
        std::string& ltrim(std::string& str);
        std::string& rtrim(std::string& str);
        std::string ltrim_copy(std::string str);
        std::string rtrim_copy(std::string str);
        std::string trim_copy(std::string str);
        std::string& trim(std::string& str);

        template <typename Container>
        std::string join(const Container& ct, std::string separator)
        {
            return std::accumulate(ct.begin(), ct.end(), std::string(),
                [&](const auto& str, typename Container::value_type val)
                {
                    std::ostringstream strm;
                    if (!str.empty())
                        strm << str << separator << val;
                    else
                        strm << val;
                    return strm.str();
                });
        }
    }
}
#ifdef _MSC_VER
    #pragma warning(pop)
#endif
#endif
