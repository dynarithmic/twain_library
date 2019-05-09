#ifndef J2KLIB32_H
#define J2KLIB32_H

#include <windows.h>
#include <winconst.h>
#include <dtwainc.h>

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
LONG FUNCCONVENTION DTWLIB_J2KWriteFile(LPCTSTR szFileIn,
                                      LPCTSTR szFileOut);
#ifdef __cplusplus
}
#endif

#endif

