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
#ifndef CTLTWAIN_H_
#define CTLTWAIN_H_

#include "ctlobstr.h"
#include "twain.h"

// Define classes that wrap TWAIN TW_xxx data types
namespace dynarithmic
{
    class CTL_TWRange
    {
        public:
            CTL_TWRange( TW_UINT16 nDataType,
                         TW_UINT32 nMinValue,
                         TW_UINT32 nMaxValue,
                         TW_UINT32 nStepSize,
                         TW_UINT32 nDefault,
                         TW_UINT32 nCurValue);

            CTL_TWRange( CTL_TWRange & RangeOb);
            CTL_TWRange& operator = (CTL_TWRange & RangeOb);
            operator TW_RANGE*();

        private:
            TW_RANGE m_Range;
    };

    // Universal Twain Type
    typedef union tagAllNumericTwainTypes
    {
        TW_INT8     nInt8;
        TW_INT16    nInt16;
        TW_INT32    nInt32;
        TW_UINT8    unInt8;
        TW_UINT16   unInt16;
        TW_UINT32   unInt32;
        TW_BOOL     twbool;
        TW_FIX32    twFix32;
    } U_TWAINNUMERICVAR;

    typedef union tagAllStringTwainTypes
    {
        TW_STR32    str32;
        TW_STR64    str64;
        TW_STR128   str128;
        TW_STR255   str255;
    } U_TWAINSTRINGVAR;


    class CTL_TwainVar
    {
        public:
            TW_UINT16   GetTwainType() const { return m_nTwainType; }

        protected:
            CTL_TwainVar( TW_UINT16 nTwainType ) : m_nTwainType(nTwainType)
            {}

        private:
            TW_UINT16   m_nTwainType;
    };

    class CTL_TwainNumericVar : public CTL_TwainVar
    {
        public:
            CTL_TwainNumericVar(TW_UINT16 nTwainType, void *pValue=NULL);
            CTL_TwainNumericVar(TW_UINT16 nTwainType, long nVal);

            operator TW_INT8()   { return m_uTwainVar.nInt8; }
            operator TW_INT16()  { return m_uTwainVar.nInt16; }
            operator TW_INT32()  { return m_uTwainVar.nInt32; }
            operator TW_UINT8()  { return m_uTwainVar.unInt8; }
            operator TW_UINT16() { return m_uTwainVar.unInt16; }
            operator TW_UINT32() { return m_uTwainVar.unInt32; }
            operator TW_FIX32()  { return m_uTwainVar.twFix32; }

        private:
            U_TWAINNUMERICVAR m_uTwainVar;
    };

    class CTL_TwainStringVar : public CTL_TwainVar
    {
        public:
            CTL_TwainStringVar(TW_UINT16 nTwainType, const char *pStr=NULL);
            operator const char *()  { return m_strTwain.c_str(); }

        private:
            CTL_String m_strTwain;
    };
}
#endif
