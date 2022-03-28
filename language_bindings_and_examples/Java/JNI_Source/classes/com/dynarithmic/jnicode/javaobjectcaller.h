#ifndef JAVAOBJECTCALLER_H
#define JAVAOBJECTCALLER_H

#include <jni.h>
#include <vector>
#include <string>
#include <map>
#include "StringDefs.h"

class JavaObjectCaller
{
private:
	jclass m_jClass;
	jobject m_jObject;

	struct JObjectCallerDefs
	{
		std::string m_jMethodName;
		std::string m_jMethodSig;
		jmethodID m_jMethodID;
		JObjectCallerDefs(const std::string& methodName = "", const std::string& methodSig="") : 
		m_jMethodID(0), m_jMethodName(methodName), m_jMethodSig(methodSig) {}
	};

	std::vector<JObjectCallerDefs> m_jGetMethods;
	std::vector<JObjectCallerDefs> m_jSetMethods;
	std::vector<JObjectCallerDefs> m_jConstructorMethods;

	std::string m_jClassName;
	size_t m_nDefaultConstructorPos;

protected:
	JNIEnv *m_pJavaEnv;
	typedef std::map<std::string, JObjectCallerDefs> NameToFuncDefsMap;
	typedef std::vector<JObjectCallerDefs> ConstructorList;
	NameToFuncDefsMap m_nNameToFuncDefs;
	ConstructorList m_nConstructorList;

public:
	jclass getClass() { return m_jClass; }
	void setObject(jobject jObj) { m_jObject = jObj; }
	virtual ~JavaObjectCaller() {}
	JavaObjectCaller(JNIEnv *pEnv, const std::string& jClass = "") : 
					m_pJavaEnv(pEnv), m_jClassName(jClass), m_jClass(0), m_nDefaultConstructorPos(-1) {}
	void setJavaClass(const std::string& s) { m_jClassName = s; }

	std::string getJavaClass() const { return m_jClassName; }

	void registerMethod(const std::string& methodName, const std::string& methodSig)
	{
		JObjectCallerDefs def(methodName, methodSig);
		if (methodName.empty())
			m_nConstructorList.push_back(def);
		else
			m_nNameToFuncDefs.insert(make_pair(methodName, def));
	}

	JObjectCallerDefs getFunctionDef(const std::string& methodName)
	{
		NameToFuncDefsMap::iterator it = m_nNameToFuncDefs.find(methodName);
		if ( it != m_nNameToFuncDefs.end())
			return it->second;
		return JObjectCallerDefs();
	}

	void initializeMethods()
	{			
		if ( !m_pJavaEnv )
			return;
		if ( m_jClassName.empty())
			return;
		m_jClass = m_pJavaEnv->FindClass(m_jClassName.c_str());
		if ( !m_jClass )
			return;
		NameToFuncDefsMap::iterator it = m_nNameToFuncDefs.begin();
		while (it != m_nNameToFuncDefs.end())
		{
			NameToFuncDefsMap::value_type& pr = *it;
			JObjectCallerDefs& defs = pr.second;
			defs.m_jMethodID = m_pJavaEnv->GetMethodID(m_jClass, defs.m_jMethodName.c_str(), defs.m_jMethodSig.c_str());
			++it;
		}
		// now do the constructors
		ConstructorList::iterator it2 = m_nConstructorList.begin();
		while (it2 != m_nConstructorList.end())
		{
			ConstructorList::value_type& defs = *it2;
			defs.m_jMethodID = GetJavaClassConstructor(m_pJavaEnv, m_jClassName.c_str(), defs.m_jMethodSig.c_str());
			++it2;
		}	
	}				

	void setDefaultConstructorPos(size_t num) { m_nDefaultConstructorPos = num; }
	size_t getDefaultConstructorPos() const { return m_nDefaultConstructorPos; }
	jobject defaultConstructObject() { return m_pJavaEnv->NewObject(m_jClass, m_nConstructorList[m_nDefaultConstructorPos].m_jMethodID); }

	template <typename T1>
	jobject constructObject(size_t constructorNumber, T1 t) 
	{ return m_pJavaEnv->NewObject(m_jClass, m_nConstructorList[constructorNumber].m_jMethodID, t); }

	template <typename T1, typename T2>
	jobject constructObject(size_t constructorNumber, T1 t, T2 t2) 
	{ return m_pJavaEnv->NewObject(m_jClass, m_nConstructorList[constructorNumber].m_jMethodID, t, t2); }

	template <typename T1, typename T2, typename T3>
	jobject constructObject(size_t constructorNumber, T1 t, T2 t2, T3 t3) 
	{ return m_pJavaEnv->NewObject(m_jClass, m_nConstructorList[constructorNumber].m_jMethodID, t, t2, t3); }

	template <typename T1, typename T2, typename T3, typename T4>
	jobject constructObject(size_t constructorNumber, T1 t, T2 t2, T3 t3, T4 t4) 
	{ return m_pJavaEnv->NewObject(m_jClass, m_nConstructorList[constructorNumber].m_jMethodID, t, t2, t3, t4); }

	template <typename T1, typename T2, typename T3, typename T4, typename T5>
	jobject constructObject(size_t constructorNumber, T1 t, T2 t2, T3 t3, T4 t4, T5 t5) 
	{ return m_pJavaEnv->NewObject(m_jClass, m_nConstructorList[constructorNumber].m_jMethodID, t, t2, t3, t4, t5); }

	template <typename T1, typename T2, typename T3, typename T4, typename T5, typename T6>
	jobject constructObject(size_t constructorNumber, T1 t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) 
	{ return m_pJavaEnv->NewObject(m_jClass, m_nConstructorList[constructorNumber].m_jMethodID, t, t2, t3, t4, t5, t6); }

	void callVoidMethod(const std::string& methodName) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			m_pJavaEnv->CallVoidMethod(m_jObject, defs.m_jMethodID);
	}		

	template <typename T1>
	void callVoidMethod(const std::string& methodName, T1 t) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			m_pJavaEnv->CallVoidMethod(m_jObject, defs.m_jMethodID, t);
	}		

	template <typename T1, typename T2>
	void callVoidMethod(const std::string& methodName, T1 t, T2 t2) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			m_pJavaEnv->CallVoidMethod(m_jObject, defs.m_jMethodID, t, t2);
	}		

	template <typename T1, typename T2, typename T3>
	void callVoidMethod(const std::string& methodName, T1 t, T2 t2, T3 t3) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			m_pJavaEnv->CallVoidMethod(m_jObject, defs.m_jMethodID, t, t2, t3);
	}		

	template <typename T1, typename T2, typename T3, typename T4>
	void callVoidMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			m_pJavaEnv->CallVoidMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4);
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5>
	void callVoidMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			m_pJavaEnv->CallVoidMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5);
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5, typename T6>
	void callVoidMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			m_pJavaEnv->CallVoidMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5, t6);
	}		

	jobject callObjectMethod(const std::string& methodName) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallObjectMethod(m_jObject, defs.m_jMethodID);
		return 0;				
	}		

	template <typename T1>
	jobject callObjectMethod(const std::string& methodName, T1 t) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallObjectMethod(m_jObject, defs.m_jMethodID, t);
		return 0;		
	}		

	template <typename T1, typename T2>
	jobject callObjectMethod(const std::string& methodName, T1 t, T2 t2) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallObjectMethod(m_jObject, defs.m_jMethodID, t, t2);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3>
	jobject callObjectMethod(const std::string& methodName, T1 t, T2 t2, T3 t3) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallObjectMethod(m_jObject, defs.m_jMethodID, t, t2, t3);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4>
	jobject callObjectMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallObjectMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5>
	jobject callObjectMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallObjectMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5, typename T6>
	jobject callObjectMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallObjectMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5, t6);
		return 0;		
	}		

	int callIntMethod(const std::string& methodName) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallIntMethod(m_jObject, defs.m_jMethodID);
		return 0;				
	}		

	template <typename T1>
	int callIntMethod(const std::string& methodName, T1 t) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallIntMethod(m_jObject, defs.m_jMethodID, t);
		return 0;		
	}		

	template <typename T1, typename T2>
	int callIntMethod(const std::string& methodName, T1 t, T2 t2) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallIntMethod(m_jObject, defs.m_jMethodID, t, t2);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3>
	int callIntMethod(const std::string& methodName, T1 t, T2 t2, T3 t3) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallIntMethod(m_jObject, defs.m_jMethodID, t, t2, t3);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4>
	int callIntMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallIntMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5>
	int callIntMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallIntMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5, typename T6>
	int callIntMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallIntMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5, t6);
		return 0;		
	}		

	int64_t callLongMethod(const std::string& methodName) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallLongMethod(m_jObject, defs.m_jMethodID);
		return 0;				
	}		

	template <typename T1>
	int64_t callLongMethod(const std::string& methodName, T1 t) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallLongMethod(m_jObject, defs.m_jMethodID, t);
		return 0;		
	}		

	template <typename T1, typename T2>
    int64_t callLongMethod(const std::string& methodName, T1 t, T2 t2)
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallLongMethod(m_jObject, defs.m_jMethodID, t, t2);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3>
    int64_t callLongMethod(const std::string& methodName, T1 t, T2 t2, T3 t3)
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallLongMethod(m_jObject, defs.m_jMethodID, t, t2, t3);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4>
    int64_t callLongMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4)
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallLongMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5>
    int64_t callLongMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5)
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallLongMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5, typename T6>
    int64_t callLongMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallLongMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5, t6);
		return 0;		
	}		

	double callDoubleMethod(const std::string& methodName) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallDoubleMethod(m_jObject, defs.m_jMethodID);
		return 0;				
	}		

	template <typename T1>
	double callDoubleMethod(const std::string& methodName, T1 t) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallDoubleMethod(m_jObject, defs.m_jMethodID, t);
		return 0;		
	}		

	template <typename T1, typename T2>
	double callDoubleMethod(const std::string& methodName, T1 t, T2 t2) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallDoubleMethod(m_jObject, defs.m_jMethodID, t, t2);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3>
	double callDoubleMethod(const std::string& methodName, T1 t, T2 t2, T3 t3) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallDoubleMethod(m_jObject, defs.m_jMethodID, t, t2, t3);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4>
	double callDoubleMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallDoubleMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4);
		return 0;		
	}		
	template <typename T1, typename T2, typename T3, typename T4, typename T5>
	double callDoubleMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallDoubleMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5);
		return 0;		
	}		

	template <typename T1, typename T2, typename T3, typename T4, typename T5, typename T6>
	double callDoubleMethod(const std::string& methodName, T1 t, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) 
	{ 
		JObjectCallerDefs defs = getFunctionDef(methodName);
		if ( !defs.m_jMethodName.empty() )
			return m_pJavaEnv->CallDoubleMethod(m_jObject, defs.m_jMethodID, t, t2, t3, t4, t5, t6);
		return 0;		
	}		

};

template <typename CharTraits = ANSICharTraits>
class JavaTwainAppInfoImpl : public JavaObjectCaller
{
public:
	JavaTwainAppInfoImpl(JNIEnv* pEnv) : JavaObjectCaller(pEnv, "com/dynarithmic/twain/DTwainAppInfo")
	{
		registerMethod("getVersionInfo", "()Ljava/lang/String;");
		registerMethod("getManufacturer", "()Ljava/lang/String;");
		registerMethod("getProductFamily", "()Ljava/lang/String;");
		registerMethod("getProductName", "()Ljava/lang/String;");
		registerMethod("setVersionInfo", "(Ljava/lang/String;)V");
		registerMethod("setManufacturer", "(Ljava/lang/String;)V");
		registerMethod("setProductFamily", "(Ljava/lang/String;)V");
		registerMethod("setProductName", "(Ljava/lang/String;)V");
		registerMethod("", "()V");
		registerMethod("", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V");
		initializeMethods();
	}

	jobject createDTwainAppInfo()
	{
		if ( m_pJavaEnv )
			return defaultConstructObject();
        return nullptr;
	}

	jobject createDTwainAppInfo(LPCTSTR str1, LPCTSTR str2, LPCTSTR str3, LPCTSTR str4)
	{
		return constructObject(1, 
			CreateJStringFromCString(m_pJavaEnv, str1),
			CreateJStringFromCString(m_pJavaEnv, str2),
			CreateJStringFromCString(m_pJavaEnv, str3),
			CreateJStringFromCString(m_pJavaEnv, str4));
	}

	void setVersionInfo(const StringType& str)
	{ callVoidMethod("setVersionInfo", CreateJStringFromCString(m_pJavaEnv,str.c_str())); }

	void setManufacturer(const StringType& str)
	{ callVoidMethod("setManufacturer", CreateJStringFromCString(m_pJavaEnv,str.c_str())); }

	void setProductFamily(const StringType& str)
	{ callVoidMethod("setProductFamily", CreateJStringFromCString(m_pJavaEnv,str.c_str())); }

	void setProductName(const StringType& str)
	{ callVoidMethod("setProductName", CreateJStringFromCString(m_pJavaEnv,str.c_str())); }

	StringType getVersionInfo()
	{ return GetStringCharsHandler(m_pJavaEnv, (jstring)callObjectMethod("getVersionInfo")).GetStringChars(); }

	StringType getManufacturer()
	{ return GetStringCharsHandler(m_pJavaEnv, (jstring)callObjectMethod("getManufacturer")).GetStringChars(); }

	StringType getProductFamily()
	{ return GetStringCharsHandler(m_pJavaEnv, (jstring)callObjectMethod("getProductFamily")).GetStringChars(); }

	StringType getProductName()
	{ return GetStringCharsHandler(m_pJavaEnv, (jstring)callObjectMethod("getProductName")).GetStringChars(); }
};
#ifdef UNICODE
	typedef JavaTwainAppInfoImpl<UnicodeCharTraits> JavaTwainAppInfo;
#else
	typedef JavaTwainAppInfoImpl<> JavaTwainAppInfo;
#endif


class JavaDTwainBufferedStripInfo : public JavaObjectCaller
{
public:
	JavaDTwainBufferedStripInfo(JNIEnv* env) : JavaObjectCaller(env, "com/dynarithmic/twain/DTwainBufferedStripInfo")
	{
		registerMethod("getBufferHandle", "()J");
		registerMethod("getBufferedStripData", "()[B");
		registerMethod("getCompressionType", "()I");
		registerMethod("getBufferSize", "()I");
		registerMethod("getImageInfo", "()Lcom/dynarithmic/twain/DTwainImageInfo;");
		registerMethod("getColumnsInBuffer", "()I");
		registerMethod("getRowsInBuffer", "()I");
		registerMethod("getxOffsetInImage", "()I");
		registerMethod("getyOffsetInImage", "()I");
		registerMethod("getBytesWritten", "()I");
		registerMethod("getBytesPerRow", "()I");
		registerMethod("getPreferredSize", "()I");
		registerMethod("setBufferHandle", "(J)V");
		registerMethod("setBufferedStripData", "([B)V");
		registerMethod("setCompressionType", "(I)V");
		registerMethod("setBufferSize", "(I)V");
		registerMethod("setImageInfo", "(Lcom/dynarithmic/twain/DTwainImageInfo;)V");
		registerMethod("setColumnsInBuffer", "(I)V");
		registerMethod("setRowsInBuffer", "(I)V");
		registerMethod("setxOffsetInImage", "(I)V");
		registerMethod("setyOffsetInImage", "(I)V");
		registerMethod("setBytesWritten", "(I)V");
		registerMethod("setBytesPerRow", "(I)V");
		// register the constructors
		registerMethod("", "()V");
		registerMethod("", "(III)V");
		setDefaultConstructorPos(0);
		initializeMethods();
	}

	void setBufferHandle(HANDLE handle)
	{ callVoidMethod("setBufferHandle", (jlong)handle); }

	HANDLE getBufferHandle()
	{ return (HANDLE)callLongMethod("getBufferHandle"); }

	int getBufferSize()
	{ return callIntMethod("getBufferSize"); }

	int getPreferredSize()
	{ return callIntMethod("getPreferredSize"); }

	void setBufferedStripData(LPBYTE pBytes, LONG size)
	{  callVoidMethod("setBufferedStripData", CreateJArrayFromCArray<JavaByteArrayTraits<char> >(m_pJavaEnv, (char *)pBytes, size)); }

	void setImageInfo(jobject jImageInfo)
	{ callVoidMethod("setImageInfo", jImageInfo); }

	void setBufferStripInfo(LONG columns, LONG rows, LONG xOffset, LONG yOffset, LONG bytesWritten, LONG BytesPerRow)
	{
		LONG vals [] = {columns, rows, xOffset, yOffset, bytesWritten, BytesPerRow};
		const char* fnNames[] = {"setColumnsInBuffer", "setRowsInBuffer", "setxOffsetInImage", "setyOffsetInImage", "setBytesWritten", "setBytesPerRow"};
		int j = 0;
		for ( int i = 0; i < sizeof(fnNames) / sizeof(fnNames[0]); ++i )
			callVoidMethod(fnNames[i], vals[i]);
	}

	void setBufferSize(LONG size)
	{ callVoidMethod("setBufferSize", size); }

	jobject createFullObject(LONG prefSize, LONG minimumSiz, LONG maximumSiz)
	{
		return constructObject(1, prefSize, minimumSiz, maximumSiz);
	}
};

class JavaFrameInfo : public JavaObjectCaller
{
public:
	JavaFrameInfo(JNIEnv *pEnv) : JavaObjectCaller(pEnv, "com/dynarithmic/twain/DTwainFrame")
	{
		registerMethod("getLeft", "()D");
		registerMethod("getTop", "()D");
		registerMethod("getRight", "()D");
		registerMethod("getBottom","()D");
		registerMethod("setLeft",  "(D)V");
		registerMethod("setTop",  "(D)V");
		registerMethod("setRight",  "(D)V");
		registerMethod("setBottom", "(D)V");
		registerMethod("", "()V");
		setDefaultConstructorPos(0);
		initializeMethods();
	}

	jobject CreateJFrameObject()
	{ return defaultConstructObject(); }

	jobjectArray CreateJFrameObjectArray(jsize numElements) 
	{ return m_pJavaEnv->NewObjectArray(numElements, getClass(), defaultConstructObject()); }

	void setJFrameDimensions(double left, double top, double right, double bottom)
	{
		const char* fnName[] = {"setLeft", "setTop", "setRight", "setBottom"};
		double vals[] = {left, top, right, bottom};
		for (int i = 0; i < 4; ++i)
			callVoidMethod(fnName[i], vals[i]);
	}

	void getJFrameDimensions(double* left, double* top, double* right, double* bottom)
	{
		const char* fnName[] = {"getLeft", "getTop", "getRight", "getBottom"};
		double* vals[] = {left, top, right, bottom};
		for (int i = 0; i < 4; ++i)
			*(vals[i]) = callDoubleMethod(fnName[i]);
	}
};

class JavaDTwainVersionInfo : public JavaObjectCaller
{
public:
	JavaDTwainVersionInfo(JNIEnv* env) : JavaObjectCaller(env, "com/dynarithmic/twain/DTwainVersionInfo") 
	{
		setDefaultConstructorPos(0);
		registerMethod("", "()V");
		registerMethod("", "(IIIILjava/lang/String;)V");
		initializeMethods();
	}

	jobject createDefaultObject() {return defaultConstructObject(); }
	jobject createFullObject(LONG var1, LONG var2, LONG var3, LONG var4, jstring str) 
	{ return constructObject(1, var1, var2, var3, var4, str); }
};		


class JavaDTwainAcquireArea : public JavaObjectCaller
{
public:
	JavaDTwainAcquireArea(JNIEnv* env) : JavaObjectCaller(env, "com/dynarithmic/twain/DTwainAcquireArea") 
	{
		setDefaultConstructorPos(0);
		registerMethod("", "()V");
		registerMethod("", "(DDDDI)V");
		registerMethod("setAll", "(DDDD)V");
		registerMethod("getLeft", "()D");
		registerMethod("getTop", "()D");
		registerMethod("getRight", "()D");
		registerMethod("getBottom", "()D");
		registerMethod("getUnitOfMeasure", "()I");
		initializeMethods();
	}

	jobject createDefaultObject() {return defaultConstructObject(); }
	jobject createFullObject(double var1, double var2, double var3, double var4, int var5) 
	{ return constructObject(1, var1, var2, var3, var4, var5); }
	double getLeft() { return callDoubleMethod("getLeft"); }
	double getTop() { return callDoubleMethod("getTop"); }
	double getRight() { return callDoubleMethod("getRight"); }
	double getBottom() { return callDoubleMethod("getBottom"); }
	int    getUnitOfMeasure() { return callIntMethod("getUnitOfMeasure");}
};		

class JavaDTwainImageInfo : public JavaObjectCaller
{
	public:
		JavaDTwainImageInfo(JNIEnv* env) : JavaObjectCaller(env, "com/dynarithmic/twain/DTwainImageInfo")
		{
			setDefaultConstructorPos(0);
			registerMethod("", "()V");
			registerMethod("setXResolution", "(D)V");
			registerMethod("setYResolution", "(D)V");
			registerMethod("setImageWidth", "(I)V");
			registerMethod("setImageLength", "(I)V");            
			registerMethod("setSamplesPerPixel", "(I)V");            
			registerMethod("setBitsPerPixel", "(I)V");            
			registerMethod("setPlanar", "(I)V");            
			registerMethod("setPixelType", "(I)V");            
			registerMethod("setCompression", "(I)V");
			registerMethod("setBitsPerSample", "([I)V");
			initializeMethods();
		}
};		

class JavaDTwainSourceInfo : public JavaObjectCaller
{
	public:
		JavaDTwainSourceInfo(JNIEnv* env) : JavaObjectCaller(env, "Lcom/dynarithmic/twain/DTwainSourceInfo;")
		{
			setDefaultConstructorPos(0);
			registerMethod("", "()V");
			registerMethod("", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;II)V");
			initializeMethods();
		}

		jobject createFullObject(LPCTSTR arg1, LPCTSTR arg2, LPCTSTR arg3, LPCTSTR arg4,
			                     int arg5, int arg6)
		{
			return constructObject(1, 
				CreateJStringFromCString(m_pJavaEnv, arg1),
				CreateJStringFromCString(m_pJavaEnv, arg2),
				CreateJStringFromCString(m_pJavaEnv, arg3),
				CreateJStringFromCString(m_pJavaEnv, arg4),
				arg5, arg6);
		}
};		

class JavaDTwainOCRInfo : public JavaObjectCaller
{
	public:
		JavaDTwainOCRInfo(JNIEnv* env) : JavaObjectCaller(env, "Lcom/dynarithmic/twain/DTwainOCRInfo;")
		{
			setDefaultConstructorPos(0);
			registerMethod("", "()V");
			registerMethod("", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V");
			initializeMethods();
		}

		jobject createFullObject(LPCTSTR arg1, LPCTSTR arg2, LPCTSTR arg3, LPCTSTR arg4)
		{
			return constructObject(1, 
				CreateJStringFromCString(m_pJavaEnv, arg1),
				CreateJStringFromCString(m_pJavaEnv, arg2),
				CreateJStringFromCString(m_pJavaEnv, arg3),
				CreateJStringFromCString(m_pJavaEnv, arg4));
		}
};	

class JavaDTwainAcquisitionArray : public JavaObjectCaller
{
public:
	JavaDTwainAcquisitionArray(JNIEnv* env) : JavaObjectCaller(env, "com/dynarithmic/twain/DTwainAcquisitionArray")
	{
		setDefaultConstructorPos(0);
		registerMethod("", "()V");
		registerMethod("addAcquisitionData", "(Lcom/dynarithmic/twain/DTwainAcquisitionData;)V");
		registerMethod("setStatus", "(I)V" );
		initializeMethods();
	}
};

class JavaDTwainAcquisitionData : public JavaObjectCaller
{
public:
	JavaDTwainAcquisitionData(JNIEnv* env) : JavaObjectCaller(env, "com/dynarithmic/twain/DTwainAcquisitionData")
	{
		setDefaultConstructorPos(0);
		registerMethod("", "()V");
		registerMethod("addImageData", "(Lcom/dynarithmic/twain/DTwainImageData;)V");
		initializeMethods();
	}
};

class JavaDTwainImageData : public JavaObjectCaller
{
public:
	JavaDTwainImageData(JNIEnv* env) : JavaObjectCaller(env, "com/dynarithmic/twain/DTwainImageData")
	{
		setDefaultConstructorPos(0);
		registerMethod("", "()V");
		registerMethod("setImageData", "([B)V");
		registerMethod("setDibHandle", "(J)V");
		initializeMethods();
	}
};

class JavaAcquirerInfo
{
	private:
		JavaDTwainAcquisitionArray m_jAcquisitionArray;
		JavaDTwainAcquisitionData m_jAcquisitionData;
		JavaDTwainImageData m_jImageData;

	public:
		JavaAcquirerInfo(JNIEnv *pEnv) : m_jAcquisitionArray(pEnv), m_jAcquisitionData(pEnv), m_jImageData(pEnv)
		{}

		jobject CreateJavaImageDataObject(JNIEnv *env)
		{ return m_jImageData.defaultConstructObject(); }

		jobject CreateJavaAcquisitionDataObject(JNIEnv *env)
		{ return m_jAcquisitionData.defaultConstructObject(); }

		jobject CreateJavaAcquisitionArrayObject(JNIEnv *env)
		{ return m_jAcquisitionArray.defaultConstructObject(); }

		void addAcquisitionToArray(jobject jAcquisitionArrayObject, jobject jAcquisitionDataObject)
		{
			m_jAcquisitionArray.setObject(jAcquisitionArrayObject);
			m_jAcquisitionArray.callVoidMethod("addAcquisitionData", jAcquisitionDataObject);
		}


		void addImageDataToAcquisition(jobject jAcquisitionDataObject, jobject jImageDataObject)
		{
			m_jAcquisitionData.setObject(jAcquisitionDataObject);
			m_jAcquisitionData.callVoidMethod("addImageData", jImageDataObject);
		}

		void setImageData(JNIEnv* env, jobject jImageDataObject, LPVOID imgData, unsigned long nDataSize, HANDLE handle =  (HANDLE)0)
		{
			// Create a jarray of bytes
			jbyteArray bArray = CreateJArrayFromCArray<JavaByteArrayTraits<char> >(env, (char *)imgData, nDataSize);

			// Call the method
			m_jImageData.setObject(jImageDataObject);
			m_jImageData.callVoidMethod("setImageData", bArray);
			m_jImageData.callVoidMethod("setDibHandle", handle);
			// release the array for internal Java GC
			env->DeleteLocalRef(bArray);
		}

		void setStatus(jobject jAcquisitionArray, LONG status)
		{
			m_jAcquisitionArray.setObject(jAcquisitionArray);
			m_jAcquisitionArray.callVoidMethod("setStatus", status);
		}
};

#endif