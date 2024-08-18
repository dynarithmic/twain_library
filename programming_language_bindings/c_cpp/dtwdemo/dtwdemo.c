// Example1.c : Acquires an image from a TWAIN Source
//
#include <windows.h>
#include "dtwdemo.h"
#include "dtwain.h"
#include <ctype.h>
#include "dibdisplay.h"


#include <tchar.h>
#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
TCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];            // The title bar text

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
DTWAIN_SOURCE DisplayGetNameDlg();
DTWAIN_SOURCE DisplayCustomDlg();
void DisplaySourceProps();
void SetCaptionToSourceName();
void AcquireNative();
void AcquireBuffered();
void AcquireFile(BOOL bUseSource);
BOOL IsAllSpace(LPCTSTR p);
void ToggleCheckedItem(UINT resId);
BOOL GetToggleMenuState(UINT resID);
BOOL IsTypeAvailable(LONG filetype);
void DisplayLoggingOptions();

BOOL bPageOK;
LONG nPageCount=0;
LONG nMajorVer, nMinorVer, nDTwainType;

void WaitLoop();
extern DWORD DIBHeight(LPSTR);

// if the following line doesn't compile, please change LONG_PTR to LONG
LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData);

// Dialog functions
LRESULT CALLBACK EnterSourceNameProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayCustomSelectProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplaySourcePropsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
//LRESULT CALLBACK DisplayDIBProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayAcquireSettingsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayFileTypesProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK DisplayLoggingProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
INT_PTR DisplayOneDibPage(HINSTANCE hInstance, HANDLE hDib, UINT resID, HWND wndHandle);


// File types structure
typedef struct
{
    LPCTSTR fType;
    LONG DTWAINType;
    LPCTSTR defName;
} AllTypes ;

AllTypes g_allTypes[] = {   {_T("BMP File"), DTWAIN_BMP, _T("test.bmp")},
                            {_T("Multi-page DCX File"),DTWAIN_DCX, _T("test.dcx")},
                            {_T("Enhanced Meta File (EMF)"),DTWAIN_EMF, _T("test.emf")},
                            {_T("GIF File"), DTWAIN_GIF, _T("test.gif")},
                            {_T("JPEG File"), DTWAIN_JPEG, _T("test.jpg")},
                            {_T("JPEG-2000 File"), DTWAIN_JPEG2000, _T("test.jp2")},
                            {_T("Adobe PDF File"), DTWAIN_PDFMULTI, _T("test.pdf")},
                            {_T("Postscript Level 1 File"), DTWAIN_POSTSCRIPT1MULTI, _T("test.ps")},
                            {_T("Postscript Level 2 File"), DTWAIN_POSTSCRIPT2MULTI, _T("test.ps")},
                            {_T("PNG File"), DTWAIN_PNG, _T("test.png")},
                            {_T("Adobe Paintshop (PSD) File"), DTWAIN_PSD, _T("test.psd")},
                            {_T("Text File"), DTWAIN_TEXTMULTI, _T("test.txt")},
                            {_T("TIFF (No compression)"), DTWAIN_TIFFNONEMULTI, _T("test.tif")},
                            {_T("TIFF (CCITT Group 3)"), DTWAIN_TIFFG3MULTI, _T("test.tif")},
                            {_T("TIFF (CCITT Group 4)"), DTWAIN_TIFFG4MULTI, _T("test.tif")},
                            {_T("TIFF (JPEG compression)"), DTWAIN_TIFFJPEGMULTI, _T("test.tif")},
                            {_T("TIFF (Packbits)"), DTWAIN_TIFFPACKBITSMULTI, _T("test.tif")},
                            {_T("TIFF (Flate compression)"), DTWAIN_TIFFDEFLATEMULTI, _T("test.tif")},
                            {_T("TIFF (LZW compression)"), DTWAIN_TIFFLZWMULTI, _T("test.tif")},
                            {_T("Targa (TGA) File"), DTWAIN_TGA, _T("test.tga")},
                            {_T("Windows Meta File (WMF)"), DTWAIN_WMF, _T("test.wmf")},
                            {_T("Windows ICON File (ICO)"), DTWAIN_ICO, _T("test.ico")},
                            {_T("Windows ICON File- Vista compatible (ICO)"), DTWAIN_ICO_VISTA, _T("test.ico")},
                            {_T("Wireless Bitmap File (WBMP)"), DTWAIN_WBMP, _T("test.wbmp")},
                            {_T("Google WebP (WEBP)"), DTWAIN_WEBP, _T("test.webp")},

                        };

AllTypes g_allTypesDemo[] = {   {_T("BMP File"), DTWAIN_BMP, _T("test.bmp")},
                            {_T("JPEG File"), DTWAIN_JPEG, _T("test.jpg")},
                            {_T("Adobe PDF File"), DTWAIN_PDFMULTI, _T("test.pdf")},
                            {_T("Text File"), DTWAIN_TEXTMULTI, _T("test.txt")},
                            {_T("TIFF (No compression)"), DTWAIN_TIFFNONEMULTI, _T("test.tif")},
                            {_T("TIFF (CCITT Group 4)"), DTWAIN_TIFFG4MULTI, _T("test.tif")},
                       };

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
    DTWAIN_CheckHandles(FALSE);
    /* Allow DTWAIN messages to be sent directly to our Window proc */
    DTWAIN_StartTwainSession(g_hWnd, NULL);
    // DTWAIN_SetTwainMode(DTWAIN_MODELESS);
    DTWAIN_EnableMsgNotify(TRUE);

    /* Also allow DTWAIN messages to be sent to our callback */
    DTWAIN_SetCallback(TwainCallbackProc,0);

    /* Call function to determine the DTWAIN version */
    DTWAIN_GetVersion(&nMajorVer, &nMinorVer, &nDTwainType);

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
    int wmId, wmEvent;
    TCHAR szHello[MAX_LOADSTRING];
    LoadString(hInst, IDS_HELLO, szHello, MAX_LOADSTRING);

    switch (message)
    {

        case WM_CREATE:
            g_Menu = GetMenu(hWnd);
            EnableSourceItems(FALSE);
            CheckMenuItem(g_Menu, IDM_USE_SOURCE_UI, MF_BYCOMMAND | MF_CHECKED);
			CheckMenuItem(g_Menu, IDM_SHOW_PREVIEW, MF_BYCOMMAND | MF_CHECKED);
        break;

        case WM_COMMAND:
            wmId    = LOWORD(wParam);
            wmEvent = HIWORD(wParam);
            // Parse the menu selections:
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
                        SetCaptionToSourceName();
                    }
                break;

                case IDM_ACQUIRE_NATIVE:
                    AcquireNative();
                break;

                case IDM_ACQUIRE_BUFFERED:
                    AcquireBuffered();
                break;

                case IDM_ACQUIRE_FILE_DTWAIN:
                    AcquireFile(FALSE);
                break;

                case IDM_ACQUIRE_FILE_SOURCE:
                    AcquireFile(TRUE);
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

                case IDM_LOGGING_OPTIONS:
                    DisplayLoggingOptions();
                break;

                case IDM_EXIT:
                   DestroyWindow(hWnd);
                   break;
                default:
                   return DefWindowProc(hWnd, message, wParam, lParam);
            }
            break;
        case WM_DESTROY:
            PostQuitMessage(0);
            break;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
   }
   return 0;
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

    switch (nWhich)
    {
        case IDM_SELECT_SOURCE:
            tempSource = DTWAIN_SelectSource();
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

    if ( tempSource )
    {
        if ( DTWAIN_OpenSource(tempSource) )
        {
            g_CurrentSource = tempSource;
            EnableSourceItems(TRUE);
            if ( !DTWAIN_IsFileXferSupported(tempSource, DTWAIN_ANYSUPPORT))
                EnableMenuItem(g_Menu, IDM_ACQUIRE_FILE_SOURCE, MF_BYCOMMAND | MF_GRAYED);
            SetCaptionToSourceName();
            DTWAIN_EnableFeeder(tempSource, TRUE);
        }
        else
            MessageBox(g_hWnd, _T("Error Opening Source"), _T("TWAIN Error"), MB_ICONSTOP);

    }
    else
        MessageBox(g_hWnd, _T("Error Selecting Source"), _T("TWAIN Error"), MB_ICONSTOP);
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


void AcquireNative()
{
    LONG ErrStatus;
    g_AcquireArray = DTWAIN_CreateAcquisitionArray();
    /* Disable main window */
    DTWAIN_DisableAppWindow(g_hWnd, TRUE);

    /* Check if feeder or duplex is supported */
    if ( DTWAIN_IsFeederSupported(g_CurrentSource) || DTWAIN_IsDuplexSupported(g_CurrentSource))
        DialogBox(g_hInstance, (LPCTSTR)IDD_dlgSettings, g_hWnd, (DLGPROC)DisplayAcquireSettingsProc);

    /* Check if we want to discard blank pages */
    /* Set the threshold to 98% blank */
    DTWAIN_SetBlankPageDetection(g_CurrentSource, 98.0, DTWAIN_BP_AUTODISCARD_ANY, 
                                 GetToggleMenuState(IDM_DISCARD_BLANKS));

    if ( !DTWAIN_AcquireNativeEx(
                    g_CurrentSource,
                    DTWAIN_PT_DEFAULT, /* Use default */
                    DTWAIN_ACQUIREALL, /* Get all pages */
                    GetToggleMenuState(IDM_USE_SOURCE_UI),
                    FALSE,  /* Close Source when UI is closed */
                    g_AcquireArray,
                    &ErrStatus /* Error Status */
    ))
    {
        MessageBox(NULL, _T("Acquisition failed"), _T("TWAIN Error"), MB_ICONSTOP);
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

void AcquireBuffered()
{
    LONG ErrStatus;
    /* Check if feeder or duplex is supported */
    if ( DTWAIN_IsFeederSupported(g_CurrentSource) || DTWAIN_IsDuplexSupported(g_CurrentSource))
        DialogBox(g_hInstance, (LPCTSTR)IDD_dlgSettings, g_hWnd, (DLGPROC)DisplayAcquireSettingsProc);

    /* Disable main window */
    DTWAIN_DisableAppWindow(g_hWnd, TRUE);

    g_AcquireArray = DTWAIN_CreateAcquisitionArray();

    if ( !DTWAIN_AcquireBufferedEx(
                    g_CurrentSource,
                    DTWAIN_PT_DEFAULT, /* Use default */
                    DTWAIN_ACQUIREALL, /* Get all pages */
                    GetToggleMenuState(IDM_USE_SOURCE_UI),
                    TRUE,  /* Close Source when UI is closed */
                    g_AcquireArray,
                    &ErrStatus /* Error Status */
                    ))
    {
        MessageBox(NULL, _T("Acquisition failed"), _T("TWAIN Error"), MB_ICONSTOP);
        DTWAIN_DestroyAcquisitionArray(g_AcquireArray, FALSE);
        return;
    }
    WaitLoop();
    if ( DTWAIN_ArrayGetCount(g_AcquireArray) == 0 )
    {
        MessageBox(g_hWnd, _T("No Images Acquired"), _T(""), MB_ICONSTOP);
        return;
    }

    RetrieveAndDisplayDibs(g_hInstance, g_AcquireArray, IDD_dlgDib, g_hWnd);
    DTWAIN_DestroyAcquisitionArray( g_AcquireArray, FALSE );
}


void AcquireFile(BOOL bUseSource)
{
    LONG ErrStatus;
    LONG FileFlags = DTWAIN_USELONGNAME;
    LONG FileType;
    BOOL bError;
    DTWAIN_ARRAY AFileNames = 0;
    BOOL UseUI;

    if ( bUseSource )
    {
        /* User wants to use the internal Source
           mode transfer.  Not all Sources
           have the ability to do this */
        if ( !DTWAIN_IsFileXferSupported(g_CurrentSource, DTWAIN_ANYSUPPORT) )
        {
            MessageBox(g_hWnd, _T("Sorry.  The selected driver does not support file transfers"), _T("Information"), MB_OK);
            return;
        }

        if ( !DTWAIN_IsFileXferSupported(g_CurrentSource, DTWAIN_FF_BMP) )
        {
            MessageBox(g_hWnd,_T("Sorry.  This demonstration program only supports BMP file transfers.\n")
                              _T("However, the selected driver does support other file formats.\n")
                              _T("The DTWAIN library can obtain these supported types with the appropriate\n")
                              _T("call to DTWAIN_AcquireFile or DTWAIN_AcquireFileEx."), _T("Information"), MB_OK);
            return;
        }

        FileFlags |= DTWAIN_USESOURCEMODE;
        FileType = DTWAIN_FF_BMP;
        _tcscpy(g_FileName, _T(".\\IMAGE.TMP"));
        MessageBox(g_hWnd, _T("The name of the image file that will be saved is IMAGE.TMP\n")
                          _T("The format of the file will be the one chosen from your ")
                          _T("device user interface"), _T("Information"), MB_OK);
    }
    else
    {
        /* User wants to use DTWAIN File Mode instead of Source mode */
        /* All Sources can use this mode */
        DialogBox(g_hInstance, (LPCTSTR)IDD_dlgFileType, g_hWnd, (DLGPROC)DisplayFileTypesProc);
        FileFlags |= DTWAIN_USEBUFFERED;
        FileType = g_FileType;

        /* This is just one of many options that can be set
          for DTWAIN / PDF support */
        if ( g_FileType == DTWAIN_PDFMULTI )
            DTWAIN_SetPDFPageSize(g_CurrentSource, DTWAIN_FS_USLETTER, 0, 0);
    }

    /* Check if TEXT type is acquired */
    if ( FileType == DTWAIN_TEXTMULTI )
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
    bError = DTWAIN_AcquireFileEx(g_CurrentSource,
                                  AFileNames,
                                  FileType,
                                  FileFlags,
                                  DTWAIN_PT_DEFAULT, /* Use default */
                                  DTWAIN_ACQUIREALL, /* Get all pages */
                                  UseUI,
                                  TRUE,  /* Close Source when UI is closed */
                                  &ErrStatus /* Error Status */
                                  );
    WaitLoop();
    EnableWindow(g_hWnd, TRUE);

    /* Reopen source since we closed it after the acquisition
       (to be safe) */
    DTWAIN_ArrayDestroy( AFileNames );
    DTWAIN_OpenSource( g_CurrentSource );
    if ( !bError || nPageCount == 0 || !bPageOK )
    {
        MessageBox(g_hWnd, _T("No Images Acquired"), _T(""), MB_ICONSTOP);
        return;
    }
	else
	{
		if (_taccess(g_FileName, 0) == 0)
		{
			MessageBox(g_hWnd, _T("Images Acquired"), _T(""), MB_ICONSTOP);
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
    LONG LogFlags = DTWAIN_LOG_CALLSTACK | DTWAIN_LOG_LOWLEVELTWAIN | DTWAIN_LOG_DECODE_TWEVENT | DTWAIN_LOG_DECODE_TWMEMREF | DTWAIN_LOG_ISTWAINMSG;
    if ( DialogBox(g_hInstance, (LPCTSTR)IDD_dlgDebug, g_hWnd, (DLGPROC)DisplayLoggingProc) == IDOK )
    {
        switch (g_LogType)
        {
            case 0:
                DTWAIN_SetTwainLog(0,_T(""));
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


/* Dialog box to display DIB */
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

LRESULT CALLBACK DisplaySourcePropsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
        {
            TCHAR szBuf[256];
            LONG nMajor, nMinor;
            DTWAIN_ARRAY CapArray = 0;
            LONG nCapCount;
            LONG nIndex;
            LONG nCapValue;

            HWND hWndName =     GetDlgItem(hDlg,  IDC_edProductName);
            HWND hWndFamily =   GetDlgItem(hDlg,  IDC_edFamilyName);
            HWND hWndManu =     GetDlgItem(hDlg,  IDC_edManufacturer);
            HWND hWndVerInfo =  GetDlgItem(hDlg,  IDC_edVersionInfo);
            HWND hWndVersion =  GetDlgItem(hDlg,  IDC_edVersion);
            HWND hWndCaps    =  GetDlgItem(hDlg,  IDC_lstCapabilities);
            HWND hWndNumCaps =  GetDlgItem(hDlg,  IDC_edTotalCaps);
            HWND hWndNumCustomCaps =  GetDlgItem(hDlg,  IDC_edCustomCaps);
            HWND hWndNumExtendedCaps =  GetDlgItem(hDlg,  IDC_edExtendedCaps);

            DTWAIN_GetSourceProductName(g_CurrentSource, szBuf, 255);
            SetWindowText(hWndName, szBuf);

            DTWAIN_GetSourceProductFamily(g_CurrentSource, szBuf, 255);
            SetWindowText(hWndFamily, szBuf);

            DTWAIN_GetSourceManufacturer(g_CurrentSource, szBuf, 255);
            SetWindowText(hWndManu, szBuf);

            DTWAIN_GetSourceVersionInfo(g_CurrentSource, szBuf, 255);
            SetWindowText(hWndVerInfo, szBuf);

            DTWAIN_GetSourceVersionNumber(g_CurrentSource, &nMajor, &nMinor);
            wsprintf(szBuf, _T("%d.%d"), nMajor, nMinor);
            SetWindowText(hWndVersion, szBuf);
            
            DTWAIN_EnumSupportedCaps(g_CurrentSource, &CapArray);
            nCapCount = DTWAIN_ArrayGetCount( CapArray );
            for ( nIndex = 0; nIndex < nCapCount; nIndex++ )
            {
                DTWAIN_ArrayGetAt( CapArray, nIndex, &nCapValue );
                DTWAIN_GetNameFromCap( nCapValue, szBuf, 255);
                SendMessage( hWndCaps, LB_ADDSTRING, 0, (LPARAM)szBuf);
            }

            wsprintf(szBuf, _T("%d"), nCapCount);
            SetWindowText(hWndNumCaps, szBuf);

            DTWAIN_ARRAY testArray;
            DTWAIN_EnumCustomCaps(g_CurrentSource, &testArray);
            wsprintf(szBuf, _T("%d"), (int)DTWAIN_ArrayGetCount(testArray));
            SetWindowText(hWndNumCustomCaps, szBuf);

            DTWAIN_EnumExtendedCaps(g_CurrentSource, &CapArray);
            wsprintf(szBuf, _T("%d"), (int)DTWAIN_ArrayGetCount(CapArray));
            SetWindowText(hWndNumExtendedCaps, szBuf);

            DTWAIN_ArrayDestroy( CapArray );
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
                case IDCANCEL:
                     EndDialog(hDlg, 1);
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


LRESULT CALLBACK DisplayFileTypesProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    static HWND hWndCombo;
    static HWND hWndEdit;
    switch (message)
    {
        case WM_INITDIALOG:
        {
            int i;
            int nTypes = sizeof(g_allTypes) / sizeof(g_allTypes[0]);
            hWndCombo = GetDlgItem(hDlg, IDC_cmbFileType);
            hWndEdit = GetDlgItem(hDlg, IDC_edFileName);
            for (i = 0; i < nTypes; ++i )
                SendMessage(hWndCombo, CB_ADDSTRING, 0, (LPARAM)g_allTypes[i].fType);
            SendMessage(hWndCombo, CB_SETCURSEL, 0, 0);
            SendMessage(hWndEdit, WM_SETTEXT, 0, (LPARAM)g_allTypes[0].defName);
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
                    /* Get the current selection */
                        SendMessage( hWndEdit, WM_GETTEXT, 256, (LPARAM)g_FileName);

                    if ( g_FileName[0] == 0 || IsAllSpace(g_FileName))
                    {
                        MessageBox(hDlg, _T("A file name must be entered"), _T("Error"), MB_ICONSTOP);
                    }
                    else
                    {
                        g_FileType = g_allTypes[(int)SendMessage(hWndCombo, CB_GETCURSEL, 0, 0)].DTWAINType;
                        if ( nDTwainType & DTWAIN_DEMODLL_VERSION ) 
                        {
                            if ( !IsTypeAvailable(g_FileType) )
                            {
                                MessageBox(hDlg, _T("Sorry.  This file type is not available in the demo version of DTWAIN"), _T("Error"), MB_ICONSTOP);
                                return TRUE;
                            }
                        }
                        EndDialog(hDlg, 1);
                    }
                }
                break;
                case IDCANCEL:
                     g_FileType = g_allTypes[(int)SendMessage(hWndCombo, CB_GETCURSEL, 0, 0)].DTWAINType;
                     EndDialog(hDlg, 1);
                break;
                case IDC_cmbFileType:
                {
                    if ( nNotification == CBN_SELCHANGE )
                    {
                        /* Set the default file name */
                        LRESULT nCurSel = SendMessage( hWndCombo, CB_GETCURSEL, 0, 0);
                        SendMessage(hWndEdit, WM_SETTEXT, 0, (LPARAM)g_allTypes[nCurSel].defName);
                    }
                }
                break;
            }
        }
        break;
    }
    return FALSE;
}

BOOL IsTypeAvailable(LONG lFileType)
{
    if ( nDTwainType & DTWAIN_DEMODLL_VERSION )
    {
       return ( lFileType == DTWAIN_BMP ||
           lFileType == DTWAIN_PDF ||
           lFileType == DTWAIN_PDFMULTI ||
           lFileType == DTWAIN_TIFFNONE ||
           lFileType == DTWAIN_TIFFNONEMULTI ||
           lFileType == DTWAIN_TIFFG4 ||
           lFileType == DTWAIN_TIFFG4MULTI ||
           lFileType == DTWAIN_TEXT ||
           lFileType == DTWAIN_TEXTMULTI );
    }
    return TRUE;
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
            DTWAIN_GetVersionCopyrightA(szBuf, 1000);
            SendMessageA(hWndEdit, WM_SETTEXT, 0, (LPARAM)szBuf);
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

// if the following line doesn't compile, please change "LONG_PTR" to LONG

INT_PTR g_nDiscardDibRetVal;

LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData)
{
	static pdf_page_count = 1;
	switch (wParam)
    {
        case DTWAIN_TN_ACQUIRESTARTED:
            bPageOK = TRUE;
            nPageCount = 0;
			pdf_page_count = 1;
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

		/* If this is a PDF file this code will put a page stamp on this page */
		case DTWAIN_TN_FILEPAGESAVING:
		{
			TCHAR text[100];
			wsprintf(text, _T("Page %d"), pdf_page_count); 
			++pdf_page_count;
			DTWAIN_AddPDFText(g_CurrentSource, text, 100, 100, _T("Helvetica"), 12, 
							  DTWAIN_MakeRGB(127, 127, 127), 0, 100.0, 0, 0.0, 0, 
				              DTWAIN_PDFTEXT_CURRENTPAGE);
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
            ++nPageCount;
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

