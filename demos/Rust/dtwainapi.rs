/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2026 Dynarithmic Software.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
    OF THIRD PARTY RIGHTS.
 */
#![allow(dead_code)]
#![allow(non_upper_case_globals)]
#![allow(non_snake_case)]
#![allow(non_camel_case_types)]

use std::ffi::{c_void, c_char};
use libloading::{Library, Symbol};

#[cfg(target_pointer_width = "64")]
type Dtwaincallbacktype = i64;

#[cfg(target_pointer_width = "64")]
type Dtwaincallbackreturntype = i64;

#[cfg(target_pointer_width = "32")]
type Dtwaincallbacktype = i32;

#[cfg(target_pointer_width = "32")]
type Dtwaincallbackreturntype = i32;

type DTWAIN_CALLBACK_PROC = extern "C" fn(Dtwaincallbacktype, Dtwaincallbacktype, Dtwaincallbacktype) -> Dtwaincallbackreturntype;
type DTWAIN_CALLBACK_PROC64 = extern "C" fn(Dtwaincallbacktype, Dtwaincallbacktype, i64) -> Dtwaincallbackreturntype;
type DTWAIN_DIBUPDATE_PROC = extern "C" fn(*const c_void, i32, *const c_void) -> *mut c_void;
type DTWAIN_LOGGER_PROC = extern "C" fn(*const u16, i64) -> Dtwaincallbackreturntype;
type DTWAIN_LOGGER_PROCA = extern "C" fn(*const c_char, i64) -> Dtwaincallbackreturntype;
type DTWAIN_LOGGER_PROCW = extern "C" fn(*const u16, i64) -> Dtwaincallbackreturntype;
type DTWAIN_ERROR_PROC = extern "C" fn(i32, i32) -> Dtwaincallbackreturntype;
type DTWAIN_ERROR_PROC64 = extern "C" fn(i32, i64) -> Dtwaincallbackreturntype;

type DtwainacquireaudiofileFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,i32,i32,*mut i32) -> i32;
type DtwainacquireaudiofileaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32,i32,i32,i32,*mut i32) -> i32;
type DtwainacquireaudiofilewFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,i32,i32,*mut i32) -> i32;
type DtwainacquireaudionativeFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,*mut i32) -> *mut c_void;
type DtwainacquireaudionativeexFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,*mut c_void,*mut i32) -> i32;
type DtwainacquirebufferedFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,*mut i32) -> *mut c_void;
type DtwainacquirebufferedexFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,*mut c_void,*mut i32) -> i32;
type DtwainacquirefileFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,i32,i32,i32,i32,*mut i32) -> i32;
type DtwainacquirefileaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32,i32,i32,i32,i32,i32,*mut i32) -> i32;
type DtwainacquirefileexFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,i32,i32,i32,i32,i32,i32,*mut i32) -> i32;
type DtwainacquirefilewFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,i32,i32,i32,i32,*mut i32) -> i32;
type DtwainacquirenativeFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,*mut i32) -> *mut c_void;
type DtwainacquirenativeexFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,*mut c_void,*mut i32) -> i32;
type DtwainacquiretoclipboardFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,i32,i32,*mut i32) -> *mut c_void;
type DtwainaddextimageinfoqueryFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainaddpdftextFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,*const u16,f64,i32,i32,f64,f64,f64,i32,u32) -> i32;
type DtwainaddpdftextaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32,i32,*const c_char,f64,i32,i32,f64,f64,f64,i32,u32) -> i32;
type DtwainaddpdftextexFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,u32) -> i32;
type DtwainaddpdftextwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,*const u16,f64,i32,i32,f64,f64,f64,i32,u32) -> i32;
type DtwainallocatememoryFunc = unsafe extern "C" fn(u32) -> *mut c_void;
type Dtwainallocatememory64Func = unsafe extern "C" fn(u64) -> *mut c_void;
type DtwainallocatememoryexFunc = unsafe extern "C" fn(u32) -> *mut c_void;
type DtwainapphandlesexceptionsFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainarrayansistringtofloatFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainarrayaddFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainarrayaddansistringFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainarrayaddansistringnFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32) -> i32;
type DtwainarrayaddfloatFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainarrayaddfloatnFunc = unsafe extern "C" fn(*mut c_void,f64,i32) -> i32;
type DtwainarrayaddfloatstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainarrayaddfloatstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainarrayaddfloatstringnFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainarrayaddfloatstringnaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32) -> i32;
type DtwainarrayaddfloatstringnwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainarrayaddfloatstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainarrayaddframeFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainarrayaddframenFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,i32) -> i32;
type DtwainarrayaddlongFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type Dtwainarrayaddlong64Func = unsafe extern "C" fn(*mut c_void,i64) -> i32;
type Dtwainarrayaddlong64nFunc = unsafe extern "C" fn(*mut c_void,i64,i32) -> i32;
type DtwainarrayaddlongnFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainarrayaddnFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,i32) -> i32;
type DtwainarrayaddstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainarrayaddstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainarrayaddstringnFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainarrayaddstringnaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32) -> i32;
type DtwainarrayaddstringnwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainarrayaddstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainarrayaddwidestringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainarrayaddwidestringnFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type Dtwainarrayconvertfix32tofloatFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type Dtwainarrayconvertfloattofix32Func = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainarraycopyFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainarraycreateFunc = unsafe extern "C" fn(i32,i32) -> *mut c_void;
type DtwainarraycreatecopyFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainarraycreatefromcapFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> *mut c_void;
type Dtwainarraycreatefromlong64sFunc = unsafe extern "C" fn(*mut i64,i32) -> *mut c_void;
type DtwainarraycreatefromlongsFunc = unsafe extern "C" fn(*mut i32,i32) -> *mut c_void;
type DtwainarraycreatefromrealsFunc = unsafe extern "C" fn(i32) -> *mut c_void;
type DtwainarraydestroyFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainarraydestroyframesFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainarrayfindFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainarrayfindansistringFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainarrayfindfloatFunc = unsafe extern "C" fn(*mut c_void,f64,f64) -> i32;
type DtwainarrayfindfloatstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16) -> i32;
type DtwainarrayfindfloatstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*const c_char) -> i32;
type DtwainarrayfindfloatstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16) -> i32;
type DtwainarrayfindlongFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type Dtwainarrayfindlong64Func = unsafe extern "C" fn(*mut c_void,i64) -> i32;
type DtwainarrayfindstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainarrayfindstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainarrayfindstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainarrayfindwidestringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type Dtwainarrayfix32getatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32,*mut i32) -> i32;
type Dtwainarrayfix32setatFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32) -> i32;
type DtwainarrayfloattoansistringFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainarrayfloattostringFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainarrayfloattowidestringFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainarraygetatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainarraygetatansistringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char) -> i32;
type DtwainarraygetatansistringptrFunc = unsafe extern "C" fn(*mut c_void,i32) -> *const c_char;
type DtwainarraygetatfloatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut f64) -> i32;
type DtwainarraygetatfloatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainarraygetatfloatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char) -> i32;
type DtwainarraygetatfloatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainarraygetatframeFunc = unsafe extern "C" fn(*mut c_void,i32,*mut f64,*mut f64,*mut f64,*mut f64) -> i32;
type DtwainarraygetatframeexFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainarraygetatframestringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwainarraygetatframestringaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char,*mut c_char,*mut c_char,*mut c_char) -> i32;
type DtwainarraygetatframestringwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwainarraygetatlongFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32) -> i32;
type Dtwainarraygetatlong64Func = unsafe extern "C" fn(*mut c_void,i32,*mut i64) -> i32;
type DtwainarraygetatsourceFunc = unsafe extern "C" fn(*mut c_void,i32,*mut *const ()) -> i32;
type DtwainarraygetatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainarraygetatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char) -> i32;
type DtwainarraygetatstringptrFunc = unsafe extern "C" fn(*mut c_void,i32) -> *const u16;
type DtwainarraygetatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainarraygetatwidestringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainarraygetatwidestringptrFunc = unsafe extern "C" fn(*mut c_void,i32) -> *const u16;
type DtwainarraygetbufferFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainarraygetcapvaluesFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> *mut c_void;
type DtwainarraygetcapvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32) -> *mut c_void;
type Dtwainarraygetcapvaluesex2Func = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32) -> *mut c_void;
type DtwainarraygetcountFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainarraygetmaxstringlengthFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainarraygetsourceatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut *const ()) -> i32;
type DtwainarraygetstringlengthFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainarraygettypeFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainarrayinitFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainarrayinsertatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainarrayinsertatansistringFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainarrayinsertatansistringnFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char,i32) -> i32;
type DtwainarrayinsertatfloatFunc = unsafe extern "C" fn(*mut c_void,i32,f64) -> i32;
type DtwainarrayinsertatfloatnFunc = unsafe extern "C" fn(*mut c_void,i32,f64,i32) -> i32;
type DtwainarrayinsertatfloatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarrayinsertatfloatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainarrayinsertatfloatstringnFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,i32) -> i32;
type DtwainarrayinsertatfloatstringnaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char,i32) -> i32;
type DtwainarrayinsertatfloatstringnwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,i32) -> i32;
type DtwainarrayinsertatfloatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarrayinsertatframeFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainarrayinsertatframenFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void,i32) -> i32;
type DtwainarrayinsertatlongFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type Dtwainarrayinsertatlong64Func = unsafe extern "C" fn(*mut c_void,i32,i64) -> i32;
type Dtwainarrayinsertatlong64nFunc = unsafe extern "C" fn(*mut c_void,i32,i64,i32) -> i32;
type DtwainarrayinsertatlongnFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32) -> i32;
type DtwainarrayinsertatnFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void,i32) -> i32;
type DtwainarrayinsertatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarrayinsertatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainarrayinsertatstringnFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,i32) -> i32;
type DtwainarrayinsertatstringnaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char,i32) -> i32;
type DtwainarrayinsertatstringnwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,i32) -> i32;
type DtwainarrayinsertatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarrayinsertatwidestringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarrayinsertatwidestringnFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,i32) -> i32;
type DtwainarrayremoveallFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainarrayremoveatFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainarrayremoveatnFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainarrayresizeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainarraysetatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainarraysetatansistringFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainarraysetatfloatFunc = unsafe extern "C" fn(*mut c_void,i32,f64) -> i32;
type DtwainarraysetatfloatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarraysetatfloatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainarraysetatfloatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarraysetatframeFunc = unsafe extern "C" fn(*mut c_void,i32,f64,f64,f64,f64) -> i32;
type DtwainarraysetatframeexFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainarraysetatframestringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainarraysetatframestringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char,*const c_char,*const c_char,*const c_char) -> i32;
type DtwainarraysetatframestringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainarraysetatlongFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type Dtwainarraysetatlong64Func = unsafe extern "C" fn(*mut c_void,i32,i64) -> i32;
type DtwainarraysetatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarraysetatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainarraysetatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarraysetatwidestringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainarraystringtofloatFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainarraywidestringtofloatFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwaincallcallbackFunc = unsafe extern "C" fn(i32,i32,i32) -> i32;
type Dtwaincallcallback64Func = unsafe extern "C" fn(i32,i32,i64) -> i32;
type DtwaincalldsmprocFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,i32,i32,i32,*mut c_void) -> i32;
type DtwaincheckhandlesFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainclearbuffersFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainclearerrorbufferFunc = unsafe extern "C" fn() -> i32;
type DtwainclearpdftextFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainclearpageFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainclosesourceFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainclosesourceuiFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainconvertdibtobitmapFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> *mut c_void;
type DtwainconvertdibtofullbitmapFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainconverttoapistringFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type DtwainconverttoapistringaFunc = unsafe extern "C" fn(*const c_char) -> *mut c_void;
type DtwainconverttoapistringexFunc = unsafe extern "C" fn(*const u16,*mut u16,i32) -> i32;
type DtwainconverttoapistringexaFunc = unsafe extern "C" fn(*const c_char,*mut c_char,i32) -> i32;
type DtwainconverttoapistringexwFunc = unsafe extern "C" fn(*const u16,*mut u16,i32) -> i32;
type DtwainconverttoapistringwFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type DtwaincreateacquisitionarrayFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwaincreatepdftextelementFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwaindeletedibFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaindestroyacquisitionarrayFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwaindestroypdftextelementFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaindisableappwindowFunc = unsafe extern "C" fn(*const c_void,i32) -> i32;
type DtwainenableautoborderdetectFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableautobrightFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableautodeskewFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableautofeedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableautorotateFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableautoscanFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableautomaticsensemediumFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableduplexFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenablefeederFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableindicatorFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenablejobfilehandlingFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenablelampFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenablemsgnotifyFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainenablepatchdetectFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenablepeekmessageloopFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenableprinterFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenablethumbnailFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainenabletripletsnotifyFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainendthreadFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainendtwainsessionFunc = unsafe extern "C" fn() -> i32;
type DtwainenumalarmvolumesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumalarmvolumesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumalarmsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumalarmsexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumaudioxfermechsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumaudioxfermechsexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumautofeedvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumautofeedvaluesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumautomaticcapturesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumautomaticcapturesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumautomaticsensemediumFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumautomaticsensemediumexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumbitdepthsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumbitdepthsexFunc = unsafe extern "C" fn(*mut c_void,i32,*mut *mut c_void) -> i32;
type Dtwainenumbitdepthsex2Func = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumbottomcamerasFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumbottomcamerasexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumbrightnessvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumbrightnessvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumcamerasFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumcamerasexFunc = unsafe extern "C" fn(*mut c_void,i32,*mut *mut c_void) -> i32;
type Dtwainenumcamerasex2Func = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type Dtwainenumcamerasex3Func = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumcompressiontypesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumcompressiontypesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type Dtwainenumcompressiontypesex2Func = unsafe extern "C" fn(*mut c_void,i32,i32) -> *mut c_void;
type DtwainenumcontrastvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumcontrastvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumcustomcapsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type Dtwainenumcustomcapsex2Func = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumdoublefeeddetectlengthsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumdoublefeeddetectlengthsexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumdoublefeeddetectvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumdoublefeeddetectvaluesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumextimageinfotypesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumextimageinfotypesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumextendedcapsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumextendedcapsexFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type Dtwainenumextendedcapsex2Func = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumfiletypebitsperpixelFunc = unsafe extern "C" fn(i32,*mut *mut c_void) -> i32;
type DtwainenumfilexferformatsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumfilexferformatsexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumhalftonesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumhalftonesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumhighlightvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumhighlightvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumjobcontrolsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumjobcontrolsexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumlightpathsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumlightpathsexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumlightsourcesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumlightsourcesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenummaxbuffersFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenummaxbuffersexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumnoisefiltersFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumnoisefiltersexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumocrinterfacesFunc = unsafe extern "C" fn(*mut *mut c_void) -> i32;
type DtwainenumocrsupportedcapsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumorientationsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumorientationsexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumoverscanvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumoverscanvaluesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpapersizesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpapersizesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpatchcodesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpatchcodesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpatchmaxprioritiesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpatchmaxprioritiesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpatchmaxretriesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpatchmaxretriesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpatchprioritiesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpatchprioritiesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpatchsearchmodesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpatchsearchmodesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpatchtimeoutvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpatchtimeoutvaluesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumpixeltypesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumpixeltypesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumprinterstringmodesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumprinterstringmodesexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumresolutionvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumresolutionvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumshadowvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumshadowvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumsourceunitsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumsourceunitsexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumsourcevaluesFunc = unsafe extern "C" fn(*mut c_void,*const u16,*mut *mut c_void,i32) -> i32;
type DtwainenumsourcevaluesaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*mut *mut c_void,i32) -> i32;
type DtwainenumsourcevalueswFunc = unsafe extern "C" fn(*mut c_void,*const u16,*mut *mut c_void,i32) -> i32;
type DtwainenumsourcesFunc = unsafe extern "C" fn(*mut *mut c_void) -> i32;
type DtwainenumsourcesexFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainenumsupportedcapsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumsupportedcapsexFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type Dtwainenumsupportedcapsex2Func = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumsupportedextimageinfoFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumsupportedextimageinfoexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumsupportedfiletypesFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainenumsupportedmultipagefiletypesFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainenumsupportedsinglepagefiletypesFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainenumthresholdvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumthresholdvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumtopcamerasFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumtopcamerasexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumtwainprintersFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumtwainprintersarrayFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainenumtwainprintersarrayexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumtwainprintersexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainenumxresolutionvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumxresolutionvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainenumyresolutionvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void,i32) -> i32;
type DtwainenumyresolutionvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainexecuteocrFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32) -> i32;
type DtwainexecuteocraFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32,i32) -> i32;
type DtwainexecuteocrwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32) -> i32;
type DtwainfeedpageFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainflipbitmapFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainflushacquiredpagesFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainforceacquirebitdepthFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainforcescanonnouiFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainframecreateFunc = unsafe extern "C" fn(f64,f64,f64,f64) -> *mut c_void;
type DtwainframecreatestringFunc = unsafe extern "C" fn(*const u16,*const u16,*const u16,*const u16) -> *mut c_void;
type DtwainframecreatestringaFunc = unsafe extern "C" fn(*const c_char,*const c_char,*const c_char,*const c_char) -> *mut c_void;
type DtwainframecreatestringwFunc = unsafe extern "C" fn(*const u16,*const u16,*const u16,*const u16) -> *mut c_void;
type DtwainframedestroyFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainframegetallFunc = unsafe extern "C" fn(*mut c_void,*mut f64,*mut f64,*mut f64,*mut f64) -> i32;
type DtwainframegetallstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwainframegetallstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,*mut c_char,*mut c_char,*mut c_char) -> i32;
type DtwainframegetallstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwainframegetvalueFunc = unsafe extern "C" fn(*mut c_void,i32,*mut f64) -> i32;
type DtwainframegetvaluestringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainframegetvaluestringaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char) -> i32;
type DtwainframegetvaluestringwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainframeisvalidFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainframesetallFunc = unsafe extern "C" fn(*mut c_void,f64,f64,f64,f64) -> i32;
type DtwainframesetallstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainframesetallstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*const c_char,*const c_char,*const c_char) -> i32;
type DtwainframesetallstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainframesetvalueFunc = unsafe extern "C" fn(*mut c_void,i32,f64) -> i32;
type DtwainframesetvaluestringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainframesetvaluestringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainframesetvaluestringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainfreeextimageinfoFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainfreememoryFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainfreememoryexFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetapihandlestatusFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetacquireareaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut *mut c_void) -> i32;
type Dtwaingetacquirearea2Func = unsafe extern "C" fn(*mut c_void,*mut f64,*mut f64,*mut f64,*mut f64,*mut i32) -> i32;
type Dtwaingetacquirearea2stringFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut u16,*mut u16,*mut i32) -> i32;
type Dtwaingetacquirearea2stringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,*mut c_char,*mut c_char,*mut c_char,*mut i32) -> i32;
type Dtwaingetacquirearea2stringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut u16,*mut u16,*mut i32) -> i32;
type DtwaingetacquireareaexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwaingetacquiremetricsFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32) -> i32;
type DtwaingetacquirestripbufferFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwaingetacquirestripdataFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut u32,*mut u32,*mut u32,*mut u32,*mut u32,*mut u32) -> i32;
type DtwaingetacquirestripsizesFunc = unsafe extern "C" fn(*mut c_void,*mut u32,*mut u32,*mut u32) -> i32;
type DtwaingetacquiredimageFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> *mut c_void;
type DtwaingetacquiredimagearrayFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwaingetactivedsmpathFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetactivedsmpathaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetactivedsmpathwFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetactivedsmversioninfoFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetactivedsmversioninfoaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetactivedsmversioninfowFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetalarmvolumeFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetallsourcedibsFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwaingetappinfoFunc = unsafe extern "C" fn(*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwaingetappinfoaFunc = unsafe extern "C" fn(*mut c_char,*mut c_char,*mut c_char,*mut c_char) -> i32;
type DtwaingetappinfowFunc = unsafe extern "C" fn(*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwaingetauthorFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetauthoraFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetauthorwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetbatteryminutesFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetbatterypercentFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetbitdepthFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetblankpageautodetectionFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetbrightnessFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetbrightnessstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetbrightnessstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetbrightnessstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetbufferedtransferinfoFunc = unsafe extern "C" fn(*mut c_void,*mut u32,*mut u32,*mut u32,*mut u32,*mut u32,*mut u32,*mut u32,*mut u32,*mut u32) -> *mut c_void;
type DtwaingetcallbackFunc = unsafe extern "C" fn() -> DTWAIN_CALLBACK_PROC;
type Dtwaingetcallback64Func = unsafe extern "C" fn() -> DTWAIN_CALLBACK_PROC64;
type DtwaingetcaparraytypeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwaingetcapcontainerFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwaingetcapcontainerexFunc = unsafe extern "C" fn(i32,i32,*mut *mut c_void) -> i32;
type Dtwaingetcapcontainerex2Func = unsafe extern "C" fn(i32,i32) -> *mut c_void;
type DtwaingetcapdatatypeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwaingetcapfromnameFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingetcapfromnameaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwaingetcapfromnamewFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingetcapoperationsFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32) -> i32;
type DtwaingetcapvaluesFunc = unsafe extern "C" fn(*mut c_void,i32,i32,*mut *mut c_void) -> i32;
type DtwaingetcapvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,*mut *mut c_void) -> i32;
type Dtwaingetcapvaluesex2Func = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,*mut *mut c_void) -> i32;
type DtwaingetcaptionFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetcaptionaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetcaptionwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetcompressionsizeFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetcompressiontypeFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetconditioncodestringFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetconditioncodestringaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingetconditioncodestringwFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetcontrastFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetcontraststringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetcontraststringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetcontraststringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetcountryFunc = unsafe extern "C" fn() -> i32;
type DtwaingetcurrentacquiredimageFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwaingetcurrentfilenameFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetcurrentfilenameaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetcurrentfilenamewFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetcurrentpagenumFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetcurrentretrycountFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetcurrenttwaintripletFunc = unsafe extern "C" fn(*mut *mut c_void,*mut *mut c_void,*mut i32,*mut i32,*mut i32,*mut i64) -> i32;
type DtwaingetcustomdsdataFunc = unsafe extern "C" fn(*mut c_void,*mut u8,u32,*mut u32,i32) -> *mut c_void;
type DtwaingetdsmfullnameFunc = unsafe extern "C" fn(i32,*mut u16,i32,*mut i32) -> i32;
type DtwaingetdsmfullnameaFunc = unsafe extern "C" fn(i32,*mut c_char,i32,*mut i32) -> i32;
type DtwaingetdsmfullnamewFunc = unsafe extern "C" fn(i32,*mut u16,i32,*mut i32) -> i32;
type DtwaingetdsmsearchorderFunc = unsafe extern "C" fn() -> i32;
type DtwaingetdtwainhandleFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwaingetdeviceeventFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetdeviceeventexFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut *mut c_void) -> i32;
type DtwaingetdeviceeventinfoFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwaingetdevicenotificationsFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetdevicetimedateFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetdevicetimedateaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetdevicetimedatewFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetdoublefeeddetectlengthFunc = unsafe extern "C" fn(*mut c_void,*mut f64,i32) -> i32;
type DtwaingetdoublefeeddetectvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwaingetduplextypeFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingeterrorbufferFunc = unsafe extern "C" fn(*mut *mut c_void) -> i32;
type DtwaingeterrorbufferthresholdFunc = unsafe extern "C" fn() -> i32;
type DtwaingeterrorcallbackFunc = unsafe extern "C" fn() -> DTWAIN_ERROR_PROC;
type Dtwaingeterrorcallback64Func = unsafe extern "C" fn() -> DTWAIN_ERROR_PROC64;
type DtwaingeterrorstringFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingeterrorstringaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingeterrorstringwFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetextcapfromnameFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingetextcapfromnameaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwaingetextcapfromnamewFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingetextimageinfoFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetextimageinfodataFunc = unsafe extern "C" fn(*mut c_void,i32,*mut *mut c_void) -> i32;
type DtwaingetextimageinfodataexFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwaingetextimageinfoitemFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetextimageinfoitemexFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetextnamefromcapFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetextnamefromcapaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingetextnamefromcapwFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetfeederalignmentFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetfeederfuncsFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetfeederorderFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetfeederwaittimeFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetfilecompressiontypeFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetfiletypeextensionsFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetfiletypeextensionsaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingetfiletypeextensionswFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetfiletypenameFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetfiletypenameaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingetfiletypenamewFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingethalftoneFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingethalftoneaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingethalftonewFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingethighlightFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingethighlightstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingethighlightstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingethighlightstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetimageinfoFunc = unsafe extern "C" fn(*mut c_void,*mut f64,*mut f64,*mut i32,*mut i32,*mut i32,*mut *mut c_void,*mut i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetimageinfostringFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut i32,*mut i32,*mut i32,*mut *mut c_void,*mut i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetimageinfostringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,*mut c_char,*mut i32,*mut i32,*mut i32,*mut *mut c_void,*mut i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetimageinfostringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut i32,*mut i32,*mut i32,*mut *mut c_void,*mut i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetjobcontrolFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetjpegvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32) -> i32;
type DtwaingetjpegxrvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32) -> i32;
type DtwaingetlanguageFunc = unsafe extern "C" fn() -> i32;
type DtwaingetlasterrorFunc = unsafe extern "C" fn() -> i32;
type DtwaingetlibrarypathFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetlibrarypathaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetlibrarypathwFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetlightpathFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetlightsourceFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetlightsourcesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwaingetloggercallbackFunc = unsafe extern "C" fn() -> DTWAIN_LOGGER_PROC;
type DtwaingetloggercallbackaFunc = unsafe extern "C" fn() -> DTWAIN_LOGGER_PROCA;
type DtwaingetloggercallbackwFunc = unsafe extern "C" fn() -> DTWAIN_LOGGER_PROCW;
type DtwaingetmanualduplexcountFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32) -> i32;
type DtwaingetmaxacquisitionsFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetmaxbuffersFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetmaxpagestoacquireFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetmaxretryattemptsFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetnamefromcapFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetnamefromcapaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingetnamefromcapwFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetnoisefilterFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetnumacquiredimagesFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwaingetnumacquisitionsFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetocrcapvaluesFunc = unsafe extern "C" fn(*mut c_void,i32,i32,*mut *mut c_void) -> i32;
type DtwaingetocrerrorstringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16,i32) -> i32;
type DtwaingetocrerrorstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char,i32) -> i32;
type DtwaingetocrerrorstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16,i32) -> i32;
type DtwaingetocrlasterrorFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetocrmajorminorversionFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32) -> i32;
type DtwaingetocrmanufacturerFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetocrmanufactureraFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetocrmanufacturerwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetocrproductfamilyFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetocrproductfamilyaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetocrproductfamilywFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetocrproductnameFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetocrproductnameaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetocrproductnamewFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetocrtextFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16,i32,*mut i32,i32) -> *mut c_void;
type DtwaingetocrtextaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char,i32,*mut i32,i32) -> *mut c_void;
type DtwaingetocrtextinfofloatFunc = unsafe extern "C" fn(*mut c_void,i32,i32,*mut f64) -> i32;
type DtwaingetocrtextinfofloatexFunc = unsafe extern "C" fn(*mut c_void,i32,*mut f64,i32) -> i32;
type DtwaingetocrtextinfohandleFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwaingetocrtextinfolongFunc = unsafe extern "C" fn(*mut c_void,i32,i32,*mut i32) -> i32;
type DtwaingetocrtextinfolongexFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32,i32) -> i32;
type DtwaingetocrtextwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16,i32,*mut i32,i32) -> *mut c_void;
type DtwaingetocrversioninfoFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetocrversioninfoaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetocrversioninfowFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetorientationFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetoverscanFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetpdftextelementfloatFunc = unsafe extern "C" fn(*mut c_void,*mut f64,*mut f64,i32) -> i32;
type DtwaingetpdftextelementlongFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32,i32) -> i32;
type DtwaingetpdftextelementstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32,i32) -> i32;
type DtwaingetpdftextelementstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32,i32) -> i32;
type DtwaingetpdftextelementstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32,i32) -> i32;
type Dtwaingetpdftype1fontnameFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type Dtwaingetpdftype1fontnameaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type Dtwaingetpdftype1fontnamewFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetpapersizeFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetpapersizenameFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetpapersizenameaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingetpapersizenamewFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetpatchmaxprioritiesFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetpatchmaxretriesFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetpatchprioritiesFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwaingetpatchsearchmodeFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetpatchtimeoutFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetpixelflavorFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetpixeltypeFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32,i32) -> i32;
type DtwaingetprinterFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetprinterstartnumberFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetprinterstringmodeFunc = unsafe extern "C" fn(*mut c_void,*mut i32,i32) -> i32;
type DtwaingetprinterstringsFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwaingetprintersuffixstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetprintersuffixstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetprintersuffixstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetregisteredmsgFunc = unsafe extern "C" fn() -> i32;
type DtwaingetresolutionFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetresolutionstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetresolutionstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetresolutionstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetresourcestringFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetresourcestringaFunc = unsafe extern "C" fn(i32,*mut c_char,i32) -> i32;
type DtwaingetresourcestringwFunc = unsafe extern "C" fn(i32,*mut u16,i32) -> i32;
type DtwaingetrotationFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetrotationstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetrotationstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetrotationstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetsavefilenameFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsavefilenameaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetsavefilenamewFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsavedfilescountFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaingetsessiondetailsFunc = unsafe extern "C" fn(*mut u16,i32,i32,i32) -> i32;
type DtwaingetsessiondetailsaFunc = unsafe extern "C" fn(*mut c_char,i32,i32,i32) -> i32;
type DtwaingetsessiondetailswFunc = unsafe extern "C" fn(*mut u16,i32,i32,i32) -> i32;
type DtwaingetshadowFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetshadowstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetshadowstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetshadowstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetshortversionstringFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetshortversionstringaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetshortversionstringwFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetsourceacquisitionsFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwaingetsourcedetailsFunc = unsafe extern "C" fn(*const u16,*mut u16,i32,i32,i32) -> i32;
type DtwaingetsourcedetailsaFunc = unsafe extern "C" fn(*const c_char,*mut c_char,i32,i32,i32) -> i32;
type DtwaingetsourcedetailswFunc = unsafe extern "C" fn(*const u16,*mut u16,i32,i32,i32) -> i32;
type DtwaingetsourceidFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwaingetsourceidexFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> *mut c_void;
type DtwaingetsourcemanufacturerFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourcemanufactureraFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetsourcemanufacturerwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourceproductfamilyFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourceproductfamilyaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetsourceproductfamilywFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourceproductnameFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourceproductnameaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetsourceproductnamewFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourceunitFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwaingetsourceversioninfoFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourceversioninfoaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,i32) -> i32;
type DtwaingetsourceversioninfowFunc = unsafe extern "C" fn(*mut c_void,*mut u16,i32) -> i32;
type DtwaingetsourceversionnumberFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32) -> i32;
type DtwaingetstaticlibversionFunc = unsafe extern "C" fn() -> i32;
type DtwaingettempfiledirectoryFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingettempfiledirectoryaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingettempfiledirectorywFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetthresholdFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetthresholdstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetthresholdstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetthresholdstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingettimedateFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingettimedateaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingettimedatewFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingettwainappidFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwaingettwainappidexFunc = unsafe extern "C" fn(*mut *mut c_void) -> *mut c_void;
type DtwaingettwainavailabilityFunc = unsafe extern "C" fn() -> i32;
type DtwaingettwainavailabilityexFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingettwainavailabilityexaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingettwainavailabilityexwFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingettwaincountrynameFunc = unsafe extern "C" fn(i32,*mut u16) -> i32;
type DtwaingettwaincountrynameaFunc = unsafe extern "C" fn(i32,*mut c_char) -> i32;
type DtwaingettwaincountrynamewFunc = unsafe extern "C" fn(i32,*mut u16) -> i32;
type DtwaingettwaincountryvalueFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingettwaincountryvalueaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwaingettwaincountryvaluewFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingettwainhwndFunc = unsafe extern "C" fn() -> *const c_void;
type DtwaingettwainidfromnameFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingettwainidfromnameaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwaingettwainidfromnamewFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingettwainlanguagenameFunc = unsafe extern "C" fn(i32,*mut u16) -> i32;
type DtwaingettwainlanguagenameaFunc = unsafe extern "C" fn(i32,*mut c_char) -> i32;
type DtwaingettwainlanguagenamewFunc = unsafe extern "C" fn(i32,*mut u16) -> i32;
type DtwaingettwainlanguagevalueFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingettwainlanguagevalueaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwaingettwainlanguagevaluewFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwaingettwainmodeFunc = unsafe extern "C" fn() -> i32;
type DtwaingettwainnamefromconstantFunc = unsafe extern "C" fn(i32,i32,*mut u16,i32) -> i32;
type DtwaingettwainnamefromconstantaFunc = unsafe extern "C" fn(i32,i32,*mut c_char,i32) -> i32;
type DtwaingettwainnamefromconstantwFunc = unsafe extern "C" fn(i32,i32,*mut u16,i32) -> i32;
type DtwaingettwainstringnameFunc = unsafe extern "C" fn(i32,i32,*mut u16,i32) -> i32;
type DtwaingettwainstringnameaFunc = unsafe extern "C" fn(i32,i32,*mut c_char,i32) -> i32;
type DtwaingettwainstringnamewFunc = unsafe extern "C" fn(i32,i32,*mut u16,i32) -> i32;
type DtwaingettwaintimeoutFunc = unsafe extern "C" fn() -> i32;
type DtwaingetversionFunc = unsafe extern "C" fn(*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetversioncopyrightFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetversioncopyrightaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetversioncopyrightwFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetversionexFunc = unsafe extern "C" fn(*mut i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwaingetversioninfoFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetversioninfoaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetversioninfowFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetversionstringFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetversionstringaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetversionstringwFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetwindowsversioninfoFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetwindowsversioninfoaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwaingetwindowsversioninfowFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwaingetxresolutionFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetxresolutionstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetxresolutionstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetxresolutionstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetyresolutionFunc = unsafe extern "C" fn(*mut c_void,*mut f64) -> i32;
type DtwaingetyresolutionstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaingetyresolutionstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char) -> i32;
type DtwaingetyresolutionstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16) -> i32;
type DtwaininitextimageinfoFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwaininitimagefileappendFunc = unsafe extern "C" fn(*const u16,i32) -> i32;
type DtwaininitimagefileappendaFunc = unsafe extern "C" fn(*const c_char,i32) -> i32;
type DtwaininitimagefileappendwFunc = unsafe extern "C" fn(*const u16,i32) -> i32;
type DtwaininitocrinterfaceFunc = unsafe extern "C" fn() -> i32;
type DtwainisacquiringFunc = unsafe extern "C" fn() -> i32;
type DtwainisaudioxfersupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainisautoborderdetectenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautoborderdetectsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautobrightenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautobrightsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautodeskewenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautodeskewsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautofeedenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautofeedsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautorotateenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautorotatesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautoscanenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautomaticsensemediumenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisautomaticsensemediumsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisblankpagedetectiononFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisbufferedtilemodeonFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisbufferedtilemodesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainiscapsupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainiscompressionsupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainiscustomdsdatasupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisdibblankFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainisdibblankstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainisdibblankstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainisdibblankstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainisdeviceeventsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisdeviceonlineFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisdoublefeeddetectlengthsupportedFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainisdoublefeeddetectsupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainisduplexenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisduplexsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisextimageinfosupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisfeederenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisfeederloadedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisfeedersensitiveFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisfeedersupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisfilesystemsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisfilexfersupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainisiafieldalastpagesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldalevelsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldaprintformatsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldavaluesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldblastpagesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldblevelsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldbprintformatsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldbvaluesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldclastpagesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldclevelsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldcprintformatsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldcvaluesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafielddlastpagesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafielddlevelsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafielddprintformatsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafielddvaluesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldelastpagesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldelevelsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldeprintformatsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisiafieldevaluesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisimageaddressingsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisindicatorenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisindicatorsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisinitializedFunc = unsafe extern "C" fn() -> i32;
type DtwainisjpegsupportedFunc = unsafe extern "C" fn() -> i32;
type DtwainisjobcontrolsupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainislampenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainislampsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainislightpathsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainislightsourcesupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainismaxbufferssupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainismemfilexfersupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainismsgnotifyenabledFunc = unsafe extern "C" fn() -> i32;
type DtwainisnotifytripletsenabledFunc = unsafe extern "C" fn() -> i32;
type DtwainisocrengineactivatedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisopensourcesonselectFunc = unsafe extern "C" fn() -> i32;
type DtwainisorientationsupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainisoverscansupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainispdfsupportedFunc = unsafe extern "C" fn() -> i32;
type DtwainispngsupportedFunc = unsafe extern "C" fn() -> i32;
type DtwainispaperdetectableFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainispapersizesupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainispatchcapssupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainispatchdetectenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainispatchsupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainispeekmessageloopenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainispixeltypesupportedFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainisprinterenabledFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainisprintersupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisrotationsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainissessionenabledFunc = unsafe extern "C" fn() -> i32;
type DtwainisskipimageinfoerrorFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainissourceacquiringFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainissourceacquiringexFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainissourceinuionlymodeFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainissourceopenFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainissourceselectedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainissourcevalidFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainistiffsupportedFunc = unsafe extern "C" fn() -> i32;
type DtwainisthumbnailenabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisthumbnailsupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainistwainavailableFunc = unsafe extern "C" fn() -> i32;
type DtwainistwainavailableexFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwainistwainavailableexaFunc = unsafe extern "C" fn(*mut c_char,i32) -> i32;
type DtwainistwainavailableexwFunc = unsafe extern "C" fn(*mut u16,i32) -> i32;
type DtwainisuicontrollableFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisuienabledFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainisuionlysupportedFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainloadcustomstringresourcesFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainloadcustomstringresourcesaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwainloadcustomstringresourcesexFunc = unsafe extern "C" fn(*const u16,i32) -> i32;
type DtwainloadcustomstringresourcesexaFunc = unsafe extern "C" fn(*const c_char,i32) -> i32;
type DtwainloadcustomstringresourcesexwFunc = unsafe extern "C" fn(*const u16,i32) -> i32;
type DtwainloadcustomstringresourceswFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainloadlanguageresourceFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainlockmemoryFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainlockmemoryexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainlogmessageFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainlogmessageaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwainlogmessagewFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainmakergbFunc = unsafe extern "C" fn(i32,i32,i32) -> i32;
type DtwainopensourceFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainopensourcesonselectFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainrangecreateFunc = unsafe extern "C" fn(i32) -> *mut c_void;
type DtwainrangecreatefromcapFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainrangedestroyFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainrangeexpandFunc = unsafe extern "C" fn(*mut c_void,*mut *mut c_void) -> i32;
type DtwainrangeexpandexFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainrangegetallFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,*mut c_void,*mut c_void,*mut c_void,*mut c_void) -> i32;
type DtwainrangegetallfloatFunc = unsafe extern "C" fn(*mut c_void,*mut f64,*mut f64,*mut f64,*mut f64,*mut f64) -> i32;
type DtwainrangegetallfloatstringFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwainrangegetallfloatstringaFunc = unsafe extern "C" fn(*mut c_void,*mut c_char,*mut c_char,*mut c_char,*mut c_char,*mut c_char) -> i32;
type DtwainrangegetallfloatstringwFunc = unsafe extern "C" fn(*mut c_void,*mut u16,*mut u16,*mut u16,*mut u16,*mut u16) -> i32;
type DtwainrangegetalllongFunc = unsafe extern "C" fn(*mut c_void,*mut i32,*mut i32,*mut i32,*mut i32,*mut i32) -> i32;
type DtwainrangegetcountFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainrangegetexpvalueFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainrangegetexpvaluefloatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut f64) -> i32;
type DtwainrangegetexpvaluefloatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainrangegetexpvaluefloatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char) -> i32;
type DtwainrangegetexpvaluefloatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainrangegetexpvaluelongFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32) -> i32;
type DtwainrangegetnearestvalueFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,*mut c_void,i32) -> i32;
type DtwainrangegetposFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,*mut i32) -> i32;
type DtwainrangegetposfloatFunc = unsafe extern "C" fn(*mut c_void,f64,*mut i32) -> i32;
type DtwainrangegetposfloatstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,*mut i32) -> i32;
type DtwainrangegetposfloatstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*mut i32) -> i32;
type DtwainrangegetposfloatstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,*mut i32) -> i32;
type DtwainrangegetposlongFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32) -> i32;
type DtwainrangegetvalueFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainrangegetvaluefloatFunc = unsafe extern "C" fn(*mut c_void,i32,*mut f64) -> i32;
type DtwainrangegetvaluefloatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainrangegetvaluefloatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_char) -> i32;
type DtwainrangegetvaluefloatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*mut u16) -> i32;
type DtwainrangegetvaluelongFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32) -> i32;
type DtwainrangeisvalidFunc = unsafe extern "C" fn(*mut c_void,*mut i32) -> i32;
type DtwainrangenearestvaluefloatFunc = unsafe extern "C" fn(*mut c_void,f64,*mut f64,i32) -> i32;
type DtwainrangenearestvaluefloatstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,*mut u16,i32) -> i32;
type DtwainrangenearestvaluefloatstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*mut c_char,i32) -> i32;
type DtwainrangenearestvaluefloatstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,*mut u16,i32) -> i32;
type DtwainrangenearestvaluelongFunc = unsafe extern "C" fn(*mut c_void,i32,*mut i32,i32) -> i32;
type DtwainrangesetallFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,*mut c_void,*mut c_void,*mut c_void,*mut c_void) -> i32;
type DtwainrangesetallfloatFunc = unsafe extern "C" fn(*mut c_void,f64,f64,f64,f64,f64) -> i32;
type DtwainrangesetallfloatstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainrangesetallfloatstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*const c_char,*const c_char,*const c_char,*const c_char) -> i32;
type DtwainrangesetallfloatstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainrangesetalllongFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,i32) -> i32;
type DtwainrangesetvalueFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void) -> i32;
type DtwainrangesetvaluefloatFunc = unsafe extern "C" fn(*mut c_void,i32,f64) -> i32;
type DtwainrangesetvaluefloatstringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainrangesetvaluefloatstringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char) -> i32;
type DtwainrangesetvaluefloatstringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16) -> i32;
type DtwainrangesetvaluelongFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainresetpdftextelementFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainrewindpageFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainselectdefaultocrengineFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainselectdefaultsourceFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainselectdefaultsourcewithopenFunc = unsafe extern "C" fn(i32) -> *mut c_void;
type DtwainselectocrengineFunc = unsafe extern "C" fn() -> *mut c_void;
type Dtwainselectocrengine2Func = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,i32) -> *mut c_void;
type Dtwainselectocrengine2aFunc = unsafe extern "C" fn(*const c_void,*const c_char,i32,i32,i32) -> *mut c_void;
type Dtwainselectocrengine2exFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,*const u16,*const u16,*const u16,i32) -> *mut c_void;
type Dtwainselectocrengine2exaFunc = unsafe extern "C" fn(*const c_void,*const c_char,i32,i32,*const c_char,*const c_char,*const c_char,i32) -> *mut c_void;
type Dtwainselectocrengine2exwFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,*const u16,*const u16,*const u16,i32) -> *mut c_void;
type Dtwainselectocrengine2wFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,i32) -> *mut c_void;
type DtwainselectocrenginebynameFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type DtwainselectocrenginebynameaFunc = unsafe extern "C" fn(*const c_char) -> *mut c_void;
type DtwainselectocrenginebynamewFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type DtwainselectsourceFunc = unsafe extern "C" fn() -> *mut c_void;
type Dtwainselectsource2Func = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,i32) -> *mut c_void;
type Dtwainselectsource2aFunc = unsafe extern "C" fn(*const c_void,*const c_char,i32,i32,i32) -> *mut c_void;
type Dtwainselectsource2exFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,*const u16,*const u16,*const u16,i32) -> *mut c_void;
type Dtwainselectsource2exaFunc = unsafe extern "C" fn(*const c_void,*const c_char,i32,i32,*const c_char,*const c_char,*const c_char,i32) -> *mut c_void;
type Dtwainselectsource2exwFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,*const u16,*const u16,*const u16,i32) -> *mut c_void;
type Dtwainselectsource2wFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,i32) -> *mut c_void;
type DtwainselectsourcebynameFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type DtwainselectsourcebynameaFunc = unsafe extern "C" fn(*const c_char) -> *mut c_void;
type DtwainselectsourcebynamewFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type DtwainselectsourcebynamewithopenFunc = unsafe extern "C" fn(*const u16,i32) -> *mut c_void;
type DtwainselectsourcebynamewithopenaFunc = unsafe extern "C" fn(*const c_char,i32) -> *mut c_void;
type DtwainselectsourcebynamewithopenwFunc = unsafe extern "C" fn(*const u16,i32) -> *mut c_void;
type DtwainselectsourcewithopenFunc = unsafe extern "C" fn(i32) -> *mut c_void;
type DtwainsetacquireareaFunc = unsafe extern "C" fn(*mut c_void,i32,*mut c_void,*mut c_void) -> i32;
type Dtwainsetacquirearea2Func = unsafe extern "C" fn(*mut c_void,f64,f64,f64,f64,i32,i32) -> i32;
type Dtwainsetacquirearea2stringFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16,*const u16,i32,i32) -> i32;
type Dtwainsetacquirearea2stringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*const c_char,*const c_char,*const c_char,i32,i32) -> i32;
type Dtwainsetacquirearea2stringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16,*const u16,i32,i32) -> i32;
type DtwainsetacquireimagenegativeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetacquireimagescaleFunc = unsafe extern "C" fn(*mut c_void,f64,f64) -> i32;
type DtwainsetacquireimagescalestringFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16) -> i32;
type DtwainsetacquireimagescalestringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*const c_char) -> i32;
type DtwainsetacquireimagescalestringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16) -> i32;
type DtwainsetacquirestripbufferFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainsetacquirestripsizeFunc = unsafe extern "C" fn(*mut c_void,u32) -> i32;
type DtwainsetalarmvolumeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetalarmsFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainsetallcapstodefaultFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainsetappinfoFunc = unsafe extern "C" fn(*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainsetappinfoaFunc = unsafe extern "C" fn(*const c_char,*const c_char,*const c_char,*const c_char) -> i32;
type DtwainsetappinfowFunc = unsafe extern "C" fn(*const u16,*const u16,*const u16,*const u16) -> i32;
type DtwainsetauthorFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetauthoraFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetauthorwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetavailableprintersFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetavailableprintersarrayFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainsetbitdepthFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetblankpagedetectionFunc = unsafe extern "C" fn(*mut c_void,f64,i32,i32) -> i32;
type DtwainsetblankpagedetectionexFunc = unsafe extern "C" fn(*mut c_void,f64,i32,i32,i32) -> i32;
type DtwainsetblankpagedetectionexstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,i32) -> i32;
type DtwainsetblankpagedetectionexstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32,i32,i32) -> i32;
type DtwainsetblankpagedetectionexstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32,i32) -> i32;
type DtwainsetblankpagedetectionstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32) -> i32;
type DtwainsetblankpagedetectionstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32,i32) -> i32;
type DtwainsetblankpagedetectionstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32,i32) -> i32;
type DtwainsetbrightnessFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetbrightnessstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetbrightnessstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetbrightnessstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetbufferedtilemodeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetcallbackFunc = unsafe extern "C" fn(DTWAIN_CALLBACK_PROC,i32) -> DTWAIN_CALLBACK_PROC;
type Dtwainsetcallback64Func = unsafe extern "C" fn(DTWAIN_CALLBACK_PROC64,i64) -> DTWAIN_CALLBACK_PROC64;
type DtwainsetcameraFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetcameraaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetcamerawFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetcapvaluesFunc = unsafe extern "C" fn(*mut c_void,i32,i32,*mut c_void) -> i32;
type DtwainsetcapvaluesexFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,*mut c_void) -> i32;
type Dtwainsetcapvaluesex2Func = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,*mut c_void) -> i32;
type DtwainsetcaptionFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetcaptionaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetcaptionwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetcompressiontypeFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetcontrastFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetcontraststringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetcontraststringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetcontraststringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetcountryFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsetcurrentretrycountFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetcustomdsdataFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,*const u8,u32,i32) -> i32;
type DtwainsetdsmsearchorderFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsetdsmsearchorderexFunc = unsafe extern "C" fn(*const u16,*const u16) -> i32;
type DtwainsetdsmsearchorderexaFunc = unsafe extern "C" fn(*const c_char,*const c_char) -> i32;
type DtwainsetdsmsearchorderexwFunc = unsafe extern "C" fn(*const u16,*const u16) -> i32;
type DtwainsetdefaultsourceFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainsetdevicenotificationsFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetdevicetimedateFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetdevicetimedateaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetdevicetimedatewFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetdoublefeeddetectlengthFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetdoublefeeddetectlengthstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetdoublefeeddetectlengthstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetdoublefeeddetectlengthstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetdoublefeeddetectvaluesFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainsetdoublepagecountonduplexFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainseteojdetectvalueFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainseterrorbufferthresholdFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainseterrorcallbackFunc = unsafe extern "C" fn(DTWAIN_ERROR_PROC,i32) -> i32;
type Dtwainseterrorcallback64Func = unsafe extern "C" fn(DTWAIN_ERROR_PROC64,i64) -> i32;
type DtwainsetfeederalignmentFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetfeederorderFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetfeederwaittimeFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetfileautoincrementFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32) -> i32;
type DtwainsetfilecompressiontypeFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetfilesaveposFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,i32) -> i32;
type DtwainsetfilesaveposaFunc = unsafe extern "C" fn(*const c_void,*const c_char,i32,i32,i32) -> i32;
type DtwainsetfilesaveposwFunc = unsafe extern "C" fn(*const c_void,*const u16,i32,i32,i32) -> i32;
type DtwainsetfilexferformatFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsethalftoneFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsethalftoneaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsethalftonewFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsethighlightFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsethighlightstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsethighlightstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsethighlightstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetjobcontrolFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetjpegvaluesFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetjpegxrvaluesFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetlanguageFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsetlasterrorFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsetlightpathFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetlightpathexFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainsetlightsourceFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetlightsourcesFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainsetloggercallbackFunc = unsafe extern "C" fn(DTWAIN_LOGGER_PROC,i64) -> i32;
type DtwainsetloggercallbackaFunc = unsafe extern "C" fn(DTWAIN_LOGGER_PROCA,i64) -> i32;
type DtwainsetloggercallbackwFunc = unsafe extern "C" fn(DTWAIN_LOGGER_PROCW,i64) -> i32;
type DtwainsetmanualduplexmodeFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetmaxacquisitionsFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetmaxbuffersFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetmaxretryattemptsFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetmultipagescanmodeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetnoisefilterFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetocrcapvaluesFunc = unsafe extern "C" fn(*mut c_void,i32,i32,*mut c_void) -> i32;
type DtwainsetorientationFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetoverscanFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetpdfaesencryptionFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetpdfasciicompressionFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpdfauthorFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfauthoraFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetpdfauthorwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfcompressionFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpdfcreatorFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfcreatoraFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetpdfcreatorwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfencryptionFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16,u32,i32) -> i32;
type DtwainsetpdfencryptionaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char,*const c_char,u32,i32) -> i32;
type DtwainsetpdfencryptionwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16,u32,i32) -> i32;
type DtwainsetpdfjpegqualityFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpdfkeywordsFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfkeywordsaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetpdfkeywordswFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfocrconversionFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32,i32,i32) -> i32;
type DtwainsetpdfocrmodeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpdforientationFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpdfpagescaleFunc = unsafe extern "C" fn(*mut c_void,i32,f64,f64) -> i32;
type DtwainsetpdfpagescalestringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16) -> i32;
type DtwainsetpdfpagescalestringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char,*const c_char) -> i32;
type DtwainsetpdfpagescalestringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16) -> i32;
type DtwainsetpdfpagesizeFunc = unsafe extern "C" fn(*mut c_void,i32,f64,f64) -> i32;
type DtwainsetpdfpagesizestringFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16) -> i32;
type DtwainsetpdfpagesizestringaFunc = unsafe extern "C" fn(*mut c_void,i32,*const c_char,*const c_char) -> i32;
type DtwainsetpdfpagesizestringwFunc = unsafe extern "C" fn(*mut c_void,i32,*const u16,*const u16) -> i32;
type DtwainsetpdfpolarityFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpdfproducerFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfproduceraFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetpdfproducerwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfsubjectFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdfsubjectaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetpdfsubjectwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdftextelementfloatFunc = unsafe extern "C" fn(*mut c_void,f64,f64,i32) -> i32;
type DtwainsetpdftextelementlongFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32) -> i32;
type DtwainsetpdftextelementstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainsetpdftextelementstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32) -> i32;
type DtwainsetpdftextelementstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainsetpdftitleFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpdftitleaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetpdftitlewFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpapersizeFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetpatchmaxprioritiesFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpatchmaxretriesFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpatchprioritiesFunc = unsafe extern "C" fn(*mut c_void,*mut c_void) -> i32;
type DtwainsetpatchsearchmodeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpatchtimeoutFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpixelflavorFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetpixeltypeFunc = unsafe extern "C" fn(*mut c_void,i32,i32,i32) -> i32;
type DtwainsetpostscripttitleFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpostscripttitleaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetpostscripttitlewFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetpostscripttypeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetprinterFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetprinterexFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetprinterstartnumberFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsetprinterstringmodeFunc = unsafe extern "C" fn(*mut c_void,i32,i32) -> i32;
type DtwainsetprinterstringsFunc = unsafe extern "C" fn(*mut c_void,*mut c_void,*mut i32) -> i32;
type DtwainsetprintersuffixstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetprintersuffixstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetprintersuffixstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetquerycapsupportFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsetresolutionFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetresolutionstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetresolutionstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetresolutionstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetresourcepathFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainsetresourcepathaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwainsetresourcepathwFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainsetrotationFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetrotationstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetrotationstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetrotationstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetsavefilenameFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetsavefilenameaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetsavefilenamewFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetshadowFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetshadowstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetshadowstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetshadowstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetsourceunitFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsettiffcompresstypeFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsettiffinvertFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainsettempfiledirectoryFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainsettempfiledirectoryaFunc = unsafe extern "C" fn(*const c_char) -> i32;
type DtwainsettempfiledirectoryexFunc = unsafe extern "C" fn(*const u16,i32) -> i32;
type DtwainsettempfiledirectoryexaFunc = unsafe extern "C" fn(*const c_char,i32) -> i32;
type DtwainsettempfiledirectoryexwFunc = unsafe extern "C" fn(*const u16,i32) -> i32;
type DtwainsettempfiledirectorywFunc = unsafe extern "C" fn(*const u16) -> i32;
type DtwainsetthresholdFunc = unsafe extern "C" fn(*mut c_void,f64,i32) -> i32;
type DtwainsetthresholdstringFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainsetthresholdstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char,i32) -> i32;
type DtwainsetthresholdstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16,i32) -> i32;
type DtwainsettwaindsmFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsettwainlogFunc = unsafe extern "C" fn(u32,*const u16) -> i32;
type DtwainsettwainlogaFunc = unsafe extern "C" fn(u32,*const c_char) -> i32;
type DtwainsettwainlogwFunc = unsafe extern "C" fn(u32,*const u16) -> i32;
type DtwainsettwainmodeFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsettwaintimeoutFunc = unsafe extern "C" fn(i32) -> i32;
type DtwainsetupdatedibprocFunc = unsafe extern "C" fn(DTWAIN_DIBUPDATE_PROC) -> DTWAIN_DIBUPDATE_PROC;
type DtwainsetxresolutionFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetxresolutionstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetxresolutionstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetxresolutionstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetyresolutionFunc = unsafe extern "C" fn(*mut c_void,f64) -> i32;
type DtwainsetyresolutionstringFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainsetyresolutionstringaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> i32;
type DtwainsetyresolutionstringwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> i32;
type DtwainshowuionlyFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainshutdownocrengineFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainskipimageinfoerrorFunc = unsafe extern "C" fn(*mut c_void,i32) -> i32;
type DtwainstartthreadFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainstarttwainsessionFunc = unsafe extern "C" fn(*const c_void,*const u16) -> i32;
type DtwainstarttwainsessionaFunc = unsafe extern "C" fn(*const c_void,*const c_char) -> i32;
type DtwainstarttwainsessionwFunc = unsafe extern "C" fn(*const c_void,*const u16) -> i32;
type DtwainsysdestroyFunc = unsafe extern "C" fn() -> i32;
type DtwainsysinitializeFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwainsysinitializeexFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type Dtwainsysinitializeex2Func = unsafe extern "C" fn(*const u16,*const u16,*const u16) -> *mut c_void;
type Dtwainsysinitializeex2aFunc = unsafe extern "C" fn(*const c_char,*const c_char,*const c_char) -> *mut c_void;
type Dtwainsysinitializeex2wFunc = unsafe extern "C" fn(*const u16,*const u16,*const u16) -> *mut c_void;
type DtwainsysinitializeexaFunc = unsafe extern "C" fn(*const c_char) -> *mut c_void;
type DtwainsysinitializeexwFunc = unsafe extern "C" fn(*const u16) -> *mut c_void;
type DtwainsysinitializelibFunc = unsafe extern "C" fn(*mut c_void) -> *mut c_void;
type DtwainsysinitializelibexFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> *mut c_void;
type Dtwainsysinitializelibex2Func = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16) -> *mut c_void;
type Dtwainsysinitializelibex2aFunc = unsafe extern "C" fn(*mut c_void,*const c_char,*const c_char,*const c_char) -> *mut c_void;
type Dtwainsysinitializelibex2wFunc = unsafe extern "C" fn(*mut c_void,*const u16,*const u16,*const u16) -> *mut c_void;
type DtwainsysinitializelibexaFunc = unsafe extern "C" fn(*mut c_void,*const c_char) -> *mut c_void;
type DtwainsysinitializelibexwFunc = unsafe extern "C" fn(*mut c_void,*const u16) -> *mut c_void;
type DtwainsysinitializenoblockingFunc = unsafe extern "C" fn() -> *mut c_void;
type DtwaintestgetcapFunc = unsafe extern "C" fn(*mut c_void,i32) -> *mut c_void;
type DtwainunlockmemoryFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainunlockmemoryexFunc = unsafe extern "C" fn(*mut c_void) -> i32;
type DtwainusemultiplethreadsFunc = unsafe extern "C" fn(i32) -> i32;
pub struct DTwainAPI<'a>
{
    DTWAIN_AcquireAudioFileFunc: Symbol<'a, DtwainacquireaudiofileFunc>,
    DTWAIN_AcquireAudioFileAFunc: Symbol<'a, DtwainacquireaudiofileaFunc>,
    DTWAIN_AcquireAudioFileWFunc: Symbol<'a, DtwainacquireaudiofilewFunc>,
    DTWAIN_AcquireAudioNativeFunc: Symbol<'a, DtwainacquireaudionativeFunc>,
    DTWAIN_AcquireAudioNativeExFunc: Symbol<'a, DtwainacquireaudionativeexFunc>,
    DTWAIN_AcquireBufferedFunc: Symbol<'a, DtwainacquirebufferedFunc>,
    DTWAIN_AcquireBufferedExFunc: Symbol<'a, DtwainacquirebufferedexFunc>,
    DTWAIN_AcquireFileFunc: Symbol<'a, DtwainacquirefileFunc>,
    DTWAIN_AcquireFileAFunc: Symbol<'a, DtwainacquirefileaFunc>,
    DTWAIN_AcquireFileExFunc: Symbol<'a, DtwainacquirefileexFunc>,
    DTWAIN_AcquireFileWFunc: Symbol<'a, DtwainacquirefilewFunc>,
    DTWAIN_AcquireNativeFunc: Symbol<'a, DtwainacquirenativeFunc>,
    DTWAIN_AcquireNativeExFunc: Symbol<'a, DtwainacquirenativeexFunc>,
    DTWAIN_AcquireToClipboardFunc: Symbol<'a, DtwainacquiretoclipboardFunc>,
    DTWAIN_AddExtImageInfoQueryFunc: Symbol<'a, DtwainaddextimageinfoqueryFunc>,
    DTWAIN_AddPDFTextFunc: Symbol<'a, DtwainaddpdftextFunc>,
    DTWAIN_AddPDFTextAFunc: Symbol<'a, DtwainaddpdftextaFunc>,
    DTWAIN_AddPDFTextExFunc: Symbol<'a, DtwainaddpdftextexFunc>,
    DTWAIN_AddPDFTextWFunc: Symbol<'a, DtwainaddpdftextwFunc>,
    DTWAIN_AllocateMemoryFunc: Symbol<'a, DtwainallocatememoryFunc>,
    DTWAIN_AllocateMemory64Func: Symbol<'a, Dtwainallocatememory64Func>,
    DTWAIN_AllocateMemoryExFunc: Symbol<'a, DtwainallocatememoryexFunc>,
    DTWAIN_AppHandlesExceptionsFunc: Symbol<'a, DtwainapphandlesexceptionsFunc>,
    DTWAIN_ArrayANSIStringToFloatFunc: Symbol<'a, DtwainarrayansistringtofloatFunc>,
    DTWAIN_ArrayAddFunc: Symbol<'a, DtwainarrayaddFunc>,
    DTWAIN_ArrayAddANSIStringFunc: Symbol<'a, DtwainarrayaddansistringFunc>,
    DTWAIN_ArrayAddANSIStringNFunc: Symbol<'a, DtwainarrayaddansistringnFunc>,
    DTWAIN_ArrayAddFloatFunc: Symbol<'a, DtwainarrayaddfloatFunc>,
    DTWAIN_ArrayAddFloatNFunc: Symbol<'a, DtwainarrayaddfloatnFunc>,
    DTWAIN_ArrayAddFloatStringFunc: Symbol<'a, DtwainarrayaddfloatstringFunc>,
    DTWAIN_ArrayAddFloatStringAFunc: Symbol<'a, DtwainarrayaddfloatstringaFunc>,
    DTWAIN_ArrayAddFloatStringNFunc: Symbol<'a, DtwainarrayaddfloatstringnFunc>,
    DTWAIN_ArrayAddFloatStringNAFunc: Symbol<'a, DtwainarrayaddfloatstringnaFunc>,
    DTWAIN_ArrayAddFloatStringNWFunc: Symbol<'a, DtwainarrayaddfloatstringnwFunc>,
    DTWAIN_ArrayAddFloatStringWFunc: Symbol<'a, DtwainarrayaddfloatstringwFunc>,
    DTWAIN_ArrayAddFrameFunc: Symbol<'a, DtwainarrayaddframeFunc>,
    DTWAIN_ArrayAddFrameNFunc: Symbol<'a, DtwainarrayaddframenFunc>,
    DTWAIN_ArrayAddLongFunc: Symbol<'a, DtwainarrayaddlongFunc>,
    DTWAIN_ArrayAddLong64Func: Symbol<'a, Dtwainarrayaddlong64Func>,
    DTWAIN_ArrayAddLong64NFunc: Symbol<'a, Dtwainarrayaddlong64nFunc>,
    DTWAIN_ArrayAddLongNFunc: Symbol<'a, DtwainarrayaddlongnFunc>,
    DTWAIN_ArrayAddNFunc: Symbol<'a, DtwainarrayaddnFunc>,
    DTWAIN_ArrayAddStringFunc: Symbol<'a, DtwainarrayaddstringFunc>,
    DTWAIN_ArrayAddStringAFunc: Symbol<'a, DtwainarrayaddstringaFunc>,
    DTWAIN_ArrayAddStringNFunc: Symbol<'a, DtwainarrayaddstringnFunc>,
    DTWAIN_ArrayAddStringNAFunc: Symbol<'a, DtwainarrayaddstringnaFunc>,
    DTWAIN_ArrayAddStringNWFunc: Symbol<'a, DtwainarrayaddstringnwFunc>,
    DTWAIN_ArrayAddStringWFunc: Symbol<'a, DtwainarrayaddstringwFunc>,
    DTWAIN_ArrayAddWideStringFunc: Symbol<'a, DtwainarrayaddwidestringFunc>,
    DTWAIN_ArrayAddWideStringNFunc: Symbol<'a, DtwainarrayaddwidestringnFunc>,
    DTWAIN_ArrayConvertFix32ToFloatFunc: Symbol<'a, Dtwainarrayconvertfix32tofloatFunc>,
    DTWAIN_ArrayConvertFloatToFix32Func: Symbol<'a, Dtwainarrayconvertfloattofix32Func>,
    DTWAIN_ArrayCopyFunc: Symbol<'a, DtwainarraycopyFunc>,
    DTWAIN_ArrayCreateFunc: Symbol<'a, DtwainarraycreateFunc>,
    DTWAIN_ArrayCreateCopyFunc: Symbol<'a, DtwainarraycreatecopyFunc>,
    DTWAIN_ArrayCreateFromCapFunc: Symbol<'a, DtwainarraycreatefromcapFunc>,
    DTWAIN_ArrayCreateFromLong64sFunc: Symbol<'a, Dtwainarraycreatefromlong64sFunc>,
    DTWAIN_ArrayCreateFromLongsFunc: Symbol<'a, DtwainarraycreatefromlongsFunc>,
    DTWAIN_ArrayCreateFromRealsFunc: Symbol<'a, DtwainarraycreatefromrealsFunc>,
    DTWAIN_ArrayDestroyFunc: Symbol<'a, DtwainarraydestroyFunc>,
    DTWAIN_ArrayDestroyFramesFunc: Symbol<'a, DtwainarraydestroyframesFunc>,
    DTWAIN_ArrayFindFunc: Symbol<'a, DtwainarrayfindFunc>,
    DTWAIN_ArrayFindANSIStringFunc: Symbol<'a, DtwainarrayfindansistringFunc>,
    DTWAIN_ArrayFindFloatFunc: Symbol<'a, DtwainarrayfindfloatFunc>,
    DTWAIN_ArrayFindFloatStringFunc: Symbol<'a, DtwainarrayfindfloatstringFunc>,
    DTWAIN_ArrayFindFloatStringAFunc: Symbol<'a, DtwainarrayfindfloatstringaFunc>,
    DTWAIN_ArrayFindFloatStringWFunc: Symbol<'a, DtwainarrayfindfloatstringwFunc>,
    DTWAIN_ArrayFindLongFunc: Symbol<'a, DtwainarrayfindlongFunc>,
    DTWAIN_ArrayFindLong64Func: Symbol<'a, Dtwainarrayfindlong64Func>,
    DTWAIN_ArrayFindStringFunc: Symbol<'a, DtwainarrayfindstringFunc>,
    DTWAIN_ArrayFindStringAFunc: Symbol<'a, DtwainarrayfindstringaFunc>,
    DTWAIN_ArrayFindStringWFunc: Symbol<'a, DtwainarrayfindstringwFunc>,
    DTWAIN_ArrayFindWideStringFunc: Symbol<'a, DtwainarrayfindwidestringFunc>,
    DTWAIN_ArrayFix32GetAtFunc: Symbol<'a, Dtwainarrayfix32getatFunc>,
    DTWAIN_ArrayFix32SetAtFunc: Symbol<'a, Dtwainarrayfix32setatFunc>,
    DTWAIN_ArrayFloatToANSIStringFunc: Symbol<'a, DtwainarrayfloattoansistringFunc>,
    DTWAIN_ArrayFloatToStringFunc: Symbol<'a, DtwainarrayfloattostringFunc>,
    DTWAIN_ArrayFloatToWideStringFunc: Symbol<'a, DtwainarrayfloattowidestringFunc>,
    DTWAIN_ArrayGetAtFunc: Symbol<'a, DtwainarraygetatFunc>,
    DTWAIN_ArrayGetAtANSIStringFunc: Symbol<'a, DtwainarraygetatansistringFunc>,
    DTWAIN_ArrayGetAtANSIStringPtrFunc: Symbol<'a, DtwainarraygetatansistringptrFunc>,
    DTWAIN_ArrayGetAtFloatFunc: Symbol<'a, DtwainarraygetatfloatFunc>,
    DTWAIN_ArrayGetAtFloatStringFunc: Symbol<'a, DtwainarraygetatfloatstringFunc>,
    DTWAIN_ArrayGetAtFloatStringAFunc: Symbol<'a, DtwainarraygetatfloatstringaFunc>,
    DTWAIN_ArrayGetAtFloatStringWFunc: Symbol<'a, DtwainarraygetatfloatstringwFunc>,
    DTWAIN_ArrayGetAtFrameFunc: Symbol<'a, DtwainarraygetatframeFunc>,
    DTWAIN_ArrayGetAtFrameExFunc: Symbol<'a, DtwainarraygetatframeexFunc>,
    DTWAIN_ArrayGetAtFrameStringFunc: Symbol<'a, DtwainarraygetatframestringFunc>,
    DTWAIN_ArrayGetAtFrameStringAFunc: Symbol<'a, DtwainarraygetatframestringaFunc>,
    DTWAIN_ArrayGetAtFrameStringWFunc: Symbol<'a, DtwainarraygetatframestringwFunc>,
    DTWAIN_ArrayGetAtLongFunc: Symbol<'a, DtwainarraygetatlongFunc>,
    DTWAIN_ArrayGetAtLong64Func: Symbol<'a, Dtwainarraygetatlong64Func>,
    DTWAIN_ArrayGetAtSourceFunc: Symbol<'a, DtwainarraygetatsourceFunc>,
    DTWAIN_ArrayGetAtStringFunc: Symbol<'a, DtwainarraygetatstringFunc>,
    DTWAIN_ArrayGetAtStringAFunc: Symbol<'a, DtwainarraygetatstringaFunc>,
    DTWAIN_ArrayGetAtStringPtrFunc: Symbol<'a, DtwainarraygetatstringptrFunc>,
    DTWAIN_ArrayGetAtStringWFunc: Symbol<'a, DtwainarraygetatstringwFunc>,
    DTWAIN_ArrayGetAtWideStringFunc: Symbol<'a, DtwainarraygetatwidestringFunc>,
    DTWAIN_ArrayGetAtWideStringPtrFunc: Symbol<'a, DtwainarraygetatwidestringptrFunc>,
    DTWAIN_ArrayGetBufferFunc: Symbol<'a, DtwainarraygetbufferFunc>,
    DTWAIN_ArrayGetCapValuesFunc: Symbol<'a, DtwainarraygetcapvaluesFunc>,
    DTWAIN_ArrayGetCapValuesExFunc: Symbol<'a, DtwainarraygetcapvaluesexFunc>,
    DTWAIN_ArrayGetCapValuesEx2Func: Symbol<'a, Dtwainarraygetcapvaluesex2Func>,
    DTWAIN_ArrayGetCountFunc: Symbol<'a, DtwainarraygetcountFunc>,
    DTWAIN_ArrayGetMaxStringLengthFunc: Symbol<'a, DtwainarraygetmaxstringlengthFunc>,
    DTWAIN_ArrayGetSourceAtFunc: Symbol<'a, DtwainarraygetsourceatFunc>,
    DTWAIN_ArrayGetStringLengthFunc: Symbol<'a, DtwainarraygetstringlengthFunc>,
    DTWAIN_ArrayGetTypeFunc: Symbol<'a, DtwainarraygettypeFunc>,
    DTWAIN_ArrayInitFunc: Symbol<'a, DtwainarrayinitFunc>,
    DTWAIN_ArrayInsertAtFunc: Symbol<'a, DtwainarrayinsertatFunc>,
    DTWAIN_ArrayInsertAtANSIStringFunc: Symbol<'a, DtwainarrayinsertatansistringFunc>,
    DTWAIN_ArrayInsertAtANSIStringNFunc: Symbol<'a, DtwainarrayinsertatansistringnFunc>,
    DTWAIN_ArrayInsertAtFloatFunc: Symbol<'a, DtwainarrayinsertatfloatFunc>,
    DTWAIN_ArrayInsertAtFloatNFunc: Symbol<'a, DtwainarrayinsertatfloatnFunc>,
    DTWAIN_ArrayInsertAtFloatStringFunc: Symbol<'a, DtwainarrayinsertatfloatstringFunc>,
    DTWAIN_ArrayInsertAtFloatStringAFunc: Symbol<'a, DtwainarrayinsertatfloatstringaFunc>,
    DTWAIN_ArrayInsertAtFloatStringNFunc: Symbol<'a, DtwainarrayinsertatfloatstringnFunc>,
    DTWAIN_ArrayInsertAtFloatStringNAFunc: Symbol<'a, DtwainarrayinsertatfloatstringnaFunc>,
    DTWAIN_ArrayInsertAtFloatStringNWFunc: Symbol<'a, DtwainarrayinsertatfloatstringnwFunc>,
    DTWAIN_ArrayInsertAtFloatStringWFunc: Symbol<'a, DtwainarrayinsertatfloatstringwFunc>,
    DTWAIN_ArrayInsertAtFrameFunc: Symbol<'a, DtwainarrayinsertatframeFunc>,
    DTWAIN_ArrayInsertAtFrameNFunc: Symbol<'a, DtwainarrayinsertatframenFunc>,
    DTWAIN_ArrayInsertAtLongFunc: Symbol<'a, DtwainarrayinsertatlongFunc>,
    DTWAIN_ArrayInsertAtLong64Func: Symbol<'a, Dtwainarrayinsertatlong64Func>,
    DTWAIN_ArrayInsertAtLong64NFunc: Symbol<'a, Dtwainarrayinsertatlong64nFunc>,
    DTWAIN_ArrayInsertAtLongNFunc: Symbol<'a, DtwainarrayinsertatlongnFunc>,
    DTWAIN_ArrayInsertAtNFunc: Symbol<'a, DtwainarrayinsertatnFunc>,
    DTWAIN_ArrayInsertAtStringFunc: Symbol<'a, DtwainarrayinsertatstringFunc>,
    DTWAIN_ArrayInsertAtStringAFunc: Symbol<'a, DtwainarrayinsertatstringaFunc>,
    DTWAIN_ArrayInsertAtStringNFunc: Symbol<'a, DtwainarrayinsertatstringnFunc>,
    DTWAIN_ArrayInsertAtStringNAFunc: Symbol<'a, DtwainarrayinsertatstringnaFunc>,
    DTWAIN_ArrayInsertAtStringNWFunc: Symbol<'a, DtwainarrayinsertatstringnwFunc>,
    DTWAIN_ArrayInsertAtStringWFunc: Symbol<'a, DtwainarrayinsertatstringwFunc>,
    DTWAIN_ArrayInsertAtWideStringFunc: Symbol<'a, DtwainarrayinsertatwidestringFunc>,
    DTWAIN_ArrayInsertAtWideStringNFunc: Symbol<'a, DtwainarrayinsertatwidestringnFunc>,
    DTWAIN_ArrayRemoveAllFunc: Symbol<'a, DtwainarrayremoveallFunc>,
    DTWAIN_ArrayRemoveAtFunc: Symbol<'a, DtwainarrayremoveatFunc>,
    DTWAIN_ArrayRemoveAtNFunc: Symbol<'a, DtwainarrayremoveatnFunc>,
    DTWAIN_ArrayResizeFunc: Symbol<'a, DtwainarrayresizeFunc>,
    DTWAIN_ArraySetAtFunc: Symbol<'a, DtwainarraysetatFunc>,
    DTWAIN_ArraySetAtANSIStringFunc: Symbol<'a, DtwainarraysetatansistringFunc>,
    DTWAIN_ArraySetAtFloatFunc: Symbol<'a, DtwainarraysetatfloatFunc>,
    DTWAIN_ArraySetAtFloatStringFunc: Symbol<'a, DtwainarraysetatfloatstringFunc>,
    DTWAIN_ArraySetAtFloatStringAFunc: Symbol<'a, DtwainarraysetatfloatstringaFunc>,
    DTWAIN_ArraySetAtFloatStringWFunc: Symbol<'a, DtwainarraysetatfloatstringwFunc>,
    DTWAIN_ArraySetAtFrameFunc: Symbol<'a, DtwainarraysetatframeFunc>,
    DTWAIN_ArraySetAtFrameExFunc: Symbol<'a, DtwainarraysetatframeexFunc>,
    DTWAIN_ArraySetAtFrameStringFunc: Symbol<'a, DtwainarraysetatframestringFunc>,
    DTWAIN_ArraySetAtFrameStringAFunc: Symbol<'a, DtwainarraysetatframestringaFunc>,
    DTWAIN_ArraySetAtFrameStringWFunc: Symbol<'a, DtwainarraysetatframestringwFunc>,
    DTWAIN_ArraySetAtLongFunc: Symbol<'a, DtwainarraysetatlongFunc>,
    DTWAIN_ArraySetAtLong64Func: Symbol<'a, Dtwainarraysetatlong64Func>,
    DTWAIN_ArraySetAtStringFunc: Symbol<'a, DtwainarraysetatstringFunc>,
    DTWAIN_ArraySetAtStringAFunc: Symbol<'a, DtwainarraysetatstringaFunc>,
    DTWAIN_ArraySetAtStringWFunc: Symbol<'a, DtwainarraysetatstringwFunc>,
    DTWAIN_ArraySetAtWideStringFunc: Symbol<'a, DtwainarraysetatwidestringFunc>,
    DTWAIN_ArrayStringToFloatFunc: Symbol<'a, DtwainarraystringtofloatFunc>,
    DTWAIN_ArrayWideStringToFloatFunc: Symbol<'a, DtwainarraywidestringtofloatFunc>,
    DTWAIN_CallCallbackFunc: Symbol<'a, DtwaincallcallbackFunc>,
    DTWAIN_CallCallback64Func: Symbol<'a, Dtwaincallcallback64Func>,
    DTWAIN_CallDSMProcFunc: Symbol<'a, DtwaincalldsmprocFunc>,
    DTWAIN_CheckHandlesFunc: Symbol<'a, DtwaincheckhandlesFunc>,
    DTWAIN_ClearBuffersFunc: Symbol<'a, DtwainclearbuffersFunc>,
    DTWAIN_ClearErrorBufferFunc: Symbol<'a, DtwainclearerrorbufferFunc>,
    DTWAIN_ClearPDFTextFunc: Symbol<'a, DtwainclearpdftextFunc>,
    DTWAIN_ClearPageFunc: Symbol<'a, DtwainclearpageFunc>,
    DTWAIN_CloseSourceFunc: Symbol<'a, DtwainclosesourceFunc>,
    DTWAIN_CloseSourceUIFunc: Symbol<'a, DtwainclosesourceuiFunc>,
    DTWAIN_ConvertDIBToBitmapFunc: Symbol<'a, DtwainconvertdibtobitmapFunc>,
    DTWAIN_ConvertDIBToFullBitmapFunc: Symbol<'a, DtwainconvertdibtofullbitmapFunc>,
    DTWAIN_ConvertToAPIStringFunc: Symbol<'a, DtwainconverttoapistringFunc>,
    DTWAIN_ConvertToAPIStringAFunc: Symbol<'a, DtwainconverttoapistringaFunc>,
    DTWAIN_ConvertToAPIStringExFunc: Symbol<'a, DtwainconverttoapistringexFunc>,
    DTWAIN_ConvertToAPIStringExAFunc: Symbol<'a, DtwainconverttoapistringexaFunc>,
    DTWAIN_ConvertToAPIStringExWFunc: Symbol<'a, DtwainconverttoapistringexwFunc>,
    DTWAIN_ConvertToAPIStringWFunc: Symbol<'a, DtwainconverttoapistringwFunc>,
    DTWAIN_CreateAcquisitionArrayFunc: Symbol<'a, DtwaincreateacquisitionarrayFunc>,
    DTWAIN_CreatePDFTextElementFunc: Symbol<'a, DtwaincreatepdftextelementFunc>,
    DTWAIN_DeleteDIBFunc: Symbol<'a, DtwaindeletedibFunc>,
    DTWAIN_DestroyAcquisitionArrayFunc: Symbol<'a, DtwaindestroyacquisitionarrayFunc>,
    DTWAIN_DestroyPDFTextElementFunc: Symbol<'a, DtwaindestroypdftextelementFunc>,
    DTWAIN_DisableAppWindowFunc: Symbol<'a, DtwaindisableappwindowFunc>,
    DTWAIN_EnableAutoBorderDetectFunc: Symbol<'a, DtwainenableautoborderdetectFunc>,
    DTWAIN_EnableAutoBrightFunc: Symbol<'a, DtwainenableautobrightFunc>,
    DTWAIN_EnableAutoDeskewFunc: Symbol<'a, DtwainenableautodeskewFunc>,
    DTWAIN_EnableAutoFeedFunc: Symbol<'a, DtwainenableautofeedFunc>,
    DTWAIN_EnableAutoRotateFunc: Symbol<'a, DtwainenableautorotateFunc>,
    DTWAIN_EnableAutoScanFunc: Symbol<'a, DtwainenableautoscanFunc>,
    DTWAIN_EnableAutomaticSenseMediumFunc: Symbol<'a, DtwainenableautomaticsensemediumFunc>,
    DTWAIN_EnableDuplexFunc: Symbol<'a, DtwainenableduplexFunc>,
    DTWAIN_EnableFeederFunc: Symbol<'a, DtwainenablefeederFunc>,
    DTWAIN_EnableIndicatorFunc: Symbol<'a, DtwainenableindicatorFunc>,
    DTWAIN_EnableJobFileHandlingFunc: Symbol<'a, DtwainenablejobfilehandlingFunc>,
    DTWAIN_EnableLampFunc: Symbol<'a, DtwainenablelampFunc>,
    DTWAIN_EnableMsgNotifyFunc: Symbol<'a, DtwainenablemsgnotifyFunc>,
    DTWAIN_EnablePatchDetectFunc: Symbol<'a, DtwainenablepatchdetectFunc>,
    DTWAIN_EnablePeekMessageLoopFunc: Symbol<'a, DtwainenablepeekmessageloopFunc>,
    DTWAIN_EnablePrinterFunc: Symbol<'a, DtwainenableprinterFunc>,
    DTWAIN_EnableThumbnailFunc: Symbol<'a, DtwainenablethumbnailFunc>,
    DTWAIN_EnableTripletsNotifyFunc: Symbol<'a, DtwainenabletripletsnotifyFunc>,
    DTWAIN_EndThreadFunc: Symbol<'a, DtwainendthreadFunc>,
    DTWAIN_EndTwainSessionFunc: Symbol<'a, DtwainendtwainsessionFunc>,
    DTWAIN_EnumAlarmVolumesFunc: Symbol<'a, DtwainenumalarmvolumesFunc>,
    DTWAIN_EnumAlarmVolumesExFunc: Symbol<'a, DtwainenumalarmvolumesexFunc>,
    DTWAIN_EnumAlarmsFunc: Symbol<'a, DtwainenumalarmsFunc>,
    DTWAIN_EnumAlarmsExFunc: Symbol<'a, DtwainenumalarmsexFunc>,
    DTWAIN_EnumAudioXferMechsFunc: Symbol<'a, DtwainenumaudioxfermechsFunc>,
    DTWAIN_EnumAudioXferMechsExFunc: Symbol<'a, DtwainenumaudioxfermechsexFunc>,
    DTWAIN_EnumAutoFeedValuesFunc: Symbol<'a, DtwainenumautofeedvaluesFunc>,
    DTWAIN_EnumAutoFeedValuesExFunc: Symbol<'a, DtwainenumautofeedvaluesexFunc>,
    DTWAIN_EnumAutomaticCapturesFunc: Symbol<'a, DtwainenumautomaticcapturesFunc>,
    DTWAIN_EnumAutomaticCapturesExFunc: Symbol<'a, DtwainenumautomaticcapturesexFunc>,
    DTWAIN_EnumAutomaticSenseMediumFunc: Symbol<'a, DtwainenumautomaticsensemediumFunc>,
    DTWAIN_EnumAutomaticSenseMediumExFunc: Symbol<'a, DtwainenumautomaticsensemediumexFunc>,
    DTWAIN_EnumBitDepthsFunc: Symbol<'a, DtwainenumbitdepthsFunc>,
    DTWAIN_EnumBitDepthsExFunc: Symbol<'a, DtwainenumbitdepthsexFunc>,
    DTWAIN_EnumBitDepthsEx2Func: Symbol<'a, Dtwainenumbitdepthsex2Func>,
    DTWAIN_EnumBottomCamerasFunc: Symbol<'a, DtwainenumbottomcamerasFunc>,
    DTWAIN_EnumBottomCamerasExFunc: Symbol<'a, DtwainenumbottomcamerasexFunc>,
    DTWAIN_EnumBrightnessValuesFunc: Symbol<'a, DtwainenumbrightnessvaluesFunc>,
    DTWAIN_EnumBrightnessValuesExFunc: Symbol<'a, DtwainenumbrightnessvaluesexFunc>,
    DTWAIN_EnumCamerasFunc: Symbol<'a, DtwainenumcamerasFunc>,
    DTWAIN_EnumCamerasExFunc: Symbol<'a, DtwainenumcamerasexFunc>,
    DTWAIN_EnumCamerasEx2Func: Symbol<'a, Dtwainenumcamerasex2Func>,
    DTWAIN_EnumCamerasEx3Func: Symbol<'a, Dtwainenumcamerasex3Func>,
    DTWAIN_EnumCompressionTypesFunc: Symbol<'a, DtwainenumcompressiontypesFunc>,
    DTWAIN_EnumCompressionTypesExFunc: Symbol<'a, DtwainenumcompressiontypesexFunc>,
    DTWAIN_EnumCompressionTypesEx2Func: Symbol<'a, Dtwainenumcompressiontypesex2Func>,
    DTWAIN_EnumContrastValuesFunc: Symbol<'a, DtwainenumcontrastvaluesFunc>,
    DTWAIN_EnumContrastValuesExFunc: Symbol<'a, DtwainenumcontrastvaluesexFunc>,
    DTWAIN_EnumCustomCapsFunc: Symbol<'a, DtwainenumcustomcapsFunc>,
    DTWAIN_EnumCustomCapsEx2Func: Symbol<'a, Dtwainenumcustomcapsex2Func>,
    DTWAIN_EnumDoubleFeedDetectLengthsFunc: Symbol<'a, DtwainenumdoublefeeddetectlengthsFunc>,
    DTWAIN_EnumDoubleFeedDetectLengthsExFunc: Symbol<'a, DtwainenumdoublefeeddetectlengthsexFunc>,
    DTWAIN_EnumDoubleFeedDetectValuesFunc: Symbol<'a, DtwainenumdoublefeeddetectvaluesFunc>,
    DTWAIN_EnumDoubleFeedDetectValuesExFunc: Symbol<'a, DtwainenumdoublefeeddetectvaluesexFunc>,
    DTWAIN_EnumExtImageInfoTypesFunc: Symbol<'a, DtwainenumextimageinfotypesFunc>,
    DTWAIN_EnumExtImageInfoTypesExFunc: Symbol<'a, DtwainenumextimageinfotypesexFunc>,
    DTWAIN_EnumExtendedCapsFunc: Symbol<'a, DtwainenumextendedcapsFunc>,
    DTWAIN_EnumExtendedCapsExFunc: Symbol<'a, DtwainenumextendedcapsexFunc>,
    DTWAIN_EnumExtendedCapsEx2Func: Symbol<'a, Dtwainenumextendedcapsex2Func>,
    DTWAIN_EnumFileTypeBitsPerPixelFunc: Symbol<'a, DtwainenumfiletypebitsperpixelFunc>,
    DTWAIN_EnumFileXferFormatsFunc: Symbol<'a, DtwainenumfilexferformatsFunc>,
    DTWAIN_EnumFileXferFormatsExFunc: Symbol<'a, DtwainenumfilexferformatsexFunc>,
    DTWAIN_EnumHalftonesFunc: Symbol<'a, DtwainenumhalftonesFunc>,
    DTWAIN_EnumHalftonesExFunc: Symbol<'a, DtwainenumhalftonesexFunc>,
    DTWAIN_EnumHighlightValuesFunc: Symbol<'a, DtwainenumhighlightvaluesFunc>,
    DTWAIN_EnumHighlightValuesExFunc: Symbol<'a, DtwainenumhighlightvaluesexFunc>,
    DTWAIN_EnumJobControlsFunc: Symbol<'a, DtwainenumjobcontrolsFunc>,
    DTWAIN_EnumJobControlsExFunc: Symbol<'a, DtwainenumjobcontrolsexFunc>,
    DTWAIN_EnumLightPathsFunc: Symbol<'a, DtwainenumlightpathsFunc>,
    DTWAIN_EnumLightPathsExFunc: Symbol<'a, DtwainenumlightpathsexFunc>,
    DTWAIN_EnumLightSourcesFunc: Symbol<'a, DtwainenumlightsourcesFunc>,
    DTWAIN_EnumLightSourcesExFunc: Symbol<'a, DtwainenumlightsourcesexFunc>,
    DTWAIN_EnumMaxBuffersFunc: Symbol<'a, DtwainenummaxbuffersFunc>,
    DTWAIN_EnumMaxBuffersExFunc: Symbol<'a, DtwainenummaxbuffersexFunc>,
    DTWAIN_EnumNoiseFiltersFunc: Symbol<'a, DtwainenumnoisefiltersFunc>,
    DTWAIN_EnumNoiseFiltersExFunc: Symbol<'a, DtwainenumnoisefiltersexFunc>,
    DTWAIN_EnumOCRInterfacesFunc: Symbol<'a, DtwainenumocrinterfacesFunc>,
    DTWAIN_EnumOCRSupportedCapsFunc: Symbol<'a, DtwainenumocrsupportedcapsFunc>,
    DTWAIN_EnumOrientationsFunc: Symbol<'a, DtwainenumorientationsFunc>,
    DTWAIN_EnumOrientationsExFunc: Symbol<'a, DtwainenumorientationsexFunc>,
    DTWAIN_EnumOverscanValuesFunc: Symbol<'a, DtwainenumoverscanvaluesFunc>,
    DTWAIN_EnumOverscanValuesExFunc: Symbol<'a, DtwainenumoverscanvaluesexFunc>,
    DTWAIN_EnumPaperSizesFunc: Symbol<'a, DtwainenumpapersizesFunc>,
    DTWAIN_EnumPaperSizesExFunc: Symbol<'a, DtwainenumpapersizesexFunc>,
    DTWAIN_EnumPatchCodesFunc: Symbol<'a, DtwainenumpatchcodesFunc>,
    DTWAIN_EnumPatchCodesExFunc: Symbol<'a, DtwainenumpatchcodesexFunc>,
    DTWAIN_EnumPatchMaxPrioritiesFunc: Symbol<'a, DtwainenumpatchmaxprioritiesFunc>,
    DTWAIN_EnumPatchMaxPrioritiesExFunc: Symbol<'a, DtwainenumpatchmaxprioritiesexFunc>,
    DTWAIN_EnumPatchMaxRetriesFunc: Symbol<'a, DtwainenumpatchmaxretriesFunc>,
    DTWAIN_EnumPatchMaxRetriesExFunc: Symbol<'a, DtwainenumpatchmaxretriesexFunc>,
    DTWAIN_EnumPatchPrioritiesFunc: Symbol<'a, DtwainenumpatchprioritiesFunc>,
    DTWAIN_EnumPatchPrioritiesExFunc: Symbol<'a, DtwainenumpatchprioritiesexFunc>,
    DTWAIN_EnumPatchSearchModesFunc: Symbol<'a, DtwainenumpatchsearchmodesFunc>,
    DTWAIN_EnumPatchSearchModesExFunc: Symbol<'a, DtwainenumpatchsearchmodesexFunc>,
    DTWAIN_EnumPatchTimeOutValuesFunc: Symbol<'a, DtwainenumpatchtimeoutvaluesFunc>,
    DTWAIN_EnumPatchTimeOutValuesExFunc: Symbol<'a, DtwainenumpatchtimeoutvaluesexFunc>,
    DTWAIN_EnumPixelTypesFunc: Symbol<'a, DtwainenumpixeltypesFunc>,
    DTWAIN_EnumPixelTypesExFunc: Symbol<'a, DtwainenumpixeltypesexFunc>,
    DTWAIN_EnumPrinterStringModesFunc: Symbol<'a, DtwainenumprinterstringmodesFunc>,
    DTWAIN_EnumPrinterStringModesExFunc: Symbol<'a, DtwainenumprinterstringmodesexFunc>,
    DTWAIN_EnumResolutionValuesFunc: Symbol<'a, DtwainenumresolutionvaluesFunc>,
    DTWAIN_EnumResolutionValuesExFunc: Symbol<'a, DtwainenumresolutionvaluesexFunc>,
    DTWAIN_EnumShadowValuesFunc: Symbol<'a, DtwainenumshadowvaluesFunc>,
    DTWAIN_EnumShadowValuesExFunc: Symbol<'a, DtwainenumshadowvaluesexFunc>,
    DTWAIN_EnumSourceUnitsFunc: Symbol<'a, DtwainenumsourceunitsFunc>,
    DTWAIN_EnumSourceUnitsExFunc: Symbol<'a, DtwainenumsourceunitsexFunc>,
    DTWAIN_EnumSourceValuesFunc: Symbol<'a, DtwainenumsourcevaluesFunc>,
    DTWAIN_EnumSourceValuesAFunc: Symbol<'a, DtwainenumsourcevaluesaFunc>,
    DTWAIN_EnumSourceValuesWFunc: Symbol<'a, DtwainenumsourcevalueswFunc>,
    DTWAIN_EnumSourcesFunc: Symbol<'a, DtwainenumsourcesFunc>,
    DTWAIN_EnumSourcesExFunc: Symbol<'a, DtwainenumsourcesexFunc>,
    DTWAIN_EnumSupportedCapsFunc: Symbol<'a, DtwainenumsupportedcapsFunc>,
    DTWAIN_EnumSupportedCapsExFunc: Symbol<'a, DtwainenumsupportedcapsexFunc>,
    DTWAIN_EnumSupportedCapsEx2Func: Symbol<'a, Dtwainenumsupportedcapsex2Func>,
    DTWAIN_EnumSupportedExtImageInfoFunc: Symbol<'a, DtwainenumsupportedextimageinfoFunc>,
    DTWAIN_EnumSupportedExtImageInfoExFunc: Symbol<'a, DtwainenumsupportedextimageinfoexFunc>,
    DTWAIN_EnumSupportedFileTypesFunc: Symbol<'a, DtwainenumsupportedfiletypesFunc>,
    DTWAIN_EnumSupportedMultiPageFileTypesFunc: Symbol<'a, DtwainenumsupportedmultipagefiletypesFunc>,
    DTWAIN_EnumSupportedSinglePageFileTypesFunc: Symbol<'a, DtwainenumsupportedsinglepagefiletypesFunc>,
    DTWAIN_EnumThresholdValuesFunc: Symbol<'a, DtwainenumthresholdvaluesFunc>,
    DTWAIN_EnumThresholdValuesExFunc: Symbol<'a, DtwainenumthresholdvaluesexFunc>,
    DTWAIN_EnumTopCamerasFunc: Symbol<'a, DtwainenumtopcamerasFunc>,
    DTWAIN_EnumTopCamerasExFunc: Symbol<'a, DtwainenumtopcamerasexFunc>,
    DTWAIN_EnumTwainPrintersFunc: Symbol<'a, DtwainenumtwainprintersFunc>,
    DTWAIN_EnumTwainPrintersArrayFunc: Symbol<'a, DtwainenumtwainprintersarrayFunc>,
    DTWAIN_EnumTwainPrintersArrayExFunc: Symbol<'a, DtwainenumtwainprintersarrayexFunc>,
    DTWAIN_EnumTwainPrintersExFunc: Symbol<'a, DtwainenumtwainprintersexFunc>,
    DTWAIN_EnumXResolutionValuesFunc: Symbol<'a, DtwainenumxresolutionvaluesFunc>,
    DTWAIN_EnumXResolutionValuesExFunc: Symbol<'a, DtwainenumxresolutionvaluesexFunc>,
    DTWAIN_EnumYResolutionValuesFunc: Symbol<'a, DtwainenumyresolutionvaluesFunc>,
    DTWAIN_EnumYResolutionValuesExFunc: Symbol<'a, DtwainenumyresolutionvaluesexFunc>,
    DTWAIN_ExecuteOCRFunc: Symbol<'a, DtwainexecuteocrFunc>,
    DTWAIN_ExecuteOCRAFunc: Symbol<'a, DtwainexecuteocraFunc>,
    DTWAIN_ExecuteOCRWFunc: Symbol<'a, DtwainexecuteocrwFunc>,
    DTWAIN_FeedPageFunc: Symbol<'a, DtwainfeedpageFunc>,
    DTWAIN_FlipBitmapFunc: Symbol<'a, DtwainflipbitmapFunc>,
    DTWAIN_FlushAcquiredPagesFunc: Symbol<'a, DtwainflushacquiredpagesFunc>,
    DTWAIN_ForceAcquireBitDepthFunc: Symbol<'a, DtwainforceacquirebitdepthFunc>,
    DTWAIN_ForceScanOnNoUIFunc: Symbol<'a, DtwainforcescanonnouiFunc>,
    DTWAIN_FrameCreateFunc: Symbol<'a, DtwainframecreateFunc>,
    DTWAIN_FrameCreateStringFunc: Symbol<'a, DtwainframecreatestringFunc>,
    DTWAIN_FrameCreateStringAFunc: Symbol<'a, DtwainframecreatestringaFunc>,
    DTWAIN_FrameCreateStringWFunc: Symbol<'a, DtwainframecreatestringwFunc>,
    DTWAIN_FrameDestroyFunc: Symbol<'a, DtwainframedestroyFunc>,
    DTWAIN_FrameGetAllFunc: Symbol<'a, DtwainframegetallFunc>,
    DTWAIN_FrameGetAllStringFunc: Symbol<'a, DtwainframegetallstringFunc>,
    DTWAIN_FrameGetAllStringAFunc: Symbol<'a, DtwainframegetallstringaFunc>,
    DTWAIN_FrameGetAllStringWFunc: Symbol<'a, DtwainframegetallstringwFunc>,
    DTWAIN_FrameGetValueFunc: Symbol<'a, DtwainframegetvalueFunc>,
    DTWAIN_FrameGetValueStringFunc: Symbol<'a, DtwainframegetvaluestringFunc>,
    DTWAIN_FrameGetValueStringAFunc: Symbol<'a, DtwainframegetvaluestringaFunc>,
    DTWAIN_FrameGetValueStringWFunc: Symbol<'a, DtwainframegetvaluestringwFunc>,
    DTWAIN_FrameIsValidFunc: Symbol<'a, DtwainframeisvalidFunc>,
    DTWAIN_FrameSetAllFunc: Symbol<'a, DtwainframesetallFunc>,
    DTWAIN_FrameSetAllStringFunc: Symbol<'a, DtwainframesetallstringFunc>,
    DTWAIN_FrameSetAllStringAFunc: Symbol<'a, DtwainframesetallstringaFunc>,
    DTWAIN_FrameSetAllStringWFunc: Symbol<'a, DtwainframesetallstringwFunc>,
    DTWAIN_FrameSetValueFunc: Symbol<'a, DtwainframesetvalueFunc>,
    DTWAIN_FrameSetValueStringFunc: Symbol<'a, DtwainframesetvaluestringFunc>,
    DTWAIN_FrameSetValueStringAFunc: Symbol<'a, DtwainframesetvaluestringaFunc>,
    DTWAIN_FrameSetValueStringWFunc: Symbol<'a, DtwainframesetvaluestringwFunc>,
    DTWAIN_FreeExtImageInfoFunc: Symbol<'a, DtwainfreeextimageinfoFunc>,
    DTWAIN_FreeMemoryFunc: Symbol<'a, DtwainfreememoryFunc>,
    DTWAIN_FreeMemoryExFunc: Symbol<'a, DtwainfreememoryexFunc>,
    DTWAIN_GetAPIHandleStatusFunc: Symbol<'a, DtwaingetapihandlestatusFunc>,
    DTWAIN_GetAcquireAreaFunc: Symbol<'a, DtwaingetacquireareaFunc>,
    DTWAIN_GetAcquireArea2Func: Symbol<'a, Dtwaingetacquirearea2Func>,
    DTWAIN_GetAcquireArea2StringFunc: Symbol<'a, Dtwaingetacquirearea2stringFunc>,
    DTWAIN_GetAcquireArea2StringAFunc: Symbol<'a, Dtwaingetacquirearea2stringaFunc>,
    DTWAIN_GetAcquireArea2StringWFunc: Symbol<'a, Dtwaingetacquirearea2stringwFunc>,
    DTWAIN_GetAcquireAreaExFunc: Symbol<'a, DtwaingetacquireareaexFunc>,
    DTWAIN_GetAcquireMetricsFunc: Symbol<'a, DtwaingetacquiremetricsFunc>,
    DTWAIN_GetAcquireStripBufferFunc: Symbol<'a, DtwaingetacquirestripbufferFunc>,
    DTWAIN_GetAcquireStripDataFunc: Symbol<'a, DtwaingetacquirestripdataFunc>,
    DTWAIN_GetAcquireStripSizesFunc: Symbol<'a, DtwaingetacquirestripsizesFunc>,
    DTWAIN_GetAcquiredImageFunc: Symbol<'a, DtwaingetacquiredimageFunc>,
    DTWAIN_GetAcquiredImageArrayFunc: Symbol<'a, DtwaingetacquiredimagearrayFunc>,
    DTWAIN_GetActiveDSMPathFunc: Symbol<'a, DtwaingetactivedsmpathFunc>,
    DTWAIN_GetActiveDSMPathAFunc: Symbol<'a, DtwaingetactivedsmpathaFunc>,
    DTWAIN_GetActiveDSMPathWFunc: Symbol<'a, DtwaingetactivedsmpathwFunc>,
    DTWAIN_GetActiveDSMVersionInfoFunc: Symbol<'a, DtwaingetactivedsmversioninfoFunc>,
    DTWAIN_GetActiveDSMVersionInfoAFunc: Symbol<'a, DtwaingetactivedsmversioninfoaFunc>,
    DTWAIN_GetActiveDSMVersionInfoWFunc: Symbol<'a, DtwaingetactivedsmversioninfowFunc>,
    DTWAIN_GetAlarmVolumeFunc: Symbol<'a, DtwaingetalarmvolumeFunc>,
    DTWAIN_GetAllSourceDibsFunc: Symbol<'a, DtwaingetallsourcedibsFunc>,
    DTWAIN_GetAppInfoFunc: Symbol<'a, DtwaingetappinfoFunc>,
    DTWAIN_GetAppInfoAFunc: Symbol<'a, DtwaingetappinfoaFunc>,
    DTWAIN_GetAppInfoWFunc: Symbol<'a, DtwaingetappinfowFunc>,
    DTWAIN_GetAuthorFunc: Symbol<'a, DtwaingetauthorFunc>,
    DTWAIN_GetAuthorAFunc: Symbol<'a, DtwaingetauthoraFunc>,
    DTWAIN_GetAuthorWFunc: Symbol<'a, DtwaingetauthorwFunc>,
    DTWAIN_GetBatteryMinutesFunc: Symbol<'a, DtwaingetbatteryminutesFunc>,
    DTWAIN_GetBatteryPercentFunc: Symbol<'a, DtwaingetbatterypercentFunc>,
    DTWAIN_GetBitDepthFunc: Symbol<'a, DtwaingetbitdepthFunc>,
    DTWAIN_GetBlankPageAutoDetectionFunc: Symbol<'a, DtwaingetblankpageautodetectionFunc>,
    DTWAIN_GetBrightnessFunc: Symbol<'a, DtwaingetbrightnessFunc>,
    DTWAIN_GetBrightnessStringFunc: Symbol<'a, DtwaingetbrightnessstringFunc>,
    DTWAIN_GetBrightnessStringAFunc: Symbol<'a, DtwaingetbrightnessstringaFunc>,
    DTWAIN_GetBrightnessStringWFunc: Symbol<'a, DtwaingetbrightnessstringwFunc>,
    DTWAIN_GetBufferedTransferInfoFunc: Symbol<'a, DtwaingetbufferedtransferinfoFunc>,
    DTWAIN_GetCallbackFunc: Symbol<'a, DtwaingetcallbackFunc>,
    DTWAIN_GetCallback64Func: Symbol<'a, Dtwaingetcallback64Func>,
    DTWAIN_GetCapArrayTypeFunc: Symbol<'a, DtwaingetcaparraytypeFunc>,
    DTWAIN_GetCapContainerFunc: Symbol<'a, DtwaingetcapcontainerFunc>,
    DTWAIN_GetCapContainerExFunc: Symbol<'a, DtwaingetcapcontainerexFunc>,
    DTWAIN_GetCapContainerEx2Func: Symbol<'a, Dtwaingetcapcontainerex2Func>,
    DTWAIN_GetCapDataTypeFunc: Symbol<'a, DtwaingetcapdatatypeFunc>,
    DTWAIN_GetCapFromNameFunc: Symbol<'a, DtwaingetcapfromnameFunc>,
    DTWAIN_GetCapFromNameAFunc: Symbol<'a, DtwaingetcapfromnameaFunc>,
    DTWAIN_GetCapFromNameWFunc: Symbol<'a, DtwaingetcapfromnamewFunc>,
    DTWAIN_GetCapOperationsFunc: Symbol<'a, DtwaingetcapoperationsFunc>,
    DTWAIN_GetCapValuesFunc: Symbol<'a, DtwaingetcapvaluesFunc>,
    DTWAIN_GetCapValuesExFunc: Symbol<'a, DtwaingetcapvaluesexFunc>,
    DTWAIN_GetCapValuesEx2Func: Symbol<'a, Dtwaingetcapvaluesex2Func>,
    DTWAIN_GetCaptionFunc: Symbol<'a, DtwaingetcaptionFunc>,
    DTWAIN_GetCaptionAFunc: Symbol<'a, DtwaingetcaptionaFunc>,
    DTWAIN_GetCaptionWFunc: Symbol<'a, DtwaingetcaptionwFunc>,
    DTWAIN_GetCompressionSizeFunc: Symbol<'a, DtwaingetcompressionsizeFunc>,
    DTWAIN_GetCompressionTypeFunc: Symbol<'a, DtwaingetcompressiontypeFunc>,
    DTWAIN_GetConditionCodeStringFunc: Symbol<'a, DtwaingetconditioncodestringFunc>,
    DTWAIN_GetConditionCodeStringAFunc: Symbol<'a, DtwaingetconditioncodestringaFunc>,
    DTWAIN_GetConditionCodeStringWFunc: Symbol<'a, DtwaingetconditioncodestringwFunc>,
    DTWAIN_GetContrastFunc: Symbol<'a, DtwaingetcontrastFunc>,
    DTWAIN_GetContrastStringFunc: Symbol<'a, DtwaingetcontraststringFunc>,
    DTWAIN_GetContrastStringAFunc: Symbol<'a, DtwaingetcontraststringaFunc>,
    DTWAIN_GetContrastStringWFunc: Symbol<'a, DtwaingetcontraststringwFunc>,
    DTWAIN_GetCountryFunc: Symbol<'a, DtwaingetcountryFunc>,
    DTWAIN_GetCurrentAcquiredImageFunc: Symbol<'a, DtwaingetcurrentacquiredimageFunc>,
    DTWAIN_GetCurrentFileNameFunc: Symbol<'a, DtwaingetcurrentfilenameFunc>,
    DTWAIN_GetCurrentFileNameAFunc: Symbol<'a, DtwaingetcurrentfilenameaFunc>,
    DTWAIN_GetCurrentFileNameWFunc: Symbol<'a, DtwaingetcurrentfilenamewFunc>,
    DTWAIN_GetCurrentPageNumFunc: Symbol<'a, DtwaingetcurrentpagenumFunc>,
    DTWAIN_GetCurrentRetryCountFunc: Symbol<'a, DtwaingetcurrentretrycountFunc>,
    DTWAIN_GetCurrentTwainTripletFunc: Symbol<'a, DtwaingetcurrenttwaintripletFunc>,
    DTWAIN_GetCustomDSDataFunc: Symbol<'a, DtwaingetcustomdsdataFunc>,
    DTWAIN_GetDSMFullNameFunc: Symbol<'a, DtwaingetdsmfullnameFunc>,
    DTWAIN_GetDSMFullNameAFunc: Symbol<'a, DtwaingetdsmfullnameaFunc>,
    DTWAIN_GetDSMFullNameWFunc: Symbol<'a, DtwaingetdsmfullnamewFunc>,
    DTWAIN_GetDSMSearchOrderFunc: Symbol<'a, DtwaingetdsmsearchorderFunc>,
    DTWAIN_GetDTWAINHandleFunc: Symbol<'a, DtwaingetdtwainhandleFunc>,
    DTWAIN_GetDeviceEventFunc: Symbol<'a, DtwaingetdeviceeventFunc>,
    DTWAIN_GetDeviceEventExFunc: Symbol<'a, DtwaingetdeviceeventexFunc>,
    DTWAIN_GetDeviceEventInfoFunc: Symbol<'a, DtwaingetdeviceeventinfoFunc>,
    DTWAIN_GetDeviceNotificationsFunc: Symbol<'a, DtwaingetdevicenotificationsFunc>,
    DTWAIN_GetDeviceTimeDateFunc: Symbol<'a, DtwaingetdevicetimedateFunc>,
    DTWAIN_GetDeviceTimeDateAFunc: Symbol<'a, DtwaingetdevicetimedateaFunc>,
    DTWAIN_GetDeviceTimeDateWFunc: Symbol<'a, DtwaingetdevicetimedatewFunc>,
    DTWAIN_GetDoubleFeedDetectLengthFunc: Symbol<'a, DtwaingetdoublefeeddetectlengthFunc>,
    DTWAIN_GetDoubleFeedDetectValuesFunc: Symbol<'a, DtwaingetdoublefeeddetectvaluesFunc>,
    DTWAIN_GetDuplexTypeFunc: Symbol<'a, DtwaingetduplextypeFunc>,
    DTWAIN_GetErrorBufferFunc: Symbol<'a, DtwaingeterrorbufferFunc>,
    DTWAIN_GetErrorBufferThresholdFunc: Symbol<'a, DtwaingeterrorbufferthresholdFunc>,
    DTWAIN_GetErrorCallbackFunc: Symbol<'a, DtwaingeterrorcallbackFunc>,
    DTWAIN_GetErrorCallback64Func: Symbol<'a, Dtwaingeterrorcallback64Func>,
    DTWAIN_GetErrorStringFunc: Symbol<'a, DtwaingeterrorstringFunc>,
    DTWAIN_GetErrorStringAFunc: Symbol<'a, DtwaingeterrorstringaFunc>,
    DTWAIN_GetErrorStringWFunc: Symbol<'a, DtwaingeterrorstringwFunc>,
    DTWAIN_GetExtCapFromNameFunc: Symbol<'a, DtwaingetextcapfromnameFunc>,
    DTWAIN_GetExtCapFromNameAFunc: Symbol<'a, DtwaingetextcapfromnameaFunc>,
    DTWAIN_GetExtCapFromNameWFunc: Symbol<'a, DtwaingetextcapfromnamewFunc>,
    DTWAIN_GetExtImageInfoFunc: Symbol<'a, DtwaingetextimageinfoFunc>,
    DTWAIN_GetExtImageInfoDataFunc: Symbol<'a, DtwaingetextimageinfodataFunc>,
    DTWAIN_GetExtImageInfoDataExFunc: Symbol<'a, DtwaingetextimageinfodataexFunc>,
    DTWAIN_GetExtImageInfoItemFunc: Symbol<'a, DtwaingetextimageinfoitemFunc>,
    DTWAIN_GetExtImageInfoItemExFunc: Symbol<'a, DtwaingetextimageinfoitemexFunc>,
    DTWAIN_GetExtNameFromCapFunc: Symbol<'a, DtwaingetextnamefromcapFunc>,
    DTWAIN_GetExtNameFromCapAFunc: Symbol<'a, DtwaingetextnamefromcapaFunc>,
    DTWAIN_GetExtNameFromCapWFunc: Symbol<'a, DtwaingetextnamefromcapwFunc>,
    DTWAIN_GetFeederAlignmentFunc: Symbol<'a, DtwaingetfeederalignmentFunc>,
    DTWAIN_GetFeederFuncsFunc: Symbol<'a, DtwaingetfeederfuncsFunc>,
    DTWAIN_GetFeederOrderFunc: Symbol<'a, DtwaingetfeederorderFunc>,
    DTWAIN_GetFeederWaitTimeFunc: Symbol<'a, DtwaingetfeederwaittimeFunc>,
    DTWAIN_GetFileCompressionTypeFunc: Symbol<'a, DtwaingetfilecompressiontypeFunc>,
    DTWAIN_GetFileTypeExtensionsFunc: Symbol<'a, DtwaingetfiletypeextensionsFunc>,
    DTWAIN_GetFileTypeExtensionsAFunc: Symbol<'a, DtwaingetfiletypeextensionsaFunc>,
    DTWAIN_GetFileTypeExtensionsWFunc: Symbol<'a, DtwaingetfiletypeextensionswFunc>,
    DTWAIN_GetFileTypeNameFunc: Symbol<'a, DtwaingetfiletypenameFunc>,
    DTWAIN_GetFileTypeNameAFunc: Symbol<'a, DtwaingetfiletypenameaFunc>,
    DTWAIN_GetFileTypeNameWFunc: Symbol<'a, DtwaingetfiletypenamewFunc>,
    DTWAIN_GetHalftoneFunc: Symbol<'a, DtwaingethalftoneFunc>,
    DTWAIN_GetHalftoneAFunc: Symbol<'a, DtwaingethalftoneaFunc>,
    DTWAIN_GetHalftoneWFunc: Symbol<'a, DtwaingethalftonewFunc>,
    DTWAIN_GetHighlightFunc: Symbol<'a, DtwaingethighlightFunc>,
    DTWAIN_GetHighlightStringFunc: Symbol<'a, DtwaingethighlightstringFunc>,
    DTWAIN_GetHighlightStringAFunc: Symbol<'a, DtwaingethighlightstringaFunc>,
    DTWAIN_GetHighlightStringWFunc: Symbol<'a, DtwaingethighlightstringwFunc>,
    DTWAIN_GetImageInfoFunc: Symbol<'a, DtwaingetimageinfoFunc>,
    DTWAIN_GetImageInfoStringFunc: Symbol<'a, DtwaingetimageinfostringFunc>,
    DTWAIN_GetImageInfoStringAFunc: Symbol<'a, DtwaingetimageinfostringaFunc>,
    DTWAIN_GetImageInfoStringWFunc: Symbol<'a, DtwaingetimageinfostringwFunc>,
    DTWAIN_GetJobControlFunc: Symbol<'a, DtwaingetjobcontrolFunc>,
    DTWAIN_GetJpegValuesFunc: Symbol<'a, DtwaingetjpegvaluesFunc>,
    DTWAIN_GetJpegXRValuesFunc: Symbol<'a, DtwaingetjpegxrvaluesFunc>,
    DTWAIN_GetLanguageFunc: Symbol<'a, DtwaingetlanguageFunc>,
    DTWAIN_GetLastErrorFunc: Symbol<'a, DtwaingetlasterrorFunc>,
    DTWAIN_GetLibraryPathFunc: Symbol<'a, DtwaingetlibrarypathFunc>,
    DTWAIN_GetLibraryPathAFunc: Symbol<'a, DtwaingetlibrarypathaFunc>,
    DTWAIN_GetLibraryPathWFunc: Symbol<'a, DtwaingetlibrarypathwFunc>,
    DTWAIN_GetLightPathFunc: Symbol<'a, DtwaingetlightpathFunc>,
    DTWAIN_GetLightSourceFunc: Symbol<'a, DtwaingetlightsourceFunc>,
    DTWAIN_GetLightSourcesFunc: Symbol<'a, DtwaingetlightsourcesFunc>,
    DTWAIN_GetLoggerCallbackFunc: Symbol<'a, DtwaingetloggercallbackFunc>,
    DTWAIN_GetLoggerCallbackAFunc: Symbol<'a, DtwaingetloggercallbackaFunc>,
    DTWAIN_GetLoggerCallbackWFunc: Symbol<'a, DtwaingetloggercallbackwFunc>,
    DTWAIN_GetManualDuplexCountFunc: Symbol<'a, DtwaingetmanualduplexcountFunc>,
    DTWAIN_GetMaxAcquisitionsFunc: Symbol<'a, DtwaingetmaxacquisitionsFunc>,
    DTWAIN_GetMaxBuffersFunc: Symbol<'a, DtwaingetmaxbuffersFunc>,
    DTWAIN_GetMaxPagesToAcquireFunc: Symbol<'a, DtwaingetmaxpagestoacquireFunc>,
    DTWAIN_GetMaxRetryAttemptsFunc: Symbol<'a, DtwaingetmaxretryattemptsFunc>,
    DTWAIN_GetNameFromCapFunc: Symbol<'a, DtwaingetnamefromcapFunc>,
    DTWAIN_GetNameFromCapAFunc: Symbol<'a, DtwaingetnamefromcapaFunc>,
    DTWAIN_GetNameFromCapWFunc: Symbol<'a, DtwaingetnamefromcapwFunc>,
    DTWAIN_GetNoiseFilterFunc: Symbol<'a, DtwaingetnoisefilterFunc>,
    DTWAIN_GetNumAcquiredImagesFunc: Symbol<'a, DtwaingetnumacquiredimagesFunc>,
    DTWAIN_GetNumAcquisitionsFunc: Symbol<'a, DtwaingetnumacquisitionsFunc>,
    DTWAIN_GetOCRCapValuesFunc: Symbol<'a, DtwaingetocrcapvaluesFunc>,
    DTWAIN_GetOCRErrorStringFunc: Symbol<'a, DtwaingetocrerrorstringFunc>,
    DTWAIN_GetOCRErrorStringAFunc: Symbol<'a, DtwaingetocrerrorstringaFunc>,
    DTWAIN_GetOCRErrorStringWFunc: Symbol<'a, DtwaingetocrerrorstringwFunc>,
    DTWAIN_GetOCRLastErrorFunc: Symbol<'a, DtwaingetocrlasterrorFunc>,
    DTWAIN_GetOCRMajorMinorVersionFunc: Symbol<'a, DtwaingetocrmajorminorversionFunc>,
    DTWAIN_GetOCRManufacturerFunc: Symbol<'a, DtwaingetocrmanufacturerFunc>,
    DTWAIN_GetOCRManufacturerAFunc: Symbol<'a, DtwaingetocrmanufactureraFunc>,
    DTWAIN_GetOCRManufacturerWFunc: Symbol<'a, DtwaingetocrmanufacturerwFunc>,
    DTWAIN_GetOCRProductFamilyFunc: Symbol<'a, DtwaingetocrproductfamilyFunc>,
    DTWAIN_GetOCRProductFamilyAFunc: Symbol<'a, DtwaingetocrproductfamilyaFunc>,
    DTWAIN_GetOCRProductFamilyWFunc: Symbol<'a, DtwaingetocrproductfamilywFunc>,
    DTWAIN_GetOCRProductNameFunc: Symbol<'a, DtwaingetocrproductnameFunc>,
    DTWAIN_GetOCRProductNameAFunc: Symbol<'a, DtwaingetocrproductnameaFunc>,
    DTWAIN_GetOCRProductNameWFunc: Symbol<'a, DtwaingetocrproductnamewFunc>,
    DTWAIN_GetOCRTextFunc: Symbol<'a, DtwaingetocrtextFunc>,
    DTWAIN_GetOCRTextAFunc: Symbol<'a, DtwaingetocrtextaFunc>,
    DTWAIN_GetOCRTextInfoFloatFunc: Symbol<'a, DtwaingetocrtextinfofloatFunc>,
    DTWAIN_GetOCRTextInfoFloatExFunc: Symbol<'a, DtwaingetocrtextinfofloatexFunc>,
    DTWAIN_GetOCRTextInfoHandleFunc: Symbol<'a, DtwaingetocrtextinfohandleFunc>,
    DTWAIN_GetOCRTextInfoLongFunc: Symbol<'a, DtwaingetocrtextinfolongFunc>,
    DTWAIN_GetOCRTextInfoLongExFunc: Symbol<'a, DtwaingetocrtextinfolongexFunc>,
    DTWAIN_GetOCRTextWFunc: Symbol<'a, DtwaingetocrtextwFunc>,
    DTWAIN_GetOCRVersionInfoFunc: Symbol<'a, DtwaingetocrversioninfoFunc>,
    DTWAIN_GetOCRVersionInfoAFunc: Symbol<'a, DtwaingetocrversioninfoaFunc>,
    DTWAIN_GetOCRVersionInfoWFunc: Symbol<'a, DtwaingetocrversioninfowFunc>,
    DTWAIN_GetOrientationFunc: Symbol<'a, DtwaingetorientationFunc>,
    DTWAIN_GetOverscanFunc: Symbol<'a, DtwaingetoverscanFunc>,
    DTWAIN_GetPDFTextElementFloatFunc: Symbol<'a, DtwaingetpdftextelementfloatFunc>,
    DTWAIN_GetPDFTextElementLongFunc: Symbol<'a, DtwaingetpdftextelementlongFunc>,
    DTWAIN_GetPDFTextElementStringFunc: Symbol<'a, DtwaingetpdftextelementstringFunc>,
    DTWAIN_GetPDFTextElementStringAFunc: Symbol<'a, DtwaingetpdftextelementstringaFunc>,
    DTWAIN_GetPDFTextElementStringWFunc: Symbol<'a, DtwaingetpdftextelementstringwFunc>,
    DTWAIN_GetPDFType1FontNameFunc: Symbol<'a, Dtwaingetpdftype1fontnameFunc>,
    DTWAIN_GetPDFType1FontNameAFunc: Symbol<'a, Dtwaingetpdftype1fontnameaFunc>,
    DTWAIN_GetPDFType1FontNameWFunc: Symbol<'a, Dtwaingetpdftype1fontnamewFunc>,
    DTWAIN_GetPaperSizeFunc: Symbol<'a, DtwaingetpapersizeFunc>,
    DTWAIN_GetPaperSizeNameFunc: Symbol<'a, DtwaingetpapersizenameFunc>,
    DTWAIN_GetPaperSizeNameAFunc: Symbol<'a, DtwaingetpapersizenameaFunc>,
    DTWAIN_GetPaperSizeNameWFunc: Symbol<'a, DtwaingetpapersizenamewFunc>,
    DTWAIN_GetPatchMaxPrioritiesFunc: Symbol<'a, DtwaingetpatchmaxprioritiesFunc>,
    DTWAIN_GetPatchMaxRetriesFunc: Symbol<'a, DtwaingetpatchmaxretriesFunc>,
    DTWAIN_GetPatchPrioritiesFunc: Symbol<'a, DtwaingetpatchprioritiesFunc>,
    DTWAIN_GetPatchSearchModeFunc: Symbol<'a, DtwaingetpatchsearchmodeFunc>,
    DTWAIN_GetPatchTimeOutFunc: Symbol<'a, DtwaingetpatchtimeoutFunc>,
    DTWAIN_GetPixelFlavorFunc: Symbol<'a, DtwaingetpixelflavorFunc>,
    DTWAIN_GetPixelTypeFunc: Symbol<'a, DtwaingetpixeltypeFunc>,
    DTWAIN_GetPrinterFunc: Symbol<'a, DtwaingetprinterFunc>,
    DTWAIN_GetPrinterStartNumberFunc: Symbol<'a, DtwaingetprinterstartnumberFunc>,
    DTWAIN_GetPrinterStringModeFunc: Symbol<'a, DtwaingetprinterstringmodeFunc>,
    DTWAIN_GetPrinterStringsFunc: Symbol<'a, DtwaingetprinterstringsFunc>,
    DTWAIN_GetPrinterSuffixStringFunc: Symbol<'a, DtwaingetprintersuffixstringFunc>,
    DTWAIN_GetPrinterSuffixStringAFunc: Symbol<'a, DtwaingetprintersuffixstringaFunc>,
    DTWAIN_GetPrinterSuffixStringWFunc: Symbol<'a, DtwaingetprintersuffixstringwFunc>,
    DTWAIN_GetRegisteredMsgFunc: Symbol<'a, DtwaingetregisteredmsgFunc>,
    DTWAIN_GetResolutionFunc: Symbol<'a, DtwaingetresolutionFunc>,
    DTWAIN_GetResolutionStringFunc: Symbol<'a, DtwaingetresolutionstringFunc>,
    DTWAIN_GetResolutionStringAFunc: Symbol<'a, DtwaingetresolutionstringaFunc>,
    DTWAIN_GetResolutionStringWFunc: Symbol<'a, DtwaingetresolutionstringwFunc>,
    DTWAIN_GetResourceStringFunc: Symbol<'a, DtwaingetresourcestringFunc>,
    DTWAIN_GetResourceStringAFunc: Symbol<'a, DtwaingetresourcestringaFunc>,
    DTWAIN_GetResourceStringWFunc: Symbol<'a, DtwaingetresourcestringwFunc>,
    DTWAIN_GetRotationFunc: Symbol<'a, DtwaingetrotationFunc>,
    DTWAIN_GetRotationStringFunc: Symbol<'a, DtwaingetrotationstringFunc>,
    DTWAIN_GetRotationStringAFunc: Symbol<'a, DtwaingetrotationstringaFunc>,
    DTWAIN_GetRotationStringWFunc: Symbol<'a, DtwaingetrotationstringwFunc>,
    DTWAIN_GetSaveFileNameFunc: Symbol<'a, DtwaingetsavefilenameFunc>,
    DTWAIN_GetSaveFileNameAFunc: Symbol<'a, DtwaingetsavefilenameaFunc>,
    DTWAIN_GetSaveFileNameWFunc: Symbol<'a, DtwaingetsavefilenamewFunc>,
    DTWAIN_GetSavedFilesCountFunc: Symbol<'a, DtwaingetsavedfilescountFunc>,
    DTWAIN_GetSessionDetailsFunc: Symbol<'a, DtwaingetsessiondetailsFunc>,
    DTWAIN_GetSessionDetailsAFunc: Symbol<'a, DtwaingetsessiondetailsaFunc>,
    DTWAIN_GetSessionDetailsWFunc: Symbol<'a, DtwaingetsessiondetailswFunc>,
    DTWAIN_GetShadowFunc: Symbol<'a, DtwaingetshadowFunc>,
    DTWAIN_GetShadowStringFunc: Symbol<'a, DtwaingetshadowstringFunc>,
    DTWAIN_GetShadowStringAFunc: Symbol<'a, DtwaingetshadowstringaFunc>,
    DTWAIN_GetShadowStringWFunc: Symbol<'a, DtwaingetshadowstringwFunc>,
    DTWAIN_GetShortVersionStringFunc: Symbol<'a, DtwaingetshortversionstringFunc>,
    DTWAIN_GetShortVersionStringAFunc: Symbol<'a, DtwaingetshortversionstringaFunc>,
    DTWAIN_GetShortVersionStringWFunc: Symbol<'a, DtwaingetshortversionstringwFunc>,
    DTWAIN_GetSourceAcquisitionsFunc: Symbol<'a, DtwaingetsourceacquisitionsFunc>,
    DTWAIN_GetSourceDetailsFunc: Symbol<'a, DtwaingetsourcedetailsFunc>,
    DTWAIN_GetSourceDetailsAFunc: Symbol<'a, DtwaingetsourcedetailsaFunc>,
    DTWAIN_GetSourceDetailsWFunc: Symbol<'a, DtwaingetsourcedetailswFunc>,
    DTWAIN_GetSourceIDFunc: Symbol<'a, DtwaingetsourceidFunc>,
    DTWAIN_GetSourceIDExFunc: Symbol<'a, DtwaingetsourceidexFunc>,
    DTWAIN_GetSourceManufacturerFunc: Symbol<'a, DtwaingetsourcemanufacturerFunc>,
    DTWAIN_GetSourceManufacturerAFunc: Symbol<'a, DtwaingetsourcemanufactureraFunc>,
    DTWAIN_GetSourceManufacturerWFunc: Symbol<'a, DtwaingetsourcemanufacturerwFunc>,
    DTWAIN_GetSourceProductFamilyFunc: Symbol<'a, DtwaingetsourceproductfamilyFunc>,
    DTWAIN_GetSourceProductFamilyAFunc: Symbol<'a, DtwaingetsourceproductfamilyaFunc>,
    DTWAIN_GetSourceProductFamilyWFunc: Symbol<'a, DtwaingetsourceproductfamilywFunc>,
    DTWAIN_GetSourceProductNameFunc: Symbol<'a, DtwaingetsourceproductnameFunc>,
    DTWAIN_GetSourceProductNameAFunc: Symbol<'a, DtwaingetsourceproductnameaFunc>,
    DTWAIN_GetSourceProductNameWFunc: Symbol<'a, DtwaingetsourceproductnamewFunc>,
    DTWAIN_GetSourceUnitFunc: Symbol<'a, DtwaingetsourceunitFunc>,
    DTWAIN_GetSourceVersionInfoFunc: Symbol<'a, DtwaingetsourceversioninfoFunc>,
    DTWAIN_GetSourceVersionInfoAFunc: Symbol<'a, DtwaingetsourceversioninfoaFunc>,
    DTWAIN_GetSourceVersionInfoWFunc: Symbol<'a, DtwaingetsourceversioninfowFunc>,
    DTWAIN_GetSourceVersionNumberFunc: Symbol<'a, DtwaingetsourceversionnumberFunc>,
    DTWAIN_GetStaticLibVersionFunc: Symbol<'a, DtwaingetstaticlibversionFunc>,
    DTWAIN_GetTempFileDirectoryFunc: Symbol<'a, DtwaingettempfiledirectoryFunc>,
    DTWAIN_GetTempFileDirectoryAFunc: Symbol<'a, DtwaingettempfiledirectoryaFunc>,
    DTWAIN_GetTempFileDirectoryWFunc: Symbol<'a, DtwaingettempfiledirectorywFunc>,
    DTWAIN_GetThresholdFunc: Symbol<'a, DtwaingetthresholdFunc>,
    DTWAIN_GetThresholdStringFunc: Symbol<'a, DtwaingetthresholdstringFunc>,
    DTWAIN_GetThresholdStringAFunc: Symbol<'a, DtwaingetthresholdstringaFunc>,
    DTWAIN_GetThresholdStringWFunc: Symbol<'a, DtwaingetthresholdstringwFunc>,
    DTWAIN_GetTimeDateFunc: Symbol<'a, DtwaingettimedateFunc>,
    DTWAIN_GetTimeDateAFunc: Symbol<'a, DtwaingettimedateaFunc>,
    DTWAIN_GetTimeDateWFunc: Symbol<'a, DtwaingettimedatewFunc>,
    DTWAIN_GetTwainAppIDFunc: Symbol<'a, DtwaingettwainappidFunc>,
    DTWAIN_GetTwainAppIDExFunc: Symbol<'a, DtwaingettwainappidexFunc>,
    DTWAIN_GetTwainAvailabilityFunc: Symbol<'a, DtwaingettwainavailabilityFunc>,
    DTWAIN_GetTwainAvailabilityExFunc: Symbol<'a, DtwaingettwainavailabilityexFunc>,
    DTWAIN_GetTwainAvailabilityExAFunc: Symbol<'a, DtwaingettwainavailabilityexaFunc>,
    DTWAIN_GetTwainAvailabilityExWFunc: Symbol<'a, DtwaingettwainavailabilityexwFunc>,
    DTWAIN_GetTwainCountryNameFunc: Symbol<'a, DtwaingettwaincountrynameFunc>,
    DTWAIN_GetTwainCountryNameAFunc: Symbol<'a, DtwaingettwaincountrynameaFunc>,
    DTWAIN_GetTwainCountryNameWFunc: Symbol<'a, DtwaingettwaincountrynamewFunc>,
    DTWAIN_GetTwainCountryValueFunc: Symbol<'a, DtwaingettwaincountryvalueFunc>,
    DTWAIN_GetTwainCountryValueAFunc: Symbol<'a, DtwaingettwaincountryvalueaFunc>,
    DTWAIN_GetTwainCountryValueWFunc: Symbol<'a, DtwaingettwaincountryvaluewFunc>,
    DTWAIN_GetTwainHwndFunc: Symbol<'a, DtwaingettwainhwndFunc>,
    DTWAIN_GetTwainIDFromNameFunc: Symbol<'a, DtwaingettwainidfromnameFunc>,
    DTWAIN_GetTwainIDFromNameAFunc: Symbol<'a, DtwaingettwainidfromnameaFunc>,
    DTWAIN_GetTwainIDFromNameWFunc: Symbol<'a, DtwaingettwainidfromnamewFunc>,
    DTWAIN_GetTwainLanguageNameFunc: Symbol<'a, DtwaingettwainlanguagenameFunc>,
    DTWAIN_GetTwainLanguageNameAFunc: Symbol<'a, DtwaingettwainlanguagenameaFunc>,
    DTWAIN_GetTwainLanguageNameWFunc: Symbol<'a, DtwaingettwainlanguagenamewFunc>,
    DTWAIN_GetTwainLanguageValueFunc: Symbol<'a, DtwaingettwainlanguagevalueFunc>,
    DTWAIN_GetTwainLanguageValueAFunc: Symbol<'a, DtwaingettwainlanguagevalueaFunc>,
    DTWAIN_GetTwainLanguageValueWFunc: Symbol<'a, DtwaingettwainlanguagevaluewFunc>,
    DTWAIN_GetTwainModeFunc: Symbol<'a, DtwaingettwainmodeFunc>,
    DTWAIN_GetTwainNameFromConstantFunc: Symbol<'a, DtwaingettwainnamefromconstantFunc>,
    DTWAIN_GetTwainNameFromConstantAFunc: Symbol<'a, DtwaingettwainnamefromconstantaFunc>,
    DTWAIN_GetTwainNameFromConstantWFunc: Symbol<'a, DtwaingettwainnamefromconstantwFunc>,
    DTWAIN_GetTwainStringNameFunc: Symbol<'a, DtwaingettwainstringnameFunc>,
    DTWAIN_GetTwainStringNameAFunc: Symbol<'a, DtwaingettwainstringnameaFunc>,
    DTWAIN_GetTwainStringNameWFunc: Symbol<'a, DtwaingettwainstringnamewFunc>,
    DTWAIN_GetTwainTimeoutFunc: Symbol<'a, DtwaingettwaintimeoutFunc>,
    DTWAIN_GetVersionFunc: Symbol<'a, DtwaingetversionFunc>,
    DTWAIN_GetVersionCopyrightFunc: Symbol<'a, DtwaingetversioncopyrightFunc>,
    DTWAIN_GetVersionCopyrightAFunc: Symbol<'a, DtwaingetversioncopyrightaFunc>,
    DTWAIN_GetVersionCopyrightWFunc: Symbol<'a, DtwaingetversioncopyrightwFunc>,
    DTWAIN_GetVersionExFunc: Symbol<'a, DtwaingetversionexFunc>,
    DTWAIN_GetVersionInfoFunc: Symbol<'a, DtwaingetversioninfoFunc>,
    DTWAIN_GetVersionInfoAFunc: Symbol<'a, DtwaingetversioninfoaFunc>,
    DTWAIN_GetVersionInfoWFunc: Symbol<'a, DtwaingetversioninfowFunc>,
    DTWAIN_GetVersionStringFunc: Symbol<'a, DtwaingetversionstringFunc>,
    DTWAIN_GetVersionStringAFunc: Symbol<'a, DtwaingetversionstringaFunc>,
    DTWAIN_GetVersionStringWFunc: Symbol<'a, DtwaingetversionstringwFunc>,
    DTWAIN_GetWindowsVersionInfoFunc: Symbol<'a, DtwaingetwindowsversioninfoFunc>,
    DTWAIN_GetWindowsVersionInfoAFunc: Symbol<'a, DtwaingetwindowsversioninfoaFunc>,
    DTWAIN_GetWindowsVersionInfoWFunc: Symbol<'a, DtwaingetwindowsversioninfowFunc>,
    DTWAIN_GetXResolutionFunc: Symbol<'a, DtwaingetxresolutionFunc>,
    DTWAIN_GetXResolutionStringFunc: Symbol<'a, DtwaingetxresolutionstringFunc>,
    DTWAIN_GetXResolutionStringAFunc: Symbol<'a, DtwaingetxresolutionstringaFunc>,
    DTWAIN_GetXResolutionStringWFunc: Symbol<'a, DtwaingetxresolutionstringwFunc>,
    DTWAIN_GetYResolutionFunc: Symbol<'a, DtwaingetyresolutionFunc>,
    DTWAIN_GetYResolutionStringFunc: Symbol<'a, DtwaingetyresolutionstringFunc>,
    DTWAIN_GetYResolutionStringAFunc: Symbol<'a, DtwaingetyresolutionstringaFunc>,
    DTWAIN_GetYResolutionStringWFunc: Symbol<'a, DtwaingetyresolutionstringwFunc>,
    DTWAIN_InitExtImageInfoFunc: Symbol<'a, DtwaininitextimageinfoFunc>,
    DTWAIN_InitImageFileAppendFunc: Symbol<'a, DtwaininitimagefileappendFunc>,
    DTWAIN_InitImageFileAppendAFunc: Symbol<'a, DtwaininitimagefileappendaFunc>,
    DTWAIN_InitImageFileAppendWFunc: Symbol<'a, DtwaininitimagefileappendwFunc>,
    DTWAIN_InitOCRInterfaceFunc: Symbol<'a, DtwaininitocrinterfaceFunc>,
    DTWAIN_IsAcquiringFunc: Symbol<'a, DtwainisacquiringFunc>,
    DTWAIN_IsAudioXferSupportedFunc: Symbol<'a, DtwainisaudioxfersupportedFunc>,
    DTWAIN_IsAutoBorderDetectEnabledFunc: Symbol<'a, DtwainisautoborderdetectenabledFunc>,
    DTWAIN_IsAutoBorderDetectSupportedFunc: Symbol<'a, DtwainisautoborderdetectsupportedFunc>,
    DTWAIN_IsAutoBrightEnabledFunc: Symbol<'a, DtwainisautobrightenabledFunc>,
    DTWAIN_IsAutoBrightSupportedFunc: Symbol<'a, DtwainisautobrightsupportedFunc>,
    DTWAIN_IsAutoDeskewEnabledFunc: Symbol<'a, DtwainisautodeskewenabledFunc>,
    DTWAIN_IsAutoDeskewSupportedFunc: Symbol<'a, DtwainisautodeskewsupportedFunc>,
    DTWAIN_IsAutoFeedEnabledFunc: Symbol<'a, DtwainisautofeedenabledFunc>,
    DTWAIN_IsAutoFeedSupportedFunc: Symbol<'a, DtwainisautofeedsupportedFunc>,
    DTWAIN_IsAutoRotateEnabledFunc: Symbol<'a, DtwainisautorotateenabledFunc>,
    DTWAIN_IsAutoRotateSupportedFunc: Symbol<'a, DtwainisautorotatesupportedFunc>,
    DTWAIN_IsAutoScanEnabledFunc: Symbol<'a, DtwainisautoscanenabledFunc>,
    DTWAIN_IsAutomaticSenseMediumEnabledFunc: Symbol<'a, DtwainisautomaticsensemediumenabledFunc>,
    DTWAIN_IsAutomaticSenseMediumSupportedFunc: Symbol<'a, DtwainisautomaticsensemediumsupportedFunc>,
    DTWAIN_IsBlankPageDetectionOnFunc: Symbol<'a, DtwainisblankpagedetectiononFunc>,
    DTWAIN_IsBufferedTileModeOnFunc: Symbol<'a, DtwainisbufferedtilemodeonFunc>,
    DTWAIN_IsBufferedTileModeSupportedFunc: Symbol<'a, DtwainisbufferedtilemodesupportedFunc>,
    DTWAIN_IsCapSupportedFunc: Symbol<'a, DtwainiscapsupportedFunc>,
    DTWAIN_IsCompressionSupportedFunc: Symbol<'a, DtwainiscompressionsupportedFunc>,
    DTWAIN_IsCustomDSDataSupportedFunc: Symbol<'a, DtwainiscustomdsdatasupportedFunc>,
    DTWAIN_IsDIBBlankFunc: Symbol<'a, DtwainisdibblankFunc>,
    DTWAIN_IsDIBBlankStringFunc: Symbol<'a, DtwainisdibblankstringFunc>,
    DTWAIN_IsDIBBlankStringAFunc: Symbol<'a, DtwainisdibblankstringaFunc>,
    DTWAIN_IsDIBBlankStringWFunc: Symbol<'a, DtwainisdibblankstringwFunc>,
    DTWAIN_IsDeviceEventSupportedFunc: Symbol<'a, DtwainisdeviceeventsupportedFunc>,
    DTWAIN_IsDeviceOnLineFunc: Symbol<'a, DtwainisdeviceonlineFunc>,
    DTWAIN_IsDoubleFeedDetectLengthSupportedFunc: Symbol<'a, DtwainisdoublefeeddetectlengthsupportedFunc>,
    DTWAIN_IsDoubleFeedDetectSupportedFunc: Symbol<'a, DtwainisdoublefeeddetectsupportedFunc>,
    DTWAIN_IsDuplexEnabledFunc: Symbol<'a, DtwainisduplexenabledFunc>,
    DTWAIN_IsDuplexSupportedFunc: Symbol<'a, DtwainisduplexsupportedFunc>,
    DTWAIN_IsExtImageInfoSupportedFunc: Symbol<'a, DtwainisextimageinfosupportedFunc>,
    DTWAIN_IsFeederEnabledFunc: Symbol<'a, DtwainisfeederenabledFunc>,
    DTWAIN_IsFeederLoadedFunc: Symbol<'a, DtwainisfeederloadedFunc>,
    DTWAIN_IsFeederSensitiveFunc: Symbol<'a, DtwainisfeedersensitiveFunc>,
    DTWAIN_IsFeederSupportedFunc: Symbol<'a, DtwainisfeedersupportedFunc>,
    DTWAIN_IsFileSystemSupportedFunc: Symbol<'a, DtwainisfilesystemsupportedFunc>,
    DTWAIN_IsFileXferSupportedFunc: Symbol<'a, DtwainisfilexfersupportedFunc>,
    DTWAIN_IsIAFieldALastPageSupportedFunc: Symbol<'a, DtwainisiafieldalastpagesupportedFunc>,
    DTWAIN_IsIAFieldALevelSupportedFunc: Symbol<'a, DtwainisiafieldalevelsupportedFunc>,
    DTWAIN_IsIAFieldAPrintFormatSupportedFunc: Symbol<'a, DtwainisiafieldaprintformatsupportedFunc>,
    DTWAIN_IsIAFieldAValueSupportedFunc: Symbol<'a, DtwainisiafieldavaluesupportedFunc>,
    DTWAIN_IsIAFieldBLastPageSupportedFunc: Symbol<'a, DtwainisiafieldblastpagesupportedFunc>,
    DTWAIN_IsIAFieldBLevelSupportedFunc: Symbol<'a, DtwainisiafieldblevelsupportedFunc>,
    DTWAIN_IsIAFieldBPrintFormatSupportedFunc: Symbol<'a, DtwainisiafieldbprintformatsupportedFunc>,
    DTWAIN_IsIAFieldBValueSupportedFunc: Symbol<'a, DtwainisiafieldbvaluesupportedFunc>,
    DTWAIN_IsIAFieldCLastPageSupportedFunc: Symbol<'a, DtwainisiafieldclastpagesupportedFunc>,
    DTWAIN_IsIAFieldCLevelSupportedFunc: Symbol<'a, DtwainisiafieldclevelsupportedFunc>,
    DTWAIN_IsIAFieldCPrintFormatSupportedFunc: Symbol<'a, DtwainisiafieldcprintformatsupportedFunc>,
    DTWAIN_IsIAFieldCValueSupportedFunc: Symbol<'a, DtwainisiafieldcvaluesupportedFunc>,
    DTWAIN_IsIAFieldDLastPageSupportedFunc: Symbol<'a, DtwainisiafielddlastpagesupportedFunc>,
    DTWAIN_IsIAFieldDLevelSupportedFunc: Symbol<'a, DtwainisiafielddlevelsupportedFunc>,
    DTWAIN_IsIAFieldDPrintFormatSupportedFunc: Symbol<'a, DtwainisiafielddprintformatsupportedFunc>,
    DTWAIN_IsIAFieldDValueSupportedFunc: Symbol<'a, DtwainisiafielddvaluesupportedFunc>,
    DTWAIN_IsIAFieldELastPageSupportedFunc: Symbol<'a, DtwainisiafieldelastpagesupportedFunc>,
    DTWAIN_IsIAFieldELevelSupportedFunc: Symbol<'a, DtwainisiafieldelevelsupportedFunc>,
    DTWAIN_IsIAFieldEPrintFormatSupportedFunc: Symbol<'a, DtwainisiafieldeprintformatsupportedFunc>,
    DTWAIN_IsIAFieldEValueSupportedFunc: Symbol<'a, DtwainisiafieldevaluesupportedFunc>,
    DTWAIN_IsImageAddressingSupportedFunc: Symbol<'a, DtwainisimageaddressingsupportedFunc>,
    DTWAIN_IsIndicatorEnabledFunc: Symbol<'a, DtwainisindicatorenabledFunc>,
    DTWAIN_IsIndicatorSupportedFunc: Symbol<'a, DtwainisindicatorsupportedFunc>,
    DTWAIN_IsInitializedFunc: Symbol<'a, DtwainisinitializedFunc>,
    DTWAIN_IsJPEGSupportedFunc: Symbol<'a, DtwainisjpegsupportedFunc>,
    DTWAIN_IsJobControlSupportedFunc: Symbol<'a, DtwainisjobcontrolsupportedFunc>,
    DTWAIN_IsLampEnabledFunc: Symbol<'a, DtwainislampenabledFunc>,
    DTWAIN_IsLampSupportedFunc: Symbol<'a, DtwainislampsupportedFunc>,
    DTWAIN_IsLightPathSupportedFunc: Symbol<'a, DtwainislightpathsupportedFunc>,
    DTWAIN_IsLightSourceSupportedFunc: Symbol<'a, DtwainislightsourcesupportedFunc>,
    DTWAIN_IsMaxBuffersSupportedFunc: Symbol<'a, DtwainismaxbufferssupportedFunc>,
    DTWAIN_IsMemFileXferSupportedFunc: Symbol<'a, DtwainismemfilexfersupportedFunc>,
    DTWAIN_IsMsgNotifyEnabledFunc: Symbol<'a, DtwainismsgnotifyenabledFunc>,
    DTWAIN_IsNotifyTripletsEnabledFunc: Symbol<'a, DtwainisnotifytripletsenabledFunc>,
    DTWAIN_IsOCREngineActivatedFunc: Symbol<'a, DtwainisocrengineactivatedFunc>,
    DTWAIN_IsOpenSourcesOnSelectFunc: Symbol<'a, DtwainisopensourcesonselectFunc>,
    DTWAIN_IsOrientationSupportedFunc: Symbol<'a, DtwainisorientationsupportedFunc>,
    DTWAIN_IsOverscanSupportedFunc: Symbol<'a, DtwainisoverscansupportedFunc>,
    DTWAIN_IsPDFSupportedFunc: Symbol<'a, DtwainispdfsupportedFunc>,
    DTWAIN_IsPNGSupportedFunc: Symbol<'a, DtwainispngsupportedFunc>,
    DTWAIN_IsPaperDetectableFunc: Symbol<'a, DtwainispaperdetectableFunc>,
    DTWAIN_IsPaperSizeSupportedFunc: Symbol<'a, DtwainispapersizesupportedFunc>,
    DTWAIN_IsPatchCapsSupportedFunc: Symbol<'a, DtwainispatchcapssupportedFunc>,
    DTWAIN_IsPatchDetectEnabledFunc: Symbol<'a, DtwainispatchdetectenabledFunc>,
    DTWAIN_IsPatchSupportedFunc: Symbol<'a, DtwainispatchsupportedFunc>,
    DTWAIN_IsPeekMessageLoopEnabledFunc: Symbol<'a, DtwainispeekmessageloopenabledFunc>,
    DTWAIN_IsPixelTypeSupportedFunc: Symbol<'a, DtwainispixeltypesupportedFunc>,
    DTWAIN_IsPrinterEnabledFunc: Symbol<'a, DtwainisprinterenabledFunc>,
    DTWAIN_IsPrinterSupportedFunc: Symbol<'a, DtwainisprintersupportedFunc>,
    DTWAIN_IsRotationSupportedFunc: Symbol<'a, DtwainisrotationsupportedFunc>,
    DTWAIN_IsSessionEnabledFunc: Symbol<'a, DtwainissessionenabledFunc>,
    DTWAIN_IsSkipImageInfoErrorFunc: Symbol<'a, DtwainisskipimageinfoerrorFunc>,
    DTWAIN_IsSourceAcquiringFunc: Symbol<'a, DtwainissourceacquiringFunc>,
    DTWAIN_IsSourceAcquiringExFunc: Symbol<'a, DtwainissourceacquiringexFunc>,
    DTWAIN_IsSourceInUIOnlyModeFunc: Symbol<'a, DtwainissourceinuionlymodeFunc>,
    DTWAIN_IsSourceOpenFunc: Symbol<'a, DtwainissourceopenFunc>,
    DTWAIN_IsSourceSelectedFunc: Symbol<'a, DtwainissourceselectedFunc>,
    DTWAIN_IsSourceValidFunc: Symbol<'a, DtwainissourcevalidFunc>,
    DTWAIN_IsTIFFSupportedFunc: Symbol<'a, DtwainistiffsupportedFunc>,
    DTWAIN_IsThumbnailEnabledFunc: Symbol<'a, DtwainisthumbnailenabledFunc>,
    DTWAIN_IsThumbnailSupportedFunc: Symbol<'a, DtwainisthumbnailsupportedFunc>,
    DTWAIN_IsTwainAvailableFunc: Symbol<'a, DtwainistwainavailableFunc>,
    DTWAIN_IsTwainAvailableExFunc: Symbol<'a, DtwainistwainavailableexFunc>,
    DTWAIN_IsTwainAvailableExAFunc: Symbol<'a, DtwainistwainavailableexaFunc>,
    DTWAIN_IsTwainAvailableExWFunc: Symbol<'a, DtwainistwainavailableexwFunc>,
    DTWAIN_IsUIControllableFunc: Symbol<'a, DtwainisuicontrollableFunc>,
    DTWAIN_IsUIEnabledFunc: Symbol<'a, DtwainisuienabledFunc>,
    DTWAIN_IsUIOnlySupportedFunc: Symbol<'a, DtwainisuionlysupportedFunc>,
    DTWAIN_LoadCustomStringResourcesFunc: Symbol<'a, DtwainloadcustomstringresourcesFunc>,
    DTWAIN_LoadCustomStringResourcesAFunc: Symbol<'a, DtwainloadcustomstringresourcesaFunc>,
    DTWAIN_LoadCustomStringResourcesExFunc: Symbol<'a, DtwainloadcustomstringresourcesexFunc>,
    DTWAIN_LoadCustomStringResourcesExAFunc: Symbol<'a, DtwainloadcustomstringresourcesexaFunc>,
    DTWAIN_LoadCustomStringResourcesExWFunc: Symbol<'a, DtwainloadcustomstringresourcesexwFunc>,
    DTWAIN_LoadCustomStringResourcesWFunc: Symbol<'a, DtwainloadcustomstringresourceswFunc>,
    DTWAIN_LoadLanguageResourceFunc: Symbol<'a, DtwainloadlanguageresourceFunc>,
    DTWAIN_LockMemoryFunc: Symbol<'a, DtwainlockmemoryFunc>,
    DTWAIN_LockMemoryExFunc: Symbol<'a, DtwainlockmemoryexFunc>,
    DTWAIN_LogMessageFunc: Symbol<'a, DtwainlogmessageFunc>,
    DTWAIN_LogMessageAFunc: Symbol<'a, DtwainlogmessageaFunc>,
    DTWAIN_LogMessageWFunc: Symbol<'a, DtwainlogmessagewFunc>,
    DTWAIN_MakeRGBFunc: Symbol<'a, DtwainmakergbFunc>,
    DTWAIN_OpenSourceFunc: Symbol<'a, DtwainopensourceFunc>,
    DTWAIN_OpenSourcesOnSelectFunc: Symbol<'a, DtwainopensourcesonselectFunc>,
    DTWAIN_RangeCreateFunc: Symbol<'a, DtwainrangecreateFunc>,
    DTWAIN_RangeCreateFromCapFunc: Symbol<'a, DtwainrangecreatefromcapFunc>,
    DTWAIN_RangeDestroyFunc: Symbol<'a, DtwainrangedestroyFunc>,
    DTWAIN_RangeExpandFunc: Symbol<'a, DtwainrangeexpandFunc>,
    DTWAIN_RangeExpandExFunc: Symbol<'a, DtwainrangeexpandexFunc>,
    DTWAIN_RangeGetAllFunc: Symbol<'a, DtwainrangegetallFunc>,
    DTWAIN_RangeGetAllFloatFunc: Symbol<'a, DtwainrangegetallfloatFunc>,
    DTWAIN_RangeGetAllFloatStringFunc: Symbol<'a, DtwainrangegetallfloatstringFunc>,
    DTWAIN_RangeGetAllFloatStringAFunc: Symbol<'a, DtwainrangegetallfloatstringaFunc>,
    DTWAIN_RangeGetAllFloatStringWFunc: Symbol<'a, DtwainrangegetallfloatstringwFunc>,
    DTWAIN_RangeGetAllLongFunc: Symbol<'a, DtwainrangegetalllongFunc>,
    DTWAIN_RangeGetCountFunc: Symbol<'a, DtwainrangegetcountFunc>,
    DTWAIN_RangeGetExpValueFunc: Symbol<'a, DtwainrangegetexpvalueFunc>,
    DTWAIN_RangeGetExpValueFloatFunc: Symbol<'a, DtwainrangegetexpvaluefloatFunc>,
    DTWAIN_RangeGetExpValueFloatStringFunc: Symbol<'a, DtwainrangegetexpvaluefloatstringFunc>,
    DTWAIN_RangeGetExpValueFloatStringAFunc: Symbol<'a, DtwainrangegetexpvaluefloatstringaFunc>,
    DTWAIN_RangeGetExpValueFloatStringWFunc: Symbol<'a, DtwainrangegetexpvaluefloatstringwFunc>,
    DTWAIN_RangeGetExpValueLongFunc: Symbol<'a, DtwainrangegetexpvaluelongFunc>,
    DTWAIN_RangeGetNearestValueFunc: Symbol<'a, DtwainrangegetnearestvalueFunc>,
    DTWAIN_RangeGetPosFunc: Symbol<'a, DtwainrangegetposFunc>,
    DTWAIN_RangeGetPosFloatFunc: Symbol<'a, DtwainrangegetposfloatFunc>,
    DTWAIN_RangeGetPosFloatStringFunc: Symbol<'a, DtwainrangegetposfloatstringFunc>,
    DTWAIN_RangeGetPosFloatStringAFunc: Symbol<'a, DtwainrangegetposfloatstringaFunc>,
    DTWAIN_RangeGetPosFloatStringWFunc: Symbol<'a, DtwainrangegetposfloatstringwFunc>,
    DTWAIN_RangeGetPosLongFunc: Symbol<'a, DtwainrangegetposlongFunc>,
    DTWAIN_RangeGetValueFunc: Symbol<'a, DtwainrangegetvalueFunc>,
    DTWAIN_RangeGetValueFloatFunc: Symbol<'a, DtwainrangegetvaluefloatFunc>,
    DTWAIN_RangeGetValueFloatStringFunc: Symbol<'a, DtwainrangegetvaluefloatstringFunc>,
    DTWAIN_RangeGetValueFloatStringAFunc: Symbol<'a, DtwainrangegetvaluefloatstringaFunc>,
    DTWAIN_RangeGetValueFloatStringWFunc: Symbol<'a, DtwainrangegetvaluefloatstringwFunc>,
    DTWAIN_RangeGetValueLongFunc: Symbol<'a, DtwainrangegetvaluelongFunc>,
    DTWAIN_RangeIsValidFunc: Symbol<'a, DtwainrangeisvalidFunc>,
    DTWAIN_RangeNearestValueFloatFunc: Symbol<'a, DtwainrangenearestvaluefloatFunc>,
    DTWAIN_RangeNearestValueFloatStringFunc: Symbol<'a, DtwainrangenearestvaluefloatstringFunc>,
    DTWAIN_RangeNearestValueFloatStringAFunc: Symbol<'a, DtwainrangenearestvaluefloatstringaFunc>,
    DTWAIN_RangeNearestValueFloatStringWFunc: Symbol<'a, DtwainrangenearestvaluefloatstringwFunc>,
    DTWAIN_RangeNearestValueLongFunc: Symbol<'a, DtwainrangenearestvaluelongFunc>,
    DTWAIN_RangeSetAllFunc: Symbol<'a, DtwainrangesetallFunc>,
    DTWAIN_RangeSetAllFloatFunc: Symbol<'a, DtwainrangesetallfloatFunc>,
    DTWAIN_RangeSetAllFloatStringFunc: Symbol<'a, DtwainrangesetallfloatstringFunc>,
    DTWAIN_RangeSetAllFloatStringAFunc: Symbol<'a, DtwainrangesetallfloatstringaFunc>,
    DTWAIN_RangeSetAllFloatStringWFunc: Symbol<'a, DtwainrangesetallfloatstringwFunc>,
    DTWAIN_RangeSetAllLongFunc: Symbol<'a, DtwainrangesetalllongFunc>,
    DTWAIN_RangeSetValueFunc: Symbol<'a, DtwainrangesetvalueFunc>,
    DTWAIN_RangeSetValueFloatFunc: Symbol<'a, DtwainrangesetvaluefloatFunc>,
    DTWAIN_RangeSetValueFloatStringFunc: Symbol<'a, DtwainrangesetvaluefloatstringFunc>,
    DTWAIN_RangeSetValueFloatStringAFunc: Symbol<'a, DtwainrangesetvaluefloatstringaFunc>,
    DTWAIN_RangeSetValueFloatStringWFunc: Symbol<'a, DtwainrangesetvaluefloatstringwFunc>,
    DTWAIN_RangeSetValueLongFunc: Symbol<'a, DtwainrangesetvaluelongFunc>,
    DTWAIN_ResetPDFTextElementFunc: Symbol<'a, DtwainresetpdftextelementFunc>,
    DTWAIN_RewindPageFunc: Symbol<'a, DtwainrewindpageFunc>,
    DTWAIN_SelectDefaultOCREngineFunc: Symbol<'a, DtwainselectdefaultocrengineFunc>,
    DTWAIN_SelectDefaultSourceFunc: Symbol<'a, DtwainselectdefaultsourceFunc>,
    DTWAIN_SelectDefaultSourceWithOpenFunc: Symbol<'a, DtwainselectdefaultsourcewithopenFunc>,
    DTWAIN_SelectOCREngineFunc: Symbol<'a, DtwainselectocrengineFunc>,
    DTWAIN_SelectOCREngine2Func: Symbol<'a, Dtwainselectocrengine2Func>,
    DTWAIN_SelectOCREngine2AFunc: Symbol<'a, Dtwainselectocrengine2aFunc>,
    DTWAIN_SelectOCREngine2ExFunc: Symbol<'a, Dtwainselectocrengine2exFunc>,
    DTWAIN_SelectOCREngine2ExAFunc: Symbol<'a, Dtwainselectocrengine2exaFunc>,
    DTWAIN_SelectOCREngine2ExWFunc: Symbol<'a, Dtwainselectocrengine2exwFunc>,
    DTWAIN_SelectOCREngine2WFunc: Symbol<'a, Dtwainselectocrengine2wFunc>,
    DTWAIN_SelectOCREngineByNameFunc: Symbol<'a, DtwainselectocrenginebynameFunc>,
    DTWAIN_SelectOCREngineByNameAFunc: Symbol<'a, DtwainselectocrenginebynameaFunc>,
    DTWAIN_SelectOCREngineByNameWFunc: Symbol<'a, DtwainselectocrenginebynamewFunc>,
    DTWAIN_SelectSourceFunc: Symbol<'a, DtwainselectsourceFunc>,
    DTWAIN_SelectSource2Func: Symbol<'a, Dtwainselectsource2Func>,
    DTWAIN_SelectSource2AFunc: Symbol<'a, Dtwainselectsource2aFunc>,
    DTWAIN_SelectSource2ExFunc: Symbol<'a, Dtwainselectsource2exFunc>,
    DTWAIN_SelectSource2ExAFunc: Symbol<'a, Dtwainselectsource2exaFunc>,
    DTWAIN_SelectSource2ExWFunc: Symbol<'a, Dtwainselectsource2exwFunc>,
    DTWAIN_SelectSource2WFunc: Symbol<'a, Dtwainselectsource2wFunc>,
    DTWAIN_SelectSourceByNameFunc: Symbol<'a, DtwainselectsourcebynameFunc>,
    DTWAIN_SelectSourceByNameAFunc: Symbol<'a, DtwainselectsourcebynameaFunc>,
    DTWAIN_SelectSourceByNameWFunc: Symbol<'a, DtwainselectsourcebynamewFunc>,
    DTWAIN_SelectSourceByNameWithOpenFunc: Symbol<'a, DtwainselectsourcebynamewithopenFunc>,
    DTWAIN_SelectSourceByNameWithOpenAFunc: Symbol<'a, DtwainselectsourcebynamewithopenaFunc>,
    DTWAIN_SelectSourceByNameWithOpenWFunc: Symbol<'a, DtwainselectsourcebynamewithopenwFunc>,
    DTWAIN_SelectSourceWithOpenFunc: Symbol<'a, DtwainselectsourcewithopenFunc>,
    DTWAIN_SetAcquireAreaFunc: Symbol<'a, DtwainsetacquireareaFunc>,
    DTWAIN_SetAcquireArea2Func: Symbol<'a, Dtwainsetacquirearea2Func>,
    DTWAIN_SetAcquireArea2StringFunc: Symbol<'a, Dtwainsetacquirearea2stringFunc>,
    DTWAIN_SetAcquireArea2StringAFunc: Symbol<'a, Dtwainsetacquirearea2stringaFunc>,
    DTWAIN_SetAcquireArea2StringWFunc: Symbol<'a, Dtwainsetacquirearea2stringwFunc>,
    DTWAIN_SetAcquireImageNegativeFunc: Symbol<'a, DtwainsetacquireimagenegativeFunc>,
    DTWAIN_SetAcquireImageScaleFunc: Symbol<'a, DtwainsetacquireimagescaleFunc>,
    DTWAIN_SetAcquireImageScaleStringFunc: Symbol<'a, DtwainsetacquireimagescalestringFunc>,
    DTWAIN_SetAcquireImageScaleStringAFunc: Symbol<'a, DtwainsetacquireimagescalestringaFunc>,
    DTWAIN_SetAcquireImageScaleStringWFunc: Symbol<'a, DtwainsetacquireimagescalestringwFunc>,
    DTWAIN_SetAcquireStripBufferFunc: Symbol<'a, DtwainsetacquirestripbufferFunc>,
    DTWAIN_SetAcquireStripSizeFunc: Symbol<'a, DtwainsetacquirestripsizeFunc>,
    DTWAIN_SetAlarmVolumeFunc: Symbol<'a, DtwainsetalarmvolumeFunc>,
    DTWAIN_SetAlarmsFunc: Symbol<'a, DtwainsetalarmsFunc>,
    DTWAIN_SetAllCapsToDefaultFunc: Symbol<'a, DtwainsetallcapstodefaultFunc>,
    DTWAIN_SetAppInfoFunc: Symbol<'a, DtwainsetappinfoFunc>,
    DTWAIN_SetAppInfoAFunc: Symbol<'a, DtwainsetappinfoaFunc>,
    DTWAIN_SetAppInfoWFunc: Symbol<'a, DtwainsetappinfowFunc>,
    DTWAIN_SetAuthorFunc: Symbol<'a, DtwainsetauthorFunc>,
    DTWAIN_SetAuthorAFunc: Symbol<'a, DtwainsetauthoraFunc>,
    DTWAIN_SetAuthorWFunc: Symbol<'a, DtwainsetauthorwFunc>,
    DTWAIN_SetAvailablePrintersFunc: Symbol<'a, DtwainsetavailableprintersFunc>,
    DTWAIN_SetAvailablePrintersArrayFunc: Symbol<'a, DtwainsetavailableprintersarrayFunc>,
    DTWAIN_SetBitDepthFunc: Symbol<'a, DtwainsetbitdepthFunc>,
    DTWAIN_SetBlankPageDetectionFunc: Symbol<'a, DtwainsetblankpagedetectionFunc>,
    DTWAIN_SetBlankPageDetectionExFunc: Symbol<'a, DtwainsetblankpagedetectionexFunc>,
    DTWAIN_SetBlankPageDetectionExStringFunc: Symbol<'a, DtwainsetblankpagedetectionexstringFunc>,
    DTWAIN_SetBlankPageDetectionExStringAFunc: Symbol<'a, DtwainsetblankpagedetectionexstringaFunc>,
    DTWAIN_SetBlankPageDetectionExStringWFunc: Symbol<'a, DtwainsetblankpagedetectionexstringwFunc>,
    DTWAIN_SetBlankPageDetectionStringFunc: Symbol<'a, DtwainsetblankpagedetectionstringFunc>,
    DTWAIN_SetBlankPageDetectionStringAFunc: Symbol<'a, DtwainsetblankpagedetectionstringaFunc>,
    DTWAIN_SetBlankPageDetectionStringWFunc: Symbol<'a, DtwainsetblankpagedetectionstringwFunc>,
    DTWAIN_SetBrightnessFunc: Symbol<'a, DtwainsetbrightnessFunc>,
    DTWAIN_SetBrightnessStringFunc: Symbol<'a, DtwainsetbrightnessstringFunc>,
    DTWAIN_SetBrightnessStringAFunc: Symbol<'a, DtwainsetbrightnessstringaFunc>,
    DTWAIN_SetBrightnessStringWFunc: Symbol<'a, DtwainsetbrightnessstringwFunc>,
    DTWAIN_SetBufferedTileModeFunc: Symbol<'a, DtwainsetbufferedtilemodeFunc>,
    DTWAIN_SetCallbackFunc: Symbol<'a, DtwainsetcallbackFunc>,
    DTWAIN_SetCallback64Func: Symbol<'a, Dtwainsetcallback64Func>,
    DTWAIN_SetCameraFunc: Symbol<'a, DtwainsetcameraFunc>,
    DTWAIN_SetCameraAFunc: Symbol<'a, DtwainsetcameraaFunc>,
    DTWAIN_SetCameraWFunc: Symbol<'a, DtwainsetcamerawFunc>,
    DTWAIN_SetCapValuesFunc: Symbol<'a, DtwainsetcapvaluesFunc>,
    DTWAIN_SetCapValuesExFunc: Symbol<'a, DtwainsetcapvaluesexFunc>,
    DTWAIN_SetCapValuesEx2Func: Symbol<'a, Dtwainsetcapvaluesex2Func>,
    DTWAIN_SetCaptionFunc: Symbol<'a, DtwainsetcaptionFunc>,
    DTWAIN_SetCaptionAFunc: Symbol<'a, DtwainsetcaptionaFunc>,
    DTWAIN_SetCaptionWFunc: Symbol<'a, DtwainsetcaptionwFunc>,
    DTWAIN_SetCompressionTypeFunc: Symbol<'a, DtwainsetcompressiontypeFunc>,
    DTWAIN_SetContrastFunc: Symbol<'a, DtwainsetcontrastFunc>,
    DTWAIN_SetContrastStringFunc: Symbol<'a, DtwainsetcontraststringFunc>,
    DTWAIN_SetContrastStringAFunc: Symbol<'a, DtwainsetcontraststringaFunc>,
    DTWAIN_SetContrastStringWFunc: Symbol<'a, DtwainsetcontraststringwFunc>,
    DTWAIN_SetCountryFunc: Symbol<'a, DtwainsetcountryFunc>,
    DTWAIN_SetCurrentRetryCountFunc: Symbol<'a, DtwainsetcurrentretrycountFunc>,
    DTWAIN_SetCustomDSDataFunc: Symbol<'a, DtwainsetcustomdsdataFunc>,
    DTWAIN_SetDSMSearchOrderFunc: Symbol<'a, DtwainsetdsmsearchorderFunc>,
    DTWAIN_SetDSMSearchOrderExFunc: Symbol<'a, DtwainsetdsmsearchorderexFunc>,
    DTWAIN_SetDSMSearchOrderExAFunc: Symbol<'a, DtwainsetdsmsearchorderexaFunc>,
    DTWAIN_SetDSMSearchOrderExWFunc: Symbol<'a, DtwainsetdsmsearchorderexwFunc>,
    DTWAIN_SetDefaultSourceFunc: Symbol<'a, DtwainsetdefaultsourceFunc>,
    DTWAIN_SetDeviceNotificationsFunc: Symbol<'a, DtwainsetdevicenotificationsFunc>,
    DTWAIN_SetDeviceTimeDateFunc: Symbol<'a, DtwainsetdevicetimedateFunc>,
    DTWAIN_SetDeviceTimeDateAFunc: Symbol<'a, DtwainsetdevicetimedateaFunc>,
    DTWAIN_SetDeviceTimeDateWFunc: Symbol<'a, DtwainsetdevicetimedatewFunc>,
    DTWAIN_SetDoubleFeedDetectLengthFunc: Symbol<'a, DtwainsetdoublefeeddetectlengthFunc>,
    DTWAIN_SetDoubleFeedDetectLengthStringFunc: Symbol<'a, DtwainsetdoublefeeddetectlengthstringFunc>,
    DTWAIN_SetDoubleFeedDetectLengthStringAFunc: Symbol<'a, DtwainsetdoublefeeddetectlengthstringaFunc>,
    DTWAIN_SetDoubleFeedDetectLengthStringWFunc: Symbol<'a, DtwainsetdoublefeeddetectlengthstringwFunc>,
    DTWAIN_SetDoubleFeedDetectValuesFunc: Symbol<'a, DtwainsetdoublefeeddetectvaluesFunc>,
    DTWAIN_SetDoublePageCountOnDuplexFunc: Symbol<'a, DtwainsetdoublepagecountonduplexFunc>,
    DTWAIN_SetEOJDetectValueFunc: Symbol<'a, DtwainseteojdetectvalueFunc>,
    DTWAIN_SetErrorBufferThresholdFunc: Symbol<'a, DtwainseterrorbufferthresholdFunc>,
    DTWAIN_SetErrorCallbackFunc: Symbol<'a, DtwainseterrorcallbackFunc>,
    DTWAIN_SetErrorCallback64Func: Symbol<'a, Dtwainseterrorcallback64Func>,
    DTWAIN_SetFeederAlignmentFunc: Symbol<'a, DtwainsetfeederalignmentFunc>,
    DTWAIN_SetFeederOrderFunc: Symbol<'a, DtwainsetfeederorderFunc>,
    DTWAIN_SetFeederWaitTimeFunc: Symbol<'a, DtwainsetfeederwaittimeFunc>,
    DTWAIN_SetFileAutoIncrementFunc: Symbol<'a, DtwainsetfileautoincrementFunc>,
    DTWAIN_SetFileCompressionTypeFunc: Symbol<'a, DtwainsetfilecompressiontypeFunc>,
    DTWAIN_SetFileSavePosFunc: Symbol<'a, DtwainsetfilesaveposFunc>,
    DTWAIN_SetFileSavePosAFunc: Symbol<'a, DtwainsetfilesaveposaFunc>,
    DTWAIN_SetFileSavePosWFunc: Symbol<'a, DtwainsetfilesaveposwFunc>,
    DTWAIN_SetFileXferFormatFunc: Symbol<'a, DtwainsetfilexferformatFunc>,
    DTWAIN_SetHalftoneFunc: Symbol<'a, DtwainsethalftoneFunc>,
    DTWAIN_SetHalftoneAFunc: Symbol<'a, DtwainsethalftoneaFunc>,
    DTWAIN_SetHalftoneWFunc: Symbol<'a, DtwainsethalftonewFunc>,
    DTWAIN_SetHighlightFunc: Symbol<'a, DtwainsethighlightFunc>,
    DTWAIN_SetHighlightStringFunc: Symbol<'a, DtwainsethighlightstringFunc>,
    DTWAIN_SetHighlightStringAFunc: Symbol<'a, DtwainsethighlightstringaFunc>,
    DTWAIN_SetHighlightStringWFunc: Symbol<'a, DtwainsethighlightstringwFunc>,
    DTWAIN_SetJobControlFunc: Symbol<'a, DtwainsetjobcontrolFunc>,
    DTWAIN_SetJpegValuesFunc: Symbol<'a, DtwainsetjpegvaluesFunc>,
    DTWAIN_SetJpegXRValuesFunc: Symbol<'a, DtwainsetjpegxrvaluesFunc>,
    DTWAIN_SetLanguageFunc: Symbol<'a, DtwainsetlanguageFunc>,
    DTWAIN_SetLastErrorFunc: Symbol<'a, DtwainsetlasterrorFunc>,
    DTWAIN_SetLightPathFunc: Symbol<'a, DtwainsetlightpathFunc>,
    DTWAIN_SetLightPathExFunc: Symbol<'a, DtwainsetlightpathexFunc>,
    DTWAIN_SetLightSourceFunc: Symbol<'a, DtwainsetlightsourceFunc>,
    DTWAIN_SetLightSourcesFunc: Symbol<'a, DtwainsetlightsourcesFunc>,
    DTWAIN_SetLoggerCallbackFunc: Symbol<'a, DtwainsetloggercallbackFunc>,
    DTWAIN_SetLoggerCallbackAFunc: Symbol<'a, DtwainsetloggercallbackaFunc>,
    DTWAIN_SetLoggerCallbackWFunc: Symbol<'a, DtwainsetloggercallbackwFunc>,
    DTWAIN_SetManualDuplexModeFunc: Symbol<'a, DtwainsetmanualduplexmodeFunc>,
    DTWAIN_SetMaxAcquisitionsFunc: Symbol<'a, DtwainsetmaxacquisitionsFunc>,
    DTWAIN_SetMaxBuffersFunc: Symbol<'a, DtwainsetmaxbuffersFunc>,
    DTWAIN_SetMaxRetryAttemptsFunc: Symbol<'a, DtwainsetmaxretryattemptsFunc>,
    DTWAIN_SetMultipageScanModeFunc: Symbol<'a, DtwainsetmultipagescanmodeFunc>,
    DTWAIN_SetNoiseFilterFunc: Symbol<'a, DtwainsetnoisefilterFunc>,
    DTWAIN_SetOCRCapValuesFunc: Symbol<'a, DtwainsetocrcapvaluesFunc>,
    DTWAIN_SetOrientationFunc: Symbol<'a, DtwainsetorientationFunc>,
    DTWAIN_SetOverscanFunc: Symbol<'a, DtwainsetoverscanFunc>,
    DTWAIN_SetPDFAESEncryptionFunc: Symbol<'a, DtwainsetpdfaesencryptionFunc>,
    DTWAIN_SetPDFASCIICompressionFunc: Symbol<'a, DtwainsetpdfasciicompressionFunc>,
    DTWAIN_SetPDFAuthorFunc: Symbol<'a, DtwainsetpdfauthorFunc>,
    DTWAIN_SetPDFAuthorAFunc: Symbol<'a, DtwainsetpdfauthoraFunc>,
    DTWAIN_SetPDFAuthorWFunc: Symbol<'a, DtwainsetpdfauthorwFunc>,
    DTWAIN_SetPDFCompressionFunc: Symbol<'a, DtwainsetpdfcompressionFunc>,
    DTWAIN_SetPDFCreatorFunc: Symbol<'a, DtwainsetpdfcreatorFunc>,
    DTWAIN_SetPDFCreatorAFunc: Symbol<'a, DtwainsetpdfcreatoraFunc>,
    DTWAIN_SetPDFCreatorWFunc: Symbol<'a, DtwainsetpdfcreatorwFunc>,
    DTWAIN_SetPDFEncryptionFunc: Symbol<'a, DtwainsetpdfencryptionFunc>,
    DTWAIN_SetPDFEncryptionAFunc: Symbol<'a, DtwainsetpdfencryptionaFunc>,
    DTWAIN_SetPDFEncryptionWFunc: Symbol<'a, DtwainsetpdfencryptionwFunc>,
    DTWAIN_SetPDFJpegQualityFunc: Symbol<'a, DtwainsetpdfjpegqualityFunc>,
    DTWAIN_SetPDFKeywordsFunc: Symbol<'a, DtwainsetpdfkeywordsFunc>,
    DTWAIN_SetPDFKeywordsAFunc: Symbol<'a, DtwainsetpdfkeywordsaFunc>,
    DTWAIN_SetPDFKeywordsWFunc: Symbol<'a, DtwainsetpdfkeywordswFunc>,
    DTWAIN_SetPDFOCRConversionFunc: Symbol<'a, DtwainsetpdfocrconversionFunc>,
    DTWAIN_SetPDFOCRModeFunc: Symbol<'a, DtwainsetpdfocrmodeFunc>,
    DTWAIN_SetPDFOrientationFunc: Symbol<'a, DtwainsetpdforientationFunc>,
    DTWAIN_SetPDFPageScaleFunc: Symbol<'a, DtwainsetpdfpagescaleFunc>,
    DTWAIN_SetPDFPageScaleStringFunc: Symbol<'a, DtwainsetpdfpagescalestringFunc>,
    DTWAIN_SetPDFPageScaleStringAFunc: Symbol<'a, DtwainsetpdfpagescalestringaFunc>,
    DTWAIN_SetPDFPageScaleStringWFunc: Symbol<'a, DtwainsetpdfpagescalestringwFunc>,
    DTWAIN_SetPDFPageSizeFunc: Symbol<'a, DtwainsetpdfpagesizeFunc>,
    DTWAIN_SetPDFPageSizeStringFunc: Symbol<'a, DtwainsetpdfpagesizestringFunc>,
    DTWAIN_SetPDFPageSizeStringAFunc: Symbol<'a, DtwainsetpdfpagesizestringaFunc>,
    DTWAIN_SetPDFPageSizeStringWFunc: Symbol<'a, DtwainsetpdfpagesizestringwFunc>,
    DTWAIN_SetPDFPolarityFunc: Symbol<'a, DtwainsetpdfpolarityFunc>,
    DTWAIN_SetPDFProducerFunc: Symbol<'a, DtwainsetpdfproducerFunc>,
    DTWAIN_SetPDFProducerAFunc: Symbol<'a, DtwainsetpdfproduceraFunc>,
    DTWAIN_SetPDFProducerWFunc: Symbol<'a, DtwainsetpdfproducerwFunc>,
    DTWAIN_SetPDFSubjectFunc: Symbol<'a, DtwainsetpdfsubjectFunc>,
    DTWAIN_SetPDFSubjectAFunc: Symbol<'a, DtwainsetpdfsubjectaFunc>,
    DTWAIN_SetPDFSubjectWFunc: Symbol<'a, DtwainsetpdfsubjectwFunc>,
    DTWAIN_SetPDFTextElementFloatFunc: Symbol<'a, DtwainsetpdftextelementfloatFunc>,
    DTWAIN_SetPDFTextElementLongFunc: Symbol<'a, DtwainsetpdftextelementlongFunc>,
    DTWAIN_SetPDFTextElementStringFunc: Symbol<'a, DtwainsetpdftextelementstringFunc>,
    DTWAIN_SetPDFTextElementStringAFunc: Symbol<'a, DtwainsetpdftextelementstringaFunc>,
    DTWAIN_SetPDFTextElementStringWFunc: Symbol<'a, DtwainsetpdftextelementstringwFunc>,
    DTWAIN_SetPDFTitleFunc: Symbol<'a, DtwainsetpdftitleFunc>,
    DTWAIN_SetPDFTitleAFunc: Symbol<'a, DtwainsetpdftitleaFunc>,
    DTWAIN_SetPDFTitleWFunc: Symbol<'a, DtwainsetpdftitlewFunc>,
    DTWAIN_SetPaperSizeFunc: Symbol<'a, DtwainsetpapersizeFunc>,
    DTWAIN_SetPatchMaxPrioritiesFunc: Symbol<'a, DtwainsetpatchmaxprioritiesFunc>,
    DTWAIN_SetPatchMaxRetriesFunc: Symbol<'a, DtwainsetpatchmaxretriesFunc>,
    DTWAIN_SetPatchPrioritiesFunc: Symbol<'a, DtwainsetpatchprioritiesFunc>,
    DTWAIN_SetPatchSearchModeFunc: Symbol<'a, DtwainsetpatchsearchmodeFunc>,
    DTWAIN_SetPatchTimeOutFunc: Symbol<'a, DtwainsetpatchtimeoutFunc>,
    DTWAIN_SetPixelFlavorFunc: Symbol<'a, DtwainsetpixelflavorFunc>,
    DTWAIN_SetPixelTypeFunc: Symbol<'a, DtwainsetpixeltypeFunc>,
    DTWAIN_SetPostScriptTitleFunc: Symbol<'a, DtwainsetpostscripttitleFunc>,
    DTWAIN_SetPostScriptTitleAFunc: Symbol<'a, DtwainsetpostscripttitleaFunc>,
    DTWAIN_SetPostScriptTitleWFunc: Symbol<'a, DtwainsetpostscripttitlewFunc>,
    DTWAIN_SetPostScriptTypeFunc: Symbol<'a, DtwainsetpostscripttypeFunc>,
    DTWAIN_SetPrinterFunc: Symbol<'a, DtwainsetprinterFunc>,
    DTWAIN_SetPrinterExFunc: Symbol<'a, DtwainsetprinterexFunc>,
    DTWAIN_SetPrinterStartNumberFunc: Symbol<'a, DtwainsetprinterstartnumberFunc>,
    DTWAIN_SetPrinterStringModeFunc: Symbol<'a, DtwainsetprinterstringmodeFunc>,
    DTWAIN_SetPrinterStringsFunc: Symbol<'a, DtwainsetprinterstringsFunc>,
    DTWAIN_SetPrinterSuffixStringFunc: Symbol<'a, DtwainsetprintersuffixstringFunc>,
    DTWAIN_SetPrinterSuffixStringAFunc: Symbol<'a, DtwainsetprintersuffixstringaFunc>,
    DTWAIN_SetPrinterSuffixStringWFunc: Symbol<'a, DtwainsetprintersuffixstringwFunc>,
    DTWAIN_SetQueryCapSupportFunc: Symbol<'a, DtwainsetquerycapsupportFunc>,
    DTWAIN_SetResolutionFunc: Symbol<'a, DtwainsetresolutionFunc>,
    DTWAIN_SetResolutionStringFunc: Symbol<'a, DtwainsetresolutionstringFunc>,
    DTWAIN_SetResolutionStringAFunc: Symbol<'a, DtwainsetresolutionstringaFunc>,
    DTWAIN_SetResolutionStringWFunc: Symbol<'a, DtwainsetresolutionstringwFunc>,
    DTWAIN_SetResourcePathFunc: Symbol<'a, DtwainsetresourcepathFunc>,
    DTWAIN_SetResourcePathAFunc: Symbol<'a, DtwainsetresourcepathaFunc>,
    DTWAIN_SetResourcePathWFunc: Symbol<'a, DtwainsetresourcepathwFunc>,
    DTWAIN_SetRotationFunc: Symbol<'a, DtwainsetrotationFunc>,
    DTWAIN_SetRotationStringFunc: Symbol<'a, DtwainsetrotationstringFunc>,
    DTWAIN_SetRotationStringAFunc: Symbol<'a, DtwainsetrotationstringaFunc>,
    DTWAIN_SetRotationStringWFunc: Symbol<'a, DtwainsetrotationstringwFunc>,
    DTWAIN_SetSaveFileNameFunc: Symbol<'a, DtwainsetsavefilenameFunc>,
    DTWAIN_SetSaveFileNameAFunc: Symbol<'a, DtwainsetsavefilenameaFunc>,
    DTWAIN_SetSaveFileNameWFunc: Symbol<'a, DtwainsetsavefilenamewFunc>,
    DTWAIN_SetShadowFunc: Symbol<'a, DtwainsetshadowFunc>,
    DTWAIN_SetShadowStringFunc: Symbol<'a, DtwainsetshadowstringFunc>,
    DTWAIN_SetShadowStringAFunc: Symbol<'a, DtwainsetshadowstringaFunc>,
    DTWAIN_SetShadowStringWFunc: Symbol<'a, DtwainsetshadowstringwFunc>,
    DTWAIN_SetSourceUnitFunc: Symbol<'a, DtwainsetsourceunitFunc>,
    DTWAIN_SetTIFFCompressTypeFunc: Symbol<'a, DtwainsettiffcompresstypeFunc>,
    DTWAIN_SetTIFFInvertFunc: Symbol<'a, DtwainsettiffinvertFunc>,
    DTWAIN_SetTempFileDirectoryFunc: Symbol<'a, DtwainsettempfiledirectoryFunc>,
    DTWAIN_SetTempFileDirectoryAFunc: Symbol<'a, DtwainsettempfiledirectoryaFunc>,
    DTWAIN_SetTempFileDirectoryExFunc: Symbol<'a, DtwainsettempfiledirectoryexFunc>,
    DTWAIN_SetTempFileDirectoryExAFunc: Symbol<'a, DtwainsettempfiledirectoryexaFunc>,
    DTWAIN_SetTempFileDirectoryExWFunc: Symbol<'a, DtwainsettempfiledirectoryexwFunc>,
    DTWAIN_SetTempFileDirectoryWFunc: Symbol<'a, DtwainsettempfiledirectorywFunc>,
    DTWAIN_SetThresholdFunc: Symbol<'a, DtwainsetthresholdFunc>,
    DTWAIN_SetThresholdStringFunc: Symbol<'a, DtwainsetthresholdstringFunc>,
    DTWAIN_SetThresholdStringAFunc: Symbol<'a, DtwainsetthresholdstringaFunc>,
    DTWAIN_SetThresholdStringWFunc: Symbol<'a, DtwainsetthresholdstringwFunc>,
    DTWAIN_SetTwainDSMFunc: Symbol<'a, DtwainsettwaindsmFunc>,
    DTWAIN_SetTwainLogFunc: Symbol<'a, DtwainsettwainlogFunc>,
    DTWAIN_SetTwainLogAFunc: Symbol<'a, DtwainsettwainlogaFunc>,
    DTWAIN_SetTwainLogWFunc: Symbol<'a, DtwainsettwainlogwFunc>,
    DTWAIN_SetTwainModeFunc: Symbol<'a, DtwainsettwainmodeFunc>,
    DTWAIN_SetTwainTimeoutFunc: Symbol<'a, DtwainsettwaintimeoutFunc>,
    DTWAIN_SetUpdateDibProcFunc: Symbol<'a, DtwainsetupdatedibprocFunc>,
    DTWAIN_SetXResolutionFunc: Symbol<'a, DtwainsetxresolutionFunc>,
    DTWAIN_SetXResolutionStringFunc: Symbol<'a, DtwainsetxresolutionstringFunc>,
    DTWAIN_SetXResolutionStringAFunc: Symbol<'a, DtwainsetxresolutionstringaFunc>,
    DTWAIN_SetXResolutionStringWFunc: Symbol<'a, DtwainsetxresolutionstringwFunc>,
    DTWAIN_SetYResolutionFunc: Symbol<'a, DtwainsetyresolutionFunc>,
    DTWAIN_SetYResolutionStringFunc: Symbol<'a, DtwainsetyresolutionstringFunc>,
    DTWAIN_SetYResolutionStringAFunc: Symbol<'a, DtwainsetyresolutionstringaFunc>,
    DTWAIN_SetYResolutionStringWFunc: Symbol<'a, DtwainsetyresolutionstringwFunc>,
    DTWAIN_ShowUIOnlyFunc: Symbol<'a, DtwainshowuionlyFunc>,
    DTWAIN_ShutdownOCREngineFunc: Symbol<'a, DtwainshutdownocrengineFunc>,
    DTWAIN_SkipImageInfoErrorFunc: Symbol<'a, DtwainskipimageinfoerrorFunc>,
    DTWAIN_StartThreadFunc: Symbol<'a, DtwainstartthreadFunc>,
    DTWAIN_StartTwainSessionFunc: Symbol<'a, DtwainstarttwainsessionFunc>,
    DTWAIN_StartTwainSessionAFunc: Symbol<'a, DtwainstarttwainsessionaFunc>,
    DTWAIN_StartTwainSessionWFunc: Symbol<'a, DtwainstarttwainsessionwFunc>,
    DTWAIN_SysDestroyFunc: Symbol<'a, DtwainsysdestroyFunc>,
    DTWAIN_SysInitializeFunc: Symbol<'a, DtwainsysinitializeFunc>,
    DTWAIN_SysInitializeExFunc: Symbol<'a, DtwainsysinitializeexFunc>,
    DTWAIN_SysInitializeEx2Func: Symbol<'a, Dtwainsysinitializeex2Func>,
    DTWAIN_SysInitializeEx2AFunc: Symbol<'a, Dtwainsysinitializeex2aFunc>,
    DTWAIN_SysInitializeEx2WFunc: Symbol<'a, Dtwainsysinitializeex2wFunc>,
    DTWAIN_SysInitializeExAFunc: Symbol<'a, DtwainsysinitializeexaFunc>,
    DTWAIN_SysInitializeExWFunc: Symbol<'a, DtwainsysinitializeexwFunc>,
    DTWAIN_SysInitializeLibFunc: Symbol<'a, DtwainsysinitializelibFunc>,
    DTWAIN_SysInitializeLibExFunc: Symbol<'a, DtwainsysinitializelibexFunc>,
    DTWAIN_SysInitializeLibEx2Func: Symbol<'a, Dtwainsysinitializelibex2Func>,
    DTWAIN_SysInitializeLibEx2AFunc: Symbol<'a, Dtwainsysinitializelibex2aFunc>,
    DTWAIN_SysInitializeLibEx2WFunc: Symbol<'a, Dtwainsysinitializelibex2wFunc>,
    DTWAIN_SysInitializeLibExAFunc: Symbol<'a, DtwainsysinitializelibexaFunc>,
    DTWAIN_SysInitializeLibExWFunc: Symbol<'a, DtwainsysinitializelibexwFunc>,
    DTWAIN_SysInitializeNoBlockingFunc: Symbol<'a, DtwainsysinitializenoblockingFunc>,
    DTWAIN_TestGetCapFunc: Symbol<'a, DtwaintestgetcapFunc>,
    DTWAIN_UnlockMemoryFunc: Symbol<'a, DtwainunlockmemoryFunc>,
    DTWAIN_UnlockMemoryExFunc: Symbol<'a, DtwainunlockmemoryexFunc>,
    DTWAIN_UseMultipleThreadsFunc: Symbol<'a, DtwainusemultiplethreadsFunc>
}
impl<'a> DTwainAPI<'a>
{
    pub const DTWAIN_FF_TIFF: i32 = 0;
    pub const DTWAIN_FF_PICT: i32 = 1;
    pub const DTWAIN_FF_BMP: i32 = 2;
    pub const DTWAIN_FF_XBM: i32 = 3;
    pub const DTWAIN_FF_JFIF: i32 = 4;
    pub const DTWAIN_FF_FPX: i32 = 5;
    pub const DTWAIN_FF_TIFFMULTI: i32 = 6;
    pub const DTWAIN_FF_PNG: i32 = 7;
    pub const DTWAIN_FF_SPIFF: i32 = 8;
    pub const DTWAIN_FF_EXIF: i32 = 9;
    pub const DTWAIN_FF_PDF: i32 = 10;
    pub const DTWAIN_FF_JP2: i32 = 11;
    pub const DTWAIN_FF_JPX: i32 = 13;
    pub const DTWAIN_FF_DEJAVU: i32 = 14;
    pub const DTWAIN_FF_PDFA: i32 = 15;
    pub const DTWAIN_FF_PDFA2: i32 = 16;
    pub const DTWAIN_FF_PDFRASTER: i32 = 17;
    pub const DTWAIN_CP_NONE: i32 = 0;
    pub const DTWAIN_CP_PACKBITS: i32 = 1;
    pub const DTWAIN_CP_GROUP31D: i32 = 2;
    pub const DTWAIN_CP_GROUP31DEOL: i32 = 3;
    pub const DTWAIN_CP_GROUP32D: i32 = 4;
    pub const DTWAIN_CP_GROUP4: i32 = 5;
    pub const DTWAIN_CP_JPEG: i32 = 6;
    pub const DTWAIN_CP_LZW: i32 = 7;
    pub const DTWAIN_CP_JBIG: i32 = 8;
    pub const DTWAIN_CP_PNG: i32 = 9;
    pub const DTWAIN_CP_RLE4: i32 = 10;
    pub const DTWAIN_CP_RLE8: i32 = 11;
    pub const DTWAIN_CP_BITFIELDS: i32 = 12;
    pub const DTWAIN_CP_ZIP: i32 = 13;
    pub const DTWAIN_CP_JPEG2000: i32 = 14;
    pub const DTWAIN_FS_NONE: i32 = 0;
    pub const DTWAIN_FS_A4LETTER: i32 = 1;
    pub const DTWAIN_FS_B5LETTER: i32 = 2;
    pub const DTWAIN_FS_USLETTER: i32 = 3;
    pub const DTWAIN_FS_USLEGAL: i32 = 4;
    pub const DTWAIN_FS_A5: i32 = 5;
    pub const DTWAIN_FS_B4: i32 = 6;
    pub const DTWAIN_FS_B6: i32 = 7;
    pub const DTWAIN_FS_USLEDGER: i32 = 9;
    pub const DTWAIN_FS_USEXECUTIVE: i32 = 10;
    pub const DTWAIN_FS_A3: i32 = 11;
    pub const DTWAIN_FS_B3: i32 = 12;
    pub const DTWAIN_FS_A6: i32 = 13;
    pub const DTWAIN_FS_C4: i32 = 14;
    pub const DTWAIN_FS_C5: i32 = 15;
    pub const DTWAIN_FS_C6: i32 = 16;
    pub const DTWAIN_FS_4A0: i32 = 17;
    pub const DTWAIN_FS_2A0: i32 = 18;
    pub const DTWAIN_FS_A0: i32 = 19;
    pub const DTWAIN_FS_A1: i32 = 20;
    pub const DTWAIN_FS_A2: i32 = 21;
    pub const DTWAIN_FS_A4: i32 = DTwainAPI::DTWAIN_FS_A4LETTER;
    pub const DTWAIN_FS_A7: i32 = 22;
    pub const DTWAIN_FS_A8: i32 = 23;
    pub const DTWAIN_FS_A9: i32 = 24;
    pub const DTWAIN_FS_A10: i32 = 25;
    pub const DTWAIN_FS_ISOB0: i32 = 26;
    pub const DTWAIN_FS_ISOB1: i32 = 27;
    pub const DTWAIN_FS_ISOB2: i32 = 28;
    pub const DTWAIN_FS_ISOB3: i32 = DTwainAPI::DTWAIN_FS_B3;
    pub const DTWAIN_FS_ISOB4: i32 = DTwainAPI::DTWAIN_FS_B4;
    pub const DTWAIN_FS_ISOB5: i32 = 29;
    pub const DTWAIN_FS_ISOB6: i32 = DTwainAPI::DTWAIN_FS_B6;
    pub const DTWAIN_FS_ISOB7: i32 = 30;
    pub const DTWAIN_FS_ISOB8: i32 = 31;
    pub const DTWAIN_FS_ISOB9: i32 = 32;
    pub const DTWAIN_FS_ISOB10: i32 = 33;
    pub const DTWAIN_FS_JISB0: i32 = 34;
    pub const DTWAIN_FS_JISB1: i32 = 35;
    pub const DTWAIN_FS_JISB2: i32 = 36;
    pub const DTWAIN_FS_JISB3: i32 = 37;
    pub const DTWAIN_FS_JISB4: i32 = 38;
    pub const DTWAIN_FS_JISB5: i32 = DTwainAPI::DTWAIN_FS_B5LETTER;
    pub const DTWAIN_FS_JISB6: i32 = 39;
    pub const DTWAIN_FS_JISB7: i32 = 40;
    pub const DTWAIN_FS_JISB8: i32 = 41;
    pub const DTWAIN_FS_JISB9: i32 = 42;
    pub const DTWAIN_FS_JISB10: i32 = 43;
    pub const DTWAIN_FS_C0: i32 = 44;
    pub const DTWAIN_FS_C1: i32 = 45;
    pub const DTWAIN_FS_C2: i32 = 46;
    pub const DTWAIN_FS_C3: i32 = 47;
    pub const DTWAIN_FS_C7: i32 = 48;
    pub const DTWAIN_FS_C8: i32 = 49;
    pub const DTWAIN_FS_C9: i32 = 50;
    pub const DTWAIN_FS_C10: i32 = 51;
    pub const DTWAIN_FS_USSTATEMENT: i32 = 52;
    pub const DTWAIN_FS_BUSINESSCARD: i32 = 53;
    pub const DTWAIN_ANYSUPPORT: i32 = -1;
    pub const DTWAIN_BMP: i32 = 100;
    pub const DTWAIN_JPEG: i32 = 200;
    pub const DTWAIN_PDF: i32 = 250;
    pub const DTWAIN_PDFMULTI: i32 = 251;
    pub const DTWAIN_PCX: i32 = 300;
    pub const DTWAIN_DCX: i32 = 301;
    pub const DTWAIN_TGA: i32 = 400;
    pub const DTWAIN_TIFFLZW: i32 = 500;
    pub const DTWAIN_TIFFNONE: i32 = 600;
    pub const DTWAIN_TIFFG3: i32 = 700;
    pub const DTWAIN_TIFFG4: i32 = 800;
    pub const DTWAIN_TIFFPACKBITS: i32 = 801;
    pub const DTWAIN_TIFFDEFLATE: i32 = 802;
    pub const DTWAIN_TIFFJPEG: i32 = 803;
    pub const DTWAIN_TIFFJBIG: i32 = 804;
    pub const DTWAIN_TIFFPIXARLOG: i32 = 805;
    pub const DTWAIN_TIFFNONEMULTI: i32 = 900;
    pub const DTWAIN_TIFFG3MULTI: i32 = 901;
    pub const DTWAIN_TIFFG4MULTI: i32 = 902;
    pub const DTWAIN_TIFFPACKBITSMULTI: i32 = 903;
    pub const DTWAIN_TIFFDEFLATEMULTI: i32 = 904;
    pub const DTWAIN_TIFFJPEGMULTI: i32 = 905;
    pub const DTWAIN_TIFFLZWMULTI: i32 = 906;
    pub const DTWAIN_TIFFJBIGMULTI: i32 = 907;
    pub const DTWAIN_TIFFPIXARLOGMULTI: i32 = 908;
    pub const DTWAIN_WMF: i32 = 850;
    pub const DTWAIN_EMF: i32 = 851;
    pub const DTWAIN_GIF: i32 = 950;
    pub const DTWAIN_PNG: i32 = 1000;
    pub const DTWAIN_PSD: i32 = 2000;
    pub const DTWAIN_JPEG2000: i32 = 3000;
    pub const DTWAIN_POSTSCRIPT1: i32 = 4000;
    pub const DTWAIN_POSTSCRIPT2: i32 = 4001;
    pub const DTWAIN_POSTSCRIPT3: i32 = 4002;
    pub const DTWAIN_POSTSCRIPT1MULTI: i32 = 4003;
    pub const DTWAIN_POSTSCRIPT2MULTI: i32 = 4004;
    pub const DTWAIN_POSTSCRIPT3MULTI: i32 = 4005;
    pub const DTWAIN_TEXT: i32 = 6000;
    pub const DTWAIN_TEXTMULTI: i32 = 6001;
    pub const DTWAIN_TIFFMULTI: i32 = 7000;
    pub const DTWAIN_ICO: i32 = 8000;
    pub const DTWAIN_ICO_VISTA: i32 = 8001;
    pub const DTWAIN_ICO_RESIZED: i32 = 8002;
    pub const DTWAIN_WBMP: i32 = 8500;
    pub const DTWAIN_WEBP: i32 = 8501;
    pub const DTWAIN_PPM: i32 = 10000;
    pub const DTWAIN_WBMP_RESIZED: i32 = 11000;
    pub const DTWAIN_TGA_RLE: i32 = 11001;
    pub const DTWAIN_BMP_RLE: i32 = 11002;
    pub const DTWAIN_BIGTIFFLZW: i32 = 11003;
    pub const DTWAIN_BIGTIFFLZWMULTI: i32 = 11004;
    pub const DTWAIN_BIGTIFFNONE: i32 = 11005;
    pub const DTWAIN_BIGTIFFNONEMULTI: i32 = 11006;
    pub const DTWAIN_BIGTIFFPACKBITS: i32 = 11007;
    pub const DTWAIN_BIGTIFFPACKBITSMULTI: i32 = 11008;
    pub const DTWAIN_BIGTIFFDEFLATE: i32 = 11009;
    pub const DTWAIN_BIGTIFFDEFLATEMULTI: i32 = 11010;
    pub const DTWAIN_BIGTIFFG3: i32 = 11011;
    pub const DTWAIN_BIGTIFFG3MULTI: i32 = 11012;
    pub const DTWAIN_BIGTIFFG4: i32 = 11013;
    pub const DTWAIN_BIGTIFFG4MULTI: i32 = 11014;
    pub const DTWAIN_BIGTIFFJPEG: i32 = 11015;
    pub const DTWAIN_BIGTIFFJPEGMULTI: i32 = 11016;
    pub const DTWAIN_JPEGXR: i32 = 12000;
    pub const DTWAIN_INCHES: i32 = 0;
    pub const DTWAIN_CENTIMETERS: i32 = 1;
    pub const DTWAIN_PICAS: i32 = 2;
    pub const DTWAIN_POINTS: i32 = 3;
    pub const DTWAIN_TWIPS: i32 = 4;
    pub const DTWAIN_PIXELS: i32 = 5;
    pub const DTWAIN_MILLIMETERS: i32 = 6;
    pub const DTWAIN_USENAME: i32 = 16;
    pub const DTWAIN_USEPROMPT: i32 = 32;
    pub const DTWAIN_USELONGNAME: i32 = 64;
    pub const DTWAIN_USESOURCEMODE: i32 = 128;
    pub const DTWAIN_USELIST: i32 = 256;
    pub const DTWAIN_CREATE_DIRECTORY: i32 = 512;
    pub const DTWAIN_CREATEDIRECTORY: i32 = DTwainAPI::DTWAIN_CREATE_DIRECTORY;
    pub const DTWAIN_ARRAYANY: i32 = 1;
    pub const DTWAIN_ArrayTypePTR: i32 = 1;
    pub const DTWAIN_ARRAYLONG: i32 = 2;
    pub const DTWAIN_ARRAYFLOAT: i32 = 3;
    pub const DTWAIN_ARRAYHANDLE: i32 = 4;
    pub const DTWAIN_ARRAYSOURCE: i32 = 5;
    pub const DTWAIN_ARRAYSTRING: i32 = 6;
    pub const DTWAIN_ARRAYFRAME: i32 = 7;
    pub const DTWAIN_ARRAYBOOL: i32 = DTwainAPI::DTWAIN_ARRAYLONG;
    pub const DTWAIN_ARRAYLONGSTRING: i32 = 8;
    pub const DTWAIN_ARRAYUNICODESTRING: i32 = 9;
    pub const DTWAIN_ARRAYLONG64: i32 = 10;
    pub const DTWAIN_ARRAYANSISTRING: i32 = 11;
    pub const DTWAIN_ARRAYWIDESTRING: i32 = 12;
    pub const DTWAIN_ARRAYTWFIX32: i32 = 200;
    pub const DTWAIN_ArrayTypeINVALID: i32 = 0;
    pub const DTWAIN_ARRAYINT16: i32 = 100;
    pub const DTWAIN_ARRAYUINT16: i32 = 110;
    pub const DTWAIN_ARRAYUINT32: i32 = 120;
    pub const DTWAIN_ARRAYINT32: i32 = 130;
    pub const DTWAIN_ARRAYINT64: i32 = 140;
    pub const DTWAIN_ARRAYUINT64: i32 = 150;
    pub const DTWAIN_RANGELONG: i32 = DTwainAPI::DTWAIN_ARRAYLONG;
    pub const DTWAIN_RANGEFLOAT: i32 = DTwainAPI::DTWAIN_ARRAYFLOAT;
    pub const DTWAIN_RANGEMIN: i32 = 0;
    pub const DTWAIN_RANGEMAX: i32 = 1;
    pub const DTWAIN_RANGESTEP: i32 = 2;
    pub const DTWAIN_RANGEDEFAULT: i32 = 3;
    pub const DTWAIN_RANGECURRENT: i32 = 4;
    pub const DTWAIN_FRAMELEFT: i32 = 0;
    pub const DTWAIN_FRAMETOP: i32 = 1;
    pub const DTWAIN_FRAMERIGHT: i32 = 2;
    pub const DTWAIN_FRAMEBOTTOM: i32 = 3;
    pub const DTWAIN_FIX32WHOLE: i32 = 0;
    pub const DTWAIN_FIX32FRAC: i32 = 1;
    pub const DTWAIN_JC_NONE: i32 = 0;
    pub const DTWAIN_JC_JSIC: i32 = 1;
    pub const DTWAIN_JC_JSIS: i32 = 2;
    pub const DTWAIN_JC_JSXC: i32 = 3;
    pub const DTWAIN_JC_JSXS: i32 = 4;
    pub const DTWAIN_CAPDATATYPE_UNKNOWN: i32 = -10;
    pub const DTWAIN_JCBP_JSIC: i32 = 5;
    pub const DTWAIN_JCBP_JSIS: i32 = 6;
    pub const DTWAIN_JCBP_JSXC: i32 = 7;
    pub const DTWAIN_JCBP_JSXS: i32 = 8;
    pub const DTWAIN_FEEDPAGEON: i32 = 1;
    pub const DTWAIN_CLEARPAGEON: i32 = 2;
    pub const DTWAIN_REWINDPAGEON: i32 = 4;
    pub const DTWAIN_AppOwnsDib: i32 = 1;
    pub const DTWAIN_SourceOwnsDib: i32 = 2;
    pub const DTWAIN_CONTARRAY: i32 = 8;
    pub const DTWAIN_CONTENUMERATION: i32 = 16;
    pub const DTWAIN_CONTONEVALUE: i32 = 32;
    pub const DTWAIN_CONTRANGE: i32 = 64;
    pub const DTWAIN_CONTDEFAULT: i32 = 0;
    pub const DTWAIN_CAPGET: i32 = 1;
    pub const DTWAIN_CAPGETCURRENT: i32 = 2;
    pub const DTWAIN_CAPGETDEFAULT: i32 = 3;
    pub const DTWAIN_CAPSET: i32 = 6;
    pub const DTWAIN_CAPRESET: i32 = 7;
    pub const DTWAIN_CAPRESETALL: i32 = 8;
    pub const DTWAIN_CAPSETCONSTRAINT: i32 = 9;
    pub const DTWAIN_CAPSETAVAILABLE: i32 = 8;
    pub const DTWAIN_CAPSETCURRENT: i32 = 16;
    pub const DTWAIN_CAPGETHELP: i32 = 9;
    pub const DTWAIN_CAPGETLABEL: i32 = 10;
    pub const DTWAIN_CAPGETLABELENUM: i32 = 11;
    pub const DTWAIN_AREASET: i32 = DTwainAPI::DTWAIN_CAPSET;
    pub const DTWAIN_AREARESET: i32 = DTwainAPI::DTWAIN_CAPRESET;
    pub const DTWAIN_AREACURRENT: i32 = DTwainAPI::DTWAIN_CAPGETCURRENT;
    pub const DTWAIN_AREADEFAULT: i32 = DTwainAPI::DTWAIN_CAPGETDEFAULT;
    pub const DTWAIN_VER15: i32 = 0;
    pub const DTWAIN_VER16: i32 = 1;
    pub const DTWAIN_VER17: i32 = 2;
    pub const DTWAIN_VER18: i32 = 3;
    pub const DTWAIN_VER20: i32 = 4;
    pub const DTWAIN_VER21: i32 = 5;
    pub const DTWAIN_VER22: i32 = 6;
    pub const DTWAIN_ACQUIREALL: i32 = -1;
    pub const DTWAIN_MAXACQUIRE: i32 = -1;
    pub const DTWAIN_DX_NONE: i32 = 0;
    pub const DTWAIN_DX_1PASSDUPLEX: i32 = 1;
    pub const DTWAIN_DX_2PASSDUPLEX: i32 = 2;
    pub const DTWAIN_PT_BW: i32 = 0;
    pub const DTWAIN_PT_GRAY: i32 = 1;
    pub const DTWAIN_PT_RGB: i32 = 2;
    pub const DTWAIN_PT_PALETTE: i32 = 3;
    pub const DTWAIN_PT_CMY: i32 = 4;
    pub const DTWAIN_PT_CMYK: i32 = 5;
    pub const DTWAIN_PT_YUV: i32 = 6;
    pub const DTWAIN_PT_YUVK: i32 = 7;
    pub const DTWAIN_PT_CIEXYZ: i32 = 8;
    pub const DTWAIN_PT_DEFAULT: i32 = 1000;
    pub const DTWAIN_CURRENT: i32 = -2;
    pub const DTWAIN_DEFAULT: i32 = -1;
    pub const DTWAIN_FLOATDEFAULT: f64 = -9999.0;
    pub const DTWAIN_CallbackERROR: i32 = 1;
    pub const DTWAIN_CallbackMESSAGE: i32 = 2;
    pub const DTWAIN_USENATIVE: i32 = 1;
    pub const DTWAIN_USEBUFFERED: i32 = 2;
    pub const DTWAIN_USECOMPRESSION: i32 = 4;
    pub const DTWAIN_USEMEMFILE: i32 = 8;
    pub const DTWAIN_FAILURE1: i32 = -1;
    pub const DTWAIN_FAILURE2: i32 = -2;
    pub const DTWAIN_DELETEALL: i32 = -1;
    pub const DTWAIN_TN_ACQUIREDONE: i32 = 1000;
    pub const DTWAIN_TN_ACQUIREFAILED: i32 = 1001;
    pub const DTWAIN_TN_ACQUIRECANCELLED: i32 = 1002;
    pub const DTWAIN_TN_ACQUIRESTARTED: i32 = 1003;
    pub const DTWAIN_TN_PAGECONTINUE: i32 = 1004;
    pub const DTWAIN_TN_PAGEFAILED: i32 = 1005;
    pub const DTWAIN_TN_PAGECANCELLED: i32 = 1006;
    pub const DTWAIN_TN_TRANSFERREADY: i32 = 1009;
    pub const DTWAIN_TN_TRANSFERDONE: i32 = 1010;
    pub const DTWAIN_TN_ACQUIREPAGEDONE: i32 = 1010;
    pub const DTWAIN_TN_UICLOSING: i32 = 3000;
    pub const DTWAIN_TN_UICLOSED: i32 = 3001;
    pub const DTWAIN_TN_UIOPENED: i32 = 3002;
    pub const DTWAIN_TN_UIOPENING: i32 = 3003;
    pub const DTWAIN_TN_UIOPENFAILURE: i32 = 3004;
    pub const DTWAIN_TN_CLIPTRANSFERDONE: i32 = 1014;
    pub const DTWAIN_TN_INVALIDIMAGEFORMAT: i32 = 1015;
    pub const DTWAIN_TN_ACQUIRETERMINATED: i32 = 1021;
    pub const DTWAIN_TN_TRANSFERSTRIPREADY: i32 = 1022;
    pub const DTWAIN_TN_TRANSFERSTRIPDONE: i32 = 1023;
    pub const DTWAIN_TN_TRANSFERSTRIPFAILED: i32 = 1029;
    pub const DTWAIN_TN_IMAGEINFOERROR: i32 = 1024;
    pub const DTWAIN_TN_TRANSFERCANCELLED: i32 = 1030;
    pub const DTWAIN_TN_FILESAVECANCELLED: i32 = 1031;
    pub const DTWAIN_TN_FILESAVEOK: i32 = 1032;
    pub const DTWAIN_TN_FILESAVEERROR: i32 = 1033;
    pub const DTWAIN_TN_FILEPAGESAVEOK: i32 = 1034;
    pub const DTWAIN_TN_FILEPAGESAVEERROR: i32 = 1035;
    pub const DTWAIN_TN_PROCESSEDDIB: i32 = 1036;
    pub const DTWAIN_TN_FEEDERLOADED: i32 = 1037;
    pub const DTWAIN_TN_GENERALERROR: i32 = 1038;
    pub const DTWAIN_TN_MANDUPFLIPPAGES: i32 = 1040;
    pub const DTWAIN_TN_MANDUPSIDE1DONE: i32 = 1041;
    pub const DTWAIN_TN_MANDUPSIDE2DONE: i32 = 1042;
    pub const DTWAIN_TN_MANDUPPAGECOUNTERROR: i32 = 1043;
    pub const DTWAIN_TN_MANDUPACQUIREDONE: i32 = 1044;
    pub const DTWAIN_TN_MANDUPSIDE1START: i32 = 1045;
    pub const DTWAIN_TN_MANDUPSIDE2START: i32 = 1046;
    pub const DTWAIN_TN_MANDUPMERGEERROR: i32 = 1047;
    pub const DTWAIN_TN_MANDUPMEMORYERROR: i32 = 1048;
    pub const DTWAIN_TN_MANDUPFILEERROR: i32 = 1049;
    pub const DTWAIN_TN_MANDUPFILESAVEERROR: i32 = 1050;
    pub const DTWAIN_TN_ENDOFJOBDETECTED: i32 = 1051;
    pub const DTWAIN_TN_EOJDETECTED: i32 = 1051;
    pub const DTWAIN_TN_EOJDETECTED_XFERDONE: i32 = 1052;
    pub const DTWAIN_TN_QUERYPAGEDISCARD: i32 = 1053;
    pub const DTWAIN_TN_PAGEDISCARDED: i32 = 1054;
    pub const DTWAIN_TN_PROCESSDIBACCEPTED: i32 = 1055;
    pub const DTWAIN_TN_PROCESSDIBFINALACCEPTED: i32 = 1056;
    pub const DTWAIN_TN_CLOSEDIBFAILED: i32 = 1057;
    pub const DTWAIN_TN_INVALID_TWAINDSM2_BITMAP: i32 = 1058;
    pub const DTWAIN_TN_IMAGE_RESAMPLE_FAILURE: i32 = 1059;
    pub const DTWAIN_TN_DEVICEEVENT: i32 = 1100;
    pub const DTWAIN_TN_TWAINPAGECANCELLED: i32 = 1105;
    pub const DTWAIN_TN_TWAINPAGEFAILED: i32 = 1106;
    pub const DTWAIN_TN_APPUPDATEDDIB: i32 = 1107;
    pub const DTWAIN_TN_FILEPAGESAVING: i32 = 1110;
    pub const DTWAIN_TN_EOJBEGINFILESAVE: i32 = 1112;
    pub const DTWAIN_TN_EOJENDFILESAVE: i32 = 1113;
    pub const DTWAIN_TN_CROPFAILED: i32 = 1120;
    pub const DTWAIN_TN_PROCESSEDDIBFINAL: i32 = 1121;
    pub const DTWAIN_TN_BLANKPAGEDETECTED1: i32 = 1130;
    pub const DTWAIN_TN_BLANKPAGEDETECTED2: i32 = 1131;
    pub const DTWAIN_TN_BLANKPAGEDETECTED3: i32 = 1132;
    pub const DTWAIN_TN_BLANKPAGEDISCARDED1: i32 = 1133;
    pub const DTWAIN_TN_BLANKPAGEDISCARDED2: i32 = 1134;
    pub const DTWAIN_TN_OCRTEXTRETRIEVED: i32 = 1140;
    pub const DTWAIN_TN_QUERYOCRTEXT: i32 = 1141;
    pub const DTWAIN_TN_PDFOCRREADY: i32 = 1142;
    pub const DTWAIN_TN_PDFOCRDONE: i32 = 1143;
    pub const DTWAIN_TN_PDFOCRERROR: i32 = 1144;
    pub const DTWAIN_TN_SETCALLBACKINIT: i32 = 1150;
    pub const DTWAIN_TN_SETCALLBACK64INIT: i32 = 1151;
    pub const DTWAIN_TN_FILENAMECHANGING: i32 = 1160;
    pub const DTWAIN_TN_FILENAMECHANGED: i32 = 1161;
    pub const DTWAIN_TN_PROCESSEDAUDIOFINAL: i32 = 1180;
    pub const DTWAIN_TN_PROCESSAUDIOFINALACCEPTED: i32 = 1181;
    pub const DTWAIN_TN_PROCESSEDAUDIOFILE: i32 = 1182;
    pub const DTWAIN_TN_TWAINTRIPLETBEGIN: i32 = 1183;
    pub const DTWAIN_TN_TWAINTRIPLETEND: i32 = 1184;
    pub const DTWAIN_TN_FEEDERNOTLOADED: i32 = 1201;
    pub const DTWAIN_TN_FEEDERTIMEOUT: i32 = 1202;
    pub const DTWAIN_TN_FEEDERNOTENABLED: i32 = 1203;
    pub const DTWAIN_TN_FEEDERNOTSUPPORTED: i32 = 1204;
    pub const DTWAIN_TN_FEEDERTOFLATBED: i32 = 1205;
    pub const DTWAIN_TN_PREACQUIRESTART: i32 = 1206;
    pub const DTWAIN_TN_TRANSFERTILEREADY: i32 = 1300;
    pub const DTWAIN_TN_TRANSFERTILEDONE: i32 = 1301;
    pub const DTWAIN_TN_FILECOMPRESSTYPEMISMATCH: i32 = 1302;
    pub const DTWAIN_PDFOCR_CLEANTEXT1: i32 = 1;
    pub const DTWAIN_PDFOCR_CLEANTEXT2: i32 = 2;
    pub const DTWAIN_MODAL: i32 = 0;
    pub const DTWAIN_MODELESS: i32 = 1;
    pub const DTWAIN_UIModeCLOSE: i32 = 0;
    pub const DTWAIN_UIModeOPEN: i32 = 1;
    pub const DTWAIN_REOPEN_SOURCE: i32 = 2;
    pub const DTWAIN_ROUNDNEAREST: i32 = 0;
    pub const DTWAIN_ROUNDUP: i32 = 1;
    pub const DTWAIN_ROUNDDOWN: i32 = 2;
    pub const DTWAIN_FLOATDELTA: f64 = 1.0e-8;
    pub const DTWAIN_OR_ROT0: i32 = 0;
    pub const DTWAIN_OR_ROT90: i32 = 1;
    pub const DTWAIN_OR_ROT180: i32 = 2;
    pub const DTWAIN_OR_ROT270: i32 = 3;
    pub const DTWAIN_OR_PORTRAIT: i32 = DTwainAPI::DTWAIN_OR_ROT0;
    pub const DTWAIN_OR_LANDSCAPE: i32 = DTwainAPI::DTWAIN_OR_ROT270;
    pub const DTWAIN_OR_ANYROTATION: i32 = -1;
    pub const DTWAIN_CO_GET: i32 = 0x0001;
    pub const DTWAIN_CO_SET: i32 = 0x0002;
    pub const DTWAIN_CO_GETDEFAULT: i32 = 0x0004;
    pub const DTWAIN_CO_GETCURRENT: i32 = 0x0008;
    pub const DTWAIN_CO_RESET: i32 = 0x0010;
    pub const DTWAIN_CO_SETCONSTRAINT: i32 = 0x0020;
    pub const DTWAIN_CO_CONSTRAINABLE: i32 = 0x0040;
    pub const DTWAIN_CO_GETHELP: i32 = 0x0100;
    pub const DTWAIN_CO_GETLABEL: i32 = 0x0200;
    pub const DTWAIN_CO_GETLABELENUM: i32 = 0x0400;
    pub const DTWAIN_CNTYAFGHANISTAN: i32 = 1001;
    pub const DTWAIN_CNTYALGERIA: i32 = 213;
    pub const DTWAIN_CNTYAMERICANSAMOA: i32 = 684;
    pub const DTWAIN_CNTYANDORRA: i32 = 33;
    pub const DTWAIN_CNTYANGOLA: i32 = 1002;
    pub const DTWAIN_CNTYANGUILLA: i32 = 8090;
    pub const DTWAIN_CNTYANTIGUA: i32 = 8091;
    pub const DTWAIN_CNTYARGENTINA: i32 = 54;
    pub const DTWAIN_CNTYARUBA: i32 = 297;
    pub const DTWAIN_CNTYASCENSIONI: i32 = 247;
    pub const DTWAIN_CNTYAUSTRALIA: i32 = 61;
    pub const DTWAIN_CNTYAUSTRIA: i32 = 43;
    pub const DTWAIN_CNTYBAHAMAS: i32 = 8092;
    pub const DTWAIN_CNTYBAHRAIN: i32 = 973;
    pub const DTWAIN_CNTYBANGLADESH: i32 = 880;
    pub const DTWAIN_CNTYBARBADOS: i32 = 8093;
    pub const DTWAIN_CNTYBELGIUM: i32 = 32;
    pub const DTWAIN_CNTYBELIZE: i32 = 501;
    pub const DTWAIN_CNTYBENIN: i32 = 229;
    pub const DTWAIN_CNTYBERMUDA: i32 = 8094;
    pub const DTWAIN_CNTYBHUTAN: i32 = 1003;
    pub const DTWAIN_CNTYBOLIVIA: i32 = 591;
    pub const DTWAIN_CNTYBOTSWANA: i32 = 267;
    pub const DTWAIN_CNTYBRITAIN: i32 = 6;
    pub const DTWAIN_CNTYBRITVIRGINIS: i32 = 8095;
    pub const DTWAIN_CNTYBRAZIL: i32 = 55;
    pub const DTWAIN_CNTYBRUNEI: i32 = 673;
    pub const DTWAIN_CNTYBULGARIA: i32 = 359;
    pub const DTWAIN_CNTYBURKINAFASO: i32 = 1004;
    pub const DTWAIN_CNTYBURMA: i32 = 1005;
    pub const DTWAIN_CNTYBURUNDI: i32 = 1006;
    pub const DTWAIN_CNTYCAMAROON: i32 = 237;
    pub const DTWAIN_CNTYCANADA: i32 = 2;
    pub const DTWAIN_CNTYCAPEVERDEIS: i32 = 238;
    pub const DTWAIN_CNTYCAYMANIS: i32 = 8096;
    pub const DTWAIN_CNTYCENTRALAFREP: i32 = 1007;
    pub const DTWAIN_CNTYCHAD: i32 = 1008;
    pub const DTWAIN_CNTYCHILE: i32 = 56;
    pub const DTWAIN_CNTYCHINA: i32 = 86;
    pub const DTWAIN_CNTYCHRISTMASIS: i32 = 1009;
    pub const DTWAIN_CNTYCOCOSIS: i32 = 1009;
    pub const DTWAIN_CNTYCOLOMBIA: i32 = 57;
    pub const DTWAIN_CNTYCOMOROS: i32 = 1010;
    pub const DTWAIN_CNTYCONGO: i32 = 1011;
    pub const DTWAIN_CNTYCOOKIS: i32 = 1012;
    pub const DTWAIN_CNTYCOSTARICA: i32 = 506;
    pub const DTWAIN_CNTYCUBA: i32 = 5;
    pub const DTWAIN_CNTYCYPRUS: i32 = 357;
    pub const DTWAIN_CNTYCZECHOSLOVAKIA: i32 = 42;
    pub const DTWAIN_CNTYDENMARK: i32 = 45;
    pub const DTWAIN_CNTYDJIBOUTI: i32 = 1013;
    pub const DTWAIN_CNTYDOMINICA: i32 = 8097;
    pub const DTWAIN_CNTYDOMINCANREP: i32 = 8098;
    pub const DTWAIN_CNTYEASTERIS: i32 = 1014;
    pub const DTWAIN_CNTYECUADOR: i32 = 593;
    pub const DTWAIN_CNTYEGYPT: i32 = 20;
    pub const DTWAIN_CNTYELSALVADOR: i32 = 503;
    pub const DTWAIN_CNTYEQGUINEA: i32 = 1015;
    pub const DTWAIN_CNTYETHIOPIA: i32 = 251;
    pub const DTWAIN_CNTYFALKLANDIS: i32 = 1016;
    pub const DTWAIN_CNTYFAEROEIS: i32 = 298;
    pub const DTWAIN_CNTYFIJIISLANDS: i32 = 679;
    pub const DTWAIN_CNTYFINLAND: i32 = 358;
    pub const DTWAIN_CNTYFRANCE: i32 = 33;
    pub const DTWAIN_CNTYFRANTILLES: i32 = 596;
    pub const DTWAIN_CNTYFRGUIANA: i32 = 594;
    pub const DTWAIN_CNTYFRPOLYNEISA: i32 = 689;
    pub const DTWAIN_CNTYFUTANAIS: i32 = 1043;
    pub const DTWAIN_CNTYGABON: i32 = 241;
    pub const DTWAIN_CNTYGAMBIA: i32 = 220;
    pub const DTWAIN_CNTYGERMANY: i32 = 49;
    pub const DTWAIN_CNTYGHANA: i32 = 233;
    pub const DTWAIN_CNTYGIBRALTER: i32 = 350;
    pub const DTWAIN_CNTYGREECE: i32 = 30;
    pub const DTWAIN_CNTYGREENLAND: i32 = 299;
    pub const DTWAIN_CNTYGRENADA: i32 = 8099;
    pub const DTWAIN_CNTYGRENEDINES: i32 = 8015;
    pub const DTWAIN_CNTYGUADELOUPE: i32 = 590;
    pub const DTWAIN_CNTYGUAM: i32 = 671;
    pub const DTWAIN_CNTYGUANTANAMOBAY: i32 = 5399;
    pub const DTWAIN_CNTYGUATEMALA: i32 = 502;
    pub const DTWAIN_CNTYGUINEA: i32 = 224;
    pub const DTWAIN_CNTYGUINEABISSAU: i32 = 1017;
    pub const DTWAIN_CNTYGUYANA: i32 = 592;
    pub const DTWAIN_CNTYHAITI: i32 = 509;
    pub const DTWAIN_CNTYHONDURAS: i32 = 504;
    pub const DTWAIN_CNTYHONGKONG: i32 = 852;
    pub const DTWAIN_CNTYHUNGARY: i32 = 36;
    pub const DTWAIN_CNTYICELAND: i32 = 354;
    pub const DTWAIN_CNTYINDIA: i32 = 91;
    pub const DTWAIN_CNTYINDONESIA: i32 = 62;
    pub const DTWAIN_CNTYIRAN: i32 = 98;
    pub const DTWAIN_CNTYIRAQ: i32 = 964;
    pub const DTWAIN_CNTYIRELAND: i32 = 353;
    pub const DTWAIN_CNTYISRAEL: i32 = 972;
    pub const DTWAIN_CNTYITALY: i32 = 39;
    pub const DTWAIN_CNTYIVORYCOAST: i32 = 225;
    pub const DTWAIN_CNTYJAMAICA: i32 = 8010;
    pub const DTWAIN_CNTYJAPAN: i32 = 81;
    pub const DTWAIN_CNTYJORDAN: i32 = 962;
    pub const DTWAIN_CNTYKENYA: i32 = 254;
    pub const DTWAIN_CNTYKIRIBATI: i32 = 1018;
    pub const DTWAIN_CNTYKOREA: i32 = 82;
    pub const DTWAIN_CNTYKUWAIT: i32 = 965;
    pub const DTWAIN_CNTYLAOS: i32 = 1019;
    pub const DTWAIN_CNTYLEBANON: i32 = 1020;
    pub const DTWAIN_CNTYLIBERIA: i32 = 231;
    pub const DTWAIN_CNTYLIBYA: i32 = 218;
    pub const DTWAIN_CNTYLIECHTENSTEIN: i32 = 41;
    pub const DTWAIN_CNTYLUXENBOURG: i32 = 352;
    pub const DTWAIN_CNTYMACAO: i32 = 853;
    pub const DTWAIN_CNTYMADAGASCAR: i32 = 1021;
    pub const DTWAIN_CNTYMALAWI: i32 = 265;
    pub const DTWAIN_CNTYMALAYSIA: i32 = 60;
    pub const DTWAIN_CNTYMALDIVES: i32 = 960;
    pub const DTWAIN_CNTYMALI: i32 = 1022;
    pub const DTWAIN_CNTYMALTA: i32 = 356;
    pub const DTWAIN_CNTYMARSHALLIS: i32 = 692;
    pub const DTWAIN_CNTYMAURITANIA: i32 = 1023;
    pub const DTWAIN_CNTYMAURITIUS: i32 = 230;
    pub const DTWAIN_CNTYMEXICO: i32 = 3;
    pub const DTWAIN_CNTYMICRONESIA: i32 = 691;
    pub const DTWAIN_CNTYMIQUELON: i32 = 508;
    pub const DTWAIN_CNTYMONACO: i32 = 33;
    pub const DTWAIN_CNTYMONGOLIA: i32 = 1024;
    pub const DTWAIN_CNTYMONTSERRAT: i32 = 8011;
    pub const DTWAIN_CNTYMOROCCO: i32 = 212;
    pub const DTWAIN_CNTYMOZAMBIQUE: i32 = 1025;
    pub const DTWAIN_CNTYNAMIBIA: i32 = 264;
    pub const DTWAIN_CNTYNAURU: i32 = 1026;
    pub const DTWAIN_CNTYNEPAL: i32 = 977;
    pub const DTWAIN_CNTYNETHERLANDS: i32 = 31;
    pub const DTWAIN_CNTYNETHANTILLES: i32 = 599;
    pub const DTWAIN_CNTYNEVIS: i32 = 8012;
    pub const DTWAIN_CNTYNEWCALEDONIA: i32 = 687;
    pub const DTWAIN_CNTYNEWZEALAND: i32 = 64;
    pub const DTWAIN_CNTYNICARAGUA: i32 = 505;
    pub const DTWAIN_CNTYNIGER: i32 = 227;
    pub const DTWAIN_CNTYNIGERIA: i32 = 234;
    pub const DTWAIN_CNTYNIUE: i32 = 1027;
    pub const DTWAIN_CNTYNORFOLKI: i32 = 1028;
    pub const DTWAIN_CNTYNORWAY: i32 = 47;
    pub const DTWAIN_CNTYOMAN: i32 = 968;
    pub const DTWAIN_CNTYPAKISTAN: i32 = 92;
    pub const DTWAIN_CNTYPALAU: i32 = 1029;
    pub const DTWAIN_CNTYPANAMA: i32 = 507;
    pub const DTWAIN_CNTYPARAGUAY: i32 = 595;
    pub const DTWAIN_CNTYPERU: i32 = 51;
    pub const DTWAIN_CNTYPHILLIPPINES: i32 = 63;
    pub const DTWAIN_CNTYPITCAIRNIS: i32 = 1030;
    pub const DTWAIN_CNTYPNEWGUINEA: i32 = 675;
    pub const DTWAIN_CNTYPOLAND: i32 = 48;
    pub const DTWAIN_CNTYPORTUGAL: i32 = 351;
    pub const DTWAIN_CNTYQATAR: i32 = 974;
    pub const DTWAIN_CNTYREUNIONI: i32 = 1031;
    pub const DTWAIN_CNTYROMANIA: i32 = 40;
    pub const DTWAIN_CNTYRWANDA: i32 = 250;
    pub const DTWAIN_CNTYSAIPAN: i32 = 670;
    pub const DTWAIN_CNTYSANMARINO: i32 = 39;
    pub const DTWAIN_CNTYSAOTOME: i32 = 1033;
    pub const DTWAIN_CNTYSAUDIARABIA: i32 = 966;
    pub const DTWAIN_CNTYSENEGAL: i32 = 221;
    pub const DTWAIN_CNTYSEYCHELLESIS: i32 = 1034;
    pub const DTWAIN_CNTYSIERRALEONE: i32 = 1035;
    pub const DTWAIN_CNTYSINGAPORE: i32 = 65;
    pub const DTWAIN_CNTYSOLOMONIS: i32 = 1036;
    pub const DTWAIN_CNTYSOMALI: i32 = 1037;
    pub const DTWAIN_CNTYSOUTHAFRICA: i32 = 27;
    pub const DTWAIN_CNTYSPAIN: i32 = 34;
    pub const DTWAIN_CNTYSRILANKA: i32 = 94;
    pub const DTWAIN_CNTYSTHELENA: i32 = 1032;
    pub const DTWAIN_CNTYSTKITTS: i32 = 8013;
    pub const DTWAIN_CNTYSTLUCIA: i32 = 8014;
    pub const DTWAIN_CNTYSTPIERRE: i32 = 508;
    pub const DTWAIN_CNTYSTVINCENT: i32 = 8015;
    pub const DTWAIN_CNTYSUDAN: i32 = 1038;
    pub const DTWAIN_CNTYSURINAME: i32 = 597;
    pub const DTWAIN_CNTYSWAZILAND: i32 = 268;
    pub const DTWAIN_CNTYSWEDEN: i32 = 46;
    pub const DTWAIN_CNTYSWITZERLAND: i32 = 41;
    pub const DTWAIN_CNTYSYRIA: i32 = 1039;
    pub const DTWAIN_CNTYTAIWAN: i32 = 886;
    pub const DTWAIN_CNTYTANZANIA: i32 = 255;
    pub const DTWAIN_CNTYTHAILAND: i32 = 66;
    pub const DTWAIN_CNTYTOBAGO: i32 = 8016;
    pub const DTWAIN_CNTYTOGO: i32 = 228;
    pub const DTWAIN_CNTYTONGAIS: i32 = 676;
    pub const DTWAIN_CNTYTRINIDAD: i32 = 8016;
    pub const DTWAIN_CNTYTUNISIA: i32 = 216;
    pub const DTWAIN_CNTYTURKEY: i32 = 90;
    pub const DTWAIN_CNTYTURKSCAICOS: i32 = 8017;
    pub const DTWAIN_CNTYTUVALU: i32 = 1040;
    pub const DTWAIN_CNTYUGANDA: i32 = 256;
    pub const DTWAIN_CNTYUSSR: i32 = 7;
    pub const DTWAIN_CNTYUAEMIRATES: i32 = 971;
    pub const DTWAIN_CNTYUNITEDKINGDOM: i32 = 44;
    pub const DTWAIN_CNTYUSA: i32 = 1;
    pub const DTWAIN_CNTYURUGUAY: i32 = 598;
    pub const DTWAIN_CNTYVANUATU: i32 = 1041;
    pub const DTWAIN_CNTYVATICANCITY: i32 = 39;
    pub const DTWAIN_CNTYVENEZUELA: i32 = 58;
    pub const DTWAIN_CNTYWAKE: i32 = 1042;
    pub const DTWAIN_CNTYWALLISIS: i32 = 1043;
    pub const DTWAIN_CNTYWESTERNSAHARA: i32 = 1044;
    pub const DTWAIN_CNTYWESTERNSAMOA: i32 = 1045;
    pub const DTWAIN_CNTYYEMEN: i32 = 1046;
    pub const DTWAIN_CNTYYUGOSLAVIA: i32 = 38;
    pub const DTWAIN_CNTYZAIRE: i32 = 243;
    pub const DTWAIN_CNTYZAMBIA: i32 = 260;
    pub const DTWAIN_CNTYZIMBABWE: i32 = 263;
    pub const DTWAIN_LANGDANISH: i32 = 0;
    pub const DTWAIN_LANGDUTCH: i32 = 1;
    pub const DTWAIN_LANGINTERNATIONALENGLISH: i32 = 2;
    pub const DTWAIN_LANGFRENCHCANADIAN: i32 = 3;
    pub const DTWAIN_LANGFINNISH: i32 = 4;
    pub const DTWAIN_LANGFRENCH: i32 = 5;
    pub const DTWAIN_LANGGERMAN: i32 = 6;
    pub const DTWAIN_LANGICELANDIC: i32 = 7;
    pub const DTWAIN_LANGITALIAN: i32 = 8;
    pub const DTWAIN_LANGNORWEGIAN: i32 = 9;
    pub const DTWAIN_LANGPORTUGUESE: i32 = 10;
    pub const DTWAIN_LANGSPANISH: i32 = 11;
    pub const DTWAIN_LANGSWEDISH: i32 = 12;
    pub const DTWAIN_LANGUSAENGLISH: i32 = 13;
    pub const DTWAIN_NO_ERROR: i32 = 0;
    pub const DTWAIN_ERR_FIRST: i32 = -1000;
    pub const DTWAIN_ERR_BAD_HANDLE: i32 = -1001;
    pub const DTWAIN_ERR_BAD_SOURCE: i32 = -1002;
    pub const DTWAIN_ERR_BAD_ARRAY: i32 = -1003;
    pub const DTWAIN_ERR_WRONG_ARRAY_TYPE: i32 = -1004;
    pub const DTWAIN_ERR_INDEX_BOUNDS: i32 = -1005;
    pub const DTWAIN_ERR_OUT_OF_MEMORY: i32 = -1006;
    pub const DTWAIN_ERR_NULL_WINDOW: i32 = -1007;
    pub const DTWAIN_ERR_BAD_PIXTYPE: i32 = -1008;
    pub const DTWAIN_ERR_BAD_CONTAINER: i32 = -1009;
    pub const DTWAIN_ERR_NO_SESSION: i32 = -1010;
    pub const DTWAIN_ERR_BAD_ACQUIRE_NUM: i32 = -1011;
    pub const DTWAIN_ERR_BAD_CAP: i32 = -1012;
    pub const DTWAIN_ERR_CAP_NO_SUPPORT: i32 = -1013;
    pub const DTWAIN_ERR_TWAIN: i32 = -1014;
    pub const DTWAIN_ERR_HOOK_FAILED: i32 = -1015;
    pub const DTWAIN_ERR_BAD_FILENAME: i32 = -1016;
    pub const DTWAIN_ERR_EMPTY_ARRAY: i32 = -1017;
    pub const DTWAIN_ERR_FILE_FORMAT: i32 = -1018;
    pub const DTWAIN_ERR_BAD_DIB_PAGE: i32 = -1019;
    pub const DTWAIN_ERR_SOURCE_ACQUIRING: i32 = -1020;
    pub const DTWAIN_ERR_INVALID_PARAM: i32 = -1021;
    pub const DTWAIN_ERR_INVALID_RANGE: i32 = -1022;
    pub const DTWAIN_ERR_UI_ERROR: i32 = -1023;
    pub const DTWAIN_ERR_BAD_UNIT: i32 = -1024;
    pub const DTWAIN_ERR_LANGDLL_NOT_FOUND: i32 = -1025;
    pub const DTWAIN_ERR_SOURCE_NOT_OPEN: i32 = -1026;
    pub const DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED: i32 = -1027;
    pub const DTWAIN_ERR_UIONLY_NOT_SUPPORTED: i32 = -1028;
    pub const DTWAIN_ERR_UI_ALREADY_OPENED: i32 = -1029;
    pub const DTWAIN_ERR_CAPSET_NOSUPPORT: i32 = -1030;
    pub const DTWAIN_ERR_NO_FILE_XFER: i32 = -1031;
    pub const DTWAIN_ERR_INVALID_BITDEPTH: i32 = -1032;
    pub const DTWAIN_ERR_NO_CAPS_DEFINED: i32 = -1033;
    pub const DTWAIN_ERR_TILES_NOT_SUPPORTED: i32 = -1034;
    pub const DTWAIN_ERR_INVALID_DTWAIN_FRAME: i32 = -1035;
    pub const DTWAIN_ERR_LIMITED_VERSION: i32 = -1036;
    pub const DTWAIN_ERR_NO_FEEDER: i32 = -1037;
    pub const DTWAIN_ERR_NO_FEEDER_QUERY: i32 = -1038;
    pub const DTWAIN_ERR_EXCEPTION_ERROR: i32 = -1039;
    pub const DTWAIN_ERR_INVALID_STATE: i32 = -1040;
    pub const DTWAIN_ERR_UNSUPPORTED_EXTINFO: i32 = -1041;
    pub const DTWAIN_ERR_DLLRESOURCE_NOTFOUND: i32 = -1042;
    pub const DTWAIN_ERR_NOT_INITIALIZED: i32 = -1043;
    pub const DTWAIN_ERR_NO_SOURCES: i32 = -1044;
    pub const DTWAIN_ERR_TWAIN_NOT_INSTALLED: i32 = -1045;
    pub const DTWAIN_ERR_WRONG_THREAD: i32 = -1046;
    pub const DTWAIN_ERR_BAD_CAPTYPE: i32 = -1047;
    pub const DTWAIN_ERR_UNKNOWN_CAPDATATYPE: i32 = -1048;
    pub const DTWAIN_ERR_DEMO_NOFILETYPE: i32 = -1049;
    pub const DTWAIN_ERR_SOURCESELECTION_CANCELED: i32 = -1050;
    pub const DTWAIN_ERR_RESOURCES_NOT_FOUND: i32 = -1051;
    pub const DTWAIN_ERR_STRINGTYPE_MISMATCH: i32 = -1052;
    pub const DTWAIN_ERR_ARRAYTYPE_MISMATCH: i32 = -1053;
    pub const DTWAIN_ERR_SOURCENAME_NOTINSTALLED: i32 = -1054;
    pub const DTWAIN_ERR_NO_MEMFILE_XFER: i32 = -1055;
    pub const DTWAIN_ERR_AREA_ARRAY_TOO_SMALL: i32 = -1056;
    pub const DTWAIN_ERR_LOG_CREATE_ERROR: i32 = -1057;
    pub const DTWAIN_ERR_FILESYSTEM_NOT_SUPPORTED: i32 = -1058;
    pub const DTWAIN_ERR_TILEMODE_NOTSET: i32 = -1059;
    pub const DTWAIN_ERR_INI32_NOT_FOUND: i32 = -1060;
    pub const DTWAIN_ERR_INI64_NOT_FOUND: i32 = -1061;
    pub const DTWAIN_ERR_CRC_CHECK: i32 = -1062;
    pub const DTWAIN_ERR_RESOURCES_BAD_VERSION: i32 = -1063;
    pub const DTWAIN_ERR_WIN32_ERROR: i32 = -1064;
    pub const DTWAIN_ERR_STRINGID_NOTFOUND: i32 = -1065;
    pub const DTWAIN_ERR_RESOURCES_DUPLICATEID_FOUND: i32 = -1066;
    pub const DTWAIN_ERR_UNAVAILABLE_EXTINFO: i32 = -1067;
    pub const DTWAIN_ERR_TWAINDSM2_BADBITMAP: i32 = -1068;
    pub const DTWAIN_ERR_ACQUISITION_CANCELED: i32 = -1069;
    pub const DTWAIN_ERR_IMAGE_RESAMPLED: i32 = -1070;
    pub const DTWAIN_ERR_UNKNOWN_TWAIN_RC: i32 = -1071;
    pub const DTWAIN_ERR_UNKNOWN_TWAIN_CC: i32 = -1072;
    pub const DTWAIN_ERR_RESOURCES_DATA_EXCEPTION: i32 = -1073;
    pub const DTWAIN_ERR_AUDIO_TRANSFER_NOTSUPPORTED: i32 = -1074;
    pub const DTWAIN_ERR_FEEDER_COMPLIANCY: i32 = -1075;
    pub const DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY1: i32 = -1076;
    pub const DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY2: i32 = -1077;
    pub const DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY1: i32 = -1078;
    pub const DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY2: i32 = -1079;
    pub const DTWAIN_ERR_ICAPBITDEPTH_COMPLIANCY1: i32 = -1080;
    pub const DTWAIN_ERR_XFERMECH_COMPLIANCY: i32 = -1081;
    pub const DTWAIN_ERR_STANDARDCAPS_COMPLIANCY: i32 = -1082;
    pub const DTWAIN_ERR_EXTIMAGEINFO_DATATYPE_MISMATCH: i32 = -1083;
    pub const DTWAIN_ERR_EXTIMAGEINFO_RETRIEVAL: i32 = -1084;
    pub const DTWAIN_ERR_RANGE_OUTOFBOUNDS: i32 = -1085;
    pub const DTWAIN_ERR_RANGE_STEPISZERO: i32 = -1086;
    pub const DTWAIN_ERR_BLANKNAMEDETECTED: i32 = -1087;
    pub const DTWAIN_ERR_FEEDER_NOPAPERSENSOR: i32 = -1088;
    pub const TWAIN_ERR_LOW_MEMORY: i32 = -1100;
    pub const TWAIN_ERR_FALSE_ALARM: i32 = -1101;
    pub const TWAIN_ERR_BUMMER: i32 = -1102;
    pub const TWAIN_ERR_NODATASOURCE: i32 = -1103;
    pub const TWAIN_ERR_MAXCONNECTIONS: i32 = -1104;
    pub const TWAIN_ERR_OPERATIONERROR: i32 = -1105;
    pub const TWAIN_ERR_BADCAPABILITY: i32 = -1106;
    pub const TWAIN_ERR_BADVALUE: i32 = -1107;
    pub const TWAIN_ERR_BADPROTOCOL: i32 = -1108;
    pub const TWAIN_ERR_SEQUENCEERROR: i32 = -1109;
    pub const TWAIN_ERR_BADDESTINATION: i32 = -1110;
    pub const TWAIN_ERR_CAPNOTSUPPORTED: i32 = -1111;
    pub const TWAIN_ERR_CAPBADOPERATION: i32 = -1112;
    pub const TWAIN_ERR_CAPSEQUENCEERROR: i32 = -1113;
    pub const TWAIN_ERR_FILEPROTECTEDERROR: i32 = -1114;
    pub const TWAIN_ERR_FILEEXISTERROR: i32 = -1115;
    pub const TWAIN_ERR_FILENOTFOUND: i32 = -1116;
    pub const TWAIN_ERR_DIRNOTEMPTY: i32 = -1117;
    pub const TWAIN_ERR_FEEDERJAMMED: i32 = -1118;
    pub const TWAIN_ERR_FEEDERMULTPAGES: i32 = -1119;
    pub const TWAIN_ERR_FEEDERWRITEERROR: i32 = -1120;
    pub const TWAIN_ERR_DEVICEOFFLINE: i32 = -1121;
    pub const TWAIN_ERR_NULL_CONTAINER: i32 = -1122;
    pub const TWAIN_ERR_INTERLOCK: i32 = -1123;
    pub const TWAIN_ERR_DAMAGEDCORNER: i32 = -1124;
    pub const TWAIN_ERR_FOCUSERROR: i32 = -1125;
    pub const TWAIN_ERR_DOCTOOLIGHT: i32 = -1126;
    pub const TWAIN_ERR_DOCTOODARK: i32 = -1127;
    pub const TWAIN_ERR_NOMEDIA: i32 = -1128;
    pub const DTWAIN_ERR_FILEXFERSTART: i32 = -2000;
    pub const DTWAIN_ERR_MEM: i32 = -2001;
    pub const DTWAIN_ERR_FILEOPEN: i32 = -2002;
    pub const DTWAIN_ERR_FILEREAD: i32 = -2003;
    pub const DTWAIN_ERR_FILEWRITE: i32 = -2004;
    pub const DTWAIN_ERR_BADPARAM: i32 = -2005;
    pub const DTWAIN_ERR_INVALIDBMP: i32 = -2006;
    pub const DTWAIN_ERR_BMPRLE: i32 = -2007;
    pub const DTWAIN_ERR_RESERVED1: i32 = -2008;
    pub const DTWAIN_ERR_INVALIDJPG: i32 = -2009;
    pub const DTWAIN_ERR_DC: i32 = -2010;
    pub const DTWAIN_ERR_DIB: i32 = -2011;
    pub const DTWAIN_ERR_RESERVED2: i32 = -2012;
    pub const DTWAIN_ERR_NORESOURCE: i32 = -2013;
    pub const DTWAIN_ERR_CALLBACKCANCEL: i32 = -2014;
    pub const DTWAIN_ERR_INVALIDPNG: i32 = -2015;
    pub const DTWAIN_ERR_PNGCREATE: i32 = -2016;
    pub const DTWAIN_ERR_INTERNAL: i32 = -2017;
    pub const DTWAIN_ERR_FONT: i32 = -2018;
    pub const DTWAIN_ERR_INTTIFF: i32 = -2019;
    pub const DTWAIN_ERR_INVALIDTIFF: i32 = -2020;
    pub const DTWAIN_ERR_NOTIFFLZW: i32 = -2021;
    pub const DTWAIN_ERR_INVALIDPCX: i32 = -2022;
    pub const DTWAIN_ERR_CREATEBMP: i32 = -2023;
    pub const DTWAIN_ERR_NOLINES: i32 = -2024;
    pub const DTWAIN_ERR_GETDIB: i32 = -2025;
    pub const DTWAIN_ERR_NODEVOP: i32 = -2026;
    pub const DTWAIN_ERR_INVALIDWMF: i32 = -2027;
    pub const DTWAIN_ERR_DEPTHMISMATCH: i32 = -2028;
    pub const DTWAIN_ERR_BITBLT: i32 = -2029;
    pub const DTWAIN_ERR_BUFTOOSMALL: i32 = -2030;
    pub const DTWAIN_ERR_TOOMANYCOLORS: i32 = -2031;
    pub const DTWAIN_ERR_INVALIDTGA: i32 = -2032;
    pub const DTWAIN_ERR_NOTGATHUMBNAIL: i32 = -2033;
    pub const DTWAIN_ERR_RESERVED3: i32 = -2034;
    pub const DTWAIN_ERR_CREATEDIB: i32 = -2035;
    pub const DTWAIN_ERR_NOLZW: i32 = -2036;
    pub const DTWAIN_ERR_SELECTOBJ: i32 = -2037;
    pub const DTWAIN_ERR_BADMANAGER: i32 = -2038;
    pub const DTWAIN_ERR_OBSOLETE: i32 = -2039;
    pub const DTWAIN_ERR_CREATEDIBSECTION: i32 = -2040;
    pub const DTWAIN_ERR_SETWINMETAFILEBITS: i32 = -2041;
    pub const DTWAIN_ERR_GETWINMETAFILEBITS: i32 = -2042;
    pub const DTWAIN_ERR_PAXPWD: i32 = -2043;
    pub const DTWAIN_ERR_INVALIDPAX: i32 = -2044;
    pub const DTWAIN_ERR_NOSUPPORT: i32 = -2045;
    pub const DTWAIN_ERR_INVALIDPSD: i32 = -2046;
    pub const DTWAIN_ERR_PSDNOTSUPPORTED: i32 = -2047;
    pub const DTWAIN_ERR_DECRYPT: i32 = -2048;
    pub const DTWAIN_ERR_ENCRYPT: i32 = -2049;
    pub const DTWAIN_ERR_COMPRESSION: i32 = -2050;
    pub const DTWAIN_ERR_DECOMPRESSION: i32 = -2051;
    pub const DTWAIN_ERR_INVALIDTLA: i32 = -2052;
    pub const DTWAIN_ERR_INVALIDWBMP: i32 = -2053;
    pub const DTWAIN_ERR_NOTIFFTAG: i32 = -2054;
    pub const DTWAIN_ERR_NOLOCALSTORAGE: i32 = -2055;
    pub const DTWAIN_ERR_INVALIDEXIF: i32 = -2056;
    pub const DTWAIN_ERR_NOEXIFSTRING: i32 = -2057;
    pub const DTWAIN_ERR_TIFFDLL32NOTFOUND: i32 = -2058;
    pub const DTWAIN_ERR_TIFFDLL16NOTFOUND: i32 = -2059;
    pub const DTWAIN_ERR_PNGDLL16NOTFOUND: i32 = -2060;
    pub const DTWAIN_ERR_JPEGDLL16NOTFOUND: i32 = -2061;
    pub const DTWAIN_ERR_BADBITSPERPIXEL: i32 = -2062;
    pub const DTWAIN_ERR_TIFFDLL32INVALIDVER: i32 = -2063;
    pub const DTWAIN_ERR_PDFDLL32NOTFOUND: i32 = -2064;
    pub const DTWAIN_ERR_PDFDLL32INVALIDVER: i32 = -2065;
    pub const DTWAIN_ERR_JPEGDLL32NOTFOUND: i32 = -2066;
    pub const DTWAIN_ERR_JPEGDLL32INVALIDVER: i32 = -2067;
    pub const DTWAIN_ERR_PNGDLL32NOTFOUND: i32 = -2068;
    pub const DTWAIN_ERR_PNGDLL32INVALIDVER: i32 = -2069;
    pub const DTWAIN_ERR_J2KDLL32NOTFOUND: i32 = -2070;
    pub const DTWAIN_ERR_J2KDLL32INVALIDVER: i32 = -2071;
    pub const DTWAIN_ERR_MANDUPLEX_UNAVAILABLE: i32 = -2072;
    pub const DTWAIN_ERR_TIMEOUT: i32 = -2073;
    pub const DTWAIN_ERR_INVALIDICONFORMAT: i32 = -2074;
    pub const DTWAIN_ERR_TWAIN32DSMNOTFOUND: i32 = -2075;
    pub const DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND: i32 = -2076;
    pub const DTWAIN_ERR_INVALID_DIRECTORY: i32 = -2077;
    pub const DTWAIN_ERR_CREATE_DIRECTORY: i32 = -2078;
    pub const DTWAIN_ERR_OCRLIBRARY_NOTFOUND: i32 = -2079;
    pub const DTWAIN_TWAINSAVE_OK: i32 = 0;
    pub const DTWAIN_ERR_TS_FIRST: i32 = -2080;
    pub const DTWAIN_ERR_TS_NOFILENAME: i32 = -2081;
    pub const DTWAIN_ERR_TS_NOTWAINSYS: i32 = -2082;
    pub const DTWAIN_ERR_TS_DEVICEFAILURE: i32 = -2083;
    pub const DTWAIN_ERR_TS_FILESAVEERROR: i32 = -2084;
    pub const DTWAIN_ERR_TS_COMMANDILLEGAL: i32 = -2085;
    pub const DTWAIN_ERR_TS_CANCELLED: i32 = -2086;
    pub const DTWAIN_ERR_TS_ACQUISITIONERROR: i32 = -2087;
    pub const DTWAIN_ERR_TS_INVALIDCOLORSPACE: i32 = -2088;
    pub const DTWAIN_ERR_TS_PDFNOTSUPPORTED: i32 = -2089;
    pub const DTWAIN_ERR_TS_NOTAVAILABLE: i32 = -2090;
    pub const DTWAIN_ERR_OCR_FIRST: i32 = -2100;
    pub const DTWAIN_ERR_OCR_INVALIDPAGENUM: i32 = -2101;
    pub const DTWAIN_ERR_OCR_INVALIDENGINE: i32 = -2102;
    pub const DTWAIN_ERR_OCR_NOTACTIVE: i32 = -2103;
    pub const DTWAIN_ERR_OCR_INVALIDFILETYPE: i32 = -2104;
    pub const DTWAIN_ERR_OCR_INVALIDPIXELTYPE: i32 = -2105;
    pub const DTWAIN_ERR_OCR_INVALIDBITDEPTH: i32 = -2106;
    pub const DTWAIN_ERR_OCR_RECOGNITIONERROR: i32 = -2107;
    pub const DTWAIN_ERR_OCR_LAST: i32 = -2108;
    pub const DTWAIN_ERR_LAST: i32 = DTwainAPI::DTWAIN_ERR_OCR_LAST;
    pub const DTWAIN_ERR_SOURCE_COULD_NOT_OPEN: i32 = -2500;
    pub const DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE: i32 = -2501;
    pub const DTWAIN_ERR_IMAGEINFO_INVALID: i32 = -2502;
    pub const DTWAIN_ERR_WRITEDATA_TOFILE: i32 = -2503;
    pub const DTWAIN_ERR_OPERATION_NOTSUPPORTED: i32 = -2504;
    pub const DTWAIN_DE_CHKAUTOCAPTURE: i32 = 1;
    pub const DTWAIN_DE_CHKBATTERY: i32 = 2;
    pub const DTWAIN_DE_CHKDEVICEONLINE: i32 = 4;
    pub const DTWAIN_DE_CHKFLASH: i32 = 8;
    pub const DTWAIN_DE_CHKPOWERSUPPLY: i32 = 16;
    pub const DTWAIN_DE_CHKRESOLUTION: i32 = 32;
    pub const DTWAIN_DE_DEVICEADDED: i32 = 64;
    pub const DTWAIN_DE_DEVICEOFFLINE: i32 = 128;
    pub const DTWAIN_DE_DEVICEREADY: i32 = 256;
    pub const DTWAIN_DE_DEVICEREMOVED: i32 = 512;
    pub const DTWAIN_DE_IMAGECAPTURED: i32 = 1024;
    pub const DTWAIN_DE_IMAGEDELETED: i32 = 2048;
    pub const DTWAIN_DE_PAPERDOUBLEFEED: i32 = 4096;
    pub const DTWAIN_DE_PAPERJAM: i32 = 8192;
    pub const DTWAIN_DE_LAMPFAILURE: i32 = 16384;
    pub const DTWAIN_DE_POWERSAVE: i32 = 32768;
    pub const DTWAIN_DE_POWERSAVENOTIFY: i32 = 65536;
    pub const DTWAIN_DE_CUSTOMEVENTS: i32 = 0x8000;
    pub const DTWAIN_GETDE_EVENT: i32 = 0;
    pub const DTWAIN_GETDE_DEVNAME: i32 = 1;
    pub const DTWAIN_GETDE_BATTERYMINUTES: i32 = 2;
    pub const DTWAIN_GETDE_BATTERYPCT: i32 = 3;
    pub const DTWAIN_GETDE_XRESOLUTION: i32 = 4;
    pub const DTWAIN_GETDE_YRESOLUTION: i32 = 5;
    pub const DTWAIN_GETDE_FLASHUSED: i32 = 6;
    pub const DTWAIN_GETDE_AUTOCAPTURE: i32 = 7;
    pub const DTWAIN_GETDE_TIMEBEFORECAPTURE: i32 = 8;
    pub const DTWAIN_GETDE_TIMEBETWEENCAPTURES: i32 = 9;
    pub const DTWAIN_GETDE_POWERSUPPLY: i32 = 10;
    pub const DTWAIN_IMPRINTERTOPBEFORE: i32 = 1;
    pub const DTWAIN_IMPRINTERTOPAFTER: i32 = 2;
    pub const DTWAIN_IMPRINTERBOTTOMBEFORE: i32 = 4;
    pub const DTWAIN_IMPRINTERBOTTOMAFTER: i32 = 8;
    pub const DTWAIN_ENDORSERTOPBEFORE: i32 = 16;
    pub const DTWAIN_ENDORSERTOPAFTER: i32 = 32;
    pub const DTWAIN_ENDORSERBOTTOMBEFORE: i32 = 64;
    pub const DTWAIN_ENDORSERBOTTOMAFTER: i32 = 128;
    pub const DTWAIN_PM_SINGLESTRING: i32 = 0;
    pub const DTWAIN_PM_MULTISTRING: i32 = 1;
    pub const DTWAIN_PM_COMPOUNDSTRING: i32 = 2;
    pub const DTWAIN_TWTY_INT8: i32 = 0x0000;
    pub const DTWAIN_TWTY_INT16: i32 = 0x0001;
    pub const DTWAIN_TWTY_INT32: i32 = 0x0002;
    pub const DTWAIN_TWTY_UINT8: i32 = 0x0003;
    pub const DTWAIN_TWTY_UINT16: i32 = 0x0004;
    pub const DTWAIN_TWTY_UINT32: i32 = 0x0005;
    pub const DTWAIN_TWTY_BOOL: i32 = 0x0006;
    pub const DTWAIN_TWTY_FIX32: i32 = 0x0007;
    pub const DTWAIN_TWTY_FRAME: i32 = 0x0008;
    pub const DTWAIN_TWTY_STR32: i32 = 0x0009;
    pub const DTWAIN_TWTY_STR64: i32 = 0x000A;
    pub const DTWAIN_TWTY_STR128: i32 = 0x000B;
    pub const DTWAIN_TWTY_STR255: i32 = 0x000C;
    pub const DTWAIN_TWTY_STR1024: i32 = 0x000D;
    pub const DTWAIN_TWTY_UNI512: i32 = 0x000E;
    pub const DTWAIN_EI_BARCODEX: i32 = 0x1200;
    pub const DTWAIN_EI_BARCODEY: i32 = 0x1201;
    pub const DTWAIN_EI_BARCODETEXT: i32 = 0x1202;
    pub const DTWAIN_EI_BARCODETYPE: i32 = 0x1203;
    pub const DTWAIN_EI_DESHADETOP: i32 = 0x1204;
    pub const DTWAIN_EI_DESHADELEFT: i32 = 0x1205;
    pub const DTWAIN_EI_DESHADEHEIGHT: i32 = 0x1206;
    pub const DTWAIN_EI_DESHADEWIDTH: i32 = 0x1207;
    pub const DTWAIN_EI_DESHADESIZE: i32 = 0x1208;
    pub const DTWAIN_EI_SPECKLESREMOVED: i32 = 0x1209;
    pub const DTWAIN_EI_HORZLINEXCOORD: i32 = 0x120A;
    pub const DTWAIN_EI_HORZLINEYCOORD: i32 = 0x120B;
    pub const DTWAIN_EI_HORZLINELENGTH: i32 = 0x120C;
    pub const DTWAIN_EI_HORZLINETHICKNESS: i32 = 0x120D;
    pub const DTWAIN_EI_VERTLINEXCOORD: i32 = 0x120E;
    pub const DTWAIN_EI_VERTLINEYCOORD: i32 = 0x120F;
    pub const DTWAIN_EI_VERTLINELENGTH: i32 = 0x1210;
    pub const DTWAIN_EI_VERTLINETHICKNESS: i32 = 0x1211;
    pub const DTWAIN_EI_PATCHCODE: i32 = 0x1212;
    pub const DTWAIN_EI_ENDORSEDTEXT: i32 = 0x1213;
    pub const DTWAIN_EI_FORMCONFIDENCE: i32 = 0x1214;
    pub const DTWAIN_EI_FORMTEMPLATEMATCH: i32 = 0x1215;
    pub const DTWAIN_EI_FORMTEMPLATEPAGEMATCH: i32 = 0x1216;
    pub const DTWAIN_EI_FORMHORZDOCOFFSET: i32 = 0x1217;
    pub const DTWAIN_EI_FORMVERTDOCOFFSET: i32 = 0x1218;
    pub const DTWAIN_EI_BARCODECOUNT: i32 = 0x1219;
    pub const DTWAIN_EI_BARCODECONFIDENCE: i32 = 0x121A;
    pub const DTWAIN_EI_BARCODEROTATION: i32 = 0x121B;
    pub const DTWAIN_EI_BARCODETEXTLENGTH: i32 = 0x121C;
    pub const DTWAIN_EI_DESHADECOUNT: i32 = 0x121D;
    pub const DTWAIN_EI_DESHADEBLACKCOUNTOLD: i32 = 0x121E;
    pub const DTWAIN_EI_DESHADEBLACKCOUNTNEW: i32 = 0x121F;
    pub const DTWAIN_EI_DESHADEBLACKRLMIN: i32 = 0x1220;
    pub const DTWAIN_EI_DESHADEBLACKRLMAX: i32 = 0x1221;
    pub const DTWAIN_EI_DESHADEWHITECOUNTOLD: i32 = 0x1222;
    pub const DTWAIN_EI_DESHADEWHITECOUNTNEW: i32 = 0x1223;
    pub const DTWAIN_EI_DESHADEWHITERLMIN: i32 = 0x1224;
    pub const DTWAIN_EI_DESHADEWHITERLAVE: i32 = 0x1225;
    pub const DTWAIN_EI_DESHADEWHITERLMAX: i32 = 0x1226;
    pub const DTWAIN_EI_BLACKSPECKLESREMOVED: i32 = 0x1227;
    pub const DTWAIN_EI_WHITESPECKLESREMOVED: i32 = 0x1228;
    pub const DTWAIN_EI_HORZLINECOUNT: i32 = 0x1229;
    pub const DTWAIN_EI_VERTLINECOUNT: i32 = 0x122A;
    pub const DTWAIN_EI_DESKEWSTATUS: i32 = 0x122B;
    pub const DTWAIN_EI_SKEWORIGINALANGLE: i32 = 0x122C;
    pub const DTWAIN_EI_SKEWFINALANGLE: i32 = 0x122D;
    pub const DTWAIN_EI_SKEWCONFIDENCE: i32 = 0x122E;
    pub const DTWAIN_EI_SKEWWINDOWX1: i32 = 0x122F;
    pub const DTWAIN_EI_SKEWWINDOWY1: i32 = 0x1230;
    pub const DTWAIN_EI_SKEWWINDOWX2: i32 = 0x1231;
    pub const DTWAIN_EI_SKEWWINDOWY2: i32 = 0x1232;
    pub const DTWAIN_EI_SKEWWINDOWX3: i32 = 0x1233;
    pub const DTWAIN_EI_SKEWWINDOWY3: i32 = 0x1234;
    pub const DTWAIN_EI_SKEWWINDOWX4: i32 = 0x1235;
    pub const DTWAIN_EI_SKEWWINDOWY4: i32 = 0x1236;
    pub const DTWAIN_EI_BOOKNAME: i32 = 0x1238;
    pub const DTWAIN_EI_CHAPTERNUMBER: i32 = 0x1239;
    pub const DTWAIN_EI_DOCUMENTNUMBER: i32 = 0x123A;
    pub const DTWAIN_EI_PAGENUMBER: i32 = 0x123B;
    pub const DTWAIN_EI_CAMERA: i32 = 0x123C;
    pub const DTWAIN_EI_FRAMENUMBER: i32 = 0x123D;
    pub const DTWAIN_EI_FRAME: i32 = 0x123E;
    pub const DTWAIN_EI_PIXELFLAVOR: i32 = 0x123F;
    pub const DTWAIN_EI_ICCPROFILE: i32 = 0x1240;
    pub const DTWAIN_EI_LASTSEGMENT: i32 = 0x1241;
    pub const DTWAIN_EI_SEGMENTNUMBER: i32 = 0x1242;
    pub const DTWAIN_EI_MAGDATA: i32 = 0x1243;
    pub const DTWAIN_EI_MAGTYPE: i32 = 0x1244;
    pub const DTWAIN_EI_PAGESIDE: i32 = 0x1245;
    pub const DTWAIN_EI_FILESYSTEMSOURCE: i32 = 0x1246;
    pub const DTWAIN_EI_IMAGEMERGED: i32 = 0x1247;
    pub const DTWAIN_EI_MAGDATALENGTH: i32 = 0x1248;
    pub const DTWAIN_EI_PAPERCOUNT: i32 = 0x1249;
    pub const DTWAIN_EI_PRINTERTEXT: i32 = 0x124A;
    pub const DTWAIN_EI_TWAINDIRECTMETADATA: i32 = 0x124B;
    pub const DTWAIN_EI_IAFIELDA_VALUE: i32 = 0x124C;
    pub const DTWAIN_EI_IAFIELDB_VALUE: i32 = 0x124D;
    pub const DTWAIN_EI_IAFIELDC_VALUE: i32 = 0x124E;
    pub const DTWAIN_EI_IAFIELDD_VALUE: i32 = 0x124F;
    pub const DTWAIN_EI_IAFIELDE_VALUE: i32 = 0x1250;
    pub const DTWAIN_EI_IALEVEL: i32 = 0x1251;
    pub const DTWAIN_EI_PRINTER: i32 = 0x1252;
    pub const DTWAIN_EI_BARCODETEXT2: i32 = 0x1253;
    pub const DTWAIN_LOG_DECODE_SOURCE: u32 = 0x1      ;
    pub const DTWAIN_LOG_DECODE_DEST: u32 = 0x2        ;
    pub const DTWAIN_LOG_DECODE_TWMEMREF: u32 = 0x4    ;
    pub const DTWAIN_LOG_DECODE_TWEVENT: u32 = 0x8     ;
    pub const DTWAIN_LOG_CALLSTACK: u32 = 0x10         ;
    pub const DTWAIN_LOG_ISTWAINMSG: u32 = 0x20        ;
    pub const DTWAIN_LOG_INITFAILURE: u32 = 0x40       ;
    pub const DTWAIN_LOG_LOWLEVELTWAIN: u32 = 0x80     ;
    pub const DTWAIN_LOG_DECODE_BITMAP: u32 = 0x100    ;
    pub const DTWAIN_LOG_NOTIFICATIONS: u32 = 0x200    ;
    pub const DTWAIN_LOG_MISCELLANEOUS: u32 = 0x400    ;
    pub const DTWAIN_LOG_DTWAINERRORS: u32 = 0x800     ;
    pub const DTWAIN_LOG_USEFILE: u32 = 0x10000        ;
    pub const DTWAIN_LOG_SHOWEXCEPTIONS: u32 = 0x20000 ;
    pub const DTWAIN_LOG_ERRORMSGBOX: u32 = 0x40000    ;
    pub const DTWAIN_LOG_USEBUFFER: u32 = 0x80000      ;
    pub const DTWAIN_LOG_FILEAPPEND: u32 = 0x100000    ;
    pub const DTWAIN_LOG_USECALLBACK: u32 = 0x200000   ;
    pub const DTWAIN_LOG_USECRLF: u32 = 0x400000       ;
    pub const DTWAIN_LOG_CONSOLE: u32 = 0x800000       ;
    pub const DTWAIN_LOG_DEBUGMONITOR: u32 = 0x1000000 ;
    pub const DTWAIN_LOG_USEWINDOW: u32 = 0x2000000    ;
    pub const DTWAIN_LOG_CREATEDIRECTORY: u32 = 0x04000000;
    pub const DTWAIN_LOG_CONSOLEWITHHANDLER: u32 = 0x08000000 | DTwainAPI::DTWAIN_LOG_CONSOLE;
    pub const DTWAIN_LOG_ALL: u32 = DTwainAPI::DTWAIN_LOG_DECODE_SOURCE | DTwainAPI::DTWAIN_LOG_DECODE_DEST | DTwainAPI::DTWAIN_LOG_DECODE_TWEVENT | DTwainAPI::DTWAIN_LOG_DECODE_TWMEMREF | DTwainAPI::DTWAIN_LOG_CALLSTACK | DTwainAPI::DTWAIN_LOG_ISTWAINMSG | DTwainAPI::DTWAIN_LOG_INITFAILURE | DTwainAPI::DTWAIN_LOG_LOWLEVELTWAIN | DTwainAPI::DTWAIN_LOG_NOTIFICATIONS | DTwainAPI::DTWAIN_LOG_MISCELLANEOUS | DTwainAPI::DTWAIN_LOG_DTWAINERRORS | DTwainAPI::DTWAIN_LOG_DECODE_BITMAP;
    pub const DTWAIN_LOG_ALL_APPEND: u32 = 0xFFFFFFFF;
    pub const DTWAIN_TEMPDIR_CREATEDIRECTORY: u32 = DTwainAPI::DTWAIN_LOG_CREATEDIRECTORY;
    pub const DTWAINGCD_RETURNHANDLE: i32 = 1;
    pub const DTWAINGCD_COPYDATA: i32 = 2;
    pub const DTWAIN_BYPOSITION: i32 = 0;
    pub const DTWAIN_BYID: i32 = 1;
    pub const DTWAINSCD_USEHANDLE: i32 = 1;
    pub const DTWAINSCD_USEDATA: i32 = 2;
    pub const DTWAIN_PAGEFAIL_RETRY: i32 = 1;
    pub const DTWAIN_PAGEFAIL_TERMINATE: i32 = 2;
    pub const DTWAIN_MAXRETRY_ATTEMPTS: i32 = 3;
    pub const DTWAIN_RETRY_FOREVER: i32 = -1;
    pub const DTWAIN_PDF_NOSCALING: i32 = 128;
    pub const DTWAIN_PDF_FITPAGE: i32 = 256;
    pub const DTWAIN_PDF_VARIABLEPAGESIZE: i32 = 512;
    pub const DTWAIN_PDF_CUSTOMSIZE: i32 = 1024;
    pub const DTWAIN_PDF_USECOMPRESSION: i32 = 2048;
    pub const DTWAIN_PDF_CUSTOMSCALE: i32 = 4096;
    pub const DTWAIN_PDF_PIXELSPERMETERSIZE: i32 = 8192;
    pub const DTWAIN_PDF_ALLOWPRINTING: u32 = 2052;
    pub const DTWAIN_PDF_ALLOWMOD: u32 = 8;
    pub const DTWAIN_PDF_ALLOWCOPY: u32 = 16;
    pub const DTWAIN_PDF_ALLOWMODANNOTATIONS: u32 = 32;
    pub const DTWAIN_PDF_ALLOWFILLIN: u32 = 256;
    pub const DTWAIN_PDF_ALLOWEXTRACTION: u32 = 512;
    pub const DTWAIN_PDF_ALLOWASSEMBLY: u32 = 1024;
    pub const DTWAIN_PDF_ALLOWDEGRADEDPRINTING: u32 = 4;
    pub const DTWAIN_PDF_ALLOWALL: u32 = 0xFFFFFFFC;
    pub const DTWAIN_PDF_ALLOWANYMOD: u32 = DTwainAPI::DTWAIN_PDF_ALLOWMOD | DTwainAPI::DTWAIN_PDF_ALLOWFILLIN | DTwainAPI::DTWAIN_PDF_ALLOWMODANNOTATIONS | DTwainAPI::DTWAIN_PDF_ALLOWASSEMBLY;
    pub const DTWAIN_PDF_ALLOWANYPRINTING: u32 = DTwainAPI::DTWAIN_PDF_ALLOWPRINTING | DTwainAPI::DTWAIN_PDF_ALLOWDEGRADEDPRINTING;
    pub const DTWAIN_PDF_PORTRAIT: i32 = 0;
    pub const DTWAIN_PDF_LANDSCAPE: i32 = 1;
    pub const DTWAIN_PS_REGULAR: i32 = 0;
    pub const DTWAIN_PS_ENCAPSULATED: i32 = 1;
    pub const DTWAIN_BP_AUTODISCARD_NONE: i32 = 0;
    pub const DTWAIN_BP_AUTODISCARD_IMMEDIATE: i32 = 1;
    pub const DTWAIN_BP_AUTODISCARD_AFTERPROCESS: i32 = 2;
    pub const DTWAIN_BP_DETECTORIGINAL: i32 = 1;
    pub const DTWAIN_BP_DETECTADJUSTED: i32 = 2;
    pub const DTWAIN_BP_DETECTALL: i32 = DTwainAPI::DTWAIN_BP_DETECTORIGINAL | DTwainAPI::DTWAIN_BP_DETECTADJUSTED;
    pub const DTWAIN_BP_DISABLE: i32 = -2;
    pub const DTWAIN_BP_AUTO: i32 = -1;
    pub const DTWAIN_BP_AUTODISCARD_ANY: u32 = 0xFFFF;
    pub const DTWAIN_LP_REFLECTIVE: i32 = 0;
    pub const DTWAIN_LP_TRANSMISSIVE: i32 = 1;
    pub const DTWAIN_LS_RED: i32 = 0;
    pub const DTWAIN_LS_GREEN: i32 = 1;
    pub const DTWAIN_LS_BLUE: i32 = 2;
    pub const DTWAIN_LS_NONE: i32 = 3;
    pub const DTWAIN_LS_WHITE: i32 = 4;
    pub const DTWAIN_LS_UV: i32 = 5;
    pub const DTWAIN_LS_IR: i32 = 6;
    pub const DTWAIN_DLG_SORTNAMES: i32 = 1;
    pub const DTWAIN_DLG_CENTER: i32 = 2;
    pub const DTWAIN_DLG_CENTER_SCREEN: i32 = 4;
    pub const DTWAIN_DLG_USETEMPLATE: i32 = 8;
    pub const DTWAIN_DLG_CLEAR_PARAMS: i32 = 16;
    pub const DTWAIN_DLG_HORIZONTALSCROLL: i32 = 32;
    pub const DTWAIN_DLG_USEINCLUDENAMES: i32 = 64;
    pub const DTWAIN_DLG_USEEXCLUDENAMES: i32 = 128;
    pub const DTWAIN_DLG_USENAMEMAPPING: i32 = 256;
    pub const DTWAIN_DLG_TOPMOSTWINDOW: i32 = 1024;
    pub const DTWAIN_DLG_OPENONSELECT: i32 = 2048;
    pub const DTWAIN_DLG_NOOPENONSELECT: i32 = 4096;
    pub const DTWAIN_DLG_HIGHLIGHTFIRST: i32 = 8192;
    pub const DTWAIN_DLG_SAVELASTSCREENPOS: i32 = 16384;
    pub const DTWAIN_RES_ENGLISH: i32 = 0;
    pub const DTWAIN_RES_FRENCH: i32 = 1;
    pub const DTWAIN_RES_SPANISH: i32 = 2;
    pub const DTWAIN_RES_DUTCH: i32 = 3;
    pub const DTWAIN_RES_GERMAN: i32 = 4;
    pub const DTWAIN_RES_ITALIAN: i32 = 5;
    pub const DTWAIN_AL_ALARM: i32 = 0;
    pub const DTWAIN_AL_FEEDERERROR: i32 = 1;
    pub const DTWAIN_AL_FEEDERWARNING: i32 = 2;
    pub const DTWAIN_AL_BARCODE: i32 = 3;
    pub const DTWAIN_AL_DOUBLEFEED: i32 = 4;
    pub const DTWAIN_AL_JAM: i32 = 5;
    pub const DTWAIN_AL_PATCHCODE: i32 = 6;
    pub const DTWAIN_AL_POWER: i32 = 7;
    pub const DTWAIN_AL_SKEW: i32 = 8;
    pub const DTWAIN_FT_CAMERA: i32 = 0;
    pub const DTWAIN_FT_CAMERATOP: i32 = 1;
    pub const DTWAIN_FT_CAMERABOTTOM: i32 = 2;
    pub const DTWAIN_FT_CAMERAPREVIEW: i32 = 3;
    pub const DTWAIN_FT_DOMAIN: i32 = 4;
    pub const DTWAIN_FT_HOST: i32 = 5;
    pub const DTWAIN_FT_DIRECTORY: i32 = 6;
    pub const DTWAIN_FT_IMAGE: i32 = 7;
    pub const DTWAIN_FT_UNKNOWN: i32 = 8;
    pub const DTWAIN_NF_NONE: i32 = 0;
    pub const DTWAIN_NF_AUTO: i32 = 1;
    pub const DTWAIN_NF_LONEPIXEL: i32 = 2;
    pub const DTWAIN_NF_MAJORITYRULE: i32 = 3;
    pub const DTWAIN_CB_AUTO: i32 = 0;
    pub const DTWAIN_CB_CLEAR: i32 = 1;
    pub const DTWAIN_CB_NOCLEAR: i32 = 2;
    pub const DTWAIN_FA_NONE: i32 = 0;
    pub const DTWAIN_FA_LEFT: i32 = 1;
    pub const DTWAIN_FA_CENTER: i32 = 2;
    pub const DTWAIN_FA_RIGHT: i32 = 3;
    pub const DTWAIN_PF_CHOCOLATE: i32 = 0;
    pub const DTWAIN_PF_VANILLA: i32 = 1;
    pub const DTWAIN_FO_FIRSTPAGEFIRST: i32 = 0;
    pub const DTWAIN_FO_LASTPAGEFIRST: i32 = 1;
    pub const DTWAIN_INCREMENT_STATIC: i32 = 0;
    pub const DTWAIN_INCREMENT_DYNAMIC: i32 = 1;
    pub const DTWAIN_INCREMENT_DEFAULT: i32 = -1;
    pub const DTWAIN_MANDUP_FACEUPTOPPAGE: i32 = 0;
    pub const DTWAIN_MANDUP_FACEUPBOTTOMPAGE: i32 = 1;
    pub const DTWAIN_MANDUP_FACEDOWNTOPPAGE: i32 = 2;
    pub const DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE: i32 = 3;
    pub const DTWAIN_FILESAVE_DEFAULT: i32 = 0;
    pub const DTWAIN_FILESAVE_UICLOSE: i32 = 1;
    pub const DTWAIN_FILESAVE_SOURCECLOSE: i32 = 2;
    pub const DTWAIN_FILESAVE_ENDACQUIRE: i32 = 3;
    pub const DTWAIN_FILESAVE_MANUALSAVE: i32 = 4;
    pub const DTWAIN_FILESAVE_SAVEINCOMPLETE: i32 = 128;
    pub const DTWAIN_MANDUP_SCANOK: i32 = 1;
    pub const DTWAIN_MANDUP_SIDE1RESCAN: i32 = 2;
    pub const DTWAIN_MANDUP_SIDE2RESCAN: i32 = 3;
    pub const DTWAIN_MANDUP_RESCANALL: i32 = 4;
    pub const DTWAIN_MANDUP_PAGEMISSING: i32 = 5;
    pub const DTWAIN_DEMODLL_VERSION: i32 = 0x00000001;
    pub const DTWAIN_UNLICENSED_VERSION: i32 = 0x00000002;
    pub const DTWAIN_COMPANY_VERSION: i32 = 0x00000004;
    pub const DTWAIN_GENERAL_VERSION: i32 = 0x00000008;
    pub const DTWAIN_DEVELOP_VERSION: i32 = 0x00000010;
    pub const DTWAIN_JAVA_VERSION: i32 = 0x00000020;
    pub const DTWAIN_TOOLKIT_VERSION: i32 = 0x00000040;
    pub const DTWAIN_LIMITEDDLL_VERSION: i32 = 0x00000080;
    pub const DTWAIN_STATICLIB_VERSION: i32 = 0x00000100;
    pub const DTWAIN_STATICLIB_STDCALL_VERSION: i32 = 0x00000200;
    pub const DTWAIN_PDF_VERSION: i32 = 0x00010000;
    pub const DTWAIN_TWAINSAVE_VERSION: i32 = 0x00020000;
    pub const DTWAIN_OCR_VERSION: i32 = 0x00040000;
    pub const DTWAIN_BARCODE_VERSION: i32 = 0x00080000;
    pub const DTWAIN_ACTIVEX_VERSION: i32 = 0x00100000;
    pub const DTWAIN_32BIT_VERSION: i32 = 0x00200000;
    pub const DTWAIN_64BIT_VERSION: i32 = 0x00400000;
    pub const DTWAIN_UNICODE_VERSION: i32 = 0x00800000;
    pub const DTWAIN_OPENSOURCE_VERSION: i32 = 0x01000000;
    pub const DTWAIN_CALLSTACK_LOGGING: i32 = 0x02000000;
    pub const DTWAIN_CALLSTACK_LOGGING_PLUS: i32 = 0x04000000;
    pub const DTWAINOCR_RETURNHANDLE: i32 = 1;
    pub const DTWAINOCR_COPYDATA: i32 = 2;
    pub const DTWAIN_OCRINFO_CHAR: i32 = 0;
    pub const DTWAIN_OCRINFO_CHARXPOS: i32 = 1;
    pub const DTWAIN_OCRINFO_CHARYPOS: i32 = 2;
    pub const DTWAIN_OCRINFO_CHARXWIDTH: i32 = 3;
    pub const DTWAIN_OCRINFO_CHARYWIDTH: i32 = 4;
    pub const DTWAIN_OCRINFO_CHARCONFIDENCE: i32 = 5;
    pub const DTWAIN_OCRINFO_PAGENUM: i32 = 6;
    pub const DTWAIN_OCRINFO_OCRENGINE: i32 = 7;
    pub const DTWAIN_OCRINFO_TEXTLENGTH: i32 = 8;
    pub const DTWAIN_PDFPAGETYPE_COLOR: i32 = 0;
    pub const DTWAIN_PDFPAGETYPE_BW: i32 = 1;
    pub const DTWAIN_TWAINDSM_LEGACY: i32 = 1;
    pub const DTWAIN_TWAINDSM_VERSION2: i32 = 2;
    pub const DTWAIN_TWAINDSM_LATESTVERSION: i32 = 4;
    pub const DTWAIN_TWAINDSMSEARCH_NOTFOUND: i32 = -1;
    pub const DTWAIN_TWAINDSMSEARCH_WSO: i32 = 0;
    pub const DTWAIN_TWAINDSMSEARCH_WOS: i32 = 1;
    pub const DTWAIN_TWAINDSMSEARCH_SWO: i32 = 2;
    pub const DTWAIN_TWAINDSMSEARCH_SOW: i32 = 3;
    pub const DTWAIN_TWAINDSMSEARCH_OWS: i32 = 4;
    pub const DTWAIN_TWAINDSMSEARCH_OSW: i32 = 5;
    pub const DTWAIN_TWAINDSMSEARCH_W: i32 = 6;
    pub const DTWAIN_TWAINDSMSEARCH_S: i32 = 7;
    pub const DTWAIN_TWAINDSMSEARCH_O: i32 = 8;
    pub const DTWAIN_TWAINDSMSEARCH_WS: i32 = 9;
    pub const DTWAIN_TWAINDSMSEARCH_WO: i32 = 10;
    pub const DTWAIN_TWAINDSMSEARCH_SW: i32 = 11;
    pub const DTWAIN_TWAINDSMSEARCH_SO: i32 = 12;
    pub const DTWAIN_TWAINDSMSEARCH_OW: i32 = 13;
    pub const DTWAIN_TWAINDSMSEARCH_OS: i32 = 14;
    pub const DTWAIN_TWAINDSMSEARCH_C: i32 = 15;
    pub const DTWAIN_TWAINDSMSEARCH_U: i32 = 16;
    pub const DTWAIN_PDFPOLARITY_POSITIVE: i32 = 1;
    pub const DTWAIN_PDFPOLARITY_NEGATIVE: i32 = 2;
    pub const DTWAIN_TWPF_NORMAL: i32 = 0;
    pub const DTWAIN_TWPF_BOLD: i32 = 1;
    pub const DTWAIN_TWPF_ITALIC: i32 = 2;
    pub const DTWAIN_TWPF_LARGESIZE: i32 = 3;
    pub const DTWAIN_TWPF_SMALLSIZE: i32 = 4;
    pub const DTWAIN_TWCT_PAGE: i32 = 0;
    pub const DTWAIN_TWCT_PATCH1: i32 = 1;
    pub const DTWAIN_TWCT_PATCH2: i32 = 2;
    pub const DTWAIN_TWCT_PATCH3: i32 = 3;
    pub const DTWAIN_TWCT_PATCH4: i32 = 4;
    pub const DTWAIN_TWCT_PATCH5: i32 = 5;
    pub const DTWAIN_TWCT_PATCH6: i32 = 6;
    pub const DTWAIN_AUTOSIZE_NONE: i32 = 0;
    pub const DTWAIN_CV_CAPCUSTOMBASE: i32 = 0x8000;
    pub const DTWAIN_CV_CAPXFERCOUNT: i32 = 0x0001;
    pub const DTWAIN_CV_ICAPCOMPRESSION: i32 = 0x0100;
    pub const DTWAIN_CV_ICAPPIXELTYPE: i32 = 0x0101;
    pub const DTWAIN_CV_ICAPUNITS: i32 = 0x0102;
    pub const DTWAIN_CV_ICAPXFERMECH: i32 = 0x0103;
    pub const DTWAIN_CV_CAPAUTHOR: i32 = 0x1000;
    pub const DTWAIN_CV_CAPCAPTION: i32 = 0x1001;
    pub const DTWAIN_CV_CAPFEEDERENABLED: i32 = 0x1002;
    pub const DTWAIN_CV_CAPFEEDERLOADED: i32 = 0x1003;
    pub const DTWAIN_CV_CAPTIMEDATE: i32 = 0x1004;
    pub const DTWAIN_CV_CAPSUPPORTEDCAPS: i32 = 0x1005;
    pub const DTWAIN_CV_CAPEXTENDEDCAPS: i32 = 0x1006;
    pub const DTWAIN_CV_CAPAUTOFEED: i32 = 0x1007;
    pub const DTWAIN_CV_CAPCLEARPAGE: i32 = 0x1008;
    pub const DTWAIN_CV_CAPFEEDPAGE: i32 = 0x1009;
    pub const DTWAIN_CV_CAPREWINDPAGE: i32 = 0x100a;
    pub const DTWAIN_CV_CAPINDICATORS: i32 = 0x100b;
    pub const DTWAIN_CV_CAPSUPPORTEDCAPSEXT: i32 = 0x100c;
    pub const DTWAIN_CV_CAPPAPERDETECTABLE: i32 = 0x100d;
    pub const DTWAIN_CV_CAPUICONTROLLABLE: i32 = 0x100e;
    pub const DTWAIN_CV_CAPDEVICEONLINE: i32 = 0x100f;
    pub const DTWAIN_CV_CAPAUTOSCAN: i32 = 0x1010;
    pub const DTWAIN_CV_CAPTHUMBNAILSENABLED: i32 = 0x1011;
    pub const DTWAIN_CV_CAPDUPLEX: i32 = 0x1012;
    pub const DTWAIN_CV_CAPDUPLEXENABLED: i32 = 0x1013;
    pub const DTWAIN_CV_CAPENABLEDSUIONLY: i32 = 0x1014;
    pub const DTWAIN_CV_CAPCUSTOMDSDATA: i32 = 0x1015;
    pub const DTWAIN_CV_CAPENDORSER: i32 = 0x1016;
    pub const DTWAIN_CV_CAPJOBCONTROL: i32 = 0x1017;
    pub const DTWAIN_CV_CAPALARMS: i32 = 0x1018;
    pub const DTWAIN_CV_CAPALARMVOLUME: i32 = 0x1019;
    pub const DTWAIN_CV_CAPAUTOMATICCAPTURE: i32 = 0x101a;
    pub const DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE: i32 = 0x101b;
    pub const DTWAIN_CV_CAPTIMEBETWEENCAPTURES: i32 = 0x101c;
    pub const DTWAIN_CV_CAPCLEARBUFFERS: i32 = 0x101d;
    pub const DTWAIN_CV_CAPMAXBATCHBUFFERS: i32 = 0x101e;
    pub const DTWAIN_CV_CAPDEVICETIMEDATE: i32 = 0x101f;
    pub const DTWAIN_CV_CAPPOWERSUPPLY: i32 = 0x1020;
    pub const DTWAIN_CV_CAPCAMERAPREVIEWUI: i32 = 0x1021;
    pub const DTWAIN_CV_CAPDEVICEEVENT: i32 = 0x1022;
    pub const DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE: i32 = 0x1023;
    pub const DTWAIN_CV_CAPSERIALNUMBER: i32 = 0x1024;
    pub const DTWAIN_CV_CAPFILESYSTEM: i32 = 0x1025;
    pub const DTWAIN_CV_CAPPRINTER: i32 = 0x1026;
    pub const DTWAIN_CV_CAPPRINTERENABLED: i32 = 0x1027;
    pub const DTWAIN_CV_CAPPRINTERINDEX: i32 = 0x1028;
    pub const DTWAIN_CV_CAPPRINTERMODE: i32 = 0x1029;
    pub const DTWAIN_CV_CAPPRINTERSTRING: i32 = 0x102a;
    pub const DTWAIN_CV_CAPPRINTERSUFFIX: i32 = 0x102b;
    pub const DTWAIN_CV_CAPLANGUAGE: i32 = 0x102c;
    pub const DTWAIN_CV_CAPFEEDERALIGNMENT: i32 = 0x102d;
    pub const DTWAIN_CV_CAPFEEDERORDER: i32 = 0x102e;
    pub const DTWAIN_CV_CAPPAPERBINDING: i32 = 0x102f;
    pub const DTWAIN_CV_CAPREACQUIREALLOWED: i32 = 0x1030;
    pub const DTWAIN_CV_CAPPASSTHRU: i32 = 0x1031;
    pub const DTWAIN_CV_CAPBATTERYMINUTES: i32 = 0x1032;
    pub const DTWAIN_CV_CAPBATTERYPERCENTAGE: i32 = 0x1033;
    pub const DTWAIN_CV_CAPPOWERDOWNTIME: i32 = 0x1034;
    pub const DTWAIN_CV_CAPSEGMENTED: i32 = 0x1035;
    pub const DTWAIN_CV_CAPCAMERAENABLED: i32 = 0x1036;
    pub const DTWAIN_CV_CAPCAMERAORDER: i32 = 0x1037;
    pub const DTWAIN_CV_CAPMICRENABLED: i32 = 0x1038;
    pub const DTWAIN_CV_CAPFEEDERPREP: i32 = 0x1039;
    pub const DTWAIN_CV_CAPFEEDERPOCKET: i32 = 0x103a;
    pub const DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM: i32 = 0x103b;
    pub const DTWAIN_CV_CAPCUSTOMINTERFACEGUID: i32 = 0x103c;
    pub const DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE: i32 = 0x103d;
    pub const DTWAIN_CV_CAPSUPPORTEDDATS: i32 = 0x103e;
    pub const DTWAIN_CV_CAPDOUBLEFEEDDETECTION: i32 = 0x103f;
    pub const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH: i32 = 0x1040;
    pub const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY: i32 = 0x1041;
    pub const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE: i32 = 0x1042;
    pub const DTWAIN_CV_CAPPAPERHANDLING: i32 = 0x1043;
    pub const DTWAIN_CV_CAPINDICATORSMODE: i32 = 0x1044;
    pub const DTWAIN_CV_CAPPRINTERVERTICALOFFSET: i32 = 0x1045;
    pub const DTWAIN_CV_CAPPOWERSAVETIME: i32 = 0x1046;
    pub const DTWAIN_CV_CAPPRINTERCHARROTATION: i32 = 0x1047;
    pub const DTWAIN_CV_CAPPRINTERFONTSTYLE: i32 = 0x1048;
    pub const DTWAIN_CV_CAPPRINTERINDEXLEADCHAR: i32 = 0x1049;
    pub const DTWAIN_CV_CAPIMAGEADDRESSENABLED: i32 = 0x1050;
    pub const DTWAIN_CV_CAPIAFIELDA_LEVEL: i32 = 0x1051;
    pub const DTWAIN_CV_CAPIAFIELDB_LEVEL: i32 = 0x1052;
    pub const DTWAIN_CV_CAPIAFIELDC_LEVEL: i32 = 0x1053;
    pub const DTWAIN_CV_CAPIAFIELDD_LEVEL: i32 = 0x1054;
    pub const DTWAIN_CV_CAPIAFIELDE_LEVEL: i32 = 0x1055;
    pub const DTWAIN_CV_CAPIAFIELDA_PRINTFORMAT: i32 = 0x1056;
    pub const DTWAIN_CV_CAPIAFIELDB_PRINTFORMAT: i32 = 0x1057;
    pub const DTWAIN_CV_CAPIAFIELDC_PRINTFORMAT: i32 = 0x1058;
    pub const DTWAIN_CV_CAPIAFIELDD_PRINTFORMAT: i32 = 0x1059;
    pub const DTWAIN_CV_CAPIAFIELDE_PRINTFORMAT: i32 = 0x105A;
    pub const DTWAIN_CV_CAPIAFIELDA_VALUE: i32 = 0x105B;
    pub const DTWAIN_CV_CAPIAFIELDB_VALUE: i32 = 0x105C;
    pub const DTWAIN_CV_CAPIAFIELDC_VALUE: i32 = 0x105D;
    pub const DTWAIN_CV_CAPIAFIELDD_VALUE: i32 = 0x105E;
    pub const DTWAIN_CV_CAPIAFIELDE_VALUE: i32 = 0x105F;
    pub const DTWAIN_CV_CAPIAFIELDA_LASTPAGE: i32 = 0x1060;
    pub const DTWAIN_CV_CAPIAFIELDB_LASTPAGE: i32 = 0x1061;
    pub const DTWAIN_CV_CAPIAFIELDC_LASTPAGE: i32 = 0x1062;
    pub const DTWAIN_CV_CAPIAFIELDD_LASTPAGE: i32 = 0x1063;
    pub const DTWAIN_CV_CAPIAFIELDE_LASTPAGE: i32 = 0x1064;
    pub const DTWAIN_CV_CAPPRINTERINDEXMAXVALUE: i32 = 0x104A;
    pub const DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS: i32 = 0x104B;
    pub const DTWAIN_CV_CAPPRINTERINDEXSTEP: i32 = 0x104C;
    pub const DTWAIN_CV_CAPPRINTERINDEXTRIGGER: i32 = 0x104D;
    pub const DTWAIN_CV_CAPPRINTERSTRINGPREVIEW: i32 = 0x104E;
    pub const DTWAIN_CV_ICAPAUTOBRIGHT: i32 = 0x1100;
    pub const DTWAIN_CV_ICAPBRIGHTNESS: i32 = 0x1101;
    pub const DTWAIN_CV_ICAPCONTRAST: i32 = 0x1103;
    pub const DTWAIN_CV_ICAPCUSTHALFTONE: i32 = 0x1104;
    pub const DTWAIN_CV_ICAPEXPOSURETIME: i32 = 0x1105;
    pub const DTWAIN_CV_ICAPFILTER: i32 = 0x1106;
    pub const DTWAIN_CV_ICAPFLASHUSED: i32 = 0x1107;
    pub const DTWAIN_CV_ICAPGAMMA: i32 = 0x1108;
    pub const DTWAIN_CV_ICAPHALFTONES: i32 = 0x1109;
    pub const DTWAIN_CV_ICAPHIGHLIGHT: i32 = 0x110a;
    pub const DTWAIN_CV_ICAPIMAGEFILEFORMAT: i32 = 0x110c;
    pub const DTWAIN_CV_ICAPLAMPSTATE: i32 = 0x110d;
    pub const DTWAIN_CV_ICAPLIGHTSOURCE: i32 = 0x110e;
    pub const DTWAIN_CV_ICAPORIENTATION: i32 = 0x1110;
    pub const DTWAIN_CV_ICAPPHYSICALWIDTH: i32 = 0x1111;
    pub const DTWAIN_CV_ICAPPHYSICALHEIGHT: i32 = 0x1112;
    pub const DTWAIN_CV_ICAPSHADOW: i32 = 0x1113;
    pub const DTWAIN_CV_ICAPFRAMES: i32 = 0x1114;
    pub const DTWAIN_CV_ICAPXNATIVERESOLUTION: i32 = 0x1116;
    pub const DTWAIN_CV_ICAPYNATIVERESOLUTION: i32 = 0x1117;
    pub const DTWAIN_CV_ICAPXRESOLUTION: i32 = 0x1118;
    pub const DTWAIN_CV_ICAPYRESOLUTION: i32 = 0x1119;
    pub const DTWAIN_CV_ICAPMAXFRAMES: i32 = 0x111a;
    pub const DTWAIN_CV_ICAPTILES: i32 = 0x111b;
    pub const DTWAIN_CV_ICAPBITORDER: i32 = 0x111c;
    pub const DTWAIN_CV_ICAPCCITTKFACTOR: i32 = 0x111d;
    pub const DTWAIN_CV_ICAPLIGHTPATH: i32 = 0x111e;
    pub const DTWAIN_CV_ICAPPIXELFLAVOR: i32 = 0x111f;
    pub const DTWAIN_CV_ICAPPLANARCHUNKY: i32 = 0x1120;
    pub const DTWAIN_CV_ICAPROTATION: i32 = 0x1121;
    pub const DTWAIN_CV_ICAPSUPPORTEDSIZES: i32 = 0x1122;
    pub const DTWAIN_CV_ICAPTHRESHOLD: i32 = 0x1123;
    pub const DTWAIN_CV_ICAPXSCALING: i32 = 0x1124;
    pub const DTWAIN_CV_ICAPYSCALING: i32 = 0x1125;
    pub const DTWAIN_CV_ICAPBITORDERCODES: i32 = 0x1126;
    pub const DTWAIN_CV_ICAPPIXELFLAVORCODES: i32 = 0x1127;
    pub const DTWAIN_CV_ICAPJPEGPIXELTYPE: i32 = 0x1128;
    pub const DTWAIN_CV_ICAPTIMEFILL: i32 = 0x112a;
    pub const DTWAIN_CV_ICAPBITDEPTH: i32 = 0x112b;
    pub const DTWAIN_CV_ICAPBITDEPTHREDUCTION: i32 = 0x112c;
    pub const DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE: i32 = 0x112d;
    pub const DTWAIN_CV_ICAPIMAGEDATASET: i32 = 0x112e;
    pub const DTWAIN_CV_ICAPEXTIMAGEINFO: i32 = 0x112f;
    pub const DTWAIN_CV_ICAPMINIMUMHEIGHT: i32 = 0x1130;
    pub const DTWAIN_CV_ICAPMINIMUMWIDTH: i32 = 0x1131;
    pub const DTWAIN_CV_ICAPAUTOBORDERDETECTION: i32 = 0x1132;
    pub const DTWAIN_CV_ICAPAUTODESKEW: i32 = 0x1133;
    pub const DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES: i32 = 0x1134;
    pub const DTWAIN_CV_ICAPAUTOROTATE: i32 = 0x1135;
    pub const DTWAIN_CV_ICAPFLIPROTATION: i32 = 0x1136;
    pub const DTWAIN_CV_ICAPBARCODEDETECTIONENABLED: i32 = 0x1137;
    pub const DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES: i32 = 0x1138;
    pub const DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES: i32 = 0x1139;
    pub const DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES: i32 = 0x113a;
    pub const DTWAIN_CV_ICAPBARCODESEARCHMODE: i32 = 0x113b;
    pub const DTWAIN_CV_ICAPBARCODEMAXRETRIES: i32 = 0x113c;
    pub const DTWAIN_CV_ICAPBARCODETIMEOUT: i32 = 0x113d;
    pub const DTWAIN_CV_ICAPZOOMFACTOR: i32 = 0x113e;
    pub const DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED: i32 = 0x113f;
    pub const DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES: i32 = 0x1140;
    pub const DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES: i32 = 0x1141;
    pub const DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES: i32 = 0x1142;
    pub const DTWAIN_CV_ICAPPATCHCODESEARCHMODE: i32 = 0x1143;
    pub const DTWAIN_CV_ICAPPATCHCODEMAXRETRIES: i32 = 0x1144;
    pub const DTWAIN_CV_ICAPPATCHCODETIMEOUT: i32 = 0x1145;
    pub const DTWAIN_CV_ICAPFLASHUSED2: i32 = 0x1146;
    pub const DTWAIN_CV_ICAPIMAGEFILTER: i32 = 0x1147;
    pub const DTWAIN_CV_ICAPNOISEFILTER: i32 = 0x1148;
    pub const DTWAIN_CV_ICAPOVERSCAN: i32 = 0x1149;
    pub const DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION: i32 = 0x1150;
    pub const DTWAIN_CV_ICAPAUTOMATICDESKEW: i32 = 0x1151;
    pub const DTWAIN_CV_ICAPAUTOMATICROTATE: i32 = 0x1152;
    pub const DTWAIN_CV_ICAPJPEGQUALITY: i32 = 0x1153;
    pub const DTWAIN_CV_ICAPFEEDERTYPE: i32 = 0x1154;
    pub const DTWAIN_CV_ICAPICCPROFILE: i32 = 0x1155;
    pub const DTWAIN_CV_ICAPAUTOSIZE: i32 = 0x1156;
    pub const DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME: i32 = 0x1157;
    pub const DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION: i32 = 0x1158;
    pub const DTWAIN_CV_ICAPAUTOMATICCOLORENABLED: i32 = 0x1159;
    pub const DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE: i32 = 0x115a;
    pub const DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED: i32 = 0x115b;
    pub const DTWAIN_CV_ICAPIMAGEMERGE: i32 = 0x115c;
    pub const DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD: i32 = 0x115d;
    pub const DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO: i32 = 0x115e;
    pub const DTWAIN_CV_ICAPFILMTYPE: i32 = 0x115f;
    pub const DTWAIN_CV_ICAPMIRROR: i32 = 0x1160;
    pub const DTWAIN_CV_ICAPJPEGSUBSAMPLING: i32 = 0x1161;
    pub const DTWAIN_CV_ACAPAUDIOFILEFORMAT: i32 = 0x1201;
    pub const DTWAIN_CV_ACAPXFERMECH: i32 = 0x1202;
    pub const DTWAIN_CFMCV_CAPCFMSTART: i32 = 2048;
    pub const DTWAIN_CFMCV_CAPDUPLEXSCANNER: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+10;
    pub const DTWAIN_CFMCV_CAPDUPLEXENABLE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+11;
    pub const DTWAIN_CFMCV_CAPSCANNERNAME: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+12;
    pub const DTWAIN_CFMCV_CAPSINGLEPASS: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+13;
    pub const DTWAIN_CFMCV_CAPERRHANDLING: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+20;
    pub const DTWAIN_CFMCV_CAPFEEDERSTATUS: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+21;
    pub const DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+22;
    pub const DTWAIN_CFMCV_CAPFEEDWAITTIME: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+23;
    pub const DTWAIN_CFMCV_ICAPWHITEBALANCE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+24;
    pub const DTWAIN_CFMCV_ICAPAUTOBINARY: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+25;
    pub const DTWAIN_CFMCV_ICAPIMAGESEPARATION: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+26;
    pub const DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+27;
    pub const DTWAIN_CFMCV_ICAPIMAGEEMPHASIS: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+28;
    pub const DTWAIN_CFMCV_ICAPOUTLINING: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+29;
    pub const DTWAIN_CFMCV_ICAPDYNTHRESHOLD: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+30;
    pub const DTWAIN_CFMCV_ICAPVARIANCE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+31;
    pub const DTWAIN_CFMCV_CAPENDORSERAVAILABLE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+32;
    pub const DTWAIN_CFMCV_CAPENDORSERENABLE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+33;
    pub const DTWAIN_CFMCV_CAPENDORSERCHARSET: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+34;
    pub const DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+35;
    pub const DTWAIN_CFMCV_CAPENDORSERSTRING: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+36;
    pub const DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+48;
    pub const DTWAIN_CFMCV_ICAPSMOOTHINGMODE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+49;
    pub const DTWAIN_CFMCV_ICAPFILTERMODE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+50;
    pub const DTWAIN_CFMCV_ICAPGRADATION: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+51;
    pub const DTWAIN_CFMCV_ICAPMIRROR: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+52;
    pub const DTWAIN_CFMCV_ICAPEASYSCANMODE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+53;
    pub const DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+54;
    pub const DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+55;
    pub const DTWAIN_CFMCV_CAPDUPLEXPAGE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+56;
    pub const DTWAIN_CFMCV_ICAPINVERTIMAGE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+57;
    pub const DTWAIN_CFMCV_ICAPSPECKLEREMOVE: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+58;
    pub const DTWAIN_CFMCV_ICAPUSMFILTER: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+59;
    pub const DTWAIN_CFMCV_ICAPNOISEFILTERCFM: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+60;
    pub const DTWAIN_CFMCV_ICAPDESCREENING: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+61;
    pub const DTWAIN_CFMCV_ICAPQUALITYFILTER: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+62;
    pub const DTWAIN_CFMCV_ICAPBINARYFILTER: i32 = DTwainAPI::DTWAIN_CV_CAPCUSTOMBASE+DTwainAPI::DTWAIN_CFMCV_CAPCFMSTART+63;
    pub const DTWAIN_OCRCV_IMAGEFILEFORMAT: i32 = 0x1000;
    pub const DTWAIN_OCRCV_DESKEW: i32 = 0x1001;
    pub const DTWAIN_OCRCV_DESHADE: i32 = 0x1002;
    pub const DTWAIN_OCRCV_ORIENTATION: i32 = 0x1003;
    pub const DTWAIN_OCRCV_NOISEREMOVE: i32 = 0x1004;
    pub const DTWAIN_OCRCV_LINEREMOVE: i32 = 0x1005;
    pub const DTWAIN_OCRCV_INVERTPAGE: i32 = 0x1006;
    pub const DTWAIN_OCRCV_INVERTZONES: i32 = 0x1007;
    pub const DTWAIN_OCRCV_LINEREJECT: i32 = 0x1008;
    pub const DTWAIN_OCRCV_CHARACTERREJECT: i32 = 0x1009;
    pub const DTWAIN_OCRCV_ERRORREPORTMODE: i32 = 0x1010;
    pub const DTWAIN_OCRCV_ERRORREPORTFILE: i32 = 0x1011;
    pub const DTWAIN_OCRCV_PIXELTYPE: i32 = 0x1012;
    pub const DTWAIN_OCRCV_BITDEPTH: i32 = 0x1013;
    pub const DTWAIN_OCRCV_RETURNCHARINFO: i32 = 0x1014;
    pub const DTWAIN_OCRCV_NATIVEFILEFORMAT: i32 = 0x1015;
    pub const DTWAIN_OCRCV_MPNATIVEFILEFORMAT: i32 = 0x1016;
    pub const DTWAIN_OCRCV_SUPPORTEDCAPS: i32 = 0x1017;
    pub const DTWAIN_OCRCV_DISABLECHARACTERS: i32 = 0x1018;
    pub const DTWAIN_OCRCV_REMOVECONTROLCHARS: i32 = 0x1019;
    pub const DTWAIN_OCRORIENT_OFF: i32 = 0;
    pub const DTWAIN_OCRORIENT_AUTO: i32 = 1;
    pub const DTWAIN_OCRORIENT_90: i32 = 2;
    pub const DTWAIN_OCRORIENT_180: i32 = 3;
    pub const DTWAIN_OCRORIENT_270: i32 = 4;
    pub const DTWAIN_OCRIMAGEFORMAT_AUTO: i32 = 10000;
    pub const DTWAIN_OCRERROR_MODENONE: i32 = 0;
    pub const DTWAIN_OCRERROR_SHOWMSGBOX: i32 = 1;
    pub const DTWAIN_OCRERROR_WRITEFILE: i32 = 2;
    pub const DTWAIN_PDFTEXT_ALLPAGES: u32 = 0x00000001;
    pub const DTWAIN_PDFTEXT_EVENPAGES: u32 = 0x00000002;
    pub const DTWAIN_PDFTEXT_ODDPAGES: u32 = 0x00000004;
    pub const DTWAIN_PDFTEXT_FIRSTPAGE: u32 = 0x00000008;
    pub const DTWAIN_PDFTEXT_LASTPAGE: u32 = 0x00000010;
    pub const DTWAIN_PDFTEXT_CURRENTPAGE: u32 = 0x00000020;
    pub const DTWAIN_PDFTEXT_DISABLED: u32 = 0x00000040;
    pub const DTWAIN_PDFTEXT_TOPLEFT: u32 = 0x00000100;
    pub const DTWAIN_PDFTEXT_TOPRIGHT: u32 = 0x00000200;
    pub const DTWAIN_PDFTEXT_HORIZCENTER: u32 = 0x00000400;
    pub const DTWAIN_PDFTEXT_VERTCENTER: u32 = 0x00000800;
    pub const DTWAIN_PDFTEXT_BOTTOMLEFT: u32 = 0x00001000;
    pub const DTWAIN_PDFTEXT_BOTTOMRIGHT: u32 = 0x00002000;
    pub const DTWAIN_PDFTEXT_BOTTOMCENTER: u32 = 0x00004000;
    pub const DTWAIN_PDFTEXT_TOPCENTER: u32 = 0x00008000;
    pub const DTWAIN_PDFTEXT_XCENTER: u32 = 0x00010000;
    pub const DTWAIN_PDFTEXT_YCENTER: u32 = 0x00020000;
    pub const DTWAIN_PDFTEXT_NOSCALING: u32 = 0x00100000;
    pub const DTWAIN_PDFTEXT_NOCHARSPACING: u32 = 0x00200000;
    pub const DTWAIN_PDFTEXT_NOWORDSPACING: u32 = 0x00400000;
    pub const DTWAIN_PDFTEXT_NOSTROKEWIDTH: u32 = 0x00800000;
    pub const DTWAIN_PDFTEXT_NORENDERMODE: u32 = 0x01000000;
    pub const DTWAIN_PDFTEXT_NORGBCOLOR: u32 = 0x02000000;
    pub const DTWAIN_PDFTEXT_NOFONTSIZE: u32 = 0x04000000;
    pub const DTWAIN_PDFTEXT_NOABSPOSITION: u32 = 0x08000000;
    pub const DTWAIN_PDFTEXT_IGNOREALL: u32 = 0xFFF00000;
    pub const DTWAIN_FONT_COURIER: i32 = 0;
    pub const DTWAIN_FONT_COURIERBOLD: i32 = 1;
    pub const DTWAIN_FONT_COURIERBOLDOBLIQUE: i32 = 2;
    pub const DTWAIN_FONT_COURIEROBLIQUE: i32 = 3;
    pub const DTWAIN_FONT_HELVETICA: i32 = 4;
    pub const DTWAIN_FONT_HELVETICABOLD: i32 = 5;
    pub const DTWAIN_FONT_HELVETICABOLDOBLIQUE: i32 = 6;
    pub const DTWAIN_FONT_HELVETICAOBLIQUE: i32 = 7;
    pub const DTWAIN_FONT_TIMESBOLD: i32 = 8;
    pub const DTWAIN_FONT_TIMESBOLDITALIC: i32 = 9;
    pub const DTWAIN_FONT_TIMESROMAN: i32 = 10;
    pub const DTWAIN_FONT_TIMESITALIC: i32 = 11;
    pub const DTWAIN_FONT_SYMBOL: i32 = 12;
    pub const DTWAIN_FONT_ZAPFDINGBATS: i32 = 13;
    pub const DTWAIN_PDFRENDER_FILL: i32 = 0;
    pub const DTWAIN_PDFRENDER_STROKE: i32 = 1;
    pub const DTWAIN_PDFRENDER_FILLSTROKE: i32 = 2;
    pub const DTWAIN_PDFRENDER_INVISIBLE: i32 = 3;
    pub const DTWAIN_PDFTEXTELEMENT_SCALINGXY: i32 = 0;
    pub const DTWAIN_PDFTEXTELEMENT_FONTHEIGHT: i32 = 1;
    pub const DTWAIN_PDFTEXTELEMENT_WORDSPACING: i32 = 2;
    pub const DTWAIN_PDFTEXTELEMENT_POSITION: i32 = 3;
    pub const DTWAIN_PDFTEXTELEMENT_COLOR: i32 = 4;
    pub const DTWAIN_PDFTEXTELEMENT_STROKEWIDTH: i32 = 5;
    pub const DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS: i32 = 6;
    pub const DTWAIN_PDFTEXTELEMENT_FONTNAME: i32 = 7;
    pub const DTWAIN_PDFTEXTELEMENT_TEXT: i32 = 8;
    pub const DTWAIN_PDFTEXTELEMENT_RENDERMODE: i32 = 9;
    pub const DTWAIN_PDFTEXTELEMENT_CHARSPACING: i32 = 10;
    pub const DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE: i32 = 11;
    pub const DTWAIN_PDFTEXTELEMENT_LEADING: i32 = 12;
    pub const DTWAIN_PDFTEXTELEMENT_SCALING: i32 = 13;
    pub const DTWAIN_PDFTEXTELEMENT_TEXTLENGTH: i32 = 14;
    pub const DTWAIN_PDFTEXTELEMENT_SKEWANGLES: i32 = 15;
    pub const DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER: i32 = 16;
    pub const DTWAIN_PDFTEXTTRANSFORM_TSRK: i32 = 0;
    pub const DTWAIN_PDFTEXTTRANSFORM_TSKR: i32 = 1;
    pub const DTWAIN_PDFTEXTTRANSFORM_TKSR: i32 = 2;
    pub const DTWAIN_PDFTEXTTRANSFORM_TKRS: i32 = 3;
    pub const DTWAIN_PDFTEXTTRANSFORM_TRSK: i32 = 4;
    pub const DTWAIN_PDFTEXTTRANSFORM_TRKS: i32 = 5;
    pub const DTWAIN_PDFTEXTTRANSFORM_STRK: i32 = 6;
    pub const DTWAIN_PDFTEXTTRANSFORM_STKR: i32 = 7;
    pub const DTWAIN_PDFTEXTTRANSFORM_SKTR: i32 = 8;
    pub const DTWAIN_PDFTEXTTRANSFORM_SKRT: i32 = 9;
    pub const DTWAIN_PDFTEXTTRANSFORM_SRTK: i32 = 10;
    pub const DTWAIN_PDFTEXTTRANSFORM_SRKT: i32 = 11;
    pub const DTWAIN_PDFTEXTTRANSFORM_RSTK: i32 = 12;
    pub const DTWAIN_PDFTEXTTRANSFORM_RSKT: i32 = 13;
    pub const DTWAIN_PDFTEXTTRANSFORM_RTSK: i32 = 14;
    pub const DTWAIN_PDFTEXTTRANSFORM_RTKT: i32 = 15;
    pub const DTWAIN_PDFTEXTTRANSFORM_RKST: i32 = 16;
    pub const DTWAIN_PDFTEXTTRANSFORM_RKTS: i32 = 17;
    pub const DTWAIN_PDFTEXTTRANSFORM_KSTR: i32 = 18;
    pub const DTWAIN_PDFTEXTTRANSFORM_KSRT: i32 = 19;
    pub const DTWAIN_PDFTEXTTRANSFORM_KRST: i32 = 20;
    pub const DTWAIN_PDFTEXTTRANSFORM_KRTS: i32 = 21;
    pub const DTWAIN_PDFTEXTTRANSFORM_KTSR: i32 = 22;
    pub const DTWAIN_PDFTEXTTRANSFORM_KTRS: i32 = 23;
    pub const DTWAIN_PDFTEXTTRANFORM_LAST: i32 = DTwainAPI::DTWAIN_PDFTEXTTRANSFORM_KTRS;
    pub const DTWAIN_TWDF_ULTRASONIC: i32 = 0;
    pub const DTWAIN_TWDF_BYLENGTH: i32 = 1;
    pub const DTWAIN_TWDF_INFRARED: i32 = 2;
    pub const DTWAIN_TWAS_NONE: i32 = 0;
    pub const DTWAIN_TWAS_AUTO: i32 = 1;
    pub const DTWAIN_TWAS_CURRENT: i32 = 2;
    pub const DTWAIN_TWFR_BOOK: i32 = 0;
    pub const DTWAIN_TWFR_FANFOLD: i32 = 1;
    pub const DTWAIN_CONSTANT_TWPT: i32 = 0 ;
    pub const DTWAIN_CONSTANT_TWUN: i32 = 1 ;
    pub const DTWAIN_CONSTANT_TWCY: i32 = 2 ;
    pub const DTWAIN_CONSTANT_TWAL: i32 = 3 ;
    pub const DTWAIN_CONSTANT_TWAS: i32 = 4 ;
    pub const DTWAIN_CONSTANT_TWBCOR: i32 = 5 ;
    pub const DTWAIN_CONSTANT_TWBD: i32 = 6 ;
    pub const DTWAIN_CONSTANT_TWBO: i32 = 7 ;
    pub const DTWAIN_CONSTANT_TWBP: i32 = 8 ;
    pub const DTWAIN_CONSTANT_TWBR: i32 = 9 ;
    pub const DTWAIN_CONSTANT_TWBT: i32 = 10;
    pub const DTWAIN_CONSTANT_TWCP: i32 = 11;
    pub const DTWAIN_CONSTANT_TWCS: i32 = 12;
    pub const DTWAIN_CONSTANT_TWDE: i32 = 13;
    pub const DTWAIN_CONSTANT_TWDR: i32 = 14;
    pub const DTWAIN_CONSTANT_TWDSK: i32 = 15;
    pub const DTWAIN_CONSTANT_TWDX: i32 = 16;
    pub const DTWAIN_CONSTANT_TWFA: i32 = 17;
    pub const DTWAIN_CONSTANT_TWFE: i32 = 18;
    pub const DTWAIN_CONSTANT_TWFF: i32 = 19;
    pub const DTWAIN_CONSTANT_TWFL: i32 = 20;
    pub const DTWAIN_CONSTANT_TWFO: i32 = 21;
    pub const DTWAIN_CONSTANT_TWFP: i32 = 22;
    pub const DTWAIN_CONSTANT_TWFR: i32 = 23;
    pub const DTWAIN_CONSTANT_TWFT: i32 = 24;
    pub const DTWAIN_CONSTANT_TWFY: i32 = 25;
    pub const DTWAIN_CONSTANT_TWIA: i32 = 26;
    pub const DTWAIN_CONSTANT_TWIC: i32 = 27;
    pub const DTWAIN_CONSTANT_TWIF: i32 = 28;
    pub const DTWAIN_CONSTANT_TWIM: i32 = 29;
    pub const DTWAIN_CONSTANT_TWJC: i32 = 30;
    pub const DTWAIN_CONSTANT_TWJQ: i32 = 31;
    pub const DTWAIN_CONSTANT_TWLP: i32 = 32;
    pub const DTWAIN_CONSTANT_TWLS: i32 = 33;
    pub const DTWAIN_CONSTANT_TWMD: i32 = 34;
    pub const DTWAIN_CONSTANT_TWNF: i32 = 35;
    pub const DTWAIN_CONSTANT_TWOR: i32 = 36;
    pub const DTWAIN_CONSTANT_TWOV: i32 = 37;
    pub const DTWAIN_CONSTANT_TWPA: i32 = 38;
    pub const DTWAIN_CONSTANT_TWPC: i32 = 39;
    pub const DTWAIN_CONSTANT_TWPCH: i32 = 40;
    pub const DTWAIN_CONSTANT_TWPF: i32 = 41;
    pub const DTWAIN_CONSTANT_TWPM: i32 = 42;
    pub const DTWAIN_CONSTANT_TWPR: i32 = 43;
    pub const DTWAIN_CONSTANT_TWPF2: i32 = 44;
    pub const DTWAIN_CONSTANT_TWCT: i32 = 45;
    pub const DTWAIN_CONSTANT_TWPS: i32 = 46;
    pub const DTWAIN_CONSTANT_TWSS: i32 = 47;
    pub const DTWAIN_CONSTANT_TWPH: i32 = 48;
    pub const DTWAIN_CONSTANT_TWCI: i32 = 49;
    pub const DTWAIN_CONSTANT_FONTNAME: i32 = 50;
    pub const DTWAIN_CONSTANT_TWEI: i32 = 51;
    pub const DTWAIN_CONSTANT_TWEJ: i32 = 52;
    pub const DTWAIN_CONSTANT_TWCC: i32 = 53;
    pub const DTWAIN_CONSTANT_TWQC: i32 = 54;
    pub const DTWAIN_CONSTANT_TWRC: i32 = 55;
    pub const DTWAIN_CONSTANT_MSG: i32 = 56;
    pub const DTWAIN_CONSTANT_TWLG: i32 = 57;
    pub const DTWAIN_CONSTANT_DLLINFO: i32 = 58;
    pub const DTWAIN_CONSTANT_DG: i32 = 59;
    pub const DTWAIN_CONSTANT_DAT: i32 = 60;
    pub const DTWAIN_CONSTANT_DF: i32 = 61;
    pub const DTWAIN_CONSTANT_TWTY: i32 = 62;
    pub const DTWAIN_CONSTANT_TWCB: i32 = 63;
    pub const DTWAIN_CONSTANT_TWAF: i32 = 64;
    pub const DTWAIN_CONSTANT_TWFS: i32 = 65;
    pub const DTWAIN_CONSTANT_TWJS: i32 = 66;
    pub const DTWAIN_CONSTANT_TWMR: i32 = 67;
    pub const DTWAIN_CONSTANT_TWDP: i32 = 68;
    pub const DTWAIN_CONSTANT_TWUS: i32 = 69;
    pub const DTWAIN_CONSTANT_TWDF: i32 = 70;
    pub const DTWAIN_CONSTANT_TWFM: i32 = 71;
    pub const DTWAIN_CONSTANT_TWSG: i32 = 72;
    pub const DTWAIN_CONSTANT_DTWAIN_TN: i32 = 73;
    pub const DTWAIN_CONSTANT_TWON: i32 = 74;
    pub const DTWAIN_CONSTANT_TWMF: i32 = 75;
    pub const DTWAIN_CONSTANT_TWSX: i32 = 76;
    pub const DTWAIN_CONSTANT_CAP: i32 = 77;
    pub const DTWAIN_CONSTANT_ICAP: i32 = 78;
    pub const DTWAIN_CONSTANT_DTWAIN_CONT: i32 = 79;
    pub const DTWAIN_CONSTANT_CAPCODE_MAP: i32 = 80;
    pub const DTWAIN_USERRES_START: i32 = 20000;
    pub const DTWAIN_USERRES_MAXSIZE: i32 = 8192;
    pub const DTWAIN_APIHANDLEOK: i32 = 1;
    pub const DTWAIN_TWAINSESSIONOK: i32 = 2;
    pub const DTWAIN_PDF_AES128: i32 = 1;
    pub const DTWAIN_PDF_AES256: i32 = 2;
    pub const DTWAIN_FEEDER_TERMINATE: i32 = 1;
    pub const DTWAIN_FEEDER_USEFLATBED: i32 = 2;

    pub fn new(library: &'a Library) -> Result<Self, Box<dyn std::error::Error>>
    {
        let DTWAIN_AcquireAudioFile: Symbol<DtwainacquireaudiofileFunc> = unsafe { library.get(b"DTWAIN_AcquireAudioFile")? };
        let DTWAIN_AcquireAudioFileA: Symbol<DtwainacquireaudiofileaFunc> = unsafe { library.get(b"DTWAIN_AcquireAudioFileA")? };
        let DTWAIN_AcquireAudioFileW: Symbol<DtwainacquireaudiofilewFunc> = unsafe { library.get(b"DTWAIN_AcquireAudioFileW")? };
        let DTWAIN_AcquireAudioNative: Symbol<DtwainacquireaudionativeFunc> = unsafe { library.get(b"DTWAIN_AcquireAudioNative")? };
        let DTWAIN_AcquireAudioNativeEx: Symbol<DtwainacquireaudionativeexFunc> = unsafe { library.get(b"DTWAIN_AcquireAudioNativeEx")? };
        let DTWAIN_AcquireBuffered: Symbol<DtwainacquirebufferedFunc> = unsafe { library.get(b"DTWAIN_AcquireBuffered")? };
        let DTWAIN_AcquireBufferedEx: Symbol<DtwainacquirebufferedexFunc> = unsafe { library.get(b"DTWAIN_AcquireBufferedEx")? };
        let DTWAIN_AcquireFile: Symbol<DtwainacquirefileFunc> = unsafe { library.get(b"DTWAIN_AcquireFile")? };
        let DTWAIN_AcquireFileA: Symbol<DtwainacquirefileaFunc> = unsafe { library.get(b"DTWAIN_AcquireFileA")? };
        let DTWAIN_AcquireFileEx: Symbol<DtwainacquirefileexFunc> = unsafe { library.get(b"DTWAIN_AcquireFileEx")? };
        let DTWAIN_AcquireFileW: Symbol<DtwainacquirefilewFunc> = unsafe { library.get(b"DTWAIN_AcquireFileW")? };
        let DTWAIN_AcquireNative: Symbol<DtwainacquirenativeFunc> = unsafe { library.get(b"DTWAIN_AcquireNative")? };
        let DTWAIN_AcquireNativeEx: Symbol<DtwainacquirenativeexFunc> = unsafe { library.get(b"DTWAIN_AcquireNativeEx")? };
        let DTWAIN_AcquireToClipboard: Symbol<DtwainacquiretoclipboardFunc> = unsafe { library.get(b"DTWAIN_AcquireToClipboard")? };
        let DTWAIN_AddExtImageInfoQuery: Symbol<DtwainaddextimageinfoqueryFunc> = unsafe { library.get(b"DTWAIN_AddExtImageInfoQuery")? };
        let DTWAIN_AddPDFText: Symbol<DtwainaddpdftextFunc> = unsafe { library.get(b"DTWAIN_AddPDFText")? };
        let DTWAIN_AddPDFTextA: Symbol<DtwainaddpdftextaFunc> = unsafe { library.get(b"DTWAIN_AddPDFTextA")? };
        let DTWAIN_AddPDFTextEx: Symbol<DtwainaddpdftextexFunc> = unsafe { library.get(b"DTWAIN_AddPDFTextEx")? };
        let DTWAIN_AddPDFTextW: Symbol<DtwainaddpdftextwFunc> = unsafe { library.get(b"DTWAIN_AddPDFTextW")? };
        let DTWAIN_AllocateMemory: Symbol<DtwainallocatememoryFunc> = unsafe { library.get(b"DTWAIN_AllocateMemory")? };
        let DTWAIN_AllocateMemory64: Symbol<Dtwainallocatememory64Func> = unsafe { library.get(b"DTWAIN_AllocateMemory64")? };
        let DTWAIN_AllocateMemoryEx: Symbol<DtwainallocatememoryexFunc> = unsafe { library.get(b"DTWAIN_AllocateMemoryEx")? };
        let DTWAIN_AppHandlesExceptions: Symbol<DtwainapphandlesexceptionsFunc> = unsafe { library.get(b"DTWAIN_AppHandlesExceptions")? };
        let DTWAIN_ArrayANSIStringToFloat: Symbol<DtwainarrayansistringtofloatFunc> = unsafe { library.get(b"DTWAIN_ArrayANSIStringToFloat")? };
        let DTWAIN_ArrayAdd: Symbol<DtwainarrayaddFunc> = unsafe { library.get(b"DTWAIN_ArrayAdd")? };
        let DTWAIN_ArrayAddANSIString: Symbol<DtwainarrayaddansistringFunc> = unsafe { library.get(b"DTWAIN_ArrayAddANSIString")? };
        let DTWAIN_ArrayAddANSIStringN: Symbol<DtwainarrayaddansistringnFunc> = unsafe { library.get(b"DTWAIN_ArrayAddANSIStringN")? };
        let DTWAIN_ArrayAddFloat: Symbol<DtwainarrayaddfloatFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloat")? };
        let DTWAIN_ArrayAddFloatN: Symbol<DtwainarrayaddfloatnFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloatN")? };
        let DTWAIN_ArrayAddFloatString: Symbol<DtwainarrayaddfloatstringFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloatString")? };
        let DTWAIN_ArrayAddFloatStringA: Symbol<DtwainarrayaddfloatstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloatStringA")? };
        let DTWAIN_ArrayAddFloatStringN: Symbol<DtwainarrayaddfloatstringnFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloatStringN")? };
        let DTWAIN_ArrayAddFloatStringNA: Symbol<DtwainarrayaddfloatstringnaFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloatStringNA")? };
        let DTWAIN_ArrayAddFloatStringNW: Symbol<DtwainarrayaddfloatstringnwFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloatStringNW")? };
        let DTWAIN_ArrayAddFloatStringW: Symbol<DtwainarrayaddfloatstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFloatStringW")? };
        let DTWAIN_ArrayAddFrame: Symbol<DtwainarrayaddframeFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFrame")? };
        let DTWAIN_ArrayAddFrameN: Symbol<DtwainarrayaddframenFunc> = unsafe { library.get(b"DTWAIN_ArrayAddFrameN")? };
        let DTWAIN_ArrayAddLong: Symbol<DtwainarrayaddlongFunc> = unsafe { library.get(b"DTWAIN_ArrayAddLong")? };
        let DTWAIN_ArrayAddLong64: Symbol<Dtwainarrayaddlong64Func> = unsafe { library.get(b"DTWAIN_ArrayAddLong64")? };
        let DTWAIN_ArrayAddLong64N: Symbol<Dtwainarrayaddlong64nFunc> = unsafe { library.get(b"DTWAIN_ArrayAddLong64N")? };
        let DTWAIN_ArrayAddLongN: Symbol<DtwainarrayaddlongnFunc> = unsafe { library.get(b"DTWAIN_ArrayAddLongN")? };
        let DTWAIN_ArrayAddN: Symbol<DtwainarrayaddnFunc> = unsafe { library.get(b"DTWAIN_ArrayAddN")? };
        let DTWAIN_ArrayAddString: Symbol<DtwainarrayaddstringFunc> = unsafe { library.get(b"DTWAIN_ArrayAddString")? };
        let DTWAIN_ArrayAddStringA: Symbol<DtwainarrayaddstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayAddStringA")? };
        let DTWAIN_ArrayAddStringN: Symbol<DtwainarrayaddstringnFunc> = unsafe { library.get(b"DTWAIN_ArrayAddStringN")? };
        let DTWAIN_ArrayAddStringNA: Symbol<DtwainarrayaddstringnaFunc> = unsafe { library.get(b"DTWAIN_ArrayAddStringNA")? };
        let DTWAIN_ArrayAddStringNW: Symbol<DtwainarrayaddstringnwFunc> = unsafe { library.get(b"DTWAIN_ArrayAddStringNW")? };
        let DTWAIN_ArrayAddStringW: Symbol<DtwainarrayaddstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayAddStringW")? };
        let DTWAIN_ArrayAddWideString: Symbol<DtwainarrayaddwidestringFunc> = unsafe { library.get(b"DTWAIN_ArrayAddWideString")? };
        let DTWAIN_ArrayAddWideStringN: Symbol<DtwainarrayaddwidestringnFunc> = unsafe { library.get(b"DTWAIN_ArrayAddWideStringN")? };
        let DTWAIN_ArrayConvertFix32ToFloat: Symbol<Dtwainarrayconvertfix32tofloatFunc> = unsafe { library.get(b"DTWAIN_ArrayConvertFix32ToFloat")? };
        let DTWAIN_ArrayConvertFloatToFix32: Symbol<Dtwainarrayconvertfloattofix32Func> = unsafe { library.get(b"DTWAIN_ArrayConvertFloatToFix32")? };
        let DTWAIN_ArrayCopy: Symbol<DtwainarraycopyFunc> = unsafe { library.get(b"DTWAIN_ArrayCopy")? };
        let DTWAIN_ArrayCreate: Symbol<DtwainarraycreateFunc> = unsafe { library.get(b"DTWAIN_ArrayCreate")? };
        let DTWAIN_ArrayCreateCopy: Symbol<DtwainarraycreatecopyFunc> = unsafe { library.get(b"DTWAIN_ArrayCreateCopy")? };
        let DTWAIN_ArrayCreateFromCap: Symbol<DtwainarraycreatefromcapFunc> = unsafe { library.get(b"DTWAIN_ArrayCreateFromCap")? };
        let DTWAIN_ArrayCreateFromLong64s: Symbol<Dtwainarraycreatefromlong64sFunc> = unsafe { library.get(b"DTWAIN_ArrayCreateFromLong64s")? };
        let DTWAIN_ArrayCreateFromLongs: Symbol<DtwainarraycreatefromlongsFunc> = unsafe { library.get(b"DTWAIN_ArrayCreateFromLongs")? };
        let DTWAIN_ArrayCreateFromReals: Symbol<DtwainarraycreatefromrealsFunc> = unsafe { library.get(b"DTWAIN_ArrayCreateFromReals")? };
        let DTWAIN_ArrayDestroy: Symbol<DtwainarraydestroyFunc> = unsafe { library.get(b"DTWAIN_ArrayDestroy")? };
        let DTWAIN_ArrayDestroyFrames: Symbol<DtwainarraydestroyframesFunc> = unsafe { library.get(b"DTWAIN_ArrayDestroyFrames")? };
        let DTWAIN_ArrayFind: Symbol<DtwainarrayfindFunc> = unsafe { library.get(b"DTWAIN_ArrayFind")? };
        let DTWAIN_ArrayFindANSIString: Symbol<DtwainarrayfindansistringFunc> = unsafe { library.get(b"DTWAIN_ArrayFindANSIString")? };
        let DTWAIN_ArrayFindFloat: Symbol<DtwainarrayfindfloatFunc> = unsafe { library.get(b"DTWAIN_ArrayFindFloat")? };
        let DTWAIN_ArrayFindFloatString: Symbol<DtwainarrayfindfloatstringFunc> = unsafe { library.get(b"DTWAIN_ArrayFindFloatString")? };
        let DTWAIN_ArrayFindFloatStringA: Symbol<DtwainarrayfindfloatstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayFindFloatStringA")? };
        let DTWAIN_ArrayFindFloatStringW: Symbol<DtwainarrayfindfloatstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayFindFloatStringW")? };
        let DTWAIN_ArrayFindLong: Symbol<DtwainarrayfindlongFunc> = unsafe { library.get(b"DTWAIN_ArrayFindLong")? };
        let DTWAIN_ArrayFindLong64: Symbol<Dtwainarrayfindlong64Func> = unsafe { library.get(b"DTWAIN_ArrayFindLong64")? };
        let DTWAIN_ArrayFindString: Symbol<DtwainarrayfindstringFunc> = unsafe { library.get(b"DTWAIN_ArrayFindString")? };
        let DTWAIN_ArrayFindStringA: Symbol<DtwainarrayfindstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayFindStringA")? };
        let DTWAIN_ArrayFindStringW: Symbol<DtwainarrayfindstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayFindStringW")? };
        let DTWAIN_ArrayFindWideString: Symbol<DtwainarrayfindwidestringFunc> = unsafe { library.get(b"DTWAIN_ArrayFindWideString")? };
        let DTWAIN_ArrayFix32GetAt: Symbol<Dtwainarrayfix32getatFunc> = unsafe { library.get(b"DTWAIN_ArrayFix32GetAt")? };
        let DTWAIN_ArrayFix32SetAt: Symbol<Dtwainarrayfix32setatFunc> = unsafe { library.get(b"DTWAIN_ArrayFix32SetAt")? };
        let DTWAIN_ArrayFloatToANSIString: Symbol<DtwainarrayfloattoansistringFunc> = unsafe { library.get(b"DTWAIN_ArrayFloatToANSIString")? };
        let DTWAIN_ArrayFloatToString: Symbol<DtwainarrayfloattostringFunc> = unsafe { library.get(b"DTWAIN_ArrayFloatToString")? };
        let DTWAIN_ArrayFloatToWideString: Symbol<DtwainarrayfloattowidestringFunc> = unsafe { library.get(b"DTWAIN_ArrayFloatToWideString")? };
        let DTWAIN_ArrayGetAt: Symbol<DtwainarraygetatFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAt")? };
        let DTWAIN_ArrayGetAtANSIString: Symbol<DtwainarraygetatansistringFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtANSIString")? };
        let DTWAIN_ArrayGetAtANSIStringPtr: Symbol<DtwainarraygetatansistringptrFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtANSIStringPtr")? };
        let DTWAIN_ArrayGetAtFloat: Symbol<DtwainarraygetatfloatFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFloat")? };
        let DTWAIN_ArrayGetAtFloatString: Symbol<DtwainarraygetatfloatstringFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFloatString")? };
        let DTWAIN_ArrayGetAtFloatStringA: Symbol<DtwainarraygetatfloatstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFloatStringA")? };
        let DTWAIN_ArrayGetAtFloatStringW: Symbol<DtwainarraygetatfloatstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFloatStringW")? };
        let DTWAIN_ArrayGetAtFrame: Symbol<DtwainarraygetatframeFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFrame")? };
        let DTWAIN_ArrayGetAtFrameEx: Symbol<DtwainarraygetatframeexFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFrameEx")? };
        let DTWAIN_ArrayGetAtFrameString: Symbol<DtwainarraygetatframestringFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFrameString")? };
        let DTWAIN_ArrayGetAtFrameStringA: Symbol<DtwainarraygetatframestringaFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFrameStringA")? };
        let DTWAIN_ArrayGetAtFrameStringW: Symbol<DtwainarraygetatframestringwFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtFrameStringW")? };
        let DTWAIN_ArrayGetAtLong: Symbol<DtwainarraygetatlongFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtLong")? };
        let DTWAIN_ArrayGetAtLong64: Symbol<Dtwainarraygetatlong64Func> = unsafe { library.get(b"DTWAIN_ArrayGetAtLong64")? };
        let DTWAIN_ArrayGetAtSource: Symbol<DtwainarraygetatsourceFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtSource")? };
        let DTWAIN_ArrayGetAtString: Symbol<DtwainarraygetatstringFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtString")? };
        let DTWAIN_ArrayGetAtStringA: Symbol<DtwainarraygetatstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtStringA")? };
        let DTWAIN_ArrayGetAtStringPtr: Symbol<DtwainarraygetatstringptrFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtStringPtr")? };
        let DTWAIN_ArrayGetAtStringW: Symbol<DtwainarraygetatstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtStringW")? };
        let DTWAIN_ArrayGetAtWideString: Symbol<DtwainarraygetatwidestringFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtWideString")? };
        let DTWAIN_ArrayGetAtWideStringPtr: Symbol<DtwainarraygetatwidestringptrFunc> = unsafe { library.get(b"DTWAIN_ArrayGetAtWideStringPtr")? };
        let DTWAIN_ArrayGetBuffer: Symbol<DtwainarraygetbufferFunc> = unsafe { library.get(b"DTWAIN_ArrayGetBuffer")? };
        let DTWAIN_ArrayGetCapValues: Symbol<DtwainarraygetcapvaluesFunc> = unsafe { library.get(b"DTWAIN_ArrayGetCapValues")? };
        let DTWAIN_ArrayGetCapValuesEx: Symbol<DtwainarraygetcapvaluesexFunc> = unsafe { library.get(b"DTWAIN_ArrayGetCapValuesEx")? };
        let DTWAIN_ArrayGetCapValuesEx2: Symbol<Dtwainarraygetcapvaluesex2Func> = unsafe { library.get(b"DTWAIN_ArrayGetCapValuesEx2")? };
        let DTWAIN_ArrayGetCount: Symbol<DtwainarraygetcountFunc> = unsafe { library.get(b"DTWAIN_ArrayGetCount")? };
        let DTWAIN_ArrayGetMaxStringLength: Symbol<DtwainarraygetmaxstringlengthFunc> = unsafe { library.get(b"DTWAIN_ArrayGetMaxStringLength")? };
        let DTWAIN_ArrayGetSourceAt: Symbol<DtwainarraygetsourceatFunc> = unsafe { library.get(b"DTWAIN_ArrayGetSourceAt")? };
        let DTWAIN_ArrayGetStringLength: Symbol<DtwainarraygetstringlengthFunc> = unsafe { library.get(b"DTWAIN_ArrayGetStringLength")? };
        let DTWAIN_ArrayGetType: Symbol<DtwainarraygettypeFunc> = unsafe { library.get(b"DTWAIN_ArrayGetType")? };
        let DTWAIN_ArrayInit: Symbol<DtwainarrayinitFunc> = unsafe { library.get(b"DTWAIN_ArrayInit")? };
        let DTWAIN_ArrayInsertAt: Symbol<DtwainarrayinsertatFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAt")? };
        let DTWAIN_ArrayInsertAtANSIString: Symbol<DtwainarrayinsertatansistringFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtANSIString")? };
        let DTWAIN_ArrayInsertAtANSIStringN: Symbol<DtwainarrayinsertatansistringnFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtANSIStringN")? };
        let DTWAIN_ArrayInsertAtFloat: Symbol<DtwainarrayinsertatfloatFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloat")? };
        let DTWAIN_ArrayInsertAtFloatN: Symbol<DtwainarrayinsertatfloatnFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloatN")? };
        let DTWAIN_ArrayInsertAtFloatString: Symbol<DtwainarrayinsertatfloatstringFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloatString")? };
        let DTWAIN_ArrayInsertAtFloatStringA: Symbol<DtwainarrayinsertatfloatstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloatStringA")? };
        let DTWAIN_ArrayInsertAtFloatStringN: Symbol<DtwainarrayinsertatfloatstringnFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloatStringN")? };
        let DTWAIN_ArrayInsertAtFloatStringNA: Symbol<DtwainarrayinsertatfloatstringnaFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloatStringNA")? };
        let DTWAIN_ArrayInsertAtFloatStringNW: Symbol<DtwainarrayinsertatfloatstringnwFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloatStringNW")? };
        let DTWAIN_ArrayInsertAtFloatStringW: Symbol<DtwainarrayinsertatfloatstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFloatStringW")? };
        let DTWAIN_ArrayInsertAtFrame: Symbol<DtwainarrayinsertatframeFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFrame")? };
        let DTWAIN_ArrayInsertAtFrameN: Symbol<DtwainarrayinsertatframenFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtFrameN")? };
        let DTWAIN_ArrayInsertAtLong: Symbol<DtwainarrayinsertatlongFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtLong")? };
        let DTWAIN_ArrayInsertAtLong64: Symbol<Dtwainarrayinsertatlong64Func> = unsafe { library.get(b"DTWAIN_ArrayInsertAtLong64")? };
        let DTWAIN_ArrayInsertAtLong64N: Symbol<Dtwainarrayinsertatlong64nFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtLong64N")? };
        let DTWAIN_ArrayInsertAtLongN: Symbol<DtwainarrayinsertatlongnFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtLongN")? };
        let DTWAIN_ArrayInsertAtN: Symbol<DtwainarrayinsertatnFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtN")? };
        let DTWAIN_ArrayInsertAtString: Symbol<DtwainarrayinsertatstringFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtString")? };
        let DTWAIN_ArrayInsertAtStringA: Symbol<DtwainarrayinsertatstringaFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtStringA")? };
        let DTWAIN_ArrayInsertAtStringN: Symbol<DtwainarrayinsertatstringnFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtStringN")? };
        let DTWAIN_ArrayInsertAtStringNA: Symbol<DtwainarrayinsertatstringnaFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtStringNA")? };
        let DTWAIN_ArrayInsertAtStringNW: Symbol<DtwainarrayinsertatstringnwFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtStringNW")? };
        let DTWAIN_ArrayInsertAtStringW: Symbol<DtwainarrayinsertatstringwFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtStringW")? };
        let DTWAIN_ArrayInsertAtWideString: Symbol<DtwainarrayinsertatwidestringFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtWideString")? };
        let DTWAIN_ArrayInsertAtWideStringN: Symbol<DtwainarrayinsertatwidestringnFunc> = unsafe { library.get(b"DTWAIN_ArrayInsertAtWideStringN")? };
        let DTWAIN_ArrayRemoveAll: Symbol<DtwainarrayremoveallFunc> = unsafe { library.get(b"DTWAIN_ArrayRemoveAll")? };
        let DTWAIN_ArrayRemoveAt: Symbol<DtwainarrayremoveatFunc> = unsafe { library.get(b"DTWAIN_ArrayRemoveAt")? };
        let DTWAIN_ArrayRemoveAtN: Symbol<DtwainarrayremoveatnFunc> = unsafe { library.get(b"DTWAIN_ArrayRemoveAtN")? };
        let DTWAIN_ArrayResize: Symbol<DtwainarrayresizeFunc> = unsafe { library.get(b"DTWAIN_ArrayResize")? };
        let DTWAIN_ArraySetAt: Symbol<DtwainarraysetatFunc> = unsafe { library.get(b"DTWAIN_ArraySetAt")? };
        let DTWAIN_ArraySetAtANSIString: Symbol<DtwainarraysetatansistringFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtANSIString")? };
        let DTWAIN_ArraySetAtFloat: Symbol<DtwainarraysetatfloatFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFloat")? };
        let DTWAIN_ArraySetAtFloatString: Symbol<DtwainarraysetatfloatstringFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFloatString")? };
        let DTWAIN_ArraySetAtFloatStringA: Symbol<DtwainarraysetatfloatstringaFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFloatStringA")? };
        let DTWAIN_ArraySetAtFloatStringW: Symbol<DtwainarraysetatfloatstringwFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFloatStringW")? };
        let DTWAIN_ArraySetAtFrame: Symbol<DtwainarraysetatframeFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFrame")? };
        let DTWAIN_ArraySetAtFrameEx: Symbol<DtwainarraysetatframeexFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFrameEx")? };
        let DTWAIN_ArraySetAtFrameString: Symbol<DtwainarraysetatframestringFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFrameString")? };
        let DTWAIN_ArraySetAtFrameStringA: Symbol<DtwainarraysetatframestringaFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFrameStringA")? };
        let DTWAIN_ArraySetAtFrameStringW: Symbol<DtwainarraysetatframestringwFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtFrameStringW")? };
        let DTWAIN_ArraySetAtLong: Symbol<DtwainarraysetatlongFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtLong")? };
        let DTWAIN_ArraySetAtLong64: Symbol<Dtwainarraysetatlong64Func> = unsafe { library.get(b"DTWAIN_ArraySetAtLong64")? };
        let DTWAIN_ArraySetAtString: Symbol<DtwainarraysetatstringFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtString")? };
        let DTWAIN_ArraySetAtStringA: Symbol<DtwainarraysetatstringaFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtStringA")? };
        let DTWAIN_ArraySetAtStringW: Symbol<DtwainarraysetatstringwFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtStringW")? };
        let DTWAIN_ArraySetAtWideString: Symbol<DtwainarraysetatwidestringFunc> = unsafe { library.get(b"DTWAIN_ArraySetAtWideString")? };
        let DTWAIN_ArrayStringToFloat: Symbol<DtwainarraystringtofloatFunc> = unsafe { library.get(b"DTWAIN_ArrayStringToFloat")? };
        let DTWAIN_ArrayWideStringToFloat: Symbol<DtwainarraywidestringtofloatFunc> = unsafe { library.get(b"DTWAIN_ArrayWideStringToFloat")? };
        let DTWAIN_CallCallback: Symbol<DtwaincallcallbackFunc> = unsafe { library.get(b"DTWAIN_CallCallback")? };
        let DTWAIN_CallCallback64: Symbol<Dtwaincallcallback64Func> = unsafe { library.get(b"DTWAIN_CallCallback64")? };
        let DTWAIN_CallDSMProc: Symbol<DtwaincalldsmprocFunc> = unsafe { library.get(b"DTWAIN_CallDSMProc")? };
        let DTWAIN_CheckHandles: Symbol<DtwaincheckhandlesFunc> = unsafe { library.get(b"DTWAIN_CheckHandles")? };
        let DTWAIN_ClearBuffers: Symbol<DtwainclearbuffersFunc> = unsafe { library.get(b"DTWAIN_ClearBuffers")? };
        let DTWAIN_ClearErrorBuffer: Symbol<DtwainclearerrorbufferFunc> = unsafe { library.get(b"DTWAIN_ClearErrorBuffer")? };
        let DTWAIN_ClearPDFText: Symbol<DtwainclearpdftextFunc> = unsafe { library.get(b"DTWAIN_ClearPDFText")? };
        let DTWAIN_ClearPage: Symbol<DtwainclearpageFunc> = unsafe { library.get(b"DTWAIN_ClearPage")? };
        let DTWAIN_CloseSource: Symbol<DtwainclosesourceFunc> = unsafe { library.get(b"DTWAIN_CloseSource")? };
        let DTWAIN_CloseSourceUI: Symbol<DtwainclosesourceuiFunc> = unsafe { library.get(b"DTWAIN_CloseSourceUI")? };
        let DTWAIN_ConvertDIBToBitmap: Symbol<DtwainconvertdibtobitmapFunc> = unsafe { library.get(b"DTWAIN_ConvertDIBToBitmap")? };
        let DTWAIN_ConvertDIBToFullBitmap: Symbol<DtwainconvertdibtofullbitmapFunc> = unsafe { library.get(b"DTWAIN_ConvertDIBToFullBitmap")? };
        let DTWAIN_ConvertToAPIString: Symbol<DtwainconverttoapistringFunc> = unsafe { library.get(b"DTWAIN_ConvertToAPIString")? };
        let DTWAIN_ConvertToAPIStringA: Symbol<DtwainconverttoapistringaFunc> = unsafe { library.get(b"DTWAIN_ConvertToAPIStringA")? };
        let DTWAIN_ConvertToAPIStringEx: Symbol<DtwainconverttoapistringexFunc> = unsafe { library.get(b"DTWAIN_ConvertToAPIStringEx")? };
        let DTWAIN_ConvertToAPIStringExA: Symbol<DtwainconverttoapistringexaFunc> = unsafe { library.get(b"DTWAIN_ConvertToAPIStringExA")? };
        let DTWAIN_ConvertToAPIStringExW: Symbol<DtwainconverttoapistringexwFunc> = unsafe { library.get(b"DTWAIN_ConvertToAPIStringExW")? };
        let DTWAIN_ConvertToAPIStringW: Symbol<DtwainconverttoapistringwFunc> = unsafe { library.get(b"DTWAIN_ConvertToAPIStringW")? };
        let DTWAIN_CreateAcquisitionArray: Symbol<DtwaincreateacquisitionarrayFunc> = unsafe { library.get(b"DTWAIN_CreateAcquisitionArray")? };
        let DTWAIN_CreatePDFTextElement: Symbol<DtwaincreatepdftextelementFunc> = unsafe { library.get(b"DTWAIN_CreatePDFTextElement")? };
        let DTWAIN_DeleteDIB: Symbol<DtwaindeletedibFunc> = unsafe { library.get(b"DTWAIN_DeleteDIB")? };
        let DTWAIN_DestroyAcquisitionArray: Symbol<DtwaindestroyacquisitionarrayFunc> = unsafe { library.get(b"DTWAIN_DestroyAcquisitionArray")? };
        let DTWAIN_DestroyPDFTextElement: Symbol<DtwaindestroypdftextelementFunc> = unsafe { library.get(b"DTWAIN_DestroyPDFTextElement")? };
        let DTWAIN_DisableAppWindow: Symbol<DtwaindisableappwindowFunc> = unsafe { library.get(b"DTWAIN_DisableAppWindow")? };
        let DTWAIN_EnableAutoBorderDetect: Symbol<DtwainenableautoborderdetectFunc> = unsafe { library.get(b"DTWAIN_EnableAutoBorderDetect")? };
        let DTWAIN_EnableAutoBright: Symbol<DtwainenableautobrightFunc> = unsafe { library.get(b"DTWAIN_EnableAutoBright")? };
        let DTWAIN_EnableAutoDeskew: Symbol<DtwainenableautodeskewFunc> = unsafe { library.get(b"DTWAIN_EnableAutoDeskew")? };
        let DTWAIN_EnableAutoFeed: Symbol<DtwainenableautofeedFunc> = unsafe { library.get(b"DTWAIN_EnableAutoFeed")? };
        let DTWAIN_EnableAutoRotate: Symbol<DtwainenableautorotateFunc> = unsafe { library.get(b"DTWAIN_EnableAutoRotate")? };
        let DTWAIN_EnableAutoScan: Symbol<DtwainenableautoscanFunc> = unsafe { library.get(b"DTWAIN_EnableAutoScan")? };
        let DTWAIN_EnableAutomaticSenseMedium: Symbol<DtwainenableautomaticsensemediumFunc> = unsafe { library.get(b"DTWAIN_EnableAutomaticSenseMedium")? };
        let DTWAIN_EnableDuplex: Symbol<DtwainenableduplexFunc> = unsafe { library.get(b"DTWAIN_EnableDuplex")? };
        let DTWAIN_EnableFeeder: Symbol<DtwainenablefeederFunc> = unsafe { library.get(b"DTWAIN_EnableFeeder")? };
        let DTWAIN_EnableIndicator: Symbol<DtwainenableindicatorFunc> = unsafe { library.get(b"DTWAIN_EnableIndicator")? };
        let DTWAIN_EnableJobFileHandling: Symbol<DtwainenablejobfilehandlingFunc> = unsafe { library.get(b"DTWAIN_EnableJobFileHandling")? };
        let DTWAIN_EnableLamp: Symbol<DtwainenablelampFunc> = unsafe { library.get(b"DTWAIN_EnableLamp")? };
        let DTWAIN_EnableMsgNotify: Symbol<DtwainenablemsgnotifyFunc> = unsafe { library.get(b"DTWAIN_EnableMsgNotify")? };
        let DTWAIN_EnablePatchDetect: Symbol<DtwainenablepatchdetectFunc> = unsafe { library.get(b"DTWAIN_EnablePatchDetect")? };
        let DTWAIN_EnablePeekMessageLoop: Symbol<DtwainenablepeekmessageloopFunc> = unsafe { library.get(b"DTWAIN_EnablePeekMessageLoop")? };
        let DTWAIN_EnablePrinter: Symbol<DtwainenableprinterFunc> = unsafe { library.get(b"DTWAIN_EnablePrinter")? };
        let DTWAIN_EnableThumbnail: Symbol<DtwainenablethumbnailFunc> = unsafe { library.get(b"DTWAIN_EnableThumbnail")? };
        let DTWAIN_EnableTripletsNotify: Symbol<DtwainenabletripletsnotifyFunc> = unsafe { library.get(b"DTWAIN_EnableTripletsNotify")? };
        let DTWAIN_EndThread: Symbol<DtwainendthreadFunc> = unsafe { library.get(b"DTWAIN_EndThread")? };
        let DTWAIN_EndTwainSession: Symbol<DtwainendtwainsessionFunc> = unsafe { library.get(b"DTWAIN_EndTwainSession")? };
        let DTWAIN_EnumAlarmVolumes: Symbol<DtwainenumalarmvolumesFunc> = unsafe { library.get(b"DTWAIN_EnumAlarmVolumes")? };
        let DTWAIN_EnumAlarmVolumesEx: Symbol<DtwainenumalarmvolumesexFunc> = unsafe { library.get(b"DTWAIN_EnumAlarmVolumesEx")? };
        let DTWAIN_EnumAlarms: Symbol<DtwainenumalarmsFunc> = unsafe { library.get(b"DTWAIN_EnumAlarms")? };
        let DTWAIN_EnumAlarmsEx: Symbol<DtwainenumalarmsexFunc> = unsafe { library.get(b"DTWAIN_EnumAlarmsEx")? };
        let DTWAIN_EnumAudioXferMechs: Symbol<DtwainenumaudioxfermechsFunc> = unsafe { library.get(b"DTWAIN_EnumAudioXferMechs")? };
        let DTWAIN_EnumAudioXferMechsEx: Symbol<DtwainenumaudioxfermechsexFunc> = unsafe { library.get(b"DTWAIN_EnumAudioXferMechsEx")? };
        let DTWAIN_EnumAutoFeedValues: Symbol<DtwainenumautofeedvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumAutoFeedValues")? };
        let DTWAIN_EnumAutoFeedValuesEx: Symbol<DtwainenumautofeedvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumAutoFeedValuesEx")? };
        let DTWAIN_EnumAutomaticCaptures: Symbol<DtwainenumautomaticcapturesFunc> = unsafe { library.get(b"DTWAIN_EnumAutomaticCaptures")? };
        let DTWAIN_EnumAutomaticCapturesEx: Symbol<DtwainenumautomaticcapturesexFunc> = unsafe { library.get(b"DTWAIN_EnumAutomaticCapturesEx")? };
        let DTWAIN_EnumAutomaticSenseMedium: Symbol<DtwainenumautomaticsensemediumFunc> = unsafe { library.get(b"DTWAIN_EnumAutomaticSenseMedium")? };
        let DTWAIN_EnumAutomaticSenseMediumEx: Symbol<DtwainenumautomaticsensemediumexFunc> = unsafe { library.get(b"DTWAIN_EnumAutomaticSenseMediumEx")? };
        let DTWAIN_EnumBitDepths: Symbol<DtwainenumbitdepthsFunc> = unsafe { library.get(b"DTWAIN_EnumBitDepths")? };
        let DTWAIN_EnumBitDepthsEx: Symbol<DtwainenumbitdepthsexFunc> = unsafe { library.get(b"DTWAIN_EnumBitDepthsEx")? };
        let DTWAIN_EnumBitDepthsEx2: Symbol<Dtwainenumbitdepthsex2Func> = unsafe { library.get(b"DTWAIN_EnumBitDepthsEx2")? };
        let DTWAIN_EnumBottomCameras: Symbol<DtwainenumbottomcamerasFunc> = unsafe { library.get(b"DTWAIN_EnumBottomCameras")? };
        let DTWAIN_EnumBottomCamerasEx: Symbol<DtwainenumbottomcamerasexFunc> = unsafe { library.get(b"DTWAIN_EnumBottomCamerasEx")? };
        let DTWAIN_EnumBrightnessValues: Symbol<DtwainenumbrightnessvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumBrightnessValues")? };
        let DTWAIN_EnumBrightnessValuesEx: Symbol<DtwainenumbrightnessvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumBrightnessValuesEx")? };
        let DTWAIN_EnumCameras: Symbol<DtwainenumcamerasFunc> = unsafe { library.get(b"DTWAIN_EnumCameras")? };
        let DTWAIN_EnumCamerasEx: Symbol<DtwainenumcamerasexFunc> = unsafe { library.get(b"DTWAIN_EnumCamerasEx")? };
        let DTWAIN_EnumCamerasEx2: Symbol<Dtwainenumcamerasex2Func> = unsafe { library.get(b"DTWAIN_EnumCamerasEx2")? };
        let DTWAIN_EnumCamerasEx3: Symbol<Dtwainenumcamerasex3Func> = unsafe { library.get(b"DTWAIN_EnumCamerasEx3")? };
        let DTWAIN_EnumCompressionTypes: Symbol<DtwainenumcompressiontypesFunc> = unsafe { library.get(b"DTWAIN_EnumCompressionTypes")? };
        let DTWAIN_EnumCompressionTypesEx: Symbol<DtwainenumcompressiontypesexFunc> = unsafe { library.get(b"DTWAIN_EnumCompressionTypesEx")? };
        let DTWAIN_EnumCompressionTypesEx2: Symbol<Dtwainenumcompressiontypesex2Func> = unsafe { library.get(b"DTWAIN_EnumCompressionTypesEx2")? };
        let DTWAIN_EnumContrastValues: Symbol<DtwainenumcontrastvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumContrastValues")? };
        let DTWAIN_EnumContrastValuesEx: Symbol<DtwainenumcontrastvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumContrastValuesEx")? };
        let DTWAIN_EnumCustomCaps: Symbol<DtwainenumcustomcapsFunc> = unsafe { library.get(b"DTWAIN_EnumCustomCaps")? };
        let DTWAIN_EnumCustomCapsEx2: Symbol<Dtwainenumcustomcapsex2Func> = unsafe { library.get(b"DTWAIN_EnumCustomCapsEx2")? };
        let DTWAIN_EnumDoubleFeedDetectLengths: Symbol<DtwainenumdoublefeeddetectlengthsFunc> = unsafe { library.get(b"DTWAIN_EnumDoubleFeedDetectLengths")? };
        let DTWAIN_EnumDoubleFeedDetectLengthsEx: Symbol<DtwainenumdoublefeeddetectlengthsexFunc> = unsafe { library.get(b"DTWAIN_EnumDoubleFeedDetectLengthsEx")? };
        let DTWAIN_EnumDoubleFeedDetectValues: Symbol<DtwainenumdoublefeeddetectvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumDoubleFeedDetectValues")? };
        let DTWAIN_EnumDoubleFeedDetectValuesEx: Symbol<DtwainenumdoublefeeddetectvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumDoubleFeedDetectValuesEx")? };
        let DTWAIN_EnumExtImageInfoTypes: Symbol<DtwainenumextimageinfotypesFunc> = unsafe { library.get(b"DTWAIN_EnumExtImageInfoTypes")? };
        let DTWAIN_EnumExtImageInfoTypesEx: Symbol<DtwainenumextimageinfotypesexFunc> = unsafe { library.get(b"DTWAIN_EnumExtImageInfoTypesEx")? };
        let DTWAIN_EnumExtendedCaps: Symbol<DtwainenumextendedcapsFunc> = unsafe { library.get(b"DTWAIN_EnumExtendedCaps")? };
        let DTWAIN_EnumExtendedCapsEx: Symbol<DtwainenumextendedcapsexFunc> = unsafe { library.get(b"DTWAIN_EnumExtendedCapsEx")? };
        let DTWAIN_EnumExtendedCapsEx2: Symbol<Dtwainenumextendedcapsex2Func> = unsafe { library.get(b"DTWAIN_EnumExtendedCapsEx2")? };
        let DTWAIN_EnumFileTypeBitsPerPixel: Symbol<DtwainenumfiletypebitsperpixelFunc> = unsafe { library.get(b"DTWAIN_EnumFileTypeBitsPerPixel")? };
        let DTWAIN_EnumFileXferFormats: Symbol<DtwainenumfilexferformatsFunc> = unsafe { library.get(b"DTWAIN_EnumFileXferFormats")? };
        let DTWAIN_EnumFileXferFormatsEx: Symbol<DtwainenumfilexferformatsexFunc> = unsafe { library.get(b"DTWAIN_EnumFileXferFormatsEx")? };
        let DTWAIN_EnumHalftones: Symbol<DtwainenumhalftonesFunc> = unsafe { library.get(b"DTWAIN_EnumHalftones")? };
        let DTWAIN_EnumHalftonesEx: Symbol<DtwainenumhalftonesexFunc> = unsafe { library.get(b"DTWAIN_EnumHalftonesEx")? };
        let DTWAIN_EnumHighlightValues: Symbol<DtwainenumhighlightvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumHighlightValues")? };
        let DTWAIN_EnumHighlightValuesEx: Symbol<DtwainenumhighlightvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumHighlightValuesEx")? };
        let DTWAIN_EnumJobControls: Symbol<DtwainenumjobcontrolsFunc> = unsafe { library.get(b"DTWAIN_EnumJobControls")? };
        let DTWAIN_EnumJobControlsEx: Symbol<DtwainenumjobcontrolsexFunc> = unsafe { library.get(b"DTWAIN_EnumJobControlsEx")? };
        let DTWAIN_EnumLightPaths: Symbol<DtwainenumlightpathsFunc> = unsafe { library.get(b"DTWAIN_EnumLightPaths")? };
        let DTWAIN_EnumLightPathsEx: Symbol<DtwainenumlightpathsexFunc> = unsafe { library.get(b"DTWAIN_EnumLightPathsEx")? };
        let DTWAIN_EnumLightSources: Symbol<DtwainenumlightsourcesFunc> = unsafe { library.get(b"DTWAIN_EnumLightSources")? };
        let DTWAIN_EnumLightSourcesEx: Symbol<DtwainenumlightsourcesexFunc> = unsafe { library.get(b"DTWAIN_EnumLightSourcesEx")? };
        let DTWAIN_EnumMaxBuffers: Symbol<DtwainenummaxbuffersFunc> = unsafe { library.get(b"DTWAIN_EnumMaxBuffers")? };
        let DTWAIN_EnumMaxBuffersEx: Symbol<DtwainenummaxbuffersexFunc> = unsafe { library.get(b"DTWAIN_EnumMaxBuffersEx")? };
        let DTWAIN_EnumNoiseFilters: Symbol<DtwainenumnoisefiltersFunc> = unsafe { library.get(b"DTWAIN_EnumNoiseFilters")? };
        let DTWAIN_EnumNoiseFiltersEx: Symbol<DtwainenumnoisefiltersexFunc> = unsafe { library.get(b"DTWAIN_EnumNoiseFiltersEx")? };
        let DTWAIN_EnumOCRInterfaces: Symbol<DtwainenumocrinterfacesFunc> = unsafe { library.get(b"DTWAIN_EnumOCRInterfaces")? };
        let DTWAIN_EnumOCRSupportedCaps: Symbol<DtwainenumocrsupportedcapsFunc> = unsafe { library.get(b"DTWAIN_EnumOCRSupportedCaps")? };
        let DTWAIN_EnumOrientations: Symbol<DtwainenumorientationsFunc> = unsafe { library.get(b"DTWAIN_EnumOrientations")? };
        let DTWAIN_EnumOrientationsEx: Symbol<DtwainenumorientationsexFunc> = unsafe { library.get(b"DTWAIN_EnumOrientationsEx")? };
        let DTWAIN_EnumOverscanValues: Symbol<DtwainenumoverscanvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumOverscanValues")? };
        let DTWAIN_EnumOverscanValuesEx: Symbol<DtwainenumoverscanvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumOverscanValuesEx")? };
        let DTWAIN_EnumPaperSizes: Symbol<DtwainenumpapersizesFunc> = unsafe { library.get(b"DTWAIN_EnumPaperSizes")? };
        let DTWAIN_EnumPaperSizesEx: Symbol<DtwainenumpapersizesexFunc> = unsafe { library.get(b"DTWAIN_EnumPaperSizesEx")? };
        let DTWAIN_EnumPatchCodes: Symbol<DtwainenumpatchcodesFunc> = unsafe { library.get(b"DTWAIN_EnumPatchCodes")? };
        let DTWAIN_EnumPatchCodesEx: Symbol<DtwainenumpatchcodesexFunc> = unsafe { library.get(b"DTWAIN_EnumPatchCodesEx")? };
        let DTWAIN_EnumPatchMaxPriorities: Symbol<DtwainenumpatchmaxprioritiesFunc> = unsafe { library.get(b"DTWAIN_EnumPatchMaxPriorities")? };
        let DTWAIN_EnumPatchMaxPrioritiesEx: Symbol<DtwainenumpatchmaxprioritiesexFunc> = unsafe { library.get(b"DTWAIN_EnumPatchMaxPrioritiesEx")? };
        let DTWAIN_EnumPatchMaxRetries: Symbol<DtwainenumpatchmaxretriesFunc> = unsafe { library.get(b"DTWAIN_EnumPatchMaxRetries")? };
        let DTWAIN_EnumPatchMaxRetriesEx: Symbol<DtwainenumpatchmaxretriesexFunc> = unsafe { library.get(b"DTWAIN_EnumPatchMaxRetriesEx")? };
        let DTWAIN_EnumPatchPriorities: Symbol<DtwainenumpatchprioritiesFunc> = unsafe { library.get(b"DTWAIN_EnumPatchPriorities")? };
        let DTWAIN_EnumPatchPrioritiesEx: Symbol<DtwainenumpatchprioritiesexFunc> = unsafe { library.get(b"DTWAIN_EnumPatchPrioritiesEx")? };
        let DTWAIN_EnumPatchSearchModes: Symbol<DtwainenumpatchsearchmodesFunc> = unsafe { library.get(b"DTWAIN_EnumPatchSearchModes")? };
        let DTWAIN_EnumPatchSearchModesEx: Symbol<DtwainenumpatchsearchmodesexFunc> = unsafe { library.get(b"DTWAIN_EnumPatchSearchModesEx")? };
        let DTWAIN_EnumPatchTimeOutValues: Symbol<DtwainenumpatchtimeoutvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumPatchTimeOutValues")? };
        let DTWAIN_EnumPatchTimeOutValuesEx: Symbol<DtwainenumpatchtimeoutvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumPatchTimeOutValuesEx")? };
        let DTWAIN_EnumPixelTypes: Symbol<DtwainenumpixeltypesFunc> = unsafe { library.get(b"DTWAIN_EnumPixelTypes")? };
        let DTWAIN_EnumPixelTypesEx: Symbol<DtwainenumpixeltypesexFunc> = unsafe { library.get(b"DTWAIN_EnumPixelTypesEx")? };
        let DTWAIN_EnumPrinterStringModes: Symbol<DtwainenumprinterstringmodesFunc> = unsafe { library.get(b"DTWAIN_EnumPrinterStringModes")? };
        let DTWAIN_EnumPrinterStringModesEx: Symbol<DtwainenumprinterstringmodesexFunc> = unsafe { library.get(b"DTWAIN_EnumPrinterStringModesEx")? };
        let DTWAIN_EnumResolutionValues: Symbol<DtwainenumresolutionvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumResolutionValues")? };
        let DTWAIN_EnumResolutionValuesEx: Symbol<DtwainenumresolutionvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumResolutionValuesEx")? };
        let DTWAIN_EnumShadowValues: Symbol<DtwainenumshadowvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumShadowValues")? };
        let DTWAIN_EnumShadowValuesEx: Symbol<DtwainenumshadowvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumShadowValuesEx")? };
        let DTWAIN_EnumSourceUnits: Symbol<DtwainenumsourceunitsFunc> = unsafe { library.get(b"DTWAIN_EnumSourceUnits")? };
        let DTWAIN_EnumSourceUnitsEx: Symbol<DtwainenumsourceunitsexFunc> = unsafe { library.get(b"DTWAIN_EnumSourceUnitsEx")? };
        let DTWAIN_EnumSourceValues: Symbol<DtwainenumsourcevaluesFunc> = unsafe { library.get(b"DTWAIN_EnumSourceValues")? };
        let DTWAIN_EnumSourceValuesA: Symbol<DtwainenumsourcevaluesaFunc> = unsafe { library.get(b"DTWAIN_EnumSourceValuesA")? };
        let DTWAIN_EnumSourceValuesW: Symbol<DtwainenumsourcevalueswFunc> = unsafe { library.get(b"DTWAIN_EnumSourceValuesW")? };
        let DTWAIN_EnumSources: Symbol<DtwainenumsourcesFunc> = unsafe { library.get(b"DTWAIN_EnumSources")? };
        let DTWAIN_EnumSourcesEx: Symbol<DtwainenumsourcesexFunc> = unsafe { library.get(b"DTWAIN_EnumSourcesEx")? };
        let DTWAIN_EnumSupportedCaps: Symbol<DtwainenumsupportedcapsFunc> = unsafe { library.get(b"DTWAIN_EnumSupportedCaps")? };
        let DTWAIN_EnumSupportedCapsEx: Symbol<DtwainenumsupportedcapsexFunc> = unsafe { library.get(b"DTWAIN_EnumSupportedCapsEx")? };
        let DTWAIN_EnumSupportedCapsEx2: Symbol<Dtwainenumsupportedcapsex2Func> = unsafe { library.get(b"DTWAIN_EnumSupportedCapsEx2")? };
        let DTWAIN_EnumSupportedExtImageInfo: Symbol<DtwainenumsupportedextimageinfoFunc> = unsafe { library.get(b"DTWAIN_EnumSupportedExtImageInfo")? };
        let DTWAIN_EnumSupportedExtImageInfoEx: Symbol<DtwainenumsupportedextimageinfoexFunc> = unsafe { library.get(b"DTWAIN_EnumSupportedExtImageInfoEx")? };
        let DTWAIN_EnumSupportedFileTypes: Symbol<DtwainenumsupportedfiletypesFunc> = unsafe { library.get(b"DTWAIN_EnumSupportedFileTypes")? };
        let DTWAIN_EnumSupportedMultiPageFileTypes: Symbol<DtwainenumsupportedmultipagefiletypesFunc> = unsafe { library.get(b"DTWAIN_EnumSupportedMultiPageFileTypes")? };
        let DTWAIN_EnumSupportedSinglePageFileTypes: Symbol<DtwainenumsupportedsinglepagefiletypesFunc> = unsafe { library.get(b"DTWAIN_EnumSupportedSinglePageFileTypes")? };
        let DTWAIN_EnumThresholdValues: Symbol<DtwainenumthresholdvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumThresholdValues")? };
        let DTWAIN_EnumThresholdValuesEx: Symbol<DtwainenumthresholdvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumThresholdValuesEx")? };
        let DTWAIN_EnumTopCameras: Symbol<DtwainenumtopcamerasFunc> = unsafe { library.get(b"DTWAIN_EnumTopCameras")? };
        let DTWAIN_EnumTopCamerasEx: Symbol<DtwainenumtopcamerasexFunc> = unsafe { library.get(b"DTWAIN_EnumTopCamerasEx")? };
        let DTWAIN_EnumTwainPrinters: Symbol<DtwainenumtwainprintersFunc> = unsafe { library.get(b"DTWAIN_EnumTwainPrinters")? };
        let DTWAIN_EnumTwainPrintersArray: Symbol<DtwainenumtwainprintersarrayFunc> = unsafe { library.get(b"DTWAIN_EnumTwainPrintersArray")? };
        let DTWAIN_EnumTwainPrintersArrayEx: Symbol<DtwainenumtwainprintersarrayexFunc> = unsafe { library.get(b"DTWAIN_EnumTwainPrintersArrayEx")? };
        let DTWAIN_EnumTwainPrintersEx: Symbol<DtwainenumtwainprintersexFunc> = unsafe { library.get(b"DTWAIN_EnumTwainPrintersEx")? };
        let DTWAIN_EnumXResolutionValues: Symbol<DtwainenumxresolutionvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumXResolutionValues")? };
        let DTWAIN_EnumXResolutionValuesEx: Symbol<DtwainenumxresolutionvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumXResolutionValuesEx")? };
        let DTWAIN_EnumYResolutionValues: Symbol<DtwainenumyresolutionvaluesFunc> = unsafe { library.get(b"DTWAIN_EnumYResolutionValues")? };
        let DTWAIN_EnumYResolutionValuesEx: Symbol<DtwainenumyresolutionvaluesexFunc> = unsafe { library.get(b"DTWAIN_EnumYResolutionValuesEx")? };
        let DTWAIN_ExecuteOCR: Symbol<DtwainexecuteocrFunc> = unsafe { library.get(b"DTWAIN_ExecuteOCR")? };
        let DTWAIN_ExecuteOCRA: Symbol<DtwainexecuteocraFunc> = unsafe { library.get(b"DTWAIN_ExecuteOCRA")? };
        let DTWAIN_ExecuteOCRW: Symbol<DtwainexecuteocrwFunc> = unsafe { library.get(b"DTWAIN_ExecuteOCRW")? };
        let DTWAIN_FeedPage: Symbol<DtwainfeedpageFunc> = unsafe { library.get(b"DTWAIN_FeedPage")? };
        let DTWAIN_FlipBitmap: Symbol<DtwainflipbitmapFunc> = unsafe { library.get(b"DTWAIN_FlipBitmap")? };
        let DTWAIN_FlushAcquiredPages: Symbol<DtwainflushacquiredpagesFunc> = unsafe { library.get(b"DTWAIN_FlushAcquiredPages")? };
        let DTWAIN_ForceAcquireBitDepth: Symbol<DtwainforceacquirebitdepthFunc> = unsafe { library.get(b"DTWAIN_ForceAcquireBitDepth")? };
        let DTWAIN_ForceScanOnNoUI: Symbol<DtwainforcescanonnouiFunc> = unsafe { library.get(b"DTWAIN_ForceScanOnNoUI")? };
        let DTWAIN_FrameCreate: Symbol<DtwainframecreateFunc> = unsafe { library.get(b"DTWAIN_FrameCreate")? };
        let DTWAIN_FrameCreateString: Symbol<DtwainframecreatestringFunc> = unsafe { library.get(b"DTWAIN_FrameCreateString")? };
        let DTWAIN_FrameCreateStringA: Symbol<DtwainframecreatestringaFunc> = unsafe { library.get(b"DTWAIN_FrameCreateStringA")? };
        let DTWAIN_FrameCreateStringW: Symbol<DtwainframecreatestringwFunc> = unsafe { library.get(b"DTWAIN_FrameCreateStringW")? };
        let DTWAIN_FrameDestroy: Symbol<DtwainframedestroyFunc> = unsafe { library.get(b"DTWAIN_FrameDestroy")? };
        let DTWAIN_FrameGetAll: Symbol<DtwainframegetallFunc> = unsafe { library.get(b"DTWAIN_FrameGetAll")? };
        let DTWAIN_FrameGetAllString: Symbol<DtwainframegetallstringFunc> = unsafe { library.get(b"DTWAIN_FrameGetAllString")? };
        let DTWAIN_FrameGetAllStringA: Symbol<DtwainframegetallstringaFunc> = unsafe { library.get(b"DTWAIN_FrameGetAllStringA")? };
        let DTWAIN_FrameGetAllStringW: Symbol<DtwainframegetallstringwFunc> = unsafe { library.get(b"DTWAIN_FrameGetAllStringW")? };
        let DTWAIN_FrameGetValue: Symbol<DtwainframegetvalueFunc> = unsafe { library.get(b"DTWAIN_FrameGetValue")? };
        let DTWAIN_FrameGetValueString: Symbol<DtwainframegetvaluestringFunc> = unsafe { library.get(b"DTWAIN_FrameGetValueString")? };
        let DTWAIN_FrameGetValueStringA: Symbol<DtwainframegetvaluestringaFunc> = unsafe { library.get(b"DTWAIN_FrameGetValueStringA")? };
        let DTWAIN_FrameGetValueStringW: Symbol<DtwainframegetvaluestringwFunc> = unsafe { library.get(b"DTWAIN_FrameGetValueStringW")? };
        let DTWAIN_FrameIsValid: Symbol<DtwainframeisvalidFunc> = unsafe { library.get(b"DTWAIN_FrameIsValid")? };
        let DTWAIN_FrameSetAll: Symbol<DtwainframesetallFunc> = unsafe { library.get(b"DTWAIN_FrameSetAll")? };
        let DTWAIN_FrameSetAllString: Symbol<DtwainframesetallstringFunc> = unsafe { library.get(b"DTWAIN_FrameSetAllString")? };
        let DTWAIN_FrameSetAllStringA: Symbol<DtwainframesetallstringaFunc> = unsafe { library.get(b"DTWAIN_FrameSetAllStringA")? };
        let DTWAIN_FrameSetAllStringW: Symbol<DtwainframesetallstringwFunc> = unsafe { library.get(b"DTWAIN_FrameSetAllStringW")? };
        let DTWAIN_FrameSetValue: Symbol<DtwainframesetvalueFunc> = unsafe { library.get(b"DTWAIN_FrameSetValue")? };
        let DTWAIN_FrameSetValueString: Symbol<DtwainframesetvaluestringFunc> = unsafe { library.get(b"DTWAIN_FrameSetValueString")? };
        let DTWAIN_FrameSetValueStringA: Symbol<DtwainframesetvaluestringaFunc> = unsafe { library.get(b"DTWAIN_FrameSetValueStringA")? };
        let DTWAIN_FrameSetValueStringW: Symbol<DtwainframesetvaluestringwFunc> = unsafe { library.get(b"DTWAIN_FrameSetValueStringW")? };
        let DTWAIN_FreeExtImageInfo: Symbol<DtwainfreeextimageinfoFunc> = unsafe { library.get(b"DTWAIN_FreeExtImageInfo")? };
        let DTWAIN_FreeMemory: Symbol<DtwainfreememoryFunc> = unsafe { library.get(b"DTWAIN_FreeMemory")? };
        let DTWAIN_FreeMemoryEx: Symbol<DtwainfreememoryexFunc> = unsafe { library.get(b"DTWAIN_FreeMemoryEx")? };
        let DTWAIN_GetAPIHandleStatus: Symbol<DtwaingetapihandlestatusFunc> = unsafe { library.get(b"DTWAIN_GetAPIHandleStatus")? };
        let DTWAIN_GetAcquireArea: Symbol<DtwaingetacquireareaFunc> = unsafe { library.get(b"DTWAIN_GetAcquireArea")? };
        let DTWAIN_GetAcquireArea2: Symbol<Dtwaingetacquirearea2Func> = unsafe { library.get(b"DTWAIN_GetAcquireArea2")? };
        let DTWAIN_GetAcquireArea2String: Symbol<Dtwaingetacquirearea2stringFunc> = unsafe { library.get(b"DTWAIN_GetAcquireArea2String")? };
        let DTWAIN_GetAcquireArea2StringA: Symbol<Dtwaingetacquirearea2stringaFunc> = unsafe { library.get(b"DTWAIN_GetAcquireArea2StringA")? };
        let DTWAIN_GetAcquireArea2StringW: Symbol<Dtwaingetacquirearea2stringwFunc> = unsafe { library.get(b"DTWAIN_GetAcquireArea2StringW")? };
        let DTWAIN_GetAcquireAreaEx: Symbol<DtwaingetacquireareaexFunc> = unsafe { library.get(b"DTWAIN_GetAcquireAreaEx")? };
        let DTWAIN_GetAcquireMetrics: Symbol<DtwaingetacquiremetricsFunc> = unsafe { library.get(b"DTWAIN_GetAcquireMetrics")? };
        let DTWAIN_GetAcquireStripBuffer: Symbol<DtwaingetacquirestripbufferFunc> = unsafe { library.get(b"DTWAIN_GetAcquireStripBuffer")? };
        let DTWAIN_GetAcquireStripData: Symbol<DtwaingetacquirestripdataFunc> = unsafe { library.get(b"DTWAIN_GetAcquireStripData")? };
        let DTWAIN_GetAcquireStripSizes: Symbol<DtwaingetacquirestripsizesFunc> = unsafe { library.get(b"DTWAIN_GetAcquireStripSizes")? };
        let DTWAIN_GetAcquiredImage: Symbol<DtwaingetacquiredimageFunc> = unsafe { library.get(b"DTWAIN_GetAcquiredImage")? };
        let DTWAIN_GetAcquiredImageArray: Symbol<DtwaingetacquiredimagearrayFunc> = unsafe { library.get(b"DTWAIN_GetAcquiredImageArray")? };
        let DTWAIN_GetActiveDSMPath: Symbol<DtwaingetactivedsmpathFunc> = unsafe { library.get(b"DTWAIN_GetActiveDSMPath")? };
        let DTWAIN_GetActiveDSMPathA: Symbol<DtwaingetactivedsmpathaFunc> = unsafe { library.get(b"DTWAIN_GetActiveDSMPathA")? };
        let DTWAIN_GetActiveDSMPathW: Symbol<DtwaingetactivedsmpathwFunc> = unsafe { library.get(b"DTWAIN_GetActiveDSMPathW")? };
        let DTWAIN_GetActiveDSMVersionInfo: Symbol<DtwaingetactivedsmversioninfoFunc> = unsafe { library.get(b"DTWAIN_GetActiveDSMVersionInfo")? };
        let DTWAIN_GetActiveDSMVersionInfoA: Symbol<DtwaingetactivedsmversioninfoaFunc> = unsafe { library.get(b"DTWAIN_GetActiveDSMVersionInfoA")? };
        let DTWAIN_GetActiveDSMVersionInfoW: Symbol<DtwaingetactivedsmversioninfowFunc> = unsafe { library.get(b"DTWAIN_GetActiveDSMVersionInfoW")? };
        let DTWAIN_GetAlarmVolume: Symbol<DtwaingetalarmvolumeFunc> = unsafe { library.get(b"DTWAIN_GetAlarmVolume")? };
        let DTWAIN_GetAllSourceDibs: Symbol<DtwaingetallsourcedibsFunc> = unsafe { library.get(b"DTWAIN_GetAllSourceDibs")? };
        let DTWAIN_GetAppInfo: Symbol<DtwaingetappinfoFunc> = unsafe { library.get(b"DTWAIN_GetAppInfo")? };
        let DTWAIN_GetAppInfoA: Symbol<DtwaingetappinfoaFunc> = unsafe { library.get(b"DTWAIN_GetAppInfoA")? };
        let DTWAIN_GetAppInfoW: Symbol<DtwaingetappinfowFunc> = unsafe { library.get(b"DTWAIN_GetAppInfoW")? };
        let DTWAIN_GetAuthor: Symbol<DtwaingetauthorFunc> = unsafe { library.get(b"DTWAIN_GetAuthor")? };
        let DTWAIN_GetAuthorA: Symbol<DtwaingetauthoraFunc> = unsafe { library.get(b"DTWAIN_GetAuthorA")? };
        let DTWAIN_GetAuthorW: Symbol<DtwaingetauthorwFunc> = unsafe { library.get(b"DTWAIN_GetAuthorW")? };
        let DTWAIN_GetBatteryMinutes: Symbol<DtwaingetbatteryminutesFunc> = unsafe { library.get(b"DTWAIN_GetBatteryMinutes")? };
        let DTWAIN_GetBatteryPercent: Symbol<DtwaingetbatterypercentFunc> = unsafe { library.get(b"DTWAIN_GetBatteryPercent")? };
        let DTWAIN_GetBitDepth: Symbol<DtwaingetbitdepthFunc> = unsafe { library.get(b"DTWAIN_GetBitDepth")? };
        let DTWAIN_GetBlankPageAutoDetection: Symbol<DtwaingetblankpageautodetectionFunc> = unsafe { library.get(b"DTWAIN_GetBlankPageAutoDetection")? };
        let DTWAIN_GetBrightness: Symbol<DtwaingetbrightnessFunc> = unsafe { library.get(b"DTWAIN_GetBrightness")? };
        let DTWAIN_GetBrightnessString: Symbol<DtwaingetbrightnessstringFunc> = unsafe { library.get(b"DTWAIN_GetBrightnessString")? };
        let DTWAIN_GetBrightnessStringA: Symbol<DtwaingetbrightnessstringaFunc> = unsafe { library.get(b"DTWAIN_GetBrightnessStringA")? };
        let DTWAIN_GetBrightnessStringW: Symbol<DtwaingetbrightnessstringwFunc> = unsafe { library.get(b"DTWAIN_GetBrightnessStringW")? };
        let DTWAIN_GetBufferedTransferInfo: Symbol<DtwaingetbufferedtransferinfoFunc> = unsafe { library.get(b"DTWAIN_GetBufferedTransferInfo")? };
        let DTWAIN_GetCallback: Symbol<DtwaingetcallbackFunc> = unsafe { library.get(b"DTWAIN_GetCallback")? };
        let DTWAIN_GetCallback64: Symbol<Dtwaingetcallback64Func> = unsafe { library.get(b"DTWAIN_GetCallback64")? };
        let DTWAIN_GetCapArrayType: Symbol<DtwaingetcaparraytypeFunc> = unsafe { library.get(b"DTWAIN_GetCapArrayType")? };
        let DTWAIN_GetCapContainer: Symbol<DtwaingetcapcontainerFunc> = unsafe { library.get(b"DTWAIN_GetCapContainer")? };
        let DTWAIN_GetCapContainerEx: Symbol<DtwaingetcapcontainerexFunc> = unsafe { library.get(b"DTWAIN_GetCapContainerEx")? };
        let DTWAIN_GetCapContainerEx2: Symbol<Dtwaingetcapcontainerex2Func> = unsafe { library.get(b"DTWAIN_GetCapContainerEx2")? };
        let DTWAIN_GetCapDataType: Symbol<DtwaingetcapdatatypeFunc> = unsafe { library.get(b"DTWAIN_GetCapDataType")? };
        let DTWAIN_GetCapFromName: Symbol<DtwaingetcapfromnameFunc> = unsafe { library.get(b"DTWAIN_GetCapFromName")? };
        let DTWAIN_GetCapFromNameA: Symbol<DtwaingetcapfromnameaFunc> = unsafe { library.get(b"DTWAIN_GetCapFromNameA")? };
        let DTWAIN_GetCapFromNameW: Symbol<DtwaingetcapfromnamewFunc> = unsafe { library.get(b"DTWAIN_GetCapFromNameW")? };
        let DTWAIN_GetCapOperations: Symbol<DtwaingetcapoperationsFunc> = unsafe { library.get(b"DTWAIN_GetCapOperations")? };
        let DTWAIN_GetCapValues: Symbol<DtwaingetcapvaluesFunc> = unsafe { library.get(b"DTWAIN_GetCapValues")? };
        let DTWAIN_GetCapValuesEx: Symbol<DtwaingetcapvaluesexFunc> = unsafe { library.get(b"DTWAIN_GetCapValuesEx")? };
        let DTWAIN_GetCapValuesEx2: Symbol<Dtwaingetcapvaluesex2Func> = unsafe { library.get(b"DTWAIN_GetCapValuesEx2")? };
        let DTWAIN_GetCaption: Symbol<DtwaingetcaptionFunc> = unsafe { library.get(b"DTWAIN_GetCaption")? };
        let DTWAIN_GetCaptionA: Symbol<DtwaingetcaptionaFunc> = unsafe { library.get(b"DTWAIN_GetCaptionA")? };
        let DTWAIN_GetCaptionW: Symbol<DtwaingetcaptionwFunc> = unsafe { library.get(b"DTWAIN_GetCaptionW")? };
        let DTWAIN_GetCompressionSize: Symbol<DtwaingetcompressionsizeFunc> = unsafe { library.get(b"DTWAIN_GetCompressionSize")? };
        let DTWAIN_GetCompressionType: Symbol<DtwaingetcompressiontypeFunc> = unsafe { library.get(b"DTWAIN_GetCompressionType")? };
        let DTWAIN_GetConditionCodeString: Symbol<DtwaingetconditioncodestringFunc> = unsafe { library.get(b"DTWAIN_GetConditionCodeString")? };
        let DTWAIN_GetConditionCodeStringA: Symbol<DtwaingetconditioncodestringaFunc> = unsafe { library.get(b"DTWAIN_GetConditionCodeStringA")? };
        let DTWAIN_GetConditionCodeStringW: Symbol<DtwaingetconditioncodestringwFunc> = unsafe { library.get(b"DTWAIN_GetConditionCodeStringW")? };
        let DTWAIN_GetContrast: Symbol<DtwaingetcontrastFunc> = unsafe { library.get(b"DTWAIN_GetContrast")? };
        let DTWAIN_GetContrastString: Symbol<DtwaingetcontraststringFunc> = unsafe { library.get(b"DTWAIN_GetContrastString")? };
        let DTWAIN_GetContrastStringA: Symbol<DtwaingetcontraststringaFunc> = unsafe { library.get(b"DTWAIN_GetContrastStringA")? };
        let DTWAIN_GetContrastStringW: Symbol<DtwaingetcontraststringwFunc> = unsafe { library.get(b"DTWAIN_GetContrastStringW")? };
        let DTWAIN_GetCountry: Symbol<DtwaingetcountryFunc> = unsafe { library.get(b"DTWAIN_GetCountry")? };
        let DTWAIN_GetCurrentAcquiredImage: Symbol<DtwaingetcurrentacquiredimageFunc> = unsafe { library.get(b"DTWAIN_GetCurrentAcquiredImage")? };
        let DTWAIN_GetCurrentFileName: Symbol<DtwaingetcurrentfilenameFunc> = unsafe { library.get(b"DTWAIN_GetCurrentFileName")? };
        let DTWAIN_GetCurrentFileNameA: Symbol<DtwaingetcurrentfilenameaFunc> = unsafe { library.get(b"DTWAIN_GetCurrentFileNameA")? };
        let DTWAIN_GetCurrentFileNameW: Symbol<DtwaingetcurrentfilenamewFunc> = unsafe { library.get(b"DTWAIN_GetCurrentFileNameW")? };
        let DTWAIN_GetCurrentPageNum: Symbol<DtwaingetcurrentpagenumFunc> = unsafe { library.get(b"DTWAIN_GetCurrentPageNum")? };
        let DTWAIN_GetCurrentRetryCount: Symbol<DtwaingetcurrentretrycountFunc> = unsafe { library.get(b"DTWAIN_GetCurrentRetryCount")? };
        let DTWAIN_GetCurrentTwainTriplet: Symbol<DtwaingetcurrenttwaintripletFunc> = unsafe { library.get(b"DTWAIN_GetCurrentTwainTriplet")? };
        let DTWAIN_GetCustomDSData: Symbol<DtwaingetcustomdsdataFunc> = unsafe { library.get(b"DTWAIN_GetCustomDSData")? };
        let DTWAIN_GetDSMFullName: Symbol<DtwaingetdsmfullnameFunc> = unsafe { library.get(b"DTWAIN_GetDSMFullName")? };
        let DTWAIN_GetDSMFullNameA: Symbol<DtwaingetdsmfullnameaFunc> = unsafe { library.get(b"DTWAIN_GetDSMFullNameA")? };
        let DTWAIN_GetDSMFullNameW: Symbol<DtwaingetdsmfullnamewFunc> = unsafe { library.get(b"DTWAIN_GetDSMFullNameW")? };
        let DTWAIN_GetDSMSearchOrder: Symbol<DtwaingetdsmsearchorderFunc> = unsafe { library.get(b"DTWAIN_GetDSMSearchOrder")? };
        let DTWAIN_GetDTWAINHandle: Symbol<DtwaingetdtwainhandleFunc> = unsafe { library.get(b"DTWAIN_GetDTWAINHandle")? };
        let DTWAIN_GetDeviceEvent: Symbol<DtwaingetdeviceeventFunc> = unsafe { library.get(b"DTWAIN_GetDeviceEvent")? };
        let DTWAIN_GetDeviceEventEx: Symbol<DtwaingetdeviceeventexFunc> = unsafe { library.get(b"DTWAIN_GetDeviceEventEx")? };
        let DTWAIN_GetDeviceEventInfo: Symbol<DtwaingetdeviceeventinfoFunc> = unsafe { library.get(b"DTWAIN_GetDeviceEventInfo")? };
        let DTWAIN_GetDeviceNotifications: Symbol<DtwaingetdevicenotificationsFunc> = unsafe { library.get(b"DTWAIN_GetDeviceNotifications")? };
        let DTWAIN_GetDeviceTimeDate: Symbol<DtwaingetdevicetimedateFunc> = unsafe { library.get(b"DTWAIN_GetDeviceTimeDate")? };
        let DTWAIN_GetDeviceTimeDateA: Symbol<DtwaingetdevicetimedateaFunc> = unsafe { library.get(b"DTWAIN_GetDeviceTimeDateA")? };
        let DTWAIN_GetDeviceTimeDateW: Symbol<DtwaingetdevicetimedatewFunc> = unsafe { library.get(b"DTWAIN_GetDeviceTimeDateW")? };
        let DTWAIN_GetDoubleFeedDetectLength: Symbol<DtwaingetdoublefeeddetectlengthFunc> = unsafe { library.get(b"DTWAIN_GetDoubleFeedDetectLength")? };
        let DTWAIN_GetDoubleFeedDetectValues: Symbol<DtwaingetdoublefeeddetectvaluesFunc> = unsafe { library.get(b"DTWAIN_GetDoubleFeedDetectValues")? };
        let DTWAIN_GetDuplexType: Symbol<DtwaingetduplextypeFunc> = unsafe { library.get(b"DTWAIN_GetDuplexType")? };
        let DTWAIN_GetErrorBuffer: Symbol<DtwaingeterrorbufferFunc> = unsafe { library.get(b"DTWAIN_GetErrorBuffer")? };
        let DTWAIN_GetErrorBufferThreshold: Symbol<DtwaingeterrorbufferthresholdFunc> = unsafe { library.get(b"DTWAIN_GetErrorBufferThreshold")? };
        let DTWAIN_GetErrorCallback: Symbol<DtwaingeterrorcallbackFunc> = unsafe { library.get(b"DTWAIN_GetErrorCallback")? };
        let DTWAIN_GetErrorCallback64: Symbol<Dtwaingeterrorcallback64Func> = unsafe { library.get(b"DTWAIN_GetErrorCallback64")? };
        let DTWAIN_GetErrorString: Symbol<DtwaingeterrorstringFunc> = unsafe { library.get(b"DTWAIN_GetErrorString")? };
        let DTWAIN_GetErrorStringA: Symbol<DtwaingeterrorstringaFunc> = unsafe { library.get(b"DTWAIN_GetErrorStringA")? };
        let DTWAIN_GetErrorStringW: Symbol<DtwaingeterrorstringwFunc> = unsafe { library.get(b"DTWAIN_GetErrorStringW")? };
        let DTWAIN_GetExtCapFromName: Symbol<DtwaingetextcapfromnameFunc> = unsafe { library.get(b"DTWAIN_GetExtCapFromName")? };
        let DTWAIN_GetExtCapFromNameA: Symbol<DtwaingetextcapfromnameaFunc> = unsafe { library.get(b"DTWAIN_GetExtCapFromNameA")? };
        let DTWAIN_GetExtCapFromNameW: Symbol<DtwaingetextcapfromnamewFunc> = unsafe { library.get(b"DTWAIN_GetExtCapFromNameW")? };
        let DTWAIN_GetExtImageInfo: Symbol<DtwaingetextimageinfoFunc> = unsafe { library.get(b"DTWAIN_GetExtImageInfo")? };
        let DTWAIN_GetExtImageInfoData: Symbol<DtwaingetextimageinfodataFunc> = unsafe { library.get(b"DTWAIN_GetExtImageInfoData")? };
        let DTWAIN_GetExtImageInfoDataEx: Symbol<DtwaingetextimageinfodataexFunc> = unsafe { library.get(b"DTWAIN_GetExtImageInfoDataEx")? };
        let DTWAIN_GetExtImageInfoItem: Symbol<DtwaingetextimageinfoitemFunc> = unsafe { library.get(b"DTWAIN_GetExtImageInfoItem")? };
        let DTWAIN_GetExtImageInfoItemEx: Symbol<DtwaingetextimageinfoitemexFunc> = unsafe { library.get(b"DTWAIN_GetExtImageInfoItemEx")? };
        let DTWAIN_GetExtNameFromCap: Symbol<DtwaingetextnamefromcapFunc> = unsafe { library.get(b"DTWAIN_GetExtNameFromCap")? };
        let DTWAIN_GetExtNameFromCapA: Symbol<DtwaingetextnamefromcapaFunc> = unsafe { library.get(b"DTWAIN_GetExtNameFromCapA")? };
        let DTWAIN_GetExtNameFromCapW: Symbol<DtwaingetextnamefromcapwFunc> = unsafe { library.get(b"DTWAIN_GetExtNameFromCapW")? };
        let DTWAIN_GetFeederAlignment: Symbol<DtwaingetfeederalignmentFunc> = unsafe { library.get(b"DTWAIN_GetFeederAlignment")? };
        let DTWAIN_GetFeederFuncs: Symbol<DtwaingetfeederfuncsFunc> = unsafe { library.get(b"DTWAIN_GetFeederFuncs")? };
        let DTWAIN_GetFeederOrder: Symbol<DtwaingetfeederorderFunc> = unsafe { library.get(b"DTWAIN_GetFeederOrder")? };
        let DTWAIN_GetFeederWaitTime: Symbol<DtwaingetfeederwaittimeFunc> = unsafe { library.get(b"DTWAIN_GetFeederWaitTime")? };
        let DTWAIN_GetFileCompressionType: Symbol<DtwaingetfilecompressiontypeFunc> = unsafe { library.get(b"DTWAIN_GetFileCompressionType")? };
        let DTWAIN_GetFileTypeExtensions: Symbol<DtwaingetfiletypeextensionsFunc> = unsafe { library.get(b"DTWAIN_GetFileTypeExtensions")? };
        let DTWAIN_GetFileTypeExtensionsA: Symbol<DtwaingetfiletypeextensionsaFunc> = unsafe { library.get(b"DTWAIN_GetFileTypeExtensionsA")? };
        let DTWAIN_GetFileTypeExtensionsW: Symbol<DtwaingetfiletypeextensionswFunc> = unsafe { library.get(b"DTWAIN_GetFileTypeExtensionsW")? };
        let DTWAIN_GetFileTypeName: Symbol<DtwaingetfiletypenameFunc> = unsafe { library.get(b"DTWAIN_GetFileTypeName")? };
        let DTWAIN_GetFileTypeNameA: Symbol<DtwaingetfiletypenameaFunc> = unsafe { library.get(b"DTWAIN_GetFileTypeNameA")? };
        let DTWAIN_GetFileTypeNameW: Symbol<DtwaingetfiletypenamewFunc> = unsafe { library.get(b"DTWAIN_GetFileTypeNameW")? };
        let DTWAIN_GetHalftone: Symbol<DtwaingethalftoneFunc> = unsafe { library.get(b"DTWAIN_GetHalftone")? };
        let DTWAIN_GetHalftoneA: Symbol<DtwaingethalftoneaFunc> = unsafe { library.get(b"DTWAIN_GetHalftoneA")? };
        let DTWAIN_GetHalftoneW: Symbol<DtwaingethalftonewFunc> = unsafe { library.get(b"DTWAIN_GetHalftoneW")? };
        let DTWAIN_GetHighlight: Symbol<DtwaingethighlightFunc> = unsafe { library.get(b"DTWAIN_GetHighlight")? };
        let DTWAIN_GetHighlightString: Symbol<DtwaingethighlightstringFunc> = unsafe { library.get(b"DTWAIN_GetHighlightString")? };
        let DTWAIN_GetHighlightStringA: Symbol<DtwaingethighlightstringaFunc> = unsafe { library.get(b"DTWAIN_GetHighlightStringA")? };
        let DTWAIN_GetHighlightStringW: Symbol<DtwaingethighlightstringwFunc> = unsafe { library.get(b"DTWAIN_GetHighlightStringW")? };
        let DTWAIN_GetImageInfo: Symbol<DtwaingetimageinfoFunc> = unsafe { library.get(b"DTWAIN_GetImageInfo")? };
        let DTWAIN_GetImageInfoString: Symbol<DtwaingetimageinfostringFunc> = unsafe { library.get(b"DTWAIN_GetImageInfoString")? };
        let DTWAIN_GetImageInfoStringA: Symbol<DtwaingetimageinfostringaFunc> = unsafe { library.get(b"DTWAIN_GetImageInfoStringA")? };
        let DTWAIN_GetImageInfoStringW: Symbol<DtwaingetimageinfostringwFunc> = unsafe { library.get(b"DTWAIN_GetImageInfoStringW")? };
        let DTWAIN_GetJobControl: Symbol<DtwaingetjobcontrolFunc> = unsafe { library.get(b"DTWAIN_GetJobControl")? };
        let DTWAIN_GetJpegValues: Symbol<DtwaingetjpegvaluesFunc> = unsafe { library.get(b"DTWAIN_GetJpegValues")? };
        let DTWAIN_GetJpegXRValues: Symbol<DtwaingetjpegxrvaluesFunc> = unsafe { library.get(b"DTWAIN_GetJpegXRValues")? };
        let DTWAIN_GetLanguage: Symbol<DtwaingetlanguageFunc> = unsafe { library.get(b"DTWAIN_GetLanguage")? };
        let DTWAIN_GetLastError: Symbol<DtwaingetlasterrorFunc> = unsafe { library.get(b"DTWAIN_GetLastError")? };
        let DTWAIN_GetLibraryPath: Symbol<DtwaingetlibrarypathFunc> = unsafe { library.get(b"DTWAIN_GetLibraryPath")? };
        let DTWAIN_GetLibraryPathA: Symbol<DtwaingetlibrarypathaFunc> = unsafe { library.get(b"DTWAIN_GetLibraryPathA")? };
        let DTWAIN_GetLibraryPathW: Symbol<DtwaingetlibrarypathwFunc> = unsafe { library.get(b"DTWAIN_GetLibraryPathW")? };
        let DTWAIN_GetLightPath: Symbol<DtwaingetlightpathFunc> = unsafe { library.get(b"DTWAIN_GetLightPath")? };
        let DTWAIN_GetLightSource: Symbol<DtwaingetlightsourceFunc> = unsafe { library.get(b"DTWAIN_GetLightSource")? };
        let DTWAIN_GetLightSources: Symbol<DtwaingetlightsourcesFunc> = unsafe { library.get(b"DTWAIN_GetLightSources")? };
        let DTWAIN_GetLoggerCallback: Symbol<DtwaingetloggercallbackFunc> = unsafe { library.get(b"DTWAIN_GetLoggerCallback")? };
        let DTWAIN_GetLoggerCallbackA: Symbol<DtwaingetloggercallbackaFunc> = unsafe { library.get(b"DTWAIN_GetLoggerCallbackA")? };
        let DTWAIN_GetLoggerCallbackW: Symbol<DtwaingetloggercallbackwFunc> = unsafe { library.get(b"DTWAIN_GetLoggerCallbackW")? };
        let DTWAIN_GetManualDuplexCount: Symbol<DtwaingetmanualduplexcountFunc> = unsafe { library.get(b"DTWAIN_GetManualDuplexCount")? };
        let DTWAIN_GetMaxAcquisitions: Symbol<DtwaingetmaxacquisitionsFunc> = unsafe { library.get(b"DTWAIN_GetMaxAcquisitions")? };
        let DTWAIN_GetMaxBuffers: Symbol<DtwaingetmaxbuffersFunc> = unsafe { library.get(b"DTWAIN_GetMaxBuffers")? };
        let DTWAIN_GetMaxPagesToAcquire: Symbol<DtwaingetmaxpagestoacquireFunc> = unsafe { library.get(b"DTWAIN_GetMaxPagesToAcquire")? };
        let DTWAIN_GetMaxRetryAttempts: Symbol<DtwaingetmaxretryattemptsFunc> = unsafe { library.get(b"DTWAIN_GetMaxRetryAttempts")? };
        let DTWAIN_GetNameFromCap: Symbol<DtwaingetnamefromcapFunc> = unsafe { library.get(b"DTWAIN_GetNameFromCap")? };
        let DTWAIN_GetNameFromCapA: Symbol<DtwaingetnamefromcapaFunc> = unsafe { library.get(b"DTWAIN_GetNameFromCapA")? };
        let DTWAIN_GetNameFromCapW: Symbol<DtwaingetnamefromcapwFunc> = unsafe { library.get(b"DTWAIN_GetNameFromCapW")? };
        let DTWAIN_GetNoiseFilter: Symbol<DtwaingetnoisefilterFunc> = unsafe { library.get(b"DTWAIN_GetNoiseFilter")? };
        let DTWAIN_GetNumAcquiredImages: Symbol<DtwaingetnumacquiredimagesFunc> = unsafe { library.get(b"DTWAIN_GetNumAcquiredImages")? };
        let DTWAIN_GetNumAcquisitions: Symbol<DtwaingetnumacquisitionsFunc> = unsafe { library.get(b"DTWAIN_GetNumAcquisitions")? };
        let DTWAIN_GetOCRCapValues: Symbol<DtwaingetocrcapvaluesFunc> = unsafe { library.get(b"DTWAIN_GetOCRCapValues")? };
        let DTWAIN_GetOCRErrorString: Symbol<DtwaingetocrerrorstringFunc> = unsafe { library.get(b"DTWAIN_GetOCRErrorString")? };
        let DTWAIN_GetOCRErrorStringA: Symbol<DtwaingetocrerrorstringaFunc> = unsafe { library.get(b"DTWAIN_GetOCRErrorStringA")? };
        let DTWAIN_GetOCRErrorStringW: Symbol<DtwaingetocrerrorstringwFunc> = unsafe { library.get(b"DTWAIN_GetOCRErrorStringW")? };
        let DTWAIN_GetOCRLastError: Symbol<DtwaingetocrlasterrorFunc> = unsafe { library.get(b"DTWAIN_GetOCRLastError")? };
        let DTWAIN_GetOCRMajorMinorVersion: Symbol<DtwaingetocrmajorminorversionFunc> = unsafe { library.get(b"DTWAIN_GetOCRMajorMinorVersion")? };
        let DTWAIN_GetOCRManufacturer: Symbol<DtwaingetocrmanufacturerFunc> = unsafe { library.get(b"DTWAIN_GetOCRManufacturer")? };
        let DTWAIN_GetOCRManufacturerA: Symbol<DtwaingetocrmanufactureraFunc> = unsafe { library.get(b"DTWAIN_GetOCRManufacturerA")? };
        let DTWAIN_GetOCRManufacturerW: Symbol<DtwaingetocrmanufacturerwFunc> = unsafe { library.get(b"DTWAIN_GetOCRManufacturerW")? };
        let DTWAIN_GetOCRProductFamily: Symbol<DtwaingetocrproductfamilyFunc> = unsafe { library.get(b"DTWAIN_GetOCRProductFamily")? };
        let DTWAIN_GetOCRProductFamilyA: Symbol<DtwaingetocrproductfamilyaFunc> = unsafe { library.get(b"DTWAIN_GetOCRProductFamilyA")? };
        let DTWAIN_GetOCRProductFamilyW: Symbol<DtwaingetocrproductfamilywFunc> = unsafe { library.get(b"DTWAIN_GetOCRProductFamilyW")? };
        let DTWAIN_GetOCRProductName: Symbol<DtwaingetocrproductnameFunc> = unsafe { library.get(b"DTWAIN_GetOCRProductName")? };
        let DTWAIN_GetOCRProductNameA: Symbol<DtwaingetocrproductnameaFunc> = unsafe { library.get(b"DTWAIN_GetOCRProductNameA")? };
        let DTWAIN_GetOCRProductNameW: Symbol<DtwaingetocrproductnamewFunc> = unsafe { library.get(b"DTWAIN_GetOCRProductNameW")? };
        let DTWAIN_GetOCRText: Symbol<DtwaingetocrtextFunc> = unsafe { library.get(b"DTWAIN_GetOCRText")? };
        let DTWAIN_GetOCRTextA: Symbol<DtwaingetocrtextaFunc> = unsafe { library.get(b"DTWAIN_GetOCRTextA")? };
        let DTWAIN_GetOCRTextInfoFloat: Symbol<DtwaingetocrtextinfofloatFunc> = unsafe { library.get(b"DTWAIN_GetOCRTextInfoFloat")? };
        let DTWAIN_GetOCRTextInfoFloatEx: Symbol<DtwaingetocrtextinfofloatexFunc> = unsafe { library.get(b"DTWAIN_GetOCRTextInfoFloatEx")? };
        let DTWAIN_GetOCRTextInfoHandle: Symbol<DtwaingetocrtextinfohandleFunc> = unsafe { library.get(b"DTWAIN_GetOCRTextInfoHandle")? };
        let DTWAIN_GetOCRTextInfoLong: Symbol<DtwaingetocrtextinfolongFunc> = unsafe { library.get(b"DTWAIN_GetOCRTextInfoLong")? };
        let DTWAIN_GetOCRTextInfoLongEx: Symbol<DtwaingetocrtextinfolongexFunc> = unsafe { library.get(b"DTWAIN_GetOCRTextInfoLongEx")? };
        let DTWAIN_GetOCRTextW: Symbol<DtwaingetocrtextwFunc> = unsafe { library.get(b"DTWAIN_GetOCRTextW")? };
        let DTWAIN_GetOCRVersionInfo: Symbol<DtwaingetocrversioninfoFunc> = unsafe { library.get(b"DTWAIN_GetOCRVersionInfo")? };
        let DTWAIN_GetOCRVersionInfoA: Symbol<DtwaingetocrversioninfoaFunc> = unsafe { library.get(b"DTWAIN_GetOCRVersionInfoA")? };
        let DTWAIN_GetOCRVersionInfoW: Symbol<DtwaingetocrversioninfowFunc> = unsafe { library.get(b"DTWAIN_GetOCRVersionInfoW")? };
        let DTWAIN_GetOrientation: Symbol<DtwaingetorientationFunc> = unsafe { library.get(b"DTWAIN_GetOrientation")? };
        let DTWAIN_GetOverscan: Symbol<DtwaingetoverscanFunc> = unsafe { library.get(b"DTWAIN_GetOverscan")? };
        let DTWAIN_GetPDFTextElementFloat: Symbol<DtwaingetpdftextelementfloatFunc> = unsafe { library.get(b"DTWAIN_GetPDFTextElementFloat")? };
        let DTWAIN_GetPDFTextElementLong: Symbol<DtwaingetpdftextelementlongFunc> = unsafe { library.get(b"DTWAIN_GetPDFTextElementLong")? };
        let DTWAIN_GetPDFTextElementString: Symbol<DtwaingetpdftextelementstringFunc> = unsafe { library.get(b"DTWAIN_GetPDFTextElementString")? };
        let DTWAIN_GetPDFTextElementStringA: Symbol<DtwaingetpdftextelementstringaFunc> = unsafe { library.get(b"DTWAIN_GetPDFTextElementStringA")? };
        let DTWAIN_GetPDFTextElementStringW: Symbol<DtwaingetpdftextelementstringwFunc> = unsafe { library.get(b"DTWAIN_GetPDFTextElementStringW")? };
        let DTWAIN_GetPDFType1FontName: Symbol<Dtwaingetpdftype1fontnameFunc> = unsafe { library.get(b"DTWAIN_GetPDFType1FontName")? };
        let DTWAIN_GetPDFType1FontNameA: Symbol<Dtwaingetpdftype1fontnameaFunc> = unsafe { library.get(b"DTWAIN_GetPDFType1FontNameA")? };
        let DTWAIN_GetPDFType1FontNameW: Symbol<Dtwaingetpdftype1fontnamewFunc> = unsafe { library.get(b"DTWAIN_GetPDFType1FontNameW")? };
        let DTWAIN_GetPaperSize: Symbol<DtwaingetpapersizeFunc> = unsafe { library.get(b"DTWAIN_GetPaperSize")? };
        let DTWAIN_GetPaperSizeName: Symbol<DtwaingetpapersizenameFunc> = unsafe { library.get(b"DTWAIN_GetPaperSizeName")? };
        let DTWAIN_GetPaperSizeNameA: Symbol<DtwaingetpapersizenameaFunc> = unsafe { library.get(b"DTWAIN_GetPaperSizeNameA")? };
        let DTWAIN_GetPaperSizeNameW: Symbol<DtwaingetpapersizenamewFunc> = unsafe { library.get(b"DTWAIN_GetPaperSizeNameW")? };
        let DTWAIN_GetPatchMaxPriorities: Symbol<DtwaingetpatchmaxprioritiesFunc> = unsafe { library.get(b"DTWAIN_GetPatchMaxPriorities")? };
        let DTWAIN_GetPatchMaxRetries: Symbol<DtwaingetpatchmaxretriesFunc> = unsafe { library.get(b"DTWAIN_GetPatchMaxRetries")? };
        let DTWAIN_GetPatchPriorities: Symbol<DtwaingetpatchprioritiesFunc> = unsafe { library.get(b"DTWAIN_GetPatchPriorities")? };
        let DTWAIN_GetPatchSearchMode: Symbol<DtwaingetpatchsearchmodeFunc> = unsafe { library.get(b"DTWAIN_GetPatchSearchMode")? };
        let DTWAIN_GetPatchTimeOut: Symbol<DtwaingetpatchtimeoutFunc> = unsafe { library.get(b"DTWAIN_GetPatchTimeOut")? };
        let DTWAIN_GetPixelFlavor: Symbol<DtwaingetpixelflavorFunc> = unsafe { library.get(b"DTWAIN_GetPixelFlavor")? };
        let DTWAIN_GetPixelType: Symbol<DtwaingetpixeltypeFunc> = unsafe { library.get(b"DTWAIN_GetPixelType")? };
        let DTWAIN_GetPrinter: Symbol<DtwaingetprinterFunc> = unsafe { library.get(b"DTWAIN_GetPrinter")? };
        let DTWAIN_GetPrinterStartNumber: Symbol<DtwaingetprinterstartnumberFunc> = unsafe { library.get(b"DTWAIN_GetPrinterStartNumber")? };
        let DTWAIN_GetPrinterStringMode: Symbol<DtwaingetprinterstringmodeFunc> = unsafe { library.get(b"DTWAIN_GetPrinterStringMode")? };
        let DTWAIN_GetPrinterStrings: Symbol<DtwaingetprinterstringsFunc> = unsafe { library.get(b"DTWAIN_GetPrinterStrings")? };
        let DTWAIN_GetPrinterSuffixString: Symbol<DtwaingetprintersuffixstringFunc> = unsafe { library.get(b"DTWAIN_GetPrinterSuffixString")? };
        let DTWAIN_GetPrinterSuffixStringA: Symbol<DtwaingetprintersuffixstringaFunc> = unsafe { library.get(b"DTWAIN_GetPrinterSuffixStringA")? };
        let DTWAIN_GetPrinterSuffixStringW: Symbol<DtwaingetprintersuffixstringwFunc> = unsafe { library.get(b"DTWAIN_GetPrinterSuffixStringW")? };
        let DTWAIN_GetRegisteredMsg: Symbol<DtwaingetregisteredmsgFunc> = unsafe { library.get(b"DTWAIN_GetRegisteredMsg")? };
        let DTWAIN_GetResolution: Symbol<DtwaingetresolutionFunc> = unsafe { library.get(b"DTWAIN_GetResolution")? };
        let DTWAIN_GetResolutionString: Symbol<DtwaingetresolutionstringFunc> = unsafe { library.get(b"DTWAIN_GetResolutionString")? };
        let DTWAIN_GetResolutionStringA: Symbol<DtwaingetresolutionstringaFunc> = unsafe { library.get(b"DTWAIN_GetResolutionStringA")? };
        let DTWAIN_GetResolutionStringW: Symbol<DtwaingetresolutionstringwFunc> = unsafe { library.get(b"DTWAIN_GetResolutionStringW")? };
        let DTWAIN_GetResourceString: Symbol<DtwaingetresourcestringFunc> = unsafe { library.get(b"DTWAIN_GetResourceString")? };
        let DTWAIN_GetResourceStringA: Symbol<DtwaingetresourcestringaFunc> = unsafe { library.get(b"DTWAIN_GetResourceStringA")? };
        let DTWAIN_GetResourceStringW: Symbol<DtwaingetresourcestringwFunc> = unsafe { library.get(b"DTWAIN_GetResourceStringW")? };
        let DTWAIN_GetRotation: Symbol<DtwaingetrotationFunc> = unsafe { library.get(b"DTWAIN_GetRotation")? };
        let DTWAIN_GetRotationString: Symbol<DtwaingetrotationstringFunc> = unsafe { library.get(b"DTWAIN_GetRotationString")? };
        let DTWAIN_GetRotationStringA: Symbol<DtwaingetrotationstringaFunc> = unsafe { library.get(b"DTWAIN_GetRotationStringA")? };
        let DTWAIN_GetRotationStringW: Symbol<DtwaingetrotationstringwFunc> = unsafe { library.get(b"DTWAIN_GetRotationStringW")? };
        let DTWAIN_GetSaveFileName: Symbol<DtwaingetsavefilenameFunc> = unsafe { library.get(b"DTWAIN_GetSaveFileName")? };
        let DTWAIN_GetSaveFileNameA: Symbol<DtwaingetsavefilenameaFunc> = unsafe { library.get(b"DTWAIN_GetSaveFileNameA")? };
        let DTWAIN_GetSaveFileNameW: Symbol<DtwaingetsavefilenamewFunc> = unsafe { library.get(b"DTWAIN_GetSaveFileNameW")? };
        let DTWAIN_GetSavedFilesCount: Symbol<DtwaingetsavedfilescountFunc> = unsafe { library.get(b"DTWAIN_GetSavedFilesCount")? };
        let DTWAIN_GetSessionDetails: Symbol<DtwaingetsessiondetailsFunc> = unsafe { library.get(b"DTWAIN_GetSessionDetails")? };
        let DTWAIN_GetSessionDetailsA: Symbol<DtwaingetsessiondetailsaFunc> = unsafe { library.get(b"DTWAIN_GetSessionDetailsA")? };
        let DTWAIN_GetSessionDetailsW: Symbol<DtwaingetsessiondetailswFunc> = unsafe { library.get(b"DTWAIN_GetSessionDetailsW")? };
        let DTWAIN_GetShadow: Symbol<DtwaingetshadowFunc> = unsafe { library.get(b"DTWAIN_GetShadow")? };
        let DTWAIN_GetShadowString: Symbol<DtwaingetshadowstringFunc> = unsafe { library.get(b"DTWAIN_GetShadowString")? };
        let DTWAIN_GetShadowStringA: Symbol<DtwaingetshadowstringaFunc> = unsafe { library.get(b"DTWAIN_GetShadowStringA")? };
        let DTWAIN_GetShadowStringW: Symbol<DtwaingetshadowstringwFunc> = unsafe { library.get(b"DTWAIN_GetShadowStringW")? };
        let DTWAIN_GetShortVersionString: Symbol<DtwaingetshortversionstringFunc> = unsafe { library.get(b"DTWAIN_GetShortVersionString")? };
        let DTWAIN_GetShortVersionStringA: Symbol<DtwaingetshortversionstringaFunc> = unsafe { library.get(b"DTWAIN_GetShortVersionStringA")? };
        let DTWAIN_GetShortVersionStringW: Symbol<DtwaingetshortversionstringwFunc> = unsafe { library.get(b"DTWAIN_GetShortVersionStringW")? };
        let DTWAIN_GetSourceAcquisitions: Symbol<DtwaingetsourceacquisitionsFunc> = unsafe { library.get(b"DTWAIN_GetSourceAcquisitions")? };
        let DTWAIN_GetSourceDetails: Symbol<DtwaingetsourcedetailsFunc> = unsafe { library.get(b"DTWAIN_GetSourceDetails")? };
        let DTWAIN_GetSourceDetailsA: Symbol<DtwaingetsourcedetailsaFunc> = unsafe { library.get(b"DTWAIN_GetSourceDetailsA")? };
        let DTWAIN_GetSourceDetailsW: Symbol<DtwaingetsourcedetailswFunc> = unsafe { library.get(b"DTWAIN_GetSourceDetailsW")? };
        let DTWAIN_GetSourceID: Symbol<DtwaingetsourceidFunc> = unsafe { library.get(b"DTWAIN_GetSourceID")? };
        let DTWAIN_GetSourceIDEx: Symbol<DtwaingetsourceidexFunc> = unsafe { library.get(b"DTWAIN_GetSourceIDEx")? };
        let DTWAIN_GetSourceManufacturer: Symbol<DtwaingetsourcemanufacturerFunc> = unsafe { library.get(b"DTWAIN_GetSourceManufacturer")? };
        let DTWAIN_GetSourceManufacturerA: Symbol<DtwaingetsourcemanufactureraFunc> = unsafe { library.get(b"DTWAIN_GetSourceManufacturerA")? };
        let DTWAIN_GetSourceManufacturerW: Symbol<DtwaingetsourcemanufacturerwFunc> = unsafe { library.get(b"DTWAIN_GetSourceManufacturerW")? };
        let DTWAIN_GetSourceProductFamily: Symbol<DtwaingetsourceproductfamilyFunc> = unsafe { library.get(b"DTWAIN_GetSourceProductFamily")? };
        let DTWAIN_GetSourceProductFamilyA: Symbol<DtwaingetsourceproductfamilyaFunc> = unsafe { library.get(b"DTWAIN_GetSourceProductFamilyA")? };
        let DTWAIN_GetSourceProductFamilyW: Symbol<DtwaingetsourceproductfamilywFunc> = unsafe { library.get(b"DTWAIN_GetSourceProductFamilyW")? };
        let DTWAIN_GetSourceProductName: Symbol<DtwaingetsourceproductnameFunc> = unsafe { library.get(b"DTWAIN_GetSourceProductName")? };
        let DTWAIN_GetSourceProductNameA: Symbol<DtwaingetsourceproductnameaFunc> = unsafe { library.get(b"DTWAIN_GetSourceProductNameA")? };
        let DTWAIN_GetSourceProductNameW: Symbol<DtwaingetsourceproductnamewFunc> = unsafe { library.get(b"DTWAIN_GetSourceProductNameW")? };
        let DTWAIN_GetSourceUnit: Symbol<DtwaingetsourceunitFunc> = unsafe { library.get(b"DTWAIN_GetSourceUnit")? };
        let DTWAIN_GetSourceVersionInfo: Symbol<DtwaingetsourceversioninfoFunc> = unsafe { library.get(b"DTWAIN_GetSourceVersionInfo")? };
        let DTWAIN_GetSourceVersionInfoA: Symbol<DtwaingetsourceversioninfoaFunc> = unsafe { library.get(b"DTWAIN_GetSourceVersionInfoA")? };
        let DTWAIN_GetSourceVersionInfoW: Symbol<DtwaingetsourceversioninfowFunc> = unsafe { library.get(b"DTWAIN_GetSourceVersionInfoW")? };
        let DTWAIN_GetSourceVersionNumber: Symbol<DtwaingetsourceversionnumberFunc> = unsafe { library.get(b"DTWAIN_GetSourceVersionNumber")? };
        let DTWAIN_GetStaticLibVersion: Symbol<DtwaingetstaticlibversionFunc> = unsafe { library.get(b"DTWAIN_GetStaticLibVersion")? };
        let DTWAIN_GetTempFileDirectory: Symbol<DtwaingettempfiledirectoryFunc> = unsafe { library.get(b"DTWAIN_GetTempFileDirectory")? };
        let DTWAIN_GetTempFileDirectoryA: Symbol<DtwaingettempfiledirectoryaFunc> = unsafe { library.get(b"DTWAIN_GetTempFileDirectoryA")? };
        let DTWAIN_GetTempFileDirectoryW: Symbol<DtwaingettempfiledirectorywFunc> = unsafe { library.get(b"DTWAIN_GetTempFileDirectoryW")? };
        let DTWAIN_GetThreshold: Symbol<DtwaingetthresholdFunc> = unsafe { library.get(b"DTWAIN_GetThreshold")? };
        let DTWAIN_GetThresholdString: Symbol<DtwaingetthresholdstringFunc> = unsafe { library.get(b"DTWAIN_GetThresholdString")? };
        let DTWAIN_GetThresholdStringA: Symbol<DtwaingetthresholdstringaFunc> = unsafe { library.get(b"DTWAIN_GetThresholdStringA")? };
        let DTWAIN_GetThresholdStringW: Symbol<DtwaingetthresholdstringwFunc> = unsafe { library.get(b"DTWAIN_GetThresholdStringW")? };
        let DTWAIN_GetTimeDate: Symbol<DtwaingettimedateFunc> = unsafe { library.get(b"DTWAIN_GetTimeDate")? };
        let DTWAIN_GetTimeDateA: Symbol<DtwaingettimedateaFunc> = unsafe { library.get(b"DTWAIN_GetTimeDateA")? };
        let DTWAIN_GetTimeDateW: Symbol<DtwaingettimedatewFunc> = unsafe { library.get(b"DTWAIN_GetTimeDateW")? };
        let DTWAIN_GetTwainAppID: Symbol<DtwaingettwainappidFunc> = unsafe { library.get(b"DTWAIN_GetTwainAppID")? };
        let DTWAIN_GetTwainAppIDEx: Symbol<DtwaingettwainappidexFunc> = unsafe { library.get(b"DTWAIN_GetTwainAppIDEx")? };
        let DTWAIN_GetTwainAvailability: Symbol<DtwaingettwainavailabilityFunc> = unsafe { library.get(b"DTWAIN_GetTwainAvailability")? };
        let DTWAIN_GetTwainAvailabilityEx: Symbol<DtwaingettwainavailabilityexFunc> = unsafe { library.get(b"DTWAIN_GetTwainAvailabilityEx")? };
        let DTWAIN_GetTwainAvailabilityExA: Symbol<DtwaingettwainavailabilityexaFunc> = unsafe { library.get(b"DTWAIN_GetTwainAvailabilityExA")? };
        let DTWAIN_GetTwainAvailabilityExW: Symbol<DtwaingettwainavailabilityexwFunc> = unsafe { library.get(b"DTWAIN_GetTwainAvailabilityExW")? };
        let DTWAIN_GetTwainCountryName: Symbol<DtwaingettwaincountrynameFunc> = unsafe { library.get(b"DTWAIN_GetTwainCountryName")? };
        let DTWAIN_GetTwainCountryNameA: Symbol<DtwaingettwaincountrynameaFunc> = unsafe { library.get(b"DTWAIN_GetTwainCountryNameA")? };
        let DTWAIN_GetTwainCountryNameW: Symbol<DtwaingettwaincountrynamewFunc> = unsafe { library.get(b"DTWAIN_GetTwainCountryNameW")? };
        let DTWAIN_GetTwainCountryValue: Symbol<DtwaingettwaincountryvalueFunc> = unsafe { library.get(b"DTWAIN_GetTwainCountryValue")? };
        let DTWAIN_GetTwainCountryValueA: Symbol<DtwaingettwaincountryvalueaFunc> = unsafe { library.get(b"DTWAIN_GetTwainCountryValueA")? };
        let DTWAIN_GetTwainCountryValueW: Symbol<DtwaingettwaincountryvaluewFunc> = unsafe { library.get(b"DTWAIN_GetTwainCountryValueW")? };
        let DTWAIN_GetTwainHwnd: Symbol<DtwaingettwainhwndFunc> = unsafe { library.get(b"DTWAIN_GetTwainHwnd")? };
        let DTWAIN_GetTwainIDFromName: Symbol<DtwaingettwainidfromnameFunc> = unsafe { library.get(b"DTWAIN_GetTwainIDFromName")? };
        let DTWAIN_GetTwainIDFromNameA: Symbol<DtwaingettwainidfromnameaFunc> = unsafe { library.get(b"DTWAIN_GetTwainIDFromNameA")? };
        let DTWAIN_GetTwainIDFromNameW: Symbol<DtwaingettwainidfromnamewFunc> = unsafe { library.get(b"DTWAIN_GetTwainIDFromNameW")? };
        let DTWAIN_GetTwainLanguageName: Symbol<DtwaingettwainlanguagenameFunc> = unsafe { library.get(b"DTWAIN_GetTwainLanguageName")? };
        let DTWAIN_GetTwainLanguageNameA: Symbol<DtwaingettwainlanguagenameaFunc> = unsafe { library.get(b"DTWAIN_GetTwainLanguageNameA")? };
        let DTWAIN_GetTwainLanguageNameW: Symbol<DtwaingettwainlanguagenamewFunc> = unsafe { library.get(b"DTWAIN_GetTwainLanguageNameW")? };
        let DTWAIN_GetTwainLanguageValue: Symbol<DtwaingettwainlanguagevalueFunc> = unsafe { library.get(b"DTWAIN_GetTwainLanguageValue")? };
        let DTWAIN_GetTwainLanguageValueA: Symbol<DtwaingettwainlanguagevalueaFunc> = unsafe { library.get(b"DTWAIN_GetTwainLanguageValueA")? };
        let DTWAIN_GetTwainLanguageValueW: Symbol<DtwaingettwainlanguagevaluewFunc> = unsafe { library.get(b"DTWAIN_GetTwainLanguageValueW")? };
        let DTWAIN_GetTwainMode: Symbol<DtwaingettwainmodeFunc> = unsafe { library.get(b"DTWAIN_GetTwainMode")? };
        let DTWAIN_GetTwainNameFromConstant: Symbol<DtwaingettwainnamefromconstantFunc> = unsafe { library.get(b"DTWAIN_GetTwainNameFromConstant")? };
        let DTWAIN_GetTwainNameFromConstantA: Symbol<DtwaingettwainnamefromconstantaFunc> = unsafe { library.get(b"DTWAIN_GetTwainNameFromConstantA")? };
        let DTWAIN_GetTwainNameFromConstantW: Symbol<DtwaingettwainnamefromconstantwFunc> = unsafe { library.get(b"DTWAIN_GetTwainNameFromConstantW")? };
        let DTWAIN_GetTwainStringName: Symbol<DtwaingettwainstringnameFunc> = unsafe { library.get(b"DTWAIN_GetTwainStringName")? };
        let DTWAIN_GetTwainStringNameA: Symbol<DtwaingettwainstringnameaFunc> = unsafe { library.get(b"DTWAIN_GetTwainStringNameA")? };
        let DTWAIN_GetTwainStringNameW: Symbol<DtwaingettwainstringnamewFunc> = unsafe { library.get(b"DTWAIN_GetTwainStringNameW")? };
        let DTWAIN_GetTwainTimeout: Symbol<DtwaingettwaintimeoutFunc> = unsafe { library.get(b"DTWAIN_GetTwainTimeout")? };
        let DTWAIN_GetVersion: Symbol<DtwaingetversionFunc> = unsafe { library.get(b"DTWAIN_GetVersion")? };
        let DTWAIN_GetVersionCopyright: Symbol<DtwaingetversioncopyrightFunc> = unsafe { library.get(b"DTWAIN_GetVersionCopyright")? };
        let DTWAIN_GetVersionCopyrightA: Symbol<DtwaingetversioncopyrightaFunc> = unsafe { library.get(b"DTWAIN_GetVersionCopyrightA")? };
        let DTWAIN_GetVersionCopyrightW: Symbol<DtwaingetversioncopyrightwFunc> = unsafe { library.get(b"DTWAIN_GetVersionCopyrightW")? };
        let DTWAIN_GetVersionEx: Symbol<DtwaingetversionexFunc> = unsafe { library.get(b"DTWAIN_GetVersionEx")? };
        let DTWAIN_GetVersionInfo: Symbol<DtwaingetversioninfoFunc> = unsafe { library.get(b"DTWAIN_GetVersionInfo")? };
        let DTWAIN_GetVersionInfoA: Symbol<DtwaingetversioninfoaFunc> = unsafe { library.get(b"DTWAIN_GetVersionInfoA")? };
        let DTWAIN_GetVersionInfoW: Symbol<DtwaingetversioninfowFunc> = unsafe { library.get(b"DTWAIN_GetVersionInfoW")? };
        let DTWAIN_GetVersionString: Symbol<DtwaingetversionstringFunc> = unsafe { library.get(b"DTWAIN_GetVersionString")? };
        let DTWAIN_GetVersionStringA: Symbol<DtwaingetversionstringaFunc> = unsafe { library.get(b"DTWAIN_GetVersionStringA")? };
        let DTWAIN_GetVersionStringW: Symbol<DtwaingetversionstringwFunc> = unsafe { library.get(b"DTWAIN_GetVersionStringW")? };
        let DTWAIN_GetWindowsVersionInfo: Symbol<DtwaingetwindowsversioninfoFunc> = unsafe { library.get(b"DTWAIN_GetWindowsVersionInfo")? };
        let DTWAIN_GetWindowsVersionInfoA: Symbol<DtwaingetwindowsversioninfoaFunc> = unsafe { library.get(b"DTWAIN_GetWindowsVersionInfoA")? };
        let DTWAIN_GetWindowsVersionInfoW: Symbol<DtwaingetwindowsversioninfowFunc> = unsafe { library.get(b"DTWAIN_GetWindowsVersionInfoW")? };
        let DTWAIN_GetXResolution: Symbol<DtwaingetxresolutionFunc> = unsafe { library.get(b"DTWAIN_GetXResolution")? };
        let DTWAIN_GetXResolutionString: Symbol<DtwaingetxresolutionstringFunc> = unsafe { library.get(b"DTWAIN_GetXResolutionString")? };
        let DTWAIN_GetXResolutionStringA: Symbol<DtwaingetxresolutionstringaFunc> = unsafe { library.get(b"DTWAIN_GetXResolutionStringA")? };
        let DTWAIN_GetXResolutionStringW: Symbol<DtwaingetxresolutionstringwFunc> = unsafe { library.get(b"DTWAIN_GetXResolutionStringW")? };
        let DTWAIN_GetYResolution: Symbol<DtwaingetyresolutionFunc> = unsafe { library.get(b"DTWAIN_GetYResolution")? };
        let DTWAIN_GetYResolutionString: Symbol<DtwaingetyresolutionstringFunc> = unsafe { library.get(b"DTWAIN_GetYResolutionString")? };
        let DTWAIN_GetYResolutionStringA: Symbol<DtwaingetyresolutionstringaFunc> = unsafe { library.get(b"DTWAIN_GetYResolutionStringA")? };
        let DTWAIN_GetYResolutionStringW: Symbol<DtwaingetyresolutionstringwFunc> = unsafe { library.get(b"DTWAIN_GetYResolutionStringW")? };
        let DTWAIN_InitExtImageInfo: Symbol<DtwaininitextimageinfoFunc> = unsafe { library.get(b"DTWAIN_InitExtImageInfo")? };
        let DTWAIN_InitImageFileAppend: Symbol<DtwaininitimagefileappendFunc> = unsafe { library.get(b"DTWAIN_InitImageFileAppend")? };
        let DTWAIN_InitImageFileAppendA: Symbol<DtwaininitimagefileappendaFunc> = unsafe { library.get(b"DTWAIN_InitImageFileAppendA")? };
        let DTWAIN_InitImageFileAppendW: Symbol<DtwaininitimagefileappendwFunc> = unsafe { library.get(b"DTWAIN_InitImageFileAppendW")? };
        let DTWAIN_InitOCRInterface: Symbol<DtwaininitocrinterfaceFunc> = unsafe { library.get(b"DTWAIN_InitOCRInterface")? };
        let DTWAIN_IsAcquiring: Symbol<DtwainisacquiringFunc> = unsafe { library.get(b"DTWAIN_IsAcquiring")? };
        let DTWAIN_IsAudioXferSupported: Symbol<DtwainisaudioxfersupportedFunc> = unsafe { library.get(b"DTWAIN_IsAudioXferSupported")? };
        let DTWAIN_IsAutoBorderDetectEnabled: Symbol<DtwainisautoborderdetectenabledFunc> = unsafe { library.get(b"DTWAIN_IsAutoBorderDetectEnabled")? };
        let DTWAIN_IsAutoBorderDetectSupported: Symbol<DtwainisautoborderdetectsupportedFunc> = unsafe { library.get(b"DTWAIN_IsAutoBorderDetectSupported")? };
        let DTWAIN_IsAutoBrightEnabled: Symbol<DtwainisautobrightenabledFunc> = unsafe { library.get(b"DTWAIN_IsAutoBrightEnabled")? };
        let DTWAIN_IsAutoBrightSupported: Symbol<DtwainisautobrightsupportedFunc> = unsafe { library.get(b"DTWAIN_IsAutoBrightSupported")? };
        let DTWAIN_IsAutoDeskewEnabled: Symbol<DtwainisautodeskewenabledFunc> = unsafe { library.get(b"DTWAIN_IsAutoDeskewEnabled")? };
        let DTWAIN_IsAutoDeskewSupported: Symbol<DtwainisautodeskewsupportedFunc> = unsafe { library.get(b"DTWAIN_IsAutoDeskewSupported")? };
        let DTWAIN_IsAutoFeedEnabled: Symbol<DtwainisautofeedenabledFunc> = unsafe { library.get(b"DTWAIN_IsAutoFeedEnabled")? };
        let DTWAIN_IsAutoFeedSupported: Symbol<DtwainisautofeedsupportedFunc> = unsafe { library.get(b"DTWAIN_IsAutoFeedSupported")? };
        let DTWAIN_IsAutoRotateEnabled: Symbol<DtwainisautorotateenabledFunc> = unsafe { library.get(b"DTWAIN_IsAutoRotateEnabled")? };
        let DTWAIN_IsAutoRotateSupported: Symbol<DtwainisautorotatesupportedFunc> = unsafe { library.get(b"DTWAIN_IsAutoRotateSupported")? };
        let DTWAIN_IsAutoScanEnabled: Symbol<DtwainisautoscanenabledFunc> = unsafe { library.get(b"DTWAIN_IsAutoScanEnabled")? };
        let DTWAIN_IsAutomaticSenseMediumEnabled: Symbol<DtwainisautomaticsensemediumenabledFunc> = unsafe { library.get(b"DTWAIN_IsAutomaticSenseMediumEnabled")? };
        let DTWAIN_IsAutomaticSenseMediumSupported: Symbol<DtwainisautomaticsensemediumsupportedFunc> = unsafe { library.get(b"DTWAIN_IsAutomaticSenseMediumSupported")? };
        let DTWAIN_IsBlankPageDetectionOn: Symbol<DtwainisblankpagedetectiononFunc> = unsafe { library.get(b"DTWAIN_IsBlankPageDetectionOn")? };
        let DTWAIN_IsBufferedTileModeOn: Symbol<DtwainisbufferedtilemodeonFunc> = unsafe { library.get(b"DTWAIN_IsBufferedTileModeOn")? };
        let DTWAIN_IsBufferedTileModeSupported: Symbol<DtwainisbufferedtilemodesupportedFunc> = unsafe { library.get(b"DTWAIN_IsBufferedTileModeSupported")? };
        let DTWAIN_IsCapSupported: Symbol<DtwainiscapsupportedFunc> = unsafe { library.get(b"DTWAIN_IsCapSupported")? };
        let DTWAIN_IsCompressionSupported: Symbol<DtwainiscompressionsupportedFunc> = unsafe { library.get(b"DTWAIN_IsCompressionSupported")? };
        let DTWAIN_IsCustomDSDataSupported: Symbol<DtwainiscustomdsdatasupportedFunc> = unsafe { library.get(b"DTWAIN_IsCustomDSDataSupported")? };
        let DTWAIN_IsDIBBlank: Symbol<DtwainisdibblankFunc> = unsafe { library.get(b"DTWAIN_IsDIBBlank")? };
        let DTWAIN_IsDIBBlankString: Symbol<DtwainisdibblankstringFunc> = unsafe { library.get(b"DTWAIN_IsDIBBlankString")? };
        let DTWAIN_IsDIBBlankStringA: Symbol<DtwainisdibblankstringaFunc> = unsafe { library.get(b"DTWAIN_IsDIBBlankStringA")? };
        let DTWAIN_IsDIBBlankStringW: Symbol<DtwainisdibblankstringwFunc> = unsafe { library.get(b"DTWAIN_IsDIBBlankStringW")? };
        let DTWAIN_IsDeviceEventSupported: Symbol<DtwainisdeviceeventsupportedFunc> = unsafe { library.get(b"DTWAIN_IsDeviceEventSupported")? };
        let DTWAIN_IsDeviceOnLine: Symbol<DtwainisdeviceonlineFunc> = unsafe { library.get(b"DTWAIN_IsDeviceOnLine")? };
        let DTWAIN_IsDoubleFeedDetectLengthSupported: Symbol<DtwainisdoublefeeddetectlengthsupportedFunc> = unsafe { library.get(b"DTWAIN_IsDoubleFeedDetectLengthSupported")? };
        let DTWAIN_IsDoubleFeedDetectSupported: Symbol<DtwainisdoublefeeddetectsupportedFunc> = unsafe { library.get(b"DTWAIN_IsDoubleFeedDetectSupported")? };
        let DTWAIN_IsDuplexEnabled: Symbol<DtwainisduplexenabledFunc> = unsafe { library.get(b"DTWAIN_IsDuplexEnabled")? };
        let DTWAIN_IsDuplexSupported: Symbol<DtwainisduplexsupportedFunc> = unsafe { library.get(b"DTWAIN_IsDuplexSupported")? };
        let DTWAIN_IsExtImageInfoSupported: Symbol<DtwainisextimageinfosupportedFunc> = unsafe { library.get(b"DTWAIN_IsExtImageInfoSupported")? };
        let DTWAIN_IsFeederEnabled: Symbol<DtwainisfeederenabledFunc> = unsafe { library.get(b"DTWAIN_IsFeederEnabled")? };
        let DTWAIN_IsFeederLoaded: Symbol<DtwainisfeederloadedFunc> = unsafe { library.get(b"DTWAIN_IsFeederLoaded")? };
        let DTWAIN_IsFeederSensitive: Symbol<DtwainisfeedersensitiveFunc> = unsafe { library.get(b"DTWAIN_IsFeederSensitive")? };
        let DTWAIN_IsFeederSupported: Symbol<DtwainisfeedersupportedFunc> = unsafe { library.get(b"DTWAIN_IsFeederSupported")? };
        let DTWAIN_IsFileSystemSupported: Symbol<DtwainisfilesystemsupportedFunc> = unsafe { library.get(b"DTWAIN_IsFileSystemSupported")? };
        let DTWAIN_IsFileXferSupported: Symbol<DtwainisfilexfersupportedFunc> = unsafe { library.get(b"DTWAIN_IsFileXferSupported")? };
        let DTWAIN_IsIAFieldALastPageSupported: Symbol<DtwainisiafieldalastpagesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldALastPageSupported")? };
        let DTWAIN_IsIAFieldALevelSupported: Symbol<DtwainisiafieldalevelsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldALevelSupported")? };
        let DTWAIN_IsIAFieldAPrintFormatSupported: Symbol<DtwainisiafieldaprintformatsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldAPrintFormatSupported")? };
        let DTWAIN_IsIAFieldAValueSupported: Symbol<DtwainisiafieldavaluesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldAValueSupported")? };
        let DTWAIN_IsIAFieldBLastPageSupported: Symbol<DtwainisiafieldblastpagesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldBLastPageSupported")? };
        let DTWAIN_IsIAFieldBLevelSupported: Symbol<DtwainisiafieldblevelsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldBLevelSupported")? };
        let DTWAIN_IsIAFieldBPrintFormatSupported: Symbol<DtwainisiafieldbprintformatsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldBPrintFormatSupported")? };
        let DTWAIN_IsIAFieldBValueSupported: Symbol<DtwainisiafieldbvaluesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldBValueSupported")? };
        let DTWAIN_IsIAFieldCLastPageSupported: Symbol<DtwainisiafieldclastpagesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldCLastPageSupported")? };
        let DTWAIN_IsIAFieldCLevelSupported: Symbol<DtwainisiafieldclevelsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldCLevelSupported")? };
        let DTWAIN_IsIAFieldCPrintFormatSupported: Symbol<DtwainisiafieldcprintformatsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldCPrintFormatSupported")? };
        let DTWAIN_IsIAFieldCValueSupported: Symbol<DtwainisiafieldcvaluesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldCValueSupported")? };
        let DTWAIN_IsIAFieldDLastPageSupported: Symbol<DtwainisiafielddlastpagesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldDLastPageSupported")? };
        let DTWAIN_IsIAFieldDLevelSupported: Symbol<DtwainisiafielddlevelsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldDLevelSupported")? };
        let DTWAIN_IsIAFieldDPrintFormatSupported: Symbol<DtwainisiafielddprintformatsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldDPrintFormatSupported")? };
        let DTWAIN_IsIAFieldDValueSupported: Symbol<DtwainisiafielddvaluesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldDValueSupported")? };
        let DTWAIN_IsIAFieldELastPageSupported: Symbol<DtwainisiafieldelastpagesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldELastPageSupported")? };
        let DTWAIN_IsIAFieldELevelSupported: Symbol<DtwainisiafieldelevelsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldELevelSupported")? };
        let DTWAIN_IsIAFieldEPrintFormatSupported: Symbol<DtwainisiafieldeprintformatsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldEPrintFormatSupported")? };
        let DTWAIN_IsIAFieldEValueSupported: Symbol<DtwainisiafieldevaluesupportedFunc> = unsafe { library.get(b"DTWAIN_IsIAFieldEValueSupported")? };
        let DTWAIN_IsImageAddressingSupported: Symbol<DtwainisimageaddressingsupportedFunc> = unsafe { library.get(b"DTWAIN_IsImageAddressingSupported")? };
        let DTWAIN_IsIndicatorEnabled: Symbol<DtwainisindicatorenabledFunc> = unsafe { library.get(b"DTWAIN_IsIndicatorEnabled")? };
        let DTWAIN_IsIndicatorSupported: Symbol<DtwainisindicatorsupportedFunc> = unsafe { library.get(b"DTWAIN_IsIndicatorSupported")? };
        let DTWAIN_IsInitialized: Symbol<DtwainisinitializedFunc> = unsafe { library.get(b"DTWAIN_IsInitialized")? };
        let DTWAIN_IsJPEGSupported: Symbol<DtwainisjpegsupportedFunc> = unsafe { library.get(b"DTWAIN_IsJPEGSupported")? };
        let DTWAIN_IsJobControlSupported: Symbol<DtwainisjobcontrolsupportedFunc> = unsafe { library.get(b"DTWAIN_IsJobControlSupported")? };
        let DTWAIN_IsLampEnabled: Symbol<DtwainislampenabledFunc> = unsafe { library.get(b"DTWAIN_IsLampEnabled")? };
        let DTWAIN_IsLampSupported: Symbol<DtwainislampsupportedFunc> = unsafe { library.get(b"DTWAIN_IsLampSupported")? };
        let DTWAIN_IsLightPathSupported: Symbol<DtwainislightpathsupportedFunc> = unsafe { library.get(b"DTWAIN_IsLightPathSupported")? };
        let DTWAIN_IsLightSourceSupported: Symbol<DtwainislightsourcesupportedFunc> = unsafe { library.get(b"DTWAIN_IsLightSourceSupported")? };
        let DTWAIN_IsMaxBuffersSupported: Symbol<DtwainismaxbufferssupportedFunc> = unsafe { library.get(b"DTWAIN_IsMaxBuffersSupported")? };
        let DTWAIN_IsMemFileXferSupported: Symbol<DtwainismemfilexfersupportedFunc> = unsafe { library.get(b"DTWAIN_IsMemFileXferSupported")? };
        let DTWAIN_IsMsgNotifyEnabled: Symbol<DtwainismsgnotifyenabledFunc> = unsafe { library.get(b"DTWAIN_IsMsgNotifyEnabled")? };
        let DTWAIN_IsNotifyTripletsEnabled: Symbol<DtwainisnotifytripletsenabledFunc> = unsafe { library.get(b"DTWAIN_IsNotifyTripletsEnabled")? };
        let DTWAIN_IsOCREngineActivated: Symbol<DtwainisocrengineactivatedFunc> = unsafe { library.get(b"DTWAIN_IsOCREngineActivated")? };
        let DTWAIN_IsOpenSourcesOnSelect: Symbol<DtwainisopensourcesonselectFunc> = unsafe { library.get(b"DTWAIN_IsOpenSourcesOnSelect")? };
        let DTWAIN_IsOrientationSupported: Symbol<DtwainisorientationsupportedFunc> = unsafe { library.get(b"DTWAIN_IsOrientationSupported")? };
        let DTWAIN_IsOverscanSupported: Symbol<DtwainisoverscansupportedFunc> = unsafe { library.get(b"DTWAIN_IsOverscanSupported")? };
        let DTWAIN_IsPDFSupported: Symbol<DtwainispdfsupportedFunc> = unsafe { library.get(b"DTWAIN_IsPDFSupported")? };
        let DTWAIN_IsPNGSupported: Symbol<DtwainispngsupportedFunc> = unsafe { library.get(b"DTWAIN_IsPNGSupported")? };
        let DTWAIN_IsPaperDetectable: Symbol<DtwainispaperdetectableFunc> = unsafe { library.get(b"DTWAIN_IsPaperDetectable")? };
        let DTWAIN_IsPaperSizeSupported: Symbol<DtwainispapersizesupportedFunc> = unsafe { library.get(b"DTWAIN_IsPaperSizeSupported")? };
        let DTWAIN_IsPatchCapsSupported: Symbol<DtwainispatchcapssupportedFunc> = unsafe { library.get(b"DTWAIN_IsPatchCapsSupported")? };
        let DTWAIN_IsPatchDetectEnabled: Symbol<DtwainispatchdetectenabledFunc> = unsafe { library.get(b"DTWAIN_IsPatchDetectEnabled")? };
        let DTWAIN_IsPatchSupported: Symbol<DtwainispatchsupportedFunc> = unsafe { library.get(b"DTWAIN_IsPatchSupported")? };
        let DTWAIN_IsPeekMessageLoopEnabled: Symbol<DtwainispeekmessageloopenabledFunc> = unsafe { library.get(b"DTWAIN_IsPeekMessageLoopEnabled")? };
        let DTWAIN_IsPixelTypeSupported: Symbol<DtwainispixeltypesupportedFunc> = unsafe { library.get(b"DTWAIN_IsPixelTypeSupported")? };
        let DTWAIN_IsPrinterEnabled: Symbol<DtwainisprinterenabledFunc> = unsafe { library.get(b"DTWAIN_IsPrinterEnabled")? };
        let DTWAIN_IsPrinterSupported: Symbol<DtwainisprintersupportedFunc> = unsafe { library.get(b"DTWAIN_IsPrinterSupported")? };
        let DTWAIN_IsRotationSupported: Symbol<DtwainisrotationsupportedFunc> = unsafe { library.get(b"DTWAIN_IsRotationSupported")? };
        let DTWAIN_IsSessionEnabled: Symbol<DtwainissessionenabledFunc> = unsafe { library.get(b"DTWAIN_IsSessionEnabled")? };
        let DTWAIN_IsSkipImageInfoError: Symbol<DtwainisskipimageinfoerrorFunc> = unsafe { library.get(b"DTWAIN_IsSkipImageInfoError")? };
        let DTWAIN_IsSourceAcquiring: Symbol<DtwainissourceacquiringFunc> = unsafe { library.get(b"DTWAIN_IsSourceAcquiring")? };
        let DTWAIN_IsSourceAcquiringEx: Symbol<DtwainissourceacquiringexFunc> = unsafe { library.get(b"DTWAIN_IsSourceAcquiringEx")? };
        let DTWAIN_IsSourceInUIOnlyMode: Symbol<DtwainissourceinuionlymodeFunc> = unsafe { library.get(b"DTWAIN_IsSourceInUIOnlyMode")? };
        let DTWAIN_IsSourceOpen: Symbol<DtwainissourceopenFunc> = unsafe { library.get(b"DTWAIN_IsSourceOpen")? };
        let DTWAIN_IsSourceSelected: Symbol<DtwainissourceselectedFunc> = unsafe { library.get(b"DTWAIN_IsSourceSelected")? };
        let DTWAIN_IsSourceValid: Symbol<DtwainissourcevalidFunc> = unsafe { library.get(b"DTWAIN_IsSourceValid")? };
        let DTWAIN_IsTIFFSupported: Symbol<DtwainistiffsupportedFunc> = unsafe { library.get(b"DTWAIN_IsTIFFSupported")? };
        let DTWAIN_IsThumbnailEnabled: Symbol<DtwainisthumbnailenabledFunc> = unsafe { library.get(b"DTWAIN_IsThumbnailEnabled")? };
        let DTWAIN_IsThumbnailSupported: Symbol<DtwainisthumbnailsupportedFunc> = unsafe { library.get(b"DTWAIN_IsThumbnailSupported")? };
        let DTWAIN_IsTwainAvailable: Symbol<DtwainistwainavailableFunc> = unsafe { library.get(b"DTWAIN_IsTwainAvailable")? };
        let DTWAIN_IsTwainAvailableEx: Symbol<DtwainistwainavailableexFunc> = unsafe { library.get(b"DTWAIN_IsTwainAvailableEx")? };
        let DTWAIN_IsTwainAvailableExA: Symbol<DtwainistwainavailableexaFunc> = unsafe { library.get(b"DTWAIN_IsTwainAvailableExA")? };
        let DTWAIN_IsTwainAvailableExW: Symbol<DtwainistwainavailableexwFunc> = unsafe { library.get(b"DTWAIN_IsTwainAvailableExW")? };
        let DTWAIN_IsUIControllable: Symbol<DtwainisuicontrollableFunc> = unsafe { library.get(b"DTWAIN_IsUIControllable")? };
        let DTWAIN_IsUIEnabled: Symbol<DtwainisuienabledFunc> = unsafe { library.get(b"DTWAIN_IsUIEnabled")? };
        let DTWAIN_IsUIOnlySupported: Symbol<DtwainisuionlysupportedFunc> = unsafe { library.get(b"DTWAIN_IsUIOnlySupported")? };
        let DTWAIN_LoadCustomStringResources: Symbol<DtwainloadcustomstringresourcesFunc> = unsafe { library.get(b"DTWAIN_LoadCustomStringResources")? };
        let DTWAIN_LoadCustomStringResourcesA: Symbol<DtwainloadcustomstringresourcesaFunc> = unsafe { library.get(b"DTWAIN_LoadCustomStringResourcesA")? };
        let DTWAIN_LoadCustomStringResourcesEx: Symbol<DtwainloadcustomstringresourcesexFunc> = unsafe { library.get(b"DTWAIN_LoadCustomStringResourcesEx")? };
        let DTWAIN_LoadCustomStringResourcesExA: Symbol<DtwainloadcustomstringresourcesexaFunc> = unsafe { library.get(b"DTWAIN_LoadCustomStringResourcesExA")? };
        let DTWAIN_LoadCustomStringResourcesExW: Symbol<DtwainloadcustomstringresourcesexwFunc> = unsafe { library.get(b"DTWAIN_LoadCustomStringResourcesExW")? };
        let DTWAIN_LoadCustomStringResourcesW: Symbol<DtwainloadcustomstringresourceswFunc> = unsafe { library.get(b"DTWAIN_LoadCustomStringResourcesW")? };
        let DTWAIN_LoadLanguageResource: Symbol<DtwainloadlanguageresourceFunc> = unsafe { library.get(b"DTWAIN_LoadLanguageResource")? };
        let DTWAIN_LockMemory: Symbol<DtwainlockmemoryFunc> = unsafe { library.get(b"DTWAIN_LockMemory")? };
        let DTWAIN_LockMemoryEx: Symbol<DtwainlockmemoryexFunc> = unsafe { library.get(b"DTWAIN_LockMemoryEx")? };
        let DTWAIN_LogMessage: Symbol<DtwainlogmessageFunc> = unsafe { library.get(b"DTWAIN_LogMessage")? };
        let DTWAIN_LogMessageA: Symbol<DtwainlogmessageaFunc> = unsafe { library.get(b"DTWAIN_LogMessageA")? };
        let DTWAIN_LogMessageW: Symbol<DtwainlogmessagewFunc> = unsafe { library.get(b"DTWAIN_LogMessageW")? };
        let DTWAIN_MakeRGB: Symbol<DtwainmakergbFunc> = unsafe { library.get(b"DTWAIN_MakeRGB")? };
        let DTWAIN_OpenSource: Symbol<DtwainopensourceFunc> = unsafe { library.get(b"DTWAIN_OpenSource")? };
        let DTWAIN_OpenSourcesOnSelect: Symbol<DtwainopensourcesonselectFunc> = unsafe { library.get(b"DTWAIN_OpenSourcesOnSelect")? };
        let DTWAIN_RangeCreate: Symbol<DtwainrangecreateFunc> = unsafe { library.get(b"DTWAIN_RangeCreate")? };
        let DTWAIN_RangeCreateFromCap: Symbol<DtwainrangecreatefromcapFunc> = unsafe { library.get(b"DTWAIN_RangeCreateFromCap")? };
        let DTWAIN_RangeDestroy: Symbol<DtwainrangedestroyFunc> = unsafe { library.get(b"DTWAIN_RangeDestroy")? };
        let DTWAIN_RangeExpand: Symbol<DtwainrangeexpandFunc> = unsafe { library.get(b"DTWAIN_RangeExpand")? };
        let DTWAIN_RangeExpandEx: Symbol<DtwainrangeexpandexFunc> = unsafe { library.get(b"DTWAIN_RangeExpandEx")? };
        let DTWAIN_RangeGetAll: Symbol<DtwainrangegetallFunc> = unsafe { library.get(b"DTWAIN_RangeGetAll")? };
        let DTWAIN_RangeGetAllFloat: Symbol<DtwainrangegetallfloatFunc> = unsafe { library.get(b"DTWAIN_RangeGetAllFloat")? };
        let DTWAIN_RangeGetAllFloatString: Symbol<DtwainrangegetallfloatstringFunc> = unsafe { library.get(b"DTWAIN_RangeGetAllFloatString")? };
        let DTWAIN_RangeGetAllFloatStringA: Symbol<DtwainrangegetallfloatstringaFunc> = unsafe { library.get(b"DTWAIN_RangeGetAllFloatStringA")? };
        let DTWAIN_RangeGetAllFloatStringW: Symbol<DtwainrangegetallfloatstringwFunc> = unsafe { library.get(b"DTWAIN_RangeGetAllFloatStringW")? };
        let DTWAIN_RangeGetAllLong: Symbol<DtwainrangegetalllongFunc> = unsafe { library.get(b"DTWAIN_RangeGetAllLong")? };
        let DTWAIN_RangeGetCount: Symbol<DtwainrangegetcountFunc> = unsafe { library.get(b"DTWAIN_RangeGetCount")? };
        let DTWAIN_RangeGetExpValue: Symbol<DtwainrangegetexpvalueFunc> = unsafe { library.get(b"DTWAIN_RangeGetExpValue")? };
        let DTWAIN_RangeGetExpValueFloat: Symbol<DtwainrangegetexpvaluefloatFunc> = unsafe { library.get(b"DTWAIN_RangeGetExpValueFloat")? };
        let DTWAIN_RangeGetExpValueFloatString: Symbol<DtwainrangegetexpvaluefloatstringFunc> = unsafe { library.get(b"DTWAIN_RangeGetExpValueFloatString")? };
        let DTWAIN_RangeGetExpValueFloatStringA: Symbol<DtwainrangegetexpvaluefloatstringaFunc> = unsafe { library.get(b"DTWAIN_RangeGetExpValueFloatStringA")? };
        let DTWAIN_RangeGetExpValueFloatStringW: Symbol<DtwainrangegetexpvaluefloatstringwFunc> = unsafe { library.get(b"DTWAIN_RangeGetExpValueFloatStringW")? };
        let DTWAIN_RangeGetExpValueLong: Symbol<DtwainrangegetexpvaluelongFunc> = unsafe { library.get(b"DTWAIN_RangeGetExpValueLong")? };
        let DTWAIN_RangeGetNearestValue: Symbol<DtwainrangegetnearestvalueFunc> = unsafe { library.get(b"DTWAIN_RangeGetNearestValue")? };
        let DTWAIN_RangeGetPos: Symbol<DtwainrangegetposFunc> = unsafe { library.get(b"DTWAIN_RangeGetPos")? };
        let DTWAIN_RangeGetPosFloat: Symbol<DtwainrangegetposfloatFunc> = unsafe { library.get(b"DTWAIN_RangeGetPosFloat")? };
        let DTWAIN_RangeGetPosFloatString: Symbol<DtwainrangegetposfloatstringFunc> = unsafe { library.get(b"DTWAIN_RangeGetPosFloatString")? };
        let DTWAIN_RangeGetPosFloatStringA: Symbol<DtwainrangegetposfloatstringaFunc> = unsafe { library.get(b"DTWAIN_RangeGetPosFloatStringA")? };
        let DTWAIN_RangeGetPosFloatStringW: Symbol<DtwainrangegetposfloatstringwFunc> = unsafe { library.get(b"DTWAIN_RangeGetPosFloatStringW")? };
        let DTWAIN_RangeGetPosLong: Symbol<DtwainrangegetposlongFunc> = unsafe { library.get(b"DTWAIN_RangeGetPosLong")? };
        let DTWAIN_RangeGetValue: Symbol<DtwainrangegetvalueFunc> = unsafe { library.get(b"DTWAIN_RangeGetValue")? };
        let DTWAIN_RangeGetValueFloat: Symbol<DtwainrangegetvaluefloatFunc> = unsafe { library.get(b"DTWAIN_RangeGetValueFloat")? };
        let DTWAIN_RangeGetValueFloatString: Symbol<DtwainrangegetvaluefloatstringFunc> = unsafe { library.get(b"DTWAIN_RangeGetValueFloatString")? };
        let DTWAIN_RangeGetValueFloatStringA: Symbol<DtwainrangegetvaluefloatstringaFunc> = unsafe { library.get(b"DTWAIN_RangeGetValueFloatStringA")? };
        let DTWAIN_RangeGetValueFloatStringW: Symbol<DtwainrangegetvaluefloatstringwFunc> = unsafe { library.get(b"DTWAIN_RangeGetValueFloatStringW")? };
        let DTWAIN_RangeGetValueLong: Symbol<DtwainrangegetvaluelongFunc> = unsafe { library.get(b"DTWAIN_RangeGetValueLong")? };
        let DTWAIN_RangeIsValid: Symbol<DtwainrangeisvalidFunc> = unsafe { library.get(b"DTWAIN_RangeIsValid")? };
        let DTWAIN_RangeNearestValueFloat: Symbol<DtwainrangenearestvaluefloatFunc> = unsafe { library.get(b"DTWAIN_RangeNearestValueFloat")? };
        let DTWAIN_RangeNearestValueFloatString: Symbol<DtwainrangenearestvaluefloatstringFunc> = unsafe { library.get(b"DTWAIN_RangeNearestValueFloatString")? };
        let DTWAIN_RangeNearestValueFloatStringA: Symbol<DtwainrangenearestvaluefloatstringaFunc> = unsafe { library.get(b"DTWAIN_RangeNearestValueFloatStringA")? };
        let DTWAIN_RangeNearestValueFloatStringW: Symbol<DtwainrangenearestvaluefloatstringwFunc> = unsafe { library.get(b"DTWAIN_RangeNearestValueFloatStringW")? };
        let DTWAIN_RangeNearestValueLong: Symbol<DtwainrangenearestvaluelongFunc> = unsafe { library.get(b"DTWAIN_RangeNearestValueLong")? };
        let DTWAIN_RangeSetAll: Symbol<DtwainrangesetallFunc> = unsafe { library.get(b"DTWAIN_RangeSetAll")? };
        let DTWAIN_RangeSetAllFloat: Symbol<DtwainrangesetallfloatFunc> = unsafe { library.get(b"DTWAIN_RangeSetAllFloat")? };
        let DTWAIN_RangeSetAllFloatString: Symbol<DtwainrangesetallfloatstringFunc> = unsafe { library.get(b"DTWAIN_RangeSetAllFloatString")? };
        let DTWAIN_RangeSetAllFloatStringA: Symbol<DtwainrangesetallfloatstringaFunc> = unsafe { library.get(b"DTWAIN_RangeSetAllFloatStringA")? };
        let DTWAIN_RangeSetAllFloatStringW: Symbol<DtwainrangesetallfloatstringwFunc> = unsafe { library.get(b"DTWAIN_RangeSetAllFloatStringW")? };
        let DTWAIN_RangeSetAllLong: Symbol<DtwainrangesetalllongFunc> = unsafe { library.get(b"DTWAIN_RangeSetAllLong")? };
        let DTWAIN_RangeSetValue: Symbol<DtwainrangesetvalueFunc> = unsafe { library.get(b"DTWAIN_RangeSetValue")? };
        let DTWAIN_RangeSetValueFloat: Symbol<DtwainrangesetvaluefloatFunc> = unsafe { library.get(b"DTWAIN_RangeSetValueFloat")? };
        let DTWAIN_RangeSetValueFloatString: Symbol<DtwainrangesetvaluefloatstringFunc> = unsafe { library.get(b"DTWAIN_RangeSetValueFloatString")? };
        let DTWAIN_RangeSetValueFloatStringA: Symbol<DtwainrangesetvaluefloatstringaFunc> = unsafe { library.get(b"DTWAIN_RangeSetValueFloatStringA")? };
        let DTWAIN_RangeSetValueFloatStringW: Symbol<DtwainrangesetvaluefloatstringwFunc> = unsafe { library.get(b"DTWAIN_RangeSetValueFloatStringW")? };
        let DTWAIN_RangeSetValueLong: Symbol<DtwainrangesetvaluelongFunc> = unsafe { library.get(b"DTWAIN_RangeSetValueLong")? };
        let DTWAIN_ResetPDFTextElement: Symbol<DtwainresetpdftextelementFunc> = unsafe { library.get(b"DTWAIN_ResetPDFTextElement")? };
        let DTWAIN_RewindPage: Symbol<DtwainrewindpageFunc> = unsafe { library.get(b"DTWAIN_RewindPage")? };
        let DTWAIN_SelectDefaultOCREngine: Symbol<DtwainselectdefaultocrengineFunc> = unsafe { library.get(b"DTWAIN_SelectDefaultOCREngine")? };
        let DTWAIN_SelectDefaultSource: Symbol<DtwainselectdefaultsourceFunc> = unsafe { library.get(b"DTWAIN_SelectDefaultSource")? };
        let DTWAIN_SelectDefaultSourceWithOpen: Symbol<DtwainselectdefaultsourcewithopenFunc> = unsafe { library.get(b"DTWAIN_SelectDefaultSourceWithOpen")? };
        let DTWAIN_SelectOCREngine: Symbol<DtwainselectocrengineFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngine")? };
        let DTWAIN_SelectOCREngine2: Symbol<Dtwainselectocrengine2Func> = unsafe { library.get(b"DTWAIN_SelectOCREngine2")? };
        let DTWAIN_SelectOCREngine2A: Symbol<Dtwainselectocrengine2aFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngine2A")? };
        let DTWAIN_SelectOCREngine2Ex: Symbol<Dtwainselectocrengine2exFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngine2Ex")? };
        let DTWAIN_SelectOCREngine2ExA: Symbol<Dtwainselectocrengine2exaFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngine2ExA")? };
        let DTWAIN_SelectOCREngine2ExW: Symbol<Dtwainselectocrengine2exwFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngine2ExW")? };
        let DTWAIN_SelectOCREngine2W: Symbol<Dtwainselectocrengine2wFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngine2W")? };
        let DTWAIN_SelectOCREngineByName: Symbol<DtwainselectocrenginebynameFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngineByName")? };
        let DTWAIN_SelectOCREngineByNameA: Symbol<DtwainselectocrenginebynameaFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngineByNameA")? };
        let DTWAIN_SelectOCREngineByNameW: Symbol<DtwainselectocrenginebynamewFunc> = unsafe { library.get(b"DTWAIN_SelectOCREngineByNameW")? };
        let DTWAIN_SelectSource: Symbol<DtwainselectsourceFunc> = unsafe { library.get(b"DTWAIN_SelectSource")? };
        let DTWAIN_SelectSource2: Symbol<Dtwainselectsource2Func> = unsafe { library.get(b"DTWAIN_SelectSource2")? };
        let DTWAIN_SelectSource2A: Symbol<Dtwainselectsource2aFunc> = unsafe { library.get(b"DTWAIN_SelectSource2A")? };
        let DTWAIN_SelectSource2Ex: Symbol<Dtwainselectsource2exFunc> = unsafe { library.get(b"DTWAIN_SelectSource2Ex")? };
        let DTWAIN_SelectSource2ExA: Symbol<Dtwainselectsource2exaFunc> = unsafe { library.get(b"DTWAIN_SelectSource2ExA")? };
        let DTWAIN_SelectSource2ExW: Symbol<Dtwainselectsource2exwFunc> = unsafe { library.get(b"DTWAIN_SelectSource2ExW")? };
        let DTWAIN_SelectSource2W: Symbol<Dtwainselectsource2wFunc> = unsafe { library.get(b"DTWAIN_SelectSource2W")? };
        let DTWAIN_SelectSourceByName: Symbol<DtwainselectsourcebynameFunc> = unsafe { library.get(b"DTWAIN_SelectSourceByName")? };
        let DTWAIN_SelectSourceByNameA: Symbol<DtwainselectsourcebynameaFunc> = unsafe { library.get(b"DTWAIN_SelectSourceByNameA")? };
        let DTWAIN_SelectSourceByNameW: Symbol<DtwainselectsourcebynamewFunc> = unsafe { library.get(b"DTWAIN_SelectSourceByNameW")? };
        let DTWAIN_SelectSourceByNameWithOpen: Symbol<DtwainselectsourcebynamewithopenFunc> = unsafe { library.get(b"DTWAIN_SelectSourceByNameWithOpen")? };
        let DTWAIN_SelectSourceByNameWithOpenA: Symbol<DtwainselectsourcebynamewithopenaFunc> = unsafe { library.get(b"DTWAIN_SelectSourceByNameWithOpenA")? };
        let DTWAIN_SelectSourceByNameWithOpenW: Symbol<DtwainselectsourcebynamewithopenwFunc> = unsafe { library.get(b"DTWAIN_SelectSourceByNameWithOpenW")? };
        let DTWAIN_SelectSourceWithOpen: Symbol<DtwainselectsourcewithopenFunc> = unsafe { library.get(b"DTWAIN_SelectSourceWithOpen")? };
        let DTWAIN_SetAcquireArea: Symbol<DtwainsetacquireareaFunc> = unsafe { library.get(b"DTWAIN_SetAcquireArea")? };
        let DTWAIN_SetAcquireArea2: Symbol<Dtwainsetacquirearea2Func> = unsafe { library.get(b"DTWAIN_SetAcquireArea2")? };
        let DTWAIN_SetAcquireArea2String: Symbol<Dtwainsetacquirearea2stringFunc> = unsafe { library.get(b"DTWAIN_SetAcquireArea2String")? };
        let DTWAIN_SetAcquireArea2StringA: Symbol<Dtwainsetacquirearea2stringaFunc> = unsafe { library.get(b"DTWAIN_SetAcquireArea2StringA")? };
        let DTWAIN_SetAcquireArea2StringW: Symbol<Dtwainsetacquirearea2stringwFunc> = unsafe { library.get(b"DTWAIN_SetAcquireArea2StringW")? };
        let DTWAIN_SetAcquireImageNegative: Symbol<DtwainsetacquireimagenegativeFunc> = unsafe { library.get(b"DTWAIN_SetAcquireImageNegative")? };
        let DTWAIN_SetAcquireImageScale: Symbol<DtwainsetacquireimagescaleFunc> = unsafe { library.get(b"DTWAIN_SetAcquireImageScale")? };
        let DTWAIN_SetAcquireImageScaleString: Symbol<DtwainsetacquireimagescalestringFunc> = unsafe { library.get(b"DTWAIN_SetAcquireImageScaleString")? };
        let DTWAIN_SetAcquireImageScaleStringA: Symbol<DtwainsetacquireimagescalestringaFunc> = unsafe { library.get(b"DTWAIN_SetAcquireImageScaleStringA")? };
        let DTWAIN_SetAcquireImageScaleStringW: Symbol<DtwainsetacquireimagescalestringwFunc> = unsafe { library.get(b"DTWAIN_SetAcquireImageScaleStringW")? };
        let DTWAIN_SetAcquireStripBuffer: Symbol<DtwainsetacquirestripbufferFunc> = unsafe { library.get(b"DTWAIN_SetAcquireStripBuffer")? };
        let DTWAIN_SetAcquireStripSize: Symbol<DtwainsetacquirestripsizeFunc> = unsafe { library.get(b"DTWAIN_SetAcquireStripSize")? };
        let DTWAIN_SetAlarmVolume: Symbol<DtwainsetalarmvolumeFunc> = unsafe { library.get(b"DTWAIN_SetAlarmVolume")? };
        let DTWAIN_SetAlarms: Symbol<DtwainsetalarmsFunc> = unsafe { library.get(b"DTWAIN_SetAlarms")? };
        let DTWAIN_SetAllCapsToDefault: Symbol<DtwainsetallcapstodefaultFunc> = unsafe { library.get(b"DTWAIN_SetAllCapsToDefault")? };
        let DTWAIN_SetAppInfo: Symbol<DtwainsetappinfoFunc> = unsafe { library.get(b"DTWAIN_SetAppInfo")? };
        let DTWAIN_SetAppInfoA: Symbol<DtwainsetappinfoaFunc> = unsafe { library.get(b"DTWAIN_SetAppInfoA")? };
        let DTWAIN_SetAppInfoW: Symbol<DtwainsetappinfowFunc> = unsafe { library.get(b"DTWAIN_SetAppInfoW")? };
        let DTWAIN_SetAuthor: Symbol<DtwainsetauthorFunc> = unsafe { library.get(b"DTWAIN_SetAuthor")? };
        let DTWAIN_SetAuthorA: Symbol<DtwainsetauthoraFunc> = unsafe { library.get(b"DTWAIN_SetAuthorA")? };
        let DTWAIN_SetAuthorW: Symbol<DtwainsetauthorwFunc> = unsafe { library.get(b"DTWAIN_SetAuthorW")? };
        let DTWAIN_SetAvailablePrinters: Symbol<DtwainsetavailableprintersFunc> = unsafe { library.get(b"DTWAIN_SetAvailablePrinters")? };
        let DTWAIN_SetAvailablePrintersArray: Symbol<DtwainsetavailableprintersarrayFunc> = unsafe { library.get(b"DTWAIN_SetAvailablePrintersArray")? };
        let DTWAIN_SetBitDepth: Symbol<DtwainsetbitdepthFunc> = unsafe { library.get(b"DTWAIN_SetBitDepth")? };
        let DTWAIN_SetBlankPageDetection: Symbol<DtwainsetblankpagedetectionFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetection")? };
        let DTWAIN_SetBlankPageDetectionEx: Symbol<DtwainsetblankpagedetectionexFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetectionEx")? };
        let DTWAIN_SetBlankPageDetectionExString: Symbol<DtwainsetblankpagedetectionexstringFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetectionExString")? };
        let DTWAIN_SetBlankPageDetectionExStringA: Symbol<DtwainsetblankpagedetectionexstringaFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetectionExStringA")? };
        let DTWAIN_SetBlankPageDetectionExStringW: Symbol<DtwainsetblankpagedetectionexstringwFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetectionExStringW")? };
        let DTWAIN_SetBlankPageDetectionString: Symbol<DtwainsetblankpagedetectionstringFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetectionString")? };
        let DTWAIN_SetBlankPageDetectionStringA: Symbol<DtwainsetblankpagedetectionstringaFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetectionStringA")? };
        let DTWAIN_SetBlankPageDetectionStringW: Symbol<DtwainsetblankpagedetectionstringwFunc> = unsafe { library.get(b"DTWAIN_SetBlankPageDetectionStringW")? };
        let DTWAIN_SetBrightness: Symbol<DtwainsetbrightnessFunc> = unsafe { library.get(b"DTWAIN_SetBrightness")? };
        let DTWAIN_SetBrightnessString: Symbol<DtwainsetbrightnessstringFunc> = unsafe { library.get(b"DTWAIN_SetBrightnessString")? };
        let DTWAIN_SetBrightnessStringA: Symbol<DtwainsetbrightnessstringaFunc> = unsafe { library.get(b"DTWAIN_SetBrightnessStringA")? };
        let DTWAIN_SetBrightnessStringW: Symbol<DtwainsetbrightnessstringwFunc> = unsafe { library.get(b"DTWAIN_SetBrightnessStringW")? };
        let DTWAIN_SetBufferedTileMode: Symbol<DtwainsetbufferedtilemodeFunc> = unsafe { library.get(b"DTWAIN_SetBufferedTileMode")? };
        let DTWAIN_SetCallback: Symbol<DtwainsetcallbackFunc> = unsafe { library.get(b"DTWAIN_SetCallback")? };
        let DTWAIN_SetCallback64: Symbol<Dtwainsetcallback64Func> = unsafe { library.get(b"DTWAIN_SetCallback64")? };
        let DTWAIN_SetCamera: Symbol<DtwainsetcameraFunc> = unsafe { library.get(b"DTWAIN_SetCamera")? };
        let DTWAIN_SetCameraA: Symbol<DtwainsetcameraaFunc> = unsafe { library.get(b"DTWAIN_SetCameraA")? };
        let DTWAIN_SetCameraW: Symbol<DtwainsetcamerawFunc> = unsafe { library.get(b"DTWAIN_SetCameraW")? };
        let DTWAIN_SetCapValues: Symbol<DtwainsetcapvaluesFunc> = unsafe { library.get(b"DTWAIN_SetCapValues")? };
        let DTWAIN_SetCapValuesEx: Symbol<DtwainsetcapvaluesexFunc> = unsafe { library.get(b"DTWAIN_SetCapValuesEx")? };
        let DTWAIN_SetCapValuesEx2: Symbol<Dtwainsetcapvaluesex2Func> = unsafe { library.get(b"DTWAIN_SetCapValuesEx2")? };
        let DTWAIN_SetCaption: Symbol<DtwainsetcaptionFunc> = unsafe { library.get(b"DTWAIN_SetCaption")? };
        let DTWAIN_SetCaptionA: Symbol<DtwainsetcaptionaFunc> = unsafe { library.get(b"DTWAIN_SetCaptionA")? };
        let DTWAIN_SetCaptionW: Symbol<DtwainsetcaptionwFunc> = unsafe { library.get(b"DTWAIN_SetCaptionW")? };
        let DTWAIN_SetCompressionType: Symbol<DtwainsetcompressiontypeFunc> = unsafe { library.get(b"DTWAIN_SetCompressionType")? };
        let DTWAIN_SetContrast: Symbol<DtwainsetcontrastFunc> = unsafe { library.get(b"DTWAIN_SetContrast")? };
        let DTWAIN_SetContrastString: Symbol<DtwainsetcontraststringFunc> = unsafe { library.get(b"DTWAIN_SetContrastString")? };
        let DTWAIN_SetContrastStringA: Symbol<DtwainsetcontraststringaFunc> = unsafe { library.get(b"DTWAIN_SetContrastStringA")? };
        let DTWAIN_SetContrastStringW: Symbol<DtwainsetcontraststringwFunc> = unsafe { library.get(b"DTWAIN_SetContrastStringW")? };
        let DTWAIN_SetCountry: Symbol<DtwainsetcountryFunc> = unsafe { library.get(b"DTWAIN_SetCountry")? };
        let DTWAIN_SetCurrentRetryCount: Symbol<DtwainsetcurrentretrycountFunc> = unsafe { library.get(b"DTWAIN_SetCurrentRetryCount")? };
        let DTWAIN_SetCustomDSData: Symbol<DtwainsetcustomdsdataFunc> = unsafe { library.get(b"DTWAIN_SetCustomDSData")? };
        let DTWAIN_SetDSMSearchOrder: Symbol<DtwainsetdsmsearchorderFunc> = unsafe { library.get(b"DTWAIN_SetDSMSearchOrder")? };
        let DTWAIN_SetDSMSearchOrderEx: Symbol<DtwainsetdsmsearchorderexFunc> = unsafe { library.get(b"DTWAIN_SetDSMSearchOrderEx")? };
        let DTWAIN_SetDSMSearchOrderExA: Symbol<DtwainsetdsmsearchorderexaFunc> = unsafe { library.get(b"DTWAIN_SetDSMSearchOrderExA")? };
        let DTWAIN_SetDSMSearchOrderExW: Symbol<DtwainsetdsmsearchorderexwFunc> = unsafe { library.get(b"DTWAIN_SetDSMSearchOrderExW")? };
        let DTWAIN_SetDefaultSource: Symbol<DtwainsetdefaultsourceFunc> = unsafe { library.get(b"DTWAIN_SetDefaultSource")? };
        let DTWAIN_SetDeviceNotifications: Symbol<DtwainsetdevicenotificationsFunc> = unsafe { library.get(b"DTWAIN_SetDeviceNotifications")? };
        let DTWAIN_SetDeviceTimeDate: Symbol<DtwainsetdevicetimedateFunc> = unsafe { library.get(b"DTWAIN_SetDeviceTimeDate")? };
        let DTWAIN_SetDeviceTimeDateA: Symbol<DtwainsetdevicetimedateaFunc> = unsafe { library.get(b"DTWAIN_SetDeviceTimeDateA")? };
        let DTWAIN_SetDeviceTimeDateW: Symbol<DtwainsetdevicetimedatewFunc> = unsafe { library.get(b"DTWAIN_SetDeviceTimeDateW")? };
        let DTWAIN_SetDoubleFeedDetectLength: Symbol<DtwainsetdoublefeeddetectlengthFunc> = unsafe { library.get(b"DTWAIN_SetDoubleFeedDetectLength")? };
        let DTWAIN_SetDoubleFeedDetectLengthString: Symbol<DtwainsetdoublefeeddetectlengthstringFunc> = unsafe { library.get(b"DTWAIN_SetDoubleFeedDetectLengthString")? };
        let DTWAIN_SetDoubleFeedDetectLengthStringA: Symbol<DtwainsetdoublefeeddetectlengthstringaFunc> = unsafe { library.get(b"DTWAIN_SetDoubleFeedDetectLengthStringA")? };
        let DTWAIN_SetDoubleFeedDetectLengthStringW: Symbol<DtwainsetdoublefeeddetectlengthstringwFunc> = unsafe { library.get(b"DTWAIN_SetDoubleFeedDetectLengthStringW")? };
        let DTWAIN_SetDoubleFeedDetectValues: Symbol<DtwainsetdoublefeeddetectvaluesFunc> = unsafe { library.get(b"DTWAIN_SetDoubleFeedDetectValues")? };
        let DTWAIN_SetDoublePageCountOnDuplex: Symbol<DtwainsetdoublepagecountonduplexFunc> = unsafe { library.get(b"DTWAIN_SetDoublePageCountOnDuplex")? };
        let DTWAIN_SetEOJDetectValue: Symbol<DtwainseteojdetectvalueFunc> = unsafe { library.get(b"DTWAIN_SetEOJDetectValue")? };
        let DTWAIN_SetErrorBufferThreshold: Symbol<DtwainseterrorbufferthresholdFunc> = unsafe { library.get(b"DTWAIN_SetErrorBufferThreshold")? };
        let DTWAIN_SetErrorCallback: Symbol<DtwainseterrorcallbackFunc> = unsafe { library.get(b"DTWAIN_SetErrorCallback")? };
        let DTWAIN_SetErrorCallback64: Symbol<Dtwainseterrorcallback64Func> = unsafe { library.get(b"DTWAIN_SetErrorCallback64")? };
        let DTWAIN_SetFeederAlignment: Symbol<DtwainsetfeederalignmentFunc> = unsafe { library.get(b"DTWAIN_SetFeederAlignment")? };
        let DTWAIN_SetFeederOrder: Symbol<DtwainsetfeederorderFunc> = unsafe { library.get(b"DTWAIN_SetFeederOrder")? };
        let DTWAIN_SetFeederWaitTime: Symbol<DtwainsetfeederwaittimeFunc> = unsafe { library.get(b"DTWAIN_SetFeederWaitTime")? };
        let DTWAIN_SetFileAutoIncrement: Symbol<DtwainsetfileautoincrementFunc> = unsafe { library.get(b"DTWAIN_SetFileAutoIncrement")? };
        let DTWAIN_SetFileCompressionType: Symbol<DtwainsetfilecompressiontypeFunc> = unsafe { library.get(b"DTWAIN_SetFileCompressionType")? };
        let DTWAIN_SetFileSavePos: Symbol<DtwainsetfilesaveposFunc> = unsafe { library.get(b"DTWAIN_SetFileSavePos")? };
        let DTWAIN_SetFileSavePosA: Symbol<DtwainsetfilesaveposaFunc> = unsafe { library.get(b"DTWAIN_SetFileSavePosA")? };
        let DTWAIN_SetFileSavePosW: Symbol<DtwainsetfilesaveposwFunc> = unsafe { library.get(b"DTWAIN_SetFileSavePosW")? };
        let DTWAIN_SetFileXferFormat: Symbol<DtwainsetfilexferformatFunc> = unsafe { library.get(b"DTWAIN_SetFileXferFormat")? };
        let DTWAIN_SetHalftone: Symbol<DtwainsethalftoneFunc> = unsafe { library.get(b"DTWAIN_SetHalftone")? };
        let DTWAIN_SetHalftoneA: Symbol<DtwainsethalftoneaFunc> = unsafe { library.get(b"DTWAIN_SetHalftoneA")? };
        let DTWAIN_SetHalftoneW: Symbol<DtwainsethalftonewFunc> = unsafe { library.get(b"DTWAIN_SetHalftoneW")? };
        let DTWAIN_SetHighlight: Symbol<DtwainsethighlightFunc> = unsafe { library.get(b"DTWAIN_SetHighlight")? };
        let DTWAIN_SetHighlightString: Symbol<DtwainsethighlightstringFunc> = unsafe { library.get(b"DTWAIN_SetHighlightString")? };
        let DTWAIN_SetHighlightStringA: Symbol<DtwainsethighlightstringaFunc> = unsafe { library.get(b"DTWAIN_SetHighlightStringA")? };
        let DTWAIN_SetHighlightStringW: Symbol<DtwainsethighlightstringwFunc> = unsafe { library.get(b"DTWAIN_SetHighlightStringW")? };
        let DTWAIN_SetJobControl: Symbol<DtwainsetjobcontrolFunc> = unsafe { library.get(b"DTWAIN_SetJobControl")? };
        let DTWAIN_SetJpegValues: Symbol<DtwainsetjpegvaluesFunc> = unsafe { library.get(b"DTWAIN_SetJpegValues")? };
        let DTWAIN_SetJpegXRValues: Symbol<DtwainsetjpegxrvaluesFunc> = unsafe { library.get(b"DTWAIN_SetJpegXRValues")? };
        let DTWAIN_SetLanguage: Symbol<DtwainsetlanguageFunc> = unsafe { library.get(b"DTWAIN_SetLanguage")? };
        let DTWAIN_SetLastError: Symbol<DtwainsetlasterrorFunc> = unsafe { library.get(b"DTWAIN_SetLastError")? };
        let DTWAIN_SetLightPath: Symbol<DtwainsetlightpathFunc> = unsafe { library.get(b"DTWAIN_SetLightPath")? };
        let DTWAIN_SetLightPathEx: Symbol<DtwainsetlightpathexFunc> = unsafe { library.get(b"DTWAIN_SetLightPathEx")? };
        let DTWAIN_SetLightSource: Symbol<DtwainsetlightsourceFunc> = unsafe { library.get(b"DTWAIN_SetLightSource")? };
        let DTWAIN_SetLightSources: Symbol<DtwainsetlightsourcesFunc> = unsafe { library.get(b"DTWAIN_SetLightSources")? };
        let DTWAIN_SetLoggerCallback: Symbol<DtwainsetloggercallbackFunc> = unsafe { library.get(b"DTWAIN_SetLoggerCallback")? };
        let DTWAIN_SetLoggerCallbackA: Symbol<DtwainsetloggercallbackaFunc> = unsafe { library.get(b"DTWAIN_SetLoggerCallbackA")? };
        let DTWAIN_SetLoggerCallbackW: Symbol<DtwainsetloggercallbackwFunc> = unsafe { library.get(b"DTWAIN_SetLoggerCallbackW")? };
        let DTWAIN_SetManualDuplexMode: Symbol<DtwainsetmanualduplexmodeFunc> = unsafe { library.get(b"DTWAIN_SetManualDuplexMode")? };
        let DTWAIN_SetMaxAcquisitions: Symbol<DtwainsetmaxacquisitionsFunc> = unsafe { library.get(b"DTWAIN_SetMaxAcquisitions")? };
        let DTWAIN_SetMaxBuffers: Symbol<DtwainsetmaxbuffersFunc> = unsafe { library.get(b"DTWAIN_SetMaxBuffers")? };
        let DTWAIN_SetMaxRetryAttempts: Symbol<DtwainsetmaxretryattemptsFunc> = unsafe { library.get(b"DTWAIN_SetMaxRetryAttempts")? };
        let DTWAIN_SetMultipageScanMode: Symbol<DtwainsetmultipagescanmodeFunc> = unsafe { library.get(b"DTWAIN_SetMultipageScanMode")? };
        let DTWAIN_SetNoiseFilter: Symbol<DtwainsetnoisefilterFunc> = unsafe { library.get(b"DTWAIN_SetNoiseFilter")? };
        let DTWAIN_SetOCRCapValues: Symbol<DtwainsetocrcapvaluesFunc> = unsafe { library.get(b"DTWAIN_SetOCRCapValues")? };
        let DTWAIN_SetOrientation: Symbol<DtwainsetorientationFunc> = unsafe { library.get(b"DTWAIN_SetOrientation")? };
        let DTWAIN_SetOverscan: Symbol<DtwainsetoverscanFunc> = unsafe { library.get(b"DTWAIN_SetOverscan")? };
        let DTWAIN_SetPDFAESEncryption: Symbol<DtwainsetpdfaesencryptionFunc> = unsafe { library.get(b"DTWAIN_SetPDFAESEncryption")? };
        let DTWAIN_SetPDFASCIICompression: Symbol<DtwainsetpdfasciicompressionFunc> = unsafe { library.get(b"DTWAIN_SetPDFASCIICompression")? };
        let DTWAIN_SetPDFAuthor: Symbol<DtwainsetpdfauthorFunc> = unsafe { library.get(b"DTWAIN_SetPDFAuthor")? };
        let DTWAIN_SetPDFAuthorA: Symbol<DtwainsetpdfauthoraFunc> = unsafe { library.get(b"DTWAIN_SetPDFAuthorA")? };
        let DTWAIN_SetPDFAuthorW: Symbol<DtwainsetpdfauthorwFunc> = unsafe { library.get(b"DTWAIN_SetPDFAuthorW")? };
        let DTWAIN_SetPDFCompression: Symbol<DtwainsetpdfcompressionFunc> = unsafe { library.get(b"DTWAIN_SetPDFCompression")? };
        let DTWAIN_SetPDFCreator: Symbol<DtwainsetpdfcreatorFunc> = unsafe { library.get(b"DTWAIN_SetPDFCreator")? };
        let DTWAIN_SetPDFCreatorA: Symbol<DtwainsetpdfcreatoraFunc> = unsafe { library.get(b"DTWAIN_SetPDFCreatorA")? };
        let DTWAIN_SetPDFCreatorW: Symbol<DtwainsetpdfcreatorwFunc> = unsafe { library.get(b"DTWAIN_SetPDFCreatorW")? };
        let DTWAIN_SetPDFEncryption: Symbol<DtwainsetpdfencryptionFunc> = unsafe { library.get(b"DTWAIN_SetPDFEncryption")? };
        let DTWAIN_SetPDFEncryptionA: Symbol<DtwainsetpdfencryptionaFunc> = unsafe { library.get(b"DTWAIN_SetPDFEncryptionA")? };
        let DTWAIN_SetPDFEncryptionW: Symbol<DtwainsetpdfencryptionwFunc> = unsafe { library.get(b"DTWAIN_SetPDFEncryptionW")? };
        let DTWAIN_SetPDFJpegQuality: Symbol<DtwainsetpdfjpegqualityFunc> = unsafe { library.get(b"DTWAIN_SetPDFJpegQuality")? };
        let DTWAIN_SetPDFKeywords: Symbol<DtwainsetpdfkeywordsFunc> = unsafe { library.get(b"DTWAIN_SetPDFKeywords")? };
        let DTWAIN_SetPDFKeywordsA: Symbol<DtwainsetpdfkeywordsaFunc> = unsafe { library.get(b"DTWAIN_SetPDFKeywordsA")? };
        let DTWAIN_SetPDFKeywordsW: Symbol<DtwainsetpdfkeywordswFunc> = unsafe { library.get(b"DTWAIN_SetPDFKeywordsW")? };
        let DTWAIN_SetPDFOCRConversion: Symbol<DtwainsetpdfocrconversionFunc> = unsafe { library.get(b"DTWAIN_SetPDFOCRConversion")? };
        let DTWAIN_SetPDFOCRMode: Symbol<DtwainsetpdfocrmodeFunc> = unsafe { library.get(b"DTWAIN_SetPDFOCRMode")? };
        let DTWAIN_SetPDFOrientation: Symbol<DtwainsetpdforientationFunc> = unsafe { library.get(b"DTWAIN_SetPDFOrientation")? };
        let DTWAIN_SetPDFPageScale: Symbol<DtwainsetpdfpagescaleFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageScale")? };
        let DTWAIN_SetPDFPageScaleString: Symbol<DtwainsetpdfpagescalestringFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageScaleString")? };
        let DTWAIN_SetPDFPageScaleStringA: Symbol<DtwainsetpdfpagescalestringaFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageScaleStringA")? };
        let DTWAIN_SetPDFPageScaleStringW: Symbol<DtwainsetpdfpagescalestringwFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageScaleStringW")? };
        let DTWAIN_SetPDFPageSize: Symbol<DtwainsetpdfpagesizeFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageSize")? };
        let DTWAIN_SetPDFPageSizeString: Symbol<DtwainsetpdfpagesizestringFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageSizeString")? };
        let DTWAIN_SetPDFPageSizeStringA: Symbol<DtwainsetpdfpagesizestringaFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageSizeStringA")? };
        let DTWAIN_SetPDFPageSizeStringW: Symbol<DtwainsetpdfpagesizestringwFunc> = unsafe { library.get(b"DTWAIN_SetPDFPageSizeStringW")? };
        let DTWAIN_SetPDFPolarity: Symbol<DtwainsetpdfpolarityFunc> = unsafe { library.get(b"DTWAIN_SetPDFPolarity")? };
        let DTWAIN_SetPDFProducer: Symbol<DtwainsetpdfproducerFunc> = unsafe { library.get(b"DTWAIN_SetPDFProducer")? };
        let DTWAIN_SetPDFProducerA: Symbol<DtwainsetpdfproduceraFunc> = unsafe { library.get(b"DTWAIN_SetPDFProducerA")? };
        let DTWAIN_SetPDFProducerW: Symbol<DtwainsetpdfproducerwFunc> = unsafe { library.get(b"DTWAIN_SetPDFProducerW")? };
        let DTWAIN_SetPDFSubject: Symbol<DtwainsetpdfsubjectFunc> = unsafe { library.get(b"DTWAIN_SetPDFSubject")? };
        let DTWAIN_SetPDFSubjectA: Symbol<DtwainsetpdfsubjectaFunc> = unsafe { library.get(b"DTWAIN_SetPDFSubjectA")? };
        let DTWAIN_SetPDFSubjectW: Symbol<DtwainsetpdfsubjectwFunc> = unsafe { library.get(b"DTWAIN_SetPDFSubjectW")? };
        let DTWAIN_SetPDFTextElementFloat: Symbol<DtwainsetpdftextelementfloatFunc> = unsafe { library.get(b"DTWAIN_SetPDFTextElementFloat")? };
        let DTWAIN_SetPDFTextElementLong: Symbol<DtwainsetpdftextelementlongFunc> = unsafe { library.get(b"DTWAIN_SetPDFTextElementLong")? };
        let DTWAIN_SetPDFTextElementString: Symbol<DtwainsetpdftextelementstringFunc> = unsafe { library.get(b"DTWAIN_SetPDFTextElementString")? };
        let DTWAIN_SetPDFTextElementStringA: Symbol<DtwainsetpdftextelementstringaFunc> = unsafe { library.get(b"DTWAIN_SetPDFTextElementStringA")? };
        let DTWAIN_SetPDFTextElementStringW: Symbol<DtwainsetpdftextelementstringwFunc> = unsafe { library.get(b"DTWAIN_SetPDFTextElementStringW")? };
        let DTWAIN_SetPDFTitle: Symbol<DtwainsetpdftitleFunc> = unsafe { library.get(b"DTWAIN_SetPDFTitle")? };
        let DTWAIN_SetPDFTitleA: Symbol<DtwainsetpdftitleaFunc> = unsafe { library.get(b"DTWAIN_SetPDFTitleA")? };
        let DTWAIN_SetPDFTitleW: Symbol<DtwainsetpdftitlewFunc> = unsafe { library.get(b"DTWAIN_SetPDFTitleW")? };
        let DTWAIN_SetPaperSize: Symbol<DtwainsetpapersizeFunc> = unsafe { library.get(b"DTWAIN_SetPaperSize")? };
        let DTWAIN_SetPatchMaxPriorities: Symbol<DtwainsetpatchmaxprioritiesFunc> = unsafe { library.get(b"DTWAIN_SetPatchMaxPriorities")? };
        let DTWAIN_SetPatchMaxRetries: Symbol<DtwainsetpatchmaxretriesFunc> = unsafe { library.get(b"DTWAIN_SetPatchMaxRetries")? };
        let DTWAIN_SetPatchPriorities: Symbol<DtwainsetpatchprioritiesFunc> = unsafe { library.get(b"DTWAIN_SetPatchPriorities")? };
        let DTWAIN_SetPatchSearchMode: Symbol<DtwainsetpatchsearchmodeFunc> = unsafe { library.get(b"DTWAIN_SetPatchSearchMode")? };
        let DTWAIN_SetPatchTimeOut: Symbol<DtwainsetpatchtimeoutFunc> = unsafe { library.get(b"DTWAIN_SetPatchTimeOut")? };
        let DTWAIN_SetPixelFlavor: Symbol<DtwainsetpixelflavorFunc> = unsafe { library.get(b"DTWAIN_SetPixelFlavor")? };
        let DTWAIN_SetPixelType: Symbol<DtwainsetpixeltypeFunc> = unsafe { library.get(b"DTWAIN_SetPixelType")? };
        let DTWAIN_SetPostScriptTitle: Symbol<DtwainsetpostscripttitleFunc> = unsafe { library.get(b"DTWAIN_SetPostScriptTitle")? };
        let DTWAIN_SetPostScriptTitleA: Symbol<DtwainsetpostscripttitleaFunc> = unsafe { library.get(b"DTWAIN_SetPostScriptTitleA")? };
        let DTWAIN_SetPostScriptTitleW: Symbol<DtwainsetpostscripttitlewFunc> = unsafe { library.get(b"DTWAIN_SetPostScriptTitleW")? };
        let DTWAIN_SetPostScriptType: Symbol<DtwainsetpostscripttypeFunc> = unsafe { library.get(b"DTWAIN_SetPostScriptType")? };
        let DTWAIN_SetPrinter: Symbol<DtwainsetprinterFunc> = unsafe { library.get(b"DTWAIN_SetPrinter")? };
        let DTWAIN_SetPrinterEx: Symbol<DtwainsetprinterexFunc> = unsafe { library.get(b"DTWAIN_SetPrinterEx")? };
        let DTWAIN_SetPrinterStartNumber: Symbol<DtwainsetprinterstartnumberFunc> = unsafe { library.get(b"DTWAIN_SetPrinterStartNumber")? };
        let DTWAIN_SetPrinterStringMode: Symbol<DtwainsetprinterstringmodeFunc> = unsafe { library.get(b"DTWAIN_SetPrinterStringMode")? };
        let DTWAIN_SetPrinterStrings: Symbol<DtwainsetprinterstringsFunc> = unsafe { library.get(b"DTWAIN_SetPrinterStrings")? };
        let DTWAIN_SetPrinterSuffixString: Symbol<DtwainsetprintersuffixstringFunc> = unsafe { library.get(b"DTWAIN_SetPrinterSuffixString")? };
        let DTWAIN_SetPrinterSuffixStringA: Symbol<DtwainsetprintersuffixstringaFunc> = unsafe { library.get(b"DTWAIN_SetPrinterSuffixStringA")? };
        let DTWAIN_SetPrinterSuffixStringW: Symbol<DtwainsetprintersuffixstringwFunc> = unsafe { library.get(b"DTWAIN_SetPrinterSuffixStringW")? };
        let DTWAIN_SetQueryCapSupport: Symbol<DtwainsetquerycapsupportFunc> = unsafe { library.get(b"DTWAIN_SetQueryCapSupport")? };
        let DTWAIN_SetResolution: Symbol<DtwainsetresolutionFunc> = unsafe { library.get(b"DTWAIN_SetResolution")? };
        let DTWAIN_SetResolutionString: Symbol<DtwainsetresolutionstringFunc> = unsafe { library.get(b"DTWAIN_SetResolutionString")? };
        let DTWAIN_SetResolutionStringA: Symbol<DtwainsetresolutionstringaFunc> = unsafe { library.get(b"DTWAIN_SetResolutionStringA")? };
        let DTWAIN_SetResolutionStringW: Symbol<DtwainsetresolutionstringwFunc> = unsafe { library.get(b"DTWAIN_SetResolutionStringW")? };
        let DTWAIN_SetResourcePath: Symbol<DtwainsetresourcepathFunc> = unsafe { library.get(b"DTWAIN_SetResourcePath")? };
        let DTWAIN_SetResourcePathA: Symbol<DtwainsetresourcepathaFunc> = unsafe { library.get(b"DTWAIN_SetResourcePathA")? };
        let DTWAIN_SetResourcePathW: Symbol<DtwainsetresourcepathwFunc> = unsafe { library.get(b"DTWAIN_SetResourcePathW")? };
        let DTWAIN_SetRotation: Symbol<DtwainsetrotationFunc> = unsafe { library.get(b"DTWAIN_SetRotation")? };
        let DTWAIN_SetRotationString: Symbol<DtwainsetrotationstringFunc> = unsafe { library.get(b"DTWAIN_SetRotationString")? };
        let DTWAIN_SetRotationStringA: Symbol<DtwainsetrotationstringaFunc> = unsafe { library.get(b"DTWAIN_SetRotationStringA")? };
        let DTWAIN_SetRotationStringW: Symbol<DtwainsetrotationstringwFunc> = unsafe { library.get(b"DTWAIN_SetRotationStringW")? };
        let DTWAIN_SetSaveFileName: Symbol<DtwainsetsavefilenameFunc> = unsafe { library.get(b"DTWAIN_SetSaveFileName")? };
        let DTWAIN_SetSaveFileNameA: Symbol<DtwainsetsavefilenameaFunc> = unsafe { library.get(b"DTWAIN_SetSaveFileNameA")? };
        let DTWAIN_SetSaveFileNameW: Symbol<DtwainsetsavefilenamewFunc> = unsafe { library.get(b"DTWAIN_SetSaveFileNameW")? };
        let DTWAIN_SetShadow: Symbol<DtwainsetshadowFunc> = unsafe { library.get(b"DTWAIN_SetShadow")? };
        let DTWAIN_SetShadowString: Symbol<DtwainsetshadowstringFunc> = unsafe { library.get(b"DTWAIN_SetShadowString")? };
        let DTWAIN_SetShadowStringA: Symbol<DtwainsetshadowstringaFunc> = unsafe { library.get(b"DTWAIN_SetShadowStringA")? };
        let DTWAIN_SetShadowStringW: Symbol<DtwainsetshadowstringwFunc> = unsafe { library.get(b"DTWAIN_SetShadowStringW")? };
        let DTWAIN_SetSourceUnit: Symbol<DtwainsetsourceunitFunc> = unsafe { library.get(b"DTWAIN_SetSourceUnit")? };
        let DTWAIN_SetTIFFCompressType: Symbol<DtwainsettiffcompresstypeFunc> = unsafe { library.get(b"DTWAIN_SetTIFFCompressType")? };
        let DTWAIN_SetTIFFInvert: Symbol<DtwainsettiffinvertFunc> = unsafe { library.get(b"DTWAIN_SetTIFFInvert")? };
        let DTWAIN_SetTempFileDirectory: Symbol<DtwainsettempfiledirectoryFunc> = unsafe { library.get(b"DTWAIN_SetTempFileDirectory")? };
        let DTWAIN_SetTempFileDirectoryA: Symbol<DtwainsettempfiledirectoryaFunc> = unsafe { library.get(b"DTWAIN_SetTempFileDirectoryA")? };
        let DTWAIN_SetTempFileDirectoryEx: Symbol<DtwainsettempfiledirectoryexFunc> = unsafe { library.get(b"DTWAIN_SetTempFileDirectoryEx")? };
        let DTWAIN_SetTempFileDirectoryExA: Symbol<DtwainsettempfiledirectoryexaFunc> = unsafe { library.get(b"DTWAIN_SetTempFileDirectoryExA")? };
        let DTWAIN_SetTempFileDirectoryExW: Symbol<DtwainsettempfiledirectoryexwFunc> = unsafe { library.get(b"DTWAIN_SetTempFileDirectoryExW")? };
        let DTWAIN_SetTempFileDirectoryW: Symbol<DtwainsettempfiledirectorywFunc> = unsafe { library.get(b"DTWAIN_SetTempFileDirectoryW")? };
        let DTWAIN_SetThreshold: Symbol<DtwainsetthresholdFunc> = unsafe { library.get(b"DTWAIN_SetThreshold")? };
        let DTWAIN_SetThresholdString: Symbol<DtwainsetthresholdstringFunc> = unsafe { library.get(b"DTWAIN_SetThresholdString")? };
        let DTWAIN_SetThresholdStringA: Symbol<DtwainsetthresholdstringaFunc> = unsafe { library.get(b"DTWAIN_SetThresholdStringA")? };
        let DTWAIN_SetThresholdStringW: Symbol<DtwainsetthresholdstringwFunc> = unsafe { library.get(b"DTWAIN_SetThresholdStringW")? };
        let DTWAIN_SetTwainDSM: Symbol<DtwainsettwaindsmFunc> = unsafe { library.get(b"DTWAIN_SetTwainDSM")? };
        let DTWAIN_SetTwainLog: Symbol<DtwainsettwainlogFunc> = unsafe { library.get(b"DTWAIN_SetTwainLog")? };
        let DTWAIN_SetTwainLogA: Symbol<DtwainsettwainlogaFunc> = unsafe { library.get(b"DTWAIN_SetTwainLogA")? };
        let DTWAIN_SetTwainLogW: Symbol<DtwainsettwainlogwFunc> = unsafe { library.get(b"DTWAIN_SetTwainLogW")? };
        let DTWAIN_SetTwainMode: Symbol<DtwainsettwainmodeFunc> = unsafe { library.get(b"DTWAIN_SetTwainMode")? };
        let DTWAIN_SetTwainTimeout: Symbol<DtwainsettwaintimeoutFunc> = unsafe { library.get(b"DTWAIN_SetTwainTimeout")? };
        let DTWAIN_SetUpdateDibProc: Symbol<DtwainsetupdatedibprocFunc> = unsafe { library.get(b"DTWAIN_SetUpdateDibProc")? };
        let DTWAIN_SetXResolution: Symbol<DtwainsetxresolutionFunc> = unsafe { library.get(b"DTWAIN_SetXResolution")? };
        let DTWAIN_SetXResolutionString: Symbol<DtwainsetxresolutionstringFunc> = unsafe { library.get(b"DTWAIN_SetXResolutionString")? };
        let DTWAIN_SetXResolutionStringA: Symbol<DtwainsetxresolutionstringaFunc> = unsafe { library.get(b"DTWAIN_SetXResolutionStringA")? };
        let DTWAIN_SetXResolutionStringW: Symbol<DtwainsetxresolutionstringwFunc> = unsafe { library.get(b"DTWAIN_SetXResolutionStringW")? };
        let DTWAIN_SetYResolution: Symbol<DtwainsetyresolutionFunc> = unsafe { library.get(b"DTWAIN_SetYResolution")? };
        let DTWAIN_SetYResolutionString: Symbol<DtwainsetyresolutionstringFunc> = unsafe { library.get(b"DTWAIN_SetYResolutionString")? };
        let DTWAIN_SetYResolutionStringA: Symbol<DtwainsetyresolutionstringaFunc> = unsafe { library.get(b"DTWAIN_SetYResolutionStringA")? };
        let DTWAIN_SetYResolutionStringW: Symbol<DtwainsetyresolutionstringwFunc> = unsafe { library.get(b"DTWAIN_SetYResolutionStringW")? };
        let DTWAIN_ShowUIOnly: Symbol<DtwainshowuionlyFunc> = unsafe { library.get(b"DTWAIN_ShowUIOnly")? };
        let DTWAIN_ShutdownOCREngine: Symbol<DtwainshutdownocrengineFunc> = unsafe { library.get(b"DTWAIN_ShutdownOCREngine")? };
        let DTWAIN_SkipImageInfoError: Symbol<DtwainskipimageinfoerrorFunc> = unsafe { library.get(b"DTWAIN_SkipImageInfoError")? };
        let DTWAIN_StartThread: Symbol<DtwainstartthreadFunc> = unsafe { library.get(b"DTWAIN_StartThread")? };
        let DTWAIN_StartTwainSession: Symbol<DtwainstarttwainsessionFunc> = unsafe { library.get(b"DTWAIN_StartTwainSession")? };
        let DTWAIN_StartTwainSessionA: Symbol<DtwainstarttwainsessionaFunc> = unsafe { library.get(b"DTWAIN_StartTwainSessionA")? };
        let DTWAIN_StartTwainSessionW: Symbol<DtwainstarttwainsessionwFunc> = unsafe { library.get(b"DTWAIN_StartTwainSessionW")? };
        let DTWAIN_SysDestroy: Symbol<DtwainsysdestroyFunc> = unsafe { library.get(b"DTWAIN_SysDestroy")? };
        let DTWAIN_SysInitialize: Symbol<DtwainsysinitializeFunc> = unsafe { library.get(b"DTWAIN_SysInitialize")? };
        let DTWAIN_SysInitializeEx: Symbol<DtwainsysinitializeexFunc> = unsafe { library.get(b"DTWAIN_SysInitializeEx")? };
        let DTWAIN_SysInitializeEx2: Symbol<Dtwainsysinitializeex2Func> = unsafe { library.get(b"DTWAIN_SysInitializeEx2")? };
        let DTWAIN_SysInitializeEx2A: Symbol<Dtwainsysinitializeex2aFunc> = unsafe { library.get(b"DTWAIN_SysInitializeEx2A")? };
        let DTWAIN_SysInitializeEx2W: Symbol<Dtwainsysinitializeex2wFunc> = unsafe { library.get(b"DTWAIN_SysInitializeEx2W")? };
        let DTWAIN_SysInitializeExA: Symbol<DtwainsysinitializeexaFunc> = unsafe { library.get(b"DTWAIN_SysInitializeExA")? };
        let DTWAIN_SysInitializeExW: Symbol<DtwainsysinitializeexwFunc> = unsafe { library.get(b"DTWAIN_SysInitializeExW")? };
        let DTWAIN_SysInitializeLib: Symbol<DtwainsysinitializelibFunc> = unsafe { library.get(b"DTWAIN_SysInitializeLib")? };
        let DTWAIN_SysInitializeLibEx: Symbol<DtwainsysinitializelibexFunc> = unsafe { library.get(b"DTWAIN_SysInitializeLibEx")? };
        let DTWAIN_SysInitializeLibEx2: Symbol<Dtwainsysinitializelibex2Func> = unsafe { library.get(b"DTWAIN_SysInitializeLibEx2")? };
        let DTWAIN_SysInitializeLibEx2A: Symbol<Dtwainsysinitializelibex2aFunc> = unsafe { library.get(b"DTWAIN_SysInitializeLibEx2A")? };
        let DTWAIN_SysInitializeLibEx2W: Symbol<Dtwainsysinitializelibex2wFunc> = unsafe { library.get(b"DTWAIN_SysInitializeLibEx2W")? };
        let DTWAIN_SysInitializeLibExA: Symbol<DtwainsysinitializelibexaFunc> = unsafe { library.get(b"DTWAIN_SysInitializeLibExA")? };
        let DTWAIN_SysInitializeLibExW: Symbol<DtwainsysinitializelibexwFunc> = unsafe { library.get(b"DTWAIN_SysInitializeLibExW")? };
        let DTWAIN_SysInitializeNoBlocking: Symbol<DtwainsysinitializenoblockingFunc> = unsafe { library.get(b"DTWAIN_SysInitializeNoBlocking")? };
        let DTWAIN_TestGetCap: Symbol<DtwaintestgetcapFunc> = unsafe { library.get(b"DTWAIN_TestGetCap")? };
        let DTWAIN_UnlockMemory: Symbol<DtwainunlockmemoryFunc> = unsafe { library.get(b"DTWAIN_UnlockMemory")? };
        let DTWAIN_UnlockMemoryEx: Symbol<DtwainunlockmemoryexFunc> = unsafe { library.get(b"DTWAIN_UnlockMemoryEx")? };
        let DTWAIN_UseMultipleThreads: Symbol<DtwainusemultiplethreadsFunc> = unsafe { library.get(b"DTWAIN_UseMultipleThreads")? };
        Ok(Self {
            DTWAIN_AcquireAudioFileFunc: DTWAIN_AcquireAudioFile,
            DTWAIN_AcquireAudioFileAFunc: DTWAIN_AcquireAudioFileA,
            DTWAIN_AcquireAudioFileWFunc: DTWAIN_AcquireAudioFileW,
            DTWAIN_AcquireAudioNativeFunc: DTWAIN_AcquireAudioNative,
            DTWAIN_AcquireAudioNativeExFunc: DTWAIN_AcquireAudioNativeEx,
            DTWAIN_AcquireBufferedFunc: DTWAIN_AcquireBuffered,
            DTWAIN_AcquireBufferedExFunc: DTWAIN_AcquireBufferedEx,
            DTWAIN_AcquireFileFunc: DTWAIN_AcquireFile,
            DTWAIN_AcquireFileAFunc: DTWAIN_AcquireFileA,
            DTWAIN_AcquireFileExFunc: DTWAIN_AcquireFileEx,
            DTWAIN_AcquireFileWFunc: DTWAIN_AcquireFileW,
            DTWAIN_AcquireNativeFunc: DTWAIN_AcquireNative,
            DTWAIN_AcquireNativeExFunc: DTWAIN_AcquireNativeEx,
            DTWAIN_AcquireToClipboardFunc: DTWAIN_AcquireToClipboard,
            DTWAIN_AddExtImageInfoQueryFunc: DTWAIN_AddExtImageInfoQuery,
            DTWAIN_AddPDFTextFunc: DTWAIN_AddPDFText,
            DTWAIN_AddPDFTextAFunc: DTWAIN_AddPDFTextA,
            DTWAIN_AddPDFTextExFunc: DTWAIN_AddPDFTextEx,
            DTWAIN_AddPDFTextWFunc: DTWAIN_AddPDFTextW,
            DTWAIN_AllocateMemoryFunc: DTWAIN_AllocateMemory,
            DTWAIN_AllocateMemory64Func: DTWAIN_AllocateMemory64,
            DTWAIN_AllocateMemoryExFunc: DTWAIN_AllocateMemoryEx,
            DTWAIN_AppHandlesExceptionsFunc: DTWAIN_AppHandlesExceptions,
            DTWAIN_ArrayANSIStringToFloatFunc: DTWAIN_ArrayANSIStringToFloat,
            DTWAIN_ArrayAddFunc: DTWAIN_ArrayAdd,
            DTWAIN_ArrayAddANSIStringFunc: DTWAIN_ArrayAddANSIString,
            DTWAIN_ArrayAddANSIStringNFunc: DTWAIN_ArrayAddANSIStringN,
            DTWAIN_ArrayAddFloatFunc: DTWAIN_ArrayAddFloat,
            DTWAIN_ArrayAddFloatNFunc: DTWAIN_ArrayAddFloatN,
            DTWAIN_ArrayAddFloatStringFunc: DTWAIN_ArrayAddFloatString,
            DTWAIN_ArrayAddFloatStringAFunc: DTWAIN_ArrayAddFloatStringA,
            DTWAIN_ArrayAddFloatStringNFunc: DTWAIN_ArrayAddFloatStringN,
            DTWAIN_ArrayAddFloatStringNAFunc: DTWAIN_ArrayAddFloatStringNA,
            DTWAIN_ArrayAddFloatStringNWFunc: DTWAIN_ArrayAddFloatStringNW,
            DTWAIN_ArrayAddFloatStringWFunc: DTWAIN_ArrayAddFloatStringW,
            DTWAIN_ArrayAddFrameFunc: DTWAIN_ArrayAddFrame,
            DTWAIN_ArrayAddFrameNFunc: DTWAIN_ArrayAddFrameN,
            DTWAIN_ArrayAddLongFunc: DTWAIN_ArrayAddLong,
            DTWAIN_ArrayAddLong64Func: DTWAIN_ArrayAddLong64,
            DTWAIN_ArrayAddLong64NFunc: DTWAIN_ArrayAddLong64N,
            DTWAIN_ArrayAddLongNFunc: DTWAIN_ArrayAddLongN,
            DTWAIN_ArrayAddNFunc: DTWAIN_ArrayAddN,
            DTWAIN_ArrayAddStringFunc: DTWAIN_ArrayAddString,
            DTWAIN_ArrayAddStringAFunc: DTWAIN_ArrayAddStringA,
            DTWAIN_ArrayAddStringNFunc: DTWAIN_ArrayAddStringN,
            DTWAIN_ArrayAddStringNAFunc: DTWAIN_ArrayAddStringNA,
            DTWAIN_ArrayAddStringNWFunc: DTWAIN_ArrayAddStringNW,
            DTWAIN_ArrayAddStringWFunc: DTWAIN_ArrayAddStringW,
            DTWAIN_ArrayAddWideStringFunc: DTWAIN_ArrayAddWideString,
            DTWAIN_ArrayAddWideStringNFunc: DTWAIN_ArrayAddWideStringN,
            DTWAIN_ArrayConvertFix32ToFloatFunc: DTWAIN_ArrayConvertFix32ToFloat,
            DTWAIN_ArrayConvertFloatToFix32Func: DTWAIN_ArrayConvertFloatToFix32,
            DTWAIN_ArrayCopyFunc: DTWAIN_ArrayCopy,
            DTWAIN_ArrayCreateFunc: DTWAIN_ArrayCreate,
            DTWAIN_ArrayCreateCopyFunc: DTWAIN_ArrayCreateCopy,
            DTWAIN_ArrayCreateFromCapFunc: DTWAIN_ArrayCreateFromCap,
            DTWAIN_ArrayCreateFromLong64sFunc: DTWAIN_ArrayCreateFromLong64s,
            DTWAIN_ArrayCreateFromLongsFunc: DTWAIN_ArrayCreateFromLongs,
            DTWAIN_ArrayCreateFromRealsFunc: DTWAIN_ArrayCreateFromReals,
            DTWAIN_ArrayDestroyFunc: DTWAIN_ArrayDestroy,
            DTWAIN_ArrayDestroyFramesFunc: DTWAIN_ArrayDestroyFrames,
            DTWAIN_ArrayFindFunc: DTWAIN_ArrayFind,
            DTWAIN_ArrayFindANSIStringFunc: DTWAIN_ArrayFindANSIString,
            DTWAIN_ArrayFindFloatFunc: DTWAIN_ArrayFindFloat,
            DTWAIN_ArrayFindFloatStringFunc: DTWAIN_ArrayFindFloatString,
            DTWAIN_ArrayFindFloatStringAFunc: DTWAIN_ArrayFindFloatStringA,
            DTWAIN_ArrayFindFloatStringWFunc: DTWAIN_ArrayFindFloatStringW,
            DTWAIN_ArrayFindLongFunc: DTWAIN_ArrayFindLong,
            DTWAIN_ArrayFindLong64Func: DTWAIN_ArrayFindLong64,
            DTWAIN_ArrayFindStringFunc: DTWAIN_ArrayFindString,
            DTWAIN_ArrayFindStringAFunc: DTWAIN_ArrayFindStringA,
            DTWAIN_ArrayFindStringWFunc: DTWAIN_ArrayFindStringW,
            DTWAIN_ArrayFindWideStringFunc: DTWAIN_ArrayFindWideString,
            DTWAIN_ArrayFix32GetAtFunc: DTWAIN_ArrayFix32GetAt,
            DTWAIN_ArrayFix32SetAtFunc: DTWAIN_ArrayFix32SetAt,
            DTWAIN_ArrayFloatToANSIStringFunc: DTWAIN_ArrayFloatToANSIString,
            DTWAIN_ArrayFloatToStringFunc: DTWAIN_ArrayFloatToString,
            DTWAIN_ArrayFloatToWideStringFunc: DTWAIN_ArrayFloatToWideString,
            DTWAIN_ArrayGetAtFunc: DTWAIN_ArrayGetAt,
            DTWAIN_ArrayGetAtANSIStringFunc: DTWAIN_ArrayGetAtANSIString,
            DTWAIN_ArrayGetAtANSIStringPtrFunc: DTWAIN_ArrayGetAtANSIStringPtr,
            DTWAIN_ArrayGetAtFloatFunc: DTWAIN_ArrayGetAtFloat,
            DTWAIN_ArrayGetAtFloatStringFunc: DTWAIN_ArrayGetAtFloatString,
            DTWAIN_ArrayGetAtFloatStringAFunc: DTWAIN_ArrayGetAtFloatStringA,
            DTWAIN_ArrayGetAtFloatStringWFunc: DTWAIN_ArrayGetAtFloatStringW,
            DTWAIN_ArrayGetAtFrameFunc: DTWAIN_ArrayGetAtFrame,
            DTWAIN_ArrayGetAtFrameExFunc: DTWAIN_ArrayGetAtFrameEx,
            DTWAIN_ArrayGetAtFrameStringFunc: DTWAIN_ArrayGetAtFrameString,
            DTWAIN_ArrayGetAtFrameStringAFunc: DTWAIN_ArrayGetAtFrameStringA,
            DTWAIN_ArrayGetAtFrameStringWFunc: DTWAIN_ArrayGetAtFrameStringW,
            DTWAIN_ArrayGetAtLongFunc: DTWAIN_ArrayGetAtLong,
            DTWAIN_ArrayGetAtLong64Func: DTWAIN_ArrayGetAtLong64,
            DTWAIN_ArrayGetAtSourceFunc: DTWAIN_ArrayGetAtSource,
            DTWAIN_ArrayGetAtStringFunc: DTWAIN_ArrayGetAtString,
            DTWAIN_ArrayGetAtStringAFunc: DTWAIN_ArrayGetAtStringA,
            DTWAIN_ArrayGetAtStringPtrFunc: DTWAIN_ArrayGetAtStringPtr,
            DTWAIN_ArrayGetAtStringWFunc: DTWAIN_ArrayGetAtStringW,
            DTWAIN_ArrayGetAtWideStringFunc: DTWAIN_ArrayGetAtWideString,
            DTWAIN_ArrayGetAtWideStringPtrFunc: DTWAIN_ArrayGetAtWideStringPtr,
            DTWAIN_ArrayGetBufferFunc: DTWAIN_ArrayGetBuffer,
            DTWAIN_ArrayGetCapValuesFunc: DTWAIN_ArrayGetCapValues,
            DTWAIN_ArrayGetCapValuesExFunc: DTWAIN_ArrayGetCapValuesEx,
            DTWAIN_ArrayGetCapValuesEx2Func: DTWAIN_ArrayGetCapValuesEx2,
            DTWAIN_ArrayGetCountFunc: DTWAIN_ArrayGetCount,
            DTWAIN_ArrayGetMaxStringLengthFunc: DTWAIN_ArrayGetMaxStringLength,
            DTWAIN_ArrayGetSourceAtFunc: DTWAIN_ArrayGetSourceAt,
            DTWAIN_ArrayGetStringLengthFunc: DTWAIN_ArrayGetStringLength,
            DTWAIN_ArrayGetTypeFunc: DTWAIN_ArrayGetType,
            DTWAIN_ArrayInitFunc: DTWAIN_ArrayInit,
            DTWAIN_ArrayInsertAtFunc: DTWAIN_ArrayInsertAt,
            DTWAIN_ArrayInsertAtANSIStringFunc: DTWAIN_ArrayInsertAtANSIString,
            DTWAIN_ArrayInsertAtANSIStringNFunc: DTWAIN_ArrayInsertAtANSIStringN,
            DTWAIN_ArrayInsertAtFloatFunc: DTWAIN_ArrayInsertAtFloat,
            DTWAIN_ArrayInsertAtFloatNFunc: DTWAIN_ArrayInsertAtFloatN,
            DTWAIN_ArrayInsertAtFloatStringFunc: DTWAIN_ArrayInsertAtFloatString,
            DTWAIN_ArrayInsertAtFloatStringAFunc: DTWAIN_ArrayInsertAtFloatStringA,
            DTWAIN_ArrayInsertAtFloatStringNFunc: DTWAIN_ArrayInsertAtFloatStringN,
            DTWAIN_ArrayInsertAtFloatStringNAFunc: DTWAIN_ArrayInsertAtFloatStringNA,
            DTWAIN_ArrayInsertAtFloatStringNWFunc: DTWAIN_ArrayInsertAtFloatStringNW,
            DTWAIN_ArrayInsertAtFloatStringWFunc: DTWAIN_ArrayInsertAtFloatStringW,
            DTWAIN_ArrayInsertAtFrameFunc: DTWAIN_ArrayInsertAtFrame,
            DTWAIN_ArrayInsertAtFrameNFunc: DTWAIN_ArrayInsertAtFrameN,
            DTWAIN_ArrayInsertAtLongFunc: DTWAIN_ArrayInsertAtLong,
            DTWAIN_ArrayInsertAtLong64Func: DTWAIN_ArrayInsertAtLong64,
            DTWAIN_ArrayInsertAtLong64NFunc: DTWAIN_ArrayInsertAtLong64N,
            DTWAIN_ArrayInsertAtLongNFunc: DTWAIN_ArrayInsertAtLongN,
            DTWAIN_ArrayInsertAtNFunc: DTWAIN_ArrayInsertAtN,
            DTWAIN_ArrayInsertAtStringFunc: DTWAIN_ArrayInsertAtString,
            DTWAIN_ArrayInsertAtStringAFunc: DTWAIN_ArrayInsertAtStringA,
            DTWAIN_ArrayInsertAtStringNFunc: DTWAIN_ArrayInsertAtStringN,
            DTWAIN_ArrayInsertAtStringNAFunc: DTWAIN_ArrayInsertAtStringNA,
            DTWAIN_ArrayInsertAtStringNWFunc: DTWAIN_ArrayInsertAtStringNW,
            DTWAIN_ArrayInsertAtStringWFunc: DTWAIN_ArrayInsertAtStringW,
            DTWAIN_ArrayInsertAtWideStringFunc: DTWAIN_ArrayInsertAtWideString,
            DTWAIN_ArrayInsertAtWideStringNFunc: DTWAIN_ArrayInsertAtWideStringN,
            DTWAIN_ArrayRemoveAllFunc: DTWAIN_ArrayRemoveAll,
            DTWAIN_ArrayRemoveAtFunc: DTWAIN_ArrayRemoveAt,
            DTWAIN_ArrayRemoveAtNFunc: DTWAIN_ArrayRemoveAtN,
            DTWAIN_ArrayResizeFunc: DTWAIN_ArrayResize,
            DTWAIN_ArraySetAtFunc: DTWAIN_ArraySetAt,
            DTWAIN_ArraySetAtANSIStringFunc: DTWAIN_ArraySetAtANSIString,
            DTWAIN_ArraySetAtFloatFunc: DTWAIN_ArraySetAtFloat,
            DTWAIN_ArraySetAtFloatStringFunc: DTWAIN_ArraySetAtFloatString,
            DTWAIN_ArraySetAtFloatStringAFunc: DTWAIN_ArraySetAtFloatStringA,
            DTWAIN_ArraySetAtFloatStringWFunc: DTWAIN_ArraySetAtFloatStringW,
            DTWAIN_ArraySetAtFrameFunc: DTWAIN_ArraySetAtFrame,
            DTWAIN_ArraySetAtFrameExFunc: DTWAIN_ArraySetAtFrameEx,
            DTWAIN_ArraySetAtFrameStringFunc: DTWAIN_ArraySetAtFrameString,
            DTWAIN_ArraySetAtFrameStringAFunc: DTWAIN_ArraySetAtFrameStringA,
            DTWAIN_ArraySetAtFrameStringWFunc: DTWAIN_ArraySetAtFrameStringW,
            DTWAIN_ArraySetAtLongFunc: DTWAIN_ArraySetAtLong,
            DTWAIN_ArraySetAtLong64Func: DTWAIN_ArraySetAtLong64,
            DTWAIN_ArraySetAtStringFunc: DTWAIN_ArraySetAtString,
            DTWAIN_ArraySetAtStringAFunc: DTWAIN_ArraySetAtStringA,
            DTWAIN_ArraySetAtStringWFunc: DTWAIN_ArraySetAtStringW,
            DTWAIN_ArraySetAtWideStringFunc: DTWAIN_ArraySetAtWideString,
            DTWAIN_ArrayStringToFloatFunc: DTWAIN_ArrayStringToFloat,
            DTWAIN_ArrayWideStringToFloatFunc: DTWAIN_ArrayWideStringToFloat,
            DTWAIN_CallCallbackFunc: DTWAIN_CallCallback,
            DTWAIN_CallCallback64Func: DTWAIN_CallCallback64,
            DTWAIN_CallDSMProcFunc: DTWAIN_CallDSMProc,
            DTWAIN_CheckHandlesFunc: DTWAIN_CheckHandles,
            DTWAIN_ClearBuffersFunc: DTWAIN_ClearBuffers,
            DTWAIN_ClearErrorBufferFunc: DTWAIN_ClearErrorBuffer,
            DTWAIN_ClearPDFTextFunc: DTWAIN_ClearPDFText,
            DTWAIN_ClearPageFunc: DTWAIN_ClearPage,
            DTWAIN_CloseSourceFunc: DTWAIN_CloseSource,
            DTWAIN_CloseSourceUIFunc: DTWAIN_CloseSourceUI,
            DTWAIN_ConvertDIBToBitmapFunc: DTWAIN_ConvertDIBToBitmap,
            DTWAIN_ConvertDIBToFullBitmapFunc: DTWAIN_ConvertDIBToFullBitmap,
            DTWAIN_ConvertToAPIStringFunc: DTWAIN_ConvertToAPIString,
            DTWAIN_ConvertToAPIStringAFunc: DTWAIN_ConvertToAPIStringA,
            DTWAIN_ConvertToAPIStringExFunc: DTWAIN_ConvertToAPIStringEx,
            DTWAIN_ConvertToAPIStringExAFunc: DTWAIN_ConvertToAPIStringExA,
            DTWAIN_ConvertToAPIStringExWFunc: DTWAIN_ConvertToAPIStringExW,
            DTWAIN_ConvertToAPIStringWFunc: DTWAIN_ConvertToAPIStringW,
            DTWAIN_CreateAcquisitionArrayFunc: DTWAIN_CreateAcquisitionArray,
            DTWAIN_CreatePDFTextElementFunc: DTWAIN_CreatePDFTextElement,
            DTWAIN_DeleteDIBFunc: DTWAIN_DeleteDIB,
            DTWAIN_DestroyAcquisitionArrayFunc: DTWAIN_DestroyAcquisitionArray,
            DTWAIN_DestroyPDFTextElementFunc: DTWAIN_DestroyPDFTextElement,
            DTWAIN_DisableAppWindowFunc: DTWAIN_DisableAppWindow,
            DTWAIN_EnableAutoBorderDetectFunc: DTWAIN_EnableAutoBorderDetect,
            DTWAIN_EnableAutoBrightFunc: DTWAIN_EnableAutoBright,
            DTWAIN_EnableAutoDeskewFunc: DTWAIN_EnableAutoDeskew,
            DTWAIN_EnableAutoFeedFunc: DTWAIN_EnableAutoFeed,
            DTWAIN_EnableAutoRotateFunc: DTWAIN_EnableAutoRotate,
            DTWAIN_EnableAutoScanFunc: DTWAIN_EnableAutoScan,
            DTWAIN_EnableAutomaticSenseMediumFunc: DTWAIN_EnableAutomaticSenseMedium,
            DTWAIN_EnableDuplexFunc: DTWAIN_EnableDuplex,
            DTWAIN_EnableFeederFunc: DTWAIN_EnableFeeder,
            DTWAIN_EnableIndicatorFunc: DTWAIN_EnableIndicator,
            DTWAIN_EnableJobFileHandlingFunc: DTWAIN_EnableJobFileHandling,
            DTWAIN_EnableLampFunc: DTWAIN_EnableLamp,
            DTWAIN_EnableMsgNotifyFunc: DTWAIN_EnableMsgNotify,
            DTWAIN_EnablePatchDetectFunc: DTWAIN_EnablePatchDetect,
            DTWAIN_EnablePeekMessageLoopFunc: DTWAIN_EnablePeekMessageLoop,
            DTWAIN_EnablePrinterFunc: DTWAIN_EnablePrinter,
            DTWAIN_EnableThumbnailFunc: DTWAIN_EnableThumbnail,
            DTWAIN_EnableTripletsNotifyFunc: DTWAIN_EnableTripletsNotify,
            DTWAIN_EndThreadFunc: DTWAIN_EndThread,
            DTWAIN_EndTwainSessionFunc: DTWAIN_EndTwainSession,
            DTWAIN_EnumAlarmVolumesFunc: DTWAIN_EnumAlarmVolumes,
            DTWAIN_EnumAlarmVolumesExFunc: DTWAIN_EnumAlarmVolumesEx,
            DTWAIN_EnumAlarmsFunc: DTWAIN_EnumAlarms,
            DTWAIN_EnumAlarmsExFunc: DTWAIN_EnumAlarmsEx,
            DTWAIN_EnumAudioXferMechsFunc: DTWAIN_EnumAudioXferMechs,
            DTWAIN_EnumAudioXferMechsExFunc: DTWAIN_EnumAudioXferMechsEx,
            DTWAIN_EnumAutoFeedValuesFunc: DTWAIN_EnumAutoFeedValues,
            DTWAIN_EnumAutoFeedValuesExFunc: DTWAIN_EnumAutoFeedValuesEx,
            DTWAIN_EnumAutomaticCapturesFunc: DTWAIN_EnumAutomaticCaptures,
            DTWAIN_EnumAutomaticCapturesExFunc: DTWAIN_EnumAutomaticCapturesEx,
            DTWAIN_EnumAutomaticSenseMediumFunc: DTWAIN_EnumAutomaticSenseMedium,
            DTWAIN_EnumAutomaticSenseMediumExFunc: DTWAIN_EnumAutomaticSenseMediumEx,
            DTWAIN_EnumBitDepthsFunc: DTWAIN_EnumBitDepths,
            DTWAIN_EnumBitDepthsExFunc: DTWAIN_EnumBitDepthsEx,
            DTWAIN_EnumBitDepthsEx2Func: DTWAIN_EnumBitDepthsEx2,
            DTWAIN_EnumBottomCamerasFunc: DTWAIN_EnumBottomCameras,
            DTWAIN_EnumBottomCamerasExFunc: DTWAIN_EnumBottomCamerasEx,
            DTWAIN_EnumBrightnessValuesFunc: DTWAIN_EnumBrightnessValues,
            DTWAIN_EnumBrightnessValuesExFunc: DTWAIN_EnumBrightnessValuesEx,
            DTWAIN_EnumCamerasFunc: DTWAIN_EnumCameras,
            DTWAIN_EnumCamerasExFunc: DTWAIN_EnumCamerasEx,
            DTWAIN_EnumCamerasEx2Func: DTWAIN_EnumCamerasEx2,
            DTWAIN_EnumCamerasEx3Func: DTWAIN_EnumCamerasEx3,
            DTWAIN_EnumCompressionTypesFunc: DTWAIN_EnumCompressionTypes,
            DTWAIN_EnumCompressionTypesExFunc: DTWAIN_EnumCompressionTypesEx,
            DTWAIN_EnumCompressionTypesEx2Func: DTWAIN_EnumCompressionTypesEx2,
            DTWAIN_EnumContrastValuesFunc: DTWAIN_EnumContrastValues,
            DTWAIN_EnumContrastValuesExFunc: DTWAIN_EnumContrastValuesEx,
            DTWAIN_EnumCustomCapsFunc: DTWAIN_EnumCustomCaps,
            DTWAIN_EnumCustomCapsEx2Func: DTWAIN_EnumCustomCapsEx2,
            DTWAIN_EnumDoubleFeedDetectLengthsFunc: DTWAIN_EnumDoubleFeedDetectLengths,
            DTWAIN_EnumDoubleFeedDetectLengthsExFunc: DTWAIN_EnumDoubleFeedDetectLengthsEx,
            DTWAIN_EnumDoubleFeedDetectValuesFunc: DTWAIN_EnumDoubleFeedDetectValues,
            DTWAIN_EnumDoubleFeedDetectValuesExFunc: DTWAIN_EnumDoubleFeedDetectValuesEx,
            DTWAIN_EnumExtImageInfoTypesFunc: DTWAIN_EnumExtImageInfoTypes,
            DTWAIN_EnumExtImageInfoTypesExFunc: DTWAIN_EnumExtImageInfoTypesEx,
            DTWAIN_EnumExtendedCapsFunc: DTWAIN_EnumExtendedCaps,
            DTWAIN_EnumExtendedCapsExFunc: DTWAIN_EnumExtendedCapsEx,
            DTWAIN_EnumExtendedCapsEx2Func: DTWAIN_EnumExtendedCapsEx2,
            DTWAIN_EnumFileTypeBitsPerPixelFunc: DTWAIN_EnumFileTypeBitsPerPixel,
            DTWAIN_EnumFileXferFormatsFunc: DTWAIN_EnumFileXferFormats,
            DTWAIN_EnumFileXferFormatsExFunc: DTWAIN_EnumFileXferFormatsEx,
            DTWAIN_EnumHalftonesFunc: DTWAIN_EnumHalftones,
            DTWAIN_EnumHalftonesExFunc: DTWAIN_EnumHalftonesEx,
            DTWAIN_EnumHighlightValuesFunc: DTWAIN_EnumHighlightValues,
            DTWAIN_EnumHighlightValuesExFunc: DTWAIN_EnumHighlightValuesEx,
            DTWAIN_EnumJobControlsFunc: DTWAIN_EnumJobControls,
            DTWAIN_EnumJobControlsExFunc: DTWAIN_EnumJobControlsEx,
            DTWAIN_EnumLightPathsFunc: DTWAIN_EnumLightPaths,
            DTWAIN_EnumLightPathsExFunc: DTWAIN_EnumLightPathsEx,
            DTWAIN_EnumLightSourcesFunc: DTWAIN_EnumLightSources,
            DTWAIN_EnumLightSourcesExFunc: DTWAIN_EnumLightSourcesEx,
            DTWAIN_EnumMaxBuffersFunc: DTWAIN_EnumMaxBuffers,
            DTWAIN_EnumMaxBuffersExFunc: DTWAIN_EnumMaxBuffersEx,
            DTWAIN_EnumNoiseFiltersFunc: DTWAIN_EnumNoiseFilters,
            DTWAIN_EnumNoiseFiltersExFunc: DTWAIN_EnumNoiseFiltersEx,
            DTWAIN_EnumOCRInterfacesFunc: DTWAIN_EnumOCRInterfaces,
            DTWAIN_EnumOCRSupportedCapsFunc: DTWAIN_EnumOCRSupportedCaps,
            DTWAIN_EnumOrientationsFunc: DTWAIN_EnumOrientations,
            DTWAIN_EnumOrientationsExFunc: DTWAIN_EnumOrientationsEx,
            DTWAIN_EnumOverscanValuesFunc: DTWAIN_EnumOverscanValues,
            DTWAIN_EnumOverscanValuesExFunc: DTWAIN_EnumOverscanValuesEx,
            DTWAIN_EnumPaperSizesFunc: DTWAIN_EnumPaperSizes,
            DTWAIN_EnumPaperSizesExFunc: DTWAIN_EnumPaperSizesEx,
            DTWAIN_EnumPatchCodesFunc: DTWAIN_EnumPatchCodes,
            DTWAIN_EnumPatchCodesExFunc: DTWAIN_EnumPatchCodesEx,
            DTWAIN_EnumPatchMaxPrioritiesFunc: DTWAIN_EnumPatchMaxPriorities,
            DTWAIN_EnumPatchMaxPrioritiesExFunc: DTWAIN_EnumPatchMaxPrioritiesEx,
            DTWAIN_EnumPatchMaxRetriesFunc: DTWAIN_EnumPatchMaxRetries,
            DTWAIN_EnumPatchMaxRetriesExFunc: DTWAIN_EnumPatchMaxRetriesEx,
            DTWAIN_EnumPatchPrioritiesFunc: DTWAIN_EnumPatchPriorities,
            DTWAIN_EnumPatchPrioritiesExFunc: DTWAIN_EnumPatchPrioritiesEx,
            DTWAIN_EnumPatchSearchModesFunc: DTWAIN_EnumPatchSearchModes,
            DTWAIN_EnumPatchSearchModesExFunc: DTWAIN_EnumPatchSearchModesEx,
            DTWAIN_EnumPatchTimeOutValuesFunc: DTWAIN_EnumPatchTimeOutValues,
            DTWAIN_EnumPatchTimeOutValuesExFunc: DTWAIN_EnumPatchTimeOutValuesEx,
            DTWAIN_EnumPixelTypesFunc: DTWAIN_EnumPixelTypes,
            DTWAIN_EnumPixelTypesExFunc: DTWAIN_EnumPixelTypesEx,
            DTWAIN_EnumPrinterStringModesFunc: DTWAIN_EnumPrinterStringModes,
            DTWAIN_EnumPrinterStringModesExFunc: DTWAIN_EnumPrinterStringModesEx,
            DTWAIN_EnumResolutionValuesFunc: DTWAIN_EnumResolutionValues,
            DTWAIN_EnumResolutionValuesExFunc: DTWAIN_EnumResolutionValuesEx,
            DTWAIN_EnumShadowValuesFunc: DTWAIN_EnumShadowValues,
            DTWAIN_EnumShadowValuesExFunc: DTWAIN_EnumShadowValuesEx,
            DTWAIN_EnumSourceUnitsFunc: DTWAIN_EnumSourceUnits,
            DTWAIN_EnumSourceUnitsExFunc: DTWAIN_EnumSourceUnitsEx,
            DTWAIN_EnumSourceValuesFunc: DTWAIN_EnumSourceValues,
            DTWAIN_EnumSourceValuesAFunc: DTWAIN_EnumSourceValuesA,
            DTWAIN_EnumSourceValuesWFunc: DTWAIN_EnumSourceValuesW,
            DTWAIN_EnumSourcesFunc: DTWAIN_EnumSources,
            DTWAIN_EnumSourcesExFunc: DTWAIN_EnumSourcesEx,
            DTWAIN_EnumSupportedCapsFunc: DTWAIN_EnumSupportedCaps,
            DTWAIN_EnumSupportedCapsExFunc: DTWAIN_EnumSupportedCapsEx,
            DTWAIN_EnumSupportedCapsEx2Func: DTWAIN_EnumSupportedCapsEx2,
            DTWAIN_EnumSupportedExtImageInfoFunc: DTWAIN_EnumSupportedExtImageInfo,
            DTWAIN_EnumSupportedExtImageInfoExFunc: DTWAIN_EnumSupportedExtImageInfoEx,
            DTWAIN_EnumSupportedFileTypesFunc: DTWAIN_EnumSupportedFileTypes,
            DTWAIN_EnumSupportedMultiPageFileTypesFunc: DTWAIN_EnumSupportedMultiPageFileTypes,
            DTWAIN_EnumSupportedSinglePageFileTypesFunc: DTWAIN_EnumSupportedSinglePageFileTypes,
            DTWAIN_EnumThresholdValuesFunc: DTWAIN_EnumThresholdValues,
            DTWAIN_EnumThresholdValuesExFunc: DTWAIN_EnumThresholdValuesEx,
            DTWAIN_EnumTopCamerasFunc: DTWAIN_EnumTopCameras,
            DTWAIN_EnumTopCamerasExFunc: DTWAIN_EnumTopCamerasEx,
            DTWAIN_EnumTwainPrintersFunc: DTWAIN_EnumTwainPrinters,
            DTWAIN_EnumTwainPrintersArrayFunc: DTWAIN_EnumTwainPrintersArray,
            DTWAIN_EnumTwainPrintersArrayExFunc: DTWAIN_EnumTwainPrintersArrayEx,
            DTWAIN_EnumTwainPrintersExFunc: DTWAIN_EnumTwainPrintersEx,
            DTWAIN_EnumXResolutionValuesFunc: DTWAIN_EnumXResolutionValues,
            DTWAIN_EnumXResolutionValuesExFunc: DTWAIN_EnumXResolutionValuesEx,
            DTWAIN_EnumYResolutionValuesFunc: DTWAIN_EnumYResolutionValues,
            DTWAIN_EnumYResolutionValuesExFunc: DTWAIN_EnumYResolutionValuesEx,
            DTWAIN_ExecuteOCRFunc: DTWAIN_ExecuteOCR,
            DTWAIN_ExecuteOCRAFunc: DTWAIN_ExecuteOCRA,
            DTWAIN_ExecuteOCRWFunc: DTWAIN_ExecuteOCRW,
            DTWAIN_FeedPageFunc: DTWAIN_FeedPage,
            DTWAIN_FlipBitmapFunc: DTWAIN_FlipBitmap,
            DTWAIN_FlushAcquiredPagesFunc: DTWAIN_FlushAcquiredPages,
            DTWAIN_ForceAcquireBitDepthFunc: DTWAIN_ForceAcquireBitDepth,
            DTWAIN_ForceScanOnNoUIFunc: DTWAIN_ForceScanOnNoUI,
            DTWAIN_FrameCreateFunc: DTWAIN_FrameCreate,
            DTWAIN_FrameCreateStringFunc: DTWAIN_FrameCreateString,
            DTWAIN_FrameCreateStringAFunc: DTWAIN_FrameCreateStringA,
            DTWAIN_FrameCreateStringWFunc: DTWAIN_FrameCreateStringW,
            DTWAIN_FrameDestroyFunc: DTWAIN_FrameDestroy,
            DTWAIN_FrameGetAllFunc: DTWAIN_FrameGetAll,
            DTWAIN_FrameGetAllStringFunc: DTWAIN_FrameGetAllString,
            DTWAIN_FrameGetAllStringAFunc: DTWAIN_FrameGetAllStringA,
            DTWAIN_FrameGetAllStringWFunc: DTWAIN_FrameGetAllStringW,
            DTWAIN_FrameGetValueFunc: DTWAIN_FrameGetValue,
            DTWAIN_FrameGetValueStringFunc: DTWAIN_FrameGetValueString,
            DTWAIN_FrameGetValueStringAFunc: DTWAIN_FrameGetValueStringA,
            DTWAIN_FrameGetValueStringWFunc: DTWAIN_FrameGetValueStringW,
            DTWAIN_FrameIsValidFunc: DTWAIN_FrameIsValid,
            DTWAIN_FrameSetAllFunc: DTWAIN_FrameSetAll,
            DTWAIN_FrameSetAllStringFunc: DTWAIN_FrameSetAllString,
            DTWAIN_FrameSetAllStringAFunc: DTWAIN_FrameSetAllStringA,
            DTWAIN_FrameSetAllStringWFunc: DTWAIN_FrameSetAllStringW,
            DTWAIN_FrameSetValueFunc: DTWAIN_FrameSetValue,
            DTWAIN_FrameSetValueStringFunc: DTWAIN_FrameSetValueString,
            DTWAIN_FrameSetValueStringAFunc: DTWAIN_FrameSetValueStringA,
            DTWAIN_FrameSetValueStringWFunc: DTWAIN_FrameSetValueStringW,
            DTWAIN_FreeExtImageInfoFunc: DTWAIN_FreeExtImageInfo,
            DTWAIN_FreeMemoryFunc: DTWAIN_FreeMemory,
            DTWAIN_FreeMemoryExFunc: DTWAIN_FreeMemoryEx,
            DTWAIN_GetAPIHandleStatusFunc: DTWAIN_GetAPIHandleStatus,
            DTWAIN_GetAcquireAreaFunc: DTWAIN_GetAcquireArea,
            DTWAIN_GetAcquireArea2Func: DTWAIN_GetAcquireArea2,
            DTWAIN_GetAcquireArea2StringFunc: DTWAIN_GetAcquireArea2String,
            DTWAIN_GetAcquireArea2StringAFunc: DTWAIN_GetAcquireArea2StringA,
            DTWAIN_GetAcquireArea2StringWFunc: DTWAIN_GetAcquireArea2StringW,
            DTWAIN_GetAcquireAreaExFunc: DTWAIN_GetAcquireAreaEx,
            DTWAIN_GetAcquireMetricsFunc: DTWAIN_GetAcquireMetrics,
            DTWAIN_GetAcquireStripBufferFunc: DTWAIN_GetAcquireStripBuffer,
            DTWAIN_GetAcquireStripDataFunc: DTWAIN_GetAcquireStripData,
            DTWAIN_GetAcquireStripSizesFunc: DTWAIN_GetAcquireStripSizes,
            DTWAIN_GetAcquiredImageFunc: DTWAIN_GetAcquiredImage,
            DTWAIN_GetAcquiredImageArrayFunc: DTWAIN_GetAcquiredImageArray,
            DTWAIN_GetActiveDSMPathFunc: DTWAIN_GetActiveDSMPath,
            DTWAIN_GetActiveDSMPathAFunc: DTWAIN_GetActiveDSMPathA,
            DTWAIN_GetActiveDSMPathWFunc: DTWAIN_GetActiveDSMPathW,
            DTWAIN_GetActiveDSMVersionInfoFunc: DTWAIN_GetActiveDSMVersionInfo,
            DTWAIN_GetActiveDSMVersionInfoAFunc: DTWAIN_GetActiveDSMVersionInfoA,
            DTWAIN_GetActiveDSMVersionInfoWFunc: DTWAIN_GetActiveDSMVersionInfoW,
            DTWAIN_GetAlarmVolumeFunc: DTWAIN_GetAlarmVolume,
            DTWAIN_GetAllSourceDibsFunc: DTWAIN_GetAllSourceDibs,
            DTWAIN_GetAppInfoFunc: DTWAIN_GetAppInfo,
            DTWAIN_GetAppInfoAFunc: DTWAIN_GetAppInfoA,
            DTWAIN_GetAppInfoWFunc: DTWAIN_GetAppInfoW,
            DTWAIN_GetAuthorFunc: DTWAIN_GetAuthor,
            DTWAIN_GetAuthorAFunc: DTWAIN_GetAuthorA,
            DTWAIN_GetAuthorWFunc: DTWAIN_GetAuthorW,
            DTWAIN_GetBatteryMinutesFunc: DTWAIN_GetBatteryMinutes,
            DTWAIN_GetBatteryPercentFunc: DTWAIN_GetBatteryPercent,
            DTWAIN_GetBitDepthFunc: DTWAIN_GetBitDepth,
            DTWAIN_GetBlankPageAutoDetectionFunc: DTWAIN_GetBlankPageAutoDetection,
            DTWAIN_GetBrightnessFunc: DTWAIN_GetBrightness,
            DTWAIN_GetBrightnessStringFunc: DTWAIN_GetBrightnessString,
            DTWAIN_GetBrightnessStringAFunc: DTWAIN_GetBrightnessStringA,
            DTWAIN_GetBrightnessStringWFunc: DTWAIN_GetBrightnessStringW,
            DTWAIN_GetBufferedTransferInfoFunc: DTWAIN_GetBufferedTransferInfo,
            DTWAIN_GetCallbackFunc: DTWAIN_GetCallback,
            DTWAIN_GetCallback64Func: DTWAIN_GetCallback64,
            DTWAIN_GetCapArrayTypeFunc: DTWAIN_GetCapArrayType,
            DTWAIN_GetCapContainerFunc: DTWAIN_GetCapContainer,
            DTWAIN_GetCapContainerExFunc: DTWAIN_GetCapContainerEx,
            DTWAIN_GetCapContainerEx2Func: DTWAIN_GetCapContainerEx2,
            DTWAIN_GetCapDataTypeFunc: DTWAIN_GetCapDataType,
            DTWAIN_GetCapFromNameFunc: DTWAIN_GetCapFromName,
            DTWAIN_GetCapFromNameAFunc: DTWAIN_GetCapFromNameA,
            DTWAIN_GetCapFromNameWFunc: DTWAIN_GetCapFromNameW,
            DTWAIN_GetCapOperationsFunc: DTWAIN_GetCapOperations,
            DTWAIN_GetCapValuesFunc: DTWAIN_GetCapValues,
            DTWAIN_GetCapValuesExFunc: DTWAIN_GetCapValuesEx,
            DTWAIN_GetCapValuesEx2Func: DTWAIN_GetCapValuesEx2,
            DTWAIN_GetCaptionFunc: DTWAIN_GetCaption,
            DTWAIN_GetCaptionAFunc: DTWAIN_GetCaptionA,
            DTWAIN_GetCaptionWFunc: DTWAIN_GetCaptionW,
            DTWAIN_GetCompressionSizeFunc: DTWAIN_GetCompressionSize,
            DTWAIN_GetCompressionTypeFunc: DTWAIN_GetCompressionType,
            DTWAIN_GetConditionCodeStringFunc: DTWAIN_GetConditionCodeString,
            DTWAIN_GetConditionCodeStringAFunc: DTWAIN_GetConditionCodeStringA,
            DTWAIN_GetConditionCodeStringWFunc: DTWAIN_GetConditionCodeStringW,
            DTWAIN_GetContrastFunc: DTWAIN_GetContrast,
            DTWAIN_GetContrastStringFunc: DTWAIN_GetContrastString,
            DTWAIN_GetContrastStringAFunc: DTWAIN_GetContrastStringA,
            DTWAIN_GetContrastStringWFunc: DTWAIN_GetContrastStringW,
            DTWAIN_GetCountryFunc: DTWAIN_GetCountry,
            DTWAIN_GetCurrentAcquiredImageFunc: DTWAIN_GetCurrentAcquiredImage,
            DTWAIN_GetCurrentFileNameFunc: DTWAIN_GetCurrentFileName,
            DTWAIN_GetCurrentFileNameAFunc: DTWAIN_GetCurrentFileNameA,
            DTWAIN_GetCurrentFileNameWFunc: DTWAIN_GetCurrentFileNameW,
            DTWAIN_GetCurrentPageNumFunc: DTWAIN_GetCurrentPageNum,
            DTWAIN_GetCurrentRetryCountFunc: DTWAIN_GetCurrentRetryCount,
            DTWAIN_GetCurrentTwainTripletFunc: DTWAIN_GetCurrentTwainTriplet,
            DTWAIN_GetCustomDSDataFunc: DTWAIN_GetCustomDSData,
            DTWAIN_GetDSMFullNameFunc: DTWAIN_GetDSMFullName,
            DTWAIN_GetDSMFullNameAFunc: DTWAIN_GetDSMFullNameA,
            DTWAIN_GetDSMFullNameWFunc: DTWAIN_GetDSMFullNameW,
            DTWAIN_GetDSMSearchOrderFunc: DTWAIN_GetDSMSearchOrder,
            DTWAIN_GetDTWAINHandleFunc: DTWAIN_GetDTWAINHandle,
            DTWAIN_GetDeviceEventFunc: DTWAIN_GetDeviceEvent,
            DTWAIN_GetDeviceEventExFunc: DTWAIN_GetDeviceEventEx,
            DTWAIN_GetDeviceEventInfoFunc: DTWAIN_GetDeviceEventInfo,
            DTWAIN_GetDeviceNotificationsFunc: DTWAIN_GetDeviceNotifications,
            DTWAIN_GetDeviceTimeDateFunc: DTWAIN_GetDeviceTimeDate,
            DTWAIN_GetDeviceTimeDateAFunc: DTWAIN_GetDeviceTimeDateA,
            DTWAIN_GetDeviceTimeDateWFunc: DTWAIN_GetDeviceTimeDateW,
            DTWAIN_GetDoubleFeedDetectLengthFunc: DTWAIN_GetDoubleFeedDetectLength,
            DTWAIN_GetDoubleFeedDetectValuesFunc: DTWAIN_GetDoubleFeedDetectValues,
            DTWAIN_GetDuplexTypeFunc: DTWAIN_GetDuplexType,
            DTWAIN_GetErrorBufferFunc: DTWAIN_GetErrorBuffer,
            DTWAIN_GetErrorBufferThresholdFunc: DTWAIN_GetErrorBufferThreshold,
            DTWAIN_GetErrorCallbackFunc: DTWAIN_GetErrorCallback,
            DTWAIN_GetErrorCallback64Func: DTWAIN_GetErrorCallback64,
            DTWAIN_GetErrorStringFunc: DTWAIN_GetErrorString,
            DTWAIN_GetErrorStringAFunc: DTWAIN_GetErrorStringA,
            DTWAIN_GetErrorStringWFunc: DTWAIN_GetErrorStringW,
            DTWAIN_GetExtCapFromNameFunc: DTWAIN_GetExtCapFromName,
            DTWAIN_GetExtCapFromNameAFunc: DTWAIN_GetExtCapFromNameA,
            DTWAIN_GetExtCapFromNameWFunc: DTWAIN_GetExtCapFromNameW,
            DTWAIN_GetExtImageInfoFunc: DTWAIN_GetExtImageInfo,
            DTWAIN_GetExtImageInfoDataFunc: DTWAIN_GetExtImageInfoData,
            DTWAIN_GetExtImageInfoDataExFunc: DTWAIN_GetExtImageInfoDataEx,
            DTWAIN_GetExtImageInfoItemFunc: DTWAIN_GetExtImageInfoItem,
            DTWAIN_GetExtImageInfoItemExFunc: DTWAIN_GetExtImageInfoItemEx,
            DTWAIN_GetExtNameFromCapFunc: DTWAIN_GetExtNameFromCap,
            DTWAIN_GetExtNameFromCapAFunc: DTWAIN_GetExtNameFromCapA,
            DTWAIN_GetExtNameFromCapWFunc: DTWAIN_GetExtNameFromCapW,
            DTWAIN_GetFeederAlignmentFunc: DTWAIN_GetFeederAlignment,
            DTWAIN_GetFeederFuncsFunc: DTWAIN_GetFeederFuncs,
            DTWAIN_GetFeederOrderFunc: DTWAIN_GetFeederOrder,
            DTWAIN_GetFeederWaitTimeFunc: DTWAIN_GetFeederWaitTime,
            DTWAIN_GetFileCompressionTypeFunc: DTWAIN_GetFileCompressionType,
            DTWAIN_GetFileTypeExtensionsFunc: DTWAIN_GetFileTypeExtensions,
            DTWAIN_GetFileTypeExtensionsAFunc: DTWAIN_GetFileTypeExtensionsA,
            DTWAIN_GetFileTypeExtensionsWFunc: DTWAIN_GetFileTypeExtensionsW,
            DTWAIN_GetFileTypeNameFunc: DTWAIN_GetFileTypeName,
            DTWAIN_GetFileTypeNameAFunc: DTWAIN_GetFileTypeNameA,
            DTWAIN_GetFileTypeNameWFunc: DTWAIN_GetFileTypeNameW,
            DTWAIN_GetHalftoneFunc: DTWAIN_GetHalftone,
            DTWAIN_GetHalftoneAFunc: DTWAIN_GetHalftoneA,
            DTWAIN_GetHalftoneWFunc: DTWAIN_GetHalftoneW,
            DTWAIN_GetHighlightFunc: DTWAIN_GetHighlight,
            DTWAIN_GetHighlightStringFunc: DTWAIN_GetHighlightString,
            DTWAIN_GetHighlightStringAFunc: DTWAIN_GetHighlightStringA,
            DTWAIN_GetHighlightStringWFunc: DTWAIN_GetHighlightStringW,
            DTWAIN_GetImageInfoFunc: DTWAIN_GetImageInfo,
            DTWAIN_GetImageInfoStringFunc: DTWAIN_GetImageInfoString,
            DTWAIN_GetImageInfoStringAFunc: DTWAIN_GetImageInfoStringA,
            DTWAIN_GetImageInfoStringWFunc: DTWAIN_GetImageInfoStringW,
            DTWAIN_GetJobControlFunc: DTWAIN_GetJobControl,
            DTWAIN_GetJpegValuesFunc: DTWAIN_GetJpegValues,
            DTWAIN_GetJpegXRValuesFunc: DTWAIN_GetJpegXRValues,
            DTWAIN_GetLanguageFunc: DTWAIN_GetLanguage,
            DTWAIN_GetLastErrorFunc: DTWAIN_GetLastError,
            DTWAIN_GetLibraryPathFunc: DTWAIN_GetLibraryPath,
            DTWAIN_GetLibraryPathAFunc: DTWAIN_GetLibraryPathA,
            DTWAIN_GetLibraryPathWFunc: DTWAIN_GetLibraryPathW,
            DTWAIN_GetLightPathFunc: DTWAIN_GetLightPath,
            DTWAIN_GetLightSourceFunc: DTWAIN_GetLightSource,
            DTWAIN_GetLightSourcesFunc: DTWAIN_GetLightSources,
            DTWAIN_GetLoggerCallbackFunc: DTWAIN_GetLoggerCallback,
            DTWAIN_GetLoggerCallbackAFunc: DTWAIN_GetLoggerCallbackA,
            DTWAIN_GetLoggerCallbackWFunc: DTWAIN_GetLoggerCallbackW,
            DTWAIN_GetManualDuplexCountFunc: DTWAIN_GetManualDuplexCount,
            DTWAIN_GetMaxAcquisitionsFunc: DTWAIN_GetMaxAcquisitions,
            DTWAIN_GetMaxBuffersFunc: DTWAIN_GetMaxBuffers,
            DTWAIN_GetMaxPagesToAcquireFunc: DTWAIN_GetMaxPagesToAcquire,
            DTWAIN_GetMaxRetryAttemptsFunc: DTWAIN_GetMaxRetryAttempts,
            DTWAIN_GetNameFromCapFunc: DTWAIN_GetNameFromCap,
            DTWAIN_GetNameFromCapAFunc: DTWAIN_GetNameFromCapA,
            DTWAIN_GetNameFromCapWFunc: DTWAIN_GetNameFromCapW,
            DTWAIN_GetNoiseFilterFunc: DTWAIN_GetNoiseFilter,
            DTWAIN_GetNumAcquiredImagesFunc: DTWAIN_GetNumAcquiredImages,
            DTWAIN_GetNumAcquisitionsFunc: DTWAIN_GetNumAcquisitions,
            DTWAIN_GetOCRCapValuesFunc: DTWAIN_GetOCRCapValues,
            DTWAIN_GetOCRErrorStringFunc: DTWAIN_GetOCRErrorString,
            DTWAIN_GetOCRErrorStringAFunc: DTWAIN_GetOCRErrorStringA,
            DTWAIN_GetOCRErrorStringWFunc: DTWAIN_GetOCRErrorStringW,
            DTWAIN_GetOCRLastErrorFunc: DTWAIN_GetOCRLastError,
            DTWAIN_GetOCRMajorMinorVersionFunc: DTWAIN_GetOCRMajorMinorVersion,
            DTWAIN_GetOCRManufacturerFunc: DTWAIN_GetOCRManufacturer,
            DTWAIN_GetOCRManufacturerAFunc: DTWAIN_GetOCRManufacturerA,
            DTWAIN_GetOCRManufacturerWFunc: DTWAIN_GetOCRManufacturerW,
            DTWAIN_GetOCRProductFamilyFunc: DTWAIN_GetOCRProductFamily,
            DTWAIN_GetOCRProductFamilyAFunc: DTWAIN_GetOCRProductFamilyA,
            DTWAIN_GetOCRProductFamilyWFunc: DTWAIN_GetOCRProductFamilyW,
            DTWAIN_GetOCRProductNameFunc: DTWAIN_GetOCRProductName,
            DTWAIN_GetOCRProductNameAFunc: DTWAIN_GetOCRProductNameA,
            DTWAIN_GetOCRProductNameWFunc: DTWAIN_GetOCRProductNameW,
            DTWAIN_GetOCRTextFunc: DTWAIN_GetOCRText,
            DTWAIN_GetOCRTextAFunc: DTWAIN_GetOCRTextA,
            DTWAIN_GetOCRTextInfoFloatFunc: DTWAIN_GetOCRTextInfoFloat,
            DTWAIN_GetOCRTextInfoFloatExFunc: DTWAIN_GetOCRTextInfoFloatEx,
            DTWAIN_GetOCRTextInfoHandleFunc: DTWAIN_GetOCRTextInfoHandle,
            DTWAIN_GetOCRTextInfoLongFunc: DTWAIN_GetOCRTextInfoLong,
            DTWAIN_GetOCRTextInfoLongExFunc: DTWAIN_GetOCRTextInfoLongEx,
            DTWAIN_GetOCRTextWFunc: DTWAIN_GetOCRTextW,
            DTWAIN_GetOCRVersionInfoFunc: DTWAIN_GetOCRVersionInfo,
            DTWAIN_GetOCRVersionInfoAFunc: DTWAIN_GetOCRVersionInfoA,
            DTWAIN_GetOCRVersionInfoWFunc: DTWAIN_GetOCRVersionInfoW,
            DTWAIN_GetOrientationFunc: DTWAIN_GetOrientation,
            DTWAIN_GetOverscanFunc: DTWAIN_GetOverscan,
            DTWAIN_GetPDFTextElementFloatFunc: DTWAIN_GetPDFTextElementFloat,
            DTWAIN_GetPDFTextElementLongFunc: DTWAIN_GetPDFTextElementLong,
            DTWAIN_GetPDFTextElementStringFunc: DTWAIN_GetPDFTextElementString,
            DTWAIN_GetPDFTextElementStringAFunc: DTWAIN_GetPDFTextElementStringA,
            DTWAIN_GetPDFTextElementStringWFunc: DTWAIN_GetPDFTextElementStringW,
            DTWAIN_GetPDFType1FontNameFunc: DTWAIN_GetPDFType1FontName,
            DTWAIN_GetPDFType1FontNameAFunc: DTWAIN_GetPDFType1FontNameA,
            DTWAIN_GetPDFType1FontNameWFunc: DTWAIN_GetPDFType1FontNameW,
            DTWAIN_GetPaperSizeFunc: DTWAIN_GetPaperSize,
            DTWAIN_GetPaperSizeNameFunc: DTWAIN_GetPaperSizeName,
            DTWAIN_GetPaperSizeNameAFunc: DTWAIN_GetPaperSizeNameA,
            DTWAIN_GetPaperSizeNameWFunc: DTWAIN_GetPaperSizeNameW,
            DTWAIN_GetPatchMaxPrioritiesFunc: DTWAIN_GetPatchMaxPriorities,
            DTWAIN_GetPatchMaxRetriesFunc: DTWAIN_GetPatchMaxRetries,
            DTWAIN_GetPatchPrioritiesFunc: DTWAIN_GetPatchPriorities,
            DTWAIN_GetPatchSearchModeFunc: DTWAIN_GetPatchSearchMode,
            DTWAIN_GetPatchTimeOutFunc: DTWAIN_GetPatchTimeOut,
            DTWAIN_GetPixelFlavorFunc: DTWAIN_GetPixelFlavor,
            DTWAIN_GetPixelTypeFunc: DTWAIN_GetPixelType,
            DTWAIN_GetPrinterFunc: DTWAIN_GetPrinter,
            DTWAIN_GetPrinterStartNumberFunc: DTWAIN_GetPrinterStartNumber,
            DTWAIN_GetPrinterStringModeFunc: DTWAIN_GetPrinterStringMode,
            DTWAIN_GetPrinterStringsFunc: DTWAIN_GetPrinterStrings,
            DTWAIN_GetPrinterSuffixStringFunc: DTWAIN_GetPrinterSuffixString,
            DTWAIN_GetPrinterSuffixStringAFunc: DTWAIN_GetPrinterSuffixStringA,
            DTWAIN_GetPrinterSuffixStringWFunc: DTWAIN_GetPrinterSuffixStringW,
            DTWAIN_GetRegisteredMsgFunc: DTWAIN_GetRegisteredMsg,
            DTWAIN_GetResolutionFunc: DTWAIN_GetResolution,
            DTWAIN_GetResolutionStringFunc: DTWAIN_GetResolutionString,
            DTWAIN_GetResolutionStringAFunc: DTWAIN_GetResolutionStringA,
            DTWAIN_GetResolutionStringWFunc: DTWAIN_GetResolutionStringW,
            DTWAIN_GetResourceStringFunc: DTWAIN_GetResourceString,
            DTWAIN_GetResourceStringAFunc: DTWAIN_GetResourceStringA,
            DTWAIN_GetResourceStringWFunc: DTWAIN_GetResourceStringW,
            DTWAIN_GetRotationFunc: DTWAIN_GetRotation,
            DTWAIN_GetRotationStringFunc: DTWAIN_GetRotationString,
            DTWAIN_GetRotationStringAFunc: DTWAIN_GetRotationStringA,
            DTWAIN_GetRotationStringWFunc: DTWAIN_GetRotationStringW,
            DTWAIN_GetSaveFileNameFunc: DTWAIN_GetSaveFileName,
            DTWAIN_GetSaveFileNameAFunc: DTWAIN_GetSaveFileNameA,
            DTWAIN_GetSaveFileNameWFunc: DTWAIN_GetSaveFileNameW,
            DTWAIN_GetSavedFilesCountFunc: DTWAIN_GetSavedFilesCount,
            DTWAIN_GetSessionDetailsFunc: DTWAIN_GetSessionDetails,
            DTWAIN_GetSessionDetailsAFunc: DTWAIN_GetSessionDetailsA,
            DTWAIN_GetSessionDetailsWFunc: DTWAIN_GetSessionDetailsW,
            DTWAIN_GetShadowFunc: DTWAIN_GetShadow,
            DTWAIN_GetShadowStringFunc: DTWAIN_GetShadowString,
            DTWAIN_GetShadowStringAFunc: DTWAIN_GetShadowStringA,
            DTWAIN_GetShadowStringWFunc: DTWAIN_GetShadowStringW,
            DTWAIN_GetShortVersionStringFunc: DTWAIN_GetShortVersionString,
            DTWAIN_GetShortVersionStringAFunc: DTWAIN_GetShortVersionStringA,
            DTWAIN_GetShortVersionStringWFunc: DTWAIN_GetShortVersionStringW,
            DTWAIN_GetSourceAcquisitionsFunc: DTWAIN_GetSourceAcquisitions,
            DTWAIN_GetSourceDetailsFunc: DTWAIN_GetSourceDetails,
            DTWAIN_GetSourceDetailsAFunc: DTWAIN_GetSourceDetailsA,
            DTWAIN_GetSourceDetailsWFunc: DTWAIN_GetSourceDetailsW,
            DTWAIN_GetSourceIDFunc: DTWAIN_GetSourceID,
            DTWAIN_GetSourceIDExFunc: DTWAIN_GetSourceIDEx,
            DTWAIN_GetSourceManufacturerFunc: DTWAIN_GetSourceManufacturer,
            DTWAIN_GetSourceManufacturerAFunc: DTWAIN_GetSourceManufacturerA,
            DTWAIN_GetSourceManufacturerWFunc: DTWAIN_GetSourceManufacturerW,
            DTWAIN_GetSourceProductFamilyFunc: DTWAIN_GetSourceProductFamily,
            DTWAIN_GetSourceProductFamilyAFunc: DTWAIN_GetSourceProductFamilyA,
            DTWAIN_GetSourceProductFamilyWFunc: DTWAIN_GetSourceProductFamilyW,
            DTWAIN_GetSourceProductNameFunc: DTWAIN_GetSourceProductName,
            DTWAIN_GetSourceProductNameAFunc: DTWAIN_GetSourceProductNameA,
            DTWAIN_GetSourceProductNameWFunc: DTWAIN_GetSourceProductNameW,
            DTWAIN_GetSourceUnitFunc: DTWAIN_GetSourceUnit,
            DTWAIN_GetSourceVersionInfoFunc: DTWAIN_GetSourceVersionInfo,
            DTWAIN_GetSourceVersionInfoAFunc: DTWAIN_GetSourceVersionInfoA,
            DTWAIN_GetSourceVersionInfoWFunc: DTWAIN_GetSourceVersionInfoW,
            DTWAIN_GetSourceVersionNumberFunc: DTWAIN_GetSourceVersionNumber,
            DTWAIN_GetStaticLibVersionFunc: DTWAIN_GetStaticLibVersion,
            DTWAIN_GetTempFileDirectoryFunc: DTWAIN_GetTempFileDirectory,
            DTWAIN_GetTempFileDirectoryAFunc: DTWAIN_GetTempFileDirectoryA,
            DTWAIN_GetTempFileDirectoryWFunc: DTWAIN_GetTempFileDirectoryW,
            DTWAIN_GetThresholdFunc: DTWAIN_GetThreshold,
            DTWAIN_GetThresholdStringFunc: DTWAIN_GetThresholdString,
            DTWAIN_GetThresholdStringAFunc: DTWAIN_GetThresholdStringA,
            DTWAIN_GetThresholdStringWFunc: DTWAIN_GetThresholdStringW,
            DTWAIN_GetTimeDateFunc: DTWAIN_GetTimeDate,
            DTWAIN_GetTimeDateAFunc: DTWAIN_GetTimeDateA,
            DTWAIN_GetTimeDateWFunc: DTWAIN_GetTimeDateW,
            DTWAIN_GetTwainAppIDFunc: DTWAIN_GetTwainAppID,
            DTWAIN_GetTwainAppIDExFunc: DTWAIN_GetTwainAppIDEx,
            DTWAIN_GetTwainAvailabilityFunc: DTWAIN_GetTwainAvailability,
            DTWAIN_GetTwainAvailabilityExFunc: DTWAIN_GetTwainAvailabilityEx,
            DTWAIN_GetTwainAvailabilityExAFunc: DTWAIN_GetTwainAvailabilityExA,
            DTWAIN_GetTwainAvailabilityExWFunc: DTWAIN_GetTwainAvailabilityExW,
            DTWAIN_GetTwainCountryNameFunc: DTWAIN_GetTwainCountryName,
            DTWAIN_GetTwainCountryNameAFunc: DTWAIN_GetTwainCountryNameA,
            DTWAIN_GetTwainCountryNameWFunc: DTWAIN_GetTwainCountryNameW,
            DTWAIN_GetTwainCountryValueFunc: DTWAIN_GetTwainCountryValue,
            DTWAIN_GetTwainCountryValueAFunc: DTWAIN_GetTwainCountryValueA,
            DTWAIN_GetTwainCountryValueWFunc: DTWAIN_GetTwainCountryValueW,
            DTWAIN_GetTwainHwndFunc: DTWAIN_GetTwainHwnd,
            DTWAIN_GetTwainIDFromNameFunc: DTWAIN_GetTwainIDFromName,
            DTWAIN_GetTwainIDFromNameAFunc: DTWAIN_GetTwainIDFromNameA,
            DTWAIN_GetTwainIDFromNameWFunc: DTWAIN_GetTwainIDFromNameW,
            DTWAIN_GetTwainLanguageNameFunc: DTWAIN_GetTwainLanguageName,
            DTWAIN_GetTwainLanguageNameAFunc: DTWAIN_GetTwainLanguageNameA,
            DTWAIN_GetTwainLanguageNameWFunc: DTWAIN_GetTwainLanguageNameW,
            DTWAIN_GetTwainLanguageValueFunc: DTWAIN_GetTwainLanguageValue,
            DTWAIN_GetTwainLanguageValueAFunc: DTWAIN_GetTwainLanguageValueA,
            DTWAIN_GetTwainLanguageValueWFunc: DTWAIN_GetTwainLanguageValueW,
            DTWAIN_GetTwainModeFunc: DTWAIN_GetTwainMode,
            DTWAIN_GetTwainNameFromConstantFunc: DTWAIN_GetTwainNameFromConstant,
            DTWAIN_GetTwainNameFromConstantAFunc: DTWAIN_GetTwainNameFromConstantA,
            DTWAIN_GetTwainNameFromConstantWFunc: DTWAIN_GetTwainNameFromConstantW,
            DTWAIN_GetTwainStringNameFunc: DTWAIN_GetTwainStringName,
            DTWAIN_GetTwainStringNameAFunc: DTWAIN_GetTwainStringNameA,
            DTWAIN_GetTwainStringNameWFunc: DTWAIN_GetTwainStringNameW,
            DTWAIN_GetTwainTimeoutFunc: DTWAIN_GetTwainTimeout,
            DTWAIN_GetVersionFunc: DTWAIN_GetVersion,
            DTWAIN_GetVersionCopyrightFunc: DTWAIN_GetVersionCopyright,
            DTWAIN_GetVersionCopyrightAFunc: DTWAIN_GetVersionCopyrightA,
            DTWAIN_GetVersionCopyrightWFunc: DTWAIN_GetVersionCopyrightW,
            DTWAIN_GetVersionExFunc: DTWAIN_GetVersionEx,
            DTWAIN_GetVersionInfoFunc: DTWAIN_GetVersionInfo,
            DTWAIN_GetVersionInfoAFunc: DTWAIN_GetVersionInfoA,
            DTWAIN_GetVersionInfoWFunc: DTWAIN_GetVersionInfoW,
            DTWAIN_GetVersionStringFunc: DTWAIN_GetVersionString,
            DTWAIN_GetVersionStringAFunc: DTWAIN_GetVersionStringA,
            DTWAIN_GetVersionStringWFunc: DTWAIN_GetVersionStringW,
            DTWAIN_GetWindowsVersionInfoFunc: DTWAIN_GetWindowsVersionInfo,
            DTWAIN_GetWindowsVersionInfoAFunc: DTWAIN_GetWindowsVersionInfoA,
            DTWAIN_GetWindowsVersionInfoWFunc: DTWAIN_GetWindowsVersionInfoW,
            DTWAIN_GetXResolutionFunc: DTWAIN_GetXResolution,
            DTWAIN_GetXResolutionStringFunc: DTWAIN_GetXResolutionString,
            DTWAIN_GetXResolutionStringAFunc: DTWAIN_GetXResolutionStringA,
            DTWAIN_GetXResolutionStringWFunc: DTWAIN_GetXResolutionStringW,
            DTWAIN_GetYResolutionFunc: DTWAIN_GetYResolution,
            DTWAIN_GetYResolutionStringFunc: DTWAIN_GetYResolutionString,
            DTWAIN_GetYResolutionStringAFunc: DTWAIN_GetYResolutionStringA,
            DTWAIN_GetYResolutionStringWFunc: DTWAIN_GetYResolutionStringW,
            DTWAIN_InitExtImageInfoFunc: DTWAIN_InitExtImageInfo,
            DTWAIN_InitImageFileAppendFunc: DTWAIN_InitImageFileAppend,
            DTWAIN_InitImageFileAppendAFunc: DTWAIN_InitImageFileAppendA,
            DTWAIN_InitImageFileAppendWFunc: DTWAIN_InitImageFileAppendW,
            DTWAIN_InitOCRInterfaceFunc: DTWAIN_InitOCRInterface,
            DTWAIN_IsAcquiringFunc: DTWAIN_IsAcquiring,
            DTWAIN_IsAudioXferSupportedFunc: DTWAIN_IsAudioXferSupported,
            DTWAIN_IsAutoBorderDetectEnabledFunc: DTWAIN_IsAutoBorderDetectEnabled,
            DTWAIN_IsAutoBorderDetectSupportedFunc: DTWAIN_IsAutoBorderDetectSupported,
            DTWAIN_IsAutoBrightEnabledFunc: DTWAIN_IsAutoBrightEnabled,
            DTWAIN_IsAutoBrightSupportedFunc: DTWAIN_IsAutoBrightSupported,
            DTWAIN_IsAutoDeskewEnabledFunc: DTWAIN_IsAutoDeskewEnabled,
            DTWAIN_IsAutoDeskewSupportedFunc: DTWAIN_IsAutoDeskewSupported,
            DTWAIN_IsAutoFeedEnabledFunc: DTWAIN_IsAutoFeedEnabled,
            DTWAIN_IsAutoFeedSupportedFunc: DTWAIN_IsAutoFeedSupported,
            DTWAIN_IsAutoRotateEnabledFunc: DTWAIN_IsAutoRotateEnabled,
            DTWAIN_IsAutoRotateSupportedFunc: DTWAIN_IsAutoRotateSupported,
            DTWAIN_IsAutoScanEnabledFunc: DTWAIN_IsAutoScanEnabled,
            DTWAIN_IsAutomaticSenseMediumEnabledFunc: DTWAIN_IsAutomaticSenseMediumEnabled,
            DTWAIN_IsAutomaticSenseMediumSupportedFunc: DTWAIN_IsAutomaticSenseMediumSupported,
            DTWAIN_IsBlankPageDetectionOnFunc: DTWAIN_IsBlankPageDetectionOn,
            DTWAIN_IsBufferedTileModeOnFunc: DTWAIN_IsBufferedTileModeOn,
            DTWAIN_IsBufferedTileModeSupportedFunc: DTWAIN_IsBufferedTileModeSupported,
            DTWAIN_IsCapSupportedFunc: DTWAIN_IsCapSupported,
            DTWAIN_IsCompressionSupportedFunc: DTWAIN_IsCompressionSupported,
            DTWAIN_IsCustomDSDataSupportedFunc: DTWAIN_IsCustomDSDataSupported,
            DTWAIN_IsDIBBlankFunc: DTWAIN_IsDIBBlank,
            DTWAIN_IsDIBBlankStringFunc: DTWAIN_IsDIBBlankString,
            DTWAIN_IsDIBBlankStringAFunc: DTWAIN_IsDIBBlankStringA,
            DTWAIN_IsDIBBlankStringWFunc: DTWAIN_IsDIBBlankStringW,
            DTWAIN_IsDeviceEventSupportedFunc: DTWAIN_IsDeviceEventSupported,
            DTWAIN_IsDeviceOnLineFunc: DTWAIN_IsDeviceOnLine,
            DTWAIN_IsDoubleFeedDetectLengthSupportedFunc: DTWAIN_IsDoubleFeedDetectLengthSupported,
            DTWAIN_IsDoubleFeedDetectSupportedFunc: DTWAIN_IsDoubleFeedDetectSupported,
            DTWAIN_IsDuplexEnabledFunc: DTWAIN_IsDuplexEnabled,
            DTWAIN_IsDuplexSupportedFunc: DTWAIN_IsDuplexSupported,
            DTWAIN_IsExtImageInfoSupportedFunc: DTWAIN_IsExtImageInfoSupported,
            DTWAIN_IsFeederEnabledFunc: DTWAIN_IsFeederEnabled,
            DTWAIN_IsFeederLoadedFunc: DTWAIN_IsFeederLoaded,
            DTWAIN_IsFeederSensitiveFunc: DTWAIN_IsFeederSensitive,
            DTWAIN_IsFeederSupportedFunc: DTWAIN_IsFeederSupported,
            DTWAIN_IsFileSystemSupportedFunc: DTWAIN_IsFileSystemSupported,
            DTWAIN_IsFileXferSupportedFunc: DTWAIN_IsFileXferSupported,
            DTWAIN_IsIAFieldALastPageSupportedFunc: DTWAIN_IsIAFieldALastPageSupported,
            DTWAIN_IsIAFieldALevelSupportedFunc: DTWAIN_IsIAFieldALevelSupported,
            DTWAIN_IsIAFieldAPrintFormatSupportedFunc: DTWAIN_IsIAFieldAPrintFormatSupported,
            DTWAIN_IsIAFieldAValueSupportedFunc: DTWAIN_IsIAFieldAValueSupported,
            DTWAIN_IsIAFieldBLastPageSupportedFunc: DTWAIN_IsIAFieldBLastPageSupported,
            DTWAIN_IsIAFieldBLevelSupportedFunc: DTWAIN_IsIAFieldBLevelSupported,
            DTWAIN_IsIAFieldBPrintFormatSupportedFunc: DTWAIN_IsIAFieldBPrintFormatSupported,
            DTWAIN_IsIAFieldBValueSupportedFunc: DTWAIN_IsIAFieldBValueSupported,
            DTWAIN_IsIAFieldCLastPageSupportedFunc: DTWAIN_IsIAFieldCLastPageSupported,
            DTWAIN_IsIAFieldCLevelSupportedFunc: DTWAIN_IsIAFieldCLevelSupported,
            DTWAIN_IsIAFieldCPrintFormatSupportedFunc: DTWAIN_IsIAFieldCPrintFormatSupported,
            DTWAIN_IsIAFieldCValueSupportedFunc: DTWAIN_IsIAFieldCValueSupported,
            DTWAIN_IsIAFieldDLastPageSupportedFunc: DTWAIN_IsIAFieldDLastPageSupported,
            DTWAIN_IsIAFieldDLevelSupportedFunc: DTWAIN_IsIAFieldDLevelSupported,
            DTWAIN_IsIAFieldDPrintFormatSupportedFunc: DTWAIN_IsIAFieldDPrintFormatSupported,
            DTWAIN_IsIAFieldDValueSupportedFunc: DTWAIN_IsIAFieldDValueSupported,
            DTWAIN_IsIAFieldELastPageSupportedFunc: DTWAIN_IsIAFieldELastPageSupported,
            DTWAIN_IsIAFieldELevelSupportedFunc: DTWAIN_IsIAFieldELevelSupported,
            DTWAIN_IsIAFieldEPrintFormatSupportedFunc: DTWAIN_IsIAFieldEPrintFormatSupported,
            DTWAIN_IsIAFieldEValueSupportedFunc: DTWAIN_IsIAFieldEValueSupported,
            DTWAIN_IsImageAddressingSupportedFunc: DTWAIN_IsImageAddressingSupported,
            DTWAIN_IsIndicatorEnabledFunc: DTWAIN_IsIndicatorEnabled,
            DTWAIN_IsIndicatorSupportedFunc: DTWAIN_IsIndicatorSupported,
            DTWAIN_IsInitializedFunc: DTWAIN_IsInitialized,
            DTWAIN_IsJPEGSupportedFunc: DTWAIN_IsJPEGSupported,
            DTWAIN_IsJobControlSupportedFunc: DTWAIN_IsJobControlSupported,
            DTWAIN_IsLampEnabledFunc: DTWAIN_IsLampEnabled,
            DTWAIN_IsLampSupportedFunc: DTWAIN_IsLampSupported,
            DTWAIN_IsLightPathSupportedFunc: DTWAIN_IsLightPathSupported,
            DTWAIN_IsLightSourceSupportedFunc: DTWAIN_IsLightSourceSupported,
            DTWAIN_IsMaxBuffersSupportedFunc: DTWAIN_IsMaxBuffersSupported,
            DTWAIN_IsMemFileXferSupportedFunc: DTWAIN_IsMemFileXferSupported,
            DTWAIN_IsMsgNotifyEnabledFunc: DTWAIN_IsMsgNotifyEnabled,
            DTWAIN_IsNotifyTripletsEnabledFunc: DTWAIN_IsNotifyTripletsEnabled,
            DTWAIN_IsOCREngineActivatedFunc: DTWAIN_IsOCREngineActivated,
            DTWAIN_IsOpenSourcesOnSelectFunc: DTWAIN_IsOpenSourcesOnSelect,
            DTWAIN_IsOrientationSupportedFunc: DTWAIN_IsOrientationSupported,
            DTWAIN_IsOverscanSupportedFunc: DTWAIN_IsOverscanSupported,
            DTWAIN_IsPDFSupportedFunc: DTWAIN_IsPDFSupported,
            DTWAIN_IsPNGSupportedFunc: DTWAIN_IsPNGSupported,
            DTWAIN_IsPaperDetectableFunc: DTWAIN_IsPaperDetectable,
            DTWAIN_IsPaperSizeSupportedFunc: DTWAIN_IsPaperSizeSupported,
            DTWAIN_IsPatchCapsSupportedFunc: DTWAIN_IsPatchCapsSupported,
            DTWAIN_IsPatchDetectEnabledFunc: DTWAIN_IsPatchDetectEnabled,
            DTWAIN_IsPatchSupportedFunc: DTWAIN_IsPatchSupported,
            DTWAIN_IsPeekMessageLoopEnabledFunc: DTWAIN_IsPeekMessageLoopEnabled,
            DTWAIN_IsPixelTypeSupportedFunc: DTWAIN_IsPixelTypeSupported,
            DTWAIN_IsPrinterEnabledFunc: DTWAIN_IsPrinterEnabled,
            DTWAIN_IsPrinterSupportedFunc: DTWAIN_IsPrinterSupported,
            DTWAIN_IsRotationSupportedFunc: DTWAIN_IsRotationSupported,
            DTWAIN_IsSessionEnabledFunc: DTWAIN_IsSessionEnabled,
            DTWAIN_IsSkipImageInfoErrorFunc: DTWAIN_IsSkipImageInfoError,
            DTWAIN_IsSourceAcquiringFunc: DTWAIN_IsSourceAcquiring,
            DTWAIN_IsSourceAcquiringExFunc: DTWAIN_IsSourceAcquiringEx,
            DTWAIN_IsSourceInUIOnlyModeFunc: DTWAIN_IsSourceInUIOnlyMode,
            DTWAIN_IsSourceOpenFunc: DTWAIN_IsSourceOpen,
            DTWAIN_IsSourceSelectedFunc: DTWAIN_IsSourceSelected,
            DTWAIN_IsSourceValidFunc: DTWAIN_IsSourceValid,
            DTWAIN_IsTIFFSupportedFunc: DTWAIN_IsTIFFSupported,
            DTWAIN_IsThumbnailEnabledFunc: DTWAIN_IsThumbnailEnabled,
            DTWAIN_IsThumbnailSupportedFunc: DTWAIN_IsThumbnailSupported,
            DTWAIN_IsTwainAvailableFunc: DTWAIN_IsTwainAvailable,
            DTWAIN_IsTwainAvailableExFunc: DTWAIN_IsTwainAvailableEx,
            DTWAIN_IsTwainAvailableExAFunc: DTWAIN_IsTwainAvailableExA,
            DTWAIN_IsTwainAvailableExWFunc: DTWAIN_IsTwainAvailableExW,
            DTWAIN_IsUIControllableFunc: DTWAIN_IsUIControllable,
            DTWAIN_IsUIEnabledFunc: DTWAIN_IsUIEnabled,
            DTWAIN_IsUIOnlySupportedFunc: DTWAIN_IsUIOnlySupported,
            DTWAIN_LoadCustomStringResourcesFunc: DTWAIN_LoadCustomStringResources,
            DTWAIN_LoadCustomStringResourcesAFunc: DTWAIN_LoadCustomStringResourcesA,
            DTWAIN_LoadCustomStringResourcesExFunc: DTWAIN_LoadCustomStringResourcesEx,
            DTWAIN_LoadCustomStringResourcesExAFunc: DTWAIN_LoadCustomStringResourcesExA,
            DTWAIN_LoadCustomStringResourcesExWFunc: DTWAIN_LoadCustomStringResourcesExW,
            DTWAIN_LoadCustomStringResourcesWFunc: DTWAIN_LoadCustomStringResourcesW,
            DTWAIN_LoadLanguageResourceFunc: DTWAIN_LoadLanguageResource,
            DTWAIN_LockMemoryFunc: DTWAIN_LockMemory,
            DTWAIN_LockMemoryExFunc: DTWAIN_LockMemoryEx,
            DTWAIN_LogMessageFunc: DTWAIN_LogMessage,
            DTWAIN_LogMessageAFunc: DTWAIN_LogMessageA,
            DTWAIN_LogMessageWFunc: DTWAIN_LogMessageW,
            DTWAIN_MakeRGBFunc: DTWAIN_MakeRGB,
            DTWAIN_OpenSourceFunc: DTWAIN_OpenSource,
            DTWAIN_OpenSourcesOnSelectFunc: DTWAIN_OpenSourcesOnSelect,
            DTWAIN_RangeCreateFunc: DTWAIN_RangeCreate,
            DTWAIN_RangeCreateFromCapFunc: DTWAIN_RangeCreateFromCap,
            DTWAIN_RangeDestroyFunc: DTWAIN_RangeDestroy,
            DTWAIN_RangeExpandFunc: DTWAIN_RangeExpand,
            DTWAIN_RangeExpandExFunc: DTWAIN_RangeExpandEx,
            DTWAIN_RangeGetAllFunc: DTWAIN_RangeGetAll,
            DTWAIN_RangeGetAllFloatFunc: DTWAIN_RangeGetAllFloat,
            DTWAIN_RangeGetAllFloatStringFunc: DTWAIN_RangeGetAllFloatString,
            DTWAIN_RangeGetAllFloatStringAFunc: DTWAIN_RangeGetAllFloatStringA,
            DTWAIN_RangeGetAllFloatStringWFunc: DTWAIN_RangeGetAllFloatStringW,
            DTWAIN_RangeGetAllLongFunc: DTWAIN_RangeGetAllLong,
            DTWAIN_RangeGetCountFunc: DTWAIN_RangeGetCount,
            DTWAIN_RangeGetExpValueFunc: DTWAIN_RangeGetExpValue,
            DTWAIN_RangeGetExpValueFloatFunc: DTWAIN_RangeGetExpValueFloat,
            DTWAIN_RangeGetExpValueFloatStringFunc: DTWAIN_RangeGetExpValueFloatString,
            DTWAIN_RangeGetExpValueFloatStringAFunc: DTWAIN_RangeGetExpValueFloatStringA,
            DTWAIN_RangeGetExpValueFloatStringWFunc: DTWAIN_RangeGetExpValueFloatStringW,
            DTWAIN_RangeGetExpValueLongFunc: DTWAIN_RangeGetExpValueLong,
            DTWAIN_RangeGetNearestValueFunc: DTWAIN_RangeGetNearestValue,
            DTWAIN_RangeGetPosFunc: DTWAIN_RangeGetPos,
            DTWAIN_RangeGetPosFloatFunc: DTWAIN_RangeGetPosFloat,
            DTWAIN_RangeGetPosFloatStringFunc: DTWAIN_RangeGetPosFloatString,
            DTWAIN_RangeGetPosFloatStringAFunc: DTWAIN_RangeGetPosFloatStringA,
            DTWAIN_RangeGetPosFloatStringWFunc: DTWAIN_RangeGetPosFloatStringW,
            DTWAIN_RangeGetPosLongFunc: DTWAIN_RangeGetPosLong,
            DTWAIN_RangeGetValueFunc: DTWAIN_RangeGetValue,
            DTWAIN_RangeGetValueFloatFunc: DTWAIN_RangeGetValueFloat,
            DTWAIN_RangeGetValueFloatStringFunc: DTWAIN_RangeGetValueFloatString,
            DTWAIN_RangeGetValueFloatStringAFunc: DTWAIN_RangeGetValueFloatStringA,
            DTWAIN_RangeGetValueFloatStringWFunc: DTWAIN_RangeGetValueFloatStringW,
            DTWAIN_RangeGetValueLongFunc: DTWAIN_RangeGetValueLong,
            DTWAIN_RangeIsValidFunc: DTWAIN_RangeIsValid,
            DTWAIN_RangeNearestValueFloatFunc: DTWAIN_RangeNearestValueFloat,
            DTWAIN_RangeNearestValueFloatStringFunc: DTWAIN_RangeNearestValueFloatString,
            DTWAIN_RangeNearestValueFloatStringAFunc: DTWAIN_RangeNearestValueFloatStringA,
            DTWAIN_RangeNearestValueFloatStringWFunc: DTWAIN_RangeNearestValueFloatStringW,
            DTWAIN_RangeNearestValueLongFunc: DTWAIN_RangeNearestValueLong,
            DTWAIN_RangeSetAllFunc: DTWAIN_RangeSetAll,
            DTWAIN_RangeSetAllFloatFunc: DTWAIN_RangeSetAllFloat,
            DTWAIN_RangeSetAllFloatStringFunc: DTWAIN_RangeSetAllFloatString,
            DTWAIN_RangeSetAllFloatStringAFunc: DTWAIN_RangeSetAllFloatStringA,
            DTWAIN_RangeSetAllFloatStringWFunc: DTWAIN_RangeSetAllFloatStringW,
            DTWAIN_RangeSetAllLongFunc: DTWAIN_RangeSetAllLong,
            DTWAIN_RangeSetValueFunc: DTWAIN_RangeSetValue,
            DTWAIN_RangeSetValueFloatFunc: DTWAIN_RangeSetValueFloat,
            DTWAIN_RangeSetValueFloatStringFunc: DTWAIN_RangeSetValueFloatString,
            DTWAIN_RangeSetValueFloatStringAFunc: DTWAIN_RangeSetValueFloatStringA,
            DTWAIN_RangeSetValueFloatStringWFunc: DTWAIN_RangeSetValueFloatStringW,
            DTWAIN_RangeSetValueLongFunc: DTWAIN_RangeSetValueLong,
            DTWAIN_ResetPDFTextElementFunc: DTWAIN_ResetPDFTextElement,
            DTWAIN_RewindPageFunc: DTWAIN_RewindPage,
            DTWAIN_SelectDefaultOCREngineFunc: DTWAIN_SelectDefaultOCREngine,
            DTWAIN_SelectDefaultSourceFunc: DTWAIN_SelectDefaultSource,
            DTWAIN_SelectDefaultSourceWithOpenFunc: DTWAIN_SelectDefaultSourceWithOpen,
            DTWAIN_SelectOCREngineFunc: DTWAIN_SelectOCREngine,
            DTWAIN_SelectOCREngine2Func: DTWAIN_SelectOCREngine2,
            DTWAIN_SelectOCREngine2AFunc: DTWAIN_SelectOCREngine2A,
            DTWAIN_SelectOCREngine2ExFunc: DTWAIN_SelectOCREngine2Ex,
            DTWAIN_SelectOCREngine2ExAFunc: DTWAIN_SelectOCREngine2ExA,
            DTWAIN_SelectOCREngine2ExWFunc: DTWAIN_SelectOCREngine2ExW,
            DTWAIN_SelectOCREngine2WFunc: DTWAIN_SelectOCREngine2W,
            DTWAIN_SelectOCREngineByNameFunc: DTWAIN_SelectOCREngineByName,
            DTWAIN_SelectOCREngineByNameAFunc: DTWAIN_SelectOCREngineByNameA,
            DTWAIN_SelectOCREngineByNameWFunc: DTWAIN_SelectOCREngineByNameW,
            DTWAIN_SelectSourceFunc: DTWAIN_SelectSource,
            DTWAIN_SelectSource2Func: DTWAIN_SelectSource2,
            DTWAIN_SelectSource2AFunc: DTWAIN_SelectSource2A,
            DTWAIN_SelectSource2ExFunc: DTWAIN_SelectSource2Ex,
            DTWAIN_SelectSource2ExAFunc: DTWAIN_SelectSource2ExA,
            DTWAIN_SelectSource2ExWFunc: DTWAIN_SelectSource2ExW,
            DTWAIN_SelectSource2WFunc: DTWAIN_SelectSource2W,
            DTWAIN_SelectSourceByNameFunc: DTWAIN_SelectSourceByName,
            DTWAIN_SelectSourceByNameAFunc: DTWAIN_SelectSourceByNameA,
            DTWAIN_SelectSourceByNameWFunc: DTWAIN_SelectSourceByNameW,
            DTWAIN_SelectSourceByNameWithOpenFunc: DTWAIN_SelectSourceByNameWithOpen,
            DTWAIN_SelectSourceByNameWithOpenAFunc: DTWAIN_SelectSourceByNameWithOpenA,
            DTWAIN_SelectSourceByNameWithOpenWFunc: DTWAIN_SelectSourceByNameWithOpenW,
            DTWAIN_SelectSourceWithOpenFunc: DTWAIN_SelectSourceWithOpen,
            DTWAIN_SetAcquireAreaFunc: DTWAIN_SetAcquireArea,
            DTWAIN_SetAcquireArea2Func: DTWAIN_SetAcquireArea2,
            DTWAIN_SetAcquireArea2StringFunc: DTWAIN_SetAcquireArea2String,
            DTWAIN_SetAcquireArea2StringAFunc: DTWAIN_SetAcquireArea2StringA,
            DTWAIN_SetAcquireArea2StringWFunc: DTWAIN_SetAcquireArea2StringW,
            DTWAIN_SetAcquireImageNegativeFunc: DTWAIN_SetAcquireImageNegative,
            DTWAIN_SetAcquireImageScaleFunc: DTWAIN_SetAcquireImageScale,
            DTWAIN_SetAcquireImageScaleStringFunc: DTWAIN_SetAcquireImageScaleString,
            DTWAIN_SetAcquireImageScaleStringAFunc: DTWAIN_SetAcquireImageScaleStringA,
            DTWAIN_SetAcquireImageScaleStringWFunc: DTWAIN_SetAcquireImageScaleStringW,
            DTWAIN_SetAcquireStripBufferFunc: DTWAIN_SetAcquireStripBuffer,
            DTWAIN_SetAcquireStripSizeFunc: DTWAIN_SetAcquireStripSize,
            DTWAIN_SetAlarmVolumeFunc: DTWAIN_SetAlarmVolume,
            DTWAIN_SetAlarmsFunc: DTWAIN_SetAlarms,
            DTWAIN_SetAllCapsToDefaultFunc: DTWAIN_SetAllCapsToDefault,
            DTWAIN_SetAppInfoFunc: DTWAIN_SetAppInfo,
            DTWAIN_SetAppInfoAFunc: DTWAIN_SetAppInfoA,
            DTWAIN_SetAppInfoWFunc: DTWAIN_SetAppInfoW,
            DTWAIN_SetAuthorFunc: DTWAIN_SetAuthor,
            DTWAIN_SetAuthorAFunc: DTWAIN_SetAuthorA,
            DTWAIN_SetAuthorWFunc: DTWAIN_SetAuthorW,
            DTWAIN_SetAvailablePrintersFunc: DTWAIN_SetAvailablePrinters,
            DTWAIN_SetAvailablePrintersArrayFunc: DTWAIN_SetAvailablePrintersArray,
            DTWAIN_SetBitDepthFunc: DTWAIN_SetBitDepth,
            DTWAIN_SetBlankPageDetectionFunc: DTWAIN_SetBlankPageDetection,
            DTWAIN_SetBlankPageDetectionExFunc: DTWAIN_SetBlankPageDetectionEx,
            DTWAIN_SetBlankPageDetectionExStringFunc: DTWAIN_SetBlankPageDetectionExString,
            DTWAIN_SetBlankPageDetectionExStringAFunc: DTWAIN_SetBlankPageDetectionExStringA,
            DTWAIN_SetBlankPageDetectionExStringWFunc: DTWAIN_SetBlankPageDetectionExStringW,
            DTWAIN_SetBlankPageDetectionStringFunc: DTWAIN_SetBlankPageDetectionString,
            DTWAIN_SetBlankPageDetectionStringAFunc: DTWAIN_SetBlankPageDetectionStringA,
            DTWAIN_SetBlankPageDetectionStringWFunc: DTWAIN_SetBlankPageDetectionStringW,
            DTWAIN_SetBrightnessFunc: DTWAIN_SetBrightness,
            DTWAIN_SetBrightnessStringFunc: DTWAIN_SetBrightnessString,
            DTWAIN_SetBrightnessStringAFunc: DTWAIN_SetBrightnessStringA,
            DTWAIN_SetBrightnessStringWFunc: DTWAIN_SetBrightnessStringW,
            DTWAIN_SetBufferedTileModeFunc: DTWAIN_SetBufferedTileMode,
            DTWAIN_SetCallbackFunc: DTWAIN_SetCallback,
            DTWAIN_SetCallback64Func: DTWAIN_SetCallback64,
            DTWAIN_SetCameraFunc: DTWAIN_SetCamera,
            DTWAIN_SetCameraAFunc: DTWAIN_SetCameraA,
            DTWAIN_SetCameraWFunc: DTWAIN_SetCameraW,
            DTWAIN_SetCapValuesFunc: DTWAIN_SetCapValues,
            DTWAIN_SetCapValuesExFunc: DTWAIN_SetCapValuesEx,
            DTWAIN_SetCapValuesEx2Func: DTWAIN_SetCapValuesEx2,
            DTWAIN_SetCaptionFunc: DTWAIN_SetCaption,
            DTWAIN_SetCaptionAFunc: DTWAIN_SetCaptionA,
            DTWAIN_SetCaptionWFunc: DTWAIN_SetCaptionW,
            DTWAIN_SetCompressionTypeFunc: DTWAIN_SetCompressionType,
            DTWAIN_SetContrastFunc: DTWAIN_SetContrast,
            DTWAIN_SetContrastStringFunc: DTWAIN_SetContrastString,
            DTWAIN_SetContrastStringAFunc: DTWAIN_SetContrastStringA,
            DTWAIN_SetContrastStringWFunc: DTWAIN_SetContrastStringW,
            DTWAIN_SetCountryFunc: DTWAIN_SetCountry,
            DTWAIN_SetCurrentRetryCountFunc: DTWAIN_SetCurrentRetryCount,
            DTWAIN_SetCustomDSDataFunc: DTWAIN_SetCustomDSData,
            DTWAIN_SetDSMSearchOrderFunc: DTWAIN_SetDSMSearchOrder,
            DTWAIN_SetDSMSearchOrderExFunc: DTWAIN_SetDSMSearchOrderEx,
            DTWAIN_SetDSMSearchOrderExAFunc: DTWAIN_SetDSMSearchOrderExA,
            DTWAIN_SetDSMSearchOrderExWFunc: DTWAIN_SetDSMSearchOrderExW,
            DTWAIN_SetDefaultSourceFunc: DTWAIN_SetDefaultSource,
            DTWAIN_SetDeviceNotificationsFunc: DTWAIN_SetDeviceNotifications,
            DTWAIN_SetDeviceTimeDateFunc: DTWAIN_SetDeviceTimeDate,
            DTWAIN_SetDeviceTimeDateAFunc: DTWAIN_SetDeviceTimeDateA,
            DTWAIN_SetDeviceTimeDateWFunc: DTWAIN_SetDeviceTimeDateW,
            DTWAIN_SetDoubleFeedDetectLengthFunc: DTWAIN_SetDoubleFeedDetectLength,
            DTWAIN_SetDoubleFeedDetectLengthStringFunc: DTWAIN_SetDoubleFeedDetectLengthString,
            DTWAIN_SetDoubleFeedDetectLengthStringAFunc: DTWAIN_SetDoubleFeedDetectLengthStringA,
            DTWAIN_SetDoubleFeedDetectLengthStringWFunc: DTWAIN_SetDoubleFeedDetectLengthStringW,
            DTWAIN_SetDoubleFeedDetectValuesFunc: DTWAIN_SetDoubleFeedDetectValues,
            DTWAIN_SetDoublePageCountOnDuplexFunc: DTWAIN_SetDoublePageCountOnDuplex,
            DTWAIN_SetEOJDetectValueFunc: DTWAIN_SetEOJDetectValue,
            DTWAIN_SetErrorBufferThresholdFunc: DTWAIN_SetErrorBufferThreshold,
            DTWAIN_SetErrorCallbackFunc: DTWAIN_SetErrorCallback,
            DTWAIN_SetErrorCallback64Func: DTWAIN_SetErrorCallback64,
            DTWAIN_SetFeederAlignmentFunc: DTWAIN_SetFeederAlignment,
            DTWAIN_SetFeederOrderFunc: DTWAIN_SetFeederOrder,
            DTWAIN_SetFeederWaitTimeFunc: DTWAIN_SetFeederWaitTime,
            DTWAIN_SetFileAutoIncrementFunc: DTWAIN_SetFileAutoIncrement,
            DTWAIN_SetFileCompressionTypeFunc: DTWAIN_SetFileCompressionType,
            DTWAIN_SetFileSavePosFunc: DTWAIN_SetFileSavePos,
            DTWAIN_SetFileSavePosAFunc: DTWAIN_SetFileSavePosA,
            DTWAIN_SetFileSavePosWFunc: DTWAIN_SetFileSavePosW,
            DTWAIN_SetFileXferFormatFunc: DTWAIN_SetFileXferFormat,
            DTWAIN_SetHalftoneFunc: DTWAIN_SetHalftone,
            DTWAIN_SetHalftoneAFunc: DTWAIN_SetHalftoneA,
            DTWAIN_SetHalftoneWFunc: DTWAIN_SetHalftoneW,
            DTWAIN_SetHighlightFunc: DTWAIN_SetHighlight,
            DTWAIN_SetHighlightStringFunc: DTWAIN_SetHighlightString,
            DTWAIN_SetHighlightStringAFunc: DTWAIN_SetHighlightStringA,
            DTWAIN_SetHighlightStringWFunc: DTWAIN_SetHighlightStringW,
            DTWAIN_SetJobControlFunc: DTWAIN_SetJobControl,
            DTWAIN_SetJpegValuesFunc: DTWAIN_SetJpegValues,
            DTWAIN_SetJpegXRValuesFunc: DTWAIN_SetJpegXRValues,
            DTWAIN_SetLanguageFunc: DTWAIN_SetLanguage,
            DTWAIN_SetLastErrorFunc: DTWAIN_SetLastError,
            DTWAIN_SetLightPathFunc: DTWAIN_SetLightPath,
            DTWAIN_SetLightPathExFunc: DTWAIN_SetLightPathEx,
            DTWAIN_SetLightSourceFunc: DTWAIN_SetLightSource,
            DTWAIN_SetLightSourcesFunc: DTWAIN_SetLightSources,
            DTWAIN_SetLoggerCallbackFunc: DTWAIN_SetLoggerCallback,
            DTWAIN_SetLoggerCallbackAFunc: DTWAIN_SetLoggerCallbackA,
            DTWAIN_SetLoggerCallbackWFunc: DTWAIN_SetLoggerCallbackW,
            DTWAIN_SetManualDuplexModeFunc: DTWAIN_SetManualDuplexMode,
            DTWAIN_SetMaxAcquisitionsFunc: DTWAIN_SetMaxAcquisitions,
            DTWAIN_SetMaxBuffersFunc: DTWAIN_SetMaxBuffers,
            DTWAIN_SetMaxRetryAttemptsFunc: DTWAIN_SetMaxRetryAttempts,
            DTWAIN_SetMultipageScanModeFunc: DTWAIN_SetMultipageScanMode,
            DTWAIN_SetNoiseFilterFunc: DTWAIN_SetNoiseFilter,
            DTWAIN_SetOCRCapValuesFunc: DTWAIN_SetOCRCapValues,
            DTWAIN_SetOrientationFunc: DTWAIN_SetOrientation,
            DTWAIN_SetOverscanFunc: DTWAIN_SetOverscan,
            DTWAIN_SetPDFAESEncryptionFunc: DTWAIN_SetPDFAESEncryption,
            DTWAIN_SetPDFASCIICompressionFunc: DTWAIN_SetPDFASCIICompression,
            DTWAIN_SetPDFAuthorFunc: DTWAIN_SetPDFAuthor,
            DTWAIN_SetPDFAuthorAFunc: DTWAIN_SetPDFAuthorA,
            DTWAIN_SetPDFAuthorWFunc: DTWAIN_SetPDFAuthorW,
            DTWAIN_SetPDFCompressionFunc: DTWAIN_SetPDFCompression,
            DTWAIN_SetPDFCreatorFunc: DTWAIN_SetPDFCreator,
            DTWAIN_SetPDFCreatorAFunc: DTWAIN_SetPDFCreatorA,
            DTWAIN_SetPDFCreatorWFunc: DTWAIN_SetPDFCreatorW,
            DTWAIN_SetPDFEncryptionFunc: DTWAIN_SetPDFEncryption,
            DTWAIN_SetPDFEncryptionAFunc: DTWAIN_SetPDFEncryptionA,
            DTWAIN_SetPDFEncryptionWFunc: DTWAIN_SetPDFEncryptionW,
            DTWAIN_SetPDFJpegQualityFunc: DTWAIN_SetPDFJpegQuality,
            DTWAIN_SetPDFKeywordsFunc: DTWAIN_SetPDFKeywords,
            DTWAIN_SetPDFKeywordsAFunc: DTWAIN_SetPDFKeywordsA,
            DTWAIN_SetPDFKeywordsWFunc: DTWAIN_SetPDFKeywordsW,
            DTWAIN_SetPDFOCRConversionFunc: DTWAIN_SetPDFOCRConversion,
            DTWAIN_SetPDFOCRModeFunc: DTWAIN_SetPDFOCRMode,
            DTWAIN_SetPDFOrientationFunc: DTWAIN_SetPDFOrientation,
            DTWAIN_SetPDFPageScaleFunc: DTWAIN_SetPDFPageScale,
            DTWAIN_SetPDFPageScaleStringFunc: DTWAIN_SetPDFPageScaleString,
            DTWAIN_SetPDFPageScaleStringAFunc: DTWAIN_SetPDFPageScaleStringA,
            DTWAIN_SetPDFPageScaleStringWFunc: DTWAIN_SetPDFPageScaleStringW,
            DTWAIN_SetPDFPageSizeFunc: DTWAIN_SetPDFPageSize,
            DTWAIN_SetPDFPageSizeStringFunc: DTWAIN_SetPDFPageSizeString,
            DTWAIN_SetPDFPageSizeStringAFunc: DTWAIN_SetPDFPageSizeStringA,
            DTWAIN_SetPDFPageSizeStringWFunc: DTWAIN_SetPDFPageSizeStringW,
            DTWAIN_SetPDFPolarityFunc: DTWAIN_SetPDFPolarity,
            DTWAIN_SetPDFProducerFunc: DTWAIN_SetPDFProducer,
            DTWAIN_SetPDFProducerAFunc: DTWAIN_SetPDFProducerA,
            DTWAIN_SetPDFProducerWFunc: DTWAIN_SetPDFProducerW,
            DTWAIN_SetPDFSubjectFunc: DTWAIN_SetPDFSubject,
            DTWAIN_SetPDFSubjectAFunc: DTWAIN_SetPDFSubjectA,
            DTWAIN_SetPDFSubjectWFunc: DTWAIN_SetPDFSubjectW,
            DTWAIN_SetPDFTextElementFloatFunc: DTWAIN_SetPDFTextElementFloat,
            DTWAIN_SetPDFTextElementLongFunc: DTWAIN_SetPDFTextElementLong,
            DTWAIN_SetPDFTextElementStringFunc: DTWAIN_SetPDFTextElementString,
            DTWAIN_SetPDFTextElementStringAFunc: DTWAIN_SetPDFTextElementStringA,
            DTWAIN_SetPDFTextElementStringWFunc: DTWAIN_SetPDFTextElementStringW,
            DTWAIN_SetPDFTitleFunc: DTWAIN_SetPDFTitle,
            DTWAIN_SetPDFTitleAFunc: DTWAIN_SetPDFTitleA,
            DTWAIN_SetPDFTitleWFunc: DTWAIN_SetPDFTitleW,
            DTWAIN_SetPaperSizeFunc: DTWAIN_SetPaperSize,
            DTWAIN_SetPatchMaxPrioritiesFunc: DTWAIN_SetPatchMaxPriorities,
            DTWAIN_SetPatchMaxRetriesFunc: DTWAIN_SetPatchMaxRetries,
            DTWAIN_SetPatchPrioritiesFunc: DTWAIN_SetPatchPriorities,
            DTWAIN_SetPatchSearchModeFunc: DTWAIN_SetPatchSearchMode,
            DTWAIN_SetPatchTimeOutFunc: DTWAIN_SetPatchTimeOut,
            DTWAIN_SetPixelFlavorFunc: DTWAIN_SetPixelFlavor,
            DTWAIN_SetPixelTypeFunc: DTWAIN_SetPixelType,
            DTWAIN_SetPostScriptTitleFunc: DTWAIN_SetPostScriptTitle,
            DTWAIN_SetPostScriptTitleAFunc: DTWAIN_SetPostScriptTitleA,
            DTWAIN_SetPostScriptTitleWFunc: DTWAIN_SetPostScriptTitleW,
            DTWAIN_SetPostScriptTypeFunc: DTWAIN_SetPostScriptType,
            DTWAIN_SetPrinterFunc: DTWAIN_SetPrinter,
            DTWAIN_SetPrinterExFunc: DTWAIN_SetPrinterEx,
            DTWAIN_SetPrinterStartNumberFunc: DTWAIN_SetPrinterStartNumber,
            DTWAIN_SetPrinterStringModeFunc: DTWAIN_SetPrinterStringMode,
            DTWAIN_SetPrinterStringsFunc: DTWAIN_SetPrinterStrings,
            DTWAIN_SetPrinterSuffixStringFunc: DTWAIN_SetPrinterSuffixString,
            DTWAIN_SetPrinterSuffixStringAFunc: DTWAIN_SetPrinterSuffixStringA,
            DTWAIN_SetPrinterSuffixStringWFunc: DTWAIN_SetPrinterSuffixStringW,
            DTWAIN_SetQueryCapSupportFunc: DTWAIN_SetQueryCapSupport,
            DTWAIN_SetResolutionFunc: DTWAIN_SetResolution,
            DTWAIN_SetResolutionStringFunc: DTWAIN_SetResolutionString,
            DTWAIN_SetResolutionStringAFunc: DTWAIN_SetResolutionStringA,
            DTWAIN_SetResolutionStringWFunc: DTWAIN_SetResolutionStringW,
            DTWAIN_SetResourcePathFunc: DTWAIN_SetResourcePath,
            DTWAIN_SetResourcePathAFunc: DTWAIN_SetResourcePathA,
            DTWAIN_SetResourcePathWFunc: DTWAIN_SetResourcePathW,
            DTWAIN_SetRotationFunc: DTWAIN_SetRotation,
            DTWAIN_SetRotationStringFunc: DTWAIN_SetRotationString,
            DTWAIN_SetRotationStringAFunc: DTWAIN_SetRotationStringA,
            DTWAIN_SetRotationStringWFunc: DTWAIN_SetRotationStringW,
            DTWAIN_SetSaveFileNameFunc: DTWAIN_SetSaveFileName,
            DTWAIN_SetSaveFileNameAFunc: DTWAIN_SetSaveFileNameA,
            DTWAIN_SetSaveFileNameWFunc: DTWAIN_SetSaveFileNameW,
            DTWAIN_SetShadowFunc: DTWAIN_SetShadow,
            DTWAIN_SetShadowStringFunc: DTWAIN_SetShadowString,
            DTWAIN_SetShadowStringAFunc: DTWAIN_SetShadowStringA,
            DTWAIN_SetShadowStringWFunc: DTWAIN_SetShadowStringW,
            DTWAIN_SetSourceUnitFunc: DTWAIN_SetSourceUnit,
            DTWAIN_SetTIFFCompressTypeFunc: DTWAIN_SetTIFFCompressType,
            DTWAIN_SetTIFFInvertFunc: DTWAIN_SetTIFFInvert,
            DTWAIN_SetTempFileDirectoryFunc: DTWAIN_SetTempFileDirectory,
            DTWAIN_SetTempFileDirectoryAFunc: DTWAIN_SetTempFileDirectoryA,
            DTWAIN_SetTempFileDirectoryExFunc: DTWAIN_SetTempFileDirectoryEx,
            DTWAIN_SetTempFileDirectoryExAFunc: DTWAIN_SetTempFileDirectoryExA,
            DTWAIN_SetTempFileDirectoryExWFunc: DTWAIN_SetTempFileDirectoryExW,
            DTWAIN_SetTempFileDirectoryWFunc: DTWAIN_SetTempFileDirectoryW,
            DTWAIN_SetThresholdFunc: DTWAIN_SetThreshold,
            DTWAIN_SetThresholdStringFunc: DTWAIN_SetThresholdString,
            DTWAIN_SetThresholdStringAFunc: DTWAIN_SetThresholdStringA,
            DTWAIN_SetThresholdStringWFunc: DTWAIN_SetThresholdStringW,
            DTWAIN_SetTwainDSMFunc: DTWAIN_SetTwainDSM,
            DTWAIN_SetTwainLogFunc: DTWAIN_SetTwainLog,
            DTWAIN_SetTwainLogAFunc: DTWAIN_SetTwainLogA,
            DTWAIN_SetTwainLogWFunc: DTWAIN_SetTwainLogW,
            DTWAIN_SetTwainModeFunc: DTWAIN_SetTwainMode,
            DTWAIN_SetTwainTimeoutFunc: DTWAIN_SetTwainTimeout,
            DTWAIN_SetUpdateDibProcFunc: DTWAIN_SetUpdateDibProc,
            DTWAIN_SetXResolutionFunc: DTWAIN_SetXResolution,
            DTWAIN_SetXResolutionStringFunc: DTWAIN_SetXResolutionString,
            DTWAIN_SetXResolutionStringAFunc: DTWAIN_SetXResolutionStringA,
            DTWAIN_SetXResolutionStringWFunc: DTWAIN_SetXResolutionStringW,
            DTWAIN_SetYResolutionFunc: DTWAIN_SetYResolution,
            DTWAIN_SetYResolutionStringFunc: DTWAIN_SetYResolutionString,
            DTWAIN_SetYResolutionStringAFunc: DTWAIN_SetYResolutionStringA,
            DTWAIN_SetYResolutionStringWFunc: DTWAIN_SetYResolutionStringW,
            DTWAIN_ShowUIOnlyFunc: DTWAIN_ShowUIOnly,
            DTWAIN_ShutdownOCREngineFunc: DTWAIN_ShutdownOCREngine,
            DTWAIN_SkipImageInfoErrorFunc: DTWAIN_SkipImageInfoError,
            DTWAIN_StartThreadFunc: DTWAIN_StartThread,
            DTWAIN_StartTwainSessionFunc: DTWAIN_StartTwainSession,
            DTWAIN_StartTwainSessionAFunc: DTWAIN_StartTwainSessionA,
            DTWAIN_StartTwainSessionWFunc: DTWAIN_StartTwainSessionW,
            DTWAIN_SysDestroyFunc: DTWAIN_SysDestroy,
            DTWAIN_SysInitializeFunc: DTWAIN_SysInitialize,
            DTWAIN_SysInitializeExFunc: DTWAIN_SysInitializeEx,
            DTWAIN_SysInitializeEx2Func: DTWAIN_SysInitializeEx2,
            DTWAIN_SysInitializeEx2AFunc: DTWAIN_SysInitializeEx2A,
            DTWAIN_SysInitializeEx2WFunc: DTWAIN_SysInitializeEx2W,
            DTWAIN_SysInitializeExAFunc: DTWAIN_SysInitializeExA,
            DTWAIN_SysInitializeExWFunc: DTWAIN_SysInitializeExW,
            DTWAIN_SysInitializeLibFunc: DTWAIN_SysInitializeLib,
            DTWAIN_SysInitializeLibExFunc: DTWAIN_SysInitializeLibEx,
            DTWAIN_SysInitializeLibEx2Func: DTWAIN_SysInitializeLibEx2,
            DTWAIN_SysInitializeLibEx2AFunc: DTWAIN_SysInitializeLibEx2A,
            DTWAIN_SysInitializeLibEx2WFunc: DTWAIN_SysInitializeLibEx2W,
            DTWAIN_SysInitializeLibExAFunc: DTWAIN_SysInitializeLibExA,
            DTWAIN_SysInitializeLibExWFunc: DTWAIN_SysInitializeLibExW,
            DTWAIN_SysInitializeNoBlockingFunc: DTWAIN_SysInitializeNoBlocking,
            DTWAIN_TestGetCapFunc: DTWAIN_TestGetCap,
            DTWAIN_UnlockMemoryFunc: DTWAIN_UnlockMemory,
            DTWAIN_UnlockMemoryExFunc: DTWAIN_UnlockMemoryEx,
            DTWAIN_UseMultipleThreadsFunc: DTWAIN_UseMultipleThreads
        })
}


    pub fn DTWAIN_AcquireAudioFile(&self, Source: *mut c_void, lpszFile: *const u16, lFileFlags: i32, lMaxClips: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireAudioFileFunc)(Source, lpszFile, lFileFlags, lMaxClips, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireAudioFileA(&self, Source: *mut c_void, lpszFile: *const c_char, lFileFlags: i32, lNumClips: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireAudioFileAFunc)(Source, lpszFile, lFileFlags, lNumClips, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireAudioFileW(&self, Source: *mut c_void, lpszFile: *const u16, lFileFlags: i32, lNumClips: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireAudioFileWFunc)(Source, lpszFile, lFileFlags, lNumClips, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireAudioNative(&self, Source: *mut c_void, nMaxAudioClips: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_AcquireAudioNativeFunc)(Source, nMaxAudioClips, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireAudioNativeEx(&self, Source: *mut c_void, nMaxAudioClips: i32, bShowUI: i32, bCloseSource: i32, Acquisitions: *mut c_void, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireAudioNativeExFunc)(Source, nMaxAudioClips, bShowUI, bCloseSource, Acquisitions, pStatus);  }
    }

    pub fn DTWAIN_AcquireBuffered(&self, Source: *mut c_void, PixelType: i32, nMaxPages: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_AcquireBufferedFunc)(Source, PixelType, nMaxPages, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireBufferedEx(&self, Source: *mut c_void, PixelType: i32, nMaxPages: i32, bShowUI: i32, bCloseSource: i32, Acquisitions: *mut c_void, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireBufferedExFunc)(Source, PixelType, nMaxPages, bShowUI, bCloseSource, Acquisitions, pStatus);  }
    }

    pub fn DTWAIN_AcquireFile(&self, Source: *mut c_void, lpszFile: *const u16, lFileType: i32, lFileFlags: i32, PixelType: i32, lMaxPages: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireFileFunc)(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireFileA(&self, Source: *mut c_void, lpszFile: *const c_char, lFileType: i32, lFileFlags: i32, PixelType: i32, lMaxPages: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireFileAFunc)(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireFileEx(&self, Source: *mut c_void, aFileNames: *mut c_void, lFileType: i32, lFileFlags: i32, PixelType: i32, lMaxPages: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireFileExFunc)(Source, aFileNames, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireFileW(&self, Source: *mut c_void, lpszFile: *const u16, lFileType: i32, lFileFlags: i32, PixelType: i32, lMaxPages: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireFileWFunc)(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireNative(&self, Source: *mut c_void, PixelType: i32, nMaxPages: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_AcquireNativeFunc)(Source, PixelType, nMaxPages, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AcquireNativeEx(&self, Source: *mut c_void, PixelType: i32, nMaxPages: i32, bShowUI: i32, bCloseSource: i32, Acquisitions: *mut c_void, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_AcquireNativeExFunc)(Source, PixelType, nMaxPages, bShowUI, bCloseSource, Acquisitions, pStatus);  }
    }

    pub fn DTWAIN_AcquireToClipboard(&self, Source: *mut c_void, PixelType: i32, nMaxPages: i32, nTransferMode: i32, bDiscardDibs: i32, bShowUI: i32, bCloseSource: i32, pStatus: *mut i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_AcquireToClipboardFunc)(Source, PixelType, nMaxPages, nTransferMode, bDiscardDibs, bShowUI, bCloseSource, pStatus);  }
    }

    pub fn DTWAIN_AddExtImageInfoQuery(&self, Source: *mut c_void, ExtImageInfo: i32) -> i32 {
        unsafe { return (self.DTWAIN_AddExtImageInfoQueryFunc)(Source, ExtImageInfo);  }
    }

    pub fn DTWAIN_AddPDFText(&self, Source: *mut c_void, szText: *const u16, xPos: i32, yPos: i32, fontName: *const u16, fontSize: f64, colorRGB: i32, renderMode: i32, scaling: f64, charSpacing: f64, wordSpacing: f64, strokeWidth: i32, Flags: u32) -> i32 {
        unsafe { return (self.DTWAIN_AddPDFTextFunc)(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);  }
    }

    pub fn DTWAIN_AddPDFTextA(&self, Source: *mut c_void, szText: *const c_char, xPos: i32, yPos: i32, fontName: *const c_char, fontSize: f64, colorRGB: i32, renderMode: i32, scaling: f64, charSpacing: f64, wordSpacing: f64, strokeWidth: i32, Flags: u32) -> i32 {
        unsafe { return (self.DTWAIN_AddPDFTextAFunc)(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);  }
    }

    pub fn DTWAIN_AddPDFTextEx(&self, Source: *mut c_void, TextElement: *mut c_void, Flags: u32) -> i32 {
        unsafe { return (self.DTWAIN_AddPDFTextExFunc)(Source, TextElement, Flags);  }
    }

    pub fn DTWAIN_AddPDFTextW(&self, Source: *mut c_void, szText: *const u16, xPos: i32, yPos: i32, fontName: *const u16, fontSize: f64, colorRGB: i32, renderMode: i32, scaling: f64, charSpacing: f64, wordSpacing: f64, strokeWidth: i32, Flags: u32) -> i32 {
        unsafe { return (self.DTWAIN_AddPDFTextWFunc)(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags);  }
    }

    pub fn DTWAIN_AllocateMemory(&self, memSize: u32) -> *mut c_void {
        unsafe { return (self.DTWAIN_AllocateMemoryFunc)(memSize);  }
    }

    pub fn DTWAIN_AllocateMemory64(&self, memSize: u64) -> *mut c_void {
        unsafe { return (self.DTWAIN_AllocateMemory64Func)(memSize);  }
    }

    pub fn DTWAIN_AllocateMemoryEx(&self, memSize: u32) -> *mut c_void {
        unsafe { return (self.DTWAIN_AllocateMemoryExFunc)(memSize);  }
    }

    pub fn DTWAIN_AppHandlesExceptions(&self, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_AppHandlesExceptionsFunc)(bSet);  }
    }

    pub fn DTWAIN_ArrayANSIStringToFloat(&self, StringArray: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayANSIStringToFloatFunc)(StringArray);  }
    }

    pub fn DTWAIN_ArrayAdd(&self, pArray: *mut c_void, pVariant: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFunc)(pArray, pVariant);  }
    }

    pub fn DTWAIN_ArrayAddANSIString(&self, pArray: *mut c_void, Val: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddANSIStringFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddANSIStringN(&self, pArray: *mut c_void, Val: *const c_char, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddANSIStringNFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddFloat(&self, pArray: *mut c_void, Val: f64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddFloatN(&self, pArray: *mut c_void, Val: f64, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatNFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddFloatString(&self, pArray: *mut c_void, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatStringFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddFloatStringA(&self, pArray: *mut c_void, Val: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatStringAFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddFloatStringN(&self, pArray: *mut c_void, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatStringNFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddFloatStringNA(&self, pArray: *mut c_void, Val: *const c_char, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatStringNAFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddFloatStringNW(&self, pArray: *mut c_void, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatStringNWFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddFloatStringW(&self, pArray: *mut c_void, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFloatStringWFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddFrame(&self, pArray: *mut c_void, frame: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFrameFunc)(pArray, frame);  }
    }

    pub fn DTWAIN_ArrayAddFrameN(&self, pArray: *mut c_void, frame: *mut c_void, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddFrameNFunc)(pArray, frame, num);  }
    }

    pub fn DTWAIN_ArrayAddLong(&self, pArray: *mut c_void, Val: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddLongFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddLong64(&self, pArray: *mut c_void, Val: i64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddLong64Func)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddLong64N(&self, pArray: *mut c_void, Val: i64, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddLong64NFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddLongN(&self, pArray: *mut c_void, Val: i32, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddLongNFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddN(&self, pArray: *mut c_void, pVariant: *mut c_void, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddNFunc)(pArray, pVariant, num);  }
    }

    pub fn DTWAIN_ArrayAddString(&self, pArray: *mut c_void, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddStringFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddStringA(&self, pArray: *mut c_void, Val: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddStringAFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddStringN(&self, pArray: *mut c_void, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddStringNFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddStringNA(&self, pArray: *mut c_void, Val: *const c_char, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddStringNAFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddStringNW(&self, pArray: *mut c_void, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddStringNWFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayAddStringW(&self, pArray: *mut c_void, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddStringWFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddWideString(&self, pArray: *mut c_void, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddWideStringFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayAddWideStringN(&self, pArray: *mut c_void, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayAddWideStringNFunc)(pArray, Val, num);  }
    }

    pub fn DTWAIN_ArrayConvertFix32ToFloat(&self, Fix32Array: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayConvertFix32ToFloatFunc)(Fix32Array);  }
    }

    pub fn DTWAIN_ArrayConvertFloatToFix32(&self, FloatArray: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayConvertFloatToFix32Func)(FloatArray);  }
    }

    pub fn DTWAIN_ArrayCopy(&self, Source: *mut c_void, Dest: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayCopyFunc)(Source, Dest);  }
    }

    pub fn DTWAIN_ArrayCreate(&self, nEnumType: i32, nInitialSize: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayCreateFunc)(nEnumType, nInitialSize);  }
    }

    pub fn DTWAIN_ArrayCreateCopy(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayCreateCopyFunc)(Source);  }
    }

    pub fn DTWAIN_ArrayCreateFromCap(&self, Source: *mut c_void, lCapType: i32, lSize: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayCreateFromCapFunc)(Source, lCapType, lSize);  }
    }

    pub fn DTWAIN_ArrayCreateFromLong64s(&self, pCArray: *mut i64, nSize: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayCreateFromLong64sFunc)(pCArray, nSize);  }
    }

    pub fn DTWAIN_ArrayCreateFromLongs(&self, pCArray: *mut i32, nSize: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayCreateFromLongsFunc)(pCArray, nSize);  }
    }

    pub fn DTWAIN_ArrayCreateFromReals(&self, nSize: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayCreateFromRealsFunc)(nSize);  }
    }

    pub fn DTWAIN_ArrayDestroy(&self, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayDestroyFunc)(pArray);  }
    }

    pub fn DTWAIN_ArrayDestroyFrames(&self, FrameArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayDestroyFramesFunc)(FrameArray);  }
    }

    pub fn DTWAIN_ArrayFind(&self, pArray: *mut c_void, pVariant: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindFunc)(pArray, pVariant);  }
    }

    pub fn DTWAIN_ArrayFindANSIString(&self, pArray: *mut c_void, pString: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindANSIStringFunc)(pArray, pString);  }
    }

    pub fn DTWAIN_ArrayFindFloat(&self, pArray: *mut c_void, Val: f64, Tolerance: f64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindFloatFunc)(pArray, Val, Tolerance);  }
    }

    pub fn DTWAIN_ArrayFindFloatString(&self, pArray: *mut c_void, Val: *const u16, Tolerance: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindFloatStringFunc)(pArray, Val, Tolerance);  }
    }

    pub fn DTWAIN_ArrayFindFloatStringA(&self, pArray: *mut c_void, Val: *const c_char, Tolerance: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindFloatStringAFunc)(pArray, Val, Tolerance);  }
    }

    pub fn DTWAIN_ArrayFindFloatStringW(&self, pArray: *mut c_void, Val: *const u16, Tolerance: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindFloatStringWFunc)(pArray, Val, Tolerance);  }
    }

    pub fn DTWAIN_ArrayFindLong(&self, pArray: *mut c_void, Val: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindLongFunc)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayFindLong64(&self, pArray: *mut c_void, Val: i64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindLong64Func)(pArray, Val);  }
    }

    pub fn DTWAIN_ArrayFindString(&self, pArray: *mut c_void, pString: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindStringFunc)(pArray, pString);  }
    }

    pub fn DTWAIN_ArrayFindStringA(&self, pArray: *mut c_void, pString: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindStringAFunc)(pArray, pString);  }
    }

    pub fn DTWAIN_ArrayFindStringW(&self, pArray: *mut c_void, pString: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindStringWFunc)(pArray, pString);  }
    }

    pub fn DTWAIN_ArrayFindWideString(&self, pArray: *mut c_void, pString: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFindWideStringFunc)(pArray, pString);  }
    }

    pub fn DTWAIN_ArrayFix32GetAt(&self, aFix32: *mut c_void, lPos: i32, Whole: *mut i32, Frac: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFix32GetAtFunc)(aFix32, lPos, Whole, Frac);  }
    }

    pub fn DTWAIN_ArrayFix32SetAt(&self, aFix32: *mut c_void, lPos: i32, Whole: i32, Frac: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayFix32SetAtFunc)(aFix32, lPos, Whole, Frac);  }
    }

    pub fn DTWAIN_ArrayFloatToANSIString(&self, FloatArray: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayFloatToANSIStringFunc)(FloatArray);  }
    }

    pub fn DTWAIN_ArrayFloatToString(&self, FloatArray: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayFloatToStringFunc)(FloatArray);  }
    }

    pub fn DTWAIN_ArrayFloatToWideString(&self, FloatArray: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayFloatToWideStringFunc)(FloatArray);  }
    }

    pub fn DTWAIN_ArrayGetAt(&self, pArray: *mut c_void, nWhere: i32, pVariant: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFunc)(pArray, nWhere, pVariant);  }
    }

    pub fn DTWAIN_ArrayGetAtANSIString(&self, pArray: *mut c_void, nWhere: i32, pStr: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtANSIStringFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArrayGetAtANSIStringPtr(&self, pArray: *mut c_void, nWhere: i32) -> *const c_char {
        unsafe { return (self.DTWAIN_ArrayGetAtANSIStringPtrFunc)(pArray, nWhere);  }
    }

    pub fn DTWAIN_ArrayGetAtFloat(&self, pArray: *mut c_void, nWhere: i32, pVal: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFloatFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayGetAtFloatString(&self, pArray: *mut c_void, nWhere: i32, Val: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFloatStringFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArrayGetAtFloatStringA(&self, pArray: *mut c_void, nWhere: i32, Val: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFloatStringAFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArrayGetAtFloatStringW(&self, pArray: *mut c_void, nWhere: i32, Val: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFloatStringWFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArrayGetAtFrame(&self, FrameArray: *mut c_void, nWhere: i32, pleft: *mut f64, ptop: *mut f64, pright: *mut f64, pbottom: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFrameFunc)(FrameArray, nWhere, pleft, ptop, pright, pbottom);  }
    }

    pub fn DTWAIN_ArrayGetAtFrameEx(&self, FrameArray: *mut c_void, nWhere: i32, Frame: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFrameExFunc)(FrameArray, nWhere, Frame);  }
    }

    pub fn DTWAIN_ArrayGetAtFrameString(&self, FrameArray: *mut c_void, nWhere: i32, left: *mut u16, top: *mut u16, right: *mut u16, bottom: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFrameStringFunc)(FrameArray, nWhere, left, top, right, bottom);  }
    }

    pub fn DTWAIN_ArrayGetAtFrameStringA(&self, FrameArray: *mut c_void, nWhere: i32, left: *mut c_char, top: *mut c_char, right: *mut c_char, bottom: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFrameStringAFunc)(FrameArray, nWhere, left, top, right, bottom);  }
    }

    pub fn DTWAIN_ArrayGetAtFrameStringW(&self, FrameArray: *mut c_void, nWhere: i32, left: *mut u16, top: *mut u16, right: *mut u16, bottom: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtFrameStringWFunc)(FrameArray, nWhere, left, top, right, bottom);  }
    }

    pub fn DTWAIN_ArrayGetAtLong(&self, pArray: *mut c_void, nWhere: i32, pVal: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtLongFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayGetAtLong64(&self, pArray: *mut c_void, nWhere: i32, pVal: *mut i64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtLong64Func)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayGetAtSource(&self, pArray: *mut c_void, nWhere: i32, ppSource: *mut *const ()) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtSourceFunc)(pArray, nWhere, ppSource);  }
    }

    pub fn DTWAIN_ArrayGetAtString(&self, pArray: *mut c_void, nWhere: i32, pStr: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtStringFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArrayGetAtStringA(&self, pArray: *mut c_void, nWhere: i32, pStr: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtStringAFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArrayGetAtStringPtr(&self, pArray: *mut c_void, nWhere: i32) -> *const u16 {
        unsafe { return (self.DTWAIN_ArrayGetAtStringPtrFunc)(pArray, nWhere);  }
    }

    pub fn DTWAIN_ArrayGetAtStringW(&self, pArray: *mut c_void, nWhere: i32, pStr: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtStringWFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArrayGetAtWideString(&self, pArray: *mut c_void, nWhere: i32, pStr: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetAtWideStringFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArrayGetAtWideStringPtr(&self, pArray: *mut c_void, nWhere: i32) -> *const u16 {
        unsafe { return (self.DTWAIN_ArrayGetAtWideStringPtrFunc)(pArray, nWhere);  }
    }

    pub fn DTWAIN_ArrayGetBuffer(&self, pArray: *mut c_void, nPos: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayGetBufferFunc)(pArray, nPos);  }
    }

    pub fn DTWAIN_ArrayGetCapValues(&self, Source: *mut c_void, lCap: i32, lGetType: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayGetCapValuesFunc)(Source, lCap, lGetType);  }
    }

    pub fn DTWAIN_ArrayGetCapValuesEx(&self, Source: *mut c_void, lCap: i32, lGetType: i32, lContainerType: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayGetCapValuesExFunc)(Source, lCap, lGetType, lContainerType);  }
    }

    pub fn DTWAIN_ArrayGetCapValuesEx2(&self, Source: *mut c_void, lCap: i32, lGetType: i32, lContainerType: i32, nDataType: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayGetCapValuesEx2Func)(Source, lCap, lGetType, lContainerType, nDataType);  }
    }

    pub fn DTWAIN_ArrayGetCount(&self, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetCountFunc)(pArray);  }
    }

    pub fn DTWAIN_ArrayGetMaxStringLength(&self, a: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetMaxStringLengthFunc)(a);  }
    }

    pub fn DTWAIN_ArrayGetSourceAt(&self, pArray: *mut c_void, nWhere: i32, ppSource: *mut *const ()) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetSourceAtFunc)(pArray, nWhere, ppSource);  }
    }

    pub fn DTWAIN_ArrayGetStringLength(&self, a: *mut c_void, nWhichString: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetStringLengthFunc)(a, nWhichString);  }
    }

    pub fn DTWAIN_ArrayGetType(&self, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayGetTypeFunc)(pArray);  }
    }

    pub fn DTWAIN_ArrayInit(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayInitFunc)();  }
    }

    pub fn DTWAIN_ArrayInsertAt(&self, pArray: *mut c_void, nWhere: i32, pVariant: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFunc)(pArray, nWhere, pVariant);  }
    }

    pub fn DTWAIN_ArrayInsertAtANSIString(&self, pArray: *mut c_void, nWhere: i32, pVal: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtANSIStringFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayInsertAtANSIStringN(&self, pArray: *mut c_void, nWhere: i32, Val: *const c_char, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtANSIStringNFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloat(&self, pArray: *mut c_void, nWhere: i32, pVal: f64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloatN(&self, pArray: *mut c_void, nWhere: i32, Val: f64, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatNFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloatString(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatStringFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloatStringA(&self, pArray: *mut c_void, nWhere: i32, Val: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatStringAFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloatStringN(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatStringNFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloatStringNA(&self, pArray: *mut c_void, nWhere: i32, Val: *const c_char, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatStringNAFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloatStringNW(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatStringNWFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtFloatStringW(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFloatStringWFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArrayInsertAtFrame(&self, pArray: *mut c_void, nWhere: i32, frame: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFrameFunc)(pArray, nWhere, frame);  }
    }

    pub fn DTWAIN_ArrayInsertAtFrameN(&self, pArray: *mut c_void, nWhere: i32, frame: *mut c_void, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtFrameNFunc)(pArray, nWhere, frame, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtLong(&self, pArray: *mut c_void, nWhere: i32, pVal: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtLongFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayInsertAtLong64(&self, pArray: *mut c_void, nWhere: i32, Val: i64) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtLong64Func)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArrayInsertAtLong64N(&self, pArray: *mut c_void, nWhere: i32, Val: i64, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtLong64NFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtLongN(&self, pArray: *mut c_void, nWhere: i32, pVal: i32, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtLongNFunc)(pArray, nWhere, pVal, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtN(&self, pArray: *mut c_void, nWhere: i32, pVariant: *mut c_void, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtNFunc)(pArray, nWhere, pVariant, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtString(&self, pArray: *mut c_void, nWhere: i32, pVal: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtStringFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayInsertAtStringA(&self, pArray: *mut c_void, nWhere: i32, pVal: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtStringAFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayInsertAtStringN(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtStringNFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtStringNA(&self, pArray: *mut c_void, nWhere: i32, Val: *const c_char, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtStringNAFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtStringNW(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtStringNWFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayInsertAtStringW(&self, pArray: *mut c_void, nWhere: i32, pVal: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtStringWFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayInsertAtWideString(&self, pArray: *mut c_void, nWhere: i32, pVal: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtWideStringFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArrayInsertAtWideStringN(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayInsertAtWideStringNFunc)(pArray, nWhere, Val, num);  }
    }

    pub fn DTWAIN_ArrayRemoveAll(&self, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArrayRemoveAllFunc)(pArray);  }
    }

    pub fn DTWAIN_ArrayRemoveAt(&self, pArray: *mut c_void, nWhere: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayRemoveAtFunc)(pArray, nWhere);  }
    }

    pub fn DTWAIN_ArrayRemoveAtN(&self, pArray: *mut c_void, nWhere: i32, num: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayRemoveAtNFunc)(pArray, nWhere, num);  }
    }

    pub fn DTWAIN_ArrayResize(&self, pArray: *mut c_void, NewSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArrayResizeFunc)(pArray, NewSize);  }
    }

    pub fn DTWAIN_ArraySetAt(&self, pArray: *mut c_void, lPos: i32, pVariant: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFunc)(pArray, lPos, pVariant);  }
    }

    pub fn DTWAIN_ArraySetAtANSIString(&self, pArray: *mut c_void, nWhere: i32, pStr: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtANSIStringFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArraySetAtFloat(&self, pArray: *mut c_void, nWhere: i32, pVal: f64) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFloatFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArraySetAtFloatString(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFloatStringFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArraySetAtFloatStringA(&self, pArray: *mut c_void, nWhere: i32, Val: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFloatStringAFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArraySetAtFloatStringW(&self, pArray: *mut c_void, nWhere: i32, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFloatStringWFunc)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArraySetAtFrame(&self, FrameArray: *mut c_void, nWhere: i32, left: f64, top: f64, right: f64, bottom: f64) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFrameFunc)(FrameArray, nWhere, left, top, right, bottom);  }
    }

    pub fn DTWAIN_ArraySetAtFrameEx(&self, FrameArray: *mut c_void, nWhere: i32, Frame: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFrameExFunc)(FrameArray, nWhere, Frame);  }
    }

    pub fn DTWAIN_ArraySetAtFrameString(&self, FrameArray: *mut c_void, nWhere: i32, left: *const u16, top: *const u16, right: *const u16, bottom: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFrameStringFunc)(FrameArray, nWhere, left, top, right, bottom);  }
    }

    pub fn DTWAIN_ArraySetAtFrameStringA(&self, FrameArray: *mut c_void, nWhere: i32, left: *const c_char, top: *const c_char, right: *const c_char, bottom: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFrameStringAFunc)(FrameArray, nWhere, left, top, right, bottom);  }
    }

    pub fn DTWAIN_ArraySetAtFrameStringW(&self, FrameArray: *mut c_void, nWhere: i32, left: *const u16, top: *const u16, right: *const u16, bottom: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtFrameStringWFunc)(FrameArray, nWhere, left, top, right, bottom);  }
    }

    pub fn DTWAIN_ArraySetAtLong(&self, pArray: *mut c_void, nWhere: i32, pVal: i32) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtLongFunc)(pArray, nWhere, pVal);  }
    }

    pub fn DTWAIN_ArraySetAtLong64(&self, pArray: *mut c_void, nWhere: i32, Val: i64) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtLong64Func)(pArray, nWhere, Val);  }
    }

    pub fn DTWAIN_ArraySetAtString(&self, pArray: *mut c_void, nWhere: i32, pStr: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtStringFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArraySetAtStringA(&self, pArray: *mut c_void, nWhere: i32, pStr: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtStringAFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArraySetAtStringW(&self, pArray: *mut c_void, nWhere: i32, pStr: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtStringWFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArraySetAtWideString(&self, pArray: *mut c_void, nWhere: i32, pStr: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_ArraySetAtWideStringFunc)(pArray, nWhere, pStr);  }
    }

    pub fn DTWAIN_ArrayStringToFloat(&self, StringArray: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayStringToFloatFunc)(StringArray);  }
    }

    pub fn DTWAIN_ArrayWideStringToFloat(&self, StringArray: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ArrayWideStringToFloatFunc)(StringArray);  }
    }

    pub fn DTWAIN_CallCallback(&self, wParam: i32, lParam: i32, UserData: i32) -> i32 {
        unsafe { return (self.DTWAIN_CallCallbackFunc)(wParam, lParam, UserData);  }
    }

    pub fn DTWAIN_CallCallback64(&self, wParam: i32, lParam: i32, UserData: i64) -> i32 {
        unsafe { return (self.DTWAIN_CallCallback64Func)(wParam, lParam, UserData);  }
    }

    pub fn DTWAIN_CallDSMProc(&self, AppID: *mut c_void, SourceId: *mut c_void, lDG: i32, lDAT: i32, lMSG: i32, pData: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_CallDSMProcFunc)(AppID, SourceId, lDG, lDAT, lMSG, pData);  }
    }

    pub fn DTWAIN_CheckHandles(&self, bCheck: i32) -> i32 {
        unsafe { return (self.DTWAIN_CheckHandlesFunc)(bCheck);  }
    }

    pub fn DTWAIN_ClearBuffers(&self, Source: *mut c_void, ClearBuffer: i32) -> i32 {
        unsafe { return (self.DTWAIN_ClearBuffersFunc)(Source, ClearBuffer);  }
    }

    pub fn DTWAIN_ClearErrorBuffer(&self) -> i32 {
        unsafe { return (self.DTWAIN_ClearErrorBufferFunc)();  }
    }

    pub fn DTWAIN_ClearPDFText(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ClearPDFTextFunc)(Source);  }
    }

    pub fn DTWAIN_ClearPage(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ClearPageFunc)(Source);  }
    }

    pub fn DTWAIN_CloseSource(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_CloseSourceFunc)(Source);  }
    }

    pub fn DTWAIN_CloseSourceUI(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_CloseSourceUIFunc)(Source);  }
    }

    pub fn DTWAIN_ConvertDIBToBitmap(&self, hDib: *mut c_void, hPalette: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_ConvertDIBToBitmapFunc)(hDib, hPalette);  }
    }

    pub fn DTWAIN_ConvertDIBToFullBitmap(&self, hDib: *mut c_void, isBMP: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_ConvertDIBToFullBitmapFunc)(hDib, isBMP);  }
    }

    pub fn DTWAIN_ConvertToAPIString(&self, lpOrigString: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_ConvertToAPIStringFunc)(lpOrigString);  }
    }

    pub fn DTWAIN_ConvertToAPIStringA(&self, lpOrigString: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_ConvertToAPIStringAFunc)(lpOrigString);  }
    }

    pub fn DTWAIN_ConvertToAPIStringEx(&self, lpOrigString: *const u16, lpOutString: *mut u16, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_ConvertToAPIStringExFunc)(lpOrigString, lpOutString, nSize);  }
    }

    pub fn DTWAIN_ConvertToAPIStringExA(&self, lpOrigString: *const c_char, lpOutString: *mut c_char, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_ConvertToAPIStringExAFunc)(lpOrigString, lpOutString, nSize);  }
    }

    pub fn DTWAIN_ConvertToAPIStringExW(&self, lpOrigString: *const u16, lpOutString: *mut u16, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_ConvertToAPIStringExWFunc)(lpOrigString, lpOutString, nSize);  }
    }

    pub fn DTWAIN_ConvertToAPIStringW(&self, lpOrigString: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_ConvertToAPIStringWFunc)(lpOrigString);  }
    }

    pub fn DTWAIN_CreateAcquisitionArray(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_CreateAcquisitionArrayFunc)();  }
    }

    pub fn DTWAIN_CreatePDFTextElement(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_CreatePDFTextElementFunc)(Source);  }
    }

    pub fn DTWAIN_DeleteDIB(&self, hDib: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_DeleteDIBFunc)(hDib);  }
    }

    pub fn DTWAIN_DestroyAcquisitionArray(&self, aAcq: *mut c_void, bDestroyData: i32) -> i32 {
        unsafe { return (self.DTWAIN_DestroyAcquisitionArrayFunc)(aAcq, bDestroyData);  }
    }

    pub fn DTWAIN_DestroyPDFTextElement(&self, TextElement: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_DestroyPDFTextElementFunc)(TextElement);  }
    }

    pub fn DTWAIN_DisableAppWindow(&self, hWnd: *const c_void, bDisable: i32) -> i32 {
        unsafe { return (self.DTWAIN_DisableAppWindowFunc)(hWnd, bDisable);  }
    }

    pub fn DTWAIN_EnableAutoBorderDetect(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableAutoBorderDetectFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableAutoBright(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableAutoBrightFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_EnableAutoDeskew(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableAutoDeskewFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableAutoFeed(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableAutoFeedFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_EnableAutoRotate(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableAutoRotateFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_EnableAutoScan(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableAutoScanFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableAutomaticSenseMedium(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableAutomaticSenseMediumFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_EnableDuplex(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableDuplexFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableFeeder(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableFeederFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_EnableIndicator(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableIndicatorFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableJobFileHandling(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableJobFileHandlingFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_EnableLamp(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableLampFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableMsgNotify(&self, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableMsgNotifyFunc)(bSet);  }
    }

    pub fn DTWAIN_EnablePatchDetect(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnablePatchDetectFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnablePeekMessageLoop(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnablePeekMessageLoopFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_EnablePrinter(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnablePrinterFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableThumbnail(&self, Source: *mut c_void, bEnable: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableThumbnailFunc)(Source, bEnable);  }
    }

    pub fn DTWAIN_EnableTripletsNotify(&self, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnableTripletsNotifyFunc)(bSet);  }
    }

    pub fn DTWAIN_EndThread(&self, DLLHandle: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EndThreadFunc)(DLLHandle);  }
    }

    pub fn DTWAIN_EndTwainSession(&self) -> i32 {
        unsafe { return (self.DTWAIN_EndTwainSessionFunc)();  }
    }

    pub fn DTWAIN_EnumAlarmVolumes(&self, Source: *mut c_void, pArray: *mut *mut c_void, expandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumAlarmVolumesFunc)(Source, pArray, expandIfRange);  }
    }

    pub fn DTWAIN_EnumAlarmVolumesEx(&self, Source: *mut c_void, expandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumAlarmVolumesExFunc)(Source, expandIfRange);  }
    }

    pub fn DTWAIN_EnumAlarms(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumAlarmsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumAlarmsEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumAlarmsExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumAudioXferMechs(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumAudioXferMechsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumAudioXferMechsEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumAudioXferMechsExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumAutoFeedValues(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumAutoFeedValuesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumAutoFeedValuesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumAutoFeedValuesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumAutomaticCaptures(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumAutomaticCapturesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumAutomaticCapturesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumAutomaticCapturesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumAutomaticSenseMedium(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumAutomaticSenseMediumFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumAutomaticSenseMediumEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumAutomaticSenseMediumExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumBitDepths(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumBitDepthsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumBitDepthsEx(&self, Source: *mut c_void, PixelType: i32, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumBitDepthsExFunc)(Source, PixelType, pArray);  }
    }

    pub fn DTWAIN_EnumBitDepthsEx2(&self, Source: *mut c_void, PixelType: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumBitDepthsEx2Func)(Source, PixelType);  }
    }

    pub fn DTWAIN_EnumBottomCameras(&self, Source: *mut c_void, Cameras: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumBottomCamerasFunc)(Source, Cameras);  }
    }

    pub fn DTWAIN_EnumBottomCamerasEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumBottomCamerasExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumBrightnessValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumBrightnessValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumBrightnessValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumBrightnessValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumCameras(&self, Source: *mut c_void, Cameras: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumCamerasFunc)(Source, Cameras);  }
    }

    pub fn DTWAIN_EnumCamerasEx(&self, Source: *mut c_void, nWhichCamera: i32, Cameras: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumCamerasExFunc)(Source, nWhichCamera, Cameras);  }
    }

    pub fn DTWAIN_EnumCamerasEx2(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumCamerasEx2Func)(Source);  }
    }

    pub fn DTWAIN_EnumCamerasEx3(&self, Source: *mut c_void, nWhichCamera: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumCamerasEx3Func)(Source, nWhichCamera);  }
    }

    pub fn DTWAIN_EnumCompressionTypes(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumCompressionTypesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumCompressionTypesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumCompressionTypesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumCompressionTypesEx2(&self, Source: *mut c_void, lFileType: i32, bUseBufferedMode: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumCompressionTypesEx2Func)(Source, lFileType, bUseBufferedMode);  }
    }

    pub fn DTWAIN_EnumContrastValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumContrastValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumContrastValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumContrastValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumCustomCaps(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumCustomCapsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumCustomCapsEx2(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumCustomCapsEx2Func)(Source);  }
    }

    pub fn DTWAIN_EnumDoubleFeedDetectLengths(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumDoubleFeedDetectLengthsFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumDoubleFeedDetectLengthsEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumDoubleFeedDetectLengthsExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumDoubleFeedDetectValues(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumDoubleFeedDetectValuesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumDoubleFeedDetectValuesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumDoubleFeedDetectValuesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumExtImageInfoTypes(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumExtImageInfoTypesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumExtImageInfoTypesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumExtImageInfoTypesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumExtendedCaps(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumExtendedCapsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumExtendedCapsEx(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumExtendedCapsExFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumExtendedCapsEx2(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumExtendedCapsEx2Func)(Source);  }
    }

    pub fn DTWAIN_EnumFileTypeBitsPerPixel(&self, FileType: i32, Array: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumFileTypeBitsPerPixelFunc)(FileType, Array);  }
    }

    pub fn DTWAIN_EnumFileXferFormats(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumFileXferFormatsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumFileXferFormatsEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumFileXferFormatsExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumHalftones(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumHalftonesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumHalftonesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumHalftonesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumHighlightValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumHighlightValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumHighlightValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumHighlightValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumJobControls(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumJobControlsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumJobControlsEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumJobControlsExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumLightPaths(&self, Source: *mut c_void, LightPath: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumLightPathsFunc)(Source, LightPath);  }
    }

    pub fn DTWAIN_EnumLightPathsEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumLightPathsExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumLightSources(&self, Source: *mut c_void, LightSources: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumLightSourcesFunc)(Source, LightSources);  }
    }

    pub fn DTWAIN_EnumLightSourcesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumLightSourcesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumMaxBuffers(&self, Source: *mut c_void, pMaxBufs: *mut *mut c_void, bExpandRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumMaxBuffersFunc)(Source, pMaxBufs, bExpandRange);  }
    }

    pub fn DTWAIN_EnumMaxBuffersEx(&self, Source: *mut c_void, bExpandRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumMaxBuffersExFunc)(Source, bExpandRange);  }
    }

    pub fn DTWAIN_EnumNoiseFilters(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumNoiseFiltersFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumNoiseFiltersEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumNoiseFiltersExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumOCRInterfaces(&self, OCRInterfaces: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumOCRInterfacesFunc)(OCRInterfaces);  }
    }

    pub fn DTWAIN_EnumOCRSupportedCaps(&self, Engine: *mut c_void, SupportedCaps: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumOCRSupportedCapsFunc)(Engine, SupportedCaps);  }
    }

    pub fn DTWAIN_EnumOrientations(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumOrientationsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumOrientationsEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumOrientationsExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumOverscanValues(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumOverscanValuesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumOverscanValuesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumOverscanValuesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPaperSizes(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPaperSizesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPaperSizesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPaperSizesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPatchCodes(&self, Source: *mut c_void, PCodes: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPatchCodesFunc)(Source, PCodes);  }
    }

    pub fn DTWAIN_EnumPatchCodesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPatchCodesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPatchMaxPriorities(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPatchMaxPrioritiesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPatchMaxPrioritiesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPatchMaxPrioritiesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPatchMaxRetries(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPatchMaxRetriesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPatchMaxRetriesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPatchMaxRetriesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPatchPriorities(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPatchPrioritiesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPatchPrioritiesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPatchPrioritiesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPatchSearchModes(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPatchSearchModesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPatchSearchModesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPatchSearchModesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPatchTimeOutValues(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPatchTimeOutValuesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPatchTimeOutValuesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPatchTimeOutValuesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPixelTypes(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPixelTypesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPixelTypesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPixelTypesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumPrinterStringModes(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumPrinterStringModesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumPrinterStringModesEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumPrinterStringModesExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumResolutionValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumResolutionValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumResolutionValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumResolutionValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumShadowValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumShadowValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumShadowValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumShadowValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumSourceUnits(&self, Source: *mut c_void, lpArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumSourceUnitsFunc)(Source, lpArray);  }
    }

    pub fn DTWAIN_EnumSourceUnitsEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumSourceUnitsExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumSourceValues(&self, Source: *mut c_void, capName: *const u16, values: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumSourceValuesFunc)(Source, capName, values, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumSourceValuesA(&self, Source: *mut c_void, capName: *const c_char, values: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumSourceValuesAFunc)(Source, capName, values, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumSourceValuesW(&self, Source: *mut c_void, capName: *const u16, values: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumSourceValuesWFunc)(Source, capName, values, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumSources(&self, lpArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumSourcesFunc)(lpArray);  }
    }

    pub fn DTWAIN_EnumSourcesEx(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumSourcesExFunc)();  }
    }

    pub fn DTWAIN_EnumSupportedCaps(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumSupportedCapsFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumSupportedCapsEx(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumSupportedCapsExFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumSupportedCapsEx2(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumSupportedCapsEx2Func)(Source);  }
    }

    pub fn DTWAIN_EnumSupportedExtImageInfo(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumSupportedExtImageInfoFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumSupportedExtImageInfoEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumSupportedExtImageInfoExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumSupportedFileTypes(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumSupportedFileTypesFunc)();  }
    }

    pub fn DTWAIN_EnumSupportedMultiPageFileTypes(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumSupportedMultiPageFileTypesFunc)();  }
    }

    pub fn DTWAIN_EnumSupportedSinglePageFileTypes(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumSupportedSinglePageFileTypesFunc)();  }
    }

    pub fn DTWAIN_EnumThresholdValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumThresholdValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumThresholdValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumThresholdValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumTopCameras(&self, Source: *mut c_void, Cameras: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumTopCamerasFunc)(Source, Cameras);  }
    }

    pub fn DTWAIN_EnumTopCamerasEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumTopCamerasExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumTwainPrinters(&self, Source: *mut c_void, lpAvailPrinters: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumTwainPrintersFunc)(Source, lpAvailPrinters);  }
    }

    pub fn DTWAIN_EnumTwainPrintersArray(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_EnumTwainPrintersArrayFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_EnumTwainPrintersArrayEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumTwainPrintersArrayExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumTwainPrintersEx(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumTwainPrintersExFunc)(Source);  }
    }

    pub fn DTWAIN_EnumXResolutionValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumXResolutionValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumXResolutionValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumXResolutionValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumYResolutionValues(&self, Source: *mut c_void, pArray: *mut *mut c_void, bExpandIfRange: i32) -> i32 {
        unsafe { return (self.DTWAIN_EnumYResolutionValuesFunc)(Source, pArray, bExpandIfRange);  }
    }

    pub fn DTWAIN_EnumYResolutionValuesEx(&self, Source: *mut c_void, bExpandIfRange: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_EnumYResolutionValuesExFunc)(Source, bExpandIfRange);  }
    }

    pub fn DTWAIN_ExecuteOCR(&self, Engine: *mut c_void, szFileName: *const u16, nStartPage: i32, nEndPage: i32) -> i32 {
        unsafe { return (self.DTWAIN_ExecuteOCRFunc)(Engine, szFileName, nStartPage, nEndPage);  }
    }

    pub fn DTWAIN_ExecuteOCRA(&self, Engine: *mut c_void, szFileName: *const c_char, nStartPage: i32, nEndPage: i32) -> i32 {
        unsafe { return (self.DTWAIN_ExecuteOCRAFunc)(Engine, szFileName, nStartPage, nEndPage);  }
    }

    pub fn DTWAIN_ExecuteOCRW(&self, Engine: *mut c_void, szFileName: *const u16, nStartPage: i32, nEndPage: i32) -> i32 {
        unsafe { return (self.DTWAIN_ExecuteOCRWFunc)(Engine, szFileName, nStartPage, nEndPage);  }
    }

    pub fn DTWAIN_FeedPage(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FeedPageFunc)(Source);  }
    }

    pub fn DTWAIN_FlipBitmap(&self, hDib: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FlipBitmapFunc)(hDib);  }
    }

    pub fn DTWAIN_FlushAcquiredPages(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FlushAcquiredPagesFunc)(Source);  }
    }

    pub fn DTWAIN_ForceAcquireBitDepth(&self, Source: *mut c_void, BitDepth: i32) -> i32 {
        unsafe { return (self.DTWAIN_ForceAcquireBitDepthFunc)(Source, BitDepth);  }
    }

    pub fn DTWAIN_ForceScanOnNoUI(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_ForceScanOnNoUIFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_FrameCreate(&self, Left: f64, Top: f64, Right: f64, Bottom: f64) -> *mut c_void {
        unsafe { return (self.DTWAIN_FrameCreateFunc)(Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameCreateString(&self, Left: *const u16, Top: *const u16, Right: *const u16, Bottom: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_FrameCreateStringFunc)(Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameCreateStringA(&self, Left: *const c_char, Top: *const c_char, Right: *const c_char, Bottom: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_FrameCreateStringAFunc)(Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameCreateStringW(&self, Left: *const u16, Top: *const u16, Right: *const u16, Bottom: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_FrameCreateStringWFunc)(Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameDestroy(&self, Frame: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FrameDestroyFunc)(Frame);  }
    }

    pub fn DTWAIN_FrameGetAll(&self, Frame: *mut c_void, Left: *mut f64, Top: *mut f64, Right: *mut f64, Bottom: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetAllFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameGetAllString(&self, Frame: *mut c_void, Left: *mut u16, Top: *mut u16, Right: *mut u16, Bottom: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetAllStringFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameGetAllStringA(&self, Frame: *mut c_void, Left: *mut c_char, Top: *mut c_char, Right: *mut c_char, Bottom: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetAllStringAFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameGetAllStringW(&self, Frame: *mut c_void, Left: *mut u16, Top: *mut u16, Right: *mut u16, Bottom: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetAllStringWFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameGetValue(&self, Frame: *mut c_void, nWhich: i32, Value: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetValueFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FrameGetValueString(&self, Frame: *mut c_void, nWhich: i32, Value: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetValueStringFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FrameGetValueStringA(&self, Frame: *mut c_void, nWhich: i32, Value: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetValueStringAFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FrameGetValueStringW(&self, Frame: *mut c_void, nWhich: i32, Value: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameGetValueStringWFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FrameIsValid(&self, Frame: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FrameIsValidFunc)(Frame);  }
    }

    pub fn DTWAIN_FrameSetAll(&self, Frame: *mut c_void, Left: f64, Top: f64, Right: f64, Bottom: f64) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetAllFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameSetAllString(&self, Frame: *mut c_void, Left: *const u16, Top: *const u16, Right: *const u16, Bottom: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetAllStringFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameSetAllStringA(&self, Frame: *mut c_void, Left: *const c_char, Top: *const c_char, Right: *const c_char, Bottom: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetAllStringAFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameSetAllStringW(&self, Frame: *mut c_void, Left: *const u16, Top: *const u16, Right: *const u16, Bottom: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetAllStringWFunc)(Frame, Left, Top, Right, Bottom);  }
    }

    pub fn DTWAIN_FrameSetValue(&self, Frame: *mut c_void, nWhich: i32, Value: f64) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetValueFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FrameSetValueString(&self, Frame: *mut c_void, nWhich: i32, Value: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetValueStringFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FrameSetValueStringA(&self, Frame: *mut c_void, nWhich: i32, Value: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetValueStringAFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FrameSetValueStringW(&self, Frame: *mut c_void, nWhich: i32, Value: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_FrameSetValueStringWFunc)(Frame, nWhich, Value);  }
    }

    pub fn DTWAIN_FreeExtImageInfo(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FreeExtImageInfoFunc)(Source);  }
    }

    pub fn DTWAIN_FreeMemory(&self, h: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FreeMemoryFunc)(h);  }
    }

    pub fn DTWAIN_FreeMemoryEx(&self, h: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_FreeMemoryExFunc)(h);  }
    }

    pub fn DTWAIN_GetAPIHandleStatus(&self, pHandle: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetAPIHandleStatusFunc)(pHandle);  }
    }

    pub fn DTWAIN_GetAcquireArea(&self, Source: *mut c_void, lGetType: i32, FloatEnum: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireAreaFunc)(Source, lGetType, FloatEnum);  }
    }

    pub fn DTWAIN_GetAcquireArea2(&self, Source: *mut c_void, left: *mut f64, top: *mut f64, right: *mut f64, bottom: *mut f64, lpUnit: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireArea2Func)(Source, left, top, right, bottom, lpUnit);  }
    }

    pub fn DTWAIN_GetAcquireArea2String(&self, Source: *mut c_void, left: *mut u16, top: *mut u16, right: *mut u16, bottom: *mut u16, Unit: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireArea2StringFunc)(Source, left, top, right, bottom, Unit);  }
    }

    pub fn DTWAIN_GetAcquireArea2StringA(&self, Source: *mut c_void, left: *mut c_char, top: *mut c_char, right: *mut c_char, bottom: *mut c_char, Unit: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireArea2StringAFunc)(Source, left, top, right, bottom, Unit);  }
    }

    pub fn DTWAIN_GetAcquireArea2StringW(&self, Source: *mut c_void, left: *mut u16, top: *mut u16, right: *mut u16, bottom: *mut u16, Unit: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireArea2StringWFunc)(Source, left, top, right, bottom, Unit);  }
    }

    pub fn DTWAIN_GetAcquireAreaEx(&self, Source: *mut c_void, lGetType: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetAcquireAreaExFunc)(Source, lGetType);  }
    }

    pub fn DTWAIN_GetAcquireMetrics(&self, source: *mut c_void, ImageCount: *mut i32, SheetCount: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireMetricsFunc)(source, ImageCount, SheetCount);  }
    }

    pub fn DTWAIN_GetAcquireStripBuffer(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetAcquireStripBufferFunc)(Source);  }
    }

    pub fn DTWAIN_GetAcquireStripData(&self, Source: *mut c_void, lpCompression: *mut i32, lpBytesPerRow: *mut u32, lpColumns: *mut u32, lpRows: *mut u32, XOffset: *mut u32, YOffset: *mut u32, lpBytesWritten: *mut u32) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireStripDataFunc)(Source, lpCompression, lpBytesPerRow, lpColumns, lpRows, XOffset, YOffset, lpBytesWritten);  }
    }

    pub fn DTWAIN_GetAcquireStripSizes(&self, Source: *mut c_void, lpMin: *mut u32, lpMax: *mut u32, lpPreferred: *mut u32) -> i32 {
        unsafe { return (self.DTWAIN_GetAcquireStripSizesFunc)(Source, lpMin, lpMax, lpPreferred);  }
    }

    pub fn DTWAIN_GetAcquiredImage(&self, aAcq: *mut c_void, nWhichAcq: i32, nWhichDib: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetAcquiredImageFunc)(aAcq, nWhichAcq, nWhichDib);  }
    }

    pub fn DTWAIN_GetAcquiredImageArray(&self, aAcq: *mut c_void, nWhichAcq: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetAcquiredImageArrayFunc)(aAcq, nWhichAcq);  }
    }

    pub fn DTWAIN_GetActiveDSMPath(&self, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetActiveDSMPathFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetActiveDSMPathA(&self, lpszBuffer: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetActiveDSMPathAFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetActiveDSMPathW(&self, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetActiveDSMPathWFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetActiveDSMVersionInfo(&self, szDLLInfo: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetActiveDSMVersionInfoFunc)(szDLLInfo, nMaxLen);  }
    }

    pub fn DTWAIN_GetActiveDSMVersionInfoA(&self, lpszBuffer: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetActiveDSMVersionInfoAFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetActiveDSMVersionInfoW(&self, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetActiveDSMVersionInfoWFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetAlarmVolume(&self, Source: *mut c_void, lpVolume: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetAlarmVolumeFunc)(Source, lpVolume);  }
    }

    pub fn DTWAIN_GetAllSourceDibs(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetAllSourceDibsFunc)(Source);  }
    }

    pub fn DTWAIN_GetAppInfo(&self, szVerStr: *mut u16, szManu: *mut u16, szProdFam: *mut u16, szProdName: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetAppInfoFunc)(szVerStr, szManu, szProdFam, szProdName);  }
    }

    pub fn DTWAIN_GetAppInfoA(&self, szVerStr: *mut c_char, szManu: *mut c_char, szProdFam: *mut c_char, szProdName: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetAppInfoAFunc)(szVerStr, szManu, szProdFam, szProdName);  }
    }

    pub fn DTWAIN_GetAppInfoW(&self, szVerStr: *mut u16, szManu: *mut u16, szProdFam: *mut u16, szProdName: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetAppInfoWFunc)(szVerStr, szManu, szProdFam, szProdName);  }
    }

    pub fn DTWAIN_GetAuthor(&self, Source: *mut c_void, szAuthor: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetAuthorFunc)(Source, szAuthor);  }
    }

    pub fn DTWAIN_GetAuthorA(&self, Source: *mut c_void, szAuthor: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetAuthorAFunc)(Source, szAuthor);  }
    }

    pub fn DTWAIN_GetAuthorW(&self, Source: *mut c_void, szAuthor: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetAuthorWFunc)(Source, szAuthor);  }
    }

    pub fn DTWAIN_GetBatteryMinutes(&self, Source: *mut c_void, lpMinutes: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetBatteryMinutesFunc)(Source, lpMinutes);  }
    }

    pub fn DTWAIN_GetBatteryPercent(&self, Source: *mut c_void, lpPercent: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetBatteryPercentFunc)(Source, lpPercent);  }
    }

    pub fn DTWAIN_GetBitDepth(&self, Source: *mut c_void, BitDepth: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetBitDepthFunc)(Source, BitDepth, bCurrent);  }
    }

    pub fn DTWAIN_GetBlankPageAutoDetection(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetBlankPageAutoDetectionFunc)(Source);  }
    }

    pub fn DTWAIN_GetBrightness(&self, Source: *mut c_void, Brightness: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetBrightnessFunc)(Source, Brightness);  }
    }

    pub fn DTWAIN_GetBrightnessString(&self, Source: *mut c_void, Brightness: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetBrightnessStringFunc)(Source, Brightness);  }
    }

    pub fn DTWAIN_GetBrightnessStringA(&self, Source: *mut c_void, Contrast: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetBrightnessStringAFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_GetBrightnessStringW(&self, Source: *mut c_void, Contrast: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetBrightnessStringWFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_GetBufferedTransferInfo(&self, Source: *mut c_void, Compression: *mut u32, BytesPerRow: *mut u32, Columns: *mut u32, Rows: *mut u32, XOffset: *mut u32, YOffset: *mut u32, Flags: *mut u32, BytesWritten: *mut u32, MemoryLength: *mut u32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetBufferedTransferInfoFunc)(Source, Compression, BytesPerRow, Columns, Rows, XOffset, YOffset, Flags, BytesWritten, MemoryLength);  }
    }

    pub fn DTWAIN_GetCallback(&self) -> DTWAIN_CALLBACK_PROC {
        unsafe { return (self.DTWAIN_GetCallbackFunc)();  }
    }

    pub fn DTWAIN_GetCallback64(&self) -> DTWAIN_CALLBACK_PROC64 {
        unsafe { return (self.DTWAIN_GetCallback64Func)();  }
    }

    pub fn DTWAIN_GetCapArrayType(&self, Source: *mut c_void, nCap: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCapArrayTypeFunc)(Source, nCap);  }
    }

    pub fn DTWAIN_GetCapContainer(&self, Source: *mut c_void, nCap: i32, lCapType: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCapContainerFunc)(Source, nCap, lCapType);  }
    }

    pub fn DTWAIN_GetCapContainerEx(&self, nCap: i32, bSetContainer: i32, ConTypes: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetCapContainerExFunc)(nCap, bSetContainer, ConTypes);  }
    }

    pub fn DTWAIN_GetCapContainerEx2(&self, nCap: i32, bSetContainer: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetCapContainerEx2Func)(nCap, bSetContainer);  }
    }

    pub fn DTWAIN_GetCapDataType(&self, Source: *mut c_void, nCap: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCapDataTypeFunc)(Source, nCap);  }
    }

    pub fn DTWAIN_GetCapFromName(&self, szName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetCapFromNameFunc)(szName);  }
    }

    pub fn DTWAIN_GetCapFromNameA(&self, szName: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetCapFromNameAFunc)(szName);  }
    }

    pub fn DTWAIN_GetCapFromNameW(&self, szName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetCapFromNameWFunc)(szName);  }
    }

    pub fn DTWAIN_GetCapOperations(&self, Source: *mut c_void, lCapability: i32, lpOps: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCapOperationsFunc)(Source, lCapability, lpOps);  }
    }

    pub fn DTWAIN_GetCapValues(&self, Source: *mut c_void, lCap: i32, lGetType: i32, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetCapValuesFunc)(Source, lCap, lGetType, pArray);  }
    }

    pub fn DTWAIN_GetCapValuesEx(&self, Source: *mut c_void, lCap: i32, lGetType: i32, lContainerType: i32, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetCapValuesExFunc)(Source, lCap, lGetType, lContainerType, pArray);  }
    }

    pub fn DTWAIN_GetCapValuesEx2(&self, Source: *mut c_void, lCap: i32, lGetType: i32, lContainerType: i32, nDataType: i32, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetCapValuesEx2Func)(Source, lCap, lGetType, lContainerType, nDataType, pArray);  }
    }

    pub fn DTWAIN_GetCaption(&self, Source: *mut c_void, Caption: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetCaptionFunc)(Source, Caption);  }
    }

    pub fn DTWAIN_GetCaptionA(&self, Source: *mut c_void, Caption: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetCaptionAFunc)(Source, Caption);  }
    }

    pub fn DTWAIN_GetCaptionW(&self, Source: *mut c_void, Caption: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetCaptionWFunc)(Source, Caption);  }
    }

    pub fn DTWAIN_GetCompressionSize(&self, Source: *mut c_void, lBytes: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCompressionSizeFunc)(Source, lBytes);  }
    }

    pub fn DTWAIN_GetCompressionType(&self, Source: *mut c_void, lpCompression: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCompressionTypeFunc)(Source, lpCompression, bCurrent);  }
    }

    pub fn DTWAIN_GetConditionCodeString(&self, lError: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetConditionCodeStringFunc)(lError, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetConditionCodeStringA(&self, lError: i32, lpszBuffer: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetConditionCodeStringAFunc)(lError, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetConditionCodeStringW(&self, lError: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetConditionCodeStringWFunc)(lError, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetContrast(&self, Source: *mut c_void, Contrast: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetContrastFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_GetContrastString(&self, Source: *mut c_void, Contrast: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetContrastStringFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_GetContrastStringA(&self, Source: *mut c_void, Contrast: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetContrastStringAFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_GetContrastStringW(&self, Source: *mut c_void, Contrast: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetContrastStringWFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_GetCountry(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetCountryFunc)();  }
    }

    pub fn DTWAIN_GetCurrentAcquiredImage(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetCurrentAcquiredImageFunc)(Source);  }
    }

    pub fn DTWAIN_GetCurrentFileName(&self, Source: *mut c_void, szName: *mut u16, MaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCurrentFileNameFunc)(Source, szName, MaxLen);  }
    }

    pub fn DTWAIN_GetCurrentFileNameA(&self, Source: *mut c_void, szName: *mut c_char, MaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCurrentFileNameAFunc)(Source, szName, MaxLen);  }
    }

    pub fn DTWAIN_GetCurrentFileNameW(&self, Source: *mut c_void, szName: *mut u16, MaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetCurrentFileNameWFunc)(Source, szName, MaxLen);  }
    }

    pub fn DTWAIN_GetCurrentPageNum(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetCurrentPageNumFunc)(Source);  }
    }

    pub fn DTWAIN_GetCurrentRetryCount(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetCurrentRetryCountFunc)(Source);  }
    }

    pub fn DTWAIN_GetCurrentTwainTriplet(&self, pAppID: *mut *mut c_void, pSourceID: *mut *mut c_void, lpDG: *mut i32, lpDAT: *mut i32, lpMsg: *mut i32, lpMemRef: *mut i64) -> i32 {
        unsafe { return (self.DTWAIN_GetCurrentTwainTripletFunc)(pAppID, pSourceID, lpDG, lpDAT, lpMsg, lpMemRef);  }
    }

    pub fn DTWAIN_GetCustomDSData(&self, Source: *mut c_void, Data: *mut u8, dSize: u32, pActualSize: *mut u32, nFlags: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetCustomDSDataFunc)(Source, Data, dSize, pActualSize, nFlags);  }
    }

    pub fn DTWAIN_GetDSMFullName(&self, DSMType: i32, szDLLName: *mut u16, nMaxLen: i32, pWhichSearch: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetDSMFullNameFunc)(DSMType, szDLLName, nMaxLen, pWhichSearch);  }
    }

    pub fn DTWAIN_GetDSMFullNameA(&self, DSMType: i32, szDLLName: *mut c_char, nMaxLen: i32, pWhichSearch: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetDSMFullNameAFunc)(DSMType, szDLLName, nMaxLen, pWhichSearch);  }
    }

    pub fn DTWAIN_GetDSMFullNameW(&self, DSMType: i32, szDLLName: *mut u16, nMaxLen: i32, pWhichSearch: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetDSMFullNameWFunc)(DSMType, szDLLName, nMaxLen, pWhichSearch);  }
    }

    pub fn DTWAIN_GetDSMSearchOrder(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetDSMSearchOrderFunc)();  }
    }

    pub fn DTWAIN_GetDTWAINHandle(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetDTWAINHandleFunc)();  }
    }

    pub fn DTWAIN_GetDeviceEvent(&self, Source: *mut c_void, lpEvent: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetDeviceEventFunc)(Source, lpEvent);  }
    }

    pub fn DTWAIN_GetDeviceEventEx(&self, Source: *mut c_void, lpEvent: *mut i32, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetDeviceEventExFunc)(Source, lpEvent, pArray);  }
    }

    pub fn DTWAIN_GetDeviceEventInfo(&self, Source: *mut c_void, nWhichInfo: i32, pValue: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetDeviceEventInfoFunc)(Source, nWhichInfo, pValue);  }
    }

    pub fn DTWAIN_GetDeviceNotifications(&self, Source: *mut c_void, DevEvents: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetDeviceNotificationsFunc)(Source, DevEvents);  }
    }

    pub fn DTWAIN_GetDeviceTimeDate(&self, Source: *mut c_void, szTimeDate: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetDeviceTimeDateFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_GetDeviceTimeDateA(&self, Source: *mut c_void, szTimeDate: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetDeviceTimeDateAFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_GetDeviceTimeDateW(&self, Source: *mut c_void, szTimeDate: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetDeviceTimeDateWFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_GetDoubleFeedDetectLength(&self, Source: *mut c_void, Value: *mut f64, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetDoubleFeedDetectLengthFunc)(Source, Value, bCurrent);  }
    }

    pub fn DTWAIN_GetDoubleFeedDetectValues(&self, Source: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetDoubleFeedDetectValuesFunc)(Source, pArray);  }
    }

    pub fn DTWAIN_GetDuplexType(&self, Source: *mut c_void, lpDupType: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetDuplexTypeFunc)(Source, lpDupType);  }
    }

    pub fn DTWAIN_GetErrorBuffer(&self, ArrayBuffer: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetErrorBufferFunc)(ArrayBuffer);  }
    }

    pub fn DTWAIN_GetErrorBufferThreshold(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetErrorBufferThresholdFunc)();  }
    }

    pub fn DTWAIN_GetErrorCallback(&self) -> DTWAIN_ERROR_PROC {
        unsafe { return (self.DTWAIN_GetErrorCallbackFunc)();  }
    }

    pub fn DTWAIN_GetErrorCallback64(&self) -> DTWAIN_ERROR_PROC64 {
        unsafe { return (self.DTWAIN_GetErrorCallback64Func)();  }
    }

    pub fn DTWAIN_GetErrorString(&self, lError: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetErrorStringFunc)(lError, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetErrorStringA(&self, lError: i32, lpszBuffer: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetErrorStringAFunc)(lError, lpszBuffer, nLength);  }
    }

    pub fn DTWAIN_GetErrorStringW(&self, lError: i32, lpszBuffer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetErrorStringWFunc)(lError, lpszBuffer, nLength);  }
    }

    pub fn DTWAIN_GetExtCapFromName(&self, szName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetExtCapFromNameFunc)(szName);  }
    }

    pub fn DTWAIN_GetExtCapFromNameA(&self, szName: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetExtCapFromNameAFunc)(szName);  }
    }

    pub fn DTWAIN_GetExtCapFromNameW(&self, szName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetExtCapFromNameWFunc)(szName);  }
    }

    pub fn DTWAIN_GetExtImageInfo(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetExtImageInfoFunc)(Source);  }
    }

    pub fn DTWAIN_GetExtImageInfoData(&self, Source: *mut c_void, nWhich: i32, Data: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetExtImageInfoDataFunc)(Source, nWhich, Data);  }
    }

    pub fn DTWAIN_GetExtImageInfoDataEx(&self, Source: *mut c_void, nWhich: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetExtImageInfoDataExFunc)(Source, nWhich);  }
    }

    pub fn DTWAIN_GetExtImageInfoItem(&self, Source: *mut c_void, nWhich: i32, InfoID: *mut i32, NumItems: *mut i32, Type: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetExtImageInfoItemFunc)(Source, nWhich, InfoID, NumItems, Type);  }
    }

    pub fn DTWAIN_GetExtImageInfoItemEx(&self, Source: *mut c_void, nWhich: i32, InfoID: *mut i32, NumItems: *mut i32, Type: *mut i32, ReturnCode: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetExtImageInfoItemExFunc)(Source, nWhich, InfoID, NumItems, Type, ReturnCode);  }
    }

    pub fn DTWAIN_GetExtNameFromCap(&self, nValue: i32, szValue: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetExtNameFromCapFunc)(nValue, szValue, nMaxLen);  }
    }

    pub fn DTWAIN_GetExtNameFromCapA(&self, nValue: i32, szValue: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetExtNameFromCapAFunc)(nValue, szValue, nLength);  }
    }

    pub fn DTWAIN_GetExtNameFromCapW(&self, nValue: i32, szValue: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetExtNameFromCapWFunc)(nValue, szValue, nLength);  }
    }

    pub fn DTWAIN_GetFeederAlignment(&self, Source: *mut c_void, lpAlignment: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFeederAlignmentFunc)(Source, lpAlignment);  }
    }

    pub fn DTWAIN_GetFeederFuncs(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetFeederFuncsFunc)(Source);  }
    }

    pub fn DTWAIN_GetFeederOrder(&self, Source: *mut c_void, lpOrder: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFeederOrderFunc)(Source, lpOrder);  }
    }

    pub fn DTWAIN_GetFeederWaitTime(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetFeederWaitTimeFunc)(Source);  }
    }

    pub fn DTWAIN_GetFileCompressionType(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetFileCompressionTypeFunc)(Source);  }
    }

    pub fn DTWAIN_GetFileTypeExtensions(&self, nType: i32, lpszName: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFileTypeExtensionsFunc)(nType, lpszName, nLength);  }
    }

    pub fn DTWAIN_GetFileTypeExtensionsA(&self, nType: i32, lpszName: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFileTypeExtensionsAFunc)(nType, lpszName, nLength);  }
    }

    pub fn DTWAIN_GetFileTypeExtensionsW(&self, nType: i32, lpszName: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFileTypeExtensionsWFunc)(nType, lpszName, nLength);  }
    }

    pub fn DTWAIN_GetFileTypeName(&self, nType: i32, lpszName: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFileTypeNameFunc)(nType, lpszName, nLength);  }
    }

    pub fn DTWAIN_GetFileTypeNameA(&self, nType: i32, lpszName: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFileTypeNameAFunc)(nType, lpszName, nLength);  }
    }

    pub fn DTWAIN_GetFileTypeNameW(&self, nType: i32, lpszName: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetFileTypeNameWFunc)(nType, lpszName, nLength);  }
    }

    pub fn DTWAIN_GetHalftone(&self, Source: *mut c_void, lpHalftone: *mut u16, GetType: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetHalftoneFunc)(Source, lpHalftone, GetType);  }
    }

    pub fn DTWAIN_GetHalftoneA(&self, Source: *mut c_void, lpHalftone: *mut c_char, GetType: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetHalftoneAFunc)(Source, lpHalftone, GetType);  }
    }

    pub fn DTWAIN_GetHalftoneW(&self, Source: *mut c_void, lpHalftone: *mut u16, GetType: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetHalftoneWFunc)(Source, lpHalftone, GetType);  }
    }

    pub fn DTWAIN_GetHighlight(&self, Source: *mut c_void, Highlight: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetHighlightFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_GetHighlightString(&self, Source: *mut c_void, Highlight: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetHighlightStringFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_GetHighlightStringA(&self, Source: *mut c_void, Highlight: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetHighlightStringAFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_GetHighlightStringW(&self, Source: *mut c_void, Highlight: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetHighlightStringWFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_GetImageInfo(&self, Source: *mut c_void, lpXResolution: *mut f64, lpYResolution: *mut f64, lpWidth: *mut i32, lpLength: *mut i32, lpNumSamples: *mut i32, lpBitsPerSample: *mut *mut c_void, lpBitsPerPixel: *mut i32, lpPlanar: *mut i32, lpPixelType: *mut i32, lpCompression: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetImageInfoFunc)(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression);  }
    }

    pub fn DTWAIN_GetImageInfoString(&self, Source: *mut c_void, lpXResolution: *mut u16, lpYResolution: *mut u16, lpWidth: *mut i32, lpLength: *mut i32, lpNumSamples: *mut i32, lpBitsPerSample: *mut *mut c_void, lpBitsPerPixel: *mut i32, lpPlanar: *mut i32, lpPixelType: *mut i32, lpCompression: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetImageInfoStringFunc)(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression);  }
    }

    pub fn DTWAIN_GetImageInfoStringA(&self, Source: *mut c_void, lpXResolution: *mut c_char, lpYResolution: *mut c_char, lpWidth: *mut i32, lpLength: *mut i32, lpNumSamples: *mut i32, lpBitsPerSample: *mut *mut c_void, lpBitsPerPixel: *mut i32, lpPlanar: *mut i32, lpPixelType: *mut i32, lpCompression: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetImageInfoStringAFunc)(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression);  }
    }

    pub fn DTWAIN_GetImageInfoStringW(&self, Source: *mut c_void, lpXResolution: *mut u16, lpYResolution: *mut u16, lpWidth: *mut i32, lpLength: *mut i32, lpNumSamples: *mut i32, lpBitsPerSample: *mut *mut c_void, lpBitsPerPixel: *mut i32, lpPlanar: *mut i32, lpPixelType: *mut i32, lpCompression: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetImageInfoStringWFunc)(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression);  }
    }

    pub fn DTWAIN_GetJobControl(&self, Source: *mut c_void, pJobControl: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetJobControlFunc)(Source, pJobControl, bCurrent);  }
    }

    pub fn DTWAIN_GetJpegValues(&self, Source: *mut c_void, pQuality: *mut i32, Progressive: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetJpegValuesFunc)(Source, pQuality, Progressive);  }
    }

    pub fn DTWAIN_GetJpegXRValues(&self, Source: *mut c_void, pQuality: *mut i32, Progressive: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetJpegXRValuesFunc)(Source, pQuality, Progressive);  }
    }

    pub fn DTWAIN_GetLanguage(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetLanguageFunc)();  }
    }

    pub fn DTWAIN_GetLastError(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetLastErrorFunc)();  }
    }

    pub fn DTWAIN_GetLibraryPath(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetLibraryPathFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetLibraryPathA(&self, lpszVer: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetLibraryPathAFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetLibraryPathW(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetLibraryPathWFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetLightPath(&self, Source: *mut c_void, lpLightPath: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetLightPathFunc)(Source, lpLightPath);  }
    }

    pub fn DTWAIN_GetLightSource(&self, Source: *mut c_void, LightSource: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetLightSourceFunc)(Source, LightSource);  }
    }

    pub fn DTWAIN_GetLightSources(&self, Source: *mut c_void, LightSources: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetLightSourcesFunc)(Source, LightSources);  }
    }

    pub fn DTWAIN_GetLoggerCallback(&self) -> DTWAIN_LOGGER_PROC {
        unsafe { return (self.DTWAIN_GetLoggerCallbackFunc)();  }
    }

    pub fn DTWAIN_GetLoggerCallbackA(&self) -> DTWAIN_LOGGER_PROCA {
        unsafe { return (self.DTWAIN_GetLoggerCallbackAFunc)();  }
    }

    pub fn DTWAIN_GetLoggerCallbackW(&self) -> DTWAIN_LOGGER_PROCW {
        unsafe { return (self.DTWAIN_GetLoggerCallbackWFunc)();  }
    }

    pub fn DTWAIN_GetManualDuplexCount(&self, Source: *mut c_void, pSide1: *mut i32, pSide2: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetManualDuplexCountFunc)(Source, pSide1, pSide2);  }
    }

    pub fn DTWAIN_GetMaxAcquisitions(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetMaxAcquisitionsFunc)(Source);  }
    }

    pub fn DTWAIN_GetMaxBuffers(&self, Source: *mut c_void, pMaxBuf: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetMaxBuffersFunc)(Source, pMaxBuf);  }
    }

    pub fn DTWAIN_GetMaxPagesToAcquire(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetMaxPagesToAcquireFunc)(Source);  }
    }

    pub fn DTWAIN_GetMaxRetryAttempts(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetMaxRetryAttemptsFunc)(Source);  }
    }

    pub fn DTWAIN_GetNameFromCap(&self, nCapValue: i32, szValue: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetNameFromCapFunc)(nCapValue, szValue, nMaxLen);  }
    }

    pub fn DTWAIN_GetNameFromCapA(&self, nCapValue: i32, szValue: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetNameFromCapAFunc)(nCapValue, szValue, nLength);  }
    }

    pub fn DTWAIN_GetNameFromCapW(&self, nCapValue: i32, szValue: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetNameFromCapWFunc)(nCapValue, szValue, nLength);  }
    }

    pub fn DTWAIN_GetNoiseFilter(&self, Source: *mut c_void, lpNoiseFilter: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetNoiseFilterFunc)(Source, lpNoiseFilter);  }
    }

    pub fn DTWAIN_GetNumAcquiredImages(&self, aAcq: *mut c_void, nWhich: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetNumAcquiredImagesFunc)(aAcq, nWhich);  }
    }

    pub fn DTWAIN_GetNumAcquisitions(&self, aAcq: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetNumAcquisitionsFunc)(aAcq);  }
    }

    pub fn DTWAIN_GetOCRCapValues(&self, Engine: *mut c_void, OCRCapValue: i32, GetType: i32, CapValues: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRCapValuesFunc)(Engine, OCRCapValue, GetType, CapValues);  }
    }

    pub fn DTWAIN_GetOCRErrorString(&self, Engine: *mut c_void, lError: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRErrorStringFunc)(Engine, lError, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetOCRErrorStringA(&self, Engine: *mut c_void, lError: i32, lpszBuffer: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRErrorStringAFunc)(Engine, lError, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetOCRErrorStringW(&self, Engine: *mut c_void, lError: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRErrorStringWFunc)(Engine, lError, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetOCRLastError(&self, Engine: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRLastErrorFunc)(Engine);  }
    }

    pub fn DTWAIN_GetOCRMajorMinorVersion(&self, Engine: *mut c_void, lpMajor: *mut i32, lpMinor: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRMajorMinorVersionFunc)(Engine, lpMajor, lpMinor);  }
    }

    pub fn DTWAIN_GetOCRManufacturer(&self, Engine: *mut c_void, szManufacturer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRManufacturerFunc)(Engine, szManufacturer, nMaxLen);  }
    }

    pub fn DTWAIN_GetOCRManufacturerA(&self, Engine: *mut c_void, szManufacturer: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRManufacturerAFunc)(Engine, szManufacturer, nLength);  }
    }

    pub fn DTWAIN_GetOCRManufacturerW(&self, Engine: *mut c_void, szManufacturer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRManufacturerWFunc)(Engine, szManufacturer, nLength);  }
    }

    pub fn DTWAIN_GetOCRProductFamily(&self, Engine: *mut c_void, szProductFamily: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRProductFamilyFunc)(Engine, szProductFamily, nMaxLen);  }
    }

    pub fn DTWAIN_GetOCRProductFamilyA(&self, Engine: *mut c_void, szProductFamily: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRProductFamilyAFunc)(Engine, szProductFamily, nLength);  }
    }

    pub fn DTWAIN_GetOCRProductFamilyW(&self, Engine: *mut c_void, szProductFamily: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRProductFamilyWFunc)(Engine, szProductFamily, nLength);  }
    }

    pub fn DTWAIN_GetOCRProductName(&self, Engine: *mut c_void, szProductName: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRProductNameFunc)(Engine, szProductName, nMaxLen);  }
    }

    pub fn DTWAIN_GetOCRProductNameA(&self, Engine: *mut c_void, szProductName: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRProductNameAFunc)(Engine, szProductName, nLength);  }
    }

    pub fn DTWAIN_GetOCRProductNameW(&self, Engine: *mut c_void, szProductName: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRProductNameWFunc)(Engine, szProductName, nLength);  }
    }

    pub fn DTWAIN_GetOCRText(&self, Engine: *mut c_void, nPageNo: i32, Data: *mut u16, dSize: i32, pActualSize: *mut i32, nFlags: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetOCRTextFunc)(Engine, nPageNo, Data, dSize, pActualSize, nFlags);  }
    }

    pub fn DTWAIN_GetOCRTextA(&self, Engine: *mut c_void, nPageNo: i32, Data: *mut c_char, dSize: i32, pActualSize: *mut i32, nFlags: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetOCRTextAFunc)(Engine, nPageNo, Data, dSize, pActualSize, nFlags);  }
    }

    pub fn DTWAIN_GetOCRTextInfoFloat(&self, OCRTextInfo: *mut c_void, nCharPos: i32, nWhichItem: i32, pInfo: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRTextInfoFloatFunc)(OCRTextInfo, nCharPos, nWhichItem, pInfo);  }
    }

    pub fn DTWAIN_GetOCRTextInfoFloatEx(&self, OCRTextInfo: *mut c_void, nWhichItem: i32, pInfo: *mut f64, bufSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRTextInfoFloatExFunc)(OCRTextInfo, nWhichItem, pInfo, bufSize);  }
    }

    pub fn DTWAIN_GetOCRTextInfoHandle(&self, Engine: *mut c_void, nPageNo: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetOCRTextInfoHandleFunc)(Engine, nPageNo);  }
    }

    pub fn DTWAIN_GetOCRTextInfoLong(&self, OCRTextInfo: *mut c_void, nCharPos: i32, nWhichItem: i32, pInfo: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRTextInfoLongFunc)(OCRTextInfo, nCharPos, nWhichItem, pInfo);  }
    }

    pub fn DTWAIN_GetOCRTextInfoLongEx(&self, OCRTextInfo: *mut c_void, nWhichItem: i32, pInfo: *mut i32, bufSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRTextInfoLongExFunc)(OCRTextInfo, nWhichItem, pInfo, bufSize);  }
    }

    pub fn DTWAIN_GetOCRTextW(&self, Engine: *mut c_void, nPageNo: i32, Data: *mut u16, dSize: i32, pActualSize: *mut i32, nFlags: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetOCRTextWFunc)(Engine, nPageNo, Data, dSize, pActualSize, nFlags);  }
    }

    pub fn DTWAIN_GetOCRVersionInfo(&self, Engine: *mut c_void, buffer: *mut u16, maxBufSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRVersionInfoFunc)(Engine, buffer, maxBufSize);  }
    }

    pub fn DTWAIN_GetOCRVersionInfoA(&self, Engine: *mut c_void, buffer: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRVersionInfoAFunc)(Engine, buffer, nLength);  }
    }

    pub fn DTWAIN_GetOCRVersionInfoW(&self, Engine: *mut c_void, buffer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOCRVersionInfoWFunc)(Engine, buffer, nLength);  }
    }

    pub fn DTWAIN_GetOrientation(&self, Source: *mut c_void, lpOrient: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOrientationFunc)(Source, lpOrient, bCurrent);  }
    }

    pub fn DTWAIN_GetOverscan(&self, Source: *mut c_void, lpOverscan: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetOverscanFunc)(Source, lpOverscan, bCurrent);  }
    }

    pub fn DTWAIN_GetPDFTextElementFloat(&self, TextElement: *mut c_void, val1: *mut f64, val2: *mut f64, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFTextElementFloatFunc)(TextElement, val1, val2, Flags);  }
    }

    pub fn DTWAIN_GetPDFTextElementLong(&self, TextElement: *mut c_void, val1: *mut i32, val2: *mut i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFTextElementLongFunc)(TextElement, val1, val2, Flags);  }
    }

    pub fn DTWAIN_GetPDFTextElementString(&self, TextElement: *mut c_void, szData: *mut u16, maxLen: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFTextElementStringFunc)(TextElement, szData, maxLen, Flags);  }
    }

    pub fn DTWAIN_GetPDFTextElementStringA(&self, TextElement: *mut c_void, szData: *mut c_char, maxLen: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFTextElementStringAFunc)(TextElement, szData, maxLen, Flags);  }
    }

    pub fn DTWAIN_GetPDFTextElementStringW(&self, TextElement: *mut c_void, szData: *mut u16, maxLen: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFTextElementStringWFunc)(TextElement, szData, maxLen, Flags);  }
    }

    pub fn DTWAIN_GetPDFType1FontName(&self, FontVal: i32, szFont: *mut u16, nChars: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFType1FontNameFunc)(FontVal, szFont, nChars);  }
    }

    pub fn DTWAIN_GetPDFType1FontNameA(&self, FontVal: i32, szFont: *mut c_char, nChars: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFType1FontNameAFunc)(FontVal, szFont, nChars);  }
    }

    pub fn DTWAIN_GetPDFType1FontNameW(&self, FontVal: i32, szFont: *mut u16, nChars: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPDFType1FontNameWFunc)(FontVal, szFont, nChars);  }
    }

    pub fn DTWAIN_GetPaperSize(&self, Source: *mut c_void, lpPaperSize: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPaperSizeFunc)(Source, lpPaperSize, bCurrent);  }
    }

    pub fn DTWAIN_GetPaperSizeName(&self, paperNumber: i32, outName: *mut u16, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPaperSizeNameFunc)(paperNumber, outName, nSize);  }
    }

    pub fn DTWAIN_GetPaperSizeNameA(&self, paperNumber: i32, outName: *mut c_char, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPaperSizeNameAFunc)(paperNumber, outName, nSize);  }
    }

    pub fn DTWAIN_GetPaperSizeNameW(&self, paperNumber: i32, outName: *mut u16, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPaperSizeNameWFunc)(paperNumber, outName, nSize);  }
    }

    pub fn DTWAIN_GetPatchMaxPriorities(&self, Source: *mut c_void, pMaxPriorities: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPatchMaxPrioritiesFunc)(Source, pMaxPriorities, bCurrent);  }
    }

    pub fn DTWAIN_GetPatchMaxRetries(&self, Source: *mut c_void, pMaxRetries: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPatchMaxRetriesFunc)(Source, pMaxRetries, bCurrent);  }
    }

    pub fn DTWAIN_GetPatchPriorities(&self, Source: *mut c_void, SearchPriorities: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetPatchPrioritiesFunc)(Source, SearchPriorities);  }
    }

    pub fn DTWAIN_GetPatchSearchMode(&self, Source: *mut c_void, pSearchMode: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPatchSearchModeFunc)(Source, pSearchMode, bCurrent);  }
    }

    pub fn DTWAIN_GetPatchTimeOut(&self, Source: *mut c_void, pTimeOut: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPatchTimeOutFunc)(Source, pTimeOut, bCurrent);  }
    }

    pub fn DTWAIN_GetPixelFlavor(&self, Source: *mut c_void, lpPixelFlavor: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPixelFlavorFunc)(Source, lpPixelFlavor);  }
    }

    pub fn DTWAIN_GetPixelType(&self, Source: *mut c_void, PixelType: *mut i32, BitDepth: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPixelTypeFunc)(Source, PixelType, BitDepth, bCurrent);  }
    }

    pub fn DTWAIN_GetPrinter(&self, Source: *mut c_void, lpPrinter: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPrinterFunc)(Source, lpPrinter, bCurrent);  }
    }

    pub fn DTWAIN_GetPrinterStartNumber(&self, Source: *mut c_void, nStart: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPrinterStartNumberFunc)(Source, nStart);  }
    }

    pub fn DTWAIN_GetPrinterStringMode(&self, Source: *mut c_void, PrinterMode: *mut i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPrinterStringModeFunc)(Source, PrinterMode, bCurrent);  }
    }

    pub fn DTWAIN_GetPrinterStrings(&self, Source: *mut c_void, ArrayString: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetPrinterStringsFunc)(Source, ArrayString);  }
    }

    pub fn DTWAIN_GetPrinterSuffixString(&self, Source: *mut c_void, Suffix: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPrinterSuffixStringFunc)(Source, Suffix, nMaxLen);  }
    }

    pub fn DTWAIN_GetPrinterSuffixStringA(&self, Source: *mut c_void, Suffix: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPrinterSuffixStringAFunc)(Source, Suffix, nLength);  }
    }

    pub fn DTWAIN_GetPrinterSuffixStringW(&self, Source: *mut c_void, Suffix: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetPrinterSuffixStringWFunc)(Source, Suffix, nLength);  }
    }

    pub fn DTWAIN_GetRegisteredMsg(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetRegisteredMsgFunc)();  }
    }

    pub fn DTWAIN_GetResolution(&self, Source: *mut c_void, Resolution: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetResolutionFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetResolutionString(&self, Source: *mut c_void, Resolution: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetResolutionStringFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetResolutionStringA(&self, Source: *mut c_void, Resolution: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetResolutionStringAFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetResolutionStringW(&self, Source: *mut c_void, Resolution: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetResolutionStringWFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetResourceString(&self, ResourceID: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetResourceStringFunc)(ResourceID, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetResourceStringA(&self, ResourceID: i32, lpszBuffer: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetResourceStringAFunc)(ResourceID, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetResourceStringW(&self, ResourceID: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetResourceStringWFunc)(ResourceID, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetRotation(&self, Source: *mut c_void, Rotation: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetRotationFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_GetRotationString(&self, Source: *mut c_void, Rotation: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetRotationStringFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_GetRotationStringA(&self, Source: *mut c_void, Rotation: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetRotationStringAFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_GetRotationStringW(&self, Source: *mut c_void, Rotation: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetRotationStringWFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_GetSaveFileName(&self, Source: *mut c_void, fName: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSaveFileNameFunc)(Source, fName, nMaxLen);  }
    }

    pub fn DTWAIN_GetSaveFileNameA(&self, Source: *mut c_void, fName: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSaveFileNameAFunc)(Source, fName, nMaxLen);  }
    }

    pub fn DTWAIN_GetSaveFileNameW(&self, Source: *mut c_void, fName: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSaveFileNameWFunc)(Source, fName, nMaxLen);  }
    }

    pub fn DTWAIN_GetSavedFilesCount(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_GetSavedFilesCountFunc)(Source);  }
    }

    pub fn DTWAIN_GetSessionDetails(&self, szBuf: *mut u16, nSize: i32, indentFactor: i32, bRefresh: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSessionDetailsFunc)(szBuf, nSize, indentFactor, bRefresh);  }
    }

    pub fn DTWAIN_GetSessionDetailsA(&self, szBuf: *mut c_char, nSize: i32, indentFactor: i32, bRefresh: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSessionDetailsAFunc)(szBuf, nSize, indentFactor, bRefresh);  }
    }

    pub fn DTWAIN_GetSessionDetailsW(&self, szBuf: *mut u16, nSize: i32, indentFactor: i32, bRefresh: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSessionDetailsWFunc)(szBuf, nSize, indentFactor, bRefresh);  }
    }

    pub fn DTWAIN_GetShadow(&self, Source: *mut c_void, Shadow: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetShadowFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_GetShadowString(&self, Source: *mut c_void, Shadow: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetShadowStringFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_GetShadowStringA(&self, Source: *mut c_void, Shadow: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetShadowStringAFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_GetShadowStringW(&self, Source: *mut c_void, Shadow: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetShadowStringWFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_GetShortVersionString(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetShortVersionStringFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetShortVersionStringA(&self, lpszVer: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetShortVersionStringAFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetShortVersionStringW(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetShortVersionStringWFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetSourceAcquisitions(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetSourceAcquisitionsFunc)(Source);  }
    }

    pub fn DTWAIN_GetSourceDetails(&self, szSources: *const u16, szBuf: *mut u16, nSize: i32, indentFactor: i32, bRefresh: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceDetailsFunc)(szSources, szBuf, nSize, indentFactor, bRefresh);  }
    }

    pub fn DTWAIN_GetSourceDetailsA(&self, szSources: *const c_char, szBuf: *mut c_char, nSize: i32, indentFactor: i32, bRefresh: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceDetailsAFunc)(szSources, szBuf, nSize, indentFactor, bRefresh);  }
    }

    pub fn DTWAIN_GetSourceDetailsW(&self, szSources: *const u16, szBuf: *mut u16, nSize: i32, indentFactor: i32, bRefresh: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceDetailsWFunc)(szSources, szBuf, nSize, indentFactor, bRefresh);  }
    }

    pub fn DTWAIN_GetSourceID(&self, Source: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetSourceIDFunc)(Source);  }
    }

    pub fn DTWAIN_GetSourceIDEx(&self, Source: *mut c_void, pIdentity: *mut *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetSourceIDExFunc)(Source, pIdentity);  }
    }

    pub fn DTWAIN_GetSourceManufacturer(&self, Source: *mut c_void, szProduct: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceManufacturerFunc)(Source, szProduct, nMaxLen);  }
    }

    pub fn DTWAIN_GetSourceManufacturerA(&self, Source: *mut c_void, szProduct: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceManufacturerAFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceManufacturerW(&self, Source: *mut c_void, szProduct: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceManufacturerWFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceProductFamily(&self, Source: *mut c_void, szProduct: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceProductFamilyFunc)(Source, szProduct, nMaxLen);  }
    }

    pub fn DTWAIN_GetSourceProductFamilyA(&self, Source: *mut c_void, szProduct: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceProductFamilyAFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceProductFamilyW(&self, Source: *mut c_void, szProduct: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceProductFamilyWFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceProductName(&self, Source: *mut c_void, szProduct: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceProductNameFunc)(Source, szProduct, nMaxLen);  }
    }

    pub fn DTWAIN_GetSourceProductNameA(&self, Source: *mut c_void, szProduct: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceProductNameAFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceProductNameW(&self, Source: *mut c_void, szProduct: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceProductNameWFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceUnit(&self, Source: *mut c_void, lpUnit: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceUnitFunc)(Source, lpUnit);  }
    }

    pub fn DTWAIN_GetSourceVersionInfo(&self, Source: *mut c_void, szProduct: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceVersionInfoFunc)(Source, szProduct, nMaxLen);  }
    }

    pub fn DTWAIN_GetSourceVersionInfoA(&self, Source: *mut c_void, szProduct: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceVersionInfoAFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceVersionInfoW(&self, Source: *mut c_void, szProduct: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceVersionInfoWFunc)(Source, szProduct, nLength);  }
    }

    pub fn DTWAIN_GetSourceVersionNumber(&self, Source: *mut c_void, pMajor: *mut i32, pMinor: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetSourceVersionNumberFunc)(Source, pMajor, pMinor);  }
    }

    pub fn DTWAIN_GetStaticLibVersion(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetStaticLibVersionFunc)();  }
    }

    pub fn DTWAIN_GetTempFileDirectory(&self, szFilePath: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTempFileDirectoryFunc)(szFilePath, nMaxLen);  }
    }

    pub fn DTWAIN_GetTempFileDirectoryA(&self, szFilePath: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTempFileDirectoryAFunc)(szFilePath, nLength);  }
    }

    pub fn DTWAIN_GetTempFileDirectoryW(&self, szFilePath: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTempFileDirectoryWFunc)(szFilePath, nLength);  }
    }

    pub fn DTWAIN_GetThreshold(&self, Source: *mut c_void, Threshold: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetThresholdFunc)(Source, Threshold);  }
    }

    pub fn DTWAIN_GetThresholdString(&self, Source: *mut c_void, Threshold: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetThresholdStringFunc)(Source, Threshold);  }
    }

    pub fn DTWAIN_GetThresholdStringA(&self, Source: *mut c_void, Threshold: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetThresholdStringAFunc)(Source, Threshold);  }
    }

    pub fn DTWAIN_GetThresholdStringW(&self, Source: *mut c_void, Threshold: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetThresholdStringWFunc)(Source, Threshold);  }
    }

    pub fn DTWAIN_GetTimeDate(&self, Source: *mut c_void, szTimeDate: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTimeDateFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_GetTimeDateA(&self, Source: *mut c_void, szTimeDate: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetTimeDateAFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_GetTimeDateW(&self, Source: *mut c_void, szTimeDate: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTimeDateWFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_GetTwainAppID(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetTwainAppIDFunc)();  }
    }

    pub fn DTWAIN_GetTwainAppIDEx(&self, pIdentity: *mut *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_GetTwainAppIDExFunc)(pIdentity);  }
    }

    pub fn DTWAIN_GetTwainAvailability(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainAvailabilityFunc)();  }
    }

    pub fn DTWAIN_GetTwainAvailabilityEx(&self, directories: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainAvailabilityExFunc)(directories, nMaxLen);  }
    }

    pub fn DTWAIN_GetTwainAvailabilityExA(&self, szDirectories: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainAvailabilityExAFunc)(szDirectories, nLength);  }
    }

    pub fn DTWAIN_GetTwainAvailabilityExW(&self, szDirectories: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainAvailabilityExWFunc)(szDirectories, nLength);  }
    }

    pub fn DTWAIN_GetTwainCountryName(&self, countryId: i32, szName: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainCountryNameFunc)(countryId, szName);  }
    }

    pub fn DTWAIN_GetTwainCountryNameA(&self, countryId: i32, szName: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainCountryNameAFunc)(countryId, szName);  }
    }

    pub fn DTWAIN_GetTwainCountryNameW(&self, countryId: i32, szName: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainCountryNameWFunc)(countryId, szName);  }
    }

    pub fn DTWAIN_GetTwainCountryValue(&self, country: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainCountryValueFunc)(country);  }
    }

    pub fn DTWAIN_GetTwainCountryValueA(&self, country: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainCountryValueAFunc)(country);  }
    }

    pub fn DTWAIN_GetTwainCountryValueW(&self, country: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainCountryValueWFunc)(country);  }
    }

    pub fn DTWAIN_GetTwainHwnd(&self) -> *const c_void {
        unsafe { return (self.DTWAIN_GetTwainHwndFunc)();  }
    }

    pub fn DTWAIN_GetTwainIDFromName(&self, lpszBuffer: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainIDFromNameFunc)(lpszBuffer);  }
    }

    pub fn DTWAIN_GetTwainIDFromNameA(&self, lpszBuffer: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainIDFromNameAFunc)(lpszBuffer);  }
    }

    pub fn DTWAIN_GetTwainIDFromNameW(&self, lpszBuffer: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainIDFromNameWFunc)(lpszBuffer);  }
    }

    pub fn DTWAIN_GetTwainLanguageName(&self, nameId: i32, szName: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainLanguageNameFunc)(nameId, szName);  }
    }

    pub fn DTWAIN_GetTwainLanguageNameA(&self, lang: i32, szName: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainLanguageNameAFunc)(lang, szName);  }
    }

    pub fn DTWAIN_GetTwainLanguageNameW(&self, lang: i32, szName: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainLanguageNameWFunc)(lang, szName);  }
    }

    pub fn DTWAIN_GetTwainLanguageValue(&self, szName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainLanguageValueFunc)(szName);  }
    }

    pub fn DTWAIN_GetTwainLanguageValueA(&self, lang: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainLanguageValueAFunc)(lang);  }
    }

    pub fn DTWAIN_GetTwainLanguageValueW(&self, lang: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainLanguageValueWFunc)(lang);  }
    }

    pub fn DTWAIN_GetTwainMode(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainModeFunc)();  }
    }

    pub fn DTWAIN_GetTwainNameFromConstant(&self, lConstantType: i32, lTwainConstant: i32, lpszOut: *mut u16, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainNameFromConstantFunc)(lConstantType, lTwainConstant, lpszOut, nSize);  }
    }

    pub fn DTWAIN_GetTwainNameFromConstantA(&self, lConstantType: i32, lTwainConstant: i32, lpszOut: *mut c_char, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainNameFromConstantAFunc)(lConstantType, lTwainConstant, lpszOut, nSize);  }
    }

    pub fn DTWAIN_GetTwainNameFromConstantW(&self, lConstantType: i32, lTwainConstant: i32, lpszOut: *mut u16, nSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainNameFromConstantWFunc)(lConstantType, lTwainConstant, lpszOut, nSize);  }
    }

    pub fn DTWAIN_GetTwainStringName(&self, category: i32, TwainID: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainStringNameFunc)(category, TwainID, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetTwainStringNameA(&self, category: i32, TwainID: i32, lpszBuffer: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainStringNameAFunc)(category, TwainID, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetTwainStringNameW(&self, category: i32, TwainID: i32, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainStringNameWFunc)(category, TwainID, lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetTwainTimeout(&self) -> i32 {
        unsafe { return (self.DTWAIN_GetTwainTimeoutFunc)();  }
    }

    pub fn DTWAIN_GetVersion(&self, lpMajor: *mut i32, lpMinor: *mut i32, lpVersionType: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionFunc)(lpMajor, lpMinor, lpVersionType);  }
    }

    pub fn DTWAIN_GetVersionCopyright(&self, lpszApp: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionCopyrightFunc)(lpszApp, nLength);  }
    }

    pub fn DTWAIN_GetVersionCopyrightA(&self, lpszApp: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionCopyrightAFunc)(lpszApp, nLength);  }
    }

    pub fn DTWAIN_GetVersionCopyrightW(&self, lpszApp: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionCopyrightWFunc)(lpszApp, nLength);  }
    }

    pub fn DTWAIN_GetVersionEx(&self, lMajor: *mut i32, lMinor: *mut i32, lVersionType: *mut i32, lPatchLevel: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionExFunc)(lMajor, lMinor, lVersionType, lPatchLevel);  }
    }

    pub fn DTWAIN_GetVersionInfo(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionInfoFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetVersionInfoA(&self, lpszVer: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionInfoAFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetVersionInfoW(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionInfoWFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetVersionString(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionStringFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetVersionStringA(&self, lpszVer: *mut c_char, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionStringAFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetVersionStringW(&self, lpszVer: *mut u16, nLength: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetVersionStringWFunc)(lpszVer, nLength);  }
    }

    pub fn DTWAIN_GetWindowsVersionInfo(&self, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetWindowsVersionInfoFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetWindowsVersionInfoA(&self, lpszBuffer: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetWindowsVersionInfoAFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetWindowsVersionInfoW(&self, lpszBuffer: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_GetWindowsVersionInfoWFunc)(lpszBuffer, nMaxLen);  }
    }

    pub fn DTWAIN_GetXResolution(&self, Source: *mut c_void, Resolution: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetXResolutionFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetXResolutionString(&self, Source: *mut c_void, Resolution: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetXResolutionStringFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetXResolutionStringA(&self, Source: *mut c_void, Resolution: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetXResolutionStringAFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetXResolutionStringW(&self, Source: *mut c_void, Resolution: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetXResolutionStringWFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetYResolution(&self, Source: *mut c_void, Resolution: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_GetYResolutionFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetYResolutionString(&self, Source: *mut c_void, Resolution: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetYResolutionStringFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetYResolutionStringA(&self, Source: *mut c_void, Resolution: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_GetYResolutionStringAFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_GetYResolutionStringW(&self, Source: *mut c_void, Resolution: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_GetYResolutionStringWFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_InitExtImageInfo(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_InitExtImageInfoFunc)(Source);  }
    }

    pub fn DTWAIN_InitImageFileAppend(&self, szFile: *const u16, fType: i32) -> i32 {
        unsafe { return (self.DTWAIN_InitImageFileAppendFunc)(szFile, fType);  }
    }

    pub fn DTWAIN_InitImageFileAppendA(&self, szFile: *const c_char, fType: i32) -> i32 {
        unsafe { return (self.DTWAIN_InitImageFileAppendAFunc)(szFile, fType);  }
    }

    pub fn DTWAIN_InitImageFileAppendW(&self, szFile: *const u16, fType: i32) -> i32 {
        unsafe { return (self.DTWAIN_InitImageFileAppendWFunc)(szFile, fType);  }
    }

    pub fn DTWAIN_InitOCRInterface(&self) -> i32 {
        unsafe { return (self.DTWAIN_InitOCRInterfaceFunc)();  }
    }

    pub fn DTWAIN_IsAcquiring(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsAcquiringFunc)();  }
    }

    pub fn DTWAIN_IsAudioXferSupported(&self, Source: *mut c_void, supportVal: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsAudioXferSupportedFunc)(Source, supportVal);  }
    }

    pub fn DTWAIN_IsAutoBorderDetectEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoBorderDetectEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoBorderDetectSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoBorderDetectSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoBrightEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoBrightEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoBrightSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoBrightSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoDeskewEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoDeskewEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoDeskewSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoDeskewSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoFeedEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoFeedEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoFeedSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoFeedSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoRotateEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoRotateEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoRotateSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoRotateSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutoScanEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutoScanEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutomaticSenseMediumEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutomaticSenseMediumEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsAutomaticSenseMediumSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsAutomaticSenseMediumSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsBlankPageDetectionOn(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsBlankPageDetectionOnFunc)(Source);  }
    }

    pub fn DTWAIN_IsBufferedTileModeOn(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsBufferedTileModeOnFunc)(Source);  }
    }

    pub fn DTWAIN_IsBufferedTileModeSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsBufferedTileModeSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsCapSupported(&self, Source: *mut c_void, lCapability: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsCapSupportedFunc)(Source, lCapability);  }
    }

    pub fn DTWAIN_IsCompressionSupported(&self, Source: *mut c_void, Compression: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsCompressionSupportedFunc)(Source, Compression);  }
    }

    pub fn DTWAIN_IsCustomDSDataSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsCustomDSDataSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsDIBBlank(&self, hDib: *mut c_void, threshold: f64) -> i32 {
        unsafe { return (self.DTWAIN_IsDIBBlankFunc)(hDib, threshold);  }
    }

    pub fn DTWAIN_IsDIBBlankString(&self, hDib: *mut c_void, threshold: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_IsDIBBlankStringFunc)(hDib, threshold);  }
    }

    pub fn DTWAIN_IsDIBBlankStringA(&self, hDib: *mut c_void, threshold: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_IsDIBBlankStringAFunc)(hDib, threshold);  }
    }

    pub fn DTWAIN_IsDIBBlankStringW(&self, hDib: *mut c_void, threshold: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_IsDIBBlankStringWFunc)(hDib, threshold);  }
    }

    pub fn DTWAIN_IsDeviceEventSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsDeviceEventSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsDeviceOnLine(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsDeviceOnLineFunc)(Source);  }
    }

    pub fn DTWAIN_IsDoubleFeedDetectLengthSupported(&self, Source: *mut c_void, value: f64) -> i32 {
        unsafe { return (self.DTWAIN_IsDoubleFeedDetectLengthSupportedFunc)(Source, value);  }
    }

    pub fn DTWAIN_IsDoubleFeedDetectSupported(&self, Source: *mut c_void, SupportVal: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsDoubleFeedDetectSupportedFunc)(Source, SupportVal);  }
    }

    pub fn DTWAIN_IsDuplexEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsDuplexEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsDuplexSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsDuplexSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsExtImageInfoSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsExtImageInfoSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsFeederEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsFeederEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsFeederLoaded(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsFeederLoadedFunc)(Source);  }
    }

    pub fn DTWAIN_IsFeederSensitive(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsFeederSensitiveFunc)(Source);  }
    }

    pub fn DTWAIN_IsFeederSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsFeederSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsFileSystemSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsFileSystemSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsFileXferSupported(&self, Source: *mut c_void, lFileType: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsFileXferSupportedFunc)(Source, lFileType);  }
    }

    pub fn DTWAIN_IsIAFieldALastPageSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldALastPageSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldALevelSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldALevelSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldAPrintFormatSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldAPrintFormatSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldAValueSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldAValueSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldBLastPageSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldBLastPageSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldBLevelSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldBLevelSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldBPrintFormatSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldBPrintFormatSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldBValueSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldBValueSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldCLastPageSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldCLastPageSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldCLevelSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldCLevelSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldCPrintFormatSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldCPrintFormatSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldCValueSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldCValueSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldDLastPageSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldDLastPageSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldDLevelSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldDLevelSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldDPrintFormatSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldDPrintFormatSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldDValueSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldDValueSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldELastPageSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldELastPageSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldELevelSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldELevelSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldEPrintFormatSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldEPrintFormatSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIAFieldEValueSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIAFieldEValueSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsImageAddressingSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsImageAddressingSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsIndicatorEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIndicatorEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsIndicatorSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsIndicatorSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsInitialized(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsInitializedFunc)();  }
    }

    pub fn DTWAIN_IsJPEGSupported(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsJPEGSupportedFunc)();  }
    }

    pub fn DTWAIN_IsJobControlSupported(&self, Source: *mut c_void, JobControl: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsJobControlSupportedFunc)(Source, JobControl);  }
    }

    pub fn DTWAIN_IsLampEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsLampEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsLampSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsLampSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsLightPathSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsLightPathSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsLightSourceSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsLightSourceSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsMaxBuffersSupported(&self, Source: *mut c_void, MaxBuf: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsMaxBuffersSupportedFunc)(Source, MaxBuf);  }
    }

    pub fn DTWAIN_IsMemFileXferSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsMemFileXferSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsMsgNotifyEnabled(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsMsgNotifyEnabledFunc)();  }
    }

    pub fn DTWAIN_IsNotifyTripletsEnabled(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsNotifyTripletsEnabledFunc)();  }
    }

    pub fn DTWAIN_IsOCREngineActivated(&self, OCREngine: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsOCREngineActivatedFunc)(OCREngine);  }
    }

    pub fn DTWAIN_IsOpenSourcesOnSelect(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsOpenSourcesOnSelectFunc)();  }
    }

    pub fn DTWAIN_IsOrientationSupported(&self, Source: *mut c_void, Orientation: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsOrientationSupportedFunc)(Source, Orientation);  }
    }

    pub fn DTWAIN_IsOverscanSupported(&self, Source: *mut c_void, SupportValue: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsOverscanSupportedFunc)(Source, SupportValue);  }
    }

    pub fn DTWAIN_IsPDFSupported(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsPDFSupportedFunc)();  }
    }

    pub fn DTWAIN_IsPNGSupported(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsPNGSupportedFunc)();  }
    }

    pub fn DTWAIN_IsPaperDetectable(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsPaperDetectableFunc)(Source);  }
    }

    pub fn DTWAIN_IsPaperSizeSupported(&self, Source: *mut c_void, PaperSize: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsPaperSizeSupportedFunc)(Source, PaperSize);  }
    }

    pub fn DTWAIN_IsPatchCapsSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsPatchCapsSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsPatchDetectEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsPatchDetectEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsPatchSupported(&self, Source: *mut c_void, PatchCode: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsPatchSupportedFunc)(Source, PatchCode);  }
    }

    pub fn DTWAIN_IsPeekMessageLoopEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsPeekMessageLoopEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsPixelTypeSupported(&self, Source: *mut c_void, PixelType: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsPixelTypeSupportedFunc)(Source, PixelType);  }
    }

    pub fn DTWAIN_IsPrinterEnabled(&self, Source: *mut c_void, Printer: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsPrinterEnabledFunc)(Source, Printer);  }
    }

    pub fn DTWAIN_IsPrinterSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsPrinterSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsRotationSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsRotationSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsSessionEnabled(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsSessionEnabledFunc)();  }
    }

    pub fn DTWAIN_IsSkipImageInfoError(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsSkipImageInfoErrorFunc)(Source);  }
    }

    pub fn DTWAIN_IsSourceAcquiring(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsSourceAcquiringFunc)(Source);  }
    }

    pub fn DTWAIN_IsSourceAcquiringEx(&self, Source: *mut c_void, bUIOnly: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsSourceAcquiringExFunc)(Source, bUIOnly);  }
    }

    pub fn DTWAIN_IsSourceInUIOnlyMode(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsSourceInUIOnlyModeFunc)(Source);  }
    }

    pub fn DTWAIN_IsSourceOpen(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsSourceOpenFunc)(Source);  }
    }

    pub fn DTWAIN_IsSourceSelected(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsSourceSelectedFunc)(Source);  }
    }

    pub fn DTWAIN_IsSourceValid(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsSourceValidFunc)(Source);  }
    }

    pub fn DTWAIN_IsTIFFSupported(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsTIFFSupportedFunc)();  }
    }

    pub fn DTWAIN_IsThumbnailEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsThumbnailEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsThumbnailSupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsThumbnailSupportedFunc)(Source);  }
    }

    pub fn DTWAIN_IsTwainAvailable(&self) -> i32 {
        unsafe { return (self.DTWAIN_IsTwainAvailableFunc)();  }
    }

    pub fn DTWAIN_IsTwainAvailableEx(&self, directories: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsTwainAvailableExFunc)(directories, nMaxLen);  }
    }

    pub fn DTWAIN_IsTwainAvailableExA(&self, directories: *mut c_char, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsTwainAvailableExAFunc)(directories, nMaxLen);  }
    }

    pub fn DTWAIN_IsTwainAvailableExW(&self, directories: *mut u16, nMaxLen: i32) -> i32 {
        unsafe { return (self.DTWAIN_IsTwainAvailableExWFunc)(directories, nMaxLen);  }
    }

    pub fn DTWAIN_IsUIControllable(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsUIControllableFunc)(Source);  }
    }

    pub fn DTWAIN_IsUIEnabled(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsUIEnabledFunc)(Source);  }
    }

    pub fn DTWAIN_IsUIOnlySupported(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_IsUIOnlySupportedFunc)(Source);  }
    }

    pub fn DTWAIN_LoadCustomStringResources(&self, sLangDLL: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_LoadCustomStringResourcesFunc)(sLangDLL);  }
    }

    pub fn DTWAIN_LoadCustomStringResourcesA(&self, sLangDLL: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_LoadCustomStringResourcesAFunc)(sLangDLL);  }
    }

    pub fn DTWAIN_LoadCustomStringResourcesEx(&self, sLangDLL: *const u16, bClear: i32) -> i32 {
        unsafe { return (self.DTWAIN_LoadCustomStringResourcesExFunc)(sLangDLL, bClear);  }
    }

    pub fn DTWAIN_LoadCustomStringResourcesExA(&self, sLangDLL: *const c_char, bClear: i32) -> i32 {
        unsafe { return (self.DTWAIN_LoadCustomStringResourcesExAFunc)(sLangDLL, bClear);  }
    }

    pub fn DTWAIN_LoadCustomStringResourcesExW(&self, sLangDLL: *const u16, bClear: i32) -> i32 {
        unsafe { return (self.DTWAIN_LoadCustomStringResourcesExWFunc)(sLangDLL, bClear);  }
    }

    pub fn DTWAIN_LoadCustomStringResourcesW(&self, sLangDLL: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_LoadCustomStringResourcesWFunc)(sLangDLL);  }
    }

    pub fn DTWAIN_LoadLanguageResource(&self, nLanguage: i32) -> i32 {
        unsafe { return (self.DTWAIN_LoadLanguageResourceFunc)(nLanguage);  }
    }

    pub fn DTWAIN_LockMemory(&self, h: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_LockMemoryFunc)(h);  }
    }

    pub fn DTWAIN_LockMemoryEx(&self, h: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_LockMemoryExFunc)(h);  }
    }

    pub fn DTWAIN_LogMessage(&self, message: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_LogMessageFunc)(message);  }
    }

    pub fn DTWAIN_LogMessageA(&self, message: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_LogMessageAFunc)(message);  }
    }

    pub fn DTWAIN_LogMessageW(&self, message: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_LogMessageWFunc)(message);  }
    }

    pub fn DTWAIN_MakeRGB(&self, red: i32, green: i32, blue: i32) -> i32 {
        unsafe { return (self.DTWAIN_MakeRGBFunc)(red, green, blue);  }
    }

    pub fn DTWAIN_OpenSource(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_OpenSourceFunc)(Source);  }
    }

    pub fn DTWAIN_OpenSourcesOnSelect(&self, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_OpenSourcesOnSelectFunc)(bSet);  }
    }

    pub fn DTWAIN_RangeCreate(&self, nEnumType: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_RangeCreateFunc)(nEnumType);  }
    }

    pub fn DTWAIN_RangeCreateFromCap(&self, Source: *mut c_void, lCapType: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_RangeCreateFromCapFunc)(Source, lCapType);  }
    }

    pub fn DTWAIN_RangeDestroy(&self, pSource: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeDestroyFunc)(pSource);  }
    }

    pub fn DTWAIN_RangeExpand(&self, pSource: *mut c_void, pArray: *mut *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeExpandFunc)(pSource, pArray);  }
    }

    pub fn DTWAIN_RangeExpandEx(&self, Range: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_RangeExpandExFunc)(Range);  }
    }

    pub fn DTWAIN_RangeGetAll(&self, pArray: *mut c_void, pVariantLow: *mut c_void, pVariantUp: *mut c_void, pVariantStep: *mut c_void, pVariantDefault: *mut c_void, pVariantCurrent: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetAllFunc)(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent);  }
    }

    pub fn DTWAIN_RangeGetAllFloat(&self, pArray: *mut c_void, pVariantLow: *mut f64, pVariantUp: *mut f64, pVariantStep: *mut f64, pVariantDefault: *mut f64, pVariantCurrent: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetAllFloatFunc)(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent);  }
    }

    pub fn DTWAIN_RangeGetAllFloatString(&self, pArray: *mut c_void, dLow: *mut u16, dUp: *mut u16, dStep: *mut u16, dDefault: *mut u16, dCurrent: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetAllFloatStringFunc)(pArray, dLow, dUp, dStep, dDefault, dCurrent);  }
    }

    pub fn DTWAIN_RangeGetAllFloatStringA(&self, pArray: *mut c_void, dLow: *mut c_char, dUp: *mut c_char, dStep: *mut c_char, dDefault: *mut c_char, dCurrent: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetAllFloatStringAFunc)(pArray, dLow, dUp, dStep, dDefault, dCurrent);  }
    }

    pub fn DTWAIN_RangeGetAllFloatStringW(&self, pArray: *mut c_void, dLow: *mut u16, dUp: *mut u16, dStep: *mut u16, dDefault: *mut u16, dCurrent: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetAllFloatStringWFunc)(pArray, dLow, dUp, dStep, dDefault, dCurrent);  }
    }

    pub fn DTWAIN_RangeGetAllLong(&self, pArray: *mut c_void, pVariantLow: *mut i32, pVariantUp: *mut i32, pVariantStep: *mut i32, pVariantDefault: *mut i32, pVariantCurrent: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetAllLongFunc)(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent);  }
    }

    pub fn DTWAIN_RangeGetCount(&self, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetCountFunc)(pArray);  }
    }

    pub fn DTWAIN_RangeGetExpValue(&self, pArray: *mut c_void, lPos: i32, pVariant: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetExpValueFunc)(pArray, lPos, pVariant);  }
    }

    pub fn DTWAIN_RangeGetExpValueFloat(&self, pArray: *mut c_void, lPos: i32, pVal: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetExpValueFloatFunc)(pArray, lPos, pVal);  }
    }

    pub fn DTWAIN_RangeGetExpValueFloatString(&self, pArray: *mut c_void, lPos: i32, pVal: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetExpValueFloatStringFunc)(pArray, lPos, pVal);  }
    }

    pub fn DTWAIN_RangeGetExpValueFloatStringA(&self, pArray: *mut c_void, lPos: i32, pVal: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetExpValueFloatStringAFunc)(pArray, lPos, pVal);  }
    }

    pub fn DTWAIN_RangeGetExpValueFloatStringW(&self, pArray: *mut c_void, lPos: i32, pVal: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetExpValueFloatStringWFunc)(pArray, lPos, pVal);  }
    }

    pub fn DTWAIN_RangeGetExpValueLong(&self, pArray: *mut c_void, lPos: i32, pVal: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetExpValueLongFunc)(pArray, lPos, pVal);  }
    }

    pub fn DTWAIN_RangeGetNearestValue(&self, pArray: *mut c_void, pVariantIn: *mut c_void, pVariantOut: *mut c_void, RoundType: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetNearestValueFunc)(pArray, pVariantIn, pVariantOut, RoundType);  }
    }

    pub fn DTWAIN_RangeGetPos(&self, pArray: *mut c_void, pVariant: *mut c_void, pPos: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetPosFunc)(pArray, pVariant, pPos);  }
    }

    pub fn DTWAIN_RangeGetPosFloat(&self, pArray: *mut c_void, Val: f64, pPos: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetPosFloatFunc)(pArray, Val, pPos);  }
    }

    pub fn DTWAIN_RangeGetPosFloatString(&self, pArray: *mut c_void, Val: *const u16, pPos: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetPosFloatStringFunc)(pArray, Val, pPos);  }
    }

    pub fn DTWAIN_RangeGetPosFloatStringA(&self, pArray: *mut c_void, Val: *const c_char, pPos: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetPosFloatStringAFunc)(pArray, Val, pPos);  }
    }

    pub fn DTWAIN_RangeGetPosFloatStringW(&self, pArray: *mut c_void, Val: *const u16, pPos: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetPosFloatStringWFunc)(pArray, Val, pPos);  }
    }

    pub fn DTWAIN_RangeGetPosLong(&self, pArray: *mut c_void, Value: i32, pPos: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetPosLongFunc)(pArray, Value, pPos);  }
    }

    pub fn DTWAIN_RangeGetValue(&self, pArray: *mut c_void, nWhich: i32, pVariant: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetValueFunc)(pArray, nWhich, pVariant);  }
    }

    pub fn DTWAIN_RangeGetValueFloat(&self, pArray: *mut c_void, nWhich: i32, pVal: *mut f64) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetValueFloatFunc)(pArray, nWhich, pVal);  }
    }

    pub fn DTWAIN_RangeGetValueFloatString(&self, pArray: *mut c_void, nWhich: i32, pVal: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetValueFloatStringFunc)(pArray, nWhich, pVal);  }
    }

    pub fn DTWAIN_RangeGetValueFloatStringA(&self, pArray: *mut c_void, nWhich: i32, dValue: *mut c_char) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetValueFloatStringAFunc)(pArray, nWhich, dValue);  }
    }

    pub fn DTWAIN_RangeGetValueFloatStringW(&self, pArray: *mut c_void, nWhich: i32, dValue: *mut u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetValueFloatStringWFunc)(pArray, nWhich, dValue);  }
    }

    pub fn DTWAIN_RangeGetValueLong(&self, pArray: *mut c_void, nWhich: i32, pVal: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeGetValueLongFunc)(pArray, nWhich, pVal);  }
    }

    pub fn DTWAIN_RangeIsValid(&self, Range: *mut c_void, pStatus: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeIsValidFunc)(Range, pStatus);  }
    }

    pub fn DTWAIN_RangeNearestValueFloat(&self, pArray: *mut c_void, dIn: f64, pOut: *mut f64, RoundType: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeNearestValueFloatFunc)(pArray, dIn, pOut, RoundType);  }
    }

    pub fn DTWAIN_RangeNearestValueFloatString(&self, pArray: *mut c_void, dIn: *const u16, pOut: *mut u16, RoundType: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeNearestValueFloatStringFunc)(pArray, dIn, pOut, RoundType);  }
    }

    pub fn DTWAIN_RangeNearestValueFloatStringA(&self, pArray: *mut c_void, dIn: *const c_char, dOut: *mut c_char, RoundType: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeNearestValueFloatStringAFunc)(pArray, dIn, dOut, RoundType);  }
    }

    pub fn DTWAIN_RangeNearestValueFloatStringW(&self, pArray: *mut c_void, dIn: *const u16, dOut: *mut u16, RoundType: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeNearestValueFloatStringWFunc)(pArray, dIn, dOut, RoundType);  }
    }

    pub fn DTWAIN_RangeNearestValueLong(&self, pArray: *mut c_void, lIn: i32, pOut: *mut i32, RoundType: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeNearestValueLongFunc)(pArray, lIn, pOut, RoundType);  }
    }

    pub fn DTWAIN_RangeSetAll(&self, pArray: *mut c_void, pVariantLow: *mut c_void, pVariantUp: *mut c_void, pVariantStep: *mut c_void, pVariantDefault: *mut c_void, pVariantCurrent: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetAllFunc)(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent);  }
    }

    pub fn DTWAIN_RangeSetAllFloat(&self, pArray: *mut c_void, dLow: f64, dUp: f64, dStep: f64, dDefault: f64, dCurrent: f64) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetAllFloatFunc)(pArray, dLow, dUp, dStep, dDefault, dCurrent);  }
    }

    pub fn DTWAIN_RangeSetAllFloatString(&self, pArray: *mut c_void, dLow: *const u16, dUp: *const u16, dStep: *const u16, dDefault: *const u16, dCurrent: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetAllFloatStringFunc)(pArray, dLow, dUp, dStep, dDefault, dCurrent);  }
    }

    pub fn DTWAIN_RangeSetAllFloatStringA(&self, pArray: *mut c_void, dLow: *const c_char, dUp: *const c_char, dStep: *const c_char, dDefault: *const c_char, dCurrent: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetAllFloatStringAFunc)(pArray, dLow, dUp, dStep, dDefault, dCurrent);  }
    }

    pub fn DTWAIN_RangeSetAllFloatStringW(&self, pArray: *mut c_void, dLow: *const u16, dUp: *const u16, dStep: *const u16, dDefault: *const u16, dCurrent: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetAllFloatStringWFunc)(pArray, dLow, dUp, dStep, dDefault, dCurrent);  }
    }

    pub fn DTWAIN_RangeSetAllLong(&self, pArray: *mut c_void, lLow: i32, lUp: i32, lStep: i32, lDefault: i32, lCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetAllLongFunc)(pArray, lLow, lUp, lStep, lDefault, lCurrent);  }
    }

    pub fn DTWAIN_RangeSetValue(&self, pArray: *mut c_void, nWhich: i32, pVal: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetValueFunc)(pArray, nWhich, pVal);  }
    }

    pub fn DTWAIN_RangeSetValueFloat(&self, pArray: *mut c_void, nWhich: i32, Val: f64) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetValueFloatFunc)(pArray, nWhich, Val);  }
    }

    pub fn DTWAIN_RangeSetValueFloatString(&self, pArray: *mut c_void, nWhich: i32, Val: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetValueFloatStringFunc)(pArray, nWhich, Val);  }
    }

    pub fn DTWAIN_RangeSetValueFloatStringA(&self, pArray: *mut c_void, nWhich: i32, dValue: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetValueFloatStringAFunc)(pArray, nWhich, dValue);  }
    }

    pub fn DTWAIN_RangeSetValueFloatStringW(&self, pArray: *mut c_void, nWhich: i32, dValue: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetValueFloatStringWFunc)(pArray, nWhich, dValue);  }
    }

    pub fn DTWAIN_RangeSetValueLong(&self, pArray: *mut c_void, nWhich: i32, Val: i32) -> i32 {
        unsafe { return (self.DTWAIN_RangeSetValueLongFunc)(pArray, nWhich, Val);  }
    }

    pub fn DTWAIN_ResetPDFTextElement(&self, TextElement: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ResetPDFTextElementFunc)(TextElement);  }
    }

    pub fn DTWAIN_RewindPage(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_RewindPageFunc)(Source);  }
    }

    pub fn DTWAIN_SelectDefaultOCREngine(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectDefaultOCREngineFunc)();  }
    }

    pub fn DTWAIN_SelectDefaultSource(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectDefaultSourceFunc)();  }
    }

    pub fn DTWAIN_SelectDefaultSourceWithOpen(&self, bOpen: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectDefaultSourceWithOpenFunc)(bOpen);  }
    }

    pub fn DTWAIN_SelectOCREngine(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngineFunc)();  }
    }

    pub fn DTWAIN_SelectOCREngine2(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngine2Func)(hWndParent, szTitle, xPos, yPos, nOptions);  }
    }

    pub fn DTWAIN_SelectOCREngine2A(&self, hWndParent: *const c_void, szTitle: *const c_char, xPos: i32, yPos: i32, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngine2AFunc)(hWndParent, szTitle, xPos, yPos, nOptions);  }
    }

    pub fn DTWAIN_SelectOCREngine2Ex(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, szIncludeFilter: *const u16, szExcludeFilter: *const u16, szNameMapping: *const u16, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngine2ExFunc)(hWndParent, szTitle, xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions);  }
    }

    pub fn DTWAIN_SelectOCREngine2ExA(&self, hWndParent: *const c_void, szTitle: *const c_char, xPos: i32, yPos: i32, szIncludeNames: *const c_char, szExcludeNames: *const c_char, szNameMapping: *const c_char, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngine2ExAFunc)(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);  }
    }

    pub fn DTWAIN_SelectOCREngine2ExW(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, szIncludeNames: *const u16, szExcludeNames: *const u16, szNameMapping: *const u16, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngine2ExWFunc)(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);  }
    }

    pub fn DTWAIN_SelectOCREngine2W(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngine2WFunc)(hWndParent, szTitle, xPos, yPos, nOptions);  }
    }

    pub fn DTWAIN_SelectOCREngineByName(&self, lpszName: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngineByNameFunc)(lpszName);  }
    }

    pub fn DTWAIN_SelectOCREngineByNameA(&self, lpszName: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngineByNameAFunc)(lpszName);  }
    }

    pub fn DTWAIN_SelectOCREngineByNameW(&self, lpszName: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectOCREngineByNameWFunc)(lpszName);  }
    }

    pub fn DTWAIN_SelectSource(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceFunc)();  }
    }

    pub fn DTWAIN_SelectSource2(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSource2Func)(hWndParent, szTitle, xPos, yPos, nOptions);  }
    }

    pub fn DTWAIN_SelectSource2A(&self, hWndParent: *const c_void, szTitle: *const c_char, xPos: i32, yPos: i32, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSource2AFunc)(hWndParent, szTitle, xPos, yPos, nOptions);  }
    }

    pub fn DTWAIN_SelectSource2Ex(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, szIncludeFilter: *const u16, szExcludeFilter: *const u16, szNameMapping: *const u16, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSource2ExFunc)(hWndParent, szTitle, xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions);  }
    }

    pub fn DTWAIN_SelectSource2ExA(&self, hWndParent: *const c_void, szTitle: *const c_char, xPos: i32, yPos: i32, szIncludeNames: *const c_char, szExcludeNames: *const c_char, szNameMapping: *const c_char, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSource2ExAFunc)(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);  }
    }

    pub fn DTWAIN_SelectSource2ExW(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, szIncludeNames: *const u16, szExcludeNames: *const u16, szNameMapping: *const u16, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSource2ExWFunc)(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions);  }
    }

    pub fn DTWAIN_SelectSource2W(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, nOptions: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSource2WFunc)(hWndParent, szTitle, xPos, yPos, nOptions);  }
    }

    pub fn DTWAIN_SelectSourceByName(&self, lpszName: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceByNameFunc)(lpszName);  }
    }

    pub fn DTWAIN_SelectSourceByNameA(&self, lpszName: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceByNameAFunc)(lpszName);  }
    }

    pub fn DTWAIN_SelectSourceByNameW(&self, lpszName: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceByNameWFunc)(lpszName);  }
    }

    pub fn DTWAIN_SelectSourceByNameWithOpen(&self, lpszName: *const u16, bOpen: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceByNameWithOpenFunc)(lpszName, bOpen);  }
    }

    pub fn DTWAIN_SelectSourceByNameWithOpenA(&self, lpszName: *const c_char, bOpen: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceByNameWithOpenAFunc)(lpszName, bOpen);  }
    }

    pub fn DTWAIN_SelectSourceByNameWithOpenW(&self, lpszName: *const u16, bOpen: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceByNameWithOpenWFunc)(lpszName, bOpen);  }
    }

    pub fn DTWAIN_SelectSourceWithOpen(&self, bOpen: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_SelectSourceWithOpenFunc)(bOpen);  }
    }

    pub fn DTWAIN_SetAcquireArea(&self, Source: *mut c_void, lSetType: i32, FloatEnum: *mut c_void, ActualEnum: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireAreaFunc)(Source, lSetType, FloatEnum, ActualEnum);  }
    }

    pub fn DTWAIN_SetAcquireArea2(&self, Source: *mut c_void, left: f64, top: f64, right: f64, bottom: f64, lUnit: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireArea2Func)(Source, left, top, right, bottom, lUnit, Flags);  }
    }

    pub fn DTWAIN_SetAcquireArea2String(&self, Source: *mut c_void, left: *const u16, top: *const u16, right: *const u16, bottom: *const u16, lUnit: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireArea2StringFunc)(Source, left, top, right, bottom, lUnit, Flags);  }
    }

    pub fn DTWAIN_SetAcquireArea2StringA(&self, Source: *mut c_void, left: *const c_char, top: *const c_char, right: *const c_char, bottom: *const c_char, lUnit: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireArea2StringAFunc)(Source, left, top, right, bottom, lUnit, Flags);  }
    }

    pub fn DTWAIN_SetAcquireArea2StringW(&self, Source: *mut c_void, left: *const u16, top: *const u16, right: *const u16, bottom: *const u16, lUnit: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireArea2StringWFunc)(Source, left, top, right, bottom, lUnit, Flags);  }
    }

    pub fn DTWAIN_SetAcquireImageNegative(&self, Source: *mut c_void, IsNegative: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireImageNegativeFunc)(Source, IsNegative);  }
    }

    pub fn DTWAIN_SetAcquireImageScale(&self, Source: *mut c_void, xscale: f64, yscale: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireImageScaleFunc)(Source, xscale, yscale);  }
    }

    pub fn DTWAIN_SetAcquireImageScaleString(&self, Source: *mut c_void, xscale: *const u16, yscale: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireImageScaleStringFunc)(Source, xscale, yscale);  }
    }

    pub fn DTWAIN_SetAcquireImageScaleStringA(&self, Source: *mut c_void, xscale: *const c_char, yscale: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireImageScaleStringAFunc)(Source, xscale, yscale);  }
    }

    pub fn DTWAIN_SetAcquireImageScaleStringW(&self, Source: *mut c_void, xscale: *const u16, yscale: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireImageScaleStringWFunc)(Source, xscale, yscale);  }
    }

    pub fn DTWAIN_SetAcquireStripBuffer(&self, Source: *mut c_void, hMem: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireStripBufferFunc)(Source, hMem);  }
    }

    pub fn DTWAIN_SetAcquireStripSize(&self, Source: *mut c_void, StripSize: u32) -> i32 {
        unsafe { return (self.DTWAIN_SetAcquireStripSizeFunc)(Source, StripSize);  }
    }

    pub fn DTWAIN_SetAlarmVolume(&self, Source: *mut c_void, Volume: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetAlarmVolumeFunc)(Source, Volume);  }
    }

    pub fn DTWAIN_SetAlarms(&self, Source: *mut c_void, Alarms: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetAlarmsFunc)(Source, Alarms);  }
    }

    pub fn DTWAIN_SetAllCapsToDefault(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetAllCapsToDefaultFunc)(Source);  }
    }

    pub fn DTWAIN_SetAppInfo(&self, szVerStr: *const u16, szManu: *const u16, szProdFam: *const u16, szProdName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetAppInfoFunc)(szVerStr, szManu, szProdFam, szProdName);  }
    }

    pub fn DTWAIN_SetAppInfoA(&self, szVerStr: *const c_char, szManu: *const c_char, szProdFam: *const c_char, szProdName: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetAppInfoAFunc)(szVerStr, szManu, szProdFam, szProdName);  }
    }

    pub fn DTWAIN_SetAppInfoW(&self, szVerStr: *const u16, szManu: *const u16, szProdFam: *const u16, szProdName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetAppInfoWFunc)(szVerStr, szManu, szProdFam, szProdName);  }
    }

    pub fn DTWAIN_SetAuthor(&self, Source: *mut c_void, szAuthor: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetAuthorFunc)(Source, szAuthor);  }
    }

    pub fn DTWAIN_SetAuthorA(&self, Source: *mut c_void, szAuthor: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetAuthorAFunc)(Source, szAuthor);  }
    }

    pub fn DTWAIN_SetAuthorW(&self, Source: *mut c_void, szAuthor: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetAuthorWFunc)(Source, szAuthor);  }
    }

    pub fn DTWAIN_SetAvailablePrinters(&self, Source: *mut c_void, lpAvailPrinters: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetAvailablePrintersFunc)(Source, lpAvailPrinters);  }
    }

    pub fn DTWAIN_SetAvailablePrintersArray(&self, Source: *mut c_void, AvailPrinters: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetAvailablePrintersArrayFunc)(Source, AvailPrinters);  }
    }

    pub fn DTWAIN_SetBitDepth(&self, Source: *mut c_void, BitDepth: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBitDepthFunc)(Source, BitDepth, bSetCurrent);  }
    }

    pub fn DTWAIN_SetBlankPageDetection(&self, Source: *mut c_void, threshold: f64, discard_option: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionFunc)(Source, threshold, discard_option, bSet);  }
    }

    pub fn DTWAIN_SetBlankPageDetectionEx(&self, Source: *mut c_void, threshold: f64, autodetect: i32, detectOpts: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionExFunc)(Source, threshold, autodetect, detectOpts, bSet);  }
    }

    pub fn DTWAIN_SetBlankPageDetectionExString(&self, Source: *mut c_void, threshold: *const u16, autodetect_option: i32, detectOpts: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionExStringFunc)(Source, threshold, autodetect_option, detectOpts, bSet);  }
    }

    pub fn DTWAIN_SetBlankPageDetectionExStringA(&self, Source: *mut c_void, threshold: *const c_char, autodetect_option: i32, detectOpts: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionExStringAFunc)(Source, threshold, autodetect_option, detectOpts, bSet);  }
    }

    pub fn DTWAIN_SetBlankPageDetectionExStringW(&self, Source: *mut c_void, threshold: *const u16, autodetect_option: i32, detectOpts: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionExStringWFunc)(Source, threshold, autodetect_option, detectOpts, bSet);  }
    }

    pub fn DTWAIN_SetBlankPageDetectionString(&self, Source: *mut c_void, threshold: *const u16, autodetect_option: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionStringFunc)(Source, threshold, autodetect_option, bSet);  }
    }

    pub fn DTWAIN_SetBlankPageDetectionStringA(&self, Source: *mut c_void, threshold: *const c_char, autodetect_option: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionStringAFunc)(Source, threshold, autodetect_option, bSet);  }
    }

    pub fn DTWAIN_SetBlankPageDetectionStringW(&self, Source: *mut c_void, threshold: *const u16, autodetect_option: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBlankPageDetectionStringWFunc)(Source, threshold, autodetect_option, bSet);  }
    }

    pub fn DTWAIN_SetBrightness(&self, Source: *mut c_void, Brightness: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetBrightnessFunc)(Source, Brightness);  }
    }

    pub fn DTWAIN_SetBrightnessString(&self, Source: *mut c_void, Brightness: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetBrightnessStringFunc)(Source, Brightness);  }
    }

    pub fn DTWAIN_SetBrightnessStringA(&self, Source: *mut c_void, Contrast: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetBrightnessStringAFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_SetBrightnessStringW(&self, Source: *mut c_void, Contrast: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetBrightnessStringWFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_SetBufferedTileMode(&self, Source: *mut c_void, bTileMode: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetBufferedTileModeFunc)(Source, bTileMode);  }
    }

    pub fn DTWAIN_SetCallback(&self, Fn: DTWAIN_CALLBACK_PROC, UserData: i32) -> DTWAIN_CALLBACK_PROC {
        unsafe { return (self.DTWAIN_SetCallbackFunc)(Fn, UserData);  }
    }

    pub fn DTWAIN_SetCallback64(&self, Fn: DTWAIN_CALLBACK_PROC64, UserData: i64) -> DTWAIN_CALLBACK_PROC64 {
        unsafe { return (self.DTWAIN_SetCallback64Func)(Fn, UserData);  }
    }

    pub fn DTWAIN_SetCamera(&self, Source: *mut c_void, szCamera: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetCameraFunc)(Source, szCamera);  }
    }

    pub fn DTWAIN_SetCameraA(&self, Source: *mut c_void, szCamera: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetCameraAFunc)(Source, szCamera);  }
    }

    pub fn DTWAIN_SetCameraW(&self, Source: *mut c_void, szCamera: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetCameraWFunc)(Source, szCamera);  }
    }

    pub fn DTWAIN_SetCapValues(&self, Source: *mut c_void, lCap: i32, lSetType: i32, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetCapValuesFunc)(Source, lCap, lSetType, pArray);  }
    }

    pub fn DTWAIN_SetCapValuesEx(&self, Source: *mut c_void, lCap: i32, lSetType: i32, lContainerType: i32, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetCapValuesExFunc)(Source, lCap, lSetType, lContainerType, pArray);  }
    }

    pub fn DTWAIN_SetCapValuesEx2(&self, Source: *mut c_void, lCap: i32, lSetType: i32, lContainerType: i32, nDataType: i32, pArray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetCapValuesEx2Func)(Source, lCap, lSetType, lContainerType, nDataType, pArray);  }
    }

    pub fn DTWAIN_SetCaption(&self, Source: *mut c_void, Caption: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetCaptionFunc)(Source, Caption);  }
    }

    pub fn DTWAIN_SetCaptionA(&self, Source: *mut c_void, Caption: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetCaptionAFunc)(Source, Caption);  }
    }

    pub fn DTWAIN_SetCaptionW(&self, Source: *mut c_void, Caption: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetCaptionWFunc)(Source, Caption);  }
    }

    pub fn DTWAIN_SetCompressionType(&self, Source: *mut c_void, lCompression: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetCompressionTypeFunc)(Source, lCompression, bSetCurrent);  }
    }

    pub fn DTWAIN_SetContrast(&self, Source: *mut c_void, Contrast: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetContrastFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_SetContrastString(&self, Source: *mut c_void, Contrast: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetContrastStringFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_SetContrastStringA(&self, Source: *mut c_void, Contrast: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetContrastStringAFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_SetContrastStringW(&self, Source: *mut c_void, Contrast: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetContrastStringWFunc)(Source, Contrast);  }
    }

    pub fn DTWAIN_SetCountry(&self, nCountry: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetCountryFunc)(nCountry);  }
    }

    pub fn DTWAIN_SetCurrentRetryCount(&self, Source: *mut c_void, nCount: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetCurrentRetryCountFunc)(Source, nCount);  }
    }

    pub fn DTWAIN_SetCustomDSData(&self, Source: *mut c_void, hData: *mut c_void, Data: *const u8, dSize: u32, nFlags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetCustomDSDataFunc)(Source, hData, Data, dSize, nFlags);  }
    }

    pub fn DTWAIN_SetDSMSearchOrder(&self, SearchPath: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetDSMSearchOrderFunc)(SearchPath);  }
    }

    pub fn DTWAIN_SetDSMSearchOrderEx(&self, SearchOrder: *const u16, UserPath: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetDSMSearchOrderExFunc)(SearchOrder, UserPath);  }
    }

    pub fn DTWAIN_SetDSMSearchOrderExA(&self, SearchOrder: *const c_char, szUserPath: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetDSMSearchOrderExAFunc)(SearchOrder, szUserPath);  }
    }

    pub fn DTWAIN_SetDSMSearchOrderExW(&self, SearchOrder: *const u16, szUserPath: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetDSMSearchOrderExWFunc)(SearchOrder, szUserPath);  }
    }

    pub fn DTWAIN_SetDefaultSource(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetDefaultSourceFunc)(Source);  }
    }

    pub fn DTWAIN_SetDeviceNotifications(&self, Source: *mut c_void, DevEvents: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetDeviceNotificationsFunc)(Source, DevEvents);  }
    }

    pub fn DTWAIN_SetDeviceTimeDate(&self, Source: *mut c_void, szTimeDate: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetDeviceTimeDateFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_SetDeviceTimeDateA(&self, Source: *mut c_void, szTimeDate: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetDeviceTimeDateAFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_SetDeviceTimeDateW(&self, Source: *mut c_void, szTimeDate: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetDeviceTimeDateWFunc)(Source, szTimeDate);  }
    }

    pub fn DTWAIN_SetDoubleFeedDetectLength(&self, Source: *mut c_void, Value: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetDoubleFeedDetectLengthFunc)(Source, Value);  }
    }

    pub fn DTWAIN_SetDoubleFeedDetectLengthString(&self, Source: *mut c_void, value: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetDoubleFeedDetectLengthStringFunc)(Source, value);  }
    }

    pub fn DTWAIN_SetDoubleFeedDetectLengthStringA(&self, Source: *mut c_void, szLength: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetDoubleFeedDetectLengthStringAFunc)(Source, szLength);  }
    }

    pub fn DTWAIN_SetDoubleFeedDetectLengthStringW(&self, Source: *mut c_void, szLength: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetDoubleFeedDetectLengthStringWFunc)(Source, szLength);  }
    }

    pub fn DTWAIN_SetDoubleFeedDetectValues(&self, Source: *mut c_void, prray: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetDoubleFeedDetectValuesFunc)(Source, prray);  }
    }

    pub fn DTWAIN_SetDoublePageCountOnDuplex(&self, Source: *mut c_void, bDoubleCount: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetDoublePageCountOnDuplexFunc)(Source, bDoubleCount);  }
    }

    pub fn DTWAIN_SetEOJDetectValue(&self, Source: *mut c_void, nValue: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetEOJDetectValueFunc)(Source, nValue);  }
    }

    pub fn DTWAIN_SetErrorBufferThreshold(&self, nErrors: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetErrorBufferThresholdFunc)(nErrors);  }
    }

    pub fn DTWAIN_SetErrorCallback(&self, proc: DTWAIN_ERROR_PROC, UserData: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetErrorCallbackFunc)(proc, UserData);  }
    }

    pub fn DTWAIN_SetErrorCallback64(&self, proc: DTWAIN_ERROR_PROC64, UserData64: i64) -> i32 {
        unsafe { return (self.DTWAIN_SetErrorCallback64Func)(proc, UserData64);  }
    }

    pub fn DTWAIN_SetFeederAlignment(&self, Source: *mut c_void, lpAlignment: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFeederAlignmentFunc)(Source, lpAlignment);  }
    }

    pub fn DTWAIN_SetFeederOrder(&self, Source: *mut c_void, lOrder: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFeederOrderFunc)(Source, lOrder);  }
    }

    pub fn DTWAIN_SetFeederWaitTime(&self, Source: *mut c_void, waitTime: i32, flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFeederWaitTimeFunc)(Source, waitTime, flags);  }
    }

    pub fn DTWAIN_SetFileAutoIncrement(&self, Source: *mut c_void, Increment: i32, bResetOnAcquire: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFileAutoIncrementFunc)(Source, Increment, bResetOnAcquire, bSet);  }
    }

    pub fn DTWAIN_SetFileCompressionType(&self, Source: *mut c_void, lCompression: i32, bIsCustom: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFileCompressionTypeFunc)(Source, lCompression, bIsCustom);  }
    }

    pub fn DTWAIN_SetFileSavePos(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, nFlags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFileSavePosFunc)(hWndParent, szTitle, xPos, yPos, nFlags);  }
    }

    pub fn DTWAIN_SetFileSavePosA(&self, hWndParent: *const c_void, szTitle: *const c_char, xPos: i32, yPos: i32, nFlags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFileSavePosAFunc)(hWndParent, szTitle, xPos, yPos, nFlags);  }
    }

    pub fn DTWAIN_SetFileSavePosW(&self, hWndParent: *const c_void, szTitle: *const u16, xPos: i32, yPos: i32, nFlags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFileSavePosWFunc)(hWndParent, szTitle, xPos, yPos, nFlags);  }
    }

    pub fn DTWAIN_SetFileXferFormat(&self, Source: *mut c_void, lFileType: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetFileXferFormatFunc)(Source, lFileType, bSetCurrent);  }
    }

    pub fn DTWAIN_SetHalftone(&self, Source: *mut c_void, lpHalftone: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetHalftoneFunc)(Source, lpHalftone);  }
    }

    pub fn DTWAIN_SetHalftoneA(&self, Source: *mut c_void, lpHalftone: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetHalftoneAFunc)(Source, lpHalftone);  }
    }

    pub fn DTWAIN_SetHalftoneW(&self, Source: *mut c_void, lpHalftone: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetHalftoneWFunc)(Source, lpHalftone);  }
    }

    pub fn DTWAIN_SetHighlight(&self, Source: *mut c_void, Highlight: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetHighlightFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_SetHighlightString(&self, Source: *mut c_void, Highlight: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetHighlightStringFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_SetHighlightStringA(&self, Source: *mut c_void, Highlight: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetHighlightStringAFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_SetHighlightStringW(&self, Source: *mut c_void, Highlight: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetHighlightStringWFunc)(Source, Highlight);  }
    }

    pub fn DTWAIN_SetJobControl(&self, Source: *mut c_void, JobControl: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetJobControlFunc)(Source, JobControl, bSetCurrent);  }
    }

    pub fn DTWAIN_SetJpegValues(&self, Source: *mut c_void, Quality: i32, Progressive: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetJpegValuesFunc)(Source, Quality, Progressive);  }
    }

    pub fn DTWAIN_SetJpegXRValues(&self, Source: *mut c_void, Quality: i32, Progressive: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetJpegXRValuesFunc)(Source, Quality, Progressive);  }
    }

    pub fn DTWAIN_SetLanguage(&self, nLanguage: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetLanguageFunc)(nLanguage);  }
    }

    pub fn DTWAIN_SetLastError(&self, nError: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetLastErrorFunc)(nError);  }
    }

    pub fn DTWAIN_SetLightPath(&self, Source: *mut c_void, LightPath: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetLightPathFunc)(Source, LightPath);  }
    }

    pub fn DTWAIN_SetLightPathEx(&self, Source: *mut c_void, LightPaths: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetLightPathExFunc)(Source, LightPaths);  }
    }

    pub fn DTWAIN_SetLightSource(&self, Source: *mut c_void, LightSource: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetLightSourceFunc)(Source, LightSource);  }
    }

    pub fn DTWAIN_SetLightSources(&self, Source: *mut c_void, LightSources: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetLightSourcesFunc)(Source, LightSources);  }
    }

    pub fn DTWAIN_SetLoggerCallback(&self, logProc: DTWAIN_LOGGER_PROC, UserData: i64) -> i32 {
        unsafe { return (self.DTWAIN_SetLoggerCallbackFunc)(logProc, UserData);  }
    }

    pub fn DTWAIN_SetLoggerCallbackA(&self, logProc: DTWAIN_LOGGER_PROCA, UserData: i64) -> i32 {
        unsafe { return (self.DTWAIN_SetLoggerCallbackAFunc)(logProc, UserData);  }
    }

    pub fn DTWAIN_SetLoggerCallbackW(&self, logProc: DTWAIN_LOGGER_PROCW, UserData: i64) -> i32 {
        unsafe { return (self.DTWAIN_SetLoggerCallbackWFunc)(logProc, UserData);  }
    }

    pub fn DTWAIN_SetManualDuplexMode(&self, Source: *mut c_void, Flags: i32, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetManualDuplexModeFunc)(Source, Flags, bSet);  }
    }

    pub fn DTWAIN_SetMaxAcquisitions(&self, Source: *mut c_void, MaxAcquires: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetMaxAcquisitionsFunc)(Source, MaxAcquires);  }
    }

    pub fn DTWAIN_SetMaxBuffers(&self, Source: *mut c_void, MaxBuf: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetMaxBuffersFunc)(Source, MaxBuf);  }
    }

    pub fn DTWAIN_SetMaxRetryAttempts(&self, Source: *mut c_void, nAttempts: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetMaxRetryAttemptsFunc)(Source, nAttempts);  }
    }

    pub fn DTWAIN_SetMultipageScanMode(&self, Source: *mut c_void, ScanType: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetMultipageScanModeFunc)(Source, ScanType);  }
    }

    pub fn DTWAIN_SetNoiseFilter(&self, Source: *mut c_void, NoiseFilter: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetNoiseFilterFunc)(Source, NoiseFilter);  }
    }

    pub fn DTWAIN_SetOCRCapValues(&self, Engine: *mut c_void, OCRCapValue: i32, SetType: i32, CapValues: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetOCRCapValuesFunc)(Engine, OCRCapValue, SetType, CapValues);  }
    }

    pub fn DTWAIN_SetOrientation(&self, Source: *mut c_void, Orient: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetOrientationFunc)(Source, Orient, bSetCurrent);  }
    }

    pub fn DTWAIN_SetOverscan(&self, Source: *mut c_void, Value: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetOverscanFunc)(Source, Value, bSetCurrent);  }
    }

    pub fn DTWAIN_SetPDFAESEncryption(&self, Source: *mut c_void, nWhichEncryption: i32, bUseAES: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFAESEncryptionFunc)(Source, nWhichEncryption, bUseAES);  }
    }

    pub fn DTWAIN_SetPDFASCIICompression(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFASCIICompressionFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_SetPDFAuthor(&self, Source: *mut c_void, lpAuthor: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFAuthorFunc)(Source, lpAuthor);  }
    }

    pub fn DTWAIN_SetPDFAuthorA(&self, Source: *mut c_void, lpAuthor: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFAuthorAFunc)(Source, lpAuthor);  }
    }

    pub fn DTWAIN_SetPDFAuthorW(&self, Source: *mut c_void, lpAuthor: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFAuthorWFunc)(Source, lpAuthor);  }
    }

    pub fn DTWAIN_SetPDFCompression(&self, Source: *mut c_void, bCompression: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFCompressionFunc)(Source, bCompression);  }
    }

    pub fn DTWAIN_SetPDFCreator(&self, Source: *mut c_void, lpCreator: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFCreatorFunc)(Source, lpCreator);  }
    }

    pub fn DTWAIN_SetPDFCreatorA(&self, Source: *mut c_void, lpCreator: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFCreatorAFunc)(Source, lpCreator);  }
    }

    pub fn DTWAIN_SetPDFCreatorW(&self, Source: *mut c_void, lpCreator: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFCreatorWFunc)(Source, lpCreator);  }
    }

    pub fn DTWAIN_SetPDFEncryption(&self, Source: *mut c_void, bUseEncryption: i32, lpszUser: *const u16, lpszOwner: *const u16, Permissions: u32, UseStrongEncryption: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFEncryptionFunc)(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption);  }
    }

    pub fn DTWAIN_SetPDFEncryptionA(&self, Source: *mut c_void, bUseEncryption: i32, lpszUser: *const c_char, lpszOwner: *const c_char, Permissions: u32, UseStrongEncryption: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFEncryptionAFunc)(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption);  }
    }

    pub fn DTWAIN_SetPDFEncryptionW(&self, Source: *mut c_void, bUseEncryption: i32, lpszUser: *const u16, lpszOwner: *const u16, Permissions: u32, UseStrongEncryption: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFEncryptionWFunc)(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption);  }
    }

    pub fn DTWAIN_SetPDFJpegQuality(&self, Source: *mut c_void, Quality: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFJpegQualityFunc)(Source, Quality);  }
    }

    pub fn DTWAIN_SetPDFKeywords(&self, Source: *mut c_void, lpKeyWords: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFKeywordsFunc)(Source, lpKeyWords);  }
    }

    pub fn DTWAIN_SetPDFKeywordsA(&self, Source: *mut c_void, lpKeyWords: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFKeywordsAFunc)(Source, lpKeyWords);  }
    }

    pub fn DTWAIN_SetPDFKeywordsW(&self, Source: *mut c_void, lpKeyWords: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFKeywordsWFunc)(Source, lpKeyWords);  }
    }

    pub fn DTWAIN_SetPDFOCRConversion(&self, Engine: *mut c_void, PageType: i32, FileType: i32, PixelType: i32, BitDepth: i32, Options: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFOCRConversionFunc)(Engine, PageType, FileType, PixelType, BitDepth, Options);  }
    }

    pub fn DTWAIN_SetPDFOCRMode(&self, Source: *mut c_void, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFOCRModeFunc)(Source, bSet);  }
    }

    pub fn DTWAIN_SetPDFOrientation(&self, Source: *mut c_void, lPOrientation: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFOrientationFunc)(Source, lPOrientation);  }
    }

    pub fn DTWAIN_SetPDFPageScale(&self, Source: *mut c_void, nOptions: i32, xScale: f64, yScale: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageScaleFunc)(Source, nOptions, xScale, yScale);  }
    }

    pub fn DTWAIN_SetPDFPageScaleString(&self, Source: *mut c_void, nOptions: i32, xScale: *const u16, yScale: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageScaleStringFunc)(Source, nOptions, xScale, yScale);  }
    }

    pub fn DTWAIN_SetPDFPageScaleStringA(&self, Source: *mut c_void, nOptions: i32, xScale: *const c_char, yScale: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageScaleStringAFunc)(Source, nOptions, xScale, yScale);  }
    }

    pub fn DTWAIN_SetPDFPageScaleStringW(&self, Source: *mut c_void, nOptions: i32, xScale: *const u16, yScale: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageScaleStringWFunc)(Source, nOptions, xScale, yScale);  }
    }

    pub fn DTWAIN_SetPDFPageSize(&self, Source: *mut c_void, PageSize: i32, CustomWidth: f64, CustomHeight: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageSizeFunc)(Source, PageSize, CustomWidth, CustomHeight);  }
    }

    pub fn DTWAIN_SetPDFPageSizeString(&self, Source: *mut c_void, PageSize: i32, CustomWidth: *const u16, CustomHeight: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageSizeStringFunc)(Source, PageSize, CustomWidth, CustomHeight);  }
    }

    pub fn DTWAIN_SetPDFPageSizeStringA(&self, Source: *mut c_void, PageSize: i32, CustomWidth: *const c_char, CustomHeight: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageSizeStringAFunc)(Source, PageSize, CustomWidth, CustomHeight);  }
    }

    pub fn DTWAIN_SetPDFPageSizeStringW(&self, Source: *mut c_void, PageSize: i32, CustomWidth: *const u16, CustomHeight: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPageSizeStringWFunc)(Source, PageSize, CustomWidth, CustomHeight);  }
    }

    pub fn DTWAIN_SetPDFPolarity(&self, Source: *mut c_void, Polarity: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFPolarityFunc)(Source, Polarity);  }
    }

    pub fn DTWAIN_SetPDFProducer(&self, Source: *mut c_void, lpProducer: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFProducerFunc)(Source, lpProducer);  }
    }

    pub fn DTWAIN_SetPDFProducerA(&self, Source: *mut c_void, lpProducer: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFProducerAFunc)(Source, lpProducer);  }
    }

    pub fn DTWAIN_SetPDFProducerW(&self, Source: *mut c_void, lpProducer: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFProducerWFunc)(Source, lpProducer);  }
    }

    pub fn DTWAIN_SetPDFSubject(&self, Source: *mut c_void, lpSubject: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFSubjectFunc)(Source, lpSubject);  }
    }

    pub fn DTWAIN_SetPDFSubjectA(&self, Source: *mut c_void, lpSubject: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFSubjectAFunc)(Source, lpSubject);  }
    }

    pub fn DTWAIN_SetPDFSubjectW(&self, Source: *mut c_void, lpSubject: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFSubjectWFunc)(Source, lpSubject);  }
    }

    pub fn DTWAIN_SetPDFTextElementFloat(&self, TextElement: *mut c_void, val1: f64, val2: f64, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTextElementFloatFunc)(TextElement, val1, val2, Flags);  }
    }

    pub fn DTWAIN_SetPDFTextElementLong(&self, TextElement: *mut c_void, val1: i32, val2: i32, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTextElementLongFunc)(TextElement, val1, val2, Flags);  }
    }

    pub fn DTWAIN_SetPDFTextElementString(&self, TextElement: *mut c_void, val1: *const u16, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTextElementStringFunc)(TextElement, val1, Flags);  }
    }

    pub fn DTWAIN_SetPDFTextElementStringA(&self, TextElement: *mut c_void, szString: *const c_char, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTextElementStringAFunc)(TextElement, szString, Flags);  }
    }

    pub fn DTWAIN_SetPDFTextElementStringW(&self, TextElement: *mut c_void, szString: *const u16, Flags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTextElementStringWFunc)(TextElement, szString, Flags);  }
    }

    pub fn DTWAIN_SetPDFTitle(&self, Source: *mut c_void, lpTitle: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTitleFunc)(Source, lpTitle);  }
    }

    pub fn DTWAIN_SetPDFTitleA(&self, Source: *mut c_void, lpTitle: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTitleAFunc)(Source, lpTitle);  }
    }

    pub fn DTWAIN_SetPDFTitleW(&self, Source: *mut c_void, lpTitle: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPDFTitleWFunc)(Source, lpTitle);  }
    }

    pub fn DTWAIN_SetPaperSize(&self, Source: *mut c_void, PaperSize: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPaperSizeFunc)(Source, PaperSize, bSetCurrent);  }
    }

    pub fn DTWAIN_SetPatchMaxPriorities(&self, Source: *mut c_void, nMaxSearchRetries: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPatchMaxPrioritiesFunc)(Source, nMaxSearchRetries);  }
    }

    pub fn DTWAIN_SetPatchMaxRetries(&self, Source: *mut c_void, nMaxRetries: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPatchMaxRetriesFunc)(Source, nMaxRetries);  }
    }

    pub fn DTWAIN_SetPatchPriorities(&self, Source: *mut c_void, SearchPriorities: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_SetPatchPrioritiesFunc)(Source, SearchPriorities);  }
    }

    pub fn DTWAIN_SetPatchSearchMode(&self, Source: *mut c_void, nSearchMode: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPatchSearchModeFunc)(Source, nSearchMode);  }
    }

    pub fn DTWAIN_SetPatchTimeOut(&self, Source: *mut c_void, TimeOutValue: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPatchTimeOutFunc)(Source, TimeOutValue);  }
    }

    pub fn DTWAIN_SetPixelFlavor(&self, Source: *mut c_void, PixelFlavor: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPixelFlavorFunc)(Source, PixelFlavor);  }
    }

    pub fn DTWAIN_SetPixelType(&self, Source: *mut c_void, PixelType: i32, BitDepth: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPixelTypeFunc)(Source, PixelType, BitDepth, bSetCurrent);  }
    }

    pub fn DTWAIN_SetPostScriptTitle(&self, Source: *mut c_void, szTitle: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPostScriptTitleFunc)(Source, szTitle);  }
    }

    pub fn DTWAIN_SetPostScriptTitleA(&self, Source: *mut c_void, szTitle: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPostScriptTitleAFunc)(Source, szTitle);  }
    }

    pub fn DTWAIN_SetPostScriptTitleW(&self, Source: *mut c_void, szTitle: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPostScriptTitleWFunc)(Source, szTitle);  }
    }

    pub fn DTWAIN_SetPostScriptType(&self, Source: *mut c_void, PSType: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPostScriptTypeFunc)(Source, PSType);  }
    }

    pub fn DTWAIN_SetPrinter(&self, Source: *mut c_void, Printer: i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterFunc)(Source, Printer, bCurrent);  }
    }

    pub fn DTWAIN_SetPrinterEx(&self, Source: *mut c_void, Printer: i32, bCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterExFunc)(Source, Printer, bCurrent);  }
    }

    pub fn DTWAIN_SetPrinterStartNumber(&self, Source: *mut c_void, nStart: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterStartNumberFunc)(Source, nStart);  }
    }

    pub fn DTWAIN_SetPrinterStringMode(&self, Source: *mut c_void, PrinterMode: i32, bSetCurrent: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterStringModeFunc)(Source, PrinterMode, bSetCurrent);  }
    }

    pub fn DTWAIN_SetPrinterStrings(&self, Source: *mut c_void, ArrayString: *mut c_void, pNumStrings: *mut i32) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterStringsFunc)(Source, ArrayString, pNumStrings);  }
    }

    pub fn DTWAIN_SetPrinterSuffixString(&self, Source: *mut c_void, Suffix: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterSuffixStringFunc)(Source, Suffix);  }
    }

    pub fn DTWAIN_SetPrinterSuffixStringA(&self, Source: *mut c_void, Suffix: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterSuffixStringAFunc)(Source, Suffix);  }
    }

    pub fn DTWAIN_SetPrinterSuffixStringW(&self, Source: *mut c_void, Suffix: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetPrinterSuffixStringWFunc)(Source, Suffix);  }
    }

    pub fn DTWAIN_SetQueryCapSupport(&self, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetQueryCapSupportFunc)(bSet);  }
    }

    pub fn DTWAIN_SetResolution(&self, Source: *mut c_void, Resolution: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetResolutionFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetResolutionString(&self, Source: *mut c_void, Resolution: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetResolutionStringFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetResolutionStringA(&self, Source: *mut c_void, Resolution: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetResolutionStringAFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetResolutionStringW(&self, Source: *mut c_void, Resolution: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetResolutionStringWFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetResourcePath(&self, ResourcePath: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetResourcePathFunc)(ResourcePath);  }
    }

    pub fn DTWAIN_SetResourcePathA(&self, ResourcePath: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetResourcePathAFunc)(ResourcePath);  }
    }

    pub fn DTWAIN_SetResourcePathW(&self, ResourcePath: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetResourcePathWFunc)(ResourcePath);  }
    }

    pub fn DTWAIN_SetRotation(&self, Source: *mut c_void, Rotation: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetRotationFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_SetRotationString(&self, Source: *mut c_void, Rotation: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetRotationStringFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_SetRotationStringA(&self, Source: *mut c_void, Rotation: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetRotationStringAFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_SetRotationStringW(&self, Source: *mut c_void, Rotation: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetRotationStringWFunc)(Source, Rotation);  }
    }

    pub fn DTWAIN_SetSaveFileName(&self, Source: *mut c_void, fName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetSaveFileNameFunc)(Source, fName);  }
    }

    pub fn DTWAIN_SetSaveFileNameA(&self, Source: *mut c_void, fName: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetSaveFileNameAFunc)(Source, fName);  }
    }

    pub fn DTWAIN_SetSaveFileNameW(&self, Source: *mut c_void, fName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetSaveFileNameWFunc)(Source, fName);  }
    }

    pub fn DTWAIN_SetShadow(&self, Source: *mut c_void, Shadow: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetShadowFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_SetShadowString(&self, Source: *mut c_void, Shadow: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetShadowStringFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_SetShadowStringA(&self, Source: *mut c_void, Shadow: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetShadowStringAFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_SetShadowStringW(&self, Source: *mut c_void, Shadow: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetShadowStringWFunc)(Source, Shadow);  }
    }

    pub fn DTWAIN_SetSourceUnit(&self, Source: *mut c_void, Unit: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetSourceUnitFunc)(Source, Unit);  }
    }

    pub fn DTWAIN_SetTIFFCompressType(&self, Source: *mut c_void, Setting: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTIFFCompressTypeFunc)(Source, Setting);  }
    }

    pub fn DTWAIN_SetTIFFInvert(&self, Source: *mut c_void, Setting: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTIFFInvertFunc)(Source, Setting);  }
    }

    pub fn DTWAIN_SetTempFileDirectory(&self, szFilePath: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetTempFileDirectoryFunc)(szFilePath);  }
    }

    pub fn DTWAIN_SetTempFileDirectoryA(&self, szFilePath: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetTempFileDirectoryAFunc)(szFilePath);  }
    }

    pub fn DTWAIN_SetTempFileDirectoryEx(&self, szFilePath: *const u16, CreationFlags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTempFileDirectoryExFunc)(szFilePath, CreationFlags);  }
    }

    pub fn DTWAIN_SetTempFileDirectoryExA(&self, szFilePath: *const c_char, CreationFlags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTempFileDirectoryExAFunc)(szFilePath, CreationFlags);  }
    }

    pub fn DTWAIN_SetTempFileDirectoryExW(&self, szFilePath: *const u16, CreationFlags: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTempFileDirectoryExWFunc)(szFilePath, CreationFlags);  }
    }

    pub fn DTWAIN_SetTempFileDirectoryW(&self, szFilePath: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetTempFileDirectoryWFunc)(szFilePath);  }
    }

    pub fn DTWAIN_SetThreshold(&self, Source: *mut c_void, Threshold: f64, bSetBithDepthReduction: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetThresholdFunc)(Source, Threshold, bSetBithDepthReduction);  }
    }

    pub fn DTWAIN_SetThresholdString(&self, Source: *mut c_void, Threshold: *const u16, bSetBitDepthReduction: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetThresholdStringFunc)(Source, Threshold, bSetBitDepthReduction);  }
    }

    pub fn DTWAIN_SetThresholdStringA(&self, Source: *mut c_void, Threshold: *const c_char, bSetBitDepthReduction: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetThresholdStringAFunc)(Source, Threshold, bSetBitDepthReduction);  }
    }

    pub fn DTWAIN_SetThresholdStringW(&self, Source: *mut c_void, Threshold: *const u16, bSetBitDepthReduction: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetThresholdStringWFunc)(Source, Threshold, bSetBitDepthReduction);  }
    }

    pub fn DTWAIN_SetTwainDSM(&self, DSMType: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTwainDSMFunc)(DSMType);  }
    }

    pub fn DTWAIN_SetTwainLog(&self, LogFlags: u32, lpszLogFile: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetTwainLogFunc)(LogFlags, lpszLogFile);  }
    }

    pub fn DTWAIN_SetTwainLogA(&self, LogFlags: u32, lpszLogFile: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetTwainLogAFunc)(LogFlags, lpszLogFile);  }
    }

    pub fn DTWAIN_SetTwainLogW(&self, LogFlags: u32, lpszLogFile: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetTwainLogWFunc)(LogFlags, lpszLogFile);  }
    }

    pub fn DTWAIN_SetTwainMode(&self, lAcquireMode: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTwainModeFunc)(lAcquireMode);  }
    }

    pub fn DTWAIN_SetTwainTimeout(&self, milliseconds: i32) -> i32 {
        unsafe { return (self.DTWAIN_SetTwainTimeoutFunc)(milliseconds);  }
    }

    pub fn DTWAIN_SetUpdateDibProc(&self, DibProc: DTWAIN_DIBUPDATE_PROC) -> DTWAIN_DIBUPDATE_PROC {
        unsafe { return (self.DTWAIN_SetUpdateDibProcFunc)(DibProc);  }
    }

    pub fn DTWAIN_SetXResolution(&self, Source: *mut c_void, xResolution: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetXResolutionFunc)(Source, xResolution);  }
    }

    pub fn DTWAIN_SetXResolutionString(&self, Source: *mut c_void, Resolution: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetXResolutionStringFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetXResolutionStringA(&self, Source: *mut c_void, Resolution: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetXResolutionStringAFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetXResolutionStringW(&self, Source: *mut c_void, Resolution: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetXResolutionStringWFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetYResolution(&self, Source: *mut c_void, yResolution: f64) -> i32 {
        unsafe { return (self.DTWAIN_SetYResolutionFunc)(Source, yResolution);  }
    }

    pub fn DTWAIN_SetYResolutionString(&self, Source: *mut c_void, Resolution: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetYResolutionStringFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetYResolutionStringA(&self, Source: *mut c_void, Resolution: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_SetYResolutionStringAFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_SetYResolutionStringW(&self, Source: *mut c_void, Resolution: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_SetYResolutionStringWFunc)(Source, Resolution);  }
    }

    pub fn DTWAIN_ShowUIOnly(&self, Source: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ShowUIOnlyFunc)(Source);  }
    }

    pub fn DTWAIN_ShutdownOCREngine(&self, OCREngine: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_ShutdownOCREngineFunc)(OCREngine);  }
    }

    pub fn DTWAIN_SkipImageInfoError(&self, Source: *mut c_void, bSkip: i32) -> i32 {
        unsafe { return (self.DTWAIN_SkipImageInfoErrorFunc)(Source, bSkip);  }
    }

    pub fn DTWAIN_StartThread(&self, DLLHandle: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_StartThreadFunc)(DLLHandle);  }
    }

    pub fn DTWAIN_StartTwainSession(&self, hWndMsg: *const c_void, lpszDLLName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_StartTwainSessionFunc)(hWndMsg, lpszDLLName);  }
    }

    pub fn DTWAIN_StartTwainSessionA(&self, hWndMsg: *const c_void, lpszDLLName: *const c_char) -> i32 {
        unsafe { return (self.DTWAIN_StartTwainSessionAFunc)(hWndMsg, lpszDLLName);  }
    }

    pub fn DTWAIN_StartTwainSessionW(&self, hWndMsg: *const c_void, lpszDLLName: *const u16) -> i32 {
        unsafe { return (self.DTWAIN_StartTwainSessionWFunc)(hWndMsg, lpszDLLName);  }
    }

    pub fn DTWAIN_SysDestroy(&self) -> i32 {
        unsafe { return (self.DTWAIN_SysDestroyFunc)();  }
    }

    pub fn DTWAIN_SysInitialize(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeFunc)();  }
    }

    pub fn DTWAIN_SysInitializeEx(&self, szINIPath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeExFunc)(szINIPath);  }
    }

    pub fn DTWAIN_SysInitializeEx2(&self, szINIPath: *const u16, szImageDLLPath: *const u16, szLangResourcePath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeEx2Func)(szINIPath, szImageDLLPath, szLangResourcePath);  }
    }

    pub fn DTWAIN_SysInitializeEx2A(&self, szINIPath: *const c_char, szImageDLLPath: *const c_char, szLangResourcePath: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeEx2AFunc)(szINIPath, szImageDLLPath, szLangResourcePath);  }
    }

    pub fn DTWAIN_SysInitializeEx2W(&self, szINIPath: *const u16, szImageDLLPath: *const u16, szLangResourcePath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeEx2WFunc)(szINIPath, szImageDLLPath, szLangResourcePath);  }
    }

    pub fn DTWAIN_SysInitializeExA(&self, szINIPath: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeExAFunc)(szINIPath);  }
    }

    pub fn DTWAIN_SysInitializeExW(&self, szINIPath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeExWFunc)(szINIPath);  }
    }

    pub fn DTWAIN_SysInitializeLib(&self, hInstance: *mut c_void) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeLibFunc)(hInstance);  }
    }

    pub fn DTWAIN_SysInitializeLibEx(&self, hInstance: *mut c_void, szINIPath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeLibExFunc)(hInstance, szINIPath);  }
    }

    pub fn DTWAIN_SysInitializeLibEx2(&self, hInstance: *mut c_void, szINIPath: *const u16, szImageDLLPath: *const u16, szLangResourcePath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeLibEx2Func)(hInstance, szINIPath, szImageDLLPath, szLangResourcePath);  }
    }

    pub fn DTWAIN_SysInitializeLibEx2A(&self, hInstance: *mut c_void, szINIPath: *const c_char, szImageDLLPath: *const c_char, szLangResourcePath: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeLibEx2AFunc)(hInstance, szINIPath, szImageDLLPath, szLangResourcePath);  }
    }

    pub fn DTWAIN_SysInitializeLibEx2W(&self, hInstance: *mut c_void, szINIPath: *const u16, szImageDLLPath: *const u16, szLangResourcePath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeLibEx2WFunc)(hInstance, szINIPath, szImageDLLPath, szLangResourcePath);  }
    }

    pub fn DTWAIN_SysInitializeLibExA(&self, hInstance: *mut c_void, szINIPath: *const c_char) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeLibExAFunc)(hInstance, szINIPath);  }
    }

    pub fn DTWAIN_SysInitializeLibExW(&self, hInstance: *mut c_void, szINIPath: *const u16) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeLibExWFunc)(hInstance, szINIPath);  }
    }

    pub fn DTWAIN_SysInitializeNoBlocking(&self) -> *mut c_void {
        unsafe { return (self.DTWAIN_SysInitializeNoBlockingFunc)();  }
    }

    pub fn DTWAIN_TestGetCap(&self, Source: *mut c_void, lCapability: i32) -> *mut c_void {
        unsafe { return (self.DTWAIN_TestGetCapFunc)(Source, lCapability);  }
    }

    pub fn DTWAIN_UnlockMemory(&self, h: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_UnlockMemoryFunc)(h);  }
    }

    pub fn DTWAIN_UnlockMemoryEx(&self, h: *mut c_void) -> i32 {
        unsafe { return (self.DTWAIN_UnlockMemoryExFunc)(h);  }
    }

    pub fn DTWAIN_UseMultipleThreads(&self, bSet: i32) -> i32 {
        unsafe { return (self.DTWAIN_UseMultipleThreadsFunc)(bSet);  }
    }
    pub unsafe fn allocate_ansi_buffer(len: usize) -> *mut c_char {
        // Allocate a Vec<c_char> with the desired length
        let mut buf: Vec<c_char> = Vec::with_capacity(len);
        unsafe {buf.set_len(len);} // makes the length match capacity (uninitialized bytes!)

        // Convert to raw pointer; caller must free.
        let ptr = buf.as_mut_ptr();

        // Prevent Vec from dropping and deallocating the memory.
        std::mem::forget(buf);

        ptr
    }

    pub fn allocate_wide_buffer(len: usize) -> Vec<u16> {
        let mut buf: Vec<u16> = vec![b' ' as u16; len];
        buf.push(0);
        buf
    }
}

