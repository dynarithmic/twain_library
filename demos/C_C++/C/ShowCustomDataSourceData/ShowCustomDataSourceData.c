#include <stdio.h>
#include <string.h>
#include "dtwain.h"

int ShowCustomDataSourceData()
{
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();

    DTWAIN_SOURCE theSource = 0;
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

    /* Enable blank page handling */
    LONG actualSize;
    DTWAIN_GetCustomDSData(theSource, NULL, 0, &actualSize, DTWAINGCD_COPYDATA);
    if (actualSize > 0)
    {
        BYTE *pBytes = malloc(actualSize * sizeof(BYTE));
        if ( !pBytes )
            return -3;
        DTWAIN_GetCustomDSData(theSource,pBytes,actualSize,NULL,DTWAINGCD_COPYDATA);
        printf("The Custom Data source data has %d bytes of data.  Data:\n%s", actualSize, (char *)pBytes);
        free(pBytes);

        DTWAIN_ShowUIOnly(theSource);
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
    ShowCustomDataSourceData();
    PressEnterKey();
}

