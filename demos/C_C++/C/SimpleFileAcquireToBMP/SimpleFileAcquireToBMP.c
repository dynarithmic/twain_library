#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif
#include <stdio.h>
#include <string.h>
#include "dtwain.h"

/* Change this to the output directory that fits your environment */
char outputDir[1024] = "";

int SimpleFileAcquireToBMP()
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
    strcat(outputDir, "SimpleFileAcquireToBMP.bmp");

    printf("Acquiring to file \"%s\"\n", outputDir);
    
    /* Note that by default, this function will block until the acquisition process is done. */

    fileError = DTWAIN_AcquireFileA(theSource, /* The TWAIN Data Source that was selected */
                                    outputDir, /* the output file name*/
                                    DTWAIN_BMP,  /* BMP format*/
                                    DTWAIN_USELONGNAME | DTWAIN_CREATE_DIRECTORY, /* Use long file names and create directory */
                                    DTWAIN_PT_DEFAULT,  /* Default pixel (color) type*/
                                    1,       /* Acquire a single page*/
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
    SimpleFileAcquireToBMP();
    PressEnterKey();
}

