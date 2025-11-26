### What is this repository for? ###

* This repository contains the DTWAIN Library, **Version 5.x**, from Dynarithmic Software.  DTWAIN is an open source programmer's library that will allow applications to acquire images from TWAIN-enabled devices using a simple Application Programmer's Interface (API).

* The Dynarithmic TWAIN Library is open source and licensed under the Apache 2.0 License.  Please read the [LICENSE](https://github.com/dynarithmic/twain_library/tree/master/LICENSE) file for more information.
* The DTWAIN Library online help file can be found [here](https://www.dynarithmic.com/onlinehelp/dtwain/newversion/Dynarithmic%20TWAIN%20Library,%20Version%205.x.html), and in .CHM (Windows Help) format [here](https://github.com/dynarithmic/twain_library-helpdocs/tree/main/windows).  

    The .CHM file and online-help are being updated to version 5.x on a constant basis.  Updates will be made available in the [help repository](https://github.com/dynarithmic/twain_library-helpdocs/tree/main), as it may have information that pertains to the older commercial version of DTWAIN that will have to be updated or removed.
* The current version of DTWAIN is [**5.7.1** (See Version History)](https://github.com/dynarithmic/twain_library/tree/master/updates/updates.txt).

**Please note that the source code and sample programs for the Dynarithmic TWAIN Library has moved to [this repository](https://github.com/dynarithmic/twain_library_source/tree/main)**.

----

### Ok, so what is this Dynarithmic TWAIN library, or "DTWAIN" as you call it? ###

* The Dynarithmic TWAIN Library (also known as DTWAIN) is an open source, powerful programmer's library that will allow you to easily integrate TWAIN image acquisition from any TWAIN scanner or digital camera into your applications.  

* DTWAIN is implemented as a 32-bit and 64-bit Windows Dynamic Link Library (DLL), and to communicate with the DLL, exported functions are provided.  This allows any Windows-based computer language that can call exported DLL functions (directly or indirectly) to be able to use DTWAIN.  This includes C, C++, C#, Visual Basic, Python, Delphi, Java, Ruby, and numerous other languages.

* If you are not familiar with the TWAIN standard and image acquisition from TWAIN-enabled devices, please head to the official TWAIN website at [http://www.twain.org](http://www.twain.org) for more information.  If you've ever bought or used a scanner, and came across the words "TWAIN compliant" or "TWAIN driver", well you're on the right track.  If you're interested in getting these devices to work in your **C, C++, C#, Lua, Java, Visual Basic, Perl, Python, Ruby** (and other languages) application, you've come to the right place.  

* The DTWAIN library relieves the programmer of having to get into the details of writing low-level code that follows the TWAIN specification to retrieve images from a TWAIN device -- just a few function calls to initialize and acquire images from the TWAIN device is all that's required.  

(There is nothing wrong with understanding the TWAIN specification, as this will enhance your knowledge of how the DTWAIN library works internally.  However the high-level of abstraction of the TWAIN protocol makes this library simple for even the novice programmer to use).

----------
### Is DTWAIN 5.x really Open Source Software (OSS)? 
 
* The Dynarithmic® TWAIN Library has been developed over the course of 20 years, so this is a very mature software component.  We have offered this library as a closed source, commercial product over those years, however we have decided to make this library open source under the Apache 2.0 license.    

* Please note -- since DTWAIN prior to version 5.0 used source code in some modules that could not be released to the general public due to licensing issues, we had to revamp these portions of the codebase so as to allow DTWAIN to become an open source library.  We have made all strives to make sure that these changes to DTWAIN will not cause issues, but as most of you know, bugs can exist.  If bugs are found, we will be addressing them in a short manner.

----------

### Preliminaries

DTWAIN is supported under *Windows 10 / Windows 11 for both 32-bit and 64-bit operating systems. Since the base libraries are built to support the Visual C++ runtime library, version 2015 and above, the minimum version of the Visual C++ runtime that is supported is **Visual C++ 2015**.  

*There is no official support for Windows 7 / Windows 8.x, as newer versions of DTWAIN are only tested for Windows versions 10 and 11.  Note that this is not to say that DTWAIN will not work for Windows 7 / 8, but it hasn't been tested with these operating systems for the past few years.  If you require support for Windows 7 or 8, please open an [issue](https://github.com/dynarithmic/twain_library/issues) to see if this can be addressed.

----

The "standard" versions of the DTWAIN library ("standard" meaning the DLL's found in the **full_logging** and **partial_logging** directories -- See below in the **How do I get set up using DTWAIN** section) will not require an installation of the Visual C++ runtime files, so there shouldn't be an issue when using the standard versions.  However the standard versions are larger in size (up to a megabyte or so) than the versions that require an installation of the Visual C++ runtime already installed on the system that DTWAIN will be running on.

Since most Windows systems within the past 8 years has the Visual C++ runtime already installed by other applications, this may not be an issue and the smaller-sized DTWAIN DLL's can be used.  However, if for some reason your system does not have the proper runtime components, you can get the Visual C++ runtime libraries <a href="https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170#latest-microsoft-visual-c-redistributable-version" target="_blank">here</a>.  When downloading, choose **vc_redist.x86.exe** for 32-bit applications, and/or **vc_redist.x64.exe** for 64-bit applications.

----------
### I don't have a TWAIN device or scanner installed on my system.  How do I work with DTWAIN?
There are sample virtual TWAIN devices [found here](https://github.com/dynarithmic/twain_virtual-drivers).  Once installed, these devices will be available for selection for acquiring images, similar to an installed scanner.

----------
----

### How do I get set up using DTWAIN? ###
----
**<u>Building the DTWAIN application</u>**

<a name="dtwaindllusage"></a>
For 32-bit applications, use the binaries found in **release_libraries.zip** in one of the following directories:

[full_logging](https://github.com/dynarithmic/twain_library/tree/master/binaries/32bit/full_logging) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/32bit/full_logging/release_ziphashes32.txt)</br>
[partial_logging](https://github.com/dynarithmic/twain_library/tree/master/binaries/32bit/partial_logging) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/32bit/partial_logging/release_ziphashes32.txt)</br>
[full_logging_require_vcruntime](https://github.com/dynarithmic/twain_library/tree/master/binaries/32bit/full_logging_require_vcruntime) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/32bit/full_logging_require_vcruntime/release_ziphashes32.txt)</br>
[partial_logging_require_vcruntime](https://github.com/dynarithmic/twain_library/tree/master/binaries/32bit/partial_logging_require_vcruntime) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/32bit/partial_logging_require_vcruntime/release_ziphashes32.txt)</br> 

----

For 64-bit applications, use the binaries found in **release_libraries.zip** in one of the following directories:

[full_logging](https://github.com/dynarithmic/twain_library/tree/master/binaries/64bit/full_logging) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/64bit/full_logging/release_ziphashes64.txt)</br>
[partial_logging](https://github.com/dynarithmic/twain_library/tree/master/binaries/64bit/partial_logging) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/64bit/partial_logging/release_ziphashes64.txt)</br>
[full_logging_require_vcruntime](https://github.com/dynarithmic/twain_library/tree/master/binaries/64bit/full_logging_require_vcruntime) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/64bit/full_logging_require_vcruntime/release_ziphashes64.txt)</br>
[partial_logging_require_vcruntime](https://github.com/dynarithmic/twain_library/tree/master/binaries/64bit/partial_logging_require_vcruntime) with checksums found here: [checksum values](https://github.com/dynarithmic/twain_library/blob/master/binaries/64bit/partial_logging_require_vcruntime/release_ziphashes64.txt)</br> 


----

The **full_logging** directory contains the DLL's that have the following characteristics: 
1) Built with full logging capabilities. Full logging consists of logging the call stack and return values when calling DTWAIN functions, plus the lower level calls that DTWAIN makes to the TWAIN Data Source Manager.  This is valuable in detecting issues that may occur when issuing calls to DTWAIN.
2) Does not require an installation of the Visual C++ Runtime on the target system.

The **partial_logging** directory contains the DLL's that are:
1) Built without call stack and return values being logged.  These DLL's are around 500K smaller in size than the DLL's in **full_logging**.  Direct calls to the lower level TWAIN DSM are included, but the call stack and return value logging is not available, and
2) Does not require an installation of the Visual C++ Runtime on the target system.

The **full_logging_require_vcruntime** directory contains the DLL's that are 
1) Built with full logging capabilities, exactly the same as **full_logging** described above, and
2) Requires an installation of the Visual C++ Runtime on the target system.

The **partial_logging_require_vcruntime** directory contains the DLL's that are 
1) Built without call stack and return values being logged, exactly the same as **partial_logging** described above, and
2) Requires an installation of the Visual C++ Runtime on the target system.

If you are not concerned with sizes of the DLL's, the **full_logging** DLL's should be used.  If you desire DLL's that are a bit smaller and can "sacrifice" call stack / return value logging, the **partial_logging** DLL's should be used.  

If you will install the Visual C++ Runtime yourself, or assume that the systems you will run DTWAIN on have the Visual C++ Runtime already installed, you can use the DLL's in the "*_require_vcruntime" directories, further reducing the size of the DTWAIN DLL's being used.

To distinguish between whether the full or partial logging DLL's are in use (since the names of the DTWAIN DLL's themselves are the same, regardless of which ones are used), see [the following information](https://github.com/dynarithmic/twain_library_source/tree/main/binaries/32bit#how-to-distinguish-between-full-and-partial-logging-dlls).

In addition, the [release version of the Program Database (.PDB) files](https://github.com/dynarithmic/dtwain-pdb) are available.  This will aid in debugging any issues involving DTWAIN.

----

A breakdown of the files contained in **release_libraries.zip** is as follows:

    dtwain32.dll   --  32-bit ANSI (MBCS) Dynamic Link Library
    dtwain32u.dll  --  32-bit Unicode Dynamic Link Library
    dtwain32.lib   --  32-bit ANSI (MBCS) Visual C++ import library
    dtwain32u.lib  --  32-bit Unicode Visual C++ import library
    dtwain32.pdb   --  32-bit PDB (Microsoft debug) files for dtwain32.dll
    dtwain32u.pdb  --  32-bit PDB (Microsoft debug) files for dtwain32u.dll

    dtwain64.dll   --  64-bit ANSI (MBCS) Dynamic Link Library
    dtwain64u.dll  --  64-bit Unicode Dynamic Link Library
    dtwain64.lib   --  64-bit ANSI (MBCS) Visual C++ import library
    dtwain64u.lib  --  64-bit Unicode Visual C++ import library
    dtwain64.pdb   --  64-bit PDB (Microsoft debug) files for dtwain64.dll
    dtwain64u.pdb  --  64-bit PDB (Microsoft debug) files for dtwain64u.dll


###### Information for C and C++ programmers:

If you are using Visual C++, the Visual C++ compatible import libraries necessary to build your 32-bit or 64-bit application (the files with the *.lib extension) are available.<br><br> 
If you do not use Visual C++ but instead are using another brand of C++ compiler, see the [section on additional C++ compiler usage](#alternatecompilers) to alleviate the import library issues.  

You will also need to include the header files found in the [c_cpp_includes](https://github.com/dynarithmic/twain_library/tree/master/c_cpp_includes) directory when building your application.  Your build **INCLUDE** path should refer to these header files.

Basically, you just need to build your application and link it to one of the import libraries that matches the environment your application is targeted for.  For example, if the application you're developing is a 32-bit, Unicode-based application, you would use the **dtwain32u.lib** file to allow your C/C++ application to link without errors.

----
<a name="runningapplication"></a>
**<u>Running the application</u>**

After building your application, for your application to run successfully, you must make sure the DTWAIN dynamic link library itself is located somewhere on the system path, or in your application directory (there are other places where the DLL can be located, but that is beyond the scope of this introduction -- please refer to the following link:

[https://docs.microsoft.com/en-us/windows/desktop/dlls/dynamic-link-library-search-order](https://docs.microsoft.com/en-us/windows/desktop/dlls/dynamic-link-library-search-order)).

In addition to the DLL files, the <a href="https://github.com/dynarithmic/twain_library/tree/master/text_resources/twaininfo.txt" target="_blank">text resource file</a>, the <a href="https://github.com/dynarithmic/twain_library/blob/master/text_resources/dtwain32.ini" target="_blank">dtwain32.ini</a> for 32-bit applications, and <a href="https://github.com/dynarithmic/twain_library/blob/master/text_resources/dtwain64.ini" target="_blank">dtwain64.ini</a> for 64-bit applications </a> must also be available (by default, these files should reside in the same directory as the DLL files above, however as of version **5.2.0.2**, these files can reside in the directory specified by **DTWAIN_SetResourcePath**).  

If **twaininfo.txt** or the INI files are not found, corrupted, incorrect version, or some other issue that prevents these files from being loaded, you will receive the following message box displayed, with one or more reasons for the error listed:

![following error when running your application](/images/resource_error.jpg)

The error message will differ depending on the reason for the error.

Note: If your application wants to suppress the above message box, but still receive an error return code, your application should issue a call to the API function **DTWAIN_SysInitializeNoBlocking** instead of **DTWAIN_SysInitialize** (see the examples below -- simply change **DTWAIN_SysInitialize** to **DTWAIN_SysInitializeNoBlocking**).  

* Make sure that you are running the latest version of **twaininfo.txt**, as changes to this file can affect how your application will run when using future versions of DTWAIN.  The simplest way to ensure that you are running the latest version is to always get the latest **twaininfo.txt** file whenever you use a newer release of the DTWAIN DLL's.  

An internal check for the resource version is done by DTWAIN.  If DTWAIN detects that the resources are corrupted or out-of-date, **DTWAIN_SysInitialize** will return a NULL handle indicating an error.  

If **DTWAIN_SysInitialize** or **DTWAIN_SysInitializeNoBlocking** returns a 0 or null handle, you should call **DTWAIN_GetLastError** to get the error value.  In addition, you can call **DTWAIN_GetErrorString** with the error number to get a string description of the error.

----

In addition, there are [optional string resource files available](https://github.com/dynarithmic/twain_library/tree/master-staging/additional_language_resources).  These files allow you to customize the language used when DTWAIN logs or reports errors.  Note that these files are loaded only after **DTWAIN_SysInitialize** or **DTWAIN_SysInitializeNoBlocking** returns without error.

These files should be placed in the same directory as the **twaininfo.txt** file and INI files.

If you want to use a different resource file or even add your own language resource, it is recommended you copy the file in question, rename the file, make the changes required, and then utilize the new file by calling the **DTWAIN_LoadCustomStringResources** API function.  

More detailed instructions on adding your own resource file can be found <a href="https://github.com/dynarithmic/twain_library/tree/master/additional_language_resources" target="_blank">here</a>.

----------
### Ok, how about a code sample?

The simplest example is probably one that opens the TWAIN "Select Source" dialog, allows the user to choose the TWAIN device.  Once chosen, the device acquires an image and saves the image as a BMP file named "Test.bmp".  Here is an entire C++ example that demonstrates this:

    #include <iostream>
    #include "dtwain.h"

    int main()
    {
        // display DTWAIN version, just for fun
        std::cout << "Hello to DTWAIN " << DTWAIN_VERINFO_FILEVERSION << "\n";

        // initialize and acquire image and save to BMP file
        DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = DTWAIN_SelectSource();
        if ( Source )
            DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP, DTWAIN_USENATIVE | DTWAIN_USENAME,
                                DTWAIN_PT_DEFAULT, DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
        DTWAIN_SysDestroy();         
    }         

That's it.  

If you desire to acquire to an image in memory instead of a file, that can be done also.  By default, a Windows-based TWAIN driver always returns a Device Independent Bitmap (DIB) as the memory image.    

    #include "dtwain.h"
    int main()
    {
        DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = DTWAIN_SelectSource();
        DTWAIN_ARRAY images;
        if ( Source )
        {
            images = DTWAIN_AcquireNative(Source, DTWAIN_PT_DEFAULT, DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
            HANDLE hDib = DTWAIN_GetAcquiredImage(images, 0, 0); // handle to first DIB acquired is returned
            GlobalLock(hDib); // lock image
            /*Now our app has a DIB. We can do any function that needs a DIB
              This time, we'll just delete it.  Your app is responsible for deleting 
              the DIBs, since DTWAIN does not delete any DIBs generated 
              by the Source  */
            GlobalFree( hDib );
        }   
        DTWAIN_SysDestroy();         
    }         

The above assumes your application knows how to handle DIBs (they are basically BMP files).  Also note that the DTWAIN_GetAcquiredImage function takes three arguments.  The first is a "2 dimensional" array of images that were acquired, and the last two arguments denote the *acquisition number* and the *image number* in the acquisition (this technique is used to remember multipage acquisitions that occur without the user closing the device's user interface). 

In addition, you can acquire images without showing a user interface by simply specifying the "show user-interface" parameter to either TRUE (1), or FALSE (0).

    DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP, DTWAIN_USENATIVE | DTWAIN_USENAME, 
                        DTWAIN_PT_DEFAULT, DTWAIN_MAXACQUIRE, FALSE, TRUE, NULL);

or if it is the second example:

    images = DTWAIN_AcquireNative(Source, DTWAIN_PT_DEFAULT, 
                                  DTWAIN_MAXACQUIRE, FALSE, TRUE, NULL);

----------

### What about setting device capabilities such as resolution, contrast, brightness, paper size, etc.?

Setting and getting device capabilities is an integral part of using a TWAIN-enabled device.  This is easily done by using the generic capability functions such as **DTWAIN_EnumCapabilities**, **DTWAIN_GetCapValues** and **DTWAIN_SetCapValues**, or one of the functions that wrap the setting of a capability such as **DTWAIN_SetResolution**, **DTWAIN_SetBrightness**, etc.

Here is an example of setting the ICAP_PIXELTYPE capability:

    #include "dtwain.h"
    int main()
    {
        DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = DTWAIN_SelectSource();
        if ( Source )
        {
            // set the pixel type to TWPT_RGB
            
            // First, create a DTWAIN_ARRAY to hold the value(s) we will set
            DTWAIN_ARRAY aPixelTypeValue = DTWAIN_ArrayCreateFromCap(Source, ICAP_PIXELTYPE, 0);
            
            // Add the TWPT_RGB value to our array of values
            DTWAIN_ArrayAddLong(aPixelTypeValue, TWPT_RGB);
            
            // Call function to set the ICAP_PIXELTYPE capability
            DTWAIN_BOOL result = DTWAIN_SetCapValues(Source, ICAP_PIXELTYPE, DTWAIN_CAPSET, aPixelTypeValue);
            
            if ( result )
            {
                // Capability was set
            }
            
            // Dispose of the array
            DTWAIN_ArrayDestroy(aPixelTypeValue);
        }   
        DTWAIN_SysDestroy();         
    }         


Here is an example of getting all of the available ICAP_PIXELTYPE values:

    #include <stdio.h>
    #include "dtwain.h"
    int main()
    {
        DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = DTWAIN_SelectSource();
        if ( Source )
        {
            // Get the pixel type values
            
            // First, declare a DTWAIN_ARRAY that will hold all the value(s) retrieved from the device
            DTWAIN_ARRAY aPixelTypeValues;
            
            // Call function to get all the ICAP_PIXELTYPE capabilities
            DTWAIN_BOOL result = DTWAIN_GetCapValues(Source, ICAP_PIXELTYPE, DTWAIN_CAPGET,                 &aPixelTypeValues);
            
            if ( result )
            {
                // Now array has all of the values.  Print each one:
                LONG numItems = DTWAIN_ArrayGetCount(aPixelTypeValues);
                LONG pixValue;
                for (int i = 0; i < numItems; ++i)
                {
                   DTWAIN_ArrayGetAtLong(aPixelTypeValues, i, &pixValue);                   
                   printf("Pixel Type value %d is %d\n", i+1, pixValue);
                }
                DTWAIN_ArrayDestroy(aPixelTypeValues);
            }               
        }   
        DTWAIN_SysDestroy();         
    }         


Here is an example that calls the high-level function DTWAIN_SetResolution that sets the resolution to 300 DPI.  Note that DTWAIN_SetResolution() is just a "shortcut" way of setting the `ICAP_XRESOLUTION` and `ICAP_YRESOLUTION` capabilities:

    #include "dtwain.h"
    int main()
    {
        DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = DTWAIN_SelectSource();
        if ( Source )
        {
            // set the resolution level to 300 dots-per-inch
            DTWAIN_SetResolution(Source, 300.0); 
            //...
        }   
        DTWAIN_SysDestroy();         
    }         
 
Of course, if the capability does not exist on the device, or if the values given to the capability are not supported (for example, if the device only supports 200 DPI and the function attempts to set the DPI to 300), the function returns FALSE and the error can be determined by calling **DTWAIN_GetLastError**.

In general, DTWAIN can set or get any capability, including custom capabilities that some manufacturers may support, and any future capabilities that may be added to the TWAIN specification.  



----------

<a name="alternatecompilers"></a>
### What if I don't have Visual C++ as the compiler to use when building an application?  The Visual C++ import libraries will not work for me.  I use Embarcadero/g++/clang/MingW (fill in with your favorite compiler or IDE).  So how do I use the library?

You can do one of two things:

1. Attempt to convert the .lib files mentioned above to your compiler's version of an import library, or
2. Use dynamic library loading using the Windows API LoadLibrary, GetProcAddress, and FreeLibrary calls.  Usage of this method requires no import libraries to be used (which makes this a better choice).

For the first item, some compilers have external tools that allow you to use Visual Studio generated library files.  However, there still may be some quirks in those tools that do not create correct import libraries.  

For the second item, no import libraries are required, thus makes this choice the recommended option.  

There are many DTWAIN functions, and you might be fearful of having to write code that tediously tries to create function pointers, call **GetProcAddress**, etc.  There is no need to do that, as there are bindings that we have built that facilitate the usage of LoadLibrary/GetProcAddress/FreeLibrary Windows API functions.  It can be [found here](https://github.com/dynarithmic/twain_library/blob/master/programming_language_bindings/C_CPP_DynamicLoad).  

In addition, one of the files in the set of bindings is the C/C++ source file **dtwimpl.cpp** (or **dtwimpl.c** if you are using plain C) -- this file will need to be added to your project, as it contains the needed infrastructure for the binding to work properly.  Failure to add this source file will result in linker errors when building your application.

Here is an example of code that works for both the LoadLibrary/GetProcAddress technique, and the "normal" sDTWAIN usage of import libraries.

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
        API_INSTANCE InitDTWAINInterface(&API, h);
        #endif
        
        API_INSTANCE DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = API_INSTANCE DTWAIN_SelectSource();
        if ( Source )
            API_INSTANCE DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP, 
                                    DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT, 
                                    DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
        API_INSTANCE DTWAIN_SysDestroy();         
    }         

The code above makes use of a preprocessor macro called **USING_LOADLIBRARY** that when defined will use the LoadLibrary technique.  If **USING_LOADLIBRARY** is defined, then the header file **dtwainx2.h** is utilized, otherwise the normal **dtwain.h** header is used.  In addition, the **API_INSTANCE** prefix is set to the **API.** text.

Note that your code has to call **LoadLibrary** with the appropriate DTWAIN DLL name.  If the DLL is found at runtime, the **InitDTWAINInterface** is called with the address of the API instance and the returned module handle from the **LoadLibrary** call.

The **InitDTWAINInterface** function fills the API instance with all of the function pointers that exist in the DLL, where each function pointer matches the name of the DTWAIN function.  Basically, all of the calls to **GetProcAddress** that you would have had to normally write is taken care of in the **InitDTWAINInterface** function.  From there, all you have to do is preface all the DTWAIN calls with **API_INSTANCE**, since it will either be **API.** or blank, depending on whether the **USING_LOADLIBRARY** was defined.

----------

<a name="otherlanguages"></a>
### Wait...What about other computer languages?  Does this library only work for C and C++ applications? ###

Note: To utilize other computer languages, it still requires that one of the [DTWAIN dynamic link libraries (DLL)](#dtwaindllusage) is available at runtime that matches the environment (32-bit or 64-bit), and character set (ANSI, Unicode).  In addition the section on [running your application](#runningapplication) also applies.

----

DTWAIN includes computer language bindings for the following computer languages and utilities found in the [programming language bindings](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings) folder:

      C/C++ header and source files for dynamic loading using the Windows API LoadLibrary() and GetProcAddress() functions.
      C# 
      Delphi
      Java (separate repository -- see Note below)
      Lua
      Perl
      Python 
      Ruby
      Visual Basic .NET 
      XBase++ (Alaska Software)
  
* Note: The Java interface is a full-featured implementation using DTWAIN, and has a dedicated repository found in [twain_library-java](https://github.com/dynarithmic/twain_library-java).

----
###### Quick Example (C#)  
Here is a bare-bones C# language example of selecting a TWAIN device, displaying the capabilities available on the device, and acquiring a BMP image from the TWAIN device.  The only additional requirement is to add one of the <a href="https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/csharp" target="_blank">dtwain*.cs</a> files to the project, depending on the type of application (32-bit / 64-bit, ANSI / Unicode):

```csharp
using System;
// The additional dtwain*.cs file needs to be added to your project for these definitions.
using Dynarithmic; 
using DTWAIN_ARRAY = System.IntPtr;

namespace Test
{    
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize DTWAIN
            var TwainHandle = TwainAPI.DTWAIN_SysInitialize();
            if (TwainHandle == IntPtr.Zero)
                Console.WriteLine("TWAIN Failed to be initialized.  Exiting...");
            else
            {
                // Select a TWAIN Source from the TWAIN Dialog
                var SelectedSource = TwainAPI.DTWAIN_SelectSource();
                if (SelectedSource != IntPtr.Zero)
                {
                    // Display the product name of the Source
                    StringBuilder szInfo = new StringBuilder(256);
                    TwainAPI.DTWAIN_GetSourceProductNameA(SelectedSource, szInfo, 256);
                    Console.WriteLine("The source product name is " + szInfo.ToString());

                    // Get the capabilities the device supports
                    DTWAIN_ARRAY dtwain_array = IntPtr.Zero;
                    TwainAPI.DTWAIN_EnumSupportedCaps(SelectedSource, ref dtwain_array);

                    // Get the number of items in the array
                    int arrcount = TwainAPI.DTWAIN_ArrayGetCount(dtwain_array);
                    Console.WriteLine("There are " + arrcount + " device capabilities");

                    // Print each capability
                    for (int curCap = 1; curCap <= arrcount; ++curCap)
                    {
                        int int_val = 0;
                        
                        // Note that LONG values in the DTWAIN API are 32-bit integers.
                        TwainAPI.DTWAIN_ArrayGetAtLong(dtwain_array, curCap-1, ref int_val);
                        TwainAPI.DTWAIN_GetNameFromCapA(int_val, szInfo, 256);
                        Console.WriteLine("Capability " + curCap + ": " + szInfo.ToString() + "  Value: " +                     int_val);
                    }

                    int status = 0;
                    // Acquire the BMP file named Test.bmp
                    TwainAPI.DTWAIN_AcquireFile(SelectedSource,
                                                "Test.bmp",
                                                 TwainAPI.DTWAIN_BMP,
                                                 TwainAPI.DTWAIN_USENATIVE | TwainAPI.DTWAIN_USENAME,
                                                 TwainAPI.DTWAIN_PT_DEFAULT,
                                                 1,
                                                 1,
                                                 1,
                                                 ref status);
                }
                TwainAPI.DTWAIN_SysDestroy();
            }
        }
    }
}
```
----
###### Quick Example (Python)  

Here is a python example using the [ctypes](https://docs.python.org/3/library/ctypes.html) module and using the [dtwain.py](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/Python) file that defines the DTWAIN constants.  The program gives an example of selecting a TWAIN device installed on your system, displaying a list of the capabilities available to the device, and acquiring a BMP image.


```python
from ctypes import *
import dtwain
import struct
import ctypes as ct

def test_dtwain():
    # Load the DTWAIN library (make sure "dtwain32u.dll" or "dtwain64u.dll" is accessible)
    # You can use a full pathname here also, to ensure python finds the dll
    
    # Check for the python environment, and load the Unicode 64-bit or 32-bit DLL
    if struct.calcsize("P") * 8 == 64:
        dtwain_dll = dtwain.load_dtwaindll("dtwain64u.dll")
    else:
        dtwain_dll = dtwain.load_dtwaindll("dtwain32u.dll")

    # Initialize DTWAIN
    dtwain_dll.DTWAIN_SysInitialize()
    
    # Select a TWAIN source
    TwainSource = dtwain_dll.DTWAIN_SelectSource()
    if TwainSource:

        # Display the product name of the Source
        # Create a char buffer to allow calling DTWAIN_GetSourceProductNameA
        #
        # If instead you wanted to call DTWAIN_GetSourceProductName, you will need a Unicode
        # buffer, i.e. ct.create_unicode_buffer(100), as python loaded the Unicode versions
        # of the DTWAIN DLL
        #
        mystrbuf = ct.create_string_buffer(100)
        dtwain_dll.DTWAIN_GetSourceProductNameA(TwainSource, mystrbuf, len(mystrbuf))
        print (mystrbuf.value)
        
        # Example usage of DTWAIN_ARRAY:
        # Get the device capabilities supported by the device
        #
        # Note: The DTWAIN_ARRAY, DTWAIN_SOURCE, DTWAIN_FRAME, and DTWAIN_RANGE are actually void pointers
        # so you have to declare them as such if a DTWAIN function requires a parameter to be of this type.
        dtwain_array = ct.c_void_p(0)

        # Note that the second parameter is the address a DTWAIN_ARRAY, i.e. a LPDTWAIN_ARRAY
        dtwain_dll.DTWAIN_EnumSupportedCaps(TwainSource, ct.byref(dtwain_array))

        # Get the number of items in the array
        arrcount = dtwain_dll.DTWAIN_ArrayGetCount(dtwain_array)
        print(f"There are {arrcount} device capabilities")

        #print each capability
        for i in range(arrcount):
            long_val = ct.c_long(0)
            dtwain_dll.DTWAIN_ArrayGetAtLong(dtwain_array, i, ct.byref(long_val))
            dtwain_dll.DTWAIN_GetNameFromCapA(long_val, mystrbuf, len(mystrbuf))
            print(f"Capability {i+1}: {mystrbuf.value}  Value: {long_val.value}")

        # Destroy the array when done
        dtwain_dll.DTWAIN_ArrayDestroy(dtwain_array)

        # Now Acquire to a BMP file
        dtwain_dll.DTWAIN_AcquireFile(TwainSource, "TEST.BMP", dtwain.DTWAIN_BMP, dtwain.DTWAIN_USELONGNAME,
                                       dtwain.DTWAIN_PT_DEFAULT, 1, 1, 1, None)
    # Close down DTWAIN                                      
    dtwain_dll.DTWAIN_SysDestroy()

if __name__ == '__main__':
    test_dtwain()
```
----
Other languages can be supported, as long as the language is capable of calling exported DLL functions.  The ones listed above just have proper interfaces to the exported functions already set up.

A full C# demo can be found <a href="https://github.com/dynarithmic/twain_library-csharp_demo" target="_blank">here</a>.

A full Visual Basic .NET demo can be found <a href="https://github.com/dynarithmic/twain_library-visualbasic_demo" target="_blank">here</a>.

For Java, it is recommended to look through the numerous demo programs in the [Java interface to DTWAIN repository](https://github.com/dynarithmic/twain_library-java).


----------

### Programming issues with an event driven system such as TWAIN.

Since TWAIN is an event-driven system, it is advantageous for whatever language you use to be able to support "callback" functions, i.e. functions that can be called during the TWAIN acquisition / scanning process.  There are a lot of events that occur during the acquisition process that you may want to take advantage of (for example, page about to be scanned, page scanned successfully, immediate access to the acquired image, etc.), and having the ability to capture these events will give your application the most flexibility.  

For example, please see the DTWDEMO.exe example program when acquiring to a PDF file, as a page number is added to the page for each page that is acquired by the device.  This is only possible (using DTWAIN) by using callbacks.
  
The **DTWAIN_SetCallback** and **DTWAIN_SetCallback64** sets up your callback function to intercept these events and act accordingly.  

Here is an example C++ program that puts a page number on the acquired image files, saved as a PDF document.  

    #include "dtwain.h"
    #include <iostream>
    #include <cstring>

    DTWAIN_SOURCE current_source;

    // Demonstrate callback mechanism 
    LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData)
    {
       // Page count  
       static int pdf_page_count = 1;

       // detect if the notification is due to the page about to be saved
       if ( wParam == DTWAIN_TN_FILEPAGESAVING )
       {
            // write text to the bottom left of the PDF page 
            char text[50]; // create the text string
            sprintf(text, "Page %d", pdf_page_count); 
            ++pdf_page_count;

            // add the text to the page 
            DTWAIN_AddPDFTextA(current_source, 
                               text,     // text to write on the page 
                               100, 100, // (x, y) postion 
                              "Helvetica", // font to use 
                               14, // font height, in PDF points
                               DTWAIN_MakeRGB(255, 0, 0), // Red text 
                               0, 100.0, 0, 0.0, 0, // scaling, lead, etc. 
                               DTWAIN_PDFTEXT_CURRENTPAGE); // flags denoting when to write this text
       }
       return 1;
    }
    
    int main()
    {
        // initialize and acquire image and save to BMP file 
        if ( !DTWAIN_SysInitialize() )
           return 0;  // TWAIN could not be initialized 

        current_source = DTWAIN_SelectSource();
        if ( current_source )
        {
            // Enable the callback notification/mechanism 
            DTWAIN_EnableMsgNotify(TRUE);
            DTWAIN_SetCallback(TwainCallbackProc, 0);

            // Acquire to a multipage PDF file 
            DTWAIN_AcquireFileA(current_source, "test.pdf", DTWAIN_PDFMULTI, DTWAIN_USENATIVE | DTWAIN_USENAME,
                                DTWAIN_PT_DEFAULT, DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
        } 
        DTWAIN_SysDestroy();         
    }         


Languages such as C, C++, C#, can use callbacks (sometimes referred to as *delegates* in the .NET world) to allow such functionality.  Other languages also have the capability to set callbacks.  Please refer to the documentation for the language you use to see if callback functionality exists (if you can get the **DTWAIN_SetCallback** or **DTWAIN_SetCallback64** to work for you, then you're not going to have any issues).

----------


### I am ambitious and would like to build the libraries, debug the internals, etc.  How do I get started? ###

The source code and instructions for building DTWAIN are found [here](https://github.com/dynarithmic/twain_library_source/tree/main). 

----------


### Is DTWAIN compatible with Linux? ###
Currently, there is no official support for Linux, but things are changing rapidly.

As some may be aware, the TWAIN standard itself is implemented for Linux, but my opinion is that the support is poor.  There is little in terms of actual TWAIN sources to run tests against, and only certain Linux distributions are "supported" (I put this in quotes, since it is still difficult to find TWAIN drivers to run tests against).

The Linux distribution must have the Data Source Manager (DSM) installed (**usr/local/lib/libtwaindsm.so**) before proceeding.  The current distributions that we are aware of that have a working TWAIN Data Source Manager (DSM) is Ubuntu.  We were able to rebuild the DSM (**/usr/local/lib/libtwaindsm.so**) for Fedora 29 (but Fedora is not officially supported, according to the build scripts available for the building of the DSM).  

Since there are no pre-built DSM binaries, you can find the DSM source code [here on Github](https://github.com/twain/twain-dsm) and attempt to build the binaries yourself.  However, you may need to change the build scripts, since some of them are hard-coded for certain Ubuntu versions.

Once the DSM is built and installed, the build of DTWAIN can be done using the files in the source directory.  We used the CodeBlocks IDE (using g++ 6.x or higher) to organize the project (we don't currently have a CMake for DTWAIN, but one is in the works).  We also used a custom FreeImage script to do the FreeImage build.  Once done, we found no issues communicating with the Linux DSM with a small test program:

    #include "dtwain.h"
    int main()
    {
        DTWAIN_SysInitialize();     /* basic setup */
        DTWAIN_StartTwainSession(); /* communicate with libtwaindsm.so */
        DTWAIN_SysDestroy();         
    }         
  
This is more or less a proof-of-concept.  We will keep you posted on any further progress.

----------
### Is there a .NET wrapper for DTWAIN? ###

We have language bindings (pinvoke's) for C# and Visual Basic.  However we currently lack a true .NET component (just did not have the time to create one).  If anyone out there is willing to create such a .NET component, we are willing to add the component to this repository once vetted and well-tested (and of course, give full credit to the author(s)).

----------
### Is there a C++ class wrapper for DTWAIN? ###

If you're a C++ programmer, and want a wrapper around the DTWAIN libarary, we do have a C++ wrapper for DTWAIN located in the <a href="https://github.com/dynarithmic/twain_library/tree/master/demos/cpp_wrapper_lib" target="_blank">demos\cpp_wrapper_lib</a> directory.  For more information, see the <a href="https://github.com/dynarithmic/twain_library/blob/master/demos/README.md" target="_blank">README.md</a> in the demos directory.

----------

### Acknowledgments ###

* Other than the interface to the TWAIN libraries to allow image acquisition, The Dynarithmic TWAIN Library makes use of the following third-party libraries to process image data.

  * FreeImage  - [Open source Imaging library](http://freeimage.sourceforge.net/).  Note:  We use the FreeImage Public License terms [found here](https://github.com/dynarithmic/twain_library/tree/master/source/FreeImage/license-fi.txt).
  * SimpleINI  - Open source (MIT License) [INI file parsing library](https://github.com/brofield/simpleini)
  * nlohmann/JSON library - [Open source C++ JSON library](https://github.com/nlohmann/json)
  
* In addition, an interface to the [TOCR OCR library](http://www.transym.com/).  This allows image files to be translated to text files for functions such as DTWAIN_AcquireFile with the type to acquire being DTWAIN_TXT.  To use TOCR requires you to purchase a separate license from Transym (we do not provide the DLL or the libraries, just the function calls to allow usage of the TOCR library).
  
* All other raw image processing, plus the interface to the TWAIN system itself, is done without third-party libraries or third-party source code.  

----------

### Final note for developers

We expect DTWAIN to work flawlessly with almost every TWAIN-enabled device.  However, there can be issues that may happen with devices that either do not behave properly, or exercise DTWAIN in a way that's unexpected (for example, we came across a SamSung TWAIN driver for their phone that didn't follow TWAIN compliance, and thus caused issue with DTWAIN).  

Given this, the secondary goal of making DTWAIN open source is for you to contribute your fixes to the current DTWAIN code if you come across a device that doesn't work properly with DTWAIN.  There are literally thousands of TWAIN enabled devices out there, old and new, some manufacturers may have discontinued the device model, or maybe even the device manufacturer has gone out-of-business.  

Thus, the nature of "fixing" DTWAIN to work with a device that has issues is not possible to be done from a distance without having the device on-hand.  Since there are thousands of devices out there, and more than just a few have TWAIN compliance issues, we rely on our contributors who have the faulty TWAIN device ready and on-hand to debug the issue.  

You can download the source code [here](https://github.com/dynarithmic/twain_library_source/tree/main), follow the directions, and thus debug a very simple program that utilizes the device you have in-house.  Note that you should be familiar with C++, as the base library is written in this language.


----------

### Who do I talk to if I have further questions?  What if I have issues with the DTWAIN Library?

All questions concerning usage, possible bugs, etc. of the Dynarithmic TWAIN Library must have an issue created on the [Issue page](https://github.com/dynarithmic/twain_library/issues) so as to allow further investigation.

Note that issues will **not** be addressed at **dynarithmic.com** (website or email domain), and instead all issues must be directed to the Github issues page noted above.

--------

### Who authored the Dynarithmic TWAIN Library?

The Dynarithmic TWAIN Library's principal developer is Paul McKenzie, and can be reached at [paulm@dynarithmic.com](mailto::paulm@dynarithmic.com).  

In addition to 30+ years in the industry, I have made extensive contributions to the [CodeGuru](http://www.codeguru.com) C++ and Visual C++ forums, being one of the top contributors for over 15 years starting in 1999, and since 2014, moved on to  [StackOverflow](https://stackoverflow.com/users/3133316/paulmckenzie?tab=profile), where hopefully I am making an impact with other passionate programmers.  Also, I have had the distinction of being a Microsoft MVP for 10 years running in the Visual C++ category.
 
