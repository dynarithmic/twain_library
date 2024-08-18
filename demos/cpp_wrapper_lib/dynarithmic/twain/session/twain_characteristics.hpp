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

#ifndef TWAIN_CHARACTERISTICS_HPP
#define TWAIN_CHARACTERISTICS_HPP
#include <dynarithmic/twain/types/twain_types.hpp>
#include <dynarithmic/twain/identity/twain_identity.hpp>
#include <dynarithmic/twain/logging/error_logger_details.hpp>

namespace dynarithmic
{
    namespace twain
    {
        // TWAIN characteristics when a session has been started
        /**
        *  The twain_characteristics class describes the various aspects of a TWAIN session such as DSM to use, logging, etc.<p>
        *  The twain_session class has an instance of twain_characteristics.  To set the current twain_characteristics,
        *  call twain_session::get_twain_characeristics() to get a reference to the twain_characteristics object, and issue the requisite
        *  functions described here to set the various options.
        *
        *   \code {.cpp}
        *   twain_session session;
        *   //...
        *   twain_characteristics& tc = session.get_twain_characteristics();
        *   tc.set_language("english").set_temporary_directory("c:\\mytemp");
        *   \endcode
        *
        */
        class twain_characteristics
        {
            dynarithmic::twain::dsm_type dsmToUse;
            twain_identity app_info;
            std::string m_strSearchOrder;
            error_logger_details errorlog_details;
            std::string m_strTemporaryDirectory;
            std::string m_strlibLanguage;
            std::string m_strResourcePath;
            std::string m_strSearchDirectory;
            int m_classicSearchOrder;
            bool m_bUsingCustomLoop;
            bool m_bCheckHandles;

        public:
            twain_characteristics() : dsmToUse(dsm_type::legacy_dsm),
                m_strSearchOrder("WSOCU"),
                m_strlibLanguage("english"),
                m_classicSearchOrder(-1),
                m_bUsingCustomLoop(false),
                m_bCheckHandles(true)
            {}

            /// Indicates the TWAIN Data Source Manager to use (version 1.x or 2.x, or default) when the TWAIN session is started.
            /// @param[in] dsm TWAIN Data Source Manager to use when TWAIN session is started.
            /// @returns Reference to current twain_characteristics object (**this**)
            /// @note the default TWAIN DSM will always be the first one found using the search order specified by get_dsm_search_order()
            /// @see set_dsm_search_order() get_dsm_search_order() twain_session::get_dsm_path() twain_session::start()
            twain_characteristics& set_dsm(dsm_type dsm) noexcept { dsmToUse = dsm; return *this; }

            /// Sets the temporary directory that is used when acquiring images to a file
            /// @param[in] dir Temporary directory to use when acquiring to image files
            /// @returns Reference to current twain_characteristics object (**this**)
            twain_characteristics& set_temporary_directory(std::string dir) noexcept { m_strTemporaryDirectory = std::move(dir); return *this; }

            /// Sets the application information that will be used by the TWAIN Data Source Manager
            /// @param[in] info Reference a twain_app_info, describing the application information to use
            /// @returns Reference to current twain_characteristics object (**this**)
            /// @note Use this function to have the TWAIN DSM recognize the application name, version, product name, etc.
            twain_characteristics& set_app_info(const twain_identity& info) { app_info = info; return *this; }

            twain_characteristics& set_errorlogging_details(const error_logger_details& elog) { errorlog_details = elog; return *this; }

            /// Sets the library's order of directories to search for an available TWAIN Data Source Manager (TWAIN_32.DLL or TWAINDSM.DLL)
            /// 
            /// The first file found that is identified to be a valid TWAIN DSM will stop the search, and that DSM is that one that will be used throughout the TWAIN Session.<p>
            /// The search order is denoted by a character string of no longer than 5 characters, where each character denotes where to search
            /// for the TWAIN DSM.  <p>
            ///     'W' --search the environment's Windows directory (for example, "C:\\Windows")<br>
            ///     'S' --search the environment's System directory (for example, "C:\\Windows\\System32")<br>
            ///     'O' --search the directories listed in the environment's PATH system variable<br>
            ///     'C' --search the current directory(usually the directory where your application is installed) <br>
            ///     'U' --search a user - defined directory.
            ///             
            /// For example :
            /// \code{ .cpp }
            /// twain_session session;
            /// ...
            /// twain_characteristics& tc = session.get_twain_characteristics().set_dsm_search_order("CWSOU");
            /// \endcode
            /// 
            ///  will start the search for the TWAIN DSM using the following sequence when the TWAIN session is started(see twain_session::start()) :
            ///  <ul>
            ///  <li>The Current directory< / li>
            ///  <li>Windows directory< / li>
            ///  <li>System directory< / li>
            ///  <li>The Operating System's PATH directory</li>
            ///  <li>The user - defined directory< / li>
            ///  </ul>
            ///  
            /// @param[in] search_order Directory search order
            /// @param[in] user_directory Optional user-defined directory
            /// @returns Reference to current twain_characteristics object (**this**)
            /// @note the default search order is "WSOCU"
            /// @see set_dsm_search_order() get_dsm_search_order() twain_session::get_dsm_path() twain_session::start()
            twain_characteristics& set_dsm_search_order(std::string search_order,
                std::string user_directory) noexcept
            {
                m_strSearchOrder = std::move(search_order);
                m_strSearchDirectory = std::move(user_directory);
                return *this;
            }

            twain_characteristics& set_dsm_search_order(int search_order) noexcept;

            /// Sets whether acquiring images requires a user-defined TWAIN message loop to run.
            /// 
            ///   An application that desires to have a customized TWAIN acquisition loop must call this function with a **true** value when twain_source::acquire() is called. Once this is done
            ///   the application must provide the loop to be processed when images are being acquired (see the dynarithmic::twain::twain_loop_win32 class as an example).
            /// 
            ///   If there is no custom TWAIN loop, then the looping mechanism internal to this library will be used. By default, the application will not use 
            ///   a custom loop, and will use the internal looping method when obtaining images.
            ///   @params[in] use_custom If **true**, sets the application to use a custom TWAIN loop
            ///   @returns Reference to current twain_characteristics object (**this**)
            twain_characteristics& set_custom_twain_loop(bool use_custom) { m_bUsingCustomLoop = use_custom; return *this; }

            twain_characteristics& set_check_handles(bool bSet) { m_bCheckHandles = bSet; return *this; }

            /// Sets the current language used for resource strings, error messages, and log file text
            /// 
            /// @param[in] lang language to use
            /// @returns Reference to current twain_characteristics object (**this**)
            /// @see get_language()
            /**
            *   \code {.cpp}
            *   twain_session session;
            *   //...
            *   twain_characteristics& tc = session.get_twain_characteristics().set_language("french");
            *   session.start();
            *   \endcode
            *   The above code will set the resource strings, error messages, log information to French before the session is started.<p>
            *   Note that the languages are found in the resource text files, with names with the following pattern:<br>
            *   **twainresourcestrings_<language>.txt**<p>
            *   where <**language**> is the language to use.
            */
            twain_characteristics& set_language(std::string lang) noexcept { m_strlibLanguage = std::move(lang); return *this; }

            /// Sets the directory where the TWAIN resource strings will be found.
            /// 
            /// By default, the resource strings are in the same directory where the DTWAIN Dynamic Link Library is found.
            /// @param[in] dir The path of the resource directory
            /// @returns Reference to current twain_characteristics object (**this**)
            /// @see get_language()
            twain_characteristics& set_resource_directory(std::string dir) noexcept { m_strResourcePath = std::move(dir); return *this; }

            /// Gets a reference to current application information
            /// 
            /// @returns Reference to the current twain_app_info that describes the application information
            /// @see set_app_info()
            twain_identity& get_app_info() { return app_info; }

            /// Gets the current TWAIN Data Source Manager type that will be used
            /// 
            /// @returns TWAIN Data Source Manager type that is to be used
            /// @see set_dsm()
            dsm_type get_dsm() const noexcept { return dsmToUse; }

            /// Gets the current directory search order when searching for the TWAIN Data Source Manager
            /// 
            /// @returns string representing the current temporary directory.
            /// @see set_dsm_search_order()
            std::string get_dsm_search_order() const noexcept { return m_strSearchOrder; }

            /// Gets the current user-defined directory for the TWAIN Data Source Manager to be searched
            /// 
            /// This function returns the path of where a search will be done for the TWAIN DSM.  See the set_dsm_user_directory() and the 'U' search specification.
            /// @returns string representing the current user-defined directory to search for the TWAIN DSM
            /// @see set_dsm_user_directory() set_dsm_search_order()
            std::string get_dsm_user_directory() const  noexcept { return m_strSearchDirectory; }

            error_logger_details& get_errorlogger_details() { return errorlog_details; }

            /// Gets the current directory used to store temporary images when acquiring to files
            /// 
            /// @returns string representing the current temporary directory.
            /// @see set_temporary_directory()
            std::string get_temporary_directory() const noexcept { return m_strTemporaryDirectory; }

            /**
            *  \returns Returns **true** if the application is acquiring images and will provide the TWAIN
            *           message loop, **false** if the message loop that is internal to this library will be used.
            */
            bool is_custom_twain_loop() const { return m_bUsingCustomLoop; }

            bool is_check_handles() const { return m_bCheckHandles; }

            /// Gets the current language used for resource strings, error messages, and log file text
            /// 
            /// @returns string representing the current language that will be used.
            /// @see set_language()
            std::string get_language() const noexcept { return m_strlibLanguage; }

            /// Gets the current directory where the DTWAIN library resource strings will be found
            /// 
            /// @returns string representing the current language that will be used.
            /// @see set_resource_directory()
            std::string get_resource_directory() const noexcept { return m_strResourcePath; }
        };
    }
}
#endif
