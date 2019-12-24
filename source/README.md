### Rebuilding DTWAIN from source ###

If you want to rebuild the libraries, you will need the following tools and computer resources:

One of the following compilers:

      * Visual Studio 2015 with Update 3
      * Visual Studio 2017 (may need to install XP tools and Windows 10 SDK from the VS 2017 Installation Manager).
      * Visual Studio 2019
      
In addition, you will need 

1) At least 20 GB of free disk space.
2) An installation of the [Boost](http://www.boost.org/) library (version 1.68 or above)
3) The following environment variables must be set before compilation (if using the Visual Studio IDE, these variables must be set before starting the IDE):

*    BOOST_INCLUDE_DIR
*    BOOST_LIBRARY_DIR_32
*    BOOST_LIBRARY_DIR_64

The **BOOST_INCLUDE_DIR** should point to your boost installation folder.  This is usually **C:\boost_install_directory**, where this folder contains a subsequent **boost** folder, containing the **boost** header files.  For example:

```plaintext
SET BOOST_INCLUDE_DIR=C:\boost_installation\boost
```



The **BOOST_LIBRARY_DIR_32** is the directory where the 32-bit boost libraries are installed.  These libraries would be named similar to **libboost_xxxxxxxx-vcyyy-zzzz-x32.lib**.  For example:

```plaintext
SET BOOST_LIBRARY_DIR_32=C:\boost_installation\lib32-msvc-14.0
```



The **BOOST_LIBRARY_DIR_64** is the directory where the 64-bit boost libraries are installed.  These libraries would be named similar to **libboost_xxxxxxxx-vcyyy-zzzz-x64.lib**.  For example:

```plaintext
SET BOOST_LIBRARY_DIR_32=C:\boost_installation\lib64-msvc-14.0
```

   
If you are using Visual C++, I recommend getting the pre-built boost libraries found at [SourceForge](https://sourceforge.net/projects/boost/files/boost-binaries/).  When installed, you will get the boost **include** files, plus the library files.  Please download the version of boost that fits your compiler.  

```plaintext
For Visual C++ 2015 -- Download files with "14.0" in the file name.
For Visual C++ 2017 -- Download files with "14.1" in the file name.
For Visual C++ 2019 -- Download files with "14.2" in the file name.
```



If you are not using Visual C++, you will need to build the following boost libraries:  
* chrono
* date_time
* filesystem
* log
* system
* thread
----------
Note that the C++ source code should be able to be built with any C++11 or C++14 compliant compiler that recognizes the Windows API headers (MingW using g++ 5.0 or above is an example).  However we have not tested builds of the DTWAIN library that have been built with any other compiler other than the Visual Studio family.   

* Start Visual Studio, and open one of the DTWAIN solution.  The DTWAIN solution files are found in the [source](https://github.com/dynarithmic/twain_library/tree/master/source) directory.  Open **dtwain_5_vs2015.sln** or **dtwain_5_vs2017.sln**, depending on whether you are using Visual Studio 2015 or 2017, respectively. 

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

