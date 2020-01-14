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
#include <algorithm>
#include <boost/filesystem.hpp>
#include "imagexferfilewriter.h"
#include "ctltr026.h"
#include "ctltwmgr.h"

#define DEMO_MAX_PAGES 10000

using namespace dynarithmic;
static void SendFileAcquireError(CTL_ITwainSource *pSource, CTL_ITwainSession *pSession,
                                 LONG Error, LONG ErrorMsg );

ImageXferFileWriter::ImageXferFileWriter() : m_pTrip(NULL), m_pSession(NULL),
m_pSource(NULL)
{ }

ImageXferFileWriter::ImageXferFileWriter(CTL_ImageXferTriplet* pTrip,
                                         CTL_ITwainSession *pSession,
                                         CTL_ITwainSource *pSource) :
                                         m_pTrip(pTrip), m_pSession(pSession), m_pSource(pSource)
{ }


int ImageXferFileWriter::CopyDibToFile(CTL_TwainDibPtr pCurDib,
                                        int MultipageOption,
                                        CTL_ImageIOHandlerPtr& pHandler,
                                        LONG rawBytes,
                                        bool bIsJobControl/*=false*/)
{
    if ( !bIsJobControl)
    {
        // Adjust name if the image page > 0
        CTL_StringType strTempFile = m_pSource->GetAcquireFile(); 
        CTL_StringType s;

        if ( !MultipageOption || MultipageOption == DIB_MULTI_FIRST )
            m_pSource->SetLastAcquiredFileName(s);

        // Check if saving via common control.  TWAIN does not like File dialog!
        long lFlags   = m_pSource->GetAcquireFileFlags();
        if ( lFlags & DTWAIN_USEPROMPT )
            return TRUE;

        // Only get a new file name if this is not a multi-page file,
        // or if this is the first page of a multi-page scan
        if ( MultipageOption == 0 || MultipageOption == DIB_MULTI_FIRST)
        {
            if ( lFlags & TWAINFileFlag_PROMPT )
                strTempFile = m_pSource->PromptForFileName();
            else
                strTempFile = m_pSource->GetCurrentImageFileName();

            // save the file name
            m_pSource->SetActualFileName(strTempFile);

            // send out notification that file will be saved right now
            CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGING, (LPARAM)m_pSource);

            // check the name to see if was changed
            CTL_StringType strTempFileNew = m_pSource->GetActualFileName();
            if ( strTempFileNew != strTempFile )
                // name changed, so notify that it has.
                CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGED, (LPARAM)m_pSource);

            // swap out old name with new name, even if they're the same
            std::swap(strTempFile, strTempFileNew);
        }

        // Write this file if using Buffered Mode transfer
        if ( rawBytes > 0 )
        {
            // Just write the data to a file since this may be a compressed image
            HANDLE hnd = pCurDib->GetHandle();
            DTWAINGlobalHandle_RAII dibHandle(hnd);
            char* pImage = (char*)ImageMemoryHandler::GlobalLock(hnd);
            if ( pImage )
            {
                std::ofstream fh(StringConversion::Convert_Native_To_Ansi(strTempFile), std::ios::binary);
                if ( !fh )
                {
                    SendFileAcquireError(m_pSource, m_pSession, DTWAIN_ERR_FILEWRITE, DTWAIN_TN_FILESAVEERROR);
                    return DTWAIN_ERR_FILEWRITE;
                }
                fh.write(pImage, rawBytes);
                fh.close();
                if (boost::filesystem::file_size(strTempFile) == 0)
                {
                    SendFileAcquireError(m_pSource, m_pSession,
                                         DTWAIN_ERR_FILEWRITE, DTWAIN_TN_FILESAVEERROR);
                    CTL_TwainAppMgr::SetError(DTWAIN_ERR_FILEWRITE);
                    return DTWAIN_ERR_FILEWRITE;
                }
                else
                {
                    if ( !MultipageOption || MultipageOption == DIB_MULTI_FIRST )
                        m_pSource->SetLastAcquiredFileName( strTempFile );
                    CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL, DTWAIN_TN_FILESAVEOK, (LPARAM)m_pSource);
                    return 0;
                }
            }
            else
            {
                SendFileAcquireError(m_pSource, m_pSession,
                                     DTWAIN_ERR_DIB, DTWAIN_TN_FILESAVEERROR);
                return DTWAIN_ERR_DIB;
            }
        }

        return CopyDibToFileEx(pCurDib, MultipageOption, pHandler, strTempFile);
    }
    return 0;
}


int ImageXferFileWriter::CopyDibToFileEx(CTL_TwainDibPtr pCurDib,
                                          int MultipageOption,
                                          CTL_ImageIOHandlerPtr& pHandler,
                                          const CTL_StringType& strTempFile)
{
    DTWAINImageInfoEx ImageInfo;

    CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL, DTWAIN_TN_FILEPAGESAVING, (LPARAM)m_pSource);

    // Get any relevant Extra Image information
    m_pSource->GetImageInfoEx(ImageInfo);

    // Set information for possible TIFF file transfer
    // Set information for possible TIFF file transfer
    ImageInfo.ResolutionX = 300;
    ImageInfo.ResolutionY = 300;
    ImageInfo.theSession = m_pSession;
    if (!DTWAIN_GetSourceUnit(m_pSource, &ImageInfo.UnitOfMeasure))
        ImageInfo.UnitOfMeasure = DTWAIN_INCHES;

    CTL_ImageXferTriplet::ResolveImageResolution(m_pSource, &ImageInfo);


    // Now check for Postscript file types.  We alias these
    // types as TIFF format
    CTL_TwainFileFormatEnum FileType = m_pSource->GetAcquireFileType();
    if ( CTL_ITwainSource::IsFileTypePostscript( FileType ) )
    {
        ImageInfo.IsPostscript = true;
        ImageInfo.IsPostscriptMultipage =
            CTL_ITwainSource::IsFileTypeMultiPage( FileType );
        ImageInfo.PostscriptType = (LONG)FileType;
    }

    if ( MultipageOption == 0 || (m_pSource->IsMultiPageModeSaveAtEnd()
                          && !CTL_ITwainSource::IsFileTypeMultiPage( FileType ))
    )
    {
        int retval = pCurDib->WriteDibBitmap(ImageInfo, strTempFile.c_str(), m_pSource->GetAcquireFileType());
        if ( retval != 0 )
           SendFileAcquireError(m_pSource, m_pSession, retval, DTWAIN_TN_FILEPAGESAVEERROR);

        else
        {
            if ( !MultipageOption || MultipageOption == DIB_MULTI_FIRST )
                m_pSource->SetLastAcquiredFileName( strTempFile );
            CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession,
                                                  NULL, DTWAIN_TN_FILEPAGESAVEOK,
                                                  (LPARAM)m_pSource);
        }
        return retval;
    }

    // Write a multi page file
    int nStatus = 0;
    if ( MultipageOption == DIB_MULTI_FIRST)
        pHandler = pCurDib->WriteFirstPageDibMulti(ImageInfo, strTempFile.c_str(), m_pSource->GetAcquireFileType(), 0, 0, nStatus);
    else
    if ( MultipageOption == DIB_MULTI_NEXT)
        pCurDib->WriteNextPageDibMulti(pHandler, nStatus, ImageInfo);
    if ( nStatus != 0 )
    {
        SendFileAcquireError(m_pSource, m_pSession,
                                nStatus, DTWAIN_TN_FILEPAGESAVEERROR);
    }
    else
    {
        if ( !MultipageOption || MultipageOption == DIB_MULTI_FIRST )
           m_pSource->SetLastAcquiredFileName( strTempFile );
       CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL, DTWAIN_TN_FILEPAGESAVEOK,(LPARAM)m_pSource);
    }
    return nStatus;
}

LONG ImageXferFileWriter::MergeDuplexFiles()
{
    // Delayed writing of multipage image files.
    // Set pending image job number.
    m_pSource->SetPendingImageNum(0);
    m_pSource->SetPendingJobNum(0);
    m_pSource->SetBlankPageCount(0);

    sDuplexFileData DupData;

    // first get the number of items
    unsigned long nFiles[2];
    nFiles[0] = m_pSource->GetNumDuplexFiles(0);  // first side
    nFiles[1] = m_pSource->GetNumDuplexFiles(1);  // second side

    if ( nFiles[0] == 0 && nFiles[1] == 0 )
        return 1;
    int retval;

    // Only do this if multi-page continuous mode is off
    if ( !m_pSource->IsMultiPageModeContinuous() &&
         !m_pSource->IsMultiPageModeSaveAtEnd() )
    {
        if ( nFiles[0] != nFiles[1] )
        {
            retval = ProcessManualDuplexState(DTWAIN_TN_MANDUPPAGECOUNTERROR);
            if (retval != 0)
                return 1;
        }
    }

    // should be the same, but this is to make sure
    int nTotalFiles = (std::min)(nFiles[0], nFiles[1]);

    bool bNotManualDuplex = m_pSource->IsMultiPageModeContinuous() ||
                            m_pSource->IsMultiPageModeSaveAtEnd();

    if ( bNotManualDuplex )
    {
        nTotalFiles = nFiles[0];
    }

    // For front-first, we merge
    // first[0] --> second[last]
    // first[1] --> second[last-1]
    // first[2] --> second[last-2] ...
//    BYTE *pDibBuffer;

//    OFSTRUCT of;
//    int nHandle;
    int MultiPageOption = DIB_MULTI_FIRST;
    CTL_StringType strTempFile = m_pSource->GetAcquireFile(); 
    m_pSource->SetActualFileName(strTempFile);
    CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGING, (LPARAM)m_pSource);
    CTL_StringType strTempFileNew = m_pSource->GetActualFileName();
    if ( strTempFile != strTempFileNew )
        CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGED, (LPARAM)m_pSource);
    std::swap(strTempFile, strTempFileNew);

    CTL_ImageIOHandlerPtr& pHandler = m_pSource->GetImageHandlerPtr();
    bool bLastWriteDone = false;
    int nIncrement[2] = {0,0};
    int nCurPage[2] ={0,0};
	int nWhichSide[2] = { 0,0 };

    LONG nFlags = m_pSource->GetManualDuplexModeFlags();
    if ( bNotManualDuplex )
      nFlags = DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE;

    bool IsOddSide1 = true;

    switch (nFlags)
    {
        case DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE:
            nIncrement[0] = 1;
            nIncrement[1] = -1;
            nCurPage[0] = 0;
            nCurPage[1] = nTotalFiles - 1;
            nWhichSide[0] = 0;
            nWhichSide[1] = 1;
        break;

        case DTWAIN_MANDUP_FACEDOWNTOPPAGE:
            nIncrement[0] = -1;
            nIncrement[1] = 1;
            nCurPage[0] = nTotalFiles - 1;
            nCurPage[1] = 0;
            nWhichSide[0] = 1;
            nWhichSide[1] = 0;
            IsOddSide1 = false;
        break;

        case DTWAIN_MANDUP_FACEUPBOTTOMPAGE:
            nIncrement[0] = -1;
            nIncrement[1] = 1;
            nCurPage[0] = nTotalFiles - 1;
            nCurPage[1] = 0;
            nWhichSide[0] = 0;
            nWhichSide[1] = 1;
        break;

        case DTWAIN_MANDUP_FACEUPTOPPAGE:
            nIncrement[0] = 1;
            nIncrement[1] = -1;
            nCurPage[0] = 0;
            nCurPage[1] = nTotalFiles - 1;
            nWhichSide[0] = 0;
            nWhichSide[1] = 1;
            IsOddSide1 = false;
        break;
    }
    bool bDoneFirstPage = false;
    int filecount = 0;
    // perform loop for matching pages
    while (filecount < nTotalFiles )
    {
        int currentside = 0;
        while ( currentside < 2 )
        {
            // front side
            if ( currentside == 0 || (currentside == 0 && m_pSource->IsMultiPageModeContinuous()))
                DupData = m_pSource->GetDuplexFileData( nCurPage[0], nWhichSide[0] );
            else
            if ( bNotManualDuplex )
            {
                // Always front side if not manual duplex mode -- skip back side
                ++currentside;
                continue;
            }
            else
                // back side
                DupData = m_pSource->GetDuplexFileData( nCurPage[1], nWhichSide[1] );

            // Is this a job control marker?
            if ( DupData.bIsJobControlPage )
            {
                // This is a job control page, so end this set and start
                // another file
                if ( bDoneFirstPage )
                {
                    // close the currently created file
                    ManualDuplexCleanUp(_T(""), false);

                    // increase the job number
                    m_pSource->SetPendingJobNum(m_pSource->GetPendingJobNum() + 1);
                    strTempFile = m_pSource->GetCurrentImageFileName();
                    m_pSource->SetActualFileName(strTempFile);
                    CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGING, (LPARAM)m_pSource);
                    CTL_StringType strTempFile2 = m_pSource->GetActualFileName();
                    if ( strTempFile != strTempFile2 )
                        CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGED, (LPARAM)m_pSource);
                    std::swap(strTempFile, strTempFile2);
                }

                MultiPageOption = DIB_MULTI_FIRST;

                currentside = 0; // not a real page, so start side 1 again
                bDoneFirstPage = false;

                // Go to next sheet
                nCurPage[0] += nIncrement[0];
                nCurPage[1] += nIncrement[1];
                ++filecount;

                // Test if we are done writing all the pages
                if ( filecount >= nTotalFiles )
                {
                    // Indicate we have written all the pages and files
                    bLastWriteDone = true;
                    break;
                }
                continue;
            }
            else
            {
                if ( m_pSource->IsMultiPageModeSaveAtEnd() )
                    strTempFile = DupData.sRealFileName;

                if ( !bDoneFirstPage )
                    MultiPageOption = DIB_MULTI_FIRST;  // Fist page of multipage file
                else
                    MultiPageOption = DIB_MULTI_NEXT;   // Next page of mutlipage file

                retval = MergeDuplexFilesEx(DupData, pHandler, strTempFile, MultiPageOption);
                bDoneFirstPage = true;

                if ( retval != DTWAIN_MANDUP_SCANOK )
                {
                    // clean up temporary files
                    ManualDuplexCleanUp(strTempFile, true);
                    return 1;
                }
            }
            ++currentside;
        }
        // Go to next set of duplex information
        ++filecount;
        nCurPage[0] += nIncrement[0];
        nCurPage[1] += nIncrement[1];
    }

    // now if there are excess pages, add those to the image file
    bool bMergeExtra = false;
    if ( !bNotManualDuplex ) //!m_pSource->IsMultiPageModeContinuous())
    {
        if ( (nFiles[0] > nFiles[1]) && IsOddSide1)
        {
            // side 1 has page 1, and more pages than side 2.  Add
            // side 1 page to image
            DupData = m_pSource->GetDuplexFileData(nCurPage[0], nWhichSide[0]);
            bMergeExtra = true;
        }
        else
        if ( (nFiles[0] > nFiles[1]) && !IsOddSide1)
        {
            DupData = m_pSource->GetDuplexFileData(nCurPage[1], nWhichSide[1]);
            bMergeExtra = true;
        }
        if ( bMergeExtra )
        {
            retval = MergeDuplexFilesEx(DupData, pHandler, strTempFile, DIB_MULTI_NEXT);
            if (retval != DTWAIN_MANDUP_SCANOK )
            {
                ManualDuplexCleanUp(strTempFile, true);
                return 1;
            }
        }
    }
    // The file created OK
    if (!m_pSource->IsMultiPageModeSaveAtEnd() && !bLastWriteDone )
        ManualDuplexCleanUp(_T(""),false);  // Close multipage image file

    // Get rid of the multi-page temp files and reset the data structures.
    m_pSource->ResetManualDuplexMode();
    return 0; // ok
}

void ImageXferFileWriter::RecordBadDuplexPage()
{
    // Now save the information to the source
    m_pSource->AddDuplexFileData( DTWAIN_PAGEMISSINGSTR, 0, m_pSource->GetCurrentSideAcquired() );
}

int ImageXferFileWriter::ProcessManualDuplexState(LONG Msg)
{
    LRESULT retval;
    retval = CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL,
                Msg, (LPARAM)m_pSource);
    switch(retval)
    {
        case DTWAIN_MANDUP_SCANOK:  // merge the pages anyway
        default:
        break;

        case DTWAIN_MANDUP_SIDE1RESCAN:  // rescan side 1
        {
            m_pSource->ResetManualDuplexMode(0);
            m_pSource->SetCurrentSideAcquired(0);
        }
        break;
        case DTWAIN_MANDUP_SIDE2RESCAN:  // rescan side 2
        {
            m_pSource->ResetManualDuplexMode(1);
            m_pSource->SetCurrentSideAcquired(1);
        }
        break;

        case DTWAIN_MANDUP_RESCANALL: // start entire duplex scan over again
        {
            m_pSource->ResetManualDuplexMode();
            m_pSource->SetCurrentSideAcquired(0);
        }
    }
    return (int)retval;
}

int ImageXferFileWriter::MergeDuplexFilesEx(const sDuplexFileData& DupData,
                                            CTL_ImageIOHandlerPtr& pHandler,
                                            const CTL_StringType& strTempFile,
                                            int MultiPageOption)
{
    char* pDibBuffer;
    std::ifstream nHandle;
    int retval = 0;
    LONG nStatus;

    // Resize the dib buffer
    HANDLE pDibBufferHandle;
    pDibBuffer = (char *)CTL_TwainDLLHandle::s_TwainMemoryFunc->AllocateMemoryPtr(DupData.nBytes, &pDibBufferHandle);
    if ( !pDibBuffer )
    {
        retval = ProcessManualDuplexState(DTWAIN_TN_MANDUPMEMORYERROR);
        if (retval != DTWAIN_MANDUP_SCANOK)
            return retval;
    }

    // Open the file
    nHandle.open(StringConversion::Convert_Native_To_Ansi(DupData.sFileName).c_str(), std::ios::binary); // GENERIC_READ, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
    if ( nHandle )
    {
        std::streamsize numBytesRead;
        // Read the data into the buffer
        nHandle.read(pDibBuffer, DupData.nBytes);

        numBytesRead = nHandle.gcount();
        if (numBytesRead != DupData.nBytes)
        {
            retval = ProcessManualDuplexState(DTWAIN_TN_MANDUPFILEERROR);
            if (retval != DTWAIN_MANDUP_SCANOK)
                return 1;
        }
        else
        {
            // Now create a DIB
            CTL_TwainDibPtr ThisDib(new CTL_TwainDib(pDibBufferHandle));

            // Move this DIB to the image file
            nStatus = CopyDibToFileEx(ThisDib, MultiPageOption, pHandler, strTempFile);
            if (nStatus != 0)
            {
                retval = ProcessManualDuplexState(DTWAIN_TN_MANDUPFILESAVEERROR);
                if (retval != DTWAIN_MANDUP_SCANOK)
                    return 1;
            }
        }
    }
    return DTWAIN_MANDUP_SCANOK;
}

void ImageXferFileWriter::ManualDuplexCleanUp(const CTL_StringType& strFile/* = ""*/, bool bDestroyFile/*=false*/)
{
    CTL_TwainDib Dib;
    int nStatus;
    // Clean up by officially closing out the multi-page processing
    Dib.WriteLastPageDibMulti(m_pSource->GetImageHandlerPtr(), nStatus);

    if ( nStatus != 0 )
        SendFileAcquireError(m_pSource, m_pSession, nStatus, DTWAIN_TN_FILESAVEERROR);

    else
    {
        CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL, DTWAIN_TN_FILESAVEOK,
                                      (LPARAM)m_pSource);

    }
    // Destroy the file if it was created and we don't want it
    if (bDestroyFile)
        boost::filesystem::remove(strFile);
}


LONG ImageXferFileWriter::CopyDuplexDibToFile(CTL_TwainDibPtr pCurDib, bool bIsJobControl/*=false*/)

{
    if ( pCurDib && // If we have a bitmap

        (!bIsJobControl ||  // no job control or
        (bIsJobControl &&  // job control and handling jobs is on
         m_pSource->IsJobFileHandlingOn())))

    {
        CTL_StringType sRealName;
        if ( m_pSource->IsMultiPageModeSaveAtEnd() )
        {
            // Get the real name of the image file to be saved
            sRealName = m_pSource->GetCurrentImageFileName();
        }

        // We need to create a temporary file here
        CTL_StringType szTempPath = GetDTWAINTempFilePath();
        if ( szTempPath.empty())
        {
            RecordBadDuplexPage();
            return DTWAIN_ERR_FILEWRITE;
        }
        szTempPath += StringWrapper::GetGUID() + _T(".TMP"); // nRet = GetTempFileName(szTempPath.c_str(), _T("TMP"), 0, szTempFile);
        boost::filesystem::path p{ szTempPath };
        boost::filesystem::ofstream fh;
        // save the raw dib data to this file.
        fh.open(p, std::ios::binary);
        if ( !fh )
        {
            SendFileAcquireError(m_pSource, m_pSession, DTWAIN_ERR_FILEWRITE, DTWAIN_TN_FILESAVEERROR );
            RecordBadDuplexPage();
            return DTWAIN_ERR_FILEWRITE;
        }

        HANDLE hnd = pCurDib->GetHandle();
        DTWAINGlobalHandle_RAII dibHandle(hnd);

        BYTE *pDib = (BYTE *)ImageMemoryHandler::GlobalLock(hnd);
        if ( pDib )
        {
            fh.write(reinterpret_cast<const char *>(pDib), ImageMemoryHandler::GlobalSize(pCurDib->GetHandle()));
            if (fh.bad())
            {
                CTL_StringStreamType strm;
                strm << _T("Error writing data to file ") << szTempPath;
                CTL_TwainAppMgr::WriteLogInfo(strm.str());
                return DTWAIN_ERR_FILEWRITE;
            }
        }

        // Now save the information to the source for later processing
        m_pSource->AddDuplexFileData( szTempPath, static_cast<unsigned long>(ImageMemoryHandler::GlobalSize(pCurDib->GetHandle())),
                                      m_pSource->GetCurrentSideAcquired(),
                                      sRealName);
    }
    if ( bIsJobControl)
    {
        // Add job control marker
        m_pSource->AddDuplexFileData( _T(""), 0, m_pSource->GetCurrentSideAcquired(), _T(""), true);
    }

    return 0;
}

bool ImageXferFileWriter::ProcessFailureCondition(int nAction)
{
    // Set this triplet's failure code here, !and only here!
    if ( m_pTrip )
        m_pTrip->SetAcquireFailAction(nAction);

    switch(nAction)
    {
        case DTWAIN_PAGEFAIL_RETRY:
            if ( m_pTrip )
                m_pTrip->ResetTransfer(MSG_ENDXFER);
            return true;

        default:
        {
            if ( m_pTrip )
            {
                m_pTrip->ResetTransfer(MSG_ENDXFER);
                m_pTrip->ResetTransfer(MSG_RESET);
            }
            // Check if canceled by user
            CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL, DTWAIN_TN_PAGECANCELLED, (LPARAM)m_pSource);
            // Send a message to close things down if
            // there was no user interface chosen
            if ( !m_pSource->IsUIOpenOnAcquire() )
                CTL_TwainAppMgr::EndTwainUI(m_pSession, m_pSource);

            // Close any open multi page DIB files
            if ( m_pTrip )
                EndProcessingImageFile(m_pSource->IsFileIncompleteSave());
        }
    }
    return true;
}


LONG ImageXferFileWriter::CloseMultiPageDibFile(bool bSaveFile/*=true*/)
{
    if( m_pSource->GetAcquireType() == TWAINAcquireType_FileUsingNative )
    {
        bool bTrueMultiPage = CTL_ITwainSource::IsFileTypeMultiPage(m_pSource->GetAcquireFileType());
        bool bIsMultiPageFile = bTrueMultiPage || m_pSource->IsMultiPageModeSaveAtEnd();
        if ( bIsMultiPageFile )
        {
            if (  !m_pSource->IsManualDuplexModeOn() &&
                  !m_pSource->IsMultiPageModeContinuous())
            {
                if ( !m_pSource->IsMultiPageModeSaveAtEnd() || bTrueMultiPage )
                {
                    CTL_TwainDib Dib;
                    int nStatus;
                    int nWriteOK = Dib.WriteLastPageDibMulti(m_pSource->GetImageHandlerPtr(), nStatus, bSaveFile);
                    // Get rid of the multi-page temp files and reset the data structures.
                    m_pSource->ResetManualDuplexMode();
                    return nWriteOK;
                }
            }

            // Set this side done
            // Check for manual duplex mode.
            if ( m_pSource->IsManualDuplexModeOn() ||
                    m_pSource->IsMultiPageModeContinuous() ||
                    m_pSource->IsMultiPageModeSaveAtEnd() )
            {
                // Set this side as done
                m_pSource->SetManualDuplexSideDone(m_pSource->GetCurrentSideAcquired());

                // merge files if this is a simple save after the acquisition has ended
                if (m_pSource->IsMultiPageModeSaveAtEnd() && !m_pSource->IsManualDuplexModeOn() && !bTrueMultiPage)
                    return MergeDuplexFiles();


                // if not done, then do the other side
                if ( !m_pSource->IsManualDuplexDone() && !m_pSource->IsMultiPageModeContinuous())
                {
                    CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL,
                                                            DTWAIN_TN_MANDUPSIDE1DONE,
                                                            (LPARAM)m_pSource);
                    m_pSource->SetCurrentSideAcquired(!m_pSource->GetCurrentSideAcquired());
                    CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL,
                                                            DTWAIN_TN_MANDUPSIDE2START,
                                                            (LPARAM)m_pSource);
                }
                else
                {
                    if ( !m_pSource->IsMultiPageModeContinuous())
                        CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL,
                                                                DTWAIN_TN_MANDUPSIDE2DONE,
                                                                (LPARAM)m_pSource);
                    // merge the images into one file
                    return MergeDuplexFiles();
                }
            }
        }
    }
    return 0;
}

void ImageXferFileWriter::EndProcessingImageFile(bool bSaveFile/*=true*/)
{
    if ( m_pSource->IsMultiPageModeSaveAtEnd() &&
         !CTL_ITwainSource::IsFileTypeMultiPage( m_pSource->GetAcquireFileType() ))
    {
        m_pSource->ProcessMultipageFile();
    }
    else
    if ( !m_pSource->IsMultiPageModeContinuous() )
    {
      if ( CloseMultiPageDibFile(bSaveFile) == 0 )
      {
          if ( bSaveFile )
            CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession,
                                              NULL, DTWAIN_TN_FILESAVEOK,
                                              (LPARAM)m_pSource);
      }
      else
      if ( bSaveFile )
        CTL_TwainAppMgr::SendTwainMsgToWindow(m_pSession, NULL, DTWAIN_TN_FILESAVEERROR, (LPARAM)m_pSource);
    }
}

void SendFileAcquireError(CTL_ITwainSource *pSource, CTL_ITwainSession *pSession,
                          LONG Error, LONG ErrorMsg )
{
    static const int MaxMessage = 1024;
    CTL_TwainAppMgr::SetError(Error);
    TCHAR szBuf[MaxMessage + 1];
    CTL_TwainAppMgr::GetLastErrorString(szBuf, MaxMessage);
    CTL_TwainAppMgr::WriteLogInfo(szBuf);
    CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,NULL, ErrorMsg, (LPARAM)pSource);
}



