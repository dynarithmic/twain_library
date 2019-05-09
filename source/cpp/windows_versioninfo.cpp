#ifdef _WIN32
#include "ctlobstr.h"
#include "ctliface.h"
#include "versioninfo.h"

namespace dynarithmic
{
    CTL_StringType GetVersionInfo()
    {
        VersionInfo vInfo(CTL_TwainDLLHandle::s_DLLInstance);
        CTL_StringStreamType strm;
        vInfo.printit(strm, _T("\r\n"));
        return strm.str();
    }
}
#endif