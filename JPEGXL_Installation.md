# Installing the JPEG XL Components for DTWAIN

DTWAIN supports saving images in **JPEG XL (JXL)** format using optional JPEG XL runtime libraries. These libraries are distributed separately from the main DTWAIN DLLs.

## Download

Download **[jpegxl_binaries.zip](https://github.com/dynarithmic/twain_library/releases/latest/download/jpegxl_binaries.zip)** from the release assets.

The archive contains:

- **`jxl32/`** — 32-bit JPEG XL runtime DLLs, including `libjxl.dll` and its required dependencies (such as Brotli DLLs).
- **`jxl64/`** — Corresponding 64-bit JPEG XL runtime DLLs.
- **`LICENSE.txt` and `LICENSE.*` files** — License notices for JPEG XL and bundled third-party components.

Keep the DLLs from each architecture together, including their dependencies. Do not mix DLLs from `jxl32` and `jxl64`.

## Installation and configuration options

The JPEG XL DLLs must be accessible to the application **at runtime**. You can use Windows DLL search behavior, configure a path in the DTWAIN INI file, or specify a path through the DTWAIN API.

### Option 1: Use the normal Windows DLL search behavior

For an application that runs in only one architecture, place the DLLs for that architecture in a location where Windows can load them, such as the application's executable directory or an appropriate directory on the DLL search path.

For example, a 64-bit application should deploy the contents of `jxl64`, while a 32-bit application should deploy the contents of `jxl32`.

If the `libjxlpath` INI setting is missing or empty, and the application does not specify a component path through the API, DTWAIN uses normal Windows DLL search behavior when initializing the JPEG XL libraries.

> **Note:** Simply storing `jxl32` and `jxl64` alongside the executable does not make Windows automatically choose between them.

### Option 2: Configure the JPEG XL directory in the DTWAIN INI file

The `dtwain32.ini` and `dtwain64.ini` files support the **`libjxlpath`** key in the **`[ImageLibraries]`** section:

```ini
[ImageLibraries]
libjxlpath=C:\MyApplication\jxl64
```

Set the path to the directory containing `libjxl.dll` and the other JPEG XL runtime DLLs, not to `libjxl.dll` itself. Use a directory appropriate to the architecture of the application (for example, `jxl32` for a 32-bit process and `jxl64` for a 64-bit process).

The setting may also be empty:

```ini
[ImageLibraries]
libjxlpath=
```

**An empty setting or no `libjxlpath` setting means that DTWAIN uses normal Windows DLL search behavior**, unless the application overrides the directory through `DTWAIN_SetComponentPath()`.

DTWAIN reads the INI setting during `DTWAIN_SysInitialize()` and stores the configured path. **The directory is not validated at that time.** A nonexistent directory or missing DLLs will be detected when DTWAIN attempts to load the JPEG XL libraries.

### Option 3: Specify the JPEG XL directory through the API

For applications that support both 32-bit and 64-bit operation, keep the two architectures in separate directories, for example:

```text
MyApplication/
    jxl32/
        libjxl.dll
        ...other 32-bit DLLs...
    jxl64/
        libjxl.dll
        ...other 64-bit DLLs...
```

After calling `DTWAIN_SysInitialize()`, select the appropriate directory and call:

```c
DTWAIN_SetComponentPath(DTWAIN_COMPONENT_JPEGXL, library_path);
```

Here, `library_path` is the directory containing `libjxl.dll` and the other required JPEG XL DLLs, **not** the full filename of `libjxl.dll`.

For example, in C or C++ (using the generic-text DTWAIN interface):

```cpp
DTWAIN_SysInitialize();

#ifdef _WIN64
    const TCHAR* library_path = _T("C:\\MyApplication\\jxl64");
#else
    const TCHAR* library_path = _T("C:\\MyApplication\\jxl32");
#endif

if (!DTWAIN_SetComponentPath(DTWAIN_COMPONENT_JPEGXL, library_path))
{
    // The component directory could not be configured.
    // Handle the error before attempting JPEG XL output.
}
```

Replace these example paths with your application's actual deployment directories.

**`DTWAIN_SetComponentPath()` must be called after `DTWAIN_SysInitialize()`.** It overrides any JPEG XL path read from the INI file and **immediately checks whether the specified directory exists**. A valid directory does not necessarily contain all the required JPEG XL DLLs.

If the JPEG XL libraries have already been loaded, an attempt to change the path using `DTWAIN_SetComponentPath()` fails with **`DTWAIN_ERR_JXL_ALREADYLOADED` (`-1095`)**. Configure the path before any operation that loads JPEG XL support.

The architecture is determined by the **running process**, not the Windows installation. A 32-bit application running on 64-bit Windows must use `jxl32`.

## Configuration precedence and loading

DTWAIN determines the JPEG XL directory as follows:

1. `DTWAIN_SysInitialize()` reads `libjxlpath` from `[ImageLibraries]`, if present, and stores it without validating the directory.
2. After initialization, `DTWAIN_SetComponentPath(DTWAIN_COMPONENT_JPEGXL, ...)` can override that path, provided JPEG XL has not already been loaded and the supplied directory exists.
3. If neither method specifies a nonempty path, normal Windows DLL search behavior is used when JPEG XL is initialized.

The INI setting alone does not load the JPEG XL DLLs. Loading occurs when JPEG XL functionality is initialized or requested. To change the configured directory after JPEG XL has already been loaded, end the DTWAIN session using `DTWAIN_SysDestroy()` and initialize a new session before configuring the new directory.

## Checking JPEG XL availability

An existing directory does **not** guarantee that the correct JPEG XL DLLs and dependencies are present or loadable. Applications can check whether the JPEG XL components can be loaded:

```cpp
if (DTWAIN_IsFileTypeSupported(DTWAIN_JPEGXL))
{
    // JPEG XL support is available.
}
else
{
    // JPEG XL support is unavailable; report the issue
    // or choose another output format.
}
```

Call this after configuring the directory and before relying on JPEG XL output. Because this check can load the JPEG XL libraries, configure the component path **before** calling it.

## Licensing and redistribution

The archive includes license notices for JPEG XL and its third-party components. When redistributing the JPEG XL binaries with your application, **retain the applicable license files and notices**. These files are provided to help meet the redistribution requirements of the bundled libraries.

## Troubleshooting

If JPEG XL output cannot be initialized, check the following:

1. **Architecture:** All JPEG XL DLLs match the application's 32-bit or 64-bit architecture.
2. **Dependencies:** The complete set of required DLLs from the matching `jxl32` or `jxl64` directory has been deployed.
3. **Directory:** The configured component path points to the directory containing `libjxl.dll`. INI paths are not validated during `DTWAIN_SysInitialize()`.
4. **Timing:** `DTWAIN_SetComponentPath()` was called after `DTWAIN_SysInitialize()` but before JPEG XL was loaded. A subsequent path change fails with `DTWAIN_ERR_JXL_ALREADYLOADED` (`-1095`).
5. **Availability:** `DTWAIN_IsFileTypeSupported(DTWAIN_JPEGXL)` confirms that the JPEG XL components can be loaded.
6. **Loading:** Windows can load the DLLs and their dependencies from the deployed locations.

The JPEG XL runtime components are optional; applications that do not use JPEG XL do not need to deploy them.
