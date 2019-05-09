#ifndef JPGLIB32_H
#define JPGLIB32_H

#include <windows.h>
#include <winconst.h>
#include <dtwainc.h>
#include <stdio.h>
#ifdef BUILDING_JPEG_DLL
#include "jpeglib.h"
#endif

#ifdef INCLUDE_DTWIMG32
#define FUNCCONVENTION CALLBACK
#else
#define FUNCCONVENTION DLLENTRY_DEF
#endif

#ifdef __cplusplus
extern "C"
{
#endif
LONG FUNCCONVENTION DTWLIB_JPEGWriteFile(LPCTSTR szFile,
                                       BYTE *pImage, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 nColors, RGBQUAD *pPal,
                                       LONG Quality,
                                       LONG UnitOfMeasure, double ScaleFactor,
                                       double ResX, double ResY,
                                       bool bProgressive,
                                       LPCTSTR szAppInfo);
#ifdef __cplusplus
}
#endif

#endif

