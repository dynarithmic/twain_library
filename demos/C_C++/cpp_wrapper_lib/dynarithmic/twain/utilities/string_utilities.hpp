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
        template <typename String, typename Pred>
        String& ltrim_if(String& str, Pred pred)
        {
            auto it2 = std::find_if_not(str.begin(), str.end(), pred);
    	    str.erase(str.begin(), it2);
			return str;
        }

        template <typename String, typename Pred>
        String& rtrim_if(String& str, Pred pred)
        {
            auto it1 = std::find_if_not(str.rbegin(), str.rend(), pred);
			str.erase(it1.base(), str.end());
			return str;
        }

        template <typename String, typename Pred>
        String ltrim_copy_if(String str, Pred pred)
        {
			return ltrim_if(str, pred);
        }

		template <typename String, typename Pred>
        String rtrim_copy(String str, Pred pred)
        {
			return ltrim_if(str, pred);
        }

		template <typename String, typename Pred>
        String trim_copy_if(String str, Pred pred)
        {
			return ltrim_if(rtrim_if(str, pred), pred);
        }

		template <typename String, typename Pred>
        String& trim_if(String& str, Pred pred)
        {
			return ltrim_if(rtrim_if(str, pred), pred);
        }

        template <typename String>
        String& ltrim(String& str)
        {
            if constexpr (std::is_same_v <String, std::wstring>)
            {
                return ltrim_if(str, [](unsigned char ch) { return !iswspace(ch); });
            }
            else
            {
                return ltrim_if(str, [](unsigned char ch) { return !isspace(ch); });
            }
            return str;
        }

		template <typename String>
        String& rtrim(String& str)
        {
			if constexpr (std::is_same_v <String, std::wstring>)
			{
				return rtrim_if(str, [](unsigned char ch) { return !iswspace(ch); });
			}
			else
			{
				return rtrim_if(str, [](unsigned char ch) { return !isspace(ch); });
			}
			return str;
        }

		template <typename String>
		String ltrim_copy(String str)
		{
			if constexpr (std::is_same_v <String, std::wstring>)
			{
				return ltrim_if(str, [](unsigned char ch) { return !iswspace(ch); });
			}
			else
			{
				return ltrim_if(str, [](unsigned char ch) { return !isspace(ch); });
			}
			return str;
		}

		template <typename String>
		String rtrim_copy(String str)
		{
			if constexpr (std::is_same_v <String, std::wstring>)
			{
				return rtrim_if(str, [](unsigned char ch) { return !iswspace(ch); });
			}
			else
			{
				return rtrim_if(str, [](unsigned char ch) { return !isspace(ch); });
			}
			return str;
		}

        template <typename String>
        String trim_copy(String str)
        {
            return ltrim_copy(rtrim_copy(str));
        }

		template <typename String>
		String& trim(String& str)
		{
			return ltrim_copy(rtrim_copy(str));
		}

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
