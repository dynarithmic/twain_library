package com.dynarithmic.twain;

public class DTwainSourceInfo extends DTwainAppInfo
{
	public DTwainSourceInfo()
	{ super();	}
	
    public DTwainSourceInfo(String ver, String manu, String prodFamily, String prodName, int major, int minor)
    { 
    	super(ver, manu, prodFamily, prodName);
    	m_majorNum = major;
    	m_minorNum = minor;
   	}
    
    public int getMajorNum() { return m_majorNum; }
    public int getMinorNum() { return m_minorNum; }
    
    int m_majorNum;
    int m_minorNum;
}
/*
public class DTwainSourceInfo
{
   private final int GETMANU = 1;
   private final int GETPRODNAME = 2;
   private final int GETPRODFAMILY = 3;
   private final int GETVERINFO = 4;
   private final int GETSUPPORTEDCAPS = 5;
   private final int GETCUSTOMCAPS = 6;
   private final int GETEXTENDEDCAPS = 7;
   
   private DTwainSource m_ThisSource;

   private String getInternalInfo(int nWhich) throws DTwainRuntimeException
   {
       if ( m_ThisSource.getInterface() == null )
           throw new DTwainRuntimeException(DTwainConstants.DTWAIN_ERR_NOT_INITIALIZED);
       DTwainInterface theInterface = m_ThisSource.getInterface();
      switch (nWhich)
      {
          case GETMANU:
             return theInterface.getJNILink().getSourceManufacturer(m_ThisSource.getHandle());
          case GETPRODNAME:
             return theInterface.getJNILink().getSourceProductName(m_ThisSource.getHandle());
          case GETPRODFAMILY:
             return theInterface.getJNILink().getSourceProductFamily(m_ThisSource.getHandle());
          case GETVERINFO:
             return theInterface.getJNILink().getSourceVersionInfo(m_ThisSource.getHandle());
       }
       return "";
  }

   public DTwainSourceInfo(DTwainSource Source)
   {
       m_ThisSource = Source;
   }

   public DTwainSourceInfo()
   {
       m_ThisSource = null;
   }
   
   public DTwainSourceInfo attach(DTwainSource Source)
   {
       m_ThisSource = Source;
       return this;
   }
   
   public DTwainAppInfo getInfo() throws DTwainRuntimeException
   {
       DTwainAppInfo Info = new DTwainAppInfo();
       try
       {
           Info.setManufacturer(getManufacturer());
           Info.setProductFamily(getProductFamily());
           Info.setProductName(getProductName());
           Info.setVersionInfo(getVersionInfo());
           int lMajor, lMinor;
           lMajor = getMajorVersion();
           lMinor = getMinorVersion();
           Info.setMajorMinorVersion(lMajor, lMinor);
       }
       catch (DTwainRuntimeException e)
       {
           throw new DTwainRuntimeException(e.getError());
       }
       return Info;
   }

   public String getManufacturer() throws DTwainRuntimeException
   {
       String retval = "";
       try
       {
          retval = getInternalInfo(GETMANU);
       }
       catch (DTwainRuntimeException e)
       {
           throw new DTwainRuntimeException(e.getError());
       }
       return retval;
   }

   public String getProductName() throws DTwainRuntimeException
   {
       String retval = "";
       try
       {
           retval = getInternalInfo(GETPRODNAME);
       }
       catch (DTwainRuntimeException e)
       {
           throw new DTwainRuntimeException(e.getError());
       }
       return retval;
   }

   public String getProductFamily() throws DTwainRuntimeException
   {
       String retval = "";
       try
       {
           retval = getInternalInfo(GETPRODFAMILY);
       }
       catch (DTwainRuntimeException e)
       {
           throw new DTwainRuntimeException(e.getError());
       }
       return retval;     
   }

   public String getVersionInfo() throws DTwainRuntimeException
   {
       String retval = "";
       try
       {
           retval = getInternalInfo(GETVERINFO);
       }
       catch (DTwainRuntimeException e)
       {
           throw new DTwainRuntimeException(e.getError());
       }
       return retval;     
   }

   public int getMajorVersion() throws DTwainRuntimeException
   {
       int retval;
       DTwainInterface theInterface = m_ThisSource.getInterface();
       if ( theInterface == null )
           throw new DTwainRuntimeException(DTwainConstants.DTWAIN_ERR_NOT_INITIALIZED);
       return theInterface.getJNILink().getSourceMajorVersion(m_ThisSource.getHandle());
   }

   public int getMinorVersion() throws DTwainRuntimeException
   {
       int retval;
       DTwainInterface theInterface = m_ThisSource.getInterface();
       if ( theInterface == null )
           throw new DTwainRuntimeException(DTwainConstants.DTWAIN_ERR_NOT_INITIALIZED);
       return theInterface.getJNILink().getSourceMinorVersion(m_ThisSource.getHandle());
   }
   
}

*/