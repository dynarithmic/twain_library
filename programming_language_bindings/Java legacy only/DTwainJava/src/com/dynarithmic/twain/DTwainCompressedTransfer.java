package com.dynarithmic.twain;
import java.util.Arrays;
public class DTwainCompressedTransfer
{
	private int preferredSize;
	private int minimumSize;
	private int maximumSize;
	private int compressionType = DTwainJavaAPIConstants.DTWAIN_CP_NONE;
	private int [] supportedCompressionTypes = null;
	private byte [] compressedStrip;
	private DTwainSource mSource;
        private DTwainBufferedStripInfo m_StripInfo;
	
	public DTwainCompressedTransfer(DTwainSource theSource) throws DTwainJavaAPIException, DTwainException
	{
		mSource = theSource;
		supportedCompressionTypes = mSource.getCompressionTypes();
		Arrays.sort(supportedCompressionTypes);
                try {
                    setCompressionType(compressionType);  // always set the default type
                    setBufferSize(preferredSize);
                }
                catch (DTwainJavaAPIException e)
                {
                    throw e;
                }
                catch (DTwainException e)
                {
                    throw e;
                }
	}
	
	public boolean isCompressionTypeSupported(int Compression)
	{
            int nFound = Arrays.binarySearch(supportedCompressionTypes, Compression);
            return( nFound >= 0 );
	}
	
	public boolean isTIFFCompressionSupported()
	{
		return isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_PACKBITS) ||
        	   isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP31D) ||
        	   isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP31DEOL) ||
        	   isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP32D) ||
        	   isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP4) ||
        	   isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_JPEG) ||
        	   isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_LZW) ||
        	   isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_JBIG);
	}
	
	public boolean isJPEGCompressionSupported()
	{
		return isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_JPEG);
	}
	
	public boolean isPNGCompressionSupported()
	{
		return isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_PNG);
	}
	
	public boolean isBMPCompressionSupported()
	{
		return isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_RLE4) ||
               isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_RLE8) ||
               isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_BITFIELDS);
	}
	
	public boolean isJBIGCompressionSupported()
	{
		return isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_JBIG);
	}
	
	public boolean isLZWCompressionSupported()
	{
		return isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_LZW);
	}
	
	public boolean isCompressionSupported()
	{
            int nCompressions = supportedCompressionTypes.length;
            if ( isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_NONE) )
            {
                return nCompressions != 1;
            }
            return nCompressions > 1;
	}
	
	public boolean setJPEGQuality(int quality) throws Exception
	{
            try {
		return mSource.setJPEGQuality(quality);
            }
            catch (DTwainRuntimeException e)
            {
                throw e;
            }
	}
	
	public int getBestCompressionType(int ct, boolean isMono)
        {
            // returns the best compression for a particular file type
            if (!isCompressionSupported())
		return DTwainJavaAPIConstants.DTWAIN_CP_NONE;
	    switch (ct) {
		case DTwainCompressionType.COMPRESS_BMP: {
                    if (!isBMPCompressionSupported())
			return DTwainJavaAPIConstants.DTWAIN_CP_NONE;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_RLE8))
			return DTwainJavaAPIConstants.DTWAIN_CP_RLE8;
		    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_RLE4))
			return DTwainJavaAPIConstants.DTWAIN_CP_RLE4;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_BITFIELDS))
			return DTwainJavaAPIConstants.DTWAIN_CP_BITFIELDS;
		}
		break;
		case DTwainCompressionType.COMPRESS_TIFF: {
                    if (!isTIFFCompressionSupported() )
			return DTwainJavaAPIConstants.DTWAIN_CP_NONE;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_LZW))
			return DTwainJavaAPIConstants.DTWAIN_CP_LZW;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_PACKBITS))
			return DTwainJavaAPIConstants.DTWAIN_CP_PACKBITS;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP4) && isMono)
			return DTwainJavaAPIConstants.DTWAIN_CP_GROUP4;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_JBIG))
			return DTwainJavaAPIConstants.DTWAIN_CP_JBIG;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_JPEG))
			return DTwainJavaAPIConstants.DTWAIN_CP_JPEG;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP32D) && isMono)
			return DTwainJavaAPIConstants.DTWAIN_CP_GROUP32D;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP31D) && isMono)
			return DTwainJavaAPIConstants.DTWAIN_CP_GROUP31D;
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_GROUP31DEOL) && isMono)
			return DTwainJavaAPIConstants.DTWAIN_CP_GROUP31DEOL;
		}
		break;

		case DTwainJavaAPIConstants.DTWAIN_CP_PNG:
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_PNG))
			return DTwainJavaAPIConstants.DTWAIN_CP_PNG;
                    return DTwainJavaAPIConstants.DTWAIN_CP_NONE;
                    
		case DTwainJavaAPIConstants.DTWAIN_CP_JPEG:
                    if (isJBIGCompressionSupported())
			return DTwainJavaAPIConstants.DTWAIN_CP_JBIG;
		
                    if (isJPEGCompressionSupported())
                        return DTwainJavaAPIConstants.DTWAIN_CP_JPEG;
		return DTwainJavaAPIConstants.DTWAIN_CP_NONE;
		
		case DTwainJavaAPIConstants.DTWAIN_CP_PACKBITS:
                    if (isCompressionTypeSupported(DTwainJavaAPIConstants.DTWAIN_CP_PACKBITS))
			return DTwainJavaAPIConstants.DTWAIN_CP_PACKBITS;
                    return DTwainJavaAPIConstants.DTWAIN_CP_NONE;
            }
            return ct;
	}
	
	public boolean setCompressionType(int compression) throws DTwainJavaAPIException, DTwainException
	{
            if ( !isCompressionTypeSupported(compression))
                return false;
            try 
            {
                if ( mSource.setCompression(compression)) 
                {
                    // set the max and minimum sizes
                    DTwainJavaAPI theInterface = mSource.getInterface();
                    m_StripInfo = theInterface.DTWAIN_CreateBufferedStripInfo(mSource.getHandle());
                    minimumSize = m_StripInfo.getMinimumSize();
                    maximumSize = m_StripInfo.getMinimumSize();
                    preferredSize = m_StripInfo.getPreferredSize();
                    compressionType = compression;
                }
                return true;
            }
            catch (DTwainJavaAPIException e)
            {
                throw e;
            }
        }
	
	public int getCompressionType()
	{
		return compressionType;
	}
	
	public int getMinimumBufferSize()
	{
		return minimumSize;
	}
	
	public int getMaximumBufferSize()
	{
		return maximumSize;
	}
	
	public int getPreferredSize()
	{
		return preferredSize;
	}
	
	public void setBufferSize(int size)
	{
		compressedStrip = new byte [size];
	}
	
	public int getBufferSize()
	{
		return compressedStrip.length;
		
	}
	public byte [] getStrip()
	{
		return compressedStrip;
	}
}