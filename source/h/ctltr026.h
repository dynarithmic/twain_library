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
#ifndef CTLTR026_H_
#define CTLTR026_H_

#include "ctltr024.h"
#include "ctltr038.h"

namespace dynarithmic
{

    class CTL_ImageIOHandler;
    class ImageXferFileWriter;

    class CTL_ImageXferTriplet : public CTL_ImageTriplet
    {
        public:
            CTL_ImageXferTriplet(CTL_ITwainSession *pSession,
                                 CTL_ITwainSource *pSource,
                                 TW_UINT16 nType);

            HANDLE          GetDibHandle();

            TW_UINT16       Execute();
            bool            IsScanPending() const;
            int             GetTotalScannedPages() const;
            int             GetTransferType() const;
            int             PromptAndSaveImage(size_t nImageNum);
            CTL_TwainFileFormatEnum GetFileTypeFromCompression(int nCompression);
            int             GetAcquireFailAction() const { return m_nFailAction; }
            void            SetAcquireFailAction(int nAction) { m_nFailAction = nAction; }
            static void     ResolveImageResolution(CTL_ITwainSource *pSource,  DTWAINImageInfoEx* ImageInfo);
            bool            ResetTransfer(TW_UINT16 Msg = MSG_RESET);


        protected:
            bool            AbortTransfer(bool bForceClose = false);
            CTL_StringType  GetPageFileName(const CTL_StringType& strBase, int nCurImage );
            bool            IsPendingXfersDone() const { return m_bPendingXfersDone; }
            void            SetPendingXfersDone(bool bSet) { m_bPendingXfersDone = bSet; }
            TW_PENDINGXFERS& GetLocalPendingXferInfo() { return m_PendingXfers; }
            void            SetLastPendingInfoCode(TW_UINT16 code) { m_lastPendingXferCode = code; }
            TW_UINT16       GetLastPendingInfoCode() const { return m_lastPendingXferCode; }
            bool            CancelAcquisition();
            bool            FailAcquisition();

            bool            StopFeeder();
            bool            IsJobControlPending(TW_PENDINGXFERS *pPending);
            bool            CopyDibToClipboard(CTL_ITwainSession *pSession, HANDLE hDib);
            TW_UINT16       GetImagePendingInfo(TW_PENDINGXFERS *pPI, TW_UINT16 nMsg=MSG_ENDXFER);
            CTL_ImageIOHandler *GetImageHandler() { return m_pImgHandler; }
            CTL_ImageIOHandler *m_pImgHandler;
            static LONG CloseMultiPageDibFile();
            bool CropDib(CTL_ITwainSession *pSession,
                         CTL_ITwainSource *pSource,
                         CTL_TwainDibPtr CurDib);
            bool ResampleDib(CTL_ITwainSession *pSession,
                             CTL_ITwainSource *pSource,
                             CTL_TwainDibPtr CurDib);

            bool NegateDib(CTL_ITwainSession *pSession, CTL_ITwainSource *pSource, CTL_TwainDibPtr CurDib);

            bool ResampleBppForJPEG(CTL_ITwainSession *pSession,
                                     CTL_ITwainSource *pSource,
                                     CTL_TwainDibPtr CurDib);
            bool ChangeBpp(CTL_ITwainSession *pSession,
                           CTL_ITwainSource *pSource,
                           CTL_TwainDibPtr CurDib);

            bool ResampleBppForGIF(CTL_ITwainSession *pSession,
                                    CTL_ITwainSource *pSource,
                                    CTL_TwainDibPtr CurDib);

            bool ResampleBppForPDF(CTL_ITwainSession *pSession,
                                    CTL_ITwainSource *pSource,
                                    CTL_TwainDibPtr CurDib);

            bool ResampleBppForWBMP(CTL_ITwainSession *pSession,
                                    CTL_ITwainSource *pSource,
                                    CTL_TwainDibPtr CurDib);

            bool IsPageBlank(CTL_ITwainSession *pSession,
                             CTL_ITwainSource *pSource,
                             CTL_TwainDibPtr CurDib);

            int ProcessBlankPage(CTL_ITwainSession *pSession,
                CTL_ITwainSource *pSource,
                CTL_TwainDibPtr CurDib,
                LONG message_to_send1,
                LONG message_to_send2,
                LONG option_to_test);

            void SaveJobPages(ImageXferFileWriter& FileWwriter);
            bool ModifyAcquiredDib();
            bool QueryAndRemoveDib(CTL_TwainAcquireEnum acquireType, size_t nWhich);
            bool ResampleAcquiredDib();



    /*        void EndProcessingImageFile(CTL_ITwainSource *pSource,
                                        ImageXferFileWriter& FileWriter,
                                        bool bSaveFile=true);*/

        protected:
            HANDLE          m_hDib;
            int             m_nTotalPagesSaved;
            bool            m_bJobControlPageRecorded;
            bool            m_bJobMarkerNeedsToBeWritten;

        private:
            bool            m_bScanPending;
            int             m_nTotalPages;
            int             m_nTransferType;
            int             m_nFailAction;
            bool            m_bPendingXfersDone;
            TW_PENDINGXFERS m_PendingXfers;
            TW_UINT16       m_lastPendingXferCode;
    };
}
#endif


