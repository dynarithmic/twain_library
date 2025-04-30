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
#ifndef DTWAIN_EXTENDEDIMAGE_INFO_HPP
#define DTWAIN_EXTENDEDIMAGE_INFO_HPP

#include <vector>
#include <string>
#include <cstdint>
#include <bitset>
#ifdef DTWAIN_CPP_NOIMPORTLIB
#include <dtwainx2.h>
#else
#include <dtwain.h>
#endif
namespace dynarithmic
{
    namespace twain
    {
        class twain_source;
        class twain_session;

        class extendedimage_info
        {
            twain_source* m_pSource = nullptr;
            friend twain_source;
        public:
            class barcode_info
            {
                friend extendedimage_info;
                uint32_t count = 0;
                enum { CONFIDENCE_SUPPORTED, ROTATION_SUPPORTED, XCOORDINATE_SUPPORTED,
                       YCOORDINATE_SUPPORTED, TYPE_SUPPORTED, TEXTLENGTH_SUPPORTED, TEXT_SUPPORTED};
                std::bitset<TEXT_SUPPORTED + 1> m_supported = {};
                class barcode_single_info
                {
                    friend extendedimage_info;
                    uint32_t m_confidence = 0;
                    uint32_t m_rotation = 0;
                    uint32_t m_xCoordinate = 0;
                    uint32_t m_yCoordinate = 0;
                    uint32_t m_type = 0;
                    uint32_t m_length = 0;
                    std::string m_text;

                public:
                    uint32_t get_confidence() const
                    {
                        return m_confidence;
                    }

                    uint32_t get_rotation() const
                    {
                        return m_rotation;
                    }

                    uint32_t get_xCoordinate() const
                    {
                        return m_xCoordinate;
                    }

                    uint32_t get_yCoordinate() const
                    {
                        return m_yCoordinate;
                    }

                    uint32_t get_type() const
                    {
                        return m_type;
                    }

                    uint32_t get_length() const
                    {
                        return m_length;
                    }

                    std::string get_typename() const
                    {
                        char sz[100] = {};
                        API_INSTANCE DTWAIN_GetTwainNameFromConstantA(DTWAIN_CONSTANT_TWBT, get_type(), sz, 100);
                        return sz;
                    }

                    std::string get_text() const
                    {
                        return m_text;
                    }
                };

                std::vector<barcode_single_info> m_vBarcodeInfos;

            public:
                barcode_info() {}

                bool is_confidence_supported() const
                {
                    return m_supported[CONFIDENCE_SUPPORTED];
                }

                bool is_rotation_supported() const
                {
                    return m_supported[ROTATION_SUPPORTED];
                }

                bool is_xCoordinate_supported() const
                {
                    return m_supported[XCOORDINATE_SUPPORTED];
                }

                bool is_yCoordinate_supported() const
                {
                    return m_supported[YCOORDINATE_SUPPORTED];
                }

                bool is_type_supported() const
                {
                    return m_supported[TYPE_SUPPORTED];
                }

                bool is_text_supported() const
                {
                    return m_supported[TEXT_SUPPORTED];
                }

                bool is_text_length_supported() const
                {
                    return m_supported[TEXTLENGTH_SUPPORTED];
                }

            private:
                barcode_info& set_count(uint32_t nCount)
                {
                    m_vBarcodeInfos.resize(nCount);
                    return *this;
                }

                barcode_info& set_single_info(barcode_single_info& info, size_t nWhich)
                {
                    m_vBarcodeInfos[nWhich] = info;
                    return *this;
                }

                auto& get_barcode_infos()
                {
                    return m_vBarcodeInfos;
                }

                const barcode_single_info& get_single_info(size_t nWhich) const
                {
                    return m_vBarcodeInfos[nWhich];
                }

                public:
                    size_t get_count() const
                    {
                        return m_vBarcodeInfos.size();
                    }
            };

            class shadedarea_detection_info
            {
                uint32_t count = 0;
                friend extendedimage_info;
                enum {
                    TOP_SUPPORTED, 
                    LEFT_SUPPORTED, 
                    HEIGHT_SUPPORTED,
                    WIDTH_SUPPORTED, 
                    SIZE_SUPPORTED, 
                    BLACKCOUNTOLD_SUPPORTED,
                    BLACKCOUNTNEW_SUPPORTED,
                    BLACKCOUNTRLMIN_SUPPORTED,
                    BLACKCOUNTRLMAX_SUPPORTED,
                    WHITECOUNTOLD_SUPPORTED,
                    WHITECOUNTNEW_SUPPORTED,
                    WHITECOUNTRLMIN_SUPPORTED,
                    WHITECOUNTRLMAX_SUPPORTED,
                    WHITECOUNTRLAVG_SUPPORTED,
                };
                std::bitset<WHITECOUNTRLAVG_SUPPORTED + 1> m_supported = {};
                class shadedarea_single_info
                {
                    friend extendedimage_info;
                    uint32_t top = 0;
                    uint32_t left = 0;
                    uint32_t height = 0;
                    uint32_t width = 0;
                    uint32_t size = 0;
                    uint32_t blackCountOld = 0;
                    uint32_t blackCountNew = 0;
                    uint32_t blackRLMin = 0;
                    uint32_t blackRLMax = 0;
                    uint32_t whiteCountOld = 0;
                    uint32_t whiteCountNew = 0;
                    uint32_t whiteRLMin = 0;
                    uint32_t whiteRLMax = 0;
                    uint32_t whiteRLAverage = 0;
                public:
                    uint32_t get_top() const
                    {
                        return top;
                    }

                    uint32_t get_left() const
                    {
                        return left;
                    }

                    uint32_t get_height() const {
                        return height;
                    }

                    uint32_t get_width() const {
                        return width;
                    }

                    uint32_t get_size() const {
                        return size;
                    }

                    uint32_t get_black_count_old() const {
                        return blackCountOld;
                    }

                    uint32_t get_black_count_new() const {
                        return blackCountNew;
                    }

                    uint32_t get_black_RLMin() const {
                        return blackRLMin;
                    }

                    uint32_t get_black_RLMax() const {
                        return blackRLMax;
                    }

                    uint32_t get_white_count_old() const {
                        return whiteCountOld;
                    }

                    uint32_t get_white_count_new() const {
                        return whiteCountNew;
                    }

                    uint32_t get_white_RLMin() const {
                        return whiteRLMin;
                    }

                    uint32_t get_white_RLMax() const {
                        return whiteRLMax;
                    }

                    uint32_t get_white_RLAverage() const {
                        return whiteRLAverage;
                    }
                };

                std::vector<shadedarea_single_info> m_vShadedAreaInfos;

            private:
                shadedarea_detection_info& set_count(uint32_t nCount)
                {
                    m_vShadedAreaInfos.resize(nCount);
                    return *this;
                }

                shadedarea_detection_info& set_single_info(shadedarea_single_info& info, size_t nWhich)
                {
                    m_vShadedAreaInfos[nWhich] = info;
                    return *this;
                }

                auto& get_shaded_area_infos()
                {
                    return m_vShadedAreaInfos;
                }

                const shadedarea_single_info& get_single_info(size_t nWhich) const
                {
                    return m_vShadedAreaInfos[nWhich];
                }

            public:
                size_t get_count() const
                {
                    return m_vShadedAreaInfos.size();
                }
            };

            class speckle_removal_info
            {
                friend extendedimage_info;
                enum {
                    SPECKLESREMOVED_SUPPORTED,
                    BLACKSPECKLESREMOVED_SUPPORTED,
                    WHITESPECKLESREMOVED_SUPPORTED
                };

                uint32_t specklesRemoved = 0;
                uint32_t blackSpecklesRemoved = 0;
                uint32_t whiteSpecklesRemoved = 0;

                std::bitset<WHITESPECKLESREMOVED_SUPPORTED + 1> m_supported = {};

            public:
                uint32_t get_speckles_removed() const {
                    return specklesRemoved;
                }

                uint32_t getBlackSpecklesRemoved() const {
                    return blackSpecklesRemoved;
                }

                uint32_t getWhiteSpecklesRemoved() const {
                    return whiteSpecklesRemoved;
                }
            };

            class skew_detection_info
            {
                friend extendedimage_info;
                enum {
                    DESKEWSTATUS_SUPPORTED,
                    ORIGINALANGLE_SUPPORTED,
                    FINALANGLE_SUPPORTED,
                    CONFIDENCE_SUPPORTED,
                    WINDOWX1_SUPPORTED,
                    WINDOWX2_SUPPORTED,
                    WINDOWX3_SUPPORTED,
                    WINDOWX4_SUPPORTED,
                    WINDOWY1_SUPPORTED,
                    WINDOWY2_SUPPORTED,
                    WINDOWY3_SUPPORTED,
                    WINDOWY4_SUPPORTED
                };
                uint32_t deskewStatus = 0;
                uint32_t originalAngle = 0;
                uint32_t finalAngle = 0;
                uint32_t confidence = 0;
                uint32_t windowX1 = 0;
                uint32_t windowX2 = 0;
                uint32_t windowX3 = 0;
                uint32_t windowX4 = 0;
                uint32_t windowY1 = 0;
                uint32_t windowY2 = 0;
                uint32_t windowY3 = 0;
                uint32_t windowY4 = 0;

                std::bitset<WINDOWY4_SUPPORTED + 1> m_supported = {};

            public:
                uint32_t get_deskew_status() const {
                    return deskewStatus;
                }

                uint32_t get_original_angle() const {
                    return originalAngle;
                }

                uint32_t get_final_angle() const {
                    return finalAngle;
                }

                uint32_t get_confidence() const {
                    return confidence;
                }

                uint32_t get_windowX1() const {
                    return windowX1;
                }

                uint32_t get_windowX2() const {
                    return windowX2;
                }

                uint32_t get_windowX3() const {
                    return windowX3;
                }

                uint32_t get_windowX4() const {
                    return windowX4;
                }

                uint32_t get_windowY1() const {
                    return windowY1;
                }

                uint32_t get_windowY2() const {
                    return windowY2;
                }

                uint32_t get_windowY3() const {
                    return windowY3;
                }

                uint32_t get_windowY4() const {
                    return windowY4;
                }
            };

            class endorsed_text_info
            {
                friend extendedimage_info;
                enum {
                    ENDORSEDTEXT_SUPPORTED
                };
                std::bitset<ENDORSEDTEXT_SUPPORTED + 1> m_supported = {};
                std::string endorsed_text;
                public:
                    std::string get_text() const
                    {
                        return endorsed_text;
                    }
            };

            class formsrecognition_info
            {
                friend extendedimage_info;
                enum {
                    FORMTEMPLATEMATCH_SUPPORTED,
                    FORMCONFIDENCE_SUPPORTED, 
                    FORMTEMPLATEPAGEMATCH_SUPPORTED, 
                    FORMHORZDOCOFFSET_SUPPORTED, 
                    FORMVERTDOCOFFSET_SUPPORTED };
                std::bitset<FORMVERTDOCOFFSET_SUPPORTED + 1> m_supported = {};
                std::vector<std::string> vTemplateMatch;
                std::vector<TW_UINT32> vFormConfidence;
                std::vector<TW_UINT32> vFormTemplatePageMatch;
                std::vector<TW_UINT32> vFormHorizDocOffset;
                std::vector<TW_UINT32> vFormVertDocOffset;

            public:
                auto& get_template_match() const
                {
                    return vTemplateMatch;
                }

                auto& get_confidence() const
                {
                    return vFormConfidence;
                }

                auto& get_templatepage_match() const
                {
                    return vFormTemplatePageMatch;
                }

                auto& get_horizontal_docoffset() const
                {
                    return vFormHorizDocOffset;
                }

                auto& get_vertical_docoffset() const
                {
                    return vFormVertDocOffset;
                }
            };


            class linedetection_info
            {
                friend extendedimage_info;
                uint32_t horizCount = 0;
                uint32_t vertCount = 0;

                class linedetection_single_info
                {
                    friend extendedimage_info;
                    uint32_t m_XCoord = 0;
                    uint32_t m_YCoord = 0;
                    uint32_t m_Length = 0;
                    uint32_t m_Thickness = 0;

                    enum {
                        XCOORD_SUPPORTED,
                        YCOORD_SUPPORTED,
                        LENGTH_SUPPORTED,
                        THICKNESS_SUPPORTED,
                    };
                    std::bitset<THICKNESS_SUPPORTED + 1> m_supported = {};
                public:
                    uint32_t get_X() const
                    {
                        return m_XCoord;
                    }

                    uint32_t get_Y() const
                    {
                        return m_YCoord;
                    }

                    uint32_t get_length() const
                    {
                        return m_Length;
                    }

                    uint32_t get_thickness() const
                    {
                        return m_Thickness;
                    }

                    bool is_xcoord_supported() const
                    {
                        return m_supported[XCOORD_SUPPORTED];
                    }

                    bool is_ycoord_supported() const
                    {
                        return m_supported[YCOORD_SUPPORTED];
                    }

                    bool is_length_supported() const
                    {
                        return m_supported[LENGTH_SUPPORTED];
                    }

                    bool is_thickness_supported() const
                    {
                        return m_supported[THICKNESS_SUPPORTED];
                    }
                };

                std::vector<linedetection_single_info> m_vHorizontalLineInfos;
                std::vector<linedetection_single_info> m_vVerticalLineInfos;

            public:
                linedetection_info() {}

                size_t get_horizontal_count() const { return horizCount; }
                size_t get_vertical_count() const { return vertCount; }

            private:
                linedetection_info& set_horizontal_count(uint32_t nCount)
                {
                    m_vHorizontalLineInfos.resize(nCount);
                    return *this;
                }

                linedetection_info& set_vertical_count(uint32_t nCount)
                {
                    m_vVerticalLineInfos.resize(nCount);
                    return *this;
                }

                linedetection_info& set_horizontal_single_info(linedetection_single_info& info, size_t nWhich)
                {
                    m_vHorizontalLineInfos[nWhich] = info;
                    return *this;
                }

                linedetection_info& set_vertical_single_info(linedetection_single_info& info, size_t nWhich)
                {
                    m_vVerticalLineInfos[nWhich] = info;
                    return *this;
                }

                auto& get_horizontal_linedetection_infos()
                {
                    return m_vHorizontalLineInfos;
                }

                auto& get_vertical_linedetection_infos()
                {
                    return m_vVerticalLineInfos;
                }

                const linedetection_single_info& get_single_horizontal_info(size_t nWhich) const
                {
                    return m_vHorizontalLineInfos[nWhich];
                }

                const linedetection_single_info& get_single_vertical_info(size_t nWhich) const
                {
                    return m_vVerticalLineInfos[nWhich];
                }
            };

            class pagesource_info
            {
                friend extendedimage_info;
                enum {
                    BOOKNAME_SUPPORTED,
                    CAMERA_SUPPORTED,
                    CHAPTERNUMBER_SUPPORTED,
                    DOCUMENTNUMBER_SUPPORTED,
                    PAGENUMBER_SUPPORTED,
                    FRAMENUMBER_SUPPORTED,
                    PIXELFLAVOR_SUPPORTED,
                    FRAME_SUPPORTED
                };
                std::bitset<FRAME_SUPPORTED + 1> m_supported = {};
                std::string m_bookname;
                std::string m_camera;
                uint32_t m_chapterNumber;
                uint32_t m_documentNumber;
                uint32_t m_pageNumber;
                uint32_t m_frameNumber;
                uint32_t m_pixelFlavor;
                twain_frame<double> m_frame;

            public:
                std::string get_bookname() const
                {
                    return m_bookname;
                }

                std::string get_camera() const
                {
                    return m_camera;
                }

                uint32_t get_chapter_number() const
                {
                    return m_chapterNumber;
                }

                uint32_t get_document_number() const
                {
                    return m_documentNumber;
                }
                uint32_t get_page_number() const
                {
                    return m_pageNumber;
                }

                uint32_t get_frame_number() const
                {
                    return m_frameNumber;
                }

                uint32_t get_pixel_flavor() const
                {
                    return m_pixelFlavor;
                }

                twain_frame<double> get_frame() const
                {
                    return m_frame;
                }
            };

            class imagesegmentation_info
            {
                friend extendedimage_info;
                enum {
                    ICCPROFILE_SUPPORTED,
                    LASTSEGMENT_SUPPORTED,
                    SEGMENTNUMBER_SUPPORTED
                };
                std::bitset<SEGMENTNUMBER_SUPPORTED + 1> m_supported = {};
                std::string m_ICCProfile;
                bool m_bLastSegment = false;
                uint32_t m_SegmentNumber = 0;

                public:
                    std::string get_ICCProfile() const 
                    {
                        return m_ICCProfile;
                    }

                    bool is_last_segment() const 
                    {
                        return m_bLastSegment;
                    }

                    uint32_t get_segment_number() const 
                    {
                        return m_SegmentNumber;
                    }
            };

            class extendedimageinfo_20
            {
                friend extendedimage_info;
                enum {
                    MAGTYPE_SUPPORTED,
                };
                std::bitset<MAGTYPE_SUPPORTED + 1> m_supported = {};
                uint16_t magtype = 0;

                public:
                    uint16_t get_magtype() const
                    {
                        return magtype;
                    }

                    bool is_magtype_supported() const
                    {
                        return m_supported[MAGTYPE_SUPPORTED];
                    }
            };

            class extendedimageinfo_21
            {
                friend extendedimage_info;
                enum {
                    MAGDATALENGTH_SUPPORTED,
                    MAGDATA_SUPPORTED,
                    FILESYSTEMSOURCE_SUPPORTED,
                    PAGESIDE_SUPPORTED,
                    IMAGEMERGED_SUPPORTED
                };
                std::bitset<IMAGEMERGED_SUPPORTED + 1> m_supported = {};
                uint32_t m_maglength = 0;
                std::vector<char> m_vMagData;
                std::string m_fileSystemSource;
                bool m_bImageMerged = false;
                uint16_t m_pageSide = 0;

            public:
                uint32_t get_mag_length() const
                {
                    return m_maglength;
                }

                std::vector<char> get_mag_data() const
                {
                    return m_vMagData;
                }

                std::string get_filesystem_source() const
                {
                    return m_fileSystemSource;
                }

                uint16_t get_page_side() const
                {
                    return m_pageSide;
                }

                bool is_image_merged() const
                {
                    return m_bImageMerged;
                }
            };

            class extendedimageinfo_22
            {
                friend extendedimage_info;
                enum {
                    PAPERCOUNT_SUPPORTED,
                };
                std::bitset<PAPERCOUNT_SUPPORTED + 1> m_supported = {};
                uint32_t paperCount = 0;
                public:
                    uint32_t get_paper_count() const
                    {
                        return paperCount;
                    }

                    bool is_papercount_supported() const
                    {
                        return m_supported[PAPERCOUNT_SUPPORTED];
                    }
            };


            class extendedimageinfo_23
            {
                friend extendedimage_info;
                enum {
                    PRINTERTEXT_SUPPORTED,
                };
                std::bitset<PRINTERTEXT_SUPPORTED + 1> m_supported = {};
                std::string m_printerText;

            public:
                std::string get_printer_text() const
                {
                    return m_printerText;
                }
            };

            class extendedimageinfo_24
            {
                friend extendedimage_info;
                enum {
                    TWAINDIRECTMETADATA_SUPPORTED,
                };
                std::bitset<TWAINDIRECTMETADATA_SUPPORTED + 1> m_supported = {};
                std::string m_twaindirectdata;

            public:
                std::string get_twaindirect_data() const
                {
                    return m_twaindirectdata;
                }
            };

            class extendedimageinfo_25
            {
                friend extendedimage_info;
                enum {
                    IAFIELDA_SUPPORTED,
                    IAFIELDB_SUPPORTED,
                    IAFIELDC_SUPPORTED,
                    IAFIELDD_SUPPORTED,
                    IAFIELDE_SUPPORTED,
                    IALEVEL_SUPPORTED,
                    PRINTER_SUPPORTED,
                    BARCODETEXT2_SUPPORTED,
                };
                std::bitset<BARCODETEXT2_SUPPORTED + 1> m_supported = {};

                std::string m_IAFIELDA;
                std::string m_IAFIELDB;
                std::string m_IAFIELDC;
                std::string m_IAFIELDD;
                std::string m_IAFIELDE;
                uint16_t m_IALEVEL = 0;
                uint16_t m_printer = 0;
                std::vector<std::string> m_barcodeText;

            public:
                std::string get_IAFieldA() const
                {
                    return m_IAFIELDA;
                }
                std::string get_IAFieldB() const
                {
                    return m_IAFIELDB;
                }
                std::string get_IAFieldC() const
                {
                    return m_IAFIELDC;
                }
                std::string get_IAFieldD() const
                {
                    return m_IAFIELDD;
                }
                std::string get_IAFieldE() const
                {
                    return m_IAFIELDE;
                }
                uint16_t get_IALevel() const
                {
                    return m_IALEVEL;
                }

                uint16_t get_printer() const
                {
                    return m_printer;
                }

                size_t get_num_barcodes() const
                {
                    return m_barcodeText.size();
                }

                std::vector<std::string> get_barcode_text() const
                {
                    return m_barcodeText;
                }
            };

            barcode_info m_barcodeInfo;
            shadedarea_detection_info m_shadedareaInfo;
            speckle_removal_info m_speckleremovalInfo;
            linedetection_info m_linedetectionInfo;
            skew_detection_info m_skewdetectionInfo;
            endorsed_text_info m_endorsedtextInfo;
            formsrecognition_info m_formsrecognitionInfo;
            pagesource_info m_pagesourceInfo;
            imagesegmentation_info m_imagesegmentationInfo;
            extendedimageinfo_20 m_extendedimageinfo20;
            extendedimageinfo_21 m_extendedimageinfo21;
            extendedimageinfo_22 m_extendedimageinfo22;
            extendedimageinfo_23 m_extendedimageinfo23;
            extendedimageinfo_24 m_extendedimageinfo24;
            extendedimageinfo_25 m_extendedimageinfo25;

            std::vector<TW_UINT16> m_vSupportedExtInfo;
            bool m_bDoDetach = true;
            void clear_infos() 
            { 
                m_barcodeInfo = {}; 
                m_shadedareaInfo = {}; 
                m_vSupportedExtInfo.clear(); 
                m_speckleremovalInfo = {}; 
                m_linedetectionInfo = {};
                m_skewdetectionInfo = {};
                m_formsrecognitionInfo = {};
                m_pagesourceInfo = {};
                m_imagesegmentationInfo = {};
                m_extendedimageinfo20 = {};
                m_extendedimageinfo21 = {};
                m_extendedimageinfo22 = {};
                m_extendedimageinfo23 = {};
                m_extendedimageinfo24 = {};
                m_extendedimageinfo25 = {};
            }
            void fillbarcode_info();
            void fillshadedarea_info();
            void fillspeckleremoval_info();
            void filllinedetection_info();
            void filllinedetection_info(int nWhichLineInfo, const std::vector<int>& lineInfos,
                                        std::vector<linedetection_info::linedetection_single_info>& vInfos);
            void fillskewdetection_info();
            void fillendorsedtext_info();
            void fillformsrecognition_info();
            void fillpagesource_info();
            void fillimagesegmentation_info();
            void fillextendedimageinfo20_info();
            void fillextendedimageinfo21_info();
            void fillextendedimageinfo22_info();
            void fillextendedimageinfo23_info();
            void fillextendedimageinfo24_info();
            void fillextendedimageinfo25_info();
            std::string get_text_from_info(DTWAIN_ARRAY aText, int nWhichText);
            bool attach(twain_source* theSource);
            bool detach();

        public:
            extendedimage_info() = default;
            ~extendedimage_info() { detach(); }
            static extendedimage_info clone(const extendedimage_info& thisOne) 
                { extendedimage_info info(thisOne); info.m_bDoDetach = false; return info; }
            const std::vector<barcode_info::barcode_single_info>& get_barcodes() const { return m_barcodeInfo.m_vBarcodeInfos; }
            const barcode_info& get_barcode_info() const { return m_barcodeInfo; }
            const std::vector<shadedarea_detection_info::shadedarea_single_info>& get_shadedareas() const { return m_shadedareaInfo.m_vShadedAreaInfos; }
            const shadedarea_detection_info& get_shadedarea_info() const { return m_shadedareaInfo; }
            const speckle_removal_info& get_speckleremoval_info() const { return m_speckleremovalInfo; }
            const skew_detection_info& get_skewdetection_info() const { return m_skewdetectionInfo; }
            const linedetection_info& get_linedetection_info() const { return m_linedetectionInfo; }
            const endorsed_text_info& get_endorsedtext_info() const { return m_endorsedtextInfo; }
            const pagesource_info& get_pagesource_info() const { return m_pagesourceInfo; }
            const imagesegmentation_info& get_imagesegmentation_info() const { return m_imagesegmentationInfo;  }
            const extendedimageinfo_20& get_extendedimageinfo20_info() const { return m_extendedimageinfo20; }
            const extendedimageinfo_21& get_extendedimageinfo21_info() const { return m_extendedimageinfo21; }
            const extendedimageinfo_22& get_extendedimageinfo22_info() const { return m_extendedimageinfo22; }
            const extendedimageinfo_23& get_extendedimageinfo23_info() const { return m_extendedimageinfo23; }
            const extendedimageinfo_24& get_extendedimageinfo24_info() const { return m_extendedimageinfo24; }
            const extendedimageinfo_25& get_extendedimageinfo25_info() const { return m_extendedimageinfo25; }
            const std::vector<linedetection_info::linedetection_single_info>& get_horizontal_linedetections() const { return m_linedetectionInfo.m_vHorizontalLineInfos; }
            const std::vector<linedetection_info::linedetection_single_info>& get_vertical_linedetections() const { return m_linedetectionInfo.m_vVerticalLineInfos; }
        };
    }
}
#endif
