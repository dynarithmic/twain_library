/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2025 Dynarithmic Software.

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
#include <dynarithmic/twain/info/imprinter_info.hpp>
#include <dynarithmic/twain/dtwain_twain.hpp>
#include <dynarithmic/twain/capability_interface/capability_interface.hpp>
#include <dynarithmic/twain/acquire_characteristics/acquire_characteristics.hpp>
#include <dynarithmic/twain/source/twain_source.hpp>
#include <dynarithmic/twain/extimageinfo/extendedimage_info.hpp>
namespace dynarithmic
{
	namespace twain
	{
		bool extendedimage_info::attach(twain_source* pSource)
		{
			m_pSource = pSource;
			auto ret = API_INSTANCE DTWAIN_InitExtImageInfo(pSource->get_source());
			if (ret)
			{
				clear_infos();
                auto& ci = m_pSource->get_capability_interface();
                m_vSupportedExtInfo = ci.get_supportedextimageinfo();
				fillbarcode_info();
                fillshadedarea_info();
                fillspeckleremoval_info();
                filllinedetection_info();
                fillskewdetection_info();
                fillendorsedtext_info();
                fillformsrecognition_info();
                fillpagesource_info();
                fillimagesegmentation_info();
                fillextendedimageinfo20_info();
                fillextendedimageinfo21_info();
                fillextendedimageinfo22_info();
                fillextendedimageinfo23_info();
                fillextendedimageinfo24_info();
                fillextendedimageinfo25_info();
            }
			return ret;
		}

		bool extendedimage_info::detach()
		{
			if (m_pSource && m_bDoDetach)
			{
                clear_infos();
				return API_INSTANCE DTWAIN_FreeExtImageInfo(m_pSource->get_source());
			}
			return true;
		}

        std::string extendedimage_info::get_text_from_info(DTWAIN_ARRAY aText, int nWhichText)
        {
            int nLen = API_INSTANCE DTWAIN_ArrayGetStringLength(aText, nWhichText);
            if (nLen > 0)
            {
                std::string temp;
                temp.resize(nLen + 1);
                API_INSTANCE DTWAIN_ArrayGetAtANSIString(aText, nWhichText, &temp[0]);
                return temp;
            }
            return {};
        }

		void extendedimage_info::fillbarcode_info()
		{
            auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), TWEI_BARCODECOUNT);
            if (iter == m_vSupportedExtInfo.end())
                return;

            static constexpr std::array<std::pair<int, int>, barcode_info::TEXT_SUPPORTED + 1> aAllData = { {
                {barcode_info::CONFIDENCE_SUPPORTED, TWEI_BARCODECONFIDENCE},
                {barcode_info::ROTATION_SUPPORTED, TWEI_BARCODEROTATION},
                {barcode_info::XCOORDINATE_SUPPORTED, TWEI_BARCODEX},
                {barcode_info::YCOORDINATE_SUPPORTED, TWEI_BARCODEY},
                {barcode_info::TYPE_SUPPORTED, TWEI_BARCODETYPE},
                {barcode_info::TEXTLENGTH_SUPPORTED, TWEI_BARCODETEXTLENGTH},
                {barcode_info::TEXT_SUPPORTED, TWEI_BARCODETEXT}}};

            using memPtr = decltype(&barcode_info::barcode_single_info::m_xCoordinate);
            std::vector<memPtr> individual_items = { &barcode_info::barcode_single_info::m_confidence ,
                                                     &barcode_info::barcode_single_info::m_rotation ,
                                                     &barcode_info::barcode_single_info::m_xCoordinate ,
                                                     &barcode_info::barcode_single_info::m_yCoordinate ,
                                                     &barcode_info::barcode_single_info::m_type,
                                                     &barcode_info::barcode_single_info::m_length };

			// Get the barcode information
			DTWAIN_ARRAY aValues = {};
			API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_BARCODECOUNT, &aValues);
            if (!aValues)
                return;
			twain_array tArray;
			tArray.set_array(aValues);
			if (tArray.get_count() > 0)
			{
				m_barcodeInfo.count = static_cast<uint32_t>(tArray.at<LONG>(0));
				m_barcodeInfo.set_count(m_barcodeInfo.count);
			}
			auto& barCodeInfos = m_barcodeInfo.get_barcode_infos();
			std::array<DTWAIN_ARRAY, aAllData.size()> aDTwainArrays = {};
            for (auto& pr : aAllData)
			    m_barcodeInfo.m_supported[pr.first] = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), pr.second, &aDTwainArrays[pr.first]);

            // Get the texts
            m_barcodeInfo.m_supported[barcode_info::TEXT_SUPPORTED] = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_BARCODETEXT, &aDTwainArrays[barcode_info::TEXT_SUPPORTED]);

            std::array<twain_array, aAllData.size()> aTwainArrays;
			int curArray = 0;
			for (auto& val : aTwainArrays)
			{
				if (aDTwainArrays[curArray])
					val.set_array(aDTwainArrays[curArray]);
				else
					val.set_array(nullptr);
				++curArray;
			}

			for (size_t curInfo = 0; curInfo < barCodeInfos.size(); ++curInfo)
			{
                auto* curObject = &barCodeInfos[curInfo];
                for (size_t i = 0; i < aAllData.size(); ++i)
                {
                    if (aAllData[i].first != barcode_info::TEXT_SUPPORTED)
                    {
                        auto pItem = individual_items[i];
                        if (aDTwainArrays[i])
                        {
                            if (aTwainArrays[i].get_count() > 0)
                                curObject->*pItem = aTwainArrays[i].at<LONG>(curInfo);
                        }
                    }
                    else
                    {
                        curObject->m_text = get_text_from_info(aDTwainArrays[barcode_info::TEXT_SUPPORTED], static_cast<int>(curInfo));
                    }
                }
            }
		}

        void extendedimage_info::fillshadedarea_info()
        {
            auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), TWEI_DESHADECOUNT);
            if (iter == m_vSupportedExtInfo.end())
                return;
            static constexpr std::array<std::pair<int, int>, 14> aAllData = { {
                {shadedarea_detection_info::LEFT_SUPPORTED, TWEI_DESHADELEFT},
                {shadedarea_detection_info::TOP_SUPPORTED, TWEI_DESHADETOP},
                {shadedarea_detection_info::WIDTH_SUPPORTED, TWEI_DESHADEWIDTH},
                {shadedarea_detection_info::HEIGHT_SUPPORTED, TWEI_DESHADEHEIGHT},
                {shadedarea_detection_info::SIZE_SUPPORTED, TWEI_DESHADESIZE},
                {shadedarea_detection_info::BLACKCOUNTOLD_SUPPORTED, TWEI_DESHADEBLACKCOUNTOLD},
                {shadedarea_detection_info::BLACKCOUNTNEW_SUPPORTED, TWEI_DESHADEBLACKCOUNTNEW},
                {shadedarea_detection_info::BLACKCOUNTRLMIN_SUPPORTED, TWEI_DESHADEBLACKRLMIN},
                {shadedarea_detection_info::BLACKCOUNTRLMAX_SUPPORTED, TWEI_DESHADEBLACKRLMAX},
                {shadedarea_detection_info::WHITECOUNTOLD_SUPPORTED, TWEI_DESHADEWHITECOUNTOLD},
                {shadedarea_detection_info::WHITECOUNTNEW_SUPPORTED, TWEI_DESHADEWHITECOUNTNEW},
                {shadedarea_detection_info::WHITECOUNTRLMIN_SUPPORTED, TWEI_DESHADEWHITERLMIN},
                {shadedarea_detection_info::WHITECOUNTRLMAX_SUPPORTED, TWEI_DESHADEWHITERLMAX},
                {shadedarea_detection_info::WHITECOUNTRLAVG_SUPPORTED, TWEI_DESHADEWHITERLAVE}} };

            using memPtr = decltype(&shadedarea_detection_info::shadedarea_single_info::left);
            std::vector<memPtr> individual_items = {
                                                     &shadedarea_detection_info::shadedarea_single_info::left,
                                                     &shadedarea_detection_info::shadedarea_single_info::top,
                                                     &shadedarea_detection_info::shadedarea_single_info::width,
                                                     &shadedarea_detection_info::shadedarea_single_info::height,
                                                     &shadedarea_detection_info::shadedarea_single_info::size,
                                                     &shadedarea_detection_info::shadedarea_single_info::blackCountOld,
                                                     &shadedarea_detection_info::shadedarea_single_info::blackCountNew,
                                                     &shadedarea_detection_info::shadedarea_single_info::blackRLMin,
                                                     &shadedarea_detection_info::shadedarea_single_info::blackRLMax,
                                                     &shadedarea_detection_info::shadedarea_single_info::whiteCountOld,
                                                     &shadedarea_detection_info::shadedarea_single_info::whiteCountNew,
                                                     &shadedarea_detection_info::shadedarea_single_info::whiteRLMin,
                                                     &shadedarea_detection_info::shadedarea_single_info::whiteRLMax,
                                                     &shadedarea_detection_info::shadedarea_single_info::whiteRLAverage
                                                    };


            // Get the shaded information
            DTWAIN_ARRAY aValues = {};
            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_DESHADECOUNT, &aValues);
            twain_array tArray;
            tArray.set_array(aValues);
            if (tArray.get_count() > 0)
            {
                m_shadedareaInfo.count = static_cast<uint32_t>(tArray.at<LONG>(0));
                m_shadedareaInfo.set_count(m_shadedareaInfo.count);
            }
            auto& shadedAreaInfos = m_shadedareaInfo.get_shaded_area_infos();
            std::array<DTWAIN_ARRAY, 14> aDTwainArrays = {};
            for (auto& pr : aAllData)
                m_shadedareaInfo.m_supported[pr.first] = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), pr.second, &aDTwainArrays[pr.first]);

            std::array<twain_array, 14> aTwainArrays;
            int curArray = 0;
            for (auto& val : aTwainArrays)
            {
                if (aDTwainArrays[curArray])
                    val.set_array(aDTwainArrays[curArray]);
                else
                    val.set_array(nullptr);
                ++curArray;
            }

            for (size_t curInfo = 0; curInfo < shadedAreaInfos.size(); ++curInfo)
            {
                for (int i = 0; i < 14; ++i)
                {
                    auto pItem = individual_items[curInfo];
                    auto* curObject = &shadedAreaInfos[curInfo];
                    if (aDTwainArrays[i])
                        curObject->*pItem = aTwainArrays[i].get_count() > 0 ? aTwainArrays[i].at<LONG>(curInfo) : 0;
                }
            }
        }

        void extendedimage_info::fillspeckleremoval_info()
        {
            auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), TWEI_SPECKLESREMOVED);
            if (iter == m_vSupportedExtInfo.end())
                return;

            static constexpr std::array<std::pair<int, int>, 3> aAllData = { {
                {speckle_removal_info::SPECKLESREMOVED_SUPPORTED, TWEI_SPECKLESREMOVED},
                {speckle_removal_info::BLACKSPECKLESREMOVED_SUPPORTED, TWEI_BLACKSPECKLESREMOVED},
                {speckle_removal_info::SPECKLESREMOVED_SUPPORTED, TWEI_WHITESPECKLESREMOVED}
                } };

            using memPtr = decltype(&speckle_removal_info::specklesRemoved);
            std::vector<memPtr> individual_items = { &speckle_removal_info::specklesRemoved,
                                                    &speckle_removal_info::blackSpecklesRemoved,
                                                    &speckle_removal_info::whiteSpecklesRemoved };
            // Get the speckle removal information
            std::array<DTWAIN_ARRAY, 3> aDTwainArrays = {};
            for (auto& pr : aAllData)
                m_speckleremovalInfo.m_supported[pr.first] = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(),
                    pr.second, &aDTwainArrays[pr.first]);

            std::array<twain_array, 3> aTwainArrays;
            int curArray = 0;
            for (auto& val : aTwainArrays)
            {
                if (aDTwainArrays[curArray])
                    val.set_array(aDTwainArrays[curArray]);
                else
                    val.set_array(nullptr);
                ++curArray;
            }

            auto* curObject = &m_speckleremovalInfo;
            for (int i = 0; i < 3; ++i)
            {
                auto pItem = individual_items[i];
                if (aDTwainArrays[i])
                    curObject->*pItem = aTwainArrays[i].get_count() > 0 ? aTwainArrays[i].at<LONG>(0) : 0;
            }
        }

        void extendedimage_info::fillformsrecognition_info()
        {
            static constexpr std::array<std::pair<int, int>, 5> aAllData = { {
                {formsrecognition_info::FORMTEMPLATEMATCH_SUPPORTED, TWEI_FORMTEMPLATEMATCH},
                {formsrecognition_info::FORMCONFIDENCE_SUPPORTED, TWEI_FORMCONFIDENCE},
                {formsrecognition_info::FORMTEMPLATEPAGEMATCH_SUPPORTED, TWEI_FORMTEMPLATEPAGEMATCH},
                {formsrecognition_info::FORMHORZDOCOFFSET_SUPPORTED, TWEI_FORMHORZDOCOFFSET},
                {formsrecognition_info::FORMVERTDOCOFFSET_SUPPORTED, TWEI_FORMVERTDOCOFFSET}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_formsrecognitionInfo.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            using memPtr = decltype(&formsrecognition_info::vFormConfidence);
            std::vector<memPtr> individual_items = { nullptr,
                                                    &formsrecognition_info::vFormConfidence,
                                                    &formsrecognition_info::vFormTemplatePageMatch,
                                                    &formsrecognition_info::vFormHorizDocOffset,
                                                    &formsrecognition_info::vFormVertDocOffset };

            std::array<DTWAIN_ARRAY, 5> aDTwainArrays = {};
            DTWAIN_ARRAY aText = {};
            m_formsrecognitionInfo.m_supported[formsrecognition_info::FORMTEMPLATEMATCH_SUPPORTED] =
                API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_FORMTEMPLATEMATCH, &aText);
            if (aText && m_formsrecognitionInfo.m_supported[formsrecognition_info::FORMTEMPLATEMATCH_SUPPORTED])
            {
                twain_array twText;
                twText.set_array(aText);
                LONG nCount = API_INSTANCE DTWAIN_ArrayGetCount(aText);
                for (LONG i = 0; i < nCount; ++i)
                    m_formsrecognitionInfo.vTemplateMatch.push_back(get_text_from_info(aText,i));
            }

            for (auto& pr : aAllData)
            {
                if (pr.first != TWEI_FORMTEMPLATEMATCH && m_shadedareaInfo.m_supported[pr.first])
                    API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), pr.second, &aDTwainArrays[pr.first]);
            }

            std::array<twain_array, 5> aTwainArrays;
            int curArray = 0;
            for (auto& val : aTwainArrays)
            {
                if (individual_items[curArray] && aDTwainArrays[curArray])
                    val.set_array(aDTwainArrays[curArray]);
                else
                    val.set_array(nullptr);
                ++curArray;
            }

            auto* curObject = &m_formsrecognitionInfo;
            for (int i = 1; i < 5; ++i)
            {
                auto pItem = individual_items[i];
                if (aDTwainArrays[i])
                {
                    auto* pBuffer = static_cast<TW_UINT32*>(API_INSTANCE DTWAIN_ArrayGetBuffer(aDTwainArrays[i], 0));
                    LONG nItems = API_INSTANCE DTWAIN_ArrayGetCount(aDTwainArrays[i]);
                    curObject->*pItem = std::vector<TW_UINT32>(pBuffer, pBuffer + nItems);
                }
            }
        }

        void extendedimage_info::fillendorsedtext_info()
        {
            auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), TWEI_FORMTEMPLATEMATCH);
            if (iter == m_vSupportedExtInfo.end())
                return;

            DTWAIN_ARRAY aEndorsedText = {};
            m_endorsedtextInfo.m_supported[endorsed_text_info::ENDORSEDTEXT_SUPPORTED] =
                API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_ENDORSEDTEXT, &aEndorsedText);
            if (aEndorsedText && m_endorsedtextInfo.m_supported[endorsed_text_info::ENDORSEDTEXT_SUPPORTED])
            {
                twain_array ta;
                ta.set_array(aEndorsedText);
                m_endorsedtextInfo.endorsed_text = get_text_from_info(aEndorsedText, 0);
            }
        }


        void extendedimage_info::fillskewdetection_info()
        {
            static constexpr std::array<std::pair<int, int>, 12> aAllData = { {
                {skew_detection_info::DESKEWSTATUS_SUPPORTED, TWEI_DESKEWSTATUS},
                {skew_detection_info::ORIGINALANGLE_SUPPORTED, TWEI_SKEWORIGINALANGLE},
                {skew_detection_info::FINALANGLE_SUPPORTED, TWEI_SKEWFINALANGLE},
                {skew_detection_info::CONFIDENCE_SUPPORTED, TWEI_SKEWCONFIDENCE},
                {skew_detection_info::WINDOWX1_SUPPORTED, TWEI_SKEWWINDOWX1},
                {skew_detection_info::WINDOWX2_SUPPORTED, TWEI_SKEWWINDOWX2},
                {skew_detection_info::WINDOWX3_SUPPORTED, TWEI_SKEWWINDOWX3},
                {skew_detection_info::WINDOWX4_SUPPORTED, TWEI_SKEWWINDOWX4},
                {skew_detection_info::WINDOWY1_SUPPORTED, TWEI_SKEWWINDOWY1},
                {skew_detection_info::WINDOWY2_SUPPORTED, TWEI_SKEWWINDOWY2},
                {skew_detection_info::WINDOWY3_SUPPORTED, TWEI_SKEWWINDOWY3},
                {skew_detection_info::WINDOWY4_SUPPORTED, TWEI_SKEWWINDOWY4}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_skewdetectionInfo.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            using memPtr = decltype(&skew_detection_info::originalAngle);
            std::vector<memPtr> individual_items = {nullptr,
                                                    &skew_detection_info::originalAngle,
                                                    &skew_detection_info::finalAngle,
                                                    &skew_detection_info::confidence,
                                                    &skew_detection_info::windowX1,
                                                    &skew_detection_info::windowX2,
                                                    &skew_detection_info::windowX3,
                                                    &skew_detection_info::windowX4,
                                                    &skew_detection_info::windowY1,
                                                    &skew_detection_info::windowY2,
                                                    &skew_detection_info::windowY3,
                                                    &skew_detection_info::windowY4};
            // Get the skew information
            std::array<DTWAIN_ARRAY, 12> aDTwainArrays = {};
            for (auto& pr : aAllData)
            {
                if (individual_items[pr.first] && m_skewdetectionInfo.m_supported[pr.first])
                    API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), pr.second, &aDTwainArrays[pr.first]);
            }

            std::array<twain_array, 12> aTwainArrays;
            int curArray = 0;
            for (auto& val : aTwainArrays)
            {
                if (aDTwainArrays[curArray])
                    val.set_array(aDTwainArrays[curArray]);
                else
                    val.set_array(nullptr);
                ++curArray;
            }

            auto* curObject = &m_skewdetectionInfo;
            for (int i = 1; i < 12; ++i)
            {
                auto pItem = individual_items[i];
                if (aDTwainArrays[i])
                    curObject->*pItem = aTwainArrays[i].get_count() > 0 ? aTwainArrays[i].at<LONG>(0) : 0;
            }
        }

        void extendedimage_info::filllinedetection_info()
        {
            filllinedetection_info(TWEI_VERTLINECOUNT,
                { TWEI_VERTLINEXCOORD, TWEI_VERTLINEYCOORD, TWEI_VERTLINELENGTH, TWEI_VERTLINETHICKNESS },
                  m_linedetectionInfo.m_vVerticalLineInfos);
            m_linedetectionInfo.set_vertical_count(static_cast<uint32_t>(m_linedetectionInfo.m_vVerticalLineInfos.size()));

            filllinedetection_info(TWEI_HORZLINECOUNT,
                { TWEI_HORZLINEXCOORD, TWEI_HORZLINEYCOORD, TWEI_HORZLINELENGTH, TWEI_HORZLINETHICKNESS },
                 m_linedetectionInfo.m_vHorizontalLineInfos);
            m_linedetectionInfo.set_horizontal_count(static_cast<uint32_t>(m_linedetectionInfo.m_vHorizontalLineInfos.size()));
        }

        void extendedimage_info::filllinedetection_info(int nWhichLineInfo, const std::vector<int>& lineInfos,
                                                        std::vector<linedetection_info::linedetection_single_info>& vInfos)
        {
            auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), nWhichLineInfo);
            if (iter == m_vSupportedExtInfo.end())
                return;

            std::array<std::pair<int, int>, 4> aAllData = { {
                {linedetection_info::linedetection_single_info::XCOORD_SUPPORTED, lineInfos[0]},
                {linedetection_info::linedetection_single_info::YCOORD_SUPPORTED, lineInfos[1]},
                {linedetection_info::linedetection_single_info::LENGTH_SUPPORTED, lineInfos[2]},
                {linedetection_info::linedetection_single_info::THICKNESS_SUPPORTED, lineInfos[3]}
                } };

            using memPtr = decltype(&linedetection_info::linedetection_single_info::m_XCoord);
            std::vector<memPtr> individual_items = {
                                                     &linedetection_info::linedetection_single_info::m_XCoord,
                                                     &linedetection_info::linedetection_single_info::m_YCoord,
                                                     &linedetection_info::linedetection_single_info::m_Length,
                                                     &linedetection_info::linedetection_single_info::m_Thickness
                                                    };

            DTWAIN_ARRAY aValues = {};
            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), nWhichLineInfo, &aValues);
            twain_array tArray;
            tArray.set_array(aValues);
            if (tArray.get_count() > 0)
            {
                auto theCount = static_cast<uint32_t>(tArray.at<LONG>(0));
                vInfos.resize(theCount);
            }
            std::array<DTWAIN_ARRAY, 4> aDTwainArrays = {};
            for (auto& pr : aAllData)
                vInfos[pr.first].m_supported[pr.first] = 
                    API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), pr.second, &aDTwainArrays[pr.first]);

            std::array<twain_array, 4> aTwainArrays;
            int curArray = 0;
            for (auto& val : aTwainArrays)
            {
                if (aDTwainArrays[curArray])
                    val.set_array(aDTwainArrays[curArray]);
                else
                    val.set_array(nullptr);
                ++curArray;
            }

            for (size_t curInfo = 0; curInfo < vInfos.size(); ++curInfo)
            {
                for (int i = 0; i < 14; ++i)
                {
                    auto pItem = individual_items[curInfo];
                    auto* curObject = &vInfos[curInfo];
                    if (aDTwainArrays[i])
                        curObject->*pItem = aTwainArrays[i].get_count() > 0 ? aTwainArrays[i].at<LONG>(0) : 0;
                }
            }
        }

        void extendedimage_info::fillpagesource_info()
        {
            static constexpr std::array<std::pair<int, int>, 9> aAllData = { {
                {pagesource_info::BOOKNAME_SUPPORTED, TWEI_BOOKNAME},
                {pagesource_info::CAMERA_SUPPORTED, TWEI_CAMERA},
                {pagesource_info::CHAPTERNUMBER_SUPPORTED, TWEI_CHAPTERNUMBER},
                {pagesource_info::DOCUMENTNUMBER_SUPPORTED, TWEI_DOCUMENTNUMBER},
                {pagesource_info::PAGENUMBER_SUPPORTED, TWEI_PAGENUMBER},
                {pagesource_info::FRAMENUMBER_SUPPORTED, TWEI_FRAMENUMBER},
                {pagesource_info::PIXELFLAVOR_SUPPORTED, TWEI_PIXELFLAVOR},
                {pagesource_info::FRAME_SUPPORTED, TWEI_FRAME}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_pagesourceInfo.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            // Get the string infos
            using memPtr1 = decltype(&pagesource_info::m_bookname);
            std::vector<memPtr1> individual_items1 = { &pagesource_info::m_bookname,
                                                       &pagesource_info::m_camera };

            DTWAIN_ARRAY aText = {};
            auto* curObject = &m_pagesourceInfo;
            for (int i = pagesource_info::BOOKNAME_SUPPORTED; i < pagesource_info::CAMERA_SUPPORTED + 1; ++i)
            {
                if (m_pagesourceInfo.m_supported[i])
                {
                    API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), aAllData[i].second, &aText);
                    if (aText && API_INSTANCE DTWAIN_ArrayGetCount(aText) > 0)
                    {
                        twain_array ta;
                        ta.set_array(aText);
                        auto pItem = individual_items1[i];
                        curObject->*pItem = get_text_from_info(aText, 0);
                    }
                }
            }

            using memPtr2 = decltype(&pagesource_info::m_chapterNumber);
            std::vector<memPtr2> individual_items2 = { &pagesource_info::m_chapterNumber,
                                                       &pagesource_info::m_documentNumber,
                                                       &pagesource_info::m_pageNumber,
                                                       &pagesource_info::m_frameNumber,
                                                       &pagesource_info::m_pixelFlavor };

            DTWAIN_ARRAY aValues = {};
            for (size_t i = pagesource_info::CHAPTERNUMBER_SUPPORTED, curItem = 0; i < pagesource_info::PIXELFLAVOR_SUPPORTED + 1; ++i, ++curItem)
            {
                if (m_pagesourceInfo.m_supported[i])
                {
                    bool bRet = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), aAllData[i].second, &aValues);
                    if (bRet && aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
                    {
                        twain_array ta;
                        ta.set_array(aValues);
                        auto pItem = individual_items2[curItem];
                        curObject->*pItem = ta.at<TW_UINT32>(0);
                    }
                }
            }

            if (m_pagesourceInfo.m_supported[pagesource_info::FRAME_SUPPORTED])
            {
                DTWAIN_ARRAY aFrame = {};
                bool bRet = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_FRAME, &aFrame);
                if (bRet && aFrame)
                {
                    twain_array ta;
                    ta.set_array(aFrame);
                    twain_frame frame;
                    API_INSTANCE DTWAIN_FrameGetAll(aFrame, &frame.left, &frame.top, &frame.right, &frame.bottom);
                    m_pagesourceInfo.m_frame = frame;
                }
            }
        }

        void extendedimage_info::fillimagesegmentation_info()
        {
            static constexpr std::array<std::pair<int, int>, 3> aAllData = { {

                {imagesegmentation_info::ICCPROFILE_SUPPORTED, TWEI_ICCPROFILE},
                {imagesegmentation_info::LASTSEGMENT_SUPPORTED, TWEI_LASTSEGMENT},
                {imagesegmentation_info::SEGMENTNUMBER_SUPPORTED, TWEI_SEGMENTNUMBER}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_imagesegmentationInfo.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            // Get the string infos
            using memPtr1 = decltype(&imagesegmentation_info::m_ICCProfile);
            std::vector<memPtr1> individual_items1 = { &imagesegmentation_info::m_ICCProfile };

            DTWAIN_ARRAY aText = {};
            auto* curObject = &m_imagesegmentationInfo;
            for (int i = imagesegmentation_info::ICCPROFILE_SUPPORTED; i < imagesegmentation_info::ICCPROFILE_SUPPORTED + 1; ++i)
            {
                if (m_imagesegmentationInfo.m_supported[i])
                {
                    API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), aAllData[i].second, &aText);
                    if (aText && API_INSTANCE DTWAIN_ArrayGetCount(aText) > 0)
                    {
                        twain_array ta;
                        ta.set_array(aText);
                        auto pItem = individual_items1[i];
                        curObject->*pItem = get_text_from_info(aText, 0);
                    }
                }
            }

            using memPtr2 = decltype(&imagesegmentation_info::m_bLastSegment);
            std::vector<memPtr2> individual_items2 = { &imagesegmentation_info::m_bLastSegment };

            DTWAIN_ARRAY aValues = {};
            for (int i = imagesegmentation_info::LASTSEGMENT_SUPPORTED, curItem = 0; i < imagesegmentation_info::LASTSEGMENT_SUPPORTED + 1; ++i, ++curItem)
            {
                if (m_imagesegmentationInfo.m_supported[i])
                {
                    bool bRet = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), aAllData[i].second, &aValues);
                    if (bRet && aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
                    {
                        twain_array ta;
                        ta.set_array(aValues);
                        auto pItem = individual_items2[curItem];
                        curObject->*pItem = static_cast<bool>(ta.at<LONG>(0));
                    }
                }
            }

            using memPtr3 = decltype(&imagesegmentation_info::m_SegmentNumber);
            std::vector<memPtr3> individual_items3 = { &imagesegmentation_info::m_SegmentNumber };
            for (int i = imagesegmentation_info::SEGMENTNUMBER_SUPPORTED, curItem = 0; i < imagesegmentation_info::SEGMENTNUMBER_SUPPORTED + 1; ++i, ++curItem)
            {
                if (m_imagesegmentationInfo.m_supported[i])
                {
                    bool bRet = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), aAllData[i].second, &aValues);
                    if (bRet && aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
                    {
                        twain_array ta;
                        ta.set_array(aValues);
                        auto pItem = individual_items3[curItem];
                        curObject->*pItem = ta.at<TW_UINT32>(0);
                    }
                }
            }
        }

        void extendedimage_info::fillextendedimageinfo20_info()
        {
            static constexpr std::array<std::pair<int, int>, 1> aAllData = { {
                {extendedimageinfo_20::MAGTYPE_SUPPORTED, TWEI_MAGTYPE}}};

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_extendedimageinfo20.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            if (!m_extendedimageinfo20.m_supported[extendedimageinfo_20::MAGTYPE_SUPPORTED])
                return;

            DTWAIN_ARRAY aMagType = {};
            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_MAGTYPE, &aMagType);
            if (aMagType && API_INSTANCE DTWAIN_ArrayGetCount(aMagType) > 0)
            {
                twain_array ta;
                ta.set_array(aMagType);
                long lVal = 0;
                API_INSTANCE DTWAIN_ArrayGetAtLong(aMagType, 0, &lVal);
                m_extendedimageinfo20.magtype = static_cast<TW_UINT16>(lVal);
            }
        }

        void extendedimage_info::fillextendedimageinfo21_info()
        {
            static constexpr std::array<std::pair<int, int>, 5> aAllData = { {
                {extendedimageinfo_21::MAGDATALENGTH_SUPPORTED, TWEI_MAGDATALENGTH},
                {extendedimageinfo_21::MAGDATA_SUPPORTED, TWEI_MAGDATA},
                {extendedimageinfo_21::FILESYSTEMSOURCE_SUPPORTED, TWEI_FILESYSTEMSOURCE},
                {extendedimageinfo_21::PAGESIDE_SUPPORTED, TWEI_PAGESIDE},
                {extendedimageinfo_21::IMAGEMERGED_SUPPORTED, TWEI_IMAGEMERGED}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_extendedimageinfo21.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            DTWAIN_ARRAY aValues = {};
            twain_array ta;
            ta.set_array(aValues);
            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_MAGDATALENGTH, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
            {
                LONG lVal = 0;
                API_INSTANCE DTWAIN_ArrayGetAtLong(aValues, 0, &lVal);
                m_extendedimageinfo21.m_maglength = static_cast<uint32_t>(lVal);
            }

            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_MAGDATA, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
            {
                auto strData = get_text_from_info(aValues, 0);
                strData.pop_back();
                m_extendedimageinfo21.m_vMagData = std::vector<char>(strData.begin(), strData.end());
            }

            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_FILESYSTEMSOURCE, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
                m_extendedimageinfo21.m_fileSystemSource = get_text_from_info(aValues, 0);

            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_PAGESIDE, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
            {
                LONG lVal = 0;
                API_INSTANCE DTWAIN_ArrayGetAtLong(aValues, 0, &lVal);
                m_extendedimageinfo21.m_pageSide = static_cast<TW_UINT16>(lVal);
            }

            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_IMAGEMERGED, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
            {
                LONG lVal = 0;
                API_INSTANCE DTWAIN_ArrayGetAtLong(aValues, 0, &lVal);
                m_extendedimageinfo21.m_bImageMerged = lVal;
            }
        }

        void extendedimage_info::fillextendedimageinfo22_info()
        {
            static constexpr std::array<std::pair<int, int>, 1> aAllData = { {
                {extendedimageinfo_22::PAPERCOUNT_SUPPORTED, TWEI_PAPERCOUNT}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_extendedimageinfo22.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            if (!m_extendedimageinfo22.m_supported[extendedimageinfo_22::PAPERCOUNT_SUPPORTED])
                return;

            DTWAIN_ARRAY aValues = {};
            twain_array ta;
            ta.set_array(aValues);
            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_PAPERCOUNT, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
            {
                LONG lVal = 0;
                API_INSTANCE DTWAIN_ArrayGetAtLong(aValues, 0, &lVal);
                m_extendedimageinfo22.paperCount = static_cast<uint32_t>(lVal);
            }
        }

        void extendedimage_info::fillextendedimageinfo23_info()
        {
            static constexpr std::array<std::pair<int, int>, 1> aAllData = { {
                {extendedimageinfo_23::PRINTERTEXT_SUPPORTED, TWEI_PRINTERTEXT}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_extendedimageinfo23.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            if (!m_extendedimageinfo23.m_supported[extendedimageinfo_23::PRINTERTEXT_SUPPORTED])
                return;

            DTWAIN_ARRAY aValues = {};
            twain_array ta;
            ta.set_array(aValues);
            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_PRINTERTEXT, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
                m_extendedimageinfo23.m_printerText = get_text_from_info(aValues, 0);
        }

        void extendedimage_info::fillextendedimageinfo24_info()
        {
            static constexpr std::array<std::pair<int, int>, 1> aAllData = { {
                {extendedimageinfo_24::TWAINDIRECTMETADATA_SUPPORTED, TWEI_TWAINDIRECTMETADATA}
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_extendedimageinfo24.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            if (!m_extendedimageinfo24.m_supported[extendedimageinfo_24::TWAINDIRECTMETADATA_SUPPORTED])
                return;

            DTWAIN_ARRAY aValues = {};
            twain_array ta;
            ta.set_array(aValues);
            API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_TWAINDIRECTMETADATA, &aValues);
            if (aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
                m_extendedimageinfo24.m_twaindirectdata = get_text_from_info(aValues, 0);
        }

        void extendedimage_info::fillextendedimageinfo25_info()
        {
            static constexpr std::array<std::pair<int, int>, 8> aAllData = { {
                {extendedimageinfo_25::IAFIELDA_SUPPORTED, TWEI_IAFIELDA_VALUE},
                {extendedimageinfo_25::IAFIELDB_SUPPORTED, TWEI_IAFIELDB_VALUE},
                {extendedimageinfo_25::IAFIELDC_SUPPORTED, TWEI_IAFIELDC_VALUE},
                {extendedimageinfo_25::IAFIELDD_SUPPORTED, TWEI_IAFIELDD_VALUE},
                {extendedimageinfo_25::IAFIELDE_SUPPORTED, TWEI_IAFIELDE_VALUE},
                {extendedimageinfo_25::IALEVEL_SUPPORTED, TWEI_IALEVEL},
                {extendedimageinfo_25::PRINTER_SUPPORTED, TWEI_PRINTER},
                {extendedimageinfo_25::BARCODETEXT2_SUPPORTED, TWEI_BARCODETEXT2},
                } };

            for (auto& pr : aAllData)
            {
                auto iter = std::find(m_vSupportedExtInfo.begin(), m_vSupportedExtInfo.end(), pr.second);
                m_extendedimageinfo25.m_supported[pr.first] = (iter != m_vSupportedExtInfo.end());
            }

            // Get the string infos
            using memPtr1 = decltype(&extendedimageinfo_25::m_IAFIELDA);
            std::vector<memPtr1> individual_items1 = {&extendedimageinfo_25::m_IAFIELDA,
                                                      &extendedimageinfo_25::m_IAFIELDB,
                                                      &extendedimageinfo_25::m_IAFIELDC,
                                                      &extendedimageinfo_25::m_IAFIELDD,
                                                      &extendedimageinfo_25::m_IAFIELDE };

            DTWAIN_ARRAY aText = {};
            auto* curObject = &m_extendedimageinfo25;
            for (int i = extendedimageinfo_25::IAFIELDA_SUPPORTED; i < extendedimageinfo_25::IAFIELDE_SUPPORTED + 1; ++i)
            {
                if (m_extendedimageinfo25.m_supported[i])
                {
                    API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), aAllData[i].second, &aText);
                    if (aText && API_INSTANCE DTWAIN_ArrayGetCount(aText) > 0)
                    {
                        twain_array ta;
                        ta.set_array(aText);
                        auto pItem = individual_items1[i];
                        curObject->*pItem = get_text_from_info(aText, 0);
                    }
                }
            }

            using memPtr2 = decltype(&extendedimageinfo_25::m_IALEVEL);
            std::vector<memPtr2> individual_items2 = { &extendedimageinfo_25::m_IALEVEL,
                                                       &extendedimageinfo_25::m_printer
                                                     };

            DTWAIN_ARRAY aValues = {};
            for (size_t i = extendedimageinfo_25::IALEVEL_SUPPORTED, curItem = 0; i < extendedimageinfo_25::PRINTER_SUPPORTED + 1; ++i, ++curItem)
            {
                if (m_extendedimageinfo25.m_supported[i])
                {
                    bool bRet = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), aAllData[i].second, &aValues);
                    if (bRet && aValues && API_INSTANCE DTWAIN_ArrayGetCount(aValues) > 0)
                    {
                        twain_array ta;
                        ta.set_array(aValues);
                        auto pItem = individual_items2[curItem];
                        curObject->*pItem = ta.at<TW_UINT16>(0);
                    }
                }
            }

            if (m_extendedimageinfo25.m_supported[extendedimageinfo_25::BARCODETEXT2_SUPPORTED])
            {
                DTWAIN_ARRAY aText;
                bool bRet = API_INSTANCE DTWAIN_GetExtImageInfoData(m_pSource->get_source(), TWEI_BARCODETEXT2, &aText);
                if (bRet && aText)
                {
                    twain_array ta;
                    ta.set_array(aText);
                    LONG nCount = API_INSTANCE DTWAIN_ArrayGetCount(aText);
                    for (int i = 0; i < nCount; ++i)
                        m_extendedimageinfo25.m_barcodeText.push_back(get_text_from_info(aText, i));
                }
            }
        }
    }
}

