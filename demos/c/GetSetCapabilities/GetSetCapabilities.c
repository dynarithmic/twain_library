#include <stdio.h>
#include "dtwain.h"

int GetSetCapabilitiesDemo()
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
        printf("Source was not selected: %ld", DTWAIN_GetLastError());
        return -2;
    }

    /* Display the product name of the selected source */
    DTWAIN_GetSourceProductNameA(theSource, prodName, 255);
    printf("The selected source is \"%s\"\n", prodName);

    /* Even though there is a DTWAIN_EnumPaperSizes() function that returns all the supported
       paper sizes, let's do this the "long way" by utilizing the DTWAIN_GetCapValues function. */
    DTWAIN_ARRAY allPageSizes;
    DTWAIN_GetCapValues(theSource, DTWAIN_CV_ICAPSUPPORTEDSIZES, DTWAIN_CAPGET, &allPageSizes);
    LONG nCount = DTWAIN_ArrayGetCount(allPageSizes);
    for (LONG i = 0; i < nCount; ++i)
    {
        LONG value;
        char szName[256];
        DTWAIN_ArrayGetAtLong(allPageSizes, i, &value);
        DTWAIN_GetPaperSizeNameA(value, szName, 256);
        printf("Page size %lu: %s\n", i + 1, szName);
    }

    /* Set the current size to the first value found */
    LONG firstValue;
    DTWAIN_ArrayGetAt(allPageSizes, 0, &firstValue);

    /* Get rid of all the items in the array */
    DTWAIN_ArrayRemoveAll(allPageSizes);

    /* Now add the first item found */
    DTWAIN_ArrayAddLong(allPageSizes,firstValue);

    /* Set the current value to the first value */
    DTWAIN_SetCapValues(theSource, DTWAIN_CV_ICAPSUPPORTEDSIZES,DTWAIN_CAPSETCURRENT,allPageSizes);

    /* See if the current value actually has been set */
    DTWAIN_ARRAY retArray;
    DTWAIN_GetCapValues(theSource, DTWAIN_CV_ICAPSUPPORTEDSIZES,DTWAIN_CAPGETCURRENT,&retArray);

    /* Test if the CAP_SERIALNUMBER capability is supported */
    if (DTWAIN_IsCapSupported(theSource, DTWAIN_CV_CAPSERIALNUMBER))
    {
        // Get the serial number
        char szSerialNumber[256];
        DTWAIN_ARRAY aSerialNumbers;
        DTWAIN_GetCapValues(theSource, DTWAIN_CV_CAPSERIALNUMBER, DTWAIN_CAPGET, &aSerialNumbers);
        if (DTWAIN_ArrayGetCount(aSerialNumbers) > 0)
        {
            DTWAIN_ArrayGetAtStringA(aSerialNumbers,0,szSerialNumber);
            printf("\nThe CAP_SERIALNUMBER setting: \"%s\"\n", szSerialNumber);
            DTWAIN_ArrayDestroy(aSerialNumbers);
        }
    }

    DTWAIN_ArrayDestroy(allPageSizes);
    DTWAIN_ArrayDestroy(retArray);

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
    GetSetCapabilitiesDemo();
    PressEnterKey();
}

