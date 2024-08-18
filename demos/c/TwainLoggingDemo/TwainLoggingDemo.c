#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif
#include <stdio.h>
#include <string.h>
#include "dtwain.h"

int TwainLoggingDemo()
{
    char szFileName[1024];
    printf("Enter name of log file to write: ");
    scanf("%s", szFileName);

    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();

    DTWAIN_SOURCE theSource = 0;
    if (handle == NULL)
    {
        printf("Could not initialize DTWAIN");
        return -1;
    }

    // Turn on TWAIN logging to a file.
    DTWAIN_SetTwainLogA(DTWAIN_LOG_USEFILE, szFileName);

    /* Select the source */
    theSource = DTWAIN_SelectSource();
    if (theSource == NULL)
    {
        char errorString[1024];
        DTWAIN_GetErrorStringA(DTWAIN_GetLastError(), errorString, 1024);
        printf("Error: %s", errorString);
        return -2;
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
    TwainLoggingDemo();
    PressEnterKey();
}

