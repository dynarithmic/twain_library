## Table of Contents

- [DTWAIN supported Single-Page Image File Types](#dtwain-supported-single-page-image-file-types)
- [DTWAIN supported Multipage Image File Types](#supported-multipage-image-file-types)
- [Device-Native File Transfer Formats](#device-native-file-transfer-formats)
 
<a id="dtwain-supported-single-page-image-file-types"></a>
## DTWAIN supported Single-Page Image File Types

DTWAIN supports acquiring images to the following single-page file formats. **All formats listed below are supported for all TWAIN devices**, regardless of whether the device provides native support for the particular image format.

- **BMP (Windows Bitmap)**
  - `DTWAIN_BMP` — Standard Windows Bitmap
  - `DTWAIN_BMPRLE` — Run-Length Encoded (RLE) Windows Bitmap
- **GIF (Graphics Interchange Format)** — `DTWAIN_GIF`
- **JPEG (Joint Photographic Experts Group)** — `DTWAIN_JPEG`
- **JPEG 2000** — `DTWAIN_JPEG2000`
- **JPEG XL** — `DTWAIN_JPEGXL`
  - Requires additional JPEG XL runtime DLLs to be installed and accessible.
  - Required libraries include `jxl.dll`, `jxl_threads.dll`, `jxl_cms.dll`, and their dependent libraries, such as the Brotli DLLs (`brotlidec.dll`, `brotlienc.dll`, and `brotlicommon.dll`).
  - Applications can use `DTWAIN_SetJXLLibraryPath()` to specify the directory containing the JPEG XL libraries.
  - If no directory is specified, DTWAIN uses the default Windows DLL search mechanism.
  - Applications can call `DTWAIN_IsFileTypeAvailable(DTWAIN_JPEGXL)` to determine whether JPEG XL support is available.
- **JPEG XR** — `DTWAIN_JPEGXR`
- **PNG (Portable Network Graphics)** — `DTWAIN_PNG`
- **PCX (ZSoft Paintbrush)** — `DTWAIN_PCX`
- **PDF (Portable Document Format)** — `DTWAIN_PDF`
- **Adobe Photoshop Document (PSD)** — `DTWAIN_PSD`
- **TARGA (Truevision TGA)**
  - `DTWAIN_TGA` — Standard TARGA image
  - `DTWAIN_TGARLE` — Run-Length Encoded (RLE) TARGA image
- **SVG (Scalable Vector Graphics)** — `DTWAIN_SVG`
- **SVGZ (Compressed Scalable Vector Graphics)** — `DTWAIN_SVGZ`
- **EMF (Enhanced Windows Metafile)** — `DTWAIN_EMF`
- **WMF (Windows Metafile)** — `DTWAIN_WMF`
- **PostScript** — Supports PostScript Levels 1, 2, and 3.
- **ICO (Windows Icon)** — `DTWAIN_ICO`
- **TIFF (Tagged Image File Format)** — `DTWAIN_TIFF`
  - No compression
  - LZW compression
  - Flate (Deflate/ZIP) compression
  - JPEG compression
  - CCITT Group 3 compression
  - PackBits compression
  - CCITT Group 4 compression
- **BigTIFF (Large TIFF Format)** — Supports the same compression methods as standard TIFF.
- **WebP (Google WebP Image Format)** — `DTWAIN_WEBP`
- **WBMP (Wireless Bitmap)** — `DTWAIN_WBMP`

### Additional Notes

- **Device independence:** DTWAIN handles image encoding independently of the TWAIN device. The device does not need native support for the destination file format.
- **JPEG XL dependencies:** JPEG XL is an optional format that requires the appropriate runtime DLLs. Applications can check availability using `DTWAIN_IsFileTypeAvailable(DTWAIN_JPEGXL)`.
- **BMP and TARGA compression:** Both formats support standard uncompressed output and RLE-compressed output through separate DTWAIN file-type constants.
- **TIFF and BigTIFF:** Multiple compression methods are available, subject to the image characteristics and compression requirements.
- **PostScript:** DTWAIN supports generating PostScript output compatible with Levels 1, 2, and 3.
- **Single-page output:** The formats listed above can be used to save individual acquired images. Some formats, such as TIFF and PDF, also support multipage output through DTWAIN's corresponding multipage file types.

----
<a id="supported-multipage-image-file-types"></a>
## Supported Multipage Image File Types

In addition to single-page image formats, DTWAIN supports acquiring multiple images into a single output file.

The following multipage formats are supported for all TWAIN devices, regardless of whether the device provides native support for these formats.

- **TIFF (Multipage Tagged Image File Format)**
  - Supports storing multiple acquired images in a single TIFF file.
  - Supports the following compression methods:
    - No compression
    - LZW compression
    - Flate (Deflate/ZIP) compression
    - JPEG compression
    - CCITT Group 3 compression
    - PackBits compression
    - CCITT Group 4 compression
  - **Each page in a multipage TIFF file can use a different compression method**, allowing applications to select the most appropriate compression for each acquired image.
  - Multipage TIFF uses DTWAIN file-type constants corresponding to the single-page TIFF formats.

- **PDF (Multipage Portable Document Format)**
  - Supports storing multiple acquired images as pages within a single PDF document.
  - Supports PDF document metadata, including:
    - Author
    - Title
    - Subject
    - Keywords
    - Other document properties
  - Supports PDF security and encryption options, including **AES-256 encryption**.
  - Applications can configure document properties and encryption settings through the DTWAIN API.
  - Uses DTWAIN multipage PDF file-type constants corresponding to the single-page PDF formats.

- **DCX (Multipage PCX)** — `DTWAIN_DCX`
  - Supports storing multiple PCX images within a single DCX file.
  - DCX is the multipage extension of the PCX image format.

### Additional Notes

- **Device independence:** Multipage file generation is handled by DTWAIN. The TWAIN device does not need native support for multipage TIFF, PDF, or DCX.
- **File-type constants:** Multipage TIFF and PDF use separate DTWAIN constants that are similar to their single-page counterparts. DCX uses `DTWAIN_DCX`.
- **Automatic page handling:** DTWAIN can combine multiple acquired images into a single multipage output file during acquisition.
- **Advanced format options:** TIFF supports page-specific compression, while PDF supports document metadata and encryption settings.
----
<a id="device-native-file-transfer-formats"></a>
## Device-Native File Transfer Formats

In addition to its built-in single-page and multipage file formats, DTWAIN supports acquiring images using the TWAIN device's internal file-transfer capabilities.

When this method is used, the TWAIN device is responsible for generating the image file in one of its natively supported formats, rather than having DTWAIN perform the image encoding.

- **Device-dependent format support:** Available file formats depend on the capabilities of the TWAIN device and its driver.
- **DTWAIN file-type constants:** All constants associated with device-native file transfers begin with the prefix `DTWAIN_FF_`.
- **File encoding:** The TWAIN device handles the encoding and generation of the output file.
- **Availability:** Not all TWAIN devices support file transfers. Applications should check whether the selected device supports the requested file-transfer format before attempting acquisition.

### Checking Device File Transfer Support

DTWAIN provides the `DTWAIN_IsFileXferSupported()` API function to determine whether a TWAIN device supports a particular device-native file transfer format.

This function is intended **exclusively for device-native file transfers**, using the `DTWAIN_FF_` file-type constants.

It does not determine whether DTWAIN can acquire images using its built-in single-page or multipage file formats.

----

### Important Distinction

- **DTWAIN-encoded formats:** Available for all TWAIN devices. DTWAIN handles the image encoding and file generation.
- **Device-native formats:** Available only when supported by the TWAIN device. The device handles the image encoding and file generation.

Applications should use `DTWAIN_IsFileXferSupported()` when checking support for a device-native file transfer format.

For DTWAIN's optional JPEG XL encoding, applications can instead use `DTWAIN_IsFileTypeAvailable(DTWAIN_JPEGXL)` to determine whether the required JPEG XL runtime libraries are available.
