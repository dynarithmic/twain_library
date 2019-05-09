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
template <class T>
CTL_CapabilitySetRangeTriplet<T>::CTL_CapabilitySetRangeTriplet(CTL_ITwainSession *pSession,
                                                             CTL_ITwainSource* pSource,
                                                             CTL_EnumSetType sType,
                                                             CTL_EnumCapability sCap,
                                                             TW_UINT16 TwainType,
                                                             const std::vector<T>& rArray)
                       :  CTL_CapabilitySetTriplet<T>(pSession, pSource, sType, sCap, TwainType, rArray)
{}


template <class T>
TW_UINT16 CTL_CapabilitySetRangeTriplet<T>::GetContainerTypeSize()
{
    return sizeof(TW_RANGE);
}

template <class T>
size_t CTL_CapabilitySetRangeTriplet<T>::GetAggregateSize()
{
    return 0;
}

template <class T>
TW_UINT16 CTL_CapabilitySetRangeTriplet<T>::GetContainerType()
{
    return TWON_RANGE;
}


template <class T>
bool CTL_CapabilitySetRangeTriplet<T>::Encode(const std::vector<T>& rArray, void *pMemBlock)
{
	T Data1, Data2, Data3;

    // Get a TW_RANGE structure
    pTW_RANGE pVal = (pTW_RANGE)pMemBlock;
    if (rArray.size() >= 3)
    {
        Data1 = rArray[0]; // Min value
        Data2 = rArray[1]; // Max value
        Data3 = rArray[2]; // Step value
        CTL_CapabilitySetTripletBase::EncodeRange(pVal, &Data1, &Data2, &Data3);
        pVal->DefaultValue = TWON_DONTCARE32;
        return true;
    }
    return false;
}

