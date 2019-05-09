#ifdef _MSC_VER
#pragma warning (disable:4786)
#endif
#include <pdflib32.h>
#include <dtwainpdf.h>
#include <vector>

using namespace dynarithmic;

//////////////////////////////////
/// Postscript stuff
int PostscriptMain(int argc, const char**, const char *szTitle);
LONG FUNCCONVENTION dynarithmic::DTWLIB_PSWriteFile(LPCTSTR szFileIn,
                                     LPCTSTR szFileOut,
                                     LONG PSType,
                                     LPCTSTR szTitle,
                                     bool bUseEncapsulated)
{
    CTL_String sFileIn = StringConversion::Convert_Native_To_Ansi(szFileIn);
    CTL_String sFileOut = StringConversion::Convert_Native_To_Ansi(szFileOut);
    CTL_String sTitle  = StringConversion::Convert_Native_To_Ansi(szTitle);

    // Create a fake command line to pass to the main Postscript conversion routine
    CTL_String sCommandLine;
    const char *argv[6];
    argv[0] = "DTWLIB";
    argv[1] = "-a";
    switch( PSType )
    {
        case 1:
            argv[2] = "-1";
        break;

        case 2:
            argv[2] = "-2";
        break;

        case 3:
            argv[2] = "-3";
        break;
    }
    sCommandLine = "-O";
    sCommandLine += sFileOut;
    std::vector<char> sCommandV(sCommandLine.length() + 1, 0);
    std::copy(sCommandLine.begin(), sCommandLine.end(), sCommandV.begin());
    argv[3] = &sCommandV[0];

    if ( bUseEncapsulated )
        argv[4] = "-e";
    else
        argv[4] = "-p";

    argv[5] = (char*)sFileIn.c_str();
    return PostscriptMain(6, argv, sTitle.c_str());
}
