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
// This class is used to listen for TWAIN events when an acquisition is occurring.  Use this to do special
// processing on acquisition startup, UI opening and closing, etc.  See the DTWAIN help manual for a list of
// the various events.

#ifndef DTWAIN_CPP_NOIMPORTLIB
    #include <dtwain.h>
#else
    #include <dtwainx2.h>
#endif
#include <dynarithmic/twain/types/twain_callback.hpp>
#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/source/twain_source.hpp>
namespace dynarithmic
{
    namespace twain
    {
        LRESULT twain_callback::call_func(WPARAM wParm, LPARAM lParm, twain_source* pSource)
        {
            // Always called when handler starts
            int status = 0;
            m_nNotificationID = static_cast<LONG>(wParm);
            if (!starthandler(*pSource, wParm, lParm, status))
                return status;

            const auto it = m_functionMap.find(static_cast<LONG>(wParm));
            if (it != m_functionMap.end())
                return (this->*((*it).second))(*pSource);
            return defaulthandler(*pSource, wParm, lParm, m_UserData);

        }
    }
}
