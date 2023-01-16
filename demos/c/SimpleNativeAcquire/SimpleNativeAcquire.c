#include <stdio.h>
#include "dtwain.h"

int SimpleNativeAcquire()
{
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();

    DTWAIN_SOURCE theSource = 0;
    DTWAIN_ARRAY allData = 0;
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

    /* Acquire the image as a Device Independent Bitmap using Native Twain transfer */
    /* Note that by default, this function will block until the acquisition process is done. */
    allData = DTWAIN_AcquireNative(theSource, /* The TWAIN Data Source that was selected */
                                    DTWAIN_PT_DEFAULT,  /* Default pixel (color) type*/
                                    1,       /* Acquire a single page*/
                                    TRUE,   /* Show the user interface */
                                    TRUE,    /* Close the source when DTWAIN_AcquireFileA returns*/
                                    &status); /* return status */

    if ( allData == NULL )
    {
        printf("Images could not be acquired: %ld\nstatus code: %ld", DTWAIN_GetLastError(), status);
        return -3;
    }

    /* Get the number of times the user attempted to acquire a set of images */
    LONG acquisitionCount = DTWAIN_GetNumAcquisitions(allData);
    printf("You probably hit the \"scan\" button %lu times.\n", acquisitionCount);

    printf("\nGet the image data for each item:\n");
    for (LONG i = 0; i < acquisitionCount; ++i)
    {
        /* Get the number of images for acquisition i */
        LONG numImages = DTWAIN_GetNumAcquiredImages(allData, i);
        printf("\n  Acquisition %lu has the following info:\n", i + 1);
        printf("    Number of images acquired: %lu\n", numImages);
        printf("    Handle to the image data:\n");
        for (LONG j = 0; j < numImages; ++j)
        {
            /* Get the image data associated with acquisition i, page j */
            HANDLE hDib = DTWAIN_GetAcquiredImage(allData, i, j);
            if ( hDib )
            {
                printf("      Image %lu: %p\n", j + 1, hDib);
                SIZE_T imageDataSize = GlobalSize(hDib);
                printf("      Image size in bytes: %llu\n", imageDataSize);

                /* From here, we can call GlobalLock(hDib) to get the image data. */
                /* We won't do that here, but this is where your application can */
                /* utilize the returned Device Independent Bitmap */
            }
        }
    }
    /* Destroy the DTWAIN Arrays, plus the image data */
    /* Note the second argument determines if DTWAIN will also destroy the image data */
    DTWAIN_DestroyAcquisitionArray(allData, TRUE);

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
    SimpleNativeAcquire();
    PressEnterKey();
}

