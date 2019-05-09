#ifndef PLUGINBMP_H
#define PLUGINBMP_H
// ----------------------------------------------------------
//   Constants + headers
// ----------------------------------------------------------

static const BYTE RLE_COMMAND = 0;
static const BYTE RLE_ENDOFLINE = 0;
static const BYTE RLE_ENDOFBITMAP = 1;
static const BYTE RLE_DELTA = 2;

static const BYTE BI_RGB = 0;	// compression: none
static const BYTE BI_RLE8 = 1;	// compression: RLE 8-bit/pixel
static const BYTE BI_RLE4 = 2;	// compression: RLE 4-bit/pixel
static const BYTE BI_BITFIELDS = 3;	// compression: Bit field or Huffman 1D compression for BITMAPCOREHEADER2
static const BYTE BI_JPEG = 4;	// compression: JPEG or RLE-24 compression for BITMAPCOREHEADER2
static const BYTE BI_PNG = 5;	// compression: PNG
static const BYTE BI_ALPHABITFIELDS = 6;	// compression: Bit field (this value is valid in Windows CE .NET 4.0 and later)

// ----------------------------------------------------------

#ifdef _WIN32
#pragma pack(push, 1)
#else
#pragma pack(1)
#endif

typedef struct tagBITMAPINFOOS2_1X_HEADER {
    DWORD  biSize;
    WORD   biWidth;
    WORD   biHeight;
    WORD   biPlanes;
    WORD   biBitCount;
} BITMAPINFOOS2_1X_HEADER, *PBITMAPINFOOS2_1X_HEADER;

typedef struct tagBITMAPFILEHEADER {
    WORD    bfType;		//! The file type
    DWORD   bfSize;		//! The size, in bytes, of the bitmap file
    WORD    bfReserved1;	//! Reserved; must be zero
    WORD    bfReserved2;	//! Reserved; must be zero
    DWORD   bfOffBits;	//! The offset, in bytes, from the beginning of the BITMAPFILEHEADER structure to the bitmap bits
} BITMAPFILEHEADER, *PBITMAPFILEHEADER;

#ifdef _WIN32
#pragma pack(pop)
#else
#pragma pack()
#endif
#endif