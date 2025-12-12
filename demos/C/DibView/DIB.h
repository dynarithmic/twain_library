
#ifndef DIB_INCLUDED
#define DIB_INCLUDED

// WIDTHBYTES takes # of bits in a scan line and rounds up to nearest
//  word.

#define WIDTHBYTES(bits)      (((bits) + 31) / 32 * 4)


   // Given a pointer to a DIB header, return TRUE if is a Windows 3.0 style
   //  DIB, false if otherwise (PM style DIB).

#define IS_WIN30_DIB(lpbi)  ((*(LPDWORD) (lpbi)) == sizeof (BITMAPINFOHEADER))

#ifdef __cplusplus
extern "C" {
#endif
void     RealizeDIBPalette    (HDC hDC, LPBITMAPINFO lpbmi);
WORD     DIBNumColors         (LPSTR lpbi);
LPSTR    FindDIBBits          (LPSTR lpbi);
WORD     PaletteSize          (LPSTR lpbi);
HPALETTE CreateDIBPalette     (HANDLE hDIB);
DWORD    DIBHeight            (LPSTR lpDIB);
DWORD    DIBWidth             (LPSTR lpDIB);
HBITMAP  DIBToBitmap          (HANDLE hDIB, HPALETTE hPal);
HANDLE   BitmapToDIB          (HBITMAP hBitmap, HPALETTE hPal);
void     InitBitmapInfoHeader (LPBITMAPINFOHEADER lpBmInfoHdr,
                                            DWORD dwWidth,
                                            DWORD dwHeight,
                                              int nBPP);

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
#endif // DIB_INCLUDED

