/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2019 Dynarithmic Software.

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
#define MC_NO_CPP
#include <cstring>
#include <stdlib.h>
#include <algorithm>
#include <cwchar>
#include <sstream>
#include <iterator>
#include "ctltwsrc.h"
#include "ctltr009.h"
#include "ctltwmgr.h"
#include "ctldib.h"
#include "dtwain.h"
#include "ctltmpl3.h"
#include "imagexferfilewriter.h"
#include "ctltr038.h"
#include "enumeratorfuncs.h"
#include "ctlfileutils.h"
#include "tiff.h"
#include "boost/lexical_cast.hpp"
using namespace std;
using namespace dynarithmic;

static CTL_StringType GetPageFileName(const CTL_StringType &strBase,
                                  int nCurImage,
                                  bool bUseLongNames );

static CTL_StringType CreateFileNameFromNumber(const CTL_StringType& sFileName, int num, int nDigits);
static int GetInitialFileNumber(const CTL_StringType& sFileName, size_t &nDigits);

//////////////////////////////////////////////////////////////////////////////
CTL_TwainSource::CTL_TwainSource(CTL_ITwainSource *pSource) : m_pSource(pSource){}

void CTL_TwainSource::SetEqual(CTL_TwainSource & SObject)
{ m_pSource = SObject.m_pSource; }

CTL_ITwainSource* CTL_ITwainSource::Create( CTL_ITwainSession* pSession,
                                            LPCTSTR lpszProduct/*=NULL*/ )
{
    CTL_ITwainSource *pSource = new CTL_ITwainSource( pSession, lpszProduct );
    return pSource;
}

void CTL_ITwainSource::Destroy( CTL_ITwainSource *pSource )
{ delete pSource; }

void CTL_ITwainSource::SetUIOpen(bool bSet)
{ m_bUIOpened = bSet; }

void CTL_ITwainSource::SetActive(bool bSet)
{ m_bActive = bSet; }

bool CTL_ITwainSource::IsSourceCompliant( CTL_EnumTwainVersion TVersion, CTL_TwainCapArray &rArray )
{
    CTL_TwainCapArray Array;
    rArray.clear();
    switch ( TVersion )
    {
        case CTL_TwainVersion15:
            Array.push_back( DTWAIN_CV_CAPXFERCOUNT );
            Array.push_back( DTWAIN_CV_ICAPCOMPRESSION );
            Array.push_back( DTWAIN_CV_ICAPPIXELTYPE );
            Array.push_back( DTWAIN_CV_ICAPUNITS );
            Array.push_back( DTWAIN_CV_ICAPXFERMECH );
        break;

        case CTL_TwainVersion16:
        case CTL_TwainVersion17:
        case CTL_TwainVersion18:
            Array.push_back( DTWAIN_CV_CAPXFERCOUNT );
            Array.push_back( DTWAIN_CV_ICAPCOMPRESSION );
            Array.push_back( DTWAIN_CV_ICAPPIXELTYPE );
            Array.push_back( DTWAIN_CV_ICAPUNITS );
            Array.push_back( DTWAIN_CV_ICAPXFERMECH );
            Array.push_back( DTWAIN_CV_CAPSUPPORTEDCAPS );
            Array.push_back( DTWAIN_CV_CAPUICONTROLLABLE );
            Array.push_back( DTWAIN_CV_ICAPPLANARCHUNKY );
            Array.push_back( DTWAIN_CV_ICAPPHYSICALHEIGHT );
            Array.push_back( DTWAIN_CV_ICAPPHYSICALWIDTH );
            Array.push_back( DTWAIN_CV_ICAPBITDEPTH );
            Array.push_back( DTWAIN_CV_ICAPBITORDER );
            Array.push_back( DTWAIN_CV_ICAPXRESOLUTION );
            Array.push_back( DTWAIN_CV_ICAPYRESOLUTION );
            Array.push_back(DTWAIN_CV_ICAPPIXELFLAVOR);
        break;
        default:
            break;
    }

    int nMask;
    TW_UINT16 Cap;
    bool bIsCompliant = true;
    int  nValue;
    for ( CTL_IntArray::size_type i = 0; i < Array.size(); i++ )
    {
        nValue = 0;
        Cap = Array[i];
        nMask = CTL_TwainAppMgr::GetCapMaskFromCap( Cap );

        if ( CTL_TwainAppMgr::IsCapMaskOn( Cap, (CTL_EnumGetType)CTL_CapMaskGET ) )
        {
            bIsCompliant = CTL_TwainAppMgr::IsCapabilitySupported( this, (int) Cap, CTL_GetTypeGET );
            if ( bIsCompliant )
                nValue |= CTL_CapMaskGET;
        }

        if ( CTL_TwainAppMgr::IsCapMaskOn( Cap, (CTL_EnumGetType)CTL_CapMaskGETCURRENT ) )
        {
            bIsCompliant = CTL_TwainAppMgr::IsCapabilitySupported( this, (int) Cap, CTL_GetTypeGETCURRENT );
            if ( bIsCompliant )
                nValue |= CTL_CapMaskGETCURRENT;
        }

        if ( CTL_TwainAppMgr::IsCapMaskOn( Cap, (CTL_EnumGetType)CTL_CapMaskGETDEFAULT ) )
        {
            bIsCompliant = CTL_TwainAppMgr::IsCapabilitySupported( this, (int) Cap, CTL_GetTypeGETDEFAULT );
            if ( bIsCompliant )
                nValue |= CTL_CapMaskGETDEFAULT;
        }

        if ( CTL_TwainAppMgr::IsCapMaskOn( Cap, (CTL_EnumSetType)CTL_CapMaskSET ) )
        {
            nValue |= CTL_CapMaskSET;
        }

        if ( CTL_TwainAppMgr::IsCapMaskOn( Cap, (CTL_EnumSetType)CTL_CapMaskRESET ) )
        {
            nValue |= CTL_CapMaskRESET;
        }

        if ( nValue != nMask )
            rArray.push_back( Cap );
    }

    if ( !rArray.empty() )
        return false;
    return true;
}


bool CTL_ITwainSource::IsActive() const
{
    return m_bActive;
}

CTL_ITwainSource::CTL_ITwainSource( CTL_ITwainSession *pSession, LPCTSTR lpszProduct )
                               :
    m_pUserPtr(nullptr),
    CapCacheInfo(),
    m_bDSMVersion2 ( false ),
    m_bIsOpened(false),
    m_SourceId {},
    m_pSession(pSession),
    m_bIsSelected(false),
    m_bUIOpened(false),
    m_bPromptPending ( false ),
    m_bActive(true),
    m_hOutWnd(0),
    m_DibArray(std::make_shared<CTL_TwainDibArray>(CTL_TwainDibArray())),
    m_bUseFeeder(true),
    m_bDibAutoDelete(false),
    m_AcquireType(TWAINAcquireType_Native),
    m_nImageNum(0),
    m_nCurDibPage(0),
    m_bDeleteOnScan(false),
    m_bUIOnAcquire(true),
    m_nFileAcquireType(TWAINFileFormat_Invalid),
    m_lFileFlags(0L),
    m_bAcquireAttempt(false),
    m_nAcquireCount(-1),
    m_lAcquireNum(-1L),
    m_pFileEnumerator(nullptr),
    m_bTransferDone ( false ),
    m_bAcquireStarted(false),
    m_bDialogModal(true),
    m_bOpenAfterAcquire(false),
    m_bAcquireAutoClose(false),
    m_nMaxAcquisitions(DTWAIN_MAXACQUIRE),
    m_nUIMaxAcquisitions(DTWAIN_MAXACQUIRE),
    m_nNumAcquires(0),
    m_nSpecialMode(0),
    m_UserInterface{},
    m_aAcqAttempts(nullptr),
    m_DeviceEvent {},
    m_bShowUIOnly(false),
    m_nCompression(DTWAIN_CP_NONE),
    m_nState(SOURCE_STATE_CLOSED),
    m_nCompressBytes(0),
    m_bCapCached(false),
    m_bRetrievedAllCaps(false),
    m_bFastCapRetrieval(false),
    m_nJpegQuality ( 0 ),
    m_bJpegProgressive ( false ),
    m_bAutoFeed(true),
    m_nJobControl ( TWJC_NONE ),
    m_nFailAction ( DTWAIN_PAGEFAIL_RETRY ),
    m_nMaxRetryAttempts ( DTWAIN_MAXRETRY_ATTEMPTS ),
    m_nCurRetryCount ( 0 ),
    m_pImageHandler(nullptr),
    m_hAcquireStrip(0),
    m_bUserStripUsed ( false ),
    m_nUserStripSize ( 0 ),
    m_bImagesStored ( false ),
    m_bAutoIncrementFile ( false ),
    m_nCurFileNum ( 0 ),
    m_nFileNameBaseNum ( DTWAIN_INCREMENT_DEFAULT ),
    m_nFileIncrement ( 1 ),
    m_nFileDigits ( 0 ),
    m_nAutoIncrementFlags ( DTWAIN_INCREMENT_DYNAMIC ),
    m_nStartFileNum ( 0 ),
    m_bManualDuplexModeOn ( false ),
    m_nManualDuplexModeFlags ( DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE ),
    m_nMultiPageScanMode ( DTWAIN_FILESAVE_DEFAULT ),
    m_nCurrentSideAcquired ( 0 ),
    m_EOJDetectedValue ( 1 ),
    m_bIsFileSaveIncomplete ( false ),
    m_nJobNum ( 0 ),
    m_bJobStarted ( false ),
    m_bJobFileHandling ( false ),
    m_bImageLayoutValid ( false ),
    m_bIsBlankPageDetectionOn ( false ),
    m_lBlankPageAutoDetect ( DTWAIN_BP_AUTODISCARD_NONE ),
    m_dBlankPageThreshold ( 0.99 ),
    m_nBlankPageCount ( 0 ),
    m_bForceScanOnNoUI ( false ),
    m_bImageNegative ( false ),
    m_bProcessingPixelInfo ( false ),
    m_bSkipImageInfoErrors ( false ),
    m_nForcedBpp ( 0 ),
    m_AltAcquireArea(),
    m_ImageInfo (),
    m_ImageLayout(),
    m_FileSystem(),
    m_pImageMemXfer(nullptr),
    m_PersistentArray(nullptr)
{
    if ( lpszProduct != NULL )
        StringWrapperA::SafeStrcpy( m_SourceId.ProductName,
                                    StringConversion::Convert_Native_To_Ansi(lpszProduct).c_str(),
                                    sizeof( m_SourceId.ProductName ) - 1 );

    // Image information default values
    m_ImageInfoEx.nJpegQuality = 75;
    m_ImageInfoEx.bProgressiveJpeg = false;
    m_ImageInfoEx.PDFAuthor = _T("None");
    m_ImageInfoEx.PDFProducer = _T("None");
    m_ImageInfoEx.PDFTitle = _T("None");
    m_ImageInfoEx.PDFKeywords = _T("None");
    m_ImageInfoEx.PDFSubject = _T("None");
    m_ImageInfoEx.PDFOrientation = DTWAIN_PDF_PORTRAIT;
    m_ImageInfoEx.PDFPageSize = DTWAIN_FS_USLETTER;
    m_ImageInfoEx.PDFCustomSize[0] = 8.5f;
    m_ImageInfoEx.PDFCustomSize[1] = 11.0f;
    m_ImageInfoEx.PDFUseCompression = false;
    m_ImageInfoEx.PDFCustomScale[0] = 100.0;
    m_ImageInfoEx.PDFCustomScale[1] = 100.0;
    m_ImageInfoEx.PDFUseThumbnail = false;
    m_ImageInfoEx.PDFThumbnailScale[0] = m_ImageInfoEx.PDFThumbnailScale[1] = 0.1;
    m_ImageInfoEx.PhotoMetric = PHOTOMETRIC_MINISBLACK;
    m_ImageInfoEx.theSource = this;

    m_AltAcquireArea.UnitOfMeasure = DTWAIN_INCHES;

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
    SetPDFValue(PDFPRODUCERKEY, pHandle->GetVersionString());
}

void CTL_ITwainSource::Reset()
{
    RemoveAllDibs();
}

void CTL_ITwainSource::SetAlternateAcquireArea(double left, double top, double right, double bottom,
                                               LONG /*flags*/, LONG Unit, bool bSet)
{
    if ( bSet )
    {
        m_AltAcquireArea.flags |= CTL_ITwainSource::CROP_FLAG;
        m_AltAcquireArea.m_rect.left = left;
        m_AltAcquireArea.m_rect.top = top;
        m_AltAcquireArea.m_rect.right = right;
        m_AltAcquireArea.m_rect.bottom = bottom;
        m_AltAcquireArea.UnitOfMeasure = Unit;
    }
    else
        m_AltAcquireArea.flags =
            m_AltAcquireArea.flags &~ CTL_ITwainSource::CROP_FLAG;
}

void CTL_ITwainSource::SetImageScale(double xscale, double yscale, bool bSet)
{
    if ( bSet )
    {
        m_AltAcquireArea.m_rectScaling.left = xscale;
        m_AltAcquireArea.m_rectScaling.top  = yscale;
        m_AltAcquireArea.flags |= CTL_ITwainSource::SCALE_FLAG;
    }
    else
        m_AltAcquireArea.flags =
            m_AltAcquireArea.flags &~ CTL_ITwainSource::SCALE_FLAG;
}

bool CTL_ITwainSource::SetManualDuplexMode(LONG nFlags, bool bSet)
{
    m_bManualDuplexModeOn = bSet;
    m_nManualDuplexModeFlags = nFlags;
    return true;
}

void CTL_ITwainSource::GetAlternateAcquireArea(FloatRect& r, LONG& UnitOfMeasure, LONG& flags)
{
    r.left = m_AltAcquireArea.m_rect.left;
    r.top = m_AltAcquireArea.m_rect.top;
    r.right = m_AltAcquireArea.m_rect.right;
    r.bottom = m_AltAcquireArea.m_rect.bottom;
    UnitOfMeasure = m_AltAcquireArea.UnitOfMeasure;
    flags = (LONG)m_AltAcquireArea.flags;
}

void CTL_ITwainSource::GetImageScale(double& xscale, double& yscale, LONG& flags)
{
    xscale = m_AltAcquireArea.m_rectScaling.left;
    yscale = m_AltAcquireArea.m_rectScaling.top;
    flags = (LONG)m_AltAcquireArea.flags;
}

void CTL_ITwainSource::AddCapToStateInfo(TW_UINT16 nCap, short int cStateInfo)
{
    m_mapCapToState[nCap] = cStateInfo;
}

bool CTL_ITwainSource::IsCapNegotiableInState(TW_UINT16 nCap, int nState) const
{
    CapToStateMap::const_iterator it;
    it = m_mapCapToState.find(nCap);
    //...
    if (it != m_mapCapToState.end())
    {
        if ((*it).second & (1 << (nState - 1)))
            return true;
        return false;
    }
    return true;
}

bool CTL_ITwainSource::IsCapabilityCached(TW_UINT16 nCap) const
{
    return m_aCapCache.find(nCap) != m_aCapCache.end();
}

void CTL_ITwainSource::SetCapCached(TW_UINT16 nCapability, bool bSet)
{
    CachedCapMap::iterator found = m_aCapCache.find((TW_UINT16)nCapability);
    TW_UINT16 nVal = nCapability;
    bool bCached = false;
    if ( found != m_aCapCache.end())
        bCached = true;
    // Check if setting and value is not cached
    if (bSet && !bCached)
        m_aCapCache[nVal] = true;   // Add to cache
    else
    if (!bSet && bCached)
        m_aCapCache.erase(found); // Delete from cache
}

int CTL_ITwainSource::IsCapSupportedFromCache(TW_UINT16 nCap)
{
    CachedCapMap::iterator found = m_aCapCache.find((TW_UINT16)nCap);
    if ( found == m_aCapCache.end())
        return -1;
    return (*found).second;
}

void CTL_ITwainSource::AddCapToUnsupportedList(TW_UINT16 nCap)
{
    AddCapToList(m_aUnsupportedCapCache, nCap);
}

void CTL_ITwainSource::AddCapToSupportedList(TW_UINT16 nCap)
{
    AddCapToList(m_aSupportedCapCache, nCap);
}

void CTL_ITwainSource::AddCapToList(CapList& vList, TW_UINT16 nCap)
{
    vList.insert(nCap);
}

bool CTL_ITwainSource::IsCapInUnsupportedList(TW_UINT16 nCap) const
{
    return IsCapInList(m_aUnsupportedCapCache, nCap);
}

bool CTL_ITwainSource::IsCapInSupportedList(TW_UINT16 nCap) const
{
    return IsCapInList(m_aSupportedCapCache, nCap);
}

bool CTL_ITwainSource::IsCapInList(const CapList& vList, TW_UINT16 nCap) const
{
    return vList.count(nCap)?true:false;
}

CapList& CTL_ITwainSource::GetCapSupportedList()
{
    return m_aSupportedCapCache;
}

void CTL_ITwainSource::SetCapSupportedList(CTL_TwainCapArray& rArray)
{
    CapList::iterator it = m_aSupportedCapCache.begin();
    std::copy(rArray.begin(), rArray.end(), std::inserter(m_aSupportedCapCache, it));
}

void CTL_ITwainSource::SetFeederEnableMode( bool bMode )
{
    m_bUseFeeder = bMode;
}

bool CTL_ITwainSource::IsFeederEnabledMode() const
{
    return m_bUseFeeder;
}

bool CTL_ITwainSource::CloseSource(bool bForce)
{
    RemoveAllDibs();
    if ( m_bIsOpened )
    {
        if ( (bForce) && m_bActive)
        {
            ProcessMultipageFile();
            CTL_CloseSourceTriplet CS( m_pSession, this );
            TW_UINT16 rc = CS.Execute();
            if ( rc != TWRC_SUCCESS )
            {
                CTL_TwainAppMgr::ProcessConditionCodeError(
                    CTL_TwainAppMgr::GetConditionCode( m_pSession, NULL, rc ));
                m_bIsOpened = false;
                return false;
            }
            m_nState = SOURCE_STATE_CLOSED;
        }
    }
    m_bIsOpened = false;
    return true;
}

void CTL_ITwainSource::RemoveAllDibs()
{
    m_DibArray->RemoveAllDibs();
}

void CTL_ITwainSource::Clone( CTL_ITwainSource* pSource)
{
    m_bIsOpened = pSource->m_bIsOpened;
    m_bUIOpened = pSource->m_bUIOpened;
}

void CTL_ITwainSource::SetDibHandle(HANDLE hDib, size_t nWhich/*=0*/)
{
    SetDibHandleProc(hDib, nWhich, false);
}


void CTL_ITwainSource::SetDibHandleProc(HANDLE hDib, size_t nWhich, bool bCreatePalette)
{
    size_t nSize = m_DibArray->GetSize();

    if ( nWhich < nSize )
    {
        // replace DIB with this DIB
        m_DibArray->GetAt(nWhich)->SetHandle( hDib, bCreatePalette );
    }
    else
    {
        // Create a dib
        CTL_TwainDibPtr pDib;
        pDib = m_DibArray->CreateDib();
        pDib->SetHandle( hDib, bCreatePalette );
        pDib->SetAutoDelete( m_bDibAutoDelete );
    }
}

void CTL_ITwainSource::SetDibHandleNoPalette(HANDLE hDib, int nWhich/*=0*/ )
{
    SetDibHandleProc(hDib, nWhich, false);
}


HANDLE CTL_ITwainSource::GetDibHandle(int nWhich /*=0*/) const
{
    CTL_TwainDibPtr pDib;
    pDib = GetDibObject(nWhich);
    if ( pDib )
        return *pDib;
    return NULL;
}


CTL_TwainDibPtr CTL_ITwainSource::GetDibObject(int nWhich /*=0*/) const
{
    CTL_TwainDibPtr pDib;
    auto nSize = m_DibArray->GetSize();
    if ( static_cast<size_t>(nWhich) < nSize )
    {
        // replace DIB with this DIB (returns reference to existing object)
        pDib = m_DibArray->GetAt( nWhich );
        return pDib;
    }
    else
        return CTL_TwainDibPtr();
}


bool CTL_ITwainSource::SetCurrentDibPage(int nPage)
{
    auto nDibPages = m_DibArray->GetSize();
    if ( static_cast<size_t>(nPage) >= nDibPages )
        return false;
    m_nCurDibPage = nPage;
    return true;
}


int  CTL_ITwainSource::GetCurrentDibPage()
{
    return m_nCurDibPage;
}

// Get the current page file name for file transfers
CTL_StringType CTL_ITwainSource::GetImageFileName(int curFile)
{
    CTL_StringType strTemp;
    DTWAIN_ARRAY pDTWAINArray = m_pFileEnumerator;
    if ( !pDTWAINArray )
        return _T("");

    int nCount = (int)EnumeratorFunctionImpl::EnumeratorGetCount(pDTWAINArray);
    if ( nCount > 0 && curFile < nCount )
    {
        EnumeratorFunctionImpl::EnumeratorGetAt(pDTWAINArray, curFile, &strTemp); //  (pDTWAINArray->Value(&strTemp, curFile, nStatus);
        return strTemp;
    }
    return _T("");
}

bool CTL_ITwainSource::IsNewJob() const
{
    if ( GetCurrentJobControl() == TWJC_NONE )
        return false;
    if ( !m_bJobStarted )
        return true;
    return false;
}

void CTL_ITwainSource::AddPixelTypeAndBitDepth(int PixelType, int BitDepth)
{
    CachedPixelTypeMap::iterator it = FindPixelType(PixelType);
    if ( it == m_aPixelTypeMap.end())
    {
        // pixel type not found, so add it
        vector<int> BitDepths;
        BitDepths.push_back( BitDepth );
        m_aPixelTypeMap[PixelType] = BitDepths;
    }
    else
    {
        // pixel type found, so see if bit depth exists
        if ( !IsBitDepthSupported( PixelType, BitDepth ))
        {
            // add the bit depth
            (*it).second.push_back(BitDepth);
        }
    }
}

CTL_ITwainSource::CachedPixelTypeMap::iterator CTL_ITwainSource::FindPixelType(int PixelType)
{
    return m_aPixelTypeMap.find(PixelType);
}

bool CTL_ITwainSource::IsPixelTypeSupported(int PixelType)
{
    return FindPixelType(PixelType) != m_aPixelTypeMap.end();
}

bool CTL_ITwainSource::IsBitDepthSupported(int PixelType, int BitDepth)
{
    CachedPixelTypeMap::iterator it = FindPixelType(PixelType);
    if ( it != m_aPixelTypeMap.end())
        // search for bit depth
        return std::find((*it).second.begin(), (*it).second.end(), BitDepth) != (*it).second.end();
    return false;
}

bool CTL_ITwainSource::PixelTypesRetrieved() const
{
    return !m_aPixelTypeMap.empty();
}

CTL_StringType CTL_ITwainSource::GetCurrentImageFileName()// const
{
    // Get the current page number
    int nCurImage = GetPendingImageNum() - GetBlankPageCount();
    if ( nCurImage < 0 )
        nCurImage = 0;

    if ( GetCurrentJobControl() != TWJC_NONE &&
        IsFileTypeMultiPage(GetAcquireFileType()) &&
        IsJobFileHandlingOn())
    {
        // Get the next job number if multipage and job control
        // is being used.
        nCurImage = GetPendingJobNum();
    }

    long lFlags   = GetAcquireFileFlags();
    CTL_StringType strFileBase = GetAcquireFile();

    if ( m_bAutoIncrementFile )
    {
        CTL_StringType strTemp;
        DTWAIN_ARRAY pDTWAINArray = m_pFileEnumerator;
        if ( !pDTWAINArray )
            return m_strAcquireFile;

        int nCount = (int)EnumeratorFunctionImpl::EnumeratorGetCount(pDTWAINArray);
        if ( nCount > 0 )
        {
            EnumeratorFunctionImpl::EnumeratorGetAt(pDTWAINArray, 0, &strTemp);
            m_strAcquireFile = CreateFileNameFromNumber(strTemp, m_nCurFileNum, static_cast<int>(m_nFileDigits));
        }
        else
            m_strAcquireFile = _T("");
        m_nCurFileNum += m_nFileIncrement;
        return m_strAcquireFile;
    }

    if ( lFlags & (DTWAIN_USENAME | DTWAIN_USELONGNAME)) //TWAINFileFlag_USELIST )
    {
        // Get array
        CTL_StringType strTemp;
        DTWAIN_ARRAY pDTWAINArray = m_pFileEnumerator;
        if ( !pDTWAINArray )
            return m_strAcquireFile;
        bool bRet = EnumeratorFunctionImpl::EnumeratorGetAt(pDTWAINArray, nCurImage, &strTemp); //pDTWAINArray->Value(&strTemp, nCurImage, nStatus);
        if ( !bRet ) // No more names
        {
            int nCount = EnumeratorFunctionImpl::EnumeratorGetCount(pDTWAINArray);
            bRet = EnumeratorFunctionImpl::EnumeratorGetAt(pDTWAINArray, nCount-1, &strTemp);
            if ( !bRet )
                return m_strAcquireFile;
            return GetPageFileName( strTemp, nCurImage, (lFlags & DTWAIN_USELONGNAME)?true:false );
        }
        else
            return strTemp;
    }
    return m_strAcquireFile;
}

bool CTL_ITwainSource::IsFileTypeMultiPage(CTL_TwainFileFormatEnum FileType) // static function
{
    return (
            (FileType == TWAINFileFormat_TIFFGROUP3MULTI) ||
            (FileType == TWAINFileFormat_TIFFGROUP4MULTI) ||
            (FileType == TWAINFileFormat_TIFFNONEMULTI)   ||
            (FileType == TWAINFileFormat_TIFFJPEGMULTI)   ||
            (FileType == TWAINFileFormat_TIFFPACKBITSMULTI) ||
            (FileType == TWAINFileFormat_TIFFDEFLATEMULTI) ||
            (FileType == TWAINFileFormat_PDFMULTI)        ||
            (FileType == TWAINFileFormat_POSTSCRIPT1MULTI) ||
            (FileType == TWAINFileFormat_POSTSCRIPT2MULTI) ||
            (FileType == TWAINFileFormat_POSTSCRIPT3MULTI) ||
            (FileType == TWAINFileFormat_TIFFLZWMULTI) ||
            (FileType == TWAINFileFormat_TIFFPIXARLOGMULTI) ||
            (FileType == TWAINFileFormat_DCX)           ||
            (FileType == TWAINFileFormat_TEXTMULTI) ||
            (FileType == DTWAIN_FF_TIFFMULTI)
            );
}

bool CTL_ITwainSource::IsFileTypeTIFF(CTL_TwainFileFormatEnum FileType)
{
    static const std::unordered_set<CTL_TwainFileFormatEnum> setInfo = {
                            TWAINFileFormat_TIFFGROUP3MULTI,
                            TWAINFileFormat_TIFFGROUP4MULTI,
                            TWAINFileFormat_TIFFNONEMULTI,
                            TWAINFileFormat_TIFFJPEGMULTI,
                            TWAINFileFormat_TIFFPACKBITSMULTI,
                            TWAINFileFormat_TIFFDEFLATEMULTI,
                            TWAINFileFormat_TIFFLZWMULTI,
                            TWAINFileFormat_TIFFGROUP4,
                            TWAINFileFormat_TIFFGROUP3,
                            TWAINFileFormat_TIFFNONE,
                            TWAINFileFormat_TIFFJPEG,
                            TWAINFileFormat_TIFFPACKBITS,
                            TWAINFileFormat_TIFFDEFLATE,
                            TWAINFileFormat_TIFFPIXARLOG,
                            TWAINFileFormat_TIFFLZW
                    };
    return (setInfo.count(FileType) == 1);
}

void CTL_ITwainSource::initFileSaveMap() const
{
    #define MAKE_FILE_FORMAT_INFO(T, E) FileFormatInfo((_T(T)), sizeof(T)-1, (_T(E)))

    if ( m_saveFileMap.empty() )
    {
        m_saveFileMap[TWAINFileFormat_TIFFLZW] =
            m_saveFileMap[TWAINFileFormat_TIFFLZWMULTI] = MAKE_FILE_FORMAT_INFO("TIFF Format (LZW) (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[TWAINFileFormat_TIFFNONE] =
            m_saveFileMap[TWAINFileFormat_TIFFNONEMULTI] =
            m_saveFileMap[DTWAIN_FF_TIFF] = MAKE_FILE_FORMAT_INFO("TIFF Uncompressed Format (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[TWAINFileFormat_TIFFGROUP3] =
            m_saveFileMap[TWAINFileFormat_TIFFGROUP3MULTI] = MAKE_FILE_FORMAT_INFO("TIFF Fax Group 3 Format (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[TWAINFileFormat_TIFFGROUP4] =
            m_saveFileMap[TWAINFileFormat_TIFFGROUP4MULTI] = MAKE_FILE_FORMAT_INFO("TIFF Fax Group 4 Format (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[TWAINFileFormat_TIFFPIXARLOG] =
            m_saveFileMap[TWAINFileFormat_TIFFPIXARLOGMULTI] = MAKE_FILE_FORMAT_INFO("TIFF (Pixar-Log Compression) (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[TWAINFileFormat_TIFFJPEG] =
            m_saveFileMap[TWAINFileFormat_TIFFJPEGMULTI] = MAKE_FILE_FORMAT_INFO("TIFF (JPEG Compression) (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[DTWAIN_TIFFPACKBITS] =
            m_saveFileMap[DTWAIN_TIFFPACKBITSMULTI] = MAKE_FILE_FORMAT_INFO("TIFF (Macintosh RLE Compression) (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[DTWAIN_TIFFDEFLATE] =
            m_saveFileMap[DTWAIN_TIFFDEFLATEMULTI] = MAKE_FILE_FORMAT_INFO("TIFF (ZLib Deflate Compression) (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[TWAINFileFormat_JBIG] = MAKE_FILE_FORMAT_INFO("JBIG Format (*.jbg)\0*.jbg\0\0", ".jbg");

        m_saveFileMap[TWAINFileFormat_JPEG2000] = MAKE_FILE_FORMAT_INFO("JPEG-2000 Format (*.jp2)\0*.jp2\0\0",".jp2");

        m_saveFileMap[TWAINFileFormat_WMF] = MAKE_FILE_FORMAT_INFO("Windows MetaFile (*.wmf)\0*.wmf\0\0",".wmf");

        m_saveFileMap[TWAINFileFormat_EMF] = MAKE_FILE_FORMAT_INFO("Windows Enhanced MetaFile (*.emf)\0*.emf\0\0",".emf");

        m_saveFileMap[TWAINFileFormat_PSD] = MAKE_FILE_FORMAT_INFO("Adobe Photoshop Format (*.psd)\0*.psd\0\0",".psd");

        m_saveFileMap[DTWAIN_FF_TIFFMULTI] = MAKE_FILE_FORMAT_INFO("Multipage TIFF Format (*.tif)\0*.tif\0\0", ".tif");

        m_saveFileMap[TWAINFileFormat_BMP] =
            m_saveFileMap[DTWAIN_FF_BMP] = MAKE_FILE_FORMAT_INFO("Windows Bitmap Format (*.bmp)\0*.bmp\0\0", ".bmp");

        m_saveFileMap[TWAINFileFormat_JPEG] =
            m_saveFileMap[DTWAIN_FF_JFIF] = MAKE_FILE_FORMAT_INFO("JFIF (JPEG) Format (*.jpg)\0*.jpg\0\0",".jpg");

        m_saveFileMap[TWAINFileFormat_PDF] =
            m_saveFileMap[TWAINFileFormat_PDFMULTI] = MAKE_FILE_FORMAT_INFO("Adobe Acrobat Format (*.pdf)\0*.pdf\0\0",".pdf");

        m_saveFileMap[TWAINFileFormat_TEXT] =
            m_saveFileMap[TWAINFileFormat_TEXTMULTI] = MAKE_FILE_FORMAT_INFO("Text file (*.txt)\0*.txt\0\0",".txt");

        m_saveFileMap[TWAINFileFormat_ICO] =
            m_saveFileMap[TWAINFileFormat_ICO_VISTA] = MAKE_FILE_FORMAT_INFO("Icon file (*.ico)\0*.ico\0\0",".ico");

        m_saveFileMap[DTWAIN_FF_SPIFF] = MAKE_FILE_FORMAT_INFO("SPIFF Format (*.spf)\0*.spf\0\0",".spf");

        m_saveFileMap[DTWAIN_FF_EXIF] = MAKE_FILE_FORMAT_INFO("EXIF Format (*.exf)\0*.exf\0\0",".exf");

        m_saveFileMap[TWAINFileFormat_PCX] = MAKE_FILE_FORMAT_INFO("PCX Format (*.pcx)\0*.pcx\0\0",".pcx");

        m_saveFileMap[TWAINFileFormat_DCX] = MAKE_FILE_FORMAT_INFO("DCX Format (*.dcx)\0*.dcx\0\0", ".dcx");

        m_saveFileMap[TWAINFileFormat_WBMP] = MAKE_FILE_FORMAT_INFO("WBMP (Wireless Bitmap Format) (*.wbmp)\0*.wbmp\0\0", ".wbmp");

        m_saveFileMap[TWAINFileFormat_PNG] =
            m_saveFileMap[DTWAIN_FF_PNG] = MAKE_FILE_FORMAT_INFO("PNG Format (*.png)\0*.png\0\0",".png");

        m_saveFileMap[TWAINFileFormat_TGA] = MAKE_FILE_FORMAT_INFO("Targa (TGA) Format (*.tga)\0*.tga\0\0",".tga");

        m_saveFileMap[DTWAIN_POSTSCRIPT1] =
            m_saveFileMap[DTWAIN_POSTSCRIPT1MULTI] = MAKE_FILE_FORMAT_INFO("Postscript Level 1 Format (*.ps)\0*.ps\0\0",".ps");

        m_saveFileMap[DTWAIN_POSTSCRIPT2] =
            m_saveFileMap[DTWAIN_POSTSCRIPT2MULTI] = MAKE_FILE_FORMAT_INFO("Postscript Level 2 Format (*.ps)\0*.ps\0\0",".ps");

        m_saveFileMap[DTWAIN_POSTSCRIPT3] =
            m_saveFileMap[DTWAIN_POSTSCRIPT3MULTI] = MAKE_FILE_FORMAT_INFO("Postscript Level 3 Format (*.ps)\0*.ps\0\0",".ps");

        m_saveFileMap[TWAINFileFormat_GIF] = MAKE_FILE_FORMAT_INFO("GIF Format (*.gif)\0*.gif\0\0",".gif");

        m_saveFileMap[DTWAIN_FF_FPX] = MAKE_FILE_FORMAT_INFO("Flash Picture (*.fpx)\0*.fpx\0\0",".fpx");

        m_saveFileMap[DTWAIN_FF_PICT] = MAKE_FILE_FORMAT_INFO("Macintosh PICT format (*.pic)\0*.pic\0\0",".pic");

        m_saveFileMap[DTWAIN_FF_XBM] = MAKE_FILE_FORMAT_INFO("XBM format (*.xbm)\0*.xbm\0\0",".xbm");

        m_saveFileMap[DTWAIN_WEBP] = MAKE_FILE_FORMAT_INFO("webp format (*.webp)\0*.webp\0\0", ".webp");
        m_saveFileMap[DTWAIN_PBM] = MAKE_FILE_FORMAT_INFO("pbm format (*.pbm)\0*.pbm\0\0", ".pbm");
    }
}

bool CTL_ITwainSource::IsFileTypePostscript(CTL_TwainFileFormatEnum FileType)
{
    return (
            (FileType == TWAINFileFormat_POSTSCRIPT1) ||
            (FileType == TWAINFileFormat_POSTSCRIPT1MULTI) ||
            (FileType == TWAINFileFormat_POSTSCRIPT2) ||
            (FileType == TWAINFileFormat_POSTSCRIPT2MULTI) ||
            (FileType == TWAINFileFormat_POSTSCRIPT3) ||
            (FileType == TWAINFileFormat_POSTSCRIPT3MULTI));
}

CTL_StringType CTL_ITwainSource::PromptForFileName() const
{
    CTL_StringType szFilter;
    LPCTSTR szExt;

    initFileSaveMap();
    auto it = m_saveFileMap.find(m_nFileAcquireType);
    if ( it != m_saveFileMap.end() )
    {
        szFilter.append(it->second.filter, it->second.len);
        szExt = it->second.extension;
    }
    else
    {
        CTL_StringStreamType strm;
        strm << m_nFileAcquireType <<_T(" format");
        szFilter = strm.str();
        szFilter.append(_T("*\0\0"), 3);
            szExt = _T(".");
    }

    #ifdef _WIN32
    TCHAR szFile[256];
    // prompt for filename

    CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());

    OPENFILENAME ofn;
    OPENFILENAME *pOfn = &ofn;
    int nExt;

    if (pHandle->m_pofn)
        pOfn = pHandle->m_pofn.get();
    else
        memset(pOfn, 0, sizeof(OPENFILENAME));
    szFile[0] = _T('\0');
    pOfn->lStructSize = sizeof(OPENFILENAME);

    if ( pOfn == &ofn )
    {
        pOfn->hwndOwner = NULL;
        pOfn->lpstrFilter = szFilter.data();
        pOfn->lpstrFile= szFile;
        pOfn->nMaxFile = sizeof(szFile) - 5;
        pOfn->Flags = OFN_OVERWRITEPROMPT | OFN_HIDEREADONLY |
                    OFN_NOREADONLYRETURN | OFN_EXPLORER;
        if ( pHandle->m_CustomPlacement.nOptions != 0 )
        {
            pOfn->lpfnHook = pHandle->m_pSaveAsDlgProc;
            pOfn->Flags |= OFN_ENABLEHOOK;
            pOfn->lCustData = (LPARAM)&pHandle->m_CustomPlacement;
            if ( !StringWrapper::IsEmpty(pHandle->m_CustomPlacement.sTitle) )
                    pOfn->lpstrTitle = pHandle->m_CustomPlacement.sTitle.c_str();
        }
    }

    if (!GetSaveFileName(pOfn)) {
        return _T("");                    // user cancelled dialog
    }

    // supply default extension - GetOpenFileName doesn't seem to do it!
    nExt = pOfn->nFileExtension;
    if (nExt && !szFile[nExt]) {
        // no extension
        lstrcat(szFile, szExt);
    }
    return szFile;
#else
    return CTL_StringType();
#endif
}


bool CTL_ITwainSource::RenderImage(int nWhich/*=0*/)
{
    auto nSize = m_DibArray->GetSize();
    if ( static_cast<size_t>(nWhich) < nSize )
    {
        // replace DIB with this DIB
        return m_DibArray->GetAt(nWhich)->Render();
    }
    return false;
}

bool CTL_ITwainSource::SetDibAutoDelete(bool bSet)
{
    m_bDibAutoDelete = bSet;
    return true;
}

void CTL_ITwainSource:: SetAcquireType(CTL_TwainAcquireEnum AcquireType,
                                      LPCTSTR lpszFile)
{
    m_AcquireType = AcquireType;
    if ( lpszFile )
        m_strAcquireFile = lpszFile;
    else
        m_strAcquireFile.clear();
}

int CTL_ITwainSource::GetNumDibs() const
{
    return static_cast<int>(m_DibArray->GetSize());
}

CTL_ITwainSource::~CTL_ITwainSource()
{
    ResetManualDuplexMode();
    CloseSource(true);
    EnumeratorFunctionImpl::EnumeratorDestroy(m_pFileEnumerator);
}

#undef GetWindow
HWND CTL_ITwainSource::GetOutputWindow() const
{
    return m_hOutWnd;
}


///////////// Specialized cap values that DTWAIN needs to keep in a cache ///////////////////
void CTL_ITwainSource::SetCapCacheValue( LONG lCap, double dValue, bool bTurnOn )
{
    switch (lCap)
    {
        case DTWAIN_CV_ICAPBRIGHTNESS:
            CapCacheInfo.Brightness     = dValue;
            CapCacheInfo.UseBrightness  = bTurnOn;
            return;

        case DTWAIN_CV_ICAPCONTRAST:
            CapCacheInfo.Contrast     = dValue;
            CapCacheInfo.UseContrast  = bTurnOn;
            return;

        case DTWAIN_CV_ICAPXRESOLUTION:
            CapCacheInfo.XResolution   = dValue;
            CapCacheInfo.UseXResolution= bTurnOn;
            return;

        case DTWAIN_CV_ICAPYRESOLUTION:
            CapCacheInfo.YResolution   = dValue;
            CapCacheInfo.UseYResolution= bTurnOn;
            return;

        case DTWAIN_CV_ICAPPIXELFLAVOR:
            CapCacheInfo.PixelFlavor   = (int)dValue;
            CapCacheInfo.UsePixelFlavor= bTurnOn;
            return;

        case DTWAIN_CV_ICAPXNATIVERESOLUTION:
            CapCacheInfo.XNativeResolution   = dValue;
            CapCacheInfo.UseXNativeResolution= bTurnOn;
            return;

        case DTWAIN_CV_ICAPPIXELTYPE:
            CapCacheInfo.PixelType = (int)dValue;
            CapCacheInfo.UsePixelType = bTurnOn;
            return;

        case DTWAIN_CV_ICAPBITDEPTH:
            CapCacheInfo.BitDepth = (int)dValue;
            CapCacheInfo.UseBitDepth = bTurnOn;
            return;
    }
}

double CTL_ITwainSource::GetCapCacheValue( LONG lCap, LONG FAR *pTurnOn ) const
{
    double dValue;
    switch (lCap)
    {
        case DTWAIN_CV_ICAPBRIGHTNESS:
            dValue = CapCacheInfo.Brightness;
            *pTurnOn = CapCacheInfo.UseBrightness;
            return dValue;

        case DTWAIN_CV_ICAPCONTRAST:
            dValue = CapCacheInfo.Contrast;
            *pTurnOn = CapCacheInfo.UseContrast;
            return dValue;

        case DTWAIN_CV_ICAPXRESOLUTION:
            dValue = CapCacheInfo.XResolution;
            *pTurnOn = CapCacheInfo.UseXResolution;
            return dValue;

        case DTWAIN_CV_ICAPYRESOLUTION:
            dValue = CapCacheInfo.YResolution;
            *pTurnOn = CapCacheInfo.UseYResolution;
            return dValue;

       case DTWAIN_CV_ICAPPIXELFLAVOR:
            dValue = (double)CapCacheInfo.PixelFlavor;
            *pTurnOn = CapCacheInfo.UsePixelFlavor;
            return dValue;

        case DTWAIN_CV_ICAPXNATIVERESOLUTION:
            dValue = CapCacheInfo.XNativeResolution;
            *pTurnOn = CapCacheInfo.UseXNativeResolution;
            return dValue;

        case DTWAIN_CV_ICAPPIXELTYPE:
            dValue = (double)CapCacheInfo.PixelType;
            *pTurnOn = CapCacheInfo.UsePixelType;
            return dValue;

        case DTWAIN_CV_ICAPBITDEPTH:
            dValue = (double)CapCacheInfo.BitDepth;
            *pTurnOn = CapCacheInfo.UseBitDepth;
            return dValue;
    }
    *pTurnOn = -1;
    return 0.0;
}

void CTL_ITwainSource::AddDibsToAcquisition(DTWAIN_ARRAY aDibs)
{
   EnumeratorFunctionImpl::EnumeratorAddValue( m_aAcqAttempts, &aDibs );
   EnumeratorFunctionImpl::EnumeratorAddValue(m_PersistentArray, &aDibs);
}

void CTL_ITwainSource::ResetAcquisitionAttempts(DTWAIN_ARRAY aNewAttempts)
{
    // Remove any old acquisitions
    if ( aNewAttempts != m_aAcqAttempts)
    {
        EnumeratorFunctionImpl::EnumeratorDestroy(m_aAcqAttempts);
        m_aAcqAttempts = aNewAttempts;
    }
}

DTWAIN_ARRAY CTL_ITwainSource::GetAcquisitionArray()
{
    return m_aAcqAttempts;
}


void CTL_ITwainSource::SetPDFValue(const CTL_StringType& nWhich, const CTL_StringType& sz)
{
    if ( nWhich == PDFAUTHORKEY)
        m_ImageInfoEx.PDFAuthor = sz.c_str();
    else
    if ( nWhich == PDFPRODUCERKEY)
        m_ImageInfoEx.PDFProducer = sz.c_str();
    else
    if ( nWhich == PDFKEYWORDSKEY)
        m_ImageInfoEx.PDFKeywords = sz.c_str();
    else
    if ( nWhich == PDFTITLEKEY)
        m_ImageInfoEx.PDFTitle = sz.c_str();
    else
    if ( nWhich == PDFSUBJECTKEY )
        m_ImageInfoEx.PDFSubject = sz.c_str();
    else
    if ( nWhich == PSTITLEKEY)
        m_ImageInfoEx.PSTitle = sz.c_str();
    else
    if ( nWhich == PDFOWNERPASSKEY)
        m_ImageInfoEx.PDFOwnerPassword = sz.c_str();
    else
    if ( nWhich == PDFUSERPASSKEY)
        m_ImageInfoEx.PDFUserPassword = sz.c_str();
    else
    if ( nWhich == PDFCREATORKEY)
        m_ImageInfoEx.PDFCreator = sz.c_str();
}


void CTL_ITwainSource::SetPDFValue(const CTL_StringType& nWhich, LONG nValue)
{
    if ( nWhich == PDFORIENTATIONKEY)
        m_ImageInfoEx.PDFOrientation = nValue;
    else
    if ( nWhich == PDFSCALINGKEY)
        m_ImageInfoEx.PDFScaleType = nValue;
    else
    if ( nWhich == PDFCOMPRESSIONKEY)
        m_ImageInfoEx.PDFUseCompression = nValue?true:false;
    else
    if ( nWhich == PDFASCIICOMPRESSKEY)
        m_ImageInfoEx.PDFUseASCIICompression = nValue?true:false;
    else
    if ( nWhich == PSTYPEKEY)
        m_ImageInfoEx.PSType = nValue;
    else
    if ( nWhich == PDFPERMISSIONSKEY)
        m_ImageInfoEx.PDFPermissions = nValue;
    else
    if ( nWhich == PDFJPEGQUALITYKEY)
        m_ImageInfoEx.nPDFJpegQuality = nValue;
    else
    if ( nWhich == PDFOCRMODE)
        m_ImageInfoEx.IsOCRUsedForPDF = nValue?true:false;
    else
    if ( nWhich == PDFPOLARITYKEY)
        m_ImageInfoEx.nPDFPolarity = nValue;
    else
    if (nWhich == PDFAESKEY )
        m_ImageInfoEx.bIsAESEncrypted = nValue?true:false;
}

void CTL_ITwainSource::SetPDFValue(const CTL_StringType& nWhich, DTWAIN_FLOAT f1, DTWAIN_FLOAT f2)
{
    if ( nWhich == PDFSCALINGKEY )
    {
        m_ImageInfoEx.PDFCustomScale[0] = f1;
        m_ImageInfoEx.PDFCustomScale[1] = f2;
    }
}

void CTL_ITwainSource::SetPDFValue(const CTL_StringType& nWhich, PDFTextElementPtr& element)
{
    if ( nWhich == PDFTEXTELEMENTKEY )
        m_ImageInfoEx.PDFTextElementList.push_back(element);
}

void CTL_ITwainSource::SetPDFPageSize(LONG nPageSize, DTWAIN_FLOAT cWidth, DTWAIN_FLOAT cHeight)
{
    m_ImageInfoEx.PDFPageSize = nPageSize;
    m_ImageInfoEx.PDFCustomSize[0] = cWidth;
    m_ImageInfoEx.PDFCustomSize[1] = cHeight;
}

void CTL_ITwainSource::SetPDFEncryption(bool bIsEncrypted,
                                        const CTL_StringType& strUserPassword, const CTL_StringType& strOwnerPassword,
                                        LONG Permissions, bool bUseStrongEncryption)
{
    if ( bIsEncrypted )
    {
        SetPDFValue(PDFUSERPASSKEY, strUserPassword);
        SetPDFValue(PDFOWNERPASSKEY, strOwnerPassword);
        SetPDFValue(PDFPERMISSIONSKEY, Permissions);
        m_ImageInfoEx.bUseStrongEncryption = bUseStrongEncryption?true:false;
        m_ImageInfoEx.bIsPDFEncrypted = true;
    }
    else
        m_ImageInfoEx.bIsPDFEncrypted = false;
}

void CTL_ITwainSource::ClearPDFText()
{
    m_ImageInfoEx.PDFTextElementList.clear();
}

void CTL_ITwainSource::SetPhotometric(LONG Setting)
{
    if ( Setting == 0 )
        m_ImageInfoEx.PhotoMetric = PHOTOMETRIC_MINISWHITE;
    else
    if ( Setting == 1 )
        m_ImageInfoEx.PhotoMetric = PHOTOMETRIC_MINISBLACK;
}


////////////////////////////////////////////////////////////////////////////
static CTL_StringType GetPageFileName(const CTL_StringType &strBase,
                                  int nCurImage,
                                  bool bUseLongNames )
{
    CTL_StringType strFormat;
    CTL_StringType strTemp;
    CTL_StringType strTotal;
    CTL_StringStreamType strm;
    strm << nCurImage;
    strFormat = strm.str();
    int nLenFormat = (int)strFormat.length();

    CTL_StringArrayType rName;
    StringWrapper::SplitPath(strBase, rName);

    CTL_StringType strName = rName[StringWrapper::NAME_POS];

    if ( bUseLongNames )
        strName += strFormat;
    else
    {
        if ((strName + strFormat).length() > 8)
        {
            int nBase = 8 - nLenFormat;
            strName = StringWrapper::Left(strName, nBase) + strFormat;
        }
        else
            strName += strFormat;
    }
    rName[StringWrapper::NAME_POS] = strName;
    return StringWrapper::MakePath(rName);
}

bool CTL_ITwainSource::InitExtImageInfo(int nNum)
{
    if ( !CTL_TwainAppMgr::IsCapabilitySupported(this, DTWAIN_CV_ICAPEXTIMAGEINFO) )
        return false;

    TW_UINT16 nValue;

    if ( !CTL_TwainAppMgr::GetOneCapValue( this, &nValue, TwainCap_EXTIMAGEINFO, TWTY_BOOL ) )
        return false;

    if ( !nValue )
        return false;

    m_pExtImageTriplet.reset(new CTL_ExtImageInfoTriplet(m_pSession, this, nNum));
    if (m_pExtImageTriplet)
    {
        return GetExtImageInfo(true);
    }

    return false;
}


bool CTL_ITwainSource::AddExtImageInfo(const TW_INFO &Info)
{
    if ( m_pExtImageTriplet )
    {
        m_pExtImageTriplet->AddInfo(Info);
        return true;
    }
    return false;
}

bool CTL_ITwainSource::GetExtImageInfo(bool bExecute)
{
    if ( !m_pExtImageTriplet )
       return false;

    if ( bExecute )
    {
        TW_UINT16 rc = m_pExtImageTriplet->Execute();
        switch (rc)
        {
            case TWRC_SUCCESS:
                m_pExtImageTriplet->RetrieveInfo(m_ExtImageVector);
                return true;
        }
    }
    return false;
}

TW_INFO CTL_ITwainSource::GetExtImageInfoItem(int nItem, int nSearchType )
{
    TW_INFO Info;
    Info.NumItems = (TW_UINT16)-1;

    if ( !m_pExtImageTriplet )
        return Info;

    return m_pExtImageTriplet->GetInfo(nItem, nSearchType );
}

bool CTL_ITwainSource::DestroyExtImageInfo()
{
    m_pExtImageTriplet.reset();
    return true;
}

bool CTL_ITwainSource::GetExtImageInfoData(int nWhichItem, int /*nSearch*/, int nWhichValue, LPVOID Data, size_t* pNumChars)
{
    if ( !m_pExtImageTriplet )
        return false;
    return m_pExtImageTriplet->GetItemData(nWhichItem, DTWAIN_BYID, nWhichValue, Data, pNumChars)?true:false;
}

bool CTL_ITwainSource::EnumExtImageInfo(CTL_IntArray& r)
{
    // Function assumes that DAT_EXTIMAGEINFO exists for the Source
    if ( CTL_ExtImageInfoTriplet::EnumSupported(this, m_pSession, r) )
        return true;
    return false;
}

///////////////////////////////////////////////
bool CTL_ITwainSource::IsExtendedCapNegotiable(LONG nCap)
{
    if (find(m_aExtendedCaps.begin(), m_aExtendedCaps.end(), nCap) !=
                m_aExtendedCaps.end())
        return true;
    return false;
}

bool CTL_ITwainSource::AddCapToExtendedCapList(LONG nCap)
{
    m_aExtendedCaps.insert(nCap);
    return  true;
}

bool CTL_ITwainSource::InitFileAutoIncrementData(const CTL_StringType& sName)
{
    if ( 1 ) //m_nFileNameBaseNum == DTWAIN_INCREMENT_DEFAULT )
        m_nCurFileNum = GetInitialFileNumber(sName, m_nFileDigits);
    else
        m_nCurFileNum = m_nFileNameBaseNum;
    m_nStartFileNum = m_nCurFileNum;
    return true;
}

bool CTL_ITwainSource::ResetFileAutoIncrementData()
{
    if ( m_nAutoIncrementFlags == DTWAIN_INCREMENT_DYNAMIC )
        m_nCurFileNum = m_nStartFileNum;
    return true;
}


void CTL_ITwainSource::AddDuplexFileData(const CTL_StringType& fName,
                                         unsigned long nBytes,
                                         int nWhich,
                                         const CTL_StringType& fRealName,
                                         bool bIsJobControl/*=false*/)
{
    sDuplexFileData filedata;
    filedata.sFileName = fName;
    filedata.nBytes = nBytes;
    filedata.sRealFileName = fRealName;
    filedata.bIsJobControlPage = bIsJobControl;

    if ( nWhich == 0 )  // add to front side
        m_DuplexFileData.first.push_back(filedata);
    else
        m_DuplexFileData.second.push_back(filedata);  // add to back side
}

sDuplexFileData CTL_ITwainSource::GetDuplexFileData( int nPage, int nWhich ) const
{
    sDuplexFileData junk;
    const std::vector<sDuplexFileData> *pData;
    if ( nWhich == 0 )
        pData = &m_DuplexFileData.first;
    else
        pData = &m_DuplexFileData.second;

    if ( nPage < (int)pData->size() )
        return pData->at(nPage);
    return junk;
}

void CTL_ITwainSource::RemoveDuplexFileData()
{
    m_bDuplexSideDone[0] = m_bDuplexSideDone[1] = false;
    m_DuplexFileData.first.clear();
    m_DuplexFileData.second.clear();
}

void CTL_ITwainSource::ResetManualDuplexMode(int nWhich/*=-1*/)
{
    if ( nWhich == -1 )
    {
        m_bDuplexSideDone[0] = m_bDuplexSideDone[1] = false;
        DeleteDuplexFiles(0);
        DeleteDuplexFiles(1);
        m_DuplexFileData.first.clear();
        m_DuplexFileData.second.clear();
    }
    else
    {
        m_bDuplexSideDone[nWhich] = false;
        if ( nWhich == 0 )
        {
            DeleteDuplexFiles(0);
            m_DuplexFileData.first.clear();
        }
        else
        {
            DeleteDuplexFiles(1);
            m_DuplexFileData.second.clear();
        }
    }
}

void CTL_ITwainSource::DeleteDuplexFiles(int nWhich)
{
    std::vector<sDuplexFileData> *pData;
    if ( nWhich == 0 )
        pData = &m_DuplexFileData.first;
    else
        pData = &m_DuplexFileData.second;
    for_each(pData->begin(), pData->end(), [](sDuplexFileData& Data) {delete_file(Data.sFileName.c_str()); });
}

unsigned long CTL_ITwainSource::GetNumDuplexFiles(int nWhich) const
{
    if ( nWhich == 0 )
        return static_cast<unsigned long>(m_DuplexFileData.first.size());
    return static_cast<unsigned long>(m_DuplexFileData.second.size());
}

void CTL_ITwainSource::GetImageInfoEx(DTWAINImageInfoEx &ImageInfoEx) const
{
    ImageInfoEx = m_ImageInfoEx;
}

void CTL_ITwainSource::ProcessMultipageFile()
{
    if ( m_DuplexFileData.first.size() > 0 ||
        m_DuplexFileData.second.size() > 0)
    {
        ImageXferFileWriter FileWriter(NULL, m_pSession ,this);
        FileWriter.CloseMultiPageDibFile(GetMutiPageScanMode() != DTWAIN_FILESAVE_MANUALSAVE);
    }
    ClearPDFText(); // clear the text elements
}

static bool isIntCap(LONG capType);

bool isIntCap(DTWAIN_SOURCE Source, LONG nCap)
{
    return isIntCap(DTWAIN_GetCapDataType(Source, nCap));
}

bool isIntCap(LONG capType)
{
    return  capType == TWTY_INT16 ||
        capType == TWTY_INT32 ||
        capType == TWTY_BOOL ||
        capType == TWTY_INT8 ||
        capType == TWTY_UINT8 ||
        capType == TWTY_UINT16 ||
        capType == TWTY_UINT32;
}

bool isFloatCap(LONG capType)
{
    return  capType == TWTY_FIX32;
}

template <typename T>
static DTWAIN_ARRAY PopulateArray(const std::vector<boost::any>& dataArray, CTL_ITwainSource* pSource, TW_UINT16 nCap)
{
    DTWAIN_ARRAY theArray = DTWAIN_ArrayCreateFromCap(pSource, static_cast<LONG>(nCap), static_cast<LONG>(dataArray.size()));
    auto& vVector = EnumeratorVector<T>(theArray);
    std::transform(dataArray.begin(), dataArray.end(), vVector.begin(), [](boost::any theAny) { return boost::any_cast<T>(theAny);});
    return theArray;
}

template <typename T>
static bool PopulateCache(DTWAIN_ARRAY theArray, std::vector<boost::any>& dataArray)
{
    auto& vVector = EnumeratorVector<T>(theArray);
    std::transform(vVector.begin(), vVector.end(), std::back_inserter(dataArray), [](T value){ return value;});
    return true;
}

DTWAIN_ARRAY CTL_ITwainSource::getCapCachedValues(TW_UINT16 lCap, LONG getType)
{
    // get the data type for this cap.
    const CapToValuesMap* mapToUse = &m_capToValuesMap_G;
    if (getType == DTWAIN_CAPGETDEFAULT)
        mapToUse = &m_capToValuesMap_GD;
    auto iter = mapToUse->find(lCap);
    if (iter == mapToUse->end() )
        return NULL;
    const container_values& cValues = (*iter).second;
    if (isIntCap(cValues.m_dataType))
        return PopulateArray<LONG>(cValues.m_data, this, lCap);
    else
    if ( isFloatCap(cValues.m_dataType))
        return PopulateArray<double>(cValues.m_data, this, lCap);
    return NULL;
}

bool CTL_ITwainSource::setCapCachedValues(DTWAIN_ARRAY array, TW_UINT16 lCap, LONG getType)
{
    // get the data type for this cap.
    CapToValuesMap* mapToUse = &m_capToValuesMap_G;
    if (getType == DTWAIN_CAPGETDEFAULT)
        mapToUse = &m_capToValuesMap_GD;
    auto iter = mapToUse->find(lCap);
    if (iter != mapToUse->end())
        return true;
    container_values cValues;
    cValues.m_dataType = DTWAIN_GetCapDataType(this, lCap);
    if (isIntCap(cValues.m_dataType))
    {
        bool retVal = PopulateCache<LONG>(array, cValues.m_data);
        if (retVal)
            return mapToUse->insert(make_pair(lCap, cValues)).second;
    }
    return false;
}

CTL_StringType CreateFileNameFromNumber(const CTL_StringType& sFileName, int num, int nDigits)
{
    CTL_StringArrayType rArray;
    StringWrapper::SplitPath(sFileName, rArray);

    // Adjust the file name
    #ifdef _UNICODE
    char szBuf__[25];
    sprintf(szBuf__, "%0*d", nDigits, num);
    TCHAR szBuf[25];
    std::copy(szBuf__, szBuf__ + 25, szBuf);
    #else
    char szBuf[25];
    sprintf(szBuf, "%0*d", nDigits, num);
    #endif
    CTL_StringType& sTemp = rArray[StringWrapper::NAME_POS];
    sTemp = sTemp.substr(0, sTemp.length() - nDigits) + szBuf;
    return StringWrapper::MakePath(rArray);
}

int GetInitialFileNumber(const CTL_StringType& sFileName, size_t &nDigits)
{
    CTL_StringArrayType rArray;
    StringWrapper::SplitPath(sFileName, rArray);
    nDigits = 0;
    CTL_StringType sTemp;

    size_t nLen = rArray[StringWrapper::NAME_POS].length();
    for ( size_t i = nLen - 1; ; --i)
    {
        if ( StringTraits::IsDigit(rArray[StringWrapper::NAME_POS][i]) )
        {
            sTemp = rArray[StringWrapper::NAME_POS][i] + sTemp;
            nDigits++;
        }
        else
            break;
        if ( i == 0 )
            break;
    }
	
	// now loop until we get a good cast from the string we have
	while (!sTemp.empty())
	{
		try 
		{
			return boost::lexical_cast<int>(sTemp);
		}
		catch (boost::bad_lexical_cast)
		{
			sTemp.erase(sTemp.begin());
		}
	}
	return 0;
}

///////////////////////////////////////////////////////////////////////////////////////////
