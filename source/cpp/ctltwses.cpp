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
#include <cstring>
#include <algorithm>
#include "ctltwses.h"
#include "ctltrall.h"
#include "ctltwmgr.h"
#include "ctltwsrc.h"

using namespace std;
using namespace dynarithmic;

//////////////////// CTL_ITwainSession functions /////////////////////////////
CTL_TwainSession::CTL_TwainSession( CTL_ITwainSession *pSession/*=NULL*/)
{
    m_pSession = pSession;
}


CTL_TwainSession::CTL_TwainSession( CTL_TwainSession & SObject )
{
    SetEqual( SObject );
}

void CTL_TwainSession::operator = (CTL_TwainSession& SObject)
{
    if ( &SObject == this )
        return;
    SetEqual( SObject);
}


void CTL_TwainSession::SetEqual(CTL_TwainSession & SObject)
{
    m_pSession = SObject.m_pSession;
}

CTL_ITwainSession * CTL_ITwainSession::Create(LPCTSTR pAppName,
                                            HWND* hAppWnd,
                                            TW_UINT16 nMajorNum,
                                            TW_UINT16 nMinorNum,
                                            CTL_TwainLanguageEnum nLanguage,
                                            CTL_TwainCountryEnum nCountry,
                                            LPCTSTR lpszVersion,
                                            LPCTSTR lpszMfg,
                                            LPCTSTR lpszFamily,
                                            LPCTSTR lpszProduct
                                            )
{
    CTL_ITwainSession *pSession =  new CTL_ITwainSession( pAppName, hAppWnd,
                                 nMajorNum,
                                 nMinorNum, nLanguage, nCountry,
                                 lpszVersion, lpszMfg,
                                 lpszFamily, lpszProduct);
    return pSession;
}


CTL_ITwainSession::CTL_ITwainSession(LPCTSTR pAppName,
                                   HWND* hAppWnd,
                                   TW_UINT16 nMajorNum,
                                   TW_UINT16 nMinorNum,
                                   CTL_TwainLanguageEnum nLanguage,
                                   CTL_TwainCountryEnum nCountry,
                                   LPCTSTR lpszVersion,
                                   LPCTSTR lpszMfg,
                                   LPCTSTR lpszFamily,
                                   LPCTSTR lpszProduct
                                   )
{
    if ( pAppName )
        m_AppName = pAppName;
    if ( hAppWnd )
        m_AppWnd = *hAppWnd;
    m_bTwainWindowCreated = false;

    if ( !hAppWnd )
    {
        m_AppWnd = CreateTwainWindow();
        m_bTwainWindowCreated = true;
    }

    memset( &m_AppId, 0, sizeof(TW_IDENTITY) );

    m_AppId.Id = 0;
    m_AppId.Version.MajorNum = nMajorNum;
    m_AppId.Version.MinorNum = nMinorNum;
    m_AppId.Version.Language = (TW_UINT16)nLanguage;
    m_AppId.Version.Country  = (TW_UINT16)nCountry;


    StringWrapperA::SafeStrcpy( m_AppId.Version.Info,
                                StringConversion::Convert_Native_To_Ansi(lpszVersion).c_str(),
                               sizeof( m_AppId.Version.Info ) - 1 );

    m_AppId.ProtocolMajor =    TWON_PROTOCOLMAJOR;
    m_AppId.ProtocolMinor =    TWON_PROTOCOLMINOR;
    m_AppId.SupportedGroups =  DG_IMAGE | DG_CONTROL | DG_AUDIO | DF_APP2 | DF_DSM2 ;

    StringWrapperA::SafeStrcpy( m_AppId.Manufacturer,  StringConversion::Convert_Native_To_Ansi(lpszMfg).c_str(),
                               sizeof( m_AppId.Manufacturer ) - 1 );

    StringWrapperA::SafeStrcpy( m_AppId.ProductFamily,  StringConversion::Convert_Native_To_Ansi(lpszFamily).c_str(),
                                sizeof( m_AppId.ProductFamily ) - 1 );

    StringWrapperA::SafeStrcpy( m_AppId.ProductName,  StringConversion::Convert_Native_To_Ansi(lpszProduct).c_str(),
                                sizeof( m_AppId.ProductName ) - 1 );
    m_pSelectedSource = nullptr;
    m_bTwainMessageFlag = false;
    m_bAllSourcesRetrieved = false;
}

CTL_ITwainSource* CTL_ITwainSession::CreateTwainSource( LPCTSTR pProduct )
{
    CTL_ITwainSource* pSource = nullptr;
    // check if source with this product name has been selected
    pSource = IsSourceSelected(pProduct);
    if (!pSource )
    {
        pSource = CTL_ITwainSource::Create( this, pProduct );
        m_arrTwainSource.insert( pSource );
    }
    return pSource;
}


HWND CTL_ITwainSession::CreateTwainWindow()
{
#ifdef _WIN32
    HWND hwnd;
    hwnd = CreateWindow(_T("STATIC"),                       // class
                        _T("Twain Window"),             // title
                        WS_POPUPWINDOW | WS_VISIBLE,    // style
                        CW_USEDEFAULT, CW_USEDEFAULT,   // x, y
                        CW_USEDEFAULT, CW_USEDEFAULT,   // width, height
                        HWND_DESKTOP,                   // parent window
                        NULL,                           // hmenu
                        CTL_TwainAppMgr::GetAppInstance(),    // hinst
                        NULL);                          // lpvparam
    return hwnd;
#else
    return 0;
#endif
}

void CTL_ITwainSession::Destroy( CTL_ITwainSession *pSession )
{
        delete pSession;
}


bool CTL_ITwainSession::IsTwainWindowActive() const
{
    return m_bTwainWindowCreated;
}

void CTL_ITwainSession::SetTwainMessageFlag(bool bSet)
{
    m_bTwainMessageFlag = bSet;
}


bool CTL_ITwainSession::AddTwainSource( CTL_ITwainSource *pSource )
{
    TW_IDENTITY *pId;
    pId = pSource->GetSourceIDPtr();
    CTL_StringType strProduct = StringConversion::Convert_AnsiPtr_To_Native(pId->ProductName);

    struct SourceFinder
    {
        CTL_StringType m_str;
        SourceFinder(const CTL_StringType str) : m_str(str) {}
        bool operator()(CTL_ITwainSource* ptr) const
            { return StringConversion::Convert_AnsiPtr_To_Native(ptr->GetSourceIDPtr()->ProductName) == m_str; }
    };

    if ( std::find_if(m_arrTwainSource.begin(), m_arrTwainSource.end(), SourceFinder(strProduct)) == m_arrTwainSource.end())
        {
            m_arrTwainSource.insert( pSource );
            return true;
        }
    return false;
}

bool CTL_ITwainSession::IsValidSource( CTL_ITwainSource *pSource )
{
    if ( find(m_arrTwainSource.begin(),
              m_arrTwainSource.end(),
              pSource) == m_arrTwainSource.end())
        return false;
    return true;
}

bool CTL_ITwainSession::SelectSource( const CTL_ITwainSource* pSource )
{
    if ( !pSource )  // Choose the default source
    {
        CTL_ITwainSource *pSourceTemp;

        // Get first source
        CTL_GetDefaultSourceTriplet ST( this );
        if ( ST.Execute() == TWRC_SUCCESS )
        {
            pSourceTemp = ST.GetSourceIDPtr();
            m_pSelectedSource = IsSourceSelected( pSourceTemp->GetProductName().c_str() );
            CTL_ITwainSource::Destroy( pSourceTemp );
        }
        else
            return false;
    }
    else
    {
        // Select the source given by pSource
        if ( !IsSourceSelected( pSource->GetProductName().c_str() ))
        {
            return false;
        }
        m_pSelectedSource = (CTL_ITwainSource*)pSource;
        m_pSelectedSource->SetTwainVersion2((m_pSelectedSource->GetSourceIDPtr()->SupportedGroups & DF_DS2) ? true : false);
    }
    return true;
}

bool CTL_ITwainSession::SelectSource( LPCTSTR strName )
{
    if ( m_arrTwainSource.empty() )
        EnumSources();
    CTL_ITwainSource* pSource =  IsSourceSelected( strName );
    if ( pSource )
        return SelectSource( pSource );
    return false;
}


bool CTL_ITwainSession::OpenSource( const CTL_ITwainSource* pSource )
{
    CTL_ITwainSource *pTemp;
    // Open the source
    if ( !pSource )
        pTemp = m_pSelectedSource;
    else
        pTemp = (CTL_ITwainSource* )pSource;

    if ( !pTemp )
        return false;

    if ( !IsSourceSelected( pTemp->GetProductName().c_str()) )
        return false;

    if ( !pTemp->IsOpened() )
    {
        // see if this is a DS 2.x source
        pTemp->SetTwainVersion2((pTemp->GetSupportedGroups() & DF_DS2) ? true : false);

        // Not opened, so open it.
        CTL_OpenSourceTriplet ST( this, pTemp );
        if ( ST.Execute() != TWRC_SUCCESS )
        {
            TW_UINT16 cc = CTL_TwainAppMgr::GetConditionCode( this, NULL );
            CTL_TwainAppMgr::ProcessConditionCodeError(cc);
            return false;
        }
        pTemp->SetState(SOURCE_STATE_OPENED);
        pTemp->SetOpenFlag(true);
    }

    // Make this the selected source
    m_pSelectedSource = pTemp;

    return true;
}


bool CTL_ITwainSession::CloseSource( const CTL_ITwainSource* pSource, bool bForce )
{
    CTL_ITwainSource *pTemp;

    // Close the source
    if ( !pSource )
        pTemp = m_pSelectedSource;
    else
        pTemp = (CTL_ITwainSource* )pSource;
    pTemp->CloseSource(bForce);
    pTemp->SetOpenFlag(false);
    m_pSelectedSource = NULL;
    return true;
}

CTL_ITwainSession::~CTL_ITwainSession()
{
    DestroyAllSources();
    // Destroy proxy window if it was created
    DestroyTwainWindow();
}


void CTL_ITwainSession::DestroyTwainWindow()
{
#ifdef _WIN32
    if ( m_bTwainWindowCreated )
    {
        ::DestroyWindow( m_AppWnd );
        m_bTwainWindowCreated = false;
    }
#endif
}

void CTL_ITwainSession::DestroyOneSource(CTL_ITwainSource *pSource)
{
    CTL_TwainSourceArray::iterator found;
    found = find(m_arrTwainSource.begin(),
              m_arrTwainSource.end(),
              pSource);
    if ( found != m_arrTwainSource.end())
    {
        CTL_ITwainSource::Destroy( pSource );
        m_arrTwainSource.erase(found);
    }
}

void CTL_ITwainSession::DestroyAllSources()
{
    std::for_each(m_arrTwainSource.begin(), m_arrTwainSource.end(), CTL_ITwainSource::Destroy);
    m_arrTwainSource.clear();
    m_pSelectedSource = NULL;
}

void CTL_ITwainSession::EnumSources()
{
    // Get first source
    CTL_GetFirstSourceTriplet ST1( this );
    if ( ST1.Execute() == TWRC_SUCCESS )
    {
        CTL_ITwainSource *pSource = ST1.GetSourceIDPtr();
        if ( !AddTwainSource( pSource ) )
            CTL_ITwainSource::Destroy( pSource );
    }
    else
    {
        // Get the condition code
        TW_UINT16 cc = CTL_TwainAppMgr::GetConditionCode( this, NULL );
        CTL_TwainAppMgr::ProcessConditionCodeError(cc);
        CTL_ITwainSource::Destroy( ST1.GetSourceIDPtr() );
        return;
    }
    // Get next sources
    CTL_ITwainSource *pSource;
    while ( true )
    {
        CTL_GetNextSourceTriplet STn( this );
        pSource = STn.GetSourceIDPtr();

        if ( STn.Execute() == TWRC_SUCCESS )
        {
            if ( !AddTwainSource( pSource ) )
                CTL_ITwainSource::Destroy( pSource );
        }
        else
        {
            CTL_ITwainSource::Destroy( pSource );
            break;
        }
    }
}

void CTL_ITwainSession::CopyAllSources( CTL_TwainSourceArray & rArray )
{
    GetNumSources();
    rArray = m_arrTwainSource;
}

int CTL_ITwainSession::GetNumSources()
{
    if ( m_arrTwainSource.empty() || !m_bAllSourcesRetrieved )
        EnumSources();
    m_bAllSourcesRetrieved = true;
    return (int)m_arrTwainSource.size();
}

CTL_ITwainSource* CTL_ITwainSession::GetSelectedSource()
{
    return m_pSelectedSource;
}


void CTL_ITwainSession::SetSelectedSource(CTL_ITwainSource* pSource)
{
    m_pSelectedSource = pSource;
}

CTL_ITwainSource* CTL_ITwainSession::Find( CTL_ITwainSource* pSource )
{
    return IsSourceSelected( pSource->GetProductName().c_str());
}

CTL_ITwainSource* CTL_ITwainSession::IsSourceSelected( LPCTSTR pSourceName )
{
    struct ProductNameFinder
    {
        CTL_StringType m_strProduct;
        ProductNameFinder(CTL_StringType& s) : m_strProduct(s) {}
        bool operator()(CTL_ITwainSource* pSource)
        {
            TW_IDENTITY* pIdentity =  pSource->GetSourceIDPtr();
            CTL_StringType strTemp = StringConversion::Convert_AnsiPtr_To_Native(pIdentity->ProductName);
            StringWrapper::MakeUpperCase(StringWrapper::TrimAll(strTemp));
            return strTemp == m_strProduct;
        }
    };

    CTL_StringType strProduct;
    if ( pSourceName )
        strProduct = pSourceName;
    strProduct = StringWrapper::TrimAll(StringWrapper::MakeUpperCase(strProduct));
    CTL_TwainSourceArray::iterator it =
        std::find_if(m_arrTwainSource.begin(), m_arrTwainSource.end(), ProductNameFinder(strProduct));
    if ( it != m_arrTwainSource.end())
        return (*it);
    return NULL;
}

CTL_ITwainSource* CTL_ITwainSession::GetDefaultSource()
{
    // Get first source
    CTL_GetDefaultSourceTriplet ST( this );
    if ( ST.Execute() == TWRC_SUCCESS )
    {
        m_arrTwainSource.insert(ST.GetSourceIDPtr());
        return  ST.GetSourceIDPtr();
    }
    else
    {
        // Get the condition code
        TW_UINT16 cc = CTL_TwainAppMgr::GetConditionCode(this, NULL);
        CTL_TwainAppMgr::ProcessConditionCodeError(cc);
    }
    return NULL;
}
