This is a **D** example using  [dtwainapi.d](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/D) file that defines the DTWAIN API constants and functions.  The program gives an example of 
1) Selecting a TWAIN device installed on your system, 

2) displaying a list of the capabilities available to the device,

3) How to setup a callback function to and 

4) Start the TWAIN device and acquire to a BMP file.

Please note that the DTWAIN DLL's that are supported are the Unicode versions of the DLL, i.e. dtwain32u.dll, dtwain32ud.dll, dtwain64u.dll, and dtwain64ud.dll.  

Since the Unicode version of the DTWAIN API also has ANSI equivalent functions (API functions whose names end with the letter "A"), your application can still call the ANSI functions if deemed necessary.


```cpp
import std.stdio;
import std.conv;
import dtwainapi;
import std.format;
import core.sys.windows.windows : OutputDebugStringW;
import std.utf : toUTF16z;

extern(Windows) ptrint myCallback64(ptrint wParam, ptrint lParam, long userData)
{
    // Cast the dll handle back to the right type
    auto theDLL = cast(DTWAIN_DynamicDLL*) userData;

    // Get the name of the notification using the DTWAIN utility function to get string versions
    // of most of the TWAIN-related and DTWAIN-related constants
        char [256] szNotification;
    auto len = theDLL.DTWAIN_GetTwainNameFromConstantA(theDLL.DTWAIN_CONSTANT_DTWAIN_TN, // The constant type
                                                       wParam,  // The actual constant value
                                                       cast(char *)szNotification, // name is returned here
                                                       256); // maximum size of the output buffer 

    immutable log = format("Notification=%s, lParam=%s\n", szNotification[0 .. len-1], lParam);

    // Log this to the debug monitor (the Output Window if you are using the Visual Studio IDE)
    OutputDebugStringW(toUTF16z(log));
    return 1;
}

void main()
{
    // Load the DTWAIN DLL, depending on the environemnt
    string defaultDLL = "dtwain64u.dll"; // 64-bit

    static if (ptrint.sizeof == 4) 
        defaultDLL = "dtwain32u.dll";    // 32-bit
    auto dll = new DTWAIN_DynamicDLL(defaultDLL);

    // Initialize DTWAIN
    auto dtwain_handle = dll.DTWAIN_SysInitialize();
    if ( !dtwain_handle )
    {
        writeln("DTWAIN failed initialization");
        return;
    }

    // Select a TWAIN source by using the enhanced 
    // TWAIN Select Source dialog (we can center it on the screen)
    auto TwainSource = dll.DTWAIN_SelectSource2W(null, "This is a test", 0, 0, dll.DTWAIN_DLG_CENTER_SCREEN);

    if ( !TwainSource )
    {
        writeln("No source was selected");
        return;
    }

    // Display the source product name
    char [256] szBuffer;
    auto len = dll.DTWAIN_GetSourceProductNameA(TwainSource, cast(char*)szBuffer, 256);

    // Example usage of DTWAIN_ARRAY
    // Get the device capabilities that are supported by the selected source
    DTWAIN_ARRAY capArray = null;
    dll.DTWAIN_EnumSupportedCaps(TwainSource, &capArray);

    // Get the number of items in the array
    int arrayCount = dll.DTWAIN_ArrayGetCount(capArray);

    writeln("There are ", arrayCount, " capabilities defined for device ", szBuffer[0 .. len-1]);

    // Now display the names of the capabilities
    // We can either use DTWAIN_ArrayGetAtLong() to retrieve each value
    // iteratively, but since we are using D, we can get a pointer to the 
    // array buffer and use simple indexing instead of incurring the call
    // to DTWAIN_ArrayGetAtLong() each time
    int* ptrCapArray = cast(int *)dll.DTWAIN_ArrayGetBuffer(capArray, 0);
    for (int i = 0; i < arrayCount; ++i)
    {
        // Get the name of the capability
        len = dll.DTWAIN_GetNameFromCapA(*ptrCapArray, cast(char*)szBuffer, 256);
        writeln("Capability ", i + 1, ": ", szBuffer[0 .. len-1], "  Value: ", *ptrCapArray);
        ++ptrCapArray; // Go to next cap in the array
    }

    // Now demonstrate setting up a callback for notification processing.
    // We must enable the notification "engine" first
    dll.DTWAIN_EnableMsgNotify(true);

    // Now set the callback
    auto userDataPtr = cast(long)&dll; // pass handle to the DLL as the user data
    dll.DTWAIN_SetCallback64(&myCallback64, userDataPtr);

    // Now let's acquire a page from the device and save to a BMP file
    dll.DTWAIN_AcquireFileA(TwainSource, "testd.bmp", 
                                                        dll.DTWAIN_BMP, dll.DTWAIN_USELONGNAME,
                           dll.DTWAIN_PT_DEFAULT, 1,1,1, null);

    // Now close down DTWAIN.  You *must* do this when done using DTWAIN, so that resources are freed, and that
    // any callbacks you have set do not fire when DTWAIN is closed.
    dll.DTWAIN_SysDestroy();
}
```

