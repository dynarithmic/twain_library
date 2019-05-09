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
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "ctlres.h"
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace std;
using namespace dynarithmic;
////////////////////////////////////////////////////////////////////////
CTL_TwainDLLHandle::CTL_TwainDLLHandle() :
            m_hWndTwain(nullptr),
            m_hInstance(0),
            m_nCurrentDibPos(0),
            m_bSessionAllocated(false),
            m_hNotifyWnd(nullptr),
            m_bDummyWindowCreated(false),
            m_bTransferDone(false),
            m_bSourceClosed(false),
            m_CallbackMsg(nullptr),
            m_CallbackError(nullptr),
            m_lLastError(0),
            m_lLastAcqError(0),
            m_lAcquireMode(DTWAIN_MODAL),
            m_nSourceCloseMode(DTWAIN_SourceCloseModeFORCE),
            m_nUIMode(DTWAIN_UIModeOPEN),
            m_bNotificationsUsed(false),
            m_pCallbackFn(nullptr),
            m_pCallbackFn64(nullptr),
            m_lCallbackData(0),
            m_lCallbackData64(0),
            m_pDummySource(nullptr),
            m_pOCRDefaultEngine(nullptr)
            #ifdef _WIN32
            , m_hOrigProc(nullptr)
            , m_hWndDummy(nullptr)
            #endif
             {}

CTL_TwainDLLHandle::~CTL_TwainDLLHandle()
{
    RemoveAllEnumerators();
    RemoveAllSourceCapInfo();
    RemoveAllSourceMaps();
}

void CTL_TwainDLLHandle::RemoveAllSourceCapInfo()
{
    m_aSourceCapInfo.clear();
}

void CTL_TwainDLLHandle::RemoveAllSourceMaps()
{
    m_mapStringToSource.clear();
}

void CTL_TwainDLLHandle::InitializeResourceRegistry()
{
    static std::array<const char*, 6> default_values = { { "english", "french","german","dutch", "italian", "spanish" } };
    m_ResourceRegistry.clear();
    for (size_t i = 0; i < default_values.size(); ++i)
        m_ResourceRegistry.insert({ default_values[i], boost::filesystem::exists(GetResourceFileNameA(default_values[i])) });
    }

std::pair<CTL_ResourceRegistryMap::iterator, bool> CTL_TwainDLLHandle::AddResourceToRegistry(LPCTSTR pLangDLL)
{
    CTL_String rName = StringConversion::Convert_Native_To_Ansi(pLangDLL);
    m_ResourceRegistry.erase(rName);
    return m_ResourceRegistry.insert({ rName, boost::filesystem::exists(GetResourceFileName(pLangDLL)) });
}

////////////////////////////////////////////////////////////////////////////////////////
HINSTANCE               CTL_TwainDLLHandle::s_DLLInstance = nullptr;
CTL_HookInfoArray       CTL_TwainDLLHandle::s_aHookInfo;
CTL_GeneralCapInfo      CTL_TwainDLLHandle::s_mapGeneralCapInfo;
CTL_GeneralErrorInfo    CTL_TwainDLLHandle::s_mapGeneralErrorInfo;
CTL_EnumeratorFactoryPtr CTL_TwainDLLHandle::s_EnumeratorFactory;
vector<int>             CTL_TwainDLLHandle::s_aAcquireNum;
bool                    CTL_TwainDLLHandle::s_bCheckReentrancy;
short int               CTL_TwainDLLHandle::s_nDSMState = DSM_STATE_NONE;
CTL_TwainNameMap        CTL_TwainDLLHandle::s_TwainNameMap;
CTL_TwainLongToStringMap        CTL_TwainDLLHandle::s_TwainCountryMap;
CTL_TwainLongToStringMap        CTL_TwainDLLHandle::s_TwainLanguageMap;
long                    CTL_TwainDLLHandle::s_lErrorFilterFlags = 0;
bool                    CTL_TwainDLLHandle::s_bProcessError = true;
CLogSystem              CTL_TwainDLLHandle::s_appLog;
LONG                    CTL_TwainDLLHandle::s_nRegisteredDTWAINMsg = 0;
CTL_StringType          CTL_TwainDLLHandle::s_sINIPath;
CTL_StringType          CTL_TwainDLLHandle::s_CurLangResource;
CTL_StringType          CTL_TwainDLLHandle::s_TempFilePath;
std::unordered_set<DTWAIN_SOURCE>   CTL_TwainDLLHandle::s_aFeederSources;
UINT_PTR                CTL_TwainDLLHandle::s_nTimerID = 0;
UINT_PTR                CTL_TwainDLLHandle::s_nTimeoutID = 0;
bool                    CTL_TwainDLLHandle::s_bTimerIDSet = false;
std::unordered_set<HWND>      CTL_TwainDLLHandle::s_appWindowsToDisable;
bool                    CTL_TwainDLLHandle::s_bQuerySupport = false;
bool                    CTL_TwainDLLHandle::s_bCheckHandles = true;
UINT                    CTL_TwainDLLHandle::s_nTimeoutMilliseconds = 0;
DTWAIN_DIBUPDATE_PROC   CTL_TwainDLLHandle::s_pDibUpdateProc = nullptr;
CTL_StringType          CTL_TwainDLLHandle::s_ImageDLLFilePath;
CTL_StringType          CTL_TwainDLLHandle::s_LangResourcePath;
std::deque<int> CTL_TwainDLLHandle::s_vErrorBuffer;
unsigned int            CTL_TwainDLLHandle::s_nErrorBufferThreshold = 50;
unsigned int            CTL_TwainDLLHandle::s_nErrorBufferReserve = 1000;
bool                    CTL_TwainDLLHandle::s_bThrowExceptions = false;
std::stack<unsigned long, std::deque<unsigned long> > CTL_TwainDLLHandle::s_vErrorFlagStack;
CTL_CallbackProcArray   CTL_TwainDLLHandle::s_aAllCallbacks;
CTL_ERRORCODEMAP        CTL_TwainDLLHandle::s_ErrorCodes;
CTL_MAPLONGTOSTRING     CTL_TwainDLLHandle::s_ResourceStrings;
bool                    CTL_TwainDLLHandle::s_UsingCustomResource = false;
bool                    CTL_TwainDLLHandle::s_DemoInitialized;
int                     CTL_TwainDLLHandle::s_nDSMVersion = DTWAIN_TWAINDSM_LEGACY;
bool                    CTL_TwainDLLHandle::s_ResourcesInitialized = false;
CTL_TwainMemoryFunctions*     CTL_TwainDLLHandle::s_TwainMemoryFunc = nullptr;
CTL_LegacyTwainMemoryFunctions CTL_TwainDLLHandle::s_TwainLegacyFunc;
CTL_Twain2MemoryFunctions      CTL_TwainDLLHandle::s_Twain2Func;
int                     CTL_TwainDLLHandle::s_TwainDSMSearchOrder = DTWAIN_TWAINDSMSEARCH_WSO;
DTWAIN_LOGGER_PROC      CTL_TwainDLLHandle::s_pLoggerCallback = nullptr;
DTWAIN_LOGGER_PROCA     CTL_TwainDLLHandle::s_pLoggerCallbackA = nullptr;
DTWAIN_LOGGER_PROCW     CTL_TwainDLLHandle::s_pLoggerCallbackW = nullptr;
DTWAIN_LONG64           CTL_TwainDLLHandle::s_pLoggerCallback_UserData = 0;
DTWAIN_LONG64           CTL_TwainDLLHandle::s_pLoggerCallback_UserDataA = 0;
DTWAIN_LONG64           CTL_TwainDLLHandle::s_pLoggerCallback_UserDataW = 0;

bool                    CTL_TwainDLLHandle::s_TwainCallbackSet = false;

#ifdef WIN32
CRITICAL_SECTION        CTL_TwainDLLHandle::s_critLogCall;
bool                    CTL_TwainDLLHandle::s_bCritSectionCreated = false;
CRITICAL_SECTION        CTL_TwainDLLHandle::s_critFileCreate;
bool                    CTL_TwainDLLHandle::s_bFileCritSectionCreated = false;
CRITICAL_SECTION        CTL_TwainDLLHandle::s_critStaticInit;
bool                    CTL_TwainDLLHandle::s_bCritStaticCreated;
#endif

CTL_IMAGEDLLINFO        CTL_TwainDLLHandle::s_ImageDLLInfo;

std::bitset<10> CTL_TwainDLLHandle::g_AvailabilityFlags = 0;

///////////////////////////////////////////////////////////////////////////
void CTL_TwainDLLHandle::NotifyWindows(UINT /*nMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/)
{
}

CTL_StringType CTL_TwainDLLHandle::GetTwainNameFromResource(int nWhichResourceID, int nWhichItem)
{
    auto iter = s_TwainNameMap.find({ nWhichResourceID,nWhichItem });
    if (iter != s_TwainNameMap.end())
        return StringConversion::Convert_Ansi_To_Native(iter->second);
    return CTL_StringType();
}

/////////////////////////////////////////////////////////////////////////
// static definitions
vector<CTL_TwainDLLHandlePtr> CTL_TwainDLLHandle::s_DLLHandles;

CTL_TwainDLLHandle* dynarithmic::FindHandle(HWND hWnd, bool bIsDisplay)
{
    auto it = std::find_if(CTL_TwainDLLHandle::s_DLLHandles.begin(), CTL_TwainDLLHandle::s_DLLHandles.end(),
        [&](CTL_TwainDLLHandlePtr& p)
    {
        if ( bIsDisplay)
            return false;
        return (p.get() && (p.get()->m_hWndTwain == hWnd));
    });
    if (it != CTL_TwainDLLHandle::s_DLLHandles.end())
        return (*it).get();
    return NULL;
}

CTL_TwainDLLHandle* dynarithmic::FindHandle(HINSTANCE hInst)
{
    auto it = std::find_if(CTL_TwainDLLHandle::s_DLLHandles.begin(), CTL_TwainDLLHandle::s_DLLHandles.end(),
                           [&](CTL_TwainDLLHandlePtr& p)
    { return (p.get() && p.get()->m_hInstance == hInst); });
    if (it != CTL_TwainDLLHandle::s_DLLHandles.end())
        return (*it).get();
    return NULL;
}
