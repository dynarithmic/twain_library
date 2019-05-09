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
#ifndef DTWSTRFN_A
#define DTWSTRFN_A

DTWAIN_HANDLE  DLLENTRY_DEF      DTWAIN_SysInitializeEx(LPCSTR szINIPath);
DTWAIN_HANDLE  DLLENTRY_DEF      DTWAIN_SysInitializeEx2(LPCSTR szINIPath,
                                                         LPCSTR szImageDLLPath,
                                                         LPCSTR szLangResourcePath);
LONG           DLLENTRY_DEF      DTWAIN_GetVersionString(LPSTR lpszVer, LONG nLength);
LONG           DLLENTRY_DEF      DTWAIN_GetVersionInfo(LPSTR lpszVer, LONG nLength);
LONG           DLLENTRY_DEF      DTWAIN_GetErrorString(LONG lError,
                                                       LPSTR lpszBuffer,
                                                       LONG nMaxLen);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_StartTwainSession(HWND hWndMsg, LPCSTR lpszDLLName);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetAppInfo(LPCSTR szVerStr, LPCSTR szManu,
                                                   LPCSTR szProdFam, LPCSTR szProdName);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetAppInfo(LPSTR szVerStr, LPSTR szManu,
                                                   LPSTR szProdFam, LPSTR szProdName);
DTWAIN_SOURCE  DLLENTRY_DEF      DTWAIN_SelectSource2(HWND hWndParent, LPCSTR szTitle,
                                                      LONG xPos, LONG yPos, LONG nOptions);
DTWAIN_SOURCE  DLLENTRY_DEF      DTWAIN_SelectSourceByName(LPCSTR lpszName);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceManufacturer(DTWAIN_SOURCE Source,
                                                              LPSTR szProduct,
                                                              LONG nMaxLen);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceProductFamily(DTWAIN_SOURCE Source,
                                                               LPSTR szProduct,
                                                               LONG nMaxLen);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceProductName(DTWAIN_SOURCE Source,
                                                             LPSTR szProduct,
                                                             LONG nMaxLen);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetSourceVersionNumber(DTWAIN_SOURCE Source,
                                                               LPLONG pMajor,
                                                               LPLONG pMinor);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceVersionInfo(DTWAIN_SOURCE Source,
                                                             LPSTR szProduct,
                                                             LONG nMaxLen);
LONG           DLLENTRY_DEF      DTWAIN_GetNameFromCap(LONG nCapValue, LPSTR szValue, LONG nMaxLen );
LONG           DLLENTRY_DEF      DTWAIN_GetCapFromName( LPCSTR szName );
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreateFromStrings(LPCSTR* pCArray, LONG nSize);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddString(DTWAIN_ARRAY pArray, LPCSTR Val );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddStringN( DTWAIN_ARRAY pArray, LPCSTR Val, LONG num );
LONG           DLLENTRY_DEF      DTWAIN_ArrayFindString( DTWAIN_ARRAY pArray, LPCSTR pString );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayGetAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPSTR pStr);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPCSTR pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtStringN(DTWAIN_ARRAY pArray, LONG nWhere, LPCSTR Val, LONG num);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPCSTR pStr);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_AcquireFile(DTWAIN_SOURCE Source,
                                                    LPCSTR   lpszFile,
                                                    LONG     lFileType,
                                                    LONG     lFileFlags,
                                                    LONG     PixelType,
                                                    LONG     lMaxPages,
                                                    DTWAIN_BOOL bShowUI,
                                                    DTWAIN_BOOL bCloseSource,
                                                    LPLONG pStatus);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_AcquireFileEx(DTWAIN_SOURCE Source,
                                                      DTWAIN_ARRAY aFileNames,
                                                      LONG     lFileType,
                                                      LONG     lFileFlags,
                                                      LONG     PixelType,
                                                      LONG     lMaxPages,
                                                      DTWAIN_BOOL bShowUI,
                                                      DTWAIN_BOOL bCloseSource,
                                                      LPLONG pStatus);
LONG           DLLENTRY_DEF      DTWAIN_GetCurrentFileName( DTWAIN_SOURCE Source, LPSTR szName, LONG MaxLen );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinterStrings(DTWAIN_SOURCE Source, DTWAIN_ARRAY ArrayString, LPLONG pNumStrings);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinterSuffixString(DTWAIN_SOURCE Source, LPCSTR Suffix);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinterStrings(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY ArrayString );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinterSuffixString(DTWAIN_SOURCE Source, LPSTR Suffix, LONG nMaxLen);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTwainLog(LONG LogFlags, LPCSTR lpszLogFile);
DTWAIN_HANDLE DLLENTRY_DEF  DTWAIN_SysInitializeLibEx(HINSTANCE hInstance, LPCSTR szINIPath);
DTWAIN_HANDLE DLLENTRY_DEF  DTWAIN_SysInitializeLibEx2(HINSTANCE hInstance, LPCSTR szINIPath,
                                                       LPCSTR szImageDLLPath,
                                                       LPCSTR szLangResourcePath);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFAuthor(DTWAIN_SOURCE Source, LPCSTR lpAuthor);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFProducer(DTWAIN_SOURCE Source, LPCSTR lpProducer);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFCreator(DTWAIN_SOURCE Source, LPCSTR lpCreator);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFTitle(DTWAIN_SOURCE Source, LPCSTR lpTitle);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFSubject(DTWAIN_SOURCE Source, LPCSTR lpSubject);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFKeywords(DTWAIN_SOURCE Source, LPCSTR lpKeyWords);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFEncryption(DTWAIN_SOURCE Source, DTWAIN_BOOL bUseEncryption,
                                                 LPCSTR lpszUser, LPCSTR lpszOwner,
                                                 LONG Permissions, DTWAIN_BOOL UseStrongEncryption);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPostScriptTitle(DTWAIN_SOURCE Source, LPCSTR szTitle);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddPDFText(DTWAIN_SOURCE Source,
                                           LPCSTR szText, LONG xPos, LONG yPos,
                                           LPCSTR fontName, DTWAIN_FLOAT fontSize, LONG colorRGB,
                                           LONG renderMode, DTWAIN_FLOAT scaling,
                                           DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing,
                                           LONG strokeWidth, LONG Flags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPDFTextElementString(DTWAIN_PDFTEXTELEMENT TextElement, LPSTR szData, LONG maxLen, LONG Flags);
LONG        DLLENTRY_DEF DTWAIN_GetPDFType1FontName(LONG FontVal, LPSTR szFont, LONG nChars);
LONG DLLENTRY_DEF DTWAIN_GetExtCapFromName(LPCSTR szName);
LONG DLLENTRY_DEF DTWAIN_GetExtNameFromCap( LONG nValue, LPSTR szValue, LONG nMaxLen );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetFileSavePos(HWND hWndParent, LPCSTR szTitle, LONG xPos, LONG yPos, LONG nFlags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_LoadCustomStringResources(LPCSTR sLangDLL);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTempFileDirectory(LPCSTR szFilePath);
LONG DLLENTRY_DEF DTWAIN_GetTempFileDirectory(LPSTR szFilePath, LONG nMaxLen);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCamera(DTWAIN_SOURCE Source, LPCSTR szCamera);
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_GetAuthor( DTWAIN_SOURCE Source, LPSTR szAuthor );
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_SetAuthor( DTWAIN_SOURCE Source, LPCSTR szAuthor );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCaption( DTWAIN_SOURCE Source, LPSTR Caption );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCaption( DTWAIN_SOURCE Source, LPCSTR Caption );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceTimeDate( DTWAIN_SOURCE Source, LPSTR szTimeDate );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetDeviceTimeDate( DTWAIN_SOURCE Source, LPCSTR szTimeDate );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetTimeDate( DTWAIN_SOURCE Source, LPSTR szTimeDate );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_InitImageFileAppend(LPCSTR szFile, LONG fType);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddFileToAppend(LPCSTR szFile);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_LogMessage(LPCSTR message);
LONG        DLLENTRY_DEF DTWAIN_TwainSave(LPCSTR cmd);
LONG   DLLENTRY_DEF DTWAIN_GetOCRVersionInfo(DTWAIN_OCRENGINE Engine, LPSTR buffer,
                                             LONG maxBufSize);
LONG   DLLENTRY_DEF DTWAIN_GetOCRProductName(DTWAIN_OCRENGINE Engine,
                                             LPSTR szProductName,
                                             LONG nMaxLen);
LONG   DLLENTRY_DEF DTWAIN_GetOCRProductFamily(DTWAIN_OCRENGINE Engine,
                                             LPSTR szProductFamily,
                                             LONG nMaxLen);
LONG   DLLENTRY_DEF DTWAIN_GetOCRManufacturer(DTWAIN_OCRENGINE Engine,
                                               LPSTR szManufacturer,
                                               LONG nMaxLen);
DTWAIN_OCRENGINE DLLENTRY_DEF DTWAIN_SelectOCREngineByName(LPCSTR lpszName);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ExecuteOCR(DTWAIN_OCRENGINE Engine,
                                           LPCSTR szFileName,
                                           LONG nStartPage,
                                           LONG nEndPage);
HANDLE DLLENTRY_DEF DTWAIN_GetOCRText(DTWAIN_OCRENGINE Engine,
                                      LONG nPageNo,
                                      LPSTR Data,
                                      LONG dSize,
                                      LPLONG pActualSize,
                                      LONG nFlags);
void DLLENTRY_DEF DTWAIN_X(LPCSTR s);
#endif
