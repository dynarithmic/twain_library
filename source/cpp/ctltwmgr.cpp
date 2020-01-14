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
#define MC_NO_CPP

#ifdef _MSC_VER
#pragma warning( disable : 4786)
#pragma warning( disable : 4996)
#pragma warning( disable : 4702)
#endif

#include <stdio.h>
#include "dtwain_resource_constants.h"
#include <algorithm>
#include <set>
#include <memory>

#define IDS_TWCCBASE            9500
#define IDS_TWRCBASE            9600
#define IDS_TWCC_EXCEPTION      9999

#define DTWAIN_SET_ERROR_CONDITION(Err,Retval) {  \
            static_cast<CTL_TwainDLLHandle *>(::GetDTWAINHandle_Internal())->m_lLastError = (Err); \
            DTWAIN_ERROR_CONDITION((Err),(Retval)) \
            }

#include "dtwain.h"
#include <cstring>
#include <sstream>
#include "ctltrall.h"
#include <ctlccerr.h>
#include "ctldib.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include <boost/format.hpp>
#include <boost/filesystem.hpp>
#include <boost/dll/shared_library.hpp>
using namespace std;
using namespace dynarithmic;

template <class T>
bool SetOneTwainCapValue( const CTL_ITwainSource *pSource,
                          T Value,
                          CTL_EnumSetType nSetType,
                          TW_UINT16 Cap,
                          TW_UINT16 TwainType = 0xFFFF)
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;

    // Set the #transfer count
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
    vector<T> arrObj;
    arrObj.push_back( Value );

    if ( TwainType == 0xFFFF )
        TwainType = static_cast<TW_UINT16>(DTWAIN_GetCapDataType((DTWAIN_SOURCE)pSource, Cap));

    // Get a set capability triplet compatible for one value
    CTL_CapabilitySetOneValTriplet<T> SetOne( pSession,
                                           pTempSource,
                                           nSetType,
                                           Cap,
                                           TwainType,
                                           arrObj);

    if ( !CTL_TwainAppMgr::IsSourceOpen( pTempSource ) )
        return false;

    TW_UINT16 rc = SetOne.Execute();
    return CTL_TwainAppMgr::ProcessReturnCodeOneValue(pTempSource, rc)?true:false;
}


#define TWRC_Error      1
#define TWCC_Error      2

void CTL_TwainAppMgr::SetDLLInstance(HINSTANCE hDLLInstance)
{
    s_ThisInstance = hDLLInstance;
}

void CTL_TwainAppMgr::Destroy()
{
    if ( s_pGlobalAppMgr )
    {
        s_pGlobalAppMgr->DestroyAllTwainSessions();
        s_pGlobalAppMgr->RemoveAllConditionCodeErrors();
        s_pGlobalAppMgr->CloseLogFile();
        /* Use for this APP only */
        s_pGlobalAppMgr->UnloadSourceManager();
    }
    s_pGlobalAppMgr.reset();
}

CTL_TwainSession CTL_TwainAppMgr::CreateTwainSession(
                                     LPCTSTR pAppName/* = NULL*/,
                                     HWND* hAppWnd,/* = NULL*/
                                     TW_UINT16 nMajorNum/*    = 1*/,
                                     TW_UINT16 nMinorNum/*    = 0*/,
                                     CTL_TwainLanguageEnum nLanguage/*  =
                                     TwainLanguage_USAENGLISH*/,
                                     CTL_TwainCountryEnum nCountry/*   =
                                     TwainCountry_USA*/,
                                     LPCTSTR lpszVersion /* = "<?>"*/,
                                     LPCTSTR lpszMfg  /*    = "<?>"*/,
                                     LPCTSTR lpszFamily /*  = "<?>"*/,
                                     LPCTSTR lpszProduct /* = "<?>"*/
                                     )
{
    // Try to load the source manager ( set in state 2 if not already
    //    in state 2 or 3)
    if ( !s_pGlobalAppMgr )
        DTWAIN_ERROR_CONDITION(IDS_ErrTwainMgrInvalid,(CTL_TwainSession)NULL);

    // Load if not already loaded
    if ( !s_pGlobalAppMgr->m_hLibModule )
    {
        if ( !s_pGlobalAppMgr->LoadSourceManager() )
            return (CTL_TwainSession)NULL;
    }

    CTL_ITwainSession* pSession =
           CTL_ITwainSession::Create( pAppName,
                                     hAppWnd,
                                     nMajorNum,
                                     nMinorNum,
                                     nLanguage,
                                     nCountry,
                                     lpszVersion,
                                     lpszMfg,
                                     lpszFamily,
                                     lpszProduct);
    if ( pSession )
    {
        // We need to now set the ProtocolMajor, ProtocolMinor, and Supported Groups
        // to the proper levels here.  We support 1.9 for TWAIN_32.DLL (LEGACY) and 2.x for
        // TWAINDSM.DLL.
        // DTWAIN assumes 2.x, but must change for legacy TWAIN_32.DLL source manager
        CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

        if ( pHandle->m_SessionStruct.DSMName == TWAINDLLVERSION_1 )
        {
            TW_IDENTITY *pIdentity = pSession->GetAppIDPtr();
            pIdentity->ProtocolMajor = 1;
            pIdentity->ProtocolMinor = 9;
            pIdentity->SupportedGroups = DG_IMAGE | DG_CONTROL | DG_AUDIO;
        }

        s_pGlobalAppMgr->m_arrTwainSession.push_back( pSession );
        if ( !OpenSourceManager( pSession ) )
        {
            DestroyTwainSession( pSession );
            return (CTL_TwainSession)NULL;
        }
    }
    s_pSelectedSession = pSession;
    return CTL_TwainSession(pSession);
}

bool CTL_TwainAppMgr::OpenSourceManager( CTL_ITwainSession *pSession )
{
    CTL_TwainOpenSMTriplet SM( pSession );
    if ( SM.Execute() != TWRC_SUCCESS )
        return false;
    CTL_TwainDLLHandle::s_nDSMState = DSM_STATE_OPENED;
    CTL_TwainDLLHandle::s_nDSMVersion = SM.GetDSMVersion();
    CTL_TwainDLLHandle::s_TwainMemoryFunc = &CTL_TwainDLLHandle::s_TwainLegacyFunc;

    if ( CTL_TwainDLLHandle::s_nDSMVersion == DTWAIN_TWAINDSM_VERSION2)
    {
        // For 2.0 data sources.  Set the handle to the memory functions
        CTL_EntryPointTripletGet EntryPoint( pSession );
        if ( EntryPoint.Execute() == TWRC_SUCCESS )
        {
            // assign the memory functions to whatever the DSM has returned for
            // the memory management functions
            CTL_TwainDLLHandle::s_Twain2Func.m_EntryPoint = EntryPoint.getEntryPoint();
            CTL_TwainDLLHandle::s_TwainMemoryFunc = &CTL_TwainDLLHandle::s_Twain2Func;
        }
        else
            return false;
    }
    return true;
}

bool CTL_TwainAppMgr::IsVersion2DSMUsed()
{
    return CTL_TwainDLLHandle::s_nDSMVersion == DTWAIN_TWAINDSM_VERSION2;
}

bool CTL_TwainAppMgr::IsVersion2DSMUsedWithCallback()
{
    return IsVersion2DSMUsed() && CTL_TwainDLLHandle::s_TwainCallbackSet;
}

int CTL_TwainAppMgr::CopyFile(const CTL_StringType& strIn, const CTL_StringType& strOut)
{
    boost::filesystem::copy_file(strIn, strOut, boost::filesystem::copy_option::overwrite_if_exists);
    return 1;
}

const CTL_ITwainSource* CTL_TwainAppMgr::SelectSourceDlg(  CTL_ITwainSession *pSession,
                                                           LPCTSTR pProduct/*=NULL*/)
{
    if ( !pSession )
        return NULL;
    CTL_SelectSourceDlgTriplet SelectSource( pSession, pProduct );
    TW_UINT16 rc;
    rc = SelectSource.Execute();

    switch (rc )
    {
        case TWRC_CANCEL:
            SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_ACQUIRECANCELLED_EX, -1L);
            return NULL;
        break;

        case TWRC_FAILURE:
            TW_UINT16 ccode = GetConditionCode(pSession, NULL);
            ProcessConditionCodeError(ccode);
            SendTwainMsgToWindow(pSession, NULL, DTWAIN_SelectSourceFailed, ccode);
            return NULL;
          break;
    }

    return pSession->GetSelectedSource();
}


unsigned int CTL_TwainAppMgr::GetRegisteredMsg()
{
    if ( s_pGlobalAppMgr )
        return s_pGlobalAppMgr->m_nTwainMsg;
    return 0;
}

void CTL_TwainAppMgr::EnumSources( CTL_ITwainSession *pSession,
                                     CTL_TwainSourceArray & rArray )
{
    rArray.clear();
    if ( !IsValidTwainSession( pSession ) )
        return;
    pSession->CopyAllSources( rArray );
}

/* static static static static static static static static static static */
void CTL_TwainAppMgr::DestroyTwainSession( CTL_ITwainSession *pSession )
{
    if ( s_pGlobalAppMgr )
        s_pGlobalAppMgr->DestroySession( pSession );
}


/* static static static static static static static static static static */
bool CTL_TwainAppMgr::IsValidTwainSession( CTL_ITwainSession* pSession )
{
    if ( ! s_pGlobalAppMgr )
        return false;
    if ( find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession) != s_pGlobalAppMgr->m_arrTwainSession.end() )
        return true;
    return false;
}


bool CTL_TwainAppMgr::IsValidTwainSource( CTL_ITwainSession* pSession,CTL_ITwainSource *pSource)
{
    if ( !IsValidTwainSession( pSession ))
        return false;

    if ( !pSession->IsValidSource( pSource ) )
        return false;
    return true;
}


// This unloads the TWAIN source manager.  This function detaches
// ALL apps from TWAIN.  This means that if you have a third-party app that uses
// TWAIN, that app will not function or will GPF when a scanning request is given.
/* static static static static static static static static static static */
void CTL_TwainAppMgr::UnloadSourceManager()
{
    if ( s_pGlobalAppMgr && s_pGlobalAppMgr->m_hLibModule.is_loaded() )
        s_pGlobalAppMgr->m_hLibModule.unload();
    CTL_TwainDLLHandle::s_nDSMState = DSM_STATE_NONE;
}

HWND* CTL_TwainAppMgr::GetWindowHandlePtr( CTL_ITwainSession *pSession )
{
    if ( !s_pGlobalAppMgr )
        return NULL;
    if ( find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession) != s_pGlobalAppMgr->m_arrTwainSession.end() )
        return pSession->GetWindowHandlePtr();
    return NULL;
}

const CTL_ITwainSource* CTL_TwainAppMgr::SelectSource( CTL_ITwainSession *pSession,
                                                 const CTL_ITwainSource* pSource/*=NULL*/)

{
    if ( !s_pGlobalAppMgr )
        return NULL;
    if ( find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession) != s_pGlobalAppMgr->m_arrTwainSession.end() )
    {
        if ( pSession->SelectSource( pSource ) )
            return pSession->GetSelectedSource();
    }
    return NULL;
}

const CTL_ITwainSource* CTL_TwainAppMgr::SelectSource(CTL_ITwainSession* pSession, LPCTSTR strSource)
{
    if ( !s_pGlobalAppMgr )
        return NULL;
    if ( find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession) != s_pGlobalAppMgr->m_arrTwainSession.end() )
    {
        if ( pSession->SelectSource( strSource ) )
            return pSession->GetSelectedSource();
    }
    return NULL;
}



bool CTL_TwainAppMgr::OpenSource( CTL_ITwainSession* pSession,
                                const CTL_ITwainSource* pSource/*=NULL*/)
{
    bool bSourceOpened;
    if ( !s_pGlobalAppMgr )
        return false;
    if ( find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession) != s_pGlobalAppMgr->m_arrTwainSession.end() )
    {
        bSourceOpened = pSession->OpenSource( pSource );
        if ( bSourceOpened )
            s_pSelectedSession = pSession;
        else
            s_pSelectedSession = NULL;
        return bSourceOpened;
    }

    return false;
}



bool CTL_TwainAppMgr::GetBestContainerType(const CTL_ITwainSource* pSource,
                                           CTL_EnumCapability nCap,
                                           UINT & rContainerGet,
                                           UINT & rContainerSet,
                                           UINT & nDataType,
                                           UINT lGetType,
                                           bool *flags)
{
    if ( !s_pGlobalAppMgr )
        return false;

    if ( !s_pGlobalAppMgr->IsSourceOpen( pSource ))
        return false;

    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();

    UINT setSave = rContainerSet;

    // Get the data from the capability (only for "normal" cap values)
    if ( nCap < CAP_CUSTOMBASE)
    {
        if ( !flags[5] )
        nDataType = GetDataTypeFromCap( nCap, pTempSource );
    }

    bool bGetTheCap =
         ((lGetType == CTL_GetTypeGET && !flags[0] ) ||
         (lGetType == CTL_GetTypeGETCURRENT && !flags[1]) ||
         (lGetType == CTL_GetTypeGETDEFAULT && !flags[2]));

    // Get the possible container types for the get cap
    if ( bGetTheCap )
    {
        rContainerGet = GetContainerTypesFromCap( nCap, 0 );

        // Get the possible container types for the set cap
        rContainerSet = GetContainerTypesFromCap( nCap, 1 );

        CTL_CapabilityGetTriplet CapTester(pSession,
                                           pTempSource,
                                           (CTL_EnumGetType)lGetType,
                                           (TW_UINT16)nCap,
                                           0
                                           );
        // Check if there is only one type of "Get"
        if ( !( rContainerGet == TwainContainer_ONEVALUE ||
             rContainerGet == TwainContainer_ENUMERATION ||
             rContainerGet == TwainContainer_ARRAY ||
                rContainerGet == TwainContainer_RANGE ))
        {
            // Need to test for getting container
            // First, set the dependent capabilities
            if ( SetDependentCaps( pSource, nCap ) )
            {
                CapTester.SetTestMode(true);
                TW_UINT16 rc = CapTester.Execute();
                if ( rc == TWRC_SUCCESS )
                {
                    rContainerGet = CapTester.GetSupportedContainer();
                    if ( !flags[5] )
                    nDataType = CapTester.GetItemType();
                }
                else
                {
                    CTL_StringStreamType strm;
                    strm << BOOST_FORMAT(_T("\nError: Source %1%: Capability %2% container defaults to TW_ONEVALUE\n")) %
                                        pSource->GetProductName() % StringConversion::Convert_Ansi_To_Native(GetCapNameFromCap(nCap));
                    nDataType = (UINT)DTWAIN_CAPDATATYPE_UNKNOWN;
                    WriteLogInfo(strm.str());
                    rContainerGet = TwainContainer_ONEVALUE;
                }
            }
        }
    }
    // Now determine Set container for custom caps
    if ( nCap >= CAP_CUSTOMBASE )
    {
        if ((lGetType == CTL_GetTypeGET && !flags[3]) ||
            (lGetType == CTL_GetTypeGETCURRENT && !flags[4]))
            rContainerSet = rContainerGet; // assume that setting and getting use the same
        else
            rContainerSet = setSave;
    }

    // container.  No way to know for sure.
    return true;
}


bool CTL_TwainAppMgr::GetBestCapDataType(const CTL_ITwainSource* pSource, TW_UINT16 nCap, UINT &nDataType)
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();

    CTL_CapabilityGetTriplet CapTester(pSession, pTempSource,
                                       CTL_GetTypeGET,
                                       (CTL_EnumCapability)nCap, 0);

    if ( SetDependentCaps( pSource, nCap ) )
    {
        CapTester.SetTestMode(true);
        TW_UINT16 rc = CapTester.Execute();
        if ( rc == TWRC_SUCCESS )
        {
            nDataType = CapTester.GetItemType();
            return true;
        }
        else
        {
            CTL_StringStreamType strm;
            strm << BOOST_FORMAT(_T("\nError: Source %1%: Data type for capability %2% was not retrieved\n")) %
                            pSource->GetProductName() % StringConversion::Convert_Ansi_To_Native(GetCapNameFromCap(nCap));
            nDataType = (UINT)DTWAIN_CAPDATATYPE_UNKNOWN;
            WriteLogInfo(strm.str());
        }
    }
    return false;
}


bool CTL_TwainAppMgr::GetImageLayoutSize(const CTL_ITwainSource* pSource,
                                         CTL_RealArray& rArray,
                                         CTL_EnumGetType GetType)
{
    if ( !s_pGlobalAppMgr )
        return false;

    if ( !s_pGlobalAppMgr->IsSourceOpen( pSource ))
        return false;

    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();

    rArray.clear();

    CTL_ImageLayoutTriplet LayoutTrip( pSession, pTempSource, (TW_UINT16)GetType );

    TW_UINT16 rc;
    rc = LayoutTrip.Execute();
    if ( rc == TWRC_SUCCESS )
    {
        rArray.push_back( LayoutTrip.GetLeft() );
        rArray.push_back( LayoutTrip.GetTop() );
        rArray.push_back( LayoutTrip.GetRight() );
        rArray.push_back( LayoutTrip.GetBottom() );
        return true;
    }
    else
    {
        TW_UINT16 ccode = GetConditionCode(pSession, pTempSource);
        if ( ccode != TWCC_SUCCESS )
        {
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
        }
    }
    return false;
}


bool CTL_TwainAppMgr::SetImageLayoutSize(const CTL_ITwainSource* pSource,
                                         const CTL_RealArray& rArray,
                                         CTL_RealArray& rActual,
                                         CTL_EnumSetType SetType)
{
    if ( !s_pGlobalAppMgr )
        return false;

    if ( !s_pGlobalAppMgr->IsSourceOpen( pSource ))
        return false;

    rActual.clear();

    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();

    CTL_ImageSetLayoutTriplet LayoutTrip( pSession,
                                          pTempSource,
                                          rArray,
                                          (TW_UINT16)SetType );

    TW_UINT16 rc;
    rc = LayoutTrip.Execute();
    switch ( rc )
    {
        case TWRC_SUCCESS:
            GetImageLayoutSize( pSource, rActual, CTL_GetTypeGET );
            return true;

        case TWRC_CHECKSTATUS:
        {
            // Get the results and set them in the array
            GetImageLayoutSize( pSource, rActual, CTL_GetTypeGET );
            return true;
        }

        default:
        {
            TW_UINT16 ccode = GetConditionCode(pSession, pTempSource);
            if ( ccode != TWCC_SUCCESS )
                CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
        }
        return false;
    }
}

bool CTL_TwainAppMgr::CloseSource(CTL_ITwainSession* pSession,
                                  const CTL_ITwainSource* pSource/*=NULL*/,
                                  bool bForce)
{
    if ( !s_pGlobalAppMgr )
        return false;
    if ( find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession) != s_pGlobalAppMgr->m_arrTwainSession.end() )
    {
        return pSession->CloseSource( pSource, bForce )?true:false;
    }
    return true;
}

bool CTL_TwainAppMgr::ShowUserInterface( const CTL_ITwainSource *pSource, bool bTest, bool bShowUIOnly )
{
    struct origSourceState
    {
        CTL_ITwainSource *pSource;
        CTL_ITwainSession *pSession;
        SourceState oldState;
        bool isModal;
        bool isUIOnly;
        origSourceState(CTL_ITwainSource *p, CTL_ITwainSession *pS) : pSource(p), pSession(pS)
        {
            oldState = pSource->GetState();
            isModal = pSource->IsModal();
            isUIOnly = pSource->IsUIOnly();
            SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_UIOPENED, (LPARAM)pSource);
        }

        void restoreState()
        {
            pSource->SetState(oldState);
            pSource->SetModal(isModal);
            pSource->SetUIOnly(isUIOnly);
            SendTwainMsgToWindow(pSession, NULL, DTWAIN_TN_UIOPENFAILURE, (LPARAM)pSource);
        }
    };

    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
    bool bOld = false;

    if ( pTempSource->IsUIOpen() )
        return true;
    // Check if testing the UI
    if ( bTest )
    {
        return false;  // Just return that UI can not be tested.  Assume no UI cannot be done.
    }

    CTL_UserInterfaceTriplet *pUITrip;
    CTL_UserInterfaceUIOnlyTriplet UIOnly(pSession, pTempSource, pTempSource->GetTWUserInterface());
    CTL_UserInterfaceEnableTriplet UIEnabled(pSession, pTempSource,
                                                      pTempSource->GetTWUserInterface(),
                                                      (TW_BOOL)pTempSource->IsUIOpenOnAcquire());

    if ( bShowUIOnly )
        pUITrip = &UIOnly;
    else
        pUITrip = &UIEnabled;

    origSourceState oState(pTempSource, pSession);

    TW_UINT16 rc = pUITrip->Execute();
    if (rc != TWRC_SUCCESS )
    {
        TW_UINT16 ccode = GetConditionCode(pSession, pTempSource);
        if ( ccode != TWCC_SUCCESS )
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);

        oState.restoreState();

        switch ( rc )
        {
            // Check if User interface suppression was attempted
            case TWRC_CHECKSTATUS:
            case TWRC_FAILURE:
                if ( !pSource->IsUIOpenOnAcquire() || bTest )
                    DisableUserInterface( pSource );
            break;
        }

        if ( bTest && !bShowUIOnly )
        {
            pTempSource->SetUIOpenOnAcquire( bOld );
        }

        return false;
    }

    if ( !bTest && !bShowUIOnly )
    {
        if ( !pTempSource->IsUIOpenOnAcquire() )
        {
            // We wake up app loop here
            if ( pTempSource->IsForceScanOnNoUI())
            {
                #ifdef _WIN32
                ::PostMessage(*pSession->GetWindowHandlePtr(), WM_NULL, (WPARAM)0, (LPARAM)0);
                #endif
            }
        }
    }
    if ( bTest && !bShowUIOnly )
    {
        DisableUserInterface( pSource );
        pTempSource->SetUIOpenOnAcquire( bOld );
    }
    return true;
}

bool CTL_TwainAppMgr::DisableUserInterface(const CTL_ITwainSource *pSource)
{
    // Check if source is valid
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
    TW_USERINTERFACE *pTWUI = pTempSource->GetTWUserInterface();
    CTL_UserInterfaceDisableTriplet UI(pSession, pTempSource,
                                       pTWUI );
    bool bRet = true;

    if ( UI.Execute() != TWRC_SUCCESS )
    {
        TW_UINT16 ccode = GetConditionCode(pSession, pTempSource);
        if ( ccode != TWCC_SUCCESS )
        {
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
            bRet = false;
        }
        bRet = true;
    }
    if ( pTempSource->IsMultiPageModeUIMode() )
        pTempSource->ProcessMultipageFile();
    pTempSource->SetState(SOURCE_STATE_OPENED);

    return bRet;
}


void CTL_TwainAppMgr::EndTwainUI(CTL_ITwainSession *pSession, CTL_ITwainSource *pSource)
{
    // The source UI must be closed for modeless Source
    if ( pSource->IsUIOpen()/* && !pSource->IsModal() */)
    {
        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                              NULL,
                                              DTWAIN_TN_UICLOSING,
                                              (LPARAM)pSource);
        if ( 1 ) //pSource->IsUIOpenOnAcquire() )
            CTL_TwainAppMgr::DisableUserInterface(pSource);

        CTL_TwainAppMgr::SendTwainMsgToWindow(pSession,
                                              NULL,
                                              DTWAIN_TN_UICLOSED,
                                              (LPARAM)pSource);
        if ( pSource->IsMultiPageModeUIMode() )
            pSource->ProcessMultipageFile();
    }
}

bool CTL_TwainAppMgr::GetImageInfo(CTL_ITwainSource *pSource, CTL_ImageInfoTriplet *pTrip/*=NULL*/)
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
    CTL_ImageInfoTriplet II(pSession, pTempSource);
    if ( !pTrip )
        pTrip = &II;
    if ( pTrip->Execute() != TWRC_SUCCESS )
    {
        TW_UINT16 ccode = GetConditionCode(pSession, pTempSource);
        if ( ccode != TWCC_SUCCESS )
        {
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
            return false;
        }
        return true;
    }
    pSource->SetImageInfo(pTrip->GetImageInfoBuffer());
    return true;
}

CTL_TwainSession CTL_TwainAppMgr::GetCurrentSession()
{
    return (CTL_TwainSession)s_pSelectedSession;
}

CTL_TwainSession CTL_TwainAppMgr::GetNthSession(int nSession)
{
    if ( !s_pGlobalAppMgr )
        return (CTL_TwainSession)NULL;
    size_t nSize = s_pGlobalAppMgr->m_arrTwainSession.size();
    if ( (int)nSize > nSession )
        return CTL_TwainSession( s_pGlobalAppMgr->m_arrTwainSession[nSession] );
    return (CTL_TwainSession)NULL;
}


TW_UINT16 CTL_TwainAppMgr::GetConditionCode( CTL_ITwainSession *pSession,
                                             const CTL_ITwainSource *pSource,
                                             TW_UINT16 rc/*=1*/)
{
    if ( rc == DTWAIN_ERR_EXCEPTION_ERROR_ )
        return TWRC_FAILURE;
    CTL_ConditionCodeTriplet CC(pSession, pSource);
    if ( CC.Execute() == TWRC_SUCCESS )
        return CC.GetConditionCode();
    return TWRC_FAILURE;
}


bool CTL_TwainAppMgr::ProcessConditionCodeError(TW_UINT16 nError)
{
    // Find the error condition that matches nError
    CTL_CondCodeInfo p;
    p = FindConditionCode(nError);
    if ( p.IsValidCode())
    {
        DTWAIN_SET_ERROR_CONDITION(p.m_nResourceID, false)
    }
    return true;
}


bool CTL_TwainAppMgr::IsTwainMsg(MSG *pMsg, bool bFromUserQueue/*=false*/)
{
    if ( !s_pGlobalAppMgr )
        return false;

    if ( !IsValidTwainSession( s_pSelectedSession ) )
        return false;

    CTL_TwainSource TS;
    TS = s_pSelectedSession->GetSelectedSource();
    s_pSelectedSession->SetTwainMessageFlag(true);
    if ( !IsSourceOpen( TS.GetSourcePtr() ) )
        return false;

    // Create a triplet to determine if message is TWAIN message
    CTL_ProcessEventTriplet PE( s_pSelectedSession, TS.GetSourcePtr(), pMsg, bFromUserQueue && IsVersion2DSMUsed());

    // execute triplet
    TW_UINT16 rc = PE.ExecuteEventHandler();
    if ( rc != TWRC_NOTDSEVENT )
        s_pGlobalAppMgr->WriteToLogFile( rc );
    switch (rc)
    {
        case TWRC_NOTDSEVENT:
            return false;

        case TWRC_DSEVENT:
            return true;

        case TWRC_FAILURE:
        {
            TW_UINT16 CC;
            CC = GetConditionCode( s_pSelectedSession, TS.GetSourcePtr() );
            ProcessConditionCodeError(CC);
            return false;
        }
        case TWRC_XFERDONE:
        {
            return true;
        }
    }
    return false;
}


bool CTL_TwainAppMgr::SetFeederEnableMode( CTL_ITwainSource *pSource, bool bMode)
{
    if ( !s_pGlobalAppMgr )
        return false;

    if ( !s_pGlobalAppMgr->IsSourceOpen( pSource ))
        return false;

    pSource->SetFeederEnableMode(bMode);
    return true;
}


int CTL_TwainAppMgr::TransferImage(const CTL_ITwainSource *pSource,
                                    int nImageNum)
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    if ( !s_pGlobalAppMgr )
        return false;

    if ( !IsValidTwainSession( s_pSelectedSession ) )
        return false;
    // Test native transfer
    // if (transfer == NATIVE)  {
    pTempSource->SetPendingImageNum( nImageNum );
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();

    CTL_TwainAcquireEnum  AcquireType;
    AcquireType = pTempSource->GetAcquireType();
    pTempSource->SetNumCompressBytes(0);

    // Get the layout information for any transfer
    if ( !StoreImageLayout(pTempSource) )
        pTempSource->SetImageLayoutValid(false);
    else
        pTempSource->SetImageLayoutValid(true);

    switch ( AcquireType )
    {
        case TWAINAcquireType_Native:
            return NativeTransfer( pSession, pTempSource);

        case TWAINAcquireType_FileUsingNative:
        {
            long lFileFlags = pSource->GetAcquireFileFlags();
            if ( lFileFlags & DTWAIN_USENATIVE )
                return NativeTransfer(pSession, pTempSource);
            else
                return BufferTransfer(pSession, pTempSource);
        }
        break;

        case TWAINAcquireType_File:
            return FileTransfer( pSession, pTempSource );

        case TWAINAcquireType_Buffer:
            return BufferTransfer( pSession, pTempSource );

        case TWAINAcquireType_Clipboard:
            return ClipboardTransfer( pSession, pTempSource );
        default:
            break;
    }

    return false;
}

/* Remember the image layout for transfer */
bool CTL_TwainAppMgr::StoreImageLayout(CTL_ITwainSource *pSource)
{
    FloatRect fRect;
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();


    // First, see if ICAP_UNDEFINED image size is used
    TW_UINT16 nValue;
    if ( GetOneCapValue( pSource, &nValue, TwainCap_UNDEFINEDIMAGESIZE, TWTY_BOOL) )
    {
        if ( nValue == 1 )
        {
            // The layout can only be determined after the image has been scanned.
            fRect.left = -1.0;
            fRect.top = -1.0;
            fRect.right = -1.0;
            fRect.bottom = -1.0;
            pTempSource->SetImageLayout(&fRect);
            return true;
        }
    }

    CTL_ImageLayoutTriplet LayoutTrip( pSession, pTempSource, MSG_GET );

    TW_UINT16 rc;
    rc = LayoutTrip.Execute();
    if ( rc == TWRC_SUCCESS )
    {
        TW_IMAGELAYOUT IL;
        memcpy(&IL, LayoutTrip.GetImageLayout(), sizeof(TW_IMAGELAYOUT));
        fRect.left = CTL_CapabilityTriplet::Twain32ToFloat( IL.Frame.Left );
        fRect.top = CTL_CapabilityTriplet::Twain32ToFloat( IL.Frame.Top );
        fRect.right = CTL_CapabilityTriplet::Twain32ToFloat( IL.Frame.Right );
        fRect.bottom = CTL_CapabilityTriplet::Twain32ToFloat( IL.Frame.Bottom );
        pTempSource->SetImageLayout(&fRect);
        return true;
    }
    else
    {
        TW_UINT16 ccode = GetConditionCode(pSession, pTempSource);
        if ( ccode != TWCC_SUCCESS )
        {
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
        }
    }
    return false;
}


/* All sources provide this transfer capability */
int CTL_TwainAppMgr::NativeTransfer( CTL_ITwainSession *pSession,
                                      CTL_ITwainSource  *pSource)
{
    CTL_ImageXferTriplet IXfer(pSession,
                               pSource,
                               DAT_IMAGENATIVEXFER);

    return StartTransfer( pSession, pSource, &IXfer );
}

int CTL_TwainAppMgr::ClipboardTransfer( CTL_ITwainSession *pSession,
                                         CTL_ITwainSource *pSource )
{
    if ( pSource->GetSpecialTransferMode() == DTWAIN_USENATIVE )
        return NativeTransfer( pSession, pSource );
    return BufferTransfer( pSession, pSource );
}


int  CTL_TwainAppMgr::FileTransfer( CTL_ITwainSession *pSession,
                                    CTL_ITwainSource  *pSource )
{
    // Set the file type
    CTL_StringType sFileName;
    long lFlags = pSource->GetAcquireFileFlags();
    if ( lFlags & DTWAIN_USEPROMPT)
    {
        CTL_StringType szTempPath;
        // Set the temp file name here
        szTempPath = GetDTWAINTempFilePath();
        if ( szTempPath.empty() )
            return false;

        CTL_StringType sGUID = StringWrapper::GetGUID();
        szTempPath += sGUID + _T(".IDT");
        StringWrapper::TrimAll(szTempPath);
        pSource->SetAcquireFile(szTempPath.c_str());
    }
    else
    {
        sFileName = pSource->GetCurrentImageFileName();

        // See if name should be changed by sending notification to user
        pSource->SetActualFileName(sFileName);
        CTL_TwainAppMgr::SendTwainMsgToWindow(pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGING, (LPARAM)pSource);
        // check if name has changed
        CTL_StringType sFileNameNew = pSource->GetActualFileName();
        if ( sFileName != sFileNameNew )
            CTL_TwainAppMgr::SendTwainMsgToWindow(pSource->GetTwainSession(), NULL, DTWAIN_TN_FILENAMECHANGED, (LPARAM)pSource);
        std::swap(sFileNameNew, sFileName);  // swap the names, even if they may not have changed.
    }

    CTL_SetupFileXferTriplet  FileXferSetup(pSession,
                                       pSource,
                                       (int)CTL_SetTypeSET,
                                       pSource->GetAcquireFileType(),
                                       sFileName
                                       );
    // Set the file type and name
    if ( FileXferSetup.Execute() == TWRC_FAILURE )
    {
        TW_UINT16 ccode = GetConditionCode(pSession, pSource);
        if ( ccode != TWCC_SUCCESS )
        {
            ProcessConditionCodeError(ccode);
            return false;
        }
        return true;
    }

    // Start the transferring of the image
    CTL_ImageXferTriplet IXfer(pSession,
                               pSource,
                               DAT_IMAGEFILEXFER);

    return StartTransfer( pSession, pSource, &IXfer );
}


int  CTL_TwainAppMgr::BufferTransfer( CTL_ITwainSession *pSession,
                                      CTL_ITwainSource  *pSource )
{
    // Get the source
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;

    // Get the image information
    CTL_ImageInfoTriplet ImageInfo(pSession, pTempSource);
    if ( ImageInfo.Execute() != TWRC_SUCCESS )
        return false;
    TW_IMAGEINFO *pInfo = ImageInfo.GetImageInfoBuffer();

    // Get the buffer strip size
    TW_SETUPMEMXFER pXfer;
    if ( !GetMemXferValues(pTempSource, &pXfer))
        return false;

    // Get the total size of an image of 256 colors

    // First check if the user has defined their own buffer
    HGLOBAL hGlobAcquire;
    DWORD nTotalSize = 0;
    TW_INT32 nSizeStrip;

    // Check if user has defined a strip size
    nSizeStrip = (TW_INT32)pSource->GetUserStripBufSize();

    // User has not defined a buffer.  Let DTWAIN handle the memory here
    if ( !pSource->GetUserStripBuffer())
    {
        // Did user specify a size?  If 0, DTWAIN determines the size
        nSizeStrip = AllocateBufferStrip( pInfo, &pXfer, &hGlobAcquire, &nTotalSize, nSizeStrip, pSource->GetCompressionType());
        if ( nSizeStrip == 0 )
            return false;
    }
    else
    {
        // Did user
        // User has defined a buffer
        hGlobAcquire = pSource->GetUserStripBuffer();
        nSizeStrip = (TW_INT32)pSource->GetUserStripBufSize();
    }
        // Setup the DIB's information for an uncompressed image
    if ( pSource->GetCompressionType() == TWCP_NONE )
        SetupMemXferDIB( pSession, pSource, hGlobAcquire, pInfo, (TW_INT32)nTotalSize);

    // hGlobAcquire is a handle to a DIB that will be used as the bitmap to display
    // Set up the transfer triplet
    // Get the default transfer strip
    CTL_ImageMemXferTriplet IXfer(pSession, pSource, hGlobAcquire, TWMF_APPOWNS | TWMF_POINTER,
                                  pInfo->PixelType, nSizeStrip, (TW_UINT16)pSource->GetCompressionType());
    return  StartTransfer( pSession, pSource, &IXfer );
}

TW_UINT16 CTL_TwainAppMgr::GetMemXferValues(CTL_ITwainSource *pSource, TW_SETUPMEMXFER *pXfer)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    CTL_SetupMemXferTriplet MemXfer(pSession, pSource);
    if ( MemXfer.Execute() != TWRC_SUCCESS )
        return false;

    memcpy(pXfer, MemXfer.GetSetupMemXferBuffer(), sizeof(TW_SETUPMEMXFER));
    return true;
}

TW_INT32 CTL_TwainAppMgr::AllocateBufferStrip(TW_IMAGEINFO *pImgInfo,
                                          TW_SETUPMEMXFER *pSetupInfo,
                                          HGLOBAL *pGlobal,
                                          DWORD* dSize,
                                          DWORD SizeToUse,
                                          LONG nCompression)
{
    if ( pSetupInfo->MinBufSize == 0 ||
         pSetupInfo->MaxBufSize == 0 ||
         pSetupInfo->Preferred == 0)
        return false;

    DWORD nExtra = sizeof(BITMAPINFOHEADER) + 256*sizeof(RGBQUAD);
    TW_INT32 nImageSize = (((((TW_INT32)pImgInfo->ImageWidth *
                        pImgInfo->BitsPerPixel + 31) / 32) *4)
                        * pImgInfo->ImageLength);
    // No image if compression is used
    if ( nCompression != TWCP_NONE)
    {
        nImageSize = 0;
        nExtra = 0;
    }

    // SizeToUse has the amount to allocate if app specifies the buffer size
    int nBlocks;
    DWORD nAllocSize;
    if ( SizeToUse > 0 )
    {
        nBlocks = (int)(nImageSize / SizeToUse);
        nAllocSize = (nBlocks + 2) * SizeToUse;
        *pGlobal = ImageMemoryHandler::GlobalAlloc( GHND,   nAllocSize + nExtra);
        *dSize = nAllocSize;
         if ( *pGlobal )
            return (TW_INT32)SizeToUse;
        return 0;
    }

    // DTWAIN determines how much to allocate
    // Attempt to allocate preferred size
    nBlocks = (int)(nImageSize / pSetupInfo->Preferred);
    nAllocSize = (nBlocks + 2) * pSetupInfo->Preferred;
    *dSize = nAllocSize;


    // Try preferred size
    *pGlobal = ImageMemoryHandler::GlobalAlloc( GHND, nAllocSize + nExtra);
     if ( *pGlobal )
        return (TW_INT32)pSetupInfo->Preferred;

    // Try the minimum size
    nBlocks = (int)(nImageSize / pSetupInfo->MinBufSize);
    nAllocSize = (nBlocks + 2) * pSetupInfo->MinBufSize;
    *dSize = nAllocSize;
    *pGlobal = ImageMemoryHandler::GlobalAlloc( GHND,   nAllocSize + nExtra );

    if ( *pGlobal )
        return (TW_INT32)pSetupInfo->MinBufSize;
    return 0;
}

bool CTL_TwainAppMgr::SetupMemXferDIB( CTL_ITwainSession *pSession, CTL_ITwainSource *pSource,
                                       HGLOBAL hGlobal, TW_IMAGEINFO *pImgInfo, TW_INT32 nSize)
{
        //Lock the Memory
    LPBITMAPINFO        pDibInfo;
    DTWAINGlobalHandle_RAII dibHandle(hGlobal);
    pDibInfo = (LPBITMAPINFO)ImageMemoryHandler::GlobalLock(hGlobal);

    // fill in the image information
    pDibInfo->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
    pDibInfo->bmiHeader.biWidth  = pImgInfo->ImageWidth;
    pDibInfo->bmiHeader.biHeight = pImgInfo->ImageLength;
    pDibInfo->bmiHeader.biPlanes = 1;
    pDibInfo->bmiHeader.biBitCount = pImgInfo->BitsPerPixel;
    pDibInfo->bmiHeader.biCompression = 0;
    pDibInfo->bmiHeader.biSizeImage   = nSize;

    // Get Units and calculate PelsPerMeter
    TW_INT16 nValue = (TW_INT16)GetCurrentUnitMeasure(pSource);
    if ( nValue == -1)
        return false;

    float XRes, YRes;

    XRes = CTL_CapabilityTriplet::Twain32ToFloat(pImgInfo->XResolution);
    YRes = CTL_CapabilityTriplet::Twain32ToFloat(pImgInfo->YResolution);

    switch( nValue )
    {
        case TWUN_INCHES:
            pDibInfo->bmiHeader.biXPelsPerMeter = (LONG)((XRes*39.37008));
            pDibInfo->bmiHeader.biYPelsPerMeter = (LONG)((YRes*39.37008));
            break;

        case TWUN_CENTIMETERS:
            pDibInfo->bmiHeader.biXPelsPerMeter = (LONG)(XRes*100);
            pDibInfo->bmiHeader.biYPelsPerMeter = (LONG)(YRes*100);
            break;

        case TWUN_TWIPS:
            pDibInfo->bmiHeader.biXPelsPerMeter = (LONG)((XRes*56692.9152));
            pDibInfo->bmiHeader.biYPelsPerMeter = (LONG)((YRes*56692.9152));
            break;

        case TWUN_PICAS:
        case TWUN_POINTS:
        case TWUN_PIXELS:
        default:
            pDibInfo->bmiHeader.biXPelsPerMeter = 0;
            pDibInfo->bmiHeader.biYPelsPerMeter = 0;
            break;
    }

    switch (pImgInfo->PixelType)
    {
        TW_INT16 nPixelFlavor;
        case TWPT_BW:
        {
            pDibInfo->bmiHeader.biClrUsed       = 2;
            pDibInfo->bmiHeader.biClrImportant  = 0;

            // Get Units and calculate PelsPerMeter
            nPixelFlavor = TWPF_CHOCOLATE;
            if ( !GetCurrentOneCapValue( pSource, &nPixelFlavor, TwainCap_PIXELFLAVOR, TWTY_UINT16))
                nPixelFlavor = TWPF_CHOCOLATE;
            switch ( nPixelFlavor )
            {
                case TWPF_CHOCOLATE:
                {
                    pDibInfo->bmiColors[0].rgbRed = 0x0000;
                    pDibInfo->bmiColors[0].rgbGreen = 0x0000;
                    pDibInfo->bmiColors[0].rgbBlue = 0x0000;
                    pDibInfo->bmiColors[0].rgbReserved = 0;

                    pDibInfo->bmiColors[1].rgbRed = 0x00FF;
                    pDibInfo->bmiColors[1].rgbGreen = 0x00FF;
                    pDibInfo->bmiColors[1].rgbBlue = 0x00FF;
                    pDibInfo->bmiColors[1].rgbReserved = 0;
                }
                break;

                case TWPF_VANILLA:
                {
                    pDibInfo->bmiColors[0].rgbRed = 0x00FF;
                    pDibInfo->bmiColors[0].rgbGreen = 0x00FF;
                    pDibInfo->bmiColors[0].rgbBlue = 0x00FF;
                    pDibInfo->bmiColors[0].rgbReserved = 0;

                    pDibInfo->bmiColors[1].rgbRed = 0x0000;
                    pDibInfo->bmiColors[1].rgbGreen = 0x0000;
                    pDibInfo->bmiColors[1].rgbBlue = 0x0000;
                    pDibInfo->bmiColors[1].rgbReserved = 0;
                }
                break;
            }
        }
        break;

        case TWPT_GRAY:
        {
            if ( pDibInfo->bmiHeader.biBitCount == 4)
            {
                pDibInfo->bmiHeader.biClrUsed = 16;
                BYTE val;
                for (DWORD i=0; i<pDibInfo->bmiHeader.biClrUsed; i++)
                {
                    val = static_cast<BYTE>((i << 4) | i);
                    pDibInfo->bmiColors[i].rgbRed = val;//(BYTE) i;
                    pDibInfo->bmiColors[i].rgbGreen = val; //(BYTE) i;
                    pDibInfo->bmiColors[i].rgbBlue = val; //(BYTE) i;
                    pDibInfo->bmiColors[i].rgbReserved = 0;
                }
            }
            else
            {
                pDibInfo->bmiHeader.biClrUsed = 256;
                for (DWORD i=0; i<pDibInfo->bmiHeader.biClrUsed; i++)
                {
                    pDibInfo->bmiColors[i].rgbRed = (BYTE) i;
                    pDibInfo->bmiColors[i].rgbGreen = (BYTE) i;
                    pDibInfo->bmiColors[i].rgbBlue = (BYTE) i;
                    pDibInfo->bmiColors[i].rgbReserved = 0;
                }
            }
        }
        break;

        case TWPT_RGB:
            pDibInfo->bmiHeader.biClrUsed = 0;
        break;

        case TWPT_PALETTE:
        case TWPT_CMY:
        case TWPT_CMYK:
        case TWPT_YUV:
        case TWPT_YUVK:
        case TWPT_CIEXYZ:
        default:
        {
            CTL_Palette8Triplet  Palette8(pSession,
                                          pSource,
                                          CTL_GetTypeGET);

            if ( Palette8.Execute() == TWRC_FAILURE )
            {
                pDibInfo->bmiHeader.biClrImportant = 0;
                pDibInfo->bmiHeader.biClrUsed = 256;
                for (int i=0; i<=255; i++)
                {
                    pDibInfo->bmiColors[i].rgbRed = (BYTE)i;
                    pDibInfo->bmiColors[i].rgbGreen = (BYTE)i;
                    pDibInfo->bmiColors[i].rgbBlue = (BYTE)i;
                    pDibInfo->bmiColors[i].rgbReserved = 0;
                }
            }
            else
            {
                TW_PALETTE8 *pBuf = Palette8.GetPalette8Buffer();
                pDibInfo->bmiHeader.biClrUsed = pBuf->NumColors;
                pDibInfo->bmiHeader.biClrImportant = 0;
                for (int i=0; i<pBuf->NumColors; i++)
                {
                    pDibInfo->bmiColors[i].rgbRed = pBuf->Colors[i].Channel1;
                    pDibInfo->bmiColors[i].rgbGreen = pBuf->Colors[i].Channel2;
                    pDibInfo->bmiColors[i].rgbBlue = pBuf->Colors[i].Channel3;
                    pDibInfo->bmiColors[i].rgbReserved = 0;
                }
            }
        }
        break;
    }

//    GlobalUnlock(hGlobal);
    return true;
}




int CTL_TwainAppMgr::StartTransfer( CTL_ITwainSession * /*pSession*/,
                                     CTL_ITwainSource *pSource,
                                     CTL_ImageXferTriplet *pTrip)
{
    int nCurImage = pSource->GetPendingImageNum();
    pSource->SetState(SOURCE_STATE_TRANSFERRING);
    pSource->SetTransferDone(false);
    TW_UINT16 rc = pTrip->Execute();
    switch (rc )
    {
        case TWRC_FAILURE:
        {
            // A failure occurred.  See if termination is done
            if (pTrip->GetAcquireFailAction() == DTWAIN_PAGEFAIL_TERMINATE)
                return false; //  No more images pending
            else
                return -1; // Failure, attempt to scan again
        }
        break;

        case TWRC_XFERDONE:
        {
            // Check if File Transfer and using Prompt
            CTL_TwainAcquireEnum nAcquireType = pSource->GetAcquireType();
            if ( nAcquireType == TWAINAcquireType_FileUsingNative )
            {
                // Check if using Prompt
                long lFlags   = pSource->GetAcquireFileFlags();
                if ( (lFlags & DTWAIN_USEPROMPT) && pSource->IsPromptPending())
                {
                    pTrip->PromptAndSaveImage( nCurImage );
                    if ( pSource->IsDeleteDibOnScan() )
                    {
                        CTL_TwainDibArray* pArray;
                        // Get the array of current array of DIBS (this pointer allows changes to Source's internal DIB array)
                        pArray = pSource->GetDibArray();
                        // Let array class handle deleting of the DIB (Global memory will be freed only)
                        pArray->DeleteDibMemory( nCurImage );
                    }
                }
            }
        }
        break;
    }
    return pTrip->IsScanPending();  // true = more images, false = no more images
}


bool CTL_TwainAppMgr::GetFileTransferDefaults(CTL_ITwainSource *pSource,
                                              CTL_StringType& strFile, int &nFileType)
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
    CTL_SetupFileXferTriplet  FileXferGetDef( pSession, pTempSource,
                                            (int)CTL_GetTypeGETDEFAULT,
                                            (CTL_TwainFileFormatEnum)0,
                                            _T(""));
    if ( FileXferGetDef.Execute() == TWRC_SUCCESS )
    {
        strFile = FileXferGetDef.GetFileName();
        nFileType = FileXferGetDef.GetFileFormat();
        return true;
    }
    return false;
}


CTL_TwainAcquireEnum CTL_TwainAppMgr::GetCompatibleFileTransferType( const CTL_ITwainSource *pSource )
{
    CTL_IntArray iArray;
    EnumTransferMechanisms( pSource, iArray );
    if ( iArray.empty() )
        return TWAINAcquireType_Invalid;
    if ( std::find(iArray.begin(), iArray.end(), TWSX_FILE) != iArray.end())
        return TWAINAcquireType_File;
    return TWAINAcquireType_FileUsingNative;
}

bool CTL_TwainAppMgr::IsSupportedFileFormat( const CTL_ITwainSource* pSource,
                                             int nFileFormat )
{
    CTL_IntArray iArray;
    EnumTwainFileFormats( pSource, iArray );
    if ( iArray.empty() )
        return ( nFileFormat != TWFF_BMP )?false:true;

    return (std::find(iArray.begin(), iArray.end(), nFileFormat) != iArray.end())?true:false;
}



const CTL_ITwainSource*  CTL_TwainAppMgr::
                        GetDefaultSource(CTL_ITwainSession *pSession)
{
    if ( !s_pGlobalAppMgr )
        DTWAIN_ERROR_CONDITION(IDS_ErrTwainMgrInvalid,NULL);

    if ( find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession) != s_pGlobalAppMgr->m_arrTwainSession.end() )
    {
        CTL_ITwainSource *pSource = pSession->GetDefaultSource();
        if ( !pSource )
            DTWAIN_ERROR_CONDITION(IDS_ErrInvalidSourceHandle,NULL);
        return pSource;
    }
    DTWAIN_ERROR_CONDITION(IDS_ErrInvalidSessionHandle,NULL);
}


HINSTANCE CTL_TwainAppMgr::GetAppInstance()
{
    if ( s_pGlobalAppMgr )
        return s_pGlobalAppMgr->m_Instance;
    return 0;
}

int CTL_TwainAppMgr::SendTwainMsgToWindow(CTL_ITwainSession *pSession,
                                           HWND hWndWhich,
                                           WPARAM wParam,
                                           LPARAM lParam)
{
#ifdef _WIN32
    unsigned int nMsg = CTL_TwainAppMgr::GetRegisteredMsg();
    if ( hWndWhich == NULL )
    {
        if ( pSession )
        {
            HWND hWnd = *pSession->GetWindowHandlePtr();
            if ( hWnd )
                return (int)::SendMessage( hWnd, nMsg, wParam, lParam );
        }
    }
    else
        return (int)::SendMessage( hWndWhich, nMsg, wParam, lParam );
#endif
    return true;
}

bool CTL_TwainAppMgr::CloseSourceManager(CTL_ITwainSession *pSession)
{
    // Close the source manager
    // Use the session
    CTL_TwainCloseSMTriplet SM( pSession );
    TW_UINT16 rc = SM.Execute();
    if ( rc != TWRC_SUCCESS )
        return false;
    CTL_TwainDLLHandle::s_nDSMState = DSM_STATE_LOADED;
    return true;
}


////////// These are static error functions that get errors from the RC file
void CTL_TwainAppMgr::SetError(int nError, const CTL_StringType& extraInfo)
{
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    TCHAR szBuffer[256] = {0};
    int nRealError = nError;
    if ( nRealError > 0 )
        nRealError = -nRealError;

    static_cast<CTL_TwainDLLHandle *>(::GetDTWAINHandle_Internal())->m_lLastError = nRealError;

    if ( nError < 0 )
        nError = -nError;  // Can't have negative error codes
    GetResourceString(nError, szBuffer, 255);
    s_strLastError  = szBuffer;
    s_strLastError += extraInfo;
    s_nLastError    = nError;

    if ( CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_USEBUFFER )
    {
        // Push error onto error stack
        std::deque<int>::size_type nEntries = CTL_TwainDLLHandle::s_vErrorBuffer.size();
        CTL_TwainDLLHandle::s_vErrorBuffer.push_front(-nError);


        // Check if beyond reserve size
        unsigned int nReserve = CTL_TwainDLLHandle::s_nErrorBufferReserve;

        if ( nEntries > nReserve)
            CTL_TwainDLLHandle::s_vErrorBuffer.resize(CTL_TwainDLLHandle::s_nErrorBufferThreshold);
    }

    // if there is a callback, call it now with the error notifications
    if ( pHandle->m_pCallbackFn )
    {
        UINT uMsg = CTL_TwainDLLHandle::s_nRegisteredDTWAINMsg;
        LogDTWAINMessage(NULL, uMsg, DTWAIN_TN_GENERALERROR, -nError, true);
        (*pHandle->m_pCallbackFn)(DTWAIN_TN_GENERALERROR, -nError, (LPARAM)0);
    }

    // If there is a 64 bit callback, call it now with the error notifications
    if ( pHandle->m_pCallbackFn64 )
    {
        UINT uMsg = CTL_TwainDLLHandle::s_nRegisteredDTWAINMsg;
        LogDTWAINMessage(NULL, uMsg, DTWAIN_TN_GENERALERROR, -nError, true);
        (*pHandle->m_pCallbackFn64)(DTWAIN_TN_GENERALERROR, -nError, (LPARAM)0);
    }

}

int CTL_TwainAppMgr::GetLastError()
{
    return s_nLastError;
}

LPTSTR CTL_TwainAppMgr::GetLastErrorString(LPTSTR lpszBuffer, int nSize)
{
    return GetErrorString(GetLastError(), lpszBuffer, nSize);
}


LPTSTR CTL_TwainAppMgr::GetErrorString(int nError, LPTSTR lpszBuffer, int nSize)
{
    if ( nError )
    GetResourceString(nError, lpszBuffer, nSize);
    return lpszBuffer;
}

////////////////////////////////////////////////////////////////////////////
///////////////////  ******* Capability Code ******* //////////////////////
///////////////////////////////////////////////////////////////////////////
bool CTL_TwainAppMgr::IsCapabilitySupported(const CTL_ITwainSource *pSource, TW_UINT16 nCap, int nType /*=CTL_GetTypeGET*/)
{
	if (!pSource)
		return false;

    bool supported = pSource->IsCapInSupportedList(nCap);
    if ( supported )
        return true;

    if (pSource->IsCapInUnsupportedList(nCap))
        return false;

    CTL_ITwainSource *pTempSource = const_cast<CTL_ITwainSource *>(pSource);
    bool bRet = false;

    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();

    if ( !IsValidTwainSession( pSession) )
        return false;

    if ( !s_pGlobalAppMgr->IsSourceOpen( pSource ) )
        return false;

    std::unique_ptr<CTL_CapabilityGetTriplet> pTrip;
    switch (nType )
    {
        case CTL_GetTypeGET:
        case CTL_GetTypeGETCURRENT:
        case CTL_GetTypeGETDEFAULT:
            pTrip.reset(new CTL_CapabilityGetTriplet(pSession, pTempSource,
                       (CTL_EnumGetType)nType,
                       (CTL_EnumCapability)nCap,
                       0));
        break;

        default:
            return false;
    }
    bRet = pTrip->IsCapabilitySupported();
    return bRet;
}


bool CTL_TwainAppMgr::IsSourceOpen( const CTL_ITwainSource *pSource )
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;

    if ( !pSource )
        return false;

    CTL_ITwainSession* pSession =
                        pTempSource->GetTwainSession();

    if ( !IsValidTwainSession( pSession) )
        return false;
    if ( !pTempSource->IsOpened() )
        return false;
    return true;
}


bool CTL_TwainAppMgr::GetMultipleIntValues( const CTL_ITwainSource *pSource, CTL_IntArray & pArray, CTL_CapabilityGetTriplet *pTrip)
{
    return GetMultipleValues<CTL_IntArray, TW_UINT16>(pSource, pArray, pTrip);
}


bool CTL_TwainAppMgr::GetMultipleRealValues( const CTL_ITwainSource *pSource,CTL_RealArray & pArray,CTL_CapabilityGetTriplet *pTrip)
{
    return GetMultipleValues<CTL_RealArray, float>(pSource, pArray, pTrip);
}


bool CTL_TwainAppMgr::GetOneIntValue(const CTL_ITwainSource *pSource,
                                     TW_UINT16* pVal,
                                     CTL_CapabilityGetTriplet *pTrip)
{
    CTL_IntArray Array;
    if ( !GetMultipleIntValues( pSource, Array, pTrip ) )
        return false;
    if ( !Array.empty() )
    {
        *pVal = (TW_UINT16)Array[0];
        return true;
    }
    return false;
}

//////////////////////////////////////////////////////////////////////////////
bool CTL_TwainAppMgr::GetCurrentOneCapValue(const CTL_ITwainSource *pSource,
                                            void *pValue,
                                            TW_UINT16 Cap,
                                            TW_UINT16 nDataType )
{
    return GetOneTwainCapValue( pSource, pValue, Cap, CTL_GetTypeGETCURRENT, nDataType);
}

bool CTL_TwainAppMgr::GetOneCapValue(const CTL_ITwainSource *pSource,
                                     void *pValue,
                                     TW_UINT16 Cap,
                                     TW_UINT16 nDataType )
{
    return GetOneTwainCapValue( pSource, pValue, Cap, CTL_GetTypeGET, nDataType);
}

bool CTL_TwainAppMgr::GetOneTwainCapValue( const CTL_ITwainSource *pSource,
                                           void *pValue,
                                           TW_UINT16 Cap,
                                           CTL_EnumGetType GetType,
                                           TW_UINT16 nDataType )
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    // Get the #transfer count
    CTL_ITwainSession* pSession = pTempSource->GetTwainSession();
    CTL_CapabilityGetOneValTriplet GetOne( pSession,
                                           pTempSource,
                                           GetType,
                                           Cap,
                                           nDataType);
    if ( !IsSourceOpen( pSource ) )
        return false;

    TW_UINT16   rc = GetOne.Execute();
    switch (rc)
    {
        case TWRC_SUCCESS:
        {
            GetOne.GetValue(pValue, 0);
            return true;
        }
        break;
        case TWRC_FAILURE:
            pSession = pTempSource->GetTwainSession();
            TW_UINT16 ccode = GetConditionCode(pSession, NULL);
            ProcessConditionCodeError(ccode);
            SendTwainMsgToWindow(pSession, NULL, TWRC_FAILURE, ccode);
            return false;
        break;
    }
    return false;
}
////////////////////////////////////////////////////////////////////////////////////////

////////////////////////// Mandatory capabilities //////////////////////////
///////////////////////// Transfer Count ///////////////////////////////////////
int CTL_TwainAppMgr::GetTransferCount( const CTL_ITwainSource *pSource )
{
    TW_UINT16 nValue;
    GetOneCapValue( pSource, &nValue, TwainCap_XFERCOUNT, TWTY_UINT16 );
    return nValue;
}

int CTL_TwainAppMgr::SetTransferCount( const CTL_ITwainSource *pSource,
                                       int nCount )
{
    if (CTL_TwainDLLHandle::s_lErrorFilterFlags )
    {
        CTL_StringStreamType strm;
        strm << BOOST_FORMAT(_T("\nSetting Transfer Count.  Transfer Count = %1%\n")) % nCount;
        WriteLogInfo(strm.str());
    }
    SetOneTwainCapValue( pSource, nCount, CTL_SetTypeSET, TwainCap_XFERCOUNT, TWTY_INT16);
    return 1;
}


TW_UINT16 CTL_TwainAppMgr::ProcessReturnCodeOneValue(CTL_ITwainSource *pSource, TW_UINT16 rc)
{
    CTL_ITwainSession* pSession = pSource->GetTwainSession();
    switch (rc)
    {
        case TWRC_SUCCESS:
            return true;

        case TWRC_FAILURE:
            pSession = pSource->GetTwainSession();
            TW_UINT16 ccode = GetConditionCode(pSession, NULL);
            ProcessConditionCodeError(ccode);
            SendTwainMsgToWindow(pSession, NULL, TWRC_FAILURE, ccode);
            return false;
          break;
    }
    return false;
}

////////////////////////// Transfer mechanisms ///////////////////////////////////
int CTL_TwainAppMgr::SetTransferMechanism( const CTL_ITwainSource *pSource,
                                           CTL_TwainAcquireEnum AcquireType,
                                           LONG ClipboardTransferType)
{
    // Set the transfer mechanism
    // Change AcquireType to TWAIN type
    TW_UINT16 uTwainType = (TW_UINT16)AcquireType;
    if ( AcquireType == TWAINAcquireType_FileUsingNative )
        uTwainType = TWSX_NATIVE;
    else
    if ( AcquireType == TWAINAcquireType_Clipboard)
        uTwainType = static_cast<TW_UINT16>(ClipboardTransferType);

    SetOneTwainCapValue( pSource, uTwainType, CTL_SetTypeSET, TwainCap_XFERMECH, TWTY_UINT16);
    return 1;
}

void CTL_TwainAppMgr::EnumTransferMechanisms( const CTL_ITwainSource *pSource,
                                                CTL_IntArray & rArray )
{
    GetMultiValuesImpl<CTL_IntArray, TW_UINT16>::GetMultipleTwainCapValues(pSource, rArray, TwainCap_XFERMECH, TWTY_UINT16);
}
//////////////////////////////////////////////////////////////////////

////////////////////// Pixel and Bit Depth settings /////////////////
void CTL_TwainAppMgr::SetPixelAndBitDepth(const CTL_ITwainSource * /*pSource*/)
{}


/////////////// Supported formats for File transfers ////////////////
void CTL_TwainAppMgr::EnumTwainFileFormats( const CTL_ITwainSource * /*pSource*/, CTL_IntArray & rArray )
{
    static const CTL_IntArray ca = { TWAINFileFormat_BMP, TWAINFileFormat_PCX, TWAINFileFormat_DCX, TWAINFileFormat_TIFFLZW,
                                    TWAINFileFormat_PDF, TWAINFileFormat_PDFMULTI, TWAINFileFormat_TIFFNONE, TWAINFileFormat_TIFFGROUP3,TWAINFileFormat_TIFFGROUP4
                                 ,TWAINFileFormat_TIFFPACKBITS, TWAINFileFormat_TIFFDEFLATE, TWAINFileFormat_TIFFJPEG, TWAINFileFormat_TIFFNONEMULTI,
                                 TWAINFileFormat_TIFFGROUP3MULTI, TWAINFileFormat_TIFFGROUP4MULTI, TWAINFileFormat_TIFFPACKBITSMULTI,TWAINFileFormat_TIFFDEFLATEMULTI,TWAINFileFormat_TIFFJPEGMULTI
                                ,TWAINFileFormat_TIFFLZWMULTI ,TWAINFileFormat_WMF,TWAINFileFormat_EMF,TWAINFileFormat_PSD,TWAINFileFormat_JPEG,TWAINFileFormat_TGA
                                ,TWAINFileFormat_JPEG2000,TWAINFileFormat_POSTSCRIPT1,TWAINFileFormat_POSTSCRIPT1MULTI,TWAINFileFormat_POSTSCRIPT2,TWAINFileFormat_POSTSCRIPT2MULTI
                                ,TWAINFileFormat_POSTSCRIPT3,TWAINFileFormat_POSTSCRIPT3MULTI,TWAINFileFormat_GIF,TWAINFileFormat_PNG,TWAINFileFormat_TEXT
                                ,TWAINFileFormat_TEXTMULTI,TWAINFileFormat_ICO,TWAINFileFormat_ICO_VISTA, TWAINFileFormat_WBMP, TWAINFileFormat_WEBP, TWAINFileFormat_PBM };
      rArray = ca;
}

struct FindTriplet
{
    FindTriplet(const RawTwainTriplet& theTriplet) : m_Trip(theTriplet) { }
    bool operator() (const RawTwainTriplet& trip)
    {
        return (m_Trip.nDG == trip.nDG &&
                m_Trip.nDAT == trip.nDAT &&
                m_Trip.nMSG == trip.nMSG);
    }
    private:
        RawTwainTriplet m_Trip;
};

void CTL_TwainAppMgr::EnumNoTimeoutTriplets()
{
    RawTwainTriplet  Trips[] = {
        {DG_AUDIO, DAT_AUDIOFILEXFER, MSG_GET},
        {DG_AUDIO, DAT_AUDIONATIVEXFER, MSG_GET},
        {DG_CONTROL, DAT_USERINTERFACE, MSG_ENABLEDS},
        {DG_CONTROL, DAT_USERINTERFACE, MSG_ENABLEDSUIONLY},
        {DG_IMAGE, DAT_IMAGEFILEXFER, MSG_GET},
        {DG_IMAGE, DAT_IMAGENATIVEXFER, MSG_GET},
        {DG_IMAGE, DAT_IMAGEMEMXFER, MSG_GET}
    };

    const int nItems = sizeof(Trips) / sizeof(Trips[0]);
    std::copy(Trips, Trips + nItems, std::back_inserter(s_NoTimeoutTriplets));
}

////////////////////////////////////////////////////////////////////////
/////////////////////// Pixel Types ///////////////////////////////////
void CTL_TwainAppMgr::GetPixelTypes( const CTL_ITwainSource *pSource,
                                     CTL_IntArray & rArray )
{
    GetMultiValuesImpl<CTL_IntArray, TW_UINT16>::GetMultipleTwainCapValues(pSource, rArray, TwainCap_PIXELTYPE,TWTY_UINT16);
}
///////////////////////////////////////////////////////////////////////
CTL_TwainUnitEnum CTL_TwainAppMgr::GetCurrentUnitMeasure(const CTL_ITwainSource *pSource)
{
    TW_INT16 nValue;
    if ( !GetCurrentOneCapValue(pSource, &nValue, TwainCap_UNITS, TWTY_UINT16) )
    {
        return TwainUnit_INCHES;
    }
    return (CTL_TwainUnitEnum) nValue;
}
//////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////
void CTL_TwainAppMgr::GetCompressionTypes( const CTL_ITwainSource *pSource,
                                           CTL_IntArray & rArray )
{
    GetMultiValuesImpl<CTL_IntArray, TW_UINT16>::GetMultipleTwainCapValues(pSource, rArray, TwainCap_COMPRESSION,TWTY_UINT16);
}


void CTL_TwainAppMgr::GetUnitTypes( const CTL_ITwainSource *pSource,
                                    CTL_IntArray & rArray )
{
    GetMultiValuesImpl<CTL_IntArray, TW_UINT16>::GetMultipleTwainCapValues(pSource, rArray, TwainCap_UNITS,TWTY_UINT16);
}

/////////////////////// End mandatory capabilities /////////////////////////

////////////////// Capabilities that should be supported ///////////////////
/*CAP_XFERCOUNT
Every Source must support DG_CONTROL / DAT_CAPABILITY MSG_GET on:
CAP_SUPPORTEDCAPS
CAP_UICONTROLLABLE
Sources that supply image information must support DG_CONTROL / DAT_CAPABILITY /
MSG_GET, MSG_GETCURRENT, MSG_GETDEFAULT on:
ICAP_COMPRESSION
ICAP_PLANARCHUNKY
ICAP_PHYSICALHEIGHT
ICAP_PHYSICALWIDTH
ICAP_PIXELFLAVOR
Sources that supply image information must support DG_CONTROL / DAT_CAPABILITY /
MSG_GET, MSG_GETCURRENT, MSG_GETDEFAULT, MSG_RESET and MSG_SET on:
ICAP_BITDEPTH
ICAP_BITORDER
ICAP_PIXELTYPE
ICAP_UNITS
ICAP_XFERMECH
ICAP_XRESOLUTION
ICAP_YRESOLUTION
  */
#ifdef __TURBOC__
typedef set<TW_UINT16, less<TW_UINT16> > MandatorySet;
#else
typedef set<TW_UINT16> MandatorySet;
#endif

void CTL_TwainAppMgr::GetCapabilities(const CTL_ITwainSource *pSource,
                                      CTL_TwainCapArray & rArray,
                                      TW_UINT16 MaxCustomBase,
                                      bool bGetCustom)
{
    CTL_EnumContainer ContainerToUse = TwainContainer_ARRAY;
    // Get the capabilities
    if ( !bGetCustom )
    {
        MandatorySet MandatoryCaps = { CAP_XFERCOUNT, ICAP_XFERMECH, ICAP_XRESOLUTION, ICAP_YRESOLUTION };

        if (IsCapabilitySupported( pSource, DTWAIN_CV_CAPSUPPORTEDCAPS, CTL_GetTypeGET))
        {
            // Double check what the right GET container is to use
            UINT cGet=0, cSet=0, nDataType=0;
            bool cFlags[6] = {false};
            bool bSuccess = CTL_TwainAppMgr::GetBestContainerType(pSource,
                            (CTL_EnumCapability)DTWAIN_CV_CAPSUPPORTEDCAPS,
                            cGet, cSet, nDataType,
                            CTL_GetTypeGET, cFlags );
            if ( bSuccess )
                ContainerToUse = (CTL_EnumContainer)cGet;
            GetMultiValuesImpl<CTL_TwainCapArray, TW_UINT16>::GetMultipleTwainCapValues(pSource, rArray, TwainCap_SUPPORTEDCAPS, TWTY_UINT16, ContainerToUse);

            // Now check if the mandatory capabilities are hidden from us
            // First mark off the ones that are found
            size_t nSize = rArray.size();
            if ( nSize > 10 )
            {
                // Look for more caps
                for ( size_t i = 0; i < nSize; ++i )
                        MandatoryCaps.erase( (const unsigned short)rArray[i] );

                // If found these caps, return since we're OK
                if ( MandatoryCaps.empty() )
                    return;

                // Check the caps left from the mandatory caps by probing
                MandatorySet::iterator it = MandatoryCaps.begin();
                MandatorySet::iterator it2 = MandatoryCaps.end();
                CTL_String s;
                CTL_String s2;
                while ( it != it2 )
                {
                    s = "Probing for capability ";
                    s += GetCapNameFromCap( *it );
                    s += "...\n";
                    WriteLogInfo(StringConversion::Convert_Ansi_To_Native(s));
                    if (IsCapabilitySupported(pSource, *it, CTL_GetTypeGET))
                    {
                        s2 = " was ";
                        rArray.push_back(*it);
                    }
                    else
                    {
                        s2 = " was not ";
                    }
                    s = "Capability ";
                    s += GetCapNameFromCap( *it );
                    s += s2;
                    s += "found\n";
                    WriteLogInfo(StringConversion::Convert_Ansi_To_Native(s));

                    ++it;
                }

                if (rArray.size() > 10 )  // Does it really have anything?
                    return;
            }
        }
    }

    rArray.clear();

    auto it= CTL_TwainDLLHandle::s_mapGeneralCapInfo.begin();
    auto itend = CTL_TwainDLLHandle::s_mapGeneralCapInfo.end();
//    TW_UINT16 iCap;
    std::for_each(it, itend, [&](CTL_GeneralCapInfo::value_type& v) {
                  if (IsCapabilitySupported(pSource, v.first, CTL_GetTypeGET))
                        rArray.push_back(v.first);
                    });

    if ( MaxCustomBase >= CAP_CUSTOMBASE )
    {
        TW_UINT16 nStart = CAP_CUSTOMBASE;
        TW_UINT16 nEnd = MaxCustomBase;
        for (TW_UINT16 i = nStart; i < nEnd; i++ )
        {
            if (IsCapabilitySupported(pSource, i, CTL_GetTypeGET))
                rArray.push_back(i);
        }
    }
}

void CTL_TwainAppMgr::GetExtendedCapabilities(const CTL_ITwainSource *pSource,
                                             CTL_IntArray & rArray)
{
    // Get the capabilities
    GetMultiValuesImpl<CTL_IntArray, TW_UINT16>::GetMultipleTwainCapValues(pSource, rArray, TwainCap_EXTENDEDCAPS, TWTY_UINT16, TwainContainer_ARRAY);
}

UINT CTL_TwainAppMgr::GetCapOps(const CTL_ITwainSource *pSource, int nCap, bool bCanQuery)
{
    UINT nOps = 0;
    if ( bCanQuery )
        nOps = GetCapabilityOperations(pSource, nCap);

    if ( nOps == 0 )
    {
        UINT nContainer = GetContainerTypesFromCap( (CTL_EnumCapability)nCap, 1 );
        nOps = 0xFFFF;
        if ( !nContainer )
            nOps = 0xFFFF & ~(TWQC_SET | TWQC_RESET);
    }
    return nOps;
}

UINT CTL_TwainAppMgr::GetCapabilityOperations(const CTL_ITwainSource *pSource,
                                              int nCap)
{
    CTL_ITwainSource *pTempSource = (CTL_ITwainSource *)pSource;
    if ( !pSource )
        return 0;

    CTL_ITwainSession* pSession =
                        pTempSource->GetTwainSession();

    if ( !IsValidTwainSession( pSession) )
        return 0;

    if ( !s_pGlobalAppMgr->IsSourceOpen( pSource ) )
        return 0;

    CTL_CapabilityQueryTriplet QT(pSession, pTempSource, (CTL_EnumCapability)nCap);
    TW_UINT16 rc = QT.Execute();
    if ( rc != TWRC_SUCCESS )
        return 0;
    return QT.GetSupport();
}

////////////////// End Capabilities that should be supported /////////////////

/////////////// Capabilities that do not have to be supported ////////////////
bool CTL_TwainAppMgr::IsFeederLoaded( const CTL_ITwainSource *pSource )
{
    TW_UINT16 nValue;
    GetOneCapValue( pSource, &nValue, TwainCap_FEEDERLOADED, TWTY_BOOL);
    return nValue?true:false;
}


bool CTL_TwainAppMgr::IsFeederEnabled( const CTL_ITwainSource *pSource, TW_UINT16& nValue )
{
    if (!GetOneCapValue( pSource, &nValue,
                         TwainCap_FEEDERENABLED, TWTY_BOOL))
        return false;
    return true;
}

bool CTL_TwainAppMgr::IsJobControlSupported( const CTL_ITwainSource *pSource, TW_UINT16& nValue )
{
    if (!GetOneCapValue( pSource, &nValue,
                         TwainCap_JOBCONTROL, TWTY_UINT16 ))
        return false;
    return true;
}

bool CTL_TwainAppMgr::SetupFeeder( const CTL_ITwainSource *pSource, int /*maxpages*/, bool bSet )
{
    if ( !pSource->IsFeederEnabledMode())
        return false;

    // Determine if there is a document feeder capability
    TW_UINT16 nValue;
    if ( !IsFeederEnabled(pSource, nValue))
        return false;

    TW_BOOL bValue;

    // Check if it needs to be turned off via source or implicitly
    bool bTurnOffAutoFeed = !pSource->GetAutoFeedMode();

    if ( bSet == false || bTurnOffAutoFeed )
    {
        // Turn off autofeed
        // Enable the CAP_AUTOFEED capability
        // Get a set capability triplet compatible for one value
        bValue = false;
        SetOneTwainCapValue( pSource, bValue, CTL_SetTypeSET, TwainCap_AUTOFEED, TWTY_BOOL );

        // Return, since the autofeed has been turned off and the feeder has been
        // disabled
        if ( bSet == false )
            return true;
    }

    // Set the automatic document feeder mode if present
    nValue = true;
    SetOneTwainCapValue( pSource, nValue, CTL_SetTypeSET, TwainCap_FEEDERENABLED, TWTY_BOOL);

    // Enable the CAP_AUTOFEED capability if the user wants to automatically feed
    // the page
    // Get a set capability triplet compatible for one value
    if ( !bTurnOffAutoFeed)
    {
        bValue = true;
        SetOneTwainCapValue( pSource, bValue, CTL_SetTypeSET, TwainCap_AUTOFEED, TWTY_BOOL);
    }

    return true;
}

bool CTL_TwainAppMgr::ShowProgressIndicator(CTL_ITwainSource *pSource, bool bShow)
{
    bool bTemp = bShow;
    return SetOneTwainCapValue( pSource, &bTemp, CTL_SetTypeSET, TwainCap_INDICATORS, TWTY_BOOL );
}

bool CTL_TwainAppMgr::IsProgressIndicatorOn(CTL_ITwainSource *pSource)
{
    bool bTemp;
    if ( GetOneCapValue( pSource, &bTemp, TwainCap_INDICATORS, TWTY_BOOL ) )
        return bTemp;
    return false;
}

void CTL_TwainAppMgr::AddConditionCodeError(TW_UINT16 nCode, int nResource)
{
    CTL_CondCodeInfo Code( nCode, nResource );
    s_mapCondCode[nCode] = Code;
}

CTL_CondCodeInfo CTL_TwainAppMgr::FindConditionCode(TW_UINT16 nCode)
{
    mapCondCodeInfo::iterator it;
    it = s_mapCondCode.find(nCode);
    if ( it == s_mapCondCode.end() )
        return CTL_CondCodeInfo((TW_UINT16)-9999, (TW_UINT16)-9999);
    return (*it).second;
}

void CTL_TwainAppMgr::RemoveAllConditionCodeErrors()
{
    s_mapCondCode.clear();
}

CTL_String CTL_TwainAppMgr::GetCapNameFromCap( LONG Cap )
{
    CTL_String sName = GetGeneralCapInfo(Cap);
    StringWrapperA::TrimAll(sName);
    if ( (UINT)Cap >= CAP_CUSTOMBASE )
    {
        CTL_StringStreamA strm;
        strm << boost::format("CAP_CUSTOMBASE + %1%") % ((long)Cap - (long)CAP_CUSTOMBASE);
        return strm.str();
    }
    else
    if ( sName.empty())
    {
        CTL_StringStreamA strm;
        strm << std::hex << Cap;
        sName += "Unknown capability.  Hex value: " + strm.str();
    }
    return sName;
}

UINT CTL_TwainAppMgr::GetDataTypeFromCap( CTL_EnumCapability Cap, CTL_ITwainSource *pSource/*=NULL*/ )
{
    if ( (unsigned int)Cap >= CAP_CUSTOMBASE )
        return DTWAIN_GetCapDataType(pSource, Cap);
    CTL_CapStruct cStruct = GetGeneralCapInfo(Cap);
    if ( ((CTL_String)cStruct).length() == 0 )
        return 0xFFFF;
    return cStruct.m_nDataType;
}

CTL_CapStruct CTL_TwainAppMgr::GetGeneralCapInfo(LONG Cap)
{
    if ( Cap >= CAP_CUSTOMBASE )
        Cap = CAP_CUSTOMBASE;

    CTL_CapStruct cStruct;
    bool bFoundCap = false;

    auto it = CTL_TwainDLLHandle::s_mapGeneralCapInfo.find( (short int)Cap );
    if ( it != CTL_TwainDLLHandle::s_mapGeneralCapInfo.end() )
    {
        bFoundCap = true;
        cStruct = (*it).second;
    }

    if ( Cap >= CAP_CUSTOMBASE && !bFoundCap )
    {
        Cap = CAP_CUSTOMBASE;
        cStruct = CTL_TwainDLLHandle::s_mapGeneralCapInfo[(short int)Cap];
    }
    return cStruct;
}

LONG CTL_TwainAppMgr::GetCapFromCapName(const char *szCapName )
{
    CTL_CapStruct cStruct;

    CTL_String strCap = szCapName;
    StringWrapperA::TrimAll(strCap);
    StringWrapperA::MakeUpperCase(strCap);
    if ( strCap.empty() )
        return TwainCap_INVALID;

    if ( StringWrapperA::Left(strCap, 14) == "CAP_CUSTOMBASE")
    {
        // Extract the integer portion
        CTL_StringArray sArray;
        CTL_String sNum;
        StringWrapperA::Tokenize(StringWrapperA::Mid(strCap, 14), "+ ", sArray);
        size_t nSize = sArray.size();
        if ( nSize > 0 )
        {
            sNum = sArray[nSize-1];
            int nNum = stoi(sNum);
            return CAP_CUSTOMBASE + nNum;
        }
    }

    auto it = CTL_TwainDLLHandle::s_mapGeneralCapInfo.begin();
    auto itend = CTL_TwainDLLHandle::s_mapGeneralCapInfo.end();
    int subtractor = 0;
    if (StringWrapperA::Left(strCap, 5) == "TWEI_")
        subtractor = 1000;
    auto foundIter = std::find_if(it, itend, [&](CTL_GeneralCapInfo::value_type& vt)
    {
        if ((CTL_String)(vt.second) == strCap)
            return true;
       return false;
    });
    if (foundIter != itend)
        return static_cast<CTL_EnumCapability>(foundIter->first) - subtractor;
    return TwainCap_INVALID;
}

UINT CTL_TwainAppMgr::GetContainerTypesFromCap( CTL_EnumCapability Cap, bool nType )
{
    CTL_CapStruct cStruct = GetGeneralCapInfo(Cap);

    if ( ((CTL_String)cStruct).empty())
        return 0;

    if ( !nType )
        return cStruct.m_nGetContainer;
    return cStruct.m_nSetContainer;
}

CTL_ErrorStruct CTL_TwainAppMgr::GetGeneralErrorInfo(LONG nDG, UINT nDAT, UINT nMSG)
{
    CTL_ErrorStruct eStruct;
    auto it = CTL_TwainDLLHandle::s_mapGeneralErrorInfo.find(std::make_tuple(nDG, nDAT, nMSG));
    if ( it != CTL_TwainDLLHandle::s_mapGeneralErrorInfo.end() )
        eStruct = (*it).second;
    return eStruct;
}

void CTL_TwainAppMgr::GetContainerNamesFromType( int nType, CTL_StringArray &rArray )
{
    rArray.clear();
    if ( nType & TwainContainer_ONEVALUE )
        rArray.push_back( "TW_ONEVALUE");
    if ( nType & TwainContainer_ENUMERATION )
        rArray.push_back( "TW_ENUMERATION");
    if ( nType & TwainContainer_ARRAY )
        rArray.push_back( "TW_ARRAY");
    if ( nType & TwainContainer_RANGE )
        rArray.push_back( "TW_RANGE");
}


int CTL_TwainAppMgr::GetCapMaskFromCap( CTL_EnumCapability Cap )
{
    // Jump table
    int CapAll = CTL_CapMaskGET | CTL_CapMaskGETCURRENT | CTL_CapMaskGETDEFAULT |
                 CTL_CapMaskSET | CTL_CapMaskRESET;

    int CapSupport = CTL_CapMaskGET | CTL_CapMaskGETCURRENT | CTL_CapMaskGETDEFAULT |
                     CTL_CapMaskRESET;
    int CapAllGets = CTL_CapMaskGET | CTL_CapMaskGETCURRENT | CTL_CapMaskGETDEFAULT;

    switch (Cap)
    {
        case TwainCap_XFERCOUNT     :
        case TwainCap_AUTOFEED      :
        case TwainCap_CLEARPAGE     :
        case TwainCap_REWINDPAGE    :
            return CapAll;

        case TwainCap_COMPRESSION   :
        case TwainCap_PIXELTYPE     :
        case TwainCap_UNITS         :
        case TwainCap_XFERMECH      :
        case TwainCap_BITDEPTH      :
        case TwainCap_BITORDER      :
        case TwainCap_XRESOLUTION   :
        case TwainCap_YRESOLUTION   :
            return CapSupport;

        case TwainCap_UICONTROLLABLE :
        case TwainCap_SUPPORTEDCAPS  :
            return CTL_CapMaskGET;

        case TwainCap_PLANARCHUNKY   :
        case TwainCap_PHYSICALHEIGHT :
        case TwainCap_PHYSICALWIDTH  :
        case TwainCap_PIXELFLAVOR    :
        case TwainCap_FEEDERENABLED  :
        case TwainCap_FEEDERLOADED   :
            return CapAllGets;
    }
    return 0;
}

bool CTL_TwainAppMgr::IsCapMaskOn( CTL_EnumCapability Cap, CTL_EnumGetType GetType)
{
    int CapMask = GetCapMaskFromCap( Cap );
    if ( CapMask & GetType )
        return true;
    return false;
}


bool CTL_TwainAppMgr::IsCapMaskOn( CTL_EnumCapability Cap, CTL_EnumSetType SetType)
{
    int CapMask = GetCapMaskFromCap( Cap );
    if ( CapMask & SetType )
        return true;
    return false;
}

bool CTL_TwainAppMgr::IsSourceCompliant( const CTL_ITwainSource *pSource,
                                         CTL_EnumTwainVersion TVersion,
                                         CTL_TwainCapArray& rArray )
{
    if ( !s_pGlobalAppMgr )
        return false;

    if ( !s_pGlobalAppMgr->IsSourceOpen( pSource ))
        return false;

    return ((CTL_ITwainSource *)pSource)->IsSourceCompliant( TVersion, rArray );
}

#ifdef _WIN32
#include "winget_twain.inl"
#else
#include "linuxget_twain.inl"
#endif

CTL_StringType CTL_TwainAppMgr::GetTwainDirFullName(LPCTSTR strTwainDLLName,
                                                    LPLONG pWhichSearch,
                                                    bool bLeaveLoaded/*=false*/,
                                                    boost::dll::shared_library *pModule)
{
    return ::GetTwainDirFullName(strTwainDLLName, pWhichSearch, bLeaveLoaded, pModule);
}

bool CTL_TwainAppMgr::CheckTwainExistence(const CTL_StringType& strTwainDLLName, LPLONG pWhichSearch)
{
    if ( GetTwainDirFullName(strTwainDLLName.c_str(), pWhichSearch).empty())
        return false;
    return true;
}

LONG CTL_TwainAppMgr::ExtImageInfoArrayType(LONG ExtType)
{
    switch(ExtType)
    {
        case DTWAIN_EI_BARCODETEXT:
        case DTWAIN_EI_ENDORSEDTEXT:
        case DTWAIN_EI_FORMTEMPLATEMATCH:
        case DTWAIN_EI_BOOKNAME:
        case DTWAIN_EI_CAMERA:
            return DTWAIN_ARRAYSTRING;

        case DTWAIN_EI_FRAME:
            return  DTWAIN_ARRAYFRAME;
    }
    return DTWAIN_ARRAYLONG;
}


/////////////////////////****************//////////////////////////////////
/////////////// member functions for the CTL_TwainAppMgr///////////////////
CTL_TwainAppMgr::CTL_TwainAppMgr(  LPCTSTR lpszDLLName,
                                    HINSTANCE hInstance,
	HINSTANCE /*hThisInstance*/) : m_nErrorTWCC(0), m_nErrorTWRC(0)
{
    if ( !lpszDLLName || lpszDLLName[0] == _T('\0') )
        m_strTwainDLLName = GetDefaultDLLName();
    else
         m_strTwainDLLName = lpszDLLName;

    #ifdef _WIN32
    // Register a twain App message
    m_nTwainMsg = ::RegisterWindowMessage(REGISTERED_DTWAIN_MSG);
    #endif

    // Record the instance
    m_Instance = hInstance;

    // Set up the error messages for condition codes
    AddConditionCodeError(TWCC_SUCCESS         ,IDS_ErrCCFalseAlarm );
    AddConditionCodeError(TWCC_BUMMER          ,IDS_ErrCCBummer     );
    AddConditionCodeError(TWCC_LOWMEMORY       ,IDS_ErrCCLowMemory );
    AddConditionCodeError(TWCC_NODS            ,IDS_ErrCCNoDataSource );
    AddConditionCodeError(TWCC_MAXCONNECTIONS  ,IDS_ErrCCMaxConnections );
    AddConditionCodeError(TWCC_OPERATIONERROR  ,IDS_ErrCCOperationError );
    AddConditionCodeError(TWCC_BADCAP          ,IDS_ErrCCBadCapability );
    AddConditionCodeError(TWCC_BADPROTOCOL     ,IDS_ErrCCBadProtocol );
    AddConditionCodeError(TWCC_BADVALUE        ,IDS_ErrCCBadValue );
    AddConditionCodeError(TWCC_SEQERROR        ,IDS_ErrCCSequenceError );
    AddConditionCodeError(TWCC_BADDEST         ,IDS_ErrCCBadDestination );
    AddConditionCodeError(TWCC_CAPUNSUPPORTED  ,IDS_ErrCCCapNotSupported );
    AddConditionCodeError(TWCC_CAPBADOPERATION ,IDS_ErrCCCapBadOperation );
    AddConditionCodeError(TWCC_CAPSEQERROR     ,IDS_ErrCCCapSequenceError );

    AddConditionCodeError(TWCC_DENIED          ,IDS_ErrCCFileProtected);
    AddConditionCodeError(TWCC_FILEEXISTS      ,IDS_ErrCCFileExists);
    AddConditionCodeError(TWCC_FILENOTFOUND    ,IDS_ErrCCFileNotFound);
    AddConditionCodeError(TWCC_NOTEMPTY        ,IDS_ErrCCDirectoryNotEmpty);
    AddConditionCodeError(TWCC_PAPERJAM        ,IDS_ErrCCFeederJammed);
    AddConditionCodeError(TWCC_PAPERDOUBLEFEED ,IDS_ErrCCFeederMultPages);
    AddConditionCodeError(TWCC_FILEWRITEERROR  ,IDS_ErrCCFileWriteError);
    AddConditionCodeError(TWCC_CHECKDEVICEONLINE,IDS_ErrCCDeviceOffline);
    AddConditionCodeError(TWCC_INTERLOCK,       IDS_ErrCCInterlock);
    AddConditionCodeError(TWCC_DAMAGEDCORNER,   IDS_ErrCCDamagedCorner);
    AddConditionCodeError(TWCC_FOCUSERROR,      IDS_ErrCCFocusError);
    AddConditionCodeError(TWCC_DOCTOOLIGHT,     IDS_ErrCCDoctooLight);
    AddConditionCodeError(TWCC_DOCTOODARK,      IDS_ErrCCDoctooDark);
    AddConditionCodeError(TWCC_NOMEDIA   ,      IDS_ErrCCNoMedia);


    // Special condition code
    AddConditionCodeError((TW_UINT16)TWAIN_ERR_NULL_CONTAINER, TWAIN_ERR_NULL_CONTAINER_);
    AddConditionCodeError(DTWAIN_ERR_EXCEPTION_ERROR_, DTWAIN_ERR_EXCEPTION_ERROR_);

    EnumNoTimeoutTriplets();

    m_lpDSMEntry = NULL;

}

void CTL_TwainAppMgr::OpenLogFile(LPCSTR lpszFile)
{
}


void CTL_TwainAppMgr::WriteToLogFile(int /*rc*/)
{
}


CTL_TwainAppMgr::~CTL_TwainAppMgr()
{
}


void CTL_TwainAppMgr::CloseLogFile()
{
}

void CTL_TwainAppMgr::DestroyAllTwainSessions()
{
    std::for_each(m_arrTwainSession.begin(), m_arrTwainSession.end(), CTL_ITwainSession::Destroy);
    m_arrTwainSession.clear();
}


void CTL_TwainAppMgr::DestroySession( CTL_ITwainSession *pSession )
{
    CTL_TwainSessionArray::iterator it;
    it = find(s_pGlobalAppMgr->m_arrTwainSession.begin(),
              s_pGlobalAppMgr->m_arrTwainSession.end(),
              pSession);
    if ( it != s_pGlobalAppMgr->m_arrTwainSession.end() )
    {
        CTL_ITwainSession::Destroy( pSession );
        m_arrTwainSession.erase( it );
    }
}

CTL_StringType CTL_TwainAppMgr::GetDefaultDLLName()
{
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    if ( pHandle )
    {
        if ( pHandle->m_SessionStruct.nSessionType == DTWAIN_TWAINDSM_LATESTVERSION )
            return GetLatestDSMVersion();
        return pHandle->m_SessionStruct.DSMName;
    }
    return TWAINDLLVERSION_1;
}

CTL_StringType CTL_TwainAppMgr::GetLatestDSMVersion()
{
    bool bRet1 = CheckTwainExistence(TWAINDLLVERSION_1);
    bool bRet2 = CheckTwainExistence(TWAINDLLVERSION_2);

    if ( bRet2 )
        return TWAINDLLVERSION_2;
    if ( bRet1 )
        return TWAINDLLVERSION_1;
    return _T("");
}

bool CTL_TwainAppMgr::LoadSourceManager(  LPCTSTR pszDLLName/*=NULL */)
{
    if ( pszDLLName != NULL )
    {
        // This is a custom path, so user knows what they're doing.
        m_strTwainDLLName = pszDLLName;
        // Attempt to load TWAIN DLL
        boost::dll::shared_library libloader;
        boost::system::error_code ec;
        libloader.load(m_strTwainDLLName, ec); //::LoadLibrary(m_strTwainDLLName.c_str());
        if ( ec != boost::system::errc::success)
        {
            CTL_StringType dllName = _T(" : ") + m_strTwainDLLName;
            DTWAIN_ERROR_CONDITION_EX(IDS_ErrTwainDLLNotFound, dllName, false);
        }
    }
    else
    {
        // load the default TWAIN_32.DLL or TWAINDSM.DLL using the
        // normal process of finding these DLL's
        m_strTwainDLLName = GetTwainDirFullName(m_strTwainDLLName.c_str(), NULL, true, &m_hLibModule);
        if ( m_strTwainDLLName.empty() )
        {
            CTL_StringType dllName = _T(" : ") + m_strTwainDLLName;
            DTWAIN_ERROR_CONDITION_EX(IDS_ErrTwainDLLNotFound, dllName, false);
        }
        CTL_StringType msg = CTL_StringType(_T("TWAIN DSM ")) +
            CTL_StringType(_T("\"")) + m_strTwainDLLName + CTL_StringType(_T("\"")) +
            CTL_StringType(_T(" is found and will be used for this TWAIN session"));
        #ifdef _WIN32
        OutputDebugString(msg.c_str());
        #else
        std::cerr << msg.c_str();
        #endif
        if (CTL_TwainDLLHandle::s_lErrorFilterFlags != 0)
        {
            DTWAIN_LogMessage(msg.c_str());
        }
    }
    return LoadDSM();
}

bool CTL_TwainAppMgr::LoadDSM()
{
    m_lpDSMEntry = dtwain_library_loader<DSMENTRYPROC>::get_func_ptr(m_hLibModule.native(), "DSM_Entry");
    if ( !m_lpDSMEntry )
        DTWAIN_ERROR_CONDITION(IDS_ErrTwainDLLInvalid,false);
    CTL_TwainDLLHandle::s_nDSMState = DSM_STATE_LOADED;
    return true;   // return success
}

TW_UINT16 CTL_TwainAppMgr::CallDSMEntryProc( TW_IDENTITY *pOrigin, TW_IDENTITY* pDest,
                                             TW_UINT32 dg, TW_UINT16 dat, TW_UINT16 msg,
                                             TW_MEMREF pMemref)
{
    CTL_TwainTriplet::TwainTripletArgs theArgs = std::make_tuple(pOrigin, pDest, dg, dat, msg, pMemref);
    return s_pGlobalAppMgr->CallDSMEntryProcInternal( theArgs );
}

void CTL_TwainAppMgr::WriteLogInfoA(const CTL_String& s, bool bFlush)
{
    WriteLogInfo(StringConversion::Convert_Ansi_To_Native(s), bFlush);
}

void CTL_TwainAppMgr::WriteLogInfoW(const CTL_WString& s, bool bFlush)
{
    WriteLogInfo(StringConversion::Convert_Wide_To_Native(s), bFlush);
}

void CTL_TwainAppMgr::WriteLogInfo(const CTL_StringType& s, bool bFlush)
{
    if (!CTL_TwainDLLHandle::s_lErrorFilterFlags )
        return;

    CTL_StringType crlf;

    if  (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_USECRLF)
        crlf = _T("\n");

    // Always call the callback if callback exists
    if ( UserDefinedLoggerExists() )
    {
        CTL_StringType outStr = CTL_TwainDLLHandle::s_appLog.GetDebugStringFull(s) + crlf;
        WriteUserDefinedLogMsg(outStr.c_str());
    }

    if ( (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_WRITE) == 0 )
        return; // no need to process further

        CTL_TwainDLLHandle::s_appLog.StatusOutFast( s.c_str());
        if ( bFlush )
            CTL_TwainDLLHandle::s_appLog.Flush();
    }


TW_UINT16 CTL_TwainAppMgr::CallDSMEntryProc( CTL_TwainTriplet & pTriplet )
{
    if ( m_lpDSMEntry )
    {
        return CallDSMEntryProcInternal(pTriplet.GetTripletArgs());
    }
    return 0;
}


TW_UINT16 CTL_TwainAppMgr::CallDSMEntryProcInternal(CTL_TwainTriplet::TwainTripletArgs& tripletArgs)
{
    TW_UINT16 retcode;
    CTL_ErrorStruct e;
    CTL_StringType s;

    pTW_IDENTITY pOrigin = std::get<0>(tripletArgs);
    pTW_IDENTITY pDest   = std::get<1>(tripletArgs);
    TW_UINT32    nDG     = std::get<2>(tripletArgs);
    TW_UINT16    nDAT    = std::get<3>(tripletArgs);
    TW_UINT16    nMSG    = std::get<4>(tripletArgs);
    TW_MEMREF    pData   = std::get<5>(tripletArgs);

    if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
    {
        e = GetGeneralErrorInfo(nDG, nDAT, nMSG);
        s = e.GetIdentityAndDataInfo(pOrigin, pDest, pData);
        s = CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_INPUTTEXT] + _T(": ") + s;
        WriteLogInfo(s);
    }
    bool bTimeOutInEffect = false;
    #ifdef _WIN32
    if ( CTL_TwainDLLHandle::s_nTimeoutMilliseconds > 0 )
    {
        // Check if time out is to be applied to this triplet
        RawTwainTriplet rtrip;
        rtrip.nDAT = nDAT;
        rtrip.nDG = nDG;
        rtrip.nMSG = nMSG;
        if ( std::find_if(s_NoTimeoutTriplets.begin(),
                                        s_NoTimeoutTriplets.end(),
                                        FindTriplet(rtrip)) == s_NoTimeoutTriplets.end())
        {
            CTL_TwainDLLHandle::s_nTimeoutID = ::SetTimer(NULL, 0,
                CTL_TwainDLLHandle::s_nTimeoutMilliseconds, (TIMERPROC)CTL_TwainAppMgr::TwainTimeOutProc);
            bTimeOutInEffect = true;
        }
    }
    #endif
    try {
        retcode = (*m_lpDSMEntry)( pOrigin, pDest, nDG, nDAT, nMSG, pData );
    }
    catch(...)
    {
        // A memory exception occurred.  This is bad!
        // Check what to do when this happens (possibly close DSM and start over?)
        // To do later...
        #ifdef _WIN32
        if ( bTimeOutInEffect )
            ::KillTimer(NULL, CTL_TwainDLLHandle::s_nTimeoutID);
        #endif
        retcode = DTWAIN_ERR_EXCEPTION_ERROR_;
        if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
        {
            CTL_StringType sz;
            CTL_StringStreamType strm;
            sz = e.GetTWAINDSMErrorCC(IDS_TWCC_EXCEPTION);
            s = e.GetIdentityAndDataInfo(pOrigin, pDest, pData);
            #ifdef UNICODE
            strm << boost::wformat(_T("%1%=%2% (%3%)\n%4%")) %
                CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_OUTPUTDSMTEXT] % retcode % sz % s;
            #else
            strm << boost::format("%1%=%2% (%3%)\n%4%") %
                CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_OUTPUTDSMTEXT] % retcode % sz % s;
            #endif
            WriteLogInfo(strm.str());
        }
        return retcode;
    }
    #ifdef _WIN32
    if ( bTimeOutInEffect )
        ::KillTimer(NULL, CTL_TwainDLLHandle::s_nTimeoutID);
    #endif
    if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
    {
        CTL_StringType sz;
        CTL_StringStreamType strm;
        s = e.GetIdentityAndDataInfo(pOrigin, pDest, pData);
        sz = e.GetTWAINDSMError(retcode);
        CTL_StringType s1 = CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_OUTPUTDSMTEXT];
        #ifdef UNICODE
        boost::wformat fmt(_T("%1%=%2% (%3%)\n%4%\n"));
        #else
        boost::format fmt("%1%=%2% (%3%)\n%4%\n");
        #endif
        strm << fmt % s1.c_str() % retcode % sz % s;
        WriteLogInfo(strm.str());
    }
    if ( retcode != TWRC_SUCCESS )
        SetLastTwainError( retcode, TWRC_Error );
    return retcode;
}

void CTL_TwainAppMgr::SetLastTwainError( TW_UINT16 nError,
                                         int nErrorType )
{
    if ( nErrorType == TWRC_Error )
        m_nErrorTWRC = nError;
    else
        m_nErrorTWCC = nError;
}

bool CTL_TwainAppMgr::SetDefaultSource( CTL_ITwainSession *pSession, const CTL_ITwainSource *pSource )
{
    CTL_ITwainSource *pTemp = (CTL_ITwainSource *)pSource;
    CTL_SetDefaultSourceTriplet Trip(pSession, pTemp);
    TW_UINT16 rc = Trip.Execute();
    if ( rc != TWRC_SUCCESS )
    {
        TW_UINT16 ccode = GetConditionCode(pSession, pTemp);
        if ( ccode != TWCC_SUCCESS )
            CTL_TwainAppMgr::ProcessConditionCodeError(ccode);
        return false;
    }
    return true;
}

bool CTL_TwainAppMgr::SetDependentCaps( const CTL_ITwainSource *pSource, CTL_EnumCapability Cap )
{
    switch (Cap)
    {
        case ICAP_THRESHOLD:
        {
            // Must set ICAP_BITDEPTHREDUCTION
            LONG Val = TWBR_THRESHOLD;
            if (IsCapabilitySupported(pSource, ICAP_BITDEPTHREDUCTION))
            {
                return SetOneTwainCapValue( pSource, Val, CTL_SetTypeSET, TwainCap_BITDEPTHREDUCTION, TWTY_UINT16 );
            }
        }
        break;
    }
    return true;
}

VOID CALLBACK CTL_TwainAppMgr::TwainTimeOutProc(HWND, UINT, ULONG, DWORD)
{
#ifdef _WIN32
    ::KillTimer(NULL, CTL_TwainDLLHandle::s_nTimeoutID);

    WriteLogInfo(_T("The last TWAIN triplet was not completed due to time out"));
    SetError(DTWAIN_ERR_TIMEOUT);
    throw(DTWAIN_ERR_TIMEOUT);
#endif
}



///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////
// Global app pointer
CTL_TwainAppMgrPtr CTL_TwainAppMgr::s_pGlobalAppMgr;
// Initialize the m_AppID structure
TW_IDENTITY CTL_TwainAppMgr::s_AppId = {};
CTL_ITwainSession* CTL_TwainAppMgr::s_pSelectedSession=NULL;
int          CTL_TwainAppMgr::s_nLastError = 0;
CTL_StringType   CTL_TwainAppMgr::s_strLastError;
HINSTANCE    CTL_TwainAppMgr::s_ThisInstance = (HINSTANCE)0;
mapCondCodeInfo  CTL_TwainAppMgr::s_mapCondCode;
std::vector<RawTwainTriplet> CTL_TwainAppMgr::s_NoTimeoutTriplets;


