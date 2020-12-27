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
#ifndef ERRSTRUC_H_
#define ERRSTRUC_H_

#include "ctlreg.h"
#include <tuple>
#include <unordered_map>
#include "dtwdecl.h"
/* Structure types are as follows
   0 - NONE
   1 - TW_CUSTOMDSDATA
   2 - TW_DEVICEEVENT
   3 - TW_EVENT
   4 - TW_FILESYSTEM
   5 - TW_IDENTITY
   6 - LPHWND (Pointer to window handle)
   7 - TW_PASSTHRU
   8 - TW_PENDINGXFERS
   9 - TW_SETUPFILEXFER
  10 - TW_SETUPMEMXFER
  11 - TW_STATUS
  12 - TW_USERINTERFACE
  13 - pTW_UINT32
  14 - TW_CIECOLOR
  15 - TW_EXTIMAGEINFO
  16 - TW_GRAYRESPONSE
  17 - TW_IMAGEINFO
  18 - TW_IMAGELAYOUT
  19 - TW_IMAGEMEMXFER
  20 - Dib Pointer
  21 - TW_JPEGCOMPRESSION
  22 - TW_PALETTE8
  23 - TW_RGBRESPONSE
  24 - TW_TWUNKIDENTITY
  25 - TW_TWUNKDSENTRYPARAMS
  26 - TW_AUDIOINFO
  27 - pointer to WAV file
  28 - TW_CAPABILITY
*/
namespace dynarithmic
{
   enum ErrorStructTypes {  ERRSTRUCT_NONE,
                            ERRSTRUCT_TW_CUSTOMDSDATA           ,
                            ERRSTRUCT_TW_DEVICEEVENT            ,
                            ERRSTRUCT_TW_EVENT                  ,
                            ERRSTRUCT_TW_FILESYSTEM             ,
                            ERRSTRUCT_TW_IDENTITY               ,
                            ERRSTRUCT_LPHWND                    ,
                            ERRSTRUCT_TW_PASSTHRU               ,
                            ERRSTRUCT_TW_PENDINGXFERS           ,
                            ERRSTRUCT_TW_SETUPFILEXFER          ,
                            ERRSTRUCT_TW_SETUPMEMXFER           ,
                            ERRSTRUCT_TW_STATUS                 ,
                            ERRSTRUCT_TW_USERINTERFACE          ,
                            ERRSTRUCT_pTW_UINT32                ,
                            ERRSTRUCT_TW_CIECOLOR               ,
                            ERRSTRUCT_TW_EXTIMAGEINFO           ,
                            ERRSTRUCT_TW_GRAYRESPONSE           ,
                            ERRSTRUCT_TW_IMAGEINFO              ,
                            ERRSTRUCT_TW_IMAGELAYOUT            ,
                            ERRSTRUCT_TW_IMAGEMEMXFER           ,
                            ERRSTRUCT_HDIB                      ,
                            ERRSTRUCT_TW_JPEGCOMPRESSION        ,
                            ERRSTRUCT_TW_PALETTE8               ,
                            ERRSTRUCT_TW_RGBRESPONSE            ,
                            ERRSTRUCT_TW_TWUNKIDENTITY          ,
                            ERRSTRUCT_TW_TWUNKDSENTRYPARAMS     ,
                            ERRSTRUCT_TW_AUDIOINFO              ,
                            ERRSTRUCT_pWAV                      ,
                            ERRSTRUCT_TW_CAPABILITY,
                            ERRSTRUCT_DTWAIN_MESSAGE,
                            ERRSTRUCT_TW_STATUSUTF8,
                            ERRSTRUCT_TW_MEMORY,
                            ERRSTRUCT_TW_ENTRYPOINT,
                            ERRSTRUCT_TW_CALLBACK,
                            ERRSTRUCT_TW_CALLBACK2,
                            ERRSTRUCT_TW_TWAINDIRECT,
                            ERRSTRUCT_TW_METRICS
   };

   typedef std::unordered_map<WPARAM, CTL_StringType> CTL_ContainerToNameMap;

class CTL_ErrorStructDecoder {
    public:
        CTL_ErrorStructDecoder();
        void StartDecoder(pTW_IDENTITY pSource, pTW_IDENTITY pDest, LONG nDG, UINT nDAT, UINT nMSG, TW_MEMREF Data,
                          ErrorStructTypes sType);
        CTL_StringType DecodeBitmap(HANDLE hBitmap) const;
        CTL_StringType DecodePDFTextElement(PDFTextElement* pEl) const;
        CTL_StringType DecodeTWAINReturnCode(TW_UINT16 retCode) const;
        CTL_StringType DecodeTWAINCode(TW_UINT16 retCode, TW_UINT16 errStart, const CTL_StringType& defMessage) const;
        CTL_StringType DecodeTWAINReturnCodeCC(TW_UINT16 retCode) const;
        void StartMessageDecoder(HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam);
        const CTL_StringType& GetDecodedString() const { return m_pString; }
        static CTL_ContainerToNameMap s_mapContainerType;
        static CTL_ContainerToNameMap s_mapNotificationType;
        static std::unordered_map<TW_UINT32, CTL_StringType> s_mapSupportedGroups;
        static std::unordered_map<TW_UINT16, CTL_StringType> s_mapTwainDSMReturnCodes;

        static bool s_bInit;

    protected:
        CTL_StringType m_pString;
};

// Define the cap info structure used
class CTL_ErrorStruct
{
    public:
        typedef std::tuple<int, int, int> key_type;
        CTL_ErrorStruct() :
            m_pOrigin(nullptr),
            m_pDest(nullptr),
            m_pData(nullptr),
            m_nStructType(0),
            m_nTWCCErrorCodes(0),
            m_nTWRCCodes(0),
            m_Key(0,0,0) {}

        CTL_ErrorStruct(LONG nDG, UINT nDAT, UINT nMsg) :
                m_pOrigin(nullptr),
                m_pDest(nullptr),
                m_pData(nullptr),
                m_nStructType(0),
                m_nTWCCErrorCodes(0),
                m_nTWRCCodes(0),
                m_Key{nDG,nDAT,nMsg}
        {}

        void    SetKey(const key_type& nVal) { m_Key = nVal; }
        const   key_type&  GetKey() const { return m_Key; }
        LONG    GetDG() const { return std::get<0>(m_Key); }
        UINT    GetDAT() const { return std::get<1>(m_Key); }
        UINT    GetMSG() const { return std::get<2>(m_Key); }
        UINT    GetDataType() const { return m_nStructType; }
        void    SetDataType(UINT nType) { m_nStructType = nType; }
        LONG    GetFailureCodes() const { return m_nTWCCErrorCodes; }
        void    SetFailureCodes(LONG lFailureCodes) { m_nTWCCErrorCodes = lFailureCodes; }
        LONG    GetSuccessCodes() const { return m_nTWRCCodes; }
        void    SetSuccessCodes(LONG lSuccessCodes) { m_nTWRCCodes = lSuccessCodes; }
        bool    IsFailureMatch(TW_UINT16 cc);
        bool    IsSuccessMatch(TW_UINT16 rc);
        bool    IsValid() const { return GetDG() || GetDAT() || GetMSG(); }

        CTL_StringType GetIdentityAndDataInfo(pTW_IDENTITY pOrigin, pTW_IDENTITY pDest, TW_MEMREF pData)
                {
                    m_pOrigin = pOrigin; m_pDest = pDest; m_pData = pData;
                    m_Decoder.StartDecoder(m_pOrigin, m_pDest, (UINT)GetDG(),
                                           GetDAT(), GetMSG(), m_pData,
                                           (ErrorStructTypes)m_nStructType);
                    return m_Decoder.GetDecodedString();
                }

        CTL_StringType GetDTWAINMessageAndDataInfo(HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam)
                {
                    m_Decoder.StartMessageDecoder(hWnd, nMsg, wParam, lParam);
                    return m_Decoder.GetDecodedString();
                }

        CTL_StringType GetTWAINDSMError(TW_UINT16 retcode)
        {
            return m_Decoder.DecodeTWAINReturnCode(retcode);
        }

        CTL_StringType GetTWAINDSMErrorCC(TW_UINT16 retcode)
        {
            return m_Decoder.DecodeTWAINReturnCodeCC(retcode);
        }

    private:
        friend class CTL_ErrorStructDecoder;
        UINT       m_nStructType;
        LONG       m_nTWCCErrorCodes;
        LONG       m_nTWRCCodes;
        CTL_ErrorStructDecoder m_Decoder;
        pTW_IDENTITY m_pOrigin, m_pDest;
        TW_MEMREF  m_pData;
        key_type m_Key;
};
typedef std::unordered_map<CTL_ErrorStruct::key_type, CTL_ErrorStruct, boost::hash<CTL_ErrorStruct::key_type>> CTL_GeneralErrorInfo;
}
#endif


