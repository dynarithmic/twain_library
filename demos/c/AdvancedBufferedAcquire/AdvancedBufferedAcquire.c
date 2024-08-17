#ifdef _MSC_VER
    #define _CRT_SECURE_NO_WARNINGS
#endif
#include <stdio.h>
#include "dtwain.h"
/* This callback is invoked by the DTWAIN library whenever an event
* during the acquisition process is triggered
*/

HANDLE hTheDibStrip = NULL;
DTWAIN_SOURCE theSource = 0;
char outName[] = "raw.bin";
FILE *rawFile = NULL;
LONG bufferWritten = 0;

LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData)
{
    switch (wParam)
    {
        /* Sent when ready to transfer the image */
        case DTWAIN_TN_TRANSFERREADY:
        {
            /* Get the image dimensions, so that we have an area to collect each
             * strip
             */
            double xres, yres;
            LONG width, height, bpp;

            /* Get the image information for the image that is about to be transferred */
            DTWAIN_GetImageInfo(theSource, &xres, &yres, &width, &height, NULL, NULL, &bpp, NULL, NULL, NULL);

            /* From here, your application should use the information from the DTWAIN_GetImageInfo() to build 
               the image headers and any other information your application needs to maintain */
            /* ... */
            return TRUE;
        }
        break;

        case DTWAIN_TN_TRANSFERSTRIPDONE:
        case DTWAIN_TN_TRANSFERDONE:
        {
            if (rawFile)
            {
                LONG bytesWritten;
                BYTE* pTheDibStrip;

                ++bufferWritten;

                /* Add the strip to the buffer */
                /* First, get the number of bytes received (that's all we care about in the example) */
                DTWAIN_GetAcquireStripData(theSource, NULL, NULL, NULL, NULL, NULL, NULL, &bytesWritten);

                printf("\nWriting buffer strip %lu.  Number of bytes written will be %lu", bufferWritten, bytesWritten);

                /* Lock the strip of data to a pointer */
                pTheDibStrip = (BYTE*)GlobalLock(hTheDibStrip);

                /* Write the strip of data to the output file */
                if (rawFile)
                    fwrite(pTheDibStrip, bytesWritten, 1, rawFile);

                GlobalUnlock(pTheDibStrip);

                if (wParam == DTWAIN_TN_TRANSFERDONE)
                {
                    /* close the output file */
                    fclose(rawFile);
                    rawFile = NULL;
                }
            }
            return TRUE;
        }

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
    }
    return 1;
}

void SetupMemoryBuffer()
{
    // Get the preferred memory size for the buffer to use when
    // acquiring data in strips.
    LONG minSize, maxSize, prefSize;

    /* Get the minimum, maximum, and preferred size */
    DTWAIN_GetAcquireStripSizes(theSource, &minSize, &maxSize, &prefSize);

    /* Use the preferred size to allocate memory */
    hTheDibStrip = GlobalAlloc(GHND, prefSize);

    /* Let DTWAIN know about the buffer */
    DTWAIN_SetAcquireStripBuffer(theSource, hTheDibStrip);

    /* If we want to set the compression type to something
     * other than DIB, we would call DTWAIN_SetCompressionType() here.
     * For now, we will only demonstrate using DIB's, since this is
     * supported for all TWAIN devices
     */

    /* Create the output file that will consist of raw image data */
    rawFile = fopen(outName, "wb");
}

int AdvancedBufferedAcquire()
{
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();

    DTWAIN_ARRAY allData = 0;
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

    SetupMemoryBuffer();
    if (hTheDibStrip == NULL )
    {
        printf("Could not create buffered memory strip\n");
        DTWAIN_SysDestroy();
        return -3;
    }

    /* Also allow DTWAIN messages to be sent to our callback */
    DTWAIN_EnableMsgNotify(TRUE);
    DTWAIN_SetCallback(TwainCallbackProc, 0);

    /* Acquire the image as a Device Independent Bitmap using Buffered transfer */
    allData = DTWAIN_AcquireBuffered(theSource, /* The TWAIN Data Source that was selected */
                                     DTWAIN_PT_DEFAULT,  /* Default pixel (color) type*/
                                     1,       /* Acquire a single page*/
                                     TRUE,   /* Show the user interface */
                                     TRUE,    /* Close the source when DTWAIN_AcquireFileA returns*/
                                     &status); /* return status */

    if (allData == NULL)
    {
        printf("Images could not be acquired: %ld\nstatus code: %ld", DTWAIN_GetLastError(), status);
        return -3;
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
    AdvancedBufferedAcquire();
    PressEnterKey();
}

