// Example1.c : Acquires an image from a TWAIN Source
//
#ifdef _MSC_VER
    #define _CRT_SECURE_NO_WARNINGS
#endif
#include <windows.h>
#include <windowsx.h>
#include "dtwdemo.h"
#include "dtwain.h"
#include "dibdisplay.h"
#include <tchar.h>
#include <stdio.h>
#include <io.h>
#include "SourceProperties.h"
#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
TCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];            // The title bar text
char szBarCodeText[500000];

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK    About(HWND, UINT, WPARAM, LPARAM);


// Global Variables
DTWAIN_SOURCE g_CurrentSource=NULL;
DTWAIN_SOURCE g_NamedSource=NULL;
HWND          g_hWnd;
HMENU         g_Menu;
HINSTANCE     g_hInstance;
DTWAIN_ARRAY  g_AcquireArray;
LONG          g_FileType;
TCHAR         g_FileName[256];
int           g_LogType;
TCHAR         g_LogFileName[MAX_PATH];

void SelectTheSource(int nWhich);
void EnableSourceItems(BOOL bEnable);
void EnableSelectSourceItems(BOOL bEnable);
void EnableAllMenuItems(BOOL bEnable);
void EnableBarcodeMenuItem(DTWAIN_SOURCE source);
DTWAIN_SOURCE DisplayGetNameDlg();
DTWAIN_SOURCE DisplayCustomDlg();
void DisplaySourceProps();
void SetCaptionToSourceName();
void AcquireNative();
void AcquireBuffered();
void AcquireFile(BOOL bUseSource, LONG fileType);
BOOL IsAllSpace(LPCTSTR p);
void ToggleCheckedItem(UINT resId);
BOOL GetToggleMenuState(UINT resID);
void DisplayLoggingOptions();
void LoadLanguage(int message);
void LoadLanguageStrings(LPCTSTR szLang);
void DisplayCustomLangDlg();
void EnableFileXFerMenuItems(DTWAIN_SOURCE source, BOOL bEnable);
INT_PTR DisplayGetFileNameDlg();

LRESULT CALLBACK EnterCustomLangNameProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

BOOL bPageOK;
LONG nMajorVer, nMinorVer, nDTwainType;

void WaitLoop();
extern DWORD DIBHeight(LPSTR);

// if the following line doesn't compile, please change LONG_PTR to LONG
LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData);

// Dialog functions
LRESULT CALLBACK EnterSourceNameProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayCustomSelectProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayBarCodeInfoProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayAcquireSettingsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayLoggingProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayTestCapProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK EnterFileNameProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
INT_PTR DisplayOneDibPage(HINSTANCE hInstance, HANDLE hDib, UINT resID, HWND wndHandle);


// File types structure
typedef struct
{
    LPCTSTR fType;
    LONG DTWAINType;
    LPCTSTR defName;
} AllTypes ;

typedef struct  
{
    LONG langID;
    LPCTSTR language;
} AllLanguages;

typedef struct
{
    UINT resourceId;
    int  dtwainType;
} AllFileTypes;

AllLanguages g_allLanguages[] = { {ID_LANGUAGE_ENGLISH               , _T("english")},
                                 {ID_LANGUAGE_FRENCH                , _T("french")},
                                 {ID_LANGUAGE_SPANISH               , _T("spanish")},
                                 {ID_LANGUAGE_ITALIAN               , _T("italian")},
                                 {ID_LANGUAGE_GERMAN                , _T("german")},
                                 {ID_LANGUAGE_GREEK                 , _T("greek")},
                                 {ID_LANGUAGE_DUTCH                 , _T("dutch")},
                                 {ID_LANGUAGE_RUSSIAN               , _T("russian")},
                                 {ID_LANGUAGE_ROMANIAN              , _T("romanian")},
                                 {ID_LANGUAGE_PORTUGUESE              , _T("portuguese")},
                                 {ID_LANGUAGE_SIMPLIFIEDCHINESE     , _T("simplified_chinese")},
                                 {ID_LANGUAGE_TRADITIONALCHINESE    , _T("traditional_chinese")},
                                 {ID_LANGUAGE_JAPANESE              , _T("japanese")},
                                 {ID_LANGUAGE_KOREAN                , _T("korean")}
                                };

AllFileTypes g_allDTWAINFileTypes[] = {
        {IDM_ACQUIREFILESOURCE_WINDOWSBMP, DTWAIN_FF_BMP},
        {IDM_ACQUIREFILESOURCE_JPEG, DTWAIN_FF_JFIF},
        {IDM_ACQUIREFILESOURCE_TIFF, DTWAIN_FF_TIFF },
        {IDM_ACQUIREFILESOURCE_TIFFMULTIPAGE, DTWAIN_FF_TIFFMULTI},
        {IDM_ACQUIREFILESOURCE_PNG, DTWAIN_FF_PNG},
        {IDM_ACQUIREFILESOURCE_PDF, DTWAIN_FF_PDF},
        {IDM_ACQUIREFILESOURCE_PDFA, DTWAIN_FF_PDFA},
        {IDM_ACQUIREFILESOURCE_PDFA2, DTWAIN_FF_PDFA2},
        {IDM_ACQUIREFILESOURCE_PDFRASTER, DTWAIN_FF_PDFRASTER},
        {IDM_ACQUIREFILESOURCE_FLASHPIX, DTWAIN_FF_FPX},
        {IDM_ACQUIREFILESOURCE_EXIF, DTWAIN_FF_EXIF},
        {IDM_ACQUIREFILESOURCE_SPIFF, DTWAIN_FF_SPIFF},
        {IDM_ACQUIREFILESOURCE_XBM, DTWAIN_FF_XBM},
        {IDM_ACQUIREFILESOURCE_PICT, DTWAIN_FF_PICT},
        {IDM_ACQUIREFILESOURCE_JP2, DTWAIN_FF_JP2},
        {IDM_ACQUIREFILESOURCE_JPX, DTWAIN_FF_JPX},
        {IDM_ACQUIREFILESOURCE_DEJAVU, DTWAIN_FF_DEJAVU},
        {IDM_ACQUIREFILE_BIGTIFF_NOCOMPRESSION  ,  DTWAIN_BIGTIFFNONEMULTI },
        {IDM_ACQUIREFILE_BIGTIFF_GROUP3         ,  DTWAIN_BIGTIFFG3MULTI },
        {IDM_ACQUIREFILE_BIGTIFF_GROUP4         ,  DTWAIN_BIGTIFFG4MULTI },
        {IDM_ACQUIREFILE_BIGTIFF_FLATE          ,  DTWAIN_BIGTIFFDEFLATEMULTI },
        {IDM_ACQUIREFILE_BIGTIFF_JPEG           ,  DTWAIN_BIGTIFFJPEGMULTI },
        {IDM_ACQUIREFILE_BIGTIFF_LZW            ,  DTWAIN_BIGTIFFLZWMULTI },
        {IDM_ACQUIREFILE_BIGTIFF_PACKBITS       ,  DTWAIN_BIGTIFFPACKBITS },
        {IDM_ACQUIREFILE_BMP                    ,  DTWAIN_BMP },
        {IDM_ACQUIREFILE_BMPRLE                 ,  DTWAIN_BMP_RLE },
        {IDM_ACQUIREFILE_DCX                    ,  DTWAIN_DCX },
        {IDM_ACQUIREFILE_ENHANCEDMETAFILE       ,  DTWAIN_EMF },
        {IDM_ACQUIREFILE_GIF                    ,  DTWAIN_GIF },
        {IDM_ACQUIREFILE_ICO                    ,  DTWAIN_ICO_RESIZED },
        {IDM_ACQUIREFILE_ICOVISTA               ,  DTWAIN_ICO_VISTA },
        {IDM_ACQUIREFILE_JPEG                   ,  DTWAIN_JPEG },
        {IDM_ACQUIREFILE_JPEG2000               ,  DTWAIN_JPEG2000 },
        {IDM_ACQUIREFILE_JPEGXR                 ,  DTWAIN_JPEGXR },
        {IDM_ACQUIREFILE_PAINTSHOP              ,  DTWAIN_PSD },
        {IDM_ACQUIREFILE_PCX                    ,  DTWAIN_PCX },
        {IDM_ACQUIREFILE_PDF                    ,  DTWAIN_PDFMULTI },
        {IDM_ACQUIREFILE_PNG                    ,  DTWAIN_PNG },
        {IDM_ACQUIREFILE_POSTSCRIPTLEVEL1       ,  DTWAIN_POSTSCRIPT1MULTI },
        {IDM_ACQUIREFILE_POSTSCRIPTLEVEL2       ,  DTWAIN_POSTSCRIPT2MULTI },
        {IDM_ACQUIREFILE_SVG                    ,  DTWAIN_SVG },
        {IDM_ACQUIREFILE_SVGZ                   ,  DTWAIN_SVGZ },
        {IDM_ACQUIREFILE_TGA                    ,  DTWAIN_TGA },
        {IDM_ACQUIREFILE_TGARLE                 ,  DTWAIN_TGA_RLE },
        {IDM_ACQUIREFILE_TEXT                   ,  DTWAIN_TEXTMULTI },
        {IDM_ACQUIREFILE_TIFF_NOCOMPRESSION     ,  DTWAIN_TIFFNONEMULTI },
        {IDM_ACQUIREFILE_TIFF_GROUP3            ,  DTWAIN_TIFFG3MULTI },
        {IDM_ACQUIREFILE_TIFF_GROUP4            ,  DTWAIN_TIFFG4MULTI },
        {IDM_ACQUIREFILE_TIFF_FLATE             ,  DTWAIN_TIFFDEFLATEMULTI },
        {IDM_ACQUIREFILE_TIFF_JPEG              ,  DTWAIN_TIFFJPEGMULTI },
        {IDM_ACQUIREFILE_TIFF_LZW               ,  DTWAIN_TIFFLZWMULTI },
        {IDM_ACQUIREFILE_TIFF_PACKBITS          ,  DTWAIN_TIFFPACKBITSMULTI },
        {IDM_ACQUIREFILE_WEBP                   ,  DTWAIN_WEBP },
        {IDM_ACQUIREFILE_WINDOWSMETAFILE        ,  DTWAIN_WMF },
        {IDM_ACQUIREFILE_WIRELESSBITMAP         ,  DTWAIN_WBMP_RESIZED },
    };

const UINT nFirstAcquireSourceID = IDM_ACQUIREFILESOURCE_WINDOWSBMP;
const UINT nLastAcquireSourceID = IDM_ACQUIREFILESOURCE_DEJAVU;

const UINT nFirstAcquireFileID = IDM_ACQUIREFILE_BIGTIFF_NOCOMPRESSION;
const UINT nLastAcquireFileID = IDM_ACQUIREFILE_WIRELESSBITMAP;
const UINT numDTWAINFileTypes = sizeof(g_allDTWAINFileTypes) / sizeof(g_allDTWAINFileTypes[0]);

UINT g_AllMenuItems[] = { IDM_SELECT_SOURCE,
                          IDM_SELECT_SOURCE_BY_NAME,
                          IDM_SELECT_DEFAULT_SOURCE,
                          IDM_SELECT_SOURCE_CUSTOM,
                          IDM_SOURCE_PROPS,
                          IDM_CLOSE_SOURCE,
                          IDM_EXIT,
                          IDM_ACQUIRE_NATIVE,
                          IDM_ACQUIRE_BUFFERED,
                          IDM_ACQUIRE_FILE_DTWAIN,
                          IDM_ACQUIRE_FILE_SOURCE,
                          IDM_SHOW_PREVIEW,
                          IDM_USE_SOURCE_UI,
                          IDM_DISCARD_BLANKS,
                          IDM_SHOW_BARCODEINFO };

TCHAR g_CustomLanguage[256];


DTWAIN_PDFTEXTELEMENT g_PDFTextElement;

int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       nCmdShow)
{
    MSG msg;
    HACCEL hAccelTable;
    if ( !DTWAIN_IsTwainAvailable() )
    {
        MessageBox(NULL, _T("TWAIN is not installed!\r\nExiting..."), _T("Error"), MB_ICONSTOP);
        return FALSE;
    }

    // Initialize global strings
    LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInstance, IDC_DTWDEMO, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // Perform application initialization:
    if (!InitInstance (hInstance, nCmdShow))
        return FALSE;

    g_hInstance = hInstance;
    hAccelTable = LoadAccelerators(hInstance, (LPCTSTR)IDC_DTWDEMO);

    /* Initialize DTWAIN.  Quit if error! */
    if ( !DTWAIN_SysInitialize( ))
        return 0;
    DTWAIN_SetAppInfoA("1.0","Demo Program Menu", "Demo Program Family", "Demo Program Name");

    /* Allow DTWAIN messages to be sent directly to our Window proc */
    DTWAIN_StartTwainSession(g_hWnd, NULL);
    // DTWAIN_SetTwainMode(DTWAIN_MODELESS);
    DTWAIN_EnableMsgNotify(TRUE);

    /* Also allow DTWAIN messages to be sent to our callback */
    DTWAIN_SetCallback(TwainCallbackProc,0);

    /* Call function to determine the DTWAIN version */
    DTWAIN_GetVersion(&nMajorVer, &nMinorVer, &nDTwainType);

    /* Create a PDF text element for usage when acquiring to a PDF file */
    g_PDFTextElement = DTWAIN_CreatePDFTextElement();

    /* Position, font height, font color, and display pages for the PDF text element */
    DTWAIN_SetPDFTextElementFloat(g_PDFTextElement, 25, 0, DTWAIN_PDFTEXTELEMENT_FONTHEIGHT);
    DTWAIN_SetPDFTextElementLong(g_PDFTextElement, 100, 100, DTWAIN_PDFTEXTELEMENT_POSITION);
    DTWAIN_SetPDFTextElementLong(g_PDFTextElement, DTWAIN_MakeRGB(127, 127, 127), 0, DTWAIN_PDFTEXTELEMENT_COLOR);
    DTWAIN_SetPDFTextElementLong(g_PDFTextElement, DTWAIN_PDFTEXT_ALLPAGES, 0, DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS);

    /* Main message loop: */
    while (GetMessage(&msg, NULL, 0, 0))
    {
        if ( !DTWAIN_IsTwainMsg(&msg) )  // send message to TWAIN if DTWAIN message
        {
            if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
            {
                TranslateMessage(&msg);
                DispatchMessage(&msg);
            }
        }
    }
    DTWAIN_SysDestroy();

    return (int)msg.wParam;
}


ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = (WNDPROC)WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, (LPCTSTR)IDI_DTWDEMO);
    wcex.hCursor        = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = (LPCTSTR)IDC_DTWDEMO;
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = (HICON)LoadIcon((HINSTANCE)wcex.hInstance, (LPCTSTR)IDI_SMALL);

    return RegisterClassEx(&wcex);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   hInst = hInstance; // Store instance handle in our global variable

   g_hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);
   EnableFileXFerMenuItems(NULL, FALSE);
   if (!g_hWnd)
   {
      return FALSE;
   }

   ShowWindow(g_hWnd, nCmdShow);
   UpdateWindow(g_hWnd);

   return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    UINT wmId, wmEvent;
    TCHAR szHello[MAX_LOADSTRING];
    LoadString(hInst, IDS_HELLO, szHello, MAX_LOADSTRING);

    switch (message)
    {
        case WM_CREATE:
            g_Menu = GetMenu(hWnd);
            EnableSourceItems(FALSE);
            EnableFileXFerMenuItems(NULL, FALSE);
            CheckMenuItem(g_Menu, IDM_USE_SOURCE_UI, MF_BYCOMMAND | MF_CHECKED);
            CheckMenuItem(g_Menu, IDM_SHOW_PREVIEW, MF_BYCOMMAND | MF_CHECKED);
            CheckMenuItem(g_Menu, IDM_SHOW_BARCODEINFO, MF_BYCOMMAND | MF_CHECKED);
            break;

        case WM_COMMAND:
            wmId    = LOWORD(wParam);
            wmEvent = HIWORD(wParam);

            /* See if the acquisition is to a file using source mode */
            UINT i;
            if (wmId >= nFirstAcquireSourceID && wmId <= nLastAcquireFileID)
            {
                for (i = 0; i < numDTWAINFileTypes; ++i)
                {
                    if (g_allDTWAINFileTypes[i].resourceId == wmId)
                    {
                        g_FileType = g_allDTWAINFileTypes[i].dtwainType;
                        AcquireFile(g_allDTWAINFileTypes[i].resourceId < nFirstAcquireFileID, g_allDTWAINFileTypes[i].dtwainType);
                        break;
                    }
                }
                return 0;
            }

            /* Parse the menu selections : */
            switch (wmId)
            {
                case IDM_ABOUT:
                   DialogBox(hInst, (LPCTSTR)IDD_ABOUTBOX, hWnd, (DLGPROC)About);
                   break;

                case IDM_SELECT_SOURCE:
                case IDM_SELECT_SOURCE_BY_NAME:
                case IDM_SELECT_DEFAULT_SOURCE:
                case IDM_SELECT_SOURCE_CUSTOM:
                    SelectTheSource(wmId);
                break;

                case IDM_SOURCE_PROPS:
                    DisplaySourceProps();
                break;

                case IDM_CLOSE_SOURCE:
                    if ( g_CurrentSource )
                    {
                        DTWAIN_CloseSource(g_CurrentSource);
                        g_CurrentSource = NULL;
                        EnableSourceItems(FALSE);
                        EnableFileXFerMenuItems(NULL, FALSE);
                        SetCaptionToSourceName();
                    }
                break;

                case IDM_ACQUIRE_NATIVE:
                    EnableAllMenuItems(FALSE);
                    AcquireNative();
                    EnableAllMenuItems(TRUE);
                break;

                case IDM_ACQUIRE_BUFFERED:
                    EnableAllMenuItems(FALSE);
                    AcquireBuffered();
                    EnableAllMenuItems(TRUE);
                break;
                case IDM_ACQUIRE_FILE_DTWAIN:
                    EnableAllMenuItems(FALSE);
                    AcquireFile(FALSE, 0);
                    EnableAllMenuItems(TRUE);
                break;

                case IDM_ACQUIRE_FILE_SOURCE:
                    AcquireFile(TRUE, 0);
                break;

                case IDM_ACQUIRETEST_USEGETMESSAGE:
                    ToggleCheckedItem(IDM_ACQUIRETEST_USEGETMESSAGE);
                    if (g_CurrentSource)
                    {
                        BOOL useGetMessage = GetToggleMenuState(IDM_ACQUIRETEST_USEGETMESSAGE);
                        DTWAIN_EnablePeekMessageLoop(g_CurrentSource, !useGetMessage);
                    }
                break;

                case IDM_USE_SOURCE_UI:
                    ToggleCheckedItem(IDM_USE_SOURCE_UI);
                break;

                case IDM_DISCARD_BLANKS:
                    ToggleCheckedItem(IDM_DISCARD_BLANKS);
                break;

                case IDM_SHOW_PREVIEW:
                    ToggleCheckedItem(IDM_SHOW_PREVIEW);
                break;

                case IDM_SHOW_BARCODEINFO:
                    ToggleCheckedItem(IDM_SHOW_BARCODEINFO);
                break;

                case IDM_LOGGING_OPTIONS:
                    DisplayLoggingOptions();
                break;

                case ID_LANGUAGE_ENGLISH            : 
                case ID_LANGUAGE_FRENCH             : 
                case ID_LANGUAGE_SPANISH            : 
                case ID_LANGUAGE_ITALIAN            : 
                case ID_LANGUAGE_GERMAN             : 
                case ID_LANGUAGE_DUTCH              : 
                case ID_LANGUAGE_GREEK              :
                case ID_LANGUAGE_JAPANESE           :
                case ID_LANGUAGE_KOREAN             :
                case ID_LANGUAGE_TRADITIONALCHINESE :
                case ID_LANGUAGE_RUSSIAN            : 
                case ID_LANGUAGE_ROMANIAN           : 
                case ID_LANGUAGE_SIMPLIFIEDCHINESE  : 
                case ID_LANGUAGE_PORTUGUESE:
                    LoadLanguage(wmId);
                break;

                case ID_LANGUAGE_CUSTOMLANGUAGE:
                    DisplayCustomLangDlg();
                break;

                case IDM_EXIT:
                    if (!DTWAIN_IsAcquiring())
                        DestroyWindow(hWnd);
                    else
                        MessageBox(NULL, _T("Cannot close application.  Images are still being acquired.\r\nPlease close the device user interface."), _T("Device is acquiring"), MB_OK);
                    return 0;
                default:
                   return DefWindowProc(hWnd, message, wParam, lParam);
            }
            break;
        case WM_CLOSE:
            if (!DTWAIN_IsAcquiring())
                DestroyWindow(hWnd);
            else
                MessageBox(NULL, _T("Cannot close application.  Images are still being acquired.\r\nPlease close the device user interface."), _T("Device is acquiring"), MB_OK);
            return 0;
        break;

        case WM_DESTROY:
            PostQuitMessage(0);
            break;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
   }
   return 0;
}

void LoadLanguage(int message)
{
    LPCSTR langugeToLoad = NULL;
    int numLanguages = sizeof(g_allLanguages) / sizeof(g_allLanguages[0]);
    for (int i = 0; i < numLanguages; ++i)
    {
        if (message == g_allLanguages[i].langID)
        {
            LoadLanguageStrings(g_allLanguages[i].language);
            return;
        }
    }
    MessageBox(NULL, _T("Could not load language resource"), _T("Language Resource Error"), MB_ICONSTOP);
}

void LoadLanguageStrings(LPCTSTR szLang)
{
    BOOL bRet = DTWAIN_LoadCustomStringResources(szLang);
    if (!bRet)
        MessageBox(NULL, _T("Could not load language resource"), _T("Language Resource Error"), MB_ICONSTOP);
    else
        MessageBox(g_hWnd, _T("Custom resource loaded.  Select a Source or choose Logging to see the new language being used"), _T("Success"), MB_OK);
}

void ToggleCheckedItem(UINT resId)
{
    UINT nState = GetMenuState(g_Menu, resId, MF_BYCOMMAND);
    if ( nState != -1 )
    {
        UINT flag = (nState & MF_CHECKED)?MF_UNCHECKED : MF_CHECKED;
        CheckMenuItem(g_Menu, resId, flag);
    }
}

BOOL GetToggleMenuState(UINT resID)
{
    UINT nState = GetMenuState(g_Menu, resID, MF_BYCOMMAND);
    if ( nState != -1 )
        return (nState & MF_CHECKED)?TRUE:FALSE;
    return TRUE;
}

BOOL IsMenuItemEnabled(UINT resID) 
{
	UINT nState = GetMenuState(g_Menu, resID, MF_BYCOMMAND);
	if (nState != -1)
		return (nState & (MF_DISABLED | MF_GRAYED)) ? FALSE: TRUE;
	return FALSE;
}

void SelectTheSource(int nWhich)
{
    DTWAIN_SOURCE tempSource=NULL;
    if ( g_CurrentSource )
    {
        int nReturn = MessageBox(g_hWnd, _T("For this demo, only one Source can be opened.\r\n")
                                          _T("Close current Source?"), _T("DTWAIN Message"), MB_YESNO);
        if (nReturn == IDYES)
        {
            DTWAIN_CloseSource(g_CurrentSource);
            g_CurrentSource = NULL;
            EnableSourceItems(FALSE);
        }
        else
            return;
    }

    EnableSelectSourceItems(FALSE);
    switch (nWhich)
    {
        case IDM_SELECT_SOURCE:
            tempSource = DTWAIN_SelectSource2(NULL, NULL,0,0, DTWAIN_DLG_CENTER_CURRENT_MONITOR| DTWAIN_DLG_SORTNAMES);
        break;

        case IDM_SELECT_DEFAULT_SOURCE:
            tempSource = DTWAIN_SelectDefaultSource();
        break;

        case IDM_SELECT_SOURCE_BY_NAME:
            tempSource = DisplayGetNameDlg();
        break;

        case IDM_SELECT_SOURCE_CUSTOM:
            tempSource = DisplayCustomDlg();
        break;
    }
    EnableSelectSourceItems(TRUE);
    if ( tempSource )
    {
        if ( DTWAIN_OpenSource(tempSource) )
        {
            // Enable the barcode detection (only valid if source supports barcode detection)
            DTWAIN_EnableBarcodeDetection(tempSource, TRUE);

            // We want to make sure that when we acquire to a PDF file, we will "stamp"
            // each PDF page with the text that g_PDFTextElement will have (see TwainCallbackProc)
            DTWAIN_AddPDFTextElement(tempSource, g_PDFTextElement);

            // Enable all of the items in the menu, depending on what the source supports
            EnableBarcodeMenuItem(tempSource);
            EnableFileXFerMenuItems(tempSource, TRUE);
            EnableSourceItems(TRUE);

            g_CurrentSource = tempSource;
            SetCaptionToSourceName();
            DTWAIN_EnableFeeder(tempSource, TRUE);

            // Enable or disable the PeekMessage() TWAIN loop processing
            DTWAIN_EnablePeekMessageLoop(tempSource, !GetToggleMenuState(IDM_ACQUIRETEST_USEGETMESSAGE));
        }
        else
            MessageBox(g_hWnd, _T("Error Opening Source"), _T("TWAIN Error"), MB_ICONSTOP);
    }
    else
    {
        LONG lastError = DTWAIN_GetLastError();
        wchar_t szCancelMsg[256];

        // We will use the wide version, to ensure we get the proper UTF-8 string
        DTWAIN_GetErrorStringW(lastError, szCancelMsg, 256);
        MessageBoxW(g_hWnd, szCancelMsg, L"Information", MB_ICONSTOP);
    }
}

void SetCaptionToSourceName()
{
    TCHAR szBuf[256];
    TCHAR szTotal[320];

    lstrcpy(szBuf, szTitle);

    if ( g_CurrentSource )
    {
        DTWAIN_GetSourceProductName(g_CurrentSource, szBuf, 255);
        wsprintf(szTotal, _T("%s - %s"), szTitle, szBuf);
        SetWindowText(g_hWnd, szTotal);
    }
    else
        SetWindowText(g_hWnd, szTitle);
}

void GenericAcquire(LONG nWhichOne)
{
    LONG ErrStatus;
    /* Disable main window */
    DTWAIN_DisableAppWindow(g_hWnd, TRUE);

    /* Check if feeder or duplex is supported */
    if ( DTWAIN_IsFeederSupported(g_CurrentSource) || DTWAIN_IsDuplexSupported(g_CurrentSource))
        DialogBox(g_hInstance, (LPCTSTR)IDD_dlgSettings, g_hWnd, (DLGPROC)DisplayAcquireSettingsProc);

    /* Check if we want to discard blank pages */
    /* Set the threshold to 98% blank */
    DTWAIN_SetBlankPageDetection(g_CurrentSource, 98.0, DTWAIN_BP_AUTODISCARD_ANY, 
                                 GetToggleMenuState(IDM_DISCARD_BLANKS));

    BOOL bRet = FALSE;
    EnableSourceItems(FALSE);
    g_AcquireArray = DTWAIN_CreateAcquisitionArray();
    if (nWhichOne == 0)
    {
        bRet = DTWAIN_AcquireNativeEx(
            g_CurrentSource,
            DTWAIN_PT_DEFAULT, /* Use default */
            DTWAIN_ACQUIREALL, /* Get all pages */
            GetToggleMenuState(IDM_USE_SOURCE_UI),
            FALSE,  /* Close Source when UI is closed */
            g_AcquireArray,
            &ErrStatus /* Error Status */
        );
    }
    else
    {
        bRet = DTWAIN_AcquireBufferedEx(
            g_CurrentSource,
            DTWAIN_PT_DEFAULT, /* Use default */
            DTWAIN_ACQUIREALL, /* Get all pages */
            GetToggleMenuState(IDM_USE_SOURCE_UI),
            TRUE,  /* Close Source when UI is closed */
            g_AcquireArray,
            &ErrStatus /* Error Status */
        );
    }
    EnableSourceItems(TRUE);
    if (!bRet)
    {
        LONG lastError = DTWAIN_GetLastError();
        char szError[1024];
        if (ErrStatus == DTWAIN_TN_ACQUIRECANCELED)
            MessageBox(NULL, _T("Acquisition cancelled without acquiring any images"), _T("Information"), MB_ICONSTOP);
        else
        {
            DTWAIN_GetErrorStringA(lastError, szError, 1023);
            MessageBoxA(NULL, szError, "TWAIN Error", MB_ICONSTOP);
        }
        DTWAIN_DestroyAcquisitionArray(g_AcquireArray, FALSE);
        return;
    }

    WaitLoop();
    if ( DTWAIN_ArrayGetCount(g_AcquireArray) == 0 )
    {
        MessageBox(g_hWnd, _T("No Images Acquired"), _T(""), MB_ICONSTOP);
        DTWAIN_DestroyAcquisitionArray(g_AcquireArray, FALSE);
        return;
    }
    RetrieveAndDisplayDibs(g_hInstance, g_AcquireArray, IDD_dlgDib, g_hWnd);
    DTWAIN_DestroyAcquisitionArray( g_AcquireArray, FALSE );
}

void AcquireNative()
{
    GenericAcquire(0);
}

void AcquireBuffered()
{
    GenericAcquire(1);
}

void AcquireFile(BOOL bUseSource, LONG fileType)
{
    LONG ErrStatus;
    LONG FileFlags = DTWAIN_USELONGNAME;
    BOOL bAcquireOK = TRUE;
    DTWAIN_ARRAY AFileNames = 0;
    BOOL UseUI;
    TCHAR szError[256];
    INT_PTR retValue;

    if (bUseSource)
    {
        MessageBox(g_hWnd, _T("Note: Image preview is not available when acquiring files using Source Mode"), _T("Image Preview not available"), MB_ICONSTOP);
        FileFlags |= DTWAIN_USESOURCEMODE;
    }
    else
        FileFlags |= DTWAIN_USENATIVE;

    retValue = DisplayGetFileNameDlg();
    if (g_FileName[0] == 0 && retValue != IDCANCEL)
    {
        MessageBox(g_hWnd, _T("Cannot enter a blank image file name"), _T("Error"), MB_ICONSTOP);
        return;
    }
    if (retValue == IDCANCEL)
        return;
    /* This is just one of many options that can be set
        for DTWAIN / PDF support */

    if ( fileType == DTWAIN_PDFMULTI )
        DTWAIN_SetPDFPageSize(g_CurrentSource, DTWAIN_FS_USLETTER, 0, 0);

    /* Check if TEXT type is acquired */
    if ( fileType == DTWAIN_TEXTMULTI )
    {
        /* Initialize the OCR engine if it's available */
        if (!DTWAIN_InitOCRInterface() )
        {
            MessageBox(g_hWnd, _T("Text mode requires OCR engine installed"), _T("No OCR engines detected"), MB_ICONSTOP);
            return;
        }
        else
        {
            if ( !DTWAIN_SelectDefaultOCREngine() )
            {
                MessageBox(g_hWnd, _T("OCR engine could not be selected"), _T("OCR engine error"), MB_ICONSTOP);
                return;
            }
        }
    }

    /* Disable main window */
    /* Check if feeder or duplex is supported */
    if ( DTWAIN_IsFeederSupported(g_CurrentSource) || DTWAIN_IsDuplexSupported(g_CurrentSource))
        DialogBox(g_hInstance, (LPCTSTR)IDD_dlgSettings, g_hWnd, (DLGPROC)DisplayAcquireSettingsProc);
    EnableWindow(g_hWnd, FALSE);
    
    /* Create the array of names.  This function is to be used
       since the user may have entered a file name that has
       embedded spaces */
    AFileNames = DTWAIN_ArrayCreate(DTWAIN_ARRAYSTRING, 1);
    DTWAIN_ArraySetAt( AFileNames, 0, g_FileName );

    /* Acquire the file */
    UseUI = GetToggleMenuState(IDM_USE_SOURCE_UI);
    EnableSourceItems(FALSE);
    bAcquireOK = DTWAIN_AcquireFileEx(g_CurrentSource,
                                  AFileNames,
                                  fileType,
                                  FileFlags | DTWAIN_CREATE_DIRECTORY,
                                  DTWAIN_PT_DEFAULT, /* Use default */
                                  DTWAIN_ACQUIREALL, /* Get all pages */
                                  UseUI,
                                  TRUE,  /* Close Source when UI is closed */
                                  &ErrStatus /* Error Status */
                                  );
    if (!bAcquireOK)
    {
        DTWAIN_GetErrorString(DTWAIN_GetLastError(), szError, 255);
    }
    WaitLoop();
    EnableWindow(g_hWnd, TRUE);
    EnableSourceItems(TRUE);

    DTWAIN_ArrayDestroy( AFileNames );
    LONG pageCount = DTWAIN_GetFileSavePageCount(g_CurrentSource);
    if ( !bAcquireOK || pageCount == 0 || !bPageOK )
    {
        if ( !bAcquireOK)
            MessageBox(g_hWnd, szError, _T(""), MB_ICONSTOP);
        else
            MessageBox(g_hWnd, _T("No Images Acquired"), _T(""), MB_OK);
        return;
    }
    else
    {
        if (_taccess(g_FileName, 0) == 0)
        {
            MessageBox(g_hWnd, _T("Images Acquired"), _T(""), MB_OK);
            return;
        }
    }
}


DTWAIN_SOURCE DisplayGetNameDlg()
{
    g_NamedSource = NULL;
    DialogBox(g_hInstance, (LPCTSTR)IDD_dlgEnterSourceName, g_hWnd, (DLGPROC)EnterSourceNameProc);
    return g_NamedSource;
}

INT_PTR DisplayGetFileNameDlg()
{
    return DialogBox(g_hInstance, (LPCTSTR)IDD_dlgEnterFileName, g_hWnd, (DLGPROC)EnterFileNameProc);
}

void DisplayCustomLangDlg()
{
    DialogBox(g_hInstance, (LPCTSTR)IDD_dlgEnterCustomLangName, g_hWnd, (DLGPROC)EnterCustomLangNameProc);
}

void DisplayBarCodeInfo()
{
    DialogBox(g_hInstance, (LPCTSTR)IDD_dlgBarCodes, g_hWnd, (DLGPROC)DisplayBarCodeInfoProc);
}

DTWAIN_SOURCE DisplayCustomDlg()
{
    g_NamedSource = NULL;
    DialogBox(g_hInstance, (LPCTSTR)IDD_dlgSelectCustom, g_hWnd, (DLGPROC)DisplayCustomSelectProc);
    return g_NamedSource;
}

void DisplaySourceProps()
{
    DialogBox(g_hInstance, (LPCTSTR)IDD_dlgProperties, g_hWnd, (DLGPROC)DisplaySourcePropsProc);
}

void DisplayLoggingOptions()
{
    LONG LogFlags = DTWAIN_LOG_ALL &~ (DTWAIN_LOG_ISTWAINMSG | DTWAIN_LOG_USEFILE | DTWAIN_LOG_DEBUGMONITOR | DTWAIN_LOG_CONSOLE);
    if ( DialogBox(g_hInstance, (LPCTSTR)IDD_dlgDebug, g_hWnd, (DLGPROC)DisplayLoggingProc) == IDOK )
    {
        // Make sure we make this exclusive by turning off all logging
        DTWAIN_SetTwainLog(0, _T(""));
        switch (g_LogType)
        {
            case 0:
            break;

            case 1:
                DTWAIN_SetTwainLog(DTWAIN_LOG_USEFILE | LogFlags, g_LogFileName);
            break;

            case 2:
                DTWAIN_SetTwainLog(DTWAIN_LOG_DEBUGMONITOR | LogFlags, _T(""));
            break;

            case 3:
                DTWAIN_SetTwainLog(DTWAIN_LOG_CONSOLE | LogFlags, _T(""));
            break;
        }
    }
}

void EnableSourceItems(BOOL bEnable)
{
    UINT nOptions;
    if ( !bEnable )
        nOptions = MF_BYCOMMAND | MF_GRAYED;
    else
        nOptions = MF_BYCOMMAND | MF_ENABLED;

    EnableMenuItem(g_Menu,IDM_SOURCE_PROPS, nOptions);
    EnableMenuItem(g_Menu,IDM_CLOSE_SOURCE, nOptions);
    EnableMenuItem(g_Menu,IDM_ACQUIRE_NATIVE, nOptions);
    EnableMenuItem(g_Menu,IDM_ACQUIRE_BUFFERED, nOptions);
    EnableMenuItem(g_Menu,IDM_ACQUIRE_FILE_DTWAIN, nOptions);
    EnableMenuItem(g_Menu,IDM_ACQUIRE_FILE_SOURCE, nOptions);
    EnableMenuItem(g_Menu,IDM_ACQUIRE_CLIPBOARD, nOptions);
    EnableMenuItem(g_Menu,IDM_ACQUIRE_OPTIONS, nOptions);
}

void EnableSelectSourceItems(BOOL bEnable)
{
    UINT nOptions;
    if (!bEnable)
        nOptions = MF_BYCOMMAND | MF_GRAYED;
    else
        nOptions = MF_BYCOMMAND | MF_ENABLED;
    EnableMenuItem(g_Menu, IDM_SELECT_SOURCE, nOptions);
    EnableMenuItem(g_Menu, IDM_SELECT_SOURCE_BY_NAME, nOptions);
    EnableMenuItem(g_Menu, IDM_SELECT_SOURCE_CUSTOM, nOptions);
    EnableMenuItem(g_Menu, IDM_SELECT_DEFAULT_SOURCE, nOptions);
    EnableMenuItem(g_Menu, IDM_EXIT, nOptions);
}

/* Dialog box to enter custom language name */
LRESULT CALLBACK EnterCustomLangNameProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            return TRUE;
        }

        case WM_COMMAND:
        {
            int nControl = LOWORD(wParam);
            int nNotification = HIWORD(wParam);

            switch( nControl )
            {
                /* Quit the dialog */
                case IDOK:
                {
                    HWND hWndEdit = GetDlgItem(hDlg, IDC_edLangName);
                    GetWindowText(hWndEdit, g_CustomLanguage, 255);
                    LoadLanguageStrings(g_CustomLanguage);
                    EndDialog(hDlg, LOWORD(wParam));
                }
                break;

                case IDCANCEL:
                    EndDialog(hDlg, LOWORD(wParam));
                    return TRUE;
                break;
            }
        }
        break;
    }
    return FALSE;
}


/* Dialog box to display source name to open */
LRESULT CALLBACK EnterSourceNameProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            return TRUE;
        }

        case WM_COMMAND:
        {
            int nControl = LOWORD(wParam);
            int nNotification = HIWORD(wParam);

            switch( nControl )
            {
                /* Quit the dialog */
                case IDOK:
                {
                    TCHAR szBuf[256];
                    DTWAIN_SOURCE tempSource;
                    HWND hWndEdit = GetDlgItem(hDlg, IDC_edSourceName);
                    GetWindowText(hWndEdit, szBuf, 255);
                    tempSource = DTWAIN_SelectSourceByName(szBuf);
                    if ( tempSource )
                    {
                        g_NamedSource = tempSource;
                        EndDialog(hDlg, LOWORD(wParam));
                    }
                    else
                    {
                        MessageBox(g_hWnd, _T("Could not select Source"), _T("Error"), MB_ICONSTOP);
                    }
                }
                break;

                case IDCANCEL:
                    EndDialog(hDlg, LOWORD(wParam));
                    return TRUE;
                break;
            }
        }
        break;
    }
    return FALSE;
}

/* Dialog box to display source name to open */
LRESULT CALLBACK EnterFileNameProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            return TRUE;
        }

        case WM_COMMAND:
        {
            int nControl = LOWORD(wParam);
            int nNotification = HIWORD(wParam);

            switch( nControl )
            {
                /* Quit the dialog */
                case IDOK:
                {
                    HWND hWndEdit = GetDlgItem(hDlg, IDC_edSaveFileName);
                    GetWindowText(hWndEdit, g_FileName, 255);
                    EndDialog(hDlg, LOWORD(wParam));
                }
                break;

                case IDCANCEL:
                    EndDialog(hDlg, LOWORD(wParam));
                    return TRUE;
                break;
            }
        }
        break;
    }
    return FALSE;
}

LRESULT CALLBACK DisplayLoggingProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            HWND hWndRadio = GetDlgItem(hDlg, IDC_radNoLogging);
            SendMessage(hWndRadio, BM_SETCHECK, BST_CHECKED, 0);
            return TRUE;
        }

        case WM_COMMAND:
        {

            int nControl = LOWORD(wParam);
            int nNotification = HIWORD(wParam);
            switch( nControl )
            {
                /* Quit the dialog */
                case IDOK:
                {
                    /* See which radio was selected */
                    HWND hWndRadio = GetDlgItem(hDlg, IDC_radNoLogging);
                    HWND hWndRadio2 = GetDlgItem(hDlg, IDC_radLogToFile);
                    HWND hWndRadio3 = GetDlgItem(hDlg, IDC_radToMonitor);
                    HWND hWndRadio4 = GetDlgItem(hDlg, IDC_radToConsole);

                    if ( SendMessage(hWndRadio, BM_GETCHECK, 0, 0) == BST_CHECKED)
                        g_LogType = 0;
                    else
                    if ( SendMessage(hWndRadio2, BM_GETCHECK, 0,0) == BST_CHECKED)
                    {
                        g_LogType = 1;
                        SendMessage(GetDlgItem(hDlg, IDC_edLogFileName), WM_GETTEXT, MAX_PATH, (LPARAM)g_LogFileName);
                        EndDialog(hDlg, LOWORD(wParam));
                    }
                    else
                    if ( SendMessage(hWndRadio3, BM_GETCHECK, 0,0 == BST_CHECKED) )
                        g_LogType = 2;
                    else
                    if (SendMessage(hWndRadio4, BM_GETCHECK, 0, 0 == BST_CHECKED))
                        g_LogType = 3;
                    EndDialog(hDlg, LOWORD(wParam));
                }
                break;
                
                case IDCANCEL:
                    EndDialog(hDlg, LOWORD(wParam));
                    return TRUE;
                    break;
            }
        }
        break;
    }
    return FALSE;
}

LRESULT CALLBACK DisplayBarCodeInfoProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            memset(szBarCodeText, 0, sizeof(szBarCodeText));

            /* Start the extended information for reporting of barcodes */
            BOOL bOk = DTWAIN_InitExtImageInfo(g_CurrentSource);
            if ( !bOk )
            {
                /* No Extended Info, or there was an error */
                EndDialog(hDlg, 0);
                return FALSE;
            }

            HWND hWndBarCodes = GetDlgItem(hDlg, IDC_edBarCodes);
            char* pOrigText = szBarCodeText;
            DTWAIN_ARRAY aText;
            DTWAIN_ARRAY aType;
            DTWAIN_ARRAY aCount;

            /* Get the bar code count */
            bOk = DTWAIN_GetExtImageInfoData(g_CurrentSource, TWEI_BARCODECOUNT, &aCount);

            if (bOk && aCount && DTWAIN_ArrayGetCount(aCount) > 0)
            {
                LONG barCount;
                DTWAIN_ArrayGetAtLong(aCount, 0, &barCount);
                DTWAIN_ArrayDestroy(aCount);
                if (barCount == 0)
                {
                    EndDialog(hDlg, 0);

                    /* Release the memory */
                    DTWAIN_FreeExtImageInfo(g_CurrentSource);
                    return FALSE;
                }
                int nChars = wsprintfA(pOrigText, "Bar Code Count: %d\r\n\r\n", barCount);
                pOrigText += nChars;

                /* Get the bar code text, and the bar code type */
                bOk = DTWAIN_GetExtImageInfoData(g_CurrentSource, TWEI_BARCODETEXT, &aText);
                bOk = DTWAIN_GetExtImageInfoData(g_CurrentSource, TWEI_BARCODETYPE, &aType);
                LONG totalChars = 0;
                if (bOk)
                {
                    LONG i;
                    for (i = 0; i < barCount; ++i)
                    {
                        nChars = wsprintfA(pOrigText, "Bar Code %d:\r\n", i + 1);
                        pOrigText += nChars;
                        totalChars += nChars;
                        if (totalChars >= 100000)
                            break;

                        /* Get the bar code text associated with bar code i + 1 */
                        const char *oneString = DTWAIN_ArrayGetAtANSIStringPtr(aText, i);
                        nChars = wsprintfA(pOrigText, "     Text: %s\r\n", oneString);
                        pOrigText += nChars;
                        totalChars += nChars;
                        if (totalChars >= 100000)
                            break;
                        LONG nType = 0;
                        DTWAIN_ArrayGetAtLong(aType, i, &nType);
                        char szType[100];

                        /* Translate the bar code type to a string defined by the TWAIN specification*/
                        DTWAIN_GetTwainNameFromConstantExA(DTWAIN_CONSTANT_TWBT, nType, szType, 100);
                        nChars = wsprintfA(pOrigText, "     Type: %s\r\n\r\n", szType);
                        totalChars += nChars;
                        if (totalChars >= 100000)
                            break;
                        pOrigText += nChars;
                    }

                    /* Set the edit control to the text */
                    SendMessageA(hWndBarCodes, WM_SETTEXT, 0, (LPARAM)szBarCodeText);
                }
                DTWAIN_ArrayDestroy(aText);
                DTWAIN_ArrayDestroy(aType);
            }
            else
            {
                MessageBoxA(NULL, "No Barcodes available.  Source does not support TWEI_BARCODECOUNT or barcode count is 0.", "Barcode error", MB_OK);
                EndDialog(hDlg, 0);
                DTWAIN_FreeExtImageInfo(g_CurrentSource);
                return FALSE;
            }
            DTWAIN_FreeExtImageInfo(g_CurrentSource);
            return TRUE;
        }
        break;
        case WM_COMMAND:
        {
            int nControl = LOWORD(wParam);
            int nNotification = HIWORD(wParam);

            switch (nControl)
            {
                /* Quit the dialog */
            case IDOK:
            case IDCANCEL:
            {
                EndDialog(hDlg, 1);
            }
            break;
            }
        }
        break;
    }
    return FALSE;
}

LRESULT CALLBACK DisplayCustomSelectProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            DTWAIN_ARRAY SourceArray = 0;
            DTWAIN_SOURCE CurSource;
            LONG lCount;
            LONG i;
            TCHAR szName[256];

            HWND hWndList = GetDlgItem(hDlg, IDC_lstSources);
            HWND hWndEdit = GetDlgItem(hDlg, IDC_edNumSources);
            DTWAIN_EnumSources( &SourceArray );
            lCount = DTWAIN_ArrayGetCount( SourceArray );
            if ( lCount <= 0 )
            {
                EndDialog(hDlg, 0);
                return FALSE;
            }

            wsprintf(szName, _T("%d"), (int)lCount);
            SetWindowText(hWndEdit, szName);

            for ( i = 0; i < lCount; i++ )
            {
                DTWAIN_ArrayGetAt( SourceArray, i, &CurSource);
                DTWAIN_GetSourceProductName(CurSource, szName, 255);
                SendMessage(hWndList, LB_ADDSTRING, 0, (LPARAM)szName);
            }

            SendMessage( hWndList, LB_SETCURSEL, 0, 0 );

            DTWAIN_ArrayDestroy( SourceArray );

            return TRUE;
        }

        case WM_COMMAND:
        {
            int nControl = LOWORD(wParam);
            int nNotification = HIWORD(wParam);

            switch( nControl )
            {
                /* Quit the dialog */
                case IDOK:
                {
                    TCHAR szBuf[256];
                    DTWAIN_SOURCE tempSource;

                    HWND hWndList = GetDlgItem(hDlg, IDC_lstSources);
                    LRESULT nCurSel = SendMessage(hWndList, LB_GETCURSEL, 0, 0);
                    SendMessage(hWndList, LB_GETTEXT, nCurSel, (LPARAM)szBuf);
                    tempSource = DTWAIN_SelectSourceByName( szBuf );
                    if ( tempSource )
                    {
                        g_NamedSource = tempSource;
                        EndDialog(hDlg, 1);
                    }
                    else
                    {
                        MessageBox(g_hWnd, _T("Could not select Source"), _T("Error"), MB_ICONSTOP);
                    }
                }
                break;

                case IDCANCEL:
                    EndDialog(hDlg, LOWORD(wParam));
                    return TRUE;
                break;
            }
        }
        break;
    }
    return FALSE;
}

LRESULT CALLBACK DisplayAcquireSettingsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    static HWND hWndFeeder;
    static HWND hWndDuplex;
    switch (message)
    {
        case WM_INITDIALOG:
        {
            hWndFeeder = GetDlgItem(hDlg, IDC_chkUseFeeder);
            hWndDuplex = GetDlgItem(hDlg, IDC_chkUseDuplex);

            EnableWindow(hWndFeeder, (DTWAIN_IsFeederSupported(g_CurrentSource) == TRUE));
            EnableWindow(hWndDuplex, (DTWAIN_IsDuplexSupported(g_CurrentSource) == TRUE));
            return TRUE;
        }
        break;

        case WM_COMMAND:
        {
            int nControl = LOWORD(wParam);
            int nNotification = HIWORD(wParam);

            switch( nControl )
            {
                /* Quit the dialog */
                case IDOK:
                {
                    LRESULT chkfeeder = SendMessage(hWndFeeder, BM_GETCHECK, 0, 0);
                    LRESULT chkDuplex = SendMessage(hWndDuplex, BM_GETCHECK, 0, 0);
                    DTWAIN_EnableFeeder(g_CurrentSource, chkfeeder?TRUE:FALSE);
                    DTWAIN_EnableDuplex(g_CurrentSource, chkDuplex?TRUE:FALSE);
                }
                case IDCANCEL:
                     EndDialog(hDlg, 1);
                break;

            }
        }
        break;
    }
    return FALSE;
}


// Message handler for about box.
LRESULT CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            char szBuf[1000];
            HWND hWndEdit = GetDlgItem(hDlg, IDC_edCopyright);
            DTWAIN_GetShortVersionStringA(szBuf, 100);
            SendMessageA(hDlg, WM_SETTEXT, 0, (LPARAM)szBuf);
            DTWAIN_GetVersionInfoA(szBuf, 1000);
            /* Edit controls need \r\n and not \n new lines. */
            HANDLE h = DTWAIN_ConvertToAPIStringA(szBuf);
            if (h)
            {
                LPCSTR pData = GlobalLock(h);
                SetWindowTextA(hWndEdit, pData);
                GlobalUnlock(h);
                GlobalFree(h);
            }
            return TRUE;
        }

        case WM_COMMAND:
            if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
            {
                EndDialog(hDlg, LOWORD(wParam));
                return TRUE;
            }
            break;
    }
    return FALSE;
}

BOOL IsAllSpace(LPCTSTR p)
{
    size_t nLen = lstrlen(p);
    size_t i;
    for ( i = 0; i < nLen; i++ )
    {
        if ( !_istspace((TCHAR)*p))
            return FALSE;
    }
    return TRUE;
}

void WaitLoop()
{
    if (DTWAIN_GetTwainMode() != DTWAIN_MODELESS)
        return;

    // When in DTWAIN_MODELESS mode, this application is responsible for the TWAIN loop
    // during when the source is enabled and acquiring images
    MSG msg;
    int val;
    while (((val = GetMessage (&msg, NULL, 0, 0)) != -1) // while there is a message
            && DTWAIN_IsSourceAcquiringEx(g_CurrentSource, FALSE)) // and the Source is acquiring
    {
        if ( val != 0 )
        {
            if ( !DTWAIN_IsTwainMsg(&msg) )  // send message to TWAIN if DTWAIN message
            {
                TranslateMessage (&msg);    // send message to app, not TWAIN
                DispatchMessage (&msg);
            }
        }
    }
}

LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData)
{
    static pdf_page_count = 1;
    switch (wParam)
    {
        case DTWAIN_TN_ACQUIRESTARTED:
            bPageOK = TRUE;
            pdf_page_count = 1;
        break;

        case DTWAIN_TN_TRANSFERDONE:
        {
            BOOL showBarCodes = GetToggleMenuState(IDM_SHOW_BARCODEINFO) && IsMenuItemEnabled(IDM_SHOW_BARCODEINFO);
            if (showBarCodes)
                DisplayBarCodeInfo();
        }
        break;

        /* See if we want to keep the DIB */
        case DTWAIN_TN_QUERYPAGEDISCARD:
        {
            LRESULT retVal;

            /* First let's see if menu option is checked */
            BOOL showPreview = GetToggleMenuState(IDM_SHOW_PREVIEW);
            if (!showPreview)
                return 1;

            /* Display the acquire DIB to the user and see if the image is to be kept */
            retVal = (DisplayOneDibPage(g_hInstance, DTWAIN_GetCurrentAcquiredImage(g_CurrentSource), IDD_dlgDib, g_hWnd) == IDCANCEL)?0:1;
            return retVal;   // return this back to DTWAIN
        }
        break;

        /* If this is a PDF file, this code will put a page stamp on this page */
        case DTWAIN_TN_FILEPAGESAVING:
        {
            if (g_FileType == DTWAIN_PDFMULTI)
            {
                /* Set the text to "Page x*, where x is the current page count */
                TCHAR text[100];
                wsprintf(text, _T("Page %d"), pdf_page_count);

                /* Change the text in the PDF Text Element that was added to the source */
                /* This will "automatically" set the PDF text to "Page x" for each page */
                DTWAIN_SetPDFTextElementString(g_PDFTextElement, text, DTWAIN_PDFTEXTELEMENT_TEXT);

                /* Increment the page count*/
                ++pdf_page_count;
            }
            return 1;
        }

        case DTWAIN_TN_FILEPAGESAVEERROR:
            MessageBox(NULL, _T("Could not save image page.\nPlease try acquiring image using a different color")
                             _T(" type selection, or check to make sure the file is not already opened"), _T("Page save error"),
                             MB_ICONSTOP);
            bPageOK = FALSE;
            return 1;
        break;

        case DTWAIN_TN_FILESAVEERROR:
            MessageBox(NULL, _T("Could not save image file.\nPlease check to make sure the file is not already opened"), _T("File Save Error"),
                             MB_ICONSTOP);
            return 1;
        break;

        case DTWAIN_TN_BLANKPAGEDISCARDED1:
            MessageBox(NULL, _T("Blank page was discarded"), _T("Blank page alert!"), MB_ICONSTOP);
        break;

        case DTWAIN_TN_BLANKPAGEDISCARDED2:
            MessageBox(NULL, _T("Blank page was discarded"), _T("Blank page alert!"), MB_ICONSTOP);
        break;

        case DTWAIN_TN_FILESAVEOK:
            bPageOK = TRUE;
            return 1;

        case DTWAIN_TN_PAGECONTINUE:
            if ( !bPageOK ) 
            {
                int nAnswer;
                nAnswer = MessageBox(NULL, _T("An error occurred saving page.  Continue scanning?"), _T("Error condition"), MB_YESNO);
                if (nAnswer == IDNO)
                    return FALSE;
                bPageOK = TRUE;
            }
    }
    return 1;
}

void EnableAllMenuItems(BOOL bEnable)
{
    const int numItems = sizeof(g_AllMenuItems) / sizeof(g_AllMenuItems[0]);
    int i = 0;
    UINT nOptions;
    if (!bEnable)
        nOptions = MF_BYCOMMAND | MF_GRAYED;
    else
        nOptions = MF_BYCOMMAND | MF_ENABLED;
    EnableMenuItem(g_Menu, IDC_DTWDEMO, nOptions);
    for (i = 0; i < numItems; ++i)
        EnableMenuItem(g_Menu, g_AllMenuItems[i], nOptions);
    EnableBarcodeMenuItem(g_CurrentSource);
    EnableFileXFerMenuItems(g_CurrentSource, bEnable);
    EnableMenuItem(g_Menu, IDM_ACQUIRETEST_USEGETMESSAGE, nOptions);
}

void DisableFileXFerSubItems()
{
    // First, disable all the menu items here
    HMENU mainMenu = GetMenu(g_hWnd);
    HMENU hSubMenu = GetSubMenu(mainMenu, 1);
    HMENU hSubMenu2 = GetSubMenu(hSubMenu, 3);
    UINT i;
    for (i = nFirstAcquireSourceID; i <= nLastAcquireSourceID; ++i)
        EnableMenuItem(hSubMenu2, i, MF_BYCOMMAND | MF_GRAYED);
}

void EnableBarcodeMenuItem(DTWAIN_SOURCE source)
{
	EnableMenuItem(g_Menu, IDM_SHOW_BARCODEINFO, MF_BYCOMMAND | MF_GRAYED);
	if (DTWAIN_IsExtImageInfoSupported(source))
    {
        if (DTWAIN_IsBarcodeSupported(source, DTWAIN_ANYSUPPORT))
            EnableMenuItem(g_Menu, IDM_SHOW_BARCODEINFO, MF_BYCOMMAND | MF_ENABLED);
    }
}

void EnableFileXFerMenuItems(DTWAIN_SOURCE source, BOOL bEnable)
{
	HMENU mainMenu = GetMenu(g_hWnd);
	HMENU hSubMenu = GetSubMenu(mainMenu, 1);
	HMENU hSubMenu2 = GetSubMenu(hSubMenu, 3);

	if (bEnable)
	{
		EnableMenuItem(hSubMenu, 2, MF_BYPOSITION | MF_ENABLED);
		EnableMenuItem(hSubMenu, 3, MF_BYPOSITION | MF_ENABLED);
	}
	else
	{
		EnableMenuItem(hSubMenu, 2, MF_BYPOSITION | MF_GRAYED);
		EnableMenuItem(hSubMenu, 3, MF_BYPOSITION | MF_GRAYED);
	}

    if (source && bEnable)
    {
        // Now enable the Source mode file xfers by querying the Source as to 
        // what is available
        if (!DTWAIN_IsFileXferSupported(source, DTWAIN_ANYSUPPORT))
        {
            EnableMenuItem(hSubMenu, 3, MF_BYCOMMAND | MF_GRAYED);
        }
        else
        {
            DTWAIN_ARRAY arrFileTypes = NULL;
            LONG nCount = 0;
            LONG i = 0;
            LONG fileType;

            // First, disable all the submenu items here
            DisableFileXFerSubItems();

            // Now enable the subitems of the file xfer support
            arrFileTypes = DTWAIN_EnumFileXferFormatsEx(source);
            nCount = DTWAIN_ArrayGetCount(arrFileTypes);
            for (i = 0; i < nCount; ++i)
            {
                UINT nFound = 0;
                UINT curId = 0;
                DTWAIN_ArrayGetAtLong(arrFileTypes, i, &fileType);
                for (nFound = nFirstAcquireSourceID; nFound <= nLastAcquireSourceID; ++nFound, ++curId)
                {
                    if (g_allDTWAINFileTypes[curId].dtwainType == fileType)
                    {
                        EnableMenuItem(hSubMenu2, curId, MF_BYPOSITION | MF_ENABLED);
                        break;
                    }
                }
            }
            DTWAIN_ArrayDestroy(arrFileTypes);
        }
    }
}