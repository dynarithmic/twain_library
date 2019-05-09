#include "tiflib32.h"
#include "ctlobstr.h"

extern "C" int TIFFSplitMain(int argc, char* argv[]);

TIFF* FUNCCONVENTION DTWLIB_TIFFOpen(LPCTSTR szFile, LPCTSTR szOpen)
{
    return TIFFOpen(dynarithmic::StringConversion::Convert_Native_To_Ansi(szFile).c_str(), 
                    dynarithmic::StringConversion::Convert_Native_To_Ansi(szOpen).c_str());
}

LONG  FUNCCONVENTION DTWLIB_TIFFWriteDirectory(TIFF* pTiff)
{
    return TIFFWriteDirectory(pTiff);
}

void FUNCCONVENTION DTWLIB_TIFFSetImageSize(TIFF* pTiff, LONG Width, LONG Length)
{
    TIFFSetField(pTiff, TIFFTAG_IMAGEWIDTH, Width);
    TIFFSetField(pTiff, TIFFTAG_IMAGELENGTH, Length);
}

void FUNCCONVENTION  DTWLIB_TIFFSetSamplesPerPixel(TIFF* pTiff, LONG SamplesPerPixel, LONG BitsPerSample)
{
    TIFFSetField(pTiff,TIFFTAG_SAMPLESPERPIXEL, SamplesPerPixel);
    TIFFSetField(pTiff,TIFFTAG_BITSPERSAMPLE, BitsPerSample);
}


void FUNCCONVENTION  DTWLIB_TIFFSetOrientation(TIFF* pTiff, LONG nOrientation)
{
    TIFFSetField(pTiff, TIFFTAG_ORIENTATION, nOrientation);
}

void FUNCCONVENTION  DTWLIB_TIFFSetCompression(TIFF* pTiff, LONG nCompression)
{
    TIFFSetField(pTiff, TIFFTAG_COMPRESSION, nCompression);
}


void FUNCCONVENTION  DTWLIB_TIFFSetResolutionPerUnit(TIFF* pTiff, LONG Unit)
{
    TIFFSetField(pTiff, TIFFTAG_RESOLUTIONUNIT, Unit);
}

void FUNCCONVENTION  DTWLIB_TIFFSetResolution(TIFF* pTiff, double Resolution, LONG nWhich)
{
    TIFFSetField(pTiff, nWhich, Resolution);
}

void FUNCCONVENTION  DTWLIB_TIFFSetPlanarConfig(TIFF* pTiff, LONG nConfig)
{
    TIFFSetField(pTiff, TIFFTAG_PLANARCONFIG, nConfig);
}

void FUNCCONVENTION  DTWLIB_TIFFSetPhotometric(TIFF* pTiff, LONG nPhoto)
{
    TIFFSetField(pTiff, TIFFTAG_PHOTOMETRIC, nPhoto);
}

void FUNCCONVENTION  DTWLIB_TIFFSetRowsPerStrip(TIFF* pTiff, LONG nRows)
{
    TIFFSetField(pTiff, TIFFTAG_ROWSPERSTRIP, nRows);
}


void FUNCCONVENTION  DTWLIB_TIFFSetColorMap(TIFF* pTiff, uint16 *pRed,
                                  uint16 *pGreen, 
                                  uint16 *pBlue)
{
    TIFFSetField(pTiff, TIFFTAG_COLORMAP, pRed, pGreen, pBlue);
}

void FUNCCONVENTION  DTWLIB_TIFFSetJpegQuality(TIFF* pTiff, LONG nQuality)
{
    TIFFSetField(pTiff, TIFFTAG_JPEGQUALITY, nQuality);
}

LONG FUNCCONVENTION DTWLIB_TIFFSetScanLine(TIFF *pTiff, tdata_t buf, uint32 row, tsample_t sample)
{
    return TIFFWriteScanline(pTiff, buf, row, sample);
}


void FUNCCONVENTION  DTWLIB_TIFFCloseImage(TIFF* pTiff)
{
    if ( pTiff )
        TIFFClose(pTiff);
}


void FUNCCONVENTION  DTWLIB_TIFFFreeColorMap(uint16 *pRed, uint16 *pGreen, uint16 *pBlue)
{
    if (pRed)
        _TIFFfree(pRed);
    if ( pGreen )
        _TIFFfree(pGreen);
    if ( pBlue )
        _TIFFfree(pBlue);
}

tdata_t FUNCCONVENTION DTWLIB_TIFFAlloc(tsize_t nSize)
{
    return _TIFFmalloc(nSize);
}

void FUNCCONVENTION DTWLIB_TIFFFree(tdata_t p)
{
    if ( p )
        _TIFFfree(p);
}

void FUNCCONVENTION DTWLIB_TIFFWriteSoftware(TIFF *pTiff, LPCTSTR szData)
{
    if ( pTiff )
        TIFFSetField(pTiff, TIFFTAG_SOFTWARE, dynarithmic::StringConversion::Convert_Native_To_Ansi(szData));
}
