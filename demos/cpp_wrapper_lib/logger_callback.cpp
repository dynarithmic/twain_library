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

#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/session/twain_session.hpp>
#include <dynarithmic/twain/logging/logger_callback.hpp>

namespace dynarithmic
{
    namespace twain
    {
        LRESULT CALLBACK logger_callback_proc(const char* msg, DTWAIN_LONG64 UserData)
        {
            const auto thisObject = reinterpret_cast<twain_session*>(UserData);
            if (thisObject)
            {
                twain_session::logger_type& sesObject = thisObject->get_logger_type();
                if (sesObject.second && sesObject.second->is_enabled())
                {
                    const auto& fn = sesObject.second;
                    if (static_cast<int>(fn->get_destination_aslong()) & static_cast<int>(logger_destination::to_custom))
                        fn->log(msg);
                }
            }
            return 1;
        }
    }
}
