### Demo programs ###

The demo programs consist of Visual Studio 2019 projects.  The projects are C and C++ projects, with an additional C# project.

It is recommended to load the **AllDemos.sln** file into Visual Studio 2019, or above.  Then a rebuild of the projects should be initiated by **Build -> Batch Build...** and then selecting all of the projects.

----
### Building the C++ demos that use the C++ wrapper library

Please note that the C++ wrapper projects require an installation of the <a href="https://www.boost.org/" target="_blank">Boost library</a> (version 1.70 or higher), since the wrapper uses some Boost components.  

In addition, the environment variable **BOOST_INCLUDE_DIR** should be defined before starting Visual Studio. This environment variable should point to the root directory where the Boost header files are located.  For example:

SET BOOST_INCLUDE_DIR = c:\boost_installation

and the directory c:\boost_installation should have a folder called **boost** which would be the root of the  header files.

----
### Why not have the demo projects already pre-built?  Why do I need to build them myself?
It has come to our attention that some of the executables, if prebuilt, generate false positive warnings on a select few virus checkers.  The reason why you should rebuild the demo applications yourself is to be assured that you are not running a virus (even though it is a false positive).  

The DTWAIN libraries themselves, dtwain32.dll, dtwain32u.dll, etc. are always checked for false positives on all the major virus checkers (using <a href="https://www.virustotal.com/gui/home/upload" target="_blank">Virus Total</a> as the baseline).  If any of the following DLL's produce any issues containing false positives, we resolve these issues before we make the DLL's available on the master branch.

* dtwain32.dll
* dtwain32u.dll
* dtwain64.dll
* dtwain64u.dll
