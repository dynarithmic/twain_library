/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2023 Dynarithmic Software.

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

#ifndef DTWAIN_TWRANGE_HPP
#define DTWAIN_TWRANGE_HPP

#include <iterator>
#include <array>
#include <algorithm>
#include <functional>
#include <type_traits>
#include <math.h>

namespace dynarithmic
{
    namespace twain
    {
        template <typename T2, typename std::enable_if<std::is_floating_point<T2>::value, bool>::type = 1>
        inline bool is_close_to(T2 value1, T2 value2)
        {
            return fabs(value1 - value2) <= 1.0e-8;
        }

        template <typename T2, typename std::enable_if<std::is_integral<T2>::value, bool>::type = 1>
        inline bool is_close_to(T2 value1, T2 value2)
        {
            return value1 == value2;
        }

        template <typename Container>
        inline bool is_valid_range(const Container& c)
        {
            if ( c.size() != 5 )
                return false;
            auto iter = c.begin();
            auto low = *iter;
            iter = std::next(iter);
            auto high = *iter;
            iter = std::next(iter);
            auto step = *iter;
            if (low > high)
                    return false;
            if (step < 0)
                    return false;
            if (is_close_to(step, typename Container::value_type(0)) && low < high)
                    return false;
            return true;
        }

        template <typename Iter>
        inline bool is_valid_range(Iter iter, Iter iter2)
        {
            if (iter == iter2)
                 return false;
            if ( std::distance(iter, iter2) != 5)
                return false;
            auto low = *iter;
            auto iterNext = std::next(iter);
            if (iterNext == iter2)
                    return false;
            auto high = *iterNext;
            iterNext = std::next(iterNext);
            if (iterNext == iter2)
                    return false;
            auto step = *iterNext;
            if (low > high)
                    return false;
            if (step < 0)
                    return false;
            if (is_close_to(step, std::iterator_traits<Iter>::value_type(0)) && low < high)
                    return false;
            return true;
        }

        template <typename T=long>
        struct twainrange_iterator : public std::iterator<std::bidirectional_iterator_tag, T, const T>
        {
            std::array<T,5> m_CurrentValue;
            T m_CurrentDerefValue;
    
            explicit twainrange_iterator() : m_CurrentDerefValue{} {}
            explicit twainrange_iterator(const std::array<T,5>& start ) : m_CurrentValue(start), m_CurrentDerefValue(start[0]) {}
    
            T operator*() { return m_CurrentDerefValue; }

            twainrange_iterator& operator++()
            {
                m_CurrentDerefValue += m_CurrentValue[2];
                if ( m_CurrentDerefValue > m_CurrentValue[1])
                    std::fill_n(m_CurrentValue.begin(), 5, (std::numeric_limits<T>::min)());
                return *this;
            }

            twainrange_iterator operator++(int)
            {
                m_CurrentDerefValue += m_CurrentValue[2];
                if ( m_CurrentDerefValue > m_CurrentValue[1])
                    std::fill_n(m_CurrentValue.begin(), 5, (std::numeric_limits<T>::min)());
                return *this;
            }
    
            twainrange_iterator& operator--()
            {
                m_CurrentDerefValue -= m_CurrentValue[2];
                return *this;
            }

            twainrange_iterator operator--(int)
            {
                m_CurrentDerefValue -= m_CurrentValue[2];
                return *this;
            }
    
            bool operator == ( const twainrange_iterator& that ) const { return m_CurrentValue == that.m_CurrentValue ; }
            bool operator != ( const twainrange_iterator& that ) const { return !(*this == that); }
        };

        template <typename T=long, typename std::enable_if<
                                std::is_floating_point<T>::value ||
                                std::is_integral<T>::value, bool>::type = 1>
        class twain_range
        {
            template<class T2, class = typename std::is_floating_point<T>::type>
            struct Modulus : std::modulus<T2>
            {};

            template<class T2>
            struct Modulus<T2, std::true_type> 
         #if ((defined(_MSVC_LANG) && _MSVC_LANG >= 201703L) || __cplusplus >= 201703L)
         #else
            : std::binary_function<T2, T2, T2>
         #endif
            {
                T2 operator()(T2 a, T2 b) const { return std::fmod(a, b); }
            };

            std::array<T, 5> m_allValues;
            std::array<T, 5> m_lastVal;
            bool m_isValid;
            twainrange_iterator<T> m_EndIter;
            public:
                typedef T value_type;
                typedef twainrange_iterator<T> iterator;
                typedef twainrange_iterator<T> const_iterator;
                twain_range() : m_isValid(false), m_lastVal({(std::numeric_limits<T>::min)(), 
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)()}),
                                                             m_EndIter(m_lastVal)
                {}
    
                twain_range(T low, T high, T step, T current = T(), T defaultVal = T()) :
                                                             m_lastVal({(std::numeric_limits<T>::min)(), 
                                                                         (std::numeric_limits<T>::min)(),
                                                                         (std::numeric_limits<T>::min)(),
                                                                         (std::numeric_limits<T>::min)(),
                                                                         (std::numeric_limits<T>::min)()}),
                                                                         m_EndIter(m_lastVal)
                {
                    m_allValues[0] = low;
                    m_allValues[1] = high;
                    m_allValues[2] = step;
                    m_allValues[3] = current;
                    m_allValues[4] = defaultVal;
                    m_isValid = is_valid_range(m_allValues) &&
                                value_exists(get_current()) &&
                                value_exists(get_default());
                }

                template <typename Iter>
                twain_range(Iter it1, Iter it2) : m_allValues{}, 
                                                 m_lastVal({(std::numeric_limits<T>::min)(), 
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)()}),
                                                             m_EndIter(m_lastVal)
                {
                    auto min_dist = (std::min)(m_allValues.size(), static_cast<size_t>(std::distance(it1, it2)));
                    std::copy(it1, it1 + min_dist, m_allValues.begin());
                    m_isValid = is_valid_range(m_allValues) &&
                                    value_exists(get_current()) &&
                                    value_exists(get_default());
                }

                template <typename Container,
                        typename std::enable_if<
                        std::is_same<typename Container::value_type, T>::value, bool>::type = 1>
                twain_range(const Container& ct) : m_lastVal({(std::numeric_limits<T>::min)(), 
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)(),
                                                             (std::numeric_limits<T>::min)()}),
                                                             m_EndIter(m_lastVal)
                {
                    std::copy(ct.begin(), ct.begin() + (std::min)(m_allValues.size(), ct.size()), m_allValues.begin());
                    m_isValid = is_valid_range(m_allValues) && 
                                value_exists(get_current()) && 
                                value_exists(get_default());
                }

                T get_min() const { return m_allValues[0]; }
                T get_max() const { return m_allValues[1]; }
                T get_step() const { return m_allValues[2]; }
                T get_current() const { return m_allValues[3]; }
                T get_default() const { return m_allValues[4]; }

                void set_min(const T& val) { m_allValues[0] = val; }
                void set_max(const T& val) { m_allValues[1] = val; }
                void set_step(const T& val) { m_allValues[2] = val; }
                void set_current(const T& val) { m_allValues[3] = val; }
                void set_default(const T& val) { m_allValues[4] = val; }
                bool is_valid() const { return m_isValid;  }

                size_t get_expand_count() const
                {
                    if (!m_isValid)
                            return 0;
                    return static_cast<size_t>(std::abs(m_allValues[1] - m_allValues[0]) / m_allValues[2] + 1);
                }

                template <typename Container=std::vector<T>, typename std::enable_if<
                    std::is_floating_point<typename Container::value_type>::value ||
                    std::is_integral<typename Container::value_type>::value, bool>::type = 1>
                Container expand_range() const
                {
                    Container ct;
                    if (!m_isValid)
                        return ct;
                    std::copy(begin(), end(), std::inserter(ct, ct.begin()));           
                    return ct;
                }

                T& operator[](size_t idx) 
                { return m_allValues[0] + idx * m_allValues[2]; }

                const T& operator[](size_t idx) const
                { return m_allValues[0] + idx * m_allValues[2]; }

                template <typename T2>
                bool value_exists(const T2& value_) const
                {
                    T2 value = value_;
                    // return immediately if step is 0
                    if ( m_allValues[2] == 0 )
                        return false;

                    // Check if value passed in is out of bounds
                    if ( value < m_allValues[0] || value > m_allValues[1])
                        return false;

                    // Get the nearest value to *pVariantIn;
                    // First get the bias value from 0
                    T lBias = 0;
                    if ( m_allValues[0] != 0 )
                        lBias = -m_allValues[0];

                    value += lBias;
                    Modulus<T2> fn;
                    auto res = fn(value, m_allValues[2]);
                    if ( res == 0 )
                        return true;
                    return false;
                }

                // iterators
                twainrange_iterator<T> begin() { return twainrange_iterator<T>(m_allValues);}
                twainrange_iterator<T> end() { return m_EndIter;}
                twainrange_iterator<T> begin() const { return twainrange_iterator<T>(m_allValues);}
                twainrange_iterator<T> end() const { return m_EndIter;}
        };
    }
}
#endif
