package com.dynarithmic.twain;

import java.util.TreeMap;

public class DTwainJavaAPI 
{
    static private class DTwainModuleNames
    {
        public String s_DTwainDLLName;
        public String s_DTwainJNIName;
        public DTwainModuleNames(String n1, String n2) 
              { s_DTwainDLLName = n1; s_DTwainJNIName = n2; }
    }

    private long  m_LibraryHandle;
    private int m_DLLName = 0;
    private static TreeMap<Integer, DTwainModuleNames> s_DLLMap;
    public static int JNIDLLName_32 = 0;
    public static int JNIDLLName_32U  = 1;
    public static int JNIDLLName_64 = 2;
    public static int JNIDLLName_64U  = 3;

    /**
     * Instantiates a DTwainJavaAPI interface that allows communication to the 
     * JNI layer.  This function must be called before calling any other 
     * DTwain API Java function.
     * <p>
     * The underlying JNI layer that will be used will be based on a 32-bit, Unicode-based
     * system.  If 64-bit Unicode is desired, see DTwainJavaAPI(int) 
     */    
    public DTwainJavaAPI()
    { 
        m_LibraryHandle = 0;	
        m_DLLName = JNIDLLName_32U;
    }

    /**
     * Instantiates a DTwainJavaAPI interface that allows communication to the 
     * JNI layer.  This function must be called before calling any other 
     * DTwain API Java function.
     * <p>
     * The underlying JNI layer that will be used will be based on a 32-bit, Unicode-based
     * system.  If 64-bit Unicode is desired, see DTwainJavaAPI(int) 
     *
     * @param  dllname  Integer denoting the DLL to use.  Valid integers:
     * 
     * JNIDLLName_32
     * JNIDLLName_32U
     * JNIDLLName_64
     * JNIDLLName_64U
     */    
    public DTwainJavaAPI(int dllName) throws Exception
    {
        m_LibraryHandle = 0;
        try {
            s_DLLMap.get(dllName);
            m_DLLName = dllName;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public boolean DTWAIN_JavaSysInitialize() throws Exception
    {
        if ( m_LibraryHandle != 0 )
                return true;
        try 
        {
        	System.out.println(s_DLLMap.get(m_DLLName).s_DTwainJNIName);
            System.loadLibrary(s_DLLMap.get(m_DLLName).s_DTwainJNIName);
            DTWAIN_LoadLibrary(s_DLLMap.get(m_DLLName).s_DTwainDLLName);
        }
        catch(Exception e)
        {
            throw new Exception(e);
        }
        try  {
                m_LibraryHandle = DTWAIN_SysInitialize();
        } catch (DTwainJavaAPIException e)	{
                e.printStackTrace();	
        }	
        if ( m_LibraryHandle != 0 )
                return true;
        return false;
    }

    public boolean DTWAIN_JavaSysDestroy() throws Exception
    {	
            if ( m_LibraryHandle != 0 )
            try	
            {
                    int val =  DTWAIN_SysDestroy();
                    if ( val == 1 )	
                    {
                        DTWAIN_FreeLibrary();
                        m_LibraryHandle = 0;
                        return true;
                    }
            } 
            catch (Exception e) 
            {
                throw e;
            }	
            return false;
    }

    public boolean startTwainThread() 
    {
            if ( m_LibraryHandle == 0 )
                    return false;
            try {
                    return DTWAIN_StartThread(m_LibraryHandle);
            } catch (DTwainJavaAPIException e) {
                    e.printStackTrace();
            }
            return false;
    }

    public boolean endTwainThread()
    {
            if ( m_LibraryHandle == 0 )
                    return false;
            try {
                    return DTWAIN_EndThread(m_LibraryHandle);
            } catch (DTwainJavaAPIException e) {
                    e.printStackTrace();
            }
            return false;
    }

    public long getLibraryHandle() { return m_LibraryHandle; }
	
    // dynamically load/unload DTWAIN DLL
    public native int DTWAIN_LoadLibrary(String s) throws DTwainJavaAPIException;
    public native int DTWAIN_FreeLibrary() throws DTwainJavaAPIException;

    // No argument functions
    // 0 arguments, returning an array of long
    public native long[] DTWAIN_EnumSources() throws DTwainJavaAPIException;

    public native boolean DTWAIN_IsTwainAvailable() throws DTwainJavaAPIException;
    public native long DTWAIN_SysInitialize() throws DTwainJavaAPIException;
    public native int DTWAIN_SysDestroy() throws DTwainJavaAPIException;
    public native long DTWAIN_SelectSource() throws DTwainJavaAPIException;
    public native long DTWAIN_SelectDefaultSource() throws DTwainJavaAPIException;
    public native int DTWAIN_GetLastError() throws DTwainJavaAPIException;
    public native int DTWAIN_GetTwainMode() throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsSessionEnabled() throws DTwainJavaAPIException;
    public native int DTWAIN_EndTwainSession() throws DTwainJavaAPIException;
    public native int DTWAIN_GetCountry() throws DTwainJavaAPIException;
    public native int DTWAIN_GetLanguage() throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsMsgNotifyEnabled() throws DTwainJavaAPIException;
    public native int DTWAIN_GetTwainHwnd() throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAcquiring() throws DTwainJavaAPIException;     
    public native long DTWAIN_CreateAcquisitionArray() throws DTwainJavaAPIException;
    public native int DTWAIN_ClearErrorBuffer() throws DTwainJavaAPIException;
    public native int DTWAIN_GetErrorBufferThreshold() throws DTwainJavaAPIException;
    public native int DTWAIN_GetTwainAvailability() throws DTwainJavaAPIException;
    public native DTwainVersionInfo DTWAIN_GetVersionInfo() throws DTwainJavaAPIException;

    // 1 argument functions
    public native boolean DTWAIN_StartThread(long h) throws DTwainJavaAPIException;
    public native boolean DTWAIN_EndThread(long h) throws DTwainJavaAPIException;
    public native int DTWAIN_SetTwainMode(int v) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCountry(int v) throws DTwainJavaAPIException;
    public native int DTWAIN_SetLanguage(int v) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableMsgNotify(int v) throws DTwainJavaAPIException;
    public native int DTWAIN_OpenSourcesOnSelect(int v) throws DTwainJavaAPIException;
    public native int DTWAIN_SetQueryCapSupport(int v) throws DTwainJavaAPIException;
    public native int DTWAIN_SetTwainTimeout(int v) throws DTwainJavaAPIException;        
    public native int DTWAIN_SetErrorBufferThreshold(int v) throws DTwainJavaAPIException;
    public native int DTWAIN_AppHandlesExceptions(int v) throws DTwainJavaAPIException;   
    public native int DTWAIN_SetTwainDSM(int v) throws DTwainJavaAPIException;

    // 1 argument (argument is Source)
    public native int DTWAIN_OpenSource(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_CloseSource(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_CloseSourceUI(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetDefaultSource(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsSourceAcquiring(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsSourceOpen(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetCurrentPageNum(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetMaxAcquisitions(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetMaxPagesToAcquire(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsUIControllable( long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsUIEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsIndicatorSupported( long Source ) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsIndicatorEnabled( long Source ) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsThumbnailSupported( long Source ) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsThumbnailEnabled( long Source ) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsDeviceEventSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsUIOnlySupported(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_ShowUIOnly(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsPrinterSupported(long Source) throws DTwainJavaAPIException;   
    public native boolean DTWAIN_IsFeederEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsFeederLoaded(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsFeederSupported(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_IsFeederSensitive(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_FeedPage(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_RewindPage(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_ClearPage(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoFeedEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoFeedSupported(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetFeederFuncs(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsPaperDetectable(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsDuplexSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsDuplexEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsCustomDSDataSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_ClearPDFText(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoDeskewSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoDeskewEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoBorderDetectSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoBorderDetectEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsLightPathSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsLampSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsLampEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsLightSourceSupported(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetMaxRetryAttempts(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetCurrentRetryCount(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsSkipImageInfoError(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsExtImageInfoSupported(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_InitExtImageInfo(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetExtImageInfo(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_FreeExtImageInfo(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_FlushAcquiredPages(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsFileSystemSupported( long Source ) throws DTwainJavaAPIException;
    public native int DTWAIN_GetBlankPageAutoDetection(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsBlankPageDetectionOn(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoScanEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsDeviceOnLine( long Source ) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoBrightEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsAutoRotateEnabled(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsRotationSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsPatchCapsSupported(long Source) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsPatchDetectEnabled(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetAllCapsToDefault(long Source) throws DTwainJavaAPIException;

    // 1 argument functions returning array
    public native int[] DTWAIN_EnumSupportedCaps(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumExtendedCaps(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumCustomCaps(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumSourceUnits(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumFileXferFormats(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumCompressionTypes(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPrinterStringModes(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumTwainPrintersArray(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumOrientations(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPaperSizes(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPixelTypes(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumBitDepths(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumJobControls(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumLightPaths(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumLightSources(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_GetLightSources(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumExtImageInfoTypes(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumAlarms(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumNoiseFilters(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPatchMaxRetries(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPatchMaxPriorities(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPatchSearchModes(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPatchTimeOutValues(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_GetPatchPriorities(long Source) throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumPatchPriorities(long Source) throws DTwainJavaAPIException;

    public native String[] DTWAIN_EnumTopCameras(long Source) throws DTwainJavaAPIException;
    public native String[] DTWAIN_EnumBottomCameras(long Source) throws DTwainJavaAPIException;
    public native String[] DTWAIN_EnumCameras(long Source) throws DTwainJavaAPIException;

    // 2 argument functions
    public native boolean DTWAIN_IsCapSupported(long Source, int capValue) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsFileXferSupported(long Source, int lFileType) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableIndicator(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_SetMaxAcquisitions(long Source, int maxAcquisitions) throws DTwainJavaAPIException;
    public native int DTWAIN_SetSourceUnit(long Source, int Unit) throws DTwainJavaAPIException;
    public native String DTWAIN_GetCapDataType(long Source, int capValue) throws DTwainJavaAPIException;
    public native int DTWAIN_GetCapArrayType(long Source, int capValue) throws DTwainJavaAPIException;

    
    public native boolean DTWAIN_IsCompressionSupported(long Source, int lCompression) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsPrinterEnabled(long Source, int printer) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableFeeder(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_EnablePrinter(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableThumbnail(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_ForceAcquireBitDepth(long Source, int lBitDepth) throws DTwainJavaAPIException;
    public native int DTWAIN_SetAvailablePrinters(long Source, int lAvailPrinters) throws DTwainJavaAPIException;
    public native int DTWAIN_SetDeviceNotifications(long Source, int lNotifications) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPrinterStartNumber(long Source, int startNumber) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableAutoFeed(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableDuplex(long Source, boolean enable) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsOrientationSupported(long Source, int orientation) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsPaperSizeSupported(long Source, int paperSize) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsPixelTypeSupported(long Source, int pixelType) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFASCIICompression(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPostScriptType(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFJpegQuality(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFOrientation(long Source, int setting) throws DTwainJavaAPIException;
    
    public native int DTWAIN_SetTIFFInvert(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetTIFFCompressType(long Source, int setting) throws DTwainJavaAPIException;
    
    public native boolean DTWAIN_IsJobControlSupported(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableJobFileHandling(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableAutoDeskew(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableAutoBorderDetect(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_SetLightPath(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableLamp(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_SetMaxRetryAttempts(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCurrentRetryCount(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SkipImageInfoError(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_SetMultipageScanMode(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetAlarmVolume(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableAutoScan(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_ClearBuffers(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_SetFeederAlignment(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetFeederOrder(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetMaxBuffers(long Source, int setting) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsMaxBuffersSupported(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableAutoBright(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_EnableAutoRotate(long Source, boolean enable) throws DTwainJavaAPIException;
    public native int DTWAIN_SetNoiseFilter(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPixelFlavor(long Source, int setting) throws DTwainJavaAPIException;
    public native int DTWAIN_SetRotation(long Source, double setting) throws DTwainJavaAPIException;
    
    public native int DTWAIN_GetSourceUnit          (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetDeviceNotifications (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetDeviceEvent         (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetCompressionSize     (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_EnumTwainPrinters      (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetPrinterStartNumber  (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetDuplexType          (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetLightPath           (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetAlarmVolume         (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetBatteryMinutes      (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetBatteryPercent      (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetFeederAlignment     (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetFeederOrder         (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetMaxBuffers          (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetNoiseFilter         (long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_GetPixelFlavor         (long Source) throws DTwainJavaAPIException;
    public native double DTWAIN_GetRotation         (long Source) throws DTwainJavaAPIException;
    
    public native double DTWAIN_GetContrast(long Source) throws DTwainJavaAPIException;
    public native double DTWAIN_GetBrightness(long Source) throws DTwainJavaAPIException;
    public native double DTWAIN_GetResolution(long Source) throws DTwainJavaAPIException;

    public native int DTWAIN_GetCapOperations(long Source, int capability) throws DTwainJavaAPIException;

    public native double[] DTWAIN_EnumContrastValues(long Source, boolean bExpandIfRange) throws DTwainJavaAPIException;
    public native double[] DTWAIN_EnumBrightnessValues(long Source, boolean bExpandIfRange) throws DTwainJavaAPIException;
    public native double[] DTWAIN_EnumResolutionValues(long Source, boolean bExpandIfRange) throws DTwainJavaAPIException;
    public native int []   DTWAIN_EnumMaxBuffers(long Source, boolean bExpandIfRange) throws DTwainJavaAPIException;
    
    public native int DTWAIN_SetCapValuesInt(long Source, int capValue, int setType, int[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesDouble(long Source, int capValue, int setType, double[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesFrame(long Source, int capValue, int setType, DTwainFrame[] frames) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesString(long Source, int capValue, int setType, String[] vals) throws DTwainJavaAPIException;
    
    public native int DTWAIN_SetCapValuesIntEx(long Source, int capValue, int setType, int containerType, int[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesDoubleEx(long Source, int capValue, int setType, int containerType, double[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesFrameEx(long Source, int capValue, int setType, int containerType, DTwainFrame[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesStringEx(long Source, int capValue, int setType, int containerType, String[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesIntEx2(long Source, int capValue, int setType, int containerType, int nDataType, int[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesDoubleEx2(long Source, int capValue, int setType, int containerType, int nDataType, double[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesFrameEx2(long Source, int capValue, int setType, int containerType, int nDataType, DTwainFrame[] vals) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCapValuesStringEx2(long Source, int capValue, int setType, int containerType, int nDataType, String[] vals) throws DTwainJavaAPIException;
    
    public native int[] DTWAIN_GetCapValuesInt(long Source, int capValue, int getType) throws DTwainJavaAPIException;
    public native double[] DTWAIN_GetCapValuesDouble(long Source, int capValue, int getType) throws DTwainJavaAPIException;
    public native DTwainFrame[] DTWAIN_GetCapValuesFrame(long Source, int capValue, int getType) throws DTwainJavaAPIException;
    public native String[] DTWAIN_GetCapValuesString(long Source, int capValue, int getType) throws DTwainJavaAPIException;
    
    public native int[] DTWAIN_GetCapValuesIntEx(long Source, int capValue, int containerType, int getType) throws DTwainJavaAPIException;
    public native double[] DTWAIN_GetCapValuesDoubleEx(long Source, int capValue, int containerType, int getType) throws DTwainJavaAPIException;
    public native String[] DTWAIN_GetCapValuesStringEx(long Source, int capValue, int containerType, int getType) throws DTwainJavaAPIException;
    public native DTwainFrame[] DTWAIN_GetCapValuesFrameEx(long Source, int capValue, int containerType, int getType) throws DTwainJavaAPIException;
    public native int[] DTWAIN_GetCapValuesIntEx2(long Source, int capValue, int containerType, int nDataType, int getType) throws DTwainJavaAPIException;
    public native double[] DTWAIN_GetCapValuesDoubleEx2(long Source, int capValue, int containerType, int nDataType, int getType) throws DTwainJavaAPIException;
    public native String[] DTWAIN_GetCapValuesStringEx2(long Source, int capValue, int containerType, int nDataType, int getType) throws DTwainJavaAPIException;
    
    public native DTwainFrame[] DTWAIN_GetCapValuesFrameEx2(long Source, int capValue, int containerType, int nDataType, int getType) throws DTwainJavaAPIException;
    public native int DTWAIN_GetCapContainer(long Source, int capValue, int getType);

    public native int DTWAIN_SetFileXferFormat(long Source, int fileType, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCompressionType(long Source, int compression, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPrinter(long Source, int printer, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPrinterStringMode(long Source, int setting, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetOrientation(long Source, int setting, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPaperSize(long Source, int setting, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetBitDepth(long Source, int setting, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetJobControl(long Source, int setting, boolean setCurrent) throws DTwainJavaAPIException;
    public native int DTWAIN_SetManualDuplexMode(long Source, int setting, boolean setCurrent) throws DTwainJavaAPIException;
    public native DTwainAcquireArea DTWAIN_GetAcquireArea(long Source, int getType) throws DTwainJavaAPIException;
    public native DTwainAcquireArea DTWAIN_SetAcquireArea(long Source, int setType, DTwainAcquireArea aArea) throws DTwainJavaAPIException;
    
    public native DTwainImageInfo DTWAIN_GetImageInfo(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetTwainLog(int nFlags, String szFile) throws DTwainJavaAPIException;
    
    // acquisitions
    public native DTwainAcquisitionArray DTWAIN_AcquireNative(long Source, int pixelType, int maxPages, boolean showUI, boolean closeSource) throws DTwainJavaAPIException;
    public native DTwainAcquisitionArray DTWAIN_AcquireBuffered(long Source, int pixelType, int maxPages, boolean showUI, boolean closeSource) throws DTwainJavaAPIException;
    public native int DTWAIN_AcquireFile(long Source, String filename, int fileType, int fileFlags, int pixelType, int maxPages, boolean showUI, boolean closeSource) throws DTwainJavaAPIException;
    
    // custom ds data
    public native byte[] DTWAIN_GetCustomDSData(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCustomDSData(long Source, byte[] customData) throws DTwainJavaAPIException;
    
    public native int DTWAIN_SetAcquireImageScale(long Source, double xScale, double yScale) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFPageSize(long Source, int pageSize, double customWidth, double customHeight) throws DTwainJavaAPIException;
    
    // String functions
    public native DTwainAppInfo DTWAIN_GetAppInfo() throws DTwainJavaAPIException;
    public native DTwainSourceInfo DTWAIN_GetSourceInfo(long Source) throws DTwainJavaAPIException;
    public native String DTWAIN_GetAuthor(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetAuthor(long Source, String author) throws DTwainJavaAPIException;
    public native String DTWAIN_GetCaption(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetCaption(long Source, String caption) throws DTwainJavaAPIException;
    public native long DTWAIN_SelectSourceByName(String sourceName) throws DTwainJavaAPIException;
    public native String DTWAIN_GetNameFromCap(int capability) throws DTwainJavaAPIException;
    public native int DTWAIN_GetCapFromName(String capName) throws DTwainJavaAPIException;
    public native int DTWAIN_TwainSave(String sCmd) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFAuthor(long Source, String sAuthor) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFCreator(long Source, String sCreator) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFTitle(long Source, String sTitle) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFSubject(long Source, String sSubject) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFKeywords(long Source, String sKeywords) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFEncryption(long Source, boolean useEncryption, String userPass, String ownerPass, int permissions, boolean useStringEncrypt) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPostScriptTitle(long Source, String sTitle) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFPageScale(long Source, int scaleOpts, double xScale, double yScale);
    public native boolean DTWAIN_IsDIBBlank(long hDib, double threshHold) throws DTwainJavaAPIException;
    public native int DTWAIN_SetTempFileDirectory(String dirName) throws DTwainJavaAPIException;
    public native String DTWAIN_GetTempFileDirectory() throws DTwainJavaAPIException;
    public native int DTWAIN_LogMessage(String sMsg) throws DTwainJavaAPIException;
    public native long DTWAIN_SelectSource2(String sTitle, int xPos, int yPos, int options) throws DTwainJavaAPIException;
    public native String DTWAIN_GetErrorString(int errorNum) throws DTwainJavaAPIException;
    public native int DTWAIN_AcquireFileEx(long Source, String[] filenames, int fileType, int fileFlags, int pixelType, int maxPages, boolean showUI, boolean closeSource) throws DTwainJavaAPIException;
    public native DTwainImageData DTWAIN_GetCurrentAcquiredImage(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetBlankPageDetection(long Source, double threshold, int autodetect, boolean bSet) throws DTwainJavaAPIException;
    public native int DTWAIN_SetAcquireArea2(long Source, DTwainAcquireArea area, int flags) throws DTwainJavaAPIException;
    public native DTwainAcquireArea DTWAIN_GetAcquireArea2(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetAppInfo(DTwainAppInfo aInfo) throws DTwainJavaAPIException;
    //  
    public native int DTWAIN_InitOCRInterface() throws DTwainJavaAPIException;
    public native long DTWAIN_SelectOCREngine() throws DTwainJavaAPIException;
    public native long DTWAIN_SelectDefaultOCREngine() throws DTwainJavaAPIException;
    public native long DTWAIN_SelectOCREngineByName(String name) throws DTwainJavaAPIException;
    public native long[] DTWAIN_EnumOCRInterfaces() throws DTwainJavaAPIException;
    public native int[] DTWAIN_EnumOCRSupportedCaps(long Engine) throws DTwainJavaAPIException;
    public native int[] DTWAIN_GetOCRCapValuesInt(long Engine, int OCRCapValue, int getType) throws DTwainJavaAPIException;
    public native String[] DTWAIN_GetOCRCapValuesString(long Engine, int OCRCapValue, int getType) throws DTwainJavaAPIException;
    public native int DTWAIN_SetOCRCapValuesInt(long Engine, int OCRCapValue, int SetType, int[] CapValues) throws DTwainJavaAPIException;
    public native int DTWAIN_SetOCRCapValuesString(long Engine, int OCRCapValue, int SetType, String[] CapValues) throws DTwainJavaAPIException;
    public native int DTWAIN_ShutdownOCREngine(long OCREngine) throws DTwainJavaAPIException;
    public native boolean DTWAIN_IsOCREngineActivated(long OCREngine) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFOCRConversion(long Engine, int pageType, int fileType, int pixelType, int bitDepth, int options) throws DTwainJavaAPIException;
    public native int DTWAIN_SetPDFOCRMode(long Source, boolean bSet) throws DTwainJavaAPIException;
    public native int DTWAIN_ExecuteOCR(long Engine, String szFileName, int startPage, int endPage) throws DTwainJavaAPIException;
    public native DTwainOCRInfo DTWAIN_GetOCRInfo(long Engine) throws DTwainJavaAPIException;
    public native byte [] DTWAIN_GetOCRText(long Engine, int pageNum) throws DTwainJavaAPIException;

    public native DTwainBufferedStripInfo DTWAIN_CreateBufferedStripInfo(long Source) throws DTwainJavaAPIException;
    public native int DTWAIN_SetBufferedTransferInfo(long Source, DTwainBufferedStripInfo info) throws DTwainJavaAPIException;
    public native int DTWAIN_GetBufferedStripData(long Source, DTwainBufferedStripInfo info) throws DTwainJavaAPIException;
    public native int DTWAIN_EndBufferedTransfer(long Source, DTwainBufferedStripInfo info) throws DTwainJavaAPIException;
    
    public native int DTWAIN_ResetCapValues(long Source, int nCapValue) throws DTwainJavaAPIException;
    
    static
    {
        s_DLLMap = new TreeMap<Integer, DTwainModuleNames>();
        s_DLLMap.put(JNIDLLName_32, new DTwainModuleNames("DTwain32", "dtwainjni32"));
        s_DLLMap.put(JNIDLLName_32U, new DTwainModuleNames("DTwain32U","dtwainjni32u"));
        s_DLLMap.put(JNIDLLName_64, new DTwainModuleNames("DTwain64", "dtwainjni64"));
        s_DLLMap.put(JNIDLLName_64U, new DTwainModuleNames("DTwain64U", "dtwainjni64u"));
    }
}