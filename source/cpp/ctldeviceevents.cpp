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
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;

////////////////////////////////////////////////////////////////////////////////////////////
// Device notifications
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetDeviceNotifications(DTWAIN_SOURCE Source, LONG DeviceEvents)
{
    LOG_FUNC_ENTRY_PARAMS((Source, DeviceEvents))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{ return (!CTL_TwainAppMgr::IsSourceOpen(pSource)); },
    DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    // See if Source supports the DEVICEEVENTS capability
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (!DTWAIN_IsCapSupported(pSource, DTWAIN_CV_CAPDEVICEEVENT)); },
    DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED, false, FUNC_MACRO);

    // Set the notifications
    DTWAIN_ARRAY Array = 0;
    DTWAINArrayPtr_RAII a(&Array);

    LONG SetType = DTWAIN_CAPSET;
    if (!DeviceEvents)
        SetType = DTWAIN_CAPRESET;
    else
    {
        LONG nBits = 0;
        LONG i;
        for (i = 0; i < 32; i++)
        {
            if (DeviceEvents & (1L << i))
                nBits++;
        }
        if (nBits == 0)
            LOG_FUNC_EXIT_PARAMS(false)

        Array = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, nBits);

        if (!Array)
            LOG_FUNC_EXIT_PARAMS(false)

        auto& vValues = EnumeratorVector<LONG>(Array);
        LONG nIndex = 0;

        for (i = 0; i < 32; i++)
        {
            if (DeviceEvents & (1L << i))
            {
                vValues[nIndex] = i;
                ++nIndex;
            }
        }
    }
    bool bRet = DTWAIN_SetCapValues(Source, DTWAIN_CV_CAPDEVICEEVENT, SetType, Array)?true:false;
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceNotifications(DTWAIN_SOURCE Source, LPLONG lpDeviceEvents)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lpDeviceEvents))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    DTWAIN_ARRAY Array = 0;
    DTWAINArrayPtr_RAII raii(&Array);

    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (!CTL_TwainAppMgr::IsSourceOpen(pSource)); },
        DTWAIN_ERR_SOURCE_NOT_OPEN, false, FUNC_MACRO);

    // See if Source supports the DEVICEEVENTS capability
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return (!DTWAIN_IsCapSupported(pSource, DTWAIN_CV_CAPDEVICEEVENT)); },
                                        DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED, false, FUNC_MACRO);

    bool bRet = DTWAIN_GetCapValues(Source, DTWAIN_CV_CAPDEVICEEVENT, DTWAIN_CAPGETCURRENT, &Array) ? true : false;
    if (!bRet)
        LOG_FUNC_EXIT_PARAMS(false)

    *lpDeviceEvents = 0L;
    auto& vValues = EnumeratorVector<LONG>(Array);
    for_each(vValues.begin(), vValues.end(), [&](LONG Value)
    {
        if (Value < 32 && Value > 0L)
            *lpDeviceEvents |= 1L << (Value - 1L);
    });
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceEvent(DTWAIN_SOURCE Source, LPLONG lpEvent)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lpEvent))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    CTL_DeviceEvent DeviceEvent = pSource->GetDeviceEvent();
    *lpEvent = DeviceEvent.GetEvent() + 1;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceEventEx(DTWAIN_SOURCE Source, LPLONG lpEvent, LPDTWAIN_ARRAY pArray)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lpEvent, pArray))
    if (!DTWAIN_GetDeviceEvent(Source, lpEvent))
        LOG_FUNC_EXIT_PARAMS(false)
    if (!pArray)
        LOG_FUNC_EXIT_PARAMS(true)

    CTL_ITwainSource* pSource = (CTL_ITwainSource *)Source;
    CTL_DeviceEvent DeviceEvent = pSource->GetDeviceEvent();
    DTWAIN_BOOL bRet = DeviceEvent.GetEventInfoEx(pArray);
    LOG_FUNC_EXIT_PARAMS(bRet)
        CATCH_BLOCK(false)
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceEventInfo(DTWAIN_SOURCE Source, LONG nWhichInfo, LPVOID pValue)
{
    LOG_FUNC_ENTRY_PARAMS((Source, nWhichInfo, pValue))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *pSource = VerifySourceHandle(pHandle, Source);
    if (!pSource)
        LOG_FUNC_EXIT_PARAMS(false)

    CTL_DeviceEvent DeviceEvent = pSource->GetDeviceEvent();
    switch (nWhichInfo)
    {
        case DTWAIN_GETDE_EVENT:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetEvent() + 1;
        }
        break;

        case DTWAIN_GETDE_DEVNAME:
        {
            char FAR *s = (char FAR *)pValue;
            strcpy(s, StringConversion::Convert_Native_To_Ansi(DeviceEvent.GetDeviceName()).c_str());
        }
        break;

        case DTWAIN_GETDE_BATTERYMINUTES:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetBatteryMinutes();
        }
            break;

        case DTWAIN_GETDE_BATTERYPCT:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetBatteryPercentage();
        }
            break;

        case DTWAIN_GETDE_XRESOLUTION:
        {
            double *p = (double *)pValue;
            *p = DeviceEvent.GetXResolution();
        }
        break;

        case DTWAIN_GETDE_YRESOLUTION:
        {
            double *p = (double *)pValue;
            *p = DeviceEvent.GetYResolution();
        }
            break;

        case DTWAIN_GETDE_FLASHUSED:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetFlashUsed2();
        }
            break;

        case DTWAIN_GETDE_AUTOCAPTURE:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetAutomaticCapture();
        }
        break;

        case DTWAIN_GETDE_TIMEBEFORECAPTURE:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetTimeBeforeFirstCapture();
        }
        break;

        case DTWAIN_GETDE_TIMEBETWEENCAPTURES:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetTimeBetweenCaptures();
        }
        break;

        case DTWAIN_GETDE_POWERSUPPLY:
        {
            LPLONG p = (LPLONG)pValue;
            *p = DeviceEvent.GetPowerSupply();
        }
        break;

    default:
        DTWAIN_Check_Error_Condition_0_Ex(pHandle, []{return true; }, DTWAIN_ERR_INVALID_PARAM, false, FUNC_MACRO);
    }
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}
