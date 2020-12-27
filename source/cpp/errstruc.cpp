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
#define USE_SSTREAM_H  // always use ostringstream.  Too bad if compiler can't support them!
#include <vector>
#include <boost/format.hpp>
#include <sstream>
#include "ctliface.h"
#include "ctltr010.h"
#include "ctltwmgr.h"
#include "errstruc.h"
#include "dtwain_resource_constants.h"

using namespace std;
using namespace dynarithmic;

static CTL_StringType DecodeSourceInfo(pTW_IDENTITY pIdentity, LPCTSTR sPrefix);
static CTL_StringType DecodeData(CTL_ErrorStructDecoder *pDecoder, TW_MEMREF pData, ErrorStructTypes sType);
static CTL_StringType DecodeTW_MEMORY(pTW_MEMORY pMemory, LPCTSTR pMem);
static CTL_StringType DecodeTW_ELEMENT8(pTW_ELEMENT8 pEl, LPCTSTR pMem);
static CTL_StringType DecodeTW_INFO(pTW_INFO pInfo, LPCTSTR pMem);
static CTL_StringType DecodeSupportedGroups(TW_UINT32 SupportedGroups);
static CTL_StringType IndentDefinition() { return CTL_StringType(4, _T(' ')); }

CTL_ContainerToNameMap CTL_ErrorStructDecoder::s_mapContainerType;
CTL_ContainerToNameMap CTL_ErrorStructDecoder::s_mapNotificationType;
std::unordered_map<TW_UINT32, CTL_StringType> CTL_ErrorStructDecoder::s_mapSupportedGroups;
std::unordered_map<TW_UINT16, CTL_StringType> CTL_ErrorStructDecoder::s_mapTwainDSMReturnCodes;

bool CTL_ErrorStructDecoder::s_bInit=false;

#define ADD_ERRORCODE_TO_MAP(theMap, start, x) theMap[(start) + x] = _T(#x);

CTL_ErrorStructDecoder::CTL_ErrorStructDecoder()
{
    if ( !s_bInit )
    {
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_SUCCESS)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_FAILURE)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_CHECKSTATUS)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_CANCEL)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_DSEVENT)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_NOTDSEVENT)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_XFERDONE)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_ENDOFLIST)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_INFONOTSUPPORTED)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_DATANOTAVAILABLE)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_BUSY)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWRC_ERRORSTART, TWRC_SCANNERLOCKED)

        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_SUCCESS)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_BUMMER)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_LOWMEMORY)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_NODS)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_MAXCONNECTIONS)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_OPERATIONERROR)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_BADCAP)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_BADPROTOCOL)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_BADVALUE)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_SEQERROR)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_BADDEST)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_CAPUNSUPPORTED)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_CAPBADOPERATION)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_CAPSEQERROR)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_DENIED)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_FILEEXISTS)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_FILENOTFOUND)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_NOTEMPTY)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_PAPERJAM)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_PAPERDOUBLEFEED)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_FILEWRITEERROR)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_CHECKDEVICEONLINE)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_INTERLOCK)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_DAMAGEDCORNER)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_FOCUSERROR)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_DOCTOOLIGHT)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_DOCTOODARK)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_NOMEDIA)
        ADD_ERRORCODE_TO_MAP(s_mapTwainDSMReturnCodes,IDS_TWCC_ERRORSTART, TWCC_DOCTOOLIGHT)

        ADD_ERRORCODE_TO_MAP(s_mapSupportedGroups, 0, DG_CONTROL)
        ADD_ERRORCODE_TO_MAP(s_mapSupportedGroups, 0, DG_IMAGE)
        ADD_ERRORCODE_TO_MAP(s_mapSupportedGroups, 0, DG_AUDIO)
        ADD_ERRORCODE_TO_MAP(s_mapSupportedGroups, 0, DF_DSM2)
        ADD_ERRORCODE_TO_MAP(s_mapSupportedGroups, 0, DF_APP2)
        ADD_ERRORCODE_TO_MAP(s_mapSupportedGroups, 0, DF_DS2)

        s_mapContainerType[TWON_ARRAY]          = _T("TW_ARRAY");
        s_mapContainerType[TWON_ENUMERATION]    = _T("TW_ENUMERATION");
        s_mapContainerType[TWON_ONEVALUE]       = _T("TW_ONEVALUE");
        s_mapContainerType[TWON_RANGE]          = _T("TW_RANGE");

        // Map of DTWAIN window messages to strings
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_ACQUIREDONE       )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_ACQUIREFAILED     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_ACQUIRECANCELLED  )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_ACQUIRESTARTED    )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PAGECONTINUE      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PAGEFAILED        )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PAGECANCELLED     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TRANSFERREADY     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TRANSFERDONE      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_ACQUIREPAGEDONE   )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_UICLOSING         )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_UICLOSED          )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_UIOPENED          )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_CLIPTRANSFERDONE  )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_INVALIDIMAGEFORMAT)
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_ACQUIRETERMINATED )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TRANSFERSTRIPREADY)
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TRANSFERSTRIPDONE )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_IMAGEINFOERROR    )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_DEVICEEVENT       )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_FILESAVECANCELLED     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_FILESAVEOK            )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_FILESAVEERROR         )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_FILEPAGESAVEOK        )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_FILEPAGESAVEERROR     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PROCESSEDDIB          )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PROCESSDIBACCEPTED      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PROCESSDIBFINALACCEPTED )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TRANSFERSTRIPFAILED   )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_IMAGEINFOERROR        )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TRANSFERCANCELLED     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_UIOPENING             )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPFLIPPAGES       )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPSIDE1DONE       )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPSIDE2DONE       )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPPAGECOUNTERROR  )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPACQUIREDONE     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPSIDE1START      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPSIDE2START      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPMERGEERROR      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPMEMORYERROR     )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPFILEERROR       )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MANDUPFILESAVEERROR   )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_ENDOFJOBDETECTED      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_EOJDETECTED_XFERDONE  )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TWAINPAGECANCELLED    )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_TWAINPAGEFAILED       )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_QUERYPAGEDISCARD      )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PAGEDISCARDED         )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_APPUPDATEDDIB         )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_FILEPAGESAVING        )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_CROPFAILED        )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_PROCESSEDDIBFINAL )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_BLANKPAGEDETECTED1    )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_BLANKPAGEDETECTED2    )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_BLANKPAGEDETECTED3    )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_BLANKPAGEDISCARDED1   )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_BLANKPAGEDISCARDED2   )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_MESSAGELOOPERROR   )
        ADD_ERRORCODE_TO_MAP(s_mapNotificationType, 0,  DTWAIN_TN_SETUPMODALACQUISITION )
        s_bInit = true;
    }
}

#define MAX_DECODE_BUFFER 100000
void CTL_ErrorStructDecoder::StartMessageDecoder(HWND hWnd, UINT nMsg,
                                                 WPARAM wParam, LPARAM lParam)
{
    CTL_StringStreamType sBuffer;

    m_pString = _T("");
    if ( s_mapNotificationType.find(wParam) != s_mapNotificationType.end())
        sBuffer << _T("\nDTWAIN Message(HWND = ") << hWnd << _T(", ") <<
                                    _T("MSG = ") << nMsg << _T(", ") <<
                                    _T("Notification code = ") << s_mapNotificationType[wParam] << _T(", ") <<
                                    _T("LPARAM = ") << lParam;
    else
        sBuffer << _T("\nDTWAIN Message(HWND = ") << hWnd << _T(", ") <<
                                    _T("MSG = ") << nMsg << _T(", ") <<
                                    _T("Notification code = ") << wParam << _T(", ") <<
                                    _T("LPARAM = ") << lParam;
    m_pString = sBuffer.str();
}

void CTL_ErrorStructDecoder::StartDecoder(pTW_IDENTITY pSource, pTW_IDENTITY pDest,
                                         LONG nDG, UINT nDAT, UINT nMSG, TW_MEMREF Data,
                                         ErrorStructTypes sType)
{
    CTL_StringStreamType sBuffer;

    m_pString = _T("");
    CTL_StringType s1;
    sBuffer << _T("\nDSM_Entry(pSource=") << pSource << _T("H, ") <<
                _T("pDest=") << pDest << _T("H, ") <<
           (CTL_TwainDLLHandle::GetTwainNameFromResource(CTL_TwainDLLHandle::GetDGResourceID(),(int)nDG)) << _T(", ") <<
           (CTL_TwainDLLHandle::GetTwainNameFromResource(CTL_TwainDLLHandle::GetDATResourceID(),nDAT)) << _T(", ") <<
           (CTL_TwainDLLHandle::GetTwainNameFromResource(CTL_TwainDLLHandle::GetMSGResourceID(),nMSG)) << _T(", ") <<
           _T("TW_MEMREF=") << Data << _T("H) called\n");
    s1 = sBuffer.str();

    CTL_StringType pSourceStr;
    CTL_StringType pDestStr;
    CTL_StringType pMemRefStr;

    // Decode the pSource argument
    long lErrorFilter = CTL_TwainDLLHandle::GetErrorFilterFlags();
    if ( nDG == DG_CONTROL && nDAT == DAT_EVENT && nMSG == MSG_PROCESSEVENT )
    {
        if (!(lErrorFilter & DTWAIN_LOG_DECODE_TWEVENT) )
            return;
    }
    if ( lErrorFilter & DTWAIN_LOG_DECODE_SOURCE )
    {
        pSourceStr = DecodeSourceInfo(pSource, _T("pSource"));
        pSourceStr += _T("\n");
    }

    // Decode the pDest argument
    if ( lErrorFilter & DTWAIN_LOG_DECODE_DEST)
    {
        pDestStr   = DecodeSourceInfo(pDest, _T("pDest"));
        pDestStr += _T("\n");
    }

    // Decode the TW_MEMREF structure
    if ( lErrorFilter & DTWAIN_LOG_DECODE_TWMEMREF)
        pMemRefStr = DecodeData(this, Data, sType);

    m_pString = s1 + pSourceStr;
    m_pString += pDestStr + pMemRefStr;
}

CTL_StringType CTL_ErrorStructDecoder::DecodeBitmap(HANDLE hBitmap) const
{
    CTL_StringStreamType sBuffer;
    if ( !hBitmap )
        return _T("\n(null bitmap)\n\n");
    LPBITMAPINFOHEADER pbi = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock(hBitmap);
    DTWAINGlobalHandle_RAII dibHandle(hBitmap);
    sBuffer << _T("\nHandle=") << hBitmap << _T("\n") <<
            _T("biSize=") << pbi->biSize << _T("\n") <<
            _T("biWidth=") << pbi->biWidth << _T("\n") <<
            _T("biHeight=") << pbi->biHeight << _T("\n") <<
            _T("biPlanes=") << pbi->biPlanes << _T("\n") <<
            _T("biBitCount=") << pbi->biBitCount << _T("\n") <<
            _T("biCompression=") << pbi->biCompression << _T("\n") <<
            _T("biSizeImage=") << pbi->biSizeImage << _T("\n") <<
            _T("biXPelsPerMeter=") << pbi->biXPelsPerMeter << _T("\n") <<
            _T("biYPelsPerMeter=") << pbi->biYPelsPerMeter << _T("\n") <<
            _T("biClrUsed=") << pbi->biClrUsed << _T("\n") <<
            _T("biClrImportant=") << pbi->biClrImportant << _T("\n\n");
    return sBuffer.str();
}

CTL_StringType CTL_ErrorStructDecoder::DecodePDFTextElement(PDFTextElement *pEl) const
{
    CTL_StringStreamType sBuffer;
    if ( !pEl )
        return _T("\n(null PDF Text Element)\n\n");

    // PDFTextHandle
    CTL_StringType indent(42, _T(' '));
    sBuffer << indent << _T("text=\"") << pEl->m_text << _T("\"\n");
    sBuffer << indent <<  _T("(xpos,ypos)=(") << pEl->xpos << _T(",") << pEl->ypos << _T(")\n");
    sBuffer <<  indent << _T("(scalex,scaley)=(") << pEl->scalingX << _T(",") << pEl->scalingY << _T(")\n");
    sBuffer <<  indent << _T("generalScaling=") << pEl->scaling << _T("\n");
    sBuffer <<  indent << _T("font=") << pEl->m_font.m_fontName << _T("\n");
    sBuffer <<  indent << _T("fontSize=") << pEl->fontSize << _T("\n");
    int r = GetRValue(pEl->colorRGB);
    int g = GetBValue(pEl->colorRGB);
    int b = GetGValue(pEl->colorRGB);
    sBuffer <<  indent << _T("RGBValue=(") << r << _T(",") << g <<_T(",") << b << _T(")\n");
    sBuffer <<  indent << _T("charSpacing=") << pEl->charSpacing << _T("\n");
    sBuffer <<  indent << _T("wordSpacing=") << pEl->wordSpacing << _T("\n");
    sBuffer <<  indent << _T("strokeWidth=") << pEl->strokeWidth << _T("\n");
    sBuffer <<  indent << _T("renderMode=") << pEl->renderMode << _T("\n\n");
    return sBuffer.str();
}


CTL_StringType CTL_ErrorStructDecoder::DecodeTWAINReturnCode(TW_UINT16 retCode) const
{
    return DecodeTWAINCode(retCode, IDS_TWRC_ERRORSTART, _T("Unknown TWAIN Return Code"));
}

CTL_StringType CTL_ErrorStructDecoder::DecodeTWAINReturnCodeCC(TW_UINT16 retCode) const
{
    return DecodeTWAINCode(retCode, IDS_TWCC_ERRORSTART, _T("Unknown TWAIN Condition Code"));
}

CTL_StringType CTL_ErrorStructDecoder::DecodeTWAINCode(TW_UINT16 retCode, TW_UINT16 errStart, const CTL_StringType& defMessage) const
{
    TW_UINT16 actualCode = retCode + errStart;
    auto it = s_mapTwainDSMReturnCodes.find(actualCode);
    if ( it != s_mapTwainDSMReturnCodes.end() )
        return it->second;
    return defMessage;
}

CTL_StringType DecodeData(CTL_ErrorStructDecoder* pDecoder, TW_MEMREF pData, ErrorStructTypes sType)
{
    CTL_StringStreamType sBuffer;
    CTL_StringType sTemp;
    CTL_StringType indenter = IndentDefinition();
    if ( !pData )
        sBuffer << _T("\nNo TW_MEMREF Data");
    else
    {
        switch (sType)
        {
            case ERRSTRUCT_NONE:
                sBuffer << _T("\nNo TW_MEMREF Data");
            break;

            case ERRSTRUCT_TW_CUSTOMDSDATA:
            {
                pTW_CUSTOMDSDATA p = (pTW_CUSTOMDSDATA)pData;
                sBuffer << _T("\nTW_MEMREF is TW_CUSTOMDATA:\n{\n") <<
                            _T("InfoLength=") << p->InfoLength << _T("\n") <<
                            _T("hData=") << p->hData << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_DEVICEEVENT:
            {
                pTW_DEVICEEVENT p = (pTW_DEVICEEVENT)pData;
                sBuffer << _T("\nTW_MEMREF is TW_DEVICEEVENT:\n{\n") <<
                            _T("Event=") << p->Event << _T("\n") <<
                            _T("DeviceName=") << p->DeviceName << _T("\n") <<
                            _T("BatteryMinutes=") << p->BatteryMinutes << _T("\n") <<
                            _T("BatteryPercentage=") << p->BatteryPercentage << _T("\n") <<
                            _T("PowerSupply=") << p->PowerSupply << _T("\n") <<
                            _T("XResolution=") << CTL_CapabilityTriplet::Twain32ToFloat(p->XResolution) << _T("\n") <<
                            _T("YResolution=") << CTL_CapabilityTriplet::Twain32ToFloat(p->YResolution) << _T("\n") <<
                            _T("FlashUsed2=") << p->FlashUsed2 << _T("\n") <<
                            _T("AutomaticCapture=") << p->AutomaticCapture << _T("\n") <<
                            _T("TimeBeforeFirstCapture=") << p->TimeBeforeFirstCapture << _T("\n") <<
                            _T("TimeBetweenCaptures=") << p->TimeBetweenCaptures << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_EVENT:
            {
                LONG lErrorFlags = CTL_TwainDLLHandle::GetErrorFilterFlags();
                if ( lErrorFlags & DTWAIN_LOG_DECODE_TWEVENT )
                {
                    pTW_EVENT p = (pTW_EVENT)pData;
                    MSG *pmsg = (MSG*)p->pEvent;
                    sBuffer << _T("\nTW_MEMREF is TW_EVENT:\n{\n") <<
                                indenter << _T("pEvent has MSG structure:\n") <<
                                indenter << _T("MSG Values\n") <<
                                indenter << _T("{")<<
                                _T(" hwnd=") <<  pmsg->hwnd <<
                                _T(", message=") << pmsg->message <<
                                _T(", wParam=") << pmsg->wParam <<
                                _T(", lParam=") << pmsg->lParam <<
                                _T(", time=") << pmsg->time <<
                                _T(", point.x=") << pmsg->pt.x <<
                                _T(", point.y=") << pmsg->pt.y <<
                                _T(" }\n") <<
                                indenter << _T("DS Message=") << p->TWMessage << _T("\n}\n");
                }
            }
            break;

            case ERRSTRUCT_TW_FILESYSTEM:
            {
                pTW_FILESYSTEM p = (pTW_FILESYSTEM)pData;
                sBuffer << _T("\nTW_MEMREF is TW_FILESYSTEM:\n{\n") <<
                        indenter << _T("InputName=") << p->InputName << _T("\n") <<
                        indenter << _T("OutputName=") << p->OutputName << _T("\n") <<
                        indenter << _T("Context=") << p->Context << _T("H\n") <<
                        indenter << _T("Recursive=") << p->Recursive << _T("\n") <<
                        indenter << _T("FileType=") << p->FileType << _T("\n") <<
                        indenter << _T("Size=") << p->Size << _T("\n") <<
                        indenter << _T("CreateTimeDate=") << p->CreateTimeDate << _T("\n") <<
                        indenter << _T("ModifiedTimeDate=") << p->ModifiedTimeDate << _T("\n") <<
                        indenter << _T("FreeSpace=") << p->FreeSpace << _T("\n") <<
                        indenter << _T("NewImageSize=") << p->NewImageSize << _T("\n") <<
                        indenter << _T("NumberOfFiles=") << p->NumberOfFiles << _T("\n") <<
                        indenter << _T("NumberOfSnippets=") << p->NumberOfSnippets << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_IDENTITY:
            {
                sBuffer << _T("\nTW_MEMREF is TW_IDENTITY:\n") << DecodeSourceInfo((pTW_IDENTITY)pData, _T("TW_MEMREF"));
            }
            break;

            case ERRSTRUCT_TW_MEMORY:
            {
                TW_MEMORY* pMemory = (TW_MEMORY*)pData;
                sBuffer <<
                    _T("\nTW_MEMREF is TW_MEMORY:\n{\n") <<
                    indenter << _T("Flags=") << pMemory->Flags<< _T("\n") <<
                    indenter << _T("Length=") << pMemory->Length << _T("\n") <<
                    indenter << _T("TheMem=") << pMemory->TheMem << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_ENTRYPOINT:
            {
                TW_ENTRYPOINT* pEntryPoint = (TW_ENTRYPOINT*)pData;
                sBuffer <<
                    _T("\nTW_MEMREF is TW_ENTRYPOINT:\n{\n") <<
                    indenter << _T("Size=") << pEntryPoint->Size << _T("\n") <<
                    indenter << _T("DSMEntry=") << pEntryPoint->DSM_Entry << _T("\n") <<
                    indenter << _T("DSMMemAllocate=") << pEntryPoint->DSM_MemAllocate << _T("\n") <<
                    indenter << _T("DSMMemLock=") << pEntryPoint->DSM_MemLock << _T("\n") <<
                    indenter << _T("DSMMemUnlock=") << pEntryPoint->DSM_MemUnlock << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_LPHWND:
            {
            #ifdef _WIN32
                RECT r;
                HWND *p = (HWND *)pData;
                ::GetWindowRect(*p, &r);

                sBuffer <<
                _T("\nTW_MEMREF is handle to window (HWND):\n{\n") <<
                indenter << _T("HWND=") << *p << _T("\n") <<
                indenter << _T("Screen Pos.= (") << r.left << _T(",") << r.top << _T(")-(") <<
                                    r.right << _T(",") << r.bottom << _T(")\n}\n");
            #endif
            }
            break;

            case ERRSTRUCT_TW_PASSTHRU:
            {
                pTW_PASSTHRU p = (pTW_PASSTHRU)pData;
                sBuffer <<
                _T("\nTW_MEMREF is TW_PASSTHRU:\n{\n") <<
                indenter << _T("Command=") << p->pCommand << _T("H\n") <<
                indenter << _T("CommandBytes=") << p->CommandBytes << _T("\n") <<
                indenter << _T("Direction=") << p->Direction << _T("\n") <<
                indenter << _T("pDataBuffer=") << p->pData << _T("H\n") <<
                indenter << _T("DataBytes=") << p->DataBytes << _T("\n") <<
                indenter << _T("DataBytesXfered=") << p->DataBytesXfered << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_PENDINGXFERS:
            {
                pTW_PENDINGXFERS p = (pTW_PENDINGXFERS)pData;
                sBuffer << _T("\nTW_MEMREF is TW_PENDINGXFERS:\n{\n") <<
                            indenter << _T("Count=") << p->Count << _T("\n") <<
                            indenter << _T("EOJ=") << p->EOJ << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_SETUPFILEXFER:
            {
                pTW_SETUPFILEXFER p = (pTW_SETUPFILEXFER)pData;
                sBuffer <<
                _T("\nTW_MEMREF is TW_SETUPFILEXFER:\n{\n") <<
                indenter << _T("FileName=") << p->FileName << _T("\n") <<
                indenter << _T("Format=") << p->Format << _T("\n") <<
                indenter << _T("VRefNum=") << p->VRefNum << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_SETUPMEMXFER:
            {
                pTW_SETUPMEMXFER p = (pTW_SETUPMEMXFER)pData;
                sBuffer << _T("\nTW_MEMREF is TW_SETUPMEMXFER:\n{\n") <<
                        indenter << _T("MinBufSize=") << p->MinBufSize << _T("\n") <<
                        indenter << _T("MaxBufSize=") << p->MaxBufSize << _T("\n") <<
                        indenter << _T("Preferred=") << p->Preferred << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_CAPABILITY:
            {
                CTL_ContainerToNameMap::iterator it;
                pTW_CAPABILITY p = (pTW_CAPABILITY)pData;
                it = pDecoder->s_mapContainerType.find((int)p->ConType);
                CTL_StringType s = _T("Unspecified (TWON_DONTCARE)");
                if (it != pDecoder->s_mapContainerType.end() )
                    s = (*it).second;

                sBuffer << _T("\nTW_MEMREF is TW_CAPABILITY:\n{\n") <<
                        indenter << _T("Cap=") << StringConversion::Convert_Ansi_To_Native(CTL_TwainAppMgr::GetCapNameFromCap(p->Cap)) << _T("\n") <<
                        indenter << _T("ContainerType=") << s << _T("\n") <<
                        indenter << _T("hContainer=") << p->hContainer << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_STATUSUTF8:
            {
                CTL_ContainerToNameMap::iterator it;
                pTW_STATUSUTF8 p = (pTW_STATUSUTF8)pData;
                pTW_STATUS pStatus = &p->Status;
                sBuffer << _T("\nTW_MEMREF is TW_STATUSUTF8:\n{\n") <<
                    indenter << _T("Status ConditionCode=") << pStatus->ConditionCode << _T("\n") <<
                    indenter << _T("Size=") << p->Size << _T("\n") <<
                    indenter << _T("UTF8string=") << p->UTF8string << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_STATUS:
            {
                pTW_STATUS p = (pTW_STATUS)pData;
                CTL_StringType sConditionCode = _T("(Unknown)");
                if (CTL_ErrorStructDecoder::s_mapTwainDSMReturnCodes.find(IDS_TWCC_ERRORSTART + p->ConditionCode)
                    != CTL_ErrorStructDecoder::s_mapTwainDSMReturnCodes.end())
                    sConditionCode = _T("(") + CTL_ErrorStructDecoder::s_mapTwainDSMReturnCodes[IDS_TWCC_ERRORSTART + p->ConditionCode] + _T(")");
                sBuffer << _T("\nTW_MEMREF is TW_STATUS:\n{\n") <<
                        indenter << _T("ConditionCode=") << p->ConditionCode << _T("  ") << sConditionCode << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_USERINTERFACE:
            {
            #ifdef _WIN32
                pTW_USERINTERFACE p = (pTW_USERINTERFACE)pData;
                TCHAR sz[256];
                RECT r;
                SetRect(&r,0,0,0,0);
                sz[0] = _T('\0');
                sBuffer << _T("\nTW_MEMREF is TW_USERINTERFACE:\n{\n") <<
                        indenter << _T("ShowUI=") <<  (p->ShowUI?_T("TRUE"):_T("FALSE")) << _T("\n") <<
                        indenter << _T("ModalUI=") << (p->ModalUI?_T("TRUE"):_T("FALSE")) << _T("\n") <<
                        indenter << _T("hParent=") << p->hParent << _T("\n") <<
                        indenter << _T("hParent.Title=") << sz << _T("\n") <<
                        indenter << _T("hParent.ScreenPos.=(") <<
                        r.left << _T(",") << r.top << _T(")-(") <<
                        r.right << _T(",") << r.bottom << _T(")\n}\n");
            #endif
            }
            break;

            case ERRSTRUCT_TW_IMAGEINFO:
            {
                pTW_IMAGEINFO p = (pTW_IMAGEINFO)pData;
                sBuffer << _T("\nTW_MEMREF is TW_IMAGEINFO:\n{\n") <<
                        indenter << _T("XResolution=") << CTL_CapabilityTriplet::Twain32ToFloat(p->XResolution) << _T("\n") <<
                        indenter << _T("YResolution=") << CTL_CapabilityTriplet::Twain32ToFloat(p->YResolution) << _T("\n") <<
                        indenter << _T("ImageWidth=") << p->ImageWidth << _T("\n") <<
                        indenter << _T("ImageLength=") << p->ImageLength << _T("\n") <<
                        indenter << _T("SamplesPerPixel=") << p->SamplesPerPixel << _T("\n") <<
                        indenter << _T("BitsPerSample(") <<
                        p->BitsPerSample[0] << _T(",")  <<
                        p->BitsPerSample[1] << _T(",")  <<
                        p->BitsPerSample[2] << _T(",")  <<
                        p->BitsPerSample[3] << _T(",")  <<
                        p->BitsPerSample[4] << _T(",")  <<
                        p->BitsPerSample[5] << _T(",")  <<
                        p->BitsPerSample[6] << _T(",")  <<
                        p->BitsPerSample[7] << _T(")\n")  <<
                        indenter << _T("BitsPerPixel=") << p->BitsPerPixel << _T("\n") <<
                        indenter << _T("Planar=") << (p->Planar?_T("TRUE"):_T("FALSE")) << _T("\n") <<
                        indenter << _T("PixelType=") << p->PixelType << _T("\n") <<
                        indenter << _T("Compression=") << p->Compression << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_IMAGELAYOUT:
            {
                pTW_IMAGELAYOUT p = (pTW_IMAGELAYOUT)pData;
                sBuffer <<
                _T("\nTW_MEMREF is TW_IMAGELAYOUT:\n{\n") <<
                indenter << _T("Frame=(") <<
                CTL_CapabilityTriplet::Twain32ToFloat(p->Frame.Left) << _T(",") <<
                CTL_CapabilityTriplet::Twain32ToFloat(p->Frame.Top) << _T(")-(") <<
                CTL_CapabilityTriplet::Twain32ToFloat(p->Frame.Right) << _T(",") <<
                CTL_CapabilityTriplet::Twain32ToFloat(p->Frame.Bottom) << _T(")\n") <<
                indenter << _T("DocmentNumber=") << p->DocumentNumber << _T("\n") <<
                indenter << _T("PageNumber=") << p->PageNumber << _T("\n") <<
                indenter << _T("FrameNumber=") << p->FrameNumber << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_IMAGEMEMXFER:
            {
                pTW_IMAGEMEMXFER p = (pTW_IMAGEMEMXFER)pData;
                sBuffer << _T("\nTW_MEMREF is TW_IMAGEMEMXFER:\n{\n") <<
                            indenter << _T("Compression=") << p->Compression << _T("\n") <<
                            indenter << _T("BytesPerRow=") << p->BytesPerRow << _T("\n") <<
                            indenter << _T("Columns=") << p->Columns << _T("\n") <<
                            indenter << _T("Rows=") << p->Rows << _T("\n") <<
                            indenter << _T("XOffset=") << p->XOffset << _T("\n") <<
                            indenter << _T("YOffset=") << p->YOffset << _T("\n") <<
                            indenter << _T("BytesWritten=") << p->BytesWritten << _T("\n") <<
                            indenter << DecodeTW_MEMORY(&p->Memory,_T("Memory")) << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_HDIB:
            {
                HANDLE h = (HANDLE)pData;
                sBuffer << _T("\nTW_MEMREF is a DIB:\n{\n") <<
                            indenter << _T("DIB Handle=") << h << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_PALETTE8:
            {
                pTW_PALETTE8 p = (pTW_PALETTE8)pData;
                sBuffer << _T("\nTW_MEMREF is a TW_PALETTE8:\n{\n") <<
                            indenter << _T("NumColors=") << p->NumColors << _T("\n") <<
                            indenter << _T("PaletteType=") << p->PaletteType << _T("\n");
                for ( int i = 0; i < 256; i++ )
                {
                    sBuffer << _T("ColorInfo[") << i << _T("]") <<
                            _T(" - Index=") << (int)p->Colors[i].Index <<
                            _T(", Channel1=") << (int)p->Colors[i].Channel1 <<
                            _T(", Channel2=") << (int)p->Colors[i].Channel2 <<
                            _T(", Channel3=") << (int)p->Colors[i].Channel3 << _T("\n");
                }
                sBuffer << _T("}\n");
            }
            break;

            case ERRSTRUCT_pTW_UINT32:
            {
                sBuffer << _T("\nTW_MEMREF is TW_UINT32 pointer:\n{\n") <<
                            indenter << _T("Address=") << pData << _T("H\n") <<
                            indenter << _T("Value at Address=") << *(TW_UINT32 *)pData << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_CIECOLOR:
            {
                const TCHAR *CIEPointNames[4] = {_T("WhitePoint"), _T("BlackPoint"), _T("WhitePaper"), _T("BlackInk")};
                CTL_StringType str2;
                int i;
                pTW_CIECOLOR p = (pTW_CIECOLOR)pData;
                pTW_CIEPOINT aPoints[4] = {&p->WhitePoint, &p->BlackPoint, &p->WhitePaper,
                                        &p->BlackInk};
                sBuffer << _T("\nTW_MEMREF is TW_CIECOLOR:\n{\n{\n") <<
                            _T("ColorSpace=") << p->ColorSpace << _T(",\n") <<
                            _T("LowEndian=") << p->LowEndian << _T(",\n") <<
                            _T("DeviceDependent=") << p->DeviceDependent << _T(",\n") <<
                            _T("VersionNumber=") << p->VersionNumber << _T("\n\nTransform Stage Info:\n}");

                pTW_TRANSFORMSTAGE pCurTransform;
                for ( int nTransform = 0; nTransform < 2; nTransform++)
                {
                    if ( nTransform == 0 )
                        pCurTransform = &p->StageABC;
                    else
                        pCurTransform = &p->StageLMN;
                    for ( i = 0; i < 3; i++ )
                    {
                        sBuffer << _T("Decode Value[") << i << _T("] =");
                        sBuffer << _T("{\n") <<
                          _T("StartIn=") << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].StartIn) << _T(", ") <<
                          _T("BreakIn=") << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].BreakIn) << _T(", ") <<
                          _T("EndIn=")   << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].EndIn) << _T(",\n") <<
                          _T("StartOut=") << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].StartOut) << _T(", ") <<
                          _T("BreakOut=") << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].BreakOut) << _T(", ") <<
                          _T("EndOut=") << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].EndOut) << _T(", \n") <<
                          _T("Gamma=") << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].Gamma) << _T(", ") <<
                          _T("SampleCount=") << CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Decode[i].SampleCount) <<
                          _T("\n}\n");
                    }
                    int j;
                    str2.clear();
                    for ( i = 0; i < 3; i++ )
                    {
                        for ( j = 0; j < 3; j++ )
                        {
                            sBuffer << _T("MixValue[") << i << _T("][") << j << _T("]=") <<
                                    CTL_CapabilityTriplet::Twain32ToFloat(pCurTransform->Mix[i][j]) << _T("\n");
                        }
                    }
                }

                // Get the CIE info
                for ( i = 0; i < 4; i++ )
                {
                    sBuffer << _T("CIEPoint ") << CIEPointNames[i] << _T(" = {") <<
                                CTL_CapabilityTriplet::Twain32ToFloat(aPoints[i]->X) << _T(",") <<
                                CTL_CapabilityTriplet::Twain32ToFloat(aPoints[i]->Y) << _T(",") <<
                                CTL_CapabilityTriplet::Twain32ToFloat(aPoints[i]->Z) << _T("}\n");
                }

                sBuffer << _T("\nSample is user-defined and can't be determined \n}\n");
            }
            break;

            case ERRSTRUCT_TW_GRAYRESPONSE:
            {
                pTW_GRAYRESPONSE p = (pTW_GRAYRESPONSE)pData;
                sBuffer << _T("\nTW_MEMREF is TW_GRAYRESPONSE:\n{\n") <<
                            DecodeTW_ELEMENT8(&p->Response[0], _T("Response[0]")) <<
                            _T("\n}\n");
            }
            break;
            case ERRSTRUCT_TW_RGBRESPONSE:
            {
                pTW_RGBRESPONSE p = (pTW_RGBRESPONSE)pData;
                sBuffer << _T("\nTW_MEMREF is TW_RGBRESPONSE:\n{\n") <<
                            DecodeTW_ELEMENT8(&p->Response[0], _T("Response[0]")) <<
                            _T("\n}\n");
            }
            break;
            case ERRSTRUCT_TW_JPEGCOMPRESSION:
            {
                pTW_JPEGCOMPRESSION p = (pTW_JPEGCOMPRESSION)pData;
                sBuffer << _T("\nTW_MEMREF is TW_JPEGCOMPRESSION:\n{\n") <<
                            indenter << _T("ColorSpace=") << p->ColorSpace << _T("\n") <<
                            indenter << _T("SubSampling=") << p->SubSampling << _T("\n") <<
                            indenter << _T("NumComponents=") << p->NumComponents << _T("\n") <<
                            indenter << _T("RestartFrequency=") << p->RestartFrequency << _T("\n") <<
                            indenter << _T("QuantMap={") <<
                            p->QuantMap[0] << _T(",") <<
                            p->QuantMap[1] << _T(",") <<
                            p->QuantMap[2] << _T(",") <<
                            p->QuantMap[3] << _T("}\n") <<
                            indenter << DecodeTW_MEMORY(&p->QuantTable[0],_T("QuantTable[0]")) <<
                            _T("\n") <<
                            indenter << DecodeTW_MEMORY(&p->QuantTable[1],_T("QuantTable[1]"))<<
                            _T("\n") <<
                            indenter << DecodeTW_MEMORY(&p->QuantTable[2],_T("QuantTable[2]"))<<
                            _T("\n") <<
                            indenter << DecodeTW_MEMORY(&p->QuantTable[3],_T("QuantTable[3]"))<<
                            _T("\n") <<
                            indenter << _T("HuffmanMap={") <<
                            p->HuffmanMap[0] << _T(",") <<
                            p->HuffmanMap[1] << _T(",") <<
                            p->HuffmanMap[2] << _T(",") <<
                            p->HuffmanMap[3] << _T("}\n") <<
                            indenter << DecodeTW_MEMORY(&p->HuffmanDC[0],_T("HuffmanDC[0]")) <<
                            _T("\n") <<
                            indenter << DecodeTW_MEMORY(&p->HuffmanDC[1],_T("HuffmanDC[1]")) <<
                            _T("\n") <<
                            indenter << DecodeTW_MEMORY(&p->HuffmanAC[0],_T("HuffmanAC[0]")) <<
                            _T("\n") <<
                            indenter << DecodeTW_MEMORY(&p->HuffmanAC[1],_T("HuffmanAC[1]")) <<
                            _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_EXTIMAGEINFO:
            {
                CTL_StringStreamType TempStream;
                pTW_EXTIMAGEINFO p = (pTW_EXTIMAGEINFO)pData;
                TempStream << _T("\nTW_MEMREF is TW_EXTIMAGINFO:\n{\n") << _T("NumInfos=") << p->NumInfos << _T("\n");

                CTL_StringType sAllInfo = TempStream.str();
                CTL_StringStreamType strm;
                for (TW_UINT32 i = 0; i < p->NumInfos; i++ )
                {
                    #ifdef UNICODE
                    strm << boost::wformat(_T("Info[%1%]=%2%\n")) % i % DecodeTW_INFO(&p->Info[i], NULL);
                    #else
                    strm << boost::format(_T("Info[%1%]=%2%\n")) % i % DecodeTW_INFO(&p->Info[i], NULL);
                    #endif
                }
                sAllInfo += strm.str();
                sBuffer << sAllInfo << _T("}\n");
            }
            break;

            case ERRSTRUCT_TW_TWUNKIDENTITY:
            {
                pTW_TWUNKIDENTITY p = (pTW_TWUNKIDENTITY)pData;
                pTW_IDENTITY pIdentity = &p->identity;
                CTL_StringType dsPath = _T(" ");
                if ( p->dsPath )
                {
                    #ifdef UNICODE
                    dsPath = (LPWSTR)p->dsPath;
                    #else
                    dsPath = p->dsPath;
                    #endif
                }
                sBuffer << _T("\nTW_MEMREF is TW_TWUNKIDENTITY:\n{\n") <<
                            indenter << DecodeSourceInfo(pIdentity, _T("TW_TWUNKIDENTITY")) << _T("\n") <<
                            indenter << _T("dsPath = ") << dsPath << _T("\n}");
            }
            break;

            case ERRSTRUCT_TW_AUDIOINFO:
            {
                pTW_AUDIOINFO p = (pTW_AUDIOINFO)pData;
                sBuffer << _T("\nTW_MEMREF is TW_AUDIOINFO:\n{\n") <<
                    indenter << _T("Name=") << p->Name << _T("\n") <<
                    indenter << _T("Reserved=") << p->Reserved << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_CALLBACK:
            {
                pTW_CALLBACK p = (pTW_CALLBACK)pData;
                sBuffer << _T("\nTW_MEMREF is TW_CALLBACK:\n{\n");
                #if defined(__APPLE__)
                    sBuffer << indenter << _T("Refcon=") << p->RefCon << _T("\n");
                #endif
                    sBuffer << indenter << _T("Message=") << p->Message << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_CALLBACK2:
            {
                pTW_CALLBACK2 p = (pTW_CALLBACK2)pData;
                sBuffer << _T("\nTW_MEMREF is TW_CALLBACK2:\n{\n");
                sBuffer << indenter << _T("CallbackProc=") << p->CallBackProc << _T("\n");
                sBuffer << indenter << _T("Refcon=") << p->RefCon << _T("\n");
                sBuffer << indenter << _T("Message=") << p->Message << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_METRICS:
            {
                pTW_METRICS p = (pTW_METRICS)pData;
                sBuffer << _T("\nTW_MEMREF is TW_METRICS:\n{\n");
                sBuffer << indenter << _T("SizeOf=") << p->SizeOf << _T("\n");
                sBuffer << indenter << _T("ImageCount=") << p->ImageCount << _T("\n");
                sBuffer << indenter << _T("SheetCount=") << p->SheetCount << _T("\n}\n");
            }
            break;

            case ERRSTRUCT_TW_TWAINDIRECT:
            {
                pTW_TWAINDIRECT p = (pTW_TWAINDIRECT)pData;
                sBuffer << _T("\nTW_MEMREF is TW_TWAINDIRECT:\n{\n");
                sBuffer << indenter << _T("SizeOf=") << p->SizeOf << _T("\n");
                sBuffer << indenter << _T("CommunicationManager=") << p->CommunicationManager << _T("\n");
                sBuffer << indenter << _T("Send=") << p->Send << _T("\n");
                sBuffer << indenter << _T("SendSize=") << p->SendSize << _T("\n");
                sBuffer << indenter << _T("Receive=") << p->Receive << _T("\n");
                sBuffer << indenter << _T("ReceiveSize=") << p->ReceiveSize << _T("\n}\n");
            }
            break;

        }
    }
    sTemp = sBuffer.str();
    return sTemp;
}

CTL_StringType DecodeSourceInfo(pTW_IDENTITY pIdentity, LPCTSTR sPrefix)
{
    CTL_StringType indenter = IndentDefinition();
    CTL_StringStreamType sBuffer;
    if ( pIdentity)
    {
        sBuffer << _T("Decoded ") << sPrefix << _T(":\n{\n") <<

        indenter << _T("Id=") << pIdentity->Id << _T("\n") <<
        indenter << _T("Version Number=") << pIdentity->Version.MajorNum << _T(".") << pIdentity->Version.MinorNum << _T("\n") <<
        indenter << _T("Version Language=") << pIdentity->Version.Language << _T("\n") <<
        indenter << _T("Version Country=") << pIdentity->Version.Country << _T("\n") <<
        indenter << _T("Version Info=")   << pIdentity->Version.Info << _T("\n") <<
        indenter << _T("ProtocolMajor=")  << pIdentity->ProtocolMajor << _T("\n")  <<
        indenter << _T("ProtocolMinor=")  << pIdentity->ProtocolMinor << _T("\n")    <<
        indenter << _T("SupportedGroups=") << DecodeSupportedGroups(pIdentity->SupportedGroups) << _T("\n") <<
        indenter << _T("Manufacturer=") << pIdentity->Manufacturer << _T("\n") <<
        indenter << _T("Product Family=") << pIdentity->ProductFamily << _T("\n") <<
        indenter << _T("Product Name=") << pIdentity->ProductName <<

        _T("\n}\n");
    }
    else
    {
        sBuffer << _T("\nNo information for ") << sPrefix << _T("\n");
    }
    return sBuffer.str();
}

CTL_StringType DecodeSupportedGroups(TW_UINT32 SupportedGroups)
{
    CTL_StringStreamType sBuffer;
    const unsigned int numberOfBits = sizeof(TW_UINT32) << 3;
    bool foundGroup = false;
    for (unsigned int i = 0; i < numberOfBits; ++i)
    {
        unsigned int curGroup = (TW_UINT32)1 << i;
        if ( SupportedGroups & curGroup )
        {
            auto it = CTL_ErrorStructDecoder::s_mapSupportedGroups.find( curGroup );
            if ( it != CTL_ErrorStructDecoder::s_mapSupportedGroups.end() )
            {
                if ( foundGroup )
                    sBuffer << _T(",");
                sBuffer << _T(" ") << it->second;
                foundGroup = true;
            }
            else
                sBuffer << _T(" Unknown(") << curGroup << _T(")");
        }
    }
    return sBuffer.str();
}

CTL_StringType DecodeTW_MEMORY(pTW_MEMORY pMemory, LPCTSTR pMem)
{
    CTL_StringType sTemp;
    CTL_StringStreamType sBuffer;
    sBuffer << _T("{Flags=") <<
            pMemory->Flags << _T(", ") <<
            _T("Length=") <<
            pMemory->Length << _T(", ") <<
            _T("TheMem=") << pMemory->TheMem << _T("H}");
    sTemp = sBuffer.str();
    if ( pMem )
    {
        sTemp = pMem;
        sTemp += _T("=");
    }
    sTemp += sBuffer.str();
    return sTemp;
}


CTL_StringType DecodeTW_ELEMENT8(pTW_ELEMENT8 pEl, LPCTSTR pMem)
{
    CTL_StringType sTemp;
    CTL_StringStreamType sBuffer;

    sBuffer << _T("{Index=") << pEl->Index << _T(", ") <<
               _T("Channel1=") << pEl->Channel1 << _T(", ") <<
               _T("Channel2=") << pEl->Channel3 << _T("}");
    sTemp = sBuffer.str();
    if ( pMem )
    {
        CTL_StringType sTemp2;
        sTemp2 = pMem;
        sTemp2 += _T("=");
        sTemp = sTemp2 + sTemp;
    }
    return sTemp;
}

CTL_StringType DecodeTW_INFO(pTW_INFO pInfo, LPCTSTR pMem)
{
    CTL_StringType sTemp;
    CTL_StringStreamType sBuffer;
    sBuffer << _T("{InfoId=") << pInfo->InfoID << _T(", ") <<
               _T("ItemType=") << pInfo->ItemType << _T(", ") <<
               _T("NumItems=") << pInfo->NumItems << _T(", ") <<
               _T("ReturnCode=") << pInfo->ReturnCode << _T(", ") <<
               _T("Item=") << pInfo->Item << _T("}");
    sTemp = sBuffer.str();
    if ( pMem )
    {
        CTL_StringType sTemp2;
        sTemp2 = pMem;
        sTemp2 += _T("=");
        sTemp = sTemp2 + sTemp;
    }
    return sTemp;
}
