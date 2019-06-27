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

For more information, the license file named LICENSE that is located in the root
directory of the DTWAIN installation covers the restrictions under the LGPL license.
Please read this file before deploying or distributing any application using DTWAIN.
*/
#ifndef PNGLIB32_H
#define PNGLIB32_H

#include <windows.h>
#include <winconst.h>
#include <dtwainc.h>
#include <stdio.h>

#ifdef INCLUDE_DTWIMG32
#pragma message ("Regular callback function")
#define FUNCCONVENTION CALLBACK
#else
#pragma message ("DLL entry function")
#define FUNCCONVENTION DLLENTRY_DEF
#endif

#ifdef __cplusplus
extern "C"
{
#endif
LONG FUNCCONVENTION DTWLIB_PNGWriteFile(LPCTSTR szFile,
                                       BYTE *pImage, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 nColors, RGBQUAD *pPal);
#ifdef __cplusplus
}
#endif

#endif

