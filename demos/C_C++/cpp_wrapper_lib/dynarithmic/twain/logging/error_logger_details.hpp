/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2020 Dynarithmic Software.

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
#ifndef DTWAIN_TWAIN_ERROR_LOGGER_DETAILS_HPP
#define DTWAIN_TWAIN_ERROR_LOGGER_DETAILS_HPP

#include <cstddef>

namespace dynarithmic
{
    namespace twain
    {
        class error_logger_details
        {
            size_t buffer_size = 50;

        public:
            error_logger_details& set_maxsize(size_t val) { buffer_size = val; return *this; }
            size_t get_maxsize() const { return buffer_size; }
        };
    }
}
#endif
