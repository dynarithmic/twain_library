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
#ifndef DTWAINX_H
#define DTWAINX_H

/* Include the basic definitions used by TWAIN */
#include "twain.h"

/* ///////////////////////////////// DTWAIN Exported functions //////////////////////////// */
#ifdef __cplusplus
  extern "C" {
#endif
/* Start of DTWAIN functional interface */
/* Check for TWAIN Availability (initialization not necessary) */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsTwainAvailable(VOID_PROTOTYPE);

/* DTWAIN Initialization / Deinitialization */
DTWAIN_HANDLE  DLLENTRY_DEF      DTWAIN_SysInitialize(VOID_PROTOTYPE);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SysDestroy(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsInitialized(VOID_PROTOTYPE);
LONG           DLLENTRY_DEF      DTWAIN_GetRegisteredMsg(VOID_PROTOTYPE);
DTWAIN_HANDLE  DLLENTRY_DEF      DTWAIN_GetDTWAINHandle(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetVersion(LPLONG lpMajor, LPLONG lpMinor,
                                                   LPLONG lpVersionType);

LONG           DLLENTRY_DEF      DTWAIN_GetStaticLibVersion(VOID_PROTOTYPE);

/* DTWAIN Error message handling */
LONG           DLLENTRY_DEF      DTWAIN_GetLastError(VOID_PROTOTYPE);

/* Modal / Modeless TWAIN message operation */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetTwainMode(LONG lAcquireMode);
LONG           DLLENTRY_DEF      DTWAIN_GetTwainMode(VOID_PROTOTYPE);

/* TWAIN Session Management */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsSessionEnabled(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EndTwainSession(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetCountry(LONG nCountry);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetLanguage(LONG nLanguage);
LONG           DLLENTRY_DEF      DTWAIN_GetCountry(VOID_PROTOTYPE);
LONG           DLLENTRY_DEF      DTWAIN_GetLanguage(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsTwainMsg(MSG* pMsg);

/* DTWAIN Message Notification functions */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EnableMsgNotify(DTWAIN_BOOL bSet);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsMsgNotifyEnabled(VOID_PROTOTYPE);

/* Callback procedure for alternate DTWAIN message notification */
DTWAIN_CALLBACK_PROC DLLENTRY_DEF DTWAIN_SetCallback(DTWAIN_CALLBACK_PROC Fn,LONG UserData);
DTWAIN_CALLBACK_PROC DLLENTRY_DEF DTWAIN_GetCallback(VOID_PROTOTYPE);
LONG DLLENTRY_DEF DTWAIN_CallCallback(WPARAM wParam, LPARAM lParam, LONG UserData);

/* 64-bit LONG type callbacks */
DTWAIN_CALLBACK_PROC64 DLLENTRY_DEF DTWAIN_SetCallback64(DTWAIN_CALLBACK_PROC64 Fn,DTWAIN_LONG64 UserData);
DTWAIN_CALLBACK_PROC64 DLLENTRY_DEF DTWAIN_GetCallback64(VOID_PROTOTYPE);
LONG DLLENTRY_DEF DTWAIN_CallCallback64(WPARAM wParam, LPARAM lParam, LONGLONG UserData);

/* Add callbacks in a queue (main callback is always first - Not implemented in this version) */
/*DTWAIN_BOOL       DLLENTRY_DEF       DTWAIN_AddCallback(DTWAIN_CALLBACK_PROC Fn,
                                                     LONG UserData);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_RemoveCallback(DTWAIN_CALLBACK_PROC Fn);
LONG           DLLENTRY_DEF       DTWAIN_GetNumCallbacks(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_RemoveAllCallbacks(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsCallback(DTWAIN_CALLBACK_PROC Fn);*/

/* Handle to TWAIN Message Window */
HWND           DLLENTRY_DEF      DTWAIN_GetTwainHwnd(VOID_PROTOTYPE);

/* General Source functions */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_OpenSource(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_CloseSource(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EnumSources(LPDTWAIN_ARRAY lpArray);
DTWAIN_ARRAY   DLLENTRY_DEF		 DTWAIN_EnumSourcesEx(VOID_PROTOTYPE);

DTWAIN_SOURCE  DLLENTRY_DEF      DTWAIN_SelectSource(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_CloseSourceUI( DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsSourceSelected(DTWAIN_SOURCE Source);
DTWAIN_SOURCE  DLLENTRY_DEF      DTWAIN_SelectDefaultSource(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetSourceVersionNumber(DTWAIN_SOURCE Source,LPLONG pMajor,LPLONG pMinor);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetDefaultSource( DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsSourceAcquiring( DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsSourceOpen( DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsAcquiring(VOID_PROTOTYPE);

/*  Capability functions.  Source must be opened before using them!!! */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetCapValues(DTWAIN_SOURCE Source,
                                                 LONG lCap,
                                                 LONG lSetType,
                                                 DTWAIN_ARRAY pArray
                                                 );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetCapValuesEx(DTWAIN_SOURCE Source,
                                                   LONG lCap,
                                                   LONG lSetType,
                                                   LONG lContainerType,
                                                   DTWAIN_ARRAY pArray
                                                   );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetCapValuesEx2(DTWAIN_SOURCE Source,
                                                        LONG lCap,
                                                        LONG lSetType,
                                                        LONG lContainerType,
                                                        LONG nDataType,
                                                        DTWAIN_ARRAY pArray );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetCapValues(DTWAIN_SOURCE Source,
                                                 LONG  lCap,
                                                 LONG  lGetType,
                                                 LPDTWAIN_ARRAY pArray
                                                 );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetCapValuesEx(DTWAIN_SOURCE Source,
                                                 LONG  lCap,
                                                 LONG  lGetType,
                                                 LONG  lContainerType,
                                                 LPDTWAIN_ARRAY pArray
                                                 );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetCapValuesEx2( DTWAIN_SOURCE Source,
                                                        LONG lCap,
                                                        LONG lGetType,
                                                        LONG lContainerType,
                                                        LONG nDataType,
                                                        LPDTWAIN_ARRAY pArray );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EnumSupportedCaps(DTWAIN_SOURCE Source,
                                                           LPDTWAIN_ARRAY pArray );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EnumExtendedCaps( DTWAIN_SOURCE Source,
                                                           LPDTWAIN_ARRAY pArray );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EnumCustomCaps( DTWAIN_SOURCE Source,
                                                         LPDTWAIN_ARRAY pArray );

DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_EnumSupportedCapsEx2(DTWAIN_SOURCE Source);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_EnumExtendedCapsEx2(DTWAIN_SOURCE Source);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_EnumCustomCapsEx2(DTWAIN_SOURCE Source);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsCapSupported(DTWAIN_SOURCE Source,
                                                      LONG lCapability );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetCapOperations(DTWAIN_SOURCE Source,
                                                           LONG lCapability,
                                                           LPLONG  lpOps);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetAllCapsToDefault(DTWAIN_SOURCE Source);
LONG           DLLENTRY_DEF      DTWAIN_GetCapArrayType(DTWAIN_SOURCE Source, LONG nCap);
LONG           DLLENTRY_DEF      DTWAIN_GetCapDataType(DTWAIN_SOURCE Source, LONG nCap);
LONG           DLLENTRY_DEF      DTWAIN_GetCapContainerEx(LONG nCap,
                                                          DTWAIN_BOOL bSetContainer,
                                                          LPDTWAIN_ARRAY ConTypes);
LONG           DLLENTRY_DEF      DTWAIN_GetCapContainer(DTWAIN_SOURCE Source, LONG nCap,
                                                            LONG lCapType);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetAcquireArea(DTWAIN_SOURCE Source,
                                                        LONG lGetType,
                                                        LPDTWAIN_ARRAY FloatEnum);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetAcquireArea(DTWAIN_SOURCE Source,
                                                        LONG lSetType,
                                                        DTWAIN_ARRAY FloatEnum,
                                                        DTWAIN_ARRAY ActualEnum);

/* DTWAIN Array functions */
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreate(LONG nEnumType,LONG nInitialSize );
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreateFromLongs(LPLONG pCArray, LONG nSize);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreateFromLong64s(LPLONG64 pCArray, LONG nSize);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreateFromReals(double* pCArray, LONG nSize);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreateCopy( DTWAIN_ARRAY Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayCopy(DTWAIN_ARRAY Source,DTWAIN_ARRAY Dest);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAdd(DTWAIN_ARRAY pArray, LPVOID pVariant );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddLong(DTWAIN_ARRAY pArray, LONG Val );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddLong64( DTWAIN_ARRAY pArray, LONG64 Val );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddFloat(DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddN( DTWAIN_ARRAY pArray, LPVOID pVariant, LONG num );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddLongN( DTWAIN_ARRAY pArray, LONG Val, LONG num );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddLong64N( DTWAIN_ARRAY pArray, LONG64 Val, LONG num );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayAddFloatN( DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, LONG num );

LONG           DLLENTRY_DEF      DTWAIN_ArrayFind( DTWAIN_ARRAY pArray, LPVOID pVariant );
LONG           DLLENTRY_DEF      DTWAIN_ArrayFindLong( DTWAIN_ARRAY pArray, LONG Val );
LONG           DLLENTRY_DEF      DTWAIN_ArrayFindLong64( DTWAIN_ARRAY pArray, LONG64 Val );
LONG           DLLENTRY_DEF      DTWAIN_ArrayFindFloat( DTWAIN_ARRAY pArray, DTWAIN_FLOAT Val, DTWAIN_FLOAT Tolerance );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayGetAt(DTWAIN_ARRAY pArray, LONG nWhere, LPVOID pVariant);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayGetAtLong(DTWAIN_ARRAY pArray, LONG nWhere, LPLONG pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayGetAtLong64(DTWAIN_ARRAY pArray, LONG nWhere, LPLONG64 pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayGetAtFloat(DTWAIN_ARRAY pArray, LONG nWhere, LPDTWAIN_FLOAT pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayGetSourceAt(DTWAIN_ARRAY pArray, LONG nWhere, DTWAIN_SOURCE* ppSource);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAt(DTWAIN_ARRAY pArray, LONG nWhere, LPVOID pVariant);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLong(DTWAIN_ARRAY pArray, LONG nWhere, LONG pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLong64(DTWAIN_ARRAY pArray, LONG nWhere, LONG64 Val);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtFloat(DTWAIN_ARRAY pArray, LONG nWhere,  DTWAIN_FLOAT pVal);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtN(DTWAIN_ARRAY pArray, LONG nWhere, LPVOID pVariant, LONG num);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLongN(DTWAIN_ARRAY pArray, LONG nWhere, LONG pVal, LONG num);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtLong64N(DTWAIN_ARRAY pArray, LONG nWhere, LONG64 Val, LONG num);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayInsertAtFloatN(DTWAIN_ARRAY pArray, LONG nWhere, DTWAIN_FLOAT Val, LONG num);

LONG           DLLENTRY_DEF      DTWAIN_ArrayGetCount( DTWAIN_ARRAY pArray );
LONG           DLLENTRY_DEF      DTWAIN_ArrayGetType(DTWAIN_ARRAY pArray);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayRemoveAll( DTWAIN_ARRAY pArray );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayRemoveAt(DTWAIN_ARRAY pArray, LONG nWhere);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayRemoveAtN(DTWAIN_ARRAY pArray, LONG nWhere, LONG num);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayResize(DTWAIN_ARRAY pArray, LONG NewSize);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAt( DTWAIN_ARRAY pArray, LONG lPos, LPVOID pVariant );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtLong(DTWAIN_ARRAY pArray, LONG nWhere, LONG pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtLong64(DTWAIN_ARRAY pArray, LONG nWhere, LONG64 Val);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArraySetAtFloat(DTWAIN_ARRAY pArray, LONG nWhere, DTWAIN_FLOAT pVal);

LPVOID         DLLENTRY_DEF      DTWAIN_ArrayGetBuffer( DTWAIN_ARRAY pArray, LONG nPos );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_ArrayDestroy( DTWAIN_ARRAY pArray);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_ArrayCreateFromCap(DTWAIN_SOURCE Source, LONG lCapType, LONG lSize);

/* DTWAIN Range functions */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetAll( DTWAIN_RANGE pArray, LPVOID pVariantLow,
                                                     LPVOID pVariantUp, LPVOID pVariantStep,
                                                     LPVOID pVariantDefault,
                                                     LPVOID pVariantCurrent );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetAllLong( DTWAIN_RANGE pArray, LONG lLow,
                                                            LONG lUp, LONG lStep,
                                                            LONG lDefault,
                                                            LONG lCurrent );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetAllFloat( DTWAIN_RANGE pArray, DTWAIN_FLOAT dLow,
                                                           DTWAIN_FLOAT dUp, DTWAIN_FLOAT dStep,
                                                           DTWAIN_FLOAT dDefault,
                                                           DTWAIN_FLOAT dCurrent );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetAll( DTWAIN_RANGE pArray, LPVOID pVariantLow,
                                                   LPVOID pVariantUp, LPVOID pVariantStep,
                                                   LPVOID pVariantDefault,
                                                   LPVOID pVariantCurrent );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetAllLong( DTWAIN_RANGE pArray, LPLONG pVariantLow,
                                                            LPLONG pVariantUp, LPLONG pVariantStep,
                                                            LPLONG pVariantDefault,
                                                            LPLONG pVariantCurrent );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetAllFloat( DTWAIN_RANGE pArray, LPDTWAIN_FLOAT pVariantLow,
                                                           LPDTWAIN_FLOAT pVariantUp, LPDTWAIN_FLOAT pVariantStep,
                                                           LPDTWAIN_FLOAT pVariantDefault,
                                                           LPDTWAIN_FLOAT pVariantCurrent );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetValue( DTWAIN_RANGE pArray, LONG nWhich,
                                                     LPVOID pVariant);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetValueLong( DTWAIN_RANGE pArray, LONG nWhich,
                                                              LPLONG pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetValueFloat( DTWAIN_RANGE pArray, LONG nWhich,
                                                             LPDTWAIN_FLOAT pVal);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeIsValid( DTWAIN_RANGE Range, LPLONG pStatus );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetValue( DTWAIN_RANGE pArray, LONG nWhich,
                                                       LPVOID pVal);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetValueLong( DTWAIN_RANGE pArray, LONG nWhich,
                                                              LONG Val);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeSetValueFloat( DTWAIN_RANGE pArray, LONG nWhich,
                                                             DTWAIN_FLOAT Val);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetPos( DTWAIN_RANGE pArray, LPVOID pVariant, LPLONG pPos );

DTWAIN_RANGE   DLLENTRY_DEF      DTWAIN_RangeCreate(LONG nEnumType);
DTWAIN_RANGE   DLLENTRY_DEF      DTWAIN_RangeCreateFromCap(DTWAIN_SOURCE Source, LONG lCapType);
LONG           DLLENTRY_DEF      DTWAIN_RangeGetCount( DTWAIN_RANGE pArray );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetExpValue( DTWAIN_RANGE pArray, LONG lPos, LPVOID pVariant );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetExpValueLong( DTWAIN_RANGE pArray, LONG lPos, LPLONG pVal );
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetExpValueFloat( DTWAIN_RANGE pArray, LONG lPos, LPDTWAIN_FLOAT pVal );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeExpand(DTWAIN_RANGE pSource,
                                                    LPDTWAIN_ARRAY pArray );

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeGetNearestValue( DTWAIN_RANGE pArray, LPVOID pVariantIn,
                                                                  LPVOID pVariantOut, LONG RoundType);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeNearestValueLong( DTWAIN_RANGE pArray, LONG lIn,
                                                                    LPLONG pOut, LONG RoundType);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeNearestValueFloat( DTWAIN_RANGE pArray, DTWAIN_FLOAT dIn,
                                                                    LPDTWAIN_FLOAT pOut, LONG RoundType);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_RangeDestroy(DTWAIN_RANGE pSource);

/* DTWAIN Frame functions */
DTWAIN_FRAME   DLLENTRY_DEF      DTWAIN_FrameCreate(DTWAIN_FLOAT Left, DTWAIN_FLOAT Top, DTWAIN_FLOAT Right,
                                                  DTWAIN_FLOAT Bottom);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_FrameDestroy(DTWAIN_FRAME Frame);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_FrameSetAll(DTWAIN_FRAME Frame,DTWAIN_FLOAT Left,
                                                  DTWAIN_FLOAT Top, DTWAIN_FLOAT Right,
                                                  DTWAIN_FLOAT Bottom);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_FrameGetAll(DTWAIN_FRAME Frame,LPDTWAIN_FLOAT Left,
                                                  LPDTWAIN_FLOAT Top, LPDTWAIN_FLOAT Right,
                                                  LPDTWAIN_FLOAT Bottom);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_FrameGetValue(DTWAIN_FRAME Frame, LONG nWhich, LPDTWAIN_FLOAT Value);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_FrameSetValue(DTWAIN_FRAME Frame, LONG nWhich, DTWAIN_FLOAT Value);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_FrameIsValid(DTWAIN_FRAME Frame);

/* DTWAIN Acquisition functions */
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_AcquireNative(DTWAIN_SOURCE Source,
                                                      LONG PixelType,
                                                      LONG nMaxPages,
                                                      DTWAIN_BOOL bShowUI,
                                                      DTWAIN_BOOL bCloseSource,
                                                      LPLONG pStatus);

DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_AcquireBuffered(DTWAIN_SOURCE Source,
                                                        LONG PixelType,
                                                        LONG nMaxPages,
                                                        DTWAIN_BOOL bShowUI,
                                                        DTWAIN_BOOL bCloseSource,
                                                        LPLONG pStatus);

DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_AcquireToClipboard(DTWAIN_SOURCE Source,
                                                           LONG PixelType,
                                                           LONG nMaxPages,
                                                           LONG nTransferMode,
                                                           DTWAIN_BOOL bDiscardDibs,
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

/* Getting acquired images after successful acquisition */
LONG           DLLENTRY_DEF      DTWAIN_GetNumAcquisitions( DTWAIN_ARRAY aAcq);
LONG           DLLENTRY_DEF      DTWAIN_GetNumAcquiredImages( DTWAIN_ARRAY aAcq, LONG nWhich );
HANDLE         DLLENTRY_DEF      DTWAIN_GetAcquiredImage( DTWAIN_ARRAY aAcq, LONG nWhichAcq, LONG nWhichDib );
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_GetAcquiredImageArray(DTWAIN_ARRAY aAcq, LONG nWhichAcq);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_CreateAcquisitionArray(VOID_PROTOTYPE);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_DestroyAcquisitionArray( DTWAIN_ARRAY aAcq, DTWAIN_BOOL bDestroyData );

/* Acquisition functions that fill a DTWAIN_ARRAY with image data */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_AcquireNativeEx(DTWAIN_SOURCE Source,
                                                        LONG PixelType,
                                                        LONG nMaxPages,
                                                        DTWAIN_BOOL bShowUI,
                                                        DTWAIN_BOOL bCloseSource,
                                                        DTWAIN_ARRAY Acquisitions,
                                                        LPLONG pStatus);

DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_AcquireBufferedEx(DTWAIN_SOURCE Source,
                                                          LONG PixelType,
                                                          LONG nMaxPages,
                                                          DTWAIN_BOOL bShowUI,
                                                          DTWAIN_BOOL bCloseSource,
                                                          DTWAIN_ARRAY Acquisitions,
                                                          LPLONG pStatus);

HANDLE         DLLENTRY_DEF      DTWAIN_GetCurrentAcquiredImage( DTWAIN_SOURCE Source );
LONG           DLLENTRY_DEF      DTWAIN_GetCurrentPageNum( DTWAIN_SOURCE Source );
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_GetSourceAcquisitions(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetMaxAcquisitions( DTWAIN_SOURCE Source, LONG MaxAcquires);
LONG           DLLENTRY_DEF      DTWAIN_GetMaxAcquisitions( DTWAIN_SOURCE Source);
LONG           DLLENTRY_DEF      DTWAIN_GetMaxPagesToAcquire(DTWAIN_SOURCE Source);

/* Setting the Source's unit of measure */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_GetSourceUnit(DTWAIN_SOURCE Source, LPLONG lpUnit);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetSourceUnit(DTWAIN_SOURCE Source, LONG Unit);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EnumSourceUnits(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY lpArray);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_EnumSourceUnitsEx(DTWAIN_SOURCE Source);


/* File acquisition query of formats supported */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_EnumFileXferFormats(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY   DLLENTRY_DEF      DTWAIN_EnumFileXferFormatsEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_IsFileXferSupported(DTWAIN_SOURCE Source, LONG lFileType);

/* The DTWAIN_SetFileXferFormat function does not need to be called, since DTWAIN does
 * this internally when a file transfer is done */
DTWAIN_BOOL    DLLENTRY_DEF      DTWAIN_SetFileXferFormat(DTWAIN_SOURCE Source, LONG lFileType, DTWAIN_BOOL bSetCurrent);


/* Image information for TWAIN State 6 */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetImageInfo(DTWAIN_SOURCE Source,
                                                     LPDTWAIN_FLOAT lpXResolution,
                                                     LPDTWAIN_FLOAT lpYResolution,
                                                     LPLONG lpWidth,
                                                     LPLONG lpLength,
                                                     LPLONG lpNumSamples,
                                                     LPDTWAIN_ARRAY lpBitsPerSample,
                                                     LPLONG lpBitsPerPixel,
                                                     LPLONG lpPlanar,
                                                     LPLONG lpPixelType,
                                                     LPLONG lpCompression);

/* Some Source setting functions. Convenient "quick and dirty" setting of most used
   values */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_SetContrast( DTWAIN_SOURCE Source,
                                                         DTWAIN_FLOAT Contrast);
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetContrast( DTWAIN_SOURCE Source,
                                                         LPDTWAIN_FLOAT Contrast);

LONG                 DLLENTRY_DEF    DTWAIN_EnumContrastValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY         DLLENTRY_DEF    DTWAIN_EnumContrastValuesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);

DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_SetBrightness( DTWAIN_SOURCE Source,
                                                           DTWAIN_FLOAT Brightness);
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetBrightness( DTWAIN_SOURCE Source,
                                                           LPDTWAIN_FLOAT Brightness);
LONG                 DLLENTRY_DEF    DTWAIN_EnumBrightnessValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY		 DLLENTRY_DEF    DTWAIN_EnumBrightnessValuesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);

DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_SetResolution( DTWAIN_SOURCE Source,
                                                           DTWAIN_FLOAT Resolution);

DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetResolution( DTWAIN_SOURCE Source,
                                                           LPDTWAIN_FLOAT Resolution);

DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetXResolution(DTWAIN_SOURCE Source, LPDTWAIN_FLOAT Resolution);
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetYResolution(DTWAIN_SOURCE Source, LPDTWAIN_FLOAT Resolution);

LONG                 DLLENTRY_DEF    DTWAIN_EnumResolutionValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY         DLLENTRY_DEF    DTWAIN_EnumResolutionValuesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);

/* Source UI functions */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_IsUIControllable( DTWAIN_SOURCE Source);
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_IsUIEnabled(DTWAIN_SOURCE Source);

/* Progress indicator */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_IsIndicatorSupported( DTWAIN_SOURCE Source );
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_IsIndicatorEnabled( DTWAIN_SOURCE Source );
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_EnableIndicator( DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable );

/* Thumbnail images (TWAIN compliant devices supporting thumbnail images (mostly digital cameras) */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_IsThumbnailSupported( DTWAIN_SOURCE Source );
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_IsThumbnailEnabled( DTWAIN_SOURCE Source );
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_EnableThumbnail(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable );

/* Scale acquired images (Can be used for thumbnailing acquired images) */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_SetAcquireImageScale( DTWAIN_SOURCE Source, DTWAIN_FLOAT xscale,
                                                                  DTWAIN_FLOAT yscale);

/* Force bits-per-pixel for acquired images */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_ForceAcquireBitDepth( DTWAIN_SOURCE Source, LONG BitDepth );
//DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_ForceGrayScale( DTWAIN_SOURCE Source );

/* Setting special events from Source */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetDeviceNotifications(DTWAIN_SOURCE Source, LONG DevEvents);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceNotifications(DTWAIN_SOURCE Source, LPLONG DevEvents);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceEvent(DTWAIN_SOURCE Source, LPLONG lpEvent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceEventEx(DTWAIN_SOURCE Source, LPLONG lpEvent, LPDTWAIN_ARRAY pArray);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDeviceEventInfo(DTWAIN_SOURCE Source, LONG nWhichInfo, LPVOID pValue);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsDeviceEventSupported(DTWAIN_SOURCE Source);


/* Showing the UI only (some Sources support this) */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsUIOnlySupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ShowUIOnly(DTWAIN_SOURCE Source);

/* Used if the TWAIN DSM is to be called directly by the application */
DTWAIN_IDENTITY  DLLENTRY_DEF DTWAIN_GetTwainAppID(VOID_PROTOTYPE);

DTWAIN_IDENTITY DLLENTRY_DEF DTWAIN_GetSourceID(DTWAIN_SOURCE Source);
LONG             DLLENTRY_DEF DTWAIN_CallDSMProc(DTWAIN_IDENTITY AppID, DTWAIN_IDENTITY SourceId,
                                                 LONG lDG, LONG lDAT, LONG lMSG, LPVOID pData);

/* Compression support for buffered transfer */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCompressionType(DTWAIN_SOURCE Source, LPLONG lpCompression, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCompressionType(DTWAIN_SOURCE Source, LONG lCompression, DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumCompressionTypes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumCompressionTypesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetCompressionSize( DTWAIN_SOURCE Source, LPLONG lBytes );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsCompressionSupported( DTWAIN_SOURCE Source, LONG Compression);

/* Imprinter / Endorser functions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPrinterStringModes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPrinterStringModesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumTwainPrinters(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY lpAvailPrinters);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumTwainPrintersEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumTwainPrintersArray(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumTwainPrintersArrayEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAvailablePrinters(DTWAIN_SOURCE Source, LONG lpAvailPrinters);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAvailablePrintersArray(DTWAIN_SOURCE Source, DTWAIN_ARRAY AvailPrinters);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnablePrinter(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinter(DTWAIN_SOURCE Source, LPLONG lpPrinter, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinter(DTWAIN_SOURCE Source, LONG Printer, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPrinterEnabled(DTWAIN_SOURCE Source, LONG Printer);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPrinterSupported(DTWAIN_SOURCE Source);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinterStartNumber(DTWAIN_SOURCE Source,  LONG nStart);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinterStringMode(DTWAIN_SOURCE Source, LONG PrinterMode, DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPrinterStrings(DTWAIN_SOURCE Source, DTWAIN_ARRAY ArrayString, LPLONG pNumStrings);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinterStartNumber(DTWAIN_SOURCE Source,  LPLONG nStart);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinterStringMode(DTWAIN_SOURCE Source, LPLONG PrinterMode, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPrinterStrings(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY ArrayString );

/* Document feeder functions */
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_EnableFeeder(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet );
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsFeederEnabled(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsFeederLoaded(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsFeederSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsFeederSensitive(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_EnableAutomaticSenseMedium(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsAutomaticSenseMediumEnabled(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsAutomaticSenseMediumSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_FeedPage(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_RewindPage(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_ClearPage(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_EnableAutoFeed(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_EnumAutoFeedValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY   DLLENTRY_DEF       DTWAIN_EnumAutoFeedValuesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsAutoFeedEnabled(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsAutoFeedSupported(DTWAIN_SOURCE Source);
LONG           DLLENTRY_DEF       DTWAIN_GetFeederFuncs(DTWAIN_SOURCE Source);
DTWAIN_BOOL    DLLENTRY_DEF       DTWAIN_IsPaperDetectable(DTWAIN_SOURCE Source);


/* Duplex Scanner support */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDuplexType(DTWAIN_SOURCE Source, LPLONG lpDupType);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableDuplex(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsDuplexSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsDuplexEnabled(DTWAIN_SOURCE Source);

/* Orientation */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsOrientationSupported(DTWAIN_SOURCE Source, LONG Orientation);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetOrientation(DTWAIN_SOURCE Source, LONG Orient, DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOrientation(DTWAIN_SOURCE Source, LPLONG lpOrient, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumOrientations(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumOrientationsEx(DTWAIN_SOURCE Source);

/* Paper sizes */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPaperSizeSupported(DTWAIN_SOURCE Source, LONG PaperSize);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPaperSize(DTWAIN_SOURCE Source, LONG PaperSize, DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPaperSize(DTWAIN_SOURCE Source, LPLONG lpPaperSize, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPaperSizes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPaperSizesEx(DTWAIN_SOURCE Source);

/* Pixel Types and Bit depths */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPixelType(DTWAIN_SOURCE Source, LPLONG PixelType,
                                         LPLONG BitDepth, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPixelType(DTWAIN_SOURCE Source, LONG PixelType, LONG BitDepth, DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetBitDepth(DTWAIN_SOURCE Source, LONG BitDepth,  DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetBitDepth(DTWAIN_SOURCE Source, LPLONG BitDepth, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPixelTypes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumBitDepths(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPixelTypeSupported(DTWAIN_SOURCE Source, LONG PixelType);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumBitDepthsEx(DTWAIN_SOURCE Source, LONG PixelType, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumBitDepthsEx2(DTWAIN_SOURCE Source, LONG PixelType);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumFileTypeBitsPerPixel(LONG FileType, LPDTWAIN_ARRAY Array);

/* Support for CAP_CUSTOMDSDATA */
HANDLE DLLENTRY_DEF DTWAIN_GetCustomDSData(DTWAIN_SOURCE Source, LPBYTE Data, LONG dSize, LPLONG pActualSize,
                                           LONG nFlags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCustomDSData(DTWAIN_SOURCE Source, HANDLE hData, LPCBYTE Data, LONG dSize, LONG nFlags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsCustomDSDataSupported(DTWAIN_SOURCE Source);

/* Only to be used by static libraries.  This is mapped to DTWAIN_SysInitializexxx() for DLL */
DTWAIN_HANDLE DLLENTRY_DEF  DTWAIN_SysInitializeLib(HINSTANCE hInstance);

/* Set JPEG Quality for JPEG file transfers */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetJpegValues(DTWAIN_SOURCE Source, LONG Quality, LONG Progressive);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetJpegValues(DTWAIN_SOURCE Source, LPLONG pQuality, LPLONG Progressive);

/* Set PDF Options for PDF file transfers */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFOrientation(DTWAIN_SOURCE Source, LONG lPOrientation);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFPageSize(DTWAIN_SOURCE Source, LONG PageSize,
                                               DTWAIN_FLOAT CustomWidth,
                                               DTWAIN_FLOAT CustomHeight);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFPageScale(DTWAIN_SOURCE Source, LONG nOptions, DTWAIN_FLOAT xScale,
                                                DTWAIN_FLOAT yScale);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFCompression(DTWAIN_SOURCE Source, DTWAIN_BOOL bCompression);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFASCIICompression(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFAESEncryption(DTWAIN_SOURCE Source, BOOL bUseAES);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPostScriptType(DTWAIN_SOURCE Source, LONG PSType);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFJpegQuality(DTWAIN_SOURCE Source, LONG Quality);

/* Text element used for DTWAIN_AddPDFTextEx function */
DTWAIN_PDFTEXTELEMENT DLLENTRY_DEF DTWAIN_CreatePDFTextElement(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_DestroyPDFTextElement(DTWAIN_PDFTEXTELEMENT TextElement);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddPDFTextEx(DTWAIN_SOURCE Source, DTWAIN_PDFTEXTELEMENT TextElement);

/* Setting the text element */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFTextElementFloat(DTWAIN_PDFTEXTELEMENT TextElement, DTWAIN_FLOAT val1, DTWAIN_FLOAT val2, LONG Flags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFTextElementLong(DTWAIN_PDFTEXTELEMENT TextElement, LONG val1, LONG val2, LONG Flags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ResetPDFTextElement(DTWAIN_PDFTEXTELEMENT TextElement);


DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPDFTextElementFloat(DTWAIN_PDFTEXTELEMENT TextElement, LPDTWAIN_FLOAT val1, LPDTWAIN_FLOAT val2, LONG Flags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPDFTextElementLong(DTWAIN_PDFTEXTELEMENT TextElement, LPLONG val1, LPLONG val2, LONG Flags);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ClearPDFText(DTWAIN_SOURCE Source);

/* TIFF options */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTIFFInvert(DTWAIN_SOURCE Source, LONG Setting);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTIFFCompressType(DTWAIN_SOURCE Source, LONG Setting);

/* Job Control for high-speed scanners */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetJobControl(DTWAIN_SOURCE Source, LONG JobControl, DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetJobControl(DTWAIN_SOURCE Source, LPLONG pJobControl, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumJobControls(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumJobControlsEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsJobControlSupported(DTWAIN_SOURCE Source, LONG JobControl);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableJobFileHandling(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet);


/* Border detection and deskew for scanners that support these capabilities */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableAutoDeskew(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoDeskewSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoDeskewEnabled(DTWAIN_SOURCE Source);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableAutoBorderDetect(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoBorderDetectSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoBorderDetectEnabled(DTWAIN_SOURCE Source);

/* Light path */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumLightPaths(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY LightPath);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumLightPathsEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsLightPathSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetLightPath(DTWAIN_SOURCE Source, LONG LightPath);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetLightPath(DTWAIN_SOURCE Source, LPLONG lpLightPath);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetLightPathEx(DTWAIN_SOURCE Source, DTWAIN_ARRAY LightPaths);

/* Lamp State */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsLampSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsLampEnabled(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableLamp(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable);

/* Light Source */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumLightSources(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY LightSources);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumLightSourcesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsLightSourceSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetLightSources(DTWAIN_SOURCE Source, DTWAIN_ARRAY LightSources);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetLightSource(DTWAIN_SOURCE Source, LONG LightSource);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetLightSources(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY LightSources);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetLightSource(DTWAIN_SOURCE Source, LPLONG LightSource);

/* Page failure action (sets what to do if a page fails to be acquired successfully) */
/* These functions are not guaranteed to work if the Data Source does not behave correctly
   when handling page failures.  Caveat Emptor! */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetMaxRetryAttempts(DTWAIN_SOURCE Source, LONG nAttempts);
LONG        DLLENTRY_DEF DTWAIN_GetMaxRetryAttempts(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCurrentRetryCount(DTWAIN_SOURCE Source, LONG nCount);
LONG        DLLENTRY_DEF DTWAIN_GetCurrentRetryCount(DTWAIN_SOURCE Source);

/* Used to skip any problems getting image info on acquire.  Currently undocumented. */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SkipImageInfoError(DTWAIN_SOURCE Source, DTWAIN_BOOL bSkip);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsSkipImageInfoError(DTWAIN_SOURCE Source);

/* Allows area of image (DIB) to be returned to application when acquiring.  Use this if DTWAIN_SetAcquireArea
   does not work correctly.  Left, top, right, bottom are in the UNIT units */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireArea2(DTWAIN_SOURCE Source, DTWAIN_FLOAT left, DTWAIN_FLOAT top,
                                                DTWAIN_FLOAT right, DTWAIN_FLOAT bottom, LONG lUnit, LONG Flags);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireArea2(DTWAIN_SOURCE Source, LPDTWAIN_FLOAT left, LPDTWAIN_FLOAT top,
                                                LPDTWAIN_FLOAT right, LPDTWAIN_FLOAT bottom, LPLONG lpUnit);

/*******************************************************************/

/* Functions to control the strip size of buffered transfer. */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireStripSizes(DTWAIN_SOURCE Source, LPLONG lpMin, LPLONG lpMax,
                                                     LPLONG lpPreferred);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireStripBuffer(DTWAIN_SOURCE Source, HANDLE hMem);
HANDLE      DLLENTRY_DEF DTWAIN_GetAcquireStripBuffer(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireStripData(DTWAIN_SOURCE Source, LPLONG lpCompression, LPLONG lpBytesPerRow,
                                                    LPLONG lpColumns, LPLONG lpRows, LPLONG XOffset,
                                                    LPLONG YOffset, LPLONG lpBytesWritten);

/* Extended image information functions. This function can be called at any time*/
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsExtImageInfoSupported(DTWAIN_SOURCE Source);

/* These functions can only be used in State 7   (when DTWAIN_TN_TRANSFERDONE notification is sent).
   This means that only languages that can utilize DTWAIN_SetCallback or can intercept Window's
   messages can use these functions. */

/* Initialize the extimageinfo interface.  This must be called first! */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_InitExtImageInfo(DTWAIN_SOURCE Source);

/* Return all the supported ExtImageInfo types.  This function is useful if your app
   wants to know what types of Extended Image Information is supported by the Source */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumExtImageInfoTypes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);

/* This function actualy initiates the querying of the ext image information.  This function
   will query the TWAIN Source.  If your TWAIN Source has bugs, this will be where any problem
   will exist */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetExtImageInfo(DTWAIN_SOURCE Source);

/* Application adds an item to query the image information.  Before getting the Extended
Image Information, the application will call DTWAIN_AddExtImageInfoQuery multiple times,
each time for each Image Information desired  */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AddExtImageInfoQuery(DTWAIN_SOURCE Source, LONG ExtImageInfo);

/* This returns the data that the Source returned when the item is queried.  Application
   must make sure that the LPVOID passed in fits the data that is returned from the Source.
   Use DTWAIN_GetExtImageInfoItem to determine the type of data.   */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetExtImageInfoData(DTWAIN_SOURCE Source, LONG nWhich, LPDTWAIN_ARRAY Data);

/* This returns the information pertaining to a certain item in the list.  The application
will call this for each information retrieved from the Source.  This function does not
return the actual data, only the information as to the number of items, data type, etc.
that the Source reports for the data item.  Use DTWAIN_GetExtImageInfoData to get the
data */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetExtImageInfoItem(DTWAIN_SOURCE Source, LONG nWhich, LPLONG InfoID, LPLONG NumItems, LPLONG Type);

/* Uninitializes the Extended Inmage information interface.  This also must be called  */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FreeExtImageInfo(DTWAIN_SOURCE Source);

/* Function to control auto-generation of image files produced by DTWAIN */
/* The Increment can be positive or negative */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetFileAutoIncrement( DTWAIN_SOURCE Source, LONG Increment, DTWAIN_BOOL bResetOnAcquire,
                                                      DTWAIN_BOOL bSet );

/* Manual duplex mode functions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetManualDuplexMode( DTWAIN_SOURCE Source, LONG Flags, DTWAIN_BOOL bSet );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetManualDuplexCount( DTWAIN_SOURCE Source, LPLONG pSide1, LPLONG pSide2);

/* Multi page mode */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetMultipageScanMode( DTWAIN_SOURCE Source, LONG ScanType);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FlushAcquiredPages(DTWAIN_SOURCE Source);

/*************************** Functions to control the Save As dialog  *****************/
#ifdef _WIN32
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetCustomFileSave(OPENFILENAME* lpOpenFileStruct);
#endif
/*************************************************************************************/
/* Miscellaneous DIB functions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FlipBitmap( HANDLE hDib );

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumSupportedCapsEx( DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumExtendedCapsEx(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);

/* Test DTWAIN support libraries for various image types */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsTIFFSupported(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPDFSupported(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPNGSupported(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsJPEGSupported(VOID_PROTOTYPE);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsFileSystemSupported( DTWAIN_SOURCE Source );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumTopCameras(DTWAIN_SOURCE Source,    LPDTWAIN_ARRAY Cameras);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumBottomCameras(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Cameras);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumCameras(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY Cameras);

/* Blank page detection functions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetBlankPageDetection(DTWAIN_SOURCE Source, DTWAIN_FLOAT threshold,
                                                      LONG autodetect_option, DTWAIN_BOOL bSet);
LONG DLLENTRY_DEF DTWAIN_GetBlankPageAutoDetection(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsBlankPageDetectionOn(DTWAIN_SOURCE Source);
LONG DLLENTRY_DEF DTWAIN_IsDIBBlank(HANDLE hDib, DTWAIN_FLOAT threshold);

/* Audio */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumAudioXferMechs(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumAudioXferMechsEx(DTWAIN_SOURCE Source);

/* CAP_ALARMS */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumAlarms( DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumAlarmsEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumAlarmVolumes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL expandIfRange);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumAlarmVolumesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL expandIfRange);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAlarms(DTWAIN_SOURCE Source, DTWAIN_ARRAY Alarms);

/* CAP_ALARMVOLUME */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAlarmVolume(DTWAIN_SOURCE Source, LPLONG lpVolume);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAlarmVolume(DTWAIN_SOURCE Source, LONG Volume);

/* CAP_AUTOMATICCAPTURE */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumAutomaticCaptures(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumAutomaticCapturesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);

/* CAP_AUTOMATICSENSEMEDIUM */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumAutomaticSenseMedium(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumAutomaticSenseMediumEx(DTWAIN_SOURCE Source);

/* CAP_AUTOSCAN */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumAutoScanValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableAutoScan(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoScanEnabled(DTWAIN_SOURCE Source);

/* CAP_BATTERYMINUTES */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetBatteryMinutes( DTWAIN_SOURCE Source, LPLONG lpMinutes );

/* CAP_BATTERYPERCENTAGE */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetBatteryPercent( DTWAIN_SOURCE Source, LPLONG lpPercent );

/* CAP_CLEARBUFFERS */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ClearBuffers( DTWAIN_SOURCE Source, LONG ClearBuffer );

/* CAP_DEVICEONLINE */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsDeviceOnLine( DTWAIN_SOURCE Source );

/* CAP_DOUBLEFEEDDETECTION */
LONG        DLLENTRY_DEF DTWAIN_EnumDoubleFeedDetectValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY  DLLENTRY_DEF DTWAIN_EnumDoubleFeedDetectValuesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsDoubleFeedDetectSupported(DTWAIN_SOURCE Source, LONG SupportVal);
DTWAIN_BOOL  DLLENTRY_DEF DTWAIN_SetDoubleFeedDetectValues(DTWAIN_SOURCE Source, DTWAIN_ARRAY prray);
LONG        DLLENTRY_DEF DTWAIN_EnumDoubleFeedDetectLengths(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumDoubleFeedDetectLengthsEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetDoubleFeedDetectLength(DTWAIN_SOURCE Source, LPDTWAIN_FLOAT Value, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsDoubleFeedDetectLengthSupported(DTWAIN_SOURCE Source, DTWAIN_FLOAT value);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetDoubleFeedDetectLength(DTWAIN_SOURCE Source, DTWAIN_FLOAT Value);

/* Gets all the current double feed detect values that are set*/
DTWAIN_BOOL DLLENTRY_DEF  DTWAIN_GetDoubleFeedDetectValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);


/* CAP_FEEDERALIGNMENT */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetFeederAlignment( DTWAIN_SOURCE Source, LPLONG lpAlignment );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetFeederAlignment( DTWAIN_SOURCE Source, LONG lpAlignment );

/* CAP_FEEDERORDER */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetFeederOrder( DTWAIN_SOURCE Source, LPLONG lpOrder );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetFeederOrder( DTWAIN_SOURCE Source, LONG lOrder );

/* CAP_MAXBATCHBUFFERS */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetMaxBuffers( DTWAIN_SOURCE Source, LONG MaxBuf );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumMaxBuffers( DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pMaxBufs, DTWAIN_BOOL bExpandRange);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumMaxBuffersEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandRange);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsMaxBuffersSupported( DTWAIN_SOURCE Source, LONG MaxBuf );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetMaxBuffers(DTWAIN_SOURCE Source, LPLONG pMaxBuf);

/* ICAP_AUTOBRIGHT */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoBrightEnabled(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableAutoBright(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoBrightSupported(DTWAIN_SOURCE Source);

/* ICAP_AUTOMATICROTATE */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoRotateSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsAutoRotateEnabled(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnableAutoRotate(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet);

/* ICAP_OVERSCAN */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsOverscanSupported(DTWAIN_SOURCE Source, LONG SupportValue);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetOverscan(DTWAIN_SOURCE Source, LONG Value, DTWAIN_BOOL bSetCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOverscan(DTWAIN_SOURCE Source, LPLONG lpOverscan, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumOverscanValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumOverscanValuesEx(DTWAIN_SOURCE Source);

/* ICAP_HIGHLIGHT */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_SetHighlight( DTWAIN_SOURCE Source, DTWAIN_FLOAT Highlight);
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetHighlight( DTWAIN_SOURCE Source, LPDTWAIN_FLOAT Highlight);
LONG                 DLLENTRY_DEF    DTWAIN_EnumHighlightValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY		 DLLENTRY_DEF    DTWAIN_EnumHighlightValuesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);

/* ICAP_HALFTONES */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumHalftones(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumHalftonesEx(DTWAIN_SOURCE Source);

/* ICAP_XRESOLUTION, ICAP_YRESOLUTION */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetXResolution(DTWAIN_SOURCE Source, DTWAIN_FLOAT xResolution);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetYResolution(DTWAIN_SOURCE Source, DTWAIN_FLOAT yResolution);

/* ICAP_NOISEFILTERS */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumNoiseFilters( DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumNoiseFiltersEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetNoiseFilter(DTWAIN_SOURCE Source, LPLONG lpNoiseFilter);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetNoiseFilter(DTWAIN_SOURCE Source, LONG NoiseFilter);

/* ICAP_PIXELFLAVOR */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPixelFlavor(DTWAIN_SOURCE Source, LPLONG lpPixelFlavor);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPixelFlavor(DTWAIN_SOURCE Source, LONG PixelFlavor);


/* ICAP_ROTATION */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetRotation(DTWAIN_SOURCE Source, LPDTWAIN_FLOAT Rotation);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetRotation(DTWAIN_SOURCE Source, DTWAIN_FLOAT Rotation);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsRotationSupported(DTWAIN_SOURCE Source);

/* ICAP_SHADOW */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_SetShadow( DTWAIN_SOURCE Source, DTWAIN_FLOAT Shadow);
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetShadow( DTWAIN_SOURCE Source, LPDTWAIN_FLOAT Shadow);
LONG                 DLLENTRY_DEF    DTWAIN_EnumShadowValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY         DLLENTRY_DEF    DTWAIN_EnumShadowValuesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);

/* ICAP_THRESHOLD */
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_SetThreshold( DTWAIN_SOURCE Source, DTWAIN_FLOAT Threshold, DTWAIN_BOOL bSetBithDepthReduction);
DTWAIN_BOOL          DLLENTRY_DEF    DTWAIN_GetThreshold( DTWAIN_SOURCE Source,LPDTWAIN_FLOAT Threshold);
LONG                 DLLENTRY_DEF    DTWAIN_EnumThresholdValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray, DTWAIN_BOOL bExpandIfRange);
DTWAIN_ARRAY         DLLENTRY_DEF    DTWAIN_EnumThresholdValuesEx(DTWAIN_SOURCE Source, DTWAIN_BOOL bExpandIfRange);

/* Patch codes */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPatchCapsSupported(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPatchSupported(DTWAIN_SOURCE Source, LONG PatchCode);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsPatchDetectEnabled(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnablePatchDetect(DTWAIN_SOURCE Source, DTWAIN_BOOL bEnable);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPatchMaxRetries(DTWAIN_SOURCE Source, LONG nMaxRetries);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPatchMaxRetries(DTWAIN_SOURCE Source, LPLONG pMaxRetries, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPatchMaxRetries(DTWAIN_SOURCE Source,LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPatchMaxRetriesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPatchMaxPriorities(DTWAIN_SOURCE Source, LONG nMaxSearchRetries);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPatchMaxPriorities(DTWAIN_SOURCE Source, LPLONG pMaxPriorities, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPatchMaxPriorities(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPatchMaxPrioritiesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPatchSearchMode(DTWAIN_SOURCE Source, LONG nSearchMode);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPatchSearchMode(DTWAIN_SOURCE Source, LPLONG pSearchMode, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPatchSearchModes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPatchSearchModesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPatchTimeOut(DTWAIN_SOURCE Source, LONG TimeOutValue);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPatchTimeOut(DTWAIN_SOURCE Source, LPLONG pTimeOut, DTWAIN_BOOL bCurrent);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPatchTimeOutValues(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPatchTimeOutValuesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPatchPriorities(DTWAIN_SOURCE Source, DTWAIN_ARRAY SearchPriorities);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetPatchPriorities(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY SearchPriorities);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPatchPriorities(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY pArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPatchPrioritiesEx(DTWAIN_SOURCE Source);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumPatchCodes(DTWAIN_SOURCE Source, LPDTWAIN_ARRAY PCodes);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_EnumPatchCodesEx(DTWAIN_SOURCE Source);

/* Miscellaneous code */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_DisableAppWindow(HWND hWnd, DTWAIN_BOOL bDisable);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_OpenSourcesOnSelect(DTWAIN_BOOL bSet);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetVersionEx(LPLONG lMajor, LPLONG lMinor,
                                             LPLONG lVersionType, LPLONG lPatchLevel);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetEOJDetectValue(DTWAIN_SOURCE Source, LONG nValue);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetQueryCapSupport(DTWAIN_BOOL bSet);

/* Threading functions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_StartThread( DTWAIN_HANDLE DLLHandle );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EndThread( DTWAIN_HANDLE DLLHandle );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_UseMultipleThreads(DTWAIN_BOOL bSet);

/* TWAIN time-out values */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTwainTimeout( LONG milliseconds );
LONG DLLENTRY_DEF DTWAIN_GetTwainTimeout(VOID_PROTOTYPE);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetNumFilesToAppend(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_StartAppend(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_CloseImageFileAppend(VOID_PROTOTYPE);

/* User-defined callback to change DIB */
DTWAIN_DIBUPDATE_PROC DLLENTRY_DEF DTWAIN_SetUpdateDibProc(DTWAIN_DIBUPDATE_PROC DibProc);

/* Error buffer access */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetErrorBuffer(LPDTWAIN_ARRAY ArrayBuffer);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ClearErrorBuffer(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetErrorBufferThreshold(LONG nErrors);
LONG        DLLENTRY_DEF DTWAIN_GetErrorBufferThreshold(VOID_PROTOTYPE);

/* Throw exceptions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_AppHandlesExceptions(DTWAIN_BOOL bSet);

/* OCR functions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_InitOCRInterface(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumOCRInterfaces(LPDTWAIN_ARRAY OCRInterfaces);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_EnumOCRSupportedCaps(DTWAIN_OCRENGINE Engine,
                                                    LPDTWAIN_ARRAY SupportedCaps);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRCapValues(DTWAIN_OCRENGINE Engine,
                                                LONG OCRCapValue,
                                                LONG GetType,
                                                LPDTWAIN_ARRAY CapValues);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetOCRCapValues(DTWAIN_OCRENGINE Engine,
                                                LONG OCRCapValue,
                                                LONG SetType,
                                                DTWAIN_ARRAY CapValues);
DTWAIN_OCRENGINE DLLENTRY_DEF DTWAIN_SelectOCREngine(VOID_PROTOTYPE);
DTWAIN_OCRENGINE DLLENTRY_DEF DTWAIN_SelectDefaultOCREngine(VOID_PROTOTYPE);

DTWAIN_OCRTEXTINFOHANDLE DLLENTRY_DEF DTWAIN_GetOCRTextInfoHandle(DTWAIN_OCRENGINE Engine, LONG nPageNo);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoLongEx(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo,
                                                     LONG nWhichItem,
                                                     LPLONG pInfo,
                                                     LONG bufSize);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoFloatEx(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo,
                                                      LONG nWhichItem,
                                                      LPDTWAIN_FLOAT pInfo,
                                                      LONG bufSize);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ShutdownOCREngine(DTWAIN_OCRENGINE OCREngine);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_IsOCREngineActivated(DTWAIN_OCRENGINE OCREngine);
LONG DLLENTRY_DEF DTWAIN_SetPDFOCRConversion(DTWAIN_OCRENGINE Engine,
                                             LONG PageType, LONG FileType,
                                             LONG PixelType, LONG BitDepth,LONG Options);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFOCRMode(DTWAIN_SOURCE Source, DTWAIN_BOOL bSet);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoLong(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, LONG nCharPos, LONG nWhichItem, LPLONG pInfo);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetOCRTextInfoFloat(DTWAIN_OCRTEXTINFOHANDLE OCRTextInfo, LONG nCharPos, LONG nWhichItem, LPDTWAIN_FLOAT pInfo);
LONG        DLLENTRY_DEF DTWAIN_GetOCRLastError(DTWAIN_OCRENGINE Engine);

/* TWAIN version 2 */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetTwainDSM(LONG DSMType);
LONG DLLENTRY_DEF DTWAIN_GetTwainAvailability(VOID_PROTOTYPE);

/* Set callback for logging messages */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetLoggerCallback(DTWAIN_LOGGER_PROC logProc, DTWAIN_LONG64 UserData);
DTWAIN_LOGGER_PROC DLLENTRY_DEF DTWAIN_GetLoggerCallback(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetLoggerCallbackA(DTWAIN_LOGGER_PROCA logProc, DTWAIN_LONG64 UserData);
DTWAIN_LOGGER_PROCA DLLENTRY_DEF DTWAIN_GetLoggerCallbackA(VOID_PROTOTYPE);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetLoggerCallbackW(DTWAIN_LOGGER_PROCW logProc, DTWAIN_LONG64 UserData);
DTWAIN_LOGGER_PROCW DLLENTRY_DEF DTWAIN_GetLoggerCallbackW(VOID_PROTOTYPE);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetErrorCallback(DTWAIN_ERROR_PROC, LONG UserData);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetErrorCallback64(DTWAIN_ERROR_PROC64, DTWAIN_LONG64 UserData64);
DTWAIN_ERROR_PROC DLLENTRY_DEF DTWAIN_GetErrorCallback(VOID_PROTOTYPE);
DTWAIN_ERROR_PROC64 DLLENTRY_DEF DTWAIN_GetErrorCallback64(VOID_PROTOTYPE);

/* set TW_FIX32 type */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFix32SetAt(DTWAIN_ARRAY aFix32, DTWAIN_LONG lPos, DTWAIN_LONG Whole, DTWAIN_LONG Frac);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFix32GetAt(DTWAIN_ARRAY aFix32, DTWAIN_LONG lPos, LPLONG Whole, LPLONG Frac);

/* special cap functions for TW_FIX32 (only use if DTWAIN_FLOAT gives issues with your environment) */
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayConvertFloatToFix32(DTWAIN_ARRAY FloatArray);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayConvertFix32ToFloat(DTWAIN_ARRAY Fix32Array);

/* functions to get string length, max string length in a DTWAIN_ARRAY that holds strings */
LONG DLLENTRY_DEF DTWAIN_ArrayGetStringLength(DTWAIN_ARRAY a, LONG nWhichString);
LONG DLLENTRY_DEF DTWAIN_ArrayGetMaxStringLength(DTWAIN_ARRAY a);

/* function to destroy array of DTWAIN_FRAME objects */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayDestroyFrames(DTWAIN_ARRAY FrameArray);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetPosFloat( DTWAIN_RANGE pArray, DTWAIN_FLOAT Val, LPLONG pPos );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_RangeGetPosLong( DTWAIN_RANGE pArray, LONG Value, LPLONG pPos );

/* load the language resource */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_LoadLanguageResource(LONG nLanguage);

/* get a frame from a DTWAIN_ARRAYFRAME */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFrameGetAt(DTWAIN_ARRAY FrameArray, LONG nWhere, LPDTWAIN_FLOAT pleft, LPDTWAIN_FLOAT ptop, LPDTWAIN_FLOAT pright, LPDTWAIN_FLOAT pbottom );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ArrayFrameSetAt(DTWAIN_ARRAY FrameArray, LONG nWhere, DTWAIN_FLOAT left, DTWAIN_FLOAT top, DTWAIN_FLOAT right, DTWAIN_FLOAT bottom );
DTWAIN_FRAME DLLENTRY_DEF DTWAIN_ArrayFrameGetFrameAt(DTWAIN_ARRAY FrameArray, LONG nWhere );

/* TWAIN 1.x memory allocation functions */
HANDLE      DLLENTRY_DEF DTWAIN_AllocateMemory(LONG memSize);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FreeMemory(HANDLE h);
DTWAIN_MEMORY_PTR DLLENTRY_DEF DTWAIN_LockMemory(HANDLE h);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_UnlockMemory(HANDLE h);

/* TWAIN 2.x memory allocation functions (actual low-level functions determined by TWAINDSM.DLL) */
HANDLE      DLLENTRY_DEF DTWAIN_AllocateMemoryEx(LONG memSize);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_FreeMemoryEx(HANDLE h);
DTWAIN_MEMORY_PTR DLLENTRY_DEF DTWAIN_LockMemoryEx(HANDLE h);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_UnlockMemoryEx(HANDLE h);

/* TWAIN DSM search order functions */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetDSMSearchOrder(LONG SearchPath);
LONG        DLLENTRY_DEF DTWAIN_GetDSMSearchOrder(VOID_PROTOTYPE);

/* New Twain 2.4 Metrics function*/
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_GetAcquireMetrics(DTWAIN_SOURCE source, LPLONG ImageCount, LPLONG SheetCount);

/* Optionally create a negative of the acquired image */
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetAcquireImageNegative( DTWAIN_SOURCE Source, DTWAIN_BOOL IsNegative );
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_SetPDFPolarity(DTWAIN_SOURCE Source, LONG Polarity);
DTWAIN_BOOL DLLENTRY_DEF DTWAIN_ForceScanOnNoUI(DTWAIN_SOURCE Source, BOOL bSet);

/* String array functions that return pointers */
LPCTSTR DLLENTRY_DEF DTWAIN_ArrayGetAtStringPtr(DTWAIN_ARRAY pArray, LONG nWhere);
LPCWSTR DLLENTRY_DEF  DTWAIN_ArrayGetAtWideStringPtr(DTWAIN_ARRAY pArray, LONG nWhere);
LPCSTR DLLENTRY_DEF  DTWAIN_ArrayGetAtANSIStringPtr(DTWAIN_ARRAY pArray, LONG nWhere);
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_ArrayInit(VOID_PROTOTYPE);

DTWAIN_BOOL DLLENTRY_DEF DTWAIN_CheckHandles(DTWAIN_BOOL bCheck);
LONG DLLENTRY_DEF DTWAIN_MakeRGB(LONG red, LONG green, LONG blue);
LONG DLLENTRY_DEF DTWAIN_SetTwainDialogFont(HFONT font);

/* Audio transfers */
DTWAIN_ARRAY DLLENTRY_DEF DTWAIN_AcquireAudioNative(DTWAIN_SOURCE Source, LONG nMaxAudioClips, DTWAIN_BOOL bShowUI, DTWAIN_BOOL bCloseSource, LPLONG pStatus);
DTWAIN_BOOL  DLLENTRY_DEF DTWAIN_AcquireAudioNativeEx(DTWAIN_SOURCE Source, LONG nMaxAudioClips, DTWAIN_BOOL bShowUI, DTWAIN_BOOL bCloseSource, DTWAIN_ARRAY Acquisitions, LPLONG pStatus);

/* Get the application and TWAIN Source TW_IDENTITY */
TWAIN_IDENTITY  DLLENTRY_DEF DTWAIN_GetTwainAppIDEx(TW_IDENTITY* pIdentity);
TWAIN_IDENTITY  DLLENTRY_DEF DTWAIN_GetSourceIDEx(DTWAIN_SOURCE Source, TW_IDENTITY* pIdentity);

/* Convert DIB to HBITMAP */
HANDLE DLLENTRY_DEF DTWAIN_ConvertDIBToBitmap(HANDLE hDib, HANDLE hPalette);
#include "dtwstrfn.h"
#ifdef __cplusplus
}
#endif
#endif

