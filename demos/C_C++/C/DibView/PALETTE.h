
#ifndef PALETTE_INCLUDED
#define PALETTE_INCLUDED

   // Size of window extra bytes (we store a handle to a PALINFO structure).

#define PAL_CBWNDEXTRA  (1 * sizeof (WORD))


typedef struct
   {
   HPALETTE hPal;                      // Handle to palette being displayed.
   WORD     wEntries;                  // # of entries in the palette.
   int      nSquareSize;               // Size of palette square (see PAL_SIZE)
   HWND     hInfoWnd;                  // Handle to the info bar window.
   int      nRows, nCols;              // # of Rows/Columns in window.
   int      cxSquare, cySquare;        // Pixel width/height of palette square.
   WORD     wEntry;                    // Currently selected palette square.
   } PALINFO, FAR *LPPALINFO;



   // Window Words.

#define WW_PAL_HPALINFO 0              // Handle to PALINFO structure.



   // The following define is for CopyPaletteChangingFlags().

#define DONT_CHANGE_FLAGS -1



   // The following is the palette version that goes in a
   //  LOGPALETTE's palVersion field.

#define PALVERSION   0x300



// This is an enumeration for the various ways we can display
//  a palette in PaletteWndProc().

enum PAL_SIZE
   {
   PALSIZE_TINY = 0,
   PALSIZE_SMALL,
   PALSIZE_MEDIUM,
   PALSIZE_LARGE
   };



// Menu Defines for Palette Windows.  Note -- these must be in sequential
//  order from TINY to FITWND.  If this is changed, the changes must be
//  reflected in PALETTE.C -- WM_INITMENUPOPUP and WM_COMMAND message
//  processing.

#define IDM_PAL_TINY        1000       // Tiny palette squares.
#define IDM_PAL_SMALL       1001       // Small palette squares.
#define IDM_PAL_MEDIUM      1002       // Medium palette squares.
#define IDM_PAL_LARGE       1003       // Large palette squares.



// String defines for strings in DIBVIEW.RC's string table.

#define IDS_PAL_RGB         2048       // String for PALETTEENTRY.peFlags=0
#define IDS_PAL_RESERVED    2049       // String for PALETTEENTRY.peFlags=1
#define IDS_PAL_EXPLICIT    2050       // String for PALETTEENTRY.peFlags=2
#define IDS_PAL_ERROR       2051       // String for PALETTEENTRY.peFlags=3
#define IDS_PAL_NOCOLLAPSE  2052       // String for PALETTEENTRY.peFlags=4

#define IDS_PAL_NOPAL       2053       // String when no palette passed in.
#define IDS_PAL_DISPRGB     2054       // Format string for status line.


// Function prototypes.

#ifndef WIN32
long FAR PASCAL __export PaletteWndProc (HWND hwnd,
                UINT message,
                WPARAM wParam,
                LPARAM lParam);
#else
long PaletteWndProc (HWND hwnd,
                UINT message,
                WPARAM wParam,
                LPARAM lParam);
#endif

void SetPaletteWindowsPal (HWND hWnd, HPALETTE hPal);

HPALETTE GetSystemPalette (void);

HPALETTE CopyPaletteChangingFlags (HPALETTE hPal, BYTE bNewFlag);

void MyAnimatePalette (HWND hWnd, HPALETTE hPal);

int ColorsInPalette (HPALETTE hPal);

#define CopyPalette(hPal)  CopyPaletteChangingFlags (hPal,(BYTE) DONT_CHANGE_FLAGS)

#define CopyPalForAnimation(hPal) CopyPaletteChangingFlags (hPal, PC_RESERVED)

#endif // PALETTE_INCLUDED
