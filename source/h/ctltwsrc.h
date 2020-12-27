/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
#ifndef CTLTwSrc_h_
#define CTLTwSrc_h_

#include <unordered_map>
#include <vector>
#include <unordered_set>
#include <boost/any.hpp>
#include <array>

#include "ctlobstr.h"
#include "ctltwain.h"
#include "ctlarray.h"
#include "ctldib.h"
#include "ctlenum.h"
#include "dtwtype.h"
#include "ctldevnt.h"
#include "dtwdecl.h"

namespace dynarithmic
{
    typedef std::unordered_map<TW_UINT16, short int> CapToStateMap;
    typedef std::unordered_set<TW_UINT16> CapList;
    typedef std::vector<TW_UINT16> JobControlList;
    typedef std::vector<TW_INFO> TWINFOVector;

    class CTL_TwainDLLHandle;
    class CTL_ITwainSource;
    class CTL_TwainAppMgr;

    class CTL_ITwainSession;
    class CTL_ITwainSource;
    class CTL_TwainDibArray;
    class CTL_TwainDib;
    class CTL_TwainCapMap;
    class CTL_ImageIOHandler;
    class CTL_ExtImageInfoTriplet;


    enum SourceState {SOURCE_STATE_CLOSED=3,
                      SOURCE_STATE_OPENED=4,
                      SOURCE_STATE_UIENABLED=5,
                      SOURCE_STATE_XFERREADY=6,
                      SOURCE_STATE_TRANSFERRING=7
                        };

    struct sDuplexFileData
    {
        CTL_StringType  sFileName;
        CTL_StringType  sRealFileName;
        unsigned long nBytes;
        bool bIsJobControlPage;
        sDuplexFileData() : nBytes(0), bIsJobControlPage(false) {}
    };

    typedef  std::pair<
             std::vector<sDuplexFileData>,
             std::vector<sDuplexFileData> > DuplexData;

    class CTL_ITwainSource
    {
        struct container_values
        {
            int m_dataType;
            std::vector<boost::any> m_data;
        };

        typedef std::unordered_map<TW_UINT16, container_values> CapToValuesMap;
        CapToValuesMap m_capToValuesMap_G;
        CapToValuesMap m_capToValuesMap_GD;

    public:
            friend class CTL_ITwainSession;
            ~CTL_ITwainSource();
            enum { CROP_FLAG = 1, SCALE_FLAG = 2, RESIZE_FLAG = 4};
            static CTL_ITwainSource *Create( CTL_ITwainSession* pSession,
                                             LPCTSTR lpszProductName = NULL);

            static void Destroy( CTL_ITwainSource* pSource);
            static bool Close( CTL_ITwainSource* pSource, bool bRemainLoaded);
            operator TW_IDENTITY* () const { return GetSourceIDPtr(); }

            CTL_ITwainSession *GetTwainSession() { return m_pSession; }

            bool isCapValuesCached(TW_UINT16 lCap, LONG getType) const
            {
               const CapToValuesMap* mapToUse = &m_capToValuesMap_G;
               if ( getType == DTWAIN_CAPGETDEFAULT)
                    mapToUse = &m_capToValuesMap_GD;
                return mapToUse->find(lCap) != mapToUse->end();
            }

            DTWAIN_ARRAY getCapCachedValues(TW_UINT16 lCap, LONG getType);
            bool setCapCachedValues(DTWAIN_ARRAY array, TW_UINT16 lCap, LONG getType);

            TW_IDENTITY *GetSourceIDPtr() const { return (TW_IDENTITY*)&m_SourceId; }

            TW_UINT32    GetId() const          { return m_SourceId.Id; }
            const TW_VERSION*  GetVersion() const { return &m_SourceId.Version; }
            TW_UINT16    GetProtocolMajor() const { return m_SourceId.ProtocolMajor; }
            TW_UINT16    GetProtocolMinor() const { return m_SourceId.ProtocolMinor; }
            TW_UINT32    GetSupportedGroups() const { return m_SourceId.SupportedGroups; }
            CTL_StringType GetManufacturer() const { return StringConversion::Convert_AnsiPtr_To_Native(m_SourceId.Manufacturer); }
            CTL_StringType GetProductFamily() const { return StringConversion::Convert_AnsiPtr_To_Native(m_SourceId.ProductFamily); }
            CTL_StringType GetProductName() const { return StringConversion::Convert_AnsiPtr_To_Native(m_SourceId.ProductName); }
            bool         IsOpened() const { return m_bIsOpened; }
            bool         IsSelected() const { return m_bIsSelected; }
            void         SetSelected(bool bSet) { m_bIsSelected = bSet; }
            void         SetUIOpen(bool bSet);
            bool         IsUIOpen() const { return m_bUIOpened; }
            void         SetUIOpenOnAcquire(bool bSet) { m_bUIOnAcquire = bSet; }
            bool         IsUIOpenOnAcquire() const { return m_bUIOnAcquire; }
            void         SetAcquireFileType(CTL_TwainFileFormatEnum FileType)
                                            { m_nFileAcquireType = FileType; }
            CTL_TwainFileFormatEnum GetAcquireFileType() const
                                    { return m_nFileAcquireType; }
            void         Clone( CTL_ITwainSource* pSource);
            void         SetActive(bool bSet);
            bool         IsActive() const;
            HWND         GetOutputWindow() const;
            void         SetDibHandle(HANDLE hDib, size_t nWhich=0);
            void         SetDibHandleNoPalette(HANDLE hDib, int nWhich=0);

            HANDLE       GetDibHandle(int nWhich=0) const;
            CTL_TwainDibPtr GetDibObject(int nWhich=0) const;
            bool         RenderImage(int nWhich=0);
            void         SetFeederEnableMode( bool bMode=true);
            bool         IsFeederEnabledMode() const;
            void         SetAutoFeedMode(bool bMode=true) { m_bAutoFeed = bMode; }
            bool         GetAutoFeedMode() const { return m_bAutoFeed; }
            bool         IsDeleteDibOnScan() const { return m_bDeleteOnScan; }
            void         SetDeleteDibOnScan(bool bSet=true) { m_bDeleteOnScan = bSet; }
            bool         SetDibAutoDelete(bool bSet=true);
            void         SetAcquireType(CTL_TwainAcquireEnum AcquireType,
                                        LPCTSTR lpszFile=NULL);
            void         SetAcquireNum(LONG lNum) { m_lAcquireNum = lNum; }
            LONG         GetAcquireNum() const { return m_lAcquireNum; }
            long         GetMaxAcquireCount() const { return m_nAcquireCount; }
            void         SetMaxAcquireCount(int nAcquire) { m_nAcquireCount = nAcquire; }
            CTL_TwainAcquireEnum  GetAcquireType() const { return m_AcquireType; }
            CTL_StringType GetAcquireFile() const { return m_strAcquireFile; }
            void         SetAcquireFile(LPCTSTR szFile) { m_strAcquireFile = szFile; }
            long         GetAcquireFileFlags() const { return m_lFileFlags; }
            void         SetAcquireFileFlags(long lFileFlags) { m_lFileFlags = lFileFlags; }
            static bool  IsFileTypeMultiPage(CTL_TwainFileFormatEnum FileType);
            static CTL_TwainFileFormatEnum GetMultiPageType(CTL_TwainFileFormatEnum FileType);
            static bool  IsFileTypeTIFF(CTL_TwainFileFormatEnum FileType);
            static bool  IsFileTypePostscript(CTL_TwainFileFormatEnum FileType);

            void         SetPendingImageNum( long nImageNum )
                        { m_nImageNum = nImageNum; }
            long        GetPendingImageNum() const { return m_nImageNum; }
            void         SetPendingJobNum( int nJobNum )
                        { m_nJobNum = nJobNum; }
            int          GetPendingJobNum() const { return m_nJobNum; }
            bool         IsNewJob() const;
            void         ResetJob() { m_bJobStarted = false; }
            void         StartJob() { m_bJobStarted = true; }
            void         SetJobFileHandling(bool bSet=true) { m_bJobFileHandling = bSet; }
            bool         IsJobFileHandlingOn() const { return m_bJobFileHandling; }
            bool         CurrentJobIncludesPage() const { return
                            (m_nJobControl == DTWAIN_JC_JSIS || m_nJobControl == DTWAIN_JC_JSIC ||
                            m_nJobControl == DTWAIN_JCBP_JSIS || m_nJobControl == DTWAIN_JCBP_JSIC); }

            int          GetNumDibs() const;
            bool         SetCurrentDibPage(int nPage);
            int          GetCurrentDibPage();
            void         Reset();
            CTL_TwainDibArray* GetDibArray() { return m_DibArray.get(); }
            void         SetAcquireAttempt(bool bSet) { m_bAcquireAttempt = bSet; }
            bool         IsAcquireAttempt() const { return m_bAcquireAttempt; }
            bool         IsSourceCompliant( CTL_EnumTwainVersion TVersion, CTL_TwainCapArray& rArray );
            void         RemoveAllDibs();
            CTL_StringType   GetCurrentImageFileName(); // const;
            CTL_StringType   GetImageFileName(int curFile=0);
            void         SetFileEnumerator(DTWAIN_ARRAY pDTWAINArray) { m_pFileEnumerator = pDTWAINArray; }
            DTWAIN_ARRAY GetFileEnumerator() { return m_pFileEnumerator; }
            void         SetTransferDone(bool bDone) { m_bTransferDone = bDone; }
            bool         GetTransferDone() const { return m_bTransferDone; }
            void         SetCapCacheValue( LONG lCap, double dValue, bool bTurnOn );
            double       GetCapCacheValue( LONG lCap, LONG FAR *pTurnOn ) const;
            CTL_StringType   PromptForFileName() const;
            bool         PromptAndSaveImage( int nCurImage );
            bool         IsAcquireStarted() const { return m_bAcquireStarted; }
            void         SetAcquireStarted(bool bSet) { m_bAcquireStarted = bSet; }
            void         SetModal(bool bSet) { m_bDialogModal = bSet; }
            bool         IsModal() const    { return m_bDialogModal; }
            bool         IsAcquireAutoClose() const { return m_bAcquireAutoClose; }
            void         SetOpenAfterAcquire(bool bSet) { m_bOpenAfterAcquire = bSet; }
            bool         IsOpenAfterAcquire() const { return m_bOpenAfterAcquire; }

            // Controls whether to reopen the source if it has been closed
            bool         IsReopenAfterAcquire() const { return (!m_bOpenAfterAcquire)?true:false; }
            int          GetMaxAcquisitions() const { return m_nMaxAcquisitions; }
            void         SetMaxAcquisitions(int nMax) { m_nMaxAcquisitions = nMax; }
            int          GetUIMaxAcquisitions() const { return m_nUIMaxAcquisitions; }
            void         SetUIMaxAcquisitions(int nMax) { m_nUIMaxAcquisitions = nMax; }
            int          GetAcquireCount() const { return m_nNumAcquires; }
            void         SetAcquireCount(int nCount) { m_nNumAcquires = nCount; }
            void         SetSpecialTransferMode(LONG lMode) { m_nSpecialMode = lMode; }
            LONG         GetSpecialTransferMode() const { return m_nSpecialMode; }
            TW_USERINTERFACE *GetTWUserInterface() { return &m_UserInterface; }
            void         ResetAcquisitionAttempts();
            void         AddDibsToAcquisition(DTWAIN_ARRAY aDibs);
            void         ResetAcquisitionAttempts(DTWAIN_ARRAY aNewAttempts);
            DTWAIN_ARRAY   GetAcquisitionArray();
            CTL_DeviceEvent GetDeviceEvent() const { return m_DeviceEvent; }
            void         SetDeviceEvent( const CTL_DeviceEvent& DevEvent )
                        {  m_DeviceEvent = DevEvent; }
            void         SetUIOnly(bool bSet) { m_bShowUIOnly = bSet; }
            bool         IsUIOnly() const { return m_bShowUIOnly; }
            void         SetCompressionType(LONG nCompression) { m_nCompression = nCompression; }
            LONG         GetCompressionType() const { return m_nCompression; }
            void         SetNumCompressBytes(LONG nCompressBytes) { m_nCompressBytes = nCompressBytes; }
            LONG         GetNumCompressBytes() const { return m_nCompressBytes; }
            void         SetState(SourceState nState) {m_nState = nState;}
            SourceState  GetState() const { return m_nState; }
            TW_UINT32    GetEOJDetectedValue() const { return m_EOJDetectedValue; }
            void         SetEOJDetectedValue(TW_UINT32 nValue) { m_EOJDetectedValue = nValue; }
            void         SetForceScanOnNoUI(bool bSet) { m_bForceScanOnNoUI = bSet; }
            bool         IsForceScanOnNoUI() const { return m_bForceScanOnNoUI; }
            void         AddCapToStateInfo(TW_UINT16 nCap, short int cStateInfo);
            bool         IsCapNegotiableInState(TW_UINT16 nCap, int nState) const;
            bool         IsCapabilityCached() const { return m_bCapCached; }
            void         SetCapCached(bool bSet) { m_bCapCached = bSet; }
            bool         IsCapabilityCached(TW_UINT16 nCap) const;
            void         SetCapCached(TW_UINT16 nCapability, bool bSet);
            int          IsCapSupportedFromCache(TW_UINT16 nCap);
            bool         RetrievedAllCaps() const { return m_bRetrievedAllCaps; }
            void         SetRetrievedAllCaps(bool bSet = true) { m_bRetrievedAllCaps = bSet; }
            bool         IsPromptPending() const { return m_bPromptPending; }
            void         SetPromptPending(bool bSet) { m_bPromptPending = bSet; }

            void         AddCapToUnsupportedList(TW_UINT16 nCap);
            void         AddCapToSupportedList(TW_UINT16 nCap);
            bool         IsCapInUnsupportedList(TW_UINT16 nCap) const;
            bool         IsCapInSupportedList(TW_UINT16 nCap) const;
            CapList&     GetCapSupportedList();
            void         SetCapSupportedList(CTL_TwainCapArray& rArray);
            void         SetFastCapRetrieval(bool bSet=true) { m_bFastCapRetrieval = bSet; }
            bool         IsFastCapRetrieval() const { return m_bFastCapRetrieval; }

            void         SetJpegValues(LONG nQuality, bool bProgressive) { m_ImageInfoEx.nJpegQuality = nQuality;
                                                                           m_ImageInfoEx.bProgressiveJpeg = bProgressive; }

            void         GetJpegValues(LONG &Quality, bool &bProgressive) const { Quality = m_ImageInfoEx.nJpegQuality;
                                                                                  bProgressive = m_ImageInfoEx.bProgressiveJpeg; }

            void         SetPDFValue(const CTL_StringType& nWhich, const CTL_StringType& sz);
            void         SetPDFValue(const CTL_StringType& nWhich, LONG nValue);
            void         SetPDFValue(const CTL_StringType& s, DTWAIN_FLOAT f1, DTWAIN_FLOAT f2);
            void         SetPDFValue(const CTL_StringType& nWhich, PDFTextElementPtr& element);
            void         SetPDFPageSize(LONG nPageSize, DTWAIN_FLOAT cWidth, DTWAIN_FLOAT cHeight);
            void         SetPDFEncryption(bool bIsEncrypted,
                                          const CTL_StringType& strOwnerPassword, const CTL_StringType& strUserPassword,
                                          LONG Permissions, bool bUseStrongEncryption);

            void         SetPhotometric(LONG Setting);

            void         GetImageInfoEx(DTWAINImageInfoEx &ImageInfoEx) const;
            DTWAINImageInfoEx& GetImageInfoExRef() { return m_ImageInfoEx; }

            void         SetCurrentJobControl(TW_UINT16 JobControl=TWJC_NONE) { m_nJobControl = JobControl; }
            TW_UINT16    GetCurrentJobControl() const { return m_nJobControl; }
            bool         IsTwainJobControl() const { return m_nJobControl > TWJC_NONE && m_nJobControl <= DTWAIN_JC_JSXS; }
            CTL_ImageIOHandlerPtr& GetImageHandlerPtr() { return m_pImageHandler; }
            int          GetAcquireFailureAction() { return m_nFailAction; }
            void         SetAcquireFailureAction(int nAction) { m_nFailAction = nAction; }
            void         SetMaxRetryAttempts(int nMax) {m_nMaxRetryAttempts = nMax; }
            int          GetMaxRetryAttempts() const { return m_nMaxRetryAttempts;  }
            void         SetCurrentRetryCount(int nCount) {m_nCurRetryCount = nCount; }
            int          GetCurrentRetryCount() const { return m_nCurRetryCount;    }
            bool         SkipImageInfoErrors() const { return m_bSkipImageInfoErrors; }
            void         SetImageInfoErrors(bool bSet=true) { m_bSkipImageInfoErrors = bSet; }
            void         SetImageInfo(TW_IMAGEINFO* pInfo) { memcpy(&m_ImageInfo, pInfo, sizeof(TW_IMAGEINFO)); }
            void         GetImageInfo(TW_IMAGEINFO* pInfo) { memcpy(pInfo, &m_ImageInfo, sizeof(TW_IMAGEINFO)); }
            void         SetImageLayout(const FloatRect* pInfo) { memcpy(&m_ImageLayout, pInfo, sizeof(FloatRect)); }
            void         GetImageLayout(FloatRect* pInfo) { memcpy(pInfo, &m_ImageLayout, sizeof(FloatRect)); }
            void         SetImageLayoutValid(bool bSet=true) { m_bImageLayoutValid = bSet; }
            bool         IsImageLayoutValid() const { return m_bImageLayoutValid; }
            void         SetAlternateAcquireArea(double left, double top, double right, double bottom, LONG UnitOfMeasure, LONG flags, bool bSet=true);
            void         GetAlternateAcquireArea(FloatRect& r, LONG& UnitOfMeasure, LONG& flags);
            void         SetImageScale(double xscale, double yscale, bool bSet=true);
            void         GetImageScale(double& xscale, double& yscale, LONG& flags);
            HANDLE       GetUserStripBuffer() const { return m_hAcquireStrip; }
            void         SetUserStripBuffer(HANDLE h) { m_hAcquireStrip = h; }
            SIZE_T       GetUserStripBufSize() const { return m_nUserStripSize; }
            void         SetUserStripBufSize(SIZE_T nSize) { m_nUserStripSize = nSize; }
            bool         IsUserBufferUsed() { return m_bUserStripUsed; }
            void         SetBufferStripData(TW_IMAGEMEMXFER* pMemXferBuffer) { m_pImageMemXfer = pMemXferBuffer; }
            TW_IMAGEMEMXFER* GetBufferStripData() const { return m_pImageMemXfer; }

            void         SetImagesStored(bool bSet=true) { m_bImagesStored = bSet; }
            bool         ImagesStored() const { return m_bImagesStored; }
            CTL_StringType   GetLastAcquiredFileName() const { return m_strLastAcquiredFile; }
            void         SetLastAcquiredFileName(const CTL_StringType& sName) { m_strLastAcquiredFile = sName; }
            TW_FILESYSTEM*  GetFileSystem() { return &m_FileSystem; }

            // Extended image info functions
            bool         InitExtImageInfo(int nNum);
            bool         GetExtImageInfo(bool bExecute);
            bool         AddExtImageInfo(const TW_INFO &Info);
            bool         EnumExtImageInfo(CTL_IntArray& r);
            TW_INFO      GetExtImageInfoItem(int nItem, int nSearch);
            bool         GetExtImageInfoData(int nWhichItem, int nSearch, int nWhichValue, LPVOID Data, size_t* pNumChars=NULL);
            void         SetUserAcquisitionArray(DTWAIN_ARRAY UserArray) { m_PersistentArray = UserArray; }
            DTWAIN_ARRAY GetUserAcquisitionArray() const { return m_PersistentArray; }

            bool         DestroyExtImageInfo();
            bool         IsExtendedCapNegotiable(LONG nCap);
            bool         AddCapToExtendedCapList(LONG nCap);

            void         SetFileAutoIncrement(bool bSet, LONG nIncrement)
            {
                m_bAutoIncrementFile = bSet;
                m_nFileIncrement = nIncrement;
            }

            bool         IsFileAutoIncrement() const { return m_bAutoIncrementFile; }
            bool         InitFileAutoIncrementData(const CTL_StringType& sName);
            void         SetFileAutoIncrementFlags(LONG nFlags){ m_nAutoIncrementFlags = nFlags; }
            LONG         GetFileAutoIncrementFlags() const { return m_nAutoIncrementFlags; }
            bool         ResetFileAutoIncrementData();
            void         SetFileAutoIncrementBase( LONG nInitial ) {m_nFileNameBaseNum = nInitial;}

            // Added for manual duplex mode processing
            bool         SetManualDuplexMode(LONG nFlags, bool bSet);
            bool         IsManualDuplexModeOn() const { return m_bManualDuplexModeOn; }
            LONG         GetManualDuplexModeFlags() const { return m_nManualDuplexModeFlags; }
            LONG         GetCurrentSideAcquired() const { return m_nCurrentSideAcquired; }
            void         SetCurrentSideAcquired(LONG nSide) { m_nCurrentSideAcquired = nSide; }
            void         ResetManualDuplexMode(int nWhich = -1);
            void         SetManualDuplexSideDone(int nSide) { m_bDuplexSideDone[nSide] = true; }
            bool         IsManualDuplexSideDone(int nSide) const { return m_bDuplexSideDone[nSide]; }
            bool         IsManualDuplexDone() const { return m_bDuplexSideDone[0] && m_bDuplexSideDone[1];}

            void         SetMultiPageScanMode(LONG ScanMode) { m_nMultiPageScanMode = ScanMode; }
            LONG         GetMutiPageScanMode() const { return m_nMultiPageScanMode; }
            bool         IsMultiPageModeSourceMode() const { return m_nMultiPageScanMode == DTWAIN_FILESAVE_SOURCECLOSE; }
            bool         IsMultiPageModeUIMode() const { return m_nMultiPageScanMode == DTWAIN_FILESAVE_UICLOSE; }
            bool         IsMultiPageModeDefaultMode() const { return m_nMultiPageScanMode == DTWAIN_FILESAVE_DEFAULT; }
            bool         IsMultiPageModeContinuous() const { return !IsMultiPageModeDefaultMode() &&
                                                                    !IsMultiPageModeSaveAtEnd(); }
            bool         IsMultiPageModeSaveAtEnd() const { return m_nMultiPageScanMode == DTWAIN_FILESAVE_ENDACQUIRE; }

            void         AddDuplexFileData(const CTL_StringType& fName, unsigned long nBytes, int nWhich,
                                           const CTL_StringType& RealName = _T(""), bool bIsJobControl=false);
            sDuplexFileData GetDuplexFileData( int nPage, int nWhich ) const;
            unsigned long GetNumDuplexFiles(int nWhich) const;
            void         RemoveDuplexFileData();
            LONG         MergeDuplexFiles();
            int          ProcessManualDuplexState(LONG Msg);
            void         DeleteDuplexFiles(int nWhich);
            void         SetImageInfoRetrieved(bool bSet) { m_bImageInfoRetrieved = bSet; }
            bool         IsImageInfoRetrieved() const {return m_bImageInfoRetrieved; }
            void         ManualDuplexCleanUp(const CTL_StringType& strFile= _T(""), bool bDestroyFile=false);
            void         ProcessMultipageFile();
            LONG         GetForcedImageBpp() const { return m_nForcedBpp; }
            void         SetForcedImageBpp(LONG bpp) { m_nForcedBpp = bpp; }
            void         SetFileIncompleteSaveMode( bool bSaveIncomplete ) { m_bIsFileSaveIncomplete = bSaveIncomplete; }
            bool         IsFileIncompleteSave() const { return m_bIsFileSaveIncomplete; }
            bool         IsBlankPageDetectionOn() const { return m_bIsBlankPageDetectionOn &&
                                                           m_nJobControl < DTWAIN_JCBP_JSIC; }
            void         SetBlankPageDetectionOn(bool bSet=true) { m_bIsBlankPageDetectionOn = bSet; }
            double       GetBlankPageThreshold() const { return m_dBlankPageThreshold; }
            void         SetBlankPageThreshold(double threshold) { m_dBlankPageThreshold = threshold; }
            void         SetBlankPageAutoDetect(LONG bSet) { m_lBlankPageAutoDetect = bSet;}
            bool         IsBlankPageAutoDetectOn() const { return (m_lBlankPageAutoDetect != DTWAIN_BP_AUTODISCARD_NONE); }
            LONG         GetBlankPageAutoDetect() const { return m_lBlankPageAutoDetect;}
            LONG         GetBlankPageCount() const { return m_nBlankPageCount; }
            void         SetBlankPageCount(LONG nCount) { m_nBlankPageCount = nCount; }
            void         SetImageNegative(bool bSet=true) { m_bImageNegative = bSet; }
            bool         IsImageNegativeOn() const { return m_bImageNegative; }
            bool         IsCurrentlyProcessingPixelInfo() const { return m_bProcessingPixelInfo; }
            void         SetCurrentlyProcessingPixelInfo(bool bSet=true) { m_bProcessingPixelInfo = bSet; }
            void         ClearPDFText();
            bool         IsTwainVersion2() const { return m_bDSMVersion2; }
            void         SetTwainVersion2(bool bSet = true) { m_bDSMVersion2 = bSet;  }
            void         SetActualFileName(const CTL_StringType& sName) { m_ActualFileName = sName;  }
            CTL_StringType   GetActualFileName() const { return m_ActualFileName;  }

            // Only public member
            void *      m_pUserPtr;

        private:
            void         AddCapToList(CapList& vList, TW_UINT16 nCap);
            bool         IsCapInList(const CapList& vList, TW_UINT16 nCap) const;


        protected:
            bool         CloseSource(bool bForce);
            CTL_ITwainSource( CTL_ITwainSession* pSession, LPCTSTR lpszProductName );
            void SetOpenFlag(bool bOpened)      { m_bIsOpened = bOpened; }
            void SetDibHandleProc(HANDLE hDib, size_t nWhich, bool bCreatePalette);
            int  MergeDuplexFilesEx(const sDuplexFileData& DupData,
                                    CTL_ImageIOHandler **pHandler,
                                    const CTL_StringType& strTempFile,
                                    int MultiPageOption);


        private:

            struct tagCapCacheInfo {
                    double Contrast;
                    double Brightness;
                    double XResolution;
                    double YResolution;
                    double XNativeResolution;
                    int    PixelFlavor;
                    int    BitDepth;
                    int    PixelType;
                    unsigned int UseContrast:1;
                    unsigned int UseBrightness:1;
                    unsigned int UseXResolution:1;
                    unsigned int UseYResolution:1;
                    unsigned int UsePixelFlavor:1;
                    unsigned int UseXNativeResolution:1;
                    unsigned int UseBitDepth:1;
                    unsigned int UsePixelType:1;
                } CapCacheInfo;

            bool            m_bDSMVersion2;
            bool            m_bIsOpened;
            bool            m_bIsSelected;
            TW_IDENTITY     m_SourceId;
            CTL_ITwainSession *m_pSession;
            bool            m_bUIOpened;
            bool            m_bPromptPending;
            bool            m_bActive;
            HWND            m_hOutWnd;
            std::shared_ptr<CTL_TwainDibArray> m_DibArray;
            bool            m_bUseFeeder;
            bool            m_bDibAutoDelete;
            CTL_StringType  m_strAcquireFile;
            CTL_StringType  m_strLastAcquiredFile;
            CTL_StringType  m_ActualFileName;
            CTL_TwainAcquireEnum m_AcquireType;
            long            m_nImageNum;
            int             m_nCurDibPage;
            bool            m_bDeleteOnScan;
            bool            m_bUIOnAcquire;
            CTL_TwainFileFormatEnum m_nFileAcquireType;
            long            m_lFileFlags;
            bool            m_bAcquireAttempt;
            int             m_nAcquireCount;
            long            m_lAcquireNum;
            DTWAIN_ARRAY    m_pFileEnumerator;
            bool            m_bTransferDone;
            bool            m_bAcquireStarted;
            bool            m_bDialogModal;
            bool            m_bOpenAfterAcquire;
            bool            m_bAcquireAutoClose;
            int             m_nMaxAcquisitions;
            int             m_nUIMaxAcquisitions;
            int             m_nNumAcquires;
            LONG            m_nSpecialMode;
            TW_USERINTERFACE m_UserInterface;
            DTWAIN_ARRAY      m_aAcqAttempts;
            CTL_DeviceEvent m_DeviceEvent;
            bool            m_bShowUIOnly;
            LONG            m_nCompression;
            SourceState     m_nState;
            LONG            m_nCompressBytes;
            CapToStateMap   m_mapCapToState;
            bool            m_bCapCached;
            bool            m_bRetrievedAllCaps;
            bool            m_bFastCapRetrieval;
            LONG            m_nJpegQuality;
            bool            m_bJpegProgressive;
            bool            m_bAutoFeed;
            TW_UINT16       m_nJobControl;
            int             m_nFailAction;
            int             m_nMaxRetryAttempts;
            int             m_nCurRetryCount;
            CTL_ImageIOHandlerPtr m_pImageHandler;
            HANDLE          m_hAcquireStrip;
            bool            m_bUserStripUsed;
            SIZE_T          m_nUserStripSize;
            bool            m_bImagesStored;
            bool            m_bAutoIncrementFile;
            int             m_nCurFileNum;
            int             m_nFileNameBaseNum;
            int             m_nFileIncrement;
            size_t          m_nFileDigits;
            LONG            m_nAutoIncrementFlags;
            int             m_nStartFileNum;
            bool            m_bManualDuplexModeOn;
            LONG            m_nManualDuplexModeFlags;
            LONG            m_nMultiPageScanMode;
            LONG            m_nCurrentSideAcquired;
            std::array<bool, 2> m_bDuplexSideDone;
            TW_UINT32       m_EOJDetectedValue;
            bool            m_bIsFileSaveIncomplete;
            int             m_nJobNum;
            bool            m_bJobStarted;
            bool            m_bJobFileHandling;
            bool            m_bImageLayoutValid;
            bool            m_bIsBlankPageDetectionOn;
            LONG            m_lBlankPageAutoDetect;
            double          m_dBlankPageThreshold;
            LONG            m_nBlankPageCount;
            bool            m_bForceScanOnNoUI;
            bool            m_bImageNegative;
            bool            m_bProcessingPixelInfo;
            bool            m_bSkipImageInfoErrors;
            LONG            m_nForcedBpp;

            struct tagCapCachInfo {
                TW_UINT16 nCap;
                bool      m_bSupported;
            };
            typedef std::unordered_map<TW_UINT16, bool> CachedCapMap;

            public:
                typedef std::unordered_map<int, std::vector<int> > CachedPixelTypeMap;
                void        AddPixelTypeAndBitDepth(int PixelType, int BitDepth);
                CachedPixelTypeMap::iterator FindPixelType(int PixelType);
                bool IsBitDepthSupported(int PixelType, int BitDepth);
                bool IsPixelTypeSupported(int PixelType);
                bool PixelTypesRetrieved() const;
                const CachedPixelTypeMap& GetPixelTypeMap() { return m_aPixelTypeMap; }


            private:
            struct AltAcquireArea {
                FloatRect m_rect;
                FloatRect m_rectScaling;
                FloatRect m_rectResample;
                LONG flags;
                LONG UnitOfMeasure;
                AltAcquireArea() : flags(0), UnitOfMeasure(0) {}
            };

            AltAcquireArea m_AltAcquireArea;
            CachedCapMap  m_aCapCache;
            CachedPixelTypeMap m_aPixelTypeMap;
            CapList m_aUnsupportedCapCache;
            CapList m_aSupportedCapCache;
            TW_IMAGEINFO  m_ImageInfo;
            FloatRect     m_ImageLayout;
            TW_FILESYSTEM   m_FileSystem;
            DTWAINImageInfoEx m_ImageInfoEx;
            TW_IMAGEMEMXFER* m_pImageMemXfer;
            std::shared_ptr<CTL_ExtImageInfoTriplet> m_pExtImageTriplet;
            TWINFOVector m_ExtImageVector;
            DTWAIN_ARRAY    m_PersistentArray;
            std::unordered_set<LONG> m_aExtendedCaps;
            DuplexData m_DuplexFileData;
            bool    m_bImageInfoRetrieved;

            struct FileFormatInfo
            {
                LPCTSTR filter;
                unsigned int len;
                LPCTSTR extension;
                FileFormatInfo(LPCTSTR f=0, unsigned int l=0, LPCTSTR e=0) : filter(f), len(l), extension(e) {}
            };

            mutable std::unordered_map<int, FileFormatInfo> m_saveFileMap;
            void initFileSaveMap() const;
    };



    class CTL_TwainSource
    {
        public:
            CTL_TwainSource( CTL_ITwainSource *pSource=NULL);
            CTL_TwainSource( const CTL_ITwainSource *pSource);

            operator const CTL_ITwainSource*() { return m_pSource; }
            operator const CTL_ITwainSource*() const { return m_pSource; }
            operator void * (){ return (void *)m_pSource; }
            CTL_ITwainSource *GetSourcePtr() { return m_pSource; }
            // Interface to source
            TW_UINT32    GetId() const { return m_pSource->GetId(); }
            const TW_VERSION*  GetVersion() const { return m_pSource->GetVersion(); }
            TW_UINT16    GetProtocolMajor() const { return m_pSource->GetProtocolMajor(); }
            TW_UINT16    GetProtocolMinor() const { return m_pSource->GetProtocolMinor(); }
            TW_UINT32    GetSupportedGroups() const { return m_pSource->GetSupportedGroups(); }
            CTL_StringType GetManufacturer() const { return m_pSource->GetManufacturer(); }
            CTL_StringType GetProductFamily() const { return m_pSource->GetProductFamily(); }
            CTL_StringType GetProductName() const { return m_pSource->GetProductName(); }
            bool         IsOpened() const { return m_pSource->IsOpened(); }
            bool         IsValid() const { return m_pSource!=NULL; }
            bool         IsUIOpen() const { return m_pSource->IsUIOpen(); }
            void         SetDibHandle(HANDLE hDib, size_t nWhich=0) { m_pSource->SetDibHandle(hDib, nWhich); }
            HANDLE       GetDibHandle(int nWhich=0) const { return m_pSource->GetDibHandle(nWhich); }
            bool         RenderImage(int nWhich=0) { if (m_pSource)
                                            return m_pSource->RenderImage(nWhich);
                                          return false; }
            void         SetAcquireType(CTL_TwainAcquireEnum AcquireType, LPCTSTR lpszFile=NULL)
                                       { if (m_pSource)
                                      m_pSource->SetAcquireType(AcquireType,lpszFile);
                                      }
            void         SetUIOpenOnAcquire(bool bSet) { if ( m_pSource )
                                        m_pSource->SetUIOpenOnAcquire( bSet ); }
            bool         IsUIOpenOnAcquire() const { if ( m_pSource )
                                        return m_pSource->IsUIOpenOnAcquire();
                                        return false;}

            CTL_TwainAcquireEnum  GetAcquireType() const
                    { return m_pSource->GetAcquireType(); }

            CTL_StringType   GetAcquireFile() const
                    { return m_pSource->GetAcquireFile(); }

            void         SetAcquireFile(LPCTSTR szFile) { if (m_pSource)
                                               m_pSource->SetAcquireFile(szFile);
                                               }

            int         GetNumDibs() const { if (m_pSource)
                                                return m_pSource->GetNumDibs();
                                             return 0;
                                           }
            void        ResetAcquisitionAttempts(DTWAIN_ARRAY aNewAttempts) { if ( m_pSource )
                                                     m_pSource->ResetAcquisitionAttempts(aNewAttempts);
                                                }
            DTWAIN_ARRAY  GetAcquisitionArray() { if ( m_pSource )
                                                    return m_pSource->GetAcquisitionArray();
                                                return NULL;
                                              }
        private:
            CTL_ITwainSource* m_pSource;
            void SetEqual(CTL_TwainSource & SObject);
    };
    #include <unordered_set>
    typedef std::unordered_set<CTL_ITwainSource *> CTL_TwainSourceArray;
}
#endif



