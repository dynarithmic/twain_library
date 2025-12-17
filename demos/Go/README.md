The language binding for Go will only support the following DTWAIN DLL's loaded at runtime:

dtwain32u.dll
dtwain32ud.dll
dtwain64u.dll
dtwain64ud.dll

Basically, only the Unicode versions of DTWAIN are compatible with the Go language binding.  
Usage of the ANSI DLL's (dtwain32.dll, dtwain64.dll, etc.) will result in some DTWAIN function calls 
failing that handle string parameters.

----
Here is a Go example using the  [dtwainapi.go](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/Go) module that defines the DTWAIN constants and functions.  

The program gives an example of selecting a TWAIN device installed on your system, getting the values defined by the ICAP_PIXELTYPE TWAIN capability, displaying a list of all the capabilities available to the device, and acquiring from the TWAIN device and saved as a BMP image.

```go
package main

import "C"
import (
    "dtwainapi"
    "fmt"
    "unsafe"
)

func main() {
    var filename string

    // Load 64-bit or 32-bit DLL, depending on the Golang environment
    if unsafe.Sizeof(uintptr(0)) == 8 {
        filename = "dtwain64u.dll"
    } else {
        filename = "dtwain32u.dll"
    }

    // Load the DLL and resolve all function pointers
    api_func, err := dtwainapi.Load_DTWAINDLL(filename)
    if err != nil {
        panic(err)
    }
    defer api_func.Close()

    // Initialize DTWAIN
    isInitialized := api_func.DTWAIN_SysInitialize()
    if isInitialized == dtwainapi.DTWAIN_HANDLE(nil) {
        panic(err)
    }
    defer api_func.DTWAIN_SysDestroy()
    api_func.DTWAIN_EnableMsgNotify(1)

    // Select a source
    twain_source := api_func.DTWAIN_SelectSource()
    if twain_source == dtwainapi.DTWAIN_SOURCE(nil) {
        fmt.Println("Source was not selected")
    } else {
        fmt.Println("Source was selected")

        // Get the product name of the source
        productname := make([]byte, 256)
        api_func.DTWAIN_GetSourceProductNameA(twain_source, productname, 256)
        fmt.Printf("Product Name: %s\n", dtwainapi.GetNullTerminatedString(productname))

        // Test DTWAIN_GetCapValues
        var pixelType dtwainapi.DTWAIN_ARRAY = nil
        api_func.DTWAIN_GetCapValues(twain_source, dtwainapi.DTWAIN_CV_ICAPPIXELTYPE, dtwainapi.DTWAIN_CAPGET, &pixelType)
        pCount := api_func.DTWAIN_ArrayGetCount(pixelType)
        fmt.Printf("Array Count for pixel type is : %d\n", pCount)

        var index int32
        var pixelTypeValue int32
        for index = 0; index < pCount; index++ {
            api_func.DTWAIN_ArrayGetAtLong(pixelType, index, &pixelTypeValue)
            fmt.Printf("Pixel type %d has a value of %d\n", index+1, pixelTypeValue)
        }

        // Get the capabilities and list them
        allCaps := api_func.DTWAIN_EnumSupportedCapsEx2(twain_source)
        allCaps_Size := api_func.DTWAIN_ArrayGetCount(allCaps)
        fmt.Printf("All Caps: %d\n", allCaps_Size)

        arrInt32 := int32(0)
        buf := make([]byte, 256) // big enough

        for index = 0; index < allCaps_Size; index++ { // init; condition; post
            api_func.DTWAIN_ArrayGetAtLong(allCaps, index, &arrInt32)
            api_func.DTWAIN_GetNameFromCapA(arrInt32, buf, 256)
            str := dtwainapi.GetNullTerminatedString(buf)
            fmt.Printf("Capability %d: %s   Value: %d\n", index+1, str, arrInt32)
        }

        // Acquire to bmp file
        // We will use the Unicode version of the function (DTWAIN_AcquireFileW) that takes
        // a wide character file name.
        // We could have also used DTWAIN_AcquireFileA and just passed a regular Go string,
        // so this is really only for demo purposes to show how to pass immutable wide strings
        // to the DTWAIN API
        api_func.DTWAIN_AcquireFileW(twain_source,
            dtwainapi.StringToUTF16("gotest.bmp"), // This is a helper function that creates a wide string
            dtwainapi.DTWAIN_BMP,
            dtwainapi.DTWAIN_USELONGNAME,
            dtwainapi.DTWAIN_PT_DEFAULT,
            1,
            1,
            1,
            &arrInt32)
   }
}
```


