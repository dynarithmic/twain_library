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
#ifndef DTWAIN_BLANKPAGE_OPTIONS_HPP
#define DTWAIN_BLANKPAGE_OPTIONS_HPP

#include <vector>
#include <iterator>
#include <algorithm>
#include <array>
#include <dynarithmic/twain/twain_values.hpp>

namespace dynarithmic
{
    namespace twain
    {
        enum class blankpage_discard_option
        {
            discard_on_notification = DTWAIN_BP_AUTODISCARD_NONE,
            discard_all = DTWAIN_BP_AUTODISCARD_ANY,
            discard_all_from_device = DTWAIN_BP_AUTODISCARD_IMMEDIATE,
            discard_all_after_resampling =  DTWAIN_BP_AUTODISCARD_AFTERPROCESS
        };

        enum class blankpage_detection_option
        {
            detect_original = DTWAIN_BP_DETECTORIGINAL,
            detect_adjusted = DTWAIN_BP_DETECTADJUSTED,
            detect_both = DTWAIN_BP_DETECTORIGINAL | DTWAIN_BP_DETECTADJUSTED
        };

         class blankpage_options
         {
             public:
                 static constexpr double default_blank_page_threshold = 98.0;
                 static constexpr int keep_page = 1;
                 static constexpr int discard_page = 0;      
             private:
                 bool m_bEnabled;
                 double m_threshold = default_blank_page_threshold;
                 blankpage_discard_option discard_option;
                 blankpage_detection_option detection_option;

             public:
                 using blankpage_detection_info = std::pair<bool, double>;
                 blankpage_options() : m_bEnabled(false), 
                     discard_option(blankpage_discard_option::discard_on_notification),
                     detection_option(blankpage_detection_option::detect_both) {}
                 blankpage_options& enable(bool bEnable) { m_bEnabled = bEnable; return *this; }
                 blankpage_options& set_threshold(double threshold) { m_threshold = threshold; return *this; }
                 blankpage_options& set_discard_option(blankpage_discard_option discardOpt) { discard_option = discardOpt; return *this; }
                 blankpage_options& set_detection_option(blankpage_detection_option detectOpt) { detection_option = detectOpt; return *this; }

                 bool is_enabled() const { return m_bEnabled; }
                 double get_threshold() const { return m_threshold; }
                 blankpage_discard_option get_discard_option() const { return discard_option; }
                 blankpage_detection_option get_detection_option() const { return detection_option; }
         };
    }
}
#endif
