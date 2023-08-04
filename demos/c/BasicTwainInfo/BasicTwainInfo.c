#include <stdio.h>
#include "dtwain.h"

/* Change this to the output directory that fits your environment */
int BasicTwainInfo()
{
    char szDisplayText[1024];
    DTWAIN_ARRAY aAllSource;

    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();
    if (handle == NULL)
    {
        printf("Could not initialize DTWAIN");
        return -1;
    }

    /* Get the TWAIN information */
    DTWAIN_GetShortVersionStringA(szDisplayText, 1024);
    printf("DTWAIN Short Version Info: %s\n", szDisplayText);

    DTWAIN_GetVersionStringA(szDisplayText, 1024);
    printf("DTWAIN Long Version Info: %s\n", szDisplayText);

    DTWAIN_GetLibraryPathA(szDisplayText, 1024);
    printf("DTWAIN Library Path: %s\n", szDisplayText);

    /* Manually start a TWAIN session.  This needs to be done to see
     * what the active DSM path will be. */
    DTWAIN_StartTwainSessionA(NULL, NULL);

    /* Now get the active Data Source Manager (DSM) being used by Twain */
    DTWAIN_GetActiveDSMPathA(szDisplayText,1024);
    printf("TWAIN DSM Path in use: %s\n", szDisplayText);

    /* Get information on the installed TWAIN sources */
    printf("\nAvailable TWAIN Sources:\n");
    DTWAIN_EnumSources(&aAllSource);

    LONG nCount = DTWAIN_ArrayGetCount(aAllSource);
    for (LONG i = 0; i < nCount; ++i)
    {
        DTWAIN_SOURCE theSource = NULL;
        DTWAIN_ArrayGetSourceAt(aAllSource, i, &theSource);
        DTWAIN_GetSourceProductNameA(theSource, szDisplayText, 1024);
        printf("   Product Name: %s\n", szDisplayText);
    }
    DTWAIN_ArrayDestroy(aAllSource);

    // Select a source and display the information using JSON format
    DTWAIN_SOURCE source = DTWAIN_SelectSource();

    if (source)
    {
        char szProductName[100];
        char* szBuf = 0;
        DTWAIN_GetSourceProductNameA(source, szProductName, 100);

        /* Get the number of bytes for the info */
        int nBytes = DTWAIN_GetSourceDetailsA(szProductName,0,0,2,TRUE);

        /* Allocate the space and call function again with the cached results
         from the last call */
        szBuf = (char *)malloc(nBytes);
        if (szBuf)
        {
            DTWAIN_GetSourceDetailsA(szProductName, szBuf, nBytes, 2, FALSE);
            printf("\n\nThe detail information for \"%s\" in JSON format is:\n", szProductName);
            printf(szBuf);
            free(szBuf);
        }
    }

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
    BasicTwainInfo();
    PressEnterKey();
}

