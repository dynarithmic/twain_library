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
#include <bitset>
#include <boost/lexical_cast.hpp>
#include <boost/format.hpp>
#include <boost/filesystem.hpp>
#include <fstream>
#include "ctltr010.h"
#include "ctltr026.h"
#include "ctltr027.h"
#include "ctltr025.h"
#include "ctltwmgr.h"
#include "imagexferfilewriter.h"
#include "ctldib.h"
#include "dtwain.h"
#include "enumeratorfuncs.h"
#include "ctlfileutils.h"

using namespace dynarithmic;

#define DEMO_MAX_PAGES 10000

#ifdef USE_NAMESPACES
   using namespace std;
#endif

static void SendFileAcquireError(CTL_ITwainSource *pSource, CTL_ITwainSession *pSession,
                                 LONG Error, LONG ErrorMsg );
static bool IsState7InfoNeeded(CTL_ITwainSource *pSource);

void ResolveImageResolution(CTL_ITwainSource *pSource,  DTWAINImageInfoEx* ImageInfo);

CTL_ImageXferTriplet::CTL_ImageXferTriplet(CTL_ITwainSession *pSession,
                                           CTL_ITwainSource* pSource,
                                           TW_UINT16 nType)
                     :  CTL_ImageTriplet(pSession, pSource),
                     m_hDib(nullptr),
                     m_nTotalPagesSaved(0),
                     m_bJobControlPageRecorded(false),
                     m_bJobMarkerNeedsToBeWritten(false),
                     m_bScanPending(true),
                     m_nTotalPages(0),
                     m_nTransferType(nType),
                     m_nFailAction(0),
                     m_bPendingXfersDone(false),
                     m_lastPendingXferCode(0),
					 m_pImgHandler(nullptr),
					 m_PendingXfers{}
{
    switch( nType )
    {
        case DAT_IMAGENATIVEXFER:
            InitVars(nType, CTL_GetTypeGET, (void *)&m_hDib);
        break;
        case DAT_IMAGEFILEXFER:
            InitVars(nType, CTL_GetTypeGET, NULL );
        break;
    }
}


TW_UINT16 CTL_ImageXferTriplet::Execute()
{
    // Check if document feeder is to be used.  If it can, check if there
    // really is a Document feeder

    // Get the app manager's AppID
    int errfile = 0;
    CTL_ITwainSource *pSource = GetSourcePtr();
    CTL_ITwainSession *pSession = GetSessionPtr();
    TW_UINT16 rc = CTL_ImageTriplet::Execute();
    bool    bInClip = false;
    bool    bPageDiscarded = false;
    bool    bProcessDibEx = true;
    bool    bKeepPage = true;
    size_t nLastDib = 0;

    ImageXferFileWriter FileWriter(this, pSession, pSource);

    m_bJobControlPageRecorded = false;

    switch (rc)
    {
        case TWRC_XFERDONE:
        {
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,
                                                  DTWAIN_TN_TRANSFERDONE,
                                                  (LPARAM)pSource);
            m_bJobMarkerNeedsToBeWritten = false;
            // Check if more images are pending (job control only)
            SetPendingXfersDone(false);

            bool bEndOfJobDetected=false;

            // Get the current page number of image being transferred
            size_t nCurImage = pSource->GetPendingImageNum();

            // See if we need to check the job control status via TWAIN
            if ( pSource->IsUIOpenOnAcquire() && nCurImage == 0 )
            {
                LONG JobControl;
                // Get the current job control if the user may have changed it
                // in the UI of the TWAIN driver
                DTWAINScopedLogControllerExclude sLogerr(DTWAIN_LOG_ERRORMSGBOX);
                if ( DTWAIN_GetJobControl(pSource,&JobControl, TRUE) != FALSE )
                    pSource->SetCurrentJobControl( (TW_UINT16)JobControl );
            }

            if ( pSource->GetCurrentJobControl() != TWJC_NONE )
            {
                bool bSuccess = false;
                if ( pSource->IsTwainJobControl())
                {
                    TW_PENDINGXFERS& Pending = GetLocalPendingXferInfo();
                    SetLastPendingInfoCode(GetImagePendingInfo(&Pending));
                    if (GetLastPendingInfoCode() == TWRC_SUCCESS)
                    {
                        bSuccess = true;
                        // Indicate that pending xfers has been executed
                        SetPendingXfersDone(true);
                        bEndOfJobDetected = ( Pending.EOJ == (TW_UINT32)pSource->GetEOJDetectedValue());
                    }
                }
                else
                {
                    // temporary
                    CTL_TwainDib theDib;
                    theDib.SetHandle(m_hDib);
                    bEndOfJobDetected = theDib.IsBlankDIB(pSource->GetBlankPageThreshold());
                    bSuccess = true;
                }
                if ( bSuccess )
                {
                    if ( bEndOfJobDetected )
                    {
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_EOJDETECTED_XFERDONE,(LPARAM)pSource);
                        // Now check if special job control file handling is done
                        if ( pSource->IsJobFileHandlingOn() &&
                            !pSource->CurrentJobIncludesPage())
                        {
                            // Indicate that a job control marker
                            // needs to be written after the real page has
                            // been written
                            m_bJobMarkerNeedsToBeWritten = true;
                        }
                    }
                    else
                    if ( nCurImage == 0 )
                    {
                        // Write a job control marker now.  This is the start of a
                        // new job.
                        FileWriter.CopyDuplexDibToFile(CTL_TwainDibPtr(), true);
                    }
                }
            }

            bool bExecuteEOJPageHandling = bEndOfJobDetected && pSource->IsJobFileHandlingOn();

            pSource->SetTransferDone(true);
            // Store DIB in object

            // Get the image if native transfer
            CTL_TwainDibPtr CurDib;
            CTL_TwainDibArray* pArray = NULL;
            if (m_nTransferType == DAT_IMAGENATIVEXFER)
            {
                // Get the array of current array of DIBS (this pointer allows changes to Source's internal DIB array)
                pArray = pSource->GetDibArray();

                // Let Source set the handle (Source knows if this is a new or old DIB to replace)
                // Emit an error if m_hDib is NULL
                if (!m_hDib)
                {
                    bPageDiscarded = true;
                    // Set the error code
                    CTL_TwainAppMgr::SetError(-DTWAIN_ERR_BAD_DIB_PAGE);
                    TCHAR szBuf[255];
                    CTL_TwainAppMgr::GetLastErrorString(szBuf, 254);
                    CTL_TwainAppMgr::WriteLogInfo(szBuf);
                    break;
                }

                if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
                {
                    CTL_StringType sOut = _T("Original bitmap from device: \n");
                    sOut += CTL_ErrorStructDecoder().DecodeBitmap(m_hDib);
                    CTL_TwainAppMgr::WriteLogInfo(sOut);
                }

                pSource->SetDibHandle(m_hDib, nCurImage);

                // Get the dib from the array (must get last dib generated)
                nLastDib = pArray->GetSize() - 1;
                CurDib = pArray->GetAt(nLastDib);

                if (CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_PROCESSEDDIB,(LPARAM)pSource) == 0)
                {
                    // User does not want to process the image further.
                    // They are satisfied with the DIB as-is.
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_PROCESSDIBACCEPTED, (LPARAM)pSource);
                    bProcessDibEx = false;
                }

                // Here we can do a check for blank page.
                if (ProcessBlankPage(pSession, pSource, CurDib, DTWAIN_TN_BLANKPAGEDETECTED1,DTWAIN_TN_BLANKPAGEDISCARDED1, DTWAIN_BP_AUTODISCARD_IMMEDIATE) == 0)
                {
                    bPageDiscarded = true;
                    break;  // The page is discarded
                }

                // Callback function for access to change DIB
                if (CTL_TwainDLLHandle::s_pDibUpdateProc != NULL)
                {
                    HANDLE hRetDib =
                        (*CTL_TwainDLLHandle::s_pDibUpdateProc)
                        (pSource, (LONG)nLastDib, m_hDib);
                    if (hRetDib && (hRetDib != m_hDib))
                    {
                         // Application changed DIB.  So make this the current dib
                         #ifdef _WIN32
                        ::GlobalFree(m_hDib);
                        #endif
                        m_hDib = hRetDib;
                        pSource->SetDibHandle(m_hDib, nLastDib);
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_APPUPDATEDDIB, (LPARAM)pSource);
                    }
                }

                // Change bpp if necessary
                if (bProcessDibEx)
                    ModifyAcquiredDib();

                if (CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_PROCESSEDDIBFINAL, (LPARAM)pSource) == 0)
                {
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_PROCESSDIBFINALACCEPTED, (LPARAM)pSource);
                    // user is satisfied with the image, so break
                    bProcessDibEx = false;
                }

                if (ProcessBlankPage(pSession, pSource, CurDib, DTWAIN_TN_BLANKPAGEDETECTED2, DTWAIN_TN_BLANKPAGEDISCARDED2, DTWAIN_BP_AUTODISCARD_AFTERPROCESS) == 0)
                {
                    bPageDiscarded = true;
                    break;  // The page is discarded
                }

                // Now see if we want to keep the bitmap for purely native transfers (not file saves or file saves that use native transfers)
                // Query if the page should be thrown away
                bKeepPage = QueryAndRemoveDib(TWAINAcquireType_Native, nLastDib);
                if (!bKeepPage)
                    break;
            }

            // Use for native and native/file transfers
            switch( pSource->GetAcquireType() )
            {
                // Check if source acquire is file using native mode
                case TWAINAcquireType_FileUsingNative:
                {
                    pSource->SetPromptPending(false);
                    long lFlags   = pSource->GetAcquireFileFlags();
                    if ( lFlags & DTWAIN_USEPROMPT )
                    {
                        pSource->SetPromptPending(true);
                        break;
                    }

                    // resample the acquired dibs
                    ResampleAcquiredDib();

                    // Check if multi page file is being used
                    bool bIsMultiPageFile = CTL_ITwainSource::IsFileTypeMultiPage(pSource->GetAcquireFileType());
                    int nMultiStage = 0;

                    // Query if the page should be thrown away
                    bKeepPage = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_QUERYPAGEDISCARD, (LPARAM)pSource)?true:false;
                    // Keep the page

                    // For demo, limit to 2 pages acquired for multipage
                    #ifdef DTWAIN_DEMO_VERSION
                    if (nCurImage > DEMO_MAX_PAGES && bIsMultiPageFile ) { nCurImage = DEMO_MAX_PAGES; } else
                    #endif
                    if ( bKeepPage )
                    {
                        #ifdef DTWAIN_DEMO_VERSION
                        if (nCurImage > DEMO_MAX_PAGES && bIsMultiPageFile ) { nCurImage = DEMO_MAX_PAGES; } else
                        #endif
                        if ( bIsMultiPageFile || pSource->IsMultiPageModeSaveAtEnd())
                        {
                            // This is the first page of the acquisition
                            if ( nLastDib == 0 ||
                                (pSource->IsNewJob() && pSource->IsJobFileHandlingOn()))
                                nMultiStage = DIB_MULTI_FIRST;
                            else
                            // This is a subsequent page of the acquisition
                                nMultiStage = DIB_MULTI_NEXT;

                            // Now check if this we are in manual duplex mode
                            // or in continuous mode
                            if ( pSource->IsManualDuplexModeOn() ||
                                 pSource->IsMultiPageModeContinuous() ||
                               ( pSource->IsMultiPageModeSaveAtEnd() && !bIsMultiPageFile))
                            {
                                // We need to copy the data to a file and store info in
                                // vector of the source
                                if ( !bEndOfJobDetected || // Not end -of-job
                                    (bExecuteEOJPageHandling && !m_bJobControlPageRecorded ) // write job control page
                                    )
                                    errfile = FileWriter.CopyDuplexDibToFile(CurDib, bExecuteEOJPageHandling);

                                if ( !m_bJobControlPageRecorded && bExecuteEOJPageHandling)
                                    m_bJobControlPageRecorded = true;
                            }
                            else
                                errfile = FileWriter.CopyDibToFile(CurDib, nMultiStage, pSource->GetImageHandlerPtr(), 0);
                        }
                        else
                           errfile = FileWriter.CopyDibToFile(CurDib, nMultiStage, pSource->GetImageHandlerPtr(), 0);
                        m_nTotalPagesSaved++;
                    }
                    else
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_PAGEDISCARDED,(LPARAM)pSource);

                    // Delete temporary bitmap here
                    if ( pSource->IsDeleteDibOnScan() )
                    {
                        // Let array class handle deleting of the DIB (Global memory will be freed only)
                        pArray->DeleteDibMemory( nLastDib );
                    }
                }
                break;

                case TWAINAcquireType_File:
                {
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_PROCESSEDDIB,(LPARAM)pSource);
                    long lFlags = pSource->GetAcquireFileFlags();

                    if ( lFlags & TWAINFileFlag_PROMPT )
                    {
                        CTL_StringType strTempFile = pSource->PromptForFileName();
                        StringWrapper::TrimAll(strTempFile);
                        if ( strTempFile.empty())
                        {
                            SendFileAcquireError(pSource, pSession, DTWAIN_ERR_BAD_FILENAME, DTWAIN_TN_FILESAVECANCELLED );
                            CTL_StringType s;
                            pSource->SetLastAcquiredFileName( s );
                        }
                        else
                        {
                            // Copy default file name to the new file
                            if ( CTL_TwainAppMgr::CopyFile(pSource->GetAcquireFile(), strTempFile) != 1 )
                            {
                                // Error in copying the file
                                SendFileAcquireError(pSource, pSession, DTWAIN_ERR_FILEWRITE, DTWAIN_TN_FILESAVEERROR );
                                CTL_StringType s;
                                pSource->SetLastAcquiredFileName( s );
                            }
                            else
                                pSource->SetLastAcquiredFileName( strTempFile );

                            // Remove the temporary file
                            if (delete_file(pSource->GetAcquireFile().c_str()))
                                pSource->SetAcquireFile(_T(""));

                        }
                        pSource->SetLastAcquiredFileName( strTempFile );
                    }
                    else
                    {
                        // We can't get here if the file copy did not work, so assume success
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_FILESAVEOK,
                                                              (LPARAM)pSource->GetAcquireNum());
                    }
                }
                break;

                case TWAINAcquireType_Clipboard:
                    if ( pSource->GetSpecialTransferMode() == DTWAIN_USENATIVE )
                        bInClip = CopyDibToClipboard( pSession, m_hDib );
                break;
                default:
                    break;
            }

            if ( bInClip )
                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_CLIPTRANSFERDONE,(LPARAM)pSource->GetAcquireNum());

            if ( errfile != 0 )
               SendFileAcquireError(pSource, pSession, errfile, DTWAIN_TN_INVALIDIMAGEFORMAT);
            break;
        }

        case TWRC_CANCEL:
        {
            CancelAcquisition();
            break;
        }

        case TWRC_FAILURE:
        {
            m_hDib = NULL;
            FailAcquisition();
            return rc;
        }
        default:
        {
            CTL_StringStreamType strm;
            strm << _T("Unknown return code ") << rc << _T(" from DSM during transfer!  Twain driver unstable!\n");
            CTL_TwainAppMgr::WriteLogInfo(strm.str());
            m_hDib = NULL;
            break;
        }
    }

    // Prompt to save image here
    bool bRetval = true;
    bool bForceClose;

    pSource->SetBlankPageCount(pSource->GetBlankPageCount() + (bPageDiscarded?1:0));

    if ( !bPageDiscarded && pSource->IsPromptPending())
    {
        bRetval = PromptAndSaveImage(pSource->GetPendingImageNum())?true:false;
        pSource->SetPromptPending(false);
    }

    // Force a close if Prompting returned false.
    if ( bRetval == true )
        bForceClose = false;
    else
        bForceClose = true;
    AbortTransfer(bForceClose);
    return rc;

}

bool CTL_ImageXferTriplet::CancelAcquisition()
{
    CTL_ITwainSource* pSource = GetSourcePtr();
    CTL_ITwainSession* pSession = GetSessionPtr();

    pSource->SetState(SOURCE_STATE_UIENABLED); // Transition to state 6
    pSource->SetTransferDone(false);
    m_hDib = NULL;
    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,
        DTWAIN_TN_TWAINPAGECANCELLED,
        (LPARAM)pSource);

    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,
        DTWAIN_TN_PAGECANCELLED,
        (LPARAM)pSource);

    if (pSource->GetAcquireType() == TWAINAcquireType_FileUsingNative)
    {
        // Remove the image file
        ImageXferFileWriter(this, pSession, pSource).EndProcessingImageFile(
            pSource->IsFileIncompleteSave());
    }
    return true;
}


bool CTL_ImageXferTriplet::FailAcquisition()
{
    CTL_ITwainSource* pSource = GetSourcePtr();
    CTL_ITwainSession* pSession = GetSessionPtr();
    int bContinue;
    pSource->SetTransferDone(false);

    int nRetryMax = pSource->GetMaxRetryAttempts();
    int nCurRetry = pSource->GetCurrentRetryCount();

    pSource->SetState(SOURCE_STATE_UIENABLED); // Transition to state 6
    pSource->SetTransferDone(false);
    TW_UINT16 ccode = CTL_TwainAppMgr::GetConditionCode(pSession, NULL);
    CTL_TwainAppMgr::ProcessConditionCodeError(ccode);

    // Get what to do, either from user-notification or from default
    // TWAIN Window Proc
    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_TWAINPAGEFAILED, (LPARAM)pSource);

    bContinue = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_PAGEFAILED, (LPARAM)pSource);
    if (bContinue == DTWAIN_RETRY_EX || // Means not a user notification
        bContinue == 2)               // Means notifications are on and user wants
        // default behavior
    {
        // Check if retrying forever
        if (nRetryMax == DTWAIN_RETRY_FOREVER)
            bContinue = DTWAIN_PAGEFAIL_RETRY;
        else
        // Check if max retries have been reached
        if (nCurRetry == nRetryMax)
        {
            bContinue = DTWAIN_PAGEFAIL_TERMINATE;
        }
        else
        {
            // Increment retry count and try again
            pSource->SetCurrentRetryCount(++nCurRetry);
            bContinue = DTWAIN_PAGEFAIL_RETRY;
        }
    }
    ImageXferFileWriter(this, pSession, pSource).ProcessFailureCondition(bContinue);
    return true;
}

HANDLE  CTL_ImageXferTriplet::GetDibHandle()
{
    return m_hDib;
}

TW_UINT16 CTL_ImageXferTriplet::GetImagePendingInfo(TW_PENDINGXFERS *pPI, TW_UINT16 nMsg  /* =MSG_ENDXFER */)
{
    CTL_ImagePendingTriplet Pending(GetSessionPtr(),
                                    GetSourcePtr(),
                                    nMsg);
    TW_UINT16 rc = Pending.Execute();

    if ( rc == TWRC_SUCCESS )
        memcpy(pPI, Pending.GetPendingXferBuffer(), sizeof(TW_PENDINGXFERS));
    return rc;
}



bool CTL_ImageXferTriplet::AbortTransfer(bool bForceClose)
{
    CTL_TwainAppMgr::WriteLogInfo(_T("Potentially aborting transfer..\n"));
    CTL_ITwainSession *pSession = GetSessionPtr();
    CTL_ITwainSource *pSource = GetSourcePtr();
    ImageXferFileWriter FileWriter(this, pSession, pSource);

    TW_PENDINGXFERS pPending;
    TW_PENDINGXFERS *ptrPending;
    TW_UINT16 rc;
    if ( !IsPendingXfersDone() )
    {
        rc = GetImagePendingInfo( &pPending );
        ptrPending = &pPending;
    }
    else
    {
        rc = GetLastPendingInfoCode();
        ptrPending = &GetLocalPendingXferInfo();
    }

    int nContinue = 1;
    bool bUserCancelled = false;
    bool bJobControlContinue = false;
    bool bEndOfJobDetected;
    switch( rc )
    {
        case TWRC_SUCCESS:
            bEndOfJobDetected = ( pSource->GetCurrentJobControl() != TWJC_NONE &&
                                  ptrPending->EOJ == pSource->GetEOJDetectedValue());

            if ( bEndOfJobDetected )
                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                  NULL, DTWAIN_TN_ENDOFJOBDETECTED,
                                                  (LPARAM)GetSourcePtr());

            bJobControlContinue = IsJobControlPending(ptrPending);
            m_nTotalPages++;

            // Make sure that job control is "on"
            pSource->StartJob();

            if ( ptrPending->Count != 0 && (ptrPending->Count > 0 || bJobControlContinue)) // More to transfer
            {
                CTL_TwainAppMgr::WriteLogInfo(_T("More To Transfer...\n"));
                // Check if max pages has been reached.  Some Sources do not detect when
                // Count has been set to a specific number, so enforce the test here.
                if ( pSource->GetMaxAcquireCount() == pSource->GetPendingImageNum() + 1 )
                    nContinue = false;
                else
                if ( !bForceClose )
                {
                    // Send message to User App
                    nContinue = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                 NULL, DTWAIN_TN_PAGECONTINUE,
                                                 (LPARAM)GetSourcePtr());
                    if ( !nContinue)
                        bUserCancelled = true;
                }
                if ( !nContinue || bForceClose )
                {
//                    ResetTransfer(MSG_ENDXFER);
                    ResetTransfer(MSG_RESET);
                    // Check if canceled by user
                    if ( bUserCancelled )
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                            NULL, DTWAIN_TN_PAGECANCELLED,
                                           (LPARAM)GetSourcePtr());
                }
                else
                // Check if the user wants to just stop the feeder
                {
                    if ( nContinue == 2 )// Stop the feeder
                        StopFeeder();
                    // Remain in state 6, even if user wanted to stop the feeder
                    m_bScanPending = true;

                    if ( bEndOfJobDetected )
                    {
                        if ( pSource->IsJobFileHandlingOn() )
                            SaveJobPages(FileWriter);

                        // Increment job number
                        pSource->SetPendingJobNum(pSource->GetPendingJobNum() + 1);

                        // Set everything for next job
                        pSource->ResetJob();
                        return true;
                    }
                    return true;
                }
            }

            if ( ptrPending->Count == 0 || !nContinue || bForceClose || bEndOfJobDetected )
            {
                // Prompt to save image here

                // Send a message to close things down if
                // there was no user interface chosen
                if ( !pSource->IsUIOpenOnAcquire() )
                    CTL_TwainAppMgr::EndTwainUI(pSession, pSource);

                // Close any open multi page DIB files
                if ( pSource->GetAcquireType() == TWAINAcquireType_FileUsingNative)
                {
                    if ( !bForceClose )
                    {
                        // Check if we've acquired any pages succesfully
                        if ( (pSource->GetPendingImageNum() + 1) - pSource->GetBlankPageCount() > 0)
                        {
                            if ( pSource->IsMultiPageModeSaveAtEnd() &&
                                 !CTL_ITwainSource::IsFileTypeMultiPage( pSource->GetAcquireFileType() ))
                            {
                                pSource->ProcessMultipageFile();
                            }
                            else
                            if ( !pSource->IsMultiPageModeContinuous() )
                            {
                                if ( !pSource->GetTransferDone())
                                {
                                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                        NULL, DTWAIN_TN_FILESAVECANCELLED,
                                        (LPARAM)GetSourcePtr());
                                }
                                else
                                {
                              if ( FileWriter.CloseMultiPageDibFile() == 0 )
                                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                                  NULL, DTWAIN_TN_FILESAVEOK,
                                                                  (LPARAM)GetSourcePtr());
                              else
                              {
                                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                                  NULL, DTWAIN_TN_FILESAVEERROR,
                                                                  (LPARAM)GetSourcePtr());
                                GetSourcePtr()->ClearPDFText(); // clear the text elements
                                  }
                              }
                            }
                        }
                    }
                }
                if ( bEndOfJobDetected)
                {
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                      NULL, DTWAIN_TN_ENDOFJOBDETECTED,
                                                      (LPARAM)GetSourcePtr());
                }
            }
        break;

        case TWRC_FAILURE:
        {
            TW_UINT16 ccode = CTL_TwainAppMgr::GetConditionCode(pSession, NULL);
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, TWRC_FAILURE, ccode);
            if ( !pSource->IsUIOpenOnAcquire())
                CTL_TwainAppMgr::EndTwainUI(pSession, pSource);
//                CTL_TwainAppMgr::DisableUserInterface(pSource);
        }
        break;
    }
    m_bScanPending = false;
    return false;
}

void CTL_ImageXferTriplet::SaveJobPages(ImageXferFileWriter& FileWriter)
{
    CTL_ITwainSource *pSource = GetSourcePtr();
    CTL_ITwainSession *pSession = GetSessionPtr();


    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                          NULL, DTWAIN_TN_EOJBEGINFILESAVE,
                                          (LPARAM)GetSourcePtr());

    if ( m_nTotalPagesSaved > 0)
    {
        if ( pSource->IsMultiPageModeSaveAtEnd() &&
             !CTL_ITwainSource::IsFileTypeMultiPage( pSource->GetAcquireFileType() ))
        {
            pSource->ProcessMultipageFile();
        }
        else
        if ( !pSource->IsMultiPageModeContinuous() )
        {
          if ( FileWriter.CloseMultiPageDibFile() == 0 )
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                              NULL, DTWAIN_TN_FILESAVEOK,
                                              (LPARAM)GetSourcePtr());
          else
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                              NULL, DTWAIN_TN_FILESAVEERROR,
                                              (LPARAM)GetSourcePtr());
        }
    }
    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                          NULL, DTWAIN_TN_EOJENDFILESAVE,
                                          (LPARAM)GetSourcePtr());
}

int CTL_ImageXferTriplet::GetTotalScannedPages() const
{
    return m_nTotalPages;
}

bool CTL_ImageXferTriplet::IsScanPending() const
{
    return m_bScanPending;
}

bool CTL_ImageXferTriplet::IsJobControlPending(TW_PENDINGXFERS *pPending)
{
    CTL_ITwainSource *pSource;
    pSource = GetSourcePtr();

    // If no job control, return false;
    if ( pSource->GetCurrentJobControl() == TWJC_NONE )
        return false;

    // If job control is set, check the TW_PENDINGXFERS data for non-zero
    if (pPending && pPending->EOJ != pSource->GetEOJDetectedValue())
       return true;  // Expect more data
    return false;   // No job control
}

CTL_StringType CTL_ImageXferTriplet::GetPageFileName(const CTL_StringType &strBase,
                                                     int nCurImage )
{
    CTL_StringType strFormat = boost::lexical_cast<CTL_StringType>(nCurImage);
    CTL_StringType strTemp;
//  StringWrapper::Format(strFormat, "%d", nCurImage);
    size_t nLenFormat = strFormat.length();

    CTL_StringArrayType aTokens;
    // Adjust name

    StringWrapper::Tokenize(strBase, _T("."), aTokens);

    // Make sure that you take the "last" token
    size_t nTokens = aTokens.size();
    size_t nLen;

    if ( nTokens == 0 )
    {
        nLen = strBase.length();
        strTemp = StringWrapper::Left(strTemp, nLen -  nLenFormat ) + strFormat;
        return strTemp;
    }

    if ( nTokens == 1 )
    {
        nLen = aTokens[0].length();
        strTemp = StringWrapper::Left(aTokens[0], nLen - nLenFormat) + strFormat;
        return strTemp;
    }

    else
    {
        for ( size_t i =0; i < nTokens - 1; i++ )
        {
            strTemp += aTokens[i];
            strTemp += _T(".");
        }
        nLen = strTemp.length();
        strTemp = StringWrapper::Left(strTemp,  nLen - 1 - nLenFormat);
        strTemp += strFormat;
        strTemp += _T(".");
        strTemp += aTokens[nTokens-1];
    }
    return strTemp;
}

int CTL_ImageXferTriplet::GetTransferType() const
{
    return m_nTransferType;
}

bool CTL_ImageXferTriplet::StopFeeder()
{
    return ResetTransfer(MSG_STOPFEEDER);
}

bool CTL_ImageXferTriplet::ResetTransfer(TW_UINT16 Msg/*=MSG_RESET*/)
{
    CTL_ITwainSession* pSession = GetSessionPtr();
    CTL_ImagePendingTriplet Pending(pSession,
                                    GetSourcePtr(),
                                    Msg);
    TW_UINT16 rc = Pending.Execute();

    switch( rc )
    {
        case TWRC_SUCCESS:
            switch (Msg )
            {
                case MSG_RESET:
                    CTL_TwainAppMgr::WriteLogInfo(_T("Transfer reset and ended.  ADF may eject page...\n"));
                break;

                case MSG_ENDXFER:
                    CTL_TwainAppMgr::WriteLogInfo(_T("Ending transfer...\n"));
                break;

                case MSG_STOPFEEDER:
                    CTL_TwainAppMgr::WriteLogInfo(_T("stopping feeder...\n"));
            }
            return true;

        case TWRC_FAILURE:
        {
            CTL_TwainAppMgr::WriteLogInfo(_T("Reset Transfer failed...\n"));
            TW_UINT16 ccode = CTL_TwainAppMgr::GetConditionCode(pSession, NULL);
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, TWRC_FAILURE, ccode);
            return false;
        }
        break;
    }
    return false;
}

// Always called for the last bitmap scanned for File Transfer using the prompt
int CTL_ImageXferTriplet::PromptAndSaveImage(size_t nImageNum)
{
    // Get the image if native transfer
    CTL_TwainDibPtr CurDib;
    CTL_TwainDibArray* pArray = NULL;
    CTL_ITwainSource *pSource;
    pSource = GetSourcePtr();
    CTL_ITwainSession *pSession;
    pSession = GetSessionPtr();
    ImageXferFileWriter FileWriter(this, pSession, pSource);

    // Check if multi page file is being used
    bool bIsMultiPageFile = CTL_ITwainSource::IsFileTypeMultiPage(pSource->GetAcquireFileType());
    int nMultiStage = 0;
    if ( bIsMultiPageFile )
    {
        // This is the fist page of the acquisition
        if ( nImageNum == 0 )
            nMultiStage = DIB_MULTI_FIRST;
        else
        // This is a subsequent page of the acquisition
            nMultiStage = DIB_MULTI_NEXT;
    }

    switch( pSource->GetAcquireType() )
    {
        // Check if source acquire is file using native mode
        case TWAINAcquireType_FileUsingNative:
        {
            // Get the array of current array of DIBS
            pArray = pSource->GetDibArray();

            // Get the dib from the array
            CurDib = pArray->GetAt( nImageNum );
        }
        break;
        default:
            break;
    }

    CTL_StringType strTempFile;
    if ( nMultiStage == 0 || nMultiStage == DIB_MULTI_FIRST)
    {
        strTempFile = pSource->PromptForFileName();
        if ( strTempFile.empty() )
        {
            SendFileAcquireError(pSource, pSession,
                          DTWAIN_ERR_BAD_FILENAME, DTWAIN_TN_FILESAVECANCELLED );
            return false;
        }
        boost::filesystem::path p{ strTempFile };
        boost::filesystem::ofstream ofs{p};
        if ( !ofs )
        {
            SendFileAcquireError(pSource, pSession,DTWAIN_ERR_FILEWRITE, DTWAIN_TN_FILESAVEERROR );
            return false;
        }
        ofs.close();
        boost::filesystem::remove(strTempFile);
    }
    switch( pSource->GetAcquireType() )
    {
        case TWAINAcquireType_FileUsingNative:
        {

            // Write this file if using Native Mode transfer
            int retval = 0;

            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                          NULL, DTWAIN_TN_FILEPAGESAVING,
                                          (LPARAM)pSource);
            DTWAINImageInfoEx ImageInfo;

            // Get any relevant JPEG or TIFF Information
            pSource->GetImageInfoEx(ImageInfo);

            // Set information for possible TIFF file transfer
            ImageInfo.ResolutionX = 300;
            ImageInfo.ResolutionY = 300;

            if (!DTWAIN_GetSourceUnit(pSource, &ImageInfo.UnitOfMeasure))
                ImageInfo.UnitOfMeasure = DTWAIN_INCHES;

            ResolveImageResolution( pSource, &ImageInfo );

            // Now check for Postscript file types.  We alias these
            // types as TIFF format
            CTL_TwainFileFormatEnum FileType = pSource->GetAcquireFileType();
            if ( CTL_ITwainSource::IsFileTypePostscript( FileType ) )
            {
                ImageInfo.IsPostscript = true;
                ImageInfo.IsPostscriptMultipage =
                    CTL_ITwainSource::IsFileTypeMultiPage( FileType );
                ImageInfo.PostscriptType = (LONG)FileType;
            }

            // Write single page if just a dib
            if ( nMultiStage == 0 && CurDib)
                retval = CurDib->WriteDibBitmap(ImageInfo, strTempFile.c_str(), pSource->GetAcquireFileType() );
            else
            {
                // Write a multi page file
                CTL_ImageIOHandlerPtr& pHandler = pSource->GetImageHandlerPtr();

                if ( nMultiStage == DIB_MULTI_FIRST)
                    pHandler = CurDib->WriteFirstPageDibMulti(ImageInfo, strTempFile.c_str(),
                                                               pSource->GetAcquireFileType(), 0, 0, retval);
                else
                if ( nMultiStage == DIB_MULTI_NEXT)
                    CurDib->WriteNextPageDibMulti(pHandler, retval, ImageInfo);
            }

            if ( retval != 0)
            {
                SendFileAcquireError(pSource, pSession, retval, DTWAIN_TN_INVALIDIMAGEFORMAT );
                if ( nMultiStage )
                    SendFileAcquireError(pSource, pSession, retval, DTWAIN_TN_FILEPAGESAVEERROR);
                else
                    SendFileAcquireError(pSource, pSession, retval, DTWAIN_TN_FILESAVEERROR);
            }
            else
            {
                if ( !nMultiStage || nMultiStage == DIB_MULTI_FIRST )
                    pSource->SetLastAcquiredFileName( strTempFile );
                if ( nMultiStage )
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL, DTWAIN_TN_FILEPAGESAVEOK,
                                                          (LPARAM)GetSourcePtr());
                else
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL, DTWAIN_TN_FILESAVEOK,
                                                          (LPARAM)GetSourcePtr());
            }
            // Check if there were any other images
            if (nMultiStage != 0 && !IsScanPending())
            {
                // This is the last page
                if ( FileWriter.CloseMultiPageDibFile() == 0 )
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL, DTWAIN_TN_FILESAVEOK,
                                                          (LPARAM)GetSourcePtr());
                else
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                          NULL, DTWAIN_TN_FILESAVEERROR,
                                                          (LPARAM)GetSourcePtr());
                return retval;
            }
        }
        break;

        case TWAINAcquireType_File:
        {
            // Copy default file name to the new file
            // Check if default file exists
            if ( boost::filesystem::exists( pSource->GetAcquireFile()))
                CTL_TwainAppMgr::CopyFile(pSource->GetAcquireFile(), strTempFile);
            else
                return false;
        }
        break;
        default:
        break;
    }
    // Delete temporary bitmap here
    if ( pSource->IsDeleteDibOnScan() && pArray )
        // Let array class handle deleting of the DIB (Global memory will be freed only)
        pArray->DeleteDibMemory( nImageNum );
    return true;
}

bool CTL_ImageXferTriplet::CopyDibToClipboard(CTL_ITwainSession * /*pSession*/, HANDLE hDib)
{
#ifdef _WIN32
    if (hDib)
    {
        // Open the clipboard
        if (::OpenClipboard( NULL/*hWnd*/ ))
        {
            // Empty the clipboard
            if ( ::EmptyClipboard() )
            {
                SetClipboardData(CF_DIB, hDib);
                ::CloseClipboard();
                return true;
            }
            ::CloseClipboard();
        }
    }
#endif
    return false;
}

bool CTL_ImageXferTriplet::CropDib(CTL_ITwainSession *pSession,
                                   CTL_ITwainSource *pSource,
                                   CTL_TwainDibPtr CurDib)
{
    // Possibly crop the DIB
    LONG SourceUnit;
    FloatRect Requested;
    FloatRect Actual;
    LONG flags;
    LONG DestUnit;
    pSource->GetAlternateAcquireArea(Requested, DestUnit, flags);
    if (flags & CTL_ITwainSource::CROP_FLAG)
    {
        if (!pSource->IsImageLayoutValid())
        {
            CTL_TwainAppMgr::WriteLogInfo(_T("Image layout is invalid.  Image cannot be cropped"));
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,
                DTWAIN_TN_CROPFAILED,
                (LPARAM)pSource);
            return false;
        }
        // Get the actual acquisition area
        pSource->GetImageLayout(&Actual);
        bool bUndefinedSize = false;
        if ( Actual.left < 0.0 && Actual.right < 0.0 &&
                Actual.top < 0.0 && Actual.bottom < 0.0 )
        {
            // the image information is undefined.  Should get the information
            // now from the image info and assume pixels.
            bUndefinedSize = true;

            // Get the image info
            CTL_ImageInfoTriplet ImageInfo(pSession, pSource);
            if ( ImageInfo.Execute() == TWRC_SUCCESS )
            {
                // Set the rectangle to the image info.  This is in
                // pixels
                TW_IMAGEINFO *pInfo = ImageInfo.GetImageInfoBuffer();
                Actual.left = Actual.top = 0.0;
                Actual.right = pInfo->ImageWidth;
                Actual.bottom = pInfo->ImageLength;
            }
        }

        // Now get the unit of measure
        if ( DTWAIN_GetSourceUnit(pSource, &SourceUnit) )
        {
            // Get the image resolution
            double Resolution;
            DTWAIN_GetResolution(pSource, &Resolution);

            // Crop the dib here
            if (CurDib->CropDib(Actual, Requested, SourceUnit, DestUnit, (int)Resolution,
                                bUndefinedSize))
                return true;
        }
    }
    return false;
}

bool CTL_ImageXferTriplet::NegateDib(CTL_ITwainSession * /*pSession*/,
                                     CTL_ITwainSource *pSource,
                                     CTL_TwainDibPtr CurDib)
{
    if ( pSource->IsImageNegativeOn() )
    {
        if (CurDib->NegateDib())
            return true;
    }
    return false;
}

bool CTL_ImageXferTriplet::ResampleDib(CTL_ITwainSession * /*pSession*/,
                                   CTL_ITwainSource *pSource,
                                   CTL_TwainDibPtr CurDib)
{
    double xscale;
    double yscale;
    LONG flags;
    pSource->GetImageScale(xscale, yscale, flags);
    if (flags & CTL_ITwainSource::SCALE_FLAG)
    {
        if (CurDib->ResampleDib(xscale, yscale))
            return true;
    }
    return false;
}


bool CTL_ImageXferTriplet::ChangeBpp(CTL_ITwainSession * /*pSession*/,
                                     CTL_ITwainSource *pSource,
                                     CTL_TwainDibPtr CurDib)
{
    LONG bpp = pSource->GetForcedImageBpp();
    bool bRetval = false;
    if ( bpp != 0 )
    {
        int depth = CurDib->GetDepth();
        if ( bpp > depth )
            bRetval = CurDib->IncreaseBpp(bpp);
        else
        if (bpp < depth)
            bRetval = CurDib->DecreaseBpp(bpp);
    }
    return bRetval;
}

int CTL_ImageXferTriplet::ProcessBlankPage(CTL_ITwainSession *pSession,
                                           CTL_ITwainSource *pSource,
                                           CTL_TwainDibPtr CurDib,
                                           LONG message_to_send1,
                                           LONG message_to_send2,
                                           LONG option_to_test)
{
    bool bIsBlankPage = IsPageBlank(pSession, pSource, CurDib)?true:false;
    if (bIsBlankPage)
    {
        LONG bKeepPage = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, (WPARAM)message_to_send1, (LPARAM)pSource);

        if ( pSource->IsBlankPageAutoDetectOn() )
        {
            LONG options = pSource->GetBlankPageAutoDetect();
            bKeepPage = ( options & option_to_test )?0:1;
        }
        if ( !bKeepPage )
        {
            // remove dib from array and delete the memory for the DIB
            CurDib->SetAutoDelete(true);
            pSource->GetDibArray()->RemoveDib(m_hDib);
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, (WPARAM)message_to_send2,(LPARAM)pSource);
            return 0;  // DIB is thrown away
        }
    }
    return 1; // keep the DIB
}


bool CTL_ImageXferTriplet::IsPageBlank(CTL_ITwainSession * /*pSession*/,
                                       CTL_ITwainSource *pSource,
                                       CTL_TwainDibPtr CurDib)
{
    if ( pSource->IsBlankPageDetectionOn() )
        return CurDib->IsBlankDIB(pSource->GetBlankPageThreshold());
    return false;
}

bool CTL_ImageXferTriplet::ResampleBppForJPEG( CTL_ITwainSession * /*pSession*/,
                                               CTL_ITwainSource * /*pSource*/,
                                               CTL_TwainDibPtr CurDib)
{
    int depth = CurDib->GetDepth();

    if ( depth == 24 ) //|| depth < 8 )
        return false;

    bool IsGray = CurDib->IsGrayScale();
    if ( !IsGray || (depth < 8 && !IsGray))
    {
        CTL_TwainAppMgr::WriteLogInfo(_T("Resampling bitmap data to 24 bpp (JPEG processing)..."));
        bool bOk = CurDib->IncreaseBpp(24);
        if ( bOk)
            CTL_TwainAppMgr::WriteLogInfo(_T("Resampling ok..."));
        return bOk;
    }

    return false;
}

bool CTL_ImageXferTriplet::ResampleBppForWBMP( CTL_ITwainSession * /*pSession*/,
                                              CTL_ITwainSource * /*pSource*/,
                                              CTL_TwainDibPtr CurDib)
{
    int depth = CurDib->GetDepth();
    bool ResamplingDone = false; // no resampling needs to be done
    if ( depth > 1 )
    {
        // Make depth == 1
        CTL_TwainAppMgr::WriteLogInfo(_T("Resampling bitmap data to 1 bpp (WBMP processing)..."));
        bool bOk = CurDib->DecreaseBpp(1);
        if ( bOk)
        {
           CTL_TwainAppMgr::WriteLogInfo(_T("Resampling to 1 bpp ok..."));
           ResamplingDone = true;
        }
        else
        {
            CTL_TwainAppMgr::WriteLogInfo(_T("Resampling to 1 bpp failed..."));
            ResamplingDone = false;
        }
    }
    return ResamplingDone;
}


bool CTL_ImageXferTriplet::ResampleBppForGIF( CTL_ITwainSession * /*pSession*/,
                                              CTL_ITwainSource * /*pSource*/,
                                              CTL_TwainDibPtr /*CurDib*/)
{
    return false;
}


bool CTL_ImageXferTriplet::ResampleBppForPDF( CTL_ITwainSession * /*pSession*/,
                                              CTL_ITwainSource * /*pSource*/,
                                              CTL_TwainDibPtr CurDib)
{
    int depth = CurDib->GetDepth();

    if ( depth == 8 || depth == 1 || depth == 24)
        return false;

    bool bOk;
    if ( depth > 8 )
    {
        CTL_TwainAppMgr::WriteLogInfo(_T("Resampling bitmap data up to to 24 bpp (PDF processing)..."));
        bOk = CurDib->IncreaseBpp(24);
    }
    else
    {
        CTL_TwainAppMgr::WriteLogInfo(_T("Resampling bitmap data up to 8 bpp (PDF processing)..."));
        bOk = CurDib->IncreaseBpp(8);
    }
    if ( bOk)
        CTL_TwainAppMgr::WriteLogInfo(_T("Resampling ok..."));
    else
        CTL_TwainAppMgr::WriteLogInfo(_T("Resampling failed..."));
    return bOk;
}

CTL_TwainFileFormatEnum CTL_ImageXferTriplet::GetFileTypeFromCompression(int nCompression)
{
    switch (nCompression)
    {
        case TWCP_GROUP31D:
        case TWCP_GROUP31DEOL:
        case TWCP_GROUP32D:
            return TWAINFileFormat_TIFFGROUP3;

        case TWCP_GROUP4:
            return TWAINFileFormat_TIFFGROUP4;

        case TWCP_PACKBITS:
            return TWAINFileFormat_TIFFPACKBITS;

        case TWCP_JPEG:
            return TWAINFileFormat_JPEG;

        case TWCP_LZW:
            return TWAINFileFormat_TIFFLZW;

        case TWCP_JBIG:
            return TWAINFileFormat_JBIG;

        case TWCP_PNG:
            return TWAINFileFormat_PNG;

        case TWCP_RLE4:
        case TWCP_RLE8:
        case TWCP_BITFIELDS:
            return TWAINFileFormat_BMP;


    }
    return TWAINFileFormat_RAW;
}

void SendFileAcquireError(CTL_ITwainSource *pSource, CTL_ITwainSession *pSession,
                          LONG Error, LONG ErrorMsg )
{
    CTL_TwainAppMgr::SetError(Error);
    TCHAR szBuf[255];
    CTL_TwainAppMgr::GetLastErrorString(szBuf, 254);
    CTL_TwainAppMgr::WriteLogInfo(szBuf);
    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, (WPARAM)ErrorMsg, (LPARAM)pSource);
}

void CTL_ImageXferTriplet::ResolveImageResolution(CTL_ITwainSource *pSource,  DTWAINImageInfoEx* ImageInfo)
{
    double Resolution;

    // Get the image info
    // First try the actual image
    double ResolutionX, ResolutionY;

    // First, check if we need to get the info from state 7
    bool bGotResolution = false;
    bool bGetResFromDriver = false;

    if ( IsState7InfoNeeded(pSource) )
    {
      if ( DTWAIN_GetImageInfo(pSource,
                              &ResolutionX,
                              &ResolutionY,
                              NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL))
        {
            CTL_StringType sError = _T("Image resolution available in state 7.\n");
            CTL_TwainAppMgr::WriteLogInfo(sError);
            ImageInfo->ResolutionX = static_cast<LONG>(ResolutionX);
            ImageInfo->ResolutionY = static_cast<LONG>(ResolutionY);
            CTL_StringStreamType strm;
            #ifdef UNICODE
            strm << boost::wformat(_T("x-Resolution=%1%, y-Resolution=%2%\n")) % ImageInfo->ResolutionX % ImageInfo->ResolutionY;
            #else
            strm << boost::format("x-Resolution=%1%, y-Resolution=%2%\n") % ImageInfo->ResolutionX % ImageInfo->ResolutionY;
            #endif
            sError = strm.str();
//          StringWrapper::Format(sError, "x-Resolution=%d, y-Resolution=%d\n", ImageInfo->ResolutionX, ImageInfo->ResolutionY);
            CTL_TwainAppMgr::WriteLogInfo(sError);
            bGotResolution = true;
        }
        else
        // Try to get resolution from the driver
           bGetResFromDriver = true;
    }

    if ( !bGotResolution && !bGetResFromDriver )
    {
        // Get the image info from when we started
        CTL_StringType sError = _T("Getting image resolution from state 6.\n");
        CTL_TwainAppMgr::WriteLogInfo(sError);
        TW_IMAGEINFO II;
        pSource->GetImageInfo( &II );

        ImageInfo->ResolutionX = static_cast<LONG>(CTL_CapabilityTriplet::Twain32ToFloat(II.XResolution));
        ImageInfo->ResolutionY = static_cast<LONG>(CTL_CapabilityTriplet::Twain32ToFloat(II.YResolution));
        CTL_StringStreamType strm;
        #ifdef UNICODE
        strm << boost::wformat(_T("x-Resolution=%1%, y-Resolution=%2%\n")) % ImageInfo->ResolutionX % ImageInfo->ResolutionY;
        #else
        strm << boost::format("x-Resolution=%1%, y-Resolution=%2%\n") % ImageInfo->ResolutionX % ImageInfo->ResolutionY;
        #endif
//      StringWrapper::Format(sError, "x-Resolution=%d, y-Resolution=%d\n", ImageInfo->ResolutionX, ImageInfo->ResolutionY);
        CTL_TwainAppMgr::WriteLogInfo(strm.str()); //sError.c_str());
        bGotResolution = true;
    }

    if ( !bGotResolution )
    {
        // Try TWAIN driver setting
        if ( DTWAIN_GetResolution(pSource, &Resolution) )
        {
            CTL_StringType sError = _T("Image resolution obtained from TWAIN driver\n");
            CTL_TwainAppMgr::WriteLogInfo(sError);

            ImageInfo->ResolutionX = static_cast<LONG>(Resolution);
            ImageInfo->ResolutionY = static_cast<LONG>(Resolution);
            CTL_StringStreamType strm;
            #ifdef UNICODE
            strm << boost::wformat(_T("x-Resolution=%1%, y-Resolution=%2%\n")) % ImageInfo->ResolutionX % ImageInfo->ResolutionY;
            #else
            strm << boost::format("x-Resolution=%1%, y-Resolution=%2%\n") % ImageInfo->ResolutionX % ImageInfo->ResolutionY;
            #endif
            CTL_TwainAppMgr::WriteLogInfo(strm.str()); //sError.c_str());
        }
        else
        {
            // Tried everything, just set the resolution to the default resolution
            CTL_StringType sError = _T("Could not obtain resolution in state 6/7 or through TWAIN.  Image resolution defaulted to 100 DPI\n");
            CTL_TwainAppMgr::WriteLogInfo(sError);
            ImageInfo->ResolutionX = 100;
            ImageInfo->ResolutionY = 100;
        }
    }
}

bool CTL_ImageXferTriplet::ModifyAcquiredDib()
{
    CTL_ITwainSession* pSession = GetSessionPtr();
    CTL_ITwainSource* pSource = GetSourcePtr();
    CTL_TwainDibArray* pArray = pSource->GetDibArray();
    CTL_TwainDibPtr CurDib;
    size_t nLastDib = 0;

    nLastDib = pArray->GetSize() - 1;
    CurDib = pArray->GetAt(nLastDib);


    typedef bool (CTL_ImageXferTriplet::*AdjustFn)(CTL_ITwainSession*, CTL_ITwainSource*, CTL_TwainDibPtr);
    AdjustFn adjfn[] = { &CTL_ImageXferTriplet::ChangeBpp, &CTL_ImageXferTriplet::CropDib,
        &CTL_ImageXferTriplet::ResampleDib, &CTL_ImageXferTriplet::NegateDib };

    const CTL_StringType msg[] = { _T("Bitmap after change to bits-per-pixel: \n"), _T("Bitmap after cropping: \n"),
        _T("Bitmap after resampling: \n"), _T("Bitmap after negating image: \n") };

    for (unsigned i = 0; i < sizeof(adjfn) / sizeof(adjfn[0]); ++i)
    {
        // call the function to adjust bitmap
        if ((this->*adjfn[i])(pSession, pSource, CurDib))
        {
            // reset the dib handle if adjusted
            pSource->SetDibHandle((m_hDib = CurDib->GetHandle()), nLastDib);
            if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
            {
                CTL_StringType sOut = msg[i];
                sOut += CTL_ErrorStructDecoder().DecodeBitmap(m_hDib);
                CTL_TwainAppMgr::WriteLogInfo(sOut);
            }
    return true;
}
    }
    return false;
}

bool CTL_ImageXferTriplet::ResampleAcquiredDib()
{
    CTL_ITwainSource* pSource = GetSourcePtr();
    int nFileType = pSource->GetAcquireFileType();

    std::bitset<3> bResampleType = 0;
    bResampleType[0] = (nFileType == TWAINFileFormat_GIF);
    bResampleType[1] = ((nFileType == TWAINFileFormat_JPEG) ||
        (nFileType == TWAINFileFormat_JPEG2000));
    bResampleType[2] = (nFileType == TWAINFileFormat_WBMP);

    if (bResampleType.to_ulong() == 0)
        return false;

    CTL_ITwainSession* pSession = GetSessionPtr();
    CTL_TwainDibArray* pArray = pSource->GetDibArray();
    CTL_TwainDibPtr CurDib;
    size_t nLastDib = 0;

    nLastDib = pArray->GetSize() - 1;
    CurDib = pArray->GetAt(nLastDib);

    typedef bool (CTL_ImageXferTriplet::*ResampleFn)(CTL_ITwainSession*, CTL_ITwainSource*, CTL_TwainDibPtr);
    ResampleFn resample[] = { &CTL_ImageXferTriplet::ResampleBppForGIF, &CTL_ImageXferTriplet::ResampleBppForJPEG,
        &CTL_ImageXferTriplet::ResampleBppForWBMP };

    const CTL_StringType msg[] = { _T("Bitmap after resampling for GIF: \n"),
        _T("Bitmap after resampling for JPEG: \n"),
        _T("Bitmap after resampling for WBMP: \n") };

    for (unsigned i = 0; i < sizeof(resample) / sizeof(resample[0]); ++i)
    {
        // call the function to resample bitmap
        if (bResampleType[i] && (this->*resample[i])(pSession, pSource, CurDib))
        {
            // reset the dib handle if resampled
            pSource->SetDibHandle((m_hDib = CurDib->GetHandle()), nLastDib);
            if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
            {
                CTL_StringType sOut = msg[i];
                sOut += CTL_ErrorStructDecoder().DecodeBitmap(m_hDib);
                CTL_TwainAppMgr::WriteLogInfo(sOut);
            }
            return true;
        }
    }
	return false;
}

bool CTL_ImageXferTriplet::QueryAndRemoveDib(CTL_TwainAcquireEnum acquireType, size_t nWhich)
{
    CTL_ITwainSource* pSource = GetSourcePtr();
    CTL_TwainDibArray* pArray = pSource->GetDibArray();
    CTL_ITwainSession* pSession = GetSessionPtr();
    bool bKeepPage = true;

    if (pSource->GetAcquireType() == acquireType)
    {
        bKeepPage = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_QUERYPAGEDISCARD, (LPARAM)pSource) ? true : false;
        // Keep the page
        if (!bKeepPage)
        {
            // throw this dib away (remove from the dib array)
            pArray->DeleteDibMemory(nWhich);
            pArray->RemoveDib(nWhich);
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_PAGEDISCARDED, (LPARAM)pSource);
        }
    }
    return bKeepPage;
}

bool IsState7InfoNeeded(CTL_ITwainSource *pSource)
{
    bool bRetval = false;
    DTWAIN_ARRAY A = 0;
    DTWAINArrayLL_RAII raii(A);
    DTWAINScopedLogControllerExclude scopedLog(DTWAIN_LOG_ERRORMSGBOX);
    if ( DTWAIN_GetCapValues(pSource, DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE, DTWAIN_CAPGETCURRENT, &A))
    {
        auto& vValues = EnumeratorVector<int>(A);
        if ( !vValues.empty())
            bRetval = (vValues[0] > 0);
    }
    return bRetval;
}
///////////////////////////////////////////////////////////////////////////
