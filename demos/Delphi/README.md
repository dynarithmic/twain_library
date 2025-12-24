For Delphi, the DTWAIN DLL must be located where the [Windows DLL search logic](https://learn.microsoft.com/en-us/windows/win32/dlls/dynamic-link-library-search-order) will be used to find DLL's (which in simpler terms means that the DTWAIN DLL can be placed in the current executable directory, or a directory specified by the system PATH environment variable).  

You must add to your Delphi project one of the unit files below, depending on the DTWAIN DLL and whether to use the Unicode version of the library:

| Delphi unit file name       | Application type and DTWAIN DLL runtime       |
|----------------|----------------|
| dtwain32.pas | 32-bit ANSI using dtwain32.dll |
| dtwain32d.pas | 32-bit Debug ANSI using dtwain32d.dll |
| dtwain32u.pas | 32-bit Unicode using dtwain32u.dll |
| dtwain32ud.pas | 32-bit Debug Unicode using dtwain32u.dll |
| dtwain64.pas | 64-bit ANSI using dtwain64.dll |
| dtwain64d.pas | 64-bit Debug ANSI using dtwain64d.dll |
| dtwain64u.pas | 64-bit Unicode using dtwain64u.dll |
| dtwain64ud.pas | 64-bit Debug Unicode using dtwain64ud.dll |

----

Here is a Delphi example of selecting a TWAIN device installed on your system, displaying a list of all the capabilities available to the device, and acquiring from the TWAIN device and saved as a BMP image.

The example uses `dtwain32u.pas` unit, thus dtwain32u.dll will be the DTWAIN DLL that is going to be loaded by Delphi:

```vbnet
program Project1;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  Windows, Messages, SysUtils, dtwain32u;

var
   SelectedSource: DTWAIN_SOURCE;
   ErrStatus: LONG;
   AcquisitionArray: DWORD;
   StringBuffer: string;
   NumChars: LONG;
   RetCode : BOOL;
   PageCount : LONG;
   len: DWORD;
   CapArray : DTWAIN_ARRAY;
   ArrayCount, i : Integer;
   ArrayValue : LONG;

begin
   { Check for Twain availability }
   if (DTWAIN_IsTwainAvailable <> 0) then
   begin
      { Initialize DTWAIN }
      if (DTWAIN_SysInitialize <> 0) then
      begin
           SelectedSource := DTWAIN_SelectSource2(0, 'Select Source',0,0,
                                                   DTWAIN_DLG_CENTER_SCREEN);
           if SelectedSource <> 0 then
           begin
               { Display the Product Name of the Source that was selected }
               SetLength(StringBuffer, MAX_PATH);
               len := DTWAIN_GetSourceProductName(SelectedSource, PChar(StringBuffer),
                                               Length(StringBuffer));
               SetLength(StringBuffer, len); // trim to actual size
               Writeln('The name of the selected TWAIN Source is ' + StringBuffer);

               { Example usage of DTWAIN_ARRAY:
                Get the device capabilities supported by the device }
               DTWAIN_EnumSupportedCaps(SelectedSource, @CapArray);

               { Get the count of the number of capabilities}
               ArrayCount := DTWAIN_ArrayGetCount(CapArray);
               Writeln(Format('There are %d device capabilities for device %s', [ArrayCount, StringBuffer]));

               { List each capability and the capability name and TWAIN constant value }
               for i := 1 to ArrayCount do
               begin
                   SetLength(StringBuffer, MAX_PATH);
                   DTWAIN_ArrayGetAtLong(CapArray, i-1, @ArrayValue);
                   len := DTWAIN_GetNameFromCap(ArrayValue, PChar(StringBuffer), Length(StringBuffer));
                   SetLength(StringBuffer, len); // trim to actual size
                   Writeln(Format('Capability %d: %s  Value: %d', [i, StringBuffer, ArrayValue]));
               end;

               { Acquire a BMP image }
               RetCode := DTWAIN_AcquireFileA(
                          SelectedSource,    { the Source }
                          'test.bmp',        { File name to save }
                          DTWAIN_BMP,        { Image format }
                          DTWAIN_USELONGNAME, { File action constants }
                          DTWAIN_PT_DEFAULT, { default pixel type }
                          DTWAIN_ACQUIREALL, { get all documents }
                          1,              { show the UI }
                          1,              { close Source when done }
                          @ErrStatus);    { Error status }

               if (RetCode = 0) then
               begin
                  { didn't work }
                  Writeln('TWAIN Acquisition Failed!');
               end
               else
               begin
                   PageCount := DTWAIN_GetSavedFilesCount(SelectedSource);
                   if (PageCount = 0) then
                   begin
                     { user canceled or no page acquired }
                       Writeln('User canceled acquisition');
                   end
               end;
           end;
           { close all sources, sessions, and DTWAIN itself }
           DTWAIN_SysDestroy
      end
   end
end.
```
