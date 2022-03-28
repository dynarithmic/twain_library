package com.dtwain.test;

import com.dynarithmic.twain.DTwainAcquisitionArray;
import com.dynarithmic.twain.DTwainJavaAPI;
import com.dynarithmic.twain.DTwainJavaAPIConstants;

public class SimpleAcquire 
{
	public void SimpleTest() throws Exception
	{
		DTwainJavaAPI api = new DTwainJavaAPI();
		api.DTWAIN_JavaSysInitialize();
		long TwainSource = api.DTWAIN_SelectSource();
		if ( TwainSource != 0 )
		{
			DTwainAcquisitionArray acqArray = api.DTWAIN_AcquireNative(TwainSource, DTwainJavaAPIConstants.DTWAIN_PT_RGB, 1, true, false);
			if (acqArray != null )
			{
				com.dtwain.test.displaydialogs.ImageDisplayDialog idg = new com.dtwain.test.displaydialogs.ImageDisplayDialog(acqArray.getAcquisitonArray(), DTwainJavaAPIConstants.DTWAIN_BMP, true);
				idg.setVisible(true);
			}
		}
	    api.DTWAIN_JavaSysDestroy();
	}

	public static void main(String [] args)
	{
		SimpleAcquire s = new SimpleAcquire();
		try 
		{
			s.SimpleTest();
		} 
		catch (Exception e) {
			e.printStackTrace();
		}
	}

}