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

#include <dynarithmic/twain/session/twain_session.hpp>
#include <dynarithmic/twain/logging/logger_callback.hpp>
#include <dynarithmic/twain/twain_source.hpp>
#include <dynarithmic/twain/utilities/string_utilities.hpp>
#include <chrono>
#include <thread>

#if __cplusplus >= 201703L
    #include <numeric>
    #include <algorithm>
    #define USE_CPPSTRING_FUNCS 
    #define join_strings_ dynarithmic::twain::join
    #define trim_copy_string_ dynarithmic::twain::trim_copy
#else
    #include <boost/algorithm/string.hpp>
    #include <boost/algorithm/string/join.hpp>
    #define join_strings_ boost::algorithm::join
    #define trim_copy_string_ boost::algorithm::trim_copy
#endif

namespace dynarithmic
{
    namespace twain
    {
        template <typename T>
        static uint64_t PtrToInt64(T p)
        {
            const void* ptr = reinterpret_cast<const void*>(p);
            return static_cast<uint64_t>(reinterpret_cast<uintptr_t>(ptr));
        }                

        bool twain_session::start(bool bCleanStart)
        {
#ifdef DTWAIN_CPP_NOIMPORTLIB
            if (bCleanStart && !get_dllhandle())
            {
#ifndef DTWAIN_USELOADEDLIB
                HMODULE hDTwainModule = ::LoadLibraryA(DTWAIN_DLLNAME);
#else
                HMODULE hDTwainModule = ::GetModuleHandleA(DTWAIN_DLLNAME);
#endif
                if (hDTwainModule)
                    set_dllhandle(hDTwainModule);
                else
                    return false;
            }
#endif 
#ifdef DTWAIN_USELOADEDLIB
            return true;
#else
            m_source_detail_map.clear();
            m_error_logger.clear();
            m_error_logger.set_maxsize(m_twain_characteristics.get_errorlogger_details().get_maxsize());

            if (bCleanStart && !API_INSTANCE DTWAIN_IsTwainAvailable())
            {
                m_error_logger.add_error(DTWAIN_ERR_TWAIN_NOT_INSTALLED);
                return false;
            }

            API_INSTANCE DTWAIN_SetResourcePathA(m_twain_characteristics.get_resource_directory().c_str());
            if (!API_INSTANCE DTWAIN_IsInitialized())
            {
                if (bCleanStart)
                {
                    m_bOCRStarted = false;
                    m_Handle = API_INSTANCE DTWAIN_SysInitialize();
                    if (m_Handle)
                    {
                        if (!API_INSTANCE DTWAIN_InitOCRInterface())
                            m_error_logger.add_error(API_INSTANCE DTWAIN_GetLastError());
                        else
                            m_bOCRStarted = true;
                    }
                }
                if (!m_Handle)
                {
                    m_error_logger.add_error(DTWAIN_ERR_NOT_INITIALIZED);
                    return false;
                }
            }

            API_INSTANCE DTWAIN_SetErrorCallback64(error_callback_proc, PtrToInt64(this)); 
            API_INSTANCE DTWAIN_LoadCustomStringResourcesA(m_twain_characteristics.get_language().c_str());

            auto sz = API_INSTANCE DTWAIN_GetShortVersionStringA(nullptr, 0);
            std::vector<char> retBuf(sz + 1);
            API_INSTANCE DTWAIN_GetShortVersionStringA(retBuf.data(), static_cast<int32_t>(retBuf.size()));
            m_short_name = retBuf.data();

            sz = API_INSTANCE DTWAIN_GetVersionStringA(nullptr, 0);
            retBuf.resize(sz + 1);
            API_INSTANCE DTWAIN_GetVersionStringA(retBuf.data(), static_cast<int32_t>(retBuf.size()));
            m_long_name = retBuf.data();

            retBuf.resize(1024);
            API_INSTANCE DTWAIN_GetLibraryPathA(retBuf.data(), static_cast<int32_t>(retBuf.size()));
            m_dtwain_path = retBuf.data();

            retBuf.resize(1024);
            API_INSTANCE DTWAIN_GetVersionCopyrightA(retBuf.data(), static_cast<int32_t>(retBuf.size()));
            m_version_copyright = retBuf.data();

            if (m_logger.second/* && m_logger.second->is_enabled()*/)
                setup_logging();
#ifdef _WIN64
            m_twain_characteristics.set_dsm(dsm_type::version2_dsm);
#endif
            API_INSTANCE DTWAIN_SetTwainDSM(static_cast<int32_t>(m_twain_characteristics.get_dsm()));
            twain_app_info aInfo = m_twain_characteristics.get_app_info();
            API_INSTANCE DTWAIN_SetAppInfoA(aInfo.get_version_info().c_str(),
                aInfo.get_manufacturer().c_str(),
                aInfo.get_product_family().c_str(),
                aInfo.get_product_name().c_str());
            std::string searchDir = m_twain_characteristics.get_dsm_user_directory();
            API_INSTANCE DTWAIN_SetDSMSearchOrderExA(m_twain_characteristics.get_dsm_search_order().c_str(),
                searchDir.empty() ? nullptr : searchDir.c_str());
            API_INSTANCE DTWAIN_SetLanguage(aInfo.get_language());
            API_INSTANCE DTWAIN_SetCountry(aInfo.get_country());

            // Temporary directory
            std::string sDir = m_twain_characteristics.get_temporary_directory();
            if (!sDir.empty())
                API_INSTANCE DTWAIN_SetTempFileDirectoryA(sDir.c_str());
            else
            {
                int32_t retVal = API_INSTANCE DTWAIN_GetTempFileDirectoryA(NULL, 0);
                if (retVal > 0)
                {
                    sDir.resize(retVal);
                    API_INSTANCE DTWAIN_GetTempFileDirectoryA(&sDir[0], retVal);
                    m_twain_characteristics.set_temporary_directory(sDir);
                }
            }
            const bool twainStarted = API_INSTANCE DTWAIN_StartTwainSession(nullptr, nullptr) != 0;
            if (twainStarted)
            {
                auto& app_info = m_twain_characteristics.get_app_info();
                app_info = *static_cast<TW_IDENTITY*>(API_INSTANCE DTWAIN_GetTwainAppID());

                auto len = API_INSTANCE DTWAIN_GetDSMFullNameA(static_cast<int32_t>(m_twain_characteristics.get_dsm()), nullptr, 0,
                    nullptr);
                if (len > 0)
                {
                    std::vector<char> szBuffer(len);
                    API_INSTANCE DTWAIN_GetDSMFullNameA(static_cast<int32_t>(m_twain_characteristics.get_dsm()), szBuffer.data(), len,
                        nullptr);
                    m_dsm_path = szBuffer.data();
                }

                API_INSTANCE DTWAIN_EnableMsgNotify(TRUE);
                API_INSTANCE DTWAIN_EnableTripletsNotify(m_bTripletsNotify);
                API_INSTANCE DTWAIN_SetCallback64(callback_proc, PtrToInt64(this));
                m_source_cache.clear();
                m_bStarted = true;
                return true;
            }
            return false;
    #endif
        }


        void twain_session::setup_error_logging()
        {
            API_INSTANCE DTWAIN_SetErrorCallback64(error_callback_proc, PtrToInt64(this));
        }

        void twain_session::setup_logging()
        {
            if (m_logger.second)
            {
                auto& details = *(m_logger.second.get());
                if (details.is_enabled())
                {
                    auto log_destination = details.get_destination_aslong();
                    auto log_verbosity = details.get_verbosity_aslong();
                    log_destination |= DTWAIN_LOG_USECALLBACK;
                    API_INSTANCE DTWAIN_SetLoggerCallbackA(dynarithmic::twain::logger_callback_proc, PtrToInt64(this));
                    API_INSTANCE DTWAIN_SetTwainLogA(log_destination | log_verbosity, details.get_filename().c_str());
                }
                else
                    // Turn off logging
					API_INSTANCE DTWAIN_SetTwainLogA(0, "");
            }

        }

        void twain_session::mover(twain_session&& rhs) noexcept
        {
            m_bStarted = rhs.m_bStarted;
            m_Handle = rhs.m_Handle;
            m_mapcallback = std::move(rhs.m_mapcallback);
            m_logger = std::move(rhs.m_logger);
            m_source_cache = std::move(rhs.m_source_cache);
            m_twain_characteristics = std::move(rhs.m_twain_characteristics);
            m_error_logger_func = std::move(rhs.m_error_logger_func);
            API_INSTANCE DTWAIN_SetCallback64(callback_proc, PtrToInt64(this));
            API_INSTANCE DTWAIN_SetErrorCallback64(error_callback_proc, PtrToInt64(this));
            rhs.m_Handle = nullptr;
        }

        twain_session::twain_session(startup_mode mode)
        {
            auto fn = [&](int32_t msg)
            {
                m_error_logger.add_error(msg);
            };
            m_error_logger_func = fn;
            if (mode == startup_mode::autostart)
                start(true);
        }

        twain_session::~twain_session()
        {
            try {
#ifdef DTWAIN_CPP_NOIMPORTLIB
                cache_dll_handle(false);
#endif
                stop();
            }
            catch (...) {}
        }

        /// Test to see if the TWAIN session has been started.
        /// @returns **true** if the TWAIN session has been started, **false** otherwise.
        /// @see started()

        /// Attaches to an existing DTWAIN_HANDLE
        /// @returns **true** if successful, **false** if TWAIN session could not be started.  
        bool twain_session::attach(DTWAIN_HANDLE handle)
        {
            auto handleStatus = API_INSTANCE DTWAIN_GetAPIHandleStatus(handle);
            if (handleStatus == 0)
                return false;
            m_Handle = handle;
            return start((handleStatus & DTWAIN_TWAINSESSIONOK)?true:false);
        }

        /// Starts a TWAIN session by opening the TWAIN Data Source Manager (DSM).
        /// 
        /// When the TWAIN DSM is started, the current set of twain_characteristics are used when initializing the TWAIN session.
        /// @returns **true** if successful, **false** if TWAIN session could not be started.  
        /// @note Only a single TWAIN session can be started per thread.
        /// @see stop() get_twain_characteristics()
        bool twain_session::start()
        {
            if (started())
                return true;
            return start(true); // a clean start
        }

        /// Stops the TWAIN Data Source Manager (DSM).  
        /// 
        /// Once the DSM is stopped, a call to start() must be issued to restart the TWAIN DSM.  
        /// @returns **true** if successful, **false** if unsuccessful
        /// @see start() get_twain_characteristics()
        /// @warning Calling stop() while a device is in the acquisition state will place your program in a busy-wait loop until the device(s) are out of the acquisition state.
        bool twain_session::stop()
        {
            using namespace std::chrono_literals;
#ifdef DTWAIN_CPP_NOIMPORTLIB
            struct HandleCloser
            {
                HMODULE h_;
                twain_session* m_pSession;
                HandleCloser(twain_session* thisSession, HMODULE h) : h_(h), m_pSession(thisSession) {}
                void detach() { h_ = nullptr; }
                ~HandleCloser()
                {
                    if (h_ && !m_pSession->is_dllhandle_cached())
                    {
                        ::FreeLibrary(h_);
                        m_pSession->set_dllhandle(nullptr);
                    }
                }
            };

            HandleCloser hCloser(this, get_dllhandle());
#endif  
            if (m_Handle)
            {
                while (API_INSTANCE DTWAIN_IsAcquiring())
                    std::this_thread::sleep_for(1ms);
                API_INSTANCE DTWAIN_SetCallback64(nullptr, 0);
                API_INSTANCE DTWAIN_SetLoggerCallbackA(nullptr, 0);
                if (API_INSTANCE DTWAIN_SysDestroy())
                {
                    m_Handle = nullptr;
                    m_logger = { nullptr, nullptr };
                    m_source_cache.clear();
                    while (!m_selected_sources.empty())
                    {
                        auto iter = m_selected_sources.begin();
                        (*iter)->close();
                    }
                    m_bStarted = false;
                    return true;
                }
            }
#ifdef DTWAIN_CPP_NOIMPORTLIB
            hCloser.detach();
#endif
            return false;
        }

        /// (For advanced TWAIN programmers) Allows low-level TWAIN triplet calls to the TWAIN Data Source Manager.
        /// 
        /// This function is intended for advanced or highly specialized calls to the TWAIN DSM, and is not usually necessary for almost all TWAIN-enabled applications.
        /// @returns The TWAIN return code that the TWAIN triplet is documented to return (for example: TWRC_SUCCESS, TWRC_FAILURE, etc.)
        /// @param[in,out] pSource Source TW_IDENTITY
        /// @param[in,out] pDest Destination TW_IDENTITY
        /// @param[in] dg TWAIN Triplet Data Group (DG)
        /// @param[in] dat TWAIN Triplet Data (DAT)
        /// @param[in] msg TWAIN Triplet Message (MSG)
        /// @param[in,out] pdata TWAIN data (depends on the triplet DG/DAT/MSG)
        /// @see get_twain_id()
        /// @note See the Twain Specification 2.4 for more information on TWAIN triplets.
        /// @warning Do not use this function if you are not highly familiar with the TWAIN API.

        int twain_session::call_dsm(TW_IDENTITY* pSource,
                                    TW_IDENTITY* pDest,
                                    int32_t dg,
                                    int32_t dat,
                                    int32_t msg,
                                    void* pdata
        )
        {
            return API_INSTANCE DTWAIN_CallDSMProc(pSource, pDest, dg, dat, msg, pdata);
        }


        /// Selects a TWAIN source by using the passed-in traits object
        /// 
        /// @Returns a source_select_info describing the selection of the source 
        source_select_info twain_session::select_source(select_source_traits& traits)
        {
            switch (traits.get_select_type())
            {
                case select_source_traits::use_legacy:
                {
                    return select_source();
                }
                break;
                case select_source_traits::use_enhanced_dialog:
                {
                    return select_source(select_usedialog(traits.get_enhanced_dialog()));
                }
                break;
                case select_source_traits::use_name:
                {
                    return select_source(select_byname(traits.get_source_name()));
                }
                break;
                case select_source_traits::use_default:
                {
                    return select_source(select_default());
                }
                break;
            }
            return {};
        }

        /// Returns an error string that describes the error given by **error_number**
        /// 
        /// @param[in] error_number The number of the error.
        /// @returns An error string that describes the error
        /// @see get_last_error() twain_characteristics.get_language()
        /// @note The error string will be in the language specified by twain_characteristics::get_language()
        std::string twain_session::get_error_string(int32_t error_number)
        {
            char sz[DTWAIN_USERRES_MAXSIZE + 1] = {};
            API_INSTANCE DTWAIN_GetErrorStringA(error_number, sz, DTWAIN_USERRES_MAXSIZE);
            return sz;
        }

        std::string twain_session::get_resource_string(int32_t resource_id)
        {
            char sz[DTWAIN_USERRES_MAXSIZE + 1] = {};
            API_INSTANCE DTWAIN_GetResourceStringA(resource_id, sz, DTWAIN_USERRES_MAXSIZE);
            return sz;
        }

        /// Returns the last error encountered by the underlying DTWAIN library
        /// 
        /// @returns An error number that represents the last error
        /// @see get_error_string()
        int32_t twain_session::get_last_error()
        {
            return API_INSTANCE DTWAIN_GetLastError();
        }

        /// Returns the last error encountered by the underlying DTWAIN library
        /// 
        /// @returns An error number that represents the last error
        /// @see get_error_string()
        std::string twain_session::get_twain_name(twain_constant_category twain_category, int32_t twain_constant)
        {
            static std::map<twain_constant_category, std::map<int32_t, std::string>> s_nameMap;
            bool bSearch = false;
            auto iter = s_nameMap.find(twain_category);
            if (iter == s_nameMap.end())
                bSearch = true;
            else
            {
                auto iter2 = iter->second.find(twain_constant);
                if (iter2 == iter->second.end())
                    bSearch = true;
                else
                    return iter2->second;
            }
            if (bSearch)
            {
                int32_t nChars = API_INSTANCE DTWAIN_GetTwainNameFromConstantA(static_cast<int32_t>(twain_category), twain_constant, nullptr, 0);
                if (nChars > 0)
                {
                    std::vector<char> vReturn(nChars + 1);
                    API_INSTANCE DTWAIN_GetTwainNameFromConstantA(static_cast<int32_t>(twain_category), twain_constant, vReturn.data(), nChars);
                    std::string sReturn(vReturn.data());
                    auto iter = s_nameMap.insert({ twain_category, {} }).first;
                    auto& iter2 = iter->second;
                    iter2.insert({ twain_constant,sReturn });
                    return sReturn;
                }
            }
            return {};
        }

        /// Removes a twain_callback for this twain_session.
        /// 
        /// @param[in] handle The callback_handle returned by register_callback.
        /// @returns **true** if the twain_callback is successfully removed, **false** otherwise.
        /// @see register_callback()
        bool twain_session::unregister_callback(callback_handle handle)
        {
            auto iter = m_mapcallback.find(handle->first);
            if (iter != m_mapcallback.end())
                m_mapcallback.erase(handle);
            return iter != m_mapcallback.end();
        }

        /// Allows logging to be turned on or off during a TWAIN Session.
        /// 
        /// To set the details of the logging setting, use the get_twain_characteristics().get_logger_details() to set the various details.
        /// @note enable_logger() is the only mechanism that can be used to enable or disable logging after a TWAIN
        /// session has started.
        /// @param[in] enable if **true** the logging is enabled, **false**, logging is disabled.
        void twain_session::enable_logger(bool enable)
        {
            if (m_logger.second)
            {
                m_logger.second->enable(enable);
                if (m_Handle)
                    setup_logging();
            }
        }

        /** Adds an error value to the error log
        *
        */
        void twain_session::log_error(int32_t msg /**< [in] Number of the error message */)
        {
            m_error_logger.add_error(msg);
        }

        bool twain_session::set_language_resource(std::string language)
        {
            std::string sCurrentLanguage = m_twain_characteristics.get_language();
            m_twain_characteristics.set_language(language);
            if (!API_INSTANCE DTWAIN_LoadCustomStringResourcesA(m_twain_characteristics.get_language().c_str()))
            {
                m_twain_characteristics.set_language(sCurrentLanguage);
                API_INSTANCE DTWAIN_LoadCustomStringResourcesA(m_twain_characteristics.get_language().c_str());
                return false;
            }
            return true;
        }

        /// Sets the temporary directory that is used when acquiring images to a file
        /// @param[in] dir Temporary directory to use when acquiring to image files
        /// @returns The current twain_session object.
        twain_session& twain_session::set_temporary_directory(std::string dir)
        {
            if (started())
            {
                DTWAIN_BOOL ret = API_INSTANCE DTWAIN_SetTempFileDirectoryA(dir.c_str());
                if (ret)
                    m_twain_characteristics.set_temporary_directory(dir);
            }
            else
                m_twain_characteristics.set_temporary_directory(dir);
            return *this;
        }


        /// Gets the current directory used to store temporary images when acquiring to files
        /// 
        /// @returns string representing the current temporary directory.
        /// @see set_temporary_directory()
        std::string twain_session::get_temporary_directory() const noexcept { return m_twain_characteristics.get_temporary_directory(); }

        /// Indicates the TWAIN Data Source Manager to use (version 1.x or 2.x, or default) when the TWAIN session is started.
        /// @param[in] dsm TWAIN Data Source Manager to use when TWAIN session is started.
        /// @returns Reference to current twain_session object (**this**)
        /// @note the default TWAIN DSM will always be the first one found using the search order specified by get_dsm_search_order()
        /// @see set_dsm_search_order() get_dsm_search_order() twain_session::get_dsm_path() twain_session::start()
        twain_session& twain_session::set_dsm(dsm_type dsm) noexcept { m_twain_characteristics.set_dsm(dsm); return *this; }


        twain_session& twain_session::set_dsm_search_order(std::string search_order, std::string user_directory) noexcept
        {
            m_twain_characteristics.set_dsm_search_order(search_order, user_directory);
            return *this;
        }

        twain_session& twain_session::set_dsm_search_order(int search_order) noexcept
        {
            m_twain_characteristics.set_dsm_search_order(search_order);
            return *this;
        }

        /// Sets whether acquiring images requires a user-defined TWAIN message loop to run.
        /// 
        ///   An application that desires to have a customized TWAIN acquisition loop must call this function with a **true** value when twain_source::acquire() is called. Once this is done
        ///   the application must provide the loop to be processed when images are being acquired (see the dynarithmic::twain::twain_loop_win32 class as an example).
        /// 
        ///   If there is no custom TWAIN loop, then the looping mechanism internal to this library will be used. By default, the application will not use 
        ///   a custom loop, and will use the internal looping method when obtaining images.
        ///   @params[in] use_custom If **true**, sets the application to use a custom TWAIN loop
        ///   @returns Reference to current twain_session object (**this**)
        twain_session& twain_session::set_custom_twain_loop(bool use_custom) { m_twain_characteristics.set_custom_twain_loop(use_custom); return *this; }

        /// Sets the application information that will be used by the TWAIN Data Source Manager
        /// @param[in] info Reference a twain_app_info, describing the application information to use
        /// @returns Reference to current twain_session object (**this**)
        /// @note Use this function to have the TWAIN DSM recognize the application name, version, product name, etc.

        twain_session& twain_session::set_app_info(const twain_app_info& info) { m_twain_characteristics.set_app_info(info); return *this; }

        twain_session& twain_session::register_error_callback(error_logger_func fn)
        {
            m_error_logger_func = fn;
            return *this;
        }

        /// Gets a reference to current application information
        /// 
        /// @returns Reference to the current twain_app_info that describes the application information
        /// @see set_app_info()
        twain_identity& twain_session::get_app_info() { return m_twain_characteristics.get_app_info(); }

        std::string twain_session::get_details(details_info info)
        {
            auto v = get_all_source_info();
            std::vector<std::string> vSourceNames;
            std::transform(v.begin(), v.end(), std::back_inserter(vSourceNames), [](twain_identity& id) { return id.get_product_name();});
            return get_details(vSourceNames, info);
        }

        std::string twain_session::get_details(const std::vector<std::string>& container_in, details_info info)
        {
            auto container = container_in;
            std::transform(std::begin(container_in), std::end(container_in), std::begin(container),
                [](const std::string& s) { return trim_copy_string_(s); });
            std::string sAllDetails;
#ifdef DTWAIN_USELOADEDLIB
            sAllDetails = json_generator().generate_details(*this, container,true);
            return sAllDetails;
#else
            std::sort(std::begin(container), std::end(container));
            std::string sMapKey = std::accumulate(container.begin(), container.end(), std::string(),
                [&](const std::string& total, const std::string& current)
                {
                    return total + "\x01" + current;
                });
            auto iter = m_source_detail_map.find(sMapKey);
            if (!info.bRefresh && iter != m_source_detail_map.end())
                return iter->second;
            if (iter != m_source_detail_map.end())
                m_source_detail_map.erase(iter);
            std::vector<std::string> aValidSources;
            auto allSources = get_all_source_info();
            for (auto& sourceName : container)
            {
                std::string sKeyToUse = trim_copy_string_(sourceName);
                if (std::find_if(allSources.begin(), allSources.end(),
                    [&](const source_basic_info& info) { return info.get_product_name() == sKeyToUse; }) ==
                    allSources.end())
                    continue;
                aValidSources.push_back(sKeyToUse);
            }
            if (aValidSources.empty())
                return {};
            std::string sources = join_strings_(aValidSources, "|");
            LONG nChars = API_INSTANCE DTWAIN_GetSessionDetailsA(nullptr, 0, info.indentFactor, TRUE);
            if (nChars > 0)
            {
                sAllDetails.resize(nChars);
                API_INSTANCE DTWAIN_GetSessionDetailsA(&sAllDetails[0], nChars, info.indentFactor, FALSE);
            }
            m_source_detail_map.insert({ sMapKey, sAllDetails });
            return sAllDetails;
#endif
        }

        struct HandleDestroyer
        {
            HANDLE h;
            HandleDestroyer(HANDLE h_) : h(h_) {}
            ~HandleDestroyer() { if (h) { GlobalUnlock(h); GlobalFree(h); } }
        };

        int twain_session::get_twain_constant(std::string twainName)
        {
            auto val = API_INSTANCE DTWAIN_GetTwainIDFromNameA(twainName.c_str());
            return val;
        }

        std::string twain_session::to_api_string(const std::string& str)
        {
            HANDLE h = API_INSTANCE DTWAIN_ConvertToAPIStringA(str.c_str());
            if (h)
            {
                HandleDestroyer hRAII(h);
                LPCSTR pData = (LPCSTR)GlobalLock(h);
                if ( pData )
                    return std::string(pData, GlobalSize(h));
            }
            return {};
        }

        void twain_session::add_source(twain_source* pSource)
        {
            m_selected_sources.insert(pSource);
        }

        void twain_session::remove_source(twain_source* pSource)
        {
            m_selected_sources.erase(pSource);
        }

        LRESULT CALLBACK twain_session::error_callback_proc(LONG error, LONG64 UserData)
        {
            const auto thisObject = reinterpret_cast<twain_session*>(UserData);
            if (thisObject)
                thisObject->log_error(error);
            return 1;
        }

        LRESULT CALLBACK twain_session::callback_proc(WPARAM wParam, LPARAM lParam, DTWAIN_LONG64 UserData)
        {
            LRESULT retVal = 1;
            auto thisObject = reinterpret_cast<twain_session*>(UserData);
            if (thisObject)
            {
                std::for_each(thisObject->get_callback_map().begin(),
                    thisObject->get_callback_map().end(),
                    [&](twain_session::callback_map_type::value_type& vt)
                    {
                        retVal = static_cast<LRESULT>(vt.second->call_func(wParam, lParam, vt.first));
                    }
                );
            }
            return retVal;
        }
        bool twain_session::is_custom_twain_loop()
        {
            return m_twain_characteristics.is_custom_twain_loop();
        }

        twain_session& twain_session::enable_triplets_notification(bool bEnable)
        {
            m_bTripletsNotify = bEnable;
            if ( started() )
                API_INSTANCE DTWAIN_EnableTripletsNotify(bEnable?1:0);
            return *this;
        }

        void twain_session::update_source_status(const twain_source& ts)
        {
            bool isOpen = ts.is_open();
            bool isSelected = ts.is_selected();
            std::string prodName = ts.get_source_info().get_product_name();
            m_source_name_to_handle[prodName] = ts.get_source();
            if (!isOpen && !isSelected)
            {
                m_source_status_map[prodName] = source_status::closed;
                m_source_name_to_handle[prodName] = nullptr;
            }
            else
            if (isOpen)
                m_source_status_map[prodName] = source_status::opened;
            else
            if (isSelected)
                m_source_status_map[prodName] = source_status::selected;
            else
            {
                m_source_status_map[prodName] = source_status::unknown;
                m_source_name_to_handle[prodName] = nullptr;
            }
        }

        twain_session::source_status twain_session::get_source_status(const twain_source& ts)
        {
            std::string prodName = ts.get_source_info().get_product_name();
            return get_source_status(prodName);
        }

        twain_session::source_status twain_session::get_source_status(std::string prodName)
        {
            auto iter = m_source_status_map.find(prodName);
            if (iter == m_source_status_map.end())
                return source_status::unknown;
            return iter->second;
        }

        DTWAIN_SOURCE twain_session::get_source_handle_from_name(std::string prodName)
        {
            auto iter = m_source_name_to_handle.find(prodName);
            if (iter == m_source_name_to_handle.end())
                return {};
            return iter->second;
        }
    };
}

