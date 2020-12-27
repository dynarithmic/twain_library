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
#ifndef TIFLIB32_H
#define TIFLIB32_H

#include <winconst.h>
#include "libtiff\tiff.h"
#include "libtiff\tiffio.h"
#include "libtiff\tiffiop.h"
#include "libtiff_def.h"
#include <windows.h>
#ifdef INCLUDE_DTWIMG32
#pragma message ("Regular callback function")
#define FUNCCONVENTION CALLBACK
#else
#pragma message ("DLL entry function")
#define FUNCCONVENTION DLLENTRY_DEF
#endif

#ifdef __cplusplus
extern "C" {
#endif
LIBTIFF_NAMESPACE TIFF* FUNCCONVENTION DTWLIB_TIFFOpen(LPCTSTR szFile, LPCTSTR szOpen);
LONG FUNCCONVENTION DTWLIB_TIFFWriteDirectory(LIBTIFF_NAMESPACE TIFF* pTiff);
void FUNCCONVENTION DTWLIB_TIFFSetImageSize(LIBTIFF_NAMESPACE TIFF* pTiff, LONG Width, LONG Length);
void FUNCCONVENTION DTWLIB_TIFFSetSamplesPerPixel(LIBTIFF_NAMESPACE TIFF* pTiff, LONG SamplesPerPixel, LONG BitsPerSample);
void FUNCCONVENTION DTWLIB_TIFFSetOrientation(LIBTIFF_NAMESPACE TIFF* pTiff, LONG nOrientation);
void FUNCCONVENTION DTWLIB_TIFFSetCompression(LIBTIFF_NAMESPACE TIFF* pTiff, LONG nCompression);
void FUNCCONVENTION DTWLIB_TIFFSetResolutionPerUnit(LIBTIFF_NAMESPACE TIFF* pTiff, LONG Unit);
void FUNCCONVENTION DTWLIB_TIFFSetResolution(LIBTIFF_NAMESPACE TIFF* pTiff, double Resolution, LONG nWhich);
void FUNCCONVENTION DTWLIB_TIFFSetPlanarConfig(LIBTIFF_NAMESPACE TIFF* pTiff, LONG nConfig);
void FUNCCONVENTION DTWLIB_TIFFSetPhotometric(LIBTIFF_NAMESPACE TIFF* pTiff, LONG nPhoto);
void FUNCCONVENTION DTWLIB_TIFFSetRowsPerStrip(LIBTIFF_NAMESPACE TIFF* pTiff, LONG nRows);
void FUNCCONVENTION DTWLIB_TIFFSetColorMap(LIBTIFF_NAMESPACE TIFF* pTiff, uint16 *pRed,
                                  uint16 *pGreen,
                                  uint16 *pBlue);
void FUNCCONVENTION DTWLIB_TIFFSetJpegQuality(LIBTIFF_NAMESPACE TIFF* pTiff, LONG nQuality);
LONG FUNCCONVENTION DTWLIB_TIFFSetScanLine(LIBTIFF_NAMESPACE TIFF *pTiff, LIBTIFF_NAMESPACE tdata_t buf, uint32 row,
                                           LIBTIFF_NAMESPACE tsample_t sample);
void FUNCCONVENTION DTWLIB_TIFFCloseImage(LIBTIFF_NAMESPACE TIFF* pTiff);
void FUNCCONVENTION DTWLIB_TIFFFreeColorMap(uint16 *pRed, uint16 *pGreen, uint16 *pBlue);
LIBTIFF_NAMESPACE tdata_t FUNCCONVENTION DTWLIB_TIFFAlloc(LIBTIFF_NAMESPACE tsize_t nSize);
void FUNCCONVENTION DTWLIB_TIFFFree(LIBTIFF_NAMESPACE tdata_t p);
void FUNCCONVENTION DTWLIB_TIFFWriteSoftware(LIBTIFF_NAMESPACE TIFF *pTiff, LPCTSTR szData);

// void FUNCCONVENTION DTWLIB_TIFFSplit(LPCSTR szInFile, LPCSTR szTempDir);

#ifdef __cplusplus
}
#endif

#endif