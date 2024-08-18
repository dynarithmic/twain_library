#ifndef DTWAINRAII_H
#define DTWAINRAII_H

#include <windows.h>
#include "dtwain.h"

template <typename T, typename DestroyTraits>
struct DTWAIN_RAII
{
	T m_a;
	DTWAIN_RAII(T a) : m_a(a) {}
	void SetObject(T a) { m_a = a; }
	void Disconnect() { m_a = 0 ; }
	virtual ~DTWAIN_RAII() { DestroyTraits::Destroy(m_a); }
};

struct HANDLE_DestroyTraits
{
	static void Destroy(HANDLE a) 
	{ 
		if (a) 
			GlobalUnlock(a);
	}
};

struct HandleRAII : public DTWAIN_RAII<HANDLE, HANDLE_DestroyTraits>
{
	LPBYTE m_pByte;
	HandleRAII(HANDLE h) : DTWAIN_RAII(h), m_pByte((LPBYTE)GlobalLock(h)) {}
	LPBYTE getData() { return m_pByte; }
};

struct DTWAINArray_DestroyTraits
{
	static void Destroy(DTWAIN_ARRAY a) 
	{ 
		if (a) 
			DTWAIN_ArrayDestroy(a);
	}
};

struct DTWAINArrayPtr_DestroyTraits
{
	static void Destroy(DTWAIN_ARRAY* a) 
	{ 
		if (a && *a) 
			DTWAIN_ArrayDestroy(*a);
	}
};

struct DTWAINFrame_DestroyTraits
{
	static void Destroy(DTWAIN_FRAME f) 
	{ 
		if (f) 
			DTWAIN_FrameDestroy(f);
	}
};

// RAII Class for DTWAIN_ARRAY
typedef DTWAIN_RAII<DTWAIN_ARRAY, DTWAINArray_DestroyTraits> DTWAINArray_RAII;
typedef DTWAIN_RAII<DTWAIN_ARRAY*, DTWAINArrayPtr_DestroyTraits> DTWAINArrayPtr_RAII;
typedef DTWAIN_RAII<DTWAIN_FRAME, DTWAINFrame_DestroyTraits> DTWAINFrame_RAII;

#endif