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
#include <errno.h>
#include <sstream>
#include <boost/format.hpp>
#include <boost/filesystem.hpp>
#include "ctliface.h"
#include "ctlres.h"
#include "dtwain_resource_constants.h"
#include "ctltwmgr.h"
#include "tiffun32.h"
#include "pdffun32.h"
#include "errorcheck.h"
#include <map>
using namespace dynarithmic;

#ifdef TWAINSAVE_STATIC
LONG  TS_Command(LPCTSTR lpCommand);
#endif

namespace dynarithmic
{
    static bool load2valueMap(std::ifstream& ifs, CTL_TwainLongToStringMap& theMap)
    {
        int value1;
        CTL_String value2;
        while (ifs >> value1 >> value2)
        {
            if (value1 == -1000 && value2 == "END")
                break;
            theMap.insert({ value1, value2 });
        }
        return true;
    }

    static CTL_String createResourceFileName(const char *resName)
    {
        CTL_String sPath = StringConversion::Convert_Native_To_Ansi(dynarithmic::GetDTWAINExecutionPath());
        sPath += boost::filesystem::path::preferred_separator;
        return sPath + resName;
    }

    bool LoadTwainResources()
    {
        LOG_FUNC_ENTRY_PARAMS(())
        CTL_ErrorStruct ErrorStruct;
        int dg, dat, msg, structtype, retcode, successcode;
        CTL_String sPath = createResourceFileName(DTWAINRESOURCEINFOFILE);
        std::ifstream ifs(sPath);
        if (!ifs)
            return false;

        while (ifs >> dg >> dat >> msg >> structtype >> retcode >> successcode)
        {
            if (dg == -1000 && dat == -1000)
                break;
            auto structKey = CTL_GeneralErrorInfo::key_type{ dg,dat,msg };
            ErrorStruct.SetKey(structKey);
            ErrorStruct.SetDataType(structtype);
            ErrorStruct.SetFailureCodes(retcode);
            ErrorStruct.SetSuccessCodes(successcode);
            CTL_TwainDLLHandle::s_mapGeneralErrorInfo.insert({ structKey, ErrorStruct });
        }

        // Load the TWAIN data resources
        int resourceID;
        int twainID;
        CTL_String twainName;
        while (ifs >> resourceID >> twainID >> twainName)
        {
            if (resourceID == -1000 && twainID == -1000)
                break;
            CTL_TwainDLLHandle::s_TwainNameMap.insert({ { resourceID,twainID },twainName });
        }

        if (!load2valueMap(ifs, CTL_TwainDLLHandle::GetTwainLanguageMap()))
            return false;
        if (!load2valueMap(ifs, CTL_TwainDLLHandle::GetTwainCountryMap()))
            return false;

        decltype(CTL_CapStruct::m_strCapName) capName;
        decltype(CTL_CapStruct::m_nDataType)  capType;
        decltype(CTL_CapStruct::m_nGetContainer)  capGet;
        decltype(CTL_CapStruct::m_nSetContainer) capSet;
        LONG lCap;

        while (ifs >> lCap >> capName >> capType >> capGet >> capSet)
        {
            if (lCap == -1000 && capName == "END")
                break;
            CTL_CapStruct cStruct;
            cStruct.m_nDataType = capType;
            cStruct.m_nGetContainer = capGet;
            cStruct.m_nSetContainer = capSet;
            cStruct.m_strCapName = capName;
            CTL_TwainDLLHandle::s_mapGeneralCapInfo.insert({ (TW_UINT16)lCap, cStruct });
        }

        auto& bppMap = CTL_ImageIOHandler::GetSupportedBPPMap();
        CTL_String line;
        while (std::getline(ifs, line))
        {
            CTL_StringStreamInA strm(line);
            LONG imgType;
            strm >> imgType;
			if (imgType == -1)
				break;
            int bppValue;
            while (strm >> bppValue)
                bppMap.insert({ imgType, std::vector<int>() }).first->second.push_back(bppValue);
		}

		auto& mediamap = CTL_TwainDLLHandle::GetPDFMediaMap();
		while (std::getline(ifs, line))
		{
			CTL_StringStreamInA strm(line);
			LONG pageType;
			strm >> pageType;
			CTL_String name;
			strm >> name;
			name = StringWrapperA::TrimAll(name);
			CTL_String dimensions;
			std::getline(strm, dimensions);
			dimensions = StringWrapperA::TrimAll(dimensions);
			mediamap.insert({ pageType, {name, dimensions } });
        }

        LOG_FUNC_EXIT_PARAMS(true)
        CATCH_BLOCK(false)
    }

    size_t GetResourceString(UINT nError, LPTSTR buffer, LONG bufSize)
    {
        auto found = CTL_TwainDLLHandle::s_ResourceStrings.find(nError);
        if (found != CTL_TwainDLLHandle::s_ResourceStrings.end())
            return CopyInfoToCString(found->second.c_str(), buffer, bufSize);
        return 0;
    }

    LONG CopyInfoToCString(const CTL_StringType& strInfo, LPTSTR szInfo, LONG nMaxLen)
    {
        if (strInfo.empty() || (szInfo && nMaxLen <= 0))
            return 0;
        if (nMaxLen > 0)
            --nMaxLen;
        LONG nRealLen = 0;
        if (szInfo != NULL && nMaxLen >= 0)
        {
            size_t nLen = strInfo.size();
            nRealLen = LONG((std::min)((size_t)nMaxLen, nLen));
            LPTSTR pEnd = std::copy(strInfo.begin(), strInfo.begin() + (size_t)nRealLen, szInfo);
            *pEnd = '\0';
        }
        else
            nRealLen = (LONG)strInfo.size();
        ++nRealLen;
        return nRealLen;
    }

    static bool load2valueMap(std::ifstream& ifs, CTL_TwainLongToStringMap& theMap, const char * path)
    {
        int value1;
        CTL_String value2;
        while (ifs >> value1 >> value2)
        {
            if (value1 == -1999 && value2 == "END")
                break;
            theMap.insert({ value1, value2 });
        }
        return true;
    }

    static bool LoadLanguageResourceFromFile(const CTL_String& sPath)
    {
        std::ifstream ifs(sPath);
        bool open = false;
        int resourceID;
        CTL_String descr;
        if (ifs)
        {
            CTL_TwainDLLHandle::s_ResourceStrings.clear();
            open = true;
            CTL_String line;
            while (getline(ifs, line))
            {
                std::istringstream strm(line);
                while (strm >> resourceID)
                {
                    getline(strm, descr);
                    StringWrapperA::TrimAll(descr);
                    CTL_TwainDLLHandle::s_ResourceStrings.insert({ resourceID, StringConversion::Convert_Ansi_To_Native(descr) });
                }
            }
        }
        return open;
    }

    CTL_String GetResourceFileName(LPCTSTR lpszName)
    {
        return createResourceFileName(DTWAINLANGRESOURCEFILE) + StringConversion::Convert_Native_To_Ansi(lpszName) + ".txt";
    }

    CTL_String GetResourceFileNameA(LPCSTR lpszName)
    {
        return (createResourceFileName(DTWAINLANGRESOURCEFILE) + lpszName) + ".txt";
    }

    bool LoadLanguageResource(LPCSTR lpszName, const CTL_ResourceRegistryMap& registryMap)
    {
        LOG_FUNC_ENTRY_PARAMS((lpszName))
        auto iter = registryMap.find(lpszName);
        if (iter != registryMap.end())
        {
            if ( !iter->second )
                LOG_FUNC_EXIT_PARAMS(false)
        }
        bool retVal = LoadLanguageResource(lpszName);
        LOG_FUNC_EXIT_PARAMS(retVal)
        CATCH_BLOCK(false)
    }

    bool LoadLanguageResource(LPCSTR lpszName)
    {
        LOG_FUNC_ENTRY_PARAMS((lpszName))
        bool bReturn = LoadLanguageResourceFromFile(GetResourceFileNameA(lpszName));
        LOG_FUNC_EXIT_PARAMS(bReturn)
        CATCH_BLOCK(false)
    }

    bool LoadLanguageResource(const CTL_String& lpszName, const CTL_ResourceRegistryMap& registryMap)
    {
        return LoadLanguageResource(lpszName.c_str(), registryMap);
    }

    bool LoadLanguageResource(const CTL_String& lpszName)
    {
        return LoadLanguageResource(lpszName.c_str());
    }

bool LoadLanguageResourceXML(LPCTSTR sLangDLL)
{
    // Load the XML version of the language resources
    if ( !boost::filesystem::exists( sLangDLL))
    {
        return false;
    }
    if ( LoadLanguageResourceXMLImpl(sLangDLL) )
        CTL_TwainDLLHandle::s_UsingCustomResource = true;
    return true;
}

void UnloadStringResources()
{
    CTL_TwainDLLHandle::s_mapGeneralCapInfo.clear();
}

void UnloadErrorResources()
{
    CTL_TwainDLLHandle::s_mapGeneralErrorInfo.clear();
}

/////////////////////////////////////////////////////////////////////
CTL_CapStruct::operator CTL_String()
{
        return m_strCapName;
}
////////////////////////////////////////////////////////////////////
bool CTL_ErrorStruct::IsFailureMatch(TW_UINT16 cc)
{
    return ((1L << cc) & m_nTWCCErrorCodes)?true:false;
}

bool CTL_ErrorStruct::IsSuccessMatch(TW_UINT16 rc)
{
    if (rc == TWRC_SUCCESS)
        return true;
    return ((1L << rc) & m_nTWRCCodes)?true:false;
}
}
