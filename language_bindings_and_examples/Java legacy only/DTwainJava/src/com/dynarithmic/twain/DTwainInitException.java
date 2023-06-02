package com.dynarithmic.twain;
public class DTwainInitException extends Exception
{
	 /**
	 * 
	 */
	private static final long serialVersionUID = -4228468040012881124L;
	String message;
	 
	 public DTwainInitException(String reason)
	 {
		 message = reason;
	 }
	 
	 public String getMessage()
	 {
		 return message;
	 }
}
