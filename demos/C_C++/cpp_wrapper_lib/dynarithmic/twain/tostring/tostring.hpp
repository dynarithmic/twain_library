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
#ifndef DTWAIN_TOSTRING_HPP
#define DTWAIN_TOSTRING_HPP

#include <map>
#include <type_traits>
#include <vector>
#include <algorithm>
#include <iterator>

namespace dynarithmic
{
    namespace twain
    {
        template <typename T>
        static std::pair<const char*, const char*>
            to_twain_string(const T& value, const typename std::map<T, std::pair<const char*, const char*>>& sMap)
        {
            auto iter = sMap.find(value);
            if (iter != sMap.end())
                return iter->second;
            return { "(Unknown TWAIN constant)", "(Unknown TWAIN constant)" };
        }

        template <typename Iter>
        static std::vector<const char*> 
            to_twain_string(Iter it1, Iter it2, const typename std::map<typename std::iterator_traits<Iter>::value_type, const char*>& sMap)
        {
            std::vector<const char*> ret;
            std::transform(it1, it2, std::back_inserter(ret), [&](auto val) { return dynarithmic::twain::to_twain_string(val, sMap); });
            return ret;
        }

        template <typename T, typename Container = std::vector<T>>
        static std::vector<const char*> to_twain_string(const Container& c, const typename std::map<const T, const char*>& sMap)
        {
            return to_twain_string(std::begin(c), std::end(c), sMap);
        }

        template <typename T>
        static const char* to_twain_string(const T& value, const typename std::map<const T, const char*>& sMap)
        {
            auto iter = sMap.find(value);
            if (iter != sMap.end())
                return iter->second;
            return "(Unknown TWAIN constant)";
        }

        template <typename Iter>
        static std::vector<std::pair<const char*, const char*>>
            to_twain_string(Iter it1, Iter it2, const typename std::map<typename std::iterator_traits<Iter>::value_type, std::pair<const char*, const char*>>& sMap)
        {
            std::vector<std::pair<const char*, const char*>> ret;
            std::transform(it1, it2, std::back_inserter(ret), [&](const auto& val) 
                { 
                    return dynarithmic::twain::to_twain_string(val, sMap); 
                });
            return ret;
        }

        template <typename T, typename Container = std::vector<T>>
        static std::vector<std::pair<const char*, const char*>>
            to_twain_string(const Container& c, const typename std::map<T, std::pair<const char*, const char*>>& sMap)
        {
            return to_twain_string(std::begin(c), std::end(c), sMap);
        }

    }
}
#endif