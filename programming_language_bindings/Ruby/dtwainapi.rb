#
# This file is part of the Dynarithmic TWAIN Library (DTWAIN).                          
# Copyright (c) 2002-2026 Dynarithmic Software.                                         
#                                                                                       
# Licensed under the Apache License, Version 2.0 (the "License");                       
# you may not use this file except in compliance with the License.                      
# You may obtain a copy of the License at                                               
#                                                                                       
#     http://www.apache.org/licenses/LICENSE-2.0                                        
#                                                                                       
# Unless required by applicable law or agreed to in writing, software                   
# distributed under the License is distributed on an "AS IS" BASIS,                     
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.              
# See the License for the specific language governing permissions and                   
# limitations under the License.                                                        
#                                                                                       
# FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY                   
# DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT 
# OF THIRD PARTY RIGHTS.                                                                
#

#!/usr/bin/env ruby
require 'fiddle'
require 'fiddle/pack'

class DTWAINAPI
    class TW_IDENTITY
      SIZE = 120

      OFFSETS = {
        id:                0,
        version_major:     4,
        version_minor:     6,
        version_language:  8,
        version_country:   10,
        version_info:      12,
        protocol_major:    46,
        protocol_minor:    48,
        supported_groups:  50,
        manufacturer:      54,
        product_family:    88,
        product_name:      122
      }

      def initialize
        @ptr = Fiddle::Pointer.malloc(SIZE)
        @ptr[0, SIZE] = "\0" * SIZE
      end

      def pointer
        @ptr
      end

      # ---- Integer helpers ----
      def read_uint16(offset)
        @ptr[offset, 2].unpack1('S')
      end

      def write_uint16(offset, value)
        @ptr[offset, 2] = [value].pack('S')
      end

      def read_uint32(offset)
        @ptr[offset, 4].unpack1('L')
      end

      def write_uint32(offset, value)
        @ptr[offset, 4] = [value].pack('L')
      end

      # ---- String helpers (TW_STR32) ----
      def read_str32(offset)
        @ptr[offset, 34].split("\0", 2).first
      end

      def write_str32(offset, value)
        buf = value.encode('ASCII')[0, 33] + "\0"
        @ptr[offset, 34] = buf.ljust(34, "\0")
      end

      # ---- Accessors ----
      def id
        read_uint32(0)
      end

      def id=(v)
        write_uint32(0, v)
      end

      def protocol_major
        read_uint16(10)
      end

      def protocol_major=(v)
        write_uint16(10, v)
      end

      def protocol_minor
        read_uint16(12)
      end

      def protocol_minor=(v)
        write_uint16(12, v)
      end

      def supported_groups
        read_uint32(14)
      end

      def supported_groups=(v)
        write_uint32(14, v)
      end

      def manufacturer
        read_str32(18)
      end

      def manufacturer=(v)
        write_str32(18, v)
      end

      def product_family
        read_str32(52)
      end

      def product_family=(v)
        write_str32(52, v)
      end

      def product_name
        read_str32(86)
      end

      def product_name=(v)
        write_str32(86, v)
      end
    end
   attr_reader :DTWAIN_AcquireAudioFile
   attr_reader :DTWAIN_AcquireAudioFileA
   attr_reader :DTWAIN_AcquireAudioFileW
   attr_reader :DTWAIN_AcquireAudioNative
   attr_reader :DTWAIN_AcquireAudioNativeEx
   attr_reader :DTWAIN_AcquireBuffered
   attr_reader :DTWAIN_AcquireBufferedEx
   attr_reader :DTWAIN_AcquireFile
   attr_reader :DTWAIN_AcquireFileA
   attr_reader :DTWAIN_AcquireFileEx
   attr_reader :DTWAIN_AcquireFileW
   attr_reader :DTWAIN_AcquireNative
   attr_reader :DTWAIN_AcquireNativeEx
   attr_reader :DTWAIN_AcquireToClipboard
   attr_reader :DTWAIN_AddExtImageInfoQuery
   attr_reader :DTWAIN_AddPDFText
   attr_reader :DTWAIN_AddPDFTextA
   attr_reader :DTWAIN_AddPDFTextEx
   attr_reader :DTWAIN_AddPDFTextW
   attr_reader :DTWAIN_AllocateMemory
   attr_reader :DTWAIN_AllocateMemory64
   attr_reader :DTWAIN_AllocateMemoryEx
   attr_reader :DTWAIN_AppHandlesExceptions
   attr_reader :DTWAIN_ArrayANSIStringToFloat
   attr_reader :DTWAIN_ArrayAdd
   attr_reader :DTWAIN_ArrayAddANSIString
   attr_reader :DTWAIN_ArrayAddANSIStringN
   attr_reader :DTWAIN_ArrayAddFloat
   attr_reader :DTWAIN_ArrayAddFloatN
   attr_reader :DTWAIN_ArrayAddFloatString
   attr_reader :DTWAIN_ArrayAddFloatStringA
   attr_reader :DTWAIN_ArrayAddFloatStringN
   attr_reader :DTWAIN_ArrayAddFloatStringNA
   attr_reader :DTWAIN_ArrayAddFloatStringNW
   attr_reader :DTWAIN_ArrayAddFloatStringW
   attr_reader :DTWAIN_ArrayAddFrame
   attr_reader :DTWAIN_ArrayAddFrameN
   attr_reader :DTWAIN_ArrayAddLong
   attr_reader :DTWAIN_ArrayAddLong64
   attr_reader :DTWAIN_ArrayAddLong64N
   attr_reader :DTWAIN_ArrayAddLongN
   attr_reader :DTWAIN_ArrayAddN
   attr_reader :DTWAIN_ArrayAddString
   attr_reader :DTWAIN_ArrayAddStringA
   attr_reader :DTWAIN_ArrayAddStringN
   attr_reader :DTWAIN_ArrayAddStringNA
   attr_reader :DTWAIN_ArrayAddStringNW
   attr_reader :DTWAIN_ArrayAddStringW
   attr_reader :DTWAIN_ArrayAddWideString
   attr_reader :DTWAIN_ArrayAddWideStringN
   attr_reader :DTWAIN_ArrayConvertFix32ToFloat
   attr_reader :DTWAIN_ArrayConvertFloatToFix32
   attr_reader :DTWAIN_ArrayCopy
   attr_reader :DTWAIN_ArrayCreate
   attr_reader :DTWAIN_ArrayCreateCopy
   attr_reader :DTWAIN_ArrayCreateFromCap
   attr_reader :DTWAIN_ArrayCreateFromLong64s
   attr_reader :DTWAIN_ArrayCreateFromLongs
   attr_reader :DTWAIN_ArrayCreateFromReals
   attr_reader :DTWAIN_ArrayDestroy
   attr_reader :DTWAIN_ArrayDestroyFrames
   attr_reader :DTWAIN_ArrayFind
   attr_reader :DTWAIN_ArrayFindANSIString
   attr_reader :DTWAIN_ArrayFindFloat
   attr_reader :DTWAIN_ArrayFindFloatString
   attr_reader :DTWAIN_ArrayFindFloatStringA
   attr_reader :DTWAIN_ArrayFindFloatStringW
   attr_reader :DTWAIN_ArrayFindLong
   attr_reader :DTWAIN_ArrayFindLong64
   attr_reader :DTWAIN_ArrayFindString
   attr_reader :DTWAIN_ArrayFindStringA
   attr_reader :DTWAIN_ArrayFindStringW
   attr_reader :DTWAIN_ArrayFindWideString
   attr_reader :DTWAIN_ArrayFix32GetAt
   attr_reader :DTWAIN_ArrayFix32SetAt
   attr_reader :DTWAIN_ArrayFloatToANSIString
   attr_reader :DTWAIN_ArrayFloatToString
   attr_reader :DTWAIN_ArrayFloatToWideString
   attr_reader :DTWAIN_ArrayGetAt
   attr_reader :DTWAIN_ArrayGetAtANSIString
   attr_reader :DTWAIN_ArrayGetAtANSIStringPtr
   attr_reader :DTWAIN_ArrayGetAtFloat
   attr_reader :DTWAIN_ArrayGetAtFloatString
   attr_reader :DTWAIN_ArrayGetAtFloatStringA
   attr_reader :DTWAIN_ArrayGetAtFloatStringW
   attr_reader :DTWAIN_ArrayGetAtFrame
   attr_reader :DTWAIN_ArrayGetAtFrameEx
   attr_reader :DTWAIN_ArrayGetAtFrameString
   attr_reader :DTWAIN_ArrayGetAtFrameStringA
   attr_reader :DTWAIN_ArrayGetAtFrameStringW
   attr_reader :DTWAIN_ArrayGetAtLong
   attr_reader :DTWAIN_ArrayGetAtLong64
   attr_reader :DTWAIN_ArrayGetAtSource
   attr_reader :DTWAIN_ArrayGetAtString
   attr_reader :DTWAIN_ArrayGetAtStringA
   attr_reader :DTWAIN_ArrayGetAtStringPtr
   attr_reader :DTWAIN_ArrayGetAtStringW
   attr_reader :DTWAIN_ArrayGetAtWideString
   attr_reader :DTWAIN_ArrayGetAtWideStringPtr
   attr_reader :DTWAIN_ArrayGetBuffer
   attr_reader :DTWAIN_ArrayGetCapValues
   attr_reader :DTWAIN_ArrayGetCapValuesEx
   attr_reader :DTWAIN_ArrayGetCapValuesEx2
   attr_reader :DTWAIN_ArrayGetCount
   attr_reader :DTWAIN_ArrayGetMaxStringLength
   attr_reader :DTWAIN_ArrayGetSourceAt
   attr_reader :DTWAIN_ArrayGetStringLength
   attr_reader :DTWAIN_ArrayGetType
   attr_reader :DTWAIN_ArrayInit
   attr_reader :DTWAIN_ArrayInsertAt
   attr_reader :DTWAIN_ArrayInsertAtANSIString
   attr_reader :DTWAIN_ArrayInsertAtANSIStringN
   attr_reader :DTWAIN_ArrayInsertAtFloat
   attr_reader :DTWAIN_ArrayInsertAtFloatN
   attr_reader :DTWAIN_ArrayInsertAtFloatString
   attr_reader :DTWAIN_ArrayInsertAtFloatStringA
   attr_reader :DTWAIN_ArrayInsertAtFloatStringN
   attr_reader :DTWAIN_ArrayInsertAtFloatStringNA
   attr_reader :DTWAIN_ArrayInsertAtFloatStringNW
   attr_reader :DTWAIN_ArrayInsertAtFloatStringW
   attr_reader :DTWAIN_ArrayInsertAtFrame
   attr_reader :DTWAIN_ArrayInsertAtFrameN
   attr_reader :DTWAIN_ArrayInsertAtLong
   attr_reader :DTWAIN_ArrayInsertAtLong64
   attr_reader :DTWAIN_ArrayInsertAtLong64N
   attr_reader :DTWAIN_ArrayInsertAtLongN
   attr_reader :DTWAIN_ArrayInsertAtN
   attr_reader :DTWAIN_ArrayInsertAtString
   attr_reader :DTWAIN_ArrayInsertAtStringA
   attr_reader :DTWAIN_ArrayInsertAtStringN
   attr_reader :DTWAIN_ArrayInsertAtStringNA
   attr_reader :DTWAIN_ArrayInsertAtStringNW
   attr_reader :DTWAIN_ArrayInsertAtStringW
   attr_reader :DTWAIN_ArrayInsertAtWideString
   attr_reader :DTWAIN_ArrayInsertAtWideStringN
   attr_reader :DTWAIN_ArrayRemoveAll
   attr_reader :DTWAIN_ArrayRemoveAt
   attr_reader :DTWAIN_ArrayRemoveAtN
   attr_reader :DTWAIN_ArrayResize
   attr_reader :DTWAIN_ArraySetAt
   attr_reader :DTWAIN_ArraySetAtANSIString
   attr_reader :DTWAIN_ArraySetAtFloat
   attr_reader :DTWAIN_ArraySetAtFloatString
   attr_reader :DTWAIN_ArraySetAtFloatStringA
   attr_reader :DTWAIN_ArraySetAtFloatStringW
   attr_reader :DTWAIN_ArraySetAtFrame
   attr_reader :DTWAIN_ArraySetAtFrameEx
   attr_reader :DTWAIN_ArraySetAtFrameString
   attr_reader :DTWAIN_ArraySetAtFrameStringA
   attr_reader :DTWAIN_ArraySetAtFrameStringW
   attr_reader :DTWAIN_ArraySetAtLong
   attr_reader :DTWAIN_ArraySetAtLong64
   attr_reader :DTWAIN_ArraySetAtString
   attr_reader :DTWAIN_ArraySetAtStringA
   attr_reader :DTWAIN_ArraySetAtStringW
   attr_reader :DTWAIN_ArraySetAtWideString
   attr_reader :DTWAIN_ArrayStringToFloat
   attr_reader :DTWAIN_ArrayWideStringToFloat
   attr_reader :DTWAIN_CallCallback
   attr_reader :DTWAIN_CallCallback64
   attr_reader :DTWAIN_CallDSMProc
   attr_reader :DTWAIN_CheckHandles
   attr_reader :DTWAIN_ClearBuffers
   attr_reader :DTWAIN_ClearErrorBuffer
   attr_reader :DTWAIN_ClearPDFText
   attr_reader :DTWAIN_ClearPage
   attr_reader :DTWAIN_CloseSource
   attr_reader :DTWAIN_CloseSourceUI
   attr_reader :DTWAIN_ConvertDIBToBitmap
   attr_reader :DTWAIN_ConvertDIBToFullBitmap
   attr_reader :DTWAIN_ConvertToAPIString
   attr_reader :DTWAIN_ConvertToAPIStringA
   attr_reader :DTWAIN_ConvertToAPIStringEx
   attr_reader :DTWAIN_ConvertToAPIStringExA
   attr_reader :DTWAIN_ConvertToAPIStringExW
   attr_reader :DTWAIN_ConvertToAPIStringW
   attr_reader :DTWAIN_CreateAcquisitionArray
   attr_reader :DTWAIN_CreatePDFTextElement
   attr_reader :DTWAIN_DeleteDIB
   attr_reader :DTWAIN_DestroyAcquisitionArray
   attr_reader :DTWAIN_DestroyPDFTextElement
   attr_reader :DTWAIN_DisableAppWindow
   attr_reader :DTWAIN_EnableAutoBorderDetect
   attr_reader :DTWAIN_EnableAutoBright
   attr_reader :DTWAIN_EnableAutoDeskew
   attr_reader :DTWAIN_EnableAutoFeed
   attr_reader :DTWAIN_EnableAutoRotate
   attr_reader :DTWAIN_EnableAutoScan
   attr_reader :DTWAIN_EnableAutomaticSenseMedium
   attr_reader :DTWAIN_EnableDuplex
   attr_reader :DTWAIN_EnableFeeder
   attr_reader :DTWAIN_EnableIndicator
   attr_reader :DTWAIN_EnableJobFileHandling
   attr_reader :DTWAIN_EnableLamp
   attr_reader :DTWAIN_EnableMsgNotify
   attr_reader :DTWAIN_EnablePatchDetect
   attr_reader :DTWAIN_EnablePeekMessageLoop
   attr_reader :DTWAIN_EnablePrinter
   attr_reader :DTWAIN_EnableThumbnail
   attr_reader :DTWAIN_EnableTripletsNotify
   attr_reader :DTWAIN_EndThread
   attr_reader :DTWAIN_EndTwainSession
   attr_reader :DTWAIN_EnumAlarmVolumes
   attr_reader :DTWAIN_EnumAlarmVolumesEx
   attr_reader :DTWAIN_EnumAlarms
   attr_reader :DTWAIN_EnumAlarmsEx
   attr_reader :DTWAIN_EnumAudioXferMechs
   attr_reader :DTWAIN_EnumAudioXferMechsEx
   attr_reader :DTWAIN_EnumAutoFeedValues
   attr_reader :DTWAIN_EnumAutoFeedValuesEx
   attr_reader :DTWAIN_EnumAutomaticCaptures
   attr_reader :DTWAIN_EnumAutomaticCapturesEx
   attr_reader :DTWAIN_EnumAutomaticSenseMedium
   attr_reader :DTWAIN_EnumAutomaticSenseMediumEx
   attr_reader :DTWAIN_EnumBitDepths
   attr_reader :DTWAIN_EnumBitDepthsEx
   attr_reader :DTWAIN_EnumBitDepthsEx2
   attr_reader :DTWAIN_EnumBottomCameras
   attr_reader :DTWAIN_EnumBottomCamerasEx
   attr_reader :DTWAIN_EnumBrightnessValues
   attr_reader :DTWAIN_EnumBrightnessValuesEx
   attr_reader :DTWAIN_EnumCameras
   attr_reader :DTWAIN_EnumCamerasEx
   attr_reader :DTWAIN_EnumCamerasEx2
   attr_reader :DTWAIN_EnumCamerasEx3
   attr_reader :DTWAIN_EnumCompressionTypes
   attr_reader :DTWAIN_EnumCompressionTypesEx
   attr_reader :DTWAIN_EnumCompressionTypesEx2
   attr_reader :DTWAIN_EnumContrastValues
   attr_reader :DTWAIN_EnumContrastValuesEx
   attr_reader :DTWAIN_EnumCustomCaps
   attr_reader :DTWAIN_EnumCustomCapsEx2
   attr_reader :DTWAIN_EnumDoubleFeedDetectLengths
   attr_reader :DTWAIN_EnumDoubleFeedDetectLengthsEx
   attr_reader :DTWAIN_EnumDoubleFeedDetectValues
   attr_reader :DTWAIN_EnumDoubleFeedDetectValuesEx
   attr_reader :DTWAIN_EnumExtImageInfoTypes
   attr_reader :DTWAIN_EnumExtImageInfoTypesEx
   attr_reader :DTWAIN_EnumExtendedCaps
   attr_reader :DTWAIN_EnumExtendedCapsEx
   attr_reader :DTWAIN_EnumExtendedCapsEx2
   attr_reader :DTWAIN_EnumFileTypeBitsPerPixel
   attr_reader :DTWAIN_EnumFileXferFormats
   attr_reader :DTWAIN_EnumFileXferFormatsEx
   attr_reader :DTWAIN_EnumHalftones
   attr_reader :DTWAIN_EnumHalftonesEx
   attr_reader :DTWAIN_EnumHighlightValues
   attr_reader :DTWAIN_EnumHighlightValuesEx
   attr_reader :DTWAIN_EnumJobControls
   attr_reader :DTWAIN_EnumJobControlsEx
   attr_reader :DTWAIN_EnumLightPaths
   attr_reader :DTWAIN_EnumLightPathsEx
   attr_reader :DTWAIN_EnumLightSources
   attr_reader :DTWAIN_EnumLightSourcesEx
   attr_reader :DTWAIN_EnumMaxBuffers
   attr_reader :DTWAIN_EnumMaxBuffersEx
   attr_reader :DTWAIN_EnumNoiseFilters
   attr_reader :DTWAIN_EnumNoiseFiltersEx
   attr_reader :DTWAIN_EnumOCRInterfaces
   attr_reader :DTWAIN_EnumOCRSupportedCaps
   attr_reader :DTWAIN_EnumOrientations
   attr_reader :DTWAIN_EnumOrientationsEx
   attr_reader :DTWAIN_EnumOverscanValues
   attr_reader :DTWAIN_EnumOverscanValuesEx
   attr_reader :DTWAIN_EnumPaperSizes
   attr_reader :DTWAIN_EnumPaperSizesEx
   attr_reader :DTWAIN_EnumPatchCodes
   attr_reader :DTWAIN_EnumPatchCodesEx
   attr_reader :DTWAIN_EnumPatchMaxPriorities
   attr_reader :DTWAIN_EnumPatchMaxPrioritiesEx
   attr_reader :DTWAIN_EnumPatchMaxRetries
   attr_reader :DTWAIN_EnumPatchMaxRetriesEx
   attr_reader :DTWAIN_EnumPatchPriorities
   attr_reader :DTWAIN_EnumPatchPrioritiesEx
   attr_reader :DTWAIN_EnumPatchSearchModes
   attr_reader :DTWAIN_EnumPatchSearchModesEx
   attr_reader :DTWAIN_EnumPatchTimeOutValues
   attr_reader :DTWAIN_EnumPatchTimeOutValuesEx
   attr_reader :DTWAIN_EnumPixelTypes
   attr_reader :DTWAIN_EnumPixelTypesEx
   attr_reader :DTWAIN_EnumPrinterStringModes
   attr_reader :DTWAIN_EnumPrinterStringModesEx
   attr_reader :DTWAIN_EnumResolutionValues
   attr_reader :DTWAIN_EnumResolutionValuesEx
   attr_reader :DTWAIN_EnumShadowValues
   attr_reader :DTWAIN_EnumShadowValuesEx
   attr_reader :DTWAIN_EnumSourceUnits
   attr_reader :DTWAIN_EnumSourceUnitsEx
   attr_reader :DTWAIN_EnumSourceValues
   attr_reader :DTWAIN_EnumSourceValuesA
   attr_reader :DTWAIN_EnumSourceValuesW
   attr_reader :DTWAIN_EnumSources
   attr_reader :DTWAIN_EnumSourcesEx
   attr_reader :DTWAIN_EnumSupportedCaps
   attr_reader :DTWAIN_EnumSupportedCapsEx
   attr_reader :DTWAIN_EnumSupportedCapsEx2
   attr_reader :DTWAIN_EnumSupportedExtImageInfo
   attr_reader :DTWAIN_EnumSupportedExtImageInfoEx
   attr_reader :DTWAIN_EnumSupportedFileTypes
   attr_reader :DTWAIN_EnumSupportedMultiPageFileTypes
   attr_reader :DTWAIN_EnumSupportedSinglePageFileTypes
   attr_reader :DTWAIN_EnumThresholdValues
   attr_reader :DTWAIN_EnumThresholdValuesEx
   attr_reader :DTWAIN_EnumTopCameras
   attr_reader :DTWAIN_EnumTopCamerasEx
   attr_reader :DTWAIN_EnumTwainPrinters
   attr_reader :DTWAIN_EnumTwainPrintersArray
   attr_reader :DTWAIN_EnumTwainPrintersArrayEx
   attr_reader :DTWAIN_EnumTwainPrintersEx
   attr_reader :DTWAIN_EnumXResolutionValues
   attr_reader :DTWAIN_EnumXResolutionValuesEx
   attr_reader :DTWAIN_EnumYResolutionValues
   attr_reader :DTWAIN_EnumYResolutionValuesEx
   attr_reader :DTWAIN_ExecuteOCR
   attr_reader :DTWAIN_ExecuteOCRA
   attr_reader :DTWAIN_ExecuteOCRW
   attr_reader :DTWAIN_FeedPage
   attr_reader :DTWAIN_FlipBitmap
   attr_reader :DTWAIN_FlushAcquiredPages
   attr_reader :DTWAIN_ForceAcquireBitDepth
   attr_reader :DTWAIN_ForceScanOnNoUI
   attr_reader :DTWAIN_FrameCreate
   attr_reader :DTWAIN_FrameCreateString
   attr_reader :DTWAIN_FrameCreateStringA
   attr_reader :DTWAIN_FrameCreateStringW
   attr_reader :DTWAIN_FrameDestroy
   attr_reader :DTWAIN_FrameGetAll
   attr_reader :DTWAIN_FrameGetAllString
   attr_reader :DTWAIN_FrameGetAllStringA
   attr_reader :DTWAIN_FrameGetAllStringW
   attr_reader :DTWAIN_FrameGetValue
   attr_reader :DTWAIN_FrameGetValueString
   attr_reader :DTWAIN_FrameGetValueStringA
   attr_reader :DTWAIN_FrameGetValueStringW
   attr_reader :DTWAIN_FrameIsValid
   attr_reader :DTWAIN_FrameSetAll
   attr_reader :DTWAIN_FrameSetAllString
   attr_reader :DTWAIN_FrameSetAllStringA
   attr_reader :DTWAIN_FrameSetAllStringW
   attr_reader :DTWAIN_FrameSetValue
   attr_reader :DTWAIN_FrameSetValueString
   attr_reader :DTWAIN_FrameSetValueStringA
   attr_reader :DTWAIN_FrameSetValueStringW
   attr_reader :DTWAIN_FreeExtImageInfo
   attr_reader :DTWAIN_FreeMemory
   attr_reader :DTWAIN_FreeMemoryEx
   attr_reader :DTWAIN_GetAPIHandleStatus
   attr_reader :DTWAIN_GetAcquireArea
   attr_reader :DTWAIN_GetAcquireArea2
   attr_reader :DTWAIN_GetAcquireArea2String
   attr_reader :DTWAIN_GetAcquireArea2StringA
   attr_reader :DTWAIN_GetAcquireArea2StringW
   attr_reader :DTWAIN_GetAcquireAreaEx
   attr_reader :DTWAIN_GetAcquireMetrics
   attr_reader :DTWAIN_GetAcquireStripBuffer
   attr_reader :DTWAIN_GetAcquireStripData
   attr_reader :DTWAIN_GetAcquireStripSizes
   attr_reader :DTWAIN_GetAcquiredImage
   attr_reader :DTWAIN_GetAcquiredImageArray
   attr_reader :DTWAIN_GetActiveDSMPath
   attr_reader :DTWAIN_GetActiveDSMPathA
   attr_reader :DTWAIN_GetActiveDSMPathW
   attr_reader :DTWAIN_GetActiveDSMVersionInfo
   attr_reader :DTWAIN_GetActiveDSMVersionInfoA
   attr_reader :DTWAIN_GetActiveDSMVersionInfoW
   attr_reader :DTWAIN_GetAlarmVolume
   attr_reader :DTWAIN_GetAllSourceDibs
   attr_reader :DTWAIN_GetAppInfo
   attr_reader :DTWAIN_GetAppInfoA
   attr_reader :DTWAIN_GetAppInfoW
   attr_reader :DTWAIN_GetAuthor
   attr_reader :DTWAIN_GetAuthorA
   attr_reader :DTWAIN_GetAuthorW
   attr_reader :DTWAIN_GetBatteryMinutes
   attr_reader :DTWAIN_GetBatteryPercent
   attr_reader :DTWAIN_GetBitDepth
   attr_reader :DTWAIN_GetBlankPageAutoDetection
   attr_reader :DTWAIN_GetBrightness
   attr_reader :DTWAIN_GetBrightnessString
   attr_reader :DTWAIN_GetBrightnessStringA
   attr_reader :DTWAIN_GetBrightnessStringW
   attr_reader :DTWAIN_GetBufferedTransferInfo
   attr_reader :DTWAIN_GetCallback
   attr_reader :DTWAIN_GetCallback64
   attr_reader :DTWAIN_GetCapArrayType
   attr_reader :DTWAIN_GetCapContainer
   attr_reader :DTWAIN_GetCapContainerEx
   attr_reader :DTWAIN_GetCapContainerEx2
   attr_reader :DTWAIN_GetCapDataType
   attr_reader :DTWAIN_GetCapFromName
   attr_reader :DTWAIN_GetCapFromNameA
   attr_reader :DTWAIN_GetCapFromNameW
   attr_reader :DTWAIN_GetCapOperations
   attr_reader :DTWAIN_GetCapValues
   attr_reader :DTWAIN_GetCapValuesEx
   attr_reader :DTWAIN_GetCapValuesEx2
   attr_reader :DTWAIN_GetCaption
   attr_reader :DTWAIN_GetCaptionA
   attr_reader :DTWAIN_GetCaptionW
   attr_reader :DTWAIN_GetCompressionSize
   attr_reader :DTWAIN_GetCompressionType
   attr_reader :DTWAIN_GetConditionCodeString
   attr_reader :DTWAIN_GetConditionCodeStringA
   attr_reader :DTWAIN_GetConditionCodeStringW
   attr_reader :DTWAIN_GetContrast
   attr_reader :DTWAIN_GetContrastString
   attr_reader :DTWAIN_GetContrastStringA
   attr_reader :DTWAIN_GetContrastStringW
   attr_reader :DTWAIN_GetCountry
   attr_reader :DTWAIN_GetCurrentAcquiredImage
   attr_reader :DTWAIN_GetCurrentFileName
   attr_reader :DTWAIN_GetCurrentFileNameA
   attr_reader :DTWAIN_GetCurrentFileNameW
   attr_reader :DTWAIN_GetCurrentPageNum
   attr_reader :DTWAIN_GetCurrentRetryCount
   attr_reader :DTWAIN_GetCurrentTwainTriplet
   attr_reader :DTWAIN_GetCustomDSData
   attr_reader :DTWAIN_GetDSMFullName
   attr_reader :DTWAIN_GetDSMFullNameA
   attr_reader :DTWAIN_GetDSMFullNameW
   attr_reader :DTWAIN_GetDSMSearchOrder
   attr_reader :DTWAIN_GetDTWAINHandle
   attr_reader :DTWAIN_GetDeviceEvent
   attr_reader :DTWAIN_GetDeviceEventEx
   attr_reader :DTWAIN_GetDeviceEventInfo
   attr_reader :DTWAIN_GetDeviceNotifications
   attr_reader :DTWAIN_GetDeviceTimeDate
   attr_reader :DTWAIN_GetDeviceTimeDateA
   attr_reader :DTWAIN_GetDeviceTimeDateW
   attr_reader :DTWAIN_GetDoubleFeedDetectLength
   attr_reader :DTWAIN_GetDoubleFeedDetectValues
   attr_reader :DTWAIN_GetDuplexType
   attr_reader :DTWAIN_GetErrorBuffer
   attr_reader :DTWAIN_GetErrorBufferThreshold
   attr_reader :DTWAIN_GetErrorCallback
   attr_reader :DTWAIN_GetErrorCallback64
   attr_reader :DTWAIN_GetErrorString
   attr_reader :DTWAIN_GetErrorStringA
   attr_reader :DTWAIN_GetErrorStringW
   attr_reader :DTWAIN_GetExtCapFromName
   attr_reader :DTWAIN_GetExtCapFromNameA
   attr_reader :DTWAIN_GetExtCapFromNameW
   attr_reader :DTWAIN_GetExtImageInfo
   attr_reader :DTWAIN_GetExtImageInfoData
   attr_reader :DTWAIN_GetExtImageInfoDataEx
   attr_reader :DTWAIN_GetExtImageInfoItem
   attr_reader :DTWAIN_GetExtImageInfoItemEx
   attr_reader :DTWAIN_GetExtNameFromCap
   attr_reader :DTWAIN_GetExtNameFromCapA
   attr_reader :DTWAIN_GetExtNameFromCapW
   attr_reader :DTWAIN_GetFeederAlignment
   attr_reader :DTWAIN_GetFeederFuncs
   attr_reader :DTWAIN_GetFeederOrder
   attr_reader :DTWAIN_GetFeederWaitTime
   attr_reader :DTWAIN_GetFileCompressionType
   attr_reader :DTWAIN_GetFileSavePageCount
   attr_reader :DTWAIN_GetFileTypeExtensions
   attr_reader :DTWAIN_GetFileTypeExtensionsA
   attr_reader :DTWAIN_GetFileTypeExtensionsW
   attr_reader :DTWAIN_GetFileTypeName
   attr_reader :DTWAIN_GetFileTypeNameA
   attr_reader :DTWAIN_GetFileTypeNameW
   attr_reader :DTWAIN_GetHalftone
   attr_reader :DTWAIN_GetHalftoneA
   attr_reader :DTWAIN_GetHalftoneW
   attr_reader :DTWAIN_GetHighlight
   attr_reader :DTWAIN_GetHighlightString
   attr_reader :DTWAIN_GetHighlightStringA
   attr_reader :DTWAIN_GetHighlightStringW
   attr_reader :DTWAIN_GetImageInfo
   attr_reader :DTWAIN_GetImageInfoString
   attr_reader :DTWAIN_GetImageInfoStringA
   attr_reader :DTWAIN_GetImageInfoStringW
   attr_reader :DTWAIN_GetJobControl
   attr_reader :DTWAIN_GetJpegValues
   attr_reader :DTWAIN_GetJpegXRValues
   attr_reader :DTWAIN_GetLanguage
   attr_reader :DTWAIN_GetLastError
   attr_reader :DTWAIN_GetLibraryPath
   attr_reader :DTWAIN_GetLibraryPathA
   attr_reader :DTWAIN_GetLibraryPathW
   attr_reader :DTWAIN_GetLightPath
   attr_reader :DTWAIN_GetLightSource
   attr_reader :DTWAIN_GetLightSources
   attr_reader :DTWAIN_GetLoggerCallback
   attr_reader :DTWAIN_GetLoggerCallbackA
   attr_reader :DTWAIN_GetLoggerCallbackW
   attr_reader :DTWAIN_GetManualDuplexCount
   attr_reader :DTWAIN_GetMaxAcquisitions
   attr_reader :DTWAIN_GetMaxBuffers
   attr_reader :DTWAIN_GetMaxPagesToAcquire
   attr_reader :DTWAIN_GetMaxRetryAttempts
   attr_reader :DTWAIN_GetNameFromCap
   attr_reader :DTWAIN_GetNameFromCapA
   attr_reader :DTWAIN_GetNameFromCapW
   attr_reader :DTWAIN_GetNoiseFilter
   attr_reader :DTWAIN_GetNumAcquiredImages
   attr_reader :DTWAIN_GetNumAcquisitions
   attr_reader :DTWAIN_GetOCRCapValues
   attr_reader :DTWAIN_GetOCRErrorString
   attr_reader :DTWAIN_GetOCRErrorStringA
   attr_reader :DTWAIN_GetOCRErrorStringW
   attr_reader :DTWAIN_GetOCRLastError
   attr_reader :DTWAIN_GetOCRMajorMinorVersion
   attr_reader :DTWAIN_GetOCRManufacturer
   attr_reader :DTWAIN_GetOCRManufacturerA
   attr_reader :DTWAIN_GetOCRManufacturerW
   attr_reader :DTWAIN_GetOCRProductFamily
   attr_reader :DTWAIN_GetOCRProductFamilyA
   attr_reader :DTWAIN_GetOCRProductFamilyW
   attr_reader :DTWAIN_GetOCRProductName
   attr_reader :DTWAIN_GetOCRProductNameA
   attr_reader :DTWAIN_GetOCRProductNameW
   attr_reader :DTWAIN_GetOCRText
   attr_reader :DTWAIN_GetOCRTextA
   attr_reader :DTWAIN_GetOCRTextInfoFloat
   attr_reader :DTWAIN_GetOCRTextInfoFloatEx
   attr_reader :DTWAIN_GetOCRTextInfoHandle
   attr_reader :DTWAIN_GetOCRTextInfoLong
   attr_reader :DTWAIN_GetOCRTextInfoLongEx
   attr_reader :DTWAIN_GetOCRTextW
   attr_reader :DTWAIN_GetOCRVersionInfo
   attr_reader :DTWAIN_GetOCRVersionInfoA
   attr_reader :DTWAIN_GetOCRVersionInfoW
   attr_reader :DTWAIN_GetOrientation
   attr_reader :DTWAIN_GetOverscan
   attr_reader :DTWAIN_GetPDFTextElementFloat
   attr_reader :DTWAIN_GetPDFTextElementLong
   attr_reader :DTWAIN_GetPDFTextElementString
   attr_reader :DTWAIN_GetPDFTextElementStringA
   attr_reader :DTWAIN_GetPDFTextElementStringW
   attr_reader :DTWAIN_GetPDFType1FontName
   attr_reader :DTWAIN_GetPDFType1FontNameA
   attr_reader :DTWAIN_GetPDFType1FontNameW
   attr_reader :DTWAIN_GetPaperSize
   attr_reader :DTWAIN_GetPaperSizeName
   attr_reader :DTWAIN_GetPaperSizeNameA
   attr_reader :DTWAIN_GetPaperSizeNameW
   attr_reader :DTWAIN_GetPatchMaxPriorities
   attr_reader :DTWAIN_GetPatchMaxRetries
   attr_reader :DTWAIN_GetPatchPriorities
   attr_reader :DTWAIN_GetPatchSearchMode
   attr_reader :DTWAIN_GetPatchTimeOut
   attr_reader :DTWAIN_GetPixelFlavor
   attr_reader :DTWAIN_GetPixelType
   attr_reader :DTWAIN_GetPrinter
   attr_reader :DTWAIN_GetPrinterStartNumber
   attr_reader :DTWAIN_GetPrinterStringMode
   attr_reader :DTWAIN_GetPrinterStrings
   attr_reader :DTWAIN_GetPrinterSuffixString
   attr_reader :DTWAIN_GetPrinterSuffixStringA
   attr_reader :DTWAIN_GetPrinterSuffixStringW
   attr_reader :DTWAIN_GetRegisteredMsg
   attr_reader :DTWAIN_GetResolution
   attr_reader :DTWAIN_GetResolutionString
   attr_reader :DTWAIN_GetResolutionStringA
   attr_reader :DTWAIN_GetResolutionStringW
   attr_reader :DTWAIN_GetResourceString
   attr_reader :DTWAIN_GetResourceStringA
   attr_reader :DTWAIN_GetResourceStringW
   attr_reader :DTWAIN_GetRotation
   attr_reader :DTWAIN_GetRotationString
   attr_reader :DTWAIN_GetRotationStringA
   attr_reader :DTWAIN_GetRotationStringW
   attr_reader :DTWAIN_GetSaveFileName
   attr_reader :DTWAIN_GetSaveFileNameA
   attr_reader :DTWAIN_GetSaveFileNameW
   attr_reader :DTWAIN_GetSessionDetails
   attr_reader :DTWAIN_GetSessionDetailsA
   attr_reader :DTWAIN_GetSessionDetailsW
   attr_reader :DTWAIN_GetShadow
   attr_reader :DTWAIN_GetShadowString
   attr_reader :DTWAIN_GetShadowStringA
   attr_reader :DTWAIN_GetShadowStringW
   attr_reader :DTWAIN_GetShortVersionString
   attr_reader :DTWAIN_GetShortVersionStringA
   attr_reader :DTWAIN_GetShortVersionStringW
   attr_reader :DTWAIN_GetSourceAcquisitions
   attr_reader :DTWAIN_GetSourceDetails
   attr_reader :DTWAIN_GetSourceDetailsA
   attr_reader :DTWAIN_GetSourceDetailsW
   attr_reader :DTWAIN_GetSourceID
   attr_reader :DTWAIN_GetSourceIDEx
   attr_reader :DTWAIN_GetSourceManufacturer
   attr_reader :DTWAIN_GetSourceManufacturerA
   attr_reader :DTWAIN_GetSourceManufacturerW
   attr_reader :DTWAIN_GetSourceProductFamily
   attr_reader :DTWAIN_GetSourceProductFamilyA
   attr_reader :DTWAIN_GetSourceProductFamilyW
   attr_reader :DTWAIN_GetSourceProductName
   attr_reader :DTWAIN_GetSourceProductNameA
   attr_reader :DTWAIN_GetSourceProductNameW
   attr_reader :DTWAIN_GetSourceUnit
   attr_reader :DTWAIN_GetSourceVersionInfo
   attr_reader :DTWAIN_GetSourceVersionInfoA
   attr_reader :DTWAIN_GetSourceVersionInfoW
   attr_reader :DTWAIN_GetSourceVersionNumber
   attr_reader :DTWAIN_GetStaticLibVersion
   attr_reader :DTWAIN_GetTempFileDirectory
   attr_reader :DTWAIN_GetTempFileDirectoryA
   attr_reader :DTWAIN_GetTempFileDirectoryW
   attr_reader :DTWAIN_GetThreshold
   attr_reader :DTWAIN_GetThresholdString
   attr_reader :DTWAIN_GetThresholdStringA
   attr_reader :DTWAIN_GetThresholdStringW
   attr_reader :DTWAIN_GetTimeDate
   attr_reader :DTWAIN_GetTimeDateA
   attr_reader :DTWAIN_GetTimeDateW
   attr_reader :DTWAIN_GetTwainAppID
   attr_reader :DTWAIN_GetTwainAppIDEx
   attr_reader :DTWAIN_GetTwainAvailability
   attr_reader :DTWAIN_GetTwainAvailabilityEx
   attr_reader :DTWAIN_GetTwainAvailabilityExA
   attr_reader :DTWAIN_GetTwainAvailabilityExW
   attr_reader :DTWAIN_GetTwainCountryName
   attr_reader :DTWAIN_GetTwainCountryNameA
   attr_reader :DTWAIN_GetTwainCountryNameW
   attr_reader :DTWAIN_GetTwainCountryValue
   attr_reader :DTWAIN_GetTwainCountryValueA
   attr_reader :DTWAIN_GetTwainCountryValueW
   attr_reader :DTWAIN_GetTwainHwnd
   attr_reader :DTWAIN_GetTwainIDFromName
   attr_reader :DTWAIN_GetTwainIDFromNameA
   attr_reader :DTWAIN_GetTwainIDFromNameW
   attr_reader :DTWAIN_GetTwainLanguageName
   attr_reader :DTWAIN_GetTwainLanguageNameA
   attr_reader :DTWAIN_GetTwainLanguageNameW
   attr_reader :DTWAIN_GetTwainLanguageValue
   attr_reader :DTWAIN_GetTwainLanguageValueA
   attr_reader :DTWAIN_GetTwainLanguageValueW
   attr_reader :DTWAIN_GetTwainMode
   attr_reader :DTWAIN_GetTwainNameFromConstant
   attr_reader :DTWAIN_GetTwainNameFromConstantA
   attr_reader :DTWAIN_GetTwainNameFromConstantW
   attr_reader :DTWAIN_GetTwainStringName
   attr_reader :DTWAIN_GetTwainStringNameA
   attr_reader :DTWAIN_GetTwainStringNameW
   attr_reader :DTWAIN_GetTwainTimeout
   attr_reader :DTWAIN_GetVersion
   attr_reader :DTWAIN_GetVersionCopyright
   attr_reader :DTWAIN_GetVersionCopyrightA
   attr_reader :DTWAIN_GetVersionCopyrightW
   attr_reader :DTWAIN_GetVersionEx
   attr_reader :DTWAIN_GetVersionInfo
   attr_reader :DTWAIN_GetVersionInfoA
   attr_reader :DTWAIN_GetVersionInfoW
   attr_reader :DTWAIN_GetVersionString
   attr_reader :DTWAIN_GetVersionStringA
   attr_reader :DTWAIN_GetVersionStringW
   attr_reader :DTWAIN_GetWindowsVersionInfo
   attr_reader :DTWAIN_GetWindowsVersionInfoA
   attr_reader :DTWAIN_GetWindowsVersionInfoW
   attr_reader :DTWAIN_GetXResolution
   attr_reader :DTWAIN_GetXResolutionString
   attr_reader :DTWAIN_GetXResolutionStringA
   attr_reader :DTWAIN_GetXResolutionStringW
   attr_reader :DTWAIN_GetYResolution
   attr_reader :DTWAIN_GetYResolutionString
   attr_reader :DTWAIN_GetYResolutionStringA
   attr_reader :DTWAIN_GetYResolutionStringW
   attr_reader :DTWAIN_InitExtImageInfo
   attr_reader :DTWAIN_InitImageFileAppend
   attr_reader :DTWAIN_InitImageFileAppendA
   attr_reader :DTWAIN_InitImageFileAppendW
   attr_reader :DTWAIN_InitOCRInterface
   attr_reader :DTWAIN_IsAcquiring
   attr_reader :DTWAIN_IsAudioXferSupported
   attr_reader :DTWAIN_IsAutoBorderDetectEnabled
   attr_reader :DTWAIN_IsAutoBorderDetectSupported
   attr_reader :DTWAIN_IsAutoBrightEnabled
   attr_reader :DTWAIN_IsAutoBrightSupported
   attr_reader :DTWAIN_IsAutoDeskewEnabled
   attr_reader :DTWAIN_IsAutoDeskewSupported
   attr_reader :DTWAIN_IsAutoFeedEnabled
   attr_reader :DTWAIN_IsAutoFeedSupported
   attr_reader :DTWAIN_IsAutoRotateEnabled
   attr_reader :DTWAIN_IsAutoRotateSupported
   attr_reader :DTWAIN_IsAutoScanEnabled
   attr_reader :DTWAIN_IsAutomaticSenseMediumEnabled
   attr_reader :DTWAIN_IsAutomaticSenseMediumSupported
   attr_reader :DTWAIN_IsBlankPageDetectionOn
   attr_reader :DTWAIN_IsBufferedTileModeOn
   attr_reader :DTWAIN_IsBufferedTileModeSupported
   attr_reader :DTWAIN_IsCapSupported
   attr_reader :DTWAIN_IsCompressionSupported
   attr_reader :DTWAIN_IsCustomDSDataSupported
   attr_reader :DTWAIN_IsDIBBlank
   attr_reader :DTWAIN_IsDIBBlankString
   attr_reader :DTWAIN_IsDIBBlankStringA
   attr_reader :DTWAIN_IsDIBBlankStringW
   attr_reader :DTWAIN_IsDeviceEventSupported
   attr_reader :DTWAIN_IsDeviceOnLine
   attr_reader :DTWAIN_IsDoubleFeedDetectLengthSupported
   attr_reader :DTWAIN_IsDoubleFeedDetectSupported
   attr_reader :DTWAIN_IsDuplexEnabled
   attr_reader :DTWAIN_IsDuplexSupported
   attr_reader :DTWAIN_IsExtImageInfoSupported
   attr_reader :DTWAIN_IsFeederEnabled
   attr_reader :DTWAIN_IsFeederLoaded
   attr_reader :DTWAIN_IsFeederSensitive
   attr_reader :DTWAIN_IsFeederSupported
   attr_reader :DTWAIN_IsFileSystemSupported
   attr_reader :DTWAIN_IsFileXferSupported
   attr_reader :DTWAIN_IsIAFieldALastPageSupported
   attr_reader :DTWAIN_IsIAFieldALevelSupported
   attr_reader :DTWAIN_IsIAFieldAPrintFormatSupported
   attr_reader :DTWAIN_IsIAFieldAValueSupported
   attr_reader :DTWAIN_IsIAFieldBLastPageSupported
   attr_reader :DTWAIN_IsIAFieldBLevelSupported
   attr_reader :DTWAIN_IsIAFieldBPrintFormatSupported
   attr_reader :DTWAIN_IsIAFieldBValueSupported
   attr_reader :DTWAIN_IsIAFieldCLastPageSupported
   attr_reader :DTWAIN_IsIAFieldCLevelSupported
   attr_reader :DTWAIN_IsIAFieldCPrintFormatSupported
   attr_reader :DTWAIN_IsIAFieldCValueSupported
   attr_reader :DTWAIN_IsIAFieldDLastPageSupported
   attr_reader :DTWAIN_IsIAFieldDLevelSupported
   attr_reader :DTWAIN_IsIAFieldDPrintFormatSupported
   attr_reader :DTWAIN_IsIAFieldDValueSupported
   attr_reader :DTWAIN_IsIAFieldELastPageSupported
   attr_reader :DTWAIN_IsIAFieldELevelSupported
   attr_reader :DTWAIN_IsIAFieldEPrintFormatSupported
   attr_reader :DTWAIN_IsIAFieldEValueSupported
   attr_reader :DTWAIN_IsImageAddressingSupported
   attr_reader :DTWAIN_IsIndicatorEnabled
   attr_reader :DTWAIN_IsIndicatorSupported
   attr_reader :DTWAIN_IsInitialized
   attr_reader :DTWAIN_IsJPEGSupported
   attr_reader :DTWAIN_IsJobControlSupported
   attr_reader :DTWAIN_IsLampEnabled
   attr_reader :DTWAIN_IsLampSupported
   attr_reader :DTWAIN_IsLightPathSupported
   attr_reader :DTWAIN_IsLightSourceSupported
   attr_reader :DTWAIN_IsMaxBuffersSupported
   attr_reader :DTWAIN_IsMemFileXferSupported
   attr_reader :DTWAIN_IsMsgNotifyEnabled
   attr_reader :DTWAIN_IsNotifyTripletsEnabled
   attr_reader :DTWAIN_IsOCREngineActivated
   attr_reader :DTWAIN_IsOpenSourcesOnSelect
   attr_reader :DTWAIN_IsOrientationSupported
   attr_reader :DTWAIN_IsOverscanSupported
   attr_reader :DTWAIN_IsPDFSupported
   attr_reader :DTWAIN_IsPNGSupported
   attr_reader :DTWAIN_IsPaperDetectable
   attr_reader :DTWAIN_IsPaperSizeSupported
   attr_reader :DTWAIN_IsPatchCapsSupported
   attr_reader :DTWAIN_IsPatchDetectEnabled
   attr_reader :DTWAIN_IsPatchSupported
   attr_reader :DTWAIN_IsPeekMessageLoopEnabled
   attr_reader :DTWAIN_IsPixelTypeSupported
   attr_reader :DTWAIN_IsPrinterEnabled
   attr_reader :DTWAIN_IsPrinterSupported
   attr_reader :DTWAIN_IsRotationSupported
   attr_reader :DTWAIN_IsSessionEnabled
   attr_reader :DTWAIN_IsSkipImageInfoError
   attr_reader :DTWAIN_IsSourceAcquiring
   attr_reader :DTWAIN_IsSourceAcquiringEx
   attr_reader :DTWAIN_IsSourceInUIOnlyMode
   attr_reader :DTWAIN_IsSourceOpen
   attr_reader :DTWAIN_IsSourceSelected
   attr_reader :DTWAIN_IsSourceValid
   attr_reader :DTWAIN_IsTIFFSupported
   attr_reader :DTWAIN_IsThumbnailEnabled
   attr_reader :DTWAIN_IsThumbnailSupported
   attr_reader :DTWAIN_IsTwainAvailable
   attr_reader :DTWAIN_IsTwainAvailableEx
   attr_reader :DTWAIN_IsTwainAvailableExA
   attr_reader :DTWAIN_IsTwainAvailableExW
   attr_reader :DTWAIN_IsUIControllable
   attr_reader :DTWAIN_IsUIEnabled
   attr_reader :DTWAIN_IsUIOnlySupported
   attr_reader :DTWAIN_LoadCustomStringResources
   attr_reader :DTWAIN_LoadCustomStringResourcesA
   attr_reader :DTWAIN_LoadCustomStringResourcesEx
   attr_reader :DTWAIN_LoadCustomStringResourcesExA
   attr_reader :DTWAIN_LoadCustomStringResourcesExW
   attr_reader :DTWAIN_LoadCustomStringResourcesW
   attr_reader :DTWAIN_LoadLanguageResource
   attr_reader :DTWAIN_LockMemory
   attr_reader :DTWAIN_LockMemoryEx
   attr_reader :DTWAIN_LogMessage
   attr_reader :DTWAIN_LogMessageA
   attr_reader :DTWAIN_LogMessageW
   attr_reader :DTWAIN_MakeRGB
   attr_reader :DTWAIN_OpenSource
   attr_reader :DTWAIN_OpenSourcesOnSelect
   attr_reader :DTWAIN_RangeCreate
   attr_reader :DTWAIN_RangeCreateFromCap
   attr_reader :DTWAIN_RangeDestroy
   attr_reader :DTWAIN_RangeExpand
   attr_reader :DTWAIN_RangeExpandEx
   attr_reader :DTWAIN_RangeGetAll
   attr_reader :DTWAIN_RangeGetAllFloat
   attr_reader :DTWAIN_RangeGetAllFloatString
   attr_reader :DTWAIN_RangeGetAllFloatStringA
   attr_reader :DTWAIN_RangeGetAllFloatStringW
   attr_reader :DTWAIN_RangeGetAllLong
   attr_reader :DTWAIN_RangeGetCount
   attr_reader :DTWAIN_RangeGetExpValue
   attr_reader :DTWAIN_RangeGetExpValueFloat
   attr_reader :DTWAIN_RangeGetExpValueFloatString
   attr_reader :DTWAIN_RangeGetExpValueFloatStringA
   attr_reader :DTWAIN_RangeGetExpValueFloatStringW
   attr_reader :DTWAIN_RangeGetExpValueLong
   attr_reader :DTWAIN_RangeGetNearestValue
   attr_reader :DTWAIN_RangeGetPos
   attr_reader :DTWAIN_RangeGetPosFloat
   attr_reader :DTWAIN_RangeGetPosFloatString
   attr_reader :DTWAIN_RangeGetPosFloatStringA
   attr_reader :DTWAIN_RangeGetPosFloatStringW
   attr_reader :DTWAIN_RangeGetPosLong
   attr_reader :DTWAIN_RangeGetValue
   attr_reader :DTWAIN_RangeGetValueFloat
   attr_reader :DTWAIN_RangeGetValueFloatString
   attr_reader :DTWAIN_RangeGetValueFloatStringA
   attr_reader :DTWAIN_RangeGetValueFloatStringW
   attr_reader :DTWAIN_RangeGetValueLong
   attr_reader :DTWAIN_RangeIsValid
   attr_reader :DTWAIN_RangeNearestValueFloat
   attr_reader :DTWAIN_RangeNearestValueFloatString
   attr_reader :DTWAIN_RangeNearestValueFloatStringA
   attr_reader :DTWAIN_RangeNearestValueFloatStringW
   attr_reader :DTWAIN_RangeNearestValueLong
   attr_reader :DTWAIN_RangeSetAll
   attr_reader :DTWAIN_RangeSetAllFloat
   attr_reader :DTWAIN_RangeSetAllFloatString
   attr_reader :DTWAIN_RangeSetAllFloatStringA
   attr_reader :DTWAIN_RangeSetAllFloatStringW
   attr_reader :DTWAIN_RangeSetAllLong
   attr_reader :DTWAIN_RangeSetValue
   attr_reader :DTWAIN_RangeSetValueFloat
   attr_reader :DTWAIN_RangeSetValueFloatString
   attr_reader :DTWAIN_RangeSetValueFloatStringA
   attr_reader :DTWAIN_RangeSetValueFloatStringW
   attr_reader :DTWAIN_RangeSetValueLong
   attr_reader :DTWAIN_ResetPDFTextElement
   attr_reader :DTWAIN_RewindPage
   attr_reader :DTWAIN_SelectDefaultOCREngine
   attr_reader :DTWAIN_SelectDefaultSource
   attr_reader :DTWAIN_SelectDefaultSourceWithOpen
   attr_reader :DTWAIN_SelectOCREngine
   attr_reader :DTWAIN_SelectOCREngine2
   attr_reader :DTWAIN_SelectOCREngine2A
   attr_reader :DTWAIN_SelectOCREngine2Ex
   attr_reader :DTWAIN_SelectOCREngine2ExA
   attr_reader :DTWAIN_SelectOCREngine2ExW
   attr_reader :DTWAIN_SelectOCREngine2W
   attr_reader :DTWAIN_SelectOCREngineByName
   attr_reader :DTWAIN_SelectOCREngineByNameA
   attr_reader :DTWAIN_SelectOCREngineByNameW
   attr_reader :DTWAIN_SelectSource
   attr_reader :DTWAIN_SelectSource2
   attr_reader :DTWAIN_SelectSource2A
   attr_reader :DTWAIN_SelectSource2Ex
   attr_reader :DTWAIN_SelectSource2ExA
   attr_reader :DTWAIN_SelectSource2ExW
   attr_reader :DTWAIN_SelectSource2W
   attr_reader :DTWAIN_SelectSourceByName
   attr_reader :DTWAIN_SelectSourceByNameA
   attr_reader :DTWAIN_SelectSourceByNameW
   attr_reader :DTWAIN_SelectSourceByNameWithOpen
   attr_reader :DTWAIN_SelectSourceByNameWithOpenA
   attr_reader :DTWAIN_SelectSourceByNameWithOpenW
   attr_reader :DTWAIN_SelectSourceWithOpen
   attr_reader :DTWAIN_SetAcquireArea
   attr_reader :DTWAIN_SetAcquireArea2
   attr_reader :DTWAIN_SetAcquireArea2String
   attr_reader :DTWAIN_SetAcquireArea2StringA
   attr_reader :DTWAIN_SetAcquireArea2StringW
   attr_reader :DTWAIN_SetAcquireImageNegative
   attr_reader :DTWAIN_SetAcquireImageScale
   attr_reader :DTWAIN_SetAcquireImageScaleString
   attr_reader :DTWAIN_SetAcquireImageScaleStringA
   attr_reader :DTWAIN_SetAcquireImageScaleStringW
   attr_reader :DTWAIN_SetAcquireStripBuffer
   attr_reader :DTWAIN_SetAcquireStripSize
   attr_reader :DTWAIN_SetAlarmVolume
   attr_reader :DTWAIN_SetAlarms
   attr_reader :DTWAIN_SetAllCapsToDefault
   attr_reader :DTWAIN_SetAppInfo
   attr_reader :DTWAIN_SetAppInfoA
   attr_reader :DTWAIN_SetAppInfoW
   attr_reader :DTWAIN_SetAuthor
   attr_reader :DTWAIN_SetAuthorA
   attr_reader :DTWAIN_SetAuthorW
   attr_reader :DTWAIN_SetAvailablePrinters
   attr_reader :DTWAIN_SetAvailablePrintersArray
   attr_reader :DTWAIN_SetBitDepth
   attr_reader :DTWAIN_SetBlankPageDetection
   attr_reader :DTWAIN_SetBlankPageDetectionEx
   attr_reader :DTWAIN_SetBlankPageDetectionExString
   attr_reader :DTWAIN_SetBlankPageDetectionExStringA
   attr_reader :DTWAIN_SetBlankPageDetectionExStringW
   attr_reader :DTWAIN_SetBlankPageDetectionString
   attr_reader :DTWAIN_SetBlankPageDetectionStringA
   attr_reader :DTWAIN_SetBlankPageDetectionStringW
   attr_reader :DTWAIN_SetBrightness
   attr_reader :DTWAIN_SetBrightnessString
   attr_reader :DTWAIN_SetBrightnessStringA
   attr_reader :DTWAIN_SetBrightnessStringW
   attr_reader :DTWAIN_SetBufferedTileMode
   attr_reader :DTWAIN_SetCallback
   attr_reader :DTWAIN_SetCallback64
   attr_reader :DTWAIN_SetCamera
   attr_reader :DTWAIN_SetCameraA
   attr_reader :DTWAIN_SetCameraW
   attr_reader :DTWAIN_SetCapValues
   attr_reader :DTWAIN_SetCapValuesEx
   attr_reader :DTWAIN_SetCapValuesEx2
   attr_reader :DTWAIN_SetCaption
   attr_reader :DTWAIN_SetCaptionA
   attr_reader :DTWAIN_SetCaptionW
   attr_reader :DTWAIN_SetCompressionType
   attr_reader :DTWAIN_SetContrast
   attr_reader :DTWAIN_SetContrastString
   attr_reader :DTWAIN_SetContrastStringA
   attr_reader :DTWAIN_SetContrastStringW
   attr_reader :DTWAIN_SetCountry
   attr_reader :DTWAIN_SetCurrentRetryCount
   attr_reader :DTWAIN_SetCustomDSData
   attr_reader :DTWAIN_SetDSMSearchOrder
   attr_reader :DTWAIN_SetDSMSearchOrderEx
   attr_reader :DTWAIN_SetDSMSearchOrderExA
   attr_reader :DTWAIN_SetDSMSearchOrderExW
   attr_reader :DTWAIN_SetDefaultSource
   attr_reader :DTWAIN_SetDeviceNotifications
   attr_reader :DTWAIN_SetDeviceTimeDate
   attr_reader :DTWAIN_SetDeviceTimeDateA
   attr_reader :DTWAIN_SetDeviceTimeDateW
   attr_reader :DTWAIN_SetDoubleFeedDetectLength
   attr_reader :DTWAIN_SetDoubleFeedDetectLengthString
   attr_reader :DTWAIN_SetDoubleFeedDetectLengthStringA
   attr_reader :DTWAIN_SetDoubleFeedDetectLengthStringW
   attr_reader :DTWAIN_SetDoubleFeedDetectValues
   attr_reader :DTWAIN_SetDoublePageCountOnDuplex
   attr_reader :DTWAIN_SetEOJDetectValue
   attr_reader :DTWAIN_SetErrorBufferThreshold
   attr_reader :DTWAIN_SetErrorCallback
   attr_reader :DTWAIN_SetErrorCallback64
   attr_reader :DTWAIN_SetFeederAlignment
   attr_reader :DTWAIN_SetFeederOrder
   attr_reader :DTWAIN_SetFeederWaitTime
   attr_reader :DTWAIN_SetFileAutoIncrement
   attr_reader :DTWAIN_SetFileCompressionType
   attr_reader :DTWAIN_SetFileSavePos
   attr_reader :DTWAIN_SetFileSavePosA
   attr_reader :DTWAIN_SetFileSavePosW
   attr_reader :DTWAIN_SetFileXferFormat
   attr_reader :DTWAIN_SetHalftone
   attr_reader :DTWAIN_SetHalftoneA
   attr_reader :DTWAIN_SetHalftoneW
   attr_reader :DTWAIN_SetHighlight
   attr_reader :DTWAIN_SetHighlightString
   attr_reader :DTWAIN_SetHighlightStringA
   attr_reader :DTWAIN_SetHighlightStringW
   attr_reader :DTWAIN_SetJobControl
   attr_reader :DTWAIN_SetJpegValues
   attr_reader :DTWAIN_SetJpegXRValues
   attr_reader :DTWAIN_SetLanguage
   attr_reader :DTWAIN_SetLastError
   attr_reader :DTWAIN_SetLightPath
   attr_reader :DTWAIN_SetLightPathEx
   attr_reader :DTWAIN_SetLightSource
   attr_reader :DTWAIN_SetLightSources
   attr_reader :DTWAIN_SetLoggerCallback
   attr_reader :DTWAIN_SetLoggerCallbackA
   attr_reader :DTWAIN_SetLoggerCallbackW
   attr_reader :DTWAIN_SetManualDuplexMode
   attr_reader :DTWAIN_SetMaxAcquisitions
   attr_reader :DTWAIN_SetMaxBuffers
   attr_reader :DTWAIN_SetMaxRetryAttempts
   attr_reader :DTWAIN_SetMultipageScanMode
   attr_reader :DTWAIN_SetNoiseFilter
   attr_reader :DTWAIN_SetOCRCapValues
   attr_reader :DTWAIN_SetOrientation
   attr_reader :DTWAIN_SetOverscan
   attr_reader :DTWAIN_SetPDFAESEncryption
   attr_reader :DTWAIN_SetPDFASCIICompression
   attr_reader :DTWAIN_SetPDFAuthor
   attr_reader :DTWAIN_SetPDFAuthorA
   attr_reader :DTWAIN_SetPDFAuthorW
   attr_reader :DTWAIN_SetPDFCompression
   attr_reader :DTWAIN_SetPDFCreator
   attr_reader :DTWAIN_SetPDFCreatorA
   attr_reader :DTWAIN_SetPDFCreatorW
   attr_reader :DTWAIN_SetPDFEncryption
   attr_reader :DTWAIN_SetPDFEncryptionA
   attr_reader :DTWAIN_SetPDFEncryptionW
   attr_reader :DTWAIN_SetPDFJpegQuality
   attr_reader :DTWAIN_SetPDFKeywords
   attr_reader :DTWAIN_SetPDFKeywordsA
   attr_reader :DTWAIN_SetPDFKeywordsW
   attr_reader :DTWAIN_SetPDFOCRConversion
   attr_reader :DTWAIN_SetPDFOCRMode
   attr_reader :DTWAIN_SetPDFOrientation
   attr_reader :DTWAIN_SetPDFPageScale
   attr_reader :DTWAIN_SetPDFPageScaleString
   attr_reader :DTWAIN_SetPDFPageScaleStringA
   attr_reader :DTWAIN_SetPDFPageScaleStringW
   attr_reader :DTWAIN_SetPDFPageSize
   attr_reader :DTWAIN_SetPDFPageSizeString
   attr_reader :DTWAIN_SetPDFPageSizeStringA
   attr_reader :DTWAIN_SetPDFPageSizeStringW
   attr_reader :DTWAIN_SetPDFPolarity
   attr_reader :DTWAIN_SetPDFProducer
   attr_reader :DTWAIN_SetPDFProducerA
   attr_reader :DTWAIN_SetPDFProducerW
   attr_reader :DTWAIN_SetPDFSubject
   attr_reader :DTWAIN_SetPDFSubjectA
   attr_reader :DTWAIN_SetPDFSubjectW
   attr_reader :DTWAIN_SetPDFTextElementFloat
   attr_reader :DTWAIN_SetPDFTextElementLong
   attr_reader :DTWAIN_SetPDFTextElementString
   attr_reader :DTWAIN_SetPDFTextElementStringA
   attr_reader :DTWAIN_SetPDFTextElementStringW
   attr_reader :DTWAIN_SetPDFTitle
   attr_reader :DTWAIN_SetPDFTitleA
   attr_reader :DTWAIN_SetPDFTitleW
   attr_reader :DTWAIN_SetPaperSize
   attr_reader :DTWAIN_SetPatchMaxPriorities
   attr_reader :DTWAIN_SetPatchMaxRetries
   attr_reader :DTWAIN_SetPatchPriorities
   attr_reader :DTWAIN_SetPatchSearchMode
   attr_reader :DTWAIN_SetPatchTimeOut
   attr_reader :DTWAIN_SetPixelFlavor
   attr_reader :DTWAIN_SetPixelType
   attr_reader :DTWAIN_SetPostScriptTitle
   attr_reader :DTWAIN_SetPostScriptTitleA
   attr_reader :DTWAIN_SetPostScriptTitleW
   attr_reader :DTWAIN_SetPostScriptType
   attr_reader :DTWAIN_SetPrinter
   attr_reader :DTWAIN_SetPrinterEx
   attr_reader :DTWAIN_SetPrinterStartNumber
   attr_reader :DTWAIN_SetPrinterStringMode
   attr_reader :DTWAIN_SetPrinterStrings
   attr_reader :DTWAIN_SetPrinterSuffixString
   attr_reader :DTWAIN_SetPrinterSuffixStringA
   attr_reader :DTWAIN_SetPrinterSuffixStringW
   attr_reader :DTWAIN_SetQueryCapSupport
   attr_reader :DTWAIN_SetResolution
   attr_reader :DTWAIN_SetResolutionString
   attr_reader :DTWAIN_SetResolutionStringA
   attr_reader :DTWAIN_SetResolutionStringW
   attr_reader :DTWAIN_SetResourcePath
   attr_reader :DTWAIN_SetResourcePathA
   attr_reader :DTWAIN_SetResourcePathW
   attr_reader :DTWAIN_SetRotation
   attr_reader :DTWAIN_SetRotationString
   attr_reader :DTWAIN_SetRotationStringA
   attr_reader :DTWAIN_SetRotationStringW
   attr_reader :DTWAIN_SetSaveFileName
   attr_reader :DTWAIN_SetSaveFileNameA
   attr_reader :DTWAIN_SetSaveFileNameW
   attr_reader :DTWAIN_SetShadow
   attr_reader :DTWAIN_SetShadowString
   attr_reader :DTWAIN_SetShadowStringA
   attr_reader :DTWAIN_SetShadowStringW
   attr_reader :DTWAIN_SetSourceUnit
   attr_reader :DTWAIN_SetTIFFCompressType
   attr_reader :DTWAIN_SetTIFFInvert
   attr_reader :DTWAIN_SetTempFileDirectory
   attr_reader :DTWAIN_SetTempFileDirectoryA
   attr_reader :DTWAIN_SetTempFileDirectoryEx
   attr_reader :DTWAIN_SetTempFileDirectoryExA
   attr_reader :DTWAIN_SetTempFileDirectoryExW
   attr_reader :DTWAIN_SetTempFileDirectoryW
   attr_reader :DTWAIN_SetThreshold
   attr_reader :DTWAIN_SetThresholdString
   attr_reader :DTWAIN_SetThresholdStringA
   attr_reader :DTWAIN_SetThresholdStringW
   attr_reader :DTWAIN_SetTwainDSM
   attr_reader :DTWAIN_SetTwainLog
   attr_reader :DTWAIN_SetTwainLogA
   attr_reader :DTWAIN_SetTwainLogW
   attr_reader :DTWAIN_SetTwainMode
   attr_reader :DTWAIN_SetTwainTimeout
   attr_reader :DTWAIN_SetUpdateDibProc
   attr_reader :DTWAIN_SetXResolution
   attr_reader :DTWAIN_SetXResolutionString
   attr_reader :DTWAIN_SetXResolutionStringA
   attr_reader :DTWAIN_SetXResolutionStringW
   attr_reader :DTWAIN_SetYResolution
   attr_reader :DTWAIN_SetYResolutionString
   attr_reader :DTWAIN_SetYResolutionStringA
   attr_reader :DTWAIN_SetYResolutionStringW
   attr_reader :DTWAIN_ShowUIOnly
   attr_reader :DTWAIN_ShutdownOCREngine
   attr_reader :DTWAIN_SkipImageInfoError
   attr_reader :DTWAIN_StartThread
   attr_reader :DTWAIN_StartTwainSession
   attr_reader :DTWAIN_StartTwainSessionA
   attr_reader :DTWAIN_StartTwainSessionW
   attr_reader :DTWAIN_SysDestroy
   attr_reader :DTWAIN_SysInitialize
   attr_reader :DTWAIN_SysInitializeEx
   attr_reader :DTWAIN_SysInitializeEx2
   attr_reader :DTWAIN_SysInitializeEx2A
   attr_reader :DTWAIN_SysInitializeEx2W
   attr_reader :DTWAIN_SysInitializeExA
   attr_reader :DTWAIN_SysInitializeExW
   attr_reader :DTWAIN_SysInitializeLib
   attr_reader :DTWAIN_SysInitializeLibEx
   attr_reader :DTWAIN_SysInitializeLibEx2
   attr_reader :DTWAIN_SysInitializeLibEx2A
   attr_reader :DTWAIN_SysInitializeLibEx2W
   attr_reader :DTWAIN_SysInitializeLibExA
   attr_reader :DTWAIN_SysInitializeLibExW
   attr_reader :DTWAIN_SysInitializeNoBlocking
   attr_reader :DTWAIN_TestGetCap
   attr_reader :DTWAIN_UnlockMemory
   attr_reader :DTWAIN_UnlockMemoryEx
   attr_reader :DTWAIN_UseMultipleThreads
   DTWAIN_FF_TIFF = 0
   DTWAIN_FF_PICT = 1
   DTWAIN_FF_BMP = 2
   DTWAIN_FF_XBM = 3
   DTWAIN_FF_JFIF = 4
   DTWAIN_FF_FPX = 5
   DTWAIN_FF_TIFFMULTI = 6
   DTWAIN_FF_PNG = 7
   DTWAIN_FF_SPIFF = 8
   DTWAIN_FF_EXIF = 9
   DTWAIN_FF_PDF = 10
   DTWAIN_FF_JP2 = 11
   DTWAIN_FF_JPX = 13
   DTWAIN_FF_DEJAVU = 14
   DTWAIN_FF_PDFA = 15
   DTWAIN_FF_PDFA2 = 16
   DTWAIN_FF_PDFRASTER = 17
   DTWAIN_CP_NONE = 0
   DTWAIN_CP_PACKBITS = 1
   DTWAIN_CP_GROUP31D = 2
   DTWAIN_CP_GROUP31DEOL = 3
   DTWAIN_CP_GROUP32D = 4
   DTWAIN_CP_GROUP4 = 5
   DTWAIN_CP_JPEG = 6
   DTWAIN_CP_LZW = 7
   DTWAIN_CP_JBIG = 8
   DTWAIN_CP_PNG = 9
   DTWAIN_CP_RLE4 = 10
   DTWAIN_CP_RLE8 = 11
   DTWAIN_CP_BITFIELDS = 12
   DTWAIN_CP_ZIP = 13
   DTWAIN_CP_JPEG2000 = 14
   DTWAIN_FS_NONE = 0
   DTWAIN_FS_A4LETTER = 1
   DTWAIN_FS_B5LETTER = 2
   DTWAIN_FS_USLETTER = 3
   DTWAIN_FS_USLEGAL = 4
   DTWAIN_FS_A5 = 5
   DTWAIN_FS_B4 = 6
   DTWAIN_FS_B6 = 7
   DTWAIN_FS_USLEDGER = 9
   DTWAIN_FS_USEXECUTIVE = 10
   DTWAIN_FS_A3 = 11
   DTWAIN_FS_B3 = 12
   DTWAIN_FS_A6 = 13
   DTWAIN_FS_C4 = 14
   DTWAIN_FS_C5 = 15
   DTWAIN_FS_C6 = 16
   DTWAIN_FS_4A0 = 17
   DTWAIN_FS_2A0 = 18
   DTWAIN_FS_A0 = 19
   DTWAIN_FS_A1 = 20
   DTWAIN_FS_A2 = 21
   DTWAIN_FS_A4 = DTWAIN_FS_A4LETTER
   DTWAIN_FS_A7 = 22
   DTWAIN_FS_A8 = 23
   DTWAIN_FS_A9 = 24
   DTWAIN_FS_A10 = 25
   DTWAIN_FS_ISOB0 = 26
   DTWAIN_FS_ISOB1 = 27
   DTWAIN_FS_ISOB2 = 28
   DTWAIN_FS_ISOB3 = DTWAIN_FS_B3
   DTWAIN_FS_ISOB4 = DTWAIN_FS_B4
   DTWAIN_FS_ISOB5 = 29
   DTWAIN_FS_ISOB6 = DTWAIN_FS_B6
   DTWAIN_FS_ISOB7 = 30
   DTWAIN_FS_ISOB8 = 31
   DTWAIN_FS_ISOB9 = 32
   DTWAIN_FS_ISOB10 = 33
   DTWAIN_FS_JISB0 = 34
   DTWAIN_FS_JISB1 = 35
   DTWAIN_FS_JISB2 = 36
   DTWAIN_FS_JISB3 = 37
   DTWAIN_FS_JISB4 = 38
   DTWAIN_FS_JISB5 = DTWAIN_FS_B5LETTER
   DTWAIN_FS_JISB6 = 39
   DTWAIN_FS_JISB7 = 40
   DTWAIN_FS_JISB8 = 41
   DTWAIN_FS_JISB9 = 42
   DTWAIN_FS_JISB10 = 43
   DTWAIN_FS_C0 = 44
   DTWAIN_FS_C1 = 45
   DTWAIN_FS_C2 = 46
   DTWAIN_FS_C3 = 47
   DTWAIN_FS_C7 = 48
   DTWAIN_FS_C8 = 49
   DTWAIN_FS_C9 = 50
   DTWAIN_FS_C10 = 51
   DTWAIN_FS_USSTATEMENT = 52
   DTWAIN_FS_BUSINESSCARD = 53
   DTWAIN_ANYSUPPORT = (-1)
   DTWAIN_BMP = 100
   DTWAIN_JPEG = 200
   DTWAIN_PDF = 250
   DTWAIN_PDFMULTI = 251
   DTWAIN_PCX = 300
   DTWAIN_DCX = 301
   DTWAIN_TGA = 400
   DTWAIN_TIFFLZW = 500
   DTWAIN_TIFFNONE = 600
   DTWAIN_TIFFG3 = 700
   DTWAIN_TIFFG4 = 800
   DTWAIN_TIFFPACKBITS = 801
   DTWAIN_TIFFDEFLATE = 802
   DTWAIN_TIFFJPEG = 803
   DTWAIN_TIFFJBIG = 804
   DTWAIN_TIFFPIXARLOG = 805
   DTWAIN_TIFFNONEMULTI = 900
   DTWAIN_TIFFG3MULTI = 901
   DTWAIN_TIFFG4MULTI = 902
   DTWAIN_TIFFPACKBITSMULTI = 903
   DTWAIN_TIFFDEFLATEMULTI = 904
   DTWAIN_TIFFJPEGMULTI = 905
   DTWAIN_TIFFLZWMULTI = 906
   DTWAIN_TIFFJBIGMULTI = 907
   DTWAIN_TIFFPIXARLOGMULTI = 908
   DTWAIN_WMF = 850
   DTWAIN_EMF = 851
   DTWAIN_GIF = 950
   DTWAIN_PNG = 1000
   DTWAIN_PSD = 2000
   DTWAIN_JPEG2000 = 3000
   DTWAIN_POSTSCRIPT1 = 4000
   DTWAIN_POSTSCRIPT2 = 4001
   DTWAIN_POSTSCRIPT3 = 4002
   DTWAIN_POSTSCRIPT1MULTI = 4003
   DTWAIN_POSTSCRIPT2MULTI = 4004
   DTWAIN_POSTSCRIPT3MULTI = 4005
   DTWAIN_TEXT = 6000
   DTWAIN_TEXTMULTI = 6001
   DTWAIN_TIFFMULTI = 7000
   DTWAIN_ICO = 8000
   DTWAIN_ICO_VISTA = 8001
   DTWAIN_ICO_RESIZED = 8002
   DTWAIN_WBMP = 8500
   DTWAIN_WEBP = 8501
   DTWAIN_PPM = 10000
   DTWAIN_WBMP_RESIZED = 11000
   DTWAIN_TGA_RLE = 11001
   DTWAIN_BMP_RLE = 11002
   DTWAIN_BIGTIFFLZW = 11003
   DTWAIN_BIGTIFFLZWMULTI = 11004
   DTWAIN_BIGTIFFNONE = 11005
   DTWAIN_BIGTIFFNONEMULTI = 11006
   DTWAIN_BIGTIFFPACKBITS = 11007
   DTWAIN_BIGTIFFPACKBITSMULTI = 11008
   DTWAIN_BIGTIFFDEFLATE = 11009
   DTWAIN_BIGTIFFDEFLATEMULTI = 11010
   DTWAIN_BIGTIFFG3 = 11011
   DTWAIN_BIGTIFFG3MULTI = 11012
   DTWAIN_BIGTIFFG4 = 11013
   DTWAIN_BIGTIFFG4MULTI = 11014
   DTWAIN_BIGTIFFJPEG = 11015
   DTWAIN_BIGTIFFJPEGMULTI = 11016
   DTWAIN_JPEGXR = 12000
   DTWAIN_INCHES = 0
   DTWAIN_CENTIMETERS = 1
   DTWAIN_PICAS = 2
   DTWAIN_POINTS = 3
   DTWAIN_TWIPS = 4
   DTWAIN_PIXELS = 5
   DTWAIN_MILLIMETERS = 6
   DTWAIN_USENAME = 16
   DTWAIN_USEPROMPT = 32
   DTWAIN_USELONGNAME = 64
   DTWAIN_USESOURCEMODE = 128
   DTWAIN_USELIST = 256
   DTWAIN_CREATE_DIRECTORY = 512
   DTWAIN_CREATEDIRECTORY = DTWAIN_CREATE_DIRECTORY
   DTWAIN_ARRAYANY = 1
   DTWAIN_ArrayTypePTR = 1
   DTWAIN_ARRAYLONG = 2
   DTWAIN_ARRAYFLOAT = 3
   DTWAIN_ARRAYHANDLE = 4
   DTWAIN_ARRAYSOURCE = 5
   DTWAIN_ARRAYSTRING = 6
   DTWAIN_ARRAYFRAME = 7
   DTWAIN_ARRAYBOOL = DTWAIN_ARRAYLONG
   DTWAIN_ARRAYLONGSTRING = 8
   DTWAIN_ARRAYUNICODESTRING = 9
   DTWAIN_ARRAYLONG64 = 10
   DTWAIN_ARRAYANSISTRING = 11
   DTWAIN_ARRAYWIDESTRING = 12
   DTWAIN_ARRAYTWFIX32 = 200
   DTWAIN_ArrayTypeINVALID = 0
   DTWAIN_ARRAYINT16 = 100
   DTWAIN_ARRAYUINT16 = 110
   DTWAIN_ARRAYUINT32 = 120
   DTWAIN_ARRAYINT32 = 130
   DTWAIN_ARRAYINT64 = 140
   DTWAIN_ARRAYUINT64 = 150
   DTWAIN_RANGELONG = DTWAIN_ARRAYLONG
   DTWAIN_RANGEFLOAT = DTWAIN_ARRAYFLOAT
   DTWAIN_RANGEMIN = 0
   DTWAIN_RANGEMAX = 1
   DTWAIN_RANGESTEP = 2
   DTWAIN_RANGEDEFAULT = 3
   DTWAIN_RANGECURRENT = 4
   DTWAIN_FRAMELEFT = 0
   DTWAIN_FRAMETOP = 1
   DTWAIN_FRAMERIGHT = 2
   DTWAIN_FRAMEBOTTOM = 3
   DTWAIN_FIX32WHOLE = 0
   DTWAIN_FIX32FRAC = 1
   DTWAIN_JC_NONE = 0
   DTWAIN_JC_JSIC = 1
   DTWAIN_JC_JSIS = 2
   DTWAIN_JC_JSXC = 3
   DTWAIN_JC_JSXS = 4
   DTWAIN_CAPDATATYPE_UNKNOWN = (-10)
   DTWAIN_JCBP_JSIC = 5
   DTWAIN_JCBP_JSIS = 6
   DTWAIN_JCBP_JSXC = 7
   DTWAIN_JCBP_JSXS = 8
   DTWAIN_FEEDPAGEON = 1
   DTWAIN_CLEARPAGEON = 2
   DTWAIN_REWINDPAGEON = 4
   DTWAIN_AppOwnsDib = 1
   DTWAIN_SourceOwnsDib = 2
   DTWAIN_CONTARRAY = 8
   DTWAIN_CONTENUMERATION = 16
   DTWAIN_CONTONEVALUE = 32
   DTWAIN_CONTRANGE = 64
   DTWAIN_CONTDEFAULT = 0
   DTWAIN_CAPGET = 1
   DTWAIN_CAPGETCURRENT = 2
   DTWAIN_CAPGETDEFAULT = 3
   DTWAIN_CAPSET = 6
   DTWAIN_CAPRESET = 7
   DTWAIN_CAPRESETALL = 8
   DTWAIN_CAPSETCONSTRAINT = 9
   DTWAIN_CAPSETAVAILABLE = 8
   DTWAIN_CAPSETCURRENT = 16
   DTWAIN_CAPGETHELP = 9
   DTWAIN_CAPGETLABEL = 10
   DTWAIN_CAPGETLABELENUM = 11
   DTWAIN_AREASET = DTWAIN_CAPSET
   DTWAIN_AREARESET = DTWAIN_CAPRESET
   DTWAIN_AREACURRENT = DTWAIN_CAPGETCURRENT
   DTWAIN_AREADEFAULT = DTWAIN_CAPGETDEFAULT
   DTWAIN_VER15 = 0
   DTWAIN_VER16 = 1
   DTWAIN_VER17 = 2
   DTWAIN_VER18 = 3
   DTWAIN_VER20 = 4
   DTWAIN_VER21 = 5
   DTWAIN_VER22 = 6
   DTWAIN_ACQUIREALL = (-1)
   DTWAIN_MAXACQUIRE = (-1)
   DTWAIN_DX_NONE = 0
   DTWAIN_DX_1PASSDUPLEX = 1
   DTWAIN_DX_2PASSDUPLEX = 2
   DTWAIN_PT_BW = 0
   DTWAIN_PT_GRAY = 1
   DTWAIN_PT_RGB = 2
   DTWAIN_PT_PALETTE = 3
   DTWAIN_PT_CMY = 4
   DTWAIN_PT_CMYK = 5
   DTWAIN_PT_YUV = 6
   DTWAIN_PT_YUVK = 7
   DTWAIN_PT_CIEXYZ = 8
   DTWAIN_PT_DEFAULT = 1000
   DTWAIN_CURRENT = (-2)
   DTWAIN_DEFAULT = (-1)
   DTWAIN_FLOATDEFAULT = (-9999.0)
   DTWAIN_CallbackERROR = 1
   DTWAIN_CallbackMESSAGE = 2
   DTWAIN_USENATIVE = 1
   DTWAIN_USEBUFFERED = 2
   DTWAIN_USECOMPRESSION = 4
   DTWAIN_USEMEMFILE = 8
   DTWAIN_FAILURE1 = (-1)
   DTWAIN_FAILURE2 = (-2)
   DTWAIN_DELETEALL = (-1)
   DTWAIN_TN_ACQUIREDONE = 1000
   DTWAIN_TN_ACQUIREFAILED = 1001
   DTWAIN_TN_ACQUIRECANCELLED = 1002
   DTWAIN_TN_ACQUIRESTARTED = 1003
   DTWAIN_TN_PAGECONTINUE = 1004
   DTWAIN_TN_PAGEFAILED = 1005
   DTWAIN_TN_PAGECANCELLED = 1006
   DTWAIN_TN_TRANSFERREADY = 1009
   DTWAIN_TN_TRANSFERDONE = 1010
   DTWAIN_TN_ACQUIREPAGEDONE = 1010
   DTWAIN_TN_UICLOSING = 3000
   DTWAIN_TN_UICLOSED = 3001
   DTWAIN_TN_UIOPENED = 3002
   DTWAIN_TN_UIOPENING = 3003
   DTWAIN_TN_UIOPENFAILURE = 3004
   DTWAIN_TN_CLIPTRANSFERDONE = 1014
   DTWAIN_TN_INVALIDIMAGEFORMAT = 1015
   DTWAIN_TN_ACQUIRETERMINATED = 1021
   DTWAIN_TN_TRANSFERSTRIPREADY = 1022
   DTWAIN_TN_TRANSFERSTRIPDONE = 1023
   DTWAIN_TN_TRANSFERSTRIPFAILED = 1029
   DTWAIN_TN_IMAGEINFOERROR = 1024
   DTWAIN_TN_TRANSFERCANCELLED = 1030
   DTWAIN_TN_FILESAVECANCELLED = 1031
   DTWAIN_TN_FILESAVEOK = 1032
   DTWAIN_TN_FILESAVEERROR = 1033
   DTWAIN_TN_FILEPAGESAVEOK = 1034
   DTWAIN_TN_FILEPAGESAVEERROR = 1035
   DTWAIN_TN_PROCESSEDDIB = 1036
   DTWAIN_TN_FEEDERLOADED = 1037
   DTWAIN_TN_GENERALERROR = 1038
   DTWAIN_TN_MANDUPFLIPPAGES = 1040
   DTWAIN_TN_MANDUPSIDE1DONE = 1041
   DTWAIN_TN_MANDUPSIDE2DONE = 1042
   DTWAIN_TN_MANDUPPAGECOUNTERROR = 1043
   DTWAIN_TN_MANDUPACQUIREDONE = 1044
   DTWAIN_TN_MANDUPSIDE1START = 1045
   DTWAIN_TN_MANDUPSIDE2START = 1046
   DTWAIN_TN_MANDUPMERGEERROR = 1047
   DTWAIN_TN_MANDUPMEMORYERROR = 1048
   DTWAIN_TN_MANDUPFILEERROR = 1049
   DTWAIN_TN_MANDUPFILESAVEERROR = 1050
   DTWAIN_TN_ENDOFJOBDETECTED = 1051
   DTWAIN_TN_EOJDETECTED = 1051
   DTWAIN_TN_EOJDETECTED_XFERDONE = 1052
   DTWAIN_TN_QUERYPAGEDISCARD = 1053
   DTWAIN_TN_PAGEDISCARDED = 1054
   DTWAIN_TN_PROCESSDIBACCEPTED = 1055
   DTWAIN_TN_PROCESSDIBFINALACCEPTED = 1056
   DTWAIN_TN_CLOSEDIBFAILED = 1057
   DTWAIN_TN_INVALID_TWAINDSM2_BITMAP = 1058
   DTWAIN_TN_IMAGE_RESAMPLE_FAILURE = 1059
   DTWAIN_TN_DEVICEEVENT = 1100
   DTWAIN_TN_TWAINPAGECANCELLED = 1105
   DTWAIN_TN_TWAINPAGEFAILED = 1106
   DTWAIN_TN_APPUPDATEDDIB = 1107
   DTWAIN_TN_FILEPAGESAVING = 1110
   DTWAIN_TN_EOJBEGINFILESAVE = 1112
   DTWAIN_TN_EOJENDFILESAVE = 1113
   DTWAIN_TN_CROPFAILED = 1120
   DTWAIN_TN_PROCESSEDDIBFINAL = 1121
   DTWAIN_TN_BLANKPAGEDETECTED1 = 1130
   DTWAIN_TN_BLANKPAGEDETECTED2 = 1131
   DTWAIN_TN_BLANKPAGEDETECTED3 = 1132
   DTWAIN_TN_BLANKPAGEDISCARDED1 = 1133
   DTWAIN_TN_BLANKPAGEDISCARDED2 = 1134
   DTWAIN_TN_OCRTEXTRETRIEVED = 1140
   DTWAIN_TN_QUERYOCRTEXT = 1141
   DTWAIN_TN_PDFOCRREADY = 1142
   DTWAIN_TN_PDFOCRDONE = 1143
   DTWAIN_TN_PDFOCRERROR = 1144
   DTWAIN_TN_SETCALLBACKINIT = 1150
   DTWAIN_TN_SETCALLBACK64INIT = 1151
   DTWAIN_TN_FILENAMECHANGING = 1160
   DTWAIN_TN_FILENAMECHANGED = 1161
   DTWAIN_TN_PROCESSEDAUDIOFINAL = 1180
   DTWAIN_TN_PROCESSAUDIOFINALACCEPTED = 1181
   DTWAIN_TN_PROCESSEDAUDIOFILE = 1182
   DTWAIN_TN_TWAINTRIPLETBEGIN = 1183
   DTWAIN_TN_TWAINTRIPLETEND = 1184
   DTWAIN_TN_FEEDERNOTLOADED = 1201
   DTWAIN_TN_FEEDERTIMEOUT = 1202
   DTWAIN_TN_FEEDERNOTENABLED = 1203
   DTWAIN_TN_FEEDERNOTSUPPORTED = 1204
   DTWAIN_TN_FEEDERTOFLATBED = 1205
   DTWAIN_TN_PREACQUIRESTART = 1206
   DTWAIN_TN_TRANSFERTILEREADY = 1300
   DTWAIN_TN_TRANSFERTILEDONE = 1301
   DTWAIN_TN_FILECOMPRESSTYPEMISMATCH = 1302
   DTWAIN_PDFOCR_CLEANTEXT1 = 1
   DTWAIN_PDFOCR_CLEANTEXT2 = 2
   DTWAIN_MODAL = 0
   DTWAIN_MODELESS = 1
   DTWAIN_UIModeCLOSE = 0
   DTWAIN_UIModeOPEN = 1
   DTWAIN_REOPEN_SOURCE = 2
   DTWAIN_ROUNDNEAREST = 0
   DTWAIN_ROUNDUP = 1
   DTWAIN_ROUNDDOWN = 2
   DTWAIN_FLOATDELTA = (+1.0e-8)
   DTWAIN_OR_ROT0 = 0
   DTWAIN_OR_ROT90 = 1
   DTWAIN_OR_ROT180 = 2
   DTWAIN_OR_ROT270 = 3
   DTWAIN_OR_PORTRAIT = DTWAIN_OR_ROT0
   DTWAIN_OR_LANDSCAPE = DTWAIN_OR_ROT270
   DTWAIN_OR_ANYROTATION = (-1)
   DTWAIN_CO_GET = 0x0001
   DTWAIN_CO_SET = 0x0002
   DTWAIN_CO_GETDEFAULT = 0x0004
   DTWAIN_CO_GETCURRENT = 0x0008
   DTWAIN_CO_RESET = 0x0010
   DTWAIN_CO_SETCONSTRAINT = 0x0020
   DTWAIN_CO_CONSTRAINABLE = 0x0040
   DTWAIN_CO_GETHELP = 0x0100
   DTWAIN_CO_GETLABEL = 0x0200
   DTWAIN_CO_GETLABELENUM = 0x0400
   DTWAIN_CNTYAFGHANISTAN = 1001
   DTWAIN_CNTYALGERIA = 213
   DTWAIN_CNTYAMERICANSAMOA = 684
   DTWAIN_CNTYANDORRA = 33
   DTWAIN_CNTYANGOLA = 1002
   DTWAIN_CNTYANGUILLA = 8090
   DTWAIN_CNTYANTIGUA = 8091
   DTWAIN_CNTYARGENTINA = 54
   DTWAIN_CNTYARUBA = 297
   DTWAIN_CNTYASCENSIONI = 247
   DTWAIN_CNTYAUSTRALIA = 61
   DTWAIN_CNTYAUSTRIA = 43
   DTWAIN_CNTYBAHAMAS = 8092
   DTWAIN_CNTYBAHRAIN = 973
   DTWAIN_CNTYBANGLADESH = 880
   DTWAIN_CNTYBARBADOS = 8093
   DTWAIN_CNTYBELGIUM = 32
   DTWAIN_CNTYBELIZE = 501
   DTWAIN_CNTYBENIN = 229
   DTWAIN_CNTYBERMUDA = 8094
   DTWAIN_CNTYBHUTAN = 1003
   DTWAIN_CNTYBOLIVIA = 591
   DTWAIN_CNTYBOTSWANA = 267
   DTWAIN_CNTYBRITAIN = 6
   DTWAIN_CNTYBRITVIRGINIS = 8095
   DTWAIN_CNTYBRAZIL = 55
   DTWAIN_CNTYBRUNEI = 673
   DTWAIN_CNTYBULGARIA = 359
   DTWAIN_CNTYBURKINAFASO = 1004
   DTWAIN_CNTYBURMA = 1005
   DTWAIN_CNTYBURUNDI = 1006
   DTWAIN_CNTYCAMAROON = 237
   DTWAIN_CNTYCANADA = 2
   DTWAIN_CNTYCAPEVERDEIS = 238
   DTWAIN_CNTYCAYMANIS = 8096
   DTWAIN_CNTYCENTRALAFREP = 1007
   DTWAIN_CNTYCHAD = 1008
   DTWAIN_CNTYCHILE = 56
   DTWAIN_CNTYCHINA = 86
   DTWAIN_CNTYCHRISTMASIS = 1009
   DTWAIN_CNTYCOCOSIS = 1009
   DTWAIN_CNTYCOLOMBIA = 57
   DTWAIN_CNTYCOMOROS = 1010
   DTWAIN_CNTYCONGO = 1011
   DTWAIN_CNTYCOOKIS = 1012
   DTWAIN_CNTYCOSTARICA = 506
   DTWAIN_CNTYCUBA = 5
   DTWAIN_CNTYCYPRUS = 357
   DTWAIN_CNTYCZECHOSLOVAKIA = 42
   DTWAIN_CNTYDENMARK = 45
   DTWAIN_CNTYDJIBOUTI = 1013
   DTWAIN_CNTYDOMINICA = 8097
   DTWAIN_CNTYDOMINCANREP = 8098
   DTWAIN_CNTYEASTERIS = 1014
   DTWAIN_CNTYECUADOR = 593
   DTWAIN_CNTYEGYPT = 20
   DTWAIN_CNTYELSALVADOR = 503
   DTWAIN_CNTYEQGUINEA = 1015
   DTWAIN_CNTYETHIOPIA = 251
   DTWAIN_CNTYFALKLANDIS = 1016
   DTWAIN_CNTYFAEROEIS = 298
   DTWAIN_CNTYFIJIISLANDS = 679
   DTWAIN_CNTYFINLAND = 358
   DTWAIN_CNTYFRANCE = 33
   DTWAIN_CNTYFRANTILLES = 596
   DTWAIN_CNTYFRGUIANA = 594
   DTWAIN_CNTYFRPOLYNEISA = 689
   DTWAIN_CNTYFUTANAIS = 1043
   DTWAIN_CNTYGABON = 241
   DTWAIN_CNTYGAMBIA = 220
   DTWAIN_CNTYGERMANY = 49
   DTWAIN_CNTYGHANA = 233
   DTWAIN_CNTYGIBRALTER = 350
   DTWAIN_CNTYGREECE = 30
   DTWAIN_CNTYGREENLAND = 299
   DTWAIN_CNTYGRENADA = 8099
   DTWAIN_CNTYGRENEDINES = 8015
   DTWAIN_CNTYGUADELOUPE = 590
   DTWAIN_CNTYGUAM = 671
   DTWAIN_CNTYGUANTANAMOBAY = 5399
   DTWAIN_CNTYGUATEMALA = 502
   DTWAIN_CNTYGUINEA = 224
   DTWAIN_CNTYGUINEABISSAU = 1017
   DTWAIN_CNTYGUYANA = 592
   DTWAIN_CNTYHAITI = 509
   DTWAIN_CNTYHONDURAS = 504
   DTWAIN_CNTYHONGKONG = 852
   DTWAIN_CNTYHUNGARY = 36
   DTWAIN_CNTYICELAND = 354
   DTWAIN_CNTYINDIA = 91
   DTWAIN_CNTYINDONESIA = 62
   DTWAIN_CNTYIRAN = 98
   DTWAIN_CNTYIRAQ = 964
   DTWAIN_CNTYIRELAND = 353
   DTWAIN_CNTYISRAEL = 972
   DTWAIN_CNTYITALY = 39
   DTWAIN_CNTYIVORYCOAST = 225
   DTWAIN_CNTYJAMAICA = 8010
   DTWAIN_CNTYJAPAN = 81
   DTWAIN_CNTYJORDAN = 962
   DTWAIN_CNTYKENYA = 254
   DTWAIN_CNTYKIRIBATI = 1018
   DTWAIN_CNTYKOREA = 82
   DTWAIN_CNTYKUWAIT = 965
   DTWAIN_CNTYLAOS = 1019
   DTWAIN_CNTYLEBANON = 1020
   DTWAIN_CNTYLIBERIA = 231
   DTWAIN_CNTYLIBYA = 218
   DTWAIN_CNTYLIECHTENSTEIN = 41
   DTWAIN_CNTYLUXENBOURG = 352
   DTWAIN_CNTYMACAO = 853
   DTWAIN_CNTYMADAGASCAR = 1021
   DTWAIN_CNTYMALAWI = 265
   DTWAIN_CNTYMALAYSIA = 60
   DTWAIN_CNTYMALDIVES = 960
   DTWAIN_CNTYMALI = 1022
   DTWAIN_CNTYMALTA = 356
   DTWAIN_CNTYMARSHALLIS = 692
   DTWAIN_CNTYMAURITANIA = 1023
   DTWAIN_CNTYMAURITIUS = 230
   DTWAIN_CNTYMEXICO = 3
   DTWAIN_CNTYMICRONESIA = 691
   DTWAIN_CNTYMIQUELON = 508
   DTWAIN_CNTYMONACO = 33
   DTWAIN_CNTYMONGOLIA = 1024
   DTWAIN_CNTYMONTSERRAT = 8011
   DTWAIN_CNTYMOROCCO = 212
   DTWAIN_CNTYMOZAMBIQUE = 1025
   DTWAIN_CNTYNAMIBIA = 264
   DTWAIN_CNTYNAURU = 1026
   DTWAIN_CNTYNEPAL = 977
   DTWAIN_CNTYNETHERLANDS = 31
   DTWAIN_CNTYNETHANTILLES = 599
   DTWAIN_CNTYNEVIS = 8012
   DTWAIN_CNTYNEWCALEDONIA = 687
   DTWAIN_CNTYNEWZEALAND = 64
   DTWAIN_CNTYNICARAGUA = 505
   DTWAIN_CNTYNIGER = 227
   DTWAIN_CNTYNIGERIA = 234
   DTWAIN_CNTYNIUE = 1027
   DTWAIN_CNTYNORFOLKI = 1028
   DTWAIN_CNTYNORWAY = 47
   DTWAIN_CNTYOMAN = 968
   DTWAIN_CNTYPAKISTAN = 92
   DTWAIN_CNTYPALAU = 1029
   DTWAIN_CNTYPANAMA = 507
   DTWAIN_CNTYPARAGUAY = 595
   DTWAIN_CNTYPERU = 51
   DTWAIN_CNTYPHILLIPPINES = 63
   DTWAIN_CNTYPITCAIRNIS = 1030
   DTWAIN_CNTYPNEWGUINEA = 675
   DTWAIN_CNTYPOLAND = 48
   DTWAIN_CNTYPORTUGAL = 351
   DTWAIN_CNTYQATAR = 974
   DTWAIN_CNTYREUNIONI = 1031
   DTWAIN_CNTYROMANIA = 40
   DTWAIN_CNTYRWANDA = 250
   DTWAIN_CNTYSAIPAN = 670
   DTWAIN_CNTYSANMARINO = 39
   DTWAIN_CNTYSAOTOME = 1033
   DTWAIN_CNTYSAUDIARABIA = 966
   DTWAIN_CNTYSENEGAL = 221
   DTWAIN_CNTYSEYCHELLESIS = 1034
   DTWAIN_CNTYSIERRALEONE = 1035
   DTWAIN_CNTYSINGAPORE = 65
   DTWAIN_CNTYSOLOMONIS = 1036
   DTWAIN_CNTYSOMALI = 1037
   DTWAIN_CNTYSOUTHAFRICA = 27
   DTWAIN_CNTYSPAIN = 34
   DTWAIN_CNTYSRILANKA = 94
   DTWAIN_CNTYSTHELENA = 1032
   DTWAIN_CNTYSTKITTS = 8013
   DTWAIN_CNTYSTLUCIA = 8014
   DTWAIN_CNTYSTPIERRE = 508
   DTWAIN_CNTYSTVINCENT = 8015
   DTWAIN_CNTYSUDAN = 1038
   DTWAIN_CNTYSURINAME = 597
   DTWAIN_CNTYSWAZILAND = 268
   DTWAIN_CNTYSWEDEN = 46
   DTWAIN_CNTYSWITZERLAND = 41
   DTWAIN_CNTYSYRIA = 1039
   DTWAIN_CNTYTAIWAN = 886
   DTWAIN_CNTYTANZANIA = 255
   DTWAIN_CNTYTHAILAND = 66
   DTWAIN_CNTYTOBAGO = 8016
   DTWAIN_CNTYTOGO = 228
   DTWAIN_CNTYTONGAIS = 676
   DTWAIN_CNTYTRINIDAD = 8016
   DTWAIN_CNTYTUNISIA = 216
   DTWAIN_CNTYTURKEY = 90
   DTWAIN_CNTYTURKSCAICOS = 8017
   DTWAIN_CNTYTUVALU = 1040
   DTWAIN_CNTYUGANDA = 256
   DTWAIN_CNTYUSSR = 7
   DTWAIN_CNTYUAEMIRATES = 971
   DTWAIN_CNTYUNITEDKINGDOM = 44
   DTWAIN_CNTYUSA = 1
   DTWAIN_CNTYURUGUAY = 598
   DTWAIN_CNTYVANUATU = 1041
   DTWAIN_CNTYVATICANCITY = 39
   DTWAIN_CNTYVENEZUELA = 58
   DTWAIN_CNTYWAKE = 1042
   DTWAIN_CNTYWALLISIS = 1043
   DTWAIN_CNTYWESTERNSAHARA = 1044
   DTWAIN_CNTYWESTERNSAMOA = 1045
   DTWAIN_CNTYYEMEN = 1046
   DTWAIN_CNTYYUGOSLAVIA = 38
   DTWAIN_CNTYZAIRE = 243
   DTWAIN_CNTYZAMBIA = 260
   DTWAIN_CNTYZIMBABWE = 263
   DTWAIN_LANGDANISH = 0
   DTWAIN_LANGDUTCH = 1
   DTWAIN_LANGINTERNATIONALENGLISH = 2
   DTWAIN_LANGFRENCHCANADIAN = 3
   DTWAIN_LANGFINNISH = 4
   DTWAIN_LANGFRENCH = 5
   DTWAIN_LANGGERMAN = 6
   DTWAIN_LANGICELANDIC = 7
   DTWAIN_LANGITALIAN = 8
   DTWAIN_LANGNORWEGIAN = 9
   DTWAIN_LANGPORTUGUESE = 10
   DTWAIN_LANGSPANISH = 11
   DTWAIN_LANGSWEDISH = 12
   DTWAIN_LANGUSAENGLISH = 13
   DTWAIN_NO_ERROR = (0)
   DTWAIN_ERR_FIRST = (-1000)
   DTWAIN_ERR_BAD_HANDLE = (-1001)
   DTWAIN_ERR_BAD_SOURCE = (-1002)
   DTWAIN_ERR_BAD_ARRAY = (-1003)
   DTWAIN_ERR_WRONG_ARRAY_TYPE = (-1004)
   DTWAIN_ERR_INDEX_BOUNDS = (-1005)
   DTWAIN_ERR_OUT_OF_MEMORY = (-1006)
   DTWAIN_ERR_NULL_WINDOW = (-1007)
   DTWAIN_ERR_BAD_PIXTYPE = (-1008)
   DTWAIN_ERR_BAD_CONTAINER = (-1009)
   DTWAIN_ERR_NO_SESSION = (-1010)
   DTWAIN_ERR_BAD_ACQUIRE_NUM = (-1011)
   DTWAIN_ERR_BAD_CAP = (-1012)
   DTWAIN_ERR_CAP_NO_SUPPORT = (-1013)
   DTWAIN_ERR_TWAIN = (-1014)
   DTWAIN_ERR_HOOK_FAILED = (-1015)
   DTWAIN_ERR_BAD_FILENAME = (-1016)
   DTWAIN_ERR_EMPTY_ARRAY = (-1017)
   DTWAIN_ERR_FILE_FORMAT = (-1018)
   DTWAIN_ERR_BAD_DIB_PAGE = (-1019)
   DTWAIN_ERR_SOURCE_ACQUIRING = (-1020)
   DTWAIN_ERR_INVALID_PARAM = (-1021)
   DTWAIN_ERR_INVALID_RANGE = (-1022)
   DTWAIN_ERR_UI_ERROR = (-1023)
   DTWAIN_ERR_BAD_UNIT = (-1024)
   DTWAIN_ERR_LANGDLL_NOT_FOUND = (-1025)
   DTWAIN_ERR_SOURCE_NOT_OPEN = (-1026)
   DTWAIN_ERR_DEVICEEVENT_NOT_SUPPORTED = (-1027)
   DTWAIN_ERR_UIONLY_NOT_SUPPORTED = (-1028)
   DTWAIN_ERR_UI_ALREADY_OPENED = (-1029)
   DTWAIN_ERR_CAPSET_NOSUPPORT = (-1030)
   DTWAIN_ERR_NO_FILE_XFER = (-1031)
   DTWAIN_ERR_INVALID_BITDEPTH = (-1032)
   DTWAIN_ERR_NO_CAPS_DEFINED = (-1033)
   DTWAIN_ERR_TILES_NOT_SUPPORTED = (-1034)
   DTWAIN_ERR_INVALID_DTWAIN_FRAME = (-1035)
   DTWAIN_ERR_LIMITED_VERSION = (-1036)
   DTWAIN_ERR_NO_FEEDER = (-1037)
   DTWAIN_ERR_NO_FEEDER_QUERY = (-1038)
   DTWAIN_ERR_EXCEPTION_ERROR = (-1039)
   DTWAIN_ERR_INVALID_STATE = (-1040)
   DTWAIN_ERR_UNSUPPORTED_EXTINFO = (-1041)
   DTWAIN_ERR_DLLRESOURCE_NOTFOUND = (-1042)
   DTWAIN_ERR_NOT_INITIALIZED = (-1043)
   DTWAIN_ERR_NO_SOURCES = (-1044)
   DTWAIN_ERR_TWAIN_NOT_INSTALLED = (-1045)
   DTWAIN_ERR_WRONG_THREAD = (-1046)
   DTWAIN_ERR_BAD_CAPTYPE = (-1047)
   DTWAIN_ERR_UNKNOWN_CAPDATATYPE = (-1048)
   DTWAIN_ERR_DEMO_NOFILETYPE = (-1049)
   DTWAIN_ERR_SOURCESELECTION_CANCELED = (-1050)
   DTWAIN_ERR_RESOURCES_NOT_FOUND = (-1051)
   DTWAIN_ERR_STRINGTYPE_MISMATCH = (-1052)
   DTWAIN_ERR_ARRAYTYPE_MISMATCH = (-1053)
   DTWAIN_ERR_SOURCENAME_NOTINSTALLED = (-1054)
   DTWAIN_ERR_NO_MEMFILE_XFER = (-1055)
   DTWAIN_ERR_AREA_ARRAY_TOO_SMALL = (-1056)
   DTWAIN_ERR_LOG_CREATE_ERROR = (-1057)
   DTWAIN_ERR_FILESYSTEM_NOT_SUPPORTED = (-1058)
   DTWAIN_ERR_TILEMODE_NOTSET = (-1059)
   DTWAIN_ERR_INI32_NOT_FOUND = (-1060)
   DTWAIN_ERR_INI64_NOT_FOUND = (-1061)
   DTWAIN_ERR_CRC_CHECK = (-1062)
   DTWAIN_ERR_RESOURCES_BAD_VERSION = (-1063)
   DTWAIN_ERR_WIN32_ERROR = (-1064)
   DTWAIN_ERR_STRINGID_NOTFOUND = (-1065)
   DTWAIN_ERR_RESOURCES_DUPLICATEID_FOUND = (-1066)
   DTWAIN_ERR_UNAVAILABLE_EXTINFO = (-1067)
   DTWAIN_ERR_TWAINDSM2_BADBITMAP = (-1068)
   DTWAIN_ERR_ACQUISITION_CANCELED = (-1069)
   DTWAIN_ERR_IMAGE_RESAMPLED = (-1070)
   DTWAIN_ERR_UNKNOWN_TWAIN_RC = (-1071)
   DTWAIN_ERR_UNKNOWN_TWAIN_CC = (-1072)
   DTWAIN_ERR_RESOURCES_DATA_EXCEPTION = (-1073)
   DTWAIN_ERR_AUDIO_TRANSFER_NOTSUPPORTED = (-1074)
   DTWAIN_ERR_FEEDER_COMPLIANCY = (-1075)
   DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY1 = (-1076)
   DTWAIN_ERR_SUPPORTEDCAPS_COMPLIANCY2 = (-1077)
   DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY1 = (-1078)
   DTWAIN_ERR_ICAPPIXELTYPE_COMPLIANCY2 = (-1079)
   DTWAIN_ERR_ICAPBITDEPTH_COMPLIANCY1 = (-1080)
   DTWAIN_ERR_XFERMECH_COMPLIANCY = (-1081)
   DTWAIN_ERR_STANDARDCAPS_COMPLIANCY = (-1082)
   DTWAIN_ERR_EXTIMAGEINFO_DATATYPE_MISMATCH = (-1083)
   DTWAIN_ERR_EXTIMAGEINFO_RETRIEVAL = (-1084)
   DTWAIN_ERR_RANGE_OUTOFBOUNDS = (-1085)
   DTWAIN_ERR_RANGE_STEPISZERO = (-1086)
   DTWAIN_ERR_BLANKNAMEDETECTED = (-1087)
   DTWAIN_ERR_FEEDER_NOPAPERSENSOR = (-1088)
   TWAIN_ERR_LOW_MEMORY = (-1100)
   TWAIN_ERR_FALSE_ALARM = (-1101)
   TWAIN_ERR_BUMMER = (-1102)
   TWAIN_ERR_NODATASOURCE = (-1103)
   TWAIN_ERR_MAXCONNECTIONS = (-1104)
   TWAIN_ERR_OPERATIONERROR = (-1105)
   TWAIN_ERR_BADCAPABILITY = (-1106)
   TWAIN_ERR_BADVALUE = (-1107)
   TWAIN_ERR_BADPROTOCOL = (-1108)
   TWAIN_ERR_SEQUENCEERROR = (-1109)
   TWAIN_ERR_BADDESTINATION = (-1110)
   TWAIN_ERR_CAPNOTSUPPORTED = (-1111)
   TWAIN_ERR_CAPBADOPERATION = (-1112)
   TWAIN_ERR_CAPSEQUENCEERROR = (-1113)
   TWAIN_ERR_FILEPROTECTEDERROR = (-1114)
   TWAIN_ERR_FILEEXISTERROR = (-1115)
   TWAIN_ERR_FILENOTFOUND = (-1116)
   TWAIN_ERR_DIRNOTEMPTY = (-1117)
   TWAIN_ERR_FEEDERJAMMED = (-1118)
   TWAIN_ERR_FEEDERMULTPAGES = (-1119)
   TWAIN_ERR_FEEDERWRITEERROR = (-1120)
   TWAIN_ERR_DEVICEOFFLINE = (-1121)
   TWAIN_ERR_NULL_CONTAINER = (-1122)
   TWAIN_ERR_INTERLOCK = (-1123)
   TWAIN_ERR_DAMAGEDCORNER = (-1124)
   TWAIN_ERR_FOCUSERROR = (-1125)
   TWAIN_ERR_DOCTOOLIGHT = (-1126)
   TWAIN_ERR_DOCTOODARK = (-1127)
   TWAIN_ERR_NOMEDIA = (-1128)
   DTWAIN_ERR_FILEXFERSTART = (-2000)
   DTWAIN_ERR_MEM = (-2001)
   DTWAIN_ERR_FILEOPEN = (-2002)
   DTWAIN_ERR_FILEREAD = (-2003)
   DTWAIN_ERR_FILEWRITE = (-2004)
   DTWAIN_ERR_BADPARAM = (-2005)
   DTWAIN_ERR_INVALIDBMP = (-2006)
   DTWAIN_ERR_BMPRLE = (-2007)
   DTWAIN_ERR_RESERVED1 = (-2008)
   DTWAIN_ERR_INVALIDJPG = (-2009)
   DTWAIN_ERR_DC = (-2010)
   DTWAIN_ERR_DIB = (-2011)
   DTWAIN_ERR_RESERVED2 = (-2012)
   DTWAIN_ERR_NORESOURCE = (-2013)
   DTWAIN_ERR_CALLBACKCANCEL = (-2014)
   DTWAIN_ERR_INVALIDPNG = (-2015)
   DTWAIN_ERR_PNGCREATE = (-2016)
   DTWAIN_ERR_INTERNAL = (-2017)
   DTWAIN_ERR_FONT = (-2018)
   DTWAIN_ERR_INTTIFF = (-2019)
   DTWAIN_ERR_INVALIDTIFF = (-2020)
   DTWAIN_ERR_NOTIFFLZW = (-2021)
   DTWAIN_ERR_INVALIDPCX = (-2022)
   DTWAIN_ERR_CREATEBMP = (-2023)
   DTWAIN_ERR_NOLINES = (-2024)
   DTWAIN_ERR_GETDIB = (-2025)
   DTWAIN_ERR_NODEVOP = (-2026)
   DTWAIN_ERR_INVALIDWMF = (-2027)
   DTWAIN_ERR_DEPTHMISMATCH = (-2028)
   DTWAIN_ERR_BITBLT = (-2029)
   DTWAIN_ERR_BUFTOOSMALL = (-2030)
   DTWAIN_ERR_TOOMANYCOLORS = (-2031)
   DTWAIN_ERR_INVALIDTGA = (-2032)
   DTWAIN_ERR_NOTGATHUMBNAIL = (-2033)
   DTWAIN_ERR_RESERVED3 = (-2034)
   DTWAIN_ERR_CREATEDIB = (-2035)
   DTWAIN_ERR_NOLZW = (-2036)
   DTWAIN_ERR_SELECTOBJ = (-2037)
   DTWAIN_ERR_BADMANAGER = (-2038)
   DTWAIN_ERR_OBSOLETE = (-2039)
   DTWAIN_ERR_CREATEDIBSECTION = (-2040)
   DTWAIN_ERR_SETWINMETAFILEBITS = (-2041)
   DTWAIN_ERR_GETWINMETAFILEBITS = (-2042)
   DTWAIN_ERR_PAXPWD = (-2043)
   DTWAIN_ERR_INVALIDPAX = (-2044)
   DTWAIN_ERR_NOSUPPORT = (-2045)
   DTWAIN_ERR_INVALIDPSD = (-2046)
   DTWAIN_ERR_PSDNOTSUPPORTED = (-2047)
   DTWAIN_ERR_DECRYPT = (-2048)
   DTWAIN_ERR_ENCRYPT = (-2049)
   DTWAIN_ERR_COMPRESSION = (-2050)
   DTWAIN_ERR_DECOMPRESSION = (-2051)
   DTWAIN_ERR_INVALIDTLA = (-2052)
   DTWAIN_ERR_INVALIDWBMP = (-2053)
   DTWAIN_ERR_NOTIFFTAG = (-2054)
   DTWAIN_ERR_NOLOCALSTORAGE = (-2055)
   DTWAIN_ERR_INVALIDEXIF = (-2056)
   DTWAIN_ERR_NOEXIFSTRING = (-2057)
   DTWAIN_ERR_TIFFDLL32NOTFOUND = (-2058)
   DTWAIN_ERR_TIFFDLL16NOTFOUND = (-2059)
   DTWAIN_ERR_PNGDLL16NOTFOUND = (-2060)
   DTWAIN_ERR_JPEGDLL16NOTFOUND = (-2061)
   DTWAIN_ERR_BADBITSPERPIXEL = (-2062)
   DTWAIN_ERR_TIFFDLL32INVALIDVER = (-2063)
   DTWAIN_ERR_PDFDLL32NOTFOUND = (-2064)
   DTWAIN_ERR_PDFDLL32INVALIDVER = (-2065)
   DTWAIN_ERR_JPEGDLL32NOTFOUND = (-2066)
   DTWAIN_ERR_JPEGDLL32INVALIDVER = (-2067)
   DTWAIN_ERR_PNGDLL32NOTFOUND = (-2068)
   DTWAIN_ERR_PNGDLL32INVALIDVER = (-2069)
   DTWAIN_ERR_J2KDLL32NOTFOUND = (-2070)
   DTWAIN_ERR_J2KDLL32INVALIDVER = (-2071)
   DTWAIN_ERR_MANDUPLEX_UNAVAILABLE = (-2072)
   DTWAIN_ERR_TIMEOUT = (-2073)
   DTWAIN_ERR_INVALIDICONFORMAT = (-2074)
   DTWAIN_ERR_TWAIN32DSMNOTFOUND = (-2075)
   DTWAIN_ERR_TWAINOPENSOURCEDSMNOTFOUND = (-2076)
   DTWAIN_ERR_INVALID_DIRECTORY = (-2077)
   DTWAIN_ERR_CREATE_DIRECTORY = (-2078)
   DTWAIN_ERR_OCRLIBRARY_NOTFOUND = (-2079)
   DTWAIN_TWAINSAVE_OK = (0)
   DTWAIN_ERR_TS_FIRST = (-2080)
   DTWAIN_ERR_TS_NOFILENAME = (-2081)
   DTWAIN_ERR_TS_NOTWAINSYS = (-2082)
   DTWAIN_ERR_TS_DEVICEFAILURE = (-2083)
   DTWAIN_ERR_TS_FILESAVEERROR = (-2084)
   DTWAIN_ERR_TS_COMMANDILLEGAL = (-2085)
   DTWAIN_ERR_TS_CANCELLED = (-2086)
   DTWAIN_ERR_TS_ACQUISITIONERROR = (-2087)
   DTWAIN_ERR_TS_INVALIDCOLORSPACE = (-2088)
   DTWAIN_ERR_TS_PDFNOTSUPPORTED = (-2089)
   DTWAIN_ERR_TS_NOTAVAILABLE = (-2090)
   DTWAIN_ERR_OCR_FIRST = (-2100)
   DTWAIN_ERR_OCR_INVALIDPAGENUM = (-2101)
   DTWAIN_ERR_OCR_INVALIDENGINE = (-2102)
   DTWAIN_ERR_OCR_NOTACTIVE = (-2103)
   DTWAIN_ERR_OCR_INVALIDFILETYPE = (-2104)
   DTWAIN_ERR_OCR_INVALIDPIXELTYPE = (-2105)
   DTWAIN_ERR_OCR_INVALIDBITDEPTH = (-2106)
   DTWAIN_ERR_OCR_RECOGNITIONERROR = (-2107)
   DTWAIN_ERR_OCR_LAST = (-2108)
   DTWAIN_ERR_LAST = DTWAIN_ERR_OCR_LAST
   DTWAIN_ERR_SOURCE_COULD_NOT_OPEN = (-2500)
   DTWAIN_ERR_SOURCE_COULD_NOT_CLOSE = (-2501)
   DTWAIN_ERR_IMAGEINFO_INVALID = (-2502)
   DTWAIN_ERR_WRITEDATA_TOFILE = (-2503)
   DTWAIN_ERR_OPERATION_NOTSUPPORTED = (-2504)
   DTWAIN_DE_CHKAUTOCAPTURE = 1
   DTWAIN_DE_CHKBATTERY = 2
   DTWAIN_DE_CHKDEVICEONLINE = 4
   DTWAIN_DE_CHKFLASH = 8
   DTWAIN_DE_CHKPOWERSUPPLY = 16
   DTWAIN_DE_CHKRESOLUTION = 32
   DTWAIN_DE_DEVICEADDED = 64
   DTWAIN_DE_DEVICEOFFLINE = 128
   DTWAIN_DE_DEVICEREADY = 256
   DTWAIN_DE_DEVICEREMOVED = 512
   DTWAIN_DE_IMAGECAPTURED = 1024
   DTWAIN_DE_IMAGEDELETED = 2048
   DTWAIN_DE_PAPERDOUBLEFEED = 4096
   DTWAIN_DE_PAPERJAM = 8192
   DTWAIN_DE_LAMPFAILURE = 16384
   DTWAIN_DE_POWERSAVE = 32768
   DTWAIN_DE_POWERSAVENOTIFY = 65536
   DTWAIN_DE_CUSTOMEVENTS = 0x8000
   DTWAIN_GETDE_EVENT = 0
   DTWAIN_GETDE_DEVNAME = 1
   DTWAIN_GETDE_BATTERYMINUTES = 2
   DTWAIN_GETDE_BATTERYPCT = 3
   DTWAIN_GETDE_XRESOLUTION = 4
   DTWAIN_GETDE_YRESOLUTION = 5
   DTWAIN_GETDE_FLASHUSED = 6
   DTWAIN_GETDE_AUTOCAPTURE = 7
   DTWAIN_GETDE_TIMEBEFORECAPTURE = 8
   DTWAIN_GETDE_TIMEBETWEENCAPTURES = 9
   DTWAIN_GETDE_POWERSUPPLY = 10
   DTWAIN_IMPRINTERTOPBEFORE = 1
   DTWAIN_IMPRINTERTOPAFTER = 2
   DTWAIN_IMPRINTERBOTTOMBEFORE = 4
   DTWAIN_IMPRINTERBOTTOMAFTER = 8
   DTWAIN_ENDORSERTOPBEFORE = 16
   DTWAIN_ENDORSERTOPAFTER = 32
   DTWAIN_ENDORSERBOTTOMBEFORE = 64
   DTWAIN_ENDORSERBOTTOMAFTER = 128
   DTWAIN_PM_SINGLESTRING = 0
   DTWAIN_PM_MULTISTRING = 1
   DTWAIN_PM_COMPOUNDSTRING = 2
   DTWAIN_TWTY_INT8 = 0x0000
   DTWAIN_TWTY_INT16 = 0x0001
   DTWAIN_TWTY_INT32 = 0x0002
   DTWAIN_TWTY_UINT8 = 0x0003
   DTWAIN_TWTY_UINT16 = 0x0004
   DTWAIN_TWTY_UINT32 = 0x0005
   DTWAIN_TWTY_BOOL = 0x0006
   DTWAIN_TWTY_FIX32 = 0x0007
   DTWAIN_TWTY_FRAME = 0x0008
   DTWAIN_TWTY_STR32 = 0x0009
   DTWAIN_TWTY_STR64 = 0x000A
   DTWAIN_TWTY_STR128 = 0x000B
   DTWAIN_TWTY_STR255 = 0x000C
   DTWAIN_TWTY_STR1024 = 0x000D
   DTWAIN_TWTY_UNI512 = 0x000E
   DTWAIN_EI_BARCODEX = 0x1200
   DTWAIN_EI_BARCODEY = 0x1201
   DTWAIN_EI_BARCODETEXT = 0x1202
   DTWAIN_EI_BARCODETYPE = 0x1203
   DTWAIN_EI_DESHADETOP = 0x1204
   DTWAIN_EI_DESHADELEFT = 0x1205
   DTWAIN_EI_DESHADEHEIGHT = 0x1206
   DTWAIN_EI_DESHADEWIDTH = 0x1207
   DTWAIN_EI_DESHADESIZE = 0x1208
   DTWAIN_EI_SPECKLESREMOVED = 0x1209
   DTWAIN_EI_HORZLINEXCOORD = 0x120A
   DTWAIN_EI_HORZLINEYCOORD = 0x120B
   DTWAIN_EI_HORZLINELENGTH = 0x120C
   DTWAIN_EI_HORZLINETHICKNESS = 0x120D
   DTWAIN_EI_VERTLINEXCOORD = 0x120E
   DTWAIN_EI_VERTLINEYCOORD = 0x120F
   DTWAIN_EI_VERTLINELENGTH = 0x1210
   DTWAIN_EI_VERTLINETHICKNESS = 0x1211
   DTWAIN_EI_PATCHCODE = 0x1212
   DTWAIN_EI_ENDORSEDTEXT = 0x1213
   DTWAIN_EI_FORMCONFIDENCE = 0x1214
   DTWAIN_EI_FORMTEMPLATEMATCH = 0x1215
   DTWAIN_EI_FORMTEMPLATEPAGEMATCH = 0x1216
   DTWAIN_EI_FORMHORZDOCOFFSET = 0x1217
   DTWAIN_EI_FORMVERTDOCOFFSET = 0x1218
   DTWAIN_EI_BARCODECOUNT = 0x1219
   DTWAIN_EI_BARCODECONFIDENCE = 0x121A
   DTWAIN_EI_BARCODEROTATION = 0x121B
   DTWAIN_EI_BARCODETEXTLENGTH = 0x121C
   DTWAIN_EI_DESHADECOUNT = 0x121D
   DTWAIN_EI_DESHADEBLACKCOUNTOLD = 0x121E
   DTWAIN_EI_DESHADEBLACKCOUNTNEW = 0x121F
   DTWAIN_EI_DESHADEBLACKRLMIN = 0x1220
   DTWAIN_EI_DESHADEBLACKRLMAX = 0x1221
   DTWAIN_EI_DESHADEWHITECOUNTOLD = 0x1222
   DTWAIN_EI_DESHADEWHITECOUNTNEW = 0x1223
   DTWAIN_EI_DESHADEWHITERLMIN = 0x1224
   DTWAIN_EI_DESHADEWHITERLAVE = 0x1225
   DTWAIN_EI_DESHADEWHITERLMAX = 0x1226
   DTWAIN_EI_BLACKSPECKLESREMOVED = 0x1227
   DTWAIN_EI_WHITESPECKLESREMOVED = 0x1228
   DTWAIN_EI_HORZLINECOUNT = 0x1229
   DTWAIN_EI_VERTLINECOUNT = 0x122A
   DTWAIN_EI_DESKEWSTATUS = 0x122B
   DTWAIN_EI_SKEWORIGINALANGLE = 0x122C
   DTWAIN_EI_SKEWFINALANGLE = 0x122D
   DTWAIN_EI_SKEWCONFIDENCE = 0x122E
   DTWAIN_EI_SKEWWINDOWX1 = 0x122F
   DTWAIN_EI_SKEWWINDOWY1 = 0x1230
   DTWAIN_EI_SKEWWINDOWX2 = 0x1231
   DTWAIN_EI_SKEWWINDOWY2 = 0x1232
   DTWAIN_EI_SKEWWINDOWX3 = 0x1233
   DTWAIN_EI_SKEWWINDOWY3 = 0x1234
   DTWAIN_EI_SKEWWINDOWX4 = 0x1235
   DTWAIN_EI_SKEWWINDOWY4 = 0x1236
   DTWAIN_EI_BOOKNAME = 0x1238
   DTWAIN_EI_CHAPTERNUMBER = 0x1239
   DTWAIN_EI_DOCUMENTNUMBER = 0x123A
   DTWAIN_EI_PAGENUMBER = 0x123B
   DTWAIN_EI_CAMERA = 0x123C
   DTWAIN_EI_FRAMENUMBER = 0x123D
   DTWAIN_EI_FRAME = 0x123E
   DTWAIN_EI_PIXELFLAVOR = 0x123F
   DTWAIN_EI_ICCPROFILE = 0x1240
   DTWAIN_EI_LASTSEGMENT = 0x1241
   DTWAIN_EI_SEGMENTNUMBER = 0x1242
   DTWAIN_EI_MAGDATA = 0x1243
   DTWAIN_EI_MAGTYPE = 0x1244
   DTWAIN_EI_PAGESIDE = 0x1245
   DTWAIN_EI_FILESYSTEMSOURCE = 0x1246
   DTWAIN_EI_IMAGEMERGED = 0x1247
   DTWAIN_EI_MAGDATALENGTH = 0x1248
   DTWAIN_EI_PAPERCOUNT = 0x1249
   DTWAIN_EI_PRINTERTEXT = 0x124A
   DTWAIN_EI_TWAINDIRECTMETADATA = 0x124B
   DTWAIN_EI_IAFIELDA_VALUE = 0x124C
   DTWAIN_EI_IAFIELDB_VALUE = 0x124D
   DTWAIN_EI_IAFIELDC_VALUE = 0x124E
   DTWAIN_EI_IAFIELDD_VALUE = 0x124F
   DTWAIN_EI_IAFIELDE_VALUE = 0x1250
   DTWAIN_EI_IALEVEL = 0x1251
   DTWAIN_EI_PRINTER = 0x1252
   DTWAIN_EI_BARCODETEXT2 = 0x1253
   DTWAIN_LOG_DECODE_SOURCE = 0x1      
   DTWAIN_LOG_DECODE_DEST = 0x2        
   DTWAIN_LOG_DECODE_TWMEMREF = 0x4    
   DTWAIN_LOG_DECODE_TWEVENT = 0x8     
   DTWAIN_LOG_CALLSTACK = 0x10         
   DTWAIN_LOG_ISTWAINMSG = 0x20        
   DTWAIN_LOG_INITFAILURE = 0x40       
   DTWAIN_LOG_LOWLEVELTWAIN = 0x80     
   DTWAIN_LOG_DECODE_BITMAP = 0x100    
   DTWAIN_LOG_NOTIFICATIONS = 0x200    
   DTWAIN_LOG_MISCELLANEOUS = 0x400    
   DTWAIN_LOG_DTWAINERRORS = 0x800     
   DTWAIN_LOG_USEFILE = 0x10000        
   DTWAIN_LOG_SHOWEXCEPTIONS = 0x20000 
   DTWAIN_LOG_ERRORMSGBOX = 0x40000    
   DTWAIN_LOG_USEBUFFER = 0x80000      
   DTWAIN_LOG_FILEAPPEND = 0x100000    
   DTWAIN_LOG_USECALLBACK = 0x200000   
   DTWAIN_LOG_USECRLF = 0x400000       
   DTWAIN_LOG_CONSOLE = 0x800000       
   DTWAIN_LOG_DEBUGMONITOR = 0x1000000 
   DTWAIN_LOG_USEWINDOW = 0x2000000    
   DTWAIN_LOG_CREATEDIRECTORY = 0x04000000
   DTWAIN_LOG_CONSOLEWITHHANDLER = (0x08000000 | DTWAIN_LOG_CONSOLE)
   DTWAIN_LOG_ALL = (DTWAIN_LOG_DECODE_SOURCE | DTWAIN_LOG_DECODE_DEST | DTWAIN_LOG_DECODE_TWEVENT | DTWAIN_LOG_DECODE_TWMEMREF | DTWAIN_LOG_CALLSTACK | DTWAIN_LOG_ISTWAINMSG | DTWAIN_LOG_INITFAILURE | DTWAIN_LOG_LOWLEVELTWAIN | DTWAIN_LOG_NOTIFICATIONS | DTWAIN_LOG_MISCELLANEOUS | DTWAIN_LOG_DTWAINERRORS | DTWAIN_LOG_DECODE_BITMAP)
   DTWAIN_LOG_ALL_APPEND = 0xFFFFFFFF
   DTWAIN_TEMPDIR_CREATEDIRECTORY = DTWAIN_LOG_CREATEDIRECTORY
   DTWAINGCD_RETURNHANDLE = 1
   DTWAINGCD_COPYDATA = 2
   DTWAIN_BYPOSITION = 0
   DTWAIN_BYID = 1
   DTWAINSCD_USEHANDLE = 1
   DTWAINSCD_USEDATA = 2
   DTWAIN_PAGEFAIL_RETRY = 1
   DTWAIN_PAGEFAIL_TERMINATE = 2
   DTWAIN_MAXRETRY_ATTEMPTS = 3
   DTWAIN_RETRY_FOREVER = (-1)
   DTWAIN_PDF_NOSCALING = 128
   DTWAIN_PDF_FITPAGE = 256
   DTWAIN_PDF_VARIABLEPAGESIZE = 512
   DTWAIN_PDF_CUSTOMSIZE = 1024
   DTWAIN_PDF_USECOMPRESSION = 2048
   DTWAIN_PDF_CUSTOMSCALE = 4096
   DTWAIN_PDF_PIXELSPERMETERSIZE = 8192
   DTWAIN_PDF_ALLOWPRINTING = 2052
   DTWAIN_PDF_ALLOWMOD = 8
   DTWAIN_PDF_ALLOWCOPY = 16
   DTWAIN_PDF_ALLOWMODANNOTATIONS = 32
   DTWAIN_PDF_ALLOWFILLIN = 256
   DTWAIN_PDF_ALLOWEXTRACTION = 512
   DTWAIN_PDF_ALLOWASSEMBLY = 1024
   DTWAIN_PDF_ALLOWDEGRADEDPRINTING = 4
   DTWAIN_PDF_ALLOWALL = 0xFFFFFFFC
   DTWAIN_PDF_ALLOWANYMOD = (DTWAIN_PDF_ALLOWMOD | DTWAIN_PDF_ALLOWFILLIN | DTWAIN_PDF_ALLOWMODANNOTATIONS | DTWAIN_PDF_ALLOWASSEMBLY)
   DTWAIN_PDF_ALLOWANYPRINTING = (DTWAIN_PDF_ALLOWPRINTING | DTWAIN_PDF_ALLOWDEGRADEDPRINTING)
   DTWAIN_PDF_PORTRAIT = 0
   DTWAIN_PDF_LANDSCAPE = 1
   DTWAIN_PS_REGULAR = 0
   DTWAIN_PS_ENCAPSULATED = 1
   DTWAIN_BP_AUTODISCARD_NONE = 0
   DTWAIN_BP_AUTODISCARD_IMMEDIATE = 1
   DTWAIN_BP_AUTODISCARD_AFTERPROCESS = 2
   DTWAIN_BP_DETECTORIGINAL = 1
   DTWAIN_BP_DETECTADJUSTED = 2
   DTWAIN_BP_DETECTALL = (DTWAIN_BP_DETECTORIGINAL | DTWAIN_BP_DETECTADJUSTED)
   DTWAIN_BP_DISABLE = (-2)
   DTWAIN_BP_AUTO = (-1)
   DTWAIN_BP_AUTODISCARD_ANY = 0xFFFF
   DTWAIN_LP_REFLECTIVE = 0
   DTWAIN_LP_TRANSMISSIVE = 1
   DTWAIN_LS_RED = 0
   DTWAIN_LS_GREEN = 1
   DTWAIN_LS_BLUE = 2
   DTWAIN_LS_NONE = 3
   DTWAIN_LS_WHITE = 4
   DTWAIN_LS_UV = 5
   DTWAIN_LS_IR = 6
   DTWAIN_DLG_SORTNAMES = 1
   DTWAIN_DLG_CENTER = 2
   DTWAIN_DLG_CENTER_SCREEN = 4
   DTWAIN_DLG_USETEMPLATE = 8
   DTWAIN_DLG_CLEAR_PARAMS = 16
   DTWAIN_DLG_HORIZONTALSCROLL = 32
   DTWAIN_DLG_USEINCLUDENAMES = 64
   DTWAIN_DLG_USEEXCLUDENAMES = 128
   DTWAIN_DLG_USENAMEMAPPING = 256
   DTWAIN_DLG_TOPMOSTWINDOW = 1024
   DTWAIN_DLG_OPENONSELECT = 2048
   DTWAIN_DLG_NOOPENONSELECT = 4096
   DTWAIN_DLG_HIGHLIGHTFIRST = 8192
   DTWAIN_DLG_SAVELASTSCREENPOS = 16384
   DTWAIN_RES_ENGLISH = 0
   DTWAIN_RES_FRENCH = 1
   DTWAIN_RES_SPANISH = 2
   DTWAIN_RES_DUTCH = 3
   DTWAIN_RES_GERMAN = 4
   DTWAIN_RES_ITALIAN = 5
   DTWAIN_AL_ALARM = 0
   DTWAIN_AL_FEEDERERROR = 1
   DTWAIN_AL_FEEDERWARNING = 2
   DTWAIN_AL_BARCODE = 3
   DTWAIN_AL_DOUBLEFEED = 4
   DTWAIN_AL_JAM = 5
   DTWAIN_AL_PATCHCODE = 6
   DTWAIN_AL_POWER = 7
   DTWAIN_AL_SKEW = 8
   DTWAIN_FT_CAMERA = 0
   DTWAIN_FT_CAMERATOP = 1
   DTWAIN_FT_CAMERABOTTOM = 2
   DTWAIN_FT_CAMERAPREVIEW = 3
   DTWAIN_FT_DOMAIN = 4
   DTWAIN_FT_HOST = 5
   DTWAIN_FT_DIRECTORY = 6
   DTWAIN_FT_IMAGE = 7
   DTWAIN_FT_UNKNOWN = 8
   DTWAIN_NF_NONE = 0
   DTWAIN_NF_AUTO = 1
   DTWAIN_NF_LONEPIXEL = 2
   DTWAIN_NF_MAJORITYRULE = 3
   DTWAIN_CB_AUTO = 0
   DTWAIN_CB_CLEAR = 1
   DTWAIN_CB_NOCLEAR = 2
   DTWAIN_FA_NONE = 0
   DTWAIN_FA_LEFT = 1
   DTWAIN_FA_CENTER = 2
   DTWAIN_FA_RIGHT = 3
   DTWAIN_PF_CHOCOLATE = 0
   DTWAIN_PF_VANILLA = 1
   DTWAIN_FO_FIRSTPAGEFIRST = 0
   DTWAIN_FO_LASTPAGEFIRST = 1
   DTWAIN_INCREMENT_STATIC = 0
   DTWAIN_INCREMENT_DYNAMIC = 1
   DTWAIN_INCREMENT_DEFAULT = -1
   DTWAIN_MANDUP_FACEUPTOPPAGE = 0
   DTWAIN_MANDUP_FACEUPBOTTOMPAGE = 1
   DTWAIN_MANDUP_FACEDOWNTOPPAGE = 2
   DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE = 3
   DTWAIN_FILESAVE_DEFAULT = 0
   DTWAIN_FILESAVE_UICLOSE = 1
   DTWAIN_FILESAVE_SOURCECLOSE = 2
   DTWAIN_FILESAVE_ENDACQUIRE = 3
   DTWAIN_FILESAVE_MANUALSAVE = 4
   DTWAIN_FILESAVE_SAVEINCOMPLETE = 128
   DTWAIN_MANDUP_SCANOK = 1
   DTWAIN_MANDUP_SIDE1RESCAN = 2
   DTWAIN_MANDUP_SIDE2RESCAN = 3
   DTWAIN_MANDUP_RESCANALL = 4
   DTWAIN_MANDUP_PAGEMISSING = 5
   DTWAIN_DEMODLL_VERSION = 0x00000001
   DTWAIN_UNLICENSED_VERSION = 0x00000002
   DTWAIN_COMPANY_VERSION = 0x00000004
   DTWAIN_GENERAL_VERSION = 0x00000008
   DTWAIN_DEVELOP_VERSION = 0x00000010
   DTWAIN_JAVA_VERSION = 0x00000020
   DTWAIN_TOOLKIT_VERSION = 0x00000040
   DTWAIN_LIMITEDDLL_VERSION = 0x00000080
   DTWAIN_STATICLIB_VERSION = 0x00000100
   DTWAIN_STATICLIB_STDCALL_VERSION = 0x00000200
   DTWAIN_PDF_VERSION = 0x00010000
   DTWAIN_TWAINSAVE_VERSION = 0x00020000
   DTWAIN_OCR_VERSION = 0x00040000
   DTWAIN_BARCODE_VERSION = 0x00080000
   DTWAIN_ACTIVEX_VERSION = 0x00100000
   DTWAIN_32BIT_VERSION = 0x00200000
   DTWAIN_64BIT_VERSION = 0x00400000
   DTWAIN_UNICODE_VERSION = 0x00800000
   DTWAIN_OPENSOURCE_VERSION = 0x01000000
   DTWAIN_CALLSTACK_LOGGING = 0x02000000
   DTWAIN_CALLSTACK_LOGGING_PLUS = 0x04000000
   DTWAINOCR_RETURNHANDLE = 1
   DTWAINOCR_COPYDATA = 2
   DTWAIN_OCRINFO_CHAR = 0
   DTWAIN_OCRINFO_CHARXPOS = 1
   DTWAIN_OCRINFO_CHARYPOS = 2
   DTWAIN_OCRINFO_CHARXWIDTH = 3
   DTWAIN_OCRINFO_CHARYWIDTH = 4
   DTWAIN_OCRINFO_CHARCONFIDENCE = 5
   DTWAIN_OCRINFO_PAGENUM = 6
   DTWAIN_OCRINFO_OCRENGINE = 7
   DTWAIN_OCRINFO_TEXTLENGTH = 8
   DTWAIN_PDFPAGETYPE_COLOR = 0
   DTWAIN_PDFPAGETYPE_BW = 1
   DTWAIN_TWAINDSM_LEGACY = 1
   DTWAIN_TWAINDSM_VERSION2 = 2
   DTWAIN_TWAINDSM_LATESTVERSION = 4
   DTWAIN_TWAINDSMSEARCH_NOTFOUND = (-1)
   DTWAIN_TWAINDSMSEARCH_WSO = 0
   DTWAIN_TWAINDSMSEARCH_WOS = 1
   DTWAIN_TWAINDSMSEARCH_SWO = 2
   DTWAIN_TWAINDSMSEARCH_SOW = 3
   DTWAIN_TWAINDSMSEARCH_OWS = 4
   DTWAIN_TWAINDSMSEARCH_OSW = 5
   DTWAIN_TWAINDSMSEARCH_W = 6
   DTWAIN_TWAINDSMSEARCH_S = 7
   DTWAIN_TWAINDSMSEARCH_O = 8
   DTWAIN_TWAINDSMSEARCH_WS = 9
   DTWAIN_TWAINDSMSEARCH_WO = 10
   DTWAIN_TWAINDSMSEARCH_SW = 11
   DTWAIN_TWAINDSMSEARCH_SO = 12
   DTWAIN_TWAINDSMSEARCH_OW = 13
   DTWAIN_TWAINDSMSEARCH_OS = 14
   DTWAIN_TWAINDSMSEARCH_C = 15
   DTWAIN_TWAINDSMSEARCH_U = 16
   DTWAIN_PDFPOLARITY_POSITIVE = 1
   DTWAIN_PDFPOLARITY_NEGATIVE = 2
   DTWAIN_TWPF_NORMAL = 0
   DTWAIN_TWPF_BOLD = 1
   DTWAIN_TWPF_ITALIC = 2
   DTWAIN_TWPF_LARGESIZE = 3
   DTWAIN_TWPF_SMALLSIZE = 4
   DTWAIN_TWCT_PAGE = 0
   DTWAIN_TWCT_PATCH1 = 1
   DTWAIN_TWCT_PATCH2 = 2
   DTWAIN_TWCT_PATCH3 = 3
   DTWAIN_TWCT_PATCH4 = 4
   DTWAIN_TWCT_PATCH5 = 5
   DTWAIN_TWCT_PATCH6 = 6
   DTWAIN_AUTOSIZE_NONE = 0
   DTWAIN_CV_CAPCUSTOMBASE = 0x8000
   DTWAIN_CV_CAPXFERCOUNT = 0x0001
   DTWAIN_CV_ICAPCOMPRESSION = 0x0100
   DTWAIN_CV_ICAPPIXELTYPE = 0x0101
   DTWAIN_CV_ICAPUNITS = 0x0102
   DTWAIN_CV_ICAPXFERMECH = 0x0103
   DTWAIN_CV_CAPAUTHOR = 0x1000
   DTWAIN_CV_CAPCAPTION = 0x1001
   DTWAIN_CV_CAPFEEDERENABLED = 0x1002
   DTWAIN_CV_CAPFEEDERLOADED = 0x1003
   DTWAIN_CV_CAPTIMEDATE = 0x1004
   DTWAIN_CV_CAPSUPPORTEDCAPS = 0x1005
   DTWAIN_CV_CAPEXTENDEDCAPS = 0x1006
   DTWAIN_CV_CAPAUTOFEED = 0x1007
   DTWAIN_CV_CAPCLEARPAGE = 0x1008
   DTWAIN_CV_CAPFEEDPAGE = 0x1009
   DTWAIN_CV_CAPREWINDPAGE = 0x100a
   DTWAIN_CV_CAPINDICATORS = 0x100b
   DTWAIN_CV_CAPSUPPORTEDCAPSEXT = 0x100c
   DTWAIN_CV_CAPPAPERDETECTABLE = 0x100d
   DTWAIN_CV_CAPUICONTROLLABLE = 0x100e
   DTWAIN_CV_CAPDEVICEONLINE = 0x100f
   DTWAIN_CV_CAPAUTOSCAN = 0x1010
   DTWAIN_CV_CAPTHUMBNAILSENABLED = 0x1011
   DTWAIN_CV_CAPDUPLEX = 0x1012
   DTWAIN_CV_CAPDUPLEXENABLED = 0x1013
   DTWAIN_CV_CAPENABLEDSUIONLY = 0x1014
   DTWAIN_CV_CAPCUSTOMDSDATA = 0x1015
   DTWAIN_CV_CAPENDORSER = 0x1016
   DTWAIN_CV_CAPJOBCONTROL = 0x1017
   DTWAIN_CV_CAPALARMS = 0x1018
   DTWAIN_CV_CAPALARMVOLUME = 0x1019
   DTWAIN_CV_CAPAUTOMATICCAPTURE = 0x101a
   DTWAIN_CV_CAPTIMEBEFOREFIRSTCAPTURE = 0x101b
   DTWAIN_CV_CAPTIMEBETWEENCAPTURES = 0x101c
   DTWAIN_CV_CAPCLEARBUFFERS = 0x101d
   DTWAIN_CV_CAPMAXBATCHBUFFERS = 0x101e
   DTWAIN_CV_CAPDEVICETIMEDATE = 0x101f
   DTWAIN_CV_CAPPOWERSUPPLY = 0x1020
   DTWAIN_CV_CAPCAMERAPREVIEWUI = 0x1021
   DTWAIN_CV_CAPDEVICEEVENT = 0x1022
   DTWAIN_CV_CAPPAGEMULTIPLEACQUIRE = 0x1023
   DTWAIN_CV_CAPSERIALNUMBER = 0x1024
   DTWAIN_CV_CAPFILESYSTEM = 0x1025
   DTWAIN_CV_CAPPRINTER = 0x1026
   DTWAIN_CV_CAPPRINTERENABLED = 0x1027
   DTWAIN_CV_CAPPRINTERINDEX = 0x1028
   DTWAIN_CV_CAPPRINTERMODE = 0x1029
   DTWAIN_CV_CAPPRINTERSTRING = 0x102a
   DTWAIN_CV_CAPPRINTERSUFFIX = 0x102b
   DTWAIN_CV_CAPLANGUAGE = 0x102c
   DTWAIN_CV_CAPFEEDERALIGNMENT = 0x102d
   DTWAIN_CV_CAPFEEDERORDER = 0x102e
   DTWAIN_CV_CAPPAPERBINDING = 0x102f
   DTWAIN_CV_CAPREACQUIREALLOWED = 0x1030
   DTWAIN_CV_CAPPASSTHRU = 0x1031
   DTWAIN_CV_CAPBATTERYMINUTES = 0x1032
   DTWAIN_CV_CAPBATTERYPERCENTAGE = 0x1033
   DTWAIN_CV_CAPPOWERDOWNTIME = 0x1034
   DTWAIN_CV_CAPSEGMENTED = 0x1035
   DTWAIN_CV_CAPCAMERAENABLED = 0x1036
   DTWAIN_CV_CAPCAMERAORDER = 0x1037
   DTWAIN_CV_CAPMICRENABLED = 0x1038
   DTWAIN_CV_CAPFEEDERPREP = 0x1039
   DTWAIN_CV_CAPFEEDERPOCKET = 0x103a
   DTWAIN_CV_CAPAUTOMATICSENSEMEDIUM = 0x103b
   DTWAIN_CV_CAPCUSTOMINTERFACEGUID = 0x103c
   DTWAIN_CV_CAPSUPPORTEDCAPSSEGMENTUNIQUE = 0x103d
   DTWAIN_CV_CAPSUPPORTEDDATS = 0x103e
   DTWAIN_CV_CAPDOUBLEFEEDDETECTION = 0x103f
   DTWAIN_CV_CAPDOUBLEFEEDDETECTIONLENGTH = 0x1040
   DTWAIN_CV_CAPDOUBLEFEEDDETECTIONSENSITIVITY = 0x1041
   DTWAIN_CV_CAPDOUBLEFEEDDETECTIONRESPONSE = 0x1042
   DTWAIN_CV_CAPPAPERHANDLING = 0x1043
   DTWAIN_CV_CAPINDICATORSMODE = 0x1044
   DTWAIN_CV_CAPPRINTERVERTICALOFFSET = 0x1045
   DTWAIN_CV_CAPPOWERSAVETIME = 0x1046
   DTWAIN_CV_CAPPRINTERCHARROTATION = 0x1047
   DTWAIN_CV_CAPPRINTERFONTSTYLE = 0x1048
   DTWAIN_CV_CAPPRINTERINDEXLEADCHAR = 0x1049
   DTWAIN_CV_CAPIMAGEADDRESSENABLED = 0x1050
   DTWAIN_CV_CAPIAFIELDA_LEVEL = 0x1051
   DTWAIN_CV_CAPIAFIELDB_LEVEL = 0x1052
   DTWAIN_CV_CAPIAFIELDC_LEVEL = 0x1053
   DTWAIN_CV_CAPIAFIELDD_LEVEL = 0x1054
   DTWAIN_CV_CAPIAFIELDE_LEVEL = 0x1055
   DTWAIN_CV_CAPIAFIELDA_PRINTFORMAT = 0x1056
   DTWAIN_CV_CAPIAFIELDB_PRINTFORMAT = 0x1057
   DTWAIN_CV_CAPIAFIELDC_PRINTFORMAT = 0x1058
   DTWAIN_CV_CAPIAFIELDD_PRINTFORMAT = 0x1059
   DTWAIN_CV_CAPIAFIELDE_PRINTFORMAT = 0x105A
   DTWAIN_CV_CAPIAFIELDA_VALUE = 0x105B
   DTWAIN_CV_CAPIAFIELDB_VALUE = 0x105C
   DTWAIN_CV_CAPIAFIELDC_VALUE = 0x105D
   DTWAIN_CV_CAPIAFIELDD_VALUE = 0x105E
   DTWAIN_CV_CAPIAFIELDE_VALUE = 0x105F
   DTWAIN_CV_CAPIAFIELDA_LASTPAGE = 0x1060
   DTWAIN_CV_CAPIAFIELDB_LASTPAGE = 0x1061
   DTWAIN_CV_CAPIAFIELDC_LASTPAGE = 0x1062
   DTWAIN_CV_CAPIAFIELDD_LASTPAGE = 0x1063
   DTWAIN_CV_CAPIAFIELDE_LASTPAGE = 0x1064
   DTWAIN_CV_CAPPRINTERINDEXMAXVALUE = 0x104A
   DTWAIN_CV_CAPPRINTERINDEXNUMDIGITS = 0x104B
   DTWAIN_CV_CAPPRINTERINDEXSTEP = 0x104C
   DTWAIN_CV_CAPPRINTERINDEXTRIGGER = 0x104D
   DTWAIN_CV_CAPPRINTERSTRINGPREVIEW = 0x104E
   DTWAIN_CV_ICAPAUTOBRIGHT = 0x1100
   DTWAIN_CV_ICAPBRIGHTNESS = 0x1101
   DTWAIN_CV_ICAPCONTRAST = 0x1103
   DTWAIN_CV_ICAPCUSTHALFTONE = 0x1104
   DTWAIN_CV_ICAPEXPOSURETIME = 0x1105
   DTWAIN_CV_ICAPFILTER = 0x1106
   DTWAIN_CV_ICAPFLASHUSED = 0x1107
   DTWAIN_CV_ICAPGAMMA = 0x1108
   DTWAIN_CV_ICAPHALFTONES = 0x1109
   DTWAIN_CV_ICAPHIGHLIGHT = 0x110a
   DTWAIN_CV_ICAPIMAGEFILEFORMAT = 0x110c
   DTWAIN_CV_ICAPLAMPSTATE = 0x110d
   DTWAIN_CV_ICAPLIGHTSOURCE = 0x110e
   DTWAIN_CV_ICAPORIENTATION = 0x1110
   DTWAIN_CV_ICAPPHYSICALWIDTH = 0x1111
   DTWAIN_CV_ICAPPHYSICALHEIGHT = 0x1112
   DTWAIN_CV_ICAPSHADOW = 0x1113
   DTWAIN_CV_ICAPFRAMES = 0x1114
   DTWAIN_CV_ICAPXNATIVERESOLUTION = 0x1116
   DTWAIN_CV_ICAPYNATIVERESOLUTION = 0x1117
   DTWAIN_CV_ICAPXRESOLUTION = 0x1118
   DTWAIN_CV_ICAPYRESOLUTION = 0x1119
   DTWAIN_CV_ICAPMAXFRAMES = 0x111a
   DTWAIN_CV_ICAPTILES = 0x111b
   DTWAIN_CV_ICAPBITORDER = 0x111c
   DTWAIN_CV_ICAPCCITTKFACTOR = 0x111d
   DTWAIN_CV_ICAPLIGHTPATH = 0x111e
   DTWAIN_CV_ICAPPIXELFLAVOR = 0x111f
   DTWAIN_CV_ICAPPLANARCHUNKY = 0x1120
   DTWAIN_CV_ICAPROTATION = 0x1121
   DTWAIN_CV_ICAPSUPPORTEDSIZES = 0x1122
   DTWAIN_CV_ICAPTHRESHOLD = 0x1123
   DTWAIN_CV_ICAPXSCALING = 0x1124
   DTWAIN_CV_ICAPYSCALING = 0x1125
   DTWAIN_CV_ICAPBITORDERCODES = 0x1126
   DTWAIN_CV_ICAPPIXELFLAVORCODES = 0x1127
   DTWAIN_CV_ICAPJPEGPIXELTYPE = 0x1128
   DTWAIN_CV_ICAPTIMEFILL = 0x112a
   DTWAIN_CV_ICAPBITDEPTH = 0x112b
   DTWAIN_CV_ICAPBITDEPTHREDUCTION = 0x112c
   DTWAIN_CV_ICAPUNDEFINEDIMAGESIZE = 0x112d
   DTWAIN_CV_ICAPIMAGEDATASET = 0x112e
   DTWAIN_CV_ICAPEXTIMAGEINFO = 0x112f
   DTWAIN_CV_ICAPMINIMUMHEIGHT = 0x1130
   DTWAIN_CV_ICAPMINIMUMWIDTH = 0x1131
   DTWAIN_CV_ICAPAUTOBORDERDETECTION = 0x1132
   DTWAIN_CV_ICAPAUTODESKEW = 0x1133
   DTWAIN_CV_ICAPAUTODISCARDBLANKPAGES = 0x1134
   DTWAIN_CV_ICAPAUTOROTATE = 0x1135
   DTWAIN_CV_ICAPFLIPROTATION = 0x1136
   DTWAIN_CV_ICAPBARCODEDETECTIONENABLED = 0x1137
   DTWAIN_CV_ICAPSUPPORTEDBARCODETYPES = 0x1138
   DTWAIN_CV_ICAPBARCODEMAXSEARCHPRIORITIES = 0x1139
   DTWAIN_CV_ICAPBARCODESEARCHPRIORITIES = 0x113a
   DTWAIN_CV_ICAPBARCODESEARCHMODE = 0x113b
   DTWAIN_CV_ICAPBARCODEMAXRETRIES = 0x113c
   DTWAIN_CV_ICAPBARCODETIMEOUT = 0x113d
   DTWAIN_CV_ICAPZOOMFACTOR = 0x113e
   DTWAIN_CV_ICAPPATCHCODEDETECTIONENABLED = 0x113f
   DTWAIN_CV_ICAPSUPPORTEDPATCHCODETYPES = 0x1140
   DTWAIN_CV_ICAPPATCHCODEMAXSEARCHPRIORITIES = 0x1141
   DTWAIN_CV_ICAPPATCHCODESEARCHPRIORITIES = 0x1142
   DTWAIN_CV_ICAPPATCHCODESEARCHMODE = 0x1143
   DTWAIN_CV_ICAPPATCHCODEMAXRETRIES = 0x1144
   DTWAIN_CV_ICAPPATCHCODETIMEOUT = 0x1145
   DTWAIN_CV_ICAPFLASHUSED2 = 0x1146
   DTWAIN_CV_ICAPIMAGEFILTER = 0x1147
   DTWAIN_CV_ICAPNOISEFILTER = 0x1148
   DTWAIN_CV_ICAPOVERSCAN = 0x1149
   DTWAIN_CV_ICAPAUTOMATICBORDERDETECTION = 0x1150
   DTWAIN_CV_ICAPAUTOMATICDESKEW = 0x1151
   DTWAIN_CV_ICAPAUTOMATICROTATE = 0x1152
   DTWAIN_CV_ICAPJPEGQUALITY = 0x1153
   DTWAIN_CV_ICAPFEEDERTYPE = 0x1154
   DTWAIN_CV_ICAPICCPROFILE = 0x1155
   DTWAIN_CV_ICAPAUTOSIZE = 0x1156
   DTWAIN_CV_ICAPAUTOMATICCROPUSESFRAME = 0x1157
   DTWAIN_CV_ICAPAUTOMATICLENGTHDETECTION = 0x1158
   DTWAIN_CV_ICAPAUTOMATICCOLORENABLED = 0x1159
   DTWAIN_CV_ICAPAUTOMATICCOLORNONCOLORPIXELTYPE = 0x115a
   DTWAIN_CV_ICAPCOLORMANAGEMENTENABLED = 0x115b
   DTWAIN_CV_ICAPIMAGEMERGE = 0x115c
   DTWAIN_CV_ICAPIMAGEMERGEHEIGHTTHRESHOLD = 0x115d
   DTWAIN_CV_ICAPSUPPORTEDEXTIMAGEINFO = 0x115e
   DTWAIN_CV_ICAPFILMTYPE = 0x115f
   DTWAIN_CV_ICAPMIRROR = 0x1160
   DTWAIN_CV_ICAPJPEGSUBSAMPLING = 0x1161
   DTWAIN_CV_ACAPAUDIOFILEFORMAT = 0x1201
   DTWAIN_CV_ACAPXFERMECH = 0x1202
   DTWAIN_CFMCV_CAPCFMSTART = 2048
   DTWAIN_CFMCV_CAPDUPLEXSCANNER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+10)
   DTWAIN_CFMCV_CAPDUPLEXENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+11)
   DTWAIN_CFMCV_CAPSCANNERNAME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+12)
   DTWAIN_CFMCV_CAPSINGLEPASS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+13)
   DTWAIN_CFMCV_CAPERRHANDLING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+20)
   DTWAIN_CFMCV_CAPFEEDERSTATUS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+21)
   DTWAIN_CFMCV_CAPFEEDMEDIUMWAIT = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+22)
   DTWAIN_CFMCV_CAPFEEDWAITTIME = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+23)
   DTWAIN_CFMCV_ICAPWHITEBALANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+24)
   DTWAIN_CFMCV_ICAPAUTOBINARY = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+25)
   DTWAIN_CFMCV_ICAPIMAGESEPARATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+26)
   DTWAIN_CFMCV_ICAPHARDWARECOMPRESSION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+27)
   DTWAIN_CFMCV_ICAPIMAGEEMPHASIS = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+28)
   DTWAIN_CFMCV_ICAPOUTLINING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+29)
   DTWAIN_CFMCV_ICAPDYNTHRESHOLD = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+30)
   DTWAIN_CFMCV_ICAPVARIANCE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+31)
   DTWAIN_CFMCV_CAPENDORSERAVAILABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+32)
   DTWAIN_CFMCV_CAPENDORSERENABLE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+33)
   DTWAIN_CFMCV_CAPENDORSERCHARSET = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+34)
   DTWAIN_CFMCV_CAPENDORSERSTRINGLENGTH = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+35)
   DTWAIN_CFMCV_CAPENDORSERSTRING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+36)
   DTWAIN_CFMCV_ICAPDYNTHRESHOLDCURVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+48)
   DTWAIN_CFMCV_ICAPSMOOTHINGMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+49)
   DTWAIN_CFMCV_ICAPFILTERMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+50)
   DTWAIN_CFMCV_ICAPGRADATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+51)
   DTWAIN_CFMCV_ICAPMIRROR = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+52)
   DTWAIN_CFMCV_ICAPEASYSCANMODE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+53)
   DTWAIN_CFMCV_ICAPSOFTWAREINTERPOLATION = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+54)
   DTWAIN_CFMCV_ICAPIMAGESEPARATIONEX = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+55)
   DTWAIN_CFMCV_CAPDUPLEXPAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+56)
   DTWAIN_CFMCV_ICAPINVERTIMAGE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+57)
   DTWAIN_CFMCV_ICAPSPECKLEREMOVE = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+58)
   DTWAIN_CFMCV_ICAPUSMFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+59)
   DTWAIN_CFMCV_ICAPNOISEFILTERCFM = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+60)
   DTWAIN_CFMCV_ICAPDESCREENING = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+61)
   DTWAIN_CFMCV_ICAPQUALITYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+62)
   DTWAIN_CFMCV_ICAPBINARYFILTER = (DTWAIN_CV_CAPCUSTOMBASE+DTWAIN_CFMCV_CAPCFMSTART+63)
   DTWAIN_OCRCV_IMAGEFILEFORMAT = 0x1000
   DTWAIN_OCRCV_DESKEW = 0x1001
   DTWAIN_OCRCV_DESHADE = 0x1002
   DTWAIN_OCRCV_ORIENTATION = 0x1003
   DTWAIN_OCRCV_NOISEREMOVE = 0x1004
   DTWAIN_OCRCV_LINEREMOVE = 0x1005
   DTWAIN_OCRCV_INVERTPAGE = 0x1006
   DTWAIN_OCRCV_INVERTZONES = 0x1007
   DTWAIN_OCRCV_LINEREJECT = 0x1008
   DTWAIN_OCRCV_CHARACTERREJECT = 0x1009
   DTWAIN_OCRCV_ERRORREPORTMODE = 0x1010
   DTWAIN_OCRCV_ERRORREPORTFILE = 0x1011
   DTWAIN_OCRCV_PIXELTYPE = 0x1012
   DTWAIN_OCRCV_BITDEPTH = 0x1013
   DTWAIN_OCRCV_RETURNCHARINFO = 0x1014
   DTWAIN_OCRCV_NATIVEFILEFORMAT = 0x1015
   DTWAIN_OCRCV_MPNATIVEFILEFORMAT = 0x1016
   DTWAIN_OCRCV_SUPPORTEDCAPS = 0x1017
   DTWAIN_OCRCV_DISABLECHARACTERS = 0x1018
   DTWAIN_OCRCV_REMOVECONTROLCHARS = 0x1019
   DTWAIN_OCRORIENT_OFF = 0
   DTWAIN_OCRORIENT_AUTO = 1
   DTWAIN_OCRORIENT_90 = 2
   DTWAIN_OCRORIENT_180 = 3
   DTWAIN_OCRORIENT_270 = 4
   DTWAIN_OCRIMAGEFORMAT_AUTO = 10000
   DTWAIN_OCRERROR_MODENONE = 0
   DTWAIN_OCRERROR_SHOWMSGBOX = 1
   DTWAIN_OCRERROR_WRITEFILE = 2
   DTWAIN_PDFTEXT_ALLPAGES = 0x00000001
   DTWAIN_PDFTEXT_EVENPAGES = 0x00000002
   DTWAIN_PDFTEXT_ODDPAGES = 0x00000004
   DTWAIN_PDFTEXT_FIRSTPAGE = 0x00000008
   DTWAIN_PDFTEXT_LASTPAGE = 0x00000010
   DTWAIN_PDFTEXT_CURRENTPAGE = 0x00000020
   DTWAIN_PDFTEXT_DISABLED = 0x00000040
   DTWAIN_PDFTEXT_TOPLEFT = 0x00000100
   DTWAIN_PDFTEXT_TOPRIGHT = 0x00000200
   DTWAIN_PDFTEXT_HORIZCENTER = 0x00000400
   DTWAIN_PDFTEXT_VERTCENTER = 0x00000800
   DTWAIN_PDFTEXT_BOTTOMLEFT = 0x00001000
   DTWAIN_PDFTEXT_BOTTOMRIGHT = 0x00002000
   DTWAIN_PDFTEXT_BOTTOMCENTER = 0x00004000
   DTWAIN_PDFTEXT_TOPCENTER = 0x00008000
   DTWAIN_PDFTEXT_XCENTER = 0x00010000
   DTWAIN_PDFTEXT_YCENTER = 0x00020000
   DTWAIN_PDFTEXT_NOSCALING = 0x00100000
   DTWAIN_PDFTEXT_NOCHARSPACING = 0x00200000
   DTWAIN_PDFTEXT_NOWORDSPACING = 0x00400000
   DTWAIN_PDFTEXT_NOSTROKEWIDTH = 0x00800000
   DTWAIN_PDFTEXT_NORENDERMODE = 0x01000000
   DTWAIN_PDFTEXT_NORGBCOLOR = 0x02000000
   DTWAIN_PDFTEXT_NOFONTSIZE = 0x04000000
   DTWAIN_PDFTEXT_NOABSPOSITION = 0x08000000
   DTWAIN_PDFTEXT_IGNOREALL = 0xFFF00000
   DTWAIN_FONT_COURIER = 0
   DTWAIN_FONT_COURIERBOLD = 1
   DTWAIN_FONT_COURIERBOLDOBLIQUE = 2
   DTWAIN_FONT_COURIEROBLIQUE = 3
   DTWAIN_FONT_HELVETICA = 4
   DTWAIN_FONT_HELVETICABOLD = 5
   DTWAIN_FONT_HELVETICABOLDOBLIQUE = 6
   DTWAIN_FONT_HELVETICAOBLIQUE = 7
   DTWAIN_FONT_TIMESBOLD = 8
   DTWAIN_FONT_TIMESBOLDITALIC = 9
   DTWAIN_FONT_TIMESROMAN = 10
   DTWAIN_FONT_TIMESITALIC = 11
   DTWAIN_FONT_SYMBOL = 12
   DTWAIN_FONT_ZAPFDINGBATS = 13
   DTWAIN_PDFRENDER_FILL = 0
   DTWAIN_PDFRENDER_STROKE = 1
   DTWAIN_PDFRENDER_FILLSTROKE = 2
   DTWAIN_PDFRENDER_INVISIBLE = 3
   DTWAIN_PDFTEXTELEMENT_SCALINGXY = 0
   DTWAIN_PDFTEXTELEMENT_FONTHEIGHT = 1
   DTWAIN_PDFTEXTELEMENT_WORDSPACING = 2
   DTWAIN_PDFTEXTELEMENT_POSITION = 3
   DTWAIN_PDFTEXTELEMENT_COLOR = 4
   DTWAIN_PDFTEXTELEMENT_STROKEWIDTH = 5
   DTWAIN_PDFTEXTELEMENT_DISPLAYFLAGS = 6
   DTWAIN_PDFTEXTELEMENT_FONTNAME = 7
   DTWAIN_PDFTEXTELEMENT_TEXT = 8
   DTWAIN_PDFTEXTELEMENT_RENDERMODE = 9
   DTWAIN_PDFTEXTELEMENT_CHARSPACING = 10
   DTWAIN_PDFTEXTELEMENT_ROTATIONANGLE = 11
   DTWAIN_PDFTEXTELEMENT_LEADING = 12
   DTWAIN_PDFTEXTELEMENT_SCALING = 13
   DTWAIN_PDFTEXTELEMENT_TEXTLENGTH = 14
   DTWAIN_PDFTEXTELEMENT_SKEWANGLES = 15
   DTWAIN_PDFTEXTELEMENT_TRANSFORMORDER = 16
   DTWAIN_PDFTEXTTRANSFORM_TSRK = 0
   DTWAIN_PDFTEXTTRANSFORM_TSKR = 1
   DTWAIN_PDFTEXTTRANSFORM_TKSR = 2
   DTWAIN_PDFTEXTTRANSFORM_TKRS = 3
   DTWAIN_PDFTEXTTRANSFORM_TRSK = 4
   DTWAIN_PDFTEXTTRANSFORM_TRKS = 5
   DTWAIN_PDFTEXTTRANSFORM_STRK = 6
   DTWAIN_PDFTEXTTRANSFORM_STKR = 7
   DTWAIN_PDFTEXTTRANSFORM_SKTR = 8
   DTWAIN_PDFTEXTTRANSFORM_SKRT = 9
   DTWAIN_PDFTEXTTRANSFORM_SRTK = 10
   DTWAIN_PDFTEXTTRANSFORM_SRKT = 11
   DTWAIN_PDFTEXTTRANSFORM_RSTK = 12
   DTWAIN_PDFTEXTTRANSFORM_RSKT = 13
   DTWAIN_PDFTEXTTRANSFORM_RTSK = 14
   DTWAIN_PDFTEXTTRANSFORM_RTKT = 15
   DTWAIN_PDFTEXTTRANSFORM_RKST = 16
   DTWAIN_PDFTEXTTRANSFORM_RKTS = 17
   DTWAIN_PDFTEXTTRANSFORM_KSTR = 18
   DTWAIN_PDFTEXTTRANSFORM_KSRT = 19
   DTWAIN_PDFTEXTTRANSFORM_KRST = 20
   DTWAIN_PDFTEXTTRANSFORM_KRTS = 21
   DTWAIN_PDFTEXTTRANSFORM_KTSR = 22
   DTWAIN_PDFTEXTTRANSFORM_KTRS = 23
   DTWAIN_PDFTEXTTRANFORM_LAST = DTWAIN_PDFTEXTTRANSFORM_KTRS
   DTWAIN_TWDF_ULTRASONIC = 0
   DTWAIN_TWDF_BYLENGTH = 1
   DTWAIN_TWDF_INFRARED = 2
   DTWAIN_TWAS_NONE = 0
   DTWAIN_TWAS_AUTO = 1
   DTWAIN_TWAS_CURRENT = 2
   DTWAIN_TWFR_BOOK = 0
   DTWAIN_TWFR_FANFOLD = 1
   DTWAIN_CONSTANT_TWPT = 0 
   DTWAIN_CONSTANT_TWUN = 1 
   DTWAIN_CONSTANT_TWCY = 2 
   DTWAIN_CONSTANT_TWAL = 3 
   DTWAIN_CONSTANT_TWAS = 4 
   DTWAIN_CONSTANT_TWBCOR = 5 
   DTWAIN_CONSTANT_TWBD = 6 
   DTWAIN_CONSTANT_TWBO = 7 
   DTWAIN_CONSTANT_TWBP = 8 
   DTWAIN_CONSTANT_TWBR = 9 
   DTWAIN_CONSTANT_TWBT = 10
   DTWAIN_CONSTANT_TWCP = 11
   DTWAIN_CONSTANT_TWCS = 12
   DTWAIN_CONSTANT_TWDE = 13
   DTWAIN_CONSTANT_TWDR = 14
   DTWAIN_CONSTANT_TWDSK = 15
   DTWAIN_CONSTANT_TWDX = 16
   DTWAIN_CONSTANT_TWFA = 17
   DTWAIN_CONSTANT_TWFE = 18
   DTWAIN_CONSTANT_TWFF = 19
   DTWAIN_CONSTANT_TWFL = 20
   DTWAIN_CONSTANT_TWFO = 21
   DTWAIN_CONSTANT_TWFP = 22
   DTWAIN_CONSTANT_TWFR = 23
   DTWAIN_CONSTANT_TWFT = 24
   DTWAIN_CONSTANT_TWFY = 25
   DTWAIN_CONSTANT_TWIA = 26
   DTWAIN_CONSTANT_TWIC = 27
   DTWAIN_CONSTANT_TWIF = 28
   DTWAIN_CONSTANT_TWIM = 29
   DTWAIN_CONSTANT_TWJC = 30
   DTWAIN_CONSTANT_TWJQ = 31
   DTWAIN_CONSTANT_TWLP = 32
   DTWAIN_CONSTANT_TWLS = 33
   DTWAIN_CONSTANT_TWMD = 34
   DTWAIN_CONSTANT_TWNF = 35
   DTWAIN_CONSTANT_TWOR = 36
   DTWAIN_CONSTANT_TWOV = 37
   DTWAIN_CONSTANT_TWPA = 38
   DTWAIN_CONSTANT_TWPC = 39
   DTWAIN_CONSTANT_TWPCH = 40
   DTWAIN_CONSTANT_TWPF = 41
   DTWAIN_CONSTANT_TWPM = 42
   DTWAIN_CONSTANT_TWPR = 43
   DTWAIN_CONSTANT_TWPF2 = 44
   DTWAIN_CONSTANT_TWCT = 45
   DTWAIN_CONSTANT_TWPS = 46
   DTWAIN_CONSTANT_TWSS = 47
   DTWAIN_CONSTANT_TWPH = 48
   DTWAIN_CONSTANT_TWCI = 49
   DTWAIN_CONSTANT_FONTNAME = 50
   DTWAIN_CONSTANT_TWEI = 51
   DTWAIN_CONSTANT_TWEJ = 52
   DTWAIN_CONSTANT_TWCC = 53
   DTWAIN_CONSTANT_TWQC = 54
   DTWAIN_CONSTANT_TWRC = 55
   DTWAIN_CONSTANT_MSG = 56
   DTWAIN_CONSTANT_TWLG = 57
   DTWAIN_CONSTANT_DLLINFO = 58
   DTWAIN_CONSTANT_DG = 59
   DTWAIN_CONSTANT_DAT = 60
   DTWAIN_CONSTANT_DF = 61
   DTWAIN_CONSTANT_TWTY = 62
   DTWAIN_CONSTANT_TWCB = 63
   DTWAIN_CONSTANT_TWAF = 64
   DTWAIN_CONSTANT_TWFS = 65
   DTWAIN_CONSTANT_TWJS = 66
   DTWAIN_CONSTANT_TWMR = 67
   DTWAIN_CONSTANT_TWDP = 68
   DTWAIN_CONSTANT_TWUS = 69
   DTWAIN_CONSTANT_TWDF = 70
   DTWAIN_CONSTANT_TWFM = 71
   DTWAIN_CONSTANT_TWSG = 72
   DTWAIN_CONSTANT_DTWAIN_TN = 73
   DTWAIN_CONSTANT_TWON = 74
   DTWAIN_CONSTANT_TWMF = 75
   DTWAIN_CONSTANT_TWSX = 76
   DTWAIN_CONSTANT_CAP = 77
   DTWAIN_CONSTANT_ICAP = 78
   DTWAIN_CONSTANT_DTWAIN_CONT = 79
   DTWAIN_CONSTANT_CAPCODE_MAP = 80
   DTWAIN_CONSTANT_ACAP = 81
   DTWAIN_USERRES_START = 20000
   DTWAIN_USERRES_MAXSIZE = 8192
   DTWAIN_APIHANDLEOK = 1
   DTWAIN_TWAINSESSIONOK = 2
   DTWAIN_PDF_AES128 = 1
   DTWAIN_PDF_AES256 = 2
   DTWAIN_FEEDER_TERMINATE = 1
   DTWAIN_FEEDER_USEFLATBED = 2

   @isinit = false

   def isInitialized()
       return @isinit
   end

   def initialize(dllname)

       is_32_bit = Fiddle::SIZEOF_VOIDP == 4
       is_64_bit = Fiddle::SIZEOF_VOIDP == 8

       valid_dll_names32 = ["dtwain32.dll", "dtwain32u.dll", "dtwain32d.dll", "dtwain32ud.dll"]
       valid_dll_names64 = ["dtwain64.dll", "dtwain64u.dll", "dtwain64d.dll", "dtwain64ud.dll"]

       filename = File.basename(dllname)
       filename.downcase!
       
       if is_64_bit
           dll_valid = valid_dll_names64.include?(filename)
           if !dll_valid
               puts "The dll name #{dllname} is not a valid DTWAIN DLL name for a 64-bit environment."
               return
           end  
       elsif is_32_bit
           dll_valid = valid_dll_names32.include?(filename)
           if !dll_valid
               puts "The dll name #{dllname} is not a valid DTWAIN DLL name for a 32-bit environment."
               return
           end  
       end

       # Load the DLL 
       dtwain_dll = Fiddle.dlopen(dllname)

       if dtwain_dll.nil?
           @isinit = false
           return
       else
           @isinit = true
       end

       @DTWAIN_AcquireAudioFile = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireAudioFile'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireAudioFileA = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireAudioFileA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireAudioFileW = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireAudioFileW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireAudioNative = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireAudioNative'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_AcquireAudioNativeEx = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireAudioNativeEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireBuffered = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireBuffered'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_AcquireBufferedEx = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireBufferedEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireFile = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireFile'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireFileA = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireFileA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireFileEx = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireFileEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireFileW = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireFileW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireNative = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireNative'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_AcquireNativeEx = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireNativeEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_AcquireToClipboard = Fiddle::Function::new(dtwain_dll['DTWAIN_AcquireToClipboard'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_AddExtImageInfoQuery = Fiddle::Function::new(dtwain_dll['DTWAIN_AddExtImageInfoQuery'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_AddPDFText = Fiddle::Function::new(dtwain_dll['DTWAIN_AddPDFText'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_UINT],Fiddle::TYPE_INT)
       @DTWAIN_AddPDFTextA = Fiddle::Function::new(dtwain_dll['DTWAIN_AddPDFTextA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_UINT],Fiddle::TYPE_INT)
       @DTWAIN_AddPDFTextEx = Fiddle::Function::new(dtwain_dll['DTWAIN_AddPDFTextEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_UINT],Fiddle::TYPE_INT)
       @DTWAIN_AddPDFTextW = Fiddle::Function::new(dtwain_dll['DTWAIN_AddPDFTextW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_UINT],Fiddle::TYPE_INT)
       @DTWAIN_AllocateMemory = Fiddle::Function::new(dtwain_dll['DTWAIN_AllocateMemory'],[Fiddle::TYPE_UINT],Fiddle::TYPE_VOIDP)
       @DTWAIN_AllocateMemory64 = Fiddle::Function::new(dtwain_dll['DTWAIN_AllocateMemory64'],[Fiddle::TYPE_ULONG_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_AllocateMemoryEx = Fiddle::Function::new(dtwain_dll['DTWAIN_AllocateMemoryEx'],[Fiddle::TYPE_UINT],Fiddle::TYPE_VOIDP)
       @DTWAIN_AppHandlesExceptions = Fiddle::Function::new(dtwain_dll['DTWAIN_AppHandlesExceptions'],[Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_ArrayANSIStringToFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayANSIStringToFloat'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayAdd = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAdd'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddANSIString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddANSIString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddANSIStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddANSIStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloatN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloatN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloatStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloatStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloatStringNA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloatStringNA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloatStringNW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloatStringNW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFrame = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFrame'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddFrameN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddFrameN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddLong = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddLong64 = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddLong64'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddLong64N = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddLong64N'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddLongN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddLongN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddStringNA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddStringNA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddStringNW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddStringNW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddWideString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddWideString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayAddWideStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayAddWideStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayConvertFix32ToFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayConvertFix32ToFloat'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayConvertFloatToFix32 = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayConvertFloatToFix32'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayCopy = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayCopy'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayCreate = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayCreate'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayCreateCopy = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayCreateCopy'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayCreateFromCap = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayCreateFromCap'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayCreateFromLong64s = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayCreateFromLong64s'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayCreateFromLongs = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayCreateFromLongs'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayCreateFromReals = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayCreateFromReals'],[Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayDestroy = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayDestroy'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayDestroyFrames = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayDestroyFrames'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayFind = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFind'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindANSIString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindANSIString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindLong = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindLong64 = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindLong64'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFindWideString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFindWideString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayFix32GetAt = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFix32GetAt'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayFix32SetAt = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFix32SetAt'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayFloatToANSIString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFloatToANSIString'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayFloatToString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFloatToString'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayFloatToWideString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayFloatToWideString'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetAt = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAt'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtANSIString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtANSIString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtANSIStringPtr = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtANSIStringPtr'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetAtFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFrame = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFrame'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFrameEx = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFrameEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFrameString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFrameString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFrameStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFrameStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtFrameStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtFrameStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtLong = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtLong64 = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtLong64'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtSource = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtSource'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtStringPtr = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtStringPtr'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetAtStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtWideString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtWideString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetAtWideStringPtr = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetAtWideStringPtr'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetBuffer = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetBuffer'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetCapValues = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetCapValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetCapValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetCapValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetCapValuesEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetCapValuesEx2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayGetCount = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetCount'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayGetMaxStringLength = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetMaxStringLength'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayGetSourceAt = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetSourceAt'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayGetStringLength = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetStringLength'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayGetType = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayGetType'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_ArrayInit = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInit'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayInsertAt = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAt'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtANSIString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtANSIString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtANSIStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtANSIStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloatN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloatN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloatStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloatStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloatStringNA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloatStringNA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloatStringNW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloatStringNW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFrame = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFrame'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtFrameN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtFrameN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtLong = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtLong64 = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtLong64'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtLong64N = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtLong64N'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtLongN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtLongN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtStringNA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtStringNA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtStringNW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtStringNW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtWideString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtWideString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayInsertAtWideStringN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayInsertAtWideStringN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayRemoveAll = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayRemoveAll'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayRemoveAt = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayRemoveAt'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayRemoveAtN = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayRemoveAtN'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArrayResize = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayResize'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAt = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAt'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtANSIString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtANSIString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFrame = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFrame'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFrameEx = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFrameEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFrameString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFrameString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFrameStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFrameStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtFrameStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtFrameStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtLong = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtLong64 = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtLong64'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArraySetAtWideString = Fiddle::Function::new(dtwain_dll['DTWAIN_ArraySetAtWideString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ArrayStringToFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayStringToFloat'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ArrayWideStringToFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_ArrayWideStringToFloat'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_CallCallback = Fiddle::Function::new(dtwain_dll['DTWAIN_CallCallback'],[Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_CallCallback64 = Fiddle::Function::new(dtwain_dll['DTWAIN_CallCallback64'],[Fiddle::TYPE_INT, Fiddle::TYPE_INT, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_CallDSMProc = Fiddle::Function::new(dtwain_dll['DTWAIN_CallDSMProc'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_CheckHandles = Fiddle::Function::new(dtwain_dll['DTWAIN_CheckHandles'],[Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_ClearBuffers = Fiddle::Function::new(dtwain_dll['DTWAIN_ClearBuffers'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ClearErrorBuffer = Fiddle::Function::new(dtwain_dll['DTWAIN_ClearErrorBuffer'],[],Fiddle::TYPE_INT)
       @DTWAIN_ClearPDFText = Fiddle::Function::new(dtwain_dll['DTWAIN_ClearPDFText'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ClearPage = Fiddle::Function::new(dtwain_dll['DTWAIN_ClearPage'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_CloseSource = Fiddle::Function::new(dtwain_dll['DTWAIN_CloseSource'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_CloseSourceUI = Fiddle::Function::new(dtwain_dll['DTWAIN_CloseSourceUI'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ConvertDIBToBitmap = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertDIBToBitmap'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ConvertDIBToFullBitmap = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertDIBToFullBitmap'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_ConvertToAPIString = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertToAPIString'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ConvertToAPIStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertToAPIStringA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_ConvertToAPIStringEx = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertToAPIStringEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_ConvertToAPIStringExA = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertToAPIStringExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_ConvertToAPIStringExW = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertToAPIStringExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_ConvertToAPIStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_ConvertToAPIStringW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_CreateAcquisitionArray = Fiddle::Function::new(dtwain_dll['DTWAIN_CreateAcquisitionArray'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_CreatePDFTextElement = Fiddle::Function::new(dtwain_dll['DTWAIN_CreatePDFTextElement'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_DeleteDIB = Fiddle::Function::new(dtwain_dll['DTWAIN_DeleteDIB'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_DestroyAcquisitionArray = Fiddle::Function::new(dtwain_dll['DTWAIN_DestroyAcquisitionArray'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_DestroyPDFTextElement = Fiddle::Function::new(dtwain_dll['DTWAIN_DestroyPDFTextElement'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_DisableAppWindow = Fiddle::Function::new(dtwain_dll['DTWAIN_DisableAppWindow'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableAutoBorderDetect = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableAutoBorderDetect'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableAutoBright = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableAutoBright'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableAutoDeskew = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableAutoDeskew'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableAutoFeed = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableAutoFeed'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableAutoRotate = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableAutoRotate'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableAutoScan = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableAutoScan'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableAutomaticSenseMedium = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableAutomaticSenseMedium'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableDuplex = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableDuplex'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableFeeder = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableFeeder'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableIndicator = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableIndicator'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableJobFileHandling = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableJobFileHandling'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableLamp = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableLamp'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableMsgNotify = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableMsgNotify'],[Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnablePatchDetect = Fiddle::Function::new(dtwain_dll['DTWAIN_EnablePatchDetect'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnablePeekMessageLoop = Fiddle::Function::new(dtwain_dll['DTWAIN_EnablePeekMessageLoop'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnablePrinter = Fiddle::Function::new(dtwain_dll['DTWAIN_EnablePrinter'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableThumbnail = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableThumbnail'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnableTripletsNotify = Fiddle::Function::new(dtwain_dll['DTWAIN_EnableTripletsNotify'],[Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EndThread = Fiddle::Function::new(dtwain_dll['DTWAIN_EndThread'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EndTwainSession = Fiddle::Function::new(dtwain_dll['DTWAIN_EndTwainSession'],[],Fiddle::TYPE_INT)
       @DTWAIN_EnumAlarmVolumes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAlarmVolumes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnumAlarmVolumesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAlarmVolumesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumAlarms = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAlarms'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumAlarmsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAlarmsEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumAudioXferMechs = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAudioXferMechs'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumAudioXferMechsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAudioXferMechsEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumAutoFeedValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAutoFeedValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumAutoFeedValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAutoFeedValuesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumAutomaticCaptures = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAutomaticCaptures'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnumAutomaticCapturesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAutomaticCapturesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumAutomaticSenseMedium = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAutomaticSenseMedium'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumAutomaticSenseMediumEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumAutomaticSenseMediumEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumBitDepths = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumBitDepths'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumBitDepthsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumBitDepthsEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumBitDepthsEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumBitDepthsEx2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumBottomCameras = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumBottomCameras'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumBottomCamerasEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumBottomCamerasEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumBrightnessValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumBrightnessValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumBrightnessValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumBrightnessValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumCameras = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCameras'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumCamerasEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCamerasEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumCamerasEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCamerasEx2'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumCamerasEx3 = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCamerasEx3'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumCompressionTypes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCompressionTypes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumCompressionTypesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCompressionTypesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumCompressionTypesEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCompressionTypesEx2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumContrastValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumContrastValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumContrastValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumContrastValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumCustomCaps = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCustomCaps'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumCustomCapsEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumCustomCapsEx2'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumDoubleFeedDetectLengths = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumDoubleFeedDetectLengths'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumDoubleFeedDetectLengthsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumDoubleFeedDetectLengthsEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumDoubleFeedDetectValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumDoubleFeedDetectValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_EnumDoubleFeedDetectValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumDoubleFeedDetectValuesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumExtImageInfoTypes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumExtImageInfoTypes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumExtImageInfoTypesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumExtImageInfoTypesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumExtendedCaps = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumExtendedCaps'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumExtendedCapsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumExtendedCapsEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumExtendedCapsEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumExtendedCapsEx2'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumFileTypeBitsPerPixel = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumFileTypeBitsPerPixel'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumFileXferFormats = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumFileXferFormats'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumFileXferFormatsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumFileXferFormatsEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumHalftones = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumHalftones'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumHalftonesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumHalftonesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumHighlightValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumHighlightValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumHighlightValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumHighlightValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumJobControls = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumJobControls'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumJobControlsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumJobControlsEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumLightPaths = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumLightPaths'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumLightPathsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumLightPathsEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumLightSources = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumLightSources'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumLightSourcesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumLightSourcesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumMaxBuffers = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumMaxBuffers'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnumMaxBuffersEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumMaxBuffersEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumNoiseFilters = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumNoiseFilters'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumNoiseFiltersEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumNoiseFiltersEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumOCRInterfaces = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumOCRInterfaces'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumOCRSupportedCaps = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumOCRSupportedCaps'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumOrientations = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumOrientations'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumOrientationsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumOrientationsEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumOverscanValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumOverscanValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumOverscanValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumOverscanValuesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPaperSizes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPaperSizes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPaperSizesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPaperSizesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPatchCodes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchCodes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPatchCodesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchCodesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPatchMaxPriorities = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchMaxPriorities'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPatchMaxPrioritiesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchMaxPrioritiesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPatchMaxRetries = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchMaxRetries'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPatchMaxRetriesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchMaxRetriesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPatchPriorities = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchPriorities'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPatchPrioritiesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchPrioritiesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPatchSearchModes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchSearchModes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPatchSearchModesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchSearchModesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPatchTimeOutValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchTimeOutValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPatchTimeOutValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPatchTimeOutValuesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPixelTypes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPixelTypes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPixelTypesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPixelTypesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumPrinterStringModes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPrinterStringModes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumPrinterStringModesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumPrinterStringModesEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumResolutionValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumResolutionValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumResolutionValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumResolutionValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumShadowValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumShadowValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumShadowValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumShadowValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumSourceUnits = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSourceUnits'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumSourceUnitsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSourceUnitsEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumSourceValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSourceValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnumSourceValuesA = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSourceValuesA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnumSourceValuesW = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSourceValuesW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_EnumSources = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSources'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumSourcesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSourcesEx'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumSupportedCaps = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedCaps'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumSupportedCapsEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedCapsEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumSupportedCapsEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedCapsEx2'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumSupportedExtImageInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedExtImageInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumSupportedExtImageInfoEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedExtImageInfoEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumSupportedFileTypes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedFileTypes'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumSupportedMultiPageFileTypes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedMultiPageFileTypes'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumSupportedSinglePageFileTypes = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumSupportedSinglePageFileTypes'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumThresholdValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumThresholdValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumThresholdValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumThresholdValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumTopCameras = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumTopCameras'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumTopCamerasEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumTopCamerasEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumTwainPrinters = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumTwainPrinters'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumTwainPrintersArray = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumTwainPrintersArray'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_EnumTwainPrintersArrayEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumTwainPrintersArrayEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumTwainPrintersEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumTwainPrintersEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumXResolutionValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumXResolutionValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumXResolutionValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumXResolutionValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_EnumYResolutionValues = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumYResolutionValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_EnumYResolutionValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_EnumYResolutionValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_ExecuteOCR = Fiddle::Function::new(dtwain_dll['DTWAIN_ExecuteOCR'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ExecuteOCRA = Fiddle::Function::new(dtwain_dll['DTWAIN_ExecuteOCRA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ExecuteOCRW = Fiddle::Function::new(dtwain_dll['DTWAIN_ExecuteOCRW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_FeedPage = Fiddle::Function::new(dtwain_dll['DTWAIN_FeedPage'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FlipBitmap = Fiddle::Function::new(dtwain_dll['DTWAIN_FlipBitmap'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FlushAcquiredPages = Fiddle::Function::new(dtwain_dll['DTWAIN_FlushAcquiredPages'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ForceAcquireBitDepth = Fiddle::Function::new(dtwain_dll['DTWAIN_ForceAcquireBitDepth'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ForceScanOnNoUI = Fiddle::Function::new(dtwain_dll['DTWAIN_ForceScanOnNoUI'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_FrameCreate = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameCreate'],[Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_VOIDP)
       @DTWAIN_FrameCreateString = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameCreateString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_FrameCreateStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameCreateStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_FrameCreateStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameCreateStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_FrameDestroy = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameDestroy'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetAll = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetAll'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetAllString = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetAllString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetAllStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetAllStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetAllStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetAllStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetValue = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetValue'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetValueString = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetValueString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetValueStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetValueStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameGetValueStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameGetValueStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameIsValid = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameIsValid'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetAll = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetAll'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetAllString = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetAllString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetAllStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetAllStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetAllStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetAllStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetValue = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetValue'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetValueString = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetValueString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetValueStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetValueStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FrameSetValueStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_FrameSetValueStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FreeExtImageInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_FreeExtImageInfo'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FreeMemory = Fiddle::Function::new(dtwain_dll['DTWAIN_FreeMemory'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_FreeMemoryEx = Fiddle::Function::new(dtwain_dll['DTWAIN_FreeMemoryEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAPIHandleStatus = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAPIHandleStatus'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetAcquireArea = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireArea'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquireArea2 = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireArea2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquireArea2String = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireArea2String'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquireArea2StringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireArea2StringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquireArea2StringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireArea2StringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquireAreaEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireAreaEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetAcquireMetrics = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireMetrics'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquireStripBuffer = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireStripBuffer'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetAcquireStripData = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireStripData'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquireStripSizes = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquireStripSizes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAcquiredImage = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquiredImage'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetAcquiredImageArray = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAcquiredImageArray'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetActiveDSMPath = Fiddle::Function::new(dtwain_dll['DTWAIN_GetActiveDSMPath'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetActiveDSMPathA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetActiveDSMPathA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetActiveDSMPathW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetActiveDSMPathW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetActiveDSMVersionInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetActiveDSMVersionInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetActiveDSMVersionInfoA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetActiveDSMVersionInfoA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetActiveDSMVersionInfoW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetActiveDSMVersionInfoW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetAlarmVolume = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAlarmVolume'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAllSourceDibs = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAllSourceDibs'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetAppInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAppInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAppInfoA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAppInfoA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAppInfoW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAppInfoW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAuthor = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAuthor'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAuthorA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAuthorA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetAuthorW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetAuthorW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetBatteryMinutes = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBatteryMinutes'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetBatteryPercent = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBatteryPercent'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetBitDepth = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBitDepth'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetBlankPageAutoDetection = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBlankPageAutoDetection'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetBrightness = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBrightness'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetBrightnessString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBrightnessString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetBrightnessStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBrightnessStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetBrightnessStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBrightnessStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetBufferedTransferInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetBufferedTransferInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetCallback = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCallback'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetCallback64 = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCallback64'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetCapArrayType = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapArrayType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetCapContainer = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapContainer'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetCapContainerEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapContainerEx'],[Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetCapContainerEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapContainerEx2'],[Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetCapDataType = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapDataType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetCapFromName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapFromName'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetCapFromNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapFromNameA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetCapFromNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapFromNameW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetCapOperations = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapOperations'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCapValues = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCapValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCapValuesEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCapValuesEx2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCaption = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCaption'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCaptionA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCaptionA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCaptionW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCaptionW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCompressionSize = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCompressionSize'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCompressionType = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCompressionType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetConditionCodeString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetConditionCodeString'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetConditionCodeStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetConditionCodeStringA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetConditionCodeStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetConditionCodeStringW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetContrast = Fiddle::Function::new(dtwain_dll['DTWAIN_GetContrast'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetContrastString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetContrastString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetContrastStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetContrastStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetContrastStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetContrastStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCountry = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCountry'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetCurrentAcquiredImage = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCurrentAcquiredImage'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetCurrentFileName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCurrentFileName'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetCurrentFileNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCurrentFileNameA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetCurrentFileNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCurrentFileNameW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetCurrentPageNum = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCurrentPageNum'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetCurrentRetryCount = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCurrentRetryCount'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetCurrentTwainTriplet = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCurrentTwainTriplet'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetCustomDSData = Fiddle::Function::new(dtwain_dll['DTWAIN_GetCustomDSData'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_UINT, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetDSMFullName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDSMFullName'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetDSMFullNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDSMFullNameA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetDSMFullNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDSMFullNameW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetDSMSearchOrder = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDSMSearchOrder'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetDTWAINHandle = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDTWAINHandle'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetDeviceEvent = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDeviceEvent'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDeviceEventEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDeviceEventEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDeviceEventInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDeviceEventInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDeviceNotifications = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDeviceNotifications'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDeviceTimeDate = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDeviceTimeDate'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDeviceTimeDateA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDeviceTimeDateA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDeviceTimeDateW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDeviceTimeDateW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDoubleFeedDetectLength = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDoubleFeedDetectLength'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetDoubleFeedDetectValues = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDoubleFeedDetectValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetDuplexType = Fiddle::Function::new(dtwain_dll['DTWAIN_GetDuplexType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetErrorBuffer = Fiddle::Function::new(dtwain_dll['DTWAIN_GetErrorBuffer'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetErrorBufferThreshold = Fiddle::Function::new(dtwain_dll['DTWAIN_GetErrorBufferThreshold'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetErrorCallback = Fiddle::Function::new(dtwain_dll['DTWAIN_GetErrorCallback'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetErrorCallback64 = Fiddle::Function::new(dtwain_dll['DTWAIN_GetErrorCallback64'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetErrorString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetErrorString'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetErrorStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetErrorStringA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetErrorStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetErrorStringW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetExtCapFromName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtCapFromName'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetExtCapFromNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtCapFromNameA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetExtCapFromNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtCapFromNameW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetExtImageInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtImageInfo'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetExtImageInfoData = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtImageInfoData'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetExtImageInfoDataEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtImageInfoDataEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetExtImageInfoItem = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtImageInfoItem'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetExtImageInfoItemEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtImageInfoItemEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetExtNameFromCap = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtNameFromCap'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetExtNameFromCapA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtNameFromCapA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetExtNameFromCapW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetExtNameFromCapW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetFeederAlignment = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFeederAlignment'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetFeederFuncs = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFeederFuncs'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetFeederOrder = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFeederOrder'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetFeederWaitTime = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFeederWaitTime'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileCompressionType = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileCompressionType'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileSavePageCount = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileSavePageCount'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileTypeExtensions = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileTypeExtensions'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileTypeExtensionsA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileTypeExtensionsA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileTypeExtensionsW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileTypeExtensionsW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileTypeName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileTypeName'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileTypeNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileTypeNameA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetFileTypeNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetFileTypeNameW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetHalftone = Fiddle::Function::new(dtwain_dll['DTWAIN_GetHalftone'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetHalftoneA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetHalftoneA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetHalftoneW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetHalftoneW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetHighlight = Fiddle::Function::new(dtwain_dll['DTWAIN_GetHighlight'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetHighlightString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetHighlightString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetHighlightStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetHighlightStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetHighlightStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetHighlightStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetImageInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetImageInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetImageInfoString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetImageInfoString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetImageInfoStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetImageInfoStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetImageInfoStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetImageInfoStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetJobControl = Fiddle::Function::new(dtwain_dll['DTWAIN_GetJobControl'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetJpegValues = Fiddle::Function::new(dtwain_dll['DTWAIN_GetJpegValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetJpegXRValues = Fiddle::Function::new(dtwain_dll['DTWAIN_GetJpegXRValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetLanguage = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLanguage'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetLastError = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLastError'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetLibraryPath = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLibraryPath'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetLibraryPathA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLibraryPathA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetLibraryPathW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLibraryPathW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetLightPath = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLightPath'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetLightSource = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLightSource'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetLightSources = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLightSources'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetLoggerCallback = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLoggerCallback'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetLoggerCallbackA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLoggerCallbackA'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetLoggerCallbackW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetLoggerCallbackW'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetManualDuplexCount = Fiddle::Function::new(dtwain_dll['DTWAIN_GetManualDuplexCount'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetMaxAcquisitions = Fiddle::Function::new(dtwain_dll['DTWAIN_GetMaxAcquisitions'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetMaxBuffers = Fiddle::Function::new(dtwain_dll['DTWAIN_GetMaxBuffers'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetMaxPagesToAcquire = Fiddle::Function::new(dtwain_dll['DTWAIN_GetMaxPagesToAcquire'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetMaxRetryAttempts = Fiddle::Function::new(dtwain_dll['DTWAIN_GetMaxRetryAttempts'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetNameFromCap = Fiddle::Function::new(dtwain_dll['DTWAIN_GetNameFromCap'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetNameFromCapA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetNameFromCapA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetNameFromCapW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetNameFromCapW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetNoiseFilter = Fiddle::Function::new(dtwain_dll['DTWAIN_GetNoiseFilter'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetNumAcquiredImages = Fiddle::Function::new(dtwain_dll['DTWAIN_GetNumAcquiredImages'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetNumAcquisitions = Fiddle::Function::new(dtwain_dll['DTWAIN_GetNumAcquisitions'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRCapValues = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRCapValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetOCRErrorString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRErrorString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRErrorStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRErrorStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRErrorStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRErrorStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRLastError = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRLastError'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRMajorMinorVersion = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRMajorMinorVersion'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetOCRManufacturer = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRManufacturer'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRManufacturerA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRManufacturerA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRManufacturerW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRManufacturerW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRProductFamily = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRProductFamily'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRProductFamilyA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRProductFamilyA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRProductFamilyW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRProductFamilyW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRProductName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRProductName'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRProductNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRProductNameA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRProductNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRProductNameW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRText = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRText'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetOCRTextA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRTextA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetOCRTextInfoFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRTextInfoFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetOCRTextInfoFloatEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRTextInfoFloatEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetOCRTextInfoHandle = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRTextInfoHandle'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetOCRTextInfoLong = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRTextInfoLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetOCRTextInfoLongEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRTextInfoLongEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetOCRTextW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRTextW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetOCRVersionInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRVersionInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRVersionInfoA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRVersionInfoA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOCRVersionInfoW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOCRVersionInfoW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetOrientation = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOrientation'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetOverscan = Fiddle::Function::new(dtwain_dll['DTWAIN_GetOverscan'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPDFTextElementFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFTextElementFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetPDFTextElementLong = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFTextElementLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetPDFTextElementString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFTextElementString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetPDFTextElementStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFTextElementStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetPDFTextElementStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFTextElementStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetPDFType1FontName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFType1FontName'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetPDFType1FontNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFType1FontNameA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetPDFType1FontNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPDFType1FontNameW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetPaperSize = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPaperSize'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPaperSizeName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPaperSizeName'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetPaperSizeNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPaperSizeNameA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetPaperSizeNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPaperSizeNameW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetPatchMaxPriorities = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPatchMaxPriorities'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPatchMaxRetries = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPatchMaxRetries'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPatchPriorities = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPatchPriorities'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetPatchSearchMode = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPatchSearchMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPatchTimeOut = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPatchTimeOut'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPixelFlavor = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPixelFlavor'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetPixelType = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPixelType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPrinter = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPrinter'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPrinterStartNumber = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPrinterStartNumber'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetPrinterStringMode = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPrinterStringMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_GetPrinterStrings = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPrinterStrings'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetPrinterSuffixString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPrinterSuffixString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetPrinterSuffixStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPrinterSuffixStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetPrinterSuffixStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetPrinterSuffixStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_GetRegisteredMsg = Fiddle::Function::new(dtwain_dll['DTWAIN_GetRegisteredMsg'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetResolution = Fiddle::Function::new(dtwain_dll['DTWAIN_GetResolution'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetResolutionString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetResolutionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetResolutionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetResolutionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetResolutionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetResolutionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetResourceString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetResourceString'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetResourceStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetResourceStringA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetResourceStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetResourceStringW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetRotation = Fiddle::Function::new(dtwain_dll['DTWAIN_GetRotation'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetRotationString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetRotationString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetRotationStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetRotationStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetRotationStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetRotationStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetSaveFileName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSaveFileName'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSaveFileNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSaveFileNameA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSaveFileNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSaveFileNameW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSessionDetails = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSessionDetails'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_GetSessionDetailsA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSessionDetailsA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_GetSessionDetailsW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSessionDetailsW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_GetShadow = Fiddle::Function::new(dtwain_dll['DTWAIN_GetShadow'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetShadowString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetShadowString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetShadowStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetShadowStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetShadowStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetShadowStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetShortVersionString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetShortVersionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetShortVersionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetShortVersionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetShortVersionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetShortVersionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceAcquisitions = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceAcquisitions'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetSourceDetails = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceDetails'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceDetailsA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceDetailsA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceDetailsW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceDetailsW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceID = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceID'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetSourceIDEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceIDEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetSourceManufacturer = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceManufacturer'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceManufacturerA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceManufacturerA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceManufacturerW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceManufacturerW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceProductFamily = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceProductFamily'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceProductFamilyA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceProductFamilyA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceProductFamilyW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceProductFamilyW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceProductName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceProductName'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceProductNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceProductNameA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceProductNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceProductNameW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceUnit = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceUnit'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetSourceVersionInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceVersionInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceVersionInfoA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceVersionInfoA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceVersionInfoW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceVersionInfoW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetSourceVersionNumber = Fiddle::Function::new(dtwain_dll['DTWAIN_GetSourceVersionNumber'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetStaticLibVersion = Fiddle::Function::new(dtwain_dll['DTWAIN_GetStaticLibVersion'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetTempFileDirectory = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTempFileDirectory'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTempFileDirectoryA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTempFileDirectoryA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTempFileDirectoryW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTempFileDirectoryW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetThreshold = Fiddle::Function::new(dtwain_dll['DTWAIN_GetThreshold'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetThresholdString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetThresholdString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetThresholdStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetThresholdStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetThresholdStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetThresholdStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTimeDate = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTimeDate'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTimeDateA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTimeDateA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTimeDateW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTimeDateW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTwainAppID = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainAppID'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetTwainAppIDEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainAppIDEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetTwainAvailability = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainAvailability'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainAvailabilityEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainAvailabilityEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainAvailabilityExA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainAvailabilityExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainAvailabilityExW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainAvailabilityExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainCountryName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainCountryName'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTwainCountryNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainCountryNameA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTwainCountryNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainCountryNameW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTwainCountryValue = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainCountryValue'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainCountryValueA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainCountryValueA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainCountryValueW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainCountryValueW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainHwnd = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainHwnd'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_GetTwainIDFromName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainIDFromName'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainIDFromNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainIDFromNameA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainIDFromNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainIDFromNameW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainLanguageName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainLanguageName'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTwainLanguageNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainLanguageNameA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTwainLanguageNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainLanguageNameW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetTwainLanguageValue = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainLanguageValue'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainLanguageValueA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainLanguageValueA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainLanguageValueW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainLanguageValueW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainMode = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainMode'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainNameFromConstant = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainNameFromConstant'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainNameFromConstantA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainNameFromConstantA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainNameFromConstantW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainNameFromConstantW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainStringName = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainStringName'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainStringNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainStringNameA'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainStringNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainStringNameW'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetTwainTimeout = Fiddle::Function::new(dtwain_dll['DTWAIN_GetTwainTimeout'],[],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersion = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersion'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetVersionCopyright = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionCopyright'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionCopyrightA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionCopyrightA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionCopyrightW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionCopyrightW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionEx = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetVersionInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionInfoA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionInfoA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionInfoW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionInfoW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetVersionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetVersionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetWindowsVersionInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_GetWindowsVersionInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetWindowsVersionInfoA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetWindowsVersionInfoA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetWindowsVersionInfoW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetWindowsVersionInfoW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_GetXResolution = Fiddle::Function::new(dtwain_dll['DTWAIN_GetXResolution'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetXResolutionString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetXResolutionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetXResolutionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetXResolutionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetXResolutionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetXResolutionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetYResolution = Fiddle::Function::new(dtwain_dll['DTWAIN_GetYResolution'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetYResolutionString = Fiddle::Function::new(dtwain_dll['DTWAIN_GetYResolutionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetYResolutionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_GetYResolutionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_GetYResolutionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_GetYResolutionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_InitExtImageInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_InitExtImageInfo'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_InitImageFileAppend = Fiddle::Function::new(dtwain_dll['DTWAIN_InitImageFileAppend'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_InitImageFileAppendA = Fiddle::Function::new(dtwain_dll['DTWAIN_InitImageFileAppendA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_InitImageFileAppendW = Fiddle::Function::new(dtwain_dll['DTWAIN_InitImageFileAppendW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_InitOCRInterface = Fiddle::Function::new(dtwain_dll['DTWAIN_InitOCRInterface'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsAcquiring = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAcquiring'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsAudioXferSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAudioXferSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoBorderDetectEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoBorderDetectEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoBorderDetectSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoBorderDetectSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoBrightEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoBrightEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoBrightSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoBrightSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoDeskewEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoDeskewEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoDeskewSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoDeskewSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoFeedEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoFeedEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoFeedSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoFeedSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoRotateEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoRotateEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoRotateSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoRotateSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutoScanEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutoScanEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutomaticSenseMediumEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutomaticSenseMediumEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsAutomaticSenseMediumSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsAutomaticSenseMediumSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsBlankPageDetectionOn = Fiddle::Function::new(dtwain_dll['DTWAIN_IsBlankPageDetectionOn'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsBufferedTileModeOn = Fiddle::Function::new(dtwain_dll['DTWAIN_IsBufferedTileModeOn'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsBufferedTileModeSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsBufferedTileModeSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsCapSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsCapSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsCompressionSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsCompressionSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsCustomDSDataSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsCustomDSDataSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsDIBBlank = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDIBBlank'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_LONG)
       @DTWAIN_IsDIBBlankString = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDIBBlankString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_IsDIBBlankStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDIBBlankStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_IsDIBBlankStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDIBBlankStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_IsDeviceEventSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDeviceEventSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsDeviceOnLine = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDeviceOnLine'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsDoubleFeedDetectLengthSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDoubleFeedDetectLengthSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_IsDoubleFeedDetectSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDoubleFeedDetectSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsDuplexEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDuplexEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsDuplexSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsDuplexSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsExtImageInfoSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsExtImageInfoSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsFeederEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsFeederEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsFeederLoaded = Fiddle::Function::new(dtwain_dll['DTWAIN_IsFeederLoaded'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsFeederSensitive = Fiddle::Function::new(dtwain_dll['DTWAIN_IsFeederSensitive'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsFeederSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsFeederSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsFileSystemSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsFileSystemSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsFileXferSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsFileXferSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldALastPageSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldALastPageSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldALevelSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldALevelSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldAPrintFormatSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldAPrintFormatSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldAValueSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldAValueSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldBLastPageSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldBLastPageSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldBLevelSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldBLevelSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldBPrintFormatSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldBPrintFormatSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldBValueSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldBValueSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldCLastPageSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldCLastPageSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldCLevelSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldCLevelSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldCPrintFormatSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldCPrintFormatSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldCValueSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldCValueSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldDLastPageSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldDLastPageSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldDLevelSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldDLevelSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldDPrintFormatSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldDPrintFormatSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldDValueSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldDValueSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldELastPageSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldELastPageSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldELevelSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldELevelSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldEPrintFormatSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldEPrintFormatSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIAFieldEValueSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIAFieldEValueSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsImageAddressingSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsImageAddressingSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIndicatorEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIndicatorEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsIndicatorSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsIndicatorSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsInitialized = Fiddle::Function::new(dtwain_dll['DTWAIN_IsInitialized'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsJPEGSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsJPEGSupported'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsJobControlSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsJobControlSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsLampEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsLampEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsLampSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsLampSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsLightPathSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsLightPathSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsLightSourceSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsLightSourceSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsMaxBuffersSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsMaxBuffersSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsMemFileXferSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsMemFileXferSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsMsgNotifyEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsMsgNotifyEnabled'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsNotifyTripletsEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsNotifyTripletsEnabled'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsOCREngineActivated = Fiddle::Function::new(dtwain_dll['DTWAIN_IsOCREngineActivated'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsOpenSourcesOnSelect = Fiddle::Function::new(dtwain_dll['DTWAIN_IsOpenSourcesOnSelect'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsOrientationSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsOrientationSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsOverscanSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsOverscanSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsPDFSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPDFSupported'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsPNGSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPNGSupported'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsPaperDetectable = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPaperDetectable'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsPaperSizeSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPaperSizeSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsPatchCapsSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPatchCapsSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsPatchDetectEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPatchDetectEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsPatchSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPatchSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsPeekMessageLoopEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPeekMessageLoopEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsPixelTypeSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPixelTypeSupported'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsPrinterEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPrinterEnabled'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_IsPrinterSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsPrinterSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsRotationSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsRotationSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsSessionEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSessionEnabled'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsSkipImageInfoError = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSkipImageInfoError'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsSourceAcquiring = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSourceAcquiring'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsSourceAcquiringEx = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSourceAcquiringEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_IsSourceInUIOnlyMode = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSourceInUIOnlyMode'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsSourceOpen = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSourceOpen'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsSourceSelected = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSourceSelected'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsSourceValid = Fiddle::Function::new(dtwain_dll['DTWAIN_IsSourceValid'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsTIFFSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsTIFFSupported'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsThumbnailEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsThumbnailEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsThumbnailSupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsThumbnailSupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsTwainAvailable = Fiddle::Function::new(dtwain_dll['DTWAIN_IsTwainAvailable'],[],Fiddle::TYPE_INT)
       @DTWAIN_IsTwainAvailableEx = Fiddle::Function::new(dtwain_dll['DTWAIN_IsTwainAvailableEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_IsTwainAvailableExA = Fiddle::Function::new(dtwain_dll['DTWAIN_IsTwainAvailableExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_IsTwainAvailableExW = Fiddle::Function::new(dtwain_dll['DTWAIN_IsTwainAvailableExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_IsUIControllable = Fiddle::Function::new(dtwain_dll['DTWAIN_IsUIControllable'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsUIEnabled = Fiddle::Function::new(dtwain_dll['DTWAIN_IsUIEnabled'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_IsUIOnlySupported = Fiddle::Function::new(dtwain_dll['DTWAIN_IsUIOnlySupported'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_LoadCustomStringResources = Fiddle::Function::new(dtwain_dll['DTWAIN_LoadCustomStringResources'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_LoadCustomStringResourcesA = Fiddle::Function::new(dtwain_dll['DTWAIN_LoadCustomStringResourcesA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_LoadCustomStringResourcesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_LoadCustomStringResourcesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_LoadCustomStringResourcesExA = Fiddle::Function::new(dtwain_dll['DTWAIN_LoadCustomStringResourcesExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_LoadCustomStringResourcesExW = Fiddle::Function::new(dtwain_dll['DTWAIN_LoadCustomStringResourcesExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_LoadCustomStringResourcesW = Fiddle::Function::new(dtwain_dll['DTWAIN_LoadCustomStringResourcesW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_LoadLanguageResource = Fiddle::Function::new(dtwain_dll['DTWAIN_LoadLanguageResource'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_LockMemory = Fiddle::Function::new(dtwain_dll['DTWAIN_LockMemory'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_LockMemoryEx = Fiddle::Function::new(dtwain_dll['DTWAIN_LockMemoryEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_LogMessage = Fiddle::Function::new(dtwain_dll['DTWAIN_LogMessage'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_LogMessageA = Fiddle::Function::new(dtwain_dll['DTWAIN_LogMessageA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_LogMessageW = Fiddle::Function::new(dtwain_dll['DTWAIN_LogMessageW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_MakeRGB = Fiddle::Function::new(dtwain_dll['DTWAIN_MakeRGB'],[Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_OpenSource = Fiddle::Function::new(dtwain_dll['DTWAIN_OpenSource'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_OpenSourcesOnSelect = Fiddle::Function::new(dtwain_dll['DTWAIN_OpenSourcesOnSelect'],[Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_RangeCreate = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeCreate'],[Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_RangeCreateFromCap = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeCreateFromCap'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_RangeDestroy = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeDestroy'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeExpand = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeExpand'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeExpandEx = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeExpandEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_RangeGetAll = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetAll'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetAllFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetAllFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetAllFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetAllFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetAllFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetAllFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetAllFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetAllFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetAllLong = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetAllLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetCount = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetCount'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_LONG)
       @DTWAIN_RangeGetExpValue = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetExpValue'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetExpValueFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetExpValueFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetExpValueFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetExpValueFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetExpValueFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetExpValueFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetExpValueFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetExpValueFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetExpValueLong = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetExpValueLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetNearestValue = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetNearestValue'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetPos = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetPos'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetPosFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetPosFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetPosFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetPosFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetPosFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetPosFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetPosFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetPosFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetPosLong = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetPosLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetValue = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetValue'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetValueFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetValueFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetValueFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetValueFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetValueFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetValueFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetValueFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetValueFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeGetValueLong = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeGetValueLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeIsValid = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeIsValid'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeNearestValueFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeNearestValueFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_RangeNearestValueFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeNearestValueFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_RangeNearestValueFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeNearestValueFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_RangeNearestValueFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeNearestValueFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_RangeNearestValueLong = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeNearestValueLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetAll = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetAll'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetAllFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetAllFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetAllFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetAllFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetAllFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetAllFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetAllFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetAllFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetAllLong = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetAllLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetValue = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetValue'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetValueFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetValueFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetValueFloatString = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetValueFloatString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetValueFloatStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetValueFloatStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetValueFloatStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetValueFloatStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RangeSetValueLong = Fiddle::Function::new(dtwain_dll['DTWAIN_RangeSetValueLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_ResetPDFTextElement = Fiddle::Function::new(dtwain_dll['DTWAIN_ResetPDFTextElement'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_RewindPage = Fiddle::Function::new(dtwain_dll['DTWAIN_RewindPage'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SelectDefaultOCREngine = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectDefaultOCREngine'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectDefaultSource = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectDefaultSource'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectDefaultSourceWithOpen = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectDefaultSourceWithOpen'],[Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngine = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngine'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngine2 = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngine2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngine2A = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngine2A'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngine2Ex = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngine2Ex'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngine2ExA = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngine2ExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngine2ExW = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngine2ExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngine2W = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngine2W'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngineByName = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngineByName'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngineByNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngineByNameA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectOCREngineByNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectOCREngineByNameW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSource = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSource'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSource2 = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSource2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSource2A = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSource2A'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSource2Ex = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSource2Ex'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSource2ExA = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSource2ExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSource2ExW = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSource2ExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSource2W = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSource2W'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSourceByName = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSourceByName'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSourceByNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSourceByNameA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSourceByNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSourceByNameW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSourceByNameWithOpen = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSourceByNameWithOpen'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSourceByNameWithOpenA = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSourceByNameWithOpenA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSourceByNameWithOpenW = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSourceByNameWithOpenW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_SelectSourceWithOpen = Fiddle::Function::new(dtwain_dll['DTWAIN_SelectSourceWithOpen'],[Fiddle::TYPE_INT],Fiddle::TYPE_VOIDP)
       @DTWAIN_SetAcquireArea = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireArea'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireArea2 = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireArea2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireArea2String = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireArea2String'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireArea2StringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireArea2StringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireArea2StringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireArea2StringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireImageNegative = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireImageNegative'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireImageScale = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireImageScale'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireImageScaleString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireImageScaleString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireImageScaleStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireImageScaleStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireImageScaleStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireImageScaleStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireStripBuffer = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireStripBuffer'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAcquireStripSize = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAcquireStripSize'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_UINT],Fiddle::TYPE_INT)
       @DTWAIN_SetAlarmVolume = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAlarmVolume'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetAlarms = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAlarms'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAllCapsToDefault = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAllCapsToDefault'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAppInfo = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAppInfo'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAppInfoA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAppInfoA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAppInfoW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAppInfoW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAuthor = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAuthor'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAuthorA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAuthorA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAuthorW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAuthorW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetAvailablePrinters = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAvailablePrinters'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetAvailablePrintersArray = Fiddle::Function::new(dtwain_dll['DTWAIN_SetAvailablePrintersArray'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetBitDepth = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBitDepth'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetection = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetection'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetectionEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetectionEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetectionExString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetectionExString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetectionExStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetectionExStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetectionExStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetectionExStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetectionString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetectionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetectionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetectionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBlankPageDetectionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBlankPageDetectionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetBrightness = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBrightness'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetBrightnessString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBrightnessString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetBrightnessStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBrightnessStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetBrightnessStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBrightnessStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetBufferedTileMode = Fiddle::Function::new(dtwain_dll['DTWAIN_SetBufferedTileMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetCallback = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCallback'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SetCallback64 = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCallback64'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_SetCamera = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCamera'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCameraA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCameraA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCameraW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCameraW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCapValues = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCapValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCapValuesEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCapValuesEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCapValuesEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCapValuesEx2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCaption = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCaption'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCaptionA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCaptionA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCaptionW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCaptionW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCompressionType = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCompressionType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetContrast = Fiddle::Function::new(dtwain_dll['DTWAIN_SetContrast'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetContrastString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetContrastString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetContrastStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetContrastStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetContrastStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetContrastStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetCountry = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCountry'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetCurrentRetryCount = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCurrentRetryCount'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetCustomDSData = Fiddle::Function::new(dtwain_dll['DTWAIN_SetCustomDSData'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_UINT, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetDSMSearchOrder = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDSMSearchOrder'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetDSMSearchOrderEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDSMSearchOrderEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDSMSearchOrderExA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDSMSearchOrderExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDSMSearchOrderExW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDSMSearchOrderExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDefaultSource = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDefaultSource'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDeviceNotifications = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDeviceNotifications'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetDeviceTimeDate = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDeviceTimeDate'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDeviceTimeDateA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDeviceTimeDateA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDeviceTimeDateW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDeviceTimeDateW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDoubleFeedDetectLength = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDoubleFeedDetectLength'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetDoubleFeedDetectLengthString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDoubleFeedDetectLengthString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDoubleFeedDetectLengthStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDoubleFeedDetectLengthStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDoubleFeedDetectLengthStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDoubleFeedDetectLengthStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDoubleFeedDetectValues = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDoubleFeedDetectValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetDoublePageCountOnDuplex = Fiddle::Function::new(dtwain_dll['DTWAIN_SetDoublePageCountOnDuplex'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetEOJDetectValue = Fiddle::Function::new(dtwain_dll['DTWAIN_SetEOJDetectValue'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetErrorBufferThreshold = Fiddle::Function::new(dtwain_dll['DTWAIN_SetErrorBufferThreshold'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetErrorCallback = Fiddle::Function::new(dtwain_dll['DTWAIN_SetErrorCallback'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetErrorCallback64 = Fiddle::Function::new(dtwain_dll['DTWAIN_SetErrorCallback64'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetFeederAlignment = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFeederAlignment'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetFeederOrder = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFeederOrder'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetFeederWaitTime = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFeederWaitTime'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetFileAutoIncrement = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFileAutoIncrement'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetFileCompressionType = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFileCompressionType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetFileSavePos = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFileSavePos'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetFileSavePosA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFileSavePosA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetFileSavePosW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFileSavePosW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetFileXferFormat = Fiddle::Function::new(dtwain_dll['DTWAIN_SetFileXferFormat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetHalftone = Fiddle::Function::new(dtwain_dll['DTWAIN_SetHalftone'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetHalftoneA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetHalftoneA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetHalftoneW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetHalftoneW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetHighlight = Fiddle::Function::new(dtwain_dll['DTWAIN_SetHighlight'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetHighlightString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetHighlightString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetHighlightStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetHighlightStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetHighlightStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetHighlightStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetJobControl = Fiddle::Function::new(dtwain_dll['DTWAIN_SetJobControl'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetJpegValues = Fiddle::Function::new(dtwain_dll['DTWAIN_SetJpegValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetJpegXRValues = Fiddle::Function::new(dtwain_dll['DTWAIN_SetJpegXRValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetLanguage = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLanguage'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetLastError = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLastError'],[Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_SetLightPath = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLightPath'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetLightPathEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLightPathEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetLightSource = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLightSource'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetLightSources = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLightSources'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetLoggerCallback = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLoggerCallback'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetLoggerCallbackA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLoggerCallbackA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetLoggerCallbackW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetLoggerCallbackW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetManualDuplexMode = Fiddle::Function::new(dtwain_dll['DTWAIN_SetManualDuplexMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetMaxAcquisitions = Fiddle::Function::new(dtwain_dll['DTWAIN_SetMaxAcquisitions'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetMaxBuffers = Fiddle::Function::new(dtwain_dll['DTWAIN_SetMaxBuffers'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetMaxRetryAttempts = Fiddle::Function::new(dtwain_dll['DTWAIN_SetMaxRetryAttempts'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetMultipageScanMode = Fiddle::Function::new(dtwain_dll['DTWAIN_SetMultipageScanMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetNoiseFilter = Fiddle::Function::new(dtwain_dll['DTWAIN_SetNoiseFilter'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetOCRCapValues = Fiddle::Function::new(dtwain_dll['DTWAIN_SetOCRCapValues'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetOrientation = Fiddle::Function::new(dtwain_dll['DTWAIN_SetOrientation'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetOverscan = Fiddle::Function::new(dtwain_dll['DTWAIN_SetOverscan'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFAESEncryption = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFAESEncryption'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFASCIICompression = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFASCIICompression'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFAuthor = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFAuthor'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFAuthorA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFAuthorA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFAuthorW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFAuthorW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFCompression = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFCompression'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFCreator = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFCreator'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFCreatorA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFCreatorA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFCreatorW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFCreatorW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFEncryption = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFEncryption'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_UINT, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFEncryptionA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFEncryptionA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_UINT, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFEncryptionW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFEncryptionW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_UINT, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFJpegQuality = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFJpegQuality'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFKeywords = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFKeywords'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFKeywordsA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFKeywordsA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFKeywordsW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFKeywordsW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFOCRConversion = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFOCRConversion'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_LONG)
       @DTWAIN_SetPDFOCRMode = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFOCRMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFOrientation = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFOrientation'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageScale = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageScale'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageScaleString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageScaleString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageScaleStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageScaleStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageScaleStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageScaleStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageSize = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageSize'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageSizeString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageSizeString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageSizeStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageSizeStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPageSizeStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPageSizeStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFPolarity = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFPolarity'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFProducer = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFProducer'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFProducerA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFProducerA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFProducerW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFProducerW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFSubject = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFSubject'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFSubjectA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFSubjectA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFSubjectW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFSubjectW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTextElementFloat = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTextElementFloat'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTextElementLong = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTextElementLong'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTextElementString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTextElementString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTextElementStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTextElementStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTextElementStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTextElementStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTitle = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTitle'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTitleA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTitleA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPDFTitleW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPDFTitleW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPaperSize = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPaperSize'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPatchMaxPriorities = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPatchMaxPriorities'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPatchMaxRetries = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPatchMaxRetries'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPatchPriorities = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPatchPriorities'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPatchSearchMode = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPatchSearchMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPatchTimeOut = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPatchTimeOut'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPixelFlavor = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPixelFlavor'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPixelType = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPixelType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPostScriptTitle = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPostScriptTitle'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPostScriptTitleA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPostScriptTitleA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPostScriptTitleW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPostScriptTitleW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPostScriptType = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPostScriptType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinter = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinter'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinterEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinterEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinterStartNumber = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinterStartNumber'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinterStringMode = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinterStringMode'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinterStrings = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinterStrings'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinterSuffixString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinterSuffixString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinterSuffixStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinterSuffixStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetPrinterSuffixStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetPrinterSuffixStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetQueryCapSupport = Fiddle::Function::new(dtwain_dll['DTWAIN_SetQueryCapSupport'],[Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetResolution = Fiddle::Function::new(dtwain_dll['DTWAIN_SetResolution'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetResolutionString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetResolutionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetResolutionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetResolutionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetResolutionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetResolutionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetResourcePath = Fiddle::Function::new(dtwain_dll['DTWAIN_SetResourcePath'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetResourcePathA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetResourcePathA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetResourcePathW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetResourcePathW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetRotation = Fiddle::Function::new(dtwain_dll['DTWAIN_SetRotation'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetRotationString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetRotationString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetRotationStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetRotationStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetRotationStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetRotationStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetSaveFileName = Fiddle::Function::new(dtwain_dll['DTWAIN_SetSaveFileName'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetSaveFileNameA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetSaveFileNameA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetSaveFileNameW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetSaveFileNameW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetShadow = Fiddle::Function::new(dtwain_dll['DTWAIN_SetShadow'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetShadowString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetShadowString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetShadowStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetShadowStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetShadowStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetShadowStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetSourceUnit = Fiddle::Function::new(dtwain_dll['DTWAIN_SetSourceUnit'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTIFFCompressType = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTIFFCompressType'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTIFFInvert = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTIFFInvert'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTempFileDirectory = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTempFileDirectory'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetTempFileDirectoryA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTempFileDirectoryA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetTempFileDirectoryEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTempFileDirectoryEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTempFileDirectoryExA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTempFileDirectoryExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTempFileDirectoryExW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTempFileDirectoryExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTempFileDirectoryW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTempFileDirectoryW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetThreshold = Fiddle::Function::new(dtwain_dll['DTWAIN_SetThreshold'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetThresholdString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetThresholdString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetThresholdStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetThresholdStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetThresholdStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetThresholdStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_SetTwainDSM = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTwainDSM'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTwainLog = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTwainLog'],[Fiddle::TYPE_UINT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetTwainLogA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTwainLogA'],[Fiddle::TYPE_UINT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetTwainLogW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTwainLogW'],[Fiddle::TYPE_UINT, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetTwainMode = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTwainMode'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetTwainTimeout = Fiddle::Function::new(dtwain_dll['DTWAIN_SetTwainTimeout'],[Fiddle::TYPE_LONG],Fiddle::TYPE_INT)
       @DTWAIN_SetUpdateDibProc = Fiddle::Function::new(dtwain_dll['DTWAIN_SetUpdateDibProc'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SetXResolution = Fiddle::Function::new(dtwain_dll['DTWAIN_SetXResolution'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetXResolutionString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetXResolutionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetXResolutionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetXResolutionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetXResolutionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetXResolutionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetYResolution = Fiddle::Function::new(dtwain_dll['DTWAIN_SetYResolution'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_DOUBLE],Fiddle::TYPE_INT)
       @DTWAIN_SetYResolutionString = Fiddle::Function::new(dtwain_dll['DTWAIN_SetYResolutionString'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetYResolutionStringA = Fiddle::Function::new(dtwain_dll['DTWAIN_SetYResolutionStringA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SetYResolutionStringW = Fiddle::Function::new(dtwain_dll['DTWAIN_SetYResolutionStringW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ShowUIOnly = Fiddle::Function::new(dtwain_dll['DTWAIN_ShowUIOnly'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_ShutdownOCREngine = Fiddle::Function::new(dtwain_dll['DTWAIN_ShutdownOCREngine'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SkipImageInfoError = Fiddle::Function::new(dtwain_dll['DTWAIN_SkipImageInfoError'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_INT],Fiddle::TYPE_INT)
       @DTWAIN_StartThread = Fiddle::Function::new(dtwain_dll['DTWAIN_StartThread'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_StartTwainSession = Fiddle::Function::new(dtwain_dll['DTWAIN_StartTwainSession'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_StartTwainSessionA = Fiddle::Function::new(dtwain_dll['DTWAIN_StartTwainSessionA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_StartTwainSessionW = Fiddle::Function::new(dtwain_dll['DTWAIN_StartTwainSessionW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_SysDestroy = Fiddle::Function::new(dtwain_dll['DTWAIN_SysDestroy'],[],Fiddle::TYPE_INT)
       @DTWAIN_SysInitialize = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitialize'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeEx2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeEx2A = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeEx2A'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeEx2W = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeEx2W'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeExA = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeExA'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeExW = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeExW'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeLib = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeLib'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeLibEx = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeLibEx'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeLibEx2 = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeLibEx2'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeLibEx2A = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeLibEx2A'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeLibEx2W = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeLibEx2W'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeLibExA = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeLibExA'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeLibExW = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeLibExW'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_VOIDP],Fiddle::TYPE_VOIDP)
       @DTWAIN_SysInitializeNoBlocking = Fiddle::Function::new(dtwain_dll['DTWAIN_SysInitializeNoBlocking'],[],Fiddle::TYPE_VOIDP)
       @DTWAIN_TestGetCap = Fiddle::Function::new(dtwain_dll['DTWAIN_TestGetCap'],[Fiddle::TYPE_VOIDP, Fiddle::TYPE_LONG],Fiddle::TYPE_VOIDP)
       @DTWAIN_UnlockMemory = Fiddle::Function::new(dtwain_dll['DTWAIN_UnlockMemory'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_UnlockMemoryEx = Fiddle::Function::new(dtwain_dll['DTWAIN_UnlockMemoryEx'],[Fiddle::TYPE_VOIDP],Fiddle::TYPE_INT)
       @DTWAIN_UseMultipleThreads = Fiddle::Function::new(dtwain_dll['DTWAIN_UseMultipleThreads'],[Fiddle::TYPE_INT],Fiddle::TYPE_INT)
    end
end

