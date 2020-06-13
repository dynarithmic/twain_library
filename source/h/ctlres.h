/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2018 Dynarithmic Software.

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
#ifndef CTLRES_H_
#define CTLRES_H_
#include "ctlobstr.h"

#define DTWAINLANGRESOURCEFILE "twainresourcestrings_"
#define DTWAINRESOURCEINFOFILE "twaininfo.txt"
#define DTWAINLANGRESOURCENAMESFILE  "twainlanguage.txt"

namespace dynarithmic
{
    void LoadImageDLL();
    bool LoadLanguageResource(LPCSTR lpszName, const CTL_ResourceRegistryMap& registryMap);
    bool LoadLanguageResource(LPCSTR lpszName);
    bool LoadLanguageResource(const CTL_String& lpszName, const CTL_ResourceRegistryMap& registryMap);
    bool LoadLanguageResource(const CTL_String& lpszName);
    bool LoadTwainResources();
    void UnloadStringResources();
    void UnloadErrorResources();
    void UnloadImageDLL();
	std::vector<CTL_String> GetLangResourceNames();
    CTL_String GetResourceFileName(LPCTSTR lpszName);
    CTL_String GetResourceFileNameA(LPCSTR lpszName);
}
#endif
