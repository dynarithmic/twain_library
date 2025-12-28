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
#ifndef DTWAIN_UNDERLYINGTYPE_HPP
#define DTWAIN_UNDERLYINGTYPE_HPP

#include <type_traits>
#include <string>

namespace dynarithmic
{
    namespace twain
    {
        template <typename T>
        struct dtwain_underlying_type
        {
            typedef int32_t value_type;
        };

        template <>
        struct dtwain_underlying_type<unsigned short>
        {
            typedef int32_t value_type;
        };

        template <>
        struct dtwain_underlying_type<double>
        {
            typedef double value_type;
        };

        template <>
        struct dtwain_underlying_type<std::string>
        {
            typedef std::string value_type;
        };

        template <typename T>
        using dtwain_underlying_type_v = typename dtwain_underlying_type<T>::value_type;

        template <typename E>
        constexpr auto to_underlying(E e) noexcept
        {
            return static_cast<std::underlying_type_t<E>>(e);
        }
    }
}
#endif