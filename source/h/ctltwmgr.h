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
#ifndef CTLTWMGR_H_
#define CTLTWMGR_H_
#ifdef _MSC_VER
#pragma warning( disable : 4786)
#pragma warning( disable : 4996)
#endif
#include <stdio.h>
#include <memory>
#include "ctltwain.h"
#include "ctlobstr.h"
#include "ctlarray.h"
#include "ctltwses.h"
#include "ctlenum.h"
#include "ctlcntry.h"
#include "capstruc.h"
#include "errstruc.h"
#include "ctliface.h"
#include "ctlccerr.h"
#include "dtwdecl.h"
#include "ctltrp.h"
#include "ctltr011.h"
#include "ctltr012.h"
#include "ctltr014.h"
#include "ctltr015.h"
#include <boost/dll/shared_library.hpp>
namespace dynarithmic
{
    typedef std::unordered_map<TW_UINT16, CTL_CondCodeInfo> mapCondCodeInfo;

    class CTL_TwainDibArray;
    class CTL_CapabilityGetTriplet;
    class CTL_CondCodeInfo;
    class CTL_ImageXferTriplet;
    class CTL_ImageInfoTriplet;

    struct RawTwainTriplet
    {
        TW_UINT32    nDG;
        TW_UINT16    nDAT;
        TW_UINT16    nMSG;
    };

    class CTL_TwainAppMgr;
    typedef std::shared_ptr<CTL_TwainAppMgr> CTL_TwainAppMgrPtr;

    class CTL_TwainAppMgr
    {
        public:
            ~CTL_TwainAppMgr();
            // Application manager functions.
            // for 16-bit DLL's, this is global and can only be one of
            // them.
            static const CTL_TwainAppMgrPtr Create( HINSTANCE hInstance,
                                                  HINSTANCE hThisInstance,
                                                  LPCTSTR lpszDLLName = NULL)

            {
                if ( s_pGlobalAppMgr )
                    return s_pGlobalAppMgr;

                s_ThisInstance = hThisInstance;
                s_nLastError = 0;
                try { s_pGlobalAppMgr.reset(new CTL_TwainAppMgr( lpszDLLName, hInstance, hThisInstance ));}
                catch(...)
                { return CTL_TwainAppMgrPtr(); }
                if ( !s_pGlobalAppMgr->LoadSourceManager() )
                { s_pGlobalAppMgr.reset(); }
                return s_pGlobalAppMgr;
            }

            static const CTL_TwainAppMgrPtr GetInstance()
            {
                return s_pGlobalAppMgr;
            }
            static void Destroy();
            static HINSTANCE GetAppInstance();
            static bool CheckTwainExistence(const CTL_StringType& DLLName, LPLONG pWhichSearch=NULL);

            // Twain session management functions.  Each App utilizing
            // this DLL will get its own session.
            static CTL_TwainSession CreateTwainSession(LPCTSTR pAppName = NULL,
                                             HWND* hAppWnd = NULL,
                                             TW_UINT16 nMajorNum    = 1,
                                             TW_UINT16 nMinorNum    = 0,
                                             CTL_TwainLanguageEnum nLanguage  =
                                             TwainLanguage_USAENGLISH,
                                             CTL_TwainCountryEnum nCountry   =
                                             TwainCountry_USA,
                                             LPCTSTR lpszVersion  = _T("<?>"),
                                             LPCTSTR lpszMfg      = _T("<?>"),
                                             LPCTSTR lpszFamily   = _T("<?>"),
                                             LPCTSTR lpszProduct  = _T("<?>")
                                  );

            static void DestroyTwainSession( CTL_ITwainSession* pSession );
            static bool IsValidTwainSession( CTL_ITwainSession* pSession );
            static bool IsValidTwainSource( CTL_ITwainSession* pSession,
                                            CTL_ITwainSource *pSource);
            static void UnloadSourceManager();
            static HWND* GetWindowHandlePtr( CTL_ITwainSession *pSession );
            static bool OpenSourceManager( CTL_ITwainSession *pSession );
            static bool CloseSourceManager(CTL_ITwainSession *pSession);
            static bool IsTwainMsg(MSG *pMsg, bool bFromUserQueue=false);
            static unsigned int GetRegisteredMsg();
            static bool IsVersion2DSMUsed();
            static bool IsVersion2DSMUsedWithCallback();



            // Source management functions
            // Get all the sources in an array
            static void EnumSources( CTL_ITwainSession* pSession, CTL_TwainSourceArray & rArray );

            // Select a source from the TWAIN source dialog
            static const CTL_ITwainSource* SelectSourceDlg(  CTL_ITwainSession *pSession, LPCTSTR pProduct=NULL);

            // Select a source from a source object (NULL opens default
            // source)
            static const CTL_ITwainSource*  SelectSource( CTL_ITwainSession* pSession,
                                                    const CTL_ITwainSource* pSource=NULL);

            // Select a source via Product Name
            static const CTL_ITwainSource*  SelectSource( CTL_ITwainSession* pSession, LPCTSTR strSource);

            static const CTL_ITwainSource*  GetDefaultSource(CTL_ITwainSession *pSession);

            // Select a source from a source object (NULL opens default
            // source)
            static bool OpenSource( CTL_ITwainSession* pSession, const CTL_ITwainSource* pSource=NULL);

            static bool CloseSource(CTL_ITwainSession* pSession, const CTL_ITwainSource* pSource=NULL, bool bForce=true);
            // Get the current selected source
            static CTL_TwainSource GetCurrentSelectedSource();
            static CTL_TwainAcquireEnum GetCompatibleFileTransferType( const CTL_ITwainSource *pSource );

            static TW_UINT16 GetConditionCode( CTL_ITwainSession *pSession, const CTL_ITwainSource *pSource=NULL, TW_UINT16 rc=1);

            // Get the current session in use
            static CTL_TwainSession GetCurrentSession();

            // Get the nth registered session (only use with 0)
            static CTL_TwainSession GetNthSession(int nSession);

            // Gets the transfer count for a selected source
            static int GetTransferCount( const CTL_ITwainSource *pSource );
            static int SetTransferCount( const CTL_ITwainSource *pSource, int nCount );

            // Get capabilities for selected source
            static void GetCapabilities(const CTL_ITwainSource *pSource,CTL_TwainCapArray& pArray,TW_UINT16 MaxCustomBase = 0,
                                        bool bGetCustom = false);

            static bool IsCustomCapability(LONG nCap) { return nCap >= DTWAIN_CV_CAPCUSTOMBASE; }

            static void GetExtendedCapabilities(const CTL_ITwainSource *pSource, CTL_IntArray& rArray);

            static UINT GetCapOps(const CTL_ITwainSource *pSource, int nCap, bool bCanQuery); // Does extra checking here

            static UINT GetCapabilityOperations(const CTL_ITwainSource *pSource, // Uses the MSG_QUERYSUPPORT triplet
                                                int nCap);

            static void EnumTransferMechanisms( const CTL_ITwainSource *pSource, CTL_IntArray & pArray );

            static void EnumTwainFileFormats( const CTL_ITwainSource *pSource, CTL_IntArray & pArray );

            static bool IsSupportedFileFormat( const CTL_ITwainSource* pSource, int nFileFormat );

            static bool GetFileTransferDefaults( CTL_ITwainSource *pSource, CTL_StringType& strFile, int &nFileType);

            static int SetTransferMechanism( const CTL_ITwainSource *pSource, CTL_TwainAcquireEnum AcquireType,
                                            LONG ClipboardTransferType);

            static void SetPixelAndBitDepth(const CTL_ITwainSource *pSource);

            static bool IsSourceOpen( const CTL_ITwainSource *pSource );

            static void GetPixelTypes( const CTL_ITwainSource *pSource, CTL_IntArray & pArray );

            static CTL_TwainUnitEnum GetCurrentUnitMeasure(const CTL_ITwainSource *pSource);

            static void GetCompressionTypes( const CTL_ITwainSource *pSource, CTL_IntArray & pArray );

            static void GetUnitTypes( const CTL_ITwainSource *pSource, CTL_IntArray & pArray );

            static bool GetImageLayoutSize(const CTL_ITwainSource* pSource, CTL_RealArray& rArray, CTL_EnumGetType GetType);
            static bool SetImageLayoutSize(const CTL_ITwainSource* pSource, const CTL_RealArray& rArray, CTL_RealArray& rActual,
                                             CTL_EnumSetType SetType);

            static bool StoreImageLayout(CTL_ITwainSource* pSource);

            static bool IsFeederLoaded( const CTL_ITwainSource *pSource );

            static bool IsFeederEnabled( const CTL_ITwainSource *pSource, TW_UINT16& nValue );

            static bool SetupFeeder( const CTL_ITwainSource *pSource, int maxpages, bool bSet = true);

            // User Interface functions
            static bool ShowUserInterface(const CTL_ITwainSource *pSource, bool bTest = false, bool bShowUIOnly = false);

            static bool DisableUserInterface( const CTL_ITwainSource *pSource );

            static bool GetImageInfo(CTL_ITwainSource *pSource, CTL_ImageInfoTriplet *pTrip=NULL);

            static int  TransferImage(const CTL_ITwainSource *pSource, int nImageNum=0);

            static bool SetFeederEnableMode( CTL_ITwainSource *pSource, bool bMode=true);

            static bool ShowProgressIndicator(CTL_ITwainSource *pSource, bool bShow=true);
            static bool IsProgressIndicatorOn(CTL_ITwainSource *pSource);

            static bool IsJobControlSupported( const CTL_ITwainSource *pSource, TW_UINT16& nValue );

            static void     SetError(int nError, const CTL_StringType& extraInfo=_T(""));
            static int      GetLastError();
            static LPTSTR    GetLastErrorString(LPTSTR lpszBuffer, int nSize);
            static LPTSTR    GetErrorString(int nError, LPTSTR lpszBuffer, int nSize);
            static void     SetDLLInstance(HINSTANCE hDLLInstance);
            // Generic capability setting functions

            // Message sending functions
            static int  SendTwainMsgToWindow(CTL_ITwainSession *pSession,
                                             HWND hWndWhich,
                                             WPARAM wParam,
                                             LPARAM lParam = 0L);

            static bool ProcessConditionCodeError(TW_UINT16 nError);
            static CTL_CondCodeInfo FindConditionCode(TW_UINT16 nCode);
            static void AddConditionCodeError(TW_UINT16 nCode, int nResource);
            static void RemoveAllConditionCodeErrors();

            static bool IsCapabilitySupported(const CTL_ITwainSource *pSource,
                                              TW_UINT16 nCap,
                                              int nType=CTL_GetTypeGET);

            static bool GetOneTwainCapValue( const CTL_ITwainSource *pSource,
                                             void *pValue,
                                             TW_UINT16 Cap,
                                             CTL_EnumGetType GetType,
                                             TW_UINT16 nDataType );

            static bool GetOneCapValue(const CTL_ITwainSource *pSource,
                                       void *pValue,
                                       TW_UINT16 Cap,
                                       TW_UINT16 nDataType );

            static TW_UINT16 ProcessReturnCodeOneValue(CTL_ITwainSource *pSource,
                                                         TW_UINT16 rc);

            static TW_UINT16 GetMemXferValues(CTL_ITwainSource *pSource, TW_SETUPMEMXFER *pXfer);
            static int  GetCapMaskFromCap( CTL_EnumCapability Cap );
            static bool IsCapMaskOn( CTL_EnumCapability Cap, CTL_EnumGetType GetType);
            static bool IsCapMaskOn( CTL_EnumCapability Cap, CTL_EnumSetType SetType);
            static bool IsSourceCompliant( const CTL_ITwainSource *pSource,
                                           CTL_EnumTwainVersion TVersion,
                                           CTL_TwainCapArray & rArray);
            static CTL_String   GetCapNameFromCap( LONG Cap );
            static UINT         GetDataTypeFromCap( CTL_EnumCapability Cap, CTL_ITwainSource *pSource=NULL );
            static UINT         GetContainerTypesFromCap( CTL_EnumCapability Cap, bool nType );
            static bool         GetBestContainerType(const CTL_ITwainSource* pSource,
                                                     CTL_EnumCapability nCap,
                                                     UINT &rGet,
                                                     UINT &rSet,
                                                     UINT &nDataType,
                                                     UINT lGetType,
                                                     bool* flags);
            static bool         GetBestCapDataType(const CTL_ITwainSource* pSource,
                                                     CTL_EnumCapability nCap,
                                                     UINT &nDataType);

            static void         GetContainerNamesFromType( int nType, CTL_StringArray &rArray );
            static void         EndTwainUI(CTL_ITwainSession *pSession, CTL_ITwainSource *pSource);

            static int          CopyFile(const CTL_StringType& strIn, const CTL_StringType& strOut);
            static LONG         GetCapFromCapName( const char *szCapName );
            static bool         SetDefaultSource( CTL_ITwainSession *pSession,
                                                  const CTL_ITwainSource *pSource );
            static void         WriteLogInfo(const CTL_StringType& s, bool bFlush=false);
            static void         WriteLogInfoA(const CTL_String& s, bool bFlush = false);
            static void         WriteLogInfoW(const CTL_WString& s, bool bFlush = false);

            static TW_UINT16 CallDSMEntryProc( TW_IDENTITY *pOrigin, TW_IDENTITY* pDest,
                                               TW_UINT32 dg, TW_UINT16 dat, TW_UINT16 msg, TW_MEMREF pMemref);


            static LONG ExtImageInfoArrayType(LONG ExtType);
            static CTL_StringType GetTwainDirFullName(LPCTSTR szFileName,
                                                      LPLONG pWhichSearch,
                                                      bool leaveLoaded=false,
                                                      boost::dll::shared_library* pModule=nullptr);

            static CTL_CapStruct GetGeneralCapInfo(LONG Cap);
            static bool GetCurrentOneCapValue(const CTL_ITwainSource *pSource, void *pValue, TW_UINT16 Cap, TW_UINT16 nDataType );

        private:
            friend class CTL_TwainTriplet;
            CTL_TwainAppMgr( LPCTSTR lpszDLLName, HINSTANCE hInstance,
                             HINSTANCE hThisInstance );

            void        SetLastTwainError( TW_UINT16 nError,
                                      int nErrorType );

            TW_UINT16 CallDSMEntryProc( CTL_TwainTriplet & pTriplet );
            TW_UINT16 CallDSMEntryProcInternal(CTL_TwainTriplet::TwainTripletArgs& tripArgs);



            // Get the default DLL name
            static CTL_StringType GetDefaultDLLName();
            static CTL_StringType GetLatestDSMVersion();

            // Load the source manager
            bool LoadSourceManager(  LPCTSTR pszDLLName=NULL);

            // Load the data source manager
            bool LoadDSM();

            // single Application manager
//            typedef std::shared_ptr<CTL_TwainAppMgr> CTL_TwainAppMgrPtr;
            static CTL_TwainAppMgrPtr    s_pGlobalAppMgr;

            // Array of Twain Sessions
            CTL_TwainSessionArray m_arrTwainSession;

            static bool GetMultipleIntValues( const CTL_ITwainSource *pSource,
                                              CTL_IntArray & pArray,
                                              CTL_CapabilityGetTriplet *pTrip);

            static bool GetMultipleRealValues( const CTL_ITwainSource *pSource,
                                              CTL_RealArray & pArray,
                                              CTL_CapabilityGetTriplet *pTrip);
            static bool GetOneIntValue( const CTL_ITwainSource *pSource,
                                        TW_UINT16 *pInt,
                                        CTL_CapabilityGetTriplet *pTrip);

            static TW_INT32 AllocateBufferStrip(TW_IMAGEINFO *pImgInfo,
                                            TW_SETUPMEMXFER *pSetupInfo,
                                            HGLOBAL *pGlobal,
                                            DWORD *pSize,
                                            DWORD SizeToUse,
                                            LONG nCompression);

            static int  NativeTransfer( CTL_ITwainSession *pSession,
                                        CTL_ITwainSource  *pSource);

            static int  FileTransfer( CTL_ITwainSession *pSession,
                                      CTL_ITwainSource  *pSource );

            static int  BufferTransfer( CTL_ITwainSession *pSession,
                                        CTL_ITwainSource  *pSource );

            static int  ClipboardTransfer( CTL_ITwainSession *pSession,
                                           CTL_ITwainSource *pSource );

            static int  StartTransfer( CTL_ITwainSession *pSession,
                                       CTL_ITwainSource * pSource,
                                       CTL_ImageXferTriplet *pTrip);

            static bool SetupMemXferDIB( CTL_ITwainSession *pSession, CTL_ITwainSource *pSource,
                                         HGLOBAL hGlobal, TW_IMAGEINFO *pImgInfo, TW_INT32 nSize);



            template<typename ArrayType, typename DataType>
            static bool GetMultipleValues( const CTL_ITwainSource *pSource,ArrayType& pArray,CTL_CapabilityGetTriplet *pTrip)
            {
                pArray.clear();
                if ( !IsSourceOpen( pSource ) )
                    return false;

                // Get the #transfer count
                TW_UINT16 rc = pTrip->Execute();

                CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;

                if ( rc == TWRC_FAILURE ) // Check if there is a real failure
                {
                    CTL_ITwainSession* pSession =
                        pTempSource->GetTwainSession();
                    TW_UINT16 ccode = GetConditionCode(pSession, NULL);
                    ProcessConditionCodeError(ccode);
                    return false;
                }

                switch (rc)
                {
                    case TWRC_SUCCESS:
                    {
                        DataType pData;
                        size_t nNumItems = pTrip->GetNumItems();

                        if ( nNumItems == 0 )
                            pArray.push_back(0);
                        else
                        for ( size_t i = 0; i < nNumItems; i++ )
                        {
                            pTrip->GetValue( &pData, i );
                            pArray.push_back( pData );
                        }
                        return true;
                    }
                }
                return false;
            }

            template <typename ArrayType, typename fn>
            struct MultiCapCopier
            {
                ArrayType* m_Array;
                CTL_ITwainSource *m_pSource;
                CTL_CapabilityGetTriplet* m_pTriplet;
                MultiCapCopier(CTL_ITwainSource* pSource, ArrayType* arr, CTL_CapabilityGetTriplet* pTriplet) :
                        m_Array(arr), m_pSource(pSource), m_pTriplet(pTriplet) {}

                void CopyValues()
                {
                    ArrayType pTemp;
                    GetMultipleValues<ArrayType, fn>(m_pSource, pTemp, m_pTriplet);
                    std::copy(pTemp.begin(), pTemp.end(), std::back_inserter(*m_Array));
                }
            };

            template <typename T, typename fnType>
            struct GetMultiValuesImpl
            {
            static void GetMultipleTwainCapValues(const CTL_ITwainSource *pSource,
                                                      T& pArray,
                                                    TW_UINT16 Cap,
                                                    TW_UINT16 nDataType,
                                                      CTL_EnumContainer Container=TwainContainer_ENUMERATION)
               {
                   CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
                   CTL_ITwainSession* pSession = pTempSource->GetTwainSession();

                   std::unique_ptr<CTL_CapabilityGetTriplet> pGetTriplet;

                   switch(Container)
                   {
                       case TwainContainer_ARRAY:
                       {
                           pGetTriplet = std::make_unique<CTL_CapabilityGetArrayTriplet>(pSession,
                               pTempSource,
                               CTL_GetTypeGET,
                               Cap, nDataType);
                       }
                       break;

                       case TwainContainer_ENUMERATION:
                       {
                           pGetTriplet = std::make_unique<CTL_CapabilityGetEnumTriplet>( pSession,
                                               pTempSource,
                                               CTL_GetTypeGET,
                                               Cap,
                                               nDataType);
                       }
                       break;

                       case TwainContainer_RANGE:
                       {
                           pGetTriplet = std::make_unique<CTL_CapabilityGetRangeTriplet>( pSession,
                                               pTempSource,
                                               CTL_GetTypeGET,
                                               Cap,nDataType);
                       }
                       break;

                       default:
                           return;
                       }

                       MultiCapCopier<T, fnType> copier(pTempSource, &pArray, pGetTriplet.get());
                       copier.CopyValues();
                   }
            };

            static CTL_ErrorStruct GetGeneralErrorInfo(LONG nDG, UINT nDAT, UINT nMSG);

            void DestroySession( CTL_ITwainSession *pSession );
            void DestroyAllTwainSessions();
            void WriteToLogFile(int rc);
            void OpenLogFile(LPCSTR lpszFile);
            void CloseLogFile();
            static bool SetDependentCaps( const CTL_ITwainSource *pSource, CTL_EnumCapability Cap );
            static void EnumNoTimeoutTriplets();

            static TW_IDENTITY s_AppId;          // Twain Identity structure
            static CTL_ITwainSession* s_pSelectedSession; // Current selected
                                                                // session
            CTL_StringType  m_strTwainDLLName;   // Twain DLL name
            boost::dll::shared_library m_hLibModule;         // Twain DLL module handle
            DSMENTRYPROC    m_lpDSMEntry;        // Proc entry point for DSM_ENTRY
            TW_UINT16       m_nErrorTWRC;
            TW_UINT16       m_nErrorTWCC;
            unsigned int    m_nTwainMsg;
            HINSTANCE       m_Instance;
            static int               s_nLastError;
            static CTL_StringType        s_strLastError;
            static HINSTANCE         s_ThisInstance;
            static mapCondCodeInfo   s_mapCondCode;
            static std::vector<RawTwainTriplet> s_NoTimeoutTriplets;
            static VOID CALLBACK TwainTimeOutProc(HWND, UINT, ULONG, DWORD);

    };

    #define DTWAIN_ERROR_CONDITION(Err, RetVal) {               \
            CTL_TwainAppMgr::SetError(Err);               \
            return(RetVal); }

    #define DTWAIN_ERROR_CONDITION_EX(Err, ExtraInfo, RetVal) {               \
        CTL_TwainAppMgr::SetError(Err, ExtraInfo);               \
        return(RetVal); }

    /////////////////// DLL stuff /////////////////////
}
#endif


