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
// This class is used to listen for TWAIN events when an acquisition is occurring.  Use this to do special
// processing on acquisition startup, UI opening and closing, etc.  See the DTWAIN help manual for a list of
// the various events.

#ifndef DTWAIN_TWAIN_CALLBACK_HPP
#define DTWAIN_TWAIN_CALLBACK_HPP

#include <unordered_map>
#include <functional>
#include <dtwain.h>
#include <dynarithmic/twain/twain_values.hpp>
namespace dynarithmic
{
    namespace twain
    {
        class twain_source;
        class twain_callback
        {
        private:
            typedef int (twain_callback::* twain_callback_func)(twain_source&);
            using twain_error_func = std::function<LRESULT(LONG, LONG)>;
            using twain_callback_map = std::unordered_map<LONG, twain_callback_func>;

            LONG m_UserData;
            bool m_bDefaultHandler;
            bool m_bEnableTripletNotification;
            LONG m_nNotificationID;
            twain_callback_map m_functionMap;

        protected:
            virtual bool starthandler(twain_source&, WPARAM, LPARAM, int&) { return true; }
            virtual int preacquire(twain_source&) { return 1; }
            virtual int preacquire_terminate(twain_source&) { return 1; }
            virtual int acquiredone(twain_source&) { return 1; }
            virtual int acquirefailed(twain_source&) { return 1; }
            virtual int acquirecancelled(twain_source&) { return 1; }
            virtual int acquirestarted(twain_source&) { return 1; }
            virtual int pagecontinue(twain_source&) { return 1; }
            virtual int pagefailed(twain_source&) { return 1; }
            virtual int pagecancelled(twain_source&) { return 1; }
            virtual int transferready(twain_source&) { return 1; }
            virtual int transferdone(twain_source&) { return 1; }
            virtual int uiclosing(twain_source&) { return 1; }
            virtual int uiclosed(twain_source&) { return 1; }
            virtual int uiopened(twain_source&) { return 1; }
            virtual int uiopenfailure(twain_source&) { return 1; }
            virtual int defaulthandler(twain_source&, WPARAM, LPARAM, LONG) { return 1; }
            virtual int cliptransferdone(twain_source&) { return 1; }
            virtual int invalidimageformat(twain_source&) { return 1; }
            virtual int acquireterminated(twain_source&) { return 1; }
            virtual int transferstripready(twain_source&) { return 1; }
            virtual int transferstripdone(twain_source&) { return 1; }
            virtual int transferstripfailed(twain_source&) { return 1; }
            virtual int imageinfoerror(twain_source&) { return 1; }
            virtual int transfercancelled(twain_source&) { return 1; }
            virtual int filesavecancelled(twain_source&) { return 1; }
            virtual int filesaveok(twain_source&) { return 1; }
            virtual int filesaveerror(twain_source&) { return 1; }
            virtual int filepagesaveok(twain_source&) { return 1; }
            virtual int filepagesaveerror(twain_source&) { return 1; }
            virtual int processeddib(twain_source&) { return 1; }
            virtual int deviceevent(twain_source&) { return 1; }
            virtual int eojdetected(twain_source&) { return 1; }
            virtual int eojdetectedtransferdone(twain_source&) { return 1; }
            virtual int twainpagecancelled(twain_source&) { return 1; }
            virtual int twainpagefailed(twain_source&) { return 1; }
            virtual int querypagediscard(twain_source&) { return 1; }
            virtual int pagediscarded(twain_source&) { return 1; }
            virtual int appupdateddib(twain_source&) { return 1; }
            virtual int filepagesaving(twain_source&) { return 1; }
            virtual int processeddibfinal(twain_source&) { return 1; }
            virtual int manualduplexside1start(twain_source&) { return 1; }
            virtual int manualduplexside2start(twain_source&) { return 1; }
            virtual int manualduplexside1done(twain_source&) { return 1; }
            virtual int manualduplexside2done(twain_source&) { return 1; }
            virtual int manualduplexmergeerror(twain_source&) { return 1; }
            virtual int manualduplexcounterror(twain_source&) { return 1; }
            virtual int manualduplexmemoryerror(twain_source&) { return 1; }
            virtual int manualduplexfileerror(twain_source&) { return 1; }
            virtual int manualduplexfilesaveerror(twain_source&) { return 1; }
            virtual int blankpagedetected_original(twain_source&) { return 1; }
            virtual int blankpagedetected_resampled(twain_source&) { return 1; }
            virtual int blankpagediscardedoriginal(twain_source&) { return 1; }
            virtual int blankpagediscardedadjusted(twain_source&) { return 1; }
            virtual int filenamechanging(twain_source&) { return 1; }
            virtual int filenamechanged(twain_source&) { return 1; }
            virtual int tripletbegin(twain_source&) { return 1; }
            virtual int tripletend(twain_source&) { return 1; }

        public:
            twain_callback() : m_UserData(0), m_bDefaultHandler(false), m_nNotificationID(0), m_functionMap({
                {twain_callback_values::DTWAIN_PREACQUIRE_START, &twain_callback::preacquire},
                {twain_callback_values::DTWAIN_PREACQUIRE_TERMINATE, &twain_callback::preacquire_terminate},
                {DTWAIN_TN_ACQUIREDONE, &twain_callback::acquiredone},
                {DTWAIN_TN_ACQUIREFAILED, &twain_callback::acquirefailed},
                {DTWAIN_TN_ACQUIRECANCELLED, &twain_callback::acquirecancelled},
                {DTWAIN_TN_ACQUIRESTARTED, &twain_callback::acquirestarted},
                {DTWAIN_TN_PAGECONTINUE, &twain_callback::pagecontinue},
                {DTWAIN_TN_PAGEFAILED, &twain_callback::pagefailed},
                {DTWAIN_TN_PAGECANCELLED, &twain_callback::pagecancelled},
                {DTWAIN_TN_TRANSFERREADY, &twain_callback::transferready},
                {DTWAIN_TN_TRANSFERDONE, &twain_callback::transferdone},
                {DTWAIN_TN_UICLOSING, &twain_callback::uiclosing},
                {DTWAIN_TN_UICLOSED, &twain_callback::uiclosed},
                {DTWAIN_TN_UIOPENED, &twain_callback::uiopened},
                {DTWAIN_TN_CLIPTRANSFERDONE, &twain_callback::cliptransferdone},
                {DTWAIN_TN_INVALIDIMAGEFORMAT, &twain_callback::invalidimageformat},
                {DTWAIN_TN_ACQUIRETERMINATED, &twain_callback::acquireterminated},
                {DTWAIN_TN_TRANSFERSTRIPREADY, &twain_callback::transferstripready},
                {DTWAIN_TN_TRANSFERSTRIPDONE, &twain_callback::transferstripdone},
                {DTWAIN_TN_TRANSFERSTRIPFAILED, &twain_callback::transferstripfailed},
                {DTWAIN_TN_IMAGEINFOERROR, &twain_callback::imageinfoerror},
                {DTWAIN_TN_TRANSFERCANCELLED, &twain_callback::transfercancelled},
                {DTWAIN_TN_FILESAVECANCELLED, &twain_callback::filesavecancelled},
                {DTWAIN_TN_FILESAVEOK, &twain_callback::filesaveok},
                {DTWAIN_TN_FILESAVEERROR, &twain_callback::filesaveerror},
                {DTWAIN_TN_FILEPAGESAVEOK, &twain_callback::filepagesaveok},
                {DTWAIN_TN_FILEPAGESAVEERROR, &twain_callback::filepagesaveerror},
                {DTWAIN_TN_PROCESSEDDIB, &twain_callback::processeddib},
                {DTWAIN_TN_DEVICEEVENT, &twain_callback::deviceevent},
                {DTWAIN_TN_ENDOFJOBDETECTED, &twain_callback::eojdetected},
                {DTWAIN_TN_EOJDETECTED_XFERDONE, &twain_callback::eojdetectedtransferdone},
                {DTWAIN_TN_TWAINPAGECANCELLED, &twain_callback::twainpagecancelled},
                {DTWAIN_TN_TWAINPAGEFAILED, &twain_callback::twainpagefailed},
                {DTWAIN_TN_QUERYPAGEDISCARD, &twain_callback::querypagediscard},
                {DTWAIN_TN_PAGEDISCARDED, &twain_callback::pagediscarded},
                {DTWAIN_TN_APPUPDATEDDIB, &twain_callback::appupdateddib},
                {DTWAIN_TN_FILEPAGESAVING, &twain_callback::filepagesaving},
                {DTWAIN_TN_PROCESSEDDIBFINAL, &twain_callback::processeddibfinal},
                {DTWAIN_TN_MANDUPSIDE1START, &twain_callback::manualduplexside1start},
                {DTWAIN_TN_MANDUPSIDE2START, &twain_callback::manualduplexside2start},
                {DTWAIN_TN_MANDUPSIDE1DONE, &twain_callback::manualduplexside1done},
                {DTWAIN_TN_MANDUPSIDE2DONE, &twain_callback::manualduplexside2done},
                {DTWAIN_TN_MANDUPMERGEERROR, &twain_callback::manualduplexmergeerror},
                {DTWAIN_TN_MANDUPPAGECOUNTERROR, &twain_callback::manualduplexcounterror},
                {DTWAIN_TN_MANDUPMEMORYERROR, &twain_callback::manualduplexmemoryerror},
                {DTWAIN_TN_MANDUPFILEERROR, &twain_callback::manualduplexfileerror},
                {DTWAIN_TN_MANDUPFILESAVEERROR, &twain_callback::manualduplexfilesaveerror},
                {DTWAIN_TN_BLANKPAGEDETECTED1, &twain_callback::blankpagedetected_original},
                {DTWAIN_TN_BLANKPAGEDETECTED2, &twain_callback::blankpagedetected_resampled},
                {DTWAIN_TN_BLANKPAGEDISCARDED1, &twain_callback::blankpagediscardedoriginal},
                {DTWAIN_TN_BLANKPAGEDISCARDED2, &twain_callback::blankpagediscardedadjusted},
                {DTWAIN_TN_FILENAMECHANGING, &twain_callback::filenamechanging},
                {DTWAIN_TN_FILENAMECHANGED, &twain_callback::filenamechanged},
                {DTWAIN_TN_UIOPENFAILURE, &twain_callback::uiopenfailure},
                {DTWAIN_TN_TWAINTRIPLETBEGIN, &twain_callback::tripletbegin},
                {DTWAIN_TN_TWAINTRIPLETEND, &twain_callback::tripletend}
                })
            {}

            LRESULT call_func(WPARAM wParm, LPARAM lParm, twain_source* pSource);
            void enable_triplet_notify(bool bSet) { m_bEnableTripletNotification = bSet; }
            virtual ~twain_callback() = default;
        };
    }
}
#endif