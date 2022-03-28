#ifndef UTFCHARSHANDLER_H
#define UTFCHARSHANDLER_H

#include "jni.h"
#include <string>
#include "StringDefs.h"
#include <windows.h>
struct ANSICharTraits
{
	typedef char CharType;
	static void ReleaseJavaChars(JNIEnv* pEnv, jstring& jstr, const CharType* ptr) 
			{ pEnv->ReleaseStringUTFChars(jstr, ptr); }

	static const CharType *GetStringChars(JNIEnv* pEnv, jstring& jstr)
	{ return pEnv->GetStringUTFChars(jstr, 0); }

	static jstring GetNewJString(JNIEnv *pEnv, const CharType* str)
	{ return pEnv->NewStringUTF(str); }
};
#ifdef UNICODE
struct UnicodeCharTraits
{
	typedef jchar CharType; 
	static void ReleaseJavaChars(JNIEnv* pEnv, jstring& jstr, const CharType* ptr) 
			{ pEnv->ReleaseStringChars(jstr, ptr); }

	static const CharType *GetStringChars(JNIEnv* pEnv, jstring& jstr)
	{
		return pEnv->GetStringChars(jstr, 0);
	}

	static jstring GetNewJString(JNIEnv *pEnv, const CharType* str)
	{ 
		StringType strT(str);
		return pEnv->NewString(str, strT.size()); 
	}
};
#endif

template <typename CharTraits = ANSICharTraits>
struct GetStringCharsHandlerImpl
{
	typename const CharTraits::CharType* m_ptr;
	typedef typename CharTraits::CharType char_type;
	JNIEnv *m_pEnv;
	jstring m_jString;

	GetStringCharsHandlerImpl(JNIEnv *pEnv, jstring jstr) : 
	m_jString(jstr), m_pEnv(pEnv), m_ptr(0) { }

	GetStringCharsHandlerImpl() : m_pEnv(0), m_ptr(0) {}

	void setEnvironment(JNIEnv *pEnv)
	{
		m_pEnv = pEnv;
	}

	void SetJString(jstring jstr)
	{
		m_jString = jstr;
	}

	jstring GetNewJString(const typename CharTraits::CharType* s)
	{ return CharTraits::GetNewJString(m_pEnv, s); }

	~GetStringCharsHandlerImpl() 
	{
		if (m_ptr)
			CharTraits::ReleaseJavaChars(m_pEnv, m_jString, m_ptr);
	}

	typename const CharTraits::CharType *GetStringChars()
	{
		m_ptr = CharTraits::GetStringChars(m_pEnv, m_jString);
		return m_ptr;
	}

	typename LPCTSTR GetWindowsStringChars()
	{
		m_ptr = CharTraits::GetStringChars(m_pEnv, m_jString);
		return reinterpret_cast<LPCTSTR>(m_ptr);
	}

};

#ifdef UNICODE
	typedef GetStringCharsHandlerImpl<UnicodeCharTraits> GetStringCharsHandler;
#else
	typedef GetStringCharsHandlerImpl<> GetStringCharsHandler;
#endif
#endif