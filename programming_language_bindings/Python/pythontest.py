from ctypes import *
import dtwainapi
import struct
import ctypes as ct

def test_dtwain():
    # Load the DTWAIN library (make sure "dtwain32u.dll" or "dtwain64u.dll" is accessible)
    # You can use a full pathname here also, to ensure python finds the dll
    
    # Check for the python environment, and load the Unicode 64-bit or 32-bit DLL
    if struct.calcsize("P") * 8 == 64:
        dtwain_dll = dtwain.load_dtwaindll("dtwain64u.dll")
    else:
        dtwain_dll = dtwain.load_dtwaindll("dtwain32u.dll")

    # Initialize DTWAIN
    dtwain_dll.DTWAIN_SysInitialize()
    
    # Select a TWAIN source
    TwainSource = dtwain_dll.DTWAIN_SelectSource()
    if TwainSource:

        # Display the product name of the Source
        # Create a char buffer to allow calling DTWAIN_GetSourceProductNameA
        #
        # If instead you wanted to call DTWAIN_GetSourceProductName, you will need a Unicode
        # buffer, i.e. ct.create_unicode_buffer(100), since python loaded the Unicode version
        # of the DTWAIN DLL
        #
        mystrbuf = ct.create_string_buffer(100)
        dtwain_dll.DTWAIN_GetSourceProductNameA(TwainSource, mystrbuf, len(mystrbuf))
        print (mystrbuf.value)
        
        # Example usage of DTWAIN_ARRAY:
        # Get the device capabilities supported by the device
        #
        # Note: The DTWAIN_ARRAY, DTWAIN_SOURCE, DTWAIN_FRAME, and DTWAIN_RANGE are actually void pointers
        # so you have to declare them as such if a DTWAIN function requires a parameter to be of this type.
        dtwain_array = ct.c_void_p(0)

        # Note that the second parameter is the address a DTWAIN_ARRAY, i.e. a LPDTWAIN_ARRAY
        dtwain_dll.DTWAIN_EnumSupportedCaps(TwainSource, ct.byref(dtwain_array))

        # Get the number of items in the array
        arrcount = dtwain_dll.DTWAIN_ArrayGetCount(dtwain_array)
        print(f"There are {arrcount} device capabilities")

        #print each capability
        for i in range(arrcount):
            long_val = ct.c_long(0)
            dtwain_dll.DTWAIN_ArrayGetAtLong(dtwain_array, i, ct.byref(long_val))
            dtwain_dll.DTWAIN_GetNameFromCapA(long_val, mystrbuf, len(mystrbuf))
            print(f"Capability {i+1}: {mystrbuf.value}  Value: {long_val.value}")

        # Destroy the array when done
        dtwain_dll.DTWAIN_ArrayDestroy(dtwain_array)

        # Now Acquire to a BMP file
        dtwain_dll.DTWAIN_AcquireFile(TwainSource, "TEST.BMP", dtwain.DTWAIN_BMP, dtwain.DTWAIN_USELONGNAME,
                                       dtwain.DTWAIN_PT_DEFAULT, 1, 1, 1, None)
    # Close down DTWAIN                                      
    dtwain_dll.DTWAIN_SysDestroy()

if __name__ == '__main__':
    test_dtwain()
