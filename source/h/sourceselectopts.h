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
#ifndef SOURCESELECTOPTS_H
#define SOURCESELECTOPTS_H

#include "ctliface.h"

enum {SELECTSOURCE=1, SELECTDEFAULTSOURCE, SELECTSOURCEBYNAME, SELECTSOURCE2};

namespace dynarithmic
{
    struct SourceSelectionOptions : NotImpl<SourceSelectionOptions>
    {
        int nWhich;
        LPCTSTR szProduct;
        HWND hWndParent;
        LPCTSTR szTitle;
        LONG xPos;
        LONG yPos;
        LPCTSTR szIncludeNames;
        LPCTSTR szExcludeNames;
        LPCTSTR szNameMapping;
        LONG nOptions;

        SourceSelectionOptions(int n = SELECTSOURCE, LPCTSTR sProd = NULL, HWND parent = NULL, LPCTSTR title = NULL, LONG xP = 0, LONG yP = 0, 
                               LPCTSTR sIncludeNames = NULL, LPCTSTR sExcludeNames = NULL, LPCTSTR sNameMapping = NULL, LONG opt = 0) :
                               nWhich(n), 
                               szProduct(sProd), 
                               hWndParent(parent), 
                               szTitle(title), 
                               xPos(xP), 
                               yPos(yP), 
                               szIncludeNames(sIncludeNames), 
                               szExcludeNames(sExcludeNames), 
                               szNameMapping(sNameMapping), 
                               nOptions(opt) {}
        friend CTL_OutputBaseStreamType& operator << (CTL_OutputBaseStreamType& strm, const SourceSelectionOptions& src);
    };

    inline CTL_OutputBaseStreamType& operator << (CTL_OutputBaseStreamType& strm, const SourceSelectionOptions& src)
    {
        LPCTSTR nuller = _T("null");
        strm << _T("whichOption=") << src.nWhich
            << _T(", productName=") << (src.szProduct ? src.szProduct : nuller)
            << _T(", parentWindow=") << src.hWndParent
            << _T(", title=") << (src.szTitle ? src.szTitle : nuller)
            << _T(", xPos=") << src.xPos
            << _T(", yPos=") << src.yPos
            << _T(", includeNames=") << (src.szIncludeNames ? src.szIncludeNames : nuller)
            << _T(", excludeNames=") << (src.szExcludeNames ? src.szExcludeNames : nuller)
            << _T(", nameMapping=") << (src.szNameMapping ? src.szNameMapping : nuller)
            << _T(", options=") << src.nOptions;
        return strm;
    }

    DTWAIN_SOURCE     DTWAIN_LLSelectSource(const SourceSelectionOptions& opts);
    DTWAIN_SOURCE     DTWAIN_LLSelectSourceByName(const SourceSelectionOptions& opts); //LPCTSTR lpszName);
    DTWAIN_SOURCE     DTWAIN_LLSelectDefaultSource(const SourceSelectionOptions& opts);
    DTWAIN_SOURCE     DTWAIN_LLSelectSource2(const SourceSelectionOptions& opts); //HWND hWndParent, LPCTSTR szTitle, LONG xPos, LONG yPos, LONG nOptions);
}
#endif
