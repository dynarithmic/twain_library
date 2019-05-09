/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2018 Dynarithmic Software.

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

    For more information, the license file LICENSE.TXT that is located in the root
    directory of the DTWAIN installation covers the restrictions under the LGPL license.
    Please read this file before deploying or distributing any application using DTWAIN.
 */
#ifndef _TWILRES_H_
#define _TWILRES_H_

#define IDC_TWAINDATA           8888
#define IDC_TWAINDEBUGDATA      8889
#define IDC_TWAINDGDATA         8890
#define IDC_TWAINDATDATA        8891
#define IDC_TWAINMSGDATA        8892
#define IDC_TWAINFILEDATA       8893
#define IDS_LIMITEDFUNCMSG1     8894
#define IDS_LIMITEDFUNCMSG2     8895
#define IDS_LIMITEDFUNCMSG3     8896
#define IDS_REGISTRATIONKEY     8900
#define IDS_ENCRYPTIONKEY       8901
#define IDS_TWCCBASE            9500
#define IDS_TWRCBASE            9600
#define IDS_TWCC_EXCEPTION      9999
//#define VS_VERSION_INFO         9000
#define IDS_DTWAINFUNCSTART     9001

#define IDS_TWCC_SUCCESS           9500 /* OkIt worked!                                */
#define IDS_TWCC_BUMMER            9501 /* Failure due to unknown causes             */
#define IDS_TWCC_LOWMEMORY         9502 /* Not enough memory to perform operation    */
#define IDS_TWCC_NODS              9503 /* No Data Source                            */
#define IDS_TWCC_MAXCONNECTIONS    9504 /* DS is connected to max possible applications      */
#define IDS_TWCC_OPERATIONERROR    9505 /* DS or DSM reported error, application shouldn't   */
#define IDS_TWCC_BADCAP            9506 /* Unknown capability                        */
#define IDS_TWCC_BADPROTOCOL       9509 /* Unrecognized MSG DG DAT combination       */
#define IDS_TWCC_BADVALUE          9510 /* Data parameter out of range              */
#define IDS_TWCC_SEQERROR          9511 /* DG DAT MSG out of expected sequence      */
#define IDS_TWCC_BADDEST           9512 /* Unknown destination Application/Source in DSM_Entry */
#define IDS_TWCC_CAPUNSUPPORTED    9513 /* Capability not supported by source            */
#define IDS_TWCC_CAPBADOPERATION   9514 /* Operation not supported by capability         */
#define IDS_TWCC_CAPSEQERROR       9515 /* Capability has dependancy on other capability */
#define IDS_TWCC_DENIED            9516 /* File System operation is denied (file is protected) */
#define IDS_TWCC_FILEEXISTS        9517 /* Operation failed because file already exists. */
#define IDS_TWCC_FILENOTFOUND      9518 /* File not found */
#define IDS_TWCC_NOTEMPTY          9519 /* Operation failed because directory is not empty */
#define IDS_TWCC_PAPERJAM          9520  /* The feeder is jammed */
#define IDS_TWCC_PAPERDOUBLEFEED   9521  /* The feeder detected multiple pages */
#define IDS_TWCC_FILEWRITEERROR    9522  /* Error writing the file (meant for things like disk full conditions) */
#define IDS_TWCC_CHECKDEVICEONLINE 9523  /* The device went offline prior to or during this operation */

#define IDS_TWRC_SUCCESS          9600
#define IDS_TWRC_FAILURE          9601
#define IDS_TWRC_CHECKSTATUS      9602
#define IDS_TWRC_CANCEL           9603
#define IDS_TWRC_DSEVENT          9604
#define IDS_TWRC_NOTDSEVENT       9605
#define IDS_TWRC_XFERDONE         9606
#define IDS_TWRC_ENDOFLIST        9607
#define IDS_TWRC_INFONOTSUPPORTED 9608
#define IDS_TWRC_DATANOTAVAILABLE 9609

#define IDS_DTWAIN_APPTITLE       9700

#define DTW_CONTARRAY           8
#define DTW_CONTENUMERATION     16
#define DTW_CONTONEVALUE        32
#define DTW_CONTRANGE           64

#define DTW_FF_TIFF        0
#define DTW_FF_PICT        1
#define DTW_FF_BMP         2
#define DTW_FF_XBM         3
#define DTW_FF_JFIF        4
#define DTW_FF_FPX         5
#define DTW_FF_TIFFMULTI   6
#define DTW_FF_PNG         7
#define DTW_FF_SPIFF       8
#define DTW_FF_EXIF        9
#define DTW_BMP          DTW_FF_BMP
#define DTW_JPEG         DTW_FF_JFIF
#define DTW_PCX          10
#define DTW_TGA          11
#define DTW_TIFFLZW      12
#define DTW_TIFFNONE     DTW_FF_TIFF
#define DTW_TIFFG3       13
#define DTW_TIFFG4       14
#define DTW_GIF          15
#define DTW_PNG          DTW_FF_PNG

#define LTWRC_SUCCESS          0L
#define LTWRC_FAILURE          1L
#define LTWRC_CHECKSTATUS      2L
#define LTWRC_CANCEL           3L
#define LTWRC_DSEVENT          4L
#define LTWRC_NOTDSEVENT       5L
#define LTWRC_XFERDONE         6L
#define LTWRC_ENDOFLIST        7L
#define LTWRC_INFONOTSUPPORTED 8L
#define LTWRC_DATANOTAVAILABLE 9L

#define LTWCC_SUCCESS            0L /* It worked!                                */
#define LTWCC_BUMMER             1L /* Failure due to unknown causes             */
#define LTWCC_LOWMEMORY          2L /* Not enough memory to perform operation    */
#define LTWCC_NODS               3L /* No Data Source                            */
#define LTWCC_MAXCONNECTIONS     4L /* DS is connected to max possible applications      */
#define LTWCC_OPERATIONERROR     5L /* DS or DSM reported error, application shouldn't   */
#define LTWCC_BADCAP             6L /* Unknown capability                        */
#define LTWCC_BADPROTOCOL        9L /* Unrecognized MSG DG DAT combination       */
#define LTWCC_BADVALUE           10L /* Data parameter out of range              */
#define LTWCC_SEQERROR           11L /* DG DAT MSG out of expected sequence      */
#define LTWCC_BADDEST            12L /* Unknown destination Application/Source in DSM_Entry */
#define LTWCC_CAPUNSUPPORTED     13L /* Capability not supported by source            */
#define LTWCC_CAPBADOPERATION    14L /* Operation not supported by capability         */
#define LTWCC_CAPSEQERROR        15L /* Capability has dependancy on other capability */
#define LTWCC_DENIED             16L /* File System operation is denied (file is protected) */
#define LTWCC_FILEEXISTS         17L /* Operation failed because file already exists. */
#define LTWCC_FILENOTFOUND       18L /* File not found */
#define LTWCC_NOTEMPTY           19L /* Operation failed because directory is not empty */
#define LTWCC_PAPERJAM           20L  /* The feeder is jammed */
#define LTWCC_PAPERDOUBLEFEED    21L  /* The feeder detected multiple pages */
#define LTWCC_FILEWRITEERROR     22L  /* Error writing the file (meant for things like disk full conditions) */
#define LTWCC_CHECKDEVICEONLINE  23L  /* The device went offline prior to or during this operation */

#define IDC_DLGSELECTSOURCE      10000
//    DEFPUSHBUTTON   "&Select",IDOK,157,46,50,14
//    PUSHBUTTON      "&Cancel",IDC     ANCEL,157,68,50,14
#define IDC_LSTSOURCES           10001
#define IDC_DLGMISC              10002
#define IDC_EDIT1                10003

#define IDC_SOURCETEXT           10002
#define IDC_STATIC               -1
//#def    LTEXT           "Sources:",IDC_STATIC,11,11,29,8

#define IDS_SELECT_SOURCE_TEXT              3000
#define IDS_SELECT_TEXT                     3001
#define IDS_CANCEL_TEXT                     3002
#define IDS_SOURCES_TEXT                    3003

#define IDS_LOGMSG_START                    3005
#define IDS_LOGMSG_ENTERTEXT                (IDS_LOGMSG_START + 0)
#define IDS_LOGMSG_EXITTEXT                 (IDS_LOGMSG_START + 1)
#define IDS_LOGMSG_RETURNTEXT               (IDS_LOGMSG_START + 2)
#define IDS_LOGMSG_EXCEPTERRORTEXT          (IDS_LOGMSG_START + 3)
#define IDS_LOGMSG_MODULETEXT               (IDS_LOGMSG_START + 4)
#define IDS_LOGMSG_NOPARAMINFOTEXT          (IDS_LOGMSG_START + 5)
#define IDS_LOGMSG_NOINFOERRORTEXT          (IDS_LOGMSG_START + 6)
#define IDS_LOGMSG_INPUTTEXT                (IDS_LOGMSG_START + 7)
#define IDS_LOGMSG_OUTPUTDSMTEXT            (IDS_LOGMSG_START + 8)
#define IDS_LOGMSG_END                      (IDS_LOGMSG_START + 8)

#define IDC_DTWAINSPLASH                    4000
#endif
