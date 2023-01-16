#include <stdio.h>
#include "dtwain.h"

int TwainCapListing()
{
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();
    DTWAIN_SOURCE theSource = 0;
    DTWAIN_ARRAY aCapValues;
    char prodName[256];
    char capName[100];
    LONG capCount;
    LONG capValue;
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

    /* List the capabilities available for the device */
    aCapValues = DTWAIN_EnumSupportedCapsEx2(theSource);

    /* Get the number of caps and loop to display all the names */
    capCount = DTWAIN_ArrayGetCount(aCapValues);
    for (int i = 0; i < capCount; ++i)
    {
        DTWAIN_ArrayGetAtLong(aCapValues,i,&capValue);
        DTWAIN_GetNameFromCapA(capValue,capName,sizeof(capName));
        printf("%s (%ld)\n", capName, capValue);
    }

    /* Destroy the array (note that DTWAIN_SysDestroy will destroy all the allocated arrays
     * but you should do this to avoid unnecessary usage of memory for DTWAIN_ARRAYs that are
     * no longer needed.  */
    DTWAIN_ArrayDestroy(aCapValues);
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
    TwainCapListing();
    PressEnterKey();
}

