from ctypes import *
import dtwain
import ctypes as ct

def test_dtwain():
    # Load the DTWAIN library (make sure "dtwain32u.dll" is accessible)
    # You can use a full pathname here also, to ensure python finds the dll
    dtwain_dll = windll.LoadLibrary("dtwain32u.dll") 
    
    # Initialize DTWAIN
    dtwain_dll.DTWAIN_SysInitialize()
    
    # Select a TWAIN source
    TwainSource = dtwain_dll.DTWAIN_SelectSource()
    if TwainSource:
        # Display the product name of the Source
        mystrbuf = ct.create_string_buffer(100)
        dtwain_dll.DTWAIN_GetSourceProductNameA(TwainSource,mystrbuf,len(mystrbuf))
        print (mystrbuf.value)
        
        # Acquire to a BMP file
        dtwain_dll.DTWAIN_AcquireFile(TwainSource, "TEST.BMP", dtwain.DTWAIN_BMP, dtwain.DTWAIN_USELONGNAME,
                                      dtwain.DTWAIN_PT_DEFAULT, 1, 1, 1, 0)
    # Close down DTWAIN                                      
    dtwain_dll.DTWAIN_SysDestroy()

if __name__ == '__main__':
    test_dtwain()
