/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
// VersionInfo.h: interface for the CVersionInfo class.
//
//////////////////////////////////////////////////////////////////////

#if !defined( _VERSIONINFO_H_INCLUDED_ )
#define _VERSIONINFO_H_INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#ifdef _MSC_VER
#pragma comment(lib, "version.lib")
#endif
#include <vector>
#include <unordered_map>
#include <iostream>
#include <tchar.h>
#include <iomanip>
#include <sstream>
#include <array>

struct charTraitsUNICODE
{
  typedef std::wstring          TraitsStringType;
  typedef std::wostringstream   TraitsStringStreamType;
  typedef std::wostream         TraitsBaseOutputStreamType;
  typedef wchar_t               TraitsCharType;
  static DWORD GetModuleFileNameImpl(HMODULE hModule, LPWCH lpFilename, DWORD nSize)
  { return GetModuleFileNameW(hModule, lpFilename, nSize); }

  static DWORD GetFileVersionInfoSizeImpl(LPCWSTR lptstrFilename, LPDWORD lpdwHandle)
  { return GetFileVersionInfoSizeW(lptstrFilename, lpdwHandle); }

  static BOOL GetFileVersionInfoImpl(LPCWSTR lptstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData )
  { return GetFileVersionInfoW(lptstrFilename, dwHandle, dwLen, lpData); }

  static BOOL VerQueryValueImpl(LPCVOID pBlock,LPCWSTR lpSubBlock, LPVOID * lplpBuffer, PUINT puLen )
  { return VerQueryValueW((LPVOID)pBlock, lpSubBlock, lplpBuffer, puLen); }

  static TraitsStringType       Compat(const char* str)
  {
      std::wstring temp;
      temp.insert(temp.begin(), str, str+strlen(str));
      return temp;
  }
};

struct charTraitsANSI
{
    typedef std::string          TraitsStringType;
    typedef std::ostringstream   TraitsStringStreamType;
    typedef std::ostream         TraitsBaseOutputStreamType;
    typedef char                 TraitsCharType;

    static DWORD GetModuleFileNameImpl(HMODULE hModule, LPCH lpFilename, DWORD nSize)
    { return GetModuleFileNameA(hModule, lpFilename, nSize); }

    static DWORD GetFileVersionInfoSizeImpl(LPCSTR lptstrFilename, LPDWORD lpdwHandle)
    { return GetFileVersionInfoSizeA(lptstrFilename, lpdwHandle); }

    static BOOL GetFileVersionInfoImpl(LPCSTR lptstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData )
    { return GetFileVersionInfoA(lptstrFilename, dwHandle, dwLen, lpData); }

    static BOOL VerQueryValueImpl(LPCVOID pBlock,LPCSTR lpSubBlock, LPVOID * lplpBuffer, PUINT puLen )
    { return VerQueryValueA((LPVOID)pBlock, lpSubBlock, lplpBuffer, puLen); }

    static TraitsStringType      Compat(const char* str)
    { return str; }
};

template< typename charTraits>
class VersionInfoImpl
{
    typedef std::unordered_map< typename charTraits::TraitsStringType,
                      typename charTraits::TraitsStringType> STLMapStringToString;

    typedef typename charTraits::TraitsCharType char_type;
    enum
    {
        lookupName_CompanyName     ,
        lookupName_FileDescription ,
        lookupName_FileVersion     ,
        lookupName_InternalName    ,
        lookupName_LegalCopyright  ,
        lookupName_OriginalFilename,
        lookupName_ProductName     ,
        lookupName_ProductVersion  ,
        lookupName_Comments        ,
        lookupName_LegalTrademarks ,
        lookupName_PrivateBuild    ,
        lookupName_SpecialBuild    ,
        lookupName_FileVersion2,
        lookupName_ProductVersion2
    };

    private:
        VS_FIXEDFILEINFO * m_vFixedFileInfo;
        STLMapStringToString m_verStrings;

        typename charTraits::TraitsStringStreamType m_sBuf;
        bool getit( typename charTraits::TraitsCharType const * const iFilename );
        VersionInfoImpl( VersionInfoImpl& );
        typedef std::unordered_map<int, typename charTraits::TraitsStringType> lookupMapType;
        lookupMapType m_lookUps;

        struct TranslationInfo
        {
            WORD language;
            WORD codePage;
        };

    public:

        const char_type* getCompanyName() const { return findVersionStringData(lookupName_CompanyName); }
        const char_type* getFileDescription() const { return findVersionStringData(lookupName_FileDescription);}
        const char_type* getFileVersion() const { return findVersionStringData(lookupName_FileVersion);}
        const char_type* getFileVersionDotted() const { return findVersionStringData(lookupName_FileVersion2);}
        const char_type* getInternalName() const { return findVersionStringData(lookupName_InternalName);}
        const char_type* getLegalCopyright() const { return findVersionStringData(lookupName_LegalCopyright);}
        const char_type* getOriginalFilename() const { return findVersionStringData(lookupName_OriginalFilename);}
        const char_type* getProductName() const { return findVersionStringData(lookupName_ProductName);}
        const char_type* getProductVersion() const { return findVersionStringData(lookupName_ProductVersion);}
        const char_type* getProductVersionDotted() const { return findVersionStringData(lookupName_ProductVersion2);}
        const char_type* getComments() const { return findVersionStringData(lookupName_Comments);}
        const char_type* getLegalTrademarks() const { return findVersionStringData(lookupName_LegalTrademarks);}
        const char_type* getPrivateBuild() const { return findVersionStringData(lookupName_PrivateBuild);}
        const char_type* getSpecialBuild() const { return findVersionStringData(lookupName_SpecialBuild);}

        VersionInfoImpl( HMODULE hMod=NULL);
        virtual ~VersionInfoImpl( ) {}
        void printit( typename charTraits::TraitsBaseOutputStreamType& stream, const char_type *eol=0 ) const;

        DWORD m_dwSignature;
        DWORD m_dwStrucVersion;
        DWORD m_dwFileVersionMS;
        DWORD m_dwFileVersionLS;
        DWORD m_dwProductVersionMS;
        DWORD m_dwProductVersionLS;
        DWORD m_dwFileFlagsMask;
        DWORD m_dwFileFlags;
        DWORD m_dwFileOS;
        DWORD m_dwFileType;
        DWORD m_dwFileSubtype;
        DWORD m_dwFileDateMS;
        DWORD m_dwFileDateLS;

    private:
        //  make sure that there is no assignment operator available for this
        //  class by making it private...
        VersionInfoImpl& operator=( const VersionInfoImpl& ) = delete;
        const char_type* findVersionStringData( int nWhich ) const;
    };

    template <typename charTraits>
    typename charTraits::TraitsBaseOutputStreamType& operator << (
    typename charTraits::TraitsBaseOutputStreamType& os, const VersionInfoImpl<charTraits>& vi);

    typedef VersionInfoImpl<charTraitsANSI> VersionInfoA;
    typedef VersionInfoImpl<charTraitsUNICODE> VersionInfoW;

#ifdef UNICODE
    typedef VersionInfoW VersionInfo;
#else
    typedef VersionInfoA VersionInfo;
#endif

#include "VersionInfo.ipp"

#endif // !defined( _VERSIONINFO_H_INCLUDED_ )
