/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2026 Dynarithmic Software.

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

#ifndef DTWAIN_TWAIN_FRAME_HPP
#define DTWAIN_TWAIN_FRAME_HPP

#include <limits>
#include <functional>

namespace dynarithmic
{
    namespace twain
    {
        // mimics the TW_FRAME type
        template <typename FrameType = double, 
                  typename CompareEq = std::equal_to<FrameType>>
        struct twain_frame
        {
            typedef FrameType value_type;
            typedef CompareEq comparereq_type;

            FrameType left = (std::numeric_limits<FrameType>::min)();
            FrameType top = (std::numeric_limits<FrameType>::min)();
            FrameType right = (std::numeric_limits<FrameType>::min)();
            FrameType bottom = (std::numeric_limits<FrameType>::min)();
            comparereq_type comparereq;

            twain_frame(const FrameType& singlevalue) : left(singlevalue), top(singlevalue), right(singlevalue),
                                                        bottom(singlevalue)
            {}

            twain_frame(const FrameType& l, const FrameType& t, const FrameType& r, const FrameType& b) : left(l), top(t), right(r), bottom(b)
            {}

            twain_frame() = default;

            bool operator==(const twain_frame& tf) const
            {
                return comparereq(tf.left, left) &&
                       comparereq(tf.top, top) &&
                       comparereq(tf.right, right) &&
                       comparereq(tf.bottom, bottom);
            }

            bool operator!=(const twain_frame& tf) const
            {
                return !(*this == tf);
            }
        };
    }
}   
#endif
