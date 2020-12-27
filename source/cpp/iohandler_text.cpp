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
#include "ctldib.h"
#include "ctliface.h"
#include "ctltwmgr.h"

using namespace dynarithmic;

// Text routines
CTL_TextIOHandler::CTL_TextIOHandler(CTL_TwainDib* pDib, int nInputFormat, DTWAINImageInfoEx &ImageInfoEx,
                                     OCREngine *pEngine)
                                     :
CTL_ImageIOHandler( pDib ), m_nInputFormat(nInputFormat),
                            m_ImageInfoEx(ImageInfoEx),
m_pOCREngine(pEngine)
{ }

CTL_TextIOHandler::~CTL_TextIOHandler()
{}


int CTL_TextIOHandler::WriteBitmap(LPCTSTR szFile, bool /*bOpenFile*/, int /*fhFile*/, LONG64 MultiStage)
{
    DibMultiPageStruct *s = (DibMultiPageStruct *)MultiStage;
    HANDLE hDib = NULL;

    if ( !s || s->Stage != DIB_MULTI_LAST )
    {
        if ( !m_pDib )
            return DTWAIN_ERR_DIB;

        hDib = m_pDib->GetHandle();
        if ( !hDib )
            return DTWAIN_ERR_DIB;
    }

    CTextImageHandler TextHandler(m_ImageInfoEx, m_pOCREngine, m_nInputFormat, m_pDib);
    if ( MultiStage )
    {
        TextHandler.SetMultiPageStatus(s);
    }

    int retval;
    if ( !s || s->Stage != DIB_MULTI_LAST )
        retval = TextHandler.WriteGraphicFile(this, szFile, hDib);
    else
        retval = TextHandler.WriteImage(NULL,0,0,0,0,0,NULL);
    if ( s )
        TextHandler.GetMultiPageStatus(s);
    return retval;
}

