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
template <class T>
CTL_CapabilitySetArrayTriplet<T>::CTL_CapabilitySetArrayTriplet(CTL_ITwainSession *pSession,
                                                             CTL_ITwainSource* pSource,
                                                             CTL_EnumSetType sType,
                                                             CTL_EnumCapability sCap,
                                                             TW_UINT16 TwainType,
                                                             const std::vector<T>& rArray)
                                                             : CTL_CapabilitySetTriplet<T>(pSession, pSource, sType, sCap, TwainType, rArray), m_nAggSize(rArray.size())
{}



template <class T>
TW_UINT16 CTL_CapabilitySetArrayTriplet<T>::GetContainerTypeSize()
{
    return sizeof(TW_ARRAY);
}


template <class T>
size_t CTL_CapabilitySetArrayTriplet<T>::GetAggregateSize()
{
    return m_nAggSize;
}


template <class T>
TW_UINT16 CTL_CapabilitySetArrayTriplet<T>::GetContainerType()
{
    return TWON_ARRAY;
}


template <class T>
bool CTL_CapabilitySetArrayTriplet<T>::Encode(const std::vector<T>& rArray, void *pMemBlock)
{
    // Get a TW_RANGE structure
    pTW_ARRAY pArray = (pTW_ARRAY)pMemBlock;

    // Set the # of elements
    pArray->NumItems = (TW_UINT32)m_nAggSize;

    // Set the data type
    pArray->ItemType = CTL_CapabilitySetTripletBase::GetTwainType();

    // Set the items in the list
    size_t i = 0;
    for_each(rArray.begin(), rArray.begin() + m_nAggSize, [&](T Data)
    {
        this->EncodeArrayValue(pArray, i, &Data);
        ++i;
    });
    return true;
}
