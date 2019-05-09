/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2019 Dynarithmic Software.

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
#include <cstring>
#include "dtwain.h"
#include "ctltr008.h"
#include "ctltr027.h"
#include "ctltr034.h"
#include "ctltwses.h"
#include <ctltwsrc.h>
#include "ctltwmgr.h"

using namespace dynarithmic;

// Copied from CTLIFACE.H
#define  DTWAIN_SelectSourceFailed                1016
#define  DTWAIN_AcquireSourceClosed               1017

/* Transfer started */
/* Scanner already has physically scanned a page.
 This is sent only once (when TWAIN actually does the transformation of the
 scanned image to the DIB) */
#define  DTWAIN_TWAINAcquireStarted               1019

/* Sent when DTWAIN_Acquire...() functions are about to return */
#define  DTWAIN_AcquireTerminated                 1020

CTL_ProcessEventTriplet::CTL_ProcessEventTriplet(CTL_ITwainSession *pSession,
                                                 const CTL_ITwainSource* pSource,
                                                 MSG *pMsg,
                                                 bool isDSM2) : m_bDSM2Used(isDSM2)
{
    SetSourcePtr((CTL_ITwainSource*)pSource);
    SetSessionPtr(pSession);
    m_pMsg = pMsg;

    memset(&m_Event, 0, sizeof(TW_EVENT));
    m_Event.pEvent      = (TW_MEMREF)pMsg;
    m_Event.TWMessage   = MSG_NULL;
    if (m_bDSM2Used)
        m_Event.TWMessage = static_cast<TW_UINT16>(pMsg->message);

    // Get the app manager's AppID
    const CTL_TwainAppMgrPtr pMgr = CTL_TwainAppMgr::GetInstance();

    if (pMgr && pMgr->IsValidTwainSession(pSession))
    {
        if (pSource)
        {
            Init(pSession->GetAppIDPtr(), *pSource, DG_CONTROL, DAT_EVENT,
                MSG_PROCESSEVENT, (TW_MEMREF)&m_Event);
            SetAlive(true);
        }
    }
}


TW_UINT16 CTL_ProcessEventTriplet::ExecuteEventHandler()
{
    static int nCount = 0;
    TW_UINT16 rc = TWRC_SUCCESS;

    // if we are doing a legacy TWAIN call, then we need to
    // call the DSM
    if (!m_bDSM2Used)
        rc = CTL_TwainTriplet::Execute();

    CTL_ITwainSession* pSession;
    CTL_ITwainSource* pSource;
    bool bNextAttemptIsRetry = false;

    pSession = GetSessionPtr();
    pSource = GetSourcePtr();

    if ( 1 ) //rc == TWRC_SUCCESS )
    {
        HWND hWnd =*pSession->GetWindowHandlePtr();
        unsigned int nMsg = CTL_TwainAppMgr::GetRegisteredMsg();

       // Check message from source
        switch (m_Event.TWMessage)
        {
            case MSG_XFERREADY:
            {
                // Set the retry count
                pSource->SetCurrentRetryCount(0);

                pSource->SetState(SOURCE_STATE_XFERREADY);
                // Set the transfer mechanism (??)
                // Set the pixel type and bit depth based on what the current values
                // found in the Source.
                CTL_TwainAppMgr::SetPixelAndBitDepth(pSource);

                // Send this message to the Twain window(s)
                #ifdef _WIN32
                ::SendMessage(hWnd, nMsg, MSG_XFERREADY, 0 );
                #endif
                // Remove Dibs if already scanned
                pSource->Reset();

                // Loop for all documents installed
                int  bPending = 1;
                nCount = 0;
                pSource->SetPendingJobNum(0);
                while (bPending != 0)
                {
                    // Get the image information
                    if ( !bNextAttemptIsRetry )
                    {
                        if (!CTL_TwainAppMgr::GetImageInfo(pSource))
                        {
                            if ( !pSource->SkipImageInfoErrors() )
                            {
                                CTL_TwainAppMgr::WriteLogInfo(_T("Invalid Image Information on acquiring image"));
                                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                                      NULL,
                                                                      DTWAIN_TN_IMAGEINFOERROR,
                                                                      (LPARAM)pSource);
                                break;
                            }
                        }
                        else
                        {
                        }
                    }
                    // Let TWAIN initiate the transfer
                    // Send message that acquire has started if nCount is 0
                    if ( nCount == 0 && !bNextAttemptIsRetry )
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                              NULL,
                                                              DTWAIN_TWAINAcquireStarted,
                                                              (LPARAM)pSource);

                    // Send message if nCount is 0 and manual duplex is on, and side 1 is
                    // being acquired
                    if ( nCount == 0 && pSource->IsManualDuplexModeOn() &&
                         pSource->GetCurrentSideAcquired() == 0 )
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                              NULL,
                                                              DTWAIN_TN_MANDUPSIDE1START,
                                                              (LPARAM)pSource);

                    // Also send that the acquisition is ready (this is sent for every page)
                    bool bContinue = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL,
                                                          DTWAIN_TN_TRANSFERREADY,
                                                          (LPARAM)pSource)?true:false;
                    if ( !bContinue )
                    {
                        // Transfer aborted by callback
                        ResetTransfer();

                        // Send a message to close things down if
                        // there was no user interface chosen
                        if ( !pSource->IsUIOpenOnAcquire() )
                            CTL_TwainAppMgr::EndTwainUI(pSession, pSource);

                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                              NULL,
                                                              DTWAIN_TN_TRANSFERCANCELLED,
                                                              (LPARAM)pSource);
                        bPending = 0;
                    }
                    else
                    {
                        bNextAttemptIsRetry = false;
                        bPending = CTL_TwainAppMgr::TransferImage(pSource, nCount);
                        if ( bPending != -1 )  // Only if aborting or images have been retrieved
                            nCount++;
                        else
                            bNextAttemptIsRetry = true;
                    }
                }
                pSource->SetState(SOURCE_STATE_UIENABLED);
                if ( !pSource->GetTransferDone() && nCount <= 1 )
                {
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL,
                                                          DTWAIN_TN_ACQUIREDONE_EX,
                                                          (LPARAM)pSource);

                    break;  // No transfer occurred.  Cancellation or Failure occurred
                }
                if ( nCount > 0)
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL,
                                                          DTWAIN_TN_ACQUIREDONE,
                                                          (LPARAM)pSource);
            }
            break;

            case MSG_CLOSEDSREQ:
            case MSG_CLOSEDSOK:
            {
                // The source UI must be closed
                if ( pSource->IsUIOpen())
                {
                   CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                         NULL,
                                                         DTWAIN_TN_UICLOSING,
                                                         (LPARAM)pSource);

                        CTL_TwainAppMgr::DisableUserInterface(pSource);

                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL,
                                                          DTWAIN_TN_UICLOSED,
                                                          (LPARAM)pSource);
                }
            }
            break;

            // Possible device event
            case MSG_DEVICEEVENT:
            {
                // Some dude has changed something on the device!!
                // Get the change
                CTL_DeviceEventTriplet DevTrip(pSession, pSource);
                DevTrip.Execute();
                if ( DevTrip.IsSuccessful() )
                {
                    pSource->SetDeviceEvent( DevTrip.GetDeviceEvent() );
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_DEVICEEVENT,(LPARAM)pSource);

                    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle*>(GetDTWAINHandle_Internal());
                    // if there is a callback, call it now with the error notifications
                    if ( pHandle->m_pCallbackFn )
                    {
                        static_assert(sizeof(pSource) <= sizeof(LONG_PTR),"pointer sizes are different");
                        UINT uMsg = CTL_TwainDLLHandle::s_nRegisteredDTWAINMsg;
                        LogDTWAINMessage(NULL, uMsg, DTWAIN_TN_DEVICEEVENT, 0, true);
                        #ifdef WIN64
                            (*pHandle->m_pCallbackFn)(DTWAIN_TN_DEVICEEVENT, 0, (LONG_PTR)pSource);
                        #else
                            (*pHandle->m_pCallbackFn)(DTWAIN_TN_DEVICEEVENT, 0, (LONG_PTR)pSource);
                        #endif
                    }

                    // if there is a 64-bit callback, call it now with the error notifications
                    if ( pHandle->m_pCallbackFn64 )
                    {
                        UINT uMsg = CTL_TwainDLLHandle::s_nRegisteredDTWAINMsg;
                        LogDTWAINMessage(NULL, uMsg, DTWAIN_TN_DEVICEEVENT, 0, true);
                        (*pHandle->m_pCallbackFn64)(DTWAIN_TN_DEVICEEVENT, 0, (LONGLONG)pSource);
                    }
                }
            }
            break;

            case MSG_NULL:
            break;
        }
    }
    return rc;
}


bool CTL_ProcessEventTriplet::ResetTransfer(TW_UINT16 Msg/*=MSG_RESET*/)
{
    CTL_ITwainSession* pSession = GetSessionPtr();
    CTL_ImagePendingTriplet Pending(pSession,
                                    GetSourcePtr(),
                                    Msg);
    TW_UINT16 rc = Pending.Execute();

    switch( rc )
    {
        case TWRC_SUCCESS:
            return true;

        case TWRC_FAILURE:
        {
            TW_UINT16 ccode = CTL_TwainAppMgr::GetConditionCode(pSession, NULL);
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, TWRC_FAILURE, ccode);
            return false;
        }
        break;
    }
    return false;
}


