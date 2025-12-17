Here is a Perl example using the  [dtwain.pl](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/Perl) interface files that defines the DTWAIN constants and functions.  

The program gives an example of selecting a TWAIN device installed on your system, displaying a list of the capabilities available to the device, and acquiring a BMP image.

```perl
#!/usr/bin/perl
use strict;
use warnings;
$|++;
use Win32::API;

# DTWAIN Constants.  See DTWAIN.PL file for these values
use constant DTWAIN_USENATIVE => 1;
use constant DTWAIN_BMP => 100;
use constant DTWAIN_USELONGNAME => 64;
use constant DTWAIN_PT_DEFAULT => 1000;

# DTWAIN Functions the app will use.  See DTWAIN.PL file for these values
my $dtwain_dllName = 'DTWAIN64.DLL';  # This is for 64-bit Perl.  Change this to 'DTWAIN32.DLL' for 32-bit Perl

# Get these function definitions from DTWAIN.PL
my $DTWAIN_AcquireFile = new Win32::API($dtwain_dllName, 'DTWAIN_AcquireFile', 'NPiiiiIIP', 'I');
my $DTWAIN_ArrayDestroy = new Win32::API($dtwain_dllName, 'DTWAIN_ArrayDestroy', 'N', 'I');
my $DTWAIN_ArrayGetAtLong = new Win32::API($dtwain_dllName, 'DTWAIN_ArrayGetAtLong', 'NiP', 'I');
my $DTWAIN_ArrayGetCount = new Win32::API($dtwain_dllName, 'DTWAIN_ArrayGetCount', 'N', 'i');
my $DTWAIN_EnumSupportedCapsEx2 = new Win32::API($dtwain_dllName, 'DTWAIN_EnumSupportedCapsEx2', 'N', 'N');
my $DTWAIN_GetNameFromCapA = new Win32::API($dtwain_dllName, 'DTWAIN_GetNameFromCapA', 'iPi', 'i');
my $DTWAIN_GetSourceProductNameA = new Win32::API($dtwain_dllName, 'DTWAIN_GetSourceProductNameA', 'NPi', 'i');
my $DTWAIN_IsTwainAvailable = new Win32::API($dtwain_dllName, 'DTWAIN_IsTwainAvailable', '', 'I');
my $DTWAIN_SelectSource = new Win32::API($dtwain_dllName, 'DTWAIN_SelectSource', '', 'N');
my $DTWAIN_SysDestroy = new Win32::API($dtwain_dllName, 'DTWAIN_SysDestroy', '', 'I');
my $DTWAIN_SysInitialize = new Win32::API($dtwain_dllName, 'DTWAIN_SysInitialize', '', 'N');

# Start to use the DTWAIN functions
my $isAvail = $DTWAIN_IsTwainAvailable->Call();

if ( $isAvail == 1 )
{
    my $return = $DTWAIN_SysInitialize->Call();
    if ( $return)
    {
       my $TwainSource = $DTWAIN_SelectSource->Call();
       if ( $TwainSource != 0 )
       {
            # Display the product name of the Source
            my $mystrbuf = " " x 100;
            $DTWAIN_GetSourceProductNameA->Call($TwainSource, $mystrbuf, 100);
            print $mystrbuf;
        
            # Example usage of DTWAIN_ARRAY:
            # Get the device capabilities supported by the device
            #
            # Note: The DTWAIN_ARRAY, DTWAIN_SOURCE, DTWAIN_FRAME, and DTWAIN_RANGE are actually void pointers
            # so you have to use integers for these types.
            #
            my $dtwain_array = 0;

            # Note that the returned value is a DTWAIN_ARRAY.
            # For Perl, it is easier to use DTWAIN_EnumSupportedCapsEx2, since there is
            # no need to pack/unpack an integer if DTWAIN_EnumSupportedCaps were used instead.
            $dtwain_array = $DTWAIN_EnumSupportedCapsEx2->Call($TwainSource);

            # Get the number of items in the array
            my $arrcount = $DTWAIN_ArrayGetCount->Call($dtwain_array);

            print "\nThere are $arrcount device capabilities";

            #print each capability
            for (my $i = 0; $i < $arrcount; $i++) 
            {
               my $long_val = 0;

               # we pack/unpack here, since DTWAIN_ArrayGetAtLong requires a pointer to an integer
               # as the third argument.
               my $buffer = pack('i', $long_val); 
               $DTWAIN_ArrayGetAtLong->Call($dtwain_array, $i, $buffer);
               my $updated_value = unpack('i', $buffer);

               # Get the name of the capability that was found and print it out
               my $bufLen = $DTWAIN_GetNameFromCapA->Call($updated_value, $mystrbuf, 100);
               my $actual_string = substr($mystrbuf, 0, $bufLen);
               print "Capability ", $i+1, ": $actual_string  Value: $updated_value\n";
            }
            # Destroy the array when done
            $DTWAIN_ArrayDestroy->Call($dtwain_array);

            # Acquire the image
            $DTWAIN_AcquireFile->Call($TwainSource, 'testperl.bmp', DTWAIN_BMP, DTWAIN_USENATIVE + DTWAIN_USELONGNAME,
                                      DTWAIN_PT_DEFAULT, 1, 1, 1, 0);
       }
   }
   $DTWAIN_SysDestroy->Call();
}
```




