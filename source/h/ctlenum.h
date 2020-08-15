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
#ifndef CTLEnum_h_
#define CTLEnum_h_
#include "ctltwain.h"
#include "dtwain.h"

namespace dynarithmic
{
    // Define <nice> enumerations for the country constants
    enum CTL_TwainCountryEnum {
                TwainCountry_AFGHANISTAN              = TWCY_AFGHANISTAN  ,
                TwainCountry_ALGERIA                  = TWCY_ALGERIA      ,
                TwainCountry_AMERICANSAMOA            = TWCY_AMERICANSAMOA,
                TwainCountry_ANDORRA                  = TWCY_ANDORRA      ,
                TwainCountry_ANGOLA                   = TWCY_ANGOLA       ,
                TwainCountry_ANGUILLA                 = TWCY_ANGUILLA     ,
                TwainCountry_ANTIGUA                  = TWCY_ANTIGUA      ,
                TwainCountry_ARGENTINA                = TWCY_ARGENTINA    ,
                TwainCountry_ARUBA                    = TWCY_ARUBA        ,
                TwainCountry_ASCENSIONI               = TWCY_ASCENSIONI   ,
                TwainCountry_AUSTRALIA                = TWCY_AUSTRALIA    ,
                TwainCountry_AUSTRIA                  = TWCY_AUSTRIA      ,
                TwainCountry_BAHAMAS                  = TWCY_BAHAMAS      ,
                TwainCountry_BAHRAIN                  = TWCY_BAHRAIN      ,
                TwainCountry_BANGLADESH               = TWCY_BANGLADESH   ,
                TwainCountry_BARBADOS                 = TWCY_BARBADOS     ,
                TwainCountry_BELGIUM                  = TWCY_BELGIUM      ,
                TwainCountry_BELIZE                   = TWCY_BELIZE       ,
                TwainCountry_BENIN                    = TWCY_BENIN        ,
                TwainCountry_BERMUDA                  = TWCY_BERMUDA      ,
                TwainCountry_BHUTAN                   = TWCY_BHUTAN       ,
                TwainCountry_BOLIVIA                  = TWCY_BOLIVIA      ,
                TwainCountry_BOTSWANA                 = TWCY_BOTSWANA     ,
                TwainCountry_BRITAIN                  = TWCY_BRITAIN      ,
                TwainCountry_BRITVIRGINIS             = TWCY_BRITVIRGINIS ,
                TwainCountry_BRAZIL                   = TWCY_BRAZIL       ,
                TwainCountry_BRUNEI                   = TWCY_BRUNEI       ,
                TwainCountry_BULGARIA                 = TWCY_BULGARIA     ,
                TwainCountry_BURKINAFASO              = TWCY_BURKINAFASO  ,
                TwainCountry_BURMA                    = TWCY_BURMA        ,
                TwainCountry_BURUNDI                  = TWCY_BURUNDI      ,
                TwainCountry_CAMAROON                 = TWCY_CAMAROON     ,
                TwainCountry_CANADA                   = TWCY_CANADA       ,
                TwainCountry_CAPEVERDEIS              = TWCY_CAPEVERDEIS  ,
                TwainCountry_CAYMANIS                 = TWCY_CAYMANIS     ,
                TwainCountry_CENTRALAFREP             = TWCY_CENTRALAFREP ,
                TwainCountry_CHAD                     = TWCY_CHAD          ,
                TwainCountry_CHILE                    = TWCY_CHILE         ,
                TwainCountry_CHINA                    = TWCY_CHINA         ,
                TwainCountry_CHRISTMASIS              = TWCY_CHRISTMASIS   ,
                TwainCountry_COCOSIS                  = TWCY_COCOSIS       ,
                TwainCountry_COLOMBIA                 = TWCY_COLOMBIA      ,
                TwainCountry_COMOROS                  = TWCY_COMOROS       ,
                TwainCountry_CONGO                    = TWCY_CONGO         ,
                TwainCountry_COOKIS                   = TWCY_COOKIS        ,
                TwainCountry_COSTARICA                = TWCY_COSTARICA     ,
                TwainCountry_CUBA                     = TWCY_CUBA          ,
                TwainCountry_CYPRUS                   = TWCY_CYPRUS        ,
                TwainCountry_CZECHOSLOVAKIA           = TWCY_CZECHOSLOVAKIA,
                TwainCountry_DENMARK                  = TWCY_DENMARK       ,
                TwainCountry_DJIBOUTI                 = TWCY_DJIBOUTI      ,
                TwainCountry_DOMINICA                 = TWCY_DOMINICA      ,
                TwainCountry_DOMINCANREP              = TWCY_DOMINCANREP   ,
                TwainCountry_EASTERIS                 = TWCY_EASTERIS      ,
                TwainCountry_ECUADOR                  = TWCY_ECUADOR       ,
                TwainCountry_EGYPT                    = TWCY_EGYPT         ,
                TwainCountry_ELSALVADOR               = TWCY_ELSALVADOR    ,
                TwainCountry_EQGUINEA                 = TWCY_EQGUINEA      ,
                TwainCountry_ETHIOPIA                 = TWCY_ETHIOPIA      ,
                TwainCountry_FALKLANDIS               = TWCY_FALKLANDIS    ,
                TwainCountry_FAEROEIS                 = TWCY_FAEROEIS      ,
                TwainCountry_FIJIISLANDS              = TWCY_FIJIISLANDS   ,
                TwainCountry_FINLAND                  = TWCY_FINLAND       ,
                TwainCountry_FRANCE                   = TWCY_FRANCE        ,
                TwainCountry_FRANTILLES               = TWCY_FRANTILLES    ,
                TwainCountry_FRGUIANA                 = TWCY_FRGUIANA      ,
                TwainCountry_FRPOLYNEISA              = TWCY_FRPOLYNEISA   ,
                TwainCountry_FUTANAIS                 = TWCY_FUTANAIS      ,
                TwainCountry_GABON                    = TWCY_GABON         ,
                TwainCountry_GAMBIA                   = TWCY_GAMBIA        ,
                TwainCountry_GERMANY                  = TWCY_GERMANY       ,
                TwainCountry_GHANA                    = TWCY_GHANA         ,
                TwainCountry_GIBRALTER                = TWCY_GIBRALTER     ,
                TwainCountry_GREECE                   = TWCY_GREECE        ,
                TwainCountry_GREENLAND                = TWCY_GREENLAND     ,
                TwainCountry_GRENADA                  = TWCY_GRENADA       ,
                TwainCountry_GRENEDINES               = TWCY_GRENEDINES    ,
                TwainCountry_GUADELOUPE               = TWCY_GUADELOUPE    ,
                TwainCountry_GUAM                     = TWCY_GUAM          ,
                TwainCountry_GUANTANAMOBAY            = TWCY_GUANTANAMOBAY ,
                TwainCountry_GUATEMALA                = TWCY_GUATEMALA     ,
                TwainCountry_GUINEA                   = TWCY_GUINEA        ,
                TwainCountry_GUINEABISSAU             = TWCY_GUINEABISSAU  ,
                TwainCountry_GUYANA                   = TWCY_GUYANA        ,
                TwainCountry_HAITI                    = TWCY_HAITI         ,
                TwainCountry_HONDURAS                 = TWCY_HONDURAS      ,
                TwainCountry_HONGKONG                 = TWCY_HONGKONG      ,
                TwainCountry_HUNGARY                  = TWCY_HUNGARY       ,
                TwainCountry_ICELAND                  = TWCY_ICELAND       ,
                TwainCountry_INDIA                    = TWCY_INDIA         ,
                TwainCountry_INDONESIA                = TWCY_INDONESIA     ,
                TwainCountry_IRAN                     = TWCY_IRAN          ,
                TwainCountry_IRAQ                     = TWCY_IRAQ          ,
                TwainCountry_IRELAND                  = TWCY_IRELAND       ,
                TwainCountry_ISRAEL                   = TWCY_ISRAEL        ,
                TwainCountry_ITALY                    = TWCY_ITALY         ,
                TwainCountry_IVORYCOAST               = TWCY_IVORYCOAST    ,
                TwainCountry_JAMAICA                  = TWCY_JAMAICA       ,
                TwainCountry_JAPAN                    = TWCY_JAPAN         ,
                TwainCountry_JORDAN                   = TWCY_JORDAN        ,
                TwainCountry_KENYA                    = TWCY_KENYA         ,
                TwainCountry_KIRIBATI                 = TWCY_KIRIBATI      ,
                TwainCountry_KOREA                    = TWCY_KOREA         ,
                TwainCountry_KUWAIT                   = TWCY_KUWAIT        ,
                TwainCountry_LAOS                     = TWCY_LAOS          ,
                TwainCountry_LEBANON                  = TWCY_LEBANON       ,
                TwainCountry_LIBERIA                  = TWCY_LIBERIA       ,
                TwainCountry_LIBYA                    = TWCY_LIBYA         ,
                TwainCountry_LIECHTENSTEIN            = TWCY_LIECHTENSTEIN ,
                TwainCountry_LUXENBOURG               = TWCY_LUXENBOURG    ,
                TwainCountry_MACAO                    = TWCY_MACAO         ,
                TwainCountry_MADAGASCAR               = TWCY_MADAGASCAR    ,
                TwainCountry_MALAWI                   = TWCY_MALAWI        ,
                TwainCountry_MALAYSIA                 = TWCY_MALAYSIA      ,
                TwainCountry_MALDIVES                 = TWCY_MALDIVES      ,
                TwainCountry_MALI                     = TWCY_MALI          ,
                TwainCountry_MALTA                    = TWCY_MALTA         ,
                TwainCountry_MARSHALLIS               = TWCY_MARSHALLIS    ,
                TwainCountry_MAURITANIA               = TWCY_MAURITANIA    ,
                TwainCountry_MAURITIUS                = TWCY_MAURITIUS     ,
                TwainCountry_MEXICO                   = TWCY_MEXICO        ,
                TwainCountry_MICRONESIA               = TWCY_MICRONESIA    ,
                TwainCountry_MIQUELON                 = TWCY_MIQUELON      ,
                TwainCountry_MONACO                   = TWCY_MONACO        ,
                TwainCountry_MONGOLIA                 = TWCY_MONGOLIA      ,
                TwainCountry_MONTSERRAT               = TWCY_MONTSERRAT    ,
                TwainCountry_MOROCCO                  = TWCY_MOROCCO       ,
                TwainCountry_MOZAMBIQUE               = TWCY_MOZAMBIQUE    ,
                TwainCountry_NAMIBIA                  = TWCY_NAMIBIA       ,
                TwainCountry_NAURU                    = TWCY_NAURU         ,
                TwainCountry_NEPAL                    = TWCY_NEPAL         ,
                TwainCountry_NETHERLANDS              = TWCY_NETHERLANDS   ,
                TwainCountry_NETHANTILLES             = TWCY_NETHANTILLES  ,
                TwainCountry_NEVIS                    = TWCY_NEVIS         ,
                TwainCountry_NEWCALEDONIA             = TWCY_NEWCALEDONIA  ,
                TwainCountry_NEWZEALAND               = TWCY_NEWZEALAND    ,
                TwainCountry_NICARAGUA                = TWCY_NICARAGUA     ,
                TwainCountry_NIGER                    = TWCY_NIGER         ,
                TwainCountry_NIGERIA                  = TWCY_NIGERIA       ,
                TwainCountry_NIUE                     = TWCY_NIUE          ,
                TwainCountry_NORFOLKI                 = TWCY_NORFOLKI      ,
                TwainCountry_NORWAY                   = TWCY_NORWAY        ,
                TwainCountry_OMAN                     = TWCY_OMAN          ,
                TwainCountry_PAKISTAN                 = TWCY_PAKISTAN      ,
                TwainCountry_PALAU                    = TWCY_PALAU         ,
                TwainCountry_PANAMA                   = TWCY_PANAMA        ,
                TwainCountry_PARAGUAY                 = TWCY_PARAGUAY      ,
                TwainCountry_PERU                     = TWCY_PERU          ,
                TwainCountry_PHILLIPPINES             = TWCY_PHILLIPPINES  ,
                TwainCountry_PITCAIRNIS               = TWCY_PITCAIRNIS    ,
                TwainCountry_PNEWGUINEA               = TWCY_PNEWGUINEA    ,
                TwainCountry_POLAND                   = TWCY_POLAND        ,
                TwainCountry_PORTUGAL                 = TWCY_PORTUGAL      ,
                TwainCountry_QATAR                    = TWCY_QATAR         ,
                TwainCountry_REUNIONI                 = TWCY_REUNIONI      ,
                TwainCountry_ROMANIA                  = TWCY_ROMANIA       ,
                TwainCountry_RWANDA                   = TWCY_RWANDA        ,
                TwainCountry_SAIPAN                   = TWCY_SAIPAN        ,
    //            TwainCountry_SANMARINO                = TWCY_SANMARINO     ,
                TwainCountry_SAOTOME                  = TWCY_SAOTOME       ,
                TwainCountry_SAUDIARABIA              = TWCY_SAUDIARABIA   ,
                TwainCountry_SENEGAL                  = TWCY_SENEGAL       ,
                TwainCountry_SEYCHELLESIS             = TWCY_SEYCHELLESIS  ,
                TwainCountry_SIERRALEONE              = TWCY_SIERRALEONE   ,
                TwainCountry_SINGAPORE                = TWCY_SINGAPORE     ,
                TwainCountry_SOLOMONIS                = TWCY_SOLOMONIS     ,
                TwainCountry_SOMALI                   = TWCY_SOMALI        ,
                TwainCountry_SOUTHAFRICA              = TWCY_SOUTHAFRICA   ,
                TwainCountry_SPAIN                    = TWCY_SPAIN         ,
                TwainCountry_SRILANKA                 = TWCY_SRILANKA      ,
                TwainCountry_STHELENA                 = TWCY_STHELENA      ,
                TwainCountry_STKITTS                  = TWCY_STKITTS       ,
                TwainCountry_STLUCIA                  = TWCY_STLUCIA       ,
                TwainCountry_STPIERRE                 = TWCY_STPIERRE      ,
                TwainCountry_STVINCENT                = TWCY_STVINCENT     ,
                TwainCountry_SUDAN                    = TWCY_SUDAN         ,
                TwainCountry_SURINAME                 = TWCY_SURINAME      ,
                TwainCountry_SWAZILAND                = TWCY_SWAZILAND     ,
                TwainCountry_SWEDEN                   = TWCY_SWEDEN        ,
                TwainCountry_SWITZERLAND              = TWCY_SWITZERLAND   ,
                TwainCountry_SYRIA                    = TWCY_SYRIA         ,
                TwainCountry_TAIWAN                   = TWCY_TAIWAN        ,
                TwainCountry_TANZANIA                 = TWCY_TANZANIA      ,
                TwainCountry_THAILAND                 = TWCY_THAILAND      ,
                TwainCountry_TOBAGO                   = TWCY_TOBAGO        ,
                TwainCountry_TOGO                     = TWCY_TOGO          ,
                TwainCountry_TONGAIS                  = TWCY_TONGAIS       ,
                TwainCountry_TRINIDAD                 = TWCY_TRINIDAD      ,
                TwainCountry_TUNISIA                  = TWCY_TUNISIA       ,
                TwainCountry_TURKEY                   = TWCY_TURKEY        ,
                TwainCountry_TURKSCAICOS              = TWCY_TURKSCAICOS   ,
                TwainCountry_TUVALU                   = TWCY_TUVALU        ,
                TwainCountry_UGANDA                   = TWCY_UGANDA        ,
                TwainCountry_USSR                     = TWCY_USSR          ,
                TwainCountry_UAEMIRATES               = TWCY_UAEMIRATES    ,
                TwainCountry_UNITEDKINGDOM            = TWCY_UNITEDKINGDOM ,
                TwainCountry_USA                      = TWCY_USA           ,
                TwainCountry_URUGUAY                  = TWCY_URUGUAY       ,
                TwainCountry_VANUATU                  = TWCY_VANUATU       ,
                TwainCountry_VATICANCITY              = TWCY_VATICANCITY   ,
                TwainCountry_VENEZUELA                = TWCY_VENEZUELA     ,
                TwainCountry_WAKE                     = TWCY_WAKE          ,
                TwainCountry_WALLISIS                 = TWCY_WALLISIS      ,
                TwainCountry_WESTERNSAHARA            = TWCY_WESTERNSAHARA ,
                TwainCountry_WESTERNSAMOA             = TWCY_WESTERNSAMOA  ,
                TwainCountry_YEMEN                    = TWCY_YEMEN         ,
                TwainCountry_YUGOSLAVIA               = TWCY_YUGOSLAVIA    ,
                TwainCountry_ZAIRE                    = TWCY_ZAIRE         ,
                TwainCountry_ZAMBIA                   = TWCY_ZAMBIA        ,
                TwainCountry_ZIMBABWE                 = TWCY_ZIMBABWE
    };

    enum CTL_TwainLanguageEnum {
            TwainLanguage_DANISH                    = TWLG_DAN ,
            TwainLanguage_DUTCH                     = TWLG_DUT ,
            TwainLanguage_INTERNATIONALENGLISH      = TWLG_ENG ,
            TwainLanguage_FRENCHCANADIAN            = TWLG_FCF ,
            TwainLanguage_FINNISH                   = TWLG_FIN ,
            TwainLanguage_FRENCH                    = TWLG_FRN ,
            TwainLanguage_GERMAN                    = TWLG_GER ,
            TwainLanguage_ICELANDIC                 = TWLG_ICE ,
            TwainLanguage_ITALIAN                   = TWLG_ITN ,
            TwainLanguage_NORWEGIAN                 = TWLG_NOR ,
            TwainLanguage_PORTUGUESE                = TWLG_POR ,
            TwainLanguage_SPANISH                   = TWLG_SPA ,
            TwainLanguage_SWEDISH                   = TWLG_SWE ,
            TwainLanguage_USAENGLISH                = TWLG_USA
    };

    enum CTL_TwainMessageEnum {   CTL_MsgSourceClosed = 1,
                                  CTL_MsgSourceClosing
    };

    enum CTL_TwainAcquireEnum {     TWAINAcquireType_Native = TWSX_NATIVE,
                                    TWAINAcquireType_File   = TWSX_FILE,
                                    TWAINAcquireType_Buffer   = TWSX_MEMORY,
									TWAINAcquireType_AudioNative = 0xFA,
									TWAINAcquireType_AudioFile = 0xFB,
                                    TWAINAcquireType_FileUsingNative = 0xFD,
                                    TWAINAcquireType_Clipboard  =      0xFE,
                                    TWAINAcquireType_Invalid
                             };
    /*        enum { DibFormat=1,
                   BmpFormat=2,
                   JpegFormat=3,
                   PcxFormat=4,
                   TgaFormat=5,
                   TiffFormatLZW=6 };
      */

    enum CTL_TwainFileFormatEnum { TWAINFileFormat_Invalid = -1,
                                   TWAINFileFormat_TWAINBMP                  = DTWAIN_FF_BMP,
                                   TWAINFileFormat_TWAINJFIF                 = DTWAIN_FF_JFIF,
                                   TWAINFileFormat_TWAINSPIFF                = DTWAIN_FF_SPIFF,
                                   TWAINFileFormat_TWAINEXIF                 = DTWAIN_FF_EXIF,
                                   TWAINFileFormat_TWAINPNG                  = DTWAIN_FF_PNG,
                                   TWAINFileFormat_TWAINFPX                  = DTWAIN_FF_FPX,
                                   TWAINFileFormat_TWAINPICT                 = DTWAIN_FF_PICT,
                                   TWAINFileFormat_TWAINXBM                  = DTWAIN_FF_XBM,
                                   TWAINFileFormat_TWAINTIFF                 = DTWAIN_FF_TIFF,
                                   TWAINFileFormat_TWAINTIFFMULTI            = DTWAIN_FF_TIFFMULTI,
                                   TWAINFileFormat_BMP                       = DTWAIN_BMP,
                                   TWAINFileFormat_JPEG                      = DTWAIN_JPEG,
                                   TWAINFileFormat_PDF                       = DTWAIN_PDF,
                                   TWAINFileFormat_PDFMULTI                  = DTWAIN_PDFMULTI,

                                   TWAINFileFormat_PCX                       = DTWAIN_PCX,
                                   TWAINFileFormat_DCX                       = DTWAIN_DCX,
                                   TWAINFileFormat_TGA                       = DTWAIN_TGA,
                                   TWAINFileFormat_TIFFLZW                   = DTWAIN_TIFFLZW,
                                   TWAINFileFormat_TIFFNONE                  = DTWAIN_TIFFNONE,
                                   TWAINFileFormat_TIFFGROUP3                = DTWAIN_TIFFG3,
                                   TWAINFileFormat_TIFFGROUP4                = DTWAIN_TIFFG4,
                                   TWAINFileFormat_TIFFPACKBITS              = DTWAIN_TIFFPACKBITS,
                                   TWAINFileFormat_TIFFDEFLATE               = DTWAIN_TIFFDEFLATE,
                                   TWAINFileFormat_TIFFJPEG                  = DTWAIN_TIFFJPEG,
                                   TWAINFileFormat_TIFFPIXARLOG              = DTWAIN_TIFFPIXARLOG,
                                   TWAINFileFormat_TIFFPIXARLOGMULTI         = DTWAIN_TIFFPIXARLOGMULTI,
                                   TWAINFileFormat_TIFFNONEMULTI             = DTWAIN_TIFFNONEMULTI,
                                   TWAINFileFormat_TIFFGROUP3MULTI           = DTWAIN_TIFFG3MULTI, TWAINFileFormat_TIFFGROUP4MULTI
                                   = DTWAIN_TIFFG4MULTI, TWAINFileFormat_TIFFPACKBITSMULTI         = DTWAIN_TIFFPACKBITSMULTI,
                                   TWAINFileFormat_TIFFDEFLATEMULTI          = DTWAIN_TIFFDEFLATEMULTI,
                                   TWAINFileFormat_TIFFJPEGMULTI             = DTWAIN_TIFFJPEGMULTI, TWAINFileFormat_TIFFLZWMULTI
                                   = DTWAIN_TIFFLZWMULTI, TWAINFileFormat_POSTSCRIPT1               = DTWAIN_POSTSCRIPT1,
                                   TWAINFileFormat_POSTSCRIPT1MULTI          = DTWAIN_POSTSCRIPT1MULTI, TWAINFileFormat_POSTSCRIPT2
                                   = DTWAIN_POSTSCRIPT2, TWAINFileFormat_POSTSCRIPT2MULTI          = DTWAIN_POSTSCRIPT2MULTI,
                                   TWAINFileFormat_POSTSCRIPT3               = DTWAIN_POSTSCRIPT3, TWAINFileFormat_POSTSCRIPT3MULTI
                                   = DTWAIN_POSTSCRIPT3MULTI,
                                   TWAINFileFormat_TEXT                      = DTWAIN_TEXT,
                                   TWAINFileFormat_TEXTMULTI                 = DTWAIN_TEXTMULTI,
                                   TWAINFileFormat_WEBP                      = DTWAIN_WEBP,
                                   TWAINFileFormat_PBM                       = DTWAIN_PBM,

                                   TWAINFileFormat_WMF = DTWAIN_WMF,
                                   TWAINFileFormat_EMF = DTWAIN_EMF,
                                   TWAINFileFormat_GIF      = DTWAIN_GIF,
                                   TWAINFileFormat_PNG      =DTWAIN_PNG,
                                   TWAINFileFormat_PSD      =DTWAIN_PSD,
                                   TWAINFileFormat_JBIG     =5000,
                                   TWAINFileFormat_JPEG2000 = DTWAIN_JPEG2000,
                                   TWAINFileFormat_ICO      = DTWAIN_ICO,
                                   TWAINFileFormat_ICO_VISTA = DTWAIN_ICO_VISTA,
                                   TWAINFileFormat_WBMP     = DTWAIN_WBMP,
                                   TWAINFileFormat_RAW      =9999
                                };


    enum CTL_TwainFileFlags {  TWAINFileFlag_DEFAULTOPTIONS = 0,
                               TWAINFileFlag_USENATIVE  = 1,
                               TWAINFileFlag_USEBUFFERED  = 2,
                               TWAINFileFlag_USENAME    = 4,
                               TWAINFileFlag_PROMPT     = 8,
                               TWAINFileFlag_USELONGNAMES = 16,
                               TWAINFileFlag_USESOURCEMODE = 32,
                               TWAINFileFlag_USELIST    = 64 };

    enum CTL_TwainUnitEnum  {   TwainUnit_INVALID       =   -1,
                                TwainUnit_INCHES        =  TWUN_INCHES,
                                TwainUnit_CENTIMETERS   =  TWUN_CENTIMETERS,
                                TwainUnit_PICAS         =  TWUN_PICAS,
                                TwainUnit_POINTS        =  TWUN_POINTS,
                                TwainUnit_TWIPS         =  TWUN_TWIPS,
                                TwainUnit_PIXELS        =  TWUN_PIXELS
                            };

    enum CTL_EnumGetType { CTL_GetTypeGET         = MSG_GET,
                           CTL_GetTypeGETCURRENT  = MSG_GETCURRENT,
                           CTL_GetTypeGETDEFAULT  = MSG_GETDEFAULT,
                           CTL_GetTypeQUERYSUPPORT = MSG_QUERYSUPPORT
                         };

    enum CTL_EnumSetType { CTL_SetTypeSET         = MSG_SET,
                           CTL_SetTypeRESET       = MSG_RESET,
                           CTL_SetTypeRESETALL    = MSG_RESETALL,
                           CTL_SetTypeSETCONSTRAINT = MSG_SETCONSTRAINT,
                         };

    enum CTL_EnumCapMask {  CTL_CapMaskGET          = 1,
                            CTL_CapMaskGETCURRENT   = 2,
                            CTL_CapMaskGETDEFAULT   = 4,
                            CTL_CapMaskSET          = 8,
                            CTL_CapMaskRESET        = 16,
                            CTL_CapMaskRESETALL     = 32,
                            CTL_CapMaskSETCONSTRAINT = 64,
                         };

    enum CTL_EnumTransferType { CTL_XferTypeNATIVE,
                                CTL_XferTypeMEMORY,
                                CTL_XferTypeFILE
                              };

    enum CTL_EnumTwainVersion { CTL_TwainVersion15 = 0,
                                CTL_TwainVersion16 = 1,
                                CTL_TwainVersion17 = 2,
                                CTL_TwainVersion18 = 3,
                                CTL_TwainVersion19 = 4,
                                CTL_TwainVersion20 = 5,
                                CTL_TwainVersion21 = 6,
                                CTL_TwainVersion22 = 7,
                                CTL_TwainVersion23 = 8,
    };
    typedef TW_UINT16 CTL_EnumCapability;

    #define  TwainCap_INVALID              -1
    #define  TwainCap_XFERCOUNT            CAP_XFERCOUNT
    #define  TwainCap_COMPRESSION          ICAP_COMPRESSION
    #define  TwainCap_PIXELTYPE            ICAP_PIXELTYPE
    #define  TwainCap_UNITS                ICAP_UNITS
    #define  TwainCap_XFERMECH             ICAP_XFERMECH
    #define  TwainCap_AUTHOR               CAP_AUTHOR
    #define  TwainCap_CAPTION              CAP_CAPTION
    #define  TwainCap_FEEDERENABLED        CAP_FEEDERENABLED
    #define  TwainCap_FEEDERLOADED         CAP_FEEDERLOADED
    #define  TwainCap_TIMEDATE             CAP_TIMEDATE
    #define  TwainCap_SUPPORTEDCAPS        CAP_SUPPORTEDCAPS
    #define  TwainCap_EXTENDEDCAPS         CAP_EXTENDEDCAPS
    #define  TwainCap_AUTOFEED             CAP_AUTOFEED
    #define  TwainCap_CLEARPAGE            CAP_CLEARPAGE
    #define  TwainCap_FEEDPAGE             CAP_FEEDPAGE
    #define  TwainCap_REWINDPAGE           CAP_REWINDPAGE
    #define  TwainCap_INDICATORS           CAP_INDICATORS
    #define  TwainCap_SUPPORTEDCAPSEXT     CAP_SUPPORTEDCAPSEXT
    #define  TwainCap_PAPERDETECTABLE      CAP_PAPERDETECTABLE
    #define  TwainCap_UICONTROLLABLE       CAP_UICONTROLLABLE
    #define  TwainCap_DEVICEONLINE         CAP_DEVICEONLINE
    #define  TwainCap_AUTOSCAN             CAP_AUTOSCAN
    #define  TwainCap_AUTOBRIGHT           ICAP_AUTOBRIGHT
    #define  TwainCap_BRIGHTNESS           ICAP_BRIGHTNESS
    #define  TwainCap_CONTRAST             ICAP_CONTRAST
    #define  TwainCap_CUSTHALFTONE         ICAP_CUSTHALFTONE
    #define  TwainCap_EXPOSURETIME         ICAP_EXPOSURETIME
    #define  TwainCap_FILTER               ICAP_FILTER
    #define  TwainCap_FLASHUSED            ICAP_FLASHUSED
    #define  TwainCap_GAMMA                ICAP_GAMMA
    #define  TwainCap_HALFTONES            ICAP_HALFTONES
    #define  TwainCap_HIGHLIGHT            ICAP_HIGHLIGHT
    #define  TwainCap_IMAGEFILEFORMAT      ICAP_IMAGEFILEFORMAT
    #define  TwainCap_LAMPSTATE            ICAP_LAMPSTATE
    #define  TwainCap_LIGHTSOURCE          ICAP_LIGHTSOURCE
    #define  TwainCap_ORIENTATION          ICAP_ORIENTATION
    #define  TwainCap_PHYSICALWIDTH        ICAP_PHYSICALWIDTH
    #define  TwainCap_PHYSICALHEIGHT       ICAP_PHYSICALHEIGHT
    #define  TwainCap_SHADOW               ICAP_SHADOW
    #define  TwainCap_FRAMES               ICAP_FRAMES
    #define  TwainCap_XNATIVERESOLUTION    ICAP_XNATIVERESOLUTION
    #define  TwainCap_YNATIVERESOLUTION    ICAP_YNATIVERESOLUTION
    #define  TwainCap_XRESOLUTION          ICAP_XRESOLUTION
    #define  TwainCap_YRESOLUTION          ICAP_YRESOLUTION
    #define  TwainCap_MAXFRAMES            ICAP_MAXFRAMES
    #define  TwainCap_TILES                ICAP_TILES
    #define  TwainCap_BITORDER             ICAP_BITORDER
    #define  TwainCap_CCITTKFACTOR         ICAP_CCITTKFACTOR
    #define  TwainCap_LIGHTPATH            ICAP_LIGHTPATH
    #define  TwainCap_PIXELFLAVOR          ICAP_PIXELFLAVOR
    #define  TwainCap_PLANARCHUNKY         ICAP_PLANARCHUNKY
    #define  TwainCap_ROTATION             ICAP_ROTATION
    #define  TwainCap_SUPPORTEDSIZES       ICAP_SUPPORTEDSIZES
    #define  TwainCap_THRESHOLD            ICAP_THRESHOLD
    #define  TwainCap_XSCALING             ICAP_XSCALING
    #define  TwainCap_YSCALING             ICAP_YSCALING
    #define  TwainCap_BITORDERCODES        ICAP_BITORDERCODES
    #define  TwainCap_PIXELFLAVORCODES     ICAP_PIXELFLAVORCODES
    #define  TwainCap_JPEGPIXELTYPE        ICAP_JPEGPIXELTYPE
    #define  TwainCap_TIMEFILL             ICAP_TIMEFILL
    #define  TwainCap_BITDEPTH             ICAP_BITDEPTH
    #define  TwainCap_BITDEPTHREDUCTION    ICAP_BITDEPTHREDUCTION
    #define  TwainCap_UNDEFINEDIMAGESIZE   ICAP_UNDEFINEDIMAGESIZE
    #define  TwainCap_IMAGEDATASET         ICAP_IMAGEDATASET
    #define  TwainCap_EXTIMAGEINFO         ICAP_EXTIMAGEINFO
    #define  TwainCap_MINIMUMHEIGHT        ICAP_MINIMUMHEIGHT
    #define  TwainCap_MINIMUMWIDTH         ICAP_MINIMUMWIDTH
    #define  TwainCap_THUMBNAILSENABLED    CAP_THUMBNAILSENABLED
    #define  TwainCap_DUPLEX               CAP_DUPLEX
    #define  TwainCap_DUPLEXENABLED        CAP_DUPLEXENABLED
    #define  TwainCap_ENABLEDSUIONLY       CAP_ENABLEDSUIONLY
    #define  TwainCap_CUSTOMDSDATA         CAP_CUSTOMDSDATA
    #define  TwainCap_ENDORSER             CAP_ENDORSER
    #define  TwainCap_JOBCONTROL           CAP_JOBCONTROL

    enum CTL_EnumContainer {
             TwainContainer_ONEVALUE    = DTWAIN_CONTONEVALUE,
             TwainContainer_ENUMERATION = DTWAIN_CONTENUMERATION,
             TwainContainer_RANGE       = DTWAIN_CONTRANGE,
             TwainContainer_ARRAY       = DTWAIN_CONTARRAY,
             TwainContainer_INVALID     = 0
    };

    enum CTL_EnumTwainRange {
            TwainRange_MIN = 0,
            TwainRange_MAX,
            TwainRange_STEP,
            TwainRange_DEFAULT,
            TwainRange_CURRENT
    };
}
#endif

