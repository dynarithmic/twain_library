#ifndef SOURCEPROPERTIES_H
#define SOURCEPROPERTIES_H
#include <windows.h>

void DisplayTestCapDlg(HWND parent, const char* szCapName);
LRESULT CALLBACK DisplaySourcePropsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

#endif