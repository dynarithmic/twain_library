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

For more information, the license file named LICENSE that is located in the root
directory of the DTWAIN installation covers the restrictions under the LGPL license.
Please read this file before deploying or distributing any application using DTWAIN.
*/
#undef __STRICT_ANSI__
#include <tchar.h>
#include <vector>
#include <algorithm>
#include "jpglib32.h"
#include "ctlobstr.h"
#include "setjmp.h"
#define IDS_DTWAIN_APPTITLE       9700
#define WIDTHBYTES(i)       ((i+31)/32*4)
#define PIXELS2BYTES(n)     ((n+7)/8)
#define GREYVALUE(r,g,b)    ((((r*30)/100) + ((g*59)/100) + ((b*11)/100)))
#define ORTHOMATCH(r,g,b)   (((r) & 0x00e0) | (((g) >> 3) & 0x001c)  | (((b) >> 6) & 0x0003))
#define RGB_RED         0
#define RGB_GREEN       1
#define RGB_BLUE        2
#define RGB_SIZE        3


#define LPBlinewidth(lpbi) (WIDTHBYTES((WORD)lpbi->biWidth*lpbi->biBitCount))
#define LPBcolourmap(lpbi) (LPRGBQUAD)((LPSTR)lpbi+lpbi->biSize)
#define LPBimage(lpbi)  ((HPSTR)lpbi+lpbi->biSize+(long)(lpbi->biClrUsed*sizeof(RGBQUAD)))
#define GetChunkyPixel(pxx,nxx) (!((nxx) & 1)) ? (((pxx)[(nxx)>>1] >> 4) & 0x0f) : ((pxx)[(nxx)>>1] & 0x0f)
#define PutChunkyPixel(pxx,nxx,cxx) (!(nxx & 1)) ? \
     (pxx[nxx>>1] &= 0x0f, pxx[nxx>>1] |= (char)((cxx & 0x0f) << 4)) : \
     (pxx[nxx>>1] &= 0xf0, pxx[nxx>>1] |= (char)(cxx & 0x0f))

#define FixedGlobalAlloc(n)     (char *)malloc(n)
#define FixedGlobalFree(p)      free(p)
#define FixedGlobalRealloc(p,n) realloc(p,n)
typedef char* HPSTR;

static int GetDibPalette(LPBITMAPINFO lpbi,LPSTR palette);
static bool IsGrayScale(BYTE *pImage, int bpp);
static BYTE * GetDibBits(BYTE *pDib);
static void RGB2BGR(BYTE *buffer, int length, int nColors);
static RGBQUAD* GetPalettePtr(BYTE *pDibData, int bpp);

char masktable[8]={(char )0x80,(char )0x40,(char )0x20,
                                  (char )0x10,(char )0x08,(char )0x04,
                                  (char )0x02,(char )0x01};

/*char bittable[8]={(char )0x01,(char )0x02,(char )0x04,
                                 (char )0x08,(char )0x10,(char )0x20,
                                 (char )0x40,(char )0x80};
char bayerPattern[8][8] = {
      { 0,32, 8,40, 2,34,10,42,  },
      { 48,16,56,24,50,18,58,26, },
      { 12,44, 4,36,14,46, 6,38, },
      { 60,28,52,20,62,30,54,22, },
      {  3,35,11,43, 1,33, 9,41, },
      { 51,19,59,27,49,17,57,25, },
      { 15,47, 7,39,13,45, 5,37, },
      { 63,31,55,23,61,29,53,21  },
    };
*/

void my_error_exit (j_common_ptr cinfo);

typedef struct my_error_mgr * my_error_ptr;

struct my_error_mgr {
    struct jpeg_error_mgr pub;  /* "public" fields */
    jmp_buf setjmp_buffer;  /* for return to caller */
};

typedef struct
{
    jpeg_compress_struct cinfo;
    my_error_mgr jerr;
} IDCJIS;

LONG FUNCCONVENTION DTWLIB_JPEGWriteFile(LPCTSTR szFile,
                                       BYTE *pImage, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 /*nColors*/, RGBQUAD * /*pPal*/,
                                       LONG Quality,
                                       LONG UnitOfMeasure, double ScaleFactor,
                                       double ResX, double ResY,
                                       bool bProgressive,
                                       LPCTSTR szAppInfo)
{
    BYTE *pImageToUse = pImage;
    FILE *pTest = fopen(dynarithmic::StringConversion::Convert_Native_To_Ansi(szFile).c_str(), "wb");
    if ( !pTest )
        return DTWAIN_ERR_FILEWRITE;

    struct jpeg_error_mgr jerr;
    struct jpeg_compress_struct cinfo;
    JSAMPROW row_pointer[1];
    LPSTR ps;
    std::vector<CHAR> linebuffer, extrabuffer;
    BYTE *pd;
    char palette[768];
    int c;
    UINT i;

    int nLineWidth = LPBlinewidth(((LPBITMAPINFOHEADER)pImageToUse));
    linebuffer.resize(nLineWidth + 1024);
    extrabuffer.resize(wid * RGB_SIZE + 1024);
    if(bpp <= 8)
        GetDibPalette((LPBITMAPINFO)pImageToUse,palette);

    cinfo.err = jpeg_std_error(&jerr);

    jpeg_create_compress(&cinfo);

    jpeg_stdio_dest(&cinfo,pTest);

    cinfo.image_width = wid;
    cinfo.image_height= ht;

    // Set the resolution
    cinfo.density_unit = 1; // dots / in
    double factor = ScaleFactor;
    switch(UnitOfMeasure)
    {
        case DTWAIN_CENTIMETERS:
            cinfo.density_unit = 2; // dots / cm
            factor = 1.0;
        break;
    }
    cinfo.X_density = (UINT16)(ResX * factor); 
    cinfo.Y_density = (UINT16)(ResY * factor); 

    if (IsGrayScale( pImageToUse, bpp))
    {
        cinfo.input_components = 1;       /* # of color components per pixel */
        cinfo.in_color_space = JCS_GRAYSCALE;     /* colorspace of input image */
    }
    else
    {
        cinfo.input_components = 3;
        cinfo.in_color_space=JCS_RGB;
    }

    jpeg_set_defaults(&cinfo);
    jpeg_set_quality(&cinfo, Quality, TRUE);

    if (bProgressive)
        jpeg_simple_progression(&cinfo);

    jpeg_start_compress(&cinfo,TRUE);

    jpeg_write_marker(&cinfo, JPEG_COM, (const JOCTET*)szAppInfo, _tcslen(szAppInfo));

    pd = GetDibBits(pImageToUse) + nLineWidth * (DWORD)ht;
    while(cinfo.next_scanline < cinfo.image_height)
    {
        pd -= nLineWidth;
        memcpy(&linebuffer[0],pd,nLineWidth);
        switch(bpp)
        {
            case 1:
                for(i=0; i< wid;++i)
                {
                    if(linebuffer[i >> 3] & masktable[i & 0x0007])
                    {
                        extrabuffer[i*RGB_SIZE+RGB_RED]=(unsigned char)255;
                        extrabuffer[i*RGB_SIZE+RGB_GREEN]=(unsigned char)255;
                        extrabuffer[i*RGB_SIZE+RGB_BLUE]=(unsigned char)255;
                    }
                    else
                    {
                        extrabuffer[i*RGB_SIZE+RGB_RED]=0;
                        extrabuffer[i*RGB_SIZE+RGB_GREEN]=0;
                        extrabuffer[i*RGB_SIZE+RGB_BLUE]=0;
                    }
                }
            break;

            case 4:
                for(i=0; i<wid; ++i)
                {
                    c = GetChunkyPixel(&linebuffer[0],i);
                    ps=palette+c*RGB_SIZE;
                    extrabuffer[i*RGB_SIZE+RGB_BLUE]=*ps++;
                    extrabuffer[i*RGB_SIZE+RGB_GREEN]=*ps++;
                    extrabuffer[i*RGB_SIZE+RGB_RED]=*ps++;
                }
            break;

            case 8:
                memcpy(&extrabuffer[0],&linebuffer[0],nLineWidth);
                break;

            case 24:
                memcpy(&extrabuffer[0],&linebuffer[0],nLineWidth);
                RGB2BGR((BYTE *)&extrabuffer[0], nLineWidth, 0);
            break;
        }
        row_pointer[0] = (JSAMPROW)&extrabuffer[0];
        jpeg_write_scanlines(&cinfo,row_pointer,1);
    }

    jpeg_finish_compress(&cinfo);
    jpeg_destroy_compress(&cinfo);

    fclose(pTest);
    return(0); // OK

}

int GetDibPalette(LPBITMAPINFO lpbi,LPSTR palette)
{
    unsigned int i,j;

    j=(std::min)(1<<lpbi->bmiHeader.biBitCount,256);

    for(i=0;i<j;i++)
    {
        palette[i*RGB_SIZE+RGB_RED] = lpbi->bmiColors[i].rgbRed;
        palette[i*RGB_SIZE+RGB_GREEN] = lpbi->bmiColors[i].rgbGreen;
        palette[i*RGB_SIZE+RGB_BLUE] = lpbi->bmiColors[i].rgbBlue;
    }

    return(j);
}

bool IsGrayScale(BYTE *pImage, int bpp)
{
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
}

BYTE * GetDibBits(BYTE *pDib)
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

void RGB2BGR(BYTE *buffer, int length, int nColors)
{
	if (buffer &&  nColors ==0)
    {
		BYTE temp;
		for (int i=0;i<length;i+=3)
        {
			temp = buffer[i]; buffer[i] = buffer[i+2]; buffer[i+2] = temp;
		}
	}
}

RGBQUAD* GetPalettePtr(BYTE *pDibData, int bpp)
{
  if ( pDibData && bpp < 16)
  {
      BYTE *pPalette = (pDibData + sizeof(BITMAPINFOHEADER));
      return (RGBQUAD *)pPalette;
  }
  return NULL;
}
