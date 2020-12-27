/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
#ifndef TRANSYM_OCRINTERFACE_H
#define TRANSYM_OCRINTERFACE_H
#include <unordered_map>
#include "ocrinterface.h"
#include "../ocr/transym/vc++/tocrdll.h"
#include "../ocr/transym/vc++/TOCRuser.h"
#include "../ocr/transym/vc++/TOCRerrs.h"
#ifndef _WIN32
#define __stdcall
#endif
using namespace dynarithmic;

typedef LONG (__stdcall * TOCRINITIALIZEFUNC)(LONG*);
typedef LONG (__stdcall * TOCRSHUTDOWNFUNC)(LONG);
typedef LONG (__stdcall * TOCRGETERRORMODEFUNC)(long JobNo, long *ErrorMode);
typedef LONG (__stdcall * TOCRSETERRORMODEFUNC)(long JobNo, long ErrorMode);
typedef LONG (__stdcall * TOCRDOJOBFUNC)(long JobNo, TOCRJOBINFO *JobInfo);
typedef LONG (__stdcall * TOCRWAITFORJOBFUNC)(long JobNo, long *JobStatus);
typedef LONG (__stdcall * TOCRWAITFORANYJOBFUNC)(long *WaitAnyStatus, long *JobNo);
typedef LONG (__stdcall * TOCRGETJOBDBINFOFUNC)(long *JobSlotInf);
typedef LONG (__stdcall * TOCRGETJOBSTATUSFUNC)(long JobNo, long *JobStatus);
typedef LONG (__stdcall * TOCRGETJOBSTATUSEXFUNC)(long JobNo, long *JobStatus, float *Progress, long *AutoOrientation);
typedef LONG (__stdcall * TOCRGETJOBSTATUSMSGFUNC)(long JobNo, char *Msg);
typedef LONG (__stdcall * TOCRGETNUMPAGESFUNC)(long JobNo, char *Filename, long JobType, long *NumPages);
typedef LONG (__stdcall * TOCRGETJOBRESULTSFUNC)(long JobNo, long *ResultsInf, TOCRRESULTS *Results);
typedef LONG (__stdcall * TOCRGETJOBRESULTSEXFUNC)(long JobNo, long Mode, long *ResultsInf, void *ResultsEx);
typedef LONG (__stdcall * TOCRGETLICENCEINFOFUNC)(long *NumberOfJobSlots, long *Volume, long *Time, long *Remaining);
typedef LONG (__stdcall * TOCRCONVERTTIFFTODIBFUNC)(long JobNo, char *InputFilename, char *OutputFilename, long PageNo);
typedef LONG (__stdcall * TOCRROTATEMONOBITMAPFUNC)(long *hBmp, long Width, long Height, long Orientation);
typedef LONG (__stdcall * TOCRCONVERTFORMATFUNC)(long JobNo, void *InputAddr, long InputFormat, void *OutputAddr, long OutputFormat, long PageNo);
typedef LONG (__stdcall * TOCRGETLICENCEINFOEXFUNC)(long JobNo, char *Licence, long *Volume, long *Time, long *Remaining, long *Features);

struct TOCRSDK
{
    TOCRINITIALIZEFUNC         TOCRInitialise      ;
    TOCRSHUTDOWNFUNC           TOCRShutdown        ;
    TOCRGETERRORMODEFUNC       TOCRGetErrorMode    ;
    TOCRSETERRORMODEFUNC       TOCRSetErrorMode    ;
    TOCRDOJOBFUNC              TOCRDoJob           ;
    TOCRWAITFORJOBFUNC         TOCRWaitForJob      ;
    TOCRWAITFORANYJOBFUNC      TOCRWaitForAnyJob   ;
    TOCRGETJOBDBINFOFUNC       TOCRGetJobDBInfo    ;
    TOCRGETJOBSTATUSFUNC       TOCRGetJobStatus    ;
    TOCRGETJOBSTATUSEXFUNC     TOCRGetJobStatusEx  ;
    TOCRGETJOBSTATUSMSGFUNC    TOCRGetJobStatusMsg ;
    TOCRGETNUMPAGESFUNC        TOCRGetNumPages     ;
    TOCRGETJOBRESULTSFUNC      TOCRGetJobResults   ;
    TOCRGETJOBRESULTSEXFUNC    TOCRGetJobResultsEx ;
    TOCRGETLICENCEINFOFUNC     TOCRGetLicenceInfo  ;
    TOCRCONVERTTIFFTODIBFUNC   TOCRConvertTIFFtoDIB;
    TOCRROTATEMONOBITMAPFUNC   TOCRRotateMonoBitmap;
    TOCRCONVERTFORMATFUNC      TOCRConvertFormat   ;
    TOCRGETLICENCEINFOEXFUNC   TOCRGetLicenceInfoEx;

    TOCRSDK();
    ~TOCRSDK();
    HMODULE InitTOCR();
    bool IsInitialized() const { return m_hMod!=NULL; }
    HMODULE GetModuleHandle() const { return m_hMod; }
    HMODULE m_hMod;
};

typedef std::unordered_map<LONG,LONG> LongCapMap;
typedef std::unordered_map<std::string,std::string> StringCapMap;
typedef std::unordered_map<LONG, CTL_StringType> TOCRErrorCodeMap;
typedef std::unordered_map<LONG, std::vector<LONG> > TOCRBitDepthMap;

class TransymOCR : public OCREngine
{
#ifdef _WIN32
public:
    TransymOCR();
    ~TransymOCR();
    virtual bool IsInitialized() const;
    virtual bool SetOptions(OCRJobOptions& options);
    virtual LONG StartOCR(const CTL_StringType& filename);
    virtual bool SetFileType()  { return true; }
    virtual CTL_StringType GetOCRVersionInfo();
    virtual bool SetOCRVersionIdentity();
    virtual bool IsReturnCodeOk(LONG returnCode);
    virtual CTL_StringType GetErrorString(LONG returnCode);
    virtual int GetNumPagesInFile(const CTL_StringType& szFileName, int& errCode);
    virtual bool ShutdownOCR(int& status);
    virtual LONG StartupOCREngine();

protected:
    virtual bool ProcessGetCapValues(LONG nOCRCap, LONG CapType, OCRLongArrayValues& vals);
    virtual bool ProcessGetCapValues(LONG nOCRCap, LONG CapType, OCRStringArrayValues& vals);
    virtual bool ProcessSetCapValues(LONG, LONG, const OCRLongArrayValues&);
    virtual bool ProcessSetCapValues(LONG nOCRCap, LONG CapType, const OCRStringArrayValues& vals);

private:
    void SetAvailableCaps();
    TOCRSDK m_SDK;
    TOCRJOBINFO m_JobInfo;
    UINT m_nJobRetrieveType;
    HMODULE m_hMod;
    LONG m_JobHandle;
    std::string m_sOCRResults;
    LONG ProcessTOCRJob();
    LongCapMap m_OrientationMap;
    TOCRRESULTS m_JobResults;
    TOCRErrorCodeMap m_ErrorCode;
    TOCRBitDepthMap m_BitDepths;
    LongCapMap m_BitDepthsCurrent;
    LongCapMap m_BitDepthsDefault;
#endif
};
#endif
