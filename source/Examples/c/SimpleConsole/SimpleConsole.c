// Example1.cpp : Acquires an image from a TWAIN Source
//
#include <dtwain.h>
#include <stdio.h>

int main()
{
    DTWAIN_HANDLE isInitialized = DTWAIN_SysInitialize();
    DTWAIN_SOURCE Source;
    if (!isInitialized)
    {
        printf("Could not initialize DTWAIN library");
        return -1;
    }
    Source = DTWAIN_SelectSource();
    if (Source)
        DTWAIN_AcquireFile(Source, NULL, DTWAIN_BMP, DTWAIN_USEPROMPT, DTWAIN_PT_BW, DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
    DTWAIN_SysDestroy();
}