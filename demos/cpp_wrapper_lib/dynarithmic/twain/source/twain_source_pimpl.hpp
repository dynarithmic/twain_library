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
#ifndef DTWAIN_TWAIN_SOURCE_PIMPL
#define DTWAIN_TWAIN_SOURCE_PIMPL

#include <dynarithmic/twain/acquire_characteristics.hpp>
#include <dynarithmic/twain/capability_interface.hpp>
#include <dynarithmic/twain/info/buffered_transfer_info.hpp>
#include <dynarithmic/twain/info/file_transfer_info.hpp>
#include <dynarithmic/twain/capability_interface/capability_interface.hpp>

namespace dynarithmic 
{
    namespace twain 
    {
        class twain_source_pimpl
        {
            public:
                std::unique_ptr<acquire_characteristics>      m_acquire_characteristics;
                std::unique_ptr<buffered_transfer_info>       m_buffered_info;
                std::unique_ptr<file_transfer_info>           m_filetransfer_info;
                std::unique_ptr<capability_listener>          m_capability_listener;
                mutable std::unique_ptr<capability_interface> m_capability_info;
        };
    }
}
#endif
