### What is this repository for? ###

* This repository contains the DTWAIN Library from Dynarithmic Software, version 5.0.  DTWAIN is an open source programmer's library that will allow applications to acquire images from TWAIN-enabled devices.

* The Dynarithmic TWAIN Library is open source and licensed under the Apache 2.0 License.  Please read the [LICENSE](https://github.com/dynarithmic/twain_library/tree/master/LICENSE) file for more information.
* The DTWAIN Library online help file can be found [here](http://www.dynarithmic.com/onlinehelp5/dtwain/index.html).




### Ok, so what is this Dynarithmic TWAIN library, or "DTWAIN" as you call it? ###

* The Dynarithmic TWAIN Library (also known as DTWAIN) is an open source, powerful programmer's library that will allow you to easily integrate TWAIN image acquisition from any TWAIN scanner or digital camera into your applications.  

* DTWAIN is implemented as a 32-bit and 64-bit Windows Dynamic Link Library (DLL), and to communicate with the DLL, exported `C` based functions are provided.  DTWAIN is supported under Windows XP / Windows 7 / Windows 8.x / Windows 10 for both 32-bit and 64-bit operating systems.  

* If you are not familiar with the TWAIN standard and image acquisition from TWAIN-enabled devices, please head to the official TWAIN website at [http://www.twain.org](http://www.twain.org) for more information.  If you've ever bought or used a scanner, and came across the words "TWAIN compliant" or "TWAIN driver", well you're on the right track.  If you're interested in getting these devices to work in your **C, C++, C#, Java, Visual Basic, Perl, Python** (and other languages) application, you've come to the right place.  

* The DTWAIN library relieves the programmer of having to get into the details of writing low-level code that follows the TWAIN specification to retrieve images from a TWAIN device -- just a few function calls to initialize and acquire images from the TWAIN device is all that's required.  

(There is nothing wrong with understanding the TWAIN specification, as this will enhance your knowledge of how the DTWAIN library works internally.  However the high-level of abstraction of the TWAIN protocol makes this library simple for even the novice programmer to use).

----------

### Is DTWAIN 5.0 really Open Source Software (OSS)? 
 
* The Dynarithmic® TWAIN Library has been developed over the course of 20 years, so this is a very mature software component.  We have offered this library as a closed source, commercial product over those years, however we have decided to make this library open source under the Apache 2.0 license.    

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

In addition, there are optional string resource files available that.  Here are a list of those files:
	
	twainresourcestrings_dutch.txt 	 	Dutch resources
	twainresourcestrings_french.txt  	French resources
	twainresourcestrings_german.txt  	German resources
	twainresourcestrings_italian.txt 	Italian resources
	twainresourcestrings_spanish.txt 	Spanish strings


If you want to use a different resource file or even add your own language resource, it is recommended you copy the file in question, rename the file, make the changes required, and then utilize the new file by calling the **DTWAIN_SetCustomLanguageResource** API function.

----------
### I don't have a TWAIN device or scanner installed on my system.  How do I work with DTWAIN?
There are sample virtual TWAIN devices [found here](https://github.com/dynarithmic/twain_library/tree/master/SampleTWAINDevices).  Once installed, these devices will be available for selection for acquiring images, similar to an installed scanner.

----------

  
### Ok, how about a code sample?

The simplest example is probably one that opens the TWAIN "Select Source" dialog, allows the user to choose the TWAIN device.  Once chosen, the device acquires an image and saves the image as a BMP file.  Here is an entire C++ example that demonstrates this:

    #include "dtwain.h"
    int main()
    {
        DTWAIN_SysInitialize();
        DTWAIN_SOURCE Source = DTWAIN_SelectSource();
        if ( Source )
            DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP. DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT,
                                TRUE, TRUE, NULL);
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
            images = DTWAIN_AcquireNative(Source, DTWAIN_BMP. DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT,
                                          TRUE, TRUE, NULL);
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

    DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP. DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT, FALSE, TRUE, NULL);

or if it is the second example:

    images = DTWAIN_AcquireNative(Source, DTWAIN_BMP. DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT,
                                          FALSE, TRUE, NULL);

----------

### What if I don't have Visual Studio?  I use Embarcadero/g++/clang/MingW/Dev++ (fill in with your favorite compiler or IDE).  How do I use the library?

You can do one of two things:

1. Attempt to convert the .lib files mentioned above to your compiler's version of an import library, or
2. Eschew using libraries altogether, and use dynamic library loading using the Windows API LoadLibrary, GetProcAddress, and FreeLibrary calls.

For the first item, some compilers have external tools that allow you to use Visual Studio generated library files.  For the second item, there are bindings that we have built that facilitate the usage of LoadLibrary/GetProcAddress/FreeLibrary, without you having to tediously write the interface.  It can be [found here](https://github.com/dynarithmic/twain_library/tree/master/language_bindings/C_CPP_DynamicLoad). 

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
            API.DTWAIN_AcquireFileA(Source, "Test.bmp", DTWAIN_BMP. DTWAIN_USENATIVE | DTWAIN_USENAME, DTWAIN_PT_DEFAULT,
                                TRUE, TRUE, NULL);
        API.DTWAIN_SysDestroy();         
    }         


----------

### Wait...What about other computer languages?  Does this library only work for C and C++ applications? ###

DTWAIN includes computer language bindings for the following computer languages and utilities found in the [language_bindings](https://github.com/dynarithmic/twain_library/tree/master/language_bindings) folder:

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
	                                           ref status);
	   }
	   TwainAPI.DTWAIN_SysDestroy();
    }
          
Other languages can be supported, as long as the language is capable of calling exported DLL functions (all exported functions are *stdcall* and have a C compatible interface, similar to the Windows API functions).  The ones listed above just have proper interfaces to the exported functions already set up.

----------


### I am ambitious and would like to build the libraries, debug the internals, etc.  How do I get started? ###

If you want to rebuild the libraries, you will need the following tools and computer resources:

One of the following compilers:

      * Visual Studio 2015 with Update 3
      * Visual Studio 2017 (may need to install XP tools and Windows 10 SDK from the VS 2017 Installation Manager).
      * Visual Studio 2019
      
In addition, you will need at least 20 GB of free disk space

Note that the C++ source code should be able to be built with any C++11 or C++14 compliant compiler that recognizes the Windows API headers (MingW using g++ 5.0 or above is an example).  However we have not tested builds of the DTWAIN library that have been built with any other compiler other than the Visual Studio family.   

* Start Visual Studio, and open one of the DTWAIN solution.  The DTWAIN solution files are found in the [source](https://github.com/dynarithmic/twain_library/tree/master/source) directory.  Open **dtwain_5_vs2015.sln** or **dtwain_5_vs2017.sln**, depending on whether you are using Visual Studio 2013, 2015, or 2017, respectively. 

* A full rebuild of all the configurations available is recommended.  Use the "Build -> Batch Build..." option in the Visual Studio IDE and check all of the configurations to build everything (take a coffee break -- this could take a while).  This will create a "binaries" directory that will contain the following DLLs:

        32bit/dtwain32.dll -32-bit ANSI (MBCS) DLL
        32bit/dtwain32u.dll -32-bit Unicode DLL
        32bit/dtwain32d.dll -32-bit Debug ANSI (MBCS) DLL
        32bit/dtwain32ud.dll -32-bit Debug Unicode DLL
        64bit/dtwain64.dll -64-bit ANSI (MBCS) DLL
        64bit/dtwain64u.dll -64-bit Unicode DLL
        64bit/dtwain64d.dll -64-bit Debug ANSI (MBCS) DLL
        64bit/dtwain64ud.dll -64-bit Debug Unicode DLL

* Note -- the resulting "*.lib* files that reside in these directories are import libraries compatible with the Visual Studio toolset.  Other compilers will require converting these .lib files to your compiler's import library format, or you can use the LoadLibrary / GetProcAddress approach (we have a wrapper for this -- see below in the "Getting DTWAIN to work with other programming languages" section).

* When all the configurations are built, there should be multiple DTWDEMO*.exe programs residing in the **binaries** subdirectory, where the suffix used in the program name matches the DTWAIN DLL that will be loaded.  For example, DTWDEMO32U.exe will load the dtwain32u.dll library when run. The easiest way to get started is to debug DTWDEMO.EXE and single step through the program using the debugger to get a feel of the flow of the program.  You should get a good idea of how DTWAIN works if you step into one or more of the DTWAIN functions (such as DTWAIN_SysInitialize or DTWAIN_SelectSource).

* DTWAIN requires an installation of the [Boost C++ library](http://www.boost.org).  The DTWAIN project files expect that a "boost" folder exists with the Boost header files located in this directory.  This repository includes boost 1.68 as part of the build.  Later versions of Boost can be used, but please make sure that if you will use a later version of Boost, the existing "boost" directory be completely replaced with the later version, and that the boost libraries that are required for your compiler are placed in the [boost_libs](https://github.com/dynarithmic/twain_library/tree/master/source/boost) directory (either [boost/boost_libs/32](https://github.com/dynarithmic/twain_library/tree/master/source/boost/boost_libs/32) or [boost/boost_lib/64](https://github.com/dynarithmic/twain_library/tree/master/source/boost/boost_libs/64), depending on whether the libraries are 32-bit or 64-bit, respectively.
  

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

We have language bindings (pinvoke's) for C# and Visual Basic.  However we currently lack a true .NET component (just did not have the time to create one).  If anyone out there is willing to create such a .NET component, we are willing to add the compoenent to this repository once vetted and well-tested (and of course, give full credit to the author(s)).

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

* The DTWAIN library's main developer is P. McKenzie, and can be reached at paulm@dynarithmic.com.  
