#ifndef BITMAPACCESS_H
#define BITMAPACCESS_H
/**
Constants for the BITMAPINFOHEADER::biCompression field
BI_RGB:
The bitmap is in uncompressed red green blue (RGB) format that is not compressed and does not use color masks.
BI_BITFIELDS:
The bitmap is not compressed and the color table consists of three DWORD color masks that specify the red, green, and blue components,
respectively, of each pixel. This is valid when used with 16 and 32-bits per pixel bitmaps.
*/
#ifndef _WINGDI_
#define BI_RGB       0L
#define BI_BITFIELDS 3L
#endif // _WINGDI_

// ----------------------------------------------------------
//  Metadata definitions
// ----------------------------------------------------------

/** helper for map<key, value> where value is a pointer to a FreeImage tag */
typedef std::map<std::string, FITAG*> TAGMAP;

/** helper for map<FREE_IMAGE_MDMODEL, TAGMAP*> */
typedef std::map<int, TAGMAP*> METADATAMAP;

/** helper for metadata iterator */
FI_STRUCT(METADATAHEADER) {
    long pos;		//! current position when iterating the map
    TAGMAP *tagmap;	//! pointer to the tag map
};

// ----------------------------------------------------------
//  FIBITMAP definition
// ----------------------------------------------------------

/**
FreeImage header structure
*/
FI_STRUCT(FREEIMAGEHEADER) {
    /** data type - bitmap, array of long, double, complex, etc */
    FREE_IMAGE_TYPE type;

    /** background color used for RGB transparency */
    RGBQUAD bkgnd_color;

    /**@name transparency management */
    //@{
    /**
    why another table ? for easy transparency table retrieval !
    transparency could be stored in the palette, which is better
    overall, but it requires quite some changes and it will render
    FreeImage_GetTransparencyTable obsolete in its current form;
    */
    BYTE transparent_table[256];
    /** number of transparent colors */
    int  transparency_count;
    /** TRUE if the image is transparent */
    BOOL transparent;
    //@}

    /** space to hold ICC profile */
    FIICCPROFILE iccProfile;

    /** contains a list of metadata models attached to the bitmap */
    METADATAMAP *metadata;

    /** FALSE if the FIBITMAP only contains the header and no pixel data */
    BOOL has_pixels;

    /** optionally contains a thumbnail attached to the bitmap */
    FIBITMAP *thumbnail;

    /** optionally contain the size of the bitmap data */
    DWORD global_size;

    /**@name external pixel buffer management */
    //@{
    /** pointer to user provided pixels, NULL otherwise */
    BYTE *external_bits;
    /** user provided pitch, 0 otherwise */
    unsigned external_pitch;
    //@}

    //BYTE filler[1];			 // fill to 32-bit alignment
};

// ----------------------------------------------------------
//  FREEIMAGERGBMASKS definition
// ----------------------------------------------------------

/**
RGB mask structure - mainly used for 16-bit RGB555 / RGB 565 FIBITMAP
*/
FI_STRUCT(FREEIMAGERGBMASKS) {
    unsigned red_mask;		//! bit layout of the red components
    unsigned green_mask;	//! bit layout of the green components
    unsigned blue_mask;		//! bit layout of the blue components
};
#endif
