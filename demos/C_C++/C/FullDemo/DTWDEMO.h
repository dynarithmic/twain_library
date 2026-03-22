#ifndef DTWDEMO_H
#define DTWDEMO_H

#include ".\dibdisplayres.h" /* resources 4000 - 4999 reserved here */

#define IDC_MYICON                      2
#define IDD_EXAMPLE1_DIALOG             102
#define IDD_ABOUTBOX                    103
#define IDS_APP_TITLE                   103
#define IDM_ABOUT                       104
#define IDM_EXIT                        105
#define IDS_HELLO                       106
#define IDI_DTWDEMO                     107
#define IDI_SMALL                       108
#define IDC_DTWDEMO                     109
#define IDR_MAINFRAME                   128
#define IDM_RETRIEVE_DIB                32773
#define IDC_STATIC                      -1

#define IDM_SELECT_SOURCE                   200
#define IDM_SELECT_SOURCE_BY_NAME           201
#define IDM_SELECT_DEFAULT_SOURCE           202
#define IDM_SOURCE_PROPS                    203
#define IDM_SELECT_SOURCE_CUSTOM            204
#define IDM_CLOSE_SOURCE                    210

#define IDM_ACQUIRE_NATIVE                  301
#define IDM_ACQUIRE_BUFFERED                302
#define IDM_ACQUIRE_FILE_DTWAIN             303
#define IDM_ACQUIRE_FILE_SOURCE             304
#define IDM_ACQUIRE_CLIPBOARD               305
#define IDM_ACQUIRE_OPTIONS                 306
#define IDM_USE_SOURCE_UI                   307
#define IDM_DISCARD_BLANKS                  308
#define IDM_SHOW_PREVIEW                    309
#define IDM_SHOW_BARCODEINFO                310
#define IDM_ACQUIRETEST_USEGETMESSAGE       311

#define IDM_ACQUIREFILESOURCE_WINDOWSBMP    400
#define IDM_ACQUIREFILESOURCE_JPEG          401
#define IDM_ACQUIREFILESOURCE_TIFF          402
#define IDM_ACQUIREFILESOURCE_TIFFMULTIPAGE 403
#define IDM_ACQUIREFILESOURCE_PNG           404
#define IDM_ACQUIREFILESOURCE_PDF           405
#define IDM_ACQUIREFILESOURCE_PDFA          406
#define IDM_ACQUIREFILESOURCE_PDFA2         407
#define IDM_ACQUIREFILESOURCE_PDFRASTER     408
#define IDM_ACQUIREFILESOURCE_FLASHPIX      409
#define IDM_ACQUIREFILESOURCE_EXIF          410
#define IDM_ACQUIREFILESOURCE_SPIFF         411
#define IDM_ACQUIREFILESOURCE_XBM           412
#define IDM_ACQUIREFILESOURCE_PICT          413
#define IDM_ACQUIREFILESOURCE_JP2           414
#define IDM_ACQUIREFILESOURCE_JPX           415
#define IDM_ACQUIREFILESOURCE_DEJAVU        416
#define IDM_ACQUIREFILE_BIGTIFF_NOCOMPRESSION  417
#define IDM_ACQUIREFILE_BIGTIFF_GROUP3         418
#define IDM_ACQUIREFILE_BIGTIFF_GROUP4         419 
#define IDM_ACQUIREFILE_BIGTIFF_FLATE          420
#define IDM_ACQUIREFILE_BIGTIFF_JPEG           421
#define IDM_ACQUIREFILE_BIGTIFF_LZW            422 
#define IDM_ACQUIREFILE_BIGTIFF_PACKBITS       423
#define IDM_ACQUIREFILE_BMP                 424
#define IDM_ACQUIREFILE_BMPRLE              425
#define IDM_ACQUIREFILE_DCX                 426
#define IDM_ACQUIREFILE_ENHANCEDMETAFILE    427
#define IDM_ACQUIREFILE_GIF                 428
#define IDM_ACQUIREFILE_ICO                 429
#define IDM_ACQUIREFILE_ICOVISTA            430
#define IDM_ACQUIREFILE_JPEG                431
#define IDM_ACQUIREFILE_JPEG2000            432
#define IDM_ACQUIREFILE_JPEGXR              433
#define IDM_ACQUIREFILE_PAINTSHOP           434
#define IDM_ACQUIREFILE_PCX                 435
#define IDM_ACQUIREFILE_PDF                 436
#define IDM_ACQUIREFILE_PNG                 437
#define IDM_ACQUIREFILE_POSTSCRIPTLEVEL1    438
#define IDM_ACQUIREFILE_POSTSCRIPTLEVEL2    439
#define IDM_ACQUIREFILE_SVG                 440
#define IDM_ACQUIREFILE_SVGZ                441
#define IDM_ACQUIREFILE_TGA                 442
#define IDM_ACQUIREFILE_TGARLE              443
#define IDM_ACQUIREFILE_TEXT                444
#define IDM_ACQUIREFILE_TIFF_NOCOMPRESSION  445
#define IDM_ACQUIREFILE_TIFF_GROUP3         446
#define IDM_ACQUIREFILE_TIFF_GROUP4         447
#define IDM_ACQUIREFILE_TIFF_FLATE          448
#define IDM_ACQUIREFILE_TIFF_JPEG           449
#define IDM_ACQUIREFILE_TIFF_LZW            450
#define IDM_ACQUIREFILE_TIFF_PACKBITS       451
#define IDM_ACQUIREFILE_WEBP                452
#define IDM_ACQUIREFILE_WINDOWSMETAFILE     453
#define IDM_ACQUIREFILE_WIRELESSBITMAP      454

#define IDD_dlgEnterSourceName              1000
#define IDC_edSourceName                    1001
#define IDD_dlgEnterCustomLangName          1002

#define IDD_dlgSelectCustom                 2000
#define IDC_lstSources                      2001
#define IDC_edNumSources                    2002

#define IDD_dlgProperties                   3000
#define IDC_edProductName                   3001
#define IDC_edFamilyName                    3002
#define IDC_edManufacturer                  3003
#define IDC_edVersionInfo                   3004
#define IDC_edVersion                       3005
#define IDC_lstCapabilities                 3006
#define IDC_edTotalCaps                     3007
#define IDC_edCustomCaps                    3008
#define IDC_edExtendedCaps                  3009
#define IDC_btnTestCap                      3010

#define IDD_dlgSettings                     5000
#define IDC_chkUseFeeder                    5001
#define IDC_chkUseDuplex                    5002

#define IDD_dlgFileType						6000
#define IDC_cmbFileType						6001
#define IDC_edFileName                      6002

#define IDD_dlgDebug                        7000
#define IDC_radNoLogging                    7001
#define IDC_radLogToFile                    7002
#define IDC_edLogFileName                   7003
#define IDC_radToMonitor                    7004
#define IDC_chkUseDebugView                 7005
#define IDC_btnBrowse                       7006
#define IDM_LOGGING_OPTIONS                 7007
#define IDC_radToConsole                    7008
#define IDC_edDSData                        7009
#define IDC_edCopyright                     7010
#define IDC_edJSONDetails                   7011
#define IDC_edLangName                      7012
#define IDC_edBarCodes                      7013
#define IDD_dlgBarCodes                     7014
#define IDD_dlgEnterFileName                7015

#define IDD_dlgTestCap                      8000
#define IDC_cmbGetTypes                     8001
#define IDC_lstResults                      8002  
#define IDC_btnTest                         8003
#define IDC_cmbContainer                    8004
#define IDC_cmbDataType                     8005
#define IDC_btnReset                        8006
#define IDC_cmbSetTypes						8007
#define IDC_cmbDataTypeSet                  8008
#define IDC_cmbContainerSet                 8009 
#define IDC_btnTestSet                      8010  
#define IDC_btnResetSet                     8011
#define IDC_lstResultsSet                   8012
#define IDC_edSetInput                      8013
#define IDC_staticSetOperation              8014 
#define IDC_staticDataType                  8015
#define IDC_staticContainer                 8016
#define IDC_staticResults                   8017
#define IDC_staticInput                     8018
#define IDC_btnResetCapabilities            8019
#define IDC_staticTestGetResults            8020
#define IDC_btnShowUIIOnly                  8021
#define IDC_btnRefreshShowUIOnly            8022
#define IDC_chkResetCapsOnClose             8023
#define IDC_edSaveFileName                  8024

#define ID_LANGUAGE_ENGLISH             32771
#define ID_LANGUAGE_FRENCH              32772
#define ID_LANGUAGE_SPANISH             32773
#define ID_LANGUAGE_ITALIAN             32774
#define ID_LANGUAGE_GERMAN              32775
#define ID_LANGUAGE_DUTCH               32776
#define ID_LANGUAGE_RUSSIAN             32777
#define ID_LANGUAGE_ROMANIAN            32778
#define ID_LANGUAGE_SIMPLIFIEDCHINESE   32779
#define ID_LANGUAGE_CUSTOMLANGUAGE      32780
#define ID_LANGUAGE_PORTUGUESE          32781
#define ID_LANGUAGE_JAPANESE            32782
#define ID_LANGUAGE_TRADITIONALCHINESE  32783
#define ID_LANGUAGE_KOREAN              32784
#define ID_LANGUAGE_GREEK               32785


#endif
