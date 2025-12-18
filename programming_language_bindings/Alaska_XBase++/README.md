Here is an XBase++ example using the  [dtwain32.ch](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/Alaska_XBase++) interface file that defines the DTWAIN constants.  

The program gives an example of selecting a TWAIN device installed on your system, displaying a list of the capabilities available to the device, and acquiring a BMP image.

Note that the example loads "DTWAIN32.DLL" in the call to `DLLLoad`.  If your application is a 32-bit Unicode application, you should load "DTWAIN32U.DLL".  

If your application is 64-bit, instead of "DTWAIN32.DLL", this would be "DTWAIN64U.DLL" or "DTWAIN64.DLL" (the first being Unicode, while the latter being ANSI):

```cpp
#include "Common.ch"
#include "dtwain32.ch"
#include "dll.ch"

PROCEDURE Main
  LOCAL TwainSource, nDLLHandle, TwainOK, status, fileOption
  LOCAL cBuffer := Space(260)
  LOCAL capArray := nil
  LOCAL capCount, i
  LOCAL capValue := 0
  LOCAL strLength := 0
  LOCAL numCapCount := 0

  fileOption := DTWAIN_USENATIVE + DTWAIN_USELONGNAME
  status := 0

  // Load the DTWAIN DLL
  nDLLHandle := DLLLoad( "DTWAIN32.DLL" ) // Or DTWAIN32U.DLL, DTWAIN64.DLL, DTWAIN64U.DLL, etc.

  // Check if TWAIN is available
  TwainOK := DLLCall( nDLLHandle, DLL_STDCALL, "DTWAIN_IsTwainAvailable" )

  IF TwainOK == 1
       // Initialize DTWAIN
      DLLCall ( nDLLHandle, DLL_STDCALL, "DTWAIN_SysInitialize" )

      // Select a TWAIN Source
      TwainSource := DLLCall( nDLLHandle, DLL_STDCALL, "DTWAIN_SelectSource2", 0, ;
                              "Select Source", 0, 0, DTWAIN_DLG_CENTER_SCREEN )

      IF  TwainSource <> 0
            // Display the name of the selected TWAIN source
            DLLCall (nDLLHandle, DLL_STDCALL, "DTWAIN_GetSourceProductName", TwainSource, ;
                       @cBuffer, 260)
            ? "The product name of the selected TWAIN source is ", PadL(cBuffer, 32)

            // Get the device capabilities of the selected source
            capArray := DLLCall (nDLLHandle, DLL_STDCALL, "DTWAIN_EnumSupportedCapsEx2", TwainSource)

            // Get the count of the number of items in the array
            capCount :=  DLLCall (nDLLHandle, DLL_STDCALL, "DTWAIN_ArrayGetCount", capArray)
            ? "There are ", capCount, " capabilities defined for the selected TWAIN source"
            ? "We will scroll through the names, 20 capabilities at a time"

            // Loop and list each capability
            FOR i := 1 TO capCount
               cBuffer := PadR( cBuffer, 256, )
               DLLCall (nDLLHandle, DLL_STDCALL, "DTWAIN_ArrayGetAtLong", capArray, i-1, @capValue)
               strLength := DLLCall (nDLLHandle, DLL_STDCALL, "DTWAIN_GetNameFromCap", capValue, @cBuffer, 260)
               cBuffer := Left( cBuffer, strLength )
               ? "Capability", i, ":", cBuffer, " Value: ", capValue
               numCapCount := numCapCount + 1
               IF numCapCount % 20 = 0
                  ? "Press any key ..."
                  Inkey(0)
               ENDIF
            NEXT
            // Acquire a file
            ? "Now starting the acquisition process ..."
            DLLCall ( nDLLHandle, DLL_STDCALL, "DTWAIN_AcquireFile", TwainSource, ;
                      "Test.bmp", DTWAIN_BMP, fileOption, DTWAIN_PT_DEFAULT, 1, 1, 1, @status )
      ENDIF

      // Uninitialize DTWAIN
      DLLCall( nDLLHandle, DLL_STDCALL, "DTWAIN_SysDestroy" )
  ENDIF

   // Unload the DLL
   DLLUnload( nDLLHandle )
RETURN
```



