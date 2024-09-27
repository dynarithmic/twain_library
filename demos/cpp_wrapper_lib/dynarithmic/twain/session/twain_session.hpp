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

#ifndef DTWAIN_TWAIN_SESSION_HPP
#define DTWAIN_TWAIN_SESSION_HPP

#include <string>
#include <iterator>
#include <utility>
#include <unordered_map>
#include <vector>
#include <type_traits>
#include <functional>
#include <memory>
#include <set>

#if __cplusplus >= 201703L
#include <optional>
#define optional_type_ std::optional
#define optional_null_ {}
#else
#include <boost/optional.hpp>
#define optional_type_ boost::optional
#define optional_null_ boost::none
#endif
#include <dynarithmic/twain/session/twain_characteristics.hpp>
#include <dynarithmic/twain/characteristics/twain_select_dialog.hpp>
#include <dynarithmic/twain/logging/error_logger.hpp>
#include <dynarithmic/twain/logging/twain_logger.hpp>
#include <dynarithmic/twain/dtwain_twain.hpp>
#include <dynarithmic/twain/twain_values.hpp>
#include <dynarithmic/twain/types/twain_callback.hpp>
#include <dynarithmic/twain/types/twain_array.hpp>
#include <dynarithmic/twain/twain_details.hpp>
#ifdef DTWAIN_CPP_NOIMPORTLIB
    #include <dtwainx2.h>
#else
    #include <dtwain.h>
#endif
#ifdef _MSC_VER
    #pragma warning( push )  // Stores the current warning state for every warning.
    #pragma warning( disable:4996)
#endif
namespace dynarithmic
{
    namespace twain
    {
        class twain_session;
        class twain_callback;
        class twain_source;
        class twain_identity;

        template <int N>
        struct source_selector
        {
            enum { value = N };
            twain_select_dialog get_dialog() const { return twain_select_dialog{}; }
        };

        /**
            Template instantiation denoting to use the TWAIN Select Source dialog when selecting a TWAIN Data Source.
            This is aliased by dynarithmic::twain::select_usedialog()
         */
        template <>
        struct source_selector<select_type::use_orig_dialog>
        {
            enum { value = select_type::use_orig_dialog };
            twain_select_dialog get_dialog() const { return twain_select_dialog{}; }

            static DTWAIN_SOURCE select(twain_select_dialog& dlg)
            {
                #ifdef _WIN32
                auto position = dlg.get_position();
                int32_t allFlags = 0;
                auto flags = dlg.get_flags();
                for (auto& f : flags)
                {
                    if (f == twain_select_dialog::uselegacy)
                        return API_INSTANCE DTWAIN_SelectSource();
                    allFlags |= static_cast<int32_t>(f);
                }
                if (allFlags & static_cast<int32_t>(twain_select_dialog::useposition))
                    allFlags &= ~(DTWAIN_DLG_CENTER_SCREEN | DTWAIN_DLG_CENTER);

                std::string strTitle = dlg.get_title();
                API_INSTANCE DTWAIN_SetTwainDialogFont(dlg.get_font());
                return API_INSTANCE DTWAIN_SelectSource2ExA(dlg.get_parent_window(),
                                            (allFlags & twain_select_dialog::usedefaulttitle)?nullptr:strTitle.c_str(),
                                            position.first,
                                            position.second,
                                            dlg.get_includename_list().c_str(),
                                            dlg.get_excludename_list().c_str(),
                                            dlg.get_name_mapping_s().c_str(),
                                            allFlags);

                #else
                    return API_INSTANCE DTWAIN_SelectSource();
                #endif
            }
        };

        /**
            Template instantiation denoting to select the default TWAIN Data Source
         */
        template <>
        struct source_selector<select_type::use_default>
        {
            enum { value = 1 };
            twain_select_dialog get_dialog() const { return twain_select_dialog{}; }
            static DTWAIN_SOURCE select(twain_select_dialog&) { return API_INSTANCE DTWAIN_SelectDefaultSource(); }
        };

        /**
            Template instantiation denoting to select the TWAIN source using the Product Name
         */
        template <>
        struct source_selector<select_type::use_name>
        {
            enum { value = select_type::use_name };
            twain_select_dialog get_dialog() const { return twain_select_dialog{}; }
            private:
                std::string m_name;
            public:
                source_selector(std::string name /**< [in] Product Name of the TWAIN device to open */) : m_name(std::move(name)) {}
                DTWAIN_SOURCE select(twain_select_dialog&) const { return API_INSTANCE DTWAIN_SelectSourceByNameA(m_name.c_str()); }
        };

        /**
        Template instantiation denoting to select the TWAIN source using the Product Name
        */
        template <>
        struct source_selector<select_type::use_dialog>
        {
            enum { value = select_type::use_dialog };
            twain_select_dialog get_dialog() const { return m_user_dialog; }

            private:
                twain_select_dialog m_user_dialog;
            public:
                source_selector(twain_select_dialog& dlg /**< [in] User-defined twain_dialog to use */) : m_user_dialog(dlg) {}
                DTWAIN_SOURCE select(twain_select_dialog& user_dialog) const
            {
                return source_selector<select_type::use_orig_dialog>::select(user_dialog);
            }
        };


        /**
            Template instantiation denoting to use the TWAIN Select Source dialog when selecting a TWAIN Data Source
         */
        using select_useorigdialog = source_selector<select_type::use_orig_dialog>;
        using select_default = source_selector<select_type::use_default>;
        using select_byname = source_selector<select_type::use_name>;
        using select_usedialog = source_selector<select_type::use_dialog>;

        /* Non-template calls to select_source using this struct */
        struct select_source_traits
        {
            enum source_select_type
            {
                use_legacy,
                use_enhanced_dialog,
                use_name,
                use_default
            };
            source_select_type m_selectType = use_legacy;
            twain_select_dialog m_useEnhancedDialog;
            std::string m_sourceName;
            bool m_selectDefault = false;

            select_source_traits& set_select_type(source_select_type sType) { m_selectType = sType; return *this; }
            select_source_traits& set_enhanced_dialog(twain_select_dialog& dlg) { m_useEnhancedDialog = dlg; return *this; }
            select_source_traits& set_source_name(std::string name) { m_sourceName = name; return *this; }
            source_select_type get_select_type() const { return m_selectType; }
            twain_select_dialog& get_enhanced_dialog() { return m_useEnhancedDialog; }
            std::string get_source_name() const { return m_sourceName; }
        };

        /**
            The twain_session class is the main class that allows the startup and stopping of the TWAIN system.<br>
            This is the main driver class, as no TWAIN interaction can occur without instantiating a twain_session.
            The twain_session class uses RAII (Resource Acquisition Is Initialization) techniques, meaning that when a
            twain_session object goes out of scope, the destructor for the twain_session will automatically close out the TWAIN session.<br>
            Note that if there are TWAIN devices still active (acquiring images), the twain_session destructor will not return until all
            the TWAIN sources are closed.  This may or may not be desirable, but currently this is how the twain_session is implemented.
        **/
        class twain_session
        {
        public:
            using twain_app_info = twain_identity;
            using twain_source_info = twain_identity;
            using error_logger_func = std::function<void(LONG)>;
            using logger_type = std::pair<twain_session*, std::unique_ptr<twain_logger>>;

            enum class source_status
            {
                closed,
                opened,
                selected,
                unknown
            };

            struct supported_filetype_info
            {
                private:
                    filetype_value::value_type m_filetype;
                    std::string m_filetypename;
                    std::vector<std::string> m_fileExtensions;
                public:
                    filetype_value::value_type get_type() const { return m_filetype; }
                    std::string get_name() const { return m_filetypename; }
                    void set_type(filetype_value::value_type val) { m_filetype = val; }
                    void set_name(std::string name) { m_filetypename = name; }
                    void add_extension(std::string ext) { m_fileExtensions.push_back(ext); }

                    template <typename Container = std::vector<std::string>>
                    Container get_extensions() const
                    {
                        Container c;
                        std::copy(m_fileExtensions.begin(), m_fileExtensions.end(), std::inserter(c, c.end()));
                        return c;
                    }
            };

        private:
            using callback_map_type = std::unordered_map<twain_source*, std::unique_ptr<twain_callback>>;
            using source_basic_info = twain_app_info;

            filetype_value::value_type m_filetype;
            std::string m_filetypename;
            std::vector<std::string> m_fileExtensions;
            twain_characteristics m_twain_characteristics;
            bool m_bStarted = false;
            bool m_bTripletsNotify = false;
            std::string m_dsm_path;
            std::string m_long_name;
            std::string m_short_name;
            std::string m_dtwain_path;
            std::string m_version_copyright;
            DTWAIN_HANDLE m_Handle = nullptr;
            logger_type m_logger;
            callback_map_type m_mapcallback;
            mutable std::vector<source_basic_info> m_source_cache;
            error_logger_func m_error_logger_func;
            std::unordered_map<std::string, std::string> m_source_detail_map;
            std::unordered_map<std::string, source_status> m_source_status_map;
            std::unordered_map<std::string, DTWAIN_SOURCE> m_source_name_to_handle;
            std::set<twain_source*> m_selected_sources;
        #ifdef DTWAIN_CPP_NOIMPORTLIB
            HMODULE m_DynamicHandle = 0;
            bool    m_bCacheHandle = true;
        #endif
        public:

        #ifdef DTWAIN_CPP_NOIMPORTLIB
            twain_session& cache_dll_handle(bool b = true) { m_bCacheHandle = b; return *this; }
            bool is_dllhandle_cached() const { return m_bCacheHandle; }
            void set_dllhandle(HMODULE h)
            {
                m_DynamicHandle = h;
                RuntimeDLL::DTWAIN_API__.InitDTWAINInterface(nullptr, h);
            }
            HMODULE get_dllhandle() const { return m_DynamicHandle; }
        #endif

            private:
                friend twain_source;
                error_logger m_error_logger;
                twain_logger m_dummy_logger;
                mutable std::vector<supported_filetype_info> m_singlepage_filetype_cache;
                mutable std::vector<supported_filetype_info> m_multipage_filetype_cache;

                callback_map_type& get_callback_map() noexcept { return m_mapcallback; }

                static LRESULT CALLBACK callback_proc(WPARAM wParam, LPARAM lParam, DTWAIN_LONG64 UserData);
                static LRESULT CALLBACK logger_callback_proc(const char* msg, DTWAIN_LONG64 UserData);
                static LRESULT CALLBACK error_callback_proc(LONG error, LONG64 UserData);

                bool start(bool bCleanStart);
                void update_source_status(const twain_source& ts);

                template <typename SourceSelector>
                DTWAIN_SOURCE select_source_impl(const SourceSelector& selector, twain_select_dialog& dlg, bool bOpen = true)
                {
                    if (m_Handle)
                    {
                        API_INSTANCE DTWAIN_OpenSourcesOnSelect(bOpen ? TRUE : FALSE);
                        auto src = selector.select(dlg);
                        return src;
                    }
                    return nullptr;
                }

                template <typename Container>
                struct CopyTraits1
                {
                    CopyTraits1(Container& c) : ref(c) {}
                    Container& ref;
                    auto getptr() { return std::inserter(ref, ref.end()); }
                };

                template <typename Container>
                struct CopyTraits2
                {
                    CopyTraits2(Container* c) : ref(c) {}
                    Container* ref;
                    auto getptr() { return ref; }
                };

                template <typename Container>
                void get_filetypes_impl(Container& c, std::vector<supported_filetype_info>& vFileTypeInfo,
                                        std::function<DTWAIN_ARRAY(void)> dtwain_func) const
                {
                    c.clear();
                    if (!vFileTypeInfo.empty())
                    {
                        std::copy(vFileTypeInfo.begin(),
                            vFileTypeInfo.end(), std::inserter(c, c.end()));
                        return;
                    }

                    DTWAIN_ARRAY allTypes = dtwain_func();

                    if (allTypes)
                    {
                        twain_array ta(allTypes);
                        size_t nTypes = ta.get_count();
                        auto insert_iter = std::inserter(c, c.end());
                        int32_t* pArrayBuffer = ta.get_buffer<int32_t>();
                        for (size_t i = 0; i < nTypes; ++i)
                        {
                            char nameBuf[100], extBuf[1024];
                            supported_filetype_info fInfo;
                            fInfo.set_type(static_cast<filetype_value::value_type>(pArrayBuffer[i]));
                            API_INSTANCE DTWAIN_GetFileTypeNameA(pArrayBuffer[i], nameBuf, 99);
                            API_INSTANCE DTWAIN_GetFileTypeExtensionsA(pArrayBuffer[i], extBuf, 1024);
                            fInfo.set_name(nameBuf);
                            std::string extStr = extBuf;
                            std::replace(extStr.begin(), extStr.end(), '|', ' ');
                            std::istringstream strm(extStr);
                            std::string oneExtension;
                            while (strm >> oneExtension)
                                fInfo.add_extension(oneExtension);
                            (*insert_iter) = fInfo;
                            ++insert_iter;
                            vFileTypeInfo.push_back(fInfo);
                        }
                    }
                }

                void setup_error_logging();
                void setup_logging();
                void mover(twain_session&& rhs) noexcept;
                void add_source(twain_source* pSource);
                void remove_source(twain_source* pSource);
            public:
                using callback_handle = callback_map_type::iterator;

                twain_session(const twain_session&) = delete;
                twain_session& operator=(const twain_session&) = delete;
                twain_session(startup_mode mode = startup_mode::none);
                twain_session(twain_session&& rhs) noexcept
                {
                    mover(std::forward<twain_session>(rhs));
                }

                twain_session& operator=(twain_session&& rhs) noexcept
                {
                    mover(std::forward<twain_session>(rhs));
                    return *this;
                }

                /// Destructor --  Automatically shuts down TWAIN Session
                ///
                /// The destructor effectively calls the stop() function.
                /// @see stop()
                ~twain_session();

                /// Test to see if the TWAIN session has been started.
                /// @returns **true** if the TWAIN session has been started, **false** otherwise.
                /// @see operator bool()
                bool started() const noexcept { return m_bStarted; }

                /// Test to see if the TWAIN session has been started.
                /// @returns **true** if the TWAIN session has been started, **false** otherwise.
                /// @see started()
                operator bool() const noexcept { return m_bStarted; }

                /// Attaches to an existing DTWAIN_HANDLE
                /// @returns **true** if successful, **false** if TWAIN session could not be started.
                bool attach(DTWAIN_HANDLE handle);

                /// Starts a TWAIN session by opening the TWAIN Data Source Manager (DSM).
                ///
                /// When the TWAIN DSM is started, the current set of twain_characteristics are used when initializing the TWAIN session.
                /// @returns **true** if successful, **false** if TWAIN session could not be started.
                /// @note Only a single TWAIN session can be started per thread.
                /// @see stop() get_twain_characteristics()
                bool start();

                /// Stops the TWAIN Data Source Manager (DSM).
                ///
                /// Once the DSM is stopped, a call to start() must be issued to restart the TWAIN DSM.
                /// @returns **true** if successful, **false** if unsuccessful
                /// @see start() get_twain_characteristics()
                /// @warning Calling stop() while a device is in the acquisition state will place your program in a busy-wait loop until the device(s) are out of the acquisition state.
                bool stop();

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
                static int call_dsm(TW_IDENTITY* pSource,
                                    TW_IDENTITY* pDest,
                                    int32_t dg,
                                    int32_t dat,
                                    int32_t msg,
                                    void* pdata);

                /// Returns an error string that describes the error given by **error_number**
                ///
                /// @param[in] error_number The number of the error.
                /// @returns An error string that describes the error
                /// @see get_last_error() twain_characteristics.get_language()
                /// @note The error string will be in the language specified by twain_characteristics::get_language()
                static std::string get_error_string(int32_t error_number);

                /// Returns a resource string
                ///
                /// @param[in] resource id.  For user-defined resources, this value should be >= DTWAIN_USERRES_START
                /// @returns A resource string
                /// @note The string will be in the language specified by twain_characteristics::get_language()
                static std::string get_resource_string(int32_t resource_id);

                /// Returns the last error encountered by the underlying DTWAIN library
                ///
                /// @returns An error number that represents the last error
                /// @see get_error_string()
                static int32_t get_last_error();

                /// Returns the string version of the TWAIN constant name.
                /// For example:
                ///
                /// get_twain_name(twain_category::TWPT, 0);
                ///
                /// will return "TWPT_BW", since the TWAIN names that start with TWPT,
                /// such as TWPT_BW has the value of 0 (see twain.h for verification).
                ///
                /// @returns A string version of the TWAIN constant name
                static std::string get_twain_name(twain_constant_category twain_category, int32_t twain_constant);

                error_logger& get_error_logger() noexcept { return m_error_logger; }

                /// Returns a short string that identifies the name of the Dynarithmic TWAIN Library version
                ///
                /// @returns a string that identifies the version of the underlying DTWAIN library in use.
                std::string get_short_version_name() const  noexcept { return m_short_name; }

                /// Returns a int32_t version string that identifies the name of the Dynarithmic TWAIN Library version
                ///
                /// @returns a int32_t version string that identifies the version of the underlying DTWAIN library in use.
                std::string get_long_version_name() const noexcept { return m_long_name; }

                /// Returns a short string that identifies the version and copyright of the Dynarithmic TWAIN Library version
                ///
                /// @returns a string that identifies the version and copyright of the underlying DTWAIN library in use.
                std::string get_version_copyright() const  noexcept { return m_version_copyright; }

                /// Returns the full path of the TWAIN Data Source Manager in use.
                ///
                /// @returns string that identifies the path of the TWAIN Data Source Manager
                std::string get_dsm_path() const noexcept { return m_dsm_path; }

                /// Returns the full path of the DTWAIN shared library being utilized.
                ///
                /// @returns Full path of the DTWAIN shared library being utilized.
                std::string get_dtwain_path() const noexcept { return m_dtwain_path; }

                /// Registers a twain_callback object with a TWAIN source for this TWAIN session.
                ///
                /// The twain_callback class allows your application to monitor and trap TWAIN events when the acquire is invoked.
                /// The twain_session keeps track of all the registered listeners.
                /// @param[in] source The twain_source that the callback will be registering the callback.
                /// @param[in] callback The user-defined callback object.
                /// @returns A handle to the registered callback (to be used later to unregister the callback), or optional_null_ if the registration fails
                /// @note Use the returned callback_handle to unregister a twain_callback
                /// @see unregister_callback()
                template <typename Callback>
                optional_type_<callback_handle> register_callback(twain_source& source, const Callback& callback)
                {
                    static_assert(std::is_base_of<twain_callback, Callback>::value == 1, "Callback is not derived from twain_callback");
                    auto iter = m_mapcallback.find(&source);
                    if (iter == m_mapcallback.end())
                    {
                        auto ptr = std::make_unique<Callback>(callback);
                        auto pr = m_mapcallback.insert({ &source, std::move(ptr) });
                        if (pr.second)
                            return pr.first;
                    }
                    return optional_null_;
                }

                /// Removes a twain_callback for this twain_session.
                ///
                /// @param[in] handle The callback_handle returned by register_callback.
                /// @returns **true** if the twain_callback is successfully removed, **false** otherwise.
                /// @see register_callback()
                bool unregister_callback(callback_handle handle);

                /// Registers a custom logging object derived from twain_logger with this TWAIN session.
                ///
                /// @param[in] logger custom logger object
                /// @see unregister_logger
                template <typename Logger, typename ...Args>
                Logger& register_logger(Args... theArgs)
                {
                    static_assert(std::is_base_of<twain_logger, Logger>::value == 1, "Logger is not derived from twain_logger");
                    auto ptr = std::make_unique<Logger>(std::forward<Args>(theArgs)...);
                    m_logger = { this, std::move(ptr) };
                    return static_cast<Logger&>(*(m_logger.second));
                }

                /// Removes logger from this TWAIN session
                ///
                /// @see register_logger
                void unregister_logger()
                {
                    if (m_logger.second)
                        m_logger.second.reset();
                    m_logger = { nullptr, nullptr };
                }

            /// Allows logging to be turned on or off during a TWAIN Session.
            ///
            /// To set the details of the logging setting, use the get_twain_characteristics().get_logger_details() to set the various details.
            /// @note enable_logger() is the only mechanism that can be used to enable or disable logging after a TWAIN
            /// session has started.
            /// @param[in] enable if **true** the logging is enabled, **false**, logging is disabled.
            void enable_logger(bool enable = true);

            /// Sets the enabling of triplet notifications being sent to the application
            ///
            /// @returns reference to the object that identifies this session by TWAIN.
            twain_session& enable_triplets_notification(bool bEnable);

            /// Returns the complete object that represents this TWAIN session's identity.
            ///
            /// @returns reference to the object that identifies this session by TWAIN.
            /// @note Unlike get_twain_id(), get_id() returns an object that describes the TWAIN session's name, language, etc.
            /// @see get_twain_id()
            twain_identity& get_id() { return m_twain_characteristics.get_app_info(); }

            /// Returns the TW_IDENTITY* that represents this TWAIN session.
            ///
            /// @returns a pointer to the **TW_IDENTITY** that represents this TWAIN session.
            /// @note The return value can be used in a call to call_dsm().
            /// @see call_dsm() get_id()
            TW_IDENTITY* get_twain_id() { return static_cast<TW_IDENTITY*>(&get_id().get_identity()); }

            template <typename Container>
            void get_sources_impl(Container& c) const
            {
                c.clear();
                if (!m_source_cache.empty())
                {
                    std::copy(m_source_cache.begin(),
                        m_source_cache.end(),
                        std::inserter(c, c.end()));
                    return;
                }

                twain_array ta;
                if (API_INSTANCE DTWAIN_EnumSources(ta.get_array_ptr()))
                {
                    DTWAIN_SOURCE src;
                    const size_t nSources = ta.get_count();
                    auto insert_iter = std::inserter(c, c.end());
                    for (size_t i = 0; i < nSources; ++i)
                    {
                        twain_identity tInfo;
                        API_INSTANCE DTWAIN_ArrayGetAt(ta.get_array(), static_cast<int32_t>(i), &src);
                        char szBuf[256];
                        API_INSTANCE DTWAIN_GetSourceProductNameA(src, szBuf, 255);
                        tInfo.set_product_name(szBuf);
                        API_INSTANCE DTWAIN_GetSourceProductFamilyA(src, szBuf, 255);
                        tInfo.set_product_family(szBuf);
                        API_INSTANCE DTWAIN_GetSourceManufacturerA(src, szBuf, 255);
                        tInfo.set_manufacturer(szBuf);
                        API_INSTANCE DTWAIN_GetSourceVersionInfoA(src, szBuf, 255);
                        tInfo.set_version_info(szBuf);
                        (*insert_iter) = tInfo;
                        ++insert_iter;
                        m_source_cache.push_back(tInfo);
                    }
                }
            }


            /// Returns a container of twain_source_info, which describes each installed TWAIN device.
            ///
            /// The container defaults to std::vector<twain_source_info>
            /// @returns A container (default is std::vector<twain_source_info>) of twain_source_info, which describes each installed TWAIN device
            template <typename Container = std::vector<twain_identity>>
            Container get_all_source_info() const
            {
                Container C;
                static_assert(std::is_same<typename Container::value_type, twain_identity>::value == 1,
                    "Container is not of type twain_source_info");
                if (m_bStarted)
                    get_sources_impl(C);
                return C;
            }


            /// Returns a container of supported_fileytpe_info, which describes each supported file type
            ///
            /// The container defaults to std::vector<supported_filetype_info>
            /// @returns A container (default is std::vector<supported_filetype_info>) of supported_filetype_info, which describes each supported file type
            template <typename Container = std::vector<supported_filetype_info>>
            Container get_singlepage_filetype_info() const
            {
                Container C;
                static_assert(std::is_same<typename Container::value_type, supported_filetype_info>::value == 1,
                    "Container is not of type supported_filetype_info");
                if (m_bStarted)
                    get_filetypes_impl(C, m_singlepage_filetype_cache, API_INSTANCE DTWAIN_EnumSupportedSinglePageFileTypes);
                return C;
            }

            /// Returns a container of supported_fileytpe_info, which describes each supported file type
            ///
            /// The container defaults to std::vector<supported_filetype_info>
            /// @returns A container (default is std::vector<supported_filetype_info>) of supported_filetype_info, which describes each supported file type
            template <typename Container = std::vector<supported_filetype_info>>
            Container get_multipage_filetype_info() const
            {
                Container C;
                static_assert(std::is_same<typename Container::value_type, supported_filetype_info>::value == 1,
                    "Container is not of type supported_filetype_info");
                if (m_bStarted)
                    get_filetypes_impl(C, m_multipage_filetype_cache, API_INSTANCE DTWAIN_EnumSupportedMultiPageFileTypes);
                return C;
            }


            /// Selects a TWAIN Device that will be used to acquire images.
            ///
            /// Allows selection of a TWAIN Source using one of 3 methods, as denoted by the selector parameter:
            /// <ul>
            ///     <li>1. Using the TWAIN Select Source dialog</li>
            ///     <li>2. Select a source by using the product name of the device</li>
            ///     <li>3. Select the default TWAIN Source</li>
            /// </ul>
            /// If no device is selected, the returned source_select_info will have the source_select_info::creation_status set to **false**.
            /// @param[in] selector The type of selection to use (dialog, by product name, or default)
            /// @param[in] open_source If **true**, and a TWAIN device is successfully selected, automatically opens the device for further operations.
            /// @returns A source_select_info that describes the DTWAIN_SOURCE selected.
            /// @note if the **open_source** parameter is **false**, the program must call the twain_source::open() function.
            /// @see dynarithmic::twain::source_selector<select_type::use_dialog>() dynarithmic::twain::source_selector<select_type::use_name>() dynarithmic::twain::source_selector<select_type::use_default>()
            ///
            /**
            \code {.cpp}
             #include <dynarithmic\twain\twain_session.hpp>
             #include <dynarithmic\twain\twain_source.hpp>
             using namespace dynarithmic::twain;
             int main()
             {
                twain_session session;
                if (session.start())
                {
                   twain_source source = session.select_source(); // select a source and automatically open
                   if ( source.is_selected() )
                   {
                      // Source was selected
                   }
                }
            }
            \endcode
            */
            template <typename T = select_useorigdialog>
            source_select_info select_source(const T& selector = dynarithmic::twain::source_selector<select_type::use_orig_dialog>(),
                bool open_source = true)
            {
                if (!started())
                {
                    if (!start())
                    {
                        API_INSTANCE DTWAIN_SetLastError(DTWAIN_ERR_NO_SESSION);
                        return source_select_info();
                    }
                }
                bool isCanceled = false;
                DTWAIN_SOURCE ret = nullptr;
                if (static_cast<int>(selector.value) == static_cast<int>(select_type::use_dialog))
                {
                    auto dlg = selector.get_dialog();
                    ret = select_source_impl(selector, dlg, open_source);
                }
                else
                    ret = select_source_impl(selector, twain_select_dialog().set_flags({ twain_select_dialog::uselegacy }), open_source);
                if (API_INSTANCE DTWAIN_GetLastError() == DTWAIN_ERR_SOURCESELECTION_CANCELED)
                    isCanceled = true;
                source_select_info sRet;
                sRet.source_handle = ret;
                sRet.session_handle = this;
                sRet.is_canceled = isCanceled;
                return sRet;
            }

            source_select_info select_source(select_source_traits& traits);

            /** Adds an error value to the error log
            *
            */
            void log_error(int32_t msg);

            bool set_language_resource(std::string language);

            /// Sets the temporary directory that is used when acquiring images to a file
            /// @param[in] dir Temporary directory to use when acquiring to image files
            /// @returns The current twain_session object.
            twain_session& set_temporary_directory(std::string dir);

            /// Gets the current directory used to store temporary images when acquiring to files
            ///
            /// @returns string representing the current temporary directory.
            /// @see set_temporary_directory()
            std::string get_temporary_directory() const noexcept;

            /// Indicates the TWAIN Data Source Manager to use (version 1.x or 2.x, or default) when the TWAIN session is started.
            /// @param[in] dsm TWAIN Data Source Manager to use when TWAIN session is started.
            /// @returns Reference to current twain_session object (**this**)
            /// @note the default TWAIN DSM will always be the first one found using the search order specified by get_dsm_search_order()
            /// @see set_dsm_search_order() get_dsm_search_order() twain_session::get_dsm_path() twain_session::start()
            twain_session& set_dsm(dsm_type dsm) noexcept;

            /// @param[in] search_order Directory search order
            /// @param[in] user_directory Optional user-defined directory
            /// @returns Reference to current twain_session object (**this**)
            /// @note the default search order is "WSOCU"
            /// @see set_dsm_search_order() get_dsm_search_order() twain_session::get_dsm_path() twain_session::start()
            twain_session& set_dsm_search_order(std::string search_order, std::string user_directory) noexcept;
            twain_session& set_dsm_search_order(int search_order) noexcept;


            /// Sets whether acquiring images requires a user-defined TWAIN message loop to run.
            ///
            ///   An application that desires to have a customized TWAIN acquisition loop must call this function with a **true** value when twain_source::acquire() is called. Once this is done
            ///   the application must provide the loop to be processed when images are being acquired (see the dynarithmic::twain::twain_loop_win32 class as an example).
            ///
            ///   If there is no custom TWAIN loop, then the looping mechanism internal to this library will be used. By default, the application will not use
            ///   a custom loop, and will use the internal looping method when obtaining images.
            ///   @params[in] use_custom If **true**, sets the application to use a custom TWAIN loop
            ///   @returns Reference to current twain_session object (**this**)
            twain_session& set_custom_twain_loop(bool use_custom);

            /**
            *  \returns Returns **true** if the application is acquiring images and will provide the TWAIN
            *           message loop, **false** if the message loop that is internal to this library will be used.
            */
            bool is_custom_twain_loop();

            /// Sets the application information that will be used by the TWAIN Data Source Manager
            /// @param[in] info Reference a twain_app_info, describing the application information to use
            /// @returns Reference to current twain_session object (**this**)
            /// @note Use this function to have the TWAIN DSM recognize the application name, version, product name, etc.
            twain_session& set_app_info(const twain_app_info& info);

            twain_session& register_error_callback(error_logger_func fn);
            /// Gets a reference to current application information
            ///
            /// @returns Reference to the current twain_app_info that describes the application information
            /// @see set_app_info()
            twain_app_info& get_app_info();

            std::string get_details(const std::vector<std::string>& container, details_info info = { true, 2 });
            std::string get_details(details_info info = { true, 2 });
            source_status get_source_status(const twain_source& ts);
            source_status get_source_status(std::string prodName);

            std::string to_api_string(const std::string& str);

            DTWAIN_SOURCE get_source_handle_from_name(std::string prodName);
            twain_logger& get_logger() noexcept
            {
                if (m_logger.second)
                    return *m_logger.second;
                return m_dummy_logger;
            }
            logger_type& get_logger_type() noexcept { return m_logger; }
        };
    }
}
#ifdef _MSC_VER
#pragma warning(pop)
#endif
#endif
