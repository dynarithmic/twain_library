This directory contains additional language resources that can be used by DTWAIN.  

--------------------------

If you want to create your own language resource, the following steps should be done:

1) Make a copy of **twainresourcestrings_english.txt**, and name the new file **twainresourcestrings_aaa.txt**

2) Edit **twainresourcestrings_aaa.txt** using any text editor and change the English text to your language's equivalent wording.  Save the changes.

3) This step is very important:  Rename the **twainresourcestrings_aaa.txt** by changing the "aaa" portion of the file name to something meaningful.  For example, if the new file represents Greek resources, a good name would be:

    **twainresourcestrings_greek.txt**

The part of the file name that represents the language (in the above case, "greek"), will be used in the DTWAIN API call **DTWAIN_LoadCustomStringResourcesA**

4) Make sure that the new resource file is located in the same directory as the other DTWAIN resources, i.e. where **twainresourcestrings_english.txt** is located during the running of your DTWAIN application.

5) Change your DTWAIN application to add the following API call (after DTWAIN_SysInitialize is called):

    **DTWAIN_LoadCustomStringResourcesA("greek")**

Note that the name parameter used in DTWAIN_LoadCustomStringResources is the same name you gave the resource file (the part of the file name after the initial "_" character in the file name).

---------------------------------

If everything goes well, you can test if the custom language resource has been loaded by calling <a href="http://www.dynarithmic.com/onlinehelp5/dtwain/dtwain_settwainlog.htm" target="_blank">DTWAIN_SetTwainLog</a> and checking if the logging information shows up in the desired language.

---------------------------------
In general, the name of the new resource file will be of the form:  
  
           twainresourcestrings_somename.txt

where **somename** can be any string, and that string will be used in the DTWAIN_LoadCustomStringResourcesA function call.

-----------------------
### Can we submit language resources to you to place in the Git repository?

**Of course you can!**

Open an <a href="https://github.com/dynarithmic/twain_library/issues" target="_blank">Issue</a>, attach the new resource file to the issue, and we will be happy to review it and place it in the repository for others to use.
