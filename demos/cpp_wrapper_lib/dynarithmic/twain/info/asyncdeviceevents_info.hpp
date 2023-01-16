/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2020 Dynarithmic Software.

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
#ifndef DTWAIN_ASYNCDEVICEEVENTS_INFO_HPP
#define DTWAIN_ASYNCDEVICEEVENTS_INFO_HPP

#include <vector>

#include "dtwain_capability_interface.hpp"
#include "dtwain_capinfo_base.hpp"
#include "dtwain_twain_values.hpp"

namespace dynarithmic
{
    namespace twain
    {
        class deviceevent_info : public twain_capinfo_base
        {
            std::vector<deviceevent_value::value_type> m_vDeviceEvents;

            protected:
                bool get(capability_interface& capInterface) override 
                { m_vDeviceEvents = capInterface.get_deviceevent(); return true; }

            public:
                std::vector<deviceevent_value::value_type> get_deviceevents() const { return m_vDeviceEvents; }
        };

        deviceevent_info get_deviceevent_info() { return deviceeent_info().get_deviceevents(); }
    }
}
#endif