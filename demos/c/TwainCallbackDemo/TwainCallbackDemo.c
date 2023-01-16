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
        case DTWAIN_TN_TRANSFERDONE:
        {
            printf(" Transfer done\n");
        }
        break;

        case DTWAIN_TN_TRANSFERREADY:
        {
            printf(" Transfer ready\n");
        }
        break;

        case DTWAIN_TN_TRANSFERCANCELLED:
        {
            printf(" Transfer cancelled\n");
        }
        break;

        case DTWAIN_TN_UIOPENED:
        {
            printf(" UI is opened\n");
        }
        break;

        case DTWAIN_TN_UICLOSING:
        {
            printf(" UI is closing\n");
        }
        break;

        case DTWAIN_TN_UICLOSED:
        {
            printf(" UI is closed\n");
        }
        break;

        case DTWAIN_TN_FILESAVEERROR:
        {
            char errMsg[1024];
            DTWAIN_GetErrorStringA(DTWAIN_GetLastError(), errMsg, 1024);
            printf(" Could not save file.  Reason: %s\n", errMsg);
        }
        break;

        case DTWAIN_TN_FILESAVEOK:
        {
            printf(" File saved successfully\n");
        }
        break;
    }
    return 1;
}

int SimpleFileAcquire()
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

    /* Display the product name of the selected source */
    DTWAIN_GetSourceProductNameA(theSource, prodName, 255);
    printf("The selected source is \"%s\"\n", prodName);

    /* Acquire the image as a BMP file */
    strcat(outputDir, "SimpleFileAcquireToBMPErrorHandling.bmp");

    printf(" Acquiring to file \"%s\"\n", outputDir);

    /* Also allow DTWAIN messages to be sent to our callback */
    DTWAIN_EnableMsgNotify(TRUE);
    DTWAIN_SetCallback(TwainCallbackProc, 0);

    /* Note that by default, this function will block until the acquisition process is done. */
    fileError = DTWAIN_AcquireFileA(theSource, /* The TWAIN Data Source that was selected */
                                    outputDir, /* the output file name*/
                                    DTWAIN_BMP,  /* BMP format*/
                                    DTWAIN_USELONGNAME | DTWAIN_CREATE_DIRECTORY, /* Use long file names and create directory */
                                    DTWAIN_PT_DEFAULT,  /* Default pixel (color) type*/
                                    1,       /* Acquire a single page*/
                                    TRUE,    /* Show the user interface */
                                    TRUE,    /* Close the source when DTWAIN_AcquireFileA returns*/
                                    &status); /* return status */

    if ( fileError != DTWAIN_TRUE )
    {
        printf(" File could not be acquired: %ld\nstatus code: %ld", DTWAIN_GetLastError(), status);
        return -3;
    }

    printf( "Acquired \"%s\" successfully\n", outputDir);

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
    SimpleFileAcquire();
    PressEnterKey();
}

