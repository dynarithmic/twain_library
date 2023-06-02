package com.dynarithmic.twain;

import java.util.ResourceBundle;

public class DTwainRuntimeException extends DTwainJavaAPIException
{
	String message;
	int errorcode;
	
    public DTwainRuntimeException(int nCode)
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
