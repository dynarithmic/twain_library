
#ifndef PAINT_INCLUDED
#define PAINT_INCLUDED

void DIBPaint (HDC hDC,
            LPRECT lpDCRect,
            HANDLE hDIB,
            LPRECT lpDIBRect,
          HPALETTE hPal);

void DDBPaint (HDC hDC,
            LPRECT lpDCRect,
           HBITMAP hDDB,
            LPRECT lpDDBRect,
          HPALETTE hPal);

void SetDIBitsPaint (HDC hDC,
                  LPRECT lpDCRect,
                  HANDLE hDIB,
                  LPRECT lpDIBRect,
                HPALETTE hPal);


#endif // PAINT_INCLUDED
