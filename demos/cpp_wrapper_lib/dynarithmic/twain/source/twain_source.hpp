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
#ifndef DTWAIN_TWAIN_SOURCE_HPP
#define DTWAIN_TWAIN_SOURCE_HPP

#include <utility>
#include <unordered_map>
#include <functional>
#include <algorithm>
#include <chrono>
#include <thread>
#include <tuple>
#include <numeric>
#include <string>
#include <vector>
#include <memory>

#include <dtwain.h>
#include <dynarithmic/twain/identity/twain_identity.hpp>
#include <dynarithmic/twain/types/twain_array.hpp>
#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/types/twain_types.hpp>
#include <dynarithmic/twain/imagehandler/image_handler.hpp>
#include <dynarithmic/twain/info/file_transfer_info.hpp>
#include <dynarithmic/twain/info/buffered_transfer_info.hpp>
namespace dynarithmic
{
    namespace twain 
    {
        class acquire_characteristics;
        class buffered_transfer_info;
        class file_transfer_info;
        class capability_interface;
        class capability_listener;
        class twain_session;
        class twain_source_pimpl;

        class twain_source 
        {
            public:
                using twain_app_info = twain_identity;
                using twain_source_info = twain_identity;
                using acquire_return_type = std::pair<int32_t, twain_array>;
                using byte_array = std::vector<BYTE>;
                using custom_data_type = unsigned char;
                using custom_data_container_type = std::vector<custom_data_type>;

                static constexpr int ACQUIRE_RETURN = 1;
                static constexpr int IMAGE_HANDLER = 2;
                static constexpr int TWAIN_ARRAY = 3;

                static constexpr int32_t acquire_ok = DTWAIN_NO_ERROR;
                static constexpr int32_t acquire_timeout = 1;
                static constexpr int32_t acquire_canceled = 2;

                twain_source(const twain_source&) = delete;
                twain_source& operator=(const twain_source&) = delete;
                twain_source(twain_source&& rhs) noexcept
                {
                    swap(*this, rhs);
                    rhs.m_theSource = nullptr;
                }
                twain_source& operator=(twain_source&& rhs) noexcept;
                twain_source(const source_select_info& select_info = source_select_info());

                ~twain_source() noexcept;

            private:
                static constexpr int IS_SUPPORTED = 1;
                static constexpr int IS_ENABLED = 2;
                typedef LONG(DLLENTRY_DEF* infofn)(DTWAIN_SOURCE, LPSTR, LONG);
                typedef twain_app_info& (twain_app_info::*appfn)(const std::string&);

                bool m_bIsSelected = false;
                twain_app_info m_sourceInfo;
                twain_session* m_pSession = nullptr;
                bool m_bCloseable = true;
                std::string m_source_details;
                DTWAIN_SOURCE m_theSource;
                bool m_bUIOnlyOn;
                bool m_bWeakAttach;
                std::shared_ptr<twain_source_pimpl> m_pTwainSourceImpl;

                void create_interfaces();
                void get_source_info_internal();
                void start_apply();
                void prepare_acquisition();
                void set_pdf_options();
                void swap(twain_source& left, twain_source& right) noexcept;
                acquire_return_type acquire_to_file(transfer_type transtype);
                acquire_return_type acquire_to_image_handles(transfer_type transtype);
                void wait_for_feeder(bool& status);
                file_transfer_info get_file_transfer_info();

            public:
                typedef double resolution_type;
                void attach(const source_select_info& select_return);
                void attach(DTWAIN_SOURCE source);
                void attach(twain_session& twSession, DTWAIN_SOURCE source);
                twain_source& make_weak(bool isWeak = true);
                void detach();

                template <typename T>
                void set_cap_listener(T& val);

                twain_identity get_source_info() const noexcept;
                HANDLE get_current_image();
                acquire_characteristics& get_acquire_characteristics();
                twain_source& set_acquire_characteristics(const acquire_characteristics& acq_characteristics) noexcept;
                buffered_transfer_info& get_buffered_transfer_info() noexcept;
                const capability_interface& get_capability_interface() const noexcept;
                acquire_return_type acquire();
                bool showui_only();
                TW_IDENTITY* get_twain_id();
                static image_handler get_images(const twain_array& images);
                bool open();
                bool close();
                bool is_open() const;
                bool is_selected() const noexcept;
                bool is_closeable() const noexcept;
                bool is_acquiring() const;
                bool is_uienabled() const;
                bool is_uionlysupported() const;
                image_information get_current_image_information() const;
                bool set_current_camera(const cameraside_value::value_type& camera);
                DTWAIN_SOURCE get_source() const noexcept { return m_theSource; }

                custom_data_container_type get_custom_data() const;
                bool set_custom_data(const custom_data_container_type& s) const
                {
                    return API_INSTANCE DTWAIN_SetCustomDSData(m_theSource, NULL, s.data(), static_cast<LONG>(s.size()), DTWAINSCD_USEDATA) ? true : false;
                }

                static bool acquire_no_error(int32_t errCode);
                static bool acquire_timed_out(int32_t errCode);
                static bool acquire_internal_error(int32_t errCode);
                const twain_session* get_session() const;
                std::string& get_details(bool refresh = false);
        };

        inline std::ostream& operator <<(std::ostream& os, const image_information& ii)
        {
            std::vector<std::string> sList;
            std::transform(ii.bitsPerSample.begin(), 
                           ii.bitsPerSample.begin() + (std::min)(static_cast<size_t>(ii.numsamples), ii.bitsPerSample.size()),
                           std::back_inserter(sList), [](int n) { return std::to_string(n);  });
            std::string commaList = std::accumulate(sList.begin(), sList.end(), std::string(),
                                              [](const std::string& total, const std::string& n) { return total + "," + n; });
            std::string bps = "[" + commaList + "]";
            os << "\nx_resolution: " << ii.x_resolution << "\n"
                << "y_resolution: " << ii.y_resolution << "\n"
                << "width: " << ii.width << "\n"
                << "length: " << ii.length << "\n"
                << "numsamples: " << ii.numsamples << "\n"
                << "bitsPerSample: " << bps << "\n"
                << "bitsPerPixel: " << ii.bitsPerPixel << "\n"
                << "planar: " << (ii.planar ? "true" : "false") << "\n"
                << "pixeltype: " << ii.pixelType << "\n"
                << "compression: " << ii.compression;
            return os;
        }
    }
}
#endif
