package com.dynarithmic.twain;
import java.util.ArrayList;

public class DTwainLogger 
{
    private boolean activated;
    private int  m_logFlags = DTwainJavaAPIConstants.DTWAIN_LOG_ALL - DTwainJavaAPIConstants.DTWAIN_LOG_USEFILE; 
    private String m_logFile = "";

    private static ArrayList<DTwainLogger> allLoggers = new ArrayList<DTwainLogger>();    
    public DTwainLogger()
    { 
    	allLoggers.add(this);
    	activated = false;
    }

    public DTwainLogger(DTwainJavaAPI iFace)
    { 
    	allLoggers.add(this);
    	activated = false;
    }
    
    public void startLogger(DTwainJavaAPI iFace) throws DTwainJavaAPIException
    {
        try {
            startLoggerInternal(iFace);
            activated = true;
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
    }

    public void stopLogger() 
    {
        activated = false;
    }
    
    public boolean startLoggerInternal(DTwainJavaAPI iFace) throws DTwainJavaAPIException
    {
        if ( iFace != null )
        {
            if ( m_logFile != null && !m_logFile.isEmpty())
                m_logFlags = DTwainJavaAPIConstants.DTWAIN_LOG_ALL; 
            else
                m_logFlags = DTwainJavaAPIConstants.DTWAIN_LOG_ALL - DTwainJavaAPIConstants.DTWAIN_LOG_USEFILE; 
            try
            {
                iFace.DTWAIN_SetTwainLog(m_logFlags, m_logFile);
                iFace.DTWAIN_EnableMsgNotify(1);
                return true;
            }
            catch (DTwainJavaAPIException e)
            {
                throw e;
            }        
        }
        return false;
    }
    
    void setInterface(DTwainJavaAPI iFace)
    {  }
    
    void setLogFileName(String logFile)
    {  m_logFile = logFile; }
    
    public void activate()
    { activated = true; }

    public void deactivate()
    { activated = false; }

    public boolean isActivated()
    { return activated;  }
    
    public static void logEvent(String logMsg)
    {
        int nLoggers = allLoggers.size();
        for ( int i = 0; i < nLoggers; ++i )
        {
        	DTwainLogger theLogger = allLoggers.get(i);
        	if (theLogger.activated)
        		theLogger.onLogEvent(logMsg);
        }
    }
    
    public void onLogEvent(String logMsg) 
    {  System.out.print(logMsg); }
}
