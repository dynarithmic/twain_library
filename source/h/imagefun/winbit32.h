/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
 */
#ifndef WINBIT32_H
#define WINBIT32_H

#include <stdio.h>
#include "dtwainc.h"
#include <string>
#include "fltrect.h"
#include "dibmulti.h"
#include "dibinfox.h"
#include "dtwdecl.h"
#include "ctlobstr.h"
#include "FreeImagePlus.h"
#include "dtwain_raii.h"
#ifdef _MSC_VER
#pragma warning (disable:4100)
#endif

#ifdef NO_NATIVE_UINT32
  typedef unsigned long UINT32;
  typedef unsigned long ULONG32;
#endif

#define GREY1(r,g,b) (BYTE)(((WORD)r*77 + (WORD)g*150 + (WORD)b*29) >> 8)   // .299R + .587G + .114B
#define GREY2(r,g,b) (BYTE)(((WORD)r*169 + (WORD)g*256 + (WORD)b*87) >> 9)  // .33R + 0.5G + .17B
#define RGB565(b, g, r) (((b) >> 3) | (((g) >> 2) << 5) | (((r) >> 3) << 11))
#define RGB555(b, g, r) (((b) >> 3) | (((g) >> 3) << 5) | (((r) >> 3) << 10))
#define WIDTHBYTES(i)   ((((i)+31)/32)*4)
#define PIXELS2BYTES(n) (((n)+7)/8)


#define GetChunkyPixel(pxx,nxx) (!((nxx) & 1)) ? (((pxx)[(nxx)>>1] >> 4) & 0x0f) : ((pxx)[(nxx)>>1] & 0x0f)
#define LPBlinewidth(lpbi) (WIDTHBYTES((WORD)lpbi->biWidth*lpbi->biBitCount))
#define LPBwidth(lpbi)  (lpbi->biWidth)
#define LPBdepth(lpbi)  (lpbi->biHeight)
#define LPBbits(lpbi)   (lpbi->biBitCount)
#define LPBcolours(lpbi) (lpbi->biClrUsed)
#define LPBimage(lpbi)  ((HPSTR)lpbi+lpbi->biSize+(long)(lpbi->biClrUsed*sizeof(RGBQUAD)))

#ifdef WIN32
#define PLATFORM                "32-bit Windows"
#define FixedGlobalAlloc(n)     (char *)malloc(n)
#define FixedGlobalFree(p)      free(p)
#define FixedGlobalRealloc(p,n) realloc(p,n)
#define hmemcpy_(d,s,n)          memcpy(d,s,n)
typedef char* HPSTR;
#endif

#define RGB_RED         0
#define RGB_GREEN       1
#define RGB_BLUE        2
#define RGB_SIZE        3
#define WRGB_RED        2
#define WRGB_GREEN      1
#define WRGB_BLUE       0
#define DITHERBRIGHTNESS    20
#define DITHERCONTRAST      20
#define BYTEBUFFERSIZE  2048

namespace dynarithmic
{

#ifndef _WIN32
    #define GMEM_MOVEABLE 0x0002
    #define GMEM_DDESHARE 0x2000
    struct MemoryNode
    {
        char *ptr;
        size_t numBytes;
    };

    struct ImageMemoryHandler
    {
        static LPVOID GlobalLock(HANDLE hDib) { return hDib; }
        static BOOL GlobalUnlock(HGLOBAL) { return 1; }
        static DWORD GetLastError() { return 1; }

        static HGLOBAL GlobalAlloc(UINT n, SIZE_T numBytes)
        {
            MemoryNode *pNode = new MemoryNode;
            pNode->ptr = new char[numBytes];
            if (n)
                std::fill_n(pNode->ptr, numBytes, 0);
            pNode->numBytes = numBytes;
            return pNode;
        }

        static HGLOBAL GlobalFree(HGLOBAL h)
        {
            MemoryNode *pNode = reinterpret_cast<MemoryNode*>(h);
            delete[] pNode->ptr;
            delete pNode;
            return 0;
        }

        static HGLOBAL GlobalReAlloc(HGLOBAL hMem, SIZE_T dwBytes, UINT uFlags)
        {
            MemoryNode *pNode = reinterpret_cast<MemoryNode*>(hMem);
            if (dwBytes <= pNode->numBytes)
                return pNode;
            char *temp = new char[dwBytes];
            if (uFlags)
                std::fill_n(temp, dwBytes, 0);
            std::copy(pNode->ptr, pNode->ptr + pNode->numBytes, temp);
            delete[] pNode->ptr;
            pNode->ptr = temp;
            pNode->numBytes = dwBytes;
            return hMem;
        }

        static HGLOBAL GlobalReAllocPr(HGLOBAL hMem, SIZE_T dwBytes, UINT uFlags) { return GlobalReAlloc(hMem, dwBytes, uFlags);}
        static BOOL DeleteObject(HGDIOBJ h) { return 1; }
        static LPVOID  GlobalAllocPr(UINT n, SIZE_T numBytes) {return GlobalLock(GlobalAlloc(n, numBytes)); }
        static LPVOID  GlobalReAllocPr(UINT n, SIZE_T numBytes) {return GlobalLock(GlobalAlloc(n, numBytes)); }
        static BOOL    GlobalFreePr(LPVOID h) {GlobalFree(h); return 1;}
        static HGLOBAL GlobalHandle(LPCVOID h) { return (HGLOBAL)h; }
        static SIZE_T  GlobalSize(HGLOBAL h)
        {
            if ( !h )
                return 0;
            MemoryNode *pNode = reinterpret_cast<MemoryNode*>(h);
            return pNode->numBytes;
        }
    };

    typedef struct _SMALL_RECT {
        SHORT Left;
        SHORT Top;
        SHORT Right;
        SHORT Bottom;
    } SMALL_RECT, *PSMALL_RECT;

    #define GetRValue(rgb)      (LOBYTE(rgb))
    #define GetGValue(rgb)      (LOBYTE(((WORD)(rgb)) >> 8))
    #define GetBValue(rgb)      (LOBYTE((rgb)>>16))

#else
    #include <windowsx.h>
    struct ImageMemoryHandler
    {
        static LPVOID GlobalLock(HANDLE hDib) { return ::GlobalLock(hDib); }
        static BOOL GlobalUnlock(HGLOBAL h) { return ::GlobalUnlock(h); }
        static DWORD GetLastError() { return ::GetLastError(); }
        static HGLOBAL GlobalFree(HGLOBAL h) { return ::GlobalFree(h); }
        static BOOL DeleteObject(HGDIOBJ h) { return ::DeleteObject(h); }
        static HGLOBAL GlobalAlloc(UINT n, SIZE_T numBytes) { return ::GlobalAlloc(n, numBytes); }
        static HGLOBAL GlobalReAlloc(HGLOBAL hMem, SIZE_T dwBytes, UINT uFlags) { return ::GlobalReAlloc(hMem, dwBytes, uFlags); }
        static HGLOBAL GlobalReAllocPr(HGLOBAL hMem, SIZE_T dwBytes, UINT uFlags) { return GlobalReAllocPtr(hMem, dwBytes, uFlags); }
        static LPVOID  GlobalAllocPr(UINT n, SIZE_T numBytes) { return GlobalAllocPtr(n, numBytes); }
        static BOOL    GlobalFreePr(LPVOID h) { GlobalUnlockPtr(h); GlobalFreePtr(h); return TRUE;}
        static HGLOBAL GlobalHandle(LPCVOID h) { return ::GlobalHandle(h); }
        static SIZE_T  GlobalSize(HGLOBAL h) { return ::GlobalSize(h); }
    };
#endif
    struct FIBITMAP_DestroyTraits
    {
        static void Destroy(FIBITMAP* b)
        {
            if (b)
                FreeImage_Unload(b);
        }
    };

    struct fipImage_DestroyTraits
    {
        static void Destroy(fipImage* fw)
        {
            if (fw)
                fw->clear();
        }
    };

    typedef DTWAIN_RAII<FIBITMAP*, FIBITMAP_DestroyTraits> FIBITMAP_RAII;
    typedef DTWAIN_RAII<fipImage*, fipImage_DestroyTraits> fipWinImage_RAII;

    class CTL_ImageIOHandler;
    enum {
        FIC_MINISWHITE = 0,             // min value is white
        FIC_MINISBLACK = 1,             // min value is black
        FIC_RGB        = 2,             // RGB color model
        FIC_PALETTE    = 3,             // color map indexed
        FIC_RGBALPHA   = 4,             // RGB color model with alpha channel
    };


    enum {
        IS_ERR_TRIALVERSION = -1,   // LIB was not initialized with a registered key
        IS_ERR_OK = 0,              // no err
        IS_ERR_MEM = 1,             // out of memory
        IS_ERR_FILEOPEN,            // error on file open
        IS_ERR_FILEREAD,            // error on file read
        IS_ERR_FILEWRITE,           // error on file write
        IS_ERR_BADPARAM = 5,        // bad user param
        IS_ERR_INVALIDBMP,          // bad BMP file
        IS_ERR_BMPRLE,              // some RLE variations are not supported
        IS_ERR_RESERVED1,           // reserved value
        IS_ERR_INVALIDJPG,          // bad JPG file
        IS_ERR_DC = 10,             // error with device context
        IS_ERR_DIB,                 // problem with a GetDIBits call
        IS_ERR_RESERVED2,           // reserved value
        IS_ERR_NORESOURCE,          // resource not found
        IS_ERR_CALLBACKCANCEL,      // callback returned FALSE - operation aborted
        IS_ERR_INVALIDPNG = 15,     // bad PNG file
        IS_ERR_PNGCREATE,           // internal PNG lib behavior - contact smaller animals s.w.
        IS_ERR_INTERNAL,            // misc unexpected behavior error - contact smaller animals s.w.
        IS_ERR_FONT,                // trouble creating a font object
        IS_ERR_INTTIFF,             // misc internal TIFF error
        IS_ERR_INVALIDTIFF = 20,    // invalid TIFF file
        IS_ERR_NOTIFFLZW,           // this will not read TIFF-LZW images (note, unused error message)
        IS_ERR_INVALIDPCX,          // invalid PCX image
        IS_ERR_CREATEBMP,           // a call to the fn CreateCompatibleBitmap failed
        IS_ERR_NOLINES,             // end of an image while using single-line de/compression
        IS_ERR_GETDIB = 25,         // error during a call to GetDIBits
        IS_ERR_NODEVOP,             // device does not support an operation required by this function
        IS_ERR_INVALIDWMF,          // invalid windows metafile
        IS_ERR_DEPTHMISMATCH,       // the file was not of the requested bit-depth
        IS_ERR_BITBLT,              // a call to BitBlt failed.
        IS_ERR_BUFTOOSMALL = 30,    // output buffer is too small for this operation
        IS_ERR_TOOMANYCOLORS,       // not enough room in the output palette to store the colors from this image
        IS_ERR_INVALIDTGA,          // Invalid TGA File
        IS_ERR_NOTGATHUMBNAIL,      // No TGA Thumbnail in the file
        IS_ERR_RESERVED3,           // reserved value
        IS_ERR_CREATEDIB = 35,      // a call to the fn CreateDIBitmap failed
        IS_ERR_NOLZW,               // LZW de/compression is not permitted
        IS_ERR_SELECTOBJ,           // a call to SelectObject has failed (DC does not support this operation?)
        IS_ERR_BADMANAGER,          // the HISSRC or HISDEST object passed into the function does appear to be valid
        IS_ERR_OBSOLETE,            // the function is obsolete
        IS_ERR_CREATEDIBSECTION=40, // a call to CreateDIBSection failed
        IS_ERR_SETWINMETAFILEBITS,  // a call to SetWinMetaFileBits failed (95/98 only)
        IS_ERR_GETWINMETAFILEBITS,  // a call to GetEnhMetaFileBits or GetWinMetaFileBits failed
        IS_ERR_PAXPWD,              // apparently invalid PAX password
        IS_ERR_INVALIDPAX,          // invalid PAX file
        IS_ERR_NOSUPPORT = 45,      // this function is not supported in this build (see DLL build options)
        IS_ERR_INVALIDPSD,          // invalid PSD (Photoshop) file
        IS_ERR_PSDNOTSUPPORTED,     // this Photoshop sub-format is not supported
        IS_ERR_DECRYPT,             // decryption error - possible bad password for encrypted files
        IS_ERR_ENCRYPT,             // encryption failed
        IS_ERR_COMPRESSION = 50,    // compression failed
        IS_ERR_DECOMPRESSION,       // decompression error - possible bad password for encrypted files
        IS_ERR_INVALIDTLA,          // invalid TLA file. may indicate incorrect password
        IS_ERR_INVALIDWBMP,         // invalid or unsupported WBMP (Wireless Bitmap) image.
        IS_ERR_NOTIFFTAG,           // ImgSource does not support reading this TIFF tag
        IS_ERR_NOLOCALSTORAGE = 55, // ImgSource was not able to allocate thread-local storage. this is a severe low-memory condition.
        IS_ERR_INVALIDEXIF,         // invalid EXIF format
        IS_ERR_NOEXIFSTRING,        // no EXIF string was found with the given ID
    };

    typedef void (*CONVERSION8_FUNC)(BYTE *target, BYTE *source, int width_in_pixels);
    typedef void (*CONVERSION16_FUNC)(BYTE *target, BYTE *source, int width_in_pixels, RGBQUAD *pPal);
    typedef void (*CONVERSION24_FUNC)(BYTE *target, BYTE *source, int width_in_pixels, RGBQUAD *pPal);

    #define RGB_RED         0
    #define RGB_GREEN       1
    #define RGB_BLUE        2
    #define RGB_SIZE        3

    class CDibInterfaceStream
    {
        public:
            bool OpenOutputFile(LPCTSTR pFileName)
            {
                m_outFileName = pFileName;
                m_outStream.open(StringConversion::Convert_NativePtr_To_Ansi(pFileName), std::ios::binary);
                if (m_outStream)
                    return true;
                return false;
            }

            bool CloseOutputFile()
            {
                if (m_outStream)
                {
                    m_outStream.close();
                    if (boost::filesystem::exists(m_outFileName))
                        return true;
                }
                return false;
            }

            std::ofstream& getStream() { return m_outStream; }
            CTL_StringType getOutputFileName() const { return m_outFileName; }

        protected:
            CTL_StringType m_outFileName;
            std::ofstream m_outStream;
    };

    class CDibInterface
    {
        public:
            CDibInterface();
            virtual ~CDibInterface() {}

            // Virtual interface
            virtual CTL_String GetFileExtension() const = 0;
            virtual HANDLE  GetFileInformation(LPCSTR path) = 0;
            virtual int     WriteImage(CTL_ImageIOHandler* ptrHandler, BYTE * /*pImage2*/, UINT32 /*wid*/, UINT32 /*ht*/, UINT32 /*bpp*/, UINT32 /*nColors*/, RGBQUAD * /*pPal*/,
                                       void * /*pUserInfo*/ = NULL) { return TRUE; }

            virtual void SetMultiPageStatus(DibMultiPageStruct * /*pStruct*/) { }
            virtual void GetMultiPageStatus(DibMultiPageStruct * /*pStruct*/) { }
            virtual int WriteGraphicFile(CTL_ImageIOHandler* /*pThis*/, LPCTSTR /*path*/, HANDLE /*bitmap*/, void * /*pUserInfo*/ = NULL);
            virtual int WriteGraphicFile(CTL_ImageIOHandler* /*pThis*/, LPCTSTR /*path*/, HANDLE /*bitmap*/, bool /*bUsefh*/ =false,
                                          int /*fhToUse*/ = 0, LONG /*Info*/ =0) { return 1; }

            static HANDLE CreateDIB(int width, int height, int bpp, LPSTR palette=NULL);

            static int CalculateLine(int width, int bitdepth) {
                return ((width * bitdepth) + 7) / 8;
            }

            static int CalculatePitch(int line) {
                return (line + 3) & ~3;
            }

            static int CalculateUsedPaletteEntries(int bit_count) {
                if ((bit_count >= 1) && (bit_count <= 8))
                    return 1 << bit_count;
                return 0;
            }

            static int CalculateEffWidth(int width, int bpp) {
                return ((((width * bpp) + 31) / 32) * 4);
            }

            static unsigned GetLine(BYTE *pDib);

            static unsigned char * CalculateScanLine(unsigned char *bits, unsigned pitch, int scanline)
            {
                return (bits + (pitch * scanline));
            }

            static unsigned char * GetScanLine(BYTE *pDib, int scanline);

            static BYTE *   GetDibBits(BYTE *pDib);
            static unsigned GetPitch(BYTE *pDib);
            static unsigned GetPitch(fipImage& pDib);


            static RGBQUAD* GetPalettePtr(BYTE *pDibData, int bpp);
            static int GetDibPalette(fipImage& lpbi,LPSTR palette);

            static bool GetWidth(BYTE *pDIB, UINT32 *puWidth);
            static bool GetHeight(BYTE *pDIB, UINT32 *piHeight);
            static bool GetBitsPerPixel(BYTE *pDIB, UINT32 *puBitCount);
            static unsigned char HINIBBLE (unsigned char byte)
            {    return byte & 240;  }

            static unsigned char LOWNIBBLE (unsigned char byte)
            {    return byte & 15;  }

            static LPSTR GetMonoPalette(LPSTR palette);

            LONG    GetLastError() { return m_lasterror; }
            static bool    IsGrayScale(BYTE *pImage, int bpp);
            static bool    IsGrayScale(HANDLE hDib, int bpp);
            static bool    IsBlankDIB(HANDLE hDib, double threshold=0.99);

            // Crop functions
            static HANDLE ResampleDIB(HANDLE hDib, long newx, long newy);
            static HANDLE ResampleDIB(HANDLE hDib, double scalex, double scaley);
            static HANDLE IncreaseBpp(HANDLE hDib, long newbpp);
            static HANDLE DecreaseBpp(HANDLE hDib, long newbpp);
            static HANDLE CropDIB(HANDLE handle, const FloatRect& ActualRect,const FloatRect& RequestedRect,int sourceunit,
                                  int destunit, int dpi, bool bConvertActual, int& retval);
            static HANDLE NegateDIB(HANDLE hDib);

            // Normalization of irregular DIBs
            static HANDLE NormalizeDib(HANDLE hDib);

            static double GetScaleFactorPerInch(LONG Unit);

        protected:
            void SetError(LONG nError) { m_lasterror = nError; }
            virtual bool OpenOutputFile(LPCTSTR pFileName);
            virtual bool CloseOutputFile();
            virtual void DestroyAllObjects() { }
            std::ofstream& GetOutputFileHandle() { return m_fStream.getStream(); }
            CTL_StringType GetOutputFileName() const { return m_fStream.getOutputFileName(); }
            static unsigned GetLine(BYTE *pDib, BYTE *pDest, int nWhichLine);
            // Lower level routines
            void resetbuffer() { bytesleft=0; }
            int      putbufferedbyte(WORD byte, std::ofstream& fh, bool bRealEOF=false, int *pStatus=NULL);

            static FloatRect Normalize(fipImage& pImage, const FloatRect& ActualRect, const FloatRect& RequestedRect,
                                int sourceunit, int destunit, int dpi);
            int      putbyte(WORD byte, std::ofstream& fh);

            static char masktable[8];
            static char bittable[8];
            static char bayerPattern[8][8];
            unsigned short int bytesleft,nextbyte;
            char        bytebuffer[BYTEBUFFERSIZE];
            DibMultiPageStruct m_MultiPageStruct;

        private:
            CDibInterfaceStream m_fStream;
            LONG m_lasterror;
            CTL_StringType m_sFileName;
    };
}
#endif

