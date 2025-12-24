#include <stdio.h>
#include "dtwain.h"

void ShowResults(DTWAIN_SOURCE source, const char *ptrInfo)
{
    if ( source )
    {
        char szSourceName[256];
        DTWAIN_GetSourceProductNameA(source, szSourceName,1024);
        printf("The source %s was successfully opened! %s\n", szSourceName, ptrInfo);
        DTWAIN_CloseSource(source);
    }
    else
    {
        char szError[1024];
        DTWAIN_GetErrorStringA(DTWAIN_GetLastError(),szError, 1024);
        printf("Unable to open the source: %s\n", szError);
    }
}

void PlainSelectSource()
{
    DTWAIN_SOURCE theSource = DTWAIN_SelectSource();
    ShowResults(theSource, "\n (TWAIN Select Source dialog used) \n");
}

void CustomSelectSource(const char *title)
{
    DTWAIN_SOURCE theSource = DTWAIN_SelectSource2A(NULL,  // No parent window
                                                    title, // Custom title
                                                    0,      // x position 
                                                    0,      // y position
                                                    DTWAIN_DLG_CENTER_SCREEN | DTWAIN_DLG_SORTNAMES | DTWAIN_DLG_HORIZONTALSCROLL); // display options
    ShowResults(theSource, "\n (TWAIN Customized Select Source dialog used) \n");
}

void DefaultSelectSource()
{
    DTWAIN_SOURCE theSource = DTWAIN_SelectDefaultSource();
    ShowResults(theSource, "\n (Default source was opened) \n");
}

void NamedSelectSource(const char *sourceName)
{
    if ( sourceName )
    {
        DTWAIN_SOURCE theSource = DTWAIN_SelectSourceByNameA(sourceName);
        ShowResults(theSource, "\n (TWAIN Source selected by name) \n");
    }
}

void FrenchSelectSource()
{
    LONG retCode = DTWAIN_LoadCustomStringResourcesA("french");
    if ( !retCode )
        printf("Could not load French language resource\n");
    else
    {
        CustomSelectSource("French dialog");
        DTWAIN_LoadCustomStringResourcesA("english");
    }
}

int CustomizeSelectSourceDialog(const char *sourceName)
{
    /* Initialize DTWAIN */
    DTWAIN_HANDLE handle = DTWAIN_SysInitialize();
    if (handle == NULL)
    {
        printf("Could not initialize DTWAIN");
        return -1;
    }

    PlainSelectSource();
    CustomSelectSource("Custom dialog");
    DefaultSelectSource();
    NamedSelectSource(sourceName);
    FrenchSelectSource();

    /* Shutdown DTWAIN */
    DTWAIN_SysDestroy();

    return 0;
}

void PressEnterKey()
{
    printf("\nPress Enter key to exit application...\n");
    getchar();
}

int main(int argc, char *argv[])
{
    if ( argc > 1 )
        CustomizeSelectSourceDialog(argv[1]);  // The argument is the product name of the source to open
    else
        CustomizeSelectSourceDialog(NULL);
    PressEnterKey();
}

