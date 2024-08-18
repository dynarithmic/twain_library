#include <windows.h>
#include "dibdisplay.h"
#include "dibdisplayres.h"
#include "..\dibview\dibfunc.h"
#include <tchar.h>

static LRESULT CALLBACK DisplayDIBProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

static void EnablePageButtons(HWND hPrev, HWND hNext, HWND hWndCurPage, HWND hWndNumPages,
                       HWND hPage, HWND hOf, HWND hBlank, int nDib, int nCurrentAcquisition, DTWAIN_ARRAY AcquireArray);

static int DisplayOne = 0;

void RetrieveAndDisplayDibs(HINSTANCE hInstance, DTWAIN_ARRAY AcquireArray, UINT resID, HWND wndHandle)
{
    LONG Count;
    LONG DibCount;
    LONG Count2;
    HANDLE hDib;
    LONG numAcquisitions;
    /* Get the number of total acquisitions attempted */
    numAcquisitions = DTWAIN_ArrayGetCount(AcquireArray);

    /* Display the acquired pages in a dialog */
    DisplayDibPages(hInstance, AcquireArray, resID, wndHandle);

    /* Get the number of DIBs (pages) that were
    scanned for each acquisition */

    /* Loop for each acquisition attempted */
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
                GlobalFree( hDib );
        }
    }
}


void DisplayDibPages(HINSTANCE hInstance, DTWAIN_ARRAY AcquireArray, UINT resID, HWND wndHandle)
{
    DialogBoxParam(hInstance, (LPCTSTR)resID, wndHandle, (DLGPROC)DisplayDIBProc, (LPARAM)AcquireArray);
}

INT_PTR DisplayOneDibPage(HINSTANCE hInstance, HANDLE hDib, UINT resID, HWND wndHandle)
{
	INT_PTR ret;
	DisplayOne = 1;
	ret = DialogBoxParam(hInstance, (LPCTSTR)resID, wndHandle, (DLGPROC)DisplayDIBProc, (LPARAM)hDib);
	DisplayOne = 0;
	return ret;
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
    static HWND hWndOf;
    static HWND hwndPage;
    static HWND hwndBlank;
	static HWND hwndOk;
	static HWND hwndCancel;
	static HWND hwndtxtAcquisition;
    static int nCurDib;
    static HWND Frame;
    static LRESULT nCurrentAcquisition;
    static RECT rectFrame;
    static RECT rectDib;
    static DTWAIN_ARRAY AcquireArray;

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

			if (DisplayOne)
				SetWindowText(hDlg, _T("Image Preview - Do you want to keep or discard image below?"));
            nCurDib = 0;
			if ( !DisplayOne )
				AcquireArray = (DTWAIN_ARRAY)lParam;

            /* Get handle of Next, Prev buttons */
            hWndNext = GetDlgItem(hDlg, IDC_btnNext);
            hWndPrev = GetDlgItem(hDlg, IDC_btnPrev);

            /* Get the handle of the "Page" and "Of" text */
            hWndOf = GetDlgItem(hDlg, IDC_txtOF);
            hwndPage = GetDlgItem(hDlg, IDC_txtPAGE);
            hwndBlank = GetDlgItem(hDlg, IDC_txtBLANKPAGE);


            /* Get handle of page information fields */
            hWndCurPage = GetDlgItem(hDlg, IDC_edCurPage);
            hWndNumPages = GetDlgItem(hDlg, IDC_edNumPages);
			hwndtxtAcquisition = GetDlgItem(hDlg, IDC_txtAcquisition);

            /* Get handle of Acquisition Combo */
            hWndAcquisition = GetDlgItem(hDlg, IDC_cmbAcquisition);

			hwndOk = GetDlgItem(hDlg, IDOK);
			hwndCancel = GetDlgItem(hDlg, IDCANCEL);

            /* Get handle of frame */
            Frame = GetDlgItem( hDlg, IDC_frmImage);

            /* Get Frame rectangle */
            GetClientRect(Frame, &rectFrame);
            MapWindowPoints(Frame, hDlg, (POINT *)&rectFrame, 2);
            InflateRect(&rectFrame, -1, -1);

            /* Get DIB Width and Height */
			if (!DisplayOne)
				hDibLocal = DTWAIN_GetAcquiredImage(AcquireArray, 0, nCurDib);
			else
				hDibLocal = (HANDLE)lParam;

            if ( hDibLocal )
            {
                LockedHandle = (LPSTR)GlobalLock(hDibLocal);
                heightDib = DIBHeight(LockedHandle);
                widthDib  = DIBWidth(LockedHandle);
                GlobalUnlock(hDibLocal);

                /* Setup DIB Rectangle */
                SetRect(&rectDib,0,0,widthDib,heightDib);
            }

            /* Enable/Disabe Next, Prev buttons */
            nCurrentAcquisition = 0;
            EnablePageButtons(hWndPrev, hWndNext, hWndCurPage, hWndNumPages, 
                hwndPage, hWndOf, hwndBlank, nCurDib,
                (int)nCurrentAcquisition, AcquireArray);

            /* Fill Acquisition Combo */
			if (!DisplayOne)
			{
				nCount = DTWAIN_GetNumAcquisitions(AcquireArray);
				for (i = 1; i <= nCount; i++)
				{
					wsprintf(sz, _T("%d"), i);
					SendMessage(hWndAcquisition, CB_ADDSTRING, 0, (LPARAM)sz);
				}
				SendMessage(hWndAcquisition, CB_SETCURSEL, 0, 0);
			}
			else
			{
				ShowWindow(hWndNext, SW_HIDE);
				ShowWindow(hWndPrev, SW_HIDE);
				ShowWindow(hWndOf, SW_HIDE);
				ShowWindow(hwndPage, SW_HIDE);
				ShowWindow(hwndBlank, SW_HIDE);
				ShowWindow(hWndCurPage, SW_HIDE);
				ShowWindow(hWndNumPages, SW_HIDE);
				ShowWindow(hWndAcquisition, SW_HIDE);
				ShowWindow(hwndtxtAcquisition, SW_HIDE);
				SetWindowText(hwndOk, _T("Keep"));
				SetWindowText(hwndCancel, _T("Discard"));
			}
   		    return TRUE;
        }

    case WM_PAINT:
        {
            /* Paint the DIB to the frame */
            PAINTSTRUCT ps;
            HDC hDC = GetDC(hDlg);
            BeginPaint(hDlg, &ps);
            if ( hDibLocal )
                DIBPaint( hDC, &rectFrame, hDibLocal, &rectDib, NULL);
            ReleaseDC(hDlg, hDC);
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
                hDibLocal = DTWAIN_GetAcquiredImage(AcquireArray, (LONG)nCurrentAcquisition, nCurDib);
                /* Redo page buttons */
                EnablePageButtons(hWndPrev, hWndNext, hWndCurPage, hWndNumPages, 
                    hwndPage, hWndOf, hwndBlank, nCurDib, (int)nCurrentAcquisition, AcquireArray);         
                /* Redisplay new DIB */
                InvalidateRect(hDlg, NULL, TRUE);
                break;

                /* Go to another set of pages that were acquired */
            case IDC_cmbAcquisition:
                if (nNotification == CBN_SELCHANGE)
                {
                    /* Get the selection from message box */
                    LRESULT nCurSel = SendMessage(hWndAcquisition, CB_GETCURSEL, 0, 0);

                    /* Change only if different */
                    if ( nCurSel != nCurrentAcquisition )
                    {
                        /* Go to selected set of pages */
                        nCurrentAcquisition = nCurSel;
                        nCurDib = 0;

                        /* Get the DIB, Redo page buttons and display DIB */
                        hDibLocal = DTWAIN_GetAcquiredImage(AcquireArray, (LONG)nCurrentAcquisition, nCurDib);
                        if ( hDibLocal )
                        {
                            DWORD heightDib;
                            DWORD widthDib;
                            LPSTR LockedHandle;
                            LockedHandle = (LPSTR)GlobalLock(hDibLocal);
                            heightDib = DIBHeight(LockedHandle);
                            widthDib  = DIBWidth(LockedHandle);
                            GlobalUnlock(hDibLocal);

                            /* Setup DIB Rectangle */
                            SetRect(&rectDib,0,0,widthDib,heightDib);
                        }
                        EnablePageButtons(hWndPrev, hWndNext, hWndCurPage, hWndNumPages,
                                          hwndPage, hWndOf, hwndBlank, nCurDib,
                                          (int)nCurrentAcquisition, AcquireArray);
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
                       HWND hPage, HWND hOf, HWND hBlank, int nDib, int nCurrentAcquisition,
                       DTWAIN_ARRAY AcquireArray)
{
    TCHAR szNum1[10];
    TCHAR szNum2[10];

    /* Get number of acquired images */
    LONG nCount = DTWAIN_GetNumAcquiredImages(AcquireArray, nCurrentAcquisition);

    /* Enable Disable Next/Prev buttons */
    EnableWindow(hNext,( nDib < nCount - 1));
    EnableWindow(hPrev, (nDib > 0));

    /* Update "Page x of x" display in dialog */

    /* First check if we really acquired any DIB's (blank ones could have been removed) */
    if ( nCount == 0 )
    {
        ShowWindow(hPage, SW_HIDE);
        ShowWindow(hOf, SW_HIDE);
        ShowWindow(hWndCurPage, SW_HIDE);
        ShowWindow(hWndNumPages, SW_HIDE);
        ShowWindow(hBlank, SW_SHOW);
    }
    else
    {
        ShowWindow(hPage, SW_SHOW);
        ShowWindow(hOf, SW_SHOW);
        ShowWindow(hBlank, SW_HIDE);
        ShowWindow(hWndCurPage, SW_SHOW);
        ShowWindow(hWndNumPages, SW_SHOW);
        wsprintf(szNum1, _T("%d"), nDib + 1);
        wsprintf(szNum2, _T("%d"), nCount);
        SetWindowText(hWndCurPage, szNum1);
        SetWindowText(hWndNumPages, szNum2);
    }
}
