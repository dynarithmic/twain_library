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
#include <boost/format.hpp>
#include <type_traits>
#include "ctltwmgr.h"
#include "enumeratorfuncs.h"
#include "errorcheck.h"
#include "ctlsupport.h"
using namespace std;
#ifdef _MSC_VER
#pragma warning (disable:4702)
#endif
using namespace dynarithmic;

static bool GetDoubleCap( DTWAIN_SOURCE Source, LONG lCap, double *pValue );
static LONG GetAllCapValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG lCap, DTWAIN_BOOL bExpandRange);
static LONG GetCurrentCapValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG lCap, DTWAIN_BOOL bExpandRange);
static LONG GetDefaultCapValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG lCap, DTWAIN_BOOL bExpandRange);

typedef bool (*SetDoubleCapFn)(DTWAIN_SOURCE, LONG, double);
typedef bool (*GetDoubleCapFn)(DTWAIN_SOURCE, LONG, double *);
typedef LONG (*GetCapValuesFn)(DTWAIN_SOURCE, LPDTWAIN_ARRAY, LONG, DTWAIN_BOOL);

static LONG EnumCapInternal(DTWAIN_SOURCE Source,
                            TW_UINT16 Cap,
                            LPDTWAIN_ARRAY arr,
                            DTWAIN_BOOL expandRange,
                            GetCapValuesFn fn,
                            const CTL_String& func,
                            const CTL_StringType& paramLog);

#define GENERATE_PARAM_LOG(argVals) \
        (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK) ? (ParamOutputter(_T(#argVals)).outputParam argVals.getString()) : _T("")

template <typename CapArrayType>
static bool GetCapability(DTWAIN_SOURCE Source, TW_UINT16 Cap, CapArrayType value,
                                 GetCapValuesFn /*capFn*/, const CTL_String& func, const CTL_StringType& paramLog)
{
    DTWAIN_ARRAY ArrayValues = 0;
    LONG retVal = EnumCapInternal(Source, Cap, &ArrayValues, false, GetCurrentCapValues, func, paramLog);
    DTWAINArrayLL_RAII arr(ArrayValues);
    if (retVal > 0)
    {
        auto& vOut = EnumeratorVector<typename std::remove_pointer<CapArrayType>::type>(ArrayValues);
        *value = vOut[0];
        return true;
    }
    return false;
}

template <typename CapDataType>
struct SetSupportFn1
{
    DTWAIN_SOURCE theSource;
    CapDataType theValue;
    TW_UINT16 theCap;
    bool setType;

    SetSupportFn1(DTWAIN_SOURCE s, CapDataType v, TW_UINT16 Cap, bool setCurrent)
    { SetAll(s, v ,Cap, setCurrent); }

    SetSupportFn1()
    { SetAll(0, 0, 0, false); }

    void SetAll(DTWAIN_SOURCE s, CapDataType v, TW_UINT16 Cap, bool setCurrent)
    {
        theSource = s;
        theCap = Cap;
        theValue = v;
        setType = setCurrent;
    }

    bool operator()()
    { return SetSupport<CapDataType>(theSource, &theValue, theCap, setType ? true : false); }
};

template <typename T>
struct SetSupportFn2 : public SetSupportFn1<T>
{
    SetSupportFn2(DTWAIN_SOURCE s, T v, TW_UINT16 Cap, bool setCurrent) : SetSupportFn1<T>(s,v,Cap, setCurrent) {}

    bool operator()()
    { return dynarithmic::SetSupportArray(this->theSource, this->theValue, this->theCap); }
};

template <typename T, typename FnToCall>
static T FunctionCaller(FnToCall fn, const CTL_String& func, const CTL_StringType& paramLog)
{
    bool doLog = (CTL_TwainDLLHandle::s_lErrorFilterFlags & DTWAIN_LOG_CALLSTACK) ? true : false;
    try
    {
        T bRet = T(0);
        if (doLog)
        {
            try { CTL_TwainAppMgr::WriteLogInfo(CTL_LogFunctionCallA(func.c_str(), LOG_INDENT_IN) + paramLog); }
            catch (...) {}
        }
        bRet = fn();
        if (doLog)
        {
            try
            {
                CTL_TwainAppMgr::WriteLogInfo(CTL_LogFunctionCallA(func.c_str(), LOG_INDENT_OUT) + ParamOutputter(_T(""), true).outputParam(bRet).getString());
            }
            catch (...)
            { return bRet; }
        }
        return bRet;
    }
    catch (T var) { return var; }
    catch (...)
    {
        LogExceptionErrorA(func.c_str());
        if (CTL_TwainDLLHandle::s_bThrowExceptions)
            DTWAIN_InternalThrowException();
        return T(0);
    }
}

struct GetDeviceCapsByStringFn
{
    DTWAIN_SOURCE theSource;
    LPTSTR theString;
    GetByStringFn theFn;

    GetDeviceCapsByStringFn(DTWAIN_SOURCE Src, LPTSTR value, GetByStringFn fn) :
        theSource(Src), theString(value), theFn(fn) {}
    DTWAIN_BOOL operator()() { return dynarithmic::DTWAIN_GetDeviceCapByString(theSource, theString, theFn); }
};

struct CapSetterFn
{
    DTWAIN_SOURCE Source;
    LPCTSTR value;
    SetByStringFn fn;
    CapSetterFn(DTWAIN_SOURCE src, LPCTSTR val, SetByStringFn theFn) : Source(src), value(val), fn(theFn) {}
    DTWAIN_BOOL operator()()
    {
        return dynarithmic::DTWAIN_SetDeviceCapByString(Source, value, fn);
    }
};

struct CapSetterFn2
{
    DTWAIN_SOURCE Source;
    LPCTSTR value;
    SetByStringFn2 fn;
    bool extra;
    CapSetterFn2(DTWAIN_SOURCE src, LPCTSTR val, bool ex, SetByStringFn2 theFn) : Source(src), value(val), fn(theFn), extra(ex) {}
    DTWAIN_BOOL operator()()
    {
        return dynarithmic::DTWAIN_SetDeviceCapByString2(Source, value, extra, fn);
    }
};

struct IsEnabledImplFn
{
    DTWAIN_SOURCE theSource;
    TW_UINT16 theCap;
    IsEnabledImplFn(DTWAIN_SOURCE Src, TW_UINT16 cap) : theSource(Src), theCap(cap) {}
    bool operator()() { return CheckEnabled(theSource, theCap) ? true : false; }
};

template <typename T>
struct IsSupportedImplFn
{
    DTWAIN_SOURCE theSource;
    TW_UINT16 theCap;
    bool supportFlag;
    T theSupportVal;

    IsSupportedImplFn(DTWAIN_SOURCE Src, TW_UINT16 cap, bool sup, T supVal) :
        theSource(Src), theCap(cap), supportFlag(sup), theSupportVal(supVal) {}
    bool operator()() { return IsSupported(theSource, supportFlag, theSupportVal, theCap); }
};

struct EnumCapInternalFn
{
    DTWAIN_SOURCE theSource;
    TW_UINT16 theCap;
    LPDTWAIN_ARRAY theArray;
    bool theExpand;
    GetCapValuesFn theFn;
    EnumCapInternalFn(DTWAIN_SOURCE Src, TW_UINT16 cap, LPDTWAIN_ARRAY arr, bool expand, GetCapValuesFn fn) :
        theSource(Src), theCap(cap), theArray(arr), theExpand(expand), theFn(fn) {}
    LONG operator()() { return theFn(theSource, theArray, theCap, theExpand); }
};

template <typename CapDataType, typename SetterFn>
static DTWAIN_BOOL SetCapability(SetterFn fn, const CTL_String& func, const CTL_StringType& paramLog)
{ return FunctionCaller<DTWAIN_BOOL>(fn, func, paramLog); }

static DTWAIN_BOOL GetCapabilityByString(GetDeviceCapsByStringFn fn,
                                        const CTL_String& func,
                                        const CTL_StringType& paramLog)
{ return FunctionCaller<DTWAIN_BOOL>(fn, func, paramLog); }

static bool IsEnabledImpl(DTWAIN_SOURCE Source, TW_UINT16 Cap, const CTL_String& func, const CTL_StringType& paramLog)
{ return FunctionCaller<bool>(IsEnabledImplFn(Source, Cap), func, paramLog); }

template <typename T>
static bool IsSupportedImpl(DTWAIN_SOURCE Source, TW_UINT16 Cap, bool anySupport, T SupportVal,
                            const CTL_String& func, const CTL_StringType& paramLog)
{ return FunctionCaller<bool>(IsSupportedImplFn<T>(Source, Cap, anySupport, SupportVal), func, paramLog); }

static LONG EnumCapInternal(DTWAIN_SOURCE Source,
                            TW_UINT16 Cap,
                            LPDTWAIN_ARRAY arr,
                            DTWAIN_BOOL expandRange,
                            GetCapValuesFn fn,
                            const CTL_String& func,
                            const CTL_StringType& paramLog)
{ return FunctionCaller<LONG>(EnumCapInternalFn(Source, Cap, arr, expandRange?true:false, fn), func, paramLog); }

template <typename SetFn>
static DTWAIN_BOOL SetCapabilityByString(SetFn fn,
                                         const CTL_String& func,
                                         const CTL_StringType& paramLog)
{ return FunctionCaller<DTWAIN_BOOL>(fn, func, paramLog); }

static bool GetStringCapability(DTWAIN_SOURCE Source, TW_UINT16 Cap, LPTSTR value, LONG nLen, GetCapValuesFn fn, const CTL_String& func, const CTL_StringType& paramLog)
{

    DTWAIN_ARRAY ArrayValues = 0;
    LONG retVal = EnumCapInternal(Source, Cap, &ArrayValues, false, fn, func, paramLog);
    if (nLen > 0)
        value[0] = _T('\0');
    DTWAINArrayLL_RAII arr(ArrayValues);
    if (retVal > 0)
    {
        CTL_StringType sVal;
        EnumeratorFunctionImpl::EnumeratorGetAt(ArrayValues, 0, &sVal);
        CopyInfoToCString(sVal, value, nLen);
        return true;
    }
    return false;
}


#define EXPORT_ENUM_CAP_VALUES(FuncName, Cap) \
    LONG DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange) {\
        return EnumCapInternal(Source, Cap, pArray, bExpandIfRange, GetAllCapValues, __FUNCTION__, GENERATE_PARAM_LOG((Source, pArray, bExpandIfRange))); }

#define EXPORT_ENUM_CAP_VALUES_NOEXPAND(FuncName, Cap) \
    LONG DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray) {\
            return EnumCapInternal(Source, Cap, pArray, false, GetAllCapValues, __FUNCTION__, GENERATE_PARAM_LOG((Source, pArray))); }

#define EXPORT_ENUM_CAP_VALUES_EX(FuncName, Cap) \
    DTWAIN_ARRAY DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange) {\
		DTWAIN_ARRAY pArray = 0; \
        EnumCapInternal(Source, Cap, &pArray, bExpandIfRange, GetAllCapValues, __FUNCTION__, GENERATE_PARAM_LOG((Source, bExpandIfRange))); \
        return pArray; }

#define EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(FuncName, Cap) \
    DTWAIN_ARRAY DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source) {\
			DTWAIN_ARRAY pArray = 0; \
            EnumCapInternal(Source, Cap, &pArray, false, GetAllCapValues, __FUNCTION__, GENERATE_PARAM_LOG((Source))); \
			return pArray; }

#define EXPORT_SET_CAP_VALUE(FuncName, Cap, CapDataType, CapFn) \
    DTWAIN_BOOL  DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, CapDataType value) \
    {\
       return SetCapability <CapDataType>(CapFn(Source, value, Cap, true), __FUNCTION__, GENERATE_PARAM_LOG((Source, value))); \
    }


#define EXPORT_SET_CAP_VALUE_PARM3_2(FuncName, Cap1, Cap2, CapDataType1, CapDataType2, value2, CapFn) \
    DTWAIN_BOOL  DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, CapDataType1 value, DTWAIN_BOOL condType ) \
{\
    if ( condType )\
        SetCapability <CapDataType2>(CapFn(Source, value2, Cap2, true), __FUNCTION__, GENERATE_PARAM_LOG((Source, value))); \
    return SetCapability <CapDataType1>(CapFn(Source, value, Cap1, true), __FUNCTION__, GENERATE_PARAM_LOG((Source, value))); \
}

#define EXPORT_SET_CAP_VALUE_2(FuncName, Cap1, Cap2, CapDataType1, CapDataType2, CapFn) \
    DTWAIN_BOOL  DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, CapDataType1 value) \
{\
    CapFn theFn, theFn2;\
    theFn.SetAll(Source, value, Cap1, true);\
    theFn2.SetAll(Source, value, Cap2, true);\
    if (SetCapability <CapDataType1>(theFn, __FUNCTION__, GENERATE_PARAM_LOG((Source, value)))) \
        return SetCapability<CapDataType2>(theFn2, __FUNCTION__, GENERATE_PARAM_LOG((Source, value))); \
    return FALSE;\
}

#define EXPORT_SET_CAP_VALUE_D_D(FuncName, Cap1, Cap2) \
    EXPORT_SET_CAP_VALUE_2(FuncName, Cap1, Cap2, DTWAIN_FLOAT, DTWAIN_FLOAT, SetSupportFn1<DTWAIN_FLOAT>)

#define EXPORT_GET_CAP_VALUE(FuncName, Cap, CapDataType, CapFn) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, CapDataType value)\
    {\
        return GetCapability<CapDataType>(Source, Cap, value, CapFn, __FUNCTION__, GENERATE_PARAM_LOG((Source, value))); \
    }

#define EXPORT_GET_CAP_VALUE_OPT_CURRENT(FuncName, Cap, CapDataType) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, CapDataType value, DTWAIN_BOOL bCurrent)\
    {\
        CTL_StringType sLog = GENERATE_PARAM_LOG((Source, value, bCurrent));\
        CTL_String func = __FUNCTION__;\
        if ( bCurrent ) \
            return GetCapability<CapDataType>(Source, Cap, value, GetCurrentCapValues, func, sLog); \
        return GetCapability<CapDataType>(Source, Cap, value, GetDefaultCapValues, func, sLog); \
    }

#define EXPORT_SET_CAP_VALUE_OPT_CURRENT(FuncName, Cap, CapDataType, CapFn) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, CapDataType value, DTWAIN_BOOL bCurrent)\
    {\
        CTL_StringType sLog = GENERATE_PARAM_LOG((Source, value, bCurrent));\
        CTL_String func = __FUNCTION__;\
        return SetCapability<CapDataType>(CapFn(Source, value, Cap, bCurrent?true:false), func, sLog); \
    }

#define EXPORT_GET_CAP_VALUE_STRING(FuncName) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName##String(DTWAIN_SOURCE Source, LPTSTR value)\
    {\
       return GetCapabilityByString(GetDeviceCapsByStringFn(Source, value, FuncName), __FUNCTION__, GENERATE_PARAM_LOG((Source, value)));\
    }

#define EXPORT_SET_CAP_VALUE_STRING(FuncName) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName##String(DTWAIN_SOURCE Source, LPCTSTR value)\
   {\
       CapSetterFn fn(Source, value, &FuncName);\
       return  SetCapabilityByString(fn, __FUNCTION__, GENERATE_PARAM_LOG((Source, value)));\
   }

#define EXPORT_SET_CAP_VALUE_STRING_2(FuncName) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName##String(DTWAIN_SOURCE Source, LPCTSTR value, DTWAIN_BOOL condition)\
{\
    CapSetterFn2 fn(Source, value, condition?true:false, &FuncName);\
    return  SetCapabilityByString(fn, __FUNCTION__, GENERATE_PARAM_LOG((Source, value, condition)));\
}

#define EXPORT_GET_CAP_VALUE_D(FuncName, Cap) EXPORT_GET_CAP_VALUE(FuncName, Cap, LPDTWAIN_FLOAT, GetCurrentCapValues)
#define EXPORT_GET_CAP_VALUE_I(FuncName, Cap) EXPORT_GET_CAP_VALUE(FuncName, Cap, LPLONG, GetCurrentCapValues)
#define EXPORT_GET_CAP_VALUE_A(FuncName, Cap) EXPORT_GET_CAP_VALUE(FuncName, Cap, LPDTWAIN_ARRAY, GetCurrentCapValues)
#define EXPORT_GET_CAP_VALUE_S(FuncName, Cap, NumChars) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, LPTSTR value)\
    {\
        return GetStringCapability(Source, Cap, value, NumChars, GetCurrentCapValues, __FUNCTION__, GENERATE_PARAM_LOG((Source, value))); \
    }

#define EXPORT_GET_VALUE_OPT_MAXLENGTH_S(FuncName, Cap) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, LPTSTR value, LONG MaxLen)\
    {\
        return GetStringCapability(Source, Cap, value, MaxLen, GetCurrentCapValues, __FUNCTION__, GENERATE_PARAM_LOG((Source, value))); \
    }

#define EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(FuncName, Cap) EXPORT_GET_CAP_VALUE_OPT_CURRENT(FuncName, Cap, LPLONG)
#define EXPORT_GET_CAP_VALUE_OPT_CURRENT_D(FuncName, Cap) EXPORT_GET_CAP_VALUE_OPT_CURRENT(FuncName, Cap, LPDTWAIN_FLOAT)
#define EXPORT_GET_CAP_VALUE_OPT_CURRENT_S(FuncName, Cap, NumChars) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, LPTSTR value, DTWAIN_LONG GetType)\
    {\
        GetCapValuesFn fn = GetAllCapValues;\
        if ( GetType == DTWAIN_CAPGETCURRENT ) \
           fn = GetCurrentCapValues;\
        else\
        if ( GetType == DTWAIN_CAPGETDEFAULT) \
           fn = GetDefaultCapValues;\
        return GetStringCapability(Source, Cap, value, NumChars, fn, __FUNCTION__, GENERATE_PARAM_LOG((Source, value, GetType))); \
    }

#define EXPORT_SET_CAP_VALUE_D(FuncName, Cap) EXPORT_SET_CAP_VALUE(FuncName, Cap, DTWAIN_FLOAT, SetSupportFn1<DTWAIN_FLOAT>)
#define EXPORT_SET_CAP_VALUE_I(FuncName, Cap) EXPORT_SET_CAP_VALUE(FuncName, Cap, LONG, SetSupportFn1<LONG>)
#define EXPORT_SET_CAP_VALUE_S(FuncName, Cap) EXPORT_SET_CAP_VALUE(FuncName, Cap, LPCTSTR, SetSupportFn1<LPCTSTR>)
#define EXPORT_SET_CAP_VALUE_OPT_CURRENT_I(FuncName, Cap) EXPORT_SET_CAP_VALUE_OPT_CURRENT(FuncName, Cap, LONG, SetSupportFn1<LONG>)
#define EXPORT_SET_CAP_VALUE_A(FuncName, Cap) EXPORT_SET_CAP_VALUE(FuncName, Cap, DTWAIN_ARRAY, SetSupportFn2<DTWAIN_ARRAY>)

#define EXPORT_SET_CAP_VALUE_D_I_EX(FuncName, Cap1, Cap2, paramEx) \
    EXPORT_SET_CAP_VALUE_PARM3_2(FuncName, Cap1, Cap2, DTWAIN_FLOAT, LONG, paramEx, SetSupportFn1<DTWAIN_FLOAT>)

#define EXPORT_IS_CAP_SUPPORTED_I(FuncName, Cap) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, LONG CapValue)\
    { return IsSupportedImpl(Source, Cap, (CapValue==DTWAIN_ANYSUPPORT), CapValue, __FUNCTION__, GENERATE_PARAM_LOG((Source, CapValue)))?TRUE:FALSE; }

#define EXPORT_IS_CAP_SUPPORTED_D(FuncName, Cap) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source, DTWAIN_FLOAT CapValue)\
    { return IsSupportedImpl(Source, Cap, false, CapValue, __FUNCTION__, GENERATE_PARAM_LOG((Source, CapValue)))?TRUE:FALSE; }

#define EXPORT_IS_CAP_SUPPORTED_I_1(FuncName, Cap) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source)\
    { return IsSupportedImpl(Source, Cap, true, 0, __FUNCTION__, GENERATE_PARAM_LOG((Source)))?TRUE:FALSE; }

#define EXPORT_IS_CAP_ENABLED(FuncName, Cap) \
    DTWAIN_BOOL DLLENTRY_DEF FuncName(DTWAIN_SOURCE Source)\
    { return IsEnabledImpl(Source, Cap, __FUNCTION__, GENERATE_PARAM_LOG((Source)))?TRUE:FALSE; }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsAutoBorderDetectSupported, DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsAutoBrightSupported, DTWAIN_CV_ICAPAUTOBRIGHT)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsAutoDeskewSupported, DTWAIN_CV_ICAPAUTOMATICDESKEW)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsAutoFeedSupported,  DTWAIN_CV_CAPAUTOFEED)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsAutoRotateSupported, DTWAIN_CV_ICAPAUTOROTATE)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsCompressionSupported, DTWAIN_CV_ICAPCOMPRESSION)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsCustomDSDataSupported, DTWAIN_CV_CAPCUSTOMDSDATA)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsDeviceEventSupported, DTWAIN_CV_CAPDEVICEEVENT)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsDoubleFeedDetectSupported, DTWAIN_CV_CAPDOUBLEFEEDDETECTION)
EXPORT_IS_CAP_SUPPORTED_D(DTWAIN_IsDoubleFeedDetectLengthSupported, DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsExtImageInfoSupported, DTWAIN_CV_ICAPEXTIMAGEINFO)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsFeederSupported, DTWAIN_CV_CAPFEEDERENABLED)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsFileXferSupported,  DTWAIN_CV_ICAPIMAGEFILEFORMAT)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsIndicatorSupported, DTWAIN_CV_CAPINDICATORS)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsLampSupported, DTWAIN_CV_ICAPLAMPSTATE)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsLightPathSupported, DTWAIN_CV_ICAPLIGHTPATH)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsLightSourceSupported, DTWAIN_CV_ICAPLIGHTSOURCE)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsMaxBuffersSupported, DTWAIN_CV_CAPMAXBATCHBUFFERS)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsOrientationSupported, DTWAIN_CV_ICAPORIENTATION)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsOverscanSupported, DTWAIN_CV_ICAPOVERSCAN)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsPaperSizeSupported, DTWAIN_CV_ICAPSUPPORTEDSIZES)
EXPORT_IS_CAP_SUPPORTED_I(DTWAIN_IsPatchSupported, DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsPrinterSupported, DTWAIN_CV_CAPPRINTER)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsRotationSupported, DTWAIN_CV_ICAPROTATION)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsThumbnailSupported, DTWAIN_CV_CAPTHUMBNAILSENABLED)
EXPORT_IS_CAP_SUPPORTED_I_1(DTWAIN_IsUIOnlySupported, DTWAIN_CV_CAPENABLEDSUIONLY)

EXPORT_IS_CAP_ENABLED(DTWAIN_IsAutoBorderDetectEnabled, DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsAutoBrightEnabled, DTWAIN_CV_ICAPAUTOBRIGHT)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsAutoDeskewEnabled, DTWAIN_CV_ICAPAUTOMATICDESKEW)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsAutoRotateEnabled, DTWAIN_CV_ICAPAUTOMATICROTATE)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsAutoScanEnabled, DTWAIN_CV_CAPAUTOSCAN)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsDeviceOnLine, DTWAIN_CV_CAPDEVICEONLINE)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsDuplexEnabled, DTWAIN_CV_CAPDUPLEXENABLED)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsIndicatorEnabled, DTWAIN_CV_CAPINDICATORS)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsLampEnabled, DTWAIN_CV_ICAPLAMPSTATE)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsPaperDetectable, DTWAIN_CV_CAPPAPERDETECTABLE)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsPatchDetectEnabled, DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED)
EXPORT_IS_CAP_ENABLED(DTWAIN_IsThumbnailEnabled, DTWAIN_CV_CAPTHUMBNAILSENABLED)

EXPORT_GET_CAP_VALUE_I(DTWAIN_GetAlarmVolume, DTWAIN_CV_CAPALARMVOLUME)
EXPORT_GET_CAP_VALUE_S(DTWAIN_GetAuthor, DTWAIN_CV_CAPAUTHOR, 128)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetBatteryMinutes, DTWAIN_CV_CAPBATTERYMINUTES)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetBatteryPercent, DTWAIN_CV_CAPBATTERYPERCENTAGE)
EXPORT_GET_CAP_VALUE_D(DTWAIN_GetBrightness, DTWAIN_CV_ICAPBRIGHTNESS)
EXPORT_GET_CAP_VALUE_S(DTWAIN_GetCaption, DTWAIN_CV_CAPCAPTION, 255)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetCompressionType, DTWAIN_CV_ICAPCOMPRESSION)
EXPORT_GET_CAP_VALUE_D(DTWAIN_GetContrast, DTWAIN_CV_ICAPCONTRAST)
EXPORT_GET_CAP_VALUE_S(DTWAIN_GetDeviceTimeDate, DTWAIN_CV_CAPDEVICETIMEDATE, 32)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_D(DTWAIN_GetDoubleFeedDetectLength, DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH)
EXPORT_GET_CAP_VALUE_A(DTWAIN_GetDoubleFeedDetectValues, DTWAIN_CV_CAPDOUBLEFEEDDETECTION)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetFeederAlignment,  DTWAIN_CV_CAPFEEDERALIGNMENT)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetFeederOrder, DTWAIN_CV_CAPFEEDERORDER)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_S(DTWAIN_GetHalftone, DTWAIN_CV_ICAPHALFTONES, 128)
EXPORT_GET_CAP_VALUE_D(DTWAIN_GetHighlight, DTWAIN_CV_ICAPHIGHLIGHT)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetJobControl, DTWAIN_CV_CAPJOBCONTROL)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetLightPath, DTWAIN_CV_ICAPLIGHTPATH)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetLightSource, DTWAIN_CV_ICAPLIGHTSOURCE)
EXPORT_GET_CAP_VALUE_A(DTWAIN_GetLightSources, DTWAIN_CV_ICAPLIGHTSOURCE)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetNoiseFilter,  DTWAIN_CV_ICAPNOISEFILTER)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetOrientation, DTWAIN_CV_ICAPORIENTATION)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetMaxBuffers, DTWAIN_CV_CAPMAXBATCHBUFFERS)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetPaperSize, DTWAIN_CV_ICAPSUPPORTEDSIZES)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetPatchMaxPriorities, DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetPatchMaxRetries, DTWAIN_CV_ICAPPATCHCODEMAXRETRIES)
EXPORT_GET_CAP_VALUE_A(DTWAIN_GetPatchPriorities, DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetPatchSearchMode, DTWAIN_CV_ICAPPATCHCODESEARCHMODE)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetPatchTimeOut, DTWAIN_CV_ICAPPATCHCODETIMEOUT)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetPixelFlavor, DTWAIN_CV_ICAPPIXELFLAVOR)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetPrinter, DTWAIN_CV_CAPPRINTER)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetPrinterStartNumber, DTWAIN_CV_CAPPRINTERINDEX)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetPrinterStringMode, DTWAIN_CV_CAPPRINTERMODE)
EXPORT_GET_CAP_VALUE_A(DTWAIN_GetPrinterStrings, DTWAIN_CV_CAPPRINTERSTRING)
EXPORT_GET_VALUE_OPT_MAXLENGTH_S(DTWAIN_GetPrinterSuffixString, DTWAIN_CV_CAPPRINTERSUFFIX)
EXPORT_GET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_GetOverscan, DTWAIN_CV_ICAPOVERSCAN)
EXPORT_GET_CAP_VALUE_D(DTWAIN_GetRotation, DTWAIN_CV_ICAPROTATION)
EXPORT_GET_CAP_VALUE_D(DTWAIN_GetShadow, DTWAIN_CV_ICAPSHADOW)
EXPORT_GET_CAP_VALUE_I(DTWAIN_GetSourceUnit, DTWAIN_CV_ICAPUNITS)

EXPORT_GET_CAP_VALUE_D(DTWAIN_GetThreshold, DTWAIN_CV_ICAPTHRESHOLD)
EXPORT_GET_CAP_VALUE_S(DTWAIN_GetTimeDate, DTWAIN_CV_CAPTIMEDATE, 32)
EXPORT_GET_CAP_VALUE_D(DTWAIN_GetXResolution, DTWAIN_CV_ICAPXRESOLUTION)
EXPORT_GET_CAP_VALUE_D(DTWAIN_GetYResolution, DTWAIN_CV_ICAPYRESOLUTION)

EXPORT_SET_CAP_VALUE_I(DTWAIN_ClearBuffers, DTWAIN_CV_CAPCLEARBUFFERS)

EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableAutoBorderDetect, DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableAutoBright, DTWAIN_CV_ICAPAUTOBRIGHT)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableAutoDeskew, DTWAIN_CV_ICAPAUTOMATICDESKEW)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableAutoRotate, DTWAIN_CV_ICAPAUTOMATICROTATE)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableAutoScan, DTWAIN_CV_CAPAUTOSCAN)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableDuplex, DTWAIN_CV_CAPDUPLEXENABLED)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableIndicator, DTWAIN_CV_CAPINDICATORS)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableLamp, DTWAIN_CV_ICAPLAMPSTATE)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnablePatchDetect, DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnablePrinter, DTWAIN_CV_CAPPRINTERENABLED)
EXPORT_SET_CAP_VALUE_I(DTWAIN_EnableThumbnail, DTWAIN_CV_CAPTHUMBNAILSENABLED)
EXPORT_SET_CAP_VALUE_A(DTWAIN_SetAlarms, DTWAIN_CV_CAPALARMS)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetAlarmVolume, DTWAIN_CV_CAPALARMVOLUME)
EXPORT_SET_CAP_VALUE_S(DTWAIN_SetAuthor, DTWAIN_CV_CAPAUTHOR)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetBrightness, DTWAIN_CV_ICAPBRIGHTNESS)
EXPORT_SET_CAP_VALUE_S(DTWAIN_SetCaption, DTWAIN_CV_CAPCAPTION)
EXPORT_SET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_SetCompressionType, DTWAIN_CV_ICAPCOMPRESSION)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetContrast, DTWAIN_CV_ICAPCONTRAST)
EXPORT_SET_CAP_VALUE_S(DTWAIN_SetDeviceTimeDate, DTWAIN_CV_CAPDEVICETIMEDATE)
EXPORT_SET_CAP_VALUE_A(DTWAIN_SetDoubleFeedDetectValues, DTWAIN_CV_CAPDOUBLEFEEDDETECTION)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetDoubleFeedDetectLength, DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetFeederAlignment, DTWAIN_CV_CAPFEEDERALIGNMENT)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetFeederOrder,  DTWAIN_CV_CAPFEEDERORDER)
EXPORT_SET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_SetFileXferFormat, DTWAIN_CV_ICAPIMAGEFILEFORMAT)
EXPORT_SET_CAP_VALUE_S(DTWAIN_SetHalftone, DTWAIN_CV_ICAPHALFTONES)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetHighlight, DTWAIN_CV_ICAPHIGHLIGHT)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetLightPath, DTWAIN_CV_ICAPLIGHTPATH)
EXPORT_SET_CAP_VALUE_A(DTWAIN_SetLightPathEx, DTWAIN_CV_ICAPLIGHTPATH)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetLightSource, DTWAIN_CV_ICAPLIGHTSOURCE)
EXPORT_SET_CAP_VALUE_A(DTWAIN_SetLightSources, DTWAIN_CV_ICAPLIGHTSOURCE)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetMaxBuffers, DTWAIN_CV_CAPMAXBATCHBUFFERS)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetNoiseFilter, DTWAIN_CV_ICAPNOISEFILTER)
EXPORT_SET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_SetOrientation, DTWAIN_CV_ICAPORIENTATION)
EXPORT_SET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_SetOverscan, DTWAIN_CV_ICAPOVERSCAN)
EXPORT_SET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_SetPaperSize, DTWAIN_CV_ICAPSUPPORTEDSIZES)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetPatchMaxPriorities, DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetPatchMaxRetries,  DTWAIN_CV_ICAPPATCHCODEMAXRETRIES)
EXPORT_SET_CAP_VALUE_A(DTWAIN_SetPatchPriorities, DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetPatchSearchMode, DTWAIN_CV_ICAPPATCHCODESEARCHMODE)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetPatchTimeOut, DTWAIN_CV_ICAPPATCHCODETIMEOUT)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetPixelFlavor, DTWAIN_CV_ICAPPIXELFLAVOR)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetPrinterStartNumber, DTWAIN_CV_CAPPRINTERINDEX)
EXPORT_SET_CAP_VALUE_S(DTWAIN_SetPrinterSuffixString, DTWAIN_CV_CAPPRINTERSUFFIX)
EXPORT_SET_CAP_VALUE_OPT_CURRENT_I(DTWAIN_SetPrinterStringMode, DTWAIN_CV_CAPPRINTERMODE)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetRotation, DTWAIN_CV_ICAPROTATION)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetShadow, DTWAIN_CV_ICAPSHADOW)
EXPORT_SET_CAP_VALUE_I(DTWAIN_SetSourceUnit, DTWAIN_CV_ICAPUNITS)
EXPORT_SET_CAP_VALUE_D_I_EX(DTWAIN_SetThreshold, DTWAIN_CV_ICAPTHRESHOLD, DTWAIN_CV_ICAPBITDEPTHREDUCTION, TWBR_THRESHOLD)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetXResolution, DTWAIN_CV_ICAPXRESOLUTION)
EXPORT_SET_CAP_VALUE_D(DTWAIN_SetYResolution, DTWAIN_CV_ICAPYRESOLUTION)

EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetBrightness)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetContrast)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetDoubleFeedDetectLength)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetHighlight)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetResolution)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetRotation)
EXPORT_SET_CAP_VALUE_STRING_2(DTWAIN_SetThreshold)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetShadow)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetXResolution)
EXPORT_SET_CAP_VALUE_STRING(DTWAIN_SetYResolution)

EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetBrightness)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetContrast)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetHighlight)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetResolution)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetRotation)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetShadow)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetThreshold)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetXResolution)
EXPORT_GET_CAP_VALUE_STRING(DTWAIN_GetYResolution)

// Exported function implementations for getting the cap values
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumDoubleFeedDetectLengths, DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumAudioXferMechs, DTWAIN_CV_ACAPXFERMECH)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumAlarms, DTWAIN_CV_CAPALARMS)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumAlarmVolumes, DTWAIN_CV_CAPALARMVOLUME)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumAutoFeedValues, DTWAIN_CV_CAPAUTOFEED)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumAutomaticCaptures, DTWAIN_CV_CAPAUTOMATICCAPTURE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumAutomaticSenseMedium, DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumBitDepths, DTWAIN_CV_ICAPBITDEPTH)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumBrightnessValues, DTWAIN_CV_ICAPBRIGHTNESS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumCompressionTypes, DTWAIN_CV_ICAPCOMPRESSION)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumContrastValues, DTWAIN_CV_ICAPCONTRAST)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumDoubleFeedDetectValues, DTWAIN_CV_CAPDOUBLEFEEDDETECTION)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumExtendedCapsEx, DTWAIN_CV_CAPEXTENDEDCAPS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumFileXferFormats,DTWAIN_CV_ICAPIMAGEFILEFORMAT)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumHalftones, DTWAIN_CV_ICAPHALFTONES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumJobControls, DTWAIN_CV_CAPJOBCONTROL)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumLightPaths, DTWAIN_CV_ICAPLIGHTPATH)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumLightSources, DTWAIN_CV_ICAPLIGHTSOURCE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumNoiseFilters, DTWAIN_CV_ICAPNOISEFILTER)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumOrientations, DTWAIN_CV_ICAPORIENTATION)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumOverscanValues, DTWAIN_CV_ICAPOVERSCAN)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPatchCodes, DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPatchMaxPriorities, DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPatchPriorities, DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPatchSearchModes, DTWAIN_CV_ICAPPATCHCODESEARCHMODE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPaperSizes, DTWAIN_CV_ICAPSUPPORTEDSIZES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPatchMaxRetries, DTWAIN_CV_ICAPPATCHCODEMAXRETRIES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPatchTimeOutValues, DTWAIN_CV_ICAPPATCHCODETIMEOUT)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumPrinterStringModes, DTWAIN_CV_CAPPRINTERMODE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumSourceUnits, ICAP_UNITS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumSupportedCapsEx, CAP_SUPPORTEDCAPS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumTwainPrinters, DTWAIN_CV_CAPPRINTER)
EXPORT_ENUM_CAP_VALUES_NOEXPAND(DTWAIN_EnumTwainPrintersArray, DTWAIN_CV_CAPPRINTER)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumHighlightValues, DTWAIN_CV_ICAPHIGHLIGHT)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumMaxBuffers, DTWAIN_CV_CAPMAXBATCHBUFFERS)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumResolutionValues, DTWAIN_CV_ICAPXRESOLUTION)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumShadowValues, DTWAIN_CV_ICAPSHADOW)
EXPORT_ENUM_CAP_VALUES(DTWAIN_EnumThresholdValues, DTWAIN_CV_ICAPTHRESHOLD)

EXPORT_SET_CAP_VALUE_D_D(DTWAIN_SetResolution, DTWAIN_CV_ICAPXRESOLUTION, DTWAIN_CV_ICAPYRESOLUTION)


EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumDoubleFeedDetectLengthsEx, DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumAudioXferMechsEx, DTWAIN_CV_ACAPXFERMECH)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumAlarmsEx, DTWAIN_CV_CAPALARMS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumSupportedCapsEx2, CAP_SUPPORTEDCAPS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumExtendedCapsEx2, DTWAIN_CV_CAPEXTENDEDCAPS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPaperSizesEx, DTWAIN_CV_ICAPSUPPORTEDSIZES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumSourceUnitsEx, ICAP_UNITS)

EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumAlarmVolumesEx, DTWAIN_CV_CAPALARMVOLUME)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumAutoFeedValuesEx, DTWAIN_CV_CAPAUTOFEED)
EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumAutomaticCapturesEx, DTWAIN_CV_CAPAUTOMATICCAPTURE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumAutomaticSenseMediumEx, DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumBitDepthsEx2, DTWAIN_CV_ICAPBITDEPTH)

EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumBrightnessValuesEx, DTWAIN_CV_ICAPBRIGHTNESS)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumCompressionTypesEx, DTWAIN_CV_ICAPCOMPRESSION)
EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumContrastValuesEx, DTWAIN_CV_ICAPCONTRAST)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumDoubleFeedDetectValuesEx, DTWAIN_CV_CAPDOUBLEFEEDDETECTION)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumFileXferFormatsEx, DTWAIN_CV_ICAPIMAGEFILEFORMAT)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumHalftonesEx, DTWAIN_CV_ICAPHALFTONES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumJobControlsEx, DTWAIN_CV_CAPJOBCONTROL)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumLightPathsEx, DTWAIN_CV_ICAPLIGHTPATH)

EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumLightSourcesEx, DTWAIN_CV_ICAPLIGHTSOURCE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumNoiseFiltersEx, DTWAIN_CV_ICAPNOISEFILTER)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumOrientationsEx, DTWAIN_CV_ICAPORIENTATION)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumOverscanValuesEx, DTWAIN_CV_ICAPOVERSCAN)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPatchCodesEx, DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPatchMaxPrioritiesEx, DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES)

EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPatchPrioritiesEx, DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPatchSearchModesEx, DTWAIN_CV_ICAPPATCHCODESEARCHMODE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPatchMaxRetriesEx, DTWAIN_CV_ICAPPATCHCODEMAXRETRIES)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPatchTimeOutValuesEx, DTWAIN_CV_ICAPPATCHCODETIMEOUT)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumPrinterStringModesEx, DTWAIN_CV_CAPPRINTERMODE)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumTwainPrintersEx, DTWAIN_CV_CAPPRINTER)
EXPORT_ENUM_CAP_VALUES_NOEXPAND_EX(DTWAIN_EnumTwainPrintersArrayEx, DTWAIN_CV_CAPPRINTER)
EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumHighlightValuesEx, DTWAIN_CV_ICAPHIGHLIGHT)
EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumMaxBuffersEx, DTWAIN_CV_CAPMAXBATCHBUFFERS)
EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumResolutionValuesEx, DTWAIN_CV_ICAPXRESOLUTION)
EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumShadowValuesEx, DTWAIN_CV_ICAPSHADOW)
EXPORT_ENUM_CAP_VALUES_EX(DTWAIN_EnumThresholdValuesEx, DTWAIN_CV_ICAPTHRESHOLD)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumSourceValues(DTWAIN_SOURCE Source, LPCTSTR capName, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL expandIfRange)
{
    LOG_FUNC_ENTRY_PARAMS((Source, capName, pArray, expandIfRange))
    DTWAIN_BOOL retVal = DTWAIN_GetCapValues(Source, CTL_TwainAppMgr::GetCapFromCapName(StringConversion::Convert_NativePtr_To_Ansi(capName).c_str()), DTWAIN_CAPGET, pArray);
    LOG_FUNC_EXIT_PARAMS(retVal)
    CATCH_BLOCK(false)
}

///////////////// These functions are high-level capability functions ///////////////
DTWAIN_BOOL dynarithmic::DTWAIN_SetDeviceCapByString(DTWAIN_SOURCE Source, LPCTSTR strVal, SetByStringFn fn)
{
    DTWAIN_FLOAT value = 0.0;
    if ( strVal )
        value = StringWrapper::ToDouble(strVal);
    return fn(Source, value);
}

DTWAIN_BOOL dynarithmic::DTWAIN_SetDeviceCapByString2(DTWAIN_SOURCE Source, LPCTSTR strVal, bool bExtra, SetByStringFn2 fn)
{
    DTWAIN_FLOAT value = 0.0;
    if ( strVal )
        value = StringWrapper::ToDouble(strVal);
    return fn(Source, value, bExtra);
}

DTWAIN_BOOL dynarithmic::DTWAIN_GetDeviceCapByString(DTWAIN_SOURCE Source, LPTSTR strVal, GetByStringFn fn)
{
    DTWAIN_FLOAT tempR;
    DTWAIN_BOOL retVal = fn(Source, &tempR);
    if ( retVal )
    {
        CTL_StringStreamType strm;
        strm << BOOST_FORMAT(_T("%1%")) % tempR;
        StringWrapper::SafeStrcpy(strVal, strm.str().c_str());
    }
    return retVal;
}

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetResolution(DTWAIN_SOURCE Source, LPDTWAIN_FLOAT Resolution)
{
    LOG_FUNC_ENTRY_PARAMS((Source, Resolution))
	LONG lCap = 0;
    if ( DTWAIN_IsCapSupported( Source, DTWAIN_CV_ICAPXRESOLUTION))
        lCap = DTWAIN_CV_ICAPXRESOLUTION;
    else
    if ( DTWAIN_IsCapSupported( Source, DTWAIN_CV_ICAPXNATIVERESOLUTION))
         lCap = DTWAIN_CV_ICAPXNATIVERESOLUTION;
    else
        LOG_FUNC_EXIT_PARAMS(FALSE)
    DTWAIN_BOOL bRet = GetDoubleCap( Source, lCap, Resolution);
    LOG_FUNC_EXIT_PARAMS(bRet)
    CATCH_BLOCK(FALSE)
}



static bool GetDoubleCap( DTWAIN_SOURCE Source, LONG lCap, double *pValue )
{
    double *pRealValue = pValue;
    if (DTWAIN_GetCapDataType(Source, lCap) != TWTY_FIX32)
        return false;
    DTWAIN_ARRAY Array = 0;
    bool bRet = false;
    bRet = DTWAIN_GetCapValues(Source, lCap, DTWAIN_CAPGETCURRENT, &Array)?true:false;
    DTWAINArrayLL_RAII arr(Array);
    auto vIn = EnumeratorVectorPtr<double>(Array);
    if ( bRet && Array )
    {
        if ( vIn->empty() )
            bRet = false;
        else
            *pRealValue = (*vIn)[0];
    }
    return bRet;
}

static LONG GetCapValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG lCap, LONG GetType, DTWAIN_BOOL bExpandRange)
{
    LOG_FUNC_ENTRY_PARAMS((Source, pArray, lCap, bExpandRange))
    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    CTL_ITwainSource *p = VerifySourceHandle(pHandle, Source);
    LONG nValues = 0;

    // See if DLL Handle exists
    if (!pHandle)
        DTWAIN_Check_Bad_Handle_Ex(pHandle, 0, FUNC_MACRO);
    // See if Source is opened
    DTWAIN_Check_Error_Condition_0_Ex(pHandle, [&]{return !CTL_TwainAppMgr::IsSourceOpen(p); }, DTWAIN_ERR_SOURCE_NOT_OPEN, 0, FUNC_MACRO);

    if (p)
    {
        DTWAIN_ARRAY OrigVals = NULL;

        // we may use a brand new array
        LPDTWAIN_ARRAY arrayToUse = &OrigVals;

        // use what was passed in if it isn't null
        if ( pArray )
            arrayToUse = pArray;

        DTWAINArrayPtr_RAII a(&OrigVals);

        // get the capability values
        if (DTWAIN_GetCapValues(Source, lCap, GetType, arrayToUse))
        {
            // Gotten the value.  Check what container type holds the data
            LONG lContainer = DTWAIN_GetCapContainer(Source, lCap, GetType);
            switch (lContainer)
            {
                case DTWAIN_CONTRANGE:
                {
                    if (bExpandRange)
                    {
                        if (pArray)
                        {
                            // we need to expand to a temporary
                            DTWAIN_ARRAY tempArray = 0;

                            // throw this away when done
                            DTWAINArrayPtr_RAII aTemp(&tempArray);

                            // expand the range into a temp array
                            DTWAIN_RangeExpand(pArray, &tempArray);

                            // destroy original and copy new values
                            EnumeratorFunctionImpl::EnumeratorDestroy(*pArray);
                            *pArray = DTWAIN_ArrayCreateCopy(tempArray);

                            // get the count
                            nValues = DTWAIN_ArrayGetCount(*pArray);
                        }
                        else
                        {
                            // there was no original array
                            DTWAIN_ARRAY TempVals;
                            DTWAIN_RangeExpand(OrigVals, &TempVals);
                            DTWAINArrayLL_RAII arr(TempVals);
                            nValues = DTWAIN_ArrayGetCount(TempVals);
                        }
                    }
                    else
                    {
                        nValues = DTWAIN_ArrayGetCount(*arrayToUse);
                    }
                }
                break;
                case DTWAIN_CONTENUMERATION:
                case DTWAIN_CONTONEVALUE:
                case DTWAIN_CONTARRAY:
                {
                    nValues = DTWAIN_ArrayGetCount(*arrayToUse);
                }
            }
        }
        LOG_FUNC_EXIT_PARAMS(nValues);
    }
    LOG_FUNC_EXIT_PARAMS(0);
    CATCH_BLOCK(DTWAIN_FAILURE1)
}

static LONG GetCurrentCapValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG lCap, DTWAIN_BOOL bExpandRange)
{ return GetCapValues(Source, pArray, lCap, DTWAIN_CAPGETCURRENT, bExpandRange); }

static LONG GetDefaultCapValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG lCap, DTWAIN_BOOL bExpandRange)
{ return GetCapValues(Source, pArray, lCap, DTWAIN_CAPGETDEFAULT, bExpandRange); }

static LONG GetAllCapValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, LONG lCap, DTWAIN_BOOL bExpandRange)
{ return GetCapValues(Source, pArray, lCap, DTWAIN_CAPGET, bExpandRange); }
