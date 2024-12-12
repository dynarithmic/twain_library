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
#ifndef MISC_UTILITIES_HPP
#define MISC_UTILITIES_HPP

#include <istream>
#include <ostream>

#ifdef _MSC_VER
    #pragma warning( push )  // Stores the current warning state for every warning.
    #pragma warning( disable:4996)
#endif

namespace dynarithmic
{
    namespace twain
    {
        /* constexpr Tribool class adapted from Niall Douglas tribool */
		/* http://www.nedproductions.biz/ */
        namespace tribool
        {
            enum class tribool : signed char
            {
                false_ = -1,        //!< False
                true_ = 1,          //!< True
                other = 0,          //!< Other/Indeterminate/Unknown
                indeterminate = 0,  //!< Other/Indeterminate/Unknown
                unknown = 0         //!< Other/Indeterminate/Unknown
            };
            //! \brief Explicit construction from some signed integer. <0 false, >0 true, 0 is other \ingroup tribool
            constexpr inline tribool make_tribool(int v) noexcept { return v > 0 ? tribool::true_ : v < 0 ? tribool::false_ : tribool::other; }
            //! \brief If tribool is true return false, if tribool is false return true, else return other \ingroup tribool
            constexpr inline tribool operator~(tribool v) noexcept { return static_cast<tribool>(-static_cast<signed char>(v)); }
            //! \brief If a is true and b is true, return true, if either is false return false, else return other \ingroup tribool
            constexpr inline tribool operator&(tribool a, tribool b) noexcept { return (a == tribool::true_ && b == tribool::true_) ? tribool::true_ : (a == tribool::false_ || b == tribool::false_) ? tribool::false_ : tribool::other; }
            //! \brief If a is true or b is true, return true, if either is other return other, else return false \ingroup tribool
            constexpr inline tribool operator|(tribool a, tribool b) noexcept { return (a == tribool::true_ || b == tribool::true_) ? tribool::true_ : (a == tribool::other || b == tribool::other) ? tribool::other : tribool::false_; }

            //  //! \brief If tribool is false return true, else return false \ingroup tribool
            //  constexpr inline bool operator !(tribool v) noexcept { return a==tribool::false_; }
            //! \brief If a is true and b is true, return true \ingroup tribool
            constexpr inline bool operator&&(tribool a, tribool b) noexcept { return (a == tribool::true_ && b == tribool::true_); }
            //! \brief If a is true or b is true, return true \ingroup tribool
            constexpr inline bool operator||(tribool a, tribool b) noexcept { return (a == tribool::true_ || b == tribool::true_); }

            //! \brief Return true if tribool is true. \ingroup tribool
            constexpr inline bool true_(tribool a) noexcept { return a == tribool::true_; }
            //! \brief Return true if tribool is true. \ingroup tribool
            constexpr inline bool false_(tribool a) noexcept { return a == tribool::false_; }
            //! \brief Return true if tribool is other/indeterminate/unknown. \ingroup tribool
            constexpr inline bool other(tribool a) noexcept { return a == tribool::indeterminate; }
            //! \brief Return true if tribool is other/indeterminate/unknown. \ingroup tribool
            constexpr inline bool indeterminate(tribool a) noexcept { return a == tribool::indeterminate; }
            //! \brief Return true if tribool is other/indeterminate/unknown. \ingroup tribool
            constexpr inline bool unknown(tribool a) noexcept { return a == tribool::indeterminate; }
            inline std::istream& operator>>(std::istream& s, tribool& a)
            {
                char c;
                s >> c;
                a = (c == '1') ? tribool::true_ : (c == '0') ? tribool::false_ : tribool::other;
                return s;
            }
            inline std::ostream& operator<<(std::ostream& s, tribool a)
            {
                char c = (a == tribool::true_) ? '1' : (a == tribool::false_) ? '0' : '?';
                return s << c;
            }
        }
    }
}
#ifdef _MSC_VER
    #pragma warning(pop)
#endif
#endif
