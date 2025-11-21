#
# This file is part of the Dynarithmic TWAIN Library (DTWAIN).                          
# Copyright (c) 2002-2026 Dynarithmic Software.                                         
#                                                                                       
# Licensed under the Apache License, Version 2.0 (the "License");                       
# you may not use this file except in compliance with the License.                      
# You may obtain a copy of the License at                                               
#                                                                                       
#     http://www.apache.org/licenses/LICENSE-2.0                                        
#                                                                                       
# Unless required by applicable law or agreed to in writing, software                   
# distributed under the License is distributed on an "AS IS" BASIS,                     
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.              
# See the License for the specific language governing permissions and                   
# limitations under the License.                                                        
#                                                                                       
# FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY                   
# DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT 
# OF THIRD PARTY RIGHTS.                                                                
#

#!/usr/bin/env ruby
require_relative 'dtwain'

if __FILE__ == $0
  # Initialize DTWAIN

  # load the 64-bit Unicode version of DTWAIN DLL. 
  # If you are running in a 32-bit environment, then the DLL should be dtwain32.dll, dtwain32u.dll, etc.

  dtwain_dll = DTWAINAPI.new('dtwain64u.dll') # replace with the absolute path to the dll here
  if !dtwain_dll.isInitialized()
      puts "Not initialized: #{dtwain_dll.isInitialized}" 
      return
  end

  #dll found and loaded, so start the initialization
  dtwain_dll.DTWAIN_SysInitialize.call()

  # Select a TWAIN souce
  twainSource = dtwain_dll.DTWAIN_SelectSource.call()

  if twainSource.null?
      puts "No TWAIN source was selected"
      return
  end

  # Display the product name of the Source
  # Create a char buffer to allow calling DTWAIN_GetSourceProductNameA
  #
  # If instead you wanted to call DTWAIN_GetSourceProductName, you will need a Unicode
  # buffer, since Ruby loaded the Unicode version of the DTWAIN DLL
  #
  # Allocate a Fiddle::Pointer for the buffer (e.g., 256 bytes)
  buffer_size = 256
  buffer = Fiddle::Pointer.malloc(buffer_size) 
  dtwain_dll.DTWAIN_GetSourceProductNameA.call(twainSource, buffer, 256)

  # convert buffer to ruby string
  result_string = buffer.to_s
  puts "Device product name is: #{result_string}" 

  # Example usage of DTWAIN_ARRAY:
  # Get the device capabilities supported by the device
  #
  # Note: The DTWAIN_ARRAY, DTWAIN_SOURCE, DTWAIN_FRAME, and DTWAIN_RANGE are actually void pointers
  # so you have to declare them as such if a DTWAIN function requires a parameter to be of this type.
  #
  # For Ruby, it is much easier to call the DTWAIN function(s) that return DTWAIN_ARRAY's and similar 
  # instead of  the version(s) that allows passing a pointer to a DTWAIN_ARRAY.  So in this case, we call
  # DTWAIN_EnumSupportedCapsEx2
  
  # get the supported caps
  dtwain_array = dtwain_dll.DTWAIN_EnumSupportedCapsEx2.call(twainSource)

  # get the number of items in the array
  arrCount = dtwain_dll.DTWAIN_ArrayGetCount.call(dtwain_array)
  puts "There are #{arrCount} device capabilities"
    
  #print each capability
  long_val = Fiddle::Pointer.malloc(Fiddle::SIZEOF_INT)
  long_val[0, Fiddle::SIZEOF_INT] = [0].pack("l!")

  arrCount.times do |i| 
      dtwain_dll.DTWAIN_ArrayGetAtLong.call(dtwain_array, i, long_val)
      modified_value = long_val[0, Fiddle::SIZEOF_INT].unpack("l!").first

      dtwain_dll.DTWAIN_GetNameFromCapA.call(modified_value, buffer, 256)
      result_string = buffer.to_s
      puts "Capability #{i+1}: #{result_string}  Value: #{modified_value}"
    end

  # Now Acquire to a BMP file
  dtwain_dll.DTWAIN_AcquireFileA.call(twainSource, "TEST.BMP", DTWAINAPI::DTWAIN_BMP, DTWAINAPI::DTWAIN_USELONGNAME,
                             DTWAINAPI::DTWAIN_PT_DEFAULT, 1, 1, 1, Fiddle::Pointer.new(0))

  dtwain_dll.DTWAIN_SysDestroy.call()
end

