#ifndef STRINGDEFS_H
#define STRINGDEFS_H

#include <jni.h>
#include <string>

#ifdef UNICODE
	typedef std::basic_string<jchar> StringType;
	#define EmptyString_ L""
#else
	typedef std::string StringType;
	#define EmptyString_ ""
#endif
#endif
