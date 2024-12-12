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
#ifndef DTWAIN_DEVICEPARAMS_OPTIONS_HPP
#define DTWAIN_DEVICEPARAMS_OPTIONS_HPP

#include <array>

#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/types/twain_capbasics.hpp>
namespace dynarithmic
{
    namespace twain
    {
        class deviceparams_options
        {
            friend class options_base;
            double m_ExposureTime;
            flashused_value::value_type m_FlashUsed;
            imagefilter_value::value_type m_ImageFilter;
            lightpath_value::value_type m_lightpath;
            filmtype_value::value_type m_filmType;
            lightsource_value::value_type m_lightsource;
            noisefilter_value::value_type m_noisefilter;
            overscan_value::value_type m_overscan;
            units_value::value_type m_unitvalue;
            capability_type::zoomfactor_type m_zoomFactor;

            public:
                deviceparams_options() : m_ExposureTime((std::numeric_limits<double>::min)()), m_FlashUsed(flashused_value::default_val),
                    m_ImageFilter(imagefilter_value::default_val), m_lightpath(lightpath_value::default_val),
                    m_filmType(filmtype_value::default_val), m_lightsource(lightsource_value::default_val),
                    m_noisefilter(noisefilter_value::default_val), m_overscan(overscan_value::default_val),
                    m_unitvalue(units_value::default_val),
                    m_zoomFactor((std::numeric_limits<capability_type::zoomfactor_type>::min)())  {}

                deviceparams_options& set_exposuretime(double exptime)
                { m_ExposureTime = exptime; return *this; }

                deviceparams_options& set_flashused(flashused_value::value_type val)
                { m_FlashUsed = val; return *this; }

                deviceparams_options& set_imagefilter(imagefilter_value::value_type imgfilter)
                { m_ImageFilter = imgfilter; return *this; }

                deviceparams_options& set_lightpath(lightpath_value::value_type lpvalue)
                { m_lightpath = lpvalue; return *this; }

                deviceparams_options& set_filmtype(filmtype_value::value_type ftype)
                { m_filmType = ftype; return *this; }

                deviceparams_options& set_lightsource(lightsource_value::value_type lsource)
                { m_lightsource = lsource; return *this; }

                deviceparams_options& set_noisefilter(noisefilter_value::value_type nfilter)
                { m_noisefilter = nfilter; return *this; }

                deviceparams_options& set_overscan(overscan_value::value_type ovalue)
                { m_overscan = ovalue; return *this; }

                deviceparams_options& set_units(units_value::value_type uvalue)
                { m_unitvalue = uvalue; return *this; }

                deviceparams_options& set_zoomfactor(capability_type::zoomfactor_type zoomFactor)
                { m_zoomFactor = zoomFactor; return *this; }

                double get_exposuretime() const { return m_ExposureTime; }
                flashused_value::value_type get_flashused() const { return m_FlashUsed; }
                imagefilter_value::value_type get_imagefilter() const { return m_ImageFilter; }
                lightpath_value::value_type get_lightpath() const { return m_lightpath; }
                filmtype_value::value_type get_filmtype() const { return m_filmType; }
                lightsource_value::value_type get_lightsource() const { return m_lightsource; }
                noisefilter_value::value_type get_noisefilter() const { return m_noisefilter; }
                overscan_value::value_type get_overscan() const { return m_overscan; }
                units_value::value_type get_units() const { return m_unitvalue; }
                capability_type::zoomfactor_type get_zoomfactor() const { return m_zoomFactor; }

                static const std::array<uint16_t, 17>& get_affected_caps()
                {
                    static std::array<uint16_t, 17> affected_caps = {   CAP_DEVICEONLINE,
                                                                        CAP_DEVICETIMEDATE,
                                                                        CAP_SERIALNUMBER,
                                                                        ICAP_MINIMUMHEIGHT,
                                                                        ICAP_MINIMUMWIDTH,
                                                                        ICAP_EXPOSURETIME,
                                                                        ICAP_FLASHUSED2,
                                                                        ICAP_IMAGEFILTER,
                                                                        ICAP_LAMPSTATE,
                                                                        ICAP_LIGHTPATH,
                                                                        ICAP_LIGHTSOURCE,
                                                                        ICAP_NOISEFILTER,
                                                                        ICAP_OVERSCAN,
                                                                        ICAP_PHYSICALHEIGHT,
                                                                        ICAP_PHYSICALWIDTH,
                                                                        ICAP_UNITS,
                                                                        ICAP_ZOOMFACTOR };
                    return affected_caps;
                }
        };
    }
}
#endif
