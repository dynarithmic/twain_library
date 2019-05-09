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

