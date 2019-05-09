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
#ifndef _WIN32
#include <sys/utsname.h>
#include "ctlobstr.h"
#include "dtwinverex.h"

namespace dynarithmic
{
    CTL_StringType GetVersionInfo()
    {
        utsname test;
        uname(&test);
        CTL_StringType out;
        CTL_StringStreamA strm;
        strm << "System: " << test.sysname <<
            "  Node: " << test.nodename <<
            "  Release: " << test.release <<
            "  Version: " << test.version <<
            "  Machine: " << test.machine <<
            "  Domain Name: " << test.domainname;
        return StringConversion::Convert_Ansi_To_Native(strm.str());
    }
}
#endif