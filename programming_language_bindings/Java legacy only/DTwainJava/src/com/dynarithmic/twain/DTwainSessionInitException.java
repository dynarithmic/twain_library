package com.dynarithmic.twain;
public class DTwainSessionInitException extends Exception
{
	 String message;
	 
	 public DTwainSessionInitException(String reason)
	 {
		 message = reason;
	 }
	 
	 public String getMessage()
	 {
		 return message;
	 }
}