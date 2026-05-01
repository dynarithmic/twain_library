# DTWAIN Visual Studio Debug Visualizer (`dtwain.natvis`)

The `dtwain.natvis` file is a Visual Studio debugger visualizer that improves how DTWAIN handle-based types are displayed while debugging C/C++ applications.

It provides readable debug views for:

* `DTWAIN_ARRAY`
* `DTWAIN_SOURCE`

Since these types are intentionally opaque (`void*`) in the public API, the visualizer allows Visual Studio to show useful internal information such as array contents and source properties during debugging.

---

# What the visualizer displays

With the visualizer installed, Visual Studio can show:

### For `DTWAIN_ARRAY`

* element count
* array data type
* numeric values
* string values
* nested arrays (when applicable)

### For `DTWAIN_SOURCE`

* device information
* source identity data
* JSON-formatted source diagnostics (when available)

---

# Required helper files

To enable full visualization support, your application must include the following files in its project:

```text
DebugUtils.c
DebugUtils.h
```

These helper files provide the runtime functions used by Visual Studio to retrieve debug information from DTWAIN handles.

Without these files, Visual Studio may only display raw pointer values instead of formatted debug output.

---

# Installing the visualizer

Copy:

```text
dtwain.natvis
```

to your Visual Studio visualizers folder:

### Visual Studio 2022

```text
Documents\Visual Studio 2022\Visualizers
```

### Visual Studio 2019

```text
Documents\Visual Studio 2019\Visualizers
```

If the folder does not exist, create it manually.

Restart Visual Studio after copying the file.

---

# Alternative: project-local installation

Instead of installing globally, you can attach the visualizer to a specific project:

1. Right-click the project
2. Select **Add → Existing Item**
3. Add `dtwain.natvis`

This allows the visualizer to travel with the source tree.

---

# Using the visualizer during debugging

Start debugging your application normally. When execution stops at a breakpoint, inspect variables such as:

```cpp
DTWAIN_ARRAY arr;
DTWAIN_SOURCE source;
```

inside:

* Autos window
* Locals window
* Watch window
* QuickWatch window

Visual Studio will automatically apply the DTWAIN visualizer.

---

# Viewing array contents manually

To explicitly generate a debug view of a `DTWAIN_ARRAY`, enter the following command in the **Watch Window** or **Immediate Window**:

```cpp
DTWAIN_CreateArrayDebugView(array_to_view)
```

Example:

```cpp
DTWAIN_CreateArrayDebugView(arr)
```

---

# Viewing source contents manually

To explicitly generate a debug view of a `DTWAIN_SOURCE`, enter:

```cpp
DTWAIN_CreateSourceDebugView(source_to_view)
```

Example:

```cpp
DTWAIN_CreateSourceDebugView(source)
```

---

# Refreshing debugger views

Visual Studio does not automatically refresh the debug display for these objects while stepping through code.

If values appear stale:

* re-evaluate the expression in the Watch window
* or step execution again
* or re-enter the helper function call

----

# Important notes

The visualizer:

* does **not** change application behavior
* is used only during debugging
* requires the helper functions from `DebugUtils.c`
* works best when debugging with symbols enabled

Visual Studio 2022 supports more advanced `.natvis` features than Visual Studio 2019, so the display experience may be richer in newer versions.
