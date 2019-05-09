/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2019 Dynarithmic Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3 (AGPL)
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS.

For more information, the license file named LICENSE that is located in the root
directory of the DTWAIN installation covers the restrictions under the AGPL license.
Please read this file before deploying or distributing any application using DTWAIN.
*/

#define _CRT_NON_CONFORMING_SWPRINTFS
#include <windows.h>
#if defined(_WIN32) || defined(_WIN64)
#include <tchar.h>
#endif
//#include <memory.h>
#include <stdio.h>
#include <cstring>
#include <sstream>
#include <iomanip>
#include "dtwinverex.h"
#include "dtwinver.h"

#if !defined(_WIN32) && !defined(_WIN64)
#define _stprintf sprintf
#define _tcscat strcat
#define LPTSTR LPSTR
#endif

using namespace dynarithmic;

CTL_StringType dynarithmic::GetWinVersion()
{
    COSVersion::OS_VERSION_INFO osvi;
    memset(&osvi, 0, sizeof(osvi));
#ifdef _UNICODE
    std::wostringstream sText, sBuf;
#else
    std::ostringstream sText, sBuf;
#endif

    COSVersion os;
    if (os.GetVersion(&osvi))
    {
#ifndef UNDER_CE
        sText << _T("Emulated OS: ");
        switch (osvi.EmulatedPlatform)
        {
        case COSVersion::Dos:
        {
            sText << _T("DOS");
            break;
        }

        case COSVersion::Windows3x:
        {
            sText << _T("Windows");
            break;
        }

        case COSVersion::WindowsCE:
        {
            //This code will never really be executed, but for the same of completeness include it here
            if (os.IsWindowsEmbeddedCompact(&osvi, FALSE))
                sText << _T("Windows Embedded Compact");
            else if (os.IsWindowsCENET(&osvi, FALSE))
                sText << _T("Windows CE .NET");
            else
                sText << _T("Windows CE");
            break;
        }

        case COSVersion::Windows9x:
        {

            if (os.IsWindows95(&osvi, FALSE))
                sBuf << _T("Windows 95");
            else if (os.IsWindows95SP1(&osvi, FALSE))
                sBuf << _T("Windows 95 SP1");
            else if (os.IsWindows95B(&osvi, FALSE))
                sBuf << _T("Windows 95 B [aka OSR2]");
            else if (os.IsWindows95C(&osvi, FALSE))
                sBuf << _T("Windows 95 C [aka OSR2.5]");
            else if (os.IsWindows98(&osvi, FALSE))
                sBuf << _T("Windows 98");
            else if (os.IsWindows98SP1(&osvi, FALSE))
                sBuf << _T("Windows 98 SP1");
            else if (os.IsWindows98SE(&osvi, FALSE))
                sBuf << _T("Windows 98 Second Edition");
            else if (os.IsWindowsME(&osvi, FALSE))
                sBuf << _T("Windows Millenium Edition");
            else
                sBuf << _T("Windows \?\?");
            sText << sBuf.str();
            break;
        }
        case COSVersion::WindowsNT:
        {
            if (os.IsNTPreWin2k(&osvi, FALSE))
            {
                sText << _T("Windows NT");

                if (os.IsNTWorkstation(&osvi, FALSE))
                    sText << _T(" (Workstation)");
                else if (os.IsNTStandAloneServer(&osvi, FALSE))
                    sText << _T(" (Server)");
                else if (os.IsNTPDC(&osvi, FALSE))
                    sText << _T(" (Primary Domain Controller)");
                else if (os.IsNTBDC(&osvi, FALSE))
                    sText << _T(" (Backup Domain Controller)");

                if (os.IsNTDatacenterServer(&osvi, FALSE))
                    sText << _T(", (Datacenter)");
                else if (os.IsNTEnterpriseServer(&osvi, FALSE))
                    sText << _T(", (Enterprise)");
            }
            else if (os.IsWindows2000(&osvi, FALSE))
            {
                if (os.IsProfessional(&osvi))
                    sText << _T("Windows 2000 (Professional)");
                else if (os.IsWindows2000Server(&osvi, FALSE))
                    sText << _T("Windows 2000 Server");
                else if (os.IsWindows2000DomainController(&osvi, FALSE))
                    sText << _T("Windows 2000 (Domain Controller)");
                else
                    sText << _T("Windows 2000");

                if (os.IsWindows2000DatacenterServer(&osvi, FALSE))
                    sText << _T(", (Datacenter)");
                else if (os.IsWindows2000AdvancedServer(&osvi, FALSE))
                    sText << _T(", (Advanced Server)");
            }
            else if (os.IsWindowsXPOrWindowsServer2003(&osvi, FALSE))
            {
                if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows XP (Starter Edition)");
                else if (os.IsPersonal(&osvi))
                    sText << _T("Windows XP (Personal)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows XP (Professional)");
                else if (os.IsWindowsServer2003(&osvi, FALSE))
                    sText << _T("Windows Server 2003");
                else if (os.IsDomainControllerWindowsServer2003(&osvi, FALSE))
                    sText << _T("Windows Server 2003 (Domain Controller)");
                else if (os.IsWindowsServer2003R2(&osvi, FALSE))
                    sText << _T("Windows Server 2003 R2");
                else if (os.IsDomainControllerWindowsServer2003R2(&osvi, FALSE))
                    sText << _T("Windows Server 2003 R2 (Domain Controller)");
                else
                    sText << _T("Windows XP");

                if (os.IsDatacenterWindowsServer2003(&osvi, FALSE) || os.IsDatacenterWindowsServer2003R2(&osvi, FALSE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2003(&osvi, FALSE) || os.IsEnterpriseWindowsServer2003R2(&osvi, FALSE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2003(&osvi, FALSE) || os.IsWebWindowsServer2003R2(&osvi, FALSE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2003(&osvi, FALSE) || os.IsStandardWindowsServer2003R2(&osvi, FALSE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindowsVistaOrWindowsServer2008(&osvi, FALSE))
            {
                if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows Vista (Starter Edition)");
                else if (os.IsHomeBasic(&osvi))
                    sText << _T("Windows Vista (Home Basic)");
                else if (os.IsHomeBasicPremium(&osvi))
                    sText << _T("Windows Vista (Home Premium)");
                else if (os.IsBusiness(&osvi))
                    sText << _T("Windows Vista (Business)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows Vista (Enterprise)");
                else if (os.IsUltimate(&osvi))
                    sText << _T("Windows Vista (Ultimate)");
                else if (os.IsWindowsServer2008(&osvi, FALSE))
                    sText << _T("Windows Server 2008");
                else if (os.IsDomainControllerWindowsServer2008(&osvi, FALSE))
                    sText << _T("Windows Server 2008 (Domain Controller)");
                else
                    sText << _T("Windows Vista");

                if (os.IsDatacenterWindowsServer2008(&osvi, FALSE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2008(&osvi, FALSE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2008(&osvi, FALSE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2008(&osvi, FALSE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows7OrWindowsServer2008R2(&osvi, FALSE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 7 Thin PC");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 7 (Starter Edition)");
                else if (os.IsHomeBasic(&osvi))
                    sText << _T("Windows 7 (Home Basic)");
                else if (os.IsHomeBasicPremium(&osvi))
                    sText << _T("Windows 7 (Home Premium)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 7 (Professional)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 7 (Enterprise)");
                else if (os.IsUltimate(&osvi))
                    sText << _T("Windows 7 (Ultimate)");
                else if (os.IsWindowsServer2008R2(&osvi, FALSE))
                    sText << _T("Windows Server 2008 R2");
                else if (os.IsDomainControllerWindowsServer2008R2(&osvi, FALSE))
                    sText << _T("Windows Server 2008 R2 (Domain Controller)");
                else
                    sText << _T("Windows 7");

                if (os.IsDatacenterWindowsServer2008R2(&osvi, FALSE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2008R2(&osvi, FALSE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2008R2(&osvi, FALSE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2008R2(&osvi, FALSE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows8OrWindowsServer2012(&osvi, FALSE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 8 Thin PC");
                else if (os.IsWindowsRT(&osvi, FALSE))
                    sText << _T("Windows 8 RT");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 8 (Starter Edition)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 8 (Pro)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 8 (Enterprise)");
                else if (os.IsWindowsServer2012(&osvi, FALSE))
                    sText << _T("Windows Server 2012");
                else if (os.IsDomainControllerWindowsServer2012(&osvi, FALSE))
                    sText << _T("Windows Server 2012 (Domain Controller)");
                else
                    sText << _T("Windows 8");

                if (os.IsDatacenterWindowsServer2012(&osvi, FALSE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2012(&osvi, FALSE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2012(&osvi, FALSE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2012(&osvi, FALSE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows8Point1OrWindowsServer2012R2(&osvi, FALSE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 8.1 Thin PC");
                else if (os.IsWindowsRT(&osvi, FALSE))
                    sText << _T("Windows 8.1 RT");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 8.1 (Starter Edition)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 8.1 (Pro)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 8.1 (Enterprise)");
                else if (os.IsWindowsServer2012R2(&osvi, FALSE))
                    sText << _T("Windows Server 2012 R2");
                else if (os.IsDomainControllerWindowsServer2012R2(&osvi, FALSE))
                    sText << _T("Windows Server 2012 R2 (Domain Controller)");
                else
                    sText << _T("Windows 8.1");

                if (os.IsCoreConnected(&osvi))
                    sText << _T(", (with Bing / CoreConnected)");
                if (os.IsWindows8Point1Or2012R2Update(&osvi))
                    sText << _T(", (Update)");

                if (os.IsDatacenterWindowsServer2012R2(&osvi, FALSE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2012R2(&osvi, FALSE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2012R2(&osvi, FALSE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2012R2(&osvi, FALSE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows10OrWindowsServer2016(&osvi, FALSE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 10 Thin PC");
                else if (os.IsWindowsRT(&osvi, FALSE))
                    sText << _T("Windows 10 RT");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 10 (Starter Edition)");
                else if (os.IsCore(&osvi))
                    sText << _T("Windows 10 (Home)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 10 (Pro)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 10 (Enterprise)");
                else if (os.IsNanoServer(&osvi))
                    sText << _T("Windows 10 Nano Server");
                else if (os.IsARM64Server(&osvi))
                    sText << _T("Windows 10 ARM64 Server");
                else if (os.IsWindowsServer2016(&osvi, FALSE))
                    sText << _T("Windows Server 2016");
                else if (os.IsDomainControllerWindowsServer2016(&osvi, FALSE))
                    sText << _T("Windows Server 2016 (Domain Controller)");
                else
                    sText << _T("Windows 10");

                if (os.IsCoreConnected(&osvi))
                    sText << _T(", (with Bing / CoreConnected)");

                if (os.IsDatacenterWindowsServer2016(&osvi, FALSE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2016(&osvi, FALSE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2016(&osvi, FALSE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2016(&osvi, FALSE))
                    sText << _T(", (Standard Edition)");
            }
            break;
        }
        default:
        {
            sBuf << _T("Unknown OS");
            break;
        }
        }

#ifndef UNDER_CE
        switch (osvi.EmulatedProcessorType)
        {
        case COSVersion::IA32_PROCESSOR:
        {
            sText << _T(", (x86-32 Processor)");
            break;
        }
        case COSVersion::MIPS_PROCESSOR:
        {
            sText << _T(", (MIPS Processor)");
            break;
        }
        case COSVersion::ALPHA_PROCESSOR:
        {
            sText << _T(", (Alpha Processor)");
            break;
        }
        case COSVersion::PPC_PROCESSOR:
        {
            sText << _T(", (PPC Processor)");
            break;
        }
        case COSVersion::IA64_PROCESSOR:
        {
            sText << _T(", (IA64 Itanium[2] Processor)");
            break;
        }
        case COSVersion::AMD64_PROCESSOR:
        {
            sText << _T(", (x86-64 Processor)");
            break;
        }
        case COSVersion::ALPHA64_PROCESSOR:
        {
            sText << _T(", (Alpha64 Processor)");
            break;
        }
        case COSVersion::MSIL_PROCESSOR:
        {
            sText << _T(", (MSIL Processor)");
            break;
        }
        case COSVersion::ARM_PROCESSOR:
        {
            sText << _T(", (ARM Processor)");
            break;
        }
        case COSVersion::SHX_PROCESSOR:
        {
            sText << _T(", (SHX Processor)");
            break;
        }
        case COSVersion::UNKNOWN_PROCESSOR: //deliberate fallthrough
        default:
        {
            sText << _T(", (Unknown Processor)");
            break;
        }
        }
#endif
        sBuf << _T(" v") << (int)(osvi.dwEmulatedMajorVersion) << _T(".");
        sText << sBuf.str();
        if (osvi.dwEmulatedMinorVersion % 10)
        {
            if (osvi.dwEmulatedMinorVersion > 9)
                sBuf << std::setfill(_T('0')) << std::setw(2) << static_cast<int>(osvi.dwEmulatedMinorVersion);
            else
                sBuf << std::setfill(_T('0')) << std::setw(1) << static_cast<int>(osvi.dwEmulatedMinorVersion);
        }
        else
            sBuf << std::setfill(_T('0')) << std::setw(1) << osvi.dwEmulatedMinorVersion / 10;
        sText << sBuf.str();
        if (osvi.dwEmulatedBuildNumber)
        {
            sBuf << _T(" Build:") << (int)(osvi.dwEmulatedBuildNumber);
            sText << sBuf.str();
        }
        if (osvi.wEmulatedServicePackMajor)
        {
            if (osvi.wEmulatedServicePackMinor)
            {
                //Handle the special case of NT 4 SP 6a which Dtwinver ver treats as SP 6.1
                if (os.IsNTPreWin2k(&osvi, FALSE) && (osvi.wEmulatedServicePackMajor == 6) && (osvi.wEmulatedServicePackMinor == 1))
                    sBuf << _T(" Service Pack: 6a");
                //Handle the special case of XP SP 1a which Dtwinver ver treats as SP 1.1
                else if (os.IsWindowsXP(&osvi, FALSE) && (osvi.wEmulatedServicePackMajor == 1) && (osvi.wEmulatedServicePackMinor == 1))
                    sBuf << _T(" Service Pack: 1a");
                else
                    sBuf << _T(" Service Pack:") << static_cast<int>(osvi.wEmulatedServicePackMajor) << _T(".") << static_cast<int>(osvi.wEmulatedServicePackMinor);
            }
            else
                sBuf << _T(" Service Pack:") << static_cast<int>(osvi.wEmulatedServicePackMajor);
            sText << sBuf.str();
        }
        else
        {
            if (osvi.wEmulatedServicePackMinor)
                sBuf << _T(" Service Pack:0.") << static_cast<int>(osvi.wEmulatedServicePackMinor);
        }

        sText << _T("\n");
#endif

        //CE does not really have a concept of an emulated OS so
        //lets not bother displaying any info on this on CE
        if (osvi.UnderlyingPlatform == COSVersion::WindowsCE)
        {
            sText.str(_T(""));
            sText << _T("OS: ");
        }
        else
            sText << _T("Underlying OS: ");

        switch (osvi.UnderlyingPlatform)
        {
        case COSVersion::Dos:
        {
            sText << _T("DOS");
            break;
        }
        case COSVersion::Windows3x:
        {
            sText << _T("Windows");
            break;
        }
        case COSVersion::WindowsCE:
        {
            if (os.IsWindowsEmbeddedCompact(&osvi, TRUE))
                sText << _T("Windows Embedded Compact");
            else if (os.IsWindowsCENET(&osvi, TRUE))
                sText << _T("Windows CE .NET");
            else
                sText << _T("Windows CE");
            break;
        }
        case COSVersion::Windows9x:
        {
            if (os.IsWindows95(&osvi, TRUE))
                sBuf << _T("Windows 95");
            else if (os.IsWindows95SP1(&osvi, TRUE))
                sBuf << _T("Windows 95 SP1");
            else if (os.IsWindows95B(&osvi, TRUE))
                sBuf << _T("Windows 95 B [aka OSR2]");
            else if (os.IsWindows95C(&osvi, TRUE))
                sBuf << _T("Windows 95 C [aka OSR2.5]");
            else if (os.IsWindows98(&osvi, TRUE))
                sBuf << _T("Windows 98");
            else if (os.IsWindows98SP1(&osvi, TRUE))
                sBuf << _T("Windows 98 SP1");
            else if (os.IsWindows98SE(&osvi, TRUE))
                sBuf << _T("Windows 98 Second Edition");
            else if (os.IsWindowsME(&osvi, TRUE))
                sBuf << _T("Windows Millenium Edition");
            else
                sBuf << _T("Windows \?\?");
            sText << sBuf.str();
            break;
        }
        case COSVersion::WindowsNT:
        {
            if (os.IsNTPreWin2k(&osvi, TRUE))
            {
                sText << _T("Windows NT");

                if (os.IsNTWorkstation(&osvi, TRUE))
                    sText << _T(" (Workstation)");
                else if (os.IsNTStandAloneServer(&osvi, TRUE))
                    sText << _T(" (Server)");
                else if (os.IsNTPDC(&osvi, TRUE))
                    sText << _T(" (Primary Domain Controller)");
                else if (os.IsNTBDC(&osvi, TRUE))
                    sText << _T(" (Backup Domain Controller)");

                if (os.IsNTDatacenterServer(&osvi, TRUE))
                    sText << _T(", (Datacenter)");
                else if (os.IsNTEnterpriseServer(&osvi, TRUE))
                    sText << _T(", (Enterprise)");
            }
            else if (os.IsWindows2000(&osvi, TRUE))
            {
                if (os.IsProfessional(&osvi))
                    sText << _T("Windows 2000 (Professional)");
                else if (os.IsWindows2000Server(&osvi, TRUE))
                    sText << _T("Windows 2000 Server");
                else if (os.IsWindows2000DomainController(&osvi, TRUE))
                    sText << _T("Windows 2000 (Domain Controller)");
                else
                    sText << _T("Windows 2000");

                if (os.IsWindows2000DatacenterServer(&osvi, TRUE))
                    sText << _T(", (Datacenter)");
                else if (os.IsWindows2000AdvancedServer(&osvi, TRUE))
                    sText << _T(", (Advanced Server)");
            }
            else if (os.IsWindowsXPOrWindowsServer2003(&osvi, TRUE))
            {
                if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows XP (Starter Edition)");
                else if (os.IsPersonal(&osvi))
                    sText << _T("Windows XP (Personal)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows XP (Professional)");
                else if (os.IsWindowsServer2003(&osvi, TRUE))
                    sText << _T("Windows Server 2003");
                else if (os.IsDomainControllerWindowsServer2003(&osvi, TRUE))
                    sText << _T("Windows Server 2003 (Domain Controller)");
                else if (os.IsWindowsServer2003R2(&osvi, TRUE))
                    sText << _T("Windows Server 2003 R2");
                else if (os.IsDomainControllerWindowsServer2003R2(&osvi, TRUE))
                    sText << _T("Windows Server 2003 R2 (Domain Controller)");
                else
                    sText << _T("Windows XP");

                if (os.IsDatacenterWindowsServer2003(&osvi, TRUE) || os.IsDatacenterWindowsServer2003R2(&osvi, TRUE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2003(&osvi, TRUE) || os.IsEnterpriseWindowsServer2003(&osvi, TRUE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2003(&osvi, TRUE) || os.IsWebWindowsServer2003R2(&osvi, TRUE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2003(&osvi, TRUE) || os.IsStandardWindowsServer2003R2(&osvi, TRUE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindowsVistaOrWindowsServer2008(&osvi, TRUE))
            {
                if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows Vista (Starter Edition)");
                else if (os.IsHomeBasic(&osvi))
                    sText << _T("Windows Vista (Home Basic)");
                else if (os.IsHomeBasicPremium(&osvi))
                    sText << _T("Windows Vista (Home Premium)");
                else if (os.IsBusiness(&osvi))
                    sText << _T("Windows Vista (Business)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows Vista (Enterprise)");
                else if (os.IsUltimate(&osvi))
                    sText << _T("Windows Vista (Ultimate)");
                else if (os.IsWindowsServer2008(&osvi, TRUE))
                    sText << _T("Windows Server 2008");
                else if (os.IsDomainControllerWindowsServer2008(&osvi, TRUE))
                    sText << _T("Windows Server 2008 (Domain Controller)");
                else
                    sText << _T("Windows Vista");

                if (os.IsDatacenterWindowsServer2008(&osvi, TRUE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2008(&osvi, TRUE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2008(&osvi, TRUE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2008(&osvi, TRUE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows7OrWindowsServer2008R2(&osvi, TRUE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 7 Thin PC");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 7 (Starter Edition)");
                else if (os.IsHomeBasic(&osvi))
                    sText << _T("Windows 7 (Home Basic)");
                else if (os.IsHomeBasicPremium(&osvi))
                    sText << _T("Windows 7 (Home Premium)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 7 (Professional)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 7 (Enterprise)");
                else if (os.IsUltimate(&osvi))
                    sText << _T("Windows 7 (Ultimate)");
                else if (os.IsWindowsServer2008R2(&osvi, TRUE))
                    sText << _T("Windows Server 2008 R2");
                else if (os.IsDomainControllerWindowsServer2008R2(&osvi, TRUE))
                    sText << _T("Windows Server 2008 R2 (Domain Controller)");
                else
                    sText << _T("Windows 7");

                if (os.IsDatacenterWindowsServer2008R2(&osvi, TRUE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2008R2(&osvi, TRUE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2008R2(&osvi, TRUE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2008R2(&osvi, TRUE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows8OrWindowsServer2012(&osvi, TRUE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 8 Thin PC");
                else if (os.IsWindowsRT(&osvi, TRUE))
                    sText << _T("Windows 8 RT");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 8 (Starter Edition)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 8 (Pro)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 8 (Enterprise)");
                else if (os.IsWindowsServer2012(&osvi, TRUE))
                    sText << _T("Windows Server 2012");
                else if (os.IsDomainControllerWindowsServer2012(&osvi, TRUE))
                    sText << _T("Windows Server 2012 (Domain Controller)");
                else
                    sText << _T("Windows 8");

                if (os.IsDatacenterWindowsServer2012(&osvi, TRUE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2012(&osvi, TRUE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2012(&osvi, TRUE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2012(&osvi, TRUE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows8Point1OrWindowsServer2012R2(&osvi, TRUE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 8.1 Thin PC");
                else if (os.IsWindowsRT(&osvi, TRUE))
                    sText << _T("Windows 8.1 RT");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 8.1 (Starter Edition)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 8.1 (Pro)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 8.1 (Enterprise)");
                else if (os.IsWindowsServer2012R2(&osvi, TRUE))
                    sText << _T("Windows Server 2012 R2");
                else if (os.IsDomainControllerWindowsServer2012R2(&osvi, TRUE))
                    sText << _T("Windows Server 2012 R2 (Domain Controller)");
                else
                    sText << _T("Windows 8.1");

                if (os.IsCoreConnected(&osvi))
                    sText << _T(", (with Bing / CoreConnected)");
                if (os.IsWindows8Point1Or2012R2Update(&osvi))
                    sText << _T(", (Update)");

                if (os.IsDatacenterWindowsServer2012R2(&osvi, TRUE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2012R2(&osvi, TRUE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2012R2(&osvi, TRUE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2012R2(&osvi, TRUE))
                    sText << _T(", (Standard Edition)");
            }
            else if (os.IsWindows10OrWindowsServer2016(&osvi, TRUE))
            {
                if (os.IsThinPC(&osvi))
                    sText << _T("Windows 10 Thin PC");
                else if (os.IsWindowsRT(&osvi, TRUE))
                    sText << _T("Windows 10 RT");
                else if (os.IsStarterEdition(&osvi))
                    sText << _T("Windows 10 (Starter Edition)");
                else if (os.IsCore(&osvi))
                    sText << _T("Windows 10 (Home)");
                else if (os.IsProfessional(&osvi))
                    sText << _T("Windows 10 (Pro)");
                else if (os.IsEnterprise(&osvi))
                    sText << _T("Windows 10 (Enterprise)");
                else if (os.IsNanoServer(&osvi))
                    sText << _T("Windows 10 Nano Server");
                else if (os.IsARM64Server(&osvi))
                    sText << _T("Windows 10 ARM64 Server");
                else if (os.IsWindowsServer2016(&osvi, TRUE))
                    sText << _T("Windows Server 2016");
                else if (os.IsDomainControllerWindowsServer2016(&osvi, TRUE))
                    sText << _T("Windows Server 2016 (Domain Controller)");
                else
                    sText << _T("Windows 10");

                if (os.IsCoreConnected(&osvi))
                    sText << _T(", (with Bing / CoreConnected)");

                if (os.IsDatacenterWindowsServer2016(&osvi, TRUE))
                    sText << _T(", (Datacenter Edition)");
                else if (os.IsEnterpriseWindowsServer2016(&osvi, TRUE))
                    sText << _T(", (Enterprise Edition)");
                else if (os.IsWebWindowsServer2016(&osvi, TRUE))
                    sText << _T(", (Web Edition)");
                else if (os.IsStandardWindowsServer2016(&osvi, TRUE))
                    sText << _T(", (Standard Edition)");
            }
            break;
        }
        default:
        {
            sBuf << _T("Unknown OS");
            sText << sBuf.str();
            break;
        }
        }

#ifndef UNDER_CE
        switch (osvi.UnderlyingProcessorType)
        {
        case COSVersion::IA32_PROCESSOR:
        {
            sText << _T(", (x86-32 Processor)");
            break;
        }
        case COSVersion::MIPS_PROCESSOR:
        {
            sText << _T(", (MIPS Processor)");
            break;
        }
        case COSVersion::ALPHA_PROCESSOR:
        {
            sText << _T(", (Alpha Processor)");
            break;
        }
        case COSVersion::PPC_PROCESSOR:
        {
            sText << _T(", (PPC Processor)");
            break;
        }
        case COSVersion::IA64_PROCESSOR:
        {
            sText << _T(", (IA64 Itanium[2] Processor)");
            break;
        }
        case COSVersion::AMD64_PROCESSOR:
        {
            sText << _T(", (x86-64 Processor)");
            break;
        }
        case COSVersion::ALPHA64_PROCESSOR:
        {
            sText << _T(", (Alpha64 Processor)");
            break;
        }
        case COSVersion::MSIL_PROCESSOR:
        {
            sText << _T(", (MSIL Processor)");
            break;
        }
        case COSVersion::ARM_PROCESSOR:
        {
            sText << _T(", (ARM Processor)");
            break;
        }
        case COSVersion::SHX_PROCESSOR:
        {
            sText << _T(", (SHX Processor)");
            break;
        }
        case COSVersion::UNKNOWN_PROCESSOR: //deliberate fallthrough
        default:
        {
            sText << _T(", (Unknown Processor)");
            break;
        }
        }
#endif
        sBuf << _T(" v") << (int)(osvi.dwUnderlyingMajorVersion);
        sText << sBuf.str();
        if (osvi.dwUnderlyingMinorVersion % 10)
        {
            if (osvi.dwUnderlyingMinorVersion > 9)
                sBuf << std::setfill(_T('0')) << std::setw(2) << static_cast<int>(osvi.dwUnderlyingMinorVersion);
            else
                sBuf << std::setfill(_T('0')) << std::setw(1) << static_cast<int>(osvi.dwUnderlyingMinorVersion);
        }
        else
            sBuf << std::setfill(_T('0')) << std::setw(1) << static_cast<int>(osvi.dwUnderlyingMinorVersion / 10);
        sText << sBuf.str();
        if (osvi.dwUnderlyingBuildNumber)
        {
            sBuf << _T(" Build:") << static_cast<int>(osvi.dwUnderlyingBuildNumber);
            sText << sBuf.str();
        }
        if (osvi.wUnderlyingServicePackMajor)
        {
            if (osvi.wUnderlyingServicePackMinor)
            {
                //Handle the special case of NT 4 SP 6a which Dtwinver treats as SP 6.1
                if (os.IsNTPreWin2k(&osvi, TRUE) && (osvi.wUnderlyingServicePackMajor == 6) && (osvi.wUnderlyingServicePackMinor == 1))
                    sBuf << _T(" Service Pack: 6a");
                //Handle the special case of XP SP 1a which Dtwinver treats as SP 1.1
                else if (os.IsWindowsXP(&osvi, TRUE) && (osvi.wUnderlyingServicePackMajor == 1) && (osvi.wUnderlyingServicePackMinor == 1))
                    sBuf << _T(" Service Pack: 1a");
                else
                    sBuf << _T(" Service Pack:") << static_cast<int>(osvi.wUnderlyingServicePackMajor) << _T(".") << static_cast<int>(osvi.wUnderlyingServicePackMinor);
            }
            else
                sBuf << _T(" Service Pack:") << static_cast<int>(osvi.wUnderlyingServicePackMajor);
            sText << sBuf.str();
        }
        else
        {
            if (osvi.wUnderlyingServicePackMinor)
                sBuf << _T(" Service Pack:0.") << static_cast<int>(osvi.wUnderlyingServicePackMinor);
        }

        if (os.IsEnterpriseStorageServer(&osvi))
            sText << _T(", (Storage Server Enterprise)");
        else if (os.IsExpressStorageServer(&osvi))
            sText << _T(", (Storage Server Express)");
        else if (os.IsStandardStorageServer(&osvi))
            sText << _T(", (Storage Server Standard)");
        else if (os.IsWorkgroupStorageServer(&osvi))
            sText << _T(", (Storage Server Workgroup)");
        else if (os.IsEssentialsStorageServer(&osvi))
            sText << _T(", (Storage Server Essentials)");
        else if (os.IsStorageServer(&osvi))
            sText << _T(", (Storage Server)");

        if (os.IsHomeServerPremiumEdition(&osvi))
            sText << _T(", (Home Server Premium Edition)");
        else if (os.IsHomeServerEdition(&osvi))
            sText << _T(", (Home Server Edition)");

        if (os.IsTerminalServices(&osvi))
            sText << _T(", (Terminal Services)");
        if (os.IsEmbedded(&osvi))
            sText << _T(", (Embedded)");
        if (os.IsTerminalServicesInRemoteAdminMode(&osvi))
            sText << _T(", (Terminal Services in Remote Admin Mode)");
        if (os.Is64Bit(&osvi, TRUE))
            sText << _T(", (64 Bit Edition)");
        if (os.IsMediaCenter(&osvi))
            sText << _T(", (Media Center Edition)");
        if (os.IsTabletPC(&osvi))
            sText << _T(", (Tablet PC Edition)");
        if (os.IsComputeClusterServerEdition(&osvi))
            sText << _T(", (Compute Cluster Edition)");
        if (os.IsServerFoundation(&osvi))
            sText << _T(", (Foundation Edition)");
        if (os.IsMultipointServerPremiumEdition(&osvi))
            sText << _T(", (MultiPoint Premium Edition)");
        else if (os.IsMultiPointServer(&osvi))
            sText << _T(", (MultiPoint Edition)");
        if (os.IsSecurityAppliance(&osvi))
            sText << _T(", (Security Appliance)");
        if (os.IsBackOffice(&osvi))
            sText << _T(", (BackOffice)");
        if (os.IsNEdition(&osvi))
            sText << _T(", (N Edition)");
        if (os.IsEEdition(&osvi))
            sText << _T(", (E Edition)");
        if (os.IsHyperVTools(&osvi))
            sText << _T(", (Hyper-V Tools)");
        if (os.IsHyperVServer(&osvi))
            sText << _T(", (Hyper-V Server)");
        if (os.IsServerCore(&osvi))
            sText << _T(", (Server Core)");
        if (os.IsUniprocessorFree(&osvi))
            sText << _T(", (Uniprocessor Free)");
        if (os.IsUniprocessorChecked(&osvi))
            sText << _T(", (Uniprocessor Checked)");
        if (os.IsMultiprocessorFree(&osvi))
            sText << _T(", (Multiprocessor Free)");
        if (os.IsMultiprocessorChecked(&osvi))
            sText << _T(", (Multiprocessor Checked)");
        if (os.IsEssentialBusinessServerManagement(&osvi))
            sText << _T(", (Windows Essential Business Server Manangement Server)");
        if (os.IsEssentialBusinessServerMessaging(&osvi))
            sText << _T(", (Windows Essential Business Server Messaging Server)");
        if (os.IsEssentialBusinessServerSecurity(&osvi))
            sText << _T(", (Windows Essential Business Server Security Server)");
        if (os.IsClusterServer(&osvi))
            sText << _T(", (Cluster Server)");
        if (os.IsSmallBusinessServer(&osvi))
            sText << _T(", (Small Business Server)");
        if (os.IsSmallBusinessServerPremium(&osvi))
            sText << _T(", (Small Business Server Premium)");
        if (os.IsPreRelease(&osvi))
            sText << _T(", (Prerelease)");
        if (os.IsEvaluation(&osvi))
            sText << _T(", (Evaluation)");
        if (os.IsAutomotive(&osvi))
            sText << _T(", (Automotive)");
        if (os.IsChina(&osvi))
            sText << _T(", (China)");
        if (os.IsSingleLanguage(&osvi))
            sText << _T(", (Single Language)");
        if (os.IsWin32sInstalled(&osvi))
            sText << _T(", (Win32s)");
        if (os.IsEducation(&osvi))
            sText << _T(", (Education)");
        if (os.IsIndustry(&osvi))
            sText << _T(", (Industry)");
        if (os.IsStudent(&osvi))
            sText << _T(", (Student)");
        if (os.IsMobile(&osvi))
            sText << _T(", (Mobile)");
        if (os.IsIoT(&osvi))
            sText << _T(", (IoT Core)");
        if (os.IsCloudHostInfrastructureServer(&osvi))
            sText << _T(", (Cloud Host Infrastructure Server)");
        if (os.IsSEdition(&osvi))
            sText << _T(", (S Edition)");
        if (os.IsCloudStorageServer(&osvi))
            sText << _T(", (Cloud Storage Server)");
        if (os.IsPPIPro(&osvi))
            sText << _T(", (PPI Pro)");
        if (os.IsConnectedCar(&osvi))
            sText << _T(", (Connected Car)");
        if (os.IsHandheld(&osvi))
            sText << _T(", (Handheld)");

        sText << _T("\n");

        //Some extra info for CE
#ifdef UNDER_CE
        if (osvi.UnderlyingPlatform == COSVersion::WindowsCE)
        {
            sText << _T("Model: ");
            sText << osvi.szOEMInfo);
            sText << _T("\nDevice Type: ");
            sText << osvi.szPlatformType);
        }
#endif
    }
    else
        sText << _T("Failed in call to GetOSVersion\n");

    return sText.str();
}
/*  #ifdef _WINDOWS
MessageBox(NULL, sText, _T("Operating System details"), MB_OK);
#elif _WIN32_WCE
MessageBox(NULL, sText, _T("Operating System details"), MB_OK);
#else
printf(sText);
#endif

return 0;
}*/

