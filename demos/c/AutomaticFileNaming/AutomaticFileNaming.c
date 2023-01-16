#include <stdio.h>
#include <string.h>
#include "dtwain.h"

/* Change this to the output directory that fits your environment */
char outputDir[1024] = "";

/* This callback is invoked by the DTWAIN library whenever an event
* during the acquisition process is triggered
*/
LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData)
{
    switch (wParam)
    {
        /* For each file saved, output the name */
        case DTWAIN_TN_FILESAVEOK:
        {
            char Msg[1024];
            DTWAIN_SOURCE Source = (DTWAIN_SOURCE)lParam;
            DTWAIN_GetCurrentFileNameA(Source, Msg, 1024);
            printf("Acquired to file %s\n", Msg);
        }
        break;
    }
    return 1;
}


int AutomaticFileNaming()
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
        printf("Source was not selected: %ld", DTWAIN_GetLastError());
        return -2;
    }

    /* Also allow DTWAIN messages to be sent to our callback */
    DTWAIN_EnableMsgNotify(TRUE);
    DTWAIN_SetCallback(TwainCallbackProc, 0);

    /* Display the product name of the selected source */
    DTWAIN_GetSourceProductNameA(theSource, prodName, 255);
    printf("The selected source is \"%s\"\n", prodName);

    /* Acquire the image as a BMP file */
    /* allow up to 10000 unique names, with the filename */
    /* having a suffix of "0000" up to "9999", prefixed with FileAcquireToBMP */ 
    strcat(outputDir, "FileAcquireToBMP0000.bmp"); 
    DTWAIN_SetFileAutoIncrement(theSource, 1, FALSE, TRUE);

    /* Let's enable the feeder if the device has one */
    DTWAIN_EnableFeeder(theSource, TRUE);

    /* Create the files when the source UI is closed */
    /* This allows us to continually scan pages, even for a flatbed scanner */
    DTWAIN_SetMultipageScanMode(theSource, DTWAIN_FILESAVE_UICLOSE);

    /* Note that by default, this function will block until the acquisition process is done. */
    fileError = DTWAIN_AcquireFileA(theSource, /* The TWAIN Data Source that was selected */
                                    outputDir, /* the output file name*/
                                    DTWAIN_BMP,  /* BMP format*/
                                    DTWAIN_USELONGNAME | DTWAIN_CREATE_DIRECTORY, /* Use long file names and create directory */
                                    DTWAIN_PT_DEFAULT,  /* Default pixel (color) type*/
                                    DTWAIN_ACQUIREALL,  /* Acquire all pages */
                                    TRUE,   /* Show the user interface */
                                    TRUE,    /* Close the source when DTWAIN_AcquireFileA returns*/
                                    &status); /* return status */

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
    AutomaticFileNaming();
    PressEnterKey();
}

