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
// timer used for document feeder access
#ifndef DTWAIN_TWAIN_TIMER_HPP
#define DTWAIN_TWAIN_TIMER_HPP

#include <chrono>

namespace dynarithmic
{
    namespace twain
    {
        class twain_timer
        {
        public:
            twain_timer() : beg_(clock_::now()) {}
            void reset() { beg_ = clock_::now(); }
            double elapsed() const {
                return std::chrono::duration_cast<second_>
                    (clock_::now() - beg_).count();
            }

        private:
            typedef std::chrono::high_resolution_clock clock_;
            typedef std::chrono::duration<double, std::ratio<1> > second_;
            std::chrono::time_point<clock_> beg_;
        };
    }
}
#endif
