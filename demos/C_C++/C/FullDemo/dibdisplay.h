#ifndef DIBDISPLAY_H
#define DIBDISPLAY_H

#include "dtwain.h"

#ifdef __cplusplus
extern "C" {
#endif
    void DisplayDibPages(HINSTANCE hInstance, DTWAIN_ARRAY AcquireArray, UINT resID, HWND wndHandle);
    void RetrieveAndDisplayDibs(HINSTANCE hInstance, DTWAIN_ARRAY AcquireArray, UINT resID, HWND wndHandle);
#ifdef __cplusplus
    }
#endif

#endif