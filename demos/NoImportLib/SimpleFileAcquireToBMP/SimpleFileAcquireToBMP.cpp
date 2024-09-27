// SimpleFileAcquireToBMP.cpp : Defines the entry point for the console application.
//
#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#include "stdafx.h"
#endif
#include <stdio.h>
#include <string.h>
#include <iostream>
#include "dtwainx2.h"

/* Change this to the output directory that fits your environment */
char outputDir[1024] = "";

struct DynamicHandler
{
    HMODULE h_;
    DynamicHandler(HMODULE h) : h_(h) {}
    ~DynamicHandler() { FreeLibrary(h_); }
};

int SimpleFileAcquireToBMP()
{
    /* Dynamically load DTWAIN */
    HMODULE hDTwainModule = ::LoadLibraryA(DTWAIN_DLLNAME);
    if (!hDTwainModule)
    {
        std::cout << "Could not find DLL -- " << DTWAIN_DLLNAME << "\n";
        return -1;
    }

    DynamicHandler dynHandler(hDTwainModule);

    DYNDTWAIN_API API;
    API.InitDTWAINInterface(hDTwainModule);
    
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = API.DTWAIN_SysInitialize();

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
    theSource = API.DTWAIN_SelectSource();
    if ( theSource == NULL )
    {
        printf("Source was not selected: %ld", API.DTWAIN_GetLastError());
        return -2;
    }

    /* Display the product name of the selected source */
    API.DTWAIN_GetSourceProductNameA(theSource, prodName, 255);
    printf("The selected source is \"%s\"\n", prodName);

    /* Acquire the image as a BMP file */
    strcat(outputDir, "SimpleFileAcquireToBMP.bmp");

    printf("Acquiring to file \"%s\"\n", outputDir);

    /* Note that by default, this function will block until the acquisition process is done. */

    fileError = API.DTWAIN_AcquireFileA(theSource, /* The TWAIN Data Source that was selected */
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
        printf("File could not be acquired: %ld\nstatus code: %ld", API.DTWAIN_GetLastError(), status);
        return -3;
    }

    printf( "Acquired \"%s\" successfully", outputDir);

    /* Shutdown DTWAIN */
    API.DTWAIN_SysDestroy();

    return 0;
}

int main()
{
    SimpleFileAcquireToBMP();
}


