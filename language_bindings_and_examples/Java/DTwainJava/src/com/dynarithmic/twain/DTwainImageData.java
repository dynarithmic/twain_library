package com.dynarithmic.twain;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.zip.DataFormatException;
import java.util.zip.Deflater;
import java.util.zip.Inflater;

public class DTwainImageData
{
   private byte[] dibdata;
   private long origDibHandle;
    
    public DTwainImageData()
    {    }
    
    public DTwainImageData(byte[] data)
    {
    	setImageData(data);
    }

    public void setDibHandle(long dibHandle)
    {
    	origDibHandle = dibHandle;
    }
    
    long getDibHandle() 
    {
    	return origDibHandle;
    }
    
    public void setImageData(byte[] data)
    {
        // Compress the bytes
        Deflater compressor = new Deflater();
        compressor.setLevel(Deflater.BEST_COMPRESSION);
        compressor.setInput(data);
        compressor.finish();
        
        // Create an expandable byte array to hold the compressed data.
        // You cannot use an array that's the same size as the orginal because
        // there is no guarantee that the compressed data will be smaller than
        // the uncompressed data.
        ByteArrayOutputStream bos = new ByteArrayOutputStream(data.length);
        
        // Compress the data
        byte[] buf = new byte[1024];
        while (!compressor.finished()) 
        {
            int count = compressor.deflate(buf);
            bos.write(buf, 0, count);
        }
        //      Get the compressed data
        dibdata = bos.toByteArray();
    }
    
    public void setImageDataType(int dType)
    {
        // origdatatype = dType;
    }
    
    public byte [] getImageData() throws DTwainException
    {
        // Decompress the bytes
        Inflater decompressor = new Inflater();
        decompressor.setInput(dibdata);
        
        // Create an expandable byte array to hold the decompressed data
        ByteArrayOutputStream bos = new ByteArrayOutputStream(dibdata.length);
        
        // Decompress the data
        byte[] buf = new byte[1024];
        while (!decompressor.finished()) 
        {
            try 
            {
                int count = decompressor.inflate(buf);
                bos.write(buf, 0, count);
            } catch (DataFormatException e) 
            {
            	throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_DECOMPRESSION);
            }
        }
        try 
        {
            bos.close();
        } catch (IOException e) 
        {
        	throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_DECOMPRESSION);
        }
        
        // Get the decompressed data
        return bos.toByteArray();        
    }
}