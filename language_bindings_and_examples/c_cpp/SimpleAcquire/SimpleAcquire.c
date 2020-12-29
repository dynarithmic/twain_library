// Example1.cpp : Acquires an image from a TWAIN Source
//
#include <dtwain.h>
#include "SimpleAcquire.h"
#include "dibfunc.h"
#include <tchar.h>
#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
TCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];            // The title bar text
HWND hWndGlobal;                                // main window handle
RECT rectFrame;                                 // DIB display frame rect
RECT rectDib;                                   // DIB rect

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
void RetrieveDib();
LRESULT CALLBACK DisplayDIBProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
void DisplayDibPages();
void EnablePageButtons(HWND hPrev, HWND hNext, HWND hWndCurPage,
                       HWND hWndNumPages, int nDib, int nCurrentAcquisition);

int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       nCmdShow)
{
    // TODO: Place code here.
    MSG msg;
    HACCEL hAccelTable;

    // Initialize global strings
    LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInstance, IDC_EXAMPLE1, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // Perform application initialization:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    hAccelTable = LoadAccelerators(hInstance, (LPCTSTR)IDC_EXAMPLE1);


    DTWAIN_SysInitialize();
    // Main message loop:
    while (GetMessage(&msg, NULL, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }
    DTWAIN_SysDestroy();

    return msg.wParam;
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
    wcex.hIcon          = LoadIcon(hInstance, (LPCTSTR)IDI_EXAMPLE1);
    wcex.hCursor        = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = (LPCTSTR)IDC_EXAMPLE1;
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, (LPCTSTR)IDI_SMALL);

    return RegisterClassEx(&wcex);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   HWND hWnd;

   hInst = hInstance; // Store instance handle in our global variable

   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

   if (!hWnd)
   {
      return FALSE;
   }

   ShowWindow(hWnd, nCmdShow);
   UpdateWindow(hWnd);

   hWndGlobal = hWnd;
   return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    int wmId, wmEvent;
    TCHAR szHello[MAX_LOADSTRING];
    LoadString(hInst, IDS_HELLO, szHello, MAX_LOADSTRING);

    switch (message)
    {
        case WM_COMMAND:
            wmId    = LOWORD(wParam);
            wmEvent = HIWORD(wParam);
            // Parse the menu selections:
            switch (wmId)
            {
                case IDM_ABOUT:
                   DialogBox(hInst, (LPCTSTR)IDD_ABOUTBOX, hWnd, (DLGPROC)About);
                   break;
                case IDM_RETRIEVE_DIB:
                    RetrieveDib();
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

        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
   }
   return 0;
}

void RetrieveAllDibs( );

DTWAIN_ARRAY AcquireArray;
char szBuf[100];

void RetrieveDib()
{
    DTWAIN_SOURCE SelectedSource;
    LONG  ErrStatus;

    /* See if TWAIN exists on the system */
    /* Quit if TWAIN is not there! */
    /* This call is added so that we can return immediately
    if no TWAIN is found */
    if ( !DTWAIN_IsTwainAvailable( ) ) return;

    /* Initialize the DTWAIN DLL.  Quit if error! */
    /* DTWAIN_LIB is for DTWAIN static library builds only! */
#ifdef DTWAIN_LIB
    if ( !DTWAIN_SysInitializeLib( hInst )) return;
#else
    if ( !DTWAIN_SysInitialize( )) return;
#endif

    /* Select the Source using TWAIN dialog */
    SelectedSource = DTWAIN_SelectSource( );
    /* Only do this if a source is selected */
    if ( SelectedSource )
   {
        /* Disable the main window */
        EnableWindow(hWndGlobal, FALSE);

        AcquireArray = DTWAIN_AcquireNative(
                        SelectedSource, /* TWAIN Source */
                        DTWAIN_PT_DEFAULT, /* Use default */
                        DTWAIN_ACQUIREALL, /* Get all pages */
                        TRUE,  /* Show Source UI */
                        TRUE,  /* Close Source when UI is closed */
                        &ErrStatus /* Error Status */
                                          );
        /* Re-enable the main window */
        EnableWindow(hWndGlobal, TRUE);

        /* Check if acquisition worked */
        if ( AcquireArray == NULL )
        {
        /* Didn't work */
                MessageBox(NULL,
                          _T("TWAIN Acquisition failed!"),
                          _T("TWAIN Error"),
                           MB_ICONSTOP);
        } /* End 'if AcquireArray */
        else
        if ( DTWAIN_ArrayGetCount(AcquireArray) < 1 )
        {
        /* Didn't work */
                MessageBox(NULL,
                    _T("No images were acquired!"),
                    _T("TWAIN"),
                     MB_ICONSTOP);
        }
        else
        /* Call a function that retrieves all of the DIBs  */
            RetrieveAllDibs( );
    } /* End 'if SourceSelected... */
} /* End the function */


/* This function demonstrates the DTWAIN Image Retrieval functions */

void RetrieveAllDibs( )
{
    LONG Count;
    LONG DibCount;
    LONG Count2;
    HANDLE hDib;
    LONG numAcquisitions;
    /* Get the number of total acquisitions attempted */
    numAcquisitions = DTWAIN_ArrayGetCount(AcquireArray);

    /* Display the first acqquired page in a dialog */
    DisplayDibPages( AcquireArray );

    /* Loop for each acquisition attempted and free the DIBS*/
    /* Alternately, the DTWAIN_GetAcquiredImageArray function
    could have been used also to get the array of DIBs */
    for ( Count = 0; Count < numAcquisitions; ++Count )
    {
        DibCount = DTWAIN_GetNumAcquiredImages( AcquireArray, Count);
        /* Loop for each DIB in the acquisition */
        for (Count2 = 0; Count2 < DibCount; ++Count2 )
        {
             /* Retrieve the DIB */
            hDib = DTWAIN_GetAcquiredImage(AcquireArray, Count, Count2);
            if ( hDib )
            {
                /* Release the memory for the DIB
                (must be done by application!) */
                GlobalFree( hDib );
            }
         } /* End for Count2 */
    }   /* End for Count */
} /* End RetrieveAllDibs */

void DisplayDibPages()
{
    DialogBox(hInst, (LPCTSTR)IDD_DIALOG1, hWndGlobal, (DLGPROC)DisplayDIBProc);
}

/* Dialog box to display DIB */
LRESULT CALLBACK DisplayDIBProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    static HANDLE hDibLocal;
    static HWND hWndNext;
    static HWND hWndPrev;
    static HWND hWndCurPage;
    static HWND hWndNumPages;
    static HWND hWndAcquisition;
    static int nCurDib;
    static HWND Frame;
    static int nCurrentAcquisition;

    switch (message)
    {
        case WM_INITDIALOG:
        {
            /* Set up DIB */
            DWORD heightDib;
            DWORD widthDib;
            LPSTR LockedHandle;
            LONG nCount;
            LONG i;
            TCHAR sz[10];

            nCurDib = 0;

            /* Get handle of Next, Prev buttons */
            hWndNext = GetDlgItem(hDlg, IDC_btnNext);
            hWndPrev = GetDlgItem(hDlg, IDC_btnPrev);

            /* Get handle of page information fields */
            hWndCurPage = GetDlgItem(hDlg, IDC_edCurPage);
            hWndNumPages = GetDlgItem(hDlg, IDC_edNumPages);

            /* Get handle of Acquisition Combo */
            hWndAcquisition = GetDlgItem(hDlg, IDC_cmbAcquisition);

            /* Get handle of frame */
            Frame = GetDlgItem( hDlg, IDC_frmImage);

            /* Get Frame rectangle */
            GetClientRect(Frame, &rectFrame);
            MapWindowPoints(Frame, hDlg, (POINT *)&rectFrame, 2);
            InflateRect(&rectFrame, -1, -1);

            /* Get DIB Width and Height */
            hDibLocal = DTWAIN_GetAcquiredImage(AcquireArray, 0, nCurDib);
            if ( hDibLocal )
            {
                LockedHandle = (LPSTR)GlobalLock(hDibLocal);
                heightDib = DIBHeight(LockedHandle);
                widthDib  = DIBWidth(LockedHandle);
                GlobalUnlock(hDibLocal);

                /* Setup DIB Rectangle */
                SetRect(&rectDib,0,0,widthDib,heightDib);

                /* Enable/Disabe Next, Prev buttons */
                nCurrentAcquisition = 0;
                EnablePageButtons(hWndPrev, hWndNext, hWndCurPage, hWndNumPages, nCurDib,
                                  nCurrentAcquisition);

                /* Fill Acquisition Combo */
                nCount = DTWAIN_GetNumAcquisitions( AcquireArray );
                for (i = 1; i <= nCount; i++ )
                {
                    wsprintf(sz,_T("%d"), i);
                    SendMessage(hWndAcquisition, CB_ADDSTRING, 0, (LPARAM)sz);
                }
                SendMessage(hWndAcquisition, CB_SETCURSEL, 0, 0);
                return TRUE;
            }
            else
            {
                MessageBox(NULL,_T("Bitmap is NULL!\r\nPlease check if Source is configured properly"),
                                 _T("NULL DIB Handle"), MB_ICONSTOP);
                EndDialog(hDlg, FALSE);
            }
        }

        case WM_PAINT:
        {
            /* Paint the DIB to the frame */
            PAINTSTRUCT ps;
            HDC hDC = GetDC(hDlg);
            BeginPaint(hDlg, &ps);
            DIBPaint( hDC, &rectFrame, hDibLocal, &rectDib, NULL);
            EndPaint(hDlg,&ps);
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
                case IDCANCEL:
                    EndDialog(hDlg, LOWORD(wParam));
                    return TRUE;
                break;

                /* Go to next page/previous page within acquisition */
                case IDC_btnPrev:
                case IDC_btnNext:
                    /* Go to next/prev DIB */
                    nCurDib += (nControl == IDC_btnPrev)?-1:1;
                    hDibLocal = DTWAIN_GetAcquiredImage(AcquireArray, nCurrentAcquisition, nCurDib);
                    /* Redo page buttons */
                    EnablePageButtons(hWndPrev, hWndNext, hWndCurPage, hWndNumPages, nCurDib,
                                      nCurrentAcquisition);
                    /* Redisplay new DIB */
                    InvalidateRect(hDlg, NULL, TRUE);
                break;

                /* Go to another set of pages that were acquired */
                case IDC_cmbAcquisition:
                    if (nNotification == CBN_SELCHANGE)
                    {
                        /* Get the selection from message box */
                        int nCurSel = SendMessage(hWndAcquisition, CB_GETCURSEL, 0, 0);

                        /* Change only if different */
                        if ( nCurSel != nCurrentAcquisition )
                        {
                            /* Go to selected set of pages */
                            nCurrentAcquisition = nCurSel;
                            nCurDib = 0;

                            /* Get the DIB, Redo page buttons and display DIB */
                            hDibLocal = DTWAIN_GetAcquiredImage(AcquireArray, nCurrentAcquisition,
                                                                nCurDib);
                            EnablePageButtons(hWndPrev, hWndNext, hWndCurPage, hWndNumPages,
                                              nCurDib, nCurrentAcquisition);
                            InvalidateRect(hDlg, NULL, TRUE);
                        }
                    }
                break;
            }
        }
        break;
    }
    return FALSE;
}


void EnablePageButtons(HWND hPrev, HWND hNext, HWND hWndCurPage, HWND hWndNumPages,
                       int nDib, int nCurrentAcquisition)
{
    TCHAR szNum1[10];
    TCHAR szNum2[10];

    /* Get number of acquired images */
    LONG nCount = DTWAIN_GetNumAcquiredImages(AcquireArray, nCurrentAcquisition);

    /* Enable Disable Next/Prev buttons */
    EnableWindow(hNext,( nDib < nCount - 1));
    EnableWindow(hPrev, (nDib > 0));

    /* Update "Page x of x" display in dialog */
    wsprintf(szNum1, _T("%d"), nDib + 1);
    wsprintf(szNum2, _T("%d"), nCount);
    SetWindowText(hWndCurPage, szNum1);
    SetWindowText(hWndNumPages, szNum2);
}

// Mesage handler for about box.
LRESULT CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
                return TRUE;

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
