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
#ifndef DTWAIN_TWAIN_LOOP_HPP
#define DTWAIN_TWAIN_LOOP_HPP

#include <dynarithmic/twain/source/twain_source_base.hpp>
#include <dtwain.h>

namespace dynarithmic {
namespace twain {

    struct twain_default_enter_dispatch
    {
        bool enter_dispatch(twain_source_base&) { return true; }
    };

    // This can be used for TWAIN 1.x and 2.x Data Source Managers
    template <typename dispatcher = twain_default_enter_dispatch>
    struct twain_looper_win32
    {
        twain_looper_win32(twain_source_base&) {}
        dispatcher m_dispatcher;

        void perform_loop(twain_source_base& ts)
        {
            MSG msg;
            while (GetMessage(&msg, NULL, 0, 0))
            {
                if (!twain_loop<twain_looper_win32>::is_source_open(ts))
                    break;
                if (!DTWAIN_IsTwainMsg(&msg) && m_dispatcher.enter_dispatch(ts))
                {
                    ::TranslateMessage(&msg);
                    ::DispatchMessage(&msg);
                }
            }

            if (ts.is_uionlyenabled() && DTWAIN_GetTwainMode() == DTWAIN_MODELESS)
                ts.set_uionlyenabled(false);
        }
    };

    // This version should only be used for version 2.x and higher TWAIN Data Source Manager
    template <typename dispatcher = twain_default_enter_dispatch>
    struct twain_looper_nowin32
    {
        dispatcher m_dispatcher;

        void perform_loop(twain_source_base& ts)
        {
            MSG msg = {};
            while (twain_loop<twain_looper_nowin32>::is_source_open(ts))
                DTWAIN_IsTwainMsg(&msg);
            if (ts.is_uionlyenabled() && DTWAIN_GetTwainMode() == DTWAIN_MODELESS)
                ts.set_uionlyenabled(false);
        }
    };

    template <typename looper>
    class twain_loop
    {
        public:
            static bool is_source_open(twain_source_base& ts)
            {
                if (ts.is_uionlyenabled())
                    return ts.is_uienabled();
                return ts.is_acquiring();
            }

        public:
            void perform_loop(twain_source_base& ts) 
            {
                looper(ts).perform_loop(ts);
            }
    };
        
    typedef twain_loop<twain_looper_win32<>> twain_loop_windows;
    typedef twain_loop<twain_looper_nowin32<>> twain_loop_ver2;

/*    void TwainMessageLoopWindowsImpl::PerformMessageLoop(CTL_ITwainSource* pSource, bool isUIOnly)
    {
        MSG msg;
        struct UIScopedRAII
        {
            CTL_ITwainSource* m_pSource;
            bool m_bOld;
            UIScopedRAII(CTL_ITwainSource* pSource) : m_pSource(pSource), m_bOld(m_pSource->IsUIOnly()) {}
            ~UIScopedRAII() { m_pSource->SetUIOnly(m_bOld); }
        };

        UIScopedRAII raii(pSource);
        pSource->SetUIOnly(isUIOnly);
#ifdef WIN32
        while (GetMessage(&msg, NULL, 0, 0))
        {
            if (!IsSourceOpen(pSource, isUIOnly))
                break;
            if (CanEnterDispatch(&msg))
            {
                ::TranslateMessage(&msg);
                ::DispatchMessage(&msg);
            }
        }
#else
        while (IsSourceOpen(pSource, isUIOnly))
        {
            CanEnterDispatch(&msg);
        }
#endif
    }*/
}
}
#endif
