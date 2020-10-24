// ==========================================================
// fipImage class implementation
//
// Design and implementation by
// - Hervé Drolon (drolon@infonie.fr)
//
// This file is part of FreeImage 3
//
// COVERED CODE IS PROVIDED UNDER THIS LICENSE ON AN "AS IS" BASIS, WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, WITHOUT LIMITATION, WARRANTIES
// THAT THE COVERED CODE IS FREE OF DEFECTS, MERCHANTABLE, FIT FOR A PARTICULAR PURPOSE
// OR NON-INFRINGING. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE COVERED
// CODE IS WITH YOU. SHOULD ANY COVERED CODE PROVE DEFECTIVE IN ANY RESPECT, YOU (NOT
// THE INITIAL DEVELOPER OR ANY OTHER CONTRIBUTOR) ASSUME THE COST OF ANY NECESSARY
// SERVICING, REPAIR OR CORRECTION. THIS DISCLAIMER OF WARRANTY CONSTITUTES AN ESSENTIAL
// PART OF THIS LICENSE. NO USE OF ANY COVERED CODE IS AUTHORIZED HEREUNDER EXCEPT UNDER
// THIS DISCLAIMER.
//
// Use at your own risk!
// ==========================================================

#include "FreeImagePlus.h"
#include <algorithm>

///////////////////////////////////////////////////////////////////   
// Protected functions

BOOL fipImage::replace(FIBITMAP *new_dib) 
{
	if (!new_dib) 
		return FALSE;
	if (_dib) 
		_dib.reset();
	_dib = std::shared_ptr<FIBITMAP>(new_dib, [](FIBITMAP *dib) { if (dib) FreeImage_Unload(dib); });
	_bHasChanged = TRUE;
	return TRUE;
}

///////////////////////////////////////////////////////////////////
// Creation & Destruction

fipImage::fipImage(FREE_IMAGE_TYPE image_type, unsigned width, unsigned height, unsigned bpp) {
	_dib = NULL;
	_fif = FIF_UNKNOWN;
	_bHasChanged = FALSE;
	if(width && height && bpp) {
		setSize(image_type, width, height, bpp);
	}
}

BOOL fipImage::setSize(FREE_IMAGE_TYPE image_type, unsigned width, unsigned height, unsigned bpp, unsigned red_mask, unsigned green_mask, unsigned blue_mask) 
{
	if (!replace(FreeImage_AllocateT(image_type, width, height, bpp, red_mask, green_mask, blue_mask)))
		return FALSE;
		
	if(image_type == FIT_BITMAP) 
	{
		// Create palette if needed
		switch(bpp)	{
			case 1:
			case 4:
			case 8:
				RGBQUAD *pal = FreeImage_GetPalette(_dib.get());
				for(unsigned i = 0; i < FreeImage_GetColorsUsed(_dib.get()); i++) {
					pal[i].rgbRed = i;
					pal[i].rgbGreen = i;
					pal[i].rgbBlue = i;
				}
				break;
		}
	}

	_bHasChanged = TRUE;

	return TRUE;
}

void fipImage::clear() 
{
	_dib.reset();
	_bHasChanged = TRUE;
}

///////////////////////////////////////////////////////////////////
// Copying

void fipImage::swap(fipImage& left, fipImage& right)
{
	std::swap(left._fif, right._fif);
	std::swap(left._dib, right._dib);
	std::swap(left._bHasChanged, right._bHasChanged);
}

fipImage::fipImage(const fipImage& Image) : _dib(nullptr), _bHasChanged(false), _fif(Image._fif)
{
	replace(FreeImage_Clone(Image._dib.get()));
}

fipImage::fipImage(fipImage&& Image)
{
	swap(*this, Image);
	Image._dib = nullptr; 
}

fipImage& fipImage::operator=(const fipImage& Image) 
{
	if (this != &Image)
	{
		fipImage temp(Image);
		swap(*this, temp);
	}
	return *this;
}

fipImage& fipImage::operator=(fipImage&& Image)
{
	if (this != &Image)
	{
		swap(*this, Image);
		Image._dib = NULL;
	}
	return *this;
}

fipImage& fipImage::operator=(FIBITMAP *dib) 
{
	if(_dib.get() != dib) 
	{
		replace(dib);
		_fif = FIF_UNKNOWN;
	}
	return *this;
}

BOOL fipImage::copySubImage(fipImage& dst, int left, int top, int right, int bottom) const 
{
	if(_dib) 
	{
		dst = FreeImage_Copy(_dib.get(), left, top, right, bottom);
		return dst.isValid();
	}
	return FALSE;
}

BOOL fipImage::pasteSubImage(fipImage& src, int left, int top, int alpha) 
{
	if(_dib) 
	{
		BOOL bResult = FreeImage_Paste(_dib.get(), src._dib.get(), left, top, alpha);
		_bHasChanged = TRUE;
		return bResult;
	}
	return FALSE;
}

BOOL fipImage::crop(int left, int top, int right, int bottom) 
{
	if(_dib) 
	{
		FIBITMAP *dst = FreeImage_Copy(_dib.get(), left, top, right, bottom);
		return replace(dst);
	}
	return FALSE;
}

BOOL fipImage::createView(fipImage& dynamicView, unsigned left, unsigned top, unsigned right, unsigned bottom) 
{
	dynamicView = FreeImage_CreateView(_dib.get(), left, top, right, bottom);
	return dynamicView.isValid();
}

///////////////////////////////////////////////////////////////////
// Information functions

FREE_IMAGE_TYPE fipImage::getImageType() const {
	return FreeImage_GetImageType(_dib.get());
}

FREE_IMAGE_FORMAT fipImage::getFIF() const {
	return _fif;
}

unsigned fipImage::getWidth() const {
	return FreeImage_GetWidth(_dib.get());
}

unsigned fipImage::getHeight() const {
	return FreeImage_GetHeight(_dib.get());
}

unsigned fipImage::getScanWidth() const {
	return FreeImage_GetPitch(_dib.get());
}

BOOL fipImage::isValid() const {
	return (_dib != NULL) ? TRUE:FALSE;
}

const BITMAPINFO* fipImage::getInfo() const {
	return FreeImage_GetInfo(_dib.get());
}

const BITMAPINFOHEADER* fipImage::getInfoHeader() const {
	return FreeImage_GetInfoHeader(_dib.get());
}

unsigned fipImage::getImageSize() const {
	return FreeImage_GetDIBSize(_dib.get());
}

unsigned fipImage::getImageMemorySize() const {
	return FreeImage_GetMemorySize(_dib.get());
}

unsigned fipImage::getBitsPerPixel() const {
	return FreeImage_GetBPP(_dib.get());
}

unsigned fipImage::getLine() const {
	return FreeImage_GetLine(_dib.get());
}

double fipImage::getHorizontalResolution() const {
	return (FreeImage_GetDotsPerMeterX(_dib.get()) / (double)100);
}

double fipImage::getVerticalResolution() const {
	return (FreeImage_GetDotsPerMeterY(_dib.get()) / (double)100);
}

void fipImage::setHorizontalResolution(double value) {
	FreeImage_SetDotsPerMeterX(_dib.get(), (unsigned)(value * 100 + 0.5));
}

void fipImage::setVerticalResolution(double value) {
	FreeImage_SetDotsPerMeterY(_dib.get(), (unsigned)(value * 100 + 0.5));
}


///////////////////////////////////////////////////////////////////
// Palette operations

RGBQUAD* fipImage::getPalette() const {
	return FreeImage_GetPalette(_dib.get());
}

unsigned fipImage::getPaletteSize() const {
	return FreeImage_GetColorsUsed(_dib.get()) * sizeof(RGBQUAD);
}

unsigned fipImage::getColorsUsed() const {
	return FreeImage_GetColorsUsed(_dib.get());
}

FREE_IMAGE_COLOR_TYPE fipImage::getColorType() const { 
	return FreeImage_GetColorType(_dib.get());
}

BOOL fipImage::isGrayscale() const {
	return ((FreeImage_GetBPP(_dib.get()) == 8) && (FreeImage_GetColorType(_dib.get()) != FIC_PALETTE)); 
}

///////////////////////////////////////////////////////////////////
// Thumbnail access

BOOL fipImage::getThumbnail(fipImage& image) const {
	image = FreeImage_Clone( FreeImage_GetThumbnail(_dib.get()) );
	return image.isValid();
}

BOOL fipImage::setThumbnail(const fipImage& image) {
	return FreeImage_SetThumbnail(_dib.get(), image._dib.get());
}

BOOL fipImage::hasThumbnail() const {
	return (FreeImage_GetThumbnail(_dib.get()) != NULL);
}

BOOL fipImage::clearThumbnail() {
	return FreeImage_SetThumbnail(_dib.get(), NULL);
}


///////////////////////////////////////////////////////////////////
// Pixel access

BYTE* fipImage::accessPixels() const {
	return FreeImage_GetBits(_dib.get()); 
}

BYTE* fipImage::getScanLine(unsigned scanline) const {
	if(scanline < FreeImage_GetHeight(_dib.get())) {
		return FreeImage_GetScanLine(_dib.get(), scanline);
	}
	return NULL;
}

BOOL fipImage::getPixelIndex(unsigned x, unsigned y, BYTE *value) const {
	return FreeImage_GetPixelIndex(_dib.get(), x, y, value);
}

BOOL fipImage::getPixelColor(unsigned x, unsigned y, RGBQUAD *value) const {
	return FreeImage_GetPixelColor(_dib.get(), x, y, value);
}

BOOL fipImage::setPixelIndex(unsigned x, unsigned y, BYTE *value) {
	_bHasChanged = TRUE;
	return FreeImage_SetPixelIndex(_dib.get(), x, y, value);
}

BOOL fipImage::setPixelColor(unsigned x, unsigned y, RGBQUAD *value) {
	_bHasChanged = TRUE;
	return FreeImage_SetPixelColor(_dib.get(), x, y, value);
}

///////////////////////////////////////////////////////////////////
// File type identification

FREE_IMAGE_FORMAT fipImage::identifyFIF(const char* lpszPathName) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;

	// check the file signature and get its format
	// (the second argument is currently not used by FreeImage)
	fif = FreeImage_GetFileType(lpszPathName, 0);
	if(fif == FIF_UNKNOWN) {
		// no signature ?
		// try to guess the file format from the file extension
		fif = FreeImage_GetFIFFromFilename(lpszPathName);
	}

	return fif;
}

FREE_IMAGE_FORMAT fipImage::identifyFIFU(const wchar_t* lpszPathName) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;

	// check the file signature and get its format
	// (the second argument is currently not used by FreeImage)
	fif = FreeImage_GetFileTypeU(lpszPathName, 0);
	if(fif == FIF_UNKNOWN) {
		// no signature ?
		// try to guess the file format from the file extension
		fif = FreeImage_GetFIFFromFilenameU(lpszPathName);
	}

	return fif;
}

FREE_IMAGE_FORMAT fipImage::identifyFIFFromHandle(FreeImageIO *io, fi_handle handle) {
	if(io && handle) {
		// check the file signature and get its format
		return FreeImage_GetFileTypeFromHandle(io, handle);
	}
	return FIF_UNKNOWN;
}

FREE_IMAGE_FORMAT fipImage::identifyFIFFromMemory(FIMEMORY *hmem) {
	if(hmem != NULL) {
		return FreeImage_GetFileTypeFromMemory(hmem, 0);
	}
	return FIF_UNKNOWN;
}


///////////////////////////////////////////////////////////////////
// Loading & Saving

BOOL fipImage::load(FREE_IMAGE_FORMAT fif, const char* lpszPathName, int flag) {
	replace(FreeImage_Load(fif, lpszPathName, flag));
	_fif = fif;
	_bHasChanged = TRUE;
	return (_dib == NULL) ? FALSE : TRUE;
}

BOOL fipImage::load(const char* lpszPathName, int flag) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;

	// check the file signature and get its format
	// (the second argument is currently not used by FreeImage)
	fif = FreeImage_GetFileType(lpszPathName, 0);
	if(fif == FIF_UNKNOWN) {
		// no signature ?
		// try to guess the file format from the file extension
		fif = FreeImage_GetFIFFromFilename(lpszPathName);
	}
	// check that the plugin has reading capabilities ...
	if((fif != FIF_UNKNOWN) && FreeImage_FIFSupportsReading(fif)) {
		return load(fif, lpszPathName, flag);
	}

	return FALSE;
}

BOOL fipImage::loadU(FREE_IMAGE_FORMAT fif, const wchar_t* lpszPathName, int flag) 
{
	replace(FreeImage_LoadU(fif, lpszPathName, flag));
	_fif = fif;
	_bHasChanged = TRUE;
	return (_dib == NULL) ? FALSE : TRUE;
}

BOOL fipImage::loadU(const wchar_t* lpszPathName, int flag) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;

	// check the file signature and get its format
	// (the second argument is currently not used by FreeImage)
	fif = FreeImage_GetFileTypeU(lpszPathName, 0);
	if(fif == FIF_UNKNOWN) {
		// no signature ?
		// try to guess the file format from the file extension
		fif = FreeImage_GetFIFFromFilenameU(lpszPathName);
	}
	// check that the plugin has reading capabilities ...
	if((fif != FIF_UNKNOWN) && FreeImage_FIFSupportsReading(fif)) {
		return loadU(fif, lpszPathName, flag);
	}

	return FALSE;
}

BOOL fipImage::loadFromHandle(FreeImageIO *io, fi_handle handle, int flag) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;

	// check the file signature and get its format
	fif = FreeImage_GetFileTypeFromHandle(io, handle);
	if((fif != FIF_UNKNOWN) && FreeImage_FIFSupportsReading(fif)) 
	{
		// Load the file
		replace(FreeImage_LoadFromHandle(fif, io, handle, flag));
		_fif = fif;
		_bHasChanged = TRUE;
		return (_dib == NULL) ? FALSE : TRUE;
	}
	return FALSE;
}

BOOL fipImage::loadFromMemory(fipMemoryIO& memIO, int flag) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;

	// check the file signature and get its format
	fif = memIO.getFileType();
	if((fif != FIF_UNKNOWN) && FreeImage_FIFSupportsReading(fif)) 
	{
		// Load the file
		replace(memIO.load(fif, flag));
		_fif = fif;
		_bHasChanged = TRUE;

		return (_dib == NULL) ? FALSE : TRUE;
	}
	return FALSE;
}

BOOL fipImage::loadFromMemory(FREE_IMAGE_FORMAT fif, fipMemoryIO& memIO, int flag) {
	if (fif != FIF_UNKNOWN) 
	{
		// Load the file
		replace(memIO.load(fif, flag));
		_fif = fif;
		_bHasChanged = TRUE;

		return (_dib == NULL) ? FALSE : TRUE;
	}
	return FALSE;
}

BOOL  fipImage::save(FREE_IMAGE_FORMAT fif, const char* lpszPathName, int flag) {
	BOOL bSuccess = FreeImage_Save(fif, _dib.get(), lpszPathName, flag);
	_fif = fif;
	return bSuccess;
}

BOOL fipImage::save(const char* lpszPathName, int flag) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;
	BOOL bSuccess = FALSE;

	// Try to guess the file format from the file extension
	fif = FreeImage_GetFIFFromFilename(lpszPathName);
	if(fif != FIF_UNKNOWN ) {
		// Check that the dib can be saved in this format
		BOOL bCanSave;

		FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
		if(image_type == FIT_BITMAP) {
			// standard bitmap type
			WORD bpp = FreeImage_GetBPP(_dib.get());
			bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
		} else {
			// special bitmap type
			bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
		}

		if(bCanSave) {
			bSuccess = FreeImage_Save(fif, _dib.get(), lpszPathName, flag);
			_fif = fif;
			return bSuccess;
		}
	}
	return bSuccess;
}

BOOL  fipImage::saveU(FREE_IMAGE_FORMAT fif, const wchar_t* lpszPathName, int flag) {
	BOOL bSuccess = FreeImage_SaveU(fif, _dib.get(), lpszPathName, flag);
	_fif = fif;
	return bSuccess;
}

BOOL fipImage::saveU(const wchar_t* lpszPathName, int flag) {
	FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;
	BOOL bSuccess = FALSE;

	// Try to guess the file format from the file extension
	fif = FreeImage_GetFIFFromFilenameU(lpszPathName);
	if(fif != FIF_UNKNOWN ) {
		// Check that the dib can be saved in this format
		BOOL bCanSave;

		FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
		if(image_type == FIT_BITMAP) {
			// standard bitmap type
			WORD bpp = FreeImage_GetBPP(_dib.get());
			bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
		} else {
			// special bitmap type
			bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
		}

		if(bCanSave) {
			bSuccess = FreeImage_SaveU(fif, _dib.get(), lpszPathName, flag);
			_fif = fif;
			return bSuccess;
		}
	}
	return bSuccess;
}

BOOL fipImage::saveToHandle(FREE_IMAGE_FORMAT fif, FreeImageIO *io, fi_handle handle, int flag) {
	BOOL bSuccess = FALSE;

	if(fif != FIF_UNKNOWN ) {
		// Check that the dib can be saved in this format
		BOOL bCanSave;

		FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
		if(image_type == FIT_BITMAP) {
			// standard bitmap type
			WORD bpp = FreeImage_GetBPP(_dib.get());
			bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
		} else {
			// special bitmap type
			bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
		}

		if(bCanSave) {
			bSuccess = FreeImage_SaveToHandle(fif, _dib.get(), io, handle, flag);
			_fif = fif;
			return bSuccess;
		}
	}
	return bSuccess;
}

BOOL fipImage::saveToMemory(FREE_IMAGE_FORMAT fif, fipMemoryIO& memIO, int flag) {
	BOOL bSuccess = FALSE;

	if(fif != FIF_UNKNOWN ) {
		// Check that the dib can be saved in this format
		BOOL bCanSave;

		FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
		if(image_type == FIT_BITMAP) {
			// standard bitmap type
			WORD bpp = FreeImage_GetBPP(_dib.get());
			bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
		} else {
			// special bitmap type
			bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
		}

		if(bCanSave) {
			bSuccess = memIO.save(fif, _dib.get(), flag);
			_fif = fif;
			return bSuccess;
		}
	}
	return bSuccess;
}

///////////////////////////////////////////////////////////////////   
BOOL  fipImage::saveEx(FREE_IMAGE_FORMAT fif, const char* lpszPathName, int page, int flag)
{
    BOOL bSuccess = FreeImage_SaveEx(fif, _dib.get(), lpszPathName, page, flag);
    _fif = fif;
    return bSuccess;
}

BOOL fipImage::saveEx(const char* lpszPathName, int page, int flag)
{
    FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;
    BOOL bSuccess = FALSE;

    // Try to guess the file format from the file extension
    fif = FreeImage_GetFIFFromFilename(lpszPathName);
    if (fif != FIF_UNKNOWN)
    {
        // Check that the dib can be saved in this format
        BOOL bCanSave;

        FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
        if (image_type == FIT_BITMAP)
        {
            // standard bitmap type
            WORD bpp = FreeImage_GetBPP(_dib.get());
            bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
        }
        else
        {
            // special bitmap type
            bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
        }

        if (bCanSave)
        {
            bSuccess = FreeImage_Save(fif, _dib.get(), lpszPathName, flag);
            _fif = fif;
            return bSuccess;
        }
    }
    return bSuccess;
}

BOOL  fipImage::saveUEx(FREE_IMAGE_FORMAT fif, const wchar_t* lpszPathName, int page, int flag)
{
    BOOL bSuccess = FreeImage_SaveU(fif, _dib.get(), lpszPathName, flag);
    _fif = fif;
    return bSuccess;
}

BOOL fipImage::saveUEx(const wchar_t* lpszPathName, int page, int flag)
{
    FREE_IMAGE_FORMAT fif = FIF_UNKNOWN;
    BOOL bSuccess = FALSE;

    // Try to guess the file format from the file extension
    fif = FreeImage_GetFIFFromFilenameU(lpszPathName);
    if (fif != FIF_UNKNOWN)
    {
        // Check that the dib can be saved in this format
        BOOL bCanSave;

        FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
        if (image_type == FIT_BITMAP)
        {
            // standard bitmap type
            WORD bpp = FreeImage_GetBPP(_dib.get());
            bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
        }
        else
        {
            // special bitmap type
            bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
        }

        if (bCanSave)
        {
            bSuccess = FreeImage_SaveU(fif, _dib.get(), lpszPathName, flag);
            _fif = fif;
            return bSuccess;
        }
    }
    return bSuccess;
}

BOOL fipImage::saveToHandleEx(FREE_IMAGE_FORMAT fif, FreeImageIO *io, fi_handle handle, int page, int flag)
{
    BOOL bSuccess = FALSE;

    if (fif != FIF_UNKNOWN)
    {
        // Check that the dib can be saved in this format
        BOOL bCanSave;

        FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
        if (image_type == FIT_BITMAP)
        {
            // standard bitmap type
            WORD bpp = FreeImage_GetBPP(_dib.get());
            bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
        }
        else
        {
            // special bitmap type
            bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
        }

        if (bCanSave)
        {
            bSuccess = FreeImage_SaveToHandle(fif, _dib.get(), io, handle, flag);
            _fif = fif;
            return bSuccess;
        }
    }
    return bSuccess;
}

BOOL fipImage::saveToMemoryEx(FREE_IMAGE_FORMAT fif, fipMemoryIO& memIO, int page, int flag)
{
    BOOL bSuccess = FALSE;

    if (fif != FIF_UNKNOWN)
    {
        // Check that the dib can be saved in this format
        BOOL bCanSave;

        FREE_IMAGE_TYPE image_type = FreeImage_GetImageType(_dib.get());
        if (image_type == FIT_BITMAP)
        {
            // standard bitmap type
            WORD bpp = FreeImage_GetBPP(_dib.get());
            bCanSave = (FreeImage_FIFSupportsWriting(fif) && FreeImage_FIFSupportsExportBPP(fif, bpp));
        }
        else
        {
            // special bitmap type
            bCanSave = FreeImage_FIFSupportsExportType(fif, image_type);
        }

        if (bCanSave)
        {
            bSuccess = memIO.save(fif, _dib.get(), flag);
            _fif = fif;
            return bSuccess;
        }
    }
    return bSuccess;
}

///////////////////////////////////////////////////////////////////   
// Conversion routines

BOOL fipImage::convertToType(FREE_IMAGE_TYPE image_type, BOOL scale_linear) 
{
	return replace(FreeImage_ConvertToType(_dib.get(), image_type, scale_linear));
}

BOOL fipImage::threshold(BYTE T) {
	if(_dib) 
	{
		return replace(FreeImage_Threshold(_dib.get(), T)); 
	}
	return FALSE;
}

BOOL fipImage::convertTo4Bits() {
	if(_dib) {
		return replace(FreeImage_ConvertTo4Bits(_dib.get()));
	}
	return FALSE;
}

BOOL fipImage::convertTo8Bits() {
	if(_dib) {
		return replace(FreeImage_ConvertTo8Bits(_dib.get()));
	}
	return FALSE;
}

BOOL fipImage::convertTo16Bits555() {
	if(_dib) {
		FIBITMAP *dib16_555 = FreeImage_ConvertTo16Bits555(_dib.get());
		return replace(dib16_555);
	}
	return FALSE;
}

BOOL fipImage::convertTo16Bits565() {
	if(_dib) {
		FIBITMAP *dib16_565 = FreeImage_ConvertTo16Bits565(_dib.get());
		return replace(dib16_565);
	}
	return FALSE;
}

BOOL fipImage::convertTo24Bits() {
	if(_dib) {
		FIBITMAP *dibRGB = FreeImage_ConvertTo24Bits(_dib.get());
		return replace(dibRGB);
	}
	return FALSE;
}

BOOL fipImage::convertTo32Bits() {
	if(_dib) {
		FIBITMAP *dib32 = FreeImage_ConvertTo32Bits(_dib.get());
		return replace(dib32);
	}
	return FALSE;
}

BOOL fipImage::convertToGrayscale() {
	if(_dib) {
		FIBITMAP *dib8 = FreeImage_ConvertToGreyscale(_dib.get());
		return replace(dib8);
	}
	return FALSE;
}

BOOL fipImage::colorQuantize(FREE_IMAGE_QUANTIZE algorithm) {
	if(_dib) {
		FIBITMAP *dib8 = FreeImage_ColorQuantize(_dib.get(), algorithm);
		return replace(dib8);
	}
	return FALSE;
}

BOOL fipImage::dither(FREE_IMAGE_DITHER algorithm) {
	if(_dib) {
		FIBITMAP *dib = FreeImage_Dither(_dib.get(), algorithm);
		return replace(dib);
	}
	return FALSE;
}

BOOL fipImage::convertToFloat() {
	if(_dib) {
		FIBITMAP *dib = FreeImage_ConvertToFloat(_dib.get());
		return replace(dib);
	}
	return FALSE;
}

BOOL fipImage::convertToRGBF() {
	if(_dib) {
		FIBITMAP *dib = FreeImage_ConvertToRGBF(_dib.get());
		return replace(dib);
	}
	return FALSE;
}

BOOL fipImage::convertToRGBAF() {
	if(_dib) {
		FIBITMAP *dib = FreeImage_ConvertToRGBAF(_dib.get());
		return replace(dib);
	}
	return FALSE;
}

BOOL fipImage::convertToUINT16() {
	if(_dib) {
		FIBITMAP *dib = FreeImage_ConvertToUINT16(_dib.get());
		return replace(dib);
	}
	return FALSE;
}

BOOL fipImage::convertToRGB16() {
	if(_dib) {
		FIBITMAP *dib = FreeImage_ConvertToRGB16(_dib.get());
		return replace(dib);
	}
	return FALSE;
}

BOOL fipImage::convertToRGBA16() {
	if(_dib) {
		FIBITMAP *dib = FreeImage_ConvertToRGBA16(_dib.get());
		return replace(dib);
	}
	return FALSE;
}

BOOL fipImage::toneMapping(FREE_IMAGE_TMO tmo, double first_param, double second_param, double third_param, double fourth_param) {
	if(_dib) {
		FIBITMAP *dst = NULL;
		// Apply a tone mapping algorithm and convert to 24-bit 
		switch(tmo) {
			case FITMO_REINHARD05:
				dst = FreeImage_TmoReinhard05Ex(_dib.get(), first_param, second_param, third_param, fourth_param);
				break;
			default:
				dst = FreeImage_ToneMapping(_dib.get(), tmo, first_param, second_param);
				break;
		}

		return replace(dst);
	}
	return FALSE;
}

///////////////////////////////////////////////////////////////////   
// Transparency support: background colour and alpha channel

BOOL fipImage::isTransparent() const {
	return FreeImage_IsTransparent(_dib.get());
}

unsigned fipImage::getTransparencyCount() const {
	return FreeImage_GetTransparencyCount(_dib.get());
}

BYTE* fipImage::getTransparencyTable() const {
	return FreeImage_GetTransparencyTable(_dib.get());
}

void fipImage::setTransparencyTable(BYTE *table, int count) {
	FreeImage_SetTransparencyTable(_dib.get(), table, count);
	_bHasChanged = TRUE;
}

BOOL fipImage::hasFileBkColor() const {
	return FreeImage_HasBackgroundColor(_dib.get());
}

BOOL fipImage::getFileBkColor(RGBQUAD *bkcolor) const {
	return FreeImage_GetBackgroundColor(_dib.get(), bkcolor);
}

BOOL fipImage::setFileBkColor(RGBQUAD *bkcolor) {
	_bHasChanged = TRUE;
	return FreeImage_SetBackgroundColor(_dib.get(), bkcolor);
}

///////////////////////////////////////////////////////////////////   
// Channel processing support

BOOL fipImage::getChannel(fipImage& image, FREE_IMAGE_COLOR_CHANNEL channel) const {
	if(_dib) {
		image = FreeImage_GetChannel(_dib.get(), channel);
		return image.isValid();
	}
	return FALSE;
}

BOOL fipImage::setChannel(fipImage& image, FREE_IMAGE_COLOR_CHANNEL channel) {
	if(_dib) {
		_bHasChanged = TRUE;
		return FreeImage_SetChannel(_dib.get(), image._dib.get(), channel);
	}
	return FALSE;
}

BOOL fipImage::splitChannels(fipImage& RedChannel, fipImage& GreenChannel, fipImage& BlueChannel) {
	if(_dib) {
		RedChannel = FreeImage_GetChannel(_dib.get(), FICC_RED);
		GreenChannel = FreeImage_GetChannel(_dib.get(), FICC_GREEN);
		BlueChannel = FreeImage_GetChannel(_dib.get(), FICC_BLUE);

		return (RedChannel.isValid() && GreenChannel.isValid() && BlueChannel.isValid());
	}
	return FALSE;
}

BOOL fipImage::combineChannels(fipImage& red, fipImage& green, fipImage& blue) {
	if(!_dib) {
		int width = red.getWidth();
		int height = red.getHeight();
		replace(FreeImage_Allocate(width, height, 24, FI_RGBA_RED_MASK, FI_RGBA_GREEN_MASK, FI_RGBA_BLUE_MASK));
	}

	if(_dib) {
		BOOL bResult = TRUE;
		bResult &= FreeImage_SetChannel(_dib.get(), red._dib.get(), FICC_RED);
		bResult &= FreeImage_SetChannel(_dib.get(), green._dib.get(), FICC_GREEN);
		bResult &= FreeImage_SetChannel(_dib.get(), blue._dib.get(), FICC_BLUE);

		_bHasChanged = TRUE;

		return bResult;
	}
	return FALSE;
}

///////////////////////////////////////////////////////////////////   
// Rotation and flipping

BOOL fipImage::rotateEx(double angle, double x_shift, double y_shift, double x_origin, double y_origin, BOOL use_mask) {
	if(_dib) {
		if(FreeImage_GetBPP(_dib.get()) >= 8) {
			FIBITMAP *rotated = FreeImage_RotateEx(_dib.get(), angle, x_shift, y_shift, x_origin, y_origin, use_mask);
			return replace(rotated);
		}
	}
	return FALSE;
}

BOOL fipImage::rotate(double angle, const void *bkcolor) {
	if(_dib) {
		switch(FreeImage_GetImageType(_dib.get())) {
			case FIT_BITMAP:
				switch(FreeImage_GetBPP(_dib.get())) {
					case 1:
					case 8:
					case 24:
					case 32:
						break;
					default:
						return FALSE;
				}
				break;

			case FIT_UINT16:
			case FIT_RGB16:
			case FIT_RGBA16:
			case FIT_FLOAT:
			case FIT_RGBF:
			case FIT_RGBAF:
				break;
			default:
				return FALSE;
				break;
		}

		FIBITMAP *rotated = FreeImage_Rotate(_dib.get(), angle, bkcolor);
		return replace(rotated);

	}
	return FALSE;
}

BOOL fipImage::flipVertical() {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_FlipVertical(_dib.get());
	}
	return FALSE;
}

BOOL fipImage::flipHorizontal() {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_FlipHorizontal(_dib.get());
	}
	return FALSE;
}

///////////////////////////////////////////////////////////////////   
// Color manipulation routines

BOOL fipImage::invert() {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_Invert(_dib.get());
	}
	return FALSE;
}

BOOL fipImage::adjustCurve(BYTE *LUT, FREE_IMAGE_COLOR_CHANNEL channel) {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_AdjustCurve(_dib.get(), LUT, channel);
	}
	return FALSE;
}

BOOL fipImage::adjustGamma(double gamma) {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_AdjustGamma(_dib.get(), gamma);
	}
	return FALSE;
}

BOOL fipImage::adjustBrightness(double percentage) {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_AdjustBrightness(_dib.get(), percentage);
	}
	return FALSE;
}

BOOL fipImage::adjustContrast(double percentage) {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_AdjustContrast(_dib.get(), percentage);
	}
	return FALSE;
}

BOOL fipImage::adjustBrightnessContrastGamma(double brightness, double contrast, double gamma) {
	if(_dib) {
		_bHasChanged = TRUE;

		return FreeImage_AdjustColors(_dib.get(), brightness, contrast, gamma, FALSE);
	}
	return FALSE;
}

BOOL fipImage::getHistogram(DWORD *histo, FREE_IMAGE_COLOR_CHANNEL channel) const {
	if(_dib) {
		return FreeImage_GetHistogram(_dib.get(), histo, channel);
	}
	return FALSE;
}

///////////////////////////////////////////////////////////////////
// Upsampling / downsampling routine

BOOL fipImage::rescale(unsigned new_width, unsigned new_height, FREE_IMAGE_FILTER filter) {
	if(_dib) {
		switch(FreeImage_GetImageType(_dib.get())) {
			case FIT_BITMAP:
			case FIT_UINT16:
			case FIT_RGB16:
			case FIT_RGBA16:
			case FIT_FLOAT:
			case FIT_RGBF:
			case FIT_RGBAF:
				break;
			default:
				return FALSE;
				break;
		}

		// Perform upsampling / downsampling
		FIBITMAP *dst = FreeImage_Rescale(_dib.get(), new_width, new_height, filter);
		return replace(dst);
	}
	return FALSE;
}

BOOL fipImage::makeThumbnail(unsigned max_size, BOOL convert) {
	if(_dib) {
		switch(FreeImage_GetImageType(_dib.get())) {
			case FIT_BITMAP:
			case FIT_UINT16:
			case FIT_RGB16:
			case FIT_RGBA16:
			case FIT_FLOAT:
			case FIT_RGBF:
			case FIT_RGBAF:
				break;
			default:
				return FALSE;
				break;
		}

		// Perform downsampling
		FIBITMAP *dst = FreeImage_MakeThumbnail(_dib.get(), max_size, convert);
		return replace(dst);
	}
	return FALSE;
}

///////////////////////////////////////////////////////////////////
// Metadata

unsigned fipImage::getMetadataCount(FREE_IMAGE_MDMODEL model) const {
	return FreeImage_GetMetadataCount(model, _dib.get());
}

BOOL fipImage::getMetadata(FREE_IMAGE_MDMODEL model, const char *key, fipTag& tag) const {
	FITAG *searchedTag = NULL;
	FreeImage_GetMetadata(model, _dib.get(), key, &searchedTag);
	if(searchedTag != NULL) {
		tag = FreeImage_CloneTag(searchedTag);
		return TRUE;
	} else {
		// clear the tag
		tag = (FITAG*)NULL;
	}
	return FALSE;
}

BOOL fipImage::setMetadata(FREE_IMAGE_MDMODEL model, const char *key, fipTag& tag) {
	return FreeImage_SetMetadata(model, _dib.get(), key, tag);
}

void fipImage::clearMetadata() {
	// clear all metadata attached to the dib
	FreeImage_SetMetadata(FIMD_COMMENTS, _dib.get(), NULL, NULL);			// single comment or keywords
	FreeImage_SetMetadata(FIMD_EXIF_MAIN, _dib.get(), NULL, NULL);		// Exif-TIFF metadata
	FreeImage_SetMetadata(FIMD_EXIF_EXIF, _dib.get(), NULL, NULL);		// Exif-specific metadata
	FreeImage_SetMetadata(FIMD_EXIF_GPS, _dib.get(), NULL, NULL);			// Exif GPS metadata
	FreeImage_SetMetadata(FIMD_EXIF_MAKERNOTE, _dib.get(), NULL, NULL);	// Exif maker note metadata
	FreeImage_SetMetadata(FIMD_EXIF_INTEROP, _dib.get(), NULL, NULL);		// Exif interoperability metadata
	FreeImage_SetMetadata(FIMD_IPTC, _dib.get(), NULL, NULL);				// IPTC/NAA metadata
	FreeImage_SetMetadata(FIMD_XMP, _dib.get(), NULL, NULL);				// Abobe XMP metadata
	FreeImage_SetMetadata(FIMD_GEOTIFF, _dib.get(), NULL, NULL);			// GeoTIFF metadata
	FreeImage_SetMetadata(FIMD_ANIMATION, _dib.get(), NULL, NULL);		// Animation metadata
	FreeImage_SetMetadata(FIMD_CUSTOM, _dib.get(), NULL, NULL);			// Used to attach other metadata types to a dib
	FreeImage_SetMetadata(FIMD_EXIF_RAW, _dib.get(), NULL, NULL);			// Exif metadata as a raw buffer
}

