### Installation instructions for the 32-bit TWAIN device

Note:  You may need Administrator Rights on your system to get write access to the directories below.

To install, 

1) Go to the Windows system directory (usually C:\Windows).  There should be a **twain_32** directory defined there.   Note:  All systems running a modern version of the Windows operating system should already have the **twain_32** directory existing.

2) Copy the contents of the [twain32 folder](https://github.com/dynarithmic/twain_library/tree/master/SampleTWAINDevices/twain_32) to the C:\Windows\twain_32 directory.

3) To check the installation, you can run the DTWDEMO32.exe program [found here](https://github.com/dynarithmic/dtwain/tree/dtwain_apache/binaries), and see if the scanner shows up in the "Select Source" dialog when selecting a Source.


----------

### Installation instructions for the 64-bit TWAIN device

Note:  You may need Administrator Rights to get write access to the directories mentioned below.

To install, 

1) Go to the Windows system directory (usually C:\Windows).  There should be a **twain_64** directory. If there is no directory named twain_64, create the directory (so once done, there will be a C:\Windows\twain_64 directory), and add **C:\Windows\twain_64** to the system PATH.

2) Once the twain_64 directory exists, copy the contents of the [twain64 folder](https://github.com/dynarithmic/twain_library/tree/master/SampleTWAINDevices/twain_64) to the C:\Windows\twain_64 directory.

3) Check in the C:\Windows directory for a file named **TWAINDSM.DLL**.  If the file does not exist, copy the version found here in the twain_64 directory to C:\Windows

4) To check the installation, you can run the DTWDEMO64.exe program [found here](https://github.com/dynarithmic/dtwain/tree/dtwain_apache/binaries), and see if the scanner shows up in the "Select Source" dialog when selecting a Source. 


