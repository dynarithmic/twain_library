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
#ifndef CTLIFACE_H_
#define CTLIFACE_H_

#include "dtwain_retail_def.h"
#include "tr1defs.h"
#include "dtwain_raii.h"
#include <ocrinterface.h>
#include "pdffont_basic.h"
#include <boost/functional/hash.hpp>

#ifdef _MSC_VER
#pragma warning( disable : 4786)
#pragma warning (disable : 4786)
#pragma warning (disable : 4127)
#endif
#include <unordered_map>
#include <vector>
#include <unordered_set>
#include <deque>
#include <stack>
#include <bitset>
#include <list>
#include <array>
#include <algorithm>
#include <queue>
#include <cstring>
#include <type_traits>

#ifdef _WIN32
#include "winlibraryloader_impl.inl"
#else
#include "linuxlibraryloader_impl.inl"
#endif
template <typename T>
struct dtwain_library_loader : library_loader_impl
{
    static T get_func_ptr(void *handle, const char *name)
    {
        return (T)library_loader_impl::get(handle, name);
    }
};

#include "ctltmpl4.h"
#include "ctltwses.h"
#include "logmsg.h"
#include "winconst.h"
#include "cppfunc.h"
#include "capstruc.h"
#include "errstruc.h"
#include "dtwain_resource_constants.h"

namespace dynarithmic
{
    class CTL_TwainDLLHandle;
    class CTL_ITwainSource;
    class CTL_TwainAppMgr;
    class CTL_TwainDibArray;
    struct SourceSelectionOptions;
    struct SourceAcquireOptions;
    #include "dtwdecl.h"

    #define  TWMSG_CancelSourceSelected         1100
    #define  TWMSG_TwainFailureConditionCode    1101
    #define  TWMSG_TwainAcquireImage            1102
        /* DTWAIN Source UI Close Modes */
    #define DTWAIN_SourceCloseModeFORCE           0
    #define DTWAIN_SourceCloseModeBYPASS          1

    #define DSM_STATE_NONE      1
    #define DSM_STATE_LOADED    2
    #define DSM_STATE_OPENED    3
        /* Select source wParam's */
    #define  DTWAIN_SelectSourceFailed                1016
    #define  DTWAIN_AcquireSourceClosed               1017
    #define  DTWAIN_TN_ACQUIRECANCELLED_EX            1200
    #define  DTWAIN_TN_ACQUIREDONE_EX                 1205
    #define  DTWAIN_RETRY_EX                          9997

        // modal processing messages
    #define DTWAIN_TN_SETUPMODALACQUISITION           1300
    #define DTWAIN_TN_MESSAGELOOPERROR                1500
    #define REGISTERED_DTWAIN_MSG _T("DTWAIN_NOTIFY-{37AE5C3E-34B6-472f-A0BC-74F3CB199F2B}")

        // Availability flags
    #define DTWAIN_BASE_AVAILABLE       0
    #define DTWAIN_PDF_AVAILABLE        1
    #define DTWAIN_TWAINSAVE_AVAILABLE  2
    #define DTWAIN_OCR_AVAILABLE        3
    #define DTWAIN_BARCODE_AVAILABLE    4

        /* Transfer started */
        /* Scanner already has physically scanned a page.
         This is sent only once (when TWAIN actually does the transformation of the
         scanned image to the DIB) */
    #define  DTWAIN_TWAINAcquireStarted               1019

        /* Sent when DTWAIN_Acquire...() functions are about to return */
    #define  DTWAIN_AcquireTerminated                 1020
    #ifdef _WIN32
    #define  TWAINDLLVERSION_1    _T("TWAIN_32.DLL")
    #define  TWAINDLLVERSION_2    _T("TWAINDSM.DLL")
    #else
    #define  TWAINDLLVERSION_1    _T("")
    #define  TWAINDLLVERSION_2    _T("/usr/local/lib/libtwaindsm.so")
    #endif

    template <typename CallbackType, typename UserType>
    struct CallbackInfo
    {
        CallbackType Fn;
        UserType UserData;
        LRESULT retvalue;
        CallbackInfo(CallbackType theFn=NULL, UserType theUserData=0) :
        Fn(theFn), UserData(theUserData), retvalue(1)
        {}
    };

    #include "capstruc.h"
    #include "errstruc.h"

    typedef CTL_ClassValues7<CTL_EnumCapability,/* Capability*/
                             UINT             , /* Container for Get*/
                             UINT             , /* Container for Set*/
                             UINT             ,  /* Data Type */
                             UINT             ,  /* Available cap support */
                             UINT             ,  /* Container for Get Current */
                             UINT                /* Container for Get Default  */
                             > CTL_CapInfo;

    typedef CTL_ClassValues7<DWORD, HHOOK, HHOOK, CTL_TwainDLLHandle*, bool,char,char>  CTL_HookInfo;

    typedef std::unordered_map<LONG, int> CTL_MAPLONGTOINT;
    typedef std::unordered_map<std::pair<int, int>, CTL_String, boost::hash<std::pair<int, int>>> CTL_TwainNameMap;
    typedef std::unordered_map<CTL_StringType, CTL_ITwainSource*> CTL_MAPSTRTOSOURCE;
    typedef std::unordered_map<CTL_StringType, int> CTL_MAPSTRINGTOINT;
    typedef std::unordered_map<LONG, HMODULE> CTL_MAPLONGTOHINSTANCE;
    typedef std::unordered_map<CTL_EnumCapability, CTL_CapInfo> CTL_MAPCAPTOINFO;
    typedef std::vector<CallbackInfo<DTWAIN_CALLBACK_PROC, LONG> > CTL_CallbackProcArray;
    typedef std::vector<CallbackInfo<DTWAIN_CALLBACK_PROC, LONGLONG> > CTL_CallbackProcArray64;
    typedef std::unordered_map<LONG, CTL_StringType> CTL_ERRORCODEMAP;
    typedef std::unordered_map<LONG, CTL_StringType> CTL_MAPLONGTOSTRING;
    typedef std::unordered_map<LONG, std::vector<LONG> > CTL_MAPLONGTOVECTORLONG;
    typedef std::vector<CTL_HookInfo>     CTL_HookInfoArray;
    typedef std::unordered_map<CTL_String, bool> CTL_ResourceRegistryMap;
    typedef std::unordered_map<LONG, CTL_String> CTL_TwainLongToStringMap;

    // Create these dynamically whenever a new source is opened
    // and source cap info does not exist.  Add cap info statically.
    typedef std::unordered_map<CTL_EnumCapability, CTL_CapInfo>  CTL_CapInfoArray;
    typedef std::shared_ptr<CTL_CapInfoArray> CTL_CapInfoArrayPtr;

    // Create this statically when initializing.  Initialize the second
    // value with the dynamically created CTL_CapInfoArray above
    typedef CTL_ClassValues7<CTL_StringType, /* Product Name */
                             CTL_CapInfoArrayPtr, /* Array of cap info*/
                             int,       /* dummy */
                             int,        /* dummy */
                             char,
                             char,
                             char> CTL_SourceCapInfo;

    // Add the statically created CTL_SourceCapInfo to this array
    typedef std::vector<CTL_SourceCapInfo> CTL_SourceCapInfoArray;

    const int DTWAIN_MaxErrorSize=256;
    class CTLTwainDibArray;


    // define a vector that holds OCREngine interfaces
    typedef std::vector<OCREnginePtr> OCRInterfaceContainer;
    typedef std::unordered_map<CTL_StringType, OCREnginePtr> OCRProductNameToEngineMap;

    struct CustomPlacement
    {
        LONG nOptions;
        int xpos;
        int ypos;
        HWND hWndParent;
        CTL_StringType sTitle;
        CustomPlacement() : nOptions(0), xpos(0), ypos(0), hWndParent(0) {}
    };

    struct SelectStruct
    {
        CTL_StringType SourceName;
        CustomPlacement CS;
        int nItems;
        bool *m_pbSourcesOnSelect;
    };

    template <typename T>
    struct SmartPointerFinder
    {
        typedef typename T::element_type *ptr_type;
        ptr_type m_ptr;
        SmartPointerFinder(ptr_type p) : m_ptr(p) {}
        bool operator()(const T& value) const
        { return value.get() == m_ptr; }
    };

    struct ImageModuleDef
    {
        CTL_StringType sName;
        LONG    ImgType;
        HMODULE hMod;
        bool  bIsLoaded;
    };
    #define DTWAINFrameInternalGUID _T("80301C36-4E51-48C3-B2C9-B04E28D5C5FD")
    struct DTWAINFrameInternal
    {
        std::array<double, 4> m_FrameComponent;
        std::array<TCHAR, sizeof(DTWAINFrameInternalGUID) / sizeof(TCHAR)> s_id;
        DTWAINFrameInternal(double left=0, double top=0, double right=0, double bottom=0)
        {
            m_FrameComponent[DTWAIN_FRAMELEFT] = left;
            m_FrameComponent[DTWAIN_FRAMETOP] = top;
            m_FrameComponent[DTWAIN_FRAMERIGHT] = right;
            m_FrameComponent[DTWAIN_FRAMEBOTTOM] = bottom;
            std::copy(DTWAINFrameInternalGUID,
                      DTWAINFrameInternalGUID + sizeof(DTWAINFrameInternalGUID) / sizeof(TCHAR),
                      s_id.begin());
            s_id.back() = _T('\0');
        }
    };

    inline bool operator==(const DTWAINFrameInternal& lhs, const DTWAINFrameInternal& rhs)
    {
        return lhs.m_FrameComponent == rhs.m_FrameComponent;
    }

    inline bool operator!=(const DTWAINFrameInternal& lhs, const DTWAINFrameInternal& rhs)
    {
        return !(lhs.m_FrameComponent == rhs.m_FrameComponent);
    }

    #define DTWAIN_TWAINSAVEMODULE   20000 /* special type used for TwainSave functionality */

    typedef std::vector<ImageModuleDef> CTL_IMAGEDLLINFO;

    #define DTWAIN_PAGEMISSINGSTR _T("<missing_page>")
    typedef std::shared_ptr<CTL_TwainDLLHandle> CTL_TwainDLLHandlePtr;

    enum CTL_EnumeratorType { CTL_EnumeratorPtrType     = 1,
                                CTL_EnumeratorIntType       = 2,
                                CTL_EnumeratorDoubleType    = 3,
                                CTL_EnumeratorDibType     = 4,
                                CTL_EnumeratorSourceType    = 5,
                                CTL_EnumeratorStringType  = 6,
                                CTL_EnumeratorDTWAINFrameType   = 7,
                                CTL_EnumeratorLongStringType = 8,
                                CTL_EnumeratorUnicodeStringType = 9,
                                CTL_EnumeratorInt64Type   = 10,
                                CTL_EnumeratorANSIStringType = 11,
                                CTL_EnumeratorWideStringType = 12,
                                CTL_EnumeratorTWFIX32Type = 200,
                                CTL_EnumeratorTWFrameType = 500,
                                CTL_EnumeratorAnyType     = 1000,
                                CTL_EnumeratorInvalid     = -1
    };

    template <typename T, int enumType=0>
    struct CTL_EnumeratorNode
    {
        typedef std::vector<T>  container_base_type;
        typedef container_base_type* container_pointer_type;
        typedef typename container_base_type::iterator container_iterator_type;
        int m_EnumType;
        container_base_type m_Array;
        CTL_EnumeratorNode(int nSize) : m_EnumType(enumType), m_Array(nSize) {}
        int GetEnumType() const { return m_EnumType; }
        void SetEnumType(int EnumType) { m_EnumType = EnumType; }
        enum {ENUMTYPE = enumType};
    };

    typedef CTL_ITwainSource* CTL_ITwainSourcePtr;
    typedef CTL_EnumeratorNode<int, CTL_EnumeratorIntType>                  CTL_Enumerator_int;
    typedef CTL_EnumeratorNode<LONG64, CTL_EnumeratorInt64Type>             CTL_Enumerator_LONG64;
    typedef CTL_EnumeratorNode<double, CTL_EnumeratorDoubleType>            CTL_Enumerator_double;
    typedef CTL_EnumeratorNode<HANDLE, CTL_EnumeratorDibType>               CTL_Enumerator_HANDLE;
    typedef CTL_EnumeratorNode<CTL_ITwainSourcePtr, CTL_EnumeratorSourceType> CTL_Enumerator_CTL_ITwainSourcePtr;
    typedef CTL_EnumeratorNode<LPVOID, CTL_EnumeratorPtrType>               CTL_Enumerator_LPVOID;
    typedef CTL_EnumeratorNode<CTL_StringType, CTL_EnumeratorStringType>        CTL_Enumerator_CTL_StringType;
    typedef CTL_EnumeratorNode<CTL_String, CTL_EnumeratorANSIStringType>        CTL_Enumerator_CTL_String;
    typedef CTL_EnumeratorNode<CTL_WString, CTL_EnumeratorWideStringType>        CTL_Enumerator_CTL_WString;
    typedef CTL_EnumeratorNode<DTWAINFrameInternal, CTL_EnumeratorDTWAINFrameType> CTL_Enumerator_DTWAINFrameInternal;
    typedef CTL_EnumeratorNode<TW_FRAME, CTL_EnumeratorTWFrameType>         CTL_Enumerator_TW_FRAME;
    typedef CTL_EnumeratorNode<TW_FIX32, CTL_EnumeratorTWFIX32Type>         CTL_Enumerator_TW_FIX32;

    typedef std::shared_ptr<TW_FIX32> TW_FIX32Ptr;

    //typedef std::list<DTWAINFrameInternalPtr> DTWAINFrameList;
    typedef std::list<DTWAINFrameInternal> DTWAINFrameList;

    struct CTL_EnumeratorFactory
    {
        // Make these lists
        std::list< CTL_Enumerator_int >         m_EnumeratorList_int;
        std::list< CTL_Enumerator_LONG64 >      m_EnumeratorList_LONG64;
        std::list< CTL_Enumerator_double>       m_EnumeratorList_double;
        std::list< CTL_Enumerator_HANDLE>       m_EnumeratorList_HANDLE;
        std::list< CTL_Enumerator_CTL_ITwainSourcePtr> m_EnumeratorList_CTL_ITwainSourcePtr;
        std::list< CTL_Enumerator_LPVOID>       m_EnumeratorList_LPVOID;
        std::list< CTL_Enumerator_CTL_StringType >  m_EnumeratorList_CTL_StringType;
        std::list< CTL_Enumerator_CTL_String >  m_EnumeratorList_CTL_String;
        std::list< CTL_Enumerator_CTL_WString >  m_EnumeratorList_CTL_WString;
        std::list< CTL_Enumerator_DTWAINFrameInternal> m_EnumeratorList_DTWAINFrameInternal;
        std::list< CTL_Enumerator_TW_FRAME >    m_EnumeratorList_TW_FRAME;
        std::list< CTL_Enumerator_TW_FIX32 >    m_EnumeratorList_TW_FIX32;

        // special list for TW_FIX32 individual instances
        std::list<TW_FIX32Ptr>                    m_AvailableFix32Values;
        DTWAINFrameList                           m_AvailableFrameValues;
    };

    typedef std::shared_ptr<CTL_EnumeratorFactory> CTL_EnumeratorFactoryPtr;

    class CTL_TwainDynMemoryHandler
    {
            TW_HANDLE m_handle;
            TW_MEMREF m_memPtr;
            TW_UINT32 m_memSize;

        public:
            TW_HANDLE getHandle() const { return m_handle; }
            TW_MEMREF getMemoryPtr() const { return m_memPtr; }
            TW_UINT32 getMemorySize() const { return m_memSize; }
            void setHandle(TW_HANDLE h) { m_handle = h; }
            void setMemoryPtr(TW_MEMREF p) { m_memPtr = p; }
            void setMemorySize(TW_UINT32 s) { m_memSize = s; }
            CTL_TwainDynMemoryHandler(TW_HANDLE h=0, TW_MEMREF p=0, TW_UINT32 memSize = 0)
                    : m_handle(h), m_memPtr(p), m_memSize(memSize) {}
    };

    // mimics 2.0 memory function pointers
    class CTL_TwainMemoryFunctions
    {
        public:
            virtual ~CTL_TwainMemoryFunctions() {}
            virtual TW_HANDLE AllocateMemory(TW_UINT32 size) = 0;
            virtual void      FreeMemory(TW_HANDLE h) = 0;
            virtual TW_MEMREF LockMemory(TW_HANDLE h) = 0;
            virtual void      UnlockMemory(TW_HANDLE h) = 0;

            TW_MEMREF AllocateMemoryPtr(TW_UINT32 size, TW_HANDLE* pHandle = NULL)
            {
                TW_HANDLE h = AllocateMemory(size);
                if ( h )
                {
                    if ( pHandle )
                       *pHandle = h;
                    return LockMemory(h);
                }
                if ( pHandle )
                  *pHandle = NULL;
                return NULL;
            }

            TW_MEMREF ReallocateMemory(CTL_TwainDynMemoryHandler& memHandler, TW_UINT32 newSize)
            {
                // Allocate new memory
                TW_HANDLE newHandle = AllocateMemory(newSize);
                if (!newHandle)
                    return NULL;

                // copy old memory to new memory
                TW_MEMREF oldMem = memHandler.getMemoryPtr();
                TW_MEMREF newMem = LockMemory(newHandle);
                memcpy(newMem, oldMem, (std::min)(newSize, memHandler.getMemorySize()));
                UnlockMemory(newMem);

                // delete the old memory
                UnlockMemory(memHandler.getHandle());
                FreeMemory(memHandler.getHandle());

                // copy memHandler by constructing a new memory handler
                memHandler = CTL_TwainDynMemoryHandler(newHandle, newMem, newSize);
                return newHandle;
            }
    };


    class CTL_LegacyTwainMemoryFunctions : public CTL_TwainMemoryFunctions
    {
        public:
        #ifdef WIN32
            TW_HANDLE AllocateMemory(TW_UINT32 size) { return ::GlobalAlloc(GHND, size); }
            void      FreeMemory(TW_HANDLE h) { if (h) ::GlobalFree( h ); }
            TW_MEMREF LockMemory(TW_HANDLE h) { if (h) return ::GlobalLock(h); return NULL; }
            void      UnlockMemory(TW_HANDLE h) { if (h) ::GlobalUnlock(h); }
        #else
            TW_HANDLE AllocateMemory(TW_UINT32) { return nullptr; }
            void      FreeMemory(TW_HANDLE) { }
            TW_MEMREF LockMemory(TW_HANDLE) { return nullptr; }
            void      UnlockMemory(TW_HANDLE) { }
        #endif
    };


    class CTL_Twain2MemoryFunctions : public CTL_TwainMemoryFunctions
    {
        public:
            TW_ENTRYPOINT m_EntryPoint;
            TW_HANDLE AllocateMemory(TW_UINT32 size) { return m_EntryPoint.DSM_MemAllocate(size); }
            void      FreeMemory(TW_HANDLE h) { if (h) m_EntryPoint.DSM_MemFree(h); }
            TW_MEMREF LockMemory(TW_HANDLE h) { if (h) return m_EntryPoint.DSM_MemLock(h); return NULL; }
            void      UnlockMemory(TW_HANDLE h) { if (h) m_EntryPoint.DSM_MemUnlock(h); }
    };

    class TwainMessageLoopImpl
    {
        private:
            CTL_TwainDLLHandle* m_pDLLHandle;

        protected:
            virtual bool IsSourceOpen(CTL_ITwainSource* pSource, bool bUIOnly);
            virtual bool CanEnterDispatch(MSG * /*pMsg*/) { return true; }

        public:
            TwainMessageLoopImpl(CTL_TwainDLLHandle* pHandle) : m_pDLLHandle(pHandle) {}
            virtual ~TwainMessageLoopImpl() {}
            virtual void PrepareLoop() {}
            virtual void PerformMessageLoop(CTL_ITwainSource * /*pSource*/, bool /*bUIOnly*/) {}
    };

    class TwainMessageLoopWindowsImpl : public TwainMessageLoopImpl
    {
        public:
            TwainMessageLoopWindowsImpl(CTL_TwainDLLHandle* pHandle) : TwainMessageLoopImpl(pHandle) {}
            void PrepareLoop() override
            {
            #ifdef WIN32
                MSG msg;
                // Call this so that we have a queue to deal with
                PeekMessage(&msg, NULL, WM_USER, WM_USER, PM_NOREMOVE);
            #endif
            }

            void PerformMessageLoop(CTL_ITwainSource *pSource, bool bUIOnly);
    };

    class TwainMessageLoopV1 : public TwainMessageLoopWindowsImpl
    {
        public:
            TwainMessageLoopV1(CTL_TwainDLLHandle* pHandle) : TwainMessageLoopWindowsImpl(pHandle) {}
            bool CanEnterDispatch(MSG *pMsg) { return !DTWAIN_IsTwainMsg(pMsg); }
    };

    class TwainMessageLoopV2 : public TwainMessageLoopWindowsImpl
    {
        public:
            static std::queue<MSG> s_MessageQueue;

            static TW_UINT16 TW_CALLINGSTYLE TwainVersion2MsgProc(
                pTW_IDENTITY pOrigin,
                pTW_IDENTITY pDest,
                TW_UINT32 DG_,
                TW_UINT16 DAT_,
                TW_UINT16 MSG_,
                TW_MEMREF pData
                );

            TwainMessageLoopV2(CTL_TwainDLLHandle* pHandle) : TwainMessageLoopWindowsImpl(pHandle) {}
            void PrepareLoop()
            {
                // remove elements from the queue
                std::queue<MSG> empty;
                std::swap(s_MessageQueue, empty);
            }

            bool IsSourceOpen(CTL_ITwainSource* pSource, bool bUIOnly);
            bool CanEnterDispatch(MSG *pMsg) { return !DTWAIN_IsTwainMsg(pMsg); }
    };

    class CTL_TwainDLLHandle
    {
        public:
			typedef std::unordered_map<LONG, std::pair<CTL_String, CTL_String>> CTL_PDFMediaMap;
            CTL_TwainDLLHandle();
            ~CTL_TwainDLLHandle();
            static HINSTANCE GetImageLibrary(LONG nWhich);
            bool    DeleteDibArray( int nWhich );
            bool    DeleteDibArray( CTL_TwainDibArray *pArray );
            bool    CreateNewDibArray( const CTL_TwainDibArray& rDibArray, int nWhere=-1 );
            void    RemoveAllDibArrays();
            CTL_TwainDibArray *GetDibArray(int nWhich);
            int     GetNewDibPos() const;
            int     GetCurrentDibPos() const;
            int     SetCurrentDibPos(int nWhere);
            void    NotifyWindows( UINT nMsg, WPARAM wParam, LPARAM lParam );
            void    RemoveAllEnumerators();
            void    RemoveAllSourceCapInfo();
            void    RemoveAllSourceMaps();
            void    InitializeResourceRegistry();
            std::pair<CTL_ResourceRegistryMap::iterator, bool> AddResourceToRegistry(LPCTSTR pLangDLL);
            CTL_ResourceRegistryMap& GetResourceRegistry() { return m_ResourceRegistry; }
            static CTL_TwainLongToStringMap& GetTwainCountryMap() { return s_TwainCountryMap;  }
            static CTL_TwainLongToStringMap& GetTwainLanguageMap() { return s_TwainLanguageMap; }
            CTL_StringType GetVersionString() const { return  m_VersionString; }
            void    SetVersionString(const CTL_StringType& s) { m_VersionString = s; }
            static LONG    GetItemSize( CTL_EnumeratorType EnumType );

            static DTWAIN_ACQUIRE     GetNewAcquireNum();
            static void             EraseAcquireNum(DTWAIN_ACQUIRE nNum);
            static CTL_StringType   GetTwainNameFromResource(int nWhichResourceID, int nWhichItem);
            static int              GetDGResourceID()  { return 8890; }
            static int              GetDATResourceID() { return 8891; }
            static int              GetMSGResourceID() { return 8892; }
            static long             GetErrorFilterFlags() { return s_lErrorFilterFlags; }
			static CTL_PDFMediaMap& GetPDFMediaMap() { return s_PDFMediaMap; }

            CTL_TwainAppMgr* m_pAppMgr;

            struct tagSessionStruct
            {
                TW_UINT16 nMajorNum;
                TW_UINT16 nMinorNum;
                TW_UINT16 nLanguage;
                TW_UINT16 nCountry;
                CTL_StringType szVersion;
                CTL_StringType szManufact;
                CTL_StringType szFamily;
                CTL_StringType szProduct;
                CTL_StringType DSMName;
                int nSessionType;
                tagSessionStruct() : nMajorNum(1),
                                     nMinorNum(0),
                                     nLanguage(TwainLanguage_USAENGLISH),
                                     nCountry(TwainCountry_USA),
                                     szVersion(_T("<?>")),
                                     szManufact(_T("<?>")),
                                     szFamily(_T("<?>")),
                                     szProduct(_T("<?>")),
                                     #ifdef _WIN64
                                     DSMName(TWAINDLLVERSION_2),
                                     nSessionType(DTWAIN_TWAINDSM_VERSION2)
                                     #else
                                     DSMName(TWAINDLLVERSION_1),
                                     nSessionType(DTWAIN_TWAINDSM_LEGACY)
                                     #endif
                                    {}
            };

            tagSessionStruct m_SessionStruct;
            CTL_ResourceRegistryMap m_ResourceRegistry;
            CTL_TwainSession    m_Session;
            CTL_StringType      m_VersionString;
            HINSTANCE           m_hInstance;
            HWND                m_hWndTwain;
            HWND                m_hNotifyWnd;
            #ifdef _WIN32
            WNDPROC             m_hOrigProc;
            HWND                m_hWndDummy;
            #endif
            int                 m_nCurrentDibPos;
            bool                m_bSessionAllocated;
            bool                m_bDummyWindowCreated;
            bool                m_bTransferDone;
            bool                m_bSourceClosed;    // Used for "WAIT" mode
            DTWAIN_CALLBACK     m_CallbackMsg;
            DTWAIN_CALLBACK     m_CallbackError;
            LONG                m_lLastError;
            LONG                m_lLastAcqError;
            LONG                m_lAcquireMode;
            bool                m_nSourceCloseMode;
            int                 m_nUIMode;
            bool                m_bNotificationsUsed;
            DTWAIN_CALLBACK_PROC m_pCallbackFn;
            DTWAIN_CALLBACK_PROC64 m_pCallbackFn64;
            static DTWAIN_LOGGER_PROC  s_pLoggerCallback;
            static DTWAIN_LOGGER_PROCA  s_pLoggerCallbackA;
            static DTWAIN_LOGGER_PROCW  s_pLoggerCallbackW;
            static DTWAIN_LONG64  s_pLoggerCallback_UserData;
            static DTWAIN_LONG64  s_pLoggerCallback_UserDataA;
            static DTWAIN_LONG64  s_pLoggerCallback_UserDataW;
            LONG                m_lCallbackData;
            LONGLONG            m_lCallbackData64;
            CTL_ITwainSource*   m_pDummySource;
            OCRInterfaceContainer m_OCRInterfaceArray;
            OCRProductNameToEngineMap m_OCRProdNameToEngine;
            OCREnginePtr          m_pOCRDefaultEngine;
			static CTL_PDFMediaMap s_PDFMediaMap;


            // File Save As information
            #ifdef _WIN32
            std::unique_ptr<OPENFILENAME>  m_pofn;
            LONG                m_nSaveAsFlags;
            POINT               m_SaveAsPos;
            LPOFNHOOKPROC       m_pSaveAsDlgProc;
            CustomPlacement     m_CustomPlacement;
            #endif
            CTL_TEXTELEMENTPTRLIST m_lPDFTextElement;
            bool                m_bUseProxy;
            CTL_SourceCapInfoArray   m_aSourceCapInfo;
            CTL_MAPSTRTOSOURCE       m_mapStringToSource;
            static CTL_TwainNameMap    s_TwainNameMap;
            static CTL_TwainLongToStringMap s_TwainCountryMap;
            static CTL_TwainLongToStringMap s_TwainLanguageMap;

            static HINSTANCE         s_DLLInstance;

            // static arrays
            static bool                     s_ResourcesInitialized;
            static std::vector<CTL_TwainDLLHandlePtr> s_DLLHandles;
            static std::unordered_set<DTWAIN_SOURCE> s_aFeederSources;
            static CTL_HookInfoArray        s_aHookInfo;
            static std::vector<int>              s_aAcquireNum;
            static bool                     s_bCheckReentrancy;
            static CTL_GeneralCapInfo       s_mapGeneralCapInfo;
            static CTL_GeneralErrorInfo     s_mapGeneralErrorInfo;
            static short int                s_nDSMState;
            static int                      s_nDSMVersion;
            static long                     s_lErrorFilterFlags;
            static bool                     s_bProcessError;
            static CLogSystem               s_appLog;
            static LONG                     s_nRegisteredDTWAINMsg;
            static CTL_StringType           s_sINIPath;
            static CTL_StringType           s_CurLangResource;
            static CTL_StringType           s_TempFilePath;
            static CTL_StringType           s_ImageDLLFilePath;
            static CTL_StringType           s_LangResourcePath;
            static UINT_PTR                 s_nTimerID;
            static UINT_PTR                 s_nTimeoutID;
            static UINT                     s_nTimeoutMilliseconds;
            static bool                     s_bTimerIDSet;
            static bool                     s_bThrowExceptions;
            static CTL_CallbackProcArray    s_aAllCallbacks;
            static CTL_ERRORCODEMAP         s_ErrorCodes;
            static CTL_MAPLONGTOSTRING      s_ResourceStrings;
            static CTL_EnumeratorFactoryPtr s_EnumeratorFactory;
            static bool                     s_UsingCustomResource;
            static bool                     s_DemoInitialized;
            static int                      s_TwainDSMSearchOrder;
			static bool						s_multipleThreads;

            static std::unordered_set<HWND>   s_appWindowsToDisable;
            bool                            m_bOpenSourceOnSelect;
            static bool                     s_bCheckHandles;
            static bool                     s_bQuerySupport;
            static DTWAIN_DIBUPDATE_PROC    s_pDibUpdateProc;
            static std::deque<int> s_vErrorBuffer;
            static unsigned int             s_nErrorBufferThreshold;
            static unsigned int             s_nErrorBufferReserve;
            static std::stack<unsigned long, std::deque<unsigned long> > s_vErrorFlagStack;
            static CTL_TwainMemoryFunctions*      s_TwainMemoryFunc;
            static CTL_LegacyTwainMemoryFunctions s_TwainLegacyFunc;
            static CTL_Twain2MemoryFunctions s_Twain2Func;
            static bool                     s_TwainCallbackSet;
            /*static */CTL_MAPLONGTOVECTORLONG  m_mapDTWAINArrayToTwainType;
            static CTL_IMAGEDLLINFO         s_ImageDLLInfo;
            static std::bitset<10>  g_AvailabilityFlags;
    #ifndef DTWAIN_RETAIL
    //        static PROCESS_INFORMATION      s_ProcessInfo;
    #endif

            /////////////////////////////////////////////////////////////////////////////
            // protection
           #ifdef WIN32
                static  CRITICAL_SECTION        s_critLogCall;
                static  CRITICAL_SECTION        s_critFileCreate;
                static  CRITICAL_SECTION        s_critStaticInit;
                static  bool                    s_bCritSectionCreated;
                static  bool                    s_bFileCritSectionCreated;
                static  bool                    s_bCritStaticCreated;

           #endif
    };

    template <typename T>
    T IsDLLHandleValid(CTL_TwainDLLHandle *pHandle, T bCheckSession = T(1))
    {
        // See if DLL Handle exists
        if (!pHandle)
            return T(0);
        if (std::find_if(CTL_TwainDLLHandle::s_DLLHandles.begin(),
            CTL_TwainDLLHandle::s_DLLHandles.end(), SmartPointerFinder<CTL_TwainDLLHandlePtr>(pHandle)) ==
            CTL_TwainDLLHandle::s_DLLHandles.end())
            return T(0);
        if (!pHandle->m_bSessionAllocated && bCheckSession)
            return T(0);
        return T(1);
    }

    CTL_TwainDLLHandle* FindHandle(HWND hWnd, bool bIsDisplay);
    CTL_TwainDLLHandle* FindHandle(HINSTANCE hInst);
    CTL_ITwainSource* VerifySourceHandle( DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source );



    int GetResolutions(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source, void* pArray,
                       CTL_EnumGetType GetType);
    bool GetImageSize( DTWAIN_HANDLE DLLHandle,
                       DTWAIN_SOURCE Source,
                       double FAR *pLeft,
                       double FAR *pRight,
                       double FAR *pTop,
                       double FAR *pBottom,
                       CTL_EnumGetType GetType);

    bool SetImageSize( DTWAIN_HANDLE DLLHandle,
                       DTWAIN_SOURCE Source,
                       double dLeft,
                       double dRight,
                       double dTop,
                       double dBottom,
                       CTL_EnumSetType SetType,
                       std::vector<double>& rArray);

    bool GetNativeResolution(DTWAIN_HANDLE DLLHandle,
                             DTWAIN_SOURCE Source,
                             double *pRes,
                             CTL_EnumCapability Cap);

    int SetResolutions(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source, void** pResolutions,
                        int nRes, void (*ResProc)(const CTL_ITwainSource *pSource,
                                                  std::vector<double>& pArray ));
    bool CenterWindow(HWND hwnd, HWND hwndParent);

    bool IsIntCapType(TW_UINT16 nCap);
    bool IsFloatCapType(TW_UINT16 nCap);
    bool IsStringCapType(TW_UINT16 nCap);
    bool IsFrameCapType(TW_UINT16 nCap);

    DTWAIN_BOOL    DTWAIN_ArrayFirst(DTWAIN_ARRAY pArray, LPVOID pVariant);

    DTWAIN_BOOL    DTWAIN_ArrayNext(DTWAIN_ARRAY pArray, LPVOID pVariant);
    LONG           DTWAIN_ArrayType(DTWAIN_ARRAY pArray, bool bGetReal=true);
    bool           DTWAINFRAMEToTWFRAME(DTWAIN_FRAME pDdtwil, pTW_FRAME pTwain);
    bool           TWFRAMEToDTWAINFRAME(const TW_FRAME& pTwain, DTWAIN_FRAME pDdtwil);

    #ifdef __cplusplus
    extern "C" {
    #endif
    #ifdef _WIN32
    LRESULT CALLBACK_DEF DTWAIN_WindowProc(HWND hWnd,
                                               UINT uMsg,
                                               WPARAM wParam,
                                               LPARAM lParam);

    LRESULT CALLBACK_DEF DTWAIN_GetMessageProc(int nCode, WPARAM wParam, LPARAM lParam );
    LRESULT CALLBACK_DEF DTWAIN_MessageProc(int nCode, WPARAM wParam, LPARAM lParam );
    DTWAIN_BOOL DTWAIN_SetCallbackProc( DTWAIN_CALLBACK fnCall, LONG nWhich);
    #endif
    void DTWAIN_AcquireProc(DTWAIN_HANDLE DLLHandle, DTWAIN_SOURCE Source, WPARAM Data1, LPARAM Data2);
    #ifdef __cplusplus
    }
    #endif

    void DTWAIN_InvokeCallback( int nWhich, DTWAIN_HANDLE pHandle, DTWAIN_SOURCE pSource, WPARAM lData1, LPARAM lData2 );

    DTWAIN_BOOL    DTWAIN_SetSourceCloseMode(LONG lCloseMode);
    LONG    DTWAIN_GetSourceCloseMode();
    DTWAIN_BOOL DTWAIN_GetAllSourceDibs(DTWAIN_SOURCE Source, DTWAIN_ARRAY pArray);

    void OutputDTWAINError(CTL_TwainDLLHandle *pHandle, LPCTSTR pFunc=NULL);
    void OutputDTWAINErrorA(CTL_TwainDLLHandle *pHandle, LPCSTR pFunc=NULL);
    void LogExceptionError(LPCTSTR fname);
    void LogExceptionErrorA(LPCSTR fname);
    void LogDTWAINMessage(HWND, UINT, WPARAM, LPARAM, bool bCallback=false);
    bool UserDefinedLoggerExists();
    void WriteUserDefinedLogMsg(LPCTSTR sz);

    //bool IsSupported(DTWAIN_SOURCE Source, LONG SupportVal, LONG Cap, bool bEnumSupported=true);
    //bool SetSupport(DTWAIN_SOURCE Source, LPVOID SupportVal, LONG Cap, bool bSetCurrent);
    //int  GetSupport(DTWAIN_SOURCE Source, LPVOID lpSupport, LONG Cap, LONG GetType, bool bEnumSupported=true);
    bool GetSupportString(DTWAIN_SOURCE Source, LPTSTR sz, LONG nLen, LONG Cap, LONG GetType);
    bool EnumSupported(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG Cap);
    LONG CheckEnabled(DTWAIN_SOURCE Source, LONG CapVal);
    bool SetSupportArray(DTWAIN_SOURCE Source, DTWAIN_ARRAY Array, LONG Cap);
    bool GetSupportArray(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Array, LONG Cap, LONG GetType=DTWAIN_CAPGET);
    bool GetOneEnabledSupport(DTWAIN_SOURCE Source, LONG Cap);
    CTL_StringType& GetDTWAINTempFilePath();
    size_t GetResourceString(UINT nError, LPTSTR buffer, LONG bufSize);
    bool LoadLanguageResourceXML(LPCTSTR sLangDLL);
    bool LoadLanguageResourceXMLImpl(LPCTSTR szFile);
    void LoadStringsInMap();
    void DumpArrayContents(DTWAIN_ARRAY Array, LONG lCap);
    void DumpArrayContents(DTWAIN_ARRAY Array);
    void LogWin32Error(DWORD lastError);
    void LoadOCRInterfaces(CTL_TwainDLLHandle *pHandle);
    void UnloadOCRInterfaces(CTL_TwainDLLHandle *pHandle);
    LONG CopyInfoToCString(const CTL_StringType& strInfo, LPTSTR szInfo, LONG nMaxLen);
    CTL_StringType GetVersionString();
    CTL_StringType GetDTWAINExecutionPath();
    DTWAIN_ARRAY DTWAIN_CreateFix32ArrayFromFloatArray(DTWAIN_ARRAY FloatArray);
    DTWAIN_ARRAY DTWAIN_CreateFloatArrayFromFix32Array(DTWAIN_ARRAY Fix32Array);

    typedef DTWAIN_BOOL (DLLENTRY_DEF *SetByStringFn)(DTWAIN_SOURCE, DTWAIN_FLOAT);
    typedef DTWAIN_BOOL (DLLENTRY_DEF *SetByStringFn2)(DTWAIN_SOURCE, DTWAIN_FLOAT, DTWAIN_BOOL);
    typedef DTWAIN_BOOL(DLLENTRY_DEF *GetByStringFn)(DTWAIN_SOURCE, LPDTWAIN_FLOAT);
    typedef DTWAIN_BOOL (*CapSetterByStringFn)(DTWAIN_SOURCE, LPCTSTR, SetByStringFn);


    DTWAIN_BOOL DTWAIN_SetDeviceCapByString(DTWAIN_SOURCE Source, LPCTSTR strVal, SetByStringFn fn);
    DTWAIN_BOOL DTWAIN_GetDeviceCapByString(DTWAIN_SOURCE Source, LPTSTR strVal, GetByStringFn fn);
    DTWAIN_BOOL DTWAIN_SetDeviceCapByString2(DTWAIN_SOURCE Source, LPCTSTR strVal, bool bExtra, SetByStringFn2 fn);

    DTWAIN_BOOL DTWAIN_CacheCapabilityInfo(CTL_ITwainSource *p, CTL_TwainDLLHandle *pHandle, TW_UINT16 nCapToCache);
    DTWAIN_BOOL DTWAIN_CacheCapabilityInfo(CTL_ITwainSource *pSource, CTL_TwainDLLHandle *pHandle, CTL_EnumeratorNode<LONG>::container_pointer_type vCaps);
    void DTWAIN_CollectCapabilityInfo(CTL_ITwainSource *p, TW_UINT16 nCap, CTL_CapInfoArray& pArray);
    CTL_CapInfoArrayPtr GetCapInfoArray(CTL_TwainDLLHandle* pHandle, CTL_ITwainSource *p);
    DTWAIN_SOURCE SourceSelect(const SourceSelectionOptions& options);
    DTWAIN_ARRAY  SourceAcquire(SourceAcquireOptions& opts);
    DTWAIN_ARRAY SourceAcquireWorkerThread(SourceAcquireOptions& opts);
    bool AcquireExHelper(SourceAcquireOptions& opts);
    DTWAIN_ACQUIRE  LLAcquireImage(SourceAcquireOptions& opts);
    DTWAIN_HANDLE GetDTWAINHandle_Internal();
    bool TileModeOn(DTWAIN_SOURCE Source);

    //#ifdef DTWAIN_DEBUG_CALL_STACK
    CTL_StringType CTL_LogFunctionCall(LPCTSTR pFuncName, int nWhich, LPCTSTR pOptionalString=NULL);
    CTL_StringType CTL_LogFunctionCallA(LPCSTR pFuncName, int nWhich, LPCSTR pOptionalString = NULL);
    //#endif

    // outputs parameter and return values
    class ParamOutputter
    {
        CTL_StringArrayType sv;
        size_t nWhich;
        CTL_StringType argNames;
        CTL_StringStreamType strm;
        bool m_bIsReturnValue;

    public:
        ParamOutputter(const CTL_StringType& s, bool isReturnValue = false) : nWhich(0), m_bIsReturnValue(isReturnValue)
        {
            StringWrapper::Tokenize(s, _T("(, )"), sv);
            if (!m_bIsReturnValue)
                strm << _T("(");
            else
                strm << s << _T(" ") << CTL_TwainDLLHandle::s_ResourceStrings[IDS_LOGMSG_RETURNTEXT] << _T(" ");
        }

        template <typename T, typename ...P>
        ParamOutputter& outputParam(T t, P ...p)
        {
            if (sv.empty() && !m_bIsReturnValue)
                return *this;
            bool bIsNull = (std::is_pointer<T>::value && !t);
            if (!m_bIsReturnValue)
            {
                if ( !bIsNull )
                    strm << sv[nWhich] << _T("=") << t;
                else
                    strm << sv[nWhich] << _T("=") << _T("(null)");
            }
            else
            {
                if ( bIsNull )
                    strm << _T("(null)");
                else
                    strm << t;
            }
            if (!m_bIsReturnValue)
            {
              if (nWhich < sv.size() - 1)
                strm << _T(", ");
              else
                strm << _T(")");
            }
            ++nWhich;
            if (sizeof...(p))
                outputParam(p...);
            return *this;
        }

        ParamOutputter& outputParam()
        {
            strm << _T(")"); return *this;
        }

        CTL_StringType getString() { return strm.str(); }
    };

    struct DTWAINArray_DestroyTraits
    {
        static void Destroy(DTWAIN_ARRAY a)
        {
            if (a)
                DTWAIN_ArrayDestroy(a);
        }
    };

    struct DTWAINArrayPtr_DestroyTraits
    {
        static void Destroy(DTWAIN_ARRAY* a)
        {
            if (a && *a)
                DTWAIN_ArrayDestroy(*a);
        }
    };

    struct DTWAINGlobalHandle_CloseTraits
    {
        static void Destroy(HANDLE h)
        {
            #ifdef _WIN32
            if ( h )
                GlobalUnlock(h);
            #endif
        }
    };

    struct DTWAINGlobalHandle_ClosePtrTraits
    {
        static void Destroy(HANDLE* h)
        {
            #ifdef _WIN32
            if (h && *h)
                GlobalUnlock(*h);
            #endif
        }
    };

    struct DTWAINGlobalHandle_CloseFreeTraits
    {
        static void Destroy(HANDLE h)
        {
            #ifdef _WIN32
            if (h)
            {
                GlobalUnlock(h);
                GlobalFree(h);
            }
            #endif
        }
    };

    struct DTWAINFrame_DestroyTraits
    {
        static void Destroy(DTWAIN_FRAME f)
        {
            if (f)
                DTWAIN_FrameDestroy(f);
        }
    };

    struct DTWAINGlobalHandle_ReleaseDCTraits
    {
        static void Destroy(std::pair<HWND, HDC> val)
        {
            #ifdef _WIN32
            if (val.second)
                ::ReleaseDC(val.first, val.second);
            #endif
        }
    };

    struct DTWAINFileHandle_CloseTraits
    {
        static void Destroy(HANDLE h)
        {
            #ifdef _WIN32
            if (h)
                ::CloseHandle(h);
            #endif
        }
    };

    struct DTWAINResource_UnlockFreeTraits
    {
        static void Destroy(HGLOBAL h)
        {
            #ifdef _WIN32
            if (h)
                FreeResource(h);
            #endif
        }
    };

    struct DTWAINResource_DeleteObjectTraits
    {
        static void Destroy(HBITMAP* h)
        {
#ifdef _WIN32
            if (h && *h)
                ::DeleteObject(*h);
#endif
        }
    };

    // RAII Class for DTWAIN_ARRAY
    typedef DTWAIN_RAII<DTWAIN_ARRAY, DTWAINArray_DestroyTraits> DTWAINArray_RAII;
    typedef DTWAIN_RAII<DTWAIN_ARRAY*, DTWAINArrayPtr_DestroyTraits> DTWAINArrayPtr_RAII;
    typedef DTWAIN_RAII<DTWAIN_FRAME, DTWAINFrame_DestroyTraits> DTWAINFrame_RAII;
    typedef DTWAIN_RAII<HANDLE, DTWAINGlobalHandle_CloseTraits> DTWAINGlobalHandle_RAII;
    typedef DTWAIN_RAII<HANDLE*, DTWAINGlobalHandle_ClosePtrTraits> DTWAINGlobalHandlePtr_RAII;
    typedef DTWAIN_RAII<HANDLE, DTWAINGlobalHandle_CloseFreeTraits> DTWAINGlobalHandleUnlockFree_RAII;
    typedef DTWAIN_RAII<std::pair<HWND, HDC>, DTWAINGlobalHandle_ReleaseDCTraits> DTWAINDeviceContextRelease_RAII;
    typedef DTWAIN_RAII<HANDLE, DTWAINFileHandle_CloseTraits> DTWAINFileHandle_RAII;
    typedef DTWAIN_RAII<HGLOBAL, DTWAINResource_UnlockFreeTraits> DTWAINResourceUnlockFree_RAII;
    typedef DTWAIN_RAII<HBITMAP*, DTWAINResource_DeleteObjectTraits> DTWAINHBITMAPFree_RAII;

    // RAII Class for turning on/off logging locally
    struct DTWAINScopedLogController
    {
        long m_ErrorFilterFlags;
        DTWAINScopedLogController(long newFilter) : m_ErrorFilterFlags(CTL_TwainDLLHandle::s_lErrorFilterFlags)
        { CTL_TwainDLLHandle::s_lErrorFilterFlags = newFilter; }
        ~DTWAINScopedLogController() { CTL_TwainDLLHandle::s_lErrorFilterFlags = m_ErrorFilterFlags; }
    };

    struct LogTraitsOff
    { static long Apply(long turnOff) { return CTL_TwainDLLHandle::s_lErrorFilterFlags &~turnOff; } };

    struct LogTraitsOn
    { static long Apply(long turnOn) { return CTL_TwainDLLHandle::s_lErrorFilterFlags  | turnOn; } };

    template <typename LogTraits>
    struct DTWAINScopedLogControllerEx
    {
        DTWAINScopedLogController m_controller;
        DTWAINScopedLogControllerEx(long newValue) : m_controller(LogTraits::Apply(newValue)) {}
    };

    template <typename T, bool Value=false>
    struct NotImpl
    { bool operator !() const { return Value; } };

    typedef DTWAINScopedLogControllerEx<LogTraitsOff> DTWAINScopedLogControllerExclude;
    typedef DTWAINScopedLogControllerEx<LogTraitsOn>  DTWAINScopedLogControllerInclude;

    #ifdef USE_EXCEPTION_SPEC
        #define THIS_FUNCTION_PROTO_THROWS throw(...);
        #define THIS_FUNCTION_THROWS throw(...)
    #else
        #define THIS_FUNCTION_PROTO_THROWS  ;
        #define THIS_FUNCTION_THROWS
    #endif

    void  DTWAIN_InternalThrowException() THIS_FUNCTION_PROTO_THROWS

    LONG  TS_Command(LPCTSTR lpCommand);

    #include <funcmac.h>

    #define IDS_DTWAIN_APPTITLE       9700

        #define IDS_LIMITEDFUNCMSG1     8894
        #define IDS_LIMITEDFUNCMSG2     8895
        #define IDS_LIMITEDFUNCMSG3     8896


    #define CHECK_FOR_PDF_TYPE() \
        (lFileType == DTWAIN_PDF) || \
        (lFileType == DTWAIN_PDFMULTI) || \
        (lFileType == DTWAIN_POSTSCRIPT1) || \
        (lFileType == DTWAIN_POSTSCRIPT2) || \
        (lFileType == DTWAIN_POSTSCRIPT3) || \
        (lFileType == DTWAIN_POSTSCRIPT1MULTI) || \
        (lFileType == DTWAIN_POSTSCRIPT2MULTI) || \
        (lFileType == DTWAIN_POSTSCRIPT3MULTI)

    #define INVALID_LICENSE (0)
}
#endif
