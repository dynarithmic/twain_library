
# DTWAIN Visual Studio Debug Visualizer (`dtwain.natvis`)

The `dtwain.natvis` file is a Visual Studio debugger visualizer that enhances how DTWAIN handle-based types are displayed while debugging C/C++ applications.

It provides readable debug views for:

* `DTWAIN_ARRAY`
* `DTWAIN_SOURCE`

Since these DTWAIN types are opaque handles (`void*`) in the public API, the visualizer allows Visual Studio to present meaningful information such as array contents and source details during debugging.

---

# What the visualizer displays

With the visualizer installed, Visual Studio can show:

## DTWAIN_ARRAY

* number of elements
* array type (string, numeric, etc.)
* contents of the array
* nested arrays (when applicable)

## DTWAIN_SOURCE

* source/device information
* product name and identity fields
* diagnostic information (often JSON-formatted)

---

# Required helper files

To enable full functionality, your application must include:

```text
DebugUtils.c
DebugUtils.h
```

These files provide the helper functions used by the debugger to extract information from DTWAIN handles.

Without these files, the visualizer may not display useful data and may fall back to showing raw pointer values.

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

You can also add the visualizer directly to a project:

1. Right-click the project
2. Select **Add → Existing Item**
3. Add `dtwain.natvis`

This allows the visualizer to travel with your project source.

---

# Using the visualizer

Start debugging your application. When execution stops at a breakpoint, inspect variables such as:

```cpp
DTWAIN_ARRAY arr;
DTWAIN_SOURCE source;
```

in:

* Autos window
* Locals window
* Watch window
* QuickWatch window

Visual Studio will automatically apply the visualizer.

---

# Viewing DTWAIN_ARRAY manually

In the **Watch Window** or **Immediate Window**, enter:

```cpp
DTWAIN_CreateArrayDebugView(array_to_view)
```

Example:

```cpp
DTWAIN_CreateArrayDebugView(arr)
```

---

# Viewing DTWAIN_SOURCE manually

In the **Watch Window** or **Immediate Window**, enter:

```cpp
DTWAIN_CreateSourceDebugView(source_to_view)
```

Example:

```cpp
DTWAIN_CreateSourceDebugView(source)
```

---

# Refreshing debugger output

Visual Studio does **not automatically refresh** the contents of these debug views during single-stepping.

If values appear outdated:

* re-evaluate the expression in the Watch window
* step execution again
* or re-enter the helper function call

---

# Example project

The **DTWDEMO** project included with DTWAIN already contains:

```text
dtwain.natvis
DebugUtils.c
DebugUtils.h
```

and demonstrates proper setup and usage.

---

# Notes

* The visualizer affects **debug display only** (no runtime impact)
* Requires helper functions from `DebugUtils.c`
* Works best with debug symbols enabled
* Visual Studio 2022 provides a richer experience than Visual Studio 2019

---
