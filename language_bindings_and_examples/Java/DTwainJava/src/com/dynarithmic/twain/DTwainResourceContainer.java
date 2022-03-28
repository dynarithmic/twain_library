package com.dynarithmic.twain;
import java.util.Locale;
import java.util.MissingResourceException;
import java.util.ResourceBundle;
import java.util.TreeMap;

public class DTwainResourceContainer
{
	private static final TreeMap<Integer, String> allLocales = new TreeMap<Integer, String>();
	private static final TreeMap<Integer, String> allErrorCodes = new TreeMap<Integer, String>();

	static {
		allLocales.put(DTwainJavaAPIConstants.DTWAIN_RES_GERMAN, "de");
		allLocales.put(DTwainJavaAPIConstants.DTWAIN_RES_ENGLISH, "en");
		allLocales.put(DTwainJavaAPIConstants.DTWAIN_RES_FRENCH, "fr");
		allLocales.put(DTwainJavaAPIConstants.DTWAIN_RES_ITALIAN, "it");
		allLocales.put(DTwainJavaAPIConstants.DTWAIN_RES_SPANISH, "es");
		allLocales.put(DTwainJavaAPIConstants.DTWAIN_RES_DUTCH, "nl");

                // error codes
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_HANDLE                      , "DTWAIN_ERR_BAD_HANDLE");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_SOURCE                      , "DTWAIN_ERR_BAD_SOURCE");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_ARRAY                       , "DTWAIN_ERR_BAD_ARRAY");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_WRONG_ARRAY_TYPE                , "DTWAIN_ERR_WRONG_ARRAY_TYPE");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INDEX_BOUNDS                    , "DTWAIN_ERR_INDEX_BOUNDS");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OUT_OF_MEMORY                   , "DTWAIN_ERR_OUT_OF_MEMORY");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NULL_WINDOW                     , "DTWAIN_ERR_NULL_WINDOW");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_PIXTYPE                     , "DTWAIN_ERR_BAD_PIXTYPE");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_CONTAINER                   , "DTWAIN_ERR_BAD_CONTAINER");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NO_SESSION                      , "DTWAIN_ERR_NO_SESSION");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_ACQUIRE_NUM                 , "DTWAIN_ERR_BAD_ACQUIRE_NUM");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_CAP                         , "DTWAIN_ERR_BAD_CAP");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_CAP_NO_SUPPORT                  , "DTWAIN_ERR_CAP_NO_SUPPORT");                                       
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TWAIN                           , "DTWAIN_ERR_TWAIN");                                                
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_HOOK_FAILED                     , "DTWAIN_ERR_HOOK_FAILED");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_FILENAME                    , "DTWAIN_ERR_BAD_FILENAME");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_EMPTY_ARRAY                     , "DTWAIN_ERR_EMPTY_ARRAY");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_FILE_FORMAT                     , "DTWAIN_ERR_FILE_FORMAT");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_DIB_PAGE                    , "DTWAIN_ERR_BAD_DIB_PAGE");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_SOURCE_ACQUIRING                , "DTWAIN_ERR_SOURCE_ACQUIRING");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_PARAM                   , "DTWAIN_ERR_INVALID_PARAM");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_RANGE                   , "DTWAIN_ERR_INVALID_RANGE");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_UI_ERROR                        , "DTWAIN_ERR_UI_ERROR");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_UNIT                        , "DTWAIN_ERR_BAD_UNIT");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_LANGDLL_NOT_FOUND               , "DTWAIN_ERR_LANGDLL_NOT_FOUND");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_SOURCE_NOT_OPEN                 , "DTWAIN_ERR_SOURCE_NOT_OPEN");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED       , "DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED");                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_UIONLY_NOT_SUPPORTED            , "DTWAIN_ERR_UIONLY_NOT_SUPPORTED");                                 
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_UI_ALREADY_OPENED               , "DTWAIN_ERR_UI_ALREADY_OPENED");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_CAPSET_NOSUPPORT                , "DTWAIN_ERR_CAPSET_NOSUPPORT");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NO_FILE_XFER                    , "DTWAIN_ERR_NO_FILE_XFER");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_BITDEPTH                , "DTWAIN_ERR_INVALID_BITDEPTH");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NO_CAPS_DEFINED                 , "DTWAIN_ERR_NO_CAPS_DEFINED");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TILES_NOT_SUPPORTED             , "DTWAIN_ERR_TILES_NOT_SUPPORTED");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_DTWAIN_FRAME            , "DTWAIN_ERR_INVALID_DTWAIN_FRAME");                                 
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_LIMITED_VERSION                 , "DTWAIN_ERR_LIMITED_VERSION");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NO_FEEDER                       , "DTWAIN_ERR_NO_FEEDER");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NO_FEEDER_QUERY                 , "DTWAIN_ERR_NO_FEEDER_QUERY");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_EXCEPTION_ERROR                 , "DTWAIN_ERR_EXCEPTION_ERROR");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALID_STATE                   , "DTWAIN_ERR_INVALID_STATE");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_UNSUPPORTED_EXTINFO             , "DTWAIN_ERR_UNSUPPORTED_EXTINFO");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DLLRESOURCE_NOTFOUND            , "DTWAIN_ERR_DLLRESOURCE_NOTFOUND");                                 
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOT_INITIALIZED                 , "DTWAIN_ERR_NOT_INITIALIZED");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NO_SOURCES                      , "DTWAIN_ERR_NO_SOURCES");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TWAIN_NOT_INSTALLED             , "DTWAIN_ERR_TWAIN_NOT_INSTALLED");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_WRONG_THREAD                    , "DTWAIN_ERR_WRONG_THREAD");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BAD_CAPTYPE                     , "DTWAIN_ERR_BAD_CAPTYPE");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_UNKNOWN_CAPDATATYPE             , "DTWAIN_ERR_UNKNOWN_CAPDATATYPE");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DEMO_NOFILETYPE                 , "DTWAIN_ERR_DEMO_NOFILETYPE");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_LAST_1                          , "DTWAIN_ERR_LAST_1");                                               
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_LOW_MEMORY                       , "TWAIN_ERR_LOW_MEMORY");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FALSE_ALARM                      , "TWAIN_ERR_FALSE_ALARM");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_BUMMER                           , "TWAIN_ERR_BUMMER");                                                
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_NODATASOURCE                     , "TWAIN_ERR_NODATASOURCE");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_MAXCONNECTIONS                   , "TWAIN_ERR_MAXCONNECTIONS");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_OPERATIONERROR                   , "TWAIN_ERR_OPERATIONERROR");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_BADCAPABILITY                    , "TWAIN_ERR_BADCAPABILITY");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_BADVALUE                         , "TWAIN_ERR_BADVALUE");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_BADPROTOCOL                      , "TWAIN_ERR_BADPROTOCOL");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_SEQUENCEERROR                    , "TWAIN_ERR_SEQUENCEERROR");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_BADDESTINATION                   , "TWAIN_ERR_BADDESTINATION");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_CAPNOTSUPPORTED                  , "TWAIN_ERR_CAPNOTSUPPORTED");                                       
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_CAPBADOPERATION                  , "TWAIN_ERR_CAPBADOPERATION");                                       
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_CAPSEQUENCEERROR                 , "TWAIN_ERR_CAPSEQUENCEERROR");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FILEPROTECTEDERROR               , "TWAIN_ERR_FILEPROTECTEDERROR");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FILEEXISTERROR                   , "TWAIN_ERR_FILEEXISTERROR");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FILENOTFOUND                     , "TWAIN_ERR_FILENOTFOUND");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_DIRNOTEMPTY                      , "TWAIN_ERR_DIRNOTEMPTY");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FEEDERJAMMED                     , "TWAIN_ERR_FEEDERJAMMED");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FEEDERMULTPAGES                  , "TWAIN_ERR_FEEDERMULTPAGES");                                       
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FEEDERWRITEERROR                 , "TWAIN_ERR_FEEDERWRITEERROR");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_DEVICEOFFLINE                    , "TWAIN_ERR_DEVICEOFFLINE");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_NULL_CONTAINER                   , "TWAIN_ERR_NULL_CONTAINER");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_INTERLOCK                        , "TWAIN_ERR_INTERLOCK");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_DAMAGEDCORNER                    , "TWAIN_ERR_DAMAGEDCORNER");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_FOCUSERROR                       , "TWAIN_ERR_FOCUSERROR");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_DOCTOOLIGHT                      , "TWAIN_ERR_DOCTOOLIGHT");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_DOCTOODARK                       , "TWAIN_ERR_DOCTOODARK");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.TWAIN_ERR_NOMEDIA                          , "TWAIN_ERR_NOMEDIA");                                               
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_FILEXFERSTART                   , "DTWAIN_ERR_FILEXFERSTART");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_MEM                             , "DTWAIN_ERR_MEM");                                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_FILEOPEN                        , "DTWAIN_ERR_FILEOPEN");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_FILEREAD                        , "DTWAIN_ERR_FILEREAD");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_FILEWRITE                       , "DTWAIN_ERR_FILEWRITE");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BADPARAM                        , "DTWAIN_ERR_BADPARAM");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDBMP                      , "DTWAIN_ERR_INVALIDBMP");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BMPRLE                          , "DTWAIN_ERR_BMPRLE");                                               
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_RESERVED1                       , "DTWAIN_ERR_RESERVED1");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDJPG                      , "DTWAIN_ERR_INVALIDJPG");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DC                              , "DTWAIN_ERR_DC");                                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DIB                             , "DTWAIN_ERR_DIB");                                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_RESERVED2                       , "DTWAIN_ERR_RESERVED2");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NORESOURCE                      , "DTWAIN_ERR_NORESOURCE");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_CALLBACKCANCEL                  , "DTWAIN_ERR_CALLBACKCANCEL");                                       
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDPNG                      , "DTWAIN_ERR_INVALIDPNG");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PNGCREATE                       , "DTWAIN_ERR_PNGCREATE");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INTERNAL                        , "DTWAIN_ERR_INTERNAL");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_FONT                            , "DTWAIN_ERR_FONT");                                                 
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INTTIFF                         , "DTWAIN_ERR_INTTIFF");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDTIFF                     , "DTWAIN_ERR_INVALIDTIFF");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOTIFFLZW                       , "DTWAIN_ERR_NOTIFFLZW");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDPCX                      , "DTWAIN_ERR_INVALIDPCX");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_CREATEBMP                       , "DTWAIN_ERR_CREATEBMP");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOLINES                         , "DTWAIN_ERR_NOLINES");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_GETDIB                          , "DTWAIN_ERR_GETDIB");                                               
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NODEVOP                         , "DTWAIN_ERR_NODEVOP");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDWMF                      , "DTWAIN_ERR_INVALIDWMF");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DEPTHMISMATCH                   , "DTWAIN_ERR_DEPTHMISMATCH");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BITBLT                          , "DTWAIN_ERR_BITBLT");                                               
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BUFTOOSMALL                     , "DTWAIN_ERR_BUFTOOSMALL");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TOOMANYCOLORS                   , "DTWAIN_ERR_TOOMANYCOLORS");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDTGA                      , "DTWAIN_ERR_INVALIDTGA");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOTGATHUMBNAIL                  , "DTWAIN_ERR_NOTGATHUMBNAIL");                                       
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_RESERVED3                       , "DTWAIN_ERR_RESERVED3");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_CREATEDIB                       , "DTWAIN_ERR_CREATEDIB");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOLZW                           , "DTWAIN_ERR_NOLZW");                                                
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_SELECTOBJ                       , "DTWAIN_ERR_SELECTOBJ");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BADMANAGER                      , "DTWAIN_ERR_BADMANAGER");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OBSOLETE                        , "DTWAIN_ERR_OBSOLETE");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_CREATEDIBSECTION                , "DTWAIN_ERR_CREATEDIBSECTION");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_SETWINMETAFILEBITS              , "DTWAIN_ERR_SETWINMETAFILEBITS");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_GETWINMETAFILEBITS              , "DTWAIN_ERR_GETWINMETAFILEBITS");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PAXPWD                          , "DTWAIN_ERR_PAXPWD");                                               
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDPAX                      , "DTWAIN_ERR_INVALIDPAX");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOSUPPORT                       , "DTWAIN_ERR_NOSUPPORT");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDPSD                      , "DTWAIN_ERR_INVALIDPSD");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PSDNOTSUPPORTED                 , "DTWAIN_ERR_PSDNOTSUPPORTED");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DECRYPT                         , "DTWAIN_ERR_DECRYPT");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_ENCRYPT                         , "DTWAIN_ERR_ENCRYPT");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_COMPRESSION                     , "DTWAIN_ERR_COMPRESSION");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_DECOMPRESSION                   , "DTWAIN_ERR_DECOMPRESSION");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDTLA                      , "DTWAIN_ERR_INVALIDTLA");                                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDWBMP                     , "DTWAIN_ERR_INVALIDWBMP");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOTIFFTAG                       , "DTWAIN_ERR_NOTIFFTAG");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOLOCALSTORAGE                  , "DTWAIN_ERR_NOLOCALSTORAGE");                                       
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDEXIF                     , "DTWAIN_ERR_INVALIDEXIF");                                          
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_NOEXIFSTRING                    , "DTWAIN_ERR_NOEXIFSTRING");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TIFFDLL32NOTFOUND               , "DTWAIN_ERR_TIFFDLL32NOTFOUND");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TIFFDLL16NOTFOUND               , "DTWAIN_ERR_TIFFDLL16NOTFOUND");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PNGDLL16NOTFOUND                , "DTWAIN_ERR_PNGDLL16NOTFOUND");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_JPEGDLL16NOTFOUND               , "DTWAIN_ERR_JPEGDLL16NOTFOUND");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_BADBITSPERPIXEL                 , "DTWAIN_ERR_BADBITSPERPIXEL");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TIFFDLL32INVALIDVER             , "DTWAIN_ERR_TIFFDLL32INVALIDVER");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PDFDLL32NOTFOUND                , "DTWAIN_ERR_PDFDLL32NOTFOUND");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PDFDLL32INVALIDVER              , "DTWAIN_ERR_PDFDLL32INVALIDVER");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_JPEGDLL32NOTFOUND               , "DTWAIN_ERR_JPEGDLL32NOTFOUND");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_JPEGDLL32INVALIDVER             , "DTWAIN_ERR_JPEGDLL32INVALIDVER");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PNGDLL32NOTFOUND                , "DTWAIN_ERR_PNGDLL32NOTFOUND");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_PNGDLL32INVALIDVER              , "DTWAIN_ERR_PNGDLL32INVALIDVER");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_J2KDLL32NOTFOUND                , "DTWAIN_ERR_J2KDLL32NOTFOUND");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_J2KDLL32INVALIDVER              , "DTWAIN_ERR_J2KDLL32INVALIDVER");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_MANDUPLEX_UNAVAILABLE           , "DTWAIN_ERR_MANDUPLEX_UNAVAILABLE");                                
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TIMEOUT                         , "DTWAIN_ERR_TIMEOUT");                                              
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_INVALIDICONFORMAT               , "DTWAIN_ERR_INVALIDICONFORMAT");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TWAIN32DSMNOTFOUND              , "DTWAIN_ERR_TWAIN32DSMNOTFOUND");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND      , "DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND");                           
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_TWAINSAVE_OK                        , "DTWAIN_TWAINSAVE_OK");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_FIRST                        , "DTWAIN_ERR_TS_FIRST");                                             
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_NOFILENAME                   , "DTWAIN_ERR_TS_NOFILENAME");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_NOTWAINSYS                   , "DTWAIN_ERR_TS_NOTWAINSYS");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_DEVICEFAILURE                , "DTWAIN_ERR_TS_DEVICEFAILURE");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_FILESAVEERROR                , "DTWAIN_ERR_TS_FILESAVEERROR");                                     
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_COMMANDILLEGAL               , "DTWAIN_ERR_TS_COMMANDILLEGAL");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_CANCELLED                    , "DTWAIN_ERR_TS_CANCELLED");                                         
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_ACQUISITIONERROR             , "DTWAIN_ERR_TS_ACQUISITIONERROR");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_INVALIDCOLORSPACE            , "DTWAIN_ERR_TS_INVALIDCOLORSPACE");                                 
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_PDFNOTSUPPORTED              , "DTWAIN_ERR_TS_PDFNOTSUPPORTED");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_TS_NOTAVAILABLE                 , "DTWAIN_ERR_TS_NOTAVAILABLE");                                      
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_FIRST                       , "DTWAIN_ERR_OCR_FIRST");                                            
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_INVALIDPAGENUM              , "DTWAIN_ERR_OCR_INVALIDPAGENUM");                                   
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_INVALIDENGINE               , "DTWAIN_ERR_OCR_INVALIDENGINE");                                    
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_NOTACTIVE                   , "DTWAIN_ERR_OCR_NOTACTIVE");                                        
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_INVALIDFILETYPE             , "DTWAIN_ERR_OCR_INVALIDFILETYPE");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_INVALIDPIXELTYPE            , "DTWAIN_ERR_OCR_INVALIDPIXELTYPE");                                 
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_INVALIDBITDEPTH             , "DTWAIN_ERR_OCR_INVALIDBITDEPTH");                                  
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_RECOGNITIONERROR            , "DTWAIN_ERR_OCR_RECOGNITIONERROR");                                 
                allErrorCodes.put(DTwainJavaAPIConstants.DTWAIN_ERR_OCR_LAST                        , "DTWAIN_ERR_OCR_LAST");                                             
                getSystemResources();   
	}

	private static ResourceBundle getSystemResources() throws MissingResourceException {
		ResourceBundle bundle = null;
		try {
			bundle = ResourceBundle.getBundle("com/dynarithmic/twain/DTwainResources", Locale.getDefault());
			return bundle;
		}
		catch (MissingResourceException e ) {
			try {
				bundle = ResourceBundle.getBundle("com/dynarithmic/twain/DTwainResources", new Locale("en"));
				return bundle;
			}
			catch (MissingResourceException e1) {
				throw e;
			}
		}
	}

	public static void setResources( int supportedlocale )
	{
		try {
			theBundle = ResourceBundle.getBundle("DTwainResources",
					new Locale(getSupportedLocale(supportedlocale)));
		}
		catch ( MissingResourceException e ) {
			throw e;
		}
	}

	public static void setResources( String localeString )
	{
		try {
			theBundle = ResourceBundle.getBundle("DTwainResources",
					new Locale(localeString));
		}
		catch ( MissingResourceException e ) {
			throw e;
		}
	}
	
	public static void setResources( Locale locale )
	{
		try {
			theBundle = ResourceBundle.getBundle("DTwainResources", locale);
		}
		catch ( MissingResourceException e ) {
			throw e;
		}
	}

	public static void setResources(String bundleName,  Locale locale )
	{
		try {
			theBundle = ResourceBundle.getBundle(bundleName, locale);
		}
		catch ( MissingResourceException e ) {
			throw e;
		}
	}

	public static void setResources(String bundleName,  String locale )
	{
		try {
			theBundle = ResourceBundle.getBundle(bundleName, new Locale(locale));
		}
		catch ( MissingResourceException e ) {
			throw e;
		}
	}
        
	private static String getSupportedLocale(int locale)
	{
		String sLocale = (String)allLocales.get(new Integer(locale));
		if (sLocale == null )
			return "";
		return sLocale;
	}
	
	private static ResourceBundle theBundle = getSystemResources();
	
	
	public static ResourceBundle getResources() {
		
		return theBundle;
	}
	
	public static String parseString(String theString) {
		int nIndex = theString.indexOf('&');
		if ( nIndex == -1 )
			return theString;
		if ( nIndex == 0 && theString.length() > 1 )
			return theString.substring(1);
		
		String prefix = theString.substring(0, nIndex - 1);
		String suffix = "";
		if ( nIndex + 1 < theString.length() )
			suffix = theString.substring(nIndex + 1 );
		return prefix + suffix;
	}
	
	public static char parseMnemonic(String theString) {
		int nIndex = theString.indexOf('&');
		if ( nIndex == -1 )
			return 0;
		if ( nIndex == theString.length() -1 )
			return 0;
		return theString.charAt(nIndex+1);
	}
	
	public static String getErrorCodeAsString(int nCode) {
		String sCode = (String)allErrorCodes.get(new Integer(nCode));
		return sCode;
	}

}