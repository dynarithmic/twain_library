#include <stdio.h>
#include "dtwain.h"

/* Pixel types supported by TWAIN */
const char * pixelTypes[] = 
        {"TWPT_BW", "TWPT_GRAY", "TWPT_RGB", "TWPT_PALETTE", "TWPT_CMY", "TWPT_CMYK", "TWPT_YUV",
        "TWPT_YUVK", "TWPT_CIEXYZ", "TWPT_LAB", "TWPT_SRGB", "TWPT_SCRGB", "TWPT_BGR","TWPT_CIELAB",
        "TWPT_CIELUV", "TWPT_YCBCR", "TWPT_INFRARED","TWPT_DEFAULT"};

int GetColorTypes()
{
    char pixelTypeName[255];
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

    /* Get all the pixel types supported */
    DTWAIN_ARRAY aPixelTypes = NULL;
    DTWAIN_ARRAY aBitDepths = NULL;
    DTWAIN_EnumPixelTypes(theSource, &aPixelTypes);

    printf("Pixel type and bit depth info for device \"%s\":\n", prodName);

    /* Loop through each pixel type found */
    LONG nPixelCount = DTWAIN_ArrayGetCount(aPixelTypes);
    for (LONG i = 0; i < nPixelCount; ++i)
    {
        LONG pixelType;
        DTWAIN_ArrayGetAtLong(aPixelTypes, i, &pixelType);
        DTWAIN_GetTwainNameFromConstantA(DTWAIN_CONSTANT_TWPT, pixelType, pixelTypeName, 254);
        printf("\n  Color/Pixel Type %s has the following supported bit depths:\n", pixelTypeName);

        /* Set the color type first */
        BOOL bSetPixelType = DTWAIN_SetPixelType(theSource,pixelType, DTWAIN_DEFAULT,TRUE);
        if ( bSetPixelType )
        {
            /* Now get the bit depths */
            DTWAIN_EnumBitDepths(theSource, &aBitDepths);

            /* Go throughDoes each bit depth */
            LONG bitDepthCount = DTWAIN_ArrayGetCount(aBitDepths);
            LONG bitDepth;
            for (int j = 0; j < bitDepthCount; ++j)
            {
                DTWAIN_ArrayGetAtLong(aBitDepths, j, &bitDepth);
                printf("        Bit depth: %lu\n", bitDepth);
            }
        }
    }
    DTWAIN_ArrayDestroy(aPixelTypes);
    DTWAIN_ArrayDestroy(aBitDepths);

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
    GetColorTypes();
    PressEnterKey();
}