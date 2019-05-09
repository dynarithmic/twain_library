/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2019 Dynarithmic Software.

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
// CVersionInfo implementation

#define SWAPWORDS(X) ( (X<<16) | (X>>16) )

template <typename charTraits>
VersionInfoImpl<charTraits>::VersionInfoImpl(HMODULE hMod )
{
    #define VERSION_SET_LOOKUP(x) m_lookUps[lookupName_##x] = charTraits::Compat(#x)

    VERSION_SET_LOOKUP(CompanyName);
    VERSION_SET_LOOKUP(FileDescription);
    VERSION_SET_LOOKUP(FileVersion);
    VERSION_SET_LOOKUP(InternalName);
    VERSION_SET_LOOKUP(LegalCopyright);
    VERSION_SET_LOOKUP(OriginalFilename);
    VERSION_SET_LOOKUP(ProductName);
    VERSION_SET_LOOKUP(ProductVersion);
    VERSION_SET_LOOKUP(Comments);
    VERSION_SET_LOOKUP(LegalTrademarks);
    VERSION_SET_LOOKUP(PrivateBuild);
    VERSION_SET_LOOKUP(SpecialBuild);
    VERSION_SET_LOOKUP(FileVersion2);
    VERSION_SET_LOOKUP(ProductVersion2);

    std::for_each(m_lookUps.begin(), m_lookUps.end(), [&](const typename lookupMapType::value_type& pr) {m_verStrings[pr.second].clear();});

    typename std::array<typename charTraits::TraitsCharType, 4096> cModule;
    charTraits::GetModuleFileNameImpl( hMod, cModule.data(), static_cast<DWORD>(cModule.size()));
    if ( !getit( cModule.data() ) )
        throw "Module cannot be loaded";
    return;
}

template <typename charTraits>
const typename charTraits::TraitsCharType* VersionInfoImpl<charTraits>::findVersionStringData(int which) const
{
    auto it = m_lookUps.find(which);
    if ( it != m_lookUps.end())
    {
        auto it2 = m_verStrings.find(it->second);
        if (it2 != m_verStrings.end() )
            return it2->second.c_str();
    }
    return nullptr;
}

template <typename charTraits>
bool VersionInfoImpl<charTraits>::getit( typename charTraits::TraitsCharType const * const iFilename )
{
    DWORD dwSize2 = 0;
    DWORD const dwSize = charTraits::GetFileVersionInfoSizeImpl( iFilename, &dwSize2 );
    if ( dwSize == 0 )
        return false;

    std::vector<BYTE> versionInfo(dwSize);
    if ( !charTraits::GetFileVersionInfoImpl( iFilename, 0, dwSize, &versionInfo[0] ) )
        return false;

    UINT  uFfiLen;

    if ( charTraits::VerQueryValueImpl( &versionInfo[0], charTraits::Compat( "\\" ).c_str(), (LPVOID *) &m_vFixedFileInfo, &uFfiLen ) )
    {
        m_dwSignature = m_vFixedFileInfo->dwSignature;
        m_dwStrucVersion = m_vFixedFileInfo->dwStrucVersion;
        m_dwFileVersionMS = m_vFixedFileInfo->dwFileVersionMS;
        m_dwFileVersionLS = m_vFixedFileInfo->dwFileVersionLS;
        m_dwProductVersionMS = m_vFixedFileInfo->dwProductVersionMS;
        m_dwProductVersionLS = m_vFixedFileInfo->dwProductVersionLS;
        m_dwFileFlagsMask = m_vFixedFileInfo->dwFileFlagsMask;
        m_dwFileFlags = m_vFixedFileInfo->dwFileFlags;
        m_dwFileOS = m_vFixedFileInfo->dwFileOS;
        m_dwFileType = m_vFixedFileInfo->dwFileType;
        m_dwFileSubtype = m_vFixedFileInfo->dwFileSubtype;
        m_dwFileDateMS = m_vFixedFileInfo->dwFileDateMS;
        m_dwFileDateLS = m_vFixedFileInfo->dwFileDateLS;

        // Create the dotted version numbers for File and Product names from the
        // raw file and product fields.
        const typename charTraits::TraitsStringType sDot = charTraits::Compat(".");
        {
            typename charTraits::TraitsStringStreamType strm;
            strm << (m_dwFileVersionMS >> 16) << sDot << (m_dwFileVersionMS & 0x00FF) << sDot
                << (m_dwFileVersionLS >> 16) << sDot << (m_dwFileVersionLS & 0x00FF);
            m_verStrings[ m_lookUps[lookupName_FileVersion2]] = strm.str();
        }

        {
            typename charTraits::TraitsStringStreamType strm;
            strm << (m_dwProductVersionMS >> 16) << sDot << (m_dwProductVersionMS & 0x00FF) << sDot
                <<  (m_dwProductVersionLS >> 16) << sDot << (m_dwProductVersionLS & 0x00FF);
            m_verStrings[ m_lookUps[lookupName_ProductVersion2]] = strm.str();
        }

        UINT     uTranLen = 0;
        LPDWORD  lpdwLangCp = NULL;
        if ( charTraits::VerQueryValueImpl( &versionInfo[0],
                                            charTraits::Compat( "\\VarFileInfo\\Translation" ).c_str(), (LPVOID *) &lpdwLangCp, &uTranLen ))
        {
            for ( int iIndex = 0; iIndex < (int)( uTranLen / sizeof( VersionInfoImpl<charTraits>::TranslationInfo ) );
						iIndex++ )
            {
                m_sBuf.str( ( charTraits::Compat("") ) );

                // Flip the words to display lang first.
                typename charTraits::TraitsStringStreamType szStrm;
                szStrm.fill((char_type)'0');
                szStrm << std::hex << std::setw(8) << SWAPWORDS( *( lpdwLangCp + iIndex ) );
                typename charTraits::TraitsStringType szLangCp = szStrm.str();

                // Cycle through possible string values.
                for ( typename STLMapStringToString::iterator it = m_verStrings.begin( )
                    ; it != m_verStrings.end( )
                    ; ++it
                    )
                {
                    typename charTraits::TraitsStringStreamType strm;
                    UINT  uVerLen = 0;
                    m_sBuf.str( charTraits::Compat(""));
                    m_sBuf <<  charTraits::Compat("\\StringFileInfo\\") << szLangCp << charTraits::Compat("\\")
                           << (*it).first.c_str();
                    strm << charTraits::Compat("\\StringFileInfo\\") << szLangCp <<
                        charTraits::Compat("\\") << (*it).first;

                    char_type* szVer = NULL;
                    if ( charTraits::VerQueryValueImpl( &versionInfo[0], strm.str().c_str(), (LPVOID *) &szVer, &uVerLen ) )
                    {
                        (*it).second = (const char_type*) szVer;
                    }
                }
            }
        }
    }
    return true;
}

template <typename charTraits>
void VersionInfoImpl<charTraits>::printit(typename charTraits::TraitsBaseOutputStreamType& stream, const char_type* eol) const
{
    for ( auto it = m_verStrings.begin( )
        ; it != m_verStrings.end( )
        ; ++it
        )
    {
        if ( (*it).second.empty() )
        {
            stream << (*it).first.c_str( ) << charTraits::Compat(": (none)");
            if ( !eol )
                stream << std::endl;
            else
                stream << eol;
        }
        else
        {
            stream << (*it).first.c_str( ) << charTraits::Compat(": ") << (*it).second.c_str( );
            if ( !eol )
                stream << std::endl;
            else
                stream << eol;
         }
    }
    return;
}

template <typename charTraits>
typename charTraits::TraitsBaseOutputStreamType& operator << (typename charTraits::TraitsBaseOutputStreamType& os,
                                                     const VersionInfoImpl<charTraits>& vi)
{
    vi.printit( os );
    return os;
}
