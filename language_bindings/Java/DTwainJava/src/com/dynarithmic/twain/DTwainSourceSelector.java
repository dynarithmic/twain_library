package com.dynarithmic.twain;

public class DTwainSourceSelector {
	
    static public long selectSource(DTwainJavaAPI theInterface) throws DTwainException, DTwainJavaAPIException
    {
        if (theInterface == null || theInterface.getLibraryHandle() == 0 ) 
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        try {
            long value = theInterface.DTWAIN_SelectSource();
            return value;
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
    }

    static public long selectDefaultSource(DTwainJavaAPI theInterface) throws DTwainException, DTwainJavaAPIException
    {
        if (theInterface == null || theInterface.getLibraryHandle() == 0 ) 
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        try {
            long value = theInterface.DTWAIN_SelectDefaultSource();
            return value;
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
    }
    
    static public long selectSourceByName(DTwainJavaAPI theInterface, String name) throws DTwainException, DTwainJavaAPIException
    {
        if (theInterface == null || theInterface.getLibraryHandle() == 0 ) 
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        if ( name == null )
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_PARAM);
        try {
            long value = theInterface.DTWAIN_SelectSourceByName(name);
            return value;
        }
        catch (DTwainJavaAPIException e)
        {
            throw e;
        }
    }
}
