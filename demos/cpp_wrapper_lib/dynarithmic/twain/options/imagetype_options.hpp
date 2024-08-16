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
#ifndef DTWAIN_IMAGETYPE_OPTIONS_HPP
#define DTWAIN_IMAGETYPE_OPTIONS_HPP

#include <vector>
#include <iterator>
#include <string>
#include <array>
#include <algorithm>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class imagetype_options
        {
            friend class options_base;
            uint16_t m_BitDepth;
            color_value::value_type m_PixelType;
            bitdepthreduction_value::value_type m_BitDepthReduction;
            bitorder_value::value_type m_BitOrderValue;
            std::vector<uint8_t> m_vCustHalfTone;
            std::string m_sHalftone;
            pixelflavor_value::value_type m_PixelFlavor;
            double m_Threshold;
            bool m_bNegateImage;
            bool m_bCustomHalfToneEnabled;
            int m_nJPegQuality;

            static double constexpr default_threshold = (std::numeric_limits<double>::max)();
            static int constexpr default_jpegquality = 75;

            public:
                imagetype_options() : m_BitDepth(0),
                                    m_bNegateImage(false),
                                    m_BitDepthReduction(bitdepthreduction_value::default_val),
                                    m_BitOrderValue(bitorder_value::default_val),
                                    m_PixelFlavor(pixelflavor_value::chocolate),
                                    m_PixelType(color_value::default_color),
                                    m_bCustomHalfToneEnabled(false),
                                    m_Threshold(default_threshold),
                                    m_nJPegQuality(m_nJPegQuality) {}

                imagetype_options& enable_customhalftones(bool bEnable = true) { m_bCustomHalfToneEnabled = bEnable; return *this; }
                bool is_customhalftones_enabled() const { return m_bCustomHalfToneEnabled; }

                imagetype_options& set_bitdepth(uint16_t bitdepth) 
                { m_BitDepth = bitdepth; return *this; }

                imagetype_options& set_pixeltype(color_value::value_type val)
                { m_PixelType = val; return *this; }

                imagetype_options& set_bitdepthreduction(bitdepthreduction_value::value_type br)
                { m_BitDepthReduction = br; return *this; }

                imagetype_options& set_bitorder(bitorder_value::value_type bv)
                { m_BitOrderValue = bv; return *this; }

                imagetype_options& set_halftone(const std::string& s) 
                { m_sHalftone = s; return *this; }

                imagetype_options& set_pixelflavor(pixelflavor_value::value_type pf)
                { m_PixelFlavor = pf; return *this; }

                imagetype_options& set_threshold(double val) 
                { m_Threshold = val; return *this; }

                imagetype_options& set_jpegquality(int val)
                { 
                    val = (std::max)(val, 0);
                    val = (std::min)(100, val);
                    m_nJPegQuality = val; 
                    return *this; 
                }

                imagetype_options& enable_negate(bool bSet) { m_bNegateImage = bSet; return *this; }

                uint16_t get_bitdepth() const { return m_BitDepth; }
                color_value::value_type get_pixeltype() const { return m_PixelType;  }
                bitdepthreduction_value::value_type get_bitdepthreduction() const { return m_BitDepthReduction;  }
                bitorder_value::value_type get_bitorder() const { return m_BitOrderValue;  }
                std::vector<uint8_t>& get_customhalftone() { return m_vCustHalfTone; }
                std::string get_halftone() const { return m_sHalftone; }
                pixelflavor_value::value_type get_pixelflavor() const { return m_PixelFlavor; }
                double get_threshold() const { return m_Threshold; }
                bool is_negate_enabled() const { return m_bNegateImage; }
                int get_jpegquality() const { return m_nJPegQuality;  }
                const std::array<uint16_t, 8>& get_affected_caps()
                {
                    static std::array<uint16_t, 8> affected_caps = { ICAP_BITDEPTH,
                                                                    ICAP_BITDEPTHREDUCTION,
                                                                    ICAP_BITORDER,
                                                                    ICAP_CUSTHALFTONE,
                                                                    ICAP_HALFTONES,
                                                                    ICAP_PIXELFLAVOR,
                                                                    ICAP_PIXELTYPE,
                                                                    ICAP_THRESHOLD };
                    return affected_caps;
                }
        };
    }
}
#endif
