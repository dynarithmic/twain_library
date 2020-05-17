### What is this repository for? ###

* This repository contains the DTWAIN Library from Dynarithmic Software, version 5.1.  DTWAIN is an open source programmer's library that will allow applications to acquire images from TWAIN-enabled devices.

* The Dynarithmic TWAIN Library is open source and licensed under the Apache 2.0 License.  Please read the [LICENSE](https://github.com/dynarithmic/twain_library/tree/master/LICENSE) file for more information.
* The DTWAIN Library online help file can be found [here](http://www.dynarithmic.com/onlinehelp5/dtwain/index.html).
* The current version is **5.1.0.6**



### Ok, so what is this Dynarithmic TWAIN library, or "DTWAIN" as you call it? ###

* The Dynarithmic TWAIN Library (also known as DTWAIN) is an open source, powerful programmer's library that will allow you to easily integrate TWAIN image acquisition from any TWAIN scanner or digital camera into your applications.  

* DTWAIN is implemented as a 32-bit and 64-bit Windows Dynamic Link Library (DLL), and to communicate with the DLL, exported `C` based functions are provided.  DTWAIN is supported under Windows 7 / Windows 8.x / Windows 10 for both 32-bit and 64-bit operating systems.  

* If you are not familiar with the TWAIN standard and image acquisition from TWAIN-enabled devices, please head to the official TWAIN website at [http://www.twain.org](http://www.twain.org) for more information.  If you've ever bought or used a scanner, and came across the words "TWAIN compliant" or "TWAIN driver", well you're on the right track.  If you're interested in getting these devices to work in your **C, C++, C#, Java, Visual Basic, Perl, Python** (and other languages) application, you've come to the right place.  

* The DTWAIN library relieves the programmer of having to get into the details of writing low-level code that follows the TWAIN specification to retrieve images from a TWAIN device -- just a few function calls to initialize and acquire images from the TWAIN device is all that's required.  

(There is nothing wrong with understanding the TWAIN specification, as this will enhance your knowledge of how the DTWAIN library works internally.  However the high-level of abstraction of the TWAIN protocol makes this library simple for even the novice programmer to use).

----------

### Is DTWAIN 5.x really Open Source Software (OSS)? 
 
* The Dynarithmic® TWAIN Library has been developed over the course of 20 years, so this is a very mature software component.  We have offered this library as a closed source, commercial product over those years, however we have decided to make this library open source under the Apache 2.0 license.    

* Please note -- since DTWAIN prior to version 5.0 used source code in some modules that could not be released to the general public due to licensing issues, we had to revamp these portions of the codebase so as to allow DTWAIN to become an open source library.  We have made all strives to make sure that these changes to DTWAIN will not cause issues, but as most of you know, bugs can exist.  If bugs are found, we will be addressing them in a short manner.

----------

### How do I get set up using DTWAIN? ###
  
If you do not want to rebuild the source code and just get started using DTWAIN, the [binaries](https://github.com/dynarithmic/twain_library/tree/master/binaries) directory contains all of the Dynamic Link Libraries (DLL).

    32bit/dtwain32.dll/lib -- 32-bit ANSI (MBCS) DLL
    32bit/dtwain32u.dll/lib -- 32-bit Unicode DLL
    32bit/dtwain32d.dll/lib -- 32-bit Debug ANSI (MBCS) DLL
    32bit/dtwain32ud.dll/lib -- 32-bit Debug Unicode DLL
    64bit/dtwain64.dll/lib -- 64-bit ANSI (MBCS) DLL
    64bit/dtwain64u.dll/lib -- 64-bit Unicode DLL
    64bit/dtwain64d.dll/lib -- 64-bit Debug ANSI (MBCS) DLL
    64bit/dtwain64ud.dll/lib -- 64-bit Debug Unicode DLL

If you are using C or C++, you will need to include the header files in the [c_cpp_includes](https://github.com/dynarithmic/twain_library/tree/master/c_cpp_includes) directory when building your application.  Your build **INCLUDE** path should refer to these header files.

Basically, you just need to build your application and link it to one of the .lib files above that matches the environment your application is targeted for.  For example, if the application you're developing is a 32-bit, Unicode-based application, you would use the **dtwain32u.lib** file to allow your C/C++ application to link without errors (assuming you're using Visual C++), and the **dtwain32u.dll** file above is what is loaded when your application is running .  

Just make sure the DLL is located somewhere on the system path, or in your application directory (there are other places where the DLL can be located, but that is beyond the scope of this introduction -- please refer to the following link:

[https://docs.microsoft.com/en-us/windows/desktop/dlls/dynamic-link-library-search-order](https://docs.microsoft.com/en-us/windows/desktop/dlls/dynamic-link-library-search-order)

In addition to the dll and lib files, the text resource files must also be available (they should reside in the same directory as the DLL files above).  The  text resources files are as follows:

    twaininfo.txt -- General TWAIN information -- this is required.
	twainresourcestrings_english.txt  	English resources -- this is required.

The files above are required, since they contain all the information concerning the naming of the TWAIN capabilities, triplet information, etc.  You do not need to know what these various aspects of TWAIN are -- just make sure these files reside in the same directory as the dtwain*.dll when your application is executed.

If these files are not found, you will receive the following error when running your application;

![following error when running your application](/images/resource_error.jpg)

In addition, there are optional string resource files available that.  Here are a list of those files:
	
	twainresourcestrings_dutch.txt 	 	Dutch resources
	twainresourcestrings_french.txt  	French resources
	twainresourcestrings_german.txt  	German resources
	twainresourcestrings_italian.txt 	Italian resources
	twainresourcestrings_spanish.txt 	Spanish strings


If you want to use a different resource file or even add your own language resource, it is recommended you copy the file in question, rename the file, make the changes required, and then utilize the new file by calling the **DTWAIN_LoadCustomStringResources** API function.  

More detailed instructions on adding your own resource file can be found <a href="https://github.com/dynarithmic/twain_library/tree/master/additional_language_resources" target="_blank">here</a>.

----------
### I don't have a TWAIN device or scanner installed on my system.  How do I work with DTWAIN?
There are sample virtual TWAIN devices [found here](https://github.com/dynarithmic/twain_library/tree/master/SampleTWAINDevices).  Once installed, these devices will be available for selection for acquiring images, similar to an installed scanner.

----------

  
### Ok, how about a code sample?

The simplest example is probably one that opens the TWAIN "Select Source" dialog, allows the user to choose the TWAIN device.  Once chosen, the device acquires an image and saves the image as a BMP file named "Test.bmp".  Here is an entire C++ example that demonstrates this:

    #include "dtwain.h"
    #include <iostream>

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

Setting and getting device capabilities is an integral part of using a TWAIN-enabled device.  This is easily done by using the generic capability functions such as *DTWAIN_EnumCapabilities*, *DTWAIN_GetCapValues* and *DTWAIN_SetCapValues*, or one of the functions that wrap the setting of a capability such as *DTWAIN_SetResolution*, *DTWAIN_SetBrightness*, etc.

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
 
Of course, if the capability does not exist on the device, or if the values given to the capability are not supported (for example, if the device only supports 200 DPI and the function attempts to set the DPI to 300), the function returns FALSE and the error can be determined by calling *DTWAIN_GetLastError*.

In general, DTWAIN can set or get any capability, including custom capabilities that some manufacturers may support, and any future capabilities that may be added to the TWAIN specification.      

----------

### What if I don't have Visual Studio as the compiler to use when building an application?  I use Embarcadero/g++/clang/MingW/Dev++ (fill in with your favorite compiler or IDE).  How do I use the library?

You can do one of two things:

1. Attempt to convert the .lib files mentioned above to your compiler's version of an import library, or
2. Forget about using libraries altogether, and use dynamic library loading using the Windows API LoadLibrary, GetProcAddress, and FreeLibrary calls.

For the first item, some compilers have external tools that allow you to use Visual Studio generated library files.  For the second item, there are bindings that we have built that facilitate the usage of LoadLibrary/GetProcAddress/FreeLibrary, without you having to tediously write the interface.  It can be [found here](https://github.com/dynarithmic/twain_library/blob/master/language_bindings_and_examples/C_CPP_DynamicLoad). 

    /* Include this header */
    #include "dtwainx2.h"
    
    /* declare the API instance and the handle to the loaded library */
    DYNDTWAIN_API API;
    HMODULE h;

    int main()
    {
        /* Load the library dynamically, and hook up the external functions */  
        h = LoadLibraryA("dtwain32.dll");
        InitDTWAINInterface(&API, h);
 
        /* Use the API hook code */
        API.DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = API.DTWAIN_SelectSource();
        if ( Source )
            API.DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP, 
                                    DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT, 
                                    DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
        API.DTWAIN_SysDestroy();         
    }         


----------

### Wait...What about other computer languages?  Does this library only work for C and C++ applications? ###

DTWAIN includes computer language bindings for the following computer languages and utilities found in the [language_bindings_and_examples](https://github.com/dynarithmic/twain_library/tree/master/language_bindings_and_examples) folder:

      C/C++ header and source files for dynamic loading using the Windows API LoadLibrary() and GetProcAddress() functions.
      C# 
      Delphi
      Java (using the Java Native Interface JNI)
      Macro Scheduler
      Perl
      Python 
      Visual Basic 6.0 (old, but we support it)
      Visual Basic .NET 
      WinBatch
      XBase++ (Alaska Software)
  
For example, here is a bare-bones C# language example of acquiring a BMP image from a default TWAIN device installed on your system:

    using DynaRithmic;
    [STAThread]
    static void Main()
    {
	   TwainAPI.DTWAIN_SysInitialize();
	   int SelectedSource = TwainAPI.DTWAIN_SelectSource();
	   if ( SelectedSource != 0 )
	   {
	      int status = 0;
	      TwainAPI.DTWAIN_AcquireFile(SelectedSource,
	                                          "Test",
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
          
Other languages can be supported, as long as the language is capable of calling exported DLL functions (all exported functions are *stdcall* and have a C compatible interface, similar to the Windows API functions).  The ones listed above just have proper interfaces to the exported functions already set up.

A full C# demo can be found <a href="https://github.com/dynarithmic/twain_library-csharp_demo" target="_blank">here</a>.

A full Visual Basic .NET demo can be found <a href="https://github.com/dynarithmic/twain_library-visualbasic_demo" target="_blank">here</a>.

Other demos for other languages will be coming soon.

----------

### Programming issues with an event driven system such as TWAIN.

Since TWAIN is an event-driven system, it is advantageous for whatever language you use to be able to support "callback" functions, i.e. functions that can be called during the TWAIN acquisition / scanning process.  There are a lot of events that occur during the acquisition process that you may want to take advantage of (for example, page about to be scanned, page scanned successfully, immediate access to the acquired image, etc.), and having the ability to capture these events will give your application the most flexibility.  

For example, please see the DTWDEMO.exe example program when acquiring to a PDF file, as a page number is added to the page for each page that is acquired by the device.  This is only possible (using DTWAIN) by using callbacks.
  
The *DTWAIN_SetCallback* and *DTWAIN_SetCallback64* sets up your callback function to intercept these events and act accordingly.  

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


Languages such as C, C++, C#, can use callbacks (sometimes referred to as *delegates* in the .NET world) to allow such functionality.  Other languages also have the capability to set callbacks.  Please refer to the documentation for the language you use to see if callback functionality exists (if you can get the DTWAIN_SetCallback or DTWAIN_SetCallback64 to work for you, then you're not going to have any issues).

----------


### I am ambitious and would like to build the libraries, debug the internals, etc.  How do I get started? ###

The source code and instructions for building DTWAIN are found [here](https://github.com/dynarithmic/twain_library/tree/master/source).

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

If you're a C++ programmer, and want a small wrapper around the DTWAIN libarary, we do have a C++ wrapper for DTWAIN, however it is rather crude for todays C++ standards, even though it does make DTWAIN simpler to use.  Thus we are in the process of creating a "better" C++ wrapper using templates, less "fat" classes, RAII techniques, etc.

----------

### Acknowledgments ###

* Other than the interface to the TWAIN libraries to allow image acquisition, The Dynarithmic TWAIN Library makes use of the following third-party libraries to process image data.

  * crypto++   - Public domain cryptography library by [Wei Dei](https://www.cryptopp.com/)
  * FreeImage  - [Open source Imaging library](http://freeimage.sourceforge.net/).  Note:  We use the FreeImage Public License terms [found here](https://github.com/dynarithmic/twain_library/tree/master/source/FreeImage/license-fi.txt).
  * SimpleINI  - Open source (MIT License) [INI file parsing library](https://github.com/brofield/simpleini)



* In addition, an interface to the [TOCR OCR library](http://www.transym.com/).  This allows image files to be translated to text files for functions such as DTWAIN_AcquireFile with the type to acquire being DTWAIN_TXT.  To use TOCR requires you to purchase a separate license from Transym (we do not provide the DLL or the libraries, just the function calls to allow usage of the TOCR library).

* All other raw image processing (plus the TWAIN acquisition) is done without third-party libraries.  The image formats that are not implemented using third-party libraries are PDF, Windows Meta File (WMF) and Enhanced Meta File (EMF).  



----------

### Who do I talk to if I have further questions? Who authored this library? ###

The Dynarithmic TWAIN Library's principal developer is Paul McKenzie, and can be reached at [paulm@dynarithmic.com](mailto::paulm@dynarithmic.com).        

My background is mostly in C and C++ programming, third-party library creation on Windows-based systems (some Linux also).    

In addition to 30+ years in the industry, I have made extensive contributions to the [CodeGuru](http://www.codeguru.com) C++ and Visual C++ forums, being one of the top contributors for over 15 years starting in 1999, and since 2014, moved on to  [StackOverflow](https://stackoverflow.com/users/3133316/paulmckenzie?tab=profile), where hopefully I am making an impact with other passionate programmers.  Also, I have had the distinction of being a Microsoft MVP for 10 years running in the Visual C++ category.

I am available for contracting work and code reviews (mostly using the C and C++ languages).  If you're interested in my services, please email me for further information on my availability.

  
