### Demo programs ###

The demo programs consist of Visual Studio 2019 projects.  The projects are C and C++ projects, with an additional C#, Visual Basic, and Delphi project.  The Delphi project was tested with the <a href="https://www.lazarus-ide.org/" target="_blank">Lazarus Delphi</a> environment.

----
### C# demo
It is recommended to load the **CSharp_FullDemo.csproj** file found <a href="https://github.com/dynarithmic/twain_library/tree/master/demos/csharp/Examples/FullDemo" target="_blank">here</a> into Visual Studio 2019, or above.  Then a rebuild will generate the demo executable.

----
### Visual Basic demo
It is recommended to load the **VB_FullDemo.sln** file found <a href="https://github.com/dynarithmic/twain_library/tree/master/demos/VisualBasic/Examples/FullDemo" target="_blank">here</a> into Visual Studio 2019, or above.  Then a rebuild will generate the demo executable.

----

### C++ demos

It is recommended to load the **AllDemos.sln** file into Visual Studio 2019, or above.  Then a rebuild of the projects should be initiated by **Build -> Batch Build...** and then selecting all of the projects.

Please note that the C++ wrapper by default uses **C++17** as the language setting, since the wrapper uses various types and classes that only exist in  C++17 or higher.

It is possible to use **C++14**, but this requires an installation of the <a href="https://www.boost.org/" target="_blank">Boost library</a> (version 1.70 or higher).  The environment variable **BOOST_INCLUDE_DIR** should be defined before starting Visual Studio. This environment variable should point to the root directory where the Boost header files are located.  For example:

SET BOOST_INCLUDE_DIR = c:\boost_installation

and the directory c:\boost_installation should have a folder called **boost** which would be the root of the  header files.

----
### Running the demos

1) Make sure that the [DTWAIN DLL's](https://github.com/dynarithmic/twain_library/tree/master/binaries) are available (system PATH, your exe directory, etc.) when the demo program you are running starts.  A missing DLL will result in a "Dynamic Link Library not found" error at runtime.

2) Make sure the files found in <a href="https://github.com/dynarithmic/twain_library/tree/master/text_resources" target="_blank">text_resources</a> are available in the same directory as the DTWAIN DLL's.  Missing text resources will result in a "DTWAIN Resources not found" error.


----
### Why not have the demo projects already pre-built?  Why do I need to build them myself?
It has come to our attention that some of the executables, if prebuilt, generate false positive warnings on a select few virus checkers.  The reason why you should rebuild the demo applications yourself is to be assured that you are not running a virus (even though it is a false positive).  

The DTWAIN libraries themselves, dtwain32.dll, dtwain32u.dll, etc. are always checked for false positives on all the major virus checkers (using <a href="https://www.virustotal.com/gui/home/upload" target="_blank">Virus Total</a> as the baseline).  If any of the following DLL's produce any issues containing false positives, we resolve these issues before we make the DLL's available on the master branch.

* dtwain32.dll
* dtwain32u.dll
* dtwain32d.dll
* dtwain32ud.dll
* dtwain64.dll
* dtwain64u.dll
* dtwain64d.dll
* dtwain64ud.dll

The files that end with **d**, for example, dtwain32d.dll, are special debug versions of the DLL, and are available in the <a href="https://github.com/dynarithmic/twain_library_source/tree/main/binaries" target="_blank">twain_library_source repo</a>.