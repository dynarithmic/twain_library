/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2024 Dynarithmic Software.

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
#ifndef DTWAIN_BUFFERED_TRANSFER_INFO_HPP
#define DTWAIN_BUFFERED_TRANSFER_INFO_HPP

#include <unordered_set>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class twain_source;
        struct acquired_strip_data
        {
            LONG Compression;
            LONG BytesPerRow;
            LONG Columns;
            LONG Rows;
            LONG XOffset;
            LONG YOffset;
            LONG BytesWritten;
            acquired_strip_data() : Compression(0), BytesPerRow(0), Columns(0), 
                                    Rows(0), XOffset(0), YOffset(0), BytesWritten() {}
        };

        class buffered_transfer_info
        {
            private:
                acquired_strip_data m_stripData;
                HANDLE m_hStrip;
                LONG m_nStripSize;
                LONG m_nCurrentStripSize;
                LONG m_nMinSize, m_nMaxSize, m_nPrefSize;
                std::unordered_set<compression_value::value_type> all_compression_types;
                DTWAIN_SOURCE m_twain_source;
            
            public:
                buffered_transfer_info() : m_hStrip(nullptr),
                                            m_nStripSize(0),
                                            m_nCurrentStripSize(0),
                                            m_nMinSize(0),
                                            m_nMaxSize(0),
                                            m_nPrefSize(0),
                                            m_twain_source(nullptr)
                {}

                ~buffered_transfer_info();

                LONG stripsize() const { return m_nStripSize; }
                LONG minstripsize() const { return m_nMinSize; }
                LONG maxstripsize() const { return m_nMaxSize; }
                LONG preferredsize() const { return m_nPrefSize; }
                HANDLE getstrip() const { return m_hStrip; }

                acquired_strip_data& get_strip_data();
                buffered_transfer_info& set_stripsize(long sz) { m_nStripSize = sz; return *this; }
            
                bool init_transfer(compression_value::value_type compression);
                void attach(const twain_source& ts);

                template <typename Container=std::vector<uint16_t>>
                Container get_compression_types() const
                {
                    Container c;
                    std::copy(all_compression_types.begin(), all_compression_types.end(), std::inserter(c, c.end()));
                    return c;
                }

        };
    }
}
#endif
