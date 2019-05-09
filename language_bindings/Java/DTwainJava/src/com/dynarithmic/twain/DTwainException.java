package com.dynarithmic.twain;

import java.util.ResourceBundle;

public class DTwainException extends Exception
{
    /**
	 * 
	 */
	private static final long serialVersionUID = -1783200622144195396L;
	String message;
    int errorcode;
	
    public DTwainException(int nCode)
    {
    	errorcode = nCode;
    	// get the error string 
    	ResourceBundle theBundle = DTwainResourceContainer.getResources();
    	String sCode = DTwainJavaAPIConstants.toString(errorcode);
    	if ( sCode != null )
    		message = theBundle.getString(sCode);
    	else
    		message = "No error code exists";
    }
	 
	 public String getMessage()
	 {
		 return message;
	 }
	 
	 public int getError()
	 {
		 return errorcode;
	 }
}	