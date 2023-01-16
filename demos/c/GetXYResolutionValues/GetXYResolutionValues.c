#include <stdio.h>
#include "dtwain.h"

/* This will get the resolution values, assuming that X and Y resolution values are the same.
 * If you are using a device that has differing X and Y resolution values, use DTWAIN_EnumXResolutionValues()
 * and DTWAIN_EnumYResolutionValues() functions.
 */
 // Units of measure supported by TWAIN
const char * unit[] = { "Dots per Inch", "Dots per centimeter", "Picas", "Points", "TWIPS", "Pixels" };

void PrintResolutionValues(DTWAIN_ARRAY aResValues, LONG unitOfMeasure, char whichRes)
{
    double resValue;
    LONG nCount = DTWAIN_ArrayGetCount(aResValues);
    for (LONG i = 0; i < nCount; ++i)
    {
        DTWAIN_ArrayGetAtFloat(aResValues, i, &resValue);
        printf("  %c Resolution value %lu: %f %s\n", whichRes, i + 1, resValue, unit[unitOfMeasure]);
    }
    printf("\n\n");
}

int GetSetXYResolutionValues()
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

    /* Get the current unit of measure */
    LONG unitOfMeasure;
    DTWAIN_GetSourceUnit(theSource, &unitOfMeasure);

    /* Get the X and Y Resolution Values supported by the Source */
    DTWAIN_ARRAY xArray = DTWAIN_EnumXResolutionValuesEx(theSource, TRUE);
    DTWAIN_ARRAY yArray = DTWAIN_EnumYResolutionValuesEx(theSource, TRUE);

    PrintResolutionValues(xArray, unitOfMeasure, 'X');
    PrintResolutionValues(yArray, unitOfMeasure, 'Y');

    DTWAIN_ArrayDestroy(xArray);
    DTWAIN_ArrayDestroy(yArray);

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
    GetSetXYResolutionValues();
    PressEnterKey();
}

