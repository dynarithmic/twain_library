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
#include <iterator>
#include <type_traits>
#include <initializer_list>
#include <vector>
#include <cstddef>

#ifdef _MSC_VER
    #pragma warning( push )  // Stores the current warning state for every warning.
    #pragma warning( disable:4996)
#endif

namespace dynarithmic
{
    namespace twain
    {
        template <typename CharT>
        class is_any_of_pred
        {
            public:
                using string_type = std::basic_string<CharT>;

                explicit is_any_of_pred(string_type chars)
                    : chars_(std::move(chars)) {
                }

                bool operator()(CharT ch) const
                {
                    return chars_.find(ch) != string_type::npos;
                }

            private:
                string_type chars_;
        };

        template <typename CharT>
        static auto is_any_of(const CharT* chars)
        {
            return is_any_of_pred<CharT>(std::basic_string<CharT>(chars));
        }

        template <typename StringType, typename Pred>
        static StringType& ltrim_if(StringType& str, Pred pred)
        {
            auto it2 = std::find_if_not(str.begin(), str.end(), pred);
            str.erase(str.begin(), it2);
            return str;
        }

        template <typename StringType, typename Pred>
        static StringType& rtrim_if(StringType& str, Pred pred)
        {
            auto it1 = std::find_if_not(str.rbegin(), str.rend(), pred);
            str.erase(it1.base(), str.end());
            return str;
        }

        template <typename StringType, typename Pred>
        static StringType ltrim_copy_if(StringType str, Pred pred)
        {
            return ltrim_if(str, pred);
        }

        template <typename StringType, typename Pred>
        static StringType rtrim_copy(StringType str, Pred pred)
        {
            return ltrim_if(str, pred);
        }

        template <typename StringType, typename Pred>
        static StringType trim_copy_if(StringType str, Pred pred)
        {
            return ltrim_if(rtrim_if(str, pred), pred);
        }

        template <typename StringType, typename Pred>
        static StringType& trim_if(StringType& str, Pred pred)
        {
            return ltrim_if(rtrim_if(str, pred), pred);
        }

        template <typename StringType>
        static decltype(auto) ltrim(StringType&& str)
        {
            if constexpr (std::is_same_v <StringType, std::wstring>)
            {
                return ltrim_if(str, [](unsigned int ch) { return iswspace(ch); });
            }
            else
            {
                return ltrim_if(str, [](char ch) { return isspace(ch); });
            }
        }

        template <typename StringType>
        static decltype(auto) rtrim(StringType&& str)
        {
            if constexpr (std::is_same_v <StringType, std::wstring>)
            {
                return rtrim_if(str, [](unsigned int ch) { return iswspace(ch); });
            }
            else
            {
                return rtrim_if(str, [](char ch) { return isspace(ch); });
            }
        }

        template <typename StringType>
        static decltype(auto) trim(StringType&& str)
        {
            return ltrim_copy(rtrim_copy(str));
        }

        template <typename StringType>
        static StringType ltrim_copy(StringType str)
        {
            return ltrim(str);
        }

        template <typename StringType>
        static StringType rtrim_copy(StringType str)
        {
            return rtrim(str);
        }

        template <typename StringType>
        static StringType trim_copy(StringType str)
        {
            return ltrim_copy(rtrim_copy(str));
        }

        template <typename StringType, typename Container>
        StringType join(const Container& c, const StringType& separator)
        {
            using CharT = typename StringType::value_type;
            using Stream = std::basic_ostringstream<CharT>;

            Stream os;

            auto it = std::begin(c);
            const auto last = std::end(c);

            if (it != last)
            {
                os << *it;
                ++it;
            }

            while (it != last)
            {
                os << separator;
                os << *it;
                ++it;
            }

            return os.str();
        }

        template <typename StringType, typename Container>
        StringType join(const Container& c, const typename StringType::value_type* separator)
        {
            return join<StringType>(c, StringType(separator));
        }

        template <typename StringType>
        StringType format_percent_args(const StringType& fmt, std::initializer_list<StringType> args)
        {
            using char_type = typename StringType::value_type;

            std::vector<StringType> values(args);

            StringType result;
            result.reserve(fmt.size());

            for (std::size_t i = 0; i < fmt.size(); ++i)
            {
                if (fmt[i] == char_type('%') && i + 1 < fmt.size())
                {
                    std::size_t j = i + 1;
                    std::size_t index = 0;

                    while (j < fmt.size() &&
                        fmt[j] >= char_type('0') &&
                        fmt[j] <= char_type('9'))
                    {
                        index = (index * 10) + static_cast<std::size_t>(fmt[j] - char_type('0'));
                        ++j;
                    }

                    if (index >= 1 && index <= values.size())
                    {
                        result += values[index - 1];
                        i = j - 1;
                        continue;
                    }
                }

                result += fmt[i];
            }

            return result;
        }
    }
}
#ifdef _MSC_VER
    #pragma warning(pop)
#endif
#endif
