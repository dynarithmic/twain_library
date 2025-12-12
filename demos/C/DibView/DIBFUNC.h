#ifndef DIBFUNC_H
#define DIBFUNC_H

#include <windows.h>

#ifdef __cplusplus
extern "C" {
#endif
void DIBPaint (HDC hDC,
            LPRECT lpDCRect,
            HANDLE hDIB,
            LPRECT lpDIBRect,
          HPALETTE hPal);

HPALETTE CreateDIBPalette (HANDLE hDIB);
DWORD DIBHeight (LPSTR lpDIB);
DWORD DIBWidth  (LPSTR lpDIB);
#ifdef __cplusplus
}
#endif

#endif

