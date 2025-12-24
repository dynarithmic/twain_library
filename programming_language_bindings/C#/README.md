There are two basic methods of using the DTWAIN library within a C# application.  Basically, the two methods are as follows:

[Method 1:](#use_method_1) -- Have the program automatically load the appropriate DTWAIN DLL (dtwain32u.dll, dtwain32.dll, dtwain64u.dll, dtwain64.dll, etc.) depending on the language binding source file you have added to your project.  

[Method 2:](#use_method_2) -- At runtime, your application controls which one of the DTWAIN DLL's listed above is loaded (dynamically load DTWAIN at runtime).

----

<a name="use_method_1">

###### Using Method 1:

Use Method 1 above if

a) You are using a C# version less than 9.0

or

b) You are certain which DTWAIN DLL you want to be used for your application, and you are relying on the Windows operating to find the DLL's at runtime without your application having to specify the exact location of where the DLL will be found.  

Note that the DTWAIN DLL must be located where the [Windows DLL search logic](https://learn.microsoft.com/en-us/windows/win32/dlls/dynamic-link-library-search-order) will be used to find DLL's (which in simpler terms means that the DTWAIN DLL can be placed in the current executable directory, or a directory specified by the system PATH environment variable).  

In this case, you would add to your C# project one of the files below, depending on the DTWAIN DLL and whether to use the Unicode version of the library:

| C# interface file name       | Application type and DTWAIN DLL runtime       |
|----------------|----------------|
| dtwain32.cs | 32-bit ANSI using dtwain32.dll |
| dtwain32d.cs | 32-bit Debug ANSI using dtwain32d.dll |
| dtwain32u.cs | 32-bit Unicode using dtwain32u.dll |
| dtwain32ud.cs | 32-bit Debug Unicode using dtwain32u.dll |
| dtwain64.cs | 64-bit ANSI using dtwain64.dll |
| dtwain64d.cs | 64-bit Debug ANSI using dtwain64d.dll |
| dtwain64u.cs | 64-bit Unicode using dtwain64u.dll |
| dtwain64ud.cs | 64-bit Debug Unicode using dtwain64ud.dll |

**Additional note: It is highly suggested to use the Unicode version of DTWAIN for C# applications.**

Here is a bare-bones C# language example of selecting a TWAIN device, displaying the capabilities available on the device, and acquiring a BMP image from the TWAIN device.  The only additional requirement is to add one of the dtwain*.cs files mentioned above to your project:

```csharp
using System;
using System.Text;
// One of the additional dtwain*.cs file needs to be added to your project for these definitions.
using Dynarithmic; 
using DTWAIN_ARRAY = System.IntPtr;

namespace Test
{    
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize DTWAIN
            var TwainHandle = TwainAPI.DTWAIN_SysInitialize();
            if (TwainHandle == IntPtr.Zero)
                Console.WriteLine("TWAIN Failed to be initialized.  Exiting...");
            else
            {
                // Select a TWAIN Source from the TWAIN Dialog
                var SelectedSource = TwainAPI.DTWAIN_SelectSource();
                if (SelectedSource != IntPtr.Zero)
                {
                    // Display the product name of the Source
                    StringBuilder szInfo = new StringBuilder(256);
                    TwainAPI.DTWAIN_GetSourceProductNameA(SelectedSource, szInfo, 256);
                    Console.WriteLine("The source product name is " + szInfo.ToString());

                    // Get the capabilities the device supports
                    DTWAIN_ARRAY dtwain_array = IntPtr.Zero;
                    TwainAPI.DTWAIN_EnumSupportedCaps(SelectedSource, ref dtwain_array);

                    // Get the number of items in the array
                    int arrcount = TwainAPI.DTWAIN_ArrayGetCount(dtwain_array);
                    Console.WriteLine("There are " + arrcount + " device capabilities");

                    // Print each capability
                    for (int curCap = 1; curCap <= arrcount; ++curCap)
                    {
                        int int_val = 0;
                        
                        // Note that LONG values in the DTWAIN API are 32-bit integers.
                        TwainAPI.DTWAIN_ArrayGetAtLong(dtwain_array, curCap-1, ref int_val);
                        TwainAPI.DTWAIN_GetNameFromCapA(int_val, szInfo, 256);
                        Console.WriteLine("Capability " + curCap + ": " + szInfo.ToString() + "  Value: " +                     int_val);
                    }

                    int status = 0;
                    // Acquire the BMP file named Test.bmp
                    TwainAPI.DTWAIN_AcquireFile(SelectedSource,
                                                "Test.bmp",
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
        }
    }
}
```
----
<a name="use_method_2">

###### Using Method 2:

Use Method 2 if: 

   a) You are using C# 9.0 or greater

   and
   
   b) Your application is Unicode only,  

   and
   
   c) You want to load the DTWAIN DLL at runtime, where your application can control exactly which DTWAIN DLL is being loaded, as well as where to find the DTWAIN DLL (in other words, your application is not relying solely on the Windows DLL search logic to find the DLL)

In this case, you would add to your C# project the interface file **dtwain_dynamic.cs** to your project.

Here is a bare-bones C# language example of selecting a TWAIN device, displaying the capabilities available on the device, and acquiring a BMP image from the TWAIN device.  The only additional requirement is to add the **dtwain_dynamic.cs** file mentioned above to your project:

```csharp
using System;
using System.Text;
// The additional dtwain_dynamic.cs file needs to be added to your project for these definitions.
using Dynarithmic;
using DTWAIN_ARRAY = System.IntPtr;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load either the 32-bit or 64-bit version of DTWAIN
            // depending on the application type
            string defaultLib = "dtwain32u.dll";
            if (IntPtr.Size == 8)
                defaultLib = "dtwain64u.dll";

            // This will throw an exception if the DLL cannot
            // be loaded, or if one or more DLL function(s) cannot be resolved
            // within the DLL
            var TwainAPI = new Dynarithmic.TwainAPI(defaultLib);

            var TwainHandle = TwainAPI.DTWAIN_SysInitialize();
            if (TwainHandle == IntPtr.Zero)
                Console.WriteLine("TWAIN Failed to be initialized.  Exiting...");
            else
            {
                // Select a TWAIN Source from the TWAIN Dialog
                var SelectedSource = TwainAPI.DTWAIN_SelectSource();
                if (SelectedSource != IntPtr.Zero)
                {
                    // Display the product name of the Source
                    StringBuilder szInfo = new StringBuilder(256);
                    TwainAPI.DTWAIN_GetSourceProductNameA(SelectedSource, szInfo, 256);
                    Console.WriteLine("The source product name is " + szInfo.ToString());

                    // Get the capabilities the device supports
                    DTWAIN_ARRAY dtwain_array = IntPtr.Zero;
                    TwainAPI.DTWAIN_EnumSupportedCaps(SelectedSource, ref dtwain_array);

                    // Get the number of items in the array
                    int arrcount = TwainAPI.DTWAIN_ArrayGetCount(dtwain_array);
                    Console.WriteLine("There are " + arrcount + " device capabilities");

                    // Print each capability
                    for (int curCap = 1; curCap <= arrcount; ++curCap)
                    {
                        int int_val = 0;

                        // Note that LONG values in the DTWAIN API are 32-bit integers.
                        TwainAPI.DTWAIN_ArrayGetAtLong(dtwain_array, curCap - 1, ref int_val);
                        TwainAPI.DTWAIN_GetNameFromCapA(int_val, szInfo, 256);
                        Console.WriteLine("Capability " + curCap + ": " + szInfo.ToString() + "  Value: " + int_val);
                    }

                    int status = 0;
                    // Acquire the BMP file named Test.bmp
                    TwainAPI.DTWAIN_AcquireFile(SelectedSource,
                                                "Test.bmp",
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
        }
    }
}
```
