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
#include "ctldib.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "ctlfileutils.h"

using namespace std;
using namespace dynarithmic;

int CTL_IcoIOHandler::WriteBitmap(LPCTSTR szFile, bool /*bOpenFile*/, int /*fhFile*/, LONG64 /*MultiStage*/)
{
    HANDLE hDib = NULL;

    if ( !m_pDib )
        return DTWAIN_ERR_DIB;

    hDib = m_pDib->GetHandle();
    if ( !hDib )
        return DTWAIN_ERR_DIB;

    int height = m_pDib->GetHeight();
    int width = m_pDib->GetWidth();

    if ( !m_ImageInfoEx.IsVistaIcon )
    {
        if ( height > 255 || width > 255 )
            return DTWAIN_ERR_INVALIDICONFORMAT;
    }
	if (!parent_directory_exists(szFile))
		return DTWAIN_ERR_FILEOPEN;
    return SaveToFile(hDib, szFile, FIF_ICO, 0, DTWAIN_INCHES, { 0,0 });
}
