package com.dynarithmic.twain;
public class DTwainInvalidImageDataException extends Exception
{
     /**
	 * 
	 */
	private static final long serialVersionUID = -865684313225030687L;
	String message;
     
     public DTwainInvalidImageDataException(String reason)
     {
         message = reason;
     }
     
     public String getMessage()
     {
         return message;
     }
}