#include <windows.h>
#include <tchar.h>
#include "jni.h"
#include "DTwainJavaAPI.h"
#include <map>
#include <string>
#include <memory>
#include <vector>
#include <algorithm>
#include <sstream>
#include <numeric>
#include <iterator>
#include "dtwain.h"
#include "twain.h"
#include "JavaAdapter.h"
#pragma warning (disable:4297)
#pragma warning (disable:4715)
#pragma warning (disable:4996)
#include "UTFCharsHandler.h"
#include "DTWAINRAII.h"
#include "DTWAINGlobalFN.h"
#include "JavaArrayTraits.h"
#include "javaobjectcaller.h"
#include "DTWAINFunctionCaller.h"
#include "DTWAINJNIGlobals.h"

#define NAME_TO_STRING(x) #x
#define ADD_FUNCTION_ENTRY(m, fName) (m)->m_FnMap[NAME_TO_STRING(fName)] = fName

DTWAINJNIGlobals g_JNIGlobals;

#define DTWAIN_TRY try {
#define DTWAIN_CATCH(env)   }  catch (...) {\
                                jclass clazz = env->FindClass("com/dynarithmic/twain/DTwainJavaAPIException");\
                                jmethodID mid = env->GetMethodID(clazz, "<init>", "()V");\
                                jthrowable throwObj = (jthrowable)env->NewObject(clazz, mid);\
                                env->Throw(throwObj);\
                                }
bool IsModuleInitialized()
{
	return  g_JNIGlobals.g_DTWAINModule?true:false;
}

void AddToFunctionCounter(const std::string& fname)
{
    DTWAINJNIGlobals::FunctionCounterMap::iterator it = g_JNIGlobals.g_functionCounter.find(fname);
    if ( it == g_JNIGlobals.g_functionCounter.end())
        it = g_JNIGlobals.g_functionCounter.insert(make_pair(fname, 0)).first;
    it->second++;        
}

void CheckForDuplicateCalls()
{
   bool bDuplicateFound = false;
   DTWAINJNIGlobals::FunctionCounterMap::iterator it = g_JNIGlobals.g_functionCounter.begin();
   while (it != g_JNIGlobals.g_functionCounter.end())
   {
        if (it->second == 0)
        {
            std::ostringstream strm;
            strm << it->first << " not called " << "\n";
            OutputDebugStringA(strm.str().c_str());
        }
        else
        if (it->second > 1)
        {
            bDuplicateFound = true;
            std::ostringstream strm;
            strm << it->first << " called multiple times! (" << it->second << ")\n";
            OutputDebugStringA(strm.str().c_str());
        }
        ++it;
   }
   if ( !bDuplicateFound )
      OutputDebugStringA("No duplicate DTWAIN calls found\n");
}

jobjectArray CallFnReturnStringArray(JNIEnv* env, DTWAIN_SOURCE src, DTWAINFN_LSa fn)
{
	if ( IsModuleInitialized() )
	{
		DTWAIN_ARRAY arr = 0;
		BOOL bRet = fn((DTWAIN_SOURCE)src, &arr);
		DTWAINArray_RAII res(arr);
		if ( arr )
			return CreateJStringArrayFromDTWAIN(env, arr);
	}
	return CreateJStringArrayFromDTWAIN(env, 0);
}

class JavaCallback;
typedef std::shared_ptr<JavaCallback> JavaCallbackPtr;

class JavaCallback : public JavaAdapter
{
    public:
    #ifdef _WIN64
        typedef LONG_PTR callback_type;
    #else
        typedef LONG callback_type;
    #endif
        static JavaCallback* m_pThisObject;

        struct JCallbackInfo
        {
            const char* m_jClassName;
            const char* m_jFunctionName;
            const char* m_jFunctionSig;
            jclass m_jCallbackClass;
            jmethodID m_jCallbackMethodID;
        };
        
        JCallbackInfo m_jCallbackInfo[2];

        enum { TWAINLISTENERFN, LOGGERFN };

    public:
        // these functions are sent to DTWAIN for the callback setup
        static LRESULT CALLBACK DTWAINCallback(WPARAM w, LPARAM l, callback_type This);

        static void CALLBACK DTWAINLoggerCallback(LPCTSTR str);

        // Constructor        
        JavaCallback(JNIEnv * pEnv);
};


JavaCallback *pNewCallback = NULL;
JavaCallbackPtr g_pDTwainAPICallback;
JavaCallback* JavaCallback::m_pThisObject = NULL;

struct JavaCallbackWrapper
{
	JavaCallbackPtr m_pCallBack;
	JavaCallbackWrapper(JavaCallbackPtr& p) : m_pCallBack(p) {}
	~JavaCallbackWrapper() 
	{ 
		m_pCallBack->releaseJNIEnv(); 
	}
};

LRESULT CALLBACK JavaCallback::DTWAINCallback(WPARAM w, LPARAM l, JavaCallback::callback_type This)
{
    // First, get back the struct
	JavaCallbackPtr pCallback = g_pDTwainAPICallback;
	if ( !pCallback )
		return 1;

    // Now call the Java method
    JavaCallback::JCallbackInfo& pCallInfo = pCallback->m_jCallbackInfo[TWAINLISTENERFN];
	JavaCallbackWrapper wrapper(pCallback);
    JNIEnv* pEnv = pCallback->getJNIEnv();
    int retval = pEnv->CallStaticIntMethod(pCallInfo.m_jCallbackClass, pCallInfo.m_jCallbackMethodID, w, (jlong)l); 
    return (callback_type)retval;
}

void CALLBACK JavaCallback::DTWAINLoggerCallback(LPCTSTR str)
{
    JavaCallbackPtr pCallback = g_pDTwainAPICallback;
    if ( !pCallback )
        return;

    // Now call the Java method
    JavaCallback::JCallbackInfo& pCallInfo = pCallback->m_jCallbackInfo[LOGGERFN];
	JavaCallbackWrapper wrapper(pCallback);
    JNIEnv* pEnv = pCallback->getJNIEnv();
    pEnv->CallStaticVoidMethod(pCallInfo.m_jCallbackClass, 
                               pCallInfo.m_jCallbackMethodID, 
                               CreateJStringFromCString(pEnv, str));
}

JavaCallback::JavaCallback(JNIEnv * pEnv) : JavaAdapter(pEnv)
{
    Init(pEnv);
    const JCallbackInfo jCallInfo[] = { {"com/dynarithmic/twain/DTwainJavaAPIListener", "onTwainEvent", "(IJ)I" ,0, 0,},
                                        {"com/dynarithmic/twain/DTwainJavaAPILogger", "logEvent",  "(Ljava/lang/String;)V", 0, 0 }
                                        };

    for ( int i = 0; i < 2; ++i )                                            
        memcpy(&m_jCallbackInfo[i], &jCallInfo[i], sizeof(JCallbackInfo));
        
    for (int i = 0; i < 2; ++i)
    {
        // Get the java class class
        m_jCallbackInfo[i].m_jCallbackClass = pEnv->FindClass( m_jCallbackInfo[i].m_jClassName );

        // get the method id stored in the class
        m_jCallbackInfo[i].m_jCallbackMethodID = pEnv->GetStaticMethodID(m_jCallbackInfo[i].m_jCallbackClass, 
                                                                         m_jCallbackInfo[i].m_jFunctionName,
                                                                         m_jCallbackInfo[i].m_jFunctionSig);
    }
    m_pThisObject = this;
}


void InitializeCallbacks(JNIEnv *env)
{
    if (!g_pDTwainAPICallback )
    {
        g_pDTwainAPICallback = JavaCallbackPtr(new JavaCallback(env));
    }
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1LoadLibrary
(JNIEnv *env, jobject, jstring dllToLoad)
{
    DTWAIN_TRY
    GetStringCharsHandler handler(env, dllToLoad);
    LPCTSTR s = reinterpret_cast<LPCTSTR>(handler.GetStringChars());
    if ( !g_JNIGlobals.g_DTWAINModule )
    {
        g_JNIGlobals.g_DTWAINModule = LoadLibrary(s);
        if ( g_JNIGlobals.g_DTWAINModule )
            InitializeCallbacks(env);
    }
    return g_JNIGlobals.g_DTWAINModule?1:0;        
    DTWAIN_CATCH(env)
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1FreeLibrary
(JNIEnv *env, jobject)
{
    DTWAIN_TRY
    if ( g_JNIGlobals.g_DTWAINModule )
    {
        CheckForDuplicateCalls();
        FreeLibrary(g_JNIGlobals.g_DTWAINModule);
        g_JNIGlobals.g_DTWAINModule = NULL;
    }
    return 1;
    DTWAIN_CATCH(env)
}


JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsTwainAvailable
  (JNIEnv *env, jobject)
  {
     DTWAIN_TRY
	 return DTWAIN_FUNCTION_CALLER0(DTWAIN_IsTwainAvailable, LV)?JNI_TRUE:JNI_FALSE;
     DTWAIN_CATCH(env)
  }

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SysInitialize
 * Signature: ()I
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SysInitialize
  (JNIEnv *env, jobject)
  {
      DTWAIN_TRY
      jlong retValue = (jlong)DTWAIN_FUNCTION_CALLER0(DTWAIN_SysInitialize,HandleV);
      if ( retValue )                                                
      {
          // set the callback for the DTWAIN logger
          DTWAIN_SetLoggerCallback(JavaCallback::DTWAINLoggerCallback);
		  JavaCallbackPtr pCallback = g_pDTwainAPICallback;
		  if ( pCallback )
			  DTWAIN_SetCallback(JavaCallback::DTWAINCallback, reinterpret_cast<JavaCallback::callback_type>(pCallback.get()));
      }
      return retValue;
      DTWAIN_CATCH(env)
  }

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SysDestroy
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SysDestroy
  (JNIEnv *env, jobject)
  {
      DTWAIN_TRY
      BOOL bRetVal = DTWAIN_FUNCTION_CALLER0(DTWAIN_SysDestroy, LV);
	  if ( bRetVal )
		  g_JNIGlobals.g_CurrentAcquireMap.clear();
	  return bRetVal;
      DTWAIN_CATCH(env)
  }

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SelectSource
 * Signature: ()I
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SelectSource
  (JNIEnv *env, jobject)
  {
      DTWAIN_TRY
      return (jlong)DTWAIN_FUNCTION_CALLER0(DTWAIN_SelectSource, SV);
      DTWAIN_CATCH(env)
  }

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SelectDefaultSource
 * Signature: ()I
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SelectDefaultSource
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return (jlong)DTWAIN_FUNCTION_CALLER0(DTWAIN_SelectDefaultSource, SV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetLastError
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetLastError
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_GetLastError,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetTwainMode
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetTwainMode
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_GetTwainMode,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsSessionEnabled
 * Signature: ()Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsSessionEnabled
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_IsSessionEnabled,LV)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EndTwainSession
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EndTwainSession
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_EndTwainSession,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCountry
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCountry
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_GetCountry,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetLanguage
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetLanguage
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_GetLanguage,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsMsgNotifyEnabled
 * Signature: ()Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsMsgNotifyEnabled
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_IsMsgNotifyEnabled,LV)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetTwainHwnd
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetTwainHwnd
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return (jint)DTWAIN_FUNCTION_CALLER0(DTWAIN_GetTwainHwnd,HV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAcquiring
 * Signature: ()Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAcquiring
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_IsAcquiring,LV)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_CreateAcquisitionArray
 * Signature: ()I
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1CreateAcquisitionArray
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return (jlong)DTWAIN_FUNCTION_CALLER0(DTWAIN_CreateAcquisitionArray,AV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ClearErrorBuffer
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ClearErrorBuffer
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_ClearErrorBuffer,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetErrorBufferThreshold
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetErrorBufferThreshold
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_GetErrorBufferThreshold,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_InitOCRInterface
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1InitOCRInterface
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_InitOCRInterface,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SelectOCREngine
 * Signature: ()J
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SelectOCREngine
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_SelectOCREngine,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SelectDefaultOCREngine
 * Signature: ()J
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SelectDefaultOCREngine
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_SelectDefaultOCREngine,LV);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetTwainAvailability
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetTwainAvailability
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER0(DTWAIN_GetTwainAvailability,LV);
    DTWAIN_CATCH(env)
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetTwainMode
  (JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetTwainMode,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCountry
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCountry
  (JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetCountry,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetLanguage
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetLanguage
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetLanguage,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableMsgNotify
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableMsgNotify
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_EnableMsgNotify,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_OpenSourcesOnSelect
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1OpenSourcesOnSelect
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_OpenSourcesOnSelect,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetQueryCapSupport
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetQueryCapSupport
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetQueryCapSupport,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetTwainTimeout
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetTwainTimeout
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
		return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetTwainTimeout,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetErrorBufferThreshold
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetErrorBufferThreshold
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetErrorBufferThreshold,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_AppHandlesExceptions
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1AppHandlesExceptions
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_AppHandlesExceptions,LL,a1);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetTwainDSM
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetTwainDSM
(JNIEnv *env, jobject, jint a1)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetTwainDSM,LL,a1);
    DTWAIN_CATCH(env)
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1OpenSource
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_OpenSource, LS, (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_CloseSource
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1CloseSource
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    BOOL bRetVal = DTWAIN_FUNCTION_CALLER1(DTWAIN_CloseSource,LS,(DTWAIN_SOURCE)src);
	if ( bRetVal )
		g_JNIGlobals.g_CurrentAcquireMap.erase((DTWAIN_SOURCE)src);
	return bRetVal;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_CloseSourceUI
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1CloseSourceUI
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_CloseSourceUI,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetDefaultSource
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetDefaultSource
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetDefaultSource,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsSourceAcquiring
 * Signature: (J)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsSourceAcquiring
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsSourceAcquiring,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsSourceOpen
 * Signature: (J)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsSourceOpen
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsSourceOpen,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAllCapsToDefault
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAllCapsToDefault
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetAllCapsToDefault,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCurrentPageNum
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCurrentPageNum
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetCurrentPageNum,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetMaxAcquisitions
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetMaxAcquisitions
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetMaxAcquisitions,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetMaxPagesToAcquire
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetMaxPagesToAcquire
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetMaxPagesToAcquire,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsUIControllable
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsUIControllable
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsUIControllable,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsUIEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsUIEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsUIEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsIndicatorSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsIndicatorSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsIndicatorSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsIndicatorEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsIndicatorEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsIndicatorEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsThumbnailSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsThumbnailSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsThumbnailSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsThumbnailEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsThumbnailEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsThumbnailEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsDeviceEventSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsDeviceEventSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsDeviceEventSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsUIOnlySupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsUIOnlySupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsUIOnlySupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ShowUIOnly
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ShowUIOnly
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_ShowUIOnly,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsPrinterSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsPrinterSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsPrinterSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsFeederEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsFeederEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsFeederEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsFeederLoaded
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsFeederLoaded
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsFeederLoaded,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsFeederSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsFeederSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsFeederSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsFeederSensitive
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsFeederSensitive
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsFeederSensitive,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_FeedPage
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1FeedPage
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_FeedPage,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_RewindPage
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1RewindPage
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_RewindPage,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ClearPage
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ClearPage
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_ClearPage,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAutoFeedEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoFeedEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoFeedEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAutoFeedSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoFeedSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoFeedSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetFeederFuncs
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetFeederFuncs
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetFeederFuncs,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsPaperDetectable
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsPaperDetectable
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsPaperDetectable,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsDuplexSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsDuplexSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsDuplexSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsDuplexEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsDuplexEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsDuplexEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsCustomDSDataSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsCustomDSDataSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsCustomDSDataSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ClearPDFText
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ClearPDFText
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_ClearPDFText,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAutoDeskewSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoDeskewSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoDeskewSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAutoDeskewEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoDeskewEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoDeskewEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoBorderDetectSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoBorderDetectSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAutoBorderDetectEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoBorderDetectEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoBorderDetectEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsLightPathSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsLightPathSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsLightPathSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsLampSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsLampSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsLampSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsLampEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsLampEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsLampEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsLightSourceSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsLightSourceSupported
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsLightSourceSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetMaxRetryAttempts
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetMaxRetryAttempts
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetMaxRetryAttempts,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCurrentRetryCount
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCurrentRetryCount
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetCurrentRetryCount,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsSkipImageInfoError
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsSkipImageInfoError
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsSkipImageInfoError,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsExtImageInfoSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsExtImageInfoSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsExtImageInfoSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_InitExtImageInfo
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1InitExtImageInfo
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_InitExtImageInfo,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetExtImageInfo
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetExtImageInfo
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetExtImageInfo,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_FreeExtImageInfo
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1FreeExtImageInfo
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_FreeExtImageInfo,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_FlushAcquiredPages
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1FlushAcquiredPages
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_FlushAcquiredPages,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsFileSystemSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsFileSystemSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsFileSystemSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetBlankPageAutoDetection
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetBlankPageAutoDetection
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetBlankPageAutoDetection,LS,(DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsBlankPageDetectionOn
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsBlankPageDetectionOn
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsBlankPageDetectionOn,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAutoScanEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoScanEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoScanEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsDeviceOnLine
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsDeviceOnLine
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsDeviceOnLine,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}


JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoBrightEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoBrightEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsAutoRotateEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsAutoRotateEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsAutoRotateEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}
/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsRotationSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsRotationSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsRotationSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}
/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsPatchCapsSupported
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsPatchCapsSupported
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsPatchCapsSupported,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsPatchDetectEnabled
 * Signature: (J)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsPatchDetectEnabled
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsPatchDetectEnabled,LS,(DTWAIN_SOURCE)src)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumSources
 * Signature: ()[J
 */
 
JNIEXPORT jlongArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumSources
(JNIEnv *env, jobject)
{
    DTWAIN_TRY
    DTWAIN_ARRAY A=0;
    BOOL bRet = DTWAIN_EnumSources(&A);
    DTWAINArray_RAII arr(A);    
    return CreateJArrayFromDTWAINArray<JavaLongArrayTraits>(env, A, bRet?true:false);
    DTWAIN_CATCH(env)
}

JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumSupportedCaps
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumSupportedCaps), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumExtendedCaps
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumExtendedCaps
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
                            (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumExtendedCaps), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumCustomCaps
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumCustomCaps
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumCustomCaps), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumSourceUnits
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumSourceUnits
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumSourceUnits), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumFileXferFormats
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumFileXferFormats
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumFileXferFormats), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumCompressionTypes
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumCompressionTypes
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumCompressionTypes), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPrinterStringModes
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPrinterStringModes
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPrinterStringModes), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumTwainPrintersArray
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumTwainPrintersArray
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumTwainPrintersArray), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumOrientations
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumOrientations
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumOrientations), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPaperSizes
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPaperSizes
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPaperSizes), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPixelTypes
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPixelTypes
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPixelTypes), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumBitDepths
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumBitDepths
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumBitDepths), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumJobControls
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumJobControls
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumJobControls), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumLightPaths
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumLightPaths
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumLightPaths), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumLightSources
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumLightSources
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumLightSources), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetLightSources
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetLightSources
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_GetLightSources), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumExtImageInfoTypes
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumExtImageInfoTypes
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumExtImageInfoTypes), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumAlarms
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumAlarms
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumAlarms), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumNoiseFilters
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumNoiseFilters
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumNoiseFilters), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPatchMaxRetries
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPatchMaxRetries
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPatchMaxRetries), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPatchMaxPriorities
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPatchMaxPriorities
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPatchMaxPriorities), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPatchSearchModes
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPatchSearchModes
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPatchSearchModes), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPatchTimeOutValues
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPatchTimeOutValues
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPatchTimeOutValues), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetPatchPriorities
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetPatchPriorities
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_GetPatchPriorities), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumPatchPriorities
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumPatchPriorities
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnArray1<FnGlobalLSaPtr, FnGlobalLSa, DTWAIN_SOURCE, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaMap, NAME_TO_STRING(DTWAIN_EnumPatchPriorities), (DTWAIN_SOURCE)src);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumTopCameras
 * Signature: (J)[Ljava/lang/String;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumTopCameras
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnStringArray(env, (DTWAIN_SOURCE)src, &DTWAIN_EnumTopCameras);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumBottomCameras
 * Signature: (J)[Ljava/lang/String;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumBottomCameras
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnStringArray(env, (DTWAIN_SOURCE)src, &DTWAIN_EnumBottomCameras);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumCameras
 * Signature: (J)[Ljava/lang/String;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumCameras
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    return CallFnReturnStringArray(env, (DTWAIN_SOURCE)src, &DTWAIN_EnumCameras);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetVersionInfo
 * Signature: ()Lcom/dynarithmic/twain/DTwainVersionInfo;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetVersionInfo(JNIEnv *env, jobject)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

	JavaDTwainVersionInfo vInfo(env);

	// Call the DTWAIN function to get the version information
	LONG majorV, minorV, patchV, versionType, sLength;
	BOOL bRet = DTWAIN_GetVersionEx(&majorV, &minorV, &versionType, &patchV);
	if ( bRet )
	{
		// call DTWAIN function to get the string version
		sLength = DTWAIN_GetVersionString(NULL, 0);
		std::vector<TCHAR> vChars(sLength,0);
		DTWAIN_GetVersionString(&vChars[0], sLength);

		// Call Java function to declare and init a new versionInfo object
		return vInfo.createFullObject(majorV, minorV, patchV, versionType, CreateJStringFromCString(env, &vChars[0]));
	}
	return vInfo.createDefaultObject();
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsCapSupported
 * Signature: (JI)I
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsCapSupported
  (JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsCapSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapDataType
 * Signature: (JI)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapDataType
  (JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    LONG retVal = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetCapDataType,LSL,(DTWAIN_SOURCE)arg1, arg2);
    LPTSTR ptrName;    
    switch (retVal)
    {
        case TWTY_INT8:
        case TWTY_INT16:
        case TWTY_INT32:
        case TWTY_UINT8:
        case TWTY_UINT16:
        case TWTY_UINT32:
        case TWTY_BOOL:
            ptrName = _T("int");
        break;
        
        case TWTY_FIX32:
            ptrName = _T("double");
        break;
        
        case TWTY_STR32:            
        case TWTY_STR64:            
        case TWTY_STR128:            
        case TWTY_STR255:            
        case TWTY_STR1024:
            ptrName = _T("java/Lang/String");
        break;

        case TWTY_FRAME:
            ptrName = _T("com/dynarithmic/twain/DTwainFrame");
        break;

        default:
            ptrName = _T("<unknown>");
    }
    return (jstring)CreateJStringFromCString(env, ptrName);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetMaxAcquisitions
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetMaxAcquisitions
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetMaxAcquisitions,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetSourceUnit
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetSourceUnit
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetSourceUnit,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsFileXferSupported
 * Signature: (JI)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsFileXferSupported
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsFileXferSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableIndicator
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableIndicator
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableIndicator,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}


JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsCompressionSupported
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsCompressionSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsPrinterEnabled
 * Signature: (JI)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsPrinterEnabled
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsPrinterEnabled,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableFeeder
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableFeeder
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableFeeder,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnablePrinter
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnablePrinter
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnablePrinter,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableThumbnail
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableThumbnail
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableThumbnail,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ForceAcquireBitDepth
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ForceAcquireBitDepth
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_ForceAcquireBitDepth,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAvailablePrinters
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAvailablePrinters
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetAvailablePrinters,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetDeviceNotifications
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetDeviceNotifications
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetDeviceNotifications,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPrinterStartNumber
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPrinterStartNumber
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPrinterStartNumber,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableAutoFeed
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableAutoFeed
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableAutoFeed,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableDuplex
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableDuplex
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableDuplex,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsOrientationSupported
 * Signature: (JI)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsOrientationSupported
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsOrientationSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsPaperSizeSupported
 * Signature: (JI)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsPaperSizeSupported
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsPaperSizeSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsPixelTypeSupported
 * Signature: (JI)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsPixelTypeSupported
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsPixelTypeSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFCompression
 * Signature: (JZ)I
 */
/*JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFCompression
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFCompression,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
}
*/
/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFASCIICompression
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFASCIICompression
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFASCIICompression,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPostScriptType
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPostScriptType
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPostScriptType,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFJpegQuality
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFJpegQuality
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFJpegQuality,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetTIFFInvert
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetTIFFInvert
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetTIFFInvert,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetTIFFCompressType
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetTIFFCompressType
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetTIFFCompressType,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsJobControlSupported
 * Signature: (JI)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsJobControlSupported
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsJobControlSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}
/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableJobFileHandling
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableJobFileHandling
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableJobFileHandling,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableAutoDeskew
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableAutoDeskew
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableAutoDeskew,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableAutoBorderDetect
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableAutoBorderDetect
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableAutoBorderDetect,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetLightPath
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetLightPath
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetLightPath,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableLamp
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableLamp
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableLamp,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetMaxRetryAttempts
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetMaxRetryAttempts
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetMaxRetryAttempts,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCurrentRetryCount
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCurrentRetryCount
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetCurrentRetryCount,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SkipImageInfoError
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SkipImageInfoError
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SkipImageInfoError,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetMultipageScanMode
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetMultipageScanMode
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetMultipageScanMode,LSL,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAlarmVolume
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAlarmVolume
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetAlarmVolume,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableAutoScan
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableAutoScan
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableAutoScan,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ClearBuffers
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ClearBuffers
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_ClearBuffers,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetFeederAlignment
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetFeederAlignment
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetFeederAlignment,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetFeederOrder
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetFeederOrder
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetFeederOrder,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetMaxBuffers
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetMaxBuffers
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetMaxBuffers,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsMaxBuffersSupported
 * Signature: (JI)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsMaxBuffersSupported
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsMaxBuffersSupported,LSL,(DTWAIN_SOURCE)arg1, arg2)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableAutoBright
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableAutoBright
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableAutoBright,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnableAutoRotate
 * Signature: (JZ)Z
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnableAutoRotate
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_EnableAutoRotate,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetNoiseFilter
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetNoiseFilter
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetNoiseFilter,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPixelFlavor
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPixelFlavor
(JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPixelFlavor,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetRotation
 * Signature: (JD)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetRotation
(JNIEnv *env, jobject, jlong arg1, jdouble arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetRotation,LSF,(DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetSourceUnit
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetSourceUnit,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetDeviceNotifications
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetDeviceNotifications
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetDeviceNotifications,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetDeviceEvent
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetDeviceEvent
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetDeviceEvent,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCompressionSize
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCompressionSize
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetCompressionSize,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumTwainPrinters
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumTwainPrinters
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_EnumTwainPrinters,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetPrinterStartNumber
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetPrinterStartNumber
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetPrinterStartNumber,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetDuplexType
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetDuplexType
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetDuplexType,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetLightPath
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetLightPath
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetLightPath,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetAlarmVolume
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetAlarmVolume
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetAlarmVolume,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetBatteryMinutes
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetBatteryMinutes
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetBatteryMinutes,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetBatteryPercent
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetBatteryPercent
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetBatteryPercent,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetFeederAlignment
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetFeederAlignment
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetFeederAlignment,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetFeederOrder
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetFeederOrder
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetFeederOrder,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetMaxBuffers
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetMaxBuffers
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetMaxBuffers,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetNoiseFilter
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetNoiseFilter
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetNoiseFilter,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetPixelFlavor
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetPixelFlavor
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetPixelFlavor,LSl,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return -1;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetRotation
 * Signature: (J)D
 */
JNIEXPORT jdouble JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetRotation
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    DTWAIN_FLOAT val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetRotation,LSf,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

JNIEXPORT jdouble JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetContrast
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    DTWAIN_FLOAT val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetContrast,LSf,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetBrightness
 * Signature: (J)D
 */
JNIEXPORT jdouble JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetBrightness
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    DTWAIN_FLOAT val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetBrightness,LSf,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetResolution
 * Signature: (J)D
 */
JNIEXPORT jdouble JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetResolution
(JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    DTWAIN_FLOAT val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER2(DTWAIN_GetResolution,LSf,(DTWAIN_SOURCE)src, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapArrayType
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapArrayType
  (JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_GetCapArrayType,LSL,(DTWAIN_SOURCE)arg1, (LONG)arg2);
    DTWAIN_CATCH(env)
}  


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapOperations
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapOperations
  (JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
    DTWAIN_LONG val;
    BOOL bRet = DTWAIN_FUNCTION_CALLER3(DTWAIN_GetCapOperations,LSLl,(DTWAIN_SOURCE)arg1, arg2, &val);
    if (bRet)
        return val;
    return 0;        
    DTWAIN_CATCH(env)
}

JNIEXPORT jdoubleArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumContrastValues
  (JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return CallFnReturnArray2<FnGlobalLSaBPtr, FnGlobalLSaB, DTWAIN_SOURCE, DTWAIN_BOOL, JavaDoubleArrayTraits>
        (env, g_JNIGlobals.g_LSaBMap, NAME_TO_STRING(DTWAIN_EnumContrastValues), (DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumBrightnessValues
 * Signature: (JZ)[D
 */
JNIEXPORT jdoubleArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumBrightnessValues
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return CallFnReturnArray2<FnGlobalLSaBPtr, FnGlobalLSaB, DTWAIN_SOURCE, DTWAIN_BOOL, JavaDoubleArrayTraits>
        (env, g_JNIGlobals.g_LSaBMap, NAME_TO_STRING(DTWAIN_EnumBrightnessValues), (DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumResolutionValues
 * Signature: (JZ)[D
 */
JNIEXPORT jdoubleArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumResolutionValues
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return CallFnReturnArray2<FnGlobalLSaBPtr, FnGlobalLSaB, DTWAIN_SOURCE, DTWAIN_BOOL, JavaDoubleArrayTraits>
        (env, g_JNIGlobals.g_LSaBMap, NAME_TO_STRING(DTWAIN_EnumResolutionValues), (DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}  

JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumMaxBuffers
(JNIEnv *env, jobject, jlong arg1, jboolean arg2)
{
    DTWAIN_TRY
    return CallFnReturnArray2<FnGlobalLSaBPtr, FnGlobalLSaB, DTWAIN_SOURCE, DTWAIN_BOOL, JavaIntArrayTraits>
        (env, g_JNIGlobals.g_LSaBMap, NAME_TO_STRING(DTWAIN_EnumMaxBuffers), (DTWAIN_SOURCE)arg1, arg2);
    DTWAIN_CATCH(env)
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ResetCapValues
  (JNIEnv *env, jobject, jlong arg1, jint arg2)
{
	DTWAIN_TRY
	return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetCapValues,LSLLa,(DTWAIN_SOURCE)arg1, arg2, DTWAIN_CAPRESET, (DTWAIN_ARRAY)NULL);
	DTWAIN_CATCH(env)
}


JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesInt
(JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jintArray arg4)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINArrayFromJArray<JavaIntArrayTraits>(env, arg4);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetCapValues,LSLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, aTmp);
    DTWAIN_CATCH(env)
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesDouble
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jdoubleArray arg4)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINArrayFromJArray<JavaDoubleArrayTraits>(env, arg4);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetCapValues,LSLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, aTmp);
    DTWAIN_CATCH(env)
}

DTWAIN_ARRAY CreateDTWAINStringArrayFromJArray(JNIEnv* env, jobjectArray arg, int arrayType=DTWAIN_ARRAYSTRING)
{
    jsize nCount = env->GetArrayLength(arg);
    if ( nCount >= 0 )
    {
        DTWAIN_ARRAY aTmp = DTWAIN_ArrayCreate(arrayType, nCount);
        if ( !aTmp )
            return NULL;
        jstring javaString;
        LPCTSTR cString;
        int i;
        for ( i = 0; i < nCount; ++i )
        {
            javaString = (jstring)env->GetObjectArrayElement(arg, i);
            GetStringCharsHandler handler(env, javaString);
            cString = reinterpret_cast<LPCTSTR>(handler.GetStringChars());
            if ( !cString )
                DTWAIN_ArraySetAtString(aTmp, i, _T(""));
            else
                DTWAIN_ArraySetAtString(aTmp, i, cString);
        }
        return aTmp;
    }            
    return NULL;
}
/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesString
 * Signature: (JII[Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesString
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jobjectArray arg4)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINStringArrayFromJArray(env, arg4);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetCapValues,LSLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, aTmp);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesStringEx
 * Signature: (JIII[Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesStringEx
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jobjectArray arg5)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINStringArrayFromJArray(env, arg5);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER5(DTWAIN_SetCapValuesEx,LSLLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesStringEx2
 * Signature: (JIIII[Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesStringEx2
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jint arg5, jobjectArray arg6)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINStringArrayFromJArray(env, arg6);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER6(DTWAIN_SetCapValuesEx2,LSLLLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, arg5, aTmp);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesIntEx
 * Signature: (JIII[I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesIntEx
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jintArray arg5)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINArrayFromJArray<JavaIntArrayTraits>(env, arg5);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER5(DTWAIN_SetCapValuesEx,LSLLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapContainer
 * Signature: (JII)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapContainer
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_GetCapContainer,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesDoubleEx
 * Signature: (JIII[D)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesDoubleEx
(JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jdoubleArray arg5)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINArrayFromJArray<JavaDoubleArrayTraits>(env, arg5);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER5(DTWAIN_SetCapValuesEx,LSLLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesIntEx2
 * Signature: (JIIII[I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesIntEx2
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jint arg5, jintArray arg6)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINArrayFromJArray<JavaIntArrayTraits>(env, arg6);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER6(DTWAIN_SetCapValuesEx2,LSLLLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, arg5, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesDoubleEx2
 * Signature: (JIIII[D)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesDoubleEx2
(JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jint arg5, jdoubleArray arg6)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = CreateDTWAINArrayFromJArray<JavaDoubleArrayTraits>(env, arg6);
    DTWAINArray_RAII raii(aTmp);
	return DTWAIN_FUNCTION_CALLER6(DTWAIN_SetCapValuesEx2,LSLLLLa,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, arg5, aTmp);
    return 0;
    DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesInt
 * Signature: (JII)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesInt
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER4(DTWAIN_GetCapValues,LSLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, &aTmp);
	DTWAINArray_RAII raii(aTmp);
    return CreateJArrayFromDTWAINArray<JavaIntArrayTraits>(env, aTmp);
    DTWAIN_CATCH(env)
}


jobjectArray CreateStringJArrayFromDTWAINArray(JNIEnv *env, DTWAIN_ARRAY arr)
{
    jobjectArray ret;
    LONG nCount = DTWAIN_ArrayGetCount(arr);
    nCount = (std::max)(0L, nCount);
    ret = (jobjectArray)env->NewObjectArray(nCount, env->FindClass("java/lang/String"),env->NewStringUTF(""));
    LPCTSTR Val;
	GetStringCharsHandler handler;
	handler.setEnvironment(env);
    for ( LONG i = 0; i < nCount; i++ )
    {
        Val = DTWAIN_ArrayGetAtStringPtr(arr, i);
		env->SetObjectArrayElement(ret, i, handler.GetNewJString(reinterpret_cast<const GetStringCharsHandler::char_type*>(Val)));
    }
    return ret;        
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesString
 * Signature: (JII)[Ljava/lang/String;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesString
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER4(DTWAIN_GetCapValues,LSLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateStringJArrayFromDTWAINArray(env, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesStringEx
 * Signature: (JIII)[Ljava/lang/String;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesStringEx
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER5(DTWAIN_GetCapValuesEx,LSLLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateStringJArrayFromDTWAINArray(env, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesStringEx2
 * Signature: (JIIII)[Ljava/lang/String;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesStringEx2
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jint arg5)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER6(DTWAIN_GetCapValuesEx2,LSLLLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, arg5, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateStringJArrayFromDTWAINArray(env, aTmp);
    DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesDouble
 * Signature: (JII)[D
 */
JNIEXPORT jdoubleArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesDouble
(JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER4(DTWAIN_GetCapValues,LSLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateJArrayFromDTWAINArray<JavaDoubleArrayTraits>(env, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesIntEx
 * Signature: (JIII)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesIntEx
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER5(DTWAIN_GetCapValuesEx,LSLLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateJArrayFromDTWAINArray<JavaIntArrayTraits>(env, aTmp);
    DTWAIN_CATCH(env)
}
  


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesDoubleEx
 * Signature: (JIII)[D
 */
JNIEXPORT jdoubleArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesDoubleEx
(JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER5(DTWAIN_GetCapValuesEx,LSLLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateJArrayFromDTWAINArray<JavaDoubleArrayTraits>(env, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesIntEx2
 * Signature: (JIIII)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesIntEx2
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jint arg5)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER6(DTWAIN_GetCapValuesEx2,LSLLLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, arg5, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateJArrayFromDTWAINArray<JavaIntArrayTraits>(env, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesDoubleEx2
 * Signature: (JIIII)[D
 */
JNIEXPORT jdoubleArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesDoubleEx2
(JNIEnv *env, jobject, jlong arg1, jint arg2, jint arg3, jint arg4, jint arg5)
{
    DTWAIN_TRY
    DTWAIN_ARRAY aTmp = 0;
	DTWAIN_FUNCTION_CALLER6(DTWAIN_GetCapValuesEx2,LSLLLLA,(DTWAIN_SOURCE)arg1, arg2, arg3, arg4, arg5, &aTmp);
    DTWAINArray_RAII raii(aTmp);
    return CreateJArrayFromDTWAINArray<JavaDoubleArrayTraits>(env, aTmp);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetFileXferFormat
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetFileXferFormat
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetFileXferFormat,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCompressionType
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCompressionType
  (JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetCompressionType,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPrinter
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPrinter
(JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetPrinter,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPrinterStringMode
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPrinterStringMode
(JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetPrinterStringMode,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetOrientation
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetOrientation
(JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetOrientation,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPaperSize
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPaperSize
(JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetPaperSize,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetBitDepth
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetBitDepth
(JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetBitDepth,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetJobControl
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetJobControl
(JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetJobControl,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetManualDuplexMode
 * Signature: (JIZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetManualDuplexMode
(JNIEnv *env, jobject, jlong arg1, jint arg2, jboolean arg3)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetManualDuplexMode,LSLL,(DTWAIN_SOURCE)arg1, arg2, arg3);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetAcquireArea
 * Signature: (JI)Lcom/dynarithmic/twain/DTwainAcquireArea;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetAcquireArea
  (JNIEnv *env, jobject, jlong arg1, jint arg2)
{
    DTWAIN_TRY
  if ( !g_JNIGlobals.g_DTWAINModule )
      throw "DTwain Module not loaded";

  JavaDTwainAcquireArea vArea(env);

  // Call the DTWAIN function to get the area information
  DTWAIN_ARRAY dArray = 0;
  BOOL bRet = DTWAIN_GetAcquireArea((DTWAIN_SOURCE)arg1, arg2, &dArray);
  DTWAINArray_RAII raii(dArray);
  if ( bRet )
  {
      // Get the current unit of measure
      LONG unit = DTWAIN_INCHES;
      BOOL bRet2 = DTWAIN_GetSourceUnit((DTWAIN_SOURCE)arg1, &unit);
      // Call Java function to declare and init a new acquire area object
      DTWAIN_FLOAT* pFloat = (DTWAIN_FLOAT *)DTWAIN_ArrayGetBuffer(dArray, 0);
	  return vArea.createFullObject(*pFloat, *(pFloat + 1), *(pFloat + 2), *(pFloat + 3), unit); 
  }

  return vArea.createDefaultObject();
  DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAcquireArea
 * Signature: (JILcom/dynarithmic/twain/DTwainAcquireArea;)Lcom/dynarithmic/twain/DTwainAcquireArea;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAcquireArea
  (JNIEnv* env, jobject, jlong arg1, jint arg2, jobject arg3)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

	JavaDTwainAcquireArea vArea(env);
	vArea.setObject(arg3);

	const char *fnGet[] = {"getLeft", "getTop", "getRight", "getBottom"};

    // Call the DTWAIN function to set the area information
    DTWAIN_ARRAY dSetArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYFLOAT, 4);
    DTWAIN_ARRAY dReturnArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYFLOAT, 4);
    DTWAINArray_RAII raii1(dSetArray);
    DTWAINArray_RAII raii2(dReturnArray);
    LONG srcUnit;
    if ( dSetArray && dReturnArray )
    {
        DTWAIN_FLOAT* pBuf = (DTWAIN_FLOAT*)DTWAIN_ArrayGetBuffer(dSetArray, 0);
        for (int i = 0; i < 5; ++i )
        {
            if ( i < 4 )
                *(pBuf + i) = vArea.callDoubleMethod(fnGet[i]);
            else
                srcUnit = vArea.getUnitOfMeasure();
        }                
   
        // Set the source unit first
        BOOL bRet2 = DTWAIN_SetSourceUnit((DTWAIN_SOURCE)arg1, srcUnit);
        
        pBuf = (DTWAIN_FLOAT*)DTWAIN_ArrayGetBuffer(dReturnArray, 0);
        
        BOOL bRet = DTWAIN_SetAcquireArea((DTWAIN_SOURCE)arg1, arg2, dSetArray, dReturnArray);
        if ( bRet )
        {
            // Get the current unit of measure
            LONG unit = DTWAIN_INCHES;
            BOOL bRet2 = DTWAIN_GetSourceUnit((DTWAIN_SOURCE)arg1, &unit);
            pBuf = (DTWAIN_FLOAT*)DTWAIN_ArrayGetBuffer(dReturnArray, 0);
            return vArea.createFullObject(*pBuf, *(pBuf + 1), *(pBuf + 2), *(pBuf + 3), unit); 
        }
    }        
    return vArea.createDefaultObject();
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetImageInfo
 * Signature: (J)Lcom/dynarithmic/twain/DTwainImageInfo;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetImageInfo
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

	JavaDTwainImageInfo vInfo(env);

    const char *intfields[] = { 
        	"ImageWidth", 
        	"ImageLength", 
        	"SamplesPerPixel", 
        	"BitsPerPixel", 
        	"PixelType",
        	"Compression",
        	"Planar"
    };
    const char *doublefields[] = {"XResolution", "YResolution"};

    // Call function to get the image info
    DTWAIN_FLOAT xResolution, yResolution;
    LONG imageWidth, imageLength, samplesPerPixel, bitsPerPixel, pixelType, compression, planar;
    DTWAIN_ARRAY bitsPerSample = 0;
    DTWAIN_BOOL bRet = DTWAIN_GetImageInfo((DTWAIN_SOURCE)src, 
                                            &xResolution, 
                                            &yResolution, 
                                            &imageWidth, 
                                            &imageLength,
                                            &samplesPerPixel, 
                                            &bitsPerSample,  // array
                                            &bitsPerPixel, 
                                            &planar, 
                                            &pixelType, 
                                            &compression);
    DTWAINArray_RAII raii(bitsPerSample);
    if ( bRet )
    {                                             
        LONG* intaddress[] = {&imageWidth, &imageLength, &samplesPerPixel, 
                              &bitsPerPixel, &planar, &pixelType, &compression};

        double* doubleaddress[] = {&xResolution, &yResolution};

        // get the setAll() function from DTwainAcquireArea
		const char* doublefn[] = {"setXResolution", "setYResolution"};
		const char* intfn[] = {"setImageWidth", "setImageLength", "setSamplesPerPixel", 
							   "setBitsPerPixel", "setPlanar", "setPixelType", "setCompression"};

        const int nIntFuncs = sizeof (intfn) / sizeof( intfn[0] );
        const int nDoubleFuncs = sizeof (doublefn) / sizeof( doublefn[0] );
        jobject newImageInfoObject = vInfo.defaultConstructObject(); 

		vInfo.setObject(newImageInfoObject);
        for (int i = 0; i < nIntFuncs; ++i )
            vInfo.callVoidMethod(intfn[i], *(intaddress[i]));

        for (int i = 0; i < nDoubleFuncs; ++i )
            vInfo.callVoidMethod(doublefn[i], *(doubleaddress[i]));

        jintArray jarr = CreateJArrayFromDTWAINArray<JavaIntArrayTraits>(env, bitsPerSample, 8);
        vInfo.callVoidMethod("setBitsPerSample", jarr);
        return newImageInfoObject;
    }
    return  vInfo.defaultConstructObject();
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetTwainLog
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetTwainLog
  (JNIEnv *env, jobject, jint arg1, jstring arg2)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    GetStringCharsHandler str(env, arg2);
	DTWAIN_FUNCTION_CALLER2(DTWAIN_SetTwainLog,LLt,arg1, reinterpret_cast<LPCTSTR>(str.GetStringChars()));
    return 1;
    DTWAIN_CATCH(env)
}

static int CalculateUsedPaletteEntries(int bit_count) {
	if ((bit_count >= 1) && (bit_count <= 8))
		return 1 << bit_count;
	return 0;
}


static jobject getFullImageBMPData(HANDLE hDib, JavaAcquirerInfo& jacqInfo, JNIEnv *env, bool isBMP)
{
	HandleRAII raii(hDib);
	LPBYTE pDibData = raii.getData(); 
	jobject imgObject = jacqInfo.CreateJavaImageDataObject(env);

	// attach file header if this is a DIB
	if ( isBMP )
	{
		BITMAPFILEHEADER fileheader;
		LPBITMAPINFOHEADER lpbi=NULL;
		memset((char *)&fileheader,0,sizeof(BITMAPFILEHEADER));
		fileheader.bfType='MB';
		lpbi = (LPBITMAPINFOHEADER)pDibData;
		unsigned int bpp = lpbi->biBitCount;
		fileheader.bfSize      = (DWORD)GlobalSize (hDib) + sizeof (BITMAPFILEHEADER);
		fileheader.bfReserved1 = 0;
		fileheader.bfReserved2 = 0;
		fileheader.bfOffBits   = (DWORD)sizeof(BITMAPFILEHEADER) +
								  lpbi->biSize + CalculateUsedPaletteEntries(bpp) * sizeof(RGBQUAD);
		// we need to attach the bitmap header info onto the data
		unsigned int totalSize = GlobalSize(hDib) + sizeof(BITMAPFILEHEADER);
		std::vector<BYTE> bFullImage(totalSize);
		char *pFileHeader = reinterpret_cast<char *>(&fileheader);
		std::copy(pFileHeader, pFileHeader + sizeof(BITMAPFILEHEADER), &bFullImage[0]);
		std::copy(pDibData, pDibData + GlobalSize(hDib), &bFullImage[sizeof(BITMAPFILEHEADER)]);
		jacqInfo.setImageData(env, imgObject, &bFullImage[0], totalSize);
	}
	else
		jacqInfo.setImageData(env, imgObject, pDibData, GlobalSize(hDib));
	return imgObject;
}

typedef DTWAIN_ARRAY (DLLENTRY_DEF *DTWAIN_AcquireFn)(DTWAIN_SOURCE Source,
                                                      LONG PixelType,
                                                      LONG nMaxPages,
                                                      DTWAIN_BOOL bShowUI,
                                                      DTWAIN_BOOL bCloseSource,
                                                      LPLONG pStatus);

jobject AcquireHandler(DTWAIN_AcquireFn fn, JNIEnv *env, jlong src, jint pixelType, 
                        jint maxPages, jboolean showUI, jboolean closeSource, bool isBMP)
{
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

    DTWAIN_ARRAY acq;
    LONG nStatus;
	std::pair<DTWAINJNIGlobals::CurrentAcquireTypeMap::iterator, bool> ret;
	ret = g_JNIGlobals.g_CurrentAcquireMap.insert(std::make_pair((DTWAIN_SOURCE)src, isBMP));
	if ( !ret.second )
		ret.first->second = isBMP;
    acq = fn((DTWAIN_SOURCE)src, pixelType, maxPages, showUI, closeSource, &nStatus);
    JavaAcquirerInfo jacqInfo(env);
    jobject arrayObject = jacqInfo.CreateJavaAcquisitionArrayObject(env);
    if ( acq )
    {
        LONG nAcquisitions = DTWAIN_GetNumAcquisitions(acq);
        for (LONG i = 0; i < nAcquisitions; ++i)
        {
            jobject acquisitionObject = jacqInfo.CreateJavaAcquisitionDataObject(env);
            LONG nDibs = DTWAIN_GetNumAcquiredImages(acq, i);
            for ( LONG j = 0; j < nDibs; ++j )
            {
                HANDLE hDib = DTWAIN_GetAcquiredImage(acq, i, j);
                if ( hDib )
                {
					jobject imgObject = getFullImageBMPData(hDib, jacqInfo, env, isBMP);
                    jacqInfo.addImageDataToAcquisition(acquisitionObject, imgObject);
                }
            }
            jacqInfo.addAcquisitionToArray(arrayObject,acquisitionObject);
        }
        jacqInfo.setStatus(arrayObject, nStatus );
        DTWAIN_DestroyAcquisitionArray(acq, TRUE);
    }
	else
		g_JNIGlobals.g_CurrentAcquireMap.erase((DTWAIN_SOURCE)src);
    return arrayObject;    
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_AcquireNative
 * Signature: (JIIZZ)Lcom/dynarithmic/twain/DTwainAcquisitionArray;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1AcquireNative
  (JNIEnv *env, jobject, jlong src, jint pixelType, jint maxPages, jboolean showUI, jboolean closeSource)
{
    DTWAIN_TRY
    return AcquireHandler(&DTWAIN_AcquireNative, env, src, pixelType, maxPages, showUI, closeSource, true);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_AcquireBuffered
 * Signature: (JIIZZ)Lcom/dynarithmic/twain/DTwainAcquisitionArray;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1AcquireBuffered
(JNIEnv *env, jobject, jlong src, jint pixelType, jint maxPages, jboolean showUI, jboolean closeSource)
{
    DTWAIN_TRY
	LONG cmpType;
	DTWAIN_GetCompressionType((DTWAIN_SOURCE)src, &cmpType, TRUE);
	bool isBMPType = (cmpType == TWCP_NONE);
    return AcquireHandler(&DTWAIN_AcquireBuffered, env, src, pixelType, maxPages, showUI, closeSource, isBMPType);
    DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_AcquireFile
 * Signature: (JLjava/lang/String;IIIIZZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1AcquireFile
  (JNIEnv *env, jobject, jlong src, jstring filename, jint filetype, jint fileflags, 
        jint pixeltype, jint maxpages, jboolean showui, jboolean closesource)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    GetStringCharsHandler str(env, filename);
    LONG status;
	bool isBMP = (fileflags & DTWAIN_USESOURCEMODE)?false:true;
	std::pair<DTWAINJNIGlobals::CurrentAcquireTypeMap::iterator, bool> ret;
	ret = g_JNIGlobals.g_CurrentAcquireMap.insert(std::make_pair((DTWAIN_SOURCE)src, isBMP));
	if ( !ret.second )
		ret.first->second = isBMP;

    DTWAIN_BOOL bRet = DTWAIN_FUNCTION_CALLER9(DTWAIN_AcquireFile, LStLLLLLLl, (DTWAIN_SOURCE)src, reinterpret_cast<LPCTSTR>(str.GetStringChars()), filetype, fileflags,
                                           pixeltype, maxpages, showui, closesource, &status);
    if ( bRet )
        return status;
	g_JNIGlobals.g_CurrentAcquireMap.erase((DTWAIN_SOURCE)src);
    return -1;
    DTWAIN_CATCH(env)
}        

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCustomDSData
 * Signature: (J)[B
 */
JNIEXPORT jbyteArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCustomDSData
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    LONG actualSize;        
    HANDLE h = DTWAIN_GetCustomDSData((DTWAIN_SOURCE)src, NULL, 0, &actualSize, DTWAINGCD_COPYDATA);
    if ( h && actualSize > 0 )
    {
        // create a vector of the correct size
        std::vector<BYTE> vBytes(actualSize);
        h = DTWAIN_GetCustomDSData((DTWAIN_SOURCE)src, &vBytes[0], actualSize, &actualSize, DTWAINGCD_COPYDATA);
        return CreateJArrayFromCArray<JavaByteArrayTraits<char> >(env, (JavaByteArrayTraits<char>::api_base_type*)&vBytes[0], 
                                                            vBytes.size());
    }
    BYTE b;
    return CreateJArrayFromCArray<JavaByteArrayTraits<char> >(env, (JavaByteArrayTraits<char> ::api_base_type*)&b, 0);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCustomDSData
 * Signature: (J[B)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCustomDSData
  (JNIEnv *env, jobject, jlong src, jbyteArray customData)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    std::vector<char> dArray = CreateCArrayFromJArray<JavaByteArrayTraits<char> >(env, customData);
    if ( dArray.empty() )
        return 1;
    return DTWAIN_SetCustomDSData((DTWAIN_SOURCE)src, NULL, (LPBYTE)&dArray[0], dArray.size(), DTWAINSCD_USEDATA);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAcquireImageScale
 * Signature: (JDD)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAcquireImageScale
  (JNIEnv *env, jobject, jlong src, jdouble xScale, jdouble yScale)
{
  DTWAIN_TRY
  return DTWAIN_FUNCTION_CALLER3(DTWAIN_SetAcquireImageScale,LSFF,(DTWAIN_SOURCE)src, xScale, yScale);
  DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFOrientation
 * Signature: (JI)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFOrientation
  (JNIEnv *env, jobject, jlong src, jint orientation)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFOrientation,LSL,(DTWAIN_SOURCE)src, orientation);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFPageSize
 * Signature: (JIDD)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFPageSize
  (JNIEnv *env, jobject, jlong src, jint pageSize, jdouble customWidth, jdouble customHeight)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetPDFPageSize,LSLFF,(DTWAIN_SOURCE)src, pageSize, customWidth, customHeight);
    DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFPageScale
 * Signature: (JIDD)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFPageScale
  (JNIEnv *env, jobject, jlong src, jint scaleOpts, jdouble xScale, jdouble yScale)
{
	DTWAIN_TRY
	return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetPDFPageScale ,LSLFF,(DTWAIN_SOURCE)src, scaleOpts, xScale, yScale);
	DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAppInfo
 * Signature: (Lcom/dynarithmic/twain/DTwainAppInfo;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAppInfo
    (JNIEnv *env, jobject, jobject appInfoObj)
{
	DTWAIN_TRY
	if ( !g_JNIGlobals.g_DTWAINModule )
		throw "DTwain Module not loaded";

	JavaTwainAppInfo appInfo(env);
	appInfo.setObject(appInfoObj);
	DTWAIN_BOOL bRet = DTWAIN_FUNCTION_CALLER4(DTWAIN_SetAppInfo, Ltttt,reinterpret_cast<LPCTSTR>(appInfo.getVersionInfo().c_str()),
																reinterpret_cast<LPCTSTR>(appInfo.getManufacturer().c_str()),
																reinterpret_cast<LPCTSTR>(appInfo.getProductFamily().c_str()),
																reinterpret_cast<LPCTSTR>(appInfo.getProductName().c_str()));
	return bRet;
	DTWAIN_CATCH(env)
}


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetAppInfo
 * Signature: ()Lcom/dynarithmic/twain/DTwainAppInfo;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetAppInfo
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

    TCHAR szInfo[4][1024]; 
	JavaTwainAppInfo appInfo(env);

	DTWAIN_BOOL bRet = DTWAIN_FUNCTION_CALLER4(DTWAIN_GetAppInfo,LTTTT,&szInfo[0][0],&szInfo[1][0],&szInfo[2][0],&szInfo[3][0]);

	if ( bRet )
		return appInfo.createDTwainAppInfo(&szInfo[0][0], &szInfo[1][0], &szInfo[2][0], &szInfo[3][0]);
	return appInfo.createDTwainAppInfo();
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetSourceInfo
 * Signature: (J)Lcom/dynarithmic/twain/DTwainSourceInfo;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetSourceInfo
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
        
	JavaDTwainSourceInfo sInfo(env);

    TCHAR szInfo[4][1024]; 
    typedef LONG (__stdcall *SourceFn)(DTWAIN_SOURCE, LPTSTR, LONG);
    SourceFn allFn[4] = {&DTWAIN_GetSourceVersionInfo, &DTWAIN_GetSourceManufacturer, &DTWAIN_GetSourceProductFamily, 
                         &DTWAIN_GetSourceProductName};
                             
    BOOL bRet = TRUE;
    for (int i = 0; i < 4; ++i )
        bRet = (*allFn[i])((DTWAIN_SOURCE)src, &szInfo[i][0], 1023);
    if ( bRet )
    {
        LONG major, minor;
        DTWAIN_GetSourceVersionNumber((DTWAIN_SOURCE)src, &major, &minor);
        return sInfo.createFullObject(szInfo[0], szInfo[1], szInfo[2], szInfo[3], major, minor);
    }                                                          
    return sInfo.defaultConstructObject();
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetAuthor
 * Signature: (J)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetAuthor
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    TCHAR arg2[1024] = {0};
    DTWAIN_FUNCTION_CALLER2(DTWAIN_GetAuthor,LST, (DTWAIN_SOURCE)src, arg2);
    return CreateJStringFromCString(env, arg2);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCaption
 * Signature: (J)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCaption
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    TCHAR arg2[1024] = {0};
    DTWAIN_FUNCTION_CALLER2(DTWAIN_GetCaption,LST, (DTWAIN_SOURCE)src, arg2);
    return CreateJStringFromCString(env, arg2);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetNameFromCap
 * Signature: (I)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetNameFromCap
  (JNIEnv *env, jobject, jint capValue)
{
    DTWAIN_TRY
    TCHAR arg2[1024] = {0};
    DTWAIN_FUNCTION_CALLER3(DTWAIN_GetNameFromCap,LLTL, capValue, arg2, 1023);
    return CreateJStringFromCString(env, arg2);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_StartThread
 * Signature: (J)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1StartThread
  (JNIEnv *env, jobject, jlong dllHandle)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_StartThread,BH,(DTWAIN_HANDLE)dllHandle)?JNI_TRUE:JNI_FALSE;;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EndThread
 * Signature: (J)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EndThread
  (JNIEnv *env, jobject, jlong dllHandle)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_EndThread,BH,(DTWAIN_HANDLE)dllHandle)?JNI_TRUE:JNI_FALSE;;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SelectSourceByName
 * Signature: (Ljava/lang/String;)J
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SelectSourceByName
  (JNIEnv *env, jobject, jstring srcName)
{
    DTWAIN_TRY
    return (jlong)DTWAIN_FUNCTION_CALLER1(DTWAIN_SelectSourceByName, St, reinterpret_cast<LPCTSTR>(GetStringCharsHandler(env, srcName).GetStringChars()));
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapFromName
 * Signature: (Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapFromName
  (JNIEnv *env, jobject, jstring capName)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_GetCapFromName, Lt, reinterpret_cast<LPCTSTR>(GetStringCharsHandler(env, capName).GetStringChars()));
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAuthor
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAuthor
  (JNIEnv *env, jobject, jlong src, jstring author)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetAuthor,LSt, (DTWAIN_SOURCE)src, reinterpret_cast<LPCTSTR>(GetStringCharsHandler(env, author).GetStringChars()));
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCaption
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCaption
  (JNIEnv *env, jobject, jlong src, jstring caption)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetCaption,LSt, (DTWAIN_SOURCE)src, (LPCTSTR)GetStringCharsHandler(env, caption).GetStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFAuthor
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFAuthor
  (JNIEnv *env, jobject, jlong src, jstring pdfStr)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFAuthor,LSt, (DTWAIN_SOURCE)src,
                                (LPCTSTR)GetStringCharsHandler(env, pdfStr).GetStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFCreator
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFCreator
(JNIEnv *env, jobject, jlong src, jstring pdfStr)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFCreator,LSt, (DTWAIN_SOURCE)src,
        GetStringCharsHandler(env, pdfStr).GetWindowsStringChars());
    DTWAIN_CATCH(env)
}
  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFTitle
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFTitle
(JNIEnv *env, jobject, jlong src, jstring pdfStr)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFTitle,LSt, (DTWAIN_SOURCE)src,
        GetStringCharsHandler(env, pdfStr).GetWindowsStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFSubject
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFSubject
(JNIEnv *env, jobject, jlong src, jstring pdfStr)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFSubject,LSt, (DTWAIN_SOURCE)src,
        GetStringCharsHandler(env, pdfStr).GetWindowsStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFKeywords
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFKeywords
(JNIEnv *env, jobject, jlong src, jstring pdfStr)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFKeywords,LSt, (DTWAIN_SOURCE)src,
        GetStringCharsHandler(env, pdfStr).GetWindowsStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPostScriptTitle
 * Signature: (JLjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPostScriptTitle
(JNIEnv *env, jobject, jlong src, jstring pdfStr)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPostScriptTitle,LSt, (DTWAIN_SOURCE)src,
        GetStringCharsHandler(env, pdfStr).GetWindowsStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetTempFileDirectory
 * Signature: (Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetTempFileDirectory
  (JNIEnv *env, jobject, jstring str)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_SetTempFileDirectory,Lt,(LPCTSTR)GetStringCharsHandler(env, str).GetStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFEncryption
 * Signature: (JZLjava/lang/String;Ljava/lang/String;IZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFEncryption
  (JNIEnv *env, jobject, jlong src, jboolean useEncryption, jstring userPass, jstring ownerPass, 
                jint permissions, jboolean useStrong)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER6(DTWAIN_SetPDFEncryption, LSLttLL,
                                                        (DTWAIN_SOURCE)src, useEncryption, 
                                                        GetStringCharsHandler(env, userPass).GetWindowsStringChars(),
                                                        GetStringCharsHandler(env, ownerPass).GetWindowsStringChars(), 
                                                        permissions, useStrong);
    DTWAIN_CATCH(env)
}  


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsDIBBlank
 * Signature: (JD)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsDIBBlank
  (JNIEnv *env, jobject, jlong dibHandle, jdouble threshold)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER2(DTWAIN_IsDIBBlank,LHF, (HANDLE)dibHandle, threshold)?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}  


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetTempFileDirectory
 * Signature: ()Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetTempFileDirectory
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    TCHAR szDir[_MAX_PATH] = {0};
    DTWAIN_FUNCTION_CALLER2(DTWAIN_GetTempFileDirectory,LTL, szDir, _MAX_PATH);
    return CreateJStringFromCString(env, szDir);    
    DTWAIN_CATCH(env)
}  

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetBlankPageDetection
(JNIEnv *env, jobject, jlong src, jdouble threshold, jint autodetect, jboolean bSet)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetBlankPageDetection,LSFLL,(DTWAIN_SOURCE)src, threshold, autodetect, bSet);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_LogMessage
 * Signature: (Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1LogMessage
  (JNIEnv *env, jobject, jstring str)
{
    DTWAIN_TRY
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_LogMessage, Lt, GetStringCharsHandler(env, str).GetWindowsStringChars());
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SelectSource2
 * Signature: (Ljava/lang/String;III)J
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SelectSource2
  (JNIEnv *env, jobject, jstring title, jint xpos, jint ypos, jint flags)
{
    DTWAIN_TRY
    return (jlong)DTWAIN_FUNCTION_CALLER5(DTWAIN_SelectSource2,SHtLLL,
                                            (HWND)0, GetStringCharsHandler(env,title).GetWindowsStringChars(),
                                            xpos, ypos, flags);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetErrorString
 * Signature: (I)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetErrorString
  (JNIEnv *env, jobject, jint nError)
{
    DTWAIN_TRY
    LONG nLen = DTWAIN_FUNCTION_CALLER3(DTWAIN_GetErrorString,LLTL, nError, (LPTSTR)0, 0);
    if ( nLen > 0 )
    {
        std::vector<TCHAR> sz(nLen+1);
        DTWAIN_FUNCTION_CALLER3(DTWAIN_GetErrorString,LLTL, nError, &sz[0], nLen);
        return CreateJStringFromCString(env, &sz[0]);
    }
    return CreateJStringFromCString(env, _T(""));        
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_AcquireFileEx
 * Signature: (J[Ljava/lang/String;IIIIZZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1AcquireFileEx
  (JNIEnv *env, jobject, jlong src, jobjectArray strArray, jint filetype, jint fileflags, 
      jint pixeltype, jint numpages, jboolean showUI, jboolean closeSource)
{
    DTWAIN_TRY
    LONG nStatus;
    DTWAIN_ARRAY dArray = CreateDTWAINArrayFromJStringArray(env, strArray);
    DTWAINArray_RAII raii(dArray);
	BOOL bRet = DTWAIN_FUNCTION_CALLER9(DTWAIN_AcquireFileEx,LSaLLLLLLl,(DTWAIN_SOURCE)src, dArray, filetype, fileflags, pixeltype, numpages, showUI, closeSource, &nStatus);
    return nStatus;                                      
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCurrentAcquiredImage
 * Signature: (J)Lcom/dynarithmic/twain/DTwainImageData;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCurrentAcquiredImage
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
	HANDLE hDib = DTWAIN_FUNCTION_CALLER1(DTWAIN_GetCurrentAcquiredImage, HandleS, (DTWAIN_SOURCE)src);
    JavaAcquirerInfo acqInfo(env);
	DTWAINJNIGlobals::CurrentAcquireTypeMap::iterator it = g_JNIGlobals.g_CurrentAcquireMap.find((DTWAIN_SOURCE)src);
	bool isBMP = true;
	if (it != g_JNIGlobals.g_CurrentAcquireMap.end())
		isBMP = it->second;
	jobject imgData = getFullImageBMPData(hDib, acqInfo, env, isBMP);
    return imgData;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesFrame
 * Signature: (JII[Lcom/dynarithmic/twain/DTwainFrame;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesFrame
  (JNIEnv *env, jobject, jlong src, jint capValue, jint setType, jobjectArray FrameArray)
{
    DTWAIN_TRY
    DTWAIN_ARRAY dFrameArray = CreateDTWAINArrayFromJFrameArray(env, FrameArray);
    DTWAINArray_RAII raii(dFrameArray);
    return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetCapValues,LSLLa,(DTWAIN_SOURCE)src, capValue, setType, dFrameArray);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesFrameEx
 * Signature: (JIII[Lcom/dynarithmic/twain/DTwainFrame;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesFrameEx
  (JNIEnv *env, jobject, jlong src, jint capValue, jint setType, jint containerType, jobjectArray FrameArray)
{
    DTWAIN_TRY
    DTWAIN_ARRAY dFrameArray = CreateDTWAINArrayFromJFrameArray(env, FrameArray);
    DTWAINArray_RAII raii(dFrameArray);
	return DTWAIN_FUNCTION_CALLER5(DTWAIN_SetCapValuesEx,LSLLLa,(DTWAIN_SOURCE)src, capValue, setType, containerType, dFrameArray);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetCapValuesFrameEx2
 * Signature: (JIIII[Lcom/dynarithmic/twain/DTwainFrame;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetCapValuesFrameEx2
(JNIEnv *env, jobject, jlong src, jint capValue, jint setType, jint containerType, jint nDataType, jobjectArray FrameArray)
{
    DTWAIN_TRY
    DTWAIN_ARRAY dFrameArray = CreateDTWAINArrayFromJFrameArray(env, FrameArray);
    DTWAINArray_RAII raii(dFrameArray);
	return DTWAIN_FUNCTION_CALLER6(DTWAIN_SetCapValuesEx2,LSLLLLa,(DTWAIN_SOURCE)src, capValue, setType, containerType, nDataType, dFrameArray);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesFrame
 * Signature: (JII)[Lcom/dynarithmic/twain/DTwainFrame;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesFrame
  (JNIEnv *env, jobject, jlong src, jint capValue, jint getType)
{
    DTWAIN_TRY
    DTWAIN_ARRAY arr = 0;
    DTWAINArray_RAII raii(arr);
	DTWAIN_FUNCTION_CALLER4(DTWAIN_GetCapValues,LSLLA,(DTWAIN_SOURCE)src, capValue, getType, &arr);
    return CreateJFrameArrayFromDTWAINArray(env, arr);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesFrameEx
 * Signature: (JIII)[Lcom/dynarithmic/twain/DTwainFrame;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesFrameEx
  (JNIEnv *env, jobject, jlong src, jint capValue, jint getType, jint containerType)
{
    DTWAIN_TRY
    DTWAIN_ARRAY arr = 0;
    DTWAINArray_RAII raii(arr);
	DTWAIN_FUNCTION_CALLER5(DTWAIN_GetCapValuesEx,LSLLLA,(DTWAIN_SOURCE)src, capValue, getType, containerType, &arr);
    return CreateJFrameArrayFromDTWAINArray(env, arr);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetCapValuesFrameEx2
 * Signature: (JIIII)[Lcom/dynarithmic/twain/DTwainFrame;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetCapValuesFrameEx2
  (JNIEnv *env, jobject, jlong src, jint capValue, jint getType, jint containerType, jint dataType)
{
    DTWAIN_TRY
    DTWAIN_ARRAY arr = 0;
    DTWAINArray_RAII raii(arr);
	DTWAIN_FUNCTION_CALLER6(DTWAIN_GetCapValuesEx2,LSLLLLA,(DTWAIN_SOURCE)src, capValue, getType, containerType, dataType, &arr);
    return CreateJFrameArrayFromDTWAINArray(env, arr);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetAcquireArea2
 * Signature: (JLcom/dynarithmic/twain/DTwainAcquireArea;I)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetAcquireArea2
  (JNIEnv *env, jobject, jlong src, jobject jFrame, jint flags)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

	JavaDTwainAcquireArea vArea(env);
	vArea.setObject(jFrame);

    // Call the DTWAIN function to set the area information
    DTWAIN_ARRAY dSetArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYFLOAT, 4);
    DTWAINArray_RAII raii1(dSetArray);
    LONG srcUnit;
    if ( dSetArray )
    {
		const char* fnName[] = {"getLeft", "getTop", "getRight", "getBottom", "getUnitOfMeasure"};
		DTWAIN_FLOAT* pBuf = (DTWAIN_FLOAT*)DTWAIN_ArrayGetBuffer(dSetArray, 0);
        for (int i = 0; i < 5; ++i )
        {
            if ( i < 4 )
                *(pBuf + i) = vArea.callDoubleMethod(fnName[i]);
            else
                srcUnit = vArea.callIntMethod(fnName[i]);
        }                
        
		DTWAIN_FUNCTION_CALLER7(DTWAIN_SetAcquireArea2,LSFFFFLL,(DTWAIN_SOURCE)src, 
														(*pBuf), *(pBuf + 1), *(pBuf + 2), *(pBuf + 3), srcUnit, flags);
    }        
    return FALSE;
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetAcquireArea2
 * Signature: (J)Lcom/dynarithmic/twain/DTwainAcquireArea;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetAcquireArea2
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

	JavaDTwainAcquireArea vArea(env);

    // Call the DTWAIN function to get the area information
    DTWAIN_ARRAY dArray = 0;
    double dLoc[4];
    LONG unit = DTWAIN_INCHES;
    BOOL bRet = DTWAIN_FUNCTION_CALLER6(DTWAIN_GetAcquireArea2,LSffffl,(DTWAIN_SOURCE)src, &dLoc[0], &dLoc[1], &dLoc[2], &dLoc[3], &unit);
    DTWAINArray_RAII raii(dArray);
    if ( bRet )
    {
        DTWAIN_FLOAT* pFloat = (DTWAIN_FLOAT *)DTWAIN_ArrayGetBuffer(dArray, 0);
        return vArea.createFullObject(*pFloat, *(pFloat + 1), *(pFloat + 2), *(pFloat + 3), unit); 
    }

    // Call Java function to declare and init a new versionInfo object
	return vArea.defaultConstructObject();
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SelectOCREngineByName
 * Signature: (Ljava/lang/String;)J
 */
JNIEXPORT jlong JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SelectOCREngineByName
  (JNIEnv *env, jobject, jstring ocrName)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
	return DTWAIN_FUNCTION_CALLER1(DTWAIN_SelectOCREngineByName,Lt,GetStringCharsHandler(env,ocrName).GetWindowsStringChars());
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumOCRInterfaces
 * Signature: ()[J
 */
JNIEXPORT jlongArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumOCRInterfaces
  (JNIEnv *env, jobject)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    DTWAIN_ARRAY arr = 0;
    DTWAINArray_RAII raii(arr);        
    BOOL bRet = DTWAIN_FUNCTION_CALLER1(DTWAIN_EnumOCRInterfaces, La, &arr); 
    return CreateJArrayFromDTWAINArray<JavaLongArrayTraits>(env, arr, 0);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EnumOCRSupportedCaps
 * Signature: (J)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EnumOCRSupportedCaps
  (JNIEnv *env, jobject, jlong ocr)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    DTWAIN_ARRAY arr = 0;
    DTWAINArray_RAII raii(arr);        
    DTWAIN_FUNCTION_CALLER2(DTWAIN_EnumOCRSupportedCaps, LLa, (DTWAIN_OCRENGINE)ocr, &arr); 
    return CreateJArrayFromDTWAINArray<JavaIntArrayTraits>(env, arr, 0);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetOCRCapValuesInt
 * Signature: (JII)[I
 */
JNIEXPORT jintArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetOCRCapValuesInt
  (JNIEnv *env, jobject, jlong ocr, jint capValue, jint getType)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    DTWAIN_ARRAY arr = 0;
    DTWAINArray_RAII raii(arr);        
    DTWAIN_FUNCTION_CALLER4(DTWAIN_GetOCRCapValues, LLLLa, (DTWAIN_OCRENGINE)ocr, capValue, getType, &arr);
    return CreateJArrayFromDTWAINArray<JavaIntArrayTraits>(env, arr, 0);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetOCRCapValuesString
 * Signature: (JII)[Ljava/lang/String;
 */
JNIEXPORT jobjectArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetOCRCapValuesString
  (JNIEnv *env, jobject, jlong ocr, jint capValue, jint getType)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    DTWAIN_ARRAY arr = 0;
    DTWAINArray_RAII raii(arr);        
	DTWAIN_FUNCTION_CALLER4(DTWAIN_GetOCRCapValues, LLLLa, (DTWAIN_OCRENGINE)ocr, capValue, getType, &arr);
    return CreateJStringArrayFromDTWAIN(env, arr);
    DTWAIN_CATCH(env)
}

JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetOCRCapValuesInt
(JNIEnv *env, jobject, jlong ocr, jint capValue, jint setType, jintArray jarr)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    DTWAIN_ARRAY arr = CreateDTWAINArrayFromJArray<JavaIntArrayTraits>(env, jarr);
    DTWAINArray_RAII raii(arr);        
    return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetOCRCapValues, LLLLA, (DTWAIN_OCRENGINE)ocr, capValue, setType, arr);
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetOCRCapValuesString
 * Signature: (JII[Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetOCRCapValuesString
  (JNIEnv *env, jobject, jlong ocr, jint capValue, jint setType, jobjectArray jarr)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    DTWAIN_ARRAY arr = CreateDTWAINArrayFromJStringArray(env, jarr);
    DTWAINArray_RAII raii(arr);        
	return DTWAIN_FUNCTION_CALLER4(DTWAIN_SetOCRCapValues, LLLLA, (DTWAIN_OCRENGINE)ocr, capValue, setType, arr);
    DTWAIN_CATCH(env)
}  


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ShutdownOCREngine
 * Signature: (J)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ShutdownOCREngine
  (JNIEnv *env, jobject, jlong ocr)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_ShutdownOCREngine, LL, (DTWAIN_OCRENGINE)ocr);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_IsOCREngineActivated
 * Signature: (J)Z
 */
JNIEXPORT jboolean JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1IsOCREngineActivated
  (JNIEnv *env, jobject, jlong ocr)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    return DTWAIN_FUNCTION_CALLER1(DTWAIN_IsOCREngineActivated, LL, (DTWAIN_OCRENGINE)ocr)?JNI_TRUE:JNI_FALSE;    
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFOCRConversion
 * Signature: (JIIIII)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFOCRConversion
  (JNIEnv *env, jobject, jlong ocr, jint pageType, jint fileType, jint pixelType, jint bitDepth, jint options)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    return DTWAIN_FUNCTION_CALLER6(DTWAIN_SetPDFOCRConversion,LLLLLLL,(DTWAIN_OCRENGINE)ocr, pageType, fileType, pixelType, bitDepth, options);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetPDFOCRMode
 * Signature: (JZ)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetPDFOCRMode
  (JNIEnv *env, jobject, jlong ocr, jboolean bSet)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
	return DTWAIN_FUNCTION_CALLER2(DTWAIN_SetPDFOCRMode,LSL,(DTWAIN_SOURCE)ocr, bSet);
    DTWAIN_CATCH(env)
}  


/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_ExecuteOCR
 * Signature: (JLjava/lang/String;II)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1ExecuteOCR
  (JNIEnv *env, jobject, jlong ocr, jstring szFileName, jint startPage, jint endPage)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    return DTWAIN_FUNCTION_CALLER4(DTWAIN_ExecuteOCR, LLtLL, (DTWAIN_OCRENGINE)ocr, GetStringCharsHandler(env, szFileName).GetWindowsStringChars(),
			                         startPage, endPage);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetOCRInfo
 * Signature: (J)Lcom/dynarithmic/twain/DTwainOCRInfo;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetOCRInfo
  (JNIEnv *env, jobject, jlong ocr)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

	JavaDTwainOCRInfo vOCR(env);
    TCHAR szInfo[4][1024]; 

    typedef LONG (__stdcall *OCRFn)(DTWAIN_OCRENGINE, LPTSTR, LONG);
    OCRFn allFn[4] = {&DTWAIN_GetOCRVersionInfo, 
                         &DTWAIN_GetOCRManufacturer, 
                         &DTWAIN_GetOCRProductFamily, 
                         &DTWAIN_GetOCRProductName};

    BOOL bRet = TRUE;
    for (int i = 0; i < 4; ++i )
        bRet = (*allFn[i])((DTWAIN_OCRENGINE)ocr, &szInfo[i][0], 1023);
    if ( bRet )
        return vOCR.createFullObject(szInfo[0], szInfo[1], szInfo[2], szInfo[3]);
    return vOCR.defaultConstructObject();
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetOCRText
 * Signature: (JI)[B
 */
JNIEXPORT jbyteArray JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetOCRText
  (JNIEnv *env, jobject, jlong ocr, jint pageNum)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    LONG actualSize;        
    HANDLE hReturn = DTWAIN_FUNCTION_CALLER6(DTWAIN_GetOCRText, HLLTLlL, (DTWAIN_OCRENGINE)ocr, pageNum, (LPTSTR)0, 0, &actualSize, DTWAINOCR_COPYDATA);
    if (hReturn)
    {
        std::vector<TCHAR> ts(actualSize);
        DTWAIN_FUNCTION_CALLER6(DTWAIN_GetOCRText, HLLTLlL, (DTWAIN_OCRENGINE)ocr, pageNum, &ts[0], ts.size(), &actualSize, DTWAINOCR_COPYDATA);
        return CreateJArrayFromCArray<JavaByteArrayTraits<TCHAR> >(env, &ts[0], ts.size()*sizeof(TCHAR));
    }
    return CreateJArrayFromCArray<JavaByteArrayTraits<TCHAR> >(env, NULL,0);
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_CreateBufferedStripInfo
 * Signature: (J)Lcom/dynarithmic/twain/DTwainBufferedStripInfo;
 */
JNIEXPORT jobject JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1CreateBufferedStripInfo
  (JNIEnv *env, jobject, jlong src)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";

	JavaDTwainBufferedStripInfo sInfo(env);
        
    LONG minSize, maxSize, prefSize;
    BOOL bRet = DTWAIN_FUNCTION_CALLER4(DTWAIN_GetAcquireStripSizes,LSlll,(DTWAIN_SOURCE)src, &minSize, &maxSize, &prefSize);
    if ( bRet )
	{
        jobject jobj = sInfo.createFullObject(prefSize, minSize, maxSize);
		sInfo.setObject(jobj);
		sInfo.setBufferSize(prefSize);
		return jobj;
	}
    return sInfo.defaultConstructObject();
    DTWAIN_CATCH(env)
}

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_SetBufferedTransferInfo
 * Signature: (JLcom/dynarithmic/twain/DTwainBufferedStripInfo;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1SetBufferedTransferInfo
  (JNIEnv *env, jobject, jlong src, jobject jBufferedStripInfo)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
        
    JavaDTwainBufferedStripInfo jBufInfo(env);
	jBufInfo.setObject(jBufferedStripInfo);
	LONG nSize = jBufInfo.getBufferSize();
    HANDLE hnd = GlobalAlloc(GHND, nSize);
    jBufInfo.setBufferHandle(hnd);
	DTWAIN_FUNCTION_CALLER2(DTWAIN_SetAcquireStripBuffer,LSH,(DTWAIN_SOURCE)src, hnd);
	jBufInfo.setBufferSize(nSize);
    return hnd?JNI_TRUE:JNI_FALSE;
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_GetBufferedStripData
 * Signature: (JLcom/dynarithmic/twain/DTwainBufferedStripInfo;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetBufferedStripData
  (JNIEnv *env, jobject obj, jlong src, jobject jBufferedStripInfo)
{
    DTWAIN_TRY
    struct HandleRAII
    {
        HANDLE m_h;
        HandleRAII(HANDLE h) : m_h(h) {}
        ~HandleRAII() { GlobalUnlock(m_h); }
    };
    
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    jobject imginfo = Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1GetImageInfo(env, obj, src);
    JavaDTwainBufferedStripInfo jInfo(env);
	jInfo.setObject(jBufferedStripInfo);
    jInfo.setImageInfo(imginfo);
    LONG Compression, BytesPerRow, Columns, Rows, XOffset, YOffset, BytesWritten;
    BOOL bRet = DTWAIN_FUNCTION_CALLER8(DTWAIN_GetAcquireStripData, LSlllllll, (DTWAIN_SOURCE)src, &Compression, &BytesPerRow, &Columns, 
                                            &Rows, &XOffset, &YOffset, &BytesWritten);
    if ( bRet )                                            
    {
        jInfo.setBufferStripInfo(Columns, Rows, XOffset, YOffset, BytesWritten, BytesPerRow);
        HANDLE h = jInfo.getBufferHandle();
        LPBYTE strip = (BYTE*)GlobalLock(h);
        HandleRAII raii(h);
        if ( strip )
            jInfo.setBufferedStripData(strip, BytesWritten);
    }
    return bRet;        
    DTWAIN_CATCH(env)
}  

/*
 * Class:     com_dynarithmic_twain_DTwainJavaAPI
 * Method:    DTWAIN_EndBufferedTransfer
 * Signature: (JLcom/dynarithmic/twain/DTwainBufferedStripInfo;)I
 */
JNIEXPORT jint JNICALL Java_com_dynarithmic_twain_DTwainJavaAPI_DTWAIN_1EndBufferedTransfer
  (JNIEnv *env, jobject, jlong src, jobject jBufferedTransfer)
{
    DTWAIN_TRY
    if ( !g_JNIGlobals.g_DTWAINModule )
        throw "DTwain Module not loaded";
    JavaDTwainBufferedStripInfo jInfo(env);
	jInfo.setObject(jBufferedTransfer);
    HANDLE h = jInfo.getBufferHandle();
    GlobalFree(h);
    return JNI_TRUE;
    DTWAIN_CATCH(env)
}  