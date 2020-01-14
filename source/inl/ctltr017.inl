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
template <class T>
CTL_CapabilitySetOneValTriplet<T>::CTL_CapabilitySetOneValTriplet(CTL_ITwainSession *pSession,
                                                                CTL_ITwainSource* pSource,
                                                                CTL_EnumSetType sType,
                                                                TW_UINT16	sCap,
                                                                TW_UINT16 TwainType,
                                                                const std::vector<T>& rArray)
                       :  CTL_CapabilitySetTriplet<T>(pSession, pSource, sType, sCap,TwainType,rArray)
{}


template <class T>
TW_UINT16 CTL_CapabilitySetOneValTriplet<T>::GetContainerTypeSize()
{
    return sizeof(TW_ONEVALUE);
}


template <class T>
size_t CTL_CapabilitySetOneValTriplet<T>::GetAggregateSize()
{
    return 0;
}


template <class T>
TW_UINT16 CTL_CapabilitySetOneValTriplet<T>::GetContainerType()
{
    return TWON_ONEVALUE;
}


template <class T>
bool CTL_CapabilitySetOneValTriplet<T>::Encode(const std::vector<T>& rArray, void *pMemBlock)
{
    T Data;

    // Get a TW_ONEVALUE structure
    pTW_ONEVALUE pVal = (pTW_ONEVALUE)pMemBlock;

    // Get the TWTY_xxx type
    pVal->ItemType = CTL_CapabilitySetTripletBase::GetTwainType();

    // Get the data
    if (!rArray.empty())
    {
        Data = rArray[0];
        CTL_CapabilitySetTripletBase::EncodeOneValue(pVal, &Data);
        return true;
    }
    return false;
}

