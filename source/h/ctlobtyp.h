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
#ifndef CTLObTyp_h_
#define CTLObTyp_h_

#include <memory>
#include "ctlobstr.h"
#include "ctltwain.h"
#include "tr1defs.h"
////////////////////////////////////////////////////////////////////////////
namespace dynarithmic
{
    class CTL_TwainTypeOb
    {
        public:
             // Set equal to twain type
             CTL_TwainTypeOb( TW_UINT16 nType, bool bGetTypeSize=true);
             virtual ~CTL_TwainTypeOb();

             // Data initialization.  This MUST fit into allocated space for type
             // specified in constructor
             void CopyData( void *pData );

             // Returns the raw data in this object
             void *GetDataRaw() const;
             void GetData( void *pData );

             int GetDataSize() const;
             int GetDataType() const;

             // Define operators for each type (useful if calling functions
             // with the known type as the receiver
             operator TW_INT8 ()    const { return *(TW_INT8   *)m_pData; }
             operator TW_INT16 ()   const { return *(TW_INT16  *)m_pData; }
             operator TW_INT32 ()   const { return *(TW_INT32  *)m_pData; }
             operator TW_UINT8 ()   const { return *(TW_UINT8  *)m_pData; }
             operator TW_UINT16 ()  const { return *(TW_UINT16 *)m_pData; }
             operator TW_UINT32 ()  const { return *(TW_UINT32 *)m_pData; }
             operator TW_FIX32 ()   const { return *(TW_FIX32  *)m_pData; }
             operator TW_FRAME ()   const { return *(TW_FRAME  *)m_pData; }
             operator CTL_String()  const { return  (CTL_String)(LPSTR)m_pData; }

        private:
             int        m_nSize;
             TW_UINT16  m_nType;
             void *m_pData;
             HGLOBAL    m_hGlobal;
    };

    typedef std::shared_ptr<CTL_TwainTypeOb> CTL_TwainTypeObPtr;
    typedef std::vector<CTL_TwainTypeObPtr> CTL_TwainTypeArray;
}
#endif


