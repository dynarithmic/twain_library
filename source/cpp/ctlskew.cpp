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

    For more information, the license file LICENSE.TXT that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#include <string.h>
#include <dtwain.h>
#include <ctliface.h>
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "ctlsupport.h"

using namespace dynarithmic;

static BOOL Enable1(DTWAIN_SOURCE Source, LONG Cap, LONG Setting=FALSE);
static BOOL Enable2(DTWAIN_SOURCE Source, LONG Cap, LONG Setting=FALSE);
static BOOL IsEnabled(DTWAIN_SOURCE Source, LONG Cap);

/////////////////////////////// Deskew //////////////////////////////////////
/// Overscan ////
BOOL Enable1(DTWAIN_SOURCE Source, LONG Cap, LONG Setting/*=FALSE*/)
{
    if ( !DTWAIN_IsCapSupported(Source, Cap) )
        return FALSE;

    // Enable the ICAP_UNDEFINEDIMAGESIZE if available
    if ( DTWAIN_IsCapSupported(Source, DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE))
        Enable2(Source, DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE, Setting);

    return (Enable2(Source, Cap, Setting));
}


BOOL Enable2(DTWAIN_SOURCE Source, LONG Cap, LONG Setting/*=FALSE*/)
{
    DTWAIN_ARRAY Array = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, 1);
    if ( !Array )
        return FALSE;
    DTWAINArrayLL_RAII a(Array);
    EnumeratorFunctionImpl::EnumeratorSetAt(Array, 0, &Setting);
    return DTWAIN_SetCapValues(Source, Cap, DTWAIN_CAPSET, Array );
}

BOOL IsEnabled(DTWAIN_SOURCE Source, LONG Cap)
{
    if (!DTWAIN_IsCapSupported(Source, Cap))
        return FALSE;

    LONG bEnable;
    DTWAIN_ARRAY Array;
    DTWAINArrayPtr_RAII raii(&Array);
    DTWAIN_BOOL bRet;
    bRet = DTWAIN_GetCapValues(Source, Cap, DTWAIN_CAPGET, &Array );
    if (bRet && EnumeratorFunctionImpl::EnumeratorGetCount(Array) >= 1)
    {
        EnumeratorFunctionImpl::EnumeratorGetAt(Array, 0, &bEnable);
        return bEnable;
    }
    return FALSE;
}
