/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2018 Dynarithmic Software.

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

    For more information, the license file LICENSE.TXT that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef IMAGEXFERFILEWRITER_H
#define IMAGEXFERFILEWRITER_H

#include "tr1defs.h"
#include "ctlobstr.h"
#include <memory>

namespace dynarithmic
{
    class CTL_ImageXferTriplet;
    class CTL_ITwainSession;
    class CTL_ITwainSource;
    class CTL_ImageIOHandler;

    typedef std::shared_ptr<CTL_ImageIOHandler> CTL_ImageIOHandlerPtr;

    class CTL_TwainDib;
    typedef std::shared_ptr<CTL_TwainDib> CTL_TwainDibPtr;

    struct sDuplexFileData;
    class ImageXferFileWriter
    {
        public:
            ImageXferFileWriter();
            ImageXferFileWriter(CTL_ImageXferTriplet* pTrip,
                                CTL_ITwainSession *pSession,
                                CTL_ITwainSource *pSource);

            int     CopyDibToFile(CTL_TwainDibPtr pCurDib,
                                   int MultipageOption,
                                   CTL_ImageIOHandlerPtr& pHandler,
                                   LONG rawBytes,
                                   bool bIsJobControl=false);

            LONG     CopyDuplexDibToFile(CTL_TwainDibPtr pCurDib, bool bIsJobControl=false);

            LONG     MergeDuplexFiles();
            void     RecordBadDuplexPage();
            int      ProcessManualDuplexState(LONG Msg);
            int      MergeDuplexFilesEx(const sDuplexFileData& DupData,
                                        CTL_ImageIOHandlerPtr& pHandler,
                                        const CTL_StringType& strTempFile,
                                        int MultiPageOption);

            int     CopyDibToFileEx(CTL_TwainDibPtr pCurDib,
                                     int MultipageOption,
                                     CTL_ImageIOHandlerPtr& pHandler,
                                     const CTL_StringType& strTempFile);

            void     ManualDuplexCleanUp(const CTL_StringType& strFile,
                                         bool bDeleteFile=false);
            bool     ProcessFailureCondition(int nAction);
            LONG     CloseMultiPageDibFile(bool bSaveFile=true);
            void     EndProcessingImageFile(bool bSaveFile=true);

        private:
            CTL_ImageXferTriplet *m_pTrip;
            CTL_ITwainSession *m_pSession;
            CTL_ITwainSource *m_pSource;
    };
}
#endif


