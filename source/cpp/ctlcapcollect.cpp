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

    For more information, the license file named LICENSE that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "ctltmpl5.h"
#include "ctliface.h"
#include <boost/assign/list_of.hpp>
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
#undef min
#undef max
using namespace std;
using namespace dynarithmic;
using namespace boost::assign; // bring 'map_list_of()' into scope

#define DTWAIN_STATE4   8

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetQueryCapSupport(DTWAIN_BOOL bSet)
{
    LOG_FUNC_ENTRY_PARAMS((bSet))
    CTL_TwainDLLHandle::s_bQuerySupport = bSet ? true : false;
    LOG_FUNC_EXIT_PARAMS(true)
    CATCH_BLOCK(false)
}

void dynarithmic::DTWAIN_CollectCapabilityInfo(CTL_ITwainSource *p, TW_UINT16 nCap, CTL_CapInfoArray& pArray)
{
    static const ContainerMap mapContainer = { {"TW_ONEVALUE", DTWAIN_CONTONEVALUE},
                                                {"TW_ENUMERATION", DTWAIN_CONTENUMERATION},
                                                {"TW_RANGE", DTWAIN_CONTRANGE},
                                                {"TW_ARRAY", DTWAIN_CONTARRAY},
                                                {"-1", (UINT)-1},
                                                {"0", 0} };

    // Add capabilities where the state info is set
    // Test the capability and see which container works.
    UINT cSet;

    UINT cGet;
    UINT cGetCurrent;
    UINT cGetDefault;
    UINT cSetCurrent;
    UINT cSetAvailable;
    UINT cQuerySupport;
    UINT cEOJValue;
    UINT cEntryFound;

    bool cFlags[6] = { false };

    UINT cSetAlt = 0;
    TW_UINT16 cStateInfo = 0xFF;
    UINT nDataType = 0;
    CTL_String strName = CTL_TwainAppMgr::GetCapNameFromCap(nCap);
    bool bOk;
    bool bCanQuerySupport;
    bool bContainerInfoFound = false;
    cEOJValue = 0;
    bOk = GetCapInfoFromIni(strName,
        StringConversion::Convert_Native_To_Ansi(p->GetProductName()),
        (UINT)nCap,
        cGet,
        cGetCurrent,
        cGetDefault,
        cSetCurrent,
        cSetAvailable,
        cQuerySupport,
        cEOJValue,
        cStateInfo,
        cEntryFound,
        nDataType,
        bContainerInfoFound,
        mapContainer);

    bCanQuerySupport = cQuerySupport ? true : false;
    if (cEntryFound)
    {
        if (cStateInfo != 0xFF)
            p->AddCapToStateInfo(nCap, cStateInfo);
    }
    if (bOk)
    {
        if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
        {
            CTL_StringStreamA strm;
            strm << "Using capability info from DTWAIN32.INI (Source="
                 << StringConversion::Convert_Native_To_Ansi(p->GetProductName())
                 << ", Cap=" << CTL_TwainAppMgr::GetCapNameFromCap(nCap) << ")\n";
            CTL_TwainAppMgr::WriteLogInfoA(strm.str());
        }
        if (bContainerInfoFound)
        {
            // set the flags here
            if (cGet != std::numeric_limits<UINT>::max())
                cFlags[0] = true;
            if (cGetCurrent != std::numeric_limits<UINT>::max())
                cFlags[1] = true;
            if (cGetDefault != std::numeric_limits<UINT>::max())
                cFlags[2] = true;
            if (cSetCurrent != std::numeric_limits<UINT>::max())
                cFlags[3] = true;
            if (cSetAvailable != std::numeric_limits<UINT>::max())
                cFlags[4] = true;
            if (nDataType != std::numeric_limits<UINT>::max())
                cFlags[5] = true;
        }

        // Get the best data type here;
        if (nDataType == std::numeric_limits<UINT>::max())
            CTL_TwainAppMgr::GetBestCapDataType(p, nCap, nDataType);
    }

    // Get the container that works the best
    CTL_TwainAppMgr::GetBestContainerType(p,
        (CTL_EnumCapability)nCap,
        cGet, cSet, nDataType,
        CTL_GetTypeGET, cFlags);

    CTL_TwainAppMgr::GetBestContainerType(p,
        (CTL_EnumCapability)nCap,
        cGetCurrent, cSetAlt, nDataType,
        CTL_GetTypeGETCURRENT, cFlags);

    if (cSet != 0)
    {
        CTL_TwainAppMgr::GetBestContainerType(p,
            (CTL_EnumCapability)nCap,
            cGetDefault, cSetAlt, nDataType,
            CTL_GetTypeGETDEFAULT, cFlags);
    }
    else
        cGetDefault = cGetCurrent;
    if (bContainerInfoFound)
    {
        if (cSetCurrent != std::numeric_limits<UINT>::max())
            cSet = cSetCurrent;
        if (cSetAvailable != std::numeric_limits<UINT>::max())
            cSet |= cSetAvailable;
    }
    // Get the supported operations for the capability
    UINT nOps = 0;
    if (bCanQuerySupport)
    {
        nOps = CTL_TwainAppMgr::GetCapabilityOperations(p, nCap);
        if (nOps == 0)
        {
            if (nCap >= CAP_CUSTOMBASE)
            {
                nOps = (cGet > 0) ? (TWQC_GET | TWQC_GETDEFAULT | TWQC_GETCURRENT) : 0;
                nOps |= (cSet > 0) ? (TWQC_SET | TWQC_RESET | TWQC_SETCONSTRAINT) : 0;
            }
            else
            {
                bCanQuerySupport = false;
                nOps = CTL_TwainAppMgr::GetCapOps(p, nCap, false);
            }
        }
    }
    else
    {
        if (nCap >= CAP_CUSTOMBASE)
        {
            nOps = (cGet > 0) ? (TWQC_GET | TWQC_GETDEFAULT | TWQC_GETCURRENT) : 0;
            nOps |= (cSet > 0) ? (TWQC_SET | TWQC_RESET | TWQC_SETCONSTRAINT) : 0;
        }
        else
            nOps = CTL_TwainAppMgr::GetCapOps(p, nCap, bCanQuerySupport);
    }
    if (cEntryFound)
        p->SetEOJDetectedValue(cEOJValue);

    CTL_CapInfo Info(static_cast<CTL_EnumCapability>(nCap), cGet, cSet, nDataType, nOps, cGetCurrent, cGetDefault);
    pArray.insert(make_pair(nCap, Info));
}

DTWAIN_BOOL dynarithmic::DTWAIN_CacheCapabilityInfo(CTL_ITwainSource *p, CTL_TwainDLLHandle *pHandle, TW_UINT16 nCapToCache)
{
    CTL_EnumeratorNode<LONG>::container_base_type vCaps(1, nCapToCache);
    return DTWAIN_CacheCapabilityInfo(p, pHandle, &vCaps);
}

DTWAIN_BOOL dynarithmic::DTWAIN_CacheCapabilityInfo(CTL_ITwainSource *pSource, CTL_TwainDLLHandle *pHandle, CTL_EnumeratorNode<LONG>::container_pointer_type vCaps)
{
    if (pSource->RetrievedAllCaps())
        return true;
    struct CapFinder
    {
        CTL_ITwainSource *m_ps;
        LONG m_nCap;
        CapFinder(CTL_ITwainSource *ps, LONG nCap) : m_ps(ps), m_nCap(nCap) {}
        bool operator()(CTL_CapInfo& CapInfo)
        {
            if ((int)std::get<0>(CapInfo) == m_nCap)
            {
                m_ps->SetCapCached(static_cast<TW_UINT16>(m_nCap), true);
                return true;
            }
            return false;
        }
    };

    // Check if this source has had capabilities negotiated and tested
    int nWhere;

    CTL_CapInfoArrayPtr pArray;
    bool bNewArray = false;

    // get the array of cap info for this source
    pArray = GetCapInfoArray(pHandle, pSource);
    if (!pArray)
    {
        // create a new one
        pArray.reset(new CTL_CapInfoArray);
        bNewArray = true;
    }

    static const ContainerMap mapContainer = map_list_of("TW_ONEVALUE", DTWAIN_CONTONEVALUE)
        ("TW_ENUMERATION", DTWAIN_CONTENUMERATION)
        ("TW_RANGE", DTWAIN_CONTRANGE)
        ("TW_ARRAY", DTWAIN_CONTARRAY)
        ("-1", (UINT)-1)
        ("0", 0);

    FindFirstValue(pSource->GetProductName(), &pHandle->m_aSourceCapInfo, &nWhere);

    auto vIt = vCaps->begin();
    CTL_SourceCapInfo InfoSource;
    for (; vIt != vCaps->end(); ++vIt)
    {
        if (nWhere != -1 && pSource->IsCapabilityCached(static_cast<TW_UINT16>(*vIt))) // Already negotiated
            continue;

        bool bCanQuerySupport = true;
        // Not found, so test capabilities
        // Create these dynamically whenever a new source is opened
        // and source cap info does not exist.  Add cap info statically.

        // Get an array
        DTWAIN_ARRAY pDTWAINArray = 0;
        DTWAINArrayLL_RAII raii(pDTWAINArray);
        bool bGetINIEntry = true;

        bool bOk = false;

        // search the current cap array for the cap value to be tested
        if (pArray->find(static_cast<TW_UINT16>(*vIt)) != pArray->end()) //find_if(pArray->begin(), pArray->end(), CapFinder(pSource, *vIt)) != pArray->end() )
        {
            pSource->SetCapCached(static_cast<TW_UINT16>(*vIt), true);
            continue;
        }

        // if we get here, then the cap was never tested.
        TW_UINT16 nCap = static_cast<TW_UINT16>(*vIt);

        // Add capabilities where the state info is set
        pSource->AddCapToStateInfo(CAP_CUSTOMDSDATA, DTWAIN_STATE4);

        // Test the capability and see which container works.
        UINT cSet;

        UINT cGet = 0;
        UINT cGetCurrent = 0;
        UINT cGetDefault = 0;
        UINT cSetCurrent = 0;
        UINT cSetAvailable = 0;
        UINT cQuerySupport;
        UINT cEOJValue;
        UINT cEntryFound;
        bool cFlags[6] = { false };
        bool bContainerInfoFound = false;

        UINT cSetAlt = 0;
        TW_UINT16 cStateInfo = 0xFF;
        UINT nDataType = 0;
        CTL_String strName = CTL_TwainAppMgr::GetCapNameFromCap(nCap);
            bOk = GetCapInfoFromIni(strName, StringConversion::Convert_Native_To_Ansi(pSource->GetProductName()), (UINT)nCap, cGet, cGetCurrent,
                cGetDefault, cSetCurrent, cSetAvailable, cQuerySupport,
                cEOJValue, cStateInfo, nDataType, cEntryFound, bContainerInfoFound, mapContainer);

            bCanQuerySupport = cQuerySupport ? true : false;

            if (cEntryFound)
                pSource->SetEOJDetectedValue(cEOJValue);

            if (!cEntryFound)
                bGetINIEntry = false;
            else
            {
                if (cStateInfo != 0xFF)
                    pSource->AddCapToStateInfo(nCap, cStateInfo);
            }

        if (bOk)
        {
            if (CTL_TwainDLLHandle::s_lErrorFilterFlags)
            {
                CTL_StringStreamA strm;
                strm << "Using capability info from DTWAIN32.INI (Source="
                     << StringConversion::Convert_Native_To_Ansi(pSource->GetProductName())
                     << ", Cap=" << CTL_TwainAppMgr::GetCapNameFromCap(nCap) << ")\n";

                CTL_TwainAppMgr::WriteLogInfoA(strm.str());
            }

            if (bContainerInfoFound)
            {
                // set the flags here
                if (cGet != std::numeric_limits<UINT>::max())
                    cFlags[0] = true;

                if (cGetCurrent != std::numeric_limits<UINT>::max())
                    cFlags[1] = true;

                if (cGetDefault != std::numeric_limits<UINT>::max())
                    cFlags[2] = true;

                if (cSetCurrent != std::numeric_limits<UINT>::max())
                    cFlags[3] = true;

                if (cSetAvailable != std::numeric_limits<UINT>::max())
                    cFlags[4] = true;

                if (nDataType != std::numeric_limits<UINT>::max())
                    cFlags[5] = true; // always the data type is known here

                if (nDataType == std::numeric_limits<UINT>::max())
                    CTL_TwainAppMgr::GetBestCapDataType(pSource, nCap, nDataType);
            }
        }

        CTL_TwainAppMgr::GetBestContainerType(pSource, (CTL_EnumCapability)nCap, cGet, cSet,
            nDataType, CTL_GetTypeGET, cFlags);
        CTL_TwainAppMgr::GetBestContainerType(pSource, (CTL_EnumCapability)nCap, cGetCurrent,
            cSetAlt, nDataType, CTL_GetTypeGETCURRENT, cFlags);
        if (cSet != 0)
        {
            CTL_TwainAppMgr::GetBestContainerType(pSource, (CTL_EnumCapability)nCap, cGetDefault,
                cSetAlt, nDataType, CTL_GetTypeGETDEFAULT, cFlags);
        }
        else
            cGetDefault = cGetCurrent;

        if (bContainerInfoFound)
        {
            if (cSetCurrent != std::numeric_limits<UINT>::max())
                cSet = cSetCurrent;

            if (cSetAvailable != std::numeric_limits<UINT>::max())
                cSet |= cSetAvailable;
        }
        // Get the supported operations for the capability
        UINT nOps = 0;

        if (bCanQuerySupport)
        {
            nOps = CTL_TwainAppMgr::GetCapabilityOperations(pSource, nCap);

            if (nOps == 0)
            {
                if (nCap >= CAP_CUSTOMBASE)
                {
                    nOps = (cGet > 0) ? (TWQC_GET | TWQC_GETDEFAULT | TWQC_GETCURRENT) : 0;
                    nOps |= (cSet > 0) ? (TWQC_SET | TWQC_RESET) : 0;
                }
                else
                {
                    bCanQuerySupport = false;
                    nOps = CTL_TwainAppMgr::GetCapOps(pSource, nCap, false);
                }
            }
        }
        else
        {
            if (nCap >= CAP_CUSTOMBASE)
            {
                nOps = (cGet > 0) ? (TWQC_GET | TWQC_GETDEFAULT | TWQC_GETCURRENT) : 0;
                nOps |= (cSet > 0) ? (TWQC_SET | TWQC_RESET) : 0;
            }
            else
                nOps = CTL_TwainAppMgr::GetCapOps(pSource, nCap, bCanQuerySupport);
        }
        CTL_CapInfo Info(static_cast<CTL_EnumCapability>(nCap), cGet, cSet, nDataType, nOps, cGetCurrent, cGetDefault);
        pArray->insert(make_pair(nCap, Info));

        if (bNewArray)
            InfoSource = CTL_SourceCapInfo(pSource->GetProductName(), pArray, 0, 0, 0, 0, 0);

        pSource->SetCapCached(static_cast<TW_UINT16>(*vIt), true);
    }

    if (bNewArray)
        pHandle->m_aSourceCapInfo.push_back(InfoSource);
    return true;
}
