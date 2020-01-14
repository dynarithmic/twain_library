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
#ifndef CTLObStr_h_
#define CTLObStr_h_

#include <string>
#include <vector>
#include <sstream>
#include <cctype>
#include <boost/tokenizer.hpp>
#include <boost/algorithm/string/join.hpp>
#include <boost/algorithm/string.hpp>
#include <boost/filesystem.hpp>
#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp>
#include <boost/uuid/uuid_io.hpp>
#include <assert.h>
#include <stdarg.h>
#include <algorithm>
#include <stdlib.h>
#include <cstring>
#include <locale>
#include "dtwain_standard_defs.h"
#ifndef _MAX_PATH
#define _MAX_PATH 260
#endif

namespace dynarithmic
{
    typedef std::string CTL_String;
    typedef std::wstring CTL_WString;
    typedef std::vector<CTL_String> CTL_StringArray;
    typedef std::vector<CTL_WString> CTL_WStringArray;
    typedef std::ostringstream CTL_StringStreamA;
    typedef std::wostringstream CTL_StringStreamW;
    typedef std::istringstream CTL_StringStreamInA;
    typedef std::wistringstream CTL_StringStreamInW;
    typedef std::wstring CTL_WString;
    typedef std::wofstream  CTL_OutputFileStreamTypeW;
    typedef std::wostream   CTL_OutputBaseStreamTypeW;
    typedef std::ofstream  CTL_OutputFileStreamTypeA;
    typedef std::ostream   CTL_OutputBaseStreamTypeA;

    #ifdef UNICODE
        typedef CTL_WString CTL_StringType;
        typedef CTL_WStringArray CTL_StringArrayType;
        typedef std::wostringstream CTL_StringStreamType;
        typedef std::wistringstream CTL_StringStreamInType;
        typedef std::wifstream      CTL_InputFileStreamType;
        typedef std::wistream       CTL_InputBaseStreamType;
        typedef std::wofstream      CTL_OutputFileStreamType;
        typedef std::wostream       CTL_OutputBaseStreamType;
        #define BOOST_FORMAT boost::wformat
        #define BOOST_PATHTYPE boost::filesystem::wpath
        #define BOOST_GENERIC_STRING(x) (x).generic_wstring()
        #define BOOST_UUID_STRING_CONVERT(x)  boost::uuids::to_wstring((x))
    #else
        typedef CTL_String CTL_StringType;
        typedef CTL_StringArray CTL_StringArrayType;
        typedef std::ostringstream CTL_StringStreamType;
        typedef std::istringstream CTL_StringStreamInType;
        typedef std::ifstream      CTL_InputFileStreamType;
        typedef std::istream       CTL_InputBaseStreamType;
        typedef std::ofstream      CTL_OutputFileStreamType;
        typedef std::ostream       CTL_OutputBaseStreamType;
        #define BOOST_FORMAT boost::format
        #define BOOST_PATHTYPE boost::filesystem::path
        #define BOOST_GENERIC_STRING(x) (x).generic_string()
        #define BOOST_UUID_STRING_CONVERT(x)  boost::uuids::to_string((x))
    #endif

    #define LOCAL_STATIC static
    #define STRINGWRAPPER_QUALIFIER StringWrapper::
    #define STRINGWRAPPER_PREFIX StringWrapper::

    struct ANSIStringTraits
    {
        typedef char char_type;
        static char_type GetSpace() { return ' ';}
        static const char_type* GetEmptyString() { return ""; }
        static size_t StringLength(const char_type* s) { return std::char_traits<char_type>::length(s); }
        static char_type* StringCopy(char_type* dest, const char_type* src) { return std::char_traits<char_type>::copy(dest, src, StringLength(src)); }
        static char_type* StringCopyN(char_type* dest, const char_type* src, size_t count) { return std::char_traits<char_type>::copy(dest, src, count); }
        static int StringCompare(const char_type* dest, const char_type* src, size_t count) { return std::char_traits<char_type>::compare(dest, src, count); }
        static int StringCompare(const char_type* dest, const char_type* src) { return std::char_traits<char_type>::compare(dest, src, (std::min)(StringLength(dest), StringLength(src))); }
        static int ToUpper(char_type ch) { return toupper((int)ch); }
        static int ToLower(char_type ch) { return tolower((int)ch); }
        static bool IsDigit(int ch) { return ::isdigit(ch)?true:false; }
        static double ToDouble(const char_type* s1)
        { return s1?strtod(s1, NULL):0.0; }
        #ifdef _WIN32
        static UINT GetWindowsDirectoryImpl(char_type* buffer)
                    { return ::GetWindowsDirectoryA(buffer, _MAX_PATH); }
        static UINT GetSystemDirectoryImpl(char_type* buffer)
                    { return ::GetSystemDirectoryA(buffer, _MAX_PATH); }
        static char_type* AddBackslashImpl(char_type* buffer)
                    { return ::PathAddBackslashA(buffer); }
        static DWORD GetModuleFileNameImpl(HMODULE hModule, char_type* lpFileName, DWORD nSize)
                    { return ::GetModuleFileNameA(hModule, lpFileName, nSize); }
        #else
        static UINT GetWindowsDirectoryImpl(char_type* buffer)
        { getcwd(buffer, 8096); return 1; }
        static UINT GetSystemDirectoryImpl(char_type* buffer)
        { getcwd(buffer, 8096); return 1; }
        static LPSTR AddBackslashImpl(char_type* buffer)
        {
            auto ps = boost::filesystem::path::preferred_separator;
            auto len = StringLength(buffer);
            if (buffer[len-1] != ps)
            {
                buffer[len] = ps;
                buffer[len+1] = 0;
            }
            return buffer;
        }
        static DWORD GetModuleFileNameImpl(HMODULE hModule, char_type* lpFileName, DWORD nSize)
        {
            return 0; // ::GetModuleFileNameA(hModule, lpFileName, nSize);
        }
        #endif
    };

    struct UnicodeStringTraits
    {
        typedef wchar_t char_type;
        static char_type GetSpace() { return L' ';}
        static const char_type* GetEmptyString() { return L""; }
        static const char_type* GetCompatStringLiteral(const char_type* x) { return x; }
        static size_t StringLength(const char_type* s) { return std::char_traits<char_type>::length(s); }
        static int StringCompare(const char_type* dest, const char_type* src, size_t count) { return std::char_traits<char_type>::compare(dest, src, count); }
        static int StringCompare(const char_type* dest, const char_type* src) { return std::char_traits<char_type>::compare(dest, src, (std::min)(StringLength(dest), StringLength(src))); }
        static char_type* StringCopy(char_type* dest, const char_type* src) { return std::char_traits<char_type>::copy(dest, src, StringLength(src)); }
        static char_type* StringCopyN(char_type* dest, const char_type* src, size_t count) { return std::char_traits<char_type>::copy(dest, src, count); }
        static wint_t ToUpper(char_type ch) { return towupper((wint_t)ch); }
        static wint_t ToLower(char_type ch) { return towlower((wint_t)ch); }
        static bool IsDigit(wint_t ch) { return ::iswdigit(ch)?true:false; }
        static double ToDouble(const char_type* s1)
        { return s1 ? wcstod(s1, NULL) : 0.0; }
        #ifdef _WIN32
        static UINT GetWindowsDirectoryImpl(char_type* buffer)
        { return ::GetWindowsDirectoryW(buffer, _MAX_PATH); }
        static UINT GetSystemDirectoryImpl(char_type* buffer)
        { return ::GetSystemDirectoryW(buffer, _MAX_PATH); }
        static LPWSTR AddBackslashImpl(char_type* buffer)
        { return ::PathAddBackslashW(buffer); }
        static DWORD GetModuleFileNameImpl(HMODULE hModule, char_type* lpFileName, DWORD nSize)
        { return ::GetModuleFileNameW(hModule, lpFileName, nSize); }
        #else
        static UINT GetWindowsDirectoryImpl(char_type* buffer)
        {
            std::vector<char> buffer_temp(buffer, buffer + 8096);
            getcwd(buffer_temp.data(), 8096);
            std::transform(buffer_temp.begin(), buffer_temp.end(), buffer, [&](char ch) { return ch; });
            return 1;
        }
        static UINT GetSystemDirectoryImpl(char_type* buffer)
        {
            return GetWindowsDirectoryImpl(buffer);
        }
        static char_type* AddBackslashImpl(char_type* buffer)
        {
            auto ps = boost::filesystem::path::preferred_separator;
            auto len = StringLength(buffer);
            if (buffer[len - 1] != ps)
            {
                buffer[len] = ps;
                buffer[len + 1] = 0;
            }
            return buffer;
        }
        static DWORD GetModuleFileNameImpl(HMODULE hModule, char_type* lpFileName, DWORD nSize)
        {
            return 0; // ::GetModuleFileNameA(hModule, lpFileName, nSize);
        }
        #endif
    };

    #ifdef WIN32
    #include "ansiwideconverter_win32.h"
    #else
    #include "ansiwideconverter_generic.h"
    #endif
    struct StringConversion
    {

        #ifdef UNICODE
        static const CTL_WString    Convert_Ansi_To_Native(const CTL_String& x) { return ANSIToWide(x); }
        static CTL_WString          Convert_AnsiPtr_To_Native(const char *x) { return ANSIToWide(x); }

        static const CTL_WString&   Convert_Wide_To_Native(const CTL_WString& x) { return x; }
        static CTL_WString          Convert_WidePtr_To_Native(const wchar_t* x) { return x; }

        static CTL_String           Convert_Native_To_Ansi(const CTL_WString& x) { return WideToANSI(x); }
        static CTL_String           Convert_NativePtr_To_Ansi(const wchar_t *x) { return WideToANSI(x); }

        static const CTL_WString&   Convert_Native_To_Wide(const CTL_WString& x) { return x; }
        static CTL_WString          Convert_NativePtr_To_Wide(const wchar_t *x) { return x; }
        #else
        static const CTL_String&   Convert_Ansi_To_Native(const CTL_String& x) { return x; }
        static CTL_String    Convert_AnsiPtr_To_Native(const char *x) { return x; }

        static CTL_String    Convert_Wide_To_Native(const CTL_WString& x) {return WideToANSI(x);}
        static CTL_String    Convert_WidePtr_To_Native(const wchar_t* x) { return WideToANSI(x); }

        static const CTL_String&   Convert_Native_To_Ansi(const CTL_String& x) { return x; }
        static CTL_String    Convert_NativePtr_To_Ansi(const char *x)  { return x; }

        static CTL_WString   Convert_Native_To_Wide(const CTL_String& x) { return ANSIToWide(x); }
        static CTL_WString   Convert_NativePtr_To_Wide(const char *x) { return ANSIToWide(x); }
        #endif

        static CTL_String WideToANSI(const CTL_WString& wstr)
        {
            return static_cast<LPCSTR>(ConvertW2A(wstr.c_str()));
        }

        static CTL_WString ANSIToWide(const CTL_String& str)
        {
            return static_cast<LPCWSTR>(ConvertA2W(str.c_str()));
        }

        template <typename T>
        struct CTL_StringVector
        {
            std::vector<typename T::value_type> m_vChar;
            CTL_StringVector(const T& s) : m_vChar(s.begin(), s.end())
            {
                m_vChar.push_back(0);
            }

            CTL_StringVector(size_t nSize) : m_vChar(nSize, 0)
            { }

            typename T::value_type* getBuffer()
            {
                return m_vChar.data();
            }
        };

        CTL_StringVector<CTL_String> WideToANSIWriteable(const CTL_WString& wstr)
        {
            ConvertW2A conv(wstr.c_str());
            CTL_StringVector<CTL_String> ret((LPCSTR)conv);
            return ret;
        }

        CTL_StringVector<CTL_WString> ANSIToWideWriteable(const CTL_String& str)
        {
            ConvertA2W conv(str.c_str());
            CTL_StringVector<CTL_WString> ret((LPCWSTR)conv);
            return ret;
        }
    };

    template <typename StringType, typename CharType, typename StringTraits>
    struct StringWrapper_Impl
    {
        enum { DRIVE_POS, DRIVE_PATH, DIRECTORY_POS, NAME_POS, EXTENSION_POS };
        typedef std::vector<StringType> StringArrayType;
        // define string helper functions here
        static StringType Right(const StringType& str, size_t nNum)
        {
            size_t nLen = str.length();
            if (nNum == 0)
                return StringTraits::GetEmptyString();
            if (nNum > nLen)
                nNum = nLen;
            return str.substr(nLen - nNum, nNum);
        }

        static StringType Mid(const StringType& str, size_t nFirst)
        {
            if (nFirst == 0)
                return str;
            return str.substr(nFirst);
        }

        static StringType Mid(const StringType& str, size_t  nFirst, size_t nNum)
        {
            return str.substr(nFirst, nNum);
        }

        static StringType Left(const StringType& str, size_t nNum)
        {
            return Mid(str, 0, nNum);
        }

        static CharType GetAt(const StringType &str, size_t nPos)
        {
            assert(nPos >= 0 && nPos < str.length());
            return str[nPos];
        }

        static void SetAt(StringType& str, size_t nPos, CharType c)
        {
            str.replace(nPos, 1, 1, c);
        }

        static bool IsEmpty(const StringType& str)
        {
            return str.empty();
        }

        static void Empty(StringType &str )
        {
            str = StringTraits::GetEmptyString();
        }

        static StringType&  TrimRight(StringType& str, const CharType *lpszTrimStr)
        {
            typename StringType::size_type nPos = str.find_last_not_of(lpszTrimStr);
            // No characters found, so string is already right trimmed
            if ( nPos ==  StringType::npos )
                return str;
            str = str.substr(0, nPos+1);
            return str;
        }

        static StringType& TrimRight(StringType &str, CharType ch= StringTraits::GetSpace() )
        {
            CharType sz[2];
            sz[0]=ch; sz[1] = 0;
            return TrimRight(str, sz);
        }

        static StringType& TrimLeft(StringType& str, const CharType * lpszTrimStr)
        {
            typename StringType::size_type nPos = str.find_first_not_of(lpszTrimStr);
            if ( nPos == StringType::npos )
                return str;
            str = str.substr(nPos);
            return str;
        }

        static StringType& TrimLeft(StringType& str, CharType ch= StringTraits::GetSpace() )
        {
            CharType sz[2];
            sz[0]=ch; sz[1] = 0;
            return TrimLeft(str, sz);
        }

        static StringType& TrimAll(StringType& str, CharType ch = StringTraits::GetSpace())
        {
            TrimRight( str, ch );
            TrimLeft( str, ch );
            return str;
        }

        static StringType& TrimAll(StringType& str, const CharType *lpszTrimStr)
        {
            TrimRight( str, lpszTrimStr );
            TrimLeft( str, lpszTrimStr );
            return str;
        }

        static StringType Join(const StringArrayType &rArray, const StringType& sep = StringTraits::GetEmptyString())
        {
            return boost::algorithm::join(rArray, sep);
        }

        static int Tokenize(const StringType& str, const CharType *lpszTokStr,
                            StringArrayType &rArray, bool bGetNullTokens=false)
        {
            return TokenizeEx(str, lpszTokStr, rArray, bGetNullTokens);
        }

        static int Compare(const StringType& str, const CharType* lpsz)
        {
            return str.compare(lpsz);
        }

        static bool CompareNoCase(const StringType& str, const CharType* lpsz)
        {
            return boost::iequals(str, lpsz);
        }

        static StringType&  MakeUpperCase(StringType& str)
        {
            std::transform(str.begin(), str.end(), str.begin(), StringTraits::ToUpper);
            return str;
        }

        static StringType&  MakeLowerCase(StringType& str)
        {
            std::transform(str.begin(), str.end(), str.begin(), StringTraits::ToLower);
            return str;
        }

        static StringType UpperCase(const StringType& str)
        {
            StringType sTemp = str;
            MakeUpperCase(sTemp);
            return sTemp;
        }

        static StringType LowerCase(const StringType& str)
        {
            StringType sTemp = str;
            MakeLowerCase(sTemp);
            return sTemp;
        }

        static double ToDouble(const StringType& s1)
        {
            return StringTraits::ToDouble(s1.c_str());
        }

        static int ReverseFind(const StringType& str, CharType ch)
        {
            return static_cast<int>(str.rfind(ch));
        }

        static CharType* GetBuffer(const StringType& str)
        {
            return str.c_str();
        }

        static CharType* SafeStrcpy( CharType *pDest,
                                     const CharType* pSrc,
                                     size_t nMaxChars)
        {
            if ( !pSrc || !pDest)
                return pDest;
            size_t nLen = StringTraits::StringLength( pSrc );
            if ( nMaxChars < nLen )
                nLen = nMaxChars; //nLen = MINMAX_NAMESPACE min( nLen, nMaxChars );
            StringTraits::StringCopyN( pDest, pSrc, nLen );
            pDest[nLen] = 0;
            return pDest;
        }

        static CharType* SafeStrcpy( CharType *pDest,
                                     const CharType* pSrc)
        {
            if ( !pSrc || !pDest)
                return pDest;
            return StringTraits::StringCopy( pDest, pSrc );
        }

        static void SplitPath(const StringType& str, StringArrayType & rArray)
        {
            BOOST_PATHTYPE p(str.c_str()); //{ "C:\\Windows\\System" };
            rArray.clear();
            rArray.push_back(BOOST_GENERIC_STRING(p.root_name()));
            rArray.push_back(BOOST_GENERIC_STRING(p.root_directory()));
            rArray.push_back(BOOST_GENERIC_STRING(p.parent_path()));
            rArray.push_back(BOOST_GENERIC_STRING(p.stem()));
            rArray.push_back(BOOST_GENERIC_STRING(p.extension()));
        }

        static StringType GetFileNameFromPath(const StringType& str)
        {
            StringArrayType rArray;
            SplitPath(str, rArray);
            return rArray[NAME_POS] + StringType(".") + rArray[EXTENSION_POS];
        }

        static StringType MakePath(const StringArrayType & rArray)
        {
            if ( rArray.size() < 5 )
                return StringTraits::GetEmptyString();
            namespace fs = boost::filesystem;
            StringType s = rArray[NAME_POS] + rArray[EXTENSION_POS];
            fs::path dir(rArray[DIRECTORY_POS]);
            fs::path file = s; //rArray[NAME_POS] + StringType(".") + rArray[EXTENSION_POS];
            fs::path full_path = dir / file;
            s = BOOST_GENERIC_STRING(full_path);
            return s;
            // std::vector<CharType> retStr(MAX_PATH, 0);
            /*_tmakepath( &retStr[0], rArray[DRIVE_POS].c_str(),
                                    rArray[DIRECTORY_POS].c_str(),
                                    rArray[NAME_POS].c_str(),
                                    rArray[EXTENSION_POS].c_str());*/

            // return &retStr[0];
        }

        static StringType GetWindowsDirectory()
        {
            CharType buffer[_MAX_PATH];
            UINT retValue = StringTraits::GetWindowsDirectoryImpl(buffer);
            if ( retValue != 0 )
                return buffer;
            return StringTraits::GetEmptyString();
        }

        static StringType GetSystemDirectory()
        {
            CharType buffer[_MAX_PATH];
            UINT retValue = StringTraits::GetSystemDirectoryImpl(buffer);
            if ( retValue != 0 )
                return buffer;
            return StringTraits::GetEmptyString();
        }

        static StringType AddBackslashToDirectory(const StringType& pathName)
        {
            std::vector<CharType> buffer(_MAX_PATH,0);
            SafeStrcpy(&buffer[0], pathName.c_str(), _MAX_PATH);
            StringTraits::AddBackslashImpl(&buffer[0]);
            return &buffer[0];
        }

        static StringType GetGUID()
        {
            boost::uuids::uuid u = boost::uuids::random_generator()();
            return StringConversion::Convert_AnsiPtr_To_Native("{") + BOOST_UUID_STRING_CONVERT(u) + StringConversion::Convert_AnsiPtr_To_Native("}");
        }

        static StringType GetModuleFileName(HMODULE hModule)
        {
            // Try 1024 bytes for the app name
            std::vector<CharType> szName(1024,0);
            DWORD nBytes = StringTraits::GetModuleFileNameImpl(hModule, &szName[0], 1024);

            // Get the file name safely
            if ( nBytes > 1024 )
            {
                szName.resize(nBytes+1,0);
                StringTraits::GetModuleFileNameImpl( hModule, &szName[0], nBytes );
            }
            return &szName[0];
        }

        static int TokenizeEx(const StringType& str, const CharType *lpszTokStr,
                                    StringArrayType &rArray, bool bGetNullTokens,
                                    std::vector<unsigned>* positionArray=NULL)
        {
            rArray.clear();
            typedef boost::tokenizer<boost::char_separator<CharType>,
                                     typename StringType::const_iterator,
                                     StringType> tokenizer;
            boost::empty_token_policy tokenPolicy = bGetNullTokens?boost::keep_empty_tokens : boost::drop_empty_tokens;
            boost::char_separator<CharType> sepr(lpszTokStr, StringTraits::GetEmptyString(), tokenPolicy);
            tokenizer tokens(str, sepr);
            typename StringType::const_iterator beg = str.begin();
            for (typename tokenizer::const_iterator tok_iter = tokens.begin();
                tok_iter != tokens.end(); ++tok_iter)
            {
                rArray.push_back(*tok_iter);
                if ( positionArray )
                {
                    std::ptrdiff_t offset = tok_iter.base() - str.begin() - tok_iter->size();
                    positionArray->push_back(static_cast<unsigned>(offset));
                }
            }
            return (int)rArray.size();
        }
    };

    typedef StringWrapper_Impl<CTL_String, char, ANSIStringTraits> StringWrapperA;
    typedef StringWrapper_Impl<CTL_WString, wchar_t, UnicodeStringTraits> StringWrapperW;
    typedef UnicodeStringTraits StringTraitsW;
    typedef ANSIStringTraits    StringTraitsA;




    #define GET_NUM_CHARS_NATIVE(x) (sizeof(x) / sizeof(x[0])
    #define GET_NUM_BYTES_NATIVE(x) sizeof(x)

    #ifdef UNICODE
        typedef StringWrapperW StringWrapper;
        typedef StringTraitsW StringTraits;
    #else
        typedef StringWrapperA StringWrapper;
        typedef StringTraitsA   StringTraits;
    #endif
}
#endif
