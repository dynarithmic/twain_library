#include "Common.ch"
#include "dtwain32.ch"
#include "dll.ch"

PROCEDURE Main
  LOCAL TwainSource, nDLLHandle, TwainOK, status, fileOption

  fileOption := DTWAIN_USENATIVE + DTWAIN_USELONGNAME
  status := 0

  // Load the DTWAIN DLL
  nDLLHandle := DLLLoad( "DTWAIN32.DLL" )

  // Check if TWAIN is available
  TwainOK := DLLCall( nDLLHandle, DLL_STDCALL, "DTWAIN_IsTwainAvailable" )

  IF TwainOK == 1
       // Initialize DTWAIN
      DLLCall ( nDLLHandle, DLL_STDCALL, "DTWAIN_SysInitialize" )

      // Select a TWAIN Source
      TwainSource := DLLCall( nDLLHandle, DLL_STDCALL, "DTWAIN_SelectSource2", 0, ;
                              "Select Source", 0, 0, DTWAIN_DLG_CENTER_SCREEN )

      IF  TwainSource <> 0
            // Acquire a file
            DLLCall ( nDLLHandle, DLL_STDCALL, "DTWAIN_AcquireFile", TwainSource, ;
                      "Test.bmp", DTWAIN_BMP, fileOption, DTWAIN_PT_DEFAULT, 1, 1, 1, @status )
      ENDIF

      // Uninitialize DTWAIN
      DLLCall( nDLLHandle, DLL_STDCALL, "DTWAIN_SysDestroy" )
  ENDIF

   // Unload the DLL
   DLLUnload( nDLLHandle )
RETURN

