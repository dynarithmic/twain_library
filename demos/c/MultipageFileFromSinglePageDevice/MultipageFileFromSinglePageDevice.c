#include <stdio.h>
#include <string.h>
#include "dtwain.h"

/* Change this to the output directory that fits your environment */
char outputDir[1024] = "";

int MultiPageFileAcquire()
{
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();

    DTWAIN_SOURCE theSource = 0;
    LONG fileError;
    LONG status;
    char prodName[256];
    if (handle == NULL)
    {
        printf("Could not initialize DTWAIN");
        return -1;
    }

    /* Select the source */
    theSource = DTWAIN_SelectSource();
    if (theSource == NULL)
    {
        printf("Source was not selected: %ld", DTWAIN_GetLastError());
        return -2;
    }

    /* Display the product name of the selected source */
    DTWAIN_GetSourceProductNameA(theSource, prodName, 255);
    printf("The selected source is \"%s\"\n", prodName);

    /* Acquire the image as a multipage TIFF file,
     * even if the device does not have a document feeder */
    strcat(outputDir, "MultipageUsingSinglePageDevice.tif");

    printf(" Acquiring to file \"%s\"\n", outputDir);

    /* Create the file when the source UI is closed */
    DTWAIN_SetMultipageScanMode(theSource, DTWAIN_FILESAVE_UICLOSE);

    /* Turn off the feeder (if it exists), just for this demo */
    DTWAIN_EnableFeeder(theSource, FALSE);

    /* Note that by default, this function will block until the acquisition process is done. */
    fileError = DTWAIN_AcquireFileA(theSource, /* The TWAIN Data Source that was selected */
                                    outputDir, /* the output file name*/
                                    DTWAIN_TIFFLZWMULTI,  /* TIFF format*/
                                    DTWAIN_USELONGNAME | DTWAIN_CREATE_DIRECTORY, /* Use long file names and create directory */
                                    DTWAIN_PT_DEFAULT,  /* Default pixel (color) type*/
                                    1,           /* Acquire a single page at a time, since the device can only acquire 1 page */
                                    TRUE,        /* Show the user interface */
                                    TRUE,        /* Close the source when DTWAIN_AcquireFileA returns*/
                                    &status);    /* return status */

    if (fileError != DTWAIN_TRUE)
    {
        printf(" File could not be acquired: %ld\nstatus code: %ld", DTWAIN_GetLastError(), status);
        return -3;
    }

    printf("Acquired \"%s\" successfully\n", outputDir);

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
    MultiPageFileAcquire();
    PressEnterKey();
}

