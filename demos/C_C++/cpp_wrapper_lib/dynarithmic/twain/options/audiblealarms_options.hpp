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
#ifndef DTWAIN_AUDIBLEALARMS_OPTIONS_HPP
#define DTWAIN_AUDIBLEALARMS_OPTIONS_HPP

#include <vector>
#include <array>
#include <algorithm>
#include <climits>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        /// The audiblealarms_options describes the options used by the TWAIN device.
        /// 
        /// The audiblearams_options are described in the "TWAIN Specification 2.4", Chapter 10, Section 3.\n
        /// The Specification is available here: https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
        /// 
        class audiblealarms_options
        {
            public:
                using alarm_volume_type = int32_t;
                using alarm_type = uint16_t;

            private:
                friend class options_base;
                std::vector<alarm_type> m_vAlarms;
                alarm_volume_type m_AlarmVolume;
            
            public:
                static constexpr alarm_type alarm = TWAL_ALARM;
                static constexpr alarm_type feedererror = TWAL_FEEDERERROR;
                static constexpr alarm_type feederwarning = TWAL_FEEDERWARNING;
                static constexpr alarm_type barcode = TWAL_BARCODE;
                static constexpr alarm_type doublefeed = TWAL_DOUBLEFEED;
                static constexpr alarm_type jam = TWAL_JAM;
                static constexpr alarm_type patchcode = TWAL_PATCHCODE;
                static constexpr alarm_type power = TWAL_POWER;
                static constexpr alarm_type skew = TWAL_SKEW;
                static constexpr alarm_volume_type disable_volume = (std::numeric_limits<alarm_volume_type>::min)();

                audiblealarms_options() : m_AlarmVolume(disable_volume) {}
                /// Sets the alarm "volume"
                /// Example:
                /// \code {.cpp}
                /// twain_source source;
                /// //...
                ///         source.get_acquire_characteristics().
                ///                   get_audiblealarm_options().      
                ///                   set_alarmvolume(100);
                /// \endcode
                /// @param[in] value any 32-bit value that represents the alarm's volume
                /// @returns a reference to the current audiblealarms_options object (**this**)
                /// @note To turn off the volume setting, pass disable_volume to this function
                audiblealarms_options& set_volume(alarm_volume_type value) noexcept
                { m_AlarmVolume = value; return *this; }

                /// Gets the alarm volume that will be used
                /// Example:
                /// \code {.cpp}
                /// twain_source source;
                /// //...
                /// auto alarmvolume = source.get_acquire_characteristics().
                ///                     get_audiblealarm_options().      
                ///                     get_alarmvolume();
                /// \endcode
                /// @returns an integer representing the alarm volume.  If no alarm volume is set, disable_volume is returned.
                alarm_volume_type get_volume() const noexcept { return m_AlarmVolume; }

                /// Gets the alarm options that will be used
                /// Example:
                /// \code {.cpp}
                /// twain_source source;
                /// //...
                /// auto all_alarms = source.get_acquire_characteristics().
                ///                   get_audiblealarm_options().      
                ///                   get_alarms();
                /// \endcode
                /// @returns a std::vector<alarm_type>, where each element is an alarms option
                std::vector<uint16_t> get_alarms() const { return m_vAlarms; }

                /// Sets the available alarm options.   
                /// Example:
                /// \code {.cpp}
                /// twain_source source;
                /// //...
                /// source.get_acquire_characteristics().
                ///        get_audiblealarm_options().      
                ///        set_alarms({audiblealarms_options::feedererror, audiblealarms_options::feederwarning});
                /// \endcode
                /// @param[in] alarm_options A std::vector (or suitable container) of alarm options to set.
                /// @returns a reference to the current object (**this**)
                /// @note The container to use must have a value_type of **alarm_type**
                ///       The alarms constants have a prefix of TWAL_.
                template <typename Container=std::vector<alarm_type>>
                audiblealarms_options& set_alarms(const Container& alarm_options)
                {
                    m_vAlarms.clear();
                    std::copy(alarm_options.begin(), alarm_options.end(), std::back_inserter(m_vAlarms));
                    return *this;
                }

                /// The audible alarms are described by the following TWAIN capabilities:\n\n
                /// **CAP_ALARMS**      -- Turns specific audible alarms on and off<br>
                /// **CAP_ALARMVOLUME** -- Controls the volume of a device's audible alarm.
                /// @returns a reference to the array of TWAIN capabilities affected by this object
                /// @note Refer to the TWAIN Specification 2.4 Chapter 10:
                /// @note https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
                static const std::array<uint16_t, 2>& get_affected_caps() noexcept
                {
                    static std::array<uint16_t, 2> affected_caps = { CAP_ALARMS, CAP_ALARMVOLUME };
                    return affected_caps;
                }
        };
    }
}
#endif
