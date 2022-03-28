Before opening the project solution, you must set these environment variables to the appropiate path

JDK_INCLUDE
DTWAIN_INCLUDE
DTWAIN_LIB_PATH

The JDK_INCLUDE is the path to the Java JDK include directory.  This directory will be found in your JDK installation, and contains such files as jni.h and should 
contain a "win32" folder that contains additional include files.  Note that there should be no trailing backslash in this name. 

For example:
JDK_INCLUDE=C:\Java\JDK1.8\include


The DTWAIN_INCLUDE is the path to the DTWAIN header files.  This directory is usually in the source\h directory of the DTWAIN repository you either cloned or downloaded:

For example:
DTWAIN_INCLUDE=C:\Github\Repo\Dtwain_5\source\h


The DTWAIN_LIB_PATH is the path to the DTWAIN import library files.  This directory is usually in the binaries directory of the DTWAIN repository you either cloned or downloaded.
This directory must have sub directories named 32bit and 64bit, denoting the bitness of the import library in those directories.

For example:
DTWAIN_LIB_PATH=C:\Github\Repo\Dtwain_5\binaries

