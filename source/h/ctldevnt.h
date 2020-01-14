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
#ifndef CTLDEVNT_H_
#define CTLDEVNT_H_

#include "ctltwsrc.h"
namespace dynarithmic
{
    class CTL_DeviceEvent
    {
        public:
            CTL_DeviceEvent();
            operator pTW_DEVICEEVENT() { return &m_DeviceEvent; }

            TW_UINT32  GetEvent() const;                  /* One of the TWDE_xxxx values. */
            CTL_StringType GetDeviceName() const;             /* The name of the device that generated the event */
            TW_UINT32  GetBatteryMinutes() const;         /* Battery Minutes Remaining    */
            TW_INT16   GetBatteryPercentage() const;      /* Battery Percentage Remaining */
            TW_INT32   GetPowerSupply() const;            /* Power Supply                 */
            double     GetXResolution() const;            /* Resolution                   */
            double     GetYResolution() const;            /* Resolution                   */
            TW_UINT32  GetFlashUsed2() const;             /* Flash Used2                  */
            TW_UINT32  GetAutomaticCapture() const;       /* Automatic Capture            */
            TW_UINT32  GetTimeBeforeFirstCapture() const; /* Automatic Capture            */
            TW_UINT32  GetTimeBetweenCaptures() const;    /* Automatic Capture            */

            bool       GetEventInfoEx(DTWAIN_ARRAY Array) const;

        private:
            TW_DEVICEEVENT  m_DeviceEvent;
    };
}
#endif
