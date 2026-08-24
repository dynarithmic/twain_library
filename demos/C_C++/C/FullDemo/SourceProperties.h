#ifndef SOURCEPROPERTIES_H
#define SOURCEPROPERTIES_H
#include <windows.h>

void DisplayTestCapDlg(HWND parent, const char* szCapName);
void DisplayBadCapDlg(HWND parent);

LRESULT CALLBACK DisplaySourcePropsProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

#endif
