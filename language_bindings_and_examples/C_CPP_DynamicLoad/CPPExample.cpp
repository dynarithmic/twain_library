#include "dtwainx2.h"

int main()
{
	// Declare a DYNDTWAIN_API variable
	DYNDTWAIN_API API;
	DTWAIN_SOURCE Source;

	// Load the Unicode library
	HMODULE h = LoadLibrary(L"dtwain32u");

	// Check if library loaded correctly.  If so, initialize the DTWAIN
	//  interface
	if (h)
	    API.InitDTWAINInterface(h);
	else
	    return 0;

	// Example DTWAIN calls

	// This is DTWAIN_SysInitialize
	API.DTWAIN_SysInitialize();

	// DTWAIN_SelectSource
	Source = API.DTWAIN_SelectSource();

	// DTWAIN_AcquireFile
	API.DTWAIN_AcquireFile(Source, L"FILE.BMP", DTWAIN_BMP, DTWAIN_USENAME, DTWAIN_PT_DEFAULT, 1, TRUE, TRUE, NULL);

	// DTWAIN_SysDestroy
	API.DTWAIN_SysDestroy();
	// Once finished with DTWAIN, free the library
	FreeLibrary(h);
}