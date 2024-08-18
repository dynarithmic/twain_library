/*
 * DTWAIN32.java
 *
 * Created on May 7, 2002, 3:19 PM
 */

/**
 *
 * @version
 */
package com.dynarithmic.twain;
    // Frame

public class DTwainAppInfo
{
    public void setVersionInfo( String version )
    {
        m_sVersion = version;
    }

    public void setManufacturer( String manu )
    {
        m_sManufacturer = manu;
    }

    public void setProductFamily( String prodfam )
    {
        m_sProdFamily = prodfam;
    }

    public void setProductName( String prodname )
    {
        m_sProdName = prodname;
    }

    public String getVersionInfo()
    {
        return m_sVersion;
    }

    public String getManufacturer()
    {
        return m_sManufacturer;
    }

    public String getProductFamily()
    {
        return m_sProdFamily;
    }

    public String getProductName()
    {
        return m_sProdName;
    }


    public DTwainAppInfo()
    {
        m_sVersion = "<?>";
        m_sManufacturer = "<?>";
        m_sProdFamily = "<?>";
        m_sProdName = "<?>";
    }

    public DTwainAppInfo(String ver, String manu, String prodFamily, String prodName)
    {
    	m_sVersion = ver;
    	m_sManufacturer = manu;
    	m_sProdFamily = prodFamily;
    	m_sProdName = prodName;
    }
    
    private String m_sVersion;
    private String m_sManufacturer;
    private String m_sProdFamily;
    private String m_sProdName;
}
