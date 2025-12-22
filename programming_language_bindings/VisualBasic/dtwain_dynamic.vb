REM
REM This file is part of the Dynarithmic TWAIN Library (DTWAIN).                          
REM Copyright (c) 2002-2026 Dynarithmic Software.                                         
REM                                                                                       
REM Licensed under the Apache License, Version 2.0 (the "License");                       
REM you may not use this file except in compliance with the License.                      
REM You may obtain a copy of the License at                                               
REM                                                                                       
REM     http://www.apache.org/licenses/LICENSE-2.0                                        
REM                                                                                       
REM Unless required by applicable law or agreed to in writing, software                   
REM distributed under the License is distributed on an "AS IS" BASIS,                     
REM WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.              
REM See the License for the specific language governing permissions and                   
REM limitations under the License.                                                        
REM                                                                                       
REM FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY                   
REM DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT 
REM OF THIRD PARTY RIGHTS.                                                                
REM

Option Explicit Off
#Disable Warning BC42020

Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO
Imports System.ComponentModel


Namespace Dynarithmic
    Friend Module NativeMethods
        <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Function LoadLibrary(path As String) As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Function FreeLibrary(hModule As IntPtr) As Boolean
        End Function

        <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
        Public Function GetProcAddress(
            hModule As IntPtr,
            name As String
        ) As IntPtr
        End Function
    End Module

    Public Class AutoDll
        Implements IDisposable

        Private _module As IntPtr
        Private _sysErrorMessage As String = ""

        Public Sub New(dllPath As String)
            Dim dll32() As String = {"dtwain32u.dll", "dtwain32ud.dll"}
            Dim dll64() As String = {"dtwain64u.dll", "dtwain64ud.dll"}
            Dim filename As String = Path.GetFileName(dllPath).ToLower()

            Dim index1 As Integer = Array.IndexOf(dll32, filename)
            Dim index2 As Integer = Array.IndexOf(dll64, filename)

            Dim is32Bit As Boolean = False
            If IntPtr.Size = 4 Then
                is32Bit = True
            End If

            Dim is64Bit As Boolean = False
            If IntPtr.Size = 8 Then
                is64Bit = True
            End If

            Dim joinedNames As String
            If is32Bit Then
                joinedNames = String.Join(", ", dll32)
            Else
                joinedNames = String.Join(", ", dll64)
            End If

            If index1 = -1 And index2 = -1 Then
                _sysErrorMessage = "DTWAIN DLL file name " & filename &
                  " is not valid (must be one of the following: [" & joinedNames & "])"
            End If
            If String.IsNullOrEmpty(_sysErrorMessage) And is64Bit And index1 <> -1 Then
                _sysErrorMessage = "Cannot load 32-bit DTWAIN DLL in a 64-bit process"
            End If
            If String.IsNullOrEmpty(_sysErrorMessage) And is32Bit And index2 <> -1 Then
                _sysErrorMessage = "Cannot load 64-bit DTWAIN DLL in a 32-bit process"
            End If
            If String.IsNullOrEmpty(_sysErrorMessage) Then
                _module = NativeMethods.LoadLibrary(dllPath)
                If _module = IntPtr.Zero Then
                    Dim lastError = Marshal.GetLastWin32Error()
                    Dim message As String = New Win32Exception(CInt(lastError)).Message
                    _sysErrorMessage = "Failed to load " & dllPath & ", error=" & Marshal.GetLastWin32Error()
                End If
            End If
        End Sub

        Public Function GetErrorMessage() As String
            Return _sysErrorMessage
        End Function
        Public Sub Bind(target As Object)
            Dim t = target.GetType()

            ' ---- Fields ----
            For Each field In t.GetFields(BindingFlags.Public Or BindingFlags.Instance)
                BindMember(
                field.FieldType,
                DirectCast(
                    Attribute.GetCustomAttribute(field, GetType(DllImportNameAttribute)),
                    DllImportNameAttribute
                ),
                field.Name,
                Sub(d) field.SetValue(target, d)
            )
            Next

            ' ---- Properties ----
            For Each prop In t.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                If Not prop.CanWrite Then Continue For

                BindMember(
                prop.PropertyType,
                DirectCast(
                    Attribute.GetCustomAttribute(prop, GetType(DllImportNameAttribute)),
                    DllImportNameAttribute
                ),
                prop.Name,
                Sub(d) prop.SetValue(target, d)
            )
            Next
        End Sub
        <AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)>
        Public Class DllImportNameAttribute
            Inherits Attribute

            Public ReadOnly Name As String

            Public Sub New(name As String)
                Me.Name = name
            End Sub
        End Class

        Private Sub BindMember(
        memberType As Type,
        attr As DllImportNameAttribute,
        defaultName As String,
        assign As Action(Of [Delegate]))
            If Not GetType([Delegate]).IsAssignableFrom(memberType) Then Return

            Dim symbol = If(attr?.Name, defaultName)
            Dim proc = NativeMethods.GetProcAddress(_module, symbol)

            If proc = IntPtr.Zero Then
                Throw New EntryPointNotFoundException(
                $"Symbol '{symbol}' not found"
            )
            End If

            Dim del = Marshal.GetDelegateForFunctionPointer(proc, memberType)
            assign(DirectCast(del, [Delegate]))
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            If _module <> IntPtr.Zero Then
                NativeMethods.FreeLibrary(_module)
                _module = IntPtr.Zero
            End If
            GC.SuppressFinalize(Me)
        End Sub

    End Class

    Public Class DTWAINAPI
        Inherits AutoDll

        Private api As AllBindings = New AllBindings

        Public Sub New(dllPath As String)
            MyBase.New(dllPath)
            Dim errMsg As String = MyBase.GetErrorMessage()
            Dim isEmpty As Boolean = String.IsNullOrEmpty(errMsg)
            If Not isEmpty Then
                If errMsg.StartsWith("Failed") Then
                    Throw New DllNotFoundException(errMsg)
                End If
                Throw New ArgumentOutOfRangeException(NameOf(dllPath), errMsg)
            End If
            Try
                MyBase.Bind(api)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Structure DTWAIN_POINTAPI
            Dim x As Integer
            Dim y As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=8)>
        Structure WinMsg
            Dim hwnd As Integer
            Dim message As Integer
            Dim wParam As Integer
            Dim lParam As Integer
            Dim time As Integer
            Dim pt As DTWAIN_POINTAPI
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=2)>
        Structure TW_VERSION
            Dim MajorNum As System.UInt16
            Dim MinorNum As System.UInt16
            Dim Language As System.UInt16
            Dim Country As System.UInt16

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
            Dim Info As String
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=2)>
        Structure TW_IDENTITY
            Dim Id As System.UInt32
            Dim Version As TW_VERSION
            Dim ProtocolMajor As System.UInt16
            Dim ProtocolMinor As System.UInt16
            Dim SupportedGroups As System.UInt32

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
            Dim Manufacturer As String

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
            Dim ProductFamily As String

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=34)>
            Dim ProductName As String
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
        Structure OPENFILENAME
            Dim lStructSize As Long
            Dim hwndOwner As Long
            Dim hInstance As Long
            Dim lpstrFilter As String
            Dim lpstrCustomFilter As String
            Dim nMaxCustFilter As Long
            Dim nFilterIndex As Long
            Dim lpstrFile As String
            Dim nMaxFile As Long
            Dim lpstrFileTitle As String
            Dim nMaxFileTitle As Long
            Dim lpstrInitialDir As String
            Dim lpstrTitle As String
            Dim flags As Long
            Dim nFileOffset As Integer
            Dim nFileExtension As Integer
            Dim lpstrDefExt As String
            Dim lCustData As Long
            Dim lpfnHook As Long
            Dim lpTemplateName As String
        End Structure

        Public Const DTWAIN_FF_TIFF As Integer = 0
        Public Const DTWAIN_FF_PICT As Integer = 1
        Public Const DTWAIN_FF_BMP As Integer = 2
        Public Const DTWAIN_FF_XBM As Integer = 3
        Public Const DTWAIN_FF_JFIF As Integer = 4
        Public Const DTWAIN_FF_FPX As Integer = 5
        Public Const DTWAIN_FF_TIFFMULTI As Integer = 6
        Public Const DTWAIN_FF_PNG As Integer = 7
        Public Const DTWAIN_FF_SPIFF As Integer = 8
        Public Const DTWAIN_FF_EXIF As Integer = 9
        Public Const DTWAIN_FF_PDF As Integer = 10
        Public Const DTWAIN_FF_JP2 As Integer = 11
        Public Const DTWAIN_FF_JPX As Integer = 13
        Public Const DTWAIN_FF_DEJAVU As Integer = 14
        Public Const DTWAIN_FF_PDFA As Integer = 15
        Public Const DTWAIN_FF_PDFA2 As Integer = 16
        Public Const DTWAIN_FF_PDFRASTER As Integer = 17
        Public Const DTWAIN_CP_NONE As Integer = 0
        Public Const DTWAIN_CP_PACKBITS As Integer = 1
        Public Const DTWAIN_CP_GROUP31D As Integer = 2
        Public Const DTWAIN_CP_GROUP31DEOL As Integer = 3
        Public Const DTWAIN_CP_GROUP32D As Integer = 4
        Public Const DTWAIN_CP_GROUP4 As Integer = 5
        Public Const DTWAIN_CP_JPEG As Integer = 6
        Public Const DTWAIN_CP_LZW As Integer = 7
        Public Const DTWAIN_CP_JBIG As Integer = 8
        Public Const DTWAIN_CP_PNG As Integer = 9
        Public Const DTWAIN_CP_RLE4 As Integer = 10
        Public Const DTWAIN_CP_RLE8 As Integer = 11
        Public Const DTWAIN_CP_BITFIELDS As Integer = 12
        Public Const DTWAIN_CP_ZIP As Integer = 13
        Public Const DTWAIN_CP_JPEG2000 As Integer = 14
        Public Const DTWAIN_FS_NONE As Integer = 0
        Public Const DTWAIN_FS_A4LETTER As Integer = 1
        Public Const DTWAIN_FS_B5LETTER As Integer = 2
        Public Const DTWAIN_FS_USLETTER As Integer = 3
        Public Const DTWAIN_FS_USLEGAL As Integer = 4
        Public Const DTWAIN_FS_A5 As Integer = 5
        Public Const DTWAIN_FS_B4 As Integer = 6
        Public Const DTWAIN_FS_B6 As Integer = 7
        Public Const DTWAIN_FS_USLEDGER As Integer = 9
        Public Const DTWAIN_FS_USEXECUTIVE As Integer = 10
        Public Const DTWAIN_FS_A3 As Integer = 11
        Public Const DTWAIN_FS_B3 As Integer = 12
        Public Const DTWAIN_FS_A6 As Integer = 13
        Public Const DTWAIN_FS_C4 As Integer = 14
        Public Const DTWAIN_FS_C5 As Integer = 15
        Public Const DTWAIN_FS_C6 As Integer = 16
        Public Const DTWAIN_FS_4A0 As Integer = 17
        Public Const DTWAIN_FS_2A0 As Integer = 18
        Public Const DTWAIN_FS_A0 As Integer = 19
        Public Const DTWAIN_FS_A1 As Integer = 20
        Public Const DTWAIN_FS_A2 As Integer = 21
        Public Const DTWAIN_FS_A4 As Integer = DTWAIN_FS_A4LETTER
        Public Const DTWAIN_FS_A7 As Integer = 22
        Public Const DTWAIN_FS_A8 As Integer = 23
        Public Const DTWAIN_FS_A9 As Integer = 24
        Public Const DTWAIN_FS_A10 As Integer = 25
        Public Const DTWAIN_FS_ISOB0 As Integer = 26
        Public Const DTWAIN_FS_ISOB1 As Integer = 27
        Public Const DTWAIN_FS_ISOB2 As Integer = 28
        Public Const DTWAIN_FS_ISOB3 As Integer = DTWAIN_FS_B3
        Public Const DTWAIN_FS_ISOB4 As Integer = DTWAIN_FS_B4
        Public Const DTWAIN_FS_ISOB5 As Integer = 29
        Public Const DTWAIN_FS_ISOB6 As Integer = DTWAIN_FS_B6
        Public Const DTWAIN_FS_ISOB7 As Integer = 30
        Public Const DTWAIN_FS_ISOB8 As Integer = 31
        Public Const DTWAIN_FS_ISOB9 As Integer = 32
        Public Const DTWAIN_FS_ISOB10 As Integer = 33
        Public Const DTWAIN_FS_JISB0 As Integer = 34
        Public Const DTWAIN_FS_JISB1 As Integer = 35
        Public Const DTWAIN_FS_JISB2 As Integer = 36
        Public Const DTWAIN_FS_JISB3 As Integer = 37
        Public Const DTWAIN_FS_JISB4 As Integer = 38
        Public Const DTWAIN_FS_JISB5 As Integer = DTWAIN_FS_B5LETTER
        Public Const DTWAIN_FS_JISB6 As Integer = 39
        Public Const DTWAIN_FS_JISB7 As Integer = 40
        Public Const DTWAIN_FS_JISB8 As Integer = 41
        Public Const DTWAIN_FS_JISB9 As Integer = 42
        Public Const DTWAIN_FS_JISB10 As Integer = 43
        Public Const DTWAIN_FS_C0 As Integer = 44
        Public Const DTWAIN_FS_C1 As Integer = 45
        Public Const DTWAIN_FS_C2 As Integer = 46
        Public Const DTWAIN_FS_C3 As Integer = 47
        Public Const DTWAIN_FS_C7 As Integer = 48
        Public Const DTWAIN_FS_C8 As Integer = 49
        Public Const DTWAIN_FS_C9 As Integer = 50
        Public Const DTWAIN_FS_C10 As Integer = 51
        Public Const DTWAIN_FS_USSTATEMENT As Integer = 52
        Public Const DTWAIN_FS_BUSINESSCARD As Integer = 53
        Public Const DTWAIN_ANYSUPPORT As Integer = (-1)
        Public Const DTWAIN_BMP As Integer = 100
        Public Const DTWAIN_JPEG As Integer = 200
        Public Const DTWAIN_PDF As Integer = 250
        Public Const DTWAIN_PDFMULTI As Integer = 251
        Public Const DTWAIN_PCX As Integer = 300
        Public Const DTWAIN_DCX As Integer = 301
        Public Const DTWAIN_TGA As Integer = 400
        Public Const DTWAIN_TIFFLZW As Integer = 500
        Public Const DTWAIN_TIFFNONE As Integer = 600
        Public Const DTWAIN_TIFFG3 As Integer = 700
        Public Const DTWAIN_TIFFG4 As Integer = 800
        Public Const DTWAIN_TIFFPACKBITS As Integer = 801
        Public Const DTWAIN_TIFFDEFLATE As Integer = 802
        Public Const DTWAIN_TIFFJPEG As Integer = 803
        Public Const DTWAIN_TIFFJBIG As Integer = 804
        Public Const DTWAIN_TIFFPIXARLOG As Integer = 805
        Public Const DTWAIN_TIFFNONEMULTI As Integer = 900
        Public Const DTWAIN_TIFFG3MULTI As Integer = 901
        Public Const DTWAIN_TIFFG4MULTI As Integer = 902
        Public Const DTWAIN_TIFFPACKBITSMULTI As Integer = 903
        Public Const DTWAIN_TIFFDEFLATEMULTI As Integer = 904
        Public Const DTWAIN_TIFFJPEGMULTI As Integer = 905
        Public Const DTWAIN_TIFFLZWMULTI As Integer = 906
        Public Const DTWAIN_TIFFJBIGMULTI As Integer = 907
        Public Const DTWAIN_TIFFPIXARLOGMULTI As Integer = 908
        Public Const DTWAIN_WMF As Integer = 850
        Public Const DTWAIN_EMF As Integer = 851
        Public Const DTWAIN_GIF As Integer = 950
        Public Const DTWAIN_PNG As Integer = 1000
        Public Const DTWAIN_PSD As Integer = 2000
        Public Const DTWAIN_JPEG2000 As Integer = 3000
        Public Const DTWAIN_POSTSCRIPT1 As Integer = 4000
        Public Const DTWAIN_POSTSCRIPT2 As Integer = 4001
        Public Const DTWAIN_POSTSCRIPT3 As Integer = 4002
        Public Const DTWAIN_POSTSCRIPT1MULTI As Integer = 4003
        Public Const DTWAIN_POSTSCRIPT2MULTI As Integer = 4004
        Public Const DTWAIN_POSTSCRIPT3MULTI As Integer = 4005
        Public Const DTWAIN_TEXT As Integer = 6000
        Public Const DTWAIN_TEXTMULTI As Integer = 6001
        Public Const DTWAIN_TIFFMULTI As Integer = 7000
        Public Const DTWAIN_ICO As Integer = 8000
        Public Const DTWAIN_ICO_VISTA As Integer = 8001
        Public Const DTWAIN_ICO_RESIZED As Integer = 8002
        Public Const DTWAIN_WBMP As Integer = 8500
        Public Const DTWAIN_WEBP As Integer = 8501
        Public Const DTWAIN_PPM As Integer = 10000
        Public Const DTWAIN_WBMP_RESIZED As Integer = 11000
        Public Const DTWAIN_TGA_RLE As Integer = 11001
        Public Const DTWAIN_BMP_RLE As Integer = 11002
        Public Const DTWAIN_BIGTIFFLZW As Integer = 11003
        Public Const DTWAIN_BIGTIFFLZWMULTI As Integer = 11004
        Public Const DTWAIN_BIGTIFFNONE As Integer = 11005
        Public Const DTWAIN_BIGTIFFNONEMULTI As Integer = 11006
        Public Const DTWAIN_BIGTIFFPACKBITS As Integer = 11007
        Public Const DTWAIN_BIGTIFFPACKBITSMULTI As Integer = 11008
        Public Const DTWAIN_BIGTIFFDEFLATE As Integer = 11009
        Public Const DTWAIN_BIGTIFFDEFLATEMULTI As Integer = 11010
        Public Const DTWAIN_BIGTIFFG3 As Integer = 11011
        Public Const DTWAIN_BIGTIFFG3MULTI As Integer = 11012
        Public Const DTWAIN_BIGTIFFG4 As Integer = 11013
        Public Const DTWAIN_BIGTIFFG4MULTI As Integer = 11014
        Public Const DTWAIN_BIGTIFFJPEG As Integer = 11015
        Public Const DTWAIN_BIGTIFFJPEGMULTI As Integer = 11016
        Public Const DTWAIN_JPEGXR As Integer = 12000
        Public Const DTWAIN_INCHES As Integer = 0
        Public Const DTWAIN_CENTIMETERS As Integer = 1
        Public Const DTWAIN_PICAS As Integer = 2
        Public Const DTWAIN_POINTS As Integer = 3
        Public Const DTWAIN_TWIPS As Integer = 4
        Public Const DTWAIN_PIXELS As Integer = 5
        Public Const DTWAIN_MILLIMETERS As Integer = 6
        Public Const DTWAIN_USENAME As Integer = 16
        Public Const DTWAIN_USEPROMPT As Integer = 32
        Public Const DTWAIN_USELONGNAME As Integer = 64
        Public Const DTWAIN_USESOURCEMODE As Integer = 128
        Public Const DTWAIN_USELIST As Integer = 256
        Public Const DTWAIN_CREATE_DIRECTORY As Integer = 512
        Public Const DTWAIN_CREATEDIRECTORY As Integer = DTWAIN_CREATE_DIRECTORY
        Public Const DTWAIN_ARRAYANY As Integer = 1
        Public Const DTWAIN_ArrayTypePTR As Integer = 1
        Public Const DTWAIN_ARRAYLONG As Integer = 2
        Public Const DTWAIN_ARRAYFLOAT As Integer = 3
        Public Const DTWAIN_ARRAYHANDLE As Integer = 4
        Public Const DTWAIN_ARRAYSOURCE As Integer = 5
        Public Const DTWAIN_ARRAYSTRING As Integer = 6
        Public Const DTWAIN_ARRAYFRAME As Integer = 7
        Public Const DTWAIN_ARRAYBOOL As Integer = DTWAIN_ARRAYLONG
        Public Const DTWAIN_ARRAYLONGSTRING As Integer = 8
        Public Const DTWAIN_ARRAYUNICODESTRING As Integer = 9
        Public Const DTWAIN_ARRAYLONG64 As Integer = 10
        Public Const DTWAIN_ARRAYANSISTRING As Integer = 11
        Public Const DTWAIN_ARRAYWIDESTRING As Integer = 12
        Public Const DTWAIN_ARRAYTWFIX32 As Integer = 200
        Public Const DTWAIN_ArrayTypeINVALID As Integer = 0
        Public Const DTWAIN_ARRAYINT16 As Integer = 100
        Public Const DTWAIN_ARRAYUINT16 As Integer = 110
        Public Const DTWAIN_ARRAYUINT32 As Integer = 120
        Public Const DTWAIN_ARRAYINT32 As Integer = 130
        Public Const DTWAIN_ARRAYINT64 As Integer = 140
        Public Const DTWAIN_ARRAYUINT64 As Integer = 150
        Public Const DTWAIN_RANGELONG As Integer = DTWAIN_ARRAYLONG
        Public Const DTWAIN_RANGEFLOAT As Integer = DTWAIN_ARRAYFLOAT
        Public Const DTWAIN_RANGEMIN As Integer = 0
        Public Const DTWAIN_RANGEMAX As Integer = 1
        Public Const DTWAIN_RANGESTEP As Integer = 2
        Public Const DTWAIN_RANGEDEFAULT As Integer = 3
        Public Const DTWAIN_RANGECURRENT As Integer = 4
        Public Const DTWAIN_FRAMELEFT As Integer = 0
        Public Const DTWAIN_FRAMETOP As Integer = 1
        Public Const DTWAIN_FRAMERIGHT As Integer = 2
        Public Const DTWAIN_FRAMEBOTTOM As Integer = 3
        Public Const DTWAIN_FIX32WHOLE As Integer = 0
        Public Const DTWAIN_FIX32FRAC As Integer = 1
        Public Const DTWAIN_JC_NONE As Integer = 0
        Public Const DTWAIN_JC_JSIC As Integer = 1
        Public Const DTWAIN_JC_JSIS As Integer = 2
        Public Const DTWAIN_JC_JSXC As Integer = 3
        Public Const DTWAIN_JC_JSXS As Integer = 4
        Public Const DTWAIN_CAPDATATYPE_UNKNOWN As Integer = (-10)
        Public Const DTWAIN_JCBP_JSIC As Integer = 5
        Public Const DTWAIN_JCBP_JSIS As Integer = 6
        Public Const DTWAIN_JCBP_JSXC As Integer = 7
        Public Const DTWAIN_JCBP_JSXS As Integer = 8
        Public Const DTWAIN_FEEDPAGEON As Integer = 1
        Public Const DTWAIN_CLEARPAGEON As Integer = 2
        Public Const DTWAIN_REWINDPAGEON As Integer = 4
        Public Const DTWAIN_AppOwnsDib As Integer = 1
        Public Const DTWAIN_SourceOwnsDib As Integer = 2
        Public Const DTWAIN_CONTARRAY As Integer = 8
        Public Const DTWAIN_CONTENUMERATION As Integer = 16
        Public Const DTWAIN_CONTONEVALUE As Integer = 32
        Public Const DTWAIN_CONTRANGE As Integer = 64
        Public Const DTWAIN_CONTDEFAULT As Integer = 0
        Public Const DTWAIN_CAPGET As Integer = 1
        Public Const DTWAIN_CAPGETCURRENT As Integer = 2
        Public Const DTWAIN_CAPGETDEFAULT As Integer = 3
        Public Const DTWAIN_CAPSET As Integer = 6
        Public Const DTWAIN_CAPRESET As Integer = 7
        Public Const DTWAIN_CAPRESETALL As Integer = 8
        Public Const DTWAIN_CAPSETCONSTRAINT As Integer = 9
        Public Const DTWAIN_CAPSETAVAILABLE As Integer = 8
        Public Const DTWAIN_CAPSETCURRENT As Integer = 16
        Public Const DTWAIN_CAPGETHELP As Integer = 9
        Public Const DTWAIN_CAPGETLABEL As Integer = 10
        Public Const DTWAIN_CAPGETLABELENUM As Integer = 11
        Public Const DTWAIN_AREASET As Integer = DTWAIN_CAPSET
        Public Const DTWAIN_AREARESET As Integer = DTWAIN_CAPRESET
        Public Const DTWAIN_AREACURRENT As Integer = DTWAIN_CAPGETCURRENT
        Public Const DTWAIN_AREADEFAULT As Integer = DTWAIN_CAPGETDEFAULT
        Public Const DTWAIN_VER15 As Integer = 0
        Public Const DTWAIN_VER16 As Integer = 1
        Public Const DTWAIN_VER17 As Integer = 2
        Public Const DTWAIN_VER18 As Integer = 3
        Public Const DTWAIN_VER20 As Integer = 4
        Public Const DTWAIN_VER21 As Integer = 5
        Public Const DTWAIN_VER22 As Integer = 6
        Public Const DTWAIN_ACQUIREALL As Integer = (-1)
        Public Const DTWAIN_MAXACQUIRE As Integer = (-1)
        Public Const DTWAIN_DX_NONE As Integer = 0
        Public Const DTWAIN_DX_1PASSDUPLEX As Integer = 1
        Public Const DTWAIN_DX_2PASSDUPLEX As Integer = 2
        Public Const DTWAIN_PT_BW As Integer = 0
        Public Const DTWAIN_PT_GRAY As Integer = 1
        Public Const DTWAIN_PT_RGB As Integer = 2
        Public Const DTWAIN_PT_PALETTE As Integer = 3
        Public Const DTWAIN_PT_CMY As Integer = 4
        Public Const DTWAIN_PT_CMYK As Integer = 5
        Public Const DTWAIN_PT_YUV As Integer = 6
        Public Const DTWAIN_PT_YUVK As Integer = 7
        Public Const DTWAIN_PT_CIEXYZ As Integer = 8
        Public Const DTWAIN_PT_DEFAULT As Integer = 1000
        Public Const DTWAIN_CURRENT As Integer = (-2)
        Public Const DTWAIN_DEFAULT As Integer = (-1)
        Public Const DTWAIN_FLOATDEFAULT As Double = (-9999.0)
        Public Const DTWAIN_CallbackERROR As Integer = 1
        Public Const DTWAIN_CallbackMESSAGE As Integer = 2
        Public Const DTWAIN_USENATIVE As Integer = 1
        Public Const DTWAIN_USEBUFFERED As Integer = 2
        Public Const DTWAIN_USECOMPRESSION As Integer = 4
        Public Const DTWAIN_USEMEMFILE As Integer = 8
        Public Const DTWAIN_FAILURE1 As Integer = (-1)
        Public Const DTWAIN_FAILURE2 As Integer = (-2)
        Public Const DTWAIN_DELETEALL As Integer = (-1)
        Public Const DTWAIN_TN_ACQUIREDONE As Integer = 1000
        Public Const DTWAIN_TN_ACQUIREFAILED As Integer = 1001
        Public Const DTWAIN_TN_ACQUIRECANCELLED As Integer = 1002
        Public Const DTWAIN_TN_ACQUIRESTARTED As Integer = 1003
        Public Const DTWAIN_TN_PAGECONTINUE As Integer = 1004
        Public Const DTWAIN_TN_PAGEFAILED As Integer = 1005
        Public Const DTWAIN_TN_PAGECANCELLED As Integer = 1006
        Public Const DTWAIN_TN_TRANSFERREADY As Integer = 1009
        Public Const DTWAIN_TN_TRANSFERDONE As Integer = 1010
        Public Const DTWAIN_TN_ACQUIREPAGEDONE As Integer = 1010
        Public Const DTWAIN_TN_UICLOSING As Integer = 3000
        Public Const DTWAIN_TN_UICLOSED As Integer = 3001
        Public Const DTWAIN_TN_UIOPENED As Integer = 3002
        Public Const DTWAIN_TN_UIOPENING As Integer = 3003
        Public Const DTWAIN_TN_UIOPENFAILURE As Integer = 3004
        Public Const DTWAIN_TN_CLIPTRANSFERDONE As Integer = 1014
        Public Const DTWAIN_TN_INVALIDIMAGEFORMAT As Integer = 1015
        Public Const DTWAIN_TN_ACQUIRETERMINATED As Integer = 1021
        Public Const DTWAIN_TN_TRANSFERSTRIPREADY As Integer = 1022
        Public Const DTWAIN_TN_TRANSFERSTRIPDONE As Integer = 1023
        Public Const DTWAIN_TN_TRANSFERSTRIPFAILED As Integer = 1029
        Public Const DTWAIN_TN_IMAGEINFOERROR As Integer = 1024
        Public Const DTWAIN_TN_TRANSFERCANCELLED As Integer = 1030
        Public Const DTWAIN_TN_FILESAVECANCELLED As Integer = 1031
        Public Const DTWAIN_TN_FILESAVEOK As Integer = 1032
        Public Const DTWAIN_TN_FILESAVEERROR As Integer = 1033
        Public Const DTWAIN_TN_FILEPAGESAVEOK As Integer = 1034
        Public Const DTWAIN_TN_FILEPAGESAVEERROR As Integer = 1035
        Public Const DTWAIN_TN_PROCESSEDDIB As Integer = 1036
        Public Const DTWAIN_TN_FEEDERLOADED As Integer = 1037
        Public Const DTWAIN_TN_GENERALERROR As Integer = 1038
        Public Const DTWAIN_TN_MANDUPFLIPPAGES As Integer = 1040
        Public Const DTWAIN_TN_MANDUPSIDE1DONE As Integer = 1041
        Public Const DTWAIN_TN_MANDUPSIDE2DONE As Integer = 1042
        Public Const DTWAIN_TN_MANDUPPAGECOUNTERROR As Integer = 1043
        Public Const DTWAIN_TN_MANDUPACQUIREDONE As Integer = 1044
        Public Const DTWAIN_TN_MANDUPSIDE1START As Integer = 1045
        Public Const DTWAIN_TN_MANDUPSIDE2START As Integer = 1046
        Public Const DTWAIN_TN_MANDUPMERGEERROR As Integer = 1047
        Public Const DTWAIN_TN_MANDUPMEMORYERROR As Integer = 1048
        Public Const DTWAIN_TN_MANDUPFILEERROR As Integer = 1049
        Public Const DTWAIN_TN_MANDUPFILESAVEERROR As Integer = 1050
        Public Const DTWAIN_TN_ENDOFJOBDETECTED As Integer = 1051
        Public Const DTWAIN_TN_EOJDETECTED As Integer = 1051
        Public Const DTWAIN_TN_EOJDETECTED_XFERDONE As Integer = 1052
        Public Const DTWAIN_TN_QUERYPAGEDISCARD As Integer = 1053
        Public Const DTWAIN_TN_PAGEDISCARDED As Integer = 1054
        Public Const DTWAIN_TN_PROCESSDIBACCEPTED As Integer = 1055
        Public Const DTWAIN_TN_PROCESSDIBFINALACCEPTED As Integer = 1056
        Public Const DTWAIN_TN_CLOSEDIBFAILED As Integer = 1057
        Public Const DTWAIN_TN_INVALID_TWAINDSM2_BITMAP As Integer = 1058
        Public Const DTWAIN_TN_IMAGE_RESAMPLE_FAILURE As Integer = 1059
        Public Const DTWAIN_TN_DEVICEEVENT As Integer = 1100
        Public Const DTWAIN_TN_TWAINPAGECANCELLED As Integer = 1105
        Public Const DTWAIN_TN_TWAINPAGEFAILED As Integer = 1106
        Public Const DTWAIN_TN_APPUPDATEDDIB As Integer = 1107
        Public Const DTWAIN_TN_FILEPAGESAVING As Integer = 1110
        Public Const DTWAIN_TN_EOJBEGINFILESAVE As Integer = 1112
        Public Const DTWAIN_TN_EOJENDFILESAVE As Integer = 1113
        Public Const DTWAIN_TN_CROPFAILED As Integer = 1120
        Public Const DTWAIN_TN_PROCESSEDDIBFINAL As Integer = 1121
        Public Const DTWAIN_TN_BLANKPAGEDETECTED1 As Integer = 1130
        Public Const DTWAIN_TN_BLANKPAGEDETECTED2 As Integer = 1131
        Public Const DTWAIN_TN_BLANKPAGEDETECTED3 As Integer = 1132
        Public Const DTWAIN_TN_BLANKPAGEDISCARDED1 As Integer = 1133
        Public Const DTWAIN_TN_BLANKPAGEDISCARDED2 As Integer = 1134
        Public Const DTWAIN_TN_OCRTEXTRETRIEVED As Integer = 1140
        Public Const DTWAIN_TN_QUERYOCRTEXT As Integer = 1141
        Public Const DTWAIN_TN_PDFOCRREADY As Integer = 1142
        Public Const DTWAIN_TN_PDFOCRDONE As Integer = 1143
        Public Const DTWAIN_TN_PDFOCRERROR As Integer = 1144
        Public Const DTWAIN_TN_SETCALLBACKINIT As Integer = 1150
        Public Const DTWAIN_TN_SETCALLBACK64INIT As Integer = 1151
        Public Const DTWAIN_TN_FILENAMECHANGING As Integer = 1160
        Public Const DTWAIN_TN_FILENAMECHANGED As Integer = 1161
        Public Const DTWAIN_TN_PROCESSEDAUDIOFINAL As Integer = 1180
        Public Const DTWAIN_TN_PROCESSAUDIOFINALACCEPTED As Integer = 1181
        Public Const DTWAIN_TN_PROCESSEDAUDIOFILE As Integer = 1182
        Public Const DTWAIN_TN_TWAINTRIPLETBEGIN As Integer = 1183
        Public Const DTWAIN_TN_TWAINTRIPLETEND As Integer = 1184
        Public Const DTWAIN_TN_FEEDERNOTLOADED As Integer = 1201
        Public Const DTWAIN_TN_FEEDERTIMEOUT As Integer = 1202
        Public Const DTWAIN_TN_FEEDERNOTENABLED As Integer = 1203
        Public Const DTWAIN_TN_FEEDERNOTSUPPORTED As Integer = 1204
        Public Const DTWAIN_TN_FEEDERTOFLATBED As Integer = 1205
        Public Const DTWAIN_TN_PREACQUIRESTART As Integer = 1206
        Public Const DTWAIN_TN_TRANSFERTILEREADY As Integer = 1300
        Public Const DTWAIN_TN_TRANSFERTILEDONE As Integer = 1301
        Public Const DTWAIN_TN_FILECOMPRESSTYPEMISMATCH As Integer = 1302
        Public Const DTWAIN_PDFOCR_CLEANTEXT1 As Integer = 1
        Public Const DTWAIN_PDFOCR_CLEANTEXT2 As Integer = 2
        Public Const DTWAIN_MODAL As Integer = 0
        Public Const DTWAIN_MODELESS As Integer = 1
        Public Const DTWAIN_UIModeCLOSE As Integer = 0
        Public Const DTWAIN_UIModeOPEN As Integer = 1
        Public Const DTWAIN_REOPEN_SOURCE As Integer = 2
        Public Const DTWAIN_ROUNDNEAREST As Integer = 0
        Public Const DTWAIN_ROUNDUP As Integer = 1
        Public Const DTWAIN_ROUNDDOWN As Integer = 2
        Public Const DTWAIN_FLOATDELTA As Double = (+1.0e-8)
        Public Const DTWAIN_OR_ROT0 As Integer = 0
        Public Const DTWAIN_OR_ROT90 As Integer = 1
        Public Const DTWAIN_OR_ROT180 As Integer = 2
        Public Const DTWAIN_OR_ROT270 As Integer = 3
        Public Const DTWAIN_OR_PORTRAIT As Integer = DTWAIN_OR_ROT0
        Public Const DTWAIN_OR_LANDSCAPE As Integer = DTWAIN_OR_ROT270
        Public Const DTWAIN_OR_ANYROTATION As Integer = (-1)
        Public Const DTWAIN_CO_GET As Integer = &H0001
        Public Const DTWAIN_CO_SET As Integer = &H0002
        Public Const DTWAIN_CO_GETDEFAULT As Integer = &H0004
        Public Const DTWAIN_CO_GETCURRENT As Integer = &H0008
        Public Const DTWAIN_CO_RESET As Integer = &H0010
        Public Const DTWAIN_CO_SETCONSTRAINT As Integer = &H0020
        Public Const DTWAIN_CO_CONSTRAINABLE As Integer = &H0040
        Public Const DTWAIN_CO_GETHELP As Integer = &H0100
        Public Const DTWAIN_CO_GETLABEL As Integer = &H0200
        Public Const DTWAIN_CO_GETLABELENUM As Integer = &H0400
        Public Const DTWAIN_CNTYAFGHANISTAN As Integer = 1001
        Public Const DTWAIN_CNTYALGERIA As Integer = 213
        Public Const DTWAIN_CNTYAMERICANSAMOA As Integer = 684
        Public Const DTWAIN_CNTYANDORRA As Integer = 33
        Public Const DTWAIN_CNTYANGOLA As Integer = 1002
        Public Const DTWAIN_CNTYANGUILLA As Integer = 8090
        Public Const DTWAIN_CNTYANTIGUA As Integer = 8091
        Public Const DTWAIN_CNTYARGENTINA As Integer = 54
        Public Const DTWAIN_CNTYARUBA As Integer = 297
        Public Const DTWAIN_CNTYASCENSIONI As Integer = 247
        Public Const DTWAIN_CNTYAUSTRALIA As Integer = 61
        Public Const DTWAIN_CNTYAUSTRIA As Integer = 43
        Public Const DTWAIN_CNTYBAHAMAS As Integer = 8092
        Public Const DTWAIN_CNTYBAHRAIN As Integer = 973
        Public Const DTWAIN_CNTYBANGLADESH As Integer = 880
        Public Const DTWAIN_CNTYBARBADOS As Integer = 8093
        Public Const DTWAIN_CNTYBELGIUM As Integer = 32
        Public Const DTWAIN_CNTYBELIZE As Integer = 501
        Public Const DTWAIN_CNTYBENIN As Integer = 229
        Public Const DTWAIN_CNTYBERMUDA As Integer = 8094
        Public Const DTWAIN_CNTYBHUTAN As Integer = 1003
        Public Const DTWAIN_CNTYBOLIVIA As Integer = 591
        Public Const DTWAIN_CNTYBOTSWANA As Integer = 267
        Public Const DTWAIN_CNTYBRITAIN As Integer = 6
        Public Const DTWAIN_CNTYBRITVIRGINIS As Integer = 8095
        Public Const DTWAIN_CNTYBRAZIL As Integer = 55
        Public Const DTWAIN_CNTYBRUNEI As Integer = 673
        Public Const DTWAIN_CNTYBULGARIA As Integer = 359
        Public Const DTWAIN_CNTYBURKINAFASO As Integer = 1004
        Public Const DTWAIN_CNTYBURMA As Integer = 1005
        Public Const DTWAIN_CNTYBURUNDI As Integer = 1006
        Public Const DTWAIN_CNTYCAMAROON As Integer = 237
        Public Const DTWAIN_CNTYCANADA As Integer = 2
        Public Const DTWAIN_CNTYCAPEVERDEIS As Integer = 238
        Public Const DTWAIN_CNTYCAYMANIS As Integer = 8096
        Public Const DTWAIN_CNTYCENTRALAFREP As Integer = 1007
        Public Const DTWAIN_CNTYCHAD As Integer = 1008
        Public Const DTWAIN_CNTYCHILE As Integer = 56
        Public Const DTWAIN_CNTYCHINA As Integer = 86
        Public Const DTWAIN_CNTYCHRISTMASIS As Integer = 1009
        Public Const DTWAIN_CNTYCOCOSIS As Integer = 1009
        Public Const DTWAIN_CNTYCOLOMBIA As Integer = 57
        Public Const DTWAIN_CNTYCOMOROS As Integer = 1010
        Public Const DTWAIN_CNTYCONGO As Integer = 1011
        Public Const DTWAIN_CNTYCOOKIS As Integer = 1012
        Public Const DTWAIN_CNTYCOSTARICA As Integer = 506
        Public Const DTWAIN_CNTYCUBA As Integer = 5
        Public Const DTWAIN_CNTYCYPRUS As Integer = 357
        Public Const DTWAIN_CNTYCZECHOSLOVAKIA As Integer = 42
        Public Const DTWAIN_CNTYDENMARK As Integer = 45
        Public Const DTWAIN_CNTYDJIBOUTI As Integer = 1013
        Public Const DTWAIN_CNTYDOMINICA As Integer = 8097
        Public Const DTWAIN_CNTYDOMINCANREP As Integer = 8098
        Public Const DTWAIN_CNTYEASTERIS As Integer = 1014
        Public Const DTWAIN_CNTYECUADOR As Integer = 593
        Public Const DTWAIN_CNTYEGYPT As Integer = 20
        Public Const DTWAIN_CNTYELSALVADOR As Integer = 503
        Public Const DTWAIN_CNTYEQGUINEA As Integer = 1015
        Public Const DTWAIN_CNTYETHIOPIA As Integer = 251
        Public Const DTWAIN_CNTYFALKLANDIS As Integer = 1016
        Public Const DTWAIN_CNTYFAEROEIS As Integer = 298
        Public Const DTWAIN_CNTYFIJIISLANDS As Integer = 679
        Public Const DTWAIN_CNTYFINLAND As Integer = 358
        Public Const DTWAIN_CNTYFRANCE As Integer = 33
        Public Const DTWAIN_CNTYFRANTILLES As Integer = 596
        Public Const DTWAIN_CNTYFRGUIANA As Integer = 594
        Public Const DTWAIN_CNTYFRPOLYNEISA As Integer = 689
        Public Const DTWAIN_CNTYFUTANAIS As Integer = 1043
        Public Const DTWAIN_CNTYGABON As Integer = 241
        Public Const DTWAIN_CNTYGAMBIA As Integer = 220
        Public Const DTWAIN_CNTYGERMANY As Integer = 49
        Public Const DTWAIN_CNTYGHANA As Integer = 233
        Public Const DTWAIN_CNTYGIBRALTER As Integer = 350
        Public Const DTWAIN_CNTYGREECE As Integer = 30
        Public Const DTWAIN_CNTYGREENLAND As Integer = 299
        Public Const DTWAIN_CNTYGRENADA As Integer = 8099
        Public Const DTWAIN_CNTYGRENEDINES As Integer = 8015
        Public Const DTWAIN_CNTYGUADELOUPE As Integer = 590
        Public Const DTWAIN_CNTYGUAM As Integer = 671
        Public Const DTWAIN_CNTYGUANTANAMOBAY As Integer = 5399
        Public Const DTWAIN_CNTYGUATEMALA As Integer = 502
        Public Const DTWAIN_CNTYGUINEA As Integer = 224
        Public Const DTWAIN_CNTYGUINEABISSAU As Integer = 1017
        Public Const DTWAIN_CNTYGUYANA As Integer = 592
        Public Const DTWAIN_CNTYHAITI As Integer = 509
        Public Const DTWAIN_CNTYHONDURAS As Integer = 504
        Public Const DTWAIN_CNTYHONGKONG As Integer = 852
        Public Const DTWAIN_CNTYHUNGARY As Integer = 36
        Public Const DTWAIN_CNTYICELAND As Integer = 354
        Public Const DTWAIN_CNTYINDIA As Integer = 91
        Public Const DTWAIN_CNTYINDONESIA As Integer = 62
        Public Const DTWAIN_CNTYIRAN As Integer = 98
        Public Const DTWAIN_CNTYIRAQ As Integer = 964
        Public Const DTWAIN_CNTYIRELAND As Integer = 353
        Public Const DTWAIN_CNTYISRAEL As Integer = 972
        Public Const DTWAIN_CNTYITALY As Integer = 39
        Public Const DTWAIN_CNTYIVORYCOAST As Integer = 225
        Public Const DTWAIN_CNTYJAMAICA As Integer = 8010
        Public Const DTWAIN_CNTYJAPAN As Integer = 81
        Public Const DTWAIN_CNTYJORDAN As Integer = 962
        Public Const DTWAIN_CNTYKENYA As Integer = 254
        Public Const DTWAIN_CNTYKIRIBATI As Integer = 1018
        Public Const DTWAIN_CNTYKOREA As Integer = 82
        Public Const DTWAIN_CNTYKUWAIT As Integer = 965
        Public Const DTWAIN_CNTYLAOS As Integer = 1019
        Public Const DTWAIN_CNTYLEBANON As Integer = 1020
        Public Const DTWAIN_CNTYLIBERIA As Integer = 231
        Public Const DTWAIN_CNTYLIBYA As Integer = 218
        Public Const DTWAIN_CNTYLIECHTENSTEIN As Integer = 41
        Public Const DTWAIN_CNTYLUXENBOURG As Integer = 352
        Public Const DTWAIN_CNTYMACAO As Integer = 853
        Public Const DTWAIN_CNTYMADAGASCAR As Integer = 1021
        Public Const DTWAIN_CNTYMALAWI As Integer = 265
        Public Const DTWAIN_CNTYMALAYSIA As Integer = 60
        Public Const DTWAIN_CNTYMALDIVES As Integer = 960
        Public Const DTWAIN_CNTYMALI As Integer = 1022
        Public Const DTWAIN_CNTYMALTA As Integer = 356
        Public Const DTWAIN_CNTYMARSHALLIS As Integer = 692
        Public Const DTWAIN_CNTYMAURITANIA As Integer = 1023
        Public Const DTWAIN_CNTYMAURITIUS As Integer = 230
        Public Const DTWAIN_CNTYMEXICO As Integer = 3
        Public Const DTWAIN_CNTYMICRONESIA As Integer = 691
        Public Const DTWAIN_CNTYMIQUELON As Integer = 508
        Public Const DTWAIN_CNTYMONACO As Integer = 33
        Public Const DTWAIN_CNTYMONGOLIA As Integer = 1024
        Public Const DTWAIN_CNTYMONTSERRAT As Integer = 8011
        Public Const DTWAIN_CNTYMOROCCO As Integer = 212
        Public Const DTWAIN_CNTYMOZAMBIQUE As Integer = 1025
        Public Const DTWAIN_CNTYNAMIBIA As Integer = 264
        Public Const DTWAIN_CNTYNAURU As Integer = 1026
        Public Const DTWAIN_CNTYNEPAL As Integer = 977
        Public Const DTWAIN_CNTYNETHERLANDS As Integer = 31
        Public Const DTWAIN_CNTYNETHANTILLES As Integer = 599
        Public Const DTWAIN_CNTYNEVIS As Integer = 8012
        Public Const DTWAIN_CNTYNEWCALEDONIA As Integer = 687
        Public Const DTWAIN_CNTYNEWZEALAND As Integer = 64
        Public Const DTWAIN_CNTYNICARAGUA As Integer = 505
        Public Const DTWAIN_CNTYNIGER As Integer = 227
        Public Const DTWAIN_CNTYNIGERIA As Integer = 234
        Public Const DTWAIN_CNTYNIUE As Integer = 1027
        Public Const DTWAIN_CNTYNORFOLKI As Integer = 1028
        Public Const DTWAIN_CNTYNORWAY As Integer = 47
        Public Const DTWAIN_CNTYOMAN As Integer = 968
        Public Const DTWAIN_CNTYPAKISTAN As Integer = 92
        Public Const DTWAIN_CNTYPALAU As Integer = 1029
        Public Const DTWAIN_CNTYPANAMA As Integer = 507
        Public Const DTWAIN_CNTYPARAGUAY As Integer = 595
        Public Const DTWAIN_CNTYPERU As Integer = 51
        Public Const DTWAIN_CNTYPHILLIPPINES As Integer = 63
        Public Const DTWAIN_CNTYPITCAIRNIS As Integer = 1030
        Public Const DTWAIN_CNTYPNEWGUINEA As Integer = 675
        Public Const DTWAIN_CNTYPOLAND As Integer = 48
        Public Const DTWAIN_CNTYPORTUGAL As Integer = 351
        Public Const DTWAIN_CNTYQATAR As Integer = 974
        Public Const DTWAIN_CNTYREUNIONI As Integer = 1031
        Public Const DTWAIN_CNTYROMANIA As Integer = 40
        Public Const DTWAIN_CNTYRWANDA As Integer = 250
        Public Const DTWAIN_CNTYSAIPAN As Integer = 670
        Public Const DTWAIN_CNTYSANMARINO As Integer = 39
        Public Const DTWAIN_CNTYSAOTOME As Integer = 1033
        Public Const DTWAIN_CNTYSAUDIARABIA As Integer = 966
        Public Const DTWAIN_CNTYSENEGAL As Integer = 221
        Public Const DTWAIN_CNTYSEYCHELLESIS As Integer = 1034
        Public Const DTWAIN_CNTYSIERRALEONE As Integer = 1035
        Public Const DTWAIN_CNTYSINGAPORE As Integer = 65
        Public Const DTWAIN_CNTYSOLOMONIS As Integer = 1036
        Public Const DTWAIN_CNTYSOMALI As Integer = 1037
        Public Const DTWAIN_CNTYSOUTHAFRICA As Integer = 27
        Public Const DTWAIN_CNTYSPAIN As Integer = 34
        Public Const DTWAIN_CNTYSRILANKA As Integer = 94
        Public Const DTWAIN_CNTYSTHELENA As Integer = 1032
        Public Const DTWAIN_CNTYSTKITTS As Integer = 8013
        Public Const DTWAIN_CNTYSTLUCIA As Integer = 8014
        Public Const DTWAIN_CNTYSTPIERRE As Integer = 508
        Public Const DTWAIN_CNTYSTVINCENT As Integer = 8015
        Public Const DTWAIN_CNTYSUDAN As Integer = 1038
        Public Const DTWAIN_CNTYSURINAME As Integer = 597
        Public Const DTWAIN_CNTYSWAZILAND As Integer = 268
        Public Const DTWAIN_CNTYSWEDEN As Integer = 46
        Public Const DTWAIN_CNTYSWITZERLAND As Integer = 41
        Public Const DTWAIN_CNTYSYRIA As Integer = 1039
        Public Const DTWAIN_CNTYTAIWAN As Integer = 886
        Public Const DTWAIN_CNTYTANZANIA As Integer = 255
        Public Const DTWAIN_CNTYTHAILAND As Integer = 66
        Public Const DTWAIN_CNTYTOBAGO As Integer = 8016
        Public Const DTWAIN_CNTYTOGO As Integer = 228
        Public Const DTWAIN_CNTYTONGAIS As Integer = 676
        Public Const DTWAIN_CNTYTRINIDAD As Integer = 8016
        Public Const DTWAIN_CNTYTUNISIA As Integer = 216
        Public Const DTWAIN_CNTYTURKEY As Integer = 90
        Public Const DTWAIN_CNTYTURKSCAICOS As Integer = 8017
        Public Const DTWAIN_CNTYTUVALU As Integer = 1040
        Public Const DTWAIN_CNTYUGANDA As Integer = 256
        Public Const DTWAIN_CNTYUSSR As Integer = 7
        Public Const DTWAIN_CNTYUAEMIRATES As Integer = 971
        Public Const DTWAIN_CNTYUNITEDKINGDOM As Integer = 44
        Public Const DTWAIN_CNTYUSA As Integer = 1
        Public Const DTWAIN_CNTYURUGUAY As Integer = 598
        Public Const DTWAIN_CNTYVANUATU As Integer = 1041
        Public Const DTWAIN_CNTYVATICANCITY As Integer = 39
        Public Const DTWAIN_CNTYVENEZUELA As Integer = 58
        Public Const DTWAIN_CNTYWAKE As Integer = 1042
        Public Const DTWAIN_CNTYWALLISIS As Integer = 1043
        Public Const DTWAIN_CNTYWESTERNSAHARA As Integer = 1044
        Public Const DTWAIN_CNTYWESTERNSAMOA As Integer = 1045
        Public Const DTWAIN_CNTYYEMEN As Integer = 1046
        Public Const DTWAIN_CNTYYUGOSLAVIA As Integer = 38
        Public Const DTWAIN_CNTYZAIRE As Integer = 243
        Public Const DTWAIN_CNTYZAMBIA As Integer = 260
        Public Const DTWAIN_CNTYZIMBABWE As Integer = 263
        Public Const DTWAIN_LANGDANISH As Integer = 0
        Public Const DTWAIN_LANGDUTCH As Integer = 1
        Public Const DTWAIN_LANGINTERNATIONALENGLISH As Integer = 2
        Public Const DTWAIN_LANGFRENCHCANADIAN As Integer = 3
        Public Const DTWAIN_LANGFINNISH As Integer = 4
        Public Const DTWAIN_LANGFRENCH As Integer = 5
        Public Const DTWAIN_LANGGERMAN As Integer = 6
        Public Const DTWAIN_LANGICELANDIC As Integer = 7
        Public Const DTWAIN_LANGITALIAN As Integer = 8
        Public Const DTWAIN_LANGNORWEGIAN As Integer = 9
        Public Const DTWAIN_LANGPORTUGUESE As Integer = 10
        Public Const DTWAIN_LANGSPANISH As Integer = 11
        Public Const DTWAIN_LANGSWEDISH As Integer = 12
        Public Const DTWAIN_LANGUSAENGLISH As Integer = 13
        Public Const DTWAIN_NO_ERROR As Integer = (0)
        Public Const DTWAIN_ERR_FIRST As Integer = (-1000)
        Public Const DTWAIN_ERR_BAD_HANDLE As Integer = (-1001)
        Public Const DTWAIN_ERR_BAD_SOURCE As Integer = (-1002)
        Public Const DTWAIN_ERR_BAD_ARRAY As Integer = (-1003)
        Public Const DTWAIN_ERR_WRONG_ARRAY_TYPE As Integer = (-1004)
        Public Const DTWAIN_ERR_INDEX_BOUNDS As Integer = (-1005)
        Public Const DTWAIN_ERR_OUT_OF_MEMORY As Integer = (-1006)
        Public Const DTWAIN_ERR_NULL_WINDOW As Integer = (-1007)
        Public Const DTWAIN_ERR_BAD_PIXTYPE As Integer = (-1008)
        Public Const DTWAIN_ERR_BAD_CONTAINER As Integer = (-1009)
        Public Const DTWAIN_ERR_NO_SESSION As Integer = (-1010)
        Public Const DTWAIN_ERR_BAD_ACQUIRE_NUM As Integer = (-1011)
        Public Const DTWAIN_ERR_BAD_CAP As Integer = (-1012)
        Public Const DTWAIN_ERR_CAP_NO_SUPPORT As Integer = (-1013)
        Public Const DTWAIN_ERR_TWAIN As Integer = (-1014)
        Public Const DTWAIN_ERR_HOOK_FAILED As Integer = (-1015)
        Public Const DTWAIN_ERR_BAD_FILENAME As Integer = (-1016)
        Public Const DTWAIN_ERR_EMPTY_ARRAY As Integer = (-1017)
        Public Const DTWAIN_ERR_FILE_FORMAT As Integer = (-1018)
        Public Const DTWAIN_ERR_BAD_DIB_PAGE As Integer = (-1019)
        Public Const DTWAIN_ERR_SOURCE_ACQUIRING As Integer = (-1020)
        Public Const DTWAIN_ERR_INVALID_PARAM As Integer = (-1021)
        Public Const DTWAIN_ERR_INVALID_RANGE As Integer = (-1022)
        Public Const DTWAIN_ERR_UI_ERROR As Integer = (-1023)
        Public Const DTWAIN_ERR_BAD_UNIT As Integer = (-1024)
        Public Const DTWAIN_ERR_LANGDLL_NOT_FOUND As Integer = (-1025)
        Public Const DTWAIN_ERR_SOURCE_NOT_OPEN As Integer = (-1026)
        Public Const DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED As Integer = (-1027)
        Public Const DTWAIN_ERR_UIONLY_NOT_SUPPORTED As Integer = (-1028)
        Public Const DTWAIN_ERR_UI_ALREADY_OPENED As Integer = (-1029)
        Public Const DTWAIN_ERR_CAPSET_NOSUPPORT As Integer = (-1030)
        Public Const DTWAIN_ERR_NO_FILE_XFER As Integer = (-1031)
        Public Const DTWAIN_ERR_INVALID_BITDEPTH As Integer = (-1032)
        Public Const DTWAIN_ERR_NO_CAPS_DEFINED As Integer = (-1033)
        Public Const DTWAIN_ERR_TILES_NOT_SUPPORTED As Integer = (-1034)
        Public Const DTWAIN_ERR_INVALID_DTWAIN_FRAME As Integer = (-1035)
        Public Const DTWAIN_ERR_LIMITED_VERSION As Integer = (-1036)
        Public Const DTWAIN_ERR_NO_FEEDER As Integer = (-1037)
        Public Const DTWAIN_ERR_NO_FEEDER_QUERY As Integer = (-1038)
        Public Const DTWAIN_ERR_EXCEPTION_ERROR As Integer = (-1039)
        Public Const DTWAIN_ERR_INVALID_STATE As Integer = (-1040)
        Public Const DTWAIN_ERR_UNSUPPORTED_EXTINFO As Integer = (-1041)
        Public Const DTWAIN_ERR_DLLRESOURCE_NOTFOUND As Integer = (-1042)
        Public Const DTWAIN_ERR_NOT_INITIALIZED As Integer = (-1043)
        Public Const DTWAIN_ERR_NO_SOURCES As Integer = (-1044)
        Public Const DTWAIN_ERR_TWAIN_NOT_INSTALLED As Integer = (-1045)
        Public Const DTWAIN_ERR_WRONG_THREAD As Integer = (-1046)
        Public Const DTWAIN_ERR_BAD_CAPTYPE As Integer = (-1047)
        Public Const DTWAIN_ERR_UNKNOWN_CAPDATATYPE As Integer = (-1048)
        Public Const DTWAIN_ERR_DEMO_NOFILETYPE As Integer = (-1049)
        Public Const DTWAIN_ERR_SOURCESELECTION_CANCELED As Integer = (-1050)
        Public Const DTWAIN_ERR_RESOURCES_NOT_FOUND As Integer = (-1051)
        Public Const DTWAIN_ERR_STRINGTYPE_MISMATCH As Integer = (-1052)
        Public Const DTWAIN_ERR_ARRAYTYPE_MISMATCH As Integer = (-1053)
        Public Const DTWAIN_ERR_SOURCENAME_NOTINSTALLED As Integer = (-1054)
        Public Const DTWAIN_ERR_NO_MEMFILE_XFER As Integer = (-1055)
        Public Const DTWAIN_ERR_AREA_ARRAY_TOO_SMALL As Integer = (-1056)
        Public Const DTWAIN_ERR_LOG_CREATE_ERROR As Integer = (-1057)
        Public Const DTWAIN_ERR_FILESYSTEM_NOT_SUPPORTED As Integer = (-1058)
        Public Const DTWAIN_ERR_TILEMODE_NOTSET As Integer = (-1059)
        Public Const DTWAIN_ERR_INI32_NOT_FOUND As Integer = (-1060)
        Public Const DTWAIN_ERR_INI64_NOT_FOUND As Integer = (-1061)
        Public Const DTWAIN_ERR_CRC_CHECK As Integer = (-1062)
        Public Const DTWAIN_ERR_RESOURCES_BAD_VERSION As Integer = (-1063)
        Public Const DTWAIN_ERR_WIN32_ERROR As Integer = (-1064)
        Public Const DTWAIN_ERR_STRINGID_NOTFOUND As Integer = (-1065)
        Public Const DTWAIN_ERR_RESOURCES_DUPLICATEID_FOUND As Integer = (-1066)
        Public Const DTWAIN_ERR_UNAVAILABLE_EXTINFO As Integer = (-1067)
        Public Const DTWAIN_ERR_TWAINDSM2_BADBITMAP As Integer = (-1068)
        Public Const DTWAIN_ERR_ACQUISITION_CANCELED As Integer = (-1069)
        Public Const DTWAIN_ERR_IMAGE_RESAMPLED As Integer = (-1070)
        Public Const DTWAIN_ERR_UNKNOWN_TWAIN_RC As Integer = (-1071)
        Public Const DTWAIN_ERR_UNKNOWN_TWAIN_CC As Integer = (-1072)
        Public Const DTWAIN_ERR_RESOURCES_DATA_EXCEPTION As Integer = (-1073)
        Public Const DTWAIN_ERR_AUDIO_TRANSFER_NOTSUPPORTED As Integer = (-1074)
        Public Const DTWAIN_ERR_FEEDER_COMPLIANCY As Integer = (-1075)
        Public Const DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY1 As Integer = (-1076)
        Public Const DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY2 As Integer = (-1077)
        Public Const DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY1 As Integer = (-1078)
        Public Const DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY2 As Integer = (-1079)
        Public Const DTWAIN_ERR_ICAPBITDEPTH_COMPLIANCY1 As Integer = (-1080)
        Public Const DTWAIN_ERR_XFERMECH_COMPLIANCY As Integer = (-1081)
        Public Const DTWAIN_ERR_STANDARDCAPS_COMPLIANCY As Integer = (-1082)
        Public Const DTWAIN_ERR_EXTIMAGEINFO_DATATYPE_MISMATCH As Integer = (-1083)
        Public Const DTWAIN_ERR_EXTIMAGEINFO_RETRIEVAL As Integer = (-1084)
        Public Const DTWAIN_ERR_RANGE_OUTOFBOUNDS As Integer = (-1085)
        Public Const DTWAIN_ERR_RANGE_STEPISZERO As Integer = (-1086)
        Public Const DTWAIN_ERR_BLANKNAMEDETECTED As Integer = (-1087)
        Public Const DTWAIN_ERR_FEEDER_NOPAPERSENSOR As Integer = (-1088)
        Public Const TWAIN_ERR_LOW_MEMORY As Integer = (-1100)
        Public Const TWAIN_ERR_FALSE_ALARM As Integer = (-1101)
        Public Const TWAIN_ERR_BUMMER As Integer = (-1102)
        Public Const TWAIN_ERR_NODATASOURCE As Integer = (-1103)
        Public Const TWAIN_ERR_MAXCONNECTIONS As Integer = (-1104)
        Public Const TWAIN_ERR_OPERATIONERROR As Integer = (-1105)
        Public Const TWAIN_ERR_BADCAPABILITY As Integer = (-1106)
        Public Const TWAIN_ERR_BADVALUE As Integer = (-1107)
        Public Const TWAIN_ERR_BADPROTOCOL As Integer = (-1108)
        Public Const TWAIN_ERR_SEQUENCEERROR As Integer = (-1109)
        Public Const TWAIN_ERR_BADDESTINATION As Integer = (-1110)
        Public Const TWAIN_ERR_CAPNOTSUPPORTED As Integer = (-1111)
        Public Const TWAIN_ERR_CAPBADOPERATION As Integer = (-1112)
        Public Const TWAIN_ERR_CAPSEQUENCEERROR As Integer = (-1113)
        Public Const TWAIN_ERR_FILEPROTECTEDERROR As Integer = (-1114)
        Public Const TWAIN_ERR_FILEEXISTERROR As Integer = (-1115)
        Public Const TWAIN_ERR_FILENOTFOUND As Integer = (-1116)
        Public Const TWAIN_ERR_DIRNOTEMPTY As Integer = (-1117)
        Public Const TWAIN_ERR_FEEDERJAMMED As Integer = (-1118)
        Public Const TWAIN_ERR_FEEDERMULTPAGES As Integer = (-1119)
        Public Const TWAIN_ERR_FEEDERWRITEERROR As Integer = (-1120)
        Public Const TWAIN_ERR_DEVICEOFFLINE As Integer = (-1121)
        Public Const TWAIN_ERR_NULL_CONTAINER As Integer = (-1122)
        Public Const TWAIN_ERR_INTERLOCK As Integer = (-1123)
        Public Const TWAIN_ERR_DAMAGEDCORNER As Integer = (-1124)
        Public Const TWAIN_ERR_FOCUSERROR As Integer = (-1125)
        Public Const TWAIN_ERR_DOCTOOLIGHT As Integer = (-1126)
        Public Const TWAIN_ERR_DOCTOODARK As Integer = (-1127)
        Public Const TWAIN_ERR_NOMEDIA As Integer = (-1128)
        Public Const DTWAIN_ERR_FILEXFERSTART As Integer = (-2000)
        Public Const DTWAIN_ERR_MEM As Integer = (-2001)
        Public Const DTWAIN_ERR_FILEOPEN As Integer = (-2002)
        Public Const DTWAIN_ERR_FILEREAD As Integer = (-2003)
        Public Const DTWAIN_ERR_FILEWRITE As Integer = (-2004)
        Public Const DTWAIN_ERR_BADPARAM As Integer = (-2005)
        Public Const DTWAIN_ERR_INVALIDBMP As Integer = (-2006)
        Public Const DTWAIN_ERR_BMPRLE As Integer = (-2007)
        Public Const DTWAIN_ERR_RESERVED1 As Integer = (-2008)
        Public Const DTWAIN_ERR_INVALIDJPG As Integer = (-2009)
        Public Const DTWAIN_ERR_DC As Integer = (-2010)
        Public Const DTWAIN_ERR_DIB As Integer = (-2011)
        Public Const DTWAIN_ERR_RESERVED2 As Integer = (-2012)
        Public Const DTWAIN_ERR_NORESOURCE As Integer = (-2013)
        Public Const DTWAIN_ERR_CALLBACKCANCEL As Integer = (-2014)
        Public Const DTWAIN_ERR_INVALIDPNG As Integer = (-2015)
        Public Const DTWAIN_ERR_PNGCREATE As Integer = (-2016)
        Public Const DTWAIN_ERR_INTERNAL As Integer = (-2017)
        Public Const DTWAIN_ERR_FONT As Integer = (-2018)
        Public Const DTWAIN_ERR_INTTIFF As Integer = (-2019)
        Public Const DTWAIN_ERR_INVALIDTIFF As Integer = (-2020)
        Public Const DTWAIN_ERR_NOTIFFLZW As Integer = (-2021)
        Public Const DTWAIN_ERR_INVALIDPCX As Integer = (-2022)
        Public Const DTWAIN_ERR_CREATEBMP As Integer = (-2023)
        Public Const DTWAIN_ERR_NOLINES As Integer = (-2024)
        Public Const DTWAIN_ERR_GETDIB As Integer = (-2025)
        Public Const DTWAIN_ERR_NODEVOP As Integer = (-2026)
        Public Const DTWAIN_ERR_INVALIDWMF As Integer = (-2027)
        Public Const DTWAIN_ERR_DEPTHMISMATCH As Integer = (-2028)
        Public Const DTWAIN_ERR_BITBLT As Integer = (-2029)
        Public Const DTWAIN_ERR_BUFTOOSMALL As Integer = (-2030)
        Public Const DTWAIN_ERR_TOOMANYCOLORS As Integer = (-2031)
        Public Const DTWAIN_ERR_INVALIDTGA As Integer = (-2032)
        Public Const DTWAIN_ERR_NOTGATHUMBNAIL As Integer = (-2033)
        Public Const DTWAIN_ERR_RESERVED3 As Integer = (-2034)
        Public Const DTWAIN_ERR_CREATEDIB As Integer = (-2035)
        Public Const DTWAIN_ERR_NOLZW As Integer = (-2036)
        Public Const DTWAIN_ERR_SELECTOBJ As Integer = (-2037)
        Public Const DTWAIN_ERR_BADMANAGER As Integer = (-2038)
        Public Const DTWAIN_ERR_OBSOLETE As Integer = (-2039)
        Public Const DTWAIN_ERR_CREATEDIBSECTION As Integer = (-2040)
        Public Const DTWAIN_ERR_SETWINMETAFILEBITS As Integer = (-2041)
        Public Const DTWAIN_ERR_GETWINMETAFILEBITS As Integer = (-2042)
        Public Const DTWAIN_ERR_PAXPWD As Integer = (-2043)
        Public Const DTWAIN_ERR_INVALIDPAX As Integer = (-2044)
        Public Const DTWAIN_ERR_NOSUPPORT As Integer = (-2045)
        Public Const DTWAIN_ERR_INVALIDPSD As Integer = (-2046)
        Public Const DTWAIN_ERR_PSDNOTSUPPORTED As Integer = (-2047)
        Public Const DTWAIN_ERR_DECRYPT As Integer = (-2048)
        Public Const DTWAIN_ERR_ENCRYPT As Integer = (-2049)
        Public Const DTWAIN_ERR_COMPRESSION As Integer = (-2050)
        Public Const DTWAIN_ERR_DECOMPRESSION As Integer = (-2051)
        Public Const DTWAIN_ERR_INVALIDTLA As Integer = (-2052)
        Public Const DTWAIN_ERR_INVALIDWBMP As Integer = (-2053)
        Public Const DTWAIN_ERR_NOTIFFTAG As Integer = (-2054)
        Public Const DTWAIN_ERR_NOLOCALSTORAGE As Integer = (-2055)
        Public Const DTWAIN_ERR_INVALIDEXIF As Integer = (-2056)
        Public Const DTWAIN_ERR_NOEXIFSTRING As Integer = (-2057)
        Public Const DTWAIN_ERR_TIFFDLL32NOTFOUND As Integer = (-2058)
        Public Const DTWAIN_ERR_TIFFDLL16NOTFOUND As Integer = (-2059)
        Public Const DTWAIN_ERR_PNGDLL16NOTFOUND As Integer = (-2060)
        Public Const DTWAIN_ERR_JPEGDLL16NOTFOUND As Integer = (-2061)
        Public Const DTWAIN_ERR_BADBITSPERPIXEL As Integer = (-2062)
        Public Const DTWAIN_ERR_TIFFDLL32INVALIDVER As Integer = (-2063)
        Public Const DTWAIN_ERR_PDFDLL32NOTFOUND As Integer = (-2064)
        Public Const DTWAIN_ERR_PDFDLL32INVALIDVER As Integer = (-2065)
        Public Const DTWAIN_ERR_JPEGDLL32NOTFOUND As Integer = (-2066)
        Public Const DTWAIN_ERR_JPEGDLL32INVALIDVER As Integer = (-2067)
        Public Const DTWAIN_ERR_PNGDLL32NOTFOUND As Integer = (-2068)
        Public Const DTWAIN_ERR_PNGDLL32INVALIDVER As Integer = (-2069)
        Public Const DTWAIN_ERR_J2KDLL32NOTFOUND As Integer = (-2070)
        Public Const DTWAIN_ERR_J2KDLL32INVALIDVER As Integer = (-2071)
        Public Const DTWAIN_ERR_MANDUPLEX_UNAVAILABLE As Integer = (-2072)
        Public Const DTWAIN_ERR_TIMEOUT As Integer = (-2073)
        Public Const DTWAIN_ERR_INVALIDICONFORMAT As Integer = (-2074)
        Public Const DTWAIN_ERR_TWAIN32DSMNOTFOUND As Integer = (-2075)
        Public Const DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND As Integer = (-2076)
        Public Const DTWAIN_ERR_INVALID_DIRECTORY As Integer = (-2077)
        Public Const DTWAIN_ERR_CREATE_DIRECTORY As Integer = (-2078)
        Public Const DTWAIN_ERR_OCRLIBRARY_NOTFOUND As Integer = (-2079)
        Public Const DTWAIN_TWAINSAVE_OK As Integer = (0)
        Public Const DTWAIN_ERR_TS_FIRST As Integer = (-2080)
        Public Const DTWAIN_ERR_TS_NOFILENAME As Integer = (-2081)
        Public Const DTWAIN_ERR_TS_NOTWAINSYS As Integer = (-2082)
        Public Const DTWAIN_ERR_TS_DEVICEFAILURE As Integer = (-2083)
        Public Const DTWAIN_ERR_TS_FILESAVEERROR As Integer = (-2084)
        Public Const DTWAIN_ERR_TS_COMMANDILLEGAL As Integer = (-2085)
        Public Const DTWAIN_ERR_TS_CANCELLED As Integer = (-2086)
        Public Const DTWAIN_ERR_TS_ACQUISITIONERROR As Integer = (-2087)
        Public Const DTWAIN_ERR_TS_INVALIDCOLORSPACE As Integer = (-2088)
        Public Const DTWAIN_ERR_TS_PDFNOTSUPPORTED As Integer = (-2089)
        Public Const DTWAIN_ERR_TS_NOTAVAILABLE As Integer = (-2090)
        Public Const DTWAIN_ERR_OCR_FIRST As Integer = (-2100)
        Public Const DTWAIN_ERR_OCR_INVALIDPAGENUM As Integer = (-2101)
        Public Const DTWAIN_ERR_OCR_INVALIDENGINE As Integer = (-2102)
        Public Const DTWAIN_ERR_OCR_NOTACTIVE As Integer = (-2103)
        Public Const DTWAIN_ERR_OCR_INVALIDFILETYPE As Integer = (-2104)
        Public Const DTWAIN_ERR_OCR_INVALIDPIXELTYPE As Integer = (-2105)
        Public Const DTWAIN_ERR_OCR_INVALIDBITDEPTH As Integer = (-2106)
        Public Const DTWAIN_ERR_OCR_RECOGNITIONERROR As Integer = (-2107)
        Public Const DTWAIN_ERR_OCR_LAST As Integer = (-2108)
        Public Const DTWAIN_ERR_LAST As Integer = DTWAIN_ERR_OCR_LAST
        Public Const DTWAIN_ERR_SOURCE_COULD_NOT_OPEN As Integer = (-2500)
        Public Const DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE As Integer = (-2501)
        Public Const DTWAIN_ERR_IMAGEINFO_INVALID As Integer = (-2502)
        Public Const DTWAIN_ERR_WRITEDATA_TOFILE As Integer = (-2503)
        Public Const DTWAIN_ERR_OPERATION_NOTSUPPORTED As Integer = (-2504)
        Public Const DTWAIN_DE_CHKAUTOCAPTURE As Integer = 1
        Public Const DTWAIN_DE_CHKBATTERY As Integer = 2
        Public Const DTWAIN_DE_CHKDEVICEONLINE As Integer = 4
        Public Const DTWAIN_DE_CHKFLASH As Integer = 8
        Public Const DTWAIN_DE_CHKPOWERSUPPLY As Integer = 16
        Public Const DTWAIN_DE_CHKRESOLUTION As Integer = 32
        Public Const DTWAIN_DE_DEVICEADDED As Integer = 64
        Public Const DTWAIN_DE_DEVICEOFFLINE As Integer = 128
        Public Const DTWAIN_DE_DEVICEREADY As Integer = 256
        Public Const DTWAIN_DE_DEVICEREMOVED As Integer = 512
        Public Const DTWAIN_DE_IMAGECAPTURED As Integer = 1024
        Public Const DTWAIN_DE_IMAGEDELETED As Integer = 2048
        Public Const DTWAIN_DE_PAPERDOUBLEFEED As Integer = 4096
        Public Const DTWAIN_DE_PAPERJAM As Integer = 8192
        Public Const DTWAIN_DE_LAMPFAILURE As Integer = 16384
        Public Const DTWAIN_DE_POWERSAVE As Integer = 32768
        Public Const DTWAIN_DE_POWERSAVENOTIFY As Integer = 65536
        Public Const DTWAIN_DE_CUSTOMEVENTS As Integer = &H8000
        Public Const DTWAIN_GETDE_EVENT As Integer = 0
        Public Const DTWAIN_GETDE_DEVNAME As Integer = 1
        Public Const DTWAIN_GETDE_BATTERYMINUTES As Integer = 2
        Public Const DTWAIN_GETDE_BATTERYPCT As Integer = 3
        Public Const DTWAIN_GETDE_XRESOLUTION As Integer = 4
        Public Const DTWAIN_GETDE_YRESOLUTION As Integer = 5
        Public Const DTWAIN_GETDE_FLASHUSED As Integer = 6
        Public Const DTWAIN_GETDE_AUTOCAPTURE As Integer = 7
        Public Const DTWAIN_GETDE_TIMEBEFORECAPTURE As Integer = 8
        Public Const DTWAIN_GETDE_TIMEBETWEENCAPTURES As Integer = 9
        Public Const DTWAIN_GETDE_POWERSUPPLY As Integer = 10
        Public Const DTWAIN_IMPRINTERTOPBEFORE As Integer = 1
        Public Const DTWAIN_IMPRINTERTOPAFTER As Integer = 2
        Public Const DTWAIN_IMPRINTERBOTTOMBEFORE As Integer = 4
        Public Const DTWAIN_IMPRINTERBOTTOMAFTER As Integer = 8
        Public Const DTWAIN_ENDORSERTOPBEFORE As Integer = 16
        Public Const DTWAIN_ENDORSERTOPAFTER As Integer = 32
        Public Const DTWAIN_ENDORSERBOTTOMBEFORE As Integer = 64
        Public Const DTWAIN_ENDORSERBOTTOMAFTER As Integer = 128
        Public Const DTWAIN_PM_SINGLESTRING As Integer = 0
        Public Const DTWAIN_PM_MULTISTRING As Integer = 1
        Public Const DTWAIN_PM_COMPOUNDSTRING As Integer = 2
        Public Const DTWAIN_TWTY_INT8 As Integer = &H0000
        Public Const DTWAIN_TWTY_INT16 As Integer = &H0001
        Public Const DTWAIN_TWTY_INT32 As Integer = &H0002
        Public Const DTWAIN_TWTY_UINT8 As Integer = &H0003
        Public Const DTWAIN_TWTY_UINT16 As Integer = &H0004
        Public Const DTWAIN_TWTY_UINT32 As Integer = &H0005
        Public Const DTWAIN_TWTY_BOOL As Integer = &H0006
        Public Const DTWAIN_TWTY_FIX32 As Integer = &H0007
        Public Const DTWAIN_TWTY_FRAME As Integer = &H0008
        Public Const DTWAIN_TWTY_STR32 As Integer = &H0009
        Public Const DTWAIN_TWTY_STR64 As Integer = &H000A
        Public Const DTWAIN_TWTY_STR128 As Integer = &H000B
        Public Const DTWAIN_TWTY_STR255 As Integer = &H000C
        Public Const DTWAIN_TWTY_STR1024 As Integer = &H000D
        Public Const DTWAIN_TWTY_UNI512 As Integer = &H000E
        Public Const DTWAIN_EI_BARCODEX As Integer = &H1200
        Public Const DTWAIN_EI_BARCODEY As Integer = &H1201
        Public Const DTWAIN_EI_BARCODETEXT As Integer = &H1202
        Public Const DTWAIN_EI_BARCODETYPE As Integer = &H1203
        Public Const DTWAIN_EI_DESHADETOP As Integer = &H1204
        Public Const DTWAIN_EI_DESHADELEFT As Integer = &H1205
        Public Const DTWAIN_EI_DESHADEHEIGHT As Integer = &H1206
        Public Const DTWAIN_EI_DESHADEWIDTH As Integer = &H1207
        Public Const DTWAIN_EI_DESHADESIZE As Integer = &H1208
        Public Const DTWAIN_EI_SPECKLESREMOVED As Integer = &H1209
        Public Const DTWAIN_EI_HORZLINEXCOORD As Integer = &H120A
        Public Const DTWAIN_EI_HORZLINEYCOORD As Integer = &H120B
        Public Const DTWAIN_EI_HORZLINELENGTH As Integer = &H120C
        Public Const DTWAIN_EI_HORZLINETHICKNESS As Integer = &H120D
        Public Const DTWAIN_EI_VERTLINEXCOORD As Integer = &H120E
        Public Const DTWAIN_EI_VERTLINEYCOORD As Integer = &H120F
        Public Const DTWAIN_EI_VERTLINELENGTH As Integer = &H1210
        Public Const DTWAIN_EI_VERTLINETHICKNESS As Integer = &H1211
        Public Const DTWAIN_EI_PATCHCODE As Integer = &H1212
        Public Const DTWAIN_EI_ENDORSEDTEXT As Integer = &H1213
        Public Const DTWAIN_EI_FORMCONFIDENCE As Integer = &H1214
        Public Const DTWAIN_EI_FORMTEMPLATEMATCH As Integer = &H1215
        Public Const DTWAIN_EI_FORMTEMPLATEPAGEMATCH As Integer = &H1216
        Public Const DTWAIN_EI_FORMHORZDOCOFFSET As Integer = &H1217
        Public Const DTWAIN_EI_FORMVERTDOCOFFSET As Integer = &H1218
        Public Const DTWAIN_EI_BARCODECOUNT As Integer = &H1219
        Public Const DTWAIN_EI_BARCODECONFIDENCE As Integer = &H121A
        Public Const DTWAIN_EI_BARCODEROTATION As Integer = &H121B
        Public Const DTWAIN_EI_BARCODETEXTLENGTH As Integer = &H121C
        Public Const DTWAIN_EI_DESHADECOUNT As Integer = &H121D
        Public Const DTWAIN_EI_DESHADEBLACKCOUNTOLD As Integer = &H121E
        Public Const DTWAIN_EI_DESHADEBLACKCOUNTNEW As Integer = &H121F
        Public Const DTWAIN_EI_DESHADEBLACKRLMIN As Integer = &H1220
        Public Const DTWAIN_EI_DESHADEBLACKRLMAX As Integer = &H1221
        Public Const DTWAIN_EI_DESHADEWHITECOUNTOLD As Integer = &H1222
        Public Const DTWAIN_EI_DESHADEWHITECOUNTNEW As Integer = &H1223
        Public Const DTWAIN_EI_DESHADEWHITERLMIN As Integer = &H1224
        Public Const DTWAIN_EI_DESHADEWHITERLAVE As Integer = &H1225
        Public Const DTWAIN_EI_DESHADEWHITERLMAX As Integer = &H1226
        Public Const DTWAIN_EI_BLACKSPECKLESREMOVED As Integer = &H1227
        Public Const DTWAIN_EI_WHITESPECKLESREMOVED As Integer = &H1228
        Public Const DTWAIN_EI_HORZLINECOUNT As Integer = &H1229
        Public Const DTWAIN_EI_VERTLINECOUNT As Integer = &H122A
        Public Const DTWAIN_EI_DESKEWSTATUS As Integer = &H122B
        Public Const DTWAIN_EI_SKEWORIGINALANGLE As Integer = &H122C
        Public Const DTWAIN_EI_SKEWFINALANGLE As Integer = &H122D
        Public Const DTWAIN_EI_SKEWCONFIDENCE As Integer = &H122E
        Public Const DTWAIN_EI_SKEWWINDOWX1 As Integer = &H122F
        Public Const DTWAIN_EI_SKEWWINDOWY1 As Integer = &H1230
        Public Const DTWAIN_EI_SKEWWINDOWX2 As Integer = &H1231
        Public Const DTWAIN_EI_SKEWWINDOWY2 As Integer = &H1232
        Public Const DTWAIN_EI_SKEWWINDOWX3 As Integer = &H1233
        Public Const DTWAIN_EI_SKEWWINDOWY3 As Integer = &H1234
        Public Const DTWAIN_EI_SKEWWINDOWX4 As Integer = &H1235
        Public Const DTWAIN_EI_SKEWWINDOWY4 As Integer = &H1236
        Public Const DTWAIN_EI_BOOKNAME As Integer = &H1238
        Public Const DTWAIN_EI_CHAPTERNUMBER As Integer = &H1239
        Public Const DTWAIN_EI_DOCUMENTNUMBER As Integer = &H123A
        Public Const DTWAIN_EI_PAGENUMBER As Integer = &H123B
        Public Const DTWAIN_EI_CAMERA As Integer = &H123C
        Public Const DTWAIN_EI_FRAMENUMBER As Integer = &H123D
        Public Const DTWAIN_EI_FRAME As Integer = &H123E
        Public Const DTWAIN_EI_PIXELFLAVOR As Integer = &H123F
        Public Const DTWAIN_EI_ICCPROFILE As Integer = &H1240
        Public Const DTWAIN_EI_LASTSEGMENT As Integer = &H1241
        Public Const DTWAIN_EI_SEGMENTNUMBER As Integer = &H1242
        Public Const DTWAIN_EI_MAGDATA As Integer = &H1243
        Public Const DTWAIN_EI_MAGTYPE As Integer = &H1244
        Public Const DTWAIN_EI_PAGESIDE As Integer = &H1245
        Public Const DTWAIN_EI_FILESYSTEMSOURCE As Integer = &H1246
        Public Const DTWAIN_EI_IMAGEMERGED As Integer = &H1247
        Public Const DTWAIN_EI_MAGDATALENGTH As Integer = &H1248
        Public Const DTWAIN_EI_PAPERCOUNT As Integer = &H1249
        Public Const DTWAIN_EI_PRINTERTEXT As Integer = &H124A
        Public Const DTWAIN_EI_TWAINDIRECTMETADATA As Integer = &H124B
        Public Const DTWAIN_EI_IAFIELDA_VALUE As Integer = &H124C
        Public Const DTWAIN_EI_IAFIELDB_VALUE As Integer = &H124D
        Public Const DTWAIN_EI_IAFIELDC_VALUE As Integer = &H124E
        Public Const DTWAIN_EI_IAFIELDD_VALUE As Integer = &H124F
        Public Const DTWAIN_EI_IAFIELDE_VALUE As Integer = &H1250
        Public Const DTWAIN_EI_IALEVEL As Integer = &H1251
        Public Const DTWAIN_EI_PRINTER As Integer = &H1252
        Public Const DTWAIN_EI_BARCODETEXT2 As Integer = &H1253
        Public Const DTWAIN_LOG_DECODE_SOURCE As UInteger = &H1
        Public Const DTWAIN_LOG_DECODE_DEST As UInteger = &H2
        Public Const DTWAIN_LOG_DECODE_TWMEMREF As UInteger = &H4
        Public Const DTWAIN_LOG_DECODE_TWEVENT As UInteger = &H8
        Public Const DTWAIN_LOG_CALLSTACK As UInteger = &H10
        Public Const DTWAIN_LOG_ISTWAINMSG As UInteger = &H20
        Public Const DTWAIN_LOG_INITFAILURE As UInteger = &H40
        Public Const DTWAIN_LOG_LOWLEVELTWAIN As UInteger = &H80
        Public Const DTWAIN_LOG_DECODE_BITMAP As UInteger = &H100
        Public Const DTWAIN_LOG_NOTIFICATIONS As UInteger = &H200
        Public Const DTWAIN_LOG_MISCELLANEOUS As UInteger = &H400
        Public Const DTWAIN_LOG_DTWAINERRORS As UInteger = &H800
        Public Const DTWAIN_LOG_USEFILE As UInteger = &H10000
        Public Const DTWAIN_LOG_SHOWEXCEPTIONS As UInteger = &H20000
        Public Const DTWAIN_LOG_ERRORMSGBOX As UInteger = &H40000
        Public Const DTWAIN_LOG_USEBUFFER As UInteger = &H80000
        Public Const DTWAIN_LOG_FILEAPPEND As UInteger = &H100000
        Public Const DTWAIN_LOG_USECALLBACK As UInteger = &H200000
        Public Const DTWAIN_LOG_USECRLF As UInteger = &H400000
        Public Const DTWAIN_LOG_CONSOLE As UInteger = &H800000
        Public Const DTWAIN_LOG_DEBUGMONITOR As UInteger = &H1000000
        Public Const DTWAIN_LOG_USEWINDOW As UInteger = &H2000000
        Public Const DTWAIN_LOG_CREATEDIRECTORY As UInteger = &H04000000
        Public Const DTWAIN_LOG_CONSOLEWITHHANDLER As UInteger = (&H08000000 Or DTWAIN_LOG_CONSOLE)
        Public Const DTWAIN_LOG_ALL As UInteger = (DTWAIN_LOG_DECODE_SOURCE Or DTWAIN_LOG_DECODE_DEST Or DTWAIN_LOG_DECODE_TWEVENT Or DTWAIN_LOG_DECODE_TWMEMREF Or DTWAIN_LOG_CALLSTACK Or DTWAIN_LOG_ISTWAINMSG Or DTWAIN_LOG_INITFAILURE Or DTWAIN_LOG_LOWLEVELTWAIN Or DTWAIN_LOG_NOTIFICATIONS Or DTWAIN_LOG_MISCELLANEOUS Or DTWAIN_LOG_DTWAINERRORS Or DTWAIN_LOG_DECODE_BITMAP)
        Public Const DTWAIN_LOG_ALL_APPEND As UInteger = &HFFFFFFFFUI
        Public Const DTWAIN_TEMPDIR_CREATEDIRECTORY As UInteger = DTWAIN_LOG_CREATEDIRECTORY
        Public Const DTWAINGCD_RETURNHANDLE As Integer = 1
        Public Const DTWAINGCD_COPYDATA As Integer = 2
        Public Const DTWAIN_BYPOSITION As Integer = 0
        Public Const DTWAIN_BYID As Integer = 1
        Public Const DTWAINSCD_USEHANDLE As Integer = 1
        Public Const DTWAINSCD_USEDATA As Integer = 2
        Public Const DTWAIN_PAGEFAIL_RETRY As Integer = 1
        Public Const DTWAIN_PAGEFAIL_TERMINATE As Integer = 2
        Public Const DTWAIN_MAXRETRY_ATTEMPTS As Integer = 3
        Public Const DTWAIN_RETRY_FOREVER As Integer = (-1)
        Public Const DTWAIN_PDF_NOSCALING As Integer = 128
        Public Const DTWAIN_PDF_FITPAGE As Integer = 256
        Public Const DTWAIN_PDF_VARIABLEPAGESIZE As Integer = 512
        Public Const DTWAIN_PDF_CUSTOMSIZE As Integer = 1024
        Public Const DTWAIN_PDF_USECOMPRESSION As Integer = 2048
        Public Const DTWAIN_PDF_CUSTOMSCALE As Integer = 4096
        Public Const DTWAIN_PDF_PIXELSPERMETERSIZE As Integer = 8192
        Public Const DTWAIN_PDF_ALLOWPRINTING As UInteger = 2052
        Public Const DTWAIN_PDF_ALLOWMOD As UInteger = 8
        Public Const DTWAIN_PDF_ALLOWCOPY As UInteger = 16
        Public Const DTWAIN_PDF_ALLOWMODANNOTATIONS As UInteger = 32
        Public Const DTWAIN_PDF_ALLOWFILLIN As UInteger = 256
        Public Const DTWAIN_PDF_ALLOWEXTRACTION As UInteger = 512
        Public Const DTWAIN_PDF_ALLOWASSEMBLY As UInteger = 1024
        Public Const DTWAIN_PDF_ALLOWDEGRADEDPRINTING As UInteger = 4
        Public Const DTWAIN_PDF_ALLOWALL As UInteger = &HFFFFFFFCUI
        Public Const DTWAIN_PDF_ALLOWANYMOD As UInteger = (DTWAIN_PDF_ALLOWMOD Or DTWAIN_PDF_ALLOWFILLIN Or DTWAIN_PDF_ALLOWMODANNOTATIONS Or DTWAIN_PDF_ALLOWASSEMBLY)
        Public Const DTWAIN_PDF_ALLOWANYPRINTING As UInteger = (DTWAIN_PDF_ALLOWPRINTING Or DTWAIN_PDF_ALLOWDEGRADEDPRINTING)
        Public Const DTWAIN_PDF_PORTRAIT As Integer = 0
        Public Const DTWAIN_PDF_LANDSCAPE As Integer = 1
        Public Const DTWAIN_PS_REGULAR As Integer = 0
        Public Const DTWAIN_PS_ENCAPSULATED As Integer = 1
        Public Const DTWAIN_BP_AUTODISCARD_NONE As Integer = 0
        Public Const DTWAIN_BP_AUTODISCARD_IMMEDIATE As Integer = 1
        Public Const DTWAIN_BP_AUTODISCARD_AFTERPROCESS As Integer = 2
        Public Const DTWAIN_BP_DETECTORIGINAL As Integer = 1
        Public Const DTWAIN_BP_DETECTADJUSTED As Integer = 2
        Public Const DTWAIN_BP_DETECTALL As Integer = (DTWAIN_BP_DETECTORIGINAL Or DTWAIN_BP_DETECTADJUSTED)
        Public Const DTWAIN_BP_DISABLE As Integer = (-2)
        Public Const DTWAIN_BP_AUTO As Integer = (-1)
        Public Const DTWAIN_BP_AUTODISCARD_ANY As UInteger = &HFFFF
        Public Const DTWAIN_LP_REFLECTIVE As Integer = 0
        Public Const DTWAIN_LP_TRANSMISSIVE As Integer = 1
        Public Const DTWAIN_LS_RED As Integer = 0
        Public Const DTWAIN_LS_GREEN As Integer = 1
        Public Const DTWAIN_LS_BLUE As Integer = 2
        Public Const DTWAIN_LS_NONE As Integer = 3
        Public Const DTWAIN_LS_WHITE As Integer = 4
        Public Const DTWAIN_LS_UV As Integer = 5
        Public Const DTWAIN_LS_IR As Integer = 6
        Public Const DTWAIN_DLG_SORTNAMES As Integer = 1
        Public Const DTWAIN_DLG_CENTER As Integer = 2
        Public Const DTWAIN_DLG_CENTER_SCREEN As Integer = 4
        Public Const DTWAIN_DLG_USETEMPLATE As Integer = 8
        Public Const DTWAIN_DLG_CLEAR_PARAMS As Integer = 16
        Public Const DTWAIN_DLG_HORIZONTALSCROLL As Integer = 32
        Public Const DTWAIN_DLG_USEINCLUDENAMES As Integer = 64
        Public Const DTWAIN_DLG_USEEXCLUDENAMES As Integer = 128
        Public Const DTWAIN_DLG_USENAMEMAPPING As Integer = 256
        Public Const DTWAIN_DLG_TOPMOSTWINDOW As Integer = 1024
        Public Const DTWAIN_DLG_OPENONSELECT As Integer = 2048
        Public Const DTWAIN_DLG_NOOPENONSELECT As Integer = 4096
        Public Const DTWAIN_DLG_HIGHLIGHTFIRST As Integer = 8192
        Public Const DTWAIN_DLG_SAVELASTSCREENPOS As Integer = 16384
        Public Const DTWAIN_RES_ENGLISH As Integer = 0
        Public Const DTWAIN_RES_FRENCH As Integer = 1
        Public Const DTWAIN_RES_SPANISH As Integer = 2
        Public Const DTWAIN_RES_DUTCH As Integer = 3
        Public Const DTWAIN_RES_GERMAN As Integer = 4
        Public Const DTWAIN_RES_ITALIAN As Integer = 5
        Public Const DTWAIN_AL_ALARM As Integer = 0
        Public Const DTWAIN_AL_FEEDERERROR As Integer = 1
        Public Const DTWAIN_AL_FEEDERWARNING As Integer = 2
        Public Const DTWAIN_AL_BARCODE As Integer = 3
        Public Const DTWAIN_AL_DOUBLEFEED As Integer = 4
        Public Const DTWAIN_AL_JAM As Integer = 5
        Public Const DTWAIN_AL_PATCHCODE As Integer = 6
        Public Const DTWAIN_AL_POWER As Integer = 7
        Public Const DTWAIN_AL_SKEW As Integer = 8
        Public Const DTWAIN_FT_CAMERA As Integer = 0
        Public Const DTWAIN_FT_CAMERATOP As Integer = 1
        Public Const DTWAIN_FT_CAMERABOTTOM As Integer = 2
        Public Const DTWAIN_FT_CAMERAPREVIEW As Integer = 3
        Public Const DTWAIN_FT_DOMAIN As Integer = 4
        Public Const DTWAIN_FT_HOST As Integer = 5
        Public Const DTWAIN_FT_DIRECTORY As Integer = 6
        Public Const DTWAIN_FT_IMAGE As Integer = 7
        Public Const DTWAIN_FT_UNKNOWN As Integer = 8
        Public Const DTWAIN_NF_NONE As Integer = 0
        Public Const DTWAIN_NF_AUTO As Integer = 1
        Public Const DTWAIN_NF_LONEPIXEL As Integer = 2
        Public Const DTWAIN_NF_MAJORITYRULE As Integer = 3
        Public Const DTWAIN_CB_AUTO As Integer = 0
        Public Const DTWAIN_CB_CLEAR As Integer = 1
        Public Const DTWAIN_CB_NOCLEAR As Integer = 2
        Public Const DTWAIN_FA_NONE As Integer = 0
        Public Const DTWAIN_FA_LEFT As Integer = 1
        Public Const DTWAIN_FA_CENTER As Integer = 2
        Public Const DTWAIN_FA_RIGHT As Integer = 3
        Public Const DTWAIN_PF_CHOCOLATE As Integer = 0
        Public Const DTWAIN_PF_VANILLA As Integer = 1
        Public Const DTWAIN_FO_FIRSTPAGEFIRST As Integer = 0
        Public Const DTWAIN_FO_LASTPAGEFIRST As Integer = 1
        Public Const DTWAIN_INCREMENT_STATIC As Integer = 0
        Public Const DTWAIN_INCREMENT_DYNAMIC As Integer = 1
        Public Const DTWAIN_INCREMENT_DEFAULT As Integer = -1
        Public Const DTWAIN_MANDUP_FACEUPTOPPAGE As Integer = 0
        Public Const DTWAIN_MANDUP_FACEUPBOTTOMPAGE As Integer = 1
        Public Const DTWAIN_MANDUP_FACEDOWNTOPPAGE As Integer = 2
        Public Const DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE As Integer = 3
        Public Const DTWAIN_FILESAVE_DEFAULT As Integer = 0
        Public Const DTWAIN_FILESAVE_UICLOSE As Integer = 1
        Public Const DTWAIN_FILESAVE_SOURCECLOSE As Integer = 2
        Public Const DTWAIN_FILESAVE_ENDACQUIRE As Integer = 3
        Public Const DTWAIN_FILESAVE_MANUALSAVE As Integer = 4
        Public Const DTWAIN_FILESAVE_SAVEINCOMPLETE As Integer = 128
        Public Const DTWAIN_MANDUP_SCANOK As Integer = 1
        Public Const DTWAIN_MANDUP_SIDE1RESCAN As Integer = 2
        Public Const DTWAIN_MANDUP_SIDE2RESCAN As Integer = 3
        Public Const DTWAIN_MANDUP_RESCANALL As Integer = 4
        Public Const DTWAIN_MANDUP_PAGEMISSING As Integer = 5
        Public Const DTWAIN_DEMODLL_VERSION As Integer = &H00000001
        Public Const DTWAIN_UNLICENSED_VERSION As Integer = &H00000002
        Public Const DTWAIN_COMPANY_VERSION As Integer = &H00000004
        Public Const DTWAIN_GENERAL_VERSION As Integer = &H00000008
        Public Const DTWAIN_DEVELOP_VERSION As Integer = &H00000010
        Public Const DTWAIN_JAVA_VERSION As Integer = &H00000020
        Public Const DTWAIN_TOOLKIT_VERSION As Integer = &H00000040
        Public Const DTWAIN_LIMITEDDLL_VERSION As Integer = &H00000080
        Public Const DTWAIN_STATICLIB_VERSION As Integer = &H00000100
        Public Const DTWAIN_STATICLIB_STDCALL_VERSION As Integer = &H00000200
        Public Const DTWAIN_PDF_VERSION As Integer = &H00010000
        Public Const DTWAIN_TWAINSAVE_VERSION As Integer = &H00020000
        Public Const DTWAIN_OCR_VERSION As Integer = &H00040000
        Public Const DTWAIN_BARCODE_VERSION As Integer = &H00080000
        Public Const DTWAIN_ACTIVEX_VERSION As Integer = &H00100000
        Public Const DTWAIN_32BIT_VERSION As Integer = &H00200000
        Public Const DTWAIN_64BIT_VERSION As Integer = &H00400000
        Public Const DTWAIN_UNICODE_VERSION As Integer = &H00800000
        Public Const DTWAIN_OPENSOURCE_VERSION As Integer = &H01000000
        Public Const DTWAIN_CALLSTACK_LOGGING As Integer = &H02000000
        Public Const DTWAIN_CALLSTACK_LOGGING_PLUS As Integer = &H04000000
        Public Const DTWAINOCR_RETURNHANDLE As Integer = 1
        Public Const DTWAINOCR_COPYDATA As Integer = 2
        Public Const DTWAIN_OCRINFO_CHAR As Integer = 0
        Public Const DTWAIN_OCRINFO_CHARXPOS As Integer = 1
        Public Const DTWAIN_OCRINFO_CHARYPOS As Integer = 2
        Public Const DTWAIN_OCRINFO_CHARXWIDTH As Integer = 3
        Public Const DTWAIN_OCRINFO_CHARYWIDTH As Integer = 4
        Public Const DTWAIN_OCRINFO_CHARCONFIDENCE As Integer = 5
        Public Const DTWAIN_OCRINFO_PAGENUM As Integer = 6
        Public Const DTWAIN_OCRINFO_OCRENGINE As Integer = 7
        Public Const DTWAIN_OCRINFO_TEXTLENGTH As Integer = 8
        Public Const DTWAIN_PDFPAGETYPE_COLOR As Integer = 0
        Public Const DTWAIN_PDFPAGETYPE_BW As Integer = 1
        Public Const DTWAIN_TWAINDSM_LEGACY As Integer = 1
        Public Const DTWAIN_TWAINDSM_VERSION2 As Integer = 2
        Public Const DTWAIN_TWAINDSM_LATESTVERSION As Integer = 4
        Public Const DTWAIN_TWAINDSMSEARCH_NOTFOUND As Integer = (-1)
        Public Const DTWAIN_TWAINDSMSEARCH_WSO As Integer = 0
        Public Const DTWAIN_TWAINDSMSEARCH_WOS As Integer = 1
        Public Const DTWAIN_TWAINDSMSEARCH_SWO As Integer = 2
        Public Const DTWAIN_TWAINDSMSEARCH_SOW As Integer = 3
        Public Const DTWAIN_TWAINDSMSEARCH_OWS As Integer = 4
        Public Const DTWAIN_TWAINDSMSEARCH_OSW As Integer = 5
        Public Const DTWAIN_TWAINDSMSEARCH_W As Integer = 6
        Public Const DTWAIN_TWAINDSMSEARCH_S As Integer = 7
        Public Const DTWAIN_TWAINDSMSEARCH_O As Integer = 8
        Public Const DTWAIN_TWAINDSMSEARCH_WS As Integer = 9
        Public Const DTWAIN_TWAINDSMSEARCH_WO As Integer = 10
        Public Const DTWAIN_TWAINDSMSEARCH_SW As Integer = 11
        Public Const DTWAIN_TWAINDSMSEARCH_SO As Integer = 12
        Public Const DTWAIN_TWAINDSMSEARCH_OW As Integer = 13
        Public Const DTWAIN_TWAINDSMSEARCH_OS As Integer = 14
        Public Const DTWAIN_TWAINDSMSEARCH_C As Integer = 15
        Public Const DTWAIN_TWAINDSMSEARCH_U As Integer = 16
        Public Const DTWAIN_PDFPOLARITY_POSITIVE As Integer = 1
        Public Const DTWAIN_PDFPOLARITY_NEGATIVE As Integer = 2
        Public Const DTWAIN_TWPF_NORMAL As Integer = 0
        Public Const DTWAIN_TWPF_BOLD As Integer = 1
        Public Const DTWAIN_TWPF_ITALIC As Integer = 2
        Public Const DTWAIN_TWPF_LARGESIZE As Integer = 3
        Public Const DTWAIN_TWPF_SMALLSIZE As Integer = 4
        Public Const DTWAIN_TWCT_PAGE As Integer = 0
        Public Const DTWAIN_TWCT_PATCH1 As Integer = 1
        Public Const DTWAIN_TWCT_PATCH2 As Integer = 2
        Public Const DTWAIN_TWCT_PATCH3 As Integer = 3
        Public Const DTWAIN_TWCT_PATCH4 As Integer = 4
        Public Const DTWAIN_TWCT_PATCH5 As Integer = 5
        Public Const DTWAIN_TWCT_PATCH6 As Integer = 6
        Public Const DTWAIN_AUTOSIZE_NONE As Integer = 0
        Public Const DTWAIN_CV_CAPCUSTOMBASE As Integer = &H8000
        Public Const DTWAIN_CV_CAPXFERCOUNT As Integer = &H0001
        Public Const DTWAIN_CV_ICAPCOMPRESSION As Integer = &H0100
        Public Const DTWAIN_CV_ICAPPIXELTYPE As Integer = &H0101
        Public Const DTWAIN_CV_ICAPUNITS As Integer = &H0102
        Public Const DTWAIN_CV_ICAPXFERMECH As Integer = &H0103
        Public Const DTWAIN_CV_CAPAUTHOR As Integer = &H1000
        Public Const DTWAIN_CV_CAPCAPTION As Integer = &H1001
        Public Const DTWAIN_CV_CAPFEEDERENABLED As Integer = &H1002
        Public Const DTWAIN_CV_CAPFEEDERLOADED As Integer = &H1003
        Public Const DTWAIN_CV_CAPTIMEDATE As Integer = &H1004
        Public Const DTWAIN_CV_CAPSUPPORTEDCAPS As Integer = &H1005
        Public Const DTWAIN_CV_CAPEXTENDEDCAPS As Integer = &H1006
        Public Const DTWAIN_CV_CAPAUTOFEED As Integer = &H1007
        Public Const DTWAIN_CV_CAPCLEARPAGE As Integer = &H1008
        Public Const DTWAIN_CV_CAPFEEDPAGE As Integer = &H1009
        Public Const DTWAIN_CV_CAPREWINDPAGE As Integer = &H100a
        Public Const DTWAIN_CV_CAPINDICATORS As Integer = &H100b
        Public Const DTWAIN_CV_CAPSUPPORTEDCAPSEXT As Integer = &H100c
        Public Const DTWAIN_CV_CAPPAPERDETECTABLE As Integer = &H100d
        Public Const DTWAIN_CV_CAPUICONTROLLABLE As Integer = &H100e
        Public Const DTWAIN_CV_CAPDEVICEONLINE As Integer = &H100f
        Public Const DTWAIN_CV_CAPAUTOSCAN As Integer = &H1010
        Public Const DTWAIN_CV_CAPTHUMBNAILSENABLED As Integer = &H1011
        Public Const DTWAIN_CV_CAPDUPLEX As Integer = &H1012
        Public Const DTWAIN_CV_CAPDUPLEXENABLED As Integer = &H1013
        Public Const DTWAIN_CV_CAPENABLEDSUIONLY As Integer = &H1014
        Public Const DTWAIN_CV_CAPCUSTOMDSDATA As Integer = &H1015
        Public Const DTWAIN_CV_CAPENDORSER As Integer = &H1016
        Public Const DTWAIN_CV_CAPJOBCONTROL As Integer = &H1017
        Public Const DTWAIN_CV_CAPALARMS As Integer = &H1018
        Public Const DTWAIN_CV_CAPALARMVOLUME As Integer = &H1019
        Public Const DTWAIN_CV_CAPAUTOMATICCAPTURE As Integer = &H101a
        Public Const DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE As Integer = &H101b
        Public Const DTWAIN_CV_CAPTIMEBETWEENCAPTURES As Integer = &H101c
        Public Const DTWAIN_CV_CAPCLEARBUFFERS As Integer = &H101d
        Public Const DTWAIN_CV_CAPMAXBATCHBUFFERS As Integer = &H101e
        Public Const DTWAIN_CV_CAPDEVICETIMEDATE As Integer = &H101f
        Public Const DTWAIN_CV_CAPPOWERSUPPLY As Integer = &H1020
        Public Const DTWAIN_CV_CAPCAMERAPREVIEWUI As Integer = &H1021
        Public Const DTWAIN_CV_CAPDEVICEEVENT As Integer = &H1022
        Public Const DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE As Integer = &H1023
        Public Const DTWAIN_CV_CAPSERIALNUMBER As Integer = &H1024
        Public Const DTWAIN_CV_CAPFILESYSTEM As Integer = &H1025
        Public Const DTWAIN_CV_CAPPRINTER As Integer = &H1026
        Public Const DTWAIN_CV_CAPPRINTERENABLED As Integer = &H1027
        Public Const DTWAIN_CV_CAPPRINTERINDEX As Integer = &H1028
        Public Const DTWAIN_CV_CAPPRINTERMODE As Integer = &H1029
        Public Const DTWAIN_CV_CAPPRINTERSTRING As Integer = &H102a
        Public Const DTWAIN_CV_CAPPRINTERSUFFIX As Integer = &H102b
        Public Const DTWAIN_CV_CAPLANGUAGE As Integer = &H102c
        Public Const DTWAIN_CV_CAPFEEDERALIGNMENT As Integer = &H102d
        Public Const DTWAIN_CV_CAPFEEDERORDER As Integer = &H102e
        Public Const DTWAIN_CV_CAPPAPERBINDING As Integer = &H102f
        Public Const DTWAIN_CV_CAPREACQUIREALLOWED As Integer = &H1030
        Public Const DTWAIN_CV_CAPPASSTHRU As Integer = &H1031
        Public Const DTWAIN_CV_CAPBATTERYMINUTES As Integer = &H1032
        Public Const DTWAIN_CV_CAPBATTERYPERCENTAGE As Integer = &H1033
        Public Const DTWAIN_CV_CAPPOWERDOWNTIME As Integer = &H1034
        Public Const DTWAIN_CV_CAPSEGMENTED As Integer = &H1035
        Public Const DTWAIN_CV_CAPCAMERAENABLED As Integer = &H1036
        Public Const DTWAIN_CV_CAPCAMERAORDER As Integer = &H1037
        Public Const DTWAIN_CV_CAPMICRENABLED As Integer = &H1038
        Public Const DTWAIN_CV_CAPFEEDERPREP As Integer = &H1039
        Public Const DTWAIN_CV_CAPFEEDERPOCKET As Integer = &H103a
        Public Const DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM As Integer = &H103b
        Public Const DTWAIN_CV_CAPCUSTOMINTERFACEGUID As Integer = &H103c
        Public Const DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE As Integer = &H103d
        Public Const DTWAIN_CV_CAPSUPPORTEDDATS As Integer = &H103e
        Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTION As Integer = &H103f
        Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH As Integer = &H1040
        Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY As Integer = &H1041
        Public Const DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE As Integer = &H1042
        Public Const DTWAIN_CV_CAPPAPERHANDLING As Integer = &H1043
        Public Const DTWAIN_CV_CAPINDICATORSMODE As Integer = &H1044
        Public Const DTWAIN_CV_CAPPRINTERVERTICALOFFSET As Integer = &H1045
        Public Const DTWAIN_CV_CAPPOWERSAVETIME As Integer = &H1046
        Public Const DTWAIN_CV_CAPPRINTERCHARROTATION As Integer = &H1047
        Public Const DTWAIN_CV_CAPPRINTERFONTSTYLE As Integer = &H1048
        Public Const DTWAIN_CV_CAPPRINTERINDEXLEADCHAR As Integer = &H1049
        Public Const DTWAIN_CV_CAPIMAGEADDRESSENABLED As Integer = &H1050
        Public Const DTWAIN_CV_CAPIAFIELDA_LEVEL As Integer = &H1051
        Public Const DTWAIN_CV_CAPIAFIELDB_LEVEL As Integer = &H1052
        Public Const DTWAIN_CV_CAPIAFIELDC_LEVEL As Integer = &H1053
        Public Const DTWAIN_CV_CAPIAFIELDD_LEVEL As Integer = &H1054
        Public Const DTWAIN_CV_CAPIAFIELDE_LEVEL As Integer = &H1055
        Public Const DTWAIN_CV_CAPIAFIELDA_PRINTFORMAT As Integer = &H1056
        Public Const DTWAIN_CV_CAPIAFIELDB_PRINTFORMAT As Integer = &H1057
        Public Const DTWAIN_CV_CAPIAFIELDC_PRINTFORMAT As Integer = &H1058
        Public Const DTWAIN_CV_CAPIAFIELDD_PRINTFORMAT As Integer = &H1059
        Public Const DTWAIN_CV_CAPIAFIELDE_PRINTFORMAT As Integer = &H105A
        Public Const DTWAIN_CV_CAPIAFIELDA_VALUE As Integer = &H105B
        Public Const DTWAIN_CV_CAPIAFIELDB_VALUE As Integer = &H105C
        Public Const DTWAIN_CV_CAPIAFIELDC_VALUE As Integer = &H105D
        Public Const DTWAIN_CV_CAPIAFIELDD_VALUE As Integer = &H105E
        Public Const DTWAIN_CV_CAPIAFIELDE_VALUE As Integer = &H105F
        Public Const DTWAIN_CV_CAPIAFIELDA_LASTPAGE As Integer = &H1060
        Public Const DTWAIN_CV_CAPIAFIELDB_LASTPAGE As Integer = &H1061
        Public Const DTWAIN_CV_CAPIAFIELDC_LASTPAGE As Integer = &H1062
        Public Const DTWAIN_CV_CAPIAFIELDD_LASTPAGE As Integer = &H1063
        Public Const DTWAIN_CV_CAPIAFIELDE_LASTPAGE As Integer = &H1064
        Public Const DTWAIN_CV_CAPPRINTERINDEXMAXVALUE As Integer = &H104A
        Public Const DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS As Integer = &H104B
        Public Const DTWAIN_CV_CAPPRINTERINDEXSTEP As Integer = &H104C
        Public Const DTWAIN_CV_CAPPRINTERINDEXTRIGGER As Integer = &H104D
        Public Const DTWAIN_CV_CAPPRINTERSTRINGPREVIEW As Integer = &H104E
        Public Const DTWAIN_CV_ICAPAUTOBRIGHT As Integer = &H1100
        Public Const DTWAIN_CV_ICAPBRIGHTNESS As Integer = &H1101
        Public Const DTWAIN_CV_ICAPCONTRAST As Integer = &H1103
        Public Const DTWAIN_CV_ICAPCUSTHALFTONE As Integer = &H1104
        Public Const DTWAIN_CV_ICAPEXPOSURETIME As Integer = &H1105
        Public Const DTWAIN_CV_ICAPFILTER As Integer = &H1106
        Public Const DTWAIN_CV_ICAPFLASHUSED As Integer = &H1107
        Public Const DTWAIN_CV_ICAPGAMMA As Integer = &H1108
        Public Const DTWAIN_CV_ICAPHALFTONES As Integer = &H1109
        Public Const DTWAIN_CV_ICAPHIGHLIGHT As Integer = &H110a
        Public Const DTWAIN_CV_ICAPIMAGEFILEFORMAT As Integer = &H110c
        Public Const DTWAIN_CV_ICAPLAMPSTATE As Integer = &H110d
        Public Const DTWAIN_CV_ICAPLIGHTSOURCE As Integer = &H110e
        Public Const DTWAIN_CV_ICAPORIENTATION As Integer = &H1110
        Public Const DTWAIN_CV_ICAPPHYSICALWIDTH As Integer = &H1111
        Public Const DTWAIN_CV_ICAPPHYSICALHEIGHT As Integer = &H1112
        Public Const DTWAIN_CV_ICAPSHADOW As Integer = &H1113
        Public Const DTWAIN_CV_ICAPFRAMES As Integer = &H1114
        Public Const DTWAIN_CV_ICAPXNATIVERESOLUTION As Integer = &H1116
        Public Const DTWAIN_CV_ICAPYNATIVERESOLUTION As Integer = &H1117
        Public Const DTWAIN_CV_ICAPXRESOLUTION As Integer = &H1118
        Public Const DTWAIN_CV_ICAPYRESOLUTION As Integer = &H1119
        Public Const DTWAIN_CV_ICAPMAXFRAMES As Integer = &H111a
        Public Const DTWAIN_CV_ICAPTILES As Integer = &H111b
        Public Const DTWAIN_CV_ICAPBITORDER As Integer = &H111c
        Public Const DTWAIN_CV_ICAPCCITTKFACTOR As Integer = &H111d
        Public Const DTWAIN_CV_ICAPLIGHTPATH As Integer = &H111e
        Public Const DTWAIN_CV_ICAPPIXELFLAVOR As Integer = &H111f
        Public Const DTWAIN_CV_ICAPPLANARCHUNKY As Integer = &H1120
        Public Const DTWAIN_CV_ICAPROTATION As Integer = &H1121
        Public Const DTWAIN_CV_ICAPSUPPORTEDSIZES As Integer = &H1122
        Public Const DTWAIN_CV_ICAPTHRESHOLD As Integer = &H1123
        Public Const DTWAIN_CV_ICAPXSCALING As Integer = &H1124
        Public Const DTWAIN_CV_ICAPYSCALING As Integer = &H1125
        Public Const DTWAIN_CV_ICAPBITORDERCODES As Integer = &H1126
        Public Const DTWAIN_CV_ICAPPIXELFLAVORCODES As Integer = &H1127
        Public Const DTWAIN_CV_ICAPJPEGPIXELTYPE As Integer = &H1128
        Public Const DTWAIN_CV_ICAPTIMEFILL As Integer = &H112a
        Public Const DTWAIN_CV_ICAPBITDEPTH As Integer = &H112b
        Public Const DTWAIN_CV_ICAPBITDEPTHREDUCTION As Integer = &H112c
        Public Const DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE As Integer = &H112d
        Public Const DTWAIN_CV_ICAPIMAGEDATASET As Integer = &H112e
        Public Const DTWAIN_CV_ICAPEXTIMAGEINFO As Integer = &H112f
        Public Const DTWAIN_CV_ICAPMINIMUMHEIGHT As Integer = &H1130
        Public Const DTWAIN_CV_ICAPMINIMUMWIDTH As Integer = &H1131
        Public Const DTWAIN_CV_ICAPAUTOBORDERDETECTION As Integer = &H1132
        Public Const DTWAIN_CV_ICAPAUTODESKEW As Integer = &H1133
        Public Const DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES As Integer = &H1134
        Public Const DTWAIN_CV_ICAPAUTOROTATE As Integer = &H1135
        Public Const DTWAIN_CV_ICAPFLIPROTATION As Integer = &H1136
        Public Const DTWAIN_CV_ICAPBARCODEDETECTIONENABLED As Integer = &H1137
        Public Const DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES As Integer = &H1138
        Public Const DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES As Integer = &H1139
        Public Const DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES As Integer = &H113a
        Public Const DTWAIN_CV_ICAPBARCODESEARCHMODE As Integer = &H113b
        Public Const DTWAIN_CV_ICAPBARCODEMAXRETRIES As Integer = &H113c
        Public Const DTWAIN_CV_ICAPBARCODETIMEOUT As Integer = &H113d
        Public Const DTWAIN_CV_ICAPZOOMFACTOR As Integer = &H113e
        Public Const DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED As Integer = &H113f
        Public Const DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES As Integer = &H1140
        Public Const DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES As Integer = &H1141
        Public Const DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES As Integer = &H1142
        Public Const DTWAIN_CV_ICAPPATCHCODESEARCHMODE As Integer = &H1143
        Public Const DTWAIN_CV_ICAPPATCHCODEMAXRETRIES As Integer = &H1144
        Public Const DTWAIN_CV_ICAPPATCHCODETIMEOUT As Integer = &H1145
        Public Const DTWAIN_CV_ICAPFLASHUSED2 As Integer = &H1146
        Public Const DTWAIN_CV_ICAPIMAGEFILTER As Integer = &H1147
        Public Const DTWAIN_CV_ICAPNOISEFILTER As Integer = &H1148
        Public Const DTWAIN_CV_ICAPOVERSCAN As Integer = &H1149
        Public Const DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION As Integer = &H1150
        Public Const DTWAIN_CV_ICAPAUTOMATICDESKEW As Integer = &H1151
        Public Const DTWAIN_CV_ICAPAUTOMATICROTATE As Integer = &H1152
        Public Const DTWAIN_CV_ICAPJPEGQUALITY As Integer = &H1153
        Public Const DTWAIN_CV_ICAPFEEDERTYPE As Integer = &H1154
        Public Const DTWAIN_CV_ICAPICCPROFILE As Integer = &H1155
        Public Const DTWAIN_CV_ICAPAUTOSIZE As Integer = &H1156
        Public Const DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME As Integer = &H1157
        Public Const DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION As Integer = &H1158
        Public Const DTWAIN_CV_ICAPAUTOMATICCOLORENABLED As Integer = &H1159
        Public Const DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE As Integer = &H115a
        Public Const DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED As Integer = &H115b
        Public Const DTWAIN_CV_ICAPIMAGEMERGE As Integer = &H115c
        Public Const DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD As Integer = &H115d
        Public Const DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO As Integer = &H115e
        Public Const DTWAIN_CV_ICAPFILMTYPE As Integer = &H115f
        Public Const DTWAIN_CV_ICAPMIRROR As Integer = &H1160
        Public Const DTWAIN_CV_ICAPJPEGSUBSAMPLING As Integer = &H1161
        Public Const DTWAIN_CV_ACAPAUDIOFILEFORMAT As Integer = &H1201
        Public Const DTWAIN_CV_ACAPXFERMECH As Integer = &H1202
        Public Const DTWAIN_CFMCV_CAPCFMSTART As Integer = 2048
        Public Const DTWAIN_CFMCV_CAPDUPLEXSCANNER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+10)
        Public Const DTWAIN_CFMCV_CAPDUPLEXENABLE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+11)
        Public Const DTWAIN_CFMCV_CAPSCANNERNAME As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+12)
        Public Const DTWAIN_CFMCV_CAPSINGLEPASS As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+13)
        Public Const DTWAIN_CFMCV_CAPERRHANDLING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+20)
        Public Const DTWAIN_CFMCV_CAPFEEDERSTATUS As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+21)
        Public Const DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+22)
        Public Const DTWAIN_CFMCV_CAPFEEDWAITTIME As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+23)
        Public Const DTWAIN_CFMCV_ICAPWHITEBALANCE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+24)
        Public Const DTWAIN_CFMCV_ICAPAUTOBINARY As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+25)
        Public Const DTWAIN_CFMCV_ICAPIMAGESEPARATION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+26)
        Public Const DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+27)
        Public Const DTWAIN_CFMCV_ICAPIMAGEEMPHASIS As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+28)
        Public Const DTWAIN_CFMCV_ICAPOUTLINING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+29)
        Public Const DTWAIN_CFMCV_ICAPDYNTHRESHOLD As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+30)
        Public Const DTWAIN_CFMCV_ICAPVARIANCE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+31)
        Public Const DTWAIN_CFMCV_CAPENDORSERAVAILABLE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+32)
        Public Const DTWAIN_CFMCV_CAPENDORSERENABLE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+33)
        Public Const DTWAIN_CFMCV_CAPENDORSERCHARSET As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+34)
        Public Const DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+35)
        Public Const DTWAIN_CFMCV_CAPENDORSERSTRING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+36)
        Public Const DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+48)
        Public Const DTWAIN_CFMCV_ICAPSMOOTHINGMODE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+49)
        Public Const DTWAIN_CFMCV_ICAPFILTERMODE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+50)
        Public Const DTWAIN_CFMCV_ICAPGRADATION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+51)
        Public Const DTWAIN_CFMCV_ICAPMIRROR As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+52)
        Public Const DTWAIN_CFMCV_ICAPEASYSCANMODE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+53)
        Public Const DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+54)
        Public Const DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+55)
        Public Const DTWAIN_CFMCV_CAPDUPLEXPAGE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+56)
        Public Const DTWAIN_CFMCV_ICAPINVERTIMAGE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+57)
        Public Const DTWAIN_CFMCV_ICAPSPECKLEREMOVE As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+58)
        Public Const DTWAIN_CFMCV_ICAPUSMFILTER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+59)
        Public Const DTWAIN_CFMCV_ICAPNOISEFILTERCFM As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+60)
        Public Const DTWAIN_CFMCV_ICAPDESCREENING As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+61)
        Public Const DTWAIN_CFMCV_ICAPQUALITYFILTER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+62)
        Public Const DTWAIN_CFMCV_ICAPBINARYFILTER As Integer = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+63)
        Public Const DTWAIN_OCRCV_IMAGEFILEFORMAT As Integer = &H1000
        Public Const DTWAIN_OCRCV_DESKEW As Integer = &H1001
        Public Const DTWAIN_OCRCV_DESHADE As Integer = &H1002
        Public Const DTWAIN_OCRCV_ORIENTATION As Integer = &H1003
        Public Const DTWAIN_OCRCV_NOISEREMOVE As Integer = &H1004
        Public Const DTWAIN_OCRCV_LINEREMOVE As Integer = &H1005
        Public Const DTWAIN_OCRCV_INVERTPAGE As Integer = &H1006
        Public Const DTWAIN_OCRCV_INVERTZONES As Integer = &H1007
        Public Const DTWAIN_OCRCV_LINEREJECT As Integer = &H1008
        Public Const DTWAIN_OCRCV_CHARACTERREJECT As Integer = &H1009
        Public Const DTWAIN_OCRCV_ERRORREPORTMODE As Integer = &H1010
        Public Const DTWAIN_OCRCV_ERRORREPORTFILE As Integer = &H1011
        Public Const DTWAIN_OCRCV_PIXELTYPE As Integer = &H1012
        Public Const DTWAIN_OCRCV_BITDEPTH As Integer = &H1013
        Public Const DTWAIN_OCRCV_RETURNCHARINFO As Integer = &H1014
        Public Const DTWAIN_OCRCV_NATIVEFILEFORMAT As Integer = &H1015
        Public Const DTWAIN_OCRCV_MPNATIVEFILEFORMAT As Integer = &H1016
        Public Const DTWAIN_OCRCV_SUPPORTEDCAPS As Integer = &H1017
        Public Const DTWAIN_OCRCV_DISABLECHARACTERS As Integer = &H1018
        Public Const DTWAIN_OCRCV_REMOVECONTROLCHARS As Integer = &H1019
        Public Const DTWAIN_OCRORIENT_OFF As Integer = 0
        Public Const DTWAIN_OCRORIENT_AUTO As Integer = 1
        Public Const DTWAIN_OCRORIENT_90 As Integer = 2
        Public Const DTWAIN_OCRORIENT_180 As Integer = 3
        Public Const DTWAIN_OCRORIENT_270 As Integer = 4
        Public Const DTWAIN_OCRIMAGEFORMAT_AUTO As Integer = 10000
        Public Const DTWAIN_OCRERROR_MODENONE As Integer = 0
        Public Const DTWAIN_OCRERROR_SHOWMSGBOX As Integer = 1
        Public Const DTWAIN_OCRERROR_WRITEFILE As Integer = 2
        Public Const DTWAIN_PDFTEXT_ALLPAGES As UInteger = &H00000001UI
        Public Const DTWAIN_PDFTEXT_EVENPAGES As UInteger = &H00000002UI
        Public Const DTWAIN_PDFTEXT_ODDPAGES As UInteger = &H00000004UI
        Public Const DTWAIN_PDFTEXT_FIRSTPAGE As UInteger = &H00000008UI
        Public Const DTWAIN_PDFTEXT_LASTPAGE As UInteger = &H00000010UI
        Public Const DTWAIN_PDFTEXT_CURRENTPAGE As UInteger = &H00000020UI
        Public Const DTWAIN_PDFTEXT_DISABLED As UInteger = &H00000040UI
        Public Const DTWAIN_PDFTEXT_TOPLEFT As UInteger = &H00000100UI
        Public Const DTWAIN_PDFTEXT_TOPRIGHT As UInteger = &H00000200UI
        Public Const DTWAIN_PDFTEXT_HORIZCENTER As UInteger = &H00000400UI
        Public Const DTWAIN_PDFTEXT_VERTCENTER As UInteger = &H00000800UI
        Public Const DTWAIN_PDFTEXT_BOTTOMLEFT As UInteger = &H00001000UI
        Public Const DTWAIN_PDFTEXT_BOTTOMRIGHT As UInteger = &H00002000UI
        Public Const DTWAIN_PDFTEXT_BOTTOMCENTER As UInteger = &H00004000UI
        Public Const DTWAIN_PDFTEXT_TOPCENTER As UInteger = &H00008000UI
        Public Const DTWAIN_PDFTEXT_XCENTER As UInteger = &H00010000UI
        Public Const DTWAIN_PDFTEXT_YCENTER As UInteger = &H00020000UI
        Public Const DTWAIN_PDFTEXT_NOSCALING As UInteger = &H00100000UI
        Public Const DTWAIN_PDFTEXT_NOCHARSPACING As UInteger = &H00200000UI
        Public Const DTWAIN_PDFTEXT_NOWORDSPACING As UInteger = &H00400000UI
        Public Const DTWAIN_PDFTEXT_NOSTROKEWIDTH As UInteger = &H00800000UI
        Public Const DTWAIN_PDFTEXT_NORENDERMODE As UInteger = &H01000000UI
        Public Const DTWAIN_PDFTEXT_NORGBCOLOR As UInteger = &H02000000UI
        Public Const DTWAIN_PDFTEXT_NOFONTSIZE As UInteger = &H04000000UI
        Public Const DTWAIN_PDFTEXT_NOABSPOSITION As UInteger = &H08000000UI
        Public Const DTWAIN_PDFTEXT_IGNOREALL As UInteger = &HFFF00000UI
        Public Const DTWAIN_FONT_COURIER As Integer = 0
        Public Const DTWAIN_FONT_COURIERBOLD As Integer = 1
        Public Const DTWAIN_FONT_COURIERBOLDOBLIQUE As Integer = 2
        Public Const DTWAIN_FONT_COURIEROBLIQUE As Integer = 3
        Public Const DTWAIN_FONT_HELVETICA As Integer = 4
        Public Const DTWAIN_FONT_HELVETICABOLD As Integer = 5
        Public Const DTWAIN_FONT_HELVETICABOLDOBLIQUE As Integer = 6
        Public Const DTWAIN_FONT_HELVETICAOBLIQUE As Integer = 7
        Public Const DTWAIN_FONT_TIMESBOLD As Integer = 8
        Public Const DTWAIN_FONT_TIMESBOLDITALIC As Integer = 9
        Public Const DTWAIN_FONT_TIMESROMAN As Integer = 10
        Public Const DTWAIN_FONT_TIMESITALIC As Integer = 11
        Public Const DTWAIN_FONT_SYMBOL As Integer = 12
        Public Const DTWAIN_FONT_ZAPFDINGBATS As Integer = 13
        Public Const DTWAIN_PDFRENDER_FILL As Integer = 0
        Public Const DTWAIN_PDFRENDER_STROKE As Integer = 1
        Public Const DTWAIN_PDFRENDER_FILLSTROKE As Integer = 2
        Public Const DTWAIN_PDFRENDER_INVISIBLE As Integer = 3
        Public Const DTWAIN_PDFTEXTELEMENT_SCALINGXY As Integer = 0
        Public Const DTWAIN_PDFTEXTELEMENT_FONTHEIGHT As Integer = 1
        Public Const DTWAIN_PDFTEXTELEMENT_WORDSPACING As Integer = 2
        Public Const DTWAIN_PDFTEXTELEMENT_POSITION As Integer = 3
        Public Const DTWAIN_PDFTEXTELEMENT_COLOR As Integer = 4
        Public Const DTWAIN_PDFTEXTELEMENT_STROKEWIDTH As Integer = 5
        Public Const DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS As Integer = 6
        Public Const DTWAIN_PDFTEXTELEMENT_FONTNAME As Integer = 7
        Public Const DTWAIN_PDFTEXTELEMENT_TEXT As Integer = 8
        Public Const DTWAIN_PDFTEXTELEMENT_RENDERMODE As Integer = 9
        Public Const DTWAIN_PDFTEXTELEMENT_CHARSPACING As Integer = 10
        Public Const DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE As Integer = 11
        Public Const DTWAIN_PDFTEXTELEMENT_LEADING As Integer = 12
        Public Const DTWAIN_PDFTEXTELEMENT_SCALING As Integer = 13
        Public Const DTWAIN_PDFTEXTELEMENT_TEXTLENGTH As Integer = 14
        Public Const DTWAIN_PDFTEXTELEMENT_SKEWANGLES As Integer = 15
        Public Const DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER As Integer = 16
        Public Const DTWAIN_PDFTEXTTRANSFORM_TSRK As Integer = 0
        Public Const DTWAIN_PDFTEXTTRANSFORM_TSKR As Integer = 1
        Public Const DTWAIN_PDFTEXTTRANSFORM_TKSR As Integer = 2
        Public Const DTWAIN_PDFTEXTTRANSFORM_TKRS As Integer = 3
        Public Const DTWAIN_PDFTEXTTRANSFORM_TRSK As Integer = 4
        Public Const DTWAIN_PDFTEXTTRANSFORM_TRKS As Integer = 5
        Public Const DTWAIN_PDFTEXTTRANSFORM_STRK As Integer = 6
        Public Const DTWAIN_PDFTEXTTRANSFORM_STKR As Integer = 7
        Public Const DTWAIN_PDFTEXTTRANSFORM_SKTR As Integer = 8
        Public Const DTWAIN_PDFTEXTTRANSFORM_SKRT As Integer = 9
        Public Const DTWAIN_PDFTEXTTRANSFORM_SRTK As Integer = 10
        Public Const DTWAIN_PDFTEXTTRANSFORM_SRKT As Integer = 11
        Public Const DTWAIN_PDFTEXTTRANSFORM_RSTK As Integer = 12
        Public Const DTWAIN_PDFTEXTTRANSFORM_RSKT As Integer = 13
        Public Const DTWAIN_PDFTEXTTRANSFORM_RTSK As Integer = 14
        Public Const DTWAIN_PDFTEXTTRANSFORM_RTKT As Integer = 15
        Public Const DTWAIN_PDFTEXTTRANSFORM_RKST As Integer = 16
        Public Const DTWAIN_PDFTEXTTRANSFORM_RKTS As Integer = 17
        Public Const DTWAIN_PDFTEXTTRANSFORM_KSTR As Integer = 18
        Public Const DTWAIN_PDFTEXTTRANSFORM_KSRT As Integer = 19
        Public Const DTWAIN_PDFTEXTTRANSFORM_KRST As Integer = 20
        Public Const DTWAIN_PDFTEXTTRANSFORM_KRTS As Integer = 21
        Public Const DTWAIN_PDFTEXTTRANSFORM_KTSR As Integer = 22
        Public Const DTWAIN_PDFTEXTTRANSFORM_KTRS As Integer = 23
        Public Const DTWAIN_PDFTEXTTRANFORM_LAST As Integer = DTWAIN_PDFTEXTTRANSFORM_KTRS
        Public Const DTWAIN_TWDF_ULTRASONIC As Integer = 0
        Public Const DTWAIN_TWDF_BYLENGTH As Integer = 1
        Public Const DTWAIN_TWDF_INFRARED As Integer = 2
        Public Const DTWAIN_TWAS_NONE As Integer = 0
        Public Const DTWAIN_TWAS_AUTO As Integer = 1
        Public Const DTWAIN_TWAS_CURRENT As Integer = 2
        Public Const DTWAIN_TWFR_BOOK As Integer = 0
        Public Const DTWAIN_TWFR_FANFOLD As Integer = 1
        Public Const DTWAIN_CONSTANT_TWPT As Integer = 0
        Public Const DTWAIN_CONSTANT_TWUN As Integer = 1
        Public Const DTWAIN_CONSTANT_TWCY As Integer = 2
        Public Const DTWAIN_CONSTANT_TWAL As Integer = 3
        Public Const DTWAIN_CONSTANT_TWAS As Integer = 4
        Public Const DTWAIN_CONSTANT_TWBCOR As Integer = 5
        Public Const DTWAIN_CONSTANT_TWBD As Integer = 6
        Public Const DTWAIN_CONSTANT_TWBO As Integer = 7
        Public Const DTWAIN_CONSTANT_TWBP As Integer = 8
        Public Const DTWAIN_CONSTANT_TWBR As Integer = 9
        Public Const DTWAIN_CONSTANT_TWBT As Integer = 10
        Public Const DTWAIN_CONSTANT_TWCP As Integer = 11
        Public Const DTWAIN_CONSTANT_TWCS As Integer = 12
        Public Const DTWAIN_CONSTANT_TWDE As Integer = 13
        Public Const DTWAIN_CONSTANT_TWDR As Integer = 14
        Public Const DTWAIN_CONSTANT_TWDSK As Integer = 15
        Public Const DTWAIN_CONSTANT_TWDX As Integer = 16
        Public Const DTWAIN_CONSTANT_TWFA As Integer = 17
        Public Const DTWAIN_CONSTANT_TWFE As Integer = 18
        Public Const DTWAIN_CONSTANT_TWFF As Integer = 19
        Public Const DTWAIN_CONSTANT_TWFL As Integer = 20
        Public Const DTWAIN_CONSTANT_TWFO As Integer = 21
        Public Const DTWAIN_CONSTANT_TWFP As Integer = 22
        Public Const DTWAIN_CONSTANT_TWFR As Integer = 23
        Public Const DTWAIN_CONSTANT_TWFT As Integer = 24
        Public Const DTWAIN_CONSTANT_TWFY As Integer = 25
        Public Const DTWAIN_CONSTANT_TWIA As Integer = 26
        Public Const DTWAIN_CONSTANT_TWIC As Integer = 27
        Public Const DTWAIN_CONSTANT_TWIF As Integer = 28
        Public Const DTWAIN_CONSTANT_TWIM As Integer = 29
        Public Const DTWAIN_CONSTANT_TWJC As Integer = 30
        Public Const DTWAIN_CONSTANT_TWJQ As Integer = 31
        Public Const DTWAIN_CONSTANT_TWLP As Integer = 32
        Public Const DTWAIN_CONSTANT_TWLS As Integer = 33
        Public Const DTWAIN_CONSTANT_TWMD As Integer = 34
        Public Const DTWAIN_CONSTANT_TWNF As Integer = 35
        Public Const DTWAIN_CONSTANT_TWOR As Integer = 36
        Public Const DTWAIN_CONSTANT_TWOV As Integer = 37
        Public Const DTWAIN_CONSTANT_TWPA As Integer = 38
        Public Const DTWAIN_CONSTANT_TWPC As Integer = 39
        Public Const DTWAIN_CONSTANT_TWPCH As Integer = 40
        Public Const DTWAIN_CONSTANT_TWPF As Integer = 41
        Public Const DTWAIN_CONSTANT_TWPM As Integer = 42
        Public Const DTWAIN_CONSTANT_TWPR As Integer = 43
        Public Const DTWAIN_CONSTANT_TWPF2 As Integer = 44
        Public Const DTWAIN_CONSTANT_TWCT As Integer = 45
        Public Const DTWAIN_CONSTANT_TWPS As Integer = 46
        Public Const DTWAIN_CONSTANT_TWSS As Integer = 47
        Public Const DTWAIN_CONSTANT_TWPH As Integer = 48
        Public Const DTWAIN_CONSTANT_TWCI As Integer = 49
        Public Const DTWAIN_CONSTANT_FONTNAME As Integer = 50
        Public Const DTWAIN_CONSTANT_TWEI As Integer = 51
        Public Const DTWAIN_CONSTANT_TWEJ As Integer = 52
        Public Const DTWAIN_CONSTANT_TWCC As Integer = 53
        Public Const DTWAIN_CONSTANT_TWQC As Integer = 54
        Public Const DTWAIN_CONSTANT_TWRC As Integer = 55
        Public Const DTWAIN_CONSTANT_MSG As Integer = 56
        Public Const DTWAIN_CONSTANT_TWLG As Integer = 57
        Public Const DTWAIN_CONSTANT_DLLINFO As Integer = 58
        Public Const DTWAIN_CONSTANT_DG As Integer = 59
        Public Const DTWAIN_CONSTANT_DAT As Integer = 60
        Public Const DTWAIN_CONSTANT_DF As Integer = 61
        Public Const DTWAIN_CONSTANT_TWTY As Integer = 62
        Public Const DTWAIN_CONSTANT_TWCB As Integer = 63
        Public Const DTWAIN_CONSTANT_TWAF As Integer = 64
        Public Const DTWAIN_CONSTANT_TWFS As Integer = 65
        Public Const DTWAIN_CONSTANT_TWJS As Integer = 66
        Public Const DTWAIN_CONSTANT_TWMR As Integer = 67
        Public Const DTWAIN_CONSTANT_TWDP As Integer = 68
        Public Const DTWAIN_CONSTANT_TWUS As Integer = 69
        Public Const DTWAIN_CONSTANT_TWDF As Integer = 70
        Public Const DTWAIN_CONSTANT_TWFM As Integer = 71
        Public Const DTWAIN_CONSTANT_TWSG As Integer = 72
        Public Const DTWAIN_CONSTANT_DTWAIN_TN As Integer = 73
        Public Const DTWAIN_CONSTANT_TWON As Integer = 74
        Public Const DTWAIN_CONSTANT_TWMF As Integer = 75
        Public Const DTWAIN_CONSTANT_TWSX As Integer = 76
        Public Const DTWAIN_CONSTANT_CAP As Integer = 77
        Public Const DTWAIN_CONSTANT_ICAP As Integer = 78
        Public Const DTWAIN_CONSTANT_DTWAIN_CONT As Integer = 79
        Public Const DTWAIN_CONSTANT_CAPCODE_MAP As Integer = 80
        Public Const DTWAIN_CONSTANT_ACAP As Integer = 81
        Public Const DTWAIN_USERRES_START As Integer = 20000
        Public Const DTWAIN_USERRES_MAXSIZE As Integer = 8192
        Public Const DTWAIN_APIHANDLEOK As Integer = 1
        Public Const DTWAIN_TWAINSESSIONOK As Integer = 2
        Public Const DTWAIN_PDF_AES128 As Integer = 1
        Public Const DTWAIN_PDF_AES256 As Integer = 2
        Public Const DTWAIN_FEEDER_TERMINATE As Integer = 1
        Public Const DTWAIN_FEEDER_USEFLATBED As Integer = 2

        Public Delegate Function DTwainCallback(WParam As IntPtr, LParam As IntPtr, UserData As IntPtr) As IntPtr
        Public Delegate Function DTwainCallback64(WParam As IntPtr, LParam As IntPtr, UserData As IntPtr) As IntPtr
        Public Delegate Function DTwainErrorProc(param1 As Integer, param2 As Integer) As IntPtr
        Public Delegate Function DTwainErrorProc64(param1 As Integer, param2 As Long) As IntPtr
        Public Delegate Function DTwainLoggerProcA(<MarshalAs(UnmanagedType.LPStr)> lpszName As String, UserData As Long) As IntPtr
        Public Delegate Function DTwainLoggerProcW(<MarshalAs(UnmanagedType.LPWStr)> lpszName As String, UserData As Long) As IntPtr
        Public Delegate Function DTwainDIBUpdateProc(TheSource As IntPtr, currentImage As Integer, DibData As IntPtr) As IntPtr
        Public Delegate Function DTwainLoggerProc(<MarshalAs(UnmanagedType.LPTStr)> lpszName As String, UserData As Long) As IntPtr
        
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireAudioFileDelegate(Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lMaxClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_AcquireAudioFileADelegate(Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lNumClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_AcquireAudioFileWDelegate(Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lNumClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireAudioNativeDelegate(Source As System.IntPtr, nMaxAudioClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireAudioNativeExDelegate(Source As System.IntPtr, nMaxAudioClips As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireBufferedDelegate(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireBufferedExDelegate(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireFileDelegate(Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_AcquireFileADelegate(Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireFileExDelegate(Source As System.IntPtr, aFileNames As System.IntPtr, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_AcquireFileWDelegate(Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireNativeDelegate(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireNativeExDelegate(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AcquireToClipboardDelegate(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, nTransferMode As Integer, bDiscardDibs As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AddExtImageInfoQueryDelegate(Source As System.IntPtr, ExtImageInfo As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AddFileToAppendDelegate(szFile As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_AddFileToAppendADelegate(szFile As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_AddFileToAppendWDelegate(szFile As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AddPDFTextDelegate(Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_AddPDFTextADelegate(Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AddPDFTextExDelegate(Source As System.IntPtr, TextElement As System.IntPtr, Flags As UInteger) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_AddPDFTextWDelegate(Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AllocateMemoryDelegate(memSize As UInteger) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AllocateMemory64Delegate(memSize As System.UInt64) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AllocateMemoryExDelegate(memSize As UInteger) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_AppHandlesExceptionsDelegate(bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayANSIStringToFloatDelegate(StringArray As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddDelegate(pArray As System.IntPtr, pVariant As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayAddANSIStringDelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayAddANSIStringNDelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddFloatDelegate(pArray As System.IntPtr, Val As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddFloatNDelegate(pArray As System.IntPtr, Val As System.Double, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddFloatStringDelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayAddFloatStringADelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddFloatStringNDelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayAddFloatStringNADelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayAddFloatStringNWDelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayAddFloatStringWDelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddFrameDelegate(pArray As System.IntPtr, frame As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddFrameNDelegate(pArray As System.IntPtr, frame As System.IntPtr, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddLongDelegate(pArray As System.IntPtr, Val As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddLong64Delegate(pArray As System.IntPtr, Val As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddLong64NDelegate(pArray As System.IntPtr, Val As System.Int64, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddLongNDelegate(pArray As System.IntPtr, Val As Integer, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddNDelegate(pArray As System.IntPtr, pVariant As System.IntPtr, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddStringDelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayAddStringADelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayAddStringNDelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayAddStringNADelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayAddStringNWDelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayAddStringWDelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayAddWideStringDelegate(pArray As System.IntPtr, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayAddWideStringNDelegate(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayConvertFix32ToFloatDelegate(Fix32Array As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayConvertFloatToFix32Delegate(FloatArray As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayCopyDelegate(Source As System.IntPtr, Dest As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayCreateDelegate(nEnumType As Integer, nInitialSize As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayCreateCopyDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayCreateFromCapDelegate(Source As System.IntPtr, lCapType As Integer, lSize As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayCreateFromLong64sDelegate(ByRef pCArray As System.Int64, nSize As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayCreateFromLongsDelegate(ByRef pCArray As Integer, nSize As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayCreateFromRealsDelegate(ByRef pCArray As System.Double, nSize As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayDestroyDelegate(pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayDestroyFramesDelegate(FrameArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFindDelegate(pArray As System.IntPtr, pVariant As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayFindANSIStringDelegate(pArray As System.IntPtr, pString As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFindFloatDelegate(pArray As System.IntPtr, Val As System.Double, Tolerance As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFindFloatStringDelegate(pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayFindFloatStringADelegate(pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayFindFloatStringWDelegate(pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFindLongDelegate(pArray As System.IntPtr, Val As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFindLong64Delegate(pArray As System.IntPtr, Val As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFindStringDelegate(pArray As System.IntPtr, pString As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayFindStringADelegate(pArray As System.IntPtr, pString As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayFindStringWDelegate(pArray As System.IntPtr, pString As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayFindWideStringDelegate(pArray As System.IntPtr, pString As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFix32GetAtDelegate(aFix32 As System.IntPtr, lPos As Integer, ByRef Whole As Integer, ByRef Frac As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFix32SetAtDelegate(aFix32 As System.IntPtr, lPos As Integer, Whole As Integer, Frac As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFloatToANSIStringDelegate(FloatArray As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFloatToStringDelegate(FloatArray As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayFloatToWideStringDelegate(FloatArray As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtDelegate(pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayGetAtANSIStringDelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> pStr As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtFloatDelegate(pArray As System.IntPtr, nWhere As Integer, ByRef pVal As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtFloatStringDelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> Val As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayGetAtFloatStringADelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> Val As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayGetAtFloatStringWDelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> Val As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtFrameDelegate(FrameArray As System.IntPtr, nWhere As Integer, ByRef pleft As System.Double, ByRef ptop As System.Double, ByRef pright As System.Double, ByRef pbottom As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtFrameExDelegate(FrameArray As System.IntPtr, nWhere As Integer, Frame As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtFrameStringDelegate(FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> bottom As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayGetAtFrameStringADelegate(FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> bottom As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayGetAtFrameStringWDelegate(FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> bottom As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtLongDelegate(pArray As System.IntPtr, nWhere As Integer, ByRef pVal As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtLong64Delegate(pArray As System.IntPtr, nWhere As Integer, ByRef pVal As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtSourceDelegate(pArray As System.IntPtr, nWhere As Integer, ByRef ppSource As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetAtStringDelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> pStr As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayGetAtStringADelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> pStr As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayGetAtStringWDelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> pStr As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayGetAtWideStringDelegate(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> pStr As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetBufferDelegate(pArray As System.IntPtr, nPos As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetCapValuesDelegate(Source As System.IntPtr, lCap As Integer, lGetType As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetCapValuesExDelegate(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetCapValuesEx2Delegate(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, nDataType As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetCountDelegate(pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetMaxStringLengthDelegate(a As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetSourceAtDelegate(pArray As System.IntPtr, nWhere As Integer, ByRef ppSource As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetStringLengthDelegate(a As System.IntPtr, nWhichString As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayGetTypeDelegate(pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInitDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtDelegate(pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayInsertAtANSIStringDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayInsertAtANSIStringNDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatNDelegate(pArray As System.IntPtr, nWhere As Integer, Val As System.Double, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatStringDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatStringADelegate(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatStringNDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatStringNADelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatStringNWDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayInsertAtFloatStringWDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtFrameDelegate(pArray As System.IntPtr, nWhere As Integer, frame As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtFrameNDelegate(pArray As System.IntPtr, nWhere As Integer, frame As System.IntPtr, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtLongDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtLong64Delegate(pArray As System.IntPtr, nWhere As Integer, Val As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtLong64NDelegate(pArray As System.IntPtr, nWhere As Integer, Val As System.Int64, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtLongNDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As Integer, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtNDelegate(pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtStringDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayInsertAtStringADelegate(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayInsertAtStringNDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArrayInsertAtStringNADelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayInsertAtStringNWDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayInsertAtStringWDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayInsertAtWideStringDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArrayInsertAtWideStringNDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayRemoveAllDelegate(pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayRemoveAtDelegate(pArray As System.IntPtr, nWhere As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayRemoveAtNDelegate(pArray As System.IntPtr, nWhere As Integer, num As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayResizeDelegate(pArray As System.IntPtr, NewSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtDelegate(pArray As System.IntPtr, lPos As Integer, pVariant As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArraySetAtANSIStringDelegate(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtFloatDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtFloatStringDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArraySetAtFloatStringADelegate(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArraySetAtFloatStringWDelegate(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtFrameDelegate(FrameArray As System.IntPtr, nWhere As Integer, left As System.Double, top As System.Double, right As System.Double, bottom As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtFrameExDelegate(FrameArray As System.IntPtr, nWhere As Integer, Frame As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtFrameStringDelegate(FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArraySetAtFrameStringADelegate(FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArraySetAtFrameStringWDelegate(FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtLongDelegate(pArray As System.IntPtr, nWhere As Integer, pVal As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtLong64Delegate(pArray As System.IntPtr, nWhere As Integer, Val As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArraySetAtStringDelegate(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ArraySetAtStringADelegate(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArraySetAtStringWDelegate(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ArraySetAtWideStringDelegate(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayStringToFloatDelegate(StringArray As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ArrayWideStringToFloatDelegate(StringArray As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CallCallbackDelegate(wParam As Integer, lParam As Integer, UserData As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CallCallback64Delegate(wParam As Integer, lParam As Integer, UserData As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CallDSMProcDelegate(AppID As System.IntPtr, SourceId As System.IntPtr, lDG As Integer, lDAT As Integer, lMSG As Integer, pData As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CheckHandlesDelegate(bCheck As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ClearBuffersDelegate(Source As System.IntPtr, ClearBuffer As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ClearErrorBufferDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ClearPDFTextDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ClearPageDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CloseSourceDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CloseSourceUIDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ConvertDIBToBitmapDelegate(hDib As System.IntPtr, hPalette As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ConvertDIBToFullBitmapDelegate(hDib As System.IntPtr, isBMP As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ConvertToAPIStringDelegate(lpOrigString As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ConvertToAPIStringADelegate(lpOrigString As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ConvertToAPIStringExDelegate(lpOrigString As String, <MarshalAs(UnmanagedType.LPTStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ConvertToAPIStringExADelegate(lpOrigString As String, <MarshalAs(UnmanagedType.LPStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ConvertToAPIStringExWDelegate(lpOrigString As String, <MarshalAs(UnmanagedType.LPWStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ConvertToAPIStringWDelegate(lpOrigString As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CreateAcquisitionArrayDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_CreatePDFTextElementDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_DeleteDIBDelegate(hDib As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_DestroyAcquisitionArrayDelegate(aAcq As System.IntPtr, bDestroyData As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_DestroyPDFTextElementDelegate(TextElement As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_DisableAppWindowDelegate(hWnd As System.IntPtr, bDisable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableAutoBorderDetectDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableAutoBrightDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableAutoDeskewDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableAutoFeedDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableAutoRotateDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableAutoScanDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableAutomaticSenseMediumDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableDuplexDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableFeederDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableIndicatorDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableJobFileHandlingDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableLampDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableMsgNotifyDelegate(bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnablePatchDetectDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnablePeekMessageLoopDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnablePrinterDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableThumbnailDelegate(Source As System.IntPtr, bEnable As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnableTripletsNotifyDelegate(bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EndThreadDelegate(DLLHandle As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EndTwainSessionDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAlarmVolumesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, expandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAlarmVolumesExDelegate(Source As System.IntPtr, expandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAlarmsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAlarmsExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAudioXferMechsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAudioXferMechsExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAutoFeedValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAutoFeedValuesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAutomaticCapturesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAutomaticCapturesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAutomaticSenseMediumDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumAutomaticSenseMediumExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumBitDepthsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumBitDepthsExDelegate(Source As System.IntPtr, PixelType As Integer, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumBitDepthsEx2Delegate(Source As System.IntPtr, PixelType As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumBottomCamerasDelegate(Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumBottomCamerasExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumBrightnessValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumBrightnessValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCamerasDelegate(Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCamerasExDelegate(Source As System.IntPtr, nWhichCamera As Integer, ByRef Cameras As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCamerasEx2Delegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCamerasEx3Delegate(Source As System.IntPtr, nWhichCamera As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCompressionTypesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCompressionTypesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCompressionTypesEx2Delegate(Source As System.IntPtr, lFileType As Integer, bUseBufferedMode As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumContrastValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumContrastValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCustomCapsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumCustomCapsEx2Delegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumDoubleFeedDetectLengthsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumDoubleFeedDetectLengthsExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumDoubleFeedDetectValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumDoubleFeedDetectValuesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumExtImageInfoTypesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumExtImageInfoTypesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumExtendedCapsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumExtendedCapsExDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumExtendedCapsEx2Delegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumFileTypeBitsPerPixelDelegate(FileType As Integer, ByRef Array As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumFileXferFormatsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumFileXferFormatsExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumHalftonesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumHalftonesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumHighlightValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumHighlightValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumJobControlsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumJobControlsExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumLightPathsDelegate(Source As System.IntPtr, ByRef LightPath As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumLightPathsExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumLightSourcesDelegate(Source As System.IntPtr, ByRef LightSources As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumLightSourcesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumMaxBuffersDelegate(Source As System.IntPtr, ByRef pMaxBufs As System.IntPtr, bExpandRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumMaxBuffersExDelegate(Source As System.IntPtr, bExpandRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumNoiseFiltersDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumNoiseFiltersExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumOCRInterfacesDelegate(ByRef OCRInterfaces As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumOCRSupportedCapsDelegate(Engine As System.IntPtr, ByRef SupportedCaps As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumOrientationsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumOrientationsExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumOverscanValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumOverscanValuesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPaperSizesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPaperSizesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchCodesDelegate(Source As System.IntPtr, ByRef PCodes As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchCodesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchMaxPrioritiesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchMaxPrioritiesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchMaxRetriesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchMaxRetriesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchPrioritiesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchPrioritiesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchSearchModesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchSearchModesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchTimeOutValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPatchTimeOutValuesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPixelTypesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPixelTypesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPrinterStringModesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumPrinterStringModesExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumResolutionValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumResolutionValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumShadowValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumShadowValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSourceUnitsDelegate(Source As System.IntPtr, ByRef lpArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSourceUnitsExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSourceValuesDelegate(Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_EnumSourceValuesADelegate(Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_EnumSourceValuesWDelegate(Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSourcesDelegate(ByRef lpArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSourcesExDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedCapsDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedCapsExDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedCapsEx2Delegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedExtImageInfoDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedExtImageInfoExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedFileTypesDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedMultiPageFileTypesDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumSupportedSinglePageFileTypesDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumThresholdValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumThresholdValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumTopCamerasDelegate(Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumTopCamerasExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumTwainPrintersDelegate(Source As System.IntPtr, ByRef lpAvailPrinters As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumTwainPrintersArrayDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumTwainPrintersArrayExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumTwainPrintersExDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumXResolutionValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumXResolutionValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumYResolutionValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_EnumYResolutionValuesExDelegate(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ExecuteOCRDelegate(Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_ExecuteOCRADelegate(Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_ExecuteOCRWDelegate(Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FeedPageDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FlipBitmapDelegate(hDib As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FlushAcquiredPagesDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ForceAcquireBitDepthDelegate(Source As System.IntPtr, BitDepth As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ForceScanOnNoUIDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameCreateDelegate(Left As System.Double, Top As System.Double, Right As System.Double, Bottom As System.Double) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameCreateStringDelegate(Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_FrameCreateStringADelegate(Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_FrameCreateStringWDelegate(Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameDestroyDelegate(Frame As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameGetAllDelegate(Frame As System.IntPtr, ByRef Left As System.Double, ByRef Top As System.Double, ByRef Right As System.Double, ByRef Bottom As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameGetAllStringDelegate(Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Bottom As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_FrameGetAllStringADelegate(Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Bottom As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_FrameGetAllStringWDelegate(Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Bottom As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameGetValueDelegate(Frame As System.IntPtr, nWhich As Integer, ByRef Value As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameGetValueStringDelegate(Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPTStr)> Value As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_FrameGetValueStringADelegate(Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPStr)> Value As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_FrameGetValueStringWDelegate(Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPWStr)> Value As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameIsValidDelegate(Frame As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameSetAllDelegate(Frame As System.IntPtr, Left As System.Double, Top As System.Double, Right As System.Double, Bottom As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameSetAllStringDelegate(Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_FrameSetAllStringADelegate(Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_FrameSetAllStringWDelegate(Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameSetValueDelegate(Frame As System.IntPtr, nWhich As Integer, Value As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FrameSetValueStringDelegate(Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_FrameSetValueStringADelegate(Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_FrameSetValueStringWDelegate(Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FreeExtImageInfoDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FreeMemoryDelegate(h As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_FreeMemoryExDelegate(h As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAPIHandleStatusDelegate(pHandle As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireAreaDelegate(Source As System.IntPtr, lGetType As Integer, ByRef FloatEnum As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireArea2Delegate(Source As System.IntPtr, ByRef left As System.Double, ByRef top As System.Double, ByRef right As System.Double, ByRef bottom As System.Double, ByRef lpUnit As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireArea2StringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetAcquireArea2StringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetAcquireArea2StringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireAreaExDelegate(Source As System.IntPtr, lGetType As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireMetricsDelegate(source As System.IntPtr, ByRef ImageCount As Integer, ByRef SheetCount As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireStripBufferDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireStripDataDelegate(Source As System.IntPtr, ByRef lpCompression As Integer, ByRef lpBytesPerRow As UInteger, ByRef lpColumns As UInteger, ByRef lpRows As UInteger, ByRef XOffset As UInteger, ByRef YOffset As UInteger, ByRef lpBytesWritten As UInteger) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquireStripSizesDelegate(Source As System.IntPtr, ByRef lpMin As UInteger, ByRef lpMax As UInteger, ByRef lpPreferred As UInteger) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquiredImageDelegate(aAcq As System.IntPtr, nWhichAcq As Integer, nWhichDib As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAcquiredImageArrayDelegate(aAcq As System.IntPtr, nWhichAcq As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetActiveDSMPathDelegate(<MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetActiveDSMPathADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetActiveDSMPathWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetActiveDSMVersionInfoDelegate(<MarshalAs(UnmanagedType.LPTStr)> szDLLInfo As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetActiveDSMVersionInfoADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetActiveDSMVersionInfoWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAlarmVolumeDelegate(Source As System.IntPtr, ByRef lpVolume As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAllSourceDibsDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAppInfoDelegate(<MarshalAs(UnmanagedType.LPTStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szProdName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetAppInfoADelegate(<MarshalAs(UnmanagedType.LPStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szProdName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetAppInfoWDelegate(<MarshalAs(UnmanagedType.LPWStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szProdName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetAuthorDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szAuthor As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetAuthorADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szAuthor As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetAuthorWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szAuthor As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetBatteryMinutesDelegate(Source As System.IntPtr, ByRef lpMinutes As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetBatteryPercentDelegate(Source As System.IntPtr, ByRef lpPercent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetBitDepthDelegate(Source As System.IntPtr, ByRef BitDepth As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetBlankPageAutoDetectionDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetBrightnessDelegate(Source As System.IntPtr, ByRef Brightness As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetBrightnessStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Brightness As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetBrightnessStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Contrast As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetBrightnessStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Contrast As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetBufferedTransferInfoDelegate(Source As System.IntPtr, ByRef Compression As UInteger, ByRef BytesPerRow As UInteger, ByRef Columns As UInteger, ByRef Rows As UInteger, ByRef XOffset As UInteger, ByRef YOffset As UInteger, ByRef Flags As UInteger, ByRef BytesWritten As UInteger, ByRef MemoryLength As UInteger) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCallbackDelegate() As DTwainCallback
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCallback64Delegate() As DTwainCallback64
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapArrayTypeDelegate(Source As System.IntPtr, nCap As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapContainerDelegate(Source As System.IntPtr, nCap As Integer, lCapType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapContainerExDelegate(nCap As Integer, bSetContainer As Integer, ByRef ConTypes As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapContainerEx2Delegate(nCap As Integer, bSetContainer As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapDataTypeDelegate(Source As System.IntPtr, nCap As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapFromNameDelegate(szName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetCapFromNameADelegate(szName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetCapFromNameWDelegate(szName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapOperationsDelegate(Source As System.IntPtr, lCapability As Integer, ByRef lpOps As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapValuesDelegate(Source As System.IntPtr, lCap As Integer, lGetType As Integer, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapValuesExDelegate(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCapValuesEx2Delegate(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, nDataType As Integer, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCaptionDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Caption As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetCaptionADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Caption As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetCaptionWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Caption As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCompressionSizeDelegate(Source As System.IntPtr, ByRef lBytes As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCompressionTypeDelegate(Source As System.IntPtr, ByRef lpCompression As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetConditionCodeStringDelegate(lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetConditionCodeStringADelegate(lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetConditionCodeStringWDelegate(lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetContrastDelegate(Source As System.IntPtr, ByRef Contrast As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetContrastStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Contrast As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetContrastStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Contrast As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetContrastStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Contrast As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCountryDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCurrentAcquiredImageDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCurrentFileNameDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder, MaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetCurrentFileNameADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder, MaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetCurrentFileNameWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder, MaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCurrentPageNumDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCurrentRetryCountDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCurrentTwainTripletDelegate(ByRef pAppID As TW_IDENTITY, ByRef pSourceID As TW_IDENTITY, ByRef lpDG As Integer, ByRef lpDAT As Integer, ByRef lpMsg As Integer, ByRef lpMemRef As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetCustomDSDataDelegate(Source As System.IntPtr, Data As Byte(), dSize As UInteger, ByRef pActualSize As UInteger, nFlags As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDSMFullNameDelegate(DSMType As Integer, <MarshalAs(UnmanagedType.LPTStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetDSMFullNameADelegate(DSMType As Integer, <MarshalAs(UnmanagedType.LPStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetDSMFullNameWDelegate(DSMType As Integer, <MarshalAs(UnmanagedType.LPWStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDSMSearchOrderDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDTWAINHandleDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDeviceEventDelegate(Source As System.IntPtr, ByRef lpEvent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDeviceEventExDelegate(Source As System.IntPtr, ByRef lpEvent As Integer, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDeviceEventInfoDelegate(Source As System.IntPtr, nWhichInfo As Integer, pValue As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDeviceNotificationsDelegate(Source As System.IntPtr, ByRef DevEvents As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDeviceTimeDateDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szTimeDate As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetDeviceTimeDateADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szTimeDate As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetDeviceTimeDateWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szTimeDate As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDoubleFeedDetectLengthDelegate(Source As System.IntPtr, ByRef Value As System.Double, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDoubleFeedDetectValuesDelegate(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetDuplexTypeDelegate(Source As System.IntPtr, ByRef lpDupType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetErrorBufferDelegate(ByRef ArrayBuffer As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetErrorBufferThresholdDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetErrorCallbackDelegate() As DTwainErrorProc
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetErrorCallback64Delegate() As DTwainErrorProc64
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetErrorStringDelegate(lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetErrorStringADelegate(lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetErrorStringWDelegate(lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetExtCapFromNameDelegate(szName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetExtCapFromNameADelegate(szName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetExtCapFromNameWDelegate(szName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetExtImageInfoDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetExtImageInfoDataDelegate(Source As System.IntPtr, nWhich As Integer, ByRef Data As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetExtImageInfoDataExDelegate(Source As System.IntPtr, nWhich As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetExtImageInfoItemDelegate(Source As System.IntPtr, nWhich As Integer, ByRef InfoID As Integer, ByRef NumItems As Integer, ByRef Type As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetExtImageInfoItemExDelegate(Source As System.IntPtr, nWhich As Integer, ByRef InfoID As Integer, ByRef NumItems As Integer, ByRef Type As Integer, ByRef ReturnCode As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetExtNameFromCapDelegate(nValue As Integer, <MarshalAs(UnmanagedType.LPTStr)> szValue As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetExtNameFromCapADelegate(nValue As Integer, <MarshalAs(UnmanagedType.LPStr)> szValue As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetExtNameFromCapWDelegate(nValue As Integer, <MarshalAs(UnmanagedType.LPWStr)> szValue As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetFeederAlignmentDelegate(Source As System.IntPtr, ByRef lpAlignment As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetFeederFuncsDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetFeederOrderDelegate(Source As System.IntPtr, ByRef lpOrder As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetFeederWaitTimeDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetFileCompressionTypeDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetFileTypeExtensionsDelegate(nType As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetFileTypeExtensionsADelegate(nType As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetFileTypeExtensionsWDelegate(nType As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetFileTypeNameDelegate(nType As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetFileTypeNameADelegate(nType As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetFileTypeNameWDelegate(nType As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetHalftoneDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetHalftoneADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetHalftoneWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetHighlightDelegate(Source As System.IntPtr, ByRef Highlight As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetHighlightStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Highlight As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetHighlightStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Highlight As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetHighlightStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Highlight As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetImageInfoDelegate(Source As System.IntPtr, ByRef lpXResolution As System.Double, ByRef lpYResolution As System.Double, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetImageInfoStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetImageInfoStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetImageInfoStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetJobControlDelegate(Source As System.IntPtr, ByRef pJobControl As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetJpegValuesDelegate(Source As System.IntPtr, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetJpegXRValuesDelegate(Source As System.IntPtr, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLanguageDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLastErrorDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLibraryPathDelegate(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetLibraryPathADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetLibraryPathWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLightPathDelegate(Source As System.IntPtr, ByRef lpLightPath As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLightSourceDelegate(Source As System.IntPtr, ByRef LightSource As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLightSourcesDelegate(Source As System.IntPtr, ByRef LightSources As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLoggerCallbackDelegate() As DTwainLoggerProc
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLoggerCallbackADelegate() As DTwainLoggerProcA
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetLoggerCallbackWDelegate() As DTwainLoggerProcW
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetManualDuplexCountDelegate(Source As System.IntPtr, ByRef pSide1 As Integer, ByRef pSide2 As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetMaxAcquisitionsDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetMaxBuffersDelegate(Source As System.IntPtr, ByRef pMaxBuf As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetMaxPagesToAcquireDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetMaxRetryAttemptsDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetNameFromCapDelegate(nCapValue As Integer, <MarshalAs(UnmanagedType.LPTStr)> szValue As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetNameFromCapADelegate(nCapValue As Integer, <MarshalAs(UnmanagedType.LPStr)> szValue As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetNameFromCapWDelegate(nCapValue As Integer, <MarshalAs(UnmanagedType.LPWStr)> szValue As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetNoiseFilterDelegate(Source As System.IntPtr, ByRef lpNoiseFilter As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetNumAcquiredImagesDelegate(aAcq As System.IntPtr, nWhich As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetNumAcquisitionsDelegate(aAcq As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRCapValuesDelegate(Engine As System.IntPtr, OCRCapValue As Integer, TypeOfGet As Integer, ByRef CapValues As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRErrorStringDelegate(Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetOCRErrorStringADelegate(Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetOCRErrorStringWDelegate(Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRLastErrorDelegate(Engine As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRMajorMinorVersionDelegate(Engine As System.IntPtr, ByRef lpMajor As Integer, ByRef lpMinor As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRManufacturerDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szManufacturer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetOCRManufacturerADelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szManufacturer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetOCRManufacturerWDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szManufacturer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRProductFamilyDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProductFamily As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetOCRProductFamilyADelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProductFamily As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetOCRProductFamilyWDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProductFamily As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRProductNameDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProductName As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetOCRProductNameADelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProductName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetOCRProductNameWDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProductName As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRTextDelegate(Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPTStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetOCRTextADelegate(Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRTextInfoFloatDelegate(OCRTextInfo As System.IntPtr, nCharPos As Integer, nWhichItem As Integer, ByRef pInfo As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRTextInfoFloatExDelegate(OCRTextInfo As System.IntPtr, nWhichItem As Integer, ByRef pInfo As System.Double, bufSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRTextInfoHandleDelegate(Engine As System.IntPtr, nPageNo As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRTextInfoLongDelegate(OCRTextInfo As System.IntPtr, nCharPos As Integer, nWhichItem As Integer, ByRef pInfo As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRTextInfoLongExDelegate(OCRTextInfo As System.IntPtr, nWhichItem As Integer, ByRef pInfo As Integer, bufSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetOCRTextWDelegate(Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPWStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOCRVersionInfoDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> buffer As StringBuilder, maxBufSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetOCRVersionInfoADelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> buffer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetOCRVersionInfoWDelegate(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> buffer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOrientationDelegate(Source As System.IntPtr, ByRef lpOrient As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetOverscanDelegate(Source As System.IntPtr, ByRef lpOverscan As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPDFTextElementFloatDelegate(TextElement As System.IntPtr, ByRef val1 As System.Double, ByRef val2 As System.Double, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPDFTextElementLongDelegate(TextElement As System.IntPtr, ByRef val1 As Integer, ByRef val2 As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPDFTextElementStringDelegate(TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetPDFTextElementStringADelegate(TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetPDFTextElementStringWDelegate(TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPDFType1FontNameDelegate(FontVal As Integer, <MarshalAs(UnmanagedType.LPTStr)> szFont As StringBuilder, nChars As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetPDFType1FontNameADelegate(FontVal As Integer, <MarshalAs(UnmanagedType.LPStr)> szFont As StringBuilder, nChars As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetPDFType1FontNameWDelegate(FontVal As Integer, <MarshalAs(UnmanagedType.LPWStr)> szFont As StringBuilder, nChars As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPaperSizeDelegate(Source As System.IntPtr, ByRef lpPaperSize As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPaperSizeNameDelegate(paperNumber As Integer, <MarshalAs(UnmanagedType.LPTStr)> outName As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetPaperSizeNameADelegate(paperNumber As Integer, <MarshalAs(UnmanagedType.LPStr)> outName As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetPaperSizeNameWDelegate(paperNumber As Integer, <MarshalAs(UnmanagedType.LPWStr)> outName As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPatchMaxPrioritiesDelegate(Source As System.IntPtr, ByRef pMaxPriorities As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPatchMaxRetriesDelegate(Source As System.IntPtr, ByRef pMaxRetries As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPatchPrioritiesDelegate(Source As System.IntPtr, ByRef SearchPriorities As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPatchSearchModeDelegate(Source As System.IntPtr, ByRef pSearchMode As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPatchTimeOutDelegate(Source As System.IntPtr, ByRef pTimeOut As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPixelFlavorDelegate(Source As System.IntPtr, ByRef lpPixelFlavor As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPixelTypeDelegate(Source As System.IntPtr, ByRef PixelType As Integer, ByRef BitDepth As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPrinterDelegate(Source As System.IntPtr, ByRef lpPrinter As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPrinterStartNumberDelegate(Source As System.IntPtr, ByRef nStart As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPrinterStringModeDelegate(Source As System.IntPtr, ByRef PrinterMode As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPrinterStringsDelegate(Source As System.IntPtr, ByRef ArrayString As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetPrinterSuffixStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Suffix As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetPrinterSuffixStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Suffix As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetPrinterSuffixStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Suffix As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetRegisteredMsgDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetResolutionDelegate(Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetResolutionStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetResolutionStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetResolutionStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetResourceStringDelegate(ResourceID As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetResourceStringADelegate(ResourceID As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetResourceStringWDelegate(ResourceID As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetRotationDelegate(Source As System.IntPtr, ByRef Rotation As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetRotationStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Rotation As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetRotationStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Rotation As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetRotationStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Rotation As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSaveFileNameDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetSaveFileNameADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetSaveFileNameWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSavedFilesCountDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSessionDetailsDelegate(<MarshalAs(UnmanagedType.LPTStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetSessionDetailsADelegate(<MarshalAs(UnmanagedType.LPStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetSessionDetailsWDelegate(<MarshalAs(UnmanagedType.LPWStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetShadowDelegate(Source As System.IntPtr, ByRef Shadow As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetShadowStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Shadow As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetShadowStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Shadow As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetShadowStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Shadow As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetShortVersionStringDelegate(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetShortVersionStringADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetShortVersionStringWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceAcquisitionsDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceDetailsDelegate(szSources As String, <MarshalAs(UnmanagedType.LPTStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetSourceDetailsADelegate(szSources As String, <MarshalAs(UnmanagedType.LPStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetSourceDetailsWDelegate(szSources As String, <MarshalAs(UnmanagedType.LPWStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceIDDelegate(Source As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceManufacturerDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetSourceManufacturerADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetSourceManufacturerWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceProductFamilyDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetSourceProductFamilyADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetSourceProductFamilyWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceProductNameDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetSourceProductNameADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetSourceProductNameWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceUnitDelegate(Source As System.IntPtr, ByRef lpUnit As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceVersionInfoDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetSourceVersionInfoADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetSourceVersionInfoWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetSourceVersionNumberDelegate(Source As System.IntPtr, ByRef pMajor As Integer, ByRef pMinor As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetStaticLibVersionDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTempFileDirectoryDelegate(<MarshalAs(UnmanagedType.LPTStr)> szFilePath As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTempFileDirectoryADelegate(<MarshalAs(UnmanagedType.LPStr)> szFilePath As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTempFileDirectoryWDelegate(<MarshalAs(UnmanagedType.LPWStr)> szFilePath As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetThresholdDelegate(Source As System.IntPtr, ByRef Threshold As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetThresholdStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Threshold As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetThresholdStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Threshold As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetThresholdStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Threshold As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTimeDateDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szTimeDate As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTimeDateADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szTimeDate As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTimeDateWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szTimeDate As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainAppIDDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainAvailabilityDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainAvailabilityExDelegate(<MarshalAs(UnmanagedType.LPTStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainAvailabilityExADelegate(<MarshalAs(UnmanagedType.LPStr)> szDirectories As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainAvailabilityExWDelegate(<MarshalAs(UnmanagedType.LPWStr)> szDirectories As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainCountryNameDelegate(countryId As Integer, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainCountryNameADelegate(countryId As Integer, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainCountryNameWDelegate(countryId As Integer, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainCountryValueDelegate(country As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainCountryValueADelegate(country As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainCountryValueWDelegate(country As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainHwndDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainIDFromNameDelegate(lpszBuffer As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainIDFromNameADelegate(lpszBuffer As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainIDFromNameWDelegate(lpszBuffer As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainLanguageNameDelegate(nameId As Integer, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainLanguageNameADelegate(lang As Integer, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainLanguageNameWDelegate(lang As Integer, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainLanguageValueDelegate(szName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainLanguageValueADelegate(lang As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainLanguageValueWDelegate(lang As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainModeDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainNameFromConstantDelegate(lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainNameFromConstantADelegate(lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainNameFromConstantWDelegate(lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainStringNameDelegate(category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetTwainStringNameADelegate(category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetTwainStringNameWDelegate(category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetTwainTimeoutDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetVersionDelegate(ByRef lpMajor As Integer, ByRef lpMinor As Integer, ByRef lpVersionType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetVersionCopyrightDelegate(<MarshalAs(UnmanagedType.LPTStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetVersionCopyrightADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetVersionCopyrightWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetVersionExDelegate(ByRef lMajor As Integer, ByRef lMinor As Integer, ByRef lVersionType As Integer, ByRef lPatchLevel As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetVersionInfoDelegate(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetVersionInfoADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetVersionInfoWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetVersionStringDelegate(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetVersionStringADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetVersionStringWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetWindowsVersionInfoDelegate(<MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetWindowsVersionInfoADelegate(<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetWindowsVersionInfoWDelegate(<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetXResolutionDelegate(Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetXResolutionStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetXResolutionStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetXResolutionStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetYResolutionDelegate(Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_GetYResolutionStringDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_GetYResolutionStringADelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_GetYResolutionStringWDelegate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_InitExtImageInfoDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_InitImageFileAppendDelegate(szFile As String, fType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_InitImageFileAppendADelegate(szFile As String, fType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_InitImageFileAppendWDelegate(szFile As String, fType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_InitOCRInterfaceDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAcquiringDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAudioXferSupportedDelegate(Source As System.IntPtr, supportVal As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoBorderDetectEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoBorderDetectSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoBrightEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoBrightSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoDeskewEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoDeskewSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoFeedEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoFeedSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoRotateEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoRotateSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutoScanEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutomaticSenseMediumEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsAutomaticSenseMediumSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsBlankPageDetectionOnDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsBufferedTileModeOnDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsBufferedTileModeSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsCapSupportedDelegate(Source As System.IntPtr, lCapability As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsCompressionSupportedDelegate(Source As System.IntPtr, Compression As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsCustomDSDataSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDIBBlankDelegate(hDib As System.IntPtr, threshold As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDIBBlankStringDelegate(hDib As System.IntPtr, threshold As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_IsDIBBlankStringADelegate(hDib As System.IntPtr, threshold As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_IsDIBBlankStringWDelegate(hDib As System.IntPtr, threshold As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDeviceEventSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDeviceOnLineDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDoubleFeedDetectLengthSupportedDelegate(Source As System.IntPtr, value As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDoubleFeedDetectSupportedDelegate(Source As System.IntPtr, SupportVal As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDoublePageCountOnDuplexDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDuplexEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsDuplexSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsExtImageInfoSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsFeederEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsFeederLoadedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsFeederSensitiveDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsFeederSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsFileSystemSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsFileXferSupportedDelegate(Source As System.IntPtr, lFileType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldALastPageSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldALevelSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldAPrintFormatSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldAValueSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldBLastPageSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldBLevelSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldBPrintFormatSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldBValueSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldCLastPageSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldCLevelSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldCPrintFormatSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldCValueSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldDLastPageSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldDLevelSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldDPrintFormatSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldDValueSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldELastPageSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldELevelSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldEPrintFormatSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIAFieldEValueSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsImageAddressingSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIndicatorEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsIndicatorSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsInitializedDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsJPEGSupportedDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsJobControlSupportedDelegate(Source As System.IntPtr, JobControl As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsLampEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsLampSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsLightPathSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsLightSourceSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsMaxBuffersSupportedDelegate(Source As System.IntPtr, MaxBuf As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsMemFileXferSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsMsgNotifyEnabledDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsNotifyTripletsEnabledDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsOCREngineActivatedDelegate(OCREngine As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsOpenSourcesOnSelectDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsOrientationSupportedDelegate(Source As System.IntPtr, Orientation As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsOverscanSupportedDelegate(Source As System.IntPtr, SupportValue As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPDFSupportedDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPNGSupportedDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPaperDetectableDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPaperSizeSupportedDelegate(Source As System.IntPtr, PaperSize As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPatchCapsSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPatchDetectEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPatchSupportedDelegate(Source As System.IntPtr, PatchCode As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPeekMessageLoopEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPixelTypeSupportedDelegate(Source As System.IntPtr, PixelType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPrinterEnabledDelegate(Source As System.IntPtr, Printer As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsPrinterSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsRotationSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSessionEnabledDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSkipImageInfoErrorDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSourceAcquiringDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSourceAcquiringExDelegate(Source As System.IntPtr, bUIOnly As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSourceInUIOnlyModeDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSourceOpenDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSourceSelectedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsSourceValidDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsTIFFSupportedDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsThumbnailEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsThumbnailSupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsTwainAvailableDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsTwainAvailableExDelegate(<MarshalAs(UnmanagedType.LPTStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_IsTwainAvailableExADelegate(<MarshalAs(UnmanagedType.LPStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_IsTwainAvailableExWDelegate(<MarshalAs(UnmanagedType.LPWStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsTwainMsgDelegate(ByRef pMsg As WinMsg) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsUIControllableDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsUIEnabledDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_IsUIOnlySupportedDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_LoadCustomStringResourcesDelegate(sLangDLL As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_LoadCustomStringResourcesADelegate(sLangDLL As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_LoadCustomStringResourcesExDelegate(sLangDLL As String, bClear As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_LoadCustomStringResourcesExADelegate(sLangDLL As String, bClear As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_LoadCustomStringResourcesExWDelegate(sLangDLL As String, bClear As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_LoadCustomStringResourcesWDelegate(sLangDLL As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_LoadLanguageResourceDelegate(nLanguage As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_LockMemoryDelegate(h As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_LockMemoryExDelegate(h As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_LogMessageDelegate(message As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_LogMessageADelegate(message As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_LogMessageWDelegate(message As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_MakeRGBDelegate(red As Integer, green As Integer, blue As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_OpenSourceDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_OpenSourcesOnSelectDelegate(bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeCreateDelegate(nEnumType As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeCreateFromCapDelegate(Source As System.IntPtr, lCapType As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeDestroyDelegate(pSource As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeExpandDelegate(pSource As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeExpandExDelegate(Range As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetAllDelegate(pArray As System.IntPtr, pVariantLow As System.IntPtr, pVariantUp As System.IntPtr, pVariantStep As System.IntPtr, pVariantDefault As System.IntPtr, pVariantCurrent As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetAllFloatDelegate(pArray As System.IntPtr, ByRef pVariantLow As System.Double, ByRef pVariantUp As System.Double, ByRef pVariantStep As System.Double, ByRef pVariantDefault As System.Double, ByRef pVariantCurrent As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetAllFloatStringDelegate(pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dCurrent As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_RangeGetAllFloatStringADelegate(pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dCurrent As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_RangeGetAllFloatStringWDelegate(pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dCurrent As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetAllLongDelegate(pArray As System.IntPtr, ByRef pVariantLow As Integer, ByRef pVariantUp As Integer, ByRef pVariantStep As Integer, ByRef pVariantDefault As Integer, ByRef pVariantCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetCountDelegate(pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetExpValueDelegate(pArray As System.IntPtr, lPos As Integer, pVariant As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetExpValueFloatDelegate(pArray As System.IntPtr, lPos As Integer, ByRef pVal As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetExpValueFloatStringDelegate(pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPTStr)> pVal As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_RangeGetExpValueFloatStringADelegate(pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPStr)> pVal As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_RangeGetExpValueFloatStringWDelegate(pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPWStr)> pVal As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetExpValueLongDelegate(pArray As System.IntPtr, lPos As Integer, ByRef pVal As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetNearestValueDelegate(pArray As System.IntPtr, pVariantIn As System.IntPtr, pVariantOut As System.IntPtr, RoundType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetPosDelegate(pArray As System.IntPtr, pVariant As System.IntPtr, ByRef pPos As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetPosFloatDelegate(pArray As System.IntPtr, Val As System.Double, ByRef pPos As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetPosFloatStringDelegate(pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_RangeGetPosFloatStringADelegate(pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_RangeGetPosFloatStringWDelegate(pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetPosLongDelegate(pArray As System.IntPtr, Value As Integer, ByRef pPos As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetValueDelegate(pArray As System.IntPtr, nWhich As Integer, pVariant As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetValueFloatDelegate(pArray As System.IntPtr, nWhich As Integer, ByRef pVal As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetValueFloatStringDelegate(pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPTStr)> pVal As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_RangeGetValueFloatStringADelegate(pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPStr)> dValue As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_RangeGetValueFloatStringWDelegate(pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPWStr)> dValue As StringBuilder) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeGetValueLongDelegate(pArray As System.IntPtr, nWhich As Integer, ByRef pVal As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeIsValidDelegate(Range As System.IntPtr, ByRef pStatus As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeNearestValueFloatDelegate(pArray As System.IntPtr, dIn As System.Double, ByRef pOut As System.Double, RoundType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeNearestValueFloatStringDelegate(pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPTStr)> pOut As StringBuilder, RoundType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_RangeNearestValueFloatStringADelegate(pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPStr)> dOut As StringBuilder, RoundType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_RangeNearestValueFloatStringWDelegate(pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPWStr)> dOut As StringBuilder, RoundType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeNearestValueLongDelegate(pArray As System.IntPtr, lIn As Integer, ByRef pOut As Integer, RoundType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetAllDelegate(pArray As System.IntPtr, pVariantLow As System.IntPtr, pVariantUp As System.IntPtr, pVariantStep As System.IntPtr, pVariantDefault As System.IntPtr, pVariantCurrent As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetAllFloatDelegate(pArray As System.IntPtr, dLow As System.Double, dUp As System.Double, dStep As System.Double, dDefault As System.Double, dCurrent As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetAllFloatStringDelegate(pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_RangeSetAllFloatStringADelegate(pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_RangeSetAllFloatStringWDelegate(pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetAllLongDelegate(pArray As System.IntPtr, lLow As Integer, lUp As Integer, lStep As Integer, lDefault As Integer, lCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetValueDelegate(pArray As System.IntPtr, nWhich As Integer, pVal As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetValueFloatDelegate(pArray As System.IntPtr, nWhich As Integer, Val As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetValueFloatStringDelegate(pArray As System.IntPtr, nWhich As Integer, Val As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_RangeSetValueFloatStringADelegate(pArray As System.IntPtr, nWhich As Integer, dValue As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_RangeSetValueFloatStringWDelegate(pArray As System.IntPtr, nWhich As Integer, dValue As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RangeSetValueLongDelegate(pArray As System.IntPtr, nWhich As Integer, Val As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ResetPDFTextElementDelegate(TextElement As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_RewindPageDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectDefaultOCREngineDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectDefaultSourceDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectDefaultSourceWithOpenDelegate(bOpen As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectOCREngineDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectOCREngine2Delegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SelectOCREngine2ADelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectOCREngine2ExDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeFilter As String, szExcludeFilter As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SelectOCREngine2ExADelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SelectOCREngine2ExWDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SelectOCREngine2WDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectOCREngineByNameDelegate(lpszName As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SelectOCREngineByNameADelegate(lpszName As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SelectOCREngineByNameWDelegate(lpszName As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectSourceDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectSource2Delegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SelectSource2ADelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectSource2ExDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeFilter As String, szExcludeFilter As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SelectSource2ExADelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SelectSource2ExWDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SelectSource2WDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectSourceByNameDelegate(lpszName As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SelectSourceByNameADelegate(lpszName As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SelectSourceByNameWDelegate(lpszName As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectSourceByNameWithOpenDelegate(lpszName As String, bOpen As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SelectSourceByNameWithOpenADelegate(lpszName As String, bOpen As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SelectSourceByNameWithOpenWDelegate(lpszName As String, bOpen As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SelectSourceWithOpenDelegate(bOpen As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireAreaDelegate(Source As System.IntPtr, lSetType As Integer, FloatEnum As System.IntPtr, ActualEnum As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireArea2Delegate(Source As System.IntPtr, left As System.Double, top As System.Double, right As System.Double, bottom As System.Double, lUnit As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireArea2StringDelegate(Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetAcquireArea2StringADelegate(Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetAcquireArea2StringWDelegate(Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireImageNegativeDelegate(Source As System.IntPtr, IsNegative As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireImageScaleDelegate(Source As System.IntPtr, xscale As System.Double, yscale As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireImageScaleStringDelegate(Source As System.IntPtr, xscale As String, yscale As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetAcquireImageScaleStringADelegate(Source As System.IntPtr, xscale As String, yscale As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetAcquireImageScaleStringWDelegate(Source As System.IntPtr, xscale As String, yscale As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireStripBufferDelegate(Source As System.IntPtr, hMem As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAcquireStripSizeDelegate(Source As System.IntPtr, StripSize As UInteger) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAlarmVolumeDelegate(Source As System.IntPtr, Volume As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAlarmsDelegate(Source As System.IntPtr, Alarms As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAllCapsToDefaultDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAppInfoDelegate(szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetAppInfoADelegate(szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetAppInfoWDelegate(szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAuthorDelegate(Source As System.IntPtr, szAuthor As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetAuthorADelegate(Source As System.IntPtr, szAuthor As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetAuthorWDelegate(Source As System.IntPtr, szAuthor As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAvailablePrintersDelegate(Source As System.IntPtr, lpAvailPrinters As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetAvailablePrintersArrayDelegate(Source As System.IntPtr, AvailPrinters As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBitDepthDelegate(Source As System.IntPtr, BitDepth As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionDelegate(Source As System.IntPtr, threshold As System.Double, discard_option As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionExDelegate(Source As System.IntPtr, threshold As System.Double, autodetect As Integer, detectOpts As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionExStringDelegate(Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionExStringADelegate(Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionExStringWDelegate(Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionStringDelegate(Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionStringADelegate(Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetBlankPageDetectionStringWDelegate(Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBrightnessDelegate(Source As System.IntPtr, Brightness As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBrightnessStringDelegate(Source As System.IntPtr, Brightness As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetBrightnessStringADelegate(Source As System.IntPtr, Contrast As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetBrightnessStringWDelegate(Source As System.IntPtr, Contrast As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetBufferedTileModeDelegate(Source As System.IntPtr, bTileMode As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCallbackDelegate(Fn As DTwainCallback, UserData As Integer) As DTwainCallback
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCallback64Delegate(Fn As DTwainCallback64, UserData As System.Int64) As DTwainCallback64
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCameraDelegate(Source As System.IntPtr, szCamera As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetCameraADelegate(Source As System.IntPtr, szCamera As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetCameraWDelegate(Source As System.IntPtr, szCamera As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCapValuesDelegate(Source As System.IntPtr, lCap As Integer, lSetType As Integer, pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCapValuesExDelegate(Source As System.IntPtr, lCap As Integer, lSetType As Integer, lContainerType As Integer, pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCapValuesEx2Delegate(Source As System.IntPtr, lCap As Integer, lSetType As Integer, lContainerType As Integer, nDataType As Integer, pArray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCaptionDelegate(Source As System.IntPtr, Caption As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetCaptionADelegate(Source As System.IntPtr, Caption As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetCaptionWDelegate(Source As System.IntPtr, Caption As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCompressionTypeDelegate(Source As System.IntPtr, lCompression As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetContrastDelegate(Source As System.IntPtr, Contrast As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetContrastStringDelegate(Source As System.IntPtr, Contrast As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetContrastStringADelegate(Source As System.IntPtr, Contrast As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetContrastStringWDelegate(Source As System.IntPtr, Contrast As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCountryDelegate(nCountry As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCurrentRetryCountDelegate(Source As System.IntPtr, nCount As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetCustomDSDataDelegate(Source As System.IntPtr, hData As System.IntPtr, <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U8, SizeParamIndex:=3)> Data As Byte(), dSize As UInteger, nFlags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDSMSearchOrderDelegate(SearchPath As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDSMSearchOrderExDelegate(SearchOrder As String, UserPath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetDSMSearchOrderExADelegate(SearchOrder As String, szUserPath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetDSMSearchOrderExWDelegate(SearchOrder As String, szUserPath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDefaultSourceDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDeviceNotificationsDelegate(Source As System.IntPtr, DevEvents As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDeviceTimeDateDelegate(Source As System.IntPtr, szTimeDate As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetDeviceTimeDateADelegate(Source As System.IntPtr, szTimeDate As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetDeviceTimeDateWDelegate(Source As System.IntPtr, szTimeDate As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDoubleFeedDetectLengthDelegate(Source As System.IntPtr, Value As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDoubleFeedDetectLengthStringDelegate(Source As System.IntPtr, value As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetDoubleFeedDetectLengthStringADelegate(Source As System.IntPtr, szLength As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetDoubleFeedDetectLengthStringWDelegate(Source As System.IntPtr, szLength As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDoubleFeedDetectValuesDelegate(Source As System.IntPtr, prray As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetDoublePageCountOnDuplexDelegate(Source As System.IntPtr, bDoubleCount As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetEOJDetectValueDelegate(Source As System.IntPtr, nValue As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetErrorBufferThresholdDelegate(nErrors As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetErrorCallbackDelegate(proc As DTwainErrorProc, UserData As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetErrorCallback64Delegate(proc As DTwainErrorProc64, UserData64 As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetFeederAlignmentDelegate(Source As System.IntPtr, lpAlignment As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetFeederOrderDelegate(Source As System.IntPtr, lOrder As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetFeederWaitTimeDelegate(Source As System.IntPtr, waitTime As Integer, flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetFileAutoIncrementDelegate(Source As System.IntPtr, Increment As Integer, bResetOnAcquire As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetFileCompressionTypeDelegate(Source As System.IntPtr, lCompression As Integer, bIsCustom As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetFileSavePosDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetFileSavePosADelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetFileSavePosWDelegate(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetFileXferFormatDelegate(Source As System.IntPtr, lFileType As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetHalftoneDelegate(Source As System.IntPtr, lpHalftone As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetHalftoneADelegate(Source As System.IntPtr, lpHalftone As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetHalftoneWDelegate(Source As System.IntPtr, lpHalftone As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetHighlightDelegate(Source As System.IntPtr, Highlight As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetHighlightStringDelegate(Source As System.IntPtr, Highlight As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetHighlightStringADelegate(Source As System.IntPtr, Highlight As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetHighlightStringWDelegate(Source As System.IntPtr, Highlight As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetJobControlDelegate(Source As System.IntPtr, JobControl As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetJpegValuesDelegate(Source As System.IntPtr, Quality As Integer, Progressive As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetJpegXRValuesDelegate(Source As System.IntPtr, Quality As Integer, Progressive As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLanguageDelegate(nLanguage As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLastErrorDelegate(nError As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLightPathDelegate(Source As System.IntPtr, LightPath As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLightPathExDelegate(Source As System.IntPtr, LightPaths As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLightSourceDelegate(Source As System.IntPtr, LightSource As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLightSourcesDelegate(Source As System.IntPtr, LightSources As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLoggerCallbackDelegate(logProc As DTwainLoggerProc, UserData As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLoggerCallbackADelegate(logProc As DTwainLoggerProcA, UserData As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetLoggerCallbackWDelegate(logProc As DTwainLoggerProcW, UserData As System.Int64) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetManualDuplexModeDelegate(Source As System.IntPtr, Flags As Integer, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetMaxAcquisitionsDelegate(Source As System.IntPtr, MaxAcquires As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetMaxBuffersDelegate(Source As System.IntPtr, MaxBuf As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetMaxRetryAttemptsDelegate(Source As System.IntPtr, nAttempts As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetMultipageScanModeDelegate(Source As System.IntPtr, ScanType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetNoiseFilterDelegate(Source As System.IntPtr, NoiseFilter As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetOCRCapValuesDelegate(Engine As System.IntPtr, OCRCapValue As Integer, SetType As Integer, CapValues As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetOrientationDelegate(Source As System.IntPtr, Orient As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetOverscanDelegate(Source As System.IntPtr, Value As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFAESEncryptionDelegate(Source As System.IntPtr, nWhichEncryption As Integer, bUseAES As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFASCIICompressionDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFAuthorDelegate(Source As System.IntPtr, lpAuthor As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFAuthorADelegate(Source As System.IntPtr, lpAuthor As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFAuthorWDelegate(Source As System.IntPtr, lpAuthor As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFCompressionDelegate(Source As System.IntPtr, bCompression As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFCreatorDelegate(Source As System.IntPtr, lpCreator As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFCreatorADelegate(Source As System.IntPtr, lpCreator As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFCreatorWDelegate(Source As System.IntPtr, lpCreator As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFEncryptionDelegate(Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFEncryptionADelegate(Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFEncryptionWDelegate(Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFJpegQualityDelegate(Source As System.IntPtr, Quality As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFKeywordsDelegate(Source As System.IntPtr, lpKeyWords As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFKeywordsADelegate(Source As System.IntPtr, lpKeyWords As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFKeywordsWDelegate(Source As System.IntPtr, lpKeyWords As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFOCRConversionDelegate(Engine As System.IntPtr, PageType As Integer, FileType As Integer, PixelType As Integer, BitDepth As Integer, Options As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFOCRModeDelegate(Source As System.IntPtr, bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFOrientationDelegate(Source As System.IntPtr, lPOrientation As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFPageScaleDelegate(Source As System.IntPtr, nOptions As Integer, xScale As System.Double, yScale As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFPageScaleStringDelegate(Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFPageScaleStringADelegate(Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFPageScaleStringWDelegate(Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFPageSizeDelegate(Source As System.IntPtr, PageSize As Integer, CustomWidth As System.Double, CustomHeight As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFPageSizeStringDelegate(Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFPageSizeStringADelegate(Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFPageSizeStringWDelegate(Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFPolarityDelegate(Source As System.IntPtr, Polarity As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFProducerDelegate(Source As System.IntPtr, lpProducer As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFProducerADelegate(Source As System.IntPtr, lpProducer As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFProducerWDelegate(Source As System.IntPtr, lpProducer As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFSubjectDelegate(Source As System.IntPtr, lpSubject As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFSubjectADelegate(Source As System.IntPtr, lpSubject As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFSubjectWDelegate(Source As System.IntPtr, lpSubject As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFTextElementFloatDelegate(TextElement As System.IntPtr, val1 As System.Double, val2 As System.Double, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFTextElementLongDelegate(TextElement As System.IntPtr, val1 As Integer, val2 As Integer, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFTextElementStringDelegate(TextElement As System.IntPtr, val1 As String, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFTextElementStringADelegate(TextElement As System.IntPtr, szString As String, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFTextElementStringWDelegate(TextElement As System.IntPtr, szString As String, Flags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPDFTitleDelegate(Source As System.IntPtr, lpTitle As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPDFTitleADelegate(Source As System.IntPtr, lpTitle As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPDFTitleWDelegate(Source As System.IntPtr, lpTitle As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPaperSizeDelegate(Source As System.IntPtr, PaperSize As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPatchMaxPrioritiesDelegate(Source As System.IntPtr, nMaxSearchRetries As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPatchMaxRetriesDelegate(Source As System.IntPtr, nMaxRetries As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPatchPrioritiesDelegate(Source As System.IntPtr, SearchPriorities As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPatchSearchModeDelegate(Source As System.IntPtr, nSearchMode As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPatchTimeOutDelegate(Source As System.IntPtr, TimeOutValue As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPixelFlavorDelegate(Source As System.IntPtr, PixelFlavor As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPixelTypeDelegate(Source As System.IntPtr, PixelType As Integer, BitDepth As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPostScriptTitleDelegate(Source As System.IntPtr, szTitle As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPostScriptTitleADelegate(Source As System.IntPtr, szTitle As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPostScriptTitleWDelegate(Source As System.IntPtr, szTitle As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPostScriptTypeDelegate(Source As System.IntPtr, PSType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPrinterDelegate(Source As System.IntPtr, Printer As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPrinterExDelegate(Source As System.IntPtr, Printer As Integer, bCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPrinterStartNumberDelegate(Source As System.IntPtr, nStart As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPrinterStringModeDelegate(Source As System.IntPtr, PrinterMode As Integer, bSetCurrent As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPrinterStringsDelegate(Source As System.IntPtr, ArrayString As System.IntPtr, ByRef pNumStrings As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetPrinterSuffixStringDelegate(Source As System.IntPtr, Suffix As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetPrinterSuffixStringADelegate(Source As System.IntPtr, Suffix As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetPrinterSuffixStringWDelegate(Source As System.IntPtr, Suffix As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetQueryCapSupportDelegate(bSet As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetResolutionDelegate(Source As System.IntPtr, Resolution As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetResolutionStringDelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetResolutionStringADelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetResolutionStringWDelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetResourcePathDelegate(ResourcePath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetResourcePathADelegate(ResourcePath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetResourcePathWDelegate(ResourcePath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetRotationDelegate(Source As System.IntPtr, Rotation As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetRotationStringDelegate(Source As System.IntPtr, Rotation As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetRotationStringADelegate(Source As System.IntPtr, Rotation As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetRotationStringWDelegate(Source As System.IntPtr, Rotation As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetSaveFileNameDelegate(Source As System.IntPtr, fName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetSaveFileNameADelegate(Source As System.IntPtr, fName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetSaveFileNameWDelegate(Source As System.IntPtr, fName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetShadowDelegate(Source As System.IntPtr, Shadow As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetShadowStringDelegate(Source As System.IntPtr, Shadow As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetShadowStringADelegate(Source As System.IntPtr, Shadow As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetShadowStringWDelegate(Source As System.IntPtr, Shadow As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetSourceUnitDelegate(Source As System.IntPtr, Unit As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTIFFCompressTypeDelegate(Source As System.IntPtr, Setting As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTIFFInvertDelegate(Source As System.IntPtr, Setting As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTempFileDirectoryDelegate(szFilePath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetTempFileDirectoryADelegate(szFilePath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTempFileDirectoryExDelegate(szFilePath As String, CreationFlags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetTempFileDirectoryExADelegate(szFilePath As String, CreationFlags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetTempFileDirectoryExWDelegate(szFilePath As String, CreationFlags As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetTempFileDirectoryWDelegate(szFilePath As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetThresholdDelegate(Source As System.IntPtr, Threshold As System.Double, bSetBithDepthReduction As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetThresholdStringDelegate(Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetThresholdStringADelegate(Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetThresholdStringWDelegate(Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTwainDSMDelegate(DSMType As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTwainLogDelegate(LogFlags As UInteger, lpszLogFile As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetTwainLogADelegate(LogFlags As UInteger, lpszLogFile As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetTwainLogWDelegate(LogFlags As UInteger, lpszLogFile As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTwainModeDelegate(lAcquireMode As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetTwainTimeoutDelegate(milliseconds As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetUpdateDibProcDelegate(DibProc As DTwainDIBUpdateProc) As DTwainDIBUpdateProc
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetXResolutionDelegate(Source As System.IntPtr, xResolution As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetXResolutionStringDelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetXResolutionStringADelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetXResolutionStringWDelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetYResolutionDelegate(Source As System.IntPtr, yResolution As System.Double) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SetYResolutionStringDelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SetYResolutionStringADelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SetYResolutionStringWDelegate(Source As System.IntPtr, Resolution As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ShowUIOnlyDelegate(Source As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_ShutdownOCREngineDelegate(OCREngine As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SkipImageInfoErrorDelegate(Source As System.IntPtr, bSkip As Integer) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_StartThreadDelegate(DLLHandle As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_StartTwainSessionDelegate(hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_StartTwainSessionADelegate(hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_StartTwainSessionWDelegate(hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysDestroyDelegate() As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysInitializeDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysInitializeExDelegate(szINIPath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysInitializeEx2Delegate(szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SysInitializeEx2ADelegate(szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SysInitializeEx2WDelegate(szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SysInitializeExADelegate(szINIPath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SysInitializeExWDelegate(szINIPath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysInitializeLibDelegate(hInstance As System.IntPtr) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysInitializeLibExDelegate(hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysInitializeLibEx2Delegate(hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SysInitializeLibEx2ADelegate(hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SysInitializeLibEx2WDelegate(hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
        Public Delegate Function DTWAIN_SysInitializeLibExADelegate(hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
        Public Delegate Function DTWAIN_SysInitializeLibExWDelegate(hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_SysInitializeNoBlockingDelegate() As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_TestGetCapDelegate(Source As System.IntPtr, lCapability As Integer) As System.IntPtr
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_UnlockMemoryDelegate(h As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_UnlockMemoryExDelegate(h As System.IntPtr) As Integer
        
        <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Auto)>
        Public Delegate Function DTWAIN_UseMultipleThreadsDelegate(bSet As Integer) As Integer
        Public Function DTWAIN_AcquireAudioFile(Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lMaxClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireAudioFile(Source, lpszFile, lFileFlags, lMaxClips, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireAudioFileA(Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lNumClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireAudioFileA(Source, lpszFile, lFileFlags, lNumClips, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireAudioFileW(Source As System.IntPtr, lpszFile As String, lFileFlags As Integer, lNumClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireAudioFileW(Source, lpszFile, lFileFlags, lNumClips, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireAudioNative(Source As System.IntPtr, nMaxAudioClips As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        Return api.DTWAIN_AcquireAudioNative(Source, nMaxAudioClips, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireAudioNativeEx(Source As System.IntPtr, nMaxAudioClips As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireAudioNativeEx(Source, nMaxAudioClips, bShowUI, bCloseSource, Acquisitions, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireBuffered(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        Return api.DTWAIN_AcquireBuffered(Source, PixelType, nMaxPages, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireBufferedEx(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireBufferedEx(Source, PixelType, nMaxPages, bShowUI, bCloseSource, Acquisitions, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireFile(Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireFile(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireFileA(Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireFileA(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireFileEx(Source As System.IntPtr, aFileNames As System.IntPtr, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireFileEx(Source, aFileNames, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireFileW(Source As System.IntPtr, lpszFile As String, lFileType As Integer, lFileFlags As Integer, PixelType As Integer, lMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireFileW(Source, lpszFile, lFileType, lFileFlags, PixelType, lMaxPages, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireNative(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        Return api.DTWAIN_AcquireNative(Source, PixelType, nMaxPages, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireNativeEx(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, bShowUI As Integer, bCloseSource As Integer, Acquisitions As System.IntPtr, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_AcquireNativeEx(Source, PixelType, nMaxPages, bShowUI, bCloseSource, Acquisitions, pStatus)
        End Function
        
        Public Function DTWAIN_AcquireToClipboard(Source As System.IntPtr, PixelType As Integer, nMaxPages As Integer, nTransferMode As Integer, bDiscardDibs As Integer, bShowUI As Integer, bCloseSource As Integer, ByRef pStatus As Integer) As System.IntPtr
        Return api.DTWAIN_AcquireToClipboard(Source, PixelType, nMaxPages, nTransferMode, bDiscardDibs, bShowUI, bCloseSource, pStatus)
        End Function
        
        Public Function DTWAIN_AddExtImageInfoQuery(Source As System.IntPtr, ExtImageInfo As Integer) As Integer
        Return api.DTWAIN_AddExtImageInfoQuery(Source, ExtImageInfo)
        End Function
        
        Public Function DTWAIN_AddFileToAppend(szFile As String) As Integer
        Return api.DTWAIN_AddFileToAppend(szFile)
        End Function
        
        Public Function DTWAIN_AddFileToAppendA(szFile As String) As Integer
        Return api.DTWAIN_AddFileToAppendA(szFile)
        End Function
        
        Public Function DTWAIN_AddFileToAppendW(szFile As String) As Integer
        Return api.DTWAIN_AddFileToAppendW(szFile)
        End Function
        
        Public Function DTWAIN_AddPDFText(Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
        Return api.DTWAIN_AddPDFText(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags)
        End Function
        
        Public Function DTWAIN_AddPDFTextA(Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
        Return api.DTWAIN_AddPDFTextA(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags)
        End Function
        
        Public Function DTWAIN_AddPDFTextEx(Source As System.IntPtr, TextElement As System.IntPtr, Flags As UInteger) As Integer
        Return api.DTWAIN_AddPDFTextEx(Source, TextElement, Flags)
        End Function
        
        Public Function DTWAIN_AddPDFTextW(Source As System.IntPtr, szText As String, xPos As Integer, yPos As Integer, fontName As String, fontSize As System.Double, colorRGB As Integer, renderMode As Integer, scaling As System.Double, charSpacing As System.Double, wordSpacing As System.Double, strokeWidth As Integer, Flags As UInteger) As Integer
        Return api.DTWAIN_AddPDFTextW(Source, szText, xPos, yPos, fontName, fontSize, colorRGB, renderMode, scaling, charSpacing, wordSpacing, strokeWidth, Flags)
        End Function
        
        Public Function DTWAIN_AllocateMemory(memSize As UInteger) As System.IntPtr
        Return api.DTWAIN_AllocateMemory(memSize)
        End Function
        
        Public Function DTWAIN_AllocateMemory64(memSize As System.UInt64) As System.IntPtr
        Return api.DTWAIN_AllocateMemory64(memSize)
        End Function
        
        Public Function DTWAIN_AllocateMemoryEx(memSize As UInteger) As System.IntPtr
        Return api.DTWAIN_AllocateMemoryEx(memSize)
        End Function
        
        Public Function DTWAIN_AppHandlesExceptions(bSet As Integer) As Integer
        Return api.DTWAIN_AppHandlesExceptions(bSet)
        End Function
        
        Public Function DTWAIN_ArrayANSIStringToFloat(StringArray As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayANSIStringToFloat(StringArray)
        End Function
        
        Public Function DTWAIN_ArrayAdd(pArray As System.IntPtr, pVariant As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayAdd(pArray, pVariant)
        End Function
        
        Public Function DTWAIN_ArrayAddANSIString(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddANSIString(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddANSIStringN(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddANSIStringN(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddFloat(pArray As System.IntPtr, Val As System.Double) As Integer
        Return api.DTWAIN_ArrayAddFloat(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddFloatN(pArray As System.IntPtr, Val As System.Double, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddFloatN(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddFloatString(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddFloatString(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddFloatStringA(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddFloatStringA(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddFloatStringN(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddFloatStringN(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddFloatStringNA(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddFloatStringNA(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddFloatStringNW(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddFloatStringNW(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddFloatStringW(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddFloatStringW(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddFrame(pArray As System.IntPtr, frame As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayAddFrame(pArray, frame)
        End Function
        
        Public Function DTWAIN_ArrayAddFrameN(pArray As System.IntPtr, frame As System.IntPtr, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddFrameN(pArray, frame, num)
        End Function
        
        Public Function DTWAIN_ArrayAddLong(pArray As System.IntPtr, Val As Integer) As Integer
        Return api.DTWAIN_ArrayAddLong(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddLong64(pArray As System.IntPtr, Val As System.Int64) As Integer
        Return api.DTWAIN_ArrayAddLong64(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddLong64N(pArray As System.IntPtr, Val As System.Int64, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddLong64N(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddLongN(pArray As System.IntPtr, Val As Integer, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddLongN(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddN(pArray As System.IntPtr, pVariant As System.IntPtr, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddN(pArray, pVariant, num)
        End Function
        
        Public Function DTWAIN_ArrayAddString(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddString(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddStringA(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddStringA(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddStringN(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddStringN(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddStringNA(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddStringNA(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddStringNW(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddStringNW(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayAddStringW(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddStringW(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddWideString(pArray As System.IntPtr, Val As String) As Integer
        Return api.DTWAIN_ArrayAddWideString(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayAddWideStringN(pArray As System.IntPtr, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayAddWideStringN(pArray, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayConvertFix32ToFloat(Fix32Array As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayConvertFix32ToFloat(Fix32Array)
        End Function
        
        Public Function DTWAIN_ArrayConvertFloatToFix32(FloatArray As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayConvertFloatToFix32(FloatArray)
        End Function
        
        Public Function DTWAIN_ArrayCopy(Source As System.IntPtr, Dest As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayCopy(Source, Dest)
        End Function
        
        Public Function DTWAIN_ArrayCreate(nEnumType As Integer, nInitialSize As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayCreate(nEnumType, nInitialSize)
        End Function
        
        Public Function DTWAIN_ArrayCreateCopy(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayCreateCopy(Source)
        End Function
        
        Public Function DTWAIN_ArrayCreateFromCap(Source As System.IntPtr, lCapType As Integer, lSize As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayCreateFromCap(Source, lCapType, lSize)
        End Function
        
        Public Function DTWAIN_ArrayCreateFromLong64s(ByRef pCArray As System.Int64, nSize As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayCreateFromLong64s(pCArray, nSize)
        End Function
        
        Public Function DTWAIN_ArrayCreateFromLongs(ByRef pCArray As Integer, nSize As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayCreateFromLongs(pCArray, nSize)
        End Function
        
        Public Function DTWAIN_ArrayCreateFromReals(ByRef pCArray As System.Double, nSize As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayCreateFromReals(pCArray, nSize)
        End Function
        
        Public Function DTWAIN_ArrayDestroy(pArray As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayDestroy(pArray)
        End Function
        
        Public Function DTWAIN_ArrayDestroyFrames(FrameArray As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayDestroyFrames(FrameArray)
        End Function
        
        Public Function DTWAIN_ArrayFind(pArray As System.IntPtr, pVariant As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayFind(pArray, pVariant)
        End Function
        
        Public Function DTWAIN_ArrayFindANSIString(pArray As System.IntPtr, pString As String) As Integer
        Return api.DTWAIN_ArrayFindANSIString(pArray, pString)
        End Function
        
        Public Function DTWAIN_ArrayFindFloat(pArray As System.IntPtr, Val As System.Double, Tolerance As System.Double) As Integer
        Return api.DTWAIN_ArrayFindFloat(pArray, Val, Tolerance)
        End Function
        
        Public Function DTWAIN_ArrayFindFloatString(pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
        Return api.DTWAIN_ArrayFindFloatString(pArray, Val, Tolerance)
        End Function
        
        Public Function DTWAIN_ArrayFindFloatStringA(pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
        Return api.DTWAIN_ArrayFindFloatStringA(pArray, Val, Tolerance)
        End Function
        
        Public Function DTWAIN_ArrayFindFloatStringW(pArray As System.IntPtr, Val As String, Tolerance As String) As Integer
        Return api.DTWAIN_ArrayFindFloatStringW(pArray, Val, Tolerance)
        End Function
        
        Public Function DTWAIN_ArrayFindLong(pArray As System.IntPtr, Val As Integer) As Integer
        Return api.DTWAIN_ArrayFindLong(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayFindLong64(pArray As System.IntPtr, Val As System.Int64) As Integer
        Return api.DTWAIN_ArrayFindLong64(pArray, Val)
        End Function
        
        Public Function DTWAIN_ArrayFindString(pArray As System.IntPtr, pString As String) As Integer
        Return api.DTWAIN_ArrayFindString(pArray, pString)
        End Function
        
        Public Function DTWAIN_ArrayFindStringA(pArray As System.IntPtr, pString As String) As Integer
        Return api.DTWAIN_ArrayFindStringA(pArray, pString)
        End Function
        
        Public Function DTWAIN_ArrayFindStringW(pArray As System.IntPtr, pString As String) As Integer
        Return api.DTWAIN_ArrayFindStringW(pArray, pString)
        End Function
        
        Public Function DTWAIN_ArrayFindWideString(pArray As System.IntPtr, pString As String) As Integer
        Return api.DTWAIN_ArrayFindWideString(pArray, pString)
        End Function
        
        Public Function DTWAIN_ArrayFix32GetAt(aFix32 As System.IntPtr, lPos As Integer, ByRef Whole As Integer, ByRef Frac As Integer) As Integer
        Return api.DTWAIN_ArrayFix32GetAt(aFix32, lPos, Whole, Frac)
        End Function
        
        Public Function DTWAIN_ArrayFix32SetAt(aFix32 As System.IntPtr, lPos As Integer, Whole As Integer, Frac As Integer) As Integer
        Return api.DTWAIN_ArrayFix32SetAt(aFix32, lPos, Whole, Frac)
        End Function
        
        Public Function DTWAIN_ArrayFloatToANSIString(FloatArray As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayFloatToANSIString(FloatArray)
        End Function
        
        Public Function DTWAIN_ArrayFloatToString(FloatArray As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayFloatToString(FloatArray)
        End Function
        
        Public Function DTWAIN_ArrayFloatToWideString(FloatArray As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayFloatToWideString(FloatArray)
        End Function
        
        Public Function DTWAIN_ArrayGetAt(pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayGetAt(pArray, nWhere, pVariant)
        End Function
        
        Public Function DTWAIN_ArrayGetAtANSIString(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> pStr As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtANSIString(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFloat(pArray As System.IntPtr, nWhere As Integer, ByRef pVal As System.Double) As Integer
        Return api.DTWAIN_ArrayGetAtFloat(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFloatString(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> Val As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtFloatString(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFloatStringA(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> Val As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtFloatStringA(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFloatStringW(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> Val As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtFloatStringW(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFrame(FrameArray As System.IntPtr, nWhere As Integer, ByRef pleft As System.Double, ByRef ptop As System.Double, ByRef pright As System.Double, ByRef pbottom As System.Double) As Integer
        Return api.DTWAIN_ArrayGetAtFrame(FrameArray, nWhere, pleft, ptop, pright, pbottom)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFrameEx(FrameArray As System.IntPtr, nWhere As Integer, Frame As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayGetAtFrameEx(FrameArray, nWhere, Frame)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFrameString(FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> bottom As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtFrameString(FrameArray, nWhere, left, top, right, bottom)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFrameStringA(FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> bottom As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtFrameStringA(FrameArray, nWhere, left, top, right, bottom)
        End Function
        
        Public Function DTWAIN_ArrayGetAtFrameStringW(FrameArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> bottom As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtFrameStringW(FrameArray, nWhere, left, top, right, bottom)
        End Function
        
        Public Function DTWAIN_ArrayGetAtLong(pArray As System.IntPtr, nWhere As Integer, ByRef pVal As Integer) As Integer
        Return api.DTWAIN_ArrayGetAtLong(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayGetAtLong64(pArray As System.IntPtr, nWhere As Integer, ByRef pVal As System.Int64) As Integer
        Return api.DTWAIN_ArrayGetAtLong64(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayGetAtSource(pArray As System.IntPtr, nWhere As Integer, ByRef ppSource As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayGetAtSource(pArray, nWhere, ppSource)
        End Function
        
        Public Function DTWAIN_ArrayGetAtString(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPTStr)> pStr As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtString(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArrayGetAtStringA(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPStr)> pStr As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtStringA(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArrayGetAtStringW(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> pStr As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtStringW(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArrayGetAtWideString(pArray As System.IntPtr, nWhere As Integer, <MarshalAs(UnmanagedType.LPWStr)> pStr As StringBuilder) As Integer
        Return api.DTWAIN_ArrayGetAtWideString(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArrayGetBuffer(pArray As System.IntPtr, nPos As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayGetBuffer(pArray, nPos)
        End Function
        
        Public Function DTWAIN_ArrayGetCapValues(Source As System.IntPtr, lCap As Integer, lGetType As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayGetCapValues(Source, lCap, lGetType)
        End Function
        
        Public Function DTWAIN_ArrayGetCapValuesEx(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayGetCapValuesEx(Source, lCap, lGetType, lContainerType)
        End Function
        
        Public Function DTWAIN_ArrayGetCapValuesEx2(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, nDataType As Integer) As System.IntPtr
        Return api.DTWAIN_ArrayGetCapValuesEx2(Source, lCap, lGetType, lContainerType, nDataType)
        End Function
        
        Public Function DTWAIN_ArrayGetCount(pArray As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayGetCount(pArray)
        End Function
        
        Public Function DTWAIN_ArrayGetMaxStringLength(a As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayGetMaxStringLength(a)
        End Function
        
        Public Function DTWAIN_ArrayGetSourceAt(pArray As System.IntPtr, nWhere As Integer, ByRef ppSource As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayGetSourceAt(pArray, nWhere, ppSource)
        End Function
        
        Public Function DTWAIN_ArrayGetStringLength(a As System.IntPtr, nWhichString As Integer) As Integer
        Return api.DTWAIN_ArrayGetStringLength(a, nWhichString)
        End Function
        
        Public Function DTWAIN_ArrayGetType(pArray As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayGetType(pArray)
        End Function
        
        Public Function DTWAIN_ArrayInit() As System.IntPtr
        Return api.DTWAIN_ArrayInit()
        End Function
        
        Public Function DTWAIN_ArrayInsertAt(pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayInsertAt(pArray, nWhere, pVariant)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtANSIString(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        Return api.DTWAIN_ArrayInsertAtANSIString(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtANSIStringN(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtANSIStringN(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloat(pArray As System.IntPtr, nWhere As Integer, pVal As System.Double) As Integer
        Return api.DTWAIN_ArrayInsertAtFloat(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloatN(pArray As System.IntPtr, nWhere As Integer, Val As System.Double, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtFloatN(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloatString(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        Return api.DTWAIN_ArrayInsertAtFloatString(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloatStringA(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        Return api.DTWAIN_ArrayInsertAtFloatStringA(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloatStringN(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtFloatStringN(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloatStringNA(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtFloatStringNA(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloatStringNW(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtFloatStringNW(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFloatStringW(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        Return api.DTWAIN_ArrayInsertAtFloatStringW(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFrame(pArray As System.IntPtr, nWhere As Integer, frame As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayInsertAtFrame(pArray, nWhere, frame)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtFrameN(pArray As System.IntPtr, nWhere As Integer, frame As System.IntPtr, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtFrameN(pArray, nWhere, frame, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtLong(pArray As System.IntPtr, nWhere As Integer, pVal As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtLong(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtLong64(pArray As System.IntPtr, nWhere As Integer, Val As System.Int64) As Integer
        Return api.DTWAIN_ArrayInsertAtLong64(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtLong64N(pArray As System.IntPtr, nWhere As Integer, Val As System.Int64, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtLong64N(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtLongN(pArray As System.IntPtr, nWhere As Integer, pVal As Integer, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtLongN(pArray, nWhere, pVal, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtN(pArray As System.IntPtr, nWhere As Integer, pVariant As System.IntPtr, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtN(pArray, nWhere, pVariant, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtString(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        Return api.DTWAIN_ArrayInsertAtString(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtStringA(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        Return api.DTWAIN_ArrayInsertAtStringA(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtStringN(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtStringN(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtStringNA(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtStringNA(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtStringNW(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtStringNW(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtStringW(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        Return api.DTWAIN_ArrayInsertAtStringW(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtWideString(pArray As System.IntPtr, nWhere As Integer, pVal As String) As Integer
        Return api.DTWAIN_ArrayInsertAtWideString(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArrayInsertAtWideStringN(pArray As System.IntPtr, nWhere As Integer, Val As String, num As Integer) As Integer
        Return api.DTWAIN_ArrayInsertAtWideStringN(pArray, nWhere, Val, num)
        End Function
        
        Public Function DTWAIN_ArrayRemoveAll(pArray As System.IntPtr) As Integer
        Return api.DTWAIN_ArrayRemoveAll(pArray)
        End Function
        
        Public Function DTWAIN_ArrayRemoveAt(pArray As System.IntPtr, nWhere As Integer) As Integer
        Return api.DTWAIN_ArrayRemoveAt(pArray, nWhere)
        End Function
        
        Public Function DTWAIN_ArrayRemoveAtN(pArray As System.IntPtr, nWhere As Integer, num As Integer) As Integer
        Return api.DTWAIN_ArrayRemoveAtN(pArray, nWhere, num)
        End Function
        
        Public Function DTWAIN_ArrayResize(pArray As System.IntPtr, NewSize As Integer) As Integer
        Return api.DTWAIN_ArrayResize(pArray, NewSize)
        End Function
        
        Public Function DTWAIN_ArraySetAt(pArray As System.IntPtr, lPos As Integer, pVariant As System.IntPtr) As Integer
        Return api.DTWAIN_ArraySetAt(pArray, lPos, pVariant)
        End Function
        
        Public Function DTWAIN_ArraySetAtANSIString(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        Return api.DTWAIN_ArraySetAtANSIString(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArraySetAtFloat(pArray As System.IntPtr, nWhere As Integer, pVal As System.Double) As Integer
        Return api.DTWAIN_ArraySetAtFloat(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArraySetAtFloatString(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        Return api.DTWAIN_ArraySetAtFloatString(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArraySetAtFloatStringA(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        Return api.DTWAIN_ArraySetAtFloatStringA(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArraySetAtFloatStringW(pArray As System.IntPtr, nWhere As Integer, Val As String) As Integer
        Return api.DTWAIN_ArraySetAtFloatStringW(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArraySetAtFrame(FrameArray As System.IntPtr, nWhere As Integer, left As System.Double, top As System.Double, right As System.Double, bottom As System.Double) As Integer
        Return api.DTWAIN_ArraySetAtFrame(FrameArray, nWhere, left, top, right, bottom)
        End Function
        
        Public Function DTWAIN_ArraySetAtFrameEx(FrameArray As System.IntPtr, nWhere As Integer, Frame As System.IntPtr) As Integer
        Return api.DTWAIN_ArraySetAtFrameEx(FrameArray, nWhere, Frame)
        End Function
        
        Public Function DTWAIN_ArraySetAtFrameString(FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
        Return api.DTWAIN_ArraySetAtFrameString(FrameArray, nWhere, left, top, right, bottom)
        End Function
        
        Public Function DTWAIN_ArraySetAtFrameStringA(FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
        Return api.DTWAIN_ArraySetAtFrameStringA(FrameArray, nWhere, left, top, right, bottom)
        End Function
        
        Public Function DTWAIN_ArraySetAtFrameStringW(FrameArray As System.IntPtr, nWhere As Integer, left As String, top As String, right As String, bottom As String) As Integer
        Return api.DTWAIN_ArraySetAtFrameStringW(FrameArray, nWhere, left, top, right, bottom)
        End Function
        
        Public Function DTWAIN_ArraySetAtLong(pArray As System.IntPtr, nWhere As Integer, pVal As Integer) As Integer
        Return api.DTWAIN_ArraySetAtLong(pArray, nWhere, pVal)
        End Function
        
        Public Function DTWAIN_ArraySetAtLong64(pArray As System.IntPtr, nWhere As Integer, Val As System.Int64) As Integer
        Return api.DTWAIN_ArraySetAtLong64(pArray, nWhere, Val)
        End Function
        
        Public Function DTWAIN_ArraySetAtString(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        Return api.DTWAIN_ArraySetAtString(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArraySetAtStringA(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        Return api.DTWAIN_ArraySetAtStringA(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArraySetAtStringW(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        Return api.DTWAIN_ArraySetAtStringW(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArraySetAtWideString(pArray As System.IntPtr, nWhere As Integer, pStr As String) As Integer
        Return api.DTWAIN_ArraySetAtWideString(pArray, nWhere, pStr)
        End Function
        
        Public Function DTWAIN_ArrayStringToFloat(StringArray As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayStringToFloat(StringArray)
        End Function
        
        Public Function DTWAIN_ArrayWideStringToFloat(StringArray As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ArrayWideStringToFloat(StringArray)
        End Function
        
        Public Function DTWAIN_CallCallback(wParam As Integer, lParam As Integer, UserData As Integer) As Integer
        Return api.DTWAIN_CallCallback(wParam, lParam, UserData)
        End Function
        
        Public Function DTWAIN_CallCallback64(wParam As Integer, lParam As Integer, UserData As System.Int64) As Integer
        Return api.DTWAIN_CallCallback64(wParam, lParam, UserData)
        End Function
        
        Public Function DTWAIN_CallDSMProc(AppID As System.IntPtr, SourceId As System.IntPtr, lDG As Integer, lDAT As Integer, lMSG As Integer, pData As System.IntPtr) As Integer
        Return api.DTWAIN_CallDSMProc(AppID, SourceId, lDG, lDAT, lMSG, pData)
        End Function
        
        Public Function DTWAIN_CheckHandles(bCheck As Integer) As Integer
        Return api.DTWAIN_CheckHandles(bCheck)
        End Function
        
        Public Function DTWAIN_ClearBuffers(Source As System.IntPtr, ClearBuffer As Integer) As Integer
        Return api.DTWAIN_ClearBuffers(Source, ClearBuffer)
        End Function
        
        Public Function DTWAIN_ClearErrorBuffer() As Integer
        Return api.DTWAIN_ClearErrorBuffer()
        End Function
        
        Public Function DTWAIN_ClearPDFText(Source As System.IntPtr) As Integer
        Return api.DTWAIN_ClearPDFText(Source)
        End Function
        
        Public Function DTWAIN_ClearPage(Source As System.IntPtr) As Integer
        Return api.DTWAIN_ClearPage(Source)
        End Function
        
        Public Function DTWAIN_CloseSource(Source As System.IntPtr) As Integer
        Return api.DTWAIN_CloseSource(Source)
        End Function
        
        Public Function DTWAIN_CloseSourceUI(Source As System.IntPtr) As Integer
        Return api.DTWAIN_CloseSourceUI(Source)
        End Function
        
        Public Function DTWAIN_ConvertDIBToBitmap(hDib As System.IntPtr, hPalette As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_ConvertDIBToBitmap(hDib, hPalette)
        End Function
        
        Public Function DTWAIN_ConvertDIBToFullBitmap(hDib As System.IntPtr, isBMP As Integer) As System.IntPtr
        Return api.DTWAIN_ConvertDIBToFullBitmap(hDib, isBMP)
        End Function
        
        Public Function DTWAIN_ConvertToAPIString(lpOrigString As String) As System.IntPtr
        Return api.DTWAIN_ConvertToAPIString(lpOrigString)
        End Function
        
        Public Function DTWAIN_ConvertToAPIStringA(lpOrigString As String) As System.IntPtr
        Return api.DTWAIN_ConvertToAPIStringA(lpOrigString)
        End Function
        
        Public Function DTWAIN_ConvertToAPIStringEx(lpOrigString As String, <MarshalAs(UnmanagedType.LPTStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_ConvertToAPIStringEx(lpOrigString, lpOutString, nSize)
        End Function
        
        Public Function DTWAIN_ConvertToAPIStringExA(lpOrigString As String, <MarshalAs(UnmanagedType.LPStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_ConvertToAPIStringExA(lpOrigString, lpOutString, nSize)
        End Function
        
        Public Function DTWAIN_ConvertToAPIStringExW(lpOrigString As String, <MarshalAs(UnmanagedType.LPWStr)> lpOutString As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_ConvertToAPIStringExW(lpOrigString, lpOutString, nSize)
        End Function
        
        Public Function DTWAIN_ConvertToAPIStringW(lpOrigString As String) As System.IntPtr
        Return api.DTWAIN_ConvertToAPIStringW(lpOrigString)
        End Function
        
        Public Function DTWAIN_CreateAcquisitionArray() As System.IntPtr
        Return api.DTWAIN_CreateAcquisitionArray()
        End Function
        
        Public Function DTWAIN_CreatePDFTextElement(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_CreatePDFTextElement(Source)
        End Function
        
        Public Function DTWAIN_DeleteDIB(hDib As System.IntPtr) As Integer
        Return api.DTWAIN_DeleteDIB(hDib)
        End Function
        
        Public Function DTWAIN_DestroyAcquisitionArray(aAcq As System.IntPtr, bDestroyData As Integer) As Integer
        Return api.DTWAIN_DestroyAcquisitionArray(aAcq, bDestroyData)
        End Function
        
        Public Function DTWAIN_DestroyPDFTextElement(TextElement As System.IntPtr) As Integer
        Return api.DTWAIN_DestroyPDFTextElement(TextElement)
        End Function
        
        Public Function DTWAIN_DisableAppWindow(hWnd As System.IntPtr, bDisable As Integer) As Integer
        Return api.DTWAIN_DisableAppWindow(hWnd, bDisable)
        End Function
        
        Public Function DTWAIN_EnableAutoBorderDetect(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnableAutoBorderDetect(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableAutoBright(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_EnableAutoBright(Source, bSet)
        End Function
        
        Public Function DTWAIN_EnableAutoDeskew(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnableAutoDeskew(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableAutoFeed(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_EnableAutoFeed(Source, bSet)
        End Function
        
        Public Function DTWAIN_EnableAutoRotate(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_EnableAutoRotate(Source, bSet)
        End Function
        
        Public Function DTWAIN_EnableAutoScan(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnableAutoScan(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableAutomaticSenseMedium(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_EnableAutomaticSenseMedium(Source, bSet)
        End Function
        
        Public Function DTWAIN_EnableDuplex(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnableDuplex(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableFeeder(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_EnableFeeder(Source, bSet)
        End Function
        
        Public Function DTWAIN_EnableIndicator(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnableIndicator(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableJobFileHandling(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_EnableJobFileHandling(Source, bSet)
        End Function
        
        Public Function DTWAIN_EnableLamp(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnableLamp(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableMsgNotify(bSet As Integer) As Integer
        Return api.DTWAIN_EnableMsgNotify(bSet)
        End Function
        
        Public Function DTWAIN_EnablePatchDetect(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnablePatchDetect(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnablePeekMessageLoop(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_EnablePeekMessageLoop(Source, bSet)
        End Function
        
        Public Function DTWAIN_EnablePrinter(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnablePrinter(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableThumbnail(Source As System.IntPtr, bEnable As Integer) As Integer
        Return api.DTWAIN_EnableThumbnail(Source, bEnable)
        End Function
        
        Public Function DTWAIN_EnableTripletsNotify(bSet As Integer) As Integer
        Return api.DTWAIN_EnableTripletsNotify(bSet)
        End Function
        
        Public Function DTWAIN_EndThread(DLLHandle As System.IntPtr) As Integer
        Return api.DTWAIN_EndThread(DLLHandle)
        End Function
        
        Public Function DTWAIN_EndTwainSession() As Integer
        Return api.DTWAIN_EndTwainSession()
        End Function
        
        Public Function DTWAIN_EnumAlarmVolumes(Source As System.IntPtr, ByRef pArray As System.IntPtr, expandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumAlarmVolumes(Source, pArray, expandIfRange)
        End Function
        
        Public Function DTWAIN_EnumAlarmVolumesEx(Source As System.IntPtr, expandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumAlarmVolumesEx(Source, expandIfRange)
        End Function
        
        Public Function DTWAIN_EnumAlarms(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumAlarms(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumAlarmsEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumAlarmsEx(Source)
        End Function
        
        Public Function DTWAIN_EnumAudioXferMechs(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumAudioXferMechs(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumAudioXferMechsEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumAudioXferMechsEx(Source)
        End Function
        
        Public Function DTWAIN_EnumAutoFeedValues(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumAutoFeedValues(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumAutoFeedValuesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumAutoFeedValuesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumAutomaticCaptures(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumAutomaticCaptures(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumAutomaticCapturesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumAutomaticCapturesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumAutomaticSenseMedium(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumAutomaticSenseMedium(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumAutomaticSenseMediumEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumAutomaticSenseMediumEx(Source)
        End Function
        
        Public Function DTWAIN_EnumBitDepths(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumBitDepths(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumBitDepthsEx(Source As System.IntPtr, PixelType As Integer, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumBitDepthsEx(Source, PixelType, pArray)
        End Function
        
        Public Function DTWAIN_EnumBitDepthsEx2(Source As System.IntPtr, PixelType As Integer) As System.IntPtr
        Return api.DTWAIN_EnumBitDepthsEx2(Source, PixelType)
        End Function
        
        Public Function DTWAIN_EnumBottomCameras(Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
        Return api.DTWAIN_EnumBottomCameras(Source, Cameras)
        End Function
        
        Public Function DTWAIN_EnumBottomCamerasEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumBottomCamerasEx(Source)
        End Function
        
        Public Function DTWAIN_EnumBrightnessValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumBrightnessValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumBrightnessValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumBrightnessValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumCameras(Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
        Return api.DTWAIN_EnumCameras(Source, Cameras)
        End Function
        
        Public Function DTWAIN_EnumCamerasEx(Source As System.IntPtr, nWhichCamera As Integer, ByRef Cameras As System.IntPtr) As Integer
        Return api.DTWAIN_EnumCamerasEx(Source, nWhichCamera, Cameras)
        End Function
        
        Public Function DTWAIN_EnumCamerasEx2(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumCamerasEx2(Source)
        End Function
        
        Public Function DTWAIN_EnumCamerasEx3(Source As System.IntPtr, nWhichCamera As Integer) As System.IntPtr
        Return api.DTWAIN_EnumCamerasEx3(Source, nWhichCamera)
        End Function
        
        Public Function DTWAIN_EnumCompressionTypes(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumCompressionTypes(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumCompressionTypesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumCompressionTypesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumCompressionTypesEx2(Source As System.IntPtr, lFileType As Integer, bUseBufferedMode As Integer) As System.IntPtr
        Return api.DTWAIN_EnumCompressionTypesEx2(Source, lFileType, bUseBufferedMode)
        End Function
        
        Public Function DTWAIN_EnumContrastValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumContrastValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumContrastValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumContrastValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumCustomCaps(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumCustomCaps(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumCustomCapsEx2(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumCustomCapsEx2(Source)
        End Function
        
        Public Function DTWAIN_EnumDoubleFeedDetectLengths(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumDoubleFeedDetectLengths(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumDoubleFeedDetectLengthsEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumDoubleFeedDetectLengthsEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumDoubleFeedDetectValues(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumDoubleFeedDetectValues(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumDoubleFeedDetectValuesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumDoubleFeedDetectValuesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumExtImageInfoTypes(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumExtImageInfoTypes(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumExtImageInfoTypesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumExtImageInfoTypesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumExtendedCaps(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumExtendedCaps(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumExtendedCapsEx(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumExtendedCapsEx(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumExtendedCapsEx2(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumExtendedCapsEx2(Source)
        End Function
        
        Public Function DTWAIN_EnumFileTypeBitsPerPixel(FileType As Integer, ByRef Array As System.IntPtr) As Integer
        Return api.DTWAIN_EnumFileTypeBitsPerPixel(FileType, Array)
        End Function
        
        Public Function DTWAIN_EnumFileXferFormats(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumFileXferFormats(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumFileXferFormatsEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumFileXferFormatsEx(Source)
        End Function
        
        Public Function DTWAIN_EnumHalftones(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumHalftones(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumHalftonesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumHalftonesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumHighlightValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumHighlightValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumHighlightValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumHighlightValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumJobControls(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumJobControls(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumJobControlsEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumJobControlsEx(Source)
        End Function
        
        Public Function DTWAIN_EnumLightPaths(Source As System.IntPtr, ByRef LightPath As System.IntPtr) As Integer
        Return api.DTWAIN_EnumLightPaths(Source, LightPath)
        End Function
        
        Public Function DTWAIN_EnumLightPathsEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumLightPathsEx(Source)
        End Function
        
        Public Function DTWAIN_EnumLightSources(Source As System.IntPtr, ByRef LightSources As System.IntPtr) As Integer
        Return api.DTWAIN_EnumLightSources(Source, LightSources)
        End Function
        
        Public Function DTWAIN_EnumLightSourcesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumLightSourcesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumMaxBuffers(Source As System.IntPtr, ByRef pMaxBufs As System.IntPtr, bExpandRange As Integer) As Integer
        Return api.DTWAIN_EnumMaxBuffers(Source, pMaxBufs, bExpandRange)
        End Function
        
        Public Function DTWAIN_EnumMaxBuffersEx(Source As System.IntPtr, bExpandRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumMaxBuffersEx(Source, bExpandRange)
        End Function
        
        Public Function DTWAIN_EnumNoiseFilters(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumNoiseFilters(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumNoiseFiltersEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumNoiseFiltersEx(Source)
        End Function
        
        Public Function DTWAIN_EnumOCRInterfaces(ByRef OCRInterfaces As System.IntPtr) As Integer
        Return api.DTWAIN_EnumOCRInterfaces(OCRInterfaces)
        End Function
        
        Public Function DTWAIN_EnumOCRSupportedCaps(Engine As System.IntPtr, ByRef SupportedCaps As System.IntPtr) As Integer
        Return api.DTWAIN_EnumOCRSupportedCaps(Engine, SupportedCaps)
        End Function
        
        Public Function DTWAIN_EnumOrientations(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumOrientations(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumOrientationsEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumOrientationsEx(Source)
        End Function
        
        Public Function DTWAIN_EnumOverscanValues(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumOverscanValues(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumOverscanValuesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumOverscanValuesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPaperSizes(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPaperSizes(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPaperSizesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPaperSizesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPatchCodes(Source As System.IntPtr, ByRef PCodes As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPatchCodes(Source, PCodes)
        End Function
        
        Public Function DTWAIN_EnumPatchCodesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPatchCodesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPatchMaxPriorities(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPatchMaxPriorities(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPatchMaxPrioritiesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPatchMaxPrioritiesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPatchMaxRetries(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPatchMaxRetries(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPatchMaxRetriesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPatchMaxRetriesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPatchPriorities(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPatchPriorities(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPatchPrioritiesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPatchPrioritiesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPatchSearchModes(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPatchSearchModes(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPatchSearchModesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPatchSearchModesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPatchTimeOutValues(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPatchTimeOutValues(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPatchTimeOutValuesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPatchTimeOutValuesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPixelTypes(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPixelTypes(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPixelTypesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPixelTypesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumPrinterStringModes(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumPrinterStringModes(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumPrinterStringModesEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumPrinterStringModesEx(Source)
        End Function
        
        Public Function DTWAIN_EnumResolutionValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumResolutionValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumResolutionValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumResolutionValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumShadowValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumShadowValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumShadowValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumShadowValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumSourceUnits(Source As System.IntPtr, ByRef lpArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumSourceUnits(Source, lpArray)
        End Function
        
        Public Function DTWAIN_EnumSourceUnitsEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumSourceUnitsEx(Source)
        End Function
        
        Public Function DTWAIN_EnumSourceValues(Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumSourceValues(Source, capName, values, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumSourceValuesA(Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumSourceValuesA(Source, capName, values, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumSourceValuesW(Source As System.IntPtr, capName As String, ByRef values As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumSourceValuesW(Source, capName, values, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumSources(ByRef lpArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumSources(lpArray)
        End Function
        
        Public Function DTWAIN_EnumSourcesEx() As System.IntPtr
        Return api.DTWAIN_EnumSourcesEx()
        End Function
        
        Public Function DTWAIN_EnumSupportedCaps(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumSupportedCaps(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumSupportedCapsEx(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumSupportedCapsEx(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumSupportedCapsEx2(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumSupportedCapsEx2(Source)
        End Function
        
        Public Function DTWAIN_EnumSupportedExtImageInfo(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumSupportedExtImageInfo(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumSupportedExtImageInfoEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumSupportedExtImageInfoEx(Source)
        End Function
        
        Public Function DTWAIN_EnumSupportedFileTypes() As System.IntPtr
        Return api.DTWAIN_EnumSupportedFileTypes()
        End Function
        
        Public Function DTWAIN_EnumSupportedMultiPageFileTypes() As System.IntPtr
        Return api.DTWAIN_EnumSupportedMultiPageFileTypes()
        End Function
        
        Public Function DTWAIN_EnumSupportedSinglePageFileTypes() As System.IntPtr
        Return api.DTWAIN_EnumSupportedSinglePageFileTypes()
        End Function
        
        Public Function DTWAIN_EnumThresholdValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumThresholdValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumThresholdValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumThresholdValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumTopCameras(Source As System.IntPtr, ByRef Cameras As System.IntPtr) As Integer
        Return api.DTWAIN_EnumTopCameras(Source, Cameras)
        End Function
        
        Public Function DTWAIN_EnumTopCamerasEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumTopCamerasEx(Source)
        End Function
        
        Public Function DTWAIN_EnumTwainPrinters(Source As System.IntPtr, ByRef lpAvailPrinters As System.IntPtr) As Integer
        Return api.DTWAIN_EnumTwainPrinters(Source, lpAvailPrinters)
        End Function
        
        Public Function DTWAIN_EnumTwainPrintersArray(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_EnumTwainPrintersArray(Source, pArray)
        End Function
        
        Public Function DTWAIN_EnumTwainPrintersArrayEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumTwainPrintersArrayEx(Source)
        End Function
        
        Public Function DTWAIN_EnumTwainPrintersEx(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_EnumTwainPrintersEx(Source)
        End Function
        
        Public Function DTWAIN_EnumXResolutionValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumXResolutionValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumXResolutionValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumXResolutionValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumYResolutionValues(Source As System.IntPtr, ByRef pArray As System.IntPtr, bExpandIfRange As Integer) As Integer
        Return api.DTWAIN_EnumYResolutionValues(Source, pArray, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_EnumYResolutionValuesEx(Source As System.IntPtr, bExpandIfRange As Integer) As System.IntPtr
        Return api.DTWAIN_EnumYResolutionValuesEx(Source, bExpandIfRange)
        End Function
        
        Public Function DTWAIN_ExecuteOCR(Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
        Return api.DTWAIN_ExecuteOCR(Engine, szFileName, nStartPage, nEndPage)
        End Function
        
        Public Function DTWAIN_ExecuteOCRA(Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
        Return api.DTWAIN_ExecuteOCRA(Engine, szFileName, nStartPage, nEndPage)
        End Function
        
        Public Function DTWAIN_ExecuteOCRW(Engine As System.IntPtr, szFileName As String, nStartPage As Integer, nEndPage As Integer) As Integer
        Return api.DTWAIN_ExecuteOCRW(Engine, szFileName, nStartPage, nEndPage)
        End Function
        
        Public Function DTWAIN_FeedPage(Source As System.IntPtr) As Integer
        Return api.DTWAIN_FeedPage(Source)
        End Function
        
        Public Function DTWAIN_FlipBitmap(hDib As System.IntPtr) As Integer
        Return api.DTWAIN_FlipBitmap(hDib)
        End Function
        
        Public Function DTWAIN_FlushAcquiredPages(Source As System.IntPtr) As Integer
        Return api.DTWAIN_FlushAcquiredPages(Source)
        End Function
        
        Public Function DTWAIN_ForceAcquireBitDepth(Source As System.IntPtr, BitDepth As Integer) As Integer
        Return api.DTWAIN_ForceAcquireBitDepth(Source, BitDepth)
        End Function
        
        Public Function DTWAIN_ForceScanOnNoUI(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_ForceScanOnNoUI(Source, bSet)
        End Function
        
        Public Function DTWAIN_FrameCreate(Left As System.Double, Top As System.Double, Right As System.Double, Bottom As System.Double) As System.IntPtr
        Return api.DTWAIN_FrameCreate(Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameCreateString(Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
        Return api.DTWAIN_FrameCreateString(Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameCreateStringA(Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
        Return api.DTWAIN_FrameCreateStringA(Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameCreateStringW(Left As String, Top As String, Right As String, Bottom As String) As System.IntPtr
        Return api.DTWAIN_FrameCreateStringW(Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameDestroy(Frame As System.IntPtr) As Integer
        Return api.DTWAIN_FrameDestroy(Frame)
        End Function
        
        Public Function DTWAIN_FrameGetAll(Frame As System.IntPtr, ByRef Left As System.Double, ByRef Top As System.Double, ByRef Right As System.Double, ByRef Bottom As System.Double) As Integer
        Return api.DTWAIN_FrameGetAll(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameGetAllString(Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> Bottom As StringBuilder) As Integer
        Return api.DTWAIN_FrameGetAllString(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameGetAllStringA(Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> Bottom As StringBuilder) As Integer
        Return api.DTWAIN_FrameGetAllStringA(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameGetAllStringW(Frame As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> Bottom As StringBuilder) As Integer
        Return api.DTWAIN_FrameGetAllStringW(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameGetValue(Frame As System.IntPtr, nWhich As Integer, ByRef Value As System.Double) As Integer
        Return api.DTWAIN_FrameGetValue(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FrameGetValueString(Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPTStr)> Value As StringBuilder) As Integer
        Return api.DTWAIN_FrameGetValueString(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FrameGetValueStringA(Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPStr)> Value As StringBuilder) As Integer
        Return api.DTWAIN_FrameGetValueStringA(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FrameGetValueStringW(Frame As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPWStr)> Value As StringBuilder) As Integer
        Return api.DTWAIN_FrameGetValueStringW(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FrameIsValid(Frame As System.IntPtr) As Integer
        Return api.DTWAIN_FrameIsValid(Frame)
        End Function
        
        Public Function DTWAIN_FrameSetAll(Frame As System.IntPtr, Left As System.Double, Top As System.Double, Right As System.Double, Bottom As System.Double) As Integer
        Return api.DTWAIN_FrameSetAll(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameSetAllString(Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
        Return api.DTWAIN_FrameSetAllString(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameSetAllStringA(Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
        Return api.DTWAIN_FrameSetAllStringA(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameSetAllStringW(Frame As System.IntPtr, Left As String, Top As String, Right As String, Bottom As String) As Integer
        Return api.DTWAIN_FrameSetAllStringW(Frame, Left, Top, Right, Bottom)
        End Function
        
        Public Function DTWAIN_FrameSetValue(Frame As System.IntPtr, nWhich As Integer, Value As System.Double) As Integer
        Return api.DTWAIN_FrameSetValue(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FrameSetValueString(Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
        Return api.DTWAIN_FrameSetValueString(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FrameSetValueStringA(Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
        Return api.DTWAIN_FrameSetValueStringA(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FrameSetValueStringW(Frame As System.IntPtr, nWhich As Integer, Value As String) As Integer
        Return api.DTWAIN_FrameSetValueStringW(Frame, nWhich, Value)
        End Function
        
        Public Function DTWAIN_FreeExtImageInfo(Source As System.IntPtr) As Integer
        Return api.DTWAIN_FreeExtImageInfo(Source)
        End Function
        
        Public Function DTWAIN_FreeMemory(h As System.IntPtr) As Integer
        Return api.DTWAIN_FreeMemory(h)
        End Function
        
        Public Function DTWAIN_FreeMemoryEx(h As System.IntPtr) As Integer
        Return api.DTWAIN_FreeMemoryEx(h)
        End Function
        
        Public Function DTWAIN_GetAPIHandleStatus(pHandle As System.IntPtr) As Integer
        Return api.DTWAIN_GetAPIHandleStatus(pHandle)
        End Function
        
        Public Function DTWAIN_GetAcquireArea(Source As System.IntPtr, lGetType As Integer, ByRef FloatEnum As System.IntPtr) As Integer
        Return api.DTWAIN_GetAcquireArea(Source, lGetType, FloatEnum)
        End Function
        
        Public Function DTWAIN_GetAcquireArea2(Source As System.IntPtr, ByRef left As System.Double, ByRef top As System.Double, ByRef right As System.Double, ByRef bottom As System.Double, ByRef lpUnit As Integer) As Integer
        Return api.DTWAIN_GetAcquireArea2(Source, left, top, right, bottom, lpUnit)
        End Function
        
        Public Function DTWAIN_GetAcquireArea2String(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
        Return api.DTWAIN_GetAcquireArea2String(Source, left, top, right, bottom, Unit)
        End Function
        
        Public Function DTWAIN_GetAcquireArea2StringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
        Return api.DTWAIN_GetAcquireArea2StringA(Source, left, top, right, bottom, Unit)
        End Function
        
        Public Function DTWAIN_GetAcquireArea2StringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> left As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> top As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> right As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> bottom As StringBuilder, ByRef Unit As Integer) As Integer
        Return api.DTWAIN_GetAcquireArea2StringW(Source, left, top, right, bottom, Unit)
        End Function
        
        Public Function DTWAIN_GetAcquireAreaEx(Source As System.IntPtr, lGetType As Integer) As System.IntPtr
        Return api.DTWAIN_GetAcquireAreaEx(Source, lGetType)
        End Function
        
        Public Function DTWAIN_GetAcquireMetrics(source As System.IntPtr, ByRef ImageCount As Integer, ByRef SheetCount As Integer) As Integer
        Return api.DTWAIN_GetAcquireMetrics(source, ImageCount, SheetCount)
        End Function
        
        Public Function DTWAIN_GetAcquireStripBuffer(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_GetAcquireStripBuffer(Source)
        End Function
        
        Public Function DTWAIN_GetAcquireStripData(Source As System.IntPtr, ByRef lpCompression As Integer, ByRef lpBytesPerRow As UInteger, ByRef lpColumns As UInteger, ByRef lpRows As UInteger, ByRef XOffset As UInteger, ByRef YOffset As UInteger, ByRef lpBytesWritten As UInteger) As Integer
        Return api.DTWAIN_GetAcquireStripData(Source, lpCompression, lpBytesPerRow, lpColumns, lpRows, XOffset, YOffset, lpBytesWritten)
        End Function
        
        Public Function DTWAIN_GetAcquireStripSizes(Source As System.IntPtr, ByRef lpMin As UInteger, ByRef lpMax As UInteger, ByRef lpPreferred As UInteger) As Integer
        Return api.DTWAIN_GetAcquireStripSizes(Source, lpMin, lpMax, lpPreferred)
        End Function
        
        Public Function DTWAIN_GetAcquiredImage(aAcq As System.IntPtr, nWhichAcq As Integer, nWhichDib As Integer) As System.IntPtr
        Return api.DTWAIN_GetAcquiredImage(aAcq, nWhichAcq, nWhichDib)
        End Function
        
        Public Function DTWAIN_GetAcquiredImageArray(aAcq As System.IntPtr, nWhichAcq As Integer) As System.IntPtr
        Return api.DTWAIN_GetAcquiredImageArray(aAcq, nWhichAcq)
        End Function
        
        Public Function DTWAIN_GetActiveDSMPath(<MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetActiveDSMPath(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetActiveDSMPathA(<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetActiveDSMPathA(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetActiveDSMPathW(<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetActiveDSMPathW(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetActiveDSMVersionInfo(<MarshalAs(UnmanagedType.LPTStr)> szDLLInfo As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetActiveDSMVersionInfo(szDLLInfo, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetActiveDSMVersionInfoA(<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetActiveDSMVersionInfoA(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetActiveDSMVersionInfoW(<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetActiveDSMVersionInfoW(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetAlarmVolume(Source As System.IntPtr, ByRef lpVolume As Integer) As Integer
        Return api.DTWAIN_GetAlarmVolume(Source, lpVolume)
        End Function
        
        Public Function DTWAIN_GetAllSourceDibs(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_GetAllSourceDibs(Source)
        End Function
        
        Public Function DTWAIN_GetAppInfo(<MarshalAs(UnmanagedType.LPTStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> szProdName As StringBuilder) As Integer
        Return api.DTWAIN_GetAppInfo(szVerStr, szManu, szProdFam, szProdName)
        End Function
        
        Public Function DTWAIN_GetAppInfoA(<MarshalAs(UnmanagedType.LPStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> szProdName As StringBuilder) As Integer
        Return api.DTWAIN_GetAppInfoA(szVerStr, szManu, szProdFam, szProdName)
        End Function
        
        Public Function DTWAIN_GetAppInfoW(<MarshalAs(UnmanagedType.LPWStr)> szVerStr As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szManu As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szProdFam As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> szProdName As StringBuilder) As Integer
        Return api.DTWAIN_GetAppInfoW(szVerStr, szManu, szProdFam, szProdName)
        End Function
        
        Public Function DTWAIN_GetAuthor(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szAuthor As StringBuilder) As Integer
        Return api.DTWAIN_GetAuthor(Source, szAuthor)
        End Function
        
        Public Function DTWAIN_GetAuthorA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szAuthor As StringBuilder) As Integer
        Return api.DTWAIN_GetAuthorA(Source, szAuthor)
        End Function
        
        Public Function DTWAIN_GetAuthorW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szAuthor As StringBuilder) As Integer
        Return api.DTWAIN_GetAuthorW(Source, szAuthor)
        End Function
        
        Public Function DTWAIN_GetBatteryMinutes(Source As System.IntPtr, ByRef lpMinutes As Integer) As Integer
        Return api.DTWAIN_GetBatteryMinutes(Source, lpMinutes)
        End Function
        
        Public Function DTWAIN_GetBatteryPercent(Source As System.IntPtr, ByRef lpPercent As Integer) As Integer
        Return api.DTWAIN_GetBatteryPercent(Source, lpPercent)
        End Function
        
        Public Function DTWAIN_GetBitDepth(Source As System.IntPtr, ByRef BitDepth As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetBitDepth(Source, BitDepth, bCurrent)
        End Function
        
        Public Function DTWAIN_GetBlankPageAutoDetection(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetBlankPageAutoDetection(Source)
        End Function
        
        Public Function DTWAIN_GetBrightness(Source As System.IntPtr, ByRef Brightness As System.Double) As Integer
        Return api.DTWAIN_GetBrightness(Source, Brightness)
        End Function
        
        Public Function DTWAIN_GetBrightnessString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Brightness As StringBuilder) As Integer
        Return api.DTWAIN_GetBrightnessString(Source, Brightness)
        End Function
        
        Public Function DTWAIN_GetBrightnessStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Contrast As StringBuilder) As Integer
        Return api.DTWAIN_GetBrightnessStringA(Source, Contrast)
        End Function
        
        Public Function DTWAIN_GetBrightnessStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Contrast As StringBuilder) As Integer
        Return api.DTWAIN_GetBrightnessStringW(Source, Contrast)
        End Function
        
        Public Function DTWAIN_GetBufferedTransferInfo(Source As System.IntPtr, ByRef Compression As UInteger, ByRef BytesPerRow As UInteger, ByRef Columns As UInteger, ByRef Rows As UInteger, ByRef XOffset As UInteger, ByRef YOffset As UInteger, ByRef Flags As UInteger, ByRef BytesWritten As UInteger, ByRef MemoryLength As UInteger) As System.IntPtr
        Return api.DTWAIN_GetBufferedTransferInfo(Source, Compression, BytesPerRow, Columns, Rows, XOffset, YOffset, Flags, BytesWritten, MemoryLength)
        End Function
        
        Public Function DTWAIN_GetCallback() As DTwainCallback
        Return api.DTWAIN_GetCallback()
        End Function
        
        Public Function DTWAIN_GetCallback64() As DTwainCallback64
        Return api.DTWAIN_GetCallback64()
        End Function
        
        Public Function DTWAIN_GetCapArrayType(Source As System.IntPtr, nCap As Integer) As Integer
        Return api.DTWAIN_GetCapArrayType(Source, nCap)
        End Function
        
        Public Function DTWAIN_GetCapContainer(Source As System.IntPtr, nCap As Integer, lCapType As Integer) As Integer
        Return api.DTWAIN_GetCapContainer(Source, nCap, lCapType)
        End Function
        
        Public Function DTWAIN_GetCapContainerEx(nCap As Integer, bSetContainer As Integer, ByRef ConTypes As System.IntPtr) As Integer
        Return api.DTWAIN_GetCapContainerEx(nCap, bSetContainer, ConTypes)
        End Function
        
        Public Function DTWAIN_GetCapContainerEx2(nCap As Integer, bSetContainer As Integer) As System.IntPtr
        Return api.DTWAIN_GetCapContainerEx2(nCap, bSetContainer)
        End Function
        
        Public Function DTWAIN_GetCapDataType(Source As System.IntPtr, nCap As Integer) As Integer
        Return api.DTWAIN_GetCapDataType(Source, nCap)
        End Function
        
        Public Function DTWAIN_GetCapFromName(szName As String) As Integer
        Return api.DTWAIN_GetCapFromName(szName)
        End Function
        
        Public Function DTWAIN_GetCapFromNameA(szName As String) As Integer
        Return api.DTWAIN_GetCapFromNameA(szName)
        End Function
        
        Public Function DTWAIN_GetCapFromNameW(szName As String) As Integer
        Return api.DTWAIN_GetCapFromNameW(szName)
        End Function
        
        Public Function DTWAIN_GetCapOperations(Source As System.IntPtr, lCapability As Integer, ByRef lpOps As Integer) As Integer
        Return api.DTWAIN_GetCapOperations(Source, lCapability, lpOps)
        End Function
        
        Public Function DTWAIN_GetCapValues(Source As System.IntPtr, lCap As Integer, lGetType As Integer, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_GetCapValues(Source, lCap, lGetType, pArray)
        End Function
        
        Public Function DTWAIN_GetCapValuesEx(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_GetCapValuesEx(Source, lCap, lGetType, lContainerType, pArray)
        End Function
        
        Public Function DTWAIN_GetCapValuesEx2(Source As System.IntPtr, lCap As Integer, lGetType As Integer, lContainerType As Integer, nDataType As Integer, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_GetCapValuesEx2(Source, lCap, lGetType, lContainerType, nDataType, pArray)
        End Function
        
        Public Function DTWAIN_GetCaption(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Caption As StringBuilder) As Integer
        Return api.DTWAIN_GetCaption(Source, Caption)
        End Function
        
        Public Function DTWAIN_GetCaptionA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Caption As StringBuilder) As Integer
        Return api.DTWAIN_GetCaptionA(Source, Caption)
        End Function
        
        Public Function DTWAIN_GetCaptionW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Caption As StringBuilder) As Integer
        Return api.DTWAIN_GetCaptionW(Source, Caption)
        End Function
        
        Public Function DTWAIN_GetCompressionSize(Source As System.IntPtr, ByRef lBytes As Integer) As Integer
        Return api.DTWAIN_GetCompressionSize(Source, lBytes)
        End Function
        
        Public Function DTWAIN_GetCompressionType(Source As System.IntPtr, ByRef lpCompression As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetCompressionType(Source, lpCompression, bCurrent)
        End Function
        
        Public Function DTWAIN_GetConditionCodeString(lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetConditionCodeString(lError, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetConditionCodeStringA(lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetConditionCodeStringA(lError, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetConditionCodeStringW(lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetConditionCodeStringW(lError, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetContrast(Source As System.IntPtr, ByRef Contrast As System.Double) As Integer
        Return api.DTWAIN_GetContrast(Source, Contrast)
        End Function
        
        Public Function DTWAIN_GetContrastString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Contrast As StringBuilder) As Integer
        Return api.DTWAIN_GetContrastString(Source, Contrast)
        End Function
        
        Public Function DTWAIN_GetContrastStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Contrast As StringBuilder) As Integer
        Return api.DTWAIN_GetContrastStringA(Source, Contrast)
        End Function
        
        Public Function DTWAIN_GetContrastStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Contrast As StringBuilder) As Integer
        Return api.DTWAIN_GetContrastStringW(Source, Contrast)
        End Function
        
        Public Function DTWAIN_GetCountry() As Integer
        Return api.DTWAIN_GetCountry()
        End Function
        
        Public Function DTWAIN_GetCurrentAcquiredImage(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_GetCurrentAcquiredImage(Source)
        End Function
        
        Public Function DTWAIN_GetCurrentFileName(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder, MaxLen As Integer) As Integer
        Return api.DTWAIN_GetCurrentFileName(Source, szName, MaxLen)
        End Function
        
        Public Function DTWAIN_GetCurrentFileNameA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder, MaxLen As Integer) As Integer
        Return api.DTWAIN_GetCurrentFileNameA(Source, szName, MaxLen)
        End Function
        
        Public Function DTWAIN_GetCurrentFileNameW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder, MaxLen As Integer) As Integer
        Return api.DTWAIN_GetCurrentFileNameW(Source, szName, MaxLen)
        End Function
        
        Public Function DTWAIN_GetCurrentPageNum(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetCurrentPageNum(Source)
        End Function
        
        Public Function DTWAIN_GetCurrentRetryCount(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetCurrentRetryCount(Source)
        End Function
        
        Public Function DTWAIN_GetCurrentTwainTriplet(ByRef pAppID As TW_IDENTITY, ByRef pSourceID As TW_IDENTITY, ByRef lpDG As Integer, ByRef lpDAT As Integer, ByRef lpMsg As Integer, ByRef lpMemRef As System.Int64) As Integer
        Return api.DTWAIN_GetCurrentTwainTriplet(pAppID, pSourceID, lpDG, lpDAT, lpMsg, lpMemRef)
        End Function
        
        Public Function DTWAIN_GetCustomDSData(Source As System.IntPtr, Data As Byte(), dSize As UInteger, ByRef pActualSize As UInteger, nFlags As Integer) As System.IntPtr
        Return api.DTWAIN_GetCustomDSData(Source, Data, dSize, pActualSize, nFlags)
        End Function
        
        Public Function DTWAIN_GetDSMFullName(DSMType As Integer, <MarshalAs(UnmanagedType.LPTStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
        Return api.DTWAIN_GetDSMFullName(DSMType, szDLLName, nMaxLen, pWhichSearch)
        End Function
        
        Public Function DTWAIN_GetDSMFullNameA(DSMType As Integer, <MarshalAs(UnmanagedType.LPStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
        Return api.DTWAIN_GetDSMFullNameA(DSMType, szDLLName, nMaxLen, pWhichSearch)
        End Function
        
        Public Function DTWAIN_GetDSMFullNameW(DSMType As Integer, <MarshalAs(UnmanagedType.LPWStr)> szDLLName As StringBuilder, nMaxLen As Integer, ByRef pWhichSearch As Integer) As Integer
        Return api.DTWAIN_GetDSMFullNameW(DSMType, szDLLName, nMaxLen, pWhichSearch)
        End Function
        
        Public Function DTWAIN_GetDSMSearchOrder() As Integer
        Return api.DTWAIN_GetDSMSearchOrder()
        End Function
        
        Public Function DTWAIN_GetDTWAINHandle() As System.IntPtr
        Return api.DTWAIN_GetDTWAINHandle()
        End Function
        
        Public Function DTWAIN_GetDeviceEvent(Source As System.IntPtr, ByRef lpEvent As Integer) As Integer
        Return api.DTWAIN_GetDeviceEvent(Source, lpEvent)
        End Function
        
        Public Function DTWAIN_GetDeviceEventEx(Source As System.IntPtr, ByRef lpEvent As Integer, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_GetDeviceEventEx(Source, lpEvent, pArray)
        End Function
        
        Public Function DTWAIN_GetDeviceEventInfo(Source As System.IntPtr, nWhichInfo As Integer, pValue As System.IntPtr) As Integer
        Return api.DTWAIN_GetDeviceEventInfo(Source, nWhichInfo, pValue)
        End Function
        
        Public Function DTWAIN_GetDeviceNotifications(Source As System.IntPtr, ByRef DevEvents As Integer) As Integer
        Return api.DTWAIN_GetDeviceNotifications(Source, DevEvents)
        End Function
        
        Public Function DTWAIN_GetDeviceTimeDate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szTimeDate As StringBuilder) As Integer
        Return api.DTWAIN_GetDeviceTimeDate(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_GetDeviceTimeDateA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szTimeDate As StringBuilder) As Integer
        Return api.DTWAIN_GetDeviceTimeDateA(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_GetDeviceTimeDateW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szTimeDate As StringBuilder) As Integer
        Return api.DTWAIN_GetDeviceTimeDateW(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_GetDoubleFeedDetectLength(Source As System.IntPtr, ByRef Value As System.Double, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetDoubleFeedDetectLength(Source, Value, bCurrent)
        End Function
        
        Public Function DTWAIN_GetDoubleFeedDetectValues(Source As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_GetDoubleFeedDetectValues(Source, pArray)
        End Function
        
        Public Function DTWAIN_GetDuplexType(Source As System.IntPtr, ByRef lpDupType As Integer) As Integer
        Return api.DTWAIN_GetDuplexType(Source, lpDupType)
        End Function
        
        Public Function DTWAIN_GetErrorBuffer(ByRef ArrayBuffer As System.IntPtr) As Integer
        Return api.DTWAIN_GetErrorBuffer(ArrayBuffer)
        End Function
        
        Public Function DTWAIN_GetErrorBufferThreshold() As Integer
        Return api.DTWAIN_GetErrorBufferThreshold()
        End Function
        
        Public Function DTWAIN_GetErrorCallback() As DTwainErrorProc
        Return api.DTWAIN_GetErrorCallback()
        End Function
        
        Public Function DTWAIN_GetErrorCallback64() As DTwainErrorProc64
        Return api.DTWAIN_GetErrorCallback64()
        End Function
        
        Public Function DTWAIN_GetErrorString(lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetErrorString(lError, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetErrorStringA(lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetErrorStringA(lError, lpszBuffer, nLength)
        End Function
        
        Public Function DTWAIN_GetErrorStringW(lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetErrorStringW(lError, lpszBuffer, nLength)
        End Function
        
        Public Function DTWAIN_GetExtCapFromName(szName As String) As Integer
        Return api.DTWAIN_GetExtCapFromName(szName)
        End Function
        
        Public Function DTWAIN_GetExtCapFromNameA(szName As String) As Integer
        Return api.DTWAIN_GetExtCapFromNameA(szName)
        End Function
        
        Public Function DTWAIN_GetExtCapFromNameW(szName As String) As Integer
        Return api.DTWAIN_GetExtCapFromNameW(szName)
        End Function
        
        Public Function DTWAIN_GetExtImageInfo(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetExtImageInfo(Source)
        End Function
        
        Public Function DTWAIN_GetExtImageInfoData(Source As System.IntPtr, nWhich As Integer, ByRef Data As System.IntPtr) As Integer
        Return api.DTWAIN_GetExtImageInfoData(Source, nWhich, Data)
        End Function
        
        Public Function DTWAIN_GetExtImageInfoDataEx(Source As System.IntPtr, nWhich As Integer) As System.IntPtr
        Return api.DTWAIN_GetExtImageInfoDataEx(Source, nWhich)
        End Function
        
        Public Function DTWAIN_GetExtImageInfoItem(Source As System.IntPtr, nWhich As Integer, ByRef InfoID As Integer, ByRef NumItems As Integer, ByRef Type As Integer) As Integer
        Return api.DTWAIN_GetExtImageInfoItem(Source, nWhich, InfoID, NumItems, Type)
        End Function
        
        Public Function DTWAIN_GetExtImageInfoItemEx(Source As System.IntPtr, nWhich As Integer, ByRef InfoID As Integer, ByRef NumItems As Integer, ByRef Type As Integer, ByRef ReturnCode As Integer) As Integer
        Return api.DTWAIN_GetExtImageInfoItemEx(Source, nWhich, InfoID, NumItems, Type, ReturnCode)
        End Function
        
        Public Function DTWAIN_GetExtNameFromCap(nValue As Integer, <MarshalAs(UnmanagedType.LPTStr)> szValue As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetExtNameFromCap(nValue, szValue, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetExtNameFromCapA(nValue As Integer, <MarshalAs(UnmanagedType.LPStr)> szValue As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetExtNameFromCapA(nValue, szValue, nLength)
        End Function
        
        Public Function DTWAIN_GetExtNameFromCapW(nValue As Integer, <MarshalAs(UnmanagedType.LPWStr)> szValue As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetExtNameFromCapW(nValue, szValue, nLength)
        End Function
        
        Public Function DTWAIN_GetFeederAlignment(Source As System.IntPtr, ByRef lpAlignment As Integer) As Integer
        Return api.DTWAIN_GetFeederAlignment(Source, lpAlignment)
        End Function
        
        Public Function DTWAIN_GetFeederFuncs(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetFeederFuncs(Source)
        End Function
        
        Public Function DTWAIN_GetFeederOrder(Source As System.IntPtr, ByRef lpOrder As Integer) As Integer
        Return api.DTWAIN_GetFeederOrder(Source, lpOrder)
        End Function
        
        Public Function DTWAIN_GetFeederWaitTime(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetFeederWaitTime(Source)
        End Function
        
        Public Function DTWAIN_GetFileCompressionType(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetFileCompressionType(Source)
        End Function
        
        Public Function DTWAIN_GetFileTypeExtensions(nType As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetFileTypeExtensions(nType, lpszName, nLength)
        End Function
        
        Public Function DTWAIN_GetFileTypeExtensionsA(nType As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetFileTypeExtensionsA(nType, lpszName, nLength)
        End Function
        
        Public Function DTWAIN_GetFileTypeExtensionsW(nType As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetFileTypeExtensionsW(nType, lpszName, nLength)
        End Function
        
        Public Function DTWAIN_GetFileTypeName(nType As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetFileTypeName(nType, lpszName, nLength)
        End Function
        
        Public Function DTWAIN_GetFileTypeNameA(nType As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetFileTypeNameA(nType, lpszName, nLength)
        End Function
        
        Public Function DTWAIN_GetFileTypeNameW(nType As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetFileTypeNameW(nType, lpszName, nLength)
        End Function
        
        Public Function DTWAIN_GetHalftone(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
        Return api.DTWAIN_GetHalftone(Source, lpHalftone, TypeOfGet)
        End Function
        
        Public Function DTWAIN_GetHalftoneA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
        Return api.DTWAIN_GetHalftoneA(Source, lpHalftone, TypeOfGet)
        End Function
        
        Public Function DTWAIN_GetHalftoneW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpHalftone As StringBuilder, TypeOfGet As Integer) As Integer
        Return api.DTWAIN_GetHalftoneW(Source, lpHalftone, TypeOfGet)
        End Function
        
        Public Function DTWAIN_GetHighlight(Source As System.IntPtr, ByRef Highlight As System.Double) As Integer
        Return api.DTWAIN_GetHighlight(Source, Highlight)
        End Function
        
        Public Function DTWAIN_GetHighlightString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Highlight As StringBuilder) As Integer
        Return api.DTWAIN_GetHighlightString(Source, Highlight)
        End Function
        
        Public Function DTWAIN_GetHighlightStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Highlight As StringBuilder) As Integer
        Return api.DTWAIN_GetHighlightStringA(Source, Highlight)
        End Function
        
        Public Function DTWAIN_GetHighlightStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Highlight As StringBuilder) As Integer
        Return api.DTWAIN_GetHighlightStringW(Source, Highlight)
        End Function
        
        Public Function DTWAIN_GetImageInfo(Source As System.IntPtr, ByRef lpXResolution As System.Double, ByRef lpYResolution As System.Double, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        Return api.DTWAIN_GetImageInfo(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression)
        End Function
        
        Public Function DTWAIN_GetImageInfoString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        Return api.DTWAIN_GetImageInfoString(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression)
        End Function
        
        Public Function DTWAIN_GetImageInfoStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        Return api.DTWAIN_GetImageInfoStringA(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression)
        End Function
        
        Public Function DTWAIN_GetImageInfoStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpXResolution As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> lpYResolution As StringBuilder, ByRef lpWidth As Integer, ByRef lpLength As Integer, ByRef lpNumSamples As Integer, ByRef lpBitsPerSample As System.IntPtr, ByRef lpBitsPerPixel As Integer, ByRef lpPlanar As Integer, ByRef lpPixelType As Integer, ByRef lpCompression As Integer) As Integer
        Return api.DTWAIN_GetImageInfoStringW(Source, lpXResolution, lpYResolution, lpWidth, lpLength, lpNumSamples, lpBitsPerSample, lpBitsPerPixel, lpPlanar, lpPixelType, lpCompression)
        End Function
        
        Public Function DTWAIN_GetJobControl(Source As System.IntPtr, ByRef pJobControl As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetJobControl(Source, pJobControl, bCurrent)
        End Function
        
        Public Function DTWAIN_GetJpegValues(Source As System.IntPtr, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
        Return api.DTWAIN_GetJpegValues(Source, pQuality, Progressive)
        End Function
        
        Public Function DTWAIN_GetJpegXRValues(Source As System.IntPtr, ByRef pQuality As Integer, ByRef Progressive As Integer) As Integer
        Return api.DTWAIN_GetJpegXRValues(Source, pQuality, Progressive)
        End Function
        
        Public Function DTWAIN_GetLanguage() As Integer
        Return api.DTWAIN_GetLanguage()
        End Function
        
        Public Function DTWAIN_GetLastError() As Integer
        Return api.DTWAIN_GetLastError()
        End Function
        
        Public Function DTWAIN_GetLibraryPath(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetLibraryPath(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetLibraryPathA(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetLibraryPathA(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetLibraryPathW(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetLibraryPathW(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetLightPath(Source As System.IntPtr, ByRef lpLightPath As Integer) As Integer
        Return api.DTWAIN_GetLightPath(Source, lpLightPath)
        End Function
        
        Public Function DTWAIN_GetLightSource(Source As System.IntPtr, ByRef LightSource As Integer) As Integer
        Return api.DTWAIN_GetLightSource(Source, LightSource)
        End Function
        
        Public Function DTWAIN_GetLightSources(Source As System.IntPtr, ByRef LightSources As System.IntPtr) As Integer
        Return api.DTWAIN_GetLightSources(Source, LightSources)
        End Function
        
        Public Function DTWAIN_GetLoggerCallback() As DTwainLoggerProc
        Return api.DTWAIN_GetLoggerCallback()
        End Function
        
        Public Function DTWAIN_GetLoggerCallbackA() As DTwainLoggerProcA
        Return api.DTWAIN_GetLoggerCallbackA()
        End Function
        
        Public Function DTWAIN_GetLoggerCallbackW() As DTwainLoggerProcW
        Return api.DTWAIN_GetLoggerCallbackW()
        End Function
        
        Public Function DTWAIN_GetManualDuplexCount(Source As System.IntPtr, ByRef pSide1 As Integer, ByRef pSide2 As Integer) As Integer
        Return api.DTWAIN_GetManualDuplexCount(Source, pSide1, pSide2)
        End Function
        
        Public Function DTWAIN_GetMaxAcquisitions(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetMaxAcquisitions(Source)
        End Function
        
        Public Function DTWAIN_GetMaxBuffers(Source As System.IntPtr, ByRef pMaxBuf As Integer) As Integer
        Return api.DTWAIN_GetMaxBuffers(Source, pMaxBuf)
        End Function
        
        Public Function DTWAIN_GetMaxPagesToAcquire(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetMaxPagesToAcquire(Source)
        End Function
        
        Public Function DTWAIN_GetMaxRetryAttempts(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetMaxRetryAttempts(Source)
        End Function
        
        Public Function DTWAIN_GetNameFromCap(nCapValue As Integer, <MarshalAs(UnmanagedType.LPTStr)> szValue As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetNameFromCap(nCapValue, szValue, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetNameFromCapA(nCapValue As Integer, <MarshalAs(UnmanagedType.LPStr)> szValue As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetNameFromCapA(nCapValue, szValue, nLength)
        End Function
        
        Public Function DTWAIN_GetNameFromCapW(nCapValue As Integer, <MarshalAs(UnmanagedType.LPWStr)> szValue As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetNameFromCapW(nCapValue, szValue, nLength)
        End Function
        
        Public Function DTWAIN_GetNoiseFilter(Source As System.IntPtr, ByRef lpNoiseFilter As Integer) As Integer
        Return api.DTWAIN_GetNoiseFilter(Source, lpNoiseFilter)
        End Function
        
        Public Function DTWAIN_GetNumAcquiredImages(aAcq As System.IntPtr, nWhich As Integer) As Integer
        Return api.DTWAIN_GetNumAcquiredImages(aAcq, nWhich)
        End Function
        
        Public Function DTWAIN_GetNumAcquisitions(aAcq As System.IntPtr) As Integer
        Return api.DTWAIN_GetNumAcquisitions(aAcq)
        End Function
        
        Public Function DTWAIN_GetOCRCapValues(Engine As System.IntPtr, OCRCapValue As Integer, TypeOfGet As Integer, ByRef CapValues As System.IntPtr) As Integer
        Return api.DTWAIN_GetOCRCapValues(Engine, OCRCapValue, TypeOfGet, CapValues)
        End Function
        
        Public Function DTWAIN_GetOCRErrorString(Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetOCRErrorString(Engine, lError, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetOCRErrorStringA(Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetOCRErrorStringA(Engine, lError, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetOCRErrorStringW(Engine As System.IntPtr, lError As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetOCRErrorStringW(Engine, lError, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetOCRLastError(Engine As System.IntPtr) As Integer
        Return api.DTWAIN_GetOCRLastError(Engine)
        End Function
        
        Public Function DTWAIN_GetOCRMajorMinorVersion(Engine As System.IntPtr, ByRef lpMajor As Integer, ByRef lpMinor As Integer) As Integer
        Return api.DTWAIN_GetOCRMajorMinorVersion(Engine, lpMajor, lpMinor)
        End Function
        
        Public Function DTWAIN_GetOCRManufacturer(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szManufacturer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetOCRManufacturer(Engine, szManufacturer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetOCRManufacturerA(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szManufacturer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRManufacturerA(Engine, szManufacturer, nLength)
        End Function
        
        Public Function DTWAIN_GetOCRManufacturerW(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szManufacturer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRManufacturerW(Engine, szManufacturer, nLength)
        End Function
        
        Public Function DTWAIN_GetOCRProductFamily(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProductFamily As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetOCRProductFamily(Engine, szProductFamily, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetOCRProductFamilyA(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProductFamily As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRProductFamilyA(Engine, szProductFamily, nLength)
        End Function
        
        Public Function DTWAIN_GetOCRProductFamilyW(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProductFamily As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRProductFamilyW(Engine, szProductFamily, nLength)
        End Function
        
        Public Function DTWAIN_GetOCRProductName(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProductName As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetOCRProductName(Engine, szProductName, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetOCRProductNameA(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProductName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRProductNameA(Engine, szProductName, nLength)
        End Function
        
        Public Function DTWAIN_GetOCRProductNameW(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProductName As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRProductNameW(Engine, szProductName, nLength)
        End Function
        
        Public Function DTWAIN_GetOCRText(Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPTStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
        Return api.DTWAIN_GetOCRText(Engine, nPageNo, Data, dSize, pActualSize, nFlags)
        End Function
        
        Public Function DTWAIN_GetOCRTextA(Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
        Return api.DTWAIN_GetOCRTextA(Engine, nPageNo, Data, dSize, pActualSize, nFlags)
        End Function
        
        Public Function DTWAIN_GetOCRTextInfoFloat(OCRTextInfo As System.IntPtr, nCharPos As Integer, nWhichItem As Integer, ByRef pInfo As System.Double) As Integer
        Return api.DTWAIN_GetOCRTextInfoFloat(OCRTextInfo, nCharPos, nWhichItem, pInfo)
        End Function
        
        Public Function DTWAIN_GetOCRTextInfoFloatEx(OCRTextInfo As System.IntPtr, nWhichItem As Integer, ByRef pInfo As System.Double, bufSize As Integer) As Integer
        Return api.DTWAIN_GetOCRTextInfoFloatEx(OCRTextInfo, nWhichItem, pInfo, bufSize)
        End Function
        
        Public Function DTWAIN_GetOCRTextInfoHandle(Engine As System.IntPtr, nPageNo As Integer) As System.IntPtr
        Return api.DTWAIN_GetOCRTextInfoHandle(Engine, nPageNo)
        End Function
        
        Public Function DTWAIN_GetOCRTextInfoLong(OCRTextInfo As System.IntPtr, nCharPos As Integer, nWhichItem As Integer, ByRef pInfo As Integer) As Integer
        Return api.DTWAIN_GetOCRTextInfoLong(OCRTextInfo, nCharPos, nWhichItem, pInfo)
        End Function
        
        Public Function DTWAIN_GetOCRTextInfoLongEx(OCRTextInfo As System.IntPtr, nWhichItem As Integer, ByRef pInfo As Integer, bufSize As Integer) As Integer
        Return api.DTWAIN_GetOCRTextInfoLongEx(OCRTextInfo, nWhichItem, pInfo, bufSize)
        End Function
        
        Public Function DTWAIN_GetOCRTextW(Engine As System.IntPtr, nPageNo As Integer, <MarshalAs(UnmanagedType.LPWStr)> Data As StringBuilder, dSize As Integer, ByRef pActualSize As Integer, nFlags As Integer) As System.IntPtr
        Return api.DTWAIN_GetOCRTextW(Engine, nPageNo, Data, dSize, pActualSize, nFlags)
        End Function
        
        Public Function DTWAIN_GetOCRVersionInfo(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> buffer As StringBuilder, maxBufSize As Integer) As Integer
        Return api.DTWAIN_GetOCRVersionInfo(Engine, buffer, maxBufSize)
        End Function
        
        Public Function DTWAIN_GetOCRVersionInfoA(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> buffer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRVersionInfoA(Engine, buffer, nLength)
        End Function
        
        Public Function DTWAIN_GetOCRVersionInfoW(Engine As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> buffer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetOCRVersionInfoW(Engine, buffer, nLength)
        End Function
        
        Public Function DTWAIN_GetOrientation(Source As System.IntPtr, ByRef lpOrient As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetOrientation(Source, lpOrient, bCurrent)
        End Function
        
        Public Function DTWAIN_GetOverscan(Source As System.IntPtr, ByRef lpOverscan As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetOverscan(Source, lpOverscan, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPDFTextElementFloat(TextElement As System.IntPtr, ByRef val1 As System.Double, ByRef val2 As System.Double, Flags As Integer) As Integer
        Return api.DTWAIN_GetPDFTextElementFloat(TextElement, val1, val2, Flags)
        End Function
        
        Public Function DTWAIN_GetPDFTextElementLong(TextElement As System.IntPtr, ByRef val1 As Integer, ByRef val2 As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_GetPDFTextElementLong(TextElement, val1, val2, Flags)
        End Function
        
        Public Function DTWAIN_GetPDFTextElementString(TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_GetPDFTextElementString(TextElement, szData, maxLen, Flags)
        End Function
        
        Public Function DTWAIN_GetPDFTextElementStringA(TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_GetPDFTextElementStringA(TextElement, szData, maxLen, Flags)
        End Function
        
        Public Function DTWAIN_GetPDFTextElementStringW(TextElement As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szData As StringBuilder, maxLen As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_GetPDFTextElementStringW(TextElement, szData, maxLen, Flags)
        End Function
        
        Public Function DTWAIN_GetPDFType1FontName(FontVal As Integer, <MarshalAs(UnmanagedType.LPTStr)> szFont As StringBuilder, nChars As Integer) As Integer
        Return api.DTWAIN_GetPDFType1FontName(FontVal, szFont, nChars)
        End Function
        
        Public Function DTWAIN_GetPDFType1FontNameA(FontVal As Integer, <MarshalAs(UnmanagedType.LPStr)> szFont As StringBuilder, nChars As Integer) As Integer
        Return api.DTWAIN_GetPDFType1FontNameA(FontVal, szFont, nChars)
        End Function
        
        Public Function DTWAIN_GetPDFType1FontNameW(FontVal As Integer, <MarshalAs(UnmanagedType.LPWStr)> szFont As StringBuilder, nChars As Integer) As Integer
        Return api.DTWAIN_GetPDFType1FontNameW(FontVal, szFont, nChars)
        End Function
        
        Public Function DTWAIN_GetPaperSize(Source As System.IntPtr, ByRef lpPaperSize As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPaperSize(Source, lpPaperSize, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPaperSizeName(paperNumber As Integer, <MarshalAs(UnmanagedType.LPTStr)> outName As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_GetPaperSizeName(paperNumber, outName, nSize)
        End Function
        
        Public Function DTWAIN_GetPaperSizeNameA(paperNumber As Integer, <MarshalAs(UnmanagedType.LPStr)> outName As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_GetPaperSizeNameA(paperNumber, outName, nSize)
        End Function
        
        Public Function DTWAIN_GetPaperSizeNameW(paperNumber As Integer, <MarshalAs(UnmanagedType.LPWStr)> outName As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_GetPaperSizeNameW(paperNumber, outName, nSize)
        End Function
        
        Public Function DTWAIN_GetPatchMaxPriorities(Source As System.IntPtr, ByRef pMaxPriorities As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPatchMaxPriorities(Source, pMaxPriorities, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPatchMaxRetries(Source As System.IntPtr, ByRef pMaxRetries As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPatchMaxRetries(Source, pMaxRetries, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPatchPriorities(Source As System.IntPtr, ByRef SearchPriorities As System.IntPtr) As Integer
        Return api.DTWAIN_GetPatchPriorities(Source, SearchPriorities)
        End Function
        
        Public Function DTWAIN_GetPatchSearchMode(Source As System.IntPtr, ByRef pSearchMode As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPatchSearchMode(Source, pSearchMode, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPatchTimeOut(Source As System.IntPtr, ByRef pTimeOut As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPatchTimeOut(Source, pTimeOut, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPixelFlavor(Source As System.IntPtr, ByRef lpPixelFlavor As Integer) As Integer
        Return api.DTWAIN_GetPixelFlavor(Source, lpPixelFlavor)
        End Function
        
        Public Function DTWAIN_GetPixelType(Source As System.IntPtr, ByRef PixelType As Integer, ByRef BitDepth As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPixelType(Source, PixelType, BitDepth, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPrinter(Source As System.IntPtr, ByRef lpPrinter As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPrinter(Source, lpPrinter, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPrinterStartNumber(Source As System.IntPtr, ByRef nStart As Integer) As Integer
        Return api.DTWAIN_GetPrinterStartNumber(Source, nStart)
        End Function
        
        Public Function DTWAIN_GetPrinterStringMode(Source As System.IntPtr, ByRef PrinterMode As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_GetPrinterStringMode(Source, PrinterMode, bCurrent)
        End Function
        
        Public Function DTWAIN_GetPrinterStrings(Source As System.IntPtr, ByRef ArrayString As System.IntPtr) As Integer
        Return api.DTWAIN_GetPrinterStrings(Source, ArrayString)
        End Function
        
        Public Function DTWAIN_GetPrinterSuffixString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Suffix As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetPrinterSuffixString(Source, Suffix, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetPrinterSuffixStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Suffix As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetPrinterSuffixStringA(Source, Suffix, nLength)
        End Function
        
        Public Function DTWAIN_GetPrinterSuffixStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Suffix As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetPrinterSuffixStringW(Source, Suffix, nLength)
        End Function
        
        Public Function DTWAIN_GetRegisteredMsg() As Integer
        Return api.DTWAIN_GetRegisteredMsg()
        End Function
        
        Public Function DTWAIN_GetResolution(Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
        Return api.DTWAIN_GetResolution(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetResolutionString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetResolutionString(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetResolutionStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetResolutionStringA(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetResolutionStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetResolutionStringW(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetResourceString(ResourceID As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetResourceString(ResourceID, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetResourceStringA(ResourceID As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetResourceStringA(ResourceID, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetResourceStringW(ResourceID As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetResourceStringW(ResourceID, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetRotation(Source As System.IntPtr, ByRef Rotation As System.Double) As Integer
        Return api.DTWAIN_GetRotation(Source, Rotation)
        End Function
        
        Public Function DTWAIN_GetRotationString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Rotation As StringBuilder) As Integer
        Return api.DTWAIN_GetRotationString(Source, Rotation)
        End Function
        
        Public Function DTWAIN_GetRotationStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Rotation As StringBuilder) As Integer
        Return api.DTWAIN_GetRotationStringA(Source, Rotation)
        End Function
        
        Public Function DTWAIN_GetRotationStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Rotation As StringBuilder) As Integer
        Return api.DTWAIN_GetRotationStringW(Source, Rotation)
        End Function
        
        Public Function DTWAIN_GetSaveFileName(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetSaveFileName(Source, fName, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetSaveFileNameA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetSaveFileNameA(Source, fName, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetSaveFileNameW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> fName As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetSaveFileNameW(Source, fName, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetSavedFilesCount(Source As System.IntPtr) As Integer
        Return api.DTWAIN_GetSavedFilesCount(Source)
        End Function
        
        Public Function DTWAIN_GetSessionDetails(<MarshalAs(UnmanagedType.LPTStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        Return api.DTWAIN_GetSessionDetails(szBuf, nSize, indentFactor, bRefresh)
        End Function
        
        Public Function DTWAIN_GetSessionDetailsA(<MarshalAs(UnmanagedType.LPStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        Return api.DTWAIN_GetSessionDetailsA(szBuf, nSize, indentFactor, bRefresh)
        End Function
        
        Public Function DTWAIN_GetSessionDetailsW(<MarshalAs(UnmanagedType.LPWStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        Return api.DTWAIN_GetSessionDetailsW(szBuf, nSize, indentFactor, bRefresh)
        End Function
        
        Public Function DTWAIN_GetShadow(Source As System.IntPtr, ByRef Shadow As System.Double) As Integer
        Return api.DTWAIN_GetShadow(Source, Shadow)
        End Function
        
        Public Function DTWAIN_GetShadowString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Shadow As StringBuilder) As Integer
        Return api.DTWAIN_GetShadowString(Source, Shadow)
        End Function
        
        Public Function DTWAIN_GetShadowStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Shadow As StringBuilder) As Integer
        Return api.DTWAIN_GetShadowStringA(Source, Shadow)
        End Function
        
        Public Function DTWAIN_GetShadowStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Shadow As StringBuilder) As Integer
        Return api.DTWAIN_GetShadowStringW(Source, Shadow)
        End Function
        
        Public Function DTWAIN_GetShortVersionString(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetShortVersionString(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetShortVersionStringA(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetShortVersionStringA(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetShortVersionStringW(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetShortVersionStringW(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceAcquisitions(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_GetSourceAcquisitions(Source)
        End Function
        
        Public Function DTWAIN_GetSourceDetails(szSources As String, <MarshalAs(UnmanagedType.LPTStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        Return api.DTWAIN_GetSourceDetails(szSources, szBuf, nSize, indentFactor, bRefresh)
        End Function
        
        Public Function DTWAIN_GetSourceDetailsA(szSources As String, <MarshalAs(UnmanagedType.LPStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        Return api.DTWAIN_GetSourceDetailsA(szSources, szBuf, nSize, indentFactor, bRefresh)
        End Function
        
        Public Function DTWAIN_GetSourceDetailsW(szSources As String, <MarshalAs(UnmanagedType.LPWStr)> szBuf As StringBuilder, nSize As Integer, indentFactor As Integer, bRefresh As Integer) As Integer
        Return api.DTWAIN_GetSourceDetailsW(szSources, szBuf, nSize, indentFactor, bRefresh)
        End Function
        
        Public Function DTWAIN_GetSourceID(Source As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_GetSourceID(Source)
        End Function
        
        Public Function DTWAIN_GetSourceManufacturer(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetSourceManufacturer(Source, szProduct, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetSourceManufacturerA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceManufacturerA(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceManufacturerW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceManufacturerW(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceProductFamily(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetSourceProductFamily(Source, szProduct, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetSourceProductFamilyA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceProductFamilyA(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceProductFamilyW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceProductFamilyW(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceProductName(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetSourceProductName(Source, szProduct, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetSourceProductNameA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceProductNameA(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceProductNameW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceProductNameW(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceUnit(Source As System.IntPtr, ByRef lpUnit As Integer) As Integer
        Return api.DTWAIN_GetSourceUnit(Source, lpUnit)
        End Function
        
        Public Function DTWAIN_GetSourceVersionInfo(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szProduct As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetSourceVersionInfo(Source, szProduct, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetSourceVersionInfoA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceVersionInfoA(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceVersionInfoW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szProduct As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetSourceVersionInfoW(Source, szProduct, nLength)
        End Function
        
        Public Function DTWAIN_GetSourceVersionNumber(Source As System.IntPtr, ByRef pMajor As Integer, ByRef pMinor As Integer) As Integer
        Return api.DTWAIN_GetSourceVersionNumber(Source, pMajor, pMinor)
        End Function
        
        Public Function DTWAIN_GetStaticLibVersion() As Integer
        Return api.DTWAIN_GetStaticLibVersion()
        End Function
        
        Public Function DTWAIN_GetTempFileDirectory(<MarshalAs(UnmanagedType.LPTStr)> szFilePath As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetTempFileDirectory(szFilePath, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetTempFileDirectoryA(<MarshalAs(UnmanagedType.LPStr)> szFilePath As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetTempFileDirectoryA(szFilePath, nLength)
        End Function
        
        Public Function DTWAIN_GetTempFileDirectoryW(<MarshalAs(UnmanagedType.LPWStr)> szFilePath As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetTempFileDirectoryW(szFilePath, nLength)
        End Function
        
        Public Function DTWAIN_GetThreshold(Source As System.IntPtr, ByRef Threshold As System.Double) As Integer
        Return api.DTWAIN_GetThreshold(Source, Threshold)
        End Function
        
        Public Function DTWAIN_GetThresholdString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Threshold As StringBuilder) As Integer
        Return api.DTWAIN_GetThresholdString(Source, Threshold)
        End Function
        
        Public Function DTWAIN_GetThresholdStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Threshold As StringBuilder) As Integer
        Return api.DTWAIN_GetThresholdStringA(Source, Threshold)
        End Function
        
        Public Function DTWAIN_GetThresholdStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Threshold As StringBuilder) As Integer
        Return api.DTWAIN_GetThresholdStringW(Source, Threshold)
        End Function
        
        Public Function DTWAIN_GetTimeDate(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> szTimeDate As StringBuilder) As Integer
        Return api.DTWAIN_GetTimeDate(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_GetTimeDateA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> szTimeDate As StringBuilder) As Integer
        Return api.DTWAIN_GetTimeDateA(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_GetTimeDateW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> szTimeDate As StringBuilder) As Integer
        Return api.DTWAIN_GetTimeDateW(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_GetTwainAppID() As System.IntPtr
        Return api.DTWAIN_GetTwainAppID()
        End Function
        
        Public Function DTWAIN_GetTwainAvailability() As Integer
        Return api.DTWAIN_GetTwainAvailability()
        End Function
        
        Public Function DTWAIN_GetTwainAvailabilityEx(<MarshalAs(UnmanagedType.LPTStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetTwainAvailabilityEx(directories, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetTwainAvailabilityExA(<MarshalAs(UnmanagedType.LPStr)> szDirectories As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetTwainAvailabilityExA(szDirectories, nLength)
        End Function
        
        Public Function DTWAIN_GetTwainAvailabilityExW(<MarshalAs(UnmanagedType.LPWStr)> szDirectories As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetTwainAvailabilityExW(szDirectories, nLength)
        End Function
        
        Public Function DTWAIN_GetTwainCountryName(countryId As Integer, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder) As Integer
        Return api.DTWAIN_GetTwainCountryName(countryId, szName)
        End Function
        
        Public Function DTWAIN_GetTwainCountryNameA(countryId As Integer, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder) As Integer
        Return api.DTWAIN_GetTwainCountryNameA(countryId, szName)
        End Function
        
        Public Function DTWAIN_GetTwainCountryNameW(countryId As Integer, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder) As Integer
        Return api.DTWAIN_GetTwainCountryNameW(countryId, szName)
        End Function
        
        Public Function DTWAIN_GetTwainCountryValue(country As String) As Integer
        Return api.DTWAIN_GetTwainCountryValue(country)
        End Function
        
        Public Function DTWAIN_GetTwainCountryValueA(country As String) As Integer
        Return api.DTWAIN_GetTwainCountryValueA(country)
        End Function
        
        Public Function DTWAIN_GetTwainCountryValueW(country As String) As Integer
        Return api.DTWAIN_GetTwainCountryValueW(country)
        End Function
        
        Public Function DTWAIN_GetTwainHwnd() As System.IntPtr
        Return api.DTWAIN_GetTwainHwnd()
        End Function
        
        Public Function DTWAIN_GetTwainIDFromName(lpszBuffer As String) As Integer
        Return api.DTWAIN_GetTwainIDFromName(lpszBuffer)
        End Function
        
        Public Function DTWAIN_GetTwainIDFromNameA(lpszBuffer As String) As Integer
        Return api.DTWAIN_GetTwainIDFromNameA(lpszBuffer)
        End Function
        
        Public Function DTWAIN_GetTwainIDFromNameW(lpszBuffer As String) As Integer
        Return api.DTWAIN_GetTwainIDFromNameW(lpszBuffer)
        End Function
        
        Public Function DTWAIN_GetTwainLanguageName(nameId As Integer, <MarshalAs(UnmanagedType.LPTStr)> szName As StringBuilder) As Integer
        Return api.DTWAIN_GetTwainLanguageName(nameId, szName)
        End Function
        
        Public Function DTWAIN_GetTwainLanguageNameA(lang As Integer, <MarshalAs(UnmanagedType.LPStr)> szName As StringBuilder) As Integer
        Return api.DTWAIN_GetTwainLanguageNameA(lang, szName)
        End Function
        
        Public Function DTWAIN_GetTwainLanguageNameW(lang As Integer, <MarshalAs(UnmanagedType.LPWStr)> szName As StringBuilder) As Integer
        Return api.DTWAIN_GetTwainLanguageNameW(lang, szName)
        End Function
        
        Public Function DTWAIN_GetTwainLanguageValue(szName As String) As Integer
        Return api.DTWAIN_GetTwainLanguageValue(szName)
        End Function
        
        Public Function DTWAIN_GetTwainLanguageValueA(lang As String) As Integer
        Return api.DTWAIN_GetTwainLanguageValueA(lang)
        End Function
        
        Public Function DTWAIN_GetTwainLanguageValueW(lang As String) As Integer
        Return api.DTWAIN_GetTwainLanguageValueW(lang)
        End Function
        
        Public Function DTWAIN_GetTwainMode() As Integer
        Return api.DTWAIN_GetTwainMode()
        End Function
        
        Public Function DTWAIN_GetTwainNameFromConstant(lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_GetTwainNameFromConstant(lConstantType, lTwainConstant, lpszOut, nSize)
        End Function
        
        Public Function DTWAIN_GetTwainNameFromConstantA(lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_GetTwainNameFromConstantA(lConstantType, lTwainConstant, lpszOut, nSize)
        End Function
        
        Public Function DTWAIN_GetTwainNameFromConstantW(lConstantType As Integer, lTwainConstant As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszOut As StringBuilder, nSize As Integer) As Integer
        Return api.DTWAIN_GetTwainNameFromConstantW(lConstantType, lTwainConstant, lpszOut, nSize)
        End Function
        
        Public Function DTWAIN_GetTwainStringName(category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetTwainStringName(category, TwainID, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetTwainStringNameA(category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetTwainStringNameA(category, TwainID, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetTwainStringNameW(category As Integer, TwainID As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetTwainStringNameW(category, TwainID, lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetTwainTimeout() As Integer
        Return api.DTWAIN_GetTwainTimeout()
        End Function
        
        Public Function DTWAIN_GetVersion(ByRef lpMajor As Integer, ByRef lpMinor As Integer, ByRef lpVersionType As Integer) As Integer
        Return api.DTWAIN_GetVersion(lpMajor, lpMinor, lpVersionType)
        End Function
        
        Public Function DTWAIN_GetVersionCopyright(<MarshalAs(UnmanagedType.LPTStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionCopyright(lpszApp, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionCopyrightA(<MarshalAs(UnmanagedType.LPStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionCopyrightA(lpszApp, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionCopyrightW(<MarshalAs(UnmanagedType.LPWStr)> lpszApp As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionCopyrightW(lpszApp, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionEx(ByRef lMajor As Integer, ByRef lMinor As Integer, ByRef lVersionType As Integer, ByRef lPatchLevel As Integer) As Integer
        Return api.DTWAIN_GetVersionEx(lMajor, lMinor, lVersionType, lPatchLevel)
        End Function
        
        Public Function DTWAIN_GetVersionInfo(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionInfo(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionInfoA(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionInfoA(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionInfoW(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionInfoW(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionString(<MarshalAs(UnmanagedType.LPTStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionString(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionStringA(<MarshalAs(UnmanagedType.LPStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionStringA(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetVersionStringW(<MarshalAs(UnmanagedType.LPWStr)> lpszVer As StringBuilder, nLength As Integer) As Integer
        Return api.DTWAIN_GetVersionStringW(lpszVer, nLength)
        End Function
        
        Public Function DTWAIN_GetWindowsVersionInfo(<MarshalAs(UnmanagedType.LPTStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetWindowsVersionInfo(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetWindowsVersionInfoA(<MarshalAs(UnmanagedType.LPStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetWindowsVersionInfoA(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetWindowsVersionInfoW(<MarshalAs(UnmanagedType.LPWStr)> lpszBuffer As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_GetWindowsVersionInfoW(lpszBuffer, nMaxLen)
        End Function
        
        Public Function DTWAIN_GetXResolution(Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
        Return api.DTWAIN_GetXResolution(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetXResolutionString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetXResolutionString(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetXResolutionStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetXResolutionStringA(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetXResolutionStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetXResolutionStringW(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetYResolution(Source As System.IntPtr, ByRef Resolution As System.Double) As Integer
        Return api.DTWAIN_GetYResolution(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetYResolutionString(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetYResolutionString(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetYResolutionStringA(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetYResolutionStringA(Source, Resolution)
        End Function
        
        Public Function DTWAIN_GetYResolutionStringW(Source As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> Resolution As StringBuilder) As Integer
        Return api.DTWAIN_GetYResolutionStringW(Source, Resolution)
        End Function
        
        Public Function DTWAIN_InitExtImageInfo(Source As System.IntPtr) As Integer
        Return api.DTWAIN_InitExtImageInfo(Source)
        End Function
        
        Public Function DTWAIN_InitImageFileAppend(szFile As String, fType As Integer) As Integer
        Return api.DTWAIN_InitImageFileAppend(szFile, fType)
        End Function
        
        Public Function DTWAIN_InitImageFileAppendA(szFile As String, fType As Integer) As Integer
        Return api.DTWAIN_InitImageFileAppendA(szFile, fType)
        End Function
        
        Public Function DTWAIN_InitImageFileAppendW(szFile As String, fType As Integer) As Integer
        Return api.DTWAIN_InitImageFileAppendW(szFile, fType)
        End Function
        
        Public Function DTWAIN_InitOCRInterface() As Integer
        Return api.DTWAIN_InitOCRInterface()
        End Function
        
        Public Function DTWAIN_IsAcquiring() As Integer
        Return api.DTWAIN_IsAcquiring()
        End Function
        
        Public Function DTWAIN_IsAudioXferSupported(Source As System.IntPtr, supportVal As Integer) As Integer
        Return api.DTWAIN_IsAudioXferSupported(Source, supportVal)
        End Function
        
        Public Function DTWAIN_IsAutoBorderDetectEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoBorderDetectEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsAutoBorderDetectSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoBorderDetectSupported(Source)
        End Function
        
        Public Function DTWAIN_IsAutoBrightEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoBrightEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsAutoBrightSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoBrightSupported(Source)
        End Function
        
        Public Function DTWAIN_IsAutoDeskewEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoDeskewEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsAutoDeskewSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoDeskewSupported(Source)
        End Function
        
        Public Function DTWAIN_IsAutoFeedEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoFeedEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsAutoFeedSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoFeedSupported(Source)
        End Function
        
        Public Function DTWAIN_IsAutoRotateEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoRotateEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsAutoRotateSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoRotateSupported(Source)
        End Function
        
        Public Function DTWAIN_IsAutoScanEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutoScanEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsAutomaticSenseMediumEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutomaticSenseMediumEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsAutomaticSenseMediumSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsAutomaticSenseMediumSupported(Source)
        End Function
        
        Public Function DTWAIN_IsBlankPageDetectionOn(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsBlankPageDetectionOn(Source)
        End Function
        
        Public Function DTWAIN_IsBufferedTileModeOn(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsBufferedTileModeOn(Source)
        End Function
        
        Public Function DTWAIN_IsBufferedTileModeSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsBufferedTileModeSupported(Source)
        End Function
        
        Public Function DTWAIN_IsCapSupported(Source As System.IntPtr, lCapability As Integer) As Integer
        Return api.DTWAIN_IsCapSupported(Source, lCapability)
        End Function
        
        Public Function DTWAIN_IsCompressionSupported(Source As System.IntPtr, Compression As Integer) As Integer
        Return api.DTWAIN_IsCompressionSupported(Source, Compression)
        End Function
        
        Public Function DTWAIN_IsCustomDSDataSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsCustomDSDataSupported(Source)
        End Function
        
        Public Function DTWAIN_IsDIBBlank(hDib As System.IntPtr, threshold As System.Double) As Integer
        Return api.DTWAIN_IsDIBBlank(hDib, threshold)
        End Function
        
        Public Function DTWAIN_IsDIBBlankString(hDib As System.IntPtr, threshold As String) As Integer
        Return api.DTWAIN_IsDIBBlankString(hDib, threshold)
        End Function
        
        Public Function DTWAIN_IsDIBBlankStringA(hDib As System.IntPtr, threshold As String) As Integer
        Return api.DTWAIN_IsDIBBlankStringA(hDib, threshold)
        End Function
        
        Public Function DTWAIN_IsDIBBlankStringW(hDib As System.IntPtr, threshold As String) As Integer
        Return api.DTWAIN_IsDIBBlankStringW(hDib, threshold)
        End Function
        
        Public Function DTWAIN_IsDeviceEventSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsDeviceEventSupported(Source)
        End Function
        
        Public Function DTWAIN_IsDeviceOnLine(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsDeviceOnLine(Source)
        End Function
        
        Public Function DTWAIN_IsDoubleFeedDetectLengthSupported(Source As System.IntPtr, value As System.Double) As Integer
        Return api.DTWAIN_IsDoubleFeedDetectLengthSupported(Source, value)
        End Function
        
        Public Function DTWAIN_IsDoubleFeedDetectSupported(Source As System.IntPtr, SupportVal As Integer) As Integer
        Return api.DTWAIN_IsDoubleFeedDetectSupported(Source, SupportVal)
        End Function
        
        Public Function DTWAIN_IsDoublePageCountOnDuplex(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsDoublePageCountOnDuplex(Source)
        End Function
        
        Public Function DTWAIN_IsDuplexEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsDuplexEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsDuplexSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsDuplexSupported(Source)
        End Function
        
        Public Function DTWAIN_IsExtImageInfoSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsExtImageInfoSupported(Source)
        End Function
        
        Public Function DTWAIN_IsFeederEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsFeederEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsFeederLoaded(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsFeederLoaded(Source)
        End Function
        
        Public Function DTWAIN_IsFeederSensitive(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsFeederSensitive(Source)
        End Function
        
        Public Function DTWAIN_IsFeederSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsFeederSupported(Source)
        End Function
        
        Public Function DTWAIN_IsFileSystemSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsFileSystemSupported(Source)
        End Function
        
        Public Function DTWAIN_IsFileXferSupported(Source As System.IntPtr, lFileType As Integer) As Integer
        Return api.DTWAIN_IsFileXferSupported(Source, lFileType)
        End Function
        
        Public Function DTWAIN_IsIAFieldALastPageSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldALastPageSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldALevelSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldALevelSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldAPrintFormatSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldAPrintFormatSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldAValueSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldAValueSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldBLastPageSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldBLastPageSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldBLevelSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldBLevelSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldBPrintFormatSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldBPrintFormatSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldBValueSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldBValueSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldCLastPageSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldCLastPageSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldCLevelSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldCLevelSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldCPrintFormatSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldCPrintFormatSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldCValueSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldCValueSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldDLastPageSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldDLastPageSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldDLevelSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldDLevelSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldDPrintFormatSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldDPrintFormatSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldDValueSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldDValueSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldELastPageSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldELastPageSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldELevelSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldELevelSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldEPrintFormatSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldEPrintFormatSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIAFieldEValueSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIAFieldEValueSupported(Source)
        End Function
        
        Public Function DTWAIN_IsImageAddressingSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsImageAddressingSupported(Source)
        End Function
        
        Public Function DTWAIN_IsIndicatorEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIndicatorEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsIndicatorSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsIndicatorSupported(Source)
        End Function
        
        Public Function DTWAIN_IsInitialized() As Integer
        Return api.DTWAIN_IsInitialized()
        End Function
        
        Public Function DTWAIN_IsJPEGSupported() As Integer
        Return api.DTWAIN_IsJPEGSupported()
        End Function
        
        Public Function DTWAIN_IsJobControlSupported(Source As System.IntPtr, JobControl As Integer) As Integer
        Return api.DTWAIN_IsJobControlSupported(Source, JobControl)
        End Function
        
        Public Function DTWAIN_IsLampEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsLampEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsLampSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsLampSupported(Source)
        End Function
        
        Public Function DTWAIN_IsLightPathSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsLightPathSupported(Source)
        End Function
        
        Public Function DTWAIN_IsLightSourceSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsLightSourceSupported(Source)
        End Function
        
        Public Function DTWAIN_IsMaxBuffersSupported(Source As System.IntPtr, MaxBuf As Integer) As Integer
        Return api.DTWAIN_IsMaxBuffersSupported(Source, MaxBuf)
        End Function
        
        Public Function DTWAIN_IsMemFileXferSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsMemFileXferSupported(Source)
        End Function
        
        Public Function DTWAIN_IsMsgNotifyEnabled() As Integer
        Return api.DTWAIN_IsMsgNotifyEnabled()
        End Function
        
        Public Function DTWAIN_IsNotifyTripletsEnabled() As Integer
        Return api.DTWAIN_IsNotifyTripletsEnabled()
        End Function
        
        Public Function DTWAIN_IsOCREngineActivated(OCREngine As System.IntPtr) As Integer
        Return api.DTWAIN_IsOCREngineActivated(OCREngine)
        End Function
        
        Public Function DTWAIN_IsOpenSourcesOnSelect() As Integer
        Return api.DTWAIN_IsOpenSourcesOnSelect()
        End Function
        
        Public Function DTWAIN_IsOrientationSupported(Source As System.IntPtr, Orientation As Integer) As Integer
        Return api.DTWAIN_IsOrientationSupported(Source, Orientation)
        End Function
        
        Public Function DTWAIN_IsOverscanSupported(Source As System.IntPtr, SupportValue As Integer) As Integer
        Return api.DTWAIN_IsOverscanSupported(Source, SupportValue)
        End Function
        
        Public Function DTWAIN_IsPDFSupported() As Integer
        Return api.DTWAIN_IsPDFSupported()
        End Function
        
        Public Function DTWAIN_IsPNGSupported() As Integer
        Return api.DTWAIN_IsPNGSupported()
        End Function
        
        Public Function DTWAIN_IsPaperDetectable(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsPaperDetectable(Source)
        End Function
        
        Public Function DTWAIN_IsPaperSizeSupported(Source As System.IntPtr, PaperSize As Integer) As Integer
        Return api.DTWAIN_IsPaperSizeSupported(Source, PaperSize)
        End Function
        
        Public Function DTWAIN_IsPatchCapsSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsPatchCapsSupported(Source)
        End Function
        
        Public Function DTWAIN_IsPatchDetectEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsPatchDetectEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsPatchSupported(Source As System.IntPtr, PatchCode As Integer) As Integer
        Return api.DTWAIN_IsPatchSupported(Source, PatchCode)
        End Function
        
        Public Function DTWAIN_IsPeekMessageLoopEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsPeekMessageLoopEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsPixelTypeSupported(Source As System.IntPtr, PixelType As Integer) As Integer
        Return api.DTWAIN_IsPixelTypeSupported(Source, PixelType)
        End Function
        
        Public Function DTWAIN_IsPrinterEnabled(Source As System.IntPtr, Printer As Integer) As Integer
        Return api.DTWAIN_IsPrinterEnabled(Source, Printer)
        End Function
        
        Public Function DTWAIN_IsPrinterSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsPrinterSupported(Source)
        End Function
        
        Public Function DTWAIN_IsRotationSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsRotationSupported(Source)
        End Function
        
        Public Function DTWAIN_IsSessionEnabled() As Integer
        Return api.DTWAIN_IsSessionEnabled()
        End Function
        
        Public Function DTWAIN_IsSkipImageInfoError(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsSkipImageInfoError(Source)
        End Function
        
        Public Function DTWAIN_IsSourceAcquiring(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsSourceAcquiring(Source)
        End Function
        
        Public Function DTWAIN_IsSourceAcquiringEx(Source As System.IntPtr, bUIOnly As Integer) As Integer
        Return api.DTWAIN_IsSourceAcquiringEx(Source, bUIOnly)
        End Function
        
        Public Function DTWAIN_IsSourceInUIOnlyMode(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsSourceInUIOnlyMode(Source)
        End Function
        
        Public Function DTWAIN_IsSourceOpen(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsSourceOpen(Source)
        End Function
        
        Public Function DTWAIN_IsSourceSelected(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsSourceSelected(Source)
        End Function
        
        Public Function DTWAIN_IsSourceValid(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsSourceValid(Source)
        End Function
        
        Public Function DTWAIN_IsTIFFSupported() As Integer
        Return api.DTWAIN_IsTIFFSupported()
        End Function
        
        Public Function DTWAIN_IsThumbnailEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsThumbnailEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsThumbnailSupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsThumbnailSupported(Source)
        End Function
        
        Public Function DTWAIN_IsTwainAvailable() As Integer
        Return api.DTWAIN_IsTwainAvailable()
        End Function
        
        Public Function DTWAIN_IsTwainAvailableEx(<MarshalAs(UnmanagedType.LPTStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_IsTwainAvailableEx(directories, nMaxLen)
        End Function
        
        Public Function DTWAIN_IsTwainAvailableExA(<MarshalAs(UnmanagedType.LPStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_IsTwainAvailableExA(directories, nMaxLen)
        End Function
        
        Public Function DTWAIN_IsTwainAvailableExW(<MarshalAs(UnmanagedType.LPWStr)> directories As StringBuilder, nMaxLen As Integer) As Integer
        Return api.DTWAIN_IsTwainAvailableExW(directories, nMaxLen)
        End Function
        
        Public Function DTWAIN_IsTwainMsg(ByRef pMsg As WinMsg) As Integer
        Return api.DTWAIN_IsTwainMsg(pMsg)
        End Function
        
        Public Function DTWAIN_IsUIControllable(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsUIControllable(Source)
        End Function
        
        Public Function DTWAIN_IsUIEnabled(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsUIEnabled(Source)
        End Function
        
        Public Function DTWAIN_IsUIOnlySupported(Source As System.IntPtr) As Integer
        Return api.DTWAIN_IsUIOnlySupported(Source)
        End Function
        
        Public Function DTWAIN_LoadCustomStringResources(sLangDLL As String) As Integer
        Return api.DTWAIN_LoadCustomStringResources(sLangDLL)
        End Function
        
        Public Function DTWAIN_LoadCustomStringResourcesA(sLangDLL As String) As Integer
        Return api.DTWAIN_LoadCustomStringResourcesA(sLangDLL)
        End Function
        
        Public Function DTWAIN_LoadCustomStringResourcesEx(sLangDLL As String, bClear As Integer) As Integer
        Return api.DTWAIN_LoadCustomStringResourcesEx(sLangDLL, bClear)
        End Function
        
        Public Function DTWAIN_LoadCustomStringResourcesExA(sLangDLL As String, bClear As Integer) As Integer
        Return api.DTWAIN_LoadCustomStringResourcesExA(sLangDLL, bClear)
        End Function
        
        Public Function DTWAIN_LoadCustomStringResourcesExW(sLangDLL As String, bClear As Integer) As Integer
        Return api.DTWAIN_LoadCustomStringResourcesExW(sLangDLL, bClear)
        End Function
        
        Public Function DTWAIN_LoadCustomStringResourcesW(sLangDLL As String) As Integer
        Return api.DTWAIN_LoadCustomStringResourcesW(sLangDLL)
        End Function
        
        Public Function DTWAIN_LoadLanguageResource(nLanguage As Integer) As Integer
        Return api.DTWAIN_LoadLanguageResource(nLanguage)
        End Function
        
        Public Function DTWAIN_LockMemory(h As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_LockMemory(h)
        End Function
        
        Public Function DTWAIN_LockMemoryEx(h As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_LockMemoryEx(h)
        End Function
        
        Public Function DTWAIN_LogMessage(message As String) As Integer
        Return api.DTWAIN_LogMessage(message)
        End Function
        
        Public Function DTWAIN_LogMessageA(message As String) As Integer
        Return api.DTWAIN_LogMessageA(message)
        End Function
        
        Public Function DTWAIN_LogMessageW(message As String) As Integer
        Return api.DTWAIN_LogMessageW(message)
        End Function
        
        Public Function DTWAIN_MakeRGB(red As Integer, green As Integer, blue As Integer) As Integer
        Return api.DTWAIN_MakeRGB(red, green, blue)
        End Function
        
        Public Function DTWAIN_OpenSource(Source As System.IntPtr) As Integer
        Return api.DTWAIN_OpenSource(Source)
        End Function
        
        Public Function DTWAIN_OpenSourcesOnSelect(bSet As Integer) As Integer
        Return api.DTWAIN_OpenSourcesOnSelect(bSet)
        End Function
        
        Public Function DTWAIN_RangeCreate(nEnumType As Integer) As System.IntPtr
        Return api.DTWAIN_RangeCreate(nEnumType)
        End Function
        
        Public Function DTWAIN_RangeCreateFromCap(Source As System.IntPtr, lCapType As Integer) As System.IntPtr
        Return api.DTWAIN_RangeCreateFromCap(Source, lCapType)
        End Function
        
        Public Function DTWAIN_RangeDestroy(pSource As System.IntPtr) As Integer
        Return api.DTWAIN_RangeDestroy(pSource)
        End Function
        
        Public Function DTWAIN_RangeExpand(pSource As System.IntPtr, ByRef pArray As System.IntPtr) As Integer
        Return api.DTWAIN_RangeExpand(pSource, pArray)
        End Function
        
        Public Function DTWAIN_RangeExpandEx(Range As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_RangeExpandEx(Range)
        End Function
        
        Public Function DTWAIN_RangeGetAll(pArray As System.IntPtr, pVariantLow As System.IntPtr, pVariantUp As System.IntPtr, pVariantStep As System.IntPtr, pVariantDefault As System.IntPtr, pVariantCurrent As System.IntPtr) As Integer
        Return api.DTWAIN_RangeGetAll(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent)
        End Function
        
        Public Function DTWAIN_RangeGetAllFloat(pArray As System.IntPtr, ByRef pVariantLow As System.Double, ByRef pVariantUp As System.Double, ByRef pVariantStep As System.Double, ByRef pVariantDefault As System.Double, ByRef pVariantCurrent As System.Double) As Integer
        Return api.DTWAIN_RangeGetAllFloat(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent)
        End Function
        
        Public Function DTWAIN_RangeGetAllFloatString(pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPTStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPTStr)> dCurrent As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetAllFloatString(pArray, dLow, dUp, dStep, dDefault, dCurrent)
        End Function
        
        Public Function DTWAIN_RangeGetAllFloatStringA(pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> dCurrent As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetAllFloatStringA(pArray, dLow, dUp, dStep, dDefault, dCurrent)
        End Function
        
        Public Function DTWAIN_RangeGetAllFloatStringW(pArray As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> dLow As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dUp As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dStep As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dDefault As StringBuilder, <MarshalAs(UnmanagedType.LPWStr)> dCurrent As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetAllFloatStringW(pArray, dLow, dUp, dStep, dDefault, dCurrent)
        End Function
        
        Public Function DTWAIN_RangeGetAllLong(pArray As System.IntPtr, ByRef pVariantLow As Integer, ByRef pVariantUp As Integer, ByRef pVariantStep As Integer, ByRef pVariantDefault As Integer, ByRef pVariantCurrent As Integer) As Integer
        Return api.DTWAIN_RangeGetAllLong(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent)
        End Function
        
        Public Function DTWAIN_RangeGetCount(pArray As System.IntPtr) As Integer
        Return api.DTWAIN_RangeGetCount(pArray)
        End Function
        
        Public Function DTWAIN_RangeGetExpValue(pArray As System.IntPtr, lPos As Integer, pVariant As System.IntPtr) As Integer
        Return api.DTWAIN_RangeGetExpValue(pArray, lPos, pVariant)
        End Function
        
        Public Function DTWAIN_RangeGetExpValueFloat(pArray As System.IntPtr, lPos As Integer, ByRef pVal As System.Double) As Integer
        Return api.DTWAIN_RangeGetExpValueFloat(pArray, lPos, pVal)
        End Function
        
        Public Function DTWAIN_RangeGetExpValueFloatString(pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPTStr)> pVal As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetExpValueFloatString(pArray, lPos, pVal)
        End Function
        
        Public Function DTWAIN_RangeGetExpValueFloatStringA(pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPStr)> pVal As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetExpValueFloatStringA(pArray, lPos, pVal)
        End Function
        
        Public Function DTWAIN_RangeGetExpValueFloatStringW(pArray As System.IntPtr, lPos As Integer, <MarshalAs(UnmanagedType.LPWStr)> pVal As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetExpValueFloatStringW(pArray, lPos, pVal)
        End Function
        
        Public Function DTWAIN_RangeGetExpValueLong(pArray As System.IntPtr, lPos As Integer, ByRef pVal As Integer) As Integer
        Return api.DTWAIN_RangeGetExpValueLong(pArray, lPos, pVal)
        End Function
        
        Public Function DTWAIN_RangeGetNearestValue(pArray As System.IntPtr, pVariantIn As System.IntPtr, pVariantOut As System.IntPtr, RoundType As Integer) As Integer
        Return api.DTWAIN_RangeGetNearestValue(pArray, pVariantIn, pVariantOut, RoundType)
        End Function
        
        Public Function DTWAIN_RangeGetPos(pArray As System.IntPtr, pVariant As System.IntPtr, ByRef pPos As Integer) As Integer
        Return api.DTWAIN_RangeGetPos(pArray, pVariant, pPos)
        End Function
        
        Public Function DTWAIN_RangeGetPosFloat(pArray As System.IntPtr, Val As System.Double, ByRef pPos As Integer) As Integer
        Return api.DTWAIN_RangeGetPosFloat(pArray, Val, pPos)
        End Function
        
        Public Function DTWAIN_RangeGetPosFloatString(pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
        Return api.DTWAIN_RangeGetPosFloatString(pArray, Val, pPos)
        End Function
        
        Public Function DTWAIN_RangeGetPosFloatStringA(pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
        Return api.DTWAIN_RangeGetPosFloatStringA(pArray, Val, pPos)
        End Function
        
        Public Function DTWAIN_RangeGetPosFloatStringW(pArray As System.IntPtr, Val As String, ByRef pPos As Integer) As Integer
        Return api.DTWAIN_RangeGetPosFloatStringW(pArray, Val, pPos)
        End Function
        
        Public Function DTWAIN_RangeGetPosLong(pArray As System.IntPtr, Value As Integer, ByRef pPos As Integer) As Integer
        Return api.DTWAIN_RangeGetPosLong(pArray, Value, pPos)
        End Function
        
        Public Function DTWAIN_RangeGetValue(pArray As System.IntPtr, nWhich As Integer, pVariant As System.IntPtr) As Integer
        Return api.DTWAIN_RangeGetValue(pArray, nWhich, pVariant)
        End Function
        
        Public Function DTWAIN_RangeGetValueFloat(pArray As System.IntPtr, nWhich As Integer, ByRef pVal As System.Double) As Integer
        Return api.DTWAIN_RangeGetValueFloat(pArray, nWhich, pVal)
        End Function
        
        Public Function DTWAIN_RangeGetValueFloatString(pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPTStr)> pVal As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetValueFloatString(pArray, nWhich, pVal)
        End Function
        
        Public Function DTWAIN_RangeGetValueFloatStringA(pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPStr)> dValue As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetValueFloatStringA(pArray, nWhich, dValue)
        End Function
        
        Public Function DTWAIN_RangeGetValueFloatStringW(pArray As System.IntPtr, nWhich As Integer, <MarshalAs(UnmanagedType.LPWStr)> dValue As StringBuilder) As Integer
        Return api.DTWAIN_RangeGetValueFloatStringW(pArray, nWhich, dValue)
        End Function
        
        Public Function DTWAIN_RangeGetValueLong(pArray As System.IntPtr, nWhich As Integer, ByRef pVal As Integer) As Integer
        Return api.DTWAIN_RangeGetValueLong(pArray, nWhich, pVal)
        End Function
        
        Public Function DTWAIN_RangeIsValid(Range As System.IntPtr, ByRef pStatus As Integer) As Integer
        Return api.DTWAIN_RangeIsValid(Range, pStatus)
        End Function
        
        Public Function DTWAIN_RangeNearestValueFloat(pArray As System.IntPtr, dIn As System.Double, ByRef pOut As System.Double, RoundType As Integer) As Integer
        Return api.DTWAIN_RangeNearestValueFloat(pArray, dIn, pOut, RoundType)
        End Function
        
        Public Function DTWAIN_RangeNearestValueFloatString(pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPTStr)> pOut As StringBuilder, RoundType As Integer) As Integer
        Return api.DTWAIN_RangeNearestValueFloatString(pArray, dIn, pOut, RoundType)
        End Function
        
        Public Function DTWAIN_RangeNearestValueFloatStringA(pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPStr)> dOut As StringBuilder, RoundType As Integer) As Integer
        Return api.DTWAIN_RangeNearestValueFloatStringA(pArray, dIn, dOut, RoundType)
        End Function
        
        Public Function DTWAIN_RangeNearestValueFloatStringW(pArray As System.IntPtr, dIn As String, <MarshalAs(UnmanagedType.LPWStr)> dOut As StringBuilder, RoundType As Integer) As Integer
        Return api.DTWAIN_RangeNearestValueFloatStringW(pArray, dIn, dOut, RoundType)
        End Function
        
        Public Function DTWAIN_RangeNearestValueLong(pArray As System.IntPtr, lIn As Integer, ByRef pOut As Integer, RoundType As Integer) As Integer
        Return api.DTWAIN_RangeNearestValueLong(pArray, lIn, pOut, RoundType)
        End Function
        
        Public Function DTWAIN_RangeSetAll(pArray As System.IntPtr, pVariantLow As System.IntPtr, pVariantUp As System.IntPtr, pVariantStep As System.IntPtr, pVariantDefault As System.IntPtr, pVariantCurrent As System.IntPtr) As Integer
        Return api.DTWAIN_RangeSetAll(pArray, pVariantLow, pVariantUp, pVariantStep, pVariantDefault, pVariantCurrent)
        End Function
        
        Public Function DTWAIN_RangeSetAllFloat(pArray As System.IntPtr, dLow As System.Double, dUp As System.Double, dStep As System.Double, dDefault As System.Double, dCurrent As System.Double) As Integer
        Return api.DTWAIN_RangeSetAllFloat(pArray, dLow, dUp, dStep, dDefault, dCurrent)
        End Function
        
        Public Function DTWAIN_RangeSetAllFloatString(pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
        Return api.DTWAIN_RangeSetAllFloatString(pArray, dLow, dUp, dStep, dDefault, dCurrent)
        End Function
        
        Public Function DTWAIN_RangeSetAllFloatStringA(pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
        Return api.DTWAIN_RangeSetAllFloatStringA(pArray, dLow, dUp, dStep, dDefault, dCurrent)
        End Function
        
        Public Function DTWAIN_RangeSetAllFloatStringW(pArray As System.IntPtr, dLow As String, dUp As String, dStep As String, dDefault As String, dCurrent As String) As Integer
        Return api.DTWAIN_RangeSetAllFloatStringW(pArray, dLow, dUp, dStep, dDefault, dCurrent)
        End Function
        
        Public Function DTWAIN_RangeSetAllLong(pArray As System.IntPtr, lLow As Integer, lUp As Integer, lStep As Integer, lDefault As Integer, lCurrent As Integer) As Integer
        Return api.DTWAIN_RangeSetAllLong(pArray, lLow, lUp, lStep, lDefault, lCurrent)
        End Function
        
        Public Function DTWAIN_RangeSetValue(pArray As System.IntPtr, nWhich As Integer, pVal As System.IntPtr) As Integer
        Return api.DTWAIN_RangeSetValue(pArray, nWhich, pVal)
        End Function
        
        Public Function DTWAIN_RangeSetValueFloat(pArray As System.IntPtr, nWhich As Integer, Val As System.Double) As Integer
        Return api.DTWAIN_RangeSetValueFloat(pArray, nWhich, Val)
        End Function
        
        Public Function DTWAIN_RangeSetValueFloatString(pArray As System.IntPtr, nWhich As Integer, Val As String) As Integer
        Return api.DTWAIN_RangeSetValueFloatString(pArray, nWhich, Val)
        End Function
        
        Public Function DTWAIN_RangeSetValueFloatStringA(pArray As System.IntPtr, nWhich As Integer, dValue As String) As Integer
        Return api.DTWAIN_RangeSetValueFloatStringA(pArray, nWhich, dValue)
        End Function
        
        Public Function DTWAIN_RangeSetValueFloatStringW(pArray As System.IntPtr, nWhich As Integer, dValue As String) As Integer
        Return api.DTWAIN_RangeSetValueFloatStringW(pArray, nWhich, dValue)
        End Function
        
        Public Function DTWAIN_RangeSetValueLong(pArray As System.IntPtr, nWhich As Integer, Val As Integer) As Integer
        Return api.DTWAIN_RangeSetValueLong(pArray, nWhich, Val)
        End Function
        
        Public Function DTWAIN_ResetPDFTextElement(TextElement As System.IntPtr) As Integer
        Return api.DTWAIN_ResetPDFTextElement(TextElement)
        End Function
        
        Public Function DTWAIN_RewindPage(Source As System.IntPtr) As Integer
        Return api.DTWAIN_RewindPage(Source)
        End Function
        
        Public Function DTWAIN_SelectDefaultOCREngine() As System.IntPtr
        Return api.DTWAIN_SelectDefaultOCREngine()
        End Function
        
        Public Function DTWAIN_SelectDefaultSource() As System.IntPtr
        Return api.DTWAIN_SelectDefaultSource()
        End Function
        
        Public Function DTWAIN_SelectDefaultSourceWithOpen(bOpen As Integer) As System.IntPtr
        Return api.DTWAIN_SelectDefaultSourceWithOpen(bOpen)
        End Function
        
        Public Function DTWAIN_SelectOCREngine() As System.IntPtr
        Return api.DTWAIN_SelectOCREngine()
        End Function
        
        Public Function DTWAIN_SelectOCREngine2(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectOCREngine2(hWndParent, szTitle, xPos, yPos, nOptions)
        End Function
        
        Public Function DTWAIN_SelectOCREngine2A(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectOCREngine2A(hWndParent, szTitle, xPos, yPos, nOptions)
        End Function
        
        Public Function DTWAIN_SelectOCREngine2Ex(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeFilter As String, szExcludeFilter As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectOCREngine2Ex(hWndParent, szTitle, xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions)
        End Function
        
        Public Function DTWAIN_SelectOCREngine2ExA(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectOCREngine2ExA(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions)
        End Function
        
        Public Function DTWAIN_SelectOCREngine2ExW(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectOCREngine2ExW(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions)
        End Function
        
        Public Function DTWAIN_SelectOCREngine2W(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectOCREngine2W(hWndParent, szTitle, xPos, yPos, nOptions)
        End Function
        
        Public Function DTWAIN_SelectOCREngineByName(lpszName As String) As System.IntPtr
        Return api.DTWAIN_SelectOCREngineByName(lpszName)
        End Function
        
        Public Function DTWAIN_SelectOCREngineByNameA(lpszName As String) As System.IntPtr
        Return api.DTWAIN_SelectOCREngineByNameA(lpszName)
        End Function
        
        Public Function DTWAIN_SelectOCREngineByNameW(lpszName As String) As System.IntPtr
        Return api.DTWAIN_SelectOCREngineByNameW(lpszName)
        End Function
        
        Public Function DTWAIN_SelectSource() As System.IntPtr
        Return api.DTWAIN_SelectSource()
        End Function
        
        Public Function DTWAIN_SelectSource2(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSource2(hWndParent, szTitle, xPos, yPos, nOptions)
        End Function
        
        Public Function DTWAIN_SelectSource2A(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSource2A(hWndParent, szTitle, xPos, yPos, nOptions)
        End Function
        
        Public Function DTWAIN_SelectSource2Ex(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeFilter As String, szExcludeFilter As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSource2Ex(hWndParent, szTitle, xPos, yPos, szIncludeFilter, szExcludeFilter, szNameMapping, nOptions)
        End Function
        
        Public Function DTWAIN_SelectSource2ExA(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSource2ExA(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions)
        End Function
        
        Public Function DTWAIN_SelectSource2ExW(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, szIncludeNames As String, szExcludeNames As String, szNameMapping As String, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSource2ExW(hWndParent, szTitle, xPos, yPos, szIncludeNames, szExcludeNames, szNameMapping, nOptions)
        End Function
        
        Public Function DTWAIN_SelectSource2W(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nOptions As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSource2W(hWndParent, szTitle, xPos, yPos, nOptions)
        End Function
        
        Public Function DTWAIN_SelectSourceByName(lpszName As String) As System.IntPtr
        Return api.DTWAIN_SelectSourceByName(lpszName)
        End Function
        
        Public Function DTWAIN_SelectSourceByNameA(lpszName As String) As System.IntPtr
        Return api.DTWAIN_SelectSourceByNameA(lpszName)
        End Function
        
        Public Function DTWAIN_SelectSourceByNameW(lpszName As String) As System.IntPtr
        Return api.DTWAIN_SelectSourceByNameW(lpszName)
        End Function
        
        Public Function DTWAIN_SelectSourceByNameWithOpen(lpszName As String, bOpen As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSourceByNameWithOpen(lpszName, bOpen)
        End Function
        
        Public Function DTWAIN_SelectSourceByNameWithOpenA(lpszName As String, bOpen As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSourceByNameWithOpenA(lpszName, bOpen)
        End Function
        
        Public Function DTWAIN_SelectSourceByNameWithOpenW(lpszName As String, bOpen As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSourceByNameWithOpenW(lpszName, bOpen)
        End Function
        
        Public Function DTWAIN_SelectSourceWithOpen(bOpen As Integer) As System.IntPtr
        Return api.DTWAIN_SelectSourceWithOpen(bOpen)
        End Function
        
        Public Function DTWAIN_SetAcquireArea(Source As System.IntPtr, lSetType As Integer, FloatEnum As System.IntPtr, ActualEnum As System.IntPtr) As Integer
        Return api.DTWAIN_SetAcquireArea(Source, lSetType, FloatEnum, ActualEnum)
        End Function
        
        Public Function DTWAIN_SetAcquireArea2(Source As System.IntPtr, left As System.Double, top As System.Double, right As System.Double, bottom As System.Double, lUnit As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_SetAcquireArea2(Source, left, top, right, bottom, lUnit, Flags)
        End Function
        
        Public Function DTWAIN_SetAcquireArea2String(Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_SetAcquireArea2String(Source, left, top, right, bottom, lUnit, Flags)
        End Function
        
        Public Function DTWAIN_SetAcquireArea2StringA(Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_SetAcquireArea2StringA(Source, left, top, right, bottom, lUnit, Flags)
        End Function
        
        Public Function DTWAIN_SetAcquireArea2StringW(Source As System.IntPtr, left As String, top As String, right As String, bottom As String, lUnit As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_SetAcquireArea2StringW(Source, left, top, right, bottom, lUnit, Flags)
        End Function
        
        Public Function DTWAIN_SetAcquireImageNegative(Source As System.IntPtr, IsNegative As Integer) As Integer
        Return api.DTWAIN_SetAcquireImageNegative(Source, IsNegative)
        End Function
        
        Public Function DTWAIN_SetAcquireImageScale(Source As System.IntPtr, xscale As System.Double, yscale As System.Double) As Integer
        Return api.DTWAIN_SetAcquireImageScale(Source, xscale, yscale)
        End Function
        
        Public Function DTWAIN_SetAcquireImageScaleString(Source As System.IntPtr, xscale As String, yscale As String) As Integer
        Return api.DTWAIN_SetAcquireImageScaleString(Source, xscale, yscale)
        End Function
        
        Public Function DTWAIN_SetAcquireImageScaleStringA(Source As System.IntPtr, xscale As String, yscale As String) As Integer
        Return api.DTWAIN_SetAcquireImageScaleStringA(Source, xscale, yscale)
        End Function
        
        Public Function DTWAIN_SetAcquireImageScaleStringW(Source As System.IntPtr, xscale As String, yscale As String) As Integer
        Return api.DTWAIN_SetAcquireImageScaleStringW(Source, xscale, yscale)
        End Function
        
        Public Function DTWAIN_SetAcquireStripBuffer(Source As System.IntPtr, hMem As System.IntPtr) As Integer
        Return api.DTWAIN_SetAcquireStripBuffer(Source, hMem)
        End Function
        
        Public Function DTWAIN_SetAcquireStripSize(Source As System.IntPtr, StripSize As UInteger) As Integer
        Return api.DTWAIN_SetAcquireStripSize(Source, StripSize)
        End Function
        
        Public Function DTWAIN_SetAlarmVolume(Source As System.IntPtr, Volume As Integer) As Integer
        Return api.DTWAIN_SetAlarmVolume(Source, Volume)
        End Function
        
        Public Function DTWAIN_SetAlarms(Source As System.IntPtr, Alarms As System.IntPtr) As Integer
        Return api.DTWAIN_SetAlarms(Source, Alarms)
        End Function
        
        Public Function DTWAIN_SetAllCapsToDefault(Source As System.IntPtr) As Integer
        Return api.DTWAIN_SetAllCapsToDefault(Source)
        End Function
        
        Public Function DTWAIN_SetAppInfo(szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
        Return api.DTWAIN_SetAppInfo(szVerStr, szManu, szProdFam, szProdName)
        End Function
        
        Public Function DTWAIN_SetAppInfoA(szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
        Return api.DTWAIN_SetAppInfoA(szVerStr, szManu, szProdFam, szProdName)
        End Function
        
        Public Function DTWAIN_SetAppInfoW(szVerStr As String, szManu As String, szProdFam As String, szProdName As String) As Integer
        Return api.DTWAIN_SetAppInfoW(szVerStr, szManu, szProdFam, szProdName)
        End Function
        
        Public Function DTWAIN_SetAuthor(Source As System.IntPtr, szAuthor As String) As Integer
        Return api.DTWAIN_SetAuthor(Source, szAuthor)
        End Function
        
        Public Function DTWAIN_SetAuthorA(Source As System.IntPtr, szAuthor As String) As Integer
        Return api.DTWAIN_SetAuthorA(Source, szAuthor)
        End Function
        
        Public Function DTWAIN_SetAuthorW(Source As System.IntPtr, szAuthor As String) As Integer
        Return api.DTWAIN_SetAuthorW(Source, szAuthor)
        End Function
        
        Public Function DTWAIN_SetAvailablePrinters(Source As System.IntPtr, lpAvailPrinters As Integer) As Integer
        Return api.DTWAIN_SetAvailablePrinters(Source, lpAvailPrinters)
        End Function
        
        Public Function DTWAIN_SetAvailablePrintersArray(Source As System.IntPtr, AvailPrinters As System.IntPtr) As Integer
        Return api.DTWAIN_SetAvailablePrintersArray(Source, AvailPrinters)
        End Function
        
        Public Function DTWAIN_SetBitDepth(Source As System.IntPtr, BitDepth As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetBitDepth(Source, BitDepth, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetection(Source As System.IntPtr, threshold As System.Double, discard_option As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetection(Source, threshold, discard_option, bSet)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetectionEx(Source As System.IntPtr, threshold As System.Double, autodetect As Integer, detectOpts As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetectionEx(Source, threshold, autodetect, detectOpts, bSet)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetectionExString(Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetectionExString(Source, threshold, autodetect_option, detectOpts, bSet)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetectionExStringA(Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetectionExStringA(Source, threshold, autodetect_option, detectOpts, bSet)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetectionExStringW(Source As System.IntPtr, threshold As String, autodetect_option As Integer, detectOpts As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetectionExStringW(Source, threshold, autodetect_option, detectOpts, bSet)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetectionString(Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetectionString(Source, threshold, autodetect_option, bSet)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetectionStringA(Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetectionStringA(Source, threshold, autodetect_option, bSet)
        End Function
        
        Public Function DTWAIN_SetBlankPageDetectionStringW(Source As System.IntPtr, threshold As String, autodetect_option As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetBlankPageDetectionStringW(Source, threshold, autodetect_option, bSet)
        End Function
        
        Public Function DTWAIN_SetBrightness(Source As System.IntPtr, Brightness As System.Double) As Integer
        Return api.DTWAIN_SetBrightness(Source, Brightness)
        End Function
        
        Public Function DTWAIN_SetBrightnessString(Source As System.IntPtr, Brightness As String) As Integer
        Return api.DTWAIN_SetBrightnessString(Source, Brightness)
        End Function
        
        Public Function DTWAIN_SetBrightnessStringA(Source As System.IntPtr, Contrast As String) As Integer
        Return api.DTWAIN_SetBrightnessStringA(Source, Contrast)
        End Function
        
        Public Function DTWAIN_SetBrightnessStringW(Source As System.IntPtr, Contrast As String) As Integer
        Return api.DTWAIN_SetBrightnessStringW(Source, Contrast)
        End Function
        
        Public Function DTWAIN_SetBufferedTileMode(Source As System.IntPtr, bTileMode As Integer) As Integer
        Return api.DTWAIN_SetBufferedTileMode(Source, bTileMode)
        End Function
        
        Public Function DTWAIN_SetCallback(Fn As DTwainCallback, UserData As Integer) As DTwainCallback
        Return api.DTWAIN_SetCallback(Fn, UserData)
        End Function
        
        Public Function DTWAIN_SetCallback64(Fn As DTwainCallback64, UserData As System.Int64) As DTwainCallback64
        Return api.DTWAIN_SetCallback64(Fn, UserData)
        End Function
        
        Public Function DTWAIN_SetCamera(Source As System.IntPtr, szCamera As String) As Integer
        Return api.DTWAIN_SetCamera(Source, szCamera)
        End Function
        
        Public Function DTWAIN_SetCameraA(Source As System.IntPtr, szCamera As String) As Integer
        Return api.DTWAIN_SetCameraA(Source, szCamera)
        End Function
        
        Public Function DTWAIN_SetCameraW(Source As System.IntPtr, szCamera As String) As Integer
        Return api.DTWAIN_SetCameraW(Source, szCamera)
        End Function
        
        Public Function DTWAIN_SetCapValues(Source As System.IntPtr, lCap As Integer, lSetType As Integer, pArray As System.IntPtr) As Integer
        Return api.DTWAIN_SetCapValues(Source, lCap, lSetType, pArray)
        End Function
        
        Public Function DTWAIN_SetCapValuesEx(Source As System.IntPtr, lCap As Integer, lSetType As Integer, lContainerType As Integer, pArray As System.IntPtr) As Integer
        Return api.DTWAIN_SetCapValuesEx(Source, lCap, lSetType, lContainerType, pArray)
        End Function
        
        Public Function DTWAIN_SetCapValuesEx2(Source As System.IntPtr, lCap As Integer, lSetType As Integer, lContainerType As Integer, nDataType As Integer, pArray As System.IntPtr) As Integer
        Return api.DTWAIN_SetCapValuesEx2(Source, lCap, lSetType, lContainerType, nDataType, pArray)
        End Function
        
        Public Function DTWAIN_SetCaption(Source As System.IntPtr, Caption As String) As Integer
        Return api.DTWAIN_SetCaption(Source, Caption)
        End Function
        
        Public Function DTWAIN_SetCaptionA(Source As System.IntPtr, Caption As String) As Integer
        Return api.DTWAIN_SetCaptionA(Source, Caption)
        End Function
        
        Public Function DTWAIN_SetCaptionW(Source As System.IntPtr, Caption As String) As Integer
        Return api.DTWAIN_SetCaptionW(Source, Caption)
        End Function
        
        Public Function DTWAIN_SetCompressionType(Source As System.IntPtr, lCompression As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetCompressionType(Source, lCompression, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetContrast(Source As System.IntPtr, Contrast As System.Double) As Integer
        Return api.DTWAIN_SetContrast(Source, Contrast)
        End Function
        
        Public Function DTWAIN_SetContrastString(Source As System.IntPtr, Contrast As String) As Integer
        Return api.DTWAIN_SetContrastString(Source, Contrast)
        End Function
        
        Public Function DTWAIN_SetContrastStringA(Source As System.IntPtr, Contrast As String) As Integer
        Return api.DTWAIN_SetContrastStringA(Source, Contrast)
        End Function
        
        Public Function DTWAIN_SetContrastStringW(Source As System.IntPtr, Contrast As String) As Integer
        Return api.DTWAIN_SetContrastStringW(Source, Contrast)
        End Function
        
        Public Function DTWAIN_SetCountry(nCountry As Integer) As Integer
        Return api.DTWAIN_SetCountry(nCountry)
        End Function
        
        Public Function DTWAIN_SetCurrentRetryCount(Source As System.IntPtr, nCount As Integer) As Integer
        Return api.DTWAIN_SetCurrentRetryCount(Source, nCount)
        End Function
        
        Public Function DTWAIN_SetCustomDSData(Source As System.IntPtr, hData As System.IntPtr, <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U8, SizeParamIndex:=3)> Data As Byte(), dSize As UInteger, nFlags As Integer) As Integer
        Return api.DTWAIN_SetCustomDSData(Source, hData, Data, dSize, nFlags)
        End Function
        
        Public Function DTWAIN_SetDSMSearchOrder(SearchPath As Integer) As Integer
        Return api.DTWAIN_SetDSMSearchOrder(SearchPath)
        End Function
        
        Public Function DTWAIN_SetDSMSearchOrderEx(SearchOrder As String, UserPath As String) As Integer
        Return api.DTWAIN_SetDSMSearchOrderEx(SearchOrder, UserPath)
        End Function
        
        Public Function DTWAIN_SetDSMSearchOrderExA(SearchOrder As String, szUserPath As String) As Integer
        Return api.DTWAIN_SetDSMSearchOrderExA(SearchOrder, szUserPath)
        End Function
        
        Public Function DTWAIN_SetDSMSearchOrderExW(SearchOrder As String, szUserPath As String) As Integer
        Return api.DTWAIN_SetDSMSearchOrderExW(SearchOrder, szUserPath)
        End Function
        
        Public Function DTWAIN_SetDefaultSource(Source As System.IntPtr) As Integer
        Return api.DTWAIN_SetDefaultSource(Source)
        End Function
        
        Public Function DTWAIN_SetDeviceNotifications(Source As System.IntPtr, DevEvents As Integer) As Integer
        Return api.DTWAIN_SetDeviceNotifications(Source, DevEvents)
        End Function
        
        Public Function DTWAIN_SetDeviceTimeDate(Source As System.IntPtr, szTimeDate As String) As Integer
        Return api.DTWAIN_SetDeviceTimeDate(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_SetDeviceTimeDateA(Source As System.IntPtr, szTimeDate As String) As Integer
        Return api.DTWAIN_SetDeviceTimeDateA(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_SetDeviceTimeDateW(Source As System.IntPtr, szTimeDate As String) As Integer
        Return api.DTWAIN_SetDeviceTimeDateW(Source, szTimeDate)
        End Function
        
        Public Function DTWAIN_SetDoubleFeedDetectLength(Source As System.IntPtr, Value As System.Double) As Integer
        Return api.DTWAIN_SetDoubleFeedDetectLength(Source, Value)
        End Function
        
        Public Function DTWAIN_SetDoubleFeedDetectLengthString(Source As System.IntPtr, value As String) As Integer
        Return api.DTWAIN_SetDoubleFeedDetectLengthString(Source, value)
        End Function
        
        Public Function DTWAIN_SetDoubleFeedDetectLengthStringA(Source As System.IntPtr, szLength As String) As Integer
        Return api.DTWAIN_SetDoubleFeedDetectLengthStringA(Source, szLength)
        End Function
        
        Public Function DTWAIN_SetDoubleFeedDetectLengthStringW(Source As System.IntPtr, szLength As String) As Integer
        Return api.DTWAIN_SetDoubleFeedDetectLengthStringW(Source, szLength)
        End Function
        
        Public Function DTWAIN_SetDoubleFeedDetectValues(Source As System.IntPtr, prray As System.IntPtr) As Integer
        Return api.DTWAIN_SetDoubleFeedDetectValues(Source, prray)
        End Function
        
        Public Function DTWAIN_SetDoublePageCountOnDuplex(Source As System.IntPtr, bDoubleCount As Integer) As Integer
        Return api.DTWAIN_SetDoublePageCountOnDuplex(Source, bDoubleCount)
        End Function
        
        Public Function DTWAIN_SetEOJDetectValue(Source As System.IntPtr, nValue As Integer) As Integer
        Return api.DTWAIN_SetEOJDetectValue(Source, nValue)
        End Function
        
        Public Function DTWAIN_SetErrorBufferThreshold(nErrors As Integer) As Integer
        Return api.DTWAIN_SetErrorBufferThreshold(nErrors)
        End Function
        
        Public Function DTWAIN_SetErrorCallback(proc As DTwainErrorProc, UserData As Integer) As Integer
        Return api.DTWAIN_SetErrorCallback(proc, UserData)
        End Function
        
        Public Function DTWAIN_SetErrorCallback64(proc As DTwainErrorProc64, UserData64 As System.Int64) As Integer
        Return api.DTWAIN_SetErrorCallback64(proc, UserData64)
        End Function
        
        Public Function DTWAIN_SetFeederAlignment(Source As System.IntPtr, lpAlignment As Integer) As Integer
        Return api.DTWAIN_SetFeederAlignment(Source, lpAlignment)
        End Function
        
        Public Function DTWAIN_SetFeederOrder(Source As System.IntPtr, lOrder As Integer) As Integer
        Return api.DTWAIN_SetFeederOrder(Source, lOrder)
        End Function
        
        Public Function DTWAIN_SetFeederWaitTime(Source As System.IntPtr, waitTime As Integer, flags As Integer) As Integer
        Return api.DTWAIN_SetFeederWaitTime(Source, waitTime, flags)
        End Function
        
        Public Function DTWAIN_SetFileAutoIncrement(Source As System.IntPtr, Increment As Integer, bResetOnAcquire As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetFileAutoIncrement(Source, Increment, bResetOnAcquire, bSet)
        End Function
        
        Public Function DTWAIN_SetFileCompressionType(Source As System.IntPtr, lCompression As Integer, bIsCustom As Integer) As Integer
        Return api.DTWAIN_SetFileCompressionType(Source, lCompression, bIsCustom)
        End Function
        
        Public Function DTWAIN_SetFileSavePos(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
        Return api.DTWAIN_SetFileSavePos(hWndParent, szTitle, xPos, yPos, nFlags)
        End Function
        
        Public Function DTWAIN_SetFileSavePosA(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
        Return api.DTWAIN_SetFileSavePosA(hWndParent, szTitle, xPos, yPos, nFlags)
        End Function
        
        Public Function DTWAIN_SetFileSavePosW(hWndParent As System.IntPtr, szTitle As String, xPos As Integer, yPos As Integer, nFlags As Integer) As Integer
        Return api.DTWAIN_SetFileSavePosW(hWndParent, szTitle, xPos, yPos, nFlags)
        End Function
        
        Public Function DTWAIN_SetFileXferFormat(Source As System.IntPtr, lFileType As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetFileXferFormat(Source, lFileType, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetHalftone(Source As System.IntPtr, lpHalftone As String) As Integer
        Return api.DTWAIN_SetHalftone(Source, lpHalftone)
        End Function
        
        Public Function DTWAIN_SetHalftoneA(Source As System.IntPtr, lpHalftone As String) As Integer
        Return api.DTWAIN_SetHalftoneA(Source, lpHalftone)
        End Function
        
        Public Function DTWAIN_SetHalftoneW(Source As System.IntPtr, lpHalftone As String) As Integer
        Return api.DTWAIN_SetHalftoneW(Source, lpHalftone)
        End Function
        
        Public Function DTWAIN_SetHighlight(Source As System.IntPtr, Highlight As System.Double) As Integer
        Return api.DTWAIN_SetHighlight(Source, Highlight)
        End Function
        
        Public Function DTWAIN_SetHighlightString(Source As System.IntPtr, Highlight As String) As Integer
        Return api.DTWAIN_SetHighlightString(Source, Highlight)
        End Function
        
        Public Function DTWAIN_SetHighlightStringA(Source As System.IntPtr, Highlight As String) As Integer
        Return api.DTWAIN_SetHighlightStringA(Source, Highlight)
        End Function
        
        Public Function DTWAIN_SetHighlightStringW(Source As System.IntPtr, Highlight As String) As Integer
        Return api.DTWAIN_SetHighlightStringW(Source, Highlight)
        End Function
        
        Public Function DTWAIN_SetJobControl(Source As System.IntPtr, JobControl As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetJobControl(Source, JobControl, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetJpegValues(Source As System.IntPtr, Quality As Integer, Progressive As Integer) As Integer
        Return api.DTWAIN_SetJpegValues(Source, Quality, Progressive)
        End Function
        
        Public Function DTWAIN_SetJpegXRValues(Source As System.IntPtr, Quality As Integer, Progressive As Integer) As Integer
        Return api.DTWAIN_SetJpegXRValues(Source, Quality, Progressive)
        End Function
        
        Public Function DTWAIN_SetLanguage(nLanguage As Integer) As Integer
        Return api.DTWAIN_SetLanguage(nLanguage)
        End Function
        
        Public Function DTWAIN_SetLastError(nError As Integer) As Integer
        Return api.DTWAIN_SetLastError(nError)
        End Function
        
        Public Function DTWAIN_SetLightPath(Source As System.IntPtr, LightPath As Integer) As Integer
        Return api.DTWAIN_SetLightPath(Source, LightPath)
        End Function
        
        Public Function DTWAIN_SetLightPathEx(Source As System.IntPtr, LightPaths As System.IntPtr) As Integer
        Return api.DTWAIN_SetLightPathEx(Source, LightPaths)
        End Function
        
        Public Function DTWAIN_SetLightSource(Source As System.IntPtr, LightSource As Integer) As Integer
        Return api.DTWAIN_SetLightSource(Source, LightSource)
        End Function
        
        Public Function DTWAIN_SetLightSources(Source As System.IntPtr, LightSources As System.IntPtr) As Integer
        Return api.DTWAIN_SetLightSources(Source, LightSources)
        End Function
        
        Public Function DTWAIN_SetLoggerCallback(logProc As DTwainLoggerProc, UserData As System.Int64) As Integer
        Return api.DTWAIN_SetLoggerCallback(logProc, UserData)
        End Function
        
        Public Function DTWAIN_SetLoggerCallbackA(logProc As DTwainLoggerProcA, UserData As System.Int64) As Integer
        Return api.DTWAIN_SetLoggerCallbackA(logProc, UserData)
        End Function
        
        Public Function DTWAIN_SetLoggerCallbackW(logProc As DTwainLoggerProcW, UserData As System.Int64) As Integer
        Return api.DTWAIN_SetLoggerCallbackW(logProc, UserData)
        End Function
        
        Public Function DTWAIN_SetManualDuplexMode(Source As System.IntPtr, Flags As Integer, bSet As Integer) As Integer
        Return api.DTWAIN_SetManualDuplexMode(Source, Flags, bSet)
        End Function
        
        Public Function DTWAIN_SetMaxAcquisitions(Source As System.IntPtr, MaxAcquires As Integer) As Integer
        Return api.DTWAIN_SetMaxAcquisitions(Source, MaxAcquires)
        End Function
        
        Public Function DTWAIN_SetMaxBuffers(Source As System.IntPtr, MaxBuf As Integer) As Integer
        Return api.DTWAIN_SetMaxBuffers(Source, MaxBuf)
        End Function
        
        Public Function DTWAIN_SetMaxRetryAttempts(Source As System.IntPtr, nAttempts As Integer) As Integer
        Return api.DTWAIN_SetMaxRetryAttempts(Source, nAttempts)
        End Function
        
        Public Function DTWAIN_SetMultipageScanMode(Source As System.IntPtr, ScanType As Integer) As Integer
        Return api.DTWAIN_SetMultipageScanMode(Source, ScanType)
        End Function
        
        Public Function DTWAIN_SetNoiseFilter(Source As System.IntPtr, NoiseFilter As Integer) As Integer
        Return api.DTWAIN_SetNoiseFilter(Source, NoiseFilter)
        End Function
        
        Public Function DTWAIN_SetOCRCapValues(Engine As System.IntPtr, OCRCapValue As Integer, SetType As Integer, CapValues As System.IntPtr) As Integer
        Return api.DTWAIN_SetOCRCapValues(Engine, OCRCapValue, SetType, CapValues)
        End Function
        
        Public Function DTWAIN_SetOrientation(Source As System.IntPtr, Orient As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetOrientation(Source, Orient, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetOverscan(Source As System.IntPtr, Value As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetOverscan(Source, Value, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetPDFAESEncryption(Source As System.IntPtr, nWhichEncryption As Integer, bUseAES As Integer) As Integer
        Return api.DTWAIN_SetPDFAESEncryption(Source, nWhichEncryption, bUseAES)
        End Function
        
        Public Function DTWAIN_SetPDFASCIICompression(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_SetPDFASCIICompression(Source, bSet)
        End Function
        
        Public Function DTWAIN_SetPDFAuthor(Source As System.IntPtr, lpAuthor As String) As Integer
        Return api.DTWAIN_SetPDFAuthor(Source, lpAuthor)
        End Function
        
        Public Function DTWAIN_SetPDFAuthorA(Source As System.IntPtr, lpAuthor As String) As Integer
        Return api.DTWAIN_SetPDFAuthorA(Source, lpAuthor)
        End Function
        
        Public Function DTWAIN_SetPDFAuthorW(Source As System.IntPtr, lpAuthor As String) As Integer
        Return api.DTWAIN_SetPDFAuthorW(Source, lpAuthor)
        End Function
        
        Public Function DTWAIN_SetPDFCompression(Source As System.IntPtr, bCompression As Integer) As Integer
        Return api.DTWAIN_SetPDFCompression(Source, bCompression)
        End Function
        
        Public Function DTWAIN_SetPDFCreator(Source As System.IntPtr, lpCreator As String) As Integer
        Return api.DTWAIN_SetPDFCreator(Source, lpCreator)
        End Function
        
        Public Function DTWAIN_SetPDFCreatorA(Source As System.IntPtr, lpCreator As String) As Integer
        Return api.DTWAIN_SetPDFCreatorA(Source, lpCreator)
        End Function
        
        Public Function DTWAIN_SetPDFCreatorW(Source As System.IntPtr, lpCreator As String) As Integer
        Return api.DTWAIN_SetPDFCreatorW(Source, lpCreator)
        End Function
        
        Public Function DTWAIN_SetPDFEncryption(Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
        Return api.DTWAIN_SetPDFEncryption(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption)
        End Function
        
        Public Function DTWAIN_SetPDFEncryptionA(Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
        Return api.DTWAIN_SetPDFEncryptionA(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption)
        End Function
        
        Public Function DTWAIN_SetPDFEncryptionW(Source As System.IntPtr, bUseEncryption As Integer, lpszUser As String, lpszOwner As String, Permissions As UInteger, UseStrongEncryption As Integer) As Integer
        Return api.DTWAIN_SetPDFEncryptionW(Source, bUseEncryption, lpszUser, lpszOwner, Permissions, UseStrongEncryption)
        End Function
        
        Public Function DTWAIN_SetPDFJpegQuality(Source As System.IntPtr, Quality As Integer) As Integer
        Return api.DTWAIN_SetPDFJpegQuality(Source, Quality)
        End Function
        
        Public Function DTWAIN_SetPDFKeywords(Source As System.IntPtr, lpKeyWords As String) As Integer
        Return api.DTWAIN_SetPDFKeywords(Source, lpKeyWords)
        End Function
        
        Public Function DTWAIN_SetPDFKeywordsA(Source As System.IntPtr, lpKeyWords As String) As Integer
        Return api.DTWAIN_SetPDFKeywordsA(Source, lpKeyWords)
        End Function
        
        Public Function DTWAIN_SetPDFKeywordsW(Source As System.IntPtr, lpKeyWords As String) As Integer
        Return api.DTWAIN_SetPDFKeywordsW(Source, lpKeyWords)
        End Function
        
        Public Function DTWAIN_SetPDFOCRConversion(Engine As System.IntPtr, PageType As Integer, FileType As Integer, PixelType As Integer, BitDepth As Integer, Options As Integer) As Integer
        Return api.DTWAIN_SetPDFOCRConversion(Engine, PageType, FileType, PixelType, BitDepth, Options)
        End Function
        
        Public Function DTWAIN_SetPDFOCRMode(Source As System.IntPtr, bSet As Integer) As Integer
        Return api.DTWAIN_SetPDFOCRMode(Source, bSet)
        End Function
        
        Public Function DTWAIN_SetPDFOrientation(Source As System.IntPtr, lPOrientation As Integer) As Integer
        Return api.DTWAIN_SetPDFOrientation(Source, lPOrientation)
        End Function
        
        Public Function DTWAIN_SetPDFPageScale(Source As System.IntPtr, nOptions As Integer, xScale As System.Double, yScale As System.Double) As Integer
        Return api.DTWAIN_SetPDFPageScale(Source, nOptions, xScale, yScale)
        End Function
        
        Public Function DTWAIN_SetPDFPageScaleString(Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
        Return api.DTWAIN_SetPDFPageScaleString(Source, nOptions, xScale, yScale)
        End Function
        
        Public Function DTWAIN_SetPDFPageScaleStringA(Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
        Return api.DTWAIN_SetPDFPageScaleStringA(Source, nOptions, xScale, yScale)
        End Function
        
        Public Function DTWAIN_SetPDFPageScaleStringW(Source As System.IntPtr, nOptions As Integer, xScale As String, yScale As String) As Integer
        Return api.DTWAIN_SetPDFPageScaleStringW(Source, nOptions, xScale, yScale)
        End Function
        
        Public Function DTWAIN_SetPDFPageSize(Source As System.IntPtr, PageSize As Integer, CustomWidth As System.Double, CustomHeight As System.Double) As Integer
        Return api.DTWAIN_SetPDFPageSize(Source, PageSize, CustomWidth, CustomHeight)
        End Function
        
        Public Function DTWAIN_SetPDFPageSizeString(Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
        Return api.DTWAIN_SetPDFPageSizeString(Source, PageSize, CustomWidth, CustomHeight)
        End Function
        
        Public Function DTWAIN_SetPDFPageSizeStringA(Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
        Return api.DTWAIN_SetPDFPageSizeStringA(Source, PageSize, CustomWidth, CustomHeight)
        End Function
        
        Public Function DTWAIN_SetPDFPageSizeStringW(Source As System.IntPtr, PageSize As Integer, CustomWidth As String, CustomHeight As String) As Integer
        Return api.DTWAIN_SetPDFPageSizeStringW(Source, PageSize, CustomWidth, CustomHeight)
        End Function
        
        Public Function DTWAIN_SetPDFPolarity(Source As System.IntPtr, Polarity As Integer) As Integer
        Return api.DTWAIN_SetPDFPolarity(Source, Polarity)
        End Function
        
        Public Function DTWAIN_SetPDFProducer(Source As System.IntPtr, lpProducer As String) As Integer
        Return api.DTWAIN_SetPDFProducer(Source, lpProducer)
        End Function
        
        Public Function DTWAIN_SetPDFProducerA(Source As System.IntPtr, lpProducer As String) As Integer
        Return api.DTWAIN_SetPDFProducerA(Source, lpProducer)
        End Function
        
        Public Function DTWAIN_SetPDFProducerW(Source As System.IntPtr, lpProducer As String) As Integer
        Return api.DTWAIN_SetPDFProducerW(Source, lpProducer)
        End Function
        
        Public Function DTWAIN_SetPDFSubject(Source As System.IntPtr, lpSubject As String) As Integer
        Return api.DTWAIN_SetPDFSubject(Source, lpSubject)
        End Function
        
        Public Function DTWAIN_SetPDFSubjectA(Source As System.IntPtr, lpSubject As String) As Integer
        Return api.DTWAIN_SetPDFSubjectA(Source, lpSubject)
        End Function
        
        Public Function DTWAIN_SetPDFSubjectW(Source As System.IntPtr, lpSubject As String) As Integer
        Return api.DTWAIN_SetPDFSubjectW(Source, lpSubject)
        End Function
        
        Public Function DTWAIN_SetPDFTextElementFloat(TextElement As System.IntPtr, val1 As System.Double, val2 As System.Double, Flags As Integer) As Integer
        Return api.DTWAIN_SetPDFTextElementFloat(TextElement, val1, val2, Flags)
        End Function
        
        Public Function DTWAIN_SetPDFTextElementLong(TextElement As System.IntPtr, val1 As Integer, val2 As Integer, Flags As Integer) As Integer
        Return api.DTWAIN_SetPDFTextElementLong(TextElement, val1, val2, Flags)
        End Function
        
        Public Function DTWAIN_SetPDFTextElementString(TextElement As System.IntPtr, val1 As String, Flags As Integer) As Integer
        Return api.DTWAIN_SetPDFTextElementString(TextElement, val1, Flags)
        End Function
        
        Public Function DTWAIN_SetPDFTextElementStringA(TextElement As System.IntPtr, szString As String, Flags As Integer) As Integer
        Return api.DTWAIN_SetPDFTextElementStringA(TextElement, szString, Flags)
        End Function
        
        Public Function DTWAIN_SetPDFTextElementStringW(TextElement As System.IntPtr, szString As String, Flags As Integer) As Integer
        Return api.DTWAIN_SetPDFTextElementStringW(TextElement, szString, Flags)
        End Function
        
        Public Function DTWAIN_SetPDFTitle(Source As System.IntPtr, lpTitle As String) As Integer
        Return api.DTWAIN_SetPDFTitle(Source, lpTitle)
        End Function
        
        Public Function DTWAIN_SetPDFTitleA(Source As System.IntPtr, lpTitle As String) As Integer
        Return api.DTWAIN_SetPDFTitleA(Source, lpTitle)
        End Function
        
        Public Function DTWAIN_SetPDFTitleW(Source As System.IntPtr, lpTitle As String) As Integer
        Return api.DTWAIN_SetPDFTitleW(Source, lpTitle)
        End Function
        
        Public Function DTWAIN_SetPaperSize(Source As System.IntPtr, PaperSize As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetPaperSize(Source, PaperSize, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetPatchMaxPriorities(Source As System.IntPtr, nMaxSearchRetries As Integer) As Integer
        Return api.DTWAIN_SetPatchMaxPriorities(Source, nMaxSearchRetries)
        End Function
        
        Public Function DTWAIN_SetPatchMaxRetries(Source As System.IntPtr, nMaxRetries As Integer) As Integer
        Return api.DTWAIN_SetPatchMaxRetries(Source, nMaxRetries)
        End Function
        
        Public Function DTWAIN_SetPatchPriorities(Source As System.IntPtr, SearchPriorities As System.IntPtr) As Integer
        Return api.DTWAIN_SetPatchPriorities(Source, SearchPriorities)
        End Function
        
        Public Function DTWAIN_SetPatchSearchMode(Source As System.IntPtr, nSearchMode As Integer) As Integer
        Return api.DTWAIN_SetPatchSearchMode(Source, nSearchMode)
        End Function
        
        Public Function DTWAIN_SetPatchTimeOut(Source As System.IntPtr, TimeOutValue As Integer) As Integer
        Return api.DTWAIN_SetPatchTimeOut(Source, TimeOutValue)
        End Function
        
        Public Function DTWAIN_SetPixelFlavor(Source As System.IntPtr, PixelFlavor As Integer) As Integer
        Return api.DTWAIN_SetPixelFlavor(Source, PixelFlavor)
        End Function
        
        Public Function DTWAIN_SetPixelType(Source As System.IntPtr, PixelType As Integer, BitDepth As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetPixelType(Source, PixelType, BitDepth, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetPostScriptTitle(Source As System.IntPtr, szTitle As String) As Integer
        Return api.DTWAIN_SetPostScriptTitle(Source, szTitle)
        End Function
        
        Public Function DTWAIN_SetPostScriptTitleA(Source As System.IntPtr, szTitle As String) As Integer
        Return api.DTWAIN_SetPostScriptTitleA(Source, szTitle)
        End Function
        
        Public Function DTWAIN_SetPostScriptTitleW(Source As System.IntPtr, szTitle As String) As Integer
        Return api.DTWAIN_SetPostScriptTitleW(Source, szTitle)
        End Function
        
        Public Function DTWAIN_SetPostScriptType(Source As System.IntPtr, PSType As Integer) As Integer
        Return api.DTWAIN_SetPostScriptType(Source, PSType)
        End Function
        
        Public Function DTWAIN_SetPrinter(Source As System.IntPtr, Printer As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_SetPrinter(Source, Printer, bCurrent)
        End Function
        
        Public Function DTWAIN_SetPrinterEx(Source As System.IntPtr, Printer As Integer, bCurrent As Integer) As Integer
        Return api.DTWAIN_SetPrinterEx(Source, Printer, bCurrent)
        End Function
        
        Public Function DTWAIN_SetPrinterStartNumber(Source As System.IntPtr, nStart As Integer) As Integer
        Return api.DTWAIN_SetPrinterStartNumber(Source, nStart)
        End Function
        
        Public Function DTWAIN_SetPrinterStringMode(Source As System.IntPtr, PrinterMode As Integer, bSetCurrent As Integer) As Integer
        Return api.DTWAIN_SetPrinterStringMode(Source, PrinterMode, bSetCurrent)
        End Function
        
        Public Function DTWAIN_SetPrinterStrings(Source As System.IntPtr, ArrayString As System.IntPtr, ByRef pNumStrings As Integer) As Integer
        Return api.DTWAIN_SetPrinterStrings(Source, ArrayString, pNumStrings)
        End Function
        
        Public Function DTWAIN_SetPrinterSuffixString(Source As System.IntPtr, Suffix As String) As Integer
        Return api.DTWAIN_SetPrinterSuffixString(Source, Suffix)
        End Function
        
        Public Function DTWAIN_SetPrinterSuffixStringA(Source As System.IntPtr, Suffix As String) As Integer
        Return api.DTWAIN_SetPrinterSuffixStringA(Source, Suffix)
        End Function
        
        Public Function DTWAIN_SetPrinterSuffixStringW(Source As System.IntPtr, Suffix As String) As Integer
        Return api.DTWAIN_SetPrinterSuffixStringW(Source, Suffix)
        End Function
        
        Public Function DTWAIN_SetQueryCapSupport(bSet As Integer) As Integer
        Return api.DTWAIN_SetQueryCapSupport(bSet)
        End Function
        
        Public Function DTWAIN_SetResolution(Source As System.IntPtr, Resolution As System.Double) As Integer
        Return api.DTWAIN_SetResolution(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetResolutionString(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetResolutionString(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetResolutionStringA(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetResolutionStringA(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetResolutionStringW(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetResolutionStringW(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetResourcePath(ResourcePath As String) As Integer
        Return api.DTWAIN_SetResourcePath(ResourcePath)
        End Function
        
        Public Function DTWAIN_SetResourcePathA(ResourcePath As String) As Integer
        Return api.DTWAIN_SetResourcePathA(ResourcePath)
        End Function
        
        Public Function DTWAIN_SetResourcePathW(ResourcePath As String) As Integer
        Return api.DTWAIN_SetResourcePathW(ResourcePath)
        End Function
        
        Public Function DTWAIN_SetRotation(Source As System.IntPtr, Rotation As System.Double) As Integer
        Return api.DTWAIN_SetRotation(Source, Rotation)
        End Function
        
        Public Function DTWAIN_SetRotationString(Source As System.IntPtr, Rotation As String) As Integer
        Return api.DTWAIN_SetRotationString(Source, Rotation)
        End Function
        
        Public Function DTWAIN_SetRotationStringA(Source As System.IntPtr, Rotation As String) As Integer
        Return api.DTWAIN_SetRotationStringA(Source, Rotation)
        End Function
        
        Public Function DTWAIN_SetRotationStringW(Source As System.IntPtr, Rotation As String) As Integer
        Return api.DTWAIN_SetRotationStringW(Source, Rotation)
        End Function
        
        Public Function DTWAIN_SetSaveFileName(Source As System.IntPtr, fName As String) As Integer
        Return api.DTWAIN_SetSaveFileName(Source, fName)
        End Function
        
        Public Function DTWAIN_SetSaveFileNameA(Source As System.IntPtr, fName As String) As Integer
        Return api.DTWAIN_SetSaveFileNameA(Source, fName)
        End Function
        
        Public Function DTWAIN_SetSaveFileNameW(Source As System.IntPtr, fName As String) As Integer
        Return api.DTWAIN_SetSaveFileNameW(Source, fName)
        End Function
        
        Public Function DTWAIN_SetShadow(Source As System.IntPtr, Shadow As System.Double) As Integer
        Return api.DTWAIN_SetShadow(Source, Shadow)
        End Function
        
        Public Function DTWAIN_SetShadowString(Source As System.IntPtr, Shadow As String) As Integer
        Return api.DTWAIN_SetShadowString(Source, Shadow)
        End Function
        
        Public Function DTWAIN_SetShadowStringA(Source As System.IntPtr, Shadow As String) As Integer
        Return api.DTWAIN_SetShadowStringA(Source, Shadow)
        End Function
        
        Public Function DTWAIN_SetShadowStringW(Source As System.IntPtr, Shadow As String) As Integer
        Return api.DTWAIN_SetShadowStringW(Source, Shadow)
        End Function
        
        Public Function DTWAIN_SetSourceUnit(Source As System.IntPtr, Unit As Integer) As Integer
        Return api.DTWAIN_SetSourceUnit(Source, Unit)
        End Function
        
        Public Function DTWAIN_SetTIFFCompressType(Source As System.IntPtr, Setting As Integer) As Integer
        Return api.DTWAIN_SetTIFFCompressType(Source, Setting)
        End Function
        
        Public Function DTWAIN_SetTIFFInvert(Source As System.IntPtr, Setting As Integer) As Integer
        Return api.DTWAIN_SetTIFFInvert(Source, Setting)
        End Function
        
        Public Function DTWAIN_SetTempFileDirectory(szFilePath As String) As Integer
        Return api.DTWAIN_SetTempFileDirectory(szFilePath)
        End Function
        
        Public Function DTWAIN_SetTempFileDirectoryA(szFilePath As String) As Integer
        Return api.DTWAIN_SetTempFileDirectoryA(szFilePath)
        End Function
        
        Public Function DTWAIN_SetTempFileDirectoryEx(szFilePath As String, CreationFlags As Integer) As Integer
        Return api.DTWAIN_SetTempFileDirectoryEx(szFilePath, CreationFlags)
        End Function
        
        Public Function DTWAIN_SetTempFileDirectoryExA(szFilePath As String, CreationFlags As Integer) As Integer
        Return api.DTWAIN_SetTempFileDirectoryExA(szFilePath, CreationFlags)
        End Function
        
        Public Function DTWAIN_SetTempFileDirectoryExW(szFilePath As String, CreationFlags As Integer) As Integer
        Return api.DTWAIN_SetTempFileDirectoryExW(szFilePath, CreationFlags)
        End Function
        
        Public Function DTWAIN_SetTempFileDirectoryW(szFilePath As String) As Integer
        Return api.DTWAIN_SetTempFileDirectoryW(szFilePath)
        End Function
        
        Public Function DTWAIN_SetThreshold(Source As System.IntPtr, Threshold As System.Double, bSetBithDepthReduction As Integer) As Integer
        Return api.DTWAIN_SetThreshold(Source, Threshold, bSetBithDepthReduction)
        End Function
        
        Public Function DTWAIN_SetThresholdString(Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
        Return api.DTWAIN_SetThresholdString(Source, Threshold, bSetBitDepthReduction)
        End Function
        
        Public Function DTWAIN_SetThresholdStringA(Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
        Return api.DTWAIN_SetThresholdStringA(Source, Threshold, bSetBitDepthReduction)
        End Function
        
        Public Function DTWAIN_SetThresholdStringW(Source As System.IntPtr, Threshold As String, bSetBitDepthReduction As Integer) As Integer
        Return api.DTWAIN_SetThresholdStringW(Source, Threshold, bSetBitDepthReduction)
        End Function
        
        Public Function DTWAIN_SetTwainDSM(DSMType As Integer) As Integer
        Return api.DTWAIN_SetTwainDSM(DSMType)
        End Function
        
        Public Function DTWAIN_SetTwainLog(LogFlags As UInteger, lpszLogFile As String) As Integer
        Return api.DTWAIN_SetTwainLog(LogFlags, lpszLogFile)
        End Function
        
        Public Function DTWAIN_SetTwainLogA(LogFlags As UInteger, lpszLogFile As String) As Integer
        Return api.DTWAIN_SetTwainLogA(LogFlags, lpszLogFile)
        End Function
        
        Public Function DTWAIN_SetTwainLogW(LogFlags As UInteger, lpszLogFile As String) As Integer
        Return api.DTWAIN_SetTwainLogW(LogFlags, lpszLogFile)
        End Function
        
        Public Function DTWAIN_SetTwainMode(lAcquireMode As Integer) As Integer
        Return api.DTWAIN_SetTwainMode(lAcquireMode)
        End Function
        
        Public Function DTWAIN_SetTwainTimeout(milliseconds As Integer) As Integer
        Return api.DTWAIN_SetTwainTimeout(milliseconds)
        End Function
        
        Public Function DTWAIN_SetUpdateDibProc(DibProc As DTwainDIBUpdateProc) As DTwainDIBUpdateProc
        Return api.DTWAIN_SetUpdateDibProc(DibProc)
        End Function
        
        Public Function DTWAIN_SetXResolution(Source As System.IntPtr, xResolution As System.Double) As Integer
        Return api.DTWAIN_SetXResolution(Source, xResolution)
        End Function
        
        Public Function DTWAIN_SetXResolutionString(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetXResolutionString(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetXResolutionStringA(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetXResolutionStringA(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetXResolutionStringW(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetXResolutionStringW(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetYResolution(Source As System.IntPtr, yResolution As System.Double) As Integer
        Return api.DTWAIN_SetYResolution(Source, yResolution)
        End Function
        
        Public Function DTWAIN_SetYResolutionString(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetYResolutionString(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetYResolutionStringA(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetYResolutionStringA(Source, Resolution)
        End Function
        
        Public Function DTWAIN_SetYResolutionStringW(Source As System.IntPtr, Resolution As String) As Integer
        Return api.DTWAIN_SetYResolutionStringW(Source, Resolution)
        End Function
        
        Public Function DTWAIN_ShowUIOnly(Source As System.IntPtr) As Integer
        Return api.DTWAIN_ShowUIOnly(Source)
        End Function
        
        Public Function DTWAIN_ShutdownOCREngine(OCREngine As System.IntPtr) As Integer
        Return api.DTWAIN_ShutdownOCREngine(OCREngine)
        End Function
        
        Public Function DTWAIN_SkipImageInfoError(Source As System.IntPtr, bSkip As Integer) As Integer
        Return api.DTWAIN_SkipImageInfoError(Source, bSkip)
        End Function
        
        Public Function DTWAIN_StartThread(DLLHandle As System.IntPtr) As Integer
        Return api.DTWAIN_StartThread(DLLHandle)
        End Function
        
        Public Function DTWAIN_StartTwainSession(hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
        Return api.DTWAIN_StartTwainSession(hWndMsg, lpszDLLName)
        End Function
        
        Public Function DTWAIN_StartTwainSessionA(hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
        Return api.DTWAIN_StartTwainSessionA(hWndMsg, lpszDLLName)
        End Function
        
        Public Function DTWAIN_StartTwainSessionW(hWndMsg As System.IntPtr, lpszDLLName As String) As Integer
        Return api.DTWAIN_StartTwainSessionW(hWndMsg, lpszDLLName)
        End Function
        
        Public Function DTWAIN_SysDestroy() As Integer
        Return api.DTWAIN_SysDestroy()
        End Function
        
        Public Function DTWAIN_SysInitialize() As System.IntPtr
        Return api.DTWAIN_SysInitialize()
        End Function
        
        Public Function DTWAIN_SysInitializeEx(szINIPath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeEx(szINIPath)
        End Function
        
        Public Function DTWAIN_SysInitializeEx2(szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeEx2(szINIPath, szImageDLLPath, szLangResourcePath)
        End Function
        
        Public Function DTWAIN_SysInitializeEx2A(szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeEx2A(szINIPath, szImageDLLPath, szLangResourcePath)
        End Function
        
        Public Function DTWAIN_SysInitializeEx2W(szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeEx2W(szINIPath, szImageDLLPath, szLangResourcePath)
        End Function
        
        Public Function DTWAIN_SysInitializeExA(szINIPath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeExA(szINIPath)
        End Function
        
        Public Function DTWAIN_SysInitializeExW(szINIPath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeExW(szINIPath)
        End Function
        
        Public Function DTWAIN_SysInitializeLib(hInstance As System.IntPtr) As System.IntPtr
        Return api.DTWAIN_SysInitializeLib(hInstance)
        End Function
        
        Public Function DTWAIN_SysInitializeLibEx(hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeLibEx(hInstance, szINIPath)
        End Function
        
        Public Function DTWAIN_SysInitializeLibEx2(hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeLibEx2(hInstance, szINIPath, szImageDLLPath, szLangResourcePath)
        End Function
        
        Public Function DTWAIN_SysInitializeLibEx2A(hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeLibEx2A(hInstance, szINIPath, szImageDLLPath, szLangResourcePath)
        End Function
        
        Public Function DTWAIN_SysInitializeLibEx2W(hInstance As System.IntPtr, szINIPath As String, szImageDLLPath As String, szLangResourcePath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeLibEx2W(hInstance, szINIPath, szImageDLLPath, szLangResourcePath)
        End Function
        
        Public Function DTWAIN_SysInitializeLibExA(hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeLibExA(hInstance, szINIPath)
        End Function
        
        Public Function DTWAIN_SysInitializeLibExW(hInstance As System.IntPtr, szINIPath As String) As System.IntPtr
        Return api.DTWAIN_SysInitializeLibExW(hInstance, szINIPath)
        End Function
        
        Public Function DTWAIN_SysInitializeNoBlocking() As System.IntPtr
        Return api.DTWAIN_SysInitializeNoBlocking()
        End Function
        
        Public Function DTWAIN_TestGetCap(Source As System.IntPtr, lCapability As Integer) As System.IntPtr
        Return api.DTWAIN_TestGetCap(Source, lCapability)
        End Function
        
        Public Function DTWAIN_UnlockMemory(h As System.IntPtr) As Integer
        Return api.DTWAIN_UnlockMemory(h)
        End Function
        
        Public Function DTWAIN_UnlockMemoryEx(h As System.IntPtr) As Integer
        Return api.DTWAIN_UnlockMemoryEx(h)
        End Function
        
        Public Function DTWAIN_UseMultipleThreads(bSet As Integer) As Integer
        Return api.DTWAIN_UseMultipleThreads(bSet)
        End Function
        Private Class AllBindings
            Public DTWAIN_AcquireAudioFile As DTWAIN_AcquireAudioFileDelegate
            Public DTWAIN_AcquireAudioFileA As DTWAIN_AcquireAudioFileADelegate
            Public DTWAIN_AcquireAudioFileW As DTWAIN_AcquireAudioFileWDelegate
            Public DTWAIN_AcquireAudioNative As DTWAIN_AcquireAudioNativeDelegate
            Public DTWAIN_AcquireAudioNativeEx As DTWAIN_AcquireAudioNativeExDelegate
            Public DTWAIN_AcquireBuffered As DTWAIN_AcquireBufferedDelegate
            Public DTWAIN_AcquireBufferedEx As DTWAIN_AcquireBufferedExDelegate
            Public DTWAIN_AcquireFile As DTWAIN_AcquireFileDelegate
            Public DTWAIN_AcquireFileA As DTWAIN_AcquireFileADelegate
            Public DTWAIN_AcquireFileEx As DTWAIN_AcquireFileExDelegate
            Public DTWAIN_AcquireFileW As DTWAIN_AcquireFileWDelegate
            Public DTWAIN_AcquireNative As DTWAIN_AcquireNativeDelegate
            Public DTWAIN_AcquireNativeEx As DTWAIN_AcquireNativeExDelegate
            Public DTWAIN_AcquireToClipboard As DTWAIN_AcquireToClipboardDelegate
            Public DTWAIN_AddExtImageInfoQuery As DTWAIN_AddExtImageInfoQueryDelegate
            Public DTWAIN_AddFileToAppend As DTWAIN_AddFileToAppendDelegate
            Public DTWAIN_AddFileToAppendA As DTWAIN_AddFileToAppendADelegate
            Public DTWAIN_AddFileToAppendW As DTWAIN_AddFileToAppendWDelegate
            Public DTWAIN_AddPDFText As DTWAIN_AddPDFTextDelegate
            Public DTWAIN_AddPDFTextA As DTWAIN_AddPDFTextADelegate
            Public DTWAIN_AddPDFTextEx As DTWAIN_AddPDFTextExDelegate
            Public DTWAIN_AddPDFTextW As DTWAIN_AddPDFTextWDelegate
            Public DTWAIN_AllocateMemory As DTWAIN_AllocateMemoryDelegate
            Public DTWAIN_AllocateMemory64 As DTWAIN_AllocateMemory64Delegate
            Public DTWAIN_AllocateMemoryEx As DTWAIN_AllocateMemoryExDelegate
            Public DTWAIN_AppHandlesExceptions As DTWAIN_AppHandlesExceptionsDelegate
            Public DTWAIN_ArrayANSIStringToFloat As DTWAIN_ArrayANSIStringToFloatDelegate
            Public DTWAIN_ArrayAdd As DTWAIN_ArrayAddDelegate
            Public DTWAIN_ArrayAddANSIString As DTWAIN_ArrayAddANSIStringDelegate
            Public DTWAIN_ArrayAddANSIStringN As DTWAIN_ArrayAddANSIStringNDelegate
            Public DTWAIN_ArrayAddFloat As DTWAIN_ArrayAddFloatDelegate
            Public DTWAIN_ArrayAddFloatN As DTWAIN_ArrayAddFloatNDelegate
            Public DTWAIN_ArrayAddFloatString As DTWAIN_ArrayAddFloatStringDelegate
            Public DTWAIN_ArrayAddFloatStringA As DTWAIN_ArrayAddFloatStringADelegate
            Public DTWAIN_ArrayAddFloatStringN As DTWAIN_ArrayAddFloatStringNDelegate
            Public DTWAIN_ArrayAddFloatStringNA As DTWAIN_ArrayAddFloatStringNADelegate
            Public DTWAIN_ArrayAddFloatStringNW As DTWAIN_ArrayAddFloatStringNWDelegate
            Public DTWAIN_ArrayAddFloatStringW As DTWAIN_ArrayAddFloatStringWDelegate
            Public DTWAIN_ArrayAddFrame As DTWAIN_ArrayAddFrameDelegate
            Public DTWAIN_ArrayAddFrameN As DTWAIN_ArrayAddFrameNDelegate
            Public DTWAIN_ArrayAddLong As DTWAIN_ArrayAddLongDelegate
            Public DTWAIN_ArrayAddLong64 As DTWAIN_ArrayAddLong64Delegate
            Public DTWAIN_ArrayAddLong64N As DTWAIN_ArrayAddLong64NDelegate
            Public DTWAIN_ArrayAddLongN As DTWAIN_ArrayAddLongNDelegate
            Public DTWAIN_ArrayAddN As DTWAIN_ArrayAddNDelegate
            Public DTWAIN_ArrayAddString As DTWAIN_ArrayAddStringDelegate
            Public DTWAIN_ArrayAddStringA As DTWAIN_ArrayAddStringADelegate
            Public DTWAIN_ArrayAddStringN As DTWAIN_ArrayAddStringNDelegate
            Public DTWAIN_ArrayAddStringNA As DTWAIN_ArrayAddStringNADelegate
            Public DTWAIN_ArrayAddStringNW As DTWAIN_ArrayAddStringNWDelegate
            Public DTWAIN_ArrayAddStringW As DTWAIN_ArrayAddStringWDelegate
            Public DTWAIN_ArrayAddWideString As DTWAIN_ArrayAddWideStringDelegate
            Public DTWAIN_ArrayAddWideStringN As DTWAIN_ArrayAddWideStringNDelegate
            Public DTWAIN_ArrayConvertFix32ToFloat As DTWAIN_ArrayConvertFix32ToFloatDelegate
            Public DTWAIN_ArrayConvertFloatToFix32 As DTWAIN_ArrayConvertFloatToFix32Delegate
            Public DTWAIN_ArrayCopy As DTWAIN_ArrayCopyDelegate
            Public DTWAIN_ArrayCreate As DTWAIN_ArrayCreateDelegate
            Public DTWAIN_ArrayCreateCopy As DTWAIN_ArrayCreateCopyDelegate
            Public DTWAIN_ArrayCreateFromCap As DTWAIN_ArrayCreateFromCapDelegate
            Public DTWAIN_ArrayCreateFromLong64s As DTWAIN_ArrayCreateFromLong64sDelegate
            Public DTWAIN_ArrayCreateFromLongs As DTWAIN_ArrayCreateFromLongsDelegate
            Public DTWAIN_ArrayCreateFromReals As DTWAIN_ArrayCreateFromRealsDelegate
            Public DTWAIN_ArrayDestroy As DTWAIN_ArrayDestroyDelegate
            Public DTWAIN_ArrayDestroyFrames As DTWAIN_ArrayDestroyFramesDelegate
            Public DTWAIN_ArrayFind As DTWAIN_ArrayFindDelegate
            Public DTWAIN_ArrayFindANSIString As DTWAIN_ArrayFindANSIStringDelegate
            Public DTWAIN_ArrayFindFloat As DTWAIN_ArrayFindFloatDelegate
            Public DTWAIN_ArrayFindFloatString As DTWAIN_ArrayFindFloatStringDelegate
            Public DTWAIN_ArrayFindFloatStringA As DTWAIN_ArrayFindFloatStringADelegate
            Public DTWAIN_ArrayFindFloatStringW As DTWAIN_ArrayFindFloatStringWDelegate
            Public DTWAIN_ArrayFindLong As DTWAIN_ArrayFindLongDelegate
            Public DTWAIN_ArrayFindLong64 As DTWAIN_ArrayFindLong64Delegate
            Public DTWAIN_ArrayFindString As DTWAIN_ArrayFindStringDelegate
            Public DTWAIN_ArrayFindStringA As DTWAIN_ArrayFindStringADelegate
            Public DTWAIN_ArrayFindStringW As DTWAIN_ArrayFindStringWDelegate
            Public DTWAIN_ArrayFindWideString As DTWAIN_ArrayFindWideStringDelegate
            Public DTWAIN_ArrayFix32GetAt As DTWAIN_ArrayFix32GetAtDelegate
            Public DTWAIN_ArrayFix32SetAt As DTWAIN_ArrayFix32SetAtDelegate
            Public DTWAIN_ArrayFloatToANSIString As DTWAIN_ArrayFloatToANSIStringDelegate
            Public DTWAIN_ArrayFloatToString As DTWAIN_ArrayFloatToStringDelegate
            Public DTWAIN_ArrayFloatToWideString As DTWAIN_ArrayFloatToWideStringDelegate
            Public DTWAIN_ArrayGetAt As DTWAIN_ArrayGetAtDelegate
            Public DTWAIN_ArrayGetAtANSIString As DTWAIN_ArrayGetAtANSIStringDelegate
            Public DTWAIN_ArrayGetAtFloat As DTWAIN_ArrayGetAtFloatDelegate
            Public DTWAIN_ArrayGetAtFloatString As DTWAIN_ArrayGetAtFloatStringDelegate
            Public DTWAIN_ArrayGetAtFloatStringA As DTWAIN_ArrayGetAtFloatStringADelegate
            Public DTWAIN_ArrayGetAtFloatStringW As DTWAIN_ArrayGetAtFloatStringWDelegate
            Public DTWAIN_ArrayGetAtFrame As DTWAIN_ArrayGetAtFrameDelegate
            Public DTWAIN_ArrayGetAtFrameEx As DTWAIN_ArrayGetAtFrameExDelegate
            Public DTWAIN_ArrayGetAtFrameString As DTWAIN_ArrayGetAtFrameStringDelegate
            Public DTWAIN_ArrayGetAtFrameStringA As DTWAIN_ArrayGetAtFrameStringADelegate
            Public DTWAIN_ArrayGetAtFrameStringW As DTWAIN_ArrayGetAtFrameStringWDelegate
            Public DTWAIN_ArrayGetAtLong As DTWAIN_ArrayGetAtLongDelegate
            Public DTWAIN_ArrayGetAtLong64 As DTWAIN_ArrayGetAtLong64Delegate
            Public DTWAIN_ArrayGetAtSource As DTWAIN_ArrayGetAtSourceDelegate
            Public DTWAIN_ArrayGetAtString As DTWAIN_ArrayGetAtStringDelegate
            Public DTWAIN_ArrayGetAtStringA As DTWAIN_ArrayGetAtStringADelegate
            Public DTWAIN_ArrayGetAtStringW As DTWAIN_ArrayGetAtStringWDelegate
            Public DTWAIN_ArrayGetAtWideString As DTWAIN_ArrayGetAtWideStringDelegate
            Public DTWAIN_ArrayGetBuffer As DTWAIN_ArrayGetBufferDelegate
            Public DTWAIN_ArrayGetCapValues As DTWAIN_ArrayGetCapValuesDelegate
            Public DTWAIN_ArrayGetCapValuesEx As DTWAIN_ArrayGetCapValuesExDelegate
            Public DTWAIN_ArrayGetCapValuesEx2 As DTWAIN_ArrayGetCapValuesEx2Delegate
            Public DTWAIN_ArrayGetCount As DTWAIN_ArrayGetCountDelegate
            Public DTWAIN_ArrayGetMaxStringLength As DTWAIN_ArrayGetMaxStringLengthDelegate
            Public DTWAIN_ArrayGetSourceAt As DTWAIN_ArrayGetSourceAtDelegate
            Public DTWAIN_ArrayGetStringLength As DTWAIN_ArrayGetStringLengthDelegate
            Public DTWAIN_ArrayGetType As DTWAIN_ArrayGetTypeDelegate
            Public DTWAIN_ArrayInit As DTWAIN_ArrayInitDelegate
            Public DTWAIN_ArrayInsertAt As DTWAIN_ArrayInsertAtDelegate
            Public DTWAIN_ArrayInsertAtANSIString As DTWAIN_ArrayInsertAtANSIStringDelegate
            Public DTWAIN_ArrayInsertAtANSIStringN As DTWAIN_ArrayInsertAtANSIStringNDelegate
            Public DTWAIN_ArrayInsertAtFloat As DTWAIN_ArrayInsertAtFloatDelegate
            Public DTWAIN_ArrayInsertAtFloatN As DTWAIN_ArrayInsertAtFloatNDelegate
            Public DTWAIN_ArrayInsertAtFloatString As DTWAIN_ArrayInsertAtFloatStringDelegate
            Public DTWAIN_ArrayInsertAtFloatStringA As DTWAIN_ArrayInsertAtFloatStringADelegate
            Public DTWAIN_ArrayInsertAtFloatStringN As DTWAIN_ArrayInsertAtFloatStringNDelegate
            Public DTWAIN_ArrayInsertAtFloatStringNA As DTWAIN_ArrayInsertAtFloatStringNADelegate
            Public DTWAIN_ArrayInsertAtFloatStringNW As DTWAIN_ArrayInsertAtFloatStringNWDelegate
            Public DTWAIN_ArrayInsertAtFloatStringW As DTWAIN_ArrayInsertAtFloatStringWDelegate
            Public DTWAIN_ArrayInsertAtFrame As DTWAIN_ArrayInsertAtFrameDelegate
            Public DTWAIN_ArrayInsertAtFrameN As DTWAIN_ArrayInsertAtFrameNDelegate
            Public DTWAIN_ArrayInsertAtLong As DTWAIN_ArrayInsertAtLongDelegate
            Public DTWAIN_ArrayInsertAtLong64 As DTWAIN_ArrayInsertAtLong64Delegate
            Public DTWAIN_ArrayInsertAtLong64N As DTWAIN_ArrayInsertAtLong64NDelegate
            Public DTWAIN_ArrayInsertAtLongN As DTWAIN_ArrayInsertAtLongNDelegate
            Public DTWAIN_ArrayInsertAtN As DTWAIN_ArrayInsertAtNDelegate
            Public DTWAIN_ArrayInsertAtString As DTWAIN_ArrayInsertAtStringDelegate
            Public DTWAIN_ArrayInsertAtStringA As DTWAIN_ArrayInsertAtStringADelegate
            Public DTWAIN_ArrayInsertAtStringN As DTWAIN_ArrayInsertAtStringNDelegate
            Public DTWAIN_ArrayInsertAtStringNA As DTWAIN_ArrayInsertAtStringNADelegate
            Public DTWAIN_ArrayInsertAtStringNW As DTWAIN_ArrayInsertAtStringNWDelegate
            Public DTWAIN_ArrayInsertAtStringW As DTWAIN_ArrayInsertAtStringWDelegate
            Public DTWAIN_ArrayInsertAtWideString As DTWAIN_ArrayInsertAtWideStringDelegate
            Public DTWAIN_ArrayInsertAtWideStringN As DTWAIN_ArrayInsertAtWideStringNDelegate
            Public DTWAIN_ArrayRemoveAll As DTWAIN_ArrayRemoveAllDelegate
            Public DTWAIN_ArrayRemoveAt As DTWAIN_ArrayRemoveAtDelegate
            Public DTWAIN_ArrayRemoveAtN As DTWAIN_ArrayRemoveAtNDelegate
            Public DTWAIN_ArrayResize As DTWAIN_ArrayResizeDelegate
            Public DTWAIN_ArraySetAt As DTWAIN_ArraySetAtDelegate
            Public DTWAIN_ArraySetAtANSIString As DTWAIN_ArraySetAtANSIStringDelegate
            Public DTWAIN_ArraySetAtFloat As DTWAIN_ArraySetAtFloatDelegate
            Public DTWAIN_ArraySetAtFloatString As DTWAIN_ArraySetAtFloatStringDelegate
            Public DTWAIN_ArraySetAtFloatStringA As DTWAIN_ArraySetAtFloatStringADelegate
            Public DTWAIN_ArraySetAtFloatStringW As DTWAIN_ArraySetAtFloatStringWDelegate
            Public DTWAIN_ArraySetAtFrame As DTWAIN_ArraySetAtFrameDelegate
            Public DTWAIN_ArraySetAtFrameEx As DTWAIN_ArraySetAtFrameExDelegate
            Public DTWAIN_ArraySetAtFrameString As DTWAIN_ArraySetAtFrameStringDelegate
            Public DTWAIN_ArraySetAtFrameStringA As DTWAIN_ArraySetAtFrameStringADelegate
            Public DTWAIN_ArraySetAtFrameStringW As DTWAIN_ArraySetAtFrameStringWDelegate
            Public DTWAIN_ArraySetAtLong As DTWAIN_ArraySetAtLongDelegate
            Public DTWAIN_ArraySetAtLong64 As DTWAIN_ArraySetAtLong64Delegate
            Public DTWAIN_ArraySetAtString As DTWAIN_ArraySetAtStringDelegate
            Public DTWAIN_ArraySetAtStringA As DTWAIN_ArraySetAtStringADelegate
            Public DTWAIN_ArraySetAtStringW As DTWAIN_ArraySetAtStringWDelegate
            Public DTWAIN_ArraySetAtWideString As DTWAIN_ArraySetAtWideStringDelegate
            Public DTWAIN_ArrayStringToFloat As DTWAIN_ArrayStringToFloatDelegate
            Public DTWAIN_ArrayWideStringToFloat As DTWAIN_ArrayWideStringToFloatDelegate
            Public DTWAIN_CallCallback As DTWAIN_CallCallbackDelegate
            Public DTWAIN_CallCallback64 As DTWAIN_CallCallback64Delegate
            Public DTWAIN_CallDSMProc As DTWAIN_CallDSMProcDelegate
            Public DTWAIN_CheckHandles As DTWAIN_CheckHandlesDelegate
            Public DTWAIN_ClearBuffers As DTWAIN_ClearBuffersDelegate
            Public DTWAIN_ClearErrorBuffer As DTWAIN_ClearErrorBufferDelegate
            Public DTWAIN_ClearPDFText As DTWAIN_ClearPDFTextDelegate
            Public DTWAIN_ClearPage As DTWAIN_ClearPageDelegate
            Public DTWAIN_CloseSource As DTWAIN_CloseSourceDelegate
            Public DTWAIN_CloseSourceUI As DTWAIN_CloseSourceUIDelegate
            Public DTWAIN_ConvertDIBToBitmap As DTWAIN_ConvertDIBToBitmapDelegate
            Public DTWAIN_ConvertDIBToFullBitmap As DTWAIN_ConvertDIBToFullBitmapDelegate
            Public DTWAIN_ConvertToAPIString As DTWAIN_ConvertToAPIStringDelegate
            Public DTWAIN_ConvertToAPIStringA As DTWAIN_ConvertToAPIStringADelegate
            Public DTWAIN_ConvertToAPIStringEx As DTWAIN_ConvertToAPIStringExDelegate
            Public DTWAIN_ConvertToAPIStringExA As DTWAIN_ConvertToAPIStringExADelegate
            Public DTWAIN_ConvertToAPIStringExW As DTWAIN_ConvertToAPIStringExWDelegate
            Public DTWAIN_ConvertToAPIStringW As DTWAIN_ConvertToAPIStringWDelegate
            Public DTWAIN_CreateAcquisitionArray As DTWAIN_CreateAcquisitionArrayDelegate
            Public DTWAIN_CreatePDFTextElement As DTWAIN_CreatePDFTextElementDelegate
            Public DTWAIN_DeleteDIB As DTWAIN_DeleteDIBDelegate
            Public DTWAIN_DestroyAcquisitionArray As DTWAIN_DestroyAcquisitionArrayDelegate
            Public DTWAIN_DestroyPDFTextElement As DTWAIN_DestroyPDFTextElementDelegate
            Public DTWAIN_DisableAppWindow As DTWAIN_DisableAppWindowDelegate
            Public DTWAIN_EnableAutoBorderDetect As DTWAIN_EnableAutoBorderDetectDelegate
            Public DTWAIN_EnableAutoBright As DTWAIN_EnableAutoBrightDelegate
            Public DTWAIN_EnableAutoDeskew As DTWAIN_EnableAutoDeskewDelegate
            Public DTWAIN_EnableAutoFeed As DTWAIN_EnableAutoFeedDelegate
            Public DTWAIN_EnableAutoRotate As DTWAIN_EnableAutoRotateDelegate
            Public DTWAIN_EnableAutoScan As DTWAIN_EnableAutoScanDelegate
            Public DTWAIN_EnableAutomaticSenseMedium As DTWAIN_EnableAutomaticSenseMediumDelegate
            Public DTWAIN_EnableDuplex As DTWAIN_EnableDuplexDelegate
            Public DTWAIN_EnableFeeder As DTWAIN_EnableFeederDelegate
            Public DTWAIN_EnableIndicator As DTWAIN_EnableIndicatorDelegate
            Public DTWAIN_EnableJobFileHandling As DTWAIN_EnableJobFileHandlingDelegate
            Public DTWAIN_EnableLamp As DTWAIN_EnableLampDelegate
            Public DTWAIN_EnableMsgNotify As DTWAIN_EnableMsgNotifyDelegate
            Public DTWAIN_EnablePatchDetect As DTWAIN_EnablePatchDetectDelegate
            Public DTWAIN_EnablePeekMessageLoop As DTWAIN_EnablePeekMessageLoopDelegate
            Public DTWAIN_EnablePrinter As DTWAIN_EnablePrinterDelegate
            Public DTWAIN_EnableThumbnail As DTWAIN_EnableThumbnailDelegate
            Public DTWAIN_EnableTripletsNotify As DTWAIN_EnableTripletsNotifyDelegate
            Public DTWAIN_EndThread As DTWAIN_EndThreadDelegate
            Public DTWAIN_EndTwainSession As DTWAIN_EndTwainSessionDelegate
            Public DTWAIN_EnumAlarmVolumes As DTWAIN_EnumAlarmVolumesDelegate
            Public DTWAIN_EnumAlarmVolumesEx As DTWAIN_EnumAlarmVolumesExDelegate
            Public DTWAIN_EnumAlarms As DTWAIN_EnumAlarmsDelegate
            Public DTWAIN_EnumAlarmsEx As DTWAIN_EnumAlarmsExDelegate
            Public DTWAIN_EnumAudioXferMechs As DTWAIN_EnumAudioXferMechsDelegate
            Public DTWAIN_EnumAudioXferMechsEx As DTWAIN_EnumAudioXferMechsExDelegate
            Public DTWAIN_EnumAutoFeedValues As DTWAIN_EnumAutoFeedValuesDelegate
            Public DTWAIN_EnumAutoFeedValuesEx As DTWAIN_EnumAutoFeedValuesExDelegate
            Public DTWAIN_EnumAutomaticCaptures As DTWAIN_EnumAutomaticCapturesDelegate
            Public DTWAIN_EnumAutomaticCapturesEx As DTWAIN_EnumAutomaticCapturesExDelegate
            Public DTWAIN_EnumAutomaticSenseMedium As DTWAIN_EnumAutomaticSenseMediumDelegate
            Public DTWAIN_EnumAutomaticSenseMediumEx As DTWAIN_EnumAutomaticSenseMediumExDelegate
            Public DTWAIN_EnumBitDepths As DTWAIN_EnumBitDepthsDelegate
            Public DTWAIN_EnumBitDepthsEx As DTWAIN_EnumBitDepthsExDelegate
            Public DTWAIN_EnumBitDepthsEx2 As DTWAIN_EnumBitDepthsEx2Delegate
            Public DTWAIN_EnumBottomCameras As DTWAIN_EnumBottomCamerasDelegate
            Public DTWAIN_EnumBottomCamerasEx As DTWAIN_EnumBottomCamerasExDelegate
            Public DTWAIN_EnumBrightnessValues As DTWAIN_EnumBrightnessValuesDelegate
            Public DTWAIN_EnumBrightnessValuesEx As DTWAIN_EnumBrightnessValuesExDelegate
            Public DTWAIN_EnumCameras As DTWAIN_EnumCamerasDelegate
            Public DTWAIN_EnumCamerasEx As DTWAIN_EnumCamerasExDelegate
            Public DTWAIN_EnumCamerasEx2 As DTWAIN_EnumCamerasEx2Delegate
            Public DTWAIN_EnumCamerasEx3 As DTWAIN_EnumCamerasEx3Delegate
            Public DTWAIN_EnumCompressionTypes As DTWAIN_EnumCompressionTypesDelegate
            Public DTWAIN_EnumCompressionTypesEx As DTWAIN_EnumCompressionTypesExDelegate
            Public DTWAIN_EnumCompressionTypesEx2 As DTWAIN_EnumCompressionTypesEx2Delegate
            Public DTWAIN_EnumContrastValues As DTWAIN_EnumContrastValuesDelegate
            Public DTWAIN_EnumContrastValuesEx As DTWAIN_EnumContrastValuesExDelegate
            Public DTWAIN_EnumCustomCaps As DTWAIN_EnumCustomCapsDelegate
            Public DTWAIN_EnumCustomCapsEx2 As DTWAIN_EnumCustomCapsEx2Delegate
            Public DTWAIN_EnumDoubleFeedDetectLengths As DTWAIN_EnumDoubleFeedDetectLengthsDelegate
            Public DTWAIN_EnumDoubleFeedDetectLengthsEx As DTWAIN_EnumDoubleFeedDetectLengthsExDelegate
            Public DTWAIN_EnumDoubleFeedDetectValues As DTWAIN_EnumDoubleFeedDetectValuesDelegate
            Public DTWAIN_EnumDoubleFeedDetectValuesEx As DTWAIN_EnumDoubleFeedDetectValuesExDelegate
            Public DTWAIN_EnumExtImageInfoTypes As DTWAIN_EnumExtImageInfoTypesDelegate
            Public DTWAIN_EnumExtImageInfoTypesEx As DTWAIN_EnumExtImageInfoTypesExDelegate
            Public DTWAIN_EnumExtendedCaps As DTWAIN_EnumExtendedCapsDelegate
            Public DTWAIN_EnumExtendedCapsEx As DTWAIN_EnumExtendedCapsExDelegate
            Public DTWAIN_EnumExtendedCapsEx2 As DTWAIN_EnumExtendedCapsEx2Delegate
            Public DTWAIN_EnumFileTypeBitsPerPixel As DTWAIN_EnumFileTypeBitsPerPixelDelegate
            Public DTWAIN_EnumFileXferFormats As DTWAIN_EnumFileXferFormatsDelegate
            Public DTWAIN_EnumFileXferFormatsEx As DTWAIN_EnumFileXferFormatsExDelegate
            Public DTWAIN_EnumHalftones As DTWAIN_EnumHalftonesDelegate
            Public DTWAIN_EnumHalftonesEx As DTWAIN_EnumHalftonesExDelegate
            Public DTWAIN_EnumHighlightValues As DTWAIN_EnumHighlightValuesDelegate
            Public DTWAIN_EnumHighlightValuesEx As DTWAIN_EnumHighlightValuesExDelegate
            Public DTWAIN_EnumJobControls As DTWAIN_EnumJobControlsDelegate
            Public DTWAIN_EnumJobControlsEx As DTWAIN_EnumJobControlsExDelegate
            Public DTWAIN_EnumLightPaths As DTWAIN_EnumLightPathsDelegate
            Public DTWAIN_EnumLightPathsEx As DTWAIN_EnumLightPathsExDelegate
            Public DTWAIN_EnumLightSources As DTWAIN_EnumLightSourcesDelegate
            Public DTWAIN_EnumLightSourcesEx As DTWAIN_EnumLightSourcesExDelegate
            Public DTWAIN_EnumMaxBuffers As DTWAIN_EnumMaxBuffersDelegate
            Public DTWAIN_EnumMaxBuffersEx As DTWAIN_EnumMaxBuffersExDelegate
            Public DTWAIN_EnumNoiseFilters As DTWAIN_EnumNoiseFiltersDelegate
            Public DTWAIN_EnumNoiseFiltersEx As DTWAIN_EnumNoiseFiltersExDelegate
            Public DTWAIN_EnumOCRInterfaces As DTWAIN_EnumOCRInterfacesDelegate
            Public DTWAIN_EnumOCRSupportedCaps As DTWAIN_EnumOCRSupportedCapsDelegate
            Public DTWAIN_EnumOrientations As DTWAIN_EnumOrientationsDelegate
            Public DTWAIN_EnumOrientationsEx As DTWAIN_EnumOrientationsExDelegate
            Public DTWAIN_EnumOverscanValues As DTWAIN_EnumOverscanValuesDelegate
            Public DTWAIN_EnumOverscanValuesEx As DTWAIN_EnumOverscanValuesExDelegate
            Public DTWAIN_EnumPaperSizes As DTWAIN_EnumPaperSizesDelegate
            Public DTWAIN_EnumPaperSizesEx As DTWAIN_EnumPaperSizesExDelegate
            Public DTWAIN_EnumPatchCodes As DTWAIN_EnumPatchCodesDelegate
            Public DTWAIN_EnumPatchCodesEx As DTWAIN_EnumPatchCodesExDelegate
            Public DTWAIN_EnumPatchMaxPriorities As DTWAIN_EnumPatchMaxPrioritiesDelegate
            Public DTWAIN_EnumPatchMaxPrioritiesEx As DTWAIN_EnumPatchMaxPrioritiesExDelegate
            Public DTWAIN_EnumPatchMaxRetries As DTWAIN_EnumPatchMaxRetriesDelegate
            Public DTWAIN_EnumPatchMaxRetriesEx As DTWAIN_EnumPatchMaxRetriesExDelegate
            Public DTWAIN_EnumPatchPriorities As DTWAIN_EnumPatchPrioritiesDelegate
            Public DTWAIN_EnumPatchPrioritiesEx As DTWAIN_EnumPatchPrioritiesExDelegate
            Public DTWAIN_EnumPatchSearchModes As DTWAIN_EnumPatchSearchModesDelegate
            Public DTWAIN_EnumPatchSearchModesEx As DTWAIN_EnumPatchSearchModesExDelegate
            Public DTWAIN_EnumPatchTimeOutValues As DTWAIN_EnumPatchTimeOutValuesDelegate
            Public DTWAIN_EnumPatchTimeOutValuesEx As DTWAIN_EnumPatchTimeOutValuesExDelegate
            Public DTWAIN_EnumPixelTypes As DTWAIN_EnumPixelTypesDelegate
            Public DTWAIN_EnumPixelTypesEx As DTWAIN_EnumPixelTypesExDelegate
            Public DTWAIN_EnumPrinterStringModes As DTWAIN_EnumPrinterStringModesDelegate
            Public DTWAIN_EnumPrinterStringModesEx As DTWAIN_EnumPrinterStringModesExDelegate
            Public DTWAIN_EnumResolutionValues As DTWAIN_EnumResolutionValuesDelegate
            Public DTWAIN_EnumResolutionValuesEx As DTWAIN_EnumResolutionValuesExDelegate
            Public DTWAIN_EnumShadowValues As DTWAIN_EnumShadowValuesDelegate
            Public DTWAIN_EnumShadowValuesEx As DTWAIN_EnumShadowValuesExDelegate
            Public DTWAIN_EnumSourceUnits As DTWAIN_EnumSourceUnitsDelegate
            Public DTWAIN_EnumSourceUnitsEx As DTWAIN_EnumSourceUnitsExDelegate
            Public DTWAIN_EnumSourceValues As DTWAIN_EnumSourceValuesDelegate
            Public DTWAIN_EnumSourceValuesA As DTWAIN_EnumSourceValuesADelegate
            Public DTWAIN_EnumSourceValuesW As DTWAIN_EnumSourceValuesWDelegate
            Public DTWAIN_EnumSources As DTWAIN_EnumSourcesDelegate
            Public DTWAIN_EnumSourcesEx As DTWAIN_EnumSourcesExDelegate
            Public DTWAIN_EnumSupportedCaps As DTWAIN_EnumSupportedCapsDelegate
            Public DTWAIN_EnumSupportedCapsEx As DTWAIN_EnumSupportedCapsExDelegate
            Public DTWAIN_EnumSupportedCapsEx2 As DTWAIN_EnumSupportedCapsEx2Delegate
            Public DTWAIN_EnumSupportedExtImageInfo As DTWAIN_EnumSupportedExtImageInfoDelegate
            Public DTWAIN_EnumSupportedExtImageInfoEx As DTWAIN_EnumSupportedExtImageInfoExDelegate
            Public DTWAIN_EnumSupportedFileTypes As DTWAIN_EnumSupportedFileTypesDelegate
            Public DTWAIN_EnumSupportedMultiPageFileTypes As DTWAIN_EnumSupportedMultiPageFileTypesDelegate
            Public DTWAIN_EnumSupportedSinglePageFileTypes As DTWAIN_EnumSupportedSinglePageFileTypesDelegate
            Public DTWAIN_EnumThresholdValues As DTWAIN_EnumThresholdValuesDelegate
            Public DTWAIN_EnumThresholdValuesEx As DTWAIN_EnumThresholdValuesExDelegate
            Public DTWAIN_EnumTopCameras As DTWAIN_EnumTopCamerasDelegate
            Public DTWAIN_EnumTopCamerasEx As DTWAIN_EnumTopCamerasExDelegate
            Public DTWAIN_EnumTwainPrinters As DTWAIN_EnumTwainPrintersDelegate
            Public DTWAIN_EnumTwainPrintersArray As DTWAIN_EnumTwainPrintersArrayDelegate
            Public DTWAIN_EnumTwainPrintersArrayEx As DTWAIN_EnumTwainPrintersArrayExDelegate
            Public DTWAIN_EnumTwainPrintersEx As DTWAIN_EnumTwainPrintersExDelegate
            Public DTWAIN_EnumXResolutionValues As DTWAIN_EnumXResolutionValuesDelegate
            Public DTWAIN_EnumXResolutionValuesEx As DTWAIN_EnumXResolutionValuesExDelegate
            Public DTWAIN_EnumYResolutionValues As DTWAIN_EnumYResolutionValuesDelegate
            Public DTWAIN_EnumYResolutionValuesEx As DTWAIN_EnumYResolutionValuesExDelegate
            Public DTWAIN_ExecuteOCR As DTWAIN_ExecuteOCRDelegate
            Public DTWAIN_ExecuteOCRA As DTWAIN_ExecuteOCRADelegate
            Public DTWAIN_ExecuteOCRW As DTWAIN_ExecuteOCRWDelegate
            Public DTWAIN_FeedPage As DTWAIN_FeedPageDelegate
            Public DTWAIN_FlipBitmap As DTWAIN_FlipBitmapDelegate
            Public DTWAIN_FlushAcquiredPages As DTWAIN_FlushAcquiredPagesDelegate
            Public DTWAIN_ForceAcquireBitDepth As DTWAIN_ForceAcquireBitDepthDelegate
            Public DTWAIN_ForceScanOnNoUI As DTWAIN_ForceScanOnNoUIDelegate
            Public DTWAIN_FrameCreate As DTWAIN_FrameCreateDelegate
            Public DTWAIN_FrameCreateString As DTWAIN_FrameCreateStringDelegate
            Public DTWAIN_FrameCreateStringA As DTWAIN_FrameCreateStringADelegate
            Public DTWAIN_FrameCreateStringW As DTWAIN_FrameCreateStringWDelegate
            Public DTWAIN_FrameDestroy As DTWAIN_FrameDestroyDelegate
            Public DTWAIN_FrameGetAll As DTWAIN_FrameGetAllDelegate
            Public DTWAIN_FrameGetAllString As DTWAIN_FrameGetAllStringDelegate
            Public DTWAIN_FrameGetAllStringA As DTWAIN_FrameGetAllStringADelegate
            Public DTWAIN_FrameGetAllStringW As DTWAIN_FrameGetAllStringWDelegate
            Public DTWAIN_FrameGetValue As DTWAIN_FrameGetValueDelegate
            Public DTWAIN_FrameGetValueString As DTWAIN_FrameGetValueStringDelegate
            Public DTWAIN_FrameGetValueStringA As DTWAIN_FrameGetValueStringADelegate
            Public DTWAIN_FrameGetValueStringW As DTWAIN_FrameGetValueStringWDelegate
            Public DTWAIN_FrameIsValid As DTWAIN_FrameIsValidDelegate
            Public DTWAIN_FrameSetAll As DTWAIN_FrameSetAllDelegate
            Public DTWAIN_FrameSetAllString As DTWAIN_FrameSetAllStringDelegate
            Public DTWAIN_FrameSetAllStringA As DTWAIN_FrameSetAllStringADelegate
            Public DTWAIN_FrameSetAllStringW As DTWAIN_FrameSetAllStringWDelegate
            Public DTWAIN_FrameSetValue As DTWAIN_FrameSetValueDelegate
            Public DTWAIN_FrameSetValueString As DTWAIN_FrameSetValueStringDelegate
            Public DTWAIN_FrameSetValueStringA As DTWAIN_FrameSetValueStringADelegate
            Public DTWAIN_FrameSetValueStringW As DTWAIN_FrameSetValueStringWDelegate
            Public DTWAIN_FreeExtImageInfo As DTWAIN_FreeExtImageInfoDelegate
            Public DTWAIN_FreeMemory As DTWAIN_FreeMemoryDelegate
            Public DTWAIN_FreeMemoryEx As DTWAIN_FreeMemoryExDelegate
            Public DTWAIN_GetAPIHandleStatus As DTWAIN_GetAPIHandleStatusDelegate
            Public DTWAIN_GetAcquireArea As DTWAIN_GetAcquireAreaDelegate
            Public DTWAIN_GetAcquireArea2 As DTWAIN_GetAcquireArea2Delegate
            Public DTWAIN_GetAcquireArea2String As DTWAIN_GetAcquireArea2StringDelegate
            Public DTWAIN_GetAcquireArea2StringA As DTWAIN_GetAcquireArea2StringADelegate
            Public DTWAIN_GetAcquireArea2StringW As DTWAIN_GetAcquireArea2StringWDelegate
            Public DTWAIN_GetAcquireAreaEx As DTWAIN_GetAcquireAreaExDelegate
            Public DTWAIN_GetAcquireMetrics As DTWAIN_GetAcquireMetricsDelegate
            Public DTWAIN_GetAcquireStripBuffer As DTWAIN_GetAcquireStripBufferDelegate
            Public DTWAIN_GetAcquireStripData As DTWAIN_GetAcquireStripDataDelegate
            Public DTWAIN_GetAcquireStripSizes As DTWAIN_GetAcquireStripSizesDelegate
            Public DTWAIN_GetAcquiredImage As DTWAIN_GetAcquiredImageDelegate
            Public DTWAIN_GetAcquiredImageArray As DTWAIN_GetAcquiredImageArrayDelegate
            Public DTWAIN_GetActiveDSMPath As DTWAIN_GetActiveDSMPathDelegate
            Public DTWAIN_GetActiveDSMPathA As DTWAIN_GetActiveDSMPathADelegate
            Public DTWAIN_GetActiveDSMPathW As DTWAIN_GetActiveDSMPathWDelegate
            Public DTWAIN_GetActiveDSMVersionInfo As DTWAIN_GetActiveDSMVersionInfoDelegate
            Public DTWAIN_GetActiveDSMVersionInfoA As DTWAIN_GetActiveDSMVersionInfoADelegate
            Public DTWAIN_GetActiveDSMVersionInfoW As DTWAIN_GetActiveDSMVersionInfoWDelegate
            Public DTWAIN_GetAlarmVolume As DTWAIN_GetAlarmVolumeDelegate
            Public DTWAIN_GetAllSourceDibs As DTWAIN_GetAllSourceDibsDelegate
            Public DTWAIN_GetAppInfo As DTWAIN_GetAppInfoDelegate
            Public DTWAIN_GetAppInfoA As DTWAIN_GetAppInfoADelegate
            Public DTWAIN_GetAppInfoW As DTWAIN_GetAppInfoWDelegate
            Public DTWAIN_GetAuthor As DTWAIN_GetAuthorDelegate
            Public DTWAIN_GetAuthorA As DTWAIN_GetAuthorADelegate
            Public DTWAIN_GetAuthorW As DTWAIN_GetAuthorWDelegate
            Public DTWAIN_GetBatteryMinutes As DTWAIN_GetBatteryMinutesDelegate
            Public DTWAIN_GetBatteryPercent As DTWAIN_GetBatteryPercentDelegate
            Public DTWAIN_GetBitDepth As DTWAIN_GetBitDepthDelegate
            Public DTWAIN_GetBlankPageAutoDetection As DTWAIN_GetBlankPageAutoDetectionDelegate
            Public DTWAIN_GetBrightness As DTWAIN_GetBrightnessDelegate
            Public DTWAIN_GetBrightnessString As DTWAIN_GetBrightnessStringDelegate
            Public DTWAIN_GetBrightnessStringA As DTWAIN_GetBrightnessStringADelegate
            Public DTWAIN_GetBrightnessStringW As DTWAIN_GetBrightnessStringWDelegate
            Public DTWAIN_GetBufferedTransferInfo As DTWAIN_GetBufferedTransferInfoDelegate
            Public DTWAIN_GetCallback As DTWAIN_GetCallbackDelegate
            Public DTWAIN_GetCallback64 As DTWAIN_GetCallback64Delegate
            Public DTWAIN_GetCapArrayType As DTWAIN_GetCapArrayTypeDelegate
            Public DTWAIN_GetCapContainer As DTWAIN_GetCapContainerDelegate
            Public DTWAIN_GetCapContainerEx As DTWAIN_GetCapContainerExDelegate
            Public DTWAIN_GetCapContainerEx2 As DTWAIN_GetCapContainerEx2Delegate
            Public DTWAIN_GetCapDataType As DTWAIN_GetCapDataTypeDelegate
            Public DTWAIN_GetCapFromName As DTWAIN_GetCapFromNameDelegate
            Public DTWAIN_GetCapFromNameA As DTWAIN_GetCapFromNameADelegate
            Public DTWAIN_GetCapFromNameW As DTWAIN_GetCapFromNameWDelegate
            Public DTWAIN_GetCapOperations As DTWAIN_GetCapOperationsDelegate
            Public DTWAIN_GetCapValues As DTWAIN_GetCapValuesDelegate
            Public DTWAIN_GetCapValuesEx As DTWAIN_GetCapValuesExDelegate
            Public DTWAIN_GetCapValuesEx2 As DTWAIN_GetCapValuesEx2Delegate
            Public DTWAIN_GetCaption As DTWAIN_GetCaptionDelegate
            Public DTWAIN_GetCaptionA As DTWAIN_GetCaptionADelegate
            Public DTWAIN_GetCaptionW As DTWAIN_GetCaptionWDelegate
            Public DTWAIN_GetCompressionSize As DTWAIN_GetCompressionSizeDelegate
            Public DTWAIN_GetCompressionType As DTWAIN_GetCompressionTypeDelegate
            Public DTWAIN_GetConditionCodeString As DTWAIN_GetConditionCodeStringDelegate
            Public DTWAIN_GetConditionCodeStringA As DTWAIN_GetConditionCodeStringADelegate
            Public DTWAIN_GetConditionCodeStringW As DTWAIN_GetConditionCodeStringWDelegate
            Public DTWAIN_GetContrast As DTWAIN_GetContrastDelegate
            Public DTWAIN_GetContrastString As DTWAIN_GetContrastStringDelegate
            Public DTWAIN_GetContrastStringA As DTWAIN_GetContrastStringADelegate
            Public DTWAIN_GetContrastStringW As DTWAIN_GetContrastStringWDelegate
            Public DTWAIN_GetCountry As DTWAIN_GetCountryDelegate
            Public DTWAIN_GetCurrentAcquiredImage As DTWAIN_GetCurrentAcquiredImageDelegate
            Public DTWAIN_GetCurrentFileName As DTWAIN_GetCurrentFileNameDelegate
            Public DTWAIN_GetCurrentFileNameA As DTWAIN_GetCurrentFileNameADelegate
            Public DTWAIN_GetCurrentFileNameW As DTWAIN_GetCurrentFileNameWDelegate
            Public DTWAIN_GetCurrentPageNum As DTWAIN_GetCurrentPageNumDelegate
            Public DTWAIN_GetCurrentRetryCount As DTWAIN_GetCurrentRetryCountDelegate
            Public DTWAIN_GetCurrentTwainTriplet As DTWAIN_GetCurrentTwainTripletDelegate
            Public DTWAIN_GetCustomDSData As DTWAIN_GetCustomDSDataDelegate
            Public DTWAIN_GetDSMFullName As DTWAIN_GetDSMFullNameDelegate
            Public DTWAIN_GetDSMFullNameA As DTWAIN_GetDSMFullNameADelegate
            Public DTWAIN_GetDSMFullNameW As DTWAIN_GetDSMFullNameWDelegate
            Public DTWAIN_GetDSMSearchOrder As DTWAIN_GetDSMSearchOrderDelegate
            Public DTWAIN_GetDTWAINHandle As DTWAIN_GetDTWAINHandleDelegate
            Public DTWAIN_GetDeviceEvent As DTWAIN_GetDeviceEventDelegate
            Public DTWAIN_GetDeviceEventEx As DTWAIN_GetDeviceEventExDelegate
            Public DTWAIN_GetDeviceEventInfo As DTWAIN_GetDeviceEventInfoDelegate
            Public DTWAIN_GetDeviceNotifications As DTWAIN_GetDeviceNotificationsDelegate
            Public DTWAIN_GetDeviceTimeDate As DTWAIN_GetDeviceTimeDateDelegate
            Public DTWAIN_GetDeviceTimeDateA As DTWAIN_GetDeviceTimeDateADelegate
            Public DTWAIN_GetDeviceTimeDateW As DTWAIN_GetDeviceTimeDateWDelegate
            Public DTWAIN_GetDoubleFeedDetectLength As DTWAIN_GetDoubleFeedDetectLengthDelegate
            Public DTWAIN_GetDoubleFeedDetectValues As DTWAIN_GetDoubleFeedDetectValuesDelegate
            Public DTWAIN_GetDuplexType As DTWAIN_GetDuplexTypeDelegate
            Public DTWAIN_GetErrorBuffer As DTWAIN_GetErrorBufferDelegate
            Public DTWAIN_GetErrorBufferThreshold As DTWAIN_GetErrorBufferThresholdDelegate
            Public DTWAIN_GetErrorCallback As DTWAIN_GetErrorCallbackDelegate
            Public DTWAIN_GetErrorCallback64 As DTWAIN_GetErrorCallback64Delegate
            Public DTWAIN_GetErrorString As DTWAIN_GetErrorStringDelegate
            Public DTWAIN_GetErrorStringA As DTWAIN_GetErrorStringADelegate
            Public DTWAIN_GetErrorStringW As DTWAIN_GetErrorStringWDelegate
            Public DTWAIN_GetExtCapFromName As DTWAIN_GetExtCapFromNameDelegate
            Public DTWAIN_GetExtCapFromNameA As DTWAIN_GetExtCapFromNameADelegate
            Public DTWAIN_GetExtCapFromNameW As DTWAIN_GetExtCapFromNameWDelegate
            Public DTWAIN_GetExtImageInfo As DTWAIN_GetExtImageInfoDelegate
            Public DTWAIN_GetExtImageInfoData As DTWAIN_GetExtImageInfoDataDelegate
            Public DTWAIN_GetExtImageInfoDataEx As DTWAIN_GetExtImageInfoDataExDelegate
            Public DTWAIN_GetExtImageInfoItem As DTWAIN_GetExtImageInfoItemDelegate
            Public DTWAIN_GetExtImageInfoItemEx As DTWAIN_GetExtImageInfoItemExDelegate
            Public DTWAIN_GetExtNameFromCap As DTWAIN_GetExtNameFromCapDelegate
            Public DTWAIN_GetExtNameFromCapA As DTWAIN_GetExtNameFromCapADelegate
            Public DTWAIN_GetExtNameFromCapW As DTWAIN_GetExtNameFromCapWDelegate
            Public DTWAIN_GetFeederAlignment As DTWAIN_GetFeederAlignmentDelegate
            Public DTWAIN_GetFeederFuncs As DTWAIN_GetFeederFuncsDelegate
            Public DTWAIN_GetFeederOrder As DTWAIN_GetFeederOrderDelegate
            Public DTWAIN_GetFeederWaitTime As DTWAIN_GetFeederWaitTimeDelegate
            Public DTWAIN_GetFileCompressionType As DTWAIN_GetFileCompressionTypeDelegate
            Public DTWAIN_GetFileTypeExtensions As DTWAIN_GetFileTypeExtensionsDelegate
            Public DTWAIN_GetFileTypeExtensionsA As DTWAIN_GetFileTypeExtensionsADelegate
            Public DTWAIN_GetFileTypeExtensionsW As DTWAIN_GetFileTypeExtensionsWDelegate
            Public DTWAIN_GetFileTypeName As DTWAIN_GetFileTypeNameDelegate
            Public DTWAIN_GetFileTypeNameA As DTWAIN_GetFileTypeNameADelegate
            Public DTWAIN_GetFileTypeNameW As DTWAIN_GetFileTypeNameWDelegate
            Public DTWAIN_GetHalftone As DTWAIN_GetHalftoneDelegate
            Public DTWAIN_GetHalftoneA As DTWAIN_GetHalftoneADelegate
            Public DTWAIN_GetHalftoneW As DTWAIN_GetHalftoneWDelegate
            Public DTWAIN_GetHighlight As DTWAIN_GetHighlightDelegate
            Public DTWAIN_GetHighlightString As DTWAIN_GetHighlightStringDelegate
            Public DTWAIN_GetHighlightStringA As DTWAIN_GetHighlightStringADelegate
            Public DTWAIN_GetHighlightStringW As DTWAIN_GetHighlightStringWDelegate
            Public DTWAIN_GetImageInfo As DTWAIN_GetImageInfoDelegate
            Public DTWAIN_GetImageInfoString As DTWAIN_GetImageInfoStringDelegate
            Public DTWAIN_GetImageInfoStringA As DTWAIN_GetImageInfoStringADelegate
            Public DTWAIN_GetImageInfoStringW As DTWAIN_GetImageInfoStringWDelegate
            Public DTWAIN_GetJobControl As DTWAIN_GetJobControlDelegate
            Public DTWAIN_GetJpegValues As DTWAIN_GetJpegValuesDelegate
            Public DTWAIN_GetJpegXRValues As DTWAIN_GetJpegXRValuesDelegate
            Public DTWAIN_GetLanguage As DTWAIN_GetLanguageDelegate
            Public DTWAIN_GetLastError As DTWAIN_GetLastErrorDelegate
            Public DTWAIN_GetLibraryPath As DTWAIN_GetLibraryPathDelegate
            Public DTWAIN_GetLibraryPathA As DTWAIN_GetLibraryPathADelegate
            Public DTWAIN_GetLibraryPathW As DTWAIN_GetLibraryPathWDelegate
            Public DTWAIN_GetLightPath As DTWAIN_GetLightPathDelegate
            Public DTWAIN_GetLightSource As DTWAIN_GetLightSourceDelegate
            Public DTWAIN_GetLightSources As DTWAIN_GetLightSourcesDelegate
            Public DTWAIN_GetLoggerCallback As DTWAIN_GetLoggerCallbackDelegate
            Public DTWAIN_GetLoggerCallbackA As DTWAIN_GetLoggerCallbackADelegate
            Public DTWAIN_GetLoggerCallbackW As DTWAIN_GetLoggerCallbackWDelegate
            Public DTWAIN_GetManualDuplexCount As DTWAIN_GetManualDuplexCountDelegate
            Public DTWAIN_GetMaxAcquisitions As DTWAIN_GetMaxAcquisitionsDelegate
            Public DTWAIN_GetMaxBuffers As DTWAIN_GetMaxBuffersDelegate
            Public DTWAIN_GetMaxPagesToAcquire As DTWAIN_GetMaxPagesToAcquireDelegate
            Public DTWAIN_GetMaxRetryAttempts As DTWAIN_GetMaxRetryAttemptsDelegate
            Public DTWAIN_GetNameFromCap As DTWAIN_GetNameFromCapDelegate
            Public DTWAIN_GetNameFromCapA As DTWAIN_GetNameFromCapADelegate
            Public DTWAIN_GetNameFromCapW As DTWAIN_GetNameFromCapWDelegate
            Public DTWAIN_GetNoiseFilter As DTWAIN_GetNoiseFilterDelegate
            Public DTWAIN_GetNumAcquiredImages As DTWAIN_GetNumAcquiredImagesDelegate
            Public DTWAIN_GetNumAcquisitions As DTWAIN_GetNumAcquisitionsDelegate
            Public DTWAIN_GetOCRCapValues As DTWAIN_GetOCRCapValuesDelegate
            Public DTWAIN_GetOCRErrorString As DTWAIN_GetOCRErrorStringDelegate
            Public DTWAIN_GetOCRErrorStringA As DTWAIN_GetOCRErrorStringADelegate
            Public DTWAIN_GetOCRErrorStringW As DTWAIN_GetOCRErrorStringWDelegate
            Public DTWAIN_GetOCRLastError As DTWAIN_GetOCRLastErrorDelegate
            Public DTWAIN_GetOCRMajorMinorVersion As DTWAIN_GetOCRMajorMinorVersionDelegate
            Public DTWAIN_GetOCRManufacturer As DTWAIN_GetOCRManufacturerDelegate
            Public DTWAIN_GetOCRManufacturerA As DTWAIN_GetOCRManufacturerADelegate
            Public DTWAIN_GetOCRManufacturerW As DTWAIN_GetOCRManufacturerWDelegate
            Public DTWAIN_GetOCRProductFamily As DTWAIN_GetOCRProductFamilyDelegate
            Public DTWAIN_GetOCRProductFamilyA As DTWAIN_GetOCRProductFamilyADelegate
            Public DTWAIN_GetOCRProductFamilyW As DTWAIN_GetOCRProductFamilyWDelegate
            Public DTWAIN_GetOCRProductName As DTWAIN_GetOCRProductNameDelegate
            Public DTWAIN_GetOCRProductNameA As DTWAIN_GetOCRProductNameADelegate
            Public DTWAIN_GetOCRProductNameW As DTWAIN_GetOCRProductNameWDelegate
            Public DTWAIN_GetOCRText As DTWAIN_GetOCRTextDelegate
            Public DTWAIN_GetOCRTextA As DTWAIN_GetOCRTextADelegate
            Public DTWAIN_GetOCRTextInfoFloat As DTWAIN_GetOCRTextInfoFloatDelegate
            Public DTWAIN_GetOCRTextInfoFloatEx As DTWAIN_GetOCRTextInfoFloatExDelegate
            Public DTWAIN_GetOCRTextInfoHandle As DTWAIN_GetOCRTextInfoHandleDelegate
            Public DTWAIN_GetOCRTextInfoLong As DTWAIN_GetOCRTextInfoLongDelegate
            Public DTWAIN_GetOCRTextInfoLongEx As DTWAIN_GetOCRTextInfoLongExDelegate
            Public DTWAIN_GetOCRTextW As DTWAIN_GetOCRTextWDelegate
            Public DTWAIN_GetOCRVersionInfo As DTWAIN_GetOCRVersionInfoDelegate
            Public DTWAIN_GetOCRVersionInfoA As DTWAIN_GetOCRVersionInfoADelegate
            Public DTWAIN_GetOCRVersionInfoW As DTWAIN_GetOCRVersionInfoWDelegate
            Public DTWAIN_GetOrientation As DTWAIN_GetOrientationDelegate
            Public DTWAIN_GetOverscan As DTWAIN_GetOverscanDelegate
            Public DTWAIN_GetPDFTextElementFloat As DTWAIN_GetPDFTextElementFloatDelegate
            Public DTWAIN_GetPDFTextElementLong As DTWAIN_GetPDFTextElementLongDelegate
            Public DTWAIN_GetPDFTextElementString As DTWAIN_GetPDFTextElementStringDelegate
            Public DTWAIN_GetPDFTextElementStringA As DTWAIN_GetPDFTextElementStringADelegate
            Public DTWAIN_GetPDFTextElementStringW As DTWAIN_GetPDFTextElementStringWDelegate
            Public DTWAIN_GetPDFType1FontName As DTWAIN_GetPDFType1FontNameDelegate
            Public DTWAIN_GetPDFType1FontNameA As DTWAIN_GetPDFType1FontNameADelegate
            Public DTWAIN_GetPDFType1FontNameW As DTWAIN_GetPDFType1FontNameWDelegate
            Public DTWAIN_GetPaperSize As DTWAIN_GetPaperSizeDelegate
            Public DTWAIN_GetPaperSizeName As DTWAIN_GetPaperSizeNameDelegate
            Public DTWAIN_GetPaperSizeNameA As DTWAIN_GetPaperSizeNameADelegate
            Public DTWAIN_GetPaperSizeNameW As DTWAIN_GetPaperSizeNameWDelegate
            Public DTWAIN_GetPatchMaxPriorities As DTWAIN_GetPatchMaxPrioritiesDelegate
            Public DTWAIN_GetPatchMaxRetries As DTWAIN_GetPatchMaxRetriesDelegate
            Public DTWAIN_GetPatchPriorities As DTWAIN_GetPatchPrioritiesDelegate
            Public DTWAIN_GetPatchSearchMode As DTWAIN_GetPatchSearchModeDelegate
            Public DTWAIN_GetPatchTimeOut As DTWAIN_GetPatchTimeOutDelegate
            Public DTWAIN_GetPixelFlavor As DTWAIN_GetPixelFlavorDelegate
            Public DTWAIN_GetPixelType As DTWAIN_GetPixelTypeDelegate
            Public DTWAIN_GetPrinter As DTWAIN_GetPrinterDelegate
            Public DTWAIN_GetPrinterStartNumber As DTWAIN_GetPrinterStartNumberDelegate
            Public DTWAIN_GetPrinterStringMode As DTWAIN_GetPrinterStringModeDelegate
            Public DTWAIN_GetPrinterStrings As DTWAIN_GetPrinterStringsDelegate
            Public DTWAIN_GetPrinterSuffixString As DTWAIN_GetPrinterSuffixStringDelegate
            Public DTWAIN_GetPrinterSuffixStringA As DTWAIN_GetPrinterSuffixStringADelegate
            Public DTWAIN_GetPrinterSuffixStringW As DTWAIN_GetPrinterSuffixStringWDelegate
            Public DTWAIN_GetRegisteredMsg As DTWAIN_GetRegisteredMsgDelegate
            Public DTWAIN_GetResolution As DTWAIN_GetResolutionDelegate
            Public DTWAIN_GetResolutionString As DTWAIN_GetResolutionStringDelegate
            Public DTWAIN_GetResolutionStringA As DTWAIN_GetResolutionStringADelegate
            Public DTWAIN_GetResolutionStringW As DTWAIN_GetResolutionStringWDelegate
            Public DTWAIN_GetResourceString As DTWAIN_GetResourceStringDelegate
            Public DTWAIN_GetResourceStringA As DTWAIN_GetResourceStringADelegate
            Public DTWAIN_GetResourceStringW As DTWAIN_GetResourceStringWDelegate
            Public DTWAIN_GetRotation As DTWAIN_GetRotationDelegate
            Public DTWAIN_GetRotationString As DTWAIN_GetRotationStringDelegate
            Public DTWAIN_GetRotationStringA As DTWAIN_GetRotationStringADelegate
            Public DTWAIN_GetRotationStringW As DTWAIN_GetRotationStringWDelegate
            Public DTWAIN_GetSaveFileName As DTWAIN_GetSaveFileNameDelegate
            Public DTWAIN_GetSaveFileNameA As DTWAIN_GetSaveFileNameADelegate
            Public DTWAIN_GetSaveFileNameW As DTWAIN_GetSaveFileNameWDelegate
            Public DTWAIN_GetSavedFilesCount As DTWAIN_GetSavedFilesCountDelegate
            Public DTWAIN_GetSessionDetails As DTWAIN_GetSessionDetailsDelegate
            Public DTWAIN_GetSessionDetailsA As DTWAIN_GetSessionDetailsADelegate
            Public DTWAIN_GetSessionDetailsW As DTWAIN_GetSessionDetailsWDelegate
            Public DTWAIN_GetShadow As DTWAIN_GetShadowDelegate
            Public DTWAIN_GetShadowString As DTWAIN_GetShadowStringDelegate
            Public DTWAIN_GetShadowStringA As DTWAIN_GetShadowStringADelegate
            Public DTWAIN_GetShadowStringW As DTWAIN_GetShadowStringWDelegate
            Public DTWAIN_GetShortVersionString As DTWAIN_GetShortVersionStringDelegate
            Public DTWAIN_GetShortVersionStringA As DTWAIN_GetShortVersionStringADelegate
            Public DTWAIN_GetShortVersionStringW As DTWAIN_GetShortVersionStringWDelegate
            Public DTWAIN_GetSourceAcquisitions As DTWAIN_GetSourceAcquisitionsDelegate
            Public DTWAIN_GetSourceDetails As DTWAIN_GetSourceDetailsDelegate
            Public DTWAIN_GetSourceDetailsA As DTWAIN_GetSourceDetailsADelegate
            Public DTWAIN_GetSourceDetailsW As DTWAIN_GetSourceDetailsWDelegate
            Public DTWAIN_GetSourceID As DTWAIN_GetSourceIDDelegate
            Public DTWAIN_GetSourceManufacturer As DTWAIN_GetSourceManufacturerDelegate
            Public DTWAIN_GetSourceManufacturerA As DTWAIN_GetSourceManufacturerADelegate
            Public DTWAIN_GetSourceManufacturerW As DTWAIN_GetSourceManufacturerWDelegate
            Public DTWAIN_GetSourceProductFamily As DTWAIN_GetSourceProductFamilyDelegate
            Public DTWAIN_GetSourceProductFamilyA As DTWAIN_GetSourceProductFamilyADelegate
            Public DTWAIN_GetSourceProductFamilyW As DTWAIN_GetSourceProductFamilyWDelegate
            Public DTWAIN_GetSourceProductName As DTWAIN_GetSourceProductNameDelegate
            Public DTWAIN_GetSourceProductNameA As DTWAIN_GetSourceProductNameADelegate
            Public DTWAIN_GetSourceProductNameW As DTWAIN_GetSourceProductNameWDelegate
            Public DTWAIN_GetSourceUnit As DTWAIN_GetSourceUnitDelegate
            Public DTWAIN_GetSourceVersionInfo As DTWAIN_GetSourceVersionInfoDelegate
            Public DTWAIN_GetSourceVersionInfoA As DTWAIN_GetSourceVersionInfoADelegate
            Public DTWAIN_GetSourceVersionInfoW As DTWAIN_GetSourceVersionInfoWDelegate
            Public DTWAIN_GetSourceVersionNumber As DTWAIN_GetSourceVersionNumberDelegate
            Public DTWAIN_GetStaticLibVersion As DTWAIN_GetStaticLibVersionDelegate
            Public DTWAIN_GetTempFileDirectory As DTWAIN_GetTempFileDirectoryDelegate
            Public DTWAIN_GetTempFileDirectoryA As DTWAIN_GetTempFileDirectoryADelegate
            Public DTWAIN_GetTempFileDirectoryW As DTWAIN_GetTempFileDirectoryWDelegate
            Public DTWAIN_GetThreshold As DTWAIN_GetThresholdDelegate
            Public DTWAIN_GetThresholdString As DTWAIN_GetThresholdStringDelegate
            Public DTWAIN_GetThresholdStringA As DTWAIN_GetThresholdStringADelegate
            Public DTWAIN_GetThresholdStringW As DTWAIN_GetThresholdStringWDelegate
            Public DTWAIN_GetTimeDate As DTWAIN_GetTimeDateDelegate
            Public DTWAIN_GetTimeDateA As DTWAIN_GetTimeDateADelegate
            Public DTWAIN_GetTimeDateW As DTWAIN_GetTimeDateWDelegate
            Public DTWAIN_GetTwainAppID As DTWAIN_GetTwainAppIDDelegate
            Public DTWAIN_GetTwainAvailability As DTWAIN_GetTwainAvailabilityDelegate
            Public DTWAIN_GetTwainAvailabilityEx As DTWAIN_GetTwainAvailabilityExDelegate
            Public DTWAIN_GetTwainAvailabilityExA As DTWAIN_GetTwainAvailabilityExADelegate
            Public DTWAIN_GetTwainAvailabilityExW As DTWAIN_GetTwainAvailabilityExWDelegate
            Public DTWAIN_GetTwainCountryName As DTWAIN_GetTwainCountryNameDelegate
            Public DTWAIN_GetTwainCountryNameA As DTWAIN_GetTwainCountryNameADelegate
            Public DTWAIN_GetTwainCountryNameW As DTWAIN_GetTwainCountryNameWDelegate
            Public DTWAIN_GetTwainCountryValue As DTWAIN_GetTwainCountryValueDelegate
            Public DTWAIN_GetTwainCountryValueA As DTWAIN_GetTwainCountryValueADelegate
            Public DTWAIN_GetTwainCountryValueW As DTWAIN_GetTwainCountryValueWDelegate
            Public DTWAIN_GetTwainHwnd As DTWAIN_GetTwainHwndDelegate
            Public DTWAIN_GetTwainIDFromName As DTWAIN_GetTwainIDFromNameDelegate
            Public DTWAIN_GetTwainIDFromNameA As DTWAIN_GetTwainIDFromNameADelegate
            Public DTWAIN_GetTwainIDFromNameW As DTWAIN_GetTwainIDFromNameWDelegate
            Public DTWAIN_GetTwainLanguageName As DTWAIN_GetTwainLanguageNameDelegate
            Public DTWAIN_GetTwainLanguageNameA As DTWAIN_GetTwainLanguageNameADelegate
            Public DTWAIN_GetTwainLanguageNameW As DTWAIN_GetTwainLanguageNameWDelegate
            Public DTWAIN_GetTwainLanguageValue As DTWAIN_GetTwainLanguageValueDelegate
            Public DTWAIN_GetTwainLanguageValueA As DTWAIN_GetTwainLanguageValueADelegate
            Public DTWAIN_GetTwainLanguageValueW As DTWAIN_GetTwainLanguageValueWDelegate
            Public DTWAIN_GetTwainMode As DTWAIN_GetTwainModeDelegate
            Public DTWAIN_GetTwainNameFromConstant As DTWAIN_GetTwainNameFromConstantDelegate
            Public DTWAIN_GetTwainNameFromConstantA As DTWAIN_GetTwainNameFromConstantADelegate
            Public DTWAIN_GetTwainNameFromConstantW As DTWAIN_GetTwainNameFromConstantWDelegate
            Public DTWAIN_GetTwainStringName As DTWAIN_GetTwainStringNameDelegate
            Public DTWAIN_GetTwainStringNameA As DTWAIN_GetTwainStringNameADelegate
            Public DTWAIN_GetTwainStringNameW As DTWAIN_GetTwainStringNameWDelegate
            Public DTWAIN_GetTwainTimeout As DTWAIN_GetTwainTimeoutDelegate
            Public DTWAIN_GetVersion As DTWAIN_GetVersionDelegate
            Public DTWAIN_GetVersionCopyright As DTWAIN_GetVersionCopyrightDelegate
            Public DTWAIN_GetVersionCopyrightA As DTWAIN_GetVersionCopyrightADelegate
            Public DTWAIN_GetVersionCopyrightW As DTWAIN_GetVersionCopyrightWDelegate
            Public DTWAIN_GetVersionEx As DTWAIN_GetVersionExDelegate
            Public DTWAIN_GetVersionInfo As DTWAIN_GetVersionInfoDelegate
            Public DTWAIN_GetVersionInfoA As DTWAIN_GetVersionInfoADelegate
            Public DTWAIN_GetVersionInfoW As DTWAIN_GetVersionInfoWDelegate
            Public DTWAIN_GetVersionString As DTWAIN_GetVersionStringDelegate
            Public DTWAIN_GetVersionStringA As DTWAIN_GetVersionStringADelegate
            Public DTWAIN_GetVersionStringW As DTWAIN_GetVersionStringWDelegate
            Public DTWAIN_GetWindowsVersionInfo As DTWAIN_GetWindowsVersionInfoDelegate
            Public DTWAIN_GetWindowsVersionInfoA As DTWAIN_GetWindowsVersionInfoADelegate
            Public DTWAIN_GetWindowsVersionInfoW As DTWAIN_GetWindowsVersionInfoWDelegate
            Public DTWAIN_GetXResolution As DTWAIN_GetXResolutionDelegate
            Public DTWAIN_GetXResolutionString As DTWAIN_GetXResolutionStringDelegate
            Public DTWAIN_GetXResolutionStringA As DTWAIN_GetXResolutionStringADelegate
            Public DTWAIN_GetXResolutionStringW As DTWAIN_GetXResolutionStringWDelegate
            Public DTWAIN_GetYResolution As DTWAIN_GetYResolutionDelegate
            Public DTWAIN_GetYResolutionString As DTWAIN_GetYResolutionStringDelegate
            Public DTWAIN_GetYResolutionStringA As DTWAIN_GetYResolutionStringADelegate
            Public DTWAIN_GetYResolutionStringW As DTWAIN_GetYResolutionStringWDelegate
            Public DTWAIN_InitExtImageInfo As DTWAIN_InitExtImageInfoDelegate
            Public DTWAIN_InitImageFileAppend As DTWAIN_InitImageFileAppendDelegate
            Public DTWAIN_InitImageFileAppendA As DTWAIN_InitImageFileAppendADelegate
            Public DTWAIN_InitImageFileAppendW As DTWAIN_InitImageFileAppendWDelegate
            Public DTWAIN_InitOCRInterface As DTWAIN_InitOCRInterfaceDelegate
            Public DTWAIN_IsAcquiring As DTWAIN_IsAcquiringDelegate
            Public DTWAIN_IsAudioXferSupported As DTWAIN_IsAudioXferSupportedDelegate
            Public DTWAIN_IsAutoBorderDetectEnabled As DTWAIN_IsAutoBorderDetectEnabledDelegate
            Public DTWAIN_IsAutoBorderDetectSupported As DTWAIN_IsAutoBorderDetectSupportedDelegate
            Public DTWAIN_IsAutoBrightEnabled As DTWAIN_IsAutoBrightEnabledDelegate
            Public DTWAIN_IsAutoBrightSupported As DTWAIN_IsAutoBrightSupportedDelegate
            Public DTWAIN_IsAutoDeskewEnabled As DTWAIN_IsAutoDeskewEnabledDelegate
            Public DTWAIN_IsAutoDeskewSupported As DTWAIN_IsAutoDeskewSupportedDelegate
            Public DTWAIN_IsAutoFeedEnabled As DTWAIN_IsAutoFeedEnabledDelegate
            Public DTWAIN_IsAutoFeedSupported As DTWAIN_IsAutoFeedSupportedDelegate
            Public DTWAIN_IsAutoRotateEnabled As DTWAIN_IsAutoRotateEnabledDelegate
            Public DTWAIN_IsAutoRotateSupported As DTWAIN_IsAutoRotateSupportedDelegate
            Public DTWAIN_IsAutoScanEnabled As DTWAIN_IsAutoScanEnabledDelegate
            Public DTWAIN_IsAutomaticSenseMediumEnabled As DTWAIN_IsAutomaticSenseMediumEnabledDelegate
            Public DTWAIN_IsAutomaticSenseMediumSupported As DTWAIN_IsAutomaticSenseMediumSupportedDelegate
            Public DTWAIN_IsBlankPageDetectionOn As DTWAIN_IsBlankPageDetectionOnDelegate
            Public DTWAIN_IsBufferedTileModeOn As DTWAIN_IsBufferedTileModeOnDelegate
            Public DTWAIN_IsBufferedTileModeSupported As DTWAIN_IsBufferedTileModeSupportedDelegate
            Public DTWAIN_IsCapSupported As DTWAIN_IsCapSupportedDelegate
            Public DTWAIN_IsCompressionSupported As DTWAIN_IsCompressionSupportedDelegate
            Public DTWAIN_IsCustomDSDataSupported As DTWAIN_IsCustomDSDataSupportedDelegate
            Public DTWAIN_IsDIBBlank As DTWAIN_IsDIBBlankDelegate
            Public DTWAIN_IsDIBBlankString As DTWAIN_IsDIBBlankStringDelegate
            Public DTWAIN_IsDIBBlankStringA As DTWAIN_IsDIBBlankStringADelegate
            Public DTWAIN_IsDIBBlankStringW As DTWAIN_IsDIBBlankStringWDelegate
            Public DTWAIN_IsDeviceEventSupported As DTWAIN_IsDeviceEventSupportedDelegate
            Public DTWAIN_IsDeviceOnLine As DTWAIN_IsDeviceOnLineDelegate
            Public DTWAIN_IsDoubleFeedDetectLengthSupported As DTWAIN_IsDoubleFeedDetectLengthSupportedDelegate
            Public DTWAIN_IsDoubleFeedDetectSupported As DTWAIN_IsDoubleFeedDetectSupportedDelegate
            Public DTWAIN_IsDoublePageCountOnDuplex As DTWAIN_IsDoublePageCountOnDuplexDelegate
            Public DTWAIN_IsDuplexEnabled As DTWAIN_IsDuplexEnabledDelegate
            Public DTWAIN_IsDuplexSupported As DTWAIN_IsDuplexSupportedDelegate
            Public DTWAIN_IsExtImageInfoSupported As DTWAIN_IsExtImageInfoSupportedDelegate
            Public DTWAIN_IsFeederEnabled As DTWAIN_IsFeederEnabledDelegate
            Public DTWAIN_IsFeederLoaded As DTWAIN_IsFeederLoadedDelegate
            Public DTWAIN_IsFeederSensitive As DTWAIN_IsFeederSensitiveDelegate
            Public DTWAIN_IsFeederSupported As DTWAIN_IsFeederSupportedDelegate
            Public DTWAIN_IsFileSystemSupported As DTWAIN_IsFileSystemSupportedDelegate
            Public DTWAIN_IsFileXferSupported As DTWAIN_IsFileXferSupportedDelegate
            Public DTWAIN_IsIAFieldALastPageSupported As DTWAIN_IsIAFieldALastPageSupportedDelegate
            Public DTWAIN_IsIAFieldALevelSupported As DTWAIN_IsIAFieldALevelSupportedDelegate
            Public DTWAIN_IsIAFieldAPrintFormatSupported As DTWAIN_IsIAFieldAPrintFormatSupportedDelegate
            Public DTWAIN_IsIAFieldAValueSupported As DTWAIN_IsIAFieldAValueSupportedDelegate
            Public DTWAIN_IsIAFieldBLastPageSupported As DTWAIN_IsIAFieldBLastPageSupportedDelegate
            Public DTWAIN_IsIAFieldBLevelSupported As DTWAIN_IsIAFieldBLevelSupportedDelegate
            Public DTWAIN_IsIAFieldBPrintFormatSupported As DTWAIN_IsIAFieldBPrintFormatSupportedDelegate
            Public DTWAIN_IsIAFieldBValueSupported As DTWAIN_IsIAFieldBValueSupportedDelegate
            Public DTWAIN_IsIAFieldCLastPageSupported As DTWAIN_IsIAFieldCLastPageSupportedDelegate
            Public DTWAIN_IsIAFieldCLevelSupported As DTWAIN_IsIAFieldCLevelSupportedDelegate
            Public DTWAIN_IsIAFieldCPrintFormatSupported As DTWAIN_IsIAFieldCPrintFormatSupportedDelegate
            Public DTWAIN_IsIAFieldCValueSupported As DTWAIN_IsIAFieldCValueSupportedDelegate
            Public DTWAIN_IsIAFieldDLastPageSupported As DTWAIN_IsIAFieldDLastPageSupportedDelegate
            Public DTWAIN_IsIAFieldDLevelSupported As DTWAIN_IsIAFieldDLevelSupportedDelegate
            Public DTWAIN_IsIAFieldDPrintFormatSupported As DTWAIN_IsIAFieldDPrintFormatSupportedDelegate
            Public DTWAIN_IsIAFieldDValueSupported As DTWAIN_IsIAFieldDValueSupportedDelegate
            Public DTWAIN_IsIAFieldELastPageSupported As DTWAIN_IsIAFieldELastPageSupportedDelegate
            Public DTWAIN_IsIAFieldELevelSupported As DTWAIN_IsIAFieldELevelSupportedDelegate
            Public DTWAIN_IsIAFieldEPrintFormatSupported As DTWAIN_IsIAFieldEPrintFormatSupportedDelegate
            Public DTWAIN_IsIAFieldEValueSupported As DTWAIN_IsIAFieldEValueSupportedDelegate
            Public DTWAIN_IsImageAddressingSupported As DTWAIN_IsImageAddressingSupportedDelegate
            Public DTWAIN_IsIndicatorEnabled As DTWAIN_IsIndicatorEnabledDelegate
            Public DTWAIN_IsIndicatorSupported As DTWAIN_IsIndicatorSupportedDelegate
            Public DTWAIN_IsInitialized As DTWAIN_IsInitializedDelegate
            Public DTWAIN_IsJPEGSupported As DTWAIN_IsJPEGSupportedDelegate
            Public DTWAIN_IsJobControlSupported As DTWAIN_IsJobControlSupportedDelegate
            Public DTWAIN_IsLampEnabled As DTWAIN_IsLampEnabledDelegate
            Public DTWAIN_IsLampSupported As DTWAIN_IsLampSupportedDelegate
            Public DTWAIN_IsLightPathSupported As DTWAIN_IsLightPathSupportedDelegate
            Public DTWAIN_IsLightSourceSupported As DTWAIN_IsLightSourceSupportedDelegate
            Public DTWAIN_IsMaxBuffersSupported As DTWAIN_IsMaxBuffersSupportedDelegate
            Public DTWAIN_IsMemFileXferSupported As DTWAIN_IsMemFileXferSupportedDelegate
            Public DTWAIN_IsMsgNotifyEnabled As DTWAIN_IsMsgNotifyEnabledDelegate
            Public DTWAIN_IsNotifyTripletsEnabled As DTWAIN_IsNotifyTripletsEnabledDelegate
            Public DTWAIN_IsOCREngineActivated As DTWAIN_IsOCREngineActivatedDelegate
            Public DTWAIN_IsOpenSourcesOnSelect As DTWAIN_IsOpenSourcesOnSelectDelegate
            Public DTWAIN_IsOrientationSupported As DTWAIN_IsOrientationSupportedDelegate
            Public DTWAIN_IsOverscanSupported As DTWAIN_IsOverscanSupportedDelegate
            Public DTWAIN_IsPDFSupported As DTWAIN_IsPDFSupportedDelegate
            Public DTWAIN_IsPNGSupported As DTWAIN_IsPNGSupportedDelegate
            Public DTWAIN_IsPaperDetectable As DTWAIN_IsPaperDetectableDelegate
            Public DTWAIN_IsPaperSizeSupported As DTWAIN_IsPaperSizeSupportedDelegate
            Public DTWAIN_IsPatchCapsSupported As DTWAIN_IsPatchCapsSupportedDelegate
            Public DTWAIN_IsPatchDetectEnabled As DTWAIN_IsPatchDetectEnabledDelegate
            Public DTWAIN_IsPatchSupported As DTWAIN_IsPatchSupportedDelegate
            Public DTWAIN_IsPeekMessageLoopEnabled As DTWAIN_IsPeekMessageLoopEnabledDelegate
            Public DTWAIN_IsPixelTypeSupported As DTWAIN_IsPixelTypeSupportedDelegate
            Public DTWAIN_IsPrinterEnabled As DTWAIN_IsPrinterEnabledDelegate
            Public DTWAIN_IsPrinterSupported As DTWAIN_IsPrinterSupportedDelegate
            Public DTWAIN_IsRotationSupported As DTWAIN_IsRotationSupportedDelegate
            Public DTWAIN_IsSessionEnabled As DTWAIN_IsSessionEnabledDelegate
            Public DTWAIN_IsSkipImageInfoError As DTWAIN_IsSkipImageInfoErrorDelegate
            Public DTWAIN_IsSourceAcquiring As DTWAIN_IsSourceAcquiringDelegate
            Public DTWAIN_IsSourceAcquiringEx As DTWAIN_IsSourceAcquiringExDelegate
            Public DTWAIN_IsSourceInUIOnlyMode As DTWAIN_IsSourceInUIOnlyModeDelegate
            Public DTWAIN_IsSourceOpen As DTWAIN_IsSourceOpenDelegate
            Public DTWAIN_IsSourceSelected As DTWAIN_IsSourceSelectedDelegate
            Public DTWAIN_IsSourceValid As DTWAIN_IsSourceValidDelegate
            Public DTWAIN_IsTIFFSupported As DTWAIN_IsTIFFSupportedDelegate
            Public DTWAIN_IsThumbnailEnabled As DTWAIN_IsThumbnailEnabledDelegate
            Public DTWAIN_IsThumbnailSupported As DTWAIN_IsThumbnailSupportedDelegate
            Public DTWAIN_IsTwainAvailable As DTWAIN_IsTwainAvailableDelegate
            Public DTWAIN_IsTwainAvailableEx As DTWAIN_IsTwainAvailableExDelegate
            Public DTWAIN_IsTwainAvailableExA As DTWAIN_IsTwainAvailableExADelegate
            Public DTWAIN_IsTwainAvailableExW As DTWAIN_IsTwainAvailableExWDelegate
            Public DTWAIN_IsTwainMsg As DTWAIN_IsTwainMsgDelegate
            Public DTWAIN_IsUIControllable As DTWAIN_IsUIControllableDelegate
            Public DTWAIN_IsUIEnabled As DTWAIN_IsUIEnabledDelegate
            Public DTWAIN_IsUIOnlySupported As DTWAIN_IsUIOnlySupportedDelegate
            Public DTWAIN_LoadCustomStringResources As DTWAIN_LoadCustomStringResourcesDelegate
            Public DTWAIN_LoadCustomStringResourcesA As DTWAIN_LoadCustomStringResourcesADelegate
            Public DTWAIN_LoadCustomStringResourcesEx As DTWAIN_LoadCustomStringResourcesExDelegate
            Public DTWAIN_LoadCustomStringResourcesExA As DTWAIN_LoadCustomStringResourcesExADelegate
            Public DTWAIN_LoadCustomStringResourcesExW As DTWAIN_LoadCustomStringResourcesExWDelegate
            Public DTWAIN_LoadCustomStringResourcesW As DTWAIN_LoadCustomStringResourcesWDelegate
            Public DTWAIN_LoadLanguageResource As DTWAIN_LoadLanguageResourceDelegate
            Public DTWAIN_LockMemory As DTWAIN_LockMemoryDelegate
            Public DTWAIN_LockMemoryEx As DTWAIN_LockMemoryExDelegate
            Public DTWAIN_LogMessage As DTWAIN_LogMessageDelegate
            Public DTWAIN_LogMessageA As DTWAIN_LogMessageADelegate
            Public DTWAIN_LogMessageW As DTWAIN_LogMessageWDelegate
            Public DTWAIN_MakeRGB As DTWAIN_MakeRGBDelegate
            Public DTWAIN_OpenSource As DTWAIN_OpenSourceDelegate
            Public DTWAIN_OpenSourcesOnSelect As DTWAIN_OpenSourcesOnSelectDelegate
            Public DTWAIN_RangeCreate As DTWAIN_RangeCreateDelegate
            Public DTWAIN_RangeCreateFromCap As DTWAIN_RangeCreateFromCapDelegate
            Public DTWAIN_RangeDestroy As DTWAIN_RangeDestroyDelegate
            Public DTWAIN_RangeExpand As DTWAIN_RangeExpandDelegate
            Public DTWAIN_RangeExpandEx As DTWAIN_RangeExpandExDelegate
            Public DTWAIN_RangeGetAll As DTWAIN_RangeGetAllDelegate
            Public DTWAIN_RangeGetAllFloat As DTWAIN_RangeGetAllFloatDelegate
            Public DTWAIN_RangeGetAllFloatString As DTWAIN_RangeGetAllFloatStringDelegate
            Public DTWAIN_RangeGetAllFloatStringA As DTWAIN_RangeGetAllFloatStringADelegate
            Public DTWAIN_RangeGetAllFloatStringW As DTWAIN_RangeGetAllFloatStringWDelegate
            Public DTWAIN_RangeGetAllLong As DTWAIN_RangeGetAllLongDelegate
            Public DTWAIN_RangeGetCount As DTWAIN_RangeGetCountDelegate
            Public DTWAIN_RangeGetExpValue As DTWAIN_RangeGetExpValueDelegate
            Public DTWAIN_RangeGetExpValueFloat As DTWAIN_RangeGetExpValueFloatDelegate
            Public DTWAIN_RangeGetExpValueFloatString As DTWAIN_RangeGetExpValueFloatStringDelegate
            Public DTWAIN_RangeGetExpValueFloatStringA As DTWAIN_RangeGetExpValueFloatStringADelegate
            Public DTWAIN_RangeGetExpValueFloatStringW As DTWAIN_RangeGetExpValueFloatStringWDelegate
            Public DTWAIN_RangeGetExpValueLong As DTWAIN_RangeGetExpValueLongDelegate
            Public DTWAIN_RangeGetNearestValue As DTWAIN_RangeGetNearestValueDelegate
            Public DTWAIN_RangeGetPos As DTWAIN_RangeGetPosDelegate
            Public DTWAIN_RangeGetPosFloat As DTWAIN_RangeGetPosFloatDelegate
            Public DTWAIN_RangeGetPosFloatString As DTWAIN_RangeGetPosFloatStringDelegate
            Public DTWAIN_RangeGetPosFloatStringA As DTWAIN_RangeGetPosFloatStringADelegate
            Public DTWAIN_RangeGetPosFloatStringW As DTWAIN_RangeGetPosFloatStringWDelegate
            Public DTWAIN_RangeGetPosLong As DTWAIN_RangeGetPosLongDelegate
            Public DTWAIN_RangeGetValue As DTWAIN_RangeGetValueDelegate
            Public DTWAIN_RangeGetValueFloat As DTWAIN_RangeGetValueFloatDelegate
            Public DTWAIN_RangeGetValueFloatString As DTWAIN_RangeGetValueFloatStringDelegate
            Public DTWAIN_RangeGetValueFloatStringA As DTWAIN_RangeGetValueFloatStringADelegate
            Public DTWAIN_RangeGetValueFloatStringW As DTWAIN_RangeGetValueFloatStringWDelegate
            Public DTWAIN_RangeGetValueLong As DTWAIN_RangeGetValueLongDelegate
            Public DTWAIN_RangeIsValid As DTWAIN_RangeIsValidDelegate
            Public DTWAIN_RangeNearestValueFloat As DTWAIN_RangeNearestValueFloatDelegate
            Public DTWAIN_RangeNearestValueFloatString As DTWAIN_RangeNearestValueFloatStringDelegate
            Public DTWAIN_RangeNearestValueFloatStringA As DTWAIN_RangeNearestValueFloatStringADelegate
            Public DTWAIN_RangeNearestValueFloatStringW As DTWAIN_RangeNearestValueFloatStringWDelegate
            Public DTWAIN_RangeNearestValueLong As DTWAIN_RangeNearestValueLongDelegate
            Public DTWAIN_RangeSetAll As DTWAIN_RangeSetAllDelegate
            Public DTWAIN_RangeSetAllFloat As DTWAIN_RangeSetAllFloatDelegate
            Public DTWAIN_RangeSetAllFloatString As DTWAIN_RangeSetAllFloatStringDelegate
            Public DTWAIN_RangeSetAllFloatStringA As DTWAIN_RangeSetAllFloatStringADelegate
            Public DTWAIN_RangeSetAllFloatStringW As DTWAIN_RangeSetAllFloatStringWDelegate
            Public DTWAIN_RangeSetAllLong As DTWAIN_RangeSetAllLongDelegate
            Public DTWAIN_RangeSetValue As DTWAIN_RangeSetValueDelegate
            Public DTWAIN_RangeSetValueFloat As DTWAIN_RangeSetValueFloatDelegate
            Public DTWAIN_RangeSetValueFloatString As DTWAIN_RangeSetValueFloatStringDelegate
            Public DTWAIN_RangeSetValueFloatStringA As DTWAIN_RangeSetValueFloatStringADelegate
            Public DTWAIN_RangeSetValueFloatStringW As DTWAIN_RangeSetValueFloatStringWDelegate
            Public DTWAIN_RangeSetValueLong As DTWAIN_RangeSetValueLongDelegate
            Public DTWAIN_ResetPDFTextElement As DTWAIN_ResetPDFTextElementDelegate
            Public DTWAIN_RewindPage As DTWAIN_RewindPageDelegate
            Public DTWAIN_SelectDefaultOCREngine As DTWAIN_SelectDefaultOCREngineDelegate
            Public DTWAIN_SelectDefaultSource As DTWAIN_SelectDefaultSourceDelegate
            Public DTWAIN_SelectDefaultSourceWithOpen As DTWAIN_SelectDefaultSourceWithOpenDelegate
            Public DTWAIN_SelectOCREngine As DTWAIN_SelectOCREngineDelegate
            Public DTWAIN_SelectOCREngine2 As DTWAIN_SelectOCREngine2Delegate
            Public DTWAIN_SelectOCREngine2A As DTWAIN_SelectOCREngine2ADelegate
            Public DTWAIN_SelectOCREngine2Ex As DTWAIN_SelectOCREngine2ExDelegate
            Public DTWAIN_SelectOCREngine2ExA As DTWAIN_SelectOCREngine2ExADelegate
            Public DTWAIN_SelectOCREngine2ExW As DTWAIN_SelectOCREngine2ExWDelegate
            Public DTWAIN_SelectOCREngine2W As DTWAIN_SelectOCREngine2WDelegate
            Public DTWAIN_SelectOCREngineByName As DTWAIN_SelectOCREngineByNameDelegate
            Public DTWAIN_SelectOCREngineByNameA As DTWAIN_SelectOCREngineByNameADelegate
            Public DTWAIN_SelectOCREngineByNameW As DTWAIN_SelectOCREngineByNameWDelegate
            Public DTWAIN_SelectSource As DTWAIN_SelectSourceDelegate
            Public DTWAIN_SelectSource2 As DTWAIN_SelectSource2Delegate
            Public DTWAIN_SelectSource2A As DTWAIN_SelectSource2ADelegate
            Public DTWAIN_SelectSource2Ex As DTWAIN_SelectSource2ExDelegate
            Public DTWAIN_SelectSource2ExA As DTWAIN_SelectSource2ExADelegate
            Public DTWAIN_SelectSource2ExW As DTWAIN_SelectSource2ExWDelegate
            Public DTWAIN_SelectSource2W As DTWAIN_SelectSource2WDelegate
            Public DTWAIN_SelectSourceByName As DTWAIN_SelectSourceByNameDelegate
            Public DTWAIN_SelectSourceByNameA As DTWAIN_SelectSourceByNameADelegate
            Public DTWAIN_SelectSourceByNameW As DTWAIN_SelectSourceByNameWDelegate
            Public DTWAIN_SelectSourceByNameWithOpen As DTWAIN_SelectSourceByNameWithOpenDelegate
            Public DTWAIN_SelectSourceByNameWithOpenA As DTWAIN_SelectSourceByNameWithOpenADelegate
            Public DTWAIN_SelectSourceByNameWithOpenW As DTWAIN_SelectSourceByNameWithOpenWDelegate
            Public DTWAIN_SelectSourceWithOpen As DTWAIN_SelectSourceWithOpenDelegate
            Public DTWAIN_SetAcquireArea As DTWAIN_SetAcquireAreaDelegate
            Public DTWAIN_SetAcquireArea2 As DTWAIN_SetAcquireArea2Delegate
            Public DTWAIN_SetAcquireArea2String As DTWAIN_SetAcquireArea2StringDelegate
            Public DTWAIN_SetAcquireArea2StringA As DTWAIN_SetAcquireArea2StringADelegate
            Public DTWAIN_SetAcquireArea2StringW As DTWAIN_SetAcquireArea2StringWDelegate
            Public DTWAIN_SetAcquireImageNegative As DTWAIN_SetAcquireImageNegativeDelegate
            Public DTWAIN_SetAcquireImageScale As DTWAIN_SetAcquireImageScaleDelegate
            Public DTWAIN_SetAcquireImageScaleString As DTWAIN_SetAcquireImageScaleStringDelegate
            Public DTWAIN_SetAcquireImageScaleStringA As DTWAIN_SetAcquireImageScaleStringADelegate
            Public DTWAIN_SetAcquireImageScaleStringW As DTWAIN_SetAcquireImageScaleStringWDelegate
            Public DTWAIN_SetAcquireStripBuffer As DTWAIN_SetAcquireStripBufferDelegate
            Public DTWAIN_SetAcquireStripSize As DTWAIN_SetAcquireStripSizeDelegate
            Public DTWAIN_SetAlarmVolume As DTWAIN_SetAlarmVolumeDelegate
            Public DTWAIN_SetAlarms As DTWAIN_SetAlarmsDelegate
            Public DTWAIN_SetAllCapsToDefault As DTWAIN_SetAllCapsToDefaultDelegate
            Public DTWAIN_SetAppInfo As DTWAIN_SetAppInfoDelegate
            Public DTWAIN_SetAppInfoA As DTWAIN_SetAppInfoADelegate
            Public DTWAIN_SetAppInfoW As DTWAIN_SetAppInfoWDelegate
            Public DTWAIN_SetAuthor As DTWAIN_SetAuthorDelegate
            Public DTWAIN_SetAuthorA As DTWAIN_SetAuthorADelegate
            Public DTWAIN_SetAuthorW As DTWAIN_SetAuthorWDelegate
            Public DTWAIN_SetAvailablePrinters As DTWAIN_SetAvailablePrintersDelegate
            Public DTWAIN_SetAvailablePrintersArray As DTWAIN_SetAvailablePrintersArrayDelegate
            Public DTWAIN_SetBitDepth As DTWAIN_SetBitDepthDelegate
            Public DTWAIN_SetBlankPageDetection As DTWAIN_SetBlankPageDetectionDelegate
            Public DTWAIN_SetBlankPageDetectionEx As DTWAIN_SetBlankPageDetectionExDelegate
            Public DTWAIN_SetBlankPageDetectionExString As DTWAIN_SetBlankPageDetectionExStringDelegate
            Public DTWAIN_SetBlankPageDetectionExStringA As DTWAIN_SetBlankPageDetectionExStringADelegate
            Public DTWAIN_SetBlankPageDetectionExStringW As DTWAIN_SetBlankPageDetectionExStringWDelegate
            Public DTWAIN_SetBlankPageDetectionString As DTWAIN_SetBlankPageDetectionStringDelegate
            Public DTWAIN_SetBlankPageDetectionStringA As DTWAIN_SetBlankPageDetectionStringADelegate
            Public DTWAIN_SetBlankPageDetectionStringW As DTWAIN_SetBlankPageDetectionStringWDelegate
            Public DTWAIN_SetBrightness As DTWAIN_SetBrightnessDelegate
            Public DTWAIN_SetBrightnessString As DTWAIN_SetBrightnessStringDelegate
            Public DTWAIN_SetBrightnessStringA As DTWAIN_SetBrightnessStringADelegate
            Public DTWAIN_SetBrightnessStringW As DTWAIN_SetBrightnessStringWDelegate
            Public DTWAIN_SetBufferedTileMode As DTWAIN_SetBufferedTileModeDelegate
            Public DTWAIN_SetCallback As DTWAIN_SetCallbackDelegate
            Public DTWAIN_SetCallback64 As DTWAIN_SetCallback64Delegate
            Public DTWAIN_SetCamera As DTWAIN_SetCameraDelegate
            Public DTWAIN_SetCameraA As DTWAIN_SetCameraADelegate
            Public DTWAIN_SetCameraW As DTWAIN_SetCameraWDelegate
            Public DTWAIN_SetCapValues As DTWAIN_SetCapValuesDelegate
            Public DTWAIN_SetCapValuesEx As DTWAIN_SetCapValuesExDelegate
            Public DTWAIN_SetCapValuesEx2 As DTWAIN_SetCapValuesEx2Delegate
            Public DTWAIN_SetCaption As DTWAIN_SetCaptionDelegate
            Public DTWAIN_SetCaptionA As DTWAIN_SetCaptionADelegate
            Public DTWAIN_SetCaptionW As DTWAIN_SetCaptionWDelegate
            Public DTWAIN_SetCompressionType As DTWAIN_SetCompressionTypeDelegate
            Public DTWAIN_SetContrast As DTWAIN_SetContrastDelegate
            Public DTWAIN_SetContrastString As DTWAIN_SetContrastStringDelegate
            Public DTWAIN_SetContrastStringA As DTWAIN_SetContrastStringADelegate
            Public DTWAIN_SetContrastStringW As DTWAIN_SetContrastStringWDelegate
            Public DTWAIN_SetCountry As DTWAIN_SetCountryDelegate
            Public DTWAIN_SetCurrentRetryCount As DTWAIN_SetCurrentRetryCountDelegate
            Public DTWAIN_SetCustomDSData As DTWAIN_SetCustomDSDataDelegate
            Public DTWAIN_SetDSMSearchOrder As DTWAIN_SetDSMSearchOrderDelegate
            Public DTWAIN_SetDSMSearchOrderEx As DTWAIN_SetDSMSearchOrderExDelegate
            Public DTWAIN_SetDSMSearchOrderExA As DTWAIN_SetDSMSearchOrderExADelegate
            Public DTWAIN_SetDSMSearchOrderExW As DTWAIN_SetDSMSearchOrderExWDelegate
            Public DTWAIN_SetDefaultSource As DTWAIN_SetDefaultSourceDelegate
            Public DTWAIN_SetDeviceNotifications As DTWAIN_SetDeviceNotificationsDelegate
            Public DTWAIN_SetDeviceTimeDate As DTWAIN_SetDeviceTimeDateDelegate
            Public DTWAIN_SetDeviceTimeDateA As DTWAIN_SetDeviceTimeDateADelegate
            Public DTWAIN_SetDeviceTimeDateW As DTWAIN_SetDeviceTimeDateWDelegate
            Public DTWAIN_SetDoubleFeedDetectLength As DTWAIN_SetDoubleFeedDetectLengthDelegate
            Public DTWAIN_SetDoubleFeedDetectLengthString As DTWAIN_SetDoubleFeedDetectLengthStringDelegate
            Public DTWAIN_SetDoubleFeedDetectLengthStringA As DTWAIN_SetDoubleFeedDetectLengthStringADelegate
            Public DTWAIN_SetDoubleFeedDetectLengthStringW As DTWAIN_SetDoubleFeedDetectLengthStringWDelegate
            Public DTWAIN_SetDoubleFeedDetectValues As DTWAIN_SetDoubleFeedDetectValuesDelegate
            Public DTWAIN_SetDoublePageCountOnDuplex As DTWAIN_SetDoublePageCountOnDuplexDelegate
            Public DTWAIN_SetEOJDetectValue As DTWAIN_SetEOJDetectValueDelegate
            Public DTWAIN_SetErrorBufferThreshold As DTWAIN_SetErrorBufferThresholdDelegate
            Public DTWAIN_SetErrorCallback As DTWAIN_SetErrorCallbackDelegate
            Public DTWAIN_SetErrorCallback64 As DTWAIN_SetErrorCallback64Delegate
            Public DTWAIN_SetFeederAlignment As DTWAIN_SetFeederAlignmentDelegate
            Public DTWAIN_SetFeederOrder As DTWAIN_SetFeederOrderDelegate
            Public DTWAIN_SetFeederWaitTime As DTWAIN_SetFeederWaitTimeDelegate
            Public DTWAIN_SetFileAutoIncrement As DTWAIN_SetFileAutoIncrementDelegate
            Public DTWAIN_SetFileCompressionType As DTWAIN_SetFileCompressionTypeDelegate
            Public DTWAIN_SetFileSavePos As DTWAIN_SetFileSavePosDelegate
            Public DTWAIN_SetFileSavePosA As DTWAIN_SetFileSavePosADelegate
            Public DTWAIN_SetFileSavePosW As DTWAIN_SetFileSavePosWDelegate
            Public DTWAIN_SetFileXferFormat As DTWAIN_SetFileXferFormatDelegate
            Public DTWAIN_SetHalftone As DTWAIN_SetHalftoneDelegate
            Public DTWAIN_SetHalftoneA As DTWAIN_SetHalftoneADelegate
            Public DTWAIN_SetHalftoneW As DTWAIN_SetHalftoneWDelegate
            Public DTWAIN_SetHighlight As DTWAIN_SetHighlightDelegate
            Public DTWAIN_SetHighlightString As DTWAIN_SetHighlightStringDelegate
            Public DTWAIN_SetHighlightStringA As DTWAIN_SetHighlightStringADelegate
            Public DTWAIN_SetHighlightStringW As DTWAIN_SetHighlightStringWDelegate
            Public DTWAIN_SetJobControl As DTWAIN_SetJobControlDelegate
            Public DTWAIN_SetJpegValues As DTWAIN_SetJpegValuesDelegate
            Public DTWAIN_SetJpegXRValues As DTWAIN_SetJpegXRValuesDelegate
            Public DTWAIN_SetLanguage As DTWAIN_SetLanguageDelegate
            Public DTWAIN_SetLastError As DTWAIN_SetLastErrorDelegate
            Public DTWAIN_SetLightPath As DTWAIN_SetLightPathDelegate
            Public DTWAIN_SetLightPathEx As DTWAIN_SetLightPathExDelegate
            Public DTWAIN_SetLightSource As DTWAIN_SetLightSourceDelegate
            Public DTWAIN_SetLightSources As DTWAIN_SetLightSourcesDelegate
            Public DTWAIN_SetLoggerCallback As DTWAIN_SetLoggerCallbackDelegate
            Public DTWAIN_SetLoggerCallbackA As DTWAIN_SetLoggerCallbackADelegate
            Public DTWAIN_SetLoggerCallbackW As DTWAIN_SetLoggerCallbackWDelegate
            Public DTWAIN_SetManualDuplexMode As DTWAIN_SetManualDuplexModeDelegate
            Public DTWAIN_SetMaxAcquisitions As DTWAIN_SetMaxAcquisitionsDelegate
            Public DTWAIN_SetMaxBuffers As DTWAIN_SetMaxBuffersDelegate
            Public DTWAIN_SetMaxRetryAttempts As DTWAIN_SetMaxRetryAttemptsDelegate
            Public DTWAIN_SetMultipageScanMode As DTWAIN_SetMultipageScanModeDelegate
            Public DTWAIN_SetNoiseFilter As DTWAIN_SetNoiseFilterDelegate
            Public DTWAIN_SetOCRCapValues As DTWAIN_SetOCRCapValuesDelegate
            Public DTWAIN_SetOrientation As DTWAIN_SetOrientationDelegate
            Public DTWAIN_SetOverscan As DTWAIN_SetOverscanDelegate
            Public DTWAIN_SetPDFAESEncryption As DTWAIN_SetPDFAESEncryptionDelegate
            Public DTWAIN_SetPDFASCIICompression As DTWAIN_SetPDFASCIICompressionDelegate
            Public DTWAIN_SetPDFAuthor As DTWAIN_SetPDFAuthorDelegate
            Public DTWAIN_SetPDFAuthorA As DTWAIN_SetPDFAuthorADelegate
            Public DTWAIN_SetPDFAuthorW As DTWAIN_SetPDFAuthorWDelegate
            Public DTWAIN_SetPDFCompression As DTWAIN_SetPDFCompressionDelegate
            Public DTWAIN_SetPDFCreator As DTWAIN_SetPDFCreatorDelegate
            Public DTWAIN_SetPDFCreatorA As DTWAIN_SetPDFCreatorADelegate
            Public DTWAIN_SetPDFCreatorW As DTWAIN_SetPDFCreatorWDelegate
            Public DTWAIN_SetPDFEncryption As DTWAIN_SetPDFEncryptionDelegate
            Public DTWAIN_SetPDFEncryptionA As DTWAIN_SetPDFEncryptionADelegate
            Public DTWAIN_SetPDFEncryptionW As DTWAIN_SetPDFEncryptionWDelegate
            Public DTWAIN_SetPDFJpegQuality As DTWAIN_SetPDFJpegQualityDelegate
            Public DTWAIN_SetPDFKeywords As DTWAIN_SetPDFKeywordsDelegate
            Public DTWAIN_SetPDFKeywordsA As DTWAIN_SetPDFKeywordsADelegate
            Public DTWAIN_SetPDFKeywordsW As DTWAIN_SetPDFKeywordsWDelegate
            Public DTWAIN_SetPDFOCRConversion As DTWAIN_SetPDFOCRConversionDelegate
            Public DTWAIN_SetPDFOCRMode As DTWAIN_SetPDFOCRModeDelegate
            Public DTWAIN_SetPDFOrientation As DTWAIN_SetPDFOrientationDelegate
            Public DTWAIN_SetPDFPageScale As DTWAIN_SetPDFPageScaleDelegate
            Public DTWAIN_SetPDFPageScaleString As DTWAIN_SetPDFPageScaleStringDelegate
            Public DTWAIN_SetPDFPageScaleStringA As DTWAIN_SetPDFPageScaleStringADelegate
            Public DTWAIN_SetPDFPageScaleStringW As DTWAIN_SetPDFPageScaleStringWDelegate
            Public DTWAIN_SetPDFPageSize As DTWAIN_SetPDFPageSizeDelegate
            Public DTWAIN_SetPDFPageSizeString As DTWAIN_SetPDFPageSizeStringDelegate
            Public DTWAIN_SetPDFPageSizeStringA As DTWAIN_SetPDFPageSizeStringADelegate
            Public DTWAIN_SetPDFPageSizeStringW As DTWAIN_SetPDFPageSizeStringWDelegate
            Public DTWAIN_SetPDFPolarity As DTWAIN_SetPDFPolarityDelegate
            Public DTWAIN_SetPDFProducer As DTWAIN_SetPDFProducerDelegate
            Public DTWAIN_SetPDFProducerA As DTWAIN_SetPDFProducerADelegate
            Public DTWAIN_SetPDFProducerW As DTWAIN_SetPDFProducerWDelegate
            Public DTWAIN_SetPDFSubject As DTWAIN_SetPDFSubjectDelegate
            Public DTWAIN_SetPDFSubjectA As DTWAIN_SetPDFSubjectADelegate
            Public DTWAIN_SetPDFSubjectW As DTWAIN_SetPDFSubjectWDelegate
            Public DTWAIN_SetPDFTextElementFloat As DTWAIN_SetPDFTextElementFloatDelegate
            Public DTWAIN_SetPDFTextElementLong As DTWAIN_SetPDFTextElementLongDelegate
            Public DTWAIN_SetPDFTextElementString As DTWAIN_SetPDFTextElementStringDelegate
            Public DTWAIN_SetPDFTextElementStringA As DTWAIN_SetPDFTextElementStringADelegate
            Public DTWAIN_SetPDFTextElementStringW As DTWAIN_SetPDFTextElementStringWDelegate
            Public DTWAIN_SetPDFTitle As DTWAIN_SetPDFTitleDelegate
            Public DTWAIN_SetPDFTitleA As DTWAIN_SetPDFTitleADelegate
            Public DTWAIN_SetPDFTitleW As DTWAIN_SetPDFTitleWDelegate
            Public DTWAIN_SetPaperSize As DTWAIN_SetPaperSizeDelegate
            Public DTWAIN_SetPatchMaxPriorities As DTWAIN_SetPatchMaxPrioritiesDelegate
            Public DTWAIN_SetPatchMaxRetries As DTWAIN_SetPatchMaxRetriesDelegate
            Public DTWAIN_SetPatchPriorities As DTWAIN_SetPatchPrioritiesDelegate
            Public DTWAIN_SetPatchSearchMode As DTWAIN_SetPatchSearchModeDelegate
            Public DTWAIN_SetPatchTimeOut As DTWAIN_SetPatchTimeOutDelegate
            Public DTWAIN_SetPixelFlavor As DTWAIN_SetPixelFlavorDelegate
            Public DTWAIN_SetPixelType As DTWAIN_SetPixelTypeDelegate
            Public DTWAIN_SetPostScriptTitle As DTWAIN_SetPostScriptTitleDelegate
            Public DTWAIN_SetPostScriptTitleA As DTWAIN_SetPostScriptTitleADelegate
            Public DTWAIN_SetPostScriptTitleW As DTWAIN_SetPostScriptTitleWDelegate
            Public DTWAIN_SetPostScriptType As DTWAIN_SetPostScriptTypeDelegate
            Public DTWAIN_SetPrinter As DTWAIN_SetPrinterDelegate
            Public DTWAIN_SetPrinterEx As DTWAIN_SetPrinterExDelegate
            Public DTWAIN_SetPrinterStartNumber As DTWAIN_SetPrinterStartNumberDelegate
            Public DTWAIN_SetPrinterStringMode As DTWAIN_SetPrinterStringModeDelegate
            Public DTWAIN_SetPrinterStrings As DTWAIN_SetPrinterStringsDelegate
            Public DTWAIN_SetPrinterSuffixString As DTWAIN_SetPrinterSuffixStringDelegate
            Public DTWAIN_SetPrinterSuffixStringA As DTWAIN_SetPrinterSuffixStringADelegate
            Public DTWAIN_SetPrinterSuffixStringW As DTWAIN_SetPrinterSuffixStringWDelegate
            Public DTWAIN_SetQueryCapSupport As DTWAIN_SetQueryCapSupportDelegate
            Public DTWAIN_SetResolution As DTWAIN_SetResolutionDelegate
            Public DTWAIN_SetResolutionString As DTWAIN_SetResolutionStringDelegate
            Public DTWAIN_SetResolutionStringA As DTWAIN_SetResolutionStringADelegate
            Public DTWAIN_SetResolutionStringW As DTWAIN_SetResolutionStringWDelegate
            Public DTWAIN_SetResourcePath As DTWAIN_SetResourcePathDelegate
            Public DTWAIN_SetResourcePathA As DTWAIN_SetResourcePathADelegate
            Public DTWAIN_SetResourcePathW As DTWAIN_SetResourcePathWDelegate
            Public DTWAIN_SetRotation As DTWAIN_SetRotationDelegate
            Public DTWAIN_SetRotationString As DTWAIN_SetRotationStringDelegate
            Public DTWAIN_SetRotationStringA As DTWAIN_SetRotationStringADelegate
            Public DTWAIN_SetRotationStringW As DTWAIN_SetRotationStringWDelegate
            Public DTWAIN_SetSaveFileName As DTWAIN_SetSaveFileNameDelegate
            Public DTWAIN_SetSaveFileNameA As DTWAIN_SetSaveFileNameADelegate
            Public DTWAIN_SetSaveFileNameW As DTWAIN_SetSaveFileNameWDelegate
            Public DTWAIN_SetShadow As DTWAIN_SetShadowDelegate
            Public DTWAIN_SetShadowString As DTWAIN_SetShadowStringDelegate
            Public DTWAIN_SetShadowStringA As DTWAIN_SetShadowStringADelegate
            Public DTWAIN_SetShadowStringW As DTWAIN_SetShadowStringWDelegate
            Public DTWAIN_SetSourceUnit As DTWAIN_SetSourceUnitDelegate
            Public DTWAIN_SetTIFFCompressType As DTWAIN_SetTIFFCompressTypeDelegate
            Public DTWAIN_SetTIFFInvert As DTWAIN_SetTIFFInvertDelegate
            Public DTWAIN_SetTempFileDirectory As DTWAIN_SetTempFileDirectoryDelegate
            Public DTWAIN_SetTempFileDirectoryA As DTWAIN_SetTempFileDirectoryADelegate
            Public DTWAIN_SetTempFileDirectoryEx As DTWAIN_SetTempFileDirectoryExDelegate
            Public DTWAIN_SetTempFileDirectoryExA As DTWAIN_SetTempFileDirectoryExADelegate
            Public DTWAIN_SetTempFileDirectoryExW As DTWAIN_SetTempFileDirectoryExWDelegate
            Public DTWAIN_SetTempFileDirectoryW As DTWAIN_SetTempFileDirectoryWDelegate
            Public DTWAIN_SetThreshold As DTWAIN_SetThresholdDelegate
            Public DTWAIN_SetThresholdString As DTWAIN_SetThresholdStringDelegate
            Public DTWAIN_SetThresholdStringA As DTWAIN_SetThresholdStringADelegate
            Public DTWAIN_SetThresholdStringW As DTWAIN_SetThresholdStringWDelegate
            Public DTWAIN_SetTwainDSM As DTWAIN_SetTwainDSMDelegate
            Public DTWAIN_SetTwainLog As DTWAIN_SetTwainLogDelegate
            Public DTWAIN_SetTwainLogA As DTWAIN_SetTwainLogADelegate
            Public DTWAIN_SetTwainLogW As DTWAIN_SetTwainLogWDelegate
            Public DTWAIN_SetTwainMode As DTWAIN_SetTwainModeDelegate
            Public DTWAIN_SetTwainTimeout As DTWAIN_SetTwainTimeoutDelegate
            Public DTWAIN_SetUpdateDibProc As DTWAIN_SetUpdateDibProcDelegate
            Public DTWAIN_SetXResolution As DTWAIN_SetXResolutionDelegate
            Public DTWAIN_SetXResolutionString As DTWAIN_SetXResolutionStringDelegate
            Public DTWAIN_SetXResolutionStringA As DTWAIN_SetXResolutionStringADelegate
            Public DTWAIN_SetXResolutionStringW As DTWAIN_SetXResolutionStringWDelegate
            Public DTWAIN_SetYResolution As DTWAIN_SetYResolutionDelegate
            Public DTWAIN_SetYResolutionString As DTWAIN_SetYResolutionStringDelegate
            Public DTWAIN_SetYResolutionStringA As DTWAIN_SetYResolutionStringADelegate
            Public DTWAIN_SetYResolutionStringW As DTWAIN_SetYResolutionStringWDelegate
            Public DTWAIN_ShowUIOnly As DTWAIN_ShowUIOnlyDelegate
            Public DTWAIN_ShutdownOCREngine As DTWAIN_ShutdownOCREngineDelegate
            Public DTWAIN_SkipImageInfoError As DTWAIN_SkipImageInfoErrorDelegate
            Public DTWAIN_StartThread As DTWAIN_StartThreadDelegate
            Public DTWAIN_StartTwainSession As DTWAIN_StartTwainSessionDelegate
            Public DTWAIN_StartTwainSessionA As DTWAIN_StartTwainSessionADelegate
            Public DTWAIN_StartTwainSessionW As DTWAIN_StartTwainSessionWDelegate
            Public DTWAIN_SysDestroy As DTWAIN_SysDestroyDelegate
            Public DTWAIN_SysInitialize As DTWAIN_SysInitializeDelegate
            Public DTWAIN_SysInitializeEx As DTWAIN_SysInitializeExDelegate
            Public DTWAIN_SysInitializeEx2 As DTWAIN_SysInitializeEx2Delegate
            Public DTWAIN_SysInitializeEx2A As DTWAIN_SysInitializeEx2ADelegate
            Public DTWAIN_SysInitializeEx2W As DTWAIN_SysInitializeEx2WDelegate
            Public DTWAIN_SysInitializeExA As DTWAIN_SysInitializeExADelegate
            Public DTWAIN_SysInitializeExW As DTWAIN_SysInitializeExWDelegate
            Public DTWAIN_SysInitializeLib As DTWAIN_SysInitializeLibDelegate
            Public DTWAIN_SysInitializeLibEx As DTWAIN_SysInitializeLibExDelegate
            Public DTWAIN_SysInitializeLibEx2 As DTWAIN_SysInitializeLibEx2Delegate
            Public DTWAIN_SysInitializeLibEx2A As DTWAIN_SysInitializeLibEx2ADelegate
            Public DTWAIN_SysInitializeLibEx2W As DTWAIN_SysInitializeLibEx2WDelegate
            Public DTWAIN_SysInitializeLibExA As DTWAIN_SysInitializeLibExADelegate
            Public DTWAIN_SysInitializeLibExW As DTWAIN_SysInitializeLibExWDelegate
            Public DTWAIN_SysInitializeNoBlocking As DTWAIN_SysInitializeNoBlockingDelegate
            Public DTWAIN_TestGetCap As DTWAIN_TestGetCapDelegate
            Public DTWAIN_UnlockMemory As DTWAIN_UnlockMemoryDelegate
            Public DTWAIN_UnlockMemoryEx As DTWAIN_UnlockMemoryExDelegate
            Public DTWAIN_UseMultipleThreads As DTWAIN_UseMultipleThreadsDelegate
        End Class
    End Class
End Namespace

