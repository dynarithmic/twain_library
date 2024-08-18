package com.dynarithmic.twain;

import java.awt.Rectangle;
import java.util.NoSuchElementException;
import java.util.StringTokenizer;

public class DTwainSelectSourceComponent
{
	public static Rectangle SelectButtonBounds =  
		toRect(DTwainResourceContainer.getResources().getString("IDR_SELECT_RECT"));
	
	public static Rectangle CancelButtonBounds = 
		toRect(DTwainResourceContainer.getResources().getString("IDR_CANCEL_RECT"));
	
	public static Rectangle SelectListBounds = 
		toRect(DTwainResourceContainer.getResources().getString("IDR_LIST_RECT"));
	
	public static Rectangle SelectSourceDialogBounds = 
		toRect(DTwainResourceContainer.getResources().getString("IDR_SELECT_SOURCE_RECT"));
	
	public static Rectangle SelectStaticTextBounds = 
		toRect(DTwainResourceContainer.getResources().getString("IDR_SOURCES_TEXT_RECT"));
	
	static private Rectangle toRect(String sRect)
	 throws NoSuchElementException
	{
		StringTokenizer sTok = new StringTokenizer(sRect, " ,");
		String sNum [] = new String[4];
		Integer theInt = new Integer(0);
		int nNum[] = new int [4];
		Rectangle theRect = new Rectangle();
		try {
			for ( int i = 0; i < 4; ++i ) {
				nNum[i] = theInt.parseInt(sTok.nextToken());
			}
		}
		catch ( NoSuchElementException e ) {
			throw new NoSuchElementException();
		}
		return new Rectangle(nNum[0], nNum[1], nNum[2], nNum[3]);
	}
}