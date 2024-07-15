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

#ifndef DTWAIN_IMAGEPARAMETER_OPTIONS_HPP
#define DTWAIN_IMAGEPARAMETER_OPTIONS_HPP

#include <vector>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        class imageparameter_options
        {
            friend class options_base;
            bool m_bThumbnailsEnabled;
            bool m_bAutoBright;
            double m_Brightness;
            double m_Contrast;
            double m_Highlight;
            std::vector<capability_type::imagedataset_type> m_vImageDataSets;
            mirror_value::value_type m_MirrorValue;
            orientation_value::value_type m_OrientationValue;
            double m_RotationValue;
            double m_ShadowValue;
            double m_xScaling;
            double m_yScaling;
    
            public:
                imageparameter_options() : m_bThumbnailsEnabled(false), m_bAutoBright(false),
                                            m_Brightness(0), m_Contrast(0), m_Highlight(255),
                                            m_MirrorValue(mirror_value::none),
                                            m_OrientationValue(orientation_value::portrait),
                                            m_RotationValue(0), m_ShadowValue(0),
                                            m_xScaling(1.0), m_yScaling(1.0) {}

                imageparameter_options& enable_thumbnails(bool bEnable)
                { m_bThumbnailsEnabled = bEnable; return *this; }

                imageparameter_options& enable_autobright(bool bEnable)
                { m_bAutoBright = bEnable; return *this; }

                imageparameter_options& set_brightness(double val)
                { m_Brightness = val; return *this; }

                imageparameter_options& set_contrast(double val)
                { m_Contrast = val; return *this; }

                imageparameter_options& set_highlight(double val)
                { m_Highlight = val; return *this; }

                imageparameter_options& set_mirror(mirror_value::value_type mv)
                { m_MirrorValue = mv; return *this; }

                imageparameter_options& set_orientation(orientation_value::value_type ov)
                { m_OrientationValue = ov; return *this; }

                imageparameter_options& set_rotation(double val)
                { m_RotationValue = val; return *this; }

                imageparameter_options& set_shadow(double val)
                { m_ShadowValue = val; return *this; }

                imageparameter_options& set_xscaling(double val)
                { m_xScaling = val; return *this; }

                imageparameter_options& set_yscaling(double val)
                { m_yScaling = val; return *this; }

                bool is_thumbnails_enabled() const { return m_bThumbnailsEnabled; }
                bool is_autobright_enabled() const { return m_bAutoBright; }
                double get_brightness() const { return m_Brightness; }
                double get_contrast() const { return m_Contrast; }
                double get_highlight() const { return m_Highlight; }

                mirror_value::value_type get_mirror() const { return m_MirrorValue; }
                orientation_value::value_type get_orientation() const { return m_OrientationValue; }
                double get_rotation() const { return m_RotationValue; }
                double get_shadow() const { return m_ShadowValue; }
                double get_xscaling() const { return m_xScaling; }
                double get_yscaling() const { return m_yScaling; }

                std::vector<capability_type::imagedataset_type> get_imagedatasets() const { return m_vImageDataSets; }

                static const std::array<uint16_t, 12>& get_affected_caps()
                {
                    static std::array<uint16_t, 12> affected_caps = { CAP_THUMBNAILSENABLED,
                                                                        ICAP_AUTOBRIGHT,
                                                                        ICAP_BRIGHTNESS,
                                                                        ICAP_CONTRAST,
                                                                        ICAP_HIGHLIGHT,
                                                                        ICAP_IMAGEDATASET,
                                                                        ICAP_MIRROR,
                                                                        ICAP_ORIENTATION,
                                                                        ICAP_ROTATION,
                                                                        ICAP_SHADOW,
                                                                        ICAP_XSCALING,
                                                                        ICAP_YSCALING };
                        return affected_caps;
                 }

        };
    }
}
#endif
