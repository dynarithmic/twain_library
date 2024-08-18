package com.dtwain.test.fulldemo;
import com.dynarithmic.twain.*;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.OutputStream;

/* This is an example of a DTWAIN listener that will trap
   notifications while the image acquisition (page scanning) is 
   in operation.  Note that this class is derived from DTwainJavaAPIListener
   and overrides the onQueryPageDiscard to listen for this notification.
*/
public class DTwainDemoImageListener extends DTwainMessageListener 
{
    DTwainJavaAPI m_api;
    DTwainDemoFrame m_mainFrame;
    ByteArrayOutputStream m_compressedStream;
    DTwainBufferedStripInfo m_stripInfo;
    boolean m_CompressionOn;
    File m_CompressedFile = null;
    
    public void initCompressedStream(DTwainBufferedStripInfo stripInfo)
    {
        m_stripInfo = stripInfo;
        m_compressedStream.reset();
    }
    
    public void setFileHandle(File f) { m_CompressedFile = f; }
    
    public void appendBytesToCompressedStream(byte [] b)
    {
        for (int i = 0; i < b.length; ++i)
            m_compressedStream.write(b[i]); //, m_compressedStream.size(), b.length);
    }
    
    public ByteArrayOutputStream getStream()
    {
        return m_compressedStream; 
    }
    public DTwainDemoImageListener(DTwainDemoFrame mFrame)
    {
        super();
        m_mainFrame = mFrame;
        m_compressedStream = new ByteArrayOutputStream();
        m_CompressionOn = false;
        
        // this is instance of DTWAIN API.  We will need it if we
        // want to retrieve the image data and display in a window
        m_api = m_mainFrame.getTwainInterface();
    }
    
    public void enableCompression(boolean bSet)
    {
        m_CompressionOn = bSet;
    }
    
    public boolean isCompressionOn()
    {
        return m_CompressionOn;
    }
    
    @Override
    public int onTransferStripDone(int event, long sourceHandle)
    {
        if ( m_CompressionOn )
        {
            // get the buffered strip
            try
            {
                m_api.DTWAIN_GetBufferedStripData(sourceHandle, m_stripInfo);
                appendBytesToCompressedStream(m_stripInfo.getBufferedStripData());
            }
            catch (DTwainJavaAPIException e)
            {
            }
        }
        return 1;
    }
    
    public int onTransferDone(int event, long sourceHandle)
    {
        if ( m_CompressionOn )
        {
            // get the buffered strip
            try
            {
                m_api.DTWAIN_GetBufferedStripData(sourceHandle, m_stripInfo);
                appendBytesToCompressedStream(m_stripInfo.getBufferedStripData());
                
                // save data to file
                try
                {
                    if ( m_CompressedFile != null )
                    {
                        OutputStream outputStream = new FileOutputStream(m_CompressedFile);
                        m_compressedStream.writeTo(outputStream);
                        outputStream.close();
                    }
                }
                catch (Exception e)
                {
                }
                
            }
            catch (DTwainJavaAPIException e)
            {
            }
        }
        try
        {
            DTwainImageInfo iInfo = m_api.DTWAIN_GetImageInfo(sourceHandle);
            return 1;
        }
        catch (DTwainJavaAPIException e)
        {
        }
        return 1;
    }
    
    /* Sent by DTWAIN to query if we want to keep the image or not.
       Image is displayed in the DTWAINImageDisplayDialog dialog to
       aid the user into making decision.
    */
   public int onQueryPageDiscard(int event, long sourceHandle) 
   { 
       // if showing the preview image is off, then just keep the image by
       // returning 1
       if ( !m_mainFrame.isShowPreviewImage() )
           return 1;
       try
       {
           // get the image data that was acquired
          DTwainImageData imgData = m_api.DTWAIN_GetCurrentAcquiredImage(sourceHandle);
          
          // display the image data in the dialog
          DTwainImageDisplayDialog dlg =  new DTwainImageDisplayDialog(imgData, DTwainJavaAPIConstants.DTWAIN_BMP);
          dlg.setVisible(true);
          
          // if we pressed "Keep", then we must return 1 back to DTWAIN
          if ( dlg.isOkPressed() )
              return 1;
          
          // throw the image away, so return 0 back to DTWAIN
          return 0;
       }
       catch (DTwainJavaAPIException e)
       {
           System.out.println(e.getMessage());
       }
       return 1;
   }
}
