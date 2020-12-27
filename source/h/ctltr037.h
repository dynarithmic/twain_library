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
#ifndef CTLTR037_H_
#define CTLTR037_H_

#include "ctltr010.h"
namespace dynarithmic
{
    class CTL_FileSystemTriplet : public CTL_TwainTriplet
    {
        public:
            CTL_FileSystemTriplet(CTL_ITwainSession *pSession,
                                   CTL_ITwainSource* pSource,
                                   TW_UINT16 nMsg=MSG_CHANGEDIRECTORY);
            //TW_UINT16   Execute();

            TW_UINT16 ChangeDirectory(const CTL_StringType& sDir);
            TW_UINT16 CopyFile(const CTL_StringType& sInput, const CTL_StringType& sOutput);
            TW_UINT16 CreateDirectory(const CTL_StringType& sDir);
            TW_UINT16 DeleteFile(const CTL_StringType& sDir, bool bRecursive);
            TW_UINT16 FormatMedia(const CTL_StringType& sDir);
            TW_UINT16 GetFirstFile();
            TW_UINT16 GetClose();
            TW_UINT16 GetInfo(const CTL_StringType& sDir);
            TW_UINT16 GetNextFile(TW_MEMREF Context);
            TW_UINT16 Rename(const CTL_StringType& sInput, const CTL_StringType& sOutput);
            TW_UINT16 SelectAutoCaptureDirectory(const CTL_StringType& sDir);
            void      GetTWFileSystem(TW_FILESYSTEM& FS);

        private:
            TW_FILESYSTEM  m_FileSystem;
            TW_UINT16 ChangeDirectoryHelper(const CTL_StringType& sDir, TW_UINT16 Msg);
            TW_UINT16 ExecuteIt(TW_UINT16 Msg);
    };
}
#endif
