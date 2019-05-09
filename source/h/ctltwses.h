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
#ifndef CTLTWSES_H_
#define CTLTWSES_H_
#include <vector>
#include "ctlobstr.h"
#include "ctlenum.h"
#include "ctltwsrc.h"
#include "dtwdecl.h"

namespace dynarithmic
{
  class CTL_ITwainSession
  {
    public:
        static CTL_ITwainSession *Create(LPCTSTR pAppName,
                                        HWND* hAppWnd,
                                        TW_UINT16 nMajorNum,
                                        TW_UINT16 nMinorNum,
                                        CTL_TwainLanguageEnum nLanguage,
                                        CTL_TwainCountryEnum nCountry,
                                        LPCTSTR lpszVersion,
                                        LPCTSTR lpszMfg,
                                        LPCTSTR lpszFamily,
                                        LPCTSTR lpszProduct
                                        );
        static void Destroy( CTL_ITwainSession *pSession );

        HWND*               GetWindowHandlePtr() const { return (HWND* )&m_AppWnd; }
        TW_IDENTITY*        GetAppIDPtr()              { return &m_AppId; }
        CTL_ITwainSource*    CreateTwainSource( LPCTSTR pProduct );
        bool                AddTwainSource( CTL_ITwainSource *pSource );
        void                CopyAllSources( CTL_TwainSourceArray & rArray );
        int                 GetNumSources();
        void                EnumSources();
        bool                SelectSource(const CTL_ITwainSource* pSource);
        bool                SelectSource(LPCTSTR strName);
        bool                OpenSource(const CTL_ITwainSource* pSource);
        bool                CloseSource(const CTL_ITwainSource* pSource,
                                        bool bForce );
        CTL_ITwainSource*   GetSelectedSource();
        CTL_ITwainSource*   Find( CTL_ITwainSource* pSource );
        void                SetSelectedSource(CTL_ITwainSource* pSource);
        CTL_ITwainSource*   GetDefaultSource();
        bool                IsValidSource(CTL_ITwainSource *pSource);
        void                SetTwainMessageFlag(bool bSet = true);
        bool                IsTwainMsgOn() const
                                    { return m_bTwainMessageFlag; }
        bool                IsAllSourcesRetrieved() const { return m_bAllSourcesRetrieved; }
        void                DestroyOneSource(CTL_ITwainSource *pSource);

    protected:
        CTL_ITwainSession( LPCTSTR pszAppName,
                          HWND* hAppWnd,
                          TW_UINT16 nMajorNum,
                          TW_UINT16 nMinorNum,
                          CTL_TwainLanguageEnum nLanguage,
                          CTL_TwainCountryEnum nCountry,
                          LPCTSTR lpszVersion,
                          LPCTSTR lpszMfg,
                          LPCTSTR lpszFamily,
                          LPCTSTR lpszProduct
                        );
        virtual ~CTL_ITwainSession();
        CTL_ITwainSource* IsSourceSelected( CTL_ITwainSource* pSource/*,
                                           int *pWhere=NULL */);
        CTL_ITwainSource* IsSourceSelected( LPCTSTR pPsourceName/*,
                                           int *pWhere=NULL */);
        void            DestroyAllSources();
        HWND            CreateTwainWindow();
        bool            IsTwainWindowActive() const;
        void            DestroyTwainWindow();

    private:
        bool       m_bAllSourcesRetrieved;
        HWND       m_AppWnd;
        CTL_StringType  m_AppName;
        TW_IDENTITY m_AppId;          // Twain Identity structure
        CTL_TwainSourceArray m_arrTwainSource;
        CTL_ITwainSource *m_pSelectedSource;
        bool        m_bTwainWindowCreated;
        bool        m_bTwainMessageFlag;
};

typedef std::vector< CTL_ITwainSession *> CTL_TwainSessionArray;

class CTL_TwainSession
{
    public:
        CTL_TwainSession( CTL_ITwainSession *pSession=NULL);
        CTL_TwainSession( CTL_TwainSession & SObject );
        void operator = (CTL_TwainSession& SessionObject);

        operator CTL_ITwainSession*() { return m_pSession; }

    private:
        CTL_ITwainSession* m_pSession;
        void SetEqual(CTL_TwainSession & SObject);
};
}
#endif
