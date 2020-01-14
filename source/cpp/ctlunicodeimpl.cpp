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
#include "dtwain.h"
#include "ctlobstr.h"
#include "ctliface.h"
#include "enumeratorfuncs.h"
#include "twain.h"

using namespace dynarithmic;

struct ArrayRAII
{
    DTWAIN_ARRAY theArray;
    ArrayRAII(DTWAIN_ARRAY arr) : theArray(arr) {}
    ~ArrayRAII(){ DTWAIN_ArrayDestroy(theArray); }
};


template <typename SourceString,
          typename DestString,
          typename fn,
          CTL_EnumeratorType enumTypeIn,
          CTL_EnumeratorType enumTypeOut
         >
static DTWAIN_ARRAY CreateArrayFromArray(LPVOID ArraySource, fn f)
{
    if ( ArraySource )
    {
        CTL_EnumeratorType eType = EnumeratorFunctionImpl::GetEnumeratorType(ArraySource);
        if ( eType != enumTypeIn )
            return NULL;
        int status;
        LPVOID TempSource = EnumeratorFunctionImpl::GetNewEnumerator(enumTypeOut,&status, 0, 0);
        if ( TempSource )
        {
            LONG nItems = EnumeratorFunctionImpl::EnumeratorGetCount(ArraySource);
            SourceString sVal;
            DestString sVal2;
            for (LONG i = 0; i < nItems; ++i )
            {
                EnumeratorFunctionImpl::EnumeratorGetAt(ArraySource, i, &sVal);
                sVal2 = f(sVal);
                EnumeratorFunctionImpl::EnumeratorAddValue(TempSource, &sVal2, 1);
            }
            return TempSource;
        }
    }
    return NULL;
}


#ifdef UNICODE
    #pragma message ("Creating UNICODE version of DTWAIN functions")
#else
    #pragma message ("Creating MBCS version of DTWAIN functions")
#endif

#include "ctlstrimpl.inl"
