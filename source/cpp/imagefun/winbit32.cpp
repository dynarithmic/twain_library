/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2019 Dynarithmic Software.

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
#include <math.h>
#include <sstream>
#include <map>
#include "winbit32.h"
#include "ctliface.h"
#include "ctltwmgr.h"
#include "dtwainc.h"
#include "ctlfileutils.h"
#include "FreeImagePlus.h"
#ifdef USE_NAMESPACES
    using namespace std;
#endif

#ifdef _MSC_VER
#pragma warning (disable:4244)
#pragma warning (disable:4018)
#pragma warning (disable:4310)
#pragma warning (disable:4309)
#pragma warning (disable:4244)
#endif

static int getNumOnBits(BYTE value);


//#define ISOURCE_INIT_STRING "{98F28E51-C24B-B1B4-9232-0080C8DA7A5E}"
#define GetAValue(rgb)      ((BYTE)((rgb)>>24))

using namespace dynarithmic;

char CDibInterface::masktable[]={(signed char)0x80,0x40,0x20,0x10,0x08,0x04,0x02,0x01};
char CDibInterface::bittable[]={0x01,0x02,0x04,0x08,0x10,0x20,0x40,(signed char)0x80};
char CDibInterface::bayerPattern[8][8] = {
      { 0,32, 8,40, 2,34,10,42,  },
      { 48,16,56,24,50,18,58,26, },
      { 12,44, 4,36,14,46, 6,38, },
      { 60,28,52,20,62,30,54,22, },
      {  3,35,11,43, 1,33, 9,41, },
      { 51,19,59,27,49,17,57,25, },
      { 15,47, 7,39,13,45, 5,37, },
      { 63,31,55,23,61,29,53,21  },
    };

CDibInterface::CDibInterface() : m_lasterror(0) {}

int  CDibInterface::WriteGraphicFile(CTL_ImageIOHandler* ptrHandler, LPCTSTR path, HANDLE handle, void *pUserInfo)
{
    UINT32 wid, ht;
    UINT32 bpp;
    LONG err;

    // Get a lock to the DIB
    BYTE *pImage = (BYTE *)ImageMemoryHandler::GlobalLock(handle);
    DTWAINGlobalHandle_RAII dibHandle(handle);
    if ( !pImage )
        return IS_ERR_INVALIDBMP;

    // Get the BPP
    GetBitsPerPixel(pImage, &bpp);

    // Get Width
    GetWidth(pImage, &wid);

    // Get Height
    GetHeight(pImage, &ht);

    BYTE *pImage2 = pImage;

    // Open a file destination
    OpenOutputFile(path);

    // Check if any error occurred
    err = GetLastError();
    if ( err == IS_ERR_OK)
    {
        int nUsedColors = CalculateUsedPaletteEntries(bpp);
        LPBITMAPINFOHEADER bi = (LPBITMAPINFOHEADER)pImage2;
        bi->biClrUsed = nUsedColors;
        CTL_StringType szNum;
        CTL_StringStreamType strm;
        strm << nUsedColors;
        CTL_TwainAppMgr::WriteLogInfo(CTL_StringType(_T("Image has ")) + strm.str() + _T(" colors\n"));

        err = WriteImage(ptrHandler,
                         pImage2,
                         wid,
                         ht,
                         bpp,
                         nUsedColors,
                         GetPalettePtr(pImage2, nUsedColors),
                         pUserInfo);
        bool goodClose = CloseOutputFile();
        if ( err != 0 || !goodClose)
        {
            if (!goodClose)
                err = DTWAIN_ERR_FILEWRITE;
            err = DTWAIN_ERR_FILEXFERSTART - (int)err;
            delete_file(path);
        }
    }
    return err;
}

RGBQUAD* CDibInterface::GetPalettePtr(BYTE *pDibData, int bpp)
{
  if ( pDibData && bpp < 16)
  {
      BYTE *pPalette = (pDibData + sizeof(BITMAPINFOHEADER));
      return (RGBQUAD *)pPalette;
  }
  return NULL;
}

unsigned char * CDibInterface::GetScanLine(BYTE *pDib, int scanline)
{
    if ( pDib )
    {
        return CalculateScanLine(GetDibBits(pDib), GetPitch(pDib), scanline);
    }
    return NULL;
}

unsigned CDibInterface::GetLine(BYTE *pDib)
{
    if ( pDib )
    {
        UINT32 width;
        UINT32 bpp;
        GetWidth(pDib, &width);
        GetBitsPerPixel(pDib, &bpp);
        return ((width * bpp) + 7) / 8;
    }
    return 0;
}

unsigned CDibInterface::GetLine(BYTE *pDib, BYTE *pDest, int nWhichLine)
{
    LPBITMAPINFOHEADER bi = (LPBITMAPINFOHEADER)pDib;
    UINT32 ht;
    GetHeight(pDib, &ht);
    int nLineWidth = LPBlinewidth(bi);

    memcpy(pDest, GetDibBits(pDib) + nLineWidth * (ht - nWhichLine -1), nLineWidth);
    return nLineWidth;
}

unsigned CDibInterface::GetPitch(BYTE *pDib)
{
    if ( pDib )
        return (GetLine(pDib) + 3) & ~3;
    return 0;
}

unsigned CDibInterface::GetPitch(fipImage& pDib)
{
    return (pDib.getScanWidth() + 3) & ~3;
}

BYTE * CDibInterface::GetDibBits(BYTE *pDib)
{
    if ( pDib )
    {
        LPBITMAPINFOHEADER pBi = (LPBITMAPINFOHEADER)pDib;
        int nColors = pBi->biClrUsed;
        BYTE *p = (BYTE *)pBi;
        return p + sizeof(BITMAPINFOHEADER) + sizeof(RGBQUAD) * nColors;
    }
    return NULL;
}

int CDibInterface::GetDibPalette(fipImage& lpbi,LPSTR palette)
{
    const BITMAPINFO* header = lpbi.getInfo();
    unsigned int i, j;
    j = (std::min)(1 << header->bmiHeader.biBitCount, 256);

    for (i = 0; i < j; i++)
    {
        palette[i*RGB_SIZE + RGB_RED] = header->bmiColors[i].rgbRed;
        palette[i*RGB_SIZE + RGB_GREEN] = header->bmiColors[i].rgbGreen;
        palette[i*RGB_SIZE + RGB_BLUE] = header->bmiColors[i].rgbBlue;
    }

    return(j);
}

// Function to ensure that DIB data is on DWORD boundaries
HANDLE CDibInterface::NormalizeDib(HANDLE hDib)
{
    if (!hDib)
        return NULL;
    HANDLE hNewDib = NULL;
    BYTE *pImage = (BYTE*)ImageMemoryHandler::GlobalLock(hDib);
    DTWAINGlobalHandle_RAII dibHandle(hDib);

    UINT32 width, height, bpp;
    GetWidth(pImage, &width);
    GetHeight(pImage, &height);
    GetBitsPerPixel( pImage, &bpp );


    // Create another DIB based on this DIB's data
    hNewDib = CreateDIB(width, height, bpp, (LPSTR)GetPalettePtr(pImage, bpp));

    if (!hNewDib)
        return hDib;

    BYTE *pNewImage = (BYTE *)ImageMemoryHandler::GlobalLock(hNewDib);
    DTWAINGlobalHandle_RAII dibHandle2(hNewDib);

    // Copy bits from old to new

    // Compute the stride for the old bitmap
    LONG OldStride = CalculateLine(width, bpp);

    // This is always DWORD aligned
    LONG NewStride = CalculateEffWidth(width,bpp);

    // Point to the DIB data
    pImage = GetDibBits(pImage);
    pNewImage = GetDibBits(pNewImage);

    // Loop through the data, copying from old to new
    for ( UINT32 i = 0; i < height; ++i)
    {
        memcpy(pNewImage, pImage, OldStride);
        pImage += OldStride;
        pNewImage += NewStride;
    }
    ImageMemoryHandler::GlobalFree(hDib);
    return hNewDib;
}

HANDLE CDibInterface::CreateDIB(int width, int height, int bpp, LPSTR palette/*=NULL*/)
{
    height = abs(height);

    int dib_size = sizeof(BITMAPINFOHEADER);
    dib_size += sizeof(RGBQUAD) * CalculateUsedPaletteEntries(bpp);
    dib_size += CalculateEffWidth(width, bpp) * height;

    HANDLE hDib = ImageMemoryHandler::GlobalAlloc (GHND | GMEM_ZEROINIT, dib_size);

    if (hDib != NULL)
    {
        LPBITMAPINFOHEADER bih = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock (hDib);
        DTWAINGlobalHandle_RAII dibHandle(hDib);

        // write out the BITMAPINFOHEADER
        bih->biSize             = sizeof(BITMAPINFOHEADER);
        bih->biWidth            = width;
        bih->biHeight           = height;
        bih->biPlanes           = 1;
        bih->biCompression      = 0;
        bih->biBitCount         = (WORD)bpp;
        bih->biClrUsed          = CalculateUsedPaletteEntries(bpp);
        bih->biClrImportant     = bih->biClrUsed;

        if(palette != NULL)
        {
            RGBQUAD FAR *pRgb;
            pRgb = (RGBQUAD FAR *)((LPSTR)bih + (unsigned int)bih->biSize);

            for(DWORD i=0;i<bih->biClrUsed;++i)
            {
                pRgb[i].rgbRed=(char)palette[i*RGB_SIZE+RGB_RED];
                pRgb[i].rgbGreen=(char)palette[i*RGB_SIZE+RGB_GREEN];
                pRgb[i].rgbBlue=(char)palette[i*RGB_SIZE+RGB_BLUE];
                pRgb[i].rgbReserved=0;
            }
        }
        return hDib;
    }
    return NULL;
}

HANDLE CDibInterface::NegateDIB(HANDLE hDib)
{
    fipImage fw;
    if ( !fipImageUtility::copyFromHandle(fw, hDib) )
        return NULL;
    fipWinImage_RAII raii(&fw);
    if ( fw.invert() )
        return NULL;
    return fipImageUtility::copyToHandle(fw);
}

HANDLE CDibInterface::ResampleDIB(HANDLE hDib, long newx, long newy)
{
    fipImage fw;
    if (!fipImageUtility::copyFromHandle(fw, hDib))
        return NULL;
    fipWinImage_RAII raii(&fw);
    fw.rescale(newx, newy, FILTER_BSPLINE);
    return fipImageUtility::copyToHandle(fw);
}

HANDLE CDibInterface::ResampleDIB(HANDLE hDib, double xscale, double yscale)
{
    BYTE *pImage = (BYTE *)ImageMemoryHandler::GlobalLock(hDib);
    DTWAINGlobalHandle_RAII dibHandler(hDib);
    UINT32 wid, ht;

    // Get Width
    GetWidth(pImage, &wid);

    // Get Height
    GetHeight(pImage, &ht);

    long newx, newy;
    newx = static_cast<long>(xscale * (double)wid);
    newy = static_cast<long>(yscale * (double)ht);
    return ResampleDIB(hDib, newx, newy);
}

HANDLE CDibInterface::IncreaseBpp(HANDLE hDib, long newbpp)
{
    fipImage fw;
    if (!fipImageUtility::copyFromHandle(fw, hDib))
        return NULL;
    fipWinImage_RAII raii(&fw);
    switch (newbpp)
    {
        case 4:
            fw.convertTo4Bits();
        break;
        case 8:
            fw.convertTo8Bits();
        break;
        case 16:
            fw.convertTo16Bits565();
        break;
        case 24:
            fw.convertTo24Bits();
        break;
        case 32:
            fw.convertTo32Bits();
        break;
        default:
            return NULL;
    }
    return fipImageUtility::copyToHandle(fw);
}

HANDLE CDibInterface::DecreaseBpp(HANDLE hDib, long newbpp)
{
    return IncreaseBpp(hDib, newbpp);
}

HANDLE CDibInterface::CropDIB(HANDLE handle, const FloatRect& ActualRect, const FloatRect& RequestedRect,int sourceunit,
                              int destunit, int dpi, bool bConvertActual, int& retval)
{
    retval = IS_ERR_OK;

    fipImage fw;
    if (!fipImageUtility::copyFromHandle(fw, handle))
        return NULL;
    fipWinImage_RAII raii(&fw);

    UINT32 width = fw.getWidth();
    UINT32 height = fw.getHeight();

    // Convert the actual rectangle first if necessary
    // This assumes that the actual rect is in pixels, but
    // the source unit does not match up correctly
    FloatRect TempActual = ActualRect;
    if ( bConvertActual )
        TempActual = Normalize(fw, ActualRect, ActualRect, DTWAIN_PIXELS, sourceunit, dpi);

    // Now return a normalized rectangle from the actual and requested rectangles
    FloatRect NormalizedRect = Normalize(fw, TempActual, RequestedRect, sourceunit, destunit, dpi);

    double left, top, right, bottom;

    left = NormalizedRect.left;
    top = NormalizedRect.top;
    right = NormalizedRect.right;
    bottom = NormalizedRect.bottom;

    // DIBs are stored upside down, so adjust coordinates here
    int newbottom = height - (UINT32)top;
    int newtop = height - (UINT32)bottom;

    long startx = max(0L,min((long)left, (long)width));
    long endx = max(0L,min((long)right, (long)width));

    long starty = max(0L,min((long)newtop, (long)height));
    long endy =   max(0L,min((long)newbottom, (long)height));

    if (startx==endx || starty==endy)
    {
        retval = IS_ERR_BADPARAM;
        return NULL;
    }

    if (startx>endx)
    {
        long tmp=startx;
        startx=endx;
        endx=tmp;
    }
    if (starty>endy)
    {
        long tmp=starty;
        starty=endy;
        endy=tmp;
    }

    if ( fw.crop(startx, starty, endx, endy) )
        return fipImageUtility::copyToHandle(fw);
    return NULL;
}

FloatRect CDibInterface::Normalize(fipImage& pImage, const FloatRect& ActualRect, const FloatRect& RequestedRect,
                                   int sourceunit, int destunit, int dpi)
{
    // These units are always in pixels
    UINT32 width, height;
    map<LONG, double, less<LONG> > Measurement = {{DTWAIN_INCHES, 1.0},
                                                  {DTWAIN_TWIPS, 1440.0},
                                                  {DTWAIN_POINTS, 72.0},
                                                  {DTWAIN_PICAS, 6.0},
                                                  {DTWAIN_CENTIMETERS, 2.54}};

    width = pImage.getWidth();
    height = pImage.getHeight();

    // Set up a return rect
    FloatRect fRect = RequestedRect;

    // Check dimensions
    if (fabs(ActualRect.right - ActualRect.left) < 1.0 )
        return fRect;

    UINT32 pitch = GetPitch(pImage);
    if ( pitch == 0 )
        return fRect;

    // Convert Actual rect to pixels
    double PixelsPerInch = dpi;
    switch(sourceunit)
    {
        case DTWAIN_PIXELS:
            break;

        case DTWAIN_INCHES:
        case DTWAIN_TWIPS:
        case DTWAIN_CENTIMETERS:
        case DTWAIN_POINTS:
        case DTWAIN_PICAS:
        {
            double NumInches = (ActualRect.right - ActualRect.left) / Measurement[sourceunit];
            PixelsPerInch = (double)width / NumInches;
        }
        break;
    }

    switch(destunit)
    {
        case DTWAIN_PIXELS:
            break;

        case DTWAIN_INCHES:
        case DTWAIN_TWIPS:
        case DTWAIN_CENTIMETERS:
        case DTWAIN_POINTS:
        case DTWAIN_PICAS:
        {
            if ( sourceunit == DTWAIN_PIXELS )
            {
                fRect.left   = RequestedRect.left / PixelsPerInch;
                fRect.right  = RequestedRect.right  /  PixelsPerInch;
                fRect.top    = RequestedRect.top   /  PixelsPerInch;
                fRect.bottom = RequestedRect.bottom  / PixelsPerInch;
            }
            else
            {
                fRect.left   = (RequestedRect.left / Measurement[destunit]) * PixelsPerInch;
                fRect.right  = (RequestedRect.right  / Measurement[destunit]) * PixelsPerInch;
                fRect.top    = (RequestedRect.top   / Measurement[destunit])  * PixelsPerInch;
                fRect.bottom = (RequestedRect.bottom  / Measurement[destunit])* PixelsPerInch;
            }
        }
        break;

    }
    return fRect;
}

int getNumOnBits(BYTE value)
{
    return (value & 0x01) +
           ((value & 0x02) >> 1) +
           ((value & 0x04) >> 2) +
           ((value & 0x08) >> 3) +
           ((value & 0x10) >> 4) +
           ((value & 0x20) >> 5) +
           ((value & 0x40) >> 6) +
           ((value & 0x80) >> 7);
}

// Test for blank page here
bool CDibInterface::IsBlankDIB(HANDLE hDib, double threshold)
{
    fipImage fw;
    if (!fipImageUtility::copyFromHandle(fw, hDib))
        return NULL;
    fipWinImage_RAII raii(&fw);

    bool retval = false;
    if ( hDib )
    {
        // do a simple test here
        UINT32 width = fw.getWidth();
        UINT32 height = fw.getHeight();

        // loop for all pixels
        RGBQUAD rgb;
        UINT32 numwhite = 0;
        UINT32 totalcomponents = 0;
        for (UINT32 row = 0; row < height; ++row)
        {
            for (UINT32 col = 0; col < width; ++col)
            {
                fw.getPixelColor(col, row, &rgb);
                numwhite += getNumOnBits(rgb.rgbRed) + getNumOnBits(rgb.rgbGreen) + getNumOnBits(rgb.rgbBlue);
                totalcomponents += 24;
            }
        }

        double pctwhite = (double)numwhite/(double)totalcomponents;
        if ( pctwhite >= threshold )
            retval = true;
    }
    return retval;
}

LPSTR CDibInterface::GetMonoPalette(LPSTR palette)
{
    static char pal[]="\000\000\000\377\377\377";
    if(palette != NULL)
        memcpy(palette,pal,6);
    return((LPSTR)pal);
}

////////////////////////////////////////////////////////////////////////

bool CDibInterface::GetWidth(BYTE *pDIB, UINT32 *puWidth)
{
    if (pDIB==NULL)
    {
        *puWidth = 0;
        return false;
    }

    if (((BITMAPINFOHEADER *)pDIB)->biSize!=sizeof(BITMAPINFOHEADER))
    {
        *puWidth = 0;
        return false;
    }

    *puWidth = ((BITMAPINFOHEADER *)pDIB)->biWidth;
    return true;
}

////////////////////////////////////////////////////////////////////////

bool CDibInterface::GetHeight(BYTE *pDIB, UINT32 *piHeight)
{
    if (pDIB==NULL)
    {
        *piHeight = 0;
        return false;
    }

    if (((BITMAPINFOHEADER *)pDIB)->biSize!=sizeof(BITMAPINFOHEADER))
    {
        *piHeight = 0;
        return false;
    }

    *piHeight = ((BITMAPINFOHEADER *)pDIB)->biHeight;
    return true;
}

////////////////////////////////////////////////////////////////////////

bool CDibInterface::GetBitsPerPixel(BYTE *pDIB, UINT32 *puBitCount)
{
    if (pDIB==NULL)
    {
        *puBitCount = 0;
        return false;
    }

    if (((BITMAPINFOHEADER *)pDIB)->biSize!=sizeof(BITMAPINFOHEADER))
    {
        *puBitCount = 0;
        return false;
    }

    *puBitCount = ((BITMAPINFOHEADER *)pDIB)->biBitCount;
    return true;
}

bool CDibInterface::OpenOutputFile(LPCTSTR pFileName)
{
    if (pFileName==NULL)
    {
        SetError(DTWAIN_ERR_FILEOPEN);
        return false;
    }

    if (pFileName[0] == 0)
    {
        SetError(DTWAIN_ERR_FILEOPEN);
        return false;
    }

    auto result = m_fStream.OpenOutputFile(pFileName);
    if (!result)
    {
        SetError(DTWAIN_ERR_FILEOPEN);
        return false;
    }
    SetError(0);
    return true;
}

bool CDibInterface::CloseOutputFile()
{
    auto retVal = m_fStream.CloseOutputFile();
    if ( retVal )
    {
        SetError(0);
        return true;
    }
    SetError(DTWAIN_ERR_FILEWRITE);
    return false;
}

bool CDibInterface::IsGrayScale(BYTE *pImage, int bpp)
{
#ifdef _WIN32
    LPBITMAPINFOHEADER pHeader = (LPBITMAPINFOHEADER)pImage;
    RGBQUAD* ppal=GetPalettePtr(pImage, bpp);
    if(!ppal || pHeader->biClrUsed == 0 )
        return false;

    for(DWORD i=0; i<pHeader->biClrUsed;i++)
    {
        if (ppal[i].rgbBlue!=i || ppal[i].rgbGreen!=i || ppal[i].rgbRed!=i)
            return false;
    }
    return true;
#else
    return false;
#endif
}

bool CDibInterface::IsGrayScale(HANDLE hDib, int bpp)
{
#ifdef _WIN32
    bool bRetval = false;
    BYTE *pImageTemp = (BYTE *)GlobalLock(hDib);
    DTWAINGlobalHandle_RAII dibHandle(hDib);
    if ( pImageTemp )
        bRetval = IsGrayScale(pImageTemp, bpp);
    return bRetval;
#else
    fipImage fw;
    if (fipImageUtility::copyFromHandle(fw, hDib))
        return false;
    fipWinImage_RAII raii(&fw);
    return fw.isGrayscale();
#endif
}

int CDibInterface::putbufferedbyte(WORD byte, std::ofstream& fh, bool bRealEOF, int *pStatus/*=NULL*/)
{
    if ( pStatus )
        *pStatus = 0;
    if(byte==(WORD)EOF && bRealEOF)
    {
        fh.write(reinterpret_cast<char*>(bytebuffer), bytesleft);
        bytesleft=0;
        return(byte);
    }
    else
    {
        if(bytesleft >= BYTEBUFFERSIZE)
        {
            fh.write(reinterpret_cast<char*>(bytebuffer), bytesleft);
            bytesleft=0;
        }
        bytebuffer[bytesleft++]=(char)byte;
        return(byte);
    }
}

int CDibInterface::putbyte(WORD byte, std::ofstream& fh)
{
    fh.write(reinterpret_cast<char*>(&byte), 1);
    return(byte);
}

double CDibInterface::GetScaleFactorPerInch(LONG Unit)
{
    switch( Unit )
    {
        case DTWAIN_TWIPS:
            return 1440.0;
        case DTWAIN_POINTS:
            return 72.0;
        case DTWAIN_PICAS:
            return 6.0;
        case DTWAIN_CENTIMETERS:
            return 2.54;
    }
    return 1.0;
}
