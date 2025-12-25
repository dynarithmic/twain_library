### Demo programs ###

The demo programs for C, C#, F#, and Visual Basic consist of Visual Studio 2019 projects and for Delphi, a simple [Delphi project](https://github.com/dynarithmic/twain_library/tree/master-staging/demos/Delphi/SimpleAcquire).  

----
The demo programs for the other languages besides C/C++, C#, F#, Visual Basic, and Delphi, consists of only the DTWAIN language binding file(s), plus a main demo program showing just *some* of the features of DTWAIN, but enough to get you started on using DTWAIN with your development language.  

Please note that you must already be familiar with setting up projects, directories, etc, in your development environment, as giving tutorials on this topic is beyond the scope of the demos presented here.

----
### <a name="build-demo"></a> Building the demo applications
##### C++

If you wish to build the C and C++ demo applications, the **demos\C_C++\AllDemos.sln** file can be loaded into Visual Studio 2019 or 2022.  Please note that you must build the base libraries first (by building using the **dtwain_5_vs2019.sln** project, mentioned above) before building the demos.  The demos consist of C and C++ language demos, plus C++ demos based on an experimental C++ wrapper library that is currently being developed.

Please note that the C++ wrapper by default uses **C++17** as the language setting, since the wrapper uses various types and classes that only exist in  C++17 or higher.

It is possible to use **C++14**, however this requires an installation of the <a href="https://www.boost.org/" target="_blank">Boost library</a> (version 1.70 or higher).  The environment variable **BOOST_INCLUDE_DIR** should be defined before starting Visual Studio. This environment variable should point to the root directory where the Boost header files are located.  For example:

SET BOOST_INCLUDE_DIR = c:\boost_installation

and the directory c:\boost_installation should have a folder called **boost** which would be the root of the  header files.

----
###### C#

The C# demo is **demos\C#\Examples\FullDemo\CSharp_FullDemo.csproj**
This project is by default, setup for 32-bit Unicode (the dtwain32u.cs is part of the projects).  If you want to try 64-bit builds, please replace the dtwain32u.cs with one of the 64-bit .cs files (for example dtwain64u.cs).

----
###### Visual Basic

The Visual Basic demo is **demos\VisualBasic\Examples\FullDemo\VB_FullDemo.vbproj**
This project is by default, setup for 32-bit Unicode (the dtwain32u.vb is part of the projects).  If you want to try a 64-bit builds, please replace the dtwain32u.vb with one of the 64-bit .cs files (for example dtwain64u.vb).

----
###### Other languages
As noted above, you will need to set up your environment to include the interface files for the language you are using.  

For example, if you use Visual Studio Code to develop in Go or Lua, you will need to set up your projects to include the interface file(s) (in these cases, for Go, you would need to add dtwainapi.go to your project, for Lua, the file would be dtwainapi.lua).  

----
### Running the demos

1) Make sure that the [DTWAIN DLL's](https://github.com/dynarithmic/twain_library/tree/master/binaries) are available (system PATH, your exe directory, etc.) when the demo program you are running starts.  A missing DLL will result in a "Dynamic Link Library not found" error at runtime.

2) Make sure the files found in <a href="https://github.com/dynarithmic/twain_library/tree/master/text_resources" target="_blank">text_resources</a> are available in the same directory as the DTWAIN DLL's.  Missing text resources will result in a "DTWAIN Resources not found" error.

