#ifndef JAVAARRAYTRAITS_H
#define JAVAARRAYTRAITS_H

#include <jni.h>
#include <vector>
#include <algorithm>
#include "dtwain.h"

struct JavaLongArrayTraits
{
	typedef jlongArray array_type;
	typedef jlong base_type;
	typedef LONG api_base_type;
	enum {dtwain_array_type = DTWAIN_ARRAYLONG};

	static array_type CreateJArray(JNIEnv* env, int nCount)
	{ return env->NewLongArray(nCount); }

	static void SetJArrayRegion(JNIEnv* env, array_type ret, int nCount, const std::vector<base_type>& vj)
	{ env->SetLongArrayRegion(ret, 0, nCount, &vj[0]); }

	static base_type* GetJArrayElements(JNIEnv *env, array_type arr)
	{ return env->GetLongArrayElements(arr, 0); }
};

struct JavaLong64ArrayTraits
{
    typedef jlongArray array_type;
    typedef jlong base_type;
    typedef int64_t api_base_type;
    enum { dtwain_array_type = DTWAIN_ARRAYLONG64 };

    static array_type CreateJArray(JNIEnv* env, int nCount)
    {
        return env->NewLongArray(nCount);
    }

    static void SetJArrayRegion(JNIEnv* env, array_type ret, int nCount, const std::vector<base_type>& vj)
    {
        env->SetLongArrayRegion(ret, 0, nCount, &vj[0]);
    }

    static base_type* GetJArrayElements(JNIEnv *env, array_type arr)
    {
        return env->GetLongArrayElements(arr, 0);
    }
};

struct JavaIntArrayTraits
{
	typedef jintArray array_type;
	typedef jint base_type;
	typedef LONG api_base_type;
	enum {dtwain_array_type = DTWAIN_ARRAYLONG};

	static array_type CreateJArray(JNIEnv* env, int nCount)
	{ return env->NewIntArray(nCount); }

	static void SetJArrayRegion(JNIEnv* env, array_type ret, int nCount, const std::vector<base_type>& vj)
	{ env->SetIntArrayRegion(ret, 0, nCount, &vj[0]); }

	static base_type* GetJArrayElements(JNIEnv *env, array_type arr)
	{ return env->GetIntArrayElements(arr, 0); }
};

struct JavaDoubleArrayTraits
{
	typedef jdoubleArray array_type;
	typedef jdouble base_type;
	typedef double api_base_type;
	enum {dtwain_array_type = DTWAIN_ARRAYFLOAT};

	static jdoubleArray CreateJArray(JNIEnv* env, int nCount)
	{ return env->NewDoubleArray(nCount); }

	static void SetJArrayRegion(JNIEnv* env, jdoubleArray ret, int nCount, 
		const std::vector<jdouble>& vj)
	{ env->SetDoubleArrayRegion(ret, 0, nCount, &vj[0]); }

	static base_type* GetJArrayElements(JNIEnv *env, array_type arr)
	{ return env->GetDoubleArrayElements(arr, 0); }
};

template <typename cchar_type = char>
struct JavaByteArrayTraits
{
	typedef jbyteArray array_type;
	typedef jbyte base_type;
	typedef cchar_type api_base_type;
	enum {dtwain_array_type = DTWAIN_ARRAYLONG};

	static array_type CreateJArray(JNIEnv* env, int nCount)
	{ return env->NewByteArray(nCount); }

	static void SetJArrayRegion(JNIEnv* env, array_type ret, int nCount, const std::vector<base_type>& vj)
	{ env->SetByteArrayRegion(ret, 0, nCount, &vj[0]); }

	static void SetJArrayRegion(JNIEnv* env, array_type ret, int nCount, base_type* vj)
	{ env->SetByteArrayRegion(ret, 0, nCount, vj); }

	static base_type* GetJArrayElements(JNIEnv *env, array_type arr)
	{ return env->GetByteArrayElements(arr, 0); }
};

template <typename JavaTraits>
DTWAIN_ARRAY CreateDTWAINArrayFromJArray(JNIEnv *env, typename JavaTraits::array_type arr)
{
	jsize nCount = env->GetArrayLength(arr);
	DTWAIN_ARRAY dArray = DTWAIN_ArrayCreate(JavaTraits::dtwain_array_type, (LONG)nCount);
	if (dArray)
	{
		JavaTraits::base_type* pElement = JavaTraits::GetJArrayElements(env, arr);
		JavaTraits::api_base_type *pDBuffer = 
			(JavaTraits::api_base_type *)DTWAIN_ArrayGetBuffer(dArray, 0);
		std::copy(pElement, pElement + nCount, pDBuffer);
		return dArray;
	}            
	return NULL;
}

template <typename JavaTraits>
typename JavaTraits::array_type CreateJArrayFromDTWAINArray(JNIEnv* env, DTWAIN_ARRAY theArray, 
															int numEntriesWhenNull = 0,
															bool bCreateAll=true)
{
	JavaTraits::array_type ret;
	if ( bCreateAll && theArray )
	{
		LONG nCount = DTWAIN_ArrayGetCount(theArray);
		std::vector<JavaTraits::base_type> vj(nCount);
		JavaTraits::api_base_type* pBuf = 
			(JavaTraits::api_base_type*)DTWAIN_ArrayGetBuffer(theArray, 0);
		std::copy(pBuf, pBuf + nCount, vj.begin());
		ret = (JavaTraits::array_type)JavaTraits::CreateJArray(env, nCount);
		JavaTraits::SetJArrayRegion(env, ret, nCount, vj);
	}
	else
		ret = (JavaTraits::array_type)JavaTraits::CreateJArray(env, numEntriesWhenNull);    
	return ret;
}

template <typename JavaTraits>
typename JavaTraits::array_type CreateJArrayFromCArray(JNIEnv* env, 
													   typename JavaTraits::api_base_type* theArray, 
													   unsigned long numElements,
													   int numEntriesWhenNull = 0,
													   bool bCreateAll=true)
{
	JavaTraits::array_type ret;
	if ( bCreateAll && theArray )
	{
		ret = (JavaTraits::array_type)JavaTraits::CreateJArray(env, numElements);
		JavaTraits::SetJArrayRegion(env, ret, numElements, (JavaTraits::base_type*)theArray);
	}
	else
		ret = (JavaTraits::array_type)JavaTraits::CreateJArray(env, numEntriesWhenNull);    
	return ret;
}

template <typename JavaTraits>
std::vector<typename JavaTraits::api_base_type> 
CreateCArrayFromJArray(JNIEnv *env, typename JavaTraits::array_type arr)
{
	jsize nCount = env->GetArrayLength(arr);
	std::vector<JavaTraits::api_base_type> dArray( nCount );
	JavaTraits::base_type* pElement = JavaTraits::GetJArrayElements(env, arr);
	if ( nCount > 0 )
	{
		JavaTraits::api_base_type *pDBuffer = (JavaTraits::api_base_type *)&dArray[0];
		std::copy(pElement, pElement + nCount, pDBuffer);
	}
	return dArray;
}            

jstring CreateJStringFromCString(JNIEnv *env, LPCTSTR str);
jclass GetJavaClassID(JNIEnv* env, const char* javaClass);
jmethodID GetJavaClassConstructor(JNIEnv *env, const char* javaClass, const char* constructorSig);
jobjectArray CreateJStringArrayFromDTWAIN(JNIEnv *env, DTWAIN_ARRAY arr);
DTWAIN_ARRAY CreateDTWAINArrayFromJStringArray(JNIEnv *env, jobjectArray strArray);
DTWAIN_ARRAY CreateDTWAINArrayFromJIntArray(JNIEnv *env, jintArray arr);
DTWAIN_ARRAY CreateDTWAINArrayFromJFrameArray(JNIEnv *env, jobjectArray frameArray);
jobjectArray CreateJFrameArrayFromDTWAINArray(JNIEnv *env, DTWAIN_ARRAY arr);
#endif
