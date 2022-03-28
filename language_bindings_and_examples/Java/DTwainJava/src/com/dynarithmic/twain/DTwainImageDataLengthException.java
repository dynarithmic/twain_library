package com.dynarithmic.twain;
public class DTwainImageDataLengthException extends Exception
{
     /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	String message;
     
     public DTwainImageDataLengthException(String reason)
     {
         message = reason;
     }
     
     public String getMessage()
     {
         return message;
     }
}