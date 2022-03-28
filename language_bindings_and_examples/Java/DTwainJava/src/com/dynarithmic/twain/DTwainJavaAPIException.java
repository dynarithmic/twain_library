package com.dynarithmic.twain;

public class DTwainJavaAPIException extends Exception 
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -1977774142175948521L;

	public String getMessage() 
		{ return "Unrecoverable error in DTWAINJavaAPI.DLL"; }
}
