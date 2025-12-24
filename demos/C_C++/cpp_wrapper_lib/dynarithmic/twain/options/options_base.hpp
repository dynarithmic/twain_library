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
#ifndef DTWAIN_OPTIONS_BASE_HPP
#define DTWAIN_OPTIONS_BASE_HPP

namespace dynarithmic
{
    namespace twain
    {
        class twain_source;
        class pages_options;
        class jobcontrol_options;
        class paperhandling_options;
        class imagetype_options;
        class file_transfer_options_ex;
        class compression_options;
        class userinterface_options;
        class language_options;
        class deviceparams_options;
        class powermonitor_options;
        class doublefeed_options;
        class autoadjust_options;
        class barcodedetection_options;
        class patchcode_options;
        class autocapture_options;
        class imageinformation_options;
        class imageparameter_options;
        class audiblealarms_options;
        class deviceevent_options;
        class resolution_options;
        class color_options;
        class capnegotiation_options;
        class micr_options;
        class imprinter_options;
        class autoscanning_options;

        class options_base
        {
            public:
                static void apply(twain_source& ts, pages_options& po);
                static void apply(twain_source& ts, jobcontrol_options& jc);
                static void apply(twain_source& ts, paperhandling_options& po);
                static void apply(twain_source& ts, imagetype_options& io);
                static void apply(twain_source& ts, file_transfer_options_ex& fo);
                static void apply(twain_source& ts, compression_options& co);
                static void apply(twain_source& ts, userinterface_options& ui);
                static void apply(twain_source& ts, language_options& lo);
                static void apply(twain_source& ts, deviceparams_options& dp);
                static void apply(twain_source& ts, powermonitor_options& po);
                static void apply(twain_source& ts, doublefeed_options& df);
                static void apply(twain_source& ts, autoadjust_options& ao);
                static void apply(twain_source& ts, barcodedetection_options& bo);
                static void apply(twain_source& ts, patchcode_options& pc);
                static void apply(twain_source& ts, autocapture_options& ac);
                static void apply(twain_source& ts, imageinformation_options& io);
                static void apply(twain_source& ts, imageparameter_options& io);
                static void apply(twain_source& ts, audiblealarms_options& aa);
                static void apply(twain_source& ts, deviceevent_options& dopt);
                static void apply(twain_source& ts, resolution_options& ro);
                static void apply(twain_source& ts, color_options& co);
                static void apply(twain_source& ts, capnegotiation_options& co);
                static void apply(twain_source& ts, micr_options& mo);
                static void apply(twain_source& ts, imprinter_options& io);
                static void apply(twain_source& ts, autoscanning_options& ao);
        };
    }
}
#endif
