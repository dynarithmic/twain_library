If:
1) Your C++ compiler cannot use Visual C++ import libraries, or 

2) You are using Visual C++, but desire to not use import libraries and instead use the Windows API `LoadLibrary` and `GetProcAddress` to load the DTWAIN library and retrieve the function pointers for each function

then the recommended solution is to use `LoadLibrary`, `GetProcAddress`, and eventually `FreeLibrary` Windows API functions.  

However, there are many DTWAIN functions, and you might be fearful of having to write code that tediously tries to create function pointers, call **GetProcAddress**, etc.  Let alone there are **many** DTWAIN API functions, and you will have to make sure that all the parameter types and return values for all of these functions are correct.

Thankfully, there is no need to do that, as there are bindings that we have built that facilitate the usage of LoadLibrary/GetProcAddress/FreeLibrary Windows API functions "automatically" (your code only has to call `LoadLibrary` and `FreeLibrary`, and one single binding function that sets up all of the function pointers).  

In addition, one of the files in the set of bindings is the C/C++ source file **dtwimpl.cpp** (or **dtwimpl.c** if you are using plain C) -- this file will need to be added to your project, as it contains the needed infrastructure for the binding to work properly.  Failure to add this source file will result in linker errors when building your application.

Here is an example of code that works for both the LoadLibrary/GetProcAddress technique, and the "normal" DTWAIN usage of import libraries.

```cpp
#ifdef USING_LOADLIBRARY
   /* Include this header */
    #include "dtwainx2.h"
    #define API_INSTANCE API.
    
    /* declare the API instance and the handle to the loaded library */
    DYNDTWAIN_API API;
    HMODULE h;
#else
    #include "dtwain.h"
    #define API_INSTANCE
#endif

int main()
{
    #ifdef USING_LOADLIBRARY
        /* Load the library dynamically, and hook up the external functions */  
        h = LoadLibraryA("dtwain32.dll");
        if ( !h )
            return -1; /* DTWAIN DLL was not found or could not be loaded */
            
        /* This is the binding function that retrieves all of the function pointers */    
        API_INSTANCE InitDTWAINInterface(&API, h);
    #endif
    
    API_INSTANCE DTWAIN_SysInitialize();
    DTWAIN_SOURCE Source = API_INSTANCE DTWAIN_SelectSource();
    if ( Source )
        API_INSTANCE DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP, 
                                DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT, 
                                DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
    API_INSTANCE DTWAIN_SysDestroy();         
    
    /* Free the library */
    #ifdef USING_LOADLIBRARY
        FreeLibrary(h);
    #endif
}
```

The code above makes use of a preprocessor macro called **USING_LOADLIBRARY** that when defined will use the LoadLibrary technique.  If **USING_LOADLIBRARY** is defined, then the header file **dtwainx2.h** is utilized, otherwise the normal **dtwain.h** header is used.  In addition, the **API_INSTANCE** prefix is set to the **API.** text.

Note that your code has to call **LoadLibrary** with the appropriate DTWAIN DLL name.  If the DLL is found at runtime, the **InitDTWAINInterface** is called with the address of the API instance and the returned module handle from the **LoadLibrary** call.

The **InitDTWAINInterface** function fills the API instance with all of the function pointers that exist in the DLL, where each function pointer matches the name of the DTWAIN function.  Basically, all of the calls to **GetProcAddress** that you would have had to normally write is taken care of in the **InitDTWAINInterface** function.  From there, all you have to do is preface all the DTWAIN calls with **API_INSTANCE**, since it will either be **API.** or blank, depending on whether the **USING_LOADLIBRARY** was defined.

