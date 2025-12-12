1) The language binding for Rust will only support the following DTWAIN DLL's loaded at runtime:

dtwain32u.dll
dtwain32ud.dll
dtwain64u.dll
dtwain64ud.dll

Basically, only the Unicode versions of DTWAIN are compatible with the Rust language binding.  
Usage of the ANSI DLL's (dtwain32.dll, dtwain64.dll, etc.) will result in some DTWAIN function calls 
failing that handle string parameters.

2) The following dependency must be added to the Cargo.toml file:

[dependencies]
libloading = "0.9.0"
