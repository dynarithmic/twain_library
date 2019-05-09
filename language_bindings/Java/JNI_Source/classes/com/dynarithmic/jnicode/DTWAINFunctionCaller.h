#ifndef DTWAINFUNCTIONCALLER_H
#define DTWAINFUNCTIONCALLER_H

#include <map>
#include <jni.h>
#include "DTWAINGlobalFn.h"
#include "DTWAINRAII.h"
#include "JavaArrayTraits.h"

template <typename FnGlobalVPtrType, typename FnGlobalType>
static std::pair<typename FnGlobalType::DTWAINFN_Map::iterator, bool>
FnPreamble(FnGlobalVPtrType& gType, const std::string& funcName)
{
	if ( !IsModuleInitialized() )
		throw "DTwain Module not loaded";
	AddToFunctionCounter(funcName);
	typename FnGlobalType::DTWAINFN_Map::iterator it = gType->m_FnMap.find(funcName);
	return std::make_pair(it, (it != gType->m_FnMap.end()));
}

template <typename FnGlobalVPtrType, typename FnGlobalType>
typename FnGlobalType::return_type CallFn0(FnGlobalVPtrType& gType, const std::string& funcName)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)();
	throw "Function Not Found";
}


template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg>
typename FnGlobalType::return_type CallFn1(FnGlobalVPtrType& gType, const std::string& funcName, Arg a1)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2>
typename FnGlobalType::return_type CallFn2(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename Arg3>
typename FnGlobalType::return_type CallFn3(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2, Arg3 a3)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2, a3);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename Arg3, typename Arg4>
typename FnGlobalType::return_type CallFn4(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2, Arg3 a3, Arg4 a4)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2, a3, a4);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename Arg3, typename Arg4, typename Arg5>
typename FnGlobalType::return_type CallFn5(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2, Arg3 a3, Arg4 a4, Arg5 a5)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2, a3, a4, a5);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename Arg3, typename Arg4, typename Arg5, typename Arg6>
typename FnGlobalType::return_type CallFn6(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2, Arg3 a3, Arg4 a4, Arg5 a5, Arg6 a6)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2, a3, a4, a5, a6);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename Arg3, typename Arg4, typename Arg5, typename Arg6, typename Arg7>
typename FnGlobalType::return_type CallFn7(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2, Arg3 a3, Arg4 a4, Arg5 a5, Arg6 a6, Arg7 a7)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2, a3, a4, a5, a6, a7);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename Arg3, typename Arg4, typename Arg5, typename Arg6, typename Arg7, typename Arg8>
typename FnGlobalType::return_type CallFn8(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2, Arg3 a3, Arg4 a4, Arg5 a5, Arg6 a6, Arg7 a7, Arg8 a8)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2, a3, a4, a5, a6, a7, a8);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename Arg3, typename Arg4, typename Arg5, typename Arg6, typename Arg7, typename Arg8, typename Arg9>
typename FnGlobalType::return_type CallFn9(FnGlobalVPtrType& gType, const std::string& funcName, Arg1 a1, Arg2 a2, Arg3 a3, Arg4 a4, Arg5 a5, Arg6 a6, Arg7 a7, Arg8 a8, Arg9 a9)
{
	typename FnGlobalType::DTWAINFN_Map::iterator it = 
		FnPreamble<FnGlobalVPtrType, FnGlobalType>(gType, funcName).first;
	if ( it != gType->m_FnMap.end() )
		return (*it->second)(a1, a2, a3, a4, a5, a6, a7, a8, a9);
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg, typename JavaTraits>
typename JavaTraits::array_type CallFnReturnArray1(JNIEnv* env, FnGlobalVPtrType& gType, 
												   const std::string& funcName, Arg a1)
{
	if ( !IsModuleInitialized() )
		throw "DTwain Module not loaded";
	AddToFunctionCounter(funcName);
	typename FnGlobalType::DTWAINFN_Map::iterator it = gType->m_FnMap.find(funcName);
	if ( it != gType->m_FnMap.end() )
	{
		DTWAIN_ARRAY A=0;
		BOOL bRet = (*it->second)(a1, &A);
		DTWAINArray_RAII arr(A);    
		return CreateJArrayFromDTWAINArray<JavaTraits>(env, A, bRet?true:false);
	}
	throw "Function Not Found";
}

template <typename FnGlobalVPtrType, typename FnGlobalType, typename Arg1, typename Arg2, typename JavaTraits>
typename JavaTraits::array_type CallFnReturnArray2(JNIEnv* env, FnGlobalVPtrType& gType, 
												   const std::string& funcName, Arg1 a1, Arg2 a2)
{
	if ( !IsModuleInitialized() )
		throw "DTwain Module not loaded";
	AddToFunctionCounter(funcName);
	typename FnGlobalType::DTWAINFN_Map::iterator it = gType->m_FnMap.find(funcName);
	if ( it != gType->m_FnMap.end() )
	{
		DTWAIN_ARRAY A=0;
		BOOL bRet = (*it->second)(a1, &A, a2);
		DTWAINArray_RAII arr(A);    
		return CreateJArrayFromDTWAINArray<JavaTraits> (env, A, bRet?true:false);
	}
	throw "Function Not Found";
}

#define DTWAIN_FUNCTION_CALLER0(FnName, FnType) CallFn0<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName))
#define DTWAIN_FUNCTION_CALLER1(FnName, FnType, a1) CallFn1<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1))
#define DTWAIN_FUNCTION_CALLER2(FnName, FnType, a1, a2) CallFn2<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2))
#define DTWAIN_FUNCTION_CALLER3(FnName, FnType, a1, a2, a3) CallFn3<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2), (a3))
#define DTWAIN_FUNCTION_CALLER4(FnName, FnType, a1, a2, a3, a4) CallFn4<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2), (a3), (a4))
#define DTWAIN_FUNCTION_CALLER5(FnName, FnType, a1, a2, a3, a4, a5) CallFn5<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2), (a3), (a4), (a5))
#define DTWAIN_FUNCTION_CALLER6(FnName, FnType, a1, a2, a3, a4, a5, a6) CallFn6<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2), (a3), (a4), (a5), (a6))
#define DTWAIN_FUNCTION_CALLER7(FnName, FnType, a1, a2, a3, a4, a5, a6, a7) CallFn7<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2), (a3), (a4), (a5), (a6), (a7))
#define DTWAIN_FUNCTION_CALLER8(FnName, FnType, a1, a2, a3, a4, a5, a6, a7, a8) CallFn8<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2), (a3), (a4), (a5), (a6), (a7), (a8))
#define DTWAIN_FUNCTION_CALLER9(FnName, FnType, a1, a2, a3, a4, a5, a6, a7, a8, a9) CallFn9<FnGlobal##FnType##Ptr, FnGlobal##FnType>(g_JNIGlobals.g_##FnType##Map, \
	NAME_TO_STRING(FnName),(a1), (a2), (a3), (a4), (a5), (a6), (a7), (a8), (a9))

#endif