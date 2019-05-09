/*************************************************************************

      File:  PAINT.C

   Purpose:  Contains the routines to do rendering of device dependent
             bitmaps (DDBs), device independent bitmaps (DIBs), and
             DDBs->DIBs via SetDIBits then DIB->device.

             These routines are called by CHILD.C to do the actual
             bitmap rendering.

 Functions:  DIBPaint
             DDBPaint
             SetDIBitsPaint

  Comments:  Note that a commercial application would probably only
             want to use DDBs, since they are much faster when rendering.
             The DIB and SetDIBits() routines are here to demonstrate
             how to manipulate rendering of DIBs, and DDB->DIB->Screen,
             which has some useful applications.

   History:   Date      Reason
             6/ 1/91    Created

*************************************************************************/


#include "master.h"

//---------------------------------------------------------------------
//
// Function:   DIBPaint
//
// Purpose:    Painting routine for a DIB.  Calls StretchDIBits() or
//             SetDIBitsToDevice() to paint the DIB.  The DIB is
//             output to the specified DC, at the coordinates given
//             in lpDCRect.  The area of the DIB to be output is
//             given by lpDIBRect.  The specified palette is used.
//
// Parms:      hDC       == DC to do output to.
//             lpDCRect  == Rectangle on DC to do output to.
//             hDIB      == Handle to global memory with a DIB spec
//                          in it (either a BITMAPINFO or BITMAPCOREINFO
//                          followed by the DIB bits).
//             lpDIBRect == Rect of DIB to output into lpDCRect.
//             hPal      == Palette to be used.
//
// History:   Date      Reason
//             6/01/91  Created
//
//---------------------------------------------------------------------

void DIBPaint (HDC hDC,
            LPRECT lpDCRect,
            HANDLE hDIB,
            LPRECT lpDIBRect,
          HPALETTE hPal)
{
   LPSTR    lpDIBHdr, lpDIBBits;

   if (!hDIB)
      return;


      // Lock down the DIB, and get a pointer to the beginning of the bit
      //  buffer.

   lpDIBHdr  = (CHAR *)GlobalLock (hDIB);
   lpDIBBits = FindDIBBits (lpDIBHdr);


      // Make sure to use the stretching mode best for color pictures.

   SetStretchBltMode (hDC, COLORONCOLOR);


      // Determine whether to call StretchDIBits() or SetDIBitsToDevice().

   if ((RECTWIDTH (lpDCRect)  == RECTWIDTH (lpDIBRect)) &&
       (RECTHEIGHT (lpDCRect) == RECTHEIGHT (lpDIBRect)))
      {
      SetDIBitsToDevice (hDC,                          // hDC
                         lpDCRect->left,               // DestX
                         lpDCRect->top,                // DestY
                         RECTWIDTH (lpDCRect),         // nDestWidth
                         RECTHEIGHT (lpDCRect),        // nDestHeight
                         lpDIBRect->left,              // SrcX
                         (int) DIBHeight (lpDIBHdr) -
                           lpDIBRect->top -
                           RECTHEIGHT (lpDIBRect),     // SrcY
                         0,                            // nStartScan
                         (WORD) DIBHeight (lpDIBHdr) ,  // nNumScans
                         lpDIBBits,                    // lpBits
                         (LPBITMAPINFO) lpDIBHdr,      // lpBitsInfo
                         DIB_RGB_COLORS);              // wUsage
      }
   else
      StretchDIBits (hDC,                            // hDC
                     lpDCRect->left,                 // DestX
                     lpDCRect->top,                  // DestY
                     RECTWIDTH (lpDCRect),           // nDestWidth
                     RECTHEIGHT (lpDCRect),          // nDestHeight
                     lpDIBRect->left,                // SrcX
                     lpDIBRect->top,                 // SrcY
                     RECTWIDTH (lpDIBRect),          // wSrcWidth
                     RECTHEIGHT (lpDIBRect),         // wSrcHeight
                     lpDIBBits,                      // lpBits
                     (LPBITMAPINFO) lpDIBHdr,        // lpBitsInfo
                     DIB_RGB_COLORS,                 // wUsage
                     SRCCOPY);                       // dwROP

   GlobalUnlock (hDIB);
}





//---------------------------------------------------------------------
//
// Function:   DDBPaint
//
// Purpose:    Painting routine for a DDB.  Calls BitBlt() or
//             StretchBlt() to paint the DDB.  The DDB is
//             output to the specified DC, at the coordinates given
//             in lpDCRect.  The area of the DDB to be output is
//             given by lpDDBRect.  The specified palette is used.
//
//             IMPORTANT assumption:  The palette has been realized
//             elsewhere...  We won't bother figuring out whether it
//             should be realized as a foreground or background palette
//             here.
//
// Parms:      hDC       == DC to do output to.
//             lpDCRect  == Rectangle on DC to do output to.
//             hDDB      == Handle to the device dependent bitmap (DDB).
//             lpDDBRect == Rect of DDB to output into lpDCRect.
//             hPal      == Palette to be used.
//
// History:   Date      Reason
//             6/01/91  Created
//
//---------------------------------------------------------------------

void DDBPaint (HDC hDC,
            LPRECT lpDCRect,
           HBITMAP hDDB,
            LPRECT lpDDBRect,
          HPALETTE hPal)
{
   HDC      hMemDC;
   HBITMAP  hOldBitmap;
   HPALETTE hOldPal1 = NULL;
   HPALETTE hOldPal2 = NULL;


   hMemDC = CreateCompatibleDC (hDC);

   if (!hMemDC)
      return;

   if (hPal)
      {
      hOldPal1   = SelectPalette (hMemDC, hPal, FALSE);
      hOldPal2   = SelectPalette (hDC, hPal, FALSE);
      // Assume the palette's already been realized (no need to
      //  call RealizePalette().  It should have been realized in
      //  our WM_QUERYNEWPALETTE or WM_PALETTECHANGED messages...
      }

   hOldBitmap = (HBITMAP)SelectObject (hMemDC, hDDB);

   SetStretchBltMode (hDC, COLORONCOLOR);

   if ((RECTWIDTH (lpDCRect)  == RECTWIDTH (lpDDBRect)) &&
       (RECTHEIGHT (lpDCRect) == RECTHEIGHT (lpDDBRect)))
      {
      BitBlt (hDC,
              lpDCRect->left,
              lpDCRect->top,
              lpDCRect->right - lpDCRect->left,
              lpDCRect->bottom - lpDCRect->top,
              hMemDC,
              lpDDBRect->left,
              lpDDBRect->top,
              SRCCOPY);
      }
   else
      StretchBlt (hDC,
                  lpDCRect->left,
                  lpDCRect->top,
                  lpDCRect->right - lpDCRect->left,
                  lpDCRect->bottom - lpDCRect->top,
                  hMemDC,
                  lpDDBRect->left,
                  lpDDBRect->top,
                  lpDDBRect->right - lpDDBRect->left,
                  lpDDBRect->bottom - lpDDBRect->top,
                  SRCCOPY);

   SelectObject (hMemDC, hOldBitmap);

   if (hOldPal1)
      SelectPalette (hMemDC, hOldPal1, FALSE);

   if (hOldPal2)
      SelectPalette (hDC, hOldPal2, FALSE);

   DeleteDC (hMemDC);
}



//---------------------------------------------------------------------
//
// Function:   SetDIBitsPaint
//
// Purpose:    Paint routine used when the SetDIBits option is being
//             used.  Routine first call SetDIBits() to convert a DIB
//             to a DDB, then it calls DDBPaint.
//
//             NOTE:  This routine was included for two reasons -- first,
//             to test drivers SetDIBits() functions.  Second, to demo
//             such a technique.  Most applications wouldn't bother to
//             do anything like this (why not just SetDIBitsToDevice()!
//
// Parms:      hDC       == DC to do output to.
//             lpDCRect  == Rectangle on DC to do output to.
//             hDIB      == Handle to global memory with a DIB spec
//                          in it (either a BITMAPINFO or BITMAPCOREINFO
//                          followed by the DIB bits).
//             lpDIBRect == Rect of DIB to output into lpDCRect.
//             hPal      == Palette to be used.
//
// History:   Date      Reason
//             6/01/91  Created
//
//---------------------------------------------------------------------

void SetDIBitsPaint (HDC hDC,
                  LPRECT lpDCRect,
                  HANDLE hDIB,
                  LPRECT lpDIBRect,
                HPALETTE hPal)
{
   LPSTR   lpDIBHdr, lpDIBBits;
   HBITMAP hBitmap;


      // Return if we don't have a DIB.

   if (!hDIB)
      return;



      // Lock down the DIB, and get a pointer to the beginning of the bit
      //  buffer.

   lpDIBHdr  = (LPSTR)GlobalLock (hDIB);
   lpDIBBits = FindDIBBits (lpDIBHdr);


      // Create the DDB.  Note that the palette has already been
      //  selected into the DC before calling this routine.

   hBitmap = CreateCompatibleBitmap (hDC,
                                     (int) DIBWidth (lpDIBHdr),
                                     (int) DIBHeight (lpDIBHdr));
   if (!hBitmap)
      {
      DIBError (ERR_CREATEDDB);
      GlobalUnlock (hDIB);
      return;
      }

   if (SetDIBits (hDC,                        // hDC compat. with DDB
                  hBitmap,                    // handle to DDB
                  0,                          // Start scan in lpBits
                  (int) DIBHeight (lpDIBHdr), // Num Scans in lpBits
                  lpDIBBits,                  // Pointer to bits
                  (LPBITMAPINFO) lpDIBHdr,    // Pointer to DIB header
                  DIB_RGB_COLORS)             // DIB contains RGBs in color table
                     == 0)
      DIBError (ERR_SETDIBITS);


      // Call DDBPaint to paint the bitmap.  Then clean up.

   GlobalUnlock (hDIB);
   DDBPaint (hDC, lpDCRect, hBitmap, lpDIBRect, hPal);
   DeleteObject (hBitmap);
}

