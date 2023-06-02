package com.dynarithmic.twain;

import java.util.TreeMap;

/**
 * Contains the TWAIN Source capability constants.
 *
 */
public class DTwainCapabilityHandler
{
    private static final TreeMap<Integer, String> intToStringMap = new TreeMap<Integer, String>(); 
    private static final TreeMap<String, Integer> stringToIntMap = new TreeMap<String, Integer>();
    private static boolean isInitialized = false;
    private static boolean isInitialized2 = false;
    public static final int CAP_CUSTOMBASE = 0x8000;
    public static final int CAP_XFERCOUNT = 0x0001;
    public static final int ICAP_COMPRESSION = 0x0100;
    public static final int ICAP_PIXELTYPE = 0x0101;
    public static final int ICAP_UNITS = 0x0102;
    public static final int ICAP_XFERMECH = 0x0103;
    public static final int CAP_AUTHOR = 0x1000;
    public static final int CAP_CAPTION = 0x1001;
    public static final int CAP_FEEDERENABLED = 0x1002;
    public static final int CAP_FEEDERLOADED = 0x1003;
    public static final int CAP_TIMEDATE = 0x1004;
    public static final int CAP_SUPPORTEDCAPS = 0x1005;
    public static final int CAP_EXTENDEDCAPS = 0x1006;
    public static final int CAP_AUTOFEED = 0x1007;
    public static final int CAP_CLEARPAGE = 0x1008;
    public static final int CAP_FEEDPAGE = 0x1009;
    public static final int CAP_REWINDPAGE = 0x100a;
    public static final int CAP_INDICATORS = 0x100b;
    public static final int CAP_SUPPORTEDCAPSEXT = 0x100c;
    public static final int CAP_PAPERDETECTABLE = 0x100d;
    public static final int CAP_UICONTROLLABLE = 0x100e;
    public static final int CAP_DEVICEONLINE = 0x100f;
    public static final int CAP_AUTOSCAN = 0x1010;
    public static final int CAP_THUMBNAILSENABLED = 0x1011;
    public static final int CAP_DUPLEX = 0x1012;
    public static final int CAP_DUPLEXENABLED = 0x1013;
    public static final int CAP_ENABLEDSUIONLY = 0x1014;
    public static final int CAP_CUSTOMDSDATA = 0x1015;
    public static final int CAP_ENDORSER = 0x1016;
    public static final int CAP_JOBCONTROL = 0x1017;
    public static final int CAP_ALARMS = 0x1018;
    public static final int CAP_ALARMVOLUME = 0x1019;
    public static final int CAP_AUTOMATICCAPTURE = 0x101a;
    public static final int CAP_TIMEBEFOREFIRSTCAPTURE = 0x101b;
    public static final int CAP_TIMEBETWEENCAPTURES = 0x101c;
    public static final int CAP_CLEARBUFFERS = 0x101d;
    public static final int CAP_MAXBATCHBUFFERS = 0x101e;
    public static final int CAP_DEVICETIMEDATE = 0x101f;
    public static final int CAP_POWERSUPPLY = 0x1020;
    public static final int CAP_CAMERAPREVIEWUI = 0x1021;
    public static final int CAP_DEVICEEVENT = 0x1022;
    public static final int CAP_PAGEMULTIPLEACQUIRE = 0x1023;
    public static final int CAP_SERIALNUMBER = 0x1024;
    public static final int CAP_FILESYSTEM = 0x1025;
    public static final int CAP_PRINTER = 0x1026;
    public static final int CAP_PRINTERENABLED = 0x1027;
    public static final int CAP_PRINTERINDEX = 0x1028;
    public static final int CAP_PRINTERMODE = 0x1029;
    public static final int CAP_PRINTERSTRING = 0x102a;
    public static final int CAP_PRINTERSUFFIX = 0x102b;
    public static final int CAP_LANGUAGE = 0x102c;
    public static final int CAP_FEEDERALIGNMENT = 0x102d;
    public static final int CAP_FEEDERORDER = 0x102e;
    public static final int CAP_PAPERBINDING = 0x102f;
    public static final int CAP_REACQUIREALLOWED = 0x1030;
    public static final int CAP_PASSTHRU = 0x1031;
    public static final int CAP_BATTERYMINUTES = 0x1032;
    public static final int CAP_BATTERYPERCENTAGE = 0x1033;
    public static final int CAP_POWERDOWNTIME = 0x1034;
    public static final int CAP_SEGMENTED = 0x1035;
    public static final int CAP_CAMERAENABLED = 0x1036;
    public static final int CAP_CAMERAORDER = 0x1037;
    public static final int CAP_MICRENABLED = 0x1038;
    public static final int CAP_FEEDERPREP = 0x1039;
    public static final int CAP_FEEDERPOCKET = 0x103a;
    public static final int CAP_AUTOMATICSENSEMEDIUM = 0x103b;
    public static final int CAP_CUSTOMINTERFACEGUID = 0x103c;
    public static final int CAP_SUPPORTEDCAPSSEGMENTUNIQUE = 0x103d;
    public static final int CAP_SUPPORTEDDATS = 0x103e;
    public static final int CAP_DOUBLEFEEDDETECTION = 0x103f;
    public static final int CAP_DOUBLEFEEDDETECTIONLENGTH = 0x1040;
    public static final int CAP_DOUBLEFEEDDETECTIONSENSITIVITY = 0x1041;
    public static final int CAP_DOUBLEFEEDDETECTIONRESPONSE = 0x1042;
    public static final int CAP_PAPERHANDLING	= 0x1043;
    public static final int CAP_INDICATORSMODE	= 0x1044;
    public static final int CAP_PRINTERVERTICALOFFSET	= 0x1045;
    public static final int CAP_POWERSAVETIME	= 0x1046;
    public static final int CAP_PRINTERCHARROTATION = 0x1047;
    public static final int CAP_PRINTERFONTSTYLE = 0x1048;
    public static final int CAP_PRINTERINDEXLEADCHAR = 0x1049;
    public static final int CAP_PRINTERINDEXMAXVALUE = 0x104A;
    public static final int CAP_PRINTERINDEXNUMDIGITS = 0x104B;
    public static final int CAP_PRINTERINDEXSTEP = 0x104C;
    public static final int CAP_PRINTERINDEXTRIGGER = 0x104D;
    public static final int CAP_PRINTERSTRINGPREVIEW = 0x104E;
    public static final int ICAP_AUTOBRIGHT = 0x1100;
    public static final int ICAP_BRIGHTNESS = 0x1101;
    public static final int ICAP_CONTRAST = 0x1103;
    public static final int ICAP_CUSTHALFTONE = 0x1104;
    public static final int ICAP_EXPOSURETIME = 0x1105;
    public static final int ICAP_FILTER = 0x1106;
    public static final int ICAP_FLASHUSED = 0x1107;
    public static final int ICAP_GAMMA = 0x1108;
    public static final int ICAP_HALFTONES = 0x1109;
    public static final int ICAP_HIGHLIGHT = 0x110a;
    public static final int ICAP_IMAGEFILEFORMAT = 0x110c;
    public static final int ICAP_LAMPSTATE = 0x110d;
    public static final int ICAP_LIGHTSOURCE = 0x110e;
    public static final int ICAP_ORIENTATION = 0x1110;
    public static final int ICAP_PHYSICALWIDTH = 0x1111;
    public static final int ICAP_PHYSICALHEIGHT = 0x1112;
    public static final int ICAP_SHADOW = 0x1113;
    public static final int ICAP_FRAMES = 0x1114;
    public static final int ICAP_XNATIVERESOLUTION = 0x1116;
    public static final int ICAP_YNATIVERESOLUTION = 0x1117;
    public static final int ICAP_XRESOLUTION = 0x1118;
    public static final int ICAP_YRESOLUTION = 0x1119;
    public static final int ICAP_MAXFRAMES = 0x111a;
    public static final int ICAP_TILES = 0x111b;
    public static final int ICAP_BITORDER = 0x111c;
    public static final int ICAP_CCITTKFACTOR = 0x111d;
    public static final int ICAP_LIGHTPATH = 0x111e;
    public static final int ICAP_PIXELFLAVOR = 0x111f;
    public static final int ICAP_PLANARCHUNKY = 0x1120;
    public static final int ICAP_ROTATION = 0x1121;
    public static final int ICAP_SUPPORTEDSIZES = 0x1122;
    public static final int ICAP_THRESHOLD = 0x1123;
    public static final int ICAP_XSCALING = 0x1124;
    public static final int ICAP_YSCALING = 0x1125;
    public static final int ICAP_BITORDERCODES = 0x1126;
    public static final int ICAP_PIXELFLAVORCODES = 0x1127;
    public static final int ICAP_JPEGPIXELTYPE = 0x1128;
    public static final int ICAP_TIMEFILL = 0x112a;
    public static final int ICAP_BITDEPTH = 0x112b;
    public static final int ICAP_BITDEPTHREDUCTION = 0x112c;
    public static final int ICAP_UNDEFINEDIMAGESIZE = 0x112d;
    public static final int ICAP_IMAGEDATASET = 0x112e;
    public static final int ICAP_EXTIMAGEINFO = 0x112f;
    public static final int ICAP_MINIMUMHEIGHT = 0x1130;
    public static final int ICAP_MINIMUMWIDTH = 0x1131;
    public static final int ICAP_AUTOBORDERDETECTION = 0x1132;
    public static final int ICAP_AUTODESKEW = 0x1133;
    public static final int ICAP_AUTODISCARDBLANKPAGES = 0x1134;
    public static final int ICAP_AUTOROTATE = 0x1135;
    public static final int ICAP_FLIPROTATION = 0x1136;
    public static final int ICAP_BARCODEDETECTIONENABLED = 0x1137;
    public static final int ICAP_SUPPORTEDBARCODETYPES = 0x1138;
    public static final int ICAP_BARCODEMAXSEARCHPRIORITIES = 0x1139;
    public static final int ICAP_BARCODESEARCHPRIORITIES = 0x113a;
    public static final int ICAP_BARCODESEARCHMODE = 0x113b;
    public static final int ICAP_BARCODEMAXRETRIES = 0x113c;
    public static final int ICAP_BARCODETIMEOUT = 0x113d;
    public static final int ICAP_ZOOMFACTOR = 0x113e;
    public static final int ICAP_PATCHCODEDETECTIONENABLED = 0x113f;
    public static final int ICAP_SUPPORTEDPATCHCODETYPES = 0x1140;
    public static final int ICAP_PATCHCODEMAXSEARCHPRIORITIES = 0x1141;
    public static final int ICAP_PATCHCODESEARCHPRIORITIES = 0x1142;
    public static final int ICAP_PATCHCODESEARCHMODE = 0x1143;
    public static final int ICAP_PATCHCODEMAXRETRIES = 0x1144;
    public static final int ICAP_PATCHCODETIMEOUT = 0x1145;
    public static final int ICAP_FLASHUSED2 = 0x1146;
    public static final int ICAP_IMAGEFILTER = 0x1147;
    public static final int ICAP_NOISEFILTER = 0x1148;
    public static final int ICAP_OVERSCAN = 0x1149;
    public static final int ICAP_AUTOMATICBORDERDETECTION = 0x1150;
    public static final int ICAP_AUTOMATICDESKEW = 0x1151;
    public static final int ICAP_AUTOMATICROTATE = 0x1152;
    public static final int ICAP_JPEGQUALITY = 0x1153;
    public static final int ICAP_FEEDERTYPE = 0x1154;
    public static final int ICAP_ICCPROFILE = 0x1155;
    public static final int ICAP_AUTOSIZE = 0x1156;
    public static final int ICAP_AUTOMATICCROPUSESFRAME = 0x1157;
    public static final int ICAP_AUTOMATICLENGTHDETECTION = 0x1158;
    public static final int ICAP_AUTOMATICCOLORENABLED = 0x1159;
    public static final int ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE = 0x115a;
    public static final int ICAP_COLORMANAGEMENTENABLED = 0x115b;
    public static final int ICAP_IMAGEMERGE = 0x115c;
    public static final int ICAP_IMAGEMERGEHEIGHTTHRESHOLD = 0x115d;
    public static final int ICAP_SUPPORTEDEXTIMAGEINFO = 0x115e;
    public static final int ICAP_FILMTYPE = 0x115f;
    public static final int ICAP_MIRROR = 0x1160;
    public static final int ICAP_JPEGSUBSAMPLING = 0x1161;
    public static final int ACAP_AUDIOFILEFORMAT = 0x1201;
    public static final int ACAP_XFERMECH = 0x1202;
    public static final String CAPABILITY_INT = "int";
    public static final String CAPABILITY_DOUBLE = "double";
    public static final String CAPABILITY_FRAME = "DTwainFrame";
    public static final String CAPABILITY_STRING = "String";

    private static void initializeMap()
    {
        intToStringMap.put( CAP_CUSTOMBASE              ,"CAP_CUSTOMBASE");                                 
        intToStringMap.put( CAP_XFERCOUNT               ,"CAP_XFERCOUNT");                                  
        intToStringMap.put( ICAP_COMPRESSION            ,"ICAP_COMPRESSION");                               
        intToStringMap.put( ICAP_PIXELTYPE              ,"ICAP_PIXELTYPE");                                 
        intToStringMap.put( ICAP_UNITS                  ,"ICAP_UNITS");                                  
        intToStringMap.put( ICAP_XFERMECH               ,"ICAP_XFERMECH");                                  
        intToStringMap.put( CAP_AUTHOR                  ,"CAP_AUTHOR");                                  
        intToStringMap.put( CAP_CAPTION                 ,"CAP_CAPTION");                                    
        intToStringMap.put( CAP_FEEDERENABLED           ,"CAP_FEEDERENABLED");                              
        intToStringMap.put( CAP_FEEDERLOADED            ,"CAP_FEEDERLOADED");                               
        intToStringMap.put( CAP_TIMEDATE                ,"CAP_TIMEDATE");                                   
        intToStringMap.put( CAP_SUPPORTEDCAPS           ,"CAP_SUPPORTEDCAPS");                              
        intToStringMap.put( CAP_EXTENDEDCAPS            ,"CAP_EXTENDEDCAPS");                               
        intToStringMap.put( CAP_AUTOFEED                ,"CAP_AUTOFEED");                                   
        intToStringMap.put( CAP_CLEARPAGE               ,"CAP_CLEARPAGE");                                  
        intToStringMap.put( CAP_FEEDPAGE                ,"CAP_FEEDPAGE");                                   
        intToStringMap.put( CAP_REWINDPAGE              ,"CAP_REWINDPAGE");                                 
        intToStringMap.put( CAP_INDICATORS              ,"CAP_INDICATORS");                                 
        intToStringMap.put( CAP_SUPPORTEDCAPSEXT        ,"CAP_SUPPORTEDCAPSEXT");                           
        intToStringMap.put( CAP_PAPERDETECTABLE         ,"CAP_PAPERDETECTABLE");                            
        intToStringMap.put( CAP_UICONTROLLABLE          ,"CAP_UICONTROLLABLE");                             
        intToStringMap.put( CAP_DEVICEONLINE            ,"CAP_DEVICEONLINE");                               
        intToStringMap.put( CAP_AUTOSCAN                ,"CAP_AUTOSCAN");                                   
        intToStringMap.put( CAP_THUMBNAILSENABLED       ,"CAP_THUMBNAILSENABLED");                          
        intToStringMap.put( CAP_DUPLEX                  ,"CAP_DUPLEX");                                  
        intToStringMap.put( CAP_DUPLEXENABLED           ,"CAP_DUPLEXENABLED");                              
        intToStringMap.put( CAP_ENABLEDSUIONLY          ,"CAP_ENABLEDSUIONLY");                             
        intToStringMap.put( CAP_CUSTOMDSDATA            ,"CAP_CUSTOMDSDATA");                               
        intToStringMap.put( CAP_ENDORSER                ,"CAP_ENDORSER");                                   
        intToStringMap.put( CAP_JOBCONTROL              ,"CAP_JOBCONTROL");                                 
        intToStringMap.put( CAP_ALARMS                  ,"CAP_ALARMS");                                  
        intToStringMap.put( CAP_ALARMVOLUME             ,"CAP_ALARMVOLUME");                                
        intToStringMap.put( CAP_AUTOMATICCAPTURE        ,"CAP_AUTOMATICCAPTURE");                           
        intToStringMap.put( CAP_TIMEBEFOREFIRSTCAPTURE  ,"CAP_TIMEBEFOREFIRSTCAPTURE");                     
        intToStringMap.put( CAP_TIMEBETWEENCAPTURES     ,"CAP_TIMEBETWEENCAPTURES");                        
        intToStringMap.put( CAP_CLEARBUFFERS            ,"CAP_CLEARBUFFERS");                               
        intToStringMap.put( CAP_MAXBATCHBUFFERS         ,"CAP_MAXBATCHBUFFERS");                            
        intToStringMap.put( CAP_DEVICETIMEDATE          ,"CAP_DEVICETIMEDATE");                             
        intToStringMap.put( CAP_POWERSUPPLY             ,"CAP_POWERSUPPLY");                                
        intToStringMap.put( CAP_CAMERAPREVIEWUI         ,"CAP_CAMERAPREVIEWUI");                            
        intToStringMap.put( CAP_DEVICEEVENT             ,"CAP_DEVICEEVENT");                                
        intToStringMap.put( CAP_PAGEMULTIPLEACQUIRE     ,"CAP_PAGEMULTIPLEACQUIRE");                        
        intToStringMap.put( CAP_SERIALNUMBER            ,"CAP_SERIALNUMBER");                               
        intToStringMap.put( CAP_FILESYSTEM              ,"CAP_FILESYSTEM");                                 
        intToStringMap.put( CAP_PRINTER                 ,"CAP_PRINTER");                                    
        intToStringMap.put( CAP_PRINTERENABLED          ,"CAP_PRINTERENABLED");                             
        intToStringMap.put( CAP_PRINTERINDEX            ,"CAP_PRINTERINDEX");                               
        intToStringMap.put( CAP_PRINTERMODE             ,"CAP_PRINTERMODE");                                
        intToStringMap.put( CAP_PRINTERSTRING           ,"CAP_PRINTERSTRING");                              
        intToStringMap.put( CAP_PRINTERSUFFIX           ,"CAP_PRINTERSUFFIX");                              
        intToStringMap.put( CAP_LANGUAGE                ,"CAP_LANGUAGE");                                   
        intToStringMap.put( CAP_FEEDERALIGNMENT         ,"CAP_FEEDERALIGNMENT");                            
        intToStringMap.put( CAP_FEEDERORDER             ,"CAP_FEEDERORDER");                                
        intToStringMap.put( CAP_PAPERBINDING            ,"CAP_PAPERBINDING");                               
        intToStringMap.put( CAP_REACQUIREALLOWED        ,"CAP_REACQUIREALLOWED");                           
        intToStringMap.put( CAP_PASSTHRU                ,"CAP_PASSTHRU");                                   
        intToStringMap.put( CAP_BATTERYMINUTES          ,"CAP_BATTERYMINUTES");                             
        intToStringMap.put( CAP_BATTERYPERCENTAGE       ,"CAP_BATTERYPERCENTAGE");                          
        intToStringMap.put( CAP_POWERDOWNTIME           ,"CAP_POWERDOWNTIME");                              
        intToStringMap.put( CAP_SEGMENTED               ,"CAP_SEGMENTED");                                  
        intToStringMap.put( CAP_CAMERAENABLED           ,"CAP_CAMERAENABLED");                              
        intToStringMap.put( CAP_CAMERAORDER             ,"CAP_CAMERAORDER");                                
        intToStringMap.put( CAP_MICRENABLED             ,"CAP_MICRENABLED");                                
        intToStringMap.put( CAP_FEEDERPREP              ,"CAP_FEEDERPREP");                                 
        intToStringMap.put( CAP_FEEDERPOCKET            ,"CAP_FEEDERPOCKET");                               
        intToStringMap.put( CAP_AUTOMATICSENSEMEDIUM    ,"CAP_AUTOMATICSENSEMEDIUM");                       
        intToStringMap.put( CAP_CUSTOMINTERFACEGUID     ,"CAP_CUSTOMINTERFACEGUID");                        
        intToStringMap.put( CAP_SUPPORTEDCAPSSEGMENTUNIQUE,"CAP_SUPPORTEDCAPSSEGMENTUNIQUE");                 
        intToStringMap.put( CAP_SUPPORTEDDATS           ,"CAP_SUPPORTEDDATS");                              
        intToStringMap.put( CAP_DOUBLEFEEDDETECTION     ,"CAP_DOUBLEFEEDDETECTION");                        
        intToStringMap.put( CAP_DOUBLEFEEDDETECTIONLENGTH,"CAP_DOUBLEFEEDDETECTIONLENGTH");                  
        intToStringMap.put( CAP_DOUBLEFEEDDETECTIONSENSITIVITY,"CAP_DOUBLEFEEDDETECTIONSENSITIVITY");             
        intToStringMap.put( CAP_DOUBLEFEEDDETECTIONRESPONSE,"CAP_DOUBLEFEEDDETECTIONRESPONSE");                
        intToStringMap.put( CAP_PAPERHANDLING	     ,"CAP_PAPERHANDLING");                              
        intToStringMap.put( CAP_INDICATORSMODE	     ,"CAP_INDICATORSMODE");                             
        intToStringMap.put( CAP_PRINTERVERTICALOFFSET	,"CAP_PRINTERVERTICALOFFSET");                      
        intToStringMap.put( CAP_POWERSAVETIME	     ,"CAP_POWERSAVETIME");                              
        intToStringMap.put( CAP_PRINTERCHARROTATION     ,"CAP_PRINTERCHARROTATION");                        
        intToStringMap.put( CAP_PRINTERFONTSTYLE        ,"CAP_PRINTERFONTSTYLE");                           
        intToStringMap.put( CAP_PRINTERINDEXLEADCHAR    ,"CAP_PRINTERINDEXLEADCHAR");                       
        intToStringMap.put( CAP_PRINTERINDEXMAXVALUE    ,"CAP_PRINTERINDEXMAXVALUE");                       
        intToStringMap.put( CAP_PRINTERINDEXNUMDIGITS   ,"CAP_PRINTERINDEXNUMDIGITS");                      
        intToStringMap.put( CAP_PRINTERINDEXSTEP        ,"CAP_PRINTERINDEXSTEP");                           
        intToStringMap.put( CAP_PRINTERINDEXTRIGGER     ,"CAP_PRINTERINDEXTRIGGER");                        
        intToStringMap.put( CAP_PRINTERSTRINGPREVIEW    ,"CAP_PRINTERSTRINGPREVIEW");                       
        intToStringMap.put( ICAP_AUTOBRIGHT             ,"ICAP_AUTOBRIGHT");                                
        intToStringMap.put( ICAP_BRIGHTNESS             ,"ICAP_BRIGHTNESS");                                
        intToStringMap.put( ICAP_CONTRAST               ,"ICAP_CONTRAST");                                  
        intToStringMap.put( ICAP_CUSTHALFTONE           ,"ICAP_CUSTHALFTONE");                              
        intToStringMap.put( ICAP_EXPOSURETIME           ,"ICAP_EXPOSURETIME");                              
        intToStringMap.put( ICAP_FILTER                 ,"ICAP_FILTER");                                    
        intToStringMap.put( ICAP_FLASHUSED              ,"ICAP_FLASHUSED");                                 
        intToStringMap.put( ICAP_GAMMA                  ,"ICAP_GAMMA");                                  
        intToStringMap.put( ICAP_HALFTONES              ,"ICAP_HALFTONES");                                 
        intToStringMap.put( ICAP_HIGHLIGHT              ,"ICAP_HIGHLIGHT");                                 
        intToStringMap.put( ICAP_IMAGEFILEFORMAT        ,"ICAP_IMAGEFILEFORMAT");                           
        intToStringMap.put( ICAP_LAMPSTATE              ,"ICAP_LAMPSTATE");                                 
        intToStringMap.put( ICAP_LIGHTSOURCE            ,"ICAP_LIGHTSOURCE");                               
        intToStringMap.put( ICAP_ORIENTATION            ,"ICAP_ORIENTATION");                               
        intToStringMap.put( ICAP_PHYSICALWIDTH          ,"ICAP_PHYSICALWIDTH");                             
        intToStringMap.put( ICAP_PHYSICALHEIGHT         ,"ICAP_PHYSICALHEIGHT");                            
        intToStringMap.put( ICAP_SHADOW                 ,"ICAP_SHADOW");                                    
        intToStringMap.put( ICAP_FRAMES                 ,"ICAP_FRAMES");                                    
        intToStringMap.put( ICAP_XNATIVERESOLUTION      ,"ICAP_XNATIVERESOLUTION");                         
        intToStringMap.put( ICAP_YNATIVERESOLUTION      ,"ICAP_YNATIVERESOLUTION");                         
        intToStringMap.put( ICAP_XRESOLUTION            ,"ICAP_XRESOLUTION");                               
        intToStringMap.put( ICAP_YRESOLUTION            ,"ICAP_YRESOLUTION");                               
        intToStringMap.put( ICAP_MAXFRAMES              ,"ICAP_MAXFRAMES");                                 
        intToStringMap.put( ICAP_TILES                  ,"ICAP_TILES");                                  
        intToStringMap.put( ICAP_BITORDER               ,"ICAP_BITORDER");                                  
        intToStringMap.put( ICAP_CCITTKFACTOR           ,"ICAP_CCITTKFACTOR");                              
        intToStringMap.put( ICAP_LIGHTPATH              ,"ICAP_LIGHTPATH");                                 
        intToStringMap.put( ICAP_PIXELFLAVOR            ,"ICAP_PIXELFLAVOR");                               
        intToStringMap.put( ICAP_PLANARCHUNKY           ,"ICAP_PLANARCHUNKY");                              
        intToStringMap.put( ICAP_ROTATION               ,"ICAP_ROTATION");                                  
        intToStringMap.put( ICAP_SUPPORTEDSIZES         ,"ICAP_SUPPORTEDSIZES");                            
        intToStringMap.put( ICAP_THRESHOLD              ,"ICAP_THRESHOLD");                                 
        intToStringMap.put( ICAP_XSCALING               ,"ICAP_XSCALING");                                  
        intToStringMap.put( ICAP_YSCALING               ,"ICAP_YSCALING");                                  
        intToStringMap.put( ICAP_BITORDERCODES          ,"ICAP_BITORDERCODES");                             
        intToStringMap.put( ICAP_PIXELFLAVORCODES       ,"ICAP_PIXELFLAVORCODES");                          
        intToStringMap.put( ICAP_JPEGPIXELTYPE          ,"ICAP_JPEGPIXELTYPE");                             
        intToStringMap.put( ICAP_TIMEFILL               ,"ICAP_TIMEFILL");                                  
        intToStringMap.put( ICAP_BITDEPTH               ,"ICAP_BITDEPTH");                                  
        intToStringMap.put( ICAP_BITDEPTHREDUCTION      ,"ICAP_BITDEPTHREDUCTION");                         
        intToStringMap.put( ICAP_UNDEFINEDIMAGESIZE     ,"ICAP_UNDEFINEDIMAGESIZE");                        
        intToStringMap.put( ICAP_IMAGEDATASET           ,"ICAP_IMAGEDATASET");                              
        intToStringMap.put( ICAP_EXTIMAGEINFO           ,"ICAP_EXTIMAGEINFO");                              
        intToStringMap.put( ICAP_MINIMUMHEIGHT          ,"ICAP_MINIMUMHEIGHT");                             
        intToStringMap.put( ICAP_MINIMUMWIDTH           ,"ICAP_MINIMUMWIDTH");                              
        intToStringMap.put( ICAP_AUTOBORDERDETECTION    ,"ICAP_AUTOBORDERDETECTION");                       
        intToStringMap.put( ICAP_AUTODESKEW             ,"ICAP_AUTODESKEW");                                
        intToStringMap.put( ICAP_AUTODISCARDBLANKPAGES  ,"ICAP_AUTODISCARDBLANKPAGES");                     
        intToStringMap.put( ICAP_AUTOROTATE             ,"ICAP_AUTOROTATE");                                
        intToStringMap.put( ICAP_FLIPROTATION           ,"ICAP_FLIPROTATION");                              
        intToStringMap.put( ICAP_BARCODEDETECTIONENABLED,"ICAP_BARCODEDETECTIONENABLED");                   
        intToStringMap.put( ICAP_SUPPORTEDBARCODETYPES  ,"ICAP_SUPPORTEDBARCODETYPES");                     
        intToStringMap.put( ICAP_BARCODEMAXSEARCHPRIORITIES,"ICAP_BARCODEMAXSEARCHPRIORITIES");                
        intToStringMap.put( ICAP_BARCODESEARCHPRIORITIES,"ICAP_BARCODESEARCHPRIORITIES");                   
        intToStringMap.put( ICAP_BARCODESEARCHMODE      ,"ICAP_BARCODESEARCHMODE");                         
        intToStringMap.put( ICAP_BARCODEMAXRETRIES      ,"ICAP_BARCODEMAXRETRIES");                         
        intToStringMap.put( ICAP_BARCODETIMEOUT         ,"ICAP_BARCODETIMEOUT");                            
        intToStringMap.put( ICAP_ZOOMFACTOR             ,"ICAP_ZOOMFACTOR");                                
        intToStringMap.put( ICAP_PATCHCODEDETECTIONENABLED,"ICAP_PATCHCODEDETECTIONENABLED");                 
        intToStringMap.put( ICAP_SUPPORTEDPATCHCODETYPES,"ICAP_SUPPORTEDPATCHCODETYPES");                   
        intToStringMap.put( ICAP_PATCHCODEMAXSEARCHPRIORITIES,"ICAP_PATCHCODEMAXSEARCHPRIORITIES");              
        intToStringMap.put( ICAP_PATCHCODESEARCHPRIORITIES,"ICAP_PATCHCODESEARCHPRIORITIES");                 
        intToStringMap.put( ICAP_PATCHCODESEARCHMODE    ,"ICAP_PATCHCODESEARCHMODE");                       
        intToStringMap.put( ICAP_PATCHCODEMAXRETRIES    ,"ICAP_PATCHCODEMAXRETRIES");                       
        intToStringMap.put( ICAP_PATCHCODETIMEOUT       ,"ICAP_PATCHCODETIMEOUT");                          
        intToStringMap.put( ICAP_FLASHUSED2             ,"ICAP_FLASHUSED2");                                
        intToStringMap.put( ICAP_IMAGEFILTER            ,"ICAP_IMAGEFILTER");                               
        intToStringMap.put( ICAP_NOISEFILTER            ,"ICAP_NOISEFILTER");                               
        intToStringMap.put( ICAP_OVERSCAN               ,"ICAP_OVERSCAN");                                  
        intToStringMap.put( ICAP_AUTOMATICBORDERDETECTION,"ICAP_AUTOMATICBORDERDETECTION");                  
        intToStringMap.put( ICAP_AUTOMATICDESKEW        ,"ICAP_AUTOMATICDESKEW");                           
        intToStringMap.put( ICAP_AUTOMATICROTATE        ,"ICAP_AUTOMATICROTATE");                           
        intToStringMap.put( ICAP_JPEGQUALITY            ,"ICAP_JPEGQUALITY");                               
        intToStringMap.put( ICAP_FEEDERTYPE             ,"ICAP_FEEDERTYPE");                                
        intToStringMap.put( ICAP_ICCPROFILE             ,"ICAP_ICCPROFILE");                                
        intToStringMap.put( ICAP_AUTOSIZE               ,"ICAP_AUTOSIZE");                                  
        intToStringMap.put( ICAP_AUTOMATICCROPUSESFRAME ,"ICAP_AUTOMATICCROPUSESFRAME");                    
        intToStringMap.put( ICAP_AUTOMATICLENGTHDETECTION,"ICAP_AUTOMATICLENGTHDETECTION");                  
        intToStringMap.put( ICAP_AUTOMATICCOLORENABLED  ,"ICAP_AUTOMATICCOLORENABLED");                     
        intToStringMap.put( ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE,"ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE");           
        intToStringMap.put( ICAP_COLORMANAGEMENTENABLED ,"ICAP_COLORMANAGEMENTENABLED");                    
        intToStringMap.put( ICAP_IMAGEMERGE             ,"ICAP_IMAGEMERGE");                                
        intToStringMap.put( ICAP_IMAGEMERGEHEIGHTTHRESHOLD,"ICAP_IMAGEMERGEHEIGHTTHRESHOLD");                 
        intToStringMap.put( ICAP_SUPPORTEDEXTIMAGEINFO  ,"ICAP_SUPPORTEDEXTIMAGEINFO");                     
        intToStringMap.put( ICAP_FILMTYPE               ,"ICAP_FILMTYPE");                                  
        intToStringMap.put( ICAP_MIRROR                 ,"ICAP_MIRROR");                                    
        intToStringMap.put( ICAP_JPEGSUBSAMPLING        ,"ICAP_JPEGSUBSAMPLING");                           
        intToStringMap.put( ACAP_AUDIOFILEFORMAT        ,"ACAP_AUDIOFILEFORMAT");                           
        intToStringMap.put( ACAP_XFERMECH               ,"ACAP_XFERMECH");                                  
        isInitialized= true;
    }

    private static void initializeMap2()
    {
        stringToIntMap.put("CAP_CUSTOMBASE",CAP_CUSTOMBASE);                                 
        stringToIntMap.put("CAP_XFERCOUNT",CAP_XFERCOUNT);                                  
        stringToIntMap.put("ICAP_COMPRESSION",ICAP_COMPRESSION);                               
        stringToIntMap.put("ICAP_PIXELTYPE",ICAP_PIXELTYPE);                                 
        stringToIntMap.put("ICAP_UNITS",ICAP_UNITS);                                  
        stringToIntMap.put("ICAP_XFERMECH",ICAP_XFERMECH);                                  
        stringToIntMap.put("CAP_AUTHOR",CAP_AUTHOR);                                  
        stringToIntMap.put("CAP_CAPTION",CAP_CAPTION);                                    
        stringToIntMap.put("CAP_FEEDERENABLED",CAP_FEEDERENABLED);                              
        stringToIntMap.put("CAP_FEEDERLOADED",CAP_FEEDERLOADED);                               
        stringToIntMap.put("CAP_TIMEDATE",CAP_TIMEDATE);                                   
        stringToIntMap.put("CAP_SUPPORTEDCAPS",CAP_SUPPORTEDCAPS);                              
        stringToIntMap.put("CAP_EXTENDEDCAPS",CAP_EXTENDEDCAPS);                               
        stringToIntMap.put("CAP_AUTOFEED",CAP_AUTOFEED);                                   
        stringToIntMap.put("CAP_CLEARPAGE",CAP_CLEARPAGE);                                  
        stringToIntMap.put("CAP_FEEDPAGE",CAP_FEEDPAGE);                                   
        stringToIntMap.put("CAP_REWINDPAGE",CAP_REWINDPAGE);                                 
        stringToIntMap.put("CAP_INDICATORS",CAP_INDICATORS);                                 
        stringToIntMap.put("CAP_SUPPORTEDCAPSEXT",CAP_SUPPORTEDCAPSEXT);                           
        stringToIntMap.put("CAP_PAPERDETECTABLE",CAP_PAPERDETECTABLE);                            
        stringToIntMap.put("CAP_UICONTROLLABLE",CAP_UICONTROLLABLE);                             
        stringToIntMap.put("CAP_DEVICEONLINE",CAP_DEVICEONLINE);                               
        stringToIntMap.put("CAP_AUTOSCAN",CAP_AUTOSCAN);                                   
        stringToIntMap.put("CAP_THUMBNAILSENABLED",CAP_THUMBNAILSENABLED);                          
        stringToIntMap.put("CAP_DUPLEX",CAP_DUPLEX);                                  
        stringToIntMap.put("CAP_DUPLEXENABLED",CAP_DUPLEXENABLED);                              
        stringToIntMap.put("CAP_ENABLEDSUIONLY",CAP_ENABLEDSUIONLY);                             
        stringToIntMap.put("CAP_CUSTOMDSDATA",CAP_CUSTOMDSDATA);                               
        stringToIntMap.put("CAP_ENDORSER",CAP_ENDORSER);                                   
        stringToIntMap.put("CAP_JOBCONTROL",CAP_JOBCONTROL);                                 
        stringToIntMap.put("CAP_ALARMS",CAP_ALARMS);                                  
        stringToIntMap.put("CAP_ALARMVOLUME",CAP_ALARMVOLUME);                                
        stringToIntMap.put("CAP_AUTOMATICCAPTURE",CAP_AUTOMATICCAPTURE);                           
        stringToIntMap.put("CAP_TIMEBEFOREFIRSTCAPTURE",CAP_TIMEBEFOREFIRSTCAPTURE);                     
        stringToIntMap.put("CAP_TIMEBETWEENCAPTURES",CAP_TIMEBETWEENCAPTURES);                        
        stringToIntMap.put("CAP_CLEARBUFFERS",CAP_CLEARBUFFERS);                               
        stringToIntMap.put("CAP_MAXBATCHBUFFERS",CAP_MAXBATCHBUFFERS);                            
        stringToIntMap.put("CAP_DEVICETIMEDATE",CAP_DEVICETIMEDATE);                             
        stringToIntMap.put("CAP_POWERSUPPLY",CAP_POWERSUPPLY);                                
        stringToIntMap.put("CAP_CAMERAPREVIEWUI",CAP_CAMERAPREVIEWUI);                            
        stringToIntMap.put("CAP_DEVICEEVENT",CAP_DEVICEEVENT);                                
        stringToIntMap.put("CAP_PAGEMULTIPLEACQUIRE",CAP_PAGEMULTIPLEACQUIRE);                        
        stringToIntMap.put("CAP_SERIALNUMBER",CAP_SERIALNUMBER);                               
        stringToIntMap.put("CAP_FILESYSTEM",CAP_FILESYSTEM);                                 
        stringToIntMap.put("CAP_PRINTER",CAP_PRINTER);                                    
        stringToIntMap.put("CAP_PRINTERENABLED",CAP_PRINTERENABLED);                             
        stringToIntMap.put("CAP_PRINTERINDEX",CAP_PRINTERINDEX);                               
        stringToIntMap.put("CAP_PRINTERMODE",CAP_PRINTERMODE);                                
        stringToIntMap.put("CAP_PRINTERSTRING",CAP_PRINTERSTRING);                              
        stringToIntMap.put("CAP_PRINTERSUFFIX",CAP_PRINTERSUFFIX);                              
        stringToIntMap.put("CAP_LANGUAGE",CAP_LANGUAGE);                                   
        stringToIntMap.put("CAP_FEEDERALIGNMENT",CAP_FEEDERALIGNMENT);                            
        stringToIntMap.put("CAP_FEEDERORDER",CAP_FEEDERORDER);                                
        stringToIntMap.put("CAP_PAPERBINDING",CAP_PAPERBINDING);                               
        stringToIntMap.put("CAP_REACQUIREALLOWED",CAP_REACQUIREALLOWED);                           
        stringToIntMap.put("CAP_PASSTHRU",CAP_PASSTHRU);                                   
        stringToIntMap.put("CAP_BATTERYMINUTES",CAP_BATTERYMINUTES);                             
        stringToIntMap.put("CAP_BATTERYPERCENTAGE",CAP_BATTERYPERCENTAGE);                          
        stringToIntMap.put("CAP_POWERDOWNTIME",CAP_POWERDOWNTIME);                              
        stringToIntMap.put("CAP_SEGMENTED",CAP_SEGMENTED);                                  
        stringToIntMap.put("CAP_CAMERAENABLED",CAP_CAMERAENABLED);                              
        stringToIntMap.put("CAP_CAMERAORDER",CAP_CAMERAORDER);                                
        stringToIntMap.put("CAP_MICRENABLED",CAP_MICRENABLED);                                
        stringToIntMap.put("CAP_FEEDERPREP",CAP_FEEDERPREP);                                 
        stringToIntMap.put("CAP_FEEDERPOCKET",CAP_FEEDERPOCKET);                               
        stringToIntMap.put("CAP_AUTOMATICSENSEMEDIUM",CAP_AUTOMATICSENSEMEDIUM);                       
        stringToIntMap.put("CAP_CUSTOMINTERFACEGUID",CAP_CUSTOMINTERFACEGUID);                        
        stringToIntMap.put("CAP_SUPPORTEDCAPSSEGMENTUNIQUE",CAP_SUPPORTEDCAPSSEGMENTUNIQUE);                 
        stringToIntMap.put("CAP_SUPPORTEDDATS",CAP_SUPPORTEDDATS);                              
        stringToIntMap.put("CAP_DOUBLEFEEDDETECTION",CAP_DOUBLEFEEDDETECTION);                        
        stringToIntMap.put("CAP_DOUBLEFEEDDETECTIONLENGTH",CAP_DOUBLEFEEDDETECTIONLENGTH);                  
        stringToIntMap.put("CAP_DOUBLEFEEDDETECTIONSENSITIVITY",CAP_DOUBLEFEEDDETECTIONSENSITIVITY);             
        stringToIntMap.put("CAP_DOUBLEFEEDDETECTIONRESPONSE",CAP_DOUBLEFEEDDETECTIONRESPONSE);                
        stringToIntMap.put("CAP_PAPERHANDLING	",CAP_PAPERHANDLING);                              
        stringToIntMap.put("CAP_INDICATORSMODE	",CAP_INDICATORSMODE);                             
        stringToIntMap.put("CAP_PRINTERVERTICALOFFSET	",CAP_PRINTERVERTICALOFFSET);                      
        stringToIntMap.put("CAP_POWERSAVETIME	",CAP_POWERSAVETIME);                              
        stringToIntMap.put("CAP_PRINTERCHARROTATION",CAP_PRINTERCHARROTATION);                        
        stringToIntMap.put("CAP_PRINTERFONTSTYLE",CAP_PRINTERFONTSTYLE);                           
        stringToIntMap.put("CAP_PRINTERINDEXLEADCHAR",CAP_PRINTERINDEXLEADCHAR);                       
        stringToIntMap.put("CAP_PRINTERINDEXMAXVALUE",CAP_PRINTERINDEXMAXVALUE);                       
        stringToIntMap.put("CAP_PRINTERINDEXNUMDIGITS",CAP_PRINTERINDEXNUMDIGITS);                      
        stringToIntMap.put("CAP_PRINTERINDEXSTEP",CAP_PRINTERINDEXSTEP);                           
        stringToIntMap.put("CAP_PRINTERINDEXTRIGGER",CAP_PRINTERINDEXTRIGGER);                        
        stringToIntMap.put("CAP_PRINTERSTRINGPREVIEW",CAP_PRINTERSTRINGPREVIEW);                       
        stringToIntMap.put("ICAP_AUTOBRIGHT",ICAP_AUTOBRIGHT);                                
        stringToIntMap.put("ICAP_BRIGHTNESS",ICAP_BRIGHTNESS);                                
        stringToIntMap.put("ICAP_CONTRAST",ICAP_CONTRAST);                                  
        stringToIntMap.put("ICAP_CUSTHALFTONE",ICAP_CUSTHALFTONE);                              
        stringToIntMap.put("ICAP_EXPOSURETIME",ICAP_EXPOSURETIME);                              
        stringToIntMap.put("ICAP_FILTER",ICAP_FILTER);                                    
        stringToIntMap.put("ICAP_FLASHUSED",ICAP_FLASHUSED);                                 
        stringToIntMap.put("ICAP_GAMMA",ICAP_GAMMA);                                  
        stringToIntMap.put("ICAP_HALFTONES",ICAP_HALFTONES);                                 
        stringToIntMap.put("ICAP_HIGHLIGHT",ICAP_HIGHLIGHT);                                 
        stringToIntMap.put("ICAP_IMAGEFILEFORMAT",ICAP_IMAGEFILEFORMAT);                           
        stringToIntMap.put("ICAP_LAMPSTATE",ICAP_LAMPSTATE);                                 
        stringToIntMap.put("ICAP_LIGHTSOURCE",ICAP_LIGHTSOURCE);                               
        stringToIntMap.put("ICAP_ORIENTATION",ICAP_ORIENTATION);                               
        stringToIntMap.put("ICAP_PHYSICALWIDTH",ICAP_PHYSICALWIDTH);                             
        stringToIntMap.put("ICAP_PHYSICALHEIGHT",ICAP_PHYSICALHEIGHT);                            
        stringToIntMap.put("ICAP_SHADOW",ICAP_SHADOW);                                    
        stringToIntMap.put("ICAP_FRAMES",ICAP_FRAMES);                                    
        stringToIntMap.put("ICAP_XNATIVERESOLUTION",ICAP_XNATIVERESOLUTION);                         
        stringToIntMap.put("ICAP_YNATIVERESOLUTION",ICAP_YNATIVERESOLUTION);                         
        stringToIntMap.put("ICAP_XRESOLUTION",ICAP_XRESOLUTION);                               
        stringToIntMap.put("ICAP_YRESOLUTION",ICAP_YRESOLUTION);                               
        stringToIntMap.put("ICAP_MAXFRAMES",ICAP_MAXFRAMES);                                 
        stringToIntMap.put("ICAP_TILES",ICAP_TILES);                                  
        stringToIntMap.put("ICAP_BITORDER",ICAP_BITORDER);                                  
        stringToIntMap.put("ICAP_CCITTKFACTOR",ICAP_CCITTKFACTOR);                              
        stringToIntMap.put("ICAP_LIGHTPATH",ICAP_LIGHTPATH);                                 
        stringToIntMap.put("ICAP_PIXELFLAVOR",ICAP_PIXELFLAVOR);                               
        stringToIntMap.put("ICAP_PLANARCHUNKY",ICAP_PLANARCHUNKY);                              
        stringToIntMap.put("ICAP_ROTATION",ICAP_ROTATION);                                  
        stringToIntMap.put("ICAP_SUPPORTEDSIZES",ICAP_SUPPORTEDSIZES);                            
        stringToIntMap.put("ICAP_THRESHOLD",ICAP_THRESHOLD);                                 
        stringToIntMap.put("ICAP_XSCALING",ICAP_XSCALING);                                  
        stringToIntMap.put("ICAP_YSCALING",ICAP_YSCALING);                                  
        stringToIntMap.put("ICAP_BITORDERCODES",ICAP_BITORDERCODES);                             
        stringToIntMap.put("ICAP_PIXELFLAVORCODES",ICAP_PIXELFLAVORCODES);                          
        stringToIntMap.put("ICAP_JPEGPIXELTYPE",ICAP_JPEGPIXELTYPE);                             
        stringToIntMap.put("ICAP_TIMEFILL",ICAP_TIMEFILL);                                  
        stringToIntMap.put("ICAP_BITDEPTH",ICAP_BITDEPTH);                                  
        stringToIntMap.put("ICAP_BITDEPTHREDUCTION",ICAP_BITDEPTHREDUCTION);                         
        stringToIntMap.put("ICAP_UNDEFINEDIMAGESIZE",ICAP_UNDEFINEDIMAGESIZE);                        
        stringToIntMap.put("ICAP_IMAGEDATASET",ICAP_IMAGEDATASET);                              
        stringToIntMap.put("ICAP_EXTIMAGEINFO",ICAP_EXTIMAGEINFO);                              
        stringToIntMap.put("ICAP_MINIMUMHEIGHT",ICAP_MINIMUMHEIGHT);                             
        stringToIntMap.put("ICAP_MINIMUMWIDTH",ICAP_MINIMUMWIDTH);                              
        stringToIntMap.put("ICAP_AUTOBORDERDETECTION",ICAP_AUTOBORDERDETECTION);                       
        stringToIntMap.put("ICAP_AUTODESKEW",ICAP_AUTODESKEW);                                
        stringToIntMap.put("ICAP_AUTODISCARDBLANKPAGES",ICAP_AUTODISCARDBLANKPAGES);                     
        stringToIntMap.put("ICAP_AUTOROTATE",ICAP_AUTOROTATE);                                
        stringToIntMap.put("ICAP_FLIPROTATION",ICAP_FLIPROTATION);                              
        stringToIntMap.put("ICAP_BARCODEDETECTIONENABLED",ICAP_BARCODEDETECTIONENABLED);                   
        stringToIntMap.put("ICAP_SUPPORTEDBARCODETYPES",ICAP_SUPPORTEDBARCODETYPES);                     
        stringToIntMap.put("ICAP_BARCODEMAXSEARCHPRIORITIES",ICAP_BARCODEMAXSEARCHPRIORITIES);                
        stringToIntMap.put("ICAP_BARCODESEARCHPRIORITIES",ICAP_BARCODESEARCHPRIORITIES);                   
        stringToIntMap.put("ICAP_BARCODESEARCHMODE",ICAP_BARCODESEARCHMODE);                         
        stringToIntMap.put("ICAP_BARCODEMAXRETRIES",ICAP_BARCODEMAXRETRIES);                         
        stringToIntMap.put("ICAP_BARCODETIMEOUT",ICAP_BARCODETIMEOUT);                            
        stringToIntMap.put("ICAP_ZOOMFACTOR",ICAP_ZOOMFACTOR);                                
        stringToIntMap.put("ICAP_PATCHCODEDETECTIONENABLED",ICAP_PATCHCODEDETECTIONENABLED);                 
        stringToIntMap.put("ICAP_SUPPORTEDPATCHCODETYPES",ICAP_SUPPORTEDPATCHCODETYPES);                   
        stringToIntMap.put("ICAP_PATCHCODEMAXSEARCHPRIORITIES",ICAP_PATCHCODEMAXSEARCHPRIORITIES);              
        stringToIntMap.put("ICAP_PATCHCODESEARCHPRIORITIES",ICAP_PATCHCODESEARCHPRIORITIES);                 
        stringToIntMap.put("ICAP_PATCHCODESEARCHMODE",ICAP_PATCHCODESEARCHMODE);                       
        stringToIntMap.put("ICAP_PATCHCODEMAXRETRIES",ICAP_PATCHCODEMAXRETRIES);                       
        stringToIntMap.put("ICAP_PATCHCODETIMEOUT",ICAP_PATCHCODETIMEOUT);                          
        stringToIntMap.put("ICAP_FLASHUSED2",ICAP_FLASHUSED2);                                
        stringToIntMap.put("ICAP_IMAGEFILTER",ICAP_IMAGEFILTER);                               
        stringToIntMap.put("ICAP_NOISEFILTER",ICAP_NOISEFILTER);                               
        stringToIntMap.put("ICAP_OVERSCAN",ICAP_OVERSCAN);                                  
        stringToIntMap.put("ICAP_AUTOMATICBORDERDETECTION",ICAP_AUTOMATICBORDERDETECTION);                  
        stringToIntMap.put("ICAP_AUTOMATICDESKEW",ICAP_AUTOMATICDESKEW);                           
        stringToIntMap.put("ICAP_AUTOMATICROTATE",ICAP_AUTOMATICROTATE);                           
        stringToIntMap.put("ICAP_JPEGQUALITY",ICAP_JPEGQUALITY);                               
        stringToIntMap.put("ICAP_FEEDERTYPE",ICAP_FEEDERTYPE);                                
        stringToIntMap.put("ICAP_ICCPROFILE",ICAP_ICCPROFILE);                                
        stringToIntMap.put("ICAP_AUTOSIZE",ICAP_AUTOSIZE);                                  
        stringToIntMap.put("ICAP_AUTOMATICCROPUSESFRAME",ICAP_AUTOMATICCROPUSESFRAME);                    
        stringToIntMap.put("ICAP_AUTOMATICLENGTHDETECTION",ICAP_AUTOMATICLENGTHDETECTION);                  
        stringToIntMap.put("ICAP_AUTOMATICCOLORENABLED",ICAP_AUTOMATICCOLORENABLED);                     
        stringToIntMap.put("ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE",ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE);           
        stringToIntMap.put("ICAP_COLORMANAGEMENTENABLED",ICAP_COLORMANAGEMENTENABLED);                    
        stringToIntMap.put("ICAP_IMAGEMERGE",ICAP_IMAGEMERGE);                                
        stringToIntMap.put("ICAP_IMAGEMERGEHEIGHTTHRESHOLD",ICAP_IMAGEMERGEHEIGHTTHRESHOLD);                 
        stringToIntMap.put("ICAP_SUPPORTEDEXTIMAGEINFO",ICAP_SUPPORTEDEXTIMAGEINFO);                     
        stringToIntMap.put("ICAP_FILMTYPE",ICAP_FILMTYPE);                                  
        stringToIntMap.put("ICAP_MIRROR",ICAP_MIRROR);                                    
        stringToIntMap.put("ICAP_JPEGSUBSAMPLING",ICAP_JPEGSUBSAMPLING);                           
        stringToIntMap.put("ACAP_AUDIOFILEFORMAT",ACAP_AUDIOFILEFORMAT);                           
        stringToIntMap.put("ACAP_XFERMECH",ACAP_XFERMECH);                                  
        isInitialized2= true;

    }
    
    /**
     * Get the string name of the capability.  If the capability is a custom capability, the
     * returned string is "CAP_CUSTOMBASE + xxxx", where xxxx is an integer constant, and CAP_CUSTOMBASE is
     * equal to 0x8000.
     * @param nCapName
     * Capability value.
     * @return
     * String name of capability.
     */
    public static String toString(int nCapName)
    {
        if ( !isInitialized )
        {
            initializeMap();
        }
        if ( nCapName < CAP_CUSTOMBASE )
        {
           return (String)intToStringMap.get(nCapName);
        }
        else
        {
            String sCapName= (String)intToStringMap.get(new Integer(CAP_CUSTOMBASE));
            sCapName += " + ";
            sCapName= sCapName + (nCapName - CAP_CUSTOMBASE);
            return sCapName;
        }
    }

    /**
     * Get the integer value of the string capability name.  If the capability
     * is a custom capability, <i>sCapName</i> must be of the form<p>
     * "CAP_CUSTOMBASE + xxxx", where xxxx is some integer (base 10) value. 
     * @param sCapName
     * The capability string name.
     * @return
     * The integer value of the capability.
     */
    public static int intValue(String sCapName)
    {
      if ( !isInitialized2 )
      {
          initializeMap2();
      }
      
      // trim the whitespace from the name
      sCapName= sCapName.trim();
      
      // convert to upper case
      sCapName= sCapName.toUpperCase();

      // check if it's a custom cap
      if ( sCapName.length() >= 14 )
      {
          if ( sCapName.substring(0, 14).equals("CAP_CUSTOMBASE"))
          {
              // find the '+' character
              int nIndex= sCapName.indexOf('+');
              if ( nIndex == -1 )
              {
              } 
              else 
              {
                  String sSuffix= sCapName.substring(nIndex+1);
                  sSuffix= sSuffix.trim();
                  return CAP_CUSTOMBASE + (new Integer(sSuffix));
              }
              return 0;
          }
      }
      return stringToIntMap.get(sCapName);
    }
}