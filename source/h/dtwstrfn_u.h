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
#ifndef DTWSTRFNU_H
#define DTWSTRFNU_H

DTWAIN_HANDLE  DLLENTRY_DEF      DTWAIN_SysInitializeEx(LPCWSTR szINIPath);
DTWAIN_HANDLE  DLLENTRY_DEF      DTWAIN_SysInitializeEx2(LPCWSTR szINIPath,
                                                         LPCWSTR szImageDLLPath,
                                                         LPCWSTR szLangResourcePath);
LONG           DLLENTRY_DEF      DTWAIN_GetVersionString(LPWSTR lpszVer, LONG nLength);
LONG           DLLENTRY_DEF      DTWAIN_GetLibraryPath(LPWSTR lpszVer, LONG nLength);
LONG           DLLENTRY_DEF      DTWAIN_GetShortVersionString(LPWSTR lpszVer, LONG nLength);
LONG           DLLENTRY_DEF      DTWAIN_GetVersionInfo(LPWSTR lpszVer, LONG nLength);
LONG           DLLENTRY_DEF      DTWAIN_GetErrorString(LONG lError,
                                                       LPWSTR lpszBuffer,
                                                       LONG nMaxLen);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_StartTwainSession(HWND hWndMsg, LPCWSTR lpszDLLName);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetAppInfo(LPCWSTR szVerStr, LPCWSTR szManu,
                                                   LPCWSTR szProdFam, LPCWSTR szProdName);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetAppInfo(LPWSTR szVerStr, LPWSTR szManu,
                                                   LPWSTR szProdFam, LPWSTR szProdName);
DTWAIN_SOURCE  DLLENTRY_DEF      DTWAIN_SelectSource2(HWND hWndParent, LPCWSTR szTitle,
                                                      LONG xPos, LONG yPos, LONG nOptions);
DTWAIN_SOURCE  DLLENTRY_DEF      DTWAIN_SelectSource2Ex(HWND hWndParent, LPCWSTR szTitle, LONG xPos, LONG yPos,
                                                        LPCWSTR szIncludeFilter,
                                                        LPCWSTR szExcludeFilter, 
                                                        LPCWSTR szNameMapping,
                                                        LONG nOptions)
DTWAIN_SOURCE  DLLENTRY_DEF      DTWAIN_SelectSourceByName(LPCWSTR lpszName);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceManufacturer(DTWAIN_SOURCE Source,
                                                              LPWSTR szProduct,
                                                              LONG nMaxLen);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceProductFamily(DTWAIN_SOURCE Source,
                                                               LPWSTR szProduct,
                                                               LONG nMaxLen);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceProductName(DTWAIN_SOURCE Source,
                                                             LPWSTR szProduct,
                                                             LONG nMaxLen);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetSourceVersionNumber(DTWAIN_SOURCE Source,
                                                               LPLONG pMajor,
                                                               LPLONG pMinor);
LONG           DLLENTRY_DEF      DTWAIN_GetSourceVersionInfo(DTWAIN_SOURCE Source,
                                                             LPWSTR szProduct,
                                                             LONG nMaxLen);
LONG           DLLENTRY_DEF      DTWAIN_GetNameFromCap(LONG nCapValue, LPWSTR szValue, LONG nMaxLen );
LONG           DLLENTRY_DEF      DTWAIN_GetCapFromName( LPCWSTR szName );
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreateFromStrings(LPCWSTR* pCArray, LONG nSize);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddString(DTWAIN_ARRAY pArray, LPCWSTR Val );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddStringN( DTWAIN_ARRAY pArray, LPCWSTR Val, LONG num );
LONG           DLLENTRY_DEF      DTWAIN_ArrayFindString( DTWAIN_ARRAY pArray, LPCWSTR pString );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayGetAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPWSTR pStr);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPCWSTR pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtStringN(DTWAIN_ARRAY pArray, LONG nWhere, LPCWSTR Val, LONG num);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtString(DTWAIN_ARRAY pArray, LONG nWhere, LPCWSTR pStr);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_AcquireFile(DTWAIN_SOURCE Source,
                                                    LPCWSTR   lpszFile,
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
LONG           DLLENTRY_DEF      DTWAIN_GetCurrentFileName( DTWAIN_SOURCE Source, LPWSTR szName, LONG MaxLen );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinterStrings(DTWAIN_SOURCE Source, DTWAIN_ARRAY ArrayString, LPLONG pNumStrings);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinterSuffixString(DTWAIN_SOURCE Source, LPCWSTR Suffix);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinterStrings(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY ArrayString );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinterSuffixString(DTWAIN_SOURCE Source, LPWSTR Suffix, LONG nMaxLen);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTwainLog(LONG LogFlags, LPCWSTR lpszLogFile);
DTWAIN_HANDLE DLLENTRY_DEF  DTWAIN_SysInitializeLibEx(HINSTANCE hInstance, LPCWSTR szINIPath);
DTWAIN_HANDLE DLLENTRY_DEF  DTWAIN_SysInitializeLibEx2(HINSTANCE hInstance, LPCWSTR szINIPath,
                                                       LPCWSTR szImageDLLPath,
                                                       LPCWSTR szLangResourcePath);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFAuthor(DTWAIN_SOURCE Source, LPCWSTR lpAuthor);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFProducer(DTWAIN_SOURCE Source, LPCWSTR lpProducer);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFCreator(DTWAIN_SOURCE Source, LPCWSTR lpCreator);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFTitle(DTWAIN_SOURCE Source, LPCWSTR lpTitle);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFSubject(DTWAIN_SOURCE Source, LPCWSTR lpSubject);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFKeywords(DTWAIN_SOURCE Source, LPCWSTR lpKeyWords);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFEncryption(DTWAIN_SOURCE Source, DTWAIN_BOOL bUseEncryption,
                                                 LPCWSTR lpszUser, LPCWSTR lpszOwner,
                                                 LONG Permissions, DTWAIN_BOOL UseStrongEncryption);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPostScriptTitle(DTWAIN_SOURCE Source, LPCWSTR szTitle);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddPDFText(DTWAIN_SOURCE Source,
                                           LPCWSTR szText, LONG xPos, LONG yPos,
                                           LPCWSTR fontName, DTWAIN_FLOAT fontSize, LONG colorRGB,
                                           LONG renderMode, DTWAIN_FLOAT scaling,
                                           DTWAIN_FLOAT charSpacing, DTWAIN_FLOAT wordSpacing,
                                           LONG strokeWidth, LONG Flags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPDFTextElementString(DTWAIN_PDFTEXTELEMENT TextElement, LPWSTR szData, LONG maxLen, LONG Flags);
LONG        DLLENTRY_DEF DTWAIN_GetPDFType1FontName(LONG FontVal, LPWSTR szFont, LONG nChars);
LONG DLLENTRY_DEF DTWAIN_GetExtCapFromName(LPCWSTR szName);
LONG DLLENTRY_DEF DTWAIN_GetExtNameFromCap( LONG nValue, LPWSTR szValue, LONG nMaxLen );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetFileSavePos(HWND hWndParent, LPCWSTR szTitle, LONG xPos, LONG yPos, LONG nFlags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_LoadCustomStringResources(LPCWSTR sLangDLL);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTempFileDirectory(LPCWSTR szFilePath);
LONG DLLENTRY_DEF DTWAIN_GetTempFileDirectory(LPWSTR szFilePath, LONG nMaxLen);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCamera(DTWAIN_SOURCE Source, LPCWSTR szCamera);
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_GetAuthor( DTWAIN_SOURCE Source, LPWSTR szAuthor );
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_SetAuthor( DTWAIN_SOURCE Source, LPCWSTR szAuthor );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCaption( DTWAIN_SOURCE Source, LPWSTR Caption );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCaption( DTWAIN_SOURCE Source, LPCWSTR Caption );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceTimeDate( DTWAIN_SOURCE Source, LPWSTR szTimeDate );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetDeviceTimeDate( DTWAIN_SOURCE Source, LPCWSTR szTimeDate );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetTimeDate( DTWAIN_SOURCE Source, LPWSTR szTimeDate );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_InitImageFileAppend(LPCWSTR szFile, LONG fType);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddFileToAppend(LPCWSTR szFile);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_LogMessage(LPCWSTR message);
LONG        DLLENTRY_DEF DTWAIN_TwainSave(LPCWSTR cmd);
LONG   DLLENTRY_DEF DTWAIN_GetOCRVersionInfo(DTWAIN_OCRENGINE Engine, LPWSTR buffer,
                                             LONG maxBufSize);
LONG   DLLENTRY_DEF DTWAIN_GetOCRProductName(DTWAIN_OCRENGINE Engine,
                                             LPWSTR szProductName,
                                             LONG nMaxLen);
LONG   DLLENTRY_DEF DTWAIN_GetOCRProductFamily(DTWAIN_OCRENGINE Engine,
                                               LPWSTR szProductFamily,
                                               LONG nMaxLen);
LONG   DLLENTRY_DEF DTWAIN_GetOCRManufacturer(DTWAIN_OCRENGINE Engine,
                                              LPWSTR szManufacturer,
                                              LONG nMaxLen);
DTWAIN_OCRENGINE DLLENTRY_DEF DTWAIN_SelectOCREngineByName(LPCWSTR lpszName);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ExecuteOCR(DTWAIN_OCRENGINE Engine,
                                           LPCWSTR szFileName,
                                           LONG nStartPage,
                                           LONG nEndPage);
HANDLE DLLENTRY_DEF DTWAIN_GetOCRText(DTWAIN_OCRENGINE Engine,
                                      LONG nPageNo,
                                      LPWSTR Data,
                                      LONG dSize,
                                      LPLONG pActualSize,
                                      LONG nFlags);
void DLLENTRY_DEF DTWAIN_X(LPCWSTR s);
#endif
