#include <png.h>
#include <pngconf.h>
#include <pnglib32.h>
#include <tchar.h>
#include <stdio.h>
#include <fstream>

LONG FUNCCONVENTION DTWLIB_PNGWriteFile(LPCTSTR szFile, BYTE *pImage2, UINT32 wid, UINT32 ht,
    UINT32 bpp, UINT32 /*nColors*/, RGBQUAD * /*pPal*/)
{
    return 1;
}
#if 0

#include "ctlfileutils.h"
#include "ctlobstr.h"
#ifdef _MSC_VER
#pragma warning (disable: 4100 4505 4702)
#endif
enum {
    FIC_MINISWHITE = 0,             // min value is white
    FIC_MINISBLACK = 1,             // min value is black
    FIC_RGB        = 2,             // RGB color model
    FIC_PALETTE    = 3,             // color map indexed
    FIC_RGBALPHA   = 4,             // RGB color model with alpha channel
};

static std::ofstream pFile;

static int GetColorType(BYTE *pImage);
static RGBQUAD* GetPalettePtr(BYTE *pDibData, int bpp);
static void ConvertLine32To24(BYTE *target, BYTE *source, int width_in_pixels, RGBQUAD *pPalette);
static unsigned char * GetScanLine(BYTE *pDib, int scanline);
static bool GetBitsPerPixel(BYTE *pDIB, UINT32 *puBitCount);
static int  GetColorsUsed(BYTE *pImage);
static bool GetHeight(BYTE *pDIB, UINT32 *piHeight);
static bool GetWidth(BYTE *pDIB, UINT32 *puWidth);
static unsigned GetPitch(BYTE *pDib);
static unsigned GetLine(BYTE *pDib);
static unsigned char * CalculateScanLine(unsigned char *bits, unsigned pitch, int scanline);

static void _WriteProc(png_structp, png_bytep data, png_size_t size)
{
    pFile.write(reinterpret_cast<char*>(data), size);
//    fwrite(data, size, 1, pFile);
}

static void _ReadProc(struct png_struct_def *, unsigned char * /*data*/, unsigned int /*size*/)
{
  //  fread(data, size, 1, pFile);
}

static void _FlushProc(png_structp /*png_ptr*/)
{
    // empty flush implementation
}


static void
error_handler(struct png_struct_def *, const char *error)
{
    throw error;
}

static void
warning_handler(struct png_struct_def *, const char * /*warning*/)
{
}

LONG FUNCCONVENTION DTWLIB_PNGWriteFile(LPCTSTR szFile, BYTE *pImage2, UINT32 wid, UINT32 ht,
                                       UINT32 bpp, UINT32 /*nColors*/, RGBQUAD * /*pPal*/)
{
    pFile.open(dynarithmic::StringConversion::Convert_Native_To_Ansi(szFile), std::ios::binary);
    if ( !pFile )
        return DTWAIN_ERR_FILEWRITE;

    png_structp png_ptr;
    png_infop info_ptr;
    png_colorp palette = NULL;
    png_uint_32 width, height;
    bool has_alpha_channel = false;

    RGBQUAD *pal;   // pointer to dib palette
    int bit_depth;
    int palette_entries;
    int interlace_type;

    try
    {
        // create the chunk manage structure

        png_ptr = png_create_write_struct(PNG_LIBPNG_VER_STRING,
                                          (png_voidp)error_handler, error_handler, warning_handler);

        if (!png_ptr)  {
            pFile.close(); // fclose(pFile);
            dynarithmic::delete_file(szFile);
            return FALSE;
        }

        // Allocate/initialize the image information data.

        info_ptr = png_create_info_struct(png_ptr);

        if (!info_ptr)  {
            png_destroy_write_struct(&png_ptr,  (png_infopp)NULL);
            pFile.close(); // fclose(pFile);
            dynarithmic::delete_file(szFile);
            return DTWAIN_ERR_FILEWRITE;
        }

        // Set error handling.  REQUIRED if you aren't supplying your own
        // error handling functions in the png_create_write_struct() call.

        // init the IO

        png_set_write_fn(png_ptr, info_ptr, _WriteProc, _FlushProc);

        // DIBs *do* support physical resolution

        BITMAPINFOHEADER *bih = (LPBITMAPINFOHEADER)pImage2;
        png_uint_32 res_x = bih->biXPelsPerMeter;
        png_uint_32 res_y = bih->biYPelsPerMeter;

        if ((res_x > 0) && (res_y > 0))  {
            png_set_pHYs(png_ptr, info_ptr, res_x, res_y, 1);
        }

        // Set the image information here.  Width and height are up to 2^31,
        // bit_depth is one of 1, 2, 4, 8, or 16, but valid values also depend on
        // the color_type selected. color_type is one of PNG_COLOR_TYPE_GRAY,
        // PNG_COLOR_TYPE_GRAY_ALPHA, PNG_COLOR_TYPE_PALETTE, PNG_COLOR_TYPE_RGB,
        // or PNG_COLOR_TYPE_RGB_ALPHA.  interlace is either PNG_INTERLACE_NONE or
        // PNG_INTERLACE_ADAM7, and the compression_type and filter_type MUST
        // currently be PNG_COMPRESSION_TYPE_BASE and PNG_FILTER_TYPE_BASE. REQUIRED

        width = wid;
        height = ht;
        bit_depth = bpp;

        if (bit_depth == 16) {
            png_destroy_write_struct(&png_ptr, &info_ptr);
            pFile.close(); // fclose(pFile);
            dynarithmic::delete_file(szFile);

            throw "Format not supported";   // Note: this could be enhanced here...
        }

        bit_depth = (bit_depth > 8) ? 8 : bit_depth;

        interlace_type = PNG_INTERLACE_NONE;    // Default value

        switch (GetColorType(pImage2))
        {
            case FIC_MINISWHITE:
                // Invert monochrome files to have 0 as black and 1 as white
                // (no break here)
                png_set_invert_mono(png_ptr);

            case FIC_MINISBLACK:
                png_set_IHDR(png_ptr, info_ptr, width, height, bit_depth,
                    PNG_COLOR_TYPE_GRAY, interlace_type,
                    PNG_COMPRESSION_TYPE_BASE, PNG_FILTER_TYPE_BASE);
                break;

            case FIC_PALETTE:
            {
                png_set_IHDR(png_ptr, info_ptr, width, height, bit_depth,
                    PNG_COLOR_TYPE_PALETTE, interlace_type,
                    PNG_COMPRESSION_TYPE_BASE, PNG_FILTER_TYPE_BASE);

                // set the palette

                palette_entries = 1 << bit_depth;
                palette = (png_colorp)png_malloc(png_ptr, palette_entries * sizeof (png_color));
                pal = GetPalettePtr(pImage2, bpp);

                for (int i = 0; i < palette_entries; i++) {
                    palette[i].red   = pal[i].rgbRed;
                    palette[i].green = pal[i].rgbGreen;
                    palette[i].blue  = pal[i].rgbBlue;
                }

                png_set_PLTE(png_ptr, info_ptr, palette, palette_entries);

                // You must not free palette here, because png_set_PLTE only makes a link to
                // the palette that you malloced.  Wait until you are about to destroy
                // the png structure.

                break;
            }

/*            case FIC_RGBALPHA :
                if (freeimage->is_transparent_proc(dib)) {
                    has_alpha_channel = TRUE;

                    png_set_IHDR(png_ptr, info_ptr, width, height, bit_depth,
                        PNG_COLOR_TYPE_RGBA, interlace_type,
                        PNG_COMPRESSION_TYPE_BASE, PNG_FILTER_TYPE_BASE);

                    png_set_bgr(png_ptr); // flip BGR pixels to RGB
                    break;
                }
*/
                // intentionally no break here...

            case FIC_RGB:
                png_set_IHDR(png_ptr, info_ptr, width, height, bit_depth,
                    PNG_COLOR_TYPE_RGB, interlace_type,
                    PNG_COMPRESSION_TYPE_BASE, PNG_FILTER_TYPE_BASE);

                png_set_bgr(png_ptr); // flip BGR pixels to RGB
                break;
        }

        // Optional gamma chunk is strongly suggested if you have any guess
        // as to the correct gamma of the image.
        // png_set_gAMA(png_ptr, info_ptr, gamma);

/*        if ( bpp == 8) && (freeimage->is_transparent_proc(dib)) && (freeimage->get_transparency_count_proc(dib) > 0))
            png_set_tRNS(png_ptr, info_ptr, freeimage->get_transparency_table_proc(dib), freeimage->get_transparency_count_proc(dib), NULL);
  */
        // Write the file header information.

        png_write_info(png_ptr, info_ptr);

        // write out the image data

        if ((bpp == 32) && (!has_alpha_channel)) {
            BYTE *buffer = (BYTE *)malloc(width * 3);

            for (png_uint_32 k = 0; k < height; k++) {
                ConvertLine32To24(buffer, GetScanLine(pImage2,height - k - 1), width, NULL);
                png_write_row(png_ptr, buffer);
            }

            free(buffer);
        }
        else
        {
            for (png_uint_32 k = 0; k < height; k++)
            {
                png_write_row(png_ptr, GetScanLine(pImage2, height - k - 1));
            }
        }

        // It is REQUIRED to call this to finish writing the rest of the file
        // Bug with png_flush

        png_write_end(png_ptr, info_ptr);

        // clean up after the write, and free any memory allocated

        png_destroy_write_struct(&png_ptr, &info_ptr);

        if (palette)
                png_free(png_ptr, palette);
        pFile.close(); // fclose(pFile);

        return 0;
    } catch (...)
    {
        pFile.close(); // fclose(pFile);
        dynarithmic::delete_file(szFile);
        //        freeimage->output_message_proc(s_format_id, text);
    }

    return DTWAIN_ERR_FILEWRITE;
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
//		length = min(length, (int)info.dwEffWidth);
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

int GetColorType(BYTE *pImage)
{
    RGBQUAD *rgb;
    ULONG32 bpp;
    GetBitsPerPixel(pImage, &bpp);

    switch ( bpp )
    {
        case 1:
        {
            rgb = GetPalettePtr(pImage,bpp);
            if ((rgb->rgbRed == 0) && (rgb->rgbGreen == 0) && (rgb->rgbBlue == 0))
            {
                rgb++;

                if ((rgb->rgbRed == 255) && (rgb->rgbGreen == 255) && (rgb->rgbBlue == 255))
                    return FIC_MINISBLACK;
            }

            if ((rgb->rgbRed == 255) && (rgb->rgbGreen == 255) && (rgb->rgbBlue == 255)) {
                rgb++;

                if ((rgb->rgbRed == 0) && (rgb->rgbGreen == 0) && (rgb->rgbBlue == 0))
                    return FIC_MINISWHITE;
            }

            return FIC_PALETTE;
        }

        case 4:
        case 8: // Check if the DIB has a color or a greyscale palette
        {
            rgb = GetPalettePtr(pImage,bpp);

            for (int i = 0; i < GetColorsUsed(pImage); i++)
            {
                if ((rgb->rgbRed != rgb->rgbGreen) || (rgb->rgbRed != rgb->rgbBlue))
                    return FIC_PALETTE;

                // The DIB has a color palette if the greyscale isn't a linear ramp

                if (rgb->rgbRed != i)
                    return FIC_PALETTE;

                rgb++;
            }

            return FIC_MINISBLACK;
        }

        case 16:
        case 24:
            return FIC_RGB;

        case 32:
        {
            UINT32 height;
            UINT32 width;
            GetHeight(pImage, &height);
            GetWidth(pImage, &width);
            for (unsigned y = 0; y < height; ++y)
            {
                rgb = (RGBQUAD *)GetScanLine(pImage, y);

                for (unsigned x = 0; x < width; ++x)
                    if (rgb[x].rgbReserved != 0)
                        return FIC_RGBALPHA;
            }

            return FIC_RGB;
        }

        default :
            return FIC_MINISBLACK;
    }
}

void ConvertLine32To24(BYTE *target, BYTE *source, int width_in_pixels, RGBQUAD * /*pPalette*/)
{
    for (int cols = 0; cols < width_in_pixels; cols++)
    {
        target[0] = source[0];
        target[1] = source[1];
        target[2] = source[2];

        target += 3;
        source += 4;
    }
}

unsigned char * GetScanLine(BYTE *pDib, int scanline)
{
    if ( pDib )
    {
        return CalculateScanLine(GetDibBits(pDib), GetPitch(pDib), scanline);
    }
    return NULL;
}

bool GetBitsPerPixel(BYTE *pDIB, UINT32 *puBitCount)
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

int  GetColorsUsed(BYTE *pImage)
{
    return pImage ? ((LPBITMAPINFOHEADER)pImage)->biClrUsed : 0;
}

bool GetHeight(BYTE *pDIB, UINT32 *piHeight)
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

bool GetWidth(BYTE *pDIB, UINT32 *puWidth)
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

static unsigned GetPitch(BYTE *pDib)
{
    if ( pDib )
        return (GetLine(pDib) + 3) & ~3;
    return 0;
}

unsigned char * CalculateScanLine(unsigned char *bits, unsigned pitch, int scanline)
{
    return (bits + (pitch * scanline));
}

unsigned GetLine(BYTE *pDib)
{
    if ( pDib )
    {
        ULONG32 width;
        ULONG32 bpp;
        GetWidth(pDib, &width);
        GetBitsPerPixel(pDib, &bpp);
        return ((width * bpp) + 7) / 8;
    }
    return 0;
}
#endif