/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2022 Dynarithmic Software.

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
#ifndef DTWAIN_COMPRESSION_OPTIONS_HPP
#define DTWAIN_COMPRESSION_OPTIONS_HPP

#include <vector>
#include <array>

#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/capability_interface.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class compression_options
        {
            friend class options_base;
            bitorder_value::value_type m_BitOrderValue;
            capability_type::ccittkfactor_type m_CCITKFactor;
            compression_value::value_type m_CompressionValue;
            jpegpixel_value::value_type m_JpegPixelType;
            jpegquality_value::value_type m_JpegQuality;
            jpegsubsampling_value::value_type m_JpegSubSampleValue;
            pixelflavor_value::value_type m_PixelFlavor;
            capability_type::timefill_type m_TimeFill;

            public:
                compression_options() :  m_BitOrderValue(bitorder_value::msbfirst),
                                        m_CCITKFactor(4),
                                        m_CompressionValue(compression_value::none),
                                        m_JpegPixelType(jpegpixel_value::default_val),
                                        m_JpegQuality(75),
                                        m_JpegSubSampleValue(jpegsubsampling_value::default_val),
                                        m_PixelFlavor(pixelflavor_value::chocolate),
                                        m_TimeFill(1) {}

                compression_options& set_bitordercodes(bitorder_value::value_type val) 
                { m_BitOrderValue = val; return *this; }

                compression_options& set_ccittkfactor(capability_type::ccittkfactor_type val) 
                { m_CCITKFactor = val; return *this; }

                compression_options& set_compression(compression_value::value_type cv)
                { m_CompressionValue = cv; return *this; }

                compression_options& set_jpegpixeltype(jpegpixel_value::value_type jv)
                { m_JpegPixelType = jv; return *this; }

                compression_options& set_jpegquality(jpegquality_value::value_type jv)
                { m_JpegQuality = static_cast<uint16_t>(jv); return *this; }

                compression_options& set_jpegsubsampling(jpegsubsampling_value::value_type jv)
                { m_JpegSubSampleValue = jv; return *this; }

                compression_options& set_pixelflavorcodes(pixelflavor_value::value_type pv)
                { m_PixelFlavor = pv; return *this; }

                compression_options& set_timefill(capability_type::timefill_type timefill)
                { m_TimeFill = timefill; return *this; }

                bitorder_value::value_type get_bitordercodes() const { return m_BitOrderValue; }
                capability_type::ccittkfactor_type get_ccittkfactor() const { return m_CCITKFactor; }
                compression_value::value_type get_compression() const { return m_CompressionValue; }
                jpegpixel_value::value_type get_jpegpixeltype() const { return m_JpegPixelType; }
                jpegquality_value::value_type get_jpegquality() const { return m_JpegQuality; }
                jpegsubsampling_value::value_type get_jpegsubsampling() const { return m_JpegSubSampleValue; }
                pixelflavor_value::value_type get_pixelflavorcodes() const { return m_PixelFlavor; }
                capability_type::timefill_type get_timefill() const { return m_TimeFill; }

                const std::array<uint16_t, 8>& get_affected_caps()
                {
                    static std::array<uint16_t, 8> affected_caps = { ICAP_BITORDERCODES,
                                                                    ICAP_CCITTKFACTOR,
                                                                    ICAP_COMPRESSION,
                                                                    ICAP_JPEGPIXELTYPE,
                                                                    ICAP_JPEGQUALITY,
                                                                    ICAP_JPEGSUBSAMPLING,
                                                                    ICAP_PIXELFLAVORCODES,
                                                                    ICAP_TIMEFILL};
                    return affected_caps;
                }
        };
    }
}
#endif
