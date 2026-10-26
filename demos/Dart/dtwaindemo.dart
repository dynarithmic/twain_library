// Dart translation of dtwaindemo.d for the 64-bit Unicode DTWAIN library.
// Requires dtwain_funcdefs_alternate(5).dart and dtwain_constants.dart.
import 'dart:ffi';
import 'package:ffi/ffi.dart';

import 'dtwainapi.dart';

const int bufferLength = 256;

void main() {
  final dtwain = DTWAIN(dllName: 'dtwain64ud.dll');
  NativeCallable<DTWAIN_CALLBACK_NATIVE>? notificationCallback;
  var initialized = false;

  try {
    if (dtwain.sysInitialize() == nullptr) {
      print('DTWAIN failed initialization');
      return;
    }
    initialized = true;

    final source = dtwain.selectSource2(
      nullptr,
      'This is a test',
      0,
      0,
      DTWAIN_DLG_CENTER_SCREEN,
    );
    if (source == nullptr) {
      print('No source was selected');
      return;
    }

    final nameBuffer = calloc<Uint16>(bufferLength).cast<Utf16>();
    final capNameBuffer = calloc<Uint16>(bufferLength).cast<Utf16>();
    final capArrayOut = calloc<Pointer<Void>>();

    try {
      dtwain.getSourceProductName(source, nameBuffer, bufferLength);
      final productName = nameBuffer.toDartString();

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

    dtwain.enableMsgNotify(1);
    dtwain.setCallback(notificationCallback.nativeFunction, 0);

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
