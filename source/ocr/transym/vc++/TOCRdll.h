#ifndef _TOCRDLL_
#define _TOCRDLL_

// TOCR declares Version 3.0.2.0

#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

typedef unsigned char       BYTE;
typedef signed short		VBBOOL;	// in VB Boolean is a signed short True is -1

typedef struct tagTOCRProcessOptions
{
	long		StructId;
	VBBOOL		InvertWholePage;
	VBBOOL		DeskewOff;
	BYTE		Orientation;
	VBBOOL		NoiseRemoveOff;
	VBBOOL		LineRemoveOff;
	VBBOOL		DeshadeOff;
	VBBOOL		InvertOff;
	VBBOOL		SectioningOn;
	VBBOOL		MergeBreakOff;
	VBBOOL		LineRejectOff;
	VBBOOL		CharacterRejectOff;
	VBBOOL		LexOff;
	VBBOOL		DisableCharacter[256];
} TOCRPROCESSOPTIONS;

typedef struct tagTOCRJobInfo
{
	long		StructId;
	long		JobType;
	char		*InputFile;
	long		PageNo;
	TOCRPROCESSOPTIONS	ProcessOptions;
} TOCRJOBINFO;

typedef struct tagTOCRRESULTSHEADER
{
    long		StructId;
	long		XPixelsPerInch;
	long		YPixelsPerInch;
	long		NumItems;
	float		MeanConfidence;
} TOCRRESULTSHEADER;

typedef struct tagTOCRRESULTSITEM
{
	unsigned short	StructId;
	unsigned short	OCRCha;
	float			Confidence;
	unsigned short	XPos;
	unsigned short	YPos;
	unsigned short	XDim;
	unsigned short	YDim;
} TOCRRESULTSITEM;


typedef struct tagTOCRRESULTS
{
	TOCRRESULTSHEADER	Hdr;
	TOCRRESULTSITEM		Item[1];
} TOCRRESULTS;


typedef struct tagTOCRRESULTSITEMEXALT
{
	unsigned short	Valid;
	unsigned short	OCRCha;
	float			Factor;
} TOCRRESULTSITEMEXALT;

typedef struct tagTOCRRESULTSITEMEX
{
	unsigned short			StructId;
	unsigned short			OCRCha;
	float					Confidence;
	unsigned short			XPos;
	unsigned short			YPos;
	unsigned short			XDim;
	unsigned short			YDim;
	TOCRRESULTSITEMEXALT	Alt[5];
} TOCRRESULTSITEMEX;

typedef struct tagTOCRRESULTSEX
{
	TOCRRESULTSHEADER	Hdr;
	TOCRRESULTSITEMEX	Item[1];
} TOCRRESULTSEX;

#ifndef _WIN32
#define WINAPI
#ifdef __cplusplus
#define EXTERN_C    extern "C"
#else
#define EXTERN_C    extern
#endif
#endif

EXTERN_C long WINAPI TOCRInitialise(long *JobNo);
EXTERN_C long WINAPI TOCRShutdown(long JobNo);
// Superseded by TOCRGetConfig
EXTERN_C long WINAPI TOCRGetErrorMode(long JobNo, long *ErrorMode);
// Superseded by TOCRSetConfig
EXTERN_C long WINAPI TOCRSetErrorMode(long JobNo, long ErrorMode);
EXTERN_C long WINAPI TOCRDoJob(long JobNo, TOCRJOBINFO *JobInfo);
EXTERN_C long WINAPI TOCRWaitForJob(long JobNo, long *JobStatus);
EXTERN_C long WINAPI TOCRWaitForAnyJob(long *WaitAnyStatus, long *JobNo);
EXTERN_C long WINAPI TOCRGetJobDBInfo(long *JobSlotInf);
EXTERN_C long WINAPI TOCRGetJobStatus(long JobNo, long *JobStatus);
EXTERN_C long WINAPI TOCRGetJobStatusEx(long JobNo, long *JobStatus, float *Progress, long *AutoOrientation);
EXTERN_C long WINAPI TOCRGetJobStatusMsg(long JobNo, char *Msg);
EXTERN_C long WINAPI TOCRGetNumPages(long JobNo, char *Filename, long JobType, long *NumPages);
EXTERN_C long WINAPI TOCRGetJobResults(long JobNo, long *ResultsInf, TOCRRESULTS *Results);
EXTERN_C long WINAPI TOCRGetJobResultsEx(long JobNo, long Mode, long *ResultsInf, void *ResultsEx);
EXTERN_C long WINAPI TOCRGetLicenceInfo(long *NumberOfJobSlots, long *Volume, long *Time, long *Remaining);
// Superseded by TOCRConvertFormat
EXTERN_C long WINAPI TOCRConvertTIFFtoDIB(long JobNo, char *InputFilename, char *OutputFilename, long PageNo);
EXTERN_C long WINAPI TOCRRotateMonoBitmap(long *hBmp, long Width, long Height, long Orientation);
EXTERN_C long WINAPI TOCRConvertFormat(long JobNo, void *InputAddr, long InputFormat, void *OutputAddr, long OutputFormat, long PageNo);
EXTERN_C long WINAPI TOCRGetLicenceInfoEx(long JobNo, char *Licence, long *Volume, long *Time, long *Remaining, long *Features);
EXTERN_C long WINAPI TOCRSetConfig(long JobNo, long Parameter, long Value);
EXTERN_C long WINAPI TOCRGetConfig(long JobNo, long Parameter, long *Value);
EXTERN_C long WINAPI TOCRTWAINAcquire(long *NumberOfImages);
EXTERN_C long WINAPI TOCRTWAINGetImages(long *GlobalMemoryDIBs);
EXTERN_C long WINAPI TOCRTWAINSelectDS(void);
EXTERN_C long WINAPI TOCRTWAINShowUI(VBBOOL Show);

#endif // _TOCRDLL_
