#include "DTWAINJNIGlobals.h"
#include <algorithm>

#define NAME_TO_STRING(x) #x
#define ADD_FUNCTION_ENTRY(m, fName)  (g_##m##Map)->m_FnMap[NAME_TO_STRING(fName)] = fName;\
									   g_ptrBase.insert((g_##m##Map).get());

#define INITIALIZE_MAP_ENTRY(Entry)   g_##Entry##Map (FnGlobal##Entry##Ptr(new FnGlobal##Entry))

static bool UndefinedFunc(FnGlobalBase* fn)
{ return fn->isEmpty(); }

DTWAINJNIGlobals::DTWAINJNIGlobals() : g_DTWAINModule(0), 
		INITIALIZE_MAP_ENTRY(LV),
		INITIALIZE_MAP_ENTRY(LS),
		INITIALIZE_MAP_ENTRY(SV),
		INITIALIZE_MAP_ENTRY(HV),
		INITIALIZE_MAP_ENTRY(AV),
		INITIALIZE_MAP_ENTRY(HandleV),
		INITIALIZE_MAP_ENTRY(LL),
		INITIALIZE_MAP_ENTRY(LSa),
		INITIALIZE_MAP_ENTRY(LSL),
		INITIALIZE_MAP_ENTRY(LSF),
		INITIALIZE_MAP_ENTRY(LSl),    
		INITIALIZE_MAP_ENTRY(LSf),    
		INITIALIZE_MAP_ENTRY(LSLl),   
		INITIALIZE_MAP_ENTRY(LSaB),   
		INITIALIZE_MAP_ENTRY(LSLLa),  
		INITIALIZE_MAP_ENTRY(LSLL),   
		INITIALIZE_MAP_ENTRY(LSFF),   
		INITIALIZE_MAP_ENTRY(LSLFF),  
		INITIALIZE_MAP_ENTRY(LST),    
		INITIALIZE_MAP_ENTRY(LLTL),   
		INITIALIZE_MAP_ENTRY(St),     
		INITIALIZE_MAP_ENTRY(Lt),     
		INITIALIZE_MAP_ENTRY(LSt),    
		INITIALIZE_MAP_ENTRY(LTL),    
		INITIALIZE_MAP_ENTRY(LSLttLL),
		INITIALIZE_MAP_ENTRY(SHtLLL), 
		INITIALIZE_MAP_ENTRY(LSFLL),  
		INITIALIZE_MAP_ENTRY(LHF),    
		INITIALIZE_MAP_ENTRY(BH),     
		INITIALIZE_MAP_ENTRY(Ltttt),  
		INITIALIZE_MAP_ENTRY(LTTTT),  
		INITIALIZE_MAP_ENTRY(LSLLLa), 
		INITIALIZE_MAP_ENTRY(LSLLLLa),
		INITIALIZE_MAP_ENTRY(LSLLA),  
		INITIALIZE_MAP_ENTRY(LSLLLA), 
		INITIALIZE_MAP_ENTRY(LSLLLLA),
		INITIALIZE_MAP_ENTRY(LLt),    
		INITIALIZE_MAP_ENTRY(HandleS),
		INITIALIZE_MAP_ENTRY(LSffffl),
		INITIALIZE_MAP_ENTRY(LSFFFFLL),  
		INITIALIZE_MAP_ENTRY(LSaLLLLLLl),
		INITIALIZE_MAP_ENTRY(LSlll),     
		INITIALIZE_MAP_ENTRY(La),
		INITIALIZE_MAP_ENTRY(LLa),
		INITIALIZE_MAP_ENTRY(LLLLa),
		INITIALIZE_MAP_ENTRY(LLLLA),
		INITIALIZE_MAP_ENTRY(LLLLLLL),
		INITIALIZE_MAP_ENTRY(LLtLL),
        INITIALIZE_MAP_ENTRY(OV),
        INITIALIZE_MAP_ENTRY(Ot),
        INITIALIZE_MAP_ENTRY(HLLTLlL),
	    INITIALIZE_MAP_ENTRY(LSH),
		INITIALIZE_MAP_ENTRY(LStLLLLLLl),
		INITIALIZE_MAP_ENTRY(LSlllllll),
        INITIALIZE_MAP_ENTRY(LOLLa),
        INITIALIZE_MAP_ENTRY(LOLLA),
        INITIALIZE_MAP_ENTRY(LO),
        INITIALIZE_MAP_ENTRY(LOLLLLL),
        INITIALIZE_MAP_ENTRY(LOtLL),
        INITIALIZE_MAP_ENTRY(HOLTLlL)
	{
		// 0 arguments
		ADD_FUNCTION_ENTRY(AV,DTWAIN_CreateAcquisitionArray);                 
		ADD_FUNCTION_ENTRY(AV,DTWAIN_CreateAcquisitionArray);                 
		ADD_FUNCTION_ENTRY(HandleV,DTWAIN_SysInitialize);                                                 
		ADD_FUNCTION_ENTRY(HV,DTWAIN_GetTwainHwnd);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_ClearErrorBuffer);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_EndTwainSession);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_GetCountry);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_GetErrorBufferThreshold);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_GetLanguage);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_GetLastError);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_GetTwainAvailability);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_GetTwainMode);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_InitOCRInterface);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_IsAcquiring);                                 	
		ADD_FUNCTION_ENTRY(LV,DTWAIN_IsMsgNotifyEnabled);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_IsSessionEnabled);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_IsTwainAvailable);                                 
		ADD_FUNCTION_ENTRY(OV,DTWAIN_SelectDefaultOCREngine);                                 
		ADD_FUNCTION_ENTRY(OV,DTWAIN_SelectOCREngine);                                 
		ADD_FUNCTION_ENTRY(LV,DTWAIN_SysDestroy);                                 
		ADD_FUNCTION_ENTRY(SV,DTWAIN_SelectDefaultSource);                                 
		ADD_FUNCTION_ENTRY(SV,DTWAIN_SelectSource);                                 

		// 1 argument LONG
		ADD_FUNCTION_ENTRY(LL,DTWAIN_AppHandlesExceptions);    
		ADD_FUNCTION_ENTRY(LL,DTWAIN_EnableMsgNotify);         
		ADD_FUNCTION_ENTRY(LL,DTWAIN_OpenSourcesOnSelect);     
		ADD_FUNCTION_ENTRY(LL,DTWAIN_SetCountry);              
		ADD_FUNCTION_ENTRY(LL,DTWAIN_SetErrorBufferThreshold); 
		ADD_FUNCTION_ENTRY(LL,DTWAIN_SetLanguage);             
		ADD_FUNCTION_ENTRY(LL,DTWAIN_SetQueryCapSupport);      
		ADD_FUNCTION_ENTRY(LL,DTWAIN_SetTwainDSM);             
		ADD_FUNCTION_ENTRY(LL,DTWAIN_SetTwainTimeout);         
		ADD_FUNCTION_ENTRY(LL,DTWAIN_SetTwainMode);
        ADD_FUNCTION_ENTRY(Ot, DTWAIN_SelectOCREngineByName);

		// 1 argument DTWAIN_SOURCE
		ADD_FUNCTION_ENTRY(LS,DTWAIN_ClearPage);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_ClearPDFText);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_CloseSource);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_CloseSourceUI);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_FeedPage);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_FlushAcquiredPages);         
		ADD_FUNCTION_ENTRY(LS,DTWAIN_FreeExtImageInfo);           
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetBlankPageAutoDetection);  
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetCurrentPageNum);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetCurrentRetryCount);       
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetExtImageInfo);            
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetFeederFuncs);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetMaxAcquisitions);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetMaxPagesToAcquire);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_GetMaxRetryAttempts);        
		ADD_FUNCTION_ENTRY(LS,DTWAIN_InitExtImageInfo);           
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoBorderDetectEnabled);  
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoBorderDetectSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoBrightEnabled); 
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoDeskewEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoDeskewSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoFeedEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoFeedSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoRotateEnabled); 
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsAutoScanEnabled);          
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsBlankPageDetectionOn);     
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsCustomDSDataSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsDeviceEventSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsDeviceOnLine);             
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsDuplexEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsDuplexSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsExtImageInfoSupported);    
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsFeederEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsFeederLoaded);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsFeederSensitive);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsFeederSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsFileSystemSupported);      
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsIndicatorEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsIndicatorSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsLampEnabled);              
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsLampSupported);            
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsLightPathSupported);       
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsLightSourceSupported);     
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsPaperDetectable);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsPatchCapsSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsPatchDetectEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsPrinterSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsRotationSupported); 
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsSkipImageInfoError);       
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsSourceAcquiring);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsSourceOpen);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsThumbnailEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsThumbnailSupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsUIControllable);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsUIEnabled);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_IsUIOnlySupported);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_OpenSource);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_RewindPage);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_SetAllCapsToDefault);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_SetDefaultSource);
		ADD_FUNCTION_ENTRY(LS,DTWAIN_ShowUIOnly);
		ADD_FUNCTION_ENTRY(BH,DTWAIN_StartThread);                                                 
		ADD_FUNCTION_ENTRY(BH,DTWAIN_EndThread);                                                 

		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumSupportedCaps);       
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumExtendedCaps);        
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumCustomCaps);          
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumSourceUnits);         
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumFileXferFormats);     
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumCompressionTypes);    
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPrinterStringModes);  
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumTwainPrintersArray);  
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumOrientations);        
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPaperSizes);          
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPixelTypes);          
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumBitDepths);           

		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumJobControls);         
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumLightPaths);          
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumLightSources);        
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_GetLightSources);         
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumExtImageInfoTypes);   
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumAlarms);              
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumNoiseFilters);        
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPatchMaxRetries);     
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPatchMaxPriorities);  
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPatchSearchModes);    
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPatchTimeOutValues);  
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_GetPatchPriorities);      
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumPatchPriorities);     
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumTopCameras);          
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumBottomCameras);       
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumCameras);             

		// 2 argument functions (DTWAIN_SOURCE, LONG) returning LONG
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsCapSupported);
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_GetCapDataType);
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetMaxAcquisitions);
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetSourceUnit);
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsFileXferSupported);
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableIndicator);
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_GetCapArrayType);

		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsCompressionSupported);   
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsPrinterEnabled);         
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableFeeder);             
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnablePrinter);            
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableThumbnail);          
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_ForceAcquireBitDepth);     
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetAvailablePrinters);     
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetDeviceNotifications);   
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetPrinterStartNumber);    
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableAutoFeed);           
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableDuplex);             
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsOrientationSupported);   
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsPaperSizeSupported);     
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsPixelTypeSupported);     
		//    ADD_FUNCTION_ENTRY(g_LSL,DTWAIN_SetPDFCompression);        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetPDFASCIICompression);   
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetPostScriptType);        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetPDFJpegQuality);        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetTIFFInvert);        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetTIFFCompressType);        

		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsJobControlSupported);                 
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableJobFileHandling);                 
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableAutoDeskew);                      
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableAutoBorderDetect);                
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetLightPath);                          
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableLamp);                            
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetMaxRetryAttempts);                   
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetCurrentRetryCount);                  
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SkipImageInfoError);                    
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetMultipageScanMode);                  
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetAlarmVolume);                        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableAutoScan);                        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_ClearBuffers);                          
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetFeederAlignment);                    
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetFeederOrder);                        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetMaxBuffers);                         
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_IsMaxBuffersSupported);                 
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableAutoBright);                      
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_EnableAutoRotate);                      
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetNoiseFilter);                        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetPixelFlavor);                        
		ADD_FUNCTION_ENTRY(LSL,DTWAIN_SetPDFOrientation);
		ADD_FUNCTION_ENTRY(LSF,DTWAIN_SetRotation);                           

		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetSourceUnit);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetDeviceNotifications);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetDeviceEvent);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetCompressionSize);
		ADD_FUNCTION_ENTRY(LSa,DTWAIN_EnumTwainPrinters);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetPrinterStartNumber);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetDuplexType);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetLightPath);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetAlarmVolume);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetBatteryMinutes);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetBatteryPercent);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetFeederAlignment);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetFeederOrder);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetMaxBuffers);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetNoiseFilter);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetPixelFlavor);
		ADD_FUNCTION_ENTRY(LSl,DTWAIN_GetPixelFlavor);
		ADD_FUNCTION_ENTRY(LSf,DTWAIN_GetRotation);
		ADD_FUNCTION_ENTRY(LSf,DTWAIN_GetContrast);
		ADD_FUNCTION_ENTRY(LSf,DTWAIN_GetBrightness);
		ADD_FUNCTION_ENTRY(LSf,DTWAIN_GetResolution);
		ADD_FUNCTION_ENTRY(LSLl,DTWAIN_GetCapOperations);

		ADD_FUNCTION_ENTRY(LSaB,DTWAIN_EnumContrastValues);
		ADD_FUNCTION_ENTRY(LSaB,DTWAIN_EnumResolutionValues);
		ADD_FUNCTION_ENTRY(LSaB,DTWAIN_EnumBrightnessValues);
		ADD_FUNCTION_ENTRY(LSaB,DTWAIN_EnumMaxBuffers);

		ADD_FUNCTION_ENTRY(LSLLa, DTWAIN_SetCapValues);
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetFileXferFormat);
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_GetCapContainer);
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetCompressionType);
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetPrinter);           
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetPrinterStringMode); 
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetOrientation);       
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetPaperSize);         
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetBitDepth);          
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetJobControl);        
		ADD_FUNCTION_ENTRY(LSLL, DTWAIN_SetManualDuplexMode);  

		ADD_FUNCTION_ENTRY(LSFF, DTWAIN_SetAcquireImageScale);

		ADD_FUNCTION_ENTRY(LSLFF, DTWAIN_SetPDFPageSize);
		ADD_FUNCTION_ENTRY(LSLFF, DTWAIN_SetPDFPageScale);

		// string functions
		ADD_FUNCTION_ENTRY(LST, DTWAIN_GetAuthor);
		ADD_FUNCTION_ENTRY(LST, DTWAIN_GetCaption);

		ADD_FUNCTION_ENTRY(LLTL, DTWAIN_GetNameFromCap);
		ADD_FUNCTION_ENTRY(LLTL, DTWAIN_GetErrorString);
		ADD_FUNCTION_ENTRY(St, DTWAIN_SelectSourceByName);
		ADD_FUNCTION_ENTRY(Lt, DTWAIN_GetCapFromName);
		ADD_FUNCTION_ENTRY(Lt, DTWAIN_SetTempFileDirectory);
		ADD_FUNCTION_ENTRY(Lt, DTWAIN_LogMessage);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetCaption);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetAuthor);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetPDFAuthor);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetPDFCreator);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetPDFTitle);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetPDFSubject);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetPDFKeywords);
		ADD_FUNCTION_ENTRY(LSt, DTWAIN_SetPostScriptTitle);

		ADD_FUNCTION_ENTRY(LTL, DTWAIN_GetTempFileDirectory);
		ADD_FUNCTION_ENTRY(LSLttLL, DTWAIN_SetPDFEncryption);
		ADD_FUNCTION_ENTRY(SHtLLL, DTWAIN_SelectSource2);
		ADD_FUNCTION_ENTRY(LSFLL, DTWAIN_SetBlankPageDetection);
		ADD_FUNCTION_ENTRY(LHF, DTWAIN_IsDIBBlank);
		ADD_FUNCTION_ENTRY(Ltttt, DTWAIN_SetAppInfo);
		ADD_FUNCTION_ENTRY(LTTTT, DTWAIN_GetAppInfo);
		ADD_FUNCTION_ENTRY(LSLLLa, DTWAIN_SetCapValuesEx);
		ADD_FUNCTION_ENTRY(LSLLLLa, DTWAIN_SetCapValuesEx2);
		ADD_FUNCTION_ENTRY(LSLLA, DTWAIN_GetCapValues);
		ADD_FUNCTION_ENTRY(LSLLLA, DTWAIN_GetCapValuesEx);
		ADD_FUNCTION_ENTRY(LSLLLLA, DTWAIN_GetCapValuesEx2);
		ADD_FUNCTION_ENTRY(LLt, DTWAIN_SetTwainLog);
		ADD_FUNCTION_ENTRY(HandleS, DTWAIN_GetCurrentAcquiredImage);
		ADD_FUNCTION_ENTRY(LSffffl, DTWAIN_GetAcquireArea2);
		ADD_FUNCTION_ENTRY(LSFFFFLL, DTWAIN_SetAcquireArea2);
		ADD_FUNCTION_ENTRY(LSaLLLLLLl, DTWAIN_AcquireFileEx);
		ADD_FUNCTION_ENTRY(LSlll, DTWAIN_GetAcquireStripSizes);
		ADD_FUNCTION_ENTRY(La, DTWAIN_EnumOCRInterfaces);
		ADD_FUNCTION_ENTRY(LLa, DTWAIN_EnumOCRSupportedCaps);
		ADD_FUNCTION_ENTRY(LOLLa, DTWAIN_GetOCRCapValues);
		ADD_FUNCTION_ENTRY(LOLLA, DTWAIN_SetOCRCapValues);
		ADD_FUNCTION_ENTRY(LO, DTWAIN_ShutdownOCREngine);
		ADD_FUNCTION_ENTRY(LO, DTWAIN_IsOCREngineActivated);
		ADD_FUNCTION_ENTRY(LOLLLLL, DTWAIN_SetPDFOCRConversion);
		ADD_FUNCTION_ENTRY(LSL, DTWAIN_SetPDFOCRMode);
		ADD_FUNCTION_ENTRY(LOtLL, DTWAIN_ExecuteOCR);
		ADD_FUNCTION_ENTRY(HOLTLlL, DTWAIN_GetOCRText);
		ADD_FUNCTION_ENTRY(LSH, DTWAIN_SetAcquireStripBuffer);
		ADD_FUNCTION_ENTRY(LSlllllll, DTWAIN_GetAcquireStripData);
		ADD_FUNCTION_ENTRY(LStLLLLLLl, DTWAIN_AcquireFile);

		GlobalSetType::iterator it = std::find_if(g_ptrBase.begin(), g_ptrBase.end(), UndefinedFunc);
		if ( it != g_ptrBase.end())
			throw "Function Not Found";
	}
