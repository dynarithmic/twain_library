### Installation instructions for the 32-bit TWAIN device

Note:  You may need Administrator Rights on your system to get write access to the directories below.

1) Run the 32-bit installer, [twainds.win32.installer.msi](https://github.com/dynarithmic/twain_library/blob/master/SampleTWAINDevices/twain_32/twainds.win32.installer.msi).  

2) Go to the Windows system directory (usually C:\Windows).  There should be a **twain_32** directory defined there.   Note:  All systems running a modern version of the Windows operating system should already have the **twain_32** directory existing.

3) There should be a subfolder with the name of **sample2** that was created.  In the **sample2** folder should be a file with a **.ds** extension.  This is the main file that the TWAIN Data Source Manager will communicate with when querying the installed TWAIN devices.

4) To check the installation, you can run the DTWDEMO32.exe program [found here](https://github.com/dynarithmic/dtwain/tree/dtwain_apache/binaries), and see if the scanner shows up in the "Select Source" dialog when selecting a Source.

----------

### Installation instructions for the 64-bit TWAIN device

1) Run the 64-bit installer, [twainds.win64.installer.msi](https://github.com/dynarithmic/twain_library/blob/master/SampleTWAINDevices/twain_64).  

2) Go to the Windows system directory (usually C:\Windows).  There should be a **twain_64** directory. If there is no directory named twain_64, create the directory (so once done, there will be a C:\Windows\twain_64 directory), and add **C:\Windows\twain_64** to the system PATH.

3) There should be a subfolder with the name of **sample2** that was created.  In the **sample2** folder should be a file with a **.ds** extension.  This is the main file that the TWAIN Data Source Manager will communicate with when querying the installed TWAIN devices.

4) Check in the C:\Windows or C:\Windows\System32 directory for a file named **TWAINDSM.DLL**.  If the file does not exist, copy the version found here in the twain_64 directory to C:\Windows

5) To check the installation, you can run the DTWDEMO64.exe program [found here](https://github.com/dynarithmic/dtwain/tree/dtwain_apache/binaries), and see if the scanner shows up in the "Select Source" dialog when selecting a Source. 


