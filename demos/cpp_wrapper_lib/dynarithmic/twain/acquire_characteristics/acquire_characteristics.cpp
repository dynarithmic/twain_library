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

#include <dynarithmic/twain/acquire_characteristics/acquire_characteristics.hpp>
#include <algorithm>

namespace dynarithmic
{
    namespace twain
    {
        acquire_characteristics::acquire_characteristics()
        {
            static constexpr uint8_t default_appliers[] =
            {
                apply_generaloptions,
                apply_pdfoptions,
                apply_coloroptions,
                apply_filetransferoptions,
                apply_bufferedtransferoptions,
                apply_jobcontroloptions,
                apply_compressionoptions,
                apply_paperhandlingoptions,
                apply_userinterfaceoptions,
                apply_blankpageoptions,
                apply_resolutionoptions,
                apply_imagetypeoptions
            };

            std::fill_n(m_aAppliers.begin(), m_aAppliers.size(), false);
            for (auto n : default_appliers)
                m_aAppliers[n] = true;
        }
    }
}