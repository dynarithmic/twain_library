package com.dynarithmic.twain;

import java.util.Arrays;
import java.util.TreeSet;
import java.util.TreeMap;

class DTwainCapabilitiesGeneral extends DTwainCapabilityHandler {

    private DTwainSource theSource = null;
    private boolean bExpandRanges = false;

    public void setExpandRanges(boolean bSet) {
        bExpandRanges = bSet;
    }

    public boolean getExpandRanges() {
        return bExpandRanges;
    }

    public DTwainSource getSource() {
        return theSource;
    }

    public void setSource(DTwainSource source) {
        theSource = source;
    }

    public boolean isSupported(int nCap) throws DTwainRuntimeException {
        if (theSource != null) {
            return theSource.isCapSupported(nCap);
        }
        return false;
    }

    public boolean resetCap(int nCap) throws DTwainRuntimeException {
        if (theSource != null) {
            try {
                return theSource.resetCap(nCap);
            } catch (DTwainJavaAPIException e) {
                return false;
            }
        }
        return false;
    }

    public boolean isSupported(String sCap) throws DTwainRuntimeException {
        return isSupported(DTwainCapabilityHandler.intValue(sCap));
    }
}

class DTwainCapabilitiesInt extends DTwainCapabilitiesGeneral {

    public int getSingleCapValue(int nCap, int getType) throws DTwainRuntimeException {
        int[] temp = getSource().getCapValuesInt(nCap, getType);
        if (temp.length == 1) {
            return temp[0];
        }
        return -1;
    }

    public int[] getMultipleCapValues(int nCap, int getType) throws DTwainRuntimeException {
        int[] temp = getSource().getCapValuesInt(nCap, getType);
        if (getExpandRanges() && getSource().isRangeType(nCap)) {
            DTwainRangeInt range = new DTwainRangeInt(temp);
            return range.expand();
        }
        return temp;
    }

    public boolean setSingleCapValue(int nCap, int nValue) throws DTwainRuntimeException {
        int nItem[] = new int[1];
        nItem[0] = nValue;
        return getSource().setCapValuesInt(nCap, nItem, DTwainJavaAPIConstants.DTWAIN_CAPSET);
    }

    public boolean setMultipleCapValues(int nCap, int[] nValues) throws DTwainRuntimeException {
        return getSource().setCapValuesInt(nCap, nValues, DTwainJavaAPIConstants.DTWAIN_CAPSETAVAILABLE);
    }

    public DTwainCapabilitiesInt(DTwainSource source) {
        setSource(source);
    }
}

class DTwainCapabilitiesDouble extends DTwainCapabilitiesGeneral {

    public double getSingleCapValue(int nCap, int getType) throws DTwainRuntimeException {
        try {
            double[] temp = getSource().getCapValuesDouble(nCap, getType);
            if (temp.length >= 1) {
                return temp[0];
            }
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_CAP_NO_SUPPORT);
        } catch (DTwainRuntimeException e) {
            throw new DTwainRuntimeException(e.getError());
        }
    }

    public double[] getMultipleCapValues(int nCap, int getType) {
        double[] temp = getSource().getCapValuesDouble(nCap, getType);
        if (getExpandRanges() && getSource().isRangeType(nCap)) {
            DTwainRangeDouble range = new DTwainRangeDouble(temp);
            return range.expand();
        }
        return temp;
    }

    public boolean setSingleCapValue(int nCap, double nValue) {
        double nItem[] = new double[1];
        nItem[0] = nValue;
        try {
            return getSource().setCapValuesDouble(nCap, nItem, DTwainJavaAPIConstants.DTWAIN_CAPSET);
        } catch (DTwainRuntimeException e) {
            return false;
        }
    }

    public boolean setMultipleCapValues(int nCap, double[] nValues) {
        try {
            return getSource().setCapValuesDouble(nCap, nValues, DTwainJavaAPIConstants.DTWAIN_CAPSETAVAILABLE);
        } catch (DTwainRuntimeException e) {
            return false;
        }
    }

    public DTwainCapabilitiesDouble(DTwainSource source) {
        setSource(source);
    }
}

class DTwainCapabilitiesString extends DTwainCapabilitiesGeneral {

    public String getSingleCapValue(int nCap, int getType) throws DTwainRuntimeException {
        try {
            try {
                String[] temp = getSource().getCapValuesString(nCap, getType);
                if (temp.length >= 1) {
                    return temp[0];
                }
            } catch (DTwainJavaAPIException e) {
                return null;
            }
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_CAP_NO_SUPPORT);
        } catch (DTwainRuntimeException e) {
            throw new DTwainRuntimeException(e.getError());
        }
    }

    public String[] getMultipleCapValues(int nCap, int getType) throws DTwainRuntimeException {
        try {
            String[] temp = getSource().getCapValuesString(nCap, getType);
            return temp;
        } catch (DTwainJavaAPIException e1) {
            return null;
        }
    }

    public boolean setSingleCapValue(int nCap, String nValue) throws DTwainRuntimeException {
        try {
            String nItem[] = new String[1];
            nItem[0] = nValue;
            return getSource().setCapValuesString(nCap, nItem, DTwainJavaAPIConstants.DTWAIN_CAPSET);
        } catch (DTwainRuntimeException e) {
            throw new DTwainRuntimeException(e.getError());
        }
    }

    public boolean setMultipleCapValues(int nCap, String[] nValues) {
        try {
            return getSource().setCapValuesString(nCap, nValues, DTwainJavaAPIConstants.DTWAIN_CAPSETAVAILABLE);
        } catch (DTwainRuntimeException e) {
            return false;
        }
    }

    public DTwainCapabilitiesString(DTwainSource source) {
        setSource(source);
    }

}

class DTwainCapabilitiesFrame extends DTwainCapabilitiesGeneral {

    public DTwainFrame getSingleCapValue(int nCap, int getType) throws DTwainRuntimeException {
        try {
            try {
                DTwainFrame[] temp = getSource().getCapValuesFrame(nCap, getType);
                if (temp.length >= 1) {
                    return temp[0];
                }
            } catch (DTwainJavaAPIException e) {
                return null;
            }
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_CAP_NO_SUPPORT);
        } catch (DTwainRuntimeException e) {
            throw new DTwainRuntimeException(e.getError());
        }
    }

    public DTwainFrame[] getMultipleCapValues(int nCap, int getType) throws DTwainRuntimeException {
        try {
            DTwainFrame[] temp = getSource().getCapValuesFrame(nCap, getType);
            return temp;
        } catch (DTwainJavaAPIException e1) {
            return null;
        }
    }

    public boolean setSingleCapValue(int nCap, DTwainFrame nValue) throws DTwainRuntimeException {
        try {
            DTwainFrame nItem[] = new DTwainFrame[1];
            nItem[0] = nValue;
            return getSource().setCapValuesFrame(nCap, nItem, DTwainJavaAPIConstants.DTWAIN_CAPSET);
        } catch (DTwainJavaAPIException e) {
            return false;
        }
    }

    public boolean setMultipleCapValues(int nCap, DTwainFrame[] nValues) {
        try {
            return getSource().setCapValuesFrame(nCap, nValues, DTwainJavaAPIConstants.DTWAIN_CAPSETAVAILABLE);
        } catch (DTwainJavaAPIException e) {
            return false;
        }
    }

    public DTwainCapabilitiesFrame(DTwainSource source) {
        setSource(source);
    }
}

/**
 * <p>
 The DTwainSource class describes the TWAIN device. To create an instance of
 the DTwainSource, the Source must be created using one of the
 DTwainInterface.select() methods.<p>
 Otherwise, a DTwainSource can be created from an integer handle value that is
 passed from the TWAIN enviroment to a user-defined DTwainListener callback
 function.
 */
public class DTwainSource 
{
    private static TreeMap<String, Object> s_SourceMap = new TreeMap<String, Object>();

    private final int GETMANU = 0;
    private final int GETPRODNAME = 1;
    private final int GETPRODFAMILY = 2;
    private final int GETVERINFO = 3;

    private long m_ThisSource;
    private DTwainJavaAPI m_Interface;
    private DTwainCapabilitiesInt intCaps = null;
    private DTwainCapabilitiesDouble doubleCaps = null;
    private DTwainCapabilitiesString stringCaps = null;
    private DTwainCapabilitiesFrame frameCaps = null;
    private DTwainSourceInfo m_SourceInfo = null;
    private DTwainAcquirer m_Acquirer = null;

    private class DTwainSourceCachedValues implements Cloneable 
    {
        public int[] sourceCaps = null;
        public int[] customCaps = null;
        public int[] extendedCaps = null;
        public String[] sourceInfo = null;
        public int sourceMajor;
        public int sourceMinor;
        private TreeSet<Integer> capRange = null;
        public byte[] dsData = null;
        private int preferredTransferSize;
        private int minimumTransferSize;
        private int maximumTransferSize;
        private int[] supportedCompressedTransferTypes = null;

        public Object clone() throws CloneNotSupportedException {
            return super.clone();
        }
    }

    private DTwainSourceCachedValues cacheValues = null;

    /**
     * @param theSource<p>
 Integer handle of the selected TWAIN device. This function can be used
 where the Source is only known by the integer representation instead of
 the DTwainSource instance.<p>
     * <p>
 The setSource() function initializes a DTwainSource instance with the
 handle of a Source. This
     *
     */
    public void setHandle(long theSource) {
        m_ThisSource = theSource;
    }

    /**
     * @return Integer handle representing the TWAIN Source.
     */
    public long getHandle() {
        return m_ThisSource;
    }

    /**
     * @return isValid() returns true if the handle describing the Source is
     * valid, otherwise false is returned.
     */
    public boolean isValid() {
        return m_ThisSource != 0;
    }

    public DTwainAcquirer getAcquirer()
    {
    	if (m_Acquirer == null )
    		m_Acquirer = new DTwainAcquirer(this);
    	return m_Acquirer;
    }
    
    public void setAcquirer(DTwainAcquirer acq)
    {
    	m_Acquirer = acq;
    }
    
    public String getManufacturer() {
        if (cacheValues.sourceInfo == null) {
            initializeSourceInfo();
        }
        return cacheValues.sourceInfo[GETMANU];
    }

    public String getProductName() {
        if (cacheValues.sourceInfo == null) {
            initializeSourceInfo();
        }
        return cacheValues.sourceInfo[GETPRODNAME];
    }

    public String getProductFamily() {
        if (cacheValues.sourceInfo == null) {
            initializeSourceInfo();
        }
        return cacheValues.sourceInfo[GETPRODFAMILY];
    }

    public String getVersionInfo() {
        if (cacheValues.sourceInfo == null) {
            initializeSourceInfo();
        }
        return cacheValues.sourceInfo[GETVERINFO];
    }

    public int getMajorVersion() {
        if (cacheValues.sourceInfo == null) {
            initializeSourceInfo();
        }
        return cacheValues.sourceMajor;
    }

    public int getMinorVersion() {
        if (cacheValues.sourceInfo == null) {
            initializeSourceInfo();
        }
        return cacheValues.sourceMinor;
    }

    private void initializeSourceInfo() {
        if (cacheValues == null) {
            cacheValues = new DTwainSourceCachedValues();
        }
        // see if info exists in treemap
        cacheValues.sourceInfo = new String[4];
        DTwainSourceInfo info;
        try {
            info = m_Interface.DTWAIN_GetSourceInfo(m_ThisSource);
            cacheValues.sourceInfo[GETMANU] = info.getManufacturer();
            cacheValues.sourceInfo[GETPRODNAME] = info.getProductName();
            cacheValues.sourceInfo[GETPRODFAMILY] = info.getProductFamily();
            cacheValues.sourceInfo[GETVERINFO] = info.getVersionInfo();
            cacheValues.sourceMajor = info.getMajorNum();
            cacheValues.sourceMinor = info.getMinorNum();

            if (intCaps == null) {
                intCaps = new DTwainCapabilitiesInt(this);
            }
            if (doubleCaps == null) {
                doubleCaps = new DTwainCapabilitiesDouble(this);
            }
            if ( frameCaps == null ) {
                frameCaps = new DTwainCapabilitiesFrame(this);
            }
            if ( stringCaps == null ) {
                stringCaps = new DTwainCapabilitiesString(this);
            }

            getSupportedCaps();
            getCompressionTypes();
        } catch (DTwainJavaAPIException e) {
        }

    }

    /**
     * @return The DTWAIN interface that is associated with the TWAIN Source
     */
    public DTwainJavaAPI getInterface() {
        return m_Interface;
    }

    /**
     * @param theInterface<p>
 The DTWAIN interface associated with the Source.

 The setInterface() function is used to set the DTWAIN interface
 associated with the TWAIN Source. Usually not needed to be called by
 application, since DTwainInterface.select() will set the DTwainSource
 interface pointer automatically.
     *
     *
     *
     */
    public void setInterface(DTwainJavaAPI theInterface) {
        m_Interface = theInterface;
    }

    /**
     * @param nCap One of the DTwainJavaAPIConstants.DTWAIN_CV_xxxx capability
     * values.
     * @return returns true if the Source supports the capability described by
     * <i>nCap</i>, false otherwise.
     */
    public boolean isCapSupported(int nCap) {
        if (cacheValues == null || cacheValues.sourceCaps == null) {
            return false;
        }
        int nFound = Arrays.binarySearch(cacheValues.sourceCaps, nCap);
        return (nFound >= 0);
    }

    public boolean isRangeType(int nCap) {
        if (isCapSupported(nCap)) {
            return (cacheValues.capRange.contains(nCap));
        }
        return false;
    }

    public DTwainSourceInfo getSourceInfo() {
        if (m_Interface != null) 
        {
            if (m_SourceInfo == null) 
            {
                try 
                {
                    m_SourceInfo = m_Interface.DTWAIN_GetSourceInfo(m_ThisSource);
                } 
                catch (DTwainJavaAPIException e) 
                {
                    return null;
                }
            }
            return m_SourceInfo;
        }
        return null;
    }

    /**
     * @return An int array denoting all supported capabilities of the TWAIN
 Source. Each integer value is equivalent to the DTwainJavaAPICapability
 constants.
     */
    public int[] getSupportedCaps() {
    	// This function triggers the getting of all the cached values
        // See if this source has values already cached.
        if (cacheValues == null) {
            cacheValues = new DTwainSourceCachedValues();
        }
        if (cacheValues.sourceCaps != null) {
            return cacheValues.sourceCaps;
        }
        try {
            cacheValues.sourceCaps = m_Interface.DTWAIN_EnumSupportedCaps(m_ThisSource);
            cacheValues.customCaps = m_Interface.DTWAIN_EnumCustomCaps(m_ThisSource);
            cacheValues.extendedCaps = m_Interface.DTWAIN_EnumExtendedCaps(m_ThisSource);
            Arrays.sort(cacheValues.sourceCaps);
            Arrays.sort(cacheValues.extendedCaps);
            Arrays.sort(cacheValues.customCaps);

            // get caps that have range containers
            if (cacheValues.capRange == null) {
                cacheValues.capRange = new TreeSet<Integer>();
            }
            for (int i = 0; i < cacheValues.sourceCaps.length; ++i) {
                if (m_Interface.DTWAIN_GetCapContainer(m_ThisSource, cacheValues.sourceCaps[i], DTwainJavaAPIConstants.DTWAIN_CAPGET)
                        == DTwainJavaAPIConstants.DTWAIN_CONTRANGE) {
                    cacheValues.capRange.add(cacheValues.sourceCaps[i]);
                }
            }

            // custom ds data
            if (isCapSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCUSTOMDSDATA)) {
                cacheValues.dsData = m_Interface.DTWAIN_GetCustomDSData(m_ThisSource);
            }

            return cacheValues.sourceCaps;
        } catch (DTwainJavaAPIException e) {
            return null;
        }
    }

    public int[] getCompressionTypes() {
        if (cacheValues.supportedCompressedTransferTypes == null) {
            try {
                if (isCapSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOMPRESSION)) {
                    cacheValues.supportedCompressedTransferTypes = getAllCompressions();
                }
            } catch (DTwainRuntimeException e) {
                return null;
            }
        }
        return cacheValues.supportedCompressedTransferTypes;
    }

    public int[] getCustomCaps() {
        getSupportedCaps();
        return cacheValues.customCaps;
    }

    public int[] getExtendedCaps() {
        getSupportedCaps();
        return cacheValues.extendedCaps;
    }

    public int[] getInternalFileFormats() {
        return getCapValuesInt(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    /**
     * @param nCap The capability to retrieve the data type of.<p>
     * @return
     * <ul>
     * <li>&nbsp;&quot;<b>java.lang.Integer</b>&quot; if the capability values
     * of <i>nCap</i>
     * are represented as Integers.</li>
     * <li>&nbsp;&quot;<b>java.lang.String</b>&quot; if the capability values of
     * <i>nCap</i>
     * are represented as Strings.</li>
     * <li>&nbsp;&quot;<b>java.lang.Double</b>&quot; if the capability values of
     * <i>nCap</i>
     * are represented as Doubles.</li>
     * <li>&nbsp;&quot;<b>com.dynarithmic.DTwainFrame</b>&quot; if the
     * capability values of <i>
     * nCap</i><span> </span>are DTwainFrames.</li>
     * </ul>
     *
     * @throws DTwainJavaAPIException
     * @throws DTwainRuntimeException
     */
    public String getCapValueType(int nCap)
            throws DTwainRuntimeException, DTwainJavaAPIException {
        if (m_Interface == null) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_HANDLE);
        }
        if (m_ThisSource == 0) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_SOURCE);
        }
        try {
            int capType = m_Interface.DTWAIN_GetCapArrayType(m_ThisSource, nCap);
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYLONG) {
                return "java.lang.Integer";
            }
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYSTRING) {
                return "java.lang.String";
            }
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYFLOAT) {
                return "java.lang.Double";
            }
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYFRAME) {
                return "com.dynarithmic.DTwainFrame";
            }
        } catch (DTwainJavaAPIException e) {
            throw e;
        }
        return "";
    }

    public String getNameFromCap(int nCap) {
        try {
            return m_Interface.DTWAIN_GetNameFromCap(nCap);
        } catch (DTwainJavaAPIException e) {
            return "";
        }
    }

    public int getCapFromName(String sName) throws DTwainJavaAPIException {
        if (m_Interface == null) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_HANDLE);
        }
        if (m_ThisSource == 0) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_SOURCE);
        }
        return DTwainCapabilityHandler.intValue(sName);
    }

    public boolean resetCap(int nCap) throws DTwainJavaAPIException {
        if (m_Interface == null) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_HANDLE);
        }
        if (m_ThisSource == 0) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_SOURCE);
        }
        try {
            return m_Interface.DTWAIN_ResetCapValues(m_ThisSource, nCap) == 1;
        } catch (DTwainJavaAPIException e) {
            throw e;
        }
    }

    /**
     * @param nCap Capabiity to retrive the values of. Must be one of the
 DTwainJavaAPICapability constants, or a custom capability defined by
 DTwainJavaAPICapability.CAP_CUSTOMBASE + x, where x is some integer.
     * @param values The returned values of the capability. Use getValueType()
     * to determine the types stored in the Vector.
     * @param getType One of the following:<ul>
     * <li>&nbsp;To get the supported value(s) of the capability:
     * (DTwainJavaAPIConstants.DTWAIN_CAPGET)</li>
     * <li>&nbsp;To get the default value(s) of the capability:
     * (DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT)</li>
     * <li>&nbsp;To get the current value(s) of the capability:
     * (DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT)<br>
     * </li>
     * </ul>
     *
     * @return true if successful, false if unsuccessful
     * @throws DTwainNoTwainSourceException
     * @throws DTwainNoInterfaceException
     */
    private boolean initAndCheckCapType(int nCap, int aType) {
        if (m_Interface == null) {
            return false;
        }
        if (m_ThisSource == 0) {
            return false;
        }

        if (!isCapSupported(nCap)) {
            return false;
        }

        int capType = 0;
        try {
            capType = m_Interface.DTWAIN_GetCapArrayType(m_ThisSource, nCap);
        } catch (DTwainJavaAPIException e) {
            return false;
        }
        return capType == aType;
    }

    public int[] getCapValuesInt(int nCap, int getType) {
        try {
            initAndCheckCapType(nCap, DTwainJavaAPIConstants.DTWAIN_ARRAYLONG);
            return m_Interface.DTWAIN_GetCapValuesInt(m_ThisSource, nCap, getType);
        } catch (DTwainJavaAPIException e) {
            return null;
        }
    }

    public double[] getCapValuesDouble(int nCap, int getType) {
        try {
            initAndCheckCapType(nCap, DTwainJavaAPIConstants.DTWAIN_ARRAYFLOAT);
            return m_Interface.DTWAIN_GetCapValuesDouble(m_ThisSource, nCap, getType);
        } catch (DTwainJavaAPIException e) {
            return null;
        }
    }

    public String[] getCapValuesString(int nCap, int getType) throws DTwainRuntimeException, DTwainJavaAPIException {
        try {
            initAndCheckCapType(nCap, DTwainJavaAPIConstants.DTWAIN_ARRAYSTRING);
            return m_Interface.DTWAIN_GetCapValuesString(m_ThisSource, nCap, getType);
        } catch (DTwainRuntimeException e) {
            throw e;
        }
    }

    public DTwainFrame[] getCapValuesFrame(int nCap, int getType) throws DTwainRuntimeException, DTwainJavaAPIException {
        try {
            initAndCheckCapType(nCap, DTwainJavaAPIConstants.DTWAIN_ARRAYFRAME);
            return m_Interface.DTWAIN_GetCapValuesFrame(m_ThisSource, nCap, getType);
        } catch (DTwainRuntimeException e) {
            throw e;
        }
    }

    /**
     * @param nCap Capability to set the values of. Must be one of the
 DTwainJavaAPICapability constants, or a custom capability defined by
 DTwainJavaAPICapability.CAP_CUSTOMBASE + x, where x is some integer.
     * @param values The capability values. Use getValueType() to determine the
     * types that you must store in the Vector.
     * @param setType One of the following:<ul>
     * <li>&nbsp;To set the current value of the capability:
     * (DTwainJavaAPIConstants.DTWAIN_CAPSET)</li>
     * <li>&nbsp;To set a series of capability values:
     * (DTwainJavaAPIConstants.DTWAIN_CAPSETALL)</li>
     * </li>
     * </ul>
     * @return true if successful, false if unsuccessful
     * @throws DTwainNoTwainSourceException
     * @throws DTwainNoInterfaceException
     */
    private void PreTest(int nCap) throws DTwainRuntimeException {
        if (m_Interface == null) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_HANDLE);
        }
        if (m_ThisSource == 0) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_SOURCE);
        }
        if (!isCapSupported(nCap)) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_CAP_NO_SUPPORT);
        }
    }

    private void PreTest() throws DTwainRuntimeException {
        PreTest(true, true);
    }

    private void PreTest(boolean testInterface, boolean testSource) throws DTwainRuntimeException {
        if (testInterface && m_Interface == null) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_HANDLE);
        }
        if (testSource && m_ThisSource == 0) {
            throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_SOURCE);
        }
    }
    
    public boolean setCapValuesInt(int nCap, int[] values, int setType) throws DTwainRuntimeException {
        PreTest(nCap);
        try {
            int capType = m_Interface.DTWAIN_GetCapArrayType(m_ThisSource, nCap);
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYLONG) {
                return m_Interface.DTWAIN_SetCapValuesInt(m_ThisSource, nCap, setType, values) == 1;
            }
        } catch (DTwainJavaAPIException e) {
            return false;
        }
        throw new DTwainRuntimeException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_CAPTYPE);
    }

    public boolean setCapValuesDouble(int nCap, double[] values, int setType) throws DTwainRuntimeException {
        PreTest(nCap);
        try {
            int capType = m_Interface.DTWAIN_GetCapArrayType(m_ThisSource, nCap);
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYFLOAT) {
                return m_Interface.DTWAIN_SetCapValuesDouble(m_ThisSource, nCap, setType, values) == 1;
            }
        } catch (DTwainJavaAPIException e) {
            return false;
        }
        return false;
    }

    public boolean setCapValuesString(int nCap, String[] values, int setType) throws DTwainRuntimeException {
        PreTest(nCap);
        try {
            int capType = m_Interface.DTWAIN_GetCapArrayType(m_ThisSource, nCap);
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYSTRING) {
                return m_Interface.DTWAIN_SetCapValuesString(m_ThisSource, nCap, setType, values) == 1;
            }
        } catch (DTwainJavaAPIException e) {
            return false;
        }
        return false;
    }

    public boolean setCapValuesFrame(int nCap, DTwainFrame[] values, int setType) throws DTwainRuntimeException, DTwainJavaAPIException {
        PreTest(nCap);
        try {
            int capType = m_Interface.DTWAIN_GetCapArrayType(m_ThisSource, nCap);
            if (capType == DTwainJavaAPIConstants.DTWAIN_ARRAYFRAME) {
                return m_Interface.DTWAIN_SetCapValuesFrame(m_ThisSource, nCap, setType, values) == 1;
            }
        } catch (DTwainJavaAPIException e) {
            return false;
        }
        return false;
    }

    /*    public boolean setCapValues(int nCap, Vector values, int setType)
     throws  DTwainNoTwainSourceException,
     DTwainNoInterfaceException
     {
     if ( m_Interface == null )
     throw new DTwainNoInterfaceException("No DTwainInterface for DTwainRange (did you forget to call setInterface())?");
     if ( m_ThisSource == 0 )
     throw new DTwainNoTwainSourceException("No Twain Source associated with DTwainSource");
     if ( !isCapSupported( nCap ) )
     return false;

     int capType = m_Interface.getJNILink().getCapArrayType(m_ThisSource, nCap);
     if ( capType == DTwainJavaAPIConstants.DTWAIN_ARRAYLONG ) {
     int [] temp = {0};
     temp = (int[])DTwainArrayResizer.arrayResize(temp, values.size());
     for ( int i = 0; i < values.size(); ++i )
     temp[i] = ((Integer)(values.get(i))).intValue();

     if ( m_Interface.getJNILink().setCapValuesInt(m_ThisSource, nCap, temp, values.size(), setType) == 1 )
     return true;
     } else
     if ( capType == DTwainJavaAPIConstants.DTWAIN_ARRAYFLOAT ) {
     double [] temp = {0.0};
     temp = (double[])DTwainArrayResizer.arrayResize(temp, values.size());
     for ( int i = 0; i < values.size(); ++i )
     temp[i] = ((Double)(values.get(i))).doubleValue();

     if ( m_Interface.getJNILink().setCapValuesDouble(m_ThisSource, nCap, temp, values.size(), setType) == 1 )
     return true;
     }

     else
     if ( capType == DTwainJavaAPIConstants.DTWAIN_ARRAYSTRING ) {
     String [] temp = {""};
     temp = (String[])DTwainArrayResizer.arrayResize(temp, values.size());
     for ( int i = 0; i < values.size(); ++i )
     temp[i] = ((String)(values.get(i))).toString();

     if ( m_Interface.getJNILink().setCapValuesString(m_ThisSource, nCap, temp, values.size(), setType) == 1 )
     return true;
     } else
     if ( capType == DTwainJavaAPIConstants.DTWAIN_ARRAYFRAME ) {
     double [] temp = {0.0};
     temp = (double[])DTwainArrayResizer.arrayResize(temp, values.size() * 4);
     int j = 0;
     for ( int i = 0; i < values.size(); ++i ) {
     DTwainFrame f = (DTwainFrame)values.get(i);
     temp[j] = f.getLeft();
     temp[j+1] = f.getTop();
     temp[j+2] = f.getRight();
     temp[j+3] = f.getBottom();
     j += 4;
     }
     if ( m_Interface.getJNILink().setCapValuesFrame(m_ThisSource, nCap, temp, temp.length, setType) == 1 )
     return true;
     }
     return false;
     }
     */
    public double[] getResolutionValues() throws DTwainRuntimeException, DTwainJavaAPIException {
        PreTest();
        return m_Interface.DTWAIN_EnumResolutionValues(m_ThisSource, true);
    }

    /**
     * @return true if the TWAIN Source user interface is currently enabled,
     * false otherwise.
     * @throws com.dynarithmic.twain.DTwainJavaAPIException
     * @throws DTwainRuntimeException
     */
    public boolean isUIEnabled()
            throws DTwainRuntimeException, DTwainJavaAPIException {
        PreTest();
        return m_Interface.DTWAIN_IsUIEnabled(m_ThisSource);
    }

    /**
     * @return true if the TWAIN Source supports displaying the user interface
     * only to make changes to the default settings, not to retrieve images.
     * @throws com.dynarithmic.twain.DTwainJavaAPIException
     * @throws DTwainRuntimeException
     */
    public boolean isUIOnlySupported()
            throws DTwainRuntimeException, DTwainJavaAPIException {
        PreTest();
        return m_Interface.DTWAIN_IsUIOnlySupported(m_ThisSource);
    }

    /**
     * @return true if the TWAIN Source is currently acquiring images or if the
     * user interface is enabled, false otherwise.
     * @throws com.dynarithmic.twain.DTwainJavaAPIException
     * @throws DTwainRuntimeException
     */
    public boolean isAcquiring()
            throws DTwainRuntimeException, DTwainJavaAPIException {
        PreTest();
        return m_Interface.DTWAIN_IsSourceAcquiring(m_ThisSource);
    }

    public boolean close()
            throws DTwainRuntimeException, DTwainJavaAPIException {
        PreTest(true, false);
        if ( m_ThisSource != 0 )
        {
            boolean value = m_Interface.DTWAIN_CloseSource(m_ThisSource) == 1;
            if ( value )
                m_ThisSource = 0;
            return value;
        }
        return true;
    }

    public DTwainImageInfo getImageInfo() throws DTwainJavaAPIException {
        return m_Interface.DTWAIN_GetImageInfo(m_ThisSource);
    }

    public byte[] getCustomDSData() throws DTwainJavaAPIException 
    {
        if (isCustomDSDataSupported()) 
            return m_Interface.DTWAIN_GetCustomDSData(m_ThisSource);
        return null;
    }

    public DTwainBufferedStripInfo getAcquiredStripData() throws DTwainJavaAPIException {
        DTwainBufferedStripInfo dInfo = new DTwainBufferedStripInfo();
        m_Interface.DTWAIN_GetBufferedStripData(m_ThisSource, dInfo);
        return dInfo;
    }
    
    public void setExpandRanges(boolean bSet) {
        intCaps.setExpandRanges(bSet);
        doubleCaps.setExpandRanges(bSet);
        stringCaps.setExpandRanges(bSet);
        frameCaps.setExpandRanges(bSet);
    }

    public DTwainSource(long Source, DTwainJavaAPI theInterface) throws DTwainException 
    {
        if (theInterface == null || theInterface.getLibraryHandle() == 0 ) {
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED);
        }
        if (Source == 0) {
            throw new DTwainException(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_SOURCE);
        }

        m_Interface = theInterface;
        m_ThisSource = Source;
        DTwainSourceInfo info = getSourceInfo();
        String prodName = info.getProductName();
        cacheValues = (DTwainSourceCachedValues) s_SourceMap.get(prodName);
        intCaps = new DTwainCapabilitiesInt(this);
        doubleCaps = new DTwainCapabilitiesDouble(this);
        stringCaps = new DTwainCapabilitiesString(this);
        frameCaps = new DTwainCapabilitiesFrame(this);
        if (cacheValues == null) {
            initializeSourceInfo();
            try {
                s_SourceMap.put(prodName, cacheValues.clone());
            } catch (CloneNotSupportedException e) {
                System.out.println("Clone not supported");
            }
        }

        // Set up capability classes
    }

    /**
     * @param Source The handle of the Source. Use this constructor when
     * attempting to construct a DTwainSource object with just a handle value.
     */
    public DTwainSource(long Source) {
        m_ThisSource = Source;
        m_Interface = null;
    }

    /**
     * Constructs a DTwainSource with no associated TWAIN handle or TWAIN
     * interface. Use setHandle() and setInterface() to set the handle and
     * interface values of the Source.
     */
    public DTwainSource() {
        m_ThisSource = 0;
        m_Interface = null;
    }

    // Capabilities (integer / boolean)
    /////////////////////////// ACAP_AUDIOFILEFORMAT /////////////////////////////////
    public int[] getAllAudioFileFormats() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ACAPAUDIOFILEFORMAT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAudioFileFormat() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ACAPAUDIOFILEFORMAT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultAudioFileFormat() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ACAPAUDIOFILEFORMAT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setAudioFileFormat(int nFormat) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ACAPAUDIOFILEFORMAT, nFormat);
    }

    /////////////////////////// ACAP_XFERMECH /////////////////////////////////
    public int[] getAllAudioTransferMechanisms() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ACAPXFERMECH, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAudioTransferMechanism() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ACAPXFERMECH, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultAudioTransferMechanism() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ACAPXFERMECH, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setAudioTransferMechanism(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ACAPXFERMECH, value);
    }

    public boolean resetAudioTransferMechanism() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ACAPXFERMECH);
    }

    public boolean isAudioTransferMechanismSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ACAPXFERMECH);
    }

    /////////////////////////// CAP_ALARMS /////////////////////////////////
    public int[] getAllAlarms() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentAlarms() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultAlarms() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setAlarms(int[] values) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMS, values);
    }

    public boolean resetAlarms() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMS);
    }

    public boolean isAlarmsSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMS);
    }

    /////////////////////////// CAP_ALARMVOLUME /////////////////////////////////
    public int[] getAllAlarmVolumes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMVOLUME, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAlarmVolume() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMVOLUME, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultAlarmVolume() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMVOLUME, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setAlarmVolume(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMVOLUME, value);
    }

    public boolean resetAlarmVolume() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMVOLUME);
    }

    public boolean isAlarmVolumeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPALARMVOLUME);
    }

    /////////////////////////// CAP_AUTOFEED /////////////////////////////////

    public boolean getAutoFeed() throws DTwainRuntimeException {
        int val = intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOFEED, DTwainJavaAPIConstants.DTWAIN_CAPGET);
        return (val == 1);
    }

    public boolean getCurrentAutoFeed() throws DTwainRuntimeException {
        int val = intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOFEED, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
        return (val == 1);
    }

    public boolean getDefaultAutoFeed() throws DTwainRuntimeException {
        int val = intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOFEED, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
        return (val == 1);
    }

    public boolean setAutoFeed(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOFEED, bSet ? 1 : 0);
    }

    public boolean resetAutoFeed() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOFEED);
    }

    public boolean isAutoFeedSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOFEED);
    }

    ////////////////////////// CAP_AUTOMATICCAPTURE /////////////////////////////////
    public int[] getAllAutomaticCaptures() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICCAPTURE,
                DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAutomaticCapture() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICCAPTURE,
                DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultAutomaticCapture() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICCAPTURE,
                DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setAutomaticCapture(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICCAPTURE, value);
    }

    public boolean resetAutomaticCapture() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICCAPTURE);
    }

    public boolean isAutomaticCaptureSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICCAPTURE);
    }

    ////////////////////////// CAP_AUTOMATICSENSEMEDIUM /////////////////////////////////
    public int[] getAllAutomaticSenseMedium() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAutomaticSenseMedium() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultAutomaticSenseMedium() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM,
                DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setAutomaticSenseMedium(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM, value);
    }

    public boolean resetAutomaticSenseMedium() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM);
    }

    public boolean isAutomaticSenseMediumSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM);
    }

    /////////////////////////// CAP_AUTOSCAN /////////////////////////////////
    public boolean isAutoScan() throws DTwainRuntimeException {
        int val = intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOSCAN, DTwainJavaAPIConstants.DTWAIN_CAPGET);
        return val == 1;
    }

    public boolean setAutoScan(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOSCAN, bSet ? 1 : 0);
    }

    public boolean resetAutoScan() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOSCAN);
    }

    public boolean isAutoScanSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPAUTOSCAN);
    }

    /////////////////////////// CAP_BATTERYMINUTES /////////////////////////////////
    public int[] geAlltBatteryMinutes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPBATTERYMINUTES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBatteryMinute() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPBATTERYMINUTES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultBatteryMinute() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPBATTERYMINUTES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean resetBatteryMinutes() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPBATTERYMINUTES);
    }

    public boolean isBatteryMinutesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPBATTERYMINUTES);
    }

    /////////////////////////// CAP_BATTERYPERCENTAGE /////////////////////////////////
    public int getBatteryPercentage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPBATTERYPERCENTAGE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public boolean isBatteryPercentageSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPBATTERYPERCENTAGE);
    }

    ////////////////////////// CAP_CAMERAENABLED /////////////////////////////////
    public boolean isCameraEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean setCameraEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAENABLED, bSet ? 1 : 0);
    }

    public boolean isCameraEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAENABLED);
    }

    public boolean resetCameraEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAENABLED);
    }

    ////////////////////////// CAP_CAMERAORDER /////////////////////////////////
    public int[] getAllCameraOrder() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAORDER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentCameraOrder() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAORDER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultCameraOrder() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAORDER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setCameraOrder(int[] values) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAORDER, values);
    }

    public boolean resetCameraOrder() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAORDER);
    }

    public boolean isCameraOrderSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAORDER);
    }

    /////////////////////////// CAP_CAMERAPREVIEWUI /////////////////////////////////
    public boolean isCameraPreviewUI() throws DTwainRuntimeException {
        int val = intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAPREVIEWUI, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
        return val == 1;
    }

    public boolean isCameraPreviewUISupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCAMERAPREVIEWUI);
    }

    ////////////////////////// CAP_CLEARBUFFERS /////////////////////////////////////
    public int[]  getAllClearBuffers() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARBUFFERS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int getCurrentClearBuffers() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARBUFFERS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int getDefaultClearBuffers() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARBUFFERS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setClearBuffers(int bSet) throws DTwainRuntimeException, DTwainJavaAPIException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARBUFFERS, bSet);
    }
    
    public boolean resetClearBuffers() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARBUFFERS);
    }

    public boolean isClearBuffersSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARBUFFERS);
    }
    
    ////////////////////////// CAP_CLEARPAGE /////////////////////////////////////
    public int getClearPage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARPAGE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int getCurrentClearPage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARPAGE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int getDefaultClearPage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARPAGE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setClearPage(int bSet) throws DTwainRuntimeException, DTwainJavaAPIException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARPAGE, bSet);
    }
    
    public boolean resetClearPage() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARPAGE);
    }

    public boolean isClearPageSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCLEARPAGE);
    }

    ////////////////////////// CAP_CLEARPAGE /////////////////////////////////////
    public String getCustomInterfaceGUID() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCUSTOMINTERFACEGUID, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public String getCurrentCustomInterfaceGUID() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCUSTOMINTERFACEGUID, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultCustomInterfaceGUID() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCUSTOMINTERFACEGUID, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean isCustomInterfaceGUIDSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCUSTOMINTERFACEGUID);
    }

    ////////////////////////// CAP_CUSTOMDSDATA /////////////////////////////////////
    public boolean isCustomDSData() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPCUSTOMDSDATA, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean isCustomDSDataSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPCUSTOMDSDATA);
    }
    
    ////////////////////////// CAP_DEVICEEVENT /////////////////////////////////////
    public int[] getAllDeviceEvents() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEEVENT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentDeviceEvents() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEEVENT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int[] getDefaultDeviceEvents() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEEVENT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setDeviceEvents(int[] values) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEEVENT, values);
    }

    public boolean resetDeviceEvents() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEEVENT);
    }
    
    public boolean isDeviceEventsSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEEVENT);
    }
    ////////////////////////// CAP_DEVICEONLINE /////////////////////////////////////
    public boolean isDeviceOnline() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEONLINE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }
    
    public boolean isDeviceOnlineSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICEONLINE);
    }

    ////////////////////////// CAP_DEVICEDATETIME /////////////////////////////////////
    public String getDeviceDateTime() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICETIMEDATE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public String getCurrentDeviceDateTime() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICETIMEDATE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultDeviceDateTime() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICETIMEDATE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setDeviceDateTime(String bSet) throws DTwainRuntimeException {
        return stringCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICETIMEDATE, bSet);
    }
    
    public boolean resetDeviceDateTime() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICETIMEDATE);
    }
    
    public boolean isDeviceDateTimeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDEVICETIMEDATE);
    }

    ////////////////////////// CAP_DOUBLEFEEDDETECTION /////////////////////////////////////
    public int getDoubleFeedDectection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int getCurrentDoubleFeedDectection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultDoubleFeedDetection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setDoubleFeedDetection(String [] bSet) throws DTwainRuntimeException {
        return stringCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTION, bSet);
    }
    
    public boolean resetDoubleFeedDetection() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTION);
    }
    
    public boolean isDoubleFeedDetectionSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTION);
    }
    
    ////////////////////////// CAP_DOUBLEFEEDDETECTIONLENGTH /////////////////////////////////////
    public double[] getAllDoubleFeedDetectionLength() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentDoubleFeedDetectionLength() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultDoubleFeedDetectionLength() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setDoubleFeedDetectionLength(double bSet) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH, bSet);
    }
    
    public boolean resetDoubleFeedDetectionLength() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH);
    }
    
    public boolean isDoubleFeedDectectionLengthSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH);
    }

    ////////////////////////// CAP_DOUBLEFEEDDETECTIONRESPONSE /////////////////////////////////////
    public int[] getAllDoubleFeedDectectionResponse() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int getCurrentDoubleFeedDectectionResponse() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultDoubleFeedDetectionResponse() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setDoubleFeedDetectionResponse(int bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE, bSet);
    }
    
    public boolean resetDoubleFeedDetectionResponse() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE);
    }
    
    public boolean isDoubleFeedDetectionResponseSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE);
    }

    ////////////////////////// CAP_DOUBLEFEEDDETECTIONSENSITIVITY /////////////////////////////////////
    public int[] getAllDoubleFeedDectectionSensitivity() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int getCurrentDoubleFeedDectectionSensitivity() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultDoubleFeedDectectionSensitivity() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setDoubleFeedDectectionSensitivity(int bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY, bSet);
    }
    
    public boolean resetDoubleFeedDectectionSensitivity() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY);
    }
    
    public boolean isDoubleFeedDectectionSensitivitySupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY);
    }
    
    ////////////////////////// CAP_DUPLEX /////////////////////////////////////
    public int getDuplex() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEX, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentDuplex() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEX, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultDuplex() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEX, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean isDuplexSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEX);
    }
    
    
    ////////////////////////// CAP_DUPLEXENABLED /////////////////////////////////////
    public boolean isDuplexEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEXENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean setDuplexEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEXENABLED, bSet ? 1 : 0);
    }
    
    public boolean resetDuplexEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEXENABLED);
    }
    
    public boolean isDuplexEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPDUPLEXENABLED);
    }

    ////////////////////////// CAP_ENABLEDSUIONLY ///////////////////////////////////
    public boolean isEnableDSUIOnly() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPENABLEDSUIONLY, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean isEnableDSUIOnlySupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPENABLEDSUIONLY);
    }

    ////////////////////////// CAP_ENDORSER /////////////////////////////////////
    public int [] getAllEndorserStartNumbers() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPENDORSER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultEndorserStartNumber() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPENDORSER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentEndorserStartNumber() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPENDORSER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public boolean setEndorserStartNumber(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPENDORSER, value);
    }

    public boolean resetEndorserStartNumber() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPENDORSER);
    }

    public boolean isEndorserStartNumberSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPENDORSER);
    }
    
    ////////////////////////// CAP_EXTENDEDCAPS /////////////////////////////////////
    public int[] getAllExtendedCaps() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPEXTENDEDCAPS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int[] getCurrentExtendedCaps() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPEXTENDEDCAPS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultExtendedCaps() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPEXTENDEDCAPS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setExtendedCaps(int[] values) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPEXTENDEDCAPS, values);
    }

    public boolean resetExtendedCaps() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPEXTENDEDCAPS);
    }

    public boolean isExtendedCapsSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPEXTENDEDCAPS);
    }
    
    ////////////////////////// CAP_FEEDERALIGNMENT /////////////////////////////////////
    public int [] getAllFeederAlignment() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERALIGNMENT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int getCurrentFeederAlignment() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERALIGNMENT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultFeederAlignment() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERALIGNMENT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setFeederAlignment(int nSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERALIGNMENT, nSet);
    }

    public boolean resetFeederAlignment() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERALIGNMENT);
    }

    public boolean isFeederAlignmentSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERALIGNMENT);
    }
    
    ////////////////////////// CAP_FEEDERENABLED /////////////////////////////////////
    public boolean isFeederEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean setFeederEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERENABLED, bSet ? 1 : 0);
    }

    public boolean resetFeederEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERENABLED);
    }
    
    public boolean isFeederEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERENABLED);
    }
    
    ////////////////////////// CAP_FEEDERLOADED /////////////////////////////////////
    public boolean isFeederLoaded() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERLOADED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }
    
    public boolean isFeederLoadedSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERLOADED);
    }
    

    ////////////////////////// CAP_FEEDERORDER /////////////////////////////////////
    public int [] getAllFeederOrder() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERORDER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int getCurrentFeederOrder() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERORDER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultFeederOrder() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERORDER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setFeederOrder(int nSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERORDER, nSet);
    }

    public boolean isFeederOrderSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERORDER);
    }

    public boolean resetFeederOrder() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERORDER);
    }

    ////////////////////////// CAP_FEEDERPOCKET /////////////////////////////////////
    public int [] getAllFeederPocket() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPOCKET, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int [] getCurrentFeederPocket() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPOCKET, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int [] getDefaultFeederPocket() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPOCKET, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setFeederPocket(int nSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPOCKET, nSet);
    }

    public boolean isFeederPocketSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPOCKET);
    }
    
    public boolean resetFeederPocket() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPOCKET);
    }
    
    ////////////////////////// CAP_FEEDERPREP /////////////////////////////////////
    public boolean isFeederPrep() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPREP, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }
    
    public boolean setFeederPrep(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPREP, bSet?1:0);
    }

    public boolean isFeederPrepSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPREP);
    }
    
    public boolean resetFeederPrep() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDERPREP);
    }
    
    ////////////////////////// CAP_FEEDPAGE /////////////////////////////////////
    public boolean isFeedPage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDPAGE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean setFeedPage(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDPAGE, bSet ? 1 : 0);
    }

    public boolean resetFeedPage() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDPAGE);
    }

    public boolean isFeedPageSupported(boolean bSet) throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPFEEDPAGE);
    }
    
    ////////////////////////// CAP_INDICATORS /////////////////////////////////////
    public boolean isProgressIndicator() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORS, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean setProgressIndicator(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORS, bSet ? 1 : 0);
    }

    public boolean resetProgressIndicator() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORS);
    }

    public boolean isProgressIndicatorSupported(boolean bSet) throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORS);
    }

    ////////////////////////// CAP_INDICATORSMODE /////////////////////////////////////
    public int [] getAllIndicatorsMode() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORSMODE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }
    
    public int [] getCurrentIndicatorsMode() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORSMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int [] getDefaultIndicatorsMode() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORSMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setIndicatorsMode(int [] nSet) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORSMODE, nSet);
    }

    public boolean isIndicatorsModeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORSMODE);
    }

    public boolean resetIndicatorsMode() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPINDICATORSMODE);
    }
    
    ////////////////////////// CAP_JOBCONTROL /////////////////////////////////////
    public int[] getAllJobControl() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPJOBCONTROL, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultJobControl() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPJOBCONTROL, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentJobControl() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPJOBCONTROL, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setJobControl(int jc) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPJOBCONTROL, jc);
    }

    public boolean isJobControlSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPJOBCONTROL);
    }

    public boolean resetJobControl() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPJOBCONTROL);
    }
    
    ////////////////////////// CAP_LANGUAGE /////////////////////////////////////
    public int[] getAllLanguage() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPLANGUAGE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentLanguage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPLANGUAGE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultLanguage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPLANGUAGE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setLanguage(int nLanguage) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPLANGUAGE, nLanguage);
    }

    public boolean isLanguageSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPLANGUAGE);
    }

    public boolean resetLanguage() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPLANGUAGE);
    }
    
    ////////////////////////// CAP_MAXBATCHBUFFERS /////////////////////////////////////
    public int[] getAllMaxBatchBuffers() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPMAXBATCHBUFFERS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentMaxBatchBuffers() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPMAXBATCHBUFFERS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultMaxBatchBuffers() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPMAXBATCHBUFFERS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setMaxBatchBuffers(int nMax) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPMAXBATCHBUFFERS, nMax);
    }

    public boolean isMaxBtchBuffersSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPMAXBATCHBUFFERS);
    }

    public boolean resetMaxBatchBuffers() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPMAXBATCHBUFFERS);
    }

    ////////////////////////// CAP_MICRENABLED /////////////////////////////////////
    public boolean isMicrEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPMICRENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean setMicrEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPMICRENABLED, bSet?1:0);
    }

    public boolean isMicrEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPMICRENABLED);
    }

    public boolean resetMicrEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPMICRENABLED);
    }
    
    ////////////////////////// CAP_PAPERDETECTABLE /////////////////////////////////////
    public boolean isPaperDetectable() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERDETECTABLE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean isPaperDetectableSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERDETECTABLE);
    }

    ////////////////////////// CAP_PAPERHANDLING /////////////////////////////////////
    public int[] getAllPaperHandling() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERHANDLING, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentPaperHandling() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERHANDLING, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultPaperHandling() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERHANDLING, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPaperHandling(int [] nSet) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERHANDLING, nSet);
    }

    public boolean isPaperHandlingSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERHANDLING);
    }

    public boolean resetPaperHandling() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPAPERHANDLING);
    }

    ////////////////////////// CAP_POWERDOWNTIME /////////////////////////////////////
    public int getPowerDownTime() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERDOWNTIME, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPowerDownTime() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERDOWNTIME, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setPowerDownTime(int nPower) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERDOWNTIME, nPower);
    }
    
    ////////////////////////// CAP_POWERSAVETIME /////////////////////////////////////
    public int getPowerSaveTime() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSAVETIME, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPowerSaveTime() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSAVETIME, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPowerSaveTime() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSAVETIME, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public boolean setPowerSaveTime(int nPower) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSAVETIME, nPower);
    }

    public boolean resetPowerSaveTime() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSAVETIME);
    }
    
    ////////////////////////// CAP_POWERSUPPLY /////////////////////////////////////
    public int[] getAllPowerSupply () throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSUPPLY, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPowerSupply() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSUPPLY, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPowerSupply() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSUPPLY, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean isPowerSupplySupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPOWERSUPPLY);
    }
    
    ////////////////////////// CAP_PRINTER /////////////////////////////////////
    public int[] getAllPrinters() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPrinter() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPrinter() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinter(int printer) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTER, printer);
    }

    public boolean isPrinterSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTER);
    }

    public boolean resetPrinter() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTER);
    }

    ////////////////////////// CAP_PRINTERENABLED /////////////////////////////////////
    public boolean isPrinterEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean setPrinterEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERENABLED, bSet ? 1 : 0);
    }

    public boolean isPrinterEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERENABLED);
    }
    
    public boolean resetPrinterEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERENABLED);
    }
    
    ////////////////////////// CAP_PRINTERCHARROTATION /////////////////////////////////////
    public int[] getAllPrinterCharRotation() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERCHARROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPrinterCharRotation() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERCHARROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPrinterCharRotation() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERCHARROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterCharRotation(int [] nSet) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERCHARROTATION, nSet);
    }

    public boolean isPrinterCharRotationSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERCHARROTATION);
    }

    public boolean resetPrinterCharRotation() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERCHARROTATION);
    }

    ////////////////////////// CAP_PRINTERFONTSTYLE /////////////////////////////////////
    public int[] getAllPrinterFontStype() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERFONTSTYLE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentPrinterFontStyle() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERFONTSTYLE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultPrinterFontStyle() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERFONTSTYLE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterFontStyle(int [] nSet) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERFONTSTYLE, nSet);
    }

    public boolean isPrinterFontStyleSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERFONTSTYLE);
    }

    public boolean resetPrinterFontStyle() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERFONTSTYLE);
    }
    
    ////////////////////////// CAP_PRINTERINDEX /////////////////////////////////////
    public int[] getAllPrinterIndex() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEX, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPrinterIndex() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEX, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPrinterIndex() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEX, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterIndex(int nIndex) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEX, nIndex);
    }

    public boolean isPrinterIndexSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEX);
    }

    public boolean resetPrinterIndex() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEX);
    }

    ////////////////////////// CAP_PRINTERINDEXLEADCHAR /////////////////////////////////////
    public String getPrinterIndexLeadChar() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXLEADCHAR, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public String getCurrentPrinterIndexLeadChar() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXLEADCHAR, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultPrinterIndexLeadChar() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXLEADCHAR, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterIndexLeadChar(String bSet) throws DTwainRuntimeException {
        return stringCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXLEADCHAR, bSet);
    }

    public boolean isPrinterIndexLeadCharSupported() throws DTwainRuntimeException {
        return stringCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXLEADCHAR);
    }

    public boolean resetPrinterIndexLeadChar() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXLEADCHAR);
    }
    

    ////////////////////////// CAP_PRINTERINDEXMAXVALUE /////////////////////////////////////
    public int[] getAllPrinterIndexMaxValues() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXMAXVALUE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPrinterIndexMaxValue() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXMAXVALUE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPrinterIndexMaxValue() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXMAXVALUE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterIndexMaxValue(int nIndex) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXMAXVALUE, nIndex);
    }

    public boolean isPrinterIndexMaxValueSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXMAXVALUE);
    }

    public boolean resetPrinterIndexMaxValue() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXMAXVALUE);
    }

    ////////////////////////// CAP_PRINTERINDEXNUMDIGITS /////////////////////////////////////
    public int[] getAllPrinterIndexNumDigits() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPrinterIndexNumDigits() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPrinterIndexNumDigits() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterIndexNumDigits(int nIndex) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS, nIndex);
    }

    public boolean isPrinterIndexNumDigitsSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS);
    }

    public boolean resetPrinterIndexNumDigits() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS);
    }

    ////////////////////////// CAP_PRINTERINDEXSTEP /////////////////////////////////////
    public int[] getAllPrinterIndexStep() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXSTEP, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPrinterIndexStep() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXSTEP, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPrinterIndexStep() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXSTEP, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterIndexStep(int nIndex) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXSTEP, nIndex);
    }

    public boolean isPrinterIndexStepSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXSTEP);
    }

    public boolean resetPrinterIndexStep() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXSTEP);
    }

    ////////////////////////// CAP_PRINTERINDEXTRIGGER /////////////////////////////////////
    public int[] getAllPrinterIndexTriggers() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXTRIGGER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentPrinterIndexTriggers() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXTRIGGER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultPrinterIndexTriggers() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXTRIGGER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterIndexTriggers(int[] nSet) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXTRIGGER, nSet);
    }

    public boolean isPrinterIndexTriggersSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXTRIGGER);
    }

    public boolean resetPrinterIndexTriggers() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERINDEXTRIGGER);
    }
    
    ////////////////////////// CAP_PRINTERMODE /////////////////////////////////////
    public int[] getAllPrinterModes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERMODE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentPrinterMode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultPrinterMode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setPrinterMode(int mode) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERMODE, mode);
    }

    public boolean isPrinterModeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERMODE);
    }

    public boolean resetPrinterMode() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERMODE);
    }
    
    ////////////////////////// CAP_PRINTERSTRING /////////////////////////////////////
    public String getPrinterString() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRING, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public String getCurrentPrinterString() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRING, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultPrinterString() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRING, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterString(String bSet) throws DTwainRuntimeException {
        return stringCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRING, bSet);
    }

    public boolean isPrinterStringSupported() throws DTwainRuntimeException {
        return stringCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRING);
    }

    public boolean resetPrinterString() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRING);
    }

    ////////////////////////// CAP_PRINTERSTRINGPREVIEW /////////////////////////////////////
    public String[] getPrinterStringPreview() throws DTwainRuntimeException {
        return stringCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRINGPREVIEW, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public String[] getCurrentPrinterStringPreview() throws DTwainRuntimeException {
        return stringCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRINGPREVIEW, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String[] getDefaultPrinterStringPreview() throws DTwainRuntimeException {
        return stringCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRINGPREVIEW, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean isPrinterStringPreviewSupported() throws DTwainRuntimeException {
        return stringCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSTRINGPREVIEW);
    }

    ////////////////////////// CAP_PRINTERSUFFIX /////////////////////////////////////
    public String getPrinterSuffix() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSUFFIX, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public String getCurrentPrinterSuffix() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSUFFIX, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultPrinterSuffix() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSUFFIX, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean isPrinterSuffixSupported() throws DTwainRuntimeException {
        return stringCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSUFFIX);
    }

    public boolean setPrinterSuffix(String bSet) throws DTwainRuntimeException {
        return stringCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSUFFIX, bSet);
    }
    
    public boolean resetPrinterSuffix() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERSUFFIX);
    }
    
    ////////////////////////// CAP_PRINTERVERTICALOFFSET /////////////////////////////////////
    public double[] getAllPrinterVerticalOffsets() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERVERTICALOFFSET, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentPrinterVerticalOffset() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERVERTICALOFFSET, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultPrinterVerticalOffset() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERVERTICALOFFSET, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setPrinterVerticalOffset(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERVERTICALOFFSET, value);
    }
    
    public boolean isPrinterVerticalOffsetSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERVERTICALOFFSET);
    }

    public boolean resetPrinterVerticalOffset() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPPRINTERVERTICALOFFSET);
    }
    
    ////////////////////////// CAP_REACQUIREALLOWED /////////////////////////////////////
    public boolean isReAcquireAllowed() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPREACQUIREALLOWED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean isReAcquireAllowedSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPREACQUIREALLOWED);
    }
    
    ////////////////////////// CAP_REWINDPAGE /////////////////////////////////////
    public boolean getRewindPage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPREWINDPAGE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getDefaultRewindPage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPREWINDPAGE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean getCurrentRewindPage() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPREWINDPAGE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }

    public boolean setRewindPage(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPREWINDPAGE, bSet ? 1 : 0);
    }

    public boolean resetRewindPage() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPREWINDPAGE);
    }

    public boolean isRewindPageSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPREWINDPAGE);
    }

    ////////////////////////// CAP_SEGMENTED /////////////////////////////////////
    public int[] getAllSegmented() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPSEGMENTED, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentSegmented() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPSEGMENTED, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int getDefaultSegmented() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPSEGMENTED, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setSegmented(int nSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPSEGMENTED, nSet);
    }
    
    public boolean isSegmentedSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPSEGMENTED);
    }

    public boolean resetSegmented() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPSEGMENTED);
    }

    ////////////////////////// CAP_SERIALNUMBER /////////////////////////////////////
    public String getSerialNumber() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPSERIALNUMBER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public String getCurrentSerialNumber() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPSERIALNUMBER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultSerialNumber() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPSERIALNUMBER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean isSerialNumberSupported() throws DTwainRuntimeException {
        return stringCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPSERIALNUMBER);
    }

   
    ////////////////////////// CAP_SUPPORTEDCAPS /////////////////////////////////////
    // Implemted elsewhere
    
    ////////////////////////// CAP_SUPPORTEDCAPSSEGMENTUNIQUE /////////////////////////////////////
    public int[] getAllSupportedCapsSegmentUnique() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentSupportedCapsSegmentUnique() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int[] getDefaultSupportedCapsSegmentUnique() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean isSupportedCapsSegmentUniqueSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE);
    }

    ////////////////////////// CAP_SUPPORTEDDATS /////////////////////////////////////
    public int[] getAllSupportedDATS() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDDATS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentSupportedDATS() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDDATS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int[] getDefaultSupporteddATS() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDDATS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean isSupportedDATSSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPSUPPORTEDDATS);
    }
   
    ////////////////////////// CAP_TIMEBEFOREFIRSTCAPTURE /////////////////////////////////////
    public int[] getAllTimesBeforeFirstCapture() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentTimeBeforeFirstCapture() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultTimeBeforeFirstCapture() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setTimeBeforeFirstCapture(int nTime) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE, nTime);
    }

    public boolean resetTimeBeforeFirstCapture(int nTime) throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE);
    }
    
    ////////////////////////// CAP_TIMEBETWEENCAPTURES /////////////////////////////////////
    public int[] getTimesBetweenCaptures() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBETWEENCAPTURES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentTimeBetweenCaptures() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBETWEENCAPTURES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultTimeBetweenCaptures() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBETWEENCAPTURES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setTimeBetweenCaptures(int nTime) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBETWEENCAPTURES, nTime);
    }

    public boolean isTimeBetweenCapturesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEBETWEENCAPTURES);
    }

    ////////////////////////// CAP_TIMEDATE /////////////////////////////////////
    public String getTimeDate() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEDATE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public String getCurrentTimeDate() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEDATE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultTimeDate() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEDATE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean isTimeDateSupported() throws DTwainRuntimeException {
        return stringCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPTIMEDATE);
    }
    
    ////////////////////////// CAP_THUMBNAILSENABLED/////////////////////////////////////
    public boolean getThumbnailsEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTHUMBNAILSENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentThumbnailsEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTHUMBNAILSENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultThumbnailsEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTHUMBNAILSENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setThumbnailsEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPTHUMBNAILSENABLED, bSet ? 1 : 0);
    }

    public boolean resetThumbnailsEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_CAPTHUMBNAILSENABLED);
    }
    
    public boolean isThumbnailsEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPTHUMBNAILSENABLED);
    }
    
    ////////////////////////// CAP_UICONTROLLABLE /////////////////////////////////////
    public boolean isUIControllable() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPUICONTROLLABLE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean isUIControllableSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPUICONTROLLABLE);
    }
    
    ////////////////////////// CAP_XFERCOUNT /////////////////////////////////////
    public int[] getAllXferCounts() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_CAPXFERCOUNT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentXferCount() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPXFERCOUNT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultXferCount() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPXFERCOUNT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setXferCount(int nSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_CAPXFERCOUNT, nSet);
    }
    
    public boolean isXferCountSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_CAPXFERCOUNT);
    }
    
    ////////////////////////// ICAP_AUTOBRIGHT /////////////////////////////////////
    public boolean getAutoBright() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOBRIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentAutoBright() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOBRIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultAutoBright() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOBRIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setAutoBright(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOBRIGHT, bSet ? 1 : 0);
    }

    public boolean resetAutoBright() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOBRIGHT);
    }

    public boolean isAutoBrightSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOBRIGHT);
    }
    
    ////////////////////////// ICAP_AUTODISCARDBLANKPAGES /////////////////////////////////////
    public int[] getAllAutoDiscardBlankPages() throws DTwainRuntimeException {
        
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAutoDiscardBlankPages() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultAutoDiscardBlankPages() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setAutoDiscardBlankPages(int num) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES, num);
    }

    public boolean resetAutoDiscardBlankPages() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES);
    }

    public boolean isAutoDiscardBlankPagesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES);
    }

    ////////////////////////// ICAP_AUTOMATICBORDERDETECTION /////////////////////////////////////
    public boolean getAutomaticBorderDetection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentAutomaticBorderDetection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultAutomaticBorderDetection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setAutomaticBorderDetection(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION, bSet?1:0);
    }
    
    public boolean resetAutomaticBorderDetection() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION);
    }

    public boolean isAutomaticBorderDetectionSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION);
    }
    
    ////////////////////////// ICAP_AUTOMATICCOLORENABLED /////////////////////////////////////
    public boolean getAutomaticColorEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentAutomaticColorEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultAutomaticColorEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setAutomaticColorEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORENABLED, bSet?1:0);
    }
    
    public boolean resetAutomaticColorEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORENABLED);
    }

    public boolean isAutomaticColorEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORENABLED);
    }

    ////////////////////////// ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE  /////////////////////////////////////
    public int [] getAllAutomaticColorNonColorPixelType() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAutomaticColorNonColorPixelType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int getDefaultAutomaticColorNonColorPixelType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setAutomaticColorNonColorPixelType(int nValue) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE, nValue);
    }

    public boolean resetAutomaticColorNonColorPixelType() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE);
    }
    
    public boolean isAutomaticColorNonColorPixelTypeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE);
    }

    ////////////////////////// ICAP_AUTOMATICCROPUSESFRAME /////////////////////////////////////
    public boolean getAutomaticCropUsesFrame() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentAutomaticCropUsesFrame() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultAutomaticCropUsesFrame() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean isAutomaticCropUsesFrameSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME);
    }
    

    ////////////////////////// ICAP_AUTOMATICDESKEW /////////////////////////////////////
    public boolean getAutomaticDeskew() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICDESKEW, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentAutomaticDeskew() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICDESKEW, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultAutomaticDeskew() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICDESKEW, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setAutomaticDeskew(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICDESKEW, bSet ? 1 : 0);
    }

    public boolean resetAutomaticDeskew() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICDESKEW);
    }

    public boolean isAutomaticDeskewSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICDESKEW);
    }
    
    ////////////////////////// ICAP_AUTOMATICLENGTHDETECTION /////////////////////////////////////
    public boolean getAutomaticLengthDetection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentAutomaticLengthDetection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultAutomaticLengthDetection() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setAutomaticLengthDetection(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION, bSet ? 1 : 0);
    }

    public boolean resetAutomaticLengthDetection() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION);
    }

    public boolean isAutomaticLengthDetectionSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION);
    }
    
    
    ////////////////////////// ICAP_AUTOMATICROTATE /////////////////////////////////////
    public boolean getAutomaticRotate() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICROTATE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentAutomaticRotate() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICROTATE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultAutomaticRotate() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICROTATE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setAutomaticRotate(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICROTATE, bSet ? 1 : 0);
    }

    public boolean resetAutomaticRotate() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICROTATE);
    }

    public boolean isAutomaticRotateSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOMATICROTATE);
    }
    
    ////////////////////////// ICAP_AUTOSIZE /////////////////////////////////////
    public int [] getAllAutoSize() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOSIZE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentAutoSize() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOSIZE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int getDefaultAutoSize() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOSIZE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setAutoSize(int nValue) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOSIZE, nValue);
    }

    public boolean resetAutoSize() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOSIZE);
    }
    
    public boolean isAutoSizeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPAUTOSIZE);
    }

    ////////////////////////// ICAP_BARCODEDETECTIONENABLED /////////////////////////////////////
    public boolean getBarcodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEDETECTIONENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentBarcodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEDETECTIONENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultBarcodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEDETECTIONENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setBarcodeDetectionEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEDETECTIONENABLED, bSet ? 1 : 0);
    }

    public boolean resetBarcodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEDETECTIONENABLED);
    }

    public boolean isBarcodeDetectionEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEDETECTIONENABLED);
    }
    
    ////////////////////////// ICAP_BARCODEMAXRETRIES /////////////////////////////////////
    public int [] getAllBarcodeMaxRetries() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXRETRIES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBarcodeMaxRetries() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXRETRIES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int getDefaultBarcodeMaxRetries() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXRETRIES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setBarcodeMaxRetries(int nValue) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXRETRIES, nValue);
    }

    public boolean resetBarcodeMaxRetries() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXRETRIES);
    }
    
    public boolean isBarcodeMaxRetriesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXRETRIES);
    }

    
    ////////////////////////// ICAP_BARCODEMAXSEARCHPRIORITIES /////////////////////////////////////
    public int [] getAllBarcodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBarcodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int getDefaultBarcodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setBarcodeMaxSearchPriorities(int nValue) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES, nValue);
    }

    public boolean resetBarcodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES);
    }
    
    public boolean isBarcodeMaxSearchPrioritiesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES);
    }
    
    //////////////////////////ICAP_BARCODESEARCHMODE //////////////////////////////////////
    public int[] geAllBarcodeSearchModes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHMODE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBarcodeSearchMode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultBarcodeSearchMode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setBarcodeSearchMode(int num) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHMODE, num);
    }

    public boolean isBarcodeSearchModeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHMODE);
    }
    
    public boolean resetBarcodeSearchMode() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHMODE);
    }

    
    //////////////////////////ICAP_BARCODESEARCHPRIORITIES //////////////////////////////////////
    public int[] geAllBarcodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int [] getCurrentBarcodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int [] getDefaultBarcodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setBarcodeSearchPriorities(int [] num) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES, num);
    }

    public boolean isBarcodeSearchPrioritiesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES);
    }
    
    public boolean resetBarcodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES);
    }
    
    
    ////////////////////////// ICAP_BITDEPTH //////////////////////////////////////
    public int[] getAllBitDepths() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTH, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBitDepth() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTH, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultBitDepth() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTH, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setBitDepth(int num) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTH, num);
    }

    public boolean resetBitDepth(int num) throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTH);
    }

    public boolean isBitDepthSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTH);
    }
    
    ////////////////////////// ICAP_BITDEPTHREDUCTION //////////////////////////////////////
    public int[] getAllBithDepthReductions() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTHREDUCTION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBitDepthReduction() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTHREDUCTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultBitDepthReduction() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTHREDUCTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setBitDepthReduction(int num) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTHREDUCTION, num);
    }

    public boolean resetBitDepthReduction() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTHREDUCTION);
    }

    public boolean isBitDepthReductionSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITDEPTHREDUCTION);
    }
    
    ////////////////////////// ICAP_BITORDER //////////////////////////////////////
    public int[] getAllBitOrders() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBitOrder() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultBitOrder() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setBitOrder(int num) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDER, num);
    }

    public boolean resetBitOrder() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDER);
    }

    public boolean isBitOrderSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDER);
    }
    
    //////////////////////////ICAP_BITORDERCODES //////////////////////////////////////
    public int[] getAllBitOrderCodes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDERCODES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentBitOrderCode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDERCODES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultBitOrderCode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDERCODES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setBitOrderCode(int num) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDERCODES, num);
    }

    public boolean resetBitOrderCode() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDERCODES);
    }

    public boolean isBitOrderCodesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBITORDERCODES);
    }

    //////////////////////////ICAP_BRIGHTNESS //////////////////////////////////////
    public double[] getAllBrightness() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBRIGHTNESS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentBrightness() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBRIGHTNESS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultBrightness() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBRIGHTNESS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setBrightness(double num) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBRIGHTNESS, num);
    }

    public boolean resetBrightness() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBRIGHTNESS);
    }

    public boolean isBrightnessSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPBRIGHTNESS);
    }
    
    //////////////////////////ICAP_CCITTKFACTOR //////////////////////////////////////
    public int getCCITTKFactor() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCCITTKFACTOR, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultCCITTKFactor() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCCITTKFACTOR, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentCCITTKFactor() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCCITTKFACTOR, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setCCITTKFactor(int factor) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCCITTKFACTOR, factor);
    }

    public boolean resetCCITTKFactor() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCCITTKFACTOR);
    }
    
    public boolean isCCITTKFactorSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCCITTKFACTOR);
    }

    
    //////////////////////////ICAP_COLORMANAGEMENTENABLED //////////////////////////////////////
    public boolean getColorManagementEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getDefaultColorManagementEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }

    public boolean getCurrentColorManagementEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }

    public boolean setColorManagementEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED, bSet?1:0);
    }

    public boolean resetColorManagementEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED);
    }
    
    public boolean isColorManagementEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED);
    }
    
    //////////////////////////ICAP_COMPRESSION //////////////////////////////////////
    public int[] getAllCompressions() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOMPRESSION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultCompression() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOMPRESSION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentCompression() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOMPRESSION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setCompression(int compression) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOMPRESSION, compression);
    }

    public boolean resetCompression() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOMPRESSION);
    }

    public boolean isCompressionSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCOMPRESSION);
    }

    //////////////////////////ICAP_CONTRAST //////////////////////////////////////
    public double[] getAllContrasts() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCONTRAST, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultContrast() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCONTRAST, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentContrast() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCONTRAST, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setContrast(double contrast) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCONTRAST, contrast);
    }

    public boolean resetContrast() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCONTRAST);
    }

    public boolean isContrastSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCONTRAST);
    }
    
    //////////////////////////ICAP_CUSTHALFTONE //////////////////////////////////////
    public int[] getAllCustomHalfTones() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCUSTHALFTONE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentCustomHalfTones() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCUSTHALFTONE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public int[] getDefaultCustomHalfTones() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCUSTHALFTONE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setCustomHalfTones(int[] values) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCUSTHALFTONE, values);
    }

    public boolean resetCustomHalfTones() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCUSTHALFTONE);
    }
    
    public boolean isCustomHalfToneSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPCUSTHALFTONE);
    }
    
    //////////////////////////ICAP_EXPOSURETIME //////////////////////////////////////
    public double[] getAllExposureTimes() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXPOSURETIME, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentExposureTime() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXPOSURETIME, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultExposureTime() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXPOSURETIME, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setExposureTime(double num) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXPOSURETIME, num);
    }

    public boolean resetExposureTime() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXPOSURETIME);
    }

    public boolean isExposureTimeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXPOSURETIME);
    }
    
    //////////////////////////ICAP_EXTIMAGEINFO /////////////////////////////////////
    public boolean getExtendedImageInfo() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXTIMAGEINFO, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1 ;
    }

    public boolean getCurrentExtendedImageInfo() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXTIMAGEINFO, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1 ;
    }
    
    public boolean getDefaultExtendedImageInfo() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXTIMAGEINFO, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1 ;
    }
    
    public boolean setExtendedImageInfo(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXTIMAGEINFO, bSet ? 1 : 0);
    }

    public boolean resetExtendedImageInfo() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXTIMAGEINFO);
    }

    public boolean isExtendedImageInfoSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPEXTIMAGEINFO);
    }

    
    //////////////////////////ICAP_FEEDERTYPE //////////////////////////////////////
    public int[] getAllFeederTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFEEDERTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentFeederType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFEEDERTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultFeederType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFEEDERTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setFeederType(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFEEDERTYPE, value);
    }

    public boolean resetFeederType() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFEEDERTYPE);
    }
    
    public boolean isFeederTypeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFEEDERTYPE);
    }
    
    //////////////////////////ICAP_FILMTYPE //////////////////////////////////////
    public int[] getAllFilmTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILMTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentFilmType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILMTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultFilmType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILMTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setFilmType(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILMTYPE, value);
    }

    public boolean resetFilmType() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILMTYPE);
    }
    
    public boolean isFilmTypeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILMTYPE);
    }
    
    
    //////////////////////////ICAP_FILTER //////////////////////////////////////
    public int[] getAllFilters() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getDefaultFilters() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int[] getCurrentFilters() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setFilters(int[] filters) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILTER, filters);
    }

    public boolean resetFilters() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILTER);
    }

    public boolean isFilterSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFILTER);
    }
    
    //////////////////////////ICAP_FLASHUSED2 //////////////////////////////////////
    public int[] getFlashUsed2() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLASHUSED2, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getCurrentFlashUsed2() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLASHUSED2, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int getDefaultFlashUsed2() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLASHUSED2, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean setFlashUsed2(int flash) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLASHUSED2, flash);
    }

    public boolean resetFlashUsed2() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLASHUSED2);
    }

    public boolean isFlashUsed2Supported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLASHUSED2);
    }

    //////////////////////////ICAP_FLIPROTATION //////////////////////////////////////
    public int[] getAllFlipRotations() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLIPROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultFlipRotation() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLIPROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentFlippRotation() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLIPROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setFlipRotation(int flip) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLIPROTATION, flip);
    }

    public boolean resetFlipRotation() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLIPROTATION);
    }

    public boolean isFlipRotationSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFLIPROTATION);
    }

    //////////////////////////ICAP_FRAMES //////////////////////////////////////
    public DTwainFrame[] getAllFrames() throws DTwainRuntimeException {
        return frameCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFRAMES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public DTwainFrame getCurrentFrame() throws DTwainRuntimeException {
        return frameCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFRAMES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public DTwainFrame getDefaultFrame() throws DTwainRuntimeException {
        return frameCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFRAMES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setFrame(DTwainFrame frame) throws DTwainRuntimeException {
        return frameCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFRAMES, frame);
    }

    public boolean resetFrame() throws DTwainRuntimeException {
        return frameCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFRAMES);
    }

    public boolean isFramesSupported() throws DTwainRuntimeException {
        return frameCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPFRAMES);
    }

    
    //////////////////////////ICAP_GAMMA //////////////////////////////////////
    public double[] getAllGammas() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPGAMMA, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentGamma() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPGAMMA, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultGamma() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPGAMMA, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setGamma(double num) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPGAMMA, num);
    }

    public boolean resetGamma() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPGAMMA);
    }

    public boolean isGammaSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPGAMMA);
    }
    
    //////////////////////////ICAP_HALFTONES //////////////////////////////////////
    public String[] getAllHalftones() throws DTwainRuntimeException {
        return stringCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public String getCurrentHalftone() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public String getDefaultHalftone() throws DTwainRuntimeException {
        return stringCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setHalftones(String num) throws DTwainRuntimeException {
        return stringCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES, num);
    }

    public boolean resetHalftones() throws DTwainRuntimeException {
        return stringCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES);
    }

    public boolean isHalftonesSupported() throws DTwainRuntimeException {
        return stringCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES);
    }
    
    //////////////////////////ICAP_HIGHLIGHT //////////////////////////////////////
    public double[] getAllHighlights() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHIGHLIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentHighlight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultHighlight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public boolean setHighlight(double num) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES, num);
    }

    public boolean resetHighlight() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES);
    }

    public boolean isHighlightSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPHALFTONES);
    }

    
    //////////////////////////ICAP_ICCPROFILE //////////////////////////////////////
    public int[] getAllICCProfiles() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPICCPROFILE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultICCProfile() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPICCPROFILE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentICCProfile() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPICCPROFILE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setICCProfile(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPICCPROFILE, value);
    }

    public boolean resetICCProfile() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPICCPROFILE);
    }
    
    public boolean isICCProfileSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPICCPROFILE);
    }
    
    //////////////////////////ICAP_IMAGEDATASET //////////////////////////////////////
    public int[] getAllImageDataSets() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEDATASET, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getDefaultImageDataSets() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEDATASET, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int[] getCurrentImageDataSets() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEDATASET, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setImageDataSets(int[] values) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEDATASET, values);
    }

    public boolean resetImageDataSets() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEDATASET);
    }
    
    public boolean isImageDataSetSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEDATASET);
    }

    //////////////////////////ICAP_IMAGEFILEFORMAT //////////////////////////////////////
    public int[] getAllImageFileFormats() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultImageFileFormat() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentImageFileFormat() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setImageFileFormat(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT, value);
    }

    public boolean resetImageFileFormat() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT);
    }
    
    public boolean isImageFileFormatSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT);
    }
    
    //////////////////////////ICAP_IMAGEFILTER //////////////////////////////////////
    public int[] getImageFilters() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultImageFilter() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentImageFilter() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setImageFilter(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT, value);
    }

    public boolean resetImageFilter() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT);
    }

    public boolean isImageFilterSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEFILEFORMAT);
    }

    
    //////////////////////////ICAP_IMAGEMERGE //////////////////////////////////////
    public int[] getAllImageMerges() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultImageMerge() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentImageMerge() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setImageMerge(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGE, value);
    }

    public boolean resetImageMerge() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGE);
    }
    
    public boolean isImageMergeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGE);
    }
    
    //////////////////////////ICAP_IMAGEMERGEHEIGHTTHRESHOLD //////////////////////////////////////
    public double[] getAllImageMergeHeightThresholds() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultImageMergeHeightThreshold() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentImageMergeHeightThreshold() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setImageMergeHeightThreshold(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD, value);
    }

    public boolean resetImageMergeHeightThreshold() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD);
    }
    
    public boolean isImageMergeHeightThresholdSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD);
    }
    

    //////////////////////////ICAP_JPEGPIXELTYPE //////////////////////////////////////
    public int[] getAllJPEGPixelTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultJPEGPixelType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentJPEGPixelType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setJPEGPixelType(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGPIXELTYPE, value);
    }

    public boolean resetJPEGPixelType() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGPIXELTYPE);
    }
    
    public boolean isJPEGPixelTypeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGPIXELTYPE);
    }
    
    
//////////////////////////ICAP_JPEGQUALITY //////////////////////////////////////
    public int[] getAllJPEGQualities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGQUALITY, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultJPEGQuality() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGQUALITY, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentJPEGQuality() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGQUALITY, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setJPEGQuality(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGQUALITY, value);
    }

    public boolean resetJPEGQuality() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGQUALITY);
    }

    public boolean isJPEGQualitySupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGQUALITY);
    }
    
//////////////////////////ICAP_JPEGSUBSAMPLING //////////////////////////////////////
    public int[] getAllJPEGSubSampling() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGSUBSAMPLING, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultJPEGSubSampling() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGSUBSAMPLING, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentJPEGSubSampling() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGSUBSAMPLING, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setJPEGSubSampling(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGSUBSAMPLING, value);
    }

    public boolean resetJPEGSubSampling() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGSUBSAMPLING);
    }

    public boolean isSubSamplingSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPJPEGSUBSAMPLING);
    }
    
    //////////////////////////ICAP_LAMPSTATE /////////////////////////////////////
    public boolean getLampState() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLAMPSTATE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentLampState() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLAMPSTATE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean getDefaultLampState() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLAMPSTATE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setLampState(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLAMPSTATE, bSet?1:0);
    }

    public boolean resetLampState() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLAMPSTATE);
    }

    public boolean isLampStateSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLAMPSTATE);
    }

    //////////////////////////ICAP_LIGHTPATH //////////////////////////////////////
    public int[] getAllLightPaths() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTPATH, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultLightPath() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTPATH, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentLightPath() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTPATH, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setLightPath(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTPATH, value);
    }

    public boolean resetLightPath() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTPATH);
    }

    public boolean isLightPathSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTPATH);
    }
    
    //////////////////////////ICAP_LIGHTSOURCE //////////////////////////////////////
    public int[] getAllLightSources() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTSOURCE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultLightSource() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTSOURCE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentLightSource() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTSOURCE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setLightSource(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTSOURCE, value);
    }

    public boolean resetLightSource() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTSOURCE);
    }

    public boolean isLightSourceSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPLIGHTSOURCE);
    }

    /////////////////////////ICAP_MAXFRAMES //////////////////////////////////////
    public int[] geAlltMaxFrames() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMAXFRAMES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultMaxFrames() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMAXFRAMES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentMaxFrames() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMAXFRAMES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setMaxFrames(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMAXFRAMES, value);
    }

    public boolean resetMaxFrames() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMAXFRAMES);
    }

    public boolean isMaxFramesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMAXFRAMES);
    }

    /////////////////////////ICAP_MINIMUMHEIGHT //////////////////////////////////////
    public double getMinimumHeight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMHEIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultMinimumHeight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMHEIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentMinimumHeight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMHEIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean isMinimumHeightSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMHEIGHT);
    }


    /////////////////////////ICAP_MINIMUMWIDTH //////////////////////////////////////
    public double getMinimumWidth() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMWIDTH, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultMinimumWidth() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMWIDTH, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentMinimumWidth() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMWIDTH, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean isMinimumWidthSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMINIMUMWIDTH);
    }


    //////////////////////////ICAP_MIRROR //////////////////////////////////////
    public int[] getAllMirrors() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMIRROR, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultMirror() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMIRROR, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentMirror() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMIRROR, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setMirror(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMIRROR, value);
    }

    public boolean resetMirror() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMIRROR);
    }

    public boolean isMirrorSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPMIRROR);
    }

    
    //////////////////////////ICAP_NOISEFILTER //////////////////////////////////////
    public int[] getAllNoiseFilters() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPNOISEFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultNoiseFilter() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPNOISEFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentNoiseFilter() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPNOISEFILTER, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setNoiseFilter(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPNOISEFILTER, value);
    }

    public boolean resetNoiseFilter() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPNOISEFILTER);
    }

    public boolean isNoiseFilterSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPNOISEFILTER);
    }

    //////////////////////////ICAP_ORIENTATION //////////////////////////////////////
    public int[] getAllOrientations() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPORIENTATION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultOrientation() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPORIENTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentOrientation() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPORIENTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setOrientation(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPORIENTATION, value);
    }

    public boolean resetOrientation() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPORIENTATION);
    }

    public boolean isOrientationSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPORIENTATION);
    }

    //////////////////////////ICAP_OVERSCAN //////////////////////////////////////
    public int[] getAllOverscans() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPOVERSCAN, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultOverscan() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPOVERSCAN, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentOverscan() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPOVERSCAN, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setOverscan(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPOVERSCAN, value);
    }

    public boolean resetOverscan(int value) throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPOVERSCAN);
    }

    public boolean isOverscanSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPOVERSCAN);
    }

    //////////////////////////ICAP_PATCHCODEDETECTIONENABLED /////////////////////////////////////
    public boolean getPatchCodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getCurrentPatchCodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }

    public boolean getDefaultPatchCodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }
    
    public boolean setPatchCodeDetectionEnabled(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED, bSet ? 1 : 0);
    }

    public boolean resetPatchCodeDetectionEnabled() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED);
    }
    
    public boolean isPatchCodeDetectionEnabledSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED);
    }
    
    ////////////////////////// ICAP_PATCHCODEMAXRETRIES //////////////////////////////////////
    public int[] getAllPatchCodeMaxRetries() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXRETRIES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPatchCodeMaxRetries() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXRETRIES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPatchCodeMaxRetries() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXRETRIES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPatchCodeMaxRetries(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXRETRIES, value);
    }

    public boolean resetPatchCodeMaxRetries(int value) throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXRETRIES);
    }

    public boolean isPatchCodeMaxRetriesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXRETRIES);
    }

    

    //////////////////////////ICAP_PATCHCODEMAXSEARCHPRIORITIES //////////////////////////////////////
    public int[] getAllPatchCodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPatchCodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPatchCodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPatchCodeMaxSearchPriorities(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES, value);
    }

    public boolean resetPatchCodeMaxSearchPriorities() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES);
    }

    public boolean isPatchCodeMaxSearchPrioritiesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES);
    }
    
    //////////////////////////ICAP_PATCHCODEMAXSEARCHMODE //////////////////////////////////////
    public int[] getAllPatchCodeSearchModes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHMODE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPatchCodeSearchMode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPatchCodeSearchMode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHMODE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPatchCodeSearchMode(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHMODE, value);
    }

    public boolean resetPatchCodeSearchMode() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHMODE);
    }

    public boolean isPatchCodeSearchModeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHMODE);
    }

    
    //////////////////////////ICAP_PATCHCODESEARCHPRIORITIES //////////////////////////////////////
    public int[] getAllPatchCodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getDefaultPatchCodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int[] getCurrentPatchCodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPatchCodeSearchPriorities(int[] values) throws DTwainRuntimeException {
        return intCaps.setMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES, values);
    }

    public boolean resetPatchCodeSearchPriorities() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES);
    }

    public boolean isPatchCodeSearchPrioritiesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES);
    }

    //////////////////////////ICAP_PATCHCODETIMEOUT //////////////////////////////////////
    public int[] getPatchCodeTimeOutValues() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODETIMEOUT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPatchCodeTimeOutValue() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODETIMEOUT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPatchCodeTimeOutValue() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODETIMEOUT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPatchCodeTimeOutValue(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODETIMEOUT, value);
    }

    public boolean resetPatchCodeTimeOutValue() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODETIMEOUT);
    }

    public boolean isPatchCodeTimeOutValueSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPATCHCODETIMEOUT);
    }

    
    //////////////////////////ICAP_PHYSICALHEIGHT //////////////////////////////////////
    public double getPhysicalHeight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALHEIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentPhysicalHeight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALHEIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultPhysicalHeight() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALHEIGHT, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean isPhysicalHeightSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALHEIGHT);
    }

    //////////////////////////ICAP_PHYSICALWIDTH //////////////////////////////////////
    public double getPhysicalWidth() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALWIDTH, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getCurrentPhysicalWidth() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALWIDTH, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public double getDefaultPhysicalWidth() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALWIDTH, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean isPhysicalWidthSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPHYSICALWIDTH);
    }
    
    
    //////////////////////////ICAP_PIXELFLAVOR //////////////////////////////////////
    public int[] getAllPixelFlavors() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVOR, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPixelFlavor() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVOR, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPixelFlavor() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVOR, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPixelFlavor(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVOR, value);
    }

    public boolean resetPixelFlavor() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVOR);
    }
    
    public boolean isPixelFlavorSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVOR);
    }
    
    //////////////////////////ICAP_PIXELFLAVORCODES //////////////////////////////////////
    public int[] getAllPixelFlavorCodes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVORCODES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPixelFlavorCode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVORCODES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPixelFlavorCode() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVORCODES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPixelFlavorCode(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVORCODES, value);
    }

    public boolean resetPixelFlavorCode() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVORCODES);
    }

    public boolean isPixelFlavorCodesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELFLAVORCODES);
    }

    //////////////////////////ICAP_PIXELTYPE //////////////////////////////////////
    public int[] getAllPixelTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPixelType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPixelType() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELTYPE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPixelType(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELTYPE, value);
    }

    public boolean resetPixelType() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELTYPE);
    }

    public boolean isPixelTypeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPIXELTYPE);
    }
    
    //////////////////////////ICAP_PLANARCHUNKY //////////////////////////////////////
    public int[] getAllPlanarChunky() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPLANARCHUNKY, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultPlanarChunky() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPLANARCHUNKY, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentPlanarChunky() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPLANARCHUNKY, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setPlanarChunky(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPLANARCHUNKY, value);
    }

    public boolean resetPlanarChunky() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPLANARCHUNKY);
    }

    public boolean isPlanarChunkySupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPPLANARCHUNKY);
    }

    //////////////////////////ICAP_ROTATION //////////////////////////////////////
    public double[] getAllRotationValues() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultRotationValue() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentRotationValue() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPROTATION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setRotationValue(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPROTATION, DTwainJavaAPIConstants.DTWAIN_CAPSET);
    }

    public boolean resetRotationValue() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPROTATION);
    }
    
    public boolean isRotationValueSupported(double value) throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPROTATION);
    }
    
    //////////////////////////ICAP_SHADOW //////////////////////////////////////
    public double[] getAllShadows() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSHADOW, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultShadow() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSHADOW, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentShadow() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSHADOW, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setShadow(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSHADOW, value);
    }

    public boolean resetShadow() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSHADOW);
    }
    
    public boolean isShadowSupported(double value) throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSHADOW);
    }
    
    //////////////////////////ICAP_SUPPORTEDBARCODETYPES //////////////////////////////////////
    public int[] getAllSupportedBarCodeTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentSupportedBarCodeTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultSupportedBarCodeTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean isSupportedBarCodeTypesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES);
    }

    //////////////////////////ICAP_SUPPORTEDEXTIMAGEINFO //////////////////////////////////////
    public int[] getAllSupportedExtImageInfo() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentSupportedExtImageInfo() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultSupportedExtImageInfo() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean isSupportedExtImageInfoSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO);
    }

    //////////////////////////ICAP_SUPPORTEDPATCHCODETYPES //////////////////////////////////////
    public int[] getAllSupportedPatchCodeTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int[] getCurrentSupportedPatchCodeTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public int[] getDefaultSupportedPatchCodeTypes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public boolean isSupportedPatchCodeTypesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES);
    }

    //////////////////////////ICAP_SUPPORTEDSIZES //////////////////////////////////////
    public int[] getAllSupportedSizes() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDSIZES, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultSupportedSize() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDSIZES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentSupportedSize() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDSIZES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setSupportedSize(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDSIZES, value);
    }

    public boolean resetSupportedSize() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDSIZES);
    }

    public boolean isSupportedSizesSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPSUPPORTEDSIZES);
    }

    //////////////////////////ICAP_THRESHOLD //////////////////////////////////////
    public double[] getAllThresholds() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTHRESHOLD, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultThreshold() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTHRESHOLD, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentThreshold() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTHRESHOLD, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setThreshold(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTHRESHOLD, value);
    }

    public boolean resetThreshold() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTHRESHOLD);
    }
    
    public boolean isThresholdSupported(double value) throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTHRESHOLD);
    }
    
    /////////////////////////// ICAP_TILES /////////////////////////////////
    public boolean getTile() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTILES, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getDefaultTile() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTILES, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }

    public boolean getCurrentTile() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTILES, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }
    
    public boolean setTile(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTILES, bSet ? 1 : 0);
    }

    public boolean resetTile() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTILES);
    }

    public boolean isTileSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTILES);
    }
    
    //////////////////////////ICAP_TIMEFILL //////////////////////////////////////
    public int[] getAllTimeFill() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTIMEFILL, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultTimeFill() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTIMEFILL, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentTimeFill() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTIMEFILL, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setTimeFill(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTIMEFILL, value);
    }

    public boolean resetTimeFill() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTIMEFILL);
    }

    public boolean isTimeFillSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPTIMEFILL);
    }

    /////////////////////////// ICAP_UNDEFINEDIMAGESIZE /////////////////////////////////
    public boolean getUndefinedImageSize() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE, DTwainJavaAPIConstants.DTWAIN_CAPGET) == 1;
    }

    public boolean getDefaultUndefinedImageSize() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT) == 1;
    }

    public boolean getCurrentUndefinedImageSize() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT) == 1;
    }

    public boolean setUndefinedImageSize(boolean bSet) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE, bSet ? 1 : 0);
    }

    public boolean resetUndefinedImageSize() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE);
    }

    public boolean isUndefinedImageSizeSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE);
    }
    
    //////////////////////////ICAP_UNITS //////////////////////////////////////
    public int[] getAllUnits() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNITS, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultUnits() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNITS, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentUnits() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNITS, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setUnits(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNITS, value);
    }

    public boolean resetUnits() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNITS);
    }
    
    public boolean isUnitsSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPUNITS);
    }
    //////////////////////////ICAP_XFERMECH //////////////////////////////////////
    public int[] getAllXferMechs() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXFERMECH, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultXferMech() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXFERMECH, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentXferMech() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXFERMECH, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setXferMech(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXFERMECH, value);
    }

    public boolean resetXferMech() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXFERMECH);
    }

    public boolean isXferMechSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXFERMECH);
    }

    //////////////////////////ICAP_XNATIVERESOLUTION //////////////////////////////////////
    public double getXNativeResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXNATIVERESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultXNativeResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXNATIVERESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public double getCurrentXNativeResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXNATIVERESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public boolean isXNativeResolutionSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXNATIVERESOLUTION);
    }

    //////////////////////////ICAP_XRESOLUTION //////////////////////////////////////
    public double[] getAllXResolutions() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXRESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultXResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXRESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentXResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXRESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setXResolution(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXRESOLUTION, value);
    }

    public boolean resetXResolution() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXRESOLUTION);
    }
    
    public boolean isXResolutionSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXRESOLUTION);
    }

    
    //////////////////////////ICAP_XSCALING //////////////////////////////////////
    public double[] getAllXScalings() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXSCALING, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultXScaling() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXSCALING, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentXScaling() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXSCALING, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setXScaling(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXSCALING, value);
    }

    public boolean resetXScaling() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXSCALING);
    }
    
    public boolean isXScalingSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXSCALING);
    }
    

    
    //////////////////////////ICAP_YNATIVERESOLUTION //////////////////////////////////////
    public double getYNativeResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYNATIVERESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultYNativeResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYNATIVERESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }
    
    public double getCurrentYNativeResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYNATIVERESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }
    
    public boolean isYNativeResolutionSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYNATIVERESOLUTION);
    }

    //////////////////////////ICAP_YRESOLUTION //////////////////////////////////////
    public double[] getAllYResolutions() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYRESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultYResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYRESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentYResolution() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYRESOLUTION, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setYResolution(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYRESOLUTION, value);
    }

    public boolean resetYResolution() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYRESOLUTION);
    }
    
    public boolean isYResolutionSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYRESOLUTION);
    }

    
    //////////////////////////ICAP_YSCALING //////////////////////////////////////
    public double[] getAllYScalings() throws DTwainRuntimeException {
        return doubleCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYSCALING, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public double getDefaultYScaling() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYSCALING, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public double getCurrentYScaling() throws DTwainRuntimeException {
        return doubleCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYSCALING, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setYScaling(double value) throws DTwainRuntimeException {
        return doubleCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYSCALING, value);
    }

    public boolean resetYScaling() throws DTwainRuntimeException {
        return doubleCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYSCALING);
    }
    
    public boolean isYScalingSupported() throws DTwainRuntimeException {
        return doubleCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPYSCALING);
    }
    
    
    //////////////////////////ICAP_ZOOMFACTOR//////////////////////////////////////
    public int[] getAllZoomFactors() throws DTwainRuntimeException {
        return intCaps.getMultipleCapValues(DTwainJavaAPIConstants.DTWAIN_CV_ICAPZOOMFACTOR, DTwainJavaAPIConstants.DTWAIN_CAPGET);
    }

    public int getDefaultZoomFactor() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPZOOMFACTOR, DTwainJavaAPIConstants.DTWAIN_CAPGETDEFAULT);
    }

    public int getCurrentZoomFactor() throws DTwainRuntimeException {
        return intCaps.getSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPZOOMFACTOR, DTwainJavaAPIConstants.DTWAIN_CAPGETCURRENT);
    }

    public boolean setZoomFactor(int value) throws DTwainRuntimeException {
        return intCaps.setSingleCapValue(DTwainJavaAPIConstants.DTWAIN_CV_ICAPZOOMFACTOR, value);
    }

    public boolean resetZoomFactor() throws DTwainRuntimeException {
        return intCaps.resetCap(DTwainJavaAPIConstants.DTWAIN_CV_ICAPZOOMFACTOR);
    }

    public boolean isZoomFactorSupported() throws DTwainRuntimeException {
        return intCaps.isSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPZOOMFACTOR);
    }
}
