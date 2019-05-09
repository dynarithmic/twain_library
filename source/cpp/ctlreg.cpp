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
 #ifdef _MSC_VER
#pragma warning( disable : 4786)
#endif
#include <cstring>
#include <cstdio>
#include <sstream>
#include <boost/format.hpp>
#include <climits>
#include "ctlreg.h"
#include "ctltwmgr.h"
#include "ctlobstr.h"
#include "dtwain.h"
#include "../simpleini/simpleini.h"
#undef min
#undef max
using namespace dynarithmic;
static CTL_String NormalizeCapName(const CTL_String& sName);

bool SaveCapInfoToIni(const CTL_String& strSourceName, UINT nCap, const CTL_IntArray& rContainerTypes)
{
    #ifdef WIN32
    const char *szName = "dtwain32.ini";
    #else
    const char *szName = "dtwain64.ini";
    #endif

    // Saves the capability information to the DTWAIN16/32.INI
    CTL_String strKeyName;
    CTL_StringStreamA strm;
    strm << boost::format("%1%_CAP%2%") % strSourceName % nCap;
    strKeyName = strm.str();
    if ( rContainerTypes.size() != 5 )
        return false;

    // Create the entry string
    CTL_String strValues;
    CTL_String strTemp;
    CTL_StringArray aStr;
    int nValue;
    for ( int i = 0; i < 5; i++ )
    {
        if ( i > 0 )
            strValues += ',';
        nValue = rContainerTypes[i];
        strTemp.clear();
        if ( nValue != 0 )
        {
            CTL_TwainAppMgr::GetContainerNamesFromType( nValue, aStr );
            if ( aStr.size() > 0 )
                strTemp = aStr[0];
        }
        strValues += strTemp;
    }

    // Get the section name
    CSimpleIniA customProfile;
    customProfile.LoadFile(szName);
    customProfile.SetValue("TwainControl", strKeyName.c_str(), strValues.c_str());
    return true;
}

bool dynarithmic::GetCapInfoFromIni(const CTL_String& strCapName,
                       const CTL_String& strSourceName,
                       UINT /* nCap*/,
                       UINT &rGetValues,
                       UINT &rGetValuesCurrent,
                       UINT &rGetValuesDefault,
                       UINT &rSetValuesCurrent,
                       UINT &rSetValuesAvailable,
                       UINT &rQuerySupport,
                       UINT &rEOJValue,
                       TW_UINT16 &rStateInfo,
                       UINT &rDataType,
                       UINT &rEntryFound,
                       bool &bContainerInfoFound,
                       const ContainerMap &mapContainer
                       )
{
    struct DataTypeToString
    {
        LPCSTR   name;
        unsigned int dataType;
    };

    static const DataTypeToString DataTypeArray[] = {
                    {"TWTY_INT8",    TWTY_INT8},
                    {"TWTY_UINT8",   TWTY_UINT8},
                    {"TWTY_BOOL",    TWTY_BOOL},
                    {"TWTY_INT16",   TWTY_INT16},
                    {"TWTY_INT32",   TWTY_INT32},
                    {"TWTY_UINT16",  TWTY_UINT16},
                    {"TWTY_UINT32",  TWTY_UINT32},
                    {"TWTY_FIX32",   TWTY_FIX32},
                    {"TWTY_STR32",   TWTY_STR32},
                    {"TWTY_STR64",   TWTY_STR64},
                    {"TWTY_STR128",  TWTY_STR128},
                    {"TWTY_STR255",  TWTY_STR255},
                    {"TWTY_STR1024", TWTY_STR1024},
                    {"TWTY_UNI512",  TWTY_UNI512},
                    {"TWTY_FRAME",   TWTY_FRAME}
                    };
    static const unsigned DataTypeArraySize = sizeof(DataTypeArray) / sizeof(DataTypeArray[0]);

    #ifdef WIN32
    CTL_String szName = "dtwain32.ini";
    #else
    CTL_String szName = "dtwain64.ini";
    #endif

    bContainerInfoFound = false;

    szName = StringConversion::Convert_Native_To_Ansi(CTL_TwainDLLHandle::s_sINIPath) + szName;

    // Initialize State Info to indicate states 4 - 7 are negotiable for capability
    rStateInfo = 0xFF;

    // Get the capability information from DTWAIN16/32.INI
    CTL_IntArray rContainerTypes;
    CTL_String strKeyName = strSourceName;
    CTL_String strStates;

    // Check if profile string is present
    CTL_String szBuffer;

    // Check if there are any entries for the Source
    // Get the section name
    CSimpleIniA customProfile;
    customProfile.LoadFile(szName.c_str());

    // Check if MSG_QUERYSUPPORT is actually supported
    // First get a global setting
    rQuerySupport = customProfile.GetLongValue("AllSources", "QUERYSUPPORT", 1);

    // Now check the job control detector value
    rEOJValue     = customProfile.GetLongValue("AllSources", "EOJVALUE", 1);

    rEntryFound = true;

    CSimpleIniA::TNamesDepend keys;
    customProfile.GetAllKeys(strKeyName.c_str(), keys);

    if ( keys.empty() )
    {
        rEntryFound = false;
        return false;
    }

    CTL_String strNormalizedCapName = NormalizeCapName(strCapName);

    szBuffer =  customProfile.GetValue(strKeyName.c_str(), strNormalizedCapName.c_str(), " ");
    bool bFound = true;
    if ( szBuffer.empty() )
    {
        rGetValues = 0;
        rGetValuesCurrent = 0;
        rGetValuesDefault = 0;
        rSetValuesCurrent = 0;
        rSetValuesAvailable = 0;
        rDataType = std::numeric_limits<UINT>::max();
        bFound = false;
    }
    else
    {
        rGetValues = std::numeric_limits<UINT>::max();
        rGetValuesCurrent = std::numeric_limits<UINT>::max();
        rGetValuesDefault = std::numeric_limits<UINT>::max();
        rSetValuesCurrent = std::numeric_limits<UINT>::max();
        rSetValuesAvailable = std::numeric_limits<UINT>::max();
        rDataType = std::numeric_limits<UINT>::max();
        bContainerInfoFound = true;
    }
    // Now see if there is the Source has a QUERYSUPPORT setting.  If not, the default setting found
    // above will be used
    rQuerySupport = customProfile.GetLongValue(strKeyName.c_str(), "QUERYSUPPORT", rQuerySupport);

    // Now see if there is the Source has a QUERYSUPPORT setting.  If not, the default setting found
    // above will be used
    rEOJValue = customProfile.GetLongValue(strKeyName.c_str(), "EOJVALUE", rEOJValue);

    // Check the data type
    CTL_String strDataType = strNormalizedCapName + "_DATATYPE";
    CTL_String szDataType;
    szDataType = customProfile.GetValue(strKeyName.c_str(), strDataType.c_str(), "");

    // Trim the name found
    strDataType = StringWrapperA::TrimAll(strDataType);
    StringWrapperA::MakeUpperCase(strDataType);
    for (unsigned i = 0; i < DataTypeArraySize; ++i)
    {
        if ( StringWrapperA::CompareNoCase(DataTypeArray[i].name, szDataType.c_str()) )
        {
            rDataType = DataTypeArray[i].dataType;
            break;
        }
    }

    // Check if there are is any state-related info
    CTL_String szStates;
    strStates = strCapName + "_STATES";
    szStates = customProfile.GetValue(strKeyName.c_str(), strStates.c_str(), "");

    if ( !bFound && szStates.empty() )
        return false;

    // Check the values in the Capability string (parse)
    CTL_String  strValues = szBuffer;

    CTL_StringArray aStr;

    // Make sure that you parse the NULL tokens
    StringWrapperA::Tokenize(strValues, ",", aStr, true );

    // Get strings and translate them to the correct values
    ContainerMap::const_iterator it;
    CTL_String str;
    if ( aStr.size() > 0 )
    {
        if ( !aStr[0].empty() )
        {
            str = aStr[0];
            it = (ContainerMap::const_iterator) mapContainer.find( str );
            if ( it != mapContainer.end())
                rGetValues = (*it).second;
        }
        if ( !aStr[1].empty() )
        {
            str = aStr[1];
            it = (ContainerMap::const_iterator) mapContainer.find( str );
            if ( it != mapContainer.end())
                rGetValuesCurrent = (*it).second;
        }
        if ( !aStr[2].empty() )
        {
            str = aStr[2];
            it = (ContainerMap::const_iterator) mapContainer.find( str );
            if ( it != mapContainer.end())
                rGetValuesDefault = (*it).second;
        }
        if ( !aStr[3].empty() )
        {
            str = aStr[3];
            it = (ContainerMap::const_iterator)mapContainer.find( str );
            if ( it != mapContainer.end())
                rSetValuesCurrent = (*it).second;
        }
        if ( !aStr[4].empty() )
        {
            str = aStr[4];
            it = (ContainerMap::const_iterator)mapContainer.find( str );
            if ( it != mapContainer.end())
                rSetValuesAvailable = (*it).second;
        }
    }

    if ( !szStates.empty() )
    {
        // Make sure that you parse the NULL tokens
        strStates = szStates;
        StringWrapperA::Tokenize(strStates, ",", aStr, true );
        CTL_String strNum;
        short int tempInfo = 0;
        bool bFoundNum = false;
        int nStates = (int)aStr.size();
        if ( nStates > 0 )
        {
            for ( int Count = 0; Count < nStates; Count++ )
            {
                strNum = StringWrapperA::TrimAll(aStr[Count]);
                if (strNum.length() == 1 )
                {
                    int nNum = stoi(strNum);
                    if ( nNum >= 4 && nNum <= 7 )
                    {
                        bFoundNum = true;
                        tempInfo |= (1 << (nNum-1));
                    }
                }
            }
            if ( bFoundNum )
                rStateInfo = tempInfo;
        }
    }
    return true;
}


CTL_String NormalizeCapName(const CTL_String& sName)
{
    // remove spaces in all the name
    CTL_StringArray arr;
    StringWrapperA::Tokenize(sName, " ", arr);
    return StringWrapperA::Join(arr);
}
