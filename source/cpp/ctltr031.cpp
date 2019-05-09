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
#include "ctltr031.h"
#include "ctltr027.h"
#include "ctltwmgr.h"
#include "imagexferfilewriter.h"
#include "ctldib.h"
#include "dtwain.h"
#include "winbit32.h"

#define DEMO_MAX_PAGES 10000

using namespace dynarithmic;

CTL_ImageMemXferTriplet::CTL_ImageMemXferTriplet(CTL_ITwainSession *pSession,
                                                 CTL_ITwainSource* pSource,
                                                 HANDLE hDib,
                                                 TW_UINT32 nFlags,
                                                 TW_UINT16 nPixelType,
                                                 TW_UINT32 nNumBytes,
                                                 TW_UINT16 nCompression/*=TWCP_NONE*/)
                     :  CTL_ImageXferTriplet(pSession, pSource, DAT_IMAGEMEMXFER)
{
    LPBITMAPINFO    pDibInfo;

    // Lock the data.  It will be unlocked when the image has been transferred
    // either successfully or unsuccessfully, or if "this" is being destroyed
    pDibInfo = (LPBITMAPINFO)ImageMemoryHandler::GlobalLock(hDib);
    m_nCurDibSize = static_cast<TW_UINT32>(ImageMemoryHandler::GlobalSize(hDib));

    m_ptrDib = (unsigned char TW_HUGE *)pDibInfo;
    m_ptrOrig = m_ptrDib;

    // if no compression and no user-defined buffer, do the DIB stuff
    // ourselves
    if ( nCompression == TWCP_NONE && !pSource->GetUserStripBuffer())
    {
        m_ptrDib += sizeof(BITMAPINFOHEADER);
        m_ptrDib += pDibInfo->bmiHeader.biClrUsed * sizeof(RGBQUAD);
    }

    m_nCompression = nCompression;

    InitXferBuffer();

    m_ImageMemXferBuffer.Compression = TWON_DONTCARE16;
    m_ImageMemXferBuffer.BytesPerRow =
    m_ImageMemXferBuffer.Columns =
    m_ImageMemXferBuffer.Rows =
    m_ImageMemXferBuffer.XOffset =
    m_ImageMemXferBuffer.YOffset =
    m_ImageMemXferBuffer.BytesWritten = TWON_DONTCARE32;

    if ( nCompression != TWCP_NONE )
    {
        m_ImageMemXferBuffer.Compression = nCompression;
        m_ImageMemXferBuffer.BytesPerRow = 0;
    }

    InitVars( DAT_IMAGEMEMXFER, CTL_GetTypeGET, &m_ImageMemXferBuffer );

    m_DibStrip = hDib;

    m_ImageMemXferBuffer.Memory.Flags  = nFlags;
    m_ImageMemXferBuffer.Memory.Length = nNumBytes;

    // Use the memory that was setup for the DIB
    if ( nCompression == TWCP_NONE )
        m_ImageMemXferBuffer.Memory.TheMem = m_ptrDib;
    // No compression, so use a temp buffer that is the size of nNumBytes
    else
    {
        HANDLE hBuffer = pSource->GetUserStripBuffer();
        if (!hBuffer)
            m_ImageMemXferBuffer.Memory.TheMem = ImageMemoryHandler::GlobalAllocPr(GMEM_MOVEABLE, nNumBytes);
        else
            m_ImageMemXferBuffer.Memory.TheMem = (TW_MEMREF)ImageMemoryHandler::GlobalLock(hBuffer);
    }

    m_TempMemory.TheMem = NULL;
    m_nCompressPos = 0;
    m_nPixelType = nPixelType;
    pSource->SetBufferStripData(&m_ImageMemXferBuffer);
}

CTL_ImageMemXferTriplet::~CTL_ImageMemXferTriplet()
{
    if ( m_nCompression == TWCP_NONE && m_ptrOrig )
        ImageMemoryHandler::GlobalUnlock(ImageMemoryHandler::GlobalHandle(m_ptrOrig));

    CTL_ITwainSource *pSource = GetSourcePtr();
    pSource->SetBufferStripData(NULL);
}

void CTL_ImageMemXferTriplet::InitXferBuffer()
{
    m_ImageMemXferBuffer.Compression = TWON_DONTCARE16;
    m_ImageMemXferBuffer.BytesPerRow =
    m_ImageMemXferBuffer.Columns =
    m_ImageMemXferBuffer.Rows =
    m_ImageMemXferBuffer.XOffset =
    m_ImageMemXferBuffer.YOffset =
    m_ImageMemXferBuffer.BytesWritten = TWON_DONTCARE32;

    if ( m_nCompression != TWCP_NONE )
    {
        m_ImageMemXferBuffer.Compression = m_nCompression;
        m_ImageMemXferBuffer.BytesPerRow = 0;
    }
}


TW_UINT16 CTL_ImageMemXferTriplet::Execute()
{
    // This transfer type works differently than the File and Native transfers.
    // This type must loop "internally" for each strip before getting the next page.
    TW_UINT16 rc;
    bool bPageDiscarded = false;
    bool    bProcessDibEx = true;
    bool    bKeepPage = true;
    size_t nLastDib;

    // Check if a temp buffer was allocated successfully
    if ( m_nCompression != TWCP_NONE )
    {
        if ( !m_ImageMemXferBuffer.Memory.TheMem )
            return TWRC_FAILURE;
    }
    // Get the app manager's AppID

    CTL_ITwainSource *pSource = GetSourcePtr();
    CTL_ITwainSession *pSession = GetSessionPtr();
    ImageXferFileWriter FileWriter(this, pSession, pSource);

    // Loop until strips have been transferred
    do
    {
        // Call base function
        rc = CTL_ImageTriplet::Execute();

        switch (rc)
        {
            case TWRC_SUCCESS:
                // Send message that a strip has been transferred successfully
                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_TRANSFERSTRIPREADY, (LPARAM)pSource);
                if ( m_nCompression == TWCP_NONE )
                {
                    m_nCompressPos += m_ImageMemXferBuffer.BytesWritten;
                    // If not user defined copy DIB data here
                    if ( !pSource->GetUserStripBuffer())
                        m_ptrDib += m_ImageMemXferBuffer.BytesWritten;

                    m_ImageMemXferBuffer.Memory.TheMem = m_ptrDib;
                }
                else /* At this point, compression was assumed.  The memory will be allocated // by the Source if strips are
                used.  To see if the memory buffer can be deleted, the pointer // to the memory buffer is compared to the
                last pointer.  If it's the same // assume that the buffer is reused and should not be deleted.  Since the //
                app is responsible for the destruction of the Source, comparisons are // assumed safe.
                      */
                {
                    {
                        // Need to Reallocate the pointer and copy strip to correct posistion

                        // Check if we are doing this ourselves
                        LONG nBytes;
                        HANDLE hUserBuffer;
                        hUserBuffer = pSource->GetUserStripBuffer();
                        if ( !hUserBuffer )
                        {
                            nBytes = m_ImageMemXferBuffer.BytesWritten;
                            char FAR *pMem = (char *)ImageMemoryHandler::GlobalReAllocPr(m_ptrDib, m_nCompressPos + nBytes,GMEM_MOVEABLE);

                            if ( !pMem )
                            {
                               CTL_TwainAppMgr::SetError(DTWAIN_ERR_OUT_OF_MEMORY);
                               TCHAR szBuf[255];
                               CTL_TwainAppMgr::GetLastErrorString(szBuf, 254);
                               CTL_TwainAppMgr::WriteLogInfo(szBuf);
                               CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                                  NULL, DTWAIN_TN_TRANSFERSTRIPFAILED,
                                                                  (LPARAM)GetSourcePtr());
                            }
                            else
                            {
                                // Lock memory
                                char *pMemory;
                                if ( ImageMemoryHandler::GlobalHandle(m_ImageMemXferBuffer.Memory.TheMem) )
                                    pMemory = (char FAR *) ImageMemoryHandler::GlobalLock(m_ImageMemXferBuffer.Memory.TheMem);
                                else
                                    pMemory = (char FAR *)m_ImageMemXferBuffer.Memory.TheMem;

                                // Write starting at the last position
                                memcpy(pMem + m_nCompressPos, pMemory, nBytes);

                                // Unlock the memory
                                if ( ImageMemoryHandler::GlobalHandle(m_ImageMemXferBuffer.Memory.TheMem))
                                    ImageMemoryHandler::GlobalUnlock( m_ImageMemXferBuffer.Memory.TheMem );
                            }

                            // Increase the last position
                            m_nCompressPos += nBytes;
                            m_ptrDib = (unsigned char *)pMem;
                        }
                    }
                }
                // Send message that a strip has been transferred successfully
                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,
                                            DTWAIN_TN_TRANSFERSTRIPDONE,
                                            (LPARAM)pSource);
                InitXferBuffer();
            break;

            case TWRC_CANCEL:
            {
                ImageMemoryHandler::GlobalUnlock(m_DibStrip);
                CancelAcquisition();
            }
            break;

            case TWRC_FAILURE:
            {
                ImageMemoryHandler::GlobalUnlock(m_DibStrip);
                FailAcquisition();
                return rc;
            }
            break;

            case TWRC_XFERDONE:             // All strips transferred.  Process bitmap
            {
                m_bJobControlPageRecorded = false;
                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_TRANSFERDONE,(LPARAM)pSource);
                m_bJobMarkerNeedsToBeWritten = false;
                // Check if more images are pending (job control only)
                SetPendingXfersDone(false);

                bool bEndOfJobDetected = false;

                // Get the current page number of image being transferred
                size_t nCurImage = pSource->GetPendingImageNum();

                // See if we need to check the job control status via TWAIN
                if ( pSource->IsUIOpenOnAcquire() && nCurImage == 0 )
                {
                    LONG JobControl;
                    // Get the current job control if the user may have changed it
                    // in the UI of the TWAIN driver
                    DTWAINScopedLogControllerExclude sLogerr(DTWAIN_LOG_ERRORMSGBOX);
                    if (DTWAIN_GetJobControl(pSource, &JobControl, TRUE) != FALSE)
                        pSource->SetCurrentJobControl( static_cast<TW_UINT16>(JobControl));
                }

                if ( pSource->GetCurrentJobControl() != TWJC_NONE )
                {
                    TW_PENDINGXFERS& Pending = GetLocalPendingXferInfo();
                    SetLastPendingInfoCode(GetImagePendingInfo(&Pending));
                    if (GetLastPendingInfoCode() == TWRC_SUCCESS)
                    {
                        // Indicate that pending xfers has been executed
                        SetPendingXfersDone(true);
                        bEndOfJobDetected = ( Pending.EOJ == pSource->GetEOJDetectedValue());
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

                // Get the image page
                int errfile = 0;
                bool bInClip = false;


                // Let Source set the handle
                m_nCompressPos += m_ImageMemXferBuffer.BytesWritten;
                pSource->SetNumCompressBytes(m_nCompressPos);

                ImageMemoryHandler::GlobalUnlock(m_DibStrip);
                if ( !pSource->GetUserStripBuffer() )
                {

                    // Get the image if native transfer
                    CTL_TwainDibPtr CurDib;
                    CTL_TwainDibArray* pArray;
                    if ( m_nCompression == TWCP_NONE )
                        pSource->SetDibHandle( m_DibStrip, nCurImage );
                    else
                    // This "DIB" is just a holder for the memory.
                    {
                        // Copy the memory we got from the compressed transfer and
                        // fake it that this is a DIB
                        pSource->SetDibHandleNoPalette( ImageMemoryHandler::GlobalHandle(m_ptrDib), (int)nCurImage );

                        // Unlock the memory used to store compressed image
                        ImageMemoryHandler::GlobalUnlock(ImageMemoryHandler::GlobalHandle(m_ptrDib));
                    }

                    //  Get the array of current array of DIBS
                    // (this pointer allows changes to Source's internal DIB array)
                    pArray = pSource->GetDibArray();

                    // Get the dib from the array
                    nLastDib = pArray->GetSize() - 1;
                    CurDib = pArray->GetAt( nLastDib );
                    if ( CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_PROCESSEDDIB, (LPARAM)pSource) == 0 )
                    {
                        // User does not want to process the image further.
                        // They are satisfied with the DIB as-is.
                        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_PROCESSDIBACCEPTED, (LPARAM)pSource);
                        bProcessDibEx = false;
                        break;
                    }

                    // This may be a compressed image instead of a DIB.  Don't flip if this is an image
                    if ( m_nCompression == TWCP_NONE )
                    {
                        // Here we can do a check for blank page.  The code has not been tested
                        // To be done...
                        if ( ProcessBlankPage(pSession, pSource, CurDib, DTWAIN_TN_BLANKPAGEDETECTED1,
                            DTWAIN_TN_BLANKPAGEDISCARDED1, DTWAIN_BP_AUTODISCARD_IMMEDIATE) == 0 )
                        {
                            bPageDiscarded = true;
                            break;  // The page is discarded
                        }

                        CurDib->FlipBitMap(m_nPixelType == TWPT_RGB?1:0);

                        // Callback function for access to change DIB
                        if ( CTL_TwainDLLHandle::s_pDibUpdateProc != NULL )
                        {
                            HANDLE hRetDib =
                                (*CTL_TwainDLLHandle::s_pDibUpdateProc)
                                        (pSource, (int)nCurImage, m_hDib);
                            if ( hRetDib && (hRetDib != m_hDib))
                            {
                                // Application changed DIB.  So make this the current dib
                                ImageMemoryHandler::GlobalFree( m_hDib );
                                m_hDib = hRetDib;
                                pSource->SetDibHandle( m_hDib, nLastDib );
                                CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_APPUPDATEDDIB, (LPARAM)pSource);
                            }
                        }

                        // Change bpp if necessary
                        if (bProcessDibEx)
                            ModifyAcquiredDib();

                        if ( CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_PROCESSEDDIBFINAL, (LPARAM)pSource) == 0 )
                        {
                            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_PROCESSDIBFINALACCEPTED, (LPARAM)pSource);
                            // user is satisfied with the image, so break
                            break;
                        }
                    }

                    if ( ProcessBlankPage(pSession, pSource, CurDib, DTWAIN_TN_BLANKPAGEDETECTED2,DTWAIN_TN_BLANKPAGEDISCARDED2,  DTWAIN_BP_AUTODISCARD_AFTERPROCESS) == 0 )
                    {
                        bPageDiscarded = true;
                        break;  // The page is discarded
                    }

                    // Now see if we want to keep the bitmap
                    // Query if the page should be thrown away
                    bKeepPage = QueryAndRemoveDib(TWAINAcquireType_Buffer, nLastDib);
                    if (!bKeepPage)
                        break;

                    if ( pSource->GetAcquireType() == TWAINAcquireType_Clipboard )
                    {
                        // This may be a compressed image instead of a DIB.  Don't place in clipboard
                        if ( m_nCompression == TWCP_NONE )
                        {
                            if ( pSource->GetSpecialTransferMode() == DTWAIN_USEBUFFERED )
                                bInClip = CopyDibToClipboard( pSession, CurDib->GetHandle());
                        }
                        else
                            bInClip = false;
                    }
                    else

                    if ( pSource->GetAcquireType() == TWAINAcquireType_FileUsingNative)
                    {
                        // This may be a compressed image instead of a DIB.  Use Raw IO handler to write this file
                        if ( m_nCompression != TWCP_NONE )
                            pSource->SetAcquireFileType(GetFileTypeFromCompression(m_nCompression));

                        pSource->SetPromptPending(false);
                        long lFlags   = pSource->GetAcquireFileFlags();
                        if ( lFlags & DTWAIN_USEPROMPT )
                        {
                            pSource->SetPromptPending(true);
                            break;
                        }

                        // resample the DIB
                        if (m_nCompression == TWCP_NONE)
                            ResampleAcquiredDib();

                        // Check if multi page file is being used
                        // Query if the page should be thrown away
                        bool bKeepPage2 = CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,DTWAIN_TN_QUERYPAGEDISCARD,(LPARAM)pSource)?true:false;
                        if (bKeepPage2 )
                        {
                            // Check if multi page file is being used
                            bool bIsMultiPageFile = CTL_ITwainSource::IsFileTypeMultiPage(pSource->GetAcquireFileType());
                            int nMultiStage = 0;
                            if ( bIsMultiPageFile || pSource->IsMultiPageModeSaveAtEnd())
                            {
                                // This is the fist page of the acquisition
                                if ( nLastDib == 0 ||
                                    (pSource->IsNewJob() && pSource->IsJobFileHandlingOn()))
                                    nMultiStage = DIB_MULTI_FIRST;
                                else
                                // This is a subsequent page of the acquisition
                                    nMultiStage = DIB_MULTI_NEXT;

                                // Now check if this we are in manual duplex mode
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
                                    errfile = FileWriter.CopyDibToFile(CurDib, nMultiStage,
                                                            pSource->GetImageHandlerPtr(), 0);
                            }
                            else
                                errfile = FileWriter.CopyDibToFile(CurDib, nMultiStage, pSource->GetImageHandlerPtr(), 0);
                            m_nTotalPagesSaved++;
                        }
                        else
                            CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,
                                                                  DTWAIN_TN_PAGEDISCARDED,
                                                                 (LPARAM)pSource);

                        // Delete temporary bitmap here
                            if ( pSource->IsDeleteDibOnScan() )
                            {
                                // Let array class handle deleting of the DIB (Global memory will be freed only)
                                pArray->DeleteDibMemory( nLastDib );
                                m_ptrOrig = NULL;
                        }
                    }
                }
                pSource->SetTransferDone(true);
                if ( bInClip )
                    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession, NULL,
                                                          DTWAIN_TN_CLIPTRANSFERDONE,
                                                          (LPARAM)pSource->GetAcquireNum());
                if ( errfile != 0 )
                {
                   CTL_TwainAppMgr::SetError(errfile);
                   TCHAR szBuf[255];
                   CTL_TwainAppMgr::GetLastErrorString(szBuf, 254);
                   CTL_TwainAppMgr::WriteLogInfo(szBuf);
                   CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                                      NULL, DTWAIN_TN_INVALIDIMAGEFORMAT,
                                                      (LPARAM)GetSourcePtr());
                }

            }
            break;
            default:
            {
                CTL_StringStreamType strm;
                strm << _T("Unknown return code ") << rc << _T(" from DSM during transfer!  Twain driver unstable!\n");
                CTL_TwainAppMgr::WriteLogInfo(strm.str());
                break;
            }
            break;
        }
    } while (rc == TWRC_SUCCESS );

    // Delete the buffer if compression used and we have saved to a file
    if ( !pSource->GetUserStripBuffer())
    {
        if ( m_nCompression != TWCP_NONE/* &&
            pSource->GetAcquireType() == TWAINAcquireType_FileUsingNative */)

            ImageMemoryHandler::GlobalFreePr(m_ImageMemXferBuffer.Memory.TheMem);
    }
    else
    {
        if (m_nCompression != TWCP_NONE )
        {
            // Make sure that handle is unlocked
            while (ImageMemoryHandler::GlobalUnlock(m_DibStrip))
                ;
        }
    }

    // Prompt to save image here
    bool bRetval = true;
    bool bForceClose;
    if ( !bPageDiscarded && pSource->IsPromptPending())
    {
        bRetval = PromptAndSaveImage(pSource->GetPendingImageNum())?true:false;
        pSource->SetPromptPending(false);
    }

    // Force a close if Prompting returned FALSE.
    if ( bRetval == true )
        bForceClose = false;
    else
        bForceClose = true;
    AbortTransfer(bForceClose);
    return rc;
}


