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
#include <dynarithmic/twain/info/buffered_transfer_info.hpp>
#include <dynarithmic/twain/dtwain_twain.hpp>
#include <dynarithmic/twain/capability_interface/capability_interface.hpp>
#include <dynarithmic/twain/acquire_characteristics/acquire_characteristics.hpp>
#include <dynarithmic/twain/source/twain_source.hpp>
namespace dynarithmic
{
    namespace twain
    {
        
        buffered_transfer_info::~buffered_transfer_info()
        {
            if (m_hStrip)
                API_INSTANCE DTWAIN_FreeMemory(m_hStrip);
        }

        acquired_strip_data& buffered_transfer_info::get_strip_data()
        {
            API_INSTANCE DTWAIN_GetAcquireStripData(m_twain_source, &m_stripData.Compression, &m_stripData.BytesPerRow,
                &m_stripData.Columns, &m_stripData.Rows, &m_stripData.XOffset,
                &m_stripData.YOffset, &m_stripData.BytesWritten);
            return m_stripData;
        }

        bool buffered_transfer_info::init_transfer(compression_value::value_type compression)
        {
            if (!m_twain_source)
                return false;
            // Check strip size here
            if (m_nStripSize > 0)
            {
                if (m_nStripSize < m_nMinSize || m_nStripSize > m_nMaxSize)
                    return false;
            }

            if (all_compression_types.find(compression) == all_compression_types.end())
                return false;

            if (m_nStripSize > 0)
            {
                // Allocate memory for strip here
                if (m_hStrip)
                    API_INSTANCE DTWAIN_FreeMemory(m_hStrip);

                m_hStrip = API_INSTANCE DTWAIN_AllocateMemory(m_nStripSize);
                if (!m_hStrip)
                    return false;

                if (!API_INSTANCE DTWAIN_SetAcquireStripBuffer(m_twain_source, m_hStrip))
                {
                    API_INSTANCE DTWAIN_FreeMemory(m_hStrip);
                    return false;
                }
            }
            return true;
        }

        void buffered_transfer_info::attach(const twain_source& ts)
        {
            auto& ci = ts.get_capability_interface();
            auto vc = ci.get_cap_values(ICAP_COMPRESSION);
            std::copy(vc.begin(), vc.end(), std::inserter(all_compression_types, all_compression_types.end()));
            API_INSTANCE DTWAIN_GetAcquireStripSizes(ts.get_source(), &m_nMinSize, &m_nMaxSize, &m_nPrefSize);
            m_nStripSize = m_nPrefSize;
            m_twain_source = ts.get_source();
        }
    }
}

