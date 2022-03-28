#include "UTFCharsHandler.h"
#include "JavaArrayTraits.h"
#include "javaobjectcaller.h"
#include "DTWAINRAII.h"
#include <tchar.h>
jstring CreateJStringFromCString(JNIEnv *env, LPCTSTR str)
{
	jclass string_class = env->FindClass("java/lang/String");
	jmethodID stringConstructor = env->GetMethodID(string_class, "<init>", "(Ljava/lang/String;)V" );
	GetStringCharsHandler handler;
	handler.setEnvironment(env);
	return (jstring)env->NewObject(string_class, stringConstructor, 
					handler.GetNewJString(reinterpret_cast<const GetStringCharsHandler::char_type*>(str)));
}

jclass GetJavaClassID(JNIEnv* env, const char* javaClass)
{
	return env->FindClass( javaClass );
}

jmethodID GetJavaClassConstructor(JNIEnv *env, const char* javaClass, const char* constructorSig)
{
	jclass theClass = GetJavaClassID( env, javaClass );
	if ( theClass )
		return env->GetMethodID(theClass, "<init>", constructorSig);
	return NULL;               
}


jobjectArray CreateJStringArrayFromDTWAIN(JNIEnv *env, DTWAIN_ARRAY arr)
{
	LONG nCount = DTWAIN_ArrayGetCount(arr);
	jobjectArray ret;
	if ( nCount > 0 )
	{
		ret = (jobjectArray)env->NewObjectArray(nCount, env->FindClass("java/lang/String"),
			env->NewStringUTF(""));
		LONG maxChars = DTWAIN_ArrayGetMaxStringLength(arr);
		std::vector<char> Val(maxChars + 1, 0);                                                          
		for ( LONG i = 0; i < nCount; i++ )
		{
			DTWAIN_ArrayGetAt(arr, i, &Val[0]);
			env->SetObjectArrayElement(ret, i, env->NewStringUTF(&Val[0]));
		}
	}
	else
		ret = env->NewObjectArray(0, env->FindClass("java/lang/String"),
		env->NewStringUTF(""));
	return ret;
}

DTWAIN_ARRAY CreateDTWAINArrayFromJStringArray(JNIEnv *env, jobjectArray strArray)
{
	jsize stringCount = env->GetArrayLength(strArray);
	DTWAIN_ARRAY arr = DTWAIN_ArrayCreate(DTWAIN_ARRAYSTRING, 0);
	if ( arr )
	{
		for (int i = 0; i < stringCount; ++i)
			DTWAIN_ArrayAddString(arr, 
								  (LPCTSTR)GetStringCharsHandler(env, 
								  (jstring)env->GetObjectArrayElement(strArray, i)).GetStringChars());
	}
	return arr;
}

DTWAIN_ARRAY CreateDTWAINArrayFromJIntArray(JNIEnv *env, jintArray arr)
{

	jsize nCount = env->GetArrayLength(arr);
	DTWAIN_ARRAY dArray = DTWAIN_ArrayCreate(DTWAIN_ARRAYLONG, (LONG)nCount);
	if (dArray)
	{
		jint* pElement = env->GetIntArrayElements(arr, 0);
		LONG *pDBuffer = (LONG *)DTWAIN_ArrayGetBuffer(dArray, 0);
		std::copy(pElement, pElement + nCount, pDBuffer);

		// test loop
		LONG tValue;
		for (LONG i = 0; i < nCount; ++i)
			DTWAIN_ArrayGetAtLong(dArray,i,&tValue);
		return dArray;
	}            
	return NULL;
}

DTWAIN_ARRAY CreateDTWAINArrayFromJFrameArray(JNIEnv *env, jobjectArray frameArray)
{
	jsize frameCount = env->GetArrayLength(frameArray);
	DTWAIN_ARRAY arr = DTWAIN_ArrayCreate(DTWAIN_ARRAYFRAME, 0);
	if ( arr )
	{
		JavaFrameInfo jInfo(env);
		double left, top, right, bottom;
		for (int i = 0; i < frameCount; ++i)
		{
			jobject jFrame = env->GetObjectArrayElement(frameArray, i);
			jInfo.setObject(jFrame);
			jInfo.getJFrameDimensions(&left, &top, &right, &bottom);
			DTWAIN_FRAME f = DTWAIN_FrameCreate(left, top, right, bottom);
			DTWAINFrame_RAII raii(f);
			DTWAIN_ArrayAdd(arr, f);
		}
	}
	return arr;
}                    

jobjectArray CreateJFrameArrayFromDTWAINArray(JNIEnv *env, DTWAIN_ARRAY arr)
{
	jsize frameCount = 0;
	JavaFrameInfo jInfo(env);
	if ( arr )
		frameCount = DTWAIN_ArrayGetCount(arr);
	jobjectArray jFrameArray = jInfo.CreateJFrameObjectArray(frameCount);
	double left, top, right, bottom;
	for (int i = 0; i < frameCount; ++i)        
	{
		DTWAIN_ArrayFrameGetAt(arr, i, &left, &top, &right, &bottom);
		jobject jObj = env->GetObjectArrayElement(jFrameArray, i);
		jInfo.setObject(jObj);
		jInfo.setJFrameDimensions(left, top, right, bottom);
	}
	return jFrameArray;
}                    
