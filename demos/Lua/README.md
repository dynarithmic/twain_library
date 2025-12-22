The Lua language binding makes usage of the [LuaJit ffi library](https://luajit.org/ext_ffi.html) to bind Lua to the DTWAIN DLL functions.  

Thus using the DTWAIN binding requires that you are running the [LuaJit compiler](https://luajit.org/luajit.html).  

----
Here is a Lua example using the  [dtwainapi.lua and dtwainapi_constants.lua](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/Lua) interface files that defines the DTWAIN constants and functions.  

The program gives an example of selecting a TWAIN device installed on your system, displaying a list of the capabilities available to the device, and acquiring a BMP image.

```lua
local dtwain = require("dtwainapi")
local dconstants = require("dtwainapi_constants")

-- Load the DTWAIN library (make sure "dtwain32u.dll" or "dtwain64u.dll" is accessible)
-- You can use a full pathname here also, to ensure lua finds the dll

-- Check for the lua environment, and load the Unicode 64-bit or 32-bit DLL
local dtwain_lib = {}
local ffi = require("ffi")
local ptr_size = ffi.sizeof("void*")
if ptr_size == 8  then
  dtwain_lib = load_dtwaindll("dtwain64.dll") 
else
  dtwain_lib = load_dtwaindll("dtwain32.dll") 
end

if dtwain_lib == nil then
    print("DTWAIN Lua binding failure")
    os.exit(-1)
end

-- Initialize DTWAIN
local dtwain_libinit = dtwain_lib.DTWAIN_SysInitialize()
if dtwain_libinit == nil then
    print("DTWAIN failed to be initialized")
    os.exit(-2)
end

-- Select a TWAIN source
local twain_source = dtwain_lib.DTWAIN_SelectSource()
if twain_source == nil then
    print("No Source was selected")
    dtwain_lib.DTWAIN_SysDestroy()
    os.exit(0)
end

-- Display the product name
-- Create a char buffer to allow calling DTWAIN_GetSourceProductNameA
local buffer_size = 256
local mystrbuf = ffi.new("char[?]", buffer_size)
dtwain_lib.DTWAIN_GetSourceProductNameA(twain_source, mystrbuf, buffer_size)
local out_string = ffi.string(mystrbuf)
print(out_string)

-- Example usage of DTWAIN_ARRAY:
-- Get the device capabilities supported by the device
-- For Lua, it is much easier to call the DTWAIN function(s) that return DTWAIN_ARRAY's and similar 
-- instead of  the version(s) that allows passing a pointer to a DTWAIN_ARRAY.  So in this case, we call
-- DTWAIN_EnumSupportedCapsEx2
local supported_caps = dtwain_lib.DTWAIN_EnumSupportedCapsEx2(twain_source)

-- Get the number of items in the array
local arrcount = dtwain_lib.DTWAIN_ArrayGetCount(supported_caps)
print(string.format("There are %d device capabilities", arrcount))

-- Get a pointer to the array buffer
-- Here we use the DTWAIN_ArrayGetBuffer to get a pointer to the DTWAIN_ARRAY buffer
-- This bypasses having to use DTWAIN_ArrayGetAtLong to get the LONG values
local void_ptr = dtwain_lib.DTWAIN_ArrayGetBuffer(supported_caps, 0)

-- The buffer is of type LONG, so make sure pointer is LPLONG
local int_ptr = ffi.cast("LPLONG", void_ptr) 

-- print each capability
for i = 0, arrcount - 1 do
    local myString = ""
    dtwain_lib.DTWAIN_GetNameFromCapA(int_ptr[i], mystrbuf, 256)
    out_string = ffi.string(mystrbuf)
    print(string.format("Capability %d: %d   Value: %s", i+1, int_ptr[i], out_string))
end

-- Destroy the array when done
dtwain_lib.DTWAIN_ArrayDestroy(supported_caps)

-- Now Acquire to a BMP file
dtwain_lib.DTWAIN_AcquireFileA(twain_source, "TEST.BMP", dconstants.DTWAIN_BMP, dconstants.DTWAIN_USELONGNAME, dconstants.DTWAIN_PT_DEFAULT, 1, 1, 1, nil)

-- Close down DTWAIN                                      
dtwain_lib.DTWAIN_SysDestroy()
```


