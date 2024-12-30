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
