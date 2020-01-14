/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2020 Dynarithmic Software.

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
 #ifdef _WIN32
#include <unordered_map>
#include <string>
#include <algorithm>
#include <cstdio>
#include <io.h>
#include <tchar.h>
#include "transym_ocrinterface.h"
#include "dtwain.h"
#include "ctlobstr.h"
#include "dtwdecl.h"
#include "versioninfo.h"

#ifdef VERSINFO_STANDALONE
using namespace VersionInformation;
#endif

TOCRSDK::TOCRSDK()
        : TOCRInitialise(0),
        TOCRShutdown        (0),
        TOCRGetErrorMode    (0),
        TOCRSetErrorMode    (0),
        TOCRDoJob           (0),
        TOCRWaitForJob      (0),
        TOCRWaitForAnyJob   (0),
        TOCRGetJobDBInfo    (0),
        TOCRGetJobStatus    (0),
        TOCRGetJobStatusEx  (0),
        TOCRGetJobStatusMsg (0),
        TOCRGetNumPages     (0),
        TOCRGetJobResults   (0),
        TOCRGetJobResultsEx (0),
        TOCRGetLicenceInfo  (0),
        TOCRConvertTIFFtoDIB(0),
        TOCRRotateMonoBitmap(0),
        TOCRConvertFormat   (0),
        TOCRGetLicenceInfoEx(0),
        m_hMod (0)
{
}

HMODULE TOCRSDK::InitTOCR()
{
    if ( m_hMod )
        return m_hMod;
    m_hMod = LoadLibrary(_T("TOCRDLL.dll"));

    if ( !m_hMod )
    {
        return NULL;
    }

    TOCRInitialise       =  (TOCRINITIALIZEFUNC      )GetProcAddress(m_hMod, "TOCRInitialise");
    TOCRShutdown         =  (TOCRSHUTDOWNFUNC        )GetProcAddress(m_hMod, "TOCRShutdown");
    TOCRGetErrorMode     =  (TOCRGETERRORMODEFUNC    )GetProcAddress(m_hMod, "TOCRGetErrorMode");
    TOCRSetErrorMode     =  (TOCRSETERRORMODEFUNC    )GetProcAddress(m_hMod, "TOCRSetErrorMode");
    TOCRDoJob            =  (TOCRDOJOBFUNC           )GetProcAddress(m_hMod, "TOCRDoJob");
    TOCRWaitForJob       =  (TOCRWAITFORJOBFUNC      )GetProcAddress(m_hMod, "TOCRWaitForJob");
    TOCRWaitForAnyJob    =  (TOCRWAITFORANYJOBFUNC   )GetProcAddress(m_hMod, "TOCRWaitForAnyJob");
    TOCRGetJobDBInfo     =  (TOCRGETJOBDBINFOFUNC    )GetProcAddress(m_hMod, "TOCRGetJobDBInfo");
    TOCRGetJobStatus     =  (TOCRGETJOBSTATUSFUNC    )GetProcAddress(m_hMod, "TOCRGetJobStatus");
    TOCRGetJobStatusEx   =  (TOCRGETJOBSTATUSEXFUNC  )GetProcAddress(m_hMod, "TOCRGetJobStatusEx");
    TOCRGetJobStatusMsg  =  (TOCRGETJOBSTATUSMSGFUNC )GetProcAddress(m_hMod, "TOCRGetJobStatusMsg");
    TOCRGetNumPages      =  (TOCRGETNUMPAGESFUNC     )GetProcAddress(m_hMod, "TOCRGetNumPages");
    TOCRGetJobResults    =  (TOCRGETJOBRESULTSFUNC   )GetProcAddress(m_hMod, "TOCRGetJobResults");
    TOCRGetJobResultsEx  =  (TOCRGETJOBRESULTSEXFUNC )GetProcAddress(m_hMod, "TOCRGetJobResultsEx");
    TOCRGetLicenceInfo   =  (TOCRGETLICENCEINFOFUNC  )GetProcAddress(m_hMod, "TOCRGetLicenceInfo");
    TOCRConvertTIFFtoDIB =  (TOCRCONVERTTIFFTODIBFUNC)GetProcAddress(m_hMod, "TOCRConvertTIFFtoDIB");
    TOCRRotateMonoBitmap =  (TOCRROTATEMONOBITMAPFUNC)GetProcAddress(m_hMod, "TOCRRotateMonoBitmap");
    TOCRConvertFormat    =  (TOCRCONVERTFORMATFUNC   )GetProcAddress(m_hMod, "TOCRConvertFormat");
    TOCRGetLicenceInfoEx =  (TOCRGETLICENCEINFOEXFUNC)GetProcAddress(m_hMod, "TOCRGetLicenceInfoEx");

    if (!TOCRInitialise ||
        !TOCRShutdown       ||
        !TOCRGetErrorMode   ||
        !TOCRSetErrorMode   ||
        !TOCRDoJob          ||
        !TOCRWaitForJob     ||
        !TOCRWaitForAnyJob  ||
        !TOCRGetJobDBInfo   ||
        !TOCRGetJobStatus   ||
        !TOCRGetJobStatusEx ||
        !TOCRGetJobStatusMsg ||
        !TOCRGetNumPages     ||
        !TOCRGetJobResults   ||
        !TOCRGetJobResultsEx ||
        !TOCRGetLicenceInfo  ||
        !TOCRConvertTIFFtoDIB ||
        !TOCRRotateMonoBitmap ||
        !TOCRConvertFormat    ||
        !TOCRGetLicenceInfoEx )
    {
        FreeLibrary(m_hMod);
        m_hMod = NULL;
        return NULL;
    }
    return m_hMod;
}

TOCRSDK::~TOCRSDK()
{
    if (m_hMod)
        FreeLibrary(m_hMod);
}

#define INIT_TOCR_ERROR_CODE(x) m_ErrorCode[x]=_T(#x);
///////////////////////////////////////////////////////////////////
TransymOCR::TransymOCR()
{
    INIT_TOCR_ERROR_CODE(TOCRERR_ILLEGALJOBNO)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILLOCKDB)
    INIT_TOCR_ERROR_CODE(TOCRERR_NOFREEJOBSLOTS)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILSTARTSERVICE)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILINITSERVICE)
    INIT_TOCR_ERROR_CODE(TOCRERR_JOBSLOTNOTINIT)
    INIT_TOCR_ERROR_CODE(TOCRERR_JOBSLOTINUSE)
    INIT_TOCR_ERROR_CODE(TOCRERR_SERVICEABORT)
    INIT_TOCR_ERROR_CODE(TOCRERR_CONNECTIONBROKEN)
    INIT_TOCR_ERROR_CODE(TOCRERR_INVALIDSTRUCTID)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETVERSION)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILLICENCEINF)
    INIT_TOCR_ERROR_CODE(TOCRERR_LICENCEEXCEEDED)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETJOBSTATUS1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETJOBSTATUS2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETJOBSTATUS3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILCONVERT)
    INIT_TOCR_ERROR_CODE(TOCRERR_INCORRECTLICENCE)
    INIT_TOCR_ERROR_CODE(TOCRERR_MISMATCH)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB5)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB6)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB7)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB8)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB9)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDOJOB10)
    INIT_TOCR_ERROR_CODE(TOCRERR_UNKNOWNJOBTYPE1)
    INIT_TOCR_ERROR_CODE(TOCRERR_JOBNOTSTARTED1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILDUPHANDLE)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETJOBSTATUSMSG1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETJOBSTATUSMSG2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETNUMPAGES1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETNUMPAGES2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETNUMPAGES3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETNUMPAGES4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETNUMPAGES5)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETRESULTS1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETRESULTS2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETRESULTS3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETRESULTS4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM100)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM101)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILENOTSPECIFIED)
    INIT_TOCR_ERROR_CODE(TOCRERR_INPUTNOTSPECIFIED)
    INIT_TOCR_ERROR_CODE(TOCRERR_OUTPUTNOTSPECIFIED)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILROTATEBITMAP)
    INIT_TOCR_ERROR_CODE(TOCRERR_INVALIDSERVICESTART)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILSERVICEINIT)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILLICENCE1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILSERVICESTART)
    INIT_TOCR_ERROR_CODE(TOCRERR_UNKNOWNCMD)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADCOMMAND)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADOPTIONS)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITEJOBSTATUS1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITEJOBSTATUS2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITETHREADH)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADJOBINFO1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADJOBINFO2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADJOBINFO3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITEPROGRESS)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITEJOBSTATUSMSG)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITERESULTSSIZE)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITERESULTS)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITEAUTOORIENT)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILSETCONFIG)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILGETCONFIG)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILLICENCE2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILLICENCE3)
    INIT_TOCR_ERROR_CODE(TOCRERR_TOOMANYCOLUMNS)
    INIT_TOCR_ERROR_CODE(TOCRERR_TOOMANYROWS)
    INIT_TOCR_ERROR_CODE(TOCRERR_EXCEEDEDMAXZONE)
    INIT_TOCR_ERROR_CODE(TOCRERR_NSTACKTOOSMALL)
    INIT_TOCR_ERROR_CODE(TOCRERR_ALGOERR1)
    INIT_TOCR_ERROR_CODE(TOCRERR_ALGOERR2)
    INIT_TOCR_ERROR_CODE(TOCRERR_EXCEEDEDMAXCP)
    INIT_TOCR_ERROR_CODE(TOCRERR_CANTFINDPAGE)
    INIT_TOCR_ERROR_CODE(TOCRERR_UNSUPPORTEDIMAGETYPE)
    INIT_TOCR_ERROR_CODE(TOCRERR_IMAGETOOWIDE)
    INIT_TOCR_ERROR_CODE(TOCRERR_IMAGETOOLONG)
    INIT_TOCR_ERROR_CODE(TOCRERR_UNKNOWNJOBTYPE2)
    INIT_TOCR_ERROR_CODE(TOCRERR_TOOWIDETOROT)
    INIT_TOCR_ERROR_CODE(TOCRERR_TOOLONGTOROT)
    INIT_TOCR_ERROR_CODE(TOCRERR_INVALIDPAGENO)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADJOBTYPENUMBYTES)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILENAME)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILSENDNUMPAGES)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILOPENCLIP)
    INIT_TOCR_ERROR_CODE(TOCRERR_NODIBONCLIP)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADDIBCLIP)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILLOCKDIBCLIP)
    INIT_TOCR_ERROR_CODE(TOCRERR_UNKOWNDIBFORMAT)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADDIB)
    INIT_TOCR_ERROR_CODE(TOCRERR_NOXYPPM)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILCREATEDIB)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITEDIBCLIP)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEMDIB)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILLOCKMEMDIB)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILCREATEFILE)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILOPENFILE1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILOPENFILE2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILOPENFILE3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILOPENFILE4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILE1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILE2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILFINDDATA1)
    INIT_TOCR_ERROR_CODE(TOCRERR_TIFFERROR1)
    INIT_TOCR_ERROR_CODE(TOCRERR_TIFFERROR2)
    INIT_TOCR_ERROR_CODE(TOCRERR_TIFFERROR3)
    INIT_TOCR_ERROR_CODE(TOCRERR_TIFFERROR4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADDIBHANDLE)
    INIT_TOCR_ERROR_CODE(TOCRERR_PAGETOOBIG)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILENAME1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILENAME2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILENAME3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILENAME4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFILENAME5)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFORMAT1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADFORMAT2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM5)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM6)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM7)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM8)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM9)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEM10)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILWRITEMMFH)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILREADACK)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILFILEMAP)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILFILEVIEW)
    INIT_TOCR_ERROR_CODE(TOCRERR_BUFFEROVERFLOW1)
    INIT_TOCR_ERROR_CODE(TOCRERR_MAPOVERFLOW)
    INIT_TOCR_ERROR_CODE(TOCRERR_REBREAKNEXTCALL)
    INIT_TOCR_ERROR_CODE(TOCRERR_REBREAKNEXTDATA)
    INIT_TOCR_ERROR_CODE(TOCRERR_REBREAKEXACTCALL)
    INIT_TOCR_ERROR_CODE(TOCRERR_MAXZCANOVERFLOW1)
    INIT_TOCR_ERROR_CODE(TOCRERR_MAXZCANOVERFLOW2)
    INIT_TOCR_ERROR_CODE(TOCRERR_BUFFEROVERFLOW2)
    INIT_TOCR_ERROR_CODE(TOCRERR_NUMKCOVERFLOW)
    INIT_TOCR_ERROR_CODE(TOCRERR_BUFFEROVERFLOW3)
    INIT_TOCR_ERROR_CODE(TOCRERR_BUFFEROVERFLOW4)
    INIT_TOCR_ERROR_CODE(TOCRERR_SEEDERROR)
    INIT_TOCR_ERROR_CODE(TOCRERR_FCZYREF)
    INIT_TOCR_ERROR_CODE(TOCRERR_MAXTEXTLINES1)
    INIT_TOCR_ERROR_CODE(TOCRERR_LINEINDEX)
    INIT_TOCR_ERROR_CODE(TOCRERR_MAXFCZSONLINE)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOC1)
    INIT_TOCR_ERROR_CODE(TOCRERR_MERGEBREAK)
    INIT_TOCR_ERROR_CODE(TOCRERR_DKERNPRANGE1)
    INIT_TOCR_ERROR_CODE(TOCRERR_DKERNPRANGE2)
    INIT_TOCR_ERROR_CODE(TOCRERR_BUFFEROVERFLOW5)
    INIT_TOCR_ERROR_CODE(TOCRERR_BUFFEROVERFLOW6)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEOPEN1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEOPEN2)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEOPEN3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEREAD1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEREAD2)
    INIT_TOCR_ERROR_CODE(TOCRERR_SPWIDZERO)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEMLEX1)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILALLOCMEMLEX2)
    INIT_TOCR_ERROR_CODE(TOCRERR_BADOBWIDTH)
    INIT_TOCR_ERROR_CODE(TOCRERR_BADROTATION)
    INIT_TOCR_ERROR_CODE(TOCRERR_REJHIDMEMALLOC)
    INIT_TOCR_ERROR_CODE(TOCRERR_UIDA)
    INIT_TOCR_ERROR_CODE(TOCRERR_UIDB)
    INIT_TOCR_ERROR_CODE(TOCRERR_ZEROUID)
    INIT_TOCR_ERROR_CODE(TOCRERR_CERTAINTYDBNOTINIT)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOCINDEX)
    INIT_TOCR_ERROR_CODE(TOCRERR_CERTAINTYDB_INIT)
    INIT_TOCR_ERROR_CODE(TOCRERR_CERTAINTYDB_DELETE)
    INIT_TOCR_ERROR_CODE(TOCRERR_CERTAINTYDB_INSERT1)                        ;
    INIT_TOCR_ERROR_CODE(TOCRERR_CERTAINTYDB_INSERT2)
    INIT_TOCR_ERROR_CODE(TOCRERR_OPENXORNEAREST)
    INIT_TOCR_ERROR_CODE(TOCRERR_XORNEAREST)
    INIT_TOCR_ERROR_CODE(TOCRERR_OPENSETTINGS)
    INIT_TOCR_ERROR_CODE(TOCRERR_READSETTINGS1)
    INIT_TOCR_ERROR_CODE(TOCRERR_READSETTINGS2)
    INIT_TOCR_ERROR_CODE(TOCRERR_BADSETTINGS)
    INIT_TOCR_ERROR_CODE(TOCRERR_WRITESETTINGS)
    INIT_TOCR_ERROR_CODE(TOCRERR_MAXSCOREDIFF)
    INIT_TOCR_ERROR_CODE(TOCRERR_YDIMREFZERO1)
    INIT_TOCR_ERROR_CODE(TOCRERR_YDIMREFZERO2)
    INIT_TOCR_ERROR_CODE(TOCRERR_YDIMREFZERO3)
    INIT_TOCR_ERROR_CODE(TOCRERR_ASMFILEOPEN)
    INIT_TOCR_ERROR_CODE(TOCRERR_ASMFILEREAD)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOCASM)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMREALLOCASM)
    INIT_TOCR_ERROR_CODE(TOCRERR_SDBFILEOPEN)
    INIT_TOCR_ERROR_CODE(TOCRERR_SDBFILEREAD)
    INIT_TOCR_ERROR_CODE(TOCRERR_SDBFILEBAD1)
    INIT_TOCR_ERROR_CODE(TOCRERR_SDBFILEBAD2)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOCSDB)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL1)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL2)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL3)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL4)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL5)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL6)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL7)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL8)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL9)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL10)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL11)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL12)
    INIT_TOCR_ERROR_CODE(TOCRERR_DEVEL13)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEOPEN4)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEOPEN5)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEOPEN6)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEREAD3)
    INIT_TOCR_ERROR_CODE(TOCRERR_FILEREAD4)
    INIT_TOCR_ERROR_CODE(TOCRERR_ZOOMGTOOBIG)
    INIT_TOCR_ERROR_CODE(TOCRERR_ZOOMGOUTOFRANGE)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOCRESULTS)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOCHEAP)
    INIT_TOCR_ERROR_CODE(TOCRERR_HEAPNOTINITIALISED)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMLIMITHEAP)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMREALLOCHEAP)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOCFCZBM)
    INIT_TOCR_ERROR_CODE(TOCRERR_FCZBMOVERLAP)
    INIT_TOCR_ERROR_CODE(TOCRERR_FCZBMLOCATION)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMREALLOCFCZBM)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMALLOCFCHBM)
    INIT_TOCR_ERROR_CODE(TOCRERR_MEMREALLOCFCHBM)
    INIT_TOCR_ERROR_CODE(TOCERR_TWAINPARTIALACQUIRE)
    INIT_TOCR_ERROR_CODE(TOCERR_TWAINFAILEDACQUIRE)
    INIT_TOCR_ERROR_CODE(TOCERR_TWAINNOIMAGES)
    INIT_TOCR_ERROR_CODE(TOCERR_TWAINSELECTDSFAILED)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILSETTHREADPRIORITY)
    INIT_TOCR_ERROR_CODE(TOCRERR_FAILSETSRVERRORMODE)

    // Set up available caps
    SetAvailableCaps();
    SetPDFFileTypes(OCRPDFInfo::PDFINFO_BW, DTWAIN_TIFFNONE, DTWAIN_PT_BW, 1);
    m_hMod = m_SDK.InitTOCR();
    if ( m_hMod )
    {
        LONG status = m_SDK.TOCRInitialise(&m_JobHandle);
        if ( status != TOCR_OK )
        {
            FreeLibrary(m_hMod);
            m_hMod = NULL;
        }
        SetActivated(true);
        FileTypeArray fArray;
        fArray.push_back(DTWAIN_TIFFG4);
        fArray.push_back(DTWAIN_TIFFPACKBITS);
        fArray.push_back(DTWAIN_TIFFNONE);
        fArray.push_back(DTWAIN_TIFFG3);
        fArray.push_back(DTWAIN_TIFFLZW);
        SetAllFileTypes(fArray);
    }
}

void TransymOCR::SetAvailableCaps()
{
    #define ALLOPS_GET  (DTWAIN_CO_GET | DTWAIN_CO_GETDEFAULT |DTWAIN_CO_GETCURRENT )
    #define ALLOPS_SET  (DTWAIN_CO_SET | DTWAIN_CO_RESET)
    #define ALLOPS  (ALLOPS_GET | ALLOPS_SET)

    // Set LONG Array caps, along with mapping information
    {
        struct ArrayCapsLong
        {
            LONG CapName;
            LONG CapOps;
            LONG CurValue;
            LONG DefValue;
            LONG CapContainers[4];
            LONG RangeMin;
            LONG RangeMax;
            std::string CommaDelValues;
            LongCapMap* pMappedValues;
            bool isSingleValue;
        };

        const ArrayCapsLong capsLongInfo[] = {
            { DTWAIN_OCRCV_IMAGEFILEFORMAT,ALLOPS, DTWAIN_TIFFG4, DTWAIN_TIFFG4,
                {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },
                0,0, "800:0,801:0,600:0,700:0,500:0,100:0", NULL, false },

            { DTWAIN_OCRCV_NATIVEFILEFORMAT,ALLOPS, DTWAIN_TIFFG4, DTWAIN_TIFFG4,
                {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,
                "800:0,801:0,600:0,700:0,500:0,100:0,"
                "900:0,901:0,902:0,903:0,906:0,7000:0", NULL, false },

            { DTWAIN_OCRCV_MPNATIVEFILEFORMAT,ALLOPS_GET, DTWAIN_TIFFG4MULTI, DTWAIN_TIFFG4MULTI,
                {DTWAIN_CONTARRAY, DTWAIN_CONTARRAY, DTWAIN_CONTARRAY, DTWAIN_CONTARRAY},0,0,
                "900:0,901:0,902:0,903:0,906:0,7000:0", NULL, false },

            { DTWAIN_OCRCV_ORIENTATION,ALLOPS, DTWAIN_OCRORIENT_OFF, DTWAIN_OCRORIENT_OFF,
                {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,
                "0:-1,1:0,2:1,3:2,4:3",&m_OrientationMap, false},

            { DTWAIN_OCRCV_ERRORREPORTMODE, ALLOPS, DTWAIN_OCRERROR_MODENONE,
                DTWAIN_OCRERROR_MODENONE,
                {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,
                "0:0,1:0", NULL , false},

            { DTWAIN_OCRCV_SUPPORTEDCAPS, ALLOPS_GET, 0, 0,
                {DTWAIN_CONTARRAY, DTWAIN_CONTARRAY, DTWAIN_CONTARRAY, DTWAIN_CONTARRAY},0,0,
                "0x1000:0,"
                "0x1001:0,"
                "0x1002:0,"
                "0x1003:0,"
                "0x1004:0,"
                "0x1005:0,"
                "0x1006:0,"
                "0x1007:0,"
                "0x1008:0,"
                "0x1009:0,"
                "0x1010:0,"
                "0x1011:0,"
                "0x1012:0,"
                "0x1013:0,"
                "0x1014:0,"
                "0x1015:0,"
                "0x1016:0,"
                "0x1017:0,"
                , NULL, false },

            { DTWAIN_OCRCV_DESKEW, ALLOPS, false, false, {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"1:1,0:0", NULL, false },
            { DTWAIN_OCRCV_NOISEREMOVE, ALLOPS, false, false, {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0, "1:1,0:0", NULL, false },
            { DTWAIN_OCRCV_LINEREMOVE, ALLOPS, false, false, {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE }, 0,0,"1:1,0:0", NULL, false },
            { DTWAIN_OCRCV_DESHADE, ALLOPS, false, false,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"1:1,0:0", NULL, false },
            { DTWAIN_OCRCV_LINEREJECT, ALLOPS, false, false,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"1:1,0:0", NULL, false },
            { DTWAIN_OCRCV_CHARACTERREJECT, ALLOPS, false, false,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"1:1,0:0", NULL, false },
            { DTWAIN_OCRCV_PIXELTYPE, ALLOPS, false, false,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"0:0", NULL, false },
            { DTWAIN_OCRCV_BITDEPTH, ALLOPS, false, false,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"1:1", NULL, false },
            { DTWAIN_OCRCV_RETURNCHARINFO, ALLOPS, TRUE, TRUE,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"1:1,0:0", NULL, false },
            { DTWAIN_OCRCV_DISABLECHARACTERS, ALLOPS, 0,0,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,255,"0:0", NULL, true },
            { DTWAIN_OCRCV_REMOVECONTROLCHARS, ALLOPS, false, false,  {DTWAIN_CONTARRAY, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE },0,0,"1:1,0:0", NULL, false }
        };

        const int numCaps = sizeof(capsLongInfo) / sizeof(capsLongInfo[0]);

        std::vector<LONG> fArray;
        for (int i = 0; i < numCaps; ++i )
        {
            fArray.clear();

            // CV_IMAGEFILEFORMAT
            OCRCapInfo CapInfo(capsLongInfo[i].CapName, DTWAIN_ARRAYLONG, ALLOPS, capsLongInfo[i].isSingleValue);
            CapInfo.SetCapContainerTypes(capsLongInfo[i].CapContainers[0],
                                         capsLongInfo[i].CapContainers[1],
                                         capsLongInfo[i].CapContainers[2],
                                         capsLongInfo[i].CapContainers[3]);
            CTL_StringArray strVals;
            CTL_StringArray strSplit;

            // Tokenize the comma delimited string
            StringWrapperA::Tokenize(capsLongInfo[i].CommaDelValues, "," , strVals);

            size_t numVals = strVals.size();

            // For each value in the string, split into two numbers
            char *p;
            for ( size_t j = 0; j < numVals; ++j )
            {
                // Get the split of the two values
                StringWrapperA::Tokenize(strVals[j],":", strSplit);
                LONG val1, val2;
                val1 = strtol(strSplit[0].c_str(), &p, 0);
                val2 = strtol(strSplit[1].c_str(), &p, 0);

                // First number is the DTWAIN cap value
                // Second number is the OCR engine value
                fArray.push_back(val1);

                // If a mapping exists for this cap, add the key/data to map
                if ( capsLongInfo[i].pMappedValues )
                    capsLongInfo[i].pMappedValues->insert(std::make_pair(val1, val2));
            }

            // Set the cap data info
            CapInfo.SetCapDataInfo(capsLongInfo[i].CurValue,
                                   capsLongInfo[i].DefValue,
                                   fArray,
                                   fArray,
                                   capsLongInfo[i].RangeMin,
                                   capsLongInfo[i].RangeMax);

            // Load the information
            AddCapValue(capsLongInfo[i].CapName,CapInfo);
        }

        // Do the bit depth map
        std::vector<LONG> BitDepths;

        // Only 1 bit depth for B/W images.
        BitDepths.push_back(1);
        m_BitDepths[DTWAIN_PT_BW] = BitDepths;
        m_BitDepthsCurrent[DTWAIN_PT_BW] = 1;
        m_BitDepthsDefault[DTWAIN_PT_BW] = 1;
    }

    // Set String single cap values
    {
        struct ArrayCapsString
        {
            LONG CapName;
            LONG CapOps;
            std::string CurValue;
            std::string DefValue;
            std::string CommaDelValues;
            StringCapMap* pMappedValues;
        };

        const ArrayCapsString capsStringInfo[] = {
            { DTWAIN_OCRCV_ERRORREPORTFILE, ALLOPS, "OCRERRORS.LOG", "OCRERRORS.LOG", "OCRERRORS.LOG:OCRERRORS.LOG", NULL },
        };

        const int numCaps = sizeof(capsStringInfo) / sizeof(capsStringInfo[0]);

        std::vector<std::string> fArray;
        for (int i = 0; i < numCaps; ++i )
        {
            fArray.clear();

            OCRCapInfo CapInfo(capsStringInfo[i].CapName, DTWAIN_ARRAYSTRING, ALLOPS, false);
            CapInfo.SetCapContainerTypes(DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE,
                                         DTWAIN_CONTONEVALUE, DTWAIN_CONTONEVALUE);

            CTL_StringArray strVals;
            CTL_StringArray strSplit;

            // Tokenize the comma delimited string
            StringWrapperA::Tokenize(capsStringInfo[i].CommaDelValues,"," , strVals);

            size_t numVals = strVals.size();

            // For each value in the string, split into two numbers
            for ( size_t j = 0; j < numVals; ++j )
            {
                // Get the split of the two values
                StringWrapperA::Tokenize(strVals[j],":", strSplit);
                std::string val1, val2;
                val1 = strSplit[0];
                val2 = strSplit[1];

                // First number is the DTWAIN cap value
                // Second number is the OCR engine value
                fArray.push_back(val1);

                // If a mapping exists for this cap, add the key/data to map
                if ( capsStringInfo[i].pMappedValues )
                    capsStringInfo[i].pMappedValues->insert(std::make_pair(val1, val2));
            }

            // Set the cap data info
            CapInfo.SetCapDataInfo(capsStringInfo[i].CurValue,
                                    capsStringInfo[i].DefValue,
                                    fArray,
                                    fArray);

            // Load the information
            AddCapValue(capsStringInfo[i].CapName,CapInfo);
        }
    }
}

TransymOCR::~TransymOCR()
{
    if ( m_hMod )
    {
        FreeLibrary(m_hMod);
        m_hMod = NULL;
    }
}

bool TransymOCR::IsInitialized() const
{
    return m_hMod?true:false;
}

bool TransymOCR::SetOptions(OCRJobOptions& options)
{
    TOCRJOBINFO *pInfo = reinterpret_cast<TOCRJOBINFO*>(options.pOtherOptions);
    memcpy(&m_JobInfo, pInfo, sizeof(TOCRJOBINFO));
    return true;
}

LONG TransymOCR::StartOCR(const CTL_StringType& filename)
{
    TOCRJOBINFO JobInfo;
    memset(&JobInfo, 0, sizeof(JobInfo));
    JobInfo.StructId = 0;
    JobInfo.PageNo = GetCurrentPageNumber();  // This will change
    JobInfo.JobType = TOCRJOBTYPE_TIFFFILE;

    // Get the file type to use
    OCRLongArrayValues vals;
    bool bRet = GetCapValues(DTWAIN_OCRCV_IMAGEFILEFORMAT, DTWAIN_CAPGETCURRENT, vals);
    if ( bRet )
    {
        if ( vals[0] == DTWAIN_BMP )
            JobInfo.JobType = TOCRJOBTYPE_DIBFILE;
    }

    // Get the orientation
    bRet = GetCapValues(DTWAIN_OCRCV_ORIENTATION,DTWAIN_CAPGETCURRENT, vals);
    if ( bRet )
        JobInfo.ProcessOptions.Orientation = (BYTE)m_OrientationMap[vals[0]];

    // Get the characters to disable
    bRet = GetCapValues(DTWAIN_OCRCV_REMOVECONTROLCHARS, DTWAIN_CAPGETCURRENT, vals);
    if ( bRet )
    {
        if ( vals[0] == 0 )
            memset(JobInfo.ProcessOptions.DisableCharacter, 0, sizeof(JobInfo.ProcessOptions.DisableCharacter));
        else
        {
            for (int i = 0; i < 32; ++i )
                JobInfo.ProcessOptions.DisableCharacter[i] = 1;
        }
    }

    // Get the error report mode
    bRet = GetCapValues(DTWAIN_OCRCV_ERRORREPORTMODE, DTWAIN_CAPGETCURRENT, vals);

    if ( bRet )
    {
        m_SDK.TOCRSetErrorMode( m_JobHandle, vals[0]);
        if ( vals[0] == DTWAIN_OCRERROR_WRITEFILE )
        {
            // Errors will be reported to the file specified by the ERRORREPORTFILE cap
            std::vector<std::string> valStr;
            bRet = GetCapValues(DTWAIN_OCRCV_ERRORREPORTFILE, DTWAIN_CAPGETCURRENT, valStr);

            // Turn off the TOCR message box if reporting to a file.
            m_SDK.TOCRSetErrorMode(m_JobHandle, TOCRERRORMODE_NONE);
            // Open the file for errors (TO BE DONE)
        }
    }

    // Get the character reporting mode
    bRet = GetCapValues(DTWAIN_OCRCV_RETURNCHARINFO, DTWAIN_CAPGETCURRENT, vals);
    if ( bRet )
        SetBaseOption(OCROPTION_GETINFO, vals[0]?true:false);

    static const LONG OffCaps[]= {
                     DTWAIN_OCRCV_DESKEW,
                     DTWAIN_OCRCV_NOISEREMOVE,
                     DTWAIN_OCRCV_LINEREMOVE,
                     DTWAIN_OCRCV_DESHADE,
                     DTWAIN_OCRCV_LINEREJECT,
                     DTWAIN_OCRCV_CHARACTERREJECT
                    };

    static LONG OffCapSize = sizeof(OffCaps) / sizeof(OffCaps[0]);

    VBBOOL* boolFuncs[] = {
                    &JobInfo.ProcessOptions.DeskewOff,
                    &JobInfo.ProcessOptions.NoiseRemoveOff,
                    &JobInfo.ProcessOptions.LineRemoveOff,
                    &JobInfo.ProcessOptions.DeshadeOff,
                    &JobInfo.ProcessOptions.LineRejectOff,
                    &JobInfo.ProcessOptions.CharacterRejectOff
                    };

    for (LONG i = 0; i < OffCapSize; ++i )
    {
        bRet = GetCapValues(OffCaps[i],DTWAIN_CAPGETCURRENT, vals);
        if ( bRet )
            *boolFuncs[i] = !(VBBOOL)vals[0];
    }

    char    InputFile[MAX_PATH];    // Input file name
    strcpy(InputFile, StringConversion::Convert_Native_To_Ansi(filename).c_str());
    JobInfo.InputFile = InputFile;

    // Now set the options
    OCRJobOptions ocrOptions;
    ocrOptions.pOtherOptions = &JobInfo;
    SetOptions(ocrOptions);

    LONG status = m_SDK.TOCRDoJob(m_JobHandle, &m_JobInfo);
    if ( status == TOCR_OK )
    {
        LONG JobStatus;
        status = m_SDK.TOCRWaitForJob(m_JobHandle, &JobStatus);
        if ( status == TOCR_OK )
        {
            if ( JobStatus == TOCRJOBSTATUS_DONE )
            {
                ProcessTOCRJob();
            }
        }
    }
    return status;
}

LONG TransymOCR::ProcessTOCRJob()
{
    LONG ResultsInf;
    bool bSaveOCRCharInfo = GetBaseOption(OCROPTION_GETINFO);

    OCRCharacterInfo cInfo(this, GetCurrentPageNumber());

    // Find out how much space to allocate for results
    LONG status = m_SDK.TOCRGetJobResults(m_JobHandle, &ResultsInf, 0);
    if ( status == TOCR_OK )
    {
        if ( ResultsInf > 0 )
        {
            m_sOCRResults = "";
            // Allocate memory for results
            std::vector<char> theResults(ResultsInf);

            // Retrieve the results
            status = m_SDK.TOCRGetJobResults(m_JobHandle, &ResultsInf,
                reinterpret_cast<TOCRRESULTS*>(&theResults[0]));

            TOCRRESULTS* TOCRResults = reinterpret_cast<TOCRRESULTS*>(&theResults[0]);
            if ( status == TOCR_OK )
            {
                m_sOCRResults.reserve(TOCRResults->Hdr.NumItems + 100);
                // Display the results
                for (int ItemNo = 0; ItemNo < TOCRResults->Hdr.NumItems;  ++ItemNo)
                {
                    m_sOCRResults += static_cast<char>(TOCRResults->Item[ItemNo].OCRCha);
                    if ( bSaveOCRCharInfo )
                    {
                        cInfo.AddCharacterInfo(TOCRResults->Item[ItemNo].Confidence,
                                               TOCRResults->Item[ItemNo].XPos,
                                               TOCRResults->Item[ItemNo].YPos,
                                               TOCRResults->Item[ItemNo].OCRCha,
                                               TOCRResults->Item[ItemNo].XDim,
                                               TOCRResults->Item[ItemNo].YDim);
                    }
                }

                SetPageTextMap(GetCurrentPageNumber(), StringConversion::Convert_Ansi_To_Native(m_sOCRResults));
                AddCharacterInfo(GetCurrentPageNumber(), cInfo);
            }
        }
    }
    else
    {
        // Print the error message that caused the job to fail (this
        // should be the same as the popup message box)
        /*                        status = TOCRGetJobStatusMsg(va, msg);
        if ( status != TOCR_OK ) goto ABORT;
        printf(">GetJobStatusMsg %s\n", msg);*/
    }
    return status;
}

bool TransymOCR::SetOCRVersionIdentity()
{
    OCRVersionIdentity theIdentity;
    HMODULE hInst = ::GetModuleHandle(_T("TOCRDLL.DLL"));
    if ( hInst )
    {
        try
        {
            VersionInfoA ver( hInst );
            CTL_StringArray aTokens;
            StringWrapperA::Tokenize(ver.getFileVersionDotted(),".", aTokens);
            if ( aTokens.size() >= 2 )
            {
                theIdentity.Version.MajorNum = atoi(aTokens[0].c_str());
                theIdentity.Version.MinorNum = atoi(aTokens[1].c_str());
            }
            else
            {
                theIdentity.Version.MajorNum = 2;
                theIdentity.Version.MinorNum = 0;
            }
        }
        catch(...)
        {
            theIdentity.Version.MajorNum = 2;
            theIdentity.Version.MinorNum = 0;
        }
    }
    theIdentity.Version.Language = DTWAIN_LANGUSAENGLISH;
    theIdentity.Version.Country = DTWAIN_CNTYUSA;
    theIdentity.Version.Info = StringConversion::Convert_Native_To_Ansi(GetOCRVersionInfo());

    theIdentity.Manufacturer = _T("Transym Computer Services Ltd.");
    theIdentity.ProductFamily = _T("TOCR");
    theIdentity.ProductName = _T("Transym TOCR");

    OCREngine::SetOCRVersionIdentity(theIdentity);
    return true;
}

CTL_StringType TransymOCR::GetOCRVersionInfo()
{
    static std::unordered_map<int, CTL_StringType> LicenseMap;
    if (LicenseMap.empty())
    {
        LicenseMap[TOCRLICENCE_EUROUPGRADE] = _T("Standard License upgraded to European License");
        LicenseMap[TOCRLICENCE_EURO] = _T("European License");
        LicenseMap[TOCRLICENCE_STANDARD] = _T("Standard License");
    }

    if (m_SDK.TOCRGetLicenceInfoEx)
    {
        LONG retvalue = TOCRERR_FAILLICENCEINF;
        char LicenseString[30];
        LONG VolumeLicense;
        LONG LicenseTime;
        LONG RemainingTime;
        LONG TOCRLicenseValue;
        retvalue = m_SDK.TOCRGetLicenceInfoEx(m_JobHandle, LicenseString, &VolumeLicense,
                        &LicenseTime, &RemainingTime, &TOCRLicenseValue);

        if (retvalue == TOCR_OK)
        {
            HMODULE hInst = ::GetModuleHandle(_T("TOCRDLL.DLL"));
            try
            {
                VersionInfoA ver(hInst);
                std::string sVer = "2";
                sVer = ver.getProductVersion();
                std::ostringstream strm;
                strm << "Transym OCR Engine - Version " << sVer <<
                        "\nLicense String: " << LicenseString <<
                        "\nVolume: " << VolumeLicense <<
                        "\nLicense Time: " << LicenseTime <<
                        "\nRemaining License Time: " << RemainingTime <<
                        "\nLicense Feature: " << StringConversion::Convert_Native_To_Ansi(LicenseMap[TOCRLicenseValue]);
                return StringConversion::Convert_Ansi_To_Native(strm.str());
            }
            catch(...)
            {
                return _T("");
            }
        }
    }
    return _T("TranSym OCR Initialization error!");
}

bool TransymOCR::ProcessGetCapValues(LONG nOCRCap, LONG CapType, OCRLongArrayValues& vals)
{
    OCRCapInfo& CapInfo = m_AllCapValues[nOCRCap];
    OCRLongArrayValues pt;
    vals.resize(0);
    switch(nOCRCap)
    {
        case DTWAIN_OCRCV_IMAGEFILEFORMAT:
        case DTWAIN_OCRCV_DESKEW:
        case DTWAIN_OCRCV_ORIENTATION:
        case DTWAIN_OCRCV_NOISEREMOVE:
        case DTWAIN_OCRCV_LINEREMOVE:
        case DTWAIN_OCRCV_DESHADE:
        case DTWAIN_OCRCV_LINEREJECT:
        case DTWAIN_OCRCV_CHARACTERREJECT:
        case DTWAIN_OCRCV_ERRORREPORTMODE:
        case DTWAIN_OCRCV_PIXELTYPE:
        case DTWAIN_OCRCV_RETURNCHARINFO:
        case DTWAIN_OCRCV_NATIVEFILEFORMAT:
        case DTWAIN_OCRCV_DISABLECHARACTERS:
        case DTWAIN_OCRCV_REMOVECONTROLCHARS:
        {
            switch(CapType)
            {
                case DTWAIN_CAPGET:
                    vals = CapInfo.longData.defaultdata_array;
                break;

                case DTWAIN_CAPGETCURRENT:
                    vals.push_back(CapInfo.longData.currentdata);
                break;

                case DTWAIN_CAPGETDEFAULT:
                    vals.push_back(CapInfo.longData.defaultdata);
                break;
            }
        }
        break;

        case DTWAIN_OCRCV_MPNATIVEFILEFORMAT:
        case DTWAIN_OCRCV_SUPPORTEDCAPS:
        {
            // Always return the entire list of values
            vals = CapInfo.longData.defaultdata_array;
        }
        break;

        case DTWAIN_OCRCV_BITDEPTH:
        {
            // Get the current pixel type
            case DTWAIN_CAPGET:
                if ( ProcessGetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPGETCURRENT, pt) )
                    vals = m_BitDepths[pt[0]];
            break;

            case DTWAIN_CAPGETCURRENT:
                if ( ProcessGetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPGETCURRENT, pt) )
                    vals.push_back(m_BitDepthsCurrent[pt[0]]);
            break;

            case DTWAIN_CAPGETDEFAULT:
                if ( ProcessGetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPGETCURRENT, pt) )
                    vals.push_back(m_BitDepthsDefault[pt[0]]);
            break;
        }
        break;
    }
    return true;
}


bool TransymOCR::ProcessGetCapValues(LONG nOCRCap, LONG CapType, OCRStringArrayValues& vals)
{
    OCRCapInfo& CapInfo = m_AllCapValues[nOCRCap];
    vals.resize(0);
    switch(nOCRCap)
    {
        case DTWAIN_OCRCV_ERRORREPORTFILE:
        {
            switch(CapType)
            {
                case DTWAIN_CAPGET:
                    vals = CapInfo.stringData.defaultdata_array;
                break;

                case DTWAIN_CAPGETCURRENT:
                    vals.push_back(CapInfo.stringData.currentdata);
                break;

                case DTWAIN_CAPGETDEFAULT:
                    vals.push_back(CapInfo.stringData.defaultdata);
                break;
            }
        }
        break;
    }
    return true;
}

bool TransymOCR::ProcessSetCapValues(LONG nOCRCap, LONG CapType, const OCRLongArrayValues& vals)
{
    OCRCapInfo& CapInfo = m_AllCapValues[nOCRCap];
    OCRLongArrayValues pt;
    bool bRetval = true;
    switch(nOCRCap)
    {
        case DTWAIN_OCRCV_IMAGEFILEFORMAT:
        case DTWAIN_OCRCV_DESKEW:
        case DTWAIN_OCRCV_ORIENTATION:
        case DTWAIN_OCRCV_NOISEREMOVE:
        case DTWAIN_OCRCV_LINEREMOVE:
        case DTWAIN_OCRCV_DESHADE:
        case DTWAIN_OCRCV_LINEREJECT:
        case DTWAIN_OCRCV_CHARACTERREJECT:
        case DTWAIN_OCRCV_ERRORREPORTMODE:
        case DTWAIN_OCRCV_PIXELTYPE:
        case DTWAIN_OCRCV_RETURNCHARINFO:
        case DTWAIN_OCRCV_NATIVEFILEFORMAT:
        case DTWAIN_OCRCV_DISABLECHARACTERS:
        case DTWAIN_OCRCV_REMOVECONTROLCHARS:
        {
            if ( CapType == DTWAIN_CAPSET )
            {
                if ( vals.size() > 0)
                {
                    if ( CapInfo.IsSingleValue() )
                        CapInfo.longData.currentdata = vals[0];
                    else
                    // check if type is supported
                    if ( std::find(CapInfo.longData.currentdata_array.begin(),
                                   CapInfo.longData.currentdata_array.end(),vals[0]) !=
                                            CapInfo.longData.currentdata_array.end())
                        CapInfo.longData.currentdata = vals[0];
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            if (CapType == DTWAIN_CAPRESET)
                CapInfo.longData.currentdata = CapInfo.longData.defaultdata;
        }
        break;

        case DTWAIN_OCRCV_BITDEPTH:
        {
            switch (CapType)
            {
                // Get the current pixel type
                case DTWAIN_CAPSET:
                    if ( ProcessGetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPGETCURRENT, pt) )
                    {
                        // Test if the value exists in the map.
                        std::vector<LONG> BitDepths = m_BitDepths[pt[0]];
                        if ( std::find(BitDepths.begin(), BitDepths.end(), vals[0]) !=
                                BitDepths.end())
                        {
                            m_BitDepthsCurrent[pt[0]] = vals[0];
                        }
                        else
                            return false;
                    }
                break;

                case DTWAIN_CAPRESET:
                    if ( ProcessGetCapValues(DTWAIN_OCRCV_PIXELTYPE, DTWAIN_CAPGETCURRENT, pt) )
                    {
                        m_BitDepthsCurrent[pt[0]] = m_BitDepthsDefault[pt[0]];
                    }
                break;
            }
        }
        break;

    }

    // now actually turn on the options if the option is one that actually changes
    // the state of the engine
    switch (nOCRCap)
    {
        case DTWAIN_OCRCV_ERRORREPORTMODE:
        {
            if ( vals[0] != DTWAIN_OCRERROR_WRITEFILE )
                m_SDK.TOCRSetErrorMode( m_JobHandle, vals[0]);
            else
            {
                m_SDK.TOCRSetErrorMode(m_JobHandle, TOCRERRORMODE_NONE);
                // open the file here??
            }
        }
        break;
    }
    return true;
}

bool TransymOCR::ProcessSetCapValues(LONG nOCRCap, LONG CapType,
                                     const OCRStringArrayValues& vals)
{
    OCRCapInfo& CapInfo = m_AllCapValues[nOCRCap];
    switch(nOCRCap)
    {
        case DTWAIN_OCRCV_ERRORREPORTFILE:
        {
            if ( CapType == DTWAIN_CAPSET )
            {
                if ( vals.size() > 0)
                {
                    CapInfo.stringData.currentdata = vals[0];
                    return true;
                }
                return false;
            }
            else
            if (CapType == DTWAIN_CAPRESET)
                CapInfo.stringData.currentdata = CapInfo.stringData.defaultdata;
        }
        break;
    }
    return true;
}

bool TransymOCR::IsReturnCodeOk(LONG returnCode)
{
    return returnCode == TOCR_OK;
}

CTL_StringType TransymOCR::GetErrorString(LONG returnCode)
{
    if ( m_ErrorCode.find(returnCode) != m_ErrorCode.end())
        return m_ErrorCode[returnCode];
    return _T("Unknown TOCR Error");
}

int TransymOCR::GetNumPagesInFile(const CTL_StringType& szFileName, int& errCode)
{
    LONG bRet;
    LONG nPages = -1;
    LONG oldErrorMode;
    bRet = m_SDK.TOCRGetErrorMode( m_JobHandle, &oldErrorMode );
    if ( bRet != TOCR_OK)
    {
        errCode = bRet;
        return 0;
    }

    // Set the mode to not show the message box
    m_SDK.TOCRSetErrorMode(m_JobHandle, TOCRERRORMODE_NONE);
    int fExists = (_taccess(szFileName.c_str(), 0 ) == 0);
    bRet = m_SDK.TOCRGetNumPages(m_JobHandle, (char *)szFileName.c_str(),
                                TOCRJOBTYPE_TIFFFILE, &nPages);

    // Set the old error mode back
    m_SDK.TOCRSetErrorMode(m_JobHandle, oldErrorMode);

    // check if current file type is supported file type
    if ( bRet != TOCR_OK )
    {
        if ( fExists )
        {
            FILE *infile = fopen(StringConversion::Convert_Native_To_Ansi(szFileName).c_str(), "rb");
            if ( infile )
            {
                char buffer[2];
                // check first two bytes
                fread(buffer, 2, 1, infile);
                fclose( infile );
                if ( buffer[0] == 'B' && buffer[1] == 'M' )
                {
                    errCode = TOCR_OK;
                    return 1;
                }
            }
        }
    }
    errCode = bRet;
    return nPages;
}

LONG TransymOCR::StartupOCREngine()
{
    LONG status = m_SDK.TOCRInitialise(&m_JobHandle);
    if ( status == TOCR_OK )
        SetActivated(true);
    return status;
}

bool TransymOCR::ShutdownOCR(int& status)
{
    status = TOCR_OK;
    if ( m_hMod )
    {
        status = m_SDK.TOCRShutdown(m_JobHandle);
        if ( status == TOCR_OK )
        {
            SetActivated(false);
            return true;
        }
    }
    else
    {
        status = TOCRERR_ILLEGALJOBNO;
        SetActivated(false);
    }
    return false;
}
#endif
