# OCR Library Installation and Configuration

DTWAIN supports optical character recognition (OCR) through an external OCR engine. OCR can be used to produce text output from scanned pages, including the `DTWAIN_TEXT` (single-page) and `DTWAIN_TEXTMULTI` (multipage) file formats.

## Currently supported OCR engine: Transym TOCR

DTWAIN currently supports **TOCR**, supplied by **Transym Computer Services**:

- [Transym Computer Services website](https://www.transym.com/)

**TOCR is a commercial, separately licensed product.** DTWAIN and Dynarithmic Software do not provide the TOCR DLLs or a TOCR license. Applications using TOCR must have the appropriate TOCR components installed and must comply with Transym's licensing and redistribution requirements. Consult Transym for the required installation package, licensing terms, and DLL dependencies.

Ensure that the TOCR DLLs installed are compatible with the architecture of the DTWAIN application (32-bit or 64-bit, as applicable).

## Configuring the TOCR library directory

The TOCR runtime DLLs must be accessible when DTWAIN initializes the OCR engine. You can configure their directory using the DTWAIN INI file or the DTWAIN API.

### Option 1: DTWAIN INI configuration

In `dtwain32.ini` or `dtwain64.ini`, use the `[OCRLibrary]` section and `TransymPath` key:

```ini
[OCRLibrary]
TransymPath=C:\OCR\TOCR
```

`DTWAIN_SysInitialize()` reads the INI configuration. Use the directory appropriate for the application's architecture and TOCR installation. An empty value can be written as:

```ini
[OCRLibrary]
TransymPath=
```

When no explicit TOCR directory is configured, the required DLLs must be discoverable through the runtime's normal DLL-loading mechanism. Ensure the TOCR installation makes its libraries accessible.

### Option 2: Configure the directory programmatically

After calling `DTWAIN_SysInitialize()`, the application can specify the TOCR directory with `DTWAIN_SetComponentPath()`:

```cpp
DTWAIN_SysInitialize();

DTWAIN_SetComponentPath(
    DTWAIN_COMPONENT_TRANSYMOCR,
    _T("C:\\OCR\\TOCR"));
```

The API-specified directory overrides the directory read from the INI configuration. Configure the directory **before** initializing the OCR engine.

## Initializing OCR

Unlike ordinary image acquisition, OCR engine initialization is explicitly requested by the application using **`DTWAIN_InitOCRInterface()`**. The installed TOCR components must be available when this initialization occurs.

If initialization fails, verify that the TOCR product is installed and licensed, the configured directory is correct, the DLL architecture matches the application, and all required dependencies are available. Applications should handle OCR initialization failures rather than assuming OCR is installed on every computer.

Once an OCR engine has been loaded, changing its library directory requires ending the DTWAIN session with `DTWAIN_SysDestroy()` and starting a new one with `DTWAIN_SysInitialize()`. The application can update the INI setting between sessions; restarting the application itself is not required.

## Selecting an OCR engine

After calling `DTWAIN_SysInitialize()` and `DTWAIN_InitOCRInterface()`, the application can select which initialized OCR engine to use. DTWAIN provides a **TWAIN-like OCR engine selection interface**, similar to selecting a TWAIN scanner source.

The following selection functions are available:

| Function | Description |
| --- | --- |
| [`DTWAIN_SelectOCREngine()`](https://www.dynarithmic.com/onlinehelp/dtwain/newversion/DTWAIN_SelectOCREngine.html) | Displays the **Select OCR Engine** dialog, allowing the user to choose an OCR engine from the available engines. |
| [`DTWAIN_SelectOCREngine2()`](https://www.dynarithmic.com/onlinehelp/dtwain/newversion/DTWAIN_SelectOCREngine2.html) | Displays the OCR selection dialog with additional options, such as centering the dialog and sorting the OCR engine names, similar to `DTWAIN_SelectSource2()`. |
| [`DTWAIN_SelectDefaultOCREngine()`](https://www.dynarithmic.com/onlinehelp/dtwain/newversion/DTWAIN_SelectDefaultOCREngine.html) | Selects the default OCR engine without displaying a selection dialog. |
| [`DTWAIN_SelectOCREngineByName()`](https://www.dynarithmic.com/onlinehelp/dtwain/newversion/DTWAIN_SelectOCREngineByName.html) | Selects an OCR engine by its product name without displaying a selection dialog. |

For example, an application that allows the user to choose an OCR engine can use:

```cpp
DTWAIN_SysInitialize();

if (DTWAIN_InitOCRInterface())
{
    DTWAIN_OCRENGINE engine = DTWAIN_SelectOCREngine();
    if (engine)
    {
        // Use the selected OCR engine for OCR operations.
    }
}
```

`DTWAIN_SelectOCREngine()` returns a `DTWAIN_OCRENGINE` handle when an engine is selected, or `NULL` if selection fails or the user cancels. To distinguish cancellation from an error, check `DTWAIN_GetLastError()`; a zero error code indicates cancellation.

For unattended applications, `DTWAIN_SelectDefaultOCREngine()` or `DTWAIN_SelectOCREngineByName()` avoids displaying a selection dialog. The selected engine handle can be used with DTWAIN's OCR functions. For further information, see the [DTWAIN OCR documentation](https://www.dynarithmic.com/onlinehelp/dtwain/newversion/DTWAIN_SelectOCREngine.html).

## OCR-generated text formats

DTWAIN supports the following OCR-based output formats:

| File type | Description |
| --- | --- |
| `DTWAIN_TEXT` | Recognized text from a single scanned page |
| `DTWAIN_TEXTMULTI` | Recognized text from multiple scanned pages in one output file |

These formats require a usable OCR engine. Unlike device-native file transfer, text recognition is performed through the configured OCR engine rather than by requiring the TWAIN scanner to support text output.

### Checking OCR file-type support

After calling `DTWAIN_InitOCRInterface()`, an application can check whether an OCR engine is properly installed and initialized by using `DTWAIN_IsFileTypeSupported()`:

```cpp
if (DTWAIN_IsFileTypeSupported(DTWAIN_TEXT))
{
    // Single-page OCR text output is supported.
}

if (DTWAIN_IsFileTypeSupported(DTWAIN_TEXTMULTI))
{
    // Multipage OCR text output is supported.
}
```

These checks determine whether **any supported OCR engine** is available and initialized; they are not specific to Transym TOCR. They do not replace the explicit call to `DTWAIN_InitOCRInterface()`.

## Future support: Tesseract

**Tesseract OCR** is a free, open-source OCR engine planned for a subsequent DTWAIN version **after version 5.10.1**. It is **not included as a supported OCR engine in DTWAIN 5.10.1**. Installation and configuration instructions for Tesseract will be provided when support is released.

## Troubleshooting checklist

- Confirm that TOCR was obtained and installed from Transym Computer Services under the appropriate license.
- Confirm that the application is using the correct 32-bit or 64-bit TOCR components.
- Check `[OCRLibrary]` / `TransymPath` in the applicable DTWAIN INI file.
- If calling `DTWAIN_SetComponentPath()`, do so after `DTWAIN_SysInitialize()` and before `DTWAIN_InitOCRInterface()`.
- Ensure the TOCR DLLs and their dependencies are accessible at runtime.
- Check the result of OCR initialization and the application's DTWAIN error handling.
