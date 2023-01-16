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
#ifndef DTWAIN_CAPINFO_BASE_HPP
#define DTWAIN_CAPINFO_BASE_HPP

namespace dynarithmic
{
    namespace twain
    {
        class capability_interface;
        class twain_capinfo_base
        {
            bool m_bCacheResults;
            protected:
                bool get(capability_interface& capInterface, bool bRefresh)
                {
                    if (bRefresh || !is_cached())
                        return get(capInterface);
                    return true;
                }

                virtual bool get(capability_interface& capInterface) = 0;

            public:
                twain_capinfo_base() : m_bCacheResults(false) {}
                virtual ~twain_capinfo_base() = default;
                void set_cached(bool bSet = true) { m_bCacheResults = bSet; }
                bool is_cached() const { return m_bCacheResults; }
        };
    }
}   
#endif
