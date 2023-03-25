#include <dynarithmic/twain/tostring/tojson.hpp>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>
#include <boost/algorithm/string/join.hpp>
#include <boost/range/adaptor/transformed.hpp>
#include <dynarithmic/twain/nlohmann/json.hpp>
#include <dynarithmic/twain/info/paperhandling_info.hpp>
#include <dynarithmic/twain/capability_interface/capability_interface.hpp>
#include <sstream>
#include <string>
#include <vector>

namespace dynarithmic
{
    namespace twain
    {
        template <typename Iter>
        static std::string join_string(Iter it1, Iter it2, char val = ',')
        {
            std::stringstream strm;
            int i = 0;
            while (it1 != it2)
            {
                if (i != 0)
                    strm << val;
                strm << *it1;
                ++it1;
                ++i;
            }
            return strm.str();
        }

        static std::string remove_quotes(std::string s)
        {
            s.erase(std::remove(s.begin(), s.end(), '\"'), s.end());
            return s;
        }

        template <typename T>
        static void create_stream(std::stringstream& strm, const capability_interface& capInfo, int capValue)
        {
            std::vector<T> imageVals;
            imageVals = capInfo.get_cap_values<std::vector<T>>(capValue);
            if (imageVals.empty())
            {
                strm << "\"<not available>\"";
            }
            else
            {
                strm << "{";
                // check if range
                if (is_valid_range(imageVals))
                    strm << "\"data-type\":\"range\",";
                else
                    strm << "\"data-type\":\"discrete\",";
                strm << "\"data-values\":[" << join_string(imageVals.begin(), imageVals.end()) << "]}";
            }
        }

        static void create_stream_from_strings(std::stringstream& strm, const capability_interface& capInfo, int capValue)
        {
            std::vector<std::string> imageVals;
            imageVals = capInfo.get_cap_values<std::vector<std::string>>(capValue);
            if (imageVals.empty())
            {
                strm << "\"<not available>\"";
            }
            else
            {
                for (auto& ivals : imageVals)
                {
                    ivals = "\"" + ivals;
                    ivals.push_back('\"');
                }
                strm << "{";
                strm << "\"data-values\":[" << join_string(imageVals.begin(), imageVals.end()) << "]}";
            }
        }

        template <typename T, typename S>
        static void create_stream(std::stringstream& strm, const capability_interface& capInfo, int capValue, bool createStringNames)
        {
            std::vector<T> imageVals;
            imageVals = capInfo.get_cap_values<std::vector<T>>(capValue);
            if (imageVals.empty())
            {
                strm << "\"<not available>\"";
            }
            else
            {
                strm << "{";
                // check if range
                if (is_valid_range(imageVals))
                    strm << "\"data-type\":\"range\",";
                else
                    strm << "\"data-type\":\"discrete\",";
                auto vInfo = S::to_twain_string(imageVals.begin(), imageVals.end());
                std::vector<std::string> vSizeNames;
                std::transform(vInfo.begin(), vInfo.end(), std::back_inserter(vSizeNames),
                    [](const std::pair<const char*, const char*>& p) { return "\"" + std::string(p.second) + "\""; });
                std::string paperSizesStr = join_string(vSizeNames.begin(), vSizeNames.end());
                strm << "\"data-values\":[" << join_string(vSizeNames.begin(), vSizeNames.end()) << "]}";
            }
        }

        static std::string get_source_file_types(const capability_interface& capInfo)
        {
            using sourceMapType = std::unordered_map<dynarithmic::twain::compression_value::value_type, std::string>;
            static sourceMapType source_map = {
                          {dynarithmic::twain::filetype_value::bmp_source_mode,"\"bmp1_mode2\""},
                          {dynarithmic::twain::filetype_value::bmp_source_mode,"\"bmp2_mode2\""},
                          {dynarithmic::twain::filetype_value::bmp_source_mode,"\"bmp3_mode2\""},
                          {dynarithmic::twain::filetype_value::bmp_source_mode,"\"bmp4_mode2\""},
                          {dynarithmic::twain::filetype_value::dejavu_source_mode,"\"dejavu_mode2\""},
                          {dynarithmic::twain::filetype_value::exif_source_mode,"\"exif_mode2\""},
                          {dynarithmic::twain::filetype_value::fpx_source_mode,"\"fpx_mode2\""},
                          {dynarithmic::twain::filetype_value::jfif_source_mode,"\"jfif_mode2\""},
                          {dynarithmic::twain::filetype_value::jpeg,"\"jpeg_mode2\""},
                          {dynarithmic::twain::filetype_value::jp2_source_mode,"\"jp2_mode2\""},
                          {dynarithmic::twain::filetype_value::jpx_source_mode,"\"jpx_mode2\""},
                          {dynarithmic::twain::filetype_value::pdf_source_mode,"\"pdf_mode2\""},
                          {dynarithmic::twain::filetype_value::pdfa_source_mode,"\"pdfa1_mode2\""},
                          {dynarithmic::twain::filetype_value::pdfa2_source_mode,"\"pdfa2_mode2\""},
                          {dynarithmic::twain::filetype_value::pict_source_mode,"\"pict_mode2\""},
                          {dynarithmic::twain::filetype_value::png_source_mode,"\"png_mode2\""},
                          {dynarithmic::twain::filetype_value::spiff_source_mode,"\"spiff1_mode2\""},
                          {dynarithmic::twain::filetype_value::spiff_source_mode,"\"spiff2_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff1_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff2_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff3_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff4_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff5_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff6_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff7_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff8_mode2\""},
                          {dynarithmic::twain::filetype_value::tiff_source_mode,"\"tiff9_mode2\""},
                          {dynarithmic::twain::filetype_value::xbm_source_mode,"\"xbm_mode2\""} };

            static sourceMapType tiffMap = {
                          {dynarithmic::twain::compression_value::none,"\"tiff1_mode2\""},
                          {dynarithmic::twain::compression_value::group31D,"\"tiff2_mode2\""},
                          {dynarithmic::twain::compression_value::group31DEOL,"\"tiff3_mode2\""},
                          {dynarithmic::twain::compression_value::group32D,"\"tiff4_mode2\""},
                          {dynarithmic::twain::compression_value::group4,"\"tiff5_mode2\""},
                          {dynarithmic::twain::compression_value::jpeg,"\"tiff6_mode2\""},
                          {dynarithmic::twain::compression_value::lzw,"\"tiff7_mode2\""},
                          {dynarithmic::twain::compression_value::jbig,"\"tiff8_mode2\""},
                          {dynarithmic::twain::compression_value::zip,"\"tiff9_mode2\""} };

            static sourceMapType bmpMap = {
                          {dynarithmic::twain::compression_value::none,"\"bmp1_mode2\""},
                          {dynarithmic::twain::compression_value::rle4,"\"bmp2_mode2\""},
                          {dynarithmic::twain::compression_value::rle8,"\"bmp3_mode2\""},
                          {dynarithmic::twain::compression_value::bitfields ,"\"bmp4_mode2\""} };

            static sourceMapType spiffMap = {
                          {dynarithmic::twain::compression_value::jpeg, "\"spiff1_mode2\""},
                          {dynarithmic::twain::compression_value::jbig, "\"spiff2_mode2\""} };

            std::map<dynarithmic::twain::compression_value::value_type, const sourceMapType*> compToMap =
            { {TWFF_TIFF, &tiffMap}, {TWFF_BMP, &bmpMap}, {TWFF_SPIFF, &spiffMap} };

            struct resetAll
            {
                dynarithmic::twain::filetype_value::value_type curFormat;
                dynarithmic::twain::compression_value::value_type curCompression;
                const capability_interface& capInfo;
                resetAll(const capability_interface& cInfo,
                    dynarithmic::twain::filetype_value::value_type cF,
                    dynarithmic::twain::compression_value::value_type cC) : curFormat(cF), curCompression(cC), capInfo(cInfo) {}
                ~resetAll()
                {
                    capInfo.set_imagefileformat({ curFormat });
                    if (curCompression != -1)
                        capInfo.set_compression({ curCompression });
                }
            };

            // get all the image file formats
            auto vFileFormats = capInfo.get_imagefileformat();

            // get the current image file format
            auto vCurrentFormat = capInfo.get_imagefileformat(capability_interface::get_current());
            auto vCurrentCompress = capInfo.get_compression(capability_interface::get_current());
            if (vCurrentFormat.empty())
                return "";

            resetAll ra(capInfo, vCurrentFormat.front(), vCurrentCompress.empty() ? -1 : vCurrentCompress.front());

            std::vector<std::string> returnFileTypes;
            for (auto fformat : vFileFormats)
            {
                auto compIter = compToMap.find(fformat);
                if (compIter != compToMap.end())
                {
                    const std::unordered_map<dynarithmic::twain::filetype_value::value_type, std::string>* ptr = compIter->second;
                    capInfo.set_imagefileformat({ fformat });
                    auto vCompressions = capInfo.get_compression();
                    for (auto compression : vCompressions)
                    {
                        auto iter = ptr->find(compression);
                        if (iter != ptr->end())
                            returnFileTypes.push_back(iter->second);
                    }
                }
                else
                {
                    auto sourceIter = source_map.find(fformat);
                    if (sourceIter != source_map.end())
                        returnFileTypes.push_back(sourceIter->second);
                }
            }
            return join_string(returnFileTypes.begin(), returnFileTypes.end());
        }

        std::string json_generator::generate_details(twain_session& ts, const std::vector<std::string>& allSources, bool bWeOpenSource/*=false*/)
        {
            using boost::algorithm::join;
            using boost::adaptors::transformed;
            using json = nlohmann::ordered_json;

            struct capabilityInfo
            {
                std::string name;
                int value;
                capabilityInfo(std::string n = "", int val = 0) : name(n), value(val) {}
            };

            struct sUniquePtrRAII
            {
                std::unique_ptr<twain_source>* m_ptr;
                bool m_bReleaseOnDestruction;
                sUniquePtrRAII(std::unique_ptr<twain_source>* p, bool isReleaseOnDestruction)
                    : m_ptr(p), m_bReleaseOnDestruction(isReleaseOnDestruction) {}
                ~sUniquePtrRAII()
                {
                    if (m_bReleaseOnDestruction)
                        (*m_ptr).release();
                }
            };
            std::vector<capabilityInfo> vCapabilityInfo;

            json glob_json;
            glob_json["device-count"] = allSources.size();
            json array_twain_identity;
            json array_source_names;
            std::vector<std::string> sNames = allSources;
            glob_json["device-names"] = sNames;
            std::string jsonString;
            std::string imageInfoString[12];
            std::string deviceInfoString[9];

            for (auto& curSource : allSources)
            {
                std::string jColorInfo;
                std::string resUnitInfo;
                std::string capabilityString;
                deviceInfoString[0] = "\"feeder-supported\":false";
                deviceInfoString[1] = "\"feeder-sensitive\":false";
                deviceInfoString[2] = "\"ui-controllable\":false";
                deviceInfoString[3] = "\"autobright-supported\":false";
                deviceInfoString[4] = "\"autodeskew-supported\":false";
                deviceInfoString[5] = "\"imprinter-supported\":false";
                deviceInfoString[6] = "\"duplex-supported\":false";
                deviceInfoString[7] = "\"jobcontrol-supported\":false";
                deviceInfoString[8] = "\"transparencyunit-supported\":false";
                bool devOpen[] = { false, false };

                // Check if we need to select and open the source to see
                // the details
                std::unique_ptr<twain_source> pCurrentSourcePtr;

                auto sourceStatus = ts.get_source_status(curSource);
                if (sourceStatus == twain_session::source_status::closed ||
                    sourceStatus == twain_session::source_status::unknown)
                {
                    auto select_info = ts.select_source(select_byname(curSource), bWeOpenSource);
                    if (select_info.source_handle)
                    {
                        pCurrentSourcePtr = std::make_unique<twain_source>(select_info);
                    }
                }
                else
                {
                    // Source already opened
                    DTWAIN_SOURCE openedSource = ts.get_source_handle_from_name(curSource);
                    pCurrentSourcePtr = std::make_unique<twain_source>();
                    pCurrentSourcePtr->attach(ts, openedSource);
                    pCurrentSourcePtr->make_weak();
                }

                if (!pCurrentSourcePtr && bWeOpenSource)
                {
                    // try to select the source only
                    auto select_info = ts.select_source(select_byname(curSource), false);
                    if (select_info.source_handle)
                    {
                        pCurrentSourcePtr = std::make_unique<twain_source>(select_info);
                    }
                }
                if (pCurrentSourcePtr && pCurrentSourcePtr->is_selected())
                {
                    devOpen[0] = true;
                    if (pCurrentSourcePtr->is_open())
                    {
                        devOpen[1] = true;
                        jsonString = pCurrentSourcePtr->get_source_info().to_json();
                        jColorInfo = "\"color-info\":{";
                        std::stringstream strm;

                        // Get the pixel information
                        auto& capInfo = pCurrentSourcePtr->get_capability_interface();
                        auto pixInfo = capInfo.get_pixeltype();
                        strm << "\"num-colors\":" << pixInfo.size() << ",";
                        jColorInfo += strm.str();

                        strm.str("");
                        std::string joinStr = join_string(pixInfo.begin(), pixInfo.end()); 
                        strm << "\"color-types\":[" << joinStr << "],";

                        std::stringstream strm2;
                        strm2 << "\"bitdepthinfo\":{";

                        for (auto p : pixInfo)
                        {
                            strm2 << "\"depth_" << p << "\":[";
                            capInfo.set_pixeltype({ p });
                            auto bdepth = capInfo.get_bitdepth();
                            std::string bdepthStr = join_string(bdepth.begin(), bdepth.end());
                            strm2 << bdepthStr << "],";
                        }
                        std::string allbdepths = strm2.str();
                        allbdepths.pop_back();
                        allbdepths += "}";

                        strm2.str("");

                        // get the paper sizes
                        auto paperSizes = capInfo.get_supportedsizes();
                        auto vSizeInfo = supportedsizes_value::to_twain_string(paperSizes.begin(), paperSizes.end());
                        std::vector<std::string> vSizeNames;
                        std::transform(vSizeInfo.begin(), vSizeInfo.end(), std::back_inserter(vSizeNames),
                            [](const std::pair<const char*, const char*>& p) { return "\"" + std::string(p.second) + "\""; });

                        std::string paperSizesStr = join_string(vSizeNames.begin(), vSizeNames.end());
                        strm2 << "\"paper-sizes\":[" << paperSizesStr << "],";
                        std::string allSizes = strm2.str();
                        jColorInfo += strm.str() + allbdepths + "}," + allSizes;

                        // get the resolution info
                        strm.str("");
                        strm << "\"resolution-info\": {";

                        auto allUnits = capInfo.get_units();
                        strm << "\"resolution-count\":" << allUnits.size() << ",";
                        strm << "\"resolution-units\":";
                        auto unitNames = units_value::to_twain_string(allUnits.begin(), allUnits.end());
                        std::vector<std::string> unitNameV;
                        std::transform(unitNames.begin(), unitNames.end(), std::back_inserter(unitNameV),
                            [](const std::pair<const char*, const char*>& p) { return "\"" + std::string(p.second) + "\""; });
                        std::string unitNameStr = "[" + join_string(unitNameV.begin(), unitNameV.end(), ',') + "]";
                        strm << unitNameStr << ",";
                        int i = 0;
                        std::string resolutionTotalStr;
                        for (auto u : allUnits)
                        {
                            strm2.str("");
                            if (i > 0)
                                strm2 << ",";
                            strm2 << "\"resolution-" << remove_quotes(unitNameV[i]) << "\": {";

                            // set the unit of measure
                            capInfo.set_units({ u });

                            // get all the values
                            auto allUnitValues = capInfo.get_xresolution();

                            // check if range
                            if (is_valid_range(allUnitValues))
                                strm2 << "\"data-type\":\"range\",";
                            else
                                strm2 << "\"data-type\":\"discrete\",";
                            strm2 << "\"data-values\":[" << join_string(allUnitValues.begin(), allUnitValues.end()) << "]}";
                            resolutionTotalStr += strm2.str();
                            ++i;
                        }

                        resUnitInfo = strm.str() + resolutionTotalStr + "},";

                        int imageInfoCaps[] = { ICAP_BRIGHTNESS, ICAP_CONTRAST, ICAP_GAMMA, ICAP_HIGHLIGHT, ICAP_SHADOW,
                            ICAP_THRESHOLD, ICAP_ROTATION, ICAP_ORIENTATION, ICAP_OVERSCAN, ICAP_HALFTONES };
                        std::string imageInfoCapsStr[] = { "\"brightness-values\":", "\"contrast-values\":", "\"gamma-values\":",
                            "\"highlight-values\":", "\"shadow-values\":", "\"threshold-values\":",
                            "\"rotation-values\":", "\"orientation-values\":", "\"overscan-values\":", "\"halftone-values\":" };
                        for (int i = 0; i < sizeof(imageInfoCaps) / sizeof(imageInfoCaps[0]); ++i)
                        {
                            strm.str("");
                            strm << imageInfoCapsStr[i];
                            if (imageInfoCaps[i] == ICAP_ORIENTATION)
                                create_stream<ICAP_ORIENTATION_::value_type>(strm, capInfo, ICAP_ORIENTATION);
                            else
                                if (imageInfoCaps[i] == ICAP_OVERSCAN)
                                    create_stream<ICAP_OVERSCAN_::value_type, overscan_value>(strm, capInfo, ICAP_OVERSCAN, true);
                                else
                                    if (imageInfoCaps[i] == ICAP_HALFTONES)
                                        create_stream_from_strings(strm, capInfo, ICAP_HALFTONES);
                                    else
                                        create_stream<double>(strm, capInfo, imageInfoCaps[i]);
                            imageInfoString[i] = strm.str();
                        }

                        // get the capability string
                        auto allCaps = capInfo.get_caps();
                        for (auto& cap : allCaps)
                        {
                            vCapabilityInfo.push_back({ capability_interface::get_cap_name_s(cap), cap });
                        }

                        if (!vCapabilityInfo.empty())
                        {
                            strm2.str("");
                            for (auto& v : vCapabilityInfo)
                            {
                                std::string capType = "\"standard\"";
                                if (capInfo.is_custom_cap(v.value))
                                    capType = "\"custom\"";
                                else
                                    if (capInfo.is_extended_cap(v.value))
                                        capType = "\"standard, extended\"";
                                strm2 << "{ \"name\":\"" << v.name << "\",\"value\":" << v.value << ",\"type\":" << capType << "},";
                            }
                            capabilityString = strm2.str();
                            capabilityString.pop_back();
                            capabilityString = "[" + capabilityString + "]";
                        }
                        std::ostringstream tempStrm;
                        tempStrm << "\"capability-count\":[{\"all\":" << vCapabilityInfo.size() << ","
                            "\"custom\":" << capInfo.get_custom_caps().size() << ","
                            "\"extended\":" << capInfo.get_extended_caps().size() << "}],\"capability-values\":" << capabilityString;
                        imageInfoString[10] = tempStrm.str();

                        // Get the filetype info
                        tempStrm.str("");
                        std::vector<std::string> fileTypes = {
                                                                "\"bmp\",",
                                                                "\"gif\",",
                                                                "\"pcx\",",
                                                                "\"dcx\",",
                                                                "\"pdf\",",
                                                                "\"ico\",",
                                                                "\"png\",",
                                                                "\"tga\",",
                                                                "\"psd\",",
                                                                "\"emf\",",
                                                                "\"wbmp\",",
                                                                "\"wmf\",",
                                                                "\"jpeg\",",
                                                                "\"jp2\",",
                                                                "\"tif1\",",
                                                                "\"tif2\",",
                                                                "\"tif3\",",
                                                                "\"tif4\",",
                                                                "\"tif5\",",
                                                                "\"tif6\",",
                                                                "\"tif7\",",
                                                                "\"ps1\",",
                                                                "\"ps2\",",
                                                                "\"webp\"" };

                        std::string allFileTypes = std::accumulate(fileTypes.begin(), fileTypes.end(), std::string());
                        std::string customTypes = get_source_file_types(capInfo);
                        if (!customTypes.empty())
                            allFileTypes += "," + customTypes;
                        tempStrm << "\"filetype-info\":[" << allFileTypes << "]";
                        imageInfoString[11] = tempStrm.str();

                        strm.str("");
                        int deviceInfoCaps[] = { CAP_FEEDERENABLED, CAP_FEEDERLOADED, CAP_UICONTROLLABLE,
                            ICAP_AUTOBRIGHT, ICAP_AUTOMATICDESKEW,
                            CAP_PRINTER, CAP_DUPLEX, CAP_JOBCONTROL, ICAP_LIGHTPATH
                        };

                        std::string deviceInfoCapsStr[sizeof(deviceInfoCaps) / sizeof(deviceInfoCaps[0])];
                        std::copy(deviceInfoString, deviceInfoString + sizeof(deviceInfoString) / sizeof(deviceInfoString[0]), deviceInfoCapsStr);
                        for (auto& s : deviceInfoCapsStr)
                            s.resize(s.size() - 5);
                        paperhandling_info pinfo(*pCurrentSourcePtr);
                        for (int i = 0; i < sizeof(deviceInfoCaps) / sizeof(deviceInfoCaps[0]); ++i)
                        {
                            if (i > 0)
                                strm << ",";
                            bool value = false;
                            if (deviceInfoCaps[i] == CAP_FEEDERENABLED)
                                value = pinfo.is_feedersupported();
                            else
                            if (deviceInfoCaps[i] == CAP_UICONTROLLABLE)
                            {
                                auto vValue = capInfo.get_uicontrollable();
                                if (!vValue.empty())
                                    value = vValue.front();
                            }
                            else
                            if (deviceInfoCaps[i] == CAP_PRINTER)
                            {
                                auto vValue = capInfo.get_printer();
                                value = (!vValue.empty() && vValue.front() != TWDX_NONE);
                            }
                            else
                            if (deviceInfoCaps[i] == CAP_JOBCONTROL)
                            {
                                auto vValue = capInfo.get_jobcontrol();
                                value = (!vValue.empty() && vValue.front() != TWJC_NONE);
                            }
                            else
                                value = capInfo.is_cap_supported(deviceInfoCaps[i]);
                            strm << deviceInfoCapsStr[i] << (value ? "true" : "false");
                            deviceInfoString[i] = strm.str();
                        }
                    }
                    else
                    {
                        jsonString = pCurrentSourcePtr->get_source_info().to_json();
                        jColorInfo = "\"color-info\":\"<not available>\",";
                        resUnitInfo = "\"resolution-info\":\"<not available>\",";
                        imageInfoString[0] = "\"brightness-values\":\"<not available>\"";
                        imageInfoString[1] = "\"contrast-values\":\"<not available>\"";
                        imageInfoString[2] = "\"gamma-values\":\"<not available>\"";
                        imageInfoString[3] = "\"highlight-values\":\"<not available>\"";
                        imageInfoString[4] = "\"shadow-values\":\"<not available>\"";
                        imageInfoString[5] = "\"threshold-values\":\"<not available>\"";
                        imageInfoString[6] = "\"rotation-values\":\"<not available>\"";
                        imageInfoString[7] = "\"orientation-values\":\"<not available>\"";
                        imageInfoString[8] = "\"overscan-values\":\"<not available>\"";
                        imageInfoString[9] = "\"halftone-values\":\"<not available>\"";
                        imageInfoString[10] = "\"capability-info\":\"<not available>\"";
                        imageInfoString[11] = "\"filetype-info\":\"<not available>\"";

                        deviceInfoString[0] = "\"feeder-supported\":false";
                        deviceInfoString[1] = "\"feeder-sensitive\":false";
                        deviceInfoString[2] = "\"ui-controllable\":false";
                        deviceInfoString[3] = "\"autobright-supported\":false";
                        deviceInfoString[4] = "\"autodeskew-supported\":false";
                        deviceInfoString[5] = "\"imprinter-supported\":false";
                        deviceInfoString[6] = "\"duplex-supported\":false";
                        deviceInfoString[7] = "\"jobcontrol-supported\":false";
                        deviceInfoString[8] = "\"transparencyunit-supported\":false";
                    }
                }
                std::string partString = "\"device-name\":\"" + curSource + "\",";
                std::string strStatus;
                if (devOpen[0] && devOpen[1])
                    strStatus = "\"<selected, opened>\"";
                else
                if (!devOpen[0] && !devOpen[1])
                    strStatus = "\"<error>\"";
                else
                if (devOpen[0] && !devOpen[1])
                    strStatus = "\"<selected, unopened>\"";

                partString += "\"device-status\":" + strStatus + ",";
                std::string imageInfoStringVal = join_string(imageInfoString, imageInfoString +
                    sizeof(imageInfoString) / sizeof(imageInfoString[0])) + ",";
                std::string deviceInfoStringVal = join_string(deviceInfoString, deviceInfoString +
                    sizeof(deviceInfoString) / sizeof(deviceInfoString[0])) + ",";
                if (jsonString.empty())
                    jsonString = " }";
                jsonString = "{" + partString + jColorInfo + resUnitInfo + imageInfoStringVal + deviceInfoStringVal + jsonString.substr(1);
                array_twain_identity.push_back(json::parse(jsonString));
            }

            glob_json["device-info"] = array_twain_identity;
            return glob_json.dump(4);
        }
    }
}
