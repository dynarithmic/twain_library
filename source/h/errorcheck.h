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

    For more information, the license file named LICENSE that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef ERRORCHECK_H
#define ERRORCHECK_H

#include "ctliface.h"
#include "ctlobstr.h"
#include "ctltwmgr.h"

namespace dynarithmic
{
    template <typename Func, typename RetType, bool doThrow>
    void DTWAIN_Check_Error_Condition_0_Impl(CTL_TwainDLLHandle* Handle,
                                            Func f,
                                            int32_t Err,
                                            RetType retErr,
                                            const CTL_String::value_type* fnName,
                                            bool logError=true)
    {
        Handle->m_lLastError = 0;
        bool bRet = f() ? true : false;
        if (bRet)
        {
            Handle->m_lLastError = Err;
            CTL_TwainAppMgr::SetError(Err);
            OutputDTWAINErrorA(Handle, fnName);
            if (logError && (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK))
            {
                CTL_TwainAppMgr::WriteLogInfo(CTL_LogFunctionCallA(fnName, LOG_INDENT_OUT) +
                    ParamOutputter(_T(""), true).outputParam(retErr).getString());
            }
            if (doThrow)
            throw retErr;
        }
    }

    template <typename Func, typename RetType>
    void DTWAIN_Check_Error_Condition_0_Ex(CTL_TwainDLLHandle* Handle,
                                            Func f,
                                            int32_t Err,
                                            RetType retErr,
                                            const CTL_String::value_type* fnName,
                                            bool logError=true)
    {
        DTWAIN_Check_Error_Condition_0_Impl<Func,RetType,true>(Handle,f,Err,retErr,fnName,logError);
    }

    template <typename Func, typename RetType>
    void DTWAIN_Check_Error_Condition_1_Ex(CTL_TwainDLLHandle* Handle,
                                            Func f,
                                            int32_t Err,
                                            RetType retErr,
                                            const CTL_String::value_type* fnName)
    { DTWAIN_Check_Error_Condition_0_Ex(Handle, f, Err, retErr, fnName, false); }

    template <typename Func, typename RetType>
    void DTWAIN_Check_Error_Condition_2_Ex(CTL_TwainDLLHandle* Handle,Func f,int32_t Err,RetType retErr,
                                           const CTL_String::value_type* fnName,bool logError = true)
    {
        DTWAIN_Check_Error_Condition_0_Impl<Func, RetType, false>(Handle, f, Err, retErr, fnName, logError);
    }

    template <typename RetType>
    void DTWAIN_Check_Bad_Handle_Ex(CTL_TwainDLLHandle* pHandle, RetType retErr, const CTL_String::value_type* fnName)
    {
        if (CTL_TwainDLLHandle::s_bCheckHandles && !IsDLLHandleValid(pHandle, false))
        {
            OutputDTWAINErrorA(NULL, fnName);
            throw retErr;
        }
    }
}
#endif
