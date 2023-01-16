#include <stdio.h>
#include <string.h>
#include "dtwain.h"

/* Change this to the output directory that fits your environment */
char outputDir[1024] = "";

int DuplexScanningDemo()
{
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();

    DTWAIN_SOURCE theSource = 0;
    LONG fileError;
    LONG status;
    char prodName[256];
    if ( handle == NULL)
    {
        printf("Could not initialize DTWAIN");
        return -1;
    }

    /* Select the source */
    theSource = DTWAIN_SelectSource();
    if ( theSource == NULL )
    {
        char errorString[1024];
        DTWAIN_GetErrorStringA(DTWAIN_GetLastError(), errorString, 1024);
        printf("Error: %s", errorString);
        return -2;
    }

    /* Display the product name of the selected source */
    DTWAIN_GetSourceProductNameA(theSource, prodName, 255);
    printf("The selected source is \"%s\"\n", prodName);

    /* Determine if source supports duplex scanning */
    BOOL duplexSupported = DTWAIN_IsDuplexSupported(theSource);
    if ( !duplexSupported )
    {
        printf("Duplex scanning not supported for device \"%s\"", prodName);
        DTWAIN_SysDestroy();
        return -1;
    }

    /* Enable duplex scanning option */
    BOOL enabled = DTWAIN_EnableDuplex(theSource, TRUE);
    if ( !enabled )
    {
        printf("Duplex scanning could not be enabled for device \"%s\"", prodName);
        DTWAIN_SysDestroy();
        return -2;
    }

    /* Acquire the image as a multipage TIFF file using LZW Compression 
     The duplex option should be enabled, or if not the UI of the device
     should have the option of enabling the duplex option. */
    fileError = DTWAIN_AcquireFileA(theSource, outputDir, DTWAIN_TIFFLZWMULTI, 
                                    DTWAIN_USELONGNAME | DTWAIN_CREATE_DIRECTORY, 
                                    DTWAIN_PT_DEFAULT,DTWAIN_ACQUIREALL,TRUE,TRUE,&status);

    /* Test if acquisition process was started and ended successfully.  
    Please note that this will *not* tell you if the acquisition was cancelled, an error
    occurred such as a paper jam, etc.  To check for specific errors such as these 
    while the acquisition process is occurring, see the SimpleFileAcquireToBMPErrorHandling.c demo */
    if ( fileError != DTWAIN_TRUE )
    {
        printf("File could not be acquired: %ld\nstatus code: %ld", DTWAIN_GetLastError(), status);
        return -3;
    }

    printf( "Acquired \"%s\" successfully", outputDir);

    /* Shutdown DTWAIN */
    DTWAIN_SysDestroy();

    return 0;
}

void PressEnterKey()
{
    printf("\nPress Enter key to exit application...\n");
    getchar();
}

int main()
{
    DuplexScanningDemo();
    PressEnterKey();
}

