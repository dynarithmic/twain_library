/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2024 Dynarithmic Software.

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
#ifndef DTWAIN_CAPABILITY_INTERFACE_HPP
#define DTWAIN_CAPABILITY_INTERFACE_HPP

#ifdef __BORLANDC__
  #define BOOST_USE_VARIANT 1
#else
#if __cplusplus >= 201703L
    #define BOOST_USE_VARIANT 0
#else
    #define BOOST_USE_VARIANT 1
#endif
#endif

#if BOOST_USE_VARIANT == 0
    #include <variant>
    #define variant_type_ std::variant
    #define variant_get_ std::get
    #define variant_get_type_(v) (v).index()
#else
    #include <boost/variant.hpp>
    #define variant_type_ boost::variant
    #define variant_get_ boost::get
    #define variant_get_type_(v) (v).which()
#endif

#include <map>
#include <unordered_map>
#include <unordered_set>
#include <type_traits>
#include <memory>
#include <vector>
#include <algorithm>
#include <set>
#include "twain.h"
#include <dynarithmic/twain/types/twain_capbasics.hpp>
#include <dynarithmic/twain/types/twain_types.hpp>
#include <dynarithmic/twain/types/twain_range.hpp>
#include <dynarithmic/twain/tostring/tostring.hpp>
#include <dynarithmic/twain/types/underlying_type.hpp>

namespace dynarithmic {
namespace twain {
    class twain_source;

    struct get_operation_type
    {
        static constexpr LONG GET = DTWAIN_CAPGET;
        static constexpr LONG GET_CURRENT = DTWAIN_CAPGETCURRENT;
        static constexpr LONG GET_DEFAULT = DTWAIN_CAPGETDEFAULT;
        static constexpr LONG GET_HELP = DTWAIN_CAPGETHELP;
        static constexpr LONG GET_LABEL = DTWAIN_CAPGETLABEL;
        static constexpr LONG GET_LABELENUM = DTWAIN_CAPGETLABELENUM;
        typedef LONG value_type;
    };

    struct set_operation_type
    {
        static constexpr LONG SET = DTWAIN_CAPSET;
        static constexpr LONG SET_CONSTRAINT = DTWAIN_CAPSETCONSTRAINT;
        static constexpr LONG RESET = DTWAIN_CAPRESET;
        static constexpr LONG RESET_ALL = DTWAIN_CAPRESETALL;
        typedef LONG value_type;
    };

    struct twain_container_type
    {
        static constexpr LONG CONTAINER_INVALID = 0;
        static constexpr LONG CONTAINER_ONEVALUE = DTWAIN_CONTONEVALUE;
        static constexpr LONG CONTAINER_ARRAY = DTWAIN_CONTARRAY;
        static constexpr LONG CONTAINER_ENUMERATION = DTWAIN_CONTENUMERATION;
        static constexpr LONG CONTAINER_RANGE = DTWAIN_CONTRANGE;
        static constexpr LONG CONTAINER_DEFAULT = DTWAIN_CONTDEFAULT;
        typedef LONG value_type;
    };

    struct twain_data_type
    {
        static constexpr LONG DATATYPE_DEFAULT = -1;
        static constexpr LONG DATATYPE_INT8 = TWTY_INT8;
        static constexpr LONG DATATYPE_UINT8 = TWTY_UINT8;
        static constexpr LONG DATATYPE_BOOL = TWTY_BOOL;
        static constexpr LONG DATATYPE_INT16 = TWTY_INT16;
        static constexpr LONG DATATYPE_INT32 = TWTY_INT32;
        static constexpr LONG DATATYPE_UINT16 = TWTY_UINT16;
        static constexpr LONG DATATYPE_UINT32 = TWTY_UINT32;
        static constexpr LONG DATATYPE_STR32 = TWTY_STR32;
        static constexpr LONG DATATYPE_STR64 = TWTY_STR64;
        static constexpr LONG DATATYPE_STR128 = TWTY_STR128;
        static constexpr LONG DATATYPE_STR255 = TWTY_STR255;
        static constexpr LONG DATATYPE_STR1024 = TWTY_STR1024;
        static constexpr LONG DATATYPE_FIX32 = TWTY_FIX32;
        static constexpr LONG DATATYPE_FRAME = TWTY_FRAME;
        typedef LONG value_type;
    };

    /** The capability_interface class is used to get or set any capability that the twain_source has available.  A capability could be the paper size,
    * pixel or bit depth, the dots-per-inch, x or y resolution, author name, etc.
     */
    class capability_interface
    {
         public:
    
        capability_interface(capability_interface&& rhs) noexcept :  m_caps(std::move(rhs.m_caps)),
                          m_custom_caps(std::move(rhs.m_custom_caps)),
                          m_extended_caps(std::move(rhs.m_extended_caps)),
                          m_extendedimage_caps(std::move(rhs.m_extendedimage_caps)),
                          m_cap_cache(std::move(rhs.m_cap_cache)),
                          m_return_type(std::move(rhs.m_return_type)),
                          m_cacheable_set(std::move(rhs.m_cacheable_set)),
                          m_Source(rhs.m_Source)
        {
            rhs.m_Source = nullptr;
        }

        capability_interface& operator= (capability_interface&& rhs) noexcept
        {
            if (&rhs != this)
            {
                m_caps = std::move(rhs.m_caps);
                m_custom_caps = std::move(rhs.m_custom_caps);
                m_extended_caps = std::move(rhs.m_extended_caps);
                m_extendedimage_caps = std::move(rhs.m_extendedimage_caps);
                m_cap_cache = std::move(rhs.m_cap_cache);
                m_cacheable_set = std::move(rhs.m_cacheable_set);
                m_return_type = rhs.m_return_type;
                m_Source = rhs.m_Source;
                rhs.m_Source = nullptr;
            }
            return *this;
        }

        capability_interface() : m_Source(nullptr), m_return_type{}
        {}

        capability_interface(const capability_interface&) = delete;
        capability_interface& operator= (const capability_interface&) = delete;

        typedef CAP_SUPPORTEDCAPS_::value_type twain_cap_type;
        typedef std::string twain_string_type;

        #if BOOST_USE_VARIANT == 0
        typedef std::variant<long, std::string, double, twain_frame<double>, twain_frame<long>> customcap_variant_type;
        #else
        typedef boost::variant<long, std::string, double, twain_frame<double>, twain_frame<long>> customcap_variant_type;
        #endif

        enum class twain_type
        {
            bool_type,
            int8_type,
            int16_type, 
            int32_type,
            uint8_type,
            uint16_type,
            uint32_type,
            string_type,
            fix32_type, 
            twain_frame_d_type
        };

        using twaintype_variant_type = variant_type_<
            bool,
            int8_t,
            int16_t,
            int32_t,
            uint8_t,
            uint16_t,
            uint32_t,
            int64_t,
            uint64_t,
            std::string,
            double,
            twain_frame<double>,
            long  // for generic integer types
            >;

        using twain_vector_variant_type = variant_type_ <
            std::vector<bool>,
            std::vector<int8_t>,
            std::vector<int16_t>,
            std::vector<int32_t>,
            std::vector<uint8_t>,
            std::vector<uint16_t>,
            std::vector<uint32_t>,
            std::vector<int64_t>,
            std::vector<uint64_t>,
            std::vector<std::string>,
            std::vector<double>,
            std::vector<twain_frame<double>>,
            std::vector<long>  // for generic integer types
        >;

        struct twain_cap_info
        {
            twain_string_type name;
            long supported_ops;
            long data_type;
            std::array<int8_t, 7> container_type; 
            twain_cap_info(const twain_string_type& n = "", int ops = 0, long type_=TWTY_INT16, long =-1) :
                            name(n), supported_ops(ops), data_type(type_), container_type{-1,-1,-1,-1,-1,-1,-1} {}
        };

        typedef std::map<twain_cap_type, twain_cap_info> source_cap_info;
        struct cap_return_type
        {
            bool return_value;
            int32_t error_code;
        };

        enum {CAP_CALLBACK_GET_BEGIN, CAP_CALLBACK_GET_END, CAP_CALLBACK_SET_BEGIN, CAP_CALLBACK_SET_END};

        // This class tells us whether to use MSG_GET, MSG_GETDEFAULT, MSG_GETCURRENT when retrieving capability info
        class getcap_operation_info
        {
            public:
                get_operation_type::value_type getop_type;
                twain_container_type::value_type container_type;
                twain_data_type::value_type data_type;
                bool expand_if_range;

                getcap_operation_info() : getop_type(get_operation_type::GET),
                                    container_type(twain_container_type::CONTAINER_DEFAULT),
                                    data_type(twain_data_type::DATATYPE_DEFAULT), expand_if_range(false)
                {}
            
                getcap_operation_info& set_operation(get_operation_type::value_type gt)
                { getop_type = gt; return *this; }
            
                getcap_operation_info& set_expand_range(bool bSet = true)
                { expand_if_range = bSet; return *this; }
            
                getcap_operation_info& set_container_type(twain_container_type::value_type ct)
                { container_type = ct; return *this; }
            
                getcap_operation_info& set_data_type(twain_data_type::value_type dt)
                { data_type = dt; return *this; }
            
                get_operation_type::value_type get_operation() const { return getop_type; }
                twain_container_type::value_type get_container_type() const { return container_type; }
            
                // tells if we want to expand the range values.  There could be thousands of values in the range, so be careful if this is set to true
                bool get_expand_range() const { return expand_if_range; }
                twain_data_type::value_type get_data_type() const { return data_type; }
        };
            
        // This class tells us whether to use MSG_SET, MSG_SETDEFAULT, MSG_SETCURRENT, MSG_SETCONSTRAINT, MSG_RESET,
        // or MSG_RESETALL when setting capability info
        class setcap_operation_info
        {
            public:
                set_operation_type::value_type setter_type;
                twain_container_type::value_type container_type;
                twain_data_type::value_type data_type;
            
                setcap_operation_info() : setter_type(set_operation_type::SET),
                                    container_type(twain_container_type::CONTAINER_DEFAULT),
                                    data_type(twain_data_type::DATATYPE_DEFAULT)
                {}
            
                setcap_operation_info& set_operation(set_operation_type::value_type st)
                { setter_type = st; return *this; }
            
                setcap_operation_info& set_container_type(twain_container_type::value_type ct)
                { container_type = ct; return *this; }
            
                setcap_operation_info& set_data_type(twain_data_type::value_type dt)
                { data_type = dt; return *this;}
            
                set_operation_type::value_type get_operation() const { return setter_type; }
                twain_container_type::value_type get_container_type() const { return container_type; }
                twain_data_type::value_type get_data_type() const { return data_type; }
        };

    private:

        DTWAIN_SOURCE m_Source;
        mutable source_cap_info m_caps;
        mutable source_cap_info m_custom_caps;
        mutable source_cap_info m_extended_caps;
        mutable source_cap_info m_extendedimage_caps;
        using cache_vector_type = std::vector<twaintype_variant_type>;
        using capability_cache = std::unordered_map<int, cache_vector_type>;
        using cache_set_type = std::unordered_set<int>;
        mutable capability_cache m_cap_cache;
        cache_set_type m_cacheable_set;
        mutable cap_return_type m_return_type;

        struct capability_info_struct;

        template <typename Container>
        void copy_to_cache(const Container& ct, int capvalue) const
        {
            auto iter = m_cap_cache.insert({capvalue, cache_vector_type()}).first;
            auto& vect = iter->second;
            for (auto val : ct)
                vect.push_back(val);
        }

        template <typename Container>
        bool copy_from_cache(Container& ct, int capvalue) const
        {
            using vType = typename Container::value_type;
            auto iter = m_cap_cache.find(capvalue);
            if (iter == m_cap_cache.end())
                return false;
            auto& vect = iter->second;
            std::transform(std::begin(vect), std::end(vect), std::inserter(ct, ct.end()),
                            [&](auto& vt)
                            {
                                return variant_get_<vType> (vt);
                            });
            return true;
        }
            
        void initialize_cached_set()
        {
            static std::vector<int> excluded_set = { CAP_DEVICEONLINE, CAP_DUPLEXENABLED, 
                                                     CAP_ENABLEDSUIONLY, ICAP_BITDEPTH, ICAP_FRAMES, 
                                                     ICAP_XRESOLUTION, ICAP_YRESOLUTION };
            for (auto& e : excluded_set)
                m_cacheable_set.erase(e);
        }

        bool fill_caps()
        {
            char szBuffer[256];
            m_caps.clear();
            m_custom_caps.clear();
            m_cacheable_set.clear();
            m_extendedimage_caps.clear();
            m_extended_caps.clear();
            auto vCaps = get_cap_values<std::vector<CAP_SUPPORTEDCAPS_::value_type>>(CAP_SUPPORTEDCAPS);
            std::for_each(vCaps.begin(), vCaps.end(), [&](const CAP_SUPPORTEDCAPS_::value_type capVal)
            {
                API_INSTANCE DTWAIN_GetNameFromCapA(capVal, szBuffer, 255);
                LONG ops;
                DTWAIN_BOOL theOpts = API_INSTANCE DTWAIN_GetCapOperations(m_Source, capVal, &ops);
                DTWAIN_LONG theType = API_INSTANCE DTWAIN_GetCapDataType(m_Source, capVal);
                if (theOpts)
                    m_caps[capVal] = { szBuffer, ops, theType };
                else
                    m_caps[capVal] = { szBuffer, -1, theType };
                m_cacheable_set.insert(capVal);
            });
            initialize_cached_set();

            // get the custom caps
            std::copy_if(m_caps.begin(), m_caps.end(),
                std::inserter(m_custom_caps, m_custom_caps.end()),
                [&](const source_cap_info::value_type& vt) { return vt.first >= CAP_CUSTOMBASE; });

            // get the extended caps
            auto extcaps = get_cap_values <std::set<CAP_EXTENDEDCAPS_::value_type>>(CAP_EXTENDEDCAPS);
            std::set<int32_t> ordered_caps(m_cacheable_set.begin(), m_cacheable_set.end());
            std::set<int32_t> result;
            std::set_intersection(extcaps.begin(), extcaps.end(),
                ordered_caps.begin(), ordered_caps.end(),
                std::inserter(result, result.begin()));
            for (auto& r : result)
            {
                auto iter = m_caps.find(r);
                if (iter != m_caps.end())
                    m_extended_caps.insert({ iter->first, iter->second });
            }

            // get the extended image caps
            auto vExtInfo = get_cap_values<std::vector<ICAP_SUPPORTEDEXTIMAGEINFO_::value_type>>(ICAP_SUPPORTEDEXTIMAGEINFO);
            for (auto s : vExtInfo)
            {
                API_INSTANCE DTWAIN_GetNameFromCapA(s + 1000, szBuffer, 255);
                DTWAIN_LONG theType = API_INSTANCE DTWAIN_GetCapDataType(m_Source, s + 1000);
                m_extendedimage_caps[s] = { szBuffer, DTWAIN_CO_GET, theType };
            }
            return !vCaps.empty();
        }

        template <typename Container, typename Cap>
        cap_return_type get_caps_impl(Container& ct, const getcap_operation_info& gcType) const
        {
            static_assert(std::is_same<typename Container::value_type, typename Cap::value_type>(),
                "Container has incompatible type");
            return get_cap_values<Cap>(ct, gcType);
        }
            
        template <typename Container, typename Cap>
        cap_return_type set_caps_impl(const Container& ct, const setcap_operation_info& scType) const
        {
            static_assert(std::is_same<typename Container::value_type, typename Cap::value_type>(),
                "Container has incompatible type");
            return set_cap_values<Cap>(ct, scType);
        }

        /// Gets all of the values of a capability.
        /// 
        /// The types of retrieval (the last parameter) can be one of the following:
        /// <ul>
        /// <li>capability_interface::get() -- get all values
        /// <li>capability_interface::get_current() -- get current values
        /// <li>capability_interface::get_default() -- get default values
        ///  </ul>
        /// The values will be returned in a container provided by the user.  
        /// The container should support insert() and clear() functions, thus a std::vector<capability_type> is the universal option.
        /// @param[in,out] container A container that will be filled in with the capability's value(s).  
        /// @param[in] capvalue The capability to retrieve the values from
        /// @param[in] gcType The type of capability retrieval.  By default capability::get() (getting all values)
        ///         
        /// @returns A cap_return_type describing the status of the capability value retrieval
        /// @note For most capabilities, the get() retrieval option will cache the returned values
        template <typename Container>
        cap_return_type get_cap_values(Container& container, int capvalue,
            const getcap_operation_info& gcType = getcap_operation_info()) const
        {
            if (!m_Source)
                return { false, DTWAIN_ERR_BAD_SOURCE };
            if (!m_caps.empty() && m_caps.find(capvalue) == m_caps.end())
                return { false, DTWAIN_ERR_CAP_NO_SUPPORT };

            const bool is_cache =
                (gcType.get_operation() == get_operation_type::GET) &&
                (m_cacheable_set.find(capvalue) != m_cacheable_set.end());

            if (is_cache)
            {
                container.clear();
                if (copy_from_cache(container, capvalue))
                    return { true, DTWAIN_NO_ERROR };
            }

            twain_array ta;
            auto getToUse = static_cast<LONG>(gcType.get_operation());
            auto containerType = gcType.get_container_type();
            auto dataType = gcType.get_data_type();

            bool retVal = API_INSTANCE DTWAIN_GetCapValuesEx2(m_Source, capvalue,
                getToUse, containerType, dataType, ta.get_array_ptr()) != 0;
            if (!retVal)
                return { false, API_INSTANCE DTWAIN_GetLastError() };
            container.clear();
            twain_array_copy_traits::copy_from_twain_array(ta, ta.get_count(), container);
            if (is_cache)
                copy_to_cache(container, capvalue);
            return { retVal, DTWAIN_NO_ERROR };
        }

        template <typename T, typename Container>
        cap_return_type get_cap_values(Container& C, const getcap_operation_info& gcType = getcap_operation_info()) const
        {
            return get_cap_values(C, T::cap_value, gcType);
        }

    public:
        typedef source_cap_info::value_type value_type;
        typedef std::string camera_name_type;
        DTWAIN_SOURCE get_source() const { return m_Source; }
            
        static getcap_operation_info get()
        {
            return getcap_operation_info().set_operation(get_operation_type::GET);
        }
            
        static getcap_operation_info get_current()
        {
            return getcap_operation_info().set_operation(get_operation_type::GET_CURRENT);
        }
            
        static getcap_operation_info get_default()
        {
            return getcap_operation_info().set_operation(get_operation_type::GET_DEFAULT);
        }

        static getcap_operation_info get_help()
        {
            return getcap_operation_info().set_operation(get_operation_type::GET_HELP);
        }

        static getcap_operation_info get_label()
        {
            return getcap_operation_info().set_operation(get_operation_type::GET_LABEL);
        }

        static getcap_operation_info get_labelenum()
        {
            return getcap_operation_info().set_operation(get_operation_type::GET_LABELENUM);
        }

        static setcap_operation_info set()
        {
            return setcap_operation_info().set_operation(set_operation_type::SET);
        }
            
        static setcap_operation_info reset()
        {
            return setcap_operation_info().set_operation(set_operation_type::RESET);
        }

        static setcap_operation_info reset_all()
        {
            return setcap_operation_info().set_operation(set_operation_type::RESET_ALL);
        }
            
        static setcap_operation_info set_constraint()
        {
            return setcap_operation_info().set_operation(set_operation_type::SET_CONSTRAINT);
        }
            
        size_t get_num_caps() const { return m_caps.size(); }
            
        size_t get_num_custom_caps() const
        {
            return m_custom_caps.size();
        }

        size_t get_num_extended_caps() const
        {
            return m_extended_caps.size();
        }

        size_t get_num_extendedimage_caps() const
        {
            return m_extendedimage_caps.size();
        }

        /// Gets all of the capabilities supported by the TWAIN source.
        /// 
        /// Returns all of the capabilities, including custom and extended capabilities.
        /// @returns A container consisting of twain_cap_type.  By default, the container is std::vector<twain_cap_type>
        /// @see get_custom_caps() get_extended_caps()
        template <typename Container = std::vector<twain_cap_type>>
        Container get_caps() const
        {
            Container C;
            std::transform(m_caps.begin(), m_caps.end(), std::inserter(C, C.end()), []
                                (const source_cap_info::value_type& v) { return v.first; });
            return C;
        }
            
        /// Gets all of the custom capabilities supported by the TWAIN source.
        /// 
        /// @returns A container consisting of twain_cap_type.  By default, the container is std::vector<twain_cap_type>
        /// @see get_caps() get_extended_caps()
        template <typename Container = std::vector<twain_cap_type>>
        Container get_custom_caps() const
        {
            Container C;
            std::transform(m_custom_caps.begin(), m_custom_caps.end(), std::inserter(C, C.end()), []
                                (const source_cap_info::value_type& v) { return v.first; });
            return C;
        }
            
        /// Gets all of the extended capabilities supported by the TWAIN source.
        /// 
        /// @returns A container consisting of twain_cap_type.  By default, the container is std::vector<twain_cap_type>
        /// @see get_caps() get_custom_caps()
        template <typename Container = std::vector<twain_cap_type>>
        Container get_extended_caps() const
        {
            Container C;
            std::transform(m_extended_caps.begin(), m_extended_caps.end(), std::inserter(C, C.end()), []
                                (const source_cap_info::value_type& v) { return v.first; });
            return C;
        }

        /// Returns true if the capability is an extended capability supported by the device
        /// 
        /// @params[in] cap The capability value to test 
        /// @returns **true** if the capability is an extended capability, **false** otherwise
        bool is_extended_cap(twain_cap_type cap) const noexcept
        {
            return m_extended_caps.count(cap)?true:false;
        }

        /// Returns true if the capability is a custom capability supported by the device
        /// 
        /// @params[in] cap The capability value to test 
        /// @returns **true** if the capability is an extended capability, **false** otherwise
        bool is_custom_cap(twain_cap_type cap) const noexcept
        {
            return m_custom_caps.count(cap)?true:false;
        }

        template <typename Container = std::vector<twain_cap_type>>
        Container get_extendedimage_caps() const
        {
            Container C;
            std::transform(m_extendedimage_caps.begin(), m_extendedimage_caps.end(), std::inserter(C, C.end()), []
            (const source_cap_info::value_type& v) { return v.first; });
            return C;
        }

        static std::string get_cap_name_s(twain_cap_type t)
        {
            char capName[256];
            API_INSTANCE DTWAIN_GetNameFromCapA(t, capName, 255);
            return capName;
        }

        template <typename ContainerIn=std::vector<uint16_t>, typename ContainerOut=std::vector<std::string>>
        static ContainerOut get_cap_name_s(const ContainerIn& ct)
        {
            ContainerOut ret;
            char capName[256];
            std::transform(ct.begin(), ct.end(), std::inserter(ret, ret.begin()), [&](uint16_t val) 
                { API_INSTANCE DTWAIN_GetNameFromCapA(val, capName, 255); return capName; });
            return ret;
        }

        std::string get_cap_name(twain_cap_type t) const
        {
            source_cap_info::const_iterator iter = m_caps.find(t);
            if ( iter != m_caps.end() )
                return iter->second.name;
            return capability_interface::get_cap_name_s(t);
        }

        const source_cap_info& get_source_cap_info() const;
        void remove_from_cache(const std::vector<int>& removed);
        void add_to_cache(const std::vector<int>& added);
        void clear_cache();
        source_cap_info::iterator begin() { return m_caps.begin(); }
        source_cap_info::iterator end() { return m_caps.end(); }
        source_cap_info::const_iterator cbegin();
        source_cap_info::const_iterator cend();
            
        template <typename Container>
        cap_return_type set_cap_values(const Container& C, int capvalue,
                                        const setcap_operation_info& scType = setcap_operation_info()) const
        {
            const auto theSource = m_Source;
            if (!theSource)
                return {false, DTWAIN_ERR_BAD_SOURCE};
            if (!m_caps.empty() && m_caps.find(capvalue) == m_caps.end())
                return {false, DTWAIN_ERR_CAP_NO_SUPPORT};

            twain_array ta;
            BOOL retval = FALSE;
            if ( C.empty() )
                retval = API_INSTANCE DTWAIN_SetCapValues(theSource, capvalue, DTWAIN_CAPRESET, NULL);
            else
            { 
                twain_array_copy_traits::copy_to_twain_array(theSource, ta, capvalue, C);
                retval = API_INSTANCE DTWAIN_SetCapValuesEx2(theSource, 
                                                             capvalue, 
                                                             static_cast<LONG>(scType.get_operation()),
                                                             scType.get_container_type(), 
                                                             scType.get_data_type(), 
                                                             ta.get_array());
            }
            LONG last_error = DTWAIN_NO_ERROR;
            if (!retval)
                last_error = API_INSTANCE DTWAIN_GetLastError();
            return {retval ? true : false, last_error};
        }

        template <typename T, typename Container = std::vector<typename T::value_type>>
        cap_return_type set_cap_values(const Container& C, const setcap_operation_info& scType = setcap_operation_info()) const
        {
            static_assert(std::is_same<typename T::value_type, typename Container::value_type>::value == 1,
                            "Capability type does not match container value type");
            return set_cap_values(C, T::cap_value, scType);
        }
            
        ///////////////////////////////////////////////////////////////////////////////////////////////
        template <typename CapType>
        std::pair<bool, int> is_capvalue_supported_impl(const CapType& capvalue, CAP_SUPPORTEDCAPS_::value_type capToTest) const
        {
            if (!m_Source)
                return {false, DTWAIN_ERR_BAD_SOURCE};
            if (!m_caps.empty() && m_caps.find(capToTest) == m_caps.end())
                return {false, DTWAIN_ERR_CAP_NO_SUPPORT};
            
            const bool is_cache = (m_cacheable_set.find(capToTest) != m_cacheable_set.end());
            
            if (is_cache)
            {
                auto iter = m_cap_cache.find(capToTest);
                if (iter != m_cap_cache.end())
                {
                    auto& vect = iter->second;
                    auto cType = get_cap_container_type(capToTest, get());
                    if ( cType != twain_container_type::CONTAINER_RANGE)
                    {
                        auto iter = std::find_if(std::begin(vect), std::end(vect), 
                                                 [&](cache_vector_type::value_type& vt)
                                                 { return variant_get_<CapType>(vt) == capvalue; });
                        if ( iter != vect.end() )
                            return { true, 1 };
                        return {false, DTWAIN_ERR_CAP_NO_SUPPORT};
                    }
                    else
                    {
                        std::vector<typename dtwain_underlying_type<CapType>::value_type> vc;
                        std::transform(std::begin(vect), std::end(vect), std::back_inserter(vc), 
                                  [&](cache_vector_type::value_type& vt)
                                        { return variant_get_<CapType>(vt); });

                        twain_range<dtwain_underlying_type_v<CapType>> tr(vc.begin(), vc.end());
                        bool found = tr.value_exists(static_cast<dtwain_underlying_type_v<CapType>>(capvalue));
                        if ( found )
                            return {found, 1};
                        return {found, 0};
                    }
                }
            }
            return {true, 0}; // Need to get the values from device.    
        }

        std::pair<bool, int> is_capvalue_supported_impl_s(const std::string& capvalue, CAP_SUPPORTEDCAPS_::value_type capToTest) const
        {
            if (!m_Source)
                return {false, DTWAIN_ERR_BAD_SOURCE};
            if (!m_caps.empty() && m_caps.find(capToTest) == m_caps.end())
                return {false, DTWAIN_ERR_CAP_NO_SUPPORT};
            
            const bool is_cache = (m_cacheable_set.find(capToTest) != m_cacheable_set.end());
            
            if (is_cache)
            {
                auto iter = m_cap_cache.find(capToTest);
                if (iter != m_cap_cache.end())
                {
                    auto& vect = iter->second;
                    auto iter = std::find_if(std::begin(vect), std::end(vect), 
                                                 [&](cache_vector_type::value_type& vt)
                                                 { return variant_get_<std::string>(vt) == capvalue; });
                    if ( iter != vect.end() )
                        return { true, 1 };
                    return {false, DTWAIN_ERR_CAP_NO_SUPPORT};
                }
            }
            return {true, 0}; // Need to get the values from device.    
        }

        bool is_long_type(long data_type) const
        {
            static std::unordered_set<long> m_type = {TWTY_BOOL, TWTY_INT16, TWTY_INT8, TWTY_INT32, TWTY_UINT16, TWTY_UINT8, TWTY_UINT32};
            return m_type.count(data_type)?true:false;
        }

        bool is_string_type(long data_type) const
        {
            static std::unordered_set<long> m_type = {TWTY_STR32, TWTY_STR64, TWTY_STR128, TWTY_STR255, TWTY_STR1024};
            return m_type.count(data_type)?true:false;
        }

        bool is_fix32_type(long data_type) const
        {
            return data_type == TWTY_FIX32;
        }

        bool is_frame_type(long data_type) const
        {
            return data_type == TWTY_FRAME;
        }

        /// Gets all of the values of a capability.
        /// 
        /// The types of retrieval (the last parameter) can be one of the following:
        /// <ul>
        /// <li>capability_interface::get() -- get all values
        /// <li>capability_interface::get_current() -- get current values
        /// <li>capability_interface::get_default() -- get default values
        ///  </ul>
        /// The values will be returned in a container provided by the user.  
        /// The container should support insert() and clear() functions, thus a std::vector<capability_type> is the universal option.
        /// @param[in] capvalue The capability to retrieve the values from
        /// @param[in] gcType The type of capability retrieval.  By default capability::get() (getting all values)
        ///         
        /// @returns A container that will be filled in with the capability's value(s).  
        /// @note For most capabilities, the get() retrieval option will cache the returned values
        template <typename Container=std::vector<uint16_t>>
        Container get_cap_values(int cap, const getcap_operation_info& gcType = getcap_operation_info()) const
        {
            Container ct {};
            m_return_type = get_cap_values(ct, cap, gcType);
            return ct;
        }

        /// Gets all of the values of a capability.
        /// 
        /// The template argument must be one of the predefined TWAIN capability constants, with a trailing underscore (_)
        /// Example:
        /// \code {.cpp}
        /// twain_source source;
        /// auto& ci = source.get_capability_interface();
        /// auto& allvalues = ci.get_cap_values<ICAP_XRESOLUTION_>(); // gets all the x-resolution values
        /// //...
        /// \endcode
        /// The second template parameter defaults to std::vector<data_type>.  This is the type of container that will be returned.
        /// The "data_type" is the data type that the capability supports (for example, ICAP_XRESOLUTION has a **double** type).
        /// 
        /// The types of retrieval (the last parameter) can be one of the following:
        /// <ul>
        /// <li>capability_interface::get() -- get all values
        /// <li>capability_interface::get_current() -- get current values
        /// <li>capability_interface::get_default() -- get default values
        ///  </ul>
        /// The second template parameter defaults to std::vector<data_type>.  This is the type of container that will be returned.
        /// The "data_type" is the data type that the capability supports (for example, ICAP_XRESOLUTION has a **double** type).
        /// If providing a user-defined container, the container should support insert() and clear() functions.
        /// @param[in] gcType The type of capability retrieval.  By default capability::get() (getting all values)
        ///         
        /// @returns A container that will be filled in with the capability's value(s).  
        /// @note For most capabilities, the get() retrieval option will cache the returned values
        template <typename T, typename Container=std::vector<typename T::value_type>>
        Container get_cap_values(const getcap_operation_info& gcType = getcap_operation_info()) const
        {
            Container C;
            m_return_type = get_cap_values(C, T::cap_value, gcType);
            return C;
        }


        template <typename Container=std::vector<uint16_t>>
        capability_interface& set_custom(int cap, const Container& ct, const setcap_operation_info& setType = setcap_operation_info())
        {
            if (std::is_same<typename Container::value_type, bool>::value)
            {
                std::vector<uint16_t> ctTemp;
                std::copy(ct.begin(), ct.end(), std::back_inserter(ctTemp));
                if (ctTemp.empty())
                    m_return_type = set_cap_values(ctTemp, cap, reset());
                else
                    m_return_type = set_cap_values(ctTemp, cap, setType);
            }
            else
            {
                if (ct.empty())
                    m_return_type = set_cap_values(ct, cap, reset());
                else
                    m_return_type = set_cap_values(ct, cap, setType);
            }
            return *this;
        }

        twain_vector_variant_type get_cap_values_v(int cap, const getcap_operation_info& gcType = getcap_operation_info()) const
        {
            twain_vector_variant_type retValue;
            // get the container type and call the correct version

            auto data_type = m_caps[cap].data_type; 
            if ( is_long_type(data_type))
            {
                auto v = get_cap_values<std::vector<long>>(cap, gcType);
                switch (data_type)
                {
                    case TWTY_BOOL:
                    {
                        std::vector<bool> ct;
                        std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()),
                            [&](long value) { return value?true:false; });
                        retValue = ct;
                    }
                    break;

                    case TWTY_INT8:
                    {
                        std::vector<int8_t> ct;
                        std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()),
                            [&](long value) { return static_cast<int8_t>(value); });
                        retValue = ct;
                    }
                    break;

                    case TWTY_INT16:
                    {
                        std::vector<int16_t> ct;
                        std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()),
                            [&](long value) { return static_cast<int16_t>(value); });
                        retValue = ct;
                    }
                    break;

                    case TWTY_INT32:
                    {
                        std::vector<int32_t> ct;
                        std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()),
                            [&](long value) { return static_cast<int32_t>(value); });
                        retValue = ct;
                    }
                    break;

                    case TWTY_UINT8:
                    {
                        std::vector<uint8_t> ct;
                        std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()),
                            [&](long value) { return static_cast<uint8_t>(value); });
                        retValue = ct;
                    }
                    break;

                    case TWTY_UINT16:
                    {
                        std::vector<uint16_t> ct;
                        std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()),
                            [&](long value) { return static_cast<uint16_t>(value); });
                        retValue = ct;
                    }
                    break;

                    case TWTY_UINT32:
                    {
                        std::vector<uint32_t> ct;
                        std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()),
                            [&](long value) { return static_cast<uint32_t>(value); });
                        retValue = ct;
                    }
                    break;
                }
            }
            else
            if ( is_string_type(data_type))
            {
                std::vector<std::string> ct;
                auto v = get_cap_values<std::vector<std::string>>(cap, gcType);
                std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()), [&](const std::string& value)
                { return value;});
                return ct;
            }
            else
            if ( is_fix32_type(data_type))
            {
                std::vector<double> ct;
                auto v = get_cap_values<std::vector<double>>(cap, gcType);
                std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()), [&](double value)
                { return value;});
                return ct;
            }
            else
            if ( is_frame_type(data_type))
            { 
                std::vector<twain_frame<>> ct;
                auto v = get_cap_values<std::vector<twain_frame<>>>(cap, gcType);
                std::transform(v.begin(), v.end(), std::inserter(ct, ct.begin()), [&](const twain_frame<>& value)
                { return value;});
                return ct;
            }
            return retValue;
        }

        template <typename Container=std::vector<uint16_t>>
        Container get_cap_operations(twain_cap_type capValue) const
        {
            Container C;
            const uint16_t allops[] = { TWQC_GET,
                                        TWQC_SET ,
                                        TWQC_GETDEFAULT,
                                        TWQC_GETCURRENT ,
                                        TWQC_RESET,
                                        TWQC_SETCONSTRAINT,
                                        TWQC_GETHELP,
                                        TWQC_GETLABEL,
                                        TWQC_GETLABELENUM };

            auto iter = m_caps.find(capValue);
            if (iter != m_caps.end())
            {
                auto supported_ops = iter->second.supported_ops;
                std::copy_if(std::begin(allops), std::end(allops), std::inserter(C, C.begin()), [&](uint16_t op) { return supported_ops & op; });
            }
            return C;
        }

        twain_container_type::value_type get_cap_container_type(int capvalue, const getcap_operation_info& gcType) const
        {
            auto iter = m_caps.find(capvalue);
            if (!m_caps.empty() && iter == m_caps.end())
                return twain_container_type::CONTAINER_INVALID;
            auto getop = gcType.get_operation();
            int idx = -1;
            switch (getop)
            {
                case get_operation_type::GET:
                    idx = 0;
                break;
                case get_operation_type::GET_CURRENT:
                    idx = 1;
                break;
                case get_operation_type::GET_DEFAULT:
                    idx = 2;
                break;
            }
            if ( iter->second.container_type[idx] != -1 )
                return static_cast<twain_container_type::value_type>(iter->second.container_type[idx]);
        
            auto container_type = API_INSTANCE DTWAIN_GetCapContainer(m_Source, capvalue, static_cast<LONG>(getop));
            iter->second.container_type[idx] = static_cast<int8_t>(container_type);
            return static_cast<twain_container_type::value_type>(container_type);
        }
            
            
        template <typename T>
        twain_container_type::value_type get_cap_container_type(const getcap_operation_info& gcType) const
        {
            return get_cap_container_type(T::cap_value, gcType);
        }
            
        twain_container_type::value_type get_cap_container_type(int capvalue, const setcap_operation_info& scType) const
        {
            auto iter = m_caps.find(capvalue);
            if (m_caps.find(capvalue) == m_caps.end())
                return twain_container_type::CONTAINER_INVALID;

            auto setop = scType.get_operation();
            int idx = -1;
            switch (setop)
            {
                case set_operation_type::SET:
                    idx = 3;
                break;
                case set_operation_type::SET_CONSTRAINT:
                    idx = 4;
                break;
                case set_operation_type::RESET:
                    idx = 5;
                break;
            }
            if ( iter->second.container_type[idx] != -1 )
                return static_cast<twain_container_type::value_type>(iter->second.container_type[idx]);
            
            auto container_type = API_INSTANCE DTWAIN_GetCapContainer(m_Source, capvalue,
                                                            static_cast<LONG>(scType.get_operation()));
            iter->second.container_type[idx] = static_cast<int8_t>(container_type);
            return static_cast<twain_container_type::value_type>(container_type);
        }
            
        template <typename T>
        twain_container_type get_cap_container_type(const setcap_operation_info& scType) const
        {
            return get_cap_values(T::cap_value, scType);
        }

        int32_t get_cap_data_type(int capValue)
        {
            auto iter = m_caps.find(capValue);
            if ( iter != m_caps.end())
                return iter->second.data_type;
            return -1;
        }
        
        bool attach(DTWAIN_SOURCE s)
        {
            m_Source = s;
            return fill_caps();
        }

        void detach()
        {
            m_Source = nullptr;
            m_cap_cache.clear();
            m_cacheable_set.clear();
        }
        
        template <typename T>
        bool is_cap_supported() const
        {
            static_assert(T::cap_value != 0, "Invalid capability type used");
            static_assert(T::template cap_value<CAP_CUSTOMBASE>, "Cannot use this function to test custom cap support.  Use is_cap_supported(int) overload instead");
            return is_cap_supported(T::cap_value);
        }

        bool is_cap_supported(twain_cap_type capValue) const
        {
            return m_caps.find(capValue) != m_caps.end();
        }

        template <typename T>
        bool is_extendedimage_cap_supported() const
        {
            static_assert(T::cap_value != 0, "Invalid capability type used");
            static_assert(T::template cap_value<CAP_CUSTOMBASE>,
                "Cannot use this function to test custom cap support.  Use is_extendedimagecap_supported(int) overload instead");
            return is_extendedimage_cap_supported(T::cap_value);
        }

        bool is_extendedimage_cap_supported(twain_cap_type capValue) const
        {
            return m_extendedimage_caps.find(capValue) != m_extendedimage_caps.end();
        }

        template <typename Cap, typename std::enable_if<
                        std::is_floating_point<typename Cap::value_type>::value ||
                        std::is_integral<typename Cap::value_type>::value, bool>::type = 1>
        bool is_capvalue_supported(const typename Cap::value_type& capValue, typename CAP_SUPPORTEDCAPS_::value_type capToTest) const
        {
            auto ret = is_capvalue_supported_impl<typename Cap::value_type>(capValue, capToTest);
            if ( !ret.first )
                return false;
            if ( !ret.second )
            {
                auto vect = get_cap_values<std::vector<typename Cap::value_type>>(capToTest); 
                auto cType = get_cap_container_type(capToTest, get());
                if ( cType != twain_container_type::CONTAINER_RANGE)
                {
                     bool found = std::find(vect.begin(), vect.end(), 
                            static_cast<typename dtwain_underlying_type<typename Cap::value_type>::value_type>(capValue)) != vect.end();
                     if ( found )
                        return true;
                     return false;
                }
                else
                {
                    twain_range<typename Cap::value_type> tr(vect.begin(), vect.end());
                    bool found = tr.value_exists(static_cast<dtwain_underlying_type_v<typename Cap::value_type>>(capValue));
                    if ( found )
                        return true;
                    return false;
                }
            }
            return ret.first;
        }

        template <typename Cap, typename std::enable_if<
                                std::is_same<typename Cap::value_type, std::string>::value, bool>::type = 1>
        bool is_capvalue_supported(const std::string& capValue, CAP_SUPPORTEDCAPS_::value_type capToTest) const
        {
            auto ret = is_capvalue_supported_impl_s(capValue, capToTest);
            if ( !ret.first )
                return false;
            if ( !ret.second )
            {
                std::vector<std::string> vect;
                get_cap_values<std::vector<std::string>>(vect, capToTest); 
                return std::find(vect.begin(), vect.end(), capValue) != vect.end();
            }
            return ret.first;
        }

        cap_return_type get_last_error() const { return m_return_type; }

        static size_t get_variant_index(const twaintype_variant_type& vt) 
        {
            return variant_get_type_(vt);
        }

        template <typename Container = std::vector<ACAP_XFERMECH_::value_type>> Container get_audio_xfermech(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ACAP_XFERMECH_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_ALARMS_::value_type>> Container get_alarms(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_ALARMS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_ALARMVOLUME_::value_type>> Container get_alarmvolume(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_ALARMVOLUME_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_AUTHOR_::value_type>> Container get_author(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_AUTHOR_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_AUTOFEED_::value_type>> Container get_autofeed(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_AUTOFEED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_AUTOMATICCAPTURE_::value_type>> Container get_automaticcapture(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_AUTOMATICCAPTURE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_AUTOMATICSENSEMEDIUM_::value_type>> Container get_automaticsensemedium(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_AUTOMATICSENSEMEDIUM_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_AUTOSCAN_::value_type>> Container get_autoscan(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_AUTOSCAN_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_BATTERYMINUTES_::value_type>> Container get_batteryminutes(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_BATTERYMINUTES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_BATTERYPERCENTAGE_::value_type>> Container get_batterypercentage(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_BATTERYPERCENTAGE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CAMERAENABLED_::value_type>> Container get_cameraenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CAMERAENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CAMERAORDER_::value_type>> Container get_cameraorder(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CAMERAORDER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CAMERAPREVIEWUI_::value_type>> Container get_camerapreviewui(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CAMERAPREVIEWUI_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CAMERASIDE_::value_type>> Container get_cameraside(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CAMERASIDE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CAPTION_::value_type>> Container get_caption(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CAPTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CLEARPAGE_::value_type>> Container get_clearpage(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CLEARPAGE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CUSTOMDSDATA_::value_type>> Container get_customdsdata(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CUSTOMDSDATA_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_CUSTOMINTERFACEGUID_::value_type>> Container get_custominterfaceguid(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_CUSTOMINTERFACEGUID_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DEVICEEVENT_::value_type>> Container get_deviceevent(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DEVICEEVENT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DEVICEONLINE_::value_type>> Container get_deviceonline(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DEVICEONLINE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DEVICETIMEDATE_::value_type>> Container get_devicetimedate(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DEVICETIMEDATE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTION_::value_type>> Container get_doublefeeddetection(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DOUBLEFEEDDETECTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTIONLENGTH_::value_type>> Container get_doublefeeddetectionlength(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONLENGTH_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTIONRESPONSE_::value_type>> Container get_doublefeeddetectionresponse(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONRESPONSE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTIONSENSITIVITY_::value_type>> Container get_doublefeeddetectionsensitivity(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONSENSITIVITY_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DUPLEX_::value_type>> Container get_duplex(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DUPLEX_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_DUPLEXENABLED_::value_type>> Container get_duplexenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_DUPLEXENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_ENABLEDSUIONLY_::value_type>> Container get_enabledsuionly(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_ENABLEDSUIONLY_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_ENDORSER_::value_type>> Container get_endorser(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_ENDORSER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_EXTENDEDCAPS_::value_type>> Container get_extendedcaps(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_EXTENDEDCAPS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_FEEDERALIGNMENT_::value_type>> Container get_feederalignment(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_FEEDERALIGNMENT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_FEEDERENABLED_::value_type>> Container get_feederenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_FEEDERENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_FEEDERLOADED_::value_type>> Container get_feederloaded(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_FEEDERLOADED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_FEEDERORDER_::value_type>> Container get_feederorder(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_FEEDERORDER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_FEEDERPOCKET_::value_type>> Container get_feederpocket(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_FEEDERPOCKET_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_FEEDERPREP_::value_type>> Container get_feederprep(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_FEEDERPREP_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_FEEDPAGE_::value_type>> Container get_feedpage(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_FEEDPAGE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_INDICATORS_::value_type>> Container get_indicators(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_INDICATORS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_INDICATORSMODE_::value_type>> Container get_indicatorsmode(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_INDICATORSMODE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_JOBCONTROL_::value_type>> Container get_jobcontrol(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_JOBCONTROL_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_LANGUAGE_::value_type>> Container get_language(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_LANGUAGE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_MAXBATCHBUFFERS_::value_type>> Container get_maxbatchbuffers(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_MAXBATCHBUFFERS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_MICRENABLED_::value_type>> Container get_micrenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_MICRENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PAPERDETECTABLE_::value_type>> Container get_paperdetectable(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PAPERDETECTABLE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PAPERHANDLING_::value_type>> Container get_paperhandling(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PAPERHANDLING_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_POWERSAVETIME_::value_type>> Container get_powersavetime(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_POWERSAVETIME_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_POWERSUPPLY_::value_type>> Container get_powersupply(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_POWERSUPPLY_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTER_::value_type>> Container get_printer(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERCHARROTATION_::value_type>> Container get_printercharrotation(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERCHARROTATION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERENABLED_::value_type>> Container get_printerenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERFONTSTYLE_::value_type>> Container get_printerfontstyle(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERFONTSTYLE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERINDEX_::value_type>> Container get_printerindex(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERINDEX_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERINDEXLEADCHAR_::value_type>> Container get_printerindexleadchar(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERINDEXLEADCHAR_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERINDEXMAXVALUE_::value_type>> Container get_printerindexmaxvalue(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERINDEXMAXVALUE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERINDEXNUMDIGITS_::value_type>> Container get_printerindexnumdigits(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERINDEXNUMDIGITS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERINDEXSTEP_::value_type>> Container get_printerindexstep(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERINDEXSTEP_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERINDEXTRIGGER_::value_type>> Container get_printerindextrigger(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERINDEXTRIGGER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERMODE_::value_type>> Container get_printermode(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERMODE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERSTRING_::value_type>> Container get_printerstring(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERSTRING_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERSTRINGPREVIEW_::value_type>> Container get_printerstringpreview(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERSTRINGPREVIEW_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERSUFFIX_::value_type>> Container get_printersuffix(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERSUFFIX_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_PRINTERVERTICALOFFSET_::value_type>> Container get_printerverticaloffset(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_PRINTERVERTICALOFFSET_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_REACQUIREALLOWED_::value_type>> Container get_reacquireallowed(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_REACQUIREALLOWED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_REWINDPAGE_::value_type>> Container get_rewindpage(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_REWINDPAGE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_SEGMENTED_::value_type>> Container get_segmented(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_SEGMENTED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_SERIALNUMBER_::value_type>> Container get_serialnumber(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_SERIALNUMBER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_SHEETCOUNT_::value_type>> Container get_sheetcount(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_SHEETCOUNT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_SUPPORTEDCAPS_::value_type>> Container get_supportedcaps(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_SUPPORTEDCAPS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_SUPPORTEDCAPSSEGMENTUNIQUE_::value_type>> Container get_supportedcapssegmentunique(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_SUPPORTEDCAPSSEGMENTUNIQUE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_SUPPORTEDDATS_::value_type>> Container get_supporteddats(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_SUPPORTEDDATS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_THUMBNAILSENABLED_::value_type>> Container get_thumbnailsenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_THUMBNAILSENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_TIMEBEFOREFIRSTCAPTURE_::value_type>> Container get_timebeforefirstcapture(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_TIMEBEFOREFIRSTCAPTURE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_TIMEBETWEENCAPTURES_::value_type>> Container get_timebetweencaptures(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_TIMEBETWEENCAPTURES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_TIMEDATE_::value_type>> Container get_timedate(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_TIMEDATE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_UICONTROLLABLE_::value_type>> Container get_uicontrollable(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_UICONTROLLABLE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<CAP_XFERCOUNT_::value_type>> Container get_xfercount(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, CAP_XFERCOUNT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOBRIGHT_::value_type>> Container get_autobright(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOBRIGHT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTODISCARDBLANKPAGES_::value_type>> Container get_autodiscardblankpages(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTODISCARDBLANKPAGES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOMATICBORDERDETECTION_::value_type>> Container get_automaticborderdetection(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOMATICBORDERDETECTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOMATICCOLORENABLED_::value_type>> Container get_automaticcolorenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOMATICCOLORENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_::value_type>> Container get_automaticcolornoncolorpixeltype(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOMATICCROPUSESFRAME_::value_type>> Container get_automaticcropusesframe(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOMATICCROPUSESFRAME_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOMATICDESKEW_::value_type>> Container get_automaticdeskew(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOMATICDESKEW_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOMATICLENGTHDETECTION_::value_type>> Container get_automaticlengthdetection(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOMATICLENGTHDETECTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOMATICROTATE_::value_type>> Container get_automaticrotate(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOMATICROTATE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_AUTOSIZE_::value_type>> Container get_autosize(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_AUTOSIZE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BARCODEDETECTIONENABLED_::value_type>> Container get_barcodedetectionenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BARCODEDETECTIONENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BARCODEMAXRETRIES_::value_type>> Container get_barcodemaxretries(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BARCODEMAXRETRIES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BARCODEMAXSEARCHPRIORITIES_::value_type>> Container get_barcodemaxsearchpriorities(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BARCODEMAXSEARCHPRIORITIES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BARCODESEARCHMODE_::value_type>> Container get_barcodesearchmode(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BARCODESEARCHMODE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BARCODESEARCHPRIORITIES_::value_type>> Container get_barcodesearchpriorities(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BARCODESEARCHPRIORITIES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BARCODETIMEOUT_::value_type>> Container get_barcodetimeout(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BARCODETIMEOUT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BITDEPTH_::value_type>> Container get_bitdepth(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BITDEPTH_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BITDEPTHREDUCTION_::value_type>> Container get_bitdepthreduction(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BITDEPTHREDUCTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BITORDER_::value_type>> Container get_bitorder(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BITORDER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BITORDERCODES_::value_type>> Container get_bitordercodes(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BITORDERCODES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_BRIGHTNESS_::value_type>> Container get_brightness(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_BRIGHTNESS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_CCITTKFACTOR_::value_type>> Container get_ccittkfactor(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_CCITTKFACTOR_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_COLORMANAGEMENTENABLED_::value_type>> Container get_colormanagementenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_COLORMANAGEMENTENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_COMPRESSION_::value_type>> Container get_compression(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_COMPRESSION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_CONTRAST_::value_type>> Container get_contrast(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_CONTRAST_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_CUSTHALFTONE_::value_type>> Container get_custhalftone(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_CUSTHALFTONE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_EXPOSURETIME_::value_type>> Container get_exposuretime(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_EXPOSURETIME_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_EXTIMAGEINFO_::value_type>> Container get_extimageinfo(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_EXTIMAGEINFO_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_FEEDERTYPE_::value_type>> Container get_feedertype(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_FEEDERTYPE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_FILMTYPE_::value_type>> Container get_filmtype(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_FILMTYPE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_FILTER_::value_type>> Container get_filter(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_FILTER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_FLASHUSED_::value_type>> Container get_flashused(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_FLASHUSED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_FLASHUSED2_::value_type>> Container get_flashused2(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_FLASHUSED2_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_FLIPROTATION_::value_type>> Container get_fliprotation(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_FLIPROTATION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_FRAMES_::value_type>> Container get_frames(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_FRAMES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_GAMMA_::value_type>> Container get_gamma(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_GAMMA_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_HALFTONES_::value_type>> Container get_halftones(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_HALFTONES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_HIGHLIGHT_::value_type>> Container get_highlight(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_HIGHLIGHT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_ICCPROFILE_::value_type>> Container get_iccprofile(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_ICCPROFILE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_IMAGEDATASET_::value_type>> Container get_imagedataset(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_IMAGEDATASET_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_IMAGEFILEFORMAT_::value_type>> Container get_imagefileformat(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_IMAGEFILEFORMAT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_IMAGEFILTER_::value_type>> Container get_imagefilter(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_IMAGEFILTER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_IMAGEMERGE_::value_type>> Container get_imagemerge(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_IMAGEMERGE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_IMAGEMERGEHEIGHTTHRESHOLD_::value_type>> Container get_imagemergeheightthreshold(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_IMAGEMERGEHEIGHTTHRESHOLD_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_JPEGPIXELTYPE_::value_type>> Container get_jpegpixeltype(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_JPEGPIXELTYPE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_JPEGQUALITY_::value_type>> Container get_jpegquality(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_JPEGQUALITY_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_JPEGSUBSAMPLING_::value_type>> Container get_jpegsubsampling(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_JPEGSUBSAMPLING_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_LAMPSTATE_::value_type>> Container get_lampstate(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_LAMPSTATE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_LIGHTPATH_::value_type>> Container get_lightpath(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_LIGHTPATH_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_LIGHTSOURCE_::value_type>> Container get_lightsource(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_LIGHTSOURCE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_MAXFRAMES_::value_type>> Container get_maxframes(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_MAXFRAMES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_MINIMUMHEIGHT_::value_type>> Container get_minimumheight(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_MINIMUMHEIGHT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_MINIMUMWIDTH_::value_type>> Container get_minimumwidth(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_MINIMUMWIDTH_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_MIRROR_::value_type>> Container get_mirror(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_MIRROR_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_NOISEFILTER_::value_type>> Container get_noisefilter(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_NOISEFILTER_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_ORIENTATION_::value_type>> Container get_orientation(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_ORIENTATION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_OVERSCAN_::value_type>> Container get_overscan(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_OVERSCAN_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PATCHCODEDETECTIONENABLED_::value_type>> Container get_patchcodedetectionenabled(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PATCHCODEDETECTIONENABLED_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PATCHCODEMAXRETRIES_::value_type>> Container get_patchcodemaxretries(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PATCHCODEMAXRETRIES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PATCHCODEMAXSEARCHPRIORITIES_::value_type>> Container get_patchcodemaxsearchpriorities(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PATCHCODEMAXSEARCHPRIORITIES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PATCHCODESEARCHMODE_::value_type>> Container get_patchcodesearchmode(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PATCHCODESEARCHMODE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PATCHCODESEARCHPRIORITIES_::value_type>> Container get_patchcodesearchpriorities(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PATCHCODESEARCHPRIORITIES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PATCHCODETIMEOUT_::value_type>> Container get_patchcodetimeout(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PATCHCODETIMEOUT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PHYSICALHEIGHT_::value_type>> Container get_physicalheight(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PHYSICALHEIGHT_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PHYSICALWIDTH_::value_type>> Container get_physicalwidth(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PHYSICALWIDTH_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PIXELFLAVOR_::value_type>> Container get_pixelflavor(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PIXELFLAVOR_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PIXELFLAVORCODES_::value_type>> Container get_pixelflavorcodes(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PIXELFLAVORCODES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PIXELTYPE_::value_type>> Container get_pixeltype(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PIXELTYPE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_PLANARCHUNKY_::value_type>> Container get_planarchunky(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_PLANARCHUNKY_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_ROTATION_::value_type>> Container get_rotation(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_ROTATION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_SHADOW_::value_type>> Container get_shadow(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_SHADOW_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_SUPPORTEDBARCODETYPES_::value_type>> Container get_supportedbarcodetypes(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_SUPPORTEDBARCODETYPES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_SUPPORTEDEXTIMAGEINFO_::value_type>> Container get_supportedextimageinfo(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_SUPPORTEDEXTIMAGEINFO_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_SUPPORTEDPATCHCODETYPES_::value_type>> Container get_supportedpatchcodetypes(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_SUPPORTEDPATCHCODETYPES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_SUPPORTEDSIZES_::value_type>> Container get_supportedsizes(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_SUPPORTEDSIZES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_THRESHOLD_::value_type>> Container get_threshold(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_THRESHOLD_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_TILES_::value_type>> Container get_tiles(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_TILES_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_TIMEFILL_::value_type>> Container get_timefill(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_TIMEFILL_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_UNDEFINEDIMAGESIZE_::value_type>> Container get_undefinedimagesize(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_UNDEFINEDIMAGESIZE_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_UNITS_::value_type>> Container get_units(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_UNITS_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_XFERMECH_::value_type>> Container get_image_xfermech(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_XFERMECH_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_XNATIVERESOLUTION_::value_type>> Container get_xnativeresolution(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_XNATIVERESOLUTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_XRESOLUTION_::value_type>> Container get_xresolution(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_XRESOLUTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_XSCALING_::value_type>> Container get_xscaling(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_XSCALING_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_YNATIVERESOLUTION_::value_type>> Container get_ynativeresolution(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_YNATIVERESOLUTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_YRESOLUTION_::value_type>> Container get_yresolution(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_YRESOLUTION_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_YSCALING_::value_type>> Container get_yscaling(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_YSCALING_>(ct, gcType); return ct; }
        template <typename Container = std::vector<ICAP_ZOOMFACTOR_::value_type>> Container get_zoomfactor(const getcap_operation_info& gcType = getcap_operation_info()) const { Container ct {}; m_return_type = get_caps_impl<Container, ICAP_ZOOMFACTOR_>(ct, gcType); return ct; }

        template <typename Container = std::vector<ACAP_XFERMECH_::value_type>> const capability_interface& set_audio_xfermech(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ACAP_XFERMECH_>(ct, scType); else m_return_type = set_caps_impl<Container, ACAP_XFERMECH_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_ALARMS_::value_type>> const capability_interface& set_alarms(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_ALARMS_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_ALARMS_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_ALARMVOLUME_::value_type>> const capability_interface& set_alarmvolume(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_ALARMVOLUME_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_ALARMVOLUME_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_AUTHOR_::value_type>> const capability_interface& set_author(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_AUTHOR_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_AUTHOR_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_AUTOFEED_::value_type>> const capability_interface& set_autofeed(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_AUTOFEED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_AUTOFEED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_AUTOMATICCAPTURE_::value_type>> const capability_interface& set_automaticcapture(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_AUTOMATICCAPTURE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_AUTOMATICCAPTURE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_AUTOMATICSENSEMEDIUM_::value_type>> const capability_interface& set_automaticsensemedium(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_AUTOMATICSENSEMEDIUM_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_AUTOMATICSENSEMEDIUM_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_AUTOSCAN_::value_type>> const capability_interface& set_autoscan(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_AUTOSCAN_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_AUTOSCAN_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_CAMERAENABLED_::value_type>> const capability_interface& set_cameraenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_CAMERAENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_CAMERAENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_CAMERAORDER_::value_type>> const capability_interface& set_cameraorder(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_CAMERAORDER_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_CAMERAORDER_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_CAMERASIDE_::value_type>> const capability_interface& set_cameraside(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_CAMERASIDE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_CAMERASIDE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_CAPTION_::value_type>> const capability_interface& set_caption(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_CAPTION_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_CAPTION_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_CLEARPAGE_::value_type>> const capability_interface& set_clearpage(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_CLEARPAGE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_CLEARPAGE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_DEVICEEVENT_::value_type>> const capability_interface& set_deviceevent(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_DEVICEEVENT_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_DEVICEEVENT_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_DEVICETIMEDATE_::value_type>> const capability_interface& set_devicetimedate(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_DEVICETIMEDATE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_DEVICETIMEDATE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTION_::value_type>> const capability_interface& set_doublefeeddetection(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTION_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTION_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTIONLENGTH_::value_type>> const capability_interface& set_doublefeeddetectionlength(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONLENGTH_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONLENGTH_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTIONRESPONSE_::value_type>> const capability_interface& set_doublefeeddetectionresponse(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONRESPONSE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONRESPONSE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_DOUBLEFEEDDETECTIONSENSITIVITY_::value_type>> const capability_interface& set_doublefeeddetectionsensitivity(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONSENSITIVITY_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_DOUBLEFEEDDETECTIONSENSITIVITY_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_DUPLEXENABLED_::value_type>> const capability_interface& set_duplexenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_DUPLEXENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_DUPLEXENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_ENDORSER_::value_type>> const capability_interface& set_endorser(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_ENDORSER_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_ENDORSER_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_EXTENDEDCAPS_::value_type>> const capability_interface& set_extendedcaps(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_EXTENDEDCAPS_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_EXTENDEDCAPS_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_FEEDERALIGNMENT_::value_type>> const capability_interface& set_feederalignment(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_FEEDERALIGNMENT_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_FEEDERALIGNMENT_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_FEEDERENABLED_::value_type>> const capability_interface& set_feederenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_FEEDERENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_FEEDERENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_FEEDERORDER_::value_type>> const capability_interface& set_feederorder(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_FEEDERORDER_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_FEEDERORDER_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_FEEDERPOCKET_::value_type>> const capability_interface& set_feederpocket(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_FEEDERPOCKET_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_FEEDERPOCKET_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_FEEDERPREP_::value_type>> const capability_interface& set_feederprep(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_FEEDERPREP_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_FEEDERPREP_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_FEEDPAGE_::value_type>> const capability_interface& set_feedpage(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_FEEDPAGE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_FEEDPAGE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_INDICATORS_::value_type>> const capability_interface& set_indicators(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_INDICATORS_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_INDICATORS_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_INDICATORSMODE_::value_type>> const capability_interface& set_indicatorsmode(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_INDICATORSMODE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_INDICATORSMODE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_JOBCONTROL_::value_type>> const capability_interface& set_jobcontrol(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_JOBCONTROL_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_JOBCONTROL_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_LANGUAGE_::value_type>> const capability_interface& set_language(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_LANGUAGE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_LANGUAGE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_MAXBATCHBUFFERS_::value_type>> const capability_interface& set_maxbatchbuffers(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_MAXBATCHBUFFERS_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_MAXBATCHBUFFERS_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_MICRENABLED_::value_type>> const capability_interface& set_micrenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_MICRENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_MICRENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PAPERHANDLING_::value_type>> const capability_interface& set_paperhandling(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PAPERHANDLING_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PAPERHANDLING_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_POWERSAVETIME_::value_type>> const capability_interface& set_powersavetime(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_POWERSAVETIME_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_POWERSAVETIME_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTER_::value_type>> const capability_interface& set_printer(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTER_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTER_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERCHARROTATION_::value_type>> const capability_interface& set_printercharrotation(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERCHARROTATION_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERCHARROTATION_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERENABLED_::value_type>> const capability_interface& set_printerenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERFONTSTYLE_::value_type>> const capability_interface& set_printerfontstyle(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERFONTSTYLE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERFONTSTYLE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERINDEX_::value_type>> const capability_interface& set_printerindex(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERINDEX_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERINDEX_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERINDEXLEADCHAR_::value_type>> const capability_interface& set_printerindexleadchar(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXLEADCHAR_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXLEADCHAR_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERINDEXMAXVALUE_::value_type>> const capability_interface& set_printerindexmaxvalue(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXMAXVALUE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXMAXVALUE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERINDEXNUMDIGITS_::value_type>> const capability_interface& set_printerindexnumdigits(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXNUMDIGITS_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXNUMDIGITS_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERINDEXSTEP_::value_type>> const capability_interface& set_printerindexstep(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXSTEP_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXSTEP_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERINDEXTRIGGER_::value_type>> const capability_interface& set_printerindextrigger(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXTRIGGER_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERINDEXTRIGGER_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERMODE_::value_type>> const capability_interface& set_printermode(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERMODE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERMODE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERSTRING_::value_type>> const capability_interface& set_printerstring(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERSTRING_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERSTRING_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERSUFFIX_::value_type>> const capability_interface& set_printersuffix(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERSUFFIX_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERSUFFIX_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_PRINTERVERTICALOFFSET_::value_type>> const capability_interface& set_printerverticaloffset(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_PRINTERVERTICALOFFSET_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_PRINTERVERTICALOFFSET_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_REWINDPAGE_::value_type>> const capability_interface& set_rewindpage(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_REWINDPAGE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_REWINDPAGE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_SEGMENTED_::value_type>> const capability_interface& set_segmented(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_SEGMENTED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_SEGMENTED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_SHEETCOUNT_::value_type>> const capability_interface& set_sheetcount(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_SHEETCOUNT_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_SHEETCOUNT_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_SUPPORTEDCAPSSEGMENTUNIQUE_::value_type>> const capability_interface& set_supportedcapssegmentunique(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_SUPPORTEDCAPSSEGMENTUNIQUE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_SUPPORTEDCAPSSEGMENTUNIQUE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_THUMBNAILSENABLED_::value_type>> const capability_interface& set_thumbnailsenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_THUMBNAILSENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_THUMBNAILSENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_TIMEBEFOREFIRSTCAPTURE_::value_type>> const capability_interface& set_timebeforefirstcapture(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_TIMEBEFOREFIRSTCAPTURE_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_TIMEBEFOREFIRSTCAPTURE_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_TIMEBETWEENCAPTURES_::value_type>> const capability_interface& set_timebetweencaptures(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_TIMEBETWEENCAPTURES_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_TIMEBETWEENCAPTURES_>({}, reset()); return *this; }
        template <typename Container = std::vector<CAP_XFERCOUNT_::value_type>> const capability_interface& set_xfercount(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, CAP_XFERCOUNT_>(ct, scType); else m_return_type = set_caps_impl<Container, CAP_XFERCOUNT_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOBRIGHT_::value_type>> const capability_interface& set_autobright(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOBRIGHT_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOBRIGHT_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTODISCARDBLANKPAGES_::value_type>> const capability_interface& set_autodiscardblankpages(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTODISCARDBLANKPAGES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTODISCARDBLANKPAGES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOMATICBORDERDETECTION_::value_type>> const capability_interface& set_automaticborderdetection(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOMATICBORDERDETECTION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOMATICBORDERDETECTION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOMATICCOLORENABLED_::value_type>> const capability_interface& set_automaticcolorenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOMATICCOLORENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOMATICCOLORENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_::value_type>> const capability_interface& set_automaticcolornoncolorpixeltype(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOMATICDESKEW_::value_type>> const capability_interface& set_automaticdeskew(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOMATICDESKEW_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOMATICDESKEW_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOMATICLENGTHDETECTION_::value_type>> const capability_interface& set_automaticlengthdetection(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOMATICLENGTHDETECTION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOMATICLENGTHDETECTION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOMATICROTATE_::value_type>> const capability_interface& set_automaticrotate(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOMATICROTATE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOMATICROTATE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_AUTOSIZE_::value_type>> const capability_interface& set_autosize(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_AUTOSIZE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_AUTOSIZE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BARCODEDETECTIONENABLED_::value_type>> const capability_interface& set_barcodedetectionenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BARCODEDETECTIONENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BARCODEDETECTIONENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BARCODEMAXRETRIES_::value_type>> const capability_interface& set_barcodemaxretries(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BARCODEMAXRETRIES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BARCODEMAXRETRIES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BARCODEMAXSEARCHPRIORITIES_::value_type>> const capability_interface& set_barcodemaxsearchpriorities(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BARCODEMAXSEARCHPRIORITIES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BARCODEMAXSEARCHPRIORITIES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BARCODESEARCHMODE_::value_type>> const capability_interface& set_barcodesearchmode(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BARCODESEARCHMODE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BARCODESEARCHMODE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BARCODESEARCHPRIORITIES_::value_type>> const capability_interface& set_barcodesearchpriorities(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BARCODESEARCHPRIORITIES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BARCODESEARCHPRIORITIES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BARCODETIMEOUT_::value_type>> const capability_interface& set_barcodetimeout(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BARCODETIMEOUT_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BARCODETIMEOUT_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BITDEPTH_::value_type>> const capability_interface& set_bitdepth(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BITDEPTH_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BITDEPTH_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BITDEPTHREDUCTION_::value_type>> const capability_interface& set_bitdepthreduction(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BITDEPTHREDUCTION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BITDEPTHREDUCTION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BITORDER_::value_type>> const capability_interface& set_bitorder(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BITORDER_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BITORDER_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BITORDERCODES_::value_type>> const capability_interface& set_bitordercodes(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BITORDERCODES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BITORDERCODES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_BRIGHTNESS_::value_type>> const capability_interface& set_brightness(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_BRIGHTNESS_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_BRIGHTNESS_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_CCITTKFACTOR_::value_type>> const capability_interface& set_ccittkfactor(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_CCITTKFACTOR_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_CCITTKFACTOR_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_COLORMANAGEMENTENABLED_::value_type>> const capability_interface& set_colormanagementenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_COLORMANAGEMENTENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_COLORMANAGEMENTENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_COMPRESSION_::value_type>> const capability_interface& set_compression(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_COMPRESSION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_COMPRESSION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_CONTRAST_::value_type>> const capability_interface& set_contrast(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_CONTRAST_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_CONTRAST_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_CUSTHALFTONE_::value_type>> const capability_interface& set_custhalftone(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_CUSTHALFTONE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_CUSTHALFTONE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_EXPOSURETIME_::value_type>> const capability_interface& set_exposuretime(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_EXPOSURETIME_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_EXPOSURETIME_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_EXTIMAGEINFO_::value_type>> const capability_interface& set_extimageinfo(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_EXTIMAGEINFO_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_EXTIMAGEINFO_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_FEEDERTYPE_::value_type>> const capability_interface& set_feedertype(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_FEEDERTYPE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_FEEDERTYPE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_FILMTYPE_::value_type>> const capability_interface& set_filmtype(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_FILMTYPE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_FILMTYPE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_FILTER_::value_type>> const capability_interface& set_filter(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_FILTER_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_FILTER_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_FLASHUSED_::value_type>> const capability_interface& set_flashused(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_FLASHUSED_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_FLASHUSED_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_FLASHUSED2_::value_type>> const capability_interface& set_flashused2(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_FLASHUSED2_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_FLASHUSED2_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_FLIPROTATION_::value_type>> const capability_interface& set_fliprotation(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_FLIPROTATION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_FLIPROTATION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_FRAMES_::value_type>> const capability_interface& set_frames(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_FRAMES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_FRAMES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_GAMMA_::value_type>> const capability_interface& set_gamma(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_GAMMA_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_GAMMA_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_HALFTONES_::value_type>> const capability_interface& set_halftones(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_HALFTONES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_HALFTONES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_HIGHLIGHT_::value_type>> const capability_interface& set_highlight(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_HIGHLIGHT_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_HIGHLIGHT_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_ICCPROFILE_::value_type>> const capability_interface& set_iccprofile(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_ICCPROFILE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_ICCPROFILE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_IMAGEDATASET_::value_type>> const capability_interface& set_imagedataset(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_IMAGEDATASET_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_IMAGEDATASET_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_IMAGEFILEFORMAT_::value_type>> const capability_interface& set_imagefileformat(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_IMAGEFILEFORMAT_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_IMAGEFILEFORMAT_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_IMAGEFILTER_::value_type>> const capability_interface& set_imagefilter(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_IMAGEFILTER_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_IMAGEFILTER_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_IMAGEMERGE_::value_type>> const capability_interface& set_imagemerge(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_IMAGEMERGE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_IMAGEMERGE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_IMAGEMERGEHEIGHTTHRESHOLD_::value_type>> const capability_interface& set_imagemergeheightthreshold(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_IMAGEMERGEHEIGHTTHRESHOLD_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_IMAGEMERGEHEIGHTTHRESHOLD_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_JPEGPIXELTYPE_::value_type>> const capability_interface& set_jpegpixeltype(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_JPEGPIXELTYPE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_JPEGPIXELTYPE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_JPEGQUALITY_::value_type>> const capability_interface& set_jpegquality(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_JPEGQUALITY_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_JPEGQUALITY_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_JPEGSUBSAMPLING_::value_type>> const capability_interface& set_jpegsubsampling(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_JPEGSUBSAMPLING_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_JPEGSUBSAMPLING_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_LAMPSTATE_::value_type>> const capability_interface& set_lampstate(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_LAMPSTATE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_LAMPSTATE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_LIGHTPATH_::value_type>> const capability_interface& set_lightpath(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_LIGHTPATH_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_LIGHTPATH_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_LIGHTSOURCE_::value_type>> const capability_interface& set_lightsource(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_LIGHTSOURCE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_LIGHTSOURCE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_MAXFRAMES_::value_type>> const capability_interface& set_maxframes(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_MAXFRAMES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_MAXFRAMES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_MINIMUMHEIGHT_::value_type>> const capability_interface& set_minimumheight(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_MINIMUMHEIGHT_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_MINIMUMHEIGHT_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_MINIMUMWIDTH_::value_type>> const capability_interface& set_minimumwidth(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_MINIMUMWIDTH_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_MINIMUMWIDTH_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_MIRROR_::value_type>> const capability_interface& set_mirror(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_MIRROR_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_MIRROR_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_NOISEFILTER_::value_type>> const capability_interface& set_noisefilter(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_NOISEFILTER_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_NOISEFILTER_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_ORIENTATION_::value_type>> const capability_interface& set_orientation(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_ORIENTATION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_ORIENTATION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_OVERSCAN_::value_type>> const capability_interface& set_overscan(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_OVERSCAN_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_OVERSCAN_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PATCHCODEDETECTIONENABLED_::value_type>> const capability_interface& set_patchcodedetectionenabled(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PATCHCODEDETECTIONENABLED_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PATCHCODEDETECTIONENABLED_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PATCHCODEMAXRETRIES_::value_type>> const capability_interface& set_patchcodemaxretries(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PATCHCODEMAXRETRIES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PATCHCODEMAXRETRIES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PATCHCODEMAXSEARCHPRIORITIES_::value_type>> const capability_interface& set_patchcodemaxsearchpriorities(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PATCHCODEMAXSEARCHPRIORITIES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PATCHCODEMAXSEARCHPRIORITIES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PATCHCODESEARCHMODE_::value_type>> const capability_interface& set_patchcodesearchmode(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PATCHCODESEARCHMODE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PATCHCODESEARCHMODE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PATCHCODESEARCHPRIORITIES_::value_type>> const capability_interface& set_patchcodesearchpriorities(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PATCHCODESEARCHPRIORITIES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PATCHCODESEARCHPRIORITIES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PATCHCODETIMEOUT_::value_type>> const capability_interface& set_patchcodetimeout(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PATCHCODETIMEOUT_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PATCHCODETIMEOUT_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PIXELFLAVOR_::value_type>> const capability_interface& set_pixelflavor(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PIXELFLAVOR_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PIXELFLAVOR_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PIXELFLAVORCODES_::value_type>> const capability_interface& set_pixelflavorcodes(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PIXELFLAVORCODES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PIXELFLAVORCODES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PIXELTYPE_::value_type>> const capability_interface& set_pixeltype(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PIXELTYPE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PIXELTYPE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_PLANARCHUNKY_::value_type>> const capability_interface& set_planarchunky(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_PLANARCHUNKY_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_PLANARCHUNKY_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_ROTATION_::value_type>> const capability_interface& set_rotation(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_ROTATION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_ROTATION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_SHADOW_::value_type>> const capability_interface& set_shadow(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_SHADOW_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_SHADOW_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_SUPPORTEDSIZES_::value_type>> const capability_interface& set_supportedsizes(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_SUPPORTEDSIZES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_SUPPORTEDSIZES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_THRESHOLD_::value_type>> const capability_interface& set_threshold(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_THRESHOLD_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_THRESHOLD_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_TILES_::value_type>> const capability_interface& set_tiles(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_TILES_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_TILES_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_TIMEFILL_::value_type>> const capability_interface& set_timefill(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_TIMEFILL_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_TIMEFILL_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_UNDEFINEDIMAGESIZE_::value_type>> const capability_interface& set_undefinedimagesize(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_UNDEFINEDIMAGESIZE_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_UNDEFINEDIMAGESIZE_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_UNITS_::value_type>> const capability_interface& set_units(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_UNITS_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_UNITS_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_XFERMECH_::value_type>> const capability_interface& set_image_xfermech(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_XFERMECH_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_XFERMECH_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_XRESOLUTION_::value_type>> const capability_interface& set_xresolution(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_XRESOLUTION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_XRESOLUTION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_XSCALING_::value_type>> const capability_interface& set_xscaling(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_XSCALING_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_XSCALING_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_YRESOLUTION_::value_type>> const capability_interface& set_yresolution(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_YRESOLUTION_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_YRESOLUTION_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_YSCALING_::value_type>> const capability_interface& set_yscaling(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_YSCALING_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_YSCALING_>({}, reset()); return *this; }
        template <typename Container = std::vector<ICAP_ZOOMFACTOR_::value_type>> const capability_interface& set_zoomfactor(const Container& ct, const setcap_operation_info& scType = setcap_operation_info()) const { if (!ct.empty()) m_return_type = set_caps_impl<Container, ICAP_ZOOMFACTOR_>(ct, scType); else m_return_type = set_caps_impl<Container, ICAP_ZOOMFACTOR_>({}, reset()); return *this; }

        template <typename Container = ACAP_XFERMECH_::value_type> bool is_audio_xfermech_value_supported(const Container& c) const { return is_capvalue_supported<ACAP_XFERMECH_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1202); }
        template <typename Container = CAP_ALARMS_::value_type> bool is_alarms_value_supported(const Container& c) const { return is_capvalue_supported<CAP_ALARMS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1018); }
        template <typename Container = CAP_ALARMVOLUME_::value_type> bool is_alarmvolume_value_supported(const Container& c) const { return is_capvalue_supported<CAP_ALARMVOLUME_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1019); }
        template <typename Container = CAP_AUTHOR_::value_type> bool is_author_value_supported(const Container& c) const { return is_capvalue_supported<CAP_AUTHOR_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1000); }
        template <typename Container = CAP_AUTOFEED_::value_type> bool is_autofeed_value_supported(const Container& c) const { return is_capvalue_supported<CAP_AUTOFEED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1007); }
        template <typename Container = CAP_AUTOMATICCAPTURE_::value_type> bool is_automaticcapture_value_supported(const Container& c) const { return is_capvalue_supported<CAP_AUTOMATICCAPTURE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x101a); }
        template <typename Container = CAP_AUTOMATICSENSEMEDIUM_::value_type> bool is_automaticsensemedium_value_supported(const Container& c) const { return is_capvalue_supported<CAP_AUTOMATICSENSEMEDIUM_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x103b); }
        template <typename Container = CAP_AUTOSCAN_::value_type> bool is_autoscan_value_supported(const Container& c) const { return is_capvalue_supported<CAP_AUTOSCAN_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1010); }
        template <typename Container = CAP_BATTERYMINUTES_::value_type> bool is_batteryminutes_value_supported(const Container& c) const { return is_capvalue_supported<CAP_BATTERYMINUTES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1032); }
        template <typename Container = CAP_BATTERYPERCENTAGE_::value_type> bool is_batterypercentage_value_supported(const Container& c) const { return is_capvalue_supported<CAP_BATTERYPERCENTAGE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1033); }
        template <typename Container = CAP_CAMERAENABLED_::value_type> bool is_cameraenabled_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CAMERAENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1036); }
        template <typename Container = CAP_CAMERAORDER_::value_type> bool is_cameraorder_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CAMERAORDER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1037); }
        template <typename Container = CAP_CAMERAPREVIEWUI_::value_type> bool is_camerapreviewui_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CAMERAPREVIEWUI_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1021); }
        template <typename Container = CAP_CAMERASIDE_::value_type> bool is_cameraside_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CAMERASIDE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1034); }
        template <typename Container = CAP_CAPTION_::value_type> bool is_caption_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CAPTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1001); }
        template <typename Container = CAP_CLEARPAGE_::value_type> bool is_clearpage_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CLEARPAGE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1008); }
        template <typename Container = CAP_CUSTOMDSDATA_::value_type> bool is_customdsdata_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CUSTOMDSDATA_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1015); }
        template <typename Container = CAP_CUSTOMINTERFACEGUID_::value_type> bool is_custominterfaceguid_value_supported(const Container& c) const { return is_capvalue_supported<CAP_CUSTOMINTERFACEGUID_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x103c); }
        template <typename Container = CAP_DEVICEEVENT_::value_type> bool is_deviceevent_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DEVICEEVENT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1022); }
        template <typename Container = CAP_DEVICEONLINE_::value_type> bool is_deviceonline_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DEVICEONLINE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x100f); }
        template <typename Container = CAP_DEVICETIMEDATE_::value_type> bool is_devicetimedate_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DEVICETIMEDATE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x101f); }
        template <typename Container = CAP_DOUBLEFEEDDETECTION_::value_type> bool is_doublefeeddetection_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DOUBLEFEEDDETECTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x103f); }
        template <typename Container = CAP_DOUBLEFEEDDETECTIONLENGTH_::value_type> bool is_doublefeeddetectionlength_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DOUBLEFEEDDETECTIONLENGTH_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1040); }
        template <typename Container = CAP_DOUBLEFEEDDETECTIONRESPONSE_::value_type> bool is_doublefeeddetectionresponse_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DOUBLEFEEDDETECTIONRESPONSE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1042); }
        template <typename Container = CAP_DOUBLEFEEDDETECTIONSENSITIVITY_::value_type> bool is_doublefeeddetectionsensitivity_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DOUBLEFEEDDETECTIONSENSITIVITY_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1041); }
        template <typename Container = CAP_DUPLEX_::value_type> bool is_duplex_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DUPLEX_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1012); }
        template <typename Container = CAP_DUPLEXENABLED_::value_type> bool is_duplexenabled_value_supported(const Container& c) const { return is_capvalue_supported<CAP_DUPLEXENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1013); }
        template <typename Container = CAP_ENABLEDSUIONLY_::value_type> bool is_enabledsuionly_value_supported(const Container& c) const { return is_capvalue_supported<CAP_ENABLEDSUIONLY_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1014); }
        template <typename Container = CAP_ENDORSER_::value_type> bool is_endorser_value_supported(const Container& c) const { return is_capvalue_supported<CAP_ENDORSER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1016); }
        template <typename Container = CAP_EXTENDEDCAPS_::value_type> bool is_extendedcaps_value_supported(const Container& c) const { return is_capvalue_supported<CAP_EXTENDEDCAPS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1006); }
        template <typename Container = CAP_FEEDERALIGNMENT_::value_type> bool is_feederalignment_value_supported(const Container& c) const { return is_capvalue_supported<CAP_FEEDERALIGNMENT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x102d); }
        template <typename Container = CAP_FEEDERENABLED_::value_type> bool is_feederenabled_value_supported(const Container& c) const { return is_capvalue_supported<CAP_FEEDERENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1002); }
        template <typename Container = CAP_FEEDERLOADED_::value_type> bool is_feederloaded_value_supported(const Container& c) const { return is_capvalue_supported<CAP_FEEDERLOADED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1003); }
        template <typename Container = CAP_FEEDERORDER_::value_type> bool is_feederorder_value_supported(const Container& c) const { return is_capvalue_supported<CAP_FEEDERORDER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x102e); }
        template <typename Container = CAP_FEEDERPOCKET_::value_type> bool is_feederpocket_value_supported(const Container& c) const { return is_capvalue_supported<CAP_FEEDERPOCKET_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x103a); }
        template <typename Container = CAP_FEEDERPREP_::value_type> bool is_feederprep_value_supported(const Container& c) const { return is_capvalue_supported<CAP_FEEDERPREP_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1039); }
        template <typename Container = CAP_FEEDPAGE_::value_type> bool is_feedpage_value_supported(const Container& c) const { return is_capvalue_supported<CAP_FEEDPAGE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1009); }
        template <typename Container = CAP_INDICATORS_::value_type> bool is_indicators_value_supported(const Container& c) const { return is_capvalue_supported<CAP_INDICATORS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x100b); }
        template <typename Container = CAP_INDICATORSMODE_::value_type> bool is_indicatorsmode_value_supported(const Container& c) const { return is_capvalue_supported<CAP_INDICATORSMODE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1044); }
        template <typename Container = CAP_JOBCONTROL_::value_type> bool is_jobcontrol_value_supported(const Container& c) const { return is_capvalue_supported<CAP_JOBCONTROL_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1017); }
        template <typename Container = CAP_LANGUAGE_::value_type> bool is_language_value_supported(const Container& c) const { return is_capvalue_supported<CAP_LANGUAGE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x102c); }
        template <typename Container = CAP_MAXBATCHBUFFERS_::value_type> bool is_maxbatchbuffers_value_supported(const Container& c) const { return is_capvalue_supported<CAP_MAXBATCHBUFFERS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x101e); }
        template <typename Container = CAP_MICRENABLED_::value_type> bool is_micrenabled_value_supported(const Container& c) const { return is_capvalue_supported<CAP_MICRENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1038); }
        template <typename Container = CAP_PAPERDETECTABLE_::value_type> bool is_paperdetectable_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PAPERDETECTABLE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x100d); }
        template <typename Container = CAP_PAPERHANDLING_::value_type> bool is_paperhandling_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PAPERHANDLING_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1043); }
        template <typename Container = CAP_POWERSAVETIME_::value_type> bool is_powersavetime_value_supported(const Container& c) const { return is_capvalue_supported<CAP_POWERSAVETIME_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1046); }
        template <typename Container = CAP_POWERSUPPLY_::value_type> bool is_powersupply_value_supported(const Container& c) const { return is_capvalue_supported<CAP_POWERSUPPLY_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1020); }
        template <typename Container = CAP_PRINTER_::value_type> bool is_printer_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1026); }
        template <typename Container = CAP_PRINTERCHARROTATION_::value_type> bool is_printercharrotation_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERCHARROTATION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1047); }
        template <typename Container = CAP_PRINTERENABLED_::value_type> bool is_printerenabled_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1027); }
        template <typename Container = CAP_PRINTERFONTSTYLE_::value_type> bool is_printerfontstyle_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERFONTSTYLE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1048); }
        template <typename Container = CAP_PRINTERINDEX_::value_type> bool is_printerindex_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERINDEX_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1028); }
        template <typename Container = CAP_PRINTERINDEXLEADCHAR_::value_type> bool is_printerindexleadchar_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERINDEXLEADCHAR_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1049); }
        template <typename Container = CAP_PRINTERINDEXMAXVALUE_::value_type> bool is_printerindexmaxvalue_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERINDEXMAXVALUE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x104A); }
        template <typename Container = CAP_PRINTERINDEXNUMDIGITS_::value_type> bool is_printerindexnumdigits_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERINDEXNUMDIGITS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x104B); }
        template <typename Container = CAP_PRINTERINDEXSTEP_::value_type> bool is_printerindexstep_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERINDEXSTEP_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x104C); }
        template <typename Container = CAP_PRINTERINDEXTRIGGER_::value_type> bool is_printerindextrigger_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERINDEXTRIGGER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x104D); }
        template <typename Container = CAP_PRINTERMODE_::value_type> bool is_printermode_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERMODE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1029); }
        template <typename Container = CAP_PRINTERSTRING_::value_type> bool is_printerstring_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERSTRING_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x102a); }
        template <typename Container = CAP_PRINTERSTRINGPREVIEW_::value_type> bool is_printerstringpreview_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERSTRINGPREVIEW_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x104E); }
        template <typename Container = CAP_PRINTERSUFFIX_::value_type> bool is_printersuffix_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERSUFFIX_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x102b); }
        template <typename Container = CAP_PRINTERVERTICALOFFSET_::value_type> bool is_printerverticaloffset_value_supported(const Container& c) const { return is_capvalue_supported<CAP_PRINTERVERTICALOFFSET_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1045); }
        template <typename Container = CAP_REACQUIREALLOWED_::value_type> bool is_reacquireallowed_value_supported(const Container& c) const { return is_capvalue_supported<CAP_REACQUIREALLOWED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1030); }
        template <typename Container = CAP_REWINDPAGE_::value_type> bool is_rewindpage_value_supported(const Container& c) const { return is_capvalue_supported<CAP_REWINDPAGE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x100a); }
        template <typename Container = CAP_SEGMENTED_::value_type> bool is_segmented_value_supported(const Container& c) const { return is_capvalue_supported<CAP_SEGMENTED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1035); }
        template <typename Container = CAP_SERIALNUMBER_::value_type> bool is_serialnumber_value_supported(const Container& c) const { return is_capvalue_supported<CAP_SERIALNUMBER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1024); }
        template <typename Container = CAP_SHEETCOUNT_::value_type> bool is_sheetcount_value_supported(const Container& c) const { return is_capvalue_supported<CAP_SHEETCOUNT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x104F); }
        template <typename Container = CAP_SUPPORTEDCAPS_::value_type> bool is_supportedcaps_value_supported(const Container& c) const { return is_capvalue_supported<CAP_SUPPORTEDCAPS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1005); }
        template <typename Container = CAP_SUPPORTEDCAPSSEGMENTUNIQUE_::value_type> bool is_supportedcapssegmentunique_value_supported(const Container& c) const { return is_capvalue_supported<CAP_SUPPORTEDCAPSSEGMENTUNIQUE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x103d); }
        template <typename Container = CAP_SUPPORTEDDATS_::value_type> bool is_supporteddats_value_supported(const Container& c) const { return is_capvalue_supported<CAP_SUPPORTEDDATS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x103e); }
        template <typename Container = CAP_THUMBNAILSENABLED_::value_type> bool is_thumbnailsenabled_value_supported(const Container& c) const { return is_capvalue_supported<CAP_THUMBNAILSENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1011); }
        template <typename Container = CAP_TIMEBEFOREFIRSTCAPTURE_::value_type> bool is_timebeforefirstcapture_value_supported(const Container& c) const { return is_capvalue_supported<CAP_TIMEBEFOREFIRSTCAPTURE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x101b); }
        template <typename Container = CAP_TIMEBETWEENCAPTURES_::value_type> bool is_timebetweencaptures_value_supported(const Container& c) const { return is_capvalue_supported<CAP_TIMEBETWEENCAPTURES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x101c); }
        template <typename Container = CAP_TIMEDATE_::value_type> bool is_timedate_value_supported(const Container& c) const { return is_capvalue_supported<CAP_TIMEDATE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1004); }
        template <typename Container = CAP_UICONTROLLABLE_::value_type> bool is_uicontrollable_value_supported(const Container& c) const { return is_capvalue_supported<CAP_UICONTROLLABLE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x100e); }
        template <typename Container = CAP_XFERCOUNT_::value_type> bool is_xfercount_value_supported(const Container& c) const { return is_capvalue_supported<CAP_XFERCOUNT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x0001); }
        template <typename Container = ICAP_AUTOBRIGHT_::value_type> bool is_autobright_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOBRIGHT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1100); }
        template <typename Container = ICAP_AUTODISCARDBLANKPAGES_::value_type> bool is_autodiscardblankpages_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTODISCARDBLANKPAGES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1134); }
        template <typename Container = ICAP_AUTOMATICBORDERDETECTION_::value_type> bool is_automaticborderdetection_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOMATICBORDERDETECTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1150); }
        template <typename Container = ICAP_AUTOMATICCOLORENABLED_::value_type> bool is_automaticcolorenabled_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOMATICCOLORENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1159); }
        template <typename Container = ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_::value_type> bool is_automaticcolornoncolorpixeltype_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x115a); }
        template <typename Container = ICAP_AUTOMATICCROPUSESFRAME_::value_type> bool is_automaticcropusesframe_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOMATICCROPUSESFRAME_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1157); }
        template <typename Container = ICAP_AUTOMATICDESKEW_::value_type> bool is_automaticdeskew_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOMATICDESKEW_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1151); }
        template <typename Container = ICAP_AUTOMATICLENGTHDETECTION_::value_type> bool is_automaticlengthdetection_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOMATICLENGTHDETECTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1158); }
        template <typename Container = ICAP_AUTOMATICROTATE_::value_type> bool is_automaticrotate_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOMATICROTATE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1152); }
        template <typename Container = ICAP_AUTOSIZE_::value_type> bool is_autosize_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_AUTOSIZE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1156); }
        template <typename Container = ICAP_BARCODEDETECTIONENABLED_::value_type> bool is_barcodedetectionenabled_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BARCODEDETECTIONENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1137); }
        template <typename Container = ICAP_BARCODEMAXRETRIES_::value_type> bool is_barcodemaxretries_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BARCODEMAXRETRIES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x113c); }
        template <typename Container = ICAP_BARCODEMAXSEARCHPRIORITIES_::value_type> bool is_barcodemaxsearchpriorities_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BARCODEMAXSEARCHPRIORITIES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1139); }
        template <typename Container = ICAP_BARCODESEARCHMODE_::value_type> bool is_barcodesearchmode_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BARCODESEARCHMODE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x113b); }
        template <typename Container = ICAP_BARCODESEARCHPRIORITIES_::value_type> bool is_barcodesearchpriorities_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BARCODESEARCHPRIORITIES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x113a); }
        template <typename Container = ICAP_BARCODETIMEOUT_::value_type> bool is_barcodetimeout_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BARCODETIMEOUT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x113d); }
        template <typename Container = ICAP_BITDEPTH_::value_type> bool is_bitdepth_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BITDEPTH_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x112b); }
        template <typename Container = ICAP_BITDEPTHREDUCTION_::value_type> bool is_bitdepthreduction_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BITDEPTHREDUCTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x112c); }
        template <typename Container = ICAP_BITORDER_::value_type> bool is_bitorder_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BITORDER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x111c); }
        template <typename Container = ICAP_BITORDERCODES_::value_type> bool is_bitordercodes_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BITORDERCODES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1126); }
        template <typename Container = ICAP_BRIGHTNESS_::value_type> bool is_brightness_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_BRIGHTNESS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1101); }
        template <typename Container = ICAP_CCITTKFACTOR_::value_type> bool is_ccittkfactor_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_CCITTKFACTOR_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x111d); }
        template <typename Container = ICAP_COLORMANAGEMENTENABLED_::value_type> bool is_colormanagementenabled_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_COLORMANAGEMENTENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x115b); }
        template <typename Container = ICAP_COMPRESSION_::value_type> bool is_compression_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_COMPRESSION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x0100); }
        template <typename Container = ICAP_CONTRAST_::value_type> bool is_contrast_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_CONTRAST_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1103); }
        template <typename Container = ICAP_CUSTHALFTONE_::value_type> bool is_custhalftone_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_CUSTHALFTONE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1104); }
        template <typename Container = ICAP_EXPOSURETIME_::value_type> bool is_exposuretime_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_EXPOSURETIME_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1105); }
        template <typename Container = ICAP_EXTIMAGEINFO_::value_type> bool is_extimageinfo_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_EXTIMAGEINFO_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x112f); }
        template <typename Container = ICAP_FEEDERTYPE_::value_type> bool is_feedertype_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_FEEDERTYPE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1154); }
        template <typename Container = ICAP_FILMTYPE_::value_type> bool is_filmtype_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_FILMTYPE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x115f); }
        template <typename Container = ICAP_FILTER_::value_type> bool is_filter_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_FILTER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1106); }
        template <typename Container = ICAP_FLASHUSED_::value_type> bool is_flashused_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_FLASHUSED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1107); }
        template <typename Container = ICAP_FLASHUSED2_::value_type> bool is_flashused2_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_FLASHUSED2_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1146); }
        template <typename Container = ICAP_FLIPROTATION_::value_type> bool is_fliprotation_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_FLIPROTATION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1136); }
        template <typename Container = ICAP_FRAMES_::value_type> bool is_frames_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_FRAMES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1114); }
        template <typename Container = ICAP_GAMMA_::value_type> bool is_gamma_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_GAMMA_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1108); }
        template <typename Container = ICAP_HALFTONES_::value_type> bool is_halftones_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_HALFTONES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1109); }
        template <typename Container = ICAP_HIGHLIGHT_::value_type> bool is_highlight_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_HIGHLIGHT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x110a); }
        template <typename Container = ICAP_ICCPROFILE_::value_type> bool is_iccprofile_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_ICCPROFILE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1155); }
        template <typename Container = ICAP_IMAGEDATASET_::value_type> bool is_imagedataset_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_IMAGEDATASET_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x112e); }
        template <typename Container = ICAP_IMAGEFILEFORMAT_::value_type> bool is_imagefileformat_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_IMAGEFILEFORMAT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x110c); }
        template <typename Container = ICAP_IMAGEFILTER_::value_type> bool is_imagefilter_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_IMAGEFILTER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1147); }
        template <typename Container = ICAP_IMAGEMERGE_::value_type> bool is_imagemerge_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_IMAGEMERGE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x115c); }
        template <typename Container = ICAP_IMAGEMERGEHEIGHTTHRESHOLD_::value_type> bool is_imagemergeheightthreshold_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_IMAGEMERGEHEIGHTTHRESHOLD_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x115d); }
        template <typename Container = ICAP_JPEGPIXELTYPE_::value_type> bool is_jpegpixeltype_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_JPEGPIXELTYPE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1128); }
        template <typename Container = ICAP_JPEGQUALITY_::value_type> bool is_jpegquality_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_JPEGQUALITY_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1153); }
        template <typename Container = ICAP_JPEGSUBSAMPLING_::value_type> bool is_jpegsubsampling_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_JPEGSUBSAMPLING_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1161); }
        template <typename Container = ICAP_LAMPSTATE_::value_type> bool is_lampstate_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_LAMPSTATE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x110d); }
        template <typename Container = ICAP_LIGHTPATH_::value_type> bool is_lightpath_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_LIGHTPATH_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x111e); }
        template <typename Container = ICAP_LIGHTSOURCE_::value_type> bool is_lightsource_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_LIGHTSOURCE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x110e); }
        template <typename Container = ICAP_MAXFRAMES_::value_type> bool is_maxframes_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_MAXFRAMES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x111a); }
        template <typename Container = ICAP_MINIMUMHEIGHT_::value_type> bool is_minimumheight_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_MINIMUMHEIGHT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1130); }
        template <typename Container = ICAP_MINIMUMWIDTH_::value_type> bool is_minimumwidth_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_MINIMUMWIDTH_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1131); }
        template <typename Container = ICAP_MIRROR_::value_type> bool is_mirror_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_MIRROR_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1160); }
        template <typename Container = ICAP_NOISEFILTER_::value_type> bool is_noisefilter_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_NOISEFILTER_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1148); }
        template <typename Container = ICAP_ORIENTATION_::value_type> bool is_orientation_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_ORIENTATION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1110); }
        template <typename Container = ICAP_OVERSCAN_::value_type> bool is_overscan_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_OVERSCAN_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1149); }
        template <typename Container = ICAP_PATCHCODEDETECTIONENABLED_::value_type> bool is_patchcodedetectionenabled_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PATCHCODEDETECTIONENABLED_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x113f); }
        template <typename Container = ICAP_PATCHCODEMAXRETRIES_::value_type> bool is_patchcodemaxretries_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PATCHCODEMAXRETRIES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1144); }
        template <typename Container = ICAP_PATCHCODEMAXSEARCHPRIORITIES_::value_type> bool is_patchcodemaxsearchpriorities_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PATCHCODEMAXSEARCHPRIORITIES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1141); }
        template <typename Container = ICAP_PATCHCODESEARCHMODE_::value_type> bool is_patchcodesearchmode_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PATCHCODESEARCHMODE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1143); }
        template <typename Container = ICAP_PATCHCODESEARCHPRIORITIES_::value_type> bool is_patchcodesearchpriorities_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PATCHCODESEARCHPRIORITIES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1142); }
        template <typename Container = ICAP_PATCHCODETIMEOUT_::value_type> bool is_patchcodetimeout_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PATCHCODETIMEOUT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1145); }
        template <typename Container = ICAP_PHYSICALHEIGHT_::value_type> bool is_physicalheight_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PHYSICALHEIGHT_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1112); }
        template <typename Container = ICAP_PHYSICALWIDTH_::value_type> bool is_physicalwidth_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PHYSICALWIDTH_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1111); }
        template <typename Container = ICAP_PIXELFLAVOR_::value_type> bool is_pixelflavor_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PIXELFLAVOR_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x111f); }
        template <typename Container = ICAP_PIXELFLAVORCODES_::value_type> bool is_pixelflavorcodes_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PIXELFLAVORCODES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1127); }
        template <typename Container = ICAP_PIXELTYPE_::value_type> bool is_pixeltype_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PIXELTYPE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x0101); }
        template <typename Container = ICAP_PLANARCHUNKY_::value_type> bool is_planarchunky_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_PLANARCHUNKY_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1120); }
        template <typename Container = ICAP_ROTATION_::value_type> bool is_rotation_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_ROTATION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1121); }
        template <typename Container = ICAP_SHADOW_::value_type> bool is_shadow_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_SHADOW_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1113); }
        template <typename Container = ICAP_SUPPORTEDBARCODETYPES_::value_type> bool is_supportedbarcodetypes_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_SUPPORTEDBARCODETYPES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1138); }
        template <typename Container = ICAP_SUPPORTEDEXTIMAGEINFO_::value_type> bool is_supportedextimageinfo_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_SUPPORTEDEXTIMAGEINFO_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x115e); }
        template <typename Container = ICAP_SUPPORTEDPATCHCODETYPES_::value_type> bool is_supportedpatchcodetypes_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_SUPPORTEDPATCHCODETYPES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1140); }
        template <typename Container = ICAP_SUPPORTEDSIZES_::value_type> bool is_supportedsizes_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_SUPPORTEDSIZES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1122); }
        template <typename Container = ICAP_THRESHOLD_::value_type> bool is_threshold_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_THRESHOLD_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1123); }
        template <typename Container = ICAP_TILES_::value_type> bool is_tiles_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_TILES_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x111b); }
        template <typename Container = ICAP_TIMEFILL_::value_type> bool is_timefill_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_TIMEFILL_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x112a); }
        template <typename Container = ICAP_UNDEFINEDIMAGESIZE_::value_type> bool is_undefinedimagesize_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_UNDEFINEDIMAGESIZE_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x112d); }
        template <typename Container = ICAP_UNITS_::value_type> bool is_units_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_UNITS_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x0102); }
        template <typename Container = ICAP_XFERMECH_::value_type> bool is_image_xfermech_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_XFERMECH_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x0103); }
        template <typename Container = ICAP_XNATIVERESOLUTION_::value_type> bool is_xnativeresolution_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_XNATIVERESOLUTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1116); }
        template <typename Container = ICAP_XRESOLUTION_::value_type> bool is_xresolution_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_XRESOLUTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1118); }
        template <typename Container = ICAP_XSCALING_::value_type> bool is_xscaling_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_XSCALING_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1124); }
        template <typename Container = ICAP_YNATIVERESOLUTION_::value_type> bool is_ynativeresolution_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_YNATIVERESOLUTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1117); }
        template <typename Container = ICAP_YRESOLUTION_::value_type> bool is_yresolution_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_YRESOLUTION_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1119); }
        template <typename Container = ICAP_YSCALING_::value_type> bool is_yscaling_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_YSCALING_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x1125); }
        template <typename Container = ICAP_ZOOMFACTOR_::value_type> bool is_zoomfactor_value_supported(const Container& c) const { return is_capvalue_supported<ICAP_ZOOMFACTOR_>(static_cast<dtwain_underlying_type_v<Container>>(c), 0x113e); }

        bool is_audio_xfermech_supported() const { return is_cap_supported(0x1202); }
        bool is_alarms_supported() const { return is_cap_supported(0x1018); }
        bool is_alarmvolume_supported() const { return is_cap_supported(0x1019); }
        bool is_author_supported() const { return is_cap_supported(0x1000); }
        bool is_autofeed_supported() const { return is_cap_supported(0x1007); }
        bool is_automaticcapture_supported() const { return is_cap_supported(0x101a); }
        bool is_automaticsensemedium_supported() const { return is_cap_supported(0x103b); }
        bool is_autoscan_supported() const { return is_cap_supported(0x1010); }
        bool is_batteryminutes_supported() const { return is_cap_supported(0x1032); }
        bool is_batterypercentage_supported() const { return is_cap_supported(0x1033); }
        bool is_cameraenabled_supported() const { return is_cap_supported(0x1036); }
        bool is_cameraorder_supported() const { return is_cap_supported(0x1037); }
        bool is_camerapreviewui_supported() const { return is_cap_supported(0x1021); }
        bool is_cameraside_supported() const { return is_cap_supported(0x1034); }
        bool is_caption_supported() const { return is_cap_supported(0x1001); }
        bool is_clearpage_supported() const { return is_cap_supported(0x1008); }
        bool is_customdsdata_supported() const { return is_cap_supported(0x1015); }
        bool is_custominterfaceguid_supported() const { return is_cap_supported(0x103c); }
        bool is_deviceevent_supported() const { return is_cap_supported(0x1022); }
        bool is_deviceonline_supported() const { return is_cap_supported(0x100f); }
        bool is_devicetimedate_supported() const { return is_cap_supported(0x101f); }
        bool is_doublefeeddetection_supported() const { return is_cap_supported(0x103f); }
        bool is_doublefeeddetectionlength_supported() const { return is_cap_supported(0x1040); }
        bool is_doublefeeddetectionresponse_supported() const { return is_cap_supported(0x1042); }
        bool is_doublefeeddetectionsensitivity_supported() const { return is_cap_supported(0x1041); }
        bool is_duplex_supported() const { return is_cap_supported(0x1012); }
        bool is_duplexenabled_supported() const { return is_cap_supported(0x1013); }
        bool is_enabledsuionly_supported() const { return is_cap_supported(0x1014); }
        bool is_endorser_supported() const { return is_cap_supported(0x1016); }
        bool is_extendedcaps_supported() const { return is_cap_supported(0x1006); }
        bool is_feederalignment_supported() const { return is_cap_supported(0x102d); }
        bool is_feederenabled_supported() const { return is_cap_supported(0x1002); }
        bool is_feederloaded_supported() const { return is_cap_supported(0x1003); }
        bool is_feederorder_supported() const { return is_cap_supported(0x102e); }
        bool is_feederpocket_supported() const { return is_cap_supported(0x103a); }
        bool is_feederprep_supported() const { return is_cap_supported(0x1039); }
        bool is_feedpage_supported() const { return is_cap_supported(0x1009); }
        bool is_indicators_supported() const { return is_cap_supported(0x100b); }
        bool is_indicatorsmode_supported() const { return is_cap_supported(0x1044); }
        bool is_jobcontrol_supported() const { return is_cap_supported(0x1017); }
        bool is_language_supported() const { return is_cap_supported(0x102c); }
        bool is_maxbatchbuffers_supported() const { return is_cap_supported(0x101e); }
        bool is_micrenabled_supported() const { return is_cap_supported(0x1038); }
        bool is_paperdetectable_supported() const { return is_cap_supported(0x100d); }
        bool is_paperhandling_supported() const { return is_cap_supported(0x1043); }
        bool is_powersavetime_supported() const { return is_cap_supported(0x1046); }
        bool is_powersupply_supported() const { return is_cap_supported(0x1020); }
        bool is_printer_supported() const { return is_cap_supported(0x1026); }
        bool is_printercharrotation_supported() const { return is_cap_supported(0x1047); }
        bool is_printerenabled_supported() const { return is_cap_supported(0x1027); }
        bool is_printerfontstyle_supported() const { return is_cap_supported(0x1048); }
        bool is_printerindex_supported() const { return is_cap_supported(0x1028); }
        bool is_printerindexleadchar_supported() const { return is_cap_supported(0x1049); }
        bool is_printerindexmaxvalue_supported() const { return is_cap_supported(0x104A); }
        bool is_printerindexnumdigits_supported() const { return is_cap_supported(0x104B); }
        bool is_printerindexstep_supported() const { return is_cap_supported(0x104C); }
        bool is_printerindextrigger_supported() const { return is_cap_supported(0x104D); }
        bool is_printermode_supported() const { return is_cap_supported(0x1029); }
        bool is_printerstring_supported() const { return is_cap_supported(0x102a); }
        bool is_printerstringpreview_supported() const { return is_cap_supported(0x104E); }
        bool is_printersuffix_supported() const { return is_cap_supported(0x102b); }
        bool is_printerverticaloffset_supported() const { return is_cap_supported(0x1045); }
        bool is_reacquireallowed_supported() const { return is_cap_supported(0x1030); }
        bool is_rewindpage_supported() const { return is_cap_supported(0x100a); }
        bool is_segmented_supported() const { return is_cap_supported(0x1035); }
        bool is_serialnumber_supported() const { return is_cap_supported(0x1024); }
        bool is_sheetcount_supported() const { return is_cap_supported(0x104F); }
        bool is_supportedcaps_supported() const { return is_cap_supported(0x1005); }
        bool is_supportedcapssegmentunique_supported() const { return is_cap_supported(0x103d); }
        bool is_supporteddats_supported() const { return is_cap_supported(0x103e); }
        bool is_thumbnailsenabled_supported() const { return is_cap_supported(0x1011); }
        bool is_timebeforefirstcapture_supported() const { return is_cap_supported(0x101b); }
        bool is_timebetweencaptures_supported() const { return is_cap_supported(0x101c); }
        bool is_timedate_supported() const { return is_cap_supported(0x1004); }
        bool is_uicontrollable_supported() const { return is_cap_supported(0x100e); }
        bool is_xfercount_supported() const { return is_cap_supported(0x0001); }
        bool is_autobright_supported() const { return is_cap_supported(0x1100); }
        bool is_autodiscardblankpages_supported() const { return is_cap_supported(0x1134); }
        bool is_automaticborderdetection_supported() const { return is_cap_supported(0x1150); }
        bool is_automaticcolorenabled_supported() const { return is_cap_supported(0x1159); }
        bool is_automaticcolornoncolorpixeltype_supported() const { return is_cap_supported(0x115a); }
        bool is_automaticcropusesframe_supported() const { return is_cap_supported(0x1157); }
        bool is_automaticdeskew_supported() const { return is_cap_supported(0x1151); }
        bool is_automaticlengthdetection_supported() const { return is_cap_supported(0x1158); }
        bool is_automaticrotate_supported() const { return is_cap_supported(0x1152); }
        bool is_autosize_supported() const { return is_cap_supported(0x1156); }
        bool is_barcodedetectionenabled_supported() const { return is_cap_supported(0x1137); }
        bool is_barcodemaxretries_supported() const { return is_cap_supported(0x113c); }
        bool is_barcodemaxsearchpriorities_supported() const { return is_cap_supported(0x1139); }
        bool is_barcodesearchmode_supported() const { return is_cap_supported(0x113b); }
        bool is_barcodesearchpriorities_supported() const { return is_cap_supported(0x113a); }
        bool is_barcodetimeout_supported() const { return is_cap_supported(0x113d); }
        bool is_bitdepth_supported() const { return is_cap_supported(0x112b); }
        bool is_bitdepthreduction_supported() const { return is_cap_supported(0x112c); }
        bool is_bitorder_supported() const { return is_cap_supported(0x111c); }
        bool is_bitordercodes_supported() const { return is_cap_supported(0x1126); }
        bool is_brightness_supported() const { return is_cap_supported(0x1101); }
        bool is_ccittkfactor_supported() const { return is_cap_supported(0x111d); }
        bool is_colormanagementenabled_supported() const { return is_cap_supported(0x115b); }
        bool is_compression_supported() const { return is_cap_supported(0x0100); }
        bool is_contrast_supported() const { return is_cap_supported(0x1103); }
        bool is_custhalftone_supported() const { return is_cap_supported(0x1104); }
        bool is_exposuretime_supported() const { return is_cap_supported(0x1105); }
        bool is_extimageinfo_supported() const { return is_cap_supported(0x112f); }
        bool is_feedertype_supported() const { return is_cap_supported(0x1154); }
        bool is_filmtype_supported() const { return is_cap_supported(0x115f); }
        bool is_filter_supported() const { return is_cap_supported(0x1106); }
        bool is_flashused_supported() const { return is_cap_supported(0x1107); }
        bool is_flashused2_supported() const { return is_cap_supported(0x1146); }
        bool is_fliprotation_supported() const { return is_cap_supported(0x1136); }
        bool is_frames_supported() const { return is_cap_supported(0x1114); }
        bool is_gamma_supported() const { return is_cap_supported(0x1108); }
        bool is_halftones_supported() const { return is_cap_supported(0x1109); }
        bool is_highlight_supported() const { return is_cap_supported(0x110a); }
        bool is_iccprofile_supported() const { return is_cap_supported(0x1155); }
        bool is_imagedataset_supported() const { return is_cap_supported(0x112e); }
        bool is_imagefileformat_supported() const { return is_cap_supported(0x110c); }
        bool is_imagefilter_supported() const { return is_cap_supported(0x1147); }
        bool is_imagemerge_supported() const { return is_cap_supported(0x115c); }
        bool is_imagemergeheightthreshold_supported() const { return is_cap_supported(0x115d); }
        bool is_jpegpixeltype_supported() const { return is_cap_supported(0x1128); }
        bool is_jpegquality_supported() const { return is_cap_supported(0x1153); }
        bool is_jpegsubsampling_supported() const { return is_cap_supported(0x1161); }
        bool is_lampstate_supported() const { return is_cap_supported(0x110d); }
        bool is_lightpath_supported() const { return is_cap_supported(0x111e); }
        bool is_lightsource_supported() const { return is_cap_supported(0x110e); }
        bool is_maxframes_supported() const { return is_cap_supported(0x111a); }
        bool is_minimumheight_supported() const { return is_cap_supported(0x1130); }
        bool is_minimumwidth_supported() const { return is_cap_supported(0x1131); }
        bool is_mirror_supported() const { return is_cap_supported(0x1160); }
        bool is_noisefilter_supported() const { return is_cap_supported(0x1148); }
        bool is_orientation_supported() const { return is_cap_supported(0x1110); }
        bool is_overscan_supported() const { return is_cap_supported(0x1149); }
        bool is_patchcodedetectionenabled_supported() const { return is_cap_supported(0x113f); }
        bool is_patchcodemaxretries_supported() const { return is_cap_supported(0x1144); }
        bool is_patchcodemaxsearchpriorities_supported() const { return is_cap_supported(0x1141); }
        bool is_patchcodesearchmode_supported() const { return is_cap_supported(0x1143); }
        bool is_patchcodesearchpriorities_supported() const { return is_cap_supported(0x1142); }
        bool is_patchcodetimeout_supported() const { return is_cap_supported(0x1145); }
        bool is_physicalheight_supported() const { return is_cap_supported(0x1112); }
        bool is_physicalwidth_supported() const { return is_cap_supported(0x1111); }
        bool is_pixelflavor_supported() const { return is_cap_supported(0x111f); }
        bool is_pixelflavorcodes_supported() const { return is_cap_supported(0x1127); }
        bool is_pixeltype_supported() const { return is_cap_supported(0x0101); }
        bool is_planarchunky_supported() const { return is_cap_supported(0x1120); }
        bool is_rotation_supported() const { return is_cap_supported(0x1121); }
        bool is_shadow_supported() const { return is_cap_supported(0x1113); }
        bool is_supportedbarcodetypes_supported() const { return is_cap_supported(0x1138); }
        bool is_supportedextimageinfo_supported() const { return is_cap_supported(0x115e); }
        bool is_supportedpatchcodetypes_supported() const { return is_cap_supported(0x1140); }
        bool is_supportedsizes_supported() const { return is_cap_supported(0x1122); }
        bool is_threshold_supported() const { return is_cap_supported(0x1123); }
        bool is_tiles_supported() const { return is_cap_supported(0x111b); }
        bool is_timefill_supported() const { return is_cap_supported(0x112a); }
        bool is_undefinedimagesize_supported() const { return is_cap_supported(0x112d); }
        bool is_units_supported() const { return is_cap_supported(0x0102); }
        bool is_image_xfermech_supported() const { return is_cap_supported(0x0103); }
        bool is_xnativeresolution_supported() const { return is_cap_supported(0x1116); }
        bool is_xresolution_supported() const { return is_cap_supported(0x1118); }
        bool is_xscaling_supported() const { return is_cap_supported(0x1124); }
        bool is_ynativeresolution_supported() const { return is_cap_supported(0x1117); }
        bool is_yresolution_supported() const { return is_cap_supported(0x1119); }
        bool is_yscaling_supported() const { return is_cap_supported(0x1125); }
        bool is_zoomfactor_supported() const { return is_cap_supported(0x113e); }


        bool is_ext_barcodeconfidence_supported() const { return is_extendedimage_cap_supported(0x121A); }
        bool is_ext_barcodecount_supported() const { return is_extendedimage_cap_supported(0x1219); }
        bool is_ext_barcoderotation_supported() const { return is_extendedimage_cap_supported(0x121B); }
        bool is_ext_barcodetext_supported() const { return is_extendedimage_cap_supported(0x1202); }
        bool is_ext_barcodetextlength_supported() const { return is_extendedimage_cap_supported(0x121C); }
        bool is_ext_barcodetype_supported() const { return is_extendedimage_cap_supported(0x1203); }
        bool is_ext_barcodex_supported() const { return is_extendedimage_cap_supported(0x1200); }
        bool is_ext_barcodey_supported() const { return is_extendedimage_cap_supported(0x1201); }
        bool is_ext_blackspecklesremoved_supported() const { return is_extendedimage_cap_supported(0x1227); }
        bool is_ext_bookname_supported() const { return is_extendedimage_cap_supported(0x1238); }
        bool is_ext_camera_supported() const { return is_extendedimage_cap_supported(0x123C); }
        bool is_ext_chapternumber_supported() const { return is_extendedimage_cap_supported(0x1239); }
        bool is_ext_deshadeblackcountnew_supported() const { return is_extendedimage_cap_supported(0x121F); }
        bool is_ext_deshadeblackcountold_supported() const { return is_extendedimage_cap_supported(0x121E); }
        bool is_ext_deshadeblackrlmax_supported() const { return is_extendedimage_cap_supported(0x1221); }
        bool is_ext_deshadeblackrlmin_supported() const { return is_extendedimage_cap_supported(0x1220); }
        bool is_ext_deshadecount_supported() const { return is_extendedimage_cap_supported(0x121D); }
        bool is_ext_deshadeheight_supported() const { return is_extendedimage_cap_supported(0x1206); }
        bool is_ext_deshadeleft_supported() const { return is_extendedimage_cap_supported(0x1205); }
        bool is_ext_deshadesize_supported() const { return is_extendedimage_cap_supported(0x1208); }
        bool is_ext_deshadetop_supported() const { return is_extendedimage_cap_supported(0x1204); }
        bool is_ext_deshadewhitecountnew_supported() const { return is_extendedimage_cap_supported(0x1223); }
        bool is_ext_deshadewhitecountold_supported() const { return is_extendedimage_cap_supported(0x1222); }
        bool is_ext_deshadewhiterlave_supported() const { return is_extendedimage_cap_supported(0x1225); }
        bool is_ext_deshadewhiterlmax_supported() const { return is_extendedimage_cap_supported(0x1226); }
        bool is_ext_deshadewhiterlmin_supported() const { return is_extendedimage_cap_supported(0x1224); }
        bool is_ext_deshadewidth_supported() const { return is_extendedimage_cap_supported(0x1207); }
        bool is_ext_deskewstatus_supported() const { return is_extendedimage_cap_supported(0x122B); }
        bool is_ext_documentnumber_supported() const { return is_extendedimage_cap_supported(0x123A); }
        bool is_ext_endorsedtext_supported() const { return is_extendedimage_cap_supported(0x1213); }
        bool is_ext_filesystemsource_supported() const { return is_extendedimage_cap_supported(0x1246); }
        bool is_ext_formconfidence_supported() const { return is_extendedimage_cap_supported(0x1214); }
        bool is_ext_formhorzdocoffset_supported() const { return is_extendedimage_cap_supported(0x1217); }
        bool is_ext_formtemplatematch_supported() const { return is_extendedimage_cap_supported(0x1215); }
        bool is_ext_formtemplatepagematch_supported() const { return is_extendedimage_cap_supported(0x1216); }
        bool is_ext_formvertdocoffset_supported() const { return is_extendedimage_cap_supported(0x1218); }
        bool is_ext_frame_supported() const { return is_extendedimage_cap_supported(0x123E); }
        bool is_ext_framenumber_supported() const { return is_extendedimage_cap_supported(0x123D); }
        bool is_ext_horzlinecount_supported() const { return is_extendedimage_cap_supported(0x1229); }
        bool is_ext_horzlinelength_supported() const { return is_extendedimage_cap_supported(0x120C); }
        bool is_ext_horzlinethickness_supported() const { return is_extendedimage_cap_supported(0x120D); }
        bool is_ext_horzlinexcoord_supported() const { return is_extendedimage_cap_supported(0x120A); }
        bool is_ext_horzlineycoord_supported() const { return is_extendedimage_cap_supported(0x120B); }
        bool is_ext_iccprofile_supported() const { return is_extendedimage_cap_supported(0x1240); }
        bool is_ext_imagemerged_supported() const { return is_extendedimage_cap_supported(0x1247); }
        bool is_ext_lastsegment_supported() const { return is_extendedimage_cap_supported(0x1241); }
        bool is_ext_magdata_supported() const { return is_extendedimage_cap_supported(0x1243); }
        bool is_ext_magdatalength_supported() const { return is_extendedimage_cap_supported(0x1248); }
        bool is_ext_magtype_supported() const { return is_extendedimage_cap_supported(0x1244); }
        bool is_ext_pagenumber_supported() const { return is_extendedimage_cap_supported(0x123B); }
        bool is_ext_pageside_supported() const { return is_extendedimage_cap_supported(0x1245); }
        bool is_ext_papercount_supported() const { return is_extendedimage_cap_supported(0x1249); }
        bool is_ext_patchcode_supported() const { return is_extendedimage_cap_supported(0x1212); }
        bool is_ext_pixelflavor_supported() const { return is_extendedimage_cap_supported(0x123F); }
        bool is_ext_printertext_supported() const { return is_extendedimage_cap_supported(0x124A); }
        bool is_ext_segmentnumber_supported() const { return is_extendedimage_cap_supported(0x1242); }
        bool is_ext_skewconfidence_supported() const { return is_extendedimage_cap_supported(0x122E); }
        bool is_ext_skewfinalangle_supported() const { return is_extendedimage_cap_supported(0x122D); }
        bool is_ext_skeworiginalangle_supported() const { return is_extendedimage_cap_supported(0x122C); }
        bool is_ext_skewwindowx1_supported() const { return is_extendedimage_cap_supported(0x122F); }
        bool is_ext_skewwindowx2_supported() const { return is_extendedimage_cap_supported(0x1231); }
        bool is_ext_skewwindowx3_supported() const { return is_extendedimage_cap_supported(0x1233); }
        bool is_ext_skewwindowx4_supported() const { return is_extendedimage_cap_supported(0x1235); }
        bool is_ext_skewwindowy1_supported() const { return is_extendedimage_cap_supported(0x1230); }
        bool is_ext_skewwindowy2_supported() const { return is_extendedimage_cap_supported(0x1232); }
        bool is_ext_skewwindowy3_supported() const { return is_extendedimage_cap_supported(0x1234); }
        bool is_ext_skewwindowy4_supported() const { return is_extendedimage_cap_supported(0x1236); }
        bool is_ext_specklesremoved_supported() const { return is_extendedimage_cap_supported(0x1209); }
        bool is_ext_twaindirectmetadata_supported() const { return is_extendedimage_cap_supported(0x124B); }
        bool is_ext_vertlinecount_supported() const { return is_extendedimage_cap_supported(0x122A); }
        bool is_ext_vertlinelength_supported() const { return is_extendedimage_cap_supported(0x1210); }
        bool is_ext_vertlinethickness_supported() const { return is_extendedimage_cap_supported(0x1211); }
        bool is_ext_vertlinexcoord_supported() const { return is_extendedimage_cap_supported(0x120E); }
        bool is_ext_vertlineycoord_supported() const { return is_extendedimage_cap_supported(0x120F); }
        bool is_ext_whitespecklesremoved_supported() const { return is_extendedimage_cap_supported(0x1228); }

        twain_container_type::value_type get_audio_xfermech_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1202, gcType); }
        twain_container_type::value_type get_alarms_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1018, gcType); }
        twain_container_type::value_type get_alarmvolume_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1019, gcType); }
        twain_container_type::value_type get_author_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1000, gcType); }
        twain_container_type::value_type get_autofeed_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1007, gcType); }
        twain_container_type::value_type get_automaticcapture_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x101a, gcType); }
        twain_container_type::value_type get_automaticsensemedium_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x103b, gcType); }
        twain_container_type::value_type get_autoscan_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1010, gcType); }
        twain_container_type::value_type get_batteryminutes_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1032, gcType); }
        twain_container_type::value_type get_batterypercentage_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1033, gcType); }
        twain_container_type::value_type get_cameraenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1036, gcType); }
        twain_container_type::value_type get_cameraorder_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1037, gcType); }
        twain_container_type::value_type get_camerapreviewui_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1021, gcType); }
        twain_container_type::value_type get_cameraside_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1034, gcType); }
        twain_container_type::value_type get_caption_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1001, gcType); }
        twain_container_type::value_type get_clearpage_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1008, gcType); }
        twain_container_type::value_type get_customdsdata_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1015, gcType); }
        twain_container_type::value_type get_custominterfaceguid_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x103c, gcType); }
        twain_container_type::value_type get_deviceevent_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1022, gcType); }
        twain_container_type::value_type get_deviceonline_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x100f, gcType); }
        twain_container_type::value_type get_devicetimedate_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x101f, gcType); }
        twain_container_type::value_type get_doublefeeddetection_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x103f, gcType); }
        twain_container_type::value_type get_doublefeeddetectionlength_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1040, gcType); }
        twain_container_type::value_type get_doublefeeddetectionresponse_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1042, gcType); }
        twain_container_type::value_type get_doublefeeddetectionsensitivity_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1041, gcType); }
        twain_container_type::value_type get_duplex_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1012, gcType); }
        twain_container_type::value_type get_duplexenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1013, gcType); }
        twain_container_type::value_type get_enabledsuionly_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1014, gcType); }
        twain_container_type::value_type get_endorser_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1016, gcType); }
        twain_container_type::value_type get_extendedcaps_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1006, gcType); }
        twain_container_type::value_type get_feederalignment_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x102d, gcType); }
        twain_container_type::value_type get_feederenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1002, gcType); }
        twain_container_type::value_type get_feederloaded_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1003, gcType); }
        twain_container_type::value_type get_feederorder_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x102e, gcType); }
        twain_container_type::value_type get_feederpocket_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x103a, gcType); }
        twain_container_type::value_type get_feederprep_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1039, gcType); }
        twain_container_type::value_type get_feedpage_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1009, gcType); }
        twain_container_type::value_type get_indicators_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x100b, gcType); }
        twain_container_type::value_type get_indicatorsmode_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1044, gcType); }
        twain_container_type::value_type get_jobcontrol_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1017, gcType); }
        twain_container_type::value_type get_language_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x102c, gcType); }
        twain_container_type::value_type get_maxbatchbuffers_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x101e, gcType); }
        twain_container_type::value_type get_micrenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1038, gcType); }
        twain_container_type::value_type get_paperdetectable_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x100d, gcType); }
        twain_container_type::value_type get_paperhandling_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1043, gcType); }
        twain_container_type::value_type get_powersavetime_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1046, gcType); }
        twain_container_type::value_type get_powersupply_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1020, gcType); }
        twain_container_type::value_type get_printer_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1026, gcType); }
        twain_container_type::value_type get_printercharrotation_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1047, gcType); }
        twain_container_type::value_type get_printerenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1027, gcType); }
        twain_container_type::value_type get_printerfontstyle_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1048, gcType); }
        twain_container_type::value_type get_printerindex_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1028, gcType); }
        twain_container_type::value_type get_printerindexleadchar_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1049, gcType); }
        twain_container_type::value_type get_printerindexmaxvalue_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x104A, gcType); }
        twain_container_type::value_type get_printerindexnumdigits_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x104B, gcType); }
        twain_container_type::value_type get_printerindexstep_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x104C, gcType); }
        twain_container_type::value_type get_printerindextrigger_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x104D, gcType); }
        twain_container_type::value_type get_printermode_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1029, gcType); }
        twain_container_type::value_type get_printerstring_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x102a, gcType); }
        twain_container_type::value_type get_printerstringpreview_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x104E, gcType); }
        twain_container_type::value_type get_printersuffix_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x102b, gcType); }
        twain_container_type::value_type get_printerverticaloffset_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1045, gcType); }
        twain_container_type::value_type get_reacquireallowed_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1030, gcType); }
        twain_container_type::value_type get_rewindpage_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x100a, gcType); }
        twain_container_type::value_type get_segmented_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1035, gcType); }
        twain_container_type::value_type get_serialnumber_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1024, gcType); }
        twain_container_type::value_type get_sheetcount_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x104F, gcType); }
        twain_container_type::value_type get_supportedcaps_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1005, gcType); }
        twain_container_type::value_type get_supportedcapssegmentunique_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x103d, gcType); }
        twain_container_type::value_type get_supporteddats_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x103e, gcType); }
        twain_container_type::value_type get_thumbnailsenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1011, gcType); }
        twain_container_type::value_type get_timebeforefirstcapture_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x101b, gcType); }
        twain_container_type::value_type get_timebetweencaptures_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x101c, gcType); }
        twain_container_type::value_type get_timedate_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1004, gcType); }
        twain_container_type::value_type get_uicontrollable_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x100e, gcType); }
        twain_container_type::value_type get_xfercount_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x0001, gcType); }
        twain_container_type::value_type get_autobright_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1100, gcType); }
        twain_container_type::value_type get_autodiscardblankpages_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1134, gcType); }
        twain_container_type::value_type get_automaticborderdetection_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1150, gcType); }
        twain_container_type::value_type get_automaticcolorenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1159, gcType); }
        twain_container_type::value_type get_automaticcolornoncolorpixeltype_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x115a, gcType); }
        twain_container_type::value_type get_automaticcropusesframe_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1157, gcType); }
        twain_container_type::value_type get_automaticdeskew_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1151, gcType); }
        twain_container_type::value_type get_automaticlengthdetection_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1158, gcType); }
        twain_container_type::value_type get_automaticrotate_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1152, gcType); }
        twain_container_type::value_type get_autosize_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1156, gcType); }
        twain_container_type::value_type get_barcodedetectionenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1137, gcType); }
        twain_container_type::value_type get_barcodemaxretries_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x113c, gcType); }
        twain_container_type::value_type get_barcodemaxsearchpriorities_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1139, gcType); }
        twain_container_type::value_type get_barcodesearchmode_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x113b, gcType); }
        twain_container_type::value_type get_barcodesearchpriorities_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x113a, gcType); }
        twain_container_type::value_type get_barcodetimeout_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x113d, gcType); }
        twain_container_type::value_type get_bitdepth_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x112b, gcType); }
        twain_container_type::value_type get_bitdepthreduction_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x112c, gcType); }
        twain_container_type::value_type get_bitorder_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x111c, gcType); }
        twain_container_type::value_type get_bitordercodes_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1126, gcType); }
        twain_container_type::value_type get_brightness_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1101, gcType); }
        twain_container_type::value_type get_ccittkfactor_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x111d, gcType); }
        twain_container_type::value_type get_colormanagementenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x115b, gcType); }
        twain_container_type::value_type get_compression_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x0100, gcType); }
        twain_container_type::value_type get_contrast_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1103, gcType); }
        twain_container_type::value_type get_custhalftone_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1104, gcType); }
        twain_container_type::value_type get_exposuretime_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1105, gcType); }
        twain_container_type::value_type get_extimageinfo_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x112f, gcType); }
        twain_container_type::value_type get_feedertype_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1154, gcType); }
        twain_container_type::value_type get_filmtype_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x115f, gcType); }
        twain_container_type::value_type get_filter_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1106, gcType); }
        twain_container_type::value_type get_flashused_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1107, gcType); }
        twain_container_type::value_type get_flashused2_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1146, gcType); }
        twain_container_type::value_type get_fliprotation_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1136, gcType); }
        twain_container_type::value_type get_frames_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1114, gcType); }
        twain_container_type::value_type get_gamma_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1108, gcType); }
        twain_container_type::value_type get_halftones_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1109, gcType); }
        twain_container_type::value_type get_highlight_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x110a, gcType); }
        twain_container_type::value_type get_iccprofile_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1155, gcType); }
        twain_container_type::value_type get_imagedataset_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x112e, gcType); }
        twain_container_type::value_type get_imagefileformat_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x110c, gcType); }
        twain_container_type::value_type get_imagefilter_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1147, gcType); }
        twain_container_type::value_type get_imagemerge_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x115c, gcType); }
        twain_container_type::value_type get_imagemergeheightthreshold_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x115d, gcType); }
        twain_container_type::value_type get_jpegpixeltype_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1128, gcType); }
        twain_container_type::value_type get_jpegquality_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1153, gcType); }
        twain_container_type::value_type get_jpegsubsampling_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1161, gcType); }
        twain_container_type::value_type get_lampstate_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x110d, gcType); }
        twain_container_type::value_type get_lightpath_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x111e, gcType); }
        twain_container_type::value_type get_lightsource_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x110e, gcType); }
        twain_container_type::value_type get_maxframes_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x111a, gcType); }
        twain_container_type::value_type get_minimumheight_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1130, gcType); }
        twain_container_type::value_type get_minimumwidth_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1131, gcType); }
        twain_container_type::value_type get_mirror_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1160, gcType); }
        twain_container_type::value_type get_noisefilter_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1148, gcType); }
        twain_container_type::value_type get_orientation_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1110, gcType); }
        twain_container_type::value_type get_overscan_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1149, gcType); }
        twain_container_type::value_type get_patchcodedetectionenabled_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x113f, gcType); }
        twain_container_type::value_type get_patchcodemaxretries_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1144, gcType); }
        twain_container_type::value_type get_patchcodemaxsearchpriorities_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1141, gcType); }
        twain_container_type::value_type get_patchcodesearchmode_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1143, gcType); }
        twain_container_type::value_type get_patchcodesearchpriorities_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1142, gcType); }
        twain_container_type::value_type get_patchcodetimeout_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1145, gcType); }
        twain_container_type::value_type get_physicalheight_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1112, gcType); }
        twain_container_type::value_type get_physicalwidth_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1111, gcType); }
        twain_container_type::value_type get_pixelflavor_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x111f, gcType); }
        twain_container_type::value_type get_pixelflavorcodes_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1127, gcType); }
        twain_container_type::value_type get_pixeltype_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x0101, gcType); }
        twain_container_type::value_type get_planarchunky_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1120, gcType); }
        twain_container_type::value_type get_rotation_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1121, gcType); }
        twain_container_type::value_type get_shadow_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1113, gcType); }
        twain_container_type::value_type get_supportedbarcodetypes_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1138, gcType); }
        twain_container_type::value_type get_supportedextimageinfo_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x115e, gcType); }
        twain_container_type::value_type get_supportedpatchcodetypes_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1140, gcType); }
        twain_container_type::value_type get_supportedsizes_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1122, gcType); }
        twain_container_type::value_type get_threshold_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1123, gcType); }
        twain_container_type::value_type get_tiles_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x111b, gcType); }
        twain_container_type::value_type get_timefill_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x112a, gcType); }
        twain_container_type::value_type get_undefinedimagesize_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x112d, gcType); }
        twain_container_type::value_type get_units_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x0102, gcType); }
        twain_container_type::value_type get_image_xfermech_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x0103, gcType); }
        twain_container_type::value_type get_xnativeresolution_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1116, gcType); }
        twain_container_type::value_type get_xresolution_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1118, gcType); }
        twain_container_type::value_type get_xscaling_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1124, gcType); }
        twain_container_type::value_type get_ynativeresolution_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1117, gcType); }
        twain_container_type::value_type get_yresolution_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1119, gcType); }
        twain_container_type::value_type get_yscaling_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x1125, gcType); }
        twain_container_type::value_type get_zoomfactor_container_type(const getcap_operation_info& gcType) { return get_cap_container_type(0x113e, gcType); }

        twain_container_type::value_type get_audio_xfermech_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1202, scType); }
        twain_container_type::value_type get_alarms_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1018, scType); }
        twain_container_type::value_type get_alarmvolume_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1019, scType); }
        twain_container_type::value_type get_author_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1000, scType); }
        twain_container_type::value_type get_autofeed_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1007, scType); }
        twain_container_type::value_type get_automaticcapture_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x101a, scType); }
        twain_container_type::value_type get_automaticsensemedium_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x103b, scType); }
        twain_container_type::value_type get_autoscan_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1010, scType); }
        twain_container_type::value_type get_batteryminutes_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1032, scType); }
        twain_container_type::value_type get_batterypercentage_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1033, scType); }
        twain_container_type::value_type get_cameraenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1036, scType); }
        twain_container_type::value_type get_cameraorder_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1037, scType); }
        twain_container_type::value_type get_camerapreviewui_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1021, scType); }
        twain_container_type::value_type get_cameraside_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1034, scType); }
        twain_container_type::value_type get_caption_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1001, scType); }
        twain_container_type::value_type get_clearpage_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1008, scType); }
        twain_container_type::value_type get_customdsdata_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1015, scType); }
        twain_container_type::value_type get_custominterfaceguid_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x103c, scType); }
        twain_container_type::value_type get_deviceevent_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1022, scType); }
        twain_container_type::value_type get_deviceonline_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x100f, scType); }
        twain_container_type::value_type get_devicetimedate_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x101f, scType); }
        twain_container_type::value_type get_doublefeeddetection_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x103f, scType); }
        twain_container_type::value_type get_doublefeeddetectionlength_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1040, scType); }
        twain_container_type::value_type get_doublefeeddetectionresponse_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1042, scType); }
        twain_container_type::value_type get_doublefeeddetectionsensitivity_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1041, scType); }
        twain_container_type::value_type get_duplex_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1012, scType); }
        twain_container_type::value_type get_duplexenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1013, scType); }
        twain_container_type::value_type get_enabledsuionly_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1014, scType); }
        twain_container_type::value_type get_endorser_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1016, scType); }
        twain_container_type::value_type get_extendedcaps_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1006, scType); }
        twain_container_type::value_type get_feederalignment_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x102d, scType); }
        twain_container_type::value_type get_feederenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1002, scType); }
        twain_container_type::value_type get_feederloaded_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1003, scType); }
        twain_container_type::value_type get_feederorder_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x102e, scType); }
        twain_container_type::value_type get_feederpocket_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x103a, scType); }
        twain_container_type::value_type get_feederprep_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1039, scType); }
        twain_container_type::value_type get_feedpage_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1009, scType); }
        twain_container_type::value_type get_indicators_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x100b, scType); }
        twain_container_type::value_type get_indicatorsmode_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1044, scType); }
        twain_container_type::value_type get_jobcontrol_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1017, scType); }
        twain_container_type::value_type get_language_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x102c, scType); }
        twain_container_type::value_type get_maxbatchbuffers_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x101e, scType); }
        twain_container_type::value_type get_micrenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1038, scType); }
        twain_container_type::value_type get_paperdetectable_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x100d, scType); }
        twain_container_type::value_type get_paperhandling_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1043, scType); }
        twain_container_type::value_type get_powersavetime_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1046, scType); }
        twain_container_type::value_type get_powersupply_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1020, scType); }
        twain_container_type::value_type get_printer_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1026, scType); }
        twain_container_type::value_type get_printercharrotation_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1047, scType); }
        twain_container_type::value_type get_printerenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1027, scType); }
        twain_container_type::value_type get_printerfontstyle_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1048, scType); }
        twain_container_type::value_type get_printerindex_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1028, scType); }
        twain_container_type::value_type get_printerindexleadchar_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1049, scType); }
        twain_container_type::value_type get_printerindexmaxvalue_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x104A, scType); }
        twain_container_type::value_type get_printerindexnumdigits_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x104B, scType); }
        twain_container_type::value_type get_printerindexstep_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x104C, scType); }
        twain_container_type::value_type get_printerindextrigger_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x104D, scType); }
        twain_container_type::value_type get_printermode_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1029, scType); }
        twain_container_type::value_type get_printerstring_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x102a, scType); }
        twain_container_type::value_type get_printerstringpreview_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x104E, scType); }
        twain_container_type::value_type get_printersuffix_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x102b, scType); }
        twain_container_type::value_type get_printerverticaloffset_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1045, scType); }
        twain_container_type::value_type get_reacquireallowed_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1030, scType); }
        twain_container_type::value_type get_rewindpage_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x100a, scType); }
        twain_container_type::value_type get_segmented_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1035, scType); }
        twain_container_type::value_type get_serialnumber_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1024, scType); }
        twain_container_type::value_type get_sheetcount_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x104F, scType); }
        twain_container_type::value_type get_supportedcaps_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1005, scType); }
        twain_container_type::value_type get_supportedcapssegmentunique_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x103d, scType); }
        twain_container_type::value_type get_supporteddats_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x103e, scType); }
        twain_container_type::value_type get_thumbnailsenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1011, scType); }
        twain_container_type::value_type get_timebeforefirstcapture_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x101b, scType); }
        twain_container_type::value_type get_timebetweencaptures_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x101c, scType); }
        twain_container_type::value_type get_timedate_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1004, scType); }
        twain_container_type::value_type get_uicontrollable_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x100e, scType); }
        twain_container_type::value_type get_xfercount_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x0001, scType); }
        twain_container_type::value_type get_autobright_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1100, scType); }
        twain_container_type::value_type get_autodiscardblankpages_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1134, scType); }
        twain_container_type::value_type get_automaticborderdetection_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1150, scType); }
        twain_container_type::value_type get_automaticcolorenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1159, scType); }
        twain_container_type::value_type get_automaticcolornoncolorpixeltype_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x115a, scType); }
        twain_container_type::value_type get_automaticcropusesframe_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1157, scType); }
        twain_container_type::value_type get_automaticdeskew_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1151, scType); }
        twain_container_type::value_type get_automaticlengthdetection_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1158, scType); }
        twain_container_type::value_type get_automaticrotate_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1152, scType); }
        twain_container_type::value_type get_autosize_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1156, scType); }
        twain_container_type::value_type get_barcodedetectionenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1137, scType); }
        twain_container_type::value_type get_barcodemaxretries_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x113c, scType); }
        twain_container_type::value_type get_barcodemaxsearchpriorities_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1139, scType); }
        twain_container_type::value_type get_barcodesearchmode_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x113b, scType); }
        twain_container_type::value_type get_barcodesearchpriorities_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x113a, scType); }
        twain_container_type::value_type get_barcodetimeout_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x113d, scType); }
        twain_container_type::value_type get_bitdepth_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x112b, scType); }
        twain_container_type::value_type get_bitdepthreduction_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x112c, scType); }
        twain_container_type::value_type get_bitorder_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x111c, scType); }
        twain_container_type::value_type get_bitordercodes_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1126, scType); }
        twain_container_type::value_type get_brightness_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1101, scType); }
        twain_container_type::value_type get_ccittkfactor_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x111d, scType); }
        twain_container_type::value_type get_colormanagementenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x115b, scType); }
        twain_container_type::value_type get_compression_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x0100, scType); }
        twain_container_type::value_type get_contrast_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1103, scType); }
        twain_container_type::value_type get_custhalftone_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1104, scType); }
        twain_container_type::value_type get_exposuretime_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1105, scType); }
        twain_container_type::value_type get_extimageinfo_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x112f, scType); }
        twain_container_type::value_type get_feedertype_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1154, scType); }
        twain_container_type::value_type get_filmtype_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x115f, scType); }
        twain_container_type::value_type get_filter_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1106, scType); }
        twain_container_type::value_type get_flashused_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1107, scType); }
        twain_container_type::value_type get_flashused2_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1146, scType); }
        twain_container_type::value_type get_fliprotation_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1136, scType); }
        twain_container_type::value_type get_frames_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1114, scType); }
        twain_container_type::value_type get_gamma_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1108, scType); }
        twain_container_type::value_type get_halftones_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1109, scType); }
        twain_container_type::value_type get_highlight_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x110a, scType); }
        twain_container_type::value_type get_iccprofile_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1155, scType); }
        twain_container_type::value_type get_imagedataset_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x112e, scType); }
        twain_container_type::value_type get_imagefileformat_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x110c, scType); }
        twain_container_type::value_type get_imagefilter_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1147, scType); }
        twain_container_type::value_type get_imagemerge_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x115c, scType); }
        twain_container_type::value_type get_imagemergeheightthreshold_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x115d, scType); }
        twain_container_type::value_type get_jpegpixeltype_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1128, scType); }
        twain_container_type::value_type get_jpegquality_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1153, scType); }
        twain_container_type::value_type get_jpegsubsampling_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1161, scType); }
        twain_container_type::value_type get_lampstate_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x110d, scType); }
        twain_container_type::value_type get_lightpath_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x111e, scType); }
        twain_container_type::value_type get_lightsource_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x110e, scType); }
        twain_container_type::value_type get_maxframes_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x111a, scType); }
        twain_container_type::value_type get_minimumheight_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1130, scType); }
        twain_container_type::value_type get_minimumwidth_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1131, scType); }
        twain_container_type::value_type get_mirror_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1160, scType); }
        twain_container_type::value_type get_noisefilter_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1148, scType); }
        twain_container_type::value_type get_orientation_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1110, scType); }
        twain_container_type::value_type get_overscan_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1149, scType); }
        twain_container_type::value_type get_patchcodedetectionenabled_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x113f, scType); }
        twain_container_type::value_type get_patchcodemaxretries_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1144, scType); }
        twain_container_type::value_type get_patchcodemaxsearchpriorities_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1141, scType); }
        twain_container_type::value_type get_patchcodesearchmode_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1143, scType); }
        twain_container_type::value_type get_patchcodesearchpriorities_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1142, scType); }
        twain_container_type::value_type get_patchcodetimeout_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1145, scType); }
        twain_container_type::value_type get_physicalheight_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1112, scType); }
        twain_container_type::value_type get_physicalwidth_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1111, scType); }
        twain_container_type::value_type get_pixelflavor_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x111f, scType); }
        twain_container_type::value_type get_pixelflavorcodes_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1127, scType); }
        twain_container_type::value_type get_pixeltype_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x0101, scType); }
        twain_container_type::value_type get_planarchunky_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1120, scType); }
        twain_container_type::value_type get_rotation_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1121, scType); }
        twain_container_type::value_type get_shadow_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1113, scType); }
        twain_container_type::value_type get_supportedbarcodetypes_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1138, scType); }
        twain_container_type::value_type get_supportedextimageinfo_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x115e, scType); }
        twain_container_type::value_type get_supportedpatchcodetypes_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1140, scType); }
        twain_container_type::value_type get_supportedsizes_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1122, scType); }
        twain_container_type::value_type get_threshold_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1123, scType); }
        twain_container_type::value_type get_tiles_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x111b, scType); }
        twain_container_type::value_type get_timefill_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x112a, scType); }
        twain_container_type::value_type get_undefinedimagesize_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x112d, scType); }
        twain_container_type::value_type get_units_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x0102, scType); }
        twain_container_type::value_type get_image_xfermech_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x0103, scType); }
        twain_container_type::value_type get_xnativeresolution_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1116, scType); }
        twain_container_type::value_type get_xresolution_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1118, scType); }
        twain_container_type::value_type get_xscaling_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1124, scType); }
        twain_container_type::value_type get_ynativeresolution_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1117, scType); }
        twain_container_type::value_type get_yresolution_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1119, scType); }
        twain_container_type::value_type get_yscaling_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x1125, scType); }
        twain_container_type::value_type get_zoomfactor_container_type(const setcap_operation_info& scType) { return get_cap_container_type(0x113e, scType); }

//        #include <dynarithmic/twain/capability_interface/generated_capfuncs.ipp>
    };

    class capability_listener
    {
        public:
            virtual void capvaluegetstart(twain_source&, int, const capability_interface::getcap_operation_info&) {}
            virtual void capvaluegetend(twain_source&, int, const capability_interface::getcap_operation_info&, bool) {}
            virtual void capvaluesetstart(twain_source&, int, const capability_interface::setcap_operation_info&) {}
            virtual void capvaluesetend(twain_source&, int, const capability_interface::setcap_operation_info&, bool) {}
            virtual ~capability_listener() {}
    };

    class extendedimagecap_handler
    {
        bool m_bStarted;
        DTWAIN_SOURCE m_pSource;
        mutable capability_interface::cap_return_type m_return_type;

    public:
        extendedimagecap_handler();
        extendedimagecap_handler(twain_source& ts);
        void start(twain_source& ts);

    #define DTWAIN_EICAPGETTER_FN(a,b) template <typename ValueType=a##_extended_::value_type> \
                                                         ValueType get_##b() const { \
                                                         return get_cap_value<ValueType>(a).first; }

        template <typename ValueType>
        std::pair<ValueType, LONG> get_cap_value(int capvalue) const
        {
            m_return_type = { true, DTWAIN_NO_ERROR };
            twain_array ta;
            bool retval = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource, capvalue, ta.get_array_ptr());
            if (!retval)
            {
                const auto lastError = API_INSTANCE DTWAIN_GetLastError();
                m_return_type = { false, lastError };
                return { ValueType(), lastError };
            }

            std::vector<ValueType> C;
            twain_array_copy_traits::copy_from_twain_array(ta, 1, C);

            if (!C.empty())
                return { C.front(), DTWAIN_NO_ERROR };
            m_return_type = { false, DTWAIN_ERR_BAD_CAP };
            return{ ValueType(), DTWAIN_ERR_BAD_CAP };
        }

        capability_interface::cap_return_type get_last_error() const { return m_return_type; }

            DTWAIN_EICAPGETTER_FN(TWEI_BARCODECONFIDENCE, barcodeconfidence)
            DTWAIN_EICAPGETTER_FN(TWEI_BARCODECOUNT, barcodecount)
            DTWAIN_EICAPGETTER_FN(TWEI_BARCODEROTATION, barcoderotation)
            DTWAIN_EICAPGETTER_FN(TWEI_BARCODETEXT, barcodetext)
            DTWAIN_EICAPGETTER_FN(TWEI_BARCODETEXTLENGTH, barcodetextlength)
            DTWAIN_EICAPGETTER_FN(TWEI_BARCODETYPE, barcodetype)
            DTWAIN_EICAPGETTER_FN(TWEI_BARCODEX, barcodex)
            DTWAIN_EICAPGETTER_FN(TWEI_BARCODEY, barcodey)
            DTWAIN_EICAPGETTER_FN(TWEI_BLACKSPECKLESREMOVED, blackspecklesremoved)
            DTWAIN_EICAPGETTER_FN(TWEI_BOOKNAME, bookname)
            DTWAIN_EICAPGETTER_FN(TWEI_CAMERA, camera)
            DTWAIN_EICAPGETTER_FN(TWEI_CHAPTERNUMBER, chapternumber)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEBLACKCOUNTNEW, deshadeblackcountnew)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEBLACKCOUNTOLD, deshadeblackcountold)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEBLACKRLMAX, deshadeblackrlmax)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEBLACKRLMIN, deshadeblackrlmin)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADECOUNT, deshadecount)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEHEIGHT, deshadeheight)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADELEFT, deshadeleft)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADESIZE, deshadesize)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADETOP, deshadetop)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEWHITECOUNTNEW, deshadewhitecountnew)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEWHITECOUNTOLD, deshadewhitecountold)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEWHITERLAVE, deshadewhiterlave)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEWHITERLMAX, deshadewhiterlmax)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEWHITERLMIN, deshadewhiterlmin)
            DTWAIN_EICAPGETTER_FN(TWEI_DESHADEWIDTH, deshadewidth)
            DTWAIN_EICAPGETTER_FN(TWEI_DESKEWSTATUS, deskewstatus)
            DTWAIN_EICAPGETTER_FN(TWEI_DOCUMENTNUMBER, documentnumber)
            DTWAIN_EICAPGETTER_FN(TWEI_ENDORSEDTEXT, endorsedtext)
            DTWAIN_EICAPGETTER_FN(TWEI_FILESYSTEMSOURCE, filesystemsource)
            DTWAIN_EICAPGETTER_FN(TWEI_FORMCONFIDENCE, formconfidence)
            DTWAIN_EICAPGETTER_FN(TWEI_FORMHORZDOCOFFSET, formhorzdocoffset)
            DTWAIN_EICAPGETTER_FN(TWEI_FORMTEMPLATEMATCH, formtemplatematch)
            DTWAIN_EICAPGETTER_FN(TWEI_FORMTEMPLATEPAGEMATCH, formtemplatepagematch)
            DTWAIN_EICAPGETTER_FN(TWEI_FORMVERTDOCOFFSET, formvertdocoffset)
            DTWAIN_EICAPGETTER_FN(TWEI_FRAME, frame)
            DTWAIN_EICAPGETTER_FN(TWEI_FRAMENUMBER, framenumber)
            DTWAIN_EICAPGETTER_FN(TWEI_HORZLINECOUNT, horzlinecount)
            DTWAIN_EICAPGETTER_FN(TWEI_HORZLINELENGTH, horzlinelength)
            DTWAIN_EICAPGETTER_FN(TWEI_HORZLINETHICKNESS, horzlinethickness)
            DTWAIN_EICAPGETTER_FN(TWEI_HORZLINEXCOORD, horzlinexcoord)
            DTWAIN_EICAPGETTER_FN(TWEI_HORZLINEYCOORD, horzlineycoord)
            DTWAIN_EICAPGETTER_FN(TWEI_ICCPROFILE, iccprofile)
            DTWAIN_EICAPGETTER_FN(TWEI_IMAGEMERGED, imagemerged)
            DTWAIN_EICAPGETTER_FN(TWEI_LASTSEGMENT, lastsegment)
            DTWAIN_EICAPGETTER_FN(TWEI_MAGDATA, magdata)
            DTWAIN_EICAPGETTER_FN(TWEI_MAGDATALENGTH, magdatalength)
            DTWAIN_EICAPGETTER_FN(TWEI_MAGTYPE, magtype)
            DTWAIN_EICAPGETTER_FN(TWEI_PAGENUMBER, pagenumber)
            DTWAIN_EICAPGETTER_FN(TWEI_PAGESIDE, pageside)
            DTWAIN_EICAPGETTER_FN(TWEI_PAPERCOUNT, papercount)
            DTWAIN_EICAPGETTER_FN(TWEI_PATCHCODE, patchcode)
            DTWAIN_EICAPGETTER_FN(TWEI_PIXELFLAVOR, pixelflavor)
            DTWAIN_EICAPGETTER_FN(TWEI_PRINTERTEXT, printertext)
            DTWAIN_EICAPGETTER_FN(TWEI_SEGMENTNUMBER, segmentnumber)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWCONFIDENCE, skewconfidence)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWFINALANGLE, skewfinalangle)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWORIGINALANGLE, skeworiginalangle)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWX1, skewwindowx1)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWX2, skewwindowx2)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWX3, skewwindowx3)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWX4, skewwindowx4)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWY1, skewwindowy1)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWY2, skewwindowy2)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWY3, skewwindowy3)
            DTWAIN_EICAPGETTER_FN(TWEI_SKEWWINDOWY4, skewwindowy4)
            DTWAIN_EICAPGETTER_FN(TWEI_SPECKLESREMOVED, specklesremoved)
            DTWAIN_EICAPGETTER_FN(TWEI_TWAINDIRECTMETADATA, twaindirectmetadata)
            DTWAIN_EICAPGETTER_FN(TWEI_VERTLINECOUNT, vertlinecount)
            DTWAIN_EICAPGETTER_FN(TWEI_VERTLINELENGTH, vertlinelength)
            DTWAIN_EICAPGETTER_FN(TWEI_VERTLINETHICKNESS, vertlinethickness)
            DTWAIN_EICAPGETTER_FN(TWEI_VERTLINEXCOORD, vertlinexcoord)
            DTWAIN_EICAPGETTER_FN(TWEI_VERTLINEYCOORD, vertlineycoord)
            DTWAIN_EICAPGETTER_FN(TWEI_WHITESPECKLESREMOVED, whitespecklesremoved)

            ~extendedimagecap_handler();
    };

    struct twain_variant_visitor
    {
        virtual ~twain_variant_visitor() {}
        virtual void operator()(std::vector<bool>& ) { }
        virtual void operator()(std::vector<int8_t>& ) { }
        virtual void operator()(std::vector<int16_t>& ) {}
        virtual void operator()(std::vector<int32_t>& ) {}
        virtual void operator()(std::vector<uint8_t>& ) {}
        virtual void operator()(std::vector<uint16_t>& ){}
        virtual void operator()(std::vector<uint32_t>& ) {}
        virtual void operator()(std::vector<int64_t>& ) {}
        virtual void operator()(std::vector<uint64_t>& ) {}
        virtual void operator()(std::vector<std::string>& ) {}
        virtual void operator()(std::vector<double>& ) {}
        virtual void operator()(std::vector<twain_frame<double>>& ) {}
        virtual void operator()(std::vector<long>& ) {}
    };

    template <typename T, typename... Ts>
    inline std::unique_ptr<twain_variant_visitor> make_twain_visitor(Ts... ts)
    {
        std::unique_ptr<T> pVisitor = std::make_unique<T>(ts...);
        return std::move(pVisitor);
    }

    template <typename T>
    inline std::unique_ptr<twain_variant_visitor> make_twain_visitor()
    {
        std::unique_ptr<T> pVisitor = std::make_unique<T>();
        return std::move(pVisitor);
    }
} // namespace twain  
} // namespace dynarithmic
#endif
