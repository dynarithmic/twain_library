open System
open dtwainapi
open System.Text

[<EntryPoint>]
let main argv =
    let dllname =
        if Environment.Is64BitProcess then
            "dtwain64u.dll"
        else
            "dtwain32u.dll"

    // Load the DLL first — required before any DTWAIN calls
    TwainAPI.Load dllname
    printfn "DTWAIN DLL loaded successfully."

    let exitCode =
        try
            let initResult = TwainAPI.DTWAIN_SysInitialize()
            if initResult = 0 then 
                printfn "No source was selected" 
                1
            else
                // Select a Source using the enhanced "Select Source" dialog.  We will center it
                // on the screen
                let sourceResult = TwainAPI.DTWAIN_SelectSource2 IntPtr.Zero "Select Source" 0 0 TwainAPI.DTWAIN_DLG_CENTER_SCREEN 
                
                if sourceResult = 0 then 
                    printfn "No TWAIN Source was selected"
                    TwainAPI.DTWAIN_SysDestroy() |> ignore
                    1
                else
                    // This will allow callbacks to be invoked by DTWAIN
                    TwainAPI.DTWAIN_EnableMsgNotify 1 |> ignore   

                    // Now get the product name of the TWAIN source that was selected
                    let buffer = new StringBuilder(256)
                    let ret = TwainAPI.DTWAIN_GetSourceProductNameW sourceResult buffer 256
                    printfn "The name of the selected TWAIN Source is: %s" (buffer.ToString())

                    // Example usage of DTWAIN_ARRAY:
                    // Get the device capabilities supported by the device
               
                    // Note: The DTWAIN_ARRAY, DTWAIN_SOURCE, DTWAIN_FRAME, and DTWAIN_RANGE are actually void pointers
                    // so you have to declare them as IntPtr.Zero if a DTWAIN function requires a parameter to be of this type.
                    let mutable cap_array = TwainAPI.DTWAIN_EnumSupportedCapsEx2 sourceResult 

                    // Get the number of items in the array
                    let mutable arrcount = TwainAPI.DTWAIN_ArrayGetCount cap_array
                    printfn "There are %d capabilities defined for device %s" (arrcount) (buffer.ToString())

                    // print each capability
                    let mutable long_val = 0
                    for i = 1 to arrcount do
                        let index = i - 1
                        TwainAPI.DTWAIN_ArrayGetAtLong cap_array index &long_val |> ignore
                        TwainAPI.DTWAIN_GetNameFromCap long_val buffer 256 |> ignore
                        printfn "Capability %d: %s  Value: %d" (i) (buffer.ToString()) (long_val)

                    // Destroy the array when done
                    TwainAPI.DTWAIN_ArrayDestroy cap_array |> ignore

                    // Example of a callback that will "watch" when the TWAIN
                    // device acquires an image.  See the DTWAIN documentation
                    // on the notifications that will be sent to your application
                    
                    let get_notification_code (wParam : nativeint) =
                        TwainAPI.DTWAIN_GetTwainNameFromConstantW (TwainAPI.DTWAIN_CONSTANT_DTWAIN_TN) (wParam.ToInt32()) (buffer) (256)  |> ignore
                        ()
                        
                    let myCallback (wParam) (lParam) (userData: int64) : nativeint =

                        // Get the name of the notification using DTWAIN_GetTwainNameFromConstant utility function
                        get_notification_code wParam

                        // print the callback information
                        printfn "DTWAIN Callback called!"
                        printfn "  wParam = %s" (buffer.ToString())
                        printfn "  lParam = 0x%016X" (uint64 lParam)  // This will have the value of the selected TWAI Source
                        printfn "  UserData = %016X" userData
                        nativeint 1 // Should always return 1 as a default

                    // Register the callback by calling DTWAIN_SetCallback64
                    TwainAPI.DTWAIN_SetCallback64 (DTWAIN_CALLBACK_PROC64(myCallback)) (0) |> ignore

                    // Now Acquire to a BMP file
                    let mutable status_ = 0
                    TwainAPI.DTWAIN_AcquireFile sourceResult "TEST.BMP" TwainAPI.DTWAIN_BMP 
                                               TwainAPI.DTWAIN_USELONGNAME
                                               TwainAPI.DTWAIN_PT_DEFAULT 1 1 1 &status_ |> ignore

                    TwainAPI.DTWAIN_SysDestroy() |> ignore

                    0  // success
        with
        | ex ->
            printfn "Error: %s" ex.Message
            1  // failure

    // Unload the DLL before exiting (no 'finally' used)
    TwainAPI.Unload()
    printfn "DLL unloaded."
    printfn "%d" TwainAPI.DTWAIN_BMP

    // Return the appropriate exit code
    exitCode
