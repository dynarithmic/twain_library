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
#include <cstring>
#include "dtwain.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
using namespace dynarithmic;

/* Duplex Scanner support */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDuplexType(DTWAIN_SOURCE Source, LPLONG lpDupType)
{
    LOG_FUNC_ENTRY_PARAMS((Source, lpDupType))
    bool bRet = true;
    if ( !DTWAIN_IsCapSupported(Source, DTWAIN_CV_CAPDUPLEX))
    {
        if ( lpDupType )
            lpDupType = DTWAIN_DX_NONE;
        bRet = false;
    }
    else
    {
        if ( lpDupType )
        {
            bool bRet2 = true;
            DTWAIN_ARRAY Array = 0;
            bRet2 = DTWAIN_GetCapValues(Source, DTWAIN_CV_CAPDUPLEX, DTWAIN_CAPGET, &Array) ? true : false;
            DTWAINArrayLL_RAII arr(Array);
            if ( bRet2 && Array)
            {
                auto vValues = EnumeratorVectorPtr<LONG>(Array);
                if ( vValues && !vValues->empty() )
                    *lpDupType = (*vValues)[0];
            }
        }
    }
    LOG_FUNC_EXIT_PARAMS(bRet);
    CATCH_BLOCK(false)
}


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsDuplexSupported(DTWAIN_SOURCE Source)
{
    LOG_FUNC_ENTRY_PARAMS((Source))
    LONG DupType;
    bool bRet = false;
    if ( DTWAIN_GetDuplexType(Source, &DupType) )
    {
        if ( DupType == TWDX_1PASSDUPLEX ||
             DupType == TWDX_2PASSDUPLEX )
            bRet = true;
    }
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(false)
}
