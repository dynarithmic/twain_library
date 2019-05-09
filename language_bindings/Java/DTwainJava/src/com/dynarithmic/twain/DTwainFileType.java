package com.dynarithmic.twain;
public class DTwainFileType
{
    // Constants
    public static final int FILETYPE_BMP  =  DTwainJavaAPIConstants.DTWAIN_BMP;
    public static final int FILETYPE_TIFFNONE =  DTwainJavaAPIConstants.DTWAIN_TIFFNONE;
    public static final int FILETYPE_TIFFJPEG =  DTwainJavaAPIConstants.DTWAIN_TIFFJPEG;
    public static final int FILETYPE_TIFFPACKBITS =  DTwainJavaAPIConstants.DTWAIN_TIFFPACKBITS;
    public static final int FILETYPE_TIFFDEFLATE =  DTwainJavaAPIConstants.DTWAIN_TIFFDEFLATE;
    public static final int FILETYPE_TIFFG3 =  DTwainJavaAPIConstants.DTWAIN_TIFFG3;
    public static final int FILETYPE_TIFFG4 =  DTwainJavaAPIConstants.DTWAIN_TIFFG4;
    public static final int FILETYPE_TIFFLZW =  DTwainJavaAPIConstants.DTWAIN_TIFFLZW;
    public static final int FILETYPE_PCX =  DTwainJavaAPIConstants.DTWAIN_PCX;
    public static final int FILETYPE_DCX =  DTwainJavaAPIConstants.DTWAIN_DCX;
    public static final int FILETYPE_GIF =  DTwainJavaAPIConstants.DTWAIN_GIF;
    public static final int FILETYPE_PDF =  DTwainJavaAPIConstants.DTWAIN_PDF;
    public static final int FILETYPE_POSTSCRIPT1 =  DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT1;
    public static final int FILETYPE_POSTSCRIPT2 =  DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT2;
    public static final int FILETYPE_POSTSCRIPT3 =  DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT3;
    public static final int FILETYPE_PNG  =  DTwainJavaAPIConstants.DTWAIN_PNG;
    public static final int FILETYPE_TGA  =  DTwainJavaAPIConstants.DTWAIN_TGA;
    public static final int FILETYPE_JPEG  =  DTwainJavaAPIConstants.DTWAIN_JPEG;
    public static final int FILETYPE_JPEG2000  =  DTwainJavaAPIConstants.DTWAIN_JPEG2000;
    public static final int FILETYPE_PSD  =  DTwainJavaAPIConstants.DTWAIN_PSD;
    public static final int FILETYPE_WMF  =  DTwainJavaAPIConstants.DTWAIN_WMF;
    public static final int FILETYPE_EMF  =  DTwainJavaAPIConstants.DTWAIN_EMF;
    public static final int FILETYPE_TIFFG3MULTI  =  DTwainJavaAPIConstants.DTWAIN_TIFFG3MULTI;
    public static final int FILETYPE_TIFFG4MULTI  =  DTwainJavaAPIConstants.DTWAIN_TIFFG4MULTI;
    public static final int FILETYPE_TIFFNONEMULTI  =  DTwainJavaAPIConstants.DTWAIN_TIFFNONEMULTI;
    public static final int FILETYPE_TIFFJPEGMULTI  =  DTwainJavaAPIConstants.DTWAIN_TIFFJPEGMULTI;
    public static final int FILETYPE_TIFFPACKBITSMULTI  =  DTwainJavaAPIConstants.DTWAIN_TIFFPACKBITSMULTI;
    public static final int FILETYPE_TIFFDEFLATEMULTI  =  DTwainJavaAPIConstants.DTWAIN_TIFFDEFLATEMULTI;
    public static final int FILETYPE_TIFFLZWMULTI  =  DTwainJavaAPIConstants.DTWAIN_TIFFLZWMULTI;
    public static final int FILETYPE_PDFMULTI  =  DTwainJavaAPIConstants.DTWAIN_PDFMULTI;
    public static final int FILETYPE_POSTSCRIPT1MULTI  =  DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT1MULTI;
    public static final int FILETYPE_POSTSCRIPT2MULTI  =  DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT2MULTI;
    public static final int FILETYPE_POSTSCRIPT3MULTI  =  DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT3MULTI;
    public static final int FILETYPE_TEXTMULTI  =  DTwainJavaAPIConstants.DTWAIN_TEXTMULTI;

    public static final int FILETYPE_SOURCEMODE_BMP = DTwainJavaAPIConstants.DTWAIN_FF_BMP;
    public static final int FILETYPE_SOURCEMODE_EXIF = DTwainJavaAPIConstants.DTWAIN_FF_EXIF;
    public static final int FILETYPE_SOURCEMODE_TIFF = DTwainJavaAPIConstants.DTWAIN_FF_TIFF;
    public static final int FILETYPE_SOURCEMODE_TIFFMULTI = DTwainJavaAPIConstants.DTWAIN_FF_TIFFMULTI;
    public static final int FILETYPE_SOURCEMODE_PNG = DTwainJavaAPIConstants.DTWAIN_FF_PNG;
    public static final int FILETYPE_SOURCEMODE_FPX = DTwainJavaAPIConstants.DTWAIN_FF_FPX;
    public static final int FILETYPE_SOURCEMODE_SPIFF = DTwainJavaAPIConstants.DTWAIN_FF_SPIFF;
    public static final int FILETYPE_SOURCEMODE_JFIF = DTwainJavaAPIConstants.DTWAIN_FF_JFIF;
    public static final int FILETYPE_SOURCEMODE_XBM = DTwainJavaAPIConstants.DTWAIN_FF_XBM;
    public static final int FILETYPE_SOURCEMODE_PICT = DTwainJavaAPIConstants.DTWAIN_FF_PICT;

    public static boolean isSourceModeType(int FileType)
    {
        return (FileType == FILETYPE_SOURCEMODE_BMP ||
                FileType == FILETYPE_SOURCEMODE_EXIF ||
                FileType == FILETYPE_SOURCEMODE_TIFF ||
                FileType == FILETYPE_SOURCEMODE_TIFFMULTI ||
                FileType == FILETYPE_SOURCEMODE_PNG ||
                FileType == FILETYPE_SOURCEMODE_FPX ||
                FileType == FILETYPE_SOURCEMODE_SPIFF ||
                FileType == FILETYPE_SOURCEMODE_JFIF ||
                FileType == FILETYPE_SOURCEMODE_XBM ||
                FileType == FILETYPE_SOURCEMODE_PICT);
    }
    
    public static boolean isMultiPageType(int FileType)
    {
    	return 
        FileType == FILETYPE_TIFFG3MULTI ||
        FileType == FILETYPE_TIFFG4MULTI ||
        FileType == FILETYPE_TIFFNONEMULTI   ||
        FileType == FILETYPE_TIFFJPEGMULTI   ||
        FileType == FILETYPE_TIFFPACKBITSMULTI ||
        FileType == FILETYPE_TIFFDEFLATEMULTI ||
        FileType == FILETYPE_PDFMULTI        ||
        FileType == FILETYPE_POSTSCRIPT1MULTI ||
        FileType == FILETYPE_POSTSCRIPT2MULTI ||
        FileType == FILETYPE_POSTSCRIPT3MULTI ||
        FileType == FILETYPE_TIFFLZWMULTI ||
        FileType == FILETYPE_DCX ||
        FileType == FILETYPE_TEXTMULTI;
   	
    }
}
