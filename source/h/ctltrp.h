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
#ifndef CTLTRP_H_
#define CTLTRP_H_

#include "ctltwain.h"
#include "ctltwses.h"
#include "tr1defs.h"
#include "ctlobstr.h"

namespace dynarithmic
{
    struct TwainUtils
    {
        static bool IsTwainStringType(TW_UINT16 nItemType)
        {
            return nItemType == TWTY_STR32 ||
                   nItemType == TWTY_STR64 ||
                   nItemType == TWTY_STR128 ||
                   nItemType == TWTY_STR255 ||
                   nItemType == TWTY_STR1024 ;
        }
    };

    class CTL_TwainTriplet
    {
    public:
        enum {DGPOS_ = 2, DATPOS_ = 3, MSGPOS_ = 4, MEMREFPOS_ = 5};

            CTL_TwainTriplet();
            CTL_TwainTriplet( pTW_IDENTITY pOrigin,
                              pTW_IDENTITY pDest,
                              TW_UINT32    DG,
                              TW_UINT16    DAT,
                              TW_UINT16    MSG,
                              TW_MEMREF    pData);

            void Init( pTW_IDENTITY pOrigin,
                       pTW_IDENTITY pDest,
                       TW_UINT32    nDG,
                       TW_UINT16    nDAT,
                       TW_UINT16    nMSG,
                       TW_MEMREF    pData);

            typedef std::tuple<pTW_IDENTITY, pTW_IDENTITY, TW_UINT32,
                                 TW_UINT16, TW_UINT16, TW_MEMREF> TwainTripletArgs;

            const TwainTripletArgs& GetTripletArgs() const { return m_TwainTripletArg; }
            TwainTripletArgs& GetTripletArgs() { return m_TwainTripletArg; }

            virtual ~CTL_TwainTriplet() {}
            virtual TW_UINT16 Execute();
            void    SetAlive( bool bSet );
            bool    IsAlive() const;
            bool    IsMSGGetType() const;
            bool    IsMSGSetType() const;

        protected:
            CTL_ITwainSession* GetSessionPtr()
            { return m_pSession; }

            void SetSessionPtr(CTL_ITwainSession* pSession)
            { m_pSession = pSession; }

            CTL_ITwainSource*  GetSourcePtr()
            { return m_pSource; }

            void SetSourcePtr(CTL_ITwainSource* pSource)
            { m_pSource = pSource; }

        private:
            TwainTripletArgs m_TwainTripletArg;
            bool                    m_bInit;
            bool                    m_bAlive;
            CTL_ITwainSource*       m_pSource;
            CTL_ITwainSession*      m_pSession;
    };
}
#endif

