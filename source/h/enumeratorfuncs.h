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
#ifndef ENUMERATORFUNCS_H
#define ENUMERATORFUNCS_H

#include <cmath>
#include <algorithm>

#include "ctliface.h"
#include "ctlobstr.h"
#include "ctltwmgr.h"


#define FLOAT_CLOSE(x,y) (fabs((x) - (y)) <= DTWAIN_FLOATDELTA)

inline bool operator == (const TW_FRAME& lhs, const TW_FRAME& rhs)
{
    return (lhs.Bottom.Frac == rhs.Bottom.Frac &&
        lhs.Bottom.Whole == rhs.Bottom.Whole &&
        lhs.Left.Frac == rhs.Left.Frac &&
        lhs.Left.Whole == rhs.Left.Whole &&
        lhs.Right.Frac == rhs.Right.Frac &&
        lhs.Right.Whole == rhs.Right.Whole &&
        lhs.Top.Frac == rhs.Top.Frac &&
        lhs.Top.Whole == rhs.Top.Whole);
}

inline bool operator == (const TW_FIX32& lhs, const TW_FIX32& rhs)
{
    return (lhs.Whole == rhs.Whole && lhs.Frac == rhs.Frac);
}

inline bool operator != (const TW_FRAME& lhs, const TW_FRAME& rhs)
{
    return !(lhs == rhs);
}

    namespace dynarithmic
    {
    ////////////////////////// Enumerator functions //////////////////////////////////
    template <typename T>
    struct VectorHelper
    {
        static void push_back(std::vector<T>& vec, void* value)
        { vec.push_back(*(T*)(value)); }

        static void erase(std::vector<T>& vec, size_t pos)
        { vec.erase(vec.begin() + pos); }

        static void erase(std::vector<T>& vec, size_t pos, size_t numValues)
        {
            size_t maxNumValues = std::distance(vec.begin() + pos, vec.end());
            size_t realNumValues = (std::min)(numValues, maxNumValues);
            vec.erase(vec.begin() + pos, vec.begin() + pos + realNumValues);
        }

        static void clear(std::vector<T>& vec)
        { vec.clear(); }

        static LONG find(const std::vector<T>&vec, void *value)
        {
            typename std::vector<T>::const_iterator it = std::find(vec.begin(), vec.end(), *(T*)(value));
            if ( it != vec.end())
                return (LONG)std::distance(vec.begin(), it);
            return -1;
        }

        static void copy(std::vector<T>& dest, const std::vector<T>& src)
        { dest = src; }

        static void assign(std::vector<T>& vec, size_t where, void* value)
        { vec[where] = *(T*)(value); }

        static size_t size(const std::vector<T>& vec)
        { return vec.size(); }

        static T at(const std::vector<T>&vec, size_t where)
        { return vec[where]; }

        static bool retrieve(const std::vector<T>&vec, size_t where, void* value)
        {
            if ( where >= vec.size() )
                return false;
            T *ptrvalue = (T*)value;
            *ptrvalue = vec[where];
            return true;
        }

        static void insert(std::vector<T>& vec, size_t where, void* value)
        { vec.insert(vec.begin() + where, *(T*)(value)); }

        static void insert(std::vector<T>& vec, size_t where, size_t numValues, void* value)
        { vec.insert(vec.begin() + where, numValues, *(T*)(value)); }

        static void insert_range(std::vector<T>& vec, size_t where, size_t numValues, void* valueStart)
        {
            T* valStart = (T*)valueStart;
            T* valEnd = valStart + numValues;
            vec.insert(vec.begin() + where, valStart, valEnd);
        }

        static void resize(std::vector<T>&vec, size_t newSize, void* value)
        {
            if ( !value )
                vec.resize(newSize);
			else
			{
				T defVal = *(reinterpret_cast<T*>(value));
				vec.resize(newSize, defVal);
			}
        }
    };

    #define ENUMERATOR_CREATE_ARRAY(eType, vecType, nItems) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == eType ) \
    {\
        CTL_Enumerator_##vecType vType;\
        vType.m_array.resize(nItems);\
        CTL_TwainDLLHandle::s_EnumeratorFactory->m_EnumeratorList_##vecType.push_back( vType );\
        CTL_Enumerator_##vecType *pType = &CTL_TwainDLLHandle::s_EnumeratorFactory->m_EnumeratorList_##vecType.back();\
        if ( CTL_TwainDLLHandle::s_lErrorFilterFlags )\
            EnumeratorFunctionImpl::WriteEnumeratorInfo(_T("Creating enumerator: "), pType);\
        return pType;\
    }


    #define ENUMERATOR_SETUP(eType, EnumType, EnumCheckType)  { if ( EnumType == EnumCheckType) \
    { \
        void* pDTWAINArray = SetUpEnumerator<std::list<CTL_Enumerator_##eType> >\
        (CTL_TwainDLLHandle::s_EnumeratorFactory->m_EnumeratorList_##eType, EnumType, nSize); \
        return pDTWAINArray;\
    } \
    }

    #define ENUMERATOR_IS_VALID(eType, EnumType, pEnum)  {  CTL_EnumeratorType eTypeTest = EnumeratorFunctionImpl::GetEnumeratorType(pEnum);\
        if ( eTypeTest == EnumType ) \
    {\
        return IsEnumeratorFound<std::list<CTL_Enumerator_##eType> >\
        (CTL_TwainDLLHandle::s_EnumeratorFactory->m_EnumeratorList_##eType, pEnum);\
    }\
    }

    #define ENUMERATOR_IS_VALID_NOCHECK(eType, pEnum)  {\
    {\
        bool isFound = IsEnumeratorFound<std::list<CTL_Enumerator_##eType> >\
        (CTL_TwainDLLHandle::s_EnumeratorFactory->m_EnumeratorList_##eType, pEnum);\
        if ( isFound )\
            return true;\
    }\
    }

    #define ENUMERATOR_IS_VALID_EX(pEnum, EnumType, vecType)  {  int eTestType1 = CTL_Enumerator_##vecType::ENUMTYPE; \
                                                                 CTL_EnumeratorType eTestType2 = EnumeratorFunctionImpl::GetEnumeratorType(pEnum);\
                                                                 if ( eTestType1 == EnumType && eTestType1 == eTestType2 ) \
                                                                 {\
                                                                    return IsEnumeratorFound<std::list<CTL_Enumerator_##vecType> >\
                                                                    (CTL_TwainDLLHandle::s_EnumeratorFactory->m_EnumeratorList_##vecType, pEnum);\
                                                                }\
                                                                }

    #define ENUMERATOR_COPY(EnumSource, EnumDest, EnumCheck, EnumVect, vecType) \
    { \
        int enumType1 = EnumeratorFunctionImpl::GetEnumeratorType(EnumSource);\
        int enumType2 = EnumeratorFunctionImpl::GetEnumeratorType(EnumDest);\
        if ( enumType1 == EnumCheck && enumType2 == EnumCheck && enumType1 == EnumVect) {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        CTL_Enumerator_##vecType *pType2 = (CTL_Enumerator_##vecType*)EnumDest;\
        VectorHelper<vecType>::copy(pType2->m_Array, pType1->m_Array);\
        return;\
    }\
    }

    #define ENUMERATOR_PUSH_BACK(EnumSource, ptrValue, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( EnumeratorFunctionImpl::GetEnumeratorType(EnumSource) == eTestType) {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::push_back(pType1->m_Array, ptrValue);\
        return true;\
        }\
    }

    #define ENUMERATOR_PUSH_BACK_MULTIPLE(EnumSource, ptrValue, numItems, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( EnumeratorFunctionImpl::GetEnumeratorType(EnumSource) == eTestType) {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::resize(pType1->m_Array, numItems, ptrValue);\
        return true;\
        }\
    }

    #define ENUMERATOR_INSERT_RANGE(EnumSource, startPos, ptrStart, numValues, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if (EnumeratorFunctionImpl::GetEnumeratorType(EnumSource) == eTestType) {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::insert_range(pType1->m_Array, startPos, numValues, ptrStart);\
        }\
    }

    #define ENUMERATOR_INSERT_RANGE_STRING(EnumSource, startPos, ptrStart, numValues, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if (EnumeratorFunctionImpl::GetEnumeratorType(EnumSource) == eTestType) {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        pType1->m_Array.insert(pType1->m_Array.begin() + startPos, ptrStart.begin(), ptrStart.end());\
        }\
    }

    #define ENUMERATOR_CLEAR(EnumSource, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::clear(pType1->m_Array);\
        return;\
    }\
    }

    #define ENUMERATOR_REMOVE_AT(EnumSource, pos, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::erase(pType1->m_Array, pos);\
        return;\
    }\
    }

    #define ENUMERATOR_GET_BUFFER(EnumSource, pos, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        if (pType1->m_Array.empty() || ((size_t)pos >= pType1->m_Array.size()))\
        return NULL;\
        return &pType1->m_Array[pos];\
    }\
    }

    #define ENUMERATOR_GET_VECTOR(EnumSource, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
                                                        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
                                                        {\
                                                            CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
                                                            return &pType1->m_Array;\
                                                        }\
                                                        }

    #define ENUMERATOR_REMOVE_AT_MULTIPLE(EnumSource, pos, num, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::erase(pType1->m_Array, pos, num);\
        return;\
    }\
    }

    #define ENUMERATOR_INSERT_AT(EnumSource, pos, value, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::insert(pType1->m_Array, pos, value);\
        return;\
    }\
    }

    #define ENUMERATOR_INSERT_AT_MULTIPLE(EnumSource, pos, value, numValues, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::insert(pType1->m_Array, pos, numValues, value);\
        return;\
    }\
    }

    #define ENUMERATOR_COUNT(EnumSource, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        return (int)VectorHelper<vecType>::size(pType1->m_Array);\
    }\
    }

    #define ENUMERATOR_DESTROY_ARRAY(EnumSource, vecType) \
    { \
       if (!EnumeratorFunctionImpl::EnumeratorIsValid(EnumSource)) return false;\
       int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
       if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        if (DestroyEnumerator<std::list<CTL_Enumerator_##vecType> >\
            (CTL_TwainDLLHandle::s_EnumeratorFactory->m_EnumeratorList_##vecType, EnumSource))\
        return true;\
        return false;\
    }\
    }

    #define ENUMERATOR_RESIZE(EnumSource, numValues, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::resize(pType1->m_Array, numValues, NULL);\
        return;\
        }\
    }

    #define ENUMERATOR_GET_AT(EnumSource, pos, value, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        return VectorHelper<vecType>::retrieve(pType1->m_Array, pos, value);\
    }\
    }

    #define ENUMERATOR_SET_AT(EnumSource, pos, value, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        VectorHelper<vecType>::assign(pType1->m_Array, pos, value);\
        return;\
    }\
    }

    #define ENUMERATOR_FIND_VALUE(EnumSource, value, vecType) { int eTestType = CTL_Enumerator_##vecType::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_##vecType *pType1 = (CTL_Enumerator_##vecType*)EnumSource;\
        return VectorHelper<vecType>::find(pType1->m_Array, value);\
    }\
    }

    #define ENUMERATOR_FIND_VALUE_WITH_TOLERANCE(EnumSource, value, tolerance) { int eTestType = CTL_Enumerator_double::ENUMTYPE; \
        if ( eTestType == EnumeratorFunctionImpl::GetEnumeratorType(EnumSource)) \
    {\
        CTL_Enumerator_double *pType1 = (CTL_Enumerator_double*)EnumSource;\
        return EnumeratorFunctionImpl::FindDoubleWithTolerance(*pType1, value, tolerance);\
    }\
    }

    struct EnumeratorFunctionImpl
    {
        static LONG FindDoubleWithTolerance(CTL_Enumerator_double& container, double value, double /*tol*/)
        {
            struct DoubleFinder
            {
                double m_value;
                DoubleFinder(double v) : m_value(v) {}
                bool operator()(double testValue) const
                { return FLOAT_CLOSE(testValue, m_value)?true:false; }
            };

            CTL_Enumerator_double::container_base_type::iterator it = std::find_if(container.m_Array.begin(), container.m_Array.end(), DoubleFinder(value));
            if ( it != container.m_Array.end() )
                    return (LONG)std::distance(container.m_Array.begin(), it);
            return -1;
        }

        template <typename T>
        static LPVOID SetUpEnumerator(T& val, CTL_EnumeratorType EnumType, int nSize)
        {
            val.push_back(typename T::value_type(nSize));
            typename T::value_type& enumP = val.back();
            enumP.SetEnumType(EnumType);
            if ( CTL_TwainDLLHandle::s_lErrorFilterFlags )
                EnumeratorFunctionImpl::WriteEnumeratorInfo(_T("Creating enumerator: "), &enumP);
            return &enumP;
        }

        template <typename T>
        struct EnumeratorFinder
        {
            LPVOID m_pEnum;
            EnumeratorFinder(LPVOID p) : m_pEnum(p) {}
            bool operator()(const typename T::value_type& vt) const
            { return &vt == m_pEnum; }
        };

        template <typename T>
        static bool IsEnumeratorFound(T& val, LPVOID pEnum)
        {
            return std::find_if(val.begin(), val.end(), EnumeratorFinder<T>(pEnum)) != val.end();
        }

        template <typename T>
        static bool IsEnumeratorFound(T& val, LPVOID pEnum, typename T::iterator& retIt)
        {
            typename T::iterator it = std::find_if(val.begin(), val.end(), EnumeratorFinder<T>(pEnum));
            if ( it != val.end())
                {
                    retIt = it;
                    return true;
            }
            return false;
        }

        template <typename T>
        static bool DestroyEnumerator(T& val, LPVOID pEnum)
        {
            typename T::iterator it;
            if ( IsEnumeratorFound<T>(val, pEnum, it))
            {
                val.erase(it);
                if ( CTL_TwainDLLHandle::s_lErrorFilterFlags )
                    WriteEnumeratorInfo(_T("Destroy Enumerator Success: "), pEnum);
                return true;
            }

            if ( CTL_TwainDLLHandle::s_lErrorFilterFlags )
                WriteEnumeratorInfo(_T("Destroy Enumerator Failure: "), pEnum);
            return false;
        }

        static void WriteEnumeratorInfo(LPCTSTR descr, LPVOID pEnum, LPCTSTR extraText = NULL)
        {
            CTL_StringStreamType strm;
            strm << _T("[Enumerator Info] ") << descr << pEnum;
            if ( extraText )
                strm << extraText;
            strm << _T("\n");
            CTL_TwainAppMgr::WriteLogInfo(strm.str());
        }

        static CTL_EnumeratorType GetEnumeratorType(LPVOID pEnum)
        {
            if ( !pEnum )
                return CTL_EnumeratorInvalid;
            // Cast to int to get the type
            int enumType = *reinterpret_cast<int *>(pEnum);
            switch (enumType)
            {
                case CTL_EnumeratorPtrType:
                case CTL_EnumeratorIntType:
                case CTL_EnumeratorInt64Type:
                case CTL_EnumeratorDoubleType:
                case CTL_EnumeratorHandleType:
                case CTL_EnumeratorSourceType:
                case CTL_EnumeratorStringType:
                case CTL_EnumeratorANSIStringType:
                case CTL_EnumeratorWideStringType:
                case CTL_EnumeratorDTWAINFrameType:
                case CTL_EnumeratorTWFrameType:
                case CTL_EnumeratorTWFIX32Type:
                    return CTL_EnumeratorType(enumType);
            }
            return CTL_EnumeratorInvalid;
        }

        static bool EnumeratorIsValid(LPVOID pEnum)
        {
            ENUMERATOR_IS_VALID(LPVOID, CTL_EnumeratorPtrType, pEnum)
            ENUMERATOR_IS_VALID(int, CTL_EnumeratorIntType, pEnum)
            ENUMERATOR_IS_VALID(double, CTL_EnumeratorDoubleType, pEnum)
            ENUMERATOR_IS_VALID(LONG64, CTL_EnumeratorInt64Type, pEnum)
            ENUMERATOR_IS_VALID(HANDLE, CTL_EnumeratorHandleType, pEnum)
            ENUMERATOR_IS_VALID(CTL_StringType, CTL_EnumeratorStringType, pEnum)
            ENUMERATOR_IS_VALID(CTL_String, CTL_EnumeratorANSIStringType, pEnum)
            ENUMERATOR_IS_VALID(CTL_WString, CTL_EnumeratorWideStringType, pEnum)
            ENUMERATOR_IS_VALID(DTWAINFrameInternal, CTL_EnumeratorDTWAINFrameType, pEnum)
            ENUMERATOR_IS_VALID(TW_FRAME, CTL_EnumeratorTWFrameType, pEnum)
            ENUMERATOR_IS_VALID(CTL_ITwainSourcePtr, CTL_EnumeratorSourceType, pEnum)
            ENUMERATOR_IS_VALID(TW_FIX32, CTL_EnumeratorTWFIX32Type, pEnum)
            return false;
        }

        static bool EnumeratorIsValidNoCheck(LPVOID pEnum)
        {
            if ( !pEnum )
                return false;
            ENUMERATOR_IS_VALID_NOCHECK(LPVOID,  pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(int, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(double, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(LONG64, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(HANDLE, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(CTL_StringType, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(CTL_String, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(CTL_WString, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(DTWAINFrameInternal, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(TW_FRAME, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(CTL_ITwainSourcePtr, pEnum)
            ENUMERATOR_IS_VALID_NOCHECK(TW_FIX32, pEnum)
            return false;
        }

        static bool EnumeratorIsValidEx(LPVOID pEnum, CTL_EnumeratorType enumType)
        {
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, LPVOID)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, int)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, LONG64)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, double)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, HANDLE)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, CTL_StringType)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, CTL_String)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, CTL_WString)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, DTWAINFrameInternal)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, TW_FRAME)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, CTL_ITwainSourcePtr)
            ENUMERATOR_IS_VALID_EX(pEnum, enumType, TW_FIX32)
            return false;
        }

        static LPVOID GetNewEnumerator( CTL_EnumeratorType EnumType, int *pStatus, LONG /*nInterpret*/, int nSize )
        {
            ENUMERATOR_SETUP(LPVOID, EnumType, CTL_EnumeratorPtrType)
            ENUMERATOR_SETUP(int, EnumType, CTL_EnumeratorIntType)
            ENUMERATOR_SETUP(LONG64, EnumType, CTL_EnumeratorInt64Type)
            ENUMERATOR_SETUP(double, EnumType, CTL_EnumeratorDoubleType)
            ENUMERATOR_SETUP(CTL_StringType, EnumType, CTL_EnumeratorStringType)
            ENUMERATOR_SETUP(HANDLE, EnumType, CTL_EnumeratorHandleType)
            ENUMERATOR_SETUP(CTL_ITwainSourcePtr, EnumType, CTL_EnumeratorSourceType)
            ENUMERATOR_SETUP(DTWAINFrameInternal, EnumType, CTL_EnumeratorDTWAINFrameType)
            ENUMERATOR_SETUP(TW_FRAME, EnumType, CTL_EnumeratorTWFrameType)
            ENUMERATOR_SETUP(CTL_String, EnumType, CTL_EnumeratorANSIStringType)
            ENUMERATOR_SETUP(CTL_WString, EnumType, CTL_EnumeratorWideStringType)
            ENUMERATOR_SETUP(TW_FIX32, EnumType, CTL_EnumeratorTWFIX32Type)
            if ( pStatus )
                *pStatus = DTWAIN_ERR_WRONG_ARRAY_TYPE;
            return NULL;
        }

        static void CopyArraysFromEnumerator(LPVOID destEnum, LPVOID sourceEnum, CTL_EnumeratorType enumToCheck)
        {

            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorPtrType,         LPVOID)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorIntType,         int)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck,  CTL_EnumeratorInt64Type,      LONG64)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorStringType,      CTL_StringType)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorANSIStringType,  CTL_String)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorWideStringType,  CTL_WString)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorHandleType,         HANDLE)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorSourceType,      CTL_ITwainSourcePtr)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorDTWAINFrameType, DTWAINFrameInternal)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorTWFrameType,     TW_FRAME)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorDoubleType,      double)
            ENUMERATOR_COPY(sourceEnum, destEnum, enumToCheck, CTL_EnumeratorTWFIX32Type,     TW_FIX32)
        }

        static bool EnumeratorAddValue(LPVOID pEnum, LPVOID value)
        {
            ENUMERATOR_PUSH_BACK(pEnum, value, LPVOID)
            ENUMERATOR_PUSH_BACK(pEnum, value, double)
            ENUMERATOR_PUSH_BACK(pEnum, value, int)
            ENUMERATOR_PUSH_BACK(pEnum, value, LONG64)
            ENUMERATOR_PUSH_BACK(pEnum, value, CTL_String)
            ENUMERATOR_PUSH_BACK(pEnum, value, CTL_WString)
            ENUMERATOR_PUSH_BACK(pEnum, value, CTL_StringType)
            ENUMERATOR_PUSH_BACK(pEnum, value, CTL_ITwainSourcePtr)
            ENUMERATOR_PUSH_BACK(pEnum, value, HANDLE)
            ENUMERATOR_PUSH_BACK(pEnum, value, DTWAINFrameInternal)
            ENUMERATOR_PUSH_BACK(pEnum, value, TW_FRAME)
            ENUMERATOR_PUSH_BACK(pEnum, value, TW_FIX32)
            return false;
        }

        static bool EnumeratorAddValue(LPVOID pEnum, LPVOID value, LONG numValues)
        {
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, LPVOID)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, double)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, int)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, LONG64)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, CTL_StringType)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, CTL_String)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, CTL_WString)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, CTL_ITwainSourcePtr)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, HANDLE)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, DTWAINFrameInternal)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, TW_FRAME)
            ENUMERATOR_PUSH_BACK_MULTIPLE(pEnum, value, numValues, TW_FIX32)
            return false;
        }

        static void ClearEnumerator(LPVOID pEnum)
        {
            ENUMERATOR_CLEAR(pEnum, LPVOID)
            ENUMERATOR_CLEAR(pEnum, double)
            ENUMERATOR_CLEAR(pEnum, int)
            ENUMERATOR_CLEAR(pEnum, LONG64)
            ENUMERATOR_CLEAR(pEnum, CTL_StringType)
            ENUMERATOR_CLEAR(pEnum, CTL_String)
            ENUMERATOR_CLEAR(pEnum, CTL_WString)
            ENUMERATOR_CLEAR(pEnum, CTL_ITwainSourcePtr)
            ENUMERATOR_CLEAR(pEnum, HANDLE)
            ENUMERATOR_CLEAR(pEnum, DTWAINFrameInternal)
            ENUMERATOR_CLEAR(pEnum, TW_FRAME)
            ENUMERATOR_CLEAR(pEnum, TW_FIX32)
        }

        static bool EnumeratorGetAt(LPVOID pEnum, LONG nWhere, LPVOID retValue)
        {
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, LPVOID)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, double)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, int)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, LONG64)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, CTL_StringType)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, CTL_String)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, CTL_WString)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, CTL_ITwainSourcePtr)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, HANDLE)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, DTWAINFrameInternal)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, TW_FRAME)
            ENUMERATOR_GET_AT(pEnum, nWhere, retValue, TW_FIX32)
            return false;
        }

        static int EnumeratorFindValue(LPVOID pEnum, LPVOID value)
        {
            ENUMERATOR_FIND_VALUE(pEnum, value, LPVOID)
            ENUMERATOR_FIND_VALUE(pEnum, value, double)
            ENUMERATOR_FIND_VALUE(pEnum, value, int)
            ENUMERATOR_FIND_VALUE(pEnum, value, LONG64)
            ENUMERATOR_FIND_VALUE(pEnum, value, CTL_StringType)
            ENUMERATOR_FIND_VALUE(pEnum, value, CTL_String)
            ENUMERATOR_FIND_VALUE(pEnum, value, CTL_WString)
            ENUMERATOR_FIND_VALUE(pEnum, value, CTL_ITwainSourcePtr)
            ENUMERATOR_FIND_VALUE(pEnum, value, HANDLE)
            ENUMERATOR_FIND_VALUE(pEnum, value, DTWAINFrameInternal)
            ENUMERATOR_FIND_VALUE(pEnum, value, TW_FRAME)
            ENUMERATOR_FIND_VALUE(pEnum, value, TW_FIX32)
            return -1;
        }

        static int EnumeratorFindValue(LPVOID pEnum, double value, double tolerance)
        {
            ENUMERATOR_FIND_VALUE_WITH_TOLERANCE(pEnum, value, tolerance)
            return -1;
        }

        static void ResizeEnumerator(LPVOID pEnum, LONG numValues)
        {
            ENUMERATOR_RESIZE(pEnum, numValues, LPVOID)
            ENUMERATOR_RESIZE(pEnum, numValues, double)
            ENUMERATOR_RESIZE(pEnum, numValues, int)
            ENUMERATOR_RESIZE(pEnum, numValues, LONG64)
            ENUMERATOR_RESIZE(pEnum, numValues, CTL_StringType)
            ENUMERATOR_RESIZE(pEnum, numValues, CTL_String)
            ENUMERATOR_RESIZE(pEnum, numValues, CTL_WString)
            ENUMERATOR_RESIZE(pEnum, numValues, CTL_ITwainSourcePtr)
            ENUMERATOR_RESIZE(pEnum, numValues, HANDLE)
            ENUMERATOR_RESIZE(pEnum, numValues, DTWAINFrameInternal)
            ENUMERATOR_RESIZE(pEnum, numValues, TW_FRAME)
            ENUMERATOR_RESIZE(pEnum, numValues, TW_FIX32)
        }

        static bool EnumeratorDestroy(LPVOID pEnum)
        {
            ENUMERATOR_DESTROY_ARRAY(pEnum, LPVOID)
            ENUMERATOR_DESTROY_ARRAY(pEnum, double)
            ENUMERATOR_DESTROY_ARRAY(pEnum, int)
            ENUMERATOR_DESTROY_ARRAY(pEnum, LONG64)
            ENUMERATOR_DESTROY_ARRAY(pEnum, CTL_StringType)
            ENUMERATOR_DESTROY_ARRAY(pEnum, CTL_String)
            ENUMERATOR_DESTROY_ARRAY(pEnum, CTL_WString)
            ENUMERATOR_DESTROY_ARRAY(pEnum, CTL_ITwainSourcePtr)
            ENUMERATOR_DESTROY_ARRAY(pEnum, HANDLE)
            ENUMERATOR_DESTROY_ARRAY(pEnum, DTWAINFrameInternal)
            ENUMERATOR_DESTROY_ARRAY(pEnum, TW_FRAME)
            ENUMERATOR_DESTROY_ARRAY(pEnum, TW_FIX32)
            return false;
        }

        static void EnumeratorRemoveAt(LPVOID pEnum, LONG nWhere)
        {
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, LPVOID)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, double)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, int)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, LONG64)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, CTL_StringType)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, CTL_String)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, CTL_WString)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, CTL_ITwainSourcePtr)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, HANDLE)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, DTWAINFrameInternal)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, TW_FRAME)
            ENUMERATOR_REMOVE_AT(pEnum, nWhere, TW_FIX32)
        }

        static void EnumeratorRemoveAt(LPVOID pEnum, LONG nWhere, LONG numItems)
        {
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, LPVOID)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, double)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, int)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, LONG64)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, CTL_StringType)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, CTL_String)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, CTL_WString)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, CTL_ITwainSourcePtr)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, HANDLE)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, DTWAINFrameInternal)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, TW_FRAME)
            ENUMERATOR_REMOVE_AT_MULTIPLE(pEnum, nWhere, numItems, TW_FIX32)
        }

        static void EnumeratorInsertAt(LPVOID pEnum, LONG nWhere, LPVOID value)
        {
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, LPVOID)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, double)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, int)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, LONG64)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, CTL_StringType)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, CTL_String)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, CTL_WString)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, CTL_ITwainSourcePtr)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, HANDLE)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, DTWAINFrameInternal)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, TW_FRAME)
            ENUMERATOR_INSERT_AT(pEnum, nWhere, value, TW_FIX32)
        }

        static void EnumeratorInsertAt(LPVOID pEnum, LONG nWhere, LONG numValues, LPVOID value)
        {
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, LPVOID)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, double)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, int)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, LONG64)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, CTL_StringType)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, CTL_String)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, CTL_ITwainSourcePtr)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, CTL_WString)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, HANDLE)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, DTWAINFrameInternal)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, TW_FRAME)
            ENUMERATOR_INSERT_RANGE(pEnum, nWhere, value, numValues, TW_FIX32)
        }

        static void EnumeratorSetAt(LPVOID pEnum, LONG nWhere, LPVOID value)
        {
            ENUMERATOR_SET_AT(pEnum, nWhere, value, LPVOID)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, double)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, int)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, LONG64)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, CTL_StringType)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, CTL_String)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, CTL_WString)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, CTL_ITwainSourcePtr)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, HANDLE)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, DTWAINFrameInternal)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, TW_FRAME)
            ENUMERATOR_SET_AT(pEnum, nWhere, value, TW_FIX32)
        }

        static int EnumeratorGetCount(LPVOID pEnum)
        {
            ENUMERATOR_COUNT(pEnum, LPVOID)
            ENUMERATOR_COUNT(pEnum, double)
            ENUMERATOR_COUNT(pEnum, int)
            ENUMERATOR_COUNT(pEnum, LONG64)
            ENUMERATOR_COUNT(pEnum, CTL_StringType)
            ENUMERATOR_COUNT(pEnum, CTL_String)
            ENUMERATOR_COUNT(pEnum, CTL_WString)
            ENUMERATOR_COUNT(pEnum, CTL_ITwainSourcePtr)
            ENUMERATOR_COUNT(pEnum, HANDLE)
            ENUMERATOR_COUNT(pEnum, DTWAINFrameInternal)
            ENUMERATOR_COUNT(pEnum, TW_FRAME)
            ENUMERATOR_COUNT(pEnum, TW_FIX32)
            return -1;
        }

        static LPVOID EnumeratorGetBuffer(LPVOID pEnum, LONG offset)
        {
            ENUMERATOR_GET_BUFFER(pEnum, offset, LPVOID)
            ENUMERATOR_GET_BUFFER(pEnum, offset, double)
            ENUMERATOR_GET_BUFFER(pEnum, offset, int)
            ENUMERATOR_GET_BUFFER(pEnum, offset, LONG64)
            ENUMERATOR_GET_BUFFER(pEnum, offset, CTL_StringType)
            ENUMERATOR_GET_BUFFER(pEnum, offset, CTL_String)
            ENUMERATOR_GET_BUFFER(pEnum, offset, CTL_WString)
            ENUMERATOR_GET_BUFFER(pEnum, offset, CTL_ITwainSourcePtr)
            ENUMERATOR_GET_BUFFER(pEnum, offset, HANDLE)
            ENUMERATOR_GET_BUFFER(pEnum, offset, DTWAINFrameInternal)
            ENUMERATOR_GET_BUFFER(pEnum, offset, TW_FRAME)
            ENUMERATOR_GET_BUFFER(pEnum, offset, TW_FIX32)
            return NULL;
        }

        static LPVOID EnumeratorGetVector(LPVOID pEnum)
        {
            ENUMERATOR_GET_VECTOR(pEnum, LPVOID)
            ENUMERATOR_GET_VECTOR(pEnum, double)
            ENUMERATOR_GET_VECTOR(pEnum, int)
            ENUMERATOR_GET_VECTOR(pEnum, LONG64)
            ENUMERATOR_GET_VECTOR(pEnum, CTL_StringType)
            ENUMERATOR_GET_VECTOR(pEnum, CTL_String)
            ENUMERATOR_GET_VECTOR(pEnum, CTL_WString)
            ENUMERATOR_GET_VECTOR(pEnum, CTL_ITwainSourcePtr)
            ENUMERATOR_GET_VECTOR(pEnum, HANDLE)
            ENUMERATOR_GET_VECTOR(pEnum, DTWAINFrameInternal)
            ENUMERATOR_GET_VECTOR(pEnum, TW_FRAME)
            ENUMERATOR_GET_VECTOR(pEnum, TW_FIX32)
            return NULL;
        }

        static bool EnumeratorClearExisting(LPDTWAIN_ARRAY pEnum)
        {
            if (pEnum && EnumeratorFunctionImpl::EnumeratorIsValidNoCheck(*pEnum))
            {
                EnumeratorFunctionImpl::ClearEnumerator(*pEnum);
                return true;
            }
            return false;
        }
    };

    template <typename T>
    struct EnumeratorGetter
    {
        static typename CTL_EnumeratorNode<T>::container_pointer_type asVectorPtr(DTWAIN_ARRAY theArray)
        {
            return static_cast<typename CTL_EnumeratorNode<T>::container_pointer_type>(EnumeratorFunctionImpl::EnumeratorGetVector(theArray));
        }

        static typename CTL_EnumeratorNode<T>::container_base_type& asVectorRef(DTWAIN_ARRAY theArray)
        {
            return *asVectorPtr(theArray);
        }
    };

    template <typename T>
    typename CTL_EnumeratorNode<T>::container_base_type& EnumeratorVector(DTWAIN_ARRAY theArray)
    {
        return EnumeratorGetter<T>::asVectorRef(theArray);
    }

    template <typename T>
    typename CTL_EnumeratorNode<T>::container_pointer_type EnumeratorVectorPtr(DTWAIN_ARRAY theArray)
    {
        return EnumeratorGetter<T>::asVectorPtr(theArray);
    }

    struct DTWAINArrayLowLevel_DestroyTraits
    {
        static void Destroy(DTWAIN_ARRAY a)
        {
            EnumeratorFunctionImpl::EnumeratorDestroy(a);
        }
    };

    typedef DTWAIN_RAII<DTWAIN_ARRAY, DTWAINArrayLowLevel_DestroyTraits> DTWAINArrayLL_RAII;
}
#endif
