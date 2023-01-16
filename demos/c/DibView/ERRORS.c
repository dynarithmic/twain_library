/*************************************************************************

      File:  ERRORS.C

   Purpose:  Contains the error message box handler.

 Functions:  DIBError()

  Comments:  Should use a string table here...We're unnecessarily
             eating up DS, and make it harder to localize for international
             markets...  Maybe next time...

   History:   Date     Reason

             6/1/91    Created

*************************************************************************/


#include "master.h"
#include <tchar.h>

static TCHAR *szErrors[] = {_T("Not a DIB file!"),
                           _T("Couldn't allocate memory!"),
                           _T("Error reading file!"),
                           _T("Error locking memory!"),
                           _T("Error opening file!"),
                           _T("Error creating palette!"),
                           _T("Error getting a DC!"),
                           _T("Error creating MDI Child!"),
                           _T("Error creating Device Dependent Bitmap"),
                           _T("StretchBlt() failed!"),
                           _T("StretchDIBits() failed!"),
                           _T("Paint requires both DDB and DIB!"),
                           _T("SetDIBitsToDevice() failed!"),
                           _T("Printer: StartDoc failed!"),
                           _T("Printing: GetModuleHandle() couldn't find GDI!"),
                           _T("Printer: SetAbortProc failed!"),
                           _T("Printer: StartPage failed!"),
                           _T("Printer: NEWFRAME failed!"),
                           _T("Printer: EndPage failed!"),
                           _T("Printer: EndDoc failed!"),
                           _T("Only one DIB can be animated at a time!"),
                           _T("No timers available for animation!"),
                           _T("Can't copy to clipboard -- no current DIB!"),
                           _T("Clipboard is busy -- operation aborted!"),
                           _T("Can't paste -- no DIBs or DDBs in clipboard!"),
                           _T("SetDIBits() failed!"),
                           _T("File Not Found!"),
                           _T("Error writing DIB file!")
                          };

void DIBError (int ErrNo)
{
   if ((ErrNo < ERR_MIN) || (ErrNo >= ERR_MAX))
   {
      MessageBox (GetFocus (), _T("Undefined Error!"), NULL, MB_OK | MB_ICONHAND);
   }
   else
   {
      MessageBox (GetFocus (), szErrors[ErrNo], NULL, MB_OK | MB_ICONHAND);
   }
}
