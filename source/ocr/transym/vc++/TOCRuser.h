// User constants, Version 3.0.2.0

#define TOCRJOBMSGLENGTH 512		// max length of a job status message

#define TOCRMAXPPM 78741			// max pixels per metre
#define TOCRMINPPM 984				// min pixels per metre

// Setting for JobNo for TOCRSetErrorMode and TOCRGetErrorMode
#define TOCRDEFERRORMODE -1			// set/get the default API error mode (applies
									// when there are no jobs and is applied to new jobs)

// Settings for ErrorMode for TOCRSetErrorMode and TOCRGetErrorMode
#define TOCRERRORMODE_NONE 0		// errors unseen (use return status of API calls)
#define TOCRERRORMODE_MSGBOX 1		// errors will bring up a message box
#define TOCRERRORMODE_LOG 2			// errors are sent to a log file

// Setting for TOCRShutdown
#define TOCRSHUTDOWNALL -1			// stop and shutdown processing for all jobs

// Values returnd by TOCRGetJobStatus JobStatus
#define TOCRJOBSTATUS_ERROR -1		// an error ocurred processing the last job
#define TOCRJOBSTATUS_BUSY 0		// the job is still processing
#define TOCRJOBSTATUS_DONE 1		// the job completed successfully
#define TOCRJOBSTATUS_IDLE 2		// no job has been specified yet

// Settings for TOCRJOBINFO.JobType
#define TOCRJOBTYPE_TIFFFILE 0		// TOCRJOBINFO.InputFile specifies a tiff file
#define TOCRJOBTYPE_DIBFILE 1		// TOCRJOBINFO.InputFile specifies a dib (bmp) file
#define TOCRJOBTYPE_DIBCLIPBOARD 2	// clipboard contains a dib (clipboard format CF_DIB)
#define TOCRJOBTYPE_MMFILEHANDLE  3 // TOCRJOBINFO.PageNo specifies a handle to a memory mapped DIB file

// Settings for TOCRJOBINFO.Orientation
#define TOCRJOBORIENT_AUTO 0		// detect orientation and rotate automatically
#define TOCRJOBORIENT_OFF -1		// don't rotate
#define TOCRJOBORIENT_90 1			// 90 degrees clockwise rotation
#define TOCRJOBORIENT_180 2			// 180 degrees clockwise rotation
#define TOCRJOBORIENT_270 3			// 270 degrees clockwise rotation

// Values returned by TOCRGetJobDBInfo
#define TOCRJOBSLOT_FREE 0			// job slot is free for use
#define TOCRJOBSLOT_OWNEDBYYOU 1	// job slot is in use by your process
#define TOCRJOBSLOT_BLOCKEDBYYOU 2	// blocked by own process (re-initialise)
#define TOCRJOBSLOT_OWNEDBYOTHER -1	// job slot is in use by another process (can't use)
#define TOCRJOBSLOT_BLOCKEDBYOTHER -2// blocked by another process (can't use)

// Values returned in WaitAnyStatus by TOCRWaitForAnyJob
#define TOCRWAIT_OK 0				// JobNo is the job that finished (get and check it's JobStatus)
#define TOCRWAIT_SERVICEABORT 1		// JobNo is the job that failed (re-initialise)
#define TOCRWAIT_CONNECTIONBROKEN 2	// JobNo is the job that failed (re-initialise)
#define TOCRWAIT_FAILED -1			// JobNo not set - check manually
#define TOCRWAIT_NOJOBSFOUND -2		// JobNo not set - no running jobs found

// Settings for Mode for TOCRGetJobResultsEx
#define TOCRGETRESULTS_NORMAL 0		// return results for TOCRRESULTS
#define TOCRGETRESULTS_EXTENDED 1	// return results for TOCRRESULTSEX

// Values returned in ResultsInf by TOCRGetJobResults and TOCRGetJobResultsEx
#define TOCRGETRESULTS_NORESULTS -1	// no results are available

// Values for TOCRConvertFormat InputFormat
#define TOCRCONVERTFORMAT_TIFFFILE TOCRJOBTYPE_TIFFFILE

// Values for TOCRConvertFormat OutputFormat
#define TOCRCONVERTFORMAT_DIBFILE TOCRJOBTYPE_DIBFILE
#define TOCRCONVERTFORMAT_MMFILEHANDLE TOCRJOBTYPE_MMFILEHANDLE

// Values for licence features (returned by TOCRGetLicenceInfoEx)
#define TOCRLICENCE_STANDARD 1		// V1 standard licence (no higher characters)
#define TOCRLICENCE_EURO 2			// V2 (higher characters)
#define TOCRLICENCE_EUROUPGRADE 3	// standard licence upgraded to euro (V1.4->V2)
#define TOCRLICENCE_V3SE 4			// V3SE version 3 standard edition licence (no API)
#define TOCRLICENCE_V3SEUPGRADE 5	// versions 1/2 upgraded to V3 standard edition (no API)
#define TOCRLICENCE_V3PRO 6			// V3PRO version 3 pro licence
#define TOCRLICENCE_V3PROUPGRADE 7	// versions 1/2 upgraded to version 3 pro
#define TOCRLICENCE_V3SEPROUPGRADE 8// version 3 standard edition upgraded to version 3 pro

// Values for TOCRSetConfig and TOCRGetConfig
#define TOCRCONFIG_DEFAULTJOB -1	// default job number (all new jobs)
#define TOCRCONFIG_DLL_ERRORMODE 0	// set the dll ErrorMode
#define TOCRCONFIG_SRV_ERRORMODE 1	// set the service ErrorMode
#define TOCRCONFIG_SRV_THREADPRIORITY 2 // set the service thread priority
#define TOCRCONFIG_DLL_MUTEXWAIT 3	// set the dll mutex wait timeout (ms)
#define TOCRCONFIG_DLL_EVENTWAIT 4	// set the dll event wait timeout (ms)
#define TOCRCONFIG_SRV_MUTEXWAIT 5	// set the service mutex wait timeout (ms)
#define TOCRCONFIG_LOGFILE 6		// set the log file name
