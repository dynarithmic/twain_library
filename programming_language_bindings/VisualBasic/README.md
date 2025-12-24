There are two basic methods of using the DTWAIN library within a Visual Basic application.  Basically, the two methods are as follows:

[Method 1:](#use_method_1) -- Have the program automatically load the appropriate DTWAIN DLL (dtwain32u.dll, dtwain32.dll, dtwain64u.dll, dtwain64.dll, etc.) depending on the language binding source file you have added to your project.  

[Method 2:](#use_method_2) -- At runtime, your application controls which one of the DTWAIN DLL's listed above is loaded (dynamically load DTWAIN at runtime).

----

<a name="use_method_1">

###### Using Method 1:

Use Method 1 if you are certain which DTWAIN DLL you want to be used for your application, and you are relying on the Windows operating to find the DLL's at runtime without your application having to specify the exact location of where the DLL will be found.  

Note that the DTWAIN DLL must be located where the [Windows DLL search logic](https://learn.microsoft.com/en-us/windows/win32/dlls/dynamic-link-library-search-order) will be used to find DLL's (which in simpler terms means that the DTWAIN DLL can be placed in the current executable directory, or a directory specified by the system PATH environment variable).  

In this case, you would add to your Visual Basic project one of the files below, depending on the DTWAIN DLL and whether to use the Unicode version of the library:

| Visual Basic interface file name       | Application type and DTWAIN DLL runtime       |
|----------------|----------------|
| dtwain32.vb | 32-bit ANSI using dtwain32.dll |
| dtwain32d.vb | 32-bit Debug ANSI using dtwain32d.dll |
| dtwain32u.vb | 32-bit Unicode using dtwain32u.dll |
| dtwain32ud.vb | 32-bit Debug Unicode using dtwain32u.dll |
| dtwain64.vb | 64-bit ANSI using dtwain64.dll |
| dtwain64d.vb | 64-bit Debug ANSI using dtwain64d.dll |
| dtwain64u.vb | 64-bit Unicode using dtwain64u.dll |
| dtwain64ud.vb | 64-bit Debug Unicode using dtwain64ud.dll |

**Additional note: It is highly suggested to use the Unicode version of DTWAIN for Visual Basic applications.**

Here is a bare-bones Visual Basic example of selecting a TWAIN device, displaying the capabilities available on the device, and acquiring a BMP image from the TWAIN device.  The only additional requirement is to add one of the dtwain*.vb files mentioned above to your project:

```vbnet
Imports System.Text
Imports DTWAINAPI
Imports DTWAIN_ARRAY = System.IntPtr

Module Module1

    Sub Main()
        Dim TwainHandle = DTWAINAPI.DTWAIN_SysInitialize()
        If TwainHandle = IntPtr.Zero Then
            Console.WriteLine("TWAIN Failed to be initialized.  Exiting...")
        Else
            ' Select a TWAIN Source from the TWAIN Dialog
            Dim SelectedSource = DTWAINAPI.DTWAIN_SelectSource()
            If SelectedSource <> IntPtr.Zero Then
                ' Display the product name of the Source
                Dim szInfo As New StringBuilder(256)
                DTWAINAPI.DTWAIN_GetSourceProductNameA(SelectedSource, szInfo, 256)
                Console.WriteLine("The source product name is " + szInfo.ToString())

                ' Get the capabilities the device supports
                Dim dtwain_array As DTWAIN_ARRAY = IntPtr.Zero
                DTWAINAPI.DTWAIN_EnumSupportedCaps(SelectedSource, dtwain_array)

                ' Get the number of items in the array
                Dim arrcount As Integer = DTWAINAPI.DTWAIN_ArrayGetCount(dtwain_array)
                Console.WriteLine("There are " & arrcount & " device capabilities")

                ' Print each capability
                For curCap As Integer = 1 To arrcount
                    Dim int_val As Integer
                    ' Note that LONG values in the DTWAIN API are 32-bit integers.
                    DTWAINAPI.DTWAIN_ArrayGetAtLong(dtwain_array, curCap - 1, int_val)
                    DTWAINAPI.DTWAIN_GetNameFromCapA(int_val, szInfo, 256)
                    Console.WriteLine("Capability " & curCap & ": " & szInfo.ToString() & "  Value: " & int_val)
                Next

                Dim status As Integer = 0
                ' Acquire the BMP file named Test.bmp
                DTWAINAPI.DTWAIN_AcquireFile(SelectedSource,
                                             "Test.bmp",
                                              DTWAINAPI.DTWAIN_BMP,
                                              DTWAINAPI.DTWAIN_USENATIVE Or DTWAINAPI.DTWAIN_USENAME,
                                              DTWAINAPI.DTWAIN_PT_DEFAULT,
                                              1,
                                              1,
                                              1,
                                              status)
                DTWAINAPI.DTWAIN_SysDestroy()
            End If
        End If
    End Sub
End Module
```

----
<a name="use_method_2">

###### Using Method 2:

Use Method 2 if: 

   a) Your application is Unicode only,  

   and
   
   b) You want to load the DTWAIN DLL at runtime, where your application can control exactly which DTWAIN DLL is being loaded, as well as where to find the DTWAIN DLL (in other words, your application is not relying solely on the Windows DLL search logic to find the DLL)

In this case, you would add to your Visual Basic project the interface file **dtwain_dynamic.vb** to your project.

Here is a bare-bones Visual Basic example of selecting a TWAIN device, displaying the capabilities available on the device, and acquiring a BMP image from the TWAIN device.  The only additional requirement is to add the **dtwain_dynamic.vb** file mentioned above to your project:

```vbnet
Imports System.Text
Imports Dynarithmic
Imports DTWAIN_ARRAY = System.IntPtr

Module Module1

    Sub Main()
        Dim DTWAINAPI As Dynarithmic.DTWAINAPI = Nothing

        ' Load the 32-bit or 64-bit DLL, depending on the application type
        Try
            If IntPtr.Size = 8 Then
                DTWAINAPI = New DTWAINAPI("dtwain64u.dll")
            Else
                DTWAINAPI = New DTWAINAPI("dtwain32u.dll")
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message & " -- TWAIN Failed to be initialized.  Exiting...")
            Environment.Exit(0)
        End Try

        Dim TwainHandle = DTWAINAPI.DTWAIN_SysInitialize()
        If TwainHandle = IntPtr.Zero Then
            Console.WriteLine("TWAIN Failed to be initialized.  Exiting...")
        Else
            ' Select a TWAIN Source from the TWAIN Dialog
            Dim SelectedSource = DTWAINAPI.DTWAIN_SelectSource()
            If SelectedSource <> IntPtr.Zero Then
                ' Display the product name of the Source
                Dim szInfo As New StringBuilder(256)
                DTWAINAPI.DTWAIN_GetSourceProductNameA(SelectedSource, szInfo, 256)
                Console.WriteLine("The source product name is " + szInfo.ToString())

                ' Get the capabilities the device supports
                Dim dtwain_array As DTWAIN_ARRAY = IntPtr.Zero
                DTWAINAPI.DTWAIN_EnumSupportedCaps(SelectedSource, dtwain_array)

                ' Get the number of items in the array
                Dim arrcount As Integer = DTWAINAPI.DTWAIN_ArrayGetCount(dtwain_array)
                Console.WriteLine("There are " & arrcount & " device capabilities")

                ' Print each capability
                For curCap As Integer = 1 To arrcount
                    Dim int_val As Integer
                    ' Note that LONG values in the DTWAIN API are 32-bit integers.
                    DTWAINAPI.DTWAIN_ArrayGetAtLong(dtwain_array, curCap - 1, int_val)
                    DTWAINAPI.DTWAIN_GetNameFromCapA(int_val, szInfo, 256)
                    Console.WriteLine("Capability " & curCap & ": " & szInfo.ToString() & "  Value: " & int_val)
                Next

                Dim status As Integer = 0
                ' Acquire the BMP file named Test.bmp
                DTWAINAPI.DTWAIN_AcquireFile(SelectedSource,
                                             "Test.bmp",
                                              DTWAINAPI.DTWAIN_BMP,
                                              DTWAINAPI.DTWAIN_USENATIVE Or DTWAINAPI.DTWAIN_USENAME,
                                              DTWAINAPI.DTWAIN_PT_DEFAULT,
                                              1,
                                              1,
                                              1,
                                              status)
                DTWAINAPI.DTWAIN_SysDestroy()
            End If
        End If
        ' Free the library
        DTWAINAPI.Dispose()
    End Sub
End Module
```
