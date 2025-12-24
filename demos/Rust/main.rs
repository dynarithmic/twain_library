use std::ptr;
use std::mem;
use std::ffi::{c_char, CStr, CString, c_void};

use libloading::{Library};
use crate::dtwainapi::DTwainAPI;

mod dtwainapi;

pub fn main() -> Result<(), Box<dyn std::error::Error>>{
    // Load the DTWAIN library (make sure "dtwain32u.dll" or "dtwain64u.dll" is accessible)
    // You can use a full pathname here also, to ensure rust finds the dll

    // Check for the rust environment, and load the Unicode 64-bit or 32-bit DLL
    let mut library_name = "dtwain64u.dll";
    if mem::size_of::<*const u8>() == 4  // 32-bit
    {
        library_name = "dtwain32u.dll";
    }

    // Load the library and resolve all the function pointers
    let library = unsafe { Library::new(library_name)? };
    let api_func = dtwainapi::DTwainAPI::new(&library).unwrap();

    // Initialize DTWAIN
    api_func.DTWAIN_SysInitialize();

    // Select a TWAIN source.  Note that instead of the usual Select Source dialog,
    // we use the specialized "Select Source" dialog that center's itself on the screen,
    // plus allows us to title the dialog as "Rust demo"
    let mut c_string = CString:: new("Rust demo").unwrap();
    let twain_source = api_func.DTWAIN_SelectSource2A(ptr::null(), c_string.as_ptr(),
                                                      0, 0, DTwainAPI::DTWAIN_DLG_CENTER_SCREEN);

    // If a source was selected, display the name
    if twain_source.is_null()
    {
        println!("No source selected");
    }
    else
    {
        unsafe
        {
            // Display the product name.
            // Note we use the helper function allocate_ansi_buffer to allocate a char buffer
            // that the DLL function can work with.
            let char_buffer = dtwainapi::DTwainAPI::allocate_ansi_buffer(256);
            api_func.DTWAIN_GetSourceProductNameA(twain_source, char_buffer, 256);

            // Get a Rust string from the allocated buffer and display the results
            let actual_prodname = String::from(CStr::from_ptr(char_buffer as *const c_char).to_str().unwrap());
            println!("The name of the selected source is: {}", actual_prodname);


            /* Example usage of DTWAIN_ARRAY:
             Get the device capabilities supported by the device */

            /* We will use the Ex2 version of DTWAIN_EnumSupportedCaps, since it
               is easier to handle the returned DTWAIN_ARRAY.
             */
            let allcaps = api_func.DTWAIN_EnumSupportedCapsEx2(twain_source);

            // Get the number of items in the array
            let arrcount = api_func.DTWAIN_ArrayGetCount(allcaps);
            println!("There are {} device capabilities", arrcount);

            // print each capability
            let mut long_val : i32 = 0;
            let ptr: *mut i32 = &mut long_val;
            for i in 0..arrcount
            {
                api_func.DTWAIN_ArrayGetAtLong(allcaps, i, ptr);
                api_func.DTWAIN_GetNameFromCapA(long_val, char_buffer, 256);
                let actual_capname = String::from(CStr::from_ptr(char_buffer as *const c_char).to_str().unwrap());
                println!("Capability {}: {}  Value: {}", i + 1, actual_capname, long_val);
            }

            // Get the pixel types by calling DTWAIN_GetCapValues()
            let mut cap_array = ptr::null_mut();
            api_func.DTWAIN_GetCapValues(twain_source, DTwainAPI::DTWAIN_CV_ICAPPIXELTYPE, DTwainAPI::DTWAIN_CAPGET, &mut cap_array as *mut *mut c_void);

            // Print out the pixel types
            let pixel_count = api_func.DTWAIN_ArrayGetCount(cap_array);
            let ptr2: *mut i32 = &mut long_val;
            for i in 0..pixel_count
            {
                api_func.DTWAIN_ArrayGetAtLong(cap_array, i, ptr2);
                println!("pixel type is: {}", long_val);
            }

            // Destroy the DTWAIN_ARRAYs
            api_func.DTWAIN_ArrayDestroy(allcaps);
            api_func.DTWAIN_ArrayDestroy(cap_array);

            // Now acquire to a bmp file
            c_string = CString::new("rust.bmp").unwrap();
            api_func.DTWAIN_AcquireFileA(twain_source, c_string.as_ptr(), DTwainAPI::DTWAIN_BMP, DTwainAPI::DTWAIN_USELONGNAME,
                                         DTwainAPI::DTWAIN_PT_DEFAULT, 1, 1, 1, ptr::null_mut());
        }
        // Close down DTWAIN
        api_func.DTWAIN_SysDestroy();
    }
    Ok(())
}
