The language binding for Dart will only support the following DTWAIN DLL's loaded at runtime:

* dtwain64u.dll
* dtwain64ud.dll

Basically, only the 64-bit Unicode versions of DTWAIN are compatible with the Dart language binding.  

----
Here is a Dart example using the  [dtwainapi.dart](https://github.com/dynarithmic/twain_library/tree/master/programming_language_bindings/Dart) module that defines the DTWAIN constants and functions.  

The program gives an example of selecting a TWAIN device installed on your system, getting the values defined by the ICAP_PIXELTYPE TWAIN capability, displaying a list of all the capabilities available to the device, and acquiring from the TWAIN device and saved as a BMP image.

Note that the Dart version of the DTWAIN API functions have the same name as the DTWAIN API functions without the `DTWAIN_` prefix.  For example, the DTWAIN function `DTWAIN_SysInitialize` is invoked in Dart by instantiating a "DTWAIN" object, and then invoking the `sysInitialize` function defined by the object.

```dart
import 'dart:ffi';
import 'package:ffi/ffi.dart';

import 'dtwainapi.dart';

const int bufferLength = 256;

void main() {
  final dtwain = DTWAIN(dllName: 'dtwain64u.dll');
  NativeCallable<DTWAIN_CALLBACK_NATIVE>? notificationCallback;
  var initialized = false;

  // Initialize DTWAIN
  try {
    if (dtwain.sysInitialize() == nullptr) {
      print('DTWAIN failed initialization');
      return;
    }
    initialized = true;

   // Select a TWAIN source
    final source = dtwain.selectSource2(nullptr,'Dart test', 0, 0, DTWAIN_DLG_CENTER_SCREEN,);
    if (source == nullptr) {
      print('No source was selected');
      return;
    }

    final nameBuffer = calloc<Uint16>(bufferLength).cast<Utf16>();
    final capNameBuffer = calloc<Uint16>(bufferLength).cast<Utf16>();
    final capArrayOut = calloc<Pointer<Void>>();

    try {
      // Display product name
      dtwain.getSourceProductName(source, nameBuffer, bufferLength);
      final productName = nameBuffer.toDartString();

      // Get and display all of the capability values supported by the device
      if (dtwain.enumSupportedCaps(source, capArrayOut) == 0 ||
          capArrayOut.value == nullptr) {
        print('Could not enumerate capabilities for $productName');
      } else {
        final capArray = capArrayOut.value;
        try {
          final count = dtwain.arrayGetCount(capArray);
          print('There are $count capabilities defined for device $productName');

          if (count > 0) {
            // As in the D demo, read the DTWAIN_ARRAY's contiguous LONG buffer.
            // This is borrowed memory: do not free it separately.
            final capBuffer = dtwain.arrayGetBuffer(capArray, 0).cast<Int32>();
            if (capBuffer == nullptr) {
              print('Could not obtain capability array buffer');
            } else {
              for (var i = 0; i < count; ++i) {
                final capValue = capBuffer[i];
                final nameLength = dtwain.getNameFromCap(
                  capValue,
                  capNameBuffer,
                  bufferLength,
                );
                final capName = nameLength > 0
                    ? capNameBuffer.toDartString()
                    : '(unknown capability)';
                print('Capability ${i + 1}: $capName  Value: $capValue');
              }
            }
          }
        } finally {
          dtwain.arrayDestroy(capArray);
        }
      }
    } finally {
      calloc.free(capArrayOut);
      calloc.free(capNameBuffer);
      calloc.free(nameBuffer);
    }

    // Set up DTWAIN notifications that get invoked while the device is
    // acquiring images

    // Native-to-Dart notifications must execute on this isolate's thread.
    int onNotification(int wParam, int lParam, int userData) {
      final notificationBuffer = calloc<Uint16>(bufferLength).cast<Utf16>();
      try {
        final length = dtwain.getTwainNameFromConstant(
          DTWAIN_CONSTANT_DTWAIN_TN,
          wParam,
          notificationBuffer,
          bufferLength,
        );
        final notificationName = length > 0
            ? notificationBuffer.toDartString()
            : 'Internal DTWAIN notification ($wParam)';
        print('Notification=$notificationName, lParam=$lParam');
      } finally {
        calloc.free(notificationBuffer);
      }
      return 1;
    }

    notificationCallback = NativeCallable<DTWAIN_CALLBACK_NATIVE>.isolateLocal(
      onNotification,
      exceptionalReturn: 1,
    );

    // Enable the message notification and assign the callbacks to DTWAIN
    dtwain.enableMsgNotify(1);
    dtwain.setCallback(notificationCallback.nativeFunction, 0);

    // Acquire to a BMP file
    final acquired = dtwain.acquireFile(
      source,
      'testdart.bmp',
      DTWAIN_BMP,
      DTWAIN_USELONGNAME,
      DTWAIN_PT_DEFAULT,
      1, // Maximum pages
      1, // Show source UI
      1, // Close source when finished
      nullptr, // Optional status output
    );
    if (acquired == 0) {
      print('Image acquisition failed (DTWAIN error ${dtwain.getLastError()})');
    }
  } finally {
    if (initialized) {
      // Unregister before releasing the native callable or shutting down DTWAIN.
      if (notificationCallback != null) {
        dtwain.setCallback(nullptr, 0);
        notificationCallback.close();
      }
      dtwain.sysDestroy();
    }
  }
}
```


