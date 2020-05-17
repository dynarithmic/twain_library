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

std::queue<MSG> TwainMessageLoopV2::s_MessageQueue;

bool TwainMessageLoopImpl::IsSourceOpen(CTL_ITwainSource* pSource, bool bUIOnly)
{
    if (bUIOnly)
        return pSource->IsUIOpen() ? true : false;
    return (!m_pDLLHandle->m_bTransferDone == true && !m_pDLLHandle->m_bSourceClosed == true);
}

void TwainMessageLoopWindowsImpl::PerformMessageLoop(CTL_ITwainSource *pSource, bool isUIOnly)
{
    MSG msg;
    struct UIScopedRAII
    {
        CTL_ITwainSource* m_pSource;
        bool m_bOld;
        UIScopedRAII(CTL_ITwainSource* pSource) : m_pSource(pSource), m_bOld(m_pSource->IsUIOnly()) {}
        ~UIScopedRAII() { m_pSource->SetUIOnly(m_bOld); }
    };

    UIScopedRAII raii(pSource);
    pSource->SetUIOnly(isUIOnly);
#ifdef WIN32
    while (GetMessage(&msg, NULL, 0, 0))
    {
		if (!IsSourceOpen(pSource, isUIOnly))
			break;
        if (CanEnterDispatch(&msg))
        {
            ::TranslateMessage(&msg);
            ::DispatchMessage(&msg);
        }
    }
#else
    while (IsSourceOpen(pSource, isUIOnly))
    {
        CanEnterDispatch(&msg);
    }
#endif
}

TW_UINT16 TW_CALLINGSTYLE TwainMessageLoopV2::TwainVersion2MsgProc(pTW_IDENTITY , pTW_IDENTITY, TW_UINT32, TW_UINT16, TW_UINT16 MSG_, TW_MEMREF)
{
    MSG msg = MSG();
    msg.message = MSG_;
    s_MessageQueue.push(msg);
    return TWRC_SUCCESS;
}

bool TwainMessageLoopV2::IsSourceOpen(CTL_ITwainSource* pSource, bool bUIOnly)
{
    return !s_MessageQueue.empty() || TwainMessageLoopImpl::IsSourceOpen(pSource, bUIOnly);
}

void dynarithmic::DTWAIN_AcquireProc(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE, WPARAM Data1, LPARAM)
{
    CTL_TwainDLLHandle *p = static_cast<CTL_TwainDLLHandle *>(DLLHandle);

    switch (Data1)
    {
        case DTWAIN_TN_ACQUIRESTARTED:
            p->m_bTransferDone = false;
            p->m_bSourceClosed = false;
            p->m_lLastAcqError = 0;
            break;

        case DTWAIN_TN_ACQUIREDONE:
            p->m_lLastAcqError = DTWAIN_TN_ACQUIREDONE;
            break;

        case DTWAIN_TN_ACQUIREFAILED:
            p->m_lLastAcqError = DTWAIN_TN_ACQUIREFAILED;
            break;

        case DTWAIN_TN_ACQUIRECANCELLED:
            p->m_lLastAcqError = DTWAIN_TN_ACQUIRECANCELLED;
            break;

        case DTWAIN_AcquireSourceClosed:
            break;

        case DTWAIN_TN_UICLOSED:
            if (p->m_lLastAcqError == 0)
                p->m_lLastAcqError = DTWAIN_TN_ACQUIRECANCELLED;
            break;

        case DTWAIN_AcquireTerminated:
            p->m_bTransferDone = true;
            p->m_bSourceClosed = true;
            if (p->m_lLastAcqError == 0)
                p->m_lLastAcqError = DTWAIN_TN_ACQUIRECANCELLED;
            break;
    }
}
