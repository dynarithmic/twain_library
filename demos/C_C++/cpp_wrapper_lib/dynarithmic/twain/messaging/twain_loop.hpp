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
#ifndef DTWAIN_TWAIN_LOOP_HPP
#define DTWAIN_TWAIN_LOOP_HPP

#ifdef _WIN32
    #include <windows.h>
#endif
#include <dynarithmic/twain/dtwain_twain.hpp>
#include <dynarithmic/twain/twain_source.hpp>

namespace dynarithmic 
{
    namespace twain 
    {
        struct twain_default_enter_dispatch
        {
            bool enter_dispatch(twain_source&) { return true; }
        };

        template <typename looper>
        class twain_loop
        {
        public:
            static bool is_source_open(twain_source& ts)
            {
                return ts.is_uienabled();
            }

        public:
            void perform_loop(twain_source& ts)
            {
                looper(ts).perform_loop(ts);
            }
        };

        // This can be used for TWAIN 1.x and 2.x Data Source Managers
        template <typename dispatcher=twain_default_enter_dispatch>
        struct twain_looper_win32
        {
            twain_looper_win32(twain_source&) {}
            dispatcher m_dispatcher;

            void perform_loop(twain_source& ts)
            {
                MSG msg;
                while (GetMessage(&msg, NULL, 0, 0))
                {
                    if (!twain_loop<twain_looper_win32>::is_source_open(ts) || !ts.is_uienabled())
                        break;
                    if (!API_INSTANCE DTWAIN_IsTwainMsg(&msg) && m_dispatcher.enter_dispatch(ts))
                    {
                        ::TranslateMessage(&msg);
                        ::DispatchMessage(&msg);
                    }
                }
            }
        };

        // This version should only be used for version 2.x and higher TWAIN Data Source Manager
        template <typename dispatcher=twain_default_enter_dispatch>
        struct twain_looper_nowin32
        {
            dispatcher m_dispatcher;

            void perform_loop(twain_source& ts)
            {
                MSG msg = {};
                while (twain_loop<twain_looper_nowin32>::is_source_open(ts) && ts.is_uienabled())
                    API_INSTANCE DTWAIN_IsTwainMsg(&msg);
            }
        };

        
        typedef twain_loop<twain_looper_win32<>> twain_loop_windows;
        typedef twain_loop<twain_looper_nowin32<>> twain_loop_ver2;
    }
}
#endif
