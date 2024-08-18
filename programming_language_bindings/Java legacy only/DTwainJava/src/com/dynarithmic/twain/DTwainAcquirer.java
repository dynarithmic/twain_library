package com.dynarithmic.twain;
import java.util.TreeMap;
public class DTwainAcquirer
{
    private DTwainSource m_TS;
    private DTwainAcquireParams acquireParams;
    private DTwainPDFOptions pdfOptions;
    private String[] m_aFileNames;
    private static int s_Handle = 0;
    private int m_Handle;
    private DTwainJavaAPI m_Interface;
    public static final int ALL_PAGES = -1;

    private DTwainAcquisitionArray theAcquisitions;
       
    private static final TreeMap<Integer, DTwainAcquirer> s_mapAcquirer = new TreeMap<Integer, DTwainAcquirer>();
    private static final TreeMap<Integer, Integer> s_mapFile = new TreeMap<Integer, Integer>();
    static
    {
        s_mapFile.put(DTwainFileType.FILETYPE_TIFFNONE, DTwainJavaAPIConstants.DTWAIN_TIFFNONEMULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_TIFFJPEG, DTwainJavaAPIConstants.DTWAIN_TIFFJPEG);
        s_mapFile.put(DTwainFileType.FILETYPE_TIFFPACKBITS, DTwainJavaAPIConstants.DTWAIN_TIFFPACKBITSMULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_TIFFDEFLATE, DTwainJavaAPIConstants.DTWAIN_TIFFDEFLATEMULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_TIFFG3, DTwainJavaAPIConstants.DTWAIN_TIFFG3MULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_TIFFG4, DTwainJavaAPIConstants.DTWAIN_TIFFG4MULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_TIFFLZW, DTwainJavaAPIConstants.DTWAIN_TIFFLZWMULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_PCX, DTwainJavaAPIConstants.DTWAIN_DCX);
        s_mapFile.put(DTwainFileType.FILETYPE_PDF, DTwainJavaAPIConstants.DTWAIN_PDFMULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_POSTSCRIPT1, DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT1MULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_POSTSCRIPT2, DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT2MULTI);
        s_mapFile.put(DTwainFileType.FILETYPE_POSTSCRIPT3, DTwainJavaAPIConstants.DTWAIN_POSTSCRIPT3MULTI);
   }

   public static DTwainAcquirer getAcquirer(final int Handle)
   {
      final Integer temp = new Integer(Handle);
      
      if ( s_mapAcquirer.containsKey(temp) )
      {
         return (DTwainAcquirer)s_mapAcquirer.get(temp);
      }
      return null;
    }

    public int getHandle()
    {
       return m_Handle;
    }

    public DTwainSource getSource()
    {
        return m_TS;
    }
    
    public DTwainAcquirer(final DTwainSource Source)
    {
       acquireParams = new DTwainAcquireParams();
       pdfOptions = new DTwainPDFOptions();
       m_TS = Source;
       m_Interface = Source.getInterface();
       m_aFileNames = new String[1];
       m_aFileNames[0] = "twain.bmp";
       m_Handle = s_Handle;
       s_mapAcquirer.put(new Integer(m_Handle), this);
       s_Handle++;
    }

    public void setParams(final DTwainAcquireParams Params)
    {
       acquireParams = Params;
    }

    public DTwainAcquireParams getParams()
    {
        return acquireParams;
    }
    
    public DTwainAcquirer setPDFOptions(final DTwainPDFOptions pdfoptions)
    {
        pdfOptions = pdfoptions;
        return this;
    }
    
    public DTwainAcquirer setAcquireType(final int AType)
    {
        acquireParams.setAcquireType( AType );
        return this;
    }

    public DTwainAcquirer setInterface(final DTwainJavaAPI theInterface)
    {
        m_Interface = theInterface;
        return this;
    }
    
/*    public boolean acquire(String filename, final int filetype) 
            throws DTwainException, DTwainJavaAPIException
    {
        if ( m_Interface == null || m_Interface.getLibraryHandle() == 0 )
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        if ( acquireParams.getAcquireType() == DTwainAcquireType.ACQUIRETYPE_ACQUIREINVALID)
            acquireParams.setAcquireType(DTwainAcquireType.ACQUIRETYPE_NATIVEFILE);

        m_aFileNames = new String [1]; 
        m_aFileNames[0] = filename;
        try {
            return acquireToFileEx(filetype);
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
    }
*/
    public boolean acquire(String[] filenames, final int filetype)
    	throws DTwainException, DTwainJavaAPIException
    {
        if ( m_Interface == null || m_Interface.getLibraryHandle() == 0 )
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        if ( acquireParams.getAcquireType() == DTwainAcquireType.ACQUIRETYPE_ACQUIREINVALID)
            acquireParams.setAcquireType(DTwainAcquireType.ACQUIRETYPE_NATIVEFILE);
        m_aFileNames = filenames;
        try {
            return acquireToFileEx(filetype);
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
    }

    public boolean acquire() throws DTwainException, DTwainJavaAPIException
    {
        if ( m_Interface == null || m_Interface.getLibraryHandle() == 0 )
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        acquireParams.setAcquireType( DTwainAcquireType.ACQUIRETYPE_NATIVE );
        try 
        {
            theAcquisitions = m_Interface.DTWAIN_AcquireNative(m_TS.getHandle(), acquireParams.getColorType(), acquireParams.getMaxPages(),
                                             acquireParams.isUseUI(), acquireParams.isCloseSource());
            if (theAcquisitions == null )
               return false;
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
        return true;
    }
    
    public boolean acquireCompressed(DTwainCompressedTransfer transfer) throws DTwainJavaAPIException, DTwainException
    {
        if ( m_Interface == null || m_Interface.getLibraryHandle() == 0 )
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        try {
            acquireParams.setAcquireType( DTwainAcquireType.ACQUIRETYPE_BUFFERED );
            m_Interface.DTWAIN_SetCompressionType(m_TS.getHandle(), acquireParams.getCompressionType(), true);
            theAcquisitions = m_Interface.DTWAIN_AcquireBuffered(m_TS.getHandle(), acquireParams.getColorType(),
                                                                 acquireParams.getMaxPages(), acquireParams.isUseUI(), 
                                                                 acquireParams.isCloseSource());

        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
        return true;
    }
    
    public boolean acquireToFileRaw(final String filename, final int filetype)
    {
/*        
        // Acquire to a raw image file using buffered memory transfer.
        // Source must support the raw file type.
        m_tTwainAcquireType = DTwainAcquireType.ACQUIRETYPE_BUFFERED;
        m_aFileNames = (String[])DTwainArrayResizer.arrayResize(m_aFileNames, 1);
        m_aFileNames[0] = filename;
        m_nCompressType = filetype;
        return acquireToFileEx(filetype, true);*/
        return  true;
    }

    private boolean acquireToFileEx(final int filetype) throws DTwainJavaAPIException
    {
    	acquireParams.setFileType ( filetype );
        int actualAcquireType = 0;
        switch (acquireParams.getAcquireType() )
        {
           case DTwainAcquireType.ACQUIRETYPE_NATIVE:
               actualAcquireType = DTwainAcquireType.ACQUIRETYPE_NATIVEFILE;
           break;
           case DTwainAcquireType.ACQUIRETYPE_BUFFERED:
               actualAcquireType = DTwainAcquireType.ACQUIRETYPE_BUFFERED;
           break;
           case DTwainAcquireType.ACQUIRETYPE_FILE:
               actualAcquireType = DTwainJavaAPIConstants.DTWAIN_USESOURCEMODE;
           default:
               return false;
        }
        try {
            return acquireToFileInternal(actualAcquireType);
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
    }

    private boolean acquireToFileInternal(int acquireType) throws DTwainJavaAPIException
    {
        try {
            setPDFOptions();
            long handle = m_TS.getHandle();
            if ( m_aFileNames != null && m_aFileNames[0] != null )
            	acquireType |= DTwainJavaAPIConstants.DTWAIN_USELONGNAME;
            else
            {
            	
            	m_aFileNames = new String [1];
            	m_aFileNames[0] = "";
            	acquireType |= DTwainJavaAPIConstants.DTWAIN_USEPROMPT;
            }
            m_Interface.DTWAIN_AcquireFileEx(handle, m_aFileNames, acquireParams.getFileType(),
                                             acquireType, acquireParams.getColorType(),
                                             acquireParams.getMaxPages(), acquireParams.isUseUI(),
                                             acquireParams.isCloseSource());
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
        return true;
    }

    private DTwainAcquirer setPDFOptions() throws DTwainJavaAPIException
    {
        long handle = m_TS.getHandle();
        try {
            m_Interface.DTWAIN_SetPDFAuthor(handle, pdfOptions.getAuthor());
            m_Interface.DTWAIN_SetPDFASCIICompression(handle, pdfOptions.isASCIICompression());
            m_Interface.DTWAIN_SetPDFEncryption(handle, pdfOptions.isEncryptionUsed(), pdfOptions.getUserPassword(),
                                                pdfOptions.getOwnerPassword(), pdfOptions.getPermissions(), pdfOptions.is128EncryptionUsed());
            m_Interface.DTWAIN_SetPDFSubject(handle, pdfOptions.getSubject());
            m_Interface.DTWAIN_SetPDFTitle(handle, pdfOptions.getTitle());
            m_Interface.DTWAIN_SetPDFKeywords(handle, pdfOptions.getKeywords());
            m_Interface.DTWAIN_SetPDFCreator(handle, pdfOptions.getCreator());
            m_Interface.DTWAIN_SetPDFOrientation(handle, pdfOptions.getOrientation());
            m_Interface.DTWAIN_SetPDFPageSize(handle, pdfOptions.getPageSizeOptions(), 
                                              pdfOptions.getCustomPageSizeW(), pdfOptions.getCustomPageSizeH());
            m_Interface.DTWAIN_SetPDFPageScale(handle, pdfOptions.getPageScaleOptions(), pdfOptions.getPageScaleX(),
                                              pdfOptions.getPageScaleY());
            m_Interface.DTWAIN_SetPDFJpegQuality(handle, pdfOptions.getJpegQuality());
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
        return this;
    }
    
    public DTwainAcquisitionArray getAcquisitions() { return this.theAcquisitions; }
}
   
