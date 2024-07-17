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
#ifndef DTWAIN_AUTOADJUST_OPTIONS_HPP
#define DTWAIN_AUTOADJUST_OPTIONS_HPP

#include <vector>
#include <climits>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        /// The autoadjust_options describes the options used by the TWAIN device that can be used to
        /// automatically adjust the color, skew, rotation, size, etc.
        /// 
        /// The autoadjust_options are described in the "TWAIN Specification 2.4", Chapter 10, Section 3.\n
        /// The Specification is available here: https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
        /// 
        class autoadjust_options
        {
            public:
                using threshold_type = double;
                static constexpr double disable_threshold = (std::numeric_limits<threshold_type>::min)();
            private:
                friend class options_base;
                bool m_bSenseMedium;
                capability_type::autodiscardblankpages_type m_bDiscardBlankPages;
                bool m_bBorderDetection;
                bool m_bColorEnabled;
                color_value::value_type m_ColorNonColorPixelType;
                bool m_bDeskew;
                bool m_bLengthDetection;
                bool m_bRotate;
                autosize_value::value_type m_AutoSize;
                fliprotation_value::value_type m_FlipRotation;
                imagemerge_value::value_type m_ImageMerge;
                threshold_type  m_ImageMergeHeightThreshold;

            public:

                autoadjust_options() : m_bSenseMedium(false),
                                        m_bDiscardBlankPages(TWBP_DISABLE),
                                        m_bBorderDetection(false),
                                        m_bColorEnabled(false),
                                        m_ColorNonColorPixelType(color_value::default_color),
                                        m_bDeskew(false),
                                        m_bLengthDetection(false),
                                        m_bRotate(false),
                                        m_AutoSize(autosize_value::none),
                                        m_FlipRotation(fliprotation_value::default_flip),
                                        m_ImageMerge(imagemerge_value::none),
                                        m_ImageMergeHeightThreshold(disable_threshold)
                {}

                autoadjust_options& enable_sensemedium(bool bSet) noexcept
                {m_bSenseMedium = bSet; return *this;}

                autoadjust_options& set_discardblankpages(int32_t num) noexcept
                {m_bDiscardBlankPages = num; return *this; }

                autoadjust_options& enable_borderdetection(bool bSet) noexcept
                {m_bBorderDetection = bSet; return *this; }

                autoadjust_options& enable_colorenabled(bool bSet) noexcept
                {m_bColorEnabled = bSet; return *this; }

                autoadjust_options& set_noncolorpixeltype(color_value::value_type ct) noexcept
                {m_ColorNonColorPixelType = ct; return *this; }

                autoadjust_options& enable_deskew(bool bSet) noexcept
                {m_bDeskew = bSet; return *this; }

                autoadjust_options& enable_lengthdetection(bool bSet) noexcept
                {m_bLengthDetection = bSet; return *this; }

                autoadjust_options& enable_rotate(bool bSet) noexcept
                {m_bRotate = bSet; return *this; }

                autoadjust_options& set_autosize(autosize_value::value_type ao) noexcept
                {m_AutoSize = ao; return *this; }

                autoadjust_options& set_fliprotation(fliprotation_value::value_type fr) noexcept
                {m_FlipRotation = fr; return *this; }

                autoadjust_options& set_imagemerge(imagemerge_value::value_type io) noexcept
                {m_ImageMerge = io; return *this; }

                autoadjust_options& set_mergeheightthreshold(threshold_type threshold) noexcept
                {m_ImageMergeHeightThreshold = threshold; return *this; }

                bool is_sensemedium_enabled() const  noexcept { return m_bSenseMedium; }
                capability_type::autodiscardblankpages_type get_discardblankpages() const noexcept { return m_bDiscardBlankPages; }
                bool is_borderdetection_enabled() const noexcept { return m_bBorderDetection; }
                bool is_colorenabled_enabled() const noexcept { return m_bColorEnabled; }
                color_value::value_type get_noncolorpixeltype() const noexcept { return m_ColorNonColorPixelType; }
                bool is_deskew_enabled() const noexcept { return m_bDeskew; }
                bool is_lengthdetection_enabled() const noexcept { return m_bLengthDetection; }
                bool is_rotate_enabled() const noexcept { return m_bRotate; }
                autosize_value::value_type get_autosize() const noexcept { return m_AutoSize; }
                fliprotation_value::value_type get_fliprotation() const noexcept { return m_FlipRotation; }
                imagemerge_value::value_type get_imagemerge() const noexcept { return m_ImageMerge; }
                threshold_type get_mergeheightthreshold() const noexcept { return m_ImageMergeHeightThreshold; }

                /// The automatic adjust options are described by the following TWAIN capabilities:\n\n
                /// **CAP_AUTOMATICSENSEMEDIUM**<br> 
                /// **ICAP_AUTODISCARDBLANKPAGES** <br>
                /// **ICAP_AUTOMATICBORDERDETECTION** <br>
                /// **ICAP_AUTOMATICCOLORENABLED**<br>
                /// **ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE**<br>
                /// **ICAP_AUTOMATICCROPUSESFRAME**<br>
                /// **ICAP_AUTOMATICDESKEW**<br>
                /// **ICAP_AUTOMATICLENGTHDETECTION**<br>
                /// **ICAP_AUTOMATICROTATE**<br>
                /// **ICAP_AUTOSIZE**<br>
                /// **ICAP_FLIPROTATION**<br>
                /// **ICAP_IMAGEMERGE**<br>
                /// **ICAP_IMAGEMERGEHEIGHTTHRESHOLD**
                /// @returns a reference to the array of TWAIN capabilities affected by this object
                /// @note Refer to the TWAIN Specification 2.4 Chapter 10:
                /// @note https://github.com/dynarithmic/twain_library/tree/master/TwainSpecification
                static const std::array<uint16_t, 13>& get_affected_caps() noexcept
                {
                    static std::array<uint16_t, 13> affected_caps = { CAP_AUTOMATICSENSEMEDIUM,
                                                                    ICAP_AUTODISCARDBLANKPAGES,
                                                                    ICAP_AUTOMATICBORDERDETECTION,
                                                                    ICAP_AUTOMATICCOLORENABLED,
                                                                    ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE,
                                                                    ICAP_AUTOMATICCROPUSESFRAME,
                                                                    ICAP_AUTOMATICDESKEW,
                                                                    ICAP_AUTOMATICLENGTHDETECTION,
                                                                    ICAP_AUTOMATICROTATE,
                                                                    ICAP_AUTOSIZE,
                                                                    ICAP_FLIPROTATION,
                                                                    ICAP_IMAGEMERGE,
                                                                    ICAP_IMAGEMERGEHEIGHTTHRESHOLD };
                    return affected_caps;
                }
        };
    }
}   
#endif
